/* Screen 171 "Item List" - desktop Architecture.WinApp.Inventory_Reports.frmRptItemList.
   Load: ItemClass (Sp_ItemClass_GetAllMethod 'ReadAll'), ItemTypes / ItemCategory (Sp_ItemType_GetAllMethod /
   Sp_ItemCategory_GetAllMethod 'ReadByOrganizationCompanyId'), Purchase / Sale / CGS GL accounts
   (Sp_Item_GetAllMethod 'GetGLAccountReferedInItemDifinition'); no default selection (ZeroIndex false).
   Show / New: GridBind -> SP_Item_List_Rpt with OrganizationId + CompanyId only (the desktop does not pass the filters).
   200-Print: ShowReport -> SP_Item_List_Rpt with every filter -> 200-InvRptItemsList.rpt. */
(() => {
    'use strict';
    const $id = id => document.getElementById(id), endpoint = '/inventory/api/reports/item-list';
    const combos = ['itemCategoryId', 'itemTypeId', 'itemClassId', 'purchaseGLAC', 'saleGLAC', 'cogsGLAC'];
    /* frmRptItemList_Load DataTable columns (RetrieveStructure: caption = column name) and GridSetting() widths.
       [column, width, procedure column]; ItemClass1 is Visible = false; IS Active is a CheckBox column. */
    const columns = [['SrNo', 100, 'SeqNo'], ['RNo', 100, 'SeqNo'], ['ItemCode', 50, 'ItemCode'], ['ItemName', 150, 'ItemName'],
        ['ItemOtherName', 100, 'ItemName'], ['UOM', 50, 'UOMCode'], ['GL Sale Account Code', 50, 'GlSaleAccountCode'],
        ['ItemCategory', 150, 'CategoryDescription'], ['ItemType', 150, 'TypeDescription'], ['ItemClass', 100, 'ClassDescription'],
        ['ItemClass1', 0, 'ClassDescription', true], ['Stock GL A/C', 100, 'GlStockAccountTitle'], ['Sale GL A/C', 150, 'GlSaleAccountTitle'],
        ['CGS GL A/C', 150, 'GlCgsAccountTitle'], ['IS Active', 50, 'ItemStatus']];
    const visible = columns.filter(c => !c[3]);
    let view = [], shown = [], busy = false, current = -1;

    const value = (row, key) => { if (!row) return undefined; if (key in row) return row[key]; const k = Object.keys(row).find(x => x.toLowerCase() === key.toLowerCase()); return k === undefined ? undefined : row[k]; };
    const text = (row, key) => { const v = value(row, key); return v === null || v === undefined ? '' : String(v); };
    function status(message, error = false) { $id('status').textContent = message; $id('status').classList.toggle('error', error); }
    function csrfHeaders(extra) { const headers = {...(extra || {})}, token = document.querySelector('meta[name=_csrf]')?.content, key = document.querySelector('meta[name=_csrf_header]')?.content; if (token && key) headers[key] = token; return headers; }
    function request(url, options) { return InventoryRequest.json(url, {...options, headers: csrfHeaders(options?.headers)}); }
    async function run(button, label, action) { if (busy || button?.disabled) return; busy = true; try { await InventoryRequest.execute(button, label, action); } catch (error) { alert(error.message); status(error.message, true); } finally { busy = false; } }

    /* DDL.BindDDL(dt, combo, valueMember, displayMember, caption, ZeroIndex:false): every row, nothing selected.
       The desktop only binds when the list has rows. */
    function fill(id, rows, idKey, nameKey) {
        const select = $id(id);
        if (!rows || !rows.length) return;
        select.replaceChildren(new Option('', ''));
        rows.forEach(row => { const key = value(row, idKey); if (key === null || key === undefined || key === '') return; select.add(new Option(text(row, nameKey), key)); });
        $(select).val('').trigger('change.select2');
    }
    async function lookups() {
        const data = await request(endpoint + '/lookups');
        fill('itemClassId', data.itemClasses, 'ClassId', 'ClassDescription');
        fill('itemTypeId', data.itemTypes, 'Id', 'TypeDescription');
        fill('itemCategoryId', data.itemCategories, 'Id', 'CategoryDescription');
        fill('purchaseGLAC', data.glAccounts, 'PurchaseGLAC', 'StockGLAccountTitle');
        fill('saleGLAC', data.glAccounts, 'SaleGLAC', 'SalesGLAccountTitle');
        fill('cogsGLAC', data.glAccounts, 'COGSGLAC', 'CgsGLAccountTitle');
    }

    /* ---- grid ---- */
    function cell(row, content, tag = 'td') { const c = document.createElement(tag); c.textContent = content; row.append(c); return c; }
    function select(index) {
        const rows = $id('rows').rows;
        if (!rows.length) { current = -1; $id('navText').textContent = 'Record 0 of 0'; return; }
        current = Math.max(0, Math.min(index, rows.length - 1));
        for (let i = 0; i < rows.length; i++) rows[i].classList.toggle('selected', i === current);
        $id('navText').textContent = 'Record ' + (current + 1) + ' of ' + rows.length;
    }
    function render(list) {
        shown = list;
        $id('rows').replaceChildren(); $id('totals').replaceChildren();
        const fragment = document.createDocumentFragment();
        list.forEach((item, index) => {
            const row = document.createElement('tr'); row.tabIndex = 0;
            row.onclick = () => select(index);
            row.onkeydown = e => { if (['ArrowUp', 'ArrowDown'].includes(e.key)) { e.preventDefault(); const next = index + (e.key === 'ArrowUp' ? -1 : 1); select(next); $id('rows').rows[current]?.focus(); } };
            visible.forEach(([key]) => {
                if (key === 'IS Active') {
                    const td = cell(row, ''); td.className = 'check';
                    const box = document.createElement('input'); box.type = 'checkbox'; box.disabled = true; box.checked = String(item[key]).toLowerCase() === 'true' || item[key] === '1';
                    box.setAttribute('aria-label', 'IS Active'); td.append(box);
                } else cell(row, item[key]);
            });
            fragment.append(row);
        });
        $id('rows').append(fragment);
        /* TotalRow = True with no aggregate set on any column: an empty total row. */
        const total = document.createElement('tr');
        visible.forEach(() => cell(total, ''));
        $id('totals').append(total);
        select(list.length ? 0 : -1);
    }
    function filter() {
        const inputs = [...$id('head').querySelectorAll('input[data-key]')];
        render(view.filter(row => inputs.every(input => !input.value || String(row[input.dataset.key]).toLowerCase().includes(input.value.toLowerCase()))));
    }
    /* GridBind(): dt.Rows.Add(SeqNo, SeqNo, ItemCode, ItemName, ItemName, UOMCode, GlSaleAccountCode, CategoryDescription,
       TypeDescription, ClassDescription, ClassDescription, GlStockAccountTitle, GlSaleAccountTitle, GlCgsAccountTitle, ItemStatus) */
    function toView(r) {
        const out = {};
        columns.forEach(([key, , source]) => {
            let v = value(r, source);
            if (key === 'IS Active') v = v === true || v === 1 || String(v).toLowerCase() === 'true' ? 'True' : v === null || v === undefined ? '' : 'False';
            out[key] = v === null || v === undefined ? '' : String(v);
        });
        return out;
    }
    async function gridBind() {
        status('Loading items…');
        const records = await request(endpoint);
        view = records.map(toView);
        filter();
        status(records.length ? `${records.length} records` : '');
    }
    /* Reset(): every combo text cleared, Is Active unchecked, focus on Item Category */
    function reset() {
        combos.forEach(id => $($id(id)).val('').trigger('change.select2'));
        $id('itemStatus').checked = false;
        status('');
        $('#itemCategoryId').select2('focus');
    }
    /* ShowReport(): Item.RptItemList with every filter; no rows -> "Not Record Found For Display";
       Reporting.ShowReportWithDataTable(dt, "200-InvRptItemsList.rpt") with @CompanyName / @CompanyAddress (added by the server). */
    async function print() {
        const body = {itemStatus: $id('itemStatus').checked};
        combos.forEach(id => { body[id] = $id(id).value ? Number($id(id).value) : null; });
        const rows = await request(endpoint + '/print-rows', {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(body)});
        if (!rows || !rows.length) { alert('Not Record Found For Display'); return; }
        const win = window.open('about:blank', '_blank');
        try {
            const r = await fetch('/reports/print/grid', {method: 'POST', credentials: 'same-origin', headers: csrfHeaders({'Content-Type': 'application/json'}), body: JSON.stringify({rpt: '200-InvRptItemsList.rpt', title: null, rows})});
            const type = r.headers.get('Content-Type') || '';
            if (r.ok && type.startsWith('application/pdf')) { const b = await r.blob(); if (win) win.location = URL.createObjectURL(b); return; }
            const t = await r.text(); if (win) win.close(); alert(t || ('Print failed (' + r.status + ')'));
        } catch (e) { try { win && win.close(); } catch (x) { } alert(e.message); }
    }

    /* grid header + Janus automatic filter row */
    const header = document.createElement('tr'), filters = document.createElement('tr');
    header.className = 'heads'; filters.className = 'column-filters';
    visible.forEach(([key, width]) => {
        const col = document.createElement('col'); col.style.width = width + 'px'; $id('cols').append(col);
        const th = cell(header, key, 'th'); th.dataset.col = key; th.title = key;
        const input = document.createElement('input'); input.dataset.key = key; input.setAttribute('aria-label', 'Filter ' + key); input.oninput = filter;
        cell(filters, '', 'th').append(input);
    });
    $id('head').append(header, filters);

    $id('filters').addEventListener('submit', event => { event.preventDefault(); run($id('show'), 'Show', gridBind); });
    $id('new').addEventListener('click', () => run($id('new'), 'New', async () => { reset(); await gridBind(); }));
    $id('print').addEventListener('click', () => run($id('print'), '200-Print', print));
    $id('navFirst').onclick = () => select(0); $id('navPrev').onclick = () => select(current - 1);
    $id('navNext').onclick = () => select(current + 1); $id('navLast').onclick = () => select(shown.length - 1);
    /* frmRptItemList_KeyDown: Enter -> Tab, Ctrl+P print, Ctrl+N new, Ctrl+E / Escape close */
    document.addEventListener('keydown', event => {
        if (document.querySelector('.select2-container--open')) return;
        const key = event.key.toLowerCase();
        if (event.ctrlKey && key === 'p') { event.preventDefault(); $id('print').click(); return; }
        if (event.ctrlKey && key === 'n') { event.preventDefault(); $id('new').click(); return; }
        if (key === 'escape' || (event.ctrlKey && key === 'e')) { event.preventDefault(); if (!busy) location.assign('/inventory'); return; }
        if (key === 'enter' && event.target.closest && event.target.closest('#filters') && event.target.id !== 'show') {
            event.preventDefault();
            const focusable = [...document.querySelectorAll('#filters .select2-selection, #filters input, #filters button')].filter(e => !e.disabled);
            const at = focusable.indexOf(event.target);
            focusable[(at + 1) % focusable.length]?.focus();
        }
    });

    $('#filters select').each(function () { $(this).select2({width: '100%', minimumResultsForSearch: 0, placeholder: '', allowClear: true, dropdownAutoWidth: true}); });
    render([]);
    run(null, 'Loading', lookups);
})();
