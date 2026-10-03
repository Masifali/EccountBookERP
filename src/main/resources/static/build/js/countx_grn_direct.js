/* ============================================================================================
 * countx_grn_direct.js - Goods Receipt Notes Direct (Architecture.WinApp.Purchase.InvFrmGRNDirect), ScreenDefinition 129
 * "frmGrnDirect", DocumentTypeId 137. Follows the form: InvFrmGRN_Load (configs, rights, combos), TotalWeight / Total /
 * TotalSupplierWeight / DeductionPolicyForGrn / StockWeightUpdateFromGridOnDeliveryTermChange / CalculationPurchaseAgainstWeight,
 * the detail Add / Edit / Update / X (and "+" for order rows), the Empty Bags grid (grdEmptyBags_CellUpdated), Insert(),
 * reset(), ReadById, History + detail of the selected row, the 211 slip, the keys. Exports window.GrnDirect.
 * Built on countx_hrm.js (HRM) and countx_pp_ppb_common.js (PPB: tabs, grid buttons, .NET number formats, guards).
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/purchase/grn-direct';
    var P = {}; window.GrnDirect = P;
    var S = {
        rights: {}, recId: 0, updateIndex: -1, uoms: [], lists: {}, ebStatus: 0, orderExist: false, defaultDays: 0,
        cfg: { juteBagsCut: 0, ppBagsCut: 0, openBulkCut: 0, deductionPolicyOn: false, termDifference: 0, termOnlyLessWeight: false, subsidiary: false, freightInwardAc: 0 }
    };

    /* The Attachment form (btnAttachment_Click :3600 AT.Show()); saved with Insert() :2046-2055. */
    var AT = window.PurchaseDocAttachments ? PurchaseDocAttachments.create({
        type: 137, getId: function () { return S.recId; },
        canEdit: function () { return S.recId > 0 ? !!S.rights.update : !!S.rights.save; },
        message: function (m) { HRM.box(m); },
        busy: function (b, work) { return HRM.busy(b, work); }
    }) : null;
    function get(path, params) { return HRM.get(API + path, params || {}); }
    function post(path, body, params) { return HRM.post(API + path + (params ? '?' + new URLSearchParams(params).toString() : ''), body); }
    var n = HRM.num;
    function r2(v) { return PPB.net(PPB.round(v, 2)); }                                      // Math.Round(x, 2).ToString()
    function box(v) { return HRM.val(v) !== '' ? n(HRM.val(v)) : 0; }                          // txt.Text != "" ? ToDouble : 0
    function term() { return HRM.comboVal('CmbDeliveryTerm') ? HRM.comboText('CmbDeliveryTerm') : ''; }
    function activeTag() { var a = document.activeElement; return a && a.getAttribute ? (a.getAttribute('data-tag') || '') : ''; }
    function dt12(v) {                                                                          // "dd-MM-yyyy hh:mm tt"
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'), t = HRM.time(v) || '00:00', h = +t.substring(0, 2);
        return p[2] + '-' + p[1] + '-' + p[0] + ' ' + String(h % 12 === 0 ? 12 : h % 12).padStart(2, '0') + ':' + t.substring(3, 5) + ' ' + (h < 12 ? 'AM' : 'PM');
    }
    /** Rebuilds a grid's header after columns were shown / hidden at run time (the form re-runs RetrieveStructure + settings). */
    function rehead(g) {
        g.table.tHead.innerHTML = '<tr>' + g.columns.map(function (c) {
            var style = (c.hidden ? 'display:none;' : '') + (c.width ? 'min-width:' + c.width + 'px;width:' + c.width + 'px;' : '');
            return '<th data-key="' + HRM.esc(c.key) + '" style="' + style + '"' + (c.align ? ' class="hrm-al-' + c.align + '"' : '') + '>' +
                HRM.esc(c.caption === undefined ? c.key : c.caption) + '</th>';
        }).join('') + '</tr>';
        g.draw();
    }
    /** POST /api/print/by-template/{rpt}/pdf - the desktop's report name; a PDF opens in a new tab, anything else is the message. */
    function printRpt(rpt, args, win) {
        var w = win || window.open('', '_blank');
        try { if (w) w.document.write('<p style="font:13px Segoe UI,sans-serif;padding:16px">Preparing report...</p>'); } catch (e) { /* ignore */ }
        var h = { 'Content-Type': 'application/json', 'Accept': 'application/pdf, text/plain' };
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn && t.getAttribute('content') && hn.getAttribute('content')) h[hn.getAttribute('content')] = t.getAttribute('content');
        return fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: h, body: JSON.stringify(args || {}) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) {
                    return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                }
                return r.text().then(function (x) { if (w) w.close(); HRM.box(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); HRM.fail(e); });
    }
    /** GenerateReport(PrintId) :3659 -> CommonServices.GrnSlipWithSubReports(PrintId, 137): the 211 rice slip with the empty bags sub report. */
    function generateReport(id) { return printRpt('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: id, documentTypeId: 137 }); }

    // ------------------------------------------------------------------ grids
    function num2(k) { return PPB.numCol(k, k, '#,##.##'); }
    var grd = new HRM.Grid('grd', {                                                              // grdSettings() :1379
        columns: [
            { key: 'Delete', caption: 'X', width: 20, render: function () { return PPB.btnCell('delete', 'X'); } },
            { key: 'Add', caption: '+', width: 20, hidden: true, render: function () { return PPB.btnCell('add', '+'); } },
            { key: 'Edit', caption: 'Edit', width: 50, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Id', hidden: true }, { key: 'OrderId', hidden: true }, { key: 'OrderDetailId', hidden: true },
            { key: 'OrderNo', caption: 'OrderNo', hidden: true }, { key: 'WarehouseId', hidden: true }, { key: 'Warehouse', caption: 'Warehouse' },
            { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' },
            { key: 'PackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType' }, { key: 'UOMId', hidden: true },
            { key: 'UOM', caption: 'UOM', align: 'right' }, { key: 'UOMEquivalent', hidden: true },
            num2('Qty'), num2('GrossWight'), num2('EbUnit'), num2('EbTotal'), num2('AddLesswt'), num2('NetWeight'), num2('StockWeight'),
            { key: 'CityId', hidden: true }, { key: 'City', caption: 'City' }
        ],
        totals: true, emptyText: '',
        onDouble: function (r, i) { grdDoubleClick(i); }
    });
    /** grdSettings(): OrderNo visible and the "+" column only when the rows came from a purchase order (OrderExist). */
    function grdSettings() {
        grd.columnOf('OrderNo').hidden = !S.orderExist;
        grd.columnOf('Add').hidden = !S.orderExist;
        rehead(grd);
    }
    PPB.onButton(grd, 'grd', function (r, act, i) {                                            // grd_ColumnButtonClick :1472
        if (act === 'delete') {
            if (S.updateIndex > -1) { HRM.box('Reset the Detail First...'); return; }
            grd.remove(i);
            enableDisableDeliveryTermSupWtFacWt();
        } else if (act === 'edit') grdDoubleClick(i);
        else if (act === 'add') {
            if (!S.orderExist) { HRM.box('You Can add rows in Order Case...Use Detail For Without Order Entry'); return; }
            grd.add(Object.assign({}, r));                                                      // dtdetail.Rows.Add(dr.ItemArray)
        }
    });

    var grdEmptyBags = new HRM.Grid('grdEmptyBags', {                                          // grdEmptyBagsSettings() :1685
        columns: [
            { key: 'Add', caption: '+', width: 20, render: function () { return PPB.btnCell('add', '+'); } },
            { key: 'OrderId', hidden: true },
            { key: 'Type', caption: 'EmptyBagsType', type: 'select', width: 80, options: function () { return opts(S.lists.emptyBagTypes, 'Type'); } },
            { key: 'ItemId', caption: 'Item Name', type: 'select', width: 250, options: function () { return opts(S.lists.emptyBagItems, 'ItemName'); } },
            { key: 'Condition', caption: 'Bags_Condition', type: 'select', width: 120, options: function () { return opts(S.lists.bagsConditions, 'Type'); } },
            { key: 'RecQty', caption: 'RecQty', type: 'edit-num', sum: true, decimals: 3 },
            { key: 'PurQty', caption: 'PurQty', type: 'edit-num', sum: true, decimals: 3 },
            { key: 'Remarks', caption: 'Remarks', type: 'edit' }
        ],
        totals: true, emptyText: '',
        onChange: function (r, k) { emptyBagsCellUpdated(r, k); }
    });
    function opts(rows, text) { return [[0, '']].concat((rows || []).map(function (x) { return [x.Id, x[text]]; })); }
    function blankBag() { return { OrderId: 0, Type: 0, ItemId: 0, Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' }; }   // AddRowInvEmptyBagsGrid
    function emptyBagsSettings() { grdEmptyBags.columnOf('Add').hidden = grdEmptyBags.rows().length !== 1; rehead(grdEmptyBags); }
    PPB.onButton(grdEmptyBags, 'grdEmptyBags', function (r, act) {                             // grdEmptyBags_ColumnButtonClick :1630
        if (act === 'add' && grdEmptyBags.rows().length === 1) grdEmptyBags.add(blankBag());
    });
    /** grdEmptyBags_CellUpdated :1716 */
    function emptyBagsCellUpdated(r, col) {
        if (HRM.int(r.PurQty) < 0) { r.PurQty = 0; HRM.box('PurQty cannot be less than Zero'); }
        if (HRM.int(r.RecQty) < 0) { r.RecQty = 0; HRM.box('RecQty cannot be less than Zero'); }
        if (col === 'PurQty' || col === 'Type') {
            S.ebStatus = HRM.int(r.Type);
            if (S.ebStatus === 2) calculationPurchaseAgainstWeight();
        }
        if ((HRM.int(r.Type) === 4 || HRM.int(r.Type) === 5) && col === 'PurQty') { r.PurQty = 0; HRM.box('Retained or Returned Stock you cannot be purchase'); }
        if (HRM.int(r.Type) === 2 && col === 'RecQty') { r.RecQty = 0; HRM.box('You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight'); }
        grdEmptyBags.draw();
    }

    var GrdHistory = new HRM.Grid('GrdHistory', {                                               // HistoryGridSettings() :3035
        columns: [
            { key: 'Print', caption: 'Print', width: 40, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'Edit', caption: 'Edit', width: 40, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'RecordNo', hidden: true }, { key: 'Id', hidden: true },
            { key: 'InvoiceNo', caption: 'InvoiceNo', width: 80 }, { key: 'DocNo', caption: 'DocNo', width: 60 },
            { key: 'DocDate', caption: 'DocDate', type: 'date', width: 80 }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 100 }, { key: 'SupplierName', caption: 'SupplierName', width: 150 },
            { key: 'GpNo', caption: 'GpNo', width: 60 }, { key: 'VehicleNo', caption: 'VehicleNo', width: 70 },
            { key: 'BiltyNo', caption: 'BiltyNo', width: 70 }, { key: 'WagesId', hidden: true },
            { key: 'WagesNo', caption: 'WagesNo', type: 'code', width: 70 },
            PPB.numCol('FactoryWeight', 'FactoryWeight', '#,##0.####'), PPB.numCol('SuppWeight', 'SuppWeight', '#,##0.####'),
            { key: 'Transporter', caption: 'Transporter' },
            PPB.numCol('FrieghtAmount', 'Freight Amount', '#,##0.##'), { key: 'RemarksHeader', caption: 'RemarksHeader' },
            { key: 'EntryDate', caption: 'EntryDate', width: 150, render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', width: 150, render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', width: 150, render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code', align: 'right' },
            { key: 'AttachmentsCount', hidden: true }, { key: 'DetailIdsCount', hidden: true }
        ],
        filterRow: true, totals: true, emptyText: '',
        onDouble: function (r) {                                                                // GrdHistory_DoubleClick :3100 (no reset first)
            if (HRM.int(r.InvoiceNo) > 0) { HRM.box("This Document is referred in invoice. So you can't update this record."); return; }
            readById(HRM.int(r.Id));
        },
        onCode: function (r, i, c) {                                                            // GrdHistory_LinkClicked :3167
            if (c && c.key === 'WagesNo') printRpt('002-ContractorWagesSlip.rpt', { id: HRM.int(r.WagesId) });
            if (c && c.key === 'NoOfAttachments' && AT) AT.view(HRM.int(r.Id));                  // GetNoofAttachmentsByRefDocumentTypeID(Id, 137)
        },
        onSelect: function (r) { historyDetail(HRM.int(r.Id)); }                                 // GrdHistory_SelectionChanged :3191
    });
    PPB.onButton(GrdHistory, 'GrdHistory', function (r, act) {                                 // GrdHistory_ColumnButtonClick :3123
        if (act === 'edit') editFromHistory(r);
        else if (act === 'print') generateReport(HRM.int(r.Id));
    });
    function editFromHistory(r) {
        if (HRM.int(r.InvoiceNo) > 0) { HRM.box("This Document is referred in invoice. So you can't update this record."); return; }
        reset().then(function () { readById(HRM.int(r.Id)); });
    }
    function h3(k, cap) { return PPB.numCol(k, cap || k, '#,##0.###'); }
    function h4(k, cap) { return PPB.numCol(k, cap || k, '#,##0.####'); }
    var GrdHistoryDetail = new HRM.Grid('GrdHistoryDetail', {                                   // gridhistorydetail(OrderExist) :3238
        columns: [
            { key: 'OrderId', hidden: true }, { key: 'OrderNo', caption: 'OrderNo', type: 'code', hidden: true },
            { key: 'WareHouse', caption: 'WareHouse' }, { key: 'Item', caption: 'Item' }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLot', caption: 'Job/Lot' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'UOM', caption: 'UOM' },
            h3('Qty'), h4('GrossWight'), h3('EbUnit'), h3('EbTotal'), h3('AddLesswt'), h4('NetWeight'), h4('StockWeight'), { key: 'City', caption: 'City' }
        ],
        totals: true, emptyText: '',
        onCode: function (r, i, c) {                                                            // GrdHistoryDetail_LinkClicked :3281
            if (!c || c.key !== 'OrderNo') return;
            if (S.rights.purchaseOrderView) printRpt('203-InvRptPurchaseOrderRiceSlip.rpt', { id: HRM.int(r.OrderId) });
            else HRM.box('Please Check Screen Rights');
        }
    });

    // ------------------------------------------------------------------ lists
    function applyConfigs(o) {                                                                  // GetConfigurations :542
        ['subsidiary', 'deductionPolicyOn', 'termDifference', 'termOnlyLessWeight', 'freightInwardAc'].forEach(function (k) { if (o[k] !== undefined) S.cfg[k] = o[k]; });
    }
    function defaultTransporter() {                                                             // FreightInwardAc when feature 4 is off
        if (!S.cfg.subsidiary && HRM.int(S.cfg.freightInwardAc) !== 0) HRM.setCombo('CmbTransport', S.cfg.freightInwardAc);
    }
    function fillLists(o, keep) {
        Object.keys(o).forEach(function (k) { S.lists[k] = o[k]; });
        HRM.fill('combsupplier', o.suppliers || [], 'Id', 'SupplierName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbTransport', o.transporters || [], 'Id', 'AccountTitle', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('combvehtyp', o.vehicleTypes || [], 'Id', 'VehicleDescription', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbWareHouse', o.warehouses || [], 'Id', 'WareHouseName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbJobLot', o.jobLots || [], 'Id', 'JobLotDescription', { zero: '...Select Any Value...', keep: keep });
        itemFill(keep);
        HRM.fill('combpcktyp', o.packingTypes || [], 'Id', 'PackTypeDesc', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbCity', o.cities || [], 'Id', 'CityName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('combcropyear', o.cropYears || [], 'Id', 'CropYear', { zero: '...Select Any Value...', keep: keep });
        applyConfigs(o);
        defaultTransporter();
        grdEmptyBags.draw();
    }
    /** ItemNameFill() :792 / rdSearchByName_CheckedChanged :1113: the item combo shows the name or the code. */
    function itemFill(keep) {
        var byName = HRM.$('rdSearchByName').checked;
        HRM.fill('combitem', S.lists.items || [], 'Id', byName ? 'ItemName' : 'ItemCode', { zero: '...Select Any Value...', keep: keep });
    }
    function itemRow() { return PPB.rowOf(S.lists.items, 'Id', HRM.comboVal('combitem')) || {}; }
    /** PackUOMFillWithoutOrder() :885 - the item's UOM schedule; the typed UOM text is kept when it is still in the list. */
    function packUomFill() {
        var prev = HRM.comboVal('combpckuom') ? HRM.comboText('combpckuom') : '';
        var itemId = HRM.comboVal('combitem');
        if (!itemId) { S.uoms = []; HRM.fill('combpckuom', [], 'Id', 'UOMCode', { zero: '' }); return Promise.resolve(); }
        return get('/uoms', { itemId: itemId }).then(function (rows) {
            S.uoms = rows || [];
            HRM.fill('combpckuom', S.uoms, 'Id', 'UOMCode', { zero: '...Select Any Value...' });
            if (prev && S.uoms.some(function (u) { return u.UOMCode === prev; })) HRM.setComboText('combpckuom', prev);
        }).catch(HRM.fail);
    }
    function uomEq() { var r = PPB.rowOf(S.uoms, 'Id', HRM.val('combpckuom')); return r ? n(r.Equivalent) : 0; }

    // ------------------------------------------------------------------ calculations
    function setGwt(v) { HRM.setVal('txtgwt', v); total(); }                                   // txtgwt_TextChanged -> Total()
    /** TotalWeight() :2411 */
    function totalWeight() {
        var eq = HRM.comboVal('combpckuom') ? uomEq() : 0;
        var qty = box('txtqty');
        setGwt(PPB.net(eq > 0 ? eq * qty : 0));
    }
    /** Total() :2438 - E.b by the focused box (Tag EbUnit / EbTotal), purchase-against-weight bags, then the stock weight by term. */
    function total() {
        var factory = box('txtfctwt'), supplier = box('txtsuppwt');
        var qty = box('txtqty'), gross = box('txtgwt'), ebUnit = box('txtebu');
        var diff = supplier - factory, purQty = 0, tpaw = 0;
        var ebTotal = n(HRM.val('txtEmptyBagsTotal'));
        var tag = activeTag();
        if (ebUnit > 0 || ebTotal > 0) {
            if (tag === 'EbUnit') { ebTotal = ebUnit * qty; HRM.setVal('txtEmptyBagsTotal', r2(ebTotal)); }
            else if (tag === 'EbTotal') { ebUnit = ebTotal / qty; HRM.setVal('txtebu', isFinite(ebUnit) ? PPB.net(PPB.round(ebUnit, 3)) : ''); }
            else { ebTotal = ebUnit > 0 && qty > 0 ? ebUnit * qty : 0; HRM.setVal('txtEmptyBagsTotal', r2(ebTotal)); }
        }
        if (ebUnit > 0) {
            S.ebStatus = 0;
            var grdQty = grd.sum('Qty');
            grdEmptyBags.rows().forEach(function (e) { if (HRM.int(e.Type) === 2) purQty += n(e.PurQty); });
            if (purQty > 0) {
                var upd = HRM.visible('btnUpdateDetail') && grd.rows()[S.updateIndex] ? n(grd.rows()[S.updateIndex].Qty) : 0;
                S.ebStatus = 2;
                tpaw = (Math.abs(purQty) - (grdQty - upd)) * ebUnit;
            }
        }
        var addLess = box('txtadlswt');
        var weight;
        if (S.ebStatus === 2 && tpaw > 0) weight = gross - ebTotal + addLess + tpaw;
        else if (S.ebStatus === 2) weight = gross + addLess;
        else weight = gross - ebTotal + addLess;
        HRM.setVal('txtnetwt', r2(weight));
        HRM.setVal('txtstockwt', r2(weight));
        if (!(supplier > 0) || !(factory > 0) || !(gross > 0)) return;
        if (S.cfg.deductionPolicyOn) {
            var bal = n(HRM.val('txtBalWeight'));
            HRM.setVal('txtstockwt', r2(bal > 0 ? factory * gross / bal : 0));
            return;
        }
        if (S.cfg.termOnlyLessWeight) {
            if (diff > 0 && n(S.cfg.termDifference) - diff >= 0) HRM.setVal('txtstockwt', r2(factory * gross / supplier - ebTotal));
            else if (factory > supplier) HRM.setVal('txtstockwt', r2(factory * gross / supplier - ebTotal));
            else HRM.setVal('txtstockwt', r2(factory * gross / factory - ebTotal));
            return;
        }
        var t = term();
        if (t === 'Load' || t === 'Load & PartyWeight' || t === 'Ponch & PartyWeight') HRM.setVal('txtstockwt', r2(factory * gross / supplier - ebTotal));
        if (t === 'Ponch') HRM.setVal('txtstockwt', r2(supplier > factory ? factory * gross / factory - ebTotal : factory * gross / supplier - ebTotal));
        if (t === 'Ponch & FactoryWeight' || t === 'Load & FactoryWeight') HRM.setVal('txtstockwt', r2(factory * gross / factory - ebTotal));
    }
    /** DeductionPolicyForGrn(0) :2356 - with the policy on and the Ponch term, the remaining gross weight of the final bill weight. */
    function deductionPolicyForGrn() {
        var factory = n(HRM.val('txtfctwt'));
        if (S.cfg.deductionPolicyOn && term() === 'Ponch') {
            var supplier = n(HRM.val('txtsuppwt')), diff = factory - supplier;
            var ask = diff > 0 ? get('/deduction-policy', { date: HRM.val('DocDate'), difference: diff }) : Promise.resolve({ PolicyTypeId: 0 });
            return ask.then(function (p) {
                var id = HRM.int(p && p.PolicyTypeId), fin = 0;
                if (id === 0 || id === 1) fin = factory; else if (id === 2) fin = factory + diff / 2; else if (id === 3) fin = supplier;
                var g = grd.sum('GrossWight');
                if (HRM.visible('btnUpdateDetail') && grd.rows()[S.updateIndex]) g -= n(grd.rows()[S.updateIndex].GrossWight);
                setGwt(PPB.net(fin - g));
            }).catch(HRM.fail);
        } else if (S.cfg.deductionPolicyOn) HRM.setVal('txtBalWeight', PPB.net(factory));
        return Promise.resolve();
    }
    /** TotalSupplierWeight() :2606 */
    function totalSupplierWeight() {
        if (HRM.val('txtsuppwt') !== '' && HRM.val('txtfctwt') !== '') HRM.setVal('txtwtdiff', PPB.net(n(HRM.val('txtsuppwt')) - n(HRM.val('txtfctwt'))));
        deductionPolicyForGrn();
    }
    /** StockWeightUpdateFromGridOnDeliveryTermChange() :3735 (EBTotal is always 0 here; two branches write the text box, not the row). */
    function stockWeightUpdate() {
        var factory = box('txtfctwt'), supplier = box('txtsuppwt'), diff = supplier - factory, t = term();
        if (grd.rows().length) {
            if (!(supplier > 0) || !(factory > 0)) return;
            grd.rows().forEach(function (r) {
                var gross = n(r.GrossWight);
                if (supplier > 0 && factory > 0 && gross > 0) {
                    if (S.cfg.termOnlyLessWeight) {
                        if (diff > 0 && n(S.cfg.termDifference) - diff >= 0) r.StockWeight = PPB.round(factory * gross / supplier, 2);
                        else if (factory > supplier) r.StockWeight = PPB.round(factory * gross / supplier, 2);
                        else r.StockWeight = PPB.round(factory * gross / factory, 2);
                    } else {
                        if (t === 'Load' || t === 'Load & PartyWeight' || t === 'Ponch & PartyWeight') r.StockWeight = PPB.round(factory * gross / supplier, 2);
                        if (t === 'Ponch') {
                            if (supplier > factory) r.StockWeight = PPB.round(factory * gross / factory, 2);
                            else HRM.setVal('txtstockwt', r2(factory * gross / supplier));
                        }
                        if (t === 'Ponch & FactoryWeight' || t === 'Load & FactoryWeight') HRM.setVal('txtstockwt', r2(factory * gross / factory));
                    }
                } else r.StockWeight = 0;
            });
            grd.draw();
            return;
        }
        if (t === 'Load') setGwt(PPB.net(supplier));
        if (t === 'Ponch') setGwt(PPB.net(supplier > factory ? factory : supplier));
    }
    /** CalculationPurchaseAgainstWeight() :2693 - every detail row's E.b total, net weight and stock weight. */
    function calculationPurchaseAgainstWeight() {
        var factory = box('txtfctwt'), supplier = box('txtsuppwt'), diff = supplier - factory, t = term();
        var allocate = 0, purQty = 0, ebLess = 0;
        grdEmptyBags.rows().forEach(function (e) { if (HRM.int(e.Type) === 2) purQty += n(e.PurQty); });
        grd.rows().forEach(function (r) {
            var tpaw = 0, qty = n(r.Qty), gross = n(r.GrossWight), ebUnit = n(r.EbUnit), addLess = n(r.AddLesswt), ebTotal;
            if (ebUnit > 0) {
                var grdQty = qty;
                if (purQty > 0) {
                    purQty -= allocate;
                    if (purQty > grdQty) ebLess = grdQty; else if (grdQty > purQty) ebLess = purQty;
                    if (purQty === grdQty) ebLess = grdQty;
                    if (purQty > 0) tpaw = ebLess * ebUnit;
                }
            }
            if (ebUnit > 0 && qty > 0) { r.EbTotal = ebUnit * qty; ebTotal = n(r.EbTotal); } else { r.EbTotal = 0; ebTotal = 0; }
            var weight = gross - ebTotal + addLess + tpaw;
            r.NetWeight = PPB.round(weight, 2);
            if (supplier > 0 && factory > 0 && gross > 0) {
                if (S.cfg.deductionPolicyOn) { var bal = n(HRM.val('txtBalWeight')); r.StockWeight = PPB.round(bal > 0 ? factory * gross / bal : 0, 2); }
                else if (S.cfg.termOnlyLessWeight) {
                    if (diff > 0 && n(S.cfg.termDifference) - diff >= 0) r.StockWeight = PPB.round(factory * gross / supplier - ebTotal, 2);
                    else if (factory > supplier) r.StockWeight = PPB.round(factory * gross / supplier - ebTotal, 2);
                    else r.StockWeight = PPB.round(factory * gross / factory - ebTotal, 2);
                }
                else if (t === 'Load & FactoryWeight' || t === 'Ponch & FactoryWeight') r.StockWeight = PPB.round(factory * gross / factory - ebTotal, 2);
                else if (t === 'Ponch') r.StockWeight = PPB.round(supplier > factory ? factory * gross / factory - ebTotal : factory * gross / supplier - ebTotal, 2);
                else r.StockWeight = PPB.round(factory * gross / supplier - ebTotal, 2);
            } else r.StockWeight = PPB.round(weight, 2);
            allocate = ebLess;
        });
        grd.draw();
    }
    /** EnableDisableDeliveryTermSupWtFacWt() :1316 */
    function enableDisableDeliveryTermSupWtFacWt() {
        var count = grd.rows().length;
        HRM.enable('CmbDeliveryTerm', count === 0); HRM.enable('txtsuppwt', count === 0); HRM.enable('txtfctwt', count === 0);
        S.orderExist = count !== 0 && S.orderExist;
        HRM.enable('combitem', !S.orderExist); HRM.enable('combsupplier', !S.orderExist);
    }

    // ------------------------------------------------------------------ detail
    /** FormValidationDetail() :1227 */
    function detailValid() {
        function no(msg, id) { HRM.box(msg); HRM.focus(id); return false; }
        if (HRM.comboVal('CmbDeliveryTerm') === 0) return no('DeliveryTerm Field is Required', 'CmbDeliveryTerm');
        if (HRM.val('txtsuppwt').trim() === '' || n(HRM.val('txtsuppwt')) === 0) return no('SupplierWeight Field is Required', 'txtsuppwt');
        if (HRM.val('txtfctwt').trim() === '' || n(HRM.val('txtfctwt')) === 0) return no('Factory Weight Field is Required', 'txtfctwt');
        if (HRM.comboVal('CmbWareHouse') === 0) return no('Warehouse Field is Required', 'CmbWareHouse');
        if (HRM.comboVal('combitem') === 0) return no('Item Name Field is Required', 'combitem');
        if (HRM.comboVal('combcropyear') === 0) return no('Crop Year Field is Required', 'combcropyear');
        if (HRM.comboVal('CmbJobLot') === 0) return no('JobLot Field is Required', 'CmbJobLot');
        if (HRM.comboVal('combpcktyp') === 0) return no('Packing Type Field is Required', 'combpcktyp');
        if (HRM.comboVal('combpckuom') === 0) return no('Pack Unit Field is Required', 'combpckuom');
        if (HRM.val('txtqty').trim() === '' || n(HRM.val('txtqty')) === 0) return no('Qty Field is Required', 'txtqty');
        if (HRM.val('txtgwt').trim() === '' || n(HRM.val('txtgwt')) === 0) return no('Gross Weight Field is Required', 'txtgwt');
        if (HRM.val('txtnetwt').trim() === '' || n(HRM.val('txtnetwt')) === 0) return no('Net Bill Weight Field is Required', 'txtnetwt');
        if (HRM.val('txtstockwt').trim() === '' || n(HRM.val('txtstockwt')) === 0) return no('Stock Weight Field is Required', 'txtstockwt');
        if (HRM.comboVal('CmbCity') === 0) return no('City Field is Required', 'CmbCity');
        return true;
    }
    function detailFields(r, trimNames) {
        var it = itemRow();
        r.WarehouseId = HRM.comboVal('CmbWareHouse'); r.Warehouse = trimNames ? HRM.comboText('CmbWareHouse').trim() : HRM.comboText('CmbWareHouse');
        r.ItemId = HRM.comboVal('combitem'); r.ItemCode = HRM.str(it.ItemCode); r.ItemName = HRM.str(it.ItemName);
        r.CropYear = trimNames ? HRM.comboText('combcropyear').trim() : HRM.comboText('combcropyear');
        r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = trimNames ? HRM.comboText('CmbJobLot').trim() : HRM.comboText('CmbJobLot');
        r.PackingTypeId = HRM.comboVal('combpcktyp'); r.PackingType = HRM.comboText('combpcktyp');
        r.UOMId = HRM.comboVal('combpckuom'); r.UOM = trimNames ? HRM.comboText('combpckuom').trim() : HRM.comboText('combpckuom'); r.UOMEquivalent = uomEq();
        r.Qty = n(HRM.val('txtqty')); r.GrossWight = n(HRM.val('txtgwt')); r.EbUnit = n(HRM.val('txtebu')); r.EbTotal = n(HRM.val('txtEmptyBagsTotal'));
        r.AddLesswt = n(HRM.val('txtadlswt')); r.NetWeight = n(HRM.val('txtnetwt')); r.StockWeight = n(HRM.val('txtstockwt'));
        r.CityId = HRM.comboVal('CmbCity'); r.City = HRM.comboText('CmbCity');
        return r;
    }
    P.add = function () {                                                                       // Add_Click :1342
        var cur = grd.current();
        if (grd.rows().length > 0 && cur && HRM.int(cur.OrderId) !== 0 && S.orderExist) { HRM.box('You cannot add Record because you had add record with Order'); return; }
        if (!detailValid()) return;
        if (n(HRM.val('txtnetwt')) > n(HRM.val('txtgwt'))) { HRM.box('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        grd.add(detailFields({ Id: 0, OrderId: 0, OrderDetailId: 0, OrderNo: 0 }, true));
        grdSettings();
        resetDetail();
        calculationPurchaseAgainstWeight();
        enableDisableDeliveryTermSupWtFacWt();
    };
    function grdDoubleClick(i) {                                                               // grd_DoubleClick :1516
        var r = grd.rows()[i]; if (!r) return;
        S.updateIndex = i;
        HRM.setCombo('CmbWareHouse', r.WarehouseId);
        HRM.setCombo('combitem', r.ItemId);
        var uomLoad = packUomFill();                                                            // combitem_Leave: UOMs + DeductionPolicyForGrn(0)
        deductionPolicyForGrn();
        HRM.setComboText('combcropyear', HRM.str(r.CropYear));
        HRM.setCombo('CmbJobLot', r.JobLotId);
        HRM.setCombo('combpcktyp', r.PackingTypeId);
        uomLoad.then(function () {
            HRM.setCombo('combpckuom', r.UOMId);
            HRM.setVal('txtqty', PPB.net(r.Qty)); HRM.setVal('txtgwt', PPB.net(r.GrossWight));
            HRM.setVal('txtebu', PPB.net(r.EbUnit)); HRM.setVal('txtEmptyBagsTotal', PPB.net(r.EbTotal));
            HRM.setVal('txtadlswt', PPB.net(r.AddLesswt)); HRM.setVal('txtnetwt', PPB.net(r.NetWeight)); HRM.setVal('txtstockwt', PPB.net(r.StockWeight));
            HRM.setCombo('CmbCity', r.CityId);
        });
        HRM.show('Add', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
        HRM.focus('CmbWareHouse');
        HRM.enable('combitem', !S.orderExist);
    }
    P.btnUpdateDetail = function () {                                                           // btnUpdateDetail_Click :1560
        if (!detailValid()) return;
        if (n(HRM.val('txtnetwt')) > n(HRM.val('txtgwt'))) { HRM.box('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        var r = grd.rows()[S.updateIndex]; if (!r) return;
        detailFields(r, false);
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        resetDetail();
        grd.draw();
        calculationPurchaseAgainstWeight();
        enableDisableDeliveryTermSupWtFacWt();
    };
    P.btnCancelUpdateDetial = function () {                                                     // btnCancelUpdateDetial_Click :1609
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        S.updateIndex = -1;
    };
    /** ResetDetial() :2292 - clearing Qty / Gross / Ad-Ls re-runs TotalWeight / Total as the text boxes' TextChanged do. */
    function resetDetail() {
        HRM.setCombo('CmbWareHouse', 0); HRM.setCombo('CmbJobLot', 0);
        HRM.setVal('txtqty', ''); totalWeight(); total();
        HRM.setVal('txtgwt', ''); total();
        HRM.setVal('txtadlswt', ''); total();
        HRM.setVal('txtnetwt', ''); HRM.setVal('txtstockwt', '');
        HRM.setCombo('combpckuom', 0);
        S.updateIndex = -1;
        HRM.focus('CmbWareHouse');
    }

    // ------------------------------------------------------------------ load / reset / read
    function buttons(update) { HRM.show('btnSave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update); }
    function load() {                                                                           // InvFrmGRN_Load :418
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            S.defaultDays = HRM.int(o.defaultDays);
            ['juteBagsCut', 'ppBagsCut', 'openBulkCut'].forEach(function (k) { S.cfg[k] = n(o[k]); });
            HRM.setVal('txtebu', '0'); HRM.setVal('txtadlswt', '0');
            HRM.enable('txtebu', !!o.emptyBagsWeightCutEditable); HRM.enable('txtEmptyBagsTotal', !!o.emptyBagsWeightCutEditable);
            HRM.enable('txtadlswt', !!o.addLessWeightEditable);
            HRM.applyRights({ save: S.rights.save, print: S.rights.print, update: S.rights.update }, { save: 'btnSave', print: 'BtnPrint', update: 'btnUpdate' });
            S.lists.emptyBagTypes = o.emptyBagTypes || []; S.lists.bagsConditions = o.bagsConditions || []; S.lists.emptyBagItems = o.emptyBagItems || [];
            fillLists(o, false);
            HRM.fillFixed('CmbDeliveryTerm', [[0, ''], [1, 'Load'], [2, 'Ponch']]); HRM.setCombo('CmbDeliveryTerm', 1);   // DeliveryTerm(): Rows[0] "Load"
            HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : '');
            grdEmptyBags.set([blankBag()]); emptyBagsSettings();
            grdSettings();
            HRM.fill('cmbSupplierNameHistory', o.historySuppliers || [], 'Id', 'name', { zero: '' });
            PPB.resetHistoryFilters(S.defaultDays);
            buttons(false);
            HRM.focus('DocDate');
            /* ?id= deep link (clickable document codes, countx_doc_link.js type 137): open the record once the lookups are in. */
            var qid = parseInt(new URLSearchParams(location.search).get('id') || '0', 10);
            if (qid > 0) return readById(qid);
        }).catch(HRM.fail);
    }
    /** reset() :2222 */
    function reset() {
        S.recId = 0; S.ebStatus = 0; S.updateIndex = -1; S.orderExist = false;
        if (AT) AT.reset();
        HRM.setCombo('combsupplier', 0); HRM.setCombo('CmbDeliveryTerm', 0); HRM.setCombo('CmbTransport', 0);
        ['txtcarramount', 'txtGpNo', 'txtvehno', 'txtbltyno', 'txtsuppwt', 'txtfctwt', 'txtwtdiff', 'txtremarks', 'txtebu', 'txtEmptyBagsTotal',
            'txtqty', 'txtadlswt', 'txtnetwt', 'txtstockwt'].forEach(function (id) { HRM.setVal(id, ''); });
        var vt = S.lists.vehicleTypes || []; HRM.setCombo('combvehtyp', vt.length ? vt[0].Id : 0);   // Rows[1] - the first vehicle type
        ['combsupplier', 'CmbTransport', 'txtcarramount', 'txtgwt', 'combitem', 'CmbDeliveryTerm'].forEach(function (id) { HRM.enable(id, true); });
        HRM.setCombo('combitem', 0); HRM.setCombo('combpckuom', 0); HRM.setCombo('CmbCity', 0);
        grd.clear();
        buttons(false);
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        grdEmptyBags.set([blankBag()]); emptyBagsSettings();
        grdSettings();
        HRM.focus('DocDate');
        enableDisableDeliveryTermSupWtFacWt();
        defaultTransporter();
        return get('/code').then(function (o) { HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : ''); }).catch(HRM.fail);
    }
    /** ReadById(ID) :2111 - Save hidden / Update + Delete shown before the read, as the form does. */
    function readById(id) {
        buttons(true);
        if (AT) AT.reset();
        S.recId = id;
        return HRM.loading(get('/by-id', { id: id })).then(function (o) {
            if (!(o.details || []).length) return;
            PPB.showTab('form');
            HRM.setVal('txtdocno', HRM.str(o.DocNo)); HRM.setVal('DocDate', HRM.day(o.DocDate));
            HRM.setCombo('combsupplier', o.SupplierCustomerId); HRM.enable('combsupplier', true);
            HRM.setComboText('CmbDeliveryTerm', HRM.str(o.DeliveryTerm));
            HRM.setCombo('CmbTransport', o.TransporterId);
            HRM.setVal('txtcarramount', PPB.net(o.CarriageAmount)); HRM.setVal('txtremarks', HRM.str(o.RemarksHeader));
            HRM.setVal('txtGpNo', HRM.str(o.GpNo)); HRM.setComboText('combvehtyp', HRM.str(o.VehicleType));
            HRM.setVal('txtvehno', HRM.str(o.VehicleNo)); HRM.setVal('txtbltyno', HRM.str(o.BiltyNo));
            HRM.setVal('txtsuppwt', PPB.net(o.PartyWeight)); HRM.setVal('txtfctwt', PPB.net(o.FactoryWeight));
            HRM.setVal('txtwtdiff', PPB.net(n(o.PartyWeight) - n(o.FactoryWeight)));
            var rows = o.details.map(function (d) { return Object.assign({}, d); });
            rows.forEach(function (d) {                                                         // the last row decides OrderExist
                S.orderExist = HRM.int(d.OrderId) > 0;
                HRM.enable('CmbDeliveryTerm', !S.orderExist); HRM.enable('combsupplier', !S.orderExist);
            });
            grd.set(rows); grdSettings();
            var bags = (o.emptyBags || []).map(function (b) { return Object.assign({}, b); });
            grdEmptyBags.set(bags.length ? bags : [blankBag()]); emptyBagsSettings();
            enableDisableDeliveryTermSupWtFacWt();
            return deductionPolicyForGrn();
        }).catch(HRM.fail);
    }
    function historyDetail(id) {                                                                // GrdHistory_SelectionChanged :3191
        return get('/by-id', { id: id }).then(function (o) {
            var rows = (o.details || []).map(function (d) {
                return { OrderId: d.OrderId, OrderNo: d.PurchaseOrder, WareHouse: d.Warehouse, Item: d.ItemName, CropYear: d.CropYear, JobLot: d.JobLot,
                    PackingType: d.PackingType, UOM: d.UOMEquivalentText, Qty: d.Qty, GrossWight: d.GrossWight, EbUnit: d.EbUnit, EbTotal: d.EbTotal,
                    AddLesswt: d.AddLesswt, NetWeight: d.NetWeight, StockWeight: d.StockWeight, City: d.City };
            });
            GrdHistoryDetail.columnOf('OrderNo').hidden = !rows.some(function (r) { return HRM.int(r.OrderId) > 0; });
            GrdHistoryDetail.data = rows; GrdHistoryDetail.cur = -1;
            rehead(GrdHistoryDetail);
        }).catch(function (e) { GrdHistoryDetail.clear(); HRM.fail(e); });
    }

    // ------------------------------------------------------------------ save / delete / print
    /** FormValidation() :1168 on the page (the server repeats every check with the same text). */
    function formValid() {
        function no(msg, id) { HRM.box(msg); HRM.focus(id); return false; }
        var d = HRM.val('txtdocno').trim();
        if (d === '' || d === '0') return no('DocNo Field is Required', 'txtdocno');
        if (HRM.comboVal('combsupplier') === 0) return no('Supplier Field is Required', 'combsupplier');
        if (HRM.comboVal('CmbDeliveryTerm') === 0) return no('DeliveryTerm Field is Required', 'CmbDeliveryTerm');
        var gp = HRM.val('txtGpNo').trim();
        if (gp === '' || gp === '0') return no('Gp No Field is Required', 'txtGpNo');
        if (HRM.comboVal('combvehtyp') === 0) return no('Vehicle Type Field is Required', 'combvehtyp');
        var vn = HRM.val('txtvehno').trim();
        if (vn === '' || vn === '0') return no('Vehicle No Field is Required', 'txtvehno');
        var bn = HRM.val('txtbltyno').trim();
        if (bn === '' || bn === '0') return no('Bilty No Field is Required', 'txtbltyno');
        if (n(HRM.val('txtsuppwt')) === 0) return no('Supplier Weight Field is Required', 'txtsuppwt');
        if (n(HRM.val('txtfctwt')) === 0) return no('Factory Weight Field is Required', 'txtfctwt');
        return true;
    }
    function insert(btn) {                                                                      // Insert() :1763
        if (!formValid()) return;
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var preview = HRM.checked('ChkBox');
        var body = {
            id: S.recId, docNo: HRM.val('txtdocno'), docDate: HRM.val('DocDate'), supplierId: HRM.comboVal('combsupplier'),
            deliveryTerm: term(), remarks: HRM.val('txtremarks'), gpNo: HRM.val('txtGpNo'), vehicleTypeId: HRM.comboVal('combvehtyp'),
            vehicleNo: HRM.val('txtvehno'), biltyNo: HRM.val('txtbltyno'), supplierWeight: HRM.val('txtsuppwt'), factoryWeight: HRM.val('txtfctwt'),
            transporterId: HRM.comboVal('CmbTransport'), carriageAmount: HRM.val('txtcarramount'), rows: grd.rows(), emptyBags: grdEmptyBags.rows()
        };
        var attached = AT ? AT.payload() : undefined;
        if (attached) body.attachments = attached;
        return HRM.busy(btn, function () {
            return post('/save', body).then(function (res) {
                HRM.box(res.message);
                if (res.openWages) HRM.open('/accounts/vouchers/labour-wages');                  // frmwagesBillHeader for RefDocTypeId 137
                return reset().then(function () { if (preview) generateReport(HRM.int(res.id)); });
            }).catch(HRM.fail);
        });
    }
    P.btnSave = function (btn) { S.recId = 0; return insert(btn || 'btnSave'); };            // btnSave_Click :2082
    P.btnUpdate = function (btn) {                                                              // btnUpdate_Click :2095
        if (S.recId === 0) { HRM.box('Record not update because Id not found'); return; }
        return insert(btn || 'btnUpdate');
    };
    P.btnNew = function (btn) { return HRM.busy(btn || 'btnNew', reset); };                    // busy while the new DocNo is read
    P.btnRefresh = function (btn) {                                                             // BtnRefresh_Click :2323
        return HRM.busy(btn || 'BtnRefresh', function () { return get('/refresh').then(function (o) { fillLists(o, true); }).catch(HRM.fail); });
    };
    P.btnDelete = function (btn) {                                                              // btnDelete_Click :2186
        if (S.recId <= 0) { HRM.box('Record Not Found'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return post('/delete', {}, { id: S.recId }).then(function (res) { HRM.box(res.message); return reset(); }).catch(HRM.fail);
        });
    };
    P.btnPrint = function (btn) { return HRM.busy(btn || 'BtnPrint', function () { return generateReport(S.recId); }); };   // BtnPrint_Click :3671
    P.btnAttachment = function (btn) { return AT ? AT.open(btn || HRM.$('btnAttachment')) : null; }   // btnAttachment_Click :3600
    P.cityDefine = function () { HRM.open('/master-data/city'); };                              // CityDefine_Click :3706 - new DefineCity(UserAccount).Show()

    // ------------------------------------------------------------------ Load Order (BtnLoadOrder_Click :4027)
    /* LoadPurchaseOrder (DocumentTypeId 41): its source is not in the desktop tree; the list is BLL 0595
       GetPurchaseOrderForPurchaseInvoice and the filters are that call's optional parameters. The picked orders
       (dtSupply) are then loaded as LoadInGridDetail :4055 does. */
    var LDR_COLS = [
        { key: 'Pick', caption: '', type: 'edit-check', width: 24 },
        { key: 'DocNo', caption: 'DocNo', width: 60 }, { key: 'DocDate', caption: 'DocDate', type: 'date', width: 90 },
        { key: 'SupplierName', caption: 'SupplierName', width: 180 }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName', width: 180 },
        { key: 'ItemQty', caption: 'ItemQty', type: 'num', decimals: 2 }, { key: 'ItemWeight', caption: 'ItemWeight', type: 'num', decimals: 2 },
        { key: 'PackUom', caption: 'PackUom' }, { key: 'UOMCode', caption: 'Rate Uom' }, { key: 'OrderItemRate', caption: 'OrderItemRate', type: 'num', decimals: 2 },
        { key: 'DeliveryTerm', caption: 'DeliveryTerm' }, { key: 'JobLotDescription', caption: 'JobLot' }
    ];
    P.btnLoadOrder = function () {
        if (grd.rows().length > 0) {
            var cur = grd.current();
            if (cur && HRM.int(cur.OrderId) === 0) { HRM.box('You cannot Load PurchaseOrder because you had add record without Order'); return; }
        }
        var sup = (S.lists.suppliers || []).map(function (r) { return '<option value="' + HRM.esc(r.Id) + '">' + HRM.esc(r.SupplierName) + '</option>'; }).join('');
        var m = HRM.modal({ title: 'Load Purchase Order', width: 'min(1100px, 98vw)', html:
            '<div class="hrm-history-bar" style="display:flex;flex-wrap:wrap;gap:8px;align-items:center;margin-bottom:6px">' +
            '<label>Supplier <select id="ldrSupplier" class="win-combo" style="width:220px"><option value="0"></option>' + sup + '</select></label>' +
            '<label>From Date <input type="date" id="ldrFromDate" class="win-textbox" style="width:130px"></label>' +
            '<label>To Date <input type="date" id="ldrToDate" class="win-textbox" style="width:130px"></label>' +
            '<label>From Doc No <input type="text" id="ldrFromDocNo" class="win-textbox" inputmode="numeric" style="width:70px"></label>' +
            '<label>To Doc No <input type="text" id="ldrToDocNo" class="win-textbox" inputmode="numeric" style="width:70px"></label>' +
            '<button type="button" id="ldrShow" class="win-btn-action">Show</button><button type="button" id="ldrLoad" class="win-btn-action">Load</button></div>' +
            '<div class="hrm-grid-box hrm-grid-box-modal"><div class="hrm-grid-wrap"><table id="ldrGrd"></table></div></div>' });
        if (HRM.comboVal('combsupplier')) HRM.$('ldrSupplier').value = String(HRM.comboVal('combsupplier'));
        var g = new HRM.Grid('ldrGrd', { columns: LDR_COLS, filterRow: true, checkAll: 'Pick', emptyText: '' });
        g.draw();
        HRM.$('ldrShow').addEventListener('click', function () {
            var b = this;
            return HRM.busy(b, function () {
                return get('/order-loader', { supplierId: HRM.int(HRM.$('ldrSupplier').value), fromDate: HRM.$('ldrFromDate').value, toDate: HRM.$('ldrToDate').value,
                    fromDocNo: HRM.int(HRM.$('ldrFromDocNo').value), toDocNo: HRM.int(HRM.$('ldrToDocNo').value) })
                    .then(function (rows) { g.set((rows || []).map(function (r) { return Object.assign({ Pick: false }, r); })); }).catch(HRM.fail);
            });
        });
        HRM.$('ldrLoad').addEventListener('click', function () {
            var ids = [];
            g.checked('Pick').forEach(function (r) { var id = HRM.int(r.Id); if (id > 0 && ids.indexOf(id) < 0) ids.push(id); });
            m.close();
            if (!ids.length) return;
            return HRM.busy('BtnLoadOrder', function () {
                return loadInGridDetail(ids).then(function () { stockWeightUpdate(); return deductionPolicyForGrn(); }).catch(HRM.fail);
            });
        });
    };
    /** LoadInGridDetail :4055 -> LoadPurchaseOrderDataForInvoice :4087 + GetEmptyBagsInformationFromOrder :4137. */
    function loadInGridDetail(ids) {
        S.orderExist = true;
        return get('/order-lines', { ids: ',' + ids.join(',') }).then(function (o) {
            var lines = o.lines || [];
            try {
                if (lines.length) {
                    var first = lines[0];
                    if (grd.rows().length > 0 && HRM.comboVal('combsupplier') !== HRM.int(first.SupplierCustomerId)) throw new Error('Data against another Supplier Already Exist');
                    HRM.setCombo('combsupplier', first.SupplierCustomerId);
                    HRM.setVal('txtremarks', HRM.str(first.RemarksHeader));
                    HRM.setComboText('CmbDeliveryTerm', HRM.str(first.DeliveryTerm));
                    HRM.enable('combsupplier', false); HRM.enable('CmbDeliveryTerm', false);
                    lines.forEach(function (l) {
                        var skip = false, rows = grd.rows();
                        for (var i = 0; i < rows.length; i++) {
                            if (HRM.int(rows[i].OrderId) !== HRM.int(l.Id)) throw new Error('Data against another OrderNo Already Exist in Detail');
                            if (HRM.int(rows[i].OrderDetailId) === HRM.int(l.PODetailId)) { skip = true; break; }
                        }
                        if (skip) return;
                        var gross = n(l.GrossWeight), ebt = n(l.EmptyBagsTotal);
                        grd.data.push({ Id: 0, OrderId: HRM.int(l.Id), OrderDetailId: HRM.int(l.PODetailId), OrderNo: HRM.int(l.OrderNo), WarehouseId: 0, Warehouse: '',
                            ItemId: HRM.int(l.ItemId), ItemCode: HRM.str(l.ItemCode), ItemName: HRM.str(l.ItemName), CropYear: HRM.str(l.Crop),
                            JobLotId: HRM.int(l.JobLotId), JobLot: HRM.str(l.JobLotDescription), PackingTypeId: 0, PackingType: '0',
                            UOMId: HRM.int(l.OrderItemUOMId), UOM: HRM.str(l.PackUom), UOMEquivalent: n(l.UOMCodeItem), Qty: n(l.ItemQty), GrossWight: gross,
                            EbUnit: n(l.EmptyBags), EbTotal: ebt, AddLesswt: 0, NetWeight: gross - ebt, StockWeight: gross - ebt, CityId: 0, City: '0' });
                    });
                }
            } catch (e) { grd.draw(); HRM.box(e.message); return; }                         // LoadInGridDetail's catch: the message, nothing more
            grdSettings();
            var bags = o.emptyBags || [];                                                         // GetEmptyBagsInformationFromOrder :4137
            if (bags.length) {
                var list = [], pack = HRM.comboVal('combpcktyp');
                bags.forEach(function (b) {
                    var t = HRM.int(b.Type);
                    if ((t === 1 || t === 2 || t === 3) && HRM.int(b.PackingTypeId) === pack) { HRM.setVal('txtebu', HRM.str(b.WeightCut)); S.ebStatus = t; }
                    list.push({ OrderId: HRM.int(b.PurchaseOrderId), Type: HRM.int(b.Type), ItemId: HRM.int(b.ItemId), Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' });
                });
                grdEmptyBags.set(list); emptyBagsSettings();
                total();
            }
        });
    }
    P.shortcuts = function () {                                                                 // MakeShortCutKeys() :3471
        HRM.box('Ctrl+S  For Save\nCtrl+U  For Update\nCtrl+Shift+Delete  For Delete\nCtrl+E  For Close\nCtrl+R  For Refresh\nCtrl+N  For New\n' +
            'Ctrl+P  For Print\nCtrl+H  For History Print\nCtrl+F5  For Focus on Doc Date\nCtrl+F10  For Open Attachments\nCtrl+T  For Tab Transfer\n' +
            'Ctrl+alt  To Show ShortCut Keys Form\nCtrl+ArrowDown  For Focus On Detail Grid\nCtrl+ArrowUp  For Focus On warehouse Combo in Detail Box\n' +
            'Ctrl+ArrowRight  For Focus From One Grid To Another\nCtrl+Enter  For Update Record When Focus On Any Grid \n' +
            'Ctrl+Space  To Call Function\'s On Button Or Link When Focus On Any Grid ');
    };

    // ------------------------------------------------------------------ history
    P.btnshow = function (btn) {                                                                // HistoryGridFill() :2911
        var ref = document.querySelector('input[name="histRef"]:checked');
        return HRM.busy(btn || 'btnshow', function () {
            return post('/history', PPB.historyFilters({ supplierId: HRM.comboVal('cmbSupplierNameHistory'), actionId: ref ? HRM.int(ref.value) : 0 }))
                .then(function (rows) { GrdHistory.set(rows || []); GrdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    };
    P.btnNewHistory = function () {                                                             // btnNewHistory_Click :2869 - both dates today
        HRM.setVal('FromDateHistory', HRM.today()); HRM.setVal('ToDateHistory', HRM.today());
        HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', '');
        HRM.setCombo('cmbSupplierNameHistory', 0);
        GrdHistory.clear(); GrdHistoryDetail.clear();
    };
    P.btnRefreshHistory = function (btn) {                                                      // btnRefreshHistory_Click -> HistoryComboFill()
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return get('/history-suppliers').then(function (rows) { HRM.fill('cmbSupplierNameHistory', rows || [], 'Id', 'name', { zero: '', keep: true }); }).catch(HRM.fail);
        });
    };
    P.btnGrnFormHistory = function () {                                                         // btnGrnFormHistory_Click :3683 -> frmGRNHistory (screen 477)
        if (S.rights.grnHistoryView) HRM.open('/purchase/reports/grn-register');
        else HRM.box('Please Check Screen Rights');
    };

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { HRM.focus(name === 'history' ? 'FromDateHistory' : 'DocDate'); });   // tabControl1_SelectedIndexChanged
    HRM.footer(function () { PPB.showTab('history'); });
    HRM.$('combitem').addEventListener('change', function () { packUomFill(); deductionPolicyForGrn(); });   // combitem ValueChanged / Leave
    function searchModeChanged() {                                                              // rdSearchByName_CheckedChanged
        if (!(S.lists.items || []).length) return;
        var v = HRM.comboVal('combitem'); itemFill(false); HRM.setCombo('combitem', v); HRM.focus('combitem');
    }
    HRM.$('rdSearchByName').addEventListener('change', searchModeChanged);
    HRM.$('rdSearchByCode').addEventListener('change', searchModeChanged);
    PPB.leave('combpckuom', function () { totalWeight(); total(); });                          // combpckuom_Leave
    PPB.leave('combpcktyp', function () {                                                       // combpcktyp_Leave :1142
        if (S.orderExist) return;
        var t = HRM.comboVal('combpcktyp');
        if (t === 1) HRM.setVal('txtebu', PPB.net(S.cfg.juteBagsCut));
        if (t === 2) HRM.setVal('txtebu', PPB.net(S.cfg.ppBagsCut));
        if (t === 5) HRM.setVal('txtebu', PPB.net(S.cfg.openBulkCut));
        total();
    });
    PPB.leave('CmbDeliveryTerm', function () { stockWeightUpdate(); });                        // CmbDeliveryTerm TextChanged / Leave
    HRM.$('txtqty').addEventListener('input', function () { totalWeight(); total(); });         // txtqty_TextChanged
    HRM.$('txtgwt').addEventListener('input', function () { total(); });
    HRM.$('txtebu').addEventListener('input', function () { total(); });
    HRM.$('txtEmptyBagsTotal').addEventListener('input', function () { total(); });
    HRM.$('txtadlswt').addEventListener('input', function () { total(); });
    HRM.$('txtadlswt').addEventListener('blur', function () { total(); });
    HRM.$('txtsuppwt').addEventListener('input', function () {                                  // txtsuppwt_TextChanged: Fact Weight follows
        HRM.setVal('txtfctwt', HRM.val('txtsuppwt').trim());
        totalSupplierWeight(); stockWeightUpdate();
    });
    HRM.$('txtfctwt').addEventListener('input', function () { totalSupplierWeight(); stockWeightUpdate(); });
    ['txtgwt', 'txtebu', 'txtqty', 'txtEmptyBagsTotal', 'txtsuppwt', 'txtfctwt'].forEach(function (id) { PPB.guard(id, 'dec'); });
    PPB.guard('txtadlswt', 'signed'); PPB.guard('txtcarramount', 'int'); PPB.guard('txtGpNo', 'int');
    PPB.guard('txtFromDocNoHistory', 'int'); PPB.guard('txtToDocNoHistory', 'int');
    HRM.setVal('DocDate', HRM.today());
    PPB.resetHistoryFilters(3);
    function focusGrid(id) { var t = HRM.$(id); var tr = t && t.querySelector('tbody tr'); if (tr) tr.focus(); }
    function formTab() { return PPB.currentTab() === 'form'; }
    function canClick(id) { return HRM.visible(id) && !HRM.$(id).disabled; }
    HRM.$('grd').addEventListener('keydown', function (e) {                                     // grd_KeyDown :3425
        var r = grd.current(); if (!r || !e.ctrlKey) return;
        if (e.key === ' ') { e.preventDefault(); if (HRM.ask('Are you sure to Delete?')) { grd.remove(grd.currentIndex()); } }
        else if (e.key === 'Enter') { e.preventDefault(); grdDoubleClick(grd.currentIndex()); }
    });
    /* grdEmptyBags_KeyDown :3505 ("+") and GrdHistory_KeyDown :3526 (Edit / Print / WagesNo / NoOfAttachments): Ctrl+Space on
       the focused button or code link runs it. */
    ['grdEmptyBags', 'GrdHistory'].forEach(function (id) {
        HRM.$(id).addEventListener('keydown', function (e) {
            if (!e.ctrlKey || e.code !== 'Space') return;
            var t = e.target.closest && e.target.closest('button[data-act], a.hrm-code');
            if (t) { e.preventDefault(); t.click(); }
        });
    });
    HRM.$('GrdHistory').addEventListener('keydown', function (e) {                              // GrdHistory_KeyDown :3526 (Ctrl+Enter)
        var r = GrdHistory.current(); if (!r || !e.ctrlKey || e.key !== 'Enter') return;
        e.preventDefault(); editFromHistory(r);
    });
    HRM.keys({                                                                                  // InvFrmGRN_KeyDown :3310
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+alt+alt': P.shortcuts, 'ctrl+alt+control': P.shortcuts,
        'ctrl+n': function () { if (formTab()) P.btnNew(); else P.btnNewHistory(); },
        'ctrl+r': function () { if (!formTab()) P.btnRefreshHistory(); },
        'ctrl+s': function () { if (!formTab()) P.btnshow(); else if (canClick('btnSave')) P.btnSave(); },
        'ctrl+u': function () { if (formTab() && canClick('btnUpdate')) P.btnUpdate(); },
        'ctrl+shift+delete': function () { if (formTab() && canClick('btnDelete')) P.btnDelete(); },
        'ctrl+h': function () { if (!formTab()) P.btnGrnFormHistory(); },
        'ctrl+p': function () {
            if (formTab()) { if (canClick('BtnPrint')) P.btnPrint(); }
            else { var r = GrdHistory.current(); if (r) generateReport(HRM.int(r.Id)); }
        },
        'ctrl+f5': function () { if (formTab()) HRM.focus('DocDate'); },
        'ctrl+f10': function () { if (formTab()) P.btnAttachment(); },                          // Ctrl+F10 "For Open Attachments
        'ctrl+arrowdown': function () { focusGrid(formTab() ? 'grd' : 'GrdHistory'); },
        'ctrl+arrowup': function () { HRM.focus(formTab() ? 'CmbWareHouse' : 'FromDateHistory'); },
        'ctrl+arrowright': function () {
            if (!formTab()) return;
            var inGrd = document.activeElement && document.activeElement.closest && document.activeElement.closest('#grd');
            focusGrid(inGrd ? 'grdEmptyBags' : 'grd');
        }
    });
    load();
})();
