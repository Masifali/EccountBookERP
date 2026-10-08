/* ============================================================================================
 * Goods Dispatch Notes - screen 142, Architecture.WinApp.Sale.frmGdnAgainstSaleOrder, DocumentTypeId 170
 * Desktop map: Load :341  DeliveryTerm :568  TransportFill :596  WarehouseFill :690  ItemNameFill :726  PackUOMFillWithoutOrder :860
 *   combitem_Leave :926  FormValidation :967  FormDetailValidation :1004  Add_Click :1067  grdSettings :1086  grd_DoubleClick :1171
 *   btnUpdateDetail_Click :1205  Insert :1262  ReadById :1496  btnDelete_Click :1544  reset :1569  ResetDetail :1617
 *   HistoryGridFill :1818  DataGridHistory_ColumnButtonClick :1980  GrossWeight :2158  NetWeight :2188  AvailableStock :2370
 *   KeyDown :2621  BtnShortCutkeys_Click :2705  btnLoadSaleOrder_Click :2555  LoadInGridDetail :2576
 *   frmLoadSaleOrder: BranchFill, HistoryCombosFill, GridSecondViewFill, btnLoadOnInvoice_Click_1
 * The server re-validates everything Insert checks; nothing here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/sale/api/gdn-against-sale-order';
    var L = {}, perms = {}, cfg = {};
    var rows = [], removed = [], hist = [], uoms = [], LD = { rows: [], branches: [], checked: {} };
    var RecId = 0, updateIndex = -1, tab = 0, loaded = false, histLoaded = false, customerLocked = false;

    function $id(x) { return document.getElementById(x); }
    function val(x) { var e = $id(x); return e ? e.value : ''; }
    function setVal(x, v) { var e = $id(x); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function first(row, names) { for (var i = 0; i < names.length; i++) { var v = col(row, names[i]); if (v !== '' && v !== null && v !== undefined) return v; } return ''; }
    function away(x, n) { x = num(x); var f = Math.pow(10, n || 0), y = Math.abs(x) * f; return (x < 0 ? -1 : 1) * Math.floor(y + 0.5 + 1e-9) / f; }
    function fmt(n, d) { return away(n, d).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d || 0 }); }
    function g2(n) { return fmt(n, 2); }                                   /* "#,#.##" */
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    function shortDate(v) { var p = dateOnly(v); return p ? p.slice(8, 10) + '/' + p.slice(5, 7) + '/' + p.slice(0, 4) : ''; }
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
    function fill(x, list, v, t) {
        var sel = $id(x); if (!sel) return;
        var html = '<option value=""></option>';
        (list || []).forEach(function (r) { html += '<option value="' + esc(typeof v === 'function' ? v(r) : col(r, v)) + '">' + esc(typeof t === 'function' ? t(r) : col(r, t)) + '</option>'; });
        sel.innerHTML = html;
    }
    function text(x) { var s = $id(x); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function show(x, on) { var e = $id(x); if (e) e.style.display = on ? '' : 'none'; }
    function enable(x, on) { var e = $id(x); if (e) e.disabled = !on; }
    function focus(x) { var e = $id(x); if (e) try { e.focus(); } catch (z) { /* ignore */ } }
    function on(x, ev, fn) { var e = $id(x); if (e) e.addEventListener(ev, fn); }
    function custName(r) { return first(r, ['CompanyName', 'CustomerName', 'Name']); }
    function byId(list, id, key) { for (var i = 0; i < (list || []).length; i++) if (int(col(list[i], key || 'Id')) === int(id)) return list[i]; return null; }
    function saveMode() { return $id('btnSave').style.display !== 'none'; }

    // ------------------------------------------------------------------ load (Load :341)
    function itemLabel(r) { return $id('rdbtnItemCode').checked ? col(r, 'ItemCode') : col(r, 'ItemName'); }
    function fillItems() {
        var keep = val('combitem');
        fill('combitem', L.items, 'Id', itemLabel);
        setVal('combitem', keep);
        refreshCombos();
    }
    function bindLists() {
        fill('combsupplier', L.customers, 'Id', custName);
        fill('CmbDeliveryTerm', L.deliveryTerms, 'Id', 'type');
        fill('CmbTransport', L.transporters, 'Id', function (r) { return first(r, ['AccountTitle', 'TransporterName', 'CompanyName']); });
        fill('combwr', L.warehouses, 'Id', 'WareHouseName');
        fillItems();
        fill('combcropyear', L.cropYears, 'Id', 'CropYear');
        fill('combjoblot', L.jobLots, 'Id', 'JobLotDescription');
        fill('combpcktyp', L.packingTypes, 'Id', 'PackTypeDesc');
        fill('CmbCity', L.cities, 'Id', function (r) { return first(r, ['CityName', 'City', 'Name']); });
        fill('CmbCustomerHistory', L.historyCustomers, 'Id', 'Customer');
        refreshCombos();
    }
    function defaultConfiguration() {                                    /* defaultConfiquration :444 */
        if (cfg.defaultCity) setVal('CmbCity', int(cfg.defaultCity));
        if (cfg.defaultJobLot) setVal('combjoblot', int(cfg.defaultJobLot));
        if (cfg.defaultCropYear) setVal('combcropyear', int(cfg.defaultCropYear));
        if (cfg.defaultPackingType) setVal('combpcktyp', int(cfg.defaultPackingType));
        if (cfg.defaultWarehouse) setVal('combwr', int(cfg.defaultWarehouse));
        refreshCombos();
    }

    // ------------------------------------------------------------------ calculations
    function equivalent() {
        var id = int(val('CmbPackUom'));
        for (var i = 0; i < uoms.length; i++) if (int(col(uoms[i], 'Id')) === id) return num(col(uoms[i], 'Equivalent'));
        return 0;
    }
    function grossWeight() {                                             /* GrossWeight :2158 */
        var g = equivalent() * num(val('txtqty'));
        setVal('txtgwt', g); setVal('txtnetwt', g); setVal('txtstockwt', g);
    }
    function netWeight() {                                               /* NetWeight :2188 */
        var qty = num(val('txtqty')), gross = num(val('txtgwt')), ebu = away(num(val('txtebu')), 4), wcut = 0, wtot = 0, adls = num(val('txtadlswt'));
        if (val('txtwtcut') !== '' && val('txtqty') !== '') { wcut = num(val('txtwtcut')); wtot = qty * wcut; setVal('txtwtcuttotal', wtot); }
        else { setVal('txtwtcuttotal', 0); }
        var ebt = ebu * qty;
        setVal('txtebt', ebt);
        setVal('txtnetwt', away(gross - ebt - wtot + adls, 2));
        setVal('txtstockwt', away(gross - ebt, 2));
    }
    function qtyChanged() { grossWeight(); netWeight(); }
    function availableStock() {                                          /* AvailableStock :2370 */
        var item = int(val('combitem'));
        if (!item) { $id('lblStock').textContent = '0'; return; }
        var q = '?itemId=' + item + '&cropYear=' + encodeURIComponent(text('combcropyear')) + '&docDate=' + encodeURIComponent(val('DocDate')) +
            '&jobLotId=' + int(val('combjoblot')) + '&warehouseId=' + int(val('combwr'));
        http('GET', api + '/stock' + q).then(function (r) {
            var s = num(r && r.stock);
            $id('lblStock').textContent = s > 0 ? s.toLocaleString('en-US', { maximumFractionDigits: 2 }) : '0';
        }).catch(function (e) { box(e.message); });
    }
    function loadUoms(keepSelection) {                                   /* PackUOMFillWithoutOrder :860 */
        var item = int(val('combitem'));
        if (!item) { uoms = []; fill('CmbPackUom', [], 'Id', 'UOMCode'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/uoms?itemId=' + item).then(function (r) {
            uoms = r || [];
            if (uoms.length) { fill('CmbPackUom', uoms, 'Id', 'UOMCode'); }
            if (saveMode() && !keepSelection) setVal('CmbPackUom', '');
            refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    function itemChanged() { loadUoms(false).then(availableStock); }

    // ------------------------------------------------------------------ detail box
    function detailValid() {                                             /* FormDetailValidation :1004 */
        if (!int(val('combwr'))) { box('Warehouse Field is Required'); focus('combwr'); return false; }
        if (!int(val('combitem'))) { box('Item Field is Required'); focus('combitem'); return false; }
        if (!int(val('combcropyear'))) { box('Crop Year Field is Required'); focus('combcropyear'); return false; }
        if (!int(val('combjoblot'))) { box('Job/Lot Field is Required'); focus('combjoblot'); return false; }
        if (!int(val('combpcktyp'))) { box('Packing Type Field is Required'); focus('combpcktyp'); return false; }
        if (!int(val('CmbPackUom'))) { box('Pack Unit Field is Required'); focus('CmbPackUom'); return false; }
        if (num(val('txtqty')) === 0) { box('Qty Field is Required'); focus('txtqty'); return false; }
        if (num(val('txtgwt')) === 0) { box('Gross Weight Field is Required'); focus('txtgwt'); return false; }
        if (num(val('txtgwt')) < num(val('txtnetwt'))) { box('GrossWeight Not less than NetWeight'); return false; }
        if (num(val('txtnetwt')) < 0) { box('NetBillWeight Field is Empty or Less than zero Please Check'); focus('txtnetwt'); return false; }
        if (num(val('txtstockwt')) < 0) { box('StockWeight Field is Empty or Less than zero Please Check'); focus('txtstockwt'); return false; }
        return true;
    }
    function itemRow() { return byId(L.items, val('combitem')) || {}; }
    function detailToRow(r) {
        var it = itemRow();
        r.WareHouseId = int(val('combwr')); r.WareHouse = text('combwr').trim();
        r.ItemId = int(val('combitem')); r.ItemCode = col(it, 'ItemCode'); r.Item = col(it, 'ItemName');
        r.CropYear = text('combcropyear'); r.JobLotId = int(val('combjoblot')); r.JobLot = text('combjoblot');
        r.PackingTypeId = int(val('combpcktyp')); r.PackingType = text('combpcktyp');
        r.UOMId = int(val('CmbPackUom')); r.UOM = text('CmbPackUom').trim();
        r.Qty = num(val('txtqty')); r.GrossWight = num(val('txtgwt')); r.EbUnit = num(val('txtebu')); r.EbTotal = num(val('txtebt'));
        r.WtCut = num(val('txtwtcut')); r.WtCutTotal = num(val('txtwtcuttotal')); r.AddLesswt = num(val('txtadlswt'));
        r.NetWeight = num(val('txtnetwt')); r.StockWeight = num(val('txtstockwt'));
        r.CityId = int(val('CmbCity')); r.City = text('CmbCity');
        return r;
    }
    function btnAdd_Click() {                                            /* Add_Click :1067 */
        if (!detailValid()) return;
        rows.push(detailToRow({ Id: 0, OrderId: 0, OrderNo: 0, SaleOrderDetailId: 0 }));
        renderGrid(); resetDetail();
    }
    function btnUpdateDetail_Click() {                                   /* btnUpdateDetail_Click :1205 */
        if (!detailValid()) return;
        detailToRow(rows[updateIndex]);
        show('Add', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        renderGrid(); resetDetail();
    }
    function resetDetail() {                                             /* ResetDetail :1617 */
        updateIndex = -1;
        setVal('combjoblot', ''); setVal('combpcktyp', '');
        ['txtqty', 'txtgwt', 'txtebu', 'txtebt', 'txtadlswt', 'txtwtcut', 'txtwtcuttotal', 'txtnetwt', 'txtstockwt'].forEach(function (x) { setVal(x, ''); });
        setVal('CmbPackUom', ''); setVal('combwr', ''); setVal('combitem', '');
        show('Add', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        refreshCombos(); focus('combwr');
    }
    function gridDoubleClick(i) {                                        /* grd_DoubleClick :1171 */
        var r = rows[i]; if (!r) return;
        updateIndex = i;
        setVal('combwr', r.WareHouseId); setVal('combitem', r.ItemId);
        var cy = $id('combcropyear'), found = '';
        for (var k = 0; k < cy.options.length; k++) if (cy.options[k].text === String(r.CropYear)) found = cy.options[k].value;
        setVal('combcropyear', found);
        setVal('combjoblot', r.JobLotId); setVal('combpcktyp', r.PackingTypeId);
        loadUoms(true).then(function () {
            setVal('CmbPackUom', r.UOMId);
            setVal('txtqty', r.Qty); setVal('txtgwt', r.GrossWight); setVal('txtebu', r.EbUnit); setVal('txtebt', r.EbTotal);
            setVal('txtwtcut', r.WtCut); setVal('txtwtcuttotal', r.WtCutTotal); setVal('txtadlswt', r.AddLesswt);
            setVal('txtnetwt', r.NetWeight); setVal('txtstockwt', r.StockWeight); setVal('CmbCity', r.CityId);
            refreshCombos(); availableStock();
        });
        show('Add', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        refreshCombos(); focus('combwr');
    }
    function deleteRow(i) {                                              /* grd_ColumnButtonClick :1147 */
        if (updateIndex !== -1) { box('Reset the Detail First...'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        var r = rows[i];
        if (r && int(r.Id) > 0) removed.push(r);
        rows.splice(i, 1); renderGrid();
    }

    // ------------------------------------------------------------------ grid (grdSettings :1086)
    var GCOLS = [['OrderNo', 'OrderNo', 0], ['WareHouse', 'WareHouse', 0], ['ItemCode', 'ItemCode', 0], ['Item', 'Item', 0], ['CropYear', 'CropYear', 0],
        ['JobLot', 'JobLot', 0], ['PackingType', 'PackingType', 0], ['UOM', 'UOM', 0], ['Qty', 'Qty', 1], ['GrossWight', 'GrossWight', 1],
        ['EbUnit', 'EbUnit', 1], ['EbTotal', 'EbTotal', 1], ['WtCut', 'WtCut', 1], ['WtCutTotal', 'WtCutTotal', 1], ['AddLesswt', 'AddLesswt', 1],
        ['NetWeight', 'NetWeight', 1], ['StockWeight', 'StockWeight', 1], ['City', 'City', 0]];
    function renderGrid() {
        var h = '<th style="width:20px;text-align:center;">X</th>';
        GCOLS.forEach(function (c) { h += '<th' + (c[0] === 'Item' ? ' style="min-width:250px;"' : '') + '>' + esc(c[1]) + '</th>'; });
        $id('grdHead').innerHTML = h;
        var b = '', tot = {};
        rows.forEach(function (r, i) {
            b += '<tr data-i="' + i + '"><td class="ctr"><a class="glink" data-del="' + i + '">X</a></td>';
            GCOLS.forEach(function (c) {
                var v = r[c[0]];
                if (c[2]) { tot[c[0]] = (tot[c[0]] || 0) + num(v); b += '<td class="n">' + g2(v) + '</td>'; }
                else b += '<td>' + esc(v === 0 ? '' : v) + '</td>';
            });
            b += '</tr>';
        });
        $id('grdBody').innerHTML = b;
        var f = '<td></td>';
        GCOLS.forEach(function (c) { f += c[2] ? '<td class="n">' + g2(tot[c[0]] || 0) + '</td>' : '<td></td>'; });
        $id('grdFoot').innerHTML = rows.length ? f : '';
        Array.prototype.forEach.call($id('grdBody').querySelectorAll('tr'), function (tr) {
            tr.addEventListener('dblclick', function () { gridDoubleClick(int(tr.getAttribute('data-i'))); });
        });
        Array.prototype.forEach.call($id('grdBody').querySelectorAll('a[data-del]'), function (a) {
            a.addEventListener('click', function (ev) { ev.stopPropagation(); deleteRow(int(a.getAttribute('data-del'))); });
        });
    }

    // ------------------------------------------------------------------ reset / new
    function nextNo() {
        return http('GET', api + '/next-no').then(function (r) { setVal('txtdocno', r && r.nextNo); }).catch(function (e) { box(e.message); });
    }
    function reset() {                                                   /* reset :1569 */
        RecId = 0; rows = []; removed = []; updateIndex = -1; customerLocked = false;
        ['combsupplier', 'CmbDeliveryTerm', 'CmbTransport', 'combitem', 'CmbPackUom'].forEach(function (x) { setVal(x, ''); });
        ['txtcarramount', 'txtvehno', 'txtbltyno', 'txtremarks', 'txtqty', 'txtebu', 'txtebt', 'txtadlswt', 'txtnetwt', 'txtstockwt', 'txtdocno'].forEach(function (x) { setVal(x, ''); });
        enable('combsupplier', true); enable('combitem', true); enable('CmbDeliveryTerm', true);
        show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); show('Add', true);
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        renderGrid(); $id('lblStock').textContent = '0';
        refreshCombos(); focus('DocDate');
        nextNo(); defaultConfiguration();
    }

    // ------------------------------------------------------------------ read by id (ReadById :1496)
    function mapDetail(d) {
        var wh = byId(L.warehouses, col(d, 'WarehouseId')), jl = byId(L.jobLots, col(d, 'JobLotId')), pk = byId(L.packingTypes, col(d, 'PackingTypeId'));
        var ct = byId(L.cities, col(d, 'CityId')), it = byId(L.items, col(d, 'ItemId'));
        return {
            Id: int(col(d, 'Id')), OrderId: int(col(d, 'SaleOrderId')), OrderNo: first(d, ['SaleOrderNo']) || 0, SaleOrderDetailId: int(col(d, 'SaleOrderDetailId')),
            WareHouseId: int(col(d, 'WarehouseId')), WareHouse: first(d, ['WareHouseCode', 'WareHouseName', 'WarehouseName']) || (wh ? col(wh, 'WareHouseName') : ''),
            ItemId: int(col(d, 'ItemId')), ItemCode: first(d, ['ItemCode']) || (it ? col(it, 'ItemCode') : ''), Item: first(d, ['Item', 'ItemName']) || (it ? col(it, 'ItemName') : ''),
            CropYear: first(d, ['CropYear']), JobLotId: int(col(d, 'JobLotId')), JobLot: first(d, ['JobLot', 'JobLotDescription']) || (jl ? col(jl, 'JobLotDescription') : ''),
            PackingTypeId: int(col(d, 'PackingTypeId')), PackingType: first(d, ['PackingType', 'PackTypeDesc']) || (pk ? col(pk, 'PackTypeDesc') : ''),
            UOMId: int(col(d, 'ItemUomId')), UOM: first(d, ['UOMCode']),
            Qty: num(col(d, 'ItemQty')), GrossWight: num(col(d, 'GrossWeight')), EbUnit: num(col(d, 'EBWPerUnit')), EbTotal: num(col(d, 'EBWTotal')),
            WtCut: num(col(d, 'WtCut')), WtCutTotal: num(col(d, 'WtCutTotal')), AddLesswt: num(col(d, 'AdLsWeight')),
            NetWeight: num(col(d, 'NetBillWeight')), StockWeight: num(col(d, 'StockWeight')),
            CityId: int(col(d, 'CityId')), City: first(d, ['AreaCity']) || (ct ? first(ct, ['CityName', 'City']) : '')
        };
    }
    function readById(id) {
        return http('GET', api + '/' + id).then(function (h) {
            if (!h || !(h.details || []).length) { reset(); throw new Error('Record not found...'); }
            RecId = id; removed = [];
            showTab(0);
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            setVal('txtdocno', col(h, 'DocNo')); setVal('DocDate', dateOnly(col(h, 'DocDate')));
            setVal('combsupplier', col(h, 'SupplierCustomerId')); enable('combsupplier', false);
            var dt = String(col(h, 'DeliveryTerm') || ''), sel = $id('CmbDeliveryTerm'), found = '';
            for (var k = 0; k < sel.options.length; k++) if (sel.options[k].text === dt) found = sel.options[k].value;
            setVal('CmbDeliveryTerm', found);
            setVal('txtremarks', col(h, 'RemarksHeader')); setVal('CmbTransport', col(h, 'TransporterId'));
            setVal('txtcarramount', col(h, 'CarriageAmount')); setVal('txtgpno', col(h, 'GpNo'));
            setVal('txtgpdate', dateOnly(first(h, ['GPDate', 'GpDate'])));
            setVal('txtvehno', col(h, 'VehicleNo')); setVal('txtbltyno', col(h, 'BiltyNo'));
            rows = h.details.map(mapDetail); renderGrid(); refreshCombos();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ save (Insert :1262)
    function formValid() {                                               /* FormValidation :967 */
        var t = val('txtdocno').trim();
        if (t === '' || t === '0') { box('DocNo Field is Required'); focus('txtdocno'); return false; }
        if (!int(val('combsupplier'))) { box('Customer Field is Required'); focus('combsupplier'); return false; }
        if (!int(val('CmbDeliveryTerm'))) { box('Delivery Term Field is Required'); focus('CmbDeliveryTerm'); return false; }
        var g = val('txtgpno').trim();
        if (g === '' || g === '0') { box('GPNo Field is Required'); focus('txtgpno'); return false; }
        return true;
    }
    function rowPayload(r, action) {
        return {
            Id: int(r.Id), SaleOrderId: int(r.OrderId), SaleOrderNo: String(r.OrderNo === 0 ? '' : r.OrderNo), SaleOrderDetailId: int(r.SaleOrderDetailId),
            WarehouseId: int(r.WareHouseId), ItemId: int(r.ItemId), CropYear: String(r.CropYear), JobLotId: int(r.JobLotId),
            PackingTypeId: int(r.PackingTypeId), ItemUomId: int(r.UOMId), ItemQty: num(r.Qty), GrossWeight: num(r.GrossWight),
            EBWPerUnit: num(r.EbUnit), EBWTotal: num(r.EbTotal), WtCut: num(r.WtCut), WtCutTotal: num(r.WtCutTotal), AdLsWeight: num(r.AddLesswt),
            NetBillWeight: num(r.NetWeight), StockWeight: num(r.StockWeight), CityId: int(r.CityId), AreaCity: String(r.City || ''),
            ActionTypeId: action
        };
    }
    function insert() {
        if (!rows.length) { box('Grid Record Not Found'); return; }
        if (!formValid()) return;
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var freight = num(val('txtcarramount')), tr = int(val('CmbTransport')), trSup = 0;
        if (freight > 0) {
            if (!tr) { box('Transporter Account field required'); focus('CmbTransport'); return; }
            if (L.subsidiary) { var t = byId(L.transporters, tr); trSup = t ? int(col(t, 'SupplierCustomerId')) : 0; }
        } else if (tr > 0) { box('Frieght field required'); focus('txtcarramount'); return; }
        for (var i = 0; i < rows.length; i++) {                         /* per-row checks, desktop messages and order */
            var r = rows[i];
            if (!int(r.WareHouseId)) { box('WareHouse Field Required'); return; }
            if (!int(r.ItemId)) { box('ItemName Field Required'); return; }
            if (String(r.CropYear) === '') { box('CropYear Field Required'); return; }
            if (!int(r.JobLotId)) { box('JobLot Field Required'); return; }
            if (!int(r.PackingTypeId)) { box('Packing Type Field Required'); return; }
            if (!int(r.UOMId)) { box('PackUom Field Required'); return; }
            if (num(r.Qty) === 0) { box('Item Qty Field Required'); return; }
            if (num(r.GrossWight) === 0) { box('GrossWight Field Required'); return; }
            if (num(r.NetWeight) === 0) { box('NetWeight Field Required'); return; }
            if (num(r.StockWeight) === 0) { box('StockWeight Field Required'); return; }
            if (String(r.City || '') === '' || !int(r.CityId)) { box('City Field Required'); return; }
        }
        var details = rows.map(function (r) { return rowPayload(r, int(r.Id) > 0 ? 2 : 1); });
        removed.forEach(function (r) { details.push(rowPayload(r, 3)); });
        var body = {
            DocNo: int(val('txtdocno')), DocDate: val('DocDate'), SupplierCustomerId: int(val('combsupplier')), DeliveryTerm: text('CmbDeliveryTerm'),
            RemarksHeader: val('txtremarks').trim(), GpNo: int(val('txtgpno')), GPDate: val('txtgpdate'), VehicleNo: val('txtvehno').trim(),
            BiltyNo: val('txtbltyno').trim(), TransporterId: freight > 0 ? tr : 0, TransporterSupCustId: trSup, CarriageAmount: freight > 0 ? freight : 0,
            details: details, expenses: []
        };
        enable('btnSave', false); enable('btnUpdate', false);
        http(RecId > 0 ? 'PUT' : 'POST', RecId > 0 ? api + '/' + RecId : api, body).then(function (r) {
            box(r.message);
            var id = int(r.id), print = $id('ChkBox').checked;
            reset();
            if (print && id) printSlip(id);
        }).catch(function (e) { box(e.message); }).then(function () { enable('btnSave', true); enable('btnUpdate', true); });
    }
    function printSlip(id) {                                             /* CommonServices.InvGdnSlip260 */
        if (!id) { box('Record not found...'); return; }
        window.open('/api/print/gdn-260/pdf?id=' + id + '&documentTypeId=170', '_blank');
    }

    // ------------------------------------------------------------------ toolbar
    function btnNew_Click() { showTab(0); reset(); }
    function btnRefresh_Click() { window.location.reload(); }
    function btnSave_Click() { RecId = 0; insert(); }
    function btnUpdate_Click() { insert(); }
    function btnDelete_Click() {                                         /* btnDelete_Click :1544 */
        if (!window.confirm('Are you sure to Delete?')) return;
        http('DELETE', api + '/' + RecId).then(function (r) { box((r && r.message) || 'Delete Record Seccessfully'); reset(); }).catch(function (e) { box(e.message); });
    }
    function print_Click() { printSlip(RecId); }
    function btnAttachment_Click() { box('Attachments are not available in the web version of this screen.'); }
    function btnShortcutKeys_Click() {
        box(['Ctrl+N\tFor New', 'Ctrl+S\tFor Save', 'Ctrl+U\tFor Update', 'Ctrl+P\tFor Print', 'Ctrl+T\tFor Switch Between Tabs', 'Ctrl+E / Esc\tFor Close',
            'Ctrl+F5\tFor Focus on Doc Date', 'Ctrl+ArrowDown\tFor Focus On Detail Grid', 'Ctrl+ArrowUp\tFor Focus On Item Combo in Detail Box',
            'Ctrl+ArrowRight\tFor Focus From One Grid To Another', 'Ctrl+Enter\tFor Update Record When Focus On Any Grid'].join('\n'));
    }

    // ------------------------------------------------------------------ history (HistoryGridFill :1818)
    var HCOLS = [['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo', ''], ['CustomerName', 'CustomerName', ''], ['VehicleNo', 'VehicleNo', ''], ['BiltyNo', 'BiltyNo', ''],
        ['AccountTitle', 'Transporter', ''], ['Freight', 'Freight', 'n'], ['UserName', 'EntryUser', ''], ['EntryDate', 'EntryDate', 'd'], ['ModifyUserName', 'ModifyUser', ''],
        ['ModifyDate', 'ModifyDate', 'd'], ['ApprovedUserName', 'ApprovedUser', ''], ['ApprovedDate', 'ApprovedDate', 'd'], ['NoOfAttachments', 'NoOfAttachments', ''],
        ['RemarksHeader', 'Remarks', '']];
    function histMode() { return $id('rdentrydate').checked ? 'entry' : $id('rdmodifydate').checked ? 'modify' : $id('rdapproveddate').checked ? 'approved' : 'doc'; }
    function showHistory() {
        var q = '?mode=' + histMode() + '&from=' + encodeURIComponent(val('FromDateHistory')) + '&to=' + encodeURIComponent(val('ToDateHistory')) +
            '&fromNo=' + int(val('FromDocNoHistory')) + '&toNo=' + int(val('ToDocNoHistory')) + '&customerId=' + int(val('CmbCustomerHistory'));
        http('GET', api + '/history' + q).then(function (r) { hist = r || []; histLoaded = true; renderHistory(); renderHistDetail([]); }).catch(function (e) { box(e.message); });
    }
    function cell(v, kind) {
        if (kind === 'd') return '<td>' + esc(v ? shortDate(v) : '') + '</td>';
        if (kind === 'n') return '<td class="n">' + g2(v) + '</td>';
        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
    }
    function renderHistory() {
        var h = '';
        HCOLS.forEach(function (c) { h += '<th>' + esc(c[1]) + '</th>'; });
        h += '<th>Edit</th><th>Print</th>';
        $id('histHead').innerHTML = h;
        var b = '';
        hist.forEach(function (r, i) {
            b += '<tr data-i="' + i + '">';
            HCOLS.forEach(function (c) { b += cell(col(r, c[0]), c[2]); });
            b += '<td class="ctr"><a class="glink" data-edit="' + i + '">Edit</a></td><td class="ctr"><a class="glink" data-print="' + i + '">Print</a></td></tr>';
        });
        $id('histBody').innerHTML = b;
        Array.prototype.forEach.call($id('histBody').querySelectorAll('tr'), function (tr) {
            tr.addEventListener('click', function () { selectHist(int(tr.getAttribute('data-i'))); });
        });
        Array.prototype.forEach.call($id('histBody').querySelectorAll('a[data-edit]'), function (a) {
            a.addEventListener('click', function (ev) { ev.stopPropagation(); reset(); readById(int(col(hist[int(a.getAttribute('data-edit'))], 'Id'))); });
        });
        Array.prototype.forEach.call($id('histBody').querySelectorAll('a[data-print]'), function (a) {
            a.addEventListener('click', function (ev) { ev.stopPropagation(); printSlip(int(col(hist[int(a.getAttribute('data-print'))], 'Id'))); });
        });
    }
    var DCOLS = [['OrderNo', 'OrderNo', ''], ['WareHouse', 'WareHouse', ''], ['ItemCode', 'ItemCode', ''], ['Item', 'Item', ''], ['PackingType', 'PackingType', ''], ['CropYear', 'CropYear', ''],
        ['JobLot', 'JobLot', ''], ['UOM', 'UOM', ''], ['Qty', 'ItemQty', 'n'], ['GrossWight', 'GrossWeight', 'n'], ['EbUnit', 'EBWPerUnit', 'n'], ['EbTotal', 'EBWTotal', 'n'],
        ['WtCut', 'WeightCut', 'n'], ['WtCutTotal', 'WeightCutTotal', 'n'], ['AddLesswt', 'AdLsWeight', 'n'], ['NetWeight', 'NetBillWeight', 'n'], ['StockWeight', 'StockWeight', 'n'], ['City', 'City', '']];
    function renderHistDetail(list) {
        var h = '', b = '';
        DCOLS.forEach(function (c) { h += '<th>' + esc(c[1]) + '</th>'; });
        $id('detHead').innerHTML = h;
        list.forEach(function (r) { b += '<tr>'; DCOLS.forEach(function (c) { b += cell(r[c[0]], c[2]); }); b += '</tr>'; });
        $id('detBody').innerHTML = b;
    }
    function selectHist(i) {
        Array.prototype.forEach.call($id('histBody').querySelectorAll('tr'), function (tr) { tr.className = int(tr.getAttribute('data-i')) === i ? 'sel' : ''; });
        http('GET', api + '/' + int(col(hist[i], 'Id'))).then(function (h) { renderHistDetail((h.details || []).map(mapDetail)); }).catch(function (e) { box(e.message); });
    }
    function btnRefreshHistory_Click() { setVal('FromDocNoHistory', ''); setVal('ToDocNoHistory', ''); setVal('CmbCustomerHistory', ''); initHistoryDates(); refreshCombos(); showHistory(); }
    function initHistoryDates() {
        var d = new Date(), days = int(cfg.historyDays) || 3;
        setVal('ToDateHistory', iso(d)); d.setDate(d.getDate() - days); setVal('FromDateHistory', iso(d));
    }

    function showTab(n) {
        tab = n;
        $id('tabPage1').style.display = n === 0 ? '' : 'none'; $id('tabPage2').style.display = n === 1 ? '' : 'none';
        $id('tabForm').className = n === 0 ? 'on' : ''; $id('tabHistory').className = n === 1 ? 'on' : '';
        if (n === 1 && !histLoaded) { initHistoryDates(); showHistory(); }
    }

    // ------------------------------------------------------------------ frmLoadSaleOrder (second view, MainDetail false)
    var LCOLS = [['DocumentType', 'OrderType', ''], ['DocNo', 'OrderNo', ''], ['DocDate', 'DocDate', 'd'], ['OrderExpiryDate', 'Expiry Date', 'd'], ['BranchName', 'BranchName', 'b'],
        ['CustomerName', 'PartyName', ''], ['ItemName', 'ItemName', ''], ['PackUom', 'PackUOM', ''], ['OrderQTY', 'OrderQTY', 'n'], ['DispatchQty', 'DispatchQty', 'n'],
        ['BalQty', 'BalQty', 'n'], ['OrderWeight', 'OrderWeight', 'n'], ['DispatchWeight', 'DispatchedWeight', 'n'], ['BalWeight', 'BalWeight', 'n'], ['ItemRate', 'ItemRate', 'n']];
    function selectedBranchIds() {
        var s = ''; LD.branches.forEach(function (b) { if (LD.checked[b.id]) s += ',' + b.id; });
        return s;
    }
    function branchText() {
        var n = []; LD.branches.forEach(function (b) { if (LD.checked[b.id]) n.push(b.name); });
        $id('ldBranchText').value = n.join(',');
    }
    function renderBranches() {
        var h = '';
        LD.branches.forEach(function (b) { h += '<label><input type="checkbox" data-b="' + b.id + '"' + (LD.checked[b.id] ? ' checked' : '') + '/> ' + esc(b.name) + '</label>'; });
        var l = $id('ldBranchList'); l.innerHTML = h;
        Array.prototype.forEach.call(l.querySelectorAll('input'), function (c) {
            c.addEventListener('change', function () { LD.checked[c.getAttribute('data-b')] = c.checked; branchText(); loaderCombos(); });
        });
        branchText();
    }
    function loaderCombos() {
        return http('GET', api + '/loader/combos?branchIds=' + encodeURIComponent(selectedBranchIds())).then(function (r) {
            fill('ldCustomer', r.customers, 'Id', 'Name'); fill('ldItem', r.items, 'Id', 'Name'); refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    function btnLoadSaleOrder_Click() {                                  /* btnLoadSaleOrder_Click :2555 */
        http('GET', api + '/loader/setup').then(function (s) {
            LD.branches = (s.branches || []).map(function (b) { return { id: String(col(b, 'Id')), name: String(col(b, 'BranchName')) }; });
            LD.checked = {}; LD.rows = [];
            if (LD.branches.length === 1) LD.checked[LD.branches[0].id] = true;                       /* single branch: Rows[0].Activate() */
            else if (s.userBranchId) LD.checked[String(s.userBranchId)] = true;                         /* Text = UserAccount.BranchName */
            setVal('ldFrom', cfg.yearStart || today()); setVal('ldTo', today());
            renderBranches();
            $id('loaderModal').style.display = 'flex';
            loaderCombos().then(loaderSearch);
        }).catch(function (e) { box(e.message); });
    }
    function loaderClose() { $id('loaderModal').style.display = 'none'; }
    function loaderSearch() {                                            /* GridSecondViewFill */
        var ids = selectedBranchIds();
        if (!ids) { box('Select branch first'); return; }
        var q = '?branchIds=' + encodeURIComponent(ids) + '&customerId=' + int(val('ldCustomer')) + '&itemId=' + int(val('ldItem')) +
            '&from=' + encodeURIComponent(val('ldFrom')) + '&to=' + encodeURIComponent(val('ldTo'));
        http('GET', api + '/loader/search' + q).then(function (r) { LD.rows = r || []; renderLoader(); }).catch(function (e) { box(e.message); });
    }
    function renderLoader() {
        var h = '<th style="width:24px;"><input type="checkbox" id="ldAll"/></th>', b = '', tot = {};
        LCOLS.forEach(function (c) { if (c[2] === 'b' && cfg.branchWiseOrders) return; h += '<th>' + esc(c[1]) + '</th>'; });
        $id('loaderHead').innerHTML = h;
        LD.rows.forEach(function (r, i) {
            b += '<tr><td class="ctr"><input type="checkbox" data-r="' + i + '"/></td>';
            LCOLS.forEach(function (c) {
                if (c[2] === 'b' && cfg.branchWiseOrders) return;
                var v = col(r, c[0]);
                if (c[2] === 'n') { tot[c[0]] = (tot[c[0]] || 0) + num(v); b += '<td class="n">' + fmt(v, c[0] === 'ItemRate' ? 2 : 0) + '</td>'; }
                else if (c[2] === 'd') b += '<td>' + esc(v ? shortDate(v) : '') + '</td>';
                else b += '<td>' + esc(v) + '</td>';
            });
            b += '</tr>';
        });
        $id('gridLoader').innerHTML = b;
        var f = '<td></td>';
        LCOLS.forEach(function (c) { if (c[2] === 'b' && cfg.branchWiseOrders) return; f += c[2] === 'n' && c[0] !== 'ItemRate' ? '<td class="n">' + fmt(tot[c[0]] || 0, 0) + '</td>' : '<td></td>'; });
        $id('loaderFoot').innerHTML = LD.rows.length ? f : '';
        var all = $id('ldAll');
        if (all) all.addEventListener('change', function () {
            Array.prototype.forEach.call($id('gridLoader').querySelectorAll('input[data-r]'), function (c) { c.checked = all.checked; });
        });
    }
    function loaderLoad() {                                              /* btnLoadOnInvoice_Click_1 + LoadInGridDetail */
        var ids = [];
        Array.prototype.forEach.call($id('gridLoader').querySelectorAll('input[data-r]'), function (c) {
            if (c.checked) ids.push(int(col(LD.rows[int(c.getAttribute('data-r'))], 'Id')));
        });
        http('POST', api + '/loader/load', { orderIds: ids }).then(function (list) {
            loaderClose();
            if (!list || !list.length) { window.location.reload(); return; }
            var h = list[0];
            rows = [];
            setVal('combsupplier', col(h, 'SupplierCustomerId')); enable('combsupplier', false); enable('combitem', false);
            var sel = $id('CmbDeliveryTerm'), found = '';
            for (var k = 0; k < sel.options.length; k++) if (sel.options[k].text === String(col(h, 'DeliveryTerm'))) found = sel.options[k].value;
            setVal('CmbDeliveryTerm', found); enable('CmbDeliveryTerm', false);
            list.forEach(function (r) {
                var gw = num(col(r, 'GrossWeight'));
                rows.push({ Id: 0, OrderId: int(col(r, 'OrderId')), OrderNo: col(r, 'OrderNo'), SaleOrderDetailId: int(col(r, 'SaleOrderDetailId')), WareHouseId: 0, WareHouse: '',
                    ItemId: int(col(r, 'ItemId')), ItemCode: col(r, 'ItemCode'), Item: col(r, 'Item'), CropYear: col(r, 'CropYear'),
                    JobLotId: int(col(r, 'JobLotId')), JobLot: col(r, 'JobLotDescription'), PackingTypeId: int(col(r, 'PackingTypeId')), PackingType: col(r, 'PackTypeDesc'),
                    UOMId: int(col(r, 'ItemUOMId')), UOM: col(r, 'ItemUOM'), Qty: num(col(r, 'QTY')), GrossWight: gw, EbUnit: 0, EbTotal: 0, WtCut: 0, WtCutTotal: 0, AddLesswt: 0,
                    NetWeight: gw, StockWeight: gw, CityId: int(col(r, 'CityId')), City: col(r, 'CityArea') });
            });
            renderGrid(); refreshCombos();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ keys (KeyDown :2621)
    function onKey(e) {
        var k = e.key, ctrl = e.ctrlKey;
        if ($id('loaderModal').style.display === 'flex') { if (k === 'Escape' || (ctrl && (k === 'e' || k === 'E'))) { e.preventDefault(); loaderClose(); } return; }
        if (k === 'Enter' && !ctrl && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && e.target.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('#frm input:not([readonly]):not([disabled]):not([type=hidden]), #frm select:not([disabled])'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i < f.length - 1) { e.preventDefault(); f[i + 1].focus(); }
            return;
        }
        if (!ctrl) return;
        var key = k.toLowerCase();
        if (key === 't') { e.preventDefault(); showTab(tab === 0 ? 1 : 0); if (tab === 0) focus('DocDate'); else focus('FromDateHistory'); }
        else if (key === 'n') { e.preventDefault(); btnNew_Click(); }
        else if (key === 'p') { e.preventDefault(); print_Click(); }
        else if (key === 's' && saveMode()) { e.preventDefault(); btnSave_Click(); }
        else if (key === 'u' && $id('btnUpdate').style.display !== 'none') { e.preventDefault(); btnUpdate_Click(); }
        else if (k === 'ArrowDown') { e.preventDefault(); var g = $id('panel6'); if (g) { g.tabIndex = 0; g.focus(); } }
        else if (k === 'ArrowUp') { e.preventDefault(); focus('combitem'); }
        else if (k === 'F5') { e.preventDefault(); focus('DocDate'); }
    }

    // ------------------------------------------------------------------ init
    function init() {
        renderGrid();
        $id('DocDate').value = today(); $id('txtgpdate').value = today();
        on('Add', 'click', btnAdd_Click); on('btnUpdateDetail', 'click', btnUpdateDetail_Click); on('btnCancelUpdateDetial', 'click', function () {
            show('Add', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); resetDetail();
        });
        on('btnShowHistory', 'click', showHistory);
        on('txtqty', 'input', qtyChanged); on('txtqty', 'blur', qtyChanged);
        on('CmbPackUom', 'change', qtyChanged);
        on('txtgwt', 'input', netWeight); on('txtebu', 'input', netWeight); on('txtwtcut', 'input', netWeight); on('txtadlswt', 'input', netWeight);
        on('combitem', 'change', itemChanged);
        on('combwr', 'change', availableStock); on('combcropyear', 'change', availableStock); on('combjoblot', 'change', availableStock); on('combpcktyp', 'change', availableStock);
        on('rdbtnItemName', 'change', fillItems); on('rdbtnItemCode', 'change', fillItems);
        on('ldBranchText', 'click', function () { var l = $id('ldBranchList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        ['txtgwt', 'txtebu', 'txtwtcut', 'txtqty', 'txtcarramount', 'txtadlswt'].forEach(function (x) {                     /* OnlytextdecimelFunction */
            on(x, 'keypress', function (e) { if (e.key.length === 1 && !/[0-9.\-]/.test(e.key)) e.preventDefault(); });
        });
        ['FromDocNoHistory', 'ToDocNoHistory', 'txtgpno'].forEach(function (x) {
            on(x, 'keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        });
        document.addEventListener('keydown', onKey);
        http('GET', api + '/lookups').then(function (r) {
            L = r || {}; perms = L.rights || {}; cfg = L.config || {}; cfg.yearStart = L.yearStart;
            setVal('txtdocno', L.nextNo);
            bindLists(); defaultConfiguration();
            loaded = true;
        }).catch(function (e) { box(e.message); });
    }

    window.SGDN = {
        showTab: showTab, btnNew_Click: btnNew_Click, btnRefresh_Click: btnRefresh_Click, btnSave_Click: btnSave_Click, btnUpdate_Click: btnUpdate_Click,
        btnDelete_Click: btnDelete_Click, print_Click: print_Click, btnAttachment_Click: btnAttachment_Click, btnShortcutKeys_Click: btnShortcutKeys_Click,
        btnLoadSaleOrder_Click: btnLoadSaleOrder_Click, loaderClose: loaderClose, loaderSearch: loaderSearch, loaderLoad: loaderLoad,
        btnRefreshHistory_Click: btnRefreshHistory_Click
    };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
