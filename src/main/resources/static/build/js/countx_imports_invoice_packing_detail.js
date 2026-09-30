/* ============================================================================================
 * countx_imports_invoice_packing_detail.js - 784 Invoice Packing Detail, ported from
 * Architecture.WinApp.Import.Transactions.frmInvoicePackingDetail. Built on countx_hrm.js (HRM) and
 * countx_imports_impb_core.js (ImpB); exports window.ImpBPacking. Each handler names the form method it follows.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/import/invoice-packing-detail';
    var P = {}; window.ImpBPacking = P;
    var RecId = 0, rights = {}, historyDays = 0, dtPMItem = [], dtUom = [], updateDetailIndexPmDetail = -1;

    function btnCell(act, text) { return '<button type="button" class="win-btn-action impb-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function num3(key, caption) { return { key: key, caption: caption, type: 'num', sum: true, decimals: 3, render: function (v) { return HRM.esc(ImpB.fmt(v, 3, 0)); } }; }

    // ------------------------------------------------------------------ detail grid (grdSettingsPM)
    var grd = new HRM.Grid('grdPackingMaterialDetail', {
        columns: [
            { key: 'Id', hidden: true }, { key: 'ContainerNo', caption: 'ContainerNo', width: 80 }, { key: 'ItemId', hidden: true },
            { key: 'ItemCode', caption: 'ItemCode', width: 45 }, { key: 'ItemName', caption: 'ItemName', width: 200 },
            { key: 'ItemDescription', caption: 'ItemDescription', width: 200 }, { key: 'JobLotId', hidden: true },
            { key: 'JobLot', caption: 'JobLot', width: 120 }, { key: 'PackUomId', hidden: true }, { key: 'PackUom', caption: 'PackUom', width: 60 },
            num3('Qty', 'Qty'), num3('Weight', 'Weight'), { key: 'Remarks', caption: 'Remarks', width: 200 },
            { key: 'Delete', caption: 'X', width: 20, render: function () { return btnCell('delete', 'X'); } }
        ],
        totals: true,
        onDouble: function (r, i) { grdPackingMaterialDetail_DoubleClick(r, i); }
    });
    HRM.$('grdPackingMaterialDetail').addEventListener('click', function (e) {          // grdPackingMaterialDetail_ColumnButtonClick
        var b = e.target.closest('button[data-act="delete"]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        DeleteDetailRecord(+tr.getAttribute('data-i'));
    });
    HRM.$('grdPackingMaterialDetail').addEventListener('keydown', function (e) {       // grdPackingMaterialDetail_KeyDown
        var i = grd.currentIndex(); if (i < 0) return;
        if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); grdPackingMaterialDetail_DoubleClick(grd.rows()[i], i); }
    });

    function DeleteDetailRecord(i) {
        if (updateDetailIndexPmDetail !== -1) { HRM.box('Reset the Detail first...'); return; }
        var r = grd.rows()[i];
        if (HRM.int(HRM.col(r, 'Id')) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            // LstRemoveRecordDetail.Add(vd): the desktop keeps the row but Insert() never sends it (see the report).
        }
        grd.remove(i);
    }

    // ------------------------------------------------------------------ history grids
    var hist = new HRM.Grid('DataGridHistory', {
        columns: [
            { key: 'Edit', caption: 'Edit', width: 40, render: function () { return btnCell('edit', 'Edit'); } },
            { key: 'Id', hidden: true }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 130 }, { key: 'InvoiceDate', caption: 'InvoiceDate', type: 'date', width: 80 },
            { key: 'EntryUser', caption: 'EntryUser', width: 135 }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime', width: 145 },
            { key: 'ModifyUser', caption: 'ModifyUser', width: 135 }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime', width: 145 }
        ],
        filterRow: true,
        onDouble: function (r) { RecId = HRM.int(HRM.col(r, 'Id')); ReadById(RecId); },                    // DataGridHistory_DoubleClick
        onSelect: function (r) { GetDetailGrdByHeadId(HRM.int(HRM.col(r, 'Id'))); }                       // DataGridHistory_SelectionChanged
    });
    HRM.$('DataGridHistory').addEventListener('click', function (e) {                                       // DataGridHistory_ColumnButtonClick
        var b = e.target.closest('button[data-act="edit"]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        RecId = HRM.int(HRM.col(hist.rows()[+tr.getAttribute('data-i')], 'Id'));
        ReadById(RecId);
    });
    HRM.$('DataGridHistory').addEventListener('keydown', function (e) {                                     // DataGridHistory_KeyDown
        var r = hist.current(); if (!r) return;
        if (e.ctrlKey && e.key === 'Enter' && rights.update) { e.preventDefault(); Reset(); RecId = HRM.int(HRM.col(r, 'Id')); ReadById(RecId); }
    });
    var pmHist = new HRM.Grid('grdPMHistory', {
        columns: [
            { key: 'ContainerNo', caption: 'ContainerNo', width: 75 }, { key: 'ItemCode', caption: 'ItemCode', width: 65 },
            { key: 'ItemName', caption: 'ItemName', width: 130 }, { key: 'ItemDescription', caption: 'ItemDescription', width: 130 },
            { key: 'JobLot', caption: 'JobLot', width: 130 }, { key: 'PackUom', caption: 'PackUom', width: 70 },
            num3('Qty', 'Qty'), num3('Weight', 'Weight'), { key: 'Remarks', caption: 'Remarks', width: 200 }
        ],
        totals: true
    });
    var detailSeq = 0;
    function GetDetailGrdByHeadId(id) {
        var seq = ++detailSeq;
        return HRM.get(API + '/by-invoice', { id: id }).then(function (d) { if (seq === detailSeq) pmHist.set(d.rows || []); }).catch(HRM.fail);
    }
    function HistoryFill() {
        var f = ImpB.historyFilter('h');
        f.fromDocNo = HRM.val('FromDocNoHistory'); f.toDocNo = HRM.val('ToDocNoHistory'); f.customerId = HRM.comboVal('CmbCustomerHistory');
        return HRM.post(API + '/history', f).then(function (rows) { hist.set(rows || []); if (!(rows || []).length) pmHist.clear(); }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ combos
    function itemText(r) { return HRM.checked('rdPackingItemName') ? HRM.col(r, 'ItemName') : HRM.col(r, 'ItemCode'); }
    function PackingItemFill(rows, zero) {
        dtPMItem = rows || [];
        HRM.fill('CmbItem', dtPMItem, 'Id', itemText, { zero: zero ? '' : '', keep: true });
    }
    function fillCombos(d) {
        if (d.invoices) HRM.fill('CmbInvoiceNo', d.invoices, 'Id', 'InvoiceNo', { zero: '', keep: true });
        if (d.items) PackingItemFill(d.items);
        if (d.jobLots) HRM.fill('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true });
    }

    /** CmbPMItem_Leave -> BindPmItemPackUom(): the item's UOM schedule, keeping the same Pack Uom text when present. */
    function BindPmItemPackUom() {
        var text = HRM.comboText('CmbPackUom'), item = HRM.comboVal('CmbItem');
        return HRM.get(API + '/uoms', { itemId: item }).then(function (rows) {
            dtUom = rows || [];
            HRM.fill('CmbPackUom', dtUom, 'Id', 'UOMCode', { zero: '' });
            if (text) HRM.setComboText('CmbPackUom', text);
            CalculateWeight();
        }).catch(HRM.fail);
    }
    /** CalculateWeight(): Weight = Qty × Pack Uom Equivalent ("#,##0.###"), "0" otherwise. */
    function CalculateWeight() {
        var qty = HRM.num(HRM.val('txtQty')), eq = 0, id = HRM.comboVal('CmbPackUom');
        dtUom.forEach(function (u) { if (HRM.int(HRM.col(u, 'Id')) === id) eq = HRM.num(HRM.col(u, 'Equivalent')); });
        HRM.setVal('txtWeight', qty > 0 && eq > 0 ? ImpB.fmt(qty * eq, 3, 0) : '0');
    }

    // ------------------------------------------------------------------ detail entry
    function PackingMaterialFormValidation() {
        if (!HRM.comboVal('CmbItem')) { HRM.box('Item Is required'); HRM.focus('CmbItem'); return false; }
        if (!HRM.comboVal('CmbPackUom')) { HRM.box('Pack Uom Is required'); HRM.focus('CmbPackUom'); return false; }
        if (HRM.num(HRM.val('txtQty').trim()) === 0) { HRM.box('Qty is required'); HRM.focus('txtQty'); return false; }
        if (HRM.num(HRM.val('txtWeight').trim()) === 0) { HRM.box('Weight is required'); HRM.focus('txtWeight'); return false; }
        return true;
    }
    function entryRow(base) {
        var item = HRM.comboRow('CmbItem', 'Id') || {};
        var r = base || { Id: 0 };
        r.ContainerNo = HRM.val('txtContainerNo').trim();
        r.ItemId = HRM.comboVal('CmbItem'); r.ItemCode = HRM.str(HRM.col(item, 'ItemCode')); r.ItemName = HRM.str(HRM.col(item, 'ItemName'));
        r.ItemDescription = HRM.val('txtItemDescription').trim();
        r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = HRM.comboText('CmbJobLot');
        r.PackUomId = HRM.comboVal('CmbPackUom'); r.PackUom = HRM.comboText('CmbPackUom');
        r.Qty = HRM.num(HRM.val('txtQty')); r.Weight = HRM.num(HRM.val('txtWeight')); r.Remarks = HRM.val('txtPackingRemarks');
        return r;
    }
    /** ResetPmDetail(): Container No, Description and Job Lot are NOT cleared by the form. */
    function ResetPmDetail() {
        updateDetailIndexPmDetail = -1;
        HRM.setCombo('CmbItem', 0); HRM.setCombo('CmbPackUom', 0);
        HRM.setVal('txtQty', ''); HRM.setVal('txtWeight', ''); HRM.setVal('txtPackingRemarks', '');
        HRM.show('btnPackingAdd', true); HRM.show('btnPackingUpdate', false); HRM.show('btnPackingCancel', false);
    }
    function grdPackingMaterialDetail_DoubleClick(r, i) {
        updateDetailIndexPmDetail = i;
        HRM.setVal('txtContainerNo', HRM.str(HRM.col(r, 'ContainerNo')));
        HRM.setCombo('CmbItem', HRM.int(HRM.col(r, 'ItemId')));
        var packId = HRM.int(HRM.col(r, 'PackUomId'));
        BindPmItemPackUom().then(function () {
            HRM.setCombo('CmbPackUom', packId);
            HRM.setVal('txtQty', ImpB.fmt(HRM.col(r, 'Qty'), 3, 0));
            HRM.setVal('txtWeight', ImpB.fmt(HRM.col(r, 'Weight'), 3, 0));
        });
        HRM.setVal('txtItemDescription', HRM.str(HRM.col(r, 'ItemDescription')));
        HRM.setCombo('CmbJobLot', HRM.int(HRM.col(r, 'JobLotId')));
        HRM.setVal('txtPackingRemarks', HRM.str(HRM.col(r, 'Remarks')));
        HRM.show('btnPackingAdd', false); HRM.show('btnPackingUpdate', true); HRM.show('btnPackingCancel', true);
        HRM.focus('CmbItem');
    }
    P.btnPackingAdd = function () {                                        // btnPackingAdd_Click
        if (!PackingMaterialFormValidation()) return;
        grd.add(entryRow());
        ResetPmDetail();
        HRM.focus('CmbItem');
    };
    P.btnPackingUpdate = function () {                                     // btnPackingUpdate_Click
        if (!PackingMaterialFormValidation()) return;
        var i = updateDetailIndexPmDetail; if (i < 0) return;
        grd.update(i, entryRow(grd.rows()[i]));
        ResetPmDetail();
        HRM.focus('CmbItem');
    };
    P.btnPackingCancel = function () { ResetPmDetail(); };

    // ------------------------------------------------------------------ header / ReadById
    function buttons(update) { HRM.show('btnSave', !update); HRM.show('btnUpdate', update); }
    function ReadById(id) {                                                // ReadById(ID): only a loaded list changes the page
        RecId = id;
        return HRM.loading(HRM.get(API + '/by-invoice', { id: id })).then(function (d) {
            if (!d || !d.found) return;
            HRM.show('btnSave', false); HRM.show('btnUpdate', true);
            ImpB.showTab('tc1', 'tabPage1');
            HRM.setCombo('CmbInvoiceNo', id);
            grd.set(d.rows || []);
        }).catch(HRM.fail);
    }
    /** CmbSalesContractNo_Leave: an invoice loads its packing rows (asks first when the grid already has rows). */
    function CmbInvoiceNo_Leave() {
        var id = HRM.comboVal('CmbInvoiceNo');
        if (id === 0) return;
        if (grd.rows().length > 0 && !HRM.ask('Grid already have Records.Do you want To Refresh Data?')) return;
        RecId = id;
        ReadById(RecId);
    }
    function Reset() {                                                     // Reset() - the Save / Update buttons are left as they are
        RecId = 0;
        ImpB.showTab('tc2', 'tabPackingMaterialDetail');
        HRM.setCombo('CmbInvoiceNo', 0);
        grd.clear();
        ResetPmDetail();
        HRM.focus('CmbInvoiceNo');
    }

    // ------------------------------------------------------------------ Insert
    function Insert(btn, update) {
        if (!HRM.comboVal('CmbInvoiceNo')) { HRM.box('Sale Invoice Is required'); HRM.focus('CmbInvoiceNo'); return; }
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var rows = grd.rows();
        for (var i = 0; i < rows.length; i++) {
            if (!HRM.int(HRM.col(rows[i], 'ItemId'))) { HRM.box('Item Is Required In Packing Material Grid in Row No ' + (i + 1)); return; }
            if (!HRM.int(HRM.col(rows[i], 'PackUomId'))) { HRM.box('PackUomId Is Required In Packing Material Grid in Row No ' + (i + 1)); return; }
            if (!HRM.int(HRM.col(rows[i], 'Qty'))) { HRM.box('Qty Is Required In Packing Material Grid in Row No ' + (i + 1)); return; }
        }
        if (!rows.length) { HRM.box('Packing Material Grid record not found'); return; }
        var preview = HRM.checked('ChkPreview');
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', { update: update, recId: RecId, invoiceMasterId: HRM.comboVal('CmbInvoiceNo'), rows: rows }).then(function (d) {
                HRM.box(d && d.message);
                Reset();
                if (preview) HRM.box('501 print (CommonServices.SaleContractReportsSlipNew501) is not available on the web: the desktop\'s CommonServices source is not in the workspace.');
            }).catch(HRM.fail);
        }, 'packing-save');
    }
    P.btnSave = function (btn) { RecId = 0; return Insert(btn || 'btnSave', false); };                 // btnSave_Click
    P.btnUpdate = function (btn) {                                                                     // btnUpdate_Click
        if (RecId === 0) { HRM.box('RecId not Found'); return; }
        return Insert(btn || 'btnUpdate', true);
    };
    P.btnnew = function () { Reset(); ResetPmDetail(); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'BtnRefresh', function () { return HRM.get(API + '/refresh', { recId: RecId }).then(fillCombos).catch(HRM.fail); });
    };
    P.Print = function () { /* Print_Click is empty on the desktop */ };
    P.btnShowHistory = function (btn) { return HRM.busy(btn || 'btnShowHistory', HistoryFill); };
    P.btnResetHistory = function () {                                                                  // btnResetHistory_Click (always 3 days)
        HRM.setVal('hFromDate', HRM.addDays(HRM.today(), -3)); HRM.setVal('hToDate', HRM.today());
        HRM.setVal('FromDocNoHistory', ''); HRM.setVal('ToDocNoHistory', ''); HRM.setCombo('CmbCustomerHistory', 0);
        hist.clear(); pmHist.clear();
    };
    P.btnRefreshHistory = function (btn) {
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return HRM.get(API + '/history-combos').then(function (rows) { HRM.fill('CmbCustomerHistory', rows, 'Id', 'Customer', { zero: '', keep: true }); }).catch(HRM.fail);
        });
    };
    P.btnShortCutKey = function () {
        ImpB.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F1', 'For Open Delivery Term Popup'], ['Ctrl+F2', 'For Open Payterm Popup'], ['Ctrl+F3', 'For Open Sea Ports Popup'],
            ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Grids'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On First Column in Detail for Entry'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    };

    // ------------------------------------------------------------------ events
    ImpB.tabs();
    ImpB.onLeave('CmbInvoiceNo', CmbInvoiceNo_Leave);
    ImpB.onLeave('CmbItem', BindPmItemPackUom);
    HRM.$('CmbPackUom').addEventListener('change', CalculateWeight);
    HRM.$('txtQty').addEventListener('input', CalculateWeight);
    ['rdPackingItemName', 'rdPackingItemCode'].forEach(function (id) {                                 // rdPackingItemName_CheckedChanged
        HRM.$(id).addEventListener('change', function () { if (dtPMItem.length) { PackingItemFill(dtPMItem, true); HRM.focus('CmbItem'); } });
    });
    ImpB.wireDateChecks('h');
    function onForm() { return ImpB.activeTab('tc1') === 'tabPage1'; }
    HRM.keys({                                                                                         // frmInvoicePackingDetail_KeyDown
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+alt': P.btnShortCutKey, 'ctrl+alt+control': P.btnShortCutKey,
        'ctrl+t': function () { if (onForm()) { ImpB.showTab('tc1', 'tabPage2'); } else { ImpB.showTab('tc1', 'tabPage1'); HRM.focus('CmbItem'); } },
        'ctrl+s': function () { if (onForm()) { if (HRM.visible('btnUpdate') && RecId > 0) P.btnUpdate(); } else P.btnShowHistory(); },
        'ctrl+u': function () { if (onForm() && HRM.visible('btnUpdate') && RecId > 0) P.btnUpdate(); },
        'ctrl+f5': function () { if (onForm()) HRM.focus('CmbInvoiceNo'); else HRM.focus('hFromDate'); },
        'ctrl+n': function () { if (onForm()) P.btnnew(); else P.btnResetHistory(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(); else P.btnRefreshHistory(); },
        'ctrl+arrowup': function () { if (onForm()) HRM.focus('CmbItem'); else HRM.focus('hFromDate'); }
    });
    HRM.footer(function (b) { ImpB.showTab('tc1', 'tabPage2'); return HRM.busy(b, HistoryFill); });

    // ------------------------------------------------------------------ frmInvoicePackingDetail_Load
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.applyRights({ print: rights.print, update: rights.update || rights.save }, { update: 'btnUpdate', print: 'Print' });   // btnSave: no right check
        historyDays = d.historyDays;
        ImpB.historyDefaults('h', historyDays);
        HRM.check(d.itemSearchByCode ? 'rdPackingItemCode' : 'rdPackingItemName', true);
        fillCombos(d);
        HRM.fill('CmbCustomerHistory', d.customers, 'Id', 'Customer', { zero: '', keep: true });
        buttons(false);
        ResetPmDetail();
        HRM.focus('CmbInvoiceNo');
    }).catch(HRM.fail);
})();
