/*
 * Screen 545  OutwardGatePass_St  (Architecture.WinApp.Steel.Sale.OutwardGatePass_St)
 * Page script. Desktop methods are named in the comments (OutwardGatePass_St.cs). Server: /sale/steel/outward-gate-pass/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/steel/outward-gate-pass/api', fail = SS.fail;
    var S = { id: 0, soId: 0, gpId: 0, approved: false, data: {}, rights: {}, files: [], removed: [], existing: [] };
    var cb = {}, G = {}, tabs1, tabs2;
    var STATUS = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Accepted' }, { Id: 3, Status: 'Rejected' }];                         // Status() :480
    var WBS = [{ Id: 'Auto', Status: 'Auto' }, { Id: 'Mannual', Status: 'Manual' }];                                               // WeighBridgeStatus() :499
    var BASED = [{ Id: 'DeliverOrder', Type: 'DeliveryOrder' }, { Id: 'SaleOrder', Type: 'SaleOrder' }, { Id: 'GateSale', Type: 'GateSale' }];   // GpBasedOn() :517

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.status = XCombo('CmbStatus', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.wb = XCombo('CmbWeighBridgeStatus', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.based = XCombo('CmbGPBasedOn', { columns: [{ key: 'Type', caption: 'Type' }], textKey: 'Type', onSelect: function () { basedOnLeave(); } });
        cb.type = XCombo('cmbgptype', { columns: [{ key: 'GpTypeDescription', caption: 'Gate Pass Type' }], textKey: 'GpTypeDescription' });
        cb.veh = XCombo('cmbvehicletype', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], textKey: 'VehicleDescription' });
        cb.city = XCombo('cmbcity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName' });
        cb.supp = XCombo('cmbsupp', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420 });
        cb.hist = XCombo('CmbCustomerHistory', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 400 });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'Godown Name' }], textKey: 'WareHouseName' });
        cb.item = XCombo('CmbVariety', { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 500 });
        cb.status.setData(STATUS); cb.wb.setData(WBS); cb.based.setData(BASED);
    }
    var selectRow = SS.selectRow, byText = SS.byText, dis = SS.dis, field = SS.val;
    /* BindDDLNew(rows, CmbVariety, valueKey, "ItemName") - the combo's value column is always Id here */
    function setItems(rows, valueKey) {
        cb.item.setData((rows || []).map(function (r) { return { Id: r[valueKey || 'Id'], ItemName: r.ItemName }; }));
    }
    function basedText() { return cb.based.text(); }
    function basedVal() { var r = cb.based.row(); return r ? r.Id : ''; }

    /* ------------------------------------------------------------------ CmbGPBasedOn_Leave :809 */
    function basedOnLeave() {
        var t = basedText(), o = $('CmbOrderno');
        if (t === 'DeliveryOrder' || t === 'SaleOrder') {
            o.disabled = false; o.value = '';
            return Promise.resolve();
        }
        // else: GateSale (ItemNameFill + CustomerFill, order no = gate pass no, disabled)
        o.disabled = true; o.value = $('txtgpno').value;
        return Promise.all([SE.api(API + '/items'), SE.api(API + '/customers')]).then(function (r) {
            setItems(r[0], 'Id'); cb.supp.setData(r[1]);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ CmbOrderno_Leave :642 */
    function orderLeave() {
        if (basedText() === '') { cb.based.focus(); return SE.alert('Order Type Field Required'); }
        if (cb.type.text() === '') { cb.type.focus(); return SE.alert('GatePass Type Field Required'); }
        cb.supp.setData([]); cb.item.setData([]);
        var v = basedVal();
        if (v === 'GateSale') return basedOnLeave();                       // the GateSale branch of CmbOrderno_Leave :775
        return SE.api(API + '/order' + SE.q({ basedOn: v, orderNo: $('CmbOrderno').value.trim(), gpDate: $('txtgpdate').value }))
            .then(function (r) {
                S.soId = r.soId || 0;
                if (v === 'SaleOrder') {
                    if (r.items) { setItems(r.items, 'OrderItemId'); selectRow(cb.item, 0); }
                    if (r.customers) { cb.supp.setData(r.customers); selectRow(cb.supp, 0); }
                    if (r.clearCustomer) cb.supp.setData([]);
                    if (r.clearOrderNo) $('CmbOrderno').value = '';
                    return;
                }
                if (r.items) {
                    setItems(r.items, 'ItemId'); cb.item.setValue(r.itemId);
                    cb.wh.setValue(r.warehouseId); $('txtContrainerWeight').value = r.containerWeight; $('txtqty').value = r.qty;
                    $('txtvehicleno').value = r.vehicleNo; cb.veh.setValue(r.vehicleTypeId);
                    if (r.deliveryOrderType === 'Export' && cb.type.text() !== 'Export') {
                        $('CmbOrderno').value = '';
                        return SE.alert('Please Select GatePass Type Export');
                    }
                    cb.supp.setData(r.customers); selectRow(cb.supp, 0);
                    differenceWeight();
                    $('txtContrainerWeight').disabled = true;
                } else {
                    if (r.clearContainerWeight) $('txtContrainerWeight').value = '';
                    if (r.clearFactoryWeight) $('txtFactoryWeight').value = '';
                    if (r.clearCustomer) cb.supp.setData([]);
                    $('txtContrainerWeight').disabled = false;
                    if (r.clearOrderNo) { $('CmbOrderno').value = ''; }
                }
            }).catch(function (e) { cb.supp.setData([]); return SE.alert(e.message, 'Database Error'); });
    }

    /* DifferenceWeight :791 */
    function differenceWeight() {
        var c = $('txtContrainerWeight').value, f = $('txtFactoryWeight').value;
        if (c !== '' && f !== '') $('txtDifferenceWeight').value = String(SE.toNum(c) - SE.toNum(f));
    }
    /* WeightCalculation :2258 (txtqty / txtPackUnit TextChanged) */
    function weightCalculation() {
        if (basedVal() === 'SaleOrder') {
            var q = SE.toNum($('txtqty').value), p = SE.toNum($('txtPackUnit').value);
            $('txtCompareWeight').value = (q > 0 && p > 0) ? String(q * p) : '0';
        } else { $('txtPackUnit').value = '0'; $('txtCompareWeight').value = '0'; }
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        // grd (open gate passes): GetHistoryonlystatusOpen columns, Id / IsApproved hidden, Print and Edit buttons (width 50) at the end
        G.open = SS.autoGrid('grd', { hide: ['Id', 'IsApproved'], sumNone: true, f: {} }, {
            onDbl: function (r) { openEdit(r.Id); },
            onBtn: function (k, r) { if (k === 'Edit') openEdit(r.Id); else if (k === 'Print') print1512(r.Id); }
        });
        G.hist = SS.autoGrid('grdhistory', { hide: ['Id', 'IsApproved', 'RecordNo'], link: ['NoOfAttachments', 'DeliveryOrderNo'], btnFirst: [{ k: 'Print', t: 'Print', w: 40, btn: 'Print' }, { k: 'Edit', t: 'Edit', w: 40, btn: 'Edit' }] }, {
            frozen: 2,
            onDbl: function (r) { histEdit(r.Id); },
            onBtn: function (k, r) {
                if (k === 'Edit') histEdit(r.Id);
                else if (k === 'Print') { if (String(r.IsApproved).toLowerCase() !== 'true') return SE.alert('GatePass Not Approve'); print1512(r.Id); }
            },
            onLink: function (k, r) {
                if (k === 'NoOfAttachments') SS.showAttachments(API, r.Id);
                else if (k === 'DeliveryOrderNo') window.open('/sale/steel/delivery-order', '_blank');      // DeliveryOrder form, ReadById(Cells[2]) - the OrderType text -> 0
            }
        });
        G.doi = SS.autoGrid('grdDoInformation', { hide: ['Id', 'SaleOrderNo', 'DocumentTypeId'], link: ['DoNo'], sum: ['DoQty', 'DoWeight', 'LoadingQty', 'LoadingWeight'],
            f: { DoQty: 'n0', DoWeight: 'n0', LoadingQty: 'n0', LoadingWeight: 'n0' } }, {
            onLink: function (k, r) { if (k === 'DoNo') SE.printRpt('1513-DeliveryOrderSlip.rpt', { id: r.Id, documentTypeId: 1506 }); }      // DeliveryOrderSlipForSteel(Id, 1506)
        });
        G.so = SS.autoGrid('GrdSaleOrderInformation', { hide: ['Id', 'DocumentTypeId'], cap: { ItemQty: 'OrderQty', NetWeight: 'OrderWeight' }, link: ['OrderNo'],
            sum: ['ItemQty', 'NetWeight', 'DispatchQty', 'DispatchWeight', 'BalQty', 'BalWeight'],
            f: { ItemQty: 'n0', NetWeight: 'n0', DispatchQty: 'n0', DispatchWeight: 'n0', BalQty: 'n0', BalWeight: 'n0' } }, {
            onLink: function (k) {
                // GrdSaleOrderInformation_LinkClicked reads the Id of the CURRENT ROW OF grdDoInformation (desktop behaviour kept)
                var cur = G.doi.cur();
                if (!cur) return SE.alert('Object reference not set to an instance of an object.', 'Database Error');
                SE.printRpt('1511-SaleOrderSlipAndRegister.rpt', { id: cur.Id });
            }
        });
    }

    /* ------------------------------------------------------------------ Load :1193 */
    function load() {
        makeCombos();
        tabs1 = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('txtFromDateHistory').focus(); else $('txtgpdate').focus(); });
        tabs2 = SE.tabs('tabControl2');
        SE.digitsOnly($('txtContrainerWeight')); SE.digitsOnly($('txtfreight')); SE.digitsOnly($('txtNetPaid')); SE.digitsOnly($('txtqty'));
        SE.digitsOnly($('CmbOrderno')); SE.digitsOnly($('txtgpno')); SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); SE.upper($('txtvehicleno'));
        makeGrids();
        SE.api(API + '/initial').then(function (d) {
            S.data = d; S.rights = d.rights || {};
            cb.supp.setData(d.customers); cb.hist.setData(d.historyCustomers);
            bindTypes(d.gatePassTypes);
            cb.veh.setData(d.vehicleTypes); cb.wh.setData(d.warehouses); cb.city.setData(d.cities);
            $('txtgpno').value = String(d.nextNo == null ? 0 : d.nextNo);
            G.open.fill(d.openRecords); G.doi.fill(d.doGrid); G.so.fill(d.soGrid);
            $('btnsave').style.display = ''; $('btnupdate').style.display = 'none';
            $('btnsave').disabled = !S.rights.save; $('btnupdate').disabled = !S.rights.update; $('btnPrint').disabled = !S.rights.print;
            $('txtintime').value = nowTime(); $('txtouttime').value = nowTime();
            $('ChkBox').checked = true;
            $('txtgpdate').value = SE.today();
            $('txtDiffWeightRemarks').readOnly = true;
            cb.status.setValue(1); cb.wb.setValue('Auto'); selectRow(cb.based, 0);
            basedOnLeave();
            if (d.cityArea) cb.city.setValue(d.cityArea);
            $('txtFromDateHistory').value = SE.addDays(-d.historyDays); $('txtToDateHistory').value = SE.today();
            cb.based.focus();
        }).catch(fail);
        wire();
        var rec = SE.param('record'); if (rec) setTimeout(function () { edit(+rec); }, 600);
    }
    function nowTime() { var n = new Date(); return SE.pad2(n.getHours()) + ':' + SE.pad2(n.getMinutes()); }
    function timeOf(v) { var p = SE.parts(v); return p ? SE.pad2(p.h) + ':' + SE.pad2(p.i) : null; }
    /* gatepasstype(): BindDDL(..., ZeroIndex:false); Rows[0].Activate() */
    function bindTypes(rows) { cb.type.setData(rows || []); if ((rows || []).length > 0) selectRow(cb.type, 0); }

    /* ------------------------------------------------------------------ Reset :1921 */
    function reset() {
        S.soId = 0; S.id = 0; S.gpId = 0; S.approved = false; S.files = []; S.removed = []; S.existing = [];
        $('txtPackUnit').value = '0'; $('txtCompareWeight').value = '0';
        cb.type.setValue(4);
        $('txtDiffWeightRemarks').value = ''; $('txtApprovedBy').value = ''; $('txtgptypeno').value = ''; $('CmbOrderno').value = '';
        cb.supp.setData([]); cb.veh.clear(); $('txtvehicleno').value = ''; $('txtbiltyno').value = '';
        cb.status.setValue(1); $('txtqty').value = ''; $('txtContrainerWeight').value = '0'; $('txtfreight').value = ''; $('txtFactoryWeight').value = '0';
        $('txtDifferenceWeight').value = '0'; $('txtremarks').value = ''; $('txtWeighBridgeSlipNo').value = '';
        $('txtNetPaid').value = '';
        $('btnsave').style.display = ''; $('btnupdate').style.display = 'none';
        $('txtintime').value = nowTime(); $('txtouttime').value = nowTime();
        $('txtgpdate').focus();
        dis(cb.based, false); dis(cb.type, false); $('CmbOrderno').disabled = false; dis(cb.wb, false);
        cb.item.setData([]); cb.wb.setValue('Auto');
        return SE.api(API + '/reset').then(function (d) {                         // gpnofill + grdfrmfill + the two information grids
            $('txtgpno').value = String(d.nextNo == null ? 0 : d.nextNo);
            G.open.fill(d.openRecords); G.doi.fill(d.doGrid); G.so.fill(d.soGrid);
        }).catch(fail);
    }

    /* btnRefresh_Click :2000 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            setItems(d.items, 'Id'); bindTypes(d.gatePassTypes); cb.veh.setData(d.vehicleTypes); cb.city.setData(d.cities); cb.wh.setData(d.warehouses); G.open.fill(d.openRecords);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ formvalidation :841 */
    function validate() {
        function stop(msg, focus) { return SE.alert(msg).then(function () { if (focus) focus(); return false; }); }
        var t = cb.type.text().trim();
        if (t === '' || t === '0') return stop('Please select GatePass Type', function () { cb.type.focus(); });
        var gp = field('txtgpno');
        if (gp === '0' || gp === '') return stop('Please select Gate Pass No', function () { $('txtgpno').focus(); });
        if (!cb.supp.row()) return stop('Please Select Customer', function () { cb.supp.focus(); });
        if (!cb.veh.row()) return stop('Please Select Vehicle Type', function () { cb.veh.focus(); });
        if (field('txtvehicleno') === '') return stop('Please Select Vehicle No', function () { $('txtvehicleno').focus(); });
        if (!cb.status.row()) return stop('Please Select Status', function () { cb.status.focus(); });
        if (!cb.wb.row()) return stop('Please Select WeighBridgeStatus', function () { cb.wb.focus(); });
        var on = field('CmbOrderno'), bt = basedText();
        if (bt === 'SaleOrder') {
            if (on === '' || on === '0') return stop('Please Enter Order#', function () { $('CmbOrderno').focus(); });
            if (!cb.supp.row()) return stop('Please Select Customer Name', function () { cb.supp.focus(); });
            if (SE.toNum($('txtPackUnit').value) <= 0) return stop('PackUnit Field Required', function () { $('txtPackUnit').focus(); });
            if (SE.toNum($('txtCompareWeight').value) <= 0) return stop('Weight Field Required', function () { $('txtCompareWeight').focus(); });
        } else if (bt === 'DeliveryOrder') {
            if (on === '' || on === '0') return stop('Please Enter Delivery Order #', function () { $('CmbOrderno').focus(); });
            var cw = field('txtContrainerWeight');
            if (cw === '' || cw === '0') return stop('Container Weight field required', function () { $('txtContrainerWeight').focus(); });
        }
        return Promise.resolve(true);
    }

    /* btnsave_Click :1062 / btnupdate_Click :323 */
    function save() {
        var upd = S.id > 0;
        return validate().then(function (ok) {
            if (!ok) return;
            if (bt() === 'DeliveryOrder') differenceWeight();
            if (!upd && cb.status.text() !== 'Open') return SE.alert('Status Not Change', 'Database Error');
            return SE.ask(upd ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    id: S.id, gpDate: $('txtgpdate').value, gpSrNo: SE.toInt($('txtgpno').value), gatepassType: cb.type.text(), gpTypeSrNo: SE.toInt($('txtgptypeno').value),
                    basedOn: basedVal(), basedOnText: basedText(), orderNo: field('CmbOrderno'), supplierCustomerId: +cb.supp.value() || 0, remarks: field('txtremarks'),
                    noOfPackages: SE.toInt($('txtqty').value), vehicleTypeId: +cb.veh.value() || 0, vehicleNo: field('txtvehicleno'), biltyNo: field('txtbiltyno'),
                    freight: SE.toNum($('txtfreight').value), netPaid: SE.toNum($('txtNetPaid').value), inTime: $('txtintime').value, outTime: $('txtouttime').value,
                    cityId: +cb.city.value() || 0, status: cb.status.text(), wbStatus: cb.wb.text(), weighBridgeSlipNo: field('txtWeighBridgeSlipNo'),
                    factoryWeightText: field('txtFactoryWeight'), warehouseId: +cb.wh.value() || 0, varietyName: cb.item.text(),
                    packUnit: SE.toNum($('txtPackUnit').value), compareWeight: SE.toNum($('txtCompareWeight').value),
                    containerWeightText: field('txtContrainerWeight'), differenceWeightText: field('txtDifferenceWeight'), diffWeightRemarks: field('txtDiffWeightRemarks'),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkBox').checked;
                        return reset().then(function () { if (preview) print1512(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }
    function bt() { return basedText(); }

    /* ------------------------------------------------------------------ ReadById :930 + GetTicketNoandWeighBridgeWeight :1701 */
    function openEdit(id) { return edit(id); }
    function histEdit(id) { return reset().then(function () { return edit(id); }); }
    function edit(id) {
        S.id = id; S.gpId = id; cb.status.setValue(2);                          // CmbStatus.Rows[1].Activate()
        return SE.api(API + '/' + id).then(function (d) {
            var r = d.rec; S.soId = d.soId || 0;
            tabs1.select('tabPage1');
            $('txtbiltyno').value = r.BiltyNo || '';
            if (r.CityId) cb.city.setValue(r.CityId);                            // desktop sets the combo text to the id number (no match); the web selects the city
            $('txtfreight').value = r.Freight == null ? '' : r.Freight;
            byText(cb.type, 'GpTypeDescription', r.GatepassType);
            $('txtgpdate').value = SE.dateInput(r.GpDate); $('txtgpno').value = r.GpSrNo; $('txtgptypeno').value = r.GpTypeSrNo == null ? '' : r.GpTypeSrNo;
            if (timeOf(r.InDateTimeStamp)) $('txtintime').value = timeOf(r.InDateTimeStamp);
            if (timeOf(r.OutDateTimeStamp)) $('txtouttime').value = timeOf(r.OutDateTimeStamp);
            $('txtqty').value = r.NoOfPackages == null ? '' : r.NoOfPackages;
            $('txtremarks').value = r.OtherRemarks || '';
            byText(cb.based, 'Type', r.OtherSupCust);                            // CmbGPBasedOn.Text = OtherSupCust (value change -> CmbGPBasedOn_Leave)
            $('txtouttime').value = nowTime();
            $('CmbOrderno').value = r.SupplierContractCode || '';
            $('txtvehicleno').value = r.VehicleNo || '';
            byText(cb.veh, 'VehicleDescription', r.VehicleType);
            if (d.customers) { cb.supp.setData(d.customers); selectRow(cb.supp, 0); }
            var after = d.basedOn === 'GateSale' ? Promise.resolve(d) : Promise.resolve(d);
            return after;
        }).then(function (d) {
            var r = d.rec;
            if (d.customers && d.basedOn === 'GateSale') cb.supp.setData(d.customers);
            cb.supp.setValue(r.SupplierCustomerId);
            byText(cb.status, 'Status', r.Status);
            $('txtNetPaid').value = r.NetPaid == null ? '' : r.NetPaid;
            // CmbVariety.Text = VarietyName (free text on the desktop): a single row carrying that text
            cb.item.setData(r.VarietyName ? [{ Id: 0, ItemName: r.VarietyName }] : []); if (r.VarietyName) selectRow(cb.item, 0);
            $('txtContrainerWeight').value = r.SupplierWeight == null ? '' : r.SupplierWeight;
            $('txtWeighBridgeSlipNo').value = r.WeighBridgeId == null ? '' : r.WeighBridgeId;
            $('txtFactoryWeight').value = r.FactoryWeight == null ? '' : r.FactoryWeight;
            if (r.WarehouseId) cb.wh.setValue(r.WarehouseId);                    // desktop sets the combo text to the id number (no match); the web selects the warehouse
            $('txtApprovedBy').value = r.ApprovedUserName || '';
            byText(cb.wb, 'Status', r.DocAttachment);
            $('txtDiffWeightRemarks').value = r.WeightDiffRemarks == null ? '' : r.WeightDiffRemarks;
            $('txtPackUnit').value = r.PackUnit == null ? '' : r.PackUnit;
            $('txtCompareWeight').value = r.WeightCommapredToSoWt == null ? '' : r.WeightCommapredToSoWt;
            S.approved = !!r.IsApproved;
            dis(cb.based, true); dis(cb.type, true); $('CmbOrderno').disabled = true;
            $('txtDiffWeightRemarks').readOnly = false;
            differenceWeight();
            if (SE.toNum($('txtFactoryWeight').value) > 0) { dis(cb.wb, true); cb.status.setValue(2); } else dis(cb.wb, false);
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            $('btnupdate').style.display = ''; $('btnsave').style.display = 'none';
            // GetTicketNoandWeighBridgeWeight
            var wb = d.wb || {};
            if (wb.ticketNo !== '0') { $('txtWeighBridgeSlipNo').value = wb.ticketNo; $('txtFactoryWeight').value = wb.netWeight; if ($('txtFactoryWeight').value !== '' && $('txtFactoryWeight').value !== '0') dis(cb.wb, true); differenceWeight(); }
            else { $('txtWeighBridgeSlipNo').value = '0'; $('txtFactoryWeight').value = '0'; }
            if (SE.toNum($('txtFactoryWeight').value) > 0) { dis(cb.wb, true); cb.status.setValue(2); } else dis(cb.wb, false);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history (gridhistory :1253) */
    function showHistory() {
        var q = { dateType: $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : 'document'),
                  fromDate: $('txtFromDateHistory_chk').checked ? $('txtFromDateHistory').value : '', toDate: $('txtToDateHistory_chk').checked ? $('txtToDateHistory').value : '',
                  fromNo: SE.toInt($('FromDocNoHistory').value), toNo: SE.toInt($('ToDocNoHistory').value), customerId: +cb.hist.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (g) { G.hist.fill(g); }).catch(fail);
    }
    function resetHistory() {                                                       // Resethistory :1437
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = ''; cb.hist.clear(); $('txtFromDateHistory').value = SE.addDays(-3); $('txtFromDateHistory').focus();
    }
    function refreshHistoryCombo() { return SE.api(API + '/initial').then(function (d) { cb.hist.setData(d.historyCustomers); }).catch(fail); }

    /* ------------------------------------------------------------------ print */
    function print1512(id) {
        if (!(id > 0)) return SE.alert('No Record Found For Display');
        SE.printRpt('1512-GatePassOutwardSlipAndRegisterSteel.rpt', { id: id });
    }
    function printButton() {                                                        // btnPrint_Click :2160 / btnPrint291_Click :2176
        if (!S.approved) return SE.alert('GatePass Not Approve');
        print1512(S.id);
    }

    /* ------------------------------------------------------------------ shortcut keys (OutwardGatePass_KeyDown :2077) */
    function onKey(e) {
        SS.enterTab(e);
        if (e.ctrlKey && (e.key === 't' || e.key === 'T')) { e.preventDefault(); if (tabs1.index() === 1) { tabs1.select('tabPage1'); $('txtgpdate').focus(); } else { tabs1.select('tabPage2'); $('txtFromDateHistory').focus(); } return; }
        if ((e.ctrlKey && (e.key === 'e' || e.key === 'E')) || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); return; }
        if (tabs1.index() === 0) {
            if (e.ctrlKey && (e.key === 's' || e.key === 'S')) { e.preventDefault(); if (SS.visible('btnsave')) save(); }
            else if (e.ctrlKey && (e.key === 'u' || e.key === 'U')) { e.preventDefault(); if (SS.visible('btnupdate')) upd(); }
            else if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); reset(); }
            else if (e.ctrlKey && (e.key === 'p' || e.key === 'P')) { e.preventDefault(); if (SS.visible('btnPrint')) printButton(); }
            else if (e.altKey && (e.key === 'r' || e.key === 'R')) { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'Enter') { var c = G.open.cur(); if (c) { e.preventDefault(); openEdit(c.Id); } }
        } else if (tabs1.index() === 1) {
            if (e.ctrlKey && (e.key === 'l' || e.key === 'L')) { e.preventDefault(); showHistory(); }
        }
    }
    function upd() { if (S.id === 0) return SE.alert('Record not update because Id not found', 'Database Error'); return save(); }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = save;
        $('btnupdate').onclick = upd;
        $('btnAttachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('btnPrint').onclick = printButton; $('btnPrint295').onclick = printButton;
        $('btndefvehicle').onclick = function () { SS.defineVehicle(API, cb.veh); };
        $('btnNewHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombo; $('btnShowHistory').onclick = showHistory;
        $('CmbOrderno').addEventListener('blur', function () { if (!$('CmbOrderno').disabled) orderLeave(); });
        $('txtqty').addEventListener('input', weightCalculation); $('txtPackUnit').addEventListener('input', weightCalculation);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        $('btnDelete').onclick = function () { };                                  // btnDelete_Click :2300 is empty
    }

    document.addEventListener('DOMContentLoaded', load);
})();
