/* ============================================================================================
 * countx_export_packing_detail_by_contract.js - frmExportSalesContractPmDetail.cs (Architecture.WinApp.Export),
 * screen 240 "Packing Detail By Export Contract". The Packing Material grid of an existing Export Contract
 * (DocumentTypeId 202): Sales Contract No -> ReadById (contract detail grouped into brand items, PM rows,
 * AutoFillGridFromMapping), the PM entry panel, Update (ExImLcOrder.SaveForPmDetail), 501-Print, History.
 * Data from /api/export/packing-detail-by-contract (ExportContractPmDetailController -> ExportContractPmDetailService).
 * Prints: exp-501new (501-Print / Preview / history Print), exp-523 (history Print523).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExportG;
    var API = '/api/export/packing-detail-by-contract';
    var $id = X.$id, box = X.box, ask = X.ask, str = X.str, netI = X.netI, netD = X.netD;
    var val = X.val, valI = X.valI, setVal = X.setVal, setText = X.setText, bind = X.bind, focus = X.focus, checked = X.checked;

    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { defaultDaysToLessFromHistoryFromDate: 0, itemSearchByCode: false, fcyDecimals: 2 };
    var L = { contracts: [], pmItems: [], itemPmMap: [], historyCustomers: [] };
    var S = { recId: 0, details: [], header: null };
    var HIST = [];
    var tabsMain, tabs2;
    function fcyDec() { return CFG.fcyDecimals > 0 ? CFG.fcyDecimals : 2; }

    var pm = X.pmTab({
        ids: { brand: 'cmbBrandForPmEntry', brandUom: 'CmbBrandUomPm', brandPackType: 'CmbBrandPackingTypePm', brandOuter: 'txtBrandOuterQtyPm', brandInner: 'txtBrandInnerQtyPm',
            pmItem: 'CmbPMItem', pmUom: 'CmbUomPm', pmOuter: 'txtOuterQtyPm', pmInner: 'txtInnerQtyPm', rate: 'txtPackingItemRate', amount: 'txtPackingAmount', remarks: 'txtPackingRemarks',
            add: 'btnPackingAdd', update: 'btnPackingUpdate', cancel: 'btnPackingCancel', grid: 'grdPackingMaterialDetail',
            brandByCode: 'radBrandCode', brandByName: 'radBrandName', pmByCode: 'rdPackingItemCode', pmByName: 'rdPackingItemName' },
        api: { lastRate: null, pmUoms: API + '/pm-uoms?itemId=' },
        fcyDecimals: fcyDec
    });
    var detailView = X.grid('grdContractDetailView', [
        { key: 'ItemCode' }, { key: 'ItemName' }, { key: 'PackType' }, { key: 'CropYear' }, { key: 'QtyMTon', num: true, dec: 6 }, { key: 'PackSize' }, { key: 'NoOfBags', num: true },
        { key: 'Rate', num: true, dec: 4 }, { key: 'RateUOM' }, { key: 'Amount', num: true, dec: 4 }], { totals: ['QtyMTon', 'NoOfBags', 'Amount'] });
    var histGrid = X.grid('DataGridHistory', [
        { key: 'DocNo', num: true, dec: 0 }, { key: 'DocDate', fmt: X.ddMMMyyyy }, { key: 'OrderNo' }, { key: 'OrderDate', date: true }, { key: 'CustomerName' }, { key: 'LotReference' },
        { key: 'MTons', num: true }, { key: 'NoOfContainers', num: true, dec: 0 }, { key: 'FcyCode' }, { key: 'FcyAmount', num: true, dec: 2 }, { key: 'LastShipmentDate', date: true },
        { key: 'NotifyParty' }, { key: 'DeliveryTerm' }, { key: 'PaymentTerm' }, { key: 'LoadingPort' }, { key: 'DestinationPort' }, { key: 'Commodity_Description' },
        { key: 'EntryUser' }, { key: 'EntryDate', datetime: true }, { key: 'ModifyUser' }, { key: 'ModifyDate', datetime: true }, { key: 'NoOfAttachments', num: true, dec: 0, link: true }
    ], {
        buttons: [{ key: 'Print', text: 'Print' }, { key: 'Edit', text: 'Edit' }, { key: 'Print523', text: 'Print523' }],
        totals: ['MTons', 'NoOfContainers', 'FcyAmount'], emptyId: 'pdHistEmpty',
        onButton: historyButton, onDblClick: function (i) { historyButton(i, 'Edit'); }, onSelect: historySelected,
        onLink: function () { box('Attachments (DMS) are not available in the web port.'); }
    });
    var hPm = X.grid('grdPMHistory', X.HIST_COLS.pm, { totals: ['PmItemOuterQty', 'PmItemInnerQty', 'Amount'] });

    // ------------------------------------------------------------------------------ load

    function applyLists(d) {
        ['contracts', 'pmItems', 'itemPmMap', 'historyCustomers'].forEach(function (k) { if (d[k] !== undefined) L[k] = d[k] || []; });
        if (d.contractsError) box(d.contractsError);
        if (d.pmItemsError) box(d.pmItemsError);
        bind('CmbSalesContractNo', L.contracts, 'Id', 'name');
        pm.bindPmItems(L.pmItems);
        if (d.historyCustomers) bind('CmbCustomerHistory', L.historyCustomers, 'Id', 'name');
    }
    function setup() {
        return X.getJson(API + '/setup').then(function (d) {
            PERM = d.permissions || PERM;
            X.setEnabled('btnUpdate', !!(PERM.Update || PERM.Save)); X.setEnabled('Print', !!PERM.Print);
            if (d.config) CFG = d.config;
            /* ItemSearchByCode: radBrandName (the "Code" radio of the desktop) + rdPackingItemCode, else radBrandCode ("Name") + rdPackingItemName. */
            X.setChecked(CFG.itemSearchByCode ? 'radBrandCode' : 'radBrandName', true);
            X.setChecked(CFG.itemSearchByCode ? 'rdPackingItemCode' : 'rdPackingItemName', true);
            applyLists(d);
            var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
            setText('FromDateHistory', X.daysAgo(days > 0 ? days : 3)); setText('ToDateHistory', X.today());
            focus('CmbSalesContractNo');
        });
    }

    // ------------------------------------------------------------------------------ contract

    /* CmbSalesContractNo_Leave */
    function contractLeave() {
        var id = valI('CmbSalesContractNo');
        if (id === 0) return;
        if (pm.rows.length > 0 && !ask('Grid already have Records.Do you want To Refresh Data?')) return;
        readById(id).catch(function (e) { box(e.message); });
    }
    function readById(id) {
        return X.getJson(API + '/read?id=' + id).then(function (d) {
            S.recId = id; S.header = d.header || {}; S.details = d.details || [];
            tabsMain.select('pdForm'); tabs2.select('tabPackingMaterialDetail');
            setVal('CmbSalesContractNo', id);
            var c = X.rowOf(L.contracts, 'Id', id);
            setText('pdPartyName', c ? str(c.PartyName) : '');
            setText('pdContractInfo', X.fmt(S.header.NetWeightKgs) + ' / ' + X.fmt(S.header.NoOfContainers, 2));
            pm.bindBrands(d.brandItems || []);
            pm.rows = d.pmRows || [];
            pm.reset(); pm.draw();
            detailView.draw(S.details);
            pm.autoMap(L.itemPmMap);
            $id('pdStatusInfo').textContent = 'Contract ' + str(S.header.LcOrderNo) + ' (' + str(S.header.Status) + ')';
            $id('pdFooterInfo').textContent = 'Record ' + id;
        });
    }
    function reset() {
        S.recId = 0; S.details = []; S.header = null;
        tabs2.select('tabPackingMaterialDetail');
        setVal('CmbSalesContractNo', 0); setText('pdPartyName', ''); setText('pdContractInfo', '');
        pm.rows = []; pm.bindBrands([]); pm.reset(); pm.draw(); detailView.draw([]);
        $id('pdStatusInfo').textContent = ''; $id('pdFooterInfo').textContent = '';
        focus('CmbSalesContractNo');
    }
    function btnNew() { reset(); }
    function btnRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/refresh').then(applyLists); }); }
    /* btnUpdate_Click -> Insert(): "RecId not Found", "Sale ContractNo Is required", the confirm, the service's checks, SaveForPmDetail, Reset, Preview. */
    function btnUpdate(btn) {
        return X.busy(btn, function () {
            if (S.recId === 0) throw new Error('RecId not Found');
            if (valI('CmbSalesContractNo') === 0) { box('Sale ContractNo Is required'); focus('CmbSalesContractNo'); return; }
            if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var preview = checked('ChkPreview'), win = preview && global.CrystalPrint ? global.CrystalPrint.reserve() : null;
            return X.postJson(API + '/save', { recId: S.recId, contractId: valI('CmbSalesContractNo'), pmRows: pm.rows }).then(function (r) {
                box(r.message || 'Record Update Successfully');
                reset();
                if (preview && global.CrystalPrint) global.CrystalPrint.open('exp-501new', { id: netI(r.id) }, null, win);
                else if (win) global.CrystalPrint.release(win);
            }, function (e) { if (win) global.CrystalPrint.release(win); throw e; });
        });
    }
    function print501(btn) {
        if (S.recId <= 0) { box('No Record found'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-501new', { id: S.recId }, btn);
    }
    /* BtnLoadContract_Click: the LoadSalesContractForPmDetail picker is not ported - the Sales Contract No combo (searchable) stands in. */
    function loadContract() { box('The Load Contract picker (LoadSalesContractForPmDetail) is not ported; pick the contract in the searchable Sales Contract No box.'); focus('CmbSalesContractNo'); }
    function shortcuts() {
        X.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F1', 'For Open Delivery Term Popup'], ['Ctrl+F2', 'For Open Payterm Popup'], ['Ctrl+F3', 'For Open Sea Ports Popup'], ['Ctrl+F5', 'For Focus on Doc Date'],
            ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowRight', 'For Moving In Detail Grids'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Column in Detail for Entry'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', 'When Focus On Any Grid To Call Function\'s On Button Or Link']]);
    }

    // ------------------------------------------------------------------------------ history

    function historyShow(btn) {
        return X.busy(btn, function () {
            var body = X.contractHistoryBody({ fromChecked: 'chkFromDateHistory', fromDate: 'FromDateHistory', toChecked: 'chkToDateHistory', toDate: 'ToDateHistory', dateBy: 'pdHistDateBy', fromDocNo: 'FromDocNoHistory', toDocNo: 'ToDocNoHistory', customer: 'CmbCustomerHistory' });
            return X.postJson(API + '/history', body).then(function (rows) {
                HIST = rows || []; histGrid.current = -1; histGrid.draw(HIST); hPm.draw([]);
                if (HIST.length === 0) box('No record found');
            });
        });
    }
    function historyNew() {
        var days = netI(CFG.defaultDaysToLessFromHistoryFromDate);
        setText('FromDateHistory', X.daysAgo(days > 0 ? days : 3)); setText('ToDateHistory', X.today());
        setText('FromDocNoHistory', ''); setText('ToDocNoHistory', ''); setVal('CmbCustomerHistory', 0); X.setChecked('drdocdate', true);
        HIST = []; histGrid.draw([]); hPm.draw([]);
    }
    function historyRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-combos').then(function (d) { L.historyCustomers = d.historyCustomers || []; bind('CmbCustomerHistory', L.historyCustomers, 'Id', 'name'); }); }); }
    function historySelected(i) {
        var r = HIST[i]; if (!r) return;
        X.getJson(API + '/history-detail?id=' + netI(r.Id)).then(function (rows) { hPm.draw(rows || []); }).catch(function (e) { box(e.message); });
    }
    function historyButton(i, key, btn) {
        var r = HIST[i]; if (!r) return;
        var id = netI(r.Id);
        if (key === 'Edit') return readById(id).catch(function (e) { box(e.message); });
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (key === 'Print') global.CrystalPrint.open('exp-501new', { id: id }, btn);
        else if (key === 'Print523') global.CrystalPrint.open('exp-523', { id: id }, btn);
    }
    function toggleHistory() { if (tabsMain.current() === 'pdHistory') tabsMain.select('pdForm'); else tabsMain.select('pdHistory'); }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function wire() {
        tabsMain = X.tabs('pd', ['pdForm', 'pdHistory'], function (p) { if (p === 'pdHistory') focus('FromDateHistory'); else focus('CmbSalesContractNo'); });
        tabs2 = X.tabs('pd2', ['tabPackingMaterialDetail', 'tabContractDetailView']);
        X.initFullscreen();
        pm.wire();
        on('CmbSalesContractNo', 'change', contractLeave);
        ['FromDocNoHistory', 'ToDocNoHistory'].forEach(function (id) { on(id, 'keypress', X.integerOnly); });
        document.addEventListener('keydown', function (e) {
            var inForm = tabsMain.current() === 'pdForm';
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'button') {
                var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) { return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (!e.ctrlKey) { if (e.key === 'Escape' && !document.querySelector('.ex-fullscreen')) X.cancelWindow(); return; }
            var k = e.key.toLowerCase();
            if (k === 'e') { e.preventDefault(); X.cancelWindow(); }
            else if (k === 't') { e.preventDefault(); tabsMain.select(inForm ? 'pdHistory' : 'pdForm'); }
            else if (k === 'n') { e.preventDefault(); if (inForm) btnNew(); else historyNew(); }
            else if (k === 'r') { e.preventDefault(); if (inForm) btnRefresh($id('BtnRefresh')); else historyRefresh($id('btnRefreshHistory')); }
            else if (inForm && (k === 's' || k === 'u') && S.recId > 0) { e.preventDefault(); btnUpdate($id('btnUpdate')); }
            else if (inForm && (k === 'p' || k === '1')) { e.preventDefault(); print501($id('Print')); }
            else if (inForm && e.key === 'F5') { e.preventDefault(); focus('CmbSalesContractNo'); }
            else if (!inForm && k === 'l') { e.preventDefault(); historyShow($id('btnShowHistory')); }
            else if (inForm && e.key === 'ArrowDown') { e.preventDefault(); var g = $id('grdPackingMaterialDetail'); if (g) g.focus(); }
            else if (inForm && e.key === 'ArrowUp') { e.preventDefault(); focus('cmbBrandForPmEntry'); }
            else if (e.altKey) { e.preventDefault(); shortcuts(); }
        });
        setup().then(function () {
            var q = /[?&]id=(\d+)/.exec(location.search);
            if (q) readById(netI(q[1])).catch(function (e) { box(e.message); });
        }).catch(function (e) { box(e.message); });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', wire); else wire();

    global.ExportContractPmDetail = { btnNew: btnNew, btnRefresh: btnRefresh, btnUpdate: btnUpdate, print501: print501, loadContract: loadContract, shortcuts: shortcuts,
        historyShow: historyShow, historyNew: historyNew, historyRefresh: historyRefresh, toggleHistory: toggleHistory, readById: readById };
})(window);
