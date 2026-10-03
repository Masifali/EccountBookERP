/* ============================================================================================
 * countx_export_contract_schedule.js - frmSaleContractSchedule.cs (Architecture.WinApp.Export), screen 216
 * "Export Contract Schedule" (ScreenName FrmExportSalesContractSchedule, DocumentTypeId 244).
 * Main Form (Form: Contracts Info / Contract Schedule (editable grid) / Schedule Brand Detail; History) |
 * Pre Shipment Planning. Every button, grid button, cell update, Leave/TextChanged, double-click and
 * shortcut of the desktop form has its counterpart here; data from /api/export/contract-schedule
 * (ExportContractScheduleController -> ExportContractScheduleService).
 * Prints: exp-395 (395-Slip / ScheduleCode link), exp-551 (551-Print), exp-551-01 (551_01-Print),
 * exp-501new (ContractNo link in Pre Shipment Planning).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExportG;
    var API = '/api/export/contract-schedule';
    var $id = X.$id, box = X.box, ask = X.ask, esc = X.esc, str = X.str, netI = X.netI, netD = X.netD, fmt = X.fmt;
    var val = X.val, valI = X.valI, valD = X.valD, setVal = X.setVal, setText = X.setText, text = X.text, bind = X.bind, show = X.show, focus = X.focus, checked = X.checked;

    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { skipFCLContractValidationOnSchedule: false, exportSalesContractApprovalMandatory: false };
    var FEAT = { sdf: false, pmrp: false };
    var L = { ports: [], cropYears: [], packTypes: [], plants: [], historyPorts: [], historyCustomers: [], historyItems: [], customGroups: [] };
    var S = { contractId: 0, pendingRow: null, main: [], removedMain: [], detail: [], removedDetail: [], loadDates: [], items: [], uoms: [], updateIndex: -1, mtonByDate: 0,
        contractWeight: 0, contractContainers: 0, deptRecId: 0, deptRow: null, latestLoadDate: '' };
    var PENDING = [], HIST = [], DEPT = [];
    var tabsMain, tabs1;

    // ------------------------------------------------------------------------------ grids

    var pendGrid = X.grid('grdPendingContract', [
        { key: 'ContractNo', link: true }, { key: 'ContractDate', fmt: X.ddMMMyyyy }, { key: 'NoOfContainers', num: true, dec: 2 }, { key: 'WeightM_Ton', num: true },
        { key: 'CustomerName' }, { key: 'ApprovedStatus' }
    ], { totals: ['NoOfContainers', 'WeightM_Ton'], emptyId: 'csPendEmpty', rowClass: function (r) { return X.netB(r.IsApproved) ? 'exg-row-approved' : ''; },
        onLink: function (i) { pendingContractClick(i); }, onDblClick: function (i) { pendingContractClick(i); } });
    var detGrid = X.grid('grd', [
        { key: 'DestinationPort' }, { key: 'Item' }, { key: 'ItemCode' }, { key: 'CropYear' }, { key: 'PackingType' }, { key: 'NoOfBags', num: true }, { key: 'PackUom' },
        { key: 'InnerQty', num: true }, { key: 'WeightM_Ton', num: true }, { key: 'Status' }, { key: 'ReferredStatus' }
    ], { withX: true, totals: ['NoOfBags', 'InnerQty', 'WeightM_Ton'], rowClass: function (r) { return str(r.ReferredStatus) === 'Referred' ? 'exg-referred' : ''; },
        onDelete: deleteDetail, onDblClick: editDetail });
    var histGrid = X.grid('DataGridHistory', [
        { key: 'RecordNo', num: true, dec: 0 }, { key: 'ContractNo' }, { key: 'CustomerName' }, { key: 'ItemName' }, { key: 'NoOfContainer', num: true, dec: 0 }, { key: 'MTon', num: true },
        { key: 'Contract_MTon', num: true }, { key: 'MtonUsedInForwarding', num: true }, { key: 'ContractBalMton', num: true }, { key: 'DestinationPort' },
        { key: 'LoadingDate', fmt: X.ddMMMyyyy }, { key: 'ProductionDate', fmt: X.ddMMMyyyy }, { key: 'PackingMaterialDate', fmt: X.ddMMMyyyy }, { key: 'Remarks' }, { key: 'Status' },
        { key: 'EntryDate', fmt: X.ddMMMyyyy }, { key: 'EntryUser' }, { key: 'NoOfAttachments', num: true, dec: 0, link: true }
    ], { checkbox: true, buttons: [{ key: 'Edit', text: 'Edit' }], totals: ['NoOfContainer', 'MTon', 'Contract_MTon', 'MtonUsedInForwarding', 'ContractBalMton'], emptyId: 'csHistEmpty',
        onButton: function (i) { historyEdit(i); }, onDblClick: function (i) { historyEdit(i); }, onLink: function () { box('Attachments (DMS) are not available in the web port.'); } });
    var deptGrid = X.grid('grdDepartmentDetail', [
        { key: 'ScheduleCode', link: true }, { key: 'ContractNo', link: true }, { key: 'CustomerName' }, { key: 'DestinationPort' }, { key: 'FCL', num: true, dec: 2 }, { key: 'ScheduleMTon', num: true },
        { key: 'LoadingDate', fmt: X.ddMMMyyyy }, { key: 'ItemCode', link: true }, { key: 'ItemName', link: true }, { key: 'ItemMTon', num: true }, { key: 'ProductionDays', num: true, dec: 0 },
        { key: 'ProductionSchedule', fmt: X.ddMMMyyyy }, { key: 'ProductionStart', fmt: X.ddMMMyyyy, link: true }, { key: 'InspectionSchedule', fmt: X.ddMMMyyyy }, { key: 'InspectionStart', fmt: X.ddMMMyyyy, link: true },
        { key: 'PackMaterialSchedule', fmt: X.ddMMMyyyy }, { key: 'PackMaterialStart', fmt: X.ddMMMyyyy, link: true }, { key: 'PlantName' }, { key: 'ScheduleLockStatus' }
    ], { buttons: [{ key: 'Edit', text: 'Edit' }], totals: ['FCL', 'ScheduleMTon', 'ItemMTon'], emptyId: 'csDeptEmpty', rowClass: function (r) { return netI(r.ScheduleLockId) === 1 ? 'exg-confirmed' : ''; },
        onButton: function (i) { deptReadById(i); }, onDblClick: function (i) { deptReadById(i); }, onLink: deptLink });

    // ------------------------------------------------------------------------------ load

    function setup() {
        return X.getJson(API + '/setup').then(function (d) {
            PERM = d.permissions || PERM;
            X.setEnabled('btnsave', !!PERM.Save); X.setEnabled('btnSavedetailSchedule', !!PERM.Save); X.setEnabled('BtnHistoryPrint', !!PERM.Print);
            FEAT.sdf = !!d.shipmentDocumentsFeature; FEAT.pmrp = !!d.packingMaterialPlanningFeature;
            show('BtnShipmentDocumentForm', FEAT.sdf); show('BtnContractPmDetailForm', FEAT.pmrp);
            var th = $id('thCustomGroup'); if (th) th.classList.toggle('is-hidden', !FEAT.sdf);
            if (d.config) CFG = d.config;
            L.ports = d.ports || []; L.cropYears = d.cropYears || []; L.packTypes = d.packTypes || []; L.plants = d.plants || [];
            bind('cmbCropYear', L.cropYears, 'Id', 'name'); bind('cmbPackingtype', L.packTypes, 'Id', 'name'); bind('CmbPlantDepartmentDetail', L.plants, 'Id', 'name');
            setVal('CmbScheduleLockStatusDepartmentDetail', 1);
            PENDING = d.pending || []; pendGrid.draw(PENDING); if (d.pendingError) box(d.pendingError);
            historyCombos(d);
            var t = X.today();
            setText('txtdatefrom', t); setText('txtdateto', t);
            /* txtDateFromDepart = now; LastScheduleLoadingDate > now -> Add Days = (now - last).Days (negative), else From = last; To = last. */
            S.latestLoadDate = X.isoDate(d.latestAttentiveLoadDate);
            setText('txtDateFromDepart', t);
            if (S.latestLoadDate && S.latestLoadDate > t) setText('tztAddDays', str(X.daysBetween(t, S.latestLoadDate)));
            else if (S.latestLoadDate) setText('txtDateFromDepart', S.latestLoadDate);
            setText('txtDatetoDepart', S.latestLoadDate || t);
            var g = $id('grdPendingContract'); if (g) g.focus();
        });
    }
    function historyCombos(d) {
        L.historyPorts = d.historyPorts || []; L.historyCustomers = d.historyCustomers || []; L.historyItems = d.historyItems || [];
        bind('cmbPort', L.historyPorts, 'Id', 'name'); bind('cmbSupplierName', L.historyCustomers, 'Id', 'name'); bind('CmbItemHistory', L.historyItems, 'Id', 'name');
    }
    function referredAction() { return netI(X.radio('csReferred')); }
    function loadPending(noOfRecords, btn) {
        return X.busy(btn, function () {
            return X.getJson(API + '/pending?actionId=' + referredAction() + '&noOfRecords=' + noOfRecords).then(function (rows) { PENDING = rows || []; pendGrid.current = -1; pendGrid.draw(PENDING); });
        });
    }
    function loadAllPending(btn) { if (tabs1.current() === 'csForm') return loadPending(0, btn); }
    /* BtnNewMain_Click */
    function newMain(btn) { formReset(); detailFormReset(); S.removedDetail = []; S.items = []; S.uoms = []; bind('cmbItem', [], 'Id', 'name'); bind('cmbPackUom', [], 'Id', 'UOMCode'); S.main = []; S.detail = []; drawMain(); detGrid.draw([]); return loadPending(50, btn); }
    function formReset() {
        S.contractId = 0; S.pendingRow = null; S.removedMain = []; S.main = []; S.detail = []; S.loadDates = []; S.contractWeight = 0; S.contractContainers = 0;
        bind('cmbAttentiveLoadDateBySchedule', [], 'ScheduleId', 'AttentiveLoadingDate');
        $id('csContractInfo').textContent = ''; $id('csFooterInfo').textContent = '';
    }

    // ------------------------------------------------------------------------------ contract load

    function pendingContractClick(i) {
        var r = PENDING[i]; if (!r) return;
        try {
            if (CFG.exportSalesContractApprovalMandatory && !X.netB(r.IsApproved)) throw new Error('Please select approved contract');
            loadContract(netI(r.Id), r);
        } catch (e) { box(e.message); }
    }
    /* grdPendingContractByContractId / ReadById */
    function loadContract(contractId, pendingRow) {
        if ((S.detail.length > 0 || S.main.length > 0) && pendingRow) { if (!ask('Are you sure to Reload Contract?')) return Promise.resolve(); }
        return X.getJson(API + '/contract?contractId=' + contractId).then(function (d) {
            S.contractId = contractId; S.pendingRow = pendingRow || X.rowOf(PENDING, 'Id', contractId); S.removedMain = []; S.removedDetail = []; S.updateIndex = -1;
            S.contractWeight = netD(d.contractWeightMTon); S.contractContainers = netD(d.contractNoOfContainers);
            L.customGroups = d.customGroups || [];
            var main = d.scheduleMain || [];
            var updateModeMain = main.length > 0;
            if (updateModeMain) S.main = main;
            else if (S.pendingRow) {
                var r = S.pendingRow, t = X.today();
                S.main = [{ Id: 0, ContractId: netI(r.Id), ScheduleNo: str(r.ContractNo) + '/1', SupplierCustomerId: netI(r.SupplierCustomerId), CustomerName: str(r.CustomerName), CustomerContractNo: '',
                    NoOfContainers: netD(r.NoOfContainers), WeightM_Ton: netD(r.WeightM_Ton), AttentiveLoadingDate: X.isoDate(r.StartShipmentDate) || t, DestinationPortId: netI(r.DestinationPortId),
                    AttentiveProductionDate: t, AttentiveInspectionDate: t, PackingMaterialDate: t, CustomGroupId: 0, Remarks: '', Status: '', ReferredStatus: '', NoOfAttachments: 0, IsApproved: X.netB(r.IsApproved) }];
            } else S.main = [];
            S.detail = d.scheduleDetail || [];
            drawMain(); detGrid.draw(S.detail);
            bindLoadDates(d.loadDates || []); bindItems(d.items || []);
            detailFormReset();
            $id('csContractInfo').textContent = str(d.contractNo) + ' - contract M.Ton ' + fmt(S.contractWeight) + ', FCL ' + fmt(S.contractContainers, 2);
            $id('csFooterInfo').textContent = 'Contract ' + str(d.contractNo);
            tabs1.select('csForm'); tabsMain.select('tabPageMain');
        }).catch(function (e) { box(e.message); });
    }
    function bindLoadDates(rows) {
        S.loadDates = rows;
        bind('cmbAttentiveLoadDateBySchedule', rows, 'ScheduleId', 'AttentiveLoadingDate');
    }
    function bindItems(rows) {
        S.items = rows;
        var byCode = checked('rdbtnItemCode');
        bind('cmbItem', rows.map(function (r) { return { Id: r.ContractDetailId, name: byCode ? r.ItemCode : r.ItemName }; }), 'Id', 'name');
    }
    function pendingTotals(contractId) {
        var r = X.rowOf(PENDING, 'Id', contractId);
        return { weight: r ? netD(r.WeightM_Ton) : 0, containers: r ? netD(r.NoOfContainers) : 0, contractNo: r ? str(r.ContractNo) : '' };
    }

    // ------------------------------------------------------------------------------ schedule main grid (editable)

    function drawMain() {
        var tb = $id('csMainBody'), tf = $id('csMainFoot'); if (!tb) return;
        var h = '';
        S.main.forEach(function (r, i) {
            var ro = str(r.ReferredStatus) === 'Referred';
            h += '<tr data-i="' + i + '">';
            h += '<td class="win-cell-btn"><button type="button" class="win-x" data-act="Delete" data-i="' + i + '" title="Delete">X</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-act="Add" data-i="' + i + '" title="Add">+</button></td>';
            h += '<td><input class="win-cell-edit" data-f="ScheduleNo" data-i="' + i + '" value="' + esc(r.ScheduleNo) + '"' + (ro ? ' readonly' : '') + '/></td>';
            h += '<td>' + esc(r.CustomerName) + '</td>';
            h += '<td><input class="win-cell-edit" data-f="CustomerContractNo" data-i="' + i + '" value="' + esc(r.CustomerContractNo) + '"/></td>';
            h += '<td><input class="win-cell-edit num" data-f="NoOfContainers" data-i="' + i + '" value="' + esc(fmt(r.NoOfContainers, 2)) + '"/></td>';
            h += '<td><input class="win-cell-edit num" data-f="WeightM_Ton" data-i="' + i + '" value="' + esc(fmt(r.WeightM_Ton, 3)) + '"/></td>';
            h += '<td><input type="date" class="win-cell-edit" data-f="AttentiveLoadingDate" data-i="' + i + '" value="' + esc(X.isoDate(r.AttentiveLoadingDate)) + '"/></td>';
            h += '<td><select class="win-cell-edit" data-f="DestinationPortId" data-i="' + i + '"><option value="0"></option>';
            L.ports.forEach(function (p) { h += '<option value="' + p.Id + '"' + (netI(p.Id) === netI(r.DestinationPortId) ? ' selected' : '') + '>' + esc(p.name) + '</option>'; });
            h += '</select></td>';
            h += '<td><input type="date" class="win-cell-edit" data-f="AttentiveProductionDate" data-i="' + i + '" value="' + esc(X.isoDate(r.AttentiveProductionDate)) + '"/></td>';
            h += '<td><input type="date" class="win-cell-edit" data-f="AttentiveInspectionDate" data-i="' + i + '" value="' + esc(X.isoDate(r.AttentiveInspectionDate)) + '"/></td>';
            h += '<td><input type="date" class="win-cell-edit" data-f="PackingMaterialDate" data-i="' + i + '" value="' + esc(X.isoDate(r.PackingMaterialDate)) + '"/></td>';
            h += '<td' + (FEAT.sdf ? '' : ' class="is-hidden"') + '><select class="win-cell-edit" data-f="CustomGroupId" data-i="' + i + '"><option value="0"></option>';
            L.customGroups.forEach(function (c) { h += '<option value="' + c.Id + '"' + (netI(c.Id) === netI(r.CustomGroupId) ? ' selected' : '') + '>' + esc(c.name) + '</option>'; });
            h += '</select></td>';
            h += '<td><input class="win-cell-edit" data-f="Remarks" data-i="' + i + '" value="' + esc(r.Remarks) + '"/></td>';
            h += '<td>' + esc(r.Status) + '</td><td>' + esc(r.ReferredStatus) + '</td><td class="num">' + esc(str(netI(r.NoOfAttachments))) + '</td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-act="AddAttachment" data-i="' + i + '">Add Attachment</button></td>';
            h += '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-act="Save" data-i="' + i + '">Document Custom Group Save</button></td>';
            h += '</tr>';
        });
        tb.innerHTML = h;
        if (tf) {
            if (S.main.length) {
                var c = 0, w = 0; S.main.forEach(function (r) { c += netD(r.NoOfContainers); w += netD(r.WeightM_Ton); });
                tf.innerHTML = '<tr class="win-total"><td></td><td></td><td></td><td></td><td></td><td class="num">&Sigma; ' + esc(fmt(c, 2)) + '</td><td class="num">&Sigma; ' + esc(fmt(w, 3)) + '</td><td colspan="12"></td></tr>';
            } else tf.innerHTML = '';
        }
    }
    /* grdContractShedule_CellUpdated: FCL and WeightM_Ton capped by the contract's totals less the other rows. */
    function mainCellUpdated(i, f, el) {
        var item = S.main[i]; if (!item) return;
        if (f === 'NoOfContainers' || f === 'WeightM_Ton') item[f] = netD(el.value);
        else if (f === 'DestinationPortId' || f === 'CustomGroupId') item[f] = netI(el.value);
        else item[f] = el.value;
        if (f === 'WeightM_Ton' || f === 'NoOfContainers') {
            var tot = pendingTotals(netI(item.ContractId));
            var otherW = 0, otherC = 0;
            S.main.forEach(function (r, j) { if (j !== i) { otherW += netD(r.WeightM_Ton); otherC += netD(r.NoOfContainers); } });
            var regW = tot.weight - otherW, regC = tot.containers - otherC;
            if (netD(item.WeightM_Ton) > 0 && netD(item.NoOfContainers) > 0) {
                var c = netD(item.NoOfContainers), w = netD(item.WeightM_Ton);
                if (c > regC && !CFG.skipFCLContractValidationOnSchedule) { box('FCL can not be Greater than total FCL of this Contract..'); item.NoOfContainers = Math.round(regC * 100) / 100; }
                else item.NoOfContainers = Math.round(c * 100) / 100;
                if (w > regW) { box('WeightM_Ton can not be Greater than  total WeightM_Ton of This Contract..'); item.WeightM_Ton = Math.round(regW * 1000) / 1000; }
                else item.WeightM_Ton = Math.round(w * 1000) / 1000;
            }
            drawMain();
        }
    }
    function mainAction(i, act) {
        var r = S.main[i]; if (!r) return;
        try {
            if (act === 'Delete') {
                if (S.main.length <= 1) { box('You Can Not Delete All rows'); return; }
                if (netI(r.Id) > 0) {
                    if (str(r.Status) !== 'Pending') throw new Error("You can't delete this row because its detail is " + str(r.Status));
                    if (!ask('Are you sure to Delete?')) return;
                    S.removedMain.push(r);
                }
                S.main.splice(i, 1); drawMain();
            } else if (act === 'Add') {
                if (CFG.exportSalesContractApprovalMandatory && !X.netB(r.IsApproved)) throw new Error("Can't break this record because contract is not approved!");
                addRowInMain(i);
            } else if (act === 'AddAttachment') {
                if (netI(r.Id) <= 0) { box('This record is not saved so can\'t add attachments'); return; }
                box('Attachments (DMS) are not available in the web port.');
            } else if (act === 'Save') {
                if (netI(r.Id) <= 0) { box('This record is not saved so can\'t save custom documents'); return; }
                if (netI(r.CustomGroupId) <= 0) { box('Custom Group field is required'); return; }
                X.postJson(API + '/custom-group-save', { id: netI(r.Id), customGroupId: netI(r.CustomGroupId) }).then(function (res) { box(res.message || 'Documents save successfully'); }, function (e) { box(e.message); });
            }
        } catch (e) { box(e.message); }
    }
    /* AddRowInScheduleMainGrid: the balance of the contract's WeightM_Ton / FCL (from the pending grid) as a new row "<ContractNo>/<n>". */
    function addRowInMain(i) {
        var t = X.today();
        if (S.main.length === 0) {
            S.main.push({ Id: 0, ContractId: 0, ScheduleNo: '', SupplierCustomerId: 0, CustomerName: '', CustomerContractNo: '', NoOfContainers: 0, WeightM_Ton: 0, AttentiveLoadingDate: t, DestinationPortId: 0,
                AttentiveProductionDate: t, AttentiveInspectionDate: t, PackingMaterialDate: t, CustomGroupId: 0, Remarks: '', Status: '', ReferredStatus: '', NoOfAttachments: 0, IsApproved: false });
            drawMain(); return;
        }
        var cur = S.main[i], tot = pendingTotals(netI(cur.ContractId));
        var w = 0, c = 0; S.main.forEach(function (r) { w += netD(r.WeightM_Ton); c += netD(r.NoOfContainers); });
        var grossW = tot.weight - w, grossC = tot.containers - c;
        if ((grossW > 0 || grossC > 0) && grossW > 0) {
            S.main.push({ Id: 0, ContractId: cur.ContractId, ScheduleNo: tot.contractNo + '/' + (S.main.length + 1), SupplierCustomerId: cur.SupplierCustomerId, CustomerName: cur.CustomerName, CustomerContractNo: '',
                NoOfContainers: grossC, WeightM_Ton: grossW, AttentiveLoadingDate: cur.AttentiveLoadingDate, DestinationPortId: cur.DestinationPortId, AttentiveProductionDate: cur.AttentiveProductionDate,
                AttentiveInspectionDate: cur.AttentiveInspectionDate, PackingMaterialDate: cur.PackingMaterialDate, CustomGroupId: cur.CustomGroupId, Remarks: cur.Remarks, Status: '', ReferredStatus: '', NoOfAttachments: 0, IsApproved: false });
            drawMain();
        } else box('Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ');
    }
    function btnNew() { formReset(); S.main = []; drawMain(); S.detail = []; detGrid.draw([]); }
    function refreshMain(btn) {
        return X.busy(btn, function () { return X.getJson(API + '/refresh-main').then(function (d) { if (d.config) CFG = d.config; L.ports = d.ports || L.ports; drawMain(); }); });
    }
    /* InsertScheduleMain -> the service's row checks and ExImLcOrderShipmentSchedule.Save; then the contract is re-read. */
    function saveMain(btn) {
        return X.busy(btn, function () {
            if (S.main.length === 0) { box('Schedule Grid have no row'); return; }
            var body = { rows: S.main, removed: S.removedMain };
            var tot = X.rowOf(PENDING, 'Id', netI(S.main[0].ContractId));
            if (tot) body.contractWeightMTon = netD(tot.WeightM_Ton);
            return X.postJson(API + '/save-main', body).then(function (r) {
                box(r.message || 'Main Schedule Save SuccessFully ');
                var cid = netI(S.main[0].ContractId) || S.contractId;
                formReset(); S.main = []; drawMain();
                return loadContract(cid, X.rowOf(PENDING, 'Id', cid)).then(function () { return loadPending(50, null); });
            });
        });
    }

    // ------------------------------------------------------------------------------ schedule detail entry

    function selectedLoadDate() { return X.rowOf(S.loadDates, 'ScheduleId', val('cmbAttentiveLoadDateBySchedule')); }
    function selectedItem() { return X.rowOf(S.items, 'ContractDetailId', val('cmbItem')); }
    function uomRow(id) { return X.rowOf(S.uoms, 'Id', id); }
    /* cmbAttentiveLoadDateBySchedule_ValueChanged: GetTotalMTonOfDate + GetMtonsFromDetail. */
    function loadDateChanged() {
        var d = selectedLoadDate();
        if (!d || valI('cmbAttentiveLoadDateBySchedule') <= 0) { setText('txtMtonOfSelectedDate', '0'); S.mtonByDate = 0; return; }
        X.getJson(API + '/schedule-mton?contractId=' + netI(d.ContractId) + '&scheduleId=' + netI(d.ScheduleId)).then(function (r) {
            S.mtonByDate = netD(r.mton); setText('txtMtonOfSelectedDate', str(S.mtonByDate));
            mtonsFromDetail();
        }).catch(function (e) { box(e.message); });
    }
    function mtonsFromDetail() {
        var mtonOfDate = valD('txtMtonOfSelectedDate');
        if (S.detail.length === 0 || !(mtonOfDate > 0)) return;
        var d = selectedLoadDate(); if (!d) return;
        var target = str(d.AttentiveLoadingDate) + ' ' + str(d.DestinationPort), m = 0;
        S.detail.forEach(function (r, i) { if (str(r.DestinationPort) === target && (S.updateIndex === -1 || i !== S.updateIndex)) m += netD(r.WeightM_Ton); });
        if (m > 0) setText('txtMtonOfSelectedDate', fmt(mtonOfDate - m));
    }
    /* cmbItem_Leave: bindItemPackUom + BindOtherDetailAuto. */
    function itemLeave() {
        var it = selectedItem(), keepText = text('cmbPackUom');
        if (!it || valI('cmbItem') <= 0) { S.uoms = []; bind('cmbPackUom', [], 'Id', 'UOMCode'); return Promise.resolve(); }
        return X.getJson(API + '/uom?itemId=' + netI(it.Id)).then(function (rows) {
            S.uoms = rows || [];
            bind('cmbPackUom', S.uoms, 'Id', 'UOMCode');
            var m = S.uoms.filter(function (u) { return str(u.UOMCode) === keepText; })[0];
            setVal('cmbPackUom', m ? m.Id : 0);
            bindOtherDetailAuto();
        }).catch(function (e) { box(e.message); });
    }
    function bindOtherDetailAuto() {
        var it = selectedItem(); if (!it || S.uoms.length === 0) return;
        setVal('cmbCropYear', it.CropYearId); setVal('cmbPackingtype', it.PackTypeId); setVal('cmbPackUom', it.PackUomId);
        var u = uomRow(val('cmbPackUom')), eq = u ? netD(u.Equivalent) : 0;
        var mtons = valD('txtMtonOfSelectedDate'), sameSchedule = 0, detailWise = 0, cdId = netI(it.ContractDetailId), schedId = valI('cmbAttentiveLoadDateBySchedule');
        S.detail.forEach(function (r) { if (netI(r.ScheduleId) === schedId) sameSchedule += netD(r.WeightM_Ton); if (netI(r.ContractDetailId) === cdId) detailWise += netD(r.WeightM_Ton); });
        mtons -= sameSchedule;
        setText('txtNoOfBagsDetail', fmt(mtons * 1000 / eq, 3));
        setText('txtBalMton', fmt(netD(it.M_Ton) - detailWise, 3));
        calcMtonDetail();
    }
    /* CalculateMtonDetail: NoOfBags <-> Weight via Equivalent; InnerQty = bags * QtyEquivalent. */
    function calcMtonDetail() {
        var u = uomRow(val('cmbPackUom')); if (!u || valI('cmbPackUom') === 0) return;
        var bags = valD('txtNoOfBagsDetail'), eq = netD(u.Equivalent), qeq = netD(u.QtyEquivalent), w = valD('txtMTonDetail'), tag = X.activeTag();
        if (tag === 'NoOfBags') setText('txtMTonDetail', fmt(bags * eq / 1000, 3));
        else if (tag === 'WeightMton') setText('txtNoOfBagsDetail', fmt(eq > 0 ? w * 1000 / eq : 0, 0));
        else setText('txtMTonDetail', fmt(bags * eq / 1000, 3));
        setText('txtInnerQty', fmt(valD('txtNoOfBagsDetail') * qeq, 3));
    }
    function detailValidation() {
        if (valI('cmbAttentiveLoadDateBySchedule') === 0) { box('Attentive Loading Date is Required!'); focus('cmbAttentiveLoadDateBySchedule'); return false; }
        if (valI('cmbItem') === 0) { box('Item is Required!'); focus('cmbItem'); return false; }
        var it = selectedItem();
        if (CFG.exportSalesContractApprovalMandatory && it && !X.netB(it.IsApproved)) { box('Contract is not approved.Please Check!'); focus('cmbAttentiveLoadDateBySchedule'); return false; }
        if (valI('cmbCropYear') === 0) { box('CropYear is Required!'); focus('cmbCropYear'); return false; }
        if (valI('cmbPackingtype') === 0) { box('Packing Type is Required!'); focus('cmbPackingtype'); return false; }
        if (valD('txtNoOfBagsDetail') === 0) { box('No Of Bags Feild is Required!'); focus('txtNoOfBagsDetail'); return false; }
        if (valI('cmbPackUom') === 0) { box('PackUom is Required!'); focus('cmbPackUom'); return false; }
        if (valD('txtMTonDetail') === 0) { box('Weight/M.Ton Feild is Required!'); focus('txtMTonDetail'); return false; }
        return true;
    }
    function detailRow(id) {
        var d = selectedLoadDate(), it = selectedItem();
        return { Id: id || 0, ScheduleId: netI(d.ScheduleId), ContractId: netI(d.ContractId), ContractDetailId: netI(it.ContractDetailId), AttentiveLoadingDate: str(d.LoadingDateIso),
            DestinationPort: str(d.AttentiveLoadingDate) + ' ' + str(d.DestinationPort) + ' ' + str(d.ScheduleNo), ItemId: netI(it.Id), Item: str(it.ItemName), ItemCode: str(it.ItemCode),
            CropYearId: valI('cmbCropYear'), CropYear: text('cmbCropYear'), PackingTypeId: valI('cmbPackingtype'), PackingType: text('cmbPackingtype'), NoOfBags: valD('txtNoOfBagsDetail'),
            PackUomId: valI('cmbPackUom'), PackUom: text('cmbPackUom'), InnerQty: valD('txtInnerQty'), WeightM_Ton: valD('txtMTonDetail'), Status: '', ReferredStatus: '' };
    }
    /* btnAdd_Click: the schedule / contract detail M.Ton checks, then the row. */
    function addDetail() {
        if (!detailValidation()) return;
        var d = selectedLoadDate(), it = selectedItem();
        var cond = str(d.AttentiveLoadingDate) + ' ' + str(d.DestinationPort) + ' ' + str(d.ScheduleNo);
        var cdId = netI(it.ContractDetailId), cdMton = netD(it.M_Ton), schedMton = S.mtonByDate, w = valD('txtMTonDetail');
        if (S.detail.length === 0) {
            if (w > schedMton) { box('M.Ton Can not be Greater than Total M.Ton of this Contract against Schedule ' + cond + '. Please Check M.Ton!'); return; }
            if (w > cdMton) { box("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract Detail MTon's" + cdMton + '. Please Check M.Ton!'); return; }
        } else {
            var gridW = 0, cdW = 0;
            S.detail.forEach(function (r) { if (str(r.DestinationPort) === cond) gridW += netD(r.WeightM_Ton); if (netI(r.ContractDetailId) === cdId) cdW += netD(r.WeightM_Ton); });
            if (w + gridW > schedMton) { box('M.Ton Can not be Greater than Total M.Ton of this Contract against Date and Port ' + cond + '.Please Check M.Ton!'); return; }
            if (w + cdW > cdMton) { box("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract detail MTon's " + cdMton + '.Please Check M.Ton!'); return; }
        }
        var contractId = netI(d.ContractId);
        S.detail.push(detailRow(0));
        detGrid.draw(S.detail); detailFormReset(); focus('cmbAttentiveLoadDateBySchedule');
        reloadLoadDates(contractId, null);
    }
    function reloadLoadDates(contractId, detailRecIds) {
        return X.getJson(API + '/load-dates?contractId=' + contractId + (detailRecIds ? '&detailRecIds=' + encodeURIComponent(detailRecIds) : '')).then(bindLoadDates).catch(function (e) { box(e.message); });
    }
    function editDetail(i) {
        var r = S.detail[i]; if (!r || str(r.ReferredStatus) === 'Referred') return;
        S.updateIndex = i;
        reloadLoadDates(netI(r.ContractId), str(r.ScheduleId)).then(function () {
            setVal('cmbAttentiveLoadDateBySchedule', r.ScheduleId); loadDateChanged();
            setVal('cmbItem', r.ContractDetailId);
            return itemLeave();
        }).then(function () {
            setVal('cmbCropYear', r.CropYearId); setVal('cmbPackingtype', r.PackingTypeId); setText('txtNoOfBagsDetail', str(r.NoOfBags)); setVal('cmbPackUom', r.PackUomId);
            setText('txtInnerQty', str(r.InnerQty)); setText('txtMTonDetail', str(r.WeightM_Ton));
            show('btnGridUpdate', true); show('btnCancel', true); show('btnAdd', false);
            focus('cmbAttentiveLoadDateBySchedule');
        });
    }
    function updateDetail() {
        if (!detailValidation() || S.updateIndex < 0) return;
        var d = selectedLoadDate(), it = selectedItem();
        var cond = str(d.AttentiveLoadingDate) + ' ' + str(d.DestinationPort) + ' ' + str(d.ScheduleNo);
        var cdId = netI(it.ContractDetailId), cdMton = netD(it.M_Ton), w = valD('txtMTonDetail'), gridW = 0, cdW = 0;
        S.detail.forEach(function (r, j) { if (j === S.updateIndex) return; if (str(r.DestinationPort) === cond) gridW += netD(r.WeightM_Ton); if (netI(r.ContractDetailId) === cdId) cdW += netD(r.WeightM_Ton); });
        if (w + gridW > S.mtonByDate) { box('M.Ton Can not be Greater than Total M.Ton of this Contract against Date and Port ' + cond + '.Please Check M.Ton!'); return; }
        if (w + cdW > cdMton) { box("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract detail MTon's " + cdMton + '.Please Check M.Ton!'); return; }
        var old = S.detail[S.updateIndex];
        var row = detailRow(netI(old.Id)); row.Status = old.Status; row.ReferredStatus = old.ReferredStatus;
        S.detail[S.updateIndex] = row;
        detGrid.draw(S.detail); detailFormReset(); reloadLoadDates(S.contractId, null);
    }
    function deleteDetail(i) {
        try {
            var r = S.detail[i]; if (!r) return;
            if (S.updateIndex !== -1) throw new Error("You can't delete this detail Record because its in update mode");
            if (netI(r.Id) > 0) {
                if (str(r.ReferredStatus) === 'Referred') throw new Error("You can't delete this row because it is referred on invoice");
                if (!ask('Are you sure to Delete?')) return;
                S.removedDetail.push(r);
            }
            S.detail.splice(i, 1); detGrid.draw(S.detail);
            reloadLoadDates(S.contractId, null);
        } catch (e) { box(e.message); }
    }
    function detailFormReset() {
        S.updateIndex = -1;
        setVal('cmbAttentiveLoadDateBySchedule', 0); setVal('cmbItem', 0); setVal('cmbCropYear', 0); setVal('cmbPackingtype', 0); bind('cmbPackUom', [], 'Id', 'UOMCode'); S.uoms = [];
        setText('txtNoOfBagsDetail', '0'); setText('txtInnerQty', '0'); setText('txtMTonDetail', '0'); setText('txtMtonOfSelectedDate', '0'); setText('txtBalMton', '0'); S.mtonByDate = 0;
        show('btnAdd', true); show('btnGridUpdate', false); show('btnCancel', false);
    }
    function newDetail() { detailFormReset(); S.detail = []; S.removedDetail = []; detGrid.draw([]); }
    function refreshDetail(btn) {
        return X.busy(btn, function () {
            if (S.contractId <= 0) return;
            return Promise.all([reloadLoadDates(S.contractId, null), X.getJson(API + '/items?contractId=' + S.contractId).then(bindItems)]);
        });
    }
    function saveDetail(btn) {
        return X.busy(btn, function () {
            if (S.detail.length === 0) { box('Schedule Grid have no row'); return; }
            return X.postJson(API + '/save-detail', { rows: S.detail, removed: S.removedDetail }).then(function (r) {
                box(r.message || 'Detail Schedule Save SuccessFully');
                var cid = S.contractId || netI(S.detail[0].ContractId);
                detailFormReset(); S.removedDetail = [];
                return loadContract(cid, X.rowOf(PENDING, 'Id', cid));
            });
        });
    }

    // ------------------------------------------------------------------------------ prints / popups

    function slip395(btn) {
        if (PENDING.length === 0) { box('(Pending Grid) has no Record'); return; }
        var r = PENDING[pendGrid.current];
        if (!r) { box('First Click On any In  (Pending Grid)'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-395', { id: netI(r.Id) }, btn);
    }
    function popup(key) {
        if (key === 'pmDetail') { global.open('/export/packing-detail-by-contract' + (S.contractId > 0 ? '?id=' + S.contractId : ''), '_blank'); return; }
        if (key === 'shipmentDocs') { box('The Shipment Document Schedule form (frmShipmentDocumentSchedule) is not ported.'); return; }
        box('Not available.');
    }
    function shortcuts() {
        X.shortcuts([['Ctrl+S', 'For Save of Focused Grid'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New of Focused Grid'], ['Ctrl+L', 'For Load All'], ['Ctrl+P', 'For Print'],
            ['Alt+1', 'For Print 551 in History'], ['Alt+2', 'For Print 551_01 in History'], ['Ctrl+F5', "For Focus on 'Sale Contract for Schedule' Grid"], ['Ctrl+F10', 'For Open Attachments'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Right', 'For Move focus From One Grid to Next'], ['Ctrl+ArrowDown', 'For Focus On Schedule Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Date Combo in Detail Box'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]]);
    }

    // ------------------------------------------------------------------------------ history

    function historyBody() { return { fromDate: val('txtdatefrom'), toDate: val('txtdateto'), customerId: valI('cmbSupplierName'), portId: valI('cmbPort'), itemId: valI('CmbItemHistory') }; }
    function historyShow(btn) {
        return X.busy(btn, function () {
            return X.postJson(API + '/history', historyBody()).then(function (d) { HIST = (d && d.rows) || []; histGrid.current = -1; histGrid.draw(HIST); if (HIST.length === 0) box('No record found'); });
        });
    }
    function historyNew() { var t = X.today(); setText('txtdatefrom', t); setText('txtdateto', t); setVal('cmbPort', 0); setVal('cmbSupplierName', 0); setVal('CmbItemHistory', 0); HIST = []; histGrid.draw([]); }
    function historyRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-combos').then(historyCombos); }); }
    function historyEdit(i) { var r = HIST[i]; if (!r) return; loadContract(netI(r.ExImLcOrderId), null); }
    /* BtnHistoryPrint_Click: the ticked rows (else all) of the shown history, in RecordNo order, on 551-ContractSchedule_FormHistory.rpt. */
    function print551(btn) {
        var idx = histGrid.checkedIdx(), rows = idx.length ? idx.map(function (i) { return HIST[i]; }) : HIST.slice();
        if (rows.length === 0) { box('No record selected for printing.'); return; }
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        rows.sort(function (a, b) { return netI(a.RecordNo) - netI(b.RecordNo); });
        var p = historyBody(); p.ids = rows.map(function (r) { return netI(r.Id); }).join(',');
        return global.CrystalPrint.open('exp-551', p, btn);
    }
    function print55101(btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        return global.CrystalPrint.open('exp-551-01', { fromDate: val('txtdatefrom'), toDate: val('txtdateto'), destinationPortId: valI('cmbPort'), supplierCustomerId: valI('cmbSupplierName'), itemId: valI('CmbItemHistory') }, btn);
    }
    function toggleHistory() { tabsMain.select('tabPageMain'); if (tabs1.current() === 'csHistory') tabs1.select('csForm'); else tabs1.select('csHistory'); }

    // ------------------------------------------------------------------------------ Pre Shipment Planning

    function deptShow(btn) {
        return X.busy(btn, function () {
            return X.postJson(API + '/department', { fromChecked: checked('chkDateFromDepart'), fromDate: val('txtDateFromDepart'), toChecked: checked('chkDatetoDepart'), toDate: val('txtDatetoDepart') })
                .then(function (rows) { DEPT = rows || []; deptGrid.current = -1; deptGrid.draw(DEPT); });
        });
    }
    function deptNew(btn) { deptFormReset(); return deptShow(btn); }
    function deptRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/plants').then(function (rows) { L.plants = rows || []; bind('CmbPlantDepartmentDetail', L.plants, 'Id', 'name'); }); }); }
    function deptFormReset() {
        S.deptRecId = 0; S.deptRow = null;
        show('BtnUpdateDepartmentDetail', false); show('BtnCancelDepartmentDetail', false);
        ['CmbContractDepartmentDetail', 'CmbScheduleNoDepartmentDetail', 'CmbCustomerDepartmentDetail', 'CmbPortDepartmentDetail', 'CmbItemDepartmentDetail'].forEach(function (id) { bind(id, [], 'Id', 'name'); });
        ['txtInspectionDays', 'txtProductionDays', 'txtPackingMaterialScheduleDays', 'txtFclDepartmentDetail', 'txtScheduleMtonDepartmentDetail', 'txtItemMtonDepartmentDetail'].forEach(function (id) { setText(id, ''); });
        setVal('CmbPlantDepartmentDetail', 0); setVal('CmbScheduleLockStatusDepartmentDetail', 1);
        var g = $id('grdDepartmentDetail'); if (g) g.focus();
    }
    /* DepartmentDetailReadById */
    function deptReadById(i) {
        var r = DEPT[i]; if (!r) return;
        try {
            S.deptRecId = netI(r.Id); S.deptRow = r;
            if (netI(r.ScheduleLockId) === 1) throw new Error("This Record has been Confirmed,So it can't be Updated...");
            bind('CmbContractDepartmentDetail', netI(r.ExImLcOrderId) > 0 ? [{ Id: r.ExImLcOrderId, name: r.ContractNo }] : [], 'Id', 'name', true);
            bind('CmbScheduleNoDepartmentDetail', netI(r.ContractScheduleId) > 0 ? [{ Id: r.ContractScheduleId, name: r.ScheduleCode }] : [], 'Id', 'name', true);
            bind('CmbCustomerDepartmentDetail', netI(r.SupplierCustomerId) > 0 ? [{ Id: r.SupplierCustomerId, name: r.CustomerName }] : [], 'Id', 'name', true);
            bind('CmbPortDepartmentDetail', netI(r.DestinationPortId) > 0 ? [{ Id: r.DestinationPortId, name: r.DestinationPort }] : [], 'Id', 'name', true);
            setText('txtFclDepartmentDetail', str(r.FCL)); setText('txtScheduleMtonDepartmentDetail', fmt(r.ScheduleMTon, 4)); setText('txtLoadingDateDepartmentDetail', X.isoDate(r.LoadingDate));
            bind('CmbItemDepartmentDetail', netI(r.ContractScheduleDetailItemId) > 0 ? [{ Id: r.ContractScheduleDetailItemId, name: r.ItemName }] : [], 'Id', 'name', true);
            setText('txtItemMtonDepartmentDetail', fmt(r.ItemMTon, 4));
            setText('txtProductionDateDepartmentDetail', X.isoDate(r.ProductionSchedule)); setText('txtInspectionDateDepartmentDetail', X.isoDate(r.InspectionSchedule)); setText('txtPMDateDepartmentDetail', X.isoDate(r.PackMaterialSchedule));
            setVal('CmbPlantDepartmentDetail', r.PlantId); setVal('CmbScheduleLockStatusDepartmentDetail', netI(r.ScheduleLockId) || 2);
            dateToDays('txtProductionDateDepartmentDetail', 'txtProductionDays'); dateToDays('txtPMDateDepartmentDetail', 'txtPackingMaterialScheduleDays'); dateToDays('txtInspectionDateDepartmentDetail', 'txtInspectionDays');
            focus('txtProductionDays');
            show('BtnUpdateDepartmentDetail', true); show('BtnCancelDepartmentDetail', true);
        } catch (e) { box(e.message); }
    }
    function daysToDate(daysId, dateId) { setText(dateId, X.addDays(val('txtLoadingDateDepartmentDetail'), -valI(daysId))); }
    function dateToDays(dateId, daysId) { var d = val(dateId); if (!X.dateNull(d)) setText(daysId, str(X.daysBetween(val('txtLoadingDateDepartmentDetail'), d))); }
    /* BtnUpdateDepartmentDetail_Click -> InsertDepartmentDetail (the service holds DepartmentDetailFormvalidation). */
    function deptUpdate(btn) {
        return X.busy(btn, function () {
            if (!ask(S.deptRecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            if (valI('CmbScheduleLockStatusDepartmentDetail') === 1 && !ask('You are about to update the schedule with lock status (Confirmed). Are you sure you want to save?\nRecords with a confirmed status will not be updated afterward.')) return;
            var body = { recId: S.deptRecId, contractId: valI('CmbContractDepartmentDetail'), scheduleId: valI('CmbScheduleNoDepartmentDetail'), customerId: valI('CmbCustomerDepartmentDetail'), portId: valI('CmbPortDepartmentDetail'),
                scheduleMton: valD('txtScheduleMtonDepartmentDetail'), itemId: valI('CmbItemDepartmentDetail'), itemMton: valD('txtItemMtonDepartmentDetail'), productionDays: valI('txtProductionDays'), plantId: valI('CmbPlantDepartmentDetail'),
                loadingDate: val('txtLoadingDateDepartmentDetail'), productionDate: val('txtProductionDateDepartmentDetail'), inspectionChecked: checked('chkInspectionDateDepartmentDetail'), inspectionDate: val('txtInspectionDateDepartmentDetail'),
                pmChecked: checked('chkPMDateDepartmentDetail'), pmDate: val('txtPMDateDepartmentDetail'), scheduleLockId: valI('CmbScheduleLockStatusDepartmentDetail') };
            return X.postJson(API + '/save-department', body).then(function (r) { box(r.message || 'Detail Save SuccessFully'); deptFormReset(); return deptShow(null); });
        });
    }
    function deptCancel() { deptFormReset(); }
    /* grdDepartmentDetail_LinkClicked */
    function deptLink(i, key, a) {
        var r = DEPT[i]; if (!r) return;
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return; }
        if (key === 'ContractNo') { if (netI(r.ExImLcOrderId) > 0) global.CrystalPrint.open('exp-501new', { id: netI(r.ExImLcOrderId) }, a); }
        else if (key === 'ScheduleCode') { if (netI(r.ExImLcOrderId) > 0 && netI(r.ContractScheduleId) > 0) global.CrystalPrint.open('exp-395', { id: netI(r.ExImLcOrderId), contractScheduleId: netI(r.ContractScheduleId) }, a); }
        else if (key === 'PackMaterialStart') { if (netI(r.PmStartRefId) > 0) box('The packing material source slip (document type ' + netI(r.PmStartRefDocumentTypeId) + ', record ' + netI(r.PmStartRefId) + ') belongs to another module and is not ported here.'); }
        else if (key === 'InspectionStart') { if (netI(r.PreShipmentInspectionId) > 0) box('The Pre Production Export Lot Inspection slip (record ' + netI(r.PreShipmentInspectionId) + ') belongs to the Quality Control module and is not ported here.'); }
        else if (key === 'ProductionStart') { if (netI(r.JobOrderId) > 0) box('The Production Recovery report 602 (job order ' + netI(r.JobOrderId) + ') belongs to the Production module and is not ported here.'); }
        else if (key === 'ItemCode' || key === 'ItemName') box('The Stock Report (frmStockReportWithValues) for item ' + str(r.ItemName) + ' is opened from the Inventory module.');
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function wire() {
        tabsMain = X.tabs('csMain', ['tabPageMain', 'tabPageDepartmentStatus'], function (p) { if (p === 'tabPageDepartmentStatus') { if (DEPT.length === 0) deptShow(null); } else { var g = $id('grdPendingContract'); if (g) g.focus(); } });
        tabs1 = X.tabs('cs1', ['csForm', 'csHistory']);
        X.initFullscreen();
        var mb = $id('grdContractShedule');
        if (mb) {
            mb.addEventListener('click', function (e) { var b = e.target.closest('button[data-act]'); if (b) { e.preventDefault(); mainAction(netI(b.getAttribute('data-i')), b.getAttribute('data-act')); } });
            mb.addEventListener('change', function (e) { var el = e.target.closest('[data-f]'); if (el) mainCellUpdated(netI(el.getAttribute('data-i')), el.getAttribute('data-f'), el); });
        }
        ['rdAll', 'rdReffered', 'rdNotReffered'].forEach(function (id) { on(id, 'change', function () { loadPending(50, null); }); });
        on('cmbAttentiveLoadDateBySchedule', 'change', loadDateChanged);
        on('cmbItem', 'change', function () { itemLeave(); });
        ['rdbtnItemCode', 'rdbtnItemName'].forEach(function (id) { on(id, 'change', function () { bindItems(S.items); }); });
        on('cmbPackUom', 'change', calcMtonDetail);
        on('txtNoOfBagsDetail', 'input', calcMtonDetail); on('txtMTonDetail', 'input', calcMtonDetail);
        on('btnAdd', 'click', addDetail); on('btnGridUpdate', 'click', updateDetail); on('btnCancel', 'click', function () { detailFormReset(); });
        on('txtProductionDays', 'input', function () { daysToDate('txtProductionDays', 'txtProductionDateDepartmentDetail'); });
        on('txtProductionDateDepartmentDetail', 'change', function () { dateToDays('txtProductionDateDepartmentDetail', 'txtProductionDays'); });
        on('txtInspectionDays', 'input', function () { daysToDate('txtInspectionDays', 'txtInspectionDateDepartmentDetail'); });
        on('txtInspectionDateDepartmentDetail', 'change', function () { dateToDays('txtInspectionDateDepartmentDetail', 'txtInspectionDays'); });
        on('txtPackingMaterialScheduleDays', 'input', function () { daysToDate('txtPackingMaterialScheduleDays', 'txtPMDateDepartmentDetail'); });
        on('txtPMDateDepartmentDetail', 'change', function () { dateToDays('txtPMDateDepartmentDetail', 'txtPackingMaterialScheduleDays'); });
        on('tztAddDays', 'input', function () { setText('txtDatetoDepart', X.addDays(val('txtDateFromDepart'), valI('tztAddDays'))); });
        ['txtNoOfBagsDetail', 'txtMTonDetail'].forEach(function (id) { on(id, 'keypress', X.numericOnly); });
        ['txtProductionDays', 'txtInspectionDays', 'txtPackingMaterialScheduleDays', 'tztAddDays'].forEach(function (id) { on(id, 'keypress', function (e) { if (e.key !== '-') X.integerOnly(e); }); });
        document.addEventListener('keydown', function (e) {
            var mainTab = tabsMain.current() === 'tabPageMain', inForm = mainTab && tabs1.current() === 'csForm';
            var a = document.activeElement, inGrid = function (id) { var g = $id(id); return g && a && g.contains(a); };
            if (e.altKey && !e.ctrlKey && (e.key === '1' || e.key === '2') && mainTab && !inForm) { e.preventDefault(); if (e.key === '1') print551($id('BtnHistoryPrint')); else print55101($id('BtnPrint551_01')); return; }
            if (e.key === 'Enter' && !e.ctrlKey && a && /^(INPUT|SELECT)$/.test(a.tagName) && a.type !== 'button' && !a.classList.contains('win-cell-edit')) {
                var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) { return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null; });
                var i = f.indexOf(a); if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if (!e.ctrlKey) { if (e.key === 'Escape' && !document.querySelector('.ex-fullscreen')) X.cancelWindow(); return; }
            var k = e.key.toLowerCase();
            if (k === 'e') { e.preventDefault(); X.cancelWindow(); }
            else if (k === 'n') { e.preventDefault(); if (!mainTab) deptNew($id('BtnNewDepartmentDetail')); else if (inForm) { if (inGrid('grdPendingContract')) newMain($id('BtnNewMain')); else if (inGrid('grdContractShedule')) btnNew(); else newDetail(); } else historyNew(); }
            else if (k === 't' && mainTab) { e.preventDefault(); tabs1.select(inForm ? 'csHistory' : 'csForm'); }
            else if (k === 'l' && mainTab) { e.preventDefault(); if (inForm) loadAllPending($id('BtnLoadAllPendingContracts')); else historyShow($id('btnShow')); }
            else if (k === 's' && inForm && PERM.Save) { e.preventDefault(); if (inGrid('grd')) saveDetail($id('btnSavedetailSchedule')); else if (inGrid('grdContractShedule')) saveMain($id('btnsave')); }
            else if (k === 's' && !mainTab) { e.preventDefault(); if (!$id('BtnUpdateDepartmentDetail').classList.contains('is-hidden')) deptUpdate($id('BtnUpdateDepartmentDetail')); }
            else if (k === 'p') { e.preventDefault(); if (inForm) slip395($id('BtnContractWiseSlip')); else if (mainTab) print551($id('BtnHistoryPrint')); }
            else if (e.key === 'F5' && inForm) { e.preventDefault(); var g = $id('grdPendingContract'); if (g) g.focus(); }
            else if (e.key === 'F10') { e.preventDefault(); box('Attachments (DMS) are not available in the web port.'); }
            else if (e.key === 'ArrowDown' && inForm) { e.preventDefault(); var g2 = $id('grd'); if (g2) g2.focus(); }
            else if (e.key === 'ArrowUp' && inForm) { e.preventDefault(); focus('cmbAttentiveLoadDateBySchedule'); }
            else if (e.key === 'ArrowRight' && inForm) { e.preventDefault(); var order = ['grdPendingContract', 'grdContractShedule', 'grd'], cur = -1; order.forEach(function (id, j) { if (inGrid(id)) cur = j; }); var nx = $id(order[(cur + 1) % order.length]); if (nx) nx.focus(); }
            else if (e.altKey) { e.preventDefault(); shortcuts(); }
        });
        setup().then(function () {
            var q = /[?&]contractId=(\d+)/.exec(location.search);
            if (q) loadContract(netI(q[1]), null);
        }).catch(function (e) { box(e.message); });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', wire); else wire();

    global.ExportContractSchedule = { newMain: newMain, loadAllPending: loadAllPending, slip395: slip395, shortcuts: shortcuts, btnNew: btnNew, refreshMain: refreshMain, saveMain: saveMain,
        newDetail: newDetail, refreshDetail: refreshDetail, saveDetail: saveDetail, popup: popup, historyShow: historyShow, historyNew: historyNew, historyRefresh: historyRefresh,
        print551: print551, print55101: print55101, toggleHistory: toggleHistory, deptShow: deptShow, deptNew: deptNew, deptRefresh: deptRefresh, deptUpdate: deptUpdate, deptCancel: deptCancel, loadContract: loadContract };
})(window);
