/* ============================================================================================
 * countx_pp_grn_gdn_store.js - Party Processing GRN / GDN Store (frmGrnGdnStorePartyProcessing.cs)
 *   ?mode=grn -> 679, DocumentTypeId 44 (gate pass 54)      ?mode=gdn -> 680, DocumentTypeId 45 (gate pass 55)
 * Follows the form: Load, UOMFill on item change, detail add / edit / update / delete (removed saved rows are sent with
 * ActionTypeId 3), gate pass Load, Insert(), Reset(), ReadById, history + detail of the selected row, keys.
 * Exports window.PpSt.
 * ============================================================================================ */
(function () {
    'use strict';
    var MODE = document.body.getAttribute('data-mode') || 'grn';
    var DOC = MODE === 'gdn' ? 45 : 44;
    var API = '/api/party-processing/grn-gdn-store';
    var P = {}; window.PpSt = P;
    var S = { rights: {}, recId: 0, updateIndex: -1, removed: [], uoms: [], pending: [] };

    function q(extra) { var o = { mode: MODE }; Object.keys(extra || {}).forEach(function (k) { o[k] = extra[k]; }); return o; }
    function get(path, extra) { return HRM.get(API + path, q(extra)); }
    function post(path, body, extra) { return HRM.post(API + path + '?' + new URLSearchParams(q(extra)).toString(), body); }
    function dt12(v) {
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'), t = HRM.time(v) || '00:00', h = +t.substring(0, 2);
        return p[2] + '-' + p[1] + '-' + p[0] + ' ' + String(h % 12 === 0 ? 12 : h % 12).padStart(2, '0') + ':' + t.substring(3, 5) + ' ' + (h < 12 ? 'AM' : 'PM');
    }

    // ------------------------------------------------------------------ grids
    var grd = new HRM.Grid('grd', {                                                            // grdSetting()
        columns: [
            { key: 'Delete', caption: 'X', width: 20, render: function () { return PPB.btnCell('delete', 'X'); } },
            { key: 'Edit', caption: 'Edit', width: 50, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Id', caption: 'Id', type: 'int' },
            { key: 'WareHouseId', hidden: true },
            { key: 'WareHouse', caption: 'WareHouse' },
            { key: 'ItemId', hidden: true },
            { key: 'Item', caption: 'Item Name' },
            { key: 'UOMId', hidden: true },
            { key: 'UOM', caption: 'Pack Uom' },
            PPB.numCol('Qty', 'Qty', '#,##0.###'),
            { key: 'Remarks', caption: 'Remarks' }
        ],
        totals: true, emptyText: '',
        onDouble: function (r, i) { grdDoubleClick(i); }
    });
    PPB.onButton(grd, 'grd', function (r, act, i) {                                           // grd_ColumnButtonClick
        if (act === 'edit') { grdDoubleClick(i); return; }
        if (HRM.int(r.Id) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            S.removed.push({ Id: r.Id, WareHouseId: r.WareHouseId, ItemId: r.ItemId, UOM: r.UOM, Qty: r.Qty, Remarks: r.Remarks });
        }
        grd.remove(i);
    });
    var grdGatePass = new HRM.Grid('grdGatePass', {                                            // grdGatePassSetting()
        columns: [
            { key: 'Load', caption: 'Load', width: 50, render: function () { return PPB.btnCell('load', 'Load'); } },
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'StockPartyId', hidden: true },
            { key: 'SupplierCustomerId', hidden: true },
            { key: 'GpSrNo', caption: 'GpSrNo', type: 'int' },
            { key: 'GpDate', caption: 'GpDate', type: 'date' },
            { key: 'VehicleType', caption: 'VehicleType' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' },
            { key: 'OtherRemarks', hidden: true },
            { key: 'Freight', caption: 'Freight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'ItemQty', caption: 'ItemQty', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'SupplierWeight', caption: 'SupplierWeight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'FactoryWeight', caption: 'FactoryWeight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'DifferenceWeight', caption: 'DifferenceWeight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'PartyName', caption: 'PartyName' }, { key: 'ReferencePartyName', caption: 'ReferencePartyName' },
            { key: 'DocumentTypeDescription', hidden: true }
        ],
        emptyText: ''
    });
    PPB.onButton(grdGatePass, 'grdGatePass', function (r) {                                    // grdGatePass_ColumnButtonClick
        if (S.recId !== 0) { HRM.box('Reset Form First'); return; }
        HRM.fill('cmbReferenceDocument', [{ Id: r.DocumentTypeId, Name: r.DocumentTypeDescription }], 'Id', 'Name', { zero: false });
        HRM.fill('cmbDocumentNo', [{ Id: r.Id, Name: r.GpSrNo }], 'Id', 'Name', { zero: false });
        HRM.setVal('txtVehicleNo', HRM.str(r.VehicleNo));
        HRM.setVal('txtBiltyNo', HRM.str(r.BiltyNo));
        HRM.setCombo('cmbStockParty', r.StockPartyId);
        HRM.setCombo('cmbReferenceParty', r.SupplierCustomerId);
    });
    var grdHistory = new HRM.Grid('grdHistory', {                                              // grdHistorySetting()
        columns: [
            { key: 'Print', caption: 'Print', width: 50, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'Edit', caption: 'Edit', width: 50, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DocNo', caption: 'DocNo', type: 'int' }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
            { key: 'StockParty', caption: 'StockParty' }, { key: 'ReferenceParty', caption: 'ReferenceParty' },
            { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' }, { key: 'RefDocement', caption: 'RefDocement' },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'ModifyDate', caption: 'ModifyDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'ApprovedDate', caption: 'ApprovedDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }, { key: 'Remarks', caption: 'Remarks' }
        ],
        filterRow: true, emptyText: 'No record found.',
        onDouble: function (r) { reset().then(function () { readById(HRM.int(r.Id)); }); },       // grdHistory_DoubleClick
        onSelect: function (r) { historyDetail(HRM.int(r.Id)); }                                  // grdHistory_SelectionChanged
    });
    PPB.onButton(grdHistory, 'grdHistory', function (r, act) {                                 // grdHistory_ColumnButtonClick
        if (act === 'edit') {
            if (S.rights.update === false) { HRM.box("you don't have update rights..."); return; }
            reset().then(function () { readById(HRM.int(r.Id)); });
        } else if (act === 'print' && S.rights.gridPrint !== false) printSlip(HRM.int(r.Id), null, true);
    });
    var grdHistoryDetail = new HRM.Grid('grdHistoryDetail', {
        columns: [{ key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemName', caption: 'ItemName' }, { key: 'PackUom', caption: 'PackUom' },
            PPB.numCol('Qty', 'Qty', '#,##0.###'), { key: 'Remarks', caption: 'Remarks' }],
        totals: true, emptyText: ''
    });

    // ------------------------------------------------------------------ lists
    function fillLists(o, keep) {
        HRM.fill('cmbStockParty', o.stockParties || [], 'Id', 'CompanyName', { zero: '', keep: keep });
        HRM.fill('cmbReferenceParty', o.refParties || [], 'Id', 'ReferencePartyName', { zero: '', keep: keep });
        HRM.fill('cmbWareHouse', o.warehouses || [], 'Id', 'WareHouseName', { zero: '', keep: keep });
        HRM.fill('cmbItem', o.items || [], 'Id', 'ItemName', { zero: '', keep: keep });
    }
    /** UOMFill(ItemId): keeps the typed pack UOM text when it is in the new list. */
    function uomFill(itemId) {
        var prev = HRM.comboText('cmbUOM');
        if (!itemId) { S.uoms = []; HRM.fill('cmbUOM', [], 'Id', 'UOMCode', { zero: false }); HRM.$('cmbUOM').selectedIndex = -1; HRM.refreshCombos(); return Promise.resolve(); }
        return get('/uoms', { itemId: itemId }).then(function (rows) {
            S.uoms = rows || [];
            HRM.fill('cmbUOM', S.uoms, 'Id', 'UOMCode', { zero: false });
            HRM.$('cmbUOM').selectedIndex = -1;
            if (prev) HRM.setComboText('cmbUOM', prev);
            HRM.refreshCombos();
        }).catch(HRM.fail);
    }
    function uomSelected() { var s = HRM.$('cmbUOM'); return s.selectedIndex >= 0 && s.value !== ''; }

    // ------------------------------------------------------------------ detail
    /** DetailFormValidation() */
    function detailValid() {
        if (!HRM.comboVal('cmbWareHouse')) { HRM.box('WareHouse Filed Required'); HRM.focus('cmbWareHouse'); return false; }
        if (!HRM.comboVal('cmbItem')) { HRM.box('Item Filed Required'); HRM.focus('cmbItem'); return false; }
        if (!uomSelected()) { HRM.box('Pack Uom Filed Required'); HRM.focus('cmbUOM'); return false; }
        if (HRM.val('txtQty') === '') { HRM.box('Qty Filed Required'); HRM.focus('txtQty'); return false; }
        return true;
    }
    function detailRow(id) {
        return { Id: id, WareHouseId: HRM.comboVal('cmbWareHouse'), WareHouse: HRM.comboText('cmbWareHouse'),
            ItemId: HRM.comboVal('cmbItem'), Item: HRM.comboText('cmbItem'), UOMId: HRM.int(HRM.val('cmbUOM')), UOM: HRM.comboText('cmbUOM'),
            Qty: HRM.num(HRM.val('txtQty')), Remarks: HRM.val('txtRemarksDetail') };
    }
    /** ResetDetail() */
    function resetDetail() {
        HRM.setCombo('cmbWareHouse', 0); HRM.setCombo('cmbItem', 0);
        S.uoms = []; HRM.fill('cmbUOM', [], 'Id', 'UOMCode', { zero: false }); HRM.$('cmbUOM').selectedIndex = -1; HRM.refreshCombos();
        HRM.setVal('txtQty', ''); HRM.setVal('txtRemarksDetail', '');
        HRM.show('btnAddDetail', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelDetail', false);
    }
    P.btnAddDetail = function () {                                                              // btnAddDetail_Click
        if (!detailValid()) return;
        grd.add(detailRow(0));
        resetDetail(); HRM.focus('cmbWareHouse');
    };
    P.btnUpdateDetail = function () {                                                           // btnUpdateDetail_Click
        if (!detailValid()) return;
        var old = grd.rows()[S.updateIndex]; if (!old) return;
        grd.update(S.updateIndex, detailRow(old.Id));
        resetDetail(); HRM.focus('cmbWareHouse');
    };
    P.btnCancelDetail = function () { resetDetail(); };
    function grdDoubleClick(i) {                                                               // grd_DoubleClick
        var r = grd.rows()[i]; if (!r) return;
        S.updateIndex = i;
        HRM.setCombo('cmbWareHouse', r.WareHouseId);
        HRM.setCombo('cmbItem', r.ItemId);
        uomFill(r.ItemId).then(function () { HRM.setCombo('cmbUOM', r.UOMId); });
        HRM.setVal('txtQty', PPB.fmt(r.Qty, '#,##0.###'));
        HRM.setVal('txtRemarksDetail', HRM.str(r.Remarks));
        HRM.focus('cmbWareHouse');
        HRM.show('btnAddDetail', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelDetail', true);
    }

    // ------------------------------------------------------------------ load / reset / read
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); HRM.show('btnDelete', update); }
    function load() {
        HRM.show('PrinGdn', DOC === 45); HRM.show('btnprint', DOC === 44);
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            HRM.applyRights(S.rights, { save: 'btnsave', print: 'btnprint', gridPrint: 'PrinGdn', update: 'btnupdate', delete: 'btnDelete' });
            fillLists(o, false);
            HRM.setVal('txtDocNo', o.docNo > 0 ? o.docNo : '');
            S.pending = o.pending || []; grdGatePass.set(S.pending);
            HRM.fill('cmbstockpartyhistory', o.historyParties || [], 'Id', 'name', { zero: '' });
            buttons(false);
            HRM.focus('txtDocDate');
        }).catch(HRM.fail);
    }
    /** Reset() */
    function reset() {
        S.removed = [];
        HRM.setVal('txtDocDate', HRM.today());
        HRM.setVal('txtDocNo', '');
        HRM.setCombo('cmbStockParty', 0); HRM.setCombo('cmbReferenceParty', 0);
        HRM.setVal('txtVehicleNo', ''); HRM.setVal('txtBiltyNo', '');
        HRM.fill('cmbReferenceDocument', [], 'Id', 'Name', { zero: false });
        HRM.fill('cmbDocumentNo', [], 'Id', 'Name', { zero: false });
        HRM.setVal('txtRemarks', '');
        S.recId = 0; buttons(false);
        grd.clear();
        return Promise.all([
            get('/code').then(function (o) { HRM.setVal('txtDocNo', o.docNo > 0 ? o.docNo : ''); }),
            get('/pending').then(function (rows) { S.pending = rows || []; grdGatePass.set(S.pending); })
        ]).then(function () { HRM.focus('txtDocNo'); }).catch(HRM.fail);
    }
    /** ReadById(Id) */
    function readById(id) {
        return HRM.loading(get('/by-id', { id: id })).then(function (o) {
            S.recId = HRM.int(o.Id); S.removed = [];
            HRM.setVal('txtDocDate', HRM.day(o.DocDate));
            HRM.setVal('txtDocNo', HRM.str(o.DocNo));
            HRM.setCombo('cmbStockParty', o.StockPartyId);
            HRM.setCombo('cmbReferenceParty', o.ReferencePartyId);
            HRM.setVal('txtVehicleNo', HRM.str(o.VehicleNo)); HRM.setVal('txtBiltyNo', HRM.str(o.BiltyNo));
            HRM.fill('cmbReferenceDocument', [{ Id: o.RefDocumentTypeId, Name: o.DocumentTypeDescription }], 'Id', 'Name', { zero: false });
            HRM.fill('cmbDocumentNo', [{ Id: o.RefEntryIdNo, Name: o.RefDoNo }], 'Id', 'Name', { zero: false });
            HRM.setVal('txtRemarks', HRM.str(o.RemarksHeader));
            grd.set((o.details || []).map(function (d) {
                return { Id: d.Id, WareHouseId: d.WarehouseId, WareHouse: d.WareHouseName, ItemId: d.ItemId, Item: d.ItemName,
                    UOMId: d.ItemUomId, UOM: d.UOMCode, Qty: HRM.num(d.ItemQty), Remarks: d.RemarksSub };
            }));
            PPB.showTab('form');
            buttons(true);
        }).catch(HRM.fail);
    }
    function historyDetail(id) {
        return get('/by-id', { id: id }).then(function (o) {
            grdHistoryDetail.set((o.details || []).map(function (d) {
                return { WareHouse: d.WareHouseName, ItemName: d.ItemName, PackUom: d.UOMCode, Qty: HRM.num(d.ItemQty), Remarks: d.RemarksSub };
            }));
        }).catch(function () { grdHistoryDetail.clear(); });
    }

    // ------------------------------------------------------------------ save / delete / print
    function insert(btn) {
        if (!HRM.ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkBox');
        var body = {
            id: S.recId, docNo: HRM.val('txtDocNo'), docDate: HRM.val('txtDocDate'),
            stockPartyId: HRM.comboVal('cmbStockParty'), referencePartyId: HRM.comboVal('cmbReferenceParty'),
            vehicleNo: HRM.val('txtVehicleNo'), biltyNo: HRM.val('txtBiltyNo'),
            refDocumentTypeId: HRM.int(HRM.val('cmbReferenceDocument')), refEntryIdNo: HRM.int(HRM.val('cmbDocumentNo')),
            remarks: HRM.val('txtRemarks'), rows: grd.rows(), removed: S.removed
        };
        return HRM.busy(btn, function () {
            return post('/save', body).then(function (res) {
                HRM.box(res.message);
                return reset().then(function () { if (preview) printSlip(res.id, null, false); });
            }).catch(HRM.fail);
        });
    }
    function printSlip(id, btn, grid) {
        return HRM.busy(btn, function () {
            return get('/slip', { id: id, grid: !!grid }).then(function (rows) {
                PPB.printRows(DOC === 44 ? '332 GRN Store Party Processing Slip' : '332_01 GDN Store Party Processing Slip', rows);
            }).catch(HRM.fail);
        }, 'st-print');
    }
    P.btnsave = function (btn) { S.recId = 0; return insert(btn || 'btnsave'); };
    P.btnupdate = function (btn) { if (S.recId === 0) { HRM.box('Record not found'); return; } return insert(btn || 'btnupdate'); };
    P.btnnew = function () { resetDetail(); return reset(); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'toolStripButton1', function () { return get('/refresh').then(function (o) { fillLists(o, true); }).catch(HRM.fail); });
    };
    P.btnDelete = function (btn) {                                                              // btnDelete_Click
        if (S.recId === 0) { HRM.box('Record Id Not Found....'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return post('/delete', {}, { id: S.recId }).then(function (res) { HRM.box(res.message); return reset(); }).catch(HRM.fail);
        });
    };
    P.btnprint = function (btn) { return printSlip(S.recId, btn || 'btnprint', false); };       // GenerateReport(RecId)
    P.PrinGdn = function (btn) { return printSlip(S.recId, btn || 'PrinGdn', true); };
    P.shortcuts = function () {
        HRM.box('Ctrl+S  For Save\nCtrl+U  For Update\nCtrl+E  For Close\nCtrl+R  For Refresh\nCtrl+N  For New\nCtrl+P  For Print\n' +
            'Ctrl+F5  For Focus on Doc Date\nCtrl+T  For Tab Transfer\nCtrl+alt  To Show ShortCut Keys Form\nCtrl+ArrowDown  For Focus On Detail Grid\n' +
            'Ctrl+ArrowUp  For Focus On warehouse Combo in Detail Box\nCtrl+ArrowRight  For Focus From One Grid To Another\n' +
            'Ctrl+Enter  For Update Record When Focus On Any Grid\nCtrl+Space  To Call Function\'s On Button Or Link When Focus On Any Grid');
    };

    // ------------------------------------------------------------------ history
    P.btnShow = function (btn) {                                                                // GridFill()
        return HRM.busy(btn || 'btnShow', function () {
            return post('/history', PPB.historyFilters({ stockPartyId: HRM.comboVal('cmbstockpartyhistory') })).then(function (rows) {
                grdHistory.set(rows || []); grdHistoryDetail.clear();
            }).catch(HRM.fail);
        });
    };
    P.btnResetHistory = function () {                                                           // btnResetHistory_Click
        PPB.resetHistoryFilters(3, ['cmbstockpartyhistory']);
        grdHistory.clear(); grdHistoryDetail.clear();
        HRM.focus('FromDateHistory');
    };
    P.btnRefreshHistory = function (btn) {                                                      // StockPartyFillHistory
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return get('/history-parties').then(function (rows) { HRM.fill('cmbstockpartyhistory', rows || [], 'Id', 'name', { zero: '', keep: true }); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { HRM.focus(name === 'history' ? 'FromDateHistory' : 'txtDocDate'); });
    HRM.footer(function () { PPB.showTab('history'); });
    HRM.$('cmbItem').addEventListener('change', function () { uomFill(HRM.comboVal('cmbItem')); });   // cmbItem_ValueChanged
    PPB.guard('txtQty', 'dec'); PPB.guard('txtFromDocNoHistory', 'int'); PPB.guard('txtToDocNoHistory', 'int');
    HRM.setVal('txtDocDate', HRM.today());
    PPB.resetHistoryFilters(3);
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+alt+alt': P.shortcuts, 'ctrl+alt+control': P.shortcuts,
        'ctrl+n': function () { if (PPB.currentTab() === 'history') P.btnResetHistory(); else P.btnnew(); },
        'ctrl+r': function () { if (PPB.currentTab() === 'history') P.btnRefreshHistory(); },
        'ctrl+s': function () { if (PPB.currentTab() === 'history') P.btnShow(); else if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
        'ctrl+u': function () { if (PPB.currentTab() === 'form' && HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); },
        'ctrl+p': function () { if (PPB.currentTab() === 'form' && HRM.visible('btnprint') && !HRM.$('btnprint').disabled) P.btnprint(); },
        'ctrl+f5': function () { HRM.focus('txtDocDate'); },
        'ctrl+arrowup': function () { if (PPB.currentTab() === 'form') HRM.focus('cmbWareHouse'); }
    });
    load();
})();
