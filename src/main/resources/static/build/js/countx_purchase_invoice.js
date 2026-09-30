/* InvfrmPurchaseInvoice (screen 122, DocumentTypeId 56) - GRN-backed purchase invoice.
 * Layout, grid columns and events follow the desktop form; amounts, validations and saving are done on the server
 * (/api/purchase/purchase-invoice-full, PurchaseInvoiceFullService / PurchaseInvoiceSaveRules). */
const purchaseInvoice = (() => {
    'use strict';
    const base = '/api/purchase/purchase-invoice-full', route = '/purchase/purchase-invoice', SCREEN_ID = 122, DOC_TYPE = 56;
    const state = {
        id: 0, h: {}, details: [], parts: { freight: [], journal: [], expenses: [], emptyBags: [], paymentTerms: [] },
        lists: {}, rights: {}, config: {}, voucherHeadId: 0, approved: false, invoiceType: 1, paymentByPercent: true,
        hideColumns: false, template: 1, firstNew: true, pending: Promise.resolve(), error: null, generation: 0,
        historyRows: [], historySelected: 0, loaderRows: [], loaderSelected: 0
    };
    const el = id => document.getElementById(id);
    const get = (r, k) => r?.[k] ?? r?.[Object.keys(r || {}).find(x => x.toLowerCase() === String(k).toLowerCase())];
    const num = (r, k) => Number(get(r, k)) || 0;
    const esc = v => String(v ?? '').replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    const date = v => String(v || '').slice(0, 10);
    const ymd = d => `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
    const today = () => ymd(new Date());
    const fmt = v => { if (v === null || v === undefined || v === '') return ''; const n = Number(v); return Number.isFinite(n) ? n.toLocaleString('en-US', { maximumFractionDigits: 3 }) : String(v); };
    const round4 = v => { const n = Number(v); return Number.isFinite(n) ? Math.round(n * 10000) / 10000 : v; };

    // ------------------------------------------------------------------ messages / requests
    function message(text = '', error = false, target = 'fxMsg') { const m = el(target); if (!m) return; m.textContent = text; m.classList.toggle('is-error', !!error && !!text); }
    const fail = (e, target) => message(e?.message || String(e), true, target);
    async function request(url, body, method) {
        return PurchaseRequest.track(async () => {
            const r = await fetch(url, { method: method || (body ? 'POST' : 'GET'), credentials: 'same-origin', headers: { Accept: 'application/json', ...(body ? { 'Content-Type': 'application/json' } : {}) }, ...(body ? { body: JSON.stringify(body) } : {}) });
            if (r.redirected || r.status === 401) throw Error('Please sign in to continue');
            const text = await r.text(); let data = null; try { data = text ? JSON.parse(text) : null; } catch (x) { data = { message: text }; }
            if (!r.ok || data?.success === false) throw Error(data?.message || data?.detail || data?.error || 'The request could not be completed');
            return data;
        });
    }
    const api = (path, body, method) => request(base + path, body, method);
    async function run(button, work, target) { try { await PurchaseRequest.run(button, work); } catch (e) { fail(e, target); } finally { rights(); } }
    function enqueue(work) {
        const previous = state.pending, generation = state.generation;
        state.pending = PurchaseRequest.track(async () => {
            await previous;
            try { if (generation !== state.generation) return; await work(generation); if (generation === state.generation) state.error = null; }
            catch (e) { if (generation === state.generation) { state.error = e; fail(e); } }
            finally { rights(); }
        });
        return state.pending;
    }
    async function settled() { let p; do { p = state.pending; await p; } while (p !== state.pending); if (state.error) throw state.error; }

    // ------------------------------------------------------------------ combos
    function option(value, label, data) { const o = new Option(String(label ?? ''), String(value ?? '')); for (const [k, v] of Object.entries(data || {})) if (v !== undefined && v !== null) o.dataset[k] = v; return o; }
    function fill(id, rows, key, label, blank = true, data) { const s = el(id); s.replaceChildren(...(blank ? [option('', '')] : []), ...(rows || []).map(r => option(get(r, key), get(r, label), data ? data(r) : null))); }
    function choose(id, v, label) {
        const node = el(id); if (!node) return;
        if (node.tagName === 'SELECT') {
            if (v === 0 || v === '0' || v === null || v === undefined) v = '';
            if (v !== '' && !Array.from(node.options).some(o => o.value === String(v))) node.add(option(v, label ?? v));
        }
        node.value = v ?? '';
    }
    const partyData = r => ({ code: get(r, 'PartyCode'), city: get(r, 'CityName'), mobile: get(r, 'MobileNo') ?? get(r, 'MobilePersonal') });
    const accountKey = () => state.lists.subsidiary ? 'Id' : 'ChartOfAccountId';
    const accountCaption = () => state.lists.subsidiary ? 'CompanyName' : 'AccountTitle';
    const TYPES = [{ Id: 1, Type: 'Normal' }, { Id: 2, Type: 'Purchase Against Weight' }, { Id: 3, Type: 'Free of Cost' }];

    function bind(data) {
        state.lists = data; state.rights = data.rights || {}; state.config = data.configuration || {};
        // BindSupplierName:1038 - supplier, commission agent and broker share dtSupplier (CustomerGroupId 7 removed).
        for (const id of ['cmbsuppliername', 'cmbcommagent', 'CmbBrokeryAc']) { const keep = el(id).value; fill(id, data.suppliers, 'Id', 'CompanyName', true, partyData); choose(id, keep); }
        { const keep = el('CmbPaymentTerm').value; fill('CmbPaymentTerm', data.paymentTerms, 'Id', 'TermsDescription', true); choose('CmbPaymentTerm', keep); }
        { const keep = el('cmbLocationType').value; fill('cmbLocationType', data.locations, 'Id', 'Location', true); choose('cmbLocationType', keep); }
        for (const id of ['cmbcommtype', 'CmbBrokeryType']) { const keep = el(id).value; fill(id, ['Flat', 'Percent', 'Comm Weight'].map(v => ({ v })), 'v', 'v', true); choose(id, keep); }
        for (const id of ['cmbcommuom', 'CmbBrokeryRateUom']) { const keep = el(id).value; fill(id, data.commissionUoms, 'type', 'type', true); choose(id, keep); }
        { const keep = el('CmbFreightAccount').value; fill('CmbFreightAccount', data.freightAccounts, accountKey(), accountCaption(), true); choose('CmbFreightAccount', keep); }
        fill('cmbSupplierNameHistory', data.historySuppliers, 'Id', 'ReferenceName', true);
        // ImplementConfiguration:947 - branch serial box only with the branch feature and branch-wise invoices.
        const branchSr = !!data.branchFeature && !!state.config.PurchaseInvoiceBranchWise;
        el('txtBranchCode').hidden = !branchSr; el('txtdocno').style.width = (branchSr ? 80 : 137) + 'px';
        el('DocDate').disabled = !!state.config.ValidateGrnAndInvoiceDateWithGpDate;
        multi('cmbBranchName', data.historyBranches || [], state.config.PurchaseInvoiceBranchWise, loadHistorySuppliers);
    }

    // ------------------------------------------------------------------ grid definitions (desktop column order and captions)
    // dtGrid (:541-584) + grdSettings/grdCommonSetting (:1416-1539). kind: n = number, r = rate, d = date, grn / po = links.
    const DETAIL = [
        ['GrnNo', 'GrnNo', 'grn'], ['WareHouseName', 'WareHouseName'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'], ['Pack UOM', 'UOMCodeItem'],
        ['ItemQty', 'ItemQty', 'n'], ['Order No', 'PurchaseOrder', 'po'], ['GrossWeight', 'GrossWeight', 'n'], ['EbTotal', 'EBTotalWt', 'n'],
        ['EbPurAgainstWeight', 'EbPurAgainstWeight', 'n'], ['WtCutTotal', 'WeightCutTotal', 'n'], ['Less Weight', 'AdLsWeight', 'n'], ['NetBillWeight', 'NetBillWeight', 'n'],
        ['StockWeight', 'NetStockWeight', 'n'], ['Item Rate', 'ItemRate', 'r'], ['RateUom', 'RateUom'], ['Rate UOM', 'EquivalentPoRate', 'r'], ['ItemAmount', 'ItemAmount', 'n'],
        ['RateCut', 'RateCut', 'r'], ['RateCutAmount', 'RateCutAmount', 'n'], ['Item Net Amount', 'BillAmount', 'n'], ['Freights', 'FreightAmount', 'n'], ['Expense', 'ExpenseAmount', 'n'],
        ['Commission', 'CommissionAmount', 'n'], ['Lab No', 'LabAnalisysNo'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'], ['GpDate', 'GpDate', 'd'], ['WagesAmount', 'WagesAmount', 'n'],
        ['BranchName', 'BranchName'], ['Empty Bags (Charge to Product)', 'EbPurAgainstWeightAmount', 'n'], ['FreightDeduction', 'FreightDeduction', 'n']
    ];
    // Special right 10 (HideColumnsFromDetailGridSpecialRight) -> hiddenColumnsExtra (:1480-1485).
    const HIDDEN_EXTRA = new Set(['WareHouseName', 'PurchaseOrder', 'EbPurAgainstWeight', 'WeightCutTotal', 'NetStockWeight', 'ItemAmount', 'EquivalentPoRate', 'RateCut', 'RateCutAmount', 'FreightAmount', 'ExpenseAmount', 'CommissionAmount', 'GpNo', 'LabAnalisysNo', 'GpDate', 'WagesAmount', 'FreightDeduction']);
    function detailVisible(key, rows) {
        const type = state.invoiceType, flags = state.config;
        if (key === 'BranchName') return !!state.lists.branchFeature && !flags.PurchaseInvoiceBranchWise;
        if (key === 'PurchaseOrder' && type === 1) return true;
        if (key === 'RateUom') return !(type === 2 || ([3, 4].includes(type) && flags.RateEditableOnPurchaseInvoice_InGatePurchase));
        if (key === 'EquivalentPoRate') return type === 2 || ([3, 4].includes(type) && !!flags.RateEditableOnPurchaseInvoice_InGatePurchase);
        if (key === 'PurchaseOrder') return rows.some(r => num(r, 'PurchaseOrder') > 0);
        return !(state.hideColumns && HIDDEN_EXTRA.has(key));
    }
    function detailEditable(key) {
        if (!allowed()) return false;
        const type = state.invoiceType, flags = state.config;
        if (key === 'RateCut') return true;
        if (key === 'AdLsWeight') return !!flags.WeightAddLessOnPurchaseInvoice;
        if (key === 'ItemRate' || key === 'EquivalentPoRate') return type === 2 || ([3, 4].includes(type) && !!flags.RateEditableOnPurchaseInvoice_InGatePurchase);
        return false;
    }
    const link = (path, id, caption) => id > 0 ? `<a href="${path}?id=${id}" target="_blank" rel="noopener">${esc(caption)}</a>` : esc(caption);
    function cellText(r, [, key, kind]) {
        const v = get(r, key);
        if (kind === 'grn') return link('/purchase/goods-receipt-notes', num(r, 'InvGrnId'), v);
        if (kind === 'po') return link('/purchase/purchase-order', num(r, 'PurchaseOrderId'), v);
        if (kind === 'd') return esc(date(v));
        if (kind === 'n' || kind === 'r') return esc(fmt(v));
        return esc(v);
    }
    function totals(cols, rows, sumKinds = ['n']) {
        if (!rows.length) return '';
        return '<tr>' + cols.map(c => '<td>' + (sumKinds.includes(c[2]) ? esc(fmt(rows.reduce((s, r) => s + num(r, c[1]), 0))) : '') + '</td>').join('') + '</tr>';
    }
    function renderDetails() {
        const rows = state.details, cols = DETAIL.filter(c => detailVisible(c[1], rows));
        const grd = el('grd');
        grd.tHead.innerHTML = '<tr>' + cols.map(c => `<th>${esc(c[0])}</th>`).join('') + '</tr>';
        grd.tBodies[0].innerHTML = rows.map((r, index) => '<tr>' + cols.map(c => {
            const key = c[1], kind = c[2], cls = kind === 'n' || kind === 'r' ? ' class="n"' : '';
            if (detailEditable(key)) {
                if (key === 'EquivalentPoRate' && state.invoiceType === 2)
                    return `<td><select class="win-combo" data-line="${index}" data-field="${key}" aria-label="Rate UOM row ${index + 1}">${options(state.h.rateUoms?.[num(r, 'ItemId')] || [], 'Equivalent', 'UOMCode', get(r, key))}</select></td>`;
                return `<td><input type="number" step="any" data-line="${index}" data-field="${key}" value="${esc(get(r, key))}" aria-label="${esc(c[0])} row ${index + 1}"></td>`;
            }
            return `<td${cls}>${cellText(r, c)}</td>`;
        }).join('') + '</tr>').join('');
        grd.tFoot.innerHTML = totals(cols, rows);
    }
    function options(rows, key, label, current, blank = true) {
        let html = blank ? '<option value=""></option>' : '';
        for (const r of rows) html += `<option value="${esc(get(r, key))}" ${String(get(r, key)) === String(current ?? '') ? 'selected' : ''}>${esc(get(r, label))}</option>`;
        if (current !== undefined && current !== null && current !== '' && current !== 0 && !rows.some(r => String(get(r, key)) === String(current))) html += `<option selected value="${esc(current)}">${esc(current)}</option>`;
        return html;
    }

    // The four expense grids and the payment grid. Field kinds: account, item, type, bagAccount, term (combos), n, t (text), d (date), ro (read-only).
    const GRIDS = {
        // grdFreightSettings:1614 - Delete/Add buttons, Transporter ("AccountTitle", bound "Charge To Product"), %, Qty, Rate, Credit, Debit (only DebitAmountChargetoExpenseAcFreightGridPurchase), Remarks.
        freight: { table: 'grdFreight', buttons: true, cols: [['Charge To Product', 'account', 'account'], ['%', 'Percentage', 'n'], ['Qty', 'FrQty', 'n'], ['Rate', 'FrRate', 'n'], ['Credit', 'FreightAmount', 'n'], ['Debit', 'Debit', 'n'], ['Remarks', 'Remarks', 't']] },
        // grdInvExpSettings:1876 - GrnNo, ItemId ("Other ItemName"), Qty, Rate, Amount, Remarks.
        expenses: { table: 'grdInvExp', buttons: true, cols: [['GrnNo', 'GrnNo', 'ro'], ['Other ItemName', 'InvRevExpItemId', 'item'], ['Qty', 'Qty', 'n'], ['Rate', 'Rate', 'n'], ['Amount', 'Amount', 'n'], ['Remarks', 'CustomRemarks', 't']] },
        // gridGLSettings:2014 - AccountId ("Supplier Add/Less"), Remarks, Percentage, Qty, Rate, Debit, Credit.
        journal: { table: 'grdGLedger', buttons: true, cols: [['Supplier Add/Less', 'account', 'account'], ['Remarks', 'JvRemarks', 't'], ['Percentage', 'JvPrcnt', 'n'], ['Qty', 'JvQty', 'n'], ['Rate', 'JvRate', 'n'], ['Debit', 'JvDebit', 'n'], ['Credit', 'JvCredit', 'n']] },
        // grdEmptyBagsSettings:2294 - TypeId ("Type"), ItemName, ItemCondition, PurchaseQty, Rate, Amount, Remarks, CreditAccountId ("Credit Title"). No buttons.
        emptyBags: { table: 'grdEmptyBags', buttons: false, cols: [['Type', 'TypeId', 'type'], ['ItemName', 'ItemName', 'ro'], ['ItemCondition', 'ItemCondition', 'ro'], ['PurchaseQty', 'PurchaseQty', 'ron'], ['Rate', 'Rate', 'n'], ['Amount', 'Amount', 'ron'], ['Remarks', 'CustomRemarks', 't'], ['Credit Title', 'CreditAccountId', 'bagAccount']] },
        // gridPaymentTermSetting:2542 - dtPaymentTerm (:633-649) with the two buttons.
        paymentTerms: { table: 'grdPaymentDetail', buttons: true, cols: [['PurchaseOrderNo', 'PurchaseOrderNo', 'ro'], ['ItemAmount', 'ItemAmount', 'ron'], ['Freight', 'Freight', 'ron'], ['Expense', 'Expense', 'ron'], ['PartyAddLessAmount', 'PartyAddLessAmount', 'ron'], ['EmptyBagAmount', 'EmptyBagAmount', 'ron'], ['- Freight Deduction', 'FreightDeduction', 'ron'], ['Commission', 'Commission', 'ron'], ['- Brokery Amount', 'BrokeryAmount', 'ron'], ['NetBillAmount', 'NetBillAmount', 'ron'], ['Payment Term', 'PaymentTermId', 'term'], ['DueDays', 'DueDays', 'n'], ['DueDate', 'DueDate', 'd'], ['%OfTotal', 'PrcntOfTotal', 'n'], ['Amount', 'Amount', 'n'], ['Remarks', 'PaymentRemarks', 't']] }
    };
    const BLANK = { freight: () => ({ Id: 0, InvGrnId: 0, FreightId: 0, TansporterId: 0, SupplierCustomerId: 0, Percentage: 0, FrQty: 0, FrRate: 0, FreightAmount: 0, Debit: 0, Remarks: '' }),
        expenses: () => ({ Id: 0, GrnId: 0, GrnNo: 0, InvRevExpItemId: 0, Qty: 0, Rate: 0, Amount: 0, CustomRemarks: '' }),
        journal: () => ({ Id: 0, InvGrnId: 0, FreightId: 0, ChartofAccountId: 0, SupplierCustomerId: 0, JvRemarks: '', JvPrcnt: 0, JvQty: 0, JvRate: 0, JvDebit: 0, JvCredit: 0, RowType: 0 }) };
    function accountValue(grid, r) { return state.lists.subsidiary ? num(r, 'SupplierCustomerId') : num(r, grid === 'freight' ? 'TansporterId' : 'ChartofAccountId'); }
    function gridCell(grid, r, index, [caption, key, kind]) {
        const attrs = `data-grid="${grid}" data-row="${index}" data-field="${key}" aria-label="${esc(caption)} row ${index + 1}"`;
        const grn = num(r, 'InvGrnId') > 0, rateCutRow = grid === 'journal' && num(r, 'RowType') === 1;
        let rows, value = kind === 'account' ? accountValue(grid, r) : get(r, key), vk, vl;
        if (kind === 'account') { rows = state.lists.accounts || []; vk = accountKey(); vl = accountCaption(); }
        if (kind === 'item') { rows = state.lists.otherItems || []; vk = 'Id'; vl = (rows[0] && get(rows[0], 'OtherItemName') !== undefined) ? 'OtherItemName' : 'ItemName'; }
        if (kind === 'type') { rows = TYPES; vk = 'Id'; vl = 'Type'; }
        if (kind === 'bagAccount') { rows = state.lists.bagCreditAccounts || []; vk = 'ChartOfAccountId'; vl = 'AccountTitle'; }
        if (kind === 'term') { rows = state.lists.paymentTerms || []; vk = 'Id'; vl = 'TermsDescription'; }
        // Account locked on GRN rows (freight: FreightId > 0; journal: not the excess-weight row) - :1760-1767, :2189-2211.
        // grdGLedger_Click:6765 - the excess-weight row (RowType 1) has Percentage/Qty/Debit/Credit read-only.
        let locked = !allowed() || kind === 'ro' || kind === 'ron'
            || (kind === 'account' && grn && (grid === 'journal' ? !rateCutRow : num(r, 'FreightId') > 0))
            || (rateCutRow && ['JvPrcnt', 'JvQty', 'JvDebit', 'JvCredit'].includes(key));
        if (rows) {
            if (locked) { const hit = rows.find(x => String(get(x, vk)) === String(value)); return esc(hit ? get(hit, vl) : (value || '')); }
            return `<select class="win-combo" ${attrs}>${options(rows, vk, vl, value)}</select>`;
        }
        if (locked) return kind === 'ron' ? esc(fmt(value)) : esc(value);
        if (kind === 'd') return `<input type="date" ${attrs} value="${esc(date(value))}">`;
        if (kind === 't') return `<input type="text" class="wide" ${attrs} value="${esc(value)}">`;
        return `<input type="number" step="any" ${attrs} value="${esc(round4(value ?? 0))}">`;
    }
    function renderGrid(grid) {
        const def = GRIDS[grid], rows = state.parts[grid], t = el(def.table);
        const cols = def.cols.filter(c => !(grid === 'freight' && c[1] === 'Debit' && !state.config.DebitAmountChargetoExpenseAcFreightGridPurchase));
        const buttons = def.buttons;
        t.tHead.innerHTML = '<tr>' + (buttons ? '<th>Delete</th><th>Add</th>' : '') + cols.map(c => `<th>${esc(c[0])}</th>`).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map((r, index) => '<tr>' + (buttons ? `<td class="c"><button type="button" class="gb" data-remove="${grid}" data-row="${index}" aria-label="Delete row ${index + 1}">X</button></td><td class="c"><button type="button" class="gb" data-add="${grid}" data-row="${index}" aria-label="Add row after ${index + 1}">+</button></td>` : '')
            + cols.map(c => `<td${['n', 'ron'].includes(c[2]) ? ' class="n"' : ''}>${gridCell(grid, r, index, c)}</td>`).join('') + '</tr>').join('');
        const sums = grid === 'paymentTerms' ? ['PrcntOfTotal', 'Amount'] : cols.filter(c => ['n', 'ron'].includes(c[2]) && !/Rate|%|Percentage|Prcnt/.test(c[0] + c[1])).map(c => c[1]);
        t.tFoot.innerHTML = rows.length ? '<tr>' + (buttons ? '<td></td><td></td>' : '') + cols.map(c => '<td>' + (sums.includes(c[1]) ? esc(fmt(rows.reduce((s, r) => s + num(r, c[1]), 0))) : '') + '</td>').join('') + '</tr>' : '';
    }
    function renderParts() { for (const g of Object.keys(GRIDS)) renderGrid(g); }

    // ------------------------------------------------------------------ rights / buttons
    function allowed() { return state.rights[state.id ? 'Update' : 'Save'] === true; }
    function rights() {
        const r = state.rights, loaded = state.id > 0, print = r.Print === true;
        el('btnSave').hidden = loaded; el('btnUpdate').hidden = !loaded; el('btnDelete').hidden = !loaded;
        el('btnSave').disabled = r.Save !== true || PurchaseRequest.isBusy(el('btnSave'));
        el('btnUpdate').disabled = r.Update !== true || PurchaseRequest.isBusy(el('btnUpdate'));
        el('btnDelete').disabled = r.Delete !== true || PurchaseRequest.isBusy(el('btnDelete'));
        for (const id of ['btnSlipDetail', 'btnSlip220A', 'btn220bSummary', 'btnPrint', 'btn104Voucher']) el(id).disabled = !print || PurchaseRequest.isBusy(el(id));
        el('cmbsuppliername').disabled = loaded || state.details.length > 0;
        el('txtGrnNo').disabled = loaded;
    }

    // ------------------------------------------------------------------ display
    const HEADER = { DocDate: 'DocDate', txtbillno: 'ManualBillNo', cmbsuppliername: 'SupplierCustomerId', CmbPaymentTerm: 'PaymentTermsId', txtDueDays: 'DueDays', DueDate: 'DueDate', datSupplierInvoiceDate: 'SupplierInvoiceDate', txtDeliveryTerm: 'DeliveryTerm', txtRefParty: 'ReferencePartyName', txtRemarks: 'RemarksHeader', cmbcommagent: 'CommissionAgentId', cmbcommtype: 'CommissionType', txtcommrate: 'CommRate', cmbcommuom: 'UomScheduleIdCmRate', txtcommremarks: 'CommissionRemarks', CmbBrokeryAc: 'BrokerAgentId', CmbBrokeryType: 'BrokeryType', txtBrokeryRate: 'BrokeryRate', CmbBrokeryRateUom: 'BrokeryUom', txtFreightDeduction: 'FreightAmount', txtWagesAmountHeader: 'WagesAmount' };
    const NUMERIC = new Set(['SupplierCustomerId', 'PaymentTermsId', 'DueDays', 'CommissionAgentId', 'CommRate', 'BrokerAgentId', 'BrokeryRate', 'BrokeryUom', 'FreightAmount', 'WagesAmount']);
    function header() {
        const h = { Id: state.id, InvoiceTypeId: state.invoiceType };
        for (const [id, key] of Object.entries(HEADER)) h[key] = NUMERIC.has(key) ? Number(el(id).value || 0) : el(id).value;
        h.LocationTypeId = Number(el('cmbLocationType').value || 0);
        const account = (state.lists.freightAccounts || []).find(r => num(r, accountKey()) === Number(el('CmbFreightAccount').value));
        h.TransportAccountId = account ? num(account, state.lists.subsidiary ? 'GlAccountId' : 'ChartOfAccountId') : 0;
        h.TransporterCreditPartyId = state.lists.subsidiary && account ? num(account, 'Id') : 0;
        h.DocumentTypeSrNo = Number(el('txtGrnNo').value || 0);
        h.CustomAccounts = el('chkCustomAccounts').checked;
        return h;
    }
    function payload() { return { ...header(), details: state.details, ...state.parts, paymentByPercent: state.paymentByPercent }; }
    function ensureBlankRows() { for (const g of ['freight', 'journal', 'expenses']) if (!state.parts[g].length) state.parts[g].push(BLANK[g]()); }

    function display(data, saved) {
        attachmentEditor.reset();
        state.id = saved ? num(data, 'Id') : 0; state.h = data; state.details = data.details || [];
        state.invoiceType = num(data, 'InvoiceTypeId') || 1; state.approved = get(data, 'IsApproved') === true || get(data, 'IsApproved') === 1;
        state.voucherHeadId = num(data, 'VoucherHeadId'); state.error = null; state.paymentByPercent = true;
        for (const g of Object.keys(GRIDS)) state.parts[g] = (data[g] || []).map(r => ({ ...r }));
        for (const r of state.parts.emptyBags) if (get(r, 'CustomRemarks') === undefined) r.CustomRemarks = get(r, 'Remarks') ?? '';
        for (const r of state.parts.expenses) if (get(r, 'CustomRemarks') === undefined) r.CustomRemarks = get(r, 'Remarks') ?? '';
        ensureBlankRows();
        for (const [id, key] of Object.entries(HEADER)) {
            let v = get(data, key);
            if (/Date$/.test(key)) v = date(v);
            if (key === 'RemarksHeader') v = get(data, 'OtherRemarks') ?? v;
            choose(id, v, id === 'cmbsuppliername' ? get(data, 'supplierName') : undefined);
        }
        el('chkCustomAccounts').checked = get(data, 'CustomAccounts') === true || get(data, 'CustomAccounts') === 1;
        choose('txtdocno', get(data, 'DocNo')); choose('txtBranchCode', get(data, 'BranchSrNo'));
        choose('txtBillAmount', fmt(get(data, 'BillAmount'))); choose('txtcommamount', fmt(get(data, 'CommAmount'))); choose('txtBrokeryAmount', fmt(get(data, 'BrokeryAmount')));
        choose('CmbFreightAccount', num(data, state.lists.subsidiary ? 'TransporterCreditPartyId' : 'TransportAccountId'));
        const location = num(state.details[0], 'LocationTypeId'); if (location > 0) choose('cmbLocationType', location);
        choose('txtGrnNo', num(data, 'DocumentTypeSrNo') || '');
        paymentTermLeave(false);
        el('lblDocCode').textContent = state.id ? '[' + (get(data, 'DocNo') ?? '') + ']' : '';
        renderDetails(); renderParts(); rights();
    }
    async function load(id) {
        const version = ++state.generation, data = await api('/' + Number(id));
        if (version !== state.generation) return;
        display(data, true); showTab('main', 'tabForm'); message();
        window.history.replaceState(null, '', route + '?id=' + state.id);
    }
    async function bill(generation = state.generation) {
        const data = await api('/calculate-bill', payload());
        if (generation !== state.generation) return;
        state.details = data.details; state.parts.paymentTerms = data.paymentTerms;
        choose('txtBillAmount', fmt(data.billAmount)); choose('txtcommamount', fmt(data.commAmount)); choose('txtBrokeryAmount', fmt(data.brokeryAmount));
        renderDetails(); renderGrid('paymentTerms');
    }
    // Reset():3245-3306.
    async function reset() {
        const version = ++state.generation; state.error = null;
        const code = await api('/next-code'); if (version !== state.generation) return;
        const keepLocation = el('cmbLocationType').value;
        // Reset() leaves DocDate and datSupplierInvoiceDate as they are.
        display({ DocNo: code.docNo, BranchSrNo: code.branchSrNo, DocDate: el('DocDate').value || today(), SupplierInvoiceDate: el('datSupplierInvoiceDate').value || today(), DueDate: today(), InvoiceTypeId: 1 }, false);
        choose('cmbLocationType', keepLocation);
        // Load: LocationTypeBind/PaymentTermBind activate Rows[1] (:995, :1068); Reset() then empties the payment term (:3286).
        if (state.firstNew) { const loc = state.lists.locations?.[0]; if (loc && !keepLocation) choose('cmbLocationType', get(loc, 'Id')); const pt = state.lists.paymentTerms?.[1]; if (pt) choose('CmbPaymentTerm', get(pt, 'Id')); state.firstNew = false; }
        else choose('CmbPaymentTerm', '');
        choose('txtDueDays', ''); choose('txtBillAmount', '0'); paymentTermLeave(false);
        window.history.replaceState(null, '', route);
    }
    const newRecord = button => run(button, reset);
    async function refresh(button) {
        await run(button, async () => { bind(await api('/dropdowns')); renderDetails(); renderParts(); });
    }

    // ------------------------------------------------------------------ save / delete
    async function save(button) {
        await run(button, async () => {
            await settled(); await attachmentEditor.settled();
            // btnUpdate_Click:4016, then Insert():3488-3503 - form checks, then FormHelper.ConfirmAction.
            if (state.id && state.approved) throw Error('Record Not Update because Record has approved');
            if (!state.details.length) throw Error('Grid Record Not Found');
            if (!el('cmbsuppliername').value) throw Error('Supplier Name field is required');
            if ((state.lists.locations || []).length && !el('cmbLocationType').value) throw Error('Location Type field is required');
            if (!confirm(state.id ? 'Are you sure to Update' : 'Are you sure to Save')) return;
            const data = await api('/save', { ...payload(), attachments: attachmentEditor.payload() });
            const savedId = data.id;
            await reset(); message(data.message);
            // :3955-3980 - prints after save by the header check boxes.
            if (el('ChkBoxParty').checked) print('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: savedId });
            if (el('ChkBoxItemPrint').checked) print('220-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: savedId });
            if (el('chkSummary').checked) print('220B-InvRptPurchaseBillSupplierRiceSummary.rpt', { id: savedId });
            if (el('ChkBokVoucher').checked) { const v = (await api('/' + savedId)).VoucherHeadId; print('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: v, documentTypeId: DOC_TYPE }); }
        });
    }
    async function remove(button) {
        if (!state.id) { message('RecordId Not Found.....', true); return; }
        if (state.approved) { message('Record Not Update because Record has approved', true); return; }
        if (!confirm('Are you sure to Delete?')) return;
        await run(button, async () => { await settled(); const r = await api('/' + state.id, null, 'DELETE'); await reset(); message(r.message || 'Delete Record Successfully'); });
    }

    // ------------------------------------------------------------------ prints (GeneratereportSlip:6547, CommonServices 103/220A, GenerateReportVoucher104:6479)
    function print(rpt, args) {
        const w = window.open('', '_blank'); try { if (w) w.document.write('<p style="font:13px Segoe UI">Preparing report...</p>'); } catch (x) { /* popup blocked */ }
        fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) })
            .then(r => (r.ok && (r.headers.get('Content-Type') || '').includes('application/pdf')) ? r.blob().then(b => { const u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); })
                : r.text().then(t => { if (w) w.close(); message(t || ('Print failed (' + r.status + ')'), true); }))
            .catch(e => { if (w) w.close(); fail(e); });
    }
    function slip(id, rpt) { if (!(id > 0)) { message('Record Id Not Found', true); return; } print(rpt, { id }); }
    function voucher103(voucherId) { if (!(voucherId > 0)) { message('Not Record Found For Display', true); return; } print('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: voucherId, documentTypeId: DOC_TYPE }); }
    function voucher104(voucherId) { if (!(voucherId > 0)) { message('VoucherId Not Found', true); return; } print('104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt', { id: voucherId, documentTypeId: DOC_TYPE }); }

    // ------------------------------------------------------------------ template (ChangeTemplate:1290)
    function changeTemplate(t) {
        state.template = t; el('rdTemplate1').checked = t === 1; el('rdTemplate2').checked = t === 2;
        const front = t === 1;
        el('templateFront').hidden = !front; el('templateTabs').hidden = front;
        const place = { TransporterGridPanel: front ? 'slotFreight' : 'tabPage1', OtherItemExpensePanel: front ? 'slotExpense' : 'tabPage2', PartyAddLessGridPanel: front ? 'slotJournal' : 'tabPage3', EmptyBagGridPanel: front ? 'slotBags' : 'tabPage4' };
        for (const [panel, slot] of Object.entries(place)) el(slot).appendChild(el(panel));
    }

    // ------------------------------------------------------------------ header events
    // CmbPaymentTerm_Leave:6660 - terms 1 and 3 clear and disable Due Days / Due Date.
    function paymentTermLeave(clear = true) {
        const v = Number(el('CmbPaymentTerm').value || 0), off = v === 1 || v === 3;
        el('txtDueDays').disabled = off; el('DueDate').disabled = off;
        if (off && clear) { choose('txtDueDays', ''); dueDays(); }
    }
    // DuedaysCalculates:6629 - DocDate + DueDays, or today when Due Days is empty.
    function dueDays() {
        const days = el('txtDueDays').value.trim();
        if (days !== '' && el('DocDate').value) { const d = new Date(el('DocDate').value + 'T12:00:00'); d.setDate(d.getDate() + Number(days || 0)); choose('DueDate', ymd(d)); }
        else choose('DueDate', today());
    }

    // ------------------------------------------------------------------ grid edits
    async function editPart(edit, generation) {
        const { grid, index, key, value } = edit, previous = state.parts[grid][index]; if (!previous) return;
        const r = { ...previous, [key]: value };
        if (key === 'account') {
            const account = (state.lists.accounts || []).find(a => num(a, accountKey()) === Number(value));
            r[grid === 'freight' ? 'TansporterId' : 'ChartofAccountId'] = account ? num(account, state.lists.subsidiary ? 'GlAccountId' : 'ChartOfAccountId') : 0;
            // Save sends SupplierCustomerId = the combo value in both modes (Insert :3689, :3752).
            r.SupplierCustomerId = Number(value || 0); delete r.account;
            if (grid === 'journal' && Number(value || 0) > 0) {
                const supplierGl = num((state.lists.suppliers || []).find(s => num(s, 'Id') === Number(el('cmbsuppliername').value)), 'GlAccountId');
                const picked = state.lists.subsidiary ? Number(value) : r.ChartofAccountId;
                if (picked === (state.lists.subsidiary ? Number(el('cmbsuppliername').value) : supplierGl)) { renderGrid(grid); throw Error('Supplier Account Not select'); }
            }
        }
        let updated;
        if (grid === 'paymentTerms') {
            const rows = state.parts[grid].map((x, i) => i === index ? r : x);
            updated = await api('/payment-row', { rows, index, changed: key, docDate: el('DocDate').value });
            if (key === 'Amount') state.paymentByPercent = false; if (key === 'PrcntOfTotal') state.paymentByPercent = true;
        } else if (grid === 'emptyBags' && key !== 'Rate') updated = r;
        else updated = await api('/supplement-row', { grid, row: r, changed: key, itemTotal: state.details.reduce((s, d) => s + num(d, 'ItemAmount'), 0) });
        if (generation !== state.generation) return;
        state.parts[grid][index] = updated; renderGrid(grid); await bill(generation);
    }
    // grdFreight/grdInvExp/grdGLedger ColumnButtonClick (:1668, :1913, :2060) and grdPaymentDetail (:2619).
    function removeRow(grid, index, keyboard = false) {
        if (!allowed()) return;
        const rows = state.parts[grid], r = rows[index]; if (!r) return;
        if (grid === 'paymentTerms') {
            const order = num(r, 'PurchaseOrderId'); if (rows.filter(x => num(x, 'PurchaseOrderId') === order).length <= 1) return;
            rows.splice(index, 1); renderGrid(grid); enqueue(bill); return;
        }
        if ((grid === 'freight' || grid === 'journal') && num(r, 'InvGrnId') > 0) { message(keyboard ? 'Record cannot be deleted because this record against grn' : 'Record cannot be deleted because this record againt grn', true); return; }
        rows.splice(index, 1); if (!rows.length) rows.push(BLANK[grid]());
        renderGrid(grid); enqueue(bill);
    }
    function addRow(grid, index) {
        if (!allowed()) return;
        const rows = state.parts[grid];
        if (grid === 'paymentTerms') {
            const source = rows[index]; if (!source) return;
            const order = num(source, 'PurchaseOrderId'), bill = num(source, 'NetBillAmount');
            const group = order > 0 ? rows.filter(x => num(x, 'PurchaseOrderId') === order) : rows;
            const amount = bill - group.reduce((s, x) => s + num(x, 'Amount'), 0), percent = 100 - group.reduce((s, x) => s + num(x, 'PrcntOfTotal'), 0);
            if (amount > 0 && percent > 0) { rows.push({ ...source, Id: 0, Amount: amount, PrcntOfTotal: percent, SystemGeneratedRow: false }); state.paymentByPercent = false; renderGrid(grid); enqueue(bill); }
            else message(`Can't break further because Amount/Percent already reached Bill Amount: ${fmt(bill)}\nIf you want to break further, first adjust existing rows.`, true);
            return;
        }
        rows.push(BLANK[grid]()); renderGrid(grid);
    }

    // ------------------------------------------------------------------ GRN loader (frmLoadGRN) and txtGrnNo_Leave
    const LOADER_COLS = [['BranchName', 'BranchName'], ['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo', 'grnLink'], ['SupplierCustomer', 'SupplierCustomer'], ['DeliveryTerm', 'DeliveryTerm'], ['GpNO', 'GpNO'], ['GpDate', 'GpDate', 'd'], ['BiltyNo', 'BiltyNo'], ['VehicleNo', 'VehicleNo'], ['PurchaseOrder', 'PurchaseOrder', 'po'], ['PurchaseAgainst', 'PurchaseAgainst'], ['GrnStatus', 'GrnStatus']];
    const LOADER_DETAIL = [['Order', 'PurchaseOrder'], ['Item', 'ItemName'], ['CropYear', 'CropYear'], ['Job', 'JobLotDescription'], ['PackingType', 'PackTypeDesc'], ['UOM', 'UOMCode'], ['Qty', 'ItemQty', 'n'], ['GrossWight', 'GrossWeight', 'n'], ['EbUnit', 'EBWPerUnit', 'n'], ['EbTotal', 'EBWTotal', 'n'], ['EbPurAgainstWeight', 'EbPurAgainstWeight', 'n'], ['WtCut', 'WtCut', 'n'], ['WtCutTotal', 'WtCutTotal', 'n'], ['AddLesswt', 'AdLsWeight', 'n'], ['NetWeight', 'NetBillWeight', 'n'], ['StockWeight', 'StockWeight', 'n'], ['WareHouse', 'WareHouseName'], ['LabNo', 'LabReportRef'], ['City', 'AreaCity']];
    async function openLoader(button) {
        // BtnLoader_Click:6039 - in the loaded (Update) state the button resets the form instead.
        if (state.id) { await newRecord(button); return; }
        await run(button, async () => {
            await settled();
            el('frmLoadGRN').hidden = false;
            if (!state.loaderBranches) { state.loaderBranches = await request('/api/grn-loader/branches?docTypeId=46'); multi('ldrBranch', state.loaderBranches, state.config.PurchaseInvoiceBranchWise); }
            if (!el('ldrFromDate').value) resetLoaderDates();
            await showGrns(null);
        });
    }
    function resetLoaderDates() { choose('ldrFromDate', state.lists.financialYearStart || today()); choose('ldrToDate', today()); }
    async function showGrns(button) {
        await run(button, async () => {
            const branches = multiValue('ldrBranch'); if (!branches) throw Error('Select branch first');
            const q = new URLSearchParams({ docTypeId: '46', branchIds: branches });
            if (el('ldrFromDate').value) q.set('fromDate', el('ldrFromDate').value); if (el('ldrToDate').value) q.set('toDate', el('ldrToDate').value);
            state.loaderRows = await request('/api/grn-loader/pending?' + q); state.loaderSelected = 0;
            const t = el('ldrGrid');
            t.tHead.innerHTML = '<tr><th><input type="checkbox" id="ldrAll" aria-label="Select all GRNs"></th>' + LOADER_COLS.map(c => `<th>${c[0]}</th>`).join('') + '</tr>';
            t.tBodies[0].innerHTML = state.loaderRows.map((r, i) => `<tr data-ldr="${i}"><td class="c"><input type="checkbox" data-grn-id="${num(r, 'Id')}" aria-label="Select GRN ${esc(get(r, 'DocNo'))}"></td>` + LOADER_COLS.map(c => '<td>' + (c[2] === 'grnLink' ? link('/purchase/goods-receipt-notes', num(r, 'Id'), get(r, c[1])) : c[2] === 'po' ? link('/purchase/purchase-order', num(r, 'PurchaseOrderId'), get(r, c[1])) : c[2] === 'd' ? esc(date(get(r, c[1]))) : esc(get(r, c[1]))) + '</td>').join('') + '</tr>').join('');
            el('ldrDetail').tHead.innerHTML = ''; el('ldrDetail').tBodies[0].innerHTML = '';
            message(state.loaderRows.length + ' pending GRNs', false, 'ldrMsg');
        }, 'ldrMsg');
    }
    async function loaderDetail(index) {
        const r = state.loaderRows[index]; if (!r) return;
        await run(null, async () => {
            const rows = await request('/api/grn-loader/details/' + num(r, 'Id'));
            const t = el('ldrDetail');
            t.tHead.innerHTML = '<tr>' + LOADER_DETAIL.map(c => `<th>${c[0]}</th>`).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(d => '<tr>' + LOADER_DETAIL.map(c => `<td${c[2] ? ' class="n"' : ''}>${esc(c[2] ? fmt(get(d, c[1])) : get(d, c[1]))}</td>`).join('') + '</tr>').join('');
        }, 'ldrMsg');
    }
    async function draft(ids, grnNo) {
        const generation = ++state.generation;
        const data = await api('/load-grns', ids.map(Id => ({ Id })));
        if (generation !== state.generation) return;
        const code = { DocNo: el('txtdocno').value, BranchSrNo: el('txtBranchCode').value };
        const keepLocation = el('cmbLocationType').value;
        // LoadDataDetailGridAgainstGP:6146 - DocDate moves to the GRN date only with ValidateGrnAndInvoiceDateWithGpDate; the supplier invoice date is untouched.
        const docDate = state.config.ValidateGrnAndInvoiceDateWithGpDate ? date(get(data, 'DocDate')) : (el('DocDate').value || today());
        display({ ...data, ...code, DocDate: docDate, SupplierInvoiceDate: el('datSupplierInvoiceDate').value || today(), DocumentTypeSrNo: grnNo || 0 }, false);
        choose('cmbLocationType', keepLocation); dueDays();
        el('frmLoadGRN').hidden = true; message();
    }
    async function loadSelectedGrns(button) {
        await run(button, async () => {
            const ids = Array.from(document.querySelectorAll('#ldrGrid [data-grn-id]:checked')).map(x => Number(x.dataset.grnId));
            if (!ids.length) throw Error('Check the row first');
            await draft(ids);
        }, 'ldrMsg');
    }
    // txtGrnNo_Leave:6002 - GRN (type 46) by DocNo in the active year.
    async function grnByNumber() {
        const no = Number(el('txtGrnNo').value); if (!no || state.id || state.details.some(d => num(d, 'GrnNo') === no)) return;
        await run(null, async () => {
            await settled();
            const rows = await request('/api/grn-loader/pending?docTypeId=46');
            const found = rows.filter(r => num(r, 'DocNo') === no);
            if (found.length !== 1) throw Error(found.length ? 'Choose this GRN in the branch-filtered loader' : 'Record Not Found For Loader');
            await draft([num(found[0], 'Id')], no);
        });
    }

    // ------------------------------------------------------------------ multi-select branch combos (UltraCombo CheckedList)
    function multi(id, rows, singleOnly, onChange) {
        const list = el(id + 'List'), text = el(id + 'Text'), host = el(id);
        const current = String(get((rows || []).find(r => num(r, 'BranchId') === Number(state.lists.currentBranchId)), 'BranchId') ?? '');
        list.innerHTML = (rows || []).map((r, i) => `<label><input type="checkbox" value="${esc(get(r, 'BranchId'))}" ${(current ? String(get(r, 'BranchId')) === current : i === 0) ? 'checked' : ''}> ${esc(get(r, 'BranchName'))}</label>`).join('');
        host.querySelector('.fx-multi-btn').disabled = !!singleOnly;
        const sync = () => { text.textContent = Array.from(list.querySelectorAll('input:checked')).map(x => x.parentElement.textContent.trim()).join(','); };
        list.onchange = () => { sync(); if (onChange) onChange(); };
        host.querySelector('.fx-multi-btn').onclick = () => { list.hidden = !list.hidden; };
        sync();
    }
    const multiValue = id => Array.from(el(id + 'List').querySelectorAll('input:checked')).map(x => x.value).join(',');
    document.addEventListener('mousedown', e => { for (const id of ['cmbBranchName', 'ldrBranch']) if (!el(id).contains(e.target)) el(id + 'List').hidden = true; });

    // ------------------------------------------------------------------ History (GetAll:4932, HistoryGridSettings:5071)
    const HISTORY = [['BranchName', 'BranchName', 'branch'], ['BranchSrNo', 'BranchSrNo', 'branch'], ['OrderNo', 'OrderNo'], ['DocNo', 'DocNo', 'doc'], ['DocDate', 'DocDate', 'd'], ['PurchaseAgainst', 'PurchaseAgainst'], ['DueDays', 'DueDays'], ['DueDate', 'DueDate', 'd'], ['ManualBillNo', 'ManualBillNo'], ['SupplierName', 'SupplierName'], ['RefParty', 'ReferencePartyName'], ['CommAgent', 'CommissionAgent'], ['CommType', 'CommissionType'], ['CommRate', 'CommRate', 'n'], ['CommAmount', 'CommAmount', 'n'], ['CommRemarks', 'CommissionRemarks'], ['BillAmount', 'BillAmount', 'n'], ['VehicleNos', 'VehicleNos'], ['ApprovedStatus', 'ApprovedStatus'], ['EntryUser', 'EntryUser'], ['EntryDate', 'EntryDate', 'dt'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 'dt'], ['ApprovedUser', 'ApprovedUser'], ['ApprovedDate', 'PostDate', 'dt'], ['NoOfAttachments', 'NoOfAttachments'], ['Remarks', 'RemarksHeader']];
    const dt = v => { const s = String(v || ''); return s ? s.slice(0, 16).replace('T', ' ') : ''; };
    function historyVisible(c) { return c[2] !== 'branch' || (!!state.lists.branchFeature && !state.config.PurchaseInvoiceBranchWise); }
    function resetHistory() {
        choose('FromDateHistory', today()); choose('ToDateHistory', today()); el('chkFromDateHistory').checked = true; el('chkToDateHistory').checked = true;
        choose('txtFromDocNoHistory', ''); choose('txtToDocNoHistory', ''); choose('cmbSupplierNameHistory', '');
        for (const t of ['grdHistory', 'grdDetail']) { el(t).tHead.innerHTML = ''; el(t).tBodies[0].innerHTML = ''; el(t).tFoot.innerHTML = ''; }
        document.querySelector('input[name=histDate][value=DocDate]').checked = true; message('', false, 'historyMsg');
    }
    function historyDefaults() {
        const days = Number(state.lists.historyFromDays) > 0 ? Number(state.lists.historyFromDays) : 3, d = new Date(); d.setDate(d.getDate() - days);
        choose('FromDateHistory', ymd(d)); choose('ToDateHistory', today());
    }
    async function loadHistorySuppliers() {
        const branches = multiValue('cmbBranchName'); if (!branches) return;
        await run(null, async () => { const keep = el('cmbSupplierNameHistory').value; fill('cmbSupplierNameHistory', await api('/history-suppliers?branchIds=' + encodeURIComponent(branches)), 'Id', 'ReferenceName', true); choose('cmbSupplierNameHistory', keep); }, 'historyMsg');
    }
    async function history(button) {
        await run(button, async () => {
            const branches = multiValue('cmbBranchName'); if (!branches) throw Error('Select branch first');
            const q = new URLSearchParams({ branchIds: branches, dateType: document.querySelector('input[name=histDate]:checked').value });
            if (el('chkFromDateHistory').checked && el('FromDateHistory').value) q.set('fromDate', el('FromDateHistory').value);
            if (el('chkToDateHistory').checked && el('ToDateHistory').value) q.set('toDate', el('ToDateHistory').value);
            for (const [id, key] of [['txtFromDocNoHistory', 'fromDocNo'], ['txtToDocNoHistory', 'toDocNo'], ['cmbSupplierNameHistory', 'supplierId']]) if (Number(el(id).value) > 0) q.set(key, String(Number(el(id).value)));
            state.historyRows = await api('/history?' + q); state.historySelected = 0;
            const cols = HISTORY.filter(historyVisible), t = el('grdHistory');
            // AddButton: Item Slip, PartySlip, Voucher, SummaryReport, Edit at positions 0-4; Add Attachment last.
            t.tHead.innerHTML = '<tr><th>View</th><th>PartySlip</th><th>Voucher</th><th>SummaryReport</th><th>Edit</th>' + cols.map(c => `<th>${c[0]}</th>`).join('') + '<th>AddAttachment</th></tr>';
            const pr = state.rights.Print === true ? '' : ' disabled';
            t.tBodies[0].innerHTML = state.historyRows.map((r, i) => `<tr data-hist="${i}"><td><button type="button" data-hact="View"${pr}>Item Slip</button></td><td><button type="button" data-hact="PartySlip"${pr}>PartySlip</button></td><td><button type="button" data-hact="Voucher"${pr}>Voucher</button></td><td><button type="button" data-hact="SummaryReport"${pr}>SummaryReport</button></td><td><button type="button" data-hact="Edit">Edit</button></td>`
                + cols.map(c => `<td${c[2] === 'n' ? ' class="n"' : ''}>${c[2] === 'd' ? esc(date(get(r, c[1]))) : c[2] === 'dt' ? esc(dt(get(r, c[1]))) : c[2] === 'n' ? esc(fmt(get(r, c[1]))) : esc(get(r, c[1]))}</td>`).join('') + '<td><button type="button" data-hact="AddAttachment">Add Attachment</button></td></tr>').join('');
            t.tFoot.innerHTML = '';
            el('grdDetail').tHead.innerHTML = ''; el('grdDetail').tBodies[0].innerHTML = ''; el('grdDetail').tFoot.innerHTML = '';
            message(state.historyRows.length + ' records', false, 'historyMsg');
            if (state.historyRows.length) historyDetail(0);
        }, 'historyMsg');
    }
    // DetailGridBind:5246 / grdHistoryDetailSettings:5275 - EbPurAgainstWeightAmount hidden, JobLot and PackingType added.
    async function historyDetail(index) {
        const r = state.historyRows[index]; if (!r) return; state.historySelected = index;
        for (const tr of el('grdHistory').tBodies[0].rows) tr.classList.toggle('sel', Number(tr.dataset.hist) === index);
        await run(null, async () => {
            const rows = await api('/history-detail/' + num(r, 'Id') + '?branchIds=' + encodeURIComponent(multiValue('cmbBranchName')));
            const cols = [];
            for (const c of DETAIL) {
                if (c[1] === 'EquivalentPoRate' || c[1] === 'EbPurAgainstWeightAmount') continue;
                if (c[1] === 'BranchName' && !(state.lists.branchFeature && !state.config.PurchaseInvoiceBranchWise)) continue;
                if (c[1] === 'PurchaseOrder' && !rows.some(x => num(x, 'PurchaseOrder') > 0)) continue;
                cols.push(c);
                if (c[1] === 'ItemQty') cols.push(['PackingType', 'PackTypeDesc']);
                if (c[1] === 'ItemAmount') cols.push(['JobLot', 'JobLotDescription']);
            }
            const t = el('grdDetail');
            t.tHead.innerHTML = '<tr>' + cols.map(c => `<th>${esc(c[0])}</th>`).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(d => '<tr>' + cols.map(c => `<td${['n', 'r'].includes(c[2]) ? ' class="n"' : ''}>${cellText(d, c)}</td>`).join('') + '</tr>').join('');
            t.tFoot.innerHTML = totals(cols, rows);
        }, 'historyMsg');
    }
    function historyAction(action, index) {
        const r = state.historyRows[index]; if (!r) return; const id = num(r, 'Id');
        if (action === 'Edit') { run(null, () => load(id), 'historyMsg'); return; }
        if (action === 'Voucher') { voucher103(num(r, 'VoucherHeadId')); return; }
        if (action === 'View') { slip(id, '220-InvRptPurchaseBillSupplierRiceSlip.rpt'); return; }
        if (action === 'PartySlip') { slip(id, '220A-InvRptPurchaseBillSupplierRiceSlip.rpt'); return; }
        if (action === 'SummaryReport') { slip(id, '220B-InvRptPurchaseBillSupplierRiceSummary.rpt'); return; }
        if (action === 'AddAttachment') { run(null, () => load(id), 'historyMsg').then(() => { if (state.id === id) attachmentEditor.open(el('btnAttachment')); }); }
    }

    // ------------------------------------------------------------------ tabs / shortcuts
    function showTab(group, pane) {
        const bar = document.querySelector(`[data-tabs="${group}"]`);
        for (const b of bar.querySelectorAll('button')) { const on = b.dataset.pane === pane; b.classList.toggle('is-active', on); el(b.dataset.pane).classList.toggle('is-active', on); }
    }
    const SHORTCUTS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+S', 'Save (Form) Or Show Record (History)'], ['Ctrl+U', 'For Update'], ['Ctrl+T', 'For Transfer Tab'], ['Ctrl+F5', 'For Focus on Doc Date(Form) Or Branch Name (History)'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+L', 'For Load Records Of GRN'], ['ALT+1', 'For 220 Slip'], ['ALT+2', 'For 220A Slip'], ['ALT+3', 'For 220B Slip'], ['ALT+4', 'For 103 Slip'], ['ALT+5', 'For 104 Slip'], ['Ctrl+2', 'Template Change'], ['Ctrl+Delete', 'Freight,Expense,PartyAddLess Grids delete row '], ['Ctrl+D', 'Freight,Expense,PartyAddLess Grids Add new row '], ['Ctrl+Right', 'To Toggle between form grids'], ['Ctrl+ArrowDown', 'For Focus On detail grid (Form) Or Main Grid (History)'], ['Ctrl+ArrowUp', 'For Focus On Date From Date (history)'], ['Ctrl+Shift+Delete', 'For Delete Record in update case'], ['Ctrl+alt', 'To Show ShortCut Keys Form']];
    function shortcuts() { el('shortcutRows').innerHTML = SHORTCUTS.map(s => `<tr><td>${s[0]}</td><td>${s[1]}</td></tr>`).join(''); el('dlgShortcuts').hidden = false; }
    const onHistory = () => el('tabHistory').classList.contains('is-active');
    function keydown(e) {
        // InvfrmPurchaseInvoice_KeyDown:5296.
        if (e.key === 'Escape') { el('frmLoadGRN').hidden = true; el('dlgShortcuts').hidden = true; return; }
        if (e.ctrlKey && e.altKey && ['Control', 'Alt'].includes(e.key)) { shortcuts(); return; }
        const ctl = e.ctrlKey && !e.altKey, k = e.key.toLowerCase();
        if (e.altKey && !e.ctrlKey && /^[1-5]$/.test(e.key)) { e.preventDefault(); const b = el(['btnSlipDetail', 'btnSlip220A', 'btn220bSummary', 'btnPrint', 'btn104Voucher'][Number(e.key) - 1]); if (!b.disabled) b.click(); return; }
        if (!ctl) return;
        if (e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (!onHistory() && !el('btnDelete').hidden && !el('btnDelete').disabled) remove(el('btnDelete')); return; }
        if (e.key === 'Delete' || k === 'd') {
            const t = e.target, grid = t.dataset?.grid;
            if (grid && ['freight', 'expenses', 'journal'].includes(grid)) { e.preventDefault(); if (e.key === 'Delete') removeRow(grid, Number(t.dataset.row), true); else addRow(grid, Number(t.dataset.row)); }
            else if (grid === 'paymentTerms') { e.preventDefault(); if (e.key === 'Delete') removeRow(grid, Number(t.dataset.row)); else addRow(grid, Number(t.dataset.row)); }
            return;
        }
        if (e.code === 'Space' && e.target.dataset?.grid === 'paymentTerms') { e.preventDefault(); addRow('paymentTerms', Number(e.target.dataset.row)); return; }
        if (e.key === 'F10') { e.preventDefault(); if (!onHistory()) attachmentEditor.open(el('btnAttachment')); return; }
        if (e.key === 'F5') { e.preventDefault(); (onHistory() ? el('cmbBranchName').querySelector('button') : el('DocDate')).focus(); return; }
        if (e.key === '2') { e.preventDefault(); changeTemplate(state.template === 1 ? 2 : 1); return; }
        if (e.key === 'ArrowDown') { e.preventDefault(); const g = onHistory() ? el('grdHistory') : el('grd'); g.closest('.fx-grid-host').focus?.(); g.scrollIntoView({ block: 'nearest' }); return; }
        if (e.key === 'ArrowUp') { e.preventDefault(); (onHistory() ? el('FromDateHistory') : el('txtRefParty')).focus(); return; }
        const act = {
            n: () => onHistory() ? resetHistory() : newRecord(el('btnNew')),
            r: () => onHistory() ? refreshHistory(el('btnRefreshHistory')) : refresh(el('btnFrmRefresh')),
            s: () => onHistory() ? history(el('btnshow')) : (!el('btnSave').hidden && !el('btnSave').disabled && save(el('btnSave'))),
            u: () => !el('btnUpdate').hidden && !el('btnUpdate').disabled && save(el('btnUpdate')),
            t: () => showTab('main', onHistory() ? 'tabForm' : 'tabHistory'),
            l: () => !onHistory() && openLoader(el('BtnLoader')),
            e: () => { window.location.href = '/purchase/dashboard'; },
            enter: () => onHistory() && state.historyRows[state.historySelected] && run(null, () => load(num(state.historyRows[state.historySelected], 'Id')), 'historyMsg')
        }[k];
        if (act) { e.preventDefault(); act(); }
    }
    async function refreshHistory(button) { await run(button, async () => { const data = await api('/dropdowns'); state.lists.historyBranches = data.historyBranches; multi('cmbBranchName', data.historyBranches || [], state.config.PurchaseInvoiceBranchWise, loadHistorySuppliers); }, 'historyMsg'); await loadHistorySuppliers(); }

    // ------------------------------------------------------------------ special rights (SpecialRightsImplement:1207)
    function specialRights() {
        if (!window.SpecialRights) return;
        window.SpecialRights.mine(SCREEN_ID).then(rows => {
            for (const r of rows || []) {
                const on = get(r, 'IsActive') === true || get(r, 'IsActive') === 1, id = num(r, 'RightId');
                if (id === 1) el('ChkBoxItemPrint').checked = on; else if (id === 2) el('ChkBokVoucher').checked = on; else if (id === 7) el('ChkBoxParty').checked = on;
                else if (id === 5) { if (on) changeTemplate(1); } else if (id === 6) { if (on) changeTemplate(2); }
                else if (id === 10) { state.hideColumns = on; renderDetails(); }
            }
        }).fail?.(x => message(PurchaseRequest.error(x), true));
    }

    // ------------------------------------------------------------------ wiring
    const attachmentEditor = PurchaseInvoiceAttachments.create({ type: DOC_TYPE, getId: () => state.id, canEdit: allowed, message });
    function initEvents() {
        for (const bar of document.querySelectorAll('[data-tabs]')) bar.addEventListener('click', e => { const b = e.target.closest('button[data-pane]'); if (b) showTab(bar.dataset.tabs, b.dataset.pane); });
        el('btnNew').addEventListener('click', () => newRecord(el('btnNew')));
        el('btnFrmRefresh').addEventListener('click', () => refresh(el('btnFrmRefresh')));
        el('btnSave').addEventListener('click', () => save(el('btnSave')));
        el('btnUpdate').addEventListener('click', () => save(el('btnUpdate')));
        el('btnDelete').addEventListener('click', () => remove(el('btnDelete')));
        el('btnAttachment').addEventListener('click', () => attachmentEditor.open(el('btnAttachment')));
        el('btnSlipDetail').addEventListener('click', () => slip(state.id, '220-InvRptPurchaseBillSupplierRiceSlip.rpt'));
        el('btnSlip220A').addEventListener('click', () => slip(state.id, '220A-InvRptPurchaseBillSupplierRiceSlip.rpt'));
        el('btn220bSummary').addEventListener('click', () => slip(state.id, '220B-InvRptPurchaseBillSupplierRiceSummary.rpt'));
        el('btnPrint').addEventListener('click', () => voucher103(state.voucherHeadId));
        el('btn104Voucher').addEventListener('click', () => voucher104(state.voucherHeadId));
        el('BtnLoader').addEventListener('click', () => openLoader(el('BtnLoader')));
        el('btnShortcutKeys').addEventListener('click', shortcuts);
        el('BtnSpecialRights').addEventListener('click', () => { if (window.SpecialRights) window.SpecialRights.open(SCREEN_ID); });
        el('rdTemplate1').addEventListener('change', () => changeTemplate(1));
        el('rdTemplate2').addEventListener('change', () => changeTemplate(2));
        for (const b of document.querySelectorAll('[data-close]')) b.addEventListener('click', () => { el(b.dataset.close).hidden = true; });
        // Header events.
        el('DocDate').addEventListener('change', dueDays);
        el('txtDueDays').addEventListener('input', dueDays);
        el('DueDate').addEventListener('change', () => { const doc = el('DocDate').value, due = el('DueDate').value; if (doc && due) { if (due < doc) { choose('DueDate', doc); message('Due Date can\'t be less than Doc Date', true); } choose('txtDueDays', Math.round((Date.parse(el('DueDate').value) - Date.parse(doc)) / 86400000)); } });
        el('CmbPaymentTerm').addEventListener('change', () => paymentTermLeave(true));
        for (const id of ['cmbcommagent', 'cmbcommtype', 'txtcommrate', 'cmbcommuom', 'CmbBrokeryAc', 'CmbBrokeryType', 'txtBrokeryRate', 'CmbBrokeryRateUom', 'txtFreightDeduction', 'CmbFreightAccount', 'cmbsuppliername'])
            el(id).addEventListener(el(id).tagName === 'SELECT' ? 'change' : 'input', () => { if (state.details.length) enqueue(bill); });
        el('txtGrnNo').addEventListener('blur', grnByNumber);
        el('txtGrnNo').addEventListener('keydown', e => { if (e.key === 'Enter') { e.preventDefault(); grnByNumber(); } });
        // Detail grid edits (grd_CellUpdated:1541).
        el('grd').addEventListener('change', e => {
            const t = e.target; if (t.dataset.line === undefined) return;
            const index = Number(t.dataset.line), key = t.dataset.field, value = Number(t.value || 0);
            enqueue(async generation => { const updated = await api('/calculate-line', { line: { ...state.details[index], [key]: value }, InvoiceTypeId: state.invoiceType }); if (generation !== state.generation) return; state.details[index] = updated; await bill(generation); });
        });
        // Expense / payment grids.
        for (const g of Object.values(GRIDS)) {
            const t = el(g.table);
            t.addEventListener('change', e => { const x = e.target; if (!x.dataset.grid) return; const edit = { grid: x.dataset.grid, index: Number(x.dataset.row), key: x.dataset.field, value: x.type === 'number' || x.tagName === 'SELECT' ? Number(x.value || 0) : x.value }; enqueue(generation => editPart(edit, generation)); });
            t.addEventListener('click', e => { const add = e.target.closest('[data-add]'), del = e.target.closest('[data-remove]'); if (add) addRow(add.dataset.add, Number(add.dataset.row)); if (del) removeRow(del.dataset.remove, Number(del.dataset.row)); });
        }
        // Loader.
        el('ldrClose').addEventListener('click', () => { el('frmLoadGRN').hidden = true; });
        el('ldrSearch').addEventListener('click', () => showGrns(el('ldrSearch')));
        el('ldrLoad').addEventListener('click', () => loadSelectedGrns(el('ldrLoad')));
        el('ldrReset').addEventListener('click', () => { resetLoaderDates(); showGrns(el('ldrReset')); });
        el('ldrGrid').addEventListener('change', e => { if (e.target.id === 'ldrAll') for (const x of document.querySelectorAll('#ldrGrid [data-grn-id]')) x.checked = e.target.checked; });
        el('ldrGrid').addEventListener('click', e => { const tr = e.target.closest('tr[data-ldr]'); if (tr && !e.target.closest('a,input')) { for (const r of el('ldrGrid').tBodies[0].rows) r.classList.toggle('sel', r === tr); loaderDetail(Number(tr.dataset.ldr)); } });
        el('frmLoadGRN').addEventListener('keydown', e => { if (!e.ctrlKey) return; const k = e.key.toLowerCase(); if (k === 's') { e.preventDefault(); e.stopPropagation(); showGrns(el('ldrSearch')); } if (k === 'l') { e.preventDefault(); e.stopPropagation(); loadSelectedGrns(el('ldrLoad')); } if (k === 'n') { e.preventDefault(); e.stopPropagation(); resetLoaderDates(); } if (k === 'e') { e.preventDefault(); e.stopPropagation(); el('frmLoadGRN').hidden = true; } });
        // History.
        el('btnshow').addEventListener('click', () => history(el('btnshow')));
        el('btnNewHistory').addEventListener('click', resetHistory);
        el('btnRefreshHistory').addEventListener('click', () => refreshHistory(el('btnRefreshHistory')));
        el('grdHistory').addEventListener('click', e => { const b = e.target.closest('[data-hact]'), tr = e.target.closest('tr[data-hist]'); if (!tr) return; if (b) historyAction(b.dataset.hact, Number(tr.dataset.hist)); else historyDetail(Number(tr.dataset.hist)); });
        el('grdHistory').addEventListener('dblclick', e => { const tr = e.target.closest('tr[data-hist]'); if (tr && !e.target.closest('button')) run(null, () => load(num(state.historyRows[Number(tr.dataset.hist)], 'Id')), 'historyMsg'); });
        document.addEventListener('keydown', keydown);
    }
    document.addEventListener('DOMContentLoaded', async () => {
        initEvents(); changeTemplate(1); historyDefaults();
        await run(null, async () => {
            bind(await api('/dropdowns'));
            specialRights();
            const id = new URLSearchParams(window.location.search).get('id');
            if (id) { state.firstNew = false; await load(id); } else await reset();
        });
    });
    return { newRecord, refresh, save, remove, history, openLoader };
})();
