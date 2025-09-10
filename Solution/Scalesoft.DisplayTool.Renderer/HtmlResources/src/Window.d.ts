interface Window {
    updateName: () => void;
    expandOrCollapseAllSections: () => void;
    expandParentCollapsers: () => void;
    localization: Localization;
    originalFetch: (input: RequestInfo | URL, init?: RequestInit) => Promise<Response>;
}
