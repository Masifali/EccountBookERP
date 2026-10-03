'use strict';
/* Inventory Payables and Receivables - InventoryPayablesandReceivables.cs (screen 61).
   Load: Date From = Date To = today (constructor FromDate/ToDate = Now), Purchase + Payables ticked, GetData() runs.
   GetData: TranTypes 1/2/3 (Purchase / Sale / Purchase & Sale), ReportType 1 Receivables, 2 Payables, 3 both.
   Grid: BranchName (feature 17 and a branch chosen), Parent Account, AccountClass, Account Code (ledger link),
   Account Title, AccountType, six amount columns summed; sorted by Account Title ascending. */
(() => {
    const el = id => document.getElementById(id);
    let lookup = {}, rows = [], busy = false, selected = null;
    const filters = new Map();
    const status = t => el('status').textContent = t;
    const localDate = d => [d.getFullYear(), String(d.getMonth() + 1).padStart(2, '0'), String(d.getDate()).padStart(2, '0')].join('-');
    const amounts = ['ObDebit', 'ObCredit', 'CurrDebit', 'CurrCredit', 'ClDebit', 'ClCredit'];
    const base = [['Ac3LevelTitle', 'Parent Account', 150], ['ClassName', 'AccountClass', 80], ['Ac4LevelCode', 'Account Code', 90], ['Ac4LevelTitle', 'Account Title', 300], ['AccountType', 'AccountType', 80],
        ['ObDebit', 'Opening Debit', 100], ['ObCredit', 'Opening Credit', 100], ['CurrDebit', 'Debit', 100], ['CurrCredit', 'Credit', 100], ['ClDebit', 'Closing Debit', 100], ['ClCredit', 'Closing Credit', 100]];
    const amount = v => ReportDecimal.format(v ?? '0', lookup.amountDecimals || 0, true);
    const branchIds = () => [$('#branches').val()].flat().filter(Boolean).join(',');
    async function request(url) {
        const r = await fetch(url, {headers: {Accept: 'application/json'}});
        if (!r.ok) throw Error('Unable to load report (' + r.status + '). The database error is recorded in the server log.');
        return r.json();
    }
    function bind(id, data, label) {
        const s = el(id); s.replaceChildren(); if (!s.multiple) s.add(new Option('', ''));
        data.forEach(r => s.add(new Option(r[label] ?? '', r.Id)));
        $(s).val(s.multiple ? [] : '').trigger('change.select2');
    }
    /* R4 2026-09-30: countx_prod_combo.js replaces select2. When BranchFeatureConsolidated (18) makes CmbBranches a checked
       list, the single branch combo is unwrapped and re-enhanced in the CHECKED mode (data-dtcombo-checked, 2026-10-02). */
    function nativeMulti(select) {
        const c = select.__dtcombo;
        if (c) {
            c.destroy();
            const wrap = c.wrap; wrap.parentNode.insertBefore(select, wrap); wrap.parentNode.removeChild(wrap);
            select.classList.remove('dtcombo-native'); select.removeAttribute('aria-hidden'); select.removeAttribute('tabindex');
            delete select.value; delete select.selectedIndex; delete select.__dtcombo; delete select.__dtcomboHooked;
            if (window.DesktopCombo) { const i = DesktopCombo.instances.indexOf(c); if (i >= 0) DesktopCombo.instances.splice(i, 1); }
        }
        select.multiple = true; select.setAttribute('data-dtcombo', 'single'); select.setAttribute('data-dtcombo-checked', '');
        if (window.DesktopCombo) DesktopCombo.enhance(select);
    }
    function columns() {
        return (lookup.branchFeature && branchIds()) ? [['BranchName', 'Branch Name', 120], ...base] : base;
    }
    function render() {
        const cols = columns(), t = el('results');
        t.tHead.replaceChildren(); t.tBodies[0].replaceChildren(); t.tFoot.replaceChildren();
        const head = t.tHead.insertRow(), filterRow = t.tHead.insertRow();
        cols.forEach(([k, label, w], i) => {
            const th = document.createElement('th'); th.textContent = label; th.style.width = th.style.minWidth = w + 'px'; head.append(th);
            const cell = document.createElement('th'); cell.style.top = '38px'; cell.style.height = '26px';
            const input = document.createElement('input'); input.style.cssText = 'width:100%;min-width:0;height:22px'; input.setAttribute('aria-label', 'Filter ' + label); input.value = filters.get(k) || '';
            input.oninput = () => { const c = input.selectionStart; filters.set(k, input.value); render(); const n = t.tHead.rows[1].cells[i].firstChild; n.focus(); n.setSelectionRange(c, c); };
            cell.append(input); filterRow.append(cell);
        });
        const term = el('search').value.toLowerCase();
        const visible = rows.filter(r => cols.some(([k]) => String(r[k] ?? '').toLowerCase().includes(term)) && [...filters].every(([k, v]) => !v || String(r[k] ?? '').toLowerCase().includes(v.toLowerCase())))
            .sort((a, b) => String(a.Ac4LevelTitle ?? '').localeCompare(String(b.Ac4LevelTitle ?? '')));
        visible.forEach(r => {
            const tr = t.tBodies[0].insertRow(); tr.tabIndex = 0; tr.classList.toggle('selected', selected === r);
            tr.onclick = () => { selected = r; t.querySelectorAll('.selected').forEach(x => x.classList.remove('selected')); tr.classList.add('selected'); };
            cols.forEach(([k]) => {
                const td = tr.insertCell();
                if (k === 'Ac4LevelCode') {
                    /* grd_LinkClicked: GoToGeneralLedgerFromLinkedEvent(AccountId, Date From, Date To, BranchId) */
                    const a = document.createElement('a'); a.textContent = r[k] ?? '';
                    a.href = '/accounts/reports/general-ledger?' + new URLSearchParams({accountId: r.AccountId, fromDate: el('fromDate').value, toDate: el('toDate').value, branchId: r.BranchesId || 0});
                    td.append(a);
                } else if (amounts.includes(k)) { td.className = 'amount'; td.textContent = amount(r[k]); }
                else td.textContent = r[k] ?? '';
            });
        });
        const total = t.tFoot.insertRow();
        cols.forEach(([k], i) => { const td = total.insertCell(); if (amounts.includes(k)) { td.className = 'amount'; td.textContent = amount(ReportDecimal.sum(visible.map(r => r[k] ?? '0'))); } else if (!i) td.textContent = 'Total'; });
        status(rows.length ? visible.length + ' records' : '');
    }
    async function getData() {
        if (busy) return; busy = true; el('show').disabled = true; status('Loading…');
        try {
            const report = el('receivables').checked && el('payables').checked ? 3 : el('payables').checked ? 2 : el('receivables').checked ? 1 : 0;
            const p = new URLSearchParams({fromDate: el('fromDate').value, toDate: el('toDate').value, tranTypes: document.querySelector('[name=tran]:checked')?.value || 0, reportType: report,
                approved: el('approved').checked, clDebit: el('clDebit').value || 0, clCredit: el('clCredit').value || 0,
                customerGroups: ($('#inventoryGroups').val() || []).join(','), tradeTypes: ($('#tradeTypes').val() || []).join(','),
                customGroupId: el('customGroup').value || 0, branches: branchIds()});
            rows = await request('/api/accounts/inventory-payables-receivables?' + p); selected = null; render();
        } catch (e) { status(e.message); } finally { busy = false; el('show').disabled = false; }
    }
    /* reset(): Inventory Group, TradeType and closing boxes cleared, Purchase ticked, grid cleared. */
    function reset() {
        $('#inventoryGroups').val([]).trigger('change.select2'); $('#tradeTypes').val([]).trigger('change.select2');
        document.querySelector('[name=tran][value="1"]').checked = true; el('clDebit').value = ''; el('clCredit').value = '';
        rows = []; render(); print134A();
    }
    /* handlePrint134AVisibility(): 134A-Print only for Purchase or Purchase & Sale. */
    function print134A() { el('print134A').hidden = document.querySelector('[name=tran]:checked')?.value === '2'; }
    document.querySelectorAll('[name=tran]').forEach(r => r.onchange = print134A);
    el('show').onclick = getData; el('refresh').onclick = reset; el('search').oninput = render;
    el('print').onclick = () => status(rows.length ? '134-InventoryPayablesandReceivables.rpt: the Crystal print is not ported yet.' : '');
    el('print134A').onclick = () => status(rows.length ? '134A-InventoryPayablesandReceivables.rpt: the Crystal print is not ported yet.' : '');
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape') { location.href = '/accounts/dashboard'; return; }
        if (!e.ctrlKey) return;
        const f = {p: () => el('print').click(), n: reset, e: () => location.href = '/accounts/dashboard'}[e.key.toLowerCase()];
        if (f) { e.preventDefault(); f(); }
    });
    el('fromDate').value = el('toDate').value = localDate(new Date());
    if ($.fn.select2) $('select').select2({allowClear: true, placeholder: ''});
    render(); print134A();
    (async () => {
        try {
            lookup = await request('/api/accounts/inventory-payables-receivables/lookups');
            const features = new Set((lookup.features || []).map(r => Number(r.Id)));
            lookup.branchFeature = features.has(17);
            el('branchField').hidden = !features.has(17);
            if (features.has(17) && features.has(18)) { if ($.fn.select2) { $('#branches').select2('destroy'); el('branches').multiple = true; $('#branches').select2({allowClear: true, placeholder: ''}); } else nativeMulti(el('branches')); }
            bind('branches', lookup.branches || [], 'BranchName');
            /* BranchesFill: CmbBranches.Text = UserAccount.BranchName */
            if (features.has(17) && lookup.branchId) $('#branches').val(el('branches').multiple ? [String(lookup.branchId)] : String(lookup.branchId)).trigger('change.select2');
            bind('customGroup', lookup.customGroups || [], 'AcLookUpsDescription');
            bind('inventoryGroups', lookup.inventoryGroups || [], 'Description');
            bind('tradeTypes', lookup.tradeTypes || [], 'InvParentCateDescription');
            await getData();
        } catch (e) { status(e.message); }
    })();
})();
