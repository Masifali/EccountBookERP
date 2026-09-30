'use strict';
/* 1005 Payables Aging (PayablesAging_New.cs, screen 47) and 1006 Receivables Aging (Receivables_New.cs,
   screen 769). One script, the page's body data-kind picks the desktop form:
   - receivable: Cost Center (usp_getCostCenters), single Control Account (ReadAll3rdLevelAccountsFor..., TypeNo 2),
     @AppId/@CostCenterId/@ParentAccountId sent, "Cost Center Not Found" when AppId 5.
   - payable: no cost centre, multi-select Control Account (ReadAllAccountGroup, class 3), @ControlAccountIds.
   Grid: RNo/AccountId/BranchesId/Closing hidden, BranchName shown when the branch feature is on and a branch is
   chosen, grouped by ParentAccount, GroupTotals=Always, TotalRow=True, FilterMode=Automatic. */
(() => {
    const el = id => document.getElementById(id);
    const payable = document.body.dataset.kind === 'payable';
    const api = payable ? '/api/accounts/payables-aging' : '/api/accounts/receivables-new';
    let lookup = {}, result = {rows: [], overdue: [], notYetDue: [], totals: {}}, sequence = 0, request, selected = null;
    const filters = new Map();
    const status = t => el('status').textContent = t;
    const amount = v => ReportDecimal.format(v ?? '0', lookup.amountDecimals || 0, true);
    const sum = (list, key) => ReportDecimal.sum(list.map(r => r[key] ?? '0'));
    async function get(path, options) {
        const r = await fetch(api + path, options);
        if (!r.ok) throw Error('Report request failed (' + r.status + '). See the server database error.');
        return r.json();
    }
    function bind(id, data, label, key = 'Id') {
        const select = el(id), old = $('#' + id).val();
        select.replaceChildren();
        if (!select.multiple) select.add(new Option('', ''));
        data.forEach(r => select.add(new Option(r[label] ?? '', r[key])));
        $('#' + id).val(old).trigger('change.select2');
    }
    /* R3 2026-09-30: pages on countx_prod_combo.js (no select2). The combo skips multi-selects, so a
       branches combo that was already enhanced is unwrapped back to a native multi-select when the
       consolidated-branch feature (18) turns it into a checked list (BranchesFill). */
    function nativeMulti(select) {
        const c = select.__dtcombo;
        if (c && c.wrap && c.wrap.parentNode) {
            c.wrap.parentNode.insertBefore(select, c.wrap);
            if (c.destroy) c.destroy();
            c.wrap.remove();
            select.classList.remove('dtcombo-native'); select.removeAttribute('aria-hidden'); select.removeAttribute('tabindex');
            delete select.value; delete select.selectedIndex; delete select.__dtcombo;
            const list = window.DesktopCombo && window.DesktopCombo.instances, i = list ? list.indexOf(c) : -1;
            if (i >= 0) list.splice(i, 1);
        }
        select.multiple = true;
    }
    function controlIds() { return [$('#control').val() || []].flat().filter(Boolean).map(Number); }
    /* AccountFillFromGlobal: AllAccountsWithCustomGroupId where AccountTypeId==3 && AccountClass==class,
       optionally ParentAccountId (ParentCodeId) in the chosen control account(s); distinct ChartOfAccountId. */
    function accounts() {
        const parents = controlIds(), seen = new Set(), cls = payable ? 3 : 2;
        bind('account', (lookup.accounts || []).filter(r => Number(r.AccountTypeId) === 3 && Number(r.AccountClass) === cls
            && (!parents.length || parents.includes(Number(r.ParentCodeId))) && !seen.has(r.ChartOfAccountId) && seen.add(r.ChartOfAccountId)),
            'AccountTitle', 'ChartOfAccountId');
    }
    async function refresh() {
        if (el('refresh').disabled) return;
        el('refresh').disabled = true;
        try {
            lookup = await get('/lookups');
            if (payable) bind('control', lookup.controls || [], 'AccountTitle', 'Id');
            else bind('control', (lookup.controls || []).filter(r => Number(r.TypeNo) === 2), 'AccountTitle', 'id');
            bind('custom', lookup.customGroups || [], 'AcLookUpsDescription');
            if (!payable) bind('cost', lookup.costCenters || [], 'CostCenterName');
            const features = new Set((lookup.features || []).map(r => Number(r.Id)));
            if (features.has(18) && !el('branches').multiple) {
                if ($.fn.select2) { $('#branches').select2('destroy'); el('branches').multiple = true; $('#branches').select2({allowClear: true, placeholder: ''}); }
                else nativeMulti(el('branches'));
            }
            bind('branches', lookup.branches || [], 'BranchName');
            /* BranchesFill: CmbBranches.Text = UserAccount.BranchName */
            if (!([$('#branches').val()].flat().filter(Boolean).length) && lookup.branchId) $('#branches').val(el('branches').multiple ? [String(lookup.branchId)] : String(lookup.branchId)).trigger('change.select2');
            el('branchLabel').hidden = !features.has(17);
            $('#branches').next('.select2').toggle(features.has(17));
            if (!$.fn.select2) { const w = $('#branches').closest('.dtcombo-wrap'); (w.length ? w : $('#branches')).toggle(features.has(17)); }
            lookup.branchFeature = features.has(17);
            if (!payable && Number(lookup.appId) === 5 && (lookup.costCenters || []).length) {
                $('#cost').val(String(lookup.costCenters[0].Id)).trigger('change.select2');
                el('cost').disabled = true;
            }
            accounts();
            status('Choose filters and click Show');
        } catch (e) { status(e.message); } finally { el('refresh').disabled = false; }
    }
    const baseFields = [['BranchName', 'Branch Name', 120], ['AccountCode', 'Account Code', payable ? 80 : 95], ['AccountTitle', 'Account Title', 250], ['AccountType', 'Account Type', 65],
        ['CurrentBalance', 'Current Balance', 110], ['OverDue', 'Over Due', 110], ['NotYetDue', 'Not Yet Due', 110],
        ['IstIntervale', '', 110], ['ScnInterval', '', 110], ['TrdIntarval', '', 110], ['Above', '', 110]];
    const numeric = new Set(['CurrentBalance', 'OverDue', 'NotYetDue', 'IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above']);
    function fields() {
        const first = result.rows[0] || {}, captions = ['FirstIntervalCaption', 'SecondIntervalCaption', 'ThirdIntervalCaption', 'AboveIntervalCaption'];
        const list = baseFields.map(f => f.slice());
        list.slice(7).forEach((f, i) => f[1] = first[captions[i]] || '');
        const branchText = [$('#branches').val()].flat().filter(Boolean).length;
        return list.filter(f => f[0] !== 'BranchName' || (lookup.branchFeature && branchText));
    }
    function popup(row, overdue) {
        const list = (overdue ? result.overdue : result.notYetDue).filter(r => String(r.AccountId) === String(row.AccountId));
        if (!list.length) return;
        /* PayablesReceivablesAgingBreakPopUp lblFor texts, verbatim (including "Net Yet Due"). */
        el('popupTitle').textContent = (payable ? 'Payables Aging of ' : 'Receivable Aging of ') + (overdue ? 'Over Due' : 'Net Yet Due');
        const cols = [['DocumentTypeDescription', 'DocumentType'], ['VoucherCode', 'VoucherCode'], ['VoucherDate', 'VoucherDate'], ['DueDays', 'DueDays'], ['DueDate', 'DueDate'],
            ...(overdue ? [['OverDueBy', 'DaysOverdue']] : []), ['AccountTitle', 'AccountTitle'], ['Amount', 'Amount']];
        const t = el('popupGrid'); t.tHead.replaceChildren(); t.tBodies[0].replaceChildren();
        const h = t.tHead.insertRow();
        cols.forEach(([, label]) => { const th = document.createElement('th'); th.textContent = label; h.append(th); });
        list.forEach(r => { const tr = t.tBodies[0].insertRow(); cols.forEach(([key]) => { const td = tr.insertCell(); if (key === 'Amount') { td.className = 'amount'; td.textContent = amount(r[key]); } else td.textContent = /Date$/.test(key) ? String(r[key] ?? '').slice(0, 10) : r[key] ?? ''; }); });
        const f = t.tHead.insertRow(0); const c = f.insertCell(); c.colSpan = cols.length; c.style.fontWeight = 'bold'; c.textContent = list[0].AccountTitle ?? '';
        const total = t.tBodies[0].insertRow(); cols.forEach(([key], i) => { const td = total.insertCell(); td.style.fontWeight = 'bold'; if (key === 'Amount') { td.className = 'amount'; td.textContent = amount(sum(list, 'Amount')); } else if (!i) td.textContent = 'Total'; });
        el('popup').showModal();
    }
    function ledger(r) {
        /* grd_LinkClicked: GoToGeneralLedgerFromLinkedEvent(AccountId, ActiveYr.Start_Period, EndDate, 0, BranchesId) */
        const start = lookup.year && lookup.year.Start_Period ? String(lookup.year.Start_Period).slice(0, 10) : String(r.StartDate ?? '').slice(0, 10);
        return '/accounts/reports/general-ledger?' + new URLSearchParams({accountId: r.AccountId, fromDate: start, toDate: el('date').value, branchId: r.BranchesId || 0});
    }
    function render() {
        const cols = fields(), t = el('grid');
        t.tHead.replaceChildren(); t.tBodies[0].replaceChildren(); (t.tFoot || t.createTFoot()).replaceChildren();
        const head = t.tHead.insertRow(), filterRow = t.tHead.insertRow();
        cols.forEach(([key, label, width], index) => {
            const th = document.createElement('th'); th.textContent = label; th.style.width = th.style.minWidth = width + 'px'; head.append(th);
            const cell = document.createElement('th'); cell.style.top = '38px'; cell.style.height = '26px';
            const input = document.createElement('input'); input.style.cssText = 'width:100%;min-width:0;height:22px'; input.setAttribute('aria-label', 'Filter ' + label); input.value = filters.get(key) || '';
            input.oninput = () => { const caret = input.selectionStart; filters.set(key, input.value); render(); const next = t.tHead.rows[1].cells[index].firstChild; next.focus(); next.setSelectionRange(caret, caret); };
            cell.append(input); filterRow.append(cell);
        });
        const term = el('search').value.toLowerCase();
        const rows = result.rows.filter(r => cols.some(([k]) => String(r[k] ?? '').toLowerCase().includes(term)) && [...filters].every(([k, v]) => !v || String(r[k] ?? '').toLowerCase().includes(v.toLowerCase())));
        const totalRow = (section, list, label, cls) => {
            const tr = section.insertRow(); tr.className = cls;
            cols.forEach(([k], i) => { const td = tr.insertCell(); if (numeric.has(k)) { td.className = 'amount'; td.textContent = amount(sum(list, k)); } else if (!i) td.textContent = label; });
        };
        /* Rows keep the order of the (optionally sorted) procedure result; ParentAccount grouping is by first appearance. */
        const groups = new Map();
        rows.forEach(r => { const g = String(r.ParentAccount ?? ''); if (!groups.has(g)) groups.set(g, []); groups.get(g).push(r); });
        groups.forEach((list, name) => {
            const gr = t.tBodies[0].insertRow(); gr.className = 'group'; const td = gr.insertCell(); td.colSpan = cols.length; td.textContent = 'ParentAccount: ' + name;
            list.forEach(r => {
                const tr = t.tBodies[0].insertRow(); tr.tabIndex = 0; tr.classList.toggle('selected', selected === r);
                tr.onclick = () => { selected = r; t.querySelectorAll('.selected').forEach(x => x.classList.remove('selected')); tr.classList.add('selected'); };
                tr.onkeydown = e => { if (e.ctrlKey && e.code === 'Space') { e.preventDefault(); location.href = ledger(r); } };
                cols.forEach(([k]) => {
                    const c = tr.insertCell();
                    if (k === 'AccountCode') { const a = document.createElement('a'); a.textContent = r[k] ?? ''; a.href = ledger(r); c.append(a); }
                    else if (k === 'OverDue' || k === 'NotYetDue') { c.className = 'amount'; const a = document.createElement('a'); a.href = '#'; a.textContent = amount(r[k]); a.onclick = e => { e.preventDefault(); popup(r, k === 'OverDue'); }; c.append(a); }
                    else if (numeric.has(k)) { c.className = 'amount'; c.textContent = amount(r[k]); }
                    else c.textContent = r[k] ?? '';
                });
            });
            totalRow(t.tBodies[0], list, 'Sum of ' + name, 'group-total');
        });
        totalRow(t.tFoot, rows, 'Total', 'grand-total');
        const has = result.rows.length > 0, first = result.rows[0] || {};
        ['cards', 'totals', 'period'].forEach(id => el(id).hidden = !has);
        el('cards').replaceChildren();
        if (has) {
            el('periodFrom').value = String(first.StartDate ?? '').slice(0, 10);
            el('periodTo').value = String(first.AsOnDate ?? '').slice(0, 10);
            cols.filter(([k]) => ['IstIntervale', 'ScnInterval', 'TrdIntarval', 'Above'].includes(k)).forEach(([k, label]) => {
                const card = document.createElement('div'); card.className = 'card';
                const h = document.createElement('strong'), v = document.createElement('output'); h.textContent = label; v.textContent = amount(sum(result.rows, k)); card.append(h, v); el('cards').append(card);
            });
            ['CurrentBalance', 'OverDue', 'NotYetDue'].forEach(k => el(k).textContent = amount(sum(result.rows, k)));
        }
        status(rows.length + ' accounts');
    }
    function applySort() {
        /* btnApplySorting / GridFill: DataView.Sort over AccountCode, AccountTitle, CurrentBalance in tick order. */
        const keys = [...document.querySelectorAll('.sort:checked')].map(e => e.value), dir = Number(document.querySelector('[name=sortDirection]:checked').value);
        el('sortCriteria').textContent = keys.length ? '(' + keys.map(k => k + (dir > 0 ? ' ASC' : ' DESC')).join(', ') + ')' : '';
        if (keys.length) result.rows.sort((a, b) => { for (const k of keys) { const d = k === 'CurrentBalance' ? Number(a[k] || 0) - Number(b[k] || 0) : String(a[k] ?? '').localeCompare(String(b[k] ?? '')); if (d) return d * dir; } return 0; });
    }
    async function show() {
        if (el('show').disabled) return;
        if (!payable && Number(lookup.appId) === 5 && !Number(el('cost').value)) { status('Cost Center Not Found'); return; }
        el('show').disabled = true; el('show').setAttribute('aria-busy', 'true');
        const token = ++sequence; request?.abort(); request = new AbortController();
        result = {rows: [], overdue: [], notYetDue: [], totals: {}}; render(); status('Loading…');
        const p = new URLSearchParams({asOnDate: el('date').value, agingDays: el('days').value || 0, reportId: document.querySelector('[name=mode]:checked').value,
            accountId: el('account').value || 0, customGroupId: el('custom').value || 0, branches: [$('#branches').val()].flat().filter(Boolean).join(',')});
        if (payable) p.set('controlAccounts', controlIds().join(','));
        else { p.set('parentId', el('control').value || 0); p.set('costCenterId', el('cost').value || 0); }
        try {
            const data = await get('?' + p, {signal: request.signal});
            if (token !== sequence) return;
            result = data; applySort(); render();
        } catch (e) { if (e.name !== 'AbortError') status(e.message); }
        finally { if (token === sequence) { el('show').disabled = false; el('show').removeAttribute('aria-busy'); } }
    }
    function reset() {
        /* reset(): clear aging days, (payables also clears the three sort ticks), hide panels, GridFill(). */
        el('days').value = '';
        if (payable) document.querySelectorAll('.sort').forEach(e => e.checked = false);
        el('days').focus();
        show();
    }
    el('show').onclick = show; el('refresh').onclick = refresh; el('new').onclick = reset;
    $('#control').on('change', accounts);
    el('sort').onclick = () => { if (result.rows.length) { applySort(); render(); } };
    el('search').oninput = render;
    el('closePopup').onclick = () => el('popup').close();
    el('print').onclick = () => status(result.rows.length ? (payable ? '124_Payables_New.rpt' : '123_Receivables_New.rpt') + ' print parity is pending.' : 'Not Record Found For Display');
    el('help').onclick = () => el('shortcutDialog').showModal();
    el('closeShortcuts').onclick = () => el('shortcutDialog').close();
    document.addEventListener('keydown', e => {
        if (e.key === 'Escape' && !document.querySelector('dialog[open]')) { location.href = '/accounts/dashboard'; return; }
        if (!e.ctrlKey) return;
        if (e.altKey) { e.preventDefault(); el('help').click(); return; }
        const k = e.key.toLowerCase();
        const f = {s: show, p: () => el('print').click(), n: reset, r: refresh, e: () => location.href = '/accounts/dashboard', f5: () => el('date').focus(), arrowup: () => el('date').focus(), arrowdown: () => el('grid').querySelector('tbody tr[tabindex]')?.focus()}[k];
        if (f) { e.preventDefault(); f(); }
    });
    const d = new Date();
    el('date').value = [d.getFullYear(), String(d.getMonth() + 1).padStart(2, '0'), String(d.getDate()).padStart(2, '0')].join('-');
    if ($.fn.select2) $('select').select2({allowClear: true, placeholder: ''});
    refresh();
})();
