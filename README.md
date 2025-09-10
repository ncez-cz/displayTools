# Introduction

This repository contains display tools for FHIR, CDA, and DASTA.

# Getting Started

## Prerequisites

### Build

- .NET 8.0 SDK
- node.js
    - Tested with:
        - node.js v22.14.0 and npm 8.19.4
        - node.js v22.19.0 and npm 10.9.3

### Run

- .NET 8.0 hosting bundle for IIS deployment, .NET 8.0 runtime otherwise

## First-time setup

- Inside the `Solution` directory, run
    - `npm install`
    - `dotnet restore`

## Run the Swagger UI

- Inside the `Solution/Scalesoft.DisplayTool.Service` directory, run
    - `dotnet run`
- The Swagger UI will be available at http://localhost:5000/swagger
- To build a deployable package, optionally edit `Solution/Scalesoft.DisplayTool.Service/appsettings.Production.json`,
  then, in the same directory, run
    - `dotnet publish -c Release /p:EnvironmentName=Production -o ./publish`
    - Copy the contents of the `publish` directory to an IIS site's physical path, or start
      `publish/Scalesoft.DisplayTool.Service.exe` manually.

# Usage as a library

## Default usage

- Reference the `Scalesoft.DisplayTool.Renderer` project in your own project
- Instantiate the `DocumentRenderer` class using the parameterless constructor
- Create an instance of `DocumentOptions`
    - ValidateDocument: false
    - ValidateCodeValues: false
    - ValidateDigitalSignature: false
    - LanguageOption: LanguageOptions.Czech
    - PreferTranslationsFromDocument: true (Tries to avoid fetching translations from terminology server when possible)
- Call the `RenderAsync` method with the input document as a byte array, select an appropriate input format, choose an
  appropriate output format, pass the `DocumentOptions` instance, choose the correct documentType (Laboratory,
  PatientSummary, etc.)

## Custom usage

### Configuration changes

- Some options (logging, choosing between predefined validators, PDF renderer config) can be changed by passing a
  `DocumentRenderOptions` instance to the `DocumentRenderer` constructor.

### Custom implementations of interfaces

- Make a copy of the `ServiceRegistration.CreateServiceProvider` method.
- Change the registration of the desired interfaces to use your own implementations.
- Call the custom `CreateServiceProvider` method and pass the result to the `DocumentRenderer` constructor.

#### Custom translation source

- Implement `ICodeTranslator`
- For example, you might want to use translations from a local file or from a database.

# Architecture

## Scalesoft.DisplayTool.Service

Contains an example service which exposes an API for rendering documents, as well as a Swagger UI.

## Scalesoft.DisplayTool.Shared

Contains useful classes for parsing XML documents, as well as the ICodeTranslator interface.
This is kept separate from the renderer project to facilitate the ability to implement a custom translation source in a
separate project, while keeping the ability to register it in `Scalesoft.DisplayTools.Renderer.ServiceRegistration`,
should you wish to do so.

## Scalesoft.DisplayTool.Renderer

Contains the actual rendering logic.

### Widgets

Widgets are the building blocks of the rendering process.

A widget is a small piece of code that can take an input (part of an XML document, parameters, or other widgets) and
produces an output. Currently only HTML output is implemented, but other formats could be added (for example a
preprocessed JSON to be used in a mobile app).

The entire widget system is inspired by flutter, but here, the widgets are designed with the goal of parsing XML files.

#### Widget types

Widgets can be divided into two categories:

##### "Basic" widgets

A widget that doesn't depend on other widgets for generating (at least a part of) its output. For example, a widget that
generates a table from a list of rows. To be considered a "basic" widget, a widget should output some HTML on its own.

By their nature, there is a small number of "basic" widgets, roughly corresponding to HTML tags or UI concepts like
cards, tooltips, etc.

To see an exhaustive list of basic widgets, see the methods in IWidgetRenderer.cs.

##### "Composite" widgets

A widget that depends solely on other widgets for generating all of its output and cannot generate any HTML on its own.
For example, a widget that renders a FHIR resource, using Cards, Tables, and other widgets.
Such widgets usually parse a part of the input XML document (using the navigator parameter)

#### Widget directory structure

Widgets that are reusable between all input standards are placed in the "root" Widget directory. Widgets that are
specific to a single input standard are placed in a directory with the name of the input standard.

#### Important concepts

##### Widget parameters

A widget's render method is called with:

- a XmlDocumentNavigator
    - Contains the input XML document, with a context (the current element). This context is set by preceding widgets. A
      widget can use relative or absolute paths to navigate the document (including parent nodes).
    - Has methods for navigating the document, like SelectSingleNode, SelectAllNodes.
    - Can set an equivalent of xslt parameters/variables.
        - Be aware that since xslt variables/parameters influence not just the current context's children (created with
          `SelectSingleNode`, `SelectAllNodes`, but the rest of the current context too (following siblings), using
          these is not thread-safe (unusable with parallel rendering).
        - Prefer passing parameters to child widgets instead.
- an IWidgetRenderer
    - Contains logic that takes a simple ViewModel and renders a string output. (Currently implemented using razor
      components)
- a RenderContext
    - Contains information about the rendering process, such as the chosen language, document type, etc.
    - Also contains useful methods / instances of objects useful for getting translations, logging, or keeping track of
      rendered elements.

##### Context

A widget's context (contained in XmlDocumentNavigator) is a place (node) in the input XML document.
The context is usually changed using helper widgets like `ChangeContext`, `Optional`, `ConcatBuilder`, `ListBuilder`,
etc. using a given XPath expression.

A widget should usually render itself in the given context.

#### Important widgets

Other than the aforementioned `ChangeContext`, `Optional`, `ConcatBuilder`, `ListBuilder`, that change the context,
notable widgets include

- Decision widgets
    - Condition (equivalent of xslt conditions)
    - Optional (combines `Condition` and `ChangeContext`)
    - Choose, When (switch-case widgets, equivalent of xslt choose and when)
    - If (similar to Condition, but uses a c# function instead of an xpath expression as the condition)
- For an example of a complex widget that renders a FHIR resource, see `ObservationCard`
    - Take note of the use of `InfrequentProperties` to dynamically hide properties that aren't present. This mechanism
      handles open type properties and extensions as well. This mechanism is also useful in tables to hide entire
      columns when they would otherwise be completely empty.
- For an example of handling FHIR references, see `ShowMultiReference` or `FhirSection` and its usage in Composition
  widgets
    - Generally, FHIR references where the target can have more than one type utilize the `AnyResource` widget to
      determine which widget should render the referenced resource.
