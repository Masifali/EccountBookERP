/* ============================================================================================
 * Sale Invoice Against GDN Without WB - frmSaleInvoiceAgainstGdnWithoutWb (DocumentTypeId 171).
 *
 * The desktop keeps four DataTables (dtGrid, dtFreight, dtGrdGL, dtInvExp) plus the header commission
 * controls, and recomputes them in the grids' CellUpdated handlers. The same arrays and handlers live
 * here, named after their C# methods (line numbers are frmSaleInvoiceAgainstGdnWithoutWb.cs). The
 * server repeats every validation and proportion before it posts; this file is only the form.
 *
 * .NET numerics: Math.Round(x) is half-to-even; custom format strings ("0,0", "#,##0.####") round
 * half away from zero; a double written to a cell through ToString() keeps 15 significant digits.
 * ============================================================================================ */
(function () {
    'use strict';
    const API = '/sale/sale-invoice-gdn-no-wb/api';
    const PAGE = '/sale/sale-invoice-gdn-no-wb';
    const DOCUMENT_TYPE_ID = 171;
    const el = id => document.getElementById(id);
    const esc = v => String(v ?? '').replace(/[&<>"']/g, c => ({'&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;'}[c]));
    const get = (r, k) => r == null ? undefined : (k in r ? r[k] : r[Object.keys(r).find(x => x.toLowerCase() === String(k).toLowerCase())]);

    /* ------------------------------------------------------------------ Conversion.* / Math.Round / formats */
    function toDouble(v) { if (v === null || v === undefined || v === '') return 0; if (typeof v === 'number') return isFinite(v) ? v : 0; const n = Number(String(v).replace(/,/g, '').trim()); return isFinite(n) ? n : 0; }
    function rint(x) { const f = Math.floor(x), d = x - f; if (d > 0.5) return f + 1; if (d < 0.5) return f; return f % 2 === 0 ? f : f + 1; }
    function toInt(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? rint(v) : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        const t = String(v).trim();
        return /^[+-]?\d+$/.test(t) ? parseInt(t, 10) : 0;
    }
    const cellInt = v => rint(toDouble(v));
    function toBool(v) { return v === true || v === 1 || String(v).toLowerCase() === 'true' || String(v) === '1'; }
    function roundAway(v, d) { const p = Math.pow(10, d), s = v * p, w = Math.trunc(s), f = s - w; return (Math.abs(f) >= 0.5 ? w + Math.sign(f) : w) / p; }
    /** double.ToString() on .NET Framework: "G" with 15 significant digits. */
    function g15(v) { if (!isFinite(v)) return String(v); return String(parseFloat(Number(v).toPrecision(15))); }
    const viaString = v => parseFloat(g15(v));
    function fmt(v, dp) { return roundAway(toDouble(v), dp).toLocaleString('en-US', {minimumFractionDigits: dp, maximumFractionDigits: dp}); }
    function fmtMax(v, max) { return roundAway(toDouble(v), max).toLocaleString('en-US', {minimumFractionDigits: 0, maximumFractionDigits: max}); }
    /** ToString("0,0"): at least two digits, grouped, half away from zero. */
    function f00(v) { const r = roundAway(toDouble(v), 0), a = Math.abs(r); const s = a < 10 ? String(a).padStart(2, '0') : a.toLocaleString('en-US'); return r < 0 ? '-' + s : s; }
    /** "0;(0);0" */
    function fParen(v) { const r = roundAway(toDouble(v), 0); return r < 0 ? `(${-r})` : String(r); }
    function today() { return iso(new Date()); }
    function iso(d) { return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`; }
    function addDays(dateStr, n) { const d = new Date((dateStr || today()) + 'T12:00:00'); d.setDate(d.getDate() + n); return iso(d); }
    function dateOnly(v) { return v ? String(v).replace(' ', 'T').slice(0, 10) : ''; }

    /* ------------------------------------------------------------------ state */
    const S = {
        L: null, cfg: {}, rights: {}, dp: 0, refreshed: false,
        Id: 0, Approved: false, VoucherHeadId: 0, AttachmentsValues: '', CustomAttachmentsValues: '',
        dtGrid: [], dtFreight: [], dtGrdGL: [], dtInvExp: [],
        SupplierGLId: 0, history: [], pendingGdn: []
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
    const COMM_TYPES = [[1, 'Flat'], [2, 'Percent'], [3, 'Comm Weight']];                                   /* CommissionTypeFill :570 */
    const COMM_UOMS = [[1, '1'], [2, '5'], [3, '10'], [4, '25'], [5, '40'], [6, '50'], [7, '60'], [8, '65'], [9, '80'], [10, '100']]; /* CommissionUOMFill :601 */
    const subsidiary = () => !!S.cfg.SubsidiaryAccountAllownOnVouchers;
    const commToExpenses = () => !!S.cfg.DebitAmountChargetoExpenseAcOfCommission;
    const freightToExpenses = () => !!S.cfg.DebitAmountChargetoExpenseAcFreightGrid;
    function accountTitle(value) { const key = subsidiary() ? 'SupplierCustomerId' : 'Id'; const r = (S.L.accounts || []).find(a => toInt(get(a, key)) === toInt(value) && toInt(value) !== 0); return r ? get(r, 'AccountTitle') : (toInt(value) ? String(value) : ''); }
    function glByTitle(title, previous) { if (!title || title === '0') return previous; const r = (S.L.accounts || []).find(a => get(a, 'AccountTitle') === title); return r ? toInt(get(r, 'Id')) : previous; }
    /** The UltraCombo Text: the display text of the selected row, or the raw text typed/assigned. */
    const comboText = id => { const o = el(id).selectedOptions[0]; return o && o.value !== '' ? o.textContent : ''; };
    function setComboText(id, text) {
        const node = el(id), t = String(text ?? '');
        node.querySelectorAll('option[data-raw]').forEach(o => o.remove());
        if (t === '') { node.value = ''; return; }
        const hit = Array.from(node.options).find(o => o.textContent === t && o.value !== '');
        if (hit) { node.value = hit.value; return; }
        const o = new Option(t, 'raw:' + t); o.dataset.raw = '1'; node.add(o); node.value = o.value;
    }
    const commTypeText = () => comboText('cmbcommtype');
    const commUomText = () => comboText('cmbcommuom');

    /* ------------------------------------------------------------------ grid definitions (grdSettings & friends) */
    const soLocked = () => S.dtGrid.length > 0 && toInt(S.dtGrid[0].SaleOrderId) > 0;
    const cols = {
        grd: () => [
            ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'], ['UOMCodeItem', 'UOMCodeItem'],
            ['ItemQty', 'ItemQty', 'raw'], ['SaleOrder', 'SaleOrder', 'so'], ['GrossWeight', 'GrossWeight', 'f00', 'sum'],
            ['EmptyBagsDeduction', 'EmptyBagsDeduction', 'raw'], ['EBWPerUnit', 'EBWPerUnit', 'raw'],
            ['NetBillWeight', 'NetBillWeight', 'f00', 'sum'], ['StockWeight', 'StockWeight', 'f00', 'sum'],
            ['ItemRate', 'ItemRate', 'raw', null, null, () => canEdit() && !soLocked()],
            ['RateUOM', 'RateUOM', 'paren', null, null, () => canEdit() && !soLocked()],
            ['RateCut', 'RateCut', 'raw', null, null, () => canEdit()],
            ['RateCutAmount', 'RateCutAmount', 'f00', 'sum'], ['ItemAmount', 'ItemAmount', 'f00', 'sum'],
            ['FcyAmount', 'FcyAmount', 'fcy', 'sumfcy', () => S.cfg.HasMultiCurrencyFeature],
            ['BillAmount', 'BillAmount', 'f00', 'sum'], ['Freights', 'Freights', 'f00', 'sum'], ['Expense', 'Expense', 'f00', 'sum'],
            ['Commission', 'Commission', 'f00', 'sum']
        ],
        freight: () => [
            ['Add', '+', 'add'], ['Delete', 'X', 'del'],
            ['Transporter', 'Transporter', 'list', null, null, () => canEdit(), () => (S.L.accounts || []).map(a => [get(a, subsidiary() ? 'SupplierCustomerId' : 'Id'), get(a, 'AccountTitle')])],
            ['Freight', 'Credit', 'amt', 'sumamt', null, () => canEdit()],
            ['Debit', 'Debit', 'amt', 'sumamt', () => freightToExpenses(), () => canEdit()],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()]
        ],
        gl: () => [
            ['Add', '+', 'add'], ['Delete', 'X', 'del'],
            ['AccountId', 'Account', 'list', null, null, () => canEdit(), () => (S.L.accounts || []).map(a => [get(a, subsidiary() ? 'SupplierCustomerId' : 'Id'), get(a, 'AccountTitle')])],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()],
            ['Percentage', 'Percentage', 'raw', null, null, () => canEdit()],
            ['Qty', 'Qty', 'q3', 'sumq3', null, () => canEdit()],
            ['Rate', 'Rate', 'rate', null, null, () => canEdit()],
            ['Debit', 'Debit', 'amt', 'sumamt', null, () => canEdit()],
            ['Credit', 'Credit', 'amt', 'sumamt', null, () => canEdit()]
        ],
        exp: () => [
            ['Add', '+', 'add'], ['Delete', 'X', 'del'],
            ['ItemId', 'Item', 'list', null, null, () => canEdit(), () => (S.L.otherItems || []).map(o => [get(o, 'Id'), get(o, 'OtherItemName')])],
            ['Qty', 'Qty', 'q3', 'sumq3', null, () => canEdit()],
            ['Rate', 'Rate', 'rate', null, null, () => canEdit()],
            ['Amount', 'Amount', 'q2', 'sumq2'],
            ['Remarks', 'Remarks', 's', null, null, () => canEdit()]
        ]
    };
    const TABLES = {grd: 'dtGrid', freight: 'dtFreight', gl: 'dtGrdGL', exp: 'dtInvExp'};
    const ELEMENTS = {grd: 'grd', freight: 'grdFreight', gl: 'grdGLedger', exp: 'grdInvExp'};
    function canEdit() { return !!S.rights[S.Id ? 'Update' : 'Save']; }

    function cellText(kind, v) {
        switch (kind) {
            case 'f00': return f00(v);
            case 'paren': return fParen(v);
            case 'amt': return fmt(v, S.dp);
            case 'fcy': return fmtMax(v, 4);
            case 'q2': return fmtMax(v, 2);
            case 'q3': return fmtMax(v, 3);
            case 'rate': return fmtMax(v, 4);
            default: return String(v ?? '');
        }
    }
    function totalText(kind, total) {
        switch (kind) {
            case 'sum': return fmtMax(total, 2);          /* "#,##0.##" */
            case 'sumfcy': return fmtMax(total, 4);
            case 'sumamt': return fmt(total, S.dp);
            case 'sumq3': return fmtMax(total, 3);       /* "#,##0.###" */
            case 'sumq2': return fmtMax(total, 2);       /* "#,##0.##" */
            default: return '';
        }
    }

    function renderGrid(key) {
        const table = el(ELEMENTS[key]), rows = S[TABLES[key]], defs = cols[key]().filter(c => !c[4] || c[4]());
        let html = '<thead><tr>' + defs.map(c => `<th>${esc(c[1])}</th>`).join('') + '</tr></thead><tbody>';
        rows.forEach((r, i) => {
            html += `<tr data-row="${i}">`;
            for (const c of defs) {
                const [name, , kind, , , editFn, listFn] = c;
                if (kind === 'add') { html += `<td class="btn"><button type="button" data-act="add" data-grid="${key}" data-row="${i}" aria-label="Add row" ${canEdit() ? '' : 'disabled'}>+</button></td>`; continue; }
                if (kind === 'del') { html += `<td class="btn"><button type="button" data-act="del" data-grid="${key}" data-row="${i}" aria-label="Delete row ${i + 1}" ${canEdit() ? '' : 'disabled'}>X</button></td>`; continue; }
                const v = r[name], editable = editFn ? !!editFn(r) : false, label = `aria-label="${esc(c[1])} row ${i + 1}"`;
                if (kind === 'list') {
                    const list = listFn ? listFn() : [];
                    if (editable) {
                        let opts = '<option value="0"></option>', found = false;
                        for (const [val, txt] of list) { const sel = String(val) === String(v); found = found || sel; opts += `<option value="${esc(val)}" ${sel ? 'selected' : ''}>${esc(txt)}</option>`; }
                        if (!found && v !== undefined && v !== null && String(v) !== '' && String(v) !== '0') opts += `<option value="${esc(v)}" selected>${esc(v)}</option>`;
                        html += `<td><select data-grid="${key}" data-row="${i}" data-col="${name}" ${label}>${opts}</select></td>`;
                    } else {
                        const hit = list.find(([val]) => String(val) === String(v));
                        html += `<td>${esc(hit ? hit[1] : (toInt(v) === 0 ? '' : v))}</td>`;
                    }
                    continue;
                }
                if (editable) {
                    const cls = kind === 's' ? '' : 'n';
                    html += `<td><input type="text" class="${cls}" data-grid="${key}" data-row="${i}" data-col="${name}" value="${esc(v ?? '')}" ${label}></td>`;
                } else if (kind === 'so' && toInt(r.SaleOrderId) > 0) {
                    html += `<td><a href="/sale/sale-order?id=${toInt(r.SaleOrderId)}" target="_blank" rel="noopener">${esc(v)}</a></td>`;
                } else {
                    html += `<td class="${!kind || kind === 's' || kind === 'so' ? '' : 'n'}">${esc(cellText(kind, v))}</td>`;
                }
            }
            html += '</tr>';
        });
        html += '</tbody>';
        if (defs.some(c => c[3])) {
            html += '<tfoot><tr>' + defs.map(c => c[3] ? `<td class="n">${esc(totalText(c[3], rows.reduce((a, r) => a + toDouble(r[c[0]]), 0)))}</td>` : '<td></td>').join('') + '</tr></tfoot>';
        }
        table.innerHTML = html;
    }
    function renderAll() { for (const k of Object.keys(TABLES)) renderGrid(k); rights(); }

    function rights() {
        const r = S.rights || {};
        el('btnSave').hidden = !!S.Id; el('btnUpdate').hidden = !S.Id; el('btnDelete').hidden = !S.Id;
        el('btnSave').disabled = !r.Save || PurchaseRequest.isBusy(el('btnSave'));
        el('btnUpdate').disabled = !r.Update || PurchaseRequest.isBusy(el('btnUpdate'));
        el('btnDelete').disabled = !r.Delete || PurchaseRequest.isBusy(el('btnDelete'));
        el('btnPrint').disabled = !r.Print;
    }

    /* ------------------------------------------------------------------ AddRow* */
    const blank = {
        freight: () => ({InvGdnId: 0, GlAccountId: 0, Transporter: 0, Freight: 0, Debit: 0, Remarks: ''}),
        gl: () => ({AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0}),
        exp: () => ({ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: ''})
    };
    function addRow(key) { S[TABLES[key]].push(blank[key]()); }

    /* ------------------------------------------------------------------ Load (:354) / ConfigurationDefault (:843) */
    function bindLookups(L) {
        S.L = L; S.cfg = L.settings || {}; S.rights = L.rights || {}; S.dp = toInt(S.cfg.DefaultNoofDecimalPointsForAmount);
        fillSelect('cmbCurrency', L.currencies, 'Id', 'CurrencyCode', true);
        const party = (id, blankFirst) => {
            const node = el(id), keep = node.value;
            node.replaceChildren(...(blankFirst ? [new Option('', '')] : []), ...(L.customers || []).map(c => { const o = new Option(String(get(c, 'CompanyName') ?? ''), String(get(c, 'Id'))); o.dataset.code = get(c, 'PartyCode') ?? ''; o.dataset.city = get(c, 'CityName') ?? ''; o.dataset.mobile = get(c, 'MobileNo') ?? ''; o.dataset.gl = get(c, 'GlAccountId') ?? 0; return o; }));
            node.value = keep;
        };
        party('cmbsuppliername', true); party('cmbcommagent', true);
        const keepType = commTypeText(), keepUom = commUomText();
        el('cmbcommtype').replaceChildren(new Option('', ''), ...COMM_TYPES.map(([v, t]) => new Option(t, String(v))));
        el('cmbcommuom').replaceChildren(new Option('', ''), ...COMM_UOMS.map(([v, t]) => new Option(t, String(v))));
        setComboText('cmbcommtype', keepType); setComboText('cmbcommuom', keepUom);
        fillSelect('CmbCommDebitAccount', L.commissionDebitAccounts, 'Id', 'AccountTitle', true);
        /* MultiCurrencyFeature (:814) and ConfigurationDefault: the commission debit account only with DebitAmountChargetoExpenseAcOfCommission. */
        document.querySelectorAll('.si-mc').forEach(x => x.hidden = !S.cfg.HasMultiCurrencyFeature);
        document.querySelectorAll('.nw-dr').forEach(x => x.hidden = !commToExpenses());
        el('CmbCommDebitAccount').disabled = !commToExpenses();
        if (!toInt(el('cmbCurrency').value)) el('cmbCurrency').value = String(S.cfg.BaseCurrencyId || '');
        if (!toDouble(el('txtExchangeRate').value)) el('txtExchangeRate').value = String(S.cfg.BaseCurrencyRate ?? '');
        fillSelect('CmbCustomerHistory', L.historyCustomers, 'Id', 'Customer', true);
        if (window.DesktopCombo) DesktopCombo.refresh();
    }
    function fillSelect(id, rows, key, label, blankFirst) {
        const node = el(id), keep = node.value;
        node.replaceChildren(...(blankFirst ? [new Option('', '0')] : []), ...(rows || []).map(r => new Option(String(get(r, label) ?? ''), String(get(r, key) ?? ''))));
        if (keep && Array.from(node.options).some(o => o.value === keep)) node.value = keep;
    }

    /* ------------------------------------------------------------------ Reset (:2513) */
    async function reset() {
        S.Id = 0; S.Approved = false; S.VoucherHeadId = 0; S.AttachmentsValues = ''; S.CustomAttachmentsValues = '';
        const codes = await api('/next-codes');
        el('txtdocno').value = codes.docNo;
        el('cmbsuppliername').disabled = false;
        for (const id of ['txtrefno', 'txtbillno', 'txtDueDays', 'txtRemarks', 'txtBillAmount', 'txtcommrate', 'txtcommamount', 'txtcommremarks', 'txtFcyAmount']) el(id).value = '';
        el('DueDate').value = today();
        el('cmbcommagent').value = ''; el('CmbCommDebitAccount').value = '0'; setComboText('cmbcommtype', ''); setComboText('cmbcommuom', '');
        S.dtGrid = []; S.dtFreight = []; S.dtInvExp = []; S.dtGrdGL = [];
        addRow('freight'); addRow('exp'); addRow('gl');
        if (!toInt(el('cmbCurrency').value)) el('cmbCurrency').value = String(S.cfg.BaseCurrencyId || '');
        if (!toDouble(el('txtExchangeRate').value)) el('txtExchangeRate').value = String(S.cfg.BaseCurrencyRate ?? '');
        if (window.DesktopCombo) DesktopCombo.refresh();
        renderAll();
        window.history.replaceState(null, '', PAGE);
        message('');
        el('cmbsuppliername').focus();
    }

    /* cmbsuppliername_ValueChanged (:745) - txtSupplierGLId. */
    function supplierChanged() {
        const opt = el('cmbsuppliername').selectedOptions[0];
        if (opt && opt.value) S.SupplierGLId = toInt(opt.dataset.gl);
    }

    /* ------------------------------------------------------------------ calculations (:3082 - :3380) */
    const sum = (rows, k) => rows.reduce((a, r) => a + toDouble(r[k]), 0);

    /* txtExchangeRate_TextChanged (:934) */
    function exchangeRateChanged() {
        const rate = toDouble(el('txtExchangeRate').value);
        for (const r of S.dtGrid) r.FcyAmount = S.dtGrid.length > 0 && rate > 0 ? toDouble(r.ItemAmount) / rate : 0;
        billAmount();
    }
    /* BillAmount (:3082) */
    function billAmount() {
        let jd = 0, jc = 0, tc = 0;
        for (const r of S.dtGrdGL) if (toInt(r.AccountId) > 0) { jd += toDouble(r.Debit); jc += toDouble(r.Credit); }
        /* txtSupplierGLId.Text == Transporter.ToString() */
        for (const r of S.dtFreight) if (toInt(r.Transporter) > 0 && String(S.SupplierGLId) === String(toInt(r.Transporter))) tc += toDouble(r.Freight);
        let bill = sum(S.dtGrid, 'ItemAmount') + sum(S.dtInvExp, 'Amount');
        if (tc > 0) bill -= Math.abs(tc);
        const gl = jc - jd;
        bill = !(gl < 0) ? bill + gl : bill - Math.abs(gl);
        if (toInt(el('cmbsuppliername').value) === toInt(el('cmbcommagent').value)) {
            if (el('txtcommrate').value !== '') bill -= toDouble(el('txtcommamount').value.trim());
            else el('txtcommamount').value = '0';
        }
        el('txtBillAmount').value = f00(rint(bill));
        const rate = toDouble(el('txtExchangeRate').value);
        el('txtFcyAmount').value = S.dtGrid.length > 0 ? fmtMax(bill / rate, 4) : '0';
        billProportion();
    }
    /* ExpProportion (:3159) */
    function expProportion() {
        const total = sum(S.dtInvExp, 'Amount'), w = sum(S.dtGrid, 'NetBillWeight');
        for (const r of S.dtGrid) r.Expense = total > 0 ? viaString(total / w * toDouble(r.NetBillWeight)) : 0;
        billProportion();
    }
    /* FreightProportion (:3183) */
    function freightProportion() {
        const w = sum(S.dtGrid, 'NetBillWeight'), credit = sum(S.dtFreight, 'Freight');
        for (const r of S.dtGrid) r.Freights = credit > 0 && !freightToExpenses() ? rint(credit) / w * toDouble(r.NetBillWeight) : 0;
        billProportion();
    }
    /* LedgerProportion (:3207) */
    function ledgerProportion() {
        const w = sum(S.dtGrid, 'NetBillWeight');
        const credit = sum(S.dtGrdGL, 'Credit') + toDouble(el('txtcommamount').value), debit = sum(S.dtGrdGL, 'Debit');
        for (const r of S.dtGrid) r.Journal = debit > credit ? (debit - credit) / w * toDouble(r.NetBillWeight) : 0;
        billProportion();
    }
    /* BillProportion (:3246) */
    function billProportion() {
        const raw = String(S.cfg.CreditAmountInItemSaleGLRaw ?? '').trim().toLowerCase();
        for (const r of S.dtGrid) {
            const full = toDouble(r.ItemAmount) + toDouble(r.Expense) - toDouble(r.Commission);
            if (raw === '' || raw === 'true') r.BillAmount = full;
            else if (raw === 'false') r.BillAmount = toDouble(r.ItemAmount);
            else return;
        }
    }
    /* CommissionProportion (:3264) */
    function commissionProportion() {
        const total = toDouble(el('txtcommamount').value), pct = toDouble(el('txtcommrate').value), w = sum(S.dtGrid, 'NetBillWeight');
        for (const r of S.dtGrid) {
            if (total > 0 && !commToExpenses()) r.Commission = commTypeText() === 'Percent' ? toDouble(r.ItemAmount) * pct / 100 : viaString(total / w * toDouble(r.NetBillWeight));
            else r.Commission = 0;
        }
        billProportion();
    }
    /* TotalCommissionAmount (:3300) */
    function totalCommissionAmount() {
        const rateText = el('txtcommrate').value;
        if (rateText === '') { el('txtcommamount').value = '0'; return; }
        const rate = toDouble(rateText.trim()), type = commTypeText();
        if (type === 'Flat') el('txtcommamount').value = g15(rate);
        if (type === 'Percent' || type === 'Percentage') el('txtcommamount').value = g15(rint(sum(S.dtGrid, 'ItemAmount') * rate / 100));
        if (type === 'Comm Weight') el('txtcommamount').value = g15(rint(sum(S.dtGrid, 'NetBillWeight') / toDouble(commUomText().trim()) * rate));
    }
    /* cmbcommtype_Leave / txtcommrate_TextChanged / cmbcommuom / cmbcommagent_Leave (:3341-3478) */
    function commissionChanged() { totalCommissionAmount(); billAmount(); commissionProportion(); renderAll(); }

    /* ------------------------------------------------------------------ grid CellUpdated handlers */
    /* grd_CellUpdated (:1845) - RateUOM is read from the cell Text ("0;(0);0"). */
    function grdCellUpdated(r, col) {
        if (col === 'RateCut' || col === 'ItemRate' || col === 'RateUOM') {
            const w = toDouble(r.NetBillWeight), eq = toDouble(fParen(r.RateUOM));
            const cut = w / eq * toDouble(r.RateCut);
            r.RateCutAmount = cut;
            r.ItemAmount = w / eq * toDouble(r.ItemRate) - cut;
        }
        exchangeRateChanged();
    }
    /* grdFreight_CellUpdated (:1205) */
    function freightCellUpdated(r, col) {
        if (col === 'Freight') { r.Freight = roundAway(toDouble(r.Freight), S.dp); if (toDouble(r.Debit) > 0) { r.Freight = 0; alert('Debit Side is aleady added'); } }
        if (col === 'Debit') { r.Debit = roundAway(toDouble(r.Debit), S.dp); if (toDouble(r.Freight) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); } }
        if (col === 'Transporter') for (const x of S.dtFreight) x.GlAccountId = glByTitle(accountTitle(x.Transporter), toInt(x.GlAccountId));
        freightProportion(); billAmount();
    }
    /* grdGLedger_CellUpdated (:1278) */
    function glCellUpdated(r, col) {
        if ((col === 'Qty' || col === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') {
            r.Credit = roundAway(toDouble(r.Qty) * toDouble(r.Rate), S.dp); r.Debit = 0; r.Percentage = 0;
        }
        if (col === 'Percentage') {
            const total = sum(S.dtGrdGL, 'Percentage');
            if (total > 100) { r.Percentage = toDouble(r.Percentage) - (total - 100); alert('TotalPercentage Can not be Greater than 100...'); return; }
            if (String(r.Percentage) !== '') {
                const v = sum(S.dtGrid, 'ItemAmount') / 100 * toDouble(r.Percentage);
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
    /* grdInvExp_CellUpdated (:1645) */
    function expCellUpdated(r, col) {
        if ((col === 'Qty' || col === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = roundAway(toDouble(r.Qty) * toDouble(r.Rate), S.dp);
        billAmount(); expProportion();
    }

    /* ------------------------------------------------------------------ grid events */
    const NUMERIC = {grd: ['ItemRate', 'RateCut', 'RateUOM'], freight: ['Freight', 'Debit'], gl: ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'], exp: ['Qty', 'Rate', 'Amount']};
    function onCellChange(e) {
        const t = e.target; if (!t.dataset || !t.dataset.grid) return;
        const key = t.dataset.grid, i = toInt(t.dataset.row), col = t.dataset.col, r = S[TABLES[key]][i];
        if (!r) return;
        /* *_UpdatingCell: "Please Type Only Numeric Value". */
        if ((NUMERIC[key] || []).includes(col) && t.value !== '' && !isFinite(Number(String(t.value).replace(/,/g, '')))) { alert('Please Type Only Numeric Value'); renderGrid(key); return; }
        /* dtGrid.ItemRate is typeof(int): a fraction cannot be stored in the cell. */
        if (key === 'grd' && col === 'ItemRate' && t.value !== '' && !/^[+-]?\d+$/.test(t.value.trim())) { alert('Invalid value: ItemRate is a whole number'); renderGrid(key); return; }
        r[col] = t.tagName === 'SELECT' ? (isFinite(Number(t.value)) ? Number(t.value) : t.value) : t.value;
        try {
            if (key === 'grd') grdCellUpdated(r, col);
            else if (key === 'freight') freightCellUpdated(r, col);
            else if (key === 'gl') glCellUpdated(r, col);
            else if (key === 'exp') expCellUpdated(r, col);
        } catch (err) { alert(err.message); }
        renderAll();
    }
    /* grdFreight / grdGLedger / grdInvExp _ColumnButtonClick */
    function rowButton(key, act, i) {
        const rows = S[TABLES[key]];
        if (act === 'del') { rows.splice(i, 1); if (!rows.length) addRow(key); } else addRow(key);
        if (key === 'freight') { freightProportion(); billAmount(); }
        else if (key === 'gl') billAmount();
        else if (key === 'exp') { expProportion(); billAmount(); }
    }
    function onGridClick(e) {
        const b = e.target.closest('button[data-act]'); if (!b || b.disabled) return;
        try { rowButton(b.dataset.grid, b.dataset.act, toInt(b.dataset.row)); } catch (err) { alert(err.message); }
        renderAll();
    }
    /* grd*_KeyDown: Ctrl+D adds a row, Ctrl+Delete deletes the current one after a confirm. */
    function onGridKey(e) {
        const t = e.target; if (!e.ctrlKey || !t.dataset || !t.dataset.grid || t.dataset.grid === 'grd' || !canEdit()) return;
        const key = t.dataset.grid, i = toInt(t.dataset.row);
        if (e.key === 'd' || e.key === 'D') { e.preventDefault(); e.stopPropagation(); rowButton(key, 'add', i); renderAll(); }
        else if (e.key === 'Delete') { e.preventDefault(); e.stopPropagation(); if (confirm('Are you sure to Delete?')) { rowButton(key, 'del', i); renderAll(); } }
    }

    /* ------------------------------------------------------------------ frmLoadGDN (BtnLoadGdn_Click :3478, DocumentTypeIds "170,221") */
    async function openLoadGdn(button) {
        await run(button, async () => {
            const data = await api('/pending-gdns');
            S.gdnCfg = data;
            el('CmbBranch').innerHTML = (data.branches || []).map(b => `<label><input type="checkbox" value="${esc(get(b, 'BranchId'))}" checked> ${esc(get(b, 'BranchName'))}</label>`).join('');
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
            const fields = ['CategoryII', 'DocumentType', 'DocDate', 'DocNo', 'CostCenter', ...(!data.branchImplemented ? ['BranchName'] : []), 'CustomerName', 'GpNO', 'BiltyNo', 'VehicleNo', 'ItemQty', 'TicketNos', 'OrderNo', ...(data.isStockReservedPerParty ? ['IsStockReserved'] : [])];
            const value = (r, f) => ({CategoryII: get(r, 'OtherCategory'), CostCenter: get(r, 'CostCenterName'), CustomerName: get(r, 'SupplierCustomer'),
                DocDate: formatDdMmmYy(get(r, 'DocDate')), IsStockReserved: toBool(get(r, 'IsStockReserved')) ? 'Reserved' : 'Not Reserved'}[f] ?? get(r, f));
            let html = '<thead><tr><th><input type="checkbox" id="selectAllGdn" aria-label="Select all GDNs"></th>' + fields.map(f => `<th>${esc(f)}</th>`).join('') + '</tr></thead><tbody>';
            S.pendingGdn.forEach((r, i) => {
                html += `<tr data-gdn="${i}"><td class="btn"><input type="checkbox" data-gdn-check="${i}" aria-label="Select GDN ${esc(get(r, 'DocNo'))}"></td>` + fields.map(f => {
                    const v = value(r, f);
                    if (f === 'DocNo') return `<td><a href="${toInt(get(r, 'DocumentTypeId')) === 221 ? '/sale/gdn-direct' : '/sale/gdn'}?id=${toInt(get(r, 'Id'))}" target="_blank" rel="noopener">${esc(v)}</a></td>`;
                    if (f === 'GpNO' && toInt(get(r, 'OutwardGatePassId')) > 0) return `<td><a href="/sale/outward-gate-pass?id=${toInt(get(r, 'OutwardGatePassId'))}" target="_blank" rel="noopener">${esc(v)}</a></td>`;
                    return `<td class="${f === 'ItemQty' ? 'n' : ''}">${esc(f === 'ItemQty' ? fmtMax(v, 0) : v)}</td>`;
                }).join('') + '</tr>';
            });
            html += '</tbody><tfoot><tr><td></td>' + fields.map(f => f === 'ItemQty' ? `<td class="n">${fmtMax(sum(S.pendingGdn.map(r => ({q: get(r, 'ItemQty')})), 'q'), 0)}</td>` : '<td></td>').join('') + '</tr></tfoot>';
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
    /* frmLoadGDN load button - the same-customer / type / order / cost centre / reserved checks. */
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
            }
            el('frmLoadGDN').hidden = true;
            await loadInGridDetail(ids);
        });
    }
    /**
     * LoadInGridDetail (:3575). The desktop runs it whenever frmLoadGDN closes - also after "Close"
     * (dtGDNS then holds a single 0), which empties the detail, transporter and expense grids.
     */
    async function loadInGridDetail(ids) {
        const data = ids.length ? await api('/load-gdns', {gdnIds: ids}) : {details: [], freights: [], soExpenses: []};
        S.dtGrid = [];
        /* LoadDataDetailGridAgainstGP (:3495) */
        const rows = data.details || [];
        if (rows.length) {
            const h = rows[0];
            el('cmbsuppliername').value = String(get(h, 'SupplierCustomerId') ?? ''); supplierChanged();
            el('cmbsuppliername').disabled = true;
            el('cmbcommagent').value = String(toInt(get(h, 'BrokerAgentSupCustId')) || '');
            setComboText('cmbcommtype', String(get(h, 'CommissionType') ?? ''));
            el('txtcommrate').value = get(h, 'CommRate') == null ? '' : g15(toDouble(get(h, 'CommRate')));
            setComboText('cmbcommuom', String(get(h, 'UomScheduleIdCmRate') ?? ''));
            el('txtcommamount').value = get(h, 'CommAmount') == null ? '' : g15(toDouble(get(h, 'CommAmount')));
            el('txtcommremarks').value = String(get(h, 'CommissionRemarks') ?? '');
            el('txtRemarks').value = String(get(h, 'RemarksHeader') ?? '');
            el('txtDueDays').value = get(h, 'OrderDueDays') == null ? '' : String(get(h, 'OrderDueDays'));
            dueDaysChanged();
            if (get(h, 'OrderDueDate')) el('DueDate').value = dateOnly(get(h, 'OrderDueDate'));
            for (const r of rows) {
                S.dtGrid.push({RefRefDocumentTypeId: get(r, 'RefDocumentTypeId'), RefRefDocIdNo: get(r, 'RefDocIdNo'), RefDocSubId: get(r, 'RefDocSubIdNo'), PurchaseGLAC: get(r, 'PurchaseGLAC'),
                    Id: get(r, 'Id'), InvGdnId: get(r, 'InvGdnId'), ItemId: get(r, 'ItemId'), ItemCode: get(r, 'ItemCode'), ItemName: get(r, 'ItemName'), CropYear: get(r, 'CropYear'),
                    ItemUomId: get(r, 'ItemUomId'), UOMCodeItem: get(r, 'UOMCodeItem'), ItemQty: get(r, 'ItemQty'), WarehouseId: get(r, 'WarehouseId'), PackingTypeId: get(r, 'PackingTypeId'),
                    SaleOrderId: get(r, 'SaleOrderId'), SaleOrder: get(r, 'SaleOrder') == null ? '' : String(get(r, 'SaleOrder')), GrossWeight: get(r, 'GrossWeight'),
                    EmptyBagsDeduction: get(r, 'EBWTotal'), EBWPerUnit: get(r, 'EBWPerUnit'), NetBillWeight: get(r, 'NetBillWeight'), StockWeight: get(r, 'StockWeight'),
                    ItemRate: get(r, 'OrderItemRate') == null ? '' : rint(toDouble(get(r, 'OrderItemRate'))), OrderItemRateUOMId: get(r, 'OrderItemRateUOMId'), RateUOM: get(r, 'EquivalentPoRate'),
                    RateCut: 0, RateCutAmount: 0, ItemAmount: get(r, 'ItemAmount'), FcyAmount: get(r, 'ItemAmount'), JobLotId: get(r, 'JobLotId'),
                    BillAmount: 0, Freights: 0, Expense: 0, Journal: 0, Commission: 0});
            }
        }
        /* LoadFreightData (:3528) */
        S.dtFreight = [];
        for (const f of data.freights || []) {
            if (S.dtFreight.some(x => toInt(x.InvGdnId) === toInt(get(f, 'MainId')))) continue;
            if (!(toInt(get(f, 'CarriageAmount')) > 0)) continue;
            let remarks = '';
            if (toInt(get(f, 'GpNo')) > 0) remarks += 'GpNo: ' + String(get(f, 'GpNo'));
            if (String(get(f, 'VehicleNo') ?? '') !== '') remarks += '   VehicleNo:  ' + String(get(f, 'VehicleNo'));
            if (String(get(f, 'BiltyNo') ?? '') !== '') remarks += '   BiltyNo: ' + String(get(f, 'BiltyNo'));
            S.dtFreight.push({InvGdnId: get(f, 'MainId'), GlAccountId: get(f, 'Transporter'), Transporter: subsidiary() ? get(f, 'TransporterSupCustId') : 0, Freight: get(f, 'CarriageAmount'), Debit: 0, Remarks: remarks});
        }
        if (!S.dtFreight.length) addRow('freight');
        transporterFromGl();
        /* LoadExpData (:3580): Amount is the BagPrice itself. */
        S.dtInvExp = (data.soExpenses || []).map(x => ({ItemId: 0, Qty: get(x, 'ItemQty'), Rate: get(x, 'BagPrice'), Amount: toDouble(get(x, 'BagPrice')), Remarks: '0'}));
        if (!S.dtInvExp.length) addRow('exp');
        totalCommissionAmount(); expProportion(); freightProportion(); commissionProportion(); ledgerProportion(); billProportion(); billAmount();
        if (window.DesktopCombo) DesktopCombo.refresh();
        renderAll();
    }
    /* grdFreightSettings / gridGLSettings without feature 4: the value cell from GlAccountId, then SupCustIdUpdate*. */
    function transporterFromGl() {
        const inAccounts = gl => (S.L.accounts || []).some(a => toInt(get(a, 'Id')) === gl);
        if (!subsidiary()) {
            for (const r of S.dtFreight) { const gl = toInt(r.GlAccountId); if (gl && inAccounts(gl)) r.Transporter = gl; }
            for (const r of S.dtGrdGL) { const gl = toInt(r.GlAccountId); if (gl && inAccounts(gl)) r.AccountId = gl; }
        }
        for (const x of S.dtFreight) x.GlAccountId = glByTitle(accountTitle(x.Transporter), toInt(x.GlAccountId));
        for (const x of S.dtGrdGL) x.GlAccountId = glByTitle(accountTitle(x.AccountId), toInt(x.GlAccountId));
    }

    /* txtDueDays_TextChanged (:3738) - DateTime.Now + days, not the doc date. */
    function dueDaysChanged() {
        const t = el('txtDueDays').value.trim();
        el('DueDate').value = t !== '' ? addDays(today(), toDouble(t)) : today();
    }

    /* ------------------------------------------------------------------ ReadById (:2344) */
    async function readById(id) {
        const inv = await api('/' + toInt(id));
        S.Id = toInt(get(inv, 'Id'));
        showTab('form');
        el('txtdocno').value = String(get(inv, 'DocNo') ?? '');
        el('DocDate').value = dateOnly(get(inv, 'DocDate'));
        el('cmbsuppliername').value = String(get(inv, 'SupplierCustomerId') ?? ''); supplierChanged();
        el('cmbsuppliername').disabled = true;
        el('txtrefno').value = String(get(inv, 'SupplierReferenceNo') ?? '');
        el('txtbillno').value = String(get(inv, 'ManualBillNo') ?? '');
        el('txtDueDays').value = String(get(inv, 'DueDays') ?? '');
        dueDaysChanged();
        el('DueDate').value = dateOnly(get(inv, 'DueDate')) || el('DueDate').value;
        el('txtBillAmount').value = g15(toDouble(get(inv, 'BillAmount')));
        el('txtRemarks').value = String(get(inv, 'RemarksHeader') ?? '');
        el('cmbcommagent').value = String(toInt(get(inv, 'CommissionAgentId')) || '');
        setComboText('cmbcommtype', String(get(inv, 'CommissionType') ?? ''));
        el('txtcommrate').value = g15(toDouble(get(inv, 'CommRate')));
        setComboText('cmbcommuom', String(get(inv, 'UomScheduleIdCmRate') ?? ''));
        el('txtcommamount').value = g15(toDouble(get(inv, 'CommAmount')));
        el('txtcommremarks').value = String(get(inv, 'CommissionRemarks') ?? '');
        el('CmbCommDebitAccount').value = String(toInt(get(inv, 'CommissionDebitAcGLId')));
        el('cmbCurrency').value = String(get(inv, 'CurrencyId') ?? '');
        el('txtExchangeRate').value = String(get(inv, 'ExchangeRate') ?? '');
        el('txtFcyAmount').value = String(get(inv, 'FcyAmount') ?? '');
        S.Approved = toBool(get(inv, 'IsApproved'));
        S.AttachmentsValues = get(inv, 'AttachmentsValues') ?? ''; S.CustomAttachmentsValues = get(inv, 'CustomAttachmentsValues') ?? '';
        S.VoucherHeadId = toInt(get(inv, 'VoucherHeadId'));
        S.dtGrid = (get(inv, 'details') || []).map(d => ({RefRefDocumentTypeId: get(d, 'RefRefDocumentTypeId'), RefRefDocIdNo: get(d, 'RefRefDocIdNo'), RefDocSubId: get(d, 'RefDocSubId'),
            PurchaseGLAC: get(d, 'SaleGLAC'), Id: get(d, 'InvGdnDetailId'), InvGdnId: get(d, 'InvGdnId'), ItemId: get(d, 'ItemId'), ItemCode: get(d, 'ItemCode'), ItemName: get(d, 'ItemName'),
            CropYear: get(d, 'CropYear'), ItemUomId: get(d, 'ItemUOMId'), UOMCodeItem: get(d, 'UOMCodeItem'), ItemQty: get(d, 'ItemQty'), WarehouseId: get(d, 'WarehouseId'),
            PackingTypeId: get(d, 'PackingTypeId'), SaleOrderId: get(d, 'SaleOrderId'), SaleOrder: get(d, 'SaleOrder') == null ? '' : String(get(d, 'SaleOrder')), GrossWeight: get(d, 'GrossWeight'),
            EmptyBagsDeduction: get(d, 'EBTotalWt'), EBWPerUnit: get(d, 'EBWeight'), NetBillWeight: get(d, 'NetBillWeight'), StockWeight: get(d, 'NetStockWeight'),
            ItemRate: rint(toDouble(get(d, 'ItemRate'))), OrderItemRateUOMId: get(d, 'UomScheduleIdRate'), RateUOM: get(d, 'RateUOM'), RateCut: get(d, 'RateCut'),
            RateCutAmount: get(d, 'RateCutAmount'), ItemAmount: get(d, 'ItemAmount'), FcyAmount: get(d, 'FcyAmount'), JobLotId: get(d, 'JobLotId'), BillAmount: get(d, 'BillAmount'),
            Freights: get(d, 'FreightAmount'), Expense: get(d, 'ExpenseAmount'), Journal: get(d, 'JournalAmount'), Commission: get(d, 'CommissionAmount')}));
        S.dtFreight = (get(inv, 'freights') || []).map(x => ({InvGdnId: get(x, 'InvGdnId'), GlAccountId: get(x, 'TansporterId'), Transporter: get(x, 'TransporterSupCustId'), Freight: get(x, 'FreightAmount'), Debit: get(x, 'Debit'), Remarks: get(x, 'Remarks') ?? ''}));
        if (!S.dtFreight.length) addRow('freight');
        S.dtInvExp = (get(inv, 'expenses') || []).map(x => ({ItemId: get(x, 'InvRevExpItemId'), Qty: get(x, 'Qty'), Rate: get(x, 'Rate'), Amount: get(x, 'Amount'), Remarks: get(x, 'Remarks') ?? ''}));
        if (!S.dtInvExp.length) addRow('exp');
        S.dtGrdGL = (get(inv, 'journals') || []).map(x => ({AccountId: get(x, 'TransporterSupCustId'), GlAccountId: get(x, 'ChartofAccountId'), Remarks: get(x, 'JvRemarks') ?? '', Percentage: get(x, 'JvPrcnt'), Qty: get(x, 'JvQty'), Rate: get(x, 'JvRate'), Debit: get(x, 'JvDebit'), Credit: get(x, 'JvCredit')}));
        if (!S.dtGrdGL.length) addRow('gl');
        transporterFromGl();
        totalCommissionAmount(); expProportion(); ledgerProportion(); freightProportion(); commissionProportion(); billProportion(); exchangeRateChanged();
        if (window.DesktopCombo) DesktopCombo.refresh();
        renderAll();
        window.history.replaceState(null, '', PAGE + '?id=' + S.Id);
        message(S.Approved ? 'This invoice is approved.' : '', true);
    }

    /* ------------------------------------------------------------------ Insert (:1887) */
    function localChecks() {
        if (!S.dtGrid.length) throw new Error('Grid Record Not Found');
        const forFreight = new Set(), forDebit = new Set();
        S.dtFreight.forEach((r, i) => {
            const t = toInt(r.Transporter), f = toDouble(r.Freight), d = toDouble(r.Debit);
            if (f > 0 && freightToExpenses()) forFreight.add(t);
            if (d > 0 && freightToExpenses()) forDebit.add(t);
            if (f > 0 && t === 0) throw new Error(`Please Select an Account Against Freight in Transporter Grid (row No: ${i + 1})`);
            if (t > 0 && f === 0 && d === 0) throw new Error(`Freight Required when Account Exists in Transporter Grid (row No: ${i + 1})`);
        });
        if (freightToExpenses()) for (const t of forFreight) if (forDebit.has(t)) throw new Error('Transporter for Freight Cannot be Same as Transporter for Debit.');
        S.dtGrdGL.forEach((r, i) => {
            if ((toDouble(r.Credit) > 0 || cellInt(r.Debit) > 0) && toInt(r.AccountId) === 0) throw new Error(`Please Select an Account  in JL Grid (row No: ${i + 1} )`);
            if (toDouble(r.Credit) === 0 && cellInt(r.Debit) === 0 && toInt(r.AccountId) > 0) throw new Error(`Please add Value in Debit or Credit in JL Grid (row No: ${i + 1} )`);
        });
        S.dtInvExp.forEach((r, i) => { if (toDouble(r.Amount) > 0 && toInt(r.ItemId) === 0) throw new Error(`Please Select an Item Against Expense Grid (row No: ${i + 1} )`); });
    }
    /* FormValidation (:979) */
    function formValidation() {
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
        if (toDouble(el('txtcommamount').value) > 0 && commToExpenses() && toInt(el('CmbCommDebitAccount').value) === 0) { alert('Commission Debit Ac field is Required'); el('CmbCommDebitAccount').focus(); return false; }
        return true;
    }
    async function insert(button) {
        await run(button, async () => {
            localChecks();
            if (!formValidation()) return;
            if (toDouble(el('txtcommamount').value) > 0 && toInt(el('cmbcommagent').value) === 0) { el('cmbcommagent').focus(); throw new Error('Please Select Commission Agent Account First'); }
            if (toDouble(el('txtcommamount').value) === 0 && toInt(el('cmbcommagent').value) > 0) throw new Error('Commission Amount Required when Commission Agent is Selected');
            if (!confirm(S.Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            totalCommissionAmount(); freightProportion(); expProportion(); commissionProportion(); exchangeRateChanged();
            const payload = {
                Id: S.Id, DocNo: el('txtdocno').value, DocDate: el('DocDate').value, SupplierCustomerId: el('cmbsuppliername').value,
                ManualBillNo: el('txtbillno').value, DueDays: el('txtDueDays').value, DueDate: el('DueDate').value, Remarks: el('txtRemarks').value,
                CommissionAgentId: el('cmbcommagent').value, CommissionType: commTypeText(), CommRate: el('txtcommrate').value, CommUom: commUomText(),
                CommAmount: el('txtcommamount').value, CommissionRemarks: el('txtcommremarks').value, CommissionDebitAccountId: el('CmbCommDebitAccount').value,
                CurrencyId: el('cmbCurrency').value, ExchangeRate: el('txtExchangeRate').value, AccountsAfterRefresh: S.refreshed,
                AttachmentsValues: S.AttachmentsValues, CustomAttachmentsValues: S.CustomAttachmentsValues,
                details: S.dtGrid, freights: S.dtFreight, journals: S.dtGrdGL, expenses: S.dtInvExp
            };
            const printVoucher = el('ChkBok').checked, printSlip = el('ChkPrintSlip').checked, printII = el('chkPrintII').checked;
            const result = await api('/save', payload);
            alert(result.message);
            await reset();
            if (printVoucher) await print('acc-103', {id: result.voucherHeadId, documentTypeId: DOCUMENT_TYPE_ID});
            if (printSlip) await print('si-318', {id: result.id, documentTypeId: DOCUMENT_TYPE_ID});
            if (printII) await print('si-318a', {id: result.id, documentTypeId: DOCUMENT_TYPE_ID});
        });
    }

    /* ------------------------------------------------------------------ Delete (:2448) */
    async function remove(button) {
        await run(button, async () => {
            if (S.Approved) throw new Error('Record cannot be  Delete beacause Record has approved');
            if (!confirm('Are you sure to Delete?')) return;
            const r = await api('/' + S.Id, null, 'DELETE');
            alert(r.message);
            await reset();
        });
    }

    /* ------------------------------------------------------------------ printing (CommonServices.*) */
    async function print(key, args) {
        const r = await PurchaseRequest.track(() => fetch(`/api/reports/${key}/print.pdf`, {method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(args)}));
        if (!r.ok) { alert(await r.text()); return; }
        window.open(URL.createObjectURL(await r.blob()), '_blank');
    }
    function printButton(button, work) { return run(button, work); }

    /* ------------------------------------------------------------------ History (GetAll :2701) */
    function showTab(tab) {
        el('tabForm').hidden = tab !== 'form'; el('tabHistory').hidden = tab !== 'history';
        el('tabFormBtn').classList.toggle('active', tab === 'form'); el('tabHistoryBtn').classList.toggle('active', tab === 'history');
        el('tabFormBtn').setAttribute('aria-selected', tab === 'form'); el('tabHistoryBtn').setAttribute('aria-selected', tab === 'history');
        (tab === 'history' ? el('FromDateHistory') : el('DocDate')).focus();
    }
    function historyFromDate() { const n = toInt(S.cfg.DefaultDaysToLessFromHistoryFromDate); return addDays(today(), n > 0 ? -n : -3); }
    /* btnNewHistory_Click (:2611) */
    function resetHistory() {
        el('FromDateHistory').value = historyFromDate(); el('ToDateHistory').value = today();
        el('FromDocNoHistory').value = ''; el('ToDocNoHistory').value = ''; el('CmbCustomerHistory').value = '0';
        S.history = []; el('grdHistory').innerHTML = ''; el('grdDetail').innerHTML = ''; el('drdocdate').checked = true;
        el('FromDateHistory').focus();
    }
    /* btnRefreshHistory_Click -> HistoryCombosFill (:2643) */
    async function refreshHistory(button) {
        await run(button, async () => {
            const L = await api('/lookups');
            S.L.historyCustomers = L.historyCustomers;
            fillSelect('CmbCustomerHistory', L.historyCustomers, 'Id', 'Customer', true);
        });
    }
    async function fillHistoryGrid(button) {
        await run(button, async () => {
            const q = new URLSearchParams({dateMode: document.querySelector('input[name=histDate]:checked').value});
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
    /* HistoryGridSettings (:2803): Edit / 318 Slip / 318A Slip / Voucher buttons first, Add Attachment last. */
    function renderHistory() {
        const f = [['DocNo', 'DocNo', 'link'], ['DocDate', 'DocDate', 'date'], ['DueDate', 'DueDate', 'date'], ['ManualBillNo', 'ManualBillNo'], ['CustomerName', 'CustomerName'],
            ['CommAgent', 'CommissionAgent'], ['CommRate', 'CommRate', 'raw'], ['CommAmount', 'CommAmount', 'raw'], ...(commToExpenses() ? [['CommDrAc', 'CommissionDebitAccount']] : []),
            ['BillAmount', 'BillAmount', 'raw'], ['EntryUser', 'UserName'], ['EntryDate', 'EntryDate', 'dt'], ['ModifyUser', 'ModifyUserName'], ['ModifyDate', 'ModifyDate', 'dt'],
            ['ApprovedUser', 'ApprovedUserName'], ['ApprovedDate', 'ApprovedDate', 'dt'], ['Remarks', 'RemarksHeader'], ['NoOfAttachments', 'NoOfAttachments', 'att']];
        const dt = v => { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return String(v); let h = d.getHours(); const ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12; return `${String(d.getDate()).padStart(2, '0')}-${String(d.getMonth() + 1).padStart(2, '0')}-${d.getFullYear()} ${String(h).padStart(2, '0')}:${String(d.getMinutes()).padStart(2, '0')} ${ap}`; };
        const rows = S.history || [];
        let html = '<thead><tr><th>Edit</th><th>318 Slip</th><th>318A Slip</th><th>Voucher</th>' + f.map(x => `<th>${x[0]}</th>`).join('') + '<th>Add Attachment</th></tr></thead><tbody>';
        rows.forEach((r, i) => {
            html += `<tr data-hist="${i}"><td><button type="button" class="gbtn" data-hact="edit" data-i="${i}">Edit</button></td><td><button type="button" class="gbtn" data-hact="318" data-i="${i}">318 Slip</button></td>`
                + `<td><button type="button" class="gbtn" data-hact="318a" data-i="${i}">318A Slip</button></td><td><button type="button" class="gbtn" data-hact="voucher" data-i="${i}">Voucher</button></td>`
                + f.map(x => {
                    const v = get(r, x[1]);
                    if (x[2] === 'link') return `<td><a href="${PAGE}?id=${toInt(get(r, 'Id'))}" data-hact="edit" data-i="${i}">${esc(v)}</a></td>`;
                    if (x[2] === 'att') return `<td><a href="#" data-hact="att" data-i="${i}">${esc(v)}</a></td>`;
                    if (x[2] === 'date') return `<td>${esc(dateOnly(v))}</td>`;
                    if (x[2] === 'dt') return `<td>${esc(dt(v))}</td>`;
                    if (x[2] === 'raw') return `<td class="n">${esc(v == null ? '' : g15(toDouble(v)))}</td>`;
                    return `<td>${esc(v)}</td>`;
                }).join('') + `<td><button type="button" class="gbtn" data-hact="att" data-i="${i}">Add Attachment</button></td></tr>`;
        });
        el('grdHistory').innerHTML = html + '</tbody>';
        el('grdDetail').innerHTML = '';
    }
    /* GetDetailGrdByHeadId (:2972). The desktop adds RateUOM into the ItemRate column and ItemRate into RateUOM. */
    async function historyDetail(i) {
        const r = S.history[i]; if (!r) return;
        document.querySelectorAll('#grdHistory tr.si-selected').forEach(x => x.classList.remove('si-selected'));
        const tr = document.querySelector(`#grdHistory tr[data-hist="${i}"]`); if (tr) tr.classList.add('si-selected');
        const inv = await api('/' + toInt(get(r, 'Id')));
        const d = get(inv, 'details') || [];
        const f = [['SaleOrder', 'SaleOrder'], ['GdnNo', 'ForwardingDocNo'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'], ['WareHouseName', 'WareHouseName'], ['ItemCode', 'ItemCode'],
            ['ItemName', 'ItemName'], ['CropYear', 'CropYear'], ['JobLot', 'JobLotDescription'], ['PackingType', 'PackTypeDesc'], ['ItemUOM', 'UOMCodeItem'],
            ['ItemQty', 'ItemQty', v => fmtMax(v, 0), 1], ['GrossWeight', 'GrossWeight', f00, 1], ['WeightCutTotal', 'WeightCutTotal', f00, 1], ['EBTotalWt', 'EBTotalWt', f00, 1],
            ['NetBillWeight', 'NetBillWeight', f00, 1], ['NetStockWeight', 'NetStockWeight', f00, 1], ['ItemRate', 'RateUOM', v => fmtMax(v, 4)], ['RateUOM', 'ItemRate', v => g15(toDouble(v))],
            ['RateCut', 'RateCut', v => fmtMax(v, 4), 1], ['RateCutAmount', 'RateCutAmount', v => fmt(v, S.dp), 1], ['ItemAmount', 'ItemAmount', v => fmt(v, S.dp), 1],
            ...(S.cfg.HasMultiCurrencyFeature ? [['FcyAmount', 'FcyAmount', v => fmt(v, S.dp), 1]] : []),
            ['FreightAmount', 'FreightAmount', v => fmt(v, S.dp), 1], ['ExpenseAmount', 'ExpenseAmount', v => fmt(v, S.dp), 1], ['CommissionAmount', 'CommissionAmount', v => fmt(v, S.dp), 1]];
        el('grdDetail').innerHTML = '<thead><tr>' + f.map(x => `<th>${esc(x[0])}</th>`).join('') + '</tr></thead><tbody>'
            + d.map(row => '<tr>' + f.map(x => `<td class="${x[2] ? 'n' : ''}">${esc(x[2] ? x[2](get(row, x[1])) : get(row, x[1]))}</td>`).join('') + '</tr>').join('')
            + '</tbody><tfoot><tr>' + f.map(x => x[3] ? `<td class="n">${esc(fmtMax(d.reduce((a, row) => a + toDouble(get(row, x[1])), 0), 2))}</td>` : '<td></td>').join('') + '</tr></tfoot>';
    }
    /* grdHistory_ColumnButtonClick (:2865) */
    async function historyAction(act, i, button) {
        const r = S.history[i]; if (!r) return;
        const id = toInt(get(r, 'Id'));
        if (act === 'edit') await run(button, async () => { await reset(); await readById(id); });
        else if (act === '318') await printButton(button, async () => { if (!(id > 0)) throw new Error('No Record Found For Display'); await print('si-318', {id, documentTypeId: DOCUMENT_TYPE_ID}); });
        else if (act === '318a') await printButton(button, () => print('si-318a', {id, documentTypeId: DOCUMENT_TYPE_ID}));
        else if (act === 'voucher') await printButton(button, () => print('acc-103', {id: toInt(get(r, 'VoucherHeadId')), documentTypeId: DOCUMENT_TYPE_ID}));
        else if (act === 'att') attachment();
    }

    /* ------------------------------------------------------------------ shortcut keys (MakeShortCutKeys :3999) */
    const SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Doc Date'], ['Ctrl+ArrowRight', 'For Focus From One Grid To Another'],
        ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];
    function showShortcuts() {
        el('shortcutGrid').innerHTML = '<thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' + SHORTCUTS.map(s => `<tr><td>${esc(s[0])}</td><td>${esc(s[1])}</td></tr>`).join('') + '</tbody>';
        el('shortcutDialog').hidden = false;
    }
    function focusGrid(id) { const f = el(id).querySelector('input,select,button,a'); if (f) f.focus(); else el(id).closest('.si-gridwrap').focus(); }
    const GRID_CYCLE = ['grd', 'grdFreight', 'grdInvExp', 'grdGLedger'];
    /* InvfrmPurchaseInvoice_KeyDown (:3858) */
    function keyDown(e) {
        if (e.key === 'Escape') { if (!el('frmLoadGDN').hidden) { closeLoadGdn(); return; } el('shortcutDialog').hidden = true; return; }
        const k = e.key.toLowerCase(), onForm = !el('tabForm').hidden;
        if (e.ctrlKey && e.altKey) { showShortcuts(); return; }
        if (!el('frmLoadGDN').hidden) {
            if (e.ctrlKey && k === 's') { e.preventDefault(); showPendingGdn(el('BtnShow')); }
            if (e.ctrlKey && k === 'l') { e.preventDefault(); btnLoadGdn(el('BtnLoadGdn')); }
            return;
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); showTab(onForm ? 'history' : 'form'); return; }
        let act = null;
        if (!onForm) {
            act = e.ctrlKey && e.key === 'ArrowUp' ? () => el('FromDateHistory').focus()
                : e.ctrlKey && e.key === 'ArrowDown' ? () => focusGrid('grdHistory')
                : e.ctrlKey && e.key === 'ArrowRight' ? () => focusGrid(el('grdHistory').contains(document.activeElement) ? 'grdDetail' : 'grdHistory')
                : e.ctrlKey && k === 's' ? () => fillHistoryGrid(el('btnShowHistory'))
                : e.ctrlKey && k === 'n' ? () => resetHistory()
                : e.ctrlKey && k === 'r' ? () => refreshHistory(el('btnRefreshHistory')) : null;
        } else {
            act = e.ctrlKey && k === 'r' ? () => refresh(el('btnRefresh'))
                : e.ctrlKey && k === 'n' ? () => newRecord(el('btnNew'))
                : e.ctrlKey && k === 'p' ? () => el('btnSlip318').click()
                : e.ctrlKey && k === 's' ? () => !el('btnSave').hidden && !el('btnSave').disabled && insert(el('btnSave'))
                : e.ctrlKey && k === 'u' ? () => !el('btnUpdate').hidden && !el('btnUpdate').disabled && insert(el('btnUpdate'))
                : e.ctrlKey && e.key === 'ArrowDown' ? () => focusGrid('grd')
                : e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5') ? () => el('DocDate').focus()
                : e.ctrlKey && e.key === 'ArrowRight' ? () => { const cur = GRID_CYCLE.findIndex(id => el(id).contains(document.activeElement)); focusGrid(GRID_CYCLE[cur < 0 ? 0 : (cur + 1) % GRID_CYCLE.length]); }
                : e.ctrlKey && e.key === 'F10' ? () => attachment() : null;
        }
        if (act) { e.preventDefault(); act(); }
    }

    function attachment() { alert('Sale Invoice attachments are not available on the web yet. Existing attachment values are kept unchanged when the invoice is updated.'); }

    /* ------------------------------------------------------------------ New / Refresh (:2483 / :2493) */
    async function newRecord(button) { await run(button, reset); }
    async function refresh(button) {
        await run(button, async () => {
            const L = await api('/lookups?refresh=true');
            S.refreshed = true;
            bindLookups(L);
            transporterFromGl();
            renderAll();
        });
    }
    function closeLoadGdn() { el('frmLoadGDN').hidden = true; loadInGridDetail([]).catch(e => alert(e.message)); }

    /* ------------------------------------------------------------------ wiring */
    function init() {
        document.addEventListener('keydown', keyDown);
        document.querySelectorAll('.si-tab').forEach(b => b.addEventListener('click', () => showTab(b.dataset.tab)));
        el('btnFooterHistory').addEventListener('click', () => showTab('history'));
        el('btnNew').addEventListener('click', e => newRecord(e.currentTarget));
        el('btnRefresh').addEventListener('click', e => refresh(e.currentTarget));
        el('btnSave').addEventListener('click', e => { S.Id = 0; insert(e.currentTarget); });
        el('btnUpdate').addEventListener('click', e => { if (!S.Id) { alert('Record not update because Id not found'); return; } insert(e.currentTarget); });
        el('btnDelete').addEventListener('click', e => remove(e.currentTarget));
        el('btnAttachment').addEventListener('click', attachment);
        el('btnPrint').addEventListener('click', e => printButton(e.currentTarget, async () => { if (!S.VoucherHeadId) throw new Error('No Record Found For Display'); await print('acc-103', {id: S.VoucherHeadId, documentTypeId: DOCUMENT_TYPE_ID}); }));
        el('btnSlip318').addEventListener('click', e => printButton(e.currentTarget, async () => { if (!(S.Id > 0)) throw new Error('No Record Found For Display'); await print('si-318', {id: S.Id, documentTypeId: DOCUMENT_TYPE_ID}); }));
        el('btnSlip318A').addEventListener('click', e => printButton(e.currentTarget, async () => { if (!(S.Id > 0)) throw new Error('No Record Found For Display'); await print('si-318a', {id: S.Id, documentTypeId: DOCUMENT_TYPE_ID}); }));
        el('toolStripButton3').addEventListener('click', e => openLoadGdn(e.currentTarget));
        el('BtnShortCutkeys').addEventListener('click', showShortcuts);
        el('shortcutClose').addEventListener('click', () => el('shortcutDialog').hidden = true);
        el('cmbsuppliername').addEventListener('change', () => { supplierChanged(); billAmount(); renderAll(); });
        el('txtDueDays').addEventListener('input', () => { el('txtDueDays').value = el('txtDueDays').value.replace(/[^0-9]/g, ''); dueDaysChanged(); });
        el('txtcommrate').addEventListener('input', () => { el('txtcommrate').value = el('txtcommrate').value.replace(/[^0-9.]/g, ''); commissionChanged(); });
        for (const id of ['cmbcommtype', 'cmbcommuom', 'cmbcommagent', 'CmbCommDebitAccount']) el(id).addEventListener('change', commissionChanged);
        el('cmbCurrency').addEventListener('change', async () => {
            /* cmbCurrency_Leave (:891) - only when a currency is chosen and the rate is still 0. */
            const cur = toInt(el('cmbCurrency').value);
            if (cur === 0 || toDouble(el('txtExchangeRate').value) !== 0) return;
            try {
                if (cur !== toInt(S.cfg.BaseCurrencyId)) { const r = await api('/exchange-rate/' + cur); el('txtExchangeRate').value = r.rate != null ? String(r.rate) : '0'; }
                else el('txtExchangeRate').value = String(S.cfg.BaseCurrencyRate ?? '');
            } catch (e) { alert(e.message); }
            exchangeRateChanged(); renderAll();
        });
        el('txtExchangeRate').addEventListener('input', () => { el('txtExchangeRate').value = el('txtExchangeRate').value.replace(/[^0-9.]/g, ''); exchangeRateChanged(); renderAll(); });
        for (const id of Object.values(ELEMENTS)) { el(id).addEventListener('change', onCellChange); el(id).addEventListener('click', onGridClick); el(id).addEventListener('keydown', onGridKey); }
        /* History */
        el('btnShowHistory').addEventListener('click', e => fillHistoryGrid(e.currentTarget));
        el('btnNewHistory').addEventListener('click', resetHistory);
        el('btnRefreshHistory').addEventListener('click', e => refreshHistory(e.currentTarget));
        el('grdHistory').addEventListener('click', e => {
            const a = e.target.closest('[data-hact]');
            if (a) { e.preventDefault(); historyAction(a.dataset.hact, toInt(a.dataset.i), a.tagName === 'BUTTON' ? a : null); return; }
            const tr = e.target.closest('tr[data-hist]'); if (tr) historyDetail(toInt(tr.dataset.hist)).catch(err => alert(err.message));
        });
        el('grdHistory').addEventListener('dblclick', e => { const tr = e.target.closest('tr[data-hist]'); if (tr) run(null, () => readById(toInt(get(S.history[toInt(tr.dataset.hist)], 'Id')))); });
        el('grdHistory').addEventListener('keydown', e => {
            if (!e.ctrlKey) return;
            const tr = e.target.closest('tr[data-hist]'); if (!tr) return;
            if (e.key === 'Enter') { e.preventDefault(); run(null, () => readById(toInt(get(S.history[toInt(tr.dataset.hist)], 'Id')))); }
            if (e.key === ' ' && e.target.dataset.hact) { e.preventDefault(); e.target.click(); }
        });
        for (const id of ['FromDocNoHistory', 'ToDocNoHistory']) el(id).addEventListener('input', () => el(id).value = el(id).value.replace(/[^0-9]/g, ''));
        /* frmLoadGDN */
        el('btnclose').addEventListener('click', closeLoadGdn);
        el('BtnShow').addEventListener('click', e => showPendingGdn(e.currentTarget));
        el('BtnLoadGdn').addEventListener('click', e => btnLoadGdn(e.currentTarget));
        el('grdPendingGdn').addEventListener('change', e => { if (e.target.id === 'selectAllGdn') document.querySelectorAll('[data-gdn-check]').forEach(x => x.checked = e.target.checked); });
        el('grdPendingGdn').addEventListener('click', e => { const tr = e.target.closest('tr[data-gdn]'); if (tr && !e.target.closest('a,input')) gdnSelectionChanged(toInt(tr.dataset.gdn)).catch(err => alert(err.message)); });
    }

    document.addEventListener('DOMContentLoaded', async () => {
        init();
        el('DocDate').value = today();
        el('DueDate').value = today();
        await run(null, async () => {
            const L = await api('/lookups');
            bindLookups(L);
            el('FromDateHistory').value = historyFromDate();
            el('ToDateHistory').value = today();
            el('txtdocno').value = L.docNo;
            addRow('gl'); addRow('exp'); addRow('freight');
            renderAll();
            const id = new URLSearchParams(window.location.search).get('id');
            if (id) await readById(id);
            else el('DocDate').focus();
        });
    });
})();
