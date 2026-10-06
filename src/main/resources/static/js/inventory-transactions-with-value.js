/* Screen 293 "Transaction Report (With Value)" - desktop Architecture.WinApp.Inventory_Stocks_Report.InventoryEvaluationItemLedger.
   Load: BranchesFill (USP_GetBranchsAllocatedToUser, user's branch checked) -> ParentCategoryComboFill
   (USP_Evalaution_DropDown_ByParentCategories @BranchesIds, @InventoryParentCategory='1,2,3,4,6', every parent checked)
   -> OtherComboByParentCategoryFill (rows of that same call filtered by the checked parents) -> Date Type "This Week"
   -> From Date = usp_getInventoryStockAsOnDate + 1 day. Show: USP_InventoryEvaluationItemLedger_Rpt. */
(() => {
    'use strict';
    const $id = id => document.getElementById(id), endpoint = '/inventory/api/reports/stock-transactions-with-value';
    const months = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    const singles = ['itemClassGroupId','itemCategoryId','itemTypeId','itemId','warehouseId','jobLotId','cropYear','itemStockAc','documentTypeId'];
    /* GridSettings(): visible columns of the desktop DataTable in order, Constants.InventoryConstants widths.
       Hidden there: Id, SupplierCustomerId, DocumentTypeId, AvgRate, BalQty, BalWeight, BalAmount. */
    const columns = [['BranchName',130],['DocNo',60],['TranDate',73],['DocType',150],['Description',130],['WareHouse',150],
        ['ItemCategory',130],['ItemType',60],['ItemName',150],['Crop',73],['JobLot',80],['PackSize',60],['PackingType',115],
        ['QtyIn',70,'qty'],['QtyOut',70,'qty'],['WeightIn',80,'qty'],['WeightOut',80,'qty'],['AmountIn',90,'amount'],['AmountOut',90,'amount'],
        ['ItemRate',70,'rate'],['JobOrderNo',80],['StepDescription',130]];
    let lookupData = {}, choices = [], records = [], view = [], busy = false, filtersReady = false, amountPlaces = 0, ratePlaces = 2, branchTimer = null;

    const value = (row, key) => { if (!row) return undefined; if (key in row) return row[key]; const k = Object.keys(row).find(x => x.toLowerCase() === key.toLowerCase()); return k === undefined ? undefined : row[k]; };
    const text = (row, key) => { const v = value(row, key); return v === null || v === undefined ? '' : String(v); };
    const dec = (row, key) => { const v = value(row, key); return v === null || v === undefined || v === '' ? '0' : String(v); };
    function status(message, error = false) { $id('status').textContent = message; $id('status').classList.toggle('error', error); }
    function csrfHeaders(extra) { const headers = {...(extra || {})}, token = document.querySelector('meta[name=_csrf]')?.content, key = document.querySelector('meta[name=_csrf_header]')?.content; if (token && key) headers[key] = token; return headers; }
    function request(url, options) { return InventoryRequest.json(url, {...options, headers: csrfHeaders(options?.headers)}); }
    async function run(button, label, action) { if (busy || button?.disabled) return; busy = true; try { await InventoryRequest.execute(button, label, action); } catch (error) { status(error.message, true); } finally { busy = false; } }

    /* ---- dates (DateTimePicker CustomFormat dd-MMM-yy) ---- */
    function setDate(key, iso) { if (!iso) return; const [year, month, day] = String(iso).slice(0, 10).split('-'); $id(key).value = day + '-' + months[Number(month) - 1] + '-' + year.slice(-2); $id(key + '-calendar').value = [year, month, day].join('-'); }
    function getDate(key) {
        const match = /^(\d{1,2})-([A-Za-z]{3})-(\d{2}|\d{4})$/.exec($id(key).value.trim());
        if (!match) throw Error('Enter dates as dd-MMM-yy');
        const month = months.findIndex(m => m.toLowerCase() === match[2].toLowerCase());
        let year = Number(match[3]);
        if (match[3].length === 2) { const previous = Number($id(key + '-calendar').value.slice(0, 4)); year = previous % 100 === year ? previous : (year < 30 ? 2000 : 1900) + year; }
        const date = new Date(year, month, Number(match[1]));
        if (month < 0 || date.getFullYear() !== year || date.getMonth() !== month || date.getDate() !== Number(match[1])) throw Error('Enter a valid date');
        return ReportLoading.localDate(date);
    }
    function dateText(v) {
        if (v === null || v === undefined || v === '') return '';
        const iso = typeof v === 'number' ? ReportLoading.localDate(new Date(v)) : String(v);
        if (!/^\d{4}-\d{2}-\d{2}/.test(iso)) return iso;
        const [y, m, d] = iso.slice(0, 10).split('-');
        return d + '-' + months[Number(m) - 1] + '-' + y.slice(-2);
    }
    /* cmbDateType_ValueChanged */
    function dateType() {
        const today = new Date(), from = new Date(today), type = $id('dateType').value;
        if (!type) return;
        if (type === '2') from.setDate(from.getDate() - 7);
        if (type === '3') from.setFullYear(today.getUTCFullYear(), today.getUTCMonth(), 1);
        if (type === '4') from.setMonth(0, 1);
        if (type === '5') {
            const years = lookupData?.financialYears || [];
            if (years.length !== 1) { status('Select one active financial year in your company before using Financial Year.', true); return; }
            const start = value(years[0], 'Start_Period');
            if (!start) { status('The active financial year has no start date.', true); return; }
            setDate('fromDate', String(start).slice(0, 10)); return;
        }
        setDate('fromDate', ReportLoading.localDate(from));
        if (type === '3' || type === '4') setDate('toDate', ReportLoading.localDate(today));
    }

    /* ---- dropdowns ---- */
    const selectedIds = id => Array.from($id(id).selectedOptions, o => Number(o.value)).filter(Boolean);
    function setValue(id, v) { $($id(id)).val(v).trigger('change.select2'); }
    function fill(id, rows, idKey = 'Id', nameKey = 'name') {
        const select = $id(id), seen = new Set();
        select.replaceChildren();
        if (!select.multiple) select.add(new Option('', ''));
        (rows || []).forEach(row => {
            const key = value(row, idKey);
            if (key === null || key === undefined || key === '' || seen.has(String(key))) return;
            seen.add(String(key));
            select.add(new Option(text(row, nameKey), key));
        });
        setValue(id, select.multiple ? [] : '');
    }
    /* OtherComboByParentCategoryFill / cmbparentCategory_Leave: the other combos list only the rows of the
       checked parent categories; with no parent checked every combo is emptied. */
    function otherCombos() {
        const parents = new Set(selectedIds('parentIds').map(String));
        for (const id of singles) {
            const activity = $id(id).dataset.activity;
            fill(id, parents.size ? choices.filter(r => text(r, 'ActivityType') === activity && parents.has(text(r, 'InventoryParentCategoriesId'))) : []);
        }
    }
    /* cmbBranchName_Leave -> ParentCategoryComboFill (every parent category checked) */
    async function loadChoices() {
        const branches = selectedIds('branchIds');
        choices = branches.length ? await request(endpoint + '/choices?branches=' + encodeURIComponent(branches.join(','))) : [];
        if (branches.join(',') !== selectedIds('branchIds').join(',')) { scheduleBranches(); return; }
        fill('parentIds', choices.filter(r => text(r, 'ActivityType') === 'ParentCategories'));
        setValue('parentIds', Array.from($id('parentIds').options, o => o.value));
        otherCombos();
    }
    function scheduleBranches() {
        clearTimeout(branchTimer);
        branchTimer = setTimeout(() => { if (busy) { scheduleBranches(); return; } run(null, 'Loading branch filters…', loadChoices); }, 350);
    }
    /* Form Load / btnRefresh_Click: BranchesFill, ParentCategoryComboFill, OtherComboByParentCategoryFill */
    async function refresh() {
        filtersReady = false; ['show', 'print411', 'print412'].forEach(id => ReportLoading.setDisabled($id(id), true));
        lookupData = await request(endpoint + '/lookups');
        amountPlaces = Number(lookupData.amountDecimals) || 0;
        ratePlaces = lookupData.rateDecimals === undefined || lookupData.rateDecimals === null ? 2 : Number(lookupData.rateDecimals);
        const branches = lookupData.branches || [];
        fill('branchIds', branches, 'BranchId', 'BranchName');
        const own = branches.find(r => Number(value(r, 'BranchId')) === Number(lookupData.branchId));
        const first = own || branches[0];
        setValue('branchIds', first ? [String(value(first, 'BranchId'))] : []);
        await loadChoices();
        filtersReady = true; ['show', 'print411', 'print412'].forEach(id => ReportLoading.setDisabled($id(id), false));
    }
    /* btnNew_Click -> reset(): clears the filter texts (no re-fill) and puts the AsOnDate back in From Date */
    function reset() {
        for (const id of singles) setValue(id, '');
        setValue('parentIds', []);
        if (lookupData.fromDate) setDate('fromDate', lookupData.fromDate);
        status('');
    }

    /* ---- grid ---- */
    const qty = v => ReportDecimal.format(v ?? '0', 3).replace(/(\.\d*?)0+$/, '$1').replace(/\.$/, '');              // ###,##0.###
    const upTo2 = v => ReportDecimal.format(v ?? '0', 2).replace(/(\.\d*?)0+$/, '$1').replace(/\.$/, '');            // #,0.##
    const amount = v => ReportDecimal.format(v ?? '0', amountPlaces);                                                 // stringFormatsingle
    const rate = n => (Number.isFinite(n) ? n : 0).toLocaleString('en-US', {minimumFractionDigits: ratePlaces, maximumFractionDigits: ratePlaces}); // DecimalRateFormate
    const minus = v => { const s = String(v ?? '0').trim(); return s.startsWith('-') ? s.slice(1) : s.startsWith('+') ? '-' + s.slice(1) : '-' + s; };
    function show(kind, v) { return kind === 'qty' ? qty(v) : kind === 'amount' ? amount(v) : kind === 'rate' ? rate(Number(v)) : String(v ?? ''); }
    /* btnshow_Click: the procedure rows become the desktop DataTable; ItemRate = AvgRate = AmountRunBal / WeightRunBal * 40 */
    function toView(r) {
        const balAmount = Number(dec(r, 'AmountRunBal')), balWeight = Number(dec(r, 'WeightRunBal'));
        const avgRate = balWeight !== 0 ? balAmount / balWeight * 40 : 0;
        return {BranchName: text(r, 'BranchName'), Id: Number(value(r, 'Id')) || 0, DocNo: Number(value(r, 'DocNo')) || 0, TranDate: dateText(value(r, 'TranDate')),
            DocumentTypeId: Number(value(r, 'DocumentTypeId')) || 0, DocType: text(r, 'DocumentTypeDescription'), SupplierCustomerId: Number(value(r, 'SupplierCustomerId')) || 0,
            Description: text(r, 'Description'), WareHouse: text(r, 'WareHouseCode'), ItemCategory: text(r, 'CategoryDescription'), ItemType: text(r, 'TypeDescription'),
            ItemName: text(r, 'ItemName'), Crop: text(r, 'CropBatch'), JobLot: text(r, 'JobLot'), PackSize: text(r, 'PackSize'), PackingType: text(r, 'PackingType'),
            QtyIn: dec(r, 'QtyIn'), QtyOut: dec(r, 'QtyOut'), BalQty: dec(r, 'QtyRunBal'), WeightIn: dec(r, 'WeightIn'), WeightOut: dec(r, 'WeightOut'), BalWeight: dec(r, 'WeightRunBal'),
            AmountIn: dec(r, 'AmountIn'), AmountOut: dec(r, 'AmountOut'), BalAmount: dec(r, 'AmountRunBal'), ItemRate: avgRate, AvgRate: avgRate,
            JobOrderNo: text(r, 'JobOrderNo'), StepDescription: text(r, 'StepDescription')};
    }
    function cell(row, content, tag = 'td') { const c = document.createElement(tag); c.textContent = content; row.append(c); return c; }
    function render(list) {
        $id('rows').replaceChildren(); $id('totals').replaceChildren();
        const fragment = document.createDocumentFragment();
        list.forEach(item => {
            const row = document.createElement('tr'); row.tabIndex = 0;
            row.onclick = () => { for (const r of $id('rows').rows) r.classList.toggle('selected', r === row); };
            row.onkeydown = e => { if (['ArrowUp', 'ArrowDown'].includes(e.key)) { e.preventDefault(); (e.key === 'ArrowUp' ? row.previousElementSibling : row.nextElementSibling)?.focus(); } };
            columns.forEach(([key, , kind]) => { const td = cell(row, show(kind, item[key])); if (kind) td.className = 'number'; });
            fragment.append(row);
        });
        $id('rows').append(fragment);
        if (!list.length) return;
        /* TotalRow: Sum on QtyIn/QtyOut/WeightIn/WeightOut/AmountIn/AmountOut, Average on ItemRate */
        const total = document.createElement('tr');
        columns.forEach(([key, , kind]) => {
            let content = '';
            if (kind === 'qty' || kind === 'amount') content = show(kind, ReportDecimal.sum(list.map(r => r[key])));
            if (kind === 'rate') content = rate(list.reduce((s, r) => s + r[key], 0) / list.length);
            const td = cell(total, content); if (kind) td.className = 'number';
        });
        $id('totals').append(total);
    }
    function filter() {
        const inputs = [...$id('head').querySelectorAll('input[data-key]')];
        const kinds = Object.fromEntries(columns.map(([key, , kind]) => [key, kind]));
        render(view.filter(row => inputs.every(input => !input.value || show(kinds[input.dataset.key], row[input.dataset.key]).toLowerCase().includes(input.value.toLowerCase()))));
    }
    /* txtQty/Weight/Amount In, Out, Bal totals (Bal = In - Out); Qty "#,0", Weight and Amount "#,0.##" */
    function totalsBox() {
        const sum = key => ReportDecimal.sum(view.map(r => r[key]));
        const qIn = sum('QtyIn'), qOut = sum('QtyOut'), wIn = sum('WeightIn'), wOut = sum('WeightOut'), aIn = sum('AmountIn'), aOut = sum('AmountOut');
        $id('txtQtyInTotal').value = ReportDecimal.format(qIn, 0); $id('txtQtyOutTotal').value = ReportDecimal.format(qOut, 0); $id('txtQtyBalTotal').value = ReportDecimal.format(ReportDecimal.sum([qIn, minus(qOut)]), 0);
        $id('txtWeightInTotal').value = upTo2(wIn); $id('txtWeightOutTotal').value = upTo2(wOut); $id('txtWeightBalTotal').value = upTo2(ReportDecimal.sum([wIn, minus(wOut)]));
        $id('txtAmountInTotal').value = upTo2(aIn); $id('txtAmountOutTotal').value = upTo2(aOut); $id('txtAmountBalTotal').value = upTo2(ReportDecimal.sum([aIn, minus(aOut)]));
    }
    async function load() {
        if (!filtersReady) throw Error('Report filters are unavailable. Refresh after your company access is enabled.');
        if (!$id('filters').reportValidity()) return;
        const body = {fromDate: getDate('fromDate'), toDate: getDate('toDate'), branchIds: selectedIds('branchIds'), parentIds: selectedIds('parentIds'), saleValue: $id('saleValue').checked};
        if (!body.branchIds.length) { $id('branchIds').closest('.ctl')?.querySelector('input,.dtcombo-input')?.focus(); throw Error('Select branch first'); }
        for (const id of singles) {
            if (id === 'cropYear') { const option = $id(id).selectedOptions[0]; body.cropYear = option && option.value ? option.text : null; }
            else body[id] = $id(id).value ? Number($id(id).value) : null;
        }
        status('Loading transactions…');
        records = await request(endpoint, {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(body)});
        view = records.map(toView);
        totalsBox(); filter();
        status(records.length ? `${records.length} records` : '');
    }
    /* print_Click_1 / btnStockReport_Click: Reporting.ShowReportWithDataTable(lst, rpt) with @CompanyName / @CompanyAddress;
       lst is the procedure's own DataTable, so its rows go as returned. The server adds company name and address. */
    function print(rpt) {
        if (!records.length) { alert('Record Not Found For Display'); return; }
        const win = window.open('about:blank', '_blank');
        fetch('/reports/print/grid', {method: 'POST', credentials: 'same-origin', headers: csrfHeaders({'Content-Type': 'application/json'}), body: JSON.stringify({rpt, title: null, rows: records})})
            .then(r => {
                const type = r.headers.get('Content-Type') || '';
                if (r.ok && type.startsWith('application/pdf')) return r.blob().then(b => { if (win) win.location = URL.createObjectURL(b); });
                return r.text().then(t => { if (win) win.close(); alert(t || ('Print failed (' + r.status + ')')); });
            })
            .catch(e => { try { win && win.close(); } catch (x) { } alert(e.message); });
    }

    /* grid header + Janus filter row */
    const header = document.createElement('tr'), filters = document.createElement('tr');
    header.className = 'heads'; filters.className = 'column-filters';
    columns.forEach(([key, width]) => {
        const th = cell(header, key, 'th'); th.dataset.col = key; th.style.width = th.style.minWidth = width + 'px';
        const input = document.createElement('input'); input.dataset.key = key; input.setAttribute('aria-label', 'Filter ' + key); input.oninput = filter;
        cell(filters, '', 'th').append(input);
    });
    $id('head').append(header, filters);

    $id('filters').addEventListener('submit', event => { event.preventDefault(); run($id('show'), 'Loading transactions…', load); });
    $('#dateType').off('change').on('change', dateType);
    $('#branchIds').on('change', scheduleBranches);
    $('#parentIds').on('change', otherCombos);
    $id('refresh').addEventListener('click', () => run($id('refresh'), 'Refreshing…', refresh));
    $id('new').addEventListener('click', () => { if (!busy) reset(); });
    $id('print412').addEventListener('click', () => print('412-StockRptInventoryTransactionsNew.rpt'));
    $id('print411').addEventListener('click', () => print('411-InvStockRptInventoryTransactionsNew.rpt'));
    $id('shortcuts').addEventListener('click', () => $id('keys').showModal()); $id('close-keys').onclick = () => $id('keys').close();
    for (const key of ['fromDate', 'toDate']) {
        $id(key + '-calendar').onchange = () => { if ($id(key + '-calendar').value) setDate(key, $id(key + '-calendar').value); };
        $id(key).onblur = () => { try { setDate(key, getDate(key)); status(''); } catch (error) { status(error.message, true); } };
        $id(key).onkeydown = e => { if (['ArrowUp', 'ArrowDown'].includes(e.key)) { e.preventDefault(); try { const date = new Date(getDate(key) + 'T00:00:00'); date.setDate(date.getDate() + (e.key === 'ArrowUp' ? 1 : -1)); setDate(key, ReportLoading.localDate(date)); } catch (error) { status(error.message, true); } } };
    }
    /* PurchaseOrderHistory_KeyDown */
    document.addEventListener('keydown', event => {
        if ($id('keys').open || document.querySelector('.select2-container--open')) return;
        const key = event.key.toLowerCase();
        if (event.ctrlKey && event.altKey && ['control', 'alt'].includes(key)) { event.preventDefault(); $id('keys').showModal(); return; }
        if (event.altKey && !event.ctrlKey && (event.code === 'Digit1' || event.code === 'Numpad1')) { event.preventDefault(); $id('print411').click(); return; }
        if (event.altKey && !event.ctrlKey && (event.code === 'Digit2' || event.code === 'Numpad2')) { event.preventDefault(); $id('print412').click(); return; }
        if (event.ctrlKey && !event.altKey) {
            const button = {s: 'show', r: 'refresh', n: 'new'}[key];
            if (button) { event.preventDefault(); $id(button).click(); }
            if (['f5', 'arrowup'].includes(key)) { event.preventDefault(); $('#dateType').select2('focus'); }
            if (key === 'arrowdown') { event.preventDefault(); document.querySelector('.grid').focus(); }
        }
        if (key === 'escape' || (event.ctrlKey && key === 'e')) { event.preventDefault(); if (!busy) location.assign('/inventory'); }
    });

    $('#filters select:not([data-dtcombo-checked])').each(function () { $(this).select2({width: '100%', minimumResultsForSearch: 0, placeholder: '', allowClear: true, dropdownAutoWidth: true}); });
    setDate('toDate', ReportLoading.localDate());
    $id('dateType').value = '2'; dateType(); $('#dateType').trigger('change.select2');
    totalsBox();
    ['show', 'print411', 'print412'].forEach(id => ReportLoading.setDisabled($id(id), true));
    run(null, 'Loading report filters…', async () => { await refresh(); if (lookupData.fromDate) setDate('fromDate', lookupData.fromDate); });
})();
