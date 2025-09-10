using Scalesoft.DisplayTool.Renderer.Models;
using Scalesoft.DisplayTool.Renderer.Models.Enums;
using Scalesoft.DisplayTool.Renderer.Renderers;
using Scalesoft.DisplayTool.Renderer.Extensions;
using Scalesoft.DisplayTool.Renderer.Widgets.WidgetUtils;
using Scalesoft.DisplayTool.Shared.DocumentNavigation;

#nullable enable

namespace Scalesoft.DisplayTool.Renderer.Widgets.Cda;

public class FormatDateTimeWidget : Widget
{
    public override Task<RenderResult> Render(XmlDocumentNavigator navigator, IWidgetRenderer renderer,
        RenderContext context)
    {
    List<Widget> widgetTree = [
new Variable("year", "substring ($date, 1, 4)"), 
new Text("$year")
, 
new Condition("string-length($date) > 4", [
new ConstantText(@"-"), 
new Variable("month", "substring ($date, 5, 2)"), 
new Text("$month")
, 
new Condition("string-length($date) > 6", [
new ConstantText(@"-"), 
new Variable("day", "substring ($date, 7, 2)"), 
new Text("$day")
, 
new Condition("string-length($date) > 8", [
new ConstantText(@"	"), 
new Variable("time", [ 
new Text("substring($date,9,6)")
, 
]), 
new Variable("hh", [ 
new Text("substring($time,1,2)")
, 
]), 
new Variable("mm", [ 
new Text("substring($time,3,2)")
, 
]), 
new Variable("ss", [ 
new Text("substring($time,5,2)")
, 
]), 
new Condition("string-length($hh)>1", [
new Text("$hh")
, 
new Condition("string-length($mm)>1 and not(contains($mm,'-')) and not (contains($mm,'+'))", [
new ConstantText(@":"), 
new Text("$mm")
, 
new Condition("string-length($ss)>1 and not(contains($ss,'-')) and not (contains($ss,'+'))", [
new ConstantText(@":"), 
new Text("$ss")
, 
])
, 
])
, 
])
, 
new Variable("tzon", [ 
new Choose([
new When("contains($date,'+')", [
new ConstantText(@"("), 
new ConstantText(@"+"), 
new Text("substring(substring-after($date, '+'),1,2)")
, 
new ConstantText(@":"), 
new Text("substring(substring-after($date, '+'),3,2)")
, 
new ConstantText(@")"), 
]),
new When("contains($date,'-')", [
new ConstantText(@"("), 
new ConstantText(@"-"), 
new Text("substring(substring-after($date, '-'),1,2)")
, 
new ConstantText(@":"), 
new Text("substring(substring-after($date, '-'),3,2)")
, 
new ConstantText(@")"), 
]),
], [
]), 
]), 
new Choose([
new When("$tzon = '-0500' ", [
new ConstantText(@", EST"), 
]),
new When("$tzon = '-0600' ", [
new ConstantText(@", CST"), 
]),
new When("$tzon = '-0700' ", [
new ConstantText(@", MST"), 
]),
new When("$tzon = '-0800' ", [
new ConstantText(@", PST"), 
]),
], [
new ConstantText(@" "), 
new Text("$tzon")
, 
]), 
])
, 
])
, 
])
, 
];
        return widgetTree.RenderConcatenatedResult(navigator, renderer, context);
    }
}
