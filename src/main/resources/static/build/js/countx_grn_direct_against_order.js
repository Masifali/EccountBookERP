/* ============================================================================================
 * countx_grn_direct_against_order.js - GRN Direct Against Order (Architecture.WinApp.Purchase.GRNDirectAgainstOrder),
 * ScreenDefinition 133 "GRNDirectAgainstOrder", DocumentTypeId 169. Follows the form line by line:
 *   InvFrmGRN_Load :1582, reset :1432, ResetDetial :1491, Total :1273, GrossWeightCalculations :1237, TotalSupplierWeight :1415,
 *   StockWeightUpdateFromGridOnDeliveryTermChange :2428, CalculationPurchaseAgainstWeight :2785, grdEmptyBags_CellUpdated :2914,
 *   grd_DoubleClick_1 :2725 + cmbOrderNo_Leave :2680 + ItemNameFill :1064, btnUpdateDetail_Click :2134, BtnLoadOrder_Click :2499 /
 *   LoadInGridDetail :2515, Insert :359, ReadById :637, HistoryGridFill :1743, GrdHistory_ColumnButtonClick :1991, the 213 slip,
 *   InvFrmGRN_KeyDown :1826.
 * A TextBox's TextChanged fires on every change of its text, also when the code sets it; setText() below does the same, so the
 * chained recalculations run in the desktop's order. Exports window.GrnDirectAgainstOrder.
 * Built on countx_hrm.js (HRM) and countx_pp_ppb_common.js (PPB).
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/purchase/grn-direct-against-order';
    var P = {}; window.GrnDirectAgainstOrder = P;
    var S = {
        rights: {}, id: 0, updateIndex: 0, lists: {}, uoms: [], items: [],
        ebStatus: 0,                                                                             // EmptyBagsWeightCutStatus (a form field, never reset)
        orderRow: null                                                                           // cmbOrderNo's only row {OrderId, OrderNo}
    };

    function get(path, params) { return HRM.get(API + path, params || {}); }
    function post(path, body) { return HRM.post(API + path, body); }
    var n = HRM.num;
    function r2(v) { return PPB.net(PPB.round(v, 2)); }                                         // Math.Round(x, 2).ToString()
    function has(id) { return HRM.val(id) !== ''; }                                             // txt.Text != string.Empty
    function dbl(id) { return has(id) ? n(HRM.val(id).trim()) : 0; }                              // double.Parse when not empty
    function term() { return HRM.comboText('CmbDeliveryTerm'); }
    function isPonch(t) { return t === 'Ponch' || t === 'Ponch & PartyWeight' || t === 'Ponch & FactoryWeight'; }
    function isLoadParty(t) { return t === 'Load' || t === 'Load & PartyWeight' || t === 'Ponch & PartyWeight'; }

    // ------------------------------------------------------------------ TextChanged plumbing
    var onText = {};
    /** TextBox.Text = v: the text changes and, only when it really changed, TextChanged runs (as WinForms does). */
    function setText(id, v) {
        var s = v === null || v === undefined ? '' : String(v);
        if (HRM.val(id) === s) return;
        HRM.setVal(id, s);
        if (onText[id]) onText[id]();
    }
    function comboTextSet(id, t) {                                                               // UltraCombo.Text = t (no row when not listed)
        HRM.setCombo(id, 0);
        if (t !== '' && t !== null && t !== undefined) HRM.setComboText(id, String(t));
    }

    /** GenerateReport(PrintId) :2229 -> CommonServices.GrnSlipReport213(169, PrintId): the 213 slip. */
    function printRpt(rpt, args) {
        var w = window.open('', '_blank');
        try { if (w) w.document.write('<p style="font:13px Segoe UI,sans-serif;padding:16px">Preparing report...</p>'); } catch (e) { /* ignore */ }
        var h = { 'Content-Type': 'application/json', 'Accept': 'application/pdf, text/plain' };
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn && t.getAttribute('content') && hn.getAttribute('content')) h[hn.getAttribute('content')] = t.getAttribute('content');
        return fetch('/reports/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: h, body: JSON.stringify(args || {}) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) {
                    return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                }
                return r.text().then(function (x) { if (w) w.close(); HRM.box(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); HRM.fail(e); });
    }
    function generateReport(id) { return printRpt('213-GoodsReceiptsNotesAgainstOrderSlip.rpt', { id: id, documentTypeId: 169 }); }

    /** .NET "0,0": grouped integer, at least two digits, rounded away from zero. */
    function fmt00(v) {
        if (v === null || v === undefined || v === '') return '';
        var x = n(v), r = Math.round(Math.abs(x)), s = String(r);
        if (s.length < 2) s = ('0' + s).slice(-2);
        return (x < 0 && r !== 0 ? '-' : '') + s.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    /** A summed column with its FormatString / TotalFormatString ("#,#" -> 0 decimals in the total, "#,##.##" -> 2). */
    function fcol(k, pattern, d) { var c = PPB.numCol(k, k, pattern); c.decimals = d; return c; }
    function plain(k, cap) { return { key: k, caption: cap || k, align: 'right', render: function (v) { return HRM.esc(v === null || v === undefined || v === '' ? '' : PPB.net(v)); } }; }

    // ------------------------------------------------------------------ grids
    var grd = new HRM.Grid('grd', {                                                              // dtdetail (:1600) + grdSettings() :1520
        columns: [
            { key: 'OrderDetailId', hidden: true }, { key: 'OrderId', hidden: true },
            { key: 'OrderNo', caption: 'OrderNo', type: 'code', width: 60 },
            { key: 'ItemId', hidden: true }, { key: 'Item', caption: 'Item', width: 160 },
            { key: 'PackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType', width: 80 },
            { key: 'UOM', caption: 'PackUom', align: 'right', width: 60 }, { key: 'UOMId', hidden: true },
            { key: 'CropYear', caption: 'CropYear', width: 70 }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot', width: 90 },
            fcol('Qty', '#,#', 0), fcol('GrossWight', '#,##.##', 2), fcol('AddLesswt', '#,##.##', 2),
            plain('EBUnit'), plain('EBTotal'), plain('WtCutUnit'), plain('WtCutTotal'),
            fcol('NetWeight', '#,##.##', 2), fcol('StockWeight', '#,##.##', 2),
            { key: 'City', caption: 'City', width: 90 }, { key: 'WarehouseId', hidden: true }, { key: 'Warehouse', caption: 'Warehouse', width: 90 },
            { key: 'RemarksDetail', caption: 'RemarksDetail', width: 120 },
            { key: 'Delete', caption: 'X', width: 20, render: function () { return PPB.btnCell('delete', 'X'); } }
        ],
        totals: true, emptyText: '',
        onDouble: function (r, i) { grdDoubleClick(i); },                                        // grd.DoubleClick -> grd_DoubleClick_1
        onCode: function (r, i, c) {                                                            // the order code opens the purchase order (41)
            if (c && c.key === 'OrderNo' && window.DocLink && HRM.int(r.OrderId) > 0) window.DocLink.open(41, HRM.int(r.OrderId));
        }
    });
    /** grd.AllowDelete = True (Designer :3728): the selected row can be removed; the "X" column (grdSettings) removes it the same way. */
    function removeRow(i) { if (i >= 0) { grd.remove(i); } }
    PPB.onButton(grd, 'grd', function (r, act, i) { if (act === 'delete') removeRow(i); });

    var grdEmptyBags = new HRM.Grid('grdEmptyBags', {                                          // dtEmptyBag (:1627) + grdEmptyBagsSettings() :1697
        columns: [
            { key: 'OrderId', hidden: true },
            { key: 'Type', caption: 'EmptyBagsType', type: 'select', width: 80, options: function () { return opts(S.lists.emptyBagTypes, 'Type'); } },
            { key: 'ItemId', caption: 'Item Name', type: 'select', width: 250, options: function () { return opts(S.lists.emptyBagItems, 'ItemName'); } },
            { key: 'Condition', caption: 'Bags_Condition', type: 'select', width: 120, options: function () { return opts(S.lists.bagsConditions, 'Type'); } },
            { key: 'RecQty', caption: 'RecQty', type: 'edit-num', sum: true, decimals: 2 },
            { key: 'PurQty', caption: 'PurQty', type: 'edit-num', sum: true, decimals: 2 },
            { key: 'Remarks', caption: 'Remarks', type: 'edit' },
            { key: 'Add', caption: '+', width: 20, hidden: true,
              render: function () { return '<button type="button" class="win-btn-action ppb-cell-btn" disabled title="No handler on the desktop form (grdEmptyBags has no ColumnButtonClick)">+</button>'; } }
        ],
        totals: true, emptyText: '',
        onChange: function (r, k) { emptyBagsCellUpdated(r, k); }                                // grdEmptyBags.CellUpdated
    });
    function opts(rows, text) { return [[0, '']].concat((rows || []).map(function (x) { return [x.Id, x[text]]; })); }
    function blankBag() { return { OrderId: 0, Type: 0, ItemId: 0, Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' }; }   // AddRowInvEmptyBagsGrid :1724
    function rehead(g) {
        g.table.tHead.innerHTML = '<tr>' + g.columns.map(function (c) {
            var style = (c.hidden ? 'display:none;' : '') + (c.width ? 'min-width:' + c.width + 'px;width:' + c.width + 'px;' : '');
            return '<th data-key="' + HRM.esc(c.key) + '" style="' + style + '"' + (c.align ? ' class="hrm-al-' + c.align + '"' : '') + '>' +
                HRM.esc(c.caption === undefined ? c.key : c.caption) + '</th>';
        }).join('') + '</tr>';
        g.draw();
    }
    /** grdEmptyBagsSettings: the "+" column only when the grid holds exactly one row. */
    function emptyBagsSettings() { grdEmptyBags.columnOf('Add').hidden = grdEmptyBags.rows().length !== 1; rehead(grdEmptyBags); }

    /** grdEmptyBags_CellUpdated :2914 */
    function emptyBagsCellUpdated(r, col) {
        if (HRM.int(r.PurQty) < 0) { r.PurQty = 0; HRM.box('PurQty cannot be less than Zero'); }
        if (HRM.int(r.RecQty) < 0) { r.RecQty = 0; HRM.box('RecQty cannot be less than Zero'); }
        if (col === 'PurQty' || col === 'Type') calculationPurchaseAgainstWeight();
        if ((HRM.int(r.Type) === 4 || HRM.int(r.Type) === 5) && col === 'PurQty') { r.PurQty = 0; HRM.box('Retained or Returned Stock you cannot be purchase'); }
        if (HRM.int(r.Type) === 2 && col === 'RecQty') { r.RecQty = 0; HRM.box('You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight'); }
        grdEmptyBags.draw();
    }

    var GrdHistory = new HRM.Grid('GrdHistory', {                                               // HistoryGridSettings() :1779 (proc column order)
        columns: [
            { key: 'Detail', caption: 'Detail', width: 50, render: function () { return PPB.btnCell('detail', 'Detail'); } },
            { key: 'Edit', caption: 'Edit', width: 45, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Print', caption: 'Print', width: 40, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'RecordNo', hidden: true }, { key: 'Id', hidden: true }, { key: 'OrderId', hidden: true }, { key: 'OrderType', hidden: true },
            { key: 'OrderNo', hidden: true }, { key: 'DocDate', caption: 'DocDate', type: 'date', width: 85 },
            { key: 'InwardGatePassId', hidden: true }, { key: 'ReferencePartyId', hidden: true },
            { key: 'ScreenName', caption: 'ScreenName', width: 130 }, { key: 'DocNo', caption: 'DocNo', type: 'code', width: 60 },
            { key: 'DocumentTypeId', hidden: true }, { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 100 },
            { key: 'SupplierName', caption: 'SupplierName', width: 160 }, { key: 'GpNo', hidden: true },
            { key: 'VehicleNo', caption: 'VehicleNo', width: 80 }, { key: 'BiltyNo', caption: 'BiltyNo', width: 70 },
            { key: 'WagesId', hidden: true }, { key: 'WagesNo', hidden: true }, { key: 'AddWages', caption: 'AddWages', type: 'check', width: 60 },
            plain('FactoryWeight'), plain('PartyWeight'), { key: 'Transporter', caption: 'Transporter', width: 130 },
            { key: 'CarriageAmount', hidden: true }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 130 },
            { key: 'EntryDate', caption: 'EntryDate', type: 'datetime', width: 130 }, { key: 'EntryUser', caption: 'EntryUser', width: 100 },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime', width: 130 }, { key: 'ModifyUser', caption: 'ModifyUser', width: 100 },
            { key: 'PostDate', caption: 'PostDate', type: 'datetime', width: 130 }, { key: 'ApprovedUser', caption: 'ApprovedUser', width: 100 },
            { key: 'RequestedBy', caption: 'RequestedBy', width: 90 }, { key: 'WbTicketId', caption: 'WbTicketId', type: 'int', width: 60 },
            { key: 'TicketNo', caption: 'TicketNo', type: 'int', width: 60 }, { key: 'InvoiceNo', caption: 'InvoiceNo', type: 'int', width: 60 },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code', width: 80 },
            { key: 'AttachmentsCount', caption: 'AttachmentsCount', type: 'int', width: 80 }, { key: 'DetailIdsCount', hidden: true },
            { key: 'ScaleShortWeightApply', caption: 'ScaleShortWeightApply', type: 'check', width: 70 },
            { key: 'SupplierShortWeightApply', caption: 'SupplierShortWeightApply', type: 'check', width: 70 },
            plain('RateCut'), { key: 'RateCutRemarks', caption: 'RateCutRemarks', width: 110 }, plain('AccessWeight'),
            { key: 'RefDocumentTypeId', caption: 'RefDocumentTypeId', type: 'int', width: 70 },
            { key: 'BaseDocumentTypeId', caption: 'BaseDocumentTypeId', type: 'int', width: 70 },
            { key: 'BaseDocumentType', caption: 'BaseDocumentType', width: 110 }
        ],
        filterRow: true, emptyText: '',
        onDouble: function (r) { readById(HRM.int(r.Id)); },                                     // GrdHistory_DoubleClick :1948
        onCode: function (r, i, c) {
            if (c && c.key === 'DocNo') readById(HRM.int(r.Id));                                 // the document code loads the record (as Edit)
            else if (c && c.key === 'NoOfAttachments') HRM.box('Attachments are not ported to the web.');   // GrdHistory_LinkClicked :2276
        }
    });
    PPB.onButton(GrdHistory, 'GrdHistory', function (r, act) {                                 // GrdHistory_ColumnButtonClick :1991
        if (act === 'edit') readById(HRM.int(r.Id));
        else if (act === 'print') generateReport(HRM.int(r.Id));
        else if (act === 'detail') historyDetail(HRM.int(r.Id));
    });
    function h00(k) { return { key: k, caption: k, type: 'num', sum: true, decimals: 0, render: function (v) { return HRM.esc(fmt00(v)); } }; }
    var GrdHistoryDetail = new HRM.Grid('GrdHistoryDetail', {                                   // the "Detail" button table + gridhistorydetail() :2033
        columns: [
            { key: 'Item', caption: 'Item' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'UOM', caption: 'UOM' },
            h00('Qty'), h00('GrossWight'), h00('AddLesswt'), h00('NetWeight'), h00('StockWeight'),
            { key: 'City', caption: 'City' }, { key: 'RemarksDetail', caption: 'RemarksDetail' }
        ],
        filterRow: true, totals: true, emptyText: ''
    });

    // ------------------------------------------------------------------ lists
    function fillLists(o, keep) {
        Object.keys(o).forEach(function (k) { S.lists[k] = o[k]; });
        if (o.suppliers) HRM.fill('combsupplier', o.suppliers, 'Id', 'CompanyName', { zero: '', keep: keep });           // SupplierNameFilll :990
        if (o.transporters) HRM.fill('CmbTransport', o.transporters, 'Id', 'AccountTitle', { zero: '', keep: keep });    // TransportFill :1018
        if (o.vehicleTypes) HRM.fill('combvehtyp', o.vehicleTypes, 'Id', 'VehicleDescription', { zero: '', keep: keep }); // VehicleTypesFill :1048
        if (o.packingTypes) {                                                                     // PackingTypeFill :1094 - Rows[0].Activate()
            HRM.fill('combpcktyp', o.packingTypes, 'Id', 'PackTypeDesc', { zero: '', keep: false });
            if (o.packingTypes.length) HRM.setCombo('combpcktyp', o.packingTypes[0].Id);
        }
        if (o.cities) HRM.fill('CmbCity', o.cities, 'Id', 'CityName', { zero: '', keep: keep });                          // CityFill :1151
        if (o.items) itemFill(o.items);                                                           // ItemNameFill :1064
        if (o.warehouses) HRM.fill('CmbWareHouse', o.warehouses, 'Id', 'WareHouseName', { zero: '', keep: keep });       // WareHouseFill :1209
        if (o.jobLots) HRM.fill('CmbJobLot', o.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: keep });            // JobLotFill :1177
    }
    /** ItemNameFill :1064 - the order's items (OrderItemId / ItemName / PODetailId); Rows[0] active; CmbCity.Text = row 0's CityArea. */
    function itemFill(rows) {
        if (!rows || !rows.length) return false;                                                  // no row: the combo keeps its old list
        S.items = rows;
        HRM.fill('combitem', rows, 'OrderItemId', 'ItemName', { zero: false });
        comboTextSet('CmbCity', HRM.str(rows[0].CityArea));
        return true;
    }
    /** PackUOMFillWithoutOrder :1123 (combitem ValueChanged / Leave) - the item's UOM schedule, Equivalent shown. */
    function packUomFill(itemIdOverride) {
        var itemId = itemIdOverride !== undefined ? itemIdOverride : HRM.comboVal('combitem');
        var keep = HRM.val('combpckuom');
        return get('/uoms', { itemId: itemId }).then(function (rows) {
            if (!(rows || []).length) return;                                                     // Rows.Count > 0 only
            S.uoms = rows;
            HRM.fill('combpckuom', rows, 'Id', function (r) { return PPB.net(r.Equivalent); }, { zero: '' });
            if (keep && HRM.hasOption('combpckuom', keep)) HRM.setCombo('combpckuom', keep);
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ calculations
    /** GrossWeightCalculations :1237 */
    function grossWeightCalculations() {
        var f = dbl('txtfctwt'), s = dbl('txtsuppwt');
        if (s > 0 && f > 0) setText('txtgwt', PPB.net(isLoadParty(term()) ? s : f));
        else setText('txtgwt', '0');
    }
    /** Total() :1273 */
    function total() {
        var f = dbl('txtfctwt'), s = dbl('txtsuppwt');
        var qty = dbl('txtqty'), gross = dbl('txtgwt'), ebUnit = dbl('txtEBUnit');
        var ebTotal = 0, wtCutTotal = 0, purQty = 0, tpaw = 0;
        if (ebUnit > 0 && HRM.comboVal('cmbOrderNo') === 0) {                                   // cmbOrderNo.ActiveRow == null
            S.ebStatus = 0;
            var grdQty = grd.sum('Qty');
            grdEmptyBags.rows().forEach(function (e) { if (HRM.int(e.Type) === 2) purQty += n(e.PurQty); });
            if (purQty > 0) {
                var upd = HRM.visible('btnUpdateDetail') && grd.rows()[S.updateIndex] ? n(grd.rows()[S.updateIndex].Qty) : 0;
                S.ebStatus = 2;
                tpaw = (Math.abs(purQty) - (grdQty - upd)) * ebUnit;
            }
        }
        if (ebUnit > 0 && qty > 0) ebTotal = ebUnit * qty;
        setText('txtEbTotal', r2(ebTotal));
        if (has('txtWtCutUnit')) setText('txtWtCutTotal', PPB.net(qty * dbl('txtWtCutUnit')));
        else setText('txtWtCutTotal', '0');
        if (has('txtWtCutTotal')) wtCutTotal = dbl('txtWtCutTotal');
        var addLess = has('txtadlswt') ? n(HRM.val('txtadlswt').trim()) : 0;
        var weight;                                                                               // RefDocumentTypeId is never set (0, not 105)
        if (S.ebStatus === 2 && tpaw > 0) weight = gross - wtCutTotal - ebTotal - PPB.round(addLess, 0) + tpaw;
        else if (S.ebStatus === 2) weight = gross - wtCutTotal - PPB.round(addLess, 0);
        else weight = gross - ebTotal - wtCutTotal + addLess;
        HRM.setVal('txtnetwt', r2(weight));
        if (!(s > 0) || !(f > 0) || !(gross > 0)) return;
        var t = term();
        if (isLoadParty(t)) HRM.setVal('txtstockwt', r2(f * gross / s - ebTotal));
        if (t === 'Ponch') HRM.setVal('txtstockwt', r2(s > f ? f * gross / f - ebTotal : f * gross / s - ebTotal));
        if (t === 'Ponch & FactoryWeight' || t === 'Load & FactoryWeight') HRM.setVal('txtstockwt', r2(f * gross / f - ebTotal));
    }
    /** TotalSupplierWeight :1415 */
    function totalSupplierWeight() {
        if (HRM.val('txtsuppwt') !== '' && HRM.val('txtfctwt') !== '') HRM.setVal('txtwtdiff', PPB.net(n(HRM.val('txtsuppwt').trim()) - n(HRM.val('txtfctwt').trim())));
    }
    /** StockWeightUpdateFromGridOnDeliveryTermChange :2428 */
    function stockWeightUpdate() {
        if (!grd.rows().length) return;
        var s = n(HRM.val('txtsuppwt').trim()), f = n(HRM.val('txtfctwt').trim()), t = term();
        if (!(s > 0) || !(f > 0)) return;
        grd.rows().forEach(function (r) {
            var g = n(r.GrossWight), al = n(r.AddLesswt), eb = n(r.EBTotal), wc = n(r.WtCutTotal);
            r.StockWeight = g + al;
            if (isLoadParty(t) && s > 0 && f > 0 && g > 0) r.StockWeight = PPB.round(f * (g / s * 100) / 100 - eb - wc + al, 2);
            if (t === 'Ponch' && f > s && s > 0 && f > 0 && g > 0) r.StockWeight = PPB.round(f * (g / s * 100) / 100 - eb - wc + al, 2);
        });
        grd.draw();
    }
    /** CalculationPurchaseAgainstWeight :2785 - every detail row's EB total, Wt cut total, net weight and stock weight. */
    function calculationPurchaseAgainstWeight() {
        var f = dbl('txtfctwt'), s = dbl('txtsuppwt'), t = term();
        var allocate = 0, purQty = 0, ebLess = 0;
        grdEmptyBags.rows().forEach(function (e) { if (HRM.int(e.Type) === 2) purQty += n(e.PurQty); });
        grd.rows().forEach(function (r) {
            var tpaw = 0, qty = n(r.Qty), gross = n(r.GrossWight), ebUnit = n(r.EBUnit), wtCut = n(r.WtCutUnit), addLess = n(r.AddLesswt), ebTotal, wtCutTotal;
            if (ebUnit > 0) {
                var grdQty = qty;
                if (purQty > 0) {
                    purQty -= allocate;
                    if (purQty > grdQty) ebLess = grdQty; else if (grdQty > purQty) ebLess = purQty;
                    if (purQty === grdQty) ebLess = grdQty;
                    if (purQty > 0) tpaw = ebLess * ebUnit;
                }
            }
            if (ebUnit > 0 && qty > 0) { r.EBTotal = ebUnit * qty; ebTotal = n(r.EBTotal); } else { r.EBTotal = 0; ebTotal = 0; }
            if (wtCut > 0 && qty > 0) { r.WtCutTotal = qty * wtCut; wtCutTotal = n(r.WtCutTotal); } else { r.WtCutTotal = 0; wtCutTotal = 0; }
            var weight = gross - wtCutTotal - ebTotal + addLess + tpaw;
            r.NetWeight = PPB.round(weight, 2);
            if (s > 0 && f > 0 && gross > 0) {
                if (t === 'Ponch') r.StockWeight = PPB.round(s > f ? f * gross / f - ebTotal - wtCutTotal + addLess : f * gross / s - ebTotal - wtCutTotal + addLess, 2);
                else r.StockWeight = PPB.round(f * gross / s - ebTotal - wtCutTotal + addLess, 2);
            } else r.StockWeight = PPB.round(weight, 2);
            allocate = ebLess;
        });
        grd.draw();
    }
    /** CmbDeliveryTerm_TextChanged :2392 */
    function deliveryTermChanged() {
        if (isPonch(term())) {
            HRM.setCombo('CmbTransport', 0);
            setText('txtcarramount', '0');
            HRM.enable('txtcarramount', false); HRM.enable('CmbTransport', false);
        } else {
            HRM.enable('txtcarramount', true); HRM.enable('CmbTransport', true);
        }
        stockWeightUpdate();
    }
    onText.txtqty = total; onText.txtgwt = total; onText.txtEBUnit = total; onText.txtEbTotal = total;
    onText.txtWtCutUnit = total; onText.txtWtCutTotal = total; onText.txtadlswt = total;
    onText.txtsuppwt = function () {                                                             // txtsuppwt_TextChanged :2183
        setText('txtfctwt', HRM.val('txtsuppwt').trim());
        totalSupplierWeight(); stockWeightUpdate();
    };
    onText.txtfctwt = function () { totalSupplierWeight(); stockWeightUpdate(); };               // txtfctwt_TextChanged :2197

    // ------------------------------------------------------------------ detail
    /** FormValidationDetail :766 */
    function detailValid() {
        function no(msg, id) { HRM.box(msg); HRM.focus(id); return false; }
        if (HRM.comboVal('cmbOrderNo') === 0) return no('Order No Field is Required.... Please Load Any Purchase Order', 'cmbOrderNo');
        if (HRM.comboVal('combitem') === 0) return no('Item Name Field is Required', 'combitem');
        if (HRM.comboVal('combpcktyp') === 0) return no('Packing Type Field is Required', 'combpcktyp');
        if (HRM.comboVal('cmbCropYear') === 0) return no('Crop Year Field is Required', 'cmbCropYear');
        if (HRM.comboVal('CmbJobLot') === 0) return no('JobLot Field is Required', 'CmbJobLot');
        if (HRM.val('txtqty').trim() === '' || n(HRM.val('txtqty')) === 0) return no('Qty Field is Required', 'txtqty');
        if (HRM.comboVal('combpckuom') === 0) return no('Pack Unit Field is Required', 'combpckuom');
        if (HRM.val('txtgwt').trim() === '' || n(HRM.val('txtgwt')) === 0) return no('Gross Weight Field is Required', 'txtgwt');
        if (HRM.val('txtnetwt').trim() === '' || n(HRM.val('txtnetwt')) === 0) return no('Net Bill Weight Field is Required', 'txtnetwt');
        if (HRM.val('txtstockwt').trim() === '' || n(HRM.val('txtstockwt')) === 0) return no('Stock Weight Field is Required', 'txtstockwt');
        if (HRM.comboVal('CmbCity') === 0) return no('City Field is Required', 'CmbCity');
        if (HRM.comboVal('CmbWareHouse') === 0) return no('Warehouse Field is Required', 'CmbWareHouse');
        return true;
    }
    /** cmbOrderNo_Leave :2680 */
    function orderNoLeave() {
        HRM.enable('combitem', false); HRM.enable('cmbOrderNo', false);
        if (HRM.comboVal('cmbOrderNo') === 0) return Promise.resolve();
        var orderNo = HRM.int(HRM.comboText('cmbOrderNo'));                                      // ToInt(cmbOrderNo.Text) - the order's DocNo
        return get('/order-leave', { orderNo: orderNo }).then(function (o) {
            if (!o || !o.found) { HRM.box('Supplier Name Not Found'); return; }
            comboTextSet('CmbDeliveryTerm', HRM.str(o.DeliveryTerm)); deliveryTermChanged();
            comboTextSet('combsupplier', HRM.str(o.CompanyName));
            return get('/order-items', { orderId: HRM.comboVal('cmbOrderNo') }).then(function (rows) {   // ItemNameFill()
                var p = itemFill(rows || []) ? packUomFill() : Promise.resolve();                 // Rows[0].Activate() -> ValueChanged
                if (isPonch(term())) {
                    HRM.setCombo('CmbTransport', 0); setText('txtcarramount', '0');
                    HRM.enable('txtcarramount', false); HRM.enable('CmbTransport', false);
                }
                return p;
            });
        });
    }
    /** grd_DoubleClick_1 :2725 */
    function grdDoubleClick(i) {
        var r = grd.rows()[i]; if (!r) return;
        S.orderRow = { OrderId: r.OrderId, OrderNo: r.OrderNo };
        HRM.fill('cmbOrderNo', [S.orderRow], 'OrderId', 'OrderNo', { zero: false });           // BindDDL + Rows[0].Activate()
        S.updateIndex = i;
        HRM.setCombo('combitem', r.ItemId);
        var uomLoad = packUomFill(HRM.int(r.ItemId));                                           // combitem ValueChanged (UOMs of the row's item)
        HRM.setCombo('combpcktyp', r.PackingTypeId);
        if (HRM.comboVal('combpcktyp') === 0 && (S.lists.packingTypes || []).length) HRM.setCombo('combpcktyp', S.lists.packingTypes[0].Id);
        setText('txtqty', PPB.net(PPB.round(n(r.Qty), 0)));                                      // Math.Round(Qty)
        return uomLoad.then(function () {
            HRM.setCombo('combpckuom', r.UOMId);
            comboTextSet('cmbCropYear', HRM.str(r.CropYear));
            HRM.setCombo('CmbJobLot', r.JobLotId);
            setText('txtgwt', PPB.net(r.GrossWight)); setText('txtadlswt', PPB.net(r.AddLesswt));
            setText('txtEBUnit', PPB.net(r.EBUnit)); setText('txtEbTotal', PPB.net(r.EBTotal));
            setText('txtWtCutUnit', PPB.net(r.WtCutUnit)); setText('txtWtCutTotal', PPB.net(r.WtCutTotal));
            HRM.setVal('txtnetwt', PPB.net(r.NetWeight)); HRM.setVal('txtstockwt', PPB.net(r.StockWeight));
            comboTextSet('CmbCity', HRM.str(r.City));
            HRM.setCombo('CmbWareHouse', r.WarehouseId);
            if (HRM.comboVal('CmbWareHouse') === 0 && (S.lists.warehouses || []).length) HRM.setCombo('CmbWareHouse', S.lists.warehouses[0].Id);
            HRM.setVal('txtremarksdetail', HRM.str(r.RemarksDetail));
            HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
            HRM.focus('combpcktyp');
            return orderNoLeave();
        }).then(function () {
            if (HRM.visible('btnSave') && !HRM.$('btnSave').disabled) grossWeightCalculations();
        }).catch(HRM.fail);                                                                       // the form's try/catch: MessageBox(ex.Message)
    }
    P.btnUpdateDetail = function () {                                                           // btnUpdateDetail_Click :2134
        if (!detailValid()) return;
        if (n(HRM.val('txtnetwt').trim()) > n(HRM.val('txtgwt').trim())) { HRM.box('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        var r = grd.rows()[S.updateIndex]; if (!r) return;
        var it = PPB.rowOf(S.items, 'OrderItemId', HRM.comboVal('combitem')) || {};
        r.OrderDetailId = it.PODetailId;                                                          // combitem.SelectedRow.Cells[2]
        r.OrderId = HRM.comboVal('cmbOrderNo'); r.OrderNo = HRM.comboText('cmbOrderNo');
        r.ItemId = HRM.comboVal('combitem'); r.Item = HRM.comboText('combitem');
        r.PackingTypeId = HRM.comboVal('combpcktyp'); r.PackingType = HRM.comboText('combpcktyp');
        r.Qty = n(HRM.val('txtqty')); r.UOM = HRM.comboText('combpckuom'); r.UOMId = HRM.comboVal('combpckuom');
        r.CropYear = HRM.comboText('cmbCropYear'); r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = HRM.comboText('CmbJobLot');
        r.GrossWight = n(HRM.val('txtgwt')); r.AddLesswt = n(HRM.val('txtadlswt')); r.EBUnit = n(HRM.val('txtEBUnit'));
        r.EBTotal = n(HRM.val('txtEbTotal')); r.WtCutUnit = n(HRM.val('txtWtCutUnit')); r.WtCutTotal = n(HRM.val('txtWtCutTotal'));
        r.NetWeight = n(HRM.val('txtnetwt')); r.StockWeight = n(HRM.val('txtstockwt'));
        r.City = HRM.comboText('CmbCity'); r.WarehouseId = HRM.comboVal('CmbWareHouse'); r.Warehouse = HRM.comboText('CmbWareHouse');
        r.RemarksDetail = HRM.val('txtremarksdetail');
        HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        resetDetail();
        grd.draw();
    };
    P.btnCancelUpdateDetial = function () {                                                     // btnCancelUpdateDetial_Click :2258 (only hides the buttons)
        HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
    };
    /** ResetDetial :1491 */
    function resetDetail() {
        S.orderRow = null;
        HRM.fill('cmbOrderNo', [], 'OrderId', 'OrderNo', { zero: '' });
        S.items = []; HRM.fill('combitem', [], 'OrderItemId', 'ItemName', { zero: '' });
        ['txtqty', 'txtgwt', 'txtadlswt', 'txtnetwt', 'txtstockwt', 'txtEBUnit', 'txtEbTotal', 'txtWtCutUnit', 'txtWtCutTotal'].forEach(function (id) {
            if (onText[id]) setText(id, ''); else HRM.setVal(id, '');
        });
        HRM.setCombo('combpckuom', 0); HRM.setCombo('CmbJobLot', 0); HRM.setCombo('cmbCropYear', 0); HRM.setCombo('CmbWareHouse', 0);
        HRM.focus('CmbTransport');
    }

    // ------------------------------------------------------------------ load order
    /** BtnLoadOrder_Click :2499 - LoadPurchaseOrder (DocumentTypeId 41); its source is not in the corpus, so the list is the pending
     *  purchase orders (GetPurchaseOrderForPurchaseInvoice) and the ticked rows are its dtSupply (column 0 = order Id). */
    P.btnLoadOrder = function () {
        var m = HRM.modal({ title: 'Load Purchase Order', width: 'min(1150px, 96vw)',
            html: '<fieldset class="hrm-group"><legend>Filters</legend><div class="hrm-entry" style="grid-template-columns:repeat(auto-fill,minmax(170px,1fr))">' +
                '<div class="hrm-field hrm-stack"><label for="ldSupplier">Supplier</label><select id="ldSupplier" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                '<div class="hrm-field hrm-stack"><label for="ldFromDate">From Date</label><input type="date" id="ldFromDate" class="win-textbox"/></div>' +
                '<div class="hrm-field hrm-stack"><label for="ldToDate">To Date</label><input type="date" id="ldToDate" class="win-textbox"/></div>' +
                '<div class="hrm-field hrm-stack"><label for="ldFromDocNo">From Doc No</label><input type="text" id="ldFromDocNo" class="win-textbox" inputmode="numeric"/></div>' +
                '<div class="hrm-field hrm-stack"><label for="ldToDocNo">To Doc No</label><input type="text" id="ldToDocNo" class="win-textbox" inputmode="numeric"/></div>' +
                '<div class="hrm-field hrm-stack"><label>&nbsp;</label><button type="button" id="ldShow" class="win-btn-action">Show</button></div>' +
                '</div></fieldset>' +
                '<div class="hrm-grid-box"><div class="hrm-subcaption"><span>Pending Purchase Orders</span></div>' +
                '<div class="hrm-grid-wrap" style="max-height:360px"><table id="ldGrid"></table></div></div>' +
                '<div class="hrm-actions"><button type="button" id="ldLoad" class="win-btn-action">Load</button></div>'
        });
        HRM.fill('ldSupplier', S.lists.suppliers || [], 'Id', 'CompanyName', { zero: '' });
        PPB.guard('ldFromDocNo', 'int'); PPB.guard('ldToDocNo', 'int');
        var g = new HRM.Grid('ldGrid', {
            columns: [
                { key: 'Select', caption: 'Select', type: 'edit-check', width: 40 }, { key: 'Id', hidden: true },
                { key: 'DocNo', caption: 'DocNo', type: 'int', width: 60 }, { key: 'DocDate', caption: 'DocDate', type: 'date', width: 85 },
                { key: 'SupplierName', caption: 'SupplierName', width: 160 }, { key: 'ItemName', caption: 'ItemName', width: 150 },
                { key: 'ItemCode', caption: 'ItemCode', width: 70 }, PPB.numCol('ItemQty', 'ItemQty', '#,##0.##'),
                PPB.numCol('ItemWeight', 'ItemWeight', '#,##0.##'), { key: 'PackUom', caption: 'PackUom', width: 60 },
                { key: 'UOMCode', caption: 'Rate Uom', width: 60 }, plain('OrderItemRate'),
                { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 100 }, { key: 'JobLotDescription', caption: 'JobLot', width: 90 },
                { key: 'Crop', caption: 'Crop', width: 60 }
            ],
            checkAll: 'Select', filterRow: true, totals: true, emptyText: 'No record found.'
        });
        function show() {
            return HRM.busy('ldShow', function () {
                return get('/order-loader', { supplierId: HRM.comboVal('ldSupplier'), fromDate: HRM.val('ldFromDate'), toDate: HRM.val('ldToDate'),
                    fromDocNo: HRM.val('ldFromDocNo'), toDocNo: HRM.val('ldToDocNo') })
                    .then(function (rows) { g.set((rows || []).map(function (r) { var c = Object.assign({}, r); c.Select = false; return c; })); })
                    .catch(HRM.fail);
            });
        }
        HRM.$('ldShow').addEventListener('click', show);
        HRM.$('ldLoad').addEventListener('click', function () {
            var picked = g.checked('Select');
            if (!picked.length) { m.close(); return; }
            m.close();
            HRM.busy('BtnLoadOrder', function () { return loadInGridDetail(picked); });
        });
        show();
    };
    /** LoadInGridDetail :2515 -> LoadPurchaseOrderDataForGrn :2586 + GetEmptyBagsInformationFromOrder :2546 */
    function loadInGridDetail(picked) {
        var ids = picked.map(function (r) { return HRM.int(r.Id); });
        return post('/load-orders', { ids: ids }).then(function (o) {
            grd.clear();                                                                          // dtdetail.Rows.Clear()
            var rows = (o && o.rows) || [];
            if (rows.length) {
                HRM.setCombo('combsupplier', o.SupplierCustomerId);
                HRM.setVal('txtremarks', HRM.str(o.RemarksHeader));
                comboTextSet('CmbDeliveryTerm', HRM.str(o.DeliveryTerm)); deliveryTermChanged();
                HRM.enable('combsupplier', false); HRM.enable('CmbDeliveryTerm', false); HRM.enable('combitem', false);
                grd.set(rows.map(function (r) { return Object.assign({}, r); }));
            }
            var bags = (o && o.emptyBags) || [];
            if (bags.length) {
                var list = [], pt = HRM.comboVal('combpcktyp');
                bags.forEach(function (b) {
                    var t = HRM.int(b.Type);
                    if ((t === 1 || t === 2 || t === 3) && HRM.int(b.PackingTypeId) === pt) { setText('txtEBUnit', HRM.str(b.WeightCut)); S.ebStatus = t; }
                    list.push({ OrderId: b.PurchaseOrderId, Type: t, ItemId: HRM.int(b.ItemId), Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' });
                });
                grdEmptyBags.set(list); emptyBagsSettings();
                total();
            }
            grd.draw();
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ load / reset / read
    function buttons(update) { HRM.show('btnSave', !update); HRM.show('btnUpdate', update); }
    function load() {                                                                           // InvFrmGRN_Load :1582
        HRM.enable('combsupplier', false); HRM.enable('CmbDeliveryTerm', false);
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            HRM.applyRights({ save: S.rights.save, print: S.rights.print, update: S.rights.update },
                { save: 'btnSave', print: 'printToolStripButton', update: 'btnUpdate' });
            S.lists.emptyBagTypes = o.emptyBagTypes || []; S.lists.bagsConditions = o.bagsConditions || []; S.lists.emptyBagItems = o.emptyBagItems || [];
            grdEmptyBags.set([blankBag()]); emptyBagsSettings();
            HRM.fill('combbrnch', o.branches || [], 'Id', 'BranchName', { zero: false });           // BranchFill - Rows[0]
            HRM.fill('combproject', o.projects || [], 'Id', 'ProjectName', { zero: false });        // ProjectFill - Rows[0]
            HRM.setVal('txtdocno', o.docNo || 0);                                                // GenerateCode
            HRM.fill('cmbCropYear', o.cropYears || [], 'Id', 'CropYear', { zero: '' });          // CropYearfill
            HRM.fill('CmbDeliveryTerm', o.deliveryTerms || [], 'Id', 'Type', { zero: false });  // DeliveryTerm() - Rows[0] ("Load")
            deliveryTermChanged();
            HRM.fill('cmbOrderNo', [], 'OrderId', 'OrderNo', { zero: '' });
            HRM.fill('combitem', [], 'OrderItemId', 'ItemName', { zero: '' });
            HRM.fill('combpckuom', [], 'Id', 'Equivalent', { zero: '' });
            fillLists(o, false);
            buttons(false);
            HRM.check('ChkBox', true);
            HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
            HRM.focus('DocDate');
        }).catch(HRM.fail);
    }
    /** reset() :1432 (Id and EmptyBagsWeightCutStatus are not reset, as on the form) */
    function reset() {
        HRM.setCombo('combsupplier', 0); HRM.setCombo('CmbTransport', 0);
        setText('txtcarramount', ''); setText('txtsuppwt', ''); setText('txtfctwt', ''); HRM.setVal('txtwtdiff', '');
        var vt = S.lists.vehicleTypes || []; HRM.setCombo('combvehtyp', vt.length ? vt[0].Id : 0);   // Rows[0].Activate()
        HRM.setVal('txtvehno', ''); HRM.setVal('txtbltyno', ''); HRM.setVal('txtremarks', '');
        HRM.setCombo('combitem', 0);
        ['txtqty', 'txtadlswt', 'txtEBUnit', 'txtEbTotal', 'txtWtCutUnit', 'txtWtCutTotal'].forEach(function (id) { setText(id, ''); });
        HRM.setVal('txtremarksdetail', ''); HRM.setVal('txtnetwt', ''); HRM.setVal('txtstockwt', '');
        HRM.setCombo('CmbCity', 0); HRM.setCombo('cmbCropYear', 0); HRM.setCombo('combpckuom', 0);
        HRM.setCombo('CmbDeliveryTerm', 0); deliveryTermChanged();
        HRM.focus('DocDate');
        grd.clear();
        grdEmptyBags.set([blankBag()]); emptyBagsSettings();
        HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        HRM.enable('combsupplier', true);
        buttons(false);
        HRM.enable('txtcarramount', true); HRM.enable('CmbTransport', true);
        var code = get('/code').then(function (o) { HRM.setVal('txtdocno', o.docNo || 0); }).catch(HRM.fail);   // GenerateCode
        HRM.enable('txtgwt', true); HRM.enable('CmbDeliveryTerm', true); HRM.enable('combitem', true);
        resetDetail();
        HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        return code;
    }
    /** ReadById :637 - Save hidden / Update shown and the delivery term locked before the read, as the form does. */
    function readById(id) {
        HRM.show('btnSave', false); HRM.show('btnUpdate', true);
        HRM.enable('CmbDeliveryTerm', false);
        S.id = id;
        return HRM.loading(get('/by-id', { id: id })).then(function (o) {
            HRM.setCombo('combbrnch', o.BranchesId); HRM.setCombo('combproject', o.ProjectsId);
            HRM.setVal('txtdocno', HRM.str(o.DocNo)); HRM.setVal('DocDate', HRM.day(o.DocDate));
            PPB.showTab('form');
            HRM.setCombo('combsupplier', o.SupplierCustomerId); HRM.enable('combsupplier', false);
            HRM.setCombo('CmbTransport', o.TransporterId);
            setText('txtcarramount', PPB.net(o.CarriageAmount));
            setText('txtsuppwt', PPB.net(o.PartyWeight)); setText('txtfctwt', PPB.net(o.FactoryWeight));
            comboTextSet('combvehtyp', HRM.str(o.VehicleType));
            HRM.setVal('txtvehno', HRM.str(o.VehicleNo)); HRM.setVal('txtbltyno', HRM.str(o.BiltyNo));
            HRM.setVal('txtremarks', HRM.str(o.RemarksHeader));
            comboTextSet('CmbDeliveryTerm', HRM.str(o.DeliveryTerm)); deliveryTermChanged();   // Ponch clears the transporter / freight just read
            grd.set((o.details || []).map(function (d) { return Object.assign({}, d); }));
            var bags = (o.emptyBags || []).map(function (b) { return Object.assign({}, b); });
            grdEmptyBags.set(bags.length ? bags : [blankBag()]); emptyBagsSettings();
            HRM.focus('DocDate');
        }).catch(HRM.fail);
    }
    /** GrdHistory "Detail" button :2004 - InvGrn.GetByID -> Item, PackingType, UOM (""), Qty ... RemarksDetail. */
    function historyDetail(id) {
        return get('/by-id', { id: id }).then(function (o) {
            GrdHistoryDetail.set((o.details || []).map(function (d) {
                return { Item: d.Item, PackingType: d.PackingType, UOM: '', Qty: d.Qty, GrossWight: d.GrossWight, AddLesswt: d.AddLesswt,
                    NetWeight: d.NetWeight, StockWeight: d.StockWeight, City: d.City, RemarksDetail: d.RemarksDetail };
            }));
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ save / print
    /** FormValidation :701 on the page (the server repeats every check with the same text). */
    function formValid() {
        function no(msg, id) { HRM.box(msg); HRM.focus(id); return false; }
        var d = HRM.val('txtdocno').trim();
        if (d === '' || d === '0') return no('DocNo Field is Required', 'txtdocno');
        if (HRM.comboVal('combbrnch') === 0) return no('Branch Field is Required', 'combbrnch');
        if (HRM.comboVal('combproject') === 0) return no('Project Field is Required', 'combproject');
        if (HRM.comboVal('combsupplier') === 0) return no('Supplier Field is Required', 'combsupplier');
        var t = term().trim();
        if (t === '' || t === '0') return no('DeliveryTerm Field is Required', 'CmbDeliveryTerm');
        if (HRM.comboVal('combvehtyp') === 0) return no('Vehicle Type Field is Required', 'combvehtyp');
        var vn = HRM.val('txtvehno').trim();
        if (vn === '' || vn === '0') return no('Vehicle No Field is Required', 'txtvehno');
        var bn = HRM.val('txtbltyno').trim();
        if (bn === '' || bn === '0') return no('Bilty No Field is Required', 'txtbltyno');
        if (n(HRM.val('txtsuppwt').trim()) === 0) return no('Supplier Weight Field is Required', 'txtsuppwt');
        if (n(HRM.val('txtfctwt').trim()) === 0) return no('Factory Weight Field is Required', 'txtfctwt');
        return true;
    }
    function insert(btn) {                                                                      // Insert() :359
        if (!formValid()) return;
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        if (!HRM.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var preview = HRM.checked('ChkBox');
        var body = {
            id: S.id, branchId: HRM.comboVal('combbrnch'), projectId: HRM.comboVal('combproject'), docNo: HRM.val('txtdocno'),
            docDate: HRM.val('DocDate'), supplierId: HRM.comboVal('combsupplier'), deliveryTerm: term().trim(),
            vehicleTypeId: HRM.comboVal('combvehtyp'), vehicleNo: HRM.val('txtvehno'), biltyNo: HRM.val('txtbltyno'),
            supplierWeight: HRM.val('txtsuppwt'), factoryWeight: HRM.val('txtfctwt'), transporterId: HRM.comboVal('CmbTransport'),
            carriageAmount: HRM.val('txtcarramount'), remarks: HRM.val('txtremarks'), rows: grd.rows(), emptyBags: grdEmptyBags.rows()
        };
        return HRM.busy(btn, function () {
            return post('/save', body).then(function (res) {
                HRM.box(res.message);
                return reset().then(function () {
                    if (preview) generateReport(HRM.int(res.id));
                    grd.clear(); HRM.show('btnSave', true); HRM.focus('txtdocno');
                });
            }).catch(HRM.fail);
        });
    }
    P.btnSave = function (btn) { S.id = 0; return insert(btn || 'btnSave'); };                // btnSave_Click :608
    P.btnUpdate = function (btn) {                                                              // btnUpdate_Click :621
        if (S.id === 0) { HRM.box('Record not update because Id not found'); return; }
        return insert(btn || 'btnUpdate');
    };
    P.btnNew = function (btn) { return HRM.busy(btn || 'btnNew', reset); };                     // btnNew_Click :1933
    P.btnRefresh = function (btn) {                                                             // toolStripButton1_Click :2210
        return HRM.busy(btn || 'toolStripButton1', function () {
            return get('/refresh', { orderId: HRM.comboVal('cmbOrderNo') }).then(function (o) {
                fillLists(o, true);
                if (o.items && o.items.length) packUomFill();
            }).catch(HRM.fail);
        });
    };
    P.btnPrint = function () { return generateReport(S.id); };                                 // printToolStripButton_Click :2241 (the form's Id)
    P.cityDefine = function () { HRM.open('/master-data/city'); };                              // CityDefine_Click :2253 -> DefineCity

    // ------------------------------------------------------------------ history
    function historyFill(noOfRecords, btn) {                                                    // HistoryGridFill :1743
        var work = function () {
            return get('/history', { noOfRecords: noOfRecords }).then(function (rows) {
                GrdHistory.set(rows || []);
            }).catch(HRM.fail);
        };
        return btn ? HRM.busy(btn, work) : HRM.loading(work());
    }
    P.btnLoadAll = function (btn) { return historyFill(0, btn || 'btnLoadAll'); };            // btnLoadAll_Click :2264
    P.btnGrnFormHistory = function () {                                                         // btnGrnFormHistory_Click :2316 -> frmGRNHistory (477)
        if (S.rights.grnHistoryView) HRM.open('/purchase/reports/grn-register');
        else HRM.box('Please Check Screen Rights');
    };

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { if (name === 'history') historyFill(50); });                     // tabControl1_SelectedIndexChanged :2059
    HRM.footer(function () { PPB.showTab('history'); });
    ['txtqty', 'txtgwt', 'txtEBUnit', 'txtWtCutUnit', 'txtadlswt', 'txtsuppwt', 'txtfctwt'].forEach(function (id) {
        HRM.$(id).addEventListener('input', function () { onText[id](); });
    });
    HRM.$('txtadlswt').addEventListener('blur', total);                                         // txtadlswt_Leave :2082
    HRM.$('combitem').addEventListener('change', function () { packUomFill(); });              // combitem ValueChanged / Leave -> combitem_Leave
    PPB.leave('combpckuom', total);                                                              // combpckuom_Leave :2067
    PPB.leave('CmbJobLot', total);                                                               // CmbJobLot.Leave -> combpckuom_Leave
    HRM.$('CmbDeliveryTerm').addEventListener('change', deliveryTermChanged);                   // CmbDeliveryTerm_TextChanged :2392
    PPB.leave('CmbDeliveryTerm', stockWeightUpdate);                                            // CmbDeliveryTerm_Leave :2416
    PPB.leave('cmbOrderNo', function () { if (!HRM.$('cmbOrderNo').disabled) orderNoLeave().catch(HRM.fail); });                                    // cmbOrderNo_Leave :2680
    ['txtqty', 'txtgwt', 'txtEBUnit', 'txtWtCutUnit', 'txtsuppwt', 'txtfctwt'].forEach(function (id) { PPB.guard(id, 'dec'); });   // OnlytextdecimelFunction
    PPB.guard('txtcarramount', 'int');                                                          // txtcarramount_KeyPress: digits only
    HRM.setVal('DocDate', HRM.today());
    function formTab() { return PPB.currentTab() === 'form'; }
    function canClick(id) { return HRM.visible(id) && !HRM.$(id).disabled; }
    HRM.$('grd').addEventListener('keydown', function (e) {                                     // grd.AllowDelete: Delete removes the selected row
        if (e.key === 'Delete' && !e.target.closest('input,select') && grd.currentIndex() >= 0) { e.preventDefault(); removeRow(grd.currentIndex()); }
    });
    HRM.keys({                                                                                  // InvFrmGRN_KeyDown :1826
        'ctrl+l': function () { if (!formTab()) P.btnLoadAll(); },
        'ctrl+h': function () { if (!formTab()) P.btnGrnFormHistory(); },
        'ctrl+s': function () { if (formTab() && canClick('btnSave')) P.btnSave(); },
        'ctrl+n': function () { if (formTab()) P.btnNew(); },
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+u': function () { if (canClick('btnUpdate')) P.btnUpdate(); },
        'ctrl+p': function () {
            if (formTab()) { if (canClick('printToolStripButton')) P.btnPrint(); }
            else { var r = GrdHistory.current(); if (r) generateReport(HRM.int(r.Id)); }
        },
        'ctrl+f5': function () { HRM.focus('combitem'); },
        'ctrl+arrowdown': function () { var tr = HRM.$('grd').querySelector('tbody tr'); if (tr) tr.focus(); },
        'ctrl+enter': function () { if (!formTab()) { var r = GrdHistory.current(); if (r) readById(HRM.int(r.Id)); } }
    });
    load();
})();
