/* ============================================================================================
 * countx_export_g_common.js - shared helpers of the Export contract pages
 *   /export/sales-contract (209), /export/contract-iii (879), /export/contract-schedule (216),
 *   /export/packing-detail-by-contract (240).
 * Conversion.ToInt / ToDouble, Math.Round(.., MidpointRounding.AwayFromZero), the desktop date
 * formats, the button-busy contract, fetch with CSRF, searchable combo binding, the GridEX-like
 * table renderer (X column, Σ totals, clickable codes, double-click, fullscreen), the shortcut
 * keys dialog and the PM / payment / other-items tab logic the 209, 879 and 240 forms share.
 * ============================================================================================ */
(function (global) {
    'use strict';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    /* Conversion.ToInt: 0 when not numeric (a decimal text truncates). */
    function netI(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? (n < 0 ? Math.ceil(n) : Math.floor(n)) : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function netB(v) { if (v === true) return true; var s = str(v).trim().toLowerCase(); return s === 'true' || s === '1' || s === 'yes'; }
    /* Math.Round(x, d, MidpointRounding.AwayFromZero) */
    function roundAway(x, d) {
        var p = Math.pow(10, d || 0), v = Math.abs(x) * p;
        v = Math.round(v + 1e-9);
        return (x < 0 ? -v : v) / p;
    }
    /* "#,##0.###"-like: thousands + up to dec decimals, trailing zeros dropped. */
    function fmt(v, dec) {
        var n = netD(v);
        if (dec === undefined) dec = 3;
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    /* "#,##0.00"-like with fixed decimals. */
    function fmtFixed(v, dec) {
        var parts = netD(v).toFixed(dec).split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return isoOf(d); }
    function addDays(iso, n) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(iso));
        if (!m) return '';
        var d = new Date(parseInt(m[1], 10), parseInt(m[2], 10) - 1, parseInt(m[3], 10));
        d.setDate(d.getDate() + netI(n));
        return isoOf(d);
    }
    /* whole days between two yyyy-MM-dd (a - b). */
    function daysBetween(a, b) {
        var ma = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(a)), mb = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(b));
        if (!ma || !mb) return 0;
        var da = Date.UTC(+ma[1], +ma[2] - 1, +ma[3]), db = Date.UTC(+mb[1], +mb[2] - 1, +mb[3]);
        return Math.round((da - db) / 86400000);
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : isoOf(d);
    }
    function ddMMMyyyy(v) {
        var s = isoDate(v); if (!s) return '';
        return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4);
    }
    function ddMMyyyyHm(v) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(s);
        if (!m) return ddMMMyyyy(s);
        var h = parseInt(m[4], 10), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + m[2] + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
    }
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    function dateNull(iso) { var s = isoDate(iso); return !s || s.indexOf('0001') === 0 || s.indexOf('1900') === 0; }

    // ------------------------------------------------------------------------------ busy / http

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }

    // ------------------------------------------------------------------------------ controls

    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /* DDL.BindDDL with ZeroIndex: an empty first row; the selection is kept when the value still exists. */
    function bind(id, rows, valueCol, textCol, noZero) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = noZero ? '' : '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = noZero && rows && rows.length ? str(col(rows[0], valueCol)) : '0';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function valI(id) { return netI(val(id)); }
    function valD(id) { return netD(val(id)); }
    function text(id) { var s = $id(id); if (!s) return ''; var o = s.options ? s.options[s.selectedIndex] : null; return o ? o.text : s.value; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function checked(id) { var e = $id(id); return !!(e && e.checked); }
    function setChecked(id, on) { var e = $id(id); if (e) e.checked = !!on; }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    /* the option row's data for a combo bound from a list: find by Id. */
    function rowOf(rows, idCol, id) { id = netI(id); for (var i = 0; i < (rows || []).length; i++) if (netI(col(rows[i], idCol)) === id) return rows[i]; return null; }
    function cancelWindow() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }
    function activeTag() { var a = document.activeElement; return a && a.getAttribute ? (a.getAttribute('data-tag') || '') : ''; }

    // ------------------------------------------------------------------------------ tabs / fullscreen

    function tabs(group, panelIds, onChange) {
        document.querySelectorAll('.win-tabs[data-tabs="' + group + '"] .win-tab').forEach(function (b) {
            b.addEventListener('click', function () { select(b.getAttribute('data-tab')); });
        });
        function select(panelId) {
            document.querySelectorAll('.win-tabs[data-tabs="' + group + '"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
            panelIds.forEach(function (p) { var e = $id(p); if (e) e.classList.toggle('is-active', p === panelId); });
            if (onChange) onChange(panelId);
        }
        return { select: select, current: function () { for (var i = 0; i < panelIds.length; i++) { var e = $id(panelIds[i]); if (e && e.classList.contains('is-active')) return panelIds[i]; } return panelIds[0]; } };
    }
    function initFullscreen() {
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') document.querySelectorAll('.ex-fullscreen').forEach(function (x) { x.classList.remove('ex-fullscreen'); });
        });
    }

    // ------------------------------------------------------------------------------ grid

    /**
     * grid(tableId, columns, opts): columns = [{key, num, fmt, dec, link, date, datetime, cls}],
     * opts = {withX, totals: [keys], onDelete(i), onDblClick(i), onLink(i, key), onSelect(i), buttons: [{key, text}], emptyId,
     *         rowClass(row), checkbox}.
     */
    function grid(tableId, columns, opts) {
        opts = opts || {};
        var t = $id(tableId);
        var g = { rows: [], current: -1 };
        function cellText(c, v) {
            if (c.fmt) return c.fmt(v);
            if (c.date) return shortDate(v);
            if (c.datetime) return ddMMyyyyHm(v);
            if (c.num) return fmt(v, c.dec === undefined ? 3 : c.dec);
            return str(v);
        }
        g.draw = function (rows) {
            g.rows = rows || [];
            if (!t) return;
            var tb = t.tBodies[0] || t.createTBody();
            var h = '';
            g.rows.forEach(function (r, i) {
                var cls = opts.rowClass ? (opts.rowClass(r) || '') : '';
                h += '<tr data-i="' + i + '"' + (cls ? ' class="' + cls + '"' : '') + (i === g.current ? ' data-current="1"' : '') + '>';
                if (opts.checkbox) h += '<td class="ctr"><input type="checkbox" class="win-rowcheck" data-i="' + i + '"/></td>';
                if (opts.withX) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
                (opts.buttons || []).forEach(function (b) {
                    h += '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-btn="' + esc(b.key) + '" data-i="' + i + '">' + esc(b.text) + '</button></td>';
                });
                columns.forEach(function (c) {
                    if (c.hidden) return;
                    var v = col(r, c.key), txt = cellText(c, v);
                    var inner = esc(txt);
                    if (c.link) inner = '<a class="win-code" data-i="' + i + '" data-key="' + esc(c.key) + '" href="javascript:void(0)">' + inner + '</a>';
                    h += '<td' + (c.num ? ' class="num"' : (c.cls ? ' class="' + c.cls + '"' : '')) + '>' + inner + '</td>';
                });
                h += '</tr>';
            });
            tb.innerHTML = h;
            var tf = t.tFoot;
            if (tf) {
                if (opts.totals && opts.totals.length && g.rows.length) {
                    var f = '<tr class="win-total">';
                    if (opts.checkbox) f += '<td></td>';
                    if (opts.withX) f += '<td></td>';
                    (opts.buttons || []).forEach(function () { f += '<td></td>'; });
                    columns.forEach(function (c) {
                        if (c.hidden) return;
                        if (opts.totals.indexOf(c.key) >= 0) {
                            var s = 0; g.rows.forEach(function (r) { s += netD(col(r, c.key)); });
                            f += '<td class="num">&Sigma; ' + esc(fmt(s, c.dec === undefined ? 3 : c.dec)) + '</td>';
                        } else f += '<td></td>';
                    });
                    tf.innerHTML = f + '</tr>';
                } else tf.innerHTML = '';
            }
            if (opts.emptyId) show(opts.emptyId, g.rows.length === 0);
        };
        g.total = function (key) { var s = 0; g.rows.forEach(function (r) { s += netD(col(r, key)); }); return s; };
        g.checkedIdx = function () { var out = []; if (!t) return out; t.querySelectorAll('input.win-rowcheck:checked').forEach(function (c) { out.push(netI(c.getAttribute('data-i'))); }); return out; };
        g.setCurrent = function (i) {
            g.current = i;
            if (!t) return;
            t.querySelectorAll('tbody tr').forEach(function (tr) { if (netI(tr.getAttribute('data-i')) === i) tr.setAttribute('data-current', '1'); else tr.removeAttribute('data-current'); });
        };
        if (t) {
            t.addEventListener('click', function (e) {
                var x = e.target.closest('button.win-x');
                if (x) { e.preventDefault(); g.setCurrent(netI(x.getAttribute('data-del'))); if (opts.onDelete) opts.onDelete(netI(x.getAttribute('data-del'))); return; }
                var b = e.target.closest('button[data-btn]');
                if (b) { e.preventDefault(); g.setCurrent(netI(b.getAttribute('data-i'))); if (opts.onButton) opts.onButton(netI(b.getAttribute('data-i')), b.getAttribute('data-btn'), b); return; }
                var a = e.target.closest('a.win-code');
                if (a) { e.preventDefault(); g.setCurrent(netI(a.getAttribute('data-i'))); if (opts.onLink) opts.onLink(netI(a.getAttribute('data-i')), a.getAttribute('data-key'), a); return; }
                var tr = e.target.closest('tbody tr');
                if (tr) { var i = netI(tr.getAttribute('data-i')); var changed = i !== g.current; g.setCurrent(i); if (opts.onSelect && (changed || !opts.selectOnce)) opts.onSelect(i); }
            });
            t.addEventListener('dblclick', function (e) {
                if (e.target.closest('button') || e.target.closest('a') || e.target.closest('input')) return;
                var tr = e.target.closest('tbody tr');
                if (tr && opts.onDblClick) { e.preventDefault(); opts.onDblClick(netI(tr.getAttribute('data-i'))); }
            });
            t.addEventListener('keydown', function (e) {
                if (g.current < 0) return;
                if (e.ctrlKey && e.key === 'Enter' && opts.onDblClick) { e.preventDefault(); opts.onDblClick(g.current); }
                else if (e.ctrlKey && e.key === ' ' && opts.onDelete && opts.withX) { e.preventDefault(); opts.onDelete(g.current); }
                else if (e.key === 'ArrowDown' && g.current < g.rows.length - 1) { e.preventDefault(); g.setCurrent(g.current + 1); if (opts.onSelect) opts.onSelect(g.current); }
                else if (e.key === 'ArrowUp' && g.current > 0) { e.preventDefault(); g.setCurrent(g.current - 1); if (opts.onSelect) opts.onSelect(g.current); }
            });
            t.tabIndex = 0;
        }
        return g;
    }

    // ------------------------------------------------------------------------------ shortcut keys dialog

    function shortcuts(rows, title) {
        var old = $id('exgShortcuts'); if (old) old.remove();
        var d = document.createElement('div');
        d.id = 'exgShortcuts'; d.className = 'exg-modal';
        var h = '<div class="exg-modal-box"><div class="pd-caption pd-caption-teal exg-modal-head"><span>' + esc(title || 'ShortCut Keys') + '</span><button type="button" class="ex-fs-btn" data-close="1">&times;</button></div>'
            + '<div class="ex-grid-scroll" style="max-height:60vh;"><table class="win-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>';
        rows.forEach(function (r) { h += '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; });
        d.innerHTML = h + '</tbody></table></div></div>';
        d.addEventListener('click', function (e) { if (e.target === d || e.target.closest('[data-close]')) d.remove(); });
        document.body.appendChild(d);
    }

    // ------------------------------------------------------------------------------ history filter (209 / 879 / 240)

    /** The groupBox5 "Filters" of the contract forms as the /history body. */
    function contractHistoryBody(ids) {
        var by = radio(ids.dateBy || 'historyDateBy') || 'doc';
        return {
            fromChecked: checked(ids.fromChecked), fromDate: val(ids.fromDate),
            toChecked: checked(ids.toChecked), toDate: val(ids.toDate),
            dateBy: by, fromDocNo: valI(ids.fromDocNo), toDocNo: valI(ids.toDocNo), customerId: valI(ids.customer)
        };
    }

    // ------------------------------------------------------------------------------ the PM tab (209 / 879 / 240)

    /**
     * pmTab(cfg): the "Packing Material" entry panel + grid shared by the three forms.
     * cfg.ids = {brand, brandUom, brandPackType, brandOuter, brandInner, pmItem, pmUom, pmOuter, pmInner, rate, amount, remarks,
     *            add, update, cancel, grid, brandByCode, brandByName, pmByCode, pmByName}
     * cfg.api = {lastRate: url?itemId=, pmUoms: url?itemId=}, cfg.fcyDecimals(), cfg.messagesIII (FormHelper texts for 879),
     * cfg.onChanged() after any grid change.
     */
    function pmTab(cfg) {
        var ids = cfg.ids;
        var P = { rows: [], brandItems: [], pmItems: [], updateIndex: -1, itemIdOfPreviousRate: 0, uoms: [] };
        var g = grid(ids.grid, [
            { key: 'BrandCode' }, { key: 'BrandName' }, { key: 'BrandUom' }, { key: 'PackingType' },
            { key: 'BrandOuterQty', num: true }, { key: 'BrandInnerQty', num: true },
            { key: 'PmItemCode' }, { key: 'PmItemName' }, { key: 'PmItemUom' },
            { key: 'PmItemOuterQty', num: true }, { key: 'PmItemInnerQty', num: true },
            { key: 'Rate', num: true }, { key: 'Amount', num: true }, { key: 'Remarks' }
        ], {
            withX: true, totals: ['BrandOuterQty', 'BrandInnerQty', 'PmItemOuterQty', 'PmItemInnerQty', 'Amount'],
            onDelete: function (i) { P.remove(i); },
            onDblClick: function (i) { P.edit(i); }
        });
        function dec() { return cfg.fcyDecimals ? cfg.fcyDecimals() : 2; }
        function draw() { g.draw(P.rows); if (cfg.onChanged) cfg.onChanged(); }
        P.draw = draw;
        P.grid = g;
        /* BindBrandItemsFromOutPutGrid / FilldtBrandForPMItem grouping of the detail grid (ItemId, PackTypeId, PackSizeId). */
        P.brandFromDetails = function (details, packSizeKey, bagsKey) {
            var acc = {}, order = [];
            (details || []).forEach(function (r) {
                var key = netI(r.ItemId) + '|' + netI(r.PackTypeId) + '|' + netI(r[packSizeKey]);
                if (!acc[key]) {
                    acc[key] = { Id: netI(r.ItemId), ItemName: str(r.ItemName), ItemCode: str(r.ItemCode), PackingTypeId: netI(r.PackTypeId), PackingType: str(r.PackType),
                        PackUomId: netI(r[packSizeKey]), PackUom: str(r[packSizeKey === 'OuterUomId' ? 'OuterUom' : 'PackSize']), bags: 0, eq: netD(r.QtyEquivalent || r.OuterQtyEquivalent), M_Ton: 0 };
                    order.push(key);
                }
                acc[key].bags += netD(r[bagsKey]); acc[key].M_Ton += netD(r.QtyMTon);
            });
            return order.map(function (k) { var a = acc[k]; return { Id: a.Id, ItemName: a.ItemName, ItemCode: a.ItemCode, PackingTypeId: a.PackingTypeId, PackingType: a.PackingType, PackUomId: a.PackUomId, PackUom: a.PackUom, OuterQty: a.bags, InnerQty: a.bags * a.eq, M_Ton: a.M_Ton }; });
        };
        P.bindBrands = function (brandItems) {
            P.brandItems = brandItems || [];
            var byCode = checked(ids.brandByCode);
            var keep = valI(ids.brand);
            var s = $id(ids.brand);
            if (!s) return;
            var h = '<option value="0"></option>';
            P.brandItems.forEach(function (r, i) { h += '<option value="' + r.Id + '" data-i="' + i + '">' + esc(byCode ? r.ItemCode : r.ItemName) + '</option>'; });
            s.innerHTML = h;
            s.value = str(keep);
            if (s.value !== str(keep)) s.value = '0';
            if (P.brandItems.length === 0) s.value = '0';
            refreshCombos();
        };
        P.bindPmItems = function (pmItems) {
            P.pmItems = pmItems || [];
            var byCode = checked(ids.pmByCode);
            bind(ids.pmItem, P.pmItems.map(function (r) { return { Id: r.Id, name: byCode ? r.ItemCode : r.ItemName }; }), 'Id', 'name');
        };
        function selectedBrand() { return rowOf(P.brandItems, 'Id', val(ids.brand)); }
        /* cmbBrandForPmEntry_Leave: the brand's own uom / packing type / outer & inner qty. */
        P.brandLeave = function () {
            var b = selectedBrand();
            if (b && netI(b.Id) > 0) {
                bind(ids.brandUom, [{ Id: b.PackUomId, name: b.PackUom }], 'Id', 'name', true);
                bind(ids.brandPackType, [{ Id: b.PackingTypeId, name: b.PackingType }], 'Id', 'name', true);
                setText(ids.brandOuter, fmt(b.OuterQty, 2));
                setText(ids.brandInner, fmt(b.InnerQty, 2));
            } else {
                bind(ids.brandUom, [], 'Id', 'name', true); bind(ids.brandPackType, [], 'Id', 'name', true);
                setText(ids.brandOuter, '0'); setText(ids.brandInner, '0');
            }
        };
        /* CmbPMItem_Leave: GetPmItemRate (once per item) + BindPmItemPackUom. */
        P.pmItemLeave = function () {
            var itemId = valI(ids.pmItem);
            var chain = Promise.resolve();
            if (itemId > 0 && itemId !== P.itemIdOfPreviousRate && cfg.api.lastRate) {
                chain = getJson(cfg.api.lastRate + itemId).then(function (r) {
                    var rate = netD(r && r.rate);
                    if (rate > 0) { setText(ids.rate, fmt(rate, 2)); P.itemIdOfPreviousRate = itemId; }
                    else { P.itemIdOfPreviousRate = 0; setText(ids.rate, '0'); }
                    P.calcAmount();
                }, function (e) { box(e.message); });
            }
            return chain.then(function () {
                var keepText = text(ids.pmUom);
                if (itemId <= 0 || !cfg.api.pmUoms) { bind(ids.pmUom, [], 'Id', 'UOMCode'); return; }
                return getJson(cfg.api.pmUoms + itemId).then(function (rows) {
                    P.uoms = rows || [];
                    var s = $id(ids.pmUom);
                    bind(ids.pmUom, P.uoms, 'Id', 'UOMCode');
                    if (keepText) { var m = P.uoms.filter(function (u) { return str(u.UOMCode) === keepText; })[0]; setVal(ids.pmUom, m ? m.Id : 0); }
                    else if (s) s.value = '0';
                    refreshCombos();
                }, function (e) { box(e.message); });
            });
        };
        /* CalculatePackingAmount: Qty = outer if > 0 else inner; Math.Round(.., fcy decimals, AwayFromZero). */
        P.calcAmount = function (which) {
            var outer = valD(ids.pmOuter), inner = valD(ids.pmInner), rate = valD(ids.rate);
            var qty = which === 'inner' ? inner : (which === 'outer' ? outer : (outer > 0 ? outer : inner));
            if (qty > 0 && rate > 0) setText(ids.amount, fmtFixed(roundAway(qty * rate, dec()), dec()));
            else setText(ids.amount, '0');
        };
        function validation() {
            var iii = !!cfg.messagesIII;
            function need(cond, m209, m879) { if (cond) { box(iii ? m879 : m209); return true; } return false; }
            if (need(valI(ids.brand) === 0, 'Brand Item Is required', 'Brand Item field is required')) { focus(ids.brand); return false; }
            if (need(valI(ids.brandUom) === 0, 'Brand Uom Is required', 'Brand Uom field is required')) { focus(ids.brandUom); return false; }
            if (need(valI(ids.brandPackType) === 0, 'Item Type Is required', 'Brand PackingType field is required')) { focus(ids.brandPackType); return false; }
            if (need(valD(ids.brandOuter) === 0, 'Brand Outer Qty field required', 'Brand Outer Qty must be a non-zero number')) { focus(ids.brandOuter); return false; }
            if (need(valD(ids.brandInner) === 0, 'Brand Inner Qty field required', 'Brand Inner Qty must be a non-zero number')) { focus(ids.brandInner); return false; }
            if (need(valI(ids.pmItem) === 0, 'Item Is required', 'PM Item field is required')) { focus(ids.pmItem); return false; }
            if (need(valI(ids.pmUom) === 0, 'Pm Uom Is required', 'PM Item Uom field is required')) { focus(ids.pmUom); return false; }
            if (valD(ids.pmInner) === 0 && valD(ids.pmOuter) === 0) { box('Pm OuterQty or Pm InnerQty is required'); focus(ids.pmOuter); return false; }
            if (valD(ids.pmOuter) !== 0 && valD(ids.pmInner) !== 0) {
                box('You can either add the PM OuterQty or the PM InnerQty at a time, not both');
                setText(ids.pmInner, ''); P.calcAmount('outer'); focus(ids.pmInner); return false;
            }
            if (need(valD(ids.rate) === 0, 'Rate field required', 'Item Rate must be a non-zero number')) { focus(ids.rate); return false; }
            if (need(valD(ids.amount) === 0, 'Amount field required', 'Amount must be a non-zero number')) { focus(ids.amount); return false; }
            return true;
        }
        /* ValidatePmQty(Update): the grid's rows of the same Brand/Uom/PackingType plus the entry vs the brand quantities. */
        function validatePmQty(update) {
            var brandOuter = valD(ids.brandOuter), brandInner = valD(ids.brandInner);
            var bId = valI(ids.brand), uId = valI(ids.brandUom), pId = valI(ids.brandPackType);
            var curOuter = valD(ids.pmOuter), curInner = valD(ids.pmInner);
            var gridOuter = 0, gridInner = 0;
            P.rows.forEach(function (r, i) {
                if (netI(r.BrandId) === bId && netI(r.BrandUomId) === uId && netI(r.PackingTypeId) === pId && (!update || i !== P.updateIndex)) { gridOuter += netD(r.PmItemOuterQty); gridInner += netD(r.PmItemInnerQty); }
            });
            var totalOuter = gridOuter + curOuter;
            if (curOuter > 0 && totalOuter > brandOuter) {
                var d1 = totalOuter - brandOuter;
                throw new Error("PmOuterQty can't be greater than Brand OuterQty.\nBrand OuterQty is " + brandOuter + "\nPmOuterQty is " + curOuter + (gridOuter > 0 ? "\nPmOuterQty in Grid is " + gridOuter : '') + "\nDifference is " + d1);
            }
            var totalInner = gridInner + curInner;
            if (curInner > 0 && totalInner > brandInner) {
                var d2 = totalInner - brandInner;
                throw new Error("PmInnerQty can't be greater than Brand InnerQty.\nBrand InnerQty is " + brandInner + "\nPmInnerQty is " + curInner + (gridInner > 0 ? "\nPmInnerQty in Grid is " + gridInner : '') + "\nDifference is " + d2);
            }
        }
        function entryRow(id) {
            var b = selectedBrand() || {};
            var pm = rowOf(P.pmItems, 'Id', val(ids.pmItem)) || {};
            return {
                Id: id || 0, BrandId: valI(ids.brand), BrandCode: str(b.ItemCode), BrandName: str(b.ItemName),
                BrandUomId: valI(ids.brandUom), BrandUom: text(ids.brandUom), PackingTypeId: valI(ids.brandPackType), PackingType: text(ids.brandPackType),
                BrandOuterQty: valD(ids.brandOuter), BrandInnerQty: valD(ids.brandInner),
                PmItemId: valI(ids.pmItem), PmItemCode: str(pm.ItemCode), PmItemName: str(pm.ItemName),
                PmItemUomId: valI(ids.pmUom), PmItemUom: text(ids.pmUom),
                PmItemOuterQty: valD(ids.pmOuter), PmItemInnerQty: valD(ids.pmInner), Rate: valD(ids.rate), Amount: valD(ids.amount), Remarks: val(ids.remarks)
            };
        }
        P.reset = function () {
            P.updateIndex = -1;
            setVal(ids.brand, 0); bind(ids.brandUom, [], 'Id', 'name', true); bind(ids.brandPackType, [], 'Id', 'name', true);
            setText(ids.brandOuter, '0'); setText(ids.brandInner, '0');
            setVal(ids.pmItem, 0); bind(ids.pmUom, [], 'Id', 'UOMCode');
            setText(ids.pmOuter, '0'); setText(ids.pmInner, '0'); setText(ids.rate, '0'); setText(ids.amount, '0'); setText(ids.remarks, '');
            show(ids.add, true); show(ids.update, false); show(ids.cancel, false);
        };
        P.add = function () {
            try {
                if (!validation()) return;
                validatePmQty(false);
                P.rows.push(entryRow(0));
                draw(); P.reset(); focus(ids.brand);
            } catch (e) { box(e.message); }
        };
        P.update = function () {
            try {
                if (!validation()) return;
                if (P.updateIndex < 0) return;
                validatePmQty(true);
                var id = netI(P.rows[P.updateIndex].Id);
                P.rows[P.updateIndex] = entryRow(id);
                draw(); P.reset(); focus(ids.brand);
            } catch (e) { box(e.message); }
        };
        P.cancel = function () { P.reset(); };
        P.edit = function (i) {
            var r = P.rows[i]; if (!r) return;
            P.updateIndex = i;
            setVal(ids.brand, r.BrandId);
            bind(ids.brandUom, [{ Id: r.BrandUomId, name: r.BrandUom }], 'Id', 'name', true);
            bind(ids.brandPackType, [{ Id: r.PackingTypeId, name: r.PackingType }], 'Id', 'name', true);
            setText(ids.brandOuter, fmt(r.BrandOuterQty, 2)); setText(ids.brandInner, fmt(r.BrandInnerQty, 2));
            setVal(ids.pmItem, r.PmItemId);
            bind(ids.pmUom, [{ Id: r.PmItemUomId, UOMCode: r.PmItemUom }], 'Id', 'UOMCode'); setVal(ids.pmUom, r.PmItemUomId);
            P.itemIdOfPreviousRate = netI(r.PmItemId);
            setText(ids.pmOuter, fmt(r.PmItemOuterQty)); setText(ids.pmInner, fmt(r.PmItemInnerQty));
            setText(ids.rate, fmt(r.Rate, 2)); setText(ids.amount, fmt(r.Amount, dec())); setText(ids.remarks, r.Remarks);
            show(ids.add, false); show(ids.update, true); show(ids.cancel, true);
            focus(ids.brand);
        };
        P.remove = function (i) {
            if (P.updateIndex !== -1) { box('Reset  the Packing Material Detail first...'); return; }
            if (!P.rows[i]) return;
            if (netI(P.rows[i].Id) > 0 && !ask('Are you sure to Delete?')) return;
            P.rows.splice(i, 1);
            draw();
        };
        /* AutoFillGridFromMapping: every (brand, mapped pm item) pair not yet in the grid. */
        P.autoMap = function (itemPmMap) {
            try {
                if (!itemPmMap || itemPmMap.length === 0) throw new Error('There are no items to map with Packing Material');
                if (!P.brandItems || P.brandItems.length === 0) throw new Error('No Brand Items found');
                P.brandItems.forEach(function (b) {
                    var brandId = netI(b.Id);
                    itemPmMap.filter(function (m) { return netI(m.ItemId) === brandId; }).forEach(function (m) {
                        var pmItemId = netI(m.PmItemId);
                        var exists = P.rows.some(function (r) { return netI(r.BrandId) === brandId && netI(r.PmItemId) === pmItemId; });
                        if (!exists) P.rows.push({ Id: 0, BrandId: brandId, BrandCode: b.ItemCode, BrandName: b.ItemName, BrandUomId: b.PackUomId, BrandUom: b.PackUom,
                            PackingTypeId: b.PackingTypeId, PackingType: b.PackingType, BrandOuterQty: b.OuterQty, BrandInnerQty: b.InnerQty,
                            PmItemId: pmItemId, PmItemCode: str(m.PmItemCode), PmItemName: str(m.PmItemName), PmItemUomId: netI(m.PmBaseUomId), PmItemUom: str(m.PmBaseUomCode),
                            PmItemOuterQty: b.OuterQty, PmItemInnerQty: 0, Rate: 0, Amount: 0, Remarks: '' });
                    });
                });
                draw();
            } catch (e) { box('Error in AutoFillGridFromMapping: ' + e.message); }
        };
        P.wire = function () {
            var e;
            if ((e = $id(ids.brand))) e.addEventListener('change', P.brandLeave);
            if ((e = $id(ids.pmItem))) e.addEventListener('change', function () { P.pmItemLeave(); });
            if ((e = $id(ids.pmOuter))) e.addEventListener('input', function () { P.calcAmount('outer'); });
            if ((e = $id(ids.pmInner))) e.addEventListener('input', function () { P.calcAmount('inner'); });
            if ((e = $id(ids.rate))) e.addEventListener('input', function () { P.calcAmount(); });
            if ((e = $id(ids.add))) e.addEventListener('click', P.add);
            if ((e = $id(ids.update))) e.addEventListener('click', P.update);
            if ((e = $id(ids.cancel))) e.addEventListener('click', P.cancel);
            [ids.brandByCode, ids.brandByName].forEach(function (id) { var r = $id(id); if (r) r.addEventListener('change', function () { P.bindBrands(P.brandItems); }); });
            [ids.pmByCode, ids.pmByName].forEach(function (id) { var r = $id(id); if (r) r.addEventListener('change', function () { P.bindPmItems(P.pmItems); }); });
            [ids.pmOuter, ids.pmInner, ids.brandOuter, ids.brandInner, ids.rate].forEach(function (id) { var t = $id(id); if (t) t.addEventListener('keypress', numericOnly); });
        };
        return P;
    }

    /* CommonServices.OnlytextNumberFunction / the KeyPress guards: digits and one dot. */
    function numericOnly(e) {
        if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
        if (!/[0-9.]/.test(e.key) || (e.key === '.' && String(e.target.value).indexOf('.') >= 0)) e.preventDefault();
    }
    function integerOnly(e) {
        if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
        if (!/[0-9]/.test(e.key)) e.preventDefault();
    }

    // ------------------------------------------------------------------------------ payment tab (209 / 879)

    function paymentTab(cfg) {
        var ids = cfg.ids;
        var T = { rows: [], updateIndex: -1 };
        var g = grid(ids.grid, [{ key: 'PaymentTerm' }, { key: 'PrcntOfTotal', num: true, dec: 2 }, { key: 'FcyAmount', num: true, dec: 4 }, { key: 'Remarks' }],
            { withX: true, totals: ['PrcntOfTotal', 'FcyAmount'], onDelete: function (i) { T.remove(i); }, onDblClick: function (i) { T.edit(i); } });
        T.grid = g;
        function dec() { return cfg.fcyDecimals ? cfg.fcyDecimals() : 2; }
        function draw() { g.draw(T.rows); if (cfg.onChanged) cfg.onChanged(); }
        T.draw = draw;
        /* PercntOfTotal_TextChanged */
        T.pctChanged = function () {
            var prcnt = valD(ids.pct), fcy = netD(cfg.headerFcyAmount());
            if (prcnt > 100) { setText(ids.pct, '0'); setText(ids.amount, '0'); box("%age Can't be Greater Than 100"); }
            else if (fcy > 0) setText(ids.amount, fmtFixed(roundAway(prcnt * fcy / 100, dec()), dec()));
            else { setText(ids.amount, '0'); box('Fcy Amount is Zero'); }
        };
        function validation() {
            var iii = !!cfg.messagesIII;
            if (valI(ids.term) === 0) { box(iii ? 'Payment Term field is required' : 'Payment Term field required'); focus(ids.term); return false; }
            if (valD(ids.pct) === 0) { box(iii ? '% Of Total must be a non-zero number' : '% of Total field required'); focus(ids.pct); return false; }
            if (valD(ids.amount) === 0) { box(iii ? 'Fcy Amount must be a non-zero number' : 'Fcy Amount field required'); focus(ids.amount); return false; }
            return true;
        }
        T.reset = function () {
            T.updateIndex = -1;
            setVal(ids.term, 0); setText(ids.pct, '0'); setText(ids.amount, '0'); setText(ids.remarks, '');
            show(ids.add, true); show(ids.update, false); show(ids.cancel, false);
        };
        T.add = function () {
            if (!validation()) return;
            var prcnt = valD(ids.pct);
            if (prcnt > 100) { box("Percent can't be Greater than 100%"); focus(ids.pct); return; }
            if (T.rows.length > 0) {
                for (var i = 0; i < T.rows.length; i++) if (netI(T.rows[i].PaymentTermId) === valI(ids.term)) { box("You Can't Add Same Payment Term"); focus(ids.term); return; }
                var total = g.total('PrcntOfTotal');
                if (total + prcnt > 100) { box("Toatl Percent can't be Greater than 100%.Remaing " + (100 - total)); setText(ids.pct, str(100 - total)); return; }
            }
            T.rows.push({ PaymentTermId: valI(ids.term), PaymentTerm: text(ids.term), PrcntOfTotal: prcnt, FcyAmount: valD(ids.amount), Remarks: val(ids.remarks) });
            draw(); T.reset(); focus(ids.term);
        };
        T.update = function () {
            if (!validation()) return;
            if (T.updateIndex < 0) return;
            var prcnt = valD(ids.pct);
            if (prcnt > 100) { box("Percent can't be Greater than 100%"); focus(ids.pct); return; }
            for (var i = 0; i < T.rows.length; i++) if (i !== T.updateIndex && netI(T.rows[i].PaymentTermId) === valI(ids.term)) { box("You Can't Add Same Payment Term"); focus(ids.term); return; }
            var total = 0; T.rows.forEach(function (r, j) { if (j !== T.updateIndex) total += netD(r.PrcntOfTotal); });
            if (total + prcnt > 100) { box("Toatl Percent can't be Greater than 100%.Remaing " + (100 - total)); setText(ids.pct, str(100 - total)); return; }
            T.rows[T.updateIndex] = { PaymentTermId: valI(ids.term), PaymentTerm: text(ids.term), PrcntOfTotal: prcnt, FcyAmount: valD(ids.amount), Remarks: val(ids.remarks) };
            draw(); T.reset(); focus(ids.term);
        };
        T.cancel = function () { T.reset(); };
        T.edit = function (i) {
            var r = T.rows[i]; if (!r) return;
            T.updateIndex = i;
            setVal(ids.term, r.PaymentTermId); setText(ids.pct, fmt(r.PrcntOfTotal, 2)); setText(ids.amount, fmt(r.FcyAmount, 4)); setText(ids.remarks, r.Remarks);
            show(ids.add, false); show(ids.update, true); show(ids.cancel, true);
            focus(ids.term);
        };
        T.remove = function (i) {
            if (T.updateIndex !== -1) { box('Reset  the Payment Detail first...'); return; }
            if (!T.rows[i]) return;
            T.rows.splice(i, 1); draw();
        };
        /* the detail payment term's description into the detail remarks (CmbPaymentTermDetail_TextChanged). */
        T.termChanged = function () {
            var r = rowOf(cfg.terms(), 'Id', val(ids.term));
            if (r && str(r.desc)) setText(ids.remarks, r.desc);
        };
        T.wire = function () {
            var e;
            if ((e = $id(ids.pct))) { e.addEventListener('change', T.pctChanged); e.addEventListener('keypress', numericOnly); }
            if ((e = $id(ids.term))) e.addEventListener('change', T.termChanged);
            if ((e = $id(ids.add))) e.addEventListener('click', T.add);
            if ((e = $id(ids.update))) e.addEventListener('click', T.update);
            if ((e = $id(ids.cancel))) e.addEventListener('click', T.cancel);
        };
        return T;
    }

    // ------------------------------------------------------------------------------ other items tab (209 / 879)

    function otherItemsTab(cfg) {
        var ids = cfg.ids;
        var O = { rows: [], items: [], updateIndex: -1 };
        var g = grid(ids.grid, [{ key: 'ItemName' }, { key: 'Qty', num: true }, { key: 'Rate', num: true }, { key: 'Amount', num: true }, { key: 'Remarks' }],
            { withX: true, totals: ['Qty', 'Amount'], onDelete: function (i) { O.remove(i); }, onDblClick: function (i) { O.edit(i); } });
        O.grid = g;
        function draw() { g.draw(O.rows); if (cfg.onChanged) cfg.onChanged(); }
        O.draw = draw;
        O.bindItems = function (items) {
            O.items = items || [];
            var byCode = ids.byCode && checked(ids.byCode);
            bind(ids.item, O.items.map(function (r) { return { Id: r.Id, name: byCode ? r.ItemCode : r.ItemName }; }), 'Id', 'name');
        };
        O.calc = function () { var q = valD(ids.qty), r = valD(ids.rate); if (q > 0 && r > 0) setText(ids.amount, str(q * r)); };
        function validation() {
            var iii = !!cfg.messagesIII;
            if (valI(ids.item) === 0) { box(iii ? 'Item field is required' : 'Item field required'); focus(ids.item); return false; }
            if (valD(ids.qty) === 0) { box(iii ? 'Qty must be a non-zero number' : 'Qty field required'); focus(ids.qty); return false; }
            if (valD(ids.rate) === 0) { box(iii ? 'Rate must be a non-zero number' : 'Rate field required'); focus(ids.rate); return false; }
            if (valD(ids.amount) === 0) { box(iii ? 'Amount must be a non-zero number' : 'Amount field required'); focus(ids.amount); return false; }
            return true;
        }
        function row() { var it = rowOf(O.items, 'Id', val(ids.item)) || {}; return { ItemId: valI(ids.item), ItemName: str(it.ItemName), Qty: valD(ids.qty), Rate: valD(ids.rate), Amount: valD(ids.amount), Remarks: val(ids.remarks) }; }
        O.reset = function () {
            O.updateIndex = -1;
            setVal(ids.item, 0); setText(ids.qty, '0'); setText(ids.rate, '0'); setText(ids.amount, '0'); setText(ids.remarks, '');
            show(ids.add, true); show(ids.update, false); show(ids.cancel, false);
        };
        O.add = function () { if (!validation()) return; O.rows.push(row()); draw(); O.reset(); focus(ids.item); };
        O.update = function () { if (!validation() || O.updateIndex < 0) return; O.rows[O.updateIndex] = row(); draw(); O.reset(); focus(ids.item); };
        O.cancel = function () { O.reset(); };
        O.edit = function (i) {
            var r = O.rows[i]; if (!r) return;
            O.updateIndex = i;
            setVal(ids.item, r.ItemId); setText(ids.qty, fmt(r.Qty)); setText(ids.rate, fmt(r.Rate)); setText(ids.amount, fmt(r.Amount, 4)); setText(ids.remarks, r.Remarks);
            show(ids.add, false); show(ids.update, true); show(ids.cancel, true);
            focus(ids.item);
        };
        O.remove = function (i) {
            if (O.updateIndex !== -1) { box('Reset  the Other Item Detail first...'); return; }
            if (!O.rows[i]) return;
            O.rows.splice(i, 1); draw();
        };
        O.wire = function () {
            var e;
            if ((e = $id(ids.qty))) e.addEventListener('input', O.calc);
            if ((e = $id(ids.rate))) e.addEventListener('input', O.calc);
            if ((e = $id(ids.add))) e.addEventListener('click', O.add);
            if ((e = $id(ids.update))) e.addEventListener('click', O.update);
            if ((e = $id(ids.cancel))) e.addEventListener('click', O.cancel);
            if (ids.byCode) [ids.byCode, ids.byName].forEach(function (id) { var r = $id(id); if (r) r.addEventListener('change', function () { O.bindItems(O.items); }); });
        };
        return O;
    }

    // ------------------------------------------------------------------------------ other charges tab (209 / 879)

    function otherChargesTab(cfg) {
        var ids = cfg.ids;
        var C = { rows: [], updateIndex: -1 };
        var g = grid(ids.grid, [{ key: 'ChargesItem' }, { key: 'AddAmount', num: true, dec: 2 }, { key: 'LessAmount', num: true, dec: 2 }, { key: 'Remarks' }],
            { withX: true, totals: ['AddAmount', 'LessAmount'], onDelete: function (i) { C.remove(i); }, onDblClick: function (i) { C.edit(i); } });
        C.grid = g;
        function draw() { g.draw(C.rows); if (cfg.onChanged) cfg.onChanged(); }
        C.draw = draw;
        function validation() {
            if (valI(ids.item) === 0) { box('Charges Item field is required'); focus(ids.item); return false; }
            if (valD(ids.less) === 0 && valD(ids.add) === 0) { box('Add Or Less Amount is required'); focus(ids.less); return false; }
            if (valD(ids.less) > 0 && valD(ids.add) > 0) { box("You Can Only Enter 'Add Or Less Amount'"); focus(ids.less); return false; }
            return true;
        }
        function row(id) { return { Id: id || 0, ChargesItemId: valI(ids.item), ChargesItem: text(ids.item), AddAmount: valD(ids.add), LessAmount: valD(ids.less), Remarks: val(ids.remarks) }; }
        C.reset = function () {
            C.updateIndex = -1;
            setVal(ids.item, 0); setText(ids.add, '0'); setText(ids.less, '0'); setText(ids.remarks, '');
            show(ids.addBtn, true); show(ids.update, false); show(ids.cancel, false);
        };
        C.addRow = function () { if (!validation()) return; C.rows.push(row(0)); draw(); C.reset(); focus(ids.item); };
        C.update = function () { if (!validation() || C.updateIndex < 0) return; C.rows[C.updateIndex] = row(netI(C.rows[C.updateIndex].Id)); draw(); C.reset(); focus(ids.item); };
        C.cancel = function () { C.reset(); };
        C.edit = function (i) {
            var r = C.rows[i]; if (!r) return;
            C.updateIndex = i;
            setVal(ids.item, r.ChargesItemId); setText(ids.add, fmt(r.AddAmount, 2)); setText(ids.less, fmt(r.LessAmount, 2)); setText(ids.remarks, r.Remarks);
            show(ids.addBtn, false); show(ids.update, true); show(ids.cancel, true);
            focus(ids.item);
        };
        C.remove = function (i) {
            if (C.updateIndex !== -1) { box('Reset  the Other Charges Detail first...'); return; }
            if (!C.rows[i]) return;
            if (netI(C.rows[i].Id) > 0 && !ask('Are you sure to Delete?')) return;
            C.rows.splice(i, 1); draw();
        };
        C.wire = function () {
            var e;
            if ((e = $id(ids.addBtn))) e.addEventListener('click', C.addRow);
            if ((e = $id(ids.update))) e.addEventListener('click', C.update);
            if ((e = $id(ids.cancel))) e.addEventListener('click', C.cancel);
            [ids.add, ids.less].forEach(function (id) { var t = $id(id); if (t) t.addEventListener('keypress', numericOnly); });
        };
        return C;
    }

    // ------------------------------------------------------------------------------ history detail grids of the contract forms

    var HIST_COLS = {
        detail: [{ key: 'ItemCode' }, { key: 'ItemName' }, { key: 'PackType' }, { key: 'CropYear' }, { key: 'QtyMTon', num: true, dec: 6 }, { key: 'PackSize' },
            { key: 'NoOfBags', num: true }, { key: 'Rate', num: true, dec: 4 }, { key: 'RateUOM' }, { key: 'Amount', num: true, dec: 4 }, { key: 'OtherItemAmount', num: true, dec: 4 },
            { key: 'CommodityDetail' }, { key: 'FarmingNTrade' }, { key: 'Remarks' }],
        payment: [{ key: 'PaymentTerm' }, { key: 'PrcntOfTotal', num: true, dec: 2 }, { key: 'FcyAmount', num: true, dec: 4 }, { key: 'Remarks' }],
        pm: [{ key: 'BrandCode' }, { key: 'BrandName' }, { key: 'BrandUom' }, { key: 'PackingType' }, { key: 'BrandOuterQty', num: true }, { key: 'BrandInnerQty', num: true },
            { key: 'PmItemCode' }, { key: 'PmItemName' }, { key: 'PmItemUom' }, { key: 'PmItemOuterQty', num: true }, { key: 'PmItemInnerQty', num: true }, { key: 'Rate', num: true }, { key: 'Amount', num: true }, { key: 'Remarks' }],
        other: [{ key: 'ItemName' }, { key: 'Qty', num: true }, { key: 'Rate', num: true }, { key: 'Amount', num: true }, { key: 'Remarks' }],
        charges: [{ key: 'ChargesItem' }, { key: 'AddAmount', num: true, dec: 2 }, { key: 'LessAmount', num: true, dec: 2 }, { key: 'Remarks' }]
    };

    global.ExportG = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, netB: netB, roundAway: roundAway, fmt: fmt, fmtFixed: fmtFixed,
        today: today, daysAgo: daysAgo, addDays: addDays, daysBetween: daysBetween, isoDate: isoDate, ddMMMyyyy: ddMMMyyyy, ddMMyyyyHm: ddMMyyyyHm, shortDate: shortDate, dateNull: dateNull,
        busy: busy, getJson: getJson, postJson: postJson, refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal, val: val, valI: valI, valD: valD,
        text: text, setText: setText, setEnabled: setEnabled, checked: checked, setChecked: setChecked, focus: focus, radio: radio, rowOf: rowOf, cancelWindow: cancelWindow, activeTag: activeTag,
        tabs: tabs, initFullscreen: initFullscreen, grid: grid, shortcuts: shortcuts, contractHistoryBody: contractHistoryBody,
        pmTab: pmTab, paymentTab: paymentTab, otherItemsTab: otherItemsTab, otherChargesTab: otherChargesTab, numericOnly: numericOnly, integerOnly: integerOnly, HIST_COLS: HIST_COLS
    };
})(window);
