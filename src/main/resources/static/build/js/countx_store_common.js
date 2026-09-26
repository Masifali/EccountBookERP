/* ============================================================================================
 * Shared plumbing for every Store Management / Store Purchase screen.
 *
 * Nothing here decides business rules — those live in each screen's own file, next to the
 * desktop line they come from. This is the request helper, escaping, the picker dialog that
 * stands in for the desktop's GrdPopUp (F1 on a Warehouse / Rack cell), the slip window, and
 * ResolveWarehouseAndRack, which three desktop forms carry word for word.
 * ============================================================================================ */
(function () {
    'use strict';

    function $id(id) { return document.getElementById(id); }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        }
        return '';
    }

    function num(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function intOf(v) { return Math.trunc(num(v)); }
    function round(v, p) { var f = Math.pow(10, p); return Math.round(num(v) * f) / f; }

    function today() {
        var d = new Date();
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function isoDay(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[0] : '';
    }
    var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function gridDate(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1] : String(v || '');
    }
    /** "dd-MM-yyyy hh:mm tt" (322 / 321 history), or "dd-MMM-yyyy hh:mm tt" with monthName (330). */
    function gridDateTime(v, monthName) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return gridDate(v);
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        var mon = monthName ? MONTHS[parseInt(m[2], 10) - 1] : m[2];
        return m[3] + '-' + mon + '-' + m[1] + ' ' + String(h).padStart(2, '0') + ':' + m[5] + ' ' + ap;
    }

    /* ------------------------------------------------------------------ busy buttons
       The button whose click started a request is disabled with a spinner until the request
       settles; clicks on a busy button are swallowed, so one click = one request. A request
       started from the .then of another keeps the same button busy (carryBtn), so a chain
       (validate -> save -> reload) holds the button for its whole length. */
    var clickBtn = null, carryBtn = null;
    document.addEventListener('click', function (e) {
        var b = e.target && e.target.closest ? e.target.closest('button') : null;
        if (!b) return;
        if (b.classList.contains('cx-busy')) { e.preventDefault(); e.stopImmediatePropagation(); return; }
        clickBtn = b;
        setTimeout(function () { if (clickBtn === b) clickBtn = null; }, 0);
    }, true);
    function busy(b, delta) {
        if (!b) return;
        b._cxn = (b._cxn || 0) + delta;
        if (b._cxn > 0 && !b.classList.contains('cx-busy') && !b.classList.contains('cx-btn-busy')) {
            /* disabled is left to the page (rights etc.); cx-busy blocks the pointer and the
               capture listener above swallows keyboard/programmatic clicks. */
            b.classList.add('cx-busy');
            b.setAttribute('aria-busy', 'true');
            var sp = document.createElement('i'); sp.className = 'fa fa-spinner fa-spin cx-spin';
            b.insertBefore(sp, b.firstChild);
            document.body.classList.add('cx-loading');
        } else if (b._cxn <= 0) {
            b._cxn = 0;
            b.classList.remove('cx-busy');
            b.removeAttribute('aria-busy');
            var s0 = b.querySelector('.cx-spin'); if (s0) s0.remove();
            if (!document.querySelector('.cx-busy')) document.body.classList.remove('cx-loading');
        }
    }

    function request(url, options) {
        var btn = clickBtn || carryBtn;
        busy(btn, 1);
        return rawRequest(url, options).then(function (v) { release(btn); return v; }, function (e) { release(btn); throw e; });
    }
    function release(btn) {
        if (!btn) return;
        carryBtn = btn;
        setTimeout(function () { if (carryBtn === btn) carryBtn = null; busy(btn, -1); }, 0);
    }
    function rawRequest(url, options) {
        var opt = options || {};
        opt.credentials = 'same-origin';
        opt.headers = opt.headers || {};
        opt.headers.Accept = 'application/json';
        var token = document.querySelector('meta[name="_csrf"]');
        var header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header && opt.method && opt.method.toUpperCase() !== 'GET') {
            opt.headers[header.getAttribute('content')] = token.getAttribute('content');
        }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    }
    function getJson(url) { return request(url, {}); }
    function postJson(url, body) {
        return request(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body || {}) });
    }
    function qs(o) {
        var p = [];
        Object.keys(o).forEach(function (k) {
            var v = o[k];
            if (v === null || v === undefined || v === '') return;
            p.push(encodeURIComponent(k) + '=' + encodeURIComponent(v));
        });
        return p.length ? '?' + p.join('&') : '';
    }

    function fillSelect(el, rows, valueKey, textKey, blank) {
        if (typeof el === 'string') el = $id(el);
        if (!el) return;
        var keep = el.value;
        var h = blank === false ? '' : '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, textKey)) + '</option>';
        });
        el.innerHTML = h;
        if (keep) el.value = keep;
    }

    function openModal(id) { var m = $id(id); if (m) m.classList.add('is-open'); }
    function closeModal(id) { var m = $id(id); if (m) m.classList.remove('is-open'); }

    /**
     * GrdPopUp — a list to pick one row from. Resolves with the chosen row, or null on cancel.
     * columns: [{key, caption}]; the first column is what the desktop returns as ReturnName.
     */
    function pickFrom(title, rows, columns) {
        return new Promise(function (resolve) {
            var host = document.createElement('div');
            host.className = 'cx-modal is-open';
            var head = columns.map(function (c) { return '<th>' + esc(c.caption) + '</th>'; }).join('');
            var body = (rows || []).map(function (r, i) {
                return '<tr data-i="' + i + '" style="cursor:pointer">' +
                    columns.map(function (c) { return '<td>' + esc(ci(r, c.key)) + '</td>'; }).join('') + '</tr>';
            }).join('');
            host.innerHTML =
                '<div class="cx-modal-box small"><div class="cx-modal-head"><span>' + esc(title) +
                '</span><button type="button" data-x>&times;</button></div><div class="cx-modal-body">' +
                '<input type="text" class="win-textbox" placeholder="Filter" data-f style="margin-bottom:4px">' +
                '<div class="win-grid-container"><div class="win-grid-scroll"><table class="win-grid"><thead><tr>' +
                head + '</tr></thead><tbody>' + (body || '<tr><td colspan="' + columns.length + '">No rows</td></tr>') +
                '</tbody></table></div></div></div></div>';
            document.body.appendChild(host);
            function done(v) { host.remove(); resolve(v); }
            host.querySelector('[data-x]').onclick = function () { done(null); };
            host.querySelectorAll('tbody tr[data-i]').forEach(function (tr) {
                tr.onclick = function () { done(rows[parseInt(tr.getAttribute('data-i'), 10)]); };
            });
            var f = host.querySelector('[data-f]');
            f.oninput = function () {
                var t = f.value.toLowerCase();
                host.querySelectorAll('tbody tr[data-i]').forEach(function (tr) {
                    tr.style.display = tr.textContent.toLowerCase().indexOf(t) >= 0 ? '' : 'none';
                });
            };
            f.onkeydown = function (e) { if (e.key === 'Escape') done(null); };
            f.focus();
        });
    }

    /**
     * ResolveWarehouseAndRack — frmGSIssuance.cs:1102 (and the same method on 321 / 330).
     *   1. a rack flagged BaseRackId > 0 wins outright;
     *   2. one warehouse only: that warehouse, and its rack when it has exactly one;
     *   3. otherwise the configured default warehouse (by parent category), if the item has it;
     *   4. otherwise nothing.
     */
    function resolveWarehouseAndRack(itemRacks, configWarehouseId) {
        var none = { WarehouseId: 0, WarehouseName: null, RackId: 0, RackName: null };
        if (!itemRacks || !itemRacks.length) return none;
        var base = itemRacks.find(function (x) { return intOf(x.BaseRackId) > 0; });
        if (base) return { WarehouseId: base.WarehouseId, WarehouseName: base.WareHouseName, RackId: base.Id, RackName: base.RackName };
        var whs = distinct(itemRacks, 'WarehouseId');
        function pick(wh) {
            var racks = distinct(itemRacks.filter(function (x) { return x.WarehouseId === wh.WarehouseId; }), 'Id');
            if (racks.length === 1) return { WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: racks[0].Id, RackName: racks[0].RackName };
            return { WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: 0, RackName: null };
        }
        if (whs.length === 1) return pick(whs[0]);
        if (configWarehouseId > 0) {
            var w2 = whs.find(function (x) { return x.WarehouseId === configWarehouseId; });
            if (w2) return pick(w2);
        }
        return none;
    }

    function distinct(rows, key) {
        var seen = {}, out = [];
        (rows || []).forEach(function (r) { if (!seen[r[key]]) { seen[r[key]] = 1; out.push(r); } });
        return out;
    }

    /** Parent category 7 → PackingMaterialDefaultWarehouse, 8 → DefaultWarehouseForStoreFlow. */
    function configWarehouse(parentCategoryId, look) {
        var p = intOf(parentCategoryId);
        if (p === 7) return intOf(look.packingMaterialDefaultWarehouse);
        if (p === 8) return intOf(look.defaultWarehouseForStoreFlow);
        return 0;
    }

    /**
     * The printed slip. The Crystal layout (.rpt) is not recoverable, so this prints the slip
     * procedure's own rows as a table — the data is the report's, the layout is not.
     */
    function printSlip(url, title) {
        getJson(url).then(function (rows) {
            if (!rows || !rows.length) { alert('No Record Found For Display'); return; }
            var cols = Object.keys(rows[0]);
            var w = window.open('', '_blank');
            if (!w) { alert('Allow pop-ups to print.'); return; }
            var h = '<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
                'table{border-collapse:collapse}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
                '<h3>' + esc(title) + '</h3><table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') +
                '</tr></thead><tbody>' + rows.map(function (r) {
                    return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
                }).join('') + '</tbody></table><script>window.print()<\/script></body></html>';
            w.document.write(h);
            w.document.close();
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ page chrome
       (1) footer with a History button on the right (clicks the page's own History tab, so the
       page's tab() logic — history reload rules, detail clearing — runs unchanged);
       (2) every grid in a horizontally scrolling box of its own, never the whole page;
       (3) contains-search type-ahead on every select.win-combo (native select only matches prefixes). */
    function footer() {
        var tabH = document.querySelector('.win-tab[data-tab="tabHistory"]');
        var tabF = document.querySelector('.win-tab[data-tab="tabForm"]');
        /* a page that already has its own footer History button (.win-footer-strip) keeps it */
        if (!tabH || document.querySelector('.cx-footer, .win-footer-strip, #btnFooterHistory')) return;
        var f = document.createElement('div');
        f.className = 'cx-footer';
        f.innerHTML = '<span class="cx-footer-left"></span>' +
            '<button type="button" class="win-btn-action cx-footer-history"><i class="fa fa-history"></i> <span>History</span></button>';
        document.body.appendChild(f);
        var btn = f.querySelector('button'), lbl = btn.querySelector('span');
        function sync() {
            var onHist = tabH.classList.contains('is-active');
            lbl.textContent = onHist ? 'Form' : 'History';
            btn.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        }
        btn.addEventListener('click', function () {
            (tabH.classList.contains('is-active') && tabF ? tabF : tabH).click();
            sync();
        });
        new MutationObserver(sync).observe(tabH, { attributes: true, attributeFilter: ['class'] });
        sync();
    }
    function wrapGrids(root) {
        (root || document).querySelectorAll('table.win-grid').forEach(function (t) {
            if (t.closest('.win-grid-scroll, .cx-grid-wrap')) return;
            var w = document.createElement('div');
            w.className = 'cx-grid-wrap';
            t.parentNode.insertBefore(w, t);
            w.appendChild(t);
        });
    }
    /* Searchable combos: every select.win-combo (also the ones pages build inside grids at run time)
       opens a search box with a contains-filter instead of the native list — the desktop Infragistics
       combos filter as you type. Choosing sets the select's value and fires its normal 'change', so each
       page's own handlers run unchanged. Delegated on document, so re-rendered grid cells need nothing. */
    var pop = null, popSel = null, popRows = [], popActive = -1;
    function comboOk(sel) {
        return sel && sel.tagName === 'SELECT' && sel.classList.contains('win-combo') && !sel.disabled && !sel.multiple;
    }
    function buildPop() {
        pop = document.createElement('div');
        pop.className = 'cx-combo-pop';
        pop.innerHTML = '<input type="text" class="cx-combo-search" placeholder="Search..." autocomplete="off"><div class="cx-combo-list"></div>';
        document.body.appendChild(pop);
        var inp = pop.querySelector('input');
        inp.addEventListener('input', function () { renderPop(inp.value); });
        inp.addEventListener('keydown', function (e) {
            if (e.key === 'ArrowDown') { e.preventDefault(); movePop(1); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); movePop(-1); }
            else if (e.key === 'Enter') { e.preventDefault(); if (popActive >= 0) choosePop(popRows[popActive]); }
            else if (e.key === 'Escape') { e.preventDefault(); closePop(true); }
            else if (e.key === 'Tab') { if (popActive >= 0) choosePop(popRows[popActive]); else closePop(false); }
        });
        pop.addEventListener('mousedown', function (e) {
            var it = e.target.closest('.cx-combo-item');
            e.preventDefault();
            if (it) choosePop(popRows[parseInt(it.getAttribute('data-i'), 10)]);
        });
    }
    function renderPop(q) {
        q = String(q || '').toLowerCase().trim();
        var opts = Array.prototype.slice.call(popSel.options);
        var starts = [], contains = [];
        opts.forEach(function (o) {
            var t = o.text.toLowerCase();
            if (!q || t.indexOf(q) === 0) starts.push(o); else if (t.indexOf(q) >= 0) contains.push(o);
        });
        popRows = starts.concat(contains).slice(0, 500);
        popActive = -1;
        for (var i = 0; i < popRows.length; i++) if (popRows[i].value === popSel.value) { popActive = i; break; }
        if (popActive < 0 && q && popRows.length) popActive = 0;
        var list = pop.querySelector('.cx-combo-list');
        list.innerHTML = popRows.length ? popRows.map(function (o, i) {
            return '<div class="cx-combo-item' + (i === popActive ? ' is-active' : '') + '" data-i="' + i + '">' +
                (o.text ? esc(o.text) : '&nbsp;') + '</div>';
        }).join('') : '<div class="cx-combo-empty">No match</div>';
        var a = list.querySelector('.is-active'); if (a) a.scrollIntoView({ block: 'nearest' });
    }
    function movePop(d) {
        if (!popRows.length) return;
        popActive = Math.max(0, Math.min(popRows.length - 1, popActive + d));
        pop.querySelectorAll('.cx-combo-item').forEach(function (el, i) { el.classList.toggle('is-active', i === popActive); });
        var a = pop.querySelector('.cx-combo-item.is-active'); if (a) a.scrollIntoView({ block: 'nearest' });
    }
    function openPop(sel, firstChar) {
        if (!pop) buildPop();
        popSel = sel;
        var r = sel.getBoundingClientRect();
        pop.style.left = (r.left + window.scrollX) + 'px';
        pop.style.top = (r.bottom + window.scrollY) + 'px';
        pop.style.minWidth = Math.max(r.width, 220) + 'px';
        pop.style.display = 'block';
        var inp = pop.querySelector('input');
        inp.value = firstChar || '';
        renderPop(inp.value);
        /* keep it on screen when the select sits near the bottom */
        var pr = pop.getBoundingClientRect();
        if (pr.bottom > window.innerHeight && r.top > pr.height) pop.style.top = (r.top + window.scrollY - pr.height) + 'px';
        inp.focus({ preventScroll: true });              // a focus scroll would fire 'scroll' and close the box
    }
    function closePop(refocus) {
        if (!pop || pop.style.display !== 'block') return;
        pop.style.display = 'none';
        var sel = popSel; popSel = null;
        if (refocus && sel && document.body.contains(sel)) sel.focus();
    }
    function choosePop(o) {
        var sel = popSel;
        closePop(false);
        if (!sel || !o) return;
        if (sel.value !== o.value) {
            sel.value = o.value;
            sel.dispatchEvent(new Event('input', { bubbles: true }));
            sel.dispatchEvent(new Event('change', { bubbles: true }));
        }
        if (document.body.contains(sel)) sel.focus();
    }
    document.addEventListener('mousedown', function (e) {
        var sel = e.target;
        if (pop && pop.style.display === 'block' && !pop.contains(sel)) closePop(false);
        if (!comboOk(sel) || e.button !== 0) return;
        e.preventDefault();                       // no native list; the search box replaces it
        sel.focus();
        openPop(sel, '');
    }, true);
    document.addEventListener('keydown', function (e) {
        var sel = e.target;
        if (!comboOk(sel) || e.ctrlKey || e.metaKey) return;
        if (e.key.length === 1 && !e.altKey && e.key !== ' ') { e.preventDefault(); openPop(sel, e.key); }
        else if (e.key === 'F4' || (e.altKey && e.key === 'ArrowDown') || e.key === ' ') { e.preventDefault(); openPop(sel, ''); }
    }, true);
    window.addEventListener('resize', function () { closePop(false); });
    document.addEventListener('scroll', function (e) { if (pop && !pop.contains(e.target)) closePop(false); }, true);
    document.addEventListener('DOMContentLoaded', function () { footer(); wrapGrids(); });

    /**
     * Executes an async action with button busy indicator, spinner, and duplicate-click prevention.
     */
    function withBusy(btnOrId, actionFn) {
        var el = typeof btnOrId === 'string' ? $id(btnOrId) : btnOrId;
        if (el && el.disabled) return Promise.resolve();
        var oldHtml = el ? el.innerHTML : '';
        if (el) {
            el.disabled = true;
            el.classList.add('cx-btn-busy');
            el.innerHTML = '<i class="fa fa-spinner fa-spin"></i> Processing...';
        }
        return Promise.resolve().then(function () {
            return actionFn();
        }).finally(function () {
            if (el) {
                el.disabled = false;
                el.classList.remove('cx-btn-busy');
                el.innerHTML = oldHtml;
            }
        });
    }

    window.StoreCommon = {
        $id: $id, esc: esc, ci: ci, num: num, intOf: intOf, round: round,
        today: today, isoDay: isoDay, gridDate: gridDate, gridDateTime: gridDateTime,
        getJson: getJson, postJson: postJson, qs: qs, fillSelect: fillSelect,
        openModal: openModal, closeModal: closeModal, pickFrom: pickFrom,
        resolveWarehouseAndRack: resolveWarehouseAndRack, configWarehouse: configWarehouse,
        distinct: distinct, printSlip: printSlip, wrapGrids: wrapGrids, withBusy: withBusy
    };
})();
