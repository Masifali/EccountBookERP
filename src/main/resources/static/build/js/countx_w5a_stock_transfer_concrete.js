/* ============================================================================================
 * Screen 596 "Stock Transfer Manual" (concrete) - Architecture.WinApp.pcc.Production.frmStockTransferManualConcrete (base.Name "frmStockTransferManualConcrete", DocumentTypeId 1860).
 * Desktop line numbers (.cs): _Load:304, ParameterFill:373, cmbperemeter_ValueChanged:386, DocumentNoFill:467, bindvarientunit:582, CmbAttributeVarient_Leave:625,
 * GetItemStock:647, FormValidation:714, FormValidationDetail:737, btnRefresh_Click:838, Reset:865, ResetDetail:907, btnUpdateDetail_Click:977, grd_DoubleClick:1018,
 * btnplus_Click:1060, grd_ColumnButtonClick:1079, grdSettings:1138, Insert:1200, getUpdate:1444, btnDelete_Click:1495, key handler:1538, MakeShortCutKeys:1644,
 * gridhistoryfill:1714, HistoryGridSettings:1776, grdhistory_ColumnButtonClick:1824, btnNewHistory_Click:1874, grdhistory_SelectionChanged:1888, btnprint_Click:2011,
 * cmbItemName_Leave:2052, AvgAmountCalculation:2075, bindPackUom:2134.
 * API /production/pcc/stock-transfer-manual-concrete/api. Not ported: Attachment dialog. Print = existing /reports/print/1860-stock-transfer-manual-slip.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/pcc/stock-transfer-manual-concrete/api';
    var $ = W3B.$;
    var look = { warehouses: [], jobLots: [], items: [] };
    var rights = {}, cfg = { fifo: false, financial: false, amountDp: 0, rateDp: 2, fyStart: '' };
    var recId = 0, isApproved = false, updateIdx = -1, rows = [], removed = [], hist = [], uoms = [], variants = [], seq = 0, timer = null;

    function applyRights() {                                   // _Load:310-316
        $('btnSave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights.update;
        $('btnPrint').disabled = !rights.print; $('chkPreview').disabled = !rights.print; $('chkPreview').checked = !!rights.print;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update/Delete'); if (!rights.print) m.push('Print');
        var n = $('rightsNote');
        n.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        n.classList.toggle('is-hidden', !m.length);
    }
    function mode() {                                          // Reset:865 / getUpdate:1444 button visibility
        var upd = recId > 0;
        W3B.show('btnSave', !upd); W3B.show('btnUpdate', upd); W3B.show('btnDelete', upd);
    }
    function entryMode(edit) { W3B.show('btnplus', !edit); W3B.show('btnUpdateDetail', edit); W3B.show('btnCancelUpdateDetial', edit); }

    // ----------------------------------------------------------------------------- combos
    function fillOpts(id, list, nameKey, zero, keep) {
        var el = $(id); if (!el) return;
        var h = zero ? '<option value="0">...Select Any Value...</option>' : '<option value=""></option>';
        (list || []).forEach(function (r) { h += '<option value="' + W3B.esc(r.Id) + '">' + W3B.esc(r[nameKey]) + '</option>'; });
        el.innerHTML = h;
        if (keep === undefined || keep === null) { el.selectedIndex = 0; return; }
        el.value = String(keep);
        if (el.value !== String(keep)) el.selectedIndex = 0;
    }
    function keepOf(id, list) { var v = W3B.int($(id).value); return v > 0 && list.some(function (r) { return W3B.int(r.Id) === v; }) ? v : null; }
    function bindLists() {                                     // WareHouseFill:423, GetJobLot:538, ItemDetailFill:497 (a selection that is gone is cleared)
        var kf = keepOf('CmbWareHouseFrom', look.warehouses), kt = keepOf('cmbWareHouseTo', look.warehouses);
        fillOpts('CmbWareHouseFrom', look.warehouses, 'WareHouseName', true, kf); fillOpts('cmbWareHouseTo', look.warehouses, 'WareHouseName', true, kt);
        var jf = keepOf('cmbJobLot', look.jobLots), jt = keepOf('CmbJobLotTo', look.jobLots);
        fillOpts('cmbJobLot', look.jobLots, 'JobLotDescription', true, jf); fillOpts('CmbJobLotTo', look.jobLots, 'JobLotDescription', true, jt);
        fillOpts('cmbItemName', look.items, 'ItemName', true, keepOf('cmbItemName', look.items));
    }
    function itemOf() { var id = W3B.int($('cmbItemName').value); return look.items.filter(function (i) { return W3B.int(i.Id) === id; })[0] || null; }
    function withVariant() { var it = itemOf(); return !!(it && it.WithVariant); }

    // ----------------------------------------------------------------------------- number helpers (stringFormatsingle / DecimalRateFormate)
    function rnd(v, dp) { var f = Math.pow(10, dp), s = v < 0 ? -1 : 1; return s * Math.round(Math.abs(v) * f + 1e-9) / f; }
    function fx(v, dp) { return rnd(v, dp).toFixed(dp); }
    function amt(v) { return fx(v, cfg.amountDp); }
    function rate(v) { return fx(v, cfg.rateDp); }

    // ----------------------------------------------------------------------------- AvgAmountCalculation:2075
    function calc() {
        var r = W3B.num($('txtAvgRate').value), q = W3B.num($('txtQty').value), iw = W3B.num($('txtitemweight').value), vu = W3B.num($('txtVarientEquivalent').value), nw;
        if (q > 0 && iw > 0 && W3B.int($('cmbItemName').value) > 0) {
            if (!withVariant()) {
                var u = uoms.filter(function (x) { return W3B.int(x.Id) === W3B.int($('CmbPackUom').value); })[0];
                vu = u ? W3B.num(u.Equivalent) : 0; nw = vu * q;
            } else nw = vu * q * iw;
            $('txtDetailNetWeight').value = amt(nw);
        } else $('txtDetailNetWeight').value = '0';
        $('txtAmount').value = (q > 0 && r > 0) ? amt(q * r) : '0';
    }
    function qtyChanged() { stockSoon(); calc(); }             // txtQty_TextChanged:2046

    // ----------------------------------------------------------------------------- GetItemStock:647
    function stockSoon() { clearTimeout(timer); timer = setTimeout(function () { refreshStock(false); }, 250); }
    function stockNow() { clearTimeout(timer); refreshStock(false); }            // CmbWareHouseFrom_TextChanged:2167
    function refreshStock(balanceOnly) {
        var item = W3B.int($('cmbItemName').value), wh = W3B.int($('CmbWareHouseFrom').value), me = ++seq;
        if (!(item > 0 || wh > 0)) { $('LabBalanceQty').textContent = '0'; if (!balanceOnly) $('txtAvgRate').value = '0'; calc(); return Promise.resolve(); }
        return W3B.getJson(api + '/stock' + W3B.qs({ itemId: item, warehouseId: wh, jobLotId: W3B.int($('cmbJobLot').value), variantId: W3B.int($('CmbAttributeVarient').value),
            withVariant: withVariant(), qty: W3B.num($('txtQty').value), recId: recId, docDate: $('txtdocdate').value })).then(function (d) {
            if (me !== seq) return;
            $('LabBalanceQty').textContent = String(d.balance);
            if (!balanceOnly) { $('txtAvgRate').value = rate(d.avgRate); calc(); }
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- item chain (cmbItemName_Leave:2052)
    function itemLeave() {
        var id = W3B.int($('cmbItemName').value);
        var p = id > 0 ? W3B.getJson(api + '/item-info?itemId=' + id) : Promise.resolve({ uoms: [], variants: [] });
        return p.then(function (d) {
            var prevUom = W3B.selText('CmbPackUom'), prevVar = W3B.selText('CmbAttributeVarient');
            uoms = d.uoms || []; variants = d.variants || [];
            var mu = uoms.filter(function (u) { return u.Name === prevUom; })[0], mv = variants.filter(function (v) { return v.Name === prevVar; })[0];
            fillOpts('CmbPackUom', uoms, 'Name', false, mu ? mu.Id : null);                  // bindPackUom:2134 (no "select" row)
            fillOpts('CmbAttributeVarient', variants, 'Name', true, mv ? mv.Id : 0);          // bindvarientunit:582
            if (!variants.length) $('txtVarientEquivalent').value = '';
            var it = itemOf();
            $('txtitemweight').value = id > 0 && it ? String(it.ItemWeight) : '0';
            if (id > 0) { variantLeave(); stockNow(); } else calc();
        }).catch(W3B.fail);
    }
    function variantLeave() {                                  // CmbAttributeVarient_Leave:625
        var id = W3B.int($('CmbAttributeVarient').value), v = variants.filter(function (x) { return W3B.int(x.Id) === id; })[0];
        $('txtVarientEquivalent').value = id > 0 && v ? String(v.Equivalent) : '0';
        calc();
    }

    // ----------------------------------------------------------------------------- detail entry
    function validateDetail() {                                // FormValidationDetail:737
        function need(id, msg) { if (!W3B.int($(id).value)) { W3B.box(msg); W3B.focus(id); return false; } return true; }
        if (!need('CmbWareHouseFrom', 'WarehouseFrom field is required')) return false;
        if (!need('cmbWareHouseTo', 'WarehouseTo field is required')) return false;
        if (!need('cmbJobLot', 'JobLot From field is required')) return false;
        if (!need('CmbJobLotTo', 'JobLotTo field is required')) return false;
        if (!need('cmbItemName', 'Item Name field is required')) return false;
        if (!need('CmbPackUom', 'Pack Uom field is required')) return false;
        var wv = withVariant();
        if (wv && !W3B.int($('CmbAttributeVarient').value)) { W3B.box('Attribute Variant field is required'); W3B.focus('CmbAttributeVarient'); return false; }
        if (!wv && W3B.int($('CmbAttributeVarient').value) > 0) W3B.clearCombo('CmbAttributeVarient');
        function nz(id, msg) { var t = $(id).value.trim(); if (t === '' || W3B.num(t) === 0) { W3B.box(msg); W3B.focus(id); return false; } return true; }
        if (!nz('txtQty', 'Qty field is required')) return false;
        if (!nz('txtitemweight', 'ItemWeight field is required')) return false;
        if (!nz('txtDetailNetWeight', 'NetWeight field is required')) return false;
        if (!nz('txtAvgRate', 'AvgRate field is required')) return false;
        if (!nz('txtAmount', 'Amount field is required')) return false;
        if (W3B.int($('CmbWareHouseFrom').value) === W3B.int($('cmbWareHouseTo').value) && W3B.int($('cmbJobLot').value) === W3B.int($('CmbJobLotTo').value)) {
            W3B.box('JoLotFrom Can not Equal to JobLotTo when Warehouse From and WarehouseTo is same'); W3B.focus('cmbJobLot'); return false;
        }
        return true;
    }
    function entryRow(base) {
        var wv = withVariant();
        base.WareHouseFromId = W3B.int($('CmbWareHouseFrom').value); base.WareHouseFrom = W3B.selText('CmbWareHouseFrom');
        base.WareHouseToId = W3B.int($('cmbWareHouseTo').value); base.WareHouseTo = W3B.selText('cmbWareHouseTo');
        base.JobLotId = W3B.int($('cmbJobLot').value); base.JobLot = W3B.selText('cmbJobLot');
        base.JobLotIdTo = W3B.int($('CmbJobLotTo').value); base.JobLotTo = W3B.selText('CmbJobLotTo');
        base.ItemId = W3B.int($('cmbItemName').value); base.ItemName = W3B.selText('cmbItemName');
        base.PackUomId = W3B.int($('CmbPackUom').value); base.PackUom = W3B.selText('CmbPackUom');
        base.AttributeVarientId = wv ? W3B.int($('CmbAttributeVarient').value) : 0; base.AttributeVarient = wv ? W3B.selText('CmbAttributeVarient') : '';
        base.VarientUnit = wv ? W3B.num($('txtVarientEquivalent').value) : 0;
        base.QTY = W3B.num($('txtQty').value); base.ItemWeight = W3B.num($('txtitemweight').value); base.NetWeight = W3B.num($('txtDetailNetWeight').value);
        base.ItemRate = W3B.num($('txtAvgRate').value); base.ItemAmount = W3B.num($('txtAmount').value); base.Remarks = $('txtRemarksDetail').value.trim();
        return base;
    }
    function addRow() { if (!validateDetail()) return; rows.push(entryRow({ Id: 0 })); resetDetail(); render(); }              // btnplus_Click:1060
    function updateRow() {                                     // btnUpdateDetail_Click:977
        if (!validateDetail()) return;
        var r = rows[updateIdx]; if (!r) return;
        entryRow(r); entryMode(false); resetDetail(); render();
    }
    function cancelRow() { entryMode(false); updateIdx = -1; render(); }                                                          // btnCancelUpdateDetial_Click:963
    function resetDetail() {                                   // ResetDetail:907
        updateIdx = -1; seq++;
        ['CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbJobLot', 'CmbJobLotTo', 'cmbItemName', 'CmbPackUom', 'CmbAttributeVarient'].forEach(function (i) { W3B.clearCombo(i); });
        ['txtVarientEquivalent', 'txtDetailNetWeight', 'txtQty', 'txtRemarksDetail', 'txtAvgRate', 'txtitemweight', 'txtAmount'].forEach(function (i) { $(i).value = ''; });
        entryMode(false); W3B.focus('CmbWareHouseFrom');
    }
    function editRow(i) {                                      // grd_DoubleClick:1018
        var r = rows[i]; if (!r) return;
        updateIdx = i;
        W3B.setCombo('CmbWareHouseFrom', r.WareHouseFromId); W3B.setCombo('cmbWareHouseTo', r.WareHouseToId);
        W3B.setCombo('cmbJobLot', r.JobLotId); W3B.setCombo('CmbJobLotTo', r.JobLotIdTo); W3B.setCombo('cmbItemName', r.ItemId);
        W3B.getJson(api + '/item-info?itemId=' + r.ItemId).then(function (d) {
            uoms = d.uoms || []; variants = d.variants || [];
            fillOpts('CmbPackUom', uoms, 'Name', false, r.PackUomId > 0 ? r.PackUomId : null);
            fillOpts('CmbAttributeVarient', variants, 'Name', true, r.AttributeVarientId > 0 ? r.AttributeVarientId : 0);
            $('txtVarientEquivalent').value = String(r.VarientUnit); $('txtQty').value = String(r.QTY); $('txtitemweight').value = String(r.ItemWeight);
            $('txtDetailNetWeight').value = String(r.NetWeight); $('txtAvgRate').value = String(r.ItemRate); $('txtAmount').value = String(r.ItemAmount);
            $('txtRemarksDetail').value = r.Remarks;
            entryMode(true); render(); W3B.focus('CmbWareHouseFrom');
            refreshStock(true);
        }).catch(W3B.fail);
    }
    function delRow(i) {                                       // grd_ColumnButtonClick:1079 (Delete)
        var r = rows[i]; if (!r) return;
        if (r.Id > 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            removed.push({ Id: r.Id, WareHouseFromId: r.WareHouseFromId, WareHouseToId: r.WareHouseToId, JobLotId: r.JobLotId, JobLotIdTo: r.JobLotIdTo, ItemId: r.ItemId,
                PackUomId: r.PackUomId, AttributeVarientId: r.AttributeVarientId, VarientUnit: r.VarientUnit, QTY: r.QTY, ItemWeight: r.ItemWeight, NetWeight: r.NetWeight,
                ItemRate: r.ItemRate, ItemAmount: r.ItemAmount, Remarks: r.Remarks });
        }
        rows.splice(i, 1); if (updateIdx === i) { updateIdx = -1; entryMode(false); }
        render(); calc();
    }
    function sum(k) { return rows.reduce(function (a, r) { return a + W3B.num(r[k]); }, 0); }
    function render() {                                        // grdSettings:1138 (totals: VarientUnit, QTY, NetWeight, ItemWeight, ItemAmount)
        var html = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 25, f: function (r, i) { return '<button type="button" class="rc-x" data-del="' + i + '">X</button>'; } },
            { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-edit="' + i + '">Edit</button>'; } },
            { h: 'WareHouseFrom', k: 'WareHouseFrom' }, { h: 'WareHouseTo', k: 'WareHouseTo' }, { h: 'JobLot', k: 'JobLot' }, { h: 'JobLotTo', k: 'JobLotTo' },
            { h: 'ItemName', k: 'ItemName' }, { h: 'PackUom', k: 'PackUom' }, { h: 'AttributeVarient', k: 'AttributeVarient' },
            { h: 'VarientUnit', cls: 'num', f: function (r) { return W3B.fmt(r.VarientUnit, 3); } }, { h: 'QTY', cls: 'num', f: function (r) { return W3B.fmt(r.QTY, 3); } },
            { h: 'ItemWeight', cls: 'num', f: function (r) { return W3B.fmt(r.ItemWeight, 3); } }, { h: 'NetWeight', cls: 'num', f: function (r) { return W3B.fmt(r.NetWeight, 3); } },
            { h: 'ItemRate', cls: 'num', f: function (r) { return W3B.fmt(r.ItemRate, cfg.rateDp); } }, { h: 'ItemAmount', cls: 'num', f: function (r) { return W3B.fmt(r.ItemAmount, cfg.amountDp); } },
            { h: 'Remarks', k: 'Remarks' }
        ], rows, { empty: 'No records', rowClass: function (r, i) { return i === updateIdx ? 'is-current' : ''; } });
        if (rows.length) html += '<tfoot><tr style="font-weight:bold"><td colspan="9"></td><td class="num">' + W3B.fmt(sum('VarientUnit'), 3) + '</td><td class="num">' + W3B.fmt(sum('QTY'), 3) +
            '</td><td class="num">' + W3B.fmt(sum('ItemWeight'), 3) + '</td><td class="num">' + W3B.fmt(sum('NetWeight'), 3) + '</td><td></td><td class="num">' +
            W3B.fmt(sum('ItemAmount'), cfg.amountDp) + '</td><td></td></tr></tfoot>';
        $('grd').innerHTML = html;
    }
    document.addEventListener('click', function (e) {
        var t = e.target.closest ? e.target : null; if (!t) return;
        var d = t.closest('button[data-del]'); if (d) { delRow(+d.dataset.del); return; }
        var ed = t.closest('button[data-edit]'); if (ed) { editRow(+ed.dataset.edit); return; }
        var he = t.closest('button[data-hedit]'); if (he) { histEdit(hist[+he.dataset.hedit].Id); return; }
        var hp = t.closest('button[data-hprint]'); if (hp) { printId(hist[+hp.dataset.hprint].Id); return; }
        var tr = t.closest('#grdhistory tbody tr[data-i]');                                      // grdhistory_SelectionChanged:1888
        if (tr) historyDetail(hist[+tr.dataset.i].Id);
    });
    document.addEventListener('dblclick', function (e) {
        var tr = e.target.closest ? e.target.closest('#grd tbody tr[data-i]') : null;
        if (tr) { editRow(+tr.dataset.i); return; }
        var hr = e.target.closest ? e.target.closest('#grdhistory tbody tr[data-i]') : null;       // grdhistory_DoubleClick_1:1689
        if (hr) load(hist[+hr.dataset.i].Id);
    });

    // ----------------------------------------------------------------------------- toolbar
    function resetForm() {                                     // Reset:865 (DocDate is kept)
        recId = 0; isApproved = false; removed = []; updateIdx = -1; seq++;
        ['txtgatepsrno', 'txtvehichleno', 'txtRemarksHead', 'txtQty', 'txtAvgRate', 'txtAmount', 'txtRemarksDetail'].forEach(function (i) { $(i).value = ''; });
        ['CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbJobLot', 'CmbJobLotTo', 'cmbItemName', 'CmbAttributeVarient'].forEach(function (i) { W3B.clearCombo(i); });
        entryMode(false); rows = []; render(); mode(); $('txtgpdate').value = $('txtdocdate').value;
        return W3B.getJson(api + '/doc-no').then(function (d) { $('txtdocno').value = d.docNo; });
    }
    function newClick() { resetForm().then(resetDetail).catch(W3B.fail); }            // btnnew_Click:854
    function refresh() {                                       // btnRefresh_Click:838
        W3B.getJson(api + '/refresh').then(function (d) {
            look.warehouses = d.warehouses; look.jobLots = d.jobLots; look.items = d.items; bindLists(); $('txtdocno').value = d.docNo;
            var id = W3B.int($('cmbItemName').value);
            if (id > 0) itemLeave();
        }).catch(W3B.fail);
    }
    function validateHeader() {                                // FormValidation:714
        var dn = $('txtdocno').value.trim();
        if (dn === '' || dn === '0') { W3B.box('DocNo Field is Required'); return false; }
        if ($('txtgatepsrno').value === '') { W3B.box('GatePassNo Field is Required'); W3B.focus('txtgatepsrno'); return false; }
        if ($('txtvehichleno').value === '') { W3B.box('Vehicle No Field is Required'); W3B.focus('txtvehichleno'); return false; }
        return true;
    }
    function insert() {                                        // Insert:1200
        if (!validateHeader()) return;
        if (!rows.length) { W3B.box('Grid Record Not Found'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var b = { id: recId, docDate: $('txtdocdate').value, gpSrNo: $('txtgatepsrno').value, vehicleNo: $('txtvehichleno').value,
            remarksHeader: $('txtRemarksHead').value, rows: rows, removed: removed };
        W3B.postJson(api + '/save', b).then(function (d) {
            W3B.box(d.message);
            var preview = $('chkPreview').checked && rights.print;
            return resetForm().then(function () { if (preview) printId(d.id); });
        }).catch(W3B.fail);
    }
    function save() { recId = 0; insert(); }                   // btnsave_Click:1412
    function update() { if (isApproved) W3B.box('Approved Record Not Update'); else insert(); }            // btnupdate_Click:1425
    function del() {                                           // btnDelete_Click:1495
        if (recId <= 0) { W3B.box('RecId not Found'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { W3B.box(d.message); return resetForm(); }).catch(W3B.fail);
    }
    function printId(id) {                                     // btnprint_Click:2011 -> StockTransferRegister + 1860-StockTransferManual-Slip.rpt
        if (!(id > 0)) { W3B.box('Record Not Found For Display'); return; }
        window.open('/reports/print/1860-stock-transfer-manual-slip?success=' + id, '_blank');
    }
    function print() { printId(recId); }
    function keys() {                                          // MakeShortCutKeys:1644
        W3B.box(['Ctrl+S : For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U : For Update', 'Ctrl+Shift+Delete : For Delete',
            'Ctrl+E : For Close', 'Ctrl+R : For Refresh', 'Ctrl+N : For New', 'Ctrl+P : For Print', 'Ctrl+T : For Tab Transfer'].join('\n'));
    }

    function load(id) {                                        // getUpdate:1444
        return W3B.getJson(api + '/read/' + id).then(function (d) {
            recId = d.id; isApproved = !!d.isApproved; removed = []; updateIdx = -1;
            $('txtdocdate').value = d.docDate; $('txtdocno').value = d.docNo; $('txtgpdate').value = d.gpDate; $('txtgatepsrno').value = d.gpSrNo;
            $('txtvehichleno').value = d.vehicleNo; $('txtRemarksHead').value = d.remarksHeader;
            rows = d.details || []; entryMode(false); render(); mode(); tab('form');
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
    }
    function paramChanged() {                                  // cmbperemeter_ValueChanged:386 (case 4 is coded as To = 1 Jan, From = today)
        var v = W3B.int($('cmbperemeter').value), t = new Date(), iso = W3B.iso;
        if (v === 1) $('fromdate').value = iso(t);
        else if (v === 2) { var w = new Date(t); w.setDate(w.getDate() - 7); $('fromdate').value = iso(w); }
        else if (v === 3) { $('fromdate').value = iso(new Date(t.getFullYear(), t.getMonth(), 1)); $('ToDate').value = iso(t); }
        else if (v === 4) { $('ToDate').value = iso(new Date(t.getFullYear(), 0, 1)); $('fromdate').value = iso(t); }
        else if (v === 5 && cfg.fyStart) $('fromdate').value = String(cfg.fyStart).substring(0, 10);
    }
    function history() {                                       // gridhistoryfill:1714
        $('GridDetailHistory').innerHTML = '';
        W3B.getJson(api + '/history' + W3B.qs({ fromDate: $('fromdate').value, toDate: $('ToDate').value, fromDocNo: W3B.int($('txtfromdocno').value),
            toDocNo: W3B.int($('txttodocno').value), vehicleNo: $('txtVehicleNoHistory').value })).then(function (r) {
            hist = r;
            $('grdhistory').innerHTML = W3B.tableHtml([
                { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (x, i) { return '<button type="button" class="rc-cellbtn" data-hedit="' + i + '">Edit</button>'; } },
                { h: 'Print', html: true, cls: 'w3-btn', w: 40, f: function (x, i) { return '<button type="button" class="rc-cellbtn" data-hprint="' + i + '">Print</button>'; } },
                { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (x) { return W3B.fmtDMY(x.DocDate); } }, { h: 'GPSrNo', k: 'GPSrNo' },
                { h: 'GpDate', f: function (x) { return W3B.fmtDMY(x.GpDate); } }, { h: 'VehicleNo', k: 'VehicleNo' },
                { h: 'EntryDate', f: function (x) { return W3B.fmtDMY(x.EntryDate); } }, { h: 'EntryUser', k: 'EntryUserName' },
                { h: 'ModifyDate', f: function (x) { return W3B.fmtDMY(x.ModifyDate); } }, { h: 'ModifyUser', k: 'ModifyUserName' },
                { h: 'RemarksHeader', k: 'RemarksHeader' }, { h: 'NoOfAttachments', k: 'NoOfAttachments', cls: 'num' }
            ], r, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function historyDetail(id) {                               // grdhistory_SelectionChanged:1888
        W3B.getJson(api + '/read/' + id).then(function (d) {
            var r = d.details || [];
            var html = W3B.tableHtml([
                { h: 'FromWarehouse', k: 'WareHouseFrom' }, { h: 'ToWarehouse', k: 'WareHouseTo' }, { h: 'JobLotFrom', k: 'JobLot' }, { h: 'JobLotTo', k: 'JobLotTo' },
                { h: 'ItemName', k: 'ItemName' }, { h: 'AttributeVarient', k: 'AttributeVarient' }, { h: 'VarientUnit', cls: 'num', f: function (x) { return W3B.fmt(x.VarientUnit, 3); } },
                { h: 'Qty', cls: 'num', f: function (x) { return W3B.fmt(x.QTY, 2); } }, { h: 'ItemWeight', cls: 'num', f: function (x) { return W3B.fmt(x.ItemWeight, 2); } },
                { h: 'NetWeight', cls: 'num', f: function (x) { return W3B.fmt(x.NetWeight, 2); } }, { h: 'ItemRate', cls: 'num', f: function (x) { return W3B.fmt(x.ItemRate, cfg.rateDp); } },
                { h: 'ItemAmount', cls: 'num', f: function (x) { return W3B.fmt(x.ItemAmount, cfg.amountDp); } }, { h: 'RemarksDetail', k: 'Remarks' }
            ], r, { empty: 'No records' });
            $('GridDetailHistory').innerHTML = html;
        }).catch(W3B.fail);
    }
    function histEdit(id) { resetForm().then(function () { return load(id); }).catch(W3B.fail); }       // grdhistory_ColumnButtonClick:1824 (Reset, then getUpdate)
    function newHistory() { $('txtfromdocno').value = ''; $('txttodocno').value = ''; $('txtvehichleno').value = ''; }       // btnNewHistory_Click:1874 (clears the form's Vehicle No box, as coded)

    window.Stc = { newClick: newClick, refresh: refresh, save: save, update: update, del: del, print: print, keys: keys, tab: tab, calc: calc, qtyChanged: qtyChanged,
        stockNow: stockNow, itemLeave: itemLeave, variantLeave: variantLeave, addRow: addRow, updateRow: updateRow, cancelRow: cancelRow, history: history,
        newHistory: newHistory, paramChanged: paramChanged };

    function on(id) { var b = $(id); return !b.classList.contains('is-hidden') && !b.disabled; }
    W3B.keys({
        n: function () { if ($('panelForm').classList.contains('is-active')) newClick(); },
        s: function () { if ($('panelForm').classList.contains('is-active')) { if (on('btnSave')) save(); } else history(); },
        u: function () { if (on('btnUpdate')) update(); },
        p: function () { if (on('btnPrint')) print(); },
        r: refresh,
        'shift+delete': function () { if (on('btnDelete') && rights['delete']) del(); },           // key handler:1561 also needs the Delete right
        t: function () { tab($('panelForm').classList.contains('is-active') ? 'hist' : 'form'); },
        e: function () { window.location.href = '/production'; }, escape: function () { window.location.href = '/production'; }
    });

    $('txtdocdate').addEventListener('change', function () { if (recId === 0) $('txtgpdate').value = $('txtdocdate').value; });

    W3B.getJson(api + '/initial').then(function (d) {
        rights = d.rights || {}; applyRights();
        cfg.fifo = !!d.fifo; cfg.financial = !!d.financialEffect; cfg.amountDp = d.amountDp; cfg.rateDp = d.rateDp; cfg.fyStart = d.fyStart || '';
        look.warehouses = d.warehouses; look.jobLots = d.jobLots; look.items = d.items; bindLists();
        var pc = $('cmbperemeter'); pc.innerHTML = (d.dateTypes || []).map(function (r) { return '<option value="' + W3B.esc(r.Id) + '">' + W3B.esc(r.Parameters) + '</option>'; }).join('');
        if (pc.options.length > 1) pc.selectedIndex = 1;                                                      // _Load: cmbperemeter.Rows[1].Activate()
        var t = W3B.today(); ['txtdocdate', 'txtgpdate', 'fromdate', 'ToDate'].forEach(function (i) { $(i).value = t; }); paramChanged();
        $('txtdocno').value = d.docNo; $('txtAvgRate').value = '0'; render(); mode(); entryMode(false);
    }).catch(W3B.fail);
}());
