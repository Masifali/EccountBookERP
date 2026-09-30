/* ============================================================================================
 * countx_imports_lc_order_schedule.js - screen 526 "Import Sale Contract Schedule"
 * (Architecture.WinApp.Import/ImLcOrderSchedule.cs). Handlers follow the form's methods of the same names.
 * API /api/import/lc-order-schedule/*. Exports window.ImpASc.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/import/lc-order-schedule';
    var P = {};
    window.ImpASc = P;

    var S = {};
    var tablePendingContracts = [];
    var LstRemoveRecordMain = [], LstRemoveRecordDetail = [];
    var dtitem = [], dates = [], uomRows = [], dtHistory = [];
    var updateDetailIndex = -1;
    var grdPendingContract, grdContractShedule, grd, DataGridHistory, tabMain;
    var lastFocusGrid = 'grdPendingContract';

    function $(id) { return HRM.$(id); }
    function val(id) { return HRM.val(id); }
    function set(id, v) { HRM.setVal(id, v === null || v === undefined ? '' : v); }
    function num(v) { return HRM.num(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); }
    function box(m) { HRM.box(m); }
    function fail(m, f) { box(m); if (f) HRM.focus(f); return false; }
    function now() { return HRM.today(); }
    var F2 = '#,##0.##';

    /** DateTime.ToString() of a date (en-US general pattern). */
    function netDateText(iso) {
        if (!iso) return '';
        var p = iso.split('-');
        return (+p[1]) + '/' + (+p[2]) + '/' + p[0] + ' 12:00:00 AM';
    }
    /** ToShortDateString(). */
    function shortDate(v) { var d = ImpA.day(v); if (!d) return ''; var p = d.split('-'); return (+p[1]) + '/' + (+p[2]) + '/' + p[0]; }

    // ------------------------------------------------------------------------ grids
    function buildGrids() {
        grdPendingContract = new HRM.Grid('grdPendingContract', {                     // grdPendingContractSetting
            columns: [
                ImpA.hid('Id'), { key: 'ContractNo', caption: 'ContractNo', type: 'code', width: 90 },
                ImpA.dateCol('ContractDate', 'ContractDate'),
                ImpA.numCol('NoOfContainers', 'NoOfContainers', F2, true), ImpA.numCol('WeightM_Ton', 'WeightM_Ton', F2, true),
                ImpA.hid('SupplierCustomerId'), { key: 'SupplierName', caption: 'SupplierName', width: 170 },
                ImpA.hid('DestinationPortId'), ImpA.dateCol('LastShipmentDate', 'LastShipmentDate', true)
            ],
            totals: true, filterRow: true, emptyText: '',
            onCode: function (r) { grdPendingContract_ColumnButtonClick(r); },
            onDraw: function (g) { ImpA.footer(g, { NoOfContainers: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NoOfContainers'), F2); },
                WeightM_Ton: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'WeightM_Ton'), F2); } }); }
        });
        var portOpts = [['0', '']].concat((S.ports || []).map(function (p) { return [String(p.Id), HRM.str(p.PortName)]; }));
        grdContractShedule = new HRM.Grid('grdContractShedule', {                     // grdContractSheduleSettings
            columns: [
                ImpA.btnCol('Delete', 'X', 26), ImpA.btnCol('Add', '+', 26),
                ImpA.hid('Id'), ImpA.hid('ContractId'), ImpA.hid('SupplierCustomerId'),
                { key: 'SupplierName', caption: 'SupplierName', width: 160 },
                { key: 'NoOfContainers', caption: 'NoOfContainers', type: 'edit-num', width: 90, onChange: cellUpdated },
                { key: 'WeightM_Ton', caption: 'WeightM_Ton', type: 'edit-num', width: 90, onChange: cellUpdated },
                { key: 'AttentiveLoadingDate', caption: 'AttentiveLoadingDate', type: 'edit-date', width: 130 },
                { key: 'DestinationPortId', caption: 'Destination Port', type: 'select', options: portOpts, width: 150 },
                { key: 'AttentiveProductionDate', caption: 'AttentiveProductionDate', type: 'edit-date', width: 130 },
                { key: 'AttentiveInspectionDate', caption: 'AttentiveInspectionDate', type: 'edit-date', width: 130 }
            ],
            totals: true, emptyText: '',
            onDraw: function (g) { ImpA.footer(g, { NoOfContainers: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NoOfContainers'), F2); },
                WeightM_Ton: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'WeightM_Ton'), F2); } }); }
        });
        ImpA.buttons(grdContractShedule, 'grdContractShedule', function (r, act, i) { grdContractShedule_ColumnButtonClick(act, i); });

        grd = new HRM.Grid('grd', {                                                     // GridDetailSetting (grouped by AttentiveLoadingDate)
            columns: [
                ImpA.btnCol('Delete', 'X', 26),
                { key: 'AttentiveLoadingDate', caption: 'AttentiveLoadingDate', type: 'date', width: 110 },
                ImpA.hid('Id'), ImpA.hid('ScheduleId'), ImpA.hid('ContractId'), ImpA.hid('ItemId'),
                { key: 'Item', caption: 'Item', width: 170 }, { key: 'ItemCode', caption: 'ItemCode' },
                ImpA.hid('CropYearId'), { key: 'CropYear', caption: 'CropYear' },
                ImpA.hid('PackingTypeId'), { key: 'PackingType', caption: 'PackingType' },
                ImpA.numCol('NoOfBags', 'NoOfBags', F2, true), ImpA.hid('PackUomId'), { key: 'PackUom', caption: 'PackUom' },
                ImpA.numCol('WeightM_Ton', 'WeightM_Ton', F2, true)
            ],
            totals: true, emptyText: '',
            onDouble: function (r, i) { grd_DoubleClick(i); },
            onDraw: function (g) { ImpA.footer(g, { NoOfBags: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NoOfBags'), F2); },
                WeightM_Ton: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'WeightM_Ton'), F2); } }); }
        });
        ImpA.buttons(grd, 'grd', function (r, act, i) { if (act === 'Delete') grd_Delete(i); });

        var R = S.rights || {};
        var hc = [];
        if (R.print) hc.push({ key: '_sel', caption: '', type: 'edit-check', width: 34 });
        if (R.update) hc.push(ImpA.btnCol('Edit', 'Edit', 50));
        hc = hc.concat([
            { key: 'RecordNo', caption: 'RecordNo', type: 'int', width: 60 }, ImpA.hid('Id'), ImpA.hid('ImLcOrderId'),
            { key: 'ContractNo', caption: 'ContractNo' }, ImpA.hid('SupplierCustomerId'),
            { key: 'CustomerName', caption: 'SupplierName', width: 160 }, { key: 'ItemName', caption: 'ItemName', width: 160 },
            ImpA.numCol('NoOfContainer', 'NoOfContainer', F2, true), ImpA.numCol('MTon', 'MTon', F2, true),
            { key: 'DestinationPort', caption: 'DestinationPort' },
            ImpA.dateCol('AttentiveLoadingDate', 'LoadingDate'), ImpA.dateCol('AttentiveProductionDate', 'ProductionDate'),
            ImpA.dateCol('AttentiveInspectionDate', 'InspectionDate', true),
            { key: 'ScheduleRemarks', caption: 'Remarks' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }
        ]);
        DataGridHistory = new HRM.Grid('DataGridHistory', {                            // DataGridHistorySetting
            columns: hc, filterRow: true, totals: true, emptyText: 'No record found.', checkAll: R.print ? '_sel' : undefined,
            onDraw: function (g) { ImpA.footer(g, { NoOfContainer: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'NoOfContainer'), F2); },
                MTon: function (rs) { return ImpA.fmt(ImpA.sumOf(rs, 'MTon'), F2); } }); }
        });
        ImpA.buttons(DataGridHistory, 'DataGridHistory', function (r, act) { if (act === 'Edit') DataGridHistory_ColumnButtonClick(r); });
        ['grdPendingContract', 'grdContractShedule', 'grd'].forEach(function (id) {
            $(id).addEventListener('focusin', function () { lastFocusGrid = id; });
            $(id).addEventListener('click', function () { lastFocusGrid = id; });
        });
    }

    // ------------------------------------------------------------------------ load
    function fillHistoryCombos(d) {
        HRM.fill('cmbPort', d.historyPorts, 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('cmbSupplierName', d.historySuppliers, 'Id', 'Name', { zero: '', keep: true });
        HRM.fill('CmbItemHistory', d.historyItems, 'Id', 'Name', { zero: '', keep: true });
    }
    function load() {
        return HRM.loading(HRM.get(API + '/setup')).then(function (d) {
            S = d || {};
            buildGrids();
            HRM.fill('cmbCropYear', S.cropYears, 'Id', 'CropYear', { zero: '' });           // CropYearFill
            HRM.fill('cmbPackingtype', S.packingTypes, 'Id', 'Description', { zero: '' });  // ItemPackingTypeFill
            tablePendingContracts = S.contracts || [];
            grdPendingContract.set(tablePendingContracts);
            fillHistoryCombos(S);
            set('txtdatefrom', now()); set('txtdateto', now());
            grdContractShedule.set([]); AddRowInScheduleMainGrid();
            HRM.applyRights(S.rights || {}, { save: ['btnsave', 'btnSavedetailSchedule'], print: 'BtnHistoryPrint' });
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ pending contracts
    function GetpendingContractsforShipmentSchedule(all, btn) {
        var run = function () { return HRM.get(API + '/contracts', { all: !!all }).then(function (rows) {
            if ((rows || []).length) { tablePendingContracts = rows; grdPendingContract.set(rows); }
        }).catch(HRM.fail); };
        return btn ? HRM.busy(btn, run) : run();
    }
    P.BtnLoadAllPendingContracts = function (b) { return GetpendingContractsforShipmentSchedule(true, b); };
    P.BtnNewMain = function (b) {
        FormReset(); DetailFormReset();
        grdContractShedule.set([]); grd.set([]);
        return GetpendingContractsforShipmentSchedule(true, b);
    };

    /** Reads the contract's schedule / detail / dates / items (ReadbyMain.., ReadScheduleDetail.., AttentiveLoadDateFill.., ItemNameFill..). */
    function loadContract(contractId, fromPending, pendingRow) {
        return HRM.loading(HRM.get(API + '/load', { contractId: contractId })).then(function (d) {
            FormReset();
            var updateMain = (d.main || []).length > 0;
            if (updateMain) grdContractShedule.set(d.main);
            DetailFormReset();
            var updateDetail = (d.detail || []).length > 0;
            if (updateDetail) grd.set(d.detail);
            if (!updateMain) {
                if (fromPending && pendingRow) {
                    grdContractShedule.set([{ Id: 0, ContractId: pendingRow.Id, SupplierCustomerId: pendingRow.SupplierCustomerId, SupplierName: pendingRow.SupplierName,
                        NoOfContainers: pendingRow.NoOfContainers, WeightM_Ton: pendingRow.WeightM_Ton,
                        AttentiveLoadingDate: fromPending === 'pending' ? ImpA.day(pendingRow.LastShipmentDate) : now(),
                        DestinationPortId: fromPending === 'pending' ? pendingRow.DestinationPortId : 0,
                        AttentiveProductionDate: now(), AttentiveInspectionDate: now() }]);
                } else grdContractShedule.set([]);
            }
            if (!updateDetail) grd.set([]);
            dates = (d.dates || []).map(function (r) { return { ScheduleId: r.ScheduleId, AttentiveLoadingDate: shortDate(r.AttentiveLoadingDate), day: ImpA.day(r.AttentiveLoadingDate), ContractId: r.ContractId }; });
            HRM.fill('cmbAttentiveLoadDateBySchedule', dates, 'ScheduleId', 'AttentiveLoadingDate', { zero: '' });
            dtitem = d.items || [];
            fillItems(0);
        }).catch(HRM.fail);
    }
    function fillItems(keep) {
        HRM.fill('cmbItem', dtitem, 'Id', function (r) { return HRM.checked('rdbtnItemName') ? HRM.col(r, 'ItemName') : HRM.col(r, 'ItemCode'); }, { zero: '' });
        if (keep) HRM.setCombo('cmbItem', keep);
    }
    /** grdPendingContract_ColumnButtonClick (ContractNo link). */
    function grdPendingContract_ColumnButtonClick(r) {
        if (grd.rows().length > 0) {
            if (!HRM.ask('Are you sure to Reload Contract?')) return;
        }
        loadContract(HRM.int(r.Id), 'pending', r);
    }

    // ------------------------------------------------------------------------ main schedule grid
    function FormReset() {
        LstRemoveRecordMain = [];
        HRM.show('btnsave', true);
        grdContractShedule.set([]);
        AddRowInScheduleMainGrid();
    }
    P.btnnew = function () { FormReset(); DetailFormReset(); };
    function contractTotals(contractId) {
        var t = { w: 0, c: 0 };
        tablePendingContracts.some(function (p) { if (HRM.int(p.Id) === contractId) { t.w = num(p.WeightM_Ton); t.c = num(p.NoOfContainers); return true; } return false; });
        return t;
    }
    /** AddRowInScheduleMainGrid(). */
    function AddRowInScheduleMainGrid() {
        var rows = grdContractShedule.rows();
        if (rows.length === 0) {
            grdContractShedule.add({ Id: 0, ContractId: 0, SupplierCustomerId: 0, SupplierName: '', NoOfContainers: 0, WeightM_Ton: 0,
                AttentiveLoadingDate: now(), DestinationPortId: 0, AttentiveProductionDate: now(), AttentiveInspectionDate: now() });
            return;
        }
        var cur = grdContractShedule.current();
        if (!cur) { box('Object reference not set to an instance of an object.'); return; }
        var t = contractTotals(HRM.int(cur.ContractId));
        var w = grdContractShedule.sum('WeightM_Ton'), c = grdContractShedule.sum('NoOfContainers');
        var gw = t.w - w, gc = t.c - c;
        if ((gw > 0 || gc > 0) && gw > 0) {
            grdContractShedule.add({ Id: 0, ContractId: cur.ContractId, SupplierCustomerId: cur.SupplierCustomerId, SupplierName: cur.SupplierName,
                NoOfContainers: gc, WeightM_Ton: gw, AttentiveLoadingDate: cur.AttentiveLoadingDate, DestinationPortId: cur.DestinationPortId,
                AttentiveProductionDate: cur.AttentiveProductionDate, AttentiveInspectionDate: cur.AttentiveInspectionDate });
        } else box('Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ');
    }
    /** grdContractShedule_CellUpdated (NoOfContainers / WeightM_Ton). */
    function cellUpdated(item) {
        var t = contractTotals(HRM.int(item.ContractId));
        var ow = 0, oc = 0;
        grdContractShedule.rows().forEach(function (r) { if (r !== item) { ow += num(r.WeightM_Ton); oc += num(r.NoOfContainers); } });
        var rw = t.w - ow, rc = t.c - oc;
        if (num(item.WeightM_Ton) > 0 && num(item.NoOfContainers) > 0) {
            var nc = num(item.NoOfContainers), nw = num(item.WeightM_Ton);
            if (nc > rc) { box('NoOfContainers can not be Greater than total NoOfContainers of this Contract..'); item.NoOfContainers = ImpA.roundAway(rc, 2); }
            else item.NoOfContainers = Math.round(nc * 100) / 100;
            if (nw > rw) { box('WeightM_Ton can not be Greater than  total WeightM_Ton of This Contract..'); item.WeightM_Ton = ImpA.roundAway(rw, 2); }
            else item.WeightM_Ton = Math.round(nw * 100) / 100;
        }
        grdContractShedule.draw();
    }
    function grdContractShedule_ColumnButtonClick(act, i) {
        if (act === 'Delete') {
            var r = grdContractShedule.rows()[i];
            if (!r || grdContractShedule.rows().length <= 1) { box('You Can Not Delete All rows'); return; }
            if (HRM.int(r.Id) > 0) {
                if (!HRM.ask('Are you sure to Delete?')) return;
                LstRemoveRecordMain.push(r);
            }
            grdContractShedule.remove(i);
        }
        if (act === 'Add') AddRowInScheduleMainGrid();
    }
    function mainRow(r) {
        return { id: HRM.int(r.Id), contractId: HRM.int(r.ContractId), supplierCustomerId: HRM.int(r.SupplierCustomerId),
            noOfContainers: String(r.NoOfContainers === null || r.NoOfContainers === undefined ? '' : r.NoOfContainers),
            weightMTon: String(r.WeightM_Ton === null || r.WeightM_Ton === undefined ? '' : r.WeightM_Ton),
            attentiveLoadingDate: HRM.day(r.AttentiveLoadingDate), destinationPortId: HRM.int(r.DestinationPortId),
            attentiveProductionDate: HRM.day(r.AttentiveProductionDate), attentiveInspectionDate: HRM.day(r.AttentiveInspectionDate) };
    }
    /** InsertScheduleMain(). */
    P.btnsave = function (b) {
        var rows = grdContractShedule.rows();
        var upd = rows.some(function (r) { return HRM.int(r.Id) > 0; });
        if (!HRM.ask(upd ? 'Are you sure to Update Main Schedule?' : 'Are you sure to Save Main Schedule?')) return;
        return HRM.busy(b, function () {
            return HRM.post(API + '/save-main', { rows: rows.map(mainRow), removed: LstRemoveRecordMain.map(mainRow) }).then(function (d) {
                box(d && d.message ? d.message : 'Main Schedule Save SuccessFully ');
                FormReset();
            }).catch(HRM.fail);
        });
    };
    P.btnattachment = function () { box('Attachments are not available on the web page (the desktop copies files to a local attachment folder).'); };

    // ------------------------------------------------------------------------ detail schedule
    function DetailFormReset() {
        HRM.setCombo('cmbAttentiveLoadDateBySchedule', 0); HRM.setCombo('cmbItem', 0); HRM.setCombo('cmbPackUom', 0);
        set('txtNoOfBagsDetail', ''); set('txtMTonDetail', '');
        updateDetailIndex = -1;
        HRM.show('btnAdd', true); HRM.show('btnGridUpdate', false); HRM.show('btnCancel', false);
    }
    /** cmbAttentiveLoadDateBySchedule_ValueChanged -> GetTotalMTonOfDate. */
    function onDateChanged() {
        var r = HRM.comboRow('cmbAttentiveLoadDateBySchedule', 'ScheduleId');
        if (!r) { set('txtMtonOfSelectedDate', '0'); return; }
        HRM.get(API + '/schedule-mton', { contractId: HRM.int(r.ContractId), scheduleId: HRM.int(r.ScheduleId) })
            .then(function (d) { set('txtMtonOfSelectedDate', String(d && d.mton !== undefined ? d.mton : 0)); }).catch(HRM.fail);
    }
    /** bindItemPackUom(): keeps the pack UOM whose text is still in the new list. */
    function bindItemPackUom(keepId) {
        var text = HRM.comboText('cmbPackUom');
        var item = HRM.comboVal('cmbItem');
        if (!item) { uomRows = []; HRM.fill('cmbPackUom', [], 'Id', 'UOMCode', { zero: '' }); return Promise.resolve(); }
        return HRM.get(API + '/uoms', { itemId: item }).then(function (rows) {
            uomRows = rows || [];
            HRM.fill('cmbPackUom', uomRows, 'Id', 'UOMCode', { zero: '' });
            if (keepId) HRM.setCombo('cmbPackUom', keepId);
            else if (text) HRM.setComboText('cmbPackUom', text);
        }).catch(HRM.fail);
    }
    function packEq() {
        var v = HRM.comboVal('cmbPackUom'), hit = null;
        uomRows.forEach(function (r) { if (HRM.int(r.Id) === v) hit = r; });
        return hit ? num(hit.Equivalent) : 0;
    }
    /** CalculateMtonDetail(): which box drives the other follows the focused control's Tag. */
    function CalculateMtonDetail(source) {
        if (!HRM.comboVal('cmbPackUom')) return;
        var bags = num(val('txtNoOfBagsDetail')), eq = packEq(), mton = num(val('txtMTonDetail'));
        if (source === 'WeightMton') set('txtNoOfBagsDetail', eq ? ImpA.fmt(mton * 1000 / eq, '#,##0.##') : '');
        else set('txtMTonDetail', ImpA.plain(bags * eq / 1000));
    }
    function DetailFormvalidation() {
        if (!HRM.comboVal('cmbAttentiveLoadDateBySchedule')) return fail('Attentive Loading Date is Required!', 'cmbAttentiveLoadDateBySchedule');
        if (!HRM.comboVal('cmbItem')) return fail('Item is Required!', 'cmbItem');
        if (!HRM.comboVal('cmbCropYear')) return fail('CropYear is Required!', 'cmbCropYear');
        if (!HRM.comboVal('cmbPackingtype')) return fail('Packing Type is Required!', 'cmbPackingtype');
        if (val('txtNoOfBagsDetail') === '' || num(val('txtNoOfBagsDetail')) === 0) return fail('No Of Bags Feild is Required!', 'txtNoOfBagsDetail');
        if (!HRM.comboVal('cmbPackUom')) return fail('PackUom is Required!', 'cmbPackUom');
        if (val('txtMTonDetail') === '' || num(val('txtMTonDetail')) === 0) return fail('Weight/M.Ton Feild is Required!', 'txtMTonDetail');
        return true;
    }
    function boxRow(r) {
        var d = HRM.comboRow('cmbAttentiveLoadDateBySchedule', 'ScheduleId') || {};
        var it = HRM.comboRow('cmbItem', 'Id') || {};
        r = r || { Id: 0 };
        r.ScheduleId = HRM.int(d.ScheduleId); r.ContractId = HRM.int(d.ContractId); r.AttentiveLoadingDate = d.day;
        r.ItemId = HRM.comboVal('cmbItem'); r.Item = HRM.str(it.ItemName); r.ItemCode = HRM.str(it.ItemCode);
        r.CropYearId = HRM.comboVal('cmbCropYear'); r.CropYear = HRM.comboText('cmbCropYear');
        r.PackingTypeId = HRM.comboVal('cmbPackingtype'); r.PackingType = HRM.comboText('cmbPackingtype');
        r.NoOfBags = num(val('txtNoOfBagsDetail')); r.PackUomId = HRM.comboVal('cmbPackUom'); r.PackUom = HRM.comboText('cmbPackUom');
        r.WeightM_Ton = num(val('txtMTonDetail'));
        return r;
    }
    function mtonCheck(excludeIndex, msgWithDate) {
        var d = HRM.comboRow('cmbAttentiveLoadDateBySchedule', 'ScheduleId') || {};
        var target = d.day, sched = num(val('txtMtonOfSelectedDate')), used = 0;
        grd.rows().forEach(function (r, i) { if (i !== excludeIndex && ImpA.day(r.AttentiveLoadingDate) === target) used += num(r.WeightM_Ton); });
        if (num(val('txtMTonDetail')) + used > sched) {
            box(msgWithDate ? 'M.Ton Can not be Greater than Total M.Ton of this Contract against Date ' + netDateText(target) + '.Please Check M.Ton!'
                : 'M.Ton Can not be Greater than Total M.Ton of this Contract.Please Check M.Ton!');
            return false;
        }
        return true;
    }
    P.btnAdd = function () {
        if (!DetailFormvalidation()) return;
        if (!mtonCheck(-1, true)) return;
        grd.add(boxRow());
        DetailFormReset(); HRM.focus('cmbAttentiveLoadDateBySchedule');
    };
    function grd_DoubleClick(i) {
        var r = grd.rows()[i]; if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('cmbAttentiveLoadDateBySchedule', r.ScheduleId); onDateChanged();
        HRM.setCombo('cmbItem', r.ItemId);
        HRM.setCombo('cmbCropYear', r.CropYearId); HRM.setCombo('cmbPackingtype', r.PackingTypeId);
        bindItemPackUom(HRM.int(r.PackUomId)).then(function () {
            set('txtNoOfBagsDetail', ImpA.fmt(r.NoOfBags, F2)); set('txtMTonDetail', ImpA.fmt(r.WeightM_Ton, F2));
        });
        HRM.show('btnGridUpdate', true); HRM.show('btnCancel', true); HRM.show('btnAdd', false);
        HRM.focus('cmbAttentiveLoadDateBySchedule');
    }
    P.btnGridUpdate = function () {
        if (!DetailFormvalidation()) return;
        if (!mtonCheck(updateDetailIndex, false)) return;
        var r = grd.rows()[updateDetailIndex]; if (!r) return;
        grd.update(updateDetailIndex, boxRow(r));
        DetailFormReset(); HRM.focus('cmbAttentiveLoadDateBySchedule');
    };
    P.btnCancel = function () { DetailFormReset(); };
    function grd_Delete(i) {
        var r = grd.rows()[i]; if (!r) return;
        if (HRM.int(r.Id) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            LstRemoveRecordDetail.push(r);
        }
        grd.remove(i);
    }
    function detailRow(r) {
        return { id: HRM.int(r.Id), scheduleId: HRM.int(r.ScheduleId), contractId: HRM.int(r.ContractId), itemId: HRM.int(r.ItemId),
            cropYearId: HRM.int(r.CropYearId), packingTypeId: HRM.int(r.PackingTypeId), noOfBags: String(r.NoOfBags), packUomId: HRM.int(r.PackUomId),
            weightMTon: String(r.WeightM_Ton) };
    }
    /** InsertScheduleDetail(). */
    P.btnSavedetailSchedule = function (b) {
        var rows = grd.rows();
        var upd = rows.some(function (r) { return HRM.int(r.Id) > 0; });
        if (!HRM.ask(upd ? 'Are you sure to Update Detail Schedule?' : 'Are you sure to Save Detail Schedule?')) return;
        return HRM.busy(b, function () {
            return HRM.post(API + '/save-detail', { rows: rows.map(detailRow), removed: LstRemoveRecordDetail.map(detailRow) }).then(function (d) {
                box(d && d.message ? d.message : 'Detail Schedule Save SuccessFully');
                LstRemoveRecordDetail = [];
                DetailFormReset(); grd.set([]);
            }).catch(HRM.fail);
        });
    };
    P.btnNewDetailSchedule = function () { DetailFormReset(); grd.set([]); LstRemoveRecordDetail = []; };

    // ------------------------------------------------------------------------ History tab
    function DataGridHistoryFill(all, btn) {
        var run = function () {
            return HRM.get(API + '/history', { from: val('txtdatefrom'), to: val('txtdateto'), portId: HRM.comboVal('cmbPort'),
                supplierId: HRM.comboVal('cmbSupplierName'), itemId: HRM.comboVal('CmbItemHistory'), all: !!all }).then(function (rows) {
                dtHistory = rows || [];
                if (dtHistory.length) DataGridHistory.set(dtHistory.map(function (r) { return Object.assign({ _sel: false }, r); }));
                HRM.text('lblStatus', dtHistory.length + ' record(s)');
            }).catch(HRM.fail);
        };
        return btn ? HRM.busy(btn, run) : HRM.loading(run());
    }
    P.btnShow = function (b) { return DataGridHistoryFill(false, b); };
    P.btnLoadAllHistoryNew = function (b) { return DataGridHistoryFill(true, b); };
    P.btnRefreshHistoryCombos = function (b) {
        return HRM.busy(b, function () { return HRM.get(API + '/history-combos').then(fillHistoryCombos).catch(HRM.fail); });
    };
    function DataGridHistory_ColumnButtonClick(r) {
        tabMain.show('pgForm');
        loadContract(HRM.int(r.ImLcOrderId), 'history', null);
    }
    /** BtnHistoryPrint_Click: the checked rows of dtHistory in RecordNo order -> 239-ImLcOrderSchedule_FormHistory.rpt. */
    P.BtnHistoryPrint = function (b) {
        if (!DataGridHistory.current()) return;
        var checked = DataGridHistory.rows().filter(function (r) { return HRM.bool(r._sel); });
        if (!checked.length) return;
        var ids = {};
        checked.forEach(function (r) { ids[HRM.int(r.Id)] = HRM.int(r.RecordNo); });
        var rows = dtHistory.filter(function (h) { return ids[HRM.int(h.Id)] !== undefined; })
            .sort(function (a, c) { return ids[HRM.int(a.Id)] - ids[HRM.int(c.Id)]; });
        if (!rows.length) { box('Not Record Found For Display'); return; }
        if (!(S.rights || {}).print) return;                                  // BtnHistoryPrint.Enabled = DoHavePrintRights
        return ImpA.gridPrint(b, '239-ImLcOrderSchedule_FormHistory.rpt', 'Import Lc Order Schedule', rows);
    };

    var SHORTCUTS = [['Ctrl+S', 'For Save of Focused Grid'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New of Focused Grid'], ['Ctrl+L', 'For Load All'],
        ['Ctrl+P', 'For Print'], ['Ctrl+F5', "For Focus on 'Sale Contract for Schedule' Grid"], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Right', 'For Move focus From One Grid to Next'], ['Ctrl+ArrowDown', 'For Focus On Schedule Detail Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Date Combo in Detail Box'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '],
        ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];
    P.BtnShortCutkeys = function () { ImpA.shortcuts(SHORTCUTS); };

    // ------------------------------------------------------------------------ wiring
    function wire() {
        tabMain = ImpA.tabs('tabMain', function (id) { if (id === 'pgHistory') DataGridHistoryFill(false); });   // tabControl1_SelectedIndexChanged
        HRM.footer(function () { tabMain.show('pgHistory'); });
        $('cmbAttentiveLoadDateBySchedule').addEventListener('change', onDateChanged);
        $('cmbItem').addEventListener('change', function () { bindItemPackUom(); });                              // cmbItem_Leave
        $('rdbtnItemName').addEventListener('change', function () { if (dtitem.length) { fillItems(HRM.comboVal('cmbItem')); HRM.focus('cmbItem'); } });
        $('rdbtnItemCode').addEventListener('change', function () { if (dtitem.length) { fillItems(HRM.comboVal('cmbItem')); HRM.focus('cmbItem'); } });
        $('txtNoOfBagsDetail').addEventListener('input', function () { CalculateMtonDetail('NoOfBags'); });
        $('txtMTonDetail').addEventListener('input', function () { CalculateMtonDetail('WeightMton'); });
        $('cmbPackUom').addEventListener('change', function () { CalculateMtonDetail('PackUom'); });
        var R = function () { return S.rights || {}; };
        HRM.keys({
            'ctrl+t': function () { if (tabMain.current === 'pgHistory') tabMain.show('pgForm'); else tabMain.show('pgHistory'); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+n': function () {
                if (tabMain.current !== 'pgForm') return;
                if (lastFocusGrid === 'grdPendingContract') P.BtnNewMain($('BtnNewMain'));
                else if (lastFocusGrid === 'grdContractShedule') P.btnnew();
                else P.btnNewDetailSchedule();
            },
            'ctrl+l': function () { if (tabMain.current === 'pgForm') P.BtnLoadAllPendingContracts($('BtnLoadAllPendingContracts')); else P.btnLoadAllHistoryNew($('btnLoadAllHistoryNew')); },
            'ctrl+s': function () {
                if (tabMain.current !== 'pgForm') { P.btnShow($('btnShow')); return; }
                if (!R().save) return;
                if (lastFocusGrid === 'grd') P.btnSavedetailSchedule($('btnSavedetailSchedule'));
                else if (lastFocusGrid === 'grdContractShedule') P.btnsave($('btnsave'));
            },
            'ctrl+p': function () { if (tabMain.current === 'pgHistory' && R().print) P.BtnHistoryPrint($('BtnHistoryPrint')); },
            'ctrl+r': function () { if (tabMain.current === 'pgHistory') P.btnRefreshHistoryCombos($('btnRefreshHistoryCombos')); },
            'ctrl+arrowup': function () { HRM.focus(tabMain.current === 'pgForm' ? 'cmbAttentiveLoadDateBySchedule' : 'txtdatefrom'); },
            'ctrl+f10': function () { if (tabMain.current === 'pgForm') P.btnattachment(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
})();
