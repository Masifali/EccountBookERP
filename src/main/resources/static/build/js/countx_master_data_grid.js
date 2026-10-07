/* Presentation of Master Data history grids. Row data-i and editor nodes stay
   intact so filtering/grouping never changes the record sent to the existing BLL. */
(function () {
    'use strict';
    function enhance(table) {
        var body = table.tBodies[0], heads = Array.from(table.tHead.rows[0].cells);
        var groupIndex = -1, collapsed = new Set(), filters = [];
        var filterRow = document.createElement('tr');
        // GridBar skips rk-filter when identifying the column-caption row.
        filterRow.className = 'md-filter-row rk-filter';
        var box = table.closest('.pd-grid-box'), bar = document.createElement('div');
        bar.className = 'md-grid-groupbar';
        bar.id = 'md-grid-groupbar-' + table.id;
        var prompt = document.createElement('span');
        prompt.className = 'md-group-prompt';
        prompt.textContent = 'Drag a column header here to group by that column.';
        var clear = document.createElement('button');
        clear.type = 'button'; clear.textContent = 'Remove grouping'; clear.hidden = true;
        bar.append(prompt, clear); box.insertBefore(bar, box.firstChild);

        heads.forEach(function (th, i) {
            var caption = th.textContent.trim();
            th.title = caption;
            th.style.width = (parseFloat(th.style.width || th.style.minWidth) || (/Code|Date/.test(th.dataset.col) ? 100 : 180)) + 'px';
            th.style.minWidth = '';
            var td = document.createElement('td'); filterRow.appendChild(td);
            if (th.style.display === 'none' || th.dataset.col === 'Edit') return;
            var input = document.createElement('input');
            input.type = 'text'; input.setAttribute('aria-label', 'Filter ' + caption);
            input.addEventListener('input', render); td.appendChild(input); filters[i] = input;
            th.draggable = true; th.tabIndex = 0;
            th.title = caption + ' — drag to group, or press Alt+G';
            th.addEventListener('dragstart', function (e) { e.dataTransfer.setData('text/plain', table.id + ':' + i); });
            th.addEventListener('keydown', function (e) {
                if (e.altKey && e.key.toLowerCase() === 'g') { e.preventDefault(); group(i); }
            });
        });
        table.tHead.appendChild(filterRow);
        function group(i) {
            groupIndex = i; collapsed.clear(); clear.hidden = i < 0;
            prompt.textContent = i < 0 ? 'Drag a column header here to group by that column.' : 'Grouped by: ' + heads[i].textContent.trim();
            render();
        }
        clear.addEventListener('click', function () { group(-1); });
        bar.addEventListener('dragover', function (e) { e.preventDefault(); });
        bar.addEventListener('drop', function (e) {
            e.preventDefault();
            var parts = e.dataTransfer.getData('text/plain').split(':');
            var i = Number(parts[1]);
            if (parts[0] === table.id && Number.isInteger(i) && filters[i]) group(i);
        });
        function text(row, index) {
            var cell = row.cells[index];
            if (!cell) return '';
            var editor = cell.querySelector('select, input');
            return editor ? (editor.tagName === 'SELECT' ? (editor.selectedOptions[0] || {}).textContent || '' : editor.value) : cell.textContent;
        }
        function fit() {
            var width = 0, auto = false;
            heads.forEach(function (th, i) {
                var hidden = getComputedStyle(th).display === 'none';
                var display = hidden ? 'none' : '';
                if (filterRow.cells[i].style.display !== display) filterRow.cells[i].style.display = display;
                if (!hidden) {
                    width += parseFloat(th.style.width) || 180;
                    if (!th.style.width) auto = true;
                }
            });
            var layout = auto ? 'auto' : 'fixed';
            if (table.style.tableLayout !== layout) table.style.tableLayout = layout;
            var value = auto ? 'max-content' : width + 'px';
            if (table.style.width !== value) table.style.width = value;
        }
        var observer = new MutationObserver(render);
        function render() {
            observer.disconnect();
            body.querySelectorAll('.md-group-row').forEach(function (r) { r.remove(); });
            var rows = Array.from(body.rows).sort(function (a, b) { return Number(a.dataset.i) - Number(b.dataset.i); });
            var groups = new Map();
            rows.forEach(function (row) {
                row.hidden = filters.some(function (input, i) { return input && text(row, i).toLowerCase().indexOf(input.value.toLowerCase()) < 0; });
                Array.from(row.cells).forEach(function (cell) { cell.title = cell.textContent; });
                row.removeAttribute('data-gb-member'); row.classList.remove('gb-collapsed');
                if (groupIndex < 0 || row.hidden) body.appendChild(row);
                else {
                    var key = text(row, groupIndex);
                    if (!groups.has(key)) groups.set(key, []);
                    groups.get(key).push(row);
                }
            });
            groups.forEach(function (members, key) {
                var tr = document.createElement('tr'), td = document.createElement('td'), toggle = document.createElement('button');
                tr.className = 'md-group-row gb-group'; tr.dataset.gbGroup = key;
                td.colSpan = heads.length;
                toggle.type = 'button'; toggle.className = 'md-group-toggle';
                toggle.setAttribute('aria-expanded', String(!collapsed.has(key)));
                toggle.textContent = (collapsed.has(key) ? '+ ' : '− ') + heads[groupIndex].textContent.trim() + ': ' + (key || '(empty)') + ' (' + members.length + ')';
                toggle.addEventListener('click', function () { if (collapsed.has(key)) collapsed.delete(key); else collapsed.add(key); render(); });
                td.appendChild(toggle); tr.appendChild(td); body.appendChild(tr);
                members.forEach(function (row) { row.dataset.gbMember = key; row.hidden = collapsed.has(key); body.appendChild(row); });
            });
            fit();
            observer.observe(body, {childList: true});
        }
        table.addEventListener('gridbar:collapse', function () {
            body.querySelectorAll('[data-gb-member]').forEach(function (r) { collapsed.add(r.dataset.gbMember); }); render();
        });
        table.addEventListener('gridbar:expand', function () { collapsed.clear(); render(); });
        new MutationObserver(fit).observe(table.tHead.rows[0], {attributes: true, subtree: true, attributeFilter: ['style', 'class']});
        render();
    }
    function boot() {
        document.querySelectorAll('body[data-mdd] select[data-dtcombo]').forEach(function (select) {
            var combo = select.__dtcombo, label = document.querySelector('label[for="' + select.id + '"]');
            if (!combo) return;
            if (label) combo.input.setAttribute('aria-label', label.textContent.trim());
            select.addEventListener('focus', function () { combo.input.focus(); });
        });
        document.querySelectorAll('body[data-mdd] table.win-grid').forEach(enhance);
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot); else boot();
}());
