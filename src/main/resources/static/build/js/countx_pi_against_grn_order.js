/* ============================================================================================
 * Purchase Invoice Against GRN Order - desktop Architecture.WinApp.Purchase.PurchaseInvoiceAgainstGrnOrder
 * (ScreenDefinition 132, DocumentTypeId 172, GRN loader frmLoadGRN DocumentTypeId 169). Line numbers in
 * comments are that .cs file.
 *
 * The browser does what the form's events do (grid edits, proportions, commission / brokery, bill amount)
 * so the operator sees the desktop numbers while typing; the server repeats every derived amount on Save and
 * re-reads every GRN value, so nothing computed here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/purchase-invoice-against-grn-order';
    var TYPE = 172, GRN_TYPE = 169;
    var $ = function (id) { return document.getElementById(id); };
    var S = {
        rows: [], freight: [], ledger: [], expenses: [], bags: [], lookups: {}, cfg: {}, rights: {},
        id: 0, voucherHeadId: 0, approved: false, invoiceTypeId: 1, loadMode: '', supplierGl: 0,
        suppliers: {}, history: [], historySel: -1, attachments: null, loaderRows: []
    };

    // ------------------------------------------------------------------ helpers
    function num(v) { if (v === null || v === undefined) return 0; var x = parseFloat(String(v).replace(/,/g, '')); return isFinite(x) ? x : 0; }
    function toInt(v) { var x = num(v), r = Math.round(x); if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2); return r; } /* Convert.ToInt32 */
    function esc(v) { return String(v === null || v === undefined ? '' : v).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }
    function away(v, d) { var p = Math.pow(10, d || 0), x = Math.round(Math.abs(v) * p); return (v < 0 ? -x : x) / p; }
    function bankers(v) { var f = Math.floor(v), diff = v - f; if (Math.abs(diff - 0.5) < 1e-9) return f % 2 === 0 ? f : f + 1; return Math.round(v); } /* Math.Round */
    function group(intText) { return intText.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* .NET custom formats used by the form's grids and boxes */
    function fmt(v, pattern) {
        var x = num(v);
        switch (pattern) {
            case '0,0': { var r = away(x, 0), t = String(Math.abs(r)); if (t.length < 2) t = '0' + t; return (r < 0 ? '-' : '') + group(t); }
            case '#,#': { if (away(x, 0) === 0) return ''; var r2 = away(x, 0); return (r2 < 0 ? '-' : '') + group(String(Math.abs(r2))); }
            case '#,##0': { var r3 = away(x, 0); return (r3 < 0 ? '-' : '') + group(String(Math.abs(r3))); }
            case '0;(0);0': { var r4 = away(x, 0); return r4 < 0 ? '(' + Math.abs(r4) + ')' : String(r4); }
            case '0.00;(0.00);0': { var r5 = away(x, 2); if (r5 === 0) return '0'; return r5 < 0 ? '(' + Math.abs(r5).toFixed(2) + ')' : r5.toFixed(2); }
            default: { /* #,##0.## */ var r6 = away(x, 2), p = Math.abs(r6).toFixed(2).split('.'), dec = p[1].replace(/0+$/, ''); return (r6 < 0 ? '-' : '') + group(p[0]) + (dec ? '.' + dec : ''); }
        }
    }
    function today() { var d = new Date(); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function addDays(iso, days) { var d = new Date(iso + 'T00:00:00'); d.setDate(d.getDate() + days); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function msg(text) { if (text) window.alert(text); }
    function sum(list, key) { return list.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    function setVal(id, v) { $(id).value = (v === null || v === undefined) ? '' : String(v); }
    function option(sel, rows, idKey, textKey, blank) {
        var el = $(sel), keep = el.value;
        el.innerHTML = (blank !== undefined ? '<option value="">' + esc(blank) + '</option>' : '') + rows.map(function (r) { return '<option value="' + esc(r[idKey]) + '">' + esc(r[textKey]) + '</option>'; }).join('');
        if (keep && rows.some(function (r) { return String(r[idKey]) === keep; })) el.value = keep;
    }
    /* UltraCombo.Text = x on a LimitToList combo bound to a fixed table: pick the row whose text is x (keep x when missing). */
    function setText(id, text) {
        var el = $(id), t = text === null || text === undefined ? '' : String(text);
        if (t !== '' && !Array.from(el.options).some(function (o) { return o.value === t; })) el.insertAdjacentHTML('beforeend', '<option value="' + esc(t) + '">' + esc(t) + '</option>');
        el.value = t;
    }
    async function api(url, opts) {
        var o = opts || {};
        var r = await fetch(url, { method: o.body ? 'POST' : 'GET', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/json' }, body: o.body ? JSON.stringify(o.body) : undefined });
        if (r.redirected || r.status === 401) throw Error('Please sign in to continue');
        var text = await r.text(), data = null;
        try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
        if (!r.ok) throw Error((data && (data.message || data.error)) || ('Request failed (' + r.status + ')'));
        return data;
    }
    function run(button, work) { return PurchaseRequest.run(button, async function () { try { await work(); } catch (e) { msg(e.message); } }); }

    // ------------------------------------------------------------------ prints (/api/print/by-template)
    function printRpt(rpt, args) {
        var w = window.open('', '_blank'); try { if (w) w.document.write('<p style="font:13px Segoe UI">Preparing report...</p>'); } catch (e) { }
        fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                return r.text().then(function (t) { if (w) w.close(); msg(t || 'Not Record Found For Display'); });
            }).catch(function (e) { if (w) w.close(); msg(e.message); });
    }
    /* CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 172) */
    function print103(voucherHeadId) { printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: voucherHeadId, documentTypeId: TYPE }); }
    /* GenerateReportVoucher104 :1829 - VoucherSlipForInventoryReport(DocumentTypeId 172, Id = VoucherHeadId) */
    function print104(voucherHeadId) { printRpt('104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt', { id: voucherHeadId, documentTypeId: TYPE }); }
    /* PurchaseInvoicettSlip_220A(Id) / GeneratereportSlip :3160 (@Id / @PihId) */
    function printSlip(rpt, id) { printRpt(rpt, { id: id }); }

    // ------------------------------------------------------------------ calculations (desktop events)
    function supplierId() { return num($('cmbSupplier').value); }
    function commAgentId() { return num($('cmbCommAgent').value); }

    /* TotalCommissionAmount :1671 / TotalBrokeryAmount :1731 */
    function chargeText(rateBox, typeBox, uomBox, amountBox) {
        var rateText = $(rateBox).value, type = $(typeBox).value, rate = num(rateText);
        if (rateText === '') return '0';
        if (type === 'Flat') return fmt(rate, '0,0');
        if (type === 'Percent') return fmt(bankers(sum(S.rows, 'ItemAmount') * rate / 100), '0,0');
        if (type === 'Comm Weight') { var uom = num($(uomBox).value); return uom === 0 ? '0' : fmt(bankers(sum(S.rows, 'NetBillWeight') / uom * rate), '0,0'); }
        return $(amountBox).value;
    }
    function totalCommission() { $('txtCommAmount').value = chargeText('txtCommRate', 'cmbCommType', 'cmbCommUom', 'txtCommAmount'); commissionProportion(); }
    function totalBrokery() { $('txtBrokeryAmount').value = chargeText('txtBrokeryRate', 'cmbBrokeryType', 'cmbBrokeryUom', 'txtBrokeryAmount'); billAmount(); }
    /* CommissionProportion :2247 */
    function commissionProportion() {
        var total = num($('txtCommAmount').value), pct = num($('txtCommRate').value), net = sum(S.rows, 'NetBillWeight');
        S.rows.forEach(function (r) {
            if (total > 0) r.Commission = $('cmbCommType').value === 'Percent' ? num(r.ItemAmount) * pct / 100 : total / net * num(r.NetBillWeight);
            else r.Commission = 0;
        });
        billProportion();
    }
    /* ExpProportion :2134 - by ItemQty */
    function expProportion() {
        var total = sum(S.expenses, 'Amount'), qty = sum(S.rows, 'ItemQty');
        S.rows.forEach(function (r) { r.Expense = total > 0 ? total / qty * num(r.ItemQty) : 0; });
        billProportion();
    }
    /* FreightProportion :2167 - credit minus debit of the Charge to Product grid, by NetBillWeight */
    function freightProportion() {
        var net = sum(S.rows, 'NetBillWeight'), credit = sum(S.freight, 'Freight'), debit = sum(S.freight, 'Debit');
        S.rows.forEach(function (r) { r.Freights = debit < credit ? bankers(credit - debit) / net * num(r.NetBillWeight) : 0; });
        billProportion();
    }
    /* BillProportion :2229 (LedgerProportion :2225 is empty, so Journal keeps its loaded value) */
    function billProportion() {
        S.rows.forEach(function (r) { r.BillAmount = num(r.ItemAmount) + num(r.Expense) + num(r.Freights) + num(r.Journal) + num(r.Commission); });
        renderGrid();
    }
    /* BillAmount :1366 */
    function billAmount() {
        var sup = supplierId(), bill = sum(S.rows, 'ItemAmount') + sum(S.expenses, 'Amount');
        var jd = 0, jc = 0, td = 0, tc = 0, eb = 0;
        S.ledger.forEach(function (r) { if (sup > 0 && toInt(r.AccountId) > 0) { jd += num(r.Debit); jc += num(r.Credit); } });
        S.freight.forEach(function (r) { if (toInt(r.Transporter) > 0 && String(S.supplierGl) === String(r.Transporter)) { td += num(r.Debit); tc += num(r.Freight); } });
        S.bags.forEach(function (b) { if (num(b.Amount) > 0 && toInt(b.TypeId) === 1) eb += num(b.Amount); });
        bill = bill + eb + jd - jc + tc - td;
        if (sup === commAgentId()) {
            if ($('txtCommRate').value !== '') bill += num($('txtCommAmount').value);
            else $('txtCommAmount').value = '0';
        }
        if ($('cmbBroker').value) {
            if (num($('txtBrokeryAmount').value) > 0) bill -= num($('txtBrokeryAmount').value);
            else $('txtBrokeryAmount').value = '0';
        } else $('txtBrokeryAmount').value = '0';
        $('txtBillAmount').value = fmt(bankers(bill), '0,0');
        billProportion();
    }
    function commissionEvent() { totalCommission(); billAmount(); commissionProportion(); } /* :1798, :3317, :3324, :3331 */

    /* grd_CellUpdated :1036 */
    function cellUpdated(r, key) {
        if (key === 'OrderItemRate' || key === 'RateCut' || key === 'EquivalentPoRate' || key === 'AdLsWeight') {
            var adLs = num(r.AdLsWeight), net;
            if (S.invoiceTypeId === 2) net = num(r.GrossWeight) - Math.abs(adLs);
            else {
                if (adLs > 0 && adLs > num(r.EbTotal)) { r.AdLsWeight = 0; renderGrid(); throw Error('AddWeight cannot be greater than EmptyBags weight'); }
                net = num(r.GrossWeight) - num(r.EbTotal) - num(r.WtCutTotal) + adLs;
            }
            r.NetBillWeight = net;
            var eq = num(r.EquivalentPoRate);
            r.RateCutAmount = net / eq * num(r.RateCut);
            r.ItemAmount = net / eq * num(r.OrderItemRate) - r.RateCutAmount;
        }
        billAmount();
    }
    /* grdFreight_CellUpdated :908 */
    function freightUpdated(r, key) {
        if ((key === 'Qty' || key === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') { r.Freight = num(r.Qty) * num(r.Rate); r.Percentage = 0; }
        if (key === 'Percentage' && String(r.Percentage) !== '') {
            if (Math.abs(toInt(r.Percentage)) > 100) { r.Percentage = 0; renderFreight(); throw Error('Percentage mustbe less than 100'); }
            var amount = sum(S.rows, 'ItemAmount') / 100 * num(r.Percentage);
            if (amount > 0) r.Freight = bankers(amount);
            r.Qty = 0; r.Rate = 0;
        }
        renderFreight(); billAmount(); freightProportion();
    }
    /* grdGLedger_CellUpdated :975 */
    function ledgerUpdated(r, key) {
        if ((key === 'Qty' || key === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') { r.Credit = num(r.Qty) * num(r.Rate); r.Debit = 0; r.Percentage = 0; }
        if (key === 'Percentage' && String(r.Percentage) !== '') {
            if (Math.abs(toInt(r.Percentage)) > 100) { r.Percentage = 0; renderLedger(); throw Error('Percentage mustbe less than 100'); }
            var amount = sum(S.rows, 'ItemAmount') / 100 * num(r.Percentage);
            if (amount > 0) { r.Credit = bankers(amount); r.Debit = 0; } else { r.Debit = Math.abs(bankers(amount)); r.Credit = 0; }
            r.Qty = 0; r.Rate = 0;
        }
        var warn = null;
        if (key === 'Credit' && num(r.Debit) > 0) { r.Credit = 0; warn = 'Debit Side is aleady added'; }
        if (key === 'Debit' && num(r.Credit) > 0) { r.Debit = 0; warn = 'Credit Side is aleady added'; }
        renderLedger(); billAmount();
        if (warn) msg(warn);
    }
    /* grdInvExp_CellUpdated :951 */
    function expenseUpdated(r, key) {
        if ((key === 'Qty' || key === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = num(r.Qty) * num(r.Rate);
        renderExpenses(); billAmount(); expProportion();
    }
    /* grdEmptyBags_CellUpdated :1343 */
    function bagUpdated(r, key) {
        if (key === 'Rate' && String(r.PurchaseQty) !== '' && String(r.Rate) !== '') r.Amount = num(r.PurchaseQty) * num(r.Rate);
        renderBags(); billAmount();
    }
    /* DuedaysCalculates :3286 */
    function dueDate() { var d = $('txtDueDays').value.trim(); setVal('dtDueDate', d !== '' ? addDays($('dtDocDate').value || today(), num(d)) : today()); }

    // ------------------------------------------------------------------ grids
    /* dtGrid :373-407 with grdSettings :594-745 */
    function gridColumns() {
        var first = S.rows[0], cols = [
            ['ItemName', 'ItemName', 's'], ['CropYear', 'CropYear', 's'], ['UOMCodeItem', 'PackUOM', 's'], ['ItemQty', 'ItemQty', 'n', '#,##0.##', 1],
            ['WareHouseName', 'WareHouseName', 's'], ['LabReportRef', 'LabNo', 's'], ['PurchaseOrder', 'PoNo', 'po'], ['GrossWeight', 'GrossWeight', 'n', '#,##0.##', 1],
            ['EbTotal', 'EbTotal', 'n', '#,##0.##', 1], ['WtCutTotal', 'WtCutTotal', 'n', '#,##0.##', 1], ['AdLsWeight', 'AdLsWeight', 'n', '#,##0.##', 1],
            ['NetBillWeight', 'NetBillWeight', 'n', '#,##0.##', 1], ['StockWeight', 'StockWeight', 'n', '#,##0.##', 1], ['OrderItemRate', 'ItemRate', 'n', '0.00;(0.00);0'],
            ['EquivalentPoRate', 'RateUOM', 'n', '0;(0);0', 1], ['ItemAmount', 'ItemAmount', 'n', '#,##0.##', 1], ['RateCut', 'RateCut', 'n', '0.00;(0.00);0', 1],
            ['RateCutAmount', 'RateCutAmount', 'n', '0,0', 1], ['BillAmount', 'Item Net Amount', 'n', '#,##0.##', 1], ['Freights', 'Freights', 'n', '0,0', 1],
            ['Expense', 'Expense', 'n', '0,0', 1], ['Commission', 'Commission', 'n', '0,0', 1], ['GpNo', 'GpNo', 's'], ['VehicleNo', 'VehicleNo', 's']];
        return cols.filter(function (c) {
            var k = c[0];
            if (first) {
                if (k === 'PurchaseOrder' && toInt(first.PurchaseOrder) === 0 && S.invoiceTypeId !== 1) return false;
                if (k === 'EbTotal' && num(first.EbTotal) === 0) return false;
                if (k === 'WtCutTotal' && num(first.WtCutTotal) === 0) return false;
                if (k === 'LabReportRef' && String(first.LabReportRef || '') === '') return false;
            }
            if (k === 'AdLsWeight' && !S.cfg.weightAddLessOnPurchaseInvoice) return false;
            return true;
        });
    }
    function editable(key) {
        if (key === 'RateCut') return true;
        if (key === 'AdLsWeight') return !!S.cfg.weightAddLessOnPurchaseInvoice;
        if (key === 'OrderItemRate' || key === 'EquivalentPoRate') return !(S.invoiceTypeId === 1 || (S.invoiceTypeId === 3 && !S.cfg.rateEditableInGatePurchase));
        return false;
    }
    function renderGrid() {
        var cols = gridColumns();
        $('grdHead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        $('grdBody').innerHTML = S.rows.map(function (r, idx) {
            return '<tr>' + cols.map(function (c) {
                var key = c[0];
                if (editable(key)) return '<td class="num"><input class="gd-edit" data-row="' + idx + '" data-key="' + key + '" value="' + esc(away(num(r[key]), 4)) + '" aria-label="' + esc(c[1]) + '"></td>';
                if (c[2] === 'po') return '<td>' + (toInt(r.PurchaseOrderId) > 0 ? '<span class="pio-link" data-order="' + esc(r.PurchaseOrderId) + '">' + esc(r[key]) + '</span>' : esc(r[key])) + '</td>';
                if (c[2] === 's') return '<td>' + esc(r[key]) + '</td>';
                return '<td class="num">' + fmt(r[key], c[3]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $('grdFoot').innerHTML = S.rows.length ? '<tr>' + cols.map(function (c) { return c[4] ? '<td class="num">' + fmt(sum(S.rows, c[0]), '#,##0.##') + '</td>' : '<td></td>'; }).join('') + '</tr>' : '';
        $('lblRecords').textContent = S.rows.length + ' record(s)';
    }
    function accountSelect(cls, idx, key, value, caption) {
        var accounts = S.lookups.accounts || [], v = String(toInt(value));
        var known = accounts.some(function (a) { return String(a.Id) === v; });
        return '<select class="' + cls + '" data-row="' + idx + '" data-key="' + key + '" data-dtcombo="single" data-dtcombo-caption="' + esc(caption) + '" aria-label="' + esc(caption) + '"><option value="0"></option>'
            + (!known && v !== '0' ? '<option value="' + esc(v) + '" selected>' + esc(v) + '</option>' : '')
            + accounts.map(function (a) { return '<option value="' + esc(a.Id) + '"' + (String(a.Id) === v ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>'; }).join('') + '</select>';
    }
    function cell(cls, idx, key, value, text) { return '<td' + (text ? '' : ' class="num"') + '><input class="' + cls + (text ? ' txt' : '') + '" data-row="' + idx + '" data-key="' + key + '" value="' + esc(value) + '" aria-label="' + esc(key) + '"></td>'; }
    function buttons(cls, idx) { return '<td><button type="button" class="' + cls + '" data-act="Delete" data-row="' + idx + '" title="Delete">X</button></td><td><button type="button" class="' + cls + '" data-act="Add" data-row="' + idx + '" title="Add">+</button></td>'; }
    /* grdFreightSettings :510 - Charge To Product, %, Qty, Rate, Credit, Remarks, X, + */
    function renderFreight() {
        $('frHead').innerHTML = '<tr><th>Charge To Product</th><th>%</th><th>Qty</th><th>Rate</th><th>Credit</th><th>Remarks</th><th>X</th><th>+</th></tr>';
        $('frBody').innerHTML = S.freight.map(function (r, i) {
            return '<tr><td>' + accountSelect('fr-edit', i, 'Transporter', r.Transporter, 'Charge To Product') + '</td>' + cell('fr-edit', i, 'Percentage', r.Percentage) + cell('fr-edit', i, 'Qty', r.Qty) + cell('fr-edit', i, 'Rate', r.Rate)
                + '<td class="num"><input class="fr-edit" data-row="' + i + '" data-key="Freight" value="' + esc(fmt(r.Freight, '#,#')) + '" aria-label="Credit"></td>' + cell('fr-edit', i, 'Remarks', r.Remarks, true) + buttons('fr-btn', i) + '</tr>';
        }).join('');
        $('frFoot').innerHTML = '<tr><td></td><td></td><td></td><td></td><td class="num">' + fmt(sum(S.freight, 'Freight'), '#,#') + '</td><td></td><td></td><td></td></tr>';
    }
    /* gridGLSettings :557 - Supplier Add/Less, Remarks, Percentage, Qty, Rate, Debit, Credit, X, + */
    function renderLedger() {
        $('glHead').innerHTML = '<tr><th>Supplier Add/Less</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th><th>X</th><th>+</th></tr>';
        $('glBody').innerHTML = S.ledger.map(function (r, i) {
            return '<tr><td>' + accountSelect('gl-edit', i, 'AccountId', r.AccountId, 'Supplier Add/Less') + '</td>' + cell('gl-edit', i, 'Remarks', r.Remarks, true) + cell('gl-edit', i, 'Percentage', r.Percentage)
                + cell('gl-edit', i, 'Qty', r.Qty) + cell('gl-edit', i, 'Rate', r.Rate) + cell('gl-edit', i, 'Debit', r.Debit) + cell('gl-edit', i, 'Credit', r.Credit) + buttons('gl-btn', i) + '</tr>';
        }).join('');
        $('glFoot').innerHTML = '<tr><td></td><td></td><td></td><td></td><td></td><td class="num">' + fmt(sum(S.ledger, 'Debit')) + '</td><td class="num">' + fmt(sum(S.ledger, 'Credit')) + '</td><td></td><td></td></tr>';
    }
    /* grdInvExpSettings :477 - Other Item, Qty, Rate, Amount, Remarks, X, + */
    function renderExpenses() {
        var items = S.lookups.otherItems || [];
        $('expHead').innerHTML = '<tr><th>Other Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th><th>X</th><th>+</th></tr>';
        $('expBody').innerHTML = S.expenses.map(function (r, i) {
            var v = String(toInt(r.ItemId));
            var sel = '<select class="exp-edit" data-row="' + i + '" data-key="ItemId" data-dtcombo="single" data-dtcombo-caption="Other Item" aria-label="Other Item"><option value="0"></option>'
                + items.map(function (o) { return '<option value="' + esc(o.Id) + '"' + (String(o.Id) === v ? ' selected' : '') + '>' + esc(o.OtherItemName) + '</option>'; }).join('') + '</select>';
            return '<tr><td>' + sel + '</td>' + cell('exp-edit', i, 'Qty', r.Qty) + cell('exp-edit', i, 'Rate', r.Rate) + cell('exp-edit', i, 'Amount', r.Amount) + cell('exp-edit', i, 'Remarks', r.Remarks, true) + buttons('exp-btn', i) + '</tr>';
        }).join('');
        $('expFoot').innerHTML = '<tr><td></td><td></td><td></td><td class="num">' + fmt(sum(S.expenses, 'Amount')) + '</td><td></td><td></td><td></td></tr>';
    }
    /* grdEmptyBagsSettings :772 - Type, ItemName, PurchaseQty, Rate (read-only for invoice type 1), Amount, Remarks */
    function renderBags() {
        var rateEdit = S.invoiceTypeId !== 1;
        $('bagHead').innerHTML = '<tr><th>Type</th><th>ItemName</th><th>PurchaseQty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        $('bagBody').innerHTML = S.bags.map(function (b, i) {
            return '<tr><td>' + esc(b.Type) + '</td><td>' + esc(b.ItemName) + '</td><td class="num">' + fmt(b.PurchaseQty) + '</td>'
                + (rateEdit ? cell('bag-edit', i, 'Rate', b.Rate) : '<td class="num">' + esc(b.Rate) + '</td>')
                + '<td class="num">' + fmt(b.Amount) + '</td>' + cell('bag-edit', i, 'Remarks', b.Remarks, true) + '</tr>';
        }).join('');
        $('bagFoot').innerHTML = '<tr><td></td><td></td><td class="num">' + fmt(sum(S.bags, 'PurchaseQty')) + '</td><td></td><td class="num">' + fmt(sum(S.bags, 'Amount')) + '</td><td></td></tr>';
    }
    function renderAll() { renderGrid(); renderFreight(); renderLedger(); renderExpenses(); renderBags(); }
    /* AddRowInFreightGrid :766 / AddRowInGLGrid :754 / AddRowInvExpGrid :760 */
    function blankFreight() { return { OrderId: 0, InvGrnId: 0, FreightAcId: 0, Transporter: 0, Percentage: 0, Qty: 0, Rate: 0, Freight: 0, Debit: 0, FreightAmount: 0, Remarks: '' }; }
    function blankLedger() { return { InvGrnId: 0, FreightAcId: 0, AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0, FreightAmount: 0 }; }
    function blankExpense() { return { ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }

    // ------------------------------------------------------------------ load / reset
    function applyLookups(d) {
        S.lookups = Object.assign(S.lookups, d);
        if (d.suppliers) {
            S.suppliers = {}; d.suppliers.forEach(function (s) { S.suppliers[num(s.Id)] = s; });
            ['cmbSupplier', 'cmbCommAgent', 'cmbBroker'].forEach(function (id) { option(id, d.suppliers, 'Id', 'CompanyName', ''); });
        }
        /* BranchFill :1482 / ProjectFill :1499 - Rows[0].Activate() */
        if (d.branches) { option('cmbBranch', d.branches, 'Id', 'BranchName'); if (S.cfg.currentBranchId) $('cmbBranch').value = String(S.cfg.currentBranchId); }
        if (d.projects) { option('cmbProject', d.projects, 'Id', 'ProjectName'); if (d.projects.length) $('cmbProject').value = String(d.projects[0].Id); }
    }
    function fixedLists() {
        /* CommissionTypeFill :1622 and CommissionUOMFill :1596 (hard-coded on the desktop too) */
        var types = ['Flat', 'Percent', 'Comm Weight'], uoms = ['1', '5', '10', '25', '40', '50', '60', '65', '80', '100'];
        ['cmbCommType', 'cmbBrokeryType'].forEach(function (id) { $(id).innerHTML = '<option value=""></option>' + types.map(function (t) { return '<option value="' + t + '">' + t + '</option>'; }).join(''); });
        ['cmbCommUom', 'cmbBrokeryUom'].forEach(function (id) { $(id).innerHTML = '<option value=""></option>' + uoms.map(function (t) { return '<option value="' + t + '">' + t + '</option>'; }).join(''); });
    }
    function supplierChanged() { var s = S.suppliers[supplierId()]; S.supplierGl = s ? num(s.GlAccountId) : S.supplierGl; } /* cmbsuppliername_ValueChanged :3270 */

    async function init() {
        fixedLists();
        var d = await api(API + '/init');
        S.rights = d.rights || {}; S.cfg = d.configuration || {};
        applyLookups(d);
        setVal('txtDocNo', d.docNo); if (d.docNoMessage) msg(d.docNoMessage);
        setVal('dtDocDate', today()); setVal('dtDueDate', today());
        S.freight = [blankFreight()]; S.ledger = [blankLedger()]; S.expenses = [blankExpense()]; S.bags = [];
        /* Load :351-353, :465-469 */
        $('btnSave').disabled = !S.rights.Save; $('btnPrint').disabled = !S.rights.Print; $('btnUpdate').disabled = !S.rights.Update;
        $('ChkBok').checked = true; $('ChkBoxParty').checked = true;
        renderAll();
        $('dtDocDate').focus();
        var q = new URLSearchParams(location.search).get('id');
        if (num(q) > 0) await readById(num(q));
    }

    /* Reset :2375 */
    async function reset() {
        if (S.attachments) S.attachments.reset();
        S.id = 0; S.voucherHeadId = 0; S.approved = false; S.loadMode = '';
        ['cmbCommAgent', 'cmbCommType', 'txtCommRate', 'txtCommAmount', 'cmbCommUom', 'txtCommRemarks', 'cmbBroker', 'cmbBrokeryType', 'cmbBrokeryUom',
            'txtBrokeryRate', 'txtBrokeryAmount', 'txtBillNo', 'txtDeliveryTerm', 'txtRemarks', 'txtDueDays', 'txtGrnNo'].forEach(function (id) { $(id).value = ''; });
        setVal('dtDueDate', today());
        S.rows = []; S.freight = [blankFreight()]; S.ledger = [blankLedger()]; S.expenses = [blankExpense()]; S.bags = [];
        var d = await api(API + '/next-code'); setVal('txtDocNo', d.docNo);
        $('btnSave').hidden = false; $('btnUpdate').hidden = true;
        $('cmbSupplier').disabled = false; $('cmbSupplier').value = '';
        $('txtBillAmount').value = '0';
        renderAll();
        $('cmbSupplier').focus();
    }

    /* ReadById :2455 */
    async function readById(id) {
        var d = await api(API + '/' + id);
        S.id = id;
        $('cmbBranch').value = String(d.BranchesId); $('cmbProject').value = String(d.ProjectsId);
        setVal('txtDocNo', d.DocNo); setVal('dtDocDate', d.DocDate); showTab('form');
        $('cmbSupplier').value = String(d.SupplierCustomerId); $('cmbSupplier').disabled = true; supplierChanged();
        setVal('txtBillNo', d.ManualBillNo);
        $('cmbCommAgent').value = num(d.CommissionAgentId) ? String(d.CommissionAgentId) : '';
        setText('cmbCommType', d.CommissionType); setVal('txtCommRate', d.CommRate); setText('cmbCommUom', d.UomScheduleIdCmRate);
        setVal('txtCommAmount', d.CommAmount); setVal('txtCommRemarks', d.CommissionRemarks);
        $('cmbBroker').value = num(d.BrokerAgentId) ? String(d.BrokerAgentId) : '';
        setText('cmbBrokeryType', d.BrokeryType); setVal('txtBrokeryRate', d.BrokeryRate); setVal('txtBrokeryAmount', d.BrokeryAmount); setText('cmbBrokeryUom', d.BrokeryUom);
        setVal('txtRemarks', d.RemarksHeader); setVal('txtDeliveryTerm', d.DeliveryTerm); setVal('txtBillAmount', d.BillAmount);
        setVal('txtDueDays', d.DueDays); dueDate();
        setVal('txtGrnNo', d.DocumentTypeSrNo);
        S.approved = !!d.IsApproved; S.invoiceTypeId = num(d.InvoiceTypeId); S.voucherHeadId = num(d.VoucherHeadId); S.loadMode = '';
        S.rows = d.details || [];
        S.expenses = (d.expenses || []).length ? d.expenses : [blankExpense()];
        S.ledger = (d.ledger || []).length ? d.ledger : [blankLedger()];
        S.freight = (d.freight || []).length ? d.freight : [blankFreight()];
        S.bags = d.emptyBags || [];
        renderAll();
        freightProportion(); expProportion(); commissionProportion(); totalBrokery(); billProportion();
        if (S.attachments) S.attachments.reset();
        $('btnSave').hidden = true; $('btnUpdate').hidden = false;
    }

    // ------------------------------------------------------------------ Load GRN (frmLoadGRN, DocumentTypeId 169) / Grn No
    var LDR = [['BranchName', 'BranchName'], ['DocDate', 'DocDate'], ['DocNo', 'DocNo'], ['SupplierCustomer', 'SupplierCustomer'], ['DeliveryTerm', 'DeliveryTerm'],
        ['GpDate', 'GpDate'], ['BiltyNo', 'BiltyNo'], ['VehicleNo', 'VehicleNo'], ['GrnStatus', 'GrnStatus']]; /* 169 hides PurchaseOrder, PurchaseAgainst, GpNO (frmLoadGRN :335) */
    var loaderLoaded = false;
    async function openLoader() {
        loaderLoaded = false;
        $('grnLoader').hidden = false;
        /* frmLoadGRN From Date = active financial year's Start_Period; the page does not know it, so the filter starts open. */
        if (!$('ldrTo').value) setVal('ldrTo', today());
        var branches = await api('/api/grn-loader/branches?docTypeId=' + GRN_TYPE);
        $('ldrBranches').innerHTML = branches.map(function (b, i) { return '<label><input type="checkbox" value="' + esc(b.BranchId) + '"' + (i === 0 ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join('');
        await showGrns();
    }
    async function showGrns() {
        var ids = Array.from($('ldrBranches').querySelectorAll('input:checked')).map(function (i) { return i.value; }).join(',');
        if (!ids) throw Error('Select branch first');
        var q = new URLSearchParams({ docTypeId: GRN_TYPE, fromDate: $('ldrFrom').value, toDate: $('ldrTo').value, branchIds: ids });
        S.loaderRows = await api('/api/grn-loader/pending?' + q.toString());
        $('ldrHead').innerHTML = '<tr><th>Select</th>' + LDR.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        $('ldrBody').innerHTML = S.loaderRows.map(function (r, i) {
            return '<tr data-row="' + i + '"><td><input type="checkbox" class="ldr-check" data-row="' + i + '" aria-label="Select GRN"></td>' + LDR.map(function (c) { var v = r[c[0]]; if (/Date$/.test(c[0]) && v) v = String(v).substring(0, 10); return '<td>' + esc(v) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $('ldrDetailHead').innerHTML = ''; $('ldrDetailBody').innerHTML = '';
    }
    async function loaderDetail(i) {
        var r = S.loaderRows[i]; if (!r) return;
        Array.from($('ldrBody').rows).forEach(function (tr, k) { tr.classList.toggle('sel', k === i); });
        var rows = await api('/api/grn-loader/details/' + r.Id);
        var cols = rows.length ? Object.keys(rows[0]).filter(function (k) { return !/Id$/.test(k); }) : [];
        $('ldrDetailHead').innerHTML = '<tr>' + cols.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr>';
        $('ldrDetailBody').innerHTML = rows.map(function (x) { return '<tr>' + cols.map(function (k) { return '<td>' + esc(x[k]) + '</td>'; }).join('') + '</tr>'; }).join('');
    }
    /* LoadInGridDetail :2066 - the grids are replaced by what the server read from the GRNs */
    function applyLoad(d, mode) {
        S.invoiceTypeId = num(d.invoiceTypeId); S.loadMode = mode;
        var h = d.header;
        if (h) { /* LoadDataDetailGridAgainstGP :1904-1922 */
            $('cmbSupplier').value = String(h.SupplierCustomerId); $('cmbSupplier').disabled = true; supplierChanged();
            $('cmbCommAgent').value = num(h.CommissionAgentId) ? String(h.CommissionAgentId) : '';
            setText('cmbCommType', h.CommissionType); setVal('txtCommRate', h.CommRate); setText('cmbCommUom', h.UomScheduleIdCmRate);
            setVal('txtCommAmount', h.CommAmount); setVal('txtCommRemarks', h.CommissionRemarks); setVal('txtRemarks', h.RemarksHeader);
            $('cmbBroker').value = num(h.BrokerAgentId) ? String(h.BrokerAgentId) : '';
            setText('cmbBrokeryType', h.BrokeryType); setText('cmbBrokeryUom', h.BrokeryUom); setVal('txtBrokeryRate', h.BrokeryRate); setVal('txtBrokeryAmount', h.BrokeryAmount);
            setVal('txtDeliveryTerm', h.DeliveryTerm); setVal('txtDueDays', h.DueDays); dueDate();
        }
        S.rows = d.rows || [];
        S.freight = (d.freight || []).length ? d.freight : [blankFreight()];
        S.ledger = (d.ledger || []).length ? d.ledger : [blankLedger()];
        S.expenses = (d.expenses || []).length ? d.expenses : [blankExpense()];
        S.bags = d.emptyBags || [];
        renderAll();
        totalCommission(); totalBrokery(); freightProportion(); expProportion(); commissionProportion(); billProportion(); billAmount();
        if (d.chargeMessage) msg(d.chargeMessage);
    }
    /* frmLoadGRN btnLoadOnInvoice_Click_1 :496 (the server repeats every check with the desktop wording) */
    async function loadOnInvoice() {
        var picked = Array.from(document.querySelectorAll('.ldr-check:checked')).map(function (c) { return S.loaderRows[num(c.dataset.row)].Id; });
        if (!picked.length) throw Error('Check the row first');
        var d = await api(API + '/load-grns', { body: { grnIds: picked, deliveryTerm: $('txtDeliveryTerm').value } });
        loaderLoaded = true;
        $('grnLoader').hidden = true;
        applyLoad(d, 'loader');
    }
    /* Closing frmLoadGRN without loading: LoadInGridDetail still runs with an empty GRN list (:3117), which empties the
       detail grid, the Charge to Product / Supplier Add/Less / Other Expense grids (one blank row each) and the Empty Bags. */
    function closeLoader() {
        $('grnLoader').hidden = true;
        if (loaderLoaded) return;
        applyLoad({ invoiceTypeId: 1, rows: [], freight: [], ledger: [], expenses: [], emptyBags: [] }, '');
    }
    /* txtGrnNo_Leave :3384 */
    async function grnNoLeave() {
        var n = toInt($('txtGrnNo').value.trim());
        if (n <= 0) return;
        var d = await api(API + '/load-grns', { body: { grnNo: n, deliveryTerm: $('txtDeliveryTerm').value } });
        applyLoad(d, 'grnNo');
    }

    // ------------------------------------------------------------------ save / update
    /* FormValidation :1453 (browser side; the server repeats every check) */
    function formValidation() {
        var checks = [[!$('cmbBranch').value, 'Branch Name is Required', 'cmbBranch'], [!$('cmbProject').value, 'Project Name is Required', 'cmbProject'],
            [supplierId() <= 0, 'Supplier Name is Required', 'cmbSupplier'], [['', '0'].indexOf($('txtDocNo').value.trim()) >= 0, 'DocNo is Required', 'txtDocNo']];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { msg(checks[i][1]); try { $(checks[i][2]).focus(); } catch (e) { } return false; }
        return true;
    }
    async function insert() {
        if (!formValidation()) return;
        if (!window.confirm(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        totalCommission(); billAmount(); expProportion(); freightProportion(); commissionProportion(); /* :2618-2622 */
        var body = {
            id: S.id, branchId: num($('cmbBranch').value), projectId: num($('cmbProject').value), docDate: $('dtDocDate').value, supplierId: supplierId(),
            manualBillNo: $('txtBillNo').value, remarks: $('txtRemarks').value, dueDays: $('txtDueDays').value, grnNoText: $('txtGrnNo').value,
            commissionAgentId: commAgentId(), commType: $('cmbCommType').value, commRate: $('txtCommRate').value, commUom: $('cmbCommUom').value,
            commAmount: $('txtCommAmount').value, commRemarks: $('txtCommRemarks').value,
            brokerAgentId: num($('cmbBroker').value), brokeryType: $('cmbBrokeryType').value, brokeryRate: $('txtBrokeryRate').value, brokeryUom: $('cmbBrokeryUom').value,
            brokeryAmount: $('txtBrokeryAmount').value, loadMode: S.loadMode,
            details: S.rows.map(function (r) { return { Id: r.Id, InvGrnDetailId: r.InvGrnDetailId, InvGrnId: r.InvGrnId, OrderItemRate: r.OrderItemRate, EquivalentPoRate: r.EquivalentPoRate, RateCut: r.RateCut, AdLsWeight: r.AdLsWeight }; }),
            freight: S.freight, ledger: S.ledger, expenses: S.expenses,
            emptyBags: S.bags.map(function (b) { return { InvGrnId: b.InvGrnId, ItemId: b.ItemId, TypeId: b.TypeId, Rate: b.Rate, Remarks: b.Remarks }; })
        };
        if (S.attachments) { await S.attachments.settled(); var a = S.attachments.payload(); if (a) body.attachments = a; }
        var d = await api(API + '/save', { body: body });
        msg(d.message);
        await reset();
        if ($('ChkBoxParty').checked) printSlip('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', d.id);
        if ($('ChkBoxItemPrint').checked) printSlip('220-InvRptPurchaseBillSupplierRiceSlip.rpt', d.id);
        if ($('ChkBok').checked) print103(num(d.voucherHeadId));
    }
    /* btnUpdate_Click :2563 */
    async function update() {
        if (S.approved) throw Error('Record Not Update beacause Record has approved');
        if (S.id === 0) throw Error('Record Not Update beacause Record Id not found');
        await insert();
    }

    // ------------------------------------------------------------------ History tab
    var HIDDEN = { Id: 1, VoucherHeadId: 1, RecordNo: 1, DocumentTypeId: 1 }; /* HistoryGridSettings :2363-2366 */
    var HBTN = [['Detail', 'Detail', 'Detail'], ['View', 'Slip', 'View'], ['PartySlip', 'PartySlip', 'PartySlip'], ['Voucher', 'Voucher', 'Voucher'], ['SummaryReport', 'SummaryReport', 'SummaryReport'], ['Edit', 'Edit', '✎']];
    function histText(key, v) {
        if (v === null || v === undefined) return '';
        if (key === 'BillAmount' || key === 'CommAmount') return fmt(v, '#,##0');
        if (/Date$/.test(key) && typeof v === 'string' && /^\d{4}-\d{2}-\d{2}/.test(v)) return /DocDate|DueDate/.test(key) ? v.substring(0, 10) : v.substring(0, 16);
        return String(v);
    }
    /* GetAll :2292 */
    async function getAll(noOfRecords) {
        S.history = await api(API + '/history?noOfRecords=' + noOfRecords);
        S.historySel = -1;
        renderHistory();
        $('detHead').innerHTML = ''; $('detBody').innerHTML = ''; $('detFoot').innerHTML = '';
    }
    function renderHistory() {
        if (!S.history.length) { $('histHead').innerHTML = ''; $('histBody').innerHTML = ''; $('histFoot').innerHTML = ''; return; } /* ClearStructure */
        var keys = Object.keys(S.history[0]).filter(function (k) { return !HIDDEN[k]; });
        $('histHead').innerHTML = '<tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + HBTN.map(function (b) { return '<th>' + b[1] + '</th>'; }).join('') + '</tr>';
        $('histBody').innerHTML = S.history.map(function (r, i) {
            return '<tr data-row="' + i + '" tabindex="0"' + (i === S.historySel ? ' class="sel"' : '') + '>' + keys.map(function (k) {
                if (k === 'NoOfAttachments') return '<td class="num"><span class="pio-link" data-act="NoOfAttachments" data-row="' + i + '">' + esc(r[k]) + '</span></td>';
                return '<td' + (k === 'BillAmount' || k === 'CommAmount' ? ' class="num"' : '') + '>' + esc(histText(k, r[k])) + '</td>';
            }).join('') + HBTN.map(function (b) { return '<td><button type="button" data-act="' + b[0] + '" data-row="' + i + '">' + b[2] + '</button></td>'; }).join('') + '</tr>';
        }).join('');
        $('histFoot').innerHTML = '<tr>' + keys.map(function (k) { return k === 'BillAmount' || k === 'CommAmount' ? '<td class="num">' + fmt(sum(S.history, k)) + '</td>' : '<td></td>'; }).join('') + HBTN.map(function () { return '<td></td>'; }).join('') + '</tr>';
    }
    /* GetDetailGrdByHeadId :1226 + DetailGridSetting :1279 */
    var DET = [['PurchaseOrder', 's'], ['WareHouseName', 's'], ['ItemName', 's'], ['PackType', 's'], ['ItemUOM', 's'], ['CropYear', 's'], ['JobLot', 's'], ['GpNo', 's'], ['VehicleNo', 's'],
        ['ItemQty', '0,0', 1], ['GrossWeight', '0,0', 1], ['EBTotalWt', '0,0', 1], ['WeightCutTotal', '0,0', 1], ['NetBillWeight', '0,0', 1], ['NetStockWeight', '0,0', 1], ['ItemRate', '0,0'],
        ['RateUOM', 's'], ['ItemAmount', '0,0', 1], ['RateCut', '0,0', 1], ['RateCutAmount', '0,0', 1], ['CommissionAmount', '0,0', 1], ['ExpenseAmount', '0,0', 1], ['FreightAmount', '0,0', 1]];
    async function historyDetail(i) {
        var r = S.history[i]; if (!r) return;
        var rows = await api(API + '/' + r.Id + '/detail');
        if (!rows.length) { $('detHead').innerHTML = ''; $('detBody').innerHTML = ''; $('detFoot').innerHTML = ''; return; }
        $('detHead').innerHTML = '<tr>' + DET.map(function (c) { return '<th>' + c[0] + '</th>'; }).join('') + '</tr>';
        $('detBody').innerHTML = rows.map(function (x) { return '<tr>' + DET.map(function (c) { return c[1] === 's' ? '<td>' + esc(x[c[0]]) + '</td>' : '<td class="num">' + fmt(x[c[0]], c[1]) + '</td>'; }).join('') + '</tr>'; }).join('');
        $('detFoot').innerHTML = '<tr>' + DET.map(function (c) { return c[2] ? '<td class="num">' + fmt(sum(rows, c[0])) + '</td>' : '<td></td>'; }).join('') + '</tr>';
    }
    /* grdHistory_ColumnButtonClick :1112 / grdHistory_LinkClicked :3236 */
    async function historyAction(act, i) {
        var r = S.history[i]; if (!r) return;
        S.historySel = i; Array.from($('histBody').rows).forEach(function (tr, k) { tr.classList.toggle('sel', k === i); });
        if (act === 'Edit') { await reset(); await readById(num(r.Id)); }
        else if (act === 'Voucher') print103(num(r.VoucherHeadId));
        else if (act === 'View') printSlip('220-InvRptPurchaseBillSupplierRiceSlip.rpt', num(r.Id));
        else if (act === 'PartySlip') printSlip('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', num(r.Id));
        else if (act === 'SummaryReport') printSlip('220B-InvRptPurchaseBillSupplierRiceSummary.rpt', num(r.Id));
        else if (act === 'Detail') await historyDetail(i);
        else if (act === 'NoOfAttachments') {
            /* CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 172) - the record's attachments */
            if (window.PurchaseInvoiceAttachments && PurchaseInvoiceAttachments.view) await PurchaseInvoiceAttachments.view({ type: TYPE, id: num(r.Id), message: msg });
            else { await readById(num(r.Id)); await attachmentsDialog(); }
        }
    }

    // ------------------------------------------------------------------ misc
    function formTab() { return !$('tabForm').hidden; }
    /* tabControl1_SelectedIndexChanged :3135 - the History page reads the last 50 records */
    function showTab(t) {
        var was = formTab() ? 'form' : 'history';
        $('tabForm').hidden = t !== 'form'; $('tabHistory').hidden = t !== 'history';
        $('tabBtnForm').setAttribute('aria-selected', t === 'form'); $('tabBtnHistory').setAttribute('aria-selected', t === 'history');
        if (t === 'history' && was !== 'history') run(null, function () { return getAll(50); });
    }
    async function attachmentsDialog(button) {
        if (!S.attachments) S.attachments = PurchaseInvoiceAttachments.create({ type: TYPE, getId: function () { return S.id; }, canEdit: function () { return S.id > 0 ? !!S.rights.Update : !!S.rights.Save; }, message: msg });
        await S.attachments.open(button);
    }
    function fullscreen(wrapId) {
        var w = $(wrapId), b = document.createElement('button');
        b.type = 'button'; b.className = 'pio-fs'; b.textContent = '⤢'; b.title = 'Full screen table';
        w.appendChild(b);
        b.addEventListener('click', function (e) { e.stopPropagation(); w.classList.toggle('pio-fs-on'); });
    }
    /* InvfrmPurchaseInvoice_KeyDown :2992 */
    document.addEventListener('keydown', function (e) {
        if (!$('grnLoader').hidden) { if (e.key === 'Escape') closeLoader(); return; }
        var k = e.key.toLowerCase(), ctrl = e.ctrlKey, t = e.target;
        if (e.key === 'Enter' && !ctrl && t && /^(INPUT|SELECT)$/.test(t.tagName) && !t.closest('.pio-wrap') && t.type !== 'checkbox') {
            e.preventDefault(); /* SendKeys "{TAB}" */
            var f = Array.from(document.querySelectorAll('#tabForm input:not([disabled]):not([readonly]):not([type=hidden]),#tabForm select:not([disabled]),#tabForm textarea,.dtcombo-input')).filter(function (x) { return x.offsetParent !== null; });
            var at = f.indexOf(t); if (at >= 0 && at + 1 < f.length) f[at + 1].focus();
        }
        if (ctrl && k === 's' && formTab()) { e.preventDefault(); if (!$('btnSave').hidden && !$('btnSave').disabled) $('btnSave').click(); }
        if (ctrl && k === 'n' && formTab()) { e.preventDefault(); $('btnNew').click(); }
        if (ctrl && k === 't') { e.preventDefault(); showTab(formTab() ? 'history' : 'form'); }
        if ((ctrl && k === 'e') || e.key === 'Escape') {
            e.preventDefault();
            var att = document.querySelector('.invoice-attachments:not([hidden])');
            if (att) { att.hidden = true; return; }
            var fs = document.querySelector('.pio-fs-on'); if (fs) { fs.classList.remove('pio-fs-on'); return; }
            window.location.href = '/purchase';
        }
        if (ctrl && e.key === 'F5') { e.preventDefault(); showTab('form'); $('dtDocDate').focus(); }
        if (ctrl && k === 'u') { e.preventDefault(); if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) $('btnUpdate').click(); }
        if (ctrl && k === 'p') { e.preventDefault(); if (!$('btnPrint').disabled) $('btnPrint').click(); }
        if (ctrl && e.key === 'F10') { e.preventDefault(); $('btnAttachment').click(); }
        if (ctrl && k === 'l') { e.preventDefault(); $('btnLoadGrn').click(); }
        if (ctrl && e.key === 'Enter' && !formTab()) {
            e.preventDefault();
            var sel = S.historySel >= 0 ? S.historySel : 0; if (S.history[sel]) run(null, function () { return readById(num(S.history[sel].Id)); });
        }
        if (ctrl && formTab() && !/^(INPUT|TEXTAREA)$/.test((t && t.tagName) || '')) {
            if (e.key === 'ArrowLeft') { e.preventDefault(); $('frWrap').focus(); }
            if (e.key === 'ArrowUp') { e.preventDefault(); $('expWrap').focus(); }
            if (e.key === 'ArrowDown') { e.preventDefault(); $('glWrap').focus(); }
            if (e.key === 'ArrowRight') { e.preventDefault(); $('bagWrap').focus(); }
        }
    });

    // ------------------------------------------------------------------ wiring
    function bind(id, ev, fn) { $(id).addEventListener(ev, fn); }
    function numeric(v) { return v === '' || !isNaN(parseFloat(v)); }
    document.addEventListener('DOMContentLoaded', function () {
        bind('btnFrmRefresh', 'click', function (e) { run(e.currentTarget, async function () { applyLookups(await api(API + '/refresh')); renderAll(); }); });
        bind('btnNew', 'click', function (e) { run(e.currentTarget, reset); });
        bind('btnSave', 'click', function (e) { run(e.currentTarget, function () { S.id = 0; return insert(); }); });
        bind('btnUpdate', 'click', function (e) { run(e.currentTarget, update); });
        bind('btnAttachment', 'click', function (e) { attachmentsDialog(e.currentTarget); });
        /* toolStripButton3_Click_1 :3107 - the loader only while Save is showing and allowed, otherwise Reset */
        bind('btnLoadGrn', 'click', function (e) { run(e.currentTarget, function () { return (!$('btnSave').hidden && !$('btnSave').disabled) ? openLoader() : reset(); }); });
        bind('btnPrint', 'click', function () { print103(S.voucherHeadId); });
        bind('btn104Voucher', 'click', function () { print104(S.voucherHeadId); });
        bind('btnSlipDetail', 'click', function () { printSlip('220-InvRptPurchaseBillSupplierRiceSlip.rpt', S.id); });
        bind('btnSlip220A', 'click', function () { printSlip('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', S.id); });
        bind('btn220bSummary', 'click', function () { printSlip('220B-InvRptPurchaseBillSupplierRiceSummary.rpt', S.id); });
        bind('btnLoadAll', 'click', function (e) { run(e.currentTarget, function () { return getAll(0); }); });
        bind('tabBtnForm', 'click', function () { showTab('form'); });
        bind('tabBtnHistory', 'click', function () { showTab('history'); });
        bind('ldrClose', 'click', closeLoader);
        bind('ldrShow', 'click', function (e) { run(e.currentTarget, showGrns); });
        bind('ldrLoad', 'click', function (e) { run(e.currentTarget, loadOnInvoice); });
        bind('ldrReset', 'click', function () { setVal('ldrFrom', ''); $('ldrBody').innerHTML = ''; $('ldrDetailBody').innerHTML = ''; });
        bind('ldrBody', 'click', function (e) { var tr = e.target.closest('tr'); if (tr && !e.target.classList.contains('ldr-check')) run(null, function () { return loaderDetail(num(tr.dataset.row)); }); });

        bind('cmbSupplier', 'change', supplierChanged);
        bind('cmbCommAgent', 'blur', commissionEvent);
        bind('cmbCommAgent', 'change', commissionEvent);
        bind('cmbCommType', 'change', commissionEvent);
        bind('txtCommRate', 'input', commissionEvent);
        bind('cmbCommUom', 'change', commissionEvent);
        bind('cmbBrokeryType', 'change', totalBrokery);
        bind('txtBrokeryRate', 'input', totalBrokery);
        bind('cmbBrokeryUom', 'change', totalBrokery);
        bind('cmbBroker', 'blur', totalBrokery);
        bind('cmbBroker', 'change', totalBrokery);
        bind('txtDueDays', 'input', dueDate);
        bind('dtDocDate', 'change', dueDate);
        /* txtGrnNo_KeyPress :3452 - OnlytextNumberFunction; txtGrnNo_Leave :3384 */
        bind('txtGrnNo', 'keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        bind('txtGrnNo', 'blur', function () { run(null, grnNoLeave); });

        bind('grdBody', 'change', function (e) {
            var t = e.target; if (!t.classList.contains('gd-edit')) return;
            var r = S.rows[num(t.dataset.row)];
            if (!numeric(t.value)) { msg('Input string was not in a correct format.'); renderGrid(); return; }
            r[t.dataset.key] = num(t.value);
            try { cellUpdated(r, t.dataset.key); } catch (x) { msg(x.message); }
        });
        bind('grdBody', 'click', function (e) { var a = e.target.closest('[data-order]'); if (a && window.DocLink) DocLink.open(41, num(a.dataset.order), { message: msg }); });
        function gridChange(bodyId, cls, list, handler, textKeys) {
            bind(bodyId, 'change', function (e) {
                var t = e.target; if (!t.classList.contains(cls)) return;
                var r = S[list][num(t.dataset.row)], key = t.dataset.key;
                if (textKeys.indexOf(key) >= 0) r[key] = t.value;
                else if (t.tagName === 'SELECT') r[key] = num(t.value);
                else { if (!numeric(t.value)) { msg('Input string was not in a correct format.'); handler(r, ''); return; } r[key] = t.value === '' ? '' : num(t.value); }
                try { handler(r, key); } catch (x) { msg(x.message); }
            });
        }
        gridChange('frBody', 'fr-edit', 'freight', freightUpdated, ['Remarks']);
        gridChange('glBody', 'gl-edit', 'ledger', ledgerUpdated, ['Remarks']);
        gridChange('expBody', 'exp-edit', 'expenses', expenseUpdated, ['Remarks']);
        gridChange('bagBody', 'bag-edit', 'bags', bagUpdated, ['Remarks']);
        /* grdFreight_ColumnButtonClick :872 / grdGLedger_ColumnButtonClick :805 / grdInvExp_ColumnButtonClick :840 */
        function gridButtons(bodyId, cls, list, blank, grnGuard, after) {
            bind(bodyId, 'click', function (e) {
                var b = e.target.closest('button.' + cls); if (!b) return;
                var i = num(b.dataset.row), rows = S[list];
                if (b.dataset.act === 'Delete') {
                    if (grnGuard && toInt(rows[i].InvGrnId) > 0) { msg('Record cannot be deleted because this record againt grn'); if (list === 'ledger') billAmount(); return; }
                    rows.splice(i, 1);
                    if (!rows.length) rows.push(blank());
                } else rows.push(blank());
                after();
            });
        }
        gridButtons('frBody', 'fr-btn', 'freight', blankFreight, true, function () { renderFreight(); billAmount(); freightProportion(); });
        gridButtons('glBody', 'gl-btn', 'ledger', blankLedger, true, function () { renderLedger(); billAmount(); });
        gridButtons('expBody', 'exp-btn', 'expenses', blankExpense, false, function () { renderExpenses(); billAmount(); expProportion(); });

        bind('histBody', 'click', function (e) {
            var b = e.target.closest('[data-act]');
            var tr = e.target.closest('tr'); if (tr) { S.historySel = num(tr.dataset.row); Array.from($('histBody').rows).forEach(function (x, k) { x.classList.toggle('sel', k === S.historySel); }); }
            if (b) { e.preventDefault(); run(b.tagName === 'BUTTON' ? b : null, function () { return historyAction(b.dataset.act, num(b.dataset.row)); }); }
        });
        /* grdHistory_DoubleClick :2442 - ReadById without Reset */
        bind('histBody', 'dblclick', function (e) { var tr = e.target.closest('tr'); if (tr && !e.target.closest('[data-act]')) run(null, function () { return readById(num(S.history[num(tr.dataset.row)].Id)); }); });
        ['grdWrap', 'histWrap', 'detWrap'].forEach(fullscreen);
        run(null, init);
    });
})();
