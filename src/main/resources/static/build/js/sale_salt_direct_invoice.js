/*
 * Screen 585  frmSaleDirectInvoice  (Architecture.WinApp.SaleForSalt.frmSaleDirectInvoice, Sale Salt - Sale Direct Invoice, document type 1809)
 * Page script. Desktop methods are named in the comments (frmSaleDirectInvoice.cs). Server: /sale/salt/sale-direct-invoice/api
 *
 * The totals of the desktop are recalculated by its text box / grid events; the server holds the same handlers (SaleSaltDirectCalc) and answers the
 * new state of the form for every event ("calc"), so the figures cannot differ between the screen and the save.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/salt/sale-direct-invoice/api', DOC_TYPE = 1809, fail = SS.fail;
    var S = { id: 0, base: null, rights: {}, fm: { amtDec: 2, fcyDec: 2, rateDec: 2 }, files: [], removed: [], existing: [], vhId: 0, multi: false, outward: 0, itemsByWh: false, wages: false,
              customers: [], accounts: [], otherItems: [], warehouses: [], items: [], defaults: {}, histSeq: 0, historyDays: 3, uomItem: -1, uoms: [], supplierGlId: 0, trId: 0, q: Promise.resolve() };
    var cb = {}, G = {}, tabs;

    function tn(v) { return SE.toNum(v); }
    function ti(v) { var n = Number(String(v == null ? '' : v).replace(/,/g, '')); return isFinite(n) ? Math.trunc(n) : 0; }
    function msg(t, cap) { return SE.alert(t, cap); }
    function show(id, on) { var b = $(id); if (b) b.style.display = on ? '' : 'none'; }
    function visible(id) { return SS.visible(id); }
    function setCombo(c, id, key) { var k = key || 'Id', hit = id != null && id !== '' && +id !== 0 && c.rows().some(function (r) { return String(r[k]) === String(id); }); if (hit) c.setValue(id); else c.clear(); }
    function setText(c, text, key) {                                                       // ComboBox.Text = value
        var hit = null; c.rows().forEach(function (r) { if (hit == null && String(r[key]) === String(text)) hit = r; });
        if (hit) c.setValue(hit.Id); else c.clear();
    }
    function amt(v) { return SE.num(v, S.fm.amtDec, S.fm.amtDec); }
    function fcy(v) { return SE.num(v, S.fm.fcyDec, S.fm.fcyDec); }
    function rate(v) { return SE.num(v, S.fm.rateDec, S.fm.rateDec); }
    function q2(v) { return v == null || v === '' ? '' : SE.num(v, 2, 0); }
    function q3(v) { return v == null || v === '' ? '' : SE.num(v, 3, 0); }
    function plain(v) { return v == null || v === '' ? '' : String(v); }
    function clean(rows) { return JSON.parse(JSON.stringify(rows, function (k, v) { return k.charAt(0) === '_' ? undefined : v; })); }
    function setv(id, v) { var el = $(id); v = v == null ? '' : String(v); if (el.value !== v) el.value = v; }
    function esc(s) { return String(s == null ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;'); }
    function byId(rows, id) { for (var i = 0; i < rows.length; i++) if (+rows[i].Id === +id) return rows[i]; return null; }

    /* ------------------------------------------------------------------ combos */
    function stock() { S.q = S.q.then(refreshStock).catch(fail); }
    function itemRows() { var code = $('rdCode').checked; return S.items.map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.Show = code ? r.ItemCode : r.ItemName; return o; }); }
    function bindItems() { var v = cb.item.value(); cb.item.setData(itemRows()); if (v) setCombo(cb.item, v); }
    function makeCombos() {
        cb.cust = XCombo('comsupplier', { columns: [{ key: 'CompanyName', caption: 'Supplier Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: function () { calc('supplier'); } });
        cb.agent = XCombo('combcommAgent', { columns: [{ key: 'CompanyName', caption: 'Commission Agent' }], textKey: 'CompanyName', popupWidth: 420, onSelect: function () { calc('agent'); } });
        cb.term = XCombo('combpttrm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: function () { calc('pterm'); } });
        cb.dterm = XCombo('combdeliverytrm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm', onSelect: function () { calc('delTerm'); } });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode', onSelect: function () { calc('curLeave'); } });
        cb.type = XCombo('CmbCommType', { columns: [{ key: 'CommissionType', caption: 'CommissionType' }], textKey: 'CommissionType', onSelect: function () { calc('comm'); } });
        cb.uom = XCombo('CmbCommUom', { columns: [{ key: 'Uom', caption: 'UOM' }], textKey: 'Uom', onSelect: function () { calc('comm'); } });
        cb.tr = XCombo('CmbTransporterAc', { columns: [{ key: 'AccountTitle', caption: 'Account' }], textKey: 'AccountTitle', popupWidth: 420, onSelect: function () { S.trId = +cb.tr.value() || 0; } });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouse' }], textKey: 'WareHouseName', popupWidth: 300, onSelect: warehouseChanged });
        cb.item = XCombo('CmbItem', { columns: [{ key: 'ItemName', caption: 'Item Name' }, { key: 'ItemCode', caption: 'Item Code' }], textKey: 'Show', popupWidth: 460, onSelect: itemChanged });
        cb.lot = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', onSelect: stock });
        cb.pack = XCombo('CmbPackingType', { columns: [{ key: 'PackTypeDesc', caption: 'Packing Type' }], textKey: 'PackTypeDesc', onSelect: stock });
        cb.puom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'UOM' }], textKey: 'UOMCode', onSelect: function () { calc('packuom').then(refreshStock).catch(fail); } });
        cb.ruom = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'UOM' }], textKey: 'UOMCode', onSelect: function () { calc('rateuom'); } });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City' }], textKey: 'CityName', popupWidth: 300 });
        cb.hcust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Customer Name' }], textKey: 'Customer', popupWidth: 380 });
    }

    /* the warehouse combo re-reads the items only when the configuration GetItemsagainstWarehouse is true (item() is called again; the desktop never clears dtitem) */
    function warehouseChanged() {
        if (S.itemsByWh) {
            S.q = S.q.then(function () { return SE.api(API + '/items' + SE.q({ warehouseId: +cb.wh.value() || 0 }), { quiet: true }); })
                .then(function (rows) { S.items = S.items.concat(rows); bindItems(); }).catch(fail);
        }
        stock();
    }

    /* CmbItem_Leave -> PackUOM() (rebinds the two UOM combos, keeps the text when the new item has it) + AvailableStockGetByItem */
    function bindUoms(itemId) {
        var keepP = cb.puom.text(), keepR = cb.ruom.text();
        S.uomItem = itemId;
        if (!(itemId > 0)) { cb.puom.setData([]); cb.ruom.setData([]); return Promise.resolve(); }
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId }), { quiet: true }).then(function (rows) {
            S.uoms = rows; cb.puom.setData(rows); cb.ruom.setData(rows);
            setText(cb.puom, keepP, 'UOMCode'); setText(cb.ruom, keepR, 'UOMCode');
        });
    }
    function itemChanged() { S.q = S.q.then(function () { return bindUoms(+cb.item.value() || 0); }).then(refreshStock).catch(fail); }
    function refreshStock() {
        var q = { warehouseId: +cb.wh.value() || 0, itemId: +cb.item.value() || 0, jobLotId: +cb.lot.value() || 0, packingTypeId: +cb.pack.value() || 0, uomId: +cb.puom.value() || 0, docDate: $('DocDate').value };
        return SE.api(API + '/stock' + SE.q(q), { quiet: true }).then(function (d) { $('lblBalance').textContent = d.balance == null ? '' : 'Balance: ' + d.balance; });
    }

    /* ------------------------------------------------------------------ the state of the form <-> the server */
    function entry() {
        var it = byId(S.items, cb.item.value()) || {};
        return {
            warehouseId: +cb.wh.value() || 0, warehouseText: cb.wh.text(), itemId: +cb.item.value() || 0, itemText: it.ItemName || '', itemCode: it.ItemCode || '',
            jobLotId: +cb.lot.value() || 0, jobLotText: cb.lot.text(), packingTypeId: +cb.pack.value() || 0, packingText: cb.pack.text(),
            packUomId: +cb.puom.value() || 0, packUomText: cb.puom.text(), rateUomId: +cb.ruom.value() || 0, rateUomText: cb.ruom.text(), cityId: +cb.city.value() || 0, cityText: cb.city.text(),
            qty: $('txtQty').value, gross: $('txtGrossWeight').value, addLss: $('txtAddLss').value, netBill: $('txtNetBillWeight').value, stock: $('txtStockWeight').value,
            rate: $('txtRate').value, rateCutTotal: $('txtratecuttotal').value, amount: $('txtAmount').value, gpDate: $('txtgpdate').value, gpNo: $('txtgatepassno').value,
            vehicleNo: $('txtvehicleno').value, remarks: $('txtRemarksDetail').value
        };
    }
    function collect() {
        var o = S.base ? JSON.parse(JSON.stringify(S.base)) : {};
        o.id = S.id; o.docNo = $('txtdocno').value; o.docDate = $('DocDate').value; o.supplierId = +cb.cust.value() || 0; o.supplierGlId = S.supplierGlId;
        o.refNo = $('txtSupplierReference').value; o.billNo = $('txtBillNo').value; o.paymentTermId = +cb.term.value() || 0; o.dueDays = $('txtduedays').value; o.dueDate = $('duedate').value;
        o.deliveryTerm = cb.dterm.text(); o.currencyId = +cb.cur.value() || 0; o.exchangeRate = $('txtExchangeRate').value; o.invoiceQty = $('txtInvoiceQty').value;
        o.invoiceWeight = $('txtInvoiceWeight').value; o.fcyAmount = $('txtFcyAmount').value; o.billAmount = $('txtBillAmount').value; o.commAgentId = +cb.agent.value() || 0;
        o.commType = cb.type.text(); o.commRate = $('txtcommrate').value; o.commUom = cb.uom.text(); o.commAmount = $('txtCommAmount').value; o.commRemarks = $('txtCommissionRemarks').value;
        o.remarks = $('txtRemarks').value; o.transporterId = S.trId; o.freightText = $('txtFreightAmount').value; o.resetOnSave = $('ChkResetOnSave').checked;
        o.e = entry(); o.lines = clean(G.grd.rows()); o.exp = clean(G.ex.rows()); o.gl = clean(G.gl.rows());
        delete o.messages; delete o.focus;
        return o;
    }

    /* the column visibility / editing that depends on OrderExist (the first row came from a sale order) and on the multi currency feature */
    function grdSettings() {
        var ord = !!(S.base && S.base.orderExist), editable = { WarehouseId: 1, ItemQty: 1, GrossWeight: 1, AddLss: 1, GpDate: 1, GpNo: 1, VehicleNo: 1, StockWeight: 1, RateCutAmount: 1 };
        G.grd.spec.cols.forEach(function (c) {
            if (c.k === 'OrderNo' || c.k === 'Add') c.hide = !ord;
            if (c.k === 'FcyAmount') c.hide = !S.multi;
            if (editable[c.k]) c.edit = ord;
        });
        G.grd.refresh();
    }
    var FOCUS = function () { return { CmbItem: cb.item, CmbJobLot: cb.lot, CmbPackingType: cb.pack, CmbPackUom: cb.puom, CmbRateUom: cb.ruom, CmbWarehouse: cb.wh, CmbCity: cb.city, CmbCommType: cb.type,
        comsupplier: cb.cust, combpttrm: cb.term, combdeliverytrm: cb.dterm, cmbCurrency: cb.cur, combcommAgent: cb.agent }; };
    function focusOn(name) {
        if (!name) return;
        var m = FOCUS(); if (m[name]) m[name].focus(); else if ($(name)) $(name).focus();
    }

    /* the texts and grids that the events change; full = also the combos of the header (a new / loaded record) */
    function applyState(st, full) {
        var act = document.activeElement && document.activeElement.id;
        function put(id, v) { if (full || act !== id) setv(id, v); }
        var b = JSON.parse(JSON.stringify(st)); delete b.lines; delete b.exp; delete b.gl; delete b.e; delete b.messages; delete b.focus; S.base = b;
        S.id = st.id || 0; S.supplierGlId = st.supplierGlId || 0; S.trId = st.transporterId || 0;
        put('txtInvoiceQty', st.invoiceQty); put('txtInvoiceWeight', st.invoiceWeight); put('txtFcyAmount', st.fcyAmount); put('txtBillAmount', st.billAmount);
        put('txtCommAmount', st.commAmount); put('txtduedays', st.dueDays); setv('duedate', st.dueDate); put('txtExchangeRate', st.exchangeRate); put('txtcommrate', st.commRate);
        put('txtFreightAmount', st.freightText); setCombo(cb.tr, st.transporterId);
        if (full) {
            setv('txtdocno', st.docNo); if (st.docDate) $('DocDate').value = st.docDate;
            setCombo(cb.cust, st.supplierId); setv('txtSupplierReference', st.refNo); setv('txtBillNo', st.billNo);
            setText(cb.dterm, st.deliveryTerm, 'DeliveryTerm'); setCombo(cb.term, st.paymentTermId); setCombo(cb.cur, st.currencyId);
            setCombo(cb.agent, st.commAgentId); setText(cb.type, st.commType, 'CommissionType'); setText(cb.uom, st.commUom, 'Uom');
            setv('txtCommissionRemarks', st.commRemarks); setv('txtRemarks', st.remarks);
        }
        /* enabled states */
        SS.dis(cb.cust, !st.supplierEnabled); SS.dis(cb.term, !st.paymentTermEnabled); SS.dis(cb.dterm, !st.deliveryTermEnabled); SS.dis(cb.item, !st.itemEnabled);
        $('txtduedays').disabled = !st.dueDaysEnabled; SS.dis(cb.type, !st.commTypeEnabled); $('txtcommrate').disabled = !st.commRateEnabled; SS.dis(cb.uom, !st.commUomEnabled);
        $('txtCommAmount').disabled = !st.commAmountEnabled;
        G.grd.setRows(st.lines || []); G.ex.setRows(st.exp || []); G.gl.setRows(st.gl || []);
        grdSettings();
        var upd = (st.updateIndex == null ? -1 : st.updateIndex) >= 0;
        show('btnAdd', !upd); show('btnUpdateDetail', upd); show('btnCancelUpdateDetial', upd);
        show('btnSave', st.saveMode !== false); show('btnUpdate', st.saveMode === false);
        return applyEntry(st.e || {}, full).then(function () { focusOn(st.focus); });
    }
    function applyEntry(e, full) {
        var p = Promise.resolve(), act = document.activeElement && document.activeElement.id;
        if ((e.itemId || 0) !== S.uomItem && (e.itemId || 0) > 0) {                       // edit of a row: the UOM combos need the schedule of its item
            p = SE.api(API + '/uoms' + SE.q({ itemId: e.itemId }), { quiet: true }).then(function (rows) { S.uoms = rows; S.uomItem = e.itemId; cb.puom.setData(rows); cb.ruom.setData(rows); });
        } else if (!(e.itemId > 0) && S.uomItem !== 0) { S.uomItem = 0; cb.puom.setData([]); cb.ruom.setData([]); }
        return p.then(function () {
            function put(id, v) { if (full || act !== id) setv(id, v); }
            setCombo(cb.wh, e.warehouseId); setCombo(cb.item, e.itemId); setCombo(cb.lot, e.jobLotId); setCombo(cb.pack, e.packingTypeId); setCombo(cb.puom, e.packUomId);
            setCombo(cb.ruom, e.rateUomId); setCombo(cb.city, e.cityId);
            put('txtQty', e.qty); put('txtGrossWeight', e.gross); put('txtAddLss', e.addLss); put('txtNetBillWeight', e.netBill); put('txtStockWeight', e.stock); put('txtRate', e.rate);
            put('txtratecuttotal', e.rateCutTotal); put('txtAmount', e.amount); put('txtgpdate', e.gpDate); put('txtgatepassno', e.gpNo); put('txtvehicleno', e.vehicleNo);
            put('txtRemarksDetail', e.remarks);
        });
    }
    function showMsgs(list) {
        var p = Promise.resolve();
        (list || []).forEach(function (m) { p = p.then(function () { return msg(m, 'Message'); }); });
        return p;
    }

    /* one desktop event on the server; calls are queued so the answers cannot overtake each other */
    function calc(ev, row, col, key, yes, full) {
        S.q = S.q.then(function () {
            var body = { event: ev, row: row == null ? -1 : row, col: col || '', key: key || '', yes: !!yes, state: collect() };
            return SE.api(API + '/calc', { method: 'POST', body: body, quiet: true }).then(function (st) { return applyState(st, !!full).then(function () { return showMsgs(st.messages); }); });
        }).catch(fail);
        return S.q;
    }

    /* ------------------------------------------------------------------ grids */
    function numCol(k, t, w, fn, extra) { var c = { k: k, t: t || k, w: w || 90, cls: 'num', render: fn || q2 }; if (extra) for (var x in extra) c[x] = extra[x]; return c; }
    function confirmDel() { return SE.ask('Are you sure to Delete?', 'Confirm'); }

    function makeGrids() {
        /* grd : dtGrid in designer order; Delete "X" and Edit frozen at the left, "+" (Add) only against an order; Id / Order* / *Id / Bal* columns are hidden */
        var cols = [
            { k: 'Delete', t: 'X', w: 20, btn: 'X' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Add', t: '+', w: 20, btn: '+', hide: true },
            { k: 'OrderNo', t: 'OrderNo', w: 70, hide: true },
            { k: 'WarehouseId', t: 'WareHouse Name', w: 250, edit: false, list: function () { return S.warehouses; }, lk: 'Id', lt: 'WareHouseName' },
            { k: 'Item', t: 'Item', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingType', t: 'PackingType', w: 100 }, { k: 'PackUOM', t: 'PackUOM', w: 80 },
            numCol('ItemQty', 'ItemQty', 80, q2, { sum: true, edit: false }), numCol('GrossWeight', 'GrossWeight', 100, q3, { sum: true, edit: false }), numCol('AddLss', 'Less', 70, q2, { edit: false }),
            numCol('NetBillWeight', 'NetBillWeight', 100, q3, { sum: true }), numCol('StockWeight', 'StockWeight', 100, q3, { sum: true, edit: false }), numCol('Rate', 'Rate', 90, rate),
            { k: 'RateUOM', t: 'RateUOM', w: 80 }, numCol('RateCutAmount', 'RateCutAmount', 100, amt, { sum: true, edit: false }), numCol('ItemAmount', 'ItemAmount', 100, amt, { sum: true }),
            { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate', edit: false }, { k: 'GpNo', t: 'GpNo', w: 70, edit: false }, { k: 'VehicleNo', t: 'VehicleNo', w: 90, edit: false },
            numCol('FcyAmount', 'FcyAmount', 100, fcy, { sum: true, hide: true }), numCol('BillAmount', 'Item Net Amount', 110, amt, { sum: true }),
            numCol('Expense', 'Expense', 90, amt, { sum: true }), numCol('Freights', 'ChargeToProduct', 100, amt, { sum: true }), numCol('Commission', 'Commission', 90, amt, { sum: true })
        ];
        G.grd = SE.grid('grd', { cols: cols, frozen: 3, dec: S.fm.amtDec,
            onBtn: function (k, r, i) {
                if (k === 'Delete') { confirmDel().then(function (yes) { if (yes) calc('grid', i, '', 'Delete'); }); }       // grd_ColumnButtonClick :1970
                else if (k === 'Edit') calc('edit', i);
                else if (k === 'Add') calc('grid', i, '', 'Add');
            },
            onDbl: function (r, i) { calc('edit', i); },
            onEdit: function (r, k, v, i) {                                                         // grd_CellUpdated :2014
                r[k] = (k === 'WarehouseId' || k === 'GpNo') ? ti(v) : (k === 'GpDate' || k === 'VehicleNo') ? String(v == null ? '' : v) : tn(v);
                calc('gridCell', i, k);
            } });

        G.ex = SE.grid('grdInvExp', { cols: [
            { k: 'ItemId', t: 'Item', w: 220, edit: true, list: function () { return S.otherItems; }, lk: 'Id', lt: 'OtherItemName' },
            numCol('Qty', 'Qty', 70, plain, { edit: true }), numCol('Rate', 'Rate', 80, amt, { edit: true }), numCol('Amount', 'Amount', 90, amt, { sum: true, edit: true }),
            { k: 'Remarks', t: 'Remarks', w: 200, edit: true }, { k: 'Delete', t: 'Delete', w: 55, btn: 'Delete' }, { k: 'Add', t: 'Add New', w: 60, btn: 'Add' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'ItemId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('expCell', i, k); },
           onBtn: function (k, r, i) {
               if (k === 'Delete') confirmDel().then(function (yes) { if (yes) calc('expBtn', i, '', 'Delete'); }); else calc('expBtn', i, '', k);          // grdInvExp_ColumnButtonClick :1415
           } });

        G.gl = SE.grid('grdGLedger', { cols: [
            { k: 'AccountId', t: 'Account', w: 220, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            { k: 'Remarks', t: 'Remarks', w: 150, edit: true }, numCol('Percentage', 'Percentage', 80, plain, { edit: true }), numCol('Qty', 'Qty', 60, plain, { edit: true }),
            numCol('Rate', 'Rate', 70, amt, { edit: true }), numCol('Debit', 'Debit', 90, amt, { sum: true, edit: true }), numCol('Credit', 'Credit', 90, amt, { sum: true, edit: true }),
            { k: 'Delete', t: 'X', w: 20, btn: 'X' }, { k: 'Add', t: '+', w: 20, btn: '+' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'AccountId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('glCell', i, k); },
           onBtn: function (k, r, i) {
               if (k === 'Delete') confirmDel().then(function (yes) { if (yes) calc('glBtn', i, '', 'Delete'); }); else calc('glBtn', i, '', 'Add');             // grdGLedger_ColumnButtonClick :1562
           } });

        /* history : Edit / Slip / PartySlip / Voucher buttons, then the columns of the history table */
        G.hist = SE.grid('grdHistory', { frozen: 4, onDbl: histOpen, onBtn: histButton, onLink: histLink, onSel: histSelected, dec: S.fm.amtDec, cols: [
            { k: 'Edit', t: 'Edit', w: 40, btn: 'Edit' }, { k: 'Slip', t: 'Slip', w: 40, btn: 'Slip' }, { k: 'PartySlip', t: 'PartySlip', w: 70, btn: 'PartySlip' }, { k: 'Voucher', t: 'Voucher', w: 60, btn: 'Voucher' },
            { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'ManualBillNo', t: 'ManualBillNo', w: 90 },
            { k: 'DueDate', t: 'DueDate', w: 90, f: 'sdate' }, { k: 'CommAgent', t: 'CommAgent', w: 150 }, { k: 'CommType', t: 'CommType', w: 80 }, numCol('CommRate', 'CommRate', 80, rate),
            { k: 'CommAmount', t: 'CommAmount', w: 90, f: 'amt', cls: 'num' }, { k: 'CommRemarks', t: 'CommRemarks', w: 120 }, { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt' },
            { k: 'EntryDate', t: 'EntryDate', w: 90, f: 'sdate' }, { k: 'EntryUser', t: 'EntryUser', w: 110 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100, link: true }, { k: 'Remarks', t: 'Remarks', w: 200 }
        ] });

        /* grdDetail : the 32 columns of GetDetailGrdByHeadId (ids hidden) */
        G.hd = SE.grid('grdDetail', { dec: S.fm.amtDec, cols: [
            { k: 'Warehouse', t: 'WareHouse', w: 120 }, { k: 'Item', t: 'Item', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingType', t: 'PackingType', w: 100 },
            { k: 'PackUOM', t: 'PackUOM', w: 80 }, numCol('ItemQty', 'ItemQty', 80, q2), numCol('GrossWeight', 'GrossWeight', 100, q3), numCol('AddLss', 'Less', 70, q2), numCol('NetBillWeight', 'NetBillWeight', 100, q3),
            numCol('Rate', 'Rate', 90, rate), { k: 'RateUOM', t: 'RateUOM', w: 80 }, numCol('RateCutAmount', 'RateCutAmount', 100, amt), numCol('ItemAmount', 'ItemAmount', 100, amt),
            { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 70 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, numCol('FcyAmount', 'FcyAmount', 100, fcy),
            numCol('BillAmount', 'BillAmount', 100, amt), numCol('Expense', 'Expense', 90, amt), numCol('Freights', 'Freights', 90, amt), numCol('Commission', 'Commission', 90, amt)
        ] });
    }

    /* ------------------------------------------------------------------ Reset :2483 (the server resets the state in the order of the desktop) */
    function reset() {
        S.files = []; S.removed = []; S.existing = []; S.vhId = 0; S.id = 0;
        return calc('reset', -1, '', '', false, true).then(function () { cb.cust.focus(); });
    }

    /* ConfigurationDefault :1175 for btnRefresh / the first load (the lists were re-read) */
    function applyDefaults() {
        var d = S.defaults || {};
        if (d.packingTypeId != null) setCombo(cb.pack, d.packingTypeId);
        if (d.warehouseId != null) setCombo(cb.wh, d.warehouseId);
        if (d.cityId != null && !(+cb.city.value())) setCombo(cb.city, d.cityId);
        if (d.jobLotId != null && !(+cb.lot.value())) setCombo(cb.lot, d.jobLotId);
        if (d.currencyId != null && !(+cb.cur.value())) setCombo(cb.cur, d.currencyId);
        if (d.baseRate != null && !(tn($('txtExchangeRate').value))) { setv('txtExchangeRate', SE.num(d.baseRate, S.fm.rateDec, S.fm.rateDec)); return calc('exch'); }
        return Promise.resolve();
    }
    function defaultTerms() {
        var t = cb.term.rows()[1]; if (t) cb.term.setValue(t.Id);                           // PaymentTerms :817 - the second row
        var l = cb.dterm.rows()[0]; if (l) cb.dterm.setValue(l.Id);                         // DeliveryTerm :847 - "Load"
    }
    function setLists(d, append) {
        S.accounts = d.accounts; S.otherItems = d.otherItems; S.warehouses = d.warehouses; S.customers = d.customers;
        cb.wh.setData(d.warehouses); cb.city.setData(d.cities); cb.cust.setData(d.customers); cb.agent.setData(d.customers);
        cb.term.setData(d.paymentTerms); cb.dterm.setData(d.deliveryTerms); cb.type.setData(d.commTypes); cb.uom.setData(d.commUoms);
        S.items = append ? S.items.concat(d.items) : d.items; bindItems();
        cb.lot.setData(d.jobLots); cb.pack.setData(d.packingTypes); cb.cur.setData(d.currencies); cb.tr.setData(d.accounts);
        defaultTerms();
        G.ex.refresh(); G.gl.refresh(); G.grd.refresh();
    }
    /* btnRefresh_Click :2599 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) { S.defaults = d.defaults || {}; setLists(d, true); return applyDefaults(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load Order (btnLoadGdn_Click :4520 -> LoadSaleOrder) */
    function loadOrder() {
        return openLoader().then(function (req) {
            if (!req) return;
            req.histDetail = G.hd.rows().map(function (r) { return { OrderId: r.OrderId, OrderDetailId: r.OrderDetailId }; });
            req.state = collect();
            return SE.api(API + '/load', { method: 'POST', body: req }).then(function (st) {
                return applyState(st, true).then(function () { return showMsgs(st.messages); });
            }).catch(function (e) { return msg(e.message, 'Database Error'); });
        });
    }

    /* LoadSaleOrder (964 x 461): resolves to the request {detailIds, from, to, fromNo, toNo, supplierId}, or null when closed */
    function openLoader() {
        return new Promise(function (resolve) {
            var html = '<div class="dpan" style="position:absolute;left:0px;top:0px;width:964px;height:50px;background:#008080;">' +
                '<div class="dts white" style="position:absolute;left:0px;top:0px;width:964px;height:25px;"><button type="button" class="tsi" id="ld_new"><i class="fa fa-plus-circle g"></i>New</button>' +
                '<button type="button" class="tsi" id="ld_refresh"><i class="fa fa-refresh g"></i>Refresh</button><button type="button" class="tsi" id="ld_short"><i class="fa fa-keyboard-o k"></i>ShortCut Keys</button></div>' +
                '<span class="dl white" style="left:5px;top:27px;font-size:15px;font-family:Tahoma,sans-serif;">Pending Sale Order</span></div>' +
                '<div class="dgb" style="left:5px;top:49px;width:747px;height:75px;"><span class="gbcap">Main</span>' +
                '<span class="dl" style="left:7px;top:21px;">From Date</span><input class="f" type="date" id="ld_from" style="left:73px;top:17px;width:116px;height:23px;">' +
                '<span class="dl" style="left:7px;top:50px;">To Date</span><input class="f" type="date" id="ld_to" style="left:73px;top:46px;width:116px;height:23px;">' +
                '<span class="dl" style="left:194px;top:21px;">Order No From</span><input class="f" id="ld_nofrom" style="left:281px;top:17px;width:114px;height:23px;">' +
                '<span class="dl" style="left:194px;top:50px;">Order No To</span><input class="f" id="ld_noto" style="left:281px;top:46px;width:114px;height:23px;">' +
                '<span class="dl" style="left:401px;top:21px;">Supplier Name</span><select class="f" id="ld_cust" style="left:493px;top:15px;width:243px;height:26px;"></select>' +
                '<button type="button" class="dbtn" id="ld_search" style="left:537px;top:44px;width:72px;height:26px;background:#008080;color:#ffffff;font-weight:bold;">Search</button>' +
                '<button type="button" class="dbtn" id="ld_load" style="left:610px;top:44px;width:64px;height:26px;background:#008080;color:#ffffff;font-weight:bold;">Load</button>' +
                '<button type="button" class="dbtn" id="ld_reset" style="left:676px;top:44px;width:60px;height:26px;background:#008080;color:#ffffff;font-weight:bold;">Reset</button></div>' +
                '<div class="dpan" style="position:absolute;left:0px;top:127px;width:964px;height:29px;background:#008080;"></div>' +
                '<div class="dgrid" id="ld_grd" tabindex="0" style="position:absolute;left:0px;top:156px;width:964px;height:305px;"></div>';
            var done = false;
            var m = SS.modal('EccountBookERP', html, 964, 461, function () { if (!done) { done = true; resolve(null); } });
            var cbs = XCombo(m.q('#ld_cust'), { columns: [{ key: 'Name', caption: 'Supplier Name' }], textKey: 'Name', popupWidth: 360 });
            var gm = SE.grid(m.q('#ld_grd'), { frozen: 2, cols: [
                { k: '_sel', t: 'Select', w: 40, sel: true }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100 },
                { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'SupplierName', t: 'SupplierName', w: 200 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 },
                { k: 'PaymentTerm', t: 'PaymentTerm', w: 110 }, { k: 'DueDate', t: 'DueDate', w: 90, f: 'sdate' }, { k: 'DueDays', t: 'DueDays', w: 70 }, { k: 'JobLot', t: 'JobLot', w: 100 },
                { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'PackUomCode', t: 'PackUomCode', w: 90 }, numCol('ItemRate', 'ItemRate', 90, rate),
                { k: 'RateUomCode', t: 'RateUomCode', w: 90 }, numCol('ItemQty', 'ItemQty', 80, q2, { sum: true }), numCol('NetWeight', 'NetWeight', 90, q3, { sum: true }),
                numCol('TotalAmount', 'TotalAmount', 100, amt, { sum: true }), numCol('RecQty', 'RecQty', 80, q2, { sum: true }), numCol('RecWeight', 'RecWeight', 90, q3, { sum: true }),
                numCol('RecAmount', 'RecAmount', 100, amt, { sum: true }), numCol('BalQty', 'BalQty', 80, q2, { sum: true }), numCol('BalNetWeight', 'BalNetWeight', 100, q3, { sum: true }),
                numCol('BalAmount', 'BalAmount', 100, amt, { sum: true }), { k: 'EntryDate', t: 'EntryDate', w: 90, f: 'sdate' }, { k: 'EntryUserName', t: 'EntryUserName', w: 110 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 90, f: 'sdate' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 110 }
            ] });
            SE.digitsOnly(m.q('#ld_nofrom')); SE.digitsOnly(m.q('#ld_noto'));
            function combos() {
                return SE.api(API + '/loader/combos', { quiet: true }).then(function (d) { cbs.setData(d.customers || []); if (!m.q('#ld_from').value) m.q('#ld_from').value = d.fromDate || SE.today(); }).catch(fail);
            }
            function query() { return { from: m.q('#ld_from').value, to: m.q('#ld_to').value, fromNo: ti(m.q('#ld_nofrom').value), toNo: ti(m.q('#ld_noto').value), supplierId: +cbs.value() || 0 }; }
            function pending() {                                                          // PendingPurchaseOrderRegularForLoad
                return SE.api(API + '/loader/pending' + SE.q(query())).then(function (rows) { gm.setRows(rows); }).catch(function (e) { return msg(e.message, 'Message'); });
            }
            m.q('#ld_to').value = SE.today();
            combos().then(pending);
            m.q('#ld_search').onclick = pending;
            m.q('#ld_refresh').onclick = combos;
            m.q('#ld_new').onclick = m.q('#ld_reset').onclick = function () { cbs.clear(); m.q('#ld_nofrom').value = ''; m.q('#ld_noto').value = ''; pending(); };
            m.q('#ld_short').onclick = function () { SE.shortcuts([['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]); };
            m.q('#ld_load').onclick = function () {                                       // btnLoadOnInvoice_Click_1
                var checked = gm.checked();
                if (checked.length === 0) { msg('Check the Row first', 'Message'); return; }
                var order = 0, ids = [];
                for (var i = 0; i < checked.length; i++) {
                    var id = ti(checked[i].Id);
                    if (order === 0) order = id;
                    if (order !== id) { msg('You Can Only Select Rows Of Same Order', 'Message'); return; }
                    ids.push(ti(checked[i].DetailId));
                }
                var q = query(); q.detailIds = ids;
                done = true; m.close(); resolve(q);
            };
        });
    }

    /* ------------------------------------------------------------------ Save (Insert :2640) */
    function insert(id) {
        var st = collect(); st.id = id;
        return SE.api(API + '/precheck', { method: 'POST', body: st, quiet: true }).then(function (p) {
            if (p.stopped) return msg(p.message, 'Message').then(function () { focusOn(p.focus); });
            return SE.ask(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = { state: collect(), attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed } };
                body.state.id = id;
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    if (r.stopped) return msg(r.message, 'Message').then(function () { focusOn(r.focus); });
                    var voucher = $('ChkBok').checked, slip = $('ChkPrintslip').checked;
                    return SE.alert(r.message).then(function () { return S.wages ? openWages(r.id, r.grossWeightTotal || 0) : null; }).then(function () {
                        return reset().then(function () {
                            if (voucher) SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: r.voucherHeadId, documentTypeId: DOC_TYPE });
                            if (slip) SE.printRpt('1809-SaleInvoiceDirectSlip_ForSalt.rpt', { id: r.id });
                        });
                    });
                });
            });
        }).catch(function (e) { return msg(e.message, 'Database Error'); });
    }
    function doSave() { S.id = 0; return insert(0); }
    function doUpdate() {
        if (S.id === 0) return msg('Record not update because Id not found', 'Database Error');
        return insert(S.id);
    }
    /* frmwagesBillHeader(RefDocTypeId 1809, RefDocId, GrossWeightTotal).ShowDialog() */
    function openWages(refDocId, gross) {
        return new Promise(function (resolve) {
            var ov = document.createElement('div'); ov.className = 'fx-overlay-salt';
            var fr = document.createElement('iframe'); fr.title = 'Contractor Wages Bill';
            fr.src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: DOC_TYPE, refDocId: refDocId, grossWeightTotal: gross });
            var close = document.createElement('button'); close.type = 'button'; close.textContent = 'Close';
            ov.appendChild(fr); ov.appendChild(close); document.body.appendChild(ov);
            var fin = function () { ov.remove(); window.P280WagesClosed = null; resolve(); };
            close.addEventListener('click', fin); window.P280WagesClosed = fin;
        });
    }

    /* prints (btnPrint_Click :3588, btnSlip_Click :3600, btn294APrint_Click) */
    function printVoucher() { SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: S.vhId, documentTypeId: DOC_TYPE }); }
    function printSlip(id) { SE.printRpt('1809-SaleInvoiceDirectSlip_ForSalt.rpt', { id: id }); }
    function printSlipA(id) { SE.printRpt('1809A-SaleInvoiceDirectSlip_ForSalt.rpt', { id: id }); }

    /* ------------------------------------------------------------------ ReadById */
    function readById(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            tabs.select('tabForm');
            S.vhId = d.voucherHeadId || 0;
            return applyState(d.state, true).then(function () { S.existing = d.attachments || []; S.files = []; S.removed = []; });
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */
    function showHistory() {
        var type = $('rdentrydate').checked ? 'entry' : $('rdmodifydate').checked ? 'modify' : $('rdapproveddate').checked ? 'approved' : 'document';
        var q = { dateType: type, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
                  fromNo: ti($('FromDocNoHistory').value), toNo: ti($('ToDocNoHistory').value), customerId: +cb.hcust.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (rows.length > 0) G.hist.setRows(rows); else { G.hist.setRows([]); G.hd.setRows([]); }
        }).catch(fail);
    }
    function resetHistory() {
        $('FromDateHistory').value = SE.addDays(-S.historyDays); $('ToDateHistory').value = SE.today(); $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hcust.clear(); G.hist.setRows([]); G.hd.setRows([]);
    }
    function histOpen(r) { return readById(ti(r.Id)); }
    function histButton(k, r) {
        if (k === 'Edit') return reset().then(function () { return readById(ti(r.Id)); });
        if (k === 'Slip') return printSlip(ti(r.Id));
        if (k === 'PartySlip') return printSlipA(ti(r.Id));
        if (k === 'Voucher') return SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: ti(r.VoucherHeadId), documentTypeId: DOC_TYPE });
    }
    function histLink(k, r) { if (k === 'NoOfAttachments' && r && ti(r.Id) > 0) return SS.showAttachments(API, ti(r.Id)); }
    function histSelected(r) {
        var seq = ++S.histSeq;
        if (!r) { G.hd.setRows([]); return; }
        SE.api(API + '/' + r.Id + '/history-detail', { quiet: true }).then(function (rows) { if (seq === S.histSeq) G.hd.setRows(rows); })
            .catch(function (e) { if (seq === S.histSeq) SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ keys (InvfrmPurchasedirectInvoice_KeyDown_1 :4631, MakeShortCutKeys :4757) */
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 294'], ['Alt+2', 'For Print Slip 294A'], ['Alt+3', 'For Print Voucher 103'], ['Ctrl+F5', 'For Focus on Customer Name'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function focused(id) { var g = $(id); return !!g && (g === document.activeElement || g.contains(document.activeElement)); }
    function focusEl(id) { var g = $(id); if (g) g.focus(); }
    function onKey(e) {
        SS.enterTab(e);
        var k = (e.key || '').toLowerCase(), tab = tabs.index();
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tab === 1) { tabs.select('tabForm'); cb.cust.focus(); } else { tabs.select('tabHistory'); focusEl('grdHistory'); } }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); }
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORTCUTS); return; }
        if (tab === 0) {
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (visible('btnSave')) doSave(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (visible('btnUpdate')) doUpdate(); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.cust.focus(); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); focusEl('grd'); }
            if (e.ctrlKey && e.key === 'ArrowRight') {                                        // two separate ifs in the desktop: a focused grd ends up on grdGLedger
                e.preventDefault();
                if (focused('grd')) focusEl('grdInvExp');
                if (focused('grdInvExp')) focusEl('grdGLedger'); else focusEl('grd');
            }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); cb.wh.focus(); }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); $('btnAttachment').click(); }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); printSlip(S.id); }
            if (e.altKey && (e.code === 'Digit1' || e.code === 'Numpad1')) { e.preventDefault(); printSlip(S.id); }
            if (e.altKey && (e.code === 'Digit2' || e.code === 'Numpad2')) { e.preventDefault(); printSlipA(S.id); }
            if (e.altKey && (e.code === 'Digit3' || e.code === 'Numpad3')) { e.preventDefault(); printVoucher(); }
            return;
        }
        if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focusEl('grdHistory'); }
        if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); focusEl('grdDetail'); }
        if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); if (focused('grdDetail')) focusEl('grdHistory'); else focusEl('grdDetail'); }
        if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); focusEl('grdHistory'); }
    }
    /* grd_KeyDown :4806 / grdInvExp_KeyDown / grdGLedger_KeyDown : Ctrl+Delete and Ctrl+D on the focused grid */
    function gridKeys() {
        function cur(g) { var i = g.curIndex(); return i == null ? -1 : i; }
        $('grd').addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var i = cur(G.grd);
            if (e.key === 'Delete') { e.preventDefault(); confirmDel().then(function (yes) { if (yes) calc('gridKey', i, '', 'CtrlDelete', true); }); }
            else if ((e.key || '').toLowerCase() === 'd') { e.preventDefault(); calc('gridKey', i, '', 'CtrlD', false); }
        });
        $('grdInvExp').addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var i = cur(G.ex);
            if (e.key === 'Delete') { e.preventDefault(); confirmDel().then(function (yes) { if (yes) calc('expKey', i, '', 'Delete', true); }); }
            else if ((e.key || '').toLowerCase() === 'd') { e.preventDefault(); calc('expKey', i, '', 'Add', false); }
        });
        $('grdGLedger').addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var i = cur(G.gl);
            if (e.key === 'Delete') { e.preventDefault(); confirmDel().then(function (yes) { calc('glKey', i, '', 'Delete', yes); }); }   // the desktop deletes the row before the answer is read
            else if ((e.key || '').toLowerCase() === 'd') { e.preventDefault(); calc('glKey', i, '', 'Add', false); }
        });
    }

    /* ------------------------------------------------------------------ load */
    function decimalOnly(el) {                                                            // CommonServices.OnlytextdecimelFunction
        el.addEventListener('keypress', function (e) {
            if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault();
            else if (e.key === '.' && el.value.indexOf('.') >= 0) e.preventDefault();
        });
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') focusEl('grdHistory'); else cb.cust.focus(); });
        ['FromDocNoHistory', 'ToDocNoHistory', 'txtduedays', 'txtFreightAmount', 'txtgatepassno'].forEach(function (k) { SE.digitsOnly($(k)); });
        ['txtQty', 'txtGrossWeight', 'txtStockWeight', 'txtRate', 'txtratecuttotal', 'txtcommrate', 'txtExchangeRate'].forEach(function (k) { decimalOnly($(k)); });
        makeGrids(); gridKeys();
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.fm = d.formats || S.fm; S.defaults = d.defaults || {}; S.historyDays = d.historyDays; S.multi = !!d.multiCurrency; S.outward = d.outwardAcId || 0;
            S.itemsByWh = !!d.itemsAgainstWarehouse; S.wages = !!d.wages;
            G.grd.spec.dec = S.fm.amtDec; G.hist.spec.dec = S.fm.amtDec; G.hd.spec.dec = S.fm.amtDec;
            setLists(d, false);
            cb.hcust.setData((d.history || {}).customers || []);
            show('fcyPane', S.multi); show('trPane', S.outward > 0);
            $('btnSave').disabled = !S.rights.save; $('btnUpdate').disabled = !S.rights.update; show('btnDelete', false);
            $('btnSlip').disabled = $('btn294APrint').disabled = $('btnPrint').disabled = !S.rights.print; $('ChkPrintslip').checked = !!S.rights.print;
            $('DocDate').value = SE.today(); $('duedate').value = SE.today(); $('txtgpdate').value = SE.today();
            $('FromDateHistory').value = SE.addDays(-S.historyDays); $('ToDateHistory').value = SE.today();
            return applyDefaults().then(reset);
        }).catch(fail);
        var rec = SE.param('record'); if (rec) setTimeout(function () { readById(+rec); }, 800);
    }

    function wire() {
        $('btnNew').onclick = reset; $('btnSave').onclick = doSave; $('btnUpdate').onclick = doUpdate; $('btnRefresh').onclick = refresh;
        $('btnAttachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('btnLoadSaleOrder').onclick = loadOrder;
        $('btnPrint').onclick = printVoucher; $('btnSlip').onclick = function () { printSlip(S.id); }; $('btn294APrint').onclick = function () { printSlipA(S.id); };
        $('btnShortcutKeys').onclick = function () { SE.shortcuts(SHORTCUTS); };
        $('btnShow').onclick = showHistory; $('btnResetHistory').onclick = resetHistory;
        $('BtnRefreshHistory').onclick = function () { SE.api(API + '/history-combos').then(function (d) { cb.hcust.setData(d.customers); }).catch(fail); };
        $('btnAdd').onclick = function () { calc('add'); };
        $('btnUpdateDetail').onclick = function () { calc('update'); };
        $('btnCancelUpdateDetial').onclick = function () { calc('cancelUpdate'); };
        $('rdName').onchange = $('rdCode').onchange = bindItems;
        [['txtQty', 'qty'], ['txtGrossWeight', 'gross'], ['txtAddLss', 'addless'], ['txtRate', 'rate'], ['txtratecuttotal', 'ratecut'], ['txtcommrate', 'comm'],
         ['txtduedays', 'due'], ['txtExchangeRate', 'exch'], ['txtFreightAmount', 'freight']].forEach(function (p) {
            $(p[0]).addEventListener('input', function () { calc(p[1]); });
        });
        $('txtExchangeRate').addEventListener('blur', function () { calc('exchLeave'); });
        $('DocDate').addEventListener('change', function () { calc('docDate'); });
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
