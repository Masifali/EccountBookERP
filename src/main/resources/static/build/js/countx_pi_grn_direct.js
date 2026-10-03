/* ============================================================================================
 * Purchase Invoice Against GRN Direct - desktop Architecture.WinApp.Purchase.frmPurchaseInvoiceAgaintGrnDirect
 * (ScreenDefinition 131, DocumentTypeId 138). Line numbers in comments are that .cs file.
 *
 * The browser does what the form's events do (grid edits, proportions, bill amount, FCY) so the
 * operator sees the desktop numbers while typing; the server repeats every derived amount on Save
 * and re-reads every GRN value, so nothing computed here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/purchase-invoice-against-grn-direct';
    var TYPE = 138, GRN_TYPE = 137;
    var $ = function (id) { return document.getElementById(id); };
    var S = {
        rows: [], expenses: [], bags: [], lookups: {}, cfg: {}, rights: {},
        recId: 0, voucherHeadId: 0, approved: false, baseCurrency: 0, baseRate: 0,
        suppliers: {}, history: [], historySel: -1, attachments: null, loaderRows: []
    };

    // ------------------------------------------------------------------ helpers
    function num(v) { if (v === null || v === undefined) return 0; var x = parseFloat(String(v).replace(/,/g, '')); return isFinite(x) ? x : 0; }
    function esc(v) { return String(v === null || v === undefined ? '' : v).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }
    function fmt(v, d) { var x = num(v); return x.toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function amt(v) { return fmt(v, S.cfg.amountDigits || 0); }
    function round(v, d) { var p = Math.pow(10, d); var x = Math.abs(v) * p; var r = Math.round(x); return (v < 0 ? -r : r) / p; } /* AwayFromZero */
    function three(v) { return round(v, 3); } /* ToString("#,##0.###") */
    function today() { var d = new Date(); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function addDays(iso, days) { var d = new Date(iso + 'T00:00:00'); d.setDate(d.getDate() + days); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function msg(text) { if (text) window.alert(text); }
    function sum(list, key) { return list.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    function option(sel, rows, idKey, textKey, blank) {
        var el = $(sel), keep = el.value;
        el.innerHTML = (blank ? '<option value="">' + esc(blank) + '</option>' : '') + rows.map(function (r) { return '<option value="' + esc(r[idKey]) + '">' + esc(r[textKey]) + '</option>'; }).join('');
        if (keep && rows.some(function (r) { return String(r[idKey]) === keep; })) el.value = keep;
    }
    function setVal(id, v) { var el = $(id); el.value = (v === null || v === undefined) ? '' : String(v); }
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
    function printRpt(rpt, args, button) {
        var w = window.open('', '_blank'); try { if (w) w.document.write('<p style="font:13px Segoe UI">Preparing report...</p>'); } catch (e) { }
        /* the clicked button stays disabled until the PDF arrived or the request failed */
        return PurchaseRequest.run(button || null, function () { return fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                return r.text().then(function (t) { if (w) w.close(); msg(t || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); msg(e.message); }); });
    }
    /* AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 138) */
    function print103(voucherHeadId, button) { printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: voucherHeadId, documentTypeId: TYPE }, button); }
    /* PurchaseInvoicettSlip_220A(Id) */
    function print220A(id, button) { printRpt('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: id }, button); }
    /* GeneratereportSlip :2816 - "No Record Found" when the id is 0 */
    function printSlip(rpt, id, button) { if (!(id > 0)) { msg('No Record Found'); return; } printRpt(rpt, { id: id }, button); }
    /* Clickable codes -> DocLink.open (CommonServices.EditMethodFromLinked): GRN Direct = 137, purchase order = 41. */
    function docLink(type, id, caption) { return num(id) > 0 ? '<a href="#" class="pigd-link" data-doclink="' + type + '" data-docid="' + num(id) + '">' + esc(caption) + '</a>' : esc(caption); }

    // ------------------------------------------------------------------ calculations (desktop events)
    function exchangeRate() { return num($('txtExchangeRate').value); }
    function fcyManual() { return $('chkFcyEnable').checked; }
    function hasOrder() { return S.rows.some(function (r) { return num(r.OrderId) > 0; }); }
    function supplierId() { return num($('cmbSupplier').value); }

    /* txtExchangeRate_TextChanged :587 */
    function exchangeChanged() {
        var ex = exchangeRate();
        S.rows.forEach(function (r) { r.FcyAmount = (S.rows.length > 0 && ex > 0) ? num(r.ItemAmount) / ex : 0; });
        billAmount();
    }
    /* TotalCommissionAmount :1381 */
    function totalCommission() {
        var rate = $('txtCommRate').value;
        $('txtCommAmount').value = rate !== '' ? String(three(sum(S.rows, 'ItemAmount') * num(rate) / 100)) : '0';
        commissionProportion();
    }
    /* TotalBrokeryAmount :1407 */
    function totalBrokery() {
        var rate = $('txtBrokeryRate').value;
        $('txtBrokeryAmount').value = rate !== '' ? String(three(sum(S.rows, 'ItemAmount') * num(rate) / 100)) : '0';
        brokeryProportion();
        billAmount();
    }
    function commissionProportion() {
        var a = num($('txtCommAmount').value), p = num($('txtCommRate').value);
        S.rows.forEach(function (r) { r.Commission = a > 0 ? num(r.ItemAmount) * p / 100 : 0; });
        billProportion();
    }
    function brokeryProportion() {
        var a = num($('txtBrokeryAmount').value), p = num($('txtBrokeryRate').value);
        S.rows.forEach(function (r) { r.Brokery = (a > 0 && p > 0) ? num(r.ItemAmount) * p / 100 : 0; });
        billProportion();
    }
    /* FreightProportion :1687 - only DeliveryTerm "Load" charges freight to the product */
    function freightProportion() {
        if (S.rows.length) {
            var net = sum(S.rows, 'NetBillWeight'), fr = num($('txtFreightAmount').value);
            S.rows.forEach(function (r) { r.Freights = $('txtDeliveryTerm').value === 'Load' ? fr / net * num(r.NetBillWeight) : 0; });
        }
        billProportion();
    }
    /* ExpProportion :1654 */
    function expProportion() {
        var total = sum(S.expenses, 'Amount'), net = sum(S.rows, 'NetBillWeight');
        S.rows.forEach(function (r) { r.Expense = total > 0 ? total / net * num(r.NetBillWeight) : 0; });
        billProportion();
    }
    /* WagesAmountProportion :1723 */
    function wagesProportion() {
        var w = num($('txtWagesAmount').value), qty = sum(S.rows, 'ItemQty'), net = sum(S.rows, 'NetBillWeight');
        S.rows.forEach(function (r) { r.WagesAmount = w > 0 ? (S.cfg.wagesAmountCalculateOnQty ? w / qty * num(r.ItemQty) : w / net * num(r.NetBillWeight)) : 0; });
        billProportion();
    }
    /* BillProportion :1773 - the wages term reads the cell object, not its value, so it is always 0 (desktop) */
    function billProportion() {
        S.rows.forEach(function (r) { r.ItemNetAmount = round(num(r.ItemAmount) + num(r.Expense) + num(r.Freights) + num(r.Commission) + num(r.Brokery), S.cfg.amountDigits || 0); });
        renderGrid();
    }
    /* BillAmount :1090 */
    function billAmount() {
        var item = sum(S.rows, 'ItemAmount'), exp = sum(S.expenses, 'Amount'), bags = 0;
        S.bags.forEach(function (b) { if (num(b.Amount) > 0 && num(b.TypeId) === 1) bags += num(b.Amount); });
        var bill = item + exp + bags, sup = supplierId();
        var comm = num($('txtCommAmount').value), brok = num($('txtBrokeryAmount').value), fr = num($('txtFreightAmount').value);
        if (comm > 0 && sup === num($('cmbCommAgent').value)) bill += comm;
        if ($('cmbBroker').value && brok > 0 && sup === num($('cmbBroker').value)) bill += brok;
        if (brok < 0) {
            if (sup === num($('cmbBroker').value)) { $('cmbBroker').value = ''; msg("Supplier And Brokery Account Can't be same!"); }
            else if (num($('cmbBroker').value) > 0) bill += brok;
        }
        if ($('cmbFreightAc').value && fr > 0) {
            var gl = S.suppliers[sup] ? S.suppliers[sup].GlAccountId : 0;
            if ($('txtDeliveryTerm').value === 'Ponch') {
                if (gl === num($('cmbFreightAc').value)) { $('cmbFreightAc').value = ''; msg("Supplier And Freight Account Can't be same in case of ponch!"); }
                else if (num($('cmbFreightAc').value) > 0) bill -= fr;
            } else if (gl === num($('cmbFreightAc').value)) bill += fr;
        }
        $('txtBillAmount').value = amt(round(bill, S.cfg.amountDigits || 0));
        if (!fcyManual()) {
            var ex = exchangeRate();
            $('txtFcyAmount').value = ex ? fmt(item / ex, 4) : '';
            $('txtLcyAmount').value = fmt(item, 4);
        }
        billProportion();
    }
    function allProportions() { totalCommission(); totalBrokery(); freightProportion(); expProportion(); wagesProportion(); billProportion(); billAmount(); }

    /* grd_CellUpdated :909 */
    function cellUpdated(r, key) {
        var ex = exchangeRate();
        if (key === 'ItemRate' || key === 'RateCut' || key === 'RateUom') {
            var net = num(r.NetBillWeight), rateUom = num(r.RateUom);
            if (rateUom === 0) { rateUom = 1; r.RateUom = 1; }
            r.RateCutAmount = net / rateUom * num(r.RateCut);
            var a = net / rateUom * num(r.ItemRate);
            r.ItemAmount = a - r.RateCutAmount;
            r.FcyAmount = (a - r.RateCutAmount) / ex;
        } else if (key === 'ItemAmount') {
            var ru = num(r.RateUom);
            if (ru === 0) { ru = 1; r.RateUom = 1; }
            r.ItemRate = num(r.ItemAmount) / ru;
            r.FcyAmount = num(r.ItemAmount) / ex;
        }
        if (!fcyManual()) exchangeChanged(); else billAmount();
    }
    /* TxtIheadertemRate_TextChanged :3191 */
    function headerRateChanged() {
        if (!S.rows.length) return;
        var rate = num($('txtHeaderItemRate').value), ex = exchangeRate();
        S.rows.forEach(function (r) {
            r.ItemRate = rate;
            var rateUom = num(r.RateUom), net = num(r.NetBillWeight);
            r.RateCutAmount = net / rateUom * num(r.RateCut);
            var a = net / rateUom * rate;
            r.ItemAmount = a - r.RateCutAmount;
            r.FcyAmount = (a - r.RateCutAmount) / ex;
        });
        totalCommission(); totalBrokery(); commissionProportion(); brokeryProportion(); wagesProportion();
        if (!fcyManual()) exchangeChanged(); else billAmount();
    }
    /* txtFcyAmount_TextChanged :696 - typing the FCY amount spreads it over the rows by net weight and
       then sets the header Item Rate box to "0", whose own TextChanged re-rates every row (desktop). */
    function fcyChanged(typed) {
        var fcy = num($('txtFcyAmount').value), ex = exchangeRate();
        $('txtLcyAmount').value = fmt(fcy * ex, 4);
        var net = sum(S.rows, 'NetBillWeight');
        if (typed && S.rows.length && net > 0) {
            S.rows.forEach(function (r) {
                var f = fcy / net * num(r.NetBillWeight);
                r.FcyAmount = f; r.ItemAmount = f * ex;
                var ru = num(r.RateUom); r.ItemRate = ru > 0 ? r.ItemAmount / ru : 0;
            });
            if ($('txtHeaderItemRate').value !== '0') { $('txtHeaderItemRate').value = '0'; headerRateChanged(); }
            renderGrid();
        }
    }
    /* DuedaysCalculates :2938 */
    function dueDate() { var d = $('txtDueDays').value.trim(); setVal('dtDueDate', d !== '' ? addDays($('dtDocDate').value || today(), num(d)) : ($('dtDocDate').value || today())); }
    /* CmbPaymentTerm_TextChanged :3251 */
    function paymentTermChanged() {
        var sel = $('cmbPaymentTerm'), text = sel.selectedIndex >= 0 ? sel.options[sel.selectedIndex].text : '';
        if (text === 'Cash') { $('txtDueDays').value = '0'; setVal('dtDueDate', $('dtDocDate').value); $('txtDueDays').disabled = true; }
        else $('txtDueDays').disabled = false;
    }

    // ------------------------------------------------------------------ grids
    var GRID = [ /* dtGrid :379-417, grdSettings :839 hides Id, OrderDetailId, OrderId, InvGrnId, GrnDetailId, ItemId, ItemUomId, WarehouseId, PackingTypeId, RateUomId, JobLotId, StockWeight */
        ['OrderNo', 'Order No', 's'], ['GrnDate', 'Grn Date', 's'], ['GrnNo', 'Grn No', 's'], ['ItemName', 'Item Name', 's'], ['CropYear', 'Crop Year', 's'],
        ['PackUom', 'Pack Uom', 's'], ['ItemQty', 'Item Qty', 'n'], ['Warehouse', 'Warehouse', 's'], ['GrossWeight', 'Gross Weight', 'n'],
        ['EbUnit', 'Eb Unit', 'n'], ['EbTotal', 'Eb Total', 'n'], ['AdLsWeight', 'Ad Ls Weight', 'n'], ['NetBillWeight', 'Net Bill Weight', 'n'],
        ['ItemRate', 'Item Rate', 'e'], ['RateUom', 'Rate Uom', 'e0'], ['ItemAmount', 'Item Amount', 'e'], ['FcyAmount', 'Fcy Amount', 'n'],
        ['RateCut', 'Rate Cut', 'e'], ['RateCutAmount', 'Rate Cut Amount', 'n'], ['Freights', 'Freights', 'n'], ['Expense', 'Expense', 'n'],
        ['Commission', 'Commission', 'n'], ['Brokery', 'Brokery', 'n'], ['ItemNetAmount', 'Item Net Amount', 'n'], ['GpNo', 'Gp No', 's'],
        ['VehicleNo', 'Vehicle No', 's'], ['WagesAmount', 'Wages Amount', 'n']];
    function renderGrid() {
        var order = hasOrder();
        var cols = GRID.filter(function (c) { return c[0] !== 'OrderNo' || order; });
        $('grdHead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        $('grdBody').innerHTML = S.rows.map(function (r, idx) {
            return '<tr>' + cols.map(function (c) {
                var key = c[0], editable = c[2] === 'e' || c[2] === 'e0';
                if (editable && order && (key === 'ItemRate' || key === 'RateUom')) editable = false; /* grdSettings :872-873 */
                if (editable) return '<td class="num"><input class="gd-edit" data-row="' + idx + '" data-key="' + key + '" value="' + esc(c[2] === 'e0' ? Math.round(num(r[key])) : round(num(r[key]), 4)) + '" aria-label="' + esc(c[1]) + '"></td>';
                if (key === 'GrnNo') return '<td>' + docLink(GRN_TYPE, r.InvGrnId, r[key]) + '</td>';
                if (key === 'OrderNo') return '<td>' + docLink(41, r.OrderId, r[key]) + '</td>';
                if (c[2] === 's') return '<td>' + esc(r[key]) + '</td>';
                return '<td class="num">' + fmt(r[key], key === 'RateUom' ? 0 : 2) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        /* the Janus total row is always on screen, 0 when the grid is empty (desktop screenshot) */
        $('grdFoot').innerHTML = '<tr>' + cols.map(function (c) { return (c[2] === 's' || c[0] === 'ItemRate' || c[0] === 'RateUom' || c[0] === 'RateCut') ? '<td></td>' : '<td class="num"><b>' + (S.rows.length ? fmt(sum(S.rows, c[0]), 2) : '0') + '</b></td>'; }).join('') + '</tr>';
        $('lblRecords').textContent = (S.rows.length ? 1 : 0) + ' Of ' + S.rows.length;
    }
    function renderExpenses() {
        $('expBody').innerHTML = S.expenses.map(function (e, idx) {
            return '<tr><td>' + esc(e.ItemName) + '</td><td><input class="exp-edit" data-row="' + idx + '" data-key="Remarks" value="' + esc(e.Remarks) + '" aria-label="Remarks"></td>'
                + '<td class="num"><input class="exp-edit" data-row="' + idx + '" data-key="Amount" value="' + esc(num(e.Amount) || 0) + '" aria-label="Amount"></td></tr>';
        }).join('');
        if ($('lblExpRecords')) $('lblExpRecords').textContent = (S.expenses.length ? 1 : 0) + ' Of ' + S.expenses.length;
        $('expFoot').innerHTML = '<tr><td></td><td></td><td class="num"><b>' + fmt(sum(S.expenses, 'Amount'), 0) + '</b></td></tr>';
    }
    function renderBags() {
        var accounts = S.lookups.bagCreditAccounts || [];
        $('bagBody').innerHTML = S.bags.map(function (b, idx) {
            var opts = '<option value="0"></option>' + accounts.map(function (a) { return '<option value="' + esc(a.Id) + '"' + (num(a.Id) === num(b.CreditAccountId) ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>'; }).join('');
            return '<tr><td>' + esc(b.Type) + '</td><td>' + esc(b.ItemName) + '</td><td class="num">' + fmt(b.PurchaseQty, 3) + '</td>'
                + '<td class="num"><input class="bag-edit" data-row="' + idx + '" data-key="Rate" value="' + esc(num(b.Rate)) + '" aria-label="Rate"></td>'
                + '<td class="num">' + amt(b.Amount) + '</td>'
                + '<td><input class="bag-edit" data-row="' + idx + '" data-key="Remarks" value="' + esc(b.Remarks) + '" aria-label="Remarks"></td>'
                + '<td><select class="bag-edit" data-row="' + idx + '" data-key="CreditAccountId" data-dtcombo="single" data-dtcombo-caption="Credit account" aria-label="Credit account">' + opts + '</select></td></tr>';
        }).join('');
        if ($('lblBagRecords')) $('lblBagRecords').textContent = (S.bags.length ? 1 : 0) + ' Of ' + S.bags.length;
        $('bagFoot').innerHTML = '<tr><td></td><td></td><td class="num"><b>' + fmt(sum(S.bags, 'PurchaseQty'), 3) + '</b></td><td></td><td class="num"><b>' + amt(sum(S.bags, 'Amount')) + '</b></td><td></td><td></td></tr>';
    }

    /* grdSettings :870-876 - rows that carry a purchase order lock the rate, the header rate and the FCY box */
    function grdSettings() {
        var order = hasOrder();
        $('txtHeaderItemRate').disabled = order;
        $('chkFcyEnable').checked = !order;
        $('chkFcyEnable').disabled = order;
        $('txtFcyAmount').disabled = !$('chkFcyEnable').checked;
        renderGrid();
    }

    // ------------------------------------------------------------------ load / reset
    function applyLookups(d) {
        S.lookups = Object.assign(S.lookups, d);
        if (d.suppliers) {
            S.suppliers = {}; d.suppliers.forEach(function (s) { S.suppliers[num(s.Id)] = s; });
            ['cmbSupplier', 'cmbCommAgent', 'cmbBroker'].forEach(function (id) { option(id, d.suppliers, 'Id', 'CompanyName', ' '); });
        }
        if (d.paymentTerms) option('cmbPaymentTerm', d.paymentTerms, 'Id', 'TermsDescription', ' ');
        if (d.freightAccounts) option('cmbFreightAc', d.freightAccounts, 'Id', 'AccountTitle', ' ');
        if (d.locationTypes) {
            option('cmbLocationType', d.locationTypes, 'Id', 'Location', '--Select--');
            if (!$('cmbLocationType').value && d.locationTypes.length) $('cmbLocationType').value = String(d.locationTypes[0].Id); /* Rows[1].Activate() :497 */
        }
        if (d.currencies) option('cmbCurrency', d.currencies, 'Id', 'CurrencyCode', ' ');
        if (d.historySuppliers) option('cmbHistorySupplier', d.historySuppliers, 'Id', 'Supplier', ' ');
    }
    /* Base Currency / BaseCurrencyRate (:463-481, :2868-2886) */
    function currencyDefaults() {
        if (S.baseCurrency && !num($('cmbCurrency').value)) $('cmbCurrency').value = String(S.baseCurrency);
        if (S.cfg.baseCurrencyRate !== '' && num($('txtExchangeRate').value) === 0) $('txtExchangeRate').value = String(S.baseRate);
    }
    function otherItemsBind() { S.expenses = (S.lookups.otherItems || []).map(function (o) { return { ItemId: o.Id, ItemName: o.OtherItemName, Remarks: '', Amount: 0 }; }); renderExpenses(); }

    async function init() {
        var d = await api(API + '/init');
        S.rights = d.rights || {}; S.cfg = d.configuration || {};
        S.finYearStart = d.financialYearStart || ''; S.currentBranchId = num(d.currentBranchId);
        S.baseCurrency = num(S.cfg.baseCurrency); S.baseRate = num(S.cfg.baseCurrencyRate);
        applyLookups(d);
        otherItemsBind();
        var last = d.lastTransport || {}; /* GetLastInvoiceTransportAndBrokerAgentId :1493 */
        if (num(last.BrokerAgentId)) $('cmbBroker').value = String(last.BrokerAgentId);
        if (num(last.TransportAccountId)) $('cmbFreightAc').value = String(last.TransportAccountId);
        setVal('txtDocNo', d.docNo); if (d.docNoMessage) msg(d.docNoMessage);
        setVal('dtDocDate', today()); dueDate();
        $('chkSlipPrint').checked = true;
        var days = num(S.cfg.defaultDaysToLessFromHistoryFromDate);
        setVal('dtHistoryFrom', addDays(today(), -(days > 0 ? days : 3))); setVal('dtHistoryTo', today());
        currencyDefaults();
        $('btnSave').disabled = !S.rights.Save; $('btnUpdate').disabled = !S.rights.Update; $('btnDelete').disabled = !S.rights.Delete; $('btnPrint103').disabled = !S.rights.Print;
        grdSettings(); renderBags();
        var q = new URLSearchParams(location.search).get('id');
        if (num(q) > 0) await readById(num(q));
    }

    /* Reset :2066 */
    async function reset() {
        if (S.attachments) S.attachments.reset();
        S.recId = 0; S.voucherHeadId = 0; S.approved = false;
        ['txtHeaderItemRate', 'txtCommRate', 'txtCommAmount', 'txtBrokeryRate', 'txtBrokeryAmount', 'txtFreightAmount', 'txtManualBillNo', 'txtDeliveryTerm', 'txtRemarks', 'txtDueDays', 'txtWagesAmount'].forEach(function (id) { $(id).value = ''; });
        $('cmbCommAgent').value = '';
        setVal('dtDueDate', today());
        S.rows = [];
        var d = await api(API + '/next-code'); setVal('txtDocNo', d.docNo);
        $('btnSave').hidden = false; $('btnUpdate').hidden = true;
        S.otherBranch = false; $('btnUpdate').disabled = !S.rights.Update; $('btnDelete').disabled = !S.rights.Delete;
        $('cmbSupplier').disabled = false; $('cmbSupplier').value = '';
        S.bags = []; renderBags();
        $('txtBillAmount').value = '0';
        S.expenses.forEach(function (e) { e.Amount = 0; e.Remarks = ''; }); renderExpenses(); /* OtherItemReset :3074 */
        $('txtHeaderItemRate').disabled = false; $('chkFcyEnable').disabled = false;
        $('btnDelete').hidden = true;
        renderGrid();
    }

    /* ReadById :2127 */
    async function readById(id) {
        var d = await api(API + '/' + id);
        S.recId = id; $('btnSave').hidden = true; $('btnUpdate').hidden = false;
        setVal('txtDocNo', d.DocNo); setVal('dtDocDate', d.DocDate); showTab('form');
        $('cmbSupplier').value = String(d.SupplierCustomerId); $('cmbSupplier').disabled = true;
        setVal('txtManualBillNo', d.ManualBillNo);
        if (num(d.PaymentTermsId) > 0) $('cmbPaymentTerm').value = String(d.PaymentTermsId);
        setVal('txtDeliveryTerm', d.DeliveryTerm);
        $('cmbCommAgent').value = num(d.CommissionAgentId) ? String(d.CommissionAgentId) : '';
        setVal('txtCommRate', d.CommRate); setVal('txtCommAmount', d.CommAmount);
        $('cmbBroker').value = num(d.BrokerAgentId) ? String(d.BrokerAgentId) : '';
        setVal('txtBrokeryRate', d.BrokeryRate); setVal('txtBrokeryAmount', d.BrokeryAmount);
        $('cmbFreightAc').value = num(d.TransportAccountId) ? String(d.TransportAccountId) : '';
        setVal('txtFreightAmount', d.FreightAmount); setVal('txtRemarks', d.RemarksHeader);
        setVal('txtWagesAmount', d.WagesAmount); setVal('txtBillAmount', d.BillAmount);
        setVal('dtDueDate', d.DueDate); setVal('txtDueDays', d.DueDays);
        paymentTermChanged();
        S.approved = !!d.IsApproved; S.voucherHeadId = num(d.VoucherHeadId);
        /* an invoice of another branch (opened from History) is read-only: desktop Update would move it to this branch */
        S.otherBranch = !!d.OtherBranch;
        $('btnUpdate').disabled = !S.rights.Update || S.otherBranch; $('btnDelete').disabled = !S.rights.Delete || S.otherBranch;
        if (S.otherBranch) msg('This invoice belongs to another branch. It is shown read-only; open it in that branch to update or delete it.');
        S.rows = d.details || [];
        if (num(d.LocationTypeId) > 0) $('cmbLocationType').value = String(d.LocationTypeId);
        S.bags = d.emptyBags || []; renderBags();
        S.expenses = d.expenses || []; renderExpenses();
        grdSettings();
        totalCommission(); totalBrokery(); freightProportion(); expProportion(); commissionProportion(); wagesProportion(); brokeryProportion();
        if (S.attachments) S.attachments.reset();
        $('btnDelete').hidden = false;
    }

    // ------------------------------------------------------------------ Load GRN (frmLoadGRN, DocumentTypeId 137)
    var LDR = [['BranchName', 'Branch Name'], ['DocDate', 'Doc Date'], ['DocNo', 'Doc No'], ['SupplierCustomer', 'Supplier Customer'], ['DeliveryTerm', 'Delivery Term'],
        ['GpDate', 'Gp Date'], ['BiltyNo', 'Bilty No'], ['VehicleNo', 'Vehicle No'], ['GrnStatus', 'Grn Status']]; /* 137 hides PurchaseOrder, PurchaseAgainst, GpNO (:335-338) */
    async function openLoader() {
        $('grnLoader').hidden = false;
        /* frmLoadGRN_Load: From Date = ActiveYr.Start_Period, To Date = today; BranchesFill ticks the user's branch
           (UserAccount.BranchName), else the first allocated branch; then PendingGrnLoad. */
        setVal('ldrFrom', S.finYearStart || today()); setVal('ldrTo', today());
        var branches = await api('/api/grn-loader/branches?docTypeId=' + GRN_TYPE);
        var mine = branches.some(function (b) { return num(b.BranchId) === S.currentBranchId; });
        $('ldrBranches').innerHTML = branches.map(function (b, i) { return '<label><input type="checkbox" value="' + esc(b.BranchId) + '"' + ((mine ? num(b.BranchId) === S.currentBranchId : i === 0) ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join('');
        await showGrns();
    }
    async function showGrns() {
        var ids = Array.from($('ldrBranches').querySelectorAll('input:checked')).map(function (i) { return i.value; }).join(',');
        if (!ids) throw Error('Select branch first');
        var q = new URLSearchParams({ docTypeId: GRN_TYPE, fromDate: $('ldrFrom').value, toDate: $('ldrTo').value, branchIds: ids });
        S.loaderRows = await api('/api/grn-loader/pending?' + q.toString());
        $('ldrHead').innerHTML = '<tr><th>Select</th>' + LDR.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        $('ldrBody').innerHTML = S.loaderRows.map(function (r, i) {
            return '<tr data-row="' + i + '"><td><input type="checkbox" class="ldr-check" data-row="' + i + '" aria-label="Select GRN"></td>' + LDR.map(function (c) { var v = r[c[0]]; if (/Date$/.test(c[0]) && v) v = String(v).substring(0, 10); return '<td>' + (c[0] === 'DocNo' ? docLink(GRN_TYPE, r.Id, v) : esc(v)) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $('ldrDetailBody').innerHTML = '';
    }
    /* frmLoadGRN.grd_SelectionChanged: dthistorydetail (Id, GrnId hidden) from the InvGrnDetail model = Sp_InvGrnDetail_GetAllMethod
       'ReadByInvGrnIdDirect' columns; UOM is UOM.Equivalent and WareHouse shows WarehouseId, as the desktop fills them. */
    var LDR_DETAIL = [['PurchaseOrder', 'Order'], ['Item', 'Item'], ['CropYear', 'CropYear'], ['JobLot', 'Job'], ['PackingType', 'PackingType'], ['UOM', 'UOM'],
        ['ItemQty', 'Qty', 'n'], ['GrossWeight', 'GrossWight', 'n'], ['EBWPerUnit', 'EbUnit', 'n'], ['EBWTotal', 'EbTotal', 'n'], ['EbPurAgainstWeight', 'EbPurAgainstWeight', 'n'],
        ['WtCut', 'WtCut', 'n'], ['WtCutTotal', 'WtCutTotal', 'n'], ['AdLsWeight', 'AddLesswt', 'n'], ['NetBillWeight', 'NetWeight', 'n'], ['StockWeight', 'StockWeight', 'n'],
        ['WarehouseId', 'WareHouse'], ['LabReportRef', 'LabNo'], ['AreaCity', 'City']];
    async function loaderDetail(i) {
        var r = S.loaderRows[i]; if (!r) return;
        Array.from($('ldrBody').rows).forEach(function (tr, k) { tr.classList.toggle('sel', k === i); });
        var rows = await api('/api/grn-loader/details/' + r.Id);
        $('ldrDetailHead').innerHTML = '<tr>' + LDR_DETAIL.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        $('ldrDetailBody').innerHTML = rows.map(function (x) { return '<tr>' + LDR_DETAIL.map(function (c) { return c[2] ? '<td class="num">' + esc(x[c[0]] == null ? '' : x[c[0]]) + '</td>' : '<td>' + esc(x[c[0]]) + '</td>'; }).join('') + '</tr>'; }).join('');
    }
    /* btnLoadOnInvoice_Click_1 (frmLoadGRN :496) then LoadInGridDetail :1593 */
    async function loadOnInvoice() {
        var picked = Array.from(document.querySelectorAll('.ldr-check:checked')).map(function (c) { return { Id: S.loaderRows[num(c.dataset.row)].Id }; });
        if (!picked.length) throw Error('Check the row first');
        var v = await api('/api/grn-loader/validate-selection?docTypeId=' + GRN_TYPE + '&acceptAccessWtHold=' + (!!S.cfg.acceptAccessWtHold), { body: picked });
        if (!v || v.success === false) throw Error((v && v.message) || 'Check the row first');
        $('grnLoader').hidden = true;
        var d = await api(API + '/load-grns', { body: {
            grnIds: v.grns.map(function (g) { return g.Id; }), supplierId: supplierId(), deliveryTerm: $('txtDeliveryTerm').value,
            rowCount: S.rows.length, firstInvGrnId: S.rows.length ? S.rows[0].InvGrnId : 0,
            existingGrnDetailIds: S.rows.map(function (r) { return r.GrnDetailId; }), exchangeRate: exchangeRate() } });
        if (d.wagesAmount !== undefined) $('txtWagesAmount').value = d.wagesAmount;
        if (d.message) { wagesProportion(); billAmount(); throw Error(d.message); }
        if (!d.rows || (!d.rows.length && d.supplierId === undefined)) return;
        $('cmbSupplier').value = String(d.supplierId); $('cmbSupplier').disabled = true;
        $('cmbCommAgent').value = String(d.supplierId);
        $('txtDeliveryTerm').value = d.deliveryTerm || '';
        S.rows = S.rows.concat(d.rows);
        $('txtFreightAmount').value = amt(d.freightAmount);
        $('cmbFreightAc').value = num(d.transporterId) ? String(d.transporterId) : '';
        grdSettings();
        S.bags = d.emptyBags || []; renderBags();
        totalCommission(); totalBrokery(); freightProportion(); expProportion(); wagesProportion(); billProportion(); billAmount();
    }

    // ------------------------------------------------------------------ save / update / delete
    /* FormValidation :1163 (browser side; the server repeats every check) */
    function formValidation() {
        var checks = [
            [(S.lookups.locationTypes || []).length && !num($('cmbLocationType').value), 'Location Type Field is Required', 'cmbLocationType'],
            [!num($('txtDocNo').value), 'Doc No Name is Required', 'txtDocNo'],
            [supplierId() <= 0, 'Supplier Name is Required', 'cmbSupplier'],
            [num($('cmbPaymentTerm').value) <= 0, 'Payment Term is Required', 'cmbPaymentTerm'],
            [num($('cmbPaymentTerm').value) === 2 && num($('txtDueDays').value) <= 0, 'DueDays Field is Required', 'txtDueDays'],
            [$('txtDeliveryTerm').value === '', 'Delivery Term is Required', 'txtDeliveryTerm'],
            [!num($('cmbCurrency').value), 'Fcy Code Field is Required', 'cmbCurrency'],
            [$('txtExchangeRate').value.trim() === '' || $('txtExchangeRate').value.trim() === '0', 'Exchange Rate Field is Required', 'txtExchangeRate'],
            [$('txtFcyAmount').value.trim() === '' || $('txtFcyAmount').value.trim() === '0', 'Fcy Amount Rate Field is Required', 'txtFcyAmount']];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { msg(checks[i][1]); try { $(checks[i][2]).focus(); } catch (e) { } return false; }
        return true;
    }
    async function insert(button) {
        if (!formValidation()) return;
        if (!window.confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            id: S.recId, docDate: $('dtDocDate').value, supplierId: supplierId(), paymentTermId: num($('cmbPaymentTerm').value),
            dueDays: $('txtDueDays').value, manualBillNo: $('txtManualBillNo').value, remarks: $('txtRemarks').value,
            commissionAgentId: num($('cmbCommAgent').value), commRate: $('txtCommRate').value, commAmount: $('txtCommAmount').value,
            brokerAgentId: num($('cmbBroker').value), brokeryRate: $('txtBrokeryRate').value,
            transportAccountId: num($('cmbFreightAc').value), freightAmount: num($('txtFreightAmount').value),
            locationTypeId: num($('cmbLocationType').value), currencyId: num($('cmbCurrency').value), exchangeRate: exchangeRate(),
            fcyManual: fcyManual(), fcyAmount: num($('txtFcyAmount').value),
            details: S.rows.map(function (r) { return { Id: r.Id, GrnDetailId: r.GrnDetailId, InvGrnId: r.InvGrnId, ItemRate: r.ItemRate, RateUom: r.RateUom, RateCut: r.RateCut, ItemAmount: r.ItemAmount, FcyAmount: r.FcyAmount }; }),
            expenses: S.expenses.map(function (e) { return { ItemId: e.ItemId, Remarks: e.Remarks, Amount: e.Amount }; }),
            emptyBags: S.bags.map(function (b) { return { InvGrnId: b.InvGrnId, ItemId: b.ItemId, TypeId: b.TypeId, Rate: b.Rate, Remarks: b.Remarks, CreditAccountId: num(b.CreditAccountId) }; })
        };
        if (S.attachments) { await S.attachments.settled(); var a = S.attachments.payload(); if (a) body.attachments = a; }
        var d = await api(API + '/save', { body: body });
        msg(d.message);
        var slip = $('chkSlipPrint').checked, voucher = $('chkVoucherPrint').checked;
        await reset();
        if (slip) print220A(d.id);
        if (voucher) print103(d.voucherHeadId);
    }
    /* btnUpdate_Click :2215 */
    async function update(button) {
        if (S.approved) throw Error('Record Not Update because Record has approved');
        if (S.recId === 0) throw Error('Record Not Update because Record Id not found');
        await insert(button);
    }
    /* btnDelete_Click :3466 */
    async function remove() {
        if (S.approved) throw Error('Record Not Delete because Record has approved');
        if (!(S.recId > 0)) { msg('Record Not Found'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        var d = await api(API + '/delete/' + S.recId, { body: {} });
        msg(d.message); await reset();
    }

    // ------------------------------------------------------------------ History tab
    var HIST = [['DocNo', 'Doc No'], ['DocDate', 'Doc Date'], ['SupplierName', 'Supplier Name'], ['ManualBillNo', 'Manual Bill No'], ['DueDays', 'Due Days'], ['DueDate', 'Due Date'],
        ['CommAgent', 'Comm Agent'], ['CommType', 'Comm Type'], ['CommRate', 'Comm Rate'], ['CommAmount', 'Comm Amount', 'n3'], ['CommRemarks', 'Comm Remarks'], ['BillAmount', 'Bill Amount', 'a'],
        ['ApprovedStatus', 'Approved Status'], ['EntryUser', 'Entry User'], ['EntryDate', 'Entry Date', 'dt'], ['ModifyUser', 'Modify User'], ['ModifyDate', 'Modify Date', 'dt'],
        ['ApprovedUser', 'Approved User'], ['ApprovedDate', 'Approved Date', 'dt'], ['NoOfAttachments', 'No Of Attachments', 'link'], ['Remarks', 'Remarks']];
    function dt(v) { if (!v) return ''; var d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return String(v); var m = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][d.getMonth()]; var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12; return String(d.getDate()).padStart(2, '0') + '-' + m + '-' + d.getFullYear() + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap; }
    async function showHistory() {
        var mode = document.querySelector('input[name=histDate]:checked').value;
        S.history = await api(API + '/history', { body: {
            dateMode: mode, fromDate: $('chkHistFrom').checked ? $('dtHistoryFrom').value : '', toDate: $('chkHistTo').checked ? $('dtHistoryTo').value : '',
            fromDocNo: num($('txtHistFromNo').value), toDocNo: num($('txtHistToNo').value), supplierId: num($('cmbHistorySupplier').value) } });
        renderHistory();
    }
    function renderHistory() {
        var btns = [['Edit', 'Edit'], ['Voucher', 'Voucher'], ['View', 'Slip'], ['PartySlip', 'PartySlip'], ['SummaryReport', 'SummaryReport']];
        $('histHead').innerHTML = '<tr>' + btns.map(function (b) { return '<th>' + b[1] + '</th>'; }).join('') + HIST.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '<th>Add Attachment</th></tr>';
        $('histBody').innerHTML = S.history.map(function (r, i) {
            return '<tr data-row="' + i + '" tabindex="0"' + (i === S.historySel ? ' class="sel"' : '') + '>' + btns.map(function (b) { return '<td><button type="button" data-act="' + b[0] + '" data-row="' + i + '">' + (b[0] === 'View' ? 'View' : b[0]) + '</button></td>'; }).join('')
                + HIST.map(function (c) {
                    var v = r[c[0]];
                    if (c[0] === 'DocNo') return '<td><a href="#" class="pigd-link" data-act="Edit" data-row="' + i + '">' + esc(v) + '</a></td>';
                    if (c[2] === 'dt') return '<td>' + esc(dt(v)) + '</td>';
                    if (c[2] === 'a') return '<td class="num">' + amt(v) + '</td>';
                    if (c[2] === 'n3') return '<td class="num">' + fmt(v, 3) + '</td>';
                    if (c[2] === 'link') return '<td class="num"><a href="#" data-act="NoOfAttachments" data-row="' + i + '">' + esc(v) + '</a></td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '<td><button type="button" data-act="AddAttachment" data-row="' + i + '">Add Attachment</button></td></tr>';
        }).join('');
        $('histFoot').innerHTML = S.history.length ? '<tr><td colspan="5"></td>' + HIST.map(function (c) { return c[0] === 'CommAmount' ? '<td class="num"><b>' + fmt(sum(S.history, 'CommAmount'), 3) + '</b></td>' : c[0] === 'BillAmount' ? '<td class="num"><b>' + amt(sum(S.history, 'BillAmount')) + '</b></td>' : '<td></td>'; }).join('') + '<td></td></tr>' : '';
    }
    var DET = [['OrderNo', 'Order No'], ['GrnDate', 'Grn Date'], ['GrnNo', 'Grn No'], ['Warehouse', 'Warehouse'], ['ItemName', 'Item Name'], ['CropYear', 'Crop Year'], ['PackType', 'Pack Type'], ['PackUom', 'Pack Uom'],
        ['ItemQty', 'Item Qty', 'n'], ['GrossWeight', 'Gross Weight', 'n'], ['EbUnit', 'Eb Unit', 'n'], ['EbTotal', 'Eb Total', 'n'], ['NetBillWeight', 'Net Bill Weight', 'n'], ['StockWeight', 'Stock Weight', 'n'],
        ['ItemRate', 'Item Rate', 'n'], ['RateUom', 'Rate Uom'], ['ItemAmount', 'Item Amount', 'n'], ['RateCut', 'Rate Cut', 'n'], ['RateCutAmount', 'Rate Cut Amount', 'n'], ['CommissionAmount', 'Commission Amount', 'n'],
        ['Brokery', 'Brokery', 'n'], ['ExpenseAmount', 'Expense Amount', 'n'], ['FreightAmount', 'Freight Amount', 'n'], ['WagesAmount', 'Wages Amount', 'n'], ['ItemNetAmount', 'Item Net Amount', 'n'], ['VehicleNo', 'Vehicle No']];
    /* grdHistory_SelectionChanged -> GetDetailGrdByHeadId :1014 */
    async function historyDetail(i) {
        S.historySel = i; Array.from($('histBody').rows).forEach(function (tr, k) { tr.classList.toggle('sel', k === i); });
        var r = S.history[i]; if (!r) { $('detHead').innerHTML = ''; $('detBody').innerHTML = ''; return; }
        var rows = await api(API + '/' + r.Id + '/detail');
        var showOrder = rows.some(function (x) { return num(x.OrderNo) > 0; });
        var cols = DET.filter(function (c) { return c[0] !== 'OrderNo' || showOrder; });
        $('detHead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        $('detBody').innerHTML = rows.map(function (x) { return '<tr>' + cols.map(function (c) { return c[2] === 'n' ? '<td class="num">' + fmt(x[c[0]], 2) + '</td>' : c[0] === 'GrnNo' ? '<td>' + docLink(GRN_TYPE, x.InvGrnId, x.GrnNo) + '</td>' : c[0] === 'OrderNo' ? '<td>' + docLink(41, x.OrderId, x.OrderNo) + '</td>' : '<td>' + esc(x[c[0]]) + '</td>'; }).join('') + '</tr>'; }).join('');
    }
    async function historyAction(act, i, button) {
        var r = S.history[i]; if (!r) return;
        if (act === 'Edit') { await reset(); await readById(r.Id); }
        else if (act === 'Voucher') await print103(num(r.VoucherHeadId), button);
        else if (act === 'View') await printSlip('220-InvRptPurchaseBillSupplierRiceSlip.rpt', r.Id, button);
        else if (act === 'PartySlip') await print220A(r.Id, button);
        else if (act === 'SummaryReport') await printSlip('220B-InvRptPurchaseBillSupplierRiceSummary.rpt', r.Id, button);
        /* grdHistory_LinkClicked :2904 - GetNoofAttachmentsByRefDocumentTypeID(Id, 138): the attachment list, read-only */
        else if (act === 'NoOfAttachments') await PurchaseInvoiceAttachments.view({ type: TYPE, id: r.Id, message: msg });
        else if (act === 'AddAttachment') {
            /* AttachmentAddingFromHistory: the web attachment dialog stages changes with the invoice, so the record is
               opened first and changes are saved with Update. */
            await reset(); await readById(r.Id); await attachmentsDialog();
        }
    }
    /* btnReset_Click :3336 - it also clears the FORM's supplier combo (desktop) */
    function resetHistory() {
        setVal('dtHistoryFrom', today()); setVal('dtHistoryTo', today()); $('txtHistFromNo').value = ''; $('txtHistToNo').value = '';
        $('cmbSupplier').value = '';
        S.history = []; S.historySel = -1; renderHistory(); $('detHead').innerHTML = ''; $('detBody').innerHTML = '';
        document.querySelector('input[name=histDate][value=doc]').checked = true;
    }

    // ------------------------------------------------------------------ misc
    function showTab(t) {
        $('tabForm').hidden = t !== 'form'; $('tabHistory').hidden = t !== 'history';
        $('tabBtnForm').setAttribute('aria-selected', t === 'form'); $('tabBtnHistory').setAttribute('aria-selected', t === 'history');
        $('btnFooterHistory').querySelector('span').textContent = t === 'history' ? 'Form' : 'History';
        $('btnFooterHistory').querySelector('i').className = t === 'history' ? 'fa fa-file-text-o' : 'fa fa-history';
        if (t === 'history') $('dtHistoryFrom').focus();
    }
    async function attachmentsDialog(button) {
        if (!S.attachments) S.attachments = PurchaseInvoiceAttachments.create({ type: TYPE, getId: function () { return S.recId; }, canEdit: function () { return S.recId > 0 ? !!S.rights.Update : !!S.rights.Save; }, message: msg });
        await S.attachments.open(button);
    }
    /* MakeShortCutKeys :2702 */
    var KEYS = [['Ctrl+S', 'For Save when in form tab and for show data when in history tab'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print Voucher when in form tab'], ['Alt+2', 'For Print 220A when in form tab'], ['Alt+3', 'For Print 220B when in form tab'],
        ['Ctrl+F5', 'For Focus on Doc Date Name'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus Grids'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcutKeys() {
        $('keysBody').innerHTML = KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        $('keysDialog').hidden = false;
    }
    function formTab() { return !$('tabForm').hidden; }
    /* InvfrmPurchaseInvoice_KeyDown :2574 */
    document.addEventListener('keydown', function (e) {
        if (!$('grnLoader').hidden) {   /* frmLoadGRN_KeyDown */
            var lk = e.key.toLowerCase();
            if (e.key === 'Escape' || (e.ctrlKey && lk === 'e')) { e.preventDefault(); $('grnLoader').hidden = true; }
            else if (e.ctrlKey && lk === 's') { e.preventDefault(); $('ldrShow').click(); }
            else if (e.ctrlKey && lk === 'l') { e.preventDefault(); $('ldrLoad').click(); }
            else if (e.ctrlKey && lk === 'n') { e.preventDefault(); $('ldrReset').click(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('ldrFrom').focus(); }
            return;
        }
        var k = e.key.toLowerCase(), ctrl = e.ctrlKey;
        if (ctrl && k === 's') { e.preventDefault(); if (formTab()) { if (!$('btnSave').hidden && !$('btnSave').disabled) $('btnSave').click(); } else $('btnShow').click(); }
        if (ctrl && k === 'n') { e.preventDefault(); if (formTab()) $('btnNew').click(); else resetHistory(); }
        if (ctrl && k === 't') { e.preventDefault(); showTab(formTab() ? 'history' : 'form'); if (formTab()) $('dtDocDate').focus(); }
        if ((ctrl && k === 'e') || e.key === 'Escape') {
            e.preventDefault();
            var att = document.querySelector('.invoice-attachments:not([hidden])');
            if (att) { att.hidden = true; return; }
            if (!$('keysDialog').hidden) { $('keysDialog').hidden = true; return; }
            window.location.href = '/purchase';
        }
        if (ctrl && e.key === 'F5') { e.preventDefault(); $('dtDocDate').focus(); }
        if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) { if (ctrl && k === 'u') { e.preventDefault(); $('btnUpdate').click(); } }
        else if (e.altKey && (e.key === '1')) { e.preventDefault(); if (S.rights.Print) $('btnPrint103').click(); }
        else if (e.altKey && (e.key === '2')) { e.preventDefault(); if (S.rights.Print) $('btn220A').click(); }
        else if (e.altKey && (e.key === '3')) { e.preventDefault(); if (S.rights.Print) $('btn220B').click(); }
        if (ctrl && e.key === 'F10') { e.preventDefault(); $('btnAttachment').click(); }
        if (ctrl && k === 'l') { e.preventDefault(); $('btnLoadGrn').click(); }
        if (ctrl && e.key === 'ArrowDown') {
            e.preventDefault();
            if (formTab()) { var a = document.activeElement, g = $('grdWrap'), x = $('expWrap'), b = $('bagWrap'); (g.contains(a) ? x : x.contains(a) ? b : g).focus(); }
            else $('histWrap').focus();
        }
        if (ctrl && e.altKey && (e.key === 'Control' || e.key === 'Alt')) shortcutKeys();
        if (ctrl && k === 'c' && formTab() && !window.getSelection().toString()) $('txtCommRate').focus();
    });

    // ------------------------------------------------------------------ wiring
    function bind(id, ev, fn) { $(id).addEventListener(ev, fn); }
    document.addEventListener('DOMContentLoaded', function () {
        bind('btnNew', 'click', function (e) { run(e.currentTarget, reset); });
        bind('btnRefresh', 'click', function (e) { run(e.currentTarget, async function () { applyLookups(await api(API + '/refresh')); currencyDefaults(); }); });
        bind('btnSave', 'click', function (e) { var b = e.currentTarget; run(b, function () { S.recId = 0; return insert(b); }); });
        bind('btnUpdate', 'click', function (e) { var b = e.currentTarget; run(b, function () { return update(b); }); });
        bind('btnDelete', 'click', function (e) { run(e.currentTarget, remove); });
        bind('btnAttachment', 'click', function (e) { attachmentsDialog(e.currentTarget); });
        bind('btnLoadGrn', 'click', function (e) { run(e.currentTarget, openLoader); });
        bind('btnPrint103', 'click', function (e) { print103(S.voucherHeadId, e.currentTarget); });
        bind('btn220A', 'click', function (e) { if (S.recId <= 0) { msg('No Record Found For Display'); return; } print220A(S.recId, e.currentTarget); });
        bind('btn220B', 'click', function (e) { printSlip('220B-InvRptPurchaseBillSupplierRiceSummary.rpt', S.recId, e.currentTarget); });
        bind('btnFooterHistory', 'click', function () { showTab(formTab() ? 'history' : 'form'); if (formTab()) $('dtDocDate').focus(); });
        document.addEventListener('click', function (e) { var a = e.target.closest('a[data-doclink]'); if (!a) return; e.preventDefault(); if (window.DocLink) window.DocLink.open(num(a.dataset.doclink), num(a.dataset.docid), { message: msg }); });
        /* KeyPress: OnlytextdecimelFunction on Item Rate, Exchange Rate, Fcy Amount and Freight Amount (Designer :4404, :4421, :5009, :5158) */
        ['txtHeaderItemRate', 'txtExchangeRate', 'txtFcyAmount', 'txtFreightAmount'].forEach(function (id) { bind(id, 'keydown', function (e) { if (e.key.length === 1 && !e.ctrlKey && !e.altKey && !/[\d.]/.test(e.key)) e.preventDefault(); }); });
        /* btnshortcutkeys has no Click handler on the desktop; the list opens with Ctrl+Alt. */
        bind('tabBtnForm', 'click', function () { showTab('form'); });
        bind('tabBtnHistory', 'click', function () { showTab('history'); });
        bind('btnShow', 'click', function (e) { run(e.currentTarget, showHistory); });
        bind('btnHistReset', 'click', resetHistory);
        /* btnRefreshHistory has no Click handler on the desktop (BtnRefreshHistory_Click :3324 is not attached). */
        bind('keysClose', 'click', function () { $('keysDialog').hidden = true; });
        bind('ldrClose', 'click', function () { $('grnLoader').hidden = true; });
        bind('ldrShow', 'click', function (e) { run(e.currentTarget, showGrns); });
        bind('ldrLoad', 'click', function (e) { run(e.currentTarget, loadOnInvoice); });
        /* frmLoadGRN.btnReset_Click: From Date back to the financial-year start, both grids cleared (no search). */
        bind('ldrReset', 'click', function () { setVal('ldrFrom', S.finYearStart || today()); $('ldrBody').innerHTML = ''; $('ldrDetailHead').innerHTML = ''; $('ldrDetailBody').innerHTML = ''; S.loaderRows = []; $('ldrFrom').focus(); });
        bind('ldrBody', 'click', function (e) { var tr = e.target.closest('tr'); if (tr && !e.target.classList.contains('ldr-check')) run(null, function () { return loaderDetail(num(tr.dataset.row)); }); });

        bind('cmbSupplier', 'change', function () { billAmount(); });
        bind('cmbCommAgent', 'blur', function () { totalCommission(); billAmount(); commissionProportion(); });
        bind('cmbCommAgent', 'change', function () { totalCommission(); billAmount(); commissionProportion(); });
        bind('txtCommRate', 'input', function () { totalCommission(); billAmount(); });
        bind('txtBrokeryRate', 'input', function () { totalBrokery(); brokeryProportion(); });
        bind('cmbBroker', 'change', function () { totalBrokery(); brokeryProportion(); });
        bind('txtFreightAmount', 'input', function () { freightProportion(); billAmount(); });
        bind('cmbFreightAc', 'change', function () { freightProportion(); billAmount(); });
        bind('txtDueDays', 'input', dueDate);
        bind('dtDocDate', 'change', dueDate);
        bind('cmbPaymentTerm', 'change', paymentTermChanged);
        bind('txtHeaderItemRate', 'input', headerRateChanged);
        bind('txtExchangeRate', 'input', exchangeChanged);
        bind('txtExchangeRate', 'blur', function () { if ($('txtExchangeRate').value !== '') $('txtExchangeRate').value = String(num($('txtExchangeRate').value)); });
        bind('chkFcyEnable', 'change', function () { $('txtFcyAmount').disabled = !$('chkFcyEnable').checked; });
        bind('txtFcyAmount', 'input', function () { fcyChanged(true); });
        /* cmbCurrency_Leave :544 */
        bind('cmbCurrency', 'change', function (e) {
            run(null, async function () {
                var c = num($('cmbCurrency').value);
                if (c === 0 || num($('txtExchangeRate').value) !== 0) return;
                if (c !== S.baseCurrency) { var d = await api(API + '/last-exchange-rate?currencyId=' + c); $('txtExchangeRate').value = d.found ? String(d.LastExchRate) : '0'; }
                else $('txtExchangeRate').value = String(S.baseRate);
                exchangeChanged();
            });
        });
        bind('grdBody', 'change', function (e) {
            var t = e.target; if (!t.classList.contains('gd-edit')) return;
            var r = S.rows[num(t.dataset.row)];
            if (isNaN(parseFloat(t.value))) { msg('Please Type Only Numeric Value'); renderGrid(); return; }
            r[t.dataset.key] = num(t.value); cellUpdated(r, t.dataset.key);
        });
        bind('expBody', 'change', function (e) {
            var t = e.target; if (!t.classList.contains('exp-edit')) return;
            var r = S.expenses[num(t.dataset.row)];
            r[t.dataset.key] = t.dataset.key === 'Amount' ? num(t.value) : t.value;
            renderExpenses(); billAmount(); expProportion(); /* grdInvExp_CellUpdated :885 */
        });
        bind('bagBody', 'change', function (e) {
            var t = e.target; if (!t.classList.contains('bag-edit')) return;
            var r = S.bags[num(t.dataset.row)], key = t.dataset.key;
            if (key === 'Rate') {
                if (isNaN(parseFloat(t.value))) { msg('Please Type Only Numeric Value'); renderBags(); return; } /* grdEmptyBags_UpdatingCell :808 */
                r.Rate = num(t.value); r.Amount = round(num(r.PurchaseQty) * r.Rate, S.cfg.amountDigits || 0); /* :792-797 */
                renderBags();
            } else r[key] = key === 'CreditAccountId' ? num(t.value) : t.value;
            billAmount();
        });
        bind('histBody', 'click', function (e) {
            var b = e.target.closest('[data-act]');
            if (b) { e.preventDefault(); var btn = b.tagName === 'BUTTON' ? b : null; run(/^(Voucher|View|PartySlip|SummaryReport)$/.test(b.dataset.act) ? null : btn, function () { return historyAction(b.dataset.act, num(b.dataset.row), btn); }); return; }
            var tr = e.target.closest('tr'); if (tr) run(null, function () { return historyDetail(num(tr.dataset.row)); });
        });
        /* grdHistory_DoubleClick :2114 */
        bind('histBody', 'dblclick', function (e) { var tr = e.target.closest('tr'); if (tr && !e.target.closest('[data-act]')) run(null, function () { return readById(S.history[num(tr.dataset.row)].Id); }); });
        /* grdHistory_KeyDown :3355 - Ctrl+Enter edits when the user has the Update right */
        bind('histBody', 'keydown', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            if (e.ctrlKey && e.key === 'Enter' && S.rights.Update) { e.preventDefault(); run(null, async function () { await reset(); await readById(S.history[num(tr.dataset.row)].Id); }); }
        });
        run(null, init);
    });
})();
