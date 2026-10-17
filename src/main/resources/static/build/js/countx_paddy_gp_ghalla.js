/* Screen 468 "GPI For Ghalla Mandi" - Architecture.WinApp.Paddy_Purchase/frmGatePassInwardForGhallaMandi.cs (DocumentTypeId 51, RefDocumentTypeId 111).
   Line numbers in the comments are to that file. */
(function () {
    'use strict';
    const API = '/api/paddy-purchase/gp-ghalla';
    const $ = PP.$;
    let Id = 0;                      // Id (int field): set by getUpdate, the grid buttons and grdhistory_SelectionChanged
    let dtDetail = [];               // dtDetail rows: ItemId, ItemName, ItemQty, PackingTypeId, PackingType, PackUnit, Weight
    let updateDetailIndex = -1;
    let rights = { canSave: false, canUpdate: false, canPrint: false, weighBridges: null };
    let historySelectGen = 0;

    function show(msg) { alert(msg); }

    /* ---------- InwardGatePass_Load (:719) ---------- */
    async function load() {
        const data = await PP.fetch(API + '/init');
        rights = data;
        $('btnsave').disabled = !data.canSave;           // btnsave.Enabled = DoHaveSaveRight
        $('btnPrint').disabled = !data.canPrint;         // btnPrint.Enabled = DoHavePrintRights
        $('btnupdate').disabled = !data.canUpdate;       // btnupdate.Enabled = DoHaveUpdateRights
        bindLookups(data, true);
        if (data.gpNo > 0) $('txtgpno').value = data.gpNo;
        $('txtgpdate').value = PP.today();
        stampTimes();
        $('ChkBox').checked = true;
        renderDetail();
        await grdfrmfill();
        $('txtgpdate').focus();
    }
    function bindLookups(d, first) {
        PP.bind('CmbGhallaMandi', d.ghallaMandi, 'id', 'name');                       // GhallaMandiBind (BindDDLNew, no row activated)
        PP.bind('CmbLoadingContractor', d.loadingContractors, 'id', 'name');          // LoadingContractorBind, group "6"
        PP.bind('CmbCarriageContractor', d.carriageContractors, 'id', 'name');        // CarriageContractorBind, group "17"
        PP.bind('CmbVariety', d.items, 'id', 'name');                                 // ItemNameFill
        PP.bind('cmbvehicletype', d.vehicleTypes, 'id', 'name', { activateFirst: true });   // vehicleTypefill: Rows[0].Activate()
        if (first) {
            PP.bind('cmbWeighBridge', d.weighBridges, 'id', 'name', { activateFirst: true });     // cmbWeighBridgeFill
            PP.bind('CmbPackingType', d.packingTypes, 'id', 'name', { activateFirst: true });     // PackingTypeBind
        }
    }
    function stampTimes() { const t = PP.localStamp().slice(11, 16); $('txtintime').value = t; $('txtouttime').value = t; }

    /* ---------- grids ---------- */
    function renderDetail() {                                // grdGpDetailSetting (:924)
        PP.renderGrid('grdGpDetail', dtDetail.map((r, i) => Object.assign({ _i: i }, r)), {
            hide: ['_i', 'ItemId', 'PackingTypeId'], numeric: ['ItemQty', 'PackUnit', 'Weight'],
            totals: { ItemQty: 0, Weight: 0 }, dec: { ItemQty: 0, PackUnit: 0, Weight: 0 },
            onDbl: row => grdGpDetail_DoubleClick(row._i)
        });
    }
    /* grdfrmfill (:859) / grdfrmsetting (:895) */
    async function grdfrmfill() {
        const rows = await PP.fetch(API + '/open');
        PP.renderGrid('grd', rows, {
            hide: ['Id', 'RefDocumentTypeId'], dateCols: ['GpDate', 'EntryDate', 'ModifyDate', 'InTime'],
            pre: [{ key: 'Print', label: 'Print', onclick: r => generateReport(r.Id) }, { key: 'Edit', label: 'Edit', onclick: r => { Id = Number(r.Id); getUpdate(Id); } }],
            frozen: 2,
            onDbl: r => { Id = Number(r.Id); getUpdate(Id); }
        });
    }
    /* gridhistory (:769) / grdhistorysetting (:828) */
    async function gridhistory(n) {
        const rows = await PP.fetch(API + '/history?noOfRecords=' + (n || 0));
        PP.renderGrid('grdhistory', rows, {
            hide: ['Id', 'RecordNo'], dateCols: ['GpDate'], link: ['Attachments'], frozen: 2, numeric: ['ItemQty', 'Freight'], dec: { ItemQty: 0, Freight: 0 },
            pre: [{ key: 'Print', label: 'Print', onclick: r => generateReport(r.Id) }, { key: 'Edit', label: 'Edit', onclick: r => { Id = Number(r.Id); getUpdate(Id); } }],
            onRow: r => grdhistory_SelectionChanged(r),
            onDbl: r => { Id = Number(r.Id); getUpdate(Id); },
            onLink: (k, r) => showAttachments(r.Id)
        });
        if (!rows.length) PP.renderGrid('grdhistorydetail', [], {});
        else grdhistory_SelectionChanged(rows[0]);          // the first row is current after the grid loads
    }
    /* grdhistory_SelectionChanged (:1593) -> FillHistoryDetail (:1536) */
    async function grdhistory_SelectionChanged(row) {
        if (!row) return;
        Id = Number(row.Id);
        const gen = ++historySelectGen;
        try {
            const rows = await PP.fetch(API + '/history-detail/' + Id);
            if (gen !== historySelectGen) return;
            PP.renderGrid('grdhistorydetail', rows, { numeric: ['Qty', 'Weight'], totals: { Qty: 2, Weight: 2 }, dec: { Qty: 0, Weight: 0 } });
        } catch (e) { PP.error(e); }
    }

    /* ---------- detail row editing ---------- */
    function weightCompared() {                              // WeightComparedPoWtCalculation (:1397)
        const q = PP.num($('txtqty').value), p = PP.num($('txtPackUnit').value);
        $('txtWeight').value = (q > 0 && p > 0) ? String(q * p) : '0';
    }
    function focusCombo(id) { const w = $(id).parentElement && $(id).parentElement.querySelector('.dtcombo-input'); (w || $(id)).focus(); }
    function formValidationDetail() {                        // FormValidationDetail (:483)
        if (!$('CmbVariety').value) { show('ItemName Field is Required'); focusCombo('CmbVariety'); return false; }
        if (PP.num($('txtPackUnit').value) <= 0) { show('PackUnit Field is Required'); $('txtPackUnit').focus(); return false; }
        if (Math.trunc(PP.num($('txtqty').value)) <= 0) { show('ItemQty Field is Required'); $('txtqty').focus(); return false; }
        if (PP.num($('txtWeight').value) <= 0) { show('Weight Field is Required'); $('txtWeight').focus(); return false; }
        return true;
    }
    function resetDetail() {                                 // ResetDetail (:1442)
        focusCombo('CmbVariety');
        PP.setSel('CmbVariety', ''); $('txtqty').value = ''; PP.setSel('CmbPackingType', ''); $('txtPackUnit').value = ''; $('txtWeight').value = '';
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
    }
    function addClick() {                                    // Add_Click (:1423)
        if (!formValidationDetail()) return;
        dtDetail.push({ ItemId: $('CmbVariety').value, ItemName: PP.selText('CmbVariety'), ItemQty: PP.num($('txtqty').value),
            PackingTypeId: $('CmbPackingType').value, PackingType: PP.selText('CmbPackingType'), PackUnit: PP.num($('txtPackUnit').value), Weight: PP.num($('txtWeight').value) });
        renderDetail(); resetDetail();
    }
    function grdGpDetail_DoubleClick(i) {                    // grdGpDetail_DoubleClick (:1462)
        const r = dtDetail[i]; if (!r) return;
        updateDetailIndex = i;
        PP.setSel('CmbVariety', r.ItemId); $('txtqty').value = r.ItemQty; PP.setSel('CmbPackingType', r.PackingTypeId);
        $('txtPackUnit').value = r.PackUnit; $('txtWeight').value = r.Weight;
        $('Add').style.display = 'none'; $('btnUpdateDetail').style.display = ''; $('btnCancelUpdateDetial').style.display = '';
        focusCombo('CmbVariety');
    }
    function btnUpdateDetailClick() {                        // btnUpdateDetail_Click (:1487)
        if (!formValidationDetail()) return;
        const r = dtDetail[updateDetailIndex]; if (!r) return;
        r.ItemId = $('CmbVariety').value; r.ItemName = PP.selText('CmbVariety'); r.PackingTypeId = $('CmbPackingType').value; r.PackingType = PP.selText('CmbPackingType');
        r.ItemQty = PP.num($('txtqty').value); r.PackUnit = PP.num($('txtPackUnit').value); r.Weight = PP.num($('txtWeight').value);
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
        renderDetail(); resetDetail();
    }
    // btnCancelUpdateDetial has no Click handler in the designer: the button does nothing (same here)

    /* ---------- getUpdate (:512) ---------- */
    async function getUpdate(id) {
        try {
            const data = await PP.fetch(API + '/' + id);
            const h = data.header;
            const get = k => { for (const x in h) if (x.toLowerCase() === k.toLowerCase()) return h[x]; return null; };
            const txt = k => get(k) == null ? '' : String(get(k));
            $('btnupdate').style.display = ''; $('btnsave').style.display = 'none';
            switchView('Form'); switchTab2('Detail');            // tabControl1.SelectedIndex = 0; tabControl2.SelectedIndex = 0
            $('txtbiltyno').value = get('BiltyNo') || '';
            PP.setSel('CmbLoadingContractor', get('LoadingContractorId'));
            PP.setSel('CmbGhallaMandi', get('GhallaMandiId'));
            PP.setSel('CmbCarriageContractor', get('CarriageContractorId'));
            $('txtWorkingReportNo').value = txt('WorkingReportNo');
            $('txtfreight').value = txt('Freight');
            PP.setSelText('cmbWeighBridge', get('DocAttachment'));
            $('txtgpdate').value = get('GpDate') ? PP.localStamp(new Date(String(get('GpDate')).replace(' ', 'T'))).slice(0, 10) : PP.today();
            $('txtgpno').value = txt('GpSrNo');
            $('txtremarks').value = get('OtherRemarks') || '';
            const tm = v => { if (!v) return null; const d = new Date(String(v).replace(' ', 'T')); return isNaN(d) || d.getFullYear() < 1900 ? null : PP.localStamp(d).slice(11, 16); };
            $('txtintime').value = tm(get('InDateTimeStamp')) || $('txtintime').value;
            $('txtouttime').value = tm(get('OutDateTimeStamp')) || PP.localStamp().slice(11, 16);
            $('txtvehicleno').value = get('VehicleNo') || '';
            PP.setSelText('cmbvehicletype', get('VehicleType'));
            $('txtDriverName').value = txt('DriverName');
            $('txtDriverMobile').value = txt('DriverMobileNo');
            dtDetail = data.details.map(r => ({ ItemId: r.ItemId, ItemName: r.ItemName, ItemQty: r.ItemQty, PackingTypeId: r.PackingTypeId, PackingType: r.PackingType, PackUnit: r.PackUnit, Weight: r.Weight }));
            renderDetail();
            Id = Number(id);
        } catch (e) { PP.error(e); reset(); }
    }

    /* ---------- Reset (:949) ---------- */
    async function reset() {
        dtDetail = []; renderDetail(); switchTab2('Detail');
        $('txtvehicleno').value = ''; $('txtbiltyno').value = '';
        PP.bind('cmbWeighBridge', rights.weighBridges || [{ id: 1, name: 'Auto' }, { id: 2, name: 'Mannual' }], 'id', 'name', { activateFirst: true });   // cmbWeighBridge.Rows[0].Activate()
        PP.setSel('CmbVariety', ''); PP.setSel('CmbGhallaMandi', '');
        $('txtqty').value = ''; $('txtfreight').value = ''; $('txtremarks').value = '';
        PP.setSel('CmbLoadingContractor', ''); PP.setSel('CmbCarriageContractor', '');
        ['txtWorkingReportNo', 'txtDriverName', 'txtDriverMobile'].forEach(i => { $(i).value = ''; });
        $('btnsave').style.display = ''; $('btnupdate').style.display = 'none';
        stampTimes(); $('cmbWeighBridge').disabled = false;
        try {
            const g = await PP.fetch(API + '/gp-no'); if (g.gpNo > 0) $('txtgpno').value = g.gpNo;
            await grdfrmfill();
        } catch (e) { PP.error(e); }
        $('txtgpdate').focus();
    }

    /* ---------- save / update (:584, :707, :597) ---------- */
    function clientValidate(b) {                             // formvalidation (:430): same order and text (the server repeats it)
        if (!b.gpNo || b.gpNo === '0') return { msg: 'GatePassNo Field Required', focus: 'txtgpno' };
        if (!b.ghallaMandiId) return { msg: 'Ghalla_Mandi Field Required', focus: 'CmbGhallaMandi' };
        if (!b.loadingContractorId) return { msg: 'Loading Contractor Field Required', focus: 'CmbLoadingContractor' };
        if (!b.carriageContractorId) return { msg: 'Carriage Contractor Field Required', focus: 'CmbCarriageContractor' };
        if (!b.vehicleNo) return { msg: 'VehicleNo Field Required', focus: 'txtvehicleno' };
        if (!b.vehicleType) return { msg: 'VehicleType Field Required', focus: 'cmbvehicletype' };
        if (!b.workingReportNo || b.workingReportNo === '0') return { msg: 'Working Report No Field Required', focus: 'txtWorkingReportNo' };
        if (!b.weighBridge) return { msg: 'WeighBridge Status Field Required', focus: 'cmbWeighBridge' };
        return null;
    }
    async function insert(updating) {
        const body = {
            id: updating ? Id : 0, gpNo: $('txtgpno').value.trim(), gpDate: $('txtgpdate').value, ghallaMandiId: $('CmbGhallaMandi').value, loadingContractorId: $('CmbLoadingContractor').value,
            carriageContractorId: $('CmbCarriageContractor').value, vehicleNo: $('txtvehicleno').value.trim(), vehicleType: PP.selText('cmbvehicletype'),
            workingReportNo: $('txtWorkingReportNo').value.trim(), weighBridge: PP.selText('cmbWeighBridge'), biltyNo: $('txtbiltyno').value.trim(), freight: $('txtfreight').value,
            remarks: $('txtremarks').value.trim(), driverName: $('txtDriverName').value, driverMobile: $('txtDriverMobile').value, details: dtDetail
        };
        const problem = clientValidate(body);
        if (problem) { show(problem.msg); if (problem.focus) { const e = $(problem.focus); (e.classList.contains('pp-cmb') ? focusCombo(problem.focus) : e.focus()); } return; }
        if (!PP.confirm(updating ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        const res = await PP.post(API + '/save', body);
        show(res.message);
        const preview = $('ChkBox').checked;
        await reset();
        if (preview) generateReport(res.id);
    }
    async function btnsaveClick() { Id = 0; await insert(false); }          // btnsave_Click
    async function btnupdateClick() { await insert(Id > 0); }               // btnupdate_Click -> Insert()

    /* ---------- print / attachments / misc ---------- */
    function generateReport(printId) {                       // GenerateReport (:1145) -> 256-GatePassInward_WithDetailSlip.rpt
        if (!printId) { show('No Record Found For Display'); return; }
        if (window.printRpt) window.printRpt('256-GatePassInward_WithDetailSlip.rpt', { id: Number(printId) }); else show('The print runtime is not loaded on this page.');
    }
    async function showAttachments(id) {                     // grdhistory_LinkClicked (:1233)
        try {
            const rows = await PP.fetch(API + '/' + id + '/attachments');
            $('attBody').innerHTML = rows.length ? rows.map(r => '<div><a href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank" rel="noopener">' + PP.esc(r.Attachment.split(/[\\/]/).pop()) + '</a></div>').join('') : 'No attachments.';
            $('attModal').style.display = 'flex';
        } catch (e) { PP.error(e); }
    }
    async function btnAttachmentClick() {                    // btnAttachment_Click (:1002): AT.Show()
        if (Id > 0) return showAttachments(Id);
        show('Attachments can be viewed for a saved record here. Adding files from the web is not available yet.');
    }
    async function btnRefreshClick() {                       // btnRefresh_Click (:1169)
        const d = await PP.fetch(API + '/lookups'); bindLookups(d, false);
    }

    /* ---------- tabs and keyboard ---------- */
    function switchTab2(which) {
        $('tab2Detail').classList.toggle('active', which === 'Detail'); $('tab2Open').classList.toggle('active', which === 'Open');
        $('pageDetail').classList.toggle('active', which === 'Detail'); $('pageOpen').classList.toggle('active', which === 'Open');
    }
    function isHistory() { return $('viewHistory').style.display !== 'none'; }
    function switchView(mode) {                              // tabControl1_SelectedIndexChanged (:1110): History loads 50 rows
        const h = mode === 'History';
        if (h === isHistory()) return;
        $('viewForm').style.display = h ? 'none' : 'block'; $('viewHistory').style.display = h ? 'block' : 'none';
        if (h) gridhistory(50).catch(PP.error);
    }
    function closeForm() { if (window.opener) window.close(); else history.back(); }
    function onKey(e) {                                      // InwardGatePass_KeyDown (:1014)
        const k = e.key.toLowerCase(), formTab = !isHistory();
        if (e.ctrlKey && k === 'l' && isHistory()) { e.preventDefault(); gridhistory(0).catch(PP.error); }
        if (e.ctrlKey && k === 's' && formTab) { e.preventDefault(); if ($('btnsave').style.display !== 'none' && !$('btnsave').disabled) PP.withButton($('btnsave'), btnsaveClick); }
        if (e.ctrlKey && k === 'u') { e.preventDefault(); if ($('btnupdate').style.display !== 'none' && !$('btnupdate').disabled) PP.withButton($('btnupdate'), btnupdateClick); }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); if (!$('btnPrint').disabled) generateReport(Id); }
        if (e.ctrlKey && k === 'n' && formTab) { e.preventDefault(); reset(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); switchView(isHistory() ? 'Form' : 'History'); }
        if (formTab && e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('txtgpdate').focus(); }
        if (formTab && e.ctrlKey && e.key === 'ArrowLeft') { switchTab2('Detail'); }
        if (formTab && e.ctrlKey && e.key === 'ArrowRight') { switchTab2('Open'); }
        if (e.ctrlKey && k === 'e') { e.preventDefault(); closeForm(); }
        if (e.altKey && k === 'r') { e.preventDefault(); btnRefreshClick().catch(PP.error); }
        if (e.ctrlKey && e.key === 'Enter') { const r = $('grd')._current; if (r) { Id = Number(r.Id); getUpdate(Id); } }
        if (e.ctrlKey && e.key === 'ArrowDown') { switchTab2('Open'); }
    }

    document.addEventListener('DOMContentLoaded', () => {
        PP.installGuards(); PP.enterToTab($('viewForm'));
        if (window.PurchaseChrome) {
            PurchaseChrome.footer({ isHistory, toggle: () => switchView(isHistory() ? 'Form' : 'History'), watch: '#viewHistory' });
            ['#wrapHistory', '#wrapHistoryDetail', '#wrapOpen', '#wrapDetail'].forEach(b => PurchaseChrome.fullscreen(b, 'grid'));
        }
        $('btnnew').onclick = () => PP.withButton($('btnnew'), reset);
        $('btnsave').onclick = () => PP.withButton($('btnsave'), btnsaveClick);
        $('btnupdate').onclick = () => PP.withButton($('btnupdate'), btnupdateClick);
        $('btnAttachment').onclick = btnAttachmentClick;
        $('btnRefresh').onclick = () => PP.withButton($('btnRefresh'), btnRefreshClick);
        $('btndefvehicle').onclick = () => show('The Vehicle Type definition form (VehicleType) has not been ported to the web yet.');
        $('btnPrint').onclick = () => generateReport(Id);
        $('btnLoadAll').onclick = () => PP.withButton($('btnLoadAll'), () => gridhistory(0));
        $('Add').onclick = addClick; $('btnUpdateDetail').onclick = btnUpdateDetailClick;
        $('attClose').onclick = () => { $('attModal').style.display = 'none'; };
        $('tab2Detail').onclick = () => switchTab2('Detail'); $('tab2Open').onclick = () => switchTab2('Open');
        $('txtqty').addEventListener('input', weightCompared); $('txtPackUnit').addEventListener('input', weightCompared);
        $('txtvehicleno').addEventListener('input', e => { const t = e.target; t.value = t.value.toUpperCase(); t.setSelectionRange(t.value.length, t.value.length); });
        document.addEventListener('keydown', onKey);
        load().catch(PP.error);
    });
}());
