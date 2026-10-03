/*
 * XCombo - the DCombo API (desktop_combo_c.js) on top of the project's searchable combo
 * countx_prod_combo.js (window.DesktopCombo). Used by the group-C account screens
 * party_voucher_desktop.html and premature_receipts.html (round 3, 2026-10-02).
 *
 * The page writes a plain <select class="f" id="..." style="left:..;top:..;width:..;height:.."> at the
 * designer Location/Size; XCombo fills it from the JSON rows and countx_prod_combo draws it (the
 * native select stays hidden and authoritative). The page code keeps its DCombo calls:
 *
 *   var cb = XCombo('cmbAccount', { columns:[{key:'AccountTitle',caption:'Account Title'},{key:'AccountCode',caption:'Code'}],
 *                                   valueKey:'Id', textKey:'AccountTitle', onSelect:fn(row), onLeave:fn(row) });
 *   cb.setData(rows); cb.value(); cb.setValue(id); cb.clear(); cb.row(); cb.rows(); cb.text(); cb.focus();
 *
 * Row 0 of the select is the empty "nothing selected" slot (BindDDL ZeroIndex:false leaves the text
 * empty). onSelect/onLeave fire when the user picks a different row (the native change event).
 * The page's show()/style.width/left/top on the select's id are mirrored onto the drawn combo.
 */
(function (global) {
    'use strict';

    function esc(s) {
        return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }

    /* select.style.{display,left,top,width,height} -> the wrap countx_prod_combo drew around it */
    function mirror(sel) {
        var c = sel.__dtcombo;
        if (!c || !c.wrap) return;
        var w = c.wrap.style, s = sel.style;
        w.display = s.display === 'none' ? 'none' : '';
        ['left', 'top', 'width', 'height'].forEach(function (p) { if (s[p]) w[p] = s[p]; });
    }

    function enhance(sel) {
        if (!sel || sel.tagName !== 'SELECT') return null;
        var c = global.DesktopCombo && global.DesktopCombo.enhance ? global.DesktopCombo.enhance(sel) : null;
        if (c && !sel.__xcMirror) {
            sel.__xcMirror = true;
            mirror(sel);
            if (global.MutationObserver) {
                new MutationObserver(function () { mirror(sel); })
                    .observe(sel, { attributes: true, attributeFilter: ['style'] });
            }
        }
        return c;
    }

    function XCombo(sel, opt) {
        if (typeof sel === 'string') sel = document.getElementById(sel);
        opt = opt || {};
        var columns = opt.columns || [{ key: opt.textKey || 'Name', caption: '' }];
        var valueKey = opt.valueKey || 'Id';
        var textKey = opt.textKey || columns[0].key;
        var data = [];

        if (columns.length > 1) sel.setAttribute('data-columns', columns.map(function (c) { return c.caption || ''; }).join('|'));
        else if (columns[0].caption) sel.setAttribute('data-dtcombo-caption', columns[0].caption);
        sel.innerHTML = '<option value=""></option>';
        enhance(sel);

        function idx() { return sel.selectedIndex; }
        function row() { var i = idx(); return i > 0 ? (data[i - 1] || null) : null; }
        function sync() { var c = sel.__dtcombo; if (c) c.syncFromSelect(); }

        sel.addEventListener('change', function () {
            var r = row();
            if (opt.onSelect) opt.onSelect(r);
            if (opt.onLeave) opt.onLeave(r);
        });

        var api = {
            setData: function (rows) {
                var keep = row() ? row()[valueKey] : null;
                data = rows || [];
                var h = '<option value=""></option>';
                for (var i = 0; i < data.length; i++) {
                    var r = data[i];
                    var extra = columns.length > 1
                        ? ' data-extra="' + esc(columns.slice(1).map(function (c) { return r[c.key] == null ? '' : String(r[c.key]).replace(/\|/g, '/'); }).join('|')) + '"'
                        : '';
                    h += '<option value="' + esc(r[valueKey]) + '"' + extra + '>' + esc(r[textKey]) + '</option>';
                }
                sel.innerHTML = h;
                sel.selectedIndex = 0;
                if (keep != null) api.setValue(keep);
                sync();
                return api;
            },
            rows: function () { return data; },
            row: row,
            value: function () { var r = row(); return r ? r[valueKey] : 0; },
            text: function () { var r = row(); return r ? String(r[textKey] == null ? '' : r[textKey]) : ''; },
            setValue: function (v) {
                var at = 0;
                for (var i = 0; i < data.length; i++) { if (String(data[i][valueKey]) === String(v)) { at = i + 1; break; } }
                sel.selectedIndex = at;
                sync();
                return at > 0;
            },
            clear: function () { sel.selectedIndex = 0; sync(); },
            focus: function () {
                var c = sel.__dtcombo;
                if (c && c.input) c.input.focus(); else sel.focus();
            },
            hide: function () { var c = sel.__dtcombo; if (c && c.closePop) c.closePop(false); },
            el: sel
        };
        return api;
    }

    /* Labels are drawn at their designer Location in the designer font (Segoe UI Semibold 9pt). Where the
       browser substitutes a wider font, a label could run into the control the designer put right after it;
       fitLabels() narrows such a label (scaleX from its left edge) so it ends 2px before that control. */
    function fitLabels(root) {
        var labels = (root || document).querySelectorAll('.dform .dl');
        for (var i = 0; i < labels.length; i++) {
            var L = labels[i];
            L.style.transform = '';
            if (L.offsetParent === null) continue;
            var par = L.parentNode, lr = L.getBoundingClientRect();
            if (!lr.width) continue;
            var best = null;
            var sibs = par.children;
            for (var j = 0; j < sibs.length; j++) {
                var c = sibs[j];
                if (c === L || c.offsetParent === null || c.tagName === 'OPTION') continue;
                var cs = global.getComputedStyle(c);
                if (cs.position !== 'absolute') continue;
                var r = c.getBoundingClientRect();
                if (!r.width || r.top >= lr.bottom - 1 || r.bottom <= lr.top + 1 || r.left < lr.left + 4) continue;
                if (!best || r.left < best) best = r.left;
            }
            if (best !== null && lr.right > best - 1) {
                var k = Math.max(0.7, (best - 2 - lr.left) / lr.width);
                L.style.transformOrigin = 'left center';
                L.style.transform = 'scaleX(' + k.toFixed(3) + ')';
            }
        }
    }
    var fitQueued = false;
    function queueFit() {
        if (fitQueued) return;
        fitQueued = true;
        setTimeout(function () { fitQueued = false; fitLabels(); }, 30);
    }
    function watchFit() {
        queueFit();
        if (!global.MutationObserver) return;
        new MutationObserver(function (recs) {
            for (var i = 0; i < recs.length; i++) {
                var t = recs[i].target;
                if (t && t.classList && t.classList.contains('dl') && recs[i].attributeName === 'style') continue;   /* our own transform */
                queueFit(); return;
            }
        }).observe(document.body, { subtree: true, attributes: true, attributeFilter: ['style', 'class'], childList: true, characterData: true });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', watchFit); else watchFit();
    if (document.fonts && document.fonts.ready) document.fonts.ready.then(queueFit);

    XCombo.enhance = enhance;
    XCombo.fitLabels = fitLabels;
    global.XCombo = XCombo;
})(window);
