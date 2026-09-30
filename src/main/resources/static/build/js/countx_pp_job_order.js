/* ============================================================================================
 * countx_pp_job_order.js - 676 Production Job Order (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.ProductionJobOrderPartyProcessing.cs. Exports window.PpJo.
 * Load, Refresh, rdbtnItemName_CheckedChanged, Reset, Insert (Save / Update / Save As), BtnDelete_Click, ReadById,
 * history (BindHistoryGrid, Edit / Print / SaveAs buttons), keys. Field checks run on the server in FormValidation's order.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/production-job-order/';
    var P = {}; window.PpJo = P;
    var S = { rights: {}, recId: 0, items: [], defaultDays: 0 };
    var tabs = PPC.tabs(function (t) { if (t === 'history') HRM.focus('FromDateHistory'); else HRM.focus('txtPlanDate'); });

    var grid = new HRM.Grid('DataGridHistory', {
        filterRow: true, emptyText: 'No record found.',
        columns: [
            PPC.btnCol('Edit', 'Edit'), PPC.btnCol('Print', 'Print'), PPC.btnCol('SaveAs', 'SaveAs', 60),
            { key: 'Id', hidden: true }, { key: 'PlanDate', caption: 'PlanDate', type: 'date' }, { key: 'PlanCode', caption: 'PlanCode', type: 'int' },
            { key: 'DocumentTypeId', hidden: true }, { key: 'PlanType', caption: 'PlanType' }, { key: 'ProductionType', caption: 'ProductionType' },
            { key: 'JobOrderNo', caption: 'JobOrderNo' }, { key: 'LotReference', caption: 'LotReference' }, { key: 'OtherInstructions', caption: 'OtherInstructions' },
            { key: 'StartDate', caption: 'StartDate', type: 'date' }, { key: 'EndDate', caption: 'EndDate', type: 'date' }, { key: 'PlanStatus', caption: 'PlanStatus' },
            { key: 'StockPartyId', hidden: true }, { key: 'StockParty', caption: 'StockParty' }, { key: 'WipWareHouseId', hidden: true },
            { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'WipItemId', hidden: true }, { key: 'WipItem', caption: 'WipItem' },
            { key: 'ApprovalStatus', caption: 'ApprovalStatus' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'EntryUserName', caption: 'EntryUserName' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' },
            { key: 'ModifyUserName', caption: 'ModifyUserName' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' },
            { key: 'ApprovedUserName', caption: 'ApprovedUserName' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }
        ],
        onDouble: function (r) { readById(HRM.int(HRM.col(r, 'Id'))); }
    });
    PPC.onButton(grid, 'DataGridHistory', function (r, act, i, b) {
        var id = HRM.int(HRM.col(r, 'Id'));
        if (act === 'Edit') readById(id);
        else if (act === 'Print') report(id, b, true);
        else if (act === 'SaveAs') readById(id).then(function (ok) {
            if (!ok) return;
            HRM.show('btnUpdate', false); HRM.show('BtnDelete', false); HRM.show('btnsave', false); HRM.show('BtnSaveAs', true);
            HRM.enable('cmbStatus', false); statusFirst();
        });
    });

    function statusFirst() { var s = PPC.$('cmbStatus'); if (s) { s.selectedIndex = 0; HRM.refreshCombos(); } }
    function fillItems() {                                                  // rdbtnItemName_CheckedChanged
        var keep = HRM.comboVal('CmbWipItem');
        HRM.fill('CmbWipItem', S.items, 'Id', HRM.checked('rdbtnItemName') ? 'ItemName' : 'ItemCode', { zero: '' });
        HRM.setCombo('CmbWipItem', keep);
    }
    function fillLookups(o) {
        HRM.fill('cmbWareHouse', o.warehouses, 'Id', 'WareHouseName', { zero: '', keep: true });
        HRM.fill('CmbStockParty', o.stockParties, 'Id', 'CompanyName', { zero: '', keep: true });
        HRM.fill('cmbPlantFeader', o.plants, 'Id', 'Description', { zero: '', keep: true });
        S.items = o.items || [];
        fillItems();
    }
    function fillHistoryCombos(o) {
        HRM.fill('CmbStockPartyHistory', o.historyParties, 'Id', 'ReferenceName', { zero: '' });
        HRM.fill('CmbJobOrderNoHistory', o.historyJobOrders, 'Id', 'ReferenceName', { zero: '' });
    }
    function histDates() {
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(S.defaultDays > 0 ? S.defaultDays : 3)));
        HRM.setVal('ToDateHistory', HRM.today());
    }

    function load() {                                                       // frmProductionJobOrder_Load
        HRM.fillFixed('cmbStatus', [[1, 'In Process'], [2, 'Complete'], [3, 'Cancel']]);
        ['txtPlanDate', 'txtstartDate', 'txtEndDate'].forEach(function (id) { HRM.setVal(id, HRM.today()); });
        return HRM.loading(HRM.get(API + 'setup').then(function (o) {
            S.rights = o.rights || {};
            HRM.applyRights(S.rights, { save: ['btnsave', 'BtnSaveAs'], print: 'Print', update: 'btnUpdate' });
            fillLookups(o);
            HRM.setVal('txtPlanCode', o.planCode);
            HRM.fill('cmbProductionType', o.productionTypes, 'Id', 'ProductionTypeDescription', { zero: '' });
            S.defaultDays = HRM.int(o.defaultDays);
            fillHistoryCombos(o);
            histDates();
            HRM.enable('cmbStatus', false);
            HRM.focus('txtPlanDate');
        }).catch(HRM.fail));
    }

    function reset() {                                                      // Reset()
        S.recId = 0;
        HRM.setVal('txtJobOrderNo', '');
        HRM.setCombo('CmbStockParty', 0); HRM.setCombo('cmbWareHouse', 0); HRM.setCombo('CmbWipItem', 0);
        HRM.setVal('txtOtherReference', ''); HRM.setVal('txtOtherInstruction', '');
        statusFirst();
        HRM.show('btnsave', true); HRM.show('btnUpdate', false); HRM.show('BtnSaveAs', false); HRM.show('BtnDelete', false);
        HRM.enable('cmbStatus', false);
        HRM.focus('txtPlanDate');
        return HRM.get(API + 'code').then(function (c) { HRM.setVal('txtPlanCode', c.planCode); }).catch(HRM.fail);
    }
    P.btnnew = function () { reset(); };
    P.btnRefresh = function (b) {                                           // btnRefresh_Click
        HRM.busy(b, function () { return HRM.get(API + 'refresh').then(fillLookups).catch(HRM.fail); });
    };

    function insert(btn, recId, ask) {                                      // Insert()
        var body = {
            id: recId, planCode: HRM.val('txtPlanCode'), planDate: HRM.val('txtPlanDate'), jobOrderNo: HRM.val('txtJobOrderNo'),
            stockPartyId: HRM.comboVal('CmbStockParty'), wareHouseId: HRM.comboVal('cmbWareHouse'), wipItemId: HRM.comboVal('CmbWipItem'),
            lotReference: HRM.val('txtOtherReference'), plantId: HRM.comboVal('cmbPlantFeader'),
            productionTypeId: HRM.comboVal('cmbProductionType'), productionTypeText: HRM.comboText('cmbProductionType'),
            otherInstructions: HRM.val('txtOtherInstruction'), startDate: HRM.val('txtstartDate'), endDate: HRM.val('txtEndDate'),
            statusId: HRM.comboVal('cmbStatus'), statusText: HRM.comboText('cmbStatus')
        };
        if (String(body.planCode).trim() === '' || String(body.planCode).trim() === '0') { HRM.box('Plan Code Field Required'); return; }
        if (String(body.jobOrderNo) === '' || body.jobOrderNo === '0') { HRM.box('Mannual ReportNo Field Required'); HRM.focus('txtJobOrderNo'); return; }
        if (!body.stockPartyId) { HRM.box('StockParty Field Required'); HRM.focus('CmbStockParty'); return; }
        if (!body.wipItemId) { HRM.box('WIPItem Field Required'); HRM.focus('CmbWipItem'); return; }
        if (!body.plantId) { HRM.box('Plan/Feader Field Required'); HRM.focus('cmbPlantFeader'); return; }
        if (!body.productionTypeId) { HRM.box('Production Type Field Required'); HRM.focus('cmbProductionType'); return; }
        if (!body.statusId) { HRM.box('Status Field Required'); return; }
        if (!HRM.ask(ask)) return;
        HRM.busy(btn, function () {
            return HRM.post(API + 'save', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                return reset().then(function () { if (HRM.checked('chkPrint')) report(id, null, false); });
            }).catch(HRM.fail);
        });
    }
    P.btnsave = function (b) { insert(b, 0, 'Are you sure to Save?'); };
    P.btnSaveAs = function (b) { insert(b, 0, 'Are you sure to Save?'); };
    P.btnUpdate = function (b) {
        if (!S.recId) { HRM.box('Record Not Update Because Record Not Found'); return; }
        insert(b, S.recId, 'Are you sure to Update?');
    };
    P.btnDelete = function (b) {                                            // BtnDelete_Click
        if (!S.recId) { HRM.box('Record Id Not Found'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () {
            return HRM.post(API + 'delete?id=' + S.recId, {}).then(function (r) { HRM.box(r.message); return reset(); }).catch(HRM.fail);
        });
    };

    function readById(id) {                                                 // ReadById
        if (!id) { HRM.box('Id not Found'); return Promise.resolve(false); }
        return HRM.loading(HRM.get(API + 'read', { id: id }).then(function (h) {
            S.recId = HRM.int(h.Id);
            tabs.show('form');
            HRM.setVal('txtPlanDate', HRM.day(h.PlanDate));
            HRM.setVal('txtPlanCode', h.PlanCode);
            HRM.setVal('txtJobOrderNo', h.RefInvoiceNo);
            HRM.setCombo('CmbStockParty', h.StockPartyId);
            HRM.setCombo('cmbWareHouse', h.WipWareHouseId);
            HRM.setCombo('CmbWipItem', h.WipItemId);
            HRM.setVal('txtOtherReference', h.LotReference);
            HRM.setCombo('cmbPlantFeader', h.InvProductionPlantId);
            HRM.setComboText('cmbProductionType', h.ProductionType);
            HRM.setVal('txtOtherInstruction', h.OtherInstructions);
            HRM.setVal('txtstartDate', HRM.day(h.StartDate));
            HRM.setVal('txtEndDate', HRM.day(h.EndDate));
            HRM.setComboText('cmbStatus', h.PlanStatus);
            HRM.show('btnsave', false); HRM.show('btnUpdate', true); HRM.show('BtnDelete', true); HRM.show('BtnSaveAs', false);
            HRM.enable('cmbStatus', true);
            return true;
        }).catch(function (e) { HRM.fail(e); return false; }));
    }

    function report(id, btn, fromHistory) {                                 // GenerateReport -> ProductionJobOrderPartyProcessing_626
        return PPC.print(API + 'print', 'ppc-626', { id: id, history: !!fromHistory }, btn);
    }
    P.print = function (b) { report(S.recId, b, false); };

    // ------------------------------------------------------------------ history
    P.btnNewHistory = function () {                                        // btnNewHistory_Click
        HRM.setVal('FromDateHistory', HRM.today()); HRM.setVal('ToDateHistory', HRM.today());
        HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', '');
        HRM.setCombo('CmbStockPartyHistory', 0); HRM.setCombo('CmbJobOrderNoHistory', 0);
        grid.clear();
        var r = document.querySelector('input[name="histDateMode"][value="doc"]'); if (r) r.checked = true;
    };
    P.btnRefreshHistory = function (b) {
        HRM.busy(b, function () { return HRM.get(API + 'history-combos').then(fillHistoryCombos).catch(HRM.fail); });
    };
    P.btnshowHistory = function (b) {                                       // BindHistoryGrid
        HRM.busy(b, function () {
            var body = PPC.hist(null, { stockPartyId: HRM.comboVal('CmbStockPartyHistory'), jobOrderId: HRM.comboVal('CmbJobOrderNoHistory') });
            return HRM.post(API + 'history', body).then(function (rows) { grid.set(rows); }).catch(HRM.fail);
        });
    };

    P.shortcuts = function () {
        PPC.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
            ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print Slip'], ['Alt+1', 'For Print Slip'],
            ['Ctrl+F5', 'For Focus on Plan Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    };

    function onForm() { return tabs.current() === 'form'; }
    function vis(id) { return HRM.visible(id) && !PPC.$(id).disabled; }
    HRM.keys({
        'ctrl+t': function () { tabs.show(onForm() ? 'history' : 'form'); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+s': function () { if (onForm() && vis('btnsave')) P.btnsave(PPC.$('btnsave')); },
        'ctrl+u': function () { if (onForm() && vis('btnUpdate')) P.btnUpdate(PPC.$('btnUpdate')); },
        'ctrl+n': function () { if (onForm()) P.btnnew(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(PPC.$('btnRefresh')); },
        'ctrl+f5': function () { HRM.focus(onForm() ? 'txtPlanDate' : 'FromDateHistory'); },
        'ctrl+p': function () { if (onForm() && !PPC.$('Print').disabled) P.print(PPC.$('Print')); },
        'alt+1': function () { if (onForm() && !PPC.$('Print').disabled) P.print(PPC.$('Print')); }
    });
    document.querySelectorAll('input[name="rdItem"]').forEach(function (r) { r.addEventListener('change', fillItems); });
    PPC.guard('txtFromDocNoHistory', 'int'); PPC.guard('txtToDocNoHistory', 'int');
    HRM.footer(function () { tabs.show('history'); });
    load();
})();
