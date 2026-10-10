/*
 * Screen 543  SaleInvoice_St  (Architecture.WinApp.Steel.Sale.SaleInvoice_St, Steel Sale Invoice, document type 1509)
 * Page script. Desktop methods are named in the comments (SaleInvoice_St.cs). Server: /sale/steel/sale-invoice/api
 *
 * The totals of the desktop are recalculated by its grid / text box events; the server holds the same handlers (SaleStInvoiceCalc) and answers the
 * new state of the form for every event ("calc"), so the figures cannot differ between the screen and the save.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/steel/sale-invoice/api', fail = SS.fail;
    var S = { id: 0, approved: false, soid: 0, orderType: '', vhId: 0, rights: {}, fm: { amtDec: 2, fcyDec: 2, rateDec: 2 }, files: [], removed: [], existing: [], customers: [],
              accounts: [], otherItems: [], defaults: {}, histSeq: 0, historyDays: 3, glDebit: 0, glCredit: 0, supCom: 0, q: Promise.resolve() };
    var cb = {}, G = {}, tabs, tabs2;

    function field(id) { return ($(id).value || '').trim(); }
    function tn(v) { return SE.toNum(v); }
    function ti(v) { var n = Number(String(v == null ? '' : v).replace(/,/g, '')); return isFinite(n) ? Math.trunc(n) : 0; }
    function msg(t, cap) { return SE.alert(t, cap); }
    function show(id, on) { var b = $(id); if (b) b.style.display = on ? '' : 'none'; }
    function visible(id) { return SS.visible(id); }
    function setCombo(c, id, key) { var k = key || 'Id', hit = id != null && id !== '' && c.rows().some(function (r) { return String(r[k]) === String(id); }); if (hit) c.setValue(id); else c.clear(); }
    function setText(c, text, key) {                                                       // ComboBox.Text = value
        var hit = null; c.rows().forEach(function (r) { if (hit == null && String(r[key]) === String(text)) hit = r; });
        if (hit) c.setValue(hit.Id); else c.clear();
    }
    function first(c) { var r = c.rows()[0]; if (r) c.setValue(r.Id); }                    // Rows[0].Activate()
    function amt(v) { return SE.num(v, S.fm.amtDec, S.fm.amtDec); }
    function fcy(v) { return SE.num(v, S.fm.fcyDec, S.fm.fcyDec); }
    function rate(v) { return SE.num(v, S.fm.rateDec, S.fm.rateDec); }
    function q2(v) { return v == null || v === '' ? '' : SE.num(v, 2, 0); }                // "#,##0.##"
    function guard(fn, cap) { try { return fn(); } catch (e) { return msg(e.message, cap || 'Error Message'); } }
    function clean(rows) { return JSON.parse(JSON.stringify(rows, function (k, v) { return k.charAt(0) === '_' ? undefined : v; })); }
    function dayText(d) { return d.getFullYear() + '-' + SE.pad2(d.getMonth() + 1) + '-' + SE.pad2(d.getDate()); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.branch = XCombo('CmbBranch', { columns: [{ key: 'BranchName', caption: 'Branch Name' }], textKey: 'BranchName' });
        cb.project = XCombo('CmbProject', { columns: [{ key: 'ProjectName', caption: 'Project Name' }], textKey: 'ProjectName' });
        cb.cust = XCombo('CmbSupplier', { columns: [{ key: 'CompanyName', caption: 'Supplier Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: supplierChanged });
        cb.agent = XCombo('CmbCommAgent', { columns: [{ key: 'CompanyName', caption: 'Commission Agent' }], textKey: 'CompanyName', popupWidth: 420, onSelect: function () { calc('comm'); } });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription' });
        cb.cur = XCombo('cmbCurrency', { columns: [{ key: 'CurrencyCode', caption: 'Currency' }], textKey: 'CurrencyCode' });
        cb.type = XCombo('CmbCommType', { columns: [{ key: 'CommissionType', caption: 'CommissionType' }], textKey: 'CommissionType', onSelect: function () { calc('comm'); } });
        cb.uom = XCombo('CmbCommUom', { columns: [{ key: 'Uom', caption: 'UOM' }], textKey: 'Uom', onSelect: function () { calc('comm'); } });
        cb.hcust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Name', caption: 'Customer Name' }], textKey: 'Name', popupWidth: 380 });
        cb.hpay = XCombo('CmbPaymentTermHistory', { columns: [{ key: 'Name', caption: 'Payment Term' }], textKey: 'Name' });
        cb.hdel = XCombo('CmbDeliveryTermHistory', { columns: [{ key: 'Name', caption: 'Delivery Term' }], textKey: 'Name' });
    }

    /* cmbsuppliername_ValueChanged :3250 - txtSupplierGLId = the GlAccountId of the party */
    function supplierChanged() {
        var id = +cb.supplierValue(), r = S.customers.filter(function (x) { return +x.Id === id; })[0];
        $('txtSupplierGLId').value = r ? String(r.GlAccountId == null ? '' : r.GlAccountId) : '';
    }
    cb.supplierValue = function () { return cb.cust ? cb.cust.value() : 0; };

    /* ------------------------------------------------------------------ the state of the form <-> the server */
    function collect() {
        return {
            id: S.id, branchId: +cb.branch.value() || 0, projectId: +cb.project.value() || 0, docNo: $('txtDocNo').value, docDate: $('DocDate').value,
            supplierId: +cb.cust.value() || 0, supplierGlId: ti($('txtSupplierGLId').value), refNo: $('txtRefNo').value, billNo: $('txtBillNo').value,
            paymentTermId: +cb.term.value() || 0, dueDays: $('txtDueDays').value, dueDate: $('DueDate').value, deliveryTerm: $('txtDeliveryTerm').value,
            currencyId: +cb.cur.value() || 0, exchangeRate: $('txtExchangeRate').value, totalQty: $('txtTotalQty').value, totalWeight: $('txtTotalWeight').value,
            fcyAmount: $('txtFcyAmount').value, billAmount: $('txtBillAmount').value, commAgentId: +cb.agent.value() || 0, commType: cb.type.text(), commRate: $('txtcommrate').value,
            commUom: cb.uom.text(), commAmount: $('txtcommamount').value, commRemarks: $('txtCommRemarks').value, remarks: $('txtRemarks').value, orderType: S.orderType, soid: S.soid,
            glDebit: S.glDebit, glCredit: S.glCredit, supplierCommission: S.supCom,
            lines: clean(G.grd.rows()), freight: clean(G.fr.rows()), exp: clean(G.ex.rows()), gl: clean(G.gl.rows())
        };
    }

    function grdSettings() {                                                              // grdSettings :525 - what depends on SOID and on the first row
        var rows = G.grd.rows(), r0 = rows[0];
        G.grd.spec.cols.forEach(function (c) {
            if (c.k === 'SaleOrder') c.hide = !!r0 && ti(r0.SaleOrder) === 0;
            if (c.k === 'WtCut') c.hide = !!r0 && tn(r0.WtCut) === 0;
            if (c.k === 'WtCutTotal') c.hide = !!r0 && tn(r0.WtCutTotal) === 0;
            if (c.k === 'ItemRate') c.edit = S.soid === 0;
            if (c.k === 'RateUom') c.hide = S.soid === 0;
            if (c.k === 'EquivalentSoRate') { c.hide = S.soid !== 0; c.edit = true; }
        });
        G.grd.refresh();
    }

    /* texts and grids that the events change; full = also the combos and the header (a loaded record) */
    function applyState(st, full) {
        $('txtcommamount').value = st.commAmount == null ? '' : st.commAmount;
        $('txtBillAmount').value = st.billAmount == null ? '' : st.billAmount;
        $('txtTotalQty').value = st.totalQty == null ? '' : st.totalQty;
        $('txtTotalWeight').value = st.totalWeight == null ? '' : st.totalWeight;
        $('txtFcyAmount').value = st.fcyAmount == null ? '' : st.fcyAmount;
        S.glDebit = st.glDebit || 0; S.glCredit = st.glCredit || 0; S.supCom = st.supplierCommission || 0;
        if (full) {
            $('txtDocNo').value = st.docNo || ''; if (st.docDate) $('DocDate').value = st.docDate;
            setCombo(cb.branch, st.branchId); setCombo(cb.project, st.projectId);
            setCombo(cb.cust, st.supplierId); supplierChanged();
            $('txtRefNo').value = st.refNo || ''; $('txtBillNo').value = st.billNo || '';
            setCombo(cb.term, st.paymentTermId);
            $('txtDueDays').value = st.dueDays || ''; $('DueDate').value = st.dueDate || SE.today();
            $('txtDeliveryTerm').value = st.deliveryTerm || '';
            setCombo(cb.cur, st.currencyId);
            $('txtExchangeRate').value = st.exchangeRate || '';
            setCombo(cb.agent, st.commAgentId);
            setText(cb.type, st.commType, 'CommissionType');
            $('txtcommrate').value = st.commRate || '';
            setText(cb.uom, st.commUom, 'Uom');
            $('txtCommRemarks').value = st.commRemarks || '';
            $('txtRemarks').value = st.remarks || '';
            S.orderType = st.orderType || ''; S.soid = st.soid || 0;
        }
        G.grd.setRows(st.lines || []); G.fr.setRows(st.freight || []); G.ex.setRows(st.exp || []); G.gl.setRows(st.gl || []);
        grdSettings();
    }

    function showMsgs(list) {
        var p = Promise.resolve();
        (list || []).forEach(function (m) { p = p.then(function () { return msg(m, 'Message'); }); });
        return p;
    }

    /* one desktop event on the server; calls are queued so the answers cannot overtake each other */
    function calc(ev, row, col, key) {
        S.q = S.q.then(function () {
            var body = { event: ev, row: row == null ? -1 : row, col: col || '', key: key || '', state: collect() };
            return SE.api(API + '/calc', { method: 'POST', body: body, quiet: true }).then(function (st) { applyState(st, false); return showMsgs(st.messages); });
        }).catch(fail);
        return S.q;
    }

    /* ------------------------------------------------------------------ grids */
    function numCol(k, t, w, fn, extra) { var c = { k: k, t: t || k, w: w || 90, cls: 'num', render: fn || q2 }; if (extra) for (var x in extra) c[x] = extra[x]; return c; }

    function makeGrids() {
        /* grd : 41 columns of dtGrid, in dtGrid order (grdSettings :525) */
        var cols = [
            { k: 'Id', t: 'Id', hide: true }, { k: 'InvGdnId', t: 'InvGdnId', hide: true }, { k: 'InvGdnDetailId', t: 'InvGdnDetailId', hide: true },
            { k: 'SaleOrderId', t: 'SaleOrderId', hide: true }, { k: 'SaleOrderDetailId', t: 'SaleOrderDetailId', hide: true },
            { k: 'SaleOrder', t: 'SaleOrder', w: 80 }, { k: 'ItemId', t: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 },
            { k: 'JobLotId', t: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingTypeId', t: 'PackingTypeId', hide: true }, { k: 'PackingType', t: 'PackingType', w: 100 },
            { k: 'PackUomId', t: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 80 },
            numCol('ItemQty', 'ItemQty', 80, q2, { sum: true }), numCol('GrossWeight', 'GrossWeight', 100, q2, { sum: true }), numCol('WtCut', 'WtCut', 70, q2),
            numCol('WtCutTotal', 'WtCutTotal', 90, q2, { sum: true }), numCol('AddLessWeight', 'AddLessWeight', 90, q2, { sum: true }), numCol('NetBillWeight', 'NetBillWeight', 100, q2, { sum: true }),
            numCol('StockWeight', 'StockWeight', 100, q2, { sum: true }), numCol('ItemRate', 'ItemRate', 90, rate, { edit: true }),
            { k: 'RateUomId', t: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 80 },
            numCol('EquivalentSoRate', 'Rate Uom', 80, function (v) { return v == null || v === '' ? '' : String(v); }, { edit: true }),
            numCol('RateCut', 'RateCut', 80, q2, { edit: true }), numCol('RateCutAmount', 'RateCutAmount', 100, amt, { sum: true }), numCol('ItemAmount', 'ItemAmount', 100, amt, { sum: true }),
            { k: 'WarehouseId', t: 'WarehouseId', hide: true }, { k: 'Warehouse', t: 'Warehouse', w: 110 }, { k: 'CityId', t: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 100 },
            { k: 'GpNo', t: 'GpNo', w: 70 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            numCol('BillAmount', 'BillAmount', 100, amt, { sum: true }), numCol('ExchangeRate', 'ExchangeRate', 90, rate), numCol('FcyAmount', 'FcyAmount', 100, fcy, { sum: true }),
            numCol('Freights', 'Freights', 90, amt, { sum: true }), numCol('Expense', 'Expense', 90, amt, { sum: true }), numCol('Commission', 'Commission', 90, amt, { sum: true }),
            { k: 'Journal', t: 'Journal', hide: true }
        ];
        G.grd = SE.grid('grd', { cols: cols, dec: S.fm.amtDec, onEdit: function (r, k, v, i) {
            if (k === 'RateCut' || k === 'ItemRate' || k === 'EquivalentSoRate') { r[k] = tn(v); calc('grid', i, k); }
        } });

        G.fr = SE.grid('grdFreight', { cols: [
            { k: 'InvGdnId', t: 'InvGdnId', hide: true },
            { k: 'Transporter', t: 'Transporter', w: 120, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            numCol('Freight', 'Credit', 100, amt, { sum: true, edit: true }), { k: 'Debit', t: 'Debit', hide: true },
            { k: 'Delete', t: 'X', w: 20, btn: 'X' }, { k: 'Add', t: '+', w: 20, btn: '+' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'Transporter' ? ti(v) : tn(v); calc('freightCell', i, k); },
           onBtn: function (k, r, i) { calc('freightBtn', i, '', k); } });

        G.ex = SE.grid('grdInvExp', { cols: [
            { k: 'ItemId', t: 'Item', w: 250, edit: true, list: function () { return S.otherItems; }, lk: 'Id', lt: 'OtherItemName' },
            numCol('Qty', 'Qty', 90, function (v) { return v == null || v === '' ? '' : String(v); }, { edit: true }), numCol('Rate', 'Rate', 90, amt, { edit: true }),
            numCol('Amount', 'Amount', 100, amt, { sum: true, edit: true }), { k: 'Remarks', t: 'Remarks', w: 250, edit: true },
            { k: 'Delete', t: 'Delete', w: 60, btn: 'Delete' }, { k: 'Add', t: 'Add New', w: 60, btn: 'Add' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'ItemId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('expCell', i, k); },
           onBtn: function (k, r, i) { calc('expBtn', i, '', k); } });

        G.gl = SE.grid('grdGLedger', { cols: [
            { k: 'AccountId', t: 'Account', w: 250, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            { k: 'Remarks', t: 'Remarks', w: 200, edit: true }, numCol('Percentage', 'Percentage', 90, function (v) { return v == null || v === '' ? '' : String(v); }, { edit: true }),
            numCol('Qty', 'Qty', 80, function (v) { return v == null || v === '' ? '' : String(v); }, { edit: true }), numCol('Rate', 'Rate', 90, amt, { edit: true }),
            numCol('Debit', 'Debit', 100, amt, { sum: true, edit: true }), numCol('Credit', 'Credit', 100, amt, { sum: true, edit: true }),
            { k: 'Delete', t: 'X', w: 20, btn: 'X' }, { k: 'Add', t: '+', w: 20, btn: '+' }
        ], onEdit: function (r, k, v, i) { r[k] = k === 'AccountId' ? ti(v) : (k === 'Remarks' ? v : tn(v)); calc('glCell', i, k); },
           onBtn: function (k, r, i) { calc('glBtn', i, '', k); } });

        /* history : Edit / Slip / Voucher buttons, then the 25 columns of dt (HistoryGridSettings :2273), FrozenColumns = 3 */
        G.hist = SE.grid('grdHistory', { frozen: 3, onDbl: histOpen, onBtn: histButton, onLink: histLink, onSel: histSelected, dec: S.fm.amtDec, cols: [
            { k: 'Edit', t: 'Edit', w: 45, btn: 'Edit' }, { k: 'View', t: 'Slip', w: 45, btn: 'Slip' }, { k: 'Voucher', t: 'Voucher', w: 60, btn: 'Voucher' },
            { k: 'Id', t: 'Id', hide: true }, { k: 'VoucherHeadId', t: 'VoucherHeadId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' },
            { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'DueDate', t: 'DueDate', w: 90, f: 'sdate' }, { k: 'CommAgent', t: 'CommAgent', w: 150 }, { k: 'CommType', t: 'CommType', hide: true },
            numCol('CommRate', 'CommRate', 80, rate), { k: 'CommAmount', t: 'CommAmount', w: 90, f: 'amt', cls: 'num' }, { k: 'CommRemarks', t: 'CommRemarks', hide: true },
            numCol('TotalQty', 'TotalQty', 80, q2), numCol('TotalWeight', 'TotalWeight', 90, q2), { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt' }, numCol('ExchangeRate', 'ExchangeRate', 90, rate),
            numCol('FcyAmount', 'FcyAmount', 100, fcy), { k: 'UserName', t: 'UserName', w: 110 }, { k: 'EntryDate', t: 'EntryDate', w: 90, f: 'sdate' }, { k: 'ModifyUser', t: 'ModifyUser', w: 110 },
            { k: 'ModifyDate', t: 'ModifyDate', w: 90, f: 'sdate' }, { k: 'IsApproved', t: 'Approved', w: 90 }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 90, f: 'sdate' },
            { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 100, link: true }
        ] });

        /* grdDetail : the 27 columns of GetDetailGrdByHeadId (DetailGridSetting :1046) */
        G.hd = SE.grid('grdDetail', { dec: S.fm.amtDec, cols: [
            { k: 'SaleOrder', t: 'SaleOrder', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingType', t: 'PackingType', w: 100 }, { k: 'PackUom', t: 'PackUom', w: 80 },
            numCol('ItemQty', 'ItemQty', 80, q2), numCol('GrossWeight', 'GrossWeight', 100, q2), numCol('WeightCut', 'WeightCut', 80, q2), numCol('WeightCutTotal', 'WeightCutTotal', 90, q2),
            numCol('AddLessWeight', 'AddLessWeight', 90, q2), numCol('NetBillWeight', 'NetBillWeight', 100, q2), numCol('NetStockWeight', 'NetStockWeight', 100, q2),
            numCol('ItemRate', 'ItemRate', 90, rate), { k: 'RateUom', t: 'RateUom', w: 80 }, numCol('RateCut', 'RateCut', 70, q2), numCol('RateCutAmount', 'RateCutAmount', 100, amt),
            numCol('ItemAmount', 'ItemAmount', 100, amt), { k: 'WareHouse', t: 'WareHouse', w: 110 }, { k: 'CityName', t: 'CityName', w: 100 }, { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate' },
            { k: 'GpNo', t: 'GpNo', w: 70 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, numCol('ExchangeRate', 'ExchangeRate', 90, rate), numCol('FcyAmount', 'FcyAmount', 100, fcy),
            numCol('BillAmount', 'BillAmount', 100, amt), numCol('CommAmount', 'CommAmount', 90, amt), numCol('ExpenseAmount', 'ExpenseAmount', 90, amt), numCol('FreightAmount', 'FreightAmount', 90, amt)
        ] });
    }

    /* the grids come back from Reset() with one blank row each */
    function blanks() {
        G.grd.setRows([]); G.gl.setRows([{ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }]);
        G.ex.setRows([{ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }]); G.fr.setRows([{ InvGdnId: 0, Transporter: 0, Freight: 0, Debit: 0 }]);
        grdSettings();
    }

    /* ------------------------------------------------------------------ Reset :2337 */
    function defaultConfig() {                                                            // defaultConfiquration :1254
        var d = S.defaults || {};
        if (d.currencyId != null) setCombo(cb.cur, d.currencyId);
        if (d.exchangeRate != null) { $('txtExchangeRate').value = d.exchangeRate; $('txtTotalQty').value = '0'; $('txtTotalWeight').value = '0'; $('txtFcyAmount').value = '0'; }   // txtExchangeRate_TextChanged on an empty grid
    }
    function reset() {
        S.files = []; S.removed = []; S.existing = [];
        S.orderType = ''; S.soid = 0; S.id = 0; S.approved = false; S.vhId = 0; S.glDebit = 0; S.glCredit = 0; S.supCom = 0;
        SS.dis(cb.cust, false);
        cb.agent.clear(); cb.type.clear(); cb.uom.clear();
        ['txtcommrate', 'txtcommamount', 'txtCommRemarks', 'txtBillNo', 'txtRefNo', 'txtDueDays', 'txtExchangeRate', 'txtTotalQty', 'txtTotalWeight', 'txtFcyAmount', 'txtBillAmount'].forEach(function (k) { $(k).value = ''; });
        cb.cur.clear();
        blanks();
        first(cb.branch); first(cb.project);
        cb.cust.setData(S.customers);
        show('btnSave', true); show('btnUpdate', false);
        cb.cust.focus();
        defaultConfig();
        return SE.api(API + '/next-no').then(function (d) { if (d.nextNo > 0) $('txtDocNo').value = String(d.nextNo); else throw new Error('Max Number Not Found'); }).catch(fail);
    }

    /* btnRefresh_Click :3166 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            S.accounts = d.accounts; S.otherItems = d.otherItems;
            cb.branch.setData(d.branches); first(cb.branch); cb.project.setData(d.projects); first(cb.project);
            S.customers = d.customers; cb.cust.setData(S.customers); cb.agent.setData(S.customers);
            cb.term.setData(d.paymentTerms); var t = cb.term.rows()[1]; if (t) cb.term.setValue(t.Id);
            G.fr.refresh(); G.ex.refresh(); G.gl.refresh();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load Gdn (toolStripButton3_Click_1 :2950 -> frmLoadGDN_St) */
    function loadGdn() {
        return openLoader().then(function (ids) {
            var body = { gdnIds: ids || [], state: collect() };
            return SE.api(API + '/load', { method: 'POST', body: body }).then(function (st) {
                if (st.supplierId > 0) SS.dis(cb.cust, true);
                applyState(st, true);
                return showMsgs(st.messages);
            }).catch(function (e) { return msg(e.message, 'Error Message'); });
        });
    }

    /* frmLoadGDN_St : GDN Load. Resolves to the ids of the checked rows, or [] when the form was closed. */
    function openLoader() {
        return new Promise(function (resolve) {
            var html = '<div class="dpan" style="position:absolute;left:0px;top:0px;width:1000px;height:30px;background:#008080;"><span class="dl white" style="left:8px;top:6px;font-weight:bold;font-size:14px;">GDN Load</span></div>' +
                '<span class="dl" style="left:8px;top:42px;font-weight:bold;">From Date</span><input class="f" type="date" id="ld_from" style="left:70px;top:38px;width:130px;height:23px;">' +
                '<span class="dl" style="left:215px;top:42px;font-weight:bold;">To Date</span><input class="f" type="date" id="ld_to" style="left:268px;top:38px;width:130px;height:23px;">' +
                '<button type="button" class="dbtn" id="ld_search" style="left:410px;top:37px;width:70px;height:25px;background:#008080;color:#ffffff;">Search</button>' +
                '<button type="button" class="dbtn" id="ld_load" style="left:486px;top:37px;width:70px;height:25px;background:#008080;color:#ffffff;">Load</button>' +
                '<span class="dl white" style="left:0px;top:70px;background:#008080;width:1000px;height:22px;line-height:22px;box-sizing:border-box;font-weight:bold;padding-left:4px;">Main Grid</span>' +
                '<div class="dgrid" id="ld_grd" tabindex="0" style="position:absolute;left:0px;top:92px;width:1000px;height:250px;"></div>' +
                '<span class="dl white" style="left:0px;top:346px;background:#008080;width:1000px;height:22px;line-height:22px;box-sizing:border-box;font-weight:bold;padding-left:4px;">Detail Grid</span>' +
                '<div class="dgrid" id="ld_det" tabindex="0" style="position:absolute;left:0px;top:368px;width:1000px;height:272px;"></div>';
            var done = false;
            var m = SS.modal('GDN Load', html, 1000, 640, function () { if (!done) { done = true; resolve([]); } });
            var gm = SE.grid(m.q('#ld_grd'), { cols: [
                { k: '_sel', t: 'Select', w: 50, sel: true }, { k: 'Id', t: 'Id', hide: true }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'date' }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'GdnType', t: 'GdnType', w: 100 },
                { k: 'RefDocId', t: 'RefDocId', hide: true }, { k: 'SupplierCustomerId', t: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 240 },
                { k: 'GpNO', t: 'GpNO', w: 70 }, { k: 'BiltyNo', t: 'BiltyNo', w: 100 }, { k: 'VehicleNo', t: 'VehicleNo', w: 100 }, numCol('ItemQty', 'ItemQty', 90, q2)
            ], onSel: function (r) {
                if (!r) { gd.setRows([]); return; }
                SE.api(API + '/loader/detail' + SE.q({ id: r.Id }), { quiet: true }).then(function (rows) { gd.setRows(rows); }).catch(function (e) { msg(e.message, 'Error Message'); });
            } });
            var gd = SE.grid(m.q('#ld_det'), { cols: [
                { k: 'SaleOrder', t: 'SaleOrder', w: 80 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingType', t: 'PackingType', w: 100 }, { k: 'PackUom', t: 'PackUom', w: 80 },
                numCol('ItemQty', 'ItemQty', 80, q2), numCol('GrossWight', 'GrossWight', 90, q2), numCol('WtCut', 'WtCut', 70, q2), numCol('WtCutTotal', 'WtCutTotal', 90, q2), numCol('AddLesswt', 'AddLesswt', 80, q2),
                numCol('NetWeight', 'NetWeight', 90, q2), numCol('StockWeight', 'StockWeight', 90, q2), { k: 'WareHouse', t: 'WareHouse', w: 110 }, { k: 'LabNo', t: 'LabNo', w: 90 }, { k: 'City', t: 'City', w: 100 }
            ] });
            function pending() {                                                          // PendingGdnLoad :84
                return SE.api(API + '/loader/pending' + SE.q({ from: m.q('#ld_from').value, to: m.q('#ld_to').value })).then(function (rows) { if (rows.length > 0) gm.setRows(rows); })
                    .catch(function (e) { return msg(e.message, 'Error Message'); });
            }
            m.q('#ld_from').value = S.yearStart || SE.today(); m.q('#ld_to').value = SE.today();
            m.q('#ld_search').onclick = pending;
            m.q('#ld_load').onclick = function () {                                       // btnLoadOnInvoice_Click_1 :206
                var checked = gm.checked();
                if (checked.length === 0) { msg('Chek the row first', 'Error Message'); return; }
                var sid = 0, type = 0, ids = [];
                for (var i = 0; i < checked.length; i++) {
                    var r = checked[i], sc = ti(r.SupplierCustomerId), ref = ti(r.RefDocId);
                    if (sc !== 0) {
                        if (sid === 0) sid = sc;
                        if (type === 0) type = ref;
                        if (type !== ref) { msg('Sorry!. The Selected Gdn\'s are not of same Type'); return; }
                        if (sid !== sc) { msg('Sorry!. The Selected Gdn\'s are not of same Customer'); return; }
                        ids.push(ti(r.Id)); sid = sc; type = ref;
                    }
                }
                done = true; m.close(); resolve(ids);
            };
            pending();
        });
    }

    /* ------------------------------------------------------------------ Save (Insert :2514) */
    function insert(id) {
        if (G.grd.rows().length === 0) return msg('Grid Record Not Found', 'Database Error');
        return SE.ask(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            var st = collect(); st.id = id;
            var body = { state: st, attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed } };
            return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                return showMsgs(r.messages).then(function () { return SE.alert(r.message); }).then(function () {
                    var voucher = $('ChkBok').checked, slip = $('ChkPrintSlip').checked;
                    return reset().then(function () {
                        if (voucher) SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: r.voucherHeadId, documentTypeId: 1509 });
                        if (slip) SE.printRpt('1514-InvRepSaleBillCustomer.rpt', { id: r.id });
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
    function printVoucher() { SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: S.vhId, documentTypeId: 95 }); }     // btnPrint_Click :1578 -> GenerateReportVoucher103 (document type 95)
    function printSlip(id) { if (id > 0) SE.printRpt('1514-InvRepSaleBillCustomer.rpt', { id: id }); else msg('No Record Found For Display', 'Message'); }
    function printSlipA(id) { if (id > 0) SE.printRpt('1514A-SaleBillCustomer-Format-II.rpt', { id: id }); else msg('Record Not Found For Display', 'Message'); }

    /* ------------------------------------------------------------------ ReadById :2399 */
    function readById(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            tabs.select('tabForm');
            SS.dis(cb.cust, true);
            S.approved = !!d.approved; S.vhId = d.voucherHeadId || 0;
            applyState(d.state, true);
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            show('btnSave', false); show('btnUpdate', true);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history (Historyfill :2158) */
    function showHistory() {
        var type = $('rdentrydate').checked ? 'entry' : $('rdmodifydate').checked ? 'modify' : $('rdapproveddate').checked ? 'approved' : 'document';
        var q = { dateType: type, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
                  fromNo: ti($('FromDocNoHistory').value), toNo: ti($('ToDocNoHistory').value), customerId: +cb.hcust.value() || 0, paymentTermId: +cb.hpay.value() || 0, deliveryTerm: cb.hdel.text() };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (rows.length > 0) G.hist.setRows(rows); else { G.hist.setRows([]); G.hd.setRows([]); }
        }).catch(fail);
    }
    function resetHistory() {                                                             // btnResetHistory_Click :2077
        $('FromDateHistory').value = SE.addDays(-S.historyDays); $('ToDateHistory').value = SE.today(); $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hcust.clear(); cb.hpay.clear(); cb.hdel.clear(); G.hist.setRows([]); G.hd.setRows([]);
    }
    function histOpen(r) { S.id = ti(r.Id); return readById(S.id); }                       // grdHistory_DoubleClick :3266
    function histButton(k, r) {                                                           // grdHistory_ColumnButtonClick :962
        if (k === 'Edit') return reset().then(function () { return readById(ti(r.Id)); });
        if (k === 'View') return printSlip(ti(r.Id));
        if (k === 'Voucher') return SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: ti(r.VoucherHeadId), documentTypeId: 1509 });
    }
    function histLink(k) {                                                                // grdHistory_LinkClicked :3201 (it reads the attachments of the form's own Id)
        if (k !== 'NoOfAttachments') return;
        if (S.id > 0) return SS.showAttachments(API, S.id);
        SE.pop('Attachments', '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody></tbody></table></div>', [{ t: 'Close' }]).open();
    }
    function histSelected(r) {                                                            // grdHistory_SelectionChanged :3279
        var seq = ++S.histSeq;
        if (!r) { G.hd.setRows([]); return; }
        SE.api(API + '/' + r.Id + '/history-detail', { quiet: true }).then(function (rows) { if (seq === S.histSeq) G.hd.setRows(rows); })
            .catch(function (e) { if (seq === S.histSeq) SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ keys (InvfrmPurchaseInvoice_KeyDown :2860) */
    function onKey(e) {
        SS.enterTab(e);
        var k = (e.key || '').toLowerCase(), tab = tabs.index();
        if (e.ctrlKey && k === 's' && tab === 0) { e.preventDefault(); if (visible('btnSave')) doSave(); }
        if (e.ctrlKey && k === 'n' && tab === 0) { e.preventDefault(); reset(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tab === 1) tabs.select('tabForm'); else tabs.select('tabHistory'); }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); }
        if (e.ctrlKey && k === 'u') { e.preventDefault(); if (visible('btnUpdate')) doUpdate(); }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); if (visible('btnPrint')) printVoucher(); }
        if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); $('btnAttachment').click(); }
        if (e.ctrlKey && k === 'l') { e.preventDefault(); loadGdn(); }
    }

    /* ------------------------------------------------------------------ load (InvfrmPurchaseInvoice_Load :1602) */
    function decimalOnly(el) {                                                            // CommonServices.OnlytextdecimelFunction
        el.addEventListener('keypress', function (e) {
            if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault();
            else if (e.key === '.' && el.value.indexOf('.') >= 0) e.preventDefault();
        });
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') $('FromDateHistory').focus(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2');
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); decimalOnly($('txtExchangeRate'));
        makeGrids(); blanks();
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.fm = d.formats || S.fm; S.accounts = d.accounts; S.otherItems = d.otherItems; S.customers = d.customers; S.defaults = d.defaults || {};
            S.historyDays = d.historyDays;
            G.grd.spec.dec = S.fm.amtDec; G.hist.spec.dec = S.fm.amtDec; G.hd.spec.dec = S.fm.amtDec;
            cb.branch.setData(d.branches); first(cb.branch); cb.project.setData(d.projects); first(cb.project);
            cb.cust.setData(S.customers); cb.agent.setData(S.customers);
            cb.term.setData(d.paymentTerms); var t = cb.term.rows()[1]; if (t) cb.term.setValue(t.Id);
            cb.type.setData(d.commTypes); cb.uom.setData(d.commUoms); cb.cur.setData(d.currencies);
            cb.hcust.setData((d.history || {}).customers || []); cb.hpay.setData((d.history || {}).paymentTerms || []); cb.hdel.setData((d.history || {}).deliveryTerms || []);
            if (d.nextNo > 0) $('txtDocNo').value = String(d.nextNo); else msg('Max Number Not Found', 'Error Message');
            show('btnSave', true); show('btnUpdate', false);
            $('btnSave').disabled = !S.rights.save; $('btnPrint').disabled = !S.rights.print; $('btnUpdate').disabled = !S.rights.update; show('btnDelete', false);
            $('DocDate').value = SE.today(); $('DueDate').value = SE.today();
            defaultConfig();
            $('FromDateHistory').value = SE.addDays(-S.historyDays); $('ToDateHistory').value = SE.today();
            cb.cust.focus();
        }).catch(fail);
        var rec = SE.param('record'); if (rec) setTimeout(function () { readById(+rec); }, 800);
    }

    function wire() {
        $('btnNew').onclick = reset; $('btnSave').onclick = doSave; $('btnUpdate').onclick = doUpdate; $('btnRefresh').onclick = refresh;
        $('btnAttachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('toolStripButton3').onclick = loadGdn;
        $('btnPrint').onclick = printVoucher;
        $('toolStripButton1').onclick = function () { printSlip(S.id); };
        $('btnSlipFormat2').onclick = function () { printSlipA(S.id); };
        $('btnShow').onclick = showHistory; $('btnResetHistory').onclick = resetHistory;
        $('btnRefreshHistory').onclick = function () { SE.api(API + '/history-combos').then(function (d) { cb.hcust.setData(d.customers); cb.hpay.setData(d.paymentTerms); cb.hdel.setData(d.deliveryTerms); }).catch(fail); };
        /* TextChanged / Leave handlers */
        $('txtDueDays').addEventListener('input', function () {                            // txtDueDays_TextChanged :3147
            var t = $('txtDueDays').value.trim();
            var days = t === '' ? 0 : tn(t);
            $('DueDate').value = dayText(new Date(Date.now() + Math.round(days * 86400000)));
        });
        $('txtExchangeRate').addEventListener('input', function () { calc('exch'); });     // txtExchangeRate_TextChanged :3326
        $('txtcommrate').addEventListener('input', function () { calc('comm'); });         // txtcommrate_TextChanged :1547
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
