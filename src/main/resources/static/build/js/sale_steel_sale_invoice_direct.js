/*
 * Screen 544  SaleInvoiceDirect_St  (Architecture.WinApp.Steel.Sale.SaleInvoiceDirect_St, Steel Sale Invoice Direct, document type 1510)
 * Page script. Desktop methods are named in the comments (SaleInvoiceDirect_St.cs). Server: /sale/steel/sale-invoice-direct/api
 *
 * The totals of the desktop are recalculated by its text box / grid events; the server holds the same handlers (SaleStDirectCalc) and answers the
 * new state of the form for every event ("calc"), so the figures cannot differ between the screen and the save.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/steel/sale-invoice-direct/api', fail = SS.fail;
    var S = { id: 0, approved: false, vhId: 0, rights: {}, fm: { amtDec: 2, fcyDec: 2, rateDec: 2 }, files: [], removed: [], existing: [], customers: [], accounts: [], otherItems: [],
              defaults: {}, histSeq: 0, historyDays: 3, delLines: [], updateIndex: -1, canSave: true, saveMode: true, uomItem: -1, uoms: [], freightText: '', transporterId: 0, supplierGlId: 0,
              yearStart: '', q: Promise.resolve() };
    var cb = {}, G = {}, tabs, tabs2;

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
    function first(c) { var r = c.rows()[0]; if (r) c.setValue(r.Id); }
    function amt(v) { return SE.num(v, S.fm.amtDec, S.fm.amtDec); }
    function fcy(v) { return SE.num(v, S.fm.fcyDec, S.fm.fcyDec); }
    function rate(v) { return SE.num(v, S.fm.rateDec, S.fm.rateDec); }
    function q2(v) { return v == null || v === '' ? '' : SE.num(v, 2, 0); }                // "#,##0.##"
    function q3(v) { return v == null || v === '' ? '' : SE.num(v, 3, 0); }               // "#,##0.###"
    function clean(rows) { return JSON.parse(JSON.stringify(rows, function (k, v) { return k.charAt(0) === '_' ? undefined : v; })); }
    function setv(id, v) { var el = $(id); v = v == null ? '' : String(v); if (el.value !== v) el.value = v; }

    /* ------------------------------------------------------------------ combos */
    function stock() { S.q = S.q.then(refreshStock).catch(fail); }
    function makeCombos() {
        cb.branch = XCombo('CmbBranch', { columns: [{ key: 'BranchName', caption: 'Branch Name' }], textKey: 'BranchName' });
        cb.project = XCombo('CmbProject', { columns: [{ key: 'ProjectName', caption: 'Project Name' }], textKey: 'ProjectName' });
        cb.cust = XCombo('CmbSuppCustomer', { columns: [{ key: 'CompanyName', caption: 'Supplier Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: function () { calc('supplier'); } });
        cb.agent = XCombo('CmbCommAgent', { columns: [{ key: 'CompanyName', caption: 'Commission Agent' }], textKey: 'CompanyName', popupWidth: 420, onSelect: function () { calc('comm'); } });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription' });
        cb.dterm = XCombo('CmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm', onSelect: function () { calc('delTerm'); } });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode' });
        cb.type = XCombo('CmbCommType', { columns: [{ key: 'CommissionType', caption: 'CommissionType' }], textKey: 'CommissionType', onSelect: function () { calc('comm'); } });
        cb.uom = XCombo('CmbCommUom', { columns: [{ key: 'Uom', caption: 'UOM' }], textKey: 'Uom', onSelect: function () { calc('comm'); } });
        cb.item = XCombo('CmbItem', { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 420, onSelect: itemChanged });
        cb.lot = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', onSelect: stock });
        cb.pack = XCombo('CmbPackingType', { columns: [{ key: 'PackTypeDesc', caption: 'Packing Type' }], textKey: 'PackTypeDesc', onSelect: stock });
        cb.puom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'UOM' }], textKey: 'UOMCode', onSelect: function () { calc('packuom').then(refreshStock).catch(fail); } });
        cb.ruom = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'UOM' }], textKey: 'UOMCode', onSelect: function () { calc('rateuom'); } });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouse' }], textKey: 'WareHouseName', popupWidth: 300, onSelect: stock });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City' }], textKey: 'CityName', popupWidth: 300 });
        cb.hcust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Name', caption: 'Customer Name' }], textKey: 'Name', popupWidth: 380 });
        cb.hpay = XCombo('CmbPaymentTermHistory', { columns: [{ key: 'Name', caption: 'Payment Term' }], textKey: 'Name' });
        cb.hdel = XCombo('CmbDeliveryTermHistory', { columns: [{ key: 'Name', caption: 'Delivery Term' }], textKey: 'Name' });
    }

    /* CmbItem_Leave -> PackUOM() :1126 (rebinds the two UOM combos, keeps the text when the new item has it) + AvailableStockGetByItem */
    function bindUoms(itemId) {
        var keepP = cb.puom.text(), keepR = cb.ruom.text();
        S.uomItem = itemId;
        if (!(itemId > 0)) { cb.puom.setData([]); cb.ruom.setData([]); return Promise.resolve(); }
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId }), { quiet: true }).then(function (rows) {
            S.uoms = rows;
            cb.puom.setData(rows); cb.ruom.setData(rows);
            setText(cb.puom, keepP, 'UOMCode'); setText(cb.ruom, keepR, 'UOMCode');
        });
    }
    function itemChanged() {
        S.q = S.q.then(function () { return bindUoms(+cb.item.value() || 0); }).then(refreshStock).catch(fail);
    }
    /* AvailableStockGetByItem :1242 */
    function refreshStock() {
        var q = { warehouseId: +cb.wh.value() || 0, itemId: +cb.item.value() || 0, jobLotId: +cb.lot.value() || 0, packingTypeId: +cb.pack.value() || 0, uomId: +cb.puom.value() || 0, docDate: $('DocDate').value };
        return SE.api(API + '/stock' + SE.q(q), { quiet: true }).then(function (d) { $('lblBalance').textContent = d.balance == null ? '' : 'Balance: ' + d.balance; });
    }

    /* ------------------------------------------------------------------ the state of the form <-> the server */
    function entry() {
        return {
            itemId: +cb.item.value() || 0, itemText: cb.item.text(), jobLotId: +cb.lot.value() || 0, jobLotText: cb.lot.text(), packingTypeId: +cb.pack.value() || 0, packingText: cb.pack.text(),
            packUomId: +cb.puom.value() || 0, packUomText: cb.puom.text(), rateUomId: +cb.ruom.value() || 0, rateUomText: cb.ruom.text(), warehouseId: +cb.wh.value() || 0, warehouseText: cb.wh.text(),
            cityId: +cb.city.value() || 0, cityText: cb.city.text(), itemQty: $('txtItemQty').value, gross: $('txtGrossWeight').value, wtCut: $('txtWtCut').value, wtCutTotal: $('txtWeightCutTotal').value,
            addLess: $('txtAddLss').value, netBill: $('txtNetBillWeight').value, stock: $('txtStockWeight').value, rate: $('txtRate').value, rateCut: $('txtratecut').value,
            rateCutTotal: $('txtratecuttotal').value, amount: $('txtAmount').value, gpDate: $('txtgpdate').value, gpNo: $('txtgatepassno').value, vehicleNo: $('txtvehicleno').value,
            remarks: $('txtRemarksDetail').value
        };
    }
    function collect() {
        return {
            id: S.id, branchId: +cb.branch.value() || 0, projectId: +cb.project.value() || 0, docNo: $('txtdocno').value, docDate: $('DocDate').value,
            supplierId: +cb.cust.value() || 0, supplierGlId: S.supplierGlId, refNo: $('txtSupplierReference').value, billNo: $('txtBillNo').value,
            deliveryTerm: cb.dterm.text(), deliveryStart: $('DeliveryStartDate').value, expiryDate: $('datExpiryDate').value, deliveryDays: $('txtDeliveryDays').value,
            paymentTermId: +cb.term.value() || 0, dueDays: $('txtduedays').value, dueDate: $('duedate').value, currencyId: +cb.cur.value() || 0, exchangeRate: $('txtExchangeRate').value,
            totalQty: $('txtTotalQty').value, totalWeight: $('txtTotalWeight').value, fcyAmount: $('txtFcyAmount').value, billAmount: $('txtBillAmount').value,
            commAgentId: +cb.agent.value() || 0, commType: cb.type.text(), commRate: $('txtCommRate').value, commUom: cb.uom.text(), commAmount: $('txtCommAmount').value,
            commRemarks: $('txtCommissionRemarks').value, remarks: $('txtRemarks').value, transporterId: S.transporterId, freightText: $('txtFreightAmount').value,
            saveMode: S.saveMode, canSave: S.canSave, updateIndex: S.updateIndex, e: entry(),
            lines: clean(G.grd.rows()), removed: clean(S.delLines), freight: clean(G.fr.rows()), exp: clean(G.ex.rows()), gl: clean(G.gl.rows())
        };
    }

    function grdSettings() {                                                              // the column visibility that depends on the first row (OrderId)
        var rows = G.grd.rows(), r0 = rows[0], ord = !!r0 && ti(r0.OrderId) > 0;
        G.grd.spec.cols.forEach(function (c) {
            if (c.k === 'OrderNo') c.hide = !ord;
            if (c.k === 'Add') c.hide = !ord;
        });
        G.grd.refresh();
    }

    function focusOn(name) {
        var map = { CmbItem: cb.item, CmbJobLot: cb.lot, CmbPackingType: cb.pack, CmbPackUom: cb.puom, CmbRateUom: cb.ruom, CmbWarehouse: cb.wh, CmbCity: cb.city };
        if (!name) return;
        if (map[name]) map[name].focus(); else if ($(name)) $(name).focus();
    }

    /* the texts and grids that the events change; full = also the combos of the header (a loaded record) */
    function applyState(st, full) {
        var act = document.activeElement && document.activeElement.id;
        function put(id, v) { if (full || act !== id) setv(id, v); }
        put('txtCommAmount', st.commAmount); put('txtBillAmount', st.billAmount); put('txtTotalQty', st.totalQty); put('txtTotalWeight', st.totalWeight); put('txtFcyAmount', st.fcyAmount);
        put('txtduedays', st.dueDays); setv('duedate', st.dueDate); setv('datExpiryDate', st.expiryDate); put('txtDeliveryDays', st.deliveryDays); put('DeliveryStartDate', st.deliveryStart);
        put('txtExchangeRate', st.exchangeRate); put('txtCommRate', st.commRate);
        S.transporterId = st.transporterId || 0; S.supplierGlId = st.supplierGlId || 0; setv('txtSupplierGLId', S.supplierGlId || ''); setv('txtFreightAmount', st.freightText);
        S.delLines = st.removed || []; S.updateIndex = st.updateIndex == null ? -1 : st.updateIndex; S.canSave = st.canSave !== false; S.saveMode = st.saveMode !== false;
        if (full) {
            setv('txtdocno', st.docNo); if (st.docDate) $('DocDate').value = st.docDate;
            setCombo(cb.branch, st.branchId); setCombo(cb.project, st.projectId); setCombo(cb.cust, st.supplierId);
            setv('txtSupplierReference', st.refNo); setv('txtBillNo', st.billNo);
            setText(cb.dterm, st.deliveryTerm, 'DeliveryTerm'); setCombo(cb.term, st.paymentTermId); setCombo(cb.cur, st.currencyId);
            setCombo(cb.agent, st.commAgentId); setText(cb.type, st.commType, 'CommissionType'); setText(cb.uom, st.commUom, 'Uom');
            setv('txtCommissionRemarks', st.commRemarks); setv('txtRemarks', st.remarks);
            setv('txtCommRate', st.commRate); setv('txtExchangeRate', st.exchangeRate); setv('txtduedays', st.dueDays); setv('txtDeliveryDays', st.deliveryDays); setv('DeliveryStartDate', st.deliveryStart);
        }
        G.grd.setRows(st.lines || []); G.fr.setRows(st.freight || []); G.ex.setRows(st.exp || []); G.gl.setRows(st.gl || []);
        grdSettings();
        show('btnAdd', S.updateIndex < 0); show('btnUpdateDetail', S.updateIndex >= 0); show('btnCancelUpdateDetial', S.updateIndex >= 0);
        return applyEntry(st.e || {}).then(function () { focusOn(st.focus); });
    }
    function applyEntry(e) {
        var p = Promise.resolve();
        if ((e.itemId || 0) !== S.uomItem && (e.itemId || 0) > 0) {                       // edit of a row: the UOM combos need the schedule of its item
            p = SE.api(API + '/uoms' + SE.q({ itemId: e.itemId }), { quiet: true }).then(function (rows) { S.uoms = rows; S.uomItem = e.itemId; cb.puom.setData(rows); cb.ruom.setData(rows); });
        } else if (!(e.itemId > 0) && S.uomItem !== 0) { S.uomItem = 0; cb.puom.setData([]); cb.ruom.setData([]); }
        return p.then(function () {
            setCombo(cb.item, e.itemId); setCombo(cb.lot, e.jobLotId); setCombo(cb.pack, e.packingTypeId); setCombo(cb.puom, e.packUomId); setCombo(cb.ruom, e.rateUomId);
            setCombo(cb.wh, e.warehouseId); setCombo(cb.city, e.cityId);
            setv('txtItemQty', e.itemQty); setv('txtGrossWeight', e.gross); setv('txtWtCut', e.wtCut); setv('txtWeightCutTotal', e.wtCutTotal); setv('txtAddLss', e.addLess);
            setv('txtNetBillWeight', e.netBill); setv('txtStockWeight', e.stock); setv('txtRate', e.rate); setv('txtratecut', e.rateCut); setv('txtratecuttotal', e.rateCutTotal);
            setv('txtAmount', e.amount); setv('txtgpdate', e.gpDate); setv('txtgatepassno', e.gpNo); setv('txtvehicleno', e.vehicleNo); setv('txtRemarksDetail', e.remarks);
        });
    }

    function showMsgs(list) {
        var p = Promise.resolve();
        (list || []).forEach(function (m) { p = p.then(function () { return msg(m, 'Message'); }); });
        return p;
    }

    /* one desktop event on the server; calls are queued so the answers cannot overtake each other */
    function calc(ev, row, col, key) {
        var done;
        S.q = S.q.then(function () {
            var body = { event: ev, row: row == null ? -1 : row, col: col || '', key: key || '', state: collect() };
            return SE.api(API + '/calc', { method: 'POST', body: body, quiet: true }).then(function (st) { return applyState(st, false).then(function () { return showMsgs(st.messages); }); });
        }).catch(fail);
        return S.q;
    }

    /* ------------------------------------------------------------------ grids */
    function numCol(k, t, w, fn, extra) { var c = { k: k, t: t || k, w: w || 90, cls: 'num', render: fn || q2 }; if (extra) for (var x in extra) c[x] = extra[x]; return c; }
    function plain(v) { return v == null || v === '' ? '' : String(v); }

    function makeGrids() {
        /* grd : the 40 columns of dtGrid in designer order; Delete "X" (and "+" Add against an order) frozen at the left */
        var cols = [
            { k: 'Add', t: '+', w: 20, btn: '+', hide: true }, { k: 'Delete', t: 'X', w: 20, btn: 'X' },
            { k: 'Id', t: 'Id', hide: true }, { k: 'OrderId', t: 'OrderId', hide: true }, { k: 'OrderDetailId', t: 'OrderDetailId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70, hide: true },
            { k: 'ItemId', t: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLotId', t: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 100 },
            { k: 'PackingTypeId', t: 'PackingTypeId', hide: true }, { k: 'PackingType', t: 'PackingType', w: 100 }, { k: 'PackUomId', t: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 80 },
            numCol('ItemQty', 'ItemQty', 80, q2, { sum: true }), numCol('GrossWeight', 'GrossWeight', 100, q3, { sum: true }), numCol('WeightCut', 'WeightCut', 80, q2, { sum: true }),
            numCol('WeightCutTotal', 'WeightCutTotal', 90, q2, { sum: true }), numCol('AddLessWeight', 'AddLessWeight', 90, q2), numCol('NetBillWeight', 'NetBillWeight', 100, q3, { sum: true }),
            numCol('StockWeight', 'StockWeight', 100, q3, { sum: true }), numCol('ItemRate', 'ItemRate', 90, rate),
            { k: 'RateUomId', t: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 80 }, numCol('RateCut', 'RateCut', 80, rate), numCol('RateCutTotal', 'RateCutTotal', 100, amt, { sum: true }),
            numCol('ItemAmount', 'ItemAmount', 100, amt, { sum: true }), { k: 'WarehouseId', t: 'WarehouseId', hide: true }, { k: 'Warehouse', t: 'Warehouse', w: 110 },
            { k: 'CityId', t: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 100 }, { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate' }, { k: 'GpNo', t: 'GpNo', w: 70 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            numCol('ExchangeRate', 'ExchangeRate', 90, rate), numCol('FcyAmount', 'FcyAmount', 100, fcy, { sum: true }), numCol('BillAmount', 'BillAmount', 100, amt, { sum: true }),
            numCol('Expense', 'Expense', 90, amt, { sum: true }), numCol('Journal', 'Journal', 90, amt, { sum: true }), numCol('Commission', 'Commission', 90, amt, { sum: true }),
            numCol('Freight', 'Freight', 90, amt, { sum: true }), { k: 'Remarks', t: 'Remarks', w: 200 }
        ];
        G.grd = SE.grid('grd', { cols: cols, frozen: 2, dec: S.fm.amtDec, onBtn: function (k, r, i) { calc('grid', i, '', k === 'Add' ? 'Add' : 'Delete'); }, onDbl: function (r, i) { calc('edit', i); } });

        G.fr = SE.grid('grdFreight', { cols: [
            { k: 'TransporterId', t: 'Charge To Product', w: 150, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            numCol('Percentage', '%', 50, plain, { edit: true }), numCol('Qty', 'Qty', 80, plain, { edit: true }), numCol('Rate', 'Rate', 80, rate, { edit: true }),
            numCol('Freight', 'Credit', 80, amt, { sum: true, edit: true }), { k: 'Remarks', t: 'Remarks', w: 120, edit: true },
            { k: 'Delete', t: 'X', w: 20, btn: 'X' }, { k: 'Add', t: '+', w: 20, btn: '+' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'TransporterId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('freightCell', i, k); },
           onBtn: function (k, r, i) { calc('freightBtn', i, '', k); } });

        G.ex = SE.grid('grdInvExp', { cols: [
            { k: 'ItemId', t: 'Item', w: 250, edit: true, list: function () { return S.otherItems; }, lk: 'Id', lt: 'OtherItemName' },
            numCol('Qty', 'Qty', 90, plain, { edit: true }), numCol('Rate', 'Rate', 90, amt, { edit: true }),
            numCol('Amount', 'Amount', 100, amt, { sum: true, edit: true }), { k: 'Remarks', t: 'Remarks', w: 250, edit: true },
            { k: 'Delete', t: 'Delete', w: 60, btn: 'Delete' }, { k: 'Add', t: 'Add New', w: 60, btn: 'Add' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'ItemId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('expCell', i, k); },
           onBtn: function (k, r, i) { calc('expBtn', i, '', k); } });

        G.gl = SE.grid('grdGLedger', { cols: [
            { k: 'AccountId', t: 'Account', w: 250, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            { k: 'Remarks', t: 'Remarks', w: 200, edit: true }, numCol('Percentage', 'Percentage', 90, plain, { edit: true }),
            numCol('Qty', 'Qty', 80, plain, { edit: true }), numCol('Rate', 'Rate', 90, amt, { edit: true }),
            numCol('Debit', 'Debit', 100, amt, { sum: true, edit: true }), numCol('Credit', 'Credit', 100, amt, { sum: true, edit: true }),
            { k: 'Delete', t: 'X', w: 20, btn: 'X' }, { k: 'Add', t: '+', w: 20, btn: '+' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'AccountId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('glCell', i, k); },
           onBtn: function (k, r, i) { calc('glBtn', i, '', k); } });

        /* history : Edit / Slip / Voucher buttons, then the 24 columns of the history table, FrozenColumns = 3 */
        G.hist = SE.grid('grdHistory', { frozen: 3, onDbl: histOpen, onBtn: histButton, onLink: histLink, onSel: histSelected, dec: S.fm.amtDec, cols: [
            { k: 'Edit', t: 'Edit', w: 45, btn: 'Edit' }, { k: 'View', t: 'Slip', w: 45, btn: 'Slip' }, { k: 'Voucher', t: 'Voucher', w: 60, btn: 'Voucher' },
            { k: 'Id', t: 'Id', hide: true }, { k: 'VoucherHeadId', t: 'VoucherHeadId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' },
            { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 90 }, { k: 'DelievryDays', t: 'DelievryDays', w: 80 }, { k: 'CommAgent', t: 'CommAgent', w: 150 },
            { k: 'CommType', t: 'CommType', w: 80 }, numCol('CommRate', 'CommRate', 80, rate), { k: 'CommAmount', t: 'CommAmount', w: 90, f: 'amt', cls: 'num' }, { k: 'CommRemarks', t: 'CommRemarks', w: 120 },
            { k: 'PaymentTerm', t: 'PaymentTerm', w: 110 }, { k: 'DueDays', t: 'DueDays', w: 70 }, { k: 'DueDate', t: 'DueDate', w: 90, f: 'sdate' }, { k: 'ExpiryDate', t: 'ExpiryDate', w: 90, f: 'sdate' },
            numCol('TotalQty', 'TotalQty', 80, q2), numCol('TotalWeight', 'TotalWeight', 90, q3), { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt' }, numCol('ExchangeRate', 'ExchangeRate', 90, rate),
            numCol('FcyAmount', 'FcyAmount', 100, fcy), { k: 'FcyCode', t: 'FcyCode', w: 70 }, { k: 'EntryUser', t: 'EntryUser', w: 110 },
            { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100, link: true }, { k: 'Remarks', t: 'Remarks', w: 200 }
        ] });

        /* grdDetail : the 25 columns of GetDetailGrdByHeadId */
        G.hd = SE.grid('grdDetail', { dec: S.fm.amtDec, cols: [
            { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingType', t: 'PackingType', w: 100 }, { k: 'PackUom', t: 'PackUom', w: 80 },
            numCol('ItemQty', 'ItemQty', 80, q2), numCol('GrossWeight', 'GrossWeight', 100, q3), numCol('WeightCut', 'WeightCut', 80, q2), numCol('WeightCutTotal', 'WeightCutTotal', 90, q2),
            numCol('AddLess', 'Add/Less', 80, q2), numCol('NetBillWeight', 'NetBillWeight', 100, q3), numCol('NetStockWeight', 'NetStockWeight', 100, q3),
            numCol('ItemRate', 'ItemRate', 90, rate), { k: 'RateUom', t: 'RateUom', w: 80 }, numCol('RateCut', 'RateCut', 70, rate), numCol('RateCutAmount', 'RateCutAmount', 100, amt),
            numCol('ItemAmount', 'ItemAmount', 100, amt), { k: 'WareHouseName', t: 'WareHouseName', w: 110 }, { k: 'CityName', t: 'CityName', w: 100 }, { k: 'GpNo', t: 'GpNo', w: 70 },
            { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, numCol('ExchangeRate', 'ExchangeRate', 90, rate), numCol('FcyAmount', 'FcyAmount', 100, fcy), numCol('FreightAmount', 'FreightAmount', 90, amt),
            numCol('CommAmount', 'CommAmount', 90, amt), numCol('ExpenseAmount', 'ExpenseAmount', 90, amt)
        ] });
    }

    /* the grids come back from Reset() with one blank row each */
    function blanks() {
        G.grd.setRows([]); G.gl.setRows([{ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }]);
        G.ex.setRows([{ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }]); G.fr.setRows([{ TransporterId: 0, Percentage: 0, Qty: 0, Rate: 0, Freight: 0, Remarks: '' }]);
        grdSettings();
    }

    /* ------------------------------------------------------------------ Reset :1768 (the server resets the state, the order of the desktop) */
    function reset() {
        S.files = []; S.removed = []; S.existing = []; S.delLines = [];
        S.id = 0; S.approved = false; S.vhId = 0; S.updateIndex = -1; S.canSave = true; S.saveMode = true;
        SS.dis(cb.cust, false);
        cb.agent.clear(); cb.type.clear(); cb.uom.clear(); cb.dterm.clear();
        blanks();
        first(cb.branch); first(cb.project);
        show('btnSave', true); show('btnUpdate', false);
        return calc('reset').then(function () { cb.cust.focus(); });
    }

    /* btnRefresh_Click :3275 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            S.accounts = d.accounts; S.otherItems = d.otherItems;
            cb.branch.setData(d.branches); first(cb.branch); cb.project.setData(d.projects); first(cb.project);
            S.customers = d.customers; cb.cust.setData(S.customers); cb.agent.setData(S.customers);
            cb.term.setData(d.paymentTerms); var t = cb.term.rows()[1]; if (t) cb.term.setValue(t.Id);
            cb.dterm.setData(d.deliveryTerms); cb.cur.setData(d.currencies); cb.type.setData(d.commTypes); cb.uom.setData(d.commUoms);
            cb.item.setData(d.items); cb.lot.setData(d.jobLots); cb.pack.setData(d.packingTypes); cb.wh.setData(d.warehouses);
            G.fr.refresh(); G.ex.refresh(); G.gl.refresh();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load Order (btnLoadSaleOrder_Click :4237 -> SaleOrderLoad_St) */
    function loadOrder() {
        return openLoader().then(function (ids) {
            if (!ids || !ids.length) return;
            return SE.api(API + '/load', { method: 'POST', body: { orderDetailIds: ids, state: collect() } }).then(function (st) {
                if (st.supplierId > 0) SS.dis(cb.cust, true);
                return applyState(st, true).then(function () { return showMsgs(st.messages); });
            }).catch(function (e) { return msg(e.message, 'Error Message'); });
        });
    }
    function opts(rows) { return '<option value="0"></option>' + rows.map(function (r) { return '<option value="' + r.Id + '">' + String(r.Name == null ? '' : r.Name).replace(/&/g, '&amp;').replace(/</g, '&lt;') + '</option>'; }).join(''); }

    /* SaleOrderLoad_St : Resolves to the OrderDetailIds of the checked rows, or [] when the form was closed. */
    function openLoader() {
        return new Promise(function (resolve) {
            var html = '<div class="dpan" style="position:absolute;left:0px;top:0px;width:1082px;height:66px;background:#008080;"><span class="dl white" style="left:8px;top:6px;font-weight:bold;font-size:14px;">Sale Order Load</span></div>' +
                '<span class="dl" style="left:8px;top:72px;font-weight:bold;">From Date</span><input class="f" type="date" id="ld_from" style="left:75px;top:70px;width:110px;height:23px;">' +
                '<span class="dl" style="left:8px;top:100px;font-weight:bold;">To Date</span><input class="f" type="date" id="ld_to" style="left:75px;top:98px;width:110px;height:23px;">' +
                '<span class="dl" style="left:200px;top:72px;font-weight:bold;">CustomerName</span><select class="f" id="ld_cust" style="left:286px;top:68px;width:351px;height:26px;"></select>' +
                '<span class="dl" style="left:200px;top:100px;font-weight:bold;">Item Name</span><select class="f" id="ld_item" style="left:286px;top:96px;width:351px;height:26px;"></select>' +
                '<button type="button" class="dbtn" id="ld_search" style="left:641px;top:68px;width:71px;height:26px;background:#008080;color:#ffffff;">Search</button>' +
                '<button type="button" class="dbtn" id="ld_load" style="left:641px;top:96px;width:71px;height:26px;background:#008080;color:#ffffff;">Load</button>' +
                '<span class="dl white" style="left:0px;top:126px;background:#008080;width:1082px;height:22px;line-height:22px;box-sizing:border-box;font-weight:bold;padding-left:4px;">Sale Order History</span>' +
                '<div class="dgrid" id="ld_grd" tabindex="0" style="position:absolute;left:0px;top:148px;width:1082px;height:330px;"></div>';
            var done = false;
            var m = SS.modal('SaleOrderLoad', html, 1082, 490, function () { if (!done) { done = true; resolve([]); } });
            var gm = SE.grid(m.q('#ld_grd'), { cols: [
                { k: '_sel', t: 'Select', w: 50, sel: true }, { k: 'OrderType', t: 'OrderType', w: 90 }, { k: 'OrderId', t: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 80 },
                { k: 'OrderDetailId', t: 'OrderDetailId', hide: true }, { k: 'OrderSupCustId', t: 'OrderSupCustId', hide: true }, { k: 'PartyName', t: 'PartyName', w: 220 },
                { k: 'ItemId', t: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemUomId', t: 'ItemUomId', hide: true }, { k: 'PackUOM', t: 'PackUOM', w: 80 },
                numCol('OrderQTY', 'OrderQTY', 90, function (v) { return SE.num(v, 0, 0); }, { sum: true }), numCol('OrderWeight', 'OrderWeight', 100, function (v) { return SE.num(v, 0, 0); }, { sum: true }),
                numCol('DispatchedWeight', 'DispatchedWeight', 110, function (v) { return SE.num(v, 0, 0); }, { sum: true }), numCol('BalWeight', 'BalWeight', 100, function (v) { return SE.num(v, 0, 0); }, { sum: true })
            ] });
            SE.api(API + '/loader/combos', { quiet: true }).then(function (d) {
                m.q('#ld_cust').innerHTML = opts(d.customers || []); m.q('#ld_item').innerHTML = opts(d.items || []);
                m.q('#ld_from').value = d.fromDate || SE.today();
            }).catch(function (e) { msg(e.message, 'Error Message'); });
            m.q('#ld_from').value = SE.today(); m.q('#ld_to').value = SE.today();
            function pending() {                                                          // PendingSaleOrderLoad :55 (the desktop sends only the organization and the company)
                return SE.api(API + '/loader/pending').then(function (rows) { gm.setRows(rows); }).catch(function (e) { return msg(e.message, 'Error Message'); });
            }
            m.q('#ld_search').onclick = pending;
            m.q('#ld_load').onclick = function () {                                       // btnLoadOnInvoice_Click_1
                var checked = gm.checked();
                if (checked.length === 0) { msg('No Row is Selected', 'Error Message'); return; }
                var no = null, ids = [];
                for (var i = 0; i < checked.length; i++) {
                    var r = checked[i];
                    if (no === null) no = String(r.OrderNo);
                    if (no !== String(r.OrderNo)) { msg('Sorry! Select Same OrderNo Rows', 'Error Message'); return; }
                    ids.push(ti(r.OrderDetailId));
                }
                done = true; m.close(); resolve(ids);
            };
            pending();
        });
    }

    /* ------------------------------------------------------------------ Save (Insert :1423) */
    function insert(id) {
        if (G.grd.rows().length === 0) return msg('Grid Record Not Found', 'Database Error');
        return SE.ask(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            var st = collect(); st.id = id;
            var body = { state: st, attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed } };
            return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                return showMsgs(r.messages).then(function () { return SE.alert(r.message); }).then(function () {
                    var voucher = $('ChkVoucherPreview').checked, slip = $('ChkPrintslip').checked;
                    return reset().then(function () {
                        if (voucher) SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: r.voucherHeadId, documentTypeId: 1510 });
                        if (slip) SE.printRpt('1516-SaleInvoiceCustomerBill.rpt', { id: r.id });
                    });
                });
            }).catch(function (e) { return msg(e.message, 'Database Error'); });
        });
    }
    function doSave() { S.id = 0; return insert(0); }
    function doUpdate() {
        if (S.id === 0) return msg('Record not update because Id not found', 'Database Error');
        return insert(S.id);
    }

    /* prints */
    function printVoucher() { SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: S.vhId, documentTypeId: 1510 }); }
    function printSlip(id) { if (id > 0) SE.printRpt('1516-SaleInvoiceCustomerBill.rpt', { id: id }); else msg('No Record Found For Display', 'Message'); }
    function printSlipA(id) { if (id > 0) SE.printRpt('1516A-SaleBillDirectSupplierBill.rpt', { id: id }); else msg('Record Not Found For Display', 'Message'); }

    /* ------------------------------------------------------------------ ReadById */
    function readById(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            tabs.select('tabForm');
            SS.dis(cb.cust, true);
            S.approved = !!d.approved; S.vhId = d.voucherHeadId || 0;
            return applyState(d.state, true).then(function () {
                S.existing = d.attachments || []; S.files = []; S.removed = [];
                show('btnSave', false); show('btnUpdate', true);
            });
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */
    function showHistory() {
        var type = $('rdentrydate').checked ? 'entry' : $('rdmodifydate').checked ? 'modify' : $('rdapproveddate').checked ? 'approved' : 'document';
        var q = { dateType: type, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
                  fromNo: ti($('FromDocNoHistory').value), toNo: ti($('ToDocNoHistory').value), customerId: +cb.hcust.value() || 0, paymentTermId: +cb.hpay.value() || 0, deliveryTerm: cb.hdel.text() };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (rows.length > 0) G.hist.setRows(rows); else { G.hist.setRows([]); G.hd.setRows([]); }
        }).catch(fail);
    }
    function resetHistory() {
        $('FromDateHistory').value = SE.addDays(-S.historyDays); $('ToDateHistory').value = SE.today(); $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hcust.clear(); cb.hpay.clear(); cb.hdel.clear(); G.hist.setRows([]); G.hd.setRows([]);
    }
    function histOpen(r) { S.id = ti(r.Id); return readById(S.id); }
    function histButton(k, r) {
        if (k === 'Edit') return reset().then(function () { return readById(ti(r.Id)); });
        if (k === 'View') return printSlip(ti(r.Id));
        if (k === 'Voucher') return SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: ti(r.VoucherHeadId), documentTypeId: 1510 });
    }
    function histLink(k, r) {
        if (k !== 'NoOfAttachments') return;
        if (r && ti(r.Id) > 0) return SS.showAttachments(API, ti(r.Id));
    }
    function histSelected(r) {
        var seq = ++S.histSeq;
        if (!r) { G.hd.setRows([]); return; }
        SE.api(API + '/' + r.Id + '/history-detail', { quiet: true }).then(function (rows) { if (seq === S.histSeq) G.hd.setRows(rows); })
            .catch(function (e) { if (seq === S.histSeq) SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ keys (InvfrmPurchasedirectInvoice_KeyDown_1) */
    function onKey(e) {
        SS.enterTab(e);
        var k = (e.key || '').toLowerCase(), tab = tabs.index();
        if (e.ctrlKey && k === 's') { e.preventDefault(); if (tab === 0) { if (visible('btnSave')) doSave(); } else showHistory(); }
        if (e.ctrlKey && k === 'n') { e.preventDefault(); if (tab === 0) reset(); else resetHistory(); }
        if (e.ctrlKey && k === 'r') { e.preventDefault(); if (tab === 0) refresh(); else $('BtnRefreshHistory').click(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tab === 1) tabs.select('tabForm'); else tabs.select('tabHistory'); }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); }
        if (e.ctrlKey && k === 'u') { e.preventDefault(); if (visible('btnUpdate')) doUpdate(); }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); if (visible('btnPrint')) printVoucher(); }
        if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); $('btnAttachment').click(); }
        if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); if (tab === 0) cb.item.focus(); else $('FromDateHistory').focus(); }
        if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var g = $(tab === 0 ? 'grd' : 'grdHistory'); if (g) g.focus(); }
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
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') $('FromDateHistory').focus(); else cb.item.focus(); });
        tabs2 = SE.tabs('tabControl2');
        ['FromDocNoHistory', 'ToDocNoHistory', 'txtduedays', 'txtDeliveryDays'].forEach(function (k) { SE.digitsOnly($(k)); });
        ['txtItemQty', 'txtGrossWeight', 'txtWtCut', 'txtRate', 'txtratecut', 'txtExchangeRate', 'txtCommRate'].forEach(function (k) { decimalOnly($(k)); });
        makeGrids(); blanks();
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.fm = d.formats || S.fm; S.accounts = d.accounts; S.otherItems = d.otherItems; S.customers = d.customers; S.defaults = d.defaults || {};
            S.historyDays = d.historyDays;
            G.grd.spec.dec = S.fm.amtDec; G.hist.spec.dec = S.fm.amtDec; G.hd.spec.dec = S.fm.amtDec;
            cb.branch.setData(d.branches); first(cb.branch); cb.project.setData(d.projects); first(cb.project);
            cb.cust.setData(S.customers); cb.agent.setData(S.customers);
            cb.term.setData(d.paymentTerms); var t = cb.term.rows()[1]; if (t) cb.term.setValue(t.Id);
            cb.dterm.setData(d.deliveryTerms); cb.type.setData(d.commTypes); cb.uom.setData(d.commUoms); cb.cur.setData(d.currencies);
            cb.item.setData(d.items); cb.lot.setData(d.jobLots); cb.pack.setData(d.packingTypes); cb.wh.setData(d.warehouses); cb.city.setData(d.cities);
            cb.hcust.setData((d.history || {}).customers || []); cb.hpay.setData((d.history || {}).paymentTerms || []); cb.hdel.setData((d.history || {}).deliveryTerms || []);
            show('btnSave', true); show('btnUpdate', false);
            $('btnSave').disabled = !S.rights.save; $('btnPrint').disabled = !S.rights.print; $('btnUpdate').disabled = !S.rights.update; show('btnDelete', false);
            $('DocDate').value = SE.today(); $('duedate').value = SE.today(); $('DeliveryStartDate').value = SE.today(); $('txtgpdate').value = SE.today();
            $('FromDateHistory').value = SE.addDays(-S.historyDays); $('ToDateHistory').value = SE.today();
            return reset();
        }).catch(fail);
        var rec = SE.param('record'); if (rec) setTimeout(function () { readById(+rec); }, 800);
    }

    function wire() {
        $('btnNew').onclick = reset; $('btnSave').onclick = doSave; $('btnUpdate').onclick = doUpdate; $('btnRefresh').onclick = refresh;
        $('btnAttachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('btnLoadSaleOrder').onclick = loadOrder;
        $('btnPrint').onclick = printVoucher;
        $('btnSlip').onclick = function () { printSlip(S.id); };
        $('btn294APrint').onclick = function () { printSlipA(S.id); };
        $('btnShow').onclick = showHistory; $('btnResetHistory').onclick = resetHistory;
        $('BtnRefreshHistory').onclick = function () { SE.api(API + '/history-combos').then(function (d) { cb.hcust.setData(d.customers); cb.hpay.setData(d.paymentTerms); cb.hdel.setData(d.deliveryTerms); }).catch(fail); };
        $('btnAdd').onclick = function () { calc('add'); };
        $('btnUpdateDetail').onclick = function () { calc('update'); };
        $('btnCancelUpdateDetial').onclick = function () { calc('cancelUpdate'); };
        /* TextChanged handlers of the entry panel and of the header */
        [ ['txtItemQty', 'qty'], ['txtGrossWeight', 'gross'], ['txtWtCut', 'wtcut'], ['txtAddLss', 'addless'], ['txtRemarksDetail', 'remarks'], ['txtRate', 'rate'], ['txtratecut', 'ratecut'],
         ['txtCommRate', 'comm'], ['txtduedays', 'due'], ['txtExchangeRate', 'exch'], ['txtDeliveryDays', 'delDays']].forEach(function (p) {
            $(p[0]).addEventListener('input', function () { calc(p[1]); });
        });
        $('DeliveryStartDate').addEventListener('change', function () { calc('delStart'); });
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
