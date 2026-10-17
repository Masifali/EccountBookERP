/*
 * Screen 343  GatePassGeneralOutward  (Architecture.WinApp.Inventory_Definition.GatePassGeneralOutward) - Store Sale.
 * Page script. Desktop methods are named in the comments (GatePassGeneralOutward.cs). Server: /store/gate-pass-general-outward/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/store/gate-pass-general-outward/api';
    var SCREEN_PRINT = '254-RptInwardGatePassSlipGeneral.rpt';
    var ZERO = '...Select Any Value...';
    var S = { id: 0, doId: 0, approved: false, updateMode: false, rights: {}, files: [], removed: [], existing: [] };
    var cb = {}, G = {}, tabs1;
    var dtHistory = [];                                   // the desktop's single DtHistory table, bound to BOTH grids
    var GPTYPES = [{ Id: 1, Type: 'General' }, { Id: 2, Type: 'Store' }];
    var STATUS = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Accepted' }, { Id: 3, Status: 'Rejected' }];
    var WBR = [{ Id: 1, Type: 'Weighable' }, { Id: 2, Type: 'Non-Weighable' }];

    function fail(e) { return SE.dbError(e); }
    function zero(rows, key) { var z = { Id: 0 }; z[key] = ZERO; return [z].concat(rows || []); }
    function byText(c, key, text) {
        var rows = c.rows();
        for (var i = 0; i < rows.length; i++) if (String(rows[i][key]) === String(text)) { c.setValue(rows[i].Id); return true; }
        c.clear(); return false;
    }
    function dis(c, on) { c.el.disabled = on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function field(id) { return ($(id).value || '').trim(); }
    function net(n) { return String(parseFloat(Number(n).toPrecision(15))); }          // .NET double.ToString()
    function show(id, on) { var e = $(id); if (e) e.style.display = on ? '' : 'none'; }
    function vis(id) { var b = $(id); return b && b.style.display !== 'none' && !b.disabled; }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.type = XCombo('CmbGatePassType', { columns: [{ key: 'Type', caption: 'GatePassType' }], textKey: 'Type' });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'Godown Name' }], textKey: 'WareHouseName' });
        cb.veh = XCombo('cmbvehicletype', { columns: [{ key: 'VehicleDescription', caption: 'VehicleDescription' }], textKey: 'VehicleDescription' });
        cb.wb = XCombo('CmbWeighBridge', { columns: [{ key: 'Type', caption: 'WeighBridge' }], textKey: 'Type', onLeave: factoryVis });
        cb.status = XCombo('CmbStatus', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.party = XCombo('cmbPartyHistory', { columns: [{ key: 'PartyName', caption: 'PartyName' }], textKey: 'PartyName', popupWidth: 300 });
    }
    /* GatePassType(): BindDDL(ZeroIndex:false) - no zero row, nothing selected */
    function bindTypes() { cb.type.setData(GPTYPES); }
    /* WeighBridgeRoute() / Status(): BindDDLNew(ZeroIndex:true) - "...Select Any Value..." row, value 0 */
    function bindWb() { cb.wb.setData(zero(WBR, 'Type')); cb.wb.setValue(0); }
    function bindStatus() { cb.status.setData(zero(STATUS, 'Status')); cb.status.setValue(0); }
    /* WarehouseFill(): BindDDLNew(true); Rows[0].Activate() */
    function bindWarehouses(rows) { var keep = +cb.wh.value() || 0; cb.wh.setData(zero(rows, 'WareHouseName')); cb.wh.setValue(0); if (keep) { var ok = (rows || []).some(function (r) { return +r.Id === keep; }); if (ok) cb.wh.setValue(keep); else cb.wh.clear(); } }
    /* vehicleTypefill(): BindDDLNew(true) and re-select the previous value when still present */
    function bindVehicles(rows) { var keep = +cb.veh.value() || 0; cb.veh.setData(zero(rows, 'VehicleDescription')); cb.veh.setValue(0); if (keep) { var ok = (rows || []).some(function (r) { return +r.Id === keep; }); if (ok) cb.veh.setValue(keep); else cb.veh.clear(); } }
    /* LookUpBind() */
    function bindLookups(d) {
        function fillList(id, rows, key) { var dl = $(id); dl.innerHTML = ''; (rows || []).forEach(function (r) { var o = document.createElement('option'); o.value = r[key] == null ? '' : r[key]; dl.appendChild(o); }); }
        fillList('dlParty', d.parties, 'PartyName'); fillList('dlVariety', d.items, 'ItemName');
        cb.party.setData(d.parties || []);
    }

    /* FactoryWeight(): hide the factory / difference weight pair for Non-Weighable */
    function factoryVis() {
        var nonW = cb.wb.text() === 'Non-Weighable';
        ['txtFactoryWeight', 'lblFactoryWt', 'lblDiffWt', 'txtDifferenceWeight'].forEach(function (id) { show(id, !nonW); });
    }

    /* DocumentNoFill() */
    function docNoFill() {
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtgpno').value = String(r.nextNo); }).catch(fail);
    }

    /* DifferenceWeight() */
    function diffWeight() {
        if ($('txtDeliveryOrderNo').value !== '' && $('txtFactoryWeight').value !== '')
            $('txtDifferenceWeight').value = net(SE.toNum(field('txtDeliveryOrderWeight')) - SE.toNum(field('txtFactoryWeight')));
    }

    /* ------------------------------------------------------------------ grids (grd = Open list on the Form tab, grdhistory) */
    var COLS = [
        { k: 'Id', hide: true }, { k: 'GpType', t: 'GpType', w: 80 }, { k: 'GpDate', t: 'GpDate', w: 73, f: 'date' }, { k: 'GpNo', t: 'GpNo', w: 45 },
        { k: 'DeliverOrderNo', t: 'DeliverOrderNo', w: 60 }, { k: 'ManualNo', t: 'ManualNo', w: 90 }, { k: 'VehicleType', t: 'VehicleType', w: 80 },
        { k: 'VehicleNo', t: 'VehicleNo', w: 80 }, { k: 'PartyName', t: 'PartyName', w: 190 }, { k: 'ItemName', t: 'ItemName', w: 170 },
        { k: 'ItemQty', t: 'ItemQty', w: 70 }, { k: 'Warehouse', t: 'Warehouse', w: 150 }, { k: 'WBStatus', t: 'WBStatus', w: 75 },
        { k: 'FactoryWeight', t: 'FactoryWeight', w: 80, f: 'n0', sum: true }, { k: 'EntryUser', t: 'EntryUser', w: 100 },
        { k: 'EntryDate', t: 'EntryDate', w: 145, render: dt12 }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 145, render: dt12 },
        { k: 'ApprovedUser', t: 'ApprovedUser', w: 85 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 145, render: dt12 },
        { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100 }, { k: 'Remarks', t: 'Remarks', w: 130 }];
    /* "dd-MMM-yyyy hh:mm tt" */
    function dt12(v) {
        var p = SE.parts(v); if (!p) return '';
        var h = p.h % 12 === 0 ? 12 : p.h % 12;
        return SE.dMonY(v) + ' ' + (h < 10 ? '0' : '') + h + ':' + (p.i < 10 ? '0' : '') + p.i + ' ' + (p.h < 12 ? 'AM' : 'PM');
    }
    function cols(btns, linkAtt) {
        var c = COLS.map(function (x) { var y = {}; Object.keys(x).forEach(function (k) { y[k] = x[k]; }); if (linkAtt && y.k === 'NoOfAttachments') y.link = true; return y; });
        return btns.concat(c);
    }
    function makeGrids() {
        G.open = SE.grid('grd', { frozen: 1, onDbl: function (r) { edit(r.Id); },
            onBtn: function (k, r) { if (k === 'View') print254(r.Id); },
            cols: cols([{ k: 'View', t: 'View', w: 50, btn: 'View' }], false) });
        G.hist = SE.grid('grdhistory', { frozen: 2, onDbl: function (r) { edit(r.Id); },
            onBtn: function (k, r) { if (k === 'Edit') edit(r.Id); else if (k === 'Print') print254(r.Id); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(r.Id); },
            cols: cols([{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }], true) });
    }
    function renderGrids() { G.open.setRows(dtHistory.slice()); G.hist.setRows(dtHistory.slice()); }

    /* GetFormHistory(Status) - the table is only replaced when the procedure returned rows (DtHistory.Rows.Clear is inside if Rows.Count > 0) */
    function getHistory(status) {
        var mode = $('rdpqtyentry').checked ? 'entry' : ($('rdpqtymodify').checked ? 'modify' : ($('rdapproved').checked ? 'approved' : 'doc'));
        var body = { status: status || '', dateMode: mode, fromDate: $('FromDateHistory').value, toDate: $('ToDateHistory').value,
                     fromDocNo: SE.toInt($('txtFromDocNoHistory').value), toDocNo: SE.toInt($('txtToDocNoHistory').value), partyId: +cb.party.value() || 0 };
        return SE.post(API + '/history', body).then(function (rows) { if (rows && rows.length > 0) dtHistory = rows; });
    }
    /* grdfrmfill() */
    function grdfrmfill() { return getHistory('Open').then(renderGrids).catch(fail); }
    /* FromHistoryFill() */
    function fromHistoryFill() { return getHistory('').then(renderGrids).catch(fail); }

    /* ------------------------------------------------------------------ Load */
    function load() {
        makeCombos();
        tabs1 = SE.tabs('tabControl1', function (id) { if (id === 'tabPage1') grdfrmfill(); });
        makeGrids();
        ['txtqty'].forEach(function (id) { decimalOnly($(id)); });
        SE.digitsOnly($('txtFromDocNoHistory')); SE.digitsOnly($('txtToDocNoHistory'));
        show('btnupdate', false); show('btnsave', true);
        $('ChkBox').checked = true;
        $('txtgpdate').value = SE.today(); $('FromDateHistory').value = SE.today(); $('ToDateHistory').value = SE.today();
        bindTypes(); bindWb(); bindStatus();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {};
            bindWarehouses(d.warehouses); bindVehicles(d.vehicleTypes);
            if (d.nextNo > 0) $('txtgpno').value = String(d.nextNo);
            bindLookups(d);
            $('btnsave').disabled = !S.rights.save; $('btnPrint').disabled = !S.rights.print; $('btnupdate').disabled = !S.rights.update;
            factoryVis();
            cb.type.focus();
        }).catch(fail);
        wire();
    }
    /* OnlytextdecimelFunction */
    function decimalOnly(el) {
        el.addEventListener('keypress', function (e) {
            if (e.ctrlKey || e.key.length !== 1) return;
            if (!/[\d.]/.test(e.key) || (e.key === '.' && el.value.indexOf('.') >= 0)) e.preventDefault();
        });
    }

    /* ------------------------------------------------------------------ Reset (btnnew_Click) */
    function reset() {
        S.files = []; S.removed = []; S.existing = [];
        S.doId = 0; S.id = 0; S.updateMode = false; S.approved = false;
        $('txtApprovedBy').value = ''; $('txtBalQty').value = ''; $('txtDeliveryOrderNo').value = '';
        $('txtFactoryWeight').value = '0'; $('txtDeliveryOrderWeight').value = '0'; $('txtDifferenceWeight').value = '0';
        cb.veh.clear(); $('txtvehicleno').value = ''; $('txtManualNo').value = ''; $('txtqty').value = ''; $('txtremarks').value = '';
        cb.wh.clear();
        dtHistory = dtHistory.slice(); G.open.setRows([]);                     // table.Clear(); grd.DataSource = table
        var p = grdfrmfill();
        SE.api(API + '/lookups').then(bindLookups).catch(fail);
        $('txtPartyName').value = ''; $('txtVarietyName').value = '';
        show('btnsave', true); show('btnupdate', false);
        dis(cb.wb, false); dis(cb.type, false); $('txtDeliveryOrderNo').disabled = false;
        $('txtgpdate').focus();
        docNoFill();
        return p;
    }

    /* btnRefresh_Click: vehicleTypefill(); WarehouseFill(); LookUpBind(); */
    function refresh() {
        return Promise.all([SE.api(API + '/vehicle-types').then(bindVehicles), SE.api(API + '/warehouses').then(bindWarehouses), SE.api(API + '/lookups').then(bindLookups)]).catch(fail);
    }

    /* ------------------------------------------------------------------ formvalidation + Insert */
    function validate() {
        function stop(msg, focus) { return SE.alert(msg).then(function () { if (focus) focus(); return false; }); }
        if (cb.type.text().trim() === '' || !(+cb.type.value())) return stop('Please Select CmbGatePassType', function () { cb.type.focus(); });
        if (field('txtgpno') === '' || field('txtgpno') === '0') return stop('Gp No Filed Required', function () { $('txtgpno').focus(); });
        if (field('txtvehicleno') === '') return stop('Please Select Vehicle No', function () { $('txtvehicleno').focus(); });
        if (cb.veh.text().trim() === '' || !(+cb.veh.value())) return stop('Please Select Vehicle Type', function () { cb.veh.focus(); });
        if (cb.type.text() !== 'General' && (field('txtDeliveryOrderNo') === '' || field('txtDeliveryOrderNo') === '0')) return stop('DeliveryOrder No Field Required', function () { $('txtDeliveryOrderNo').focus(); });
        if (cb.status.text().trim() === '' || !(+cb.status.value())) return stop('Please Select Status', function () { cb.status.focus(); });
        if (cb.wb.text().trim() === '' || !(+cb.wb.value())) return stop('Please Select WeighBridge', function () { cb.wb.focus(); });
        if (field('txtqty') === '' || field('txtqty') === '0') return stop('Qty Field Required', function () { $('txtqty').focus(); });
        return Promise.resolve(true);
    }
    function save(forceNew) {
        if (forceNew) S.id = 0;                                    // btnsave_Click: Id = 0
        return validate().then(function (ok) {
            if (!ok) return;
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    id: S.id, gpDate: $('txtgpdate').value, gpNo: field('txtgpno'), qty: field('txtqty'), vehicleNo: field('txtvehicleno'),
                    deliveryOrderNo: field('txtDeliveryOrderNo'), remarks: field('txtremarks'), manualNo: field('txtManualNo'), factoryWeight: field('txtFactoryWeight'),
                    gatePassTypeText: cb.type.text().trim(), gatePassTypeId: +cb.type.value() || 0, vehicleTypeText: cb.veh.text().trim(), vehicleTypeId: +cb.veh.value() || 0,
                    statusText: cb.status.text().trim(), statusId: +cb.status.value() || 0, weighBridgeText: cb.wb.text().trim(), weighBridgeId: +cb.wb.value() || 0,
                    partyName: field('txtPartyName'), varietyName: field('txtVarietyName'), warehouseId: +cb.wh.value() || 0,
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed }
                };
                return SE.post(API, body).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkBox').checked;
                        return reset().then(function () { if (preview) print254(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Error'); });
            });
        });
    }

    /* ------------------------------------------------------------------ ReadById + GetWeighBridgeWeight */
    function edit(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            var r = d.rec;
            if (tabs1.index() !== 0) tabs1.select('tabPage1');                    // tabControl1.SelectedIndex = 0 (fires grdfrmfill from the tab change)
            $('txtgpdate').value = SE.dateInput(r.GpDate); $('txtgpno').value = r.GpSrNo == null ? '' : r.GpSrNo;
            $('txtqty').value = r.NoOfPackages == null ? '' : r.NoOfPackages;
            $('txtremarks').value = r.OtherRemarks || '';
            byText(cb.type, 'Type', r.GatepassType);
            $('txtvehicleno').value = r.VehicleNo || '';
            byText(cb.veh, 'VehicleDescription', r.VehicleType);
            $('txtVarietyName').value = r.VarietyName || '';
            $('txtPartyName').value = r.OtherSupCust || '';
            cb.wh.setValue(+r.WarehouseId || 0);
            byText(cb.wb, 'Type', r.Status);
            $('txtDeliveryOrderNo').value = r.SupplierContractCode || '';
            $('txtManualNo').value = r.ManualNo || '';
            byText(cb.status, 'Status', r.DocAttachment);
            S.doId = +r.PurchaseOrderId || 0;
            $('txtFactoryWeight').value = r.FactoryWeight == null ? '0' : net(r.FactoryWeight);
            if (SE.toNum(r.FactoryWeight) > 0) dis(cb.wb, true);
            $('txtDeliveryOrderWeight').value = r.SupplierWeight == null ? '0' : net(r.SupplierWeight);
            $('txtDifferenceWeight').value = r.DifferenceWeight == null ? '0' : net(r.DifferenceWeight);
            S.approved = !!r.IsApproved; $('txtApprovedBy').value = r.ApprovedUserName || '';
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            var p = Promise.resolve();
            if (r.GatepassType === 'Store') {
                if (d.doNotFound) {
                    S.doId = 0; $('txtDeliveryOrderNo').value = '';
                    p = SE.alert('Record Not Found against this Number');          // MessageBox + return: the lines below are skipped
                } else {
                    S.doId = d.deliveryOrderId; $('txtremarks').value = d.loadingInstructions || ''; $('txtDeliveryOrderWeight').value = d.doWeight || '';
                    diffWeight(); $('txtBalQty').value = d.balQty;
                    dis(cb.type, true); $('txtDeliveryOrderNo').disabled = true; show('btnupdate', true); show('btnsave', false); S.updateMode = true; factoryVis();
                }
            } else {
                dis(cb.type, true); $('txtDeliveryOrderNo').disabled = true; show('btnupdate', true); show('btnsave', false); S.updateMode = true; factoryVis();
            }
            return p.then(function () {
                // GetWeighBridgeWeight()
                if (d.wbFound) { $('txtFactoryWeight').value = net(d.wbNetWeight); if (SE.toNum($('txtFactoryWeight').value) > 0) dis(cb.wb, true); }
                else $('txtFactoryWeight').value = '0';
                $('txtBalQty').value = d.balQty;
                factoryVis();
            });
        }).catch(fail);
    }

    /* textBox1_Leave (txtDeliveryOrderNo.Leave) */
    function doLeave() {
        if (!vis('btnsave')) return;
        if (cb.type.text() === 'Store') {
            return SE.api(API + '/delivery-order' + SE.q({ docNo: SE.toInt(field('txtDeliveryOrderNo')) })).then(function (r) {
                if (r.found) {
                    S.doId = r.deliveryOrderId; $('txtremarks').value = r.loadingInstructions || ''; $('txtDeliveryOrderWeight').value = r.netWeight || '';
                    byText(cb.veh, 'VehicleDescription', r.vehicleType); $('txtvehicleno').value = r.vehicleNo || '';
                    diffWeight(); $('txtBalQty').value = r.balQty;
                } else {
                    S.doId = 0;
                    return SE.alert('Record Not Found against this Number').then(function () { return reset(); });
                }
            }).catch(fail);
        }
        S.doId = 0; $('txtDeliveryOrderNo').value = '0'; $('txtDeliveryOrderWeight').value = '0'; $('txtVarietyName').value = '0';
    }

    /* ------------------------------------------------------------------ print (CommonServices.GatePassGeneralInward254(id, 92)) */
    function print254(id) { return SE.printRpt(SCREEN_PRINT, { id: id, documentTypeId: 92 }); }
    function btnPrint() {
        if (!S.approved) return SE.alert('GatePass Not Approve');
        print254(S.id);
    }

    /* ------------------------------------------------------------------ attachments (AT.Show) */
    function showAttachments(gpId) { return SE.api(API + '/' + gpId + '/attachments').then(function (rows) { renderAttachments(rows, gpId, true); }).catch(fail); }
    function openAttachmentDialog() {
        var rows = (S.existing || []).filter(function (r) { return S.removed.indexOf(r.Id) < 0; });
        renderAttachments(rows, S.id, false);
    }
    function renderAttachments(rows, gpId, readOnly) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
        rows.forEach(function (r) {
            h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (gpId > 0 ? '<a class="lnk" href="' + API + '/' + gpId + '/attachments/' + r.Id + '" target="_blank">Open</a>' : '') +
                 (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
        });
        S.files.forEach(function (f, i) { if (!readOnly) h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
        h += '</tbody></table></div>';
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the gate pass is saved.</span></div>';
        var pop = SE.pop('Attachments', h, [{ t: 'Close' }]);
        pop.open();
        pop.body.addEventListener('click', function (e) {
            var rm = e.target.getAttribute && e.target.getAttribute('data-rm');
            if (rm) { S.removed.push(+rm); pop.close(); openAttachmentDialog(); }
            if (e.target.hasAttribute && e.target.hasAttribute('data-nf')) { S.files.splice(+e.target.getAttribute('data-nf'), 1); pop.close(); openAttachmentDialog(); }
        });
        var fi = pop.body.querySelector('#atFile');
        if (fi) fi.addEventListener('change', function () {
            var list = Array.prototype.slice.call(fi.files), left = list.length;
            if (!left) return;
            list.forEach(function (f) {
                if (f.size > 5 * 1024 * 1024) { SE.alert(f.name + ' exceeds 5 MB'); left--; return; }
                var fr = new FileReader();
                fr.onload = function () { S.files.push({ name: f.name, base64: String(fr.result).split(',')[1] || '' }); if (--left <= 0) { pop.close(); openAttachmentDialog(); } };
                fr.readAsDataURL(f);
            });
        });
    }

    /* ------------------------------------------------------------------ history tab buttons */
    function newHistory() {                                          // btnNewHistory_Click
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('txtFromDocNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        G.hist.setRows([]); $('rdpqtydoc').checked = true;
    }
    function lookUpBind() { return SE.api(API + '/lookups').then(bindLookups).catch(fail); }   // toolStripButton1_Click

    /* ------------------------------------------------------------------ shortcut keys (MakeShortCutKeys / InwardGatePass_KeyDown) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on GatePass Type'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Item Combo in Detail Box'], ['Ctrl+ArrowRight', 'To change Grid Tab Control,'],
        ['Ctrl+Enter', 'For Update Record When Focus On Any Grid']];
    function closeForm() { if (window.history.length > 1) window.history.back(); }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = (e.key || '').toLowerCase(), idx = tabs1.index();
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORT); return; }
        if (idx === 0 && e.ctrlKey && k === 's' && vis('btnsave')) { e.preventDefault(); save(true); }
        if (idx === 0 && e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (idx === 1) { tabs1.select('tabPage1'); cb.type.focus(); } else { tabs1.select('tabPage2'); $('grdhistory').focus(); } }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); closeForm(); }
        if (vis('btnupdate') && e.ctrlKey && k === 'u' && idx === 0) { e.preventDefault(); save(false); }
        if (vis('btnPrint') && e.ctrlKey && k === 'p' && idx === 0) { e.preventDefault(); btnPrint(); }
        if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.type.focus(); }
        if (e.ctrlKey && e.key === 'ArrowUp') cb.type.focus();
        if (e.ctrlKey && e.key === 'ArrowDown') $('grd').focus();
    }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh;
        $('btnsave').onclick = function () { save(true); };
        $('btnupdate').onclick = function () { save(false); };
        $('btnPrint').onclick = btnPrint;
        $('btnAttachment').onclick = openAttachmentDialog;
        $('btnWeighBridgeLookUp').onclick = function () { window.open('/weighbridge/weigh-bridge-general-lookups', '_blank'); };   // new WeighBridgeGeneralLookups(UserAccount).Show()
        $('btnshortcut').onclick = function () { SE.shortcuts(SHORT); };
        $('btnNewHistory').onclick = newHistory; $('toolStripButton1').onclick = lookUpBind; $('btnshow').onclick = fromHistoryFill;
        $('txtDeliveryOrderNo').addEventListener('blur', doLeave);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
