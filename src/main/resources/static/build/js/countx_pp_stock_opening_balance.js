/* ============================================================================================
 * countx_pp_stock_opening_balance.js - 678 Stock Opening Balance (Party Processing), DocumentTypeId 121
 * Architecture.WinApp.PartyProcessing.StockOpeningBalancePartyProcessing. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/stock-opening-balance/{setup|refresh|doc-no|uoms|by-id|save|delete|history|print-check}
 * Print: pp-416_01 through countx_crystal_print.js.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/stock-opening-balance';
    var P = {};
    window.PpOpening = P;
    var RECID = 0, UpdateMode = false, rights = {}, uomRows = [];
    var pickerTime = HRM.nowTime() + ':00';                      // DateTimePicker.Value keeps the time it was created with

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    function btnCell(text, cls) { return '<button type="button" class="win-btn-action pp-cell-btn ' + cls + '">' + text + '</button>'; }

    var grd = new HRM.Grid('DataGridHistory', {
        columns: [
            { key: '_edit', caption: 'Edit', width: 44, render: function () { return btnCell('Edit', 'pp-edit'); } },
            { key: '_print', caption: 'Print', width: 44, render: function () { return btnCell('Print', 'pp-print'); } },
            { key: 'Id', hidden: true },
            { key: 'DocNo', caption: 'DocNo', type: 'int', width: 60 },
            { key: 'DocDate', caption: 'DocDate', type: 'date', width: 90 },
            { key: 'StockParty', caption: 'StockParty', width: 170 },
            { key: 'RefParty', caption: 'RefParty', width: 150 },
            { key: 'Warehouse', caption: 'Warehouse', width: 130 },
            { key: 'ItemName', caption: 'ItemName', width: 170 },
            { key: 'CropYear', caption: 'CropYear', width: 80 },
            { key: 'JobLot', caption: 'JobLot', width: 110 },
            { key: 'PackUom', caption: 'PackUom', width: 80 },
            { key: 'PackingType', caption: 'PackingType', width: 100 },
            { key: 'Qty', caption: 'Qty', width: 80, sum: true, render: fmt3, align: 'right' },
            { key: 'Weight', caption: 'Weight', width: 90, sum: true, render: fmt3, align: 'right' },
            { key: 'EntryUser', caption: 'EntryUser', width: 100 },
            { key: 'EntryDate', caption: 'EntryDate', type: 'datetime', width: 140 },
            { key: 'ModifyUser', caption: 'ModifyUser', width: 100 },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime', width: 140 },
            { key: 'ApprovedUser', caption: 'ApprovedUser', width: 100 },
            { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime', width: 140 },
            { key: 'Remarks', caption: 'Remarks', width: 200 }
        ],
        filterRow: true, totals: true,
        onDouble: function (r) { FormReset(); ReadById(HRM.int(r.Id)); }       // DataGridHistory_DoubleClick
    });
    /** "#,##0.###" */
    function fmt3(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v), s = HRM.fmtNum(n, 3);
        return HRM.esc(s.replace(/\.?0+$/, ''));
    }
    HRM.$('DataGridHistory').addEventListener('click', function (e) {    // DataGridHistory_ColumnButtonClick
        var b = e.target.closest('button.pp-cell-btn'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var r = grd.rows()[+tr.getAttribute('data-i')];
        if (b.classList.contains('pp-edit')) ReadById(HRM.int(r.Id));
        else print416(b, HRM.int(r.Id));
    });

    // ------------------------------------------------------------------ combos
    function fillCombos(d) {
        HRM.fill('cmbStockParty', d.stockParties || [], 'Id', 'CompanyName', { zero: '', keep: true });
        HRM.fill('CmbRefParty', d.refParties || [], 'Id', 'ReferencePartyName', { zero: '', keep: true });
        HRM.fill('cmbWarehouse', d.warehouses || [], 'Id', 'WareHouseName', { zero: '', keep: true });
        HRM.fill('cmbItem', d.items || [], 'Id', 'ItemName', { zero: '', keep: true });
        HRM.fill('cmbCrop', d.cropYears || [], 'Id', 'CropYear', { zero: '', keep: true });
        HRM.fill('CmbPackingType', d.packingTypes || [], 'Id', 'PackTypeDesc', { zero: '', keep: true });
        HRM.fill('cmbJobLot', d.jobLots || [], 'Id', 'JobLotDescription', { zero: '', keep: true });
    }

    /** UOMFill(ItemId): the Pack Uom list; the previous Uom text kept when the new list has it. */
    function fillUoms(rows) {
        var packUom = HRM.comboText('cmbUOM');
        uomRows = rows || [];
        HRM.fill('cmbUOM', uomRows, 'Id', 'UOMCode', { zero: '' });
        if (packUom) HRM.setComboText('cmbUOM', packUom);
        CalculateWeight();
    }
    function UOMFill(itemId) {
        return HRM.get(API + '/uoms', { itemId: itemId }).then(fillUoms).catch(HRM.fail);
    }
    HRM.$('cmbItem').addEventListener('change', function () { UOMFill(HRM.comboVal('cmbItem')); });   // cmbItem_Leave

    /** CalculateWeight(): Qty x the Pack Uom's Equivalent, "#,##0.###"; "0" when either is missing. */
    function CalculateWeight() {
        var q = HRM.val('txtQty');
        if (q !== '' && HRM.num(q) !== 0 && HRM.comboText('cmbUOM') !== '') {
            var u = HRM.comboRow('cmbUOM', 'Id');
            var eq = u ? HRM.num(u.Equivalent) : 0;
            var n = HRM.num(q) * eq;
            HRM.setVal('txtWeight', HRM.fmtNum(n, 3).replace(/\.?0+$/, ''));
        } else HRM.setVal('txtWeight', '0');
    }
    HRM.$('txtQty').addEventListener('input', CalculateWeight);            // txtQty_TextChanged
    HRM.$('cmbUOM').addEventListener('change', CalculateWeight);           // cmbUOM_TextChanged

    function GenerateDocNo() {
        return HRM.get(API + '/doc-no').then(function (d) { HRM.setVal('txtDocNo', HRM.str(d.docNo)); }).catch(HRM.fail);
    }

    function modeButtons(update) {
        HRM.show('btnSave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update);
    }

    /** FormReset(). */
    function FormReset() {
        modeButtons(false);
        UpdateMode = false;
        RECID = 0;
        HRM.setVal('txtDocdate', HRM.today());
        ['cmbWarehouse', 'cmbItem', 'cmbUOM', 'cmbJobLot', 'cmbCrop', 'CmbPackingType', 'CmbRefParty', 'cmbStockParty'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.setVal('txtQty', ''); HRM.setVal('txtWeight', ''); HRM.setVal('txtRemarks', '');
        HRM.focus('txtDocdate');
        return GenerateDocNo();
    }

    /** ReadById(Id). */
    function ReadById(id) {
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            RECID = id;
            HRM.setVal('txtDocNo', HRM.str(o.DocNo));
            HRM.setVal('txtDocdate', HRM.day(o.DocDate));
            HRM.setCombo('cmbWarehouse', o.WarehouseId);
            HRM.setCombo('cmbItem', o.ItemId);
            fillUoms(o.uoms);
            HRM.setCombo('cmbUOM', o.ItemUomSch);
            HRM.setCombo('cmbJobLot', o.JobLotId);
            HRM.setCombo('CmbPackingType', o.PackingTypeId);
            HRM.setVal('txtQty', HRM.str(o.Qty));
            HRM.setVal('txtWeight', HRM.str(o.WeightKgs));
            HRM.setVal('txtRemarks', HRM.str(o.Remarks));
            HRM.setCombo('cmbStockParty', o.StockPartyId);
            HRM.setCombo('CmbRefParty', o.SupplierCustomerId);
            HRM.setCombo('cmbCrop', 0); HRM.setComboText('cmbCrop', HRM.str(o.CropYear));
            UpdateMode = true;
            modeButtons(true);
        }).catch(function (e) { HRM.fail(e); FormReset(); });
    }

    /** FormValidation(): order and wording of the form. */
    function FormValidation() {
        var dn = HRM.val('txtDocNo').trim();
        if (dn === '' || dn === '0') { HRM.box('Doc No Field Required'); HRM.focus('txtDocNo'); return false; }
        var checks = [['cmbStockParty', 'StockParty Field Required'], ['CmbRefParty', 'Ref Party Field Required'], ['cmbWarehouse', 'WareHouse Field Required'],
            ['cmbItem', 'Item Name Field Required'], ['cmbCrop', 'CropYear Field Required'], ['cmbJobLot', 'JobLot Field Required'],
            ['cmbUOM', 'Pack Uom Field Required'], ['CmbPackingType', 'Packing Type Field Required']];
        for (var x = 0; x < checks.length; x++) {
            if (HRM.comboVal(checks[x][0]) === 0) { HRM.box(checks[x][1]); HRM.focus(checks[x][0]); return false; }
        }
        if (HRM.val('txtQty').trim() === '' || HRM.num(HRM.val('txtQty').trim()) === 0) { HRM.box('Qty Field Required'); HRM.focus('txtQty'); return false; }
        if (HRM.val('txtWeight').trim() === '' || HRM.num(HRM.val('txtWeight').trim()) === 0) { HRM.box('Weight Field Required'); HRM.focus('txtWeight'); return false; }
        return true;
    }

    function Insert(btn) {
        if (!FormValidation()) return;
        if (!HRM.ask(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var printAfter = HRM.checked('ChkBox');
        var win = printAfter && window.CrystalPrint ? CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RECID, docNo: HRM.val('txtDocNo'), docDate: HRM.val('txtDocdate') + 'T' + pickerTime,
                stockPartyId: HRM.comboVal('cmbStockParty'), refPartyId: HRM.comboVal('CmbRefParty'), remarks: HRM.val('txtRemarks'),
                warehouseId: HRM.comboVal('cmbWarehouse'), itemId: HRM.comboVal('cmbItem'), cropYearId: HRM.comboVal('cmbCrop'),
                jobLotId: HRM.comboVal('cmbJobLot'), packingTypeId: HRM.comboVal('CmbPackingType'), uomId: HRM.comboVal('cmbUOM'),
                qty: HRM.val('txtQty'), weight: HRM.val('txtWeight')
            }).then(function (d) {
                HRM.box(d.message);
                FormReset();
                if (printAfter) {                                        // ChkBox: StockOpeningBalancePartyProcSlipandRegister416_01(Success)
                    return HRM.get(API + '/print-check', { id: d.id }).then(function () {
                        return CrystalPrint.open('pp-416_01', { id: d.id }, null, win);
                    }).catch(function (e) { if (win) CrystalPrint.release(win); HRM.fail(e); });
                }
            }).catch(function (e) { if (win) CrystalPrint.release(win); HRM.fail(e); });
        }, 'pp-opening-save');
    }

    function print416(btn, id) {
        var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(API + '/print-check', { id: id }).then(function () {
                return CrystalPrint.open('pp-416_01', { id: id }, null, win);
            }).catch(function (e) { if (win) CrystalPrint.release(win); HRM.fail(e); });
        }, 'pp-416');
    }

    P.btnSave = function (btn) { RECID = 0; return Insert(btn || 'btnSave'); };
    P.btnUpdate = function (btn) {
        if (RECID === 0) { HRM.box('Record not found'); return; }
        return Insert(btn || 'btnUpdate');
    };
    P.btnDelete = function (btn) {
        if (RECID === 0) { HRM.box('Record Id Not Found....'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return HRM.post(API + '/delete', { id: RECID }).then(function (d) { HRM.box(d.message); return FormReset(); }).catch(HRM.fail);
        });
    };
    P.btnPrint = function (btn) {
        if (RECID === 0) { HRM.box('Record not found'); return; }
        return print416(btn || 'btnPrint', RECID);
    };
    P.BtnNew = function () { FormReset(); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/refresh').then(fillCombos).catch(HRM.fail); });
    };
    P.btnAttachment = function () { HRM.box('Attachments are not available on the web page yet.'); };
    P.btnFormReferenceParty = function () { HRM.open('/party-processing/reference-parties'); };

    /** BindGridHistory(): the filters as the form reads them; the grid is rebound only when rows came back. */
    P.btnShow = function (btn) {
        var kind = (document.querySelector('input[name="histDate"]:checked') || {}).value || 'doc';
        return HRM.busy(btn || 'btnShow', function () {
            return HRM.post(API + '/history', {
                dateKind: kind,
                fromChecked: HRM.checked('chkFromDateHistory'), fromDate: HRM.val('FromDateHistory') ? HRM.val('FromDateHistory') + 'T' + pickerTime : '',
                toChecked: HRM.checked('chkToDateHistory'), toDate: HRM.val('ToDateHistory') ? HRM.val('ToDateHistory') + 'T' + pickerTime : '',
                docNoFrom: HRM.val('FromDocNoHistory'), docNoTo: HRM.val('ToDocNoHistory'), stockPartyId: HRM.comboVal('cmbstockpartyhistory')
            }).then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
        });
    };

    P.shortCuts = function () {
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus On Doc No. in Stock Opening Balance'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus on WareHouse'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        HRM.modal({ title: 'ShortCut Keys', width: 'min(560px, 96vw)', html: '<table class="win-grid hrm-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + HRM.esc(r[0]) + '</td><td>' + HRM.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>' });
    };

    HRM.keys({                                                  // frmOpeningStockBlancing_KeyDown (Enter -> {TAB})
        'ctrl+s': function () { if (!HRM.$('btnSave').disabled && !UpdateMode) P.btnSave(); },
        'ctrl+n': function () { P.BtnNew(); },
        'ctrl+r': function () { P.btnRefresh(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+u': function () { if (!HRM.$('btnUpdate').disabled && UpdateMode) P.btnUpdate(); },
        'ctrl+p': function () { if (!HRM.$('btnPrint').disabled && HRM.visible('btnPrint')) P.btnPrint(); },
        'ctrl+arrowdown': function () { var t = HRM.$('DataGridHistory').querySelector('tbody tr[data-i]'); if (t) t.focus(); },
        'ctrl+arrowup': function () { HRM.focus('cmbWarehouse'); },
        'ctrl+f5': function () { HRM.focus('txtDocNo'); },
        'ctrl+enter': function () { var r = grd.current(); if (r) ReadById(HRM.int(r.Id)); },
        'ctrl+alt+control': P.shortCuts, 'ctrl+alt+alt': P.shortCuts
    });
    HRM.footer(function (b) {
        var x = document.getElementById('historyBox');
        return P.btnShow(b).then(function () { if (x) x.scrollIntoView({ block: 'start', behavior: 'smooth' }); });
    });

    /** frmOpeningStockBlancing_Load. */
    HRM.setVal('txtDocdate', HRM.today());
    HRM.setVal('FromDateHistory', HRM.today());
    HRM.setVal('ToDateHistory', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.enable('btnSave', rights.save !== false);
        HRM.enable('btnUpdate', rights.update !== false);
        HRM.enable('btnPrint', rights.print !== false);
        HRM.enable('btnDelete', rights['delete'] !== false);
        HRM.setVal('txtDocNo', HRM.str(d.docNo));
        fillCombos(d);
        if (d.cropYears && d.cropYears.length) HRM.setCombo('cmbCrop', d.cropYears[0].Id);     // CropYear(): Rows[1] (first year) active
        HRM.fill('cmbstockpartyhistory', d.historyParties || [], 'Id', 'name', { zero: '' });
        HRM.fill('cmbUOM', [], 'Id', 'UOMCode', { zero: '' });
        modeButtons(false);
        HRM.focus('txtDocdate');
    }).catch(HRM.fail);
})();
