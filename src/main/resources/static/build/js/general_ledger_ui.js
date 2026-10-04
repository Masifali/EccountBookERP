(function (w) {
    'use strict';
    const widths = new Map();

    function normalizeAccounts(rows) {
        return rows.map(row => {
            // JDBC lookup maps are case-insensitive; their JSON keys are not.
            const fields = Object.fromEntries(Object.entries(row).map(([key, value]) => [key.toLowerCase(), value]));
            return {
                ...row,
                Id: fields.id,
                AccountTitle: fields.accounttitle ?? '',
                AccountCode: fields.accountcode ?? '',
                AccountType: fields.accounttype ?? '',
                AccountClass: fields.accountclass ?? fields.classname ?? '',
                ParentAccount: fields.parentaccount ?? fields.parentaccounttitle ?? ''
            };
        });
    }

    function fillAccounts(select, rows, byName) {
        const current = select.value;
        const selected = new Set(Array.from(select.selectedOptions, option => option.value));
        const textKey = byName ? 'AccountTitle' : 'AccountCode';
        const otherKey = byName ? 'AccountCode' : 'AccountTitle';
        const family = byName ? 'ledgerAccountTitle' : 'ledgerAccountCode';
        w.DesktopCombo.define(family, [
            { caption: byName ? 'Account Title' : 'AccountCode', flex: byName ? 4 : 1.3 },
            { caption: byName ? 'AccountCode' : 'Account Title', key: 'other', flex: byName ? 1.3 : 4 },
            { caption: 'AccountType', key: 'account-type', flex: 1.3 },
            { caption: 'AccountClass', key: 'account-class', flex: 1.3 },
            { caption: 'ParentAccount', key: 'parent-account', flex: 1.3 }
        ]);
        select.setAttribute('data-dtcombo', family);
        select.removeAttribute('data-dtcombo-caption');
        select.replaceChildren();
        if (!select.multiple) select.append(new Option('', ''));
        normalizeAccounts(rows).forEach(row => {
            const option = new Option(row[textKey] ?? '', row.Id);
            option.setAttribute('data-other', row[otherKey] ?? '');
            option.setAttribute('data-account-type', row.AccountType ?? '');
            option.setAttribute('data-account-class', row.AccountClass ?? row.ClassName ?? '');
            option.setAttribute('data-parent-account', row.ParentAccount ?? row.ParentAccountTitle ?? '');
            option.selected = selected.has(String(row.Id));
            select.append(option);
        });
        if (!select.multiple) select.value = current;
        const combo = w.AccF ? w.AccF.combo(select) : w.DesktopCombo.enhance(select);
        if (combo) {
            combo.wrap.classList.add('gl-account-combo');
            combo.wrap.classList.toggle('gl-account-by-code', !byName);
            combo.input.setAttribute('aria-label', select.multiple ? 'Accounts' : 'Account');
        }
    }

    function comments(table, columns) {
        if (!table || !table.tHead) return;
        table.classList.add('gl-ledger-grid');
        columns = columns || Array.from(table.tHead.rows[0].cells, cell => ({key:cell.textContent.trim()}));
        const visible = columns.filter(column => column.visible !== false);
        const index = visible.findIndex(column => column.key === 'Comments' || column.key === 'Remarks');
        if (index < 0 || !table.tHead) return;
        const key = visible[index].key;
        const heading = table.tHead.rows[0].cells[index];
        if (!heading) return;
        heading.classList.add('gl-comment-heading');
        heading.style.minWidth = 'var(--gl-comment-width, 250px)';
        const handle = document.createElement('span');
        handle.className = 'gl-column-resize';
        handle.tabIndex = 0;
        handle.setAttribute('role', 'separator');
        handle.setAttribute('aria-orientation', 'vertical');
        handle.setAttribute('aria-label', 'Resize ' + key + ' column');
        handle.setAttribute('aria-valuemin', '80');
        handle.setAttribute('aria-valuemax', '2400');
        handle.title = 'Drag to widen. Double-click to fit text. Arrow keys adjust width.';
        heading.append(handle);
        for (const body of table.tBodies) for (const row of body.rows) {
            if (row.cells.length !== visible.length) continue;
            const cell = row.cells[index];
            const text = cell.textContent;
            cell.classList.remove('wrap');
            cell.classList.add('gl-comment-cell');
            const span = document.createElement('span');
            span.className = 'gl-comment-text';
            span.textContent = text;
            span.title = text;
            cell.replaceChildren(span);
        }
        function size(value) {
            const width = Math.max(80, Math.min(2400, Math.round(value)));
            widths.set(key, width);
            table.style.setProperty('--gl-comment-width', width + 'px');
            handle.setAttribute('aria-valuenow', String(width));
        }
        size(widths.get(key) || 250);
        handle.addEventListener('pointerdown', event => {
            if (event.button !== 0) return;
            event.preventDefault();
            const startX = event.clientX, startWidth = widths.get(key);
            handle.setPointerCapture(event.pointerId);
            const move = e => size(startWidth + e.clientX - startX);
            const end = () => {
                handle.removeEventListener('pointermove', move);
                handle.removeEventListener('pointerup', end);
                handle.removeEventListener('pointercancel', end);
            };
            handle.addEventListener('pointermove', move);
            handle.addEventListener('pointerup', end);
            handle.addEventListener('pointercancel', end);
        });
        handle.addEventListener('keydown', event => {
            if (event.key !== 'ArrowLeft' && event.key !== 'ArrowRight') return;
            event.preventDefault();
            event.stopPropagation();
            size(widths.get(key) + (event.key === 'ArrowRight' ? 20 : -20));
        });
        handle.addEventListener('dblclick', () => {
            const widest = Array.from(table.querySelectorAll('.gl-comment-text'))
                .reduce((width, span) => Math.max(width, span.scrollWidth), 250);
            size(widest);
        });
    }

    w.GeneralLedgerUi = { normalizeAccounts, fillAccounts, comments };
})(window);
