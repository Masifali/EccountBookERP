/*
 * Account Reports (module 3) - shared helpers for the rechecked report pages (group F, 2026-09-30).
 * Talks only to /accounts/api/reports-desktop (AccountReportsDesktopRestController). Formats follow the
 * desktop's clsGlobalVariables.stringFormatboth / stringFormatsingle (CommonServices.GetDecimalConfiguration).
 */
/* jQuery 2.2.4 (the version these pages load) decides "is this a function?" through
   Object.prototype.toString, and an async function reports "[object AsyncFunction]", which 2.x does
   not know. So $(async function () {...}) was silently treated as a selector and the page's load
   handler NEVER ran: no context, no dropdowns, no default dates. Teach isFunction about it once. */
(function ($) {
    if (!$ || $.__asyncReadyFix) return;
    var orig = $.isFunction;
    $.isFunction = function (o) { return typeof o === 'function' || orig.call($, o); };
    $.__asyncReadyFix = true;
})(window.jQuery);

(function (w, $) {
    'use strict';
    const BASE = '/accounts/api/reports-desktop';
    let ctxPromise = null;
    let decimals = 2;

    function unwrap(res) {
        if (res && res.success === false) throw new Error(res.message || 'Error');
        return res && Object.prototype.hasOwnProperty.call(res, 'data') ? res.data : res;
    }
    function errText(xhr) {
        try { const j = xhr.responseJSON || JSON.parse(xhr.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return (xhr && xhr.status ? ('HTTP ' + xhr.status) : 'Request failed');
    }
    function get(path, params) {
        return new Promise(function (resolve, reject) {
            $.ajax({ url: BASE + path, type: 'GET', data: params || {}, dataType: 'json' })
                .done(function (r) { try { resolve(unwrap(r)); } catch (e) { reject(e); } })
                .fail(function (x) { reject(new Error(errText(x))); });
        });
    }
    function post(path, body) {
        return new Promise(function (resolve, reject) {
            $.ajax({ url: BASE + path, type: 'POST', contentType: 'application/json', data: JSON.stringify(body || {}), dataType: 'json' })
                .done(function (r) { try { resolve(unwrap(r)); } catch (e) { reject(e); } })
                .fail(function (x) { reject(new Error(errText(x))); });
        });
    }
    function context() {
        if (!ctxPromise) {
            ctxPromise = get('/context').then(function (c) {
                const d = parseInt(c && c.amountDecimals, 10);
                decimals = isNaN(d) || d < 0 ? 0 : Math.min(d, 4);
                return c;
            });
        }
        return ctxPromise;
    }
    function lookup(name, params) { return get('/lookups/' + name, params); }

    function num(v) { const n = parseFloat(v); return isNaN(n) ? 0 : n; }
    function grp(n, dec) { return Math.abs(n).toLocaleString('en-US', { minimumFractionDigits: dec, maximumFractionDigits: dec }); }
    /** "#,##0.dd;(0,0.dd); 0" */
    function both(v) { const n = num(v); if (n === 0) return '0'; return n < 0 ? '(' + grp(n, decimals) + ')' : grp(n, decimals); }
    /** "#,##0.dd" */
    function single(v) { const n = num(v); return (n < 0 ? '-' : '') + grp(n, decimals); }
    /** "#,#" - zero shows empty */
    function hash(v) { const n = Math.round(num(v)); if (n === 0) return ''; return (n < 0 ? '-' : '') + grp(n, 0); }
    /** "#,#;(#,#);0" */
    function hashBoth(v) { const n = Math.round(num(v)); if (n === 0) return '0'; return n < 0 ? '(' + grp(n, 0) + ')' : grp(n, 0); }
    const MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dmy(v) {
        if (!v) return '';
        const s = String(v).substring(0, 10);
        const m = /^(\d{4})-(\d{2})-(\d{2})$/.exec(s);
        if (!m) return String(v);
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1];
    }
    function esc(v) { return $('<div>').text(v == null ? '' : String(v)).html(); }
    function iso(d) {
        const y = d.getFullYear(), m = ('0' + (d.getMonth() + 1)).slice(-2), dd = ('0' + d.getDate()).slice(-2);
        return y + '-' + m + '-' + dd;
    }
    function today() { return iso(new Date()); }
    function addDays(n) { const d = new Date(); d.setDate(d.getDate() + n); return iso(d); }
    function monthStart() { const d = new Date(); return iso(new Date(d.getFullYear(), d.getMonth(), 1)); }
    function yearStart() { return new Date().getFullYear() + '-01-01'; }

    function fillSelect(sel, rows, valueKey, textKey, placeholder) {
        const $s = $(sel);
        $s.empty();
        if (placeholder !== null && placeholder !== undefined) $s.append($('<option>').val('').text(placeholder));
        (rows || []).forEach(function (r) { $s.append($('<option>').val(r[valueKey]).text(r[textKey] == null ? '' : r[textKey])); });
        $s.each(function () { combo(this); });
        return $s;
    }

    /* ---- searchable dropdowns (countx_prod_combo.js, 2026-09-30r) ------------------------------
       combo(el): a SINGLE select (not multiple, not data-dtcombo-skip) gets data-dtcombo="single" and is
       enhanced through window.DesktopCombo when that script is on the page; a no-op otherwise, so a page
       that does not load countx_prod_combo.js keeps its native select. fillSelect calls it, so every
       dropdown filled through AccF becomes searchable. The native <select> stays authoritative (.val(),
       change events); the wrap takes over the select's grid placement (grid-column, flex, display). */
    function combo(el) {
        /* 2026-10-02: a multiple select is enhanced only when it opts in to the checked mode (data-dtcombo-checked) */
        if (!el || el.tagName !== 'SELECT' || (el.multiple && !el.hasAttribute('data-dtcombo-checked')) || el.hasAttribute('data-dtcombo-skip')) return null;
        if (!el.hasAttribute('data-dtcombo')) el.setAttribute('data-dtcombo', 'single');
        const DC = w.DesktopCombo;
        if (!DC || typeof DC.enhance !== 'function') return null;
        const c = DC.enhance(el);
        const wrap = c && c.wrap ? c.wrap : (el.parentNode && el.parentNode.classList && el.parentNode.classList.contains('dtcombo-wrap') ? el.parentNode : null);
        if (wrap && !wrap.__accfPlaced) {
            wrap.__accfPlaced = true;
            ['gridColumn', 'gridRow', 'flex', 'minWidth', 'maxWidth', 'marginLeft', 'marginRight'].forEach(function (k) { if (el.style[k]) wrap.style[k] = el.style[k]; });
            if (el.style.display === 'none') wrap.style.display = 'none';
            if (el.style.color) wrap.style.color = '';
        }
        return c;
    }
    /** Enhance every single select under root (default: the whole document) that carries data-dtcombo. */
    function combos(root) {
        const r = root ? $(root) : $(document);
        let n = 0;
        r.find('select[data-dtcombo]').addBack('select[data-dtcombo]').each(function () { if (combo(this)) n++; });
        return n;
    }
    /** The element to show / hide / focus for a select: its combo wrap when enhanced, else the select. */
    function ctl(sel) {
        const $s = $(sel);
        const $w = $s.closest('.dtcombo-wrap');
        return $w.length ? $w : $s;
    }
    function focus(sel) {
        const $in = $(sel).closest('.dtcombo-wrap').find('.dtcombo-input');
        if ($in.length) $in.first().focus(); else $(sel).focus();
    }

    /** CommonServices.DateType(): the five rows every ledger form binds to its Date Type combo. */
    const DATE_TYPES = [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' },
        { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }];
    /** CmbDateType_ValueChanged (GeneralLedger.cs / CustomerLedger.cs): 1 from=today; 2 from=today-7; 3 from=1st of month,
        to=today; 4 from=1 Jan, to=today; 5 from=ActiveYr.Start_Period. The To date is left alone for 1, 2 and 5. */
    function applyDateType(id, fromSel, toSel, ctx) {
        id = parseInt(id, 10);
        if (id === 1) $(fromSel).val(today());
        else if (id === 2) $(fromSel).val(addDays(-7));
        else if (id === 3) { $(fromSel).val(monthStart()); $(toSel).val(today()); }
        else if (id === 4) { $(fromSel).val(yearStart()); $(toSel).val(today()); }
        else if (id === 5 && ctx && ctx.activeYear && ctx.activeYear.startPeriod) $(fromSel).val(ctx.activeYear.startPeriod);
    }

    /* Selects that carry data-dtcombo in the markup are enhanced by countx_prod_combo.js at DOMContentLoaded;
       run after it so the wrap also takes the select's grid placement. */
    $(function () { try { combos(); } catch (e) { /* never block a page load */ } });
    function selectedIds(sel) {
        const v = $(sel).val();
        if (!v) return '';
        return (Array.isArray(v) ? v : [v]).filter(function (x) { return x !== '' && x != null; }).join(',');
    }
    function fmtCell(col, v) {
        switch (col.type) {
            case 'both': return both(v);
            case 'single': return single(v);
            case 'hash': return hash(v);
            case 'hashboth': return hashBoth(v);
            case 'date': return dmy(v);
            case 'bool': return (v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true') ? '&#10004;' : '';
            default: return esc(v);
        }
    }
    function alignOf(col) {
        if (col.align) return col.align;
        return (col.type === 'both' || col.type === 'single' || col.type === 'hash' || col.type === 'hashboth') ? 'r' : '';
    }

    /**
     * Janus-style grid: visible columns only, optional GroupBy with group header rows (+ group totals),
     * SUM footer for columns flagged sum:true, link columns via col.link(row).
     */
    function renderGrid(opts) {
        const $t = $(opts.table);
        const cols = (opts.columns || []).filter(function (c) { return c.visible !== false; });
        const rows = opts.rows || [];
        const $thead = $('<thead>'), $tr = $('<tr>');
        cols.forEach(function (c) { $tr.append($('<th>').text(c.caption == null ? c.key : c.caption).css('min-width', c.width ? c.width + 'px' : '')); });
        $thead.append($tr);
        const $tbody = $('<tbody>');
        const totals = {};
        function addTotals(acc, r) { cols.forEach(function (c) { if (c.sum) acc[c.key] = (acc[c.key] || 0) + num(r[c.key]); }); }
        function rowEl(r) {
            const $r = $('<tr>');
            if (opts.rowClass) $r.addClass(opts.rowClass(r) || '');
            cols.forEach(function (c) {
                const $td = $('<td>').addClass(alignOf(c));
                if (c.wrap) $td.addClass('wrap');
                const html = c.render ? c.render(r) : fmtCell(c, r[c.key]);
                if (c.link && html !== '') {
                    const $a = $('<a class="lnk">').html(html);
                    $a.on('click', function (e) { e.preventDefault(); c.link(r); });
                    $td.append($a);
                } else {
                    $td.html(html);
                }
                if (c.style) $td.attr('style', typeof c.style === 'function' ? c.style(r) : c.style);
                $r.append($td);
            });
            return $r;
        }
        if (!rows.length) {
            $tbody.append($('<tr>').append($('<td>').attr('colspan', cols.length).addClass('c').css('padding', '12px').text(opts.emptyText || 'No record found.')));
        } else if (opts.groupBy) {
            const order = [], groups = {};
            rows.forEach(function (r) { const g = r[opts.groupBy] == null ? '' : String(r[opts.groupBy]); if (!groups[g]) { groups[g] = []; order.push(g); } groups[g].push(r); });
            order.forEach(function (g) {
                const gRows = groups[g];
                const gt = {};
                gRows.forEach(function (r) { addTotals(gt, r); addTotals(totals, r); });
                const $h = $('<tr class="grp">').append($('<td>').attr('colspan', cols.length)
                    .html('<span class="tree-t">&minus;</span> ' + esc(opts.groupCaption ? opts.groupCaption + ': ' : '') + esc(g) + ' (' + gRows.length + ')'));
                const bodyRows = gRows.map(rowEl);
                let $gt = null;
                if (opts.groupTotals) {
                    $gt = $('<tr class="grp-total">');
                    cols.forEach(function (c, idx) {
                        const $td = $('<td>').addClass(alignOf(c));
                        if (c.sum) $td.html(fmtCell(c.totalType ? { type: c.totalType } : c, gt[c.key]));
                        else if (idx === 0) $td.text('Total');
                        $gt.append($td);
                    });
                }
                $h.on('click', function () {
                    const collapsed = $h.data('collapsed') === true;
                    bodyRows.forEach(function ($x) { $x.toggle(collapsed); });
                    if ($gt) $gt.toggle(collapsed);
                    $h.data('collapsed', !collapsed);
                    $h.find('.tree-t').html(collapsed ? '&minus;' : '+');
                });
                $tbody.append($h);
                bodyRows.forEach(function ($x) { $tbody.append($x); });
                if ($gt) $tbody.append($gt);
            });
        } else {
            rows.forEach(function (r) { addTotals(totals, r); $tbody.append(rowEl(r)); });
        }
        const $tfoot = $('<tfoot>');
        if (cols.some(function (c) { return c.sum; })) {
            const $f = $('<tr>');
            cols.forEach(function (c, idx) {
                const $td = $('<td>').addClass(alignOf(c));
                if (c.sum) $td.html(fmtCell(c.totalType ? { type: c.totalType } : c, totals[c.key] || 0));
                else if (idx === 0) $td.text('Total');
                $f.append($td);
            });
            $tfoot.append($f);
        }
        $t.empty().append($thead, $tbody, $tfoot);
        return totals;
    }

    function busy(btn, on) {
        const $b = $(btn);
        if (on) { $b.data('html', $b.html()).prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> Loading...'); }
        else { $b.prop('disabled', false).html($b.data('html') || $b.html()); }
    }

    /** CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, From, To, SubsidiaryAccountId, BranchId). */
    function openLedger(accountId, from, to, subsidiaryAccountId, branchId) {
        const q = new URLSearchParams();
        q.set('accountId', accountId);
        if (from) q.set('fromDate', from);
        if (to) q.set('toDate', to);
        if (subsidiaryAccountId) q.set('subsidiaryAccountId', subsidiaryAccountId);
        if (branchId) q.set('branchId', branchId);
        w.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
    }

    /** CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo, BaseDocumentTypeId) via DocLink. */
    function editLinked(documentTypeId, id, srNo, baseDocumentTypeId) {
        if (!w.DocLink) { alert('Document links are not available on this page.'); return; }
        w.DocLink.open(documentTypeId, id, { srNo: srNo, baseDocumentTypeId: baseDocumentTypeId || 0 });
    }

    w.AccF = {
        get: get, post: post, context: context, lookup: lookup,
        both: both, single: single, hash: hash, hashBoth: hashBoth, dmy: dmy, esc: esc, num: num,
        today: today, addDays: addDays, monthStart: monthStart, yearStart: yearStart, iso: iso,
        fillSelect: fillSelect, combo: combo, combos: combos, ctl: ctl, focus: focus, DATE_TYPES: DATE_TYPES, applyDateType: applyDateType,
        selectedIds: selectedIds, renderGrid: renderGrid, busy: busy, openLedger: openLedger, editLinked: editLinked,
        decimals: function () { return decimals; }
    };
})(window, jQuery);
