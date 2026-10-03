/* ============================================================================================
 * countx_export_contract_register.js - ExImSaleContractRegister.cs, screen 251 "Sale Contract Register".
 * Data: /api/export/contract-register (Sp_ExImLcOrder_ExportRegister_Rpt, Sp_ExImLcOrder_ExportRegisterItemWise_Rpt,
 * Usp_ExportContractSummaryTotalForDashboardsandCardandReports, usp_ExportContractStatusUpdate).
 * Prints through the seeded CrystalPrint keys: 526-salecontractregister, 561-salecontractregisteritemwise,
 * 523-exprptsalescontractwiseinvoiceregister, 518-exprptsalescontractexportregister (hidden button),
 * 501-exportsalescontractexportnew (row Print / Ctrl+Space).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box, ask = H.ask;
    var API = '/api/export/contract-register';
    var S = { summary: [], product: [], card: [], cur: -1, yearStart: '', suspend: false, hasSummary: false, hasProduct: false };

    function btnCell(key, text) { return function () { return '<button type="button" class="win-btn-cell" data-btn="' + key + '">' + text + '</button>'; }; }
    function summaryCols() {
        var open = H.radio('regStatus') === 'Open', closed = H.radio('regStatus') === 'Complete' || H.radio('regStatus') === 'Cancel';
        return [
            { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'Doc No', fmt: 'i' }, { key: 'DocDate', caption: 'Doc Date', fmt: 'd' },
            { key: 'ContractNo', caption: 'Contract No', link: true }, { key: 'FarmingTrade', caption: 'Farming Trade' }, { key: 'CustomerName', caption: 'Customer Name' },
            { key: 'DeliveryTerms', caption: 'Delivery Terms' }, { key: 'PaymentTerms', caption: 'Payment Terms' }, { key: 'Fcy', caption: 'Fcy' },
            { key: 'FcyAmount', caption: 'Fcy Amount', num: true, sum: true, fmt: 'o3' }, { key: 'FcyShipped', caption: 'Fcy Shipped', num: true, sum: true, fmt: 'o3' },
            { key: 'FcyBalance', caption: 'Fcy Balance', num: true, sum: true, fmt: 'o3' }, { key: 'NetWeight', caption: 'Net Weight', num: true, sum: true, fmt: 'o3' },
            { key: 'ShippedWeight', caption: 'Shipped Weight', num: true, sum: true, fmt: 'o3' }, { key: 'BalanceWeight', caption: 'Balance Weight', num: true, sum: true, fmt: 'o3' },
            { key: 'Fcl', caption: 'Fcl', num: true, sum: true, fmt: 'i' }, { key: 'ShippedFcl', caption: 'Shipped Fcl', num: true, sum: true, fmt: 'i' },
            { key: 'BalanceFcl', caption: 'Balance Fcl', num: true, sum: true, fmt: 'i' }, { key: 'DestinationPort', caption: 'Destination Port' }, { key: 'Status', caption: 'Status' },
            { key: 'ActionRemarks', caption: 'Action Remarks', html: function (v, r, i) { return '<input type="text" class="win-cell-edit" data-remarks="' + i + '" value="' + H.esc(v) + '"/>'; } },
            { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true, link: true },
            { key: 'Print', caption: 'Print', html: btnCell('Print', 'Print') },
            { key: 'Open', caption: 'Open', hidden: !closed, html: btnCell('Open', 'Open') },
            { key: 'Cancel', caption: 'Cancel', hidden: !open, html: btnCell('Cancel', 'Cancel') },
            { key: 'Complete', caption: 'Complete', hidden: !open, html: btnCell('Complete', 'Complete') }
        ];
    }
    var PRODUCT_COLS = [
        { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'Doc No', fmt: 'i' }, { key: 'DocDate', caption: 'Doc Date', fmt: 'd' }, { key: 'ContractNo', caption: 'Contract No' },
        { key: 'CustomerName', caption: 'Customer Name' }, { key: 'DeliveryTerms', caption: 'Delivery Terms' }, { key: 'PaymentTerms', caption: 'Payment Terms' },
        { key: 'ItemName', caption: 'Item Name' }, { key: 'PackUom', caption: 'Pack Uom' }, { key: 'Fcy', caption: 'Fcy' }, { key: 'RatePrice', caption: 'Rate Price', num: true, fmt: 'o4' },
        { key: 'FcyAmount', caption: 'Fcy Amount', num: true, sum: true, fmt: 'o3' }, { key: 'FcyShipped', caption: 'Fcy Shipped', num: true, sum: true, fmt: 'o3' },
        { key: 'FcyBalance', caption: 'Fcy Balance', num: true, sum: true, fmt: 'o3' }, { key: 'NetWeight', caption: 'Net Weight', num: true, sum: true, fmt: 'o3' },
        { key: 'ShippedWeight', caption: 'Shipped Weight', num: true, sum: true, fmt: 'o3' }, { key: 'BalanceWeight', caption: 'Balance Weight', num: true, sum: true, fmt: 'o3' },
        { key: 'Fcl', hidden: true }, { key: 'ShippedFcl', hidden: true }, { key: 'BalanceFcl', hidden: true },
        { key: 'CommodityDetail', caption: 'Commodity Detail' }, { key: 'Status', caption: 'Status' }
    ];
    var CARD_COLS = [{ key: 'Descriptions', caption: 'Descriptions', width: 160 }, { key: 'Fcl', caption: 'Fcl', num: true, fmt: 'o3' }, { key: 'MTons', caption: 'M Tons', num: true, fmt: 'o3' },
        { key: 'FcyAmount', caption: 'Fcy Amount', num: true, fmt: 'o3' }, { key: 'PrctOfTot', caption: '% Of Total', num: true, fmt: 'o2' }];

    function bindCombos(d) {
        if (d.combosError) box(d.combosError);
        [['contracts', 'cmbcontractNo'], ['customers', 'cmbSupplierName'], ['items', 'cmbItemName'], ['tradeTypes', 'Cmbtradtype'], ['farmingTypes', 'CmbFarmingType'], ['salePersons', 'CmbSalePerson']]
            .forEach(function (p) { var keep = H.val(p[1]); H.bind(p[1], d[p[0]], 'Id', 'name', false); if (!keep || H.val(p[1]) !== keep) $id(p[1]).selectedIndex = -1; });
        H.refreshCombos();
        reportsShowHide();
    }
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.yearStart = d.yearStart || '';
            S.suspend = true;
            H.bind('cmbperemeter', d.dateTypes, 'Id', 'Parameters', false); $id('cmbperemeter').selectedIndex = -1;
            S.suspend = false;
            H.setText('datDateFrom', H.today()); H.setText('datDateTo', H.today());
            $id('datDateFromChk').checked = false; $id('datDateToChk').checked = false;
            bindCombos(d);
            if (d.scheduleTab) H.show('tabScheduleBtn', true);
            /* Load: GetDataForExportInfo, GridBind, GridBindProductWise, ReportsShowHide */
            return Promise.all([card(), summary(), productWise()]).then(reportsShowHide);
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() {
        return { contractId: H.netI(H.val('cmbcontractNo')), supplierCustomerId: H.netI(H.val('cmbSupplierName')), itemId: H.netI(H.val('cmbItemName')),
            tradeTypeId: H.netI(H.val('Cmbtradtype')), farmingTypeId: H.netI(H.val('CmbFarmingType')), fromChecked: H.checked('datDateFromChk'), fromDate: H.val('datDateFrom'),
            toChecked: H.checked('datDateToChk'), toDate: H.val('datDateTo'), skipZero: H.checked('chkSkipZero'), status: H.radio('regStatus'),
            salePersonId: H.netI(H.val('CmbSalePerson')), dateType: H.radio('regCard') };
    }
    function card() {
        return H.postJson(API + '/card', filters()).then(function (rows) { S.card = rows || []; H.drawGrid('grdCard', CARD_COLS, S.card, {}); }).catch(function (e) { box(e.message); });
    }
    function summary() {
        return H.postJson(API + '/summary', filters()).then(function (rows) {
            S.summary = rows || []; S.hasSummary = S.summary.length > 0; S.cur = -1;
            H.drawGrid('grdContractRegister', summaryCols(), S.summary, {});
            H.show('grdContractRegisterEmpty', !S.summary.length);
        }).catch(function (e) { box(e.message); });
    }
    function productWise() {
        return H.postJson(API + '/product-wise', filters()).then(function (rows) {
            S.product = rows || []; S.hasProduct = S.product.length > 0;
            H.drawGrid('grdProductWise', PRODUCT_COLS, S.product, {});
            H.show('grdProductWiseEmpty', !S.product.length);
        }).catch(function (e) { box(e.message); });
    }
    /** btnShow_Click: card, then Summary (product grid cleared) or ProductWise (summary grid cleared). */
    function refreshByMode() {
        return card().then(function () {
            if (H.radio('regMode') === 'summary') { tab2('tabPage2'); S.product = []; S.hasProduct = false; H.clearGrid('grdProductWise'); return summary(); }
            tab2('tabPage3'); S.summary = []; S.hasSummary = false; H.clearGrid('grdContractRegister'); return productWise();
        });
    }
    function show(btn) { return H.busy(btn, refreshByMode); }
    /** btnnew_Click -> Reset(): focus From, Contract / Customer / Sales Person blank. */
    function btnNew() { H.focus('datDateFrom'); ['cmbcontractNo', 'cmbSupplierName', 'CmbSalePerson'].forEach(function (id) { $id(id).selectedIndex = -1; }); H.refreshCombos(); reportsShowHide(); }
    function btnRefresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** ReportsShowHide + ChkSummary_CheckedChanged: 526 with Summary, 561 with ProductWise, 523 only with a contract, 518 never. */
    function reportsShowHide() {
        var sum = H.radio('regMode') === 'summary';
        H.show('btnregisterprint526', sum); H.show('btnregister561', !sum); H.show('toolStripButton2', false);
        H.show('btnregister523', H.netI(H.val('cmbcontractNo')) > 0);
    }
    function print518(btn) {
        var f = filters();
        return H.print('518-exprptsalescontractexportregister', { fromDate: f.fromDate, toDate: f.toDate, lcOrderId: f.contractId, supplierCustomerId: f.supplierCustomerId, itemId: f.itemId }, btn);
    }
    /** ShowRegister523: "Please Select Contract No or Customer To Proceed!" */
    function print523(btn) {
        var f = filters();
        if (f.contractId === 0 && f.supplierCustomerId === 0) { box('Please Select Contract No or Customer To Proceed!'); return; }
        return H.print('523-exprptsalescontractwiseinvoiceregister', { exImLcOrderId: f.contractId, supplierCustomerId: f.supplierCustomerId, fromDate: f.fromDate, toDate: f.toDate, actionId: 0 }, btn);
    }
    function regArgs() {
        var f = filters();
        return { contractNo: f.contractId, exImLcOrderId: f.contractId, datefrom: f.fromChecked ? f.fromDate : '', dateto: f.toChecked ? f.toDate : '', fromDate: f.fromChecked ? f.fromDate : '', toDate: f.toChecked ? f.toDate : '',
            status: f.status, supplierName: f.supplierCustomerId, supplierCustomerId: f.supplierCustomerId, itemId: f.itemId, tradeTypeId: f.tradeTypeId, farmingTypeId: f.farmingTypeId,
            skipZero: f.skipZero ? 1 : 0, refSaleManId: f.salePersonId };
    }
    /** ShowRegister526 / 561: "Not Record Found For Display" without rows. */
    function print526(btn) { if (!S.hasSummary) { box('Not Record Found For Display'); return; } return H.print('526-salecontractregister', regArgs(), btn); }
    function print561(btn) { if (!S.hasProduct) { box('Not Record Found For Display'); return; } return H.print('561-salecontractregisteritemwise', regArgs(), btn); }
    function print501(i, btn) { var r = S.summary[i]; if (!r) return; return H.print('501-exportsalescontractexportnew', { id: H.netI(r.Id), status: 'All' }, btn); }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+S', 'For Showing Data'],
            ['Alt+1', 'For Print 518'], ['Alt+2', 'For Print 523'], ['Alt+3', 'For Print 526'], ['Alt+4', 'For Print 561'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    /** grdfrm_ColumnButtonClick: Print 501; Complete / Open need Action Remarks; Cancel refuses shipped > 0; then ContractStatusChange. */
    function button(key, i, btn) {
        var r = S.summary[i]; if (!r) return;
        if (key === 'Print') { print501(i, btn); return; }
        var remarks = H.str(r.ActionRemarks).trim();
        if (key === 'Cancel' && H.netD(r.FcyShipped) > 0) { box('You cannot cancel this record because the shipped weight is greater than zero. You should mark it as complete.'); return; }
        if (!remarks) { box('Action Remarks Field is Compulsory.'); return; }
        if (!ask('Are you sure to ' + key + ' ?')) return;
        H.busy(btn, function () {
            return H.postJson(API + '/status', { id: H.netI(r.Id), reqType: key, actionRemarks: remarks, fcyShipped: H.netD(r.FcyShipped) }).then(function (d) {
                box((d && d.message) || (key + ' Successfully'));
                return refreshByMode();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** grdContractRegister_LinkClicked: ContractNo -> frmContractDetailByContractId, NoOfAttachments -> attachments (202). */
    function link(key, i) {
        var r = S.summary[i]; if (!r) return;
        if (key === 'ContractNo') H.contractDetail(H.netI(r.Id), 251);
        else if (key === 'NoOfAttachments') box('Attachments of contract ' + H.netI(r.Id) + ' (document type 202) - the attachment viewer has no web page yet.');
    }
    function tab1(id) {
        document.querySelectorAll('.win-tabs[data-tabs="reg1"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        ['tabPage1', 'tabSchedule'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
        if (id === 'tabSchedule') { var f = $id('scheduleFrame'); if (f && !f.getAttribute('src')) f.setAttribute('src', f.getAttribute('data-src')); }
    }
    function tab2(id) {
        document.querySelectorAll('.win-tabs[data-tabs="reg2"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        ['tabPage2', 'tabPage3'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        $id('cmbperemeter').addEventListener('change', function () { if (!S.suspend) H.dateTypeChanged('cmbperemeter', 'datDateFrom', 'datDateTo', S.yearStart); });
        $id('cmbcontractNo').addEventListener('change', reportsShowHide);
        document.querySelectorAll('input[name="regMode"]').forEach(function (r) { r.addEventListener('change', reportsShowHide); });
        /* ActionRemarks edits go straight into the row, as GridEX writes into dtHistory */
        $id('grdContractRegister').addEventListener('input', function (e) { var inp = e.target.closest('input[data-remarks]'); if (!inp) return; var r = S.summary[+inp.getAttribute('data-remarks')]; if (r) r.ActionRemarks = inp.value; });
        /* btnShow_Leave: focus returns to Date Type unless Shift+Tab */
        $id('btnShow').addEventListener('keydown', function (e) { S.shiftTab = e.shiftKey && e.key === 'Tab'; });
        $id('btnShow').addEventListener('blur', function () { if (!S.shiftTab) setTimeout(function () { if (document.activeElement === document.body) H.focus('cmbperemeter'); }, 0); });
        H.gridEvents('grdContractRegister', { select: function (i) { S.cur = i; }, link: link, button: button, ctrlSpace: function (i) { print501(i, null); } });
        H.gridEvents('grdProductWise', {});
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('cmbperemeter'); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnShow')); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdContractRegister'); }
            if (e.altKey && !e.ctrlKey && (e.key === '1' || e.code === 'Numpad1')) { e.preventDefault(); print518($id('toolStripButton2')); }
            if (e.altKey && !e.ctrlKey && (e.key === '2' || e.code === 'Numpad2')) { e.preventDefault(); print523($id('btnregister523')); }
            if (e.altKey && !e.ctrlKey && (e.key === '3' || e.code === 'Numpad3')) { e.preventDefault(); print526($id('btnregisterprint526')); }
            if (e.altKey && !e.ctrlKey && (e.key === '4' || e.code === 'Numpad4')) { e.preventDefault(); print561($id('btnregister561')); }
        });
        load();
    });
    global.ExportReg = { btnNew: btnNew, btnRefresh: btnRefresh, show: show, print518: print518, print523: print523, print526: print526, print561: print561, shortcuts: shortcuts, tab1: tab1, tab2: tab2 };
}(window));
