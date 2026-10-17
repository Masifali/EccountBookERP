/* ============================================================================================
 * Screen 928 "Outward GatePass" — Architecture.WinApp.ReturnableInventoryManagement.OutwardGatePassReturnable
 * (base.Name "OutwardGatePassReturnable"), DocumentTypeId 1201, RefDocumentTypeId 1200.
 *
 *   InitializeComponentMethod :327   rights, combos, GenerategpCode, open passes, pending DOs    (init)
 *   cmbgptype_Leave_1 :430           GenerateGPTypeCode → txtgptypeno
 *   CmbOrderno_Leave :490            DeliveryOrderNoCheckinGatePass → Qty, Vehicle, Customer list
 *   Insert :765 / btnsave :752 / btnupdate :839 / ReadById :855 / Reset :1430 / btnPrint :1580
 *   gridhistory :994 / grdhistory buttons Print, Edit :1146 / grd (open) buttons Print, Edit :1302
 *   OutwardGatePass_KeyDown :1684 / MakeShortCutKeys :1791
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var api = '/inventory/returnable/outward-gate-pass-returnable/api';
    var RPT = '1201_OutwardGatePassReturnable_Slip.rpt';

    var look = { rights: {} };
    var id = 0, soId = 0, approve = false, updateMode = false;
    var hist = [], open = [], dos = [];

    function num(v) { var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim(); if (!s) return 0; var n = Number(s); return isNaN(n) ? 0 : n; }
    function fmt(v, p) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: p }); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function val(k) { return parseInt($id(k).value || '0', 10) || 0; }
    function text(k) { var el = $id(k); return el.selectedIndex >= 0 && el.options.length ? el.options[el.selectedIndex].text : ''; }
    function addDays(d, n) { var x = new Date(d.getFullYear(), d.getMonth(), d.getDate() + n); return x.getFullYear() + '-' + String(x.getMonth() + 1).padStart(2, '0') + '-' + String(x.getDate()).padStart(2, '0'); }
    function nowLocal() { var d = new Date(), p = function (n) { return String(n).padStart(2, '0'); }; return d.getFullYear() + '-' + p(d.getMonth() + 1) + '-' + p(d.getDate()) + 'T' + p(d.getHours()) + ':' + p(d.getMinutes()); }
    function show(k, on) { $id(k).classList.toggle('is-hidden', !on); }

    function bind(k, list, idKey, nameKey, defaultRow, extra) {
        var el = $id(k), prev = el.value;
        var h = defaultRow === null ? '' : '<option value="0">' + esc(defaultRow || '') + '</option>';
        (list || []).forEach(function (r) {
            var t = esc(r[nameKey]);
            if (extra) extra.forEach(function (x) { if (r[x] !== undefined && r[x] !== null && String(r[x]) !== '') t += ' | ' + esc(r[x]); });
            h += '<option value="' + esc(r[idKey]) + '">' + t + '</option>';
        });
        el.innerHTML = h;
        el.value = (list || []).some(function (r) { return String(r[idKey]) === prev; }) ? prev : (defaultRow === null && list && list.length ? String(list[0][idKey]) : '0');
    }

    // ------------------------------------------------------------------------------ load

    function init() {
        $id('txtgpdate').value = C.today();
        $id('txtintime').value = nowLocal(); $id('txtouttime').value = nowLocal();
        C.getJson(api + '/lookups').then(function (d) {
            look = d;
            var r = d.rights || {};
            $id('btnsave').disabled = !r.save;                                 // :360-363
            $id('btnupdate').disabled = !r.update;
            $id('btnPrint').disabled = !r.print;
            if (d.gpNo > 0) $id('txtgpno').value = d.gpNo;
            bind('CmbGPBasedOn', d.orderTypes, 'Id', 'Name', '');              // :482
            applyLookups(d);
            bind('CmbStatus', d.statuses, 'Id', 'Name', '');                   // :725
            $id('CmbStatus').selectedIndex = 1;                                 // Rows[0].Activate() → "Open"
            renderOpen(d.openGatePasses || []);
            renderDos(d.pendingDeliveryOrders || []);
            bind('CmbCustomerHistory', d.historyCustomers, 'Id', 'Name', '');  // :914
            var days = d.defaultDaysToLessFromHistoryFromDate > 0 ? d.defaultDaysToLessFromHistoryFromDate : 3;   // :383
            $id('txtFromdateHistory').value = addDays(new Date(), -days);
            $id('txtToDateHistory').value = C.today();
            $id('txtgpdate').focus();
        }).catch(fail);
        document.addEventListener('keydown', keyDown);
    }
    function applyLookups(d) {
        bind('cmbgptype', d.gatePassTypes, 'Id', 'Name', '');                                        // :421 then cmbgptype_Leave_1
        bind('CmbReferenceParty', d.referenceParties, 'Id', 'Name');                                 // :648
        bind('CmbTransporter', d.transporters, 'Id', 'AccountTitle', null, ['AccountCode']);        // :695
        bind('cmbvehicletype', d.vehicleTypes, 'Id', 'Name');                                        // :707
        gpTypeLeave();
    }
    /** btnRefresh_Click :1498. */
    function refresh() { C.getJson(api + '/refresh').then(applyLookups).catch(fail); }

    /** cmbgptype_Leave_1 :430. */
    function gpTypeLeave() {
        $id('CmbOrderno').value = '';
        var t = val('cmbgptype') ? text('cmbgptype') : '';
        if (!t) { $id('txtgptypeno').value = ''; return; }
        C.getJson(api + '/type-code' + C.qs({ gatepassType: t })).then(function (d) {
            if (d && d.gpTypeSrNo > 0) { $id('txtgptypeno').value = d.gpTypeSrNo; return; }
            alert('Please GatePass Type Select'); $id('cmbgptype').focus();
        }).catch(fail);
    }

    /** CmbOrderno_Leave :490. */
    function orderNoLeave() {
        if (val('CmbGPBasedOn') === 0) { alert('Order Type Field Required'); $id('CmbGPBasedOn').focus(); return; }
        if (val('cmbgptype') === 0) { alert('GatePass Type Field Required'); $id('cmbgptype').focus(); return; }
        var no = num($id('CmbOrderno').value);
        if (!no) return;
        C.getJson(api + '/delivery-order' + C.qs({ docNo: no })).then(function (d) {
            if (!(d.saleOrderId > 0) || !d.rows || !d.rows.length) { clearCustomerFields(); return; }
            soId = d.saleOrderId;
            var row = d.rows[0];
            $id('txtqty').value = row.DoTotalQty;
            $id('txtvehicleno').value = row.VehicleNo || '';
            var vt = Array.prototype.filter.call($id('cmbvehicletype').options, function (o) { return o.text === row.VehicleType; })[0];
            if (vt) $id('cmbvehicletype').value = vt.value;
            bind('CmbCustomer', d.rows, 'Id', 'CompanyName', null);                                   // :536
        }).catch(function (e) { clearCustomerFields(); alert(e.message); });
    }
    /** ClearCustomerFields :547. */
    function clearCustomerFields() { soId = 0; $id('CmbCustomer').innerHTML = ''; }

    // ------------------------------------------------------------------------------ save

    function body() {
        return {
            Id: id, GpDate: $id('txtgpdate').value, GatePassTypeId: val('cmbgptype'), GatepassType: val('cmbgptype') ? text('cmbgptype') : '',
            GpTypeSrNo: num($id('txtgptypeno').value), OtherSupCust: val('CmbGPBasedOn') ? text('CmbGPBasedOn') : '', OrderNo: num($id('CmbOrderno').value),
            SaleOrderId: soId, SupplierCustomerId: val('CmbCustomer'), ReferencePartyId: val('CmbReferenceParty'), OtherRemarks: $id('txtremarks').value,
            TransporterId: val('CmbTransporter'), Freight: num($id('txtfreight').value), NetPaid: num($id('txtNetPaid').value),
            VehicleTypeId: val('cmbvehicletype'), VehicleType: val('cmbvehicletype') ? text('cmbvehicletype') : '', VehicleNo: $id('txtvehicleno').value,
            BiltyNo: $id('txtbiltyno').value, Qty: $id('txtqty').value, InDateTimeStamp: $id('txtintime').value, OutDateTimeStamp: $id('txtouttime').value,
            Status: val('CmbStatus') ? text('CmbStatus') : ''
        };
    }
    /** formvalidation :733 — FormHelper.ValidateControls, in the desktop's control order. */
    function formValidation() {
        var checks = [['txtgpno', 'Gate Pass No', 'int'], ['cmbgptype', 'GatePass Type', 'int'], ['CmbOrderno', 'Delivery Order #', 'int'], ['CmbCustomer', 'Customer Name', 'int'],
            ['cmbvehicletype', 'Vehicle Type', 'int'], ['txtvehicleno', 'Vehicle No', 'str'], ['txtqty', 'Qty', 'str'], ['CmbStatus', 'Status', 'int']];
        for (var i = 0; i < checks.length; i++) {
            var el = $id(checks[i][0]), ok = checks[i][2] === 'int' ? (el.tagName === 'SELECT' ? val(checks[i][0]) > 0 : num(el.value) > 0) : el.value.trim() !== '';
            if (!ok) { alert(checks[i][1] + ' Field Required'); el.focus(); return false; }
        }
        return true;
    }
    /** Insert :765. */
    function insert() {
        if (!formValidation()) return;
        if (text('CmbStatus') !== 'Open' && id === 0) { alert('Status should be Open in Case of Save'); $id('CmbStatus').focus(); return; }
        if (!confirm(id === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
        C.postJson(api + '/save', body()).then(function (res) {
            alert(res.message);
            var saved = res.id;
            reset();
            if ($id('ChkBox').checked) printSlip(saved);                                             // :827
        }).catch(fail);
    }
    function save() { id = 0; insert(); }                                                            // :752
    function update() { if (id === 0) { alert('Record not update because Id not found'); return; } insert(); }   // :839

    /** btnPrint_Click :1580 → OutwardGatePassReturnable_Slip1201. */
    function printSlip(gp) { if (!gp) { alert('Record Id Not Found'); return; } window.printRpt(RPT, { gatePassId: gp, id: gp }); }
    function print() { printSlip(id); }

    // ------------------------------------------------------------------------------ read / reset

    /** ReadById :855. */
    function readById(gp) {
        C.getJson(api + '/' + gp).then(function (g) {
            id = g.Id;
            tab('tabPage1');
            show('btnupdate', true); show('btnsave', false); updateMode = true;
            $id('txtgpno').value = g.GpSrNo;
            $id('txtgpdate').value = C.isoDay(g.GpDate);
            var gt = Array.prototype.filter.call($id('cmbgptype').options, function (o) { return o.text === g.GatepassType; })[0];
            $id('cmbgptype').value = gt ? gt.value : '0';
            $id('txtgptypeno').value = g.GpTypeSrNo;
            var ot = Array.prototype.filter.call($id('CmbGPBasedOn').options, function (o) { return o.text === g.OtherSupCust; })[0];
            $id('CmbGPBasedOn').value = ot ? ot.value : '0';
            $id('CmbOrderno').value = g.SupplierContractCode || '';
            soId = g.SaleOrderId | 0;
            bind('CmbCustomer', g.customers, 'Id', 'CompanyName', null);
            $id('CmbCustomer').value = String(g.SupplierCustomerId);
            $id('CmbReferenceParty').value = String(g.ReferencePartyId);
            $id('txtremarks').value = g.OtherRemarks || '';
            $id('CmbTransporter').value = String(g.TransporterId);
            $id('txtfreight').value = g.Freight;
            $id('txtNetPaid').value = g.NetPaid;
            var vt = Array.prototype.filter.call($id('cmbvehicletype').options, function (o) { return o.text === g.VehicleType; })[0];
            $id('cmbvehicletype').value = vt ? vt.value : '0';
            $id('txtvehicleno').value = g.VehicleNo || '';
            $id('txtbiltyno').value = g.BiltyNo || '';
            $id('txtqty').value = g.NoOfPackages;
            if (g.InDateTimeStamp) $id('txtintime').value = String(g.InDateTimeStamp).replace(' ', 'T').substring(0, 16);
            $id('txtouttime').value = nowLocal();                                                    // :894
            approve = !!g.IsApproved;
            var st = Array.prototype.filter.call($id('CmbStatus').options, function (o) { return o.text === g.Status; })[0];
            $id('CmbStatus').value = st ? st.value : '0';
        }).catch(fail);
    }

    /** Reset :1430. */
    function reset() {
        soId = 0; id = 0; approve = false; updateMode = false;
        show('btnsave', true); show('btnupdate', false);
        $id('cmbgptype').value = '0'; $id('txtgptypeno').value = ''; $id('CmbOrderno').value = ''; $id('txtremarks').value = '';
        $id('CmbCustomer').innerHTML = '';
        ['CmbReferenceParty', 'CmbTransporter', 'cmbvehicletype'].forEach(function (k) { $id(k).value = '0'; });
        ['txtfreight', 'txtNetPaid', 'txtvehicleno', 'txtbiltyno', 'txtqty'].forEach(function (k) { $id(k).value = ''; });
        $id('txtintime').value = nowLocal(); $id('txtouttime').value = nowLocal();
        $id('CmbStatus').selectedIndex = 1;
        C.getJson(api + '/open-gate-passes').then(renderOpen).catch(fail);
        C.getJson(api + '/pending-delivery-orders').then(renderDos).catch(fail);
        C.getJson(api + '/gp-no').then(function (d) { if (d) $id('txtgpno').value = d.gpNo; }).catch(fail);
        $id('txtgpdate').focus();
    }

    // ------------------------------------------------------------------------------ grids

    var OCOLS = ['GpDate', 'GpNo', 'GatepassType', 'OrderType', 'OrderNo', 'Customer', 'Qty', 'VehicleType', 'VehicleNo', 'BiltyNo', 'Status', 'Remaks'];
    /** OpenGatePassGridFill :1218 / grdfrmsetting :1257 (Print, Edit at positions 0, 1). */
    function renderOpen(list) {
        open = list || [];
        var t = $id('grd');
        t.tHead.innerHTML = '<tr><th>Print</th><th>Edit</th>' + OCOLS.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = open.map(function (g, i) {
            return '<tr data-i="' + i + '"><td class="ctr"><button type="button" class="cx-link" data-p="' + i + '">Print</button></td><td class="ctr"><button type="button" class="cx-link" data-e="' + i + '">Edit</button></td>' +
                OCOLS.map(function (c) { if (c === 'Qty') return '<td class="num">' + fmt(g.Qty, 3) + '</td>'; if (c === 'GpDate') return '<td>' + esc(C.gridDate(g.GpDate)) + '</td>'; return '<td>' + esc(g[c]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) { tr.ondblclick = function () { readById(open[parseInt(tr.getAttribute('data-i'), 10)].Id); }; });   // :1284
        t.tBodies[0].querySelectorAll('[data-p]').forEach(function (b) { b.onclick = function (e) { e.stopPropagation(); printSlip(open[parseInt(b.getAttribute('data-p'), 10)].Id); }; });
        t.tBodies[0].querySelectorAll('[data-e]').forEach(function (b) { b.onclick = function (e) { e.stopPropagation(); readById(open[parseInt(b.getAttribute('data-e'), 10)].Id); }; });
    }
    var DCOLS = ['DocDate', 'DoNo', 'DeliveryOrderType', 'CustomerName', 'DoQty', 'RequestedBy', 'ApprovedBy', 'Remarks', 'AccountRemarks', 'EntryDate', 'EntryUserName'];
    /** PendingDeliveryOrderGridBind :1353 — DoNo is a link → InvDeliveryOrderSlip_Engr1606 (1606-DeliveryOrderSlip_Engr.rpt). */
    function renderDos(list) {
        dos = list || [];
        var t = $id('grdDoInformation');
        t.tHead.innerHTML = '<tr>' + DCOLS.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = dos.map(function (d, i) {
            return '<tr>' + DCOLS.map(function (c) {
                if (c === 'DoNo') return '<td class="ctr"><a data-do="' + i + '">' + esc(d.DoNo) + '</a></td>';
                if (c === 'DoQty') return '<td class="num">' + fmt(d.DoQty, 3) + '</td>';
                if (c === 'DocDate') return '<td>' + esc(C.gridDate(d.DocDate)) + '</td>';
                if (c === 'EntryDate') return '<td>' + esc(C.gridDateTime(d.EntryDate, true)) + '</td>';
                return '<td>' + esc(d[c]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('[data-do]').forEach(function (a) {
            a.onclick = function () { var d = dos[parseInt(a.getAttribute('data-do'), 10)]; window.printRpt('1606-DeliveryOrderSlip_Engr.rpt', { id: d.Id, documentTypeId: d.DocumentTypeId }); };
        });
    }

    // ------------------------------------------------------------------------------ history

    function dateType() { var r = document.querySelector('input[name="rdHistDate"]:checked'); return r ? r.value : 'doc'; }
    /** gridhistory :994. */
    function showHistory() {
        C.postJson(api + '/history', {
            dateType: dateType(), fromDate: $id('txtFromdateHistory').value, toDate: $id('txtToDateHistory').value,
            fromNo: num($id('txtFromNoHistory').value), toNo: num($id('txtToDocNoHistory').value), supplierCustomerId: val('CmbCustomerHistory')
        }).then(function (list) { hist = list || []; renderHistory(); }).catch(fail);
    }
    /** Resethistory :942. */
    function newHistory() {
        $id('txtFromNoHistory').value = ''; $id('txtToDocNoHistory').value = ''; $id('CmbCustomerHistory').value = '0';
        $id('txtFromdateHistory').value = addDays(new Date(), -3); $id('txtFromdateHistory').focus();
    }
    var HCOLS = ['GatepassType', 'OrderType', 'GpSrNo', 'GpDate', 'DeliveryOrderNo', 'CustomerName', 'ReferencePartyName', 'TransporterName', 'Freight', 'NetPaid',
        'VehicleType', 'VehicleNo', 'BiltyNo', 'ItemQty', 'InTime', 'OutTime', 'Status', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'OtherRemarks', 'NoOfAttachments'];
    /** grdhistorysetting :1095 — Print (0), Edit (2). */
    function renderHistory() {
        var t = $id('grdhistory');
        t.tHead.innerHTML = '<tr><th>Print</th><th>' + HCOLS[0] + '</th><th>Edit</th>' + HCOLS.slice(1).map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = hist.map(function (h, i) {
            var cell = function (c) {
                var v = h[c];
                if (c === 'GpDate') v = C.gridDate(v);
                if (c === 'InTime' || c === 'OutTime' || c === 'EntryDate' || c === 'ModifyDate') v = C.gridDateTime(v, true);
                if (c === 'Freight' || c === 'NetPaid' || c === 'ItemQty') return '<td class="num">' + fmt(v, 3) + '</td>';
                if (c === 'NoOfAttachments') return '<td class="lnk" title="Attachments are not ported to the web page">' + esc(v) + '</td>';
                return '<td>' + esc(v) + '</td>';
            };
            return '<tr data-i="' + i + '"><td class="ctr"><button type="button" class="cx-link" data-p="' + i + '">Print</button></td>' + cell(HCOLS[0]) +
                '<td class="ctr"><button type="button" class="cx-link" data-e="' + i + '">Edit</button></td>' + HCOLS.slice(1).map(cell).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) { tr.ondblclick = function () { readById(hist[parseInt(tr.getAttribute('data-i'), 10)].Id); }; });   // :1128
        t.tBodies[0].querySelectorAll('[data-p]').forEach(function (b) { b.onclick = function (e) { e.stopPropagation(); printSlip(hist[parseInt(b.getAttribute('data-p'), 10)].Id); }; });
        t.tBodies[0].querySelectorAll('[data-e]').forEach(function (b) { b.onclick = function (e) { e.stopPropagation(); readById(hist[parseInt(b.getAttribute('data-e'), 10)].Id); }; });
    }

    // ------------------------------------------------------------------------------ tabs / keys

    function tab(name) {
        document.querySelectorAll('.r42-tabs:not(.inner) .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === name); });
        $id('tabPage1').classList.toggle('is-active', name === 'tabPage1');
        $id('tabPage2').classList.toggle('is-active', name === 'tabPage2');
        if (name === 'tabPage2') { $id('txtFromdateHistory').value = addDays(new Date(), -3); $id('txtFromdateHistory').focus(); }   // :926-929
        else $id('txtgpdate').focus();
    }
    function innerTab(name) {
        document.querySelectorAll('.r42-tabs.inner .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-itab') === name); });
        $id('tabPage3').classList.toggle('is-active', name === 'tabPage3');
        $id('tabDoInformation').classList.toggle('is-active', name === 'tabDoInformation');
    }
    function onForm() { return $id('tabPage1').classList.contains('is-active'); }
    /** MakeShortCutKeys :1791. */
    function shortcutKeys() {
        alert(['Ctrl+S  When In Form For Save Else For Show Data in History', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New',
            'Ctrl+P  For Print', 'Ctrl+F5  For Focus on GpDate', 'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+ArrowDown  For Focus On Grid', 'Ctrl+ArrowUp  For Focus On GpDate', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }
    /** OutwardGatePass_KeyDown :1684. */
    function keyDown(e) {
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (e.altKey) { e.preventDefault(); shortcutKeys(); return; }
        if (k === 's') { e.preventDefault(); if (onForm()) { if (!$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) save(); } else showHistory(); }
        else if (k === 'e') { e.preventDefault(); window.location.href = '/app/Inventory?module=42'; }
        else if (k === 'n') { e.preventDefault(); if (onForm()) reset(); else newHistory(); }
        else if (k === 'r') { e.preventDefault(); if (onForm()) refresh(); else showHistory(); }
        else if (k === 'u') { if (!$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled) { e.preventDefault(); update(); } }
        else if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 't') { e.preventDefault(); tab(onForm() ? 'tabPage2' : 'tabPage1'); }
        else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); $id('txtgpdate').focus(); }
        else if (k === 'arrowdown') { e.preventDefault(); var tr = (onForm() ? $id('grd') : $id('grdhistory')).tBodies[0].querySelector('tr'); if (tr) tr.focus(); }
    }

    document.addEventListener('DOMContentLoaded', init);

    window.GpoRet = {
        newClick: reset, refresh: refresh, save: save, update: update, print: print, tab: tab, innerTab: innerTab,
        gpTypeLeave: gpTypeLeave, orderNoLeave: orderNoLeave, shortcutKeys: shortcutKeys,
        showHistory: showHistory, newHistory: newHistory
    };
})();
