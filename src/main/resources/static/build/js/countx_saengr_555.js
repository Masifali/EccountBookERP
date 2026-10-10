/* ============================================================================================
 * Screen 555 frmGDNHistory_Engr - Architecture.WinApp.Inventory_Reports.frmGDNHistory_Engr (.cs, 2478 lines)
 * Page: templates/sale/reports/saengr_gdn_history.html (route /sale/reports/engr/gdn-history).
 *   frmGDNHistory_Load :153      ParameterFill, ComboFill, (not requested by another document) cmbperemeter.Rows[1].Activate() = "This Week", focus the date type
 *   ParameterFill :179           CommonServices.DateType, BindDDL(ZeroIndex false) -> 1 This Day .. 5 Financial Year
 *   cmbperemeter_ValueChanged :199  1 From = today | 2 today - 7 | 3 first of (UTC) month, To = today | 4 1 Jan, To = today | 5 Start_Period, To = today
 *   ComboFill :237               InvGdn.GetDataForDropDownFromGdn(DocumentTypeIds "1612") = USP_GetDataForDropDownFromGdn, split by Activity: ParentCategories, ItemCategories,
 *                                ItemTypes, Item, ItemClassGroup, Warehouse, JobLot, Supplier, RequestedBy, DeliveryType (text ReferenceName), ZeroIndex false
 *   btnshow_Click :351 / gridHisory :363   InvGdn.GdnRegister_Eng = USP_GdnRegister_Eng (DocumentTypeId 1612; Reffered -> 1, Not Reffered -> 2); the rows are copied into a 40 column
 *                                table; no rows -> the grid is cleared
 *   grdSetting :462              Id, GpId, DocumentTypeId, DeliveryTypeId, SaleOrderId, WarehouseId, ItemId, ItemUomId, JobLotId, AssetId hidden; NoOfAttachments = link "Attached";
 *                                FactoryWeight / ItemQty / PartyWeight "#,##0.##", Freight / NetPaid stringFormatsingle (all summed, right); GridAutoAdjustmentNew;
 *                                Print button (50) at position 0, Attached at position 1, FrozenColumns 2
 *   DataGridHistory_ColumnButtonClick :516  Print -> CommonServices.InvGdnSlip1612(Id) (1612-InvGdn_Slip.rpt)
 *   DataGridHistory_LinkClicked :533  Attached -> GetNoofAttachmentsByRefDocumentTypeID(Id, 1612)
 *   Reset :550 (btnNew)          customer row 0, doc / order numbers cleared, parent category / class / type / item / warehouse / joblot / delivery type / requested by text cleared
 *                                (item category stays), All radio, focus the date type, ParameterFill
 *   toolStripButton1_Click :577  ComboFill (Refresh)        btnRegister_Click :751  no rows -> "Record Not Found For Display"; 1612-GdnRegister_Eng.rpt
 *   KeyDown :640                 Ctrl+E / Esc close, N, R, S, Down grid, Up / F5 date type, P and Alt+1 print, Ctrl+Alt shortcut keys;   MakeShortCutKeys :696
 *   btnshow_Leave :625           focus on the grid -> back to the date type;  txt*_KeyPress :773..809 digits only
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, E = w.SaEngr, el = A.el;
    var API = '/api/sale/saengr/gdn-history';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var lastArgs = null, hasRows = false, seq = 0;
    var SHORTCUTS = [['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+alt', 'To Show ShortCut Date Type'], ['Ctrl+ArrowDown', 'For Focus On Grid']];

    // ------------------------------------------------------------------ lists
    function parameterFill() { S.fill(el('cmbperemeter'), S.dateTypeRows(false), false); }
    function bindCombos(data) {
        var c = (data && data.combos) || {};
        function rows(k) { return c[k] || []; }
        if (!Object.keys(c).length) return;                                      // ComboFill returns when the lookup procedure gives no rows
        S.fill(el('cmbParentCategory'), rows('ParentCategories'), false);
        S.fill(el('cmbCategory'), rows('ItemCategories'), false);
        S.fill(el('cmbItemtype'), rows('ItemTypes'), false);
        S.fill(el('cmbItem'), rows('Item'), false);
        S.fill(el('cmbitemClass'), rows('ItemClassGroup'), false);
        S.fill(el('cmbware'), rows('Warehouse'), false);
        S.fill(el('cmbjoblot'), rows('JobLot'), false);
        S.fill(el('CmbSupplier'), rows('Supplier'), false);
        S.fill(el('CmbDeliveryType'), rows('DeliveryType'), false);
        S.fill(el('CmbRequestedByHistory'), rows('RequestedBy'), false);
    }
    function comboFill() { return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; bindCombos(data); }); }
    el('cmbperemeter').addEventListener('change', function () {              // cmbperemeter_ValueChanged
        S.dateRule(A.toInt(this.value), el('datGdnFromDate'), el('dateGdnToDate'), lookup.yearStart, { to5: true });
    });

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, frozen: 2, autosize: true,
        onLink: function (row, col) { if (col && col.key === 'NoOfAttachments') E.attachments(API, A.toInt(row.Id), 1612).catch(function (e) { alert(e.message); }); },
        onButton: function (row) {                                           // InvGdnSlip1612(Id)
            if (typeof w.printRpt !== 'function') { alert('Printing is not available on this page.'); return; }
            w.printRpt('1612-InvGdn_Slip.rpt', { id: A.toInt(row.Id) });
        }
    });
    function cols() {
        var single = S.fmtSingle(lookup.amountDecimals);
        return [
            { key: '_Print', caption: 'Print', width: 50, button: 'Print' },
            { key: 'NoOfAttachments', caption: 'Attached', link: true },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true }, { key: 'DeliveryTypeId', caption: 'DeliveryTypeId', hidden: true },
            { key: 'DeliveryType', caption: 'DeliveryType' }, { key: 'DocDate', caption: 'DocDate', date: 'dd/MM/yyyy' }, { key: 'DocNo', caption: 'DocNo' },
            { key: 'GpId', caption: 'GpId', hidden: true }, { key: 'DeliveryTerm', caption: 'DeliveryTerm' }, { key: 'CustomerName', caption: 'CustomerName' },
            { key: 'RequestedBy', caption: 'RequestedBy' }, { key: 'ManualNo', caption: 'ManualNo' }, { key: 'SaleOrderId', caption: 'SaleOrderId', hidden: true },
            { key: 'OrderNo', caption: 'OrderNo' }, { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'ItemUomId', caption: 'ItemUomId', hidden: true }, { key: 'PackUom', caption: 'PackUom' },
            { key: 'ItemQty', caption: 'ItemQty', fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true },
            { key: 'Specification/Remarks', caption: 'Specification/Remarks' }, { key: 'WarehouseId', caption: 'WarehouseId', hidden: true }, { key: 'WareHouseName', caption: 'WareHouseName' },
            { key: 'AssetId', caption: 'AssetId', hidden: true }, { key: 'Machine/Product', caption: 'Machine/Product' }, { key: 'GdnStatus', caption: 'GdnStatus' },
            { key: 'GpNo', caption: 'GpNo' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' },
            { key: 'FactoryWeight', caption: 'FactoryWeight', fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true },
            { key: 'PartyWeight', caption: 'PartyWeight', fmt: '#,##0.##', totalFmt: '#,##0.##', sum: true, num: true },
            { key: 'AccountTitle', caption: 'AccountTitle' },
            { key: 'Freight', caption: 'Freight', fmt: single, totalFmt: single, sum: true, num: true },
            { key: 'NetPaid', caption: 'NetPaid', fmt: single, totalFmt: single, sum: true, num: true },
            { key: 'RemarksHeader', caption: 'RemarksHeader' }, { key: 'JobLotId', caption: 'JobLotId', hidden: true }, { key: 'JobLotDescription', caption: 'JobLotDescription' },
            { key: 'EntryDate', caption: 'EntryDate', date: 'dd/MM/yyyy hh:mm:ss tt' }, { key: 'UserName', caption: 'UserName' },
            { key: 'ReturnableDate', caption: 'ReturnableDate', date: 'dd/MM/yyyy' }
        ];
    }

    // ------------------------------------------------------------------ Show / print
    function radio(name) { var x = d.querySelector('input[name="' + name + '"]:checked'); return x ? x.value : ''; }
    function args() {
        return { fromDate: el('datGdnFromDate').value, toDate: el('dateGdnToDate').value,
                 fromDocNo: A.toInt(el('txtGdnNoFrom').value), toDocNo: A.toInt(el('txtGdnNoTo').value),
                 orderNoFrom: A.toInt(el('txtOrderNoFrom').value), orderNoTo: A.toInt(el('txtOrderNoTo').value),
                 parentCategoryId: S.selInt('cmbParentCategory'), categoryId: S.selInt('cmbCategory'), itemTypeId: S.selInt('cmbItemtype'),
                 itemClassId: S.selInt('cmbitemClass'), itemId: S.selInt('cmbItem'), warehouseId: S.selInt('cmbware'), jobLotId: S.selInt('cmbjoblot'),
                 supplierCustomerId: S.selInt('CmbSupplier'), deliveryTypeId: S.selInt('CmbDeliveryType'), requestedById: S.selInt('CmbRequestedByHistory'),
                 referred: radio('rdRef') };
    }
    function show() {                                                        // btnshow_Click -> gridHisory
        var b = el('show'); if (b.disabled) return;
        var a = args(), token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }                           // DataGridHistory.DataSource = null
            grid.setData(cols(), rows);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print() {                                                       // btnRegister_Click
        var b = el('print'); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/saengr/print/gdn-history?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                                       // Reset
        S.activate('CmbSupplier', 0, false);                                 // CmbSupplier.Rows[0].Activate() (the first customer row, as the desktop)
        ['txtGdnNoFrom', 'txtGdnNoTo', 'txtOrderNoFrom', 'txtOrderNoTo'].forEach(function (id) { el(id).value = ''; });
        ['cmbParentCategory', 'cmbitemClass', 'cmbItemtype', 'cmbItem', 'cmbware', 'cmbjoblot', 'CmbDeliveryType', 'CmbRequestedByHistory'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        d.querySelector('input[name="rdRef"][value="0"]').checked = true;
        S.focus('cmbperemeter');
        parameterFill();
    }
    function refresh() {                                                     // toolStripButton1_Click -> ComboFill
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        comboFill().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    ['txtGdnNoFrom', 'txtGdnNoTo', 'txtOrderNoFrom', 'txtOrderNoTo'].forEach(E.digits);
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbperemeter'); });   // btnshow_Leave
    S.keys({ s: show, e: closeForm, r: refresh, n: reset, p: print, F5: function () { S.focus('cmbperemeter'); }, ArrowUp: function () { S.focus('cmbperemeter'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);
    d.addEventListener('keydown', function (e) {                             // Alt+1 / Alt+NumPad1 -> btnRegister_Click
        if (e.altKey && !e.ctrlKey && (e.key === '1' || e.code === 'Numpad1')) { e.preventDefault(); print(); }
    });

    // ------------------------------------------------------------------ start (Load)
    el('datGdnFromDate').value = A.today(); el('dateGdnToDate').value = A.today();
    grid.render();
    parameterFill();                                                         // ParameterFill
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        bindCombos(data);                                                    // ComboFill
        S.activate('cmbperemeter', 1, false);                                // cmbperemeter.Rows[1].Activate() = This Week
        S.focus('cmbperemeter');
    }).catch(function (e) { alert(e.message); });
}(window, document));
