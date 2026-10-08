/* ============================================================================================
 * Screen 908 SaleInvoiceStockRateUpdate - Architecture.WinApp.FIFO.SaleInvoiceStockRateUpdate ("Sale & Export CGS Rate Update", .cs)
 * Page: templates/sale/reports/sarpt1_sale_invoice_stock_rate_update.html (route /sale/reports/sale-invoice-stock-rate-update).
 *   Load                    both dates today, grids empty, Sale Invoice and Un Settled checked
 *   btnshow_Click           usp_getSaleDataForCgsUpdate (Sale) / usp_getEXportDataForCgsUpdate (Export) @FromDate @ToDate [@ActionId 1 when Settled]; Table0 = invoices (grd),
 *                           Table1 = details (grdDetail, the rows whose InvSaleInvoiceId (sale) / ExportVoucherId (export) is the current invoice); the Settled panel is disabled after Show
 *   GridSetting grd         Select check column (30, position 0, header selector), Id / DocumentTypeId hidden, ManualBillNo + BillAmount only for Sale, FrozenColumns 1, HeaderLines 2,
 *                           GridEX_Helper formats (Amount / Freight columns single format with a sum, Rate columns rate format, the rest "#,##0.##" with a sum)
 *   GridSetting grdDetail   Id / MainId hidden, Freight / Expense / Commission / NetAmount only for Sale (0 for Export)
 *   btnInsertVoucherAndStock_Click (Update)
 *                           confirm "Are you sure to update stock values?"; no checked row -> "Check the row first"; Un Settled and a detail JobStatus = UnSettled ->
 *                           "Some job order settlement is pending please check..."; InvSaleInvoice.StockRateUpdateFromJobOrder -> usp_StockRateUpdateFromJobOrder per invoice;
 *                           "Stocks rate have been successfully updated." / "An error occurred: ..."; the list is read again
 *   New                     clears both grids, enables the Settled panel, focus on the From date
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/sale-invoice-stock-rate-update';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var details = [], isSale = true, busy = false;
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+U', 'For Update'], ['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On From Date']];

    function radio(name) { var x = d.querySelector('input[name="' + name + '"]:checked'); return x ? x.value : ''; }
    function single() { return S.fmtSingle(lookup.amountDecimals); }
    function rate() { return S.fmtRate(lookup.rateDecimals); }

    var grd = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, frozen: 1, onSelect: showDetail });
    var grdDetail = new S.Grid({ tableId: 'resultsDetail', gridId: 'gridDetail', navId: 'navDetail', navTextId: 'navTextDetail', headerLines: 2 });

    function mainCols() {
        var sale = isSale, f = single();
        return [
            { key: '_Select', caption: 'Select', width: 30, check: true },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocumentType', caption: 'DocumentType', width: 130 }, { key: 'DocNo', caption: 'DocNo', width: 60 },
            { key: 'DocDate', caption: 'DocDate', width: 73, date: 'dd-MMM-yy' }, { key: 'CustomerName', caption: 'CustomerName', width: 170 },
            { key: 'ManualBillNo', caption: 'ManualBillNo', width: 60, hidden: !sale },
            { key: 'Qty', caption: 'Qty', width: 70, fmt: '#,##0.##', sum: true, num: true },
            { key: 'Weight', caption: 'Weight', width: 80, fmt: '#,##0.##', sum: true, num: true },
            { key: 'ItemAmount', caption: 'ItemAmount', width: 90, fmt: f, sum: true, num: true },
            { key: 'FcyCode', caption: 'FcyCode', width: 60 },
            { key: 'ExchangeRate', caption: 'ExchangeRate', width: 70, fmt: rate(), num: true },
            { key: 'FcyAmount', caption: 'FcyAmount', width: 90, fmt: f, sum: true, num: true },
            { key: 'BillAmount', caption: 'BillAmount', width: 90, fmt: f, sum: true, num: true, hidden: !sale },
            { key: 'ApprovedStatus', caption: 'ApprovedStatus', width: 85 }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 130 },
            { key: 'EntryDate', caption: 'EntryDate', date: 'dd-MMM-yy hh:mm tt', width: 135 }, { key: 'EntryUser', caption: 'EntryUser', width: 80 }
        ];
    }
    function detailCols() {
        var f = single(), r = rate(), sale = isSale;
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'MainId', caption: 'MainId', hidden: true },
            { key: 'ItemName', caption: 'ItemName', width: 130 }, { key: 'PackUom', caption: 'PackUom', width: 60 }, { key: 'JobLot', caption: 'JobLot', width: 80 },
            { key: 'WareHouse', caption: 'WareHouse', width: 130 },
            { key: 'ItemQty', caption: 'ItemQty', width: 70, fmt: '#,##0.##', sum: true, num: true },
            { key: 'BillWeight', caption: 'BillWeight', width: 80, fmt: '#,##0.##', sum: true, num: true },
            { key: 'StockWeight', caption: 'StockWeight', width: 80, fmt: '#,##0.##', sum: true, num: true },
            { key: 'ItemRate', caption: 'ItemRate', width: 70, fmt: r, num: true }, { key: 'RateUom', caption: 'RateUom', width: 60 },
            { key: 'Amount', caption: 'Amount', width: 90, fmt: f, sum: true, num: true },
            { key: 'CgsRate', caption: 'CgsRate', width: 70, fmt: r, num: true }, { key: 'CgsRatePerKg', caption: 'CgsRatePerKg', width: 70, fmt: r, num: true },
            { key: 'CgsAmount', caption: 'CgsAmount', width: 90, fmt: f, sum: true, num: true },
            { key: 'Freight', caption: 'Freight', width: 90, fmt: f, sum: true, num: true, hidden: !sale },
            { key: 'Expense', caption: 'Expense', width: 90, fmt: '#,##0.##', sum: true, num: true, hidden: !sale },
            { key: 'Commission', caption: 'Commission', width: 130, fmt: '#,##0.##', sum: true, num: true, hidden: !sale },
            { key: 'NetAmount', caption: 'NetAmount', width: 90, fmt: f, sum: true, num: true, hidden: !sale },
            { key: 'JobStatus', caption: 'JobStatus', width: 75 }
        ];
    }
    function mapMain(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), DocumentTypeId: c(r, 'DocumentTypeId'), DocumentType: c(r, 'DocumentTypeDescription'), DocNo: c(r, 'DocNo'), DocDate: c(r, 'DocDate'),
            CustomerName: c(r, 'CustomerName'), ManualBillNo: isSale ? c(r, 'ManualBillNo') : '', Qty: c(r, 'Qty'), Weight: c(r, 'Weight'), ItemAmount: c(r, 'Amount'),
            FcyCode: c(r, 'FcyCode'), ExchangeRate: c(r, 'ExchangeRate'), FcyAmount: c(r, 'FcyAmount'), BillAmount: isSale ? c(r, 'BillAmount') : 0,
            ApprovedStatus: c(r, 'ApprovedStatus'), RemarksHeader: c(r, 'RemarksHeader'), EntryDate: c(r, 'EntryDate'), EntryUser: c(r, 'EntryUser'), _chk: false };
    }
    function mapDetail(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), MainId: isSale ? c(r, 'InvSaleInvoiceId') : c(r, 'ExportVoucherId'), ItemName: c(r, 'ItemName'), PackUom: c(r, 'PackUom'),
            JobLot: c(r, 'JobLotCode'), WareHouse: c(r, 'WareHouseName'), ItemQty: c(r, 'ItemQty'), BillWeight: c(r, 'NetBillWeight'), StockWeight: c(r, 'NetStockWeight'),
            ItemRate: c(r, 'ItemRate'), RateUom: c(r, 'RateUom'), Amount: c(r, 'ItemAmount'), CgsRate: c(r, 'CgsRate'), CgsRatePerKg: c(r, 'CgsRatePerKg'),
            CgsAmount: c(r, 'CgsAmount'), Freight: isSale ? c(r, 'FreightAmount') : 0, Expense: isSale ? c(r, 'ExpenseAmount') : 0,
            Commission: isSale ? c(r, 'CommissionAmount') : 0, NetAmount: isSale ? c(r, 'ItemNetAmount') : 0, JobStatus: c(r, 'JobStatus') };
    }
    function showDetail(row) {                                                // the details of the current invoice
        var id = row ? A.toInt(row.Id) : -1;
        var rows = details.filter(function (x) { return A.toInt(x.MainId) === id; });
        if (!rows.length) grdDetail.clear(); else grdDetail.setData(detailCols(), rows);
    }

    function show() {
        var b = el('show'); if (b.disabled || busy) return;
        isSale = radio('rdKind') !== 'export';
        var settled = radio('rdSet') === '1';
        A.busy(b, true); busy = true;
        return A.getJson(API + '/data?' + S.qs({ sale: isSale, settled: settled, fromDate: el('datFromDate').value, toDate: el('datToDate').value })).then(function (data) {
            var main = (data && data.main) || [];
            details = ((data && data.detail) || []).map(mapDetail);
            if (!main.length) { grd.clear(); grdDetail.clear(); }
            else {
                grd.setData(mainCols(), main.map(mapMain));
                grd.select(0);
            }
            el('pnlsettled').classList.add('dis');                              // pnlsettled.Enabled = false
            d.querySelectorAll('#pnlsettled input').forEach(function (x) { x.disabled = true; });
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); busy = false; });
    }
    function update() {
        var b = el('update'); if (b.disabled || busy) return;
        if (!w.confirm('Are you sure to update stock values?')) return;
        var checked = grd.checkedRows();
        if (!checked.length) { alert('Check the row first'); return; }
        var settled = radio('rdSet') === '1';
        if (!settled) {
            var ids = checked.map(function (r) { return A.toInt(r.Id); });
            var pending = details.some(function (x) { return ids.indexOf(A.toInt(x.MainId)) >= 0 && String(x.JobStatus) === 'UnSettled'; });
            if (pending) { alert('Some job order settlement is pending please check...'); return; }
        }
        A.busy(b, true); busy = true;
        S.post(API + '/update', { sale: isSale, settled: settled, fromDate: el('datFromDate').value, toDate: el('datToDate').value,
            ids: checked.map(function (r) { return A.toInt(r.Id); }) }).then(function (res) {
            alert((res && res.message) || 'Stocks rate have been successfully updated.');
        }).catch(function (e) { alert('An error occurred: ' + e.message); }).then(function () {
            A.busy(b, false); busy = false;
            reload();
        });
    }
    function reload() {                                                       // the desktop shows the list again after Update
        var b = el('show'); b.disabled = false; return show();
    }
    function reset() {
        grd.clear(); grdDetail.clear(); details = [];
        el('pnlsettled').classList.remove('dis');
        d.querySelectorAll('#pnlsettled input').forEach(function (x) { x.disabled = false; });
        S.focus('datFromDate');
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('show').addEventListener('click', show);
    el('update').addEventListener('click', update);
    el('reset').addEventListener('click', reset);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    S.keys({ s: show, u: update, n: reset, e: closeForm, ArrowDown: function () { grd.focus(); }, ArrowUp: function () { S.focus('datFromDate'); },
             alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    el('datFromDate').value = A.today(); el('datToDate').value = A.today();
    grd.render(); grdDetail.render();
    A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; })
        .catch(function (e) { alert(e.message); });
}(window, document));
