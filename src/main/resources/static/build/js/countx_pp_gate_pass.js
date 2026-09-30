/* ============================================================================================
 * countx_pp_gate_pass.js - Party Processing Gate Pass (GatePassInwardPartyProcessing.cs)
 *   ?mode=inward  -> 681, DocumentTypeId 54      ?mode=outward -> 682, DocumentTypeId 55
 * Each handler follows the desktop form: Load, cmbgptype_Leave, CmbAdvanceDo_Leave, Total(), formvalidation (server),
 * Insert(), Reset(), getUpdate() + GetFactoryWeightFromWb(), grids' Edit / Print buttons, history, keys.
 * Exports window.PpGp.
 * ============================================================================================ */
(function () {
    'use strict';
    var MODE = document.body.getAttribute('data-mode') || 'inward';
    var DOC = MODE === 'outward' ? 55 : 54;
    var API = '/api/party-processing/gate-pass';
    var P = {}; window.PpGp = P;
    var S = { rights: {}, recId: 0, gpTypes: [], ados: [], items: [], stockParties: [], adoExpiry: null };

    function q(extra) { var o = { mode: MODE }; Object.keys(extra || {}).forEach(function (k) { o[k] = extra[k]; }); return o; }
    function get(path, extra) { return HRM.get(API + path, q(extra)); }
    function post(path, body, extra) { return HRM.post(API + path + '?' + new URLSearchParams(q(extra)).toString(), body); }

    // ------------------------------------------------------------------ grids (grdfrmsetting / grdhistorysetting)
    function columns(itemCaption) {
        return [
            { key: 'Print', caption: 'Print', width: 50, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'Edit', caption: 'Edit', width: 50, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Id', hidden: true },
            { key: 'GpSrNo', caption: 'GpSrNo', type: 'int' },
            { key: 'GpDate', caption: 'GpDate', type: 'date' },
            { key: 'GatepassType', caption: 'GatepassType' },
            { key: 'StockParty', caption: 'StockParty' },
            { key: 'ReferenceParty', caption: 'ReferenceParty' },
            { key: 'City', caption: 'City' },
            { key: 'VarietyName', caption: itemCaption },
            { key: 'Qty', caption: 'Qty', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'VehicleType', caption: 'VehicleType' },
            { key: 'VehicleNo', caption: 'VehicleNo' },
            { key: 'BiltyNo', caption: 'BiltyNo' },
            { key: 'InTime', caption: 'InTime', render: function (v) { return HRM.esc(time12(v)); } },
            { key: 'OutTime', caption: 'OutTime', render: function (v) { return HRM.esc(time12(v)); } },
            { key: 'Freight', caption: 'Freight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'Status', caption: 'Status' },
            { key: 'SupplierWeight', caption: 'SupplierWeight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'FactoryWeight', caption: 'FactoryWeight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'DifferenceWeight', caption: 'DifferenceWeight', type: 'num', render: function (v) { return HRM.esc(PPB.net(v)); } },
            { key: 'WeighBridgeStatus', caption: 'WeighBridgeStatus' },
            { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' },
            { key: 'Remarks', caption: 'Remarks' }
        ];
    }
    function time12(v) {
        var t = HRM.time(v); if (!t) return '';
        var h = +t.substring(0, 2), m = t.substring(3, 5);
        return String(h % 12 === 0 ? 12 : h % 12).padStart(2, '0') + ':' + m + ' ' + (h < 12 ? 'AM' : 'PM');
    }
    function dt12(v) { var d = HRM.fmtDate(v); return d ? d + ' ' + time12(v) : ''; }

    var grd = new HRM.Grid('grd', { columns: columns('VarietyName'), filterRow: true, emptyText: '',
        onDouble: function (r) { gridEdit(r, false); } });
    var grdhistory = new HRM.Grid('grdhistory', { columns: columns('Item Name'), filterRow: true, emptyText: 'No record found.',
        onDouble: function (r) { gridEdit(r, false); } });
    PPB.onButton(grd, 'grd', function (r, act) {                                  // grd_ColumnButtonClick
        if (act === 'edit') gridEdit(r, false);
        else if (act === 'print' && S.rights.print !== false) printSlip(HRM.int(HRM.col(r, 'Id')), null);
    });
    PPB.onButton(grdhistory, 'grdhistory', function (r, act) {                    // grdhistory_ColumnButtonClick
        if (act === 'edit') gridEdit(r, true);
        else if (act === 'print' && S.rights.print !== false) printSlip(HRM.int(HRM.col(r, 'Id')), null);
    });

    // ------------------------------------------------------------------ combos
    function fillFixed() {
        HRM.fillFixed('CmbStatus', [[1, 'Open'], [2, 'Accepted'], [3, 'Rejected']]);          // Status(): Rows[0] active
        HRM.fillFixed('cmbWeighBridge', [[1, 'Auto'], [2, 'Manual']]);                         // cmbWeighBridgeFill(): Rows[0] active
    }
    function fillLists(o, keep) {
        S.gpTypes = o.gpTypes || [];
        HRM.fill('cmbgptype', S.gpTypes, 'Id', 'type', { zero: '', keep: keep });
        S.stockParties = o.stockParties || [];
        HRM.fill('cmbStockParty', S.stockParties, 'Id', 'CompanyName', { zero: '', keep: keep });
        HRM.fill('cmbsupp', o.refParties || [], 'Id', 'ReferencePartyName', { zero: '', keep: keep });
        HRM.fill('cmbvehicletype', o.vehicleTypes || [], 'Id', 'VehicleDescription', { zero: '', keep: keep });
        HRM.fill('cmbcity', o.cities || [], 'Id', 'CityName', { zero: '', keep: keep });
        if (o.items) { S.items = o.items; HRM.fill('CmbVariety', S.items, 'Id', 'ItemName', { zero: '', keep: keep }); }
        HRM.fill('CmbStockPartyHistory', o.historyParties || [], 'Id', 'name', { zero: false });
        HRM.$('CmbStockPartyHistory').selectedIndex = -1; HRM.refreshCombos();
    }
    function gpTypeText() { return HRM.comboText('cmbgptype'); }

    /** cmbgptype_Leave */
    function gpTypeLeave(keepValues) {
        HRM.show('boxAdvanceDo', false);
        HRM.text('labelGpType', 'Gp Type');
        var t = gpTypeText();
        if (t === 'Packing Material' || t === 'Rice' || t === 'Paddy') {
            if (!keepValues) HRM.setCombo('CmbAdvanceDo', 0);
            if (t === 'Packing Material') HRM.enable('ChkIsWeighable', true);
            else { HRM.enable('ChkIsWeighable', false); HRM.check('ChkIsWeighable', true); }
            return get('/items', { gpType: t }).then(function (o) {                       // StockPartyBind(); ItemNameFill("7,8" | "1,2,4")
                S.stockParties = o.stockParties || [];
                HRM.fill('cmbStockParty', S.stockParties, 'Id', 'CompanyName', { zero: '', keep: true });
                S.items = o.items || [];
                HRM.fill('CmbVariety', S.items, 'Id', 'ItemName', { zero: '', keep: true });
            }).catch(HRM.fail);
        }
        if (DOC === 55 && t === 'AdvanceDO') {
            HRM.enable('ChkIsWeighable', false); HRM.check('ChkIsWeighable', true);
            HRM.show('boxAdvanceDo', true);
            HRM.text('labelGpType', 'Gp Type & ADO #');
            return advanceDoBind();
        }
        HRM.setCombo('CmbAdvanceDo', 0);
        S.items = []; HRM.fill('CmbVariety', [], 'Id', 'ItemName', { zero: '' });
        return Promise.resolve();
    }
    /** AdvanceDoBind(): USP_DeliveryOrder_GetAdvanceDo (GpRecId = RecId); BindAndRetainSelection with a default row. */
    function advanceDoBind() {
        return get('/advance-dos', { recId: S.recId }).then(function (rows) {
            S.ados = rows || [];
            HRM.fill('CmbAdvanceDo', S.ados, 'Id', 'DocNo', { zero: '', keep: true });
        }).catch(HRM.fail);
    }
    /** CmbAdvanceDo_Leave */
    function adoLeave() {
        var id = HRM.comboVal('CmbAdvanceDo'), qty = 0;
        var row = PPB.rowOf(S.ados, 'Id', id);
        var p;
        if (row && id > 0) {
            qty = HRM.num(HRM.col(row, 'BalQty'));
            p = get('/ado-data', { id: id }).then(function (o) {                         // BindDataFromADO(id)
                if (!o.found) return;
                S.items = o.items || [];
                HRM.fill('CmbVariety', S.items, 'Id', 'ItemName', { zero: false });
                HRM.setCombo('CmbVariety', o.itemId);
                HRM.setComboText('cmbvehicletype', o.vehicleType);
                HRM.setVal('txtvehicleno', o.vehicleNo);
                S.adoExpiry = o.expiryDate;
                S.stockParties = o.stockParties || [];
                HRM.fill('cmbStockParty', S.stockParties, 'Id', 'CompanyName', { zero: false });
            }).catch(HRM.fail);
        } else {
            S.stockParties = []; S.items = [];
            HRM.fill('cmbStockParty', [], 'Id', 'CompanyName', { zero: '' });
            HRM.fill('CmbVariety', [], 'Id', 'ItemName', { zero: '' });
            p = Promise.resolve();
        }
        HRM.setVal('txtqty', PPB.fmt(qty, '#,##.##'));
        return p;
    }
    /** Total(): Difference = Supplier - Factory when both boxes have text. */
    function total() {
        var s = HRM.val('txtSupplierWeight'), f = HRM.val('txtFactoryWeight');
        if (s !== '' && f !== '') HRM.setVal('txtDifferenceWeight', PPB.net(HRM.num(s.trim()) - HRM.num(f.trim())));
    }

    // ------------------------------------------------------------------ load / reset
    function buttons(update) {
        HRM.show('btnsave', !update); HRM.show('btnupdate', update); HRM.show('btnDelete', update);
    }
    function applyRights() {
        HRM.applyRights(S.rights, { save: 'btnsave', update: 'btnupdate', delete: 'btnDelete', print: 'btnPrint' });
    }
    function load() {
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            applyRights();
            fillFixed();
            fillLists(o, false);
            HRM.setVal('txtgpno', o.gpNo > 0 ? o.gpNo : '');
            if (o.defaultCityId) HRM.setCombo('cmbcity', o.defaultCityId);            // "City Area" configuration
            grd.set(o.openRows || []);
            if (DOC === 55) { HRM.show('rowSupplierWeight', false); HRM.show('rowDifferenceWeight', false); }
            HRM.focus('txtgpdate');
        }).catch(HRM.fail);
    }
    /** Reset() */
    function reset() {
        S.recId = 0;
        buttons(false);
        HRM.setCombo('cmbsupp', 0);
        HRM.setVal('txtvehicleno', ''); HRM.setVal('txtbiltyno', '');
        HRM.setCombo('CmbStatus', 1); HRM.setCombo('cmbWeighBridge', 1);
        HRM.setCombo('CmbVariety', 0);
        HRM.setVal('txtqty', ''); HRM.setVal('txtfreight', '');
        HRM.setVal('txtSupplierWeight', ''); HRM.enable('txtSupplierWeight', true);
        HRM.setVal('txtFactoryWeight', ''); HRM.setVal('txtDifferenceWeight', ''); HRM.setVal('txtremarks', '');
        HRM.enable('ChkIsWeighable', false); HRM.check('ChkIsWeighable', true);
        HRM.enable('cmbWeighBridge', true);
        HRM.focus('txtgpdate');
        return Promise.all([
            get('/code').then(function (o) { if (o.gpNo > 0) HRM.setVal('txtgpno', o.gpNo); }),      // gpnofill (keeps the old text on 0)
            get('/open').then(function (rows) { grd.set(rows || []); }),                           // grdfrmfill
            advanceDoBind()
        ]).catch(HRM.fail);
    }

    /** getUpdate(ID) + GetFactoryWeightFromWb() */
    function readById(id) {
        S.recId = id;
        return HRM.loading(get('/by-id', { id: id })).then(function (g) {
            HRM.setVal('txtbiltyno', HRM.col(g, 'BiltyNo'));
            HRM.setCombo('cmbcity', HRM.col(g, 'CityId'));
            HRM.setVal('txtfreight', PPB.net(HRM.col(g, 'Freight')));
            HRM.setComboText('cmbgptype', HRM.str(HRM.col(g, 'GatepassType')));
            S.ados = g.advanceDos || [];
            HRM.fill('CmbAdvanceDo', S.ados, 'Id', 'DocNo', { zero: '' });
            HRM.setCombo('CmbAdvanceDo', HRM.int(HRM.col(g, 'AdvanceDeliveryOrderId')));
            var t = HRM.str(HRM.col(g, 'GatepassType'));
            var lists = (DOC === 55 && t === 'AdvanceDO' && HRM.int(HRM.col(g, 'AdvanceDeliveryOrderId')) > 0)
                ? (HRM.show('boxAdvanceDo', true), HRM.text('labelGpType', 'Gp Type & ADO #'), adoLeave())
                : gpTypeLeave(true);
            return Promise.resolve(lists).then(function () {
                HRM.setVal('txtgpdate', HRM.day(HRM.col(g, 'GpDate')));
                HRM.setVal('txtgpno', HRM.col(g, 'GpSrNo'));
                HRM.setVal('txtqty', PPB.net(HRM.col(g, 'ItemQty')));
                HRM.setVal('txtremarks', HRM.str(HRM.col(g, 'OtherRemarks')));
                HRM.setVal('txtvehicleno', HRM.str(HRM.col(g, 'VehicleNo')));
                HRM.setComboText('cmbvehicletype', HRM.str(HRM.col(g, 'VehicleType')));
                HRM.setCombo('cmbsupp', HRM.int(HRM.col(g, 'SupplierCustomerId')));
                HRM.setCombo('cmbStockParty', HRM.int(HRM.col(g, 'StockPartyId')));
                var st = HRM.str(HRM.col(g, 'Status'));
                HRM.setComboText('CmbStatus', st);
                if (st === 'Open') HRM.setCombo('CmbStatus', 3);                           // CmbStatus.Rows[2].Activate() (desktop)
                HRM.check('ChkIsWeighable', HRM.bool(HRM.col(g, 'IsWeighable')));
                HRM.setCombo('CmbVariety', HRM.int(HRM.col(g, 'ItemId')));
                HRM.setVal('txtSupplierWeight', PPB.net(HRM.col(g, 'SupplierWeight')));
                HRM.setVal('txtFactoryWeight', PPB.net(HRM.col(g, 'FactoryWeight')));
                var fw = HRM.val('txtFactoryWeight');
                HRM.enable('cmbWeighBridge', !(fw !== '' && fw !== '0'));
                HRM.setComboText('cmbWeighBridge', HRM.str(HRM.col(g, 'WeighBridgeStatus')));
                HRM.setVal('txtDifferenceWeight', PPB.net(HRM.col(g, 'DifferenceWeight')));
                PPB.showTab('form');
                buttons(true);
                // GetFactoryWeightFromWb()
                if (g.wbFound) {
                    HRM.setVal('txtFactoryWeight', g.wbNetWeight);
                    if (DOC === 55) HRM.setVal('txtSupplierWeight', g.wbNetWeight);
                    if (g.wbNetWeight !== '' && g.wbNetWeight !== '0') HRM.enable('cmbWeighBridge', false);
                } else HRM.setVal('txtFactoryWeight', '0');
                total();
            });
        }).catch(HRM.fail);
    }
    /** grd / grdhistory Edit: (history resets first) CmbStatus.Rows[2].Activate(); getUpdate; GetFactoryWeightFromWb. */
    function gridEdit(r, withReset) {
        var id = HRM.int(HRM.col(r, 'Id'));
        var go = function () { HRM.setCombo('CmbStatus', 3); return readById(id); };
        return withReset ? reset().then(go) : go();
    }

    // ------------------------------------------------------------------ save / delete / print
    function body() {
        return {
            id: S.recId, gpNo: HRM.val('txtgpno'), gpDate: HRM.val('txtgpdate'),
            gpTypeId: HRM.comboVal('cmbgptype'), adoId: HRM.visible('boxAdvanceDo') ? HRM.comboVal('CmbAdvanceDo') : 0,
            stockPartyId: HRM.comboVal('cmbStockParty'), refPartyId: HRM.comboVal('cmbsupp'), cityId: HRM.comboVal('cmbcity'),
            itemId: HRM.comboVal('CmbVariety'), itemName: HRM.comboText('CmbVariety'),
            qty: HRM.val('txtqty'), vehicleTypeId: HRM.comboVal('cmbvehicletype'), vehicleTypeText: HRM.comboText('cmbvehicletype'),
            vehicleNo: HRM.val('txtvehicleno'), biltyNo: HRM.val('txtbiltyno'), freight: HRM.val('txtfreight'),
            statusId: HRM.comboVal('CmbStatus'), statusText: HRM.comboText('CmbStatus'),
            wbStatusId: HRM.comboVal('cmbWeighBridge'), wbStatusText: HRM.comboText('cmbWeighBridge'),
            supplierWeight: HRM.val('txtSupplierWeight'), factoryWeight: HRM.val('txtFactoryWeight'), differenceWeight: HRM.val('txtDifferenceWeight'),
            isWeighable: HRM.checked('ChkIsWeighable'), remarks: HRM.val('txtremarks')
        };
    }
    function insert(btn) {
        if (!HRM.ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var preview = HRM.checked('ChkBox');
        return HRM.busy(btn, function () {
            return post('/save', body()).then(function (res) {
                HRM.box(res.message);
                return reset().then(function () { if (preview) printSlip(res.id, null); });   // Reset(); if (ChkBox.Checked) GenerateReport(success)
            }).catch(HRM.fail);
        });
    }
    function printSlip(id, btn) {
        return HRM.busy(btn, function () {
            return get('/slip', { id: id }).then(function (rows) {
                PPB.printRows((DOC === 54 ? '299 Inward' : '299 Outward') + ' Gate Pass Slip', rows);
            }).catch(HRM.fail);
        }, 'gp-print');
    }

    P.btnsave = function (btn) { S.recId = 0; return insert(btn || 'btnsave'); };                   // btnsave_Click
    P.btnupdate = function (btn) {                                                                 // btnupdate_Click
        if (S.recId === 0) { HRM.box('Record not found'); return; }
        return insert(btn || 'btnupdate');
    };
    P.btnnew = function () { return reset(); };
    P.btnRefresh = function (btn) {                                                                // btnRefresh_Click
        return HRM.busy(btn || 'btnRefresh', function () {
            return get('/refresh').then(function (o) { fillLists(o, true); grd.set(o.openRows || []); }).catch(HRM.fail);
        });
    };
    P.btnDelete = function (btn) {                                                                 // btnDelete_Click
        if (S.recId <= 0) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return post('/delete', {}, { id: S.recId }).then(function (res) { HRM.box(res.message); return reset(); }).catch(HRM.fail);
        });
    };
    P.btnPrint = function (btn) {                                                                  // btnPrint_Click
        if (S.recId === 0) { HRM.box('Record not found for display'); return; }
        return printSlip(S.recId, btn || 'btnPrint');
    };
    P.defineCity = function () { HRM.open('/master-data/city'); };                                  // toolStripButton3_Click -> DefineCity
    P.defineReferenceParty = function () { HRM.open('/party-processing/reference-parties'); };     // btnFormReferenceParty_Click
    P.shortcuts = function () {
        HRM.box('Ctrl+E  For Close\nCtrl+N  For New\nCtrl+R  For Refresh\nCtrl+S  For Save\nCtrl+U  For Update\nAlt+P  For Print\n' +
            'Ctrl+F5  For Focus on gp Date\nCtrl+F10  For Open Attachments\nCtrl+Shift+Delete  For delete Record\nCtrl+T  For Tab Transfer\n' +
            'Ctrl+alt  To Show ShortCut Keys Form\nCtrl+ArrowDown  For Focus On Detail Grid\nCtrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link');
    };

    // ------------------------------------------------------------------ history tab
    function gridhistory(btn) {
        return HRM.busy(btn || 'btnShowHistory', function () {
            return post('/history', PPB.historyFilters({ stockPartyId: HRM.comboVal('CmbStockPartyHistory') })).then(function (rows) {
                grdhistory.set(rows || []);
            }).catch(HRM.fail);
        });
    }
    P.btnShowHistory = gridhistory;
    P.btnNewHistory = function () {                                                                // btnNewHistory_Click
        PPB.resetHistoryFilters(0, ['CmbStockPartyHistory']);
        grdhistory.clear();
        HRM.focus('FromDateHistory');
    };
    P.btnRefreshHistory = function (btn) {                                                         // HistoryCombosFill
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return get('/history-parties').then(function (rows) {
                HRM.fill('CmbStockPartyHistory', rows || [], 'Id', 'name', { zero: false, keep: true });
            }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { HRM.focus(name === 'history' ? 'FromDateHistory' : 'txtgpdate'); });
    HRM.footer(function () { PPB.showTab('history'); });
    PPB.leave('cmbgptype', function () { gpTypeLeave(false); });
    PPB.leave('CmbAdvanceDo', function () { adoLeave(); });
    ['txtSupplierWeight', 'txtFactoryWeight'].forEach(function (id) { HRM.$(id).addEventListener('input', total); });
    PPB.upper('txtvehicleno');
    PPB.guard('txtSupplierWeight', 'dec'); PPB.guard('txtqty', 'int'); PPB.guard('txtfreight', 'int');
    PPB.guard('txtFromDocNoHistory', 'int'); PPB.guard('txtToDocNoHistory', 'int');
    HRM.setVal('txtgpdate', HRM.today());
    PPB.resetHistoryFilters(0);
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+alt+alt': P.shortcuts, 'ctrl+alt+control': P.shortcuts,
        'ctrl+s': function () { if (PPB.currentTab() === 'history') gridhistory(); else if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
        'ctrl+u': function () { if (PPB.currentTab() === 'form' && HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); },
        'ctrl+p': function () { if (PPB.currentTab() === 'form' && !HRM.$('btnPrint').disabled) P.btnPrint(); },
        'ctrl+n': function () { if (PPB.currentTab() === 'form') P.btnnew(); else P.btnNewHistory(); },
        'ctrl+r': function () { if (PPB.currentTab() === 'form') P.btnRefresh(); },
        'alt+r': function () { if (PPB.currentTab() === 'history') P.btnRefreshHistory(); },
        'ctrl+f5': function () { HRM.focus(PPB.currentTab() === 'form' ? 'txtgpdate' : 'FromDateHistory'); },
        'ctrl+shift+delete': function () { if (PPB.currentTab() === 'form' && S.rights.delete !== false && HRM.visible('btnDelete')) P.btnDelete(); }
    });
    load();
})();
