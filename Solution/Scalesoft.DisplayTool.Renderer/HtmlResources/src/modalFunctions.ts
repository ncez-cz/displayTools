let outsideClickHandler: ((event: MouseEvent) => void) | null = null;
let backdropEl: HTMLDivElement | null = null;

export function hidePrintCollapsers() {
    document
        .querySelectorAll<HTMLLabelElement>('.narrative-print-collapser')
        .forEach(collapser => {
            collapser.classList.add('narrative-print-collapser-hidden');
        });
}

export function initModals() {
    hidePrintCollapsers();

    document.querySelectorAll<HTMLElement>('.modal').forEach(modalEl => {
        const id = modalEl.id.replace("modal-", "");
        const button = document.getElementById(`button-${id}`) as HTMLButtonElement | null;

        if (!button) {
            console.warn(`Button with id button-${id} not found`);
            return;
        }

        button.addEventListener('click', () => {
            openModal(button, modalEl);
        });

        modalEl.querySelectorAll<HTMLButtonElement>("button[data-bs-dismiss='modal']").forEach(el => {
            el.addEventListener('click', () => {
                closeModal(modalEl);
            });
        });

        button.classList.add("show");
    });
}

function openModal(button: HTMLElement, modalEl: HTMLElement) {
    modalEl.classList.add('show');
    modalEl.style.display = 'block';

    const body = document.querySelector<HTMLBodyElement>('body');
    if (body) {
        body.classList.add('modal-open');

        const scrollbarWidth = window.innerWidth - document.documentElement.clientWidth;
        document.documentElement.style.paddingRight = `${scrollbarWidth}px`;
        document.documentElement.style.overflow = 'hidden';

        if (!backdropEl) {
            backdropEl = document.createElement('div');
            backdropEl.classList.add('modal-backdrop', 'fade', 'show');
        }
        body.appendChild(backdropEl);
    }

    const dialog = modalEl.querySelector<HTMLDivElement>(".modal-dialog");
    if (dialog) {
        outsideClickHandler = (event: MouseEvent) => {
            if (!dialog.contains(event.target as Node) && !button.contains(event.target as Node)) {
                closeModal(modalEl);
            }
        };
        document.addEventListener('click', outsideClickHandler);
    }
}

function closeModal(modalEl: HTMLElement) {
    modalEl.classList.remove('show');
    modalEl.style.display = 'none';

    const body = document.querySelector<HTMLBodyElement>('body');
    if (body) {
        body.classList.remove('modal-open');
        backdropEl?.remove();
    }

    if (outsideClickHandler) {
        document.removeEventListener('click', outsideClickHandler);
        outsideClickHandler = null;
    }

    document.documentElement.style.removeProperty('padding-right');
    document.documentElement.style.removeProperty('overflow');
}
