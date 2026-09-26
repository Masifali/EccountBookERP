/* ============================================================================================
 * Sale Invoice - InvfrmSaleInvoice (DocumentTypeId 95), ported event for event.
 *
 * The desktop keeps six DataTables (dtGrid, dtFreight, dtComm, dtGrdGL, dtInvExp, dtPaymentTerm) and
 * recomputes them in the grids' CellUpdated handlers. The same arrays and the same handlers live here,
 * named after their C# methods so each can be read against InvfrmSaleInvoice.cs. The server repeats
 * every validation and every proportion before it posts anything; this file is only the form.
 *
 * Rounding follows .NET Framework: Math.Round(x, n) is half-to-even on the scaled double and
 * Math.Round(x, n, AwayFromZero) steps a |fraction| >= 0.5 away from zero.
 * ============================================================================================ */
(function () {
    'use strict';
    const API = '/sale/sale-invoice/api';
    const DOCUMENT_TYPE_ID = 95;
    const el = id => document.getElementById(id);
    const esc = v => String(v ?? '').replace(/[&<>"']/g, c => ({'&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;'}[c]));
    const get = (r, k) => r == null ? undefined : (k in r ? r[k] : r[Object.keys(r).find(x => x.toLowerCase() === String(k).toLowerCase())]);

    /* ------------------------------------------------------------------ Conversion.* / Math.Round */
    function toDouble(v) { if (v === null || v === undefined || v === '') return 0; if (typeof v === 'number') return isFinite(v) ? v : 0; const n = Number(String(v).replace(/,/g, '').trim()); return isFinite(n) ? n : 0; }
    function rint(x) { const f = Math.floor(x), d = x - f; if (d > 0.5) return f + 1; if (d < 0.5) return f; return f % 2 === 0 ? f : f + 1; }
    /** Convert.ToInt32: a number rounds half-to-even, a string must be a whole number. */
    function toInt(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? rint(v) : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        const t = String(v).trim();
        return /^[+-]?\d+$/.test(t) ? parseInt(t, 10) : 0;
    }
    function toBool(v) { return v === true || v === 1 || String(v).toLowerCase() === 'true' || String(v) === '1'; }
    function round(v, d) { const p = Math.pow(10, d); return rint(v * p) / p; }
    function roundAway(v, d) { const p = Math.pow(10, d), s = v * p, w = Math.trunc(s), f = s - w; return (Math.abs(f) >= 0.5 ? w + Math.sign(f) : w) / p; }
    function fmt(v, dp) { const n = toDouble(v); return n.toLocaleString('en-US', {minimumFractionDigits: dp, maximumFractionDigits: dp}); }
    function fmtMax(v, max) { const n = toDouble(v); return n.toLocaleString('en-US', {minimumFractionDigits: 0, maximumFractionDigits: max}); }
    function today() { const d = new Date(); return iso(d); }
    function iso(d) { return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`; }
    function addDays(dateStr, n) { const d = new Date((dateStr || today()) + 'T12:00:00'); d.setDate(d.getDate() + n); return iso(d); }
    function dateOnly(v) { return v ? String(v).replace(' ', 'T').slice(0, 10) : ''; }
    function daysBetween(a, b) { return Math.round((Date.parse(b + 'T12:00:00') - Date.parse(a + 'T12:00:00')) / 86400000); }

    /* ------------------------------------------------------------------ state */
    const S = {
        L: null, cfg: {}, rights: {}, dp: 0,
        Id: 0, Approved: false, VoucherHeadId: 0, AttachmentsValues: '', CustomAttachmentsValues: '',
        dtGrid: [], dtFreight: [], dtComm: [], dtGrdGL: [], dtInvExp: [], dtPaymentTerm: [],
        LedgerBalance: 0, SupplierGLId: 0, ItemIdsForComm: '', CheckReservedNotReservedOnInsertOnly: '', hasSaleOrder: false,
        /* The hidden header commission controls (txtcommrate, cmbcommtype, cmbcommuom, txtcommamount):
           invisible on the desktop but still read by TotalCommissionAmount / LedgerProportion. */
        commRate: '', commTypeText: '', commUomText: '', commAmount: '0',
        commCurrent: 0, current: {}, PCalculateByPercent: false, PCalculateByAmount: true,
        orderNoList: null, busy: false
    };

    /* ------------------------------------------------------------------ http */
    async function api(path, body, method) {
        return PurchaseRequest.track(async () => {
            const r = await fetch(API + path, {method: method || (body ? 'POST' : 'GET'), headers: {Accept: 'application/json', ...(body ? {'Content-Type': 'application/json'} : {})}, ...(body ? {body: JSON.stringify(body)} : {})});
            if (r.redirected || r.status === 401) throw new Error('Please sign in to continue');
            const text = await r.text();
            let data = null; try { data = text ? JSON.parse(text) : null; } catch (e) { data = {message: text}; }
            if (!r.ok || (data && data.success === false)) throw new Error((data && (data.message || data.detail)) || 'The request could not be completed');
            return data;
        });
    }
    function message(text, ok) { const m = el('siMessage'); m.textContent = text || ''; m.classList.toggle('ok', !!ok); }
    async function run(button, work) {
        try { await PurchaseRequest.run(button, work); }
        catch (e) { message(e.message); alert(e.message); }
        finally { rights(); }
    }

    /* ------------------------------------------------------------------ value lists */
    const COMM_TYPES = [{Id: 1, CommissionType: 'Flat'}, {Id: 2, CommissionType: 'Percent'}, {Id: 3, CommissionType: 'Comm Weight'}];
    const subsidiary = () => !!S.cfg.SubsidiaryAccountAllownOnVouchers;
    function accountTitle(value) { const key = subsidiary() ? 'SupplierCustomerId' : 'Id'; const r = (S.L.accounts || []).find(a => toInt(get(a, key)) === toInt(value) && toInt(value) !== 0); return r ? get(r, 'AccountTitle') : (toInt(value) ? String(value) : ''); }
    /** The Text of a cell whose ValueList does not contain the value is the value itself. */
    function commTypeText(v) { const r = COMM_TYPES.find(x => String(x.Id) === String(v)); return r ? r.CommissionType : String(v ?? ''); }
    function commUomText(v) { const r = (S.L.commissionUoms || []).find(x => String(get(x, 'Id')) === String(v)); return r ? String(get(r, 'type')) : String(v ?? ''); }
    function glByTitle(title, previous) { if (!title || title === '0') return previous; const r = (S.L.accounts || []).find(a => get(a, 'AccountTitle') === title); return r ? toInt(get(r, 'Id')) : previous; }

    /* ------------------------------------------------------------------ grid definitions (grdSettings & friends) */
    const V = () => S.cfg;
    const cols = {
        grd: () => [
            ['SaleOrder', 'SaleOrder', 'i'], ['CategoryII', 'CategoryII'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'],
            ['BrandItemCode', 'BrandItemCode'], ['BrandItemName', 'BrandItemName'], ['CropYear', 'CropYear'], ['UOMCodeItem', 'Pack UOM'],
            ['ItemQty', 'ItemQty', 'q2', 'sum'], ['Warehouse', 'Warehouse'],
            ['GrossWeight', 'GrossWeight', 'q2', 'sum'], ['AddLsWt', 'AddLsWt', 'q2', 'sum'], ['EBWPerUnit', 'EbW Per Unit', 'q3', 'sum'],
            ['EmptyBagsDeduction', 'EmptyBagsDeduction', 'q2', 'sum'], ['NetBillWeight', 'NetBillWeight', 'q2', 'sum'], ['StockWeight', 'StockWeight', 'q2', 'sum'],
            ['ItemRateWOExp', 'Item Rate', 'rate'], ['RateUOM', 'RateUOM'],
            ['RateCut', 'RateCut', 'rate', null, () => V().RateCutAndRateCutAmountAddOnInvoice, () => V().RateCutAndRateCutAmountAddOnInvoice && canEdit()],
            ['RateCutAmount', 'RateCutAmount', 'amt', 'sum', () => V().RateCutAndRateCutAmountAddOnInvoice],
            ['ItemAmount', 'ItemAmount', 'amt', 'sum'], ['FcyAmount', 'FcyAmount', 'fcy', 'sum', () => V().HasMultiCurrencyFeature],
            ['BillAmount', 'Item Net Amount', 'amt', 'sum'], ['Freights', 'Freights', 'q3', 'sum'], ['Expense', 'Expense', 'q3', 'sum'],
            ['Commission', 'Commission', 'q3', 'sum'],
            ['VehicleNo', 'VehicleNo', 's', null, null, () => canEdit()], ['GpNo', 'GpNo', 'i', null, null, () => canEdit()],
            ['CommOnSale', 'CommOnSale', 'bool', null, () => V().EnableItemWiseCommOnSale],
            ['CommPercent', 'CommPercent', 'q2', 'sum', () => V().CommissionPolicyIsActive || V().CommissionPolicyForCommissionAgentIsActive],
            ['BranchName', 'BranchName', 's', null, () => V().BranchFeature && !V().SaleInvoiceBranchWise]
        ],
        freight: () => [
            ['Add', '+', 'add'], ['Delete', 'X', 'del'],
            ['Transporter', 'Transporter', 'list', null, null, () => canEdit(), () => (S.L.accounts || []).map(a => [get(a, subsidiary() ? 'SupplierCustomerId' : 'Id'), get(a, 'AccountTitle')])],
            ['Freight', 'Credit', 'amt', 'sum', null, () => canEdit()],
            ['Debit', 'Debit', 'amt', 'sum', () => V().DebitAmountChargetoExpenseAcFreightGrid, () => canEdit()],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()]
        ],
        comm: () => {
            const policy = V().CommissionPolicyIsActive || V().CommissionPolicyForCommissionAgentIsActive;
            const lock = !!V().CommissionEditableOnInvoice;
            const out = [];
            if (lock && !policy) out.push(['Add', '+', 'add'], ['Delete', 'X', 'del']);
            /* BindCommGridOrderNo turns the whole OrderNo column into a drop-down once the grid has orders. */
            out.push(['OrderNo', 'OrderNo', 'list', null, () => !policy, () => canEdit() && !!S.orderNoList && S.orderNoList.length > 0, () => (S.orderNoList || []).map(n => [n, n])]);
            out.push(['CommissionAgentId', 'Commission Agent', 'list', null, null, () => canEdit() && lock && !policy,
                () => subsidiary() ? (S.L.accounts || []).map(a => [get(a, 'SupplierCustomerId'), get(a, 'AccountTitle')]) : (S.L.customers || []).map(c => [get(c, 'Id'), get(c, 'CompanyName')])]);
            out.push(['CommType', 'CommType', 'list', null, () => !policy, () => canEdit() && lock, () => COMM_TYPES.map(t => [t.Id, t.CommissionType])]);
            out.push(['CommRate', 'CommRate', 'q3', 'avg', () => !policy, () => canEdit() && lock]);
            out.push(['CommUom', 'CommUom', 'list', null, () => !policy && S.commUomVisible !== false, () => canEdit() && lock, () => (S.L.commissionUoms || []).map(u => [get(u, 'Id'), get(u, 'type')])]);
            out.push(['CommissionAmount', 'CommissionAmount', 'q4', 'sum']);
            out.push(['DebitAccountId', 'Dr Account', 'list', null, () => !!V().DebitAmountChargetoExpenseAcOfCommission, () => canEdit() && (lock || !!V().DebitAmountChargetoExpenseAcOfCommission),
                () => subsidiary() ? (S.L.accounts || []).map(a => [get(a, 'SupplierCustomerId'), get(a, 'AccountTitle')]) : (S.L.commissionDebitAccounts || []).map(a => [get(a, 'Id'), get(a, 'AccountTitle')])]);
            out.push(['Remarks', 'Remarks', 's', null, null, () => canEdit()]);
            return out;
        },
        gl: () => [
            ['Add', '+', 'add'], ['Delete', 'X', 'del'],
            ['AccountId', 'Account', 'list', null, null, () => canEdit(), () => (S.L.accounts || []).map(a => [get(a, subsidiary() ? 'SupplierCustomerId' : 'Id'), get(a, 'AccountTitle')])],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()],
            ['Percentage', 'Percentage', 'q2', null, null, () => canEdit()],
            ['Qty', 'Qty', 'q3', 'sum', null, () => canEdit()],
            ['Rate', 'Rate', 'rate', null, null, () => canEdit()],
            ['Debit', 'Debit', 'amt', 'sum', null, () => canEdit()],
            ['Credit', 'Credit', 'amt', 'sum', null, () => canEdit()]
        ],
        exp: () => [
            ['Add', '+', 'add'], ['Delete', 'X', 'del'],
            ['ItemId', 'Item', 'list', null, null, () => canEdit(), () => (S.L.otherItems || []).map(o => [get(o, 'Id'), get(o, 'OtherItemName')])],
            ['Qty', 'Qty', 'q3', 'sum', null, () => canEdit()],
            ['Rate', 'Rate', 'rate', null, null, () => canEdit()],
            ['Amount', 'Amount', 'q2', 'sum', null, () => canEdit()],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()]
        ],
        pay: () => [
            ['Delete', 'X', 'del'], ['Add', '+', 'add'],
            ['SaleOrderNo', 'SaleOrderNo', 'i'],
            ['ItemAmount', 'ItemAmount', 'q3'], ['Expense', 'Expense', 'q3'], ['Freight', 'Freight', 'q3'],
            ['PartyAddLessAmount', 'PartyAddLessAmount', 'q3'], ['Commission', 'Commission', 'q3'], ['NetBillAmount', 'NetBillAmount', 'q3'],
            ['PaymentTerm', 'Payment Term', 'list', null, null, () => canEdit(), () => (S.L.paymentTerms || []).map(t => [get(t, 'Id'), get(t, 'TermsDescription')])],
            ['DueDays', 'DueDays', 'i', null, null, () => canEdit()],
            ['DueDate', 'DueDate', 'date', null, null, () => canEdit()],
            ['%OfTotal', '%OfTotal', 'q4', 'sum', null, () => canEdit()],
            ['Amount', 'Amount', 'q4', 'sum', null, () => canEdit()],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()]
        ]
    };
    const TABLES = {grd: 'dtGrid', freight: 'dtFreight', comm: 'dtComm', gl: 'dtGrdGL', exp: 'dtInvExp', pay: 'dtPaymentTerm'};
    const ELEMENTS = {grd: 'grd', freight: 'grdFreight', comm: 'grdComm', gl: 'grdGLedger', exp: 'grdInvExp', pay: 'grdPaymentDetail'};

    function canEdit() { return !!S.rights[S.Id ? 'Update' : 'Save']; }

    function cellText(kind, v) {
        switch (kind) {
            case 'amt': return fmt(v, S.dp);
            case 'fcy': return fmtMax(v, 4);
            case 'q2': return fmtMax(v, 2);
            case 'q3': return fmtMax(v, 3);
            case 'q4': return fmtMax(v, 4);
            case 'rate': return fmtMax(v, 4);
            case 'i': return toInt(v) === 0 && (v === '' || v == null) ? '' : String(v ?? '');
            case 'date': return dateOnly(v);
            default: return String(v ?? '');
        }
    }

    function renderGrid(key) {
        const table = el(ELEMENTS[key]), rows = S[TABLES[key]], defs = cols[key]().filter(c => !c[4] || c[4]());
        let html = '<thead><tr>' + defs.map(c => `<th>${esc(c[1])}</th>`).join('') + '</tr></thead><tbody>';
        rows.forEach((r, i) => {
            html += `<tr data-row="${i}" class="${key === 'comm' && i === S.commCurrent ? 'si-current' : ''}">`;
            for (const c of defs) {
                const [name, , kind, , , editFn, listFn] = c;
                if (kind === 'add') { html += `<td class="btn"><button type="button" data-act="add" data-grid="${key}" data-row="${i}" aria-label="Add row" ${canEdit() ? '' : 'disabled'}>+</button></td>`; continue; }
                if (kind === 'del') { html += `<td class="btn"><button type="button" data-act="del" data-grid="${key}" data-row="${i}" aria-label="Delete row ${i + 1}" ${canEdit() ? '' : 'disabled'}>X</button></td>`; continue; }
                const v = r[name];
                const editable = editFn ? !!editFn(r) : false;
                const label = `aria-label="${esc(c[1])} row ${i + 1}"`;
                if (kind === 'list') {
                    const list = listFn ? listFn() : [];
                    if (editable) {
                        let opts = '<option value="0"></option>', found = false;
                        for (const [val, txt] of list) { const sel = String(val) === String(v); found = found || sel; opts += `<option value="${esc(val)}" ${sel ? 'selected' : ''}>${esc(txt)}</option>`; }
                        if (!found && v !== undefined && v !== null && String(v) !== '' && String(v) !== '0') opts += `<option value="${esc(v)}" selected>${esc(v)}</option>`;
                        html += `<td><select data-grid="${key}" data-row="${i}" data-col="${name}" ${label}>${opts}</select></td>`;
                    } else {
                        const hit = list.find(([val]) => String(val) === String(v));
                        html += `<td>${esc(hit ? hit[1] : (toInt(v) === 0 && /^[0-9.]*$/.test(String(v ?? '')) ? '' : v))}</td>`;
                    }
                    continue;
                }
                if (kind === 'bool') { html += `<td class="btn"><input type="checkbox" ${toBool(v) ? 'checked' : ''} disabled ${label}></td>`; continue; }
                if (editable) {
                    const type = kind === 'date' ? 'date' : 'text';
                    const cls = ['s', 'date'].includes(kind) ? '' : 'n';
                    html += `<td><input type="${type}" class="${cls}" data-grid="${key}" data-row="${i}" data-col="${name}" value="${esc(kind === 'date' ? dateOnly(v) : (v ?? ''))}" ${label}></td>`;
                } else if (key === 'grd' && name === 'SaleOrder' && toInt(r.SaleOrderId) > 0) {
                    html += `<td><a href="/sale/sale-order?id=${toInt(r.SaleOrderId)}" target="_blank" rel="noopener">${esc(v)}</a></td>`;
                } else {
                    html += `<td class="${['s', 'date', 'i'].includes(kind) || !kind ? '' : 'n'}">${esc(cellText(kind, v))}</td>`;
                }
            }
            html += '</tr>';
        });
        html += '</tbody>';
        if (defs.some(c => c[3])) {
            html += '<tfoot><tr>' + defs.map(c => {
                if (!c[3]) return '<td></td>';
                const vals = rows.map(r => toDouble(r[c[0]]));
                const total = c[3] === 'avg' ? (vals.length ? vals.reduce((a, b) => a + b, 0) / vals.length : 0) : vals.reduce((a, b) => a + b, 0);
                return `<td class="n">${esc(cellText(c[2], total))}</td>`;
            }).join('') + '</tr></tfoot>';
        }
        table.innerHTML = html;
    }
    function renderAll() { for (const k of Object.keys(TABLES)) renderGrid(k); header(); }

    /* ------------------------------------------------------------------ header state */
    function header() {
        el('txtBillAmount').value = el('txtBillAmount').value || '0';
        rights();
    }
    function rights() {
        const r = S.rights || {};
        el('btnSave').hidden = !!S.Id; el('btnUpdate').hidden = !S.Id; el('btnDelete').hidden = !S.Id;
        el('btnSave').disabled = !r.Save || PurchaseRequest.isBusy(el('btnSave'));
        el('btnUpdate').disabled = !r.Update || PurchaseRequest.isBusy(el('btnUpdate'));
        for (const id of ['btnPrint', 'toolStripButton1', 'btnSlipFormat2', 'BtnItemSlip']) el(id).disabled = !r.Print;
        el('cmbsuppliername').disabled = !!S.Id || S.dtGrid.length > 0;
    }

    /* ------------------------------------------------------------------ AddRow* (the blank rows) */
    const blank = {
        freight: () => ({InvGdnId: 0, GlAccountId: 0, Transporter: 0, Freight: 0, Debit: 0, Remarks: ''}),
        comm: () => ({Id: 0, OrderId: 0, OrderNo: 0, InvoiceId: 0, CommissionAgentId: 0, CommType: 0, CommRate: 0, CommUom: 0, CommissionAmount: 0, DebitAccountId: 0, Remarks: '', GlAccountId: 0}),
        gl: () => ({AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0}),
        exp: () => ({ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: ''})
    };
    function addRow(key) { S[TABLES[key]].push(blank[key]()); }

    /* ------------------------------------------------------------------ configuration (GetConfigurationsFromGlobal / ImplementConfiguration) */
    function bindLookups(L) {
        S.L = L; S.cfg = L.settings || {}; S.rights = L.rights || {}; S.dp = toInt(S.cfg.DefaultNoofDecimalPointsForAmount);
        fillSelect('cmbLocationType', L.locationTypes, 'Id', 'Location', true);
        const loc = el('cmbLocationType');
        if ((L.locationTypes || []).length && !toInt(loc.value)) loc.selectedIndex = 1;
        fillSelect('cmbCurrency', L.currencies, 'Id', 'CurrencyCode', false);
        const cust = el('cmbsuppliername'), keep = cust.value;
        cust.replaceChildren(new Option('', ''), ...(L.customers || []).map(c => { const o = new Option(String(get(c, 'CompanyName') ?? ''), String(get(c, 'Id'))); o.dataset.code = get(c, 'PartyCode') ?? ''; o.dataset.city = get(c, 'CityName') ?? ''; o.dataset.mobile = get(c, 'MobileNo') ?? ''; o.dataset.gl = get(c, 'GlAccountId') ?? 0; return o; }));
        cust.value = keep;
        document.querySelectorAll('.si-mc').forEach(x => x.hidden = !S.cfg.HasMultiCurrencyFeature);
        document.querySelector('.si-panel8').classList.toggle('no-mc', !S.cfg.HasMultiCurrencyFeature);
        el('txtBranchSrNo').hidden = !(S.cfg.BranchFeature && S.cfg.SaleInvoiceBranchWise);
        if (!toInt(el('cmbCurrency').value)) el('cmbCurrency').value = String(S.cfg.BaseCurrencyId || '');
        if (!toDouble(el('txtExchangeRate').value)) el('txtExchangeRate').value = String(S.cfg.BaseCurrencyRate ?? '');
        /* History tab combos. */
        const branches = el('cmbBranchName');
        branches.innerHTML = (L.historyBranches || []).map(b => `<label><input type="checkbox" value="${esc(get(b, 'Id'))}" ${toInt(get(b, 'Id')) === toInt(L.userBranchId) ? 'checked' : ''} ${S.cfg.SaleInvoiceBranchWise ? 'disabled' : ''}> ${esc(get(b, 'BranchName'))}</label>`).join('');
        if (!branches.querySelector('input:checked')) { const f = branches.querySelector('input'); if (f) f.checked = true; }
        fillSelect('CmbCustomerHistory', L.historyCustomers, 'Id', 'ReferenceName', true);
        if (window.DesktopCombo) DesktopCombo.refresh();
    }
    function fillSelect(id, rows, key, label, blankFirst) {
        const node = el(id), keep = node.value;
        node.replaceChildren(...(blankFirst ? [new Option('', '0')] : []), ...(rows || []).map(r => new Option(String(get(r, label) ?? ''), String(get(r, key) ?? ''))));
        if (keep && Array.from(node.options).some(o => o.value === keep)) node.value = keep;
    }

    /* ------------------------------------------------------------------ Reset (:1298) */
    async function reset() {
        S.Id = 0; S.Approved = false; S.VoucherHeadId = 0; S.AttachmentsValues = ''; S.CustomAttachmentsValues = '';
        S.CheckReservedNotReservedOnInsertOnly = ''; S.ItemIdsForComm = ''; S.orderNoList = null; S.commCurrent = 0; S.commUomVisible = true;
        el('ChkOpenADOForm').hidden = true; el('ChkOpenADOForm').checked = false; el('AdvanceDoFormLink').hidden = true;
        el('chkCustomAccounts').checked = false;
        el('cmbsuppliername').value = ''; el('cmbsuppliername').dispatchEvent(new Event('change'));
        for (const id of ['txtbillno', 'txtrefno', 'txtDueDays']) el(id).value = '';
        el('txtLedgerBalance').value = '0'; el('txtBillAmount').value = '0'; el('txtLedgerBalanceAfterBill').value = '0';
        S.SupplierGLId = 0; S.LedgerBalance = 0;
        S.dtGrid = []; S.dtInvExp = []; S.dtGrdGL = []; S.dtFreight = []; S.dtComm = []; S.dtPaymentTerm = [];
        addRow('gl'); addRow('exp'); addRow('freight'); addRow('comm');
        const codes = await api('/next-codes');
        el('txtdocno').value = codes.docNo; el('txtBranchSrNo').value = codes.branchSrNo;
        dueDateGenerate();
        renderAll();
        window.history.replaceState(null, '', '/sale/sale-invoice');
        el('DocDate').focus();
    }

    /* ------------------------------------------------------------------ DueDateGenerate (:6315) */
    function dueDateGenerate() {
        const days = el('txtDueDays').value.trim();
        el('DueDate').value = days !== '' ? addDays(el('DocDate').value, toDouble(days)) : el('DocDate').value;
    }

    /* ------------------------------------------------------------------ cmbsuppliername_ValueChanged (:6446) */
    async function supplierChanged() {
        el('txtLedgerBalance').value = '0'; S.LedgerBalance = 0;
        const id = toInt(el('cmbsuppliername').value);
        if (id > 0) {
            const opt = el('cmbsuppliername').selectedOptions[0];
            S.SupplierGLId = toInt(opt && opt.dataset.gl);
            const r = await api(`/ledger?customerId=${id}&docDate=${encodeURIComponent(el('DocDate').value)}`);
            S.LedgerBalance = toDouble(r.balance);
            el('txtLedgerBalance').value = fmtLedger(S.LedgerBalance);
        }
        billAmount();
    }
    /** Math.Round(x).ToString("#,#;(#,#);0") */
    function fmtLedger(v) { const r = rint(v); if (r === 0) return '0'; const s = Math.abs(r).toLocaleString('en-US'); return r < 0 ? `(${s})` : s; }

    /* ------------------------------------------------------------------ txtExchangeRate_TextChanged (:1253) */
    function exchangeRateChanged() {
        const rate = toDouble(el('txtExchangeRate').value);
        for (const r of S.dtGrid) r.FcyAmount = S.dtGrid.length > 0 && rate > 0 ? toDouble(r.ItemAmount) / rate : 0;
        billAmount();
    }

    /* ------------------------------------------------------------------ BillAmount (:5763) */
    function billAmount() {
        const sum = (rows, k) => rows.reduce((a, r) => a + toDouble(r[k]), 0);
        const itemAmount = sum(S.dtGrid, 'ItemAmount'), expAmount = sum(S.dtInvExp, 'Amount');
        let jd = 0, jc = 0, tc = 0, td = 0;
        for (const r of S.dtGrdGL) if (toInt(r.AccountId) > 0) { jd += toDouble(r.Debit); jc += toDouble(r.Credit); }
        for (const r of S.dtFreight) {
            const own = subsidiary() ? String(el('cmbsuppliername').value || '') === String(r.Transporter ?? '') : String(S.SupplierGLId || '') === String(r.Transporter ?? '');
            if (own) { td += toDouble(r.Debit); tc += toDouble(r.Freight); }
        }
        let bill = itemAmount + expAmount;
        const trans = roundAway(tc - td, S.dp);
        bill = !(trans > 0) ? bill + Math.abs(trans) : bill - Math.abs(trans);
        const gl = roundAway(jc - jd, S.dp);
        bill = !(gl < 0) ? bill + gl : bill - Math.abs(gl);
        let party = 0;
        for (const r of S.dtComm) if (toInt(el('cmbsuppliername').value) === toInt(r.CommissionAgentId)) party += toDouble(r.CommissionAmount);
        bill -= party;
        el('txtBillAmount').value = fmt(roundAway(bill, S.dp), S.dp);
        el('txtLedgerBalanceAfterBill').value = fmt(roundAway(S.LedgerBalance + bill, S.dp), S.dp);
        const rate = toDouble(el('txtExchangeRate').value);
        el('txtFcyAmount').value = S.dtGrid.length > 0 ? fmtMax(rate ? bill / rate : 0, 4) : '0';
        billProportion();
    }

    /* ExpProportion (:5835) */
    function expProportion() {
        if (!S.cfg.CreditAmountInItemSaleGL) {
            const total = S.dtInvExp.reduce((a, r) => a + toDouble(r.Amount), 0), qty = S.dtGrid.reduce((a, r) => a + toDouble(r.ItemQty), 0);
            for (const r of S.dtGrid) r.Expense = total > 0 ? total / qty * toDouble(r.ItemQty) : 0;
        } else for (const r of S.dtGrid) r.Expense = 0;
        billProportion();
    }
    /* FreightProportion (:5880) */
    function freightProportion() {
        if (!S.cfg.DebitAmountChargetoExpenseAcFreightGrid) {
            const weight = S.dtGrid.reduce((a, r) => a + toDouble(r.NetBillWeight), 0), credit = S.dtFreight.reduce((a, r) => a + toDouble(r.Freight), 0);
            for (const r of S.dtGrid) r.Freights = credit > 0 ? credit / weight * toDouble(r.NetBillWeight) : 0;
        }
        billProportion();
    }
    /* LedgerProportion (:5916) */
    function ledgerProportion() {
        const total = toDouble(S.commAmount), weight = S.dtGrid.reduce((a, r) => a + toDouble(r.NetBillWeight), 0);
        const credit = S.dtGrdGL.reduce((a, r) => a + toDouble(r.Credit), 0) + total, debit = S.dtGrdGL.reduce((a, r) => a + toDouble(r.Debit), 0);
        if (debit > credit) { const diff = debit - credit; for (const r of S.dtGrid) r.Journal = diff / weight * toDouble(r.NetBillWeight); }
        else for (const r of S.dtGrid) r.Journal = 0;
        billProportion();
    }
    /* BillProportion (:5975) */
    function billProportion() {
        for (const r of S.dtGrid) {
            const a = toDouble(r.ItemAmount), e = toDouble(r.Expense), c = toDouble(r.Commission), f = toDouble(r.Freights);
            r.BillAmount = S.cfg.CreditAmountInItemSaleGL ? roundAway(a - c - f, S.dp) : roundAway(a + e - c - f, S.dp);
        }
    }
    /* TotalCommissionAmount (:5632) over the hidden header commission controls. */
    function totalCommissionAmount() {
        if (S.commRate === '') { S.commAmount = '0'; return; }
        const rate = toDouble(S.commRate);
        if (S.commTypeText === 'Flat') S.commAmount = String(roundAway(rate, S.dp));
        if (S.commTypeText === 'Percent' || S.commTypeText === 'Percentage') S.commAmount = String(roundAway(S.dtGrid.reduce((a, r) => a + toDouble(r.ItemAmount), 0) * rate / 100, S.dp));
        if (S.commTypeText === 'Comm Weight') S.commAmount = String(roundAway(S.dtGrid.reduce((a, r) => a + toDouble(r.NetBillWeight), 0) / toDouble(S.commUomText) * rate, S.dp));
    }
    /* CalculateItemDiscount (:5733) */
    function calculateItemDiscount() {
        for (const r of S.dtGrid) { const p = toDouble(r.ItemDiscount); if (p > 0) r.DiscAmount = toDouble(r.ItemAmount) * p / 100; }
    }

    /* PropotionateCommissiongrid (:1789) */
    function propotionateCommissiongrid() {
        const c = S.cfg, policy = c.CommissionPolicyIsActive || c.CommissionPolicyForCommissionAgentIsActive;
        if (!c.DebitAmountChargetoExpenseAcOfCommission && !policy) {
            let totalItemAmount = 0; const totalForCommOnSale = 0;
            for (const r of S.dtGrid) { totalItemAmount += toDouble(r.ItemAmount); r.Commission = 0; }
            for (const row2 of S.dtComm) {
                const orderId = toInt(row2.OrderId);
                const orderItemAmount = S.dtGrid.filter(r => String(toInt(r.SaleOrderId)) === String(orderId)).reduce((a, r) => a + toDouble(r.ItemAmount), 0);
                const type = commTypeText(row2.CommType), rate = toDouble(row2.CommRate), uom = toDouble(commUomText(row2.CommUom));
                for (const item of S.dtGrid) {
                    const itemWise = !!c.EnableItemWiseCommOnSale;
                    if (itemWise && !toBool(item.CommOnSale)) continue;
                    if (orderId > 0 && toInt(item.SaleOrderId) !== orderId) continue;
                    const pre = toDouble(item.Commission), amt = toDouble(item.ItemAmount), w = toDouble(item.NetBillWeight);
                    const flatBase = itemWise ? (orderId > 0 ? totalItemAmount : totalForCommOnSale) : (orderId > 0 ? orderItemAmount : totalItemAmount);
                    if (type === 'Percent') item.Commission = amt * rate / 100 + pre;
                    else if (type === 'Comm Weight') item.Commission = w / uom * rate + pre;
                    else if (type === 'Flat') item.Commission = rate / flatBase * amt + pre;
                }
            }
        } else if (c.DebitAmountChargetoExpenseAcOfCommission && !policy) {
            for (const r of S.dtGrid) r.Commission = 0;
        }
    }

    /* CalculateCommissionGrid (:1972) */
    function calculateCommissionGrid() {
        const c = S.cfg;
        for (const item of S.dtComm) {
            const orderId = toInt(item.OrderId);
            let amount = 0, weight = 0;
            for (const row of S.dtGrid) {
                if (c.EnableItemWiseCommOnSale && !toBool(row.CommOnSale)) continue;
                if (orderId > 0 && toInt(row.SaleOrderId) !== orderId) continue;
                amount += toDouble(row.ItemAmount); weight += toDouble(row.NetBillWeight);
            }
            const type = commTypeText(item.CommType), rate = toDouble(item.CommRate);
            let value = null;
            if (type === 'Percent') value = amount * rate / 100;
            else if (type === 'Comm Weight') value = weight / toDouble(commUomText(item.CommUom)) * rate;
            else if (type === 'Flat') value = rate;
            if (value !== null) item.CommissionAmount = c.DebitAmountChargetoExpenseAcOfCommission ? roundAway(value, S.dp) : value;
        }
    }

    /* CommissionAmountCalculateInCaseofPolicy (:6008) */
    async function commissionAmountCalculateInCaseofPolicy() {
        const c = S.cfg;
        if (c.CommissionPolicyIsActive && c.CommissionPolicyForCommissionAgentIsActive)
            throw new Error("Configuration 'CommissionPolicyIsActive' and 'CommissionPolicyForCommissionAgentIsActive' can't be be true on the same time,please disable on of them");
        const weight = S.dtGrid.reduce((a, r) => a + toDouble(r.NetBillWeight), 0);
        const date = el('DocDate').value;
        if (c.CommissionPolicyIsActive && S.dtGrid.length > 0) {
            let total = 0, party = 0;
            if (weight > 0) {
                const dt = await api(`/commission-policy?customerId=${toInt(el('cmbsuppliername').value)}&docDate=${encodeURIComponent(date)}&itemIds=${encodeURIComponent(S.ItemIdsForComm)}&policyTypeId=1`);
                if (dt.length > 0) {
                    party = toInt(get(dt[0], 'CustomerIncentivePartyId'));
                    for (const r of S.dtGrid) {
                        const pct = dt.filter(x => toInt(get(x, 'ItemId')) === toInt(r.ItemId)).reduce((a, x) => a + toDouble(get(x, 'CommissionRate')), 0);
                        const amt = toDouble(r.ItemAmount) * pct / 100;
                        total += amt; r.Commission = amt; r.CommPercent = pct;
                    }
                } else for (const r of S.dtGrid) { r.Commission = 0; r.CommPercent = 0; }
            }
            for (const r of S.dtComm) { r.CommissionAmount = total; r.CommissionAgentId = party; }
        }
        if (!c.CommissionPolicyForCommissionAgentIsActive || S.dtGrid.length <= 0) return;
        for (const rrr of S.dtComm) {
            let total = 0, party = 0;
            if (weight > 0) {
                const dt = await api(`/commission-policy?customerId=${toInt(rrr.CommissionAgentId)}&docDate=${encodeURIComponent(date)}&itemIds=${encodeURIComponent(S.ItemIdsForComm)}&policyTypeId=2`);
                const lines = S.dtGrid.filter(v => toInt(v.SaleOrderId) === toInt(rrr.OrderId));
                if (dt.length > 0) {
                    party = toInt(get(dt[0], 'CustomerIncentivePartyId'));
                    for (const r2 of lines) {
                        const pct = dt.filter(x => toInt(get(x, 'ItemId')) === toInt(r2.ItemId)).reduce((a, x) => a + toDouble(get(x, 'CommissionRate')), 0);
                        const amt = toDouble(r2.ItemAmount) * pct / 100;
                        total += amt; r2.Commission = amt; r2.CommPercent = pct;
                    }
                } else for (const r2 of lines) { r2.Commission = 0; r2.CommPercent = 0; }
            }
            for (const it of S.dtComm.filter(v => toInt(v.OrderId) === toInt(rrr.OrderId))) { it.CommissionAmount = total; it.CommissionAgentId = party; }
        }
    }

    /* BindCommGridOrderNo (:2449) - the OrderNo value list of the order numbers on the detail grid. */
    function bindCommGridOrderNo() {
        const r = S.dtComm[S.commCurrent];
        if (!r || toInt(r.OrderId) !== 0) return;
        const list = [...new Set(S.dtGrid.filter(g => toInt(g.SaleOrderId) > 0).map(g => toInt(g.SaleOrder)))];
        if (list.length > 0) S.orderNoList = list;
    }
    /* GetOrderIdAgainstOrderNo (:2415) */
    function getOrderIdAgainstOrderNo(row) {
        const no = toInt(row.OrderNo);
        const hit = S.dtGrid.find(g => toInt(g.SaleOrder) === no);
        row.OrderId = hit ? toInt(hit.SaleOrderId) : 0;
    }

    /* ------------------------------------------------------------------ payment detail */
    function addPaymentRow(saleOrderId, saleOrderNo, paymentTermsId, dueDays, dueDate, pct) {
        S.dtPaymentTerm.push({SaleOrderId: saleOrderId, SaleOrderNo: saleOrderNo, ItemAmount: 0, Expense: 0, Freight: 0, PartyAddLessAmount: 0, Commission: 0, NetBillAmount: 0,
            PaymentTerm: paymentTermsId, DueDays: dueDays, DueDate: dueDate, '%OfTotal': pct || 0, Amount: 0, Remarks: ''});
    }
    function distinctOrders() {
        const seen = new Set(), out = [];
        for (const r of S.dtGrid) {
            const o = {OrderId: toInt(r.SaleOrderId), OrderNo: toInt(r.SaleOrder), PaymentTermId: toInt(r.SaleOrderPaymentTermId)};
            if (o.OrderId <= 0) continue;
            const k = `${o.OrderId}|${o.OrderNo}|${o.PaymentTermId}`;
            if (!seen.has(k)) { seen.add(k); out.push(o); }
        }
        return out;
    }
    /* LoadPaymentDetail (:3331) */
    async function loadPaymentDetail() {
        const orders = distinctOrders();
        if (!orders.length) return;
        const existing = new Set(S.dtPaymentTerm.map(r => toInt(r.SaleOrderId)));
        const dt = await api('/so-payment-terms', {orderIds: orders.map(o => o.OrderId)});
        const lookup = new Map();
        for (const row of dt || []) { const id = toInt(get(row, 'SaleOrderId')); if (!lookup.has(id)) lookup.set(id, []); lookup.get(id).push(row); }
        for (const o of orders) {
            if (existing.has(o.OrderId)) continue;
            if (lookup.has(o.OrderId)) for (const dr of lookup.get(o.OrderId)) addPaymentRow(o.OrderId, toInt(get(dr, 'SaleOrderNo')), toInt(get(dr, 'PaymentTermsId')), toInt(get(dr, 'DueDays')), dateOnly(get(dr, 'DueDate')), toDouble(get(dr, 'PrcntOfTotal')));
            else addPaymentRow(o.OrderId, o.OrderNo, o.PaymentTermId, 0, today());
        }
    }
    /* CalculateOrderWiseAmountForPaymentDetail (:3397) */
    function calculateOrderWiseAmountForPaymentDetail() {
        const groups = new Map();
        for (const r of S.dtGrid) {
            const id = toInt(r.SaleOrderId); if (id <= 0) continue;
            const k = `${id}|${toInt(r.SaleOrder)}`;
            if (!groups.has(k)) groups.set(k, {OrderId: id, OrderNo: toInt(r.SaleOrder), TotalItemAmount: 0});
            groups.get(k).TotalItemAmount += toDouble(r.ItemAmount);
        }
        const orders = [...groups.values()], count = orders.length;
        if (count === 0) return;
        const partyId = toInt(el('cmbsuppliername').value);
        let jd = 0, jc = 0;
        for (const r of S.dtGrdGL) if (toInt(r.AccountId) > 0) { jd += toDouble(r.Debit); jc += toDouble(r.Credit); }
        const glDiff = jc - jd;
        const transporterId = subsidiary() ? partyId : S.SupplierGLId;
        const own = S.dtFreight.filter(v => toInt(v.Transporter) === transporterId);
        const transDiff = roundAway(own.reduce((a, v) => a + toDouble(v.Freight), 0) - own.reduce((a, v) => a + toDouble(v.Debit), 0), S.dp);
        const totalExpense = S.dtInvExp.reduce((a, v) => a + toDouble(v.Amount), 0);
        const sums = new Map();
        for (const r of S.dtGrid) { const id = toInt(r.SaleOrderId); const s = sums.get(id) || {ItemAmount: 0, Expense: 0}; s.ItemAmount += toDouble(r.ItemAmount); s.Expense += toDouble(r.Expense); sums.set(id, s); }
        const comm = new Map();
        for (const v of S.dtComm.filter(v => toInt(v.CommissionAgentId) === partyId)) comm.set(toInt(v.OrderId), (comm.get(toInt(v.OrderId)) || 0) + toDouble(v.CommissionAmount));
        const globalComm = comm.get(0) || 0;
        const totalItemAmount = S.dtGrid.reduce((a, r) => a + toDouble(r.ItemAmount), 0);
        if (totalItemAmount === 0) throw new Error('[CalculateOrderWiseAmountForPaymentDetail] Attempted to divide by zero.');
        for (const o of orders) {
            const s = sums.get(o.OrderId);
            const perOrderExpense = totalExpense !== 0 ? totalExpense / count : 0;
            const freight = Math.abs(transDiff) / totalItemAmount * o.TotalItemAmount;
            const journal = Math.abs(glDiff) / totalItemAmount * o.TotalItemAmount;
            const itemAmount = s ? s.ItemAmount : 0, propExpense = s ? s.Expense : 0;
            let orderComm = comm.get(o.OrderId) || 0;
            if (globalComm > 0) orderComm += globalComm / count;
            const expense = propExpense > 0 ? propExpense : perOrderExpense;
            let net = itemAmount + expense - orderComm;
            net = transDiff > 0 ? net - freight : net + Math.abs(freight);
            net = glDiff > 0 ? net + journal : net - Math.abs(journal);
            for (const row of S.dtPaymentTerm.filter(p => toInt(p.SaleOrderId) === o.OrderId)) {
                row.ItemAmount = itemAmount; row.PartyAddLessAmount = journal; row.Freight = freight; row.Expense = expense; row.Commission = orderComm; row.NetBillAmount = net;
            }
        }
    }
    /* PaymentAmountReCalculate (:3498) */
    function paymentAmountReCalculate() {
        if (!S.PCalculateByPercent) return;
        for (const r of S.dtPaymentTerm) {
            let pct = toDouble(r['%OfTotal']); if (pct < 0) pct = 0;
            r.Amount = roundAway(pct * toDouble(r.NetBillAmount) / 100, 4);
        }
    }

    /* ------------------------------------------------------------------ grd_CellUpdated (:3538) */
    async function grdCellUpdated(r, col) {
        if (col === 'RateCut' || col === 'ItemRateWOExp' || col === 'RateEquivalent') {
            const w = toDouble(r.NetBillWeight), cut = toDouble(r.RateCut), rate = toDouble(r.ItemRateWOExp);
            r.ItemRate = cut > 0 ? rate - Math.abs(cut) : rate + Math.abs(cut);
            const eq = toDouble(r.RateEquivalent);
            r.RateCutAmount = roundAway(w / eq * cut, S.dp);
            const amount = roundAway(w / eq * toDouble(r.ItemRate), S.dp);
            r.ItemAmount = amount;
            const exp = toDouble(r.Expense), comm = toDouble(r.Commission);
            const raw = S.cfg.CreditAmountInItemSaleGLRaw;
            if (raw !== undefined && raw !== null && raw !== '') r.BillAmount = String(raw).trim().toLowerCase() !== 'true' ? amount : roundAway(amount + exp - comm, S.dp);
            else r.BillAmount = roundAway(amount + exp - comm, S.dp);
        }
        await commissionAmountCalculateInCaseofPolicy();
        exchangeRateChanged();
        calculateItemDiscount();
    }

    /* ------------------------------------------------------------------ grdFreight_CellUpdated (:1696) */
    function freightCellUpdated(r, col) {
        if (col === 'Freight') { r.Freight = roundAway(toDouble(r.Freight), S.dp); if (toDouble(r.Debit) > 0) { r.Freight = 0; alert('Debit Side is aleady added'); } }
        if (col === 'Debit') { r.Debit = roundAway(toDouble(r.Debit), S.dp); if (toDouble(r.Freight) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); } }
        if (col === 'Transporter') for (const x of S.dtFreight) x.GlAccountId = glByTitle(accountTitle(x.Transporter), toInt(x.GlAccountId));
        freightProportion(); billAmount();
    }
    /* grdComm_CellUpdated (:2118) */
    async function commCellUpdated(r, col) {
        const capPercent = () => {
            if (commTypeText(r.CommType) !== 'Percent') return;
            const total = S.dtComm.reduce((a, x) => a + toDouble(x.CommRate), 0);
            if (total > 100) { r.CommRate = toDouble(r.CommRate) - (total - 100); alert('TotalPercentage Can not be Greater than 100...'); }
        };
        if (col === 'CommRate') { capPercent(); calculateCommissionGrid(); }
        if (col === 'CommType') {
            const t = commTypeText(r.CommType);
            if (t === 'Flat' || t === 'Percent') { S.commUomVisible = false; capPercent(); } else S.commUomVisible = true;
            calculateCommissionGrid();
        }
        if (col === 'OrderNo') { calculateCommissionGrid(); getOrderIdAgainstOrderNo(r); }
        if (col === 'DebitAccountId') for (const x of S.dtComm) x.GlAccountId = glByTitle(commDebitTitle(x.DebitAccountId), toInt(x.GlAccountId));
        propotionateCommissiongrid();
        await commissionAmountCalculateInCaseofPolicy();
        billAmount(); billProportion();
    }
    function commDebitTitle(v) {
        if (subsidiary()) return accountTitle(v);
        const r = (S.L.commissionDebitAccounts || []).find(a => toInt(get(a, 'Id')) === toInt(v) && toInt(v) !== 0);
        return r ? get(r, 'AccountTitle') : null;
    }
    /* grdGLedger_CellUpdated (:2546) */
    function glCellUpdated(r, col) {
        if ((col === 'Qty' || col === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') {
            r.Credit = roundAway(toDouble(r.Qty) * toDouble(r.Rate), S.dp); r.Debit = 0; r.Percentage = 0;
        }
        if (col === 'Percentage') {
            const total = S.dtGrdGL.reduce((a, x) => a + toDouble(x.Percentage), 0);
            if (total > 100) { r.Percentage = toDouble(r.Percentage) - (total - 100); alert('TotalPercentage Can not be Greater than 100...'); return; }
            if (String(r.Percentage) !== '') {
                const items = S.dtGrid.reduce((a, x) => a + toDouble(x.ItemAmount), 0);
                const v = items / 100 * toDouble(r.Percentage);
                if (v > 0) { r.Credit = roundAway(v, S.dp); r.Debit = 0; }
                else { r.Debit = Math.abs(roundAway(v, S.dp)); r.Credit = 0; }
                r.Qty = 0; r.Rate = 0;
            }
        }
        if (col === 'Credit') { r.Credit = roundAway(toDouble(r.Credit), S.dp); if (toDouble(r.Debit) > 0) { r.Credit = 0; alert('Debit Side is aleady added'); } }
        if (col === 'Debit') { r.Debit = roundAway(toDouble(r.Debit), S.dp); if (toDouble(r.Credit) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); } }
        if (col === 'AccountId') {
            if (!subsidiary() && S.SupplierGLId === toInt(r.AccountId)) { alert('Customer Account Not select'); r.AccountId = 0; return; }
            for (const x of S.dtGrdGL) x.GlAccountId = glByTitle(accountTitle(x.AccountId), toInt(x.GlAccountId));
        }
        billAmount(); ledgerProportion();
    }
    /* grdInvExp_CellUpdated (:2914) */
    function expCellUpdated(r, col) {
        if ((col === 'Qty' || col === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = roundAway(toDouble(r.Qty) * toDouble(r.Rate), S.dp);
        else if (col === 'Amount' && String(r.Amount) !== '') { r.Qty = 0; r.Rate = 0; r.Amount = roundAway(toDouble(r.Amount), S.dp); }
        expProportion(); billAmount();
    }
    /* grdPaymentDetail_CellUpdated (:3170) */
    function payCellUpdated(r, col) {
        if (S.dtGrid.length === 0) throw new Error('No Detail Record Found');
        const bill = toDouble(r.NetBillAmount), same = S.dtPaymentTerm.filter(x => toInt(x.SaleOrderId) === toInt(r.SaleOrderId) && x !== r);
        const otherPct = same.reduce((a, x) => a + toDouble(x['%OfTotal']), 0), otherAmt = same.reduce((a, x) => a + toDouble(x.Amount), 0);
        let pct = toDouble(r['%OfTotal']), amt = toDouble(r.Amount);
        if (col === '%OfTotal') {
            if (otherPct + pct > 100.01) { alert('Total % cannot exceed 100 for this order'); r['%OfTotal'] = 0; r.Amount = 0; return; }
            amt = bill * pct / 100; r.Amount = round(amt, 4); S.PCalculateByPercent = true; S.PCalculateByAmount = false;
        }
        if (col === 'Amount') {
            if (otherAmt + amt > bill) { const rem = bill - otherAmt; amt = rem < 0 ? 0 : rem; r.Amount = S.cfg.BaseCurrencyRate; r['%OfTotal'] = 0; }
            pct = amt * 100 / bill; r['%OfTotal'] = round(pct, 8); S.PCalculateByAmount = true; S.PCalculateByPercent = false;
        }
        if (col === 'DueDays') r.DueDate = addDays(el('DocDate').value, toInt(r.DueDays));
        if (col === 'DueDate') {
            if (dateOnly(r.DueDate) < el('DocDate').value) { r.DueDate = el('DocDate').value; throw new Error("Due Date Can't less Than DocDate"); }
            r.DueDays = daysBetween(el('DocDate').value, dateOnly(r.DueDate));
        }
    }

    /* ------------------------------------------------------------------ grid events */
    function numericGuard(key, col, value) {
        const numeric = {grd: ['RateCut', 'GpNo'], freight: ['Freight', 'Debit'], comm: ['CommRate', 'CommissionAmount'], gl: ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'], exp: ['Qty', 'Rate', 'Amount'], pay: []}[key] || [];
        if (numeric.includes(col) && value !== '' && !isFinite(Number(String(value).replace(/,/g, '')))) { alert('Please Type Only Numeric Value'); return false; }
        return true;
    }
    async function onCellChange(e) {
        const t = e.target; if (!t.dataset || !t.dataset.grid) return;
        const key = t.dataset.grid, i = toInt(t.dataset.row), col = t.dataset.col, rows = S[TABLES[key]], r = rows[i];
        if (!r) return;
        if (!numericGuard(key, col, t.value)) { renderGrid(key); return; }
        r[col] = t.tagName === 'SELECT' ? (isFinite(Number(t.value)) ? Number(t.value) : t.value) : t.value;
        if (key === 'comm') S.commCurrent = i;
        try {
            if (key === 'grd') await grdCellUpdated(r, col);
            else if (key === 'freight') freightCellUpdated(r, col);
            else if (key === 'comm') await commCellUpdated(r, col);
            else if (key === 'gl') glCellUpdated(r, col);
            else if (key === 'exp') expCellUpdated(r, col);
            else if (key === 'pay') payCellUpdated(r, col);
        } catch (err) { alert(err.message); }
        renderAll();
    }
    async function onGridClick(e) {
        const tr = e.target.closest('tr[data-row]');
        if (tr && tr.closest('#grdComm')) { const i = toInt(tr.dataset.row); if (i !== S.commCurrent) { S.commCurrent = i; bindCommGridOrderNo(); if (!e.target.closest('button,select,input')) renderGrid('comm'); } }
        const b = e.target.closest('button[data-act]'); if (!b || b.disabled) return;
        const key = b.dataset.grid, i = toInt(b.dataset.row), rows = S[TABLES[key]];
        try {
            if (key === 'freight') {
                if (b.dataset.act === 'del') { rows.splice(i, 1); if (!rows.length) addRow('freight'); } else addRow('freight');
                freightProportion(); billAmount();
            } else if (key === 'gl') {
                if (b.dataset.act === 'del') { rows.splice(i, 1); if (!rows.length) addRow('gl'); } else addRow('gl');
                billAmount();
            } else if (key === 'exp') {
                if (b.dataset.act === 'del') { rows.splice(i, 1); if (!rows.length) addRow('exp'); } else addRow('exp');
                expProportion(); billAmount();
            } else if (key === 'comm') {
                S.commCurrent = i;
                if (b.dataset.act === 'del') {
                    if (toInt(rows[i].OrderId) > 0 && rows.length === 1) { alert('You Can not Delete Row Because The Above Entry Is From Sale Order'); return; }
                    rows.splice(i, 1); if (!rows.length) addRow('comm');
                    S.commCurrent = Math.min(S.commCurrent, S.dtComm.length - 1);
                } else addRow('comm');
                propotionateCommissiongrid(); await commissionAmountCalculateInCaseofPolicy(); billAmount();
            } else if (key === 'pay') paymentButton(b.dataset.act, i);
        } catch (err) { alert(err.message); }
        renderAll();
    }
    /* grdPaymentDetail_ColumnButtonClick (:3123) */
    function paymentButton(act, i) {
        const item = S.dtPaymentTerm[i]; if (!item) return;
        const orderId = toInt(item.SaleOrderId), same = S.dtPaymentTerm.filter(r => toInt(r.SaleOrderId) === orderId);
        if (act === 'del') { if (same.length > 1) S.dtPaymentTerm.splice(i, 1); return; }
        const bill = toDouble(item.NetBillAmount), sumAmt = same.reduce((a, r) => a + toDouble(r.Amount), 0), sumPct = same.reduce((a, r) => a + toDouble(r['%OfTotal']), 0);
        const rem = bill - sumAmt, remPct = 100 - sumPct;
        if (rem > 0 && remPct > 0) S.dtPaymentTerm.push({...item, Amount: rem, '%OfTotal': remPct});
        else alert(`Can't break further because percent Or Amount is Equal To Order Bill Amount:${fmtMax(bill, 3)}.\n If you want to break further first change percent or amount`);
    }

    /* ------------------------------------------------------------------ Load GDN (toolStripButton3_Click_1) */
    async function openLoadGdn(button) {
        await run(button, async () => {
            const data = await api('/pending-gdns');
            S.gdnBranches = data.branches || []; S.gdnCfg = data;
            el('CmbBranch').innerHTML = S.gdnBranches.map(b => `<label><input type="checkbox" value="${esc(get(b, 'BranchId'))}" checked> ${esc(get(b, 'BranchName'))}</label>`).join('');
            el('LoadFromDate').value = S.L.yearStart || today();
            el('LoadToDate').value = today();
            el('grdPendingGdn').innerHTML = ''; el('GridDetail').innerHTML = '';
            el('frmLoadGDN').hidden = false;
            await showPendingGdn(null);
        });
    }
    async function showPendingGdn(button) {
        await run(button, async () => {
            const branchIds = Array.from(el('CmbBranch').querySelectorAll('input:checked')).map(x => x.value).join(',');
            if (!branchIds) throw new Error('Select branch first');
            const data = await api(`/pending-gdns?from=${el('LoadFromDate').value}&to=${el('LoadToDate').value}&branchIds=${encodeURIComponent(branchIds)}`);
            S.pendingGdn = data.rows || [];
            const showReserved = !!data.isStockReservedPerParty, showBranch = !data.branchImplemented;
            const fields = ['CategoryII', 'DocumentType', 'DocDate', 'DocNo', 'CostCenter', ...(showBranch ? ['BranchName'] : []), 'CustomerName', 'GpNO', 'BiltyNo', 'VehicleNo', 'ItemQty', 'TicketNos', 'OrderNo', ...(showReserved ? ['IsStockReserved'] : [])];
            const value = (r, f) => ({CategoryII: get(r, 'OtherCategory'), CostCenter: get(r, 'CostCenterName'), CustomerName: get(r, 'SupplierCustomer'),
                DocDate: formatDdMmmYy(get(r, 'DocDate')), IsStockReserved: toBool(get(r, 'IsStockReserved')) ? 'Reserved' : 'Not Reserved'}[f] ?? get(r, f));
            let html = '<thead><tr><th><input type="checkbox" id="selectAllGdn" aria-label="Select all GDNs"></th>' + fields.map(f => `<th>${esc(f)}</th>`).join('') + '</tr></thead><tbody>';
            S.pendingGdn.forEach((r, i) => {
                html += `<tr data-gdn="${i}"><td class="btn"><input type="checkbox" data-gdn-check="${i}" aria-label="Select GDN ${esc(get(r, 'DocNo'))}"></td>` + fields.map(f => {
                    const v = value(r, f);
                    if (f === 'DocNo') return `<td><a href="/sale/gdn?id=${toInt(get(r, 'Id'))}" target="_blank" rel="noopener">${esc(v)}</a></td>`;
                    if (f === 'GpNO' && toInt(get(r, 'OutwardGatePassId')) > 0) return `<td><a href="/sale/outward-gate-pass?id=${toInt(get(r, 'OutwardGatePassId'))}" target="_blank" rel="noopener">${esc(v)}</a></td>`;
                    return `<td class="${f === 'ItemQty' ? 'n' : ''}">${esc(f === 'ItemQty' ? fmtMax(v, 0) : v)}</td>`;
                }).join('') + '</tr>';
            });
            html += '</tbody><tfoot><tr><td></td>' + fields.map(f => f === 'ItemQty' ? `<td class="n">${fmtMax(S.pendingGdn.reduce((a, r) => a + toDouble(get(r, 'ItemQty')), 0), 0)}</td>` : '<td></td>').join('') + '</tr></tfoot>';
            el('grdPendingGdn').innerHTML = html;
            el('GridDetail').innerHTML = '';
        });
    }
    function formatDdMmmYy(v) { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return String(v); return `${String(d.getDate()).padStart(2, '0')}-${d.toLocaleString('en-US', {month: 'short'})}-${String(d.getFullYear()).slice(2)}`; }
    async function gdnSelectionChanged(i) {
        const r = S.pendingGdn[i]; if (!r) return;
        document.querySelectorAll('#grdPendingGdn tr.si-selected').forEach(x => x.classList.remove('si-selected'));
        const tr = document.querySelector(`#grdPendingGdn tr[data-gdn="${i}"]`); if (tr) tr.classList.add('si-selected');
        const rows = await api('/gdn-detail/' + toInt(get(r, 'Id')));
        const f = [['Order', 'SaleOrderNo'], ['Item', 'Item'], ['CropYear', 'CropYear'], ['JobLot', 'JobLot'], ['PackingType', 'PackingType'], ['UOM', 'UOMCode'], ['Qty', 'ItemQty', 1], ['GrossWight', 'GrossWeight', 1], ['EbUnit', 'EBWPerUnit'], ['EbTotal', 'EBWTotal', 1], ['WtCut', 'WtCut'], ['WtCutTotal', 'WtCutTotal', 1], ['AddLesswt', 'AdLsWeight', 1], ['NetWeight', 'NetBillWeight', 1], ['StockWeight', 'StockWeight', 1], ['WareHouse', 'WareHouseCode'], ['LabNo', 'LabReportRef'], ['City', 'AreaCity']];
        el('GridDetail').innerHTML = '<thead><tr>' + f.map(x => `<th>${x[0]}</th>`).join('') + '</tr></thead><tbody>' + rows.map(d => '<tr>' + f.map(x => `<td class="${x[2] ? 'n' : ''}">${esc(x[2] ? fmtMax(get(d, x[1]), 2) : get(d, x[1]))}</td>`).join('') + '</tr>').join('') + '</tbody><tfoot><tr>' + f.map(x => x[2] ? `<td class="n">${fmtMax(rows.reduce((a, d) => a + toDouble(get(d, x[1])), 0), 2)}</td>` : '<td></td>').join('') + '</tr></tfoot>';
    }
    /* frmLoadGDN.BtnLoad (:482) */
    async function btnLoadGdn(button) {
        await run(button, async () => {
            const checked = Array.from(document.querySelectorAll('[data-gdn-check]:checked')).map(x => S.pendingGdn[toInt(x.dataset.gdnCheck)]);
            if (!checked.length) throw new Error('Check the row first');
            let sId = 0, docId = 0, orderNo = 0, cc = 0, reserved = '';
            const ids = [];
            for (const r of checked) {
                const sc = toInt(get(r, 'SupplierCustomerId'));
                if (sc !== 0) { if (sId === 0) sId = sc; if (sId !== sc) throw new Error('Sorry!. The Selected Gdn\'s are not of same Customer'); }
                const dt = toInt(get(r, 'DocumentTypeId'));
                if (dt !== 0) { if (docId === 0) docId = dt; if (docId !== dt) throw new Error('Sorry!. The Selected Gdn\'s are not of same DocumentType'); }
                const on = toInt(get(r, 'OrderNo'));
                if (on !== 0) { if (orderNo === 0) orderNo = on; if (orderNo !== on) throw new Error('Sorry!. The Selected Gdn\'s are not of same Orders'); }
                const c = toInt(get(r, 'CostCenterId'));
                if (c !== 0) { if (cc === 0) cc = c; if (cc !== c) throw new Error('Sorry!. The Selected Gdn\'s are not of same Cost Center'); }
                if (S.gdnCfg && S.gdnCfg.isStockReservedPerParty) {
                    const res = toBool(get(r, 'IsStockReserved')) ? 'Reserved' : 'Not Reserved';
                    if (reserved === '') reserved = res; if (res !== reserved) throw new Error('Sorry!. The Selected Gdn\'s are not of same Reserved Status');
                }
                ids.push(toInt(get(r, 'Id')));
                sId = sc; docId = dt; orderNo = on;
            }
            el('frmLoadGDN').hidden = true;
            await loadInGridDetail(ids);
        });
    }
    /* LoadInGridDetail (:5577) + the rest of toolStripButton3_Click_1 */
    async function loadInGridDetail(ids) {
        const data = await api('/load-gdns', {gdnIds: ids, customerId: toInt(el('cmbsuppliername').value) || null, reserveStatus: S.Id ? '' : S.CheckReservedNotReservedOnInsertOnly});
        const rows = data.details || [];
        if (rows.length) {
            /* LoadDataDetailGridAgainstGP (:5373) */
            if (!S.Id) {
                if (S.CheckReservedNotReservedOnInsertOnly === '') S.CheckReservedNotReservedOnInsertOnly = data.reserveStatus;
                const show = S.CheckReservedNotReservedOnInsertOnly === 'Reserved' && !!S.cfg.IsStockReservedPerParty;
                el('ChkOpenADOForm').hidden = !show; el('AdvanceDoFormLink').hidden = !show; el('ChkOpenADOForm').checked = show;
            }
            el('cmbsuppliername').value = String(data.customerId); await supplierChanged();
            S.dtComm = [];
            for (const r of rows) {
                const so = toInt(get(r, 'SaleOrderId'));
                if (S.dtComm.some(c => toInt(c.OrderId) === so)) continue;
                if (toInt(get(r, 'BrokerAgentSupCustId')) !== 0)
                    S.dtComm.push({Id: 0, OrderId: get(r, 'SaleOrderId'), OrderNo: get(r, 'SaleOrder'), InvoiceId: 0, CommissionAgentId: get(r, 'BrokerAgentSupCustId'), CommType: String(get(r, 'CommissionType') ?? ''), CommRate: String(get(r, 'CommRate') ?? ''), CommUom: String(get(r, 'UomScheduleIdCmRate') ?? ''), CommissionAmount: String(get(r, 'CommAmount') ?? ''), DebitAccountId: String(get(r, 'CommissionRemarks') ?? ''), Remarks: 0, GlAccountId: 0});
                if (toInt(get(r, 'OtherCommissionAgentId')) !== 0)
                    S.dtComm.push({Id: 0, OrderId: get(r, 'SaleOrderId'), OrderNo: get(r, 'SaleOrder'), InvoiceId: 0, CommissionAgentId: get(r, 'OtherCommissionAgentId'), CommType: String(get(r, 'OtherCommissionType') ?? ''), CommRate: String(get(r, 'OtherCommissionRate') ?? ''), CommUom: String(get(r, 'OtherCommissionUom') ?? ''), CommissionAmount: String(get(r, 'OtherCommissionAmount') ?? ''), DebitAccountId: String(get(r, 'OtherCommissionRemarks') ?? ''), Remarks: 0, GlAccountId: 0});
            }
            el('txtRemarks').value = String(get(rows[0], 'RemarksHeader') ?? '');
            el('txtDueDays').value = String(get(rows[0], 'OrderDueDays') ?? '');
            el('DueDate').value = dateOnly(get(rows[0], 'OrderDueDate'));
            dueDateGenerate();
            for (const r of rows) {
                if (S.dtGrid.some(g => toInt(g.Id) === toInt(get(r, 'Id')))) continue;
                const amount = roundAway(toDouble(get(r, 'ItemAmount')), S.dp);
                S.dtGrid.push({OtherCategoryId: get(r, 'OtherCategoryId'), CategoryII: get(r, 'OtherCategory'), RefRefDocumentTypeId: get(r, 'RefDocumentTypeId'), RefRefDocIdNo: get(r, 'RefDocIdNo'), RefDocSubId: get(r, 'RefDocSubIdNo'),
                    PurchaseGLAC: get(r, 'PurchaseGLAC'), Id: get(r, 'Id'), InvGdnId: get(r, 'invGdnId'), ItemId: get(r, 'ItemId'), ItemCode: get(r, 'ItemCode'), ItemName: get(r, 'ItemName'),
                    BrandItemId: get(r, 'BrandItemId'), BrandItemCode: get(r, 'BrandItemCode'), BrandItemName: get(r, 'BrandItemName'), CropYear: get(r, 'CropYear'), ItemUomId: get(r, 'ItemUomId'),
                    UOMCodeItem: get(r, 'UOMCodeItem'), PackEquivalent: get(r, 'PackEquivalent'), ItemQty: toDouble(get(r, 'ItemQty')), WarehouseId: toInt(get(r, 'WarehouseId')), Warehouse: get(r, 'WareHouseName'),
                    PackingTypeId: get(r, 'PackingTypeId'), SaleOrderId: get(r, 'SaleOrderId'), SaleOrder: get(r, 'SaleOrder'), GrossWeight: toDouble(get(r, 'GrossWeight')), AddLsWt: toDouble(get(r, 'AdLsWeight')),
                    EBWPerUnit: toDouble(get(r, 'EBWPerUnit')), EmptyBagsDeduction: toDouble(get(r, 'EBWTotal')), NetBillWeight: toDouble(get(r, 'NetBillWeight')), StockWeight: toDouble(get(r, 'StockWeight')),
                    ItemRateWOExp: toDouble(get(r, 'OrderItemRate')), PackingAddLess: toDouble(get(r, 'PackingAddLess')), ItemRate: toDouble(get(r, 'OrderItemRate')), OrderItemRateUOMId: get(r, 'OrderItemRateUOMId'),
                    RateUOM: get(r, 'RateUom'), RateEquivalent: toDouble(get(r, 'EquivalentPoRate')), RateCut: 0, RateCutAmount: 0, ItemAmount: amount, FcyAmount: amount,
                    ItemDiscount: get(r, 'ItemDiscount'), DiscAmount: 0, JobLotId: get(r, 'JobLotId'), BillAmount: 0, Freights: 0, Expense: 0, Journal: 0, Commission: 0,
                    VehicleNo: get(r, 'VehicleNo'), GpNo: toInt(get(r, 'GpNo')), CommOnSale: toBool(get(r, 'CommOnSale')), CommPercent: 0, BranchId: toInt(get(r, 'BranchId')), BranchName: get(r, 'BranchName'),
                    CostCenterId: toInt(get(r, 'CostCenterId')), SaleOrderPaymentTermId: toInt(get(r, 'PaymentTermsId'))});
                S.ItemIdsForComm += ',' + String(get(r, 'ItemId'));
            }
            S.hasSaleOrder = rows.some(r => toInt(get(r, 'SaleOrderId')) > 0);
        }
        /* LoadFreightData (:5473) */
        S.dtFreight = [];
        for (const f of data.freights || []) {
            if (!(toInt(get(f, 'CarriageAmount')) > 0)) continue;
            let remarks = '';
            if (toInt(get(f, 'GpNo')) > 0) remarks += 'GpNo: ' + String(get(f, 'GpNo'));
            if (String(get(f, 'VehicleNo') ?? '') !== '') remarks += '   VehicleNo:  ' + String(get(f, 'VehicleNo'));
            if (String(get(f, 'BiltyNo') ?? '') !== '') remarks += '   BiltyNo: ' + String(get(f, 'BiltyNo'));
            S.dtFreight.push({InvGdnId: get(f, 'MainId'), GlAccountId: get(f, 'Transporter'), Transporter: subsidiary() ? get(f, 'TransporterSupCustId') : 0, Freight: get(f, 'CarriageAmount'), Debit: 0, Remarks: remarks});
        }
        if (!S.dtFreight.length) addRow('freight');
        /* grdFreightSettings: without feature 4 the Transporter cell is filled from GlAccountId. */
        if (!subsidiary()) for (const r of S.dtFreight) { const gl = toInt(r.GlAccountId); if (gl && (S.L.accounts || []).some(a => toInt(get(a, 'Id')) === gl)) r.Transporter = gl; }
        for (const x of S.dtFreight) x.GlAccountId = glByTitle(accountTitle(x.Transporter), toInt(x.GlAccountId));
        if (!S.dtComm.length) addRow('comm');
        S.commCurrent = 0;
        /* LoadExpensesFromGdn / LoadExpData */
        const expenses = data.expenses || [];
        if (expenses.length) {
            if (S.dtInvExp.reduce((a, r) => a + toDouble(r.Amount), 0) === 0) S.dtInvExp = [];
            for (const x of expenses) S.dtInvExp.push({ItemId: get(x, 'ItemId'), Qty: get(x, 'Qty'), Rate: get(x, 'BagPrice'), Amount: toDouble(get(x, 'Qty')) * toDouble(get(x, 'BagPrice')), Remarks: String(get(x, 'Remarks') ?? '')});
        }
        if (!S.dtInvExp.length) {
            const so = data.soExpenses || [];
            if (so.length) {
                if (S.dtInvExp.reduce((a, r) => a + toDouble(r.Amount), 0) === 0) S.dtInvExp = [];
                for (const x of so) S.dtInvExp.push({ItemId: 0, Qty: get(x, 'ItemQty'), Rate: get(x, 'BagPrice'), Amount: toDouble(get(x, 'ItemQty')) * toDouble(get(x, 'BagPrice')), Remarks: 0});
            }
        }
        if (!S.dtInvExp.length) addRow('exp');
        expProportion(); freightProportion(); totalCommissionAmount(); calculateCommissionGrid(); propotionateCommissiongrid(); ledgerProportion(); billProportion(); billAmount();
        bindCommGridOrderNo();
        await commissionAmountCalculateInCaseofPolicy();
        await loadPaymentDetail();
        calculateOrderWiseAmountForPaymentDetail();
        paymentAmountReCalculate();
        renderAll();
    }

    /* ------------------------------------------------------------------ ReadById (:3868) */
    async function readById(id) {
        const inv = await api('/' + toInt(id));
        S.Id = toInt(get(inv, 'Id'));
        el('txtdocno').value = String(get(inv, 'DocNo') ?? '');
        el('txtBranchSrNo').value = String(get(inv, 'BranchSrNo') ?? '');
        el('DocDate').value = dateOnly(get(inv, 'DocDate'));
        showTab('form');
        el('cmbsuppliername').value = String(get(inv, 'SupplierCustomerId') ?? '');
        el('txtrefno').value = String(get(inv, 'SupplierReferenceNo') ?? '');
        el('txtbillno').value = String(get(inv, 'ManualBillNo') ?? '');
        el('txtDueDays').value = String(get(inv, 'DueDays') ?? '');
        el('DueDate').value = dateOnly(get(inv, 'DueDate'));
        S.commRate = String(get(inv, 'CommRate') ?? '');
        S.commAmount = String(get(inv, 'CommAmount') ?? '0');
        S.commTypeText = String(get(inv, 'CommissionType') ?? '');
        S.commUomText = String(get(inv, 'UomScheduleIdCmRate') ?? '');
        el('txtRemarks').value = String(get(inv, 'OtherRemarks') ?? '');
        el('txtBillAmount').value = String(get(inv, 'BillAmount') ?? '0');
        if (S.cfg.HasMultiCurrencyFeature) { el('cmbCurrency').value = String(get(inv, 'CurrencyId') ?? ''); el('txtExchangeRate').value = String(get(inv, 'ExchangeRate') ?? ''); }
        else { el('cmbCurrency').value = String(S.cfg.BaseCurrencyId ?? ''); el('txtExchangeRate').value = String(S.cfg.BaseCurrencyRate ?? ''); }
        el('txtFcyAmount').value = String(get(inv, 'FcyAmount') ?? '0');
        S.Approved = toBool(get(inv, 'IsApproved'));
        const loc = toInt(get(inv, 'LocationTypeId'));
        el('cmbLocationType').value = loc > 0 ? String(loc) : ((S.L.locationTypes || []).length ? '1' : '0');
        S.AttachmentsValues = get(inv, 'AttachmentsValues') ?? ''; S.CustomAttachmentsValues = get(inv, 'CustomAttachmentsValues') ?? '';
        el('chkCustomAccounts').checked = toBool(get(inv, 'CustomAccounts'));
        S.VoucherHeadId = toInt(get(inv, 'VoucherHeadId'));
        const details = get(inv, 'details') || [];
        S.hasSaleOrder = details.some(d => toInt(get(d, 'SaleOrderId')) > 0);
        S.dtGrid = details.map(d => ({OtherCategoryId: get(d, 'OtherCategoryId'), CategoryII: get(d, 'OtherCategory'), RefRefDocumentTypeId: get(d, 'RefRefDocumentTypeId'), RefRefDocIdNo: get(d, 'RefRefDocIdNo'),
            RefDocSubId: get(d, 'RefDocSubId'), PurchaseGLAC: get(d, 'SaleGLAC'), Id: get(d, 'InvGdnDetailId'), InvGdnId: get(d, 'InvGdnId'), ItemId: get(d, 'ItemId'), ItemCode: get(d, 'ItemCode'),
            ItemName: get(d, 'ItemName'), BrandItemId: get(d, 'BrandItemId'), BrandItemCode: get(d, 'BrandItemCode'), BrandItemName: get(d, 'BrandItemName'), CropYear: get(d, 'CropYear'),
            ItemUomId: get(d, 'ItemUOMId'), UOMCodeItem: get(d, 'UOMCodeItem'), PackEquivalent: get(d, 'PackEquivalent'), ItemQty: toDouble(get(d, 'ItemQty')), WarehouseId: toInt(get(d, 'WarehouseId')),
            Warehouse: get(d, 'WareHouseName'), PackingTypeId: get(d, 'PackingTypeId'), SaleOrderId: get(d, 'SaleOrderId'), SaleOrder: get(d, 'SaleOrder'), GrossWeight: toDouble(get(d, 'GrossWeight')),
            AddLsWt: toDouble(get(d, 'AdLsWeight')), EBWPerUnit: toDouble(get(d, 'EBWeight')), EmptyBagsDeduction: toDouble(get(d, 'EBTotalWt')), NetBillWeight: toDouble(get(d, 'NetBillWeight')),
            StockWeight: toDouble(get(d, 'NetStockWeight')), ItemRateWOExp: toDouble(get(d, 'ItemRateWOExp')), PackingAddLess: toDouble(get(d, 'PackingAddLess')), ItemRate: toDouble(get(d, 'ItemRate')),
            OrderItemRateUOMId: get(d, 'UomScheduleIdRate'), RateUOM: get(d, 'UOMCodeRate'), RateEquivalent: toDouble(get(d, 'RateUOM')), RateCut: toDouble(get(d, 'RateCut')),
            RateCutAmount: toDouble(get(d, 'RateCutAmount')), ItemAmount: toDouble(get(d, 'ItemAmount')), FcyAmount: toDouble(get(d, 'FcyAmount')), ItemDiscount: toDouble(get(d, 'ItemDiscount')),
            DiscAmount: toDouble(get(d, 'ItemDiscountAmount')), JobLotId: get(d, 'JobLotId'), BillAmount: toDouble(get(d, 'BillAmount')), Freights: toDouble(get(d, 'FreightAmount')),
            Expense: toDouble(get(d, 'ExpenseAmount')), Journal: toDouble(get(d, 'JournalAmount')), Commission: toDouble(get(d, 'CommissionAmount')), VehicleNo: get(d, 'VehicleNo'), GpNo: toInt(get(d, 'GpNo')),
            CommOnSale: toBool(get(d, 'CommOnSale')), CommPercent: 0, BranchId: toInt(get(d, 'BranchId')), BranchName: get(d, 'BranchName'), CostCenterId: toInt(get(d, 'CostCenterId')),
            SaleOrderPaymentTermId: toInt(get(d, 'PaymentTermId'))}));
        S.ItemIdsForComm = details.map(d => ',' + get(d, 'ItemId')).join('');
        S.dtInvExp = (get(inv, 'expenses') || []).map(x => ({ItemId: get(x, 'InvRevExpItemId'), Qty: get(x, 'Qty'), Rate: get(x, 'Rate'), Amount: get(x, 'Amount'), Remarks: get(x, 'CustomRemarks') ?? ''}));
        S.dtGrdGL = (get(inv, 'journals') || []).map(x => ({AccountId: get(x, 'TransporterSupCustId'), GlAccountId: get(x, 'ChartofAccountId'), Remarks: get(x, 'JvRemarks') ?? '', Percentage: get(x, 'JvPrcnt'), Qty: get(x, 'JvQty'), Rate: get(x, 'JvRate'), Debit: get(x, 'JvDebit'), Credit: get(x, 'JvCredit')}));
        S.dtComm = (get(inv, 'commissions') || []).map(x => ({Id: get(x, 'Id'), OrderId: get(x, 'SaleOrderId'), OrderNo: get(x, 'OrderNo'), InvoiceId: get(x, 'InvSaleInvoiceId'), CommissionAgentId: get(x, 'CommissionAgentId'), CommType: get(x, 'CommType'), CommRate: get(x, 'Rate'), CommUom: get(x, 'RateUom'), CommissionAmount: get(x, 'CommAmount'), DebitAccountId: get(x, 'CommDebitSupCustId'), Remarks: get(x, 'Remarks') ?? '', GlAccountId: get(x, 'DebitAccountId')}));
        S.dtFreight = (get(inv, 'freights') || []).map(x => ({InvGdnId: get(x, 'InvGdnId'), GlAccountId: get(x, 'TansporterId'), Transporter: get(x, 'TransporterSupCustId'), Freight: get(x, 'FreightAmount'), Debit: get(x, 'Debit'), Remarks: get(x, 'Remarks') ?? ''}));
        if (!S.dtInvExp.length) addRow('exp');
        if (!S.dtGrdGL.length) addRow('gl');
        if (!S.dtFreight.length) addRow('freight');
        if (!S.dtComm.length) addRow('comm');
        /* grdCommissionSettings / grdFreightSettings / gridGLSettings without feature 4: the value cell from GlAccountId. */
        if (!subsidiary()) {
            const inAccounts = gl => (S.L.accounts || []).some(a => toInt(get(a, 'Id')) === gl);
            for (const r of S.dtComm) { const gl = toInt(r.GlAccountId); if (gl && inAccounts(gl)) r.DebitAccountId = gl; }
            for (const r of S.dtFreight) { const gl = toInt(r.GlAccountId); if (gl && inAccounts(gl)) r.Transporter = gl; }
            for (const r of S.dtGrdGL) { const gl = toInt(r.GlAccountId); if (gl && inAccounts(gl)) r.AccountId = gl; }
        }
        S.dtPaymentTerm = [];
        const pt = get(inv, 'paymentTerms') || [];
        const systemGenerated = pt.length === 0 || pt.some(r => toBool(get(r, 'SystemGeneratedRow')));
        if (systemGenerated) for (const o of distinctOrders()) addPaymentRow(o.OrderId, o.OrderNo, o.PaymentTermId, 0, today());
        else for (const r of pt) S.dtPaymentTerm.push({SaleOrderId: get(r, 'SaleOrderId'), SaleOrderNo: get(r, 'SaleOrderNo'), ItemAmount: 0, Expense: 0, Freight: 0, PartyAddLessAmount: 0, Commission: 0, NetBillAmount: 0,
            PaymentTerm: get(r, 'PaymentTermId'), DueDays: get(r, 'DueDays'), DueDate: dateOnly(get(r, 'DueDate')), '%OfTotal': get(r, 'PrcntOfTotal'), Amount: get(r, 'Amount'), Remarks: get(r, 'PaymentRemarks') ?? ''});
        await supplierChanged();
        try { calculateOrderWiseAmountForPaymentDetail(); } catch (e) { message(e.message); }
        expProportion(); ledgerProportion(); freightProportion(); calculateCommissionGrid(); propotionateCommissiongrid(); totalCommissionAmount(); exchangeRateChanged();
        S.commCurrent = 0; bindCommGridOrderNo();
        billProportion();
        await commissionAmountCalculateInCaseofPolicy();
        renderAll();
        window.history.replaceState(null, '', '/sale/sale-invoice?id=' + S.Id);
        message(S.Approved ? 'This invoice is approved.' : '', true);
    }

    /* ------------------------------------------------------------------ Insert (:4054) */
    async function insert(button) {
        await run(button, async () => {
            if (!S.dtGrid.length) throw new Error('Grid Record Not Found');
            if (!formValidation()) return;
            if (!confirm(S.Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            totalCommissionAmount(); exchangeRateChanged(); freightProportion(); expProportion();
            const payload = {
                Id: S.Id, DocNo: el('txtdocno').value, BranchSrNo: el('txtBranchSrNo').value, DocDate: el('DocDate').value, SupplierCustomerId: el('cmbsuppliername').value,
                SupplierReferenceNo: el('txtrefno').value, ManualBillNo: el('txtbillno').value, Remarks: el('txtRemarks').value, DueDays: el('txtDueDays').value, DueDate: el('DueDate').value,
                UomScheduleIdCmRate: S.commUomText, LocationTypeId: el('cmbLocationType').value, CustomAccounts: el('chkCustomAccounts').checked,
                CurrencyId: el('cmbCurrency').value, ExchangeRate: el('txtExchangeRate').value, AttachmentsValues: S.AttachmentsValues, CustomAttachmentsValues: S.CustomAttachmentsValues,
                CommissionCurrentRow: S.commCurrent, PaymentByPercent: S.PCalculateByPercent,
                details: S.dtGrid, freights: S.dtFreight, journals: S.dtGrdGL,
                expenses: S.dtInvExp.map(r => ({...r, ItemName: ((S.L.otherItems || []).find(o => toInt(get(o, 'Id')) === toInt(r.ItemId)) || {}).OtherItemName || ''})),
                commissions: S.dtComm,
                paymentTerms: S.dtPaymentTerm.map(r => ({...r, PrcntOfTotal: r['%OfTotal']}))
            };
            const printVoucher = el('ChkBok').checked, printSlip = el('ChkPrintSlip').checked, openAdo = !S.Id && el('ChkOpenADOForm').checked;
            const date = el('DocDate').value, customer = toInt(el('cmbsuppliername').value);
            const result = await api('/save', payload);
            alert(result.message);
            await reset();
            if (openAdo) message('Advance Delivery Order (AdvanceDeliveryOrderPP) is not available on the web yet - open it on the desktop for invoice ' + result.docNo + '.');
            if (printVoucher) await print('acc-103', {id: result.voucherHeadId, documentTypeId: DOCUMENT_TYPE_ID});
            if (printSlip) await print('si-301', {id: result.id, fromDate: date, toDate: date, supplierCustomerId: customer});
        });
    }
    /* FormValidation (:1402) */
    function formValidation() {
        const loc = el('cmbLocationType');
        if ((S.L.locationTypes || []).length && toInt(loc.value) === 0) { alert('Location Type Field is Required'); loc.focus(); return false; }
        const doc = el('txtdocno').value.trim();
        if (doc === '' || doc === '0') { alert('DocNo  field is Required'); return false; }
        if (S.cfg.HasMultiCurrencyFeature) {
            if (toInt(el('cmbCurrency').value) === 0) { alert('Fcy Code Field is Required'); el('cmbCurrency').focus(); return false; }
            if (['', '0'].includes(el('txtExchangeRate').value.trim())) { alert('Exchange Rate Field is Required'); el('txtExchangeRate').focus(); return false; }
            if (['', '0'].includes(el('txtFcyAmount').value.trim())) { alert('Fcy Amount Field is Required'); return false; }
        } else {
            if (toInt(el('cmbCurrency').value) === 0) { alert('Please Configure Your Base Currency In configurations'); return false; }
            if (['', '0'].includes(el('txtExchangeRate').value.trim())) { alert('Please Configure Your Base Currency Rate In configurations'); return false; }
        }
        if (!toInt(el('cmbsuppliername').value)) { alert('Customer Name field is Required'); el('cmbsuppliername').focus(); return false; }
        return true;
    }

    /* ------------------------------------------------------------------ Delete (:4727) */
    async function remove(button) {
        await run(button, async () => {
            if (S.Approved) throw new Error('Record cannot be  Delete because Record has approved');
            if (!(S.Id > 0)) throw new Error('RecordId Not Found.....');
            if (!confirm('Are you sure to Delete?')) return;
            await api('/' + S.Id, null, 'DELETE');
            alert('Delete Record Successfully');
            await reset();
        });
    }

    /* ------------------------------------------------------------------ printing (CommonServices.*) */
    async function print(key, args) {
        const r = await PurchaseRequest.track(() => fetch(`/api/reports/${key}/print.pdf`, {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(args)}));
        if (!r.ok) { alert(await r.text()); return; }
        const blob = await r.blob();
        window.open(URL.createObjectURL(blob), '_blank');
    }
    async function print301(button, id, customerId, date) {
        await run(button, async () => { if (!(id > 0)) throw new Error('PrintId not found...'); await print('si-301', {id, fromDate: date, toDate: date, supplierCustomerId: customerId}); });
    }

    /* ------------------------------------------------------------------ History (FillHistoryGrid :4854) */
    function showTab(tab) {
        el('tabForm').hidden = tab !== 'form'; el('tabHistory').hidden = tab !== 'history';
        el('tabFormBtn').classList.toggle('active', tab === 'form'); el('tabHistoryBtn').classList.toggle('active', tab === 'history');
        el('tabFormBtn').setAttribute('aria-selected', tab === 'form'); el('tabHistoryBtn').setAttribute('aria-selected', tab === 'history');
        (tab === 'history' ? el('FromDateHistory') : el('DocDate')).focus();
    }
    function resetHistory() {
        el('FromDateHistory').value = addDays(today(), -3); el('ToDateHistory').value = today();
        el('FromDocNoHistory').value = ''; el('ToDocNoHistory').value = ''; el('CmbCustomerHistory').value = '0';
        el('grdHistory').innerHTML = ''; el('grdDetail').innerHTML = ''; el('drdocdate').checked = true;
    }
    async function refreshHistory(button) {
        await run(button, async () => {
            const L = await api('/lookups');
            S.L.historyBranches = L.historyBranches; S.L.historyCustomers = L.historyCustomers;
            bindLookups({...S.L});
        });
    }
    async function fillHistoryGrid(button) {
        await run(button, async () => {
            const branchIds = Array.from(el('cmbBranchName').querySelectorAll('input:checked')).map(x => x.value).join(',');
            if (!branchIds) throw new Error('Select branch first');
            const q = new URLSearchParams({dateMode: document.querySelector('input[name=histDate]:checked').value, branchIds});
            if (el('FromDateHistoryOn').checked && el('FromDateHistory').value) q.set('from', el('FromDateHistory').value);
            if (el('ToDateHistoryOn').checked && el('ToDateHistory').value) q.set('to', el('ToDateHistory').value);
            if (toInt(el('FromDocNoHistory').value)) q.set('fromDocNo', toInt(el('FromDocNoHistory').value));
            if (toInt(el('ToDocNoHistory').value)) q.set('toDocNo', toInt(el('ToDocNoHistory').value));
            if (toInt(el('CmbCustomerHistory').value)) q.set('customerId', toInt(el('CmbCustomerHistory').value));
            S.history = await api('/history?' + q);
            renderHistory();
            message(S.history.length + ' records', true);
        });
    }
    function renderHistory() {
        const showBranch = S.cfg.BranchFeature && !S.cfg.SaleInvoiceBranchWise;
        const f = [['DocNo', 'DocNo', 'link'], ['DocDate', 'DocDate', 'date'], ['DueDate', 'DueDate', 'date'], ...(showBranch ? [['BranchSrNo', 'BranchSrNo'], ['BranchName', 'BranchName']] : []),
            ['ManualBillNo', 'ManualBillNo'], ['CustomerName', 'CustomerName'], ['CommAgent', 'CommissionAgent'], ['CommRate', 'CommRate', 'rate'], ['CommAmount', 'CommAmount', 'amt'],
            ['BillAmount', 'BillAmount', 'amt'], ['EntryUser', 'UserName'], ['EntryDate', 'EntryDate', 'dt'], ['ModifyUser', 'ModifyUserName'], ['ModifyDate', 'ModifyDate', 'dt'],
            ['ApprovedUser', 'ApprovedUserName'], ['ApprovedDate', 'ApprovedDate', 'dt'], ['Remarks', 'RemarksHeader'], ['NoOfAttachments', 'NoOfAttachments']];
        const dt = v => { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return String(v); let h = d.getHours(); const ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12; return `${String(d.getDate()).padStart(2, '0')}-${String(d.getMonth() + 1).padStart(2, '0')}-${d.getFullYear()} ${String(h).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')} ${ap}`; };
        const rows = S.history || [];
        let html = '<thead><tr><th>Edit</th><th>Slip</th><th>303_Print</th><th>Print_303A</th><th>Voucher</th>' + f.map(x => `<th>${x[0]}</th>`).join('') + '</tr></thead><tbody>';
        rows.forEach((r, i) => {
            html += `<tr data-hist="${i}"><td><button type="button" class="gbtn" data-hact="edit" data-i="${i}">Edit</button></td><td><button type="button" class="gbtn" data-hact="slip" data-i="${i}">Slip</button></td>`
                + `<td><button type="button" class="gbtn" data-hact="303" data-i="${i}">303_Print</button></td><td><button type="button" class="gbtn" data-hact="303a" data-i="${i}">Print_303A</button></td>`
                + `<td><button type="button" class="gbtn" data-hact="voucher" data-i="${i}">Voucher</button></td>`
                + f.map(x => {
                    const v = get(r, x[1]);
                    if (x[2] === 'link') return `<td><a href="/sale/sale-invoice?id=${toInt(get(r, 'Id'))}" data-hact="edit" data-i="${i}">${esc(v)}</a></td>`;
                    if (x[2] === 'date') return `<td>${esc(dateOnly(v))}</td>`;
                    if (x[2] === 'dt') return `<td>${esc(dt(v))}</td>`;
                    if (x[2] === 'amt') return `<td class="n">${esc(fmt(v, S.dp))}</td>`;
                    if (x[2] === 'rate') return `<td class="n">${esc(fmtMax(v, 4))}</td>`;
                    return `<td>${esc(v)}</td>`;
                }).join('') + '</tr>';
        });
        html += '</tbody><tfoot><tr><td colspan="5"></td>' + f.map(x => ['CommAmount', 'BillAmount'].includes(x[0]) ? `<td class="n">${fmt(rows.reduce((a, r) => a + toDouble(get(r, x[1])), 0), S.dp)}</td>` : '<td></td>').join('') + '</tr></tfoot>';
        el('grdHistory').innerHTML = html;
        el('grdDetail').innerHTML = '';
    }
    /* GetDetailGrdByHeadId (:5166) */
    async function historyDetail(i) {
        const r = S.history[i]; if (!r) return;
        document.querySelectorAll('#grdHistory tr.si-selected').forEach(x => x.classList.remove('si-selected'));
        const tr = document.querySelector(`#grdHistory tr[data-hist="${i}"]`); if (tr) tr.classList.add('si-selected');
        const inv = await api('/' + toInt(get(r, 'Id')));
        const d = get(inv, 'details') || [];
        const showBranch = S.cfg.BranchFeature && !S.cfg.SaleInvoiceBranchWise;
        const f = [['CategoryII', 'OtherCategory'], ['OrderNo', 'SaleOrder'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['BrandItemCode', 'BrandItemCode'], ['BrandItemName', 'BrandItemName'],
            ['CropYear', 'CropYear'], ['ItemUOM', 'UOMCodeItem'], ['ItemQty', 'ItemQty', 'q2', 1], ['WareHouseName', 'WareHouseName'], ['PackType', 'PackTypeDesc'], ['SaleOrder', 'SaleOrder'],
            ['GrossWeight', 'GrossWeight', 'q2', 1], ['AddLsWt', 'AdLsWeight', 'q2', 1], ['EBTotalWt', 'EBTotalWt', 'q2', 1], ['WeightCutTotal', 'WeightCutTotal', 'q2', 1], ['NetBillWeight', 'NetBillWeight', 'q2', 1],
            ['NetStockWeight', 'NetStockWeight', 'q2', 1], ['Item Rate', 'ItemRateWOExp', 'rate'], ['PackingAddLess', 'PackingAddLess', 'q2', 1], ['Item Net Rate', 'ItemRate', 'rate'], ['RateUOM', 'UOMCodeRate'],
            ['RateCut', 'RateCut', 'rate'], ['RateCutAmount', 'RateCutAmount', 'amt', 1], ['ItemAmount', 'ItemAmount', 'amt', 1], ...(S.cfg.HasMultiCurrencyFeature ? [['FcyAmount', 'FcyAmount', 'amt', 1]] : []),
            ['JobLot', 'JobLotDescription'], ['CommissionAmount', 'CommissionAmount', 'q3', 1], ['ExpenseAmount', 'ExpenseAmount', 'q3', 1], ['FreightAmount', 'FreightAmount', 'q3', 1],
            ['Item Net Amount', 'BillAmount', 'amt', 1], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'], ...(showBranch ? [['BranchName', 'BranchName']] : [])];
        el('grdDetail').innerHTML = '<thead><tr>' + f.map(x => `<th>${esc(x[0])}</th>`).join('') + '</tr></thead><tbody>' + d.map(row => '<tr>' + f.map(x => `<td class="${x[2] ? 'n' : ''}">${esc(x[2] ? cellText(x[2], get(row, x[1])) : get(row, x[1]))}</td>`).join('') + '</tr>').join('')
            + '</tbody><tfoot><tr>' + f.map(x => x[3] ? `<td class="n">${esc(cellText(x[2], d.reduce((a, row) => a + toDouble(get(row, x[1])), 0)))}</td>` : '<td></td>').join('') + '</tr></tfoot>';
    }
    async function historyAction(act, i, button) {
        const r = S.history[i]; if (!r) return;
        const id = toInt(get(r, 'Id'));
        if (act === 'edit') await run(button, async () => { await reset(); await readById(id); });
        else if (act === 'slip') await print301(button, id, toInt(get(r, 'SupplierCustomerId')), dateOnly(get(r, 'DocDate')));
        else if (act === 'voucher') await run(button, () => print('acc-103', {id: toInt(get(r, 'VoucherHeadId')), documentTypeId: DOCUMENT_TYPE_ID}));
        else if (act === '303') await run(button, () => print('si-303', {id, documentTypeId: DOCUMENT_TYPE_ID}));
        else if (act === '303a') await run(button, () => print('si-303a', {id}));
    }

    /* ------------------------------------------------------------------ shortcut keys (MakeShortCutKeys :6699) */
    const SHORTCUTS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+S', 'For Save in form tab and for show data in history tab'], ['Ctrl+U', 'For Update'],
        ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+F5', 'For Focus on Customer Name'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+P', 'For 301 Print'], ['Alt+1', 'For Print Slip 303A'],
        ['Alt+2', 'For Print Slip 303'], ['Alt+3', 'For Print Voucher 103'], ['Ctrl+L', 'For Load GDN'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"], ['Ctrl+alt', 'To Show ShortCut Keys Form']];
    function showShortcuts() {
        el('shortcutGrid').innerHTML = '<thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' + SHORTCUTS.map(s => `<tr><td>${esc(s[0])}</td><td>${esc(s[1])}</td></tr>`).join('') + '</tbody>';
        el('shortcutDialog').hidden = false;
    }
    function keyDown(e) {
        if (e.key === 'Escape') { el('frmLoadGDN').hidden = true; el('shortcutDialog').hidden = true; return; }
        const k = e.key.toLowerCase(), onForm = !el('tabForm').hidden;
        if (e.ctrlKey && e.altKey) { showShortcuts(); return; }
        if (e.ctrlKey && k === 't') { e.preventDefault(); showTab(onForm ? 'history' : 'form'); return; }
        if (onForm) {
            const act = e.ctrlKey && e.shiftKey && e.key === 'Delete' ? () => !el('btnDelete').hidden && remove(el('btnDelete'))
                : e.ctrlKey && k === 'n' ? () => newRecord(el('btnNew'))
                : e.ctrlKey && k === 'r' ? () => refresh(el('btnRefresh'))
                : e.ctrlKey && k === 's' ? () => !el('btnSave').hidden && !el('btnSave').disabled && insert(el('btnSave'))
                : e.ctrlKey && k === 'u' ? () => !el('btnUpdate').hidden && !el('btnUpdate').disabled && insert(el('btnUpdate'))
                : e.ctrlKey && e.key === 'F5' ? () => el('cmbsuppliername').focus()
                : e.ctrlKey && e.key === 'F10' ? () => attachment()
                : e.ctrlKey && k === 'p' ? () => el('toolStripButton1').click()
                : e.altKey && k === '1' ? () => el('BtnItemSlip').click()
                : e.altKey && k === '2' ? () => el('btnSlipFormat2').click()
                : e.altKey && k === '3' ? () => el('btnPrint').click()
                : e.ctrlKey && k === 'l' ? () => openLoadGdn(el('toolStripButton3'))
                : e.ctrlKey && e.key === 'ArrowUp' ? () => el('cmbsuppliername').focus()
                : null;
            if (act) { e.preventDefault(); act(); }
        } else {
            const act = e.ctrlKey && k === 's' ? () => fillHistoryGrid(el('btnShowHistory')) : e.ctrlKey && k === 'n' ? () => resetHistory() : e.ctrlKey && k === 'r' ? () => refreshHistory(el('btnRefreshHistory')) : e.ctrlKey && e.key === 'F5' ? () => el('FromDateHistory').focus() : null;
            if (act) { e.preventDefault(); act(); }
        }
    }

    function attachment() { alert('Sale Invoice attachments are not available on the web yet. Existing attachment values are kept unchanged when the invoice is updated.'); }

    /* ------------------------------------------------------------------ New / Refresh */
    async function newRecord(button) { await run(button, reset); }
    async function refresh(button) {
        await run(button, async () => {
            const L = await api('/lookups');
            bindLookups(L);
            renderAll();
        });
    }

    /* ------------------------------------------------------------------ wiring */
    function init() {
        document.addEventListener('keydown', keyDown);
        document.querySelectorAll('.si-tab').forEach(b => b.addEventListener('click', () => showTab(b.dataset.tab)));
        document.querySelectorAll('.si-subtab').forEach(b => b.addEventListener('click', () => {
            document.querySelectorAll('.si-subtab').forEach(x => x.classList.toggle('active', x === b));
            el('tabDetail').hidden = b.dataset.subtab !== 'detail'; el('tabPaymentDetail').hidden = b.dataset.subtab !== 'payment';
        }));
        el('btnFooterHistory').addEventListener('click', () => showTab('history'));
        el('btnNew').addEventListener('click', e => newRecord(e.currentTarget));
        el('btnRefresh').addEventListener('click', e => refresh(e.currentTarget));
        el('btnSave').addEventListener('click', e => { S.Id = 0; insert(e.currentTarget); });
        el('btnUpdate').addEventListener('click', e => { if (!S.Id) { alert('Record not update because Id not found'); return; } insert(e.currentTarget); });
        el('btnDelete').addEventListener('click', e => remove(e.currentTarget));
        el('btnAttachment').addEventListener('click', attachment);
        el('toolStripButton1').addEventListener('click', e => print301(e.currentTarget, S.Id, toInt(el('cmbsuppliername').value), el('DocDate').value));
        el('BtnItemSlip').addEventListener('click', e => run(e.currentTarget, async () => { if (!(S.Id > 0)) throw new Error('No Record Found For Display'); await print('si-303a', {id: S.Id}); }));
        el('btnSlipFormat2').addEventListener('click', e => run(e.currentTarget, async () => { if (!(S.Id > 0)) throw new Error('No Record Found For Display'); await print('si-303', {id: S.Id, documentTypeId: DOCUMENT_TYPE_ID}); }));
        el('btnPrint').addEventListener('click', e => run(e.currentTarget, async () => { if (!S.VoucherHeadId) throw new Error('No Record Found For Display'); await print('acc-103', {id: S.VoucherHeadId, documentTypeId: DOCUMENT_TYPE_ID}); }));
        el('toolStripButton3').addEventListener('click', e => openLoadGdn(e.currentTarget));
        el('btnshortcutkeys').addEventListener('click', showShortcuts);
        el('shortcutClose').addEventListener('click', () => el('shortcutDialog').hidden = true);
        el('AdvanceDoFormLink').addEventListener('click', e => { e.preventDefault(); message('Advance Delivery Order (AdvanceDeliveryOrderPP) is not available on the web yet.'); });
        el('DocDate').addEventListener('change', async () => { dueDateGenerate(); try { await supplierChanged(); await commissionAmountCalculateInCaseofPolicy(); } catch (e) { alert(e.message); } renderAll(); });
        el('txtDueDays').addEventListener('input', () => { el('txtDueDays').value = el('txtDueDays').value.replace(/[^0-9]/g, ''); dueDateGenerate(); });
        el('cmbsuppliername').addEventListener('change', () => supplierChanged().then(renderAll).catch(e => alert(e.message)));
        el('cmbCurrency').addEventListener('change', async () => {
            /* cmbCurrency_Leave (:1210) - only when a currency is chosen and the rate is still 0. */
            const cur = toInt(el('cmbCurrency').value);
            if (cur === 0 || toDouble(el('txtExchangeRate').value) !== 0) return;
            if (cur !== toInt(S.cfg.BaseCurrencyId)) { const r = await api('/exchange-rate/' + cur); el('txtExchangeRate').value = r.rate != null ? String(r.rate) : '0'; }
            else el('txtExchangeRate').value = String(S.cfg.BaseCurrencyRate ?? '');
            exchangeRateChanged(); renderAll();
        });
        el('txtExchangeRate').addEventListener('input', () => { el('txtExchangeRate').value = el('txtExchangeRate').value.replace(/[^0-9.]/g, ''); exchangeRateChanged(); renderAll(); });
        for (const id of Object.values(ELEMENTS)) { el(id).addEventListener('change', onCellChange); el(id).addEventListener('click', onGridClick); }
        document.querySelector('.si-panel9').addEventListener('keydown', e => {
            if (!e.ctrlKey || !e.target.dataset || !e.target.dataset.grid) return;
            const key = e.target.dataset.grid, i = toInt(e.target.dataset.row);
            if (key === 'pay' && (e.key === 'd' || e.key === 'D')) { e.preventDefault(); paymentButton('add', i); renderAll(); }
            if (key === 'pay' && e.key === 'Delete') { e.preventDefault(); paymentButton('del', i); renderAll(); }
        });
        document.querySelector('.si-panel4').addEventListener('keydown', e => {
            if (!e.ctrlKey || !e.target.dataset || !e.target.dataset.grid) return;
            const key = e.target.dataset.grid;
            if (e.key === 'd' || e.key === 'D') { e.preventDefault(); if (canEdit()) { addRow(key); renderAll(); } }
        });
        /* History */
        el('btnShowHistory').addEventListener('click', e => fillHistoryGrid(e.currentTarget));
        el('btnNewHistory').addEventListener('click', resetHistory);
        el('btnRefreshHistory').addEventListener('click', e => refreshHistory(e.currentTarget));
        el('grdHistory').addEventListener('click', e => {
            const a = e.target.closest('[data-hact]');
            if (a) { e.preventDefault(); historyAction(a.dataset.hact, toInt(a.dataset.i), a.tagName === 'BUTTON' ? a : null); return; }
            const tr = e.target.closest('tr[data-hist]'); if (tr) historyDetail(toInt(tr.dataset.hist)).catch(err => alert(err.message));
        });
        el('grdHistory').addEventListener('dblclick', e => { const tr = e.target.closest('tr[data-hist]'); if (tr) historyAction('edit', toInt(tr.dataset.hist), null); });
        for (const id of ['FromDocNoHistory', 'ToDocNoHistory']) el(id).addEventListener('input', () => el(id).value = el(id).value.replace(/[^0-9]/g, ''));
        el('cmbBranchName').addEventListener('change', () => refreshHistory(null));
        /* frmLoadGDN */
        el('btnclose').addEventListener('click', () => el('frmLoadGDN').hidden = true);
        el('BtnShow').addEventListener('click', e => showPendingGdn(e.currentTarget));
        el('BtnLoadGdn').addEventListener('click', e => btnLoadGdn(e.currentTarget));
        el('grdPendingGdn').addEventListener('change', e => { if (e.target.id === 'selectAllGdn') document.querySelectorAll('[data-gdn-check]').forEach(x => x.checked = e.target.checked); });
        el('grdPendingGdn').addEventListener('click', e => { const tr = e.target.closest('tr[data-gdn]'); if (tr && !e.target.closest('a,input')) gdnSelectionChanged(toInt(tr.dataset.gdn)).catch(err => alert(err.message)); });
    }

    document.addEventListener('DOMContentLoaded', async () => {
        init();
        el('DocDate').value = today();
        await run(null, async () => {
            const L = await api('/lookups');
            bindLookups(L);
            el('FromDateHistory').value = S.cfg.DefaultDaysToLessFromHistoryFromDate > 0 ? addDays(today(), -S.cfg.DefaultDaysToLessFromHistoryFromDate) : addDays(today(), -3);
            el('ToDateHistory').value = today();
            el('txtdocno').value = L.docNo; el('txtBranchSrNo').value = L.branchSrNo;
            addRow('gl'); addRow('exp'); addRow('freight'); addRow('comm');
            dueDateGenerate();
            renderAll();
            const id = new URLSearchParams(window.location.search).get('id');
            if (id) await readById(id);
        });
    });
})();
