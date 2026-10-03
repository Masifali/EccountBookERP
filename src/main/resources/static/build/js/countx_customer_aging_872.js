/* ============================================================================================
 * Screen 872 "Customer Aging Report" - Architecture.WinApp.Account_Reports.frmReceivableAgingDocumentWise
 * Page: templates/accounts/reports/receivables_aging_new.html (routes /accounts/reports/customer-aging,
 * /accounts/reports/receivables-aging-new). 2026-10-03 group P-CUSTOMER-AGING, ported from the sibling 873 page
 * (countx_supplier_aging_873.js) with the 872 differences - every desktop handler, in desktop order:
 *   ctor InitializeComponentMethod  -> lists bound together: Customer = CoaAllocationAccountTitleByAccountTypeIds("3","",0,"2"),
 *                                      Custom Group = CustomeGroupsDefine(1), Party Group =
 *                                      CustomerGroup.GetSupplierCustomerGroupFromInventoryStockEvaluation; then
 *                                      txtAgingDays = "30", datAsOnDate.Focus(); a failure -> "Error occurred during database call."
 *   btnShow_Click / GridFill        -> usp_CustomerAgingReport_DocumentWise (AgingDays/AccountId/PartyGroupId/CustomGruopId only
 *                                      when non-zero - the server builds the call); no rows -> ClearStructure
 *                                      DataTable: Id, DocumentTypeId, DocumentType, DocNo (int <- InvoiceNo), DueDate (<- InvoiceDueDate),
 *                                      ParentAccountCode, ParentAccount, ClassName, AccountId, AccountCode, PartyName, 4 intervals
 *                                      (captions from the proc), DueAmount, Remarks - NO ManualNo / InvoiceDate / PartyBillDate (873 has them)
 *   DataGridHistorySetting          -> Id/AccountId/DocumentTypeId/ParentAccountCode/ParentAccount/ClassName hidden, grouped by
 *                                      PartyName (hidden when grouped), RowHeight 30, AccountCode link, Remarks 350 word-wrap,
 *                                      GridWrappingAndColumnSettings(…, NegativeFormat:true): DocNo Int32 -> centred
 *   btnNew_Click (Reset)            -> txtAgingDays = "", datAsOnDate.Focus(), grid ClearStructure (dtRegister is NOT cleared)
 *   btnRefresh_Click                -> BindCustomerAccountCombo(CustomerAccountData()) - the Customer list only
 *   btnPrint_Click                  -> dtRegister empty -> "Record Not Found For Display"; else 126-CustomerAgingReport_DocumentWise.rpt
 *                                      over dtRegister (the last Show) -> Jasper print layer with the last Show's arguments
 *   btnShortcut_Click / Ctrl+Alt    -> ShortCutKeyPopUp (MakeShortCutKeys rows)
 *   txtAgingDays KeyPress           -> OnlytextNumberFunction (digits + control keys only)
 *   btnShow_Leave                   -> focus went to the grid -> datAsOnDate.Focus()
 *   form KeyDown (KeyPreview)       -> Enter=Tab, Ctrl+S Show, Ctrl+N Reset, Ctrl+R Refresh, Ctrl+F5/Ctrl+Up date, Ctrl+P Print,
 *                                      Ctrl+Down grid, Ctrl+E/Esc Close, Ctrl+Alt shortcut keys
 *   DataGridHistory LinkClicked / KeyDown Ctrl+Space -> GoToGeneralLedgerFromLinkedEvent(AccountId, ActiveYr.Start_Period, AsOnDate)
 *   cmbActivity_TextChanged, DataGridHistory_ColumnButtonClick -> empty bodies on the desktop (nothing to port)
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var el = function (id) { return d.getElementById(id); };
    var API = '/api/accounts/document-aging/customer';
    var RPT = '126-CustomerAgingReport_DocumentWise.rpt';

    /* CmbCustomerAccounts: DDL.BindDDL(dt, "Id", "AccountTitle", "Customer") - column 0 (Id) hidden, column 1 captioned
       "Customer", every other column of the proc's row (AccountCode) still shown. */
    if (w.DesktopCombo) w.DesktopCombo.define('cusAging872', [
        { caption: 'Customer', flex: 3 },
        { caption: 'AccountCode', flex: 2, key: 'code' }
    ]);

    var lookup = { amountDecimals: 0, year: null };
    var dtRegister = [];          // the last Show's rows (what Print hands to the .rpt)
    var lastArgs = null;          // the last Show's arguments
    var gridRows = [];            // rows the grid shows (cleared by Reset / ClearStructure)
    var columns = null;           // null = ClearStructure
    var filters = {}, sortKey = '', sortDir = 1, collapsed = {}, selected = -1, flat = [];
    var seq = 0, aborter = null;

    // ------------------------------------------------------------------ helpers
    function busy(btn, on) {
        btn.disabled = !!on;
        btn.classList.toggle('busy', !!on);
        if (on) btn.setAttribute('aria-busy', 'true'); else btn.removeAttribute('aria-busy');
    }
    function getJson(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opts || {}))
            .then(function (r) {
                if (!r.ok) return r.text().then(function (t) {
                    var m = t; try { var j = JSON.parse(t); m = j.message || j.error || t; } catch (e) { }
                    throw new Error(m || ('Request failed (' + r.status + ')'));
                });
                return r.json();
            });
    }
    function today() {
        var t = new Date();
        return t.getFullYear() + '-' + ('0' + (t.getMonth() + 1)).slice(-2) + '-' + ('0' + t.getDate()).slice(-2);
    }
    function ci(row, key) {
        if (!row) return undefined;
        if (key in row) return row[key];
        var low = key.toLowerCase();
        for (var k in row) if (k.toLowerCase() === low) return row[k];
        return undefined;
    }

    /* .NET custom numeric formats used by GridColumnSettings with NegativeFormat = true:
         Amount columns  : clsGlobalVariables.stringFormatboth = "#,##0.<n zeros>;(0,0.<n zeros>); 0"
         other decimals  : "#,##0.<n #>;(0,0.<n #>); 0"  with n = DefaultNoofDecimalPointsForAmount (0 -> 2)        */
    function group3(intPart, minDigits) {
        while (intPart.length < minDigits) intPart = '0' + intPart;
        return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    function netFormat(value, places, optional) {
        var n = Number(value);
        if (!isFinite(n)) return value == null ? '' : String(value);
        var abs = Math.abs(n), s = abs.toFixed(places), parts = s.split('.');
        var frac = parts[1] || '';
        if (optional) frac = frac.replace(/0+$/, '');
        if (Number(s) === 0) return '0';                                     // third section " 0"
        if (n < 0) return '(' + group3(parts[0], 2) + (frac ? '.' + frac : '') + ')';
        return group3(parts[0], 1) + (frac ? '.' + frac : '');
    }
    function fmtCell(col, v) {
        if (v === null || v === undefined || v === '') return '';
        if (col.type === 'amount') return netFormat(v, lookup.amountDecimals || 0, false);
        if (col.type === 'decimal') return netFormat(v, lookup.amountDecimals || 2, true);
        if (col.type === 'date') {
            var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
            return m ? m[3] + '-' + ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][+m[2] - 1] + '-' + m[1] : String(v);
        }
        return String(v);
    }
    /* exact sums of the procedure's DECIMAL strings (scaled integers, no float drift) */
    function sumDec(values) {
        var SCALE = 6, total = BigInt(0);
        values.forEach(function (v) {
            if (v === null || v === undefined || v === '') return;
            var s = String(v).trim(), neg = s.charAt(0) === '-'; if (neg || s.charAt(0) === '+') s = s.slice(1);
            var p = s.split('.'), frac = ((p[1] || '') + '000000').slice(0, SCALE);
            if (!/^\d*$/.test(p[0]) || !/^\d*$/.test(frac)) return;
            var b = BigInt((p[0] || '0') + frac); total += neg ? -b : b;
        });
        var negT = total < 0, a = (negT ? -total : total).toString().padStart(SCALE + 1, '0');
        return (negT ? '-' : '') + a.slice(0, -SCALE) + '.' + a.slice(-SCALE);
    }

    // ------------------------------------------------------------------ lists
    function fill(sel, rows, valueKey, textKey, extra) {
        var old = sel.value;
        while (sel.options.length) sel.remove(0);
        sel.add(new Option('', ''));                          // AllowNull, ZeroIndex:false - nothing chosen until the user picks
        (rows || []).forEach(function (r) {
            var o = new Option(ci(r, textKey) == null ? '' : String(ci(r, textKey)), String(ci(r, valueKey)));
            if (extra) Object.keys(extra).forEach(function (a) { var v = ci(r, extra[a]); if (v != null) o.setAttribute('data-' + a, v); });
            sel.add(o);
        });
        sel.value = old;
        if (sel.value !== old) sel.value = '';
    }
    function bindAccounts(rows) { fill(el('account'), rows, 'Id', 'AccountTitle', { code: 'AccountCode' }); }

    function loadLists() {
        return getJson(API + '/lookups').then(function (data) {
            lookup = data || lookup;
            bindAccounts(lookup.accounts);                                          // BindCustomerAccountCombo
            fill(el('custom'), lookup.customGroups, 'Id', 'AcLookUpsDescription');   // BindCustomGroups
            fill(el('party'), lookup.partyGroups, 'Id', 'Description');             // BindCustomerGroupData
            el('days').value = '30';
            el('asOnDate').focus();
        }).catch(function (e) {
            if (w.console) console.error(e);
            alert('Error occurred during database call.');
        });
    }

    function refresh() {                                       // btnRefresh_Click
        var b = el('refresh'); if (b.disabled) return;
        busy(b, true);
        getJson(API + '/accounts').then(bindAccounts)
            .catch(function (e) { alert(e.message); })
            .then(function () { busy(b, false); });
    }

    // ------------------------------------------------------------------ Show
    function intArg(v) { var n = parseInt(String(v || '').trim(), 10); return isNaN(n) ? 0 : n; }   // Conversion.ToInt

    function show() {                                          // btnShow_Click -> GridFill
        var b = el('show'); if (b.disabled) return;
        var args = {
            asOnDate: el('asOnDate').value || today(),
            agingDays: intArg(el('days').value),
            accountId: intArg(el('account').value),
            customGroupId: intArg(el('custom').value),
            partyGroupId: intArg(el('party').value)
        };
        busy(b, true);
        var token = ++seq;
        if (aborter) aborter.abort();
        aborter = w.AbortController ? new AbortController() : null;
        dtRegister = []; gridRows = []; columns = null; render();                // dtGridForLayout / dtRegister .Rows.Clear()
        var q = new URLSearchParams({ asOnDate: args.asOnDate, agingDays: args.agingDays, accountId: args.accountId,
                                      customGroupId: args.customGroupId, partyGroupId: args.partyGroupId });
        getJson(API + '?' + q.toString(), aborter ? { signal: aborter.signal } : undefined).then(function (rows) {
            if (token !== seq) return;
            dtRegister = rows || [];
            lastArgs = args;
            if (!dtRegister.length) { columns = null; gridRows = []; render(); return; }      // DataGridHistory.ClearStructure()
            buildColumns(dtRegister[0]);
            gridRows = dtRegister;
            filters = {}; sortKey = ''; collapsed = {}; selected = -1;
            render();
        }).catch(function (e) {
            if (e && e.name === 'AbortError') return;
            alert(e.message);
        }).then(function () { if (token === seq) busy(b, false); });
    }

    /* GridFill's DataTable (column order) + DataGridHistorySetting (hidden, captions, widths, types). */
    function buildColumns(r0) {
        var c1 = String(ci(r0, 'FirstIntervalCaption') || ''), c2 = String(ci(r0, 'SecondIntervalCaption') || ''),
            c3 = String(ci(r0, 'ThirdIntervalCaption') || ''), c4 = String(ci(r0, 'AboveIntervalCaption') || '');
        columns = [
            { key: 'DocumentType', caption: 'DocumentType', width: 70 },
            { key: 'InvoiceNo', caption: 'DocNo', width: 60, type: 'int' },                 // typeof(int), DocNoConstant
            { key: 'InvoiceDueDate', caption: 'DueDate', width: 85, type: 'date' },         // DateConstant + 12
            { key: 'AccountCode', caption: 'AccountCode', width: 100, link: true },         // ColumnType Link
            { key: '1stInterval', caption: c1, width: 90, type: 'decimal' },
            { key: '2ndInterval', caption: c2, width: 90, type: 'decimal' },
            { key: '3rdInterval', caption: c3, width: 90, type: 'decimal' },
            { key: 'Above', caption: c4, width: 90, type: 'decimal' },
            { key: 'DueAmount', caption: 'DueAmount', width: 90, type: 'amount' },
            { key: 'Remarks', caption: 'Remarks', width: 350, wrap: true }
        ];
    }

    // ------------------------------------------------------------------ grid
    function isNum(c) { return c.type === 'amount' || c.type === 'decimal'; }
    function visibleRows() {
        var keys = Object.keys(filters).filter(function (k) { return filters[k]; });
        var out = gridRows.filter(function (r) {
            return keys.every(function (k) {
                var c = columns.filter(function (x) { return x.key === k; })[0];
                return fmtCell(c, ci(r, k)).toLowerCase().indexOf(filters[k].toLowerCase()) >= 0;   // Contains
            });
        });
        if (sortKey) {
            var c = columns.filter(function (x) { return x.key === sortKey; })[0];
            out = out.slice().sort(function (a, b) {
                var x = ci(a, sortKey), y = ci(b, sortKey);
                if (isNum(c) || c.type === 'int') return sortDir * (Number(x || 0) - Number(y || 0));
                return sortDir * String(x == null ? '' : x).localeCompare(String(y == null ? '' : y));
            });
        }
        return out;
    }
    function totalsRow(cls, rows, tag) {
        var tr = d.createElement('tr'); tr.className = cls;
        columns.forEach(function (c, i) {
            var td = d.createElement('td');
            if (isNum(c)) { td.className = 'num'; td.textContent = fmtCell(c, sumDec(rows.map(function (r) { return ci(r, c.key); }))); }
            else if (i === 0 && tag) td.textContent = tag;
            tr.appendChild(td);
        });
        return tr;
    }
    function render() {
        var t = el('results'), cg = t.querySelector('colgroup'), th = t.tHead, tb = t.tBodies[0], tf = t.tFoot;
        cg.innerHTML = ''; th.innerHTML = ''; tb.innerHTML = ''; tf.innerHTML = ''; flat = [];
        el('nav').hidden = !columns;
        if (!columns) { navText(); return; }
        columns.forEach(function (c) { var col = d.createElement('col'); col.style.width = c.width + 'px'; cg.appendChild(col); });
        var hr = th.insertRow();
        columns.forEach(function (c) {
            var h = d.createElement('th'); h.textContent = c.caption; h.title = c.caption;
            if (sortKey === c.key) { var s = d.createElement('span'); s.className = 'sort'; s.textContent = sortDir > 0 ? '▲' : '▼'; h.appendChild(s); }
            h.onclick = function () { sortDir = sortKey === c.key ? -sortDir : 1; sortKey = c.key; render(); };
            hr.appendChild(h);
        });
        var fr = th.insertRow(); fr.className = 'flt';                            // FilterMode Automatic, DynamicFiltering
        columns.forEach(function (c) {
            var td = fr.insertCell(), inp = d.createElement('input');
            inp.value = filters[c.key] || ''; inp.setAttribute('aria-label', 'Filter ' + c.caption); inp.dataset.key = c.key;
            inp.oninput = function () {
                var pos = inp.selectionStart; filters[c.key] = inp.value; selected = -1; render();
                var again = th.querySelector('input[data-key="' + c.key + '"]'); if (again) { again.focus(); again.setSelectionRange(pos, pos); }
            };
            td.appendChild(inp);
        });
        var rows = visibleRows(), groups = {}, order = [];
        rows.forEach(function (r) { var g = String(ci(r, 'PartyName') == null ? '' : ci(r, 'PartyName')); if (!groups[g]) { groups[g] = []; order.push(g); } groups[g].push(r); });
        order.sort(function (a, b) { return a.localeCompare(b); });                // Groups.Add("PartyName") - ascending
        order.forEach(function (g) {
            var gr = tb.insertRow(); gr.className = 'grp';
            var gc = gr.insertCell(); gc.colSpan = columns.length;
            var tg = d.createElement('span'); tg.className = 'tg'; tg.textContent = collapsed[g] ? '+' : '−';
            gc.appendChild(tg); gc.appendChild(d.createTextNode('PartyName: ' + g));
            gr.onclick = function () { collapsed[g] = !collapsed[g]; render(); };
            if (!collapsed[g]) groups[g].forEach(function (r) {
                var tr = tb.insertRow(), idx = flat.length; tr.className = 'r'; tr.tabIndex = -1; flat.push({ row: r, tr: tr });
                if (idx === selected) tr.classList.add('sel');
                columns.forEach(function (c) {
                    var td = tr.insertCell(), text = fmtCell(c, ci(r, c.key));
                    if (c.link) { var a = d.createElement('a'); a.className = 'lnk'; a.textContent = text; a.href = '#';
                                  a.onclick = function (e) { e.preventDefault(); select(idx); ledger(r); }; td.appendChild(a); }
                    else td.textContent = text;
                    if (isNum(c)) td.className = 'num'; else if (c.type === 'int') td.className = 'ctr'; else if (c.wrap) td.className = 'wrap';
                    if (!c.wrap) td.title = text;
                });
                tr.onmousedown = function () { select(idx); };
            });
            tb.appendChild(totalsRow('gtot', groups[g], ''));                      // GroupTotals Always
        });
        tf.appendChild(totalsRow('tot', rows, 'Total'));                           // TotalRow
        if (selected >= flat.length) selected = flat.length ? flat.length - 1 : -1;
        navText();
    }
    function select(i) {
        if (i < 0 || i >= flat.length) return;
        if (selected >= 0 && flat[selected]) flat[selected].tr.classList.remove('sel');
        selected = i; flat[i].tr.classList.add('sel');
        var g = el('grid'), tr = flat[i].tr;
        var top = tr.offsetTop - 54, bottom = tr.offsetTop + tr.offsetHeight - g.clientHeight + 24;
        if (g.scrollTop > top) g.scrollTop = top; else if (g.scrollTop < bottom) g.scrollTop = bottom;
        navText();
    }
    function navText() {
        el('navText').textContent = columns ? ('Record: ' + (selected >= 0 ? selected + 1 : 0) + ' of ' + flat.length) : '';
    }
    /* CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, ActiveYr.Start_Period, datAsOnDate.Value) */
    function ledger(r) {
        var q = new URLSearchParams();
        q.set('accountId', ci(r, 'AccountId'));
        var start = lookup.year ? ci(lookup.year, 'Start_Period') : null;
        if (start) q.set('fromDate', String(start).slice(0, 10));
        q.set('toDate', el('asOnDate').value || today());
        w.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
    }

    // ------------------------------------------------------------------ Reset / Print / shortcuts
    function reset() {                                         // btnNew_Click
        ++seq; if (aborter) aborter.abort(); busy(el('show'), false);
        el('days').value = '';
        el('asOnDate').focus();
        columns = null; gridRows = []; filters = {}; sortKey = ''; selected = -1; render();
    }
    function print() {                                         // btnPrint_Click
        var b = el('print'); if (b.disabled) return;
        if (!dtRegister.length || !lastArgs) { alert('Record Not Found For Display'); return; }
        var a = { asOnDate: lastArgs.asOnDate };               // ReportsParameters of GridFill; zero ids are left out (BLL guards)
        if (lastArgs.agingDays) a.agingDays = lastArgs.agingDays;
        if (lastArgs.accountId) a.accountId = lastArgs.accountId;
        if (lastArgs.partyGroupId) a.customerGroupId = lastArgs.partyGroupId;
        if (lastArgs.customGroupId) a.customGroupId = lastArgs.customGroupId;
        if (typeof w.printRpt !== 'function') { alert('Print layer not loaded.'); return; }
        busy(b, true);
        Promise.resolve(w.printRpt(RPT, a)).catch(function (e) { alert(e.message); }).then(function () { busy(b, false); });
    }
    var SHORTCUTS = [                                          // MakeShortCutKeys
        ['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus On As On Date '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On As On Date in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
    ];
    function shortcutKeys() {
        var dlg = el('shortcutDialog'); if (dlg.open) return;
        var tb = el('shortcutRows'); tb.innerHTML = '';
        SHORTCUTS.forEach(function (s) { var tr = tb.insertRow(); tr.insertCell().textContent = s[0]; tr.insertCell().textContent = s[1]; });
        if (dlg.showModal) dlg.showModal(); else dlg.setAttribute('open', '');
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('refresh').addEventListener('click', refresh);
    el('reset').addEventListener('click', reset);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', shortcutKeys);
    el('closeShortcuts').addEventListener('click', function () { el('shortcutDialog').close(); });
    /* txtAgingDays.KeyPress = OnlytextNumberFunction: digits and control keys only */
    el('days').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
    el('days').addEventListener('input', function () { var v = this.value.replace(/\D/g, ''); if (v !== this.value) this.value = v; });
    /* btnShow_Leave: if the grid takes the focus, the focus goes back to As On Date */
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) el('asOnDate').focus(); });
    el('nav').addEventListener('click', function (e) {
        var b = e.target.closest('[data-nav]'); if (!b || !flat.length) return;
        var n = b.getAttribute('data-nav');
        select(n === 'first' ? 0 : n === 'last' ? flat.length - 1 : n === 'prev' ? Math.max(0, selected - 1) : Math.min(flat.length - 1, selected + 1));
    });
    /* DataGridHistory_KeyDown: Ctrl+Space on a record row -> General Ledger; arrows move the current row */
    el('grid').addEventListener('keydown', function (e) {
        if (e.target.tagName === 'INPUT') return;
        if (e.ctrlKey && (e.code === 'Space' || e.key === ' ')) { e.preventDefault(); if (selected >= 0 && flat[selected]) ledger(flat[selected].row); return; }
        if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
            if (e.ctrlKey) return;
            e.preventDefault(); if (!flat.length) return;
            select(selected < 0 ? 0 : Math.max(0, Math.min(flat.length - 1, selected + (e.key === 'ArrowDown' ? 1 : -1))));
        }
    });

    function comboOpen() { var p = d.querySelector('.dtcombo-pop'); return !!Array.prototype.some.call(d.querySelectorAll('.dtcombo-pop'), function (x) { return x.style.display === 'block'; }) && !!p; }
    function filterFocusables() {
        return ['asOnDate', 'account', 'custom', 'party', 'days', 'show'].map(function (id) {
            var x = el(id); if (x.tagName === 'SELECT') { var wrap = x.closest('.dtcombo-wrap'); return wrap ? wrap.querySelector('.dtcombo-input') : x; } return x;
        });
    }
    /* frmEvaulationDetailSalesReports_KeyDown (KeyPreview = true) */
    d.addEventListener('keydown', function (e) {
        var dlg = el('shortcutDialog');
        if (dlg.open) return;
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); shortcutKeys(); return; }
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !comboOpen()) {         // SendKeys "{TAB}"
            var list = filterFocusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 's') { e.preventDefault(); show(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); el('asOnDate').focus(); }
        else if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'arrowdown') { e.preventDefault(); el('grid').focus(); if (selected < 0 && flat.length) select(0); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
    });

    // ------------------------------------------------------------------ start
    el('asOnDate').value = today();                            // DateTimePicker default = Now
    render();
    loadLists();
}(window, document));
