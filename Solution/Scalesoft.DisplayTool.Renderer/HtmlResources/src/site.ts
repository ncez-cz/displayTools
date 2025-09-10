import "./main.scss";
import "./collapseSectionButtonFuncs"
import {updateCollapsibleUI} from "./collapseSectionButtonFuncs";
import {expandParentCollapsers} from "./referenceLinkFunctions";
import {initModals} from "./modalFunctions";
import {
    flip,
    computePosition,
    offset,
    shift,
    arrow
} from "@floating-ui/dom";

updateCollapsibleUI()
expandParentCollapsers()

const initTooltips = () => {
    const tooltipWrappers = document.querySelectorAll<HTMLElement>(".tooltip-wrapper");
    tooltipWrappers.forEach((wrapperElement: HTMLElement): void => {
        const tooltipElement = wrapperElement?.querySelector<HTMLElement>(".tooltip");
        const arrowElement = wrapperElement?.querySelector<HTMLElement>(".arrow");
        if (!tooltipElement || !arrowElement) {
            return;
        }

        //removing classes that is using default tooltip without javascript
        tooltipElement.classList.remove("tooltip-top", "tooltip-bottom");
        const updateTooltip = (): void => {
            computePosition(wrapperElement, tooltipElement, {
                placement: "top",
                middleware: [
                    offset(5),
                    flip(),
                    shift({padding: 5}),
                    arrow({element: arrowElement}),
                ],
            })
                .then(({x, y, placement, middlewareData}): void => {
                    Object.assign(tooltipElement.style, {
                        left: `${x}px`,
                        top: `${y}px`,
                    });
                    const {x: arrowX, y: arrowY} = middlewareData.arrow ?? {};
                    const staticSide = {
                        top: "bottom",
                        right: "left",
                        bottom: "top",
                        left: "right",
                    }[placement.split("-")[0]] ?? "";
                    Object.assign(arrowElement.style, {
                        left: arrowX != null ? `${arrowX}px` : "",
                        top: arrowY != null ? `${arrowY}px` : "",
                        [staticSide]: "-4px",
                    });
                });
        }

        const showTooltip = (tooltipElement: HTMLElement): void => {
            tooltipElement.style.display = "block";
            updateTooltip();
            document.body.appendChild(tooltipElement);
        }

        const hideTooltip = (tooltipElement: HTMLElement): void => {
            tooltipElement.style.display = "none";
            wrapperElement.appendChild(tooltipElement);
        }

        wrapperElement.addEventListener("mouseenter", () => showTooltip(tooltipElement));
        wrapperElement.addEventListener("mouseleave", () => hideTooltip(tooltipElement));
        wrapperElement.addEventListener("focus", () => showTooltip(tooltipElement));
        wrapperElement.addEventListener("blur", () => hideTooltip(tooltipElement));
        updateTooltip();
    })
}

const collapseAllButton = document.querySelector<HTMLButtonElement>(".collapse-all-btn");
if (collapseAllButton) {
    collapseAllButton.hidden = false;
}

document.querySelectorAll('.modal').forEach((modal: Element) => {
    modal.addEventListener("click", (e) => {
        const target = e.target as HTMLElement;
        if (!target.closest("button")) {
            e.preventDefault()
        }
    })
});

document.addEventListener("DOMContentLoaded", () => {
    initTooltips();
    initModals();
}, {once: true});