/* ============================================================================================
 * Delivery Order Packing Material - screen 506, module 54, DocumentTypeId 85.
 * Desktop: Architecture.WinApp.StoreManagement.DeliveryOrderPackingMaterial.
 *
 *   Load :275   FormValidation :343   FormValidationDetail :366   btnplus :395   grd_DoubleClick :444
 *   btnUpdateDetail :479   grd_ColumnButtonClick :523   Insert :582   btnupdate :705   ReadById :724
 *   txtdocno (Leave) :766   CmbPurchaseOrderNo :989   combitem_ValueChanged :1021   BindWarehouseDropdown :1037
 *   BalRetainQty :1126   BalanceStock :1168   Reset :1233   ResetDetail :1267   gridhistoryfill :1561
 * The server repeats every check; nothing here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/delivery-order';
    var L = null, R = {};
    var table = [], removed = [], Id = 0, IsApproved = false, updateDetailIndex = -1, orders = [], historyRows = [], currentTab = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function fmt(n) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return iso(d); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }
    function dt(v) { if (!v) return ''; var d = new Date(String(v).replace(' ', 'T')); if (isNaN(d.getTime())) return String(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return ddmmm(iso(d)) + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap; }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
        var p; try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function http(method, url, body) {
        var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
                return b;
            });
        });
    }
    function getJson(url) { return http('GET', url); }
    function fill(id, rows, v, t, blank) {
        var sel = $id(id); if (!sel) return; var keep = sel.value;
        var html = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function has(id, v) { var s = $id(id); return !!(s && v !== '' && v !== null && s.querySelector('option[value="' + String(v).replace(/"/g, '') + '"]')); }
    function text(id) { var s = $id(id); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] && s.value !== '' ? s.options[s.selectedIndex].text : ''; }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function byId(list, id) { id = int(id); return (list || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0] || null; }
    function first(id) { var s = $id(id); if (s && s.options.length) s.selectedIndex = 0; }
    function saveMode() { return $id('btnsave').style.display !== 'none' && !$id('btnsave').disabled; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today()); setVal('FromDateHistory', daysAgo(7)); setVal('ToDateHistory', today());
        loadLookups().then(renderGrid);
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; R = d.rights || {};
            fill('cmbbranch1', d.branches, 'Id', 'Name', false); fill('combproject1', d.projects, 'Id', 'Name', false);
            fill('cmbType', [{ Id: 1, Type: 'Normal' }, { Id: 2, Type: 'Retain' }], 'Id', 'Type', false);
            fill('cmbvehicletype', d.vehicleTypes, 'Name', 'Name');
            fill('CmbSupplierCustomer', d.customers, 'Id', 'CompanyName');
            ItemDetailFill();
            fill('CmbCustomerHistory', d.historyCustomers, 'Id', 'Name');
            if (Id === 0) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'DO-' + d.docNo; }
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }
    /* ItemDetailFill:921 - by name or by code. */
    function ItemDetailFill() { fill('CmbItemName', L.items, 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode'); }

    function applyRights() {
        var upd = Id > 0;
        show('btnsave', !upd); show('btnupdate', upd);
        $id('btnsave').disabled = !R.save; $id('btnupdate').disabled = !R.update;
    }

    // ------------------------------------------------------------------ entry bar

    /* combitem_ValueChanged :1021 */
    function itemChanged() {
        var itemId = int(val('CmbItemName'));
        bindUom(itemId);
        bindWarehouseDropdown(itemId);
        refreshCombos();
        return balances();
    }
    function bindUom(itemId) {
        var keep = text('CmbPackUom');
        var list = (L.uoms || []).filter(function (u) { return int(u.ItemId) === itemId; });
        fill('CmbPackUom', list, 'Id', 'UOMCode');
        var m = list.filter(function (u) { return String(u.UOMCode) === keep; })[0];
        setVal('CmbPackUom', m && keep ? m.Id : '');
        if (int(val('CmbPackUom'))) return;
        if (list.length === 1) setVal('CmbPackUom', list[0].Id);
        else if (list.length > 1) { var e = list.filter(function (u) { return num(u.Equivalent) === 1; })[0]; if (e) setVal('CmbPackUom', e.Id); }
    }
    /* BindWarehouseDropdown :1037 */
    function bindWarehouseDropdown(itemId) {
        var seen = {}, list = [];
        (L.racks || []).forEach(function (r) { if (int(r.ItemId) === itemId && !seen[r.WarehouseId]) { seen[r.WarehouseId] = 1; list.push({ Id: r.WarehouseId, Warehouse: r.WareHouseName }); } });
        fill('CmbWareHouse', list, 'Id', 'Warehouse');
        if (int(val('CmbWareHouse'))) return;
        if (list.length === 1) setVal('CmbWareHouse', list[0].Id);
        else if (list.length > 1) {
            var cfg = int(L.packingMaterialDefaultWarehouse);
            if (cfg && list.some(function (w) { return int(w.Id) === cfg; })) setVal('CmbWareHouse', cfg);
        }
    }
    /* BalRetainQty :1126 + BalanceStock :1168 */
    function balances() {
        var q = '?bagTypeId=' + int(val('cmbType')) + '&customerId=' + int(val('CmbSupplierCustomer')) + '&itemId=' + int(val('CmbItemName'))
            + '&docDate=' + encodeURIComponent(val('DocDate'));
        return getJson(api + '/balances' + q).then(function (d) {
            var retain = int(val('cmbType')) === 2;
            Array.prototype.forEach.call(document.querySelectorAll('.retain'), function (e) { e.style.display = retain ? '' : 'none'; });
            if (retain) setVal('txtbalRetainQty', num(d.retain) > 0 ? String(d.retain) : '');
            var l = $id('lblStockQty'); l.style.display = ''; l.textContent = num(d.stock) === 0 ? '0' : num(d.stock).toLocaleString('en-US', { maximumFractionDigits: 2 });
        }).catch(function (e) { box(e.message); });
    }
    /* CmbSupplierCustomer_Leave :1071 → CmbPurchaseOrderNo :989 */
    function customerLeave() {
        var c = int(val('CmbSupplierCustomer'));
        var p = c ? getJson(api + '/orders?customerId=' + c) : Promise.resolve([]);
        return p.then(function (rows) {
            orders = rows || [];
            $id('orderList').innerHTML = orders.map(function (o) { return '<option value="' + esc(o.Name) + '"></option>'; }).join('');
            return balances();
        }).catch(function (e) { box(e.message); });
    }
    function orderId() { var t = val('CmbOrderNo').trim(); var o = orders.filter(function (x) { return String(x.Name) === t; })[0]; return o ? int(o.Id) : 0; }

    /* FormValidationDetail :366 */
    function FormValidationDetail() {
        if (text('cmbType').trim() === '' || !int(val('CmbItemName'))) { box('Bag Type field is required'); focus('cmbType'); return false; }
        if (text('CmbSupplierCustomer').trim() === '' || !int(val('CmbSupplierCustomer'))) { box('Customer field is required'); focus('CmbSupplierCustomer'); return false; }
        if (text('CmbItemName').trim() === '' || !int(val('CmbItemName'))) { box('ItemName field is required'); focus('CmbItemName'); return false; }
        if (text('CmbPackUom').trim() === '' || !int(val('CmbPackUom'))) { box('PackUOM field is required'); focus('CmbPackUom'); return false; }
        return true;
    }
    function retainExceeded() {
        return int(val('cmbType')) === 2 && num(val('txtqty')) > num(val('txtbalRetainQty'));
    }
    function barRow(id) {
        var it = byId(L.items, val('CmbItemName')) || {};
        return { Id: id, BagTypeId: int(val('cmbType')), BagType: text('cmbType').trim(), SupplierCustomerId: int(val('CmbSupplierCustomer')),
            SupplierCustomer: text('CmbSupplierCustomer').trim(), OrderId: orderId(), OrderNo: int(val('CmbOrderNo').trim()),
            ItemId: int(val('CmbItemName')), ItemCode: it.ItemCode || '', Item: it.ItemName || '', WareHouseId: int(val('CmbWareHouse')),
            WareHouseName: text('CmbWareHouse'), ItemUOMId: int(val('CmbPackUom')), ItemUOM: text('CmbPackUom').trim(),
            QTY: num(val('txtqty').trim()), Remarks: val('txtremarksdetail').trim() };
    }
    function btnplus_Click() {
        if (!FormValidationDetail()) return;
        if (retainExceeded()) { box('Qty can Not be Greater than Bal retain Qty When Bag Type Is Retain....'); return; }
        table.push(barRow(0)); ResetDetail(); renderGrid();
    }
    function btnUpdateDetail_Click() {
        if (!FormValidationDetail()) return;
        if (retainExceeded()) { box('Qty can Not be Greater than Bal retain Qty When Bag Type Is Retain....'); return; }
        var t = table[updateDetailIndex]; if (!t) return;
        var b = barRow(int(t.Id)); b.Remarks = val('txtremarksdetail');
        table[updateDetailIndex] = b;
        ResetDetail(); renderGrid();
    }
    /* ResetDetail :1267 - the bag type TEXT is cleared too. */
    function ResetDetail() {
        updateDetailIndex = -1;
        setVal('cmbType', ''); setVal('CmbOrderNo', ''); setVal('CmbItemName', ''); fill('CmbWareHouse', [], 'Id', 'Warehouse');
        setVal('CmbPackUom', ''); setVal('txtqty', ''); setVal('txtremarksdetail', '');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        refreshCombos(); renderGrid(); focus('cmbType');
    }
    /* grd_DoubleClick :444 */
    function editRow(i) {
        var r = table[i]; if (!r) return;
        updateDetailIndex = i;
        setVal('cmbType', r.BagTypeId);
        setVal('CmbSupplierCustomer', r.SupplierCustomerId);
        customerLeave().then(function () { if (int(r.OrderId) > 0) { var o = byId(orders, r.OrderId); setVal('CmbOrderNo', o ? o.Name : r.OrderNo); } });
        setVal('CmbItemName', r.ItemId); itemChanged();
        if (int(r.WareHouseId) > 0) setVal('CmbWareHouse', r.WareHouseId);
        setVal('CmbPackUom', r.ItemUOMId);
        setVal('txtqty', String(num(r.QTY))); setVal('txtremarksdetail', r.Remarks);
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        refreshCombos(); renderGrid(); focus('CmbItemName');
    }
    /* grd_ColumnButtonClick :523 */
    function deleteRow(i) {
        if (!ask('Are you sure to Delete?')) return;
        var r = table[i]; if (!r) return;
        if (!saveMode() && int(r.Id) > 0) removed.push(int(r.Id));
        table.splice(i, 1);
        if (updateDetailIndex === i) updateDetailIndex = -1; else if (updateDetailIndex > i) updateDetailIndex--;
        renderGrid();
    }

    var GRID_COLS = [['BagType', 'BagType'], ['SupplierCustomer', 'SupplierCustomer'], ['OrderNo', 'OrderNo'], ['ItemCode', 'ItemCode'], ['Item', 'Item'],
        ['WareHouseName', 'WareHouseName'], ['ItemUOM', 'ItemUOM'], ['QTY', 'QTY', 1], ['Remarks', 'Remarks']];
    function renderGrid() {
        $id('grdHead').innerHTML = '<th>X</th>' + GRID_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
        var q = 0;
        $id('grd').innerHTML = table.map(function (r, i) {
            q += num(r.QTY);
            return '<tr class="data-row' + (i === updateDetailIndex ? ' editing' : '') + '" data-i="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + GRID_COLS.map(function (c) { return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[2] ? fmt(r[c[0]]) : r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = table.length ? '<td colspan="8">' + table.length + ' row(s)</td><td class="num">' + fmt(q) + '</td><td></td>' : '';
    }

    // ------------------------------------------------------------------ save / read

    /* Insert :582 */
    function Insert(btn) {
        if (text('cmbbranch1').trim() === '') { box('branch field is required'); focus('cmbbranch1'); return; }
        if (text('combproject1').trim() === '') { box('Project Field is Required'); focus('combproject1'); return; }
        if (val('txtdocno').trim() === '' || val('txtdocno').trim() === '0') { box('DocNo Field is Required'); focus('txtdocno'); return; }
        if (table.length === 0) { box('Grid Record Not Found'); return; }
        if (!ask(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { Id: Id, DocDate: val('DocDate'), BranchesId: int(val('cmbbranch1')), ProjectsId: int(val('combproject1')),
            Remarks: val('txtremarks'), VehicleType: text('cmbvehicletype'), VehicleNo: val('txtVehicleNo').trim(),
            rows: table.map(function (r) { return { Id: int(r.Id), BagTypeId: int(r.BagTypeId), SupplierCustomerId: int(r.SupplierCustomerId), OrderId: int(r.OrderId),
                ItemId: int(r.ItemId), WareHouseId: int(r.WareHouseId), ItemUOMId: int(r.ItemUOMId), QTY: num(r.QTY), Remarks: r.Remarks || '' }; }),
            removedIds: Id > 0 ? removed.slice() : [] };
        return busy(btn, function () {
            return http('POST', api + '/save', body).then(function (d) {
                box(d.message);
                var print = $id('ChkPrint').checked;
                return Reset().then(function () { if (print) InvDeliveryOrderSlip(d.id); });
            }).catch(function (e) { box(e.message); });
        });
    }
    /* Reset :1233 */
    function Reset() {
        Id = 0; IsApproved = false; first('cmbbranch1'); first('combproject1');
        setVal('CmbItemName', ''); setVal('CmbPackUom', ''); setVal('txtqty', ''); setVal('CmbOrderNo', '');
        setVal('txtremarks', ''); setVal('txtVehicleNo', ''); setVal('cmbvehicletype', '');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        table = []; removed = []; applyRights(); renderGrid(); refreshCombos(); focus('DocDate');
        return getJson(api + '/doc-no').then(function (d) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'DO-' + d.docNo; })
            .catch(function (e) { box(e.message); });
    }
    /* ReadById :724 */
    function ReadById(id) {
        return getJson(api + '/' + id).then(function (d) {
            Id = int(d.Id); removed = [];
            if (has('cmbbranch1', d.BranchesId)) setVal('cmbbranch1', d.BranchesId);
            if (has('combproject1', d.ProjectsId)) setVal('combproject1', d.ProjectsId);
            showTab(0);
            setVal('DocDate', dateOnly(d.DocDate)); setVal('txtdocno', d.DocNo); $id('txtDocNoShow').textContent = 'DO-' + d.DocNo;
            if (!has('cmbvehicletype', d.VehicleType) && d.VehicleType) { var o = document.createElement('option'); o.value = d.VehicleType; o.textContent = d.VehicleType; $id('cmbvehicletype').appendChild(o); }
            setVal('cmbvehicletype', d.VehicleType); setVal('txtVehicleNo', d.VehicleNo); setVal('txtremarks', d.Remarks);
            IsApproved = !!d.IsApproved;
            table = d.rows || [];
            applyRights(); ResetDetail(); setVal('CmbSupplierCustomer', ''); refreshCombos(); renderGrid();
        }).catch(function (e) { box(e.message); });
    }
    function InvDeliveryOrderSlip(id) {
        if (!int(id)) { box('PrintId not found...'); return; }
        window.open('/api/reports/dopm-262/data?id=' + encodeURIComponent(id), '_blank');
    }
    /* txtdocno_TextChanged (Leave) :766 */
    function docNoLeave() {
        var n = int(val('txtdocno')); if (!n) return;
        getJson(api + '/id-by-doc-no?docNo=' + n).then(function (d) { if (int(d.id) > 0) ReadById(int(d.id)); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history

    var HIST_COLS = [['DocNo', 'DocNo'], ['DocDate', 'DocDate', 'd'], ['VehicleType', 'VehicleType'], ['VehicleNo', 'VehicleNo'], ['Remarks', 'Remarks'],
        ['EntryDate', 'EntryDate', 't'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 't'], ['ModifyUser', 'ModifyUser'], ['NoOfAttachments', 'NoOfAttachments']];
    function gridhistoryfill() {
        var dtp = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var q = '?dateType=' + dtp + '&fromDate=' + ($id('chkFromDate').checked ? encodeURIComponent(val('FromDateHistory')) : '')
            + '&toDate=' + ($id('chkToDate').checked ? encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('FromDocNoHistory')) + '&toDocNo=' + int(val('ToDocNoHistory')) + '&customerId=' + int(val('CmbCustomerHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            $id('histDetailHead').innerHTML = ''; $id('grddetailHistory').innerHTML = '';
            if (!historyRows.length) { $id('histHead').innerHTML = ''; $id('grdhistory').innerHTML = ''; $id('lblHistCount').textContent = ''; return; }
            var btns = (R.print ? '<th>Print</th>' : '') + (R.update ? '<th>Edit</th>' : '');
            $id('histHead').innerHTML = btns + HIST_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
            $id('grdhistory').innerHTML = historyRows.map(function (r, i) {
                return '<tr class="data-row" data-i="' + i + '">'
                    + (R.print ? '<td><button type="button" class="win-btn-mini" data-act="print">Print</button></td>' : '')
                    + (R.update ? '<td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>' : '')
                    + HIST_COLS.map(function (c) { var v = r[c[0]]; return '<td>' + esc(c[2] === 'd' ? ddmmm(v) : c[2] === 't' ? dt(v) : v) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('lblHistCount').textContent = historyRows.length + ' record(s)';
        }).catch(function (e) { box(e.message); });
    }
    /* grdhistory_SelectionChanged :1777 */
    function historySelect(i) {
        var r = historyRows[i]; if (!r) return;
        Array.prototype.forEach.call($id('grdhistory').rows, function (tr) { tr.classList.toggle('sel', int(tr.getAttribute('data-i')) === i); });
        getJson(api + '/' + int(r.Id)).then(function (d) {
            var cols = [['BagType', 'BagType'], ['SupplierCustomer', 'SupplierCustomer'], ['OrderNo', 'OrderNo'], ['ItemCode', 'ItemCode'], ['Item', 'Item'],
                ['WareHouseName', 'WareHouse'], ['ItemUOM', 'ItemUOM'], ['QTY', 'QTY', 1], ['Remarks', 'Remarks']];
            $id('histDetailHead').innerHTML = cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
            $id('grddetailHistory').innerHTML = (d.rows || []).map(function (x) {
                return '<tr>' + cols.map(function (c) { return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[2] ? fmt(x[c[0]]) : x[c[0]]) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ wiring

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    function bindEvents() {
        on('cmbType', 'change', balances);
        on('CmbSupplierCustomer', 'change', customerLeave);
        on('CmbItemName', 'change', function () { setVal('CmbWareHouse', ''); itemChanged(); });
        on('rdbtnItemName', 'change', function () { var k = val('CmbItemName'); ItemDetailFill(); setVal('CmbItemName', k); refreshCombos(); });
        on('rdbtnItemCode', 'change', function () { var k = val('CmbItemName'); ItemDetailFill(); setVal('CmbItemName', k); refreshCombos(); });
        on('txtdocno', 'change', docNoLeave);
        on('grd', 'click', function (e) { var b = e.target.closest('button'), tr = e.target.closest('tr'); if (b && tr) deleteRow(int(tr.getAttribute('data-i'))); });
        on('grd', 'dblclick', function (e) { if (e.target.closest('button')) return; var tr = e.target.closest('tr'); if (tr) editRow(int(tr.getAttribute('data-i'))); });
        on('grdhistory', 'click', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            var i = int(tr.getAttribute('data-i')), b = e.target.closest('button');
            if (!b) { historySelect(i); return; }
            if (b.getAttribute('data-act') === 'edit' && R.update) ReadById(int(historyRows[i].Id));
            else if (b.getAttribute('data-act') === 'print' && R.print) InvDeliveryOrderSlip(historyRows[i].Id);
        });
        on('grdhistory', 'dblclick', function (e) { if (!R.update) return; var tr = e.target.closest('tr'); if (tr) ReadById(int(historyRows[int(tr.getAttribute('data-i'))].Id)); });
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
            else if (k === 's' && currentTab === 0 && saveMode()) { e.preventDefault(); window.Dop.btnsave_Click(); }
            else if (k === 'u' && Id > 0) { e.preventDefault(); window.Dop.btnupdate_Click(); }
            else if (k === 'n' && currentTab === 0) { e.preventDefault(); window.Dop.btnnew_Click(); }
            else if (k === 'p') { e.preventDefault(); window.Dop.btnprint_Click(); }
        });
    }

    window.Dop = {
        btnnew_Click: function () { return busy('btnnew', Reset); },
        btnRefresh_Click: function () { return busy('btnRefresh', loadLookups); },
        btnsave_Click: function () { if ($id('btnsave').disabled || Id !== 0) return; return Insert('btnsave'); },
        btnupdate_Click: function () { if ($id('btnupdate').disabled) return; if (IsApproved) { box('Approved Record Not Update'); return; } return Insert('btnupdate'); },
        btnprint_Click: function () { InvDeliveryOrderSlip(Id); },
        btnplus_Click: btnplus_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, btnCancelUpdateDetial_Click: ResetDetail,
        showTab: showTab, btnShowHistory_Click: function () { return busy('btnShowHistory', gridhistoryfill); },
        btnRefreshHistory_Click: function () { return getJson(api + '/history-customers').then(function (rows) { fill('CmbCustomerHistory', rows, 'Id', 'Name'); }).catch(function (e) { box(e.message); }); },
        btnNewHistory_Click: function () {
            setVal('FromDateHistory', daysAgo(3)); setVal('ToDateHistory', today()); setVal('FromDocNoHistory', ''); setVal('ToDocNoHistory', '');
            setVal('CmbCustomerHistory', ''); historyRows = [];
            ['histHead', 'grdhistory', 'histDetailHead', 'grddetailHistory'].forEach(function (k) { $id(k).innerHTML = ''; });
            var d = document.querySelector('input[name="rdDate"][value="doc"]'); if (d) d.checked = true;
        }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
