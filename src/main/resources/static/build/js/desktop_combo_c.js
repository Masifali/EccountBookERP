/*
 * DCombo - a small searchable multi-column combo standing in for the desktop's Infragistics
 * UltraCombo (DDL.BindDDL / BindDDLNew). Used by the group-C account screens:
 * party_voucher_desktop.html, premature_receipts.html, bills_voucher_desktop.html.
 *
 *   const cb = DCombo(inputEl, { columns:[{key:'AccountTitle',caption:'Account Title'},...],
 *                                valueKey:'Id', textKey:'AccountTitle',
 *                                onSelect:function(row){}, onLeave:function(row){} });
 *   cb.setData(rows); cb.value(); cb.setValue(id); cb.clear(); cb.row(); cb.rows();
 *
 * Behaviour copied from the UltraCombo as these forms use it: typing filters the list (any
 * column, case-insensitive), Up/Down move, Enter picks, and leaving the box with text that does
 * not match the picked row clears the value (LimitToList). A value set that is not in the list
 * leaves the box empty - the desktop's "else Text = string.Empty" branch of every *Fill().
 */
(function (global) {
    'use strict';
    var openPopup = null;

    function esc(s) {
        return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    function DCombo(input, opt) {
        if (typeof input === 'string') input = document.getElementById(input);
        opt = opt || {};
        var columns = opt.columns || [{ key: opt.textKey || 'Name', caption: '' }];
        var valueKey = opt.valueKey || 'Id';
        var textKey = opt.textKey || columns[0].key;
        var data = [];
        var selected = null;
        var active = -1;
        var shown = [];
        var popup = document.createElement('div');
        popup.className = 'dcombo-popup';
        popup.style.display = 'none';
        document.body.appendChild(popup);
        input.setAttribute('autocomplete', 'off');
        input.classList.add('dcombo-input');

        function place() {
            var r = input.getBoundingClientRect();
            popup.style.left = (r.left + window.scrollX) + 'px';
            popup.style.top = (r.bottom + window.scrollY) + 'px';
            popup.style.minWidth = Math.max(r.width, opt.popupWidth || 0) + 'px';
        }

        function render() {
            var q = (input.value || '').toLowerCase();
            var exact = selected && String(selected[textKey]) === input.value;
            shown = data.filter(function (row) {
                if (!q || exact) return true;
                for (var i = 0; i < columns.length; i++) {
                    var v = row[columns[i].key];
                    if (v != null && String(v).toLowerCase().indexOf(q) >= 0) return true;
                }
                return false;
            });
            var h = '<table><thead><tr>';
            columns.forEach(function (c) { h += '<th>' + esc(c.caption) + '</th>'; });
            h += '</tr></thead><tbody>';
            shown.slice(0, 500).forEach(function (row, i) {
                h += '<tr data-i="' + i + '" class="' + (i === active ? 'act' : '') + '">';
                columns.forEach(function (c) { h += '<td>' + esc(row[c.key]) + '</td>'; });
                h += '</tr>';
            });
            h += '</tbody></table>';
            popup.innerHTML = h;
            var rows = popup.querySelectorAll('tbody tr');
            for (var k = 0; k < rows.length; k++) {
                rows[k].addEventListener('mousedown', function (e) {
                    e.preventDefault();
                    pick(shown[+this.getAttribute('data-i')]);
                    hide();
                });
            }
            var a = popup.querySelector('tr.act');
            if (a && a.scrollIntoView) a.scrollIntoView({ block: 'nearest' });
        }

        function show() {
            if (input.disabled || input.readOnly) return;
            if (openPopup && openPopup !== api) openPopup.hide();
            openPopup = api;
            place(); render();
            popup.style.display = 'block';
        }
        function hide() { popup.style.display = 'none'; if (openPopup === api) openPopup = null; }

        function pick(row, silent) {
            selected = row || null;
            input.value = row ? String(row[textKey] == null ? '' : row[textKey]) : '';
            if (!silent && opt.onSelect) opt.onSelect(selected);
        }

        input.addEventListener('focus', function () { active = -1; show(); });
        input.addEventListener('input', function () { active = 0; show(); });
        input.addEventListener('keydown', function (e) {
            if (e.key === 'ArrowDown') { if (popup.style.display === 'none') show(); active = Math.min(active + 1, shown.length - 1); render(); e.preventDefault(); }
            else if (e.key === 'ArrowUp') { active = Math.max(active - 1, 0); render(); e.preventDefault(); }
            else if (e.key === 'Enter') {
                if (popup.style.display !== 'none' && active >= 0 && shown[active]) { pick(shown[active]); hide(); }
            } else if (e.key === 'Escape') { hide(); }
        });
        input.addEventListener('blur', function () {
            setTimeout(function () {
                hide();
                if (!selected || String(selected[textKey]) !== input.value) {
                    if (input.value && shown.length === 1 && active >= 0) { pick(shown[0]); }
                    else if (selected && String(selected[textKey]) !== input.value) { pick(null); }
                    else if (!selected && input.value) { input.value = ''; }
                }
                if (opt.onLeave) opt.onLeave(selected);
            }, 120);
        });

        var api = {
            setData: function (rows) {
                var keep = selected ? selected[valueKey] : null;
                data = rows || [];
                selected = null;
                if (keep != null) api.setValue(keep, true); else if (!opt.keepText) input.value = '';
                return api;
            },
            rows: function () { return data; },
            value: function () { return selected ? selected[valueKey] : 0; },
            row: function () { return selected; },
            text: function () { return input.value; },
            setValue: function (v, silent) {
                var row = null;
                for (var i = 0; i < data.length; i++) { if (String(data[i][valueKey]) === String(v)) { row = data[i]; break; } }
                pick(row, silent !== false);
                return !!row;
            },
            clear: function () { pick(null, true); },
            focus: function () { input.focus(); },
            hide: hide,
            el: input
        };
        return api;
    }

    document.addEventListener('scroll', function () { if (openPopup) openPopup.hide(); }, true);
    global.DCombo = DCombo;
})(window);
