'use strict';
/* Payables Report Invoice Wise - frmPayablesReportInvoiceWise.cs (screen 909).
   Lookups: Usp_AllComboAgainstPurchaseInvoice (Activity Supplier / CommissionAgent, ZeroIndex rows),
   AcLookUps type 1 (Custom Group), ReadAllAccountGroup level 3 type 3 classes 2,3 (Parent Account).
   GridFill -> usp_PayablesReportInvoiceWise: dates only when ticked, other filters only when non-zero,
   ActionId 0/1/2 from All/Paid/Balance (Balance is the designer default).
   Grid: grouped by DocumentType, GroupTotals=Always, TotalRow; decimal columns whose name contains
   "Amount" plus ItemQty/BillWeight/StockWeight are summed, AvgRate is not (GridColumnSettings). */
(() => {
    const el = id => document.getElementById(id);
    let rows = [], lookup = {}, busy = false, selected = null;
    const filters = new Map();
    const columns = [['DocDate', 'Doc Date', 80, 'date'], ['DocNo', 'Doc No', 70, 'link'], ['ManualBillNo', 'Manual Bill No', 90], ['SupplierName', 'Party Name', 200, 'link'],
        ['CommissionAgent', 'Commission Agent', 180], ['VehicleNos', 'Vehicle Nos', 150], ['ItemName', 'Item Name', 170], ['ItemQty', 'Item Qty', 80, 'sum'],
        ['BillWeight', 'Bill Weight', 100, 'sum'], ['StockWeight', 'Stock Weight', 100, 'sum'], ['AvgRate', 'Avg Rate', 80, 'rate'], ['BillAmount', 'Bill Amount', 110, 'sum'],
        ['PaidAmount', 'Paid Amount', 110, 'sum'], ['UnPaidAmount', 'Balance', 110, 'sum'], ['DueDays', 'Due Days', 60, 'int'], ['DueDate', 'Due Date', 80, 'date'],
        ['CurrentDate', 'Current Date', 80, 'date'], ['DaysUptoNow', 'Days Up To Now', 60, 'int'], ['OverDueBy', 'Over Due By', 60, 'int']];
    const status = t => el('status').textContent = t;
    const localDate = d => [d.getFullYear(), String(d.getMonth() + 1).padStart(2, '0'), String(d.getDate()).padStart(2, '0')].join('-');
    const shortDate = v => { const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v ?? '')); return m ? m[3] + '-' + ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][+m[2] - 1] + '-' + m[1].slice(-2) : String(v ?? ''); };
    const value = (row, key) => row[Object.keys(row).find(k => k.toLowerCase() === key.toLowerCase())] ?? '';
    async function request(url) {
        const r = await fetch(url, {headers: {Accept: 'application/json'}});
        if (!r.ok || !r.headers.get('content-type')?.includes('application/json')) throw Error(r.status === 401 || r.redirected ? 'Please sign in again.' : 'Unable to load report data (' + r.status + ').');
        return r.json();
    }
    async function run(message, work) {
        if (busy) return; busy = true;
        const buttons = [...document.querySelectorAll('nav button,#show')]; buttons.forEach(b => b.disabled = true); status(message);
        try { await work(); } catch (e) { status(e.message); } finally { buttons.forEach(b => b.disabled = false); busy = false; }
    }
    function bind(id, data, key, label, zeroIndex) {
        const select = el(id); select.replaceChildren(new Option('', zeroIndex ? '0' : ''));
        data.forEach(r => select.add(new Option(value(r, label), value(r, key))));
        $(select).val(zeroIndex ? '0' : '').trigger('change.select2');
    }
    function bindParties() {
        bind('partyId', (lookup.parties || []).filter(r => value(r, 'Activity') === 'Supplier'), 'Id', 'ReferenceName', true);
        bind('agentId', (lookup.parties || []).filter(r => value(r, 'Activity') === 'CommissionAgent'), 'Id', 'ReferenceName', true);
    }
    async function loadLookups(all) {
        lookup = await request('/api/accounts/payables-invoice-wise/lookups');
        bindParties();
        if (all) { bind('customGroup', lookup.groups || [], 'Id', 'AcLookUpsDescription'); bind('parentAccount', lookup.parents || [], 'Id', 'AccountTitle'); }
        status('');
    }
    function ledger(r) {
        /* PartyName link: GoToGeneralLedgerFromLinkedEvent(SupplierGlId, From ticked ? From : ActiveYr.Start_Period, To ticked ? To : Now) */
        const start = lookup.year && lookup.year.Start_Period ? String(lookup.year.Start_Period).slice(0, 10) : '';
        const p = new URLSearchParams({accountId: value(r, 'SupplierGlAccountId'), fromDate: el('useFrom').checked ? el('fromDate').value : start, toDate: el('useTo').checked ? el('toDate').value : localDate(new Date())});
        return '/accounts/reports/general-ledger?' + p;
    }
    const amount = (v, key) => ReportDecimal.format(v === '' || v == null ? '0' : v, key === 'AvgRate' ? 2 : 2, true);
    function render() {
        const t = el('results'); t.tHead.replaceChildren(); t.tBodies[0].replaceChildren(); t.tFoot.replaceChildren();
        const head = t.tHead.insertRow(), filterRow = t.tHead.insertRow();
        columns.forEach(([key, label, width], index) => {
            const th = document.createElement('th'); th.textContent = label; th.style.width = th.style.minWidth = width + 'px'; head.append(th);
            const cell = document.createElement('th'); cell.style.top = '38px'; cell.style.height = '26px';
            const input = document.createElement('input'); input.style.cssText = 'width:100%;min-width:0;height:22px'; input.setAttribute('aria-label', 'Filter ' + label); input.value = filters.get(key) || '';
            input.oninput = () => { const caret = input.selectionStart; filters.set(key, input.value); render(); const next = t.tHead.rows[1].cells[index].firstChild; next.focus(); next.setSelectionRange(caret, caret); };
            cell.append(input); filterRow.append(cell);
        });
        const term = el('search').value.toLowerCase();
        const visible = rows.filter(r => columns.some(([k]) => String(value(r, k)).toLowerCase().includes(term)) && [...filters].every(([k, v]) => !v || String(value(r, k)).toLowerCase().includes(v.toLowerCase())));
        const totals = (section, list, label, cls) => {
            const tr = section.insertRow(); tr.className = cls;
            columns.forEach(([k, , , type], i) => { const td = tr.insertCell(); if (type === 'sum') { td.className = 'amount'; td.textContent = amount(ReportDecimal.sum(list.map(r => { const v = value(r, k); return v === '' ? '0' : v; })), k); } else if (!i) td.textContent = label; });
        };
        const groups = new Map();
        visible.forEach(r => { const g = String(value(r, 'DocumentTypeDescription')); if (!groups.has(g)) groups.set(g, []); groups.get(g).push(r); });
        groups.forEach((list, name) => {
            const gr = t.tBodies[0].insertRow(); gr.className = 'group'; const gc = gr.insertCell(); gc.colSpan = columns.length; gc.textContent = 'Document Type: ' + name;
            list.forEach(r => {
                const tr = t.tBodies[0].insertRow(); tr.tabIndex = 0; tr.classList.toggle('selected', selected === r);
                tr.onclick = () => { selected = r; t.querySelectorAll('.selected').forEach(x => x.classList.remove('selected')); tr.classList.add('selected'); };
                columns.forEach(([k, , , type]) => {
                    const td = tr.insertCell(), v = value(r, k);
                    if (k === 'SupplierName') { const a = document.createElement('a'); a.href = ledger(r); a.textContent = v; td.append(a); }
                    else if (k === 'DocNo') { const a = document.createElement('a'); a.href = '#'; a.textContent = v; a.onclick = e => { e.preventDefault(); status('OpenSlips(' + value(r, 'Id') + ', ' + value(r, 'DocumentTypeId') + '): the purchase invoice slip print is not ported yet.'); }; td.append(a); }
                    else if (type === 'sum' || type === 'rate') { td.className = 'amount'; td.textContent = v === '' ? '' : amount(v, k); }
                    else if (type === 'int') { td.className = 'int'; td.textContent = v; }
                    else if (type === 'date') td.textContent = shortDate(v);
                    else td.textContent = v;
                });
            });
            totals(t.tBodies[0], list, 'Sum of ' + name, 'group-total');
        });
        totals(t.tFoot, visible, 'Total', 'grand-total');
        status(rows.length ? visible.length + ' records' : '');
    }
    function show() {
        return run('Loading report…', async () => {
            const p = new URLSearchParams({partyId: el('partyId').value || 0, agentId: el('agentId').value || 0, parentId: el('parentAccount').value || 0, groupId: el('customGroup').value || 0, actionId: document.querySelector('[name=action]:checked').value});
            if (el('useFrom').checked) p.set('fromDate', el('fromDate').value);
            if (el('useTo').checked) p.set('toDate', el('toDate').value);
            const d = await request('/api/accounts/payables-invoice-wise?' + p);
            rows = d.data || []; selected = null; render();
            if (!rows.length) status('');
        });
    }
    /* btnNew_Click: focus From Date and clear the grid structure; filters are left as they are. */
    function reset() { if (busy) return; rows = []; render(); el('fromDate').focus(); }
    el('show').onclick = show; el('reset').onclick = reset;
    el('refresh').onclick = () => run('Loading…', () => loadLookups(false));
    const printMessage = name => status(rows.length ? name + ': the Crystal print is not ported yet.' : 'Record Not Found For Display');
    el('print').onclick = () => printMessage('128_PayablesReportInvoiceWise.rpt');
    el('print01').onclick = () => printMessage('128-01_PayablesReportInvoiceWise.rpt');
    el('shortcuts').onclick = () => el('shortcutDialog').showModal(); el('closeShortcuts').onclick = () => el('shortcutDialog').close();
    el('search').oninput = render;
    ['useFrom', 'useTo'].forEach(id => el(id).onchange = () => el(id === 'useFrom' ? 'fromDate' : 'toDate').disabled = !el(id).checked);
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape' && !document.querySelector('dialog[open]')) { location.href = '/accounts/dashboard'; return; }
        if (e.altKey && !e.ctrlKey && e.key === '2') { e.preventDefault(); el('print01').click(); return; }
        if (!e.ctrlKey) return;
        if (e.altKey) { e.preventDefault(); el('shortcuts').click(); return; }
        const f = {s: show, n: reset, r: () => el('refresh').click(), p: () => el('print').click(), e: () => location.href = '/accounts/dashboard', f5: () => el('fromDate').focus(), arrowup: () => el('fromDate').focus(), arrowdown: () => el('results').querySelector('tbody tr[tabindex]')?.focus()}[e.key.toLowerCase()];
        if (f) { e.preventDefault(); f(); }
    });
    ['fromDate', 'toDate'].forEach(id => el(id).value = localDate(new Date()));
    /* R3 2026-09-30: searchable combos come from countx_prod_combo.js; select2 only if a page still loads it. */
    if ($.fn.select2) $('select').select2({allowClear: true, placeholder: ''});
    render();
    run('Loading filters…', () => loadLookups(true));
})();
