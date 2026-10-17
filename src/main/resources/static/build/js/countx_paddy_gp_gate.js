/* Screen 464 "GPI FOR GATE PURCHASE" - Architecture.WinApp.Paddy_Purchase/frmGatePassInwardForGate.cs (DocumentTypeId 51, RefDocumentTypeId 110).
   Line numbers in the comments are to that file. */
(function () {
    'use strict';
    const API = '/api/paddy-purchase/gp-gate';
    const $ = PP.$;
    let Id = 0;                      // Id (int field): set by getUpdate, grd/grdhistory clicks and grdhistory_SelectionChanged
    let dtDetail = [];               // dtDetail rows: ItemId, ItemName, ItemQty, PackingTypeId, PackingType, PackUnit, Weight
    let updateDetailIndex = -1;
    let supplierInfo = [];           // dtSupplier (Id, CNIC, MobilePersonal)
    let rights = { canSave: false, canUpdate: false, canPrint: false };
    let districtGen = 0, historySelectGen = 0;

    function show(msg) { alert(msg); }

    /* ---------- InwardGatePass_Load (:769) ---------- */
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
        supplierInfo = d.supplierInfo || [];
        PP.bind('cmbsupp', d.suppliers, 'id', 'name');
        PP.bind('CmbVariety', d.items, 'id', 'name');
        PP.bind('cmbvehicletype', d.vehicleTypes, 'id', 'name', { activateFirst: true });   // vehicleTypefill: Rows[0].Activate()
        PP.bind('cmbcity', d.cities, 'id', 'name', { activateFirst: true });                // CityFill: Rows[0].Activate()
        if (first) {
            PP.bind('cmbWeighBridge', d.weighBridges, 'id', 'name', { activateFirst: true });     // cmbWeighBridgeFill
            PP.bind('CmbPackingType', d.packingTypes, 'id', 'name', { activateFirst: true });     // PackingTypeBind
        }
    }
    function stampTimes() { const t = PP.localStamp().slice(11, 16); $('txtintime').value = t; $('txtouttime').value = t; }

    /* ---------- grids ---------- */
    function renderDetail() {
        PP.renderGrid('grdGpDetail', dtDetail.map((r, i) => Object.assign({ _i: i }, r)), {
            hide: ['_i', 'ItemId', 'PackingTypeId'], numeric: ['ItemQty', 'PackUnit', 'Weight'],
            totals: { ItemQty: 0, Weight: 0 }, dec: { ItemQty: 0, PackUnit: 0, Weight: 0 },
            onDbl: row => grdGpDetail_DoubleClick(row._i)
        });
    }
    /* grdfrmfill (:872) / grdfrmsetting (:908) */
    async function grdfrmfill() {
        const rows = await PP.fetch(API + '/open');
        PP.renderGrid('grd', rows, {
            hide: ['Id', 'RefDocumentTypeId'], dateCols: ['GpDate', 'EntryDate', 'ModifyDate', 'InTime'],
            post: [{ key: 'Print', label: 'Print', onclick: r => generateReport(r.Id) }, { key: 'Edit', label: 'Edit', onclick: r => { Id = Number(r.Id); getUpdate(Id); } }],
            onDbl: r => { Id = Number(r.Id); getUpdate(Id); }
        });
    }
    /* gridhistory (:818) / grdhistorysetting (:840) */
    async function gridhistory(n) {
        const rows = await PP.fetch(API + '/history?noOfRecords=' + (n || 0));
        PP.renderGrid('grdhistory', rows, {
            hide: ['Id', 'RecordNo'], caption: { CompanyName: 'SupplierName', Description: 'CityName' },
            dateCols: ['GpDate', 'InTime', 'OutTime'], link: ['NoOfAttachments'], frozen: 2,
            pre: [{ key: 'Print', label: 'Print', onclick: r => generateReport(r.Id) }, { key: 'Edit', label: 'Edit', onclick: r => { Id = Number(r.Id); getUpdate(Id).then(() => switchView('Form')); } }],
            onRow: r => grdhistory_SelectionChanged(r),
            onDbl: r => { Id = Number(r.Id); getUpdate(Id).then(() => switchView('Form')); },
            onLink: (k, r) => showAttachments(r.Id)
        });
        if (!rows.length) PP.renderGrid('grdHistoryDetail', [], {});
        else grdhistory_SelectionChanged(rows[0]);          // the first row is current after the grid loads
    }
    /* grdhistory_SelectionChanged (:1695) -> FillHistoryDetail (:1393) */
    async function grdhistory_SelectionChanged(row) {
        if (!row) return;
        Id = Number(row.Id);
        const gen = ++historySelectGen;
        try {
            const rows = await PP.fetch(API + '/history-detail/' + Id);
            if (gen !== historySelectGen) return;
            PP.renderGrid('grdHistoryDetail', rows, { numeric: ['Qty', 'Weight'], totals: { Qty: 2, Weight: 2 }, dec: { Qty: 0, Weight: 0 } });
        } catch (e) { PP.error(e); }
    }

    /* ---------- detail row editing ---------- */
    function weightCompared() {                              // WeightComparedPoWtCalculation (:1483)
        const q = PP.num($('txtqty').value), p = PP.num($('txtPackUnit').value);
        $('txtWeight').value = (q > 0 && p > 0) ? String(q * p) : '0';
    }
    function focusCombo(id) { const w = $(id).parentElement && $(id).parentElement.querySelector('.dtcombo-input'); (w || $(id)).focus(); }
    function formValidationDetail() {                        // (:524)
        if (!$('CmbVariety').value) { show('ItemName Field is Required'); focusCombo('CmbVariety'); return false; }
        if (PP.num($('txtPackUnit').value) <= 0) { show('PackUnit Field is Required'); $('txtPackUnit').focus(); return false; }
        if (Math.trunc(PP.num($('txtqty').value)) <= 0) { show('ItemQty Field is Required'); $('txtqty').focus(); return false; }
        if (PP.num($('txtWeight').value) <= 0) { show('Weight Field is Required'); $('txtWeight').focus(); return false; }
        return true;
    }
    function resetDetail() {                                 // ResetDetail (:1625)
        PP.setSel('CmbVariety', ''); $('txtqty').value = ''; PP.setSel('CmbPackingType', ''); $('txtPackUnit').value = ''; $('txtWeight').value = '';
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
        focusCombo('CmbVariety');
    }
    function addClick() {                                    // Add_Click (:1606)
        if (!formValidationDetail()) return;
        dtDetail.push({ ItemId: $('CmbVariety').value, ItemName: PP.selText('CmbVariety'), ItemQty: PP.num($('txtqty').value),
            PackingTypeId: $('CmbPackingType').value, PackingType: PP.selText('CmbPackingType'), PackUnit: PP.num($('txtPackUnit').value), Weight: PP.num($('txtWeight').value) });
        renderDetail(); resetDetail();
    }
    function grdGpDetail_DoubleClick(i) {                    // (:1645)
        const r = dtDetail[i]; if (!r) return;
        updateDetailIndex = i;
        PP.setSel('CmbVariety', r.ItemId); $('txtqty').value = r.ItemQty; PP.setSel('CmbPackingType', r.PackingTypeId);
        $('txtPackUnit').value = r.PackUnit; $('txtWeight').value = r.Weight;
        $('Add').style.display = 'none'; $('btnUpdateDetail').style.display = ''; $('btnCancelUpdateDetial').style.display = '';
        focusCombo('CmbVariety');
    }
    function btnUpdateDetailClick() {                        // (:1670)
        if (!formValidationDetail()) return;
        const r = dtDetail[updateDetailIndex]; if (!r) return;
        r.ItemId = $('CmbVariety').value; r.ItemName = PP.selText('CmbVariety'); r.PackingTypeId = $('CmbPackingType').value; r.PackingType = PP.selText('CmbPackingType');
        r.ItemQty = PP.num($('txtqty').value); r.PackUnit = PP.num($('txtPackUnit').value); r.Weight = PP.num($('txtWeight').value);
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
        renderDetail(); resetDetail();
    }

    /* ---------- city / supplier Leave ---------- */
    function bindDistrictTehsil(d) {
        if (d.districts.length) {
            PP.bind('cmbDistrict', d.districts, 'id', 'name', { activateFirst: true });
            PP.bind('CmbTehseel', d.tehsils, 'id', 'name', { activateFirst: true });
        } else { PP.bind('cmbDistrict', [], 'id', 'name'); PP.bind('CmbTehseel', [], 'id', 'name'); }
    }
    async function cmbcityLeave() {                          // cmbcity_Leave (:1509)
        const city = $('cmbcity').value; if (!city) return;
        const gen = ++districtGen;
        const d = await PP.fetch(API + '/district-tehsil?cityId=' + encodeURIComponent(city));
        if (gen !== districtGen) return;
        bindDistrictTehsil(d);
    }
    function cmbsuppLeave() {                                // cmbsupp_Leave (:1570)
        const row = supplierInfo.find(r => String(r.id) === String($('cmbsupp').value));
        if (row) { $('txtSupplierCNIC').value = row.cnic; $('txtSupplierMobileNo').value = row.mobile; }
    }

    /* ---------- getUpdate (:553) ---------- */
    async function getUpdate(id) {
        try {
            const data = await PP.fetch(API + '/' + id);
            const h = data.header;
            const get = k => { for (const x in h) if (x.toLowerCase() === k.toLowerCase()) return h[x]; return null; };
            const txt = k => get(k) == null ? '' : String(get(k));
            $('btnupdate').style.display = ''; $('btnsave').style.display = 'none';
            switchTab2('Detail');
            $('txtbiltyno').value = get('BiltyNo') || '';
            PP.setSel('cmbcity', get('CityId'));
            bindDistrictTehsil(data.districtTehsil);          // cmbcity_Leave(null, null) inside getUpdate
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
            $('txtSupplierCNIC').value = txt('SupplierCNICNO');
            $('txtSupplierMobileNo').value = txt('SupplierMobileNo');
            $('txtTransporter').value = txt('TransporterName');
            $('txtDriverName').value = txt('DriverName');
            $('txtDriverCNIC').value = txt('DriverCNICNO');
            $('txtDriverMobile').value = txt('DriverMobileNo');
            PP.setSel('cmbsupp', get('SupplierCustomerId'));
            $('txtSupplierWeight').value = txt('SupplierWeight');
            dtDetail = data.details.map(r => ({ ItemId: r.ItemId, ItemName: r.ItemName, ItemQty: r.ItemQty, PackingTypeId: r.PackingTypeId, PackingType: r.PackingType, PackUnit: r.PackUnit, Weight: r.Weight }));
            renderDetail();
            Id = Number(id);
        } catch (e) { PP.error(e); reset(); }
    }

    /* ---------- Reset (:960) ---------- */
    async function reset() {
        dtDetail = []; renderDetail(); switchTab2('Detail');
        $('txtvehicleno').value = ''; $('txtbiltyno').value = '';
        PP.bind('cmbWeighBridge', rights.weighBridges || [{ id: 1, name: 'Auto' }, { id: 2, name: 'Mannual' }], 'id', 'name', { activateFirst: true });   // Rows[0].Activate()
        PP.setSel('CmbVariety', ''); PP.setSel('cmbsupp', '');
        $('txtqty').value = ''; $('txtfreight').value = ''; $('txtSupplierWeight').value = ''; $('txtremarks').value = '';
        PP.setSel('cmbcity', '');
        PP.bind('cmbDistrict', [], 'id', 'name'); PP.bind('CmbTehseel', [], 'id', 'name');
        ['txtSupplierCNIC', 'txtSupplierMobileNo', 'txtDriverName', 'txtDriverCNIC', 'txtDriverMobile'].forEach(i => { $(i).value = ''; });
        $('btnsave').style.display = ''; $('btnupdate').style.display = 'none';
        stampTimes(); $('cmbWeighBridge').disabled = false;
        try {
            const g = await PP.fetch(API + '/gp-no'); if (g.gpNo > 0) $('txtgpno').value = g.gpNo;
            await grdfrmfill();
        } catch (e) { PP.error(e); }
        $('txtgpdate').focus();
    }

    /* ---------- save / update (:629, :757, :642) ---------- */
    async function insert(updating) {
        // formvalidation() runs first on the desktop; the server repeats it in the same order and its message is shown
        const body = {
            id: updating ? Id : 0, gpNo: $('txtgpno').value.trim(), gpDate: $('txtgpdate').value, vehicleNo: $('txtvehicleno').value.trim(),
            vehicleType: PP.selText('cmbvehicletype'), weighBridge: PP.selText('cmbWeighBridge'), cityId: $('cmbcity').value, districtId: $('cmbDistrict').value,
            tehsilId: $('CmbTehseel').value, supplierMobileNo: $('txtSupplierMobileNo').value.trim(), supplierCnic: $('txtSupplierCNIC').value, transporter: $('txtTransporter').value,
            driverName: $('txtDriverName').value.trim(), driverCnic: $('txtDriverCNIC').value.trim(), driverMobile: $('txtDriverMobile').value.trim(),
            supplierId: $('cmbsupp').value, biltyNo: $('txtbiltyno').value.trim(), freight: $('txtfreight').value, remarks: $('txtremarks').value.trim(),
            supplierWeight: $('txtSupplierWeight').value.trim(), details: dtDetail
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
    function clientValidate(b) {                             // formvalidation (:447): same order and text (the server repeats it)
        if (!b.vehicleNo) return { msg: 'VehicleNo Field Required', focus: 'txtvehicleno' };
        if (!b.vehicleType) return { msg: 'VehicleType Field Required', focus: 'cmbvehicletype' };
        if (!b.gpNo || b.gpNo === '0') return { msg: 'GatePassNo Field Required', focus: 'txtgpno' };
        if (!b.weighBridge) return { msg: 'WeighBridge Status Field Required', focus: 'cmbWeighBridge' };
        if (!b.cityId) return { msg: 'City Field Required', focus: 'cmbcity' };
        if (!b.districtId) return { msg: 'District Field Required', focus: 'cmbDistrict' };
        if (!b.tehsilId) return { msg: 'Tehsil Field Required', focus: 'CmbTehseel' };
        if (!b.supplierMobileNo || b.supplierMobileNo === '0') return { msg: 'SupplierMobileNo Field Required', focus: 'txtSupplierMobileNo' };
        if (!b.driverName || b.driverName === '0') return { msg: 'DriverName Field Required', focus: 'txtDriverName' };
        if (!b.driverCnic || b.driverCnic === '0') return { msg: 'DriverCNIC Field Required', focus: 'txtDriverCNIC' };
        if (!b.driverMobile || b.driverMobile === '0') return { msg: 'DriverMobile Field Required', focus: 'txtDriverMobile' };
        if (!b.supplierId) return { msg: 'Supplier Name Field Required', focus: 'cmbsupp' };
        return null;
    }
    async function btnsaveClick() { Id = 0; await insert(false); }          // btnsave_Click
    async function btnupdateClick() { await insert(Id > 0); }               // btnupdate_Click -> Insert()

    /* ---------- print / attachments / misc ---------- */
    function generateReport(printId) {                       // GenerateReport (:1163) -> 256-GatePassInward_WithDetailSlip.rpt
        if (!printId) { show('No Record Found For Display'); return; }
        if (window.printRpt) window.printRpt('256-GatePassInward_WithDetailSlip.rpt', { id: Number(printId) }); else show('The print runtime is not loaded on this page.');
    }
    async function showAttachments(id) {                     // grdhistory_LinkClicked (:1262)
        try {
            const rows = await PP.fetch(API + '/' + id + '/attachments');
            $('attBody').innerHTML = rows.length ? rows.map(r => '<div><a href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank" rel="noopener">' + PP.esc(r.Attachment.split(/[\\/]/).pop()) + '</a></div>').join('') : 'No attachments.';
            $('attModal').style.display = 'flex';
        } catch (e) { PP.error(e); }
    }
    async function btnAttachmentClick() {                    // btnAttachment_Click (:1020): AT.Show()
        if (Id > 0) return showAttachments(Id);
        show('Attachments can be viewed for a saved record here. Adding files from the web is not available yet.');
    }
    async function btnRefreshClick() {                       // btnRefresh_Click (:1187)
        const d = await PP.fetch(API + '/lookups'); bindLookups(d, false);
    }

    /* ---------- tabs and keyboard ---------- */
    function switchTab2(which) {
        $('tab2Detail').classList.toggle('active', which === 'Detail'); $('tab2Open').classList.toggle('active', which === 'Open');
        $('pageDetail').classList.toggle('active', which === 'Detail'); $('pageOpen').classList.toggle('active', which === 'Open');
    }
    function isHistory() { return $('viewHistory').style.display !== 'none'; }
    function switchView(mode) {                              // tabControl1_SelectedIndexChanged (:1128): History loads 50 rows
        const h = mode === 'History';
        $('viewForm').style.display = h ? 'none' : 'block'; $('viewHistory').style.display = h ? 'block' : 'none';
        if (h) gridhistory(50).catch(PP.error);
    }
    function closeForm() { if (window.opener) window.close(); else history.back(); }
    function onKey(e) {                                      // InwardGatePass_KeyDown (:1032)
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
        $('btndefinecity').onclick = () => { if (!window.open('/master-data/city', '_blank')) show('The browser blocked the Define City window. Allow pop-ups for this site and retry.'); };
        $('btnPrint').onclick = () => generateReport(Id);
        $('btnLoadAll').onclick = () => PP.withButton($('btnLoadAll'), () => gridhistory(0));
        $('Add').onclick = addClick; $('btnUpdateDetail').onclick = btnUpdateDetailClick;
        $('attClose').onclick = () => { $('attModal').style.display = 'none'; };
        $('tab2Detail').onclick = () => switchTab2('Detail'); $('tab2Open').onclick = () => switchTab2('Open');
        $('txtqty').addEventListener('input', weightCompared); $('txtPackUnit').addEventListener('input', weightCompared);
        $('txtvehicleno').addEventListener('input', e => { const t = e.target; t.value = t.value.toUpperCase(); t.setSelectionRange(t.value.length, t.value.length); });
        $('cmbcity').addEventListener('change', () => cmbcityLeave().catch(PP.error));
        $('cmbsupp').addEventListener('change', cmbsuppLeave);
        PP.onComboLeave('cmbcity', () => cmbcityLeave().catch(PP.error));      // UltraCombo.Leave
        PP.onComboLeave('cmbsupp', cmbsuppLeave);
        document.addEventListener('keydown', onKey);
        load().catch(PP.error);
    });
}());
