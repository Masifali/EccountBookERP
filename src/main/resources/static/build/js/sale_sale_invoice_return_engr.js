/*
 * Screen 537  frmSaleInvoiceReturn_Engr  (Architecture.WinApp.SaleTrading.frmSaleInvoiceReturn_Engr, document type 1611)
 * Page script. Desktop methods are named in the comments (sirT.cs). Server: /sale/engr/sale-invoice-return/api
 * The grid arithmetic (grd_CellUpdated, FreightProportion, LedgerProportion, BillAmount, BillProportion) lives on the server (POST /calc, the same
 * code Save runs); this page runs the entry panel (CalculateWeight / CalculateNetWeightAgainstAddLess / CalculateAmount / CalculateTaxAmount) and
 * the cell logic of the two small grids.
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/sale-invoice-return/api', DOC = 1611;
    var S = {
        id: 0, rights: {}, files: [], removedAtt: [], existing: [], voucherHeadId: 0, updateIdx: -1,
        branches: [], projects: [], cust: [], terms: [], dterms: [], accounts: [], taxTypes: [], items: [], wh: [], jobs: [], cities: [], uoms: [],
        taxEditable: false, fmt: { amountRound: 0, amount: 0, rate: 2 }, branchId: 0, lastTaxAc: 0
    };
    var cb = {}, G = {}, tabs, seq = 0;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function ci(r, k) { if (!r) return null; if (k in r) return r[k]; var l = String(k).toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return null; }
    function pick(r, ks) { for (var i = 0; i < ks.length; i++) { var v = ci(r, ks[i]); if (v != null) return v; } return null; }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function vid(c) { return +c.value() || 0; }
    function num(id) { return SE.toNum($(id).value); }
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }
    function fmtRate(v) { var d = S.fmt.rate; return SE.num(v, d, d); }
    function f3(v) { return SE.num(v, 3, 0); }
    function rnd(v, dec) { var p = Math.pow(10, Math.max(0, dec)); return (v < 0 ? -1 : 1) * Math.round(Math.abs(v) * p + 1e-9) / p; }
    function bankers(v) { var f = Math.floor(v), d = v - f; if (d < 0.5) return f; if (d > 0.5) return f + 1; return f % 2 === 0 ? f : f + 1; }   // Math.Round(double)
    function iso(d) { return d.getFullYear() + '-' + SE.pad2(d.getMonth() + 1) + '-' + SE.pad2(d.getDate()); }
    function parseIso(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? new Date(+m[1], +m[2] - 1, +m[3]) : null; }
    function addDays(s, n) { var d = parseIso(s) || new Date(); d.setDate(d.getDate() + n); return iso(d); }
    function dayDiff(a, b) { var x = parseIso(a), y = parseIso(b); return x && y ? Math.round((x - y) / 86400000) : 0; }
    function isNumeric(t) { return /^\s*-?\d+(\.\d+)?\s*$/.test(String(t).replace(/,/g, '')); }
    function saveMode() { return isShown('btnSave') && !$('btnSave').disabled; }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.branch = XCombo('cmbbranch', { columns: [{ key: 'BranchName', caption: 'Branch Name' }], textKey: 'BranchName', popupWidth: 260 });
        cb.project = XCombo('cmbproject', { columns: [{ key: 'ProjectName', caption: 'Project Name' }], textKey: 'ProjectName', popupWidth: 260 });
        cb.cust = XCombo('cmbsuppliername', { columns: [{ key: 'RefName', caption: 'Party Name' }], textKey: 'RefName', popupWidth: 420 });
        cb.dterm = XCombo('CmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.term = XCombo('CmbPaymentTerm', { columns: [{ key: 'TermsDescription', caption: 'Payment Term' }], textKey: 'TermsDescription', onSelect: termChanged });
        cb.tax = XCombo('CmbTaxAccount', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 360 });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'RefName', caption: 'WareHouse' }], textKey: 'RefName', popupWidth: 260 });
        cb.item = XCombo('CmbItem', { columns: [{ key: 'RefName', caption: 'Item Name' }], textKey: 'RefName', popupWidth: 420, onSelect: itemLeave });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'RefName', caption: 'JobLot' }], textKey: 'RefName', popupWidth: 300 });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: packChanged });
        cb.rate = XCombo('CmbRateUom', { columns: [{ key: 'UOMCode', caption: 'RateUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: rateChanged });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName', popupWidth: 320 });
        cb.ttype = XCombo('CmbTaxType', { columns: [{ key: 'TaxName', caption: 'TaxType' }], valueKey: 'TaxNameId', textKey: 'TaxName', onSelect: taxTypePicked });
        cb.hCust = XCombo('cmbSupplierNameHistory', { columns: [{ key: 'RefName', caption: 'Party Name' }], textKey: 'RefName', popupWidth: 380 });
    }
    function keepFill(c, rows, keepId) { c.setData(rows); if (keepId > 0 && !c.setValue(keepId)) c.clear(); }
    function applyLists(d) {
        S.taxEditable = !!d.taxEditable;
        S.branches = d.branches || []; S.projects = d.projects || []; S.cust = d.customers || []; S.items = d.items || []; S.jobs = d.jobLots || []; S.wh = d.warehouses || [];
        S.accounts = d.accounts || []; S.taxTypes = d.taxTypes || [];
        if (d.terms) S.terms = d.terms;
        if (d.deliveryTerms) S.dterms = d.deliveryTerms;
        if (d.cities) S.cities = d.cities;
        S.fmt = d.fmt || S.fmt;
        G.grd.spec.dec = S.fmt.amount; G.hist.spec.dec = S.fmt.amount; G.hd.spec.dec = S.fmt.amount; G.fr.spec.dec = S.fmt.amount; G.gl.spec.dec = S.fmt.amount;
    }
    function fillAll() {
        keepFill(cb.branch, S.branches, vid(cb.branch)); keepFill(cb.project, S.projects, vid(cb.project));
        keepFill(cb.cust, S.cust, vid(cb.cust)); keepFill(cb.hCust, S.cust, vid(cb.hCust)); keepFill(cb.term, S.terms, vid(cb.term)); keepFill(cb.dterm, S.dterms, vid(cb.dterm));
        keepFill(cb.tax, S.accounts, vid(cb.tax)); keepFill(cb.wh, S.wh, vid(cb.wh)); keepFill(cb.item, S.items, vid(cb.item)); keepFill(cb.job, S.jobs, vid(cb.job));
        keepFill(cb.city, S.cities, vid(cb.city)); keepFill(cb.ttype, S.taxTypes, vid(cb.ttype));
        G.fr.refresh(); G.gl.refresh(); G.grd.refresh();
    }

    /* ------------------------------------------------------------------ server calculation */
    function reqBody(withAtt) {
        var b = {
            id: S.id, docNo: $('txtdocno').value, docDate: $('DocDate').value, branchId: vid(cb.branch), projectId: vid(cb.project), customerId: vid(cb.cust),
            deliveryTerm: cb.dterm.text() || '', paymentTermId: vid(cb.term), dueDays: $('txtDueDays').value, dueDate: $('DueDate').value, taxAccountId: vid(cb.tax),
            manualBillNo: $('txtbillno').value, refNo: $('txtRefrenceNo').value, remarks: $('txtRemarks').value,
            lines: G.grd.rows(), freights: G.fr.rows(), journals: G.gl.rows()
        };
        if (withAtt) b.attachments = { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt };
        return b;
    }
    function applyCalc(r) {
        G.grd.setRows(r.lines || []);
        $('txtBillAmount').value = fmtAmt(+r.billAmount || 0);
        G.grd.refresh();
    }
    /* FreightProportion + LedgerProportion + BillAmount + BillProportion, as Save runs them */
    function recalc() {
        var my = ++seq;
        return SE.api(API + '/calc', { method: 'POST', body: reqBody(false), quiet: true }).then(function (r) { if (my === seq) applyCalc(r); }).catch(function (e) { if (my === seq) fail(e); });
    }
    function itemTotal() { return G.grd.rows().reduce(function (a, r) { return a + (+r.ItemAmount || 0); }, 0); }

    /* ------------------------------------------------------------------ header events */
    function termChanged() {                                                                 // CmbPaymentTerm_TextChanged
        if ((cb.term.text() || '').trim() === 'Cash') { $('txtDueDays').disabled = true; $('txtDueDays').value = ''; $('DueDate').value = $('DocDate').value; }
        else $('txtDueDays').disabled = false;
    }
    function dueDaysChanged() {                                                              // txtDueDays_TextChanged
        var t = $('txtDueDays').value.trim();
        $('DueDate').value = t !== '' ? addDays($('DocDate').value, SE.toInt(t)) : $('DocDate').value;
    }
    function dueDateChanged() {                                                              // DueDate_ValueChanged
        if (!$('DueDate').value) return;
        if ($('DueDate').value < $('DocDate').value) $('DueDate').value = $('DocDate').value;
        $('txtDueDays').value = String(dayDiff($('DueDate').value, $('DocDate').value));
    }
    function docDateLeave() {                                                                // DocDate_Leave
        var p = Promise.resolve();
        if (G.grd.count() > 0 && saveMode()) {
            var ids = ''; G.grd.rows().forEach(function (r) { ids += ',' + r.ItemId; });
            p = SE.api(API + '/tax-by-items' + SE.q({ itemIds: ids, date: $('DocDate').value })).then(function (dt) {
                G.grd.rows().forEach(function (it) {
                    var hit = null; dt.forEach(function (x) { if (+ci(x, 'ItemId') === +it.ItemId) hit = x; });
                    if (hit) { it.TaxTypeId = +ci(hit, 'TaxNameId') || 0; it.TaxPct = +ci(hit, 'TaxPercent') || 0; }
                    else { it.TaxTypeId = 0; it.TaxPct = 0; }
                    if (!it.calc || it.calc === 'tax' || it.calc === 'taxc') it.calc = 'dl';
                });
            });
        }
        return p.then(function () { dueDaysChanged(); return SE.api(API + '/tax-types' + SE.q({ date: $('DocDate').value })); })
            .then(function (tt) { S.taxTypes = tt; keepFill(cb.ttype, S.taxTypes, vid(cb.ttype)); G.grd.refresh(); return recalc(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ entry panel */
    function calcWeight() {                                                                  // CalculateWeight
        var r = cb.uom.row(), q = num('txtItemQty');
        if (r && vid(cb.uom) > 0) { var w = q > 0 ? q * (+r.Equivalent || 0) : 0; $('txtGrossWeight').value = f3(w); $('txtNetBillWeight').value = f3(w); }
        else { $('txtGrossWeight').value = '0'; $('txtNetBillWeight').value = '0'; }
    }
    function calcNet() {                                                                     // CalculateNetWeightAgainstAddLess
        var add = num('txtAddLss');
        $('txtNetBillWeight').value = f3(add !== 0 ? num('txtGrossWeight') + add : num('txtGrossWeight'));
    }
    function calcAmount() {                                                                  // CalculateAmount
        var ru = cb.rate.row(), eq = ru && vid(cb.rate) > 0 ? (+ru.Equivalent || 0) : 0;
        var net = num('txtNetBillWeight'), rate = num('txtRate'), cut = num('txtratecut');
        if (net > 0 && rate > 0 && eq > 0) {
            var amount = net / eq * rate, rc = cut > 0 ? net / eq * cut : 0;
            amount -= rc;
            $('txtratecuttotal').value = fmtAmt(rnd(rc, S.fmt.amount)); $('txtAmount').value = fmtAmt(rnd(amount, S.fmt.amount));
        } else { $('txtAmount').value = '0'; $('txtratecuttotal').value = '0'; }
    }
    function calcTax() {                                                                     // CalculateTaxAmount
        var amt = num('txtAmount'), tr = cb.ttype.row();
        if (tr && vid(cb.ttype) > 0) {
            var pct;
            if (!(num('txtTaxPercent') > 0)) { pct = +tr.TaxPercent || 0; $('txtTaxPercent').value = f3(pct); }
            else pct = num('txtTaxPercent');
            if (pct > 100) { msg("Tax % Can't be greater than 100..."); pct = 100; $('txtTaxPercent').value = '100'; }
            var t = amt * pct / 100;
            $('txtTaxAmount').value = fmtAmt(rnd(t, S.fmt.amount)); $('txtTotalAmount').value = fmtAmt(rnd(amt + t, S.fmt.amount));
        } else $('txtTotalAmount').value = fmtAmt(amt);
    }
    function calcAll() { calcWeight(); calcNet(); calcAmount(); calcTax(); }
    function calcFromNet() { calcNet(); calcAmount(); calcTax(); }
    function packChanged() { calcAll(); }                                                    // CmbPackUom_TextChanged
    function rateChanged() { calcFromNet(); }                                                // CmbRateUom_TextChanged
    function taxTypePicked() {                                                               // CmbTaxType_Leave
        if (!S.taxEditable) $('txtTaxPercent').value = '';                                   // the percent box is locked: it follows the type
        if (cb.ttype.row()) calcTax();
        else { $('txtTaxPercent').value = ''; $('txtTaxAmount').value = ''; $('txtTotalAmount').value = fmtAmt(num('txtAmount')); }
    }
    function taxPercentChanged() {                                                           // txtTaxPercent_TextChanged
        if (cb.ttype.row()) calcTax();
        else { $('txtTaxPercent').value = ''; $('txtTaxAmount').value = ''; $('txtTotalAmount').value = fmtAmt(num('txtAmount')); }
    }

    function byCode(rows, code) { for (var i = 0; i < rows.length; i++) if (String(rows[i].UOMCode) === String(code)) return rows[i]; return null; }
    function itemLeave() {                                                                   // CmbItem_Leave -> BindPackUomAndRateUom
        var pu = cb.uom.text(), ru = cb.rate.text(), id = vid(cb.item);
        cb.uom.clear(); cb.rate.clear();
        if (!(id > 0)) { cb.uom.setData([]); cb.rate.setData([]); return Promise.resolve(); }
        return SE.api(API + '/uoms' + SE.q({ itemId: id })).then(function (rows) {
            S.uoms = rows; cb.uom.setData(rows); cb.rate.setData(rows);
            var a = byCode(rows, pu), b = byCode(rows, ru);
            if (rows.length && a) cb.uom.setValue(a.Id); else cb.uom.clear();
            if (rows.length && b) cb.rate.setValue(b.Id); else cb.rate.clear();
            $('txtTaxPercent').disabled = !S.taxEditable;
            calcAll();
        }).catch(fail);
    }
    function resetDetail() {                                                                 // ResetDetail
        cb.item.clear(); cb.uom.clear(); cb.rate.clear(); cb.ttype.clear();
        ['txtItemQty', 'txtGrossWeight', 'txtAddLss', 'txtNetBillWeight', 'txtRate', 'txtratecut', 'txtratecuttotal', 'txtAmount', 'txtTaxPercent', 'txtTaxAmount',
            'txtgatepassno', 'txtvehicleno', 'txtTotalAmount'].forEach(function (id) { $(id).value = ''; });
    }
    function detailButtons(updating) { show('btnAdd', !updating); show('btnUpdateDetail', updating); show('btnCancelUpdateDetial', updating); }
    function detailValid() {                                                                 // FormValidationDetila
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.wh.row() || !(vid(cb.wh) > 0)) return stop('Warehouse Field is Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || !(vid(cb.item) > 0)) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.job.row() || !(vid(cb.job) > 0)) return stop('Job/Lot Field is Required', function () { cb.job.focus(); });
        if (!cb.uom.row() || !(vid(cb.uom) > 0)) return stop('Pack Uom Field is Required', function () { cb.uom.focus(); });
        if (num('txtItemQty') === 0) return stop('Qty Field is Required', function () { $('txtItemQty').focus(); });
        if (num('txtGrossWeight') === 0) return stop('Gross Weight Field is Required', function () { $('txtGrossWeight').focus(); });
        if (num('txtNetBillWeight') === 0) return stop('Net Bill Weight Field is Required', function () { $('txtNetBillWeight').focus(); });
        if (num('txtRate') === 0) return stop('Item Rate Field is Required', function () { $('txtRate').focus(); });
        if (!cb.rate.row() || !(vid(cb.rate) > 0)) return stop('Rate UOM Field is Required', function () { cb.rate.focus(); });
        if (!cb.city.row() || !(vid(cb.city) > 0)) return stop('CityName Field is Required', function () { cb.city.focus(); });
        return true;
    }
    function panelValues(row) {                                                              // the columns btnAdd / btnUpdateDetail write
        var pu = cb.uom.row(), ru = cb.rate.row();
        row.WarehouseId = vid(cb.wh); row.ItemId = vid(cb.item); row.ItemName = cb.item.text();
        row.JobLotId = vid(cb.job); row.JobLot = cb.job.text();
        row.PackUomId = vid(cb.uom); row.PackUom = cb.uom.text(); row.PackEquivalent = pu ? +pu.Equivalent || 0 : 0;
        row.ItemQty = num('txtItemQty'); row.GrossWeight = num('txtGrossWeight'); row.AdLsWt = num('txtAddLss'); row.NetBillWeight = num('txtNetBillWeight');
        row.ItemRate = num('txtRate'); row.RateUomId = vid(cb.rate); row.RateUom = cb.rate.text(); row.RateEquivalent = ru ? +ru.Equivalent || 0 : 0;
        row.RateCut = num('txtratecut'); row.RateCutTotal = num('txtratecuttotal'); row.ItemAmount = num('txtAmount'); row.CityId = vid(cb.city);
        row.GpDate = $('txtgpdate').value || SE.today(); row.GpNo = SE.toInt($('txtgatepassno').value); row.VehicleNo = $('txtvehicleno').value;
        row.TaxTypeId = vid(cb.ttype); row.TaxPct = num('txtTaxPercent'); row.TaxAmount = num('txtTaxAmount');
        row.calc = 'entry'; row.tc = false;
        return row;
    }
    function btnAdd() {                                                                      // btnAdd_Click
        if (!detailValid()) return;
        var r = panelValues({ Id: 0, OrderId: 0, OrderDetailId: 0, OrderNo: 0, Freights: 0, Journal: 0, BillAmount: 0 });
        G.grd.addRow(r);
        gridUi();
        resetDetail();
        cb.item.focus();
        return recalc();
    }
    function gridDbl(row, i) {                                                               // grd_DoubleClick
        if (!row) return;
        S.updateIdx = i;
        cb.wh.setValue(+row.WarehouseId || 0); cb.item.setValue(+row.ItemId || 0);
        return itemLeave().then(function () {
            cb.job.setValue(+row.JobLotId || 0); cb.uom.setValue(+row.PackUomId || 0);
            $('txtItemQty').value = String(row.ItemQty); $('txtGrossWeight').value = String(row.GrossWeight); $('txtAddLss').value = String(row.AdLsWt);
            $('txtNetBillWeight').value = String(row.NetBillWeight); $('txtRate').value = fmtRate(+row.ItemRate || 0);
            cb.rate.setValue(+row.RateUomId || 0); $('txtratecut').value = String(row.RateCut); $('txtratecuttotal').value = String(row.RateCutTotal);
            $('txtAmount').value = fmtAmt(+row.ItemAmount || 0); cb.city.setValue(+row.CityId || 0);
            $('txtgpdate').value = SE.dateInput(row.GpDate) || SE.today(); $('txtgatepassno').value = String(row.GpNo == null ? '' : row.GpNo); $('txtvehicleno').value = row.VehicleNo || '';
            cb.ttype.setValue(+row.TaxTypeId || 0); $('txtTaxPercent').value = String(row.TaxPct == null ? '' : row.TaxPct); $('txtTaxAmount').value = String(row.TaxAmount == null ? '' : row.TaxAmount);
            detailButtons(true); cb.wh.focus();
        });
    }
    function btnUpdateDetail() {                                                             // btnUpdateDetail_Click
        if (!detailValid()) return;
        var row = G.grd.rows()[S.updateIdx];
        if (row) panelValues(row);
        detailButtons(false);
        resetDetail(); cb.item.focus(); S.updateIdx = -1;
        gridUi();
        return recalc();
    }
    function btnCancelUpdateDetail() { detailButtons(false); resetDetail(); S.updateIdx = -1; cb.item.focus(); }   // btnCancelUpdateDetial_Click

    /* ------------------------------------------------------------------ grids */
    function rateRender(v) { return v == null || v === '' ? '' : fmtRate(+v); }
    function makeGrids() {
        G.grd = SE.grid('grd', { frozen: 1, dec: 0, onEdit: gridEdit, onBtn: gridBtn, onDbl: gridDbl, cols: [
            { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'WarehouseId', t: 'Ware House', w: 110, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'RefName' },
            { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 90 }, { k: 'PackUom', t: 'PackUom', w: 60 },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3', edit: true, sum: true }, { k: 'GrossWeight', t: 'GrossWeight', w: 90, f: 'n3', sum: true },
            { k: 'AdLsWt', t: 'AdLsWt', w: 70, f: 'n3', edit: true, sum: true }, { k: 'NetBillWeight', t: 'NetBillWeight', w: 90, f: 'n3', sum: true },
            { k: 'ItemRate', t: 'ItemRate', w: 80, f: 'n2', render: rateRender }, { k: 'RateUom', t: 'RateUom', w: 70 },
            { k: 'RateCut', t: 'RateCut', w: 70, f: 'n2', edit: true, render: rateRender }, { k: 'RateCutTotal', t: 'RateCutTotal', w: 90, f: 'amt', sum: true },
            { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt', sum: true },
            { k: 'CityId', t: 'CityName', w: 100, edit: true, list: function () { return S.cities; }, lk: 'Id', lt: 'CityName' },
            { k: 'GpDate', t: 'GpDate', w: 85, f: 'date' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            { k: 'TaxTypeId', t: 'Tax Type', w: 60, list: function () { return S.taxTypes; }, lk: 'TaxNameId', lt: 'TaxName' },
            { k: 'TaxPct', t: 'Tax%', w: 60, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt', sum: true },
            { k: 'Freights', t: 'Freights', w: 80, f: 'amt', sum: true }, { k: 'Journal', t: 'Journal', w: 80, f: 'amt', sum: true }, { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt', sum: true }] });
        G.fr = SE.grid('grdFreight', { footer: true, frozen: 2, dec: 0, onBtn: frBtn, onEdit: frEdit, cols: [
            { k: '_add', t: '+', w: 26, btn: '+' }, { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'Transporter', t: 'Transporter', w: 220, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            { k: 'Freight', t: 'Credit', w: 80, edit: true, f: 'amt', sum: true }] });
        G.gl = SE.grid('grdGLedger', { footer: true, frozen: 2, dec: 0, onBtn: glBtn, onEdit: glEdit, cols: [
            { k: '_add', t: '+', w: 26, btn: '+' }, { k: '_del', t: 'X', w: 26, btn: 'X' },
            { k: 'AccountId', t: 'Account', w: 250, edit: true, list: function () { return S.accounts; }, lk: 'Id', lt: 'AccountTitle' },
            { k: 'Remarks', t: 'Remarks', w: 140, edit: true }, { k: 'Prcnt', t: 'Prcnt', w: 60, edit: true }, { k: 'Qty', t: 'Qty', w: 60, edit: true }, { k: 'Rate', t: 'Rate', w: 60, edit: true },
            { k: 'Debit', t: 'Debit', w: 80, edit: true, f: 'amt', sum: true }, { k: 'Credit', t: 'Credit', w: 80, edit: true, f: 'amt', sum: true }] });
        G.hist = SE.grid('grdHistory', { frozen: 4, dec: 0, onDbl: function (r) { histEdit(r); }, onBtn: histBtn, onLink: histLink, onSel: histSelected, cols: [
            { k: 'Edit', t: 'Edit', w: 44, btn: 'Edit' }, { k: 'Print1', t: 'Print-1611', w: 84, btn: 'Print-1611' }, { k: 'Print2', t: 'Print-1611A', w: 90, btn: 'Print-1611A' },
            { k: 'Voucher', t: 'Voucher', w: 70, btn: 'Voucher' },
            { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'DueDate', t: 'DueDate', w: 90, f: 'sdate' },
            { k: 'ManualBillNo', t: 'ManualBillNo', w: 90 }, { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'BillAmount', t: 'BillAmount', w: 100, f: 'amt', sum: true },
            { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 },
            { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
            { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 90, link: true }] });
        G.hd = SE.grid('grdDetail', { dec: 0, cols: [
            { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'Warehouse', t: 'Warehouse', w: 120 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 90 }, { k: 'PackUom', t: 'PackUom', w: 60 },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3', sum: true }, { k: 'GrossWeight', t: 'GrossWeight', w: 90, f: 'n3', sum: true }, { k: 'AdLsWt', t: 'AdLsWt', w: 70, f: 'n3', sum: true },
            { k: 'NetBillWeight', t: 'NetBillWeight', w: 90, f: 'n3', sum: true }, { k: 'ItemRate', t: 'ItemRate', w: 80, f: 'n2', render: rateRender }, { k: 'RateUOM', t: 'RateUOM', w: 70 },
            { k: 'RateCut', t: 'RateCut', w: 70, f: 'n2', render: rateRender }, { k: 'RateCutAmount', t: 'RateCutAmount', w: 90, f: 'amt', sum: true }, { k: 'ItemAmount', t: 'ItemAmount', w: 90, f: 'amt', sum: true },
            { k: 'CityName', t: 'CityName', w: 100 }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'date' }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
            { k: 'TaxName', t: 'TaxName', w: 90 }, { k: 'TaxPct', t: 'Tax%', w: 60, f: 'n3' }, { k: 'TaxAmount', t: 'TaxAmount', w: 80, f: 'amt', sum: true }] });
    }
    function colOf(g, k) { return g.spec.cols.filter(function (x) { return x.k === k; })[0]; }
    function gridUi() {                                                                      // grdSettings: what depends on Save mode and TaxPercentEditable
        var c;
        if ((c = colOf(G.grd, '_del'))) c.hide = !saveMode();
        if ((c = colOf(G.grd, 'TaxTypeId'))) c.edit = S.taxEditable;
        if ((c = colOf(G.grd, 'TaxPct'))) c.edit = S.taxEditable;
        G.grd.refresh(); G.hd.refresh();
    }

    /* grd_CellUpdated: the edited cell sets the sticky calc flag, the server runs the formulas */
    function taxPct(id) { var p = 0; S.taxTypes.forEach(function (x) { if (+x.TaxNameId === id) p = +x.TaxPercent || 0; }); return p; }
    function gridEdit(row, key, val) {
        if (key === 'WarehouseId' || key === 'CityId') { row[key] = +val || 0; row.calc = row.calc || 'tax'; }
        else if (key === 'TaxTypeId') {
            if (!S.taxEditable) return;
            var id = +val || 0; row.TaxTypeId = id; row.TaxPct = id === 0 ? 0 : taxPct(id); row.tc = true; row.calc = row.calc || 'taxc';
        } else {
            if (val !== '' && !isNumeric(val)) return;
            var n = SE.toNum(val);
            if (key === 'TaxPct') {
                if (!S.taxEditable) return;
                row.TaxPct = n;
                if (n === 0 && +row.TaxTypeId > 0) row.TaxPct = taxPct(+row.TaxTypeId);
                if (row.TaxPct > 100) { row.TaxPct = 100; msg("Tax % Can't be greater than 100..."); }
                row.tc = true; row.calc = row.calc || 'taxc';
            } else { row[key] = n; row.calc = 'amt'; row.tc = false; }                       // ItemQty, AdLsWt, RateCut
        }
        G.grd.refresh(); recalc();
    }
    function gridBtn(k, row, i) { return delRow(i); }                                        // grd_ColumnButtonClick: Delete, no confirmation
    function delRow(i) { G.grd.removeAt(i); gridUi(); return recalc(); }

    /* grdFreight */
    function frBlank() { return { InvGrnId: 0, Transporter: 0, Freight: 0, Debit: 0 }; }
    function frBtn(k, row, i) {
        if (k === '_del') { G.fr.removeAt(i); if (G.fr.count() === 0) G.fr.addRow(frBlank()); }
        else G.fr.addRow(frBlank());
        recalc();
    }
    function frEdit(row, key, val) {                                                         // grdFreight_CellUpdated
        if (key === 'Transporter') row.Transporter = +val || 0;
        else {
            if (val !== '' && !isNumeric(val)) { G.fr.refresh(); return; }
            row.Freight = SE.toNum(val);
            if ((+row.Debit || 0) > 0) { row.Freight = 0; msg('Debit Side is aleady added'); }
        }
        G.fr.refresh(); recalc();
    }

    /* grdGLedger */
    function glBlank() { return { AccountId: 0, Remarks: '', Prcnt: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function glBtn(k, row, i) {
        if (k === '_del') { G.gl.removeAt(i); if (G.gl.count() === 0) G.gl.addRow(glBlank()); }
        else G.gl.addRow(glBlank());
        recalc();
    }
    function glEdit(row, key, val) {                                                         // grdGLedger_CellUpdated
        if (key === 'Remarks') { row.Remarks = val; G.gl.refresh(); return; }
        if (key === 'AccountId') row.AccountId = +val || 0;
        else {
            var n = SE.toNum(val);
            if (key === 'Qty' || key === 'Rate') {
                row[key] = n; row.Credit = (+row.Qty || 0) * (+row.Rate || 0); row.Debit = 0; row.Prcnt = 0;
            } else if (key === 'Prcnt') {
                row.Prcnt = n;
                var amt = itemTotal() / 100 * n;
                if (amt > 0) { row.Credit = bankers(amt); row.Debit = 0; } else { row.Debit = Math.abs(bankers(amt)); row.Credit = 0; }
                row.Qty = 0; row.Rate = 0;
            } else if (key === 'Credit') {
                row.Credit = n;
                if ((+row.Debit || 0) > 0) { row.Credit = 0; msg('Debit Side is aleady added'); }
            } else if (key === 'Debit') {
                row.Debit = n;
                if ((+row.Credit || 0) > 0) { row.Debit = 0; msg('Credit Side is aleady added'); }
            }
        }
        G.gl.refresh(); recalc();
    }

    /* ------------------------------------------------------------------ Save / Update */
    function formValidation() {                                                              // FormValidation (the server runs it again, word for word)
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.branch.row() || !(vid(cb.branch) > 0)) return stop('Branch Name is Required', function () { cb.branch.focus(); });
        if (!cb.project.row() || !(vid(cb.project) > 0)) return stop('Project Name is Required', function () { cb.project.focus(); });
        if (SE.toInt($('txtdocno').value.trim()) === 0) return stop('Doc No Name is Required', function () { $('txtdocno').focus(); });
        if (!cb.cust.row() || !(vid(cb.cust) > 0)) return stop('Customer Name is Required', function () { cb.cust.focus(); });
        if (!cb.term.row() || !(vid(cb.term) > 0)) return stop('Payment Term is Required', function () { cb.term.focus(); });
        if (vid(cb.term) === 2 && SE.toInt($('txtDueDays').value) === 0) return stop('Due Days Field is Required', function () { $('txtDueDays').focus(); });
        if (!cb.dterm.row() || !(vid(cb.dterm) > 0)) return stop('Delivery Term is Required', function () { cb.dterm.focus(); });
        return true;
    }
    function insert(update) {                                                                // Insert()
        if (!formValidation()) return Promise.resolve();
        return SE.ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API, { method: 'POST', body: reqBody(true) }).then(function (r) {
                return msg(r.message || 'Record Save Successfully').then(function () {
                    var slip = $('ChkBok').checked, vch = $('ChkVoucherPreview').checked;
                    return reset().then(function () {
                        if (slip) printSlip(r.id, r.docNo);
                        if (vch) printVoucher(r.voucherHeadId);
                    });
                });
            });
        }).catch(function (e) { return msg(e.message, 'Message'); });
    }
    function btnSave() { S.id = 0; return insert(false); }
    function btnUpdate() { return insert(true); }
    function setButtons() {
        var editing = S.id > 0;
        show('btnSave', !editing); show('btnUpdate', editing); show('btnDelete', editing);
        $('btnSave').disabled = !S.rights.save; $('btnUpdate').disabled = !S.rights.update; $('btnDelete').disabled = !S.rights['delete'];
        ['btnSaleInvoice1', 'btnSaleinvoice2', 'btnPrint'].forEach(function (id) { $(id).disabled = !S.rights.print; });
        gridUi();
    }

    /* ------------------------------------------------------------------ Reset / Refresh */
    function reset() {                                                                       // Reset(): branch, project, dates, terms, tax account and the entry panel stay as they are
        S.id = 0; S.files = []; S.removedAtt = []; S.existing = []; S.voucherHeadId = 0; S.updateIdx = -1;
        enable(cb.cust, true); cb.cust.clear();
        ['txtbillno', 'txtRemarks', 'txtBillAmount', 'txtcommremarks', 'txtcommrate', 'txtcommamount'].forEach(function (id) { var e = $(id); if (e) e.value = ''; });
        G.grd.setRows([]); G.fr.setRows([frBlank()]); G.gl.setRows([glBlank()]);
        detailButtons(false); setButtons();
        cb.cust.focus();
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }
    function btnNew() { return reset().then(resetDetail); }                                  // btnNew_Click: Reset + ResetDetail
    function refresh() {                                                                     // btnFrmRefresh_Click: BranchFill, ProjectFill, AllComboBind, AccountsFill, GetallTaxType
        return SE.api(API + '/refresh' + SE.q({ date: $('DocDate').value })).then(function (d) {
            applyLists(d); fillAll();
            if (S.branches.length) cb.branch.setValue(S.branches[0].Id);                      // Rows[0].Activate()
            if (S.projects.length) cb.project.setValue(S.projects[0].Id);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            tabs.select('tabForm');
            S.id = id; S.files = []; S.removedAtt = []; S.existing = d.attachments || []; S.voucherHeadId = +d.voucherHeadId || 0; S.updateIdx = -1;
            cb.branch.setValue(+h.BranchesId || 0); cb.project.setValue(+h.ProjectsId || 0);
            $('txtdocno').value = h.DocNo;
            $('txtDueDays').value = h.DueDays == null ? '' : String(h.DueDays); dueDaysChanged();   // set before DocDate, as ReadById does: DueDate follows the date the form held
            $('DocDate').value = SE.dateInput(h.DocDate);
            cb.cust.setValue(+h.SupplierCustomerId || 0); enable(cb.cust, false);
            cb.tax.clear(); if (+h.TaxAccountId > 0) cb.tax.setValue(+h.TaxAccountId);
            $('txtbillno').value = h.ManualBillNo || '';
            $('txtRemarks').value = h.RemarksHeader || '';
            $('txtBillAmount').value = String(h.BillAmount == null ? '' : h.BillAmount);
            G.grd.setRows(d.lines || []);
            G.gl.setRows((d.journals && d.journals.length) ? d.journals : [glBlank()]);
            G.fr.setRows((d.freights && d.freights.length) ? d.freights : [frBlank()]);
            detailButtons(false); setButtons();
            return recalc();                                                                 // FreightProportion, LedgerProportion, BillAmount
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */
    function showHistory() {                                                                 // GetAll (the desktop sends the dates only)
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '' };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            G.hist.setRows(rows.map(function (r) {
                return { Id: ci(r, 'Id'), VoucherHeadId: ci(r, 'VoucherHeadId'), DocNo: ci(r, 'DocNo'), DocDate: ci(r, 'DocDate'), DueDate: ci(r, 'DueDate'), ManualBillNo: ci(r, 'ManualBillNo'),
                    CustomerName: pick(r, ['CustomerName', 'SupplierCustomer', 'Customer']), BillAmount: ci(r, 'BillAmount'), EntryUser: pick(r, ['UserName', 'EntryUser']), EntryDate: ci(r, 'EntryDate'),
                    ModifyUser: pick(r, ['ModifyUserName', 'ModifyUser']), ModifyDate: ci(r, 'ModifyDate'), ApprovedUserName: pick(r, ['ApprovedUserName', 'ApprovedUser']),
                    ApprovedDate: ci(r, 'ApprovedDate'), Remarks: pick(r, ['RemarksHeader', 'Remarks']), NoOfAttachments: ci(r, 'NoOfAttachments') };
            }));
            if (!rows.length) G.hd.setRows([]);
        }).catch(function (e) { return msg(e.message); });
    }
    function resetHistory() {                                                                // btnNewHistory_Click
        var t = SE.today();
        $('FromDateHistory').value = t; $('ToDateHistory').value = t; $('txtFromDocNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        cb.hCust.clear(); G.hist.setRows([]); G.hd.setRows([]); $('drdocdate').checked = true;
    }
    function histSelected(item) {                                                            // grdHistory_SelectionChanged -> BindDetailOfHeaderId
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id + '/history-detail', { quiet: true }).then(function (rows) {
            if (!G.hist.cur() || +G.hist.cur().Id !== +item.Id) return;
            G.hd.setRows(rows);
        }).catch(function () { G.hd.setRows([]); });
    }
    function histEdit(r) {                                                                   // grdHistory_DoubleClick / Edit
        if (!S.rights.update) return msg("You don't have Update Right...");
        return edit(+r.Id);
    }
    function histBtn(k, r) {
        if (k === 'Edit') histEdit(r);
        else if (k === 'Print1') printSlip(+r.Id, +r.DocNo);
        else if (k === 'Print2') printSlip2(+r.Id, +r.DocNo);
        else if (k === 'Voucher') printVoucher(+r.VoucherHeadId);
    }
    function histLink(k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); }

    /* ------------------------------------------------------------------ print / attachments */
    function slipArgs(id, no) { return { id: id, pbmId: id, documentTypeId: DOC, branchesId: S.branchId, fromDocNo: no || 0, toDocNo: no || 0 }; }
    function printSlip(id, no) { SE.printRpt('1611-SaleInvoice_CustomerBillReturn_Engr.rpt', slipArgs(id, no)); }      // GeneratePrint1611
    function printSlip2(id, no) { SE.printRpt('1611A-SaleInvoice_CustomerBillReturn_Engr.rpt', slipArgs(id, no)); }    // GeneratePrint1611A
    function printVoucher(vhId) {                                                            // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1611)
        if (!(vhId > 0)) return msg('No Record Found For Display');
        SE.printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: vhId, documentTypeId: DOC });
    }
    function btnSlipDetail() { if (S.id > 0) return printSlip(S.id, +$('txtdocno').value); return msg('No Record Selected'); }       // btnSlipDetail_Click
    function btnSaleinvoice2() { if (S.id > 0) return printSlip2(S.id, +$('txtdocno').value); return msg('No Record Selected'); }  // btnSaleinvoice2_Click
    function showAttachments(id) { return SE.api(API + '/' + id + '/attachments').then(function (rows) { renderAttachments(rows, id, true); }).catch(fail); }
    function openAttachmentDialog() {
        var rows = (S.existing || []).filter(function (r) { return S.removedAtt.indexOf(r.Id) < 0; });
        renderAttachments(rows, S.id, false);
    }
    function renderAttachments(rows, id, readOnly) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
        rows.forEach(function (r) {
            h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (id > 0 ? '<a class="lnk" href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank">Open</a>' : '') +
                (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
        });
        S.files.forEach(function (f, i) { if (!readOnly) h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
        h += '</tbody></table></div>';
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the invoice is saved.</span></div>';
        var pop = SE.pop('Attachments', h, [{ t: 'Close' }]);
        pop.open();
        pop.body.addEventListener('click', function (e) {
            var rm = e.target.getAttribute && e.target.getAttribute('data-rm');
            if (rm) { S.removedAtt.push(+rm); pop.close(); openAttachmentDialog(); }
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

    /* ------------------------------------------------------------------ keys / wiring */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Alt+1', 'For Print Voucher 103'], ['Alt+2', 'For Print Slip 1611'], ['Alt+3', 'For Print Slip 1611A'], ['Ctrl+F5', 'For Focus on DocDate'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On warehouse Name in Detail Box'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];
    function gridOf(id) { return id === 'grd' ? G.grd : id === 'grdFreight' ? G.fr : id === 'grdGLedger' ? G.gl : null; }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = (e.key || '').toLowerCase();
        if (e.altKey && !e.ctrlKey && tabs.index() === 0) {
            if (e.key === '1') { e.preventDefault(); if (!$('btnPrint').disabled) printVoucher(S.voucherHeadId); }
            else if (e.key === '2') { e.preventDefault(); if (!$('btnSaleInvoice1').disabled) btnSlipDetail(); }
            else if (e.key === '3') { e.preventDefault(); if (!$('btnSaleinvoice2').disabled) btnSaleinvoice2(); }
            return;
        }
        if (!e.ctrlKey) return;
        if (k === 'e') { e.preventDefault(); try { window.history.back(); } catch (x) { } return; }
        if (k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabForm'); $('DocDate').focus(); } else { tabs.select('tabHistory'); $('FromDateHistory').focus(); } return; }
        var tg = t.closest ? t.closest('.dgrid') : null, gid = tg ? tg.id : '';
        if (tabs.index() === 0) {
            if (e.key === 'Delete') {
                var g = gridOf(gid);
                if (g && g.curIndex() >= 0) {
                    e.preventDefault();
                    if (g === G.grd) { if (S.rights.save) delRow(g.curIndex()); }
                    else if (g === G.fr) frBtn('_del', g.cur(), g.curIndex());
                    else glBtn('_del', g.cur(), g.curIndex());
                }
            }
            else if (e.key === 'Enter') { if (gid === 'grd' && G.grd.cur()) { e.preventDefault(); gridDbl(G.grd.cur(), G.grd.curIndex()); } }
            else if (k === 'n') { e.preventDefault(); btnNew(); }
            else if (k === 'r') { e.preventDefault(); refresh(); }
            else if (k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (k === 'd') { if (gid === 'grdFreight') { e.preventDefault(); G.fr.addRow(frBlank()); recalc(); } else if (gid === 'grdGLedger') { e.preventDefault(); G.gl.addRow(glBlank()); recalc(); } }
            else if (e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $('grd').focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.key === 'ArrowRight') { e.preventDefault(); $(gid === 'grd' ? 'grdFreight' : gid === 'grdFreight' ? 'grdGLedger' : 'grd').focus(); }
        } else {
            if (e.key === 'F5') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $('grdDetail').focus(); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.key === 'ArrowRight') { e.preventDefault(); $(gid === 'grdDetail' ? 'grdHistory' : 'grdDetail').focus(); }
            else if (k === 'p') { e.preventDefault(); var hc = G.hist.cur(); if (hc && gid === 'grdHistory') printSlip(+hc.Id, +hc.DocNo); }
            else if (e.key === 'Enter') { var hr = G.hist.cur(); if (hr && gid === 'grdHistory') { e.preventDefault(); histEdit(hr); } }
        }
    }
    function wire() {
        $('btnNew').onclick = btnNew; $('btnFrmRefresh').onclick = refresh; $('btnSave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate;
        $('btnAttachment').onclick = openAttachmentDialog;
        $('btnDelete').onclick = function () { };                                            // btnDelete_Click is empty on the desktop
        $('btnSaleInvoice1').onclick = btnSlipDetail;
        $('btnSaleinvoice2').onclick = btnSaleinvoice2;
        $('btnPrint').onclick = function () { printVoucher(S.voucherHeadId); };               // btnPrint_Click -> GenerateVoucherPrint(RecId)
        $('BtnShortCutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnNewHistory').onclick = resetHistory; $('btnshow').onclick = showHistory;
        $('btnAdd').onclick = btnAdd; $('btnUpdateDetail').onclick = btnUpdateDetail; $('btnCancelUpdateDetial').onclick = btnCancelUpdateDetail;
        $('txtdocno').readOnly = true; $('txtBillAmount').readOnly = true;
        ['txtDueDays', 'txtFromDocNoHistory', 'txtToDocNoHistory', 'txtgatepassno'].forEach(function (id) { SE.digitsOnly($(id)); });
        ['txtItemQty', 'txtGrossWeight', 'txtRate', 'txtratecut'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        });
        $('txtAddLss').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.\-]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        $('txtItemQty').addEventListener('input', calcAll);
        $('txtGrossWeight').addEventListener('input', calcFromNet);
        $('txtAddLss').addEventListener('input', calcFromNet);
        $('txtRate').addEventListener('input', calcFromNet);
        $('txtRate').addEventListener('blur', function () { if ($('txtRate').value !== '') $('txtRate').value = fmtRate(num('txtRate')); });       // txtRate_Leave
        $('txtratecut').addEventListener('input', calcFromNet);
        $('txtTaxPercent').addEventListener('input', taxPercentChanged);
        $('txtDueDays').addEventListener('input', dueDaysChanged);
        $('DueDate').addEventListener('change', dueDateChanged);
        $('DocDate').addEventListener('change', docDateLeave);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    function load() {
        makeCombos(); makeGrids();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabHistory') $('FromDateHistory').focus(); else $('DocDate').focus(); });
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.branchId = d.branchId || 0; S.lastTaxAc = +d.lastTaxAcId || 0;
            applyLists(d); fillAll();
            $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : '';
            var t = SE.today(); $('DocDate').value = t; $('DueDate').value = t; $('txtgpdate').value = t;
            $('FromDateHistory').value = t; $('ToDateHistory').value = t;
            $('ChkBok').checked = false; $('ChkVoucherPreview').checked = false;
            G.fr.setRows([frBlank()]); G.gl.setRows([glBlank()]);
            if (S.branches.length) cb.branch.setValue(S.branches[0].Id);                      // Rows[0].Activate()
            if (S.projects.length) cb.project.setValue(S.projects[0].Id);
            if (S.terms.length > 1) cb.term.setValue(S.terms[1].Id);                          // Rows[1].Activate()
            if (S.dterms.length > 0) cb.dterm.setValue(S.dterms[0].Id);                       // Rows[0].Activate()
            termChanged();
            if (S.lastTaxAc > 0) cb.tax.setValue(S.lastTaxAc);                               // PreviousSalesTaxAc
            $('txtTaxPercent').disabled = !S.taxEditable;
            detailButtons(false); setButtons();
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) edit(+rec);
        }).catch(fail);
    }
    document.addEventListener('DOMContentLoaded', load);
})();
