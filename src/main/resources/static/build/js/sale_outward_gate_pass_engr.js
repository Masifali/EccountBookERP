/*
 * Screen 534  OutwardGatePassTrading  (Architecture.WinApp.SaleTrading.OutwardGatePassTrading)
 * Page script. Desktop methods are named in the comments (OutwardGatePassTrading.cs). Server: /sale/engr/outward-gate-pass/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/outward-gate-pass/api';
    var S = { id: 0, soId: 0, gpId: 0, approved: false, data: {}, rights: {}, enableSO: false, enableDO: false,
              files: [], removed: [], existing: [], supplierRows: [] };
    var cb = {}, G = {}, tabs1, tabs2;
    var STATUS = [{ Id: 1, Status: 'Open' }, { Id: 2, Status: 'Accepted' }, { Id: 3, Status: 'Rejected' }];
    var WBS = [{ Id: 1, Status: 'Auto' }, { Id: 2, Status: 'Mannual' }];

    function byText(c, key, text) {
        var rows = c.rows(); for (var i = 0; i < rows.length; i++) if (String(rows[i][key]) === String(text)) { c.setValue(rows[i][c.el.getAttribute('data-vk') || 'Id']); return true; }
        c.clear(); return false;
    }
    function fail(e) { return SE.dbError(e); }
    function dis(c, on) { c.el.disabled = on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.status = XCombo('CmbStatus', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.wb = XCombo('CmbWeighBridgeStatus', { columns: [{ key: 'Status', caption: 'Status' }], textKey: 'Status' });
        cb.based = XCombo('CmbGPBasedOn', { columns: [{ key: 'OrderType', caption: 'OrderType' }], textKey: 'OrderType', onLeave: basedOnLeave });
        cb.type = XCombo('cmbgptype', { columns: [{ key: 'GpTypeDescription', caption: 'Gate Pass Type' }], textKey: 'GpTypeDescription', onLeave: typeLeave });
        cb.veh = XCombo('cmbvehicletype', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], textKey: 'VehicleDescription' });
        cb.city = XCombo('cmbcity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName' });
        cb.supp = XCombo('cmbsupp', { columns: [{ key: 'CompanyName', caption: 'Supplier Name' }], textKey: 'CompanyName', popupWidth: 420 });
        cb.hist = XCombo('CmbCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420 });
        cb.status.setData(STATUS); cb.wb.setData(WBS);
    }
    function selectRow(c, i) { var r = c.rows()[i]; if (r) c.setValue(r[c.el.getAttribute('data-vk') || 'Id']); }

    /* gpnofill */
    function setGpNo(n) { $('txtgpno').value = String(n == null ? 0 : n); }

    /* gatepasstype(): BindDDL(..., ZeroIndex:false); Rows[1].Activate() */
    function bindTypes(rows) { cb.type.setData(rows || []); if ((rows || []).length > 1) selectRow(cb.type, 1); }

    /* cmbgptype_Leave_1 */
    function typeLeave() {
        $('CmbOrderno').value = '';
        var t = cb.type.text();
        return SE.api(API + '/type-code' + SE.q({ type: t }), { quiet: false }).then(function (r) {
            if (r.code > 0) { $('txtgptypeno').value = String(r.code); return; }
            return SE.alert('Please GatePass Type Select').then(function () { cb.type.focus(); });
        }).catch(fail);
    }

    /* CmbGPBasedOn_Leave */
    function basedOnLeave() {
        var id = +cb.based.value() || 0;
        var o = $('CmbOrderno');
        if (id === 3) { o.disabled = true; return SE.api(API + '/customers?orderTypeId=3').then(function (rows) { cb.supp.setData(rows); }).catch(fail); }
        if (cb.based.text() === 'Purchase Return') { o.disabled = true; o.value = $('txtgpno').value; return orderLeave(); }
        o.disabled = false;
    }

    /* CmbOrderno_Leave */
    function orderLeave() {
        var id = +cb.based.value() || 0;
        if (cb.based.text() === '') { cb.based.focus(); return SE.alert('Order Type Field Required'); }
        if (cb.type.text() === '') { cb.type.focus(); return SE.alert('GatePass Type Field Required'); }
        if (id !== 1605 && id !== 1606 && id !== 59) return Promise.resolve();
        return SE.api(API + '/order' + SE.q({ orderTypeId: id, orderNo: $('CmbOrderno').value.trim(), gpDate: $('txtgpdate').value, gatePassType: cb.type.text() }))
            .then(function (r) {
                S.soId = r.soId || 0;
                if (r.qty !== undefined) $('txtqty').value = r.qty;
                if (r.vehicleNo !== undefined) $('txtvehicleno').value = r.vehicleNo;
                if (r.vehicleTypeText !== undefined) byText(cb.veh, 'VehicleDescription', r.vehicleTypeText);
                if (r.clearFactoryWeight) $('txtFactoryWeight').value = '';
                if (r.customers) { cb.supp.setData(r.customers); if (r.selectFirst) selectRow(cb.supp, 0); }
                if (r.clearCustomer) cb.supp.setData([]);
                if (r.clearOrderNo) $('CmbOrderno').value = '';
                if (r.message) return SE.alert(r.message);
            }).catch(function (e) { cb.supp.setData([]); return SE.alert(e.message, 'Database Error'); });
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        var dec = S.data.amountDecimals || 0;
        G.open = SE.grid('grd', { frozen: 2, dec: dec, onDbl: function (r) { editOpen(r, false); },
            onBtn: function (k, r) { if (k === 'Edit') editOpen(r, false); else if (k === 'Print') print295(r.Id); },
            cols: [
                { k: 'Print', t: 'Print', w: 40, btn: 'Print' }, { k: 'Edit', t: 'Edit', w: 40, btn: 'Edit' }, { k: 'Id', hide: true },
                { k: 'GpDate', t: 'GpDate', w: 80, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'GatepassType', t: 'GatepassType', w: 100 },
                { k: 'OrderType', t: 'OrderType', w: 100 }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'Customer', t: 'Customer', w: 220 },
                { k: 'Qty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'VehicleType', t: 'VehicleType', w: 100 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
                { k: 'BiltyNo', t: 'BiltyNo', w: 80 }, { k: 'Status', t: 'Status', w: 70 }, { k: 'CityName', t: 'CityName', w: 100 },
                { k: 'FactoryWeight', t: 'FactoryWeight', w: 100, f: 'n3', sum: true }, { k: 'ApprovedBy', t: 'ApprovedBy', w: 110 }, { k: 'Remaks', t: 'Remaks', w: 200 }] });
        G.hist = SE.grid('grdhistory', { frozen: 3, dec: dec, onDbl: function (r) { editHist(r); },
            onBtn: function (k, r) {
                if (k === 'Edit') editHist(r); else if (k === 'Print') print295(r.Id); else if (k === 'Print290') print290(r.Id);
            },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(r.Id); },
            cols: [
                { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'Print290', t: 'Print290', w: 60, btn: 'Print290' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' },
                { k: 'Id', hide: true }, { k: 'GatepassType', t: 'GatepassType', w: 100 }, { k: 'OrderType', t: 'OrderType', w: 100 },
                { k: 'GpSrNo', t: 'GpSrNo', w: 60 }, { k: 'GpDate', t: 'GpDate', w: 90, f: 'date' }, { k: 'DeliveryOrderNo', t: 'DeliveryOrderNo', w: 100 },
                { k: 'OrderNo', t: 'OrderNo', w: 80 }, { k: 'CustomerName', t: 'CustomerName', w: 220 }, { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3', sum: true },
                { k: 'VehicleType', t: 'VehicleType', w: 100 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'BiltyNo', t: 'BiltyNo', w: 80 },
                { k: 'InTime', t: 'InTime', w: 140, f: 'dt' }, { k: 'OutTime', t: 'OutTime', w: 140, f: 'dt' }, { k: 'CityName', t: 'CityName', w: 100 },
                { k: 'Freight', t: 'Freight', w: 90, f: 'amt', sum: true }, { k: 'NetPaid', t: 'NetPaid', w: 90, f: 'amt', sum: true }, { k: 'Status', t: 'Status', w: 70 },
                { k: 'SupplierWeight', t: 'SupplierWeight', w: 100, f: 'n3', sum: true }, { k: 'FactoryWeight', t: 'FactoryWeight', w: 100, f: 'n3', sum: true },
                { k: 'DifferenceWeight', t: 'DifferenceWeight', w: 110, f: 'n3', sum: true }, { k: 'WeighBridgeStatus', t: 'WeighBridgeStatus', w: 110 },
                { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'IsApproved', hide: true }, { k: 'OtherRemarks', t: 'OtherRemarks', w: 200 },
                { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.doi = SE.grid('grdDoInformation', { dec: dec, onLink: function (k, r) { if (k === 'DoNo') SE.printRpt('1606-DeliveryOrderSlip_Engr.rpt', { id: r.Id, documentTypeId: r.DocumentTypeId }); },
            cols: [{ k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'date' }, { k: 'DoNo', t: 'DoNo', w: 70, link: true },
                { k: 'SONo', t: 'SONo', w: 70 }, { k: 'DeliveryOrderType', t: 'DeliveryOrderType', w: 150 }, { k: 'CustomerName', t: 'CustomerName', w: 300 },
                { k: 'DoQty', t: 'DoQty', w: 90, f: 'n3', sum: true }, { k: 'RequestedBy', t: 'RequestedBy', w: 110 }, { k: 'ApprovedBy', t: 'ApprovedBy', w: 110 },
                { k: 'Remarks', t: 'Remarks', w: 300 }, { k: 'AccountRemarks', t: 'AccountRemarks', w: 300 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' },
                { k: 'EntryUserName', t: 'EntryUserName', w: 150 }] });
        G.so = SE.grid('GrdSaleOrderInformation', { dec: dec, onLink: function () { },
            cols: [{ k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'date' }, { k: 'OrderNo', t: 'OrderNo', w: 70 },
                { k: 'CustomerName', t: 'CustomerName', w: 300 }, { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n3', sum: true }, { k: 'DispatchQty', t: 'DispatchQty', w: 90, f: 'n3', sum: true },
                { k: 'BalQty', t: 'BalQty', w: 90, f: 'n3', sum: true }, { k: 'OrderStatus', t: 'OrderStatus', w: 100 }, { k: 'OrderRemarks', t: 'OrderRemarks', w: 300 },
                { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 150 }] });
    }

    /* tabControl2: Open Gatepass | Sale Order Detail (if EnableSaleOrderFlow) | Delivery Order Detail (if EnableDeliveryOrderFlow) - SO is added first */
    function setInfoTabs() {
        var hdr = $('tabControl2').querySelector(':scope > .dsubtabs');
        var so = hdr.querySelector('[data-tab="tabSoInformation"]'), dO = hdr.querySelector('[data-tab="tabDoInformation"]');
        hdr.appendChild(so); hdr.appendChild(dO);
        tabs2.show('tabSoInformation', S.enableSO); tabs2.show('tabDoInformation', S.enableDO);
        tabs2.select('tabPage3');
    }
    function loadInfoGrids() {
        var p = [];
        if (S.enableSO) p.push(SE.api(API + '/so-rows').then(function (r) { G.so.setRows(r); }));
        if (S.enableDO) p.push(SE.api(API + '/do-rows').then(function (r) { G.doi.setRows(r); }));
        return Promise.all(p);
    }

    /* ------------------------------------------------------------------ Load */
    function load() {
        makeCombos();
        tabs1 = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') { setFromDate(-3); $('txtFromdateHistory').focus(); } else $('txtgpdate').focus(); });
        tabs2 = SE.tabs('tabControl2');
        SE.digitsOnly($('txtqty')); SE.digitsOnly($('txtfreight')); SE.digitsOnly($('txtNetPaid')); SE.digitsOnly($('CmbOrderno'));
        SE.digitsOnly($('txtFromNoHistory')); SE.digitsOnly($('txtToDocNoHistory')); SE.upper($('txtvehicleno'));
        SE.api(API + '/initial').then(function (d) {
            S.data = d; S.rights = d.rights || {}; S.enableSO = !!d.enableSO; S.enableDO = !!d.enableDO;
            makeGrids();                                                   // amount decimals are known now
            cb.supp.setData(d.customers); cb.hist.setData(d.historyCustomers);
            cb.based.setData(d.orderTypes); if (d.orderTypes.length) selectRow(cb.based, 0);
            bindTypes(d.gatePassTypes);
            cb.veh.setData(d.vehicleTypes); cb.city.setData(d.cities);
            setGpNo(d.nextNo);
            G.open.setRows(d.openRecords);
            $('btnsave').style.display = ''; $('btnupdate').style.display = 'none';
            $('btnsave').disabled = !S.rights.save; $('btnupdate').disabled = !S.rights.update;
            $('btnPrint').disabled = !S.rights.print;
            $('txtintime').value = SE.nowInput(); $('txtouttime').value = SE.nowInput();
            $('ChkBox').checked = true;
            $('txtgpdate').value = SE.today(); setFromDate(-3); $('txtToDateHistory').value = SE.today();
            cb.status.setValue(1); cb.wb.setValue(1);
            if (d.cityArea) cb.city.setValue(d.cityArea);
            setInfoTabs(); G.so.setRows(d.soRows); G.doi.setRows(d.doRows);
            var chain = $('cmbgptype') ? typeLeave() : null;
            return chain;
        }).catch(fail);
        wire();
        var rec = SE.param('record'); if (rec) setTimeout(function () { edit(+rec, true); }, 600);
    }
    function setFromDate(days) { $('txtFromdateHistory').value = SE.addDays(days); }

    /* ------------------------------------------------------------------ Reset (btnnew_Click) */
    function reset() {
        S.soId = 0; S.id = 0; S.gpId = 0; S.approved = false; S.files = []; S.removed = []; S.existing = [];
        $('txtApprovedBy').value = ''; $('txtgptypeno').value = ''; $('CmbOrderno').value = '';
        cb.supp.setData([]); cb.veh.clear(); $('txtvehicleno').value = ''; $('txtbiltyno').value = '';
        cb.status.setValue(1); $('txtqty').value = ''; $('txtfreight').value = ''; $('txtFactoryWeight').value = '0'; $('txtremarks').value = ''; $('txtWeighBridgeSlipNo').value = '';
        $('txtNetPaid').value = '';
        $('btnsave').style.display = ''; $('btnupdate').style.display = 'none';
        $('txtintime').value = SE.nowInput(); $('txtouttime').value = SE.nowInput();
        $('txtgpdate').focus();
        dis(cb.based, false); dis(cb.type, false); $('CmbOrderno').disabled = false; dis(cb.wb, false);
        cb.wb.setValue(1);
        setInfoTabs();
        return SE.api(API + '/initial').then(function (d) {                       // gpnofill + grdfrmfill + the two information grids
            setGpNo(d.nextNo); G.open.setRows(d.openRecords); G.so.setRows(d.soRows); G.doi.setRows(d.doRows);
        }).then(basedOnLeave).catch(fail);
    }

    /* btnRefresh_Click: gatepasstype(); vehicleTypefill(); CityFill(); grdfrmfill(); */
    function refresh() {
        return SE.api(API + '/initial').then(function (d) { bindTypes(d.gatePassTypes); cb.veh.setData(d.vehicleTypes); cb.city.setData(d.cities); G.open.setRows(d.openRecords); }).catch(fail);
    }

    /* ------------------------------------------------------------------ formvalidation + Insert */
    function field(id) { return ($(id).value || '').trim(); }
    function validate() {
        function stop(msg, focus) { return SE.alert(msg).then(function () { if (focus) focus(); return false; }); }
        var t = cb.type.text().trim();
        if (t === '' || t === '0') return stop('Please select GatePass Type', function () { cb.type.focus(); });
        var gp = field('txtgpno');
        if (gp === '0' || gp === '') return stop('Please select Gate Pass No', function () { $('txtgpno').focus(); });
        if (field('txtvehicleno') === '') return stop('Please Select Vehicle No', function () { $('txtvehicleno').focus(); });
        if (!(+cb.veh.value())) return stop('Please Select Vehicle Type', function () { cb.veh.focus(); });
        if (!(+cb.status.value())) return stop('Please Select Status', function () { cb.status.focus(); });
        if (!(+cb.wb.value())) return stop('Please Select WeighBridgeStatus', function () { cb.wb.focus(); });
        var on = field('CmbOrderno');
        if (cb.based.text() === 'SaleOrder') {
            if (on === '' || on === '0') return stop('Please Enter Order#', function () { $('CmbOrderno').focus(); });
            if (!cb.supp.row()) return stop('Please Select Customer Name', function () { cb.supp.focus(); });
        } else if ((+cb.based.value() || 0) !== 3 && (on === '' || on === '0')) {
            return stop('Please Enter Delivery Order #', function () { $('CmbOrderno').focus(); });
        }
        return Promise.resolve(true);
    }
    function save() {
        return validate().then(function (ok) {
            if (!ok) return;
            if (cb.status.text() !== 'Open' && S.id === 0) { cb.status.focus(); return SE.alert('Status should be Open in Case of Save', 'Database Error'); }
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    id: S.id, gpDate: $('txtgpdate').value, gpSrNo: SE.toInt($('txtgpno').value), gatepassType: cb.type.text(), gpTypeSrNo: SE.toInt($('txtgptypeno').value),
                    orderTypeId: +cb.based.value() || 0, orderNo: field('CmbOrderno'), supplierCustomerId: +cb.supp.value() || 0, remarks: field('txtremarks'),
                    noOfPackages: SE.toInt($('txtqty').value), vehicleTypeId: +cb.veh.value() || 0, vehicleNo: field('txtvehicleno'), biltyNo: field('txtbiltyno'),
                    freight: SE.toNum($('txtfreight').value), netPaid: SE.toNum($('txtNetPaid').value), inTime: $('txtintime').value, outTime: $('txtouttime').value,
                    cityId: +cb.city.value() || 0, statusId: +cb.status.value() || 0, wbStatusId: +cb.wb.value() || 0, weighBridgeSlipNo: field('txtWeighBridgeSlipNo'),
                    factoryWeight: SE.toNum($('txtFactoryWeight').value),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkBox').checked;
                        return reset().then(function () { if (preview) print295(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }

    /* ------------------------------------------------------------------ ReadById + GetTicketNoandWeighBridgeWeight */
    function edit(id, resetFirst) {
        var p = resetFirst ? reset() : Promise.resolve();
        return p.then(function () {
            S.id = id; cb.status.setValue(2); S.gpId = id;
            return SE.api(API + '/' + id);
        }).then(function (d) {
            var r = d.rec; S.soId = d.soId || 0;
            tabs1.select('tabPage1');
            $('txtbiltyno').value = r.BiltyNo || '';
            $('txtfreight').value = r.Freight == null ? '' : r.Freight;
            byText(cb.type, 'GpTypeDescription', r.GatepassType);
            $('txtgpdate').value = SE.dateInput(r.GpDate); $('txtgpno').value = r.GpSrNo; $('txtgptypeno').value = r.GpTypeSrNo;
            if (SE.dtInput(r.InDateTimeStamp)) $('txtintime').value = SE.dtInput(r.InDateTimeStamp);
            $('txtqty').value = r.NoOfPackages == null ? '' : r.NoOfPackages;
            $('txtremarks').value = r.OtherRemarks || '';
            byText(cb.based, 'OrderType', r.OtherSupCust);
            $('txtouttime').value = SE.nowInput();
            $('CmbOrderno').value = r.SupplierContractCode || '';
            $('txtvehicleno').value = r.VehicleNo || '';
            byText(cb.veh, 'VehicleDescription', r.VehicleType);
            if (d.customers) { cb.supp.setData(d.customers); if (d.orderTypeId !== 3) selectRow(cb.supp, 0); }
            if (d.orderTypeId === 3) { $('CmbOrderno').value = ''; }
            cb.supp.setValue(r.SupplierCustomerId);
            byText(cb.status, 'Status', r.Status);
            if (r.CityId) cb.city.setValue(r.CityId);          // desktop sets the combo text to the id number (no match); the web selects the city
            $('txtNetPaid').value = r.NetPaid == null ? '' : r.NetPaid;
            $('txtWeighBridgeSlipNo').value = r.WeighBridgeId == null ? '' : r.WeighBridgeId;
            $('txtFactoryWeight').value = r.FactoryWeight == null ? '' : r.FactoryWeight;
            $('txtApprovedBy').value = r.ApprovedUserName || '';
            byText(cb.wb, 'Status', r.DocAttachment);
            S.approved = !!r.IsApproved;
            dis(cb.type, true);
            if (SE.toNum($('txtFactoryWeight').value) > 0) { dis(cb.wb, true); cb.status.setValue(2); } else dis(cb.wb, false);
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            $('btnupdate').style.display = ''; $('btnsave').style.display = 'none';
            // GetTicketNoandWeighBridgeWeight()
            var wb = d.wb || {};
            $('txtWeighBridgeSlipNo').value = wb.ticketNo == null ? '0' : wb.ticketNo;
            $('txtFactoryWeight').value = wb.netWeight == null ? '0' : wb.netWeight;
            if (SE.toNum($('txtFactoryWeight').value) > 0) { dis(cb.wb, true); cb.status.setValue(2); } else dis(cb.wb, false);
            if (d.orderTypeId === 3) { $('CmbOrderno').disabled = true; } else if (cb.based.text() === 'Purchase Return') { $('CmbOrderno').disabled = true; } else $('CmbOrderno').disabled = false;
        }).catch(fail);
    }
    function editOpen(r, doReset) { return edit(r.Id, doReset); }
    function editHist(r) { return edit(r.Id, true); }

    /* ------------------------------------------------------------------ history (gridhistory) */
    function showHistory() {
        var q = { dateType: $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : 'document'),
                  fromDate: $('txtFromdateHistory_chk').checked ? $('txtFromdateHistory').value : '', toDate: $('txtToDateHistory_chk').checked ? $('txtToDateHistory').value : '',
                  fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value), customerId: +cb.hist.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) { G.hist.setRows(rows); }).catch(fail);
    }
    function resetHistory() {
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = ''; cb.hist.clear(); setFromDate(-3); $('txtFromdateHistory').focus();
    }
    function refreshHistoryCombo() { return SE.api(API + '/initial').then(function (d) { cb.hist.setData(d.historyCustomers); }).catch(fail); }

    /* ------------------------------------------------------------------ print */
    function print290(id) {
        if (!(id > 0)) return SE.alert('Record Not Found For Display');
        SE.printRpt('290-InvRptOutwardGatePassSlip.rpt', { id: id, object: id });
    }
    function print295(id) {
        if (!(id > 0)) return SE.alert('PrintId not found...');
        SE.printRpt('295-InvRptOutwardGatePassSlipWithItems.rpt', { id: id });
    }

    /* ------------------------------------------------------------------ attachments (AT.Show) */
    function showAttachments(gpId) {
        var url = API + '/' + gpId + '/attachments';
        return SE.api(url).then(function (rows) { renderAttachments(rows, gpId, true); }).catch(fail);
    }
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
            var rm = e.target.getAttribute && e.target.getAttribute('data-rm'); var nf = e.target.getAttribute && e.target.getAttribute('data-nf');
            if (rm) { S.removed.push(+rm); pop.close(); openAttachmentDialog(); }
            if (nf !== null && nf !== undefined && e.target.hasAttribute('data-nf')) { S.files.splice(+nf, 1); pop.close(); openAttachmentDialog(); }
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

    /* ------------------------------------------------------------------ Define Vehicle (VehicleType form) */
    function defineVehicle() {
        var h = '<div style="padding:6px">Vehicle Description <input id="vtDesc" class="f" style="position:static;width:260px;height:23px"> <span class="se-note">Record Save / Update as the VehicleType form</span></div><div class="dgrid" id="vtGrid" style="max-height:40vh;height:260px"></div>';
        var editId = 0, grid;
        var pop = SE.pop('Define Vehicle', h, [
            { t: 'New', fn: function () { editId = 0; pop.body.querySelector('#vtDesc').value = ''; pop.body.querySelector('#vtDesc').focus(); } },
            { t: 'Save', fn: function () {
                var desc = pop.body.querySelector('#vtDesc').value.trim(); if (!desc) return;
                SE.ask(editId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                    if (!yes) return;
                    SE.api(API + '/vehicle-type', { method: 'POST', body: { id: editId, description: desc } }).then(function (r) {
                        SE.alert(editId > 0 ? 'Record Update Successfully' : 'Record Save Successfully'); editId = 0; pop.body.querySelector('#vtDesc').value = '';
                        grid.setRows(r.rows); cb.veh.setData(r.rows);
                    }).catch(fail);
                });
            } }, { t: 'Close' }]);
        pop.open();
        grid = SE.grid(pop.body.querySelector('#vtGrid'), { footer: false, onDbl: function (r) { editId = r.Id; pop.body.querySelector('#vtDesc').value = r.VehicleDescription; },
            cols: [{ k: 'Id', hide: true }, { k: 'VehicleDescription', t: 'VehicleType', w: 300 }] });
        grid.setRows(cb.veh.rows());
    }

    /* ------------------------------------------------------------------ shortcut keys (MakeShortCutKeys / OutwardGatePass_KeyDown) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Customer'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORT); return; }
        if (e.ctrlKey && (e.key === 't' || e.key === 'T')) { e.preventDefault(); if (tabs1.index() === 1) { tabs1.select('tabPage1'); $('txtgpdate').focus(); } else { tabs1.select('tabPage2'); $('txtFromdateHistory').focus(); } return; }
        if (tabs1.index() === 0) {
            if (e.ctrlKey && (e.key === 's' || e.key === 'S')) { e.preventDefault(); if (visible('btnsave')) save(); }
            else if (e.ctrlKey && (e.key === 'u' || e.key === 'U')) { e.preventDefault(); if (visible('btnupdate')) save(); }
            else if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); reset(); }
            else if (e.ctrlKey && (e.key === 'p' || e.key === 'P')) { e.preventDefault(); if (visible('btnPrint')) print290(S.id); }
            else if (e.altKey && (e.key === 'r' || e.key === 'R')) { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.supp.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
        } else if (tabs1.index() === 1) {
            if (e.ctrlKey && (e.key === 's' || e.key === 'S')) { e.preventDefault(); showHistory(); }
            else if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); resetHistory(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('txtFromdateHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdhistory').focus(); }
        }
    }
    function visible(id) { var b = $(id); return b && b.style.display !== 'none' && !b.disabled; }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = save;
        $('btnupdate').onclick = function () { if (S.id === 0) return SE.alert('Record not update because Id not found', 'Database Error'); save(); };
        $('btnAttachment').onclick = openAttachmentDialog;
        $('btnPrint').onclick = function () { print290(S.id); };
        $('btnPrint295').onclick = function () { print295(S.id); };
        $('btndefvehicle').onclick = defineVehicle;
        $('btnShortCut').onclick = function () { SE.shortcuts(SHORT); };
        $('BtnNewHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombo; $('btnshow').onclick = showHistory;
        $('CmbOrderno').addEventListener('blur', function () { orderLeave(); });
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        $('txtvehicleno').addEventListener('input', function () { /* ToUpper */ });
        ['txtgpdate', 'txtintime', 'txtouttime'].forEach(function (id) { $(id).addEventListener('keydown', function () { }); });
        if (!document.getElementById('btnDelete')) return;
        $('btnDelete').onclick = function () { };
    }

    document.addEventListener('DOMContentLoaded', load);
})();
