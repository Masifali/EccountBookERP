/* ============================================================================================
 * Goods Receipt Notes PM - screen 500, module 54, DocumentTypeId 701.
 * Desktop: Architecture.WinApp.PackingMaterial_Store.GrnPackingMaterial (4,632 lines)
 *          + LoadPurchaseOrderPM (the order loader dialog for a General gate pass).
 *
 * Handler names follow the desktop's:
 *   InitializeComponentMethod :377   FormValidation :452   Insert :534   btnDelete_Click :739
 *   ReadById :775   reset :1076   PendingGatePassForGrnLoad :1166   LoadGpData :1283
 *   HistoryGridFill :1359   GrdHistory_ColumnButtonClick :1576   BindHistoryDetail :1605
 *   txtsuppwt_TextChanged :1910   CmbDeliveryTerm_TextChanged :1937   TotalSupplierWeight :1977
 *   WeightPerQtyCalculate :1994   BindGridByOrderId :2085   grd_ColumnButtonClick :2262
 *   grd_CellUpdated :2297   grd_KeyDown (F1 warehouse / rack) :2329   LoadInGridDetail :2513
 *   ResolveWarehouseAndRack :2616
 * The server repeats every validation and re-derives every disabled field; nothing here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/grn';
    var L = null, cfg = {}, perms = {};
    var dtdetail = [];              // the grid's DataTable
    var Id = 0;
    var IsGeneralGp = false;
    var BillCalculateTypeId = 0;
    var gpRows = [];
    var newFiles = [], removeAttachmentIds = [], existingAttachments = [];
    var historyRows = [];
    var currentTab = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
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
    function round2(x) { return Math.round((x + (x < 0 ? -1e-9 : 1e-9)) * 100) / 100; }
    function upTo(n, dp) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dp }); }
    function today() { var d = new Date(); return iso(d); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function addDays(days) { var d = new Date(); d.setDate(d.getDate() + days); return iso(d); }
    function dateOnly(v) {
        if (!v) return '';
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(v); if (isNaN(d.getTime())) return '';
        return iso(d);
    }
    function monthOnly(v) { var d = dateOnly(v); return d ? d.substring(0, 7) : ''; }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function mmmyyyy(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return MON[+p[1] - 1] + '-' + p[0]; }
    function ddmmmyyyy(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }
    function stamp(v) {
        if (!v) return ''; var d = new Date(v); if (isNaN(d.getTime())) return String(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return String(d.getDate()).padStart(2, '0') + '-' + MON[d.getMonth()] + '-' + d.getFullYear() + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
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

    function fill(id, rows, valueKey, textKey, blank) {
        var sel = $id(id); if (!sel) return;
        var keep = sel.value;
        var html = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            html += '<option value="' + esc(col(r, valueKey)) + '">' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    /** UltraCombo.Text = "..." - selects the row whose display text matches, else clears. */
    function selectByText(id, text) {
        var sel = $id(id); if (!sel) return;
        var t = String(text === null || text === undefined ? '' : text).trim();
        sel.value = '';
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].textContent.trim() === t && t) { sel.selectedIndex = i; break; }
    }
    function selText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value ? o.textContent : ''; }
    function hasOption(id, v) { var s = $id(id); return !!(s && s.querySelector('option[value="' + String(v).replace(/"/g, '') + '"]')); }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today());
        loadLookups();
    }

    /** InitializeComponentMethod :377 - rights, number, vehicle types, pending GPs, history combo. */
    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; cfg = d.configuration || {}; perms = d.permissions || {};
            SupplierBind();
            fill('combvehtyp', d.vehicleTypes, 'Id', 'VehicleDescription');
            fill('CmbDeliveryTerm', d.deliveryTerms, 'Id', 'Description');
            TransporterAccountFill();
            HistoryComboFill(d.historySuppliers);
            if (Id === 0) setVal('txtdocno', d.docNo);
            $id('txtDocNoShow').textContent = Id === 0 ? ('GRN-' + (d.docNo || '')) : $id('txtDocNoShow').textContent;
            PendingGatePassForGrnLoad(d.pendingGatePasses);
            renderGrid();
            // :435 - FreightInwardAc pre-selected when that account is in the list
            var acId = int(cfg.freightInwardAc);
            if (acId > 0 && (L.transporters || []).some(function (r) { return int(r.Id) === acId; }) && hasOption('CmbTransport', acId)) setVal('CmbTransport', acId);
            var back = int(cfg.defaultDaysToLessFromHistoryFromDate);
            setVal('FromDateHistory', addDays(-(back > 0 ? back : 3)));
            setVal('ToDateHistory', today());
            refreshCombos();
            applyRights();
            say('');
            focus('DocDate');
        }).catch(function (e) { say(''); box(e.message || 'Error occurred during database call.'); });
    }

    function SupplierBind() { fill('combsupplier', L.suppliers, 'Id', 'CompanyName'); }

    /** TransporterAccountFill :1024 - value member SupplierCustomerId with feature 4, else Id; blank row first. */
    function TransporterAccountFill() {
        fill('CmbTransport', L.transporters, cfg.subsidiaryAccounts ? 'SupplierCustomerId' : 'Id', 'AccountTitle');
    }

    function HistoryComboFill(rows) { fill('cmbSupplierNameHistory', rows, 'Id', 'Description'); }

    function applyRights() {
        var upd = Id > 0;
        show('btnSave', !upd); show('btnUpdate', upd); show('btnDelete', upd && !!perms.Delete);
        $id('btnSave').disabled = !perms.Save;
        $id('btnUpdate').disabled = !perms.Update;
        $id('btnDelete').disabled = !perms.Delete;
        $id('printToolStripButton').disabled = !perms.Print;
        $id('BtnPrintII').disabled = !perms.Print;
        show('BtnLoadOrder', IsGeneralGp);
    }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }

    // ------------------------------------------------------------------ pending gate passes

    var GP_COLS = [
        ['OrderNo', 'OrderNo'], ['GpSrNo', 'Gp No'], ['GpDate', 'GpDate', 'date'], ['OrderType', 'OrderType'],
        ['SupplierName', 'SupplierName'], ['VehicleType', 'VehicleType'], ['VehicleNo', 'VehicleNo'], ['BiltyNo', 'BiltyNo'],
        ['VarietyName', 'Item Name'], ['Qty', 'Qty', 'n'], ['SupplierWeight', 'SupplierWeight', 'n'], ['FactoryWeight', 'FactoryWeight', 'n'],
        ['ReceivedWeight', 'ReceivedWeight', 'n'], ['StockWeight', 'StockWeight', 'n'], ['NetPaid', 'NetPaid', 'n'], ['Status', 'Status'],
        ['EntryDate', 'EntryDate', 'stamp'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 'stamp'], ['ModifyUser', 'ModifyUser']
    ];

    function cell(v, kind) {
        if (kind === 'date') return ddmmmyyyy(v);
        if (kind === 'stamp') return stamp(v);
        if (kind === 'month') return mmmyyyy(v);
        if (kind === 'n') return upTo(v, 3);
        return esc(v);
    }

    /** PendingGatePassForGrnLoad :1166 - the rows as the proc returns them; PurchaseOrderId/No become OrderId/No. */
    function PendingGatePassForGrnLoad(rows) {
        gpRows = (rows || []).map(function (r) {
            var m = {}; for (var k in r) m[k] = r[k];
            m.OrderId = col(r, 'PurchaseOrderId'); m.OrderNo = col(r, 'PurchaseOrderNo');
            return m;
        });
        var head = '<th>Load</th>'; GP_COLS.forEach(function (c) { head += '<th>' + esc(c[1]) + '</th>'; });
        $id('grdGpHead').innerHTML = gpRows.length ? head : '';
        var html = '';
        gpRows.forEach(function (r, i) {
            html += '<tr class="data-row"><td><button type="button" class="win-btn-mini" data-gp="' + i + '">Load</button></td>';
            GP_COLS.forEach(function (c) { html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + cell(col(r, c[0]), c[2]) + '</td>'; });
            html += '</tr>';
        });
        $id('grdGp').innerHTML = html;
        $id('lblGpCount').textContent = gpRows.length ? gpRows.length + ' pending' : '';
    }

    /** LoadGpData :1283. */
    function LoadGpData(r) {
        if (!r) return;
        if (Id > 0 || $id('btnSave').disabled) { box('Please Reset the form First...'); return; }
        IsGeneralGp = false;
        if (String(col(r, 'Status')) !== 'Accepted') { box('Status Not Accepted Please check status'); return; }
        if (int(col(r, 'RefDocumentTypeId')) === 52) IsGeneralGp = true;
        setVal('txtGpId', col(r, 'Id'));
        setVal('txtGpNo', col(r, 'GpSrNo'));
        setVal('txtbltyno', col(r, 'BiltyNo'));
        selectByText('combvehtyp', col(r, 'VehicleType'));
        setVal('txtvehno', col(r, 'VehicleNo'));
        setVal('txtfctwt', col(r, 'FactoryWeight'));
        setVal('txtsuppwt', col(r, 'SupplierWeight'));
        txtsuppwt_TextChanged();                                // :1910 - Fact Weight takes the Supp Weight
        var freight = num(col(r, 'NetPaid'));
        if (freight > 0) setVal('txtcarramount', String(freight));
        if (int(col(r, 'FreightId')) > 0 && hasOption('CmbTransport', int(col(r, 'FreightId')))) setVal('CmbTransport', int(col(r, 'FreightId')));
        refreshCombos();
        applyRights();
        if (IsGeneralGp) { loader.open(); return; }
        return BindGridByOrderId(int(col(r, 'OrderId')), int(col(r, 'Id')));
    }

    // ------------------------------------------------------------------ detail rows

    /** ResolveWarehouseAndRack :2616 over racksWithWarehouseAndItems for one item. */
    function ResolveWarehouseAndRack(itemId) {
        var itemData = (L.racks || []).filter(function (x) { return int(x.ItemId) === itemId; });
        var none = { WarehouseId: 0, WarehouseName: '', RackId: 0, RackName: '' };
        if (!itemData.length) return none;
        var base = itemData.filter(function (x) { return int(x.BaseRackId) > 0; })[0];
        if (base) return { WarehouseId: int(base.WarehouseId), WarehouseName: base.WareHouseName, RackId: int(base.Id), RackName: base.RackName };
        var whs = distinct(itemData, 'WarehouseId');
        function pick(wh) {
            var racks = distinct(itemData.filter(function (x) { return int(x.WarehouseId) === int(wh.WarehouseId); }), 'Id');
            if (racks.length === 1) return { WarehouseId: int(wh.WarehouseId), WarehouseName: wh.WareHouseName, RackId: int(racks[0].Id), RackName: racks[0].RackName };
            return { WarehouseId: int(wh.WarehouseId), WarehouseName: wh.WareHouseName, RackId: 0, RackName: '' };
        }
        if (whs.length === 1) return pick(whs[0]);
        var conf = int(cfg.defaultWarehouseId);
        if (conf > 0) { var wh2 = whs.filter(function (x) { return int(x.WarehouseId) === conf; })[0]; if (wh2) return pick(wh2); }
        return none;
    }
    function distinct(rows, key) {
        var seen = {}, out = [];
        rows.forEach(function (r) { var k = String(r[key]); if (!seen[k]) { seen[k] = 1; out.push(r); } });
        return out;
    }

    function newRow(src, qty, wtPerQty, itemId, orderDate, orderNo, uomId, uom, remarks) {
        var wr = ResolveWarehouseAndRack(itemId);
        return {
            Id: 0, OrderId: int(col(src, 'Id')), OrderDetailId: int(col(src, 'OrderDetailId')),
            OrderDate: dateOnly(orderDate), OrderNo: orderNo, ItemId: itemId, ItemName: col(src, 'ItemName'),
            CropYearId: int(col(src, 'CropYearId')), PackingDate: dateOnly(col(src, 'PackingDate')), ExpiryDate: dateOnly(col(src, 'ExpiryDate')),
            PackUOMId: uomId, PackUOM: uom, ItemQty: qty, QtyValidate: qty, WtPerQty: wtPerQty, GrossWeight: qty * wtPerQty,
            SupplierQty: 0, CityName: '', WarehouseId: wr.WarehouseId, Warehouse: wr.WarehouseName,
            RemarksDetail: remarks || '', ItemConditionId: 0, RackId: wr.RackId, RackName: wr.RackName
        };
    }

    function setBillType(t) {
        BillCalculateTypeId = int(t);
        setVal('txtBillCalculateType', BillCalculateTypeId === 1 ? 'OnWeight' : BillCalculateTypeId === 2 ? 'OnQty' : '');
    }

    /** BindGridByOrderId :2085 - the whole detail is replaced. */
    function BindGridByOrderId(orderId, gpId) {
        say('Loading order...');
        return getJson(api + '/order-lines?orderId=' + orderId + '&gpId=' + gpId).then(function (rows) {
            say('');
            dtdetail = [];
            if (!rows || !rows.length) { renderGrid(); return; }
            setBillType(col(rows[0], 'BillCalculateTypeId'));
            var supplier = 0;
            rows.forEach(function (src) {
                dtdetail.push(newRow(src, num(col(src, 'BalQty')), num(col(src, 'WeightPerQty')), int(col(src, 'ItemId')),
                    col(src, 'DocDate'), col(src, 'DocNo'), int(col(src, 'ItemUOMId')), col(src, 'PackUom'), col(src, 'RemarksHeader')));
                supplier = int(col(src, 'OrderSupCustId'));
            });
            setVal('combsupplier', supplier);
            selectByText('CmbDeliveryTerm', col(rows[0], 'DeliveryTerm'));
            CmbDeliveryTerm_TextChanged();
            refreshCombos();
            renderGrid();
        }).catch(function (e) { say(''); box(e.message); });
    }

    /** LoadInGridDetail :2513 - appended, rows already present skipped, one party only. */
    function LoadInGridDetail(sourceRows) {
        if (!sourceRows || !sourceRows.length) return;
        setBillType(col(sourceRows[0], 'BillCalculateTypeId'));
        var existing = {}; dtdetail.forEach(function (r) { existing[int(r.OrderDetailId)] = 1; });
        var selectedSupplier = int(val('combsupplier'));
        var add = [];
        for (var i = 0; i < sourceRows.length; i++) {
            var src = sourceRows[i];
            var odId = int(col(src, 'OrderDetailId')), sup = int(col(src, 'OrderSupCustId'));
            if (selectedSupplier > 0 && selectedSupplier !== sup) { box('Data against another Party Already Exist in Detail'); return; }
            if (existing[odId]) continue;
            add.push(newRow(src, num(col(src, 'BalQty')), num(col(src, 'WeightPerQty')), int(col(src, 'ItemId')),
                col(src, 'DocDate'), col(src, 'DocNo'), int(col(src, 'ItemUOMId')), col(src, 'PackUom'), col(src, 'RemarksHeader')));
            existing[odId] = 1;
            selectedSupplier = sup;
        }
        dtdetail = dtdetail.concat(add);
        setVal('combsupplier', selectedSupplier);
        selectByText('CmbDeliveryTerm', col(sourceRows[0], 'DeliveryTerm'));
        CmbDeliveryTerm_TextChanged();
        refreshCombos();
        renderGrid();
    }

    function options(rows, valueKey, textKey, current, byText) {
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var v = byText ? col(r, textKey) : col(r, valueKey);
            var on = byText ? String(v) === String(current) : int(v) === int(current);
            html += '<option value="' + esc(v) + '"' + (on ? ' selected' : '') + '>' + esc(col(r, textKey)) + '</option>';
        });
        return html;
    }

    function renderGrid() {
        $id('grdHead').innerHTML = '<th>X</th><th>+</th><th>OrderDate</th><th>OrderNo</th><th>ItemName</th><th>CropYear</th>'
            + '<th>PackingDate</th><th>ExpiryDate</th><th>PackUOM</th><th>ItemCondition</th><th>ItemQty</th><th>WtPerQty</th>'
            + '<th>GrossWeight</th><th>SupplierQty</th><th>CityName</th><th>Warehouse</th><th>RackName</th><th>RemarksDetail</th>';
        var html = '', qty = 0, gw = 0;
        dtdetail.forEach(function (r, i) {
            qty += num(r.ItemQty); gw += num(r.GrossWeight);
            html += '<tr class="data-row" data-i="' + i + '">'
                + '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + '<td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td>' + ddmmmyyyy(r.OrderDate) + '</td><td>' + esc(r.OrderNo) + '</td><td>' + esc(r.ItemName) + '</td>'
                + '<td><select class="cell-sel" data-f="CropYearId">' + options(L ? L.cropYears : [], 'Id', 'Description', r.CropYearId) + '</select></td>'
                + '<td><input type="month" class="cell-month" data-f="PackingDate" value="' + esc(monthOnly(r.PackingDate)) + '"/></td>'
                + '<td><input type="month" class="cell-month" data-f="ExpiryDate" value="' + esc(monthOnly(r.ExpiryDate)) + '"/></td>'
                + '<td>' + esc(r.PackUOM) + '</td>'
                + '<td><select class="cell-sel" data-f="ItemConditionId">' + options(L ? L.itemConditions : [], 'Id', 'Description', r.ItemConditionId) + '</select></td>'
                + '<td><input class="cell" data-f="ItemQty" data-guard="decimal" value="' + esc(r.ItemQty) + '"/></td>'
                + '<td class="num">' + upTo(r.WtPerQty, 3) + '</td>'
                + '<td><input class="cell" data-f="GrossWeight" data-guard="decimal" value="' + esc(r.GrossWeight) + '"/></td>'
                + '<td><input class="cell" data-f="SupplierQty" data-guard="decimal" value="' + esc(r.SupplierQty) + '"/></td>'
                + '<td><select class="cell-sel" data-f="CityName">' + options(L ? L.cities : [], 'Id', 'Description', r.CityName, true) + '</select></td>'
                + '<td><select class="cell-sel" data-f="WarehouseId" title="F1 opens the list">' + warehouseOptions(r) + '</select></td>'
                + '<td><select class="cell-sel" data-f="RackId" title="F1 opens the list">' + rackOptions(r) + '</select></td>'
                + '<td><input class="cell-text" data-f="RemarksDetail" value="' + esc(r.RemarksDetail) + '"/></td>'
                + '</tr>';
        });
        $id('grd').innerHTML = html;
        $id('grdFoot').innerHTML = dtdetail.length
            ? '<td colspan="10">' + dtdetail.length + ' row(s)</td><td class="num">' + upTo(qty, 3) + '</td><td></td><td class="num">' + upTo(gw, 3) + '</td><td colspan="5"></td>'
            : '';
        WeightPerQtyCalculate();
    }

    /** grd_KeyDown F1 :2406 - warehouses that hold a rack for this item. */
    function warehouseOptions(r) {
        var list = distinct((L && L.racks || []).filter(function (x) { return int(x.ItemId) === int(r.ItemId); }), 'WarehouseId');
        var html = '<option value=""></option>', found = false;
        list.forEach(function (w) {
            var on = int(w.WarehouseId) === int(r.WarehouseId); if (on) found = true;
            html += '<option value="' + int(w.WarehouseId) + '"' + (on ? ' selected' : '') + '>' + esc(w.WareHouseName) + '</option>';
        });
        if (!found && int(r.WarehouseId)) html += '<option value="' + int(r.WarehouseId) + '" selected>' + esc(r.Warehouse) + '</option>';
        return html;
    }
    /** grd_KeyDown F1 :2463 - racks of this item, in the chosen warehouse (or any when none chosen). */
    function rackOptions(r) {
        var wid = int(r.WarehouseId);
        var list = distinct((L && L.racks || []).filter(function (x) { return int(x.ItemId) === int(r.ItemId) && (wid === 0 || int(x.WarehouseId) === wid); }), 'Id');
        var html = '<option value=""></option>', found = false;
        list.forEach(function (k) {
            var on = int(k.Id) === int(r.RackId); if (on) found = true;
            html += '<option value="' + int(k.Id) + '"' + (on ? ' selected' : '') + '>' + esc(k.RackName) + (wid ? '' : ' - ' + esc(k.WareHouseName)) + '</option>';
        });
        if (!found && int(r.RackId)) html += '<option value="' + int(r.RackId) + '" selected>' + esc(r.RackName) + '</option>';
        return html;
    }

    /** grd_CellUpdated :2297 - the grid is redrawn after the event settles, never inside it. */
    function onCell(e) { var t = e.target; setTimeout(function () { cellUpdated(t); }, 0); }
    function cellUpdated(t) {
        var f = t.getAttribute('data-f'); if (!f) return;
        var tr = t.closest('tr'); if (!tr) return; var r = dtdetail[int(tr.getAttribute('data-i'))]; if (!r) return;
        if (f === 'PackingDate' || f === 'ExpiryDate') { r[f] = t.value ? t.value + '-01' : ''; return; }   // first of the month
        if (f === 'CityName' || f === 'RemarksDetail') { r[f] = t.value; return; }
        if (f === 'CropYearId' || f === 'ItemConditionId') { r[f] = int(t.value); return; }
        if (f === 'WarehouseId') {
            var wid = int(t.value);
            r.WarehouseId = wid; r.Warehouse = wid ? t.options[t.selectedIndex].textContent : '';
            var racks = distinct((L.racks || []).filter(function (x) { return int(x.ItemId) === int(r.ItemId) && int(x.WarehouseId) === wid; }), 'Id');
            if (r.RackId > 0) {
                if (!racks.some(function (x) { return int(x.Id) === int(r.RackId); })) {
                    if (racks.length === 1) { r.RackId = int(racks[0].Id); r.RackName = racks[0].RackName; }
                    else { r.RackId = 0; r.RackName = ''; }
                }
            } else if (racks.length === 1) { r.RackId = int(racks[0].Id); r.RackName = racks[0].RackName; }
            renderGrid(); return;
        }
        if (f === 'RackId') {
            var rid = int(t.value);
            var hit = (L.racks || []).filter(function (x) { return int(x.Id) === rid && int(x.ItemId) === int(r.ItemId); })[0];
            r.RackId = rid; r.RackName = hit ? hit.RackName : '';
            if (int(r.WarehouseId) === 0 && hit) { r.WarehouseId = int(hit.WarehouseId); r.Warehouse = hit.WareHouseName; }
            renderGrid(); return;
        }
        r[f] = num(t.value);
        if (f === 'ItemQty') { r.GrossWeight = num(r.ItemQty) * num(r.WtPerQty); renderGrid(); focusCell(tr.getAttribute('data-i'), f); return; }
        if (f === 'GrossWeight') { WeightPerQtyCalculate(); updateFoot(); }
    }
    function updateFoot() {
        var qty = 0, gw = 0; dtdetail.forEach(function (r) { qty += num(r.ItemQty); gw += num(r.GrossWeight); });
        var tds = $id('grdFoot').querySelectorAll('td');
        if (tds.length > 3) { tds[1].textContent = upTo(qty, 3); tds[3].textContent = upTo(gw, 3); }
    }
    function focusCell(i, f) { var e = document.querySelector('#grd tr[data-i="' + i + '"] [data-f="' + f + '"]'); if (e) e.focus(); }

    /** grd_ColumnButtonClick :2262. */
    function onGridButton(e) {
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var i = int(b.closest('tr').getAttribute('data-i')); var r = dtdetail[i]; if (!r) return;
        if (b.getAttribute('data-act') === 'del') {
            if (int(r.Id) !== 0) { box("You Can't Delete Already Saved Row......"); return; }
            dtdetail.splice(i, 1);
        } else {
            var copy = {}; for (var k in r) copy[k] = r[k];
            if (Id > 0) copy.Id = 0;                                    // only in update mode (:2285)
            dtdetail.push(copy);
        }
        renderGrid();
    }

    /** WeightPerQtyCalculate :1994. */
    function WeightPerQtyCalculate() {
        if (!dtdetail.length) { setVal('txtAvgWtPerQty', ''); return; }
        var gw = 0, qty = 0; dtdetail.forEach(function (r) { gw += num(r.GrossWeight); qty += num(r.ItemQty); });
        setVal('txtAvgWtPerQty', qty ? String(round2(gw / qty)) : 'NaN');
    }

    // ------------------------------------------------------------------ header events

    function txtsuppwt_TextChanged() { setVal('txtfctwt', val('txtsuppwt').trim()); TotalSupplierWeight(); }
    function TotalSupplierWeight() {
        if (val('txtsuppwt') !== '' && val('txtfctwt') !== '') setVal('txtwtdiff', String(num(val('txtsuppwt')) - num(val('txtfctwt'))));
    }
    /** CmbDeliveryTerm_TextChanged :1937. */
    function CmbDeliveryTerm_TextChanged() {
        var t = selText('CmbDeliveryTerm');
        var ponch = t === 'Ponch' || t === 'Ponch & PartyWeight' || t === 'Ponch & FactoryWeight';
        if (ponch) { setVal('CmbTransport', ''); setVal('txtcarramount', '0'); }
        $id('CmbTransport').disabled = ponch;
        $id('txtcarramount').disabled = ponch;
        refreshCombos();
    }

    // ------------------------------------------------------------------ save

    /** FormValidation :452 - the page half; the server repeats all of it. */
    function FormValidation() {
        var v = val('txtdocno').trim();
        if (v === '' || v === '0') { box('DocNo Field is Required'); return false; }
        if (!int(val('combsupplier'))) { box('Supplier Field is Required'); return false; }
        var dt = selText('CmbDeliveryTerm').trim();
        if (dt === '' || dt === '0') { box('DeliveryTerm Field is Required'); return false; }
        var vn = val('txtvehno').trim();
        if (vn === '' || vn === '0') { box('Vehicle No Field is Required'); return false; }
        if (num(val('txtsuppwt')) === 0 && !cfg.wbNotCompulsory) { box('Supplier Weight Field is Required'); return false; }
        if (num(val('txtfctwt')) === 0 && !IsGeneralGp && !cfg.wbNotCompulsory) { box('Factory Weight Field is Required'); return false; }
        return true;
    }

    function Insert(btnId) {
        if (!FormValidation()) return;
        if (!dtdetail.length) { box('Grid Record Not Found'); return; }
        if (!window.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var carr = num(val('txtcarramount'));
        if (carr > 0 && !int(val('CmbTransport'))) { box('Transporter Account field required'); focus('CmbTransport'); return; }
        if (int(val('CmbTransport')) > 0 && carr === 0) { box('Freight field required'); focus('txtcarramount'); return; }
        var req = {
            Id: Id, DocDate: val('DocDate'), InwardGatePassId: int(val('txtGpId')),
            TransporterValue: int(val('CmbTransport')), CarriageAmount: val('txtcarramount'), RemarksHeader: val('txtremarks'),
            lines: dtdetail.map(function (r) {
                return { Id: int(r.Id), OrderId: int(r.OrderId), OrderDetailId: int(r.OrderDetailId), CropYearId: int(r.CropYearId),
                    PackingDate: dateOnly(r.PackingDate), ExpiryDate: dateOnly(r.ExpiryDate), ItemQty: num(r.ItemQty),
                    GrossWeight: num(r.GrossWeight), SupplierQty: num(r.SupplierQty), CityName: r.CityName || '',
                    WarehouseId: int(r.WarehouseId), RackId: int(r.RackId), ItemConditionId: int(r.ItemConditionId),
                    RemarksDetail: r.RemarksDetail || '' };
            }),
            files: newFiles, removeAttachmentIds: removeAttachmentIds
        };
        return busy(btnId, function () {
            say('Saving...');
            return http('POST', api + '/save', req).then(function (d) {
                say('');
                box(d.message);
                if (d.wagesRequired) box('Wages bill (frmwagesBillHeader) is not available on the web yet - enter it on the desktop for GRN ' + d.docNo + '.');
                var id = d.id;
                return reset().then(function () {
                    if ($id('ChkBox').checked) GenerateReport(id);
                    if ($id('ChkPreviewII').checked) PrintII(id);
                });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }
    function btnSave_Click() { if (Id !== 0 || $id('btnSave').disabled) return; return Insert('btnSave'); }
    function btnUpdate_Click() {
        if (Id === 0) { box('Record not update because Id not found'); return; }
        if ($id('btnUpdate').disabled) return;
        return Insert('btnUpdate');
    }

    /** btnDelete_Click :739. */
    function btnDelete_Click() {
        if (Id <= 0) { box('Record Not Found'); return; }
        if (!perms.Delete || !window.confirm('Are you sure to Delete?')) return;
        return busy('btnDelete', function () {
            return http('POST', api + '/' + Id + '/delete').then(function (d) { box(d.message); return reset(); })
                .catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------ print

    /** GenerateReport :2033 / GrnPmSlipWithSubReport214_01 - the report's rows (Crystal is not rendered). */
    function GenerateReport(id) {
        if (!id) { box('No Record Found For Display'); return; }
        window.open('/api/reports/grn-214/data?id=' + encodeURIComponent(id), '_blank');
    }
    function PrintII(id) {
        if (!id) { box('No Record Found For Display'); return; }
        window.open('/api/reports/grn-214-01/data?id=' + encodeURIComponent(id), '_blank');
    }

    // ------------------------------------------------------------------ reset / read

    /** reset() :1076. */
    function reset() {
        Id = 0; IsGeneralGp = false; BillCalculateTypeId = 0;
        newFiles = []; removeAttachmentIds = []; existingAttachments = []; renderAttachments();
        ['combsupplier', 'txtGpNo', 'txtGpId', 'combvehtyp', 'CmbTransport', 'txtcarramount', 'txtsuppwt', 'txtfctwt', 'txtwtdiff',
            'txtvehno', 'txtbltyno', 'txtremarks', 'CmbDeliveryTerm', 'txtBillCalculateType', 'txtAvgWtPerQty'].forEach(function (k) { setVal(k, ''); });
        $id('txtcarramount').disabled = false; $id('CmbTransport').disabled = false;
        dtdetail = []; renderGrid();
        setVal('DocDate', today());
        applyRights();
        return getJson(api + '/fresh').then(function (d) {
            setVal('txtdocno', d.docNo);
            $id('txtDocNoShow').textContent = 'GRN-' + d.docNo;
            PendingGatePassForGrnLoad(d.pendingGatePasses);
            refreshCombos();
            focus('DocDate');
        }).catch(function (e) { box(e.message); });
    }
    function btnNew_Click() { return busy('btnNew', reset); }

    /** ReadById :775. */
    function ReadById(id) {
        say('Loading...');
        return getJson(api + '/' + id).then(function (d) {
            say('');
            var h = d.header;
            Id = int(col(h, 'Id'));
            setVal('txtdocno', col(h, 'DocNo'));
            $id('txtDocNoShow').textContent = 'GRN-' + col(h, 'DocNo');
            setVal('DocDate', dateOnly(col(h, 'DocDate')));
            showTab(0);
            setVal('combsupplier', int(col(h, 'SupplierCustomerId')));
            var tv = cfg.subsidiaryAccounts ? int(col(h, 'TransporterSupCustId')) : int(col(h, 'TransporterId'));
            setVal('CmbTransport', hasOption('CmbTransport', tv) ? tv : '');
            setVal('txtcarramount', col(h, 'CarriageAmount'));
            setVal('txtsuppwt', col(h, 'PartyWeight'));
            setVal('txtfctwt', col(h, 'FactoryWeight'));
            TotalSupplierWeight();
            selectByText('combvehtyp', col(h, 'VehicleType'));
            setVal('txtvehno', col(h, 'VehicleNo'));
            setVal('txtGpId', col(h, 'InwardGatePassId'));
            setVal('txtGpNo', col(h, 'GpNo'));
            setVal('txtbltyno', col(h, 'BiltyNo'));
            setVal('txtremarks', col(h, 'RemarksHeader'));
            selectByText('CmbDeliveryTerm', col(h, 'DeliveryTerm'));
            CmbDeliveryTerm_TextChanged();                      // :809 - a Ponch term clears the transporter just set
            setBillType(col(h, 'BillCalculateTypeId'));
            IsGeneralGp = !!d.isGeneralGp;
            dtdetail = (d.lines || []).map(function (l) {
                var r = {}; for (var k in l) r[k] = l[k];
                r.OrderDate = dateOnly(l.OrderDate); r.PackingDate = dateOnly(l.PackingDate); r.ExpiryDate = dateOnly(l.ExpiryDate);
                r.CityName = l.CityName || '';
                return r;
            });
            existingAttachments = d.attachments || []; newFiles = []; removeAttachmentIds = []; renderAttachments();
            renderGrid();
            refreshCombos();
            applyRights();
            focus('DocDate');
            return getJson(api + '/fresh?recId=' + Id).then(function (f) { PendingGatePassForGrnLoad(f.pendingGatePasses); });
        }).catch(function (e) { say(''); box(e.message); });
    }

    /** toolStripButton1_Click :2012 - global lists re-read. */
    function toolStripButton1_Click() { return busy('toolStripButton1', loadLookups); }

    // ------------------------------------------------------------------ LoadPurchaseOrderPM

    var loader = (function () {
        var rows = [];
        var COLS = [['BillCalculateTypeId', 'BillCalculateTypeId'], ['DocNo', 'OrderNo'], ['DocDate', 'OrderDate', 'date'],
            ['SupplierName', 'SupplierName'], ['DeliveryTerm', 'DeliveryTerm'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'],
            ['PackingDate', 'PackingDate', 'month'], ['ExpiryDate', 'ExpiryDate', 'month'], ['PackUom', 'PackUom'],
            ['BalQty', 'ItemQty', 'n'], ['WeightPerQty', 'WtPerQty', 'n'], ['RemarksHeader', 'RemarksHeader']];
        function open() {
            $id('dlgLoadPO').classList.add('open');
            setVal('LoaderFromDate', val('LoaderFromDate') || (L && L.financialYearStart) || '');
            setVal('LoaderToDate', val('LoaderToDate') || today());
            return load(true);
        }
        function close() { $id('dlgLoadPO').classList.remove('open'); }
        function load(withSuppliers) {
            var q = '?supplierId=' + int(val('LoaderSupplier')) + '&fromDate=' + encodeURIComponent(val('LoaderFromDate')) + '&toDate=' + encodeURIComponent(val('LoaderToDate'));
            return getJson(api + '/order-loader' + q).then(function (d) {
                if (withSuppliers) fill('LoaderSupplier', d.suppliers, 'Id', 'Description');
                rows = d.rows || [];
                var head = '<th><input type="checkbox" id="loaderAll"/></th>'; COLS.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
                $id('loaderHead').innerHTML = rows.length ? head : '';
                var html = '';
                rows.forEach(function (r, i) {
                    html += '<tr class="data-row"><td><input type="checkbox" data-pick="' + i + '"/></td>';
                    COLS.forEach(function (c) { html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + cell(col(r, c[0]), c[2]) + '</td>'; });
                    html += '</tr>';
                });
                $id('loaderBody').innerHTML = html;
                var all = $id('loaderAll');
                if (all) all.addEventListener('change', function () {
                    Array.prototype.forEach.call(document.querySelectorAll('#loaderBody input[data-pick]'), function (x) { x.checked = all.checked; });
                });
            }).catch(function (e) { box(e.message); });
        }
        function reset() { setVal('LoaderSupplier', ''); return load(false); }
        /** btnLoadOnInvoice_Click - one supplier and one bill type only. */
        function take() {
            var picked = Array.prototype.filter.call(document.querySelectorAll('#loaderBody input[data-pick]'), function (x) { return x.checked; })
                .map(function (x) { return rows[int(x.getAttribute('data-pick'))]; });
            if (!picked.length) { box('Check the Row first'); return; }
            var sup = 0, bill = 0;
            for (var i = 0; i < picked.length; i++) {
                var s = int(col(picked[i], 'OrderSupCustId')), b = int(col(picked[i], 'BillCalculateTypeId'));
                if (!sup) sup = s; if (!bill) bill = b;
                if (sup !== s || bill !== b) { box('You Can Only Select Rows Of Same Supplier and Bill Type'); return; }
            }
            close();
            LoadInGridDetail(picked);
        }
        return { open: open, close: close, load: function () { return load(false); }, reset: reset, take: take };
    })();

    function BtnLoadOrder_Click() { if (IsGeneralGp) loader.open(); }

    // ------------------------------------------------------------------ history

    var H_COLS = [['DocDate', 'DocDate', 'date'], ['DocNo', 'DocNo'], ['InvoiceNo', 'InvoiceNo'], ['SupplierName', 'SupplierName'],
        ['DeliveryTerm', 'DeliveryTerm'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'], ['BiltyNo', 'BiltyNo'],
        ['FactoryWeight', 'FactoryWeight', 'n'], ['PartyWeight', 'PartyWeight', 'n'], ['DiffWeight', 'DiffWeight', 'n'],
        ['Transporter', 'TransporterName'], ['CarriageAmount', 'FreightAmount', 'n'], ['EntryDate', 'EntryDate', 'stamp'],
        ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 'stamp'], ['ModifyUser', 'ModifyUser'],
        ['NoOfAttachments', 'NoOfAttachments'], ['RemarksHeader', 'RemarksHeader']];

    /** HistoryGridFill :1359. */
    function HistoryGridFill() {
        var mode = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var ref = (document.querySelector('input[name="rdRef"]:checked') || {}).value || 'all';
        var q = '?dateMode=' + mode
            + ($id('chkFromDate').checked ? '&fromDate=' + encodeURIComponent(val('FromDateHistory')) : '')
            + ($id('chkToDate').checked ? '&toDate=' + encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory'))
            + '&supplierId=' + int(val('cmbSupplierNameHistory')) + '&referred=' + ref;
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            historyRows.forEach(function (r) { r.DiffWeight = num(col(r, 'FactoryWeight')) - num(col(r, 'PartyWeight')); });
            var head = '<th>Edit</th><th>Print</th><th>PrintII</th>'; H_COLS.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
            $id('histHead').innerHTML = historyRows.length ? head : '';
            var html = '';
            historyRows.forEach(function (r, i) {
                html += '<tr class="data-row" data-h="' + i + '">'
                    + '<td><button type="button" class="win-btn-mini" data-h-act="edit">Edit</button></td>'
                    + '<td><button type="button" class="win-btn-mini" data-h-act="print">Print</button></td>'
                    + '<td><button type="button" class="win-btn-mini" data-h-act="print2">PrintII</button></td>';
                H_COLS.forEach(function (c) { html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + cell(col(r, c[0]), c[2]) + '</td>'; });
                html += '</tr>';
            });
            $id('GrdHistory').innerHTML = html;
            $id('GrdHistoryDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            $id('lblHistCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
        }).catch(function (e) { box(e.message); });
    }
    function btnshowHistory_Click() { return busy('btnshowHistory', HistoryGridFill); }

    function onHistoryClick(e) {
        var tr = e.target.closest('tr[data-h]'); if (!tr) return;
        var r = historyRows[int(tr.getAttribute('data-h'))]; if (!r) return;
        Array.prototype.forEach.call(document.querySelectorAll('#GrdHistory tr'), function (x) { x.classList.toggle('sel', x === tr); });
        var b = e.target.closest('button[data-h-act]');
        var id = int(col(r, 'Id'));
        if (!b) { BindHistoryDetail(id); return; }
        var act = b.getAttribute('data-h-act');
        if (act === 'edit') {                                                                 // :1580
            if (int(col(r, 'InvoiceNo')) > 0) { box("This Document is reffered in invoice. So you can't update this record."); return; }
            if (!perms.Update) return;
            ReadById(id);
        } else if (act === 'print') { if (perms.Print) GenerateReport(id); }
        else if (act === 'print2') { if (perms.Print) PrintII(id); }
    }

    /** BindHistoryDetail :1605. */
    function BindHistoryDetail(id) {
        return getJson(api + '/' + id).then(function (d) {
            var cols = [['OrderDate', 'OrderDate', 'date'], ['OrderNo', 'OrderNo'], ['ItemName', 'ItemName'], ['CropYear', 'CropYear'],
                ['PackingDate', 'PackingDate', 'month'], ['ExpiryDate', 'ExpiryDate', 'month'], ['PackUOM', 'PackUOM'],
                ['ItemCondition', 'ItemCondition'], ['ItemQty', 'ItemQty', 'n'], ['WtPerQty', 'WtPerQty', 'n'],
                ['GrossWeight', 'GrossWeight', 'n'], ['CityName', 'CityName'], ['Warehouse', 'Warehouse'], ['RackName', 'RackName'],
                ['RemarksDetail', 'RemarksDetail']];
            var head = ''; cols.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
            $id('histDetailHead').innerHTML = head;
            var html = '';
            (d.lines || []).forEach(function (l) {
                html += '<tr>'; cols.forEach(function (c) { html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + cell(col(l, c[0]), c[2]) + '</td>'; }); html += '</tr>';
            });
            $id('GrdHistoryDetail').innerHTML = html;
        }).catch(function (e) { box(e.message); });
    }

    function btnNewHistory_Click() {                                                        // :2820
        var back = int(cfg.defaultDaysToLessFromHistoryFromDate);
        setVal('FromDateHistory', addDays(-(back > 0 ? back : 3)));
        setVal('ToDateHistory', today());
        setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', ''); setVal('cmbSupplierNameHistory', '');
        historyRows = []; $id('GrdHistory').innerHTML = ''; $id('histHead').innerHTML = '';
        $id('GrdHistoryDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
        refreshCombos();
    }
    function btnRefreshHistory_Click() {                                                    // :2808
        return busy('btnRefreshHistory', function () {
            return getJson(api + '/lookups').then(function (d) { HistoryComboFill(d.historySuppliers); refreshCombos(); }).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------ attachments

    function btnAttachment_Click() {
        var g = $id('grpAttachments');
        g.style.display = g.style.display === 'none' ? '' : 'none';
    }
    function onAttachmentsPicked(e) {
        Array.prototype.slice.call(e.target.files || []).forEach(function (file) {
            if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }
            var reader = new FileReader();
            reader.onload = function () { var s = String(reader.result); newFiles.push({ name: file.name, base64: s.substring(s.indexOf(',') + 1) }); renderAttachments(); };
            reader.readAsDataURL(file);
        });
        e.target.value = '';
    }
    function renderAttachments() {
        var html = '';
        existingAttachments.forEach(function (a) {
            var id = int(col(a, 'Id')); if (removeAttachmentIds.indexOf(id) >= 0) return;
            html += '<div><a class="win-link" href="' + api + '/' + Id + '/attachments/' + id + '">' + esc(col(a, 'Attachment')) + '</a> <span class="win-link" data-remove="' + id + '">[remove]</span></div>';
        });
        newFiles.forEach(function (f, i) { html += '<div>' + esc(f.name) + ' <em>(new)</em> <span class="win-link" data-drop="' + i + '">[remove]</span></div>'; });
        $id('lstAttachments').innerHTML = html;
    }

    // ------------------------------------------------------------------ tabs, events, keys

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none';
        $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0);
        $id('tabHistory').classList.toggle('active', i === 1);
        focus(i === 1 ? 'FromDateHistory' : 'DocDate');
    }

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function bindEvents() {
        on('grdGp', 'click', function (e) {
            var b = e.target.closest('button[data-gp]'); if (b) LoadGpData(gpRows[int(b.getAttribute('data-gp'))]);
        });
        on('grd', 'change', onCell);
        on('grd', 'click', onGridButton);
        on('grd', 'keydown', function (e) {
            if (e.key !== 'F1') return;
            var f = e.target.getAttribute && e.target.getAttribute('data-f');
            if (f === 'WarehouseId' || f === 'RackId') { e.preventDefault(); try { e.target.showPicker(); } catch (x) { e.target.focus(); } }
        });
        on('GrdHistory', 'click', onHistoryClick);
        on('GrdHistory', 'dblclick', function (e) {                                           // GrdHistory_DoubleClick :1346
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var r = historyRows[int(tr.getAttribute('data-h'))]; if (r && perms.Update) ReadById(int(col(r, 'Id')));
        });
        on('CmbDeliveryTerm', 'change', CmbDeliveryTerm_TextChanged);
        on('fileAttachment', 'change', onAttachmentsPicked);
        on('lstAttachments', 'click', function (e) {
            var rm = e.target.getAttribute('data-remove'), dr = e.target.getAttribute('data-drop');
            if (rm) { removeAttachmentIds.push(int(rm)); renderAttachments(); }
            if (dr !== null && dr !== undefined) { newFiles.splice(int(dr), 1); renderAttachments(); }
        });
        document.addEventListener('keydown', function (e) {                                  // InvFrmGRN_KeyDown :1710
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 's') { e.preventDefault(); if (currentTab === 0) btnSave_Click(); else btnshowHistory_Click(); }
            else if (k === 'u') { e.preventDefault(); btnUpdate_Click(); }
            else if (k === 'n') { e.preventDefault(); if (currentTab === 0) btnNew_Click(); else btnNewHistory_Click(); }
            else if (k === 'r') { e.preventDefault(); if (currentTab === 0) toolStripButton1_Click(); else btnRefreshHistory_Click(); }
            else if (k === 'p') { e.preventDefault(); if (currentTab === 0 && perms.Print) GenerateReport(Id); }
            else if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
            else if (k === 'e') { e.preventDefault(); window.location.href = '/dashboard'; }
            else if (e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (perms.Delete) btnDelete_Click(); }
        });
    }

    window.GrnPm = {
        btnNew_Click: btnNew_Click, toolStripButton1_Click: toolStripButton1_Click,
        btnSave_Click: btnSave_Click, btnUpdate_Click: btnUpdate_Click, btnDelete_Click: btnDelete_Click,
        printToolStripButton_Click: function () { if (perms.Print) GenerateReport(Id); },
        BtnPrintII_Click: function () { if (perms.Print) PrintII(Id); },
        btnAttachment_Click: btnAttachment_Click, BtnLoadOrder_Click: BtnLoadOrder_Click,
        showTab: showTab, btnshowHistory_Click: btnshowHistory_Click,
        btnNewHistory_Click: btnNewHistory_Click, btnRefreshHistory_Click: btnRefreshHistory_Click,
        loader: loader
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
