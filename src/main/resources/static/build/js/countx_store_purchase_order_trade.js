/* ============================================================================================
 * Screen 328 "Purchase Order Store" - Architecture.WinApp.PurchaseTrading.PurchsaeOrderTrade,
 * ScreenName "PurchsaeOrderTrade", DocumentTypeId 42 (the non-taxable run of the form).
 * Server side and the numbered behaviour notes / deviations: StorePurchaseOrderTradeService.java.
 *
 *   PurchsaeOrder_Load:401      combos, doc no, branch sr no, history dates and branch combo
 *   combitem_Leave_1:960        pack / rate UOM lists of the item, the configured Job/Lot
 *   btnplus_Click:1093          FormValidationDetail:1052, duplicate item, add the row
 *   grd_DoubleClick:1262        edit a row in the entry bar; btnUpdateDetail_Click:1304
 *   grd_CellUpdated:1377        Item Qty / Rate edited in the grid (balance rule, weight, amount)
 *   Insert():1499               FormValidation:1023, confirm, save, Reset(), 201 slip when Preview is ticked
 *   btnLoader_Click:3366        Pending Purchase Demand loader (frmPendingPurchaseDemand, IsForPreBill)
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf, ci = C.ci;
    var api = '/api/store/purchase-order-store';

    var look = { rights: {}, suppliers: [], dueTerms: [], items: [], uoms: [], jobLots: [], deliveryTerms: [], statuses: [], historyBranches: [] };
    var RECID = 0;
    var table = [];                     // the detail DataTable
    var updateDetailIndex = -1;
    var removedIds = [];                // OrderDetailRemoveIds
    var loaded = false;
    var lastBalance = 0;                // txtqty.AccessibleName

    var COLS = [['RefDocNo', 'RefDocNo'], ['ItemCode', 'ItemCode'], ['Item', 'Item'], ['JobLot', 'Job/Lot'], ['PackUom', 'Pack Uom'],
        ['ItemQty', 'Item Qty', 1, 1], ['TotalQty', 'TotalQty', 1], ['Rate', 'Rate', 1, 1], ['RateUom', 'RateUOM'],
        ['Amount', 'Item Amount', 1], ['Remarks', 'Remarks', 0, 1]];

    /* ------------------------------------------------------------------ small helpers */

    function selText(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] && e.value !== '0' ? e.selectedOptions[0].textContent.trim() : ''; }
    function byName() { return $id('rdbtnItemName').checked; }
    function amountDigits() { return Math.max(0, Math.min(15, intOf(look.amountDecimals))); }
    function roundAway(v, p) { var s = v < 0 ? -1 : 1, f = Math.pow(10, p); return s * Math.round(Math.abs(v) * f + 1e-9) / f; }
    function roundEven(v) {                         // Math.Round(double) - banker's rounding
        var f = Math.floor(v), d = v - f;
        if (Math.abs(d - 0.5) < 1e-9) return f % 2 === 0 ? f : f + 1;
        return Math.round(v);
    }
    function fmtAmt(v) {                            // clsGlobalVariables.stringFormatsingle
        var n = Math.max(0, Math.min(4, intOf(look.amountFormatDecimals)));
        return num(v).toLocaleString('en-US', { minimumFractionDigits: n, maximumFractionDigits: n });
    }
    function fmt3(v) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }   // "#,##0.###"
    function fillDef(id, rows, text, def) {        // DropDownBind.BindDDL / BindDDLNew with ZeroIndex
        C.fillSelect(id, rows, 'Id', text);
        var el = $id(id);
        if (el.options.length && el.options[0].value === '0') el.options[0].textContent = def || '...Select Any Value...';
    }
    function decimalKey(e, box) {                  // OnlytextdecimelFunction
        if (e.key.length !== 1) return true;
        if (e.key === '.') return box.value.indexOf('.') < 0;
        return /\d/.test(e.key);
    }
    function digitKey(e) { return e.key.length !== 1 || /\d/.test(e.key); }       // OnlytextNumberFunction
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }

    /* ------------------------------------------------------------------ combos */

    function itemBind() {                                                       // ItemDetailFill:811 / rdbtnItemName_CheckedChanged:982
        var keep = $id('cmbItem').value, nm = byName();
        C.fillSelectCols('cmbItem', look.items, 'Id', nm ? 'ItemName' : 'ItemCode', [nm ? 'ItemCode' : 'ItemName'],
            nm ? ['Item Name', 'ItemCode'] : ['Item Code', 'ItemName']);
        var el = $id('cmbItem');
        if (el.options.length && el.options[0].value === '0') el.options[0].textContent = '...Select Any Value...';
        if (keep && Array.prototype.some.call(el.options, function (o) { return o.value === keep; })) el.value = keep;
    }
    function uomBind(id, label, itemId) {                                       // bindRateUomAndItemPackUom:855 - keeps the choice by its text
        var el = $id(id), keep = selText(id);
        var list = look.uoms.filter(function (u) { return intOf(u.ItemId) === itemId; });
        C.fillSelectCols(el, list, 'Id', 'UOMCode', ['Equivalent'], [label, 'Equivalent']);
        if (el.options.length && el.options[0].value === '0') el.options[0].textContent = '-- Select --';
        var hit = list.find(function (u) { return keep && u.UOMCode === keep; });
        el.value = hit ? String(hit.Id) : '0';
    }
    function eqOf(id) {                                                         // SelectedRow.Cells["Equivalent"]
        var o = $id(id).selectedOptions && $id(id).selectedOptions[0];
        if (!o || o.value === '0') return 0;
        var ex = (o.getAttribute('data-extra') || '').split('|');
        return num(ex[0]);
    }

    function itemLeave() {                                                      // combitem_Leave_1:960
        uomBind('combitempck', 'Pack Uom', intOf($id('cmbItem').value));
        uomBind('combrateuom', 'Rate Uom', intOf($id('cmbItem').value));
        if (intOf(look.defaultJobLotId) > 0) $id('combjob').value = String(look.defaultJobLotId);   // configrationsAllocation "Job/Lot"
    }
    function searchModeChanged() { itemBind(); }

    /* ------------------------------------------------------------------ entry bar calculation */

    function calcWeight() {                                                     // CalculateWeight:2553
        var pack = intOf($id('combitempck').value) > 0 ? eqOf('combitempck') : 0;
        $id('txtweight').value = String(pack * num($id('txtqty').value));
        calcAmount();
    }
    function calcAmount() {                                                     // TotalAmount:2642
        var w = num($id('txtweight').value), rate = num($id('txtRate').value), ru = $id('combrateuom').value !== '0' ? eqOf('combrateuom') : 0;
        var amt = ru > 0 ? w / ru * rate : 0;
        amt = roundAway(amt, amountDigits());
        $id('txtItemAmount').value = fmtAmt(amt);
    }

    /* ------------------------------------------------------------------ detail grid */

    function cell(r, c) {
        var v = r[c[0]];
        if (c[0] === 'ItemQty' || c[0] === 'TotalQty') return fmt3(v);
        if (c[0] === 'Rate') return num(v) ? String(v) : '';
        return v === null || v === undefined ? '' : v;
    }
    function render() {                                                         // grdSettings:1202
        var t = $id('grd'), upd = !!(look.rights || {}).update;
        t.tHead.innerHTML = '<tr><th>Edit</th><th>&nbsp;</th>' + COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="PoTrade.edit(' + i + ')"' + (i === updateDetailIndex ? ' class="is-selected"' : '') + '>' +
                '<td><button type="button" class="cx-link" onclick="PoTrade.edit(' + i + ')">Edit</button></td>' +
                '<td><button type="button" class="cx-x" onclick="PoTrade.del(' + i + ')">X</button></td>' +
                COLS.map(function (c) {
                    if (c[3]) {                                                 // QTY / Rate / Remarks are the editable columns
                        return '<td' + (c[2] ? ' class="num"' : '') + '><input type="text" class="' + (c[2] ? 'num' : '') + '" value="' + esc(c[0] === 'Remarks' ? (r.Remarks || '') : cell(r, c)) +
                            '" onchange="PoTrade.cellEdit(' + i + ',\'' + c[0] + '\',this)" ondblclick="event.stopPropagation()"></td>';
                    }
                    return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[0] === 'Amount' ? fmtAmt(r.Amount) : cell(r, c)) + '</td>';
                }).join('') + '</tr>';
        }).join('');
    }

    function validateDetail() {                                                 // FormValidationDetail:1052
        if (intOf($id('cmbItem').value) === 0) { alert('Item Name Field is Required'); $id('cmbItem').focus(); return false; }
        if (intOf($id('combitempck').value) === 0) { alert('Item Pack Unit Field is Required'); $id('combitempck').focus(); return false; }
        if (num($id('txtqty').value) === 0) { alert('Item Qty Field is Required'); $id('txtqty').focus(); return false; }
        if (num($id('txtRate').value) === 0) { alert('Item Rate Field is Required'); $id('txtRate').focus(); return false; }
        if (num($id('txtItemAmount').value) === 0) { alert('ItemAmount Field is Required'); $id('txtItemAmount').focus(); return false; }
        if (intOf($id('combrateuom').value) === 0) { alert('Rate UOM Field is Required'); $id('combrateuom').focus(); return false; }
        return true;
    }
    function rowFromBar() {
        var item = look.items.find(function (x) { return x.Id === intOf($id('cmbItem').value); }) || {};
        return {
            ItemId: intOf($id('cmbItem').value), ItemCode: item.ItemCode || '', Item: item.ItemName || '',
            JobLotId: intOf($id('combjob').value), JobLot: selText('combjob'),
            PackUomId: intOf($id('combitempck').value), PackUom: selText('combitempck'), PackEquivalent: eqOf('combitempck'),
            ItemQty: num($id('txtqty').value), TotalQty: num($id('txtweight').value), Rate: num($id('txtRate').value),
            RateUomId: intOf($id('combrateuom').value), RateUom: selText('combrateuom'), RateEquivalent: eqOf('combrateuom'),
            Amount: num($id('txtItemAmount').value), Remarks: $id('txtremarksdetail').value.trim()
        };
    }
    function add() {                                                            // btnplus_Click:1093
        if (!validateDetail()) return;
        var sel = intOf($id('cmbItem').value);
        if (table.some(function (r) { return r.ItemId === sel; })) { alert('Duplicate Item Not Add in Grid'); return; }
        var r = rowFromBar();
        r.Id = 0; r.RefDocumentTypeId = 0; r.RefDocId = 0; r.RefDocInvoiceId = 0; r.RefDocNo = 0; r.BalQty = 0;
        table.push(r);
        resetDetail();
    }
    function edit(i) {                                                          // grd_DoubleClick:1262
        var r = table[i];
        if (!r) return;
        updateDetailIndex = i;
        lastBalance = 0;
        if (intOf(r.RefDocId) > 0 && intOf(r.RefDocInvoiceId) > 0) {
            $id('cmbItem').disabled = true;
            lastBalance = num(r.BalQty);
        }
        $id('cmbItem').value = String(r.ItemId);
        itemLeave();
        $id('combjob').value = String(r.JobLotId);
        $id('combitempck').value = String(r.PackUomId);
        $id('txtqty').value = fmt3(r.ItemQty);
        $id('txtweight').value = fmt3(r.TotalQty);
        $id('combrateuom').value = String(r.RateUomId);
        $id('txtRate').value = fmt3(r.Rate);
        $id('txtItemAmount').value = fmt3(r.Amount);
        $id('txtremarksdetail').value = r.Remarks || '';
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        render();
    }
    function updateDetail() {                                                   // btnUpdateDetail_Click:1304
        if (!validateDetail()) return;
        var sel = intOf($id('cmbItem').value);
        for (var k = 0; k < table.length; k++) {
            if (k !== updateDetailIndex && table[k].ItemId === sel) { alert('Duplicate Item Not Add in Grid'); return; }
        }
        if (lastBalance > 0) {
            var qty = num($id('txtqty').value);
            if (qty > lastBalance) { alert('Entered Qty:' + qty + ' is greater than balance Qty:' + lastBalance + '.Please Check'); return; }
        }
        var n = rowFromBar(), t = table[updateDetailIndex];
        ['ItemId', 'ItemCode', 'Item', 'JobLotId', 'JobLot', 'PackUomId', 'PackUom', 'PackEquivalent', 'ItemQty', 'TotalQty',
            'RateUomId', 'RateUom', 'RateEquivalent', 'Rate', 'Amount', 'Remarks'].forEach(function (key) { t[key] = n[key]; });
        resetDetail();
    }
    function del(i) {                                                           // grd_ColumnButtonClick:1157 "Delete"
        var r = table[i];
        if (!r) return;
        if (updateDetailIndex === i) { alert("You can't delete this detail because its in Update Mode"); return; }
        if (!confirm('Are you sure to Delete?')) return;
        if (intOf(r.Id) > 0) {
            var countIds = table.filter(function (x) { return intOf(x.Id) > 0; }).length;
            if (countIds <= 1) { alert('All rows Can not be Deleted in Update Mode'); return; }
            removedIds.push(intOf(r.Id));
        }
        table.splice(i, 1);
        if (updateDetailIndex > i) updateDetailIndex--;
        render();
    }
    function resetDetail() {                                                    // ResetDetail:1134
        updateDetailIndex = -1;
        $id('cmbItem').disabled = false;
        ['cmbItem', 'combitempck', 'combrateuom'].forEach(function (id) { $id(id).value = '0'; });
        ['txtqty', 'txtweight', 'txtRate', 'txtItemAmount', 'txtremarksdetail'].forEach(function (id) { $id(id).value = ''; });
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        lastBalance = 0;
        render();
        $id('cmbItem').focus();
    }

    /* grd_CellUpdated:1377 - the three editable columns. The balance rule below is the desktop's own, including that a
       row with no Purchase Demand has BalQty 0, so its Item Qty cell can only be set to the "max allowed" (<= 0). */
    function cellEdit(i, key, box) {
        var r = table[i];
        if (!r) return;
        if (key === 'Remarks') { r.Remarks = box.value; return; }
        if (key === 'ItemQty') {
            var bal = num(r.BalQty), entered = num(box.value), detailId = intOf(r.RefDocInvoiceId);
            var other = table.reduce(function (s, x, k) { return k !== i && intOf(x.RefDocInvoiceId) === detailId ? s + num(x.ItemQty) : s; }, 0);
            var maxAllowed = bal - other;
            if (entered <= 0) {
                entered = maxAllowed;
                alert('Qty must be greater than 0. Reset to max allowed: ' + maxAllowed);
            } else if (entered > maxAllowed) {
                entered = maxAllowed;
                alert('Qty exceeds allowed balance for row# ' + (i + 1) + '.\nmax allowed is ' + maxAllowed + '.');
            }
            r.ItemQty = entered;
            r.TotalQty = (entered > 0 && num(r.PackEquivalent) > 0) ? num(r.PackEquivalent) * entered : 0;      // CalculateWeight:1436
        } else if (key === 'Rate') {
            r.Rate = num(box.value);
        }
        var a = 0;                                                                // CalculateAmount:1446
        if (num(r.ItemQty) > 0 && num(r.RateEquivalent) > 0 && num(r.Rate) > 0 && num(r.TotalQty) > 0) a = num(r.TotalQty) / num(r.RateEquivalent) * num(r.Rate);
        r.Amount = roundAway(a, amountDigits());
        render();
    }

    /* ------------------------------------------------------------------ header behaviour */

    function paymentChanged() {                                                 // combpttrm_TextChanged:1812
        var cash = selText('combpttrm') === 'Cash';
        $id('txtduedays').disabled = cash; $id('duedate').disabled = cash;
        if (cash) { $id('txtduedays').value = ''; $id('duedate').value = $id('DocDate').value; }
    }
    function dueDaysChanged() {                                                 // txtduedays_TextChanged:2920 (from now, not the doc date)
        var v = $id('txtduedays').value.trim(), d = new Date();
        if (v !== '') d.setDate(d.getDate() + num(v));
        $id('duedate').value = iso(d);
    }
    function statusSet(text) {
        var s = look.statuses.find(function (x) { return x.Name === text; });
        $id('CmbStatus').value = s ? String(s.Id) : '0';
    }
    function setStatusEnabled() { $id('CmbStatus').disabled = !$id('btnUpdate') || $id('btnUpdate').classList.contains('is-hidden'); }

    /* ------------------------------------------------------------------ save / open / print */

    function save() { RECID = 0; submit(); }             // btnsave_Click:1690
    function update() { submit(); }                                                           // btnupdate_Click:1711
    function saveAs() { RECID = 0; submit(); }                                                // btnSaveAs_Click:1791
    function submit() {
        var st = selText('CmbStatus');
        if (RECID === 0 && (st === 'Complete' || st === 'Cancel')) {
            alert('Order Status Should not Be Complete Or Cancel In Save Mode'); $id('CmbStatus').focus(); return;
        }
        if (intOf($id('combsuppname').value) === 0) { alert('Supplier Name Field is Required'); $id('combsuppname').focus(); return; }     // FormValidation:1023
        if (intOf($id('combdeliverytrm').value) === 0) { alert('Delivery Term Field is Required'); $id('combdeliverytrm').focus(); return; }
        if (intOf($id('combpttrm').value) === 0) { alert('Payment Term Field is Required'); $id('combpttrm').focus(); return; }
        if (selText('combpttrm') === 'Credit' && intOf($id('txtduedays').value.trim()) <= 0) { alert('Due Days Field is Required'); $id('txtduedays').focus(); return; }
        if (!confirm(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        if (!table.length) { alert('Grid Record Not Found'); return; }
        var preview = $id('ChkBoxPrint').checked;
        C.postJson(api + '/save', {
            Id: RECID, DocDate: $id('DocDate').value, OrderSupCustId: intOf($id('combsuppname').value),
            SupplierRefNo: $id('txtsupprefno').value, RemarksHeader: $id('txtremarks').value,
            PaymentTermsId: intOf($id('combpttrm').value), OrderDueDays: $id('txtduedays').value.trim(), OrderDueDate: $id('duedate').value,
            DeliveryTermId: intOf($id('combdeliverytrm').value), DeliveryStartDate: $id('deliverystartdate').value,
            DeliveryDays: $id('txtdeliverydays').value.trim(), StatusId: intOf($id('CmbStatus').value),
            removedDetailIds: removedIds,
            lines: table.map(function (r) {
                return { Id: RECID > 0 ? intOf(r.Id) : 0, RefDocumentTypeId: intOf(r.RefDocumentTypeId), RefDocId: intOf(r.RefDocId),
                    RefDocInvoiceId: intOf(r.RefDocInvoiceId), ItemId: intOf(r.ItemId), JobLotId: intOf(r.JobLotId), PackUomId: intOf(r.PackUomId),
                    ItemQty: num(r.ItemQty), Rate: num(r.Rate), RateUomId: intOf(r.RateUomId), Remarks: r.Remarks || '' };
            })
        }).then(function (res) {
            alert(res.message);
            var id = res.id;
            reset().then(function () { if (preview) printId(id); });
        }).catch(function (e) { alert(e.message); });
    }
    function printId(id) {                                                      // GenerateReport:2831 -> PurchaseOrderSlipReport201
        if (!id) { alert('No Record Found For Display'); return; }
        if (typeof window.printRpt === 'function') window.printRpt('201-InvRptPurchaseOrderGeneralSlip.rpt', { id: id });
        else C.printSlip(api + '/' + id + '/slip', 'Purchase Order Slip (201)');
    }

    function readById(id, keepTab) {                                            // ReadById:1723
        return C.getJson(api + '/' + id).then(function (res) {
            var h = res.header;
            RECID = id;
            removedIds = [];
            show('btnSave', false); show('btnUpdate', true); show('btnSaveAs', false);
            if (!keepTab) tab('tabForm');
            $id('DocDate').value = C.isoDay(ci(h, 'DocDate'));
            $id('txtdocno').value = ' PO-' + ci(h, 'DocNo');
            $id('txtBranchSrNo').value = ci(h, 'BranchSrNo');
            $id('combsuppname').value = String(intOf(ci(h, 'OrderSupCustId')));
            $id('txtsupprefno').value = ci(h, 'SupplierRefNo') || '';
            $id('txtremarks').value = ci(h, 'RemarksHeader') || '';
            $id('combpttrm').value = String(intOf(ci(h, 'PaymentTermsId')));
            paymentChanged();
            $id('txtduedays').value = String(ci(h, 'OrderDueDays'));
            $id('duedate').value = C.isoDay(ci(h, 'OrderDueDate'));
            var dt = look.deliveryTerms.find(function (x) { return x.Name === ci(h, 'DeliveryTerm'); });
            $id('combdeliverytrm').value = dt ? String(dt.Id) : '0';
            $id('deliverystartdate').value = C.isoDay(ci(h, 'DeliveryStartDate'));
            $id('txtdeliverydays').value = String(ci(h, 'DeliveryDays'));
            statusSet(ci(h, 'OrderStatus'));
            table = (res.lines || []).map(function (l) { l.Amount = roundEven(num(l.Amount)); return l; });   // :1768 Math.Round(Amount)
            updateDetailIndex = -1;
            render();
            $id('CmbStatus').disabled = false;                                   // CmbStatus.Enabled = btnupdate.Visible
        }).catch(function (e) { alert(e.message); });
    }

    function reset() {                                                          // Reset:1858
        RECID = 0; removedIds = [];
        $id('combsuppname').value = '0'; $id('txtsupprefno').value = '';
        $id('txtduedays').value = ''; $id('txtdeliverydays').value = '';
        $id('combdeliverytrm').value = '0';
        ['cmbItem', 'combitempck', 'combrateuom', 'combjob'].forEach(function (id) { $id(id).value = '0'; });
        ['txtqty', 'txtweight', 'txtRate', 'txtItemAmount', 'txtremarksdetail'].forEach(function (id) { $id(id).value = ''; });
        $id('cmbItem').disabled = false;
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        show('btnSave', true); show('btnUpdate', false); show('btnSaveAs', false);
        table = []; updateDetailIndex = -1;
        statusSet('Open');
        $id('CmbStatus').disabled = true;
        render();
        return C.getJson(api + '/numbers').then(function (n) {                   // DocumentNoFill:794 / BranchSrNoFill:540
            if (n.docNo > 0) $id('txtdocno').value = 'PO-' + n.docNo;
            if (n.branchSrNo > 0) $id('txtBranchSrNo').value = n.branchSrNo;
            $id('DocDate').focus();
        }).catch(function (e) { alert(e.message); });
    }
    function newDoc() { return reset(); }                                       // btnnew_Click:1835
    function refresh() { return init(); }                                       // toolStripButton1_Click:1917

    /* ------------------------------------------------------------------ history */

    var branchSel = {};                                                         // cmbBranchName checked list: id -> bool
    function branchText() {
        return look.historyBranches.filter(function (b) { return branchSel[b.Id]; }).map(function (b) { return b.BranchName; }).join(',');
    }
    function branchIds() { return look.historyBranches.filter(function (b) { return branchSel[b.Id]; }).map(function (b) { return b.Id; }).join(','); }
    function branchRender() {
        $id('cmbBranchText').value = branchText();
        $id('cmbBranchList').innerHTML = look.historyBranches.map(function (b) {
            return '<label class="dz-chk" style="display:flex;padding:2px 4px"><input type="checkbox" data-b="' + b.Id + '"' + (branchSel[b.Id] ? ' checked' : '') +
                ' onchange="PoTrade.branchTick(this)"> ' + esc(b.BranchName) + '</label>';
        }).join('');
    }
    function branchTick(cb) { branchSel[cb.getAttribute('data-b')] = cb.checked; $id('cmbBranchText').value = branchText(); }
    function branchToggle(open) { $id('cmbBranchList').classList.toggle('is-hidden', open === false ? true : !$id('cmbBranchList').classList.contains('is-hidden')); }
    function historyBranchFill() {                                              // HistoryBranchComboFill:2059 - the user's own branch is ticked
        branchSel = {};
        look.historyBranches.forEach(function (b) { branchSel[b.Id] = intOf(b.Id) === intOf(look.userBranchId); });
        branchRender();
    }
    function historyComboFill() {                                               // HistoryComboFill:2010
        if (!branchIds()) { alert('Select branch first'); return Promise.resolve(); }
        return C.getJson(api + '/history-suppliers' + C.qs({ branchIds: branchIds() })).then(function (rows) {
            C.fillSelect('cmbSupplierNameHistory', rows, 'Id', 'Supplier', false);
            var el = $id('cmbSupplierNameHistory'); el.insertAdjacentHTML('afterbegin', '<option value="0"></option>'); el.value = '0';
        }).catch(function (e) { alert(e.message); });
    }
    function refreshHistory() { historyBranchFill(); return historyComboFill(); }     // btnRefreshHistory_Click:1942

    var HCOLS = [['DocNo', 'DocNo'], ['DocDate', 'DocDate', 'd'], ['BranchSrNo', 'BranchSrNo', 'b'], ['BranchName', 'BranchName', 'b'],
        ['SupplierName', 'SupplierName'], ['TermsDescription', 'PaymentTerm'], ['DueDays', 'DueDays', 'n'], ['DueDate', 'DueDate', 'd'],
        ['DeliveryTerm', 'DeliveryTerm'], ['DeliveryStartDate', 'DeliveryStartDate', 'd'], ['OrderExpiryDate', 'OrderExpiryDate', 'd'],
        ['OrderStatus', 'OrderStatus'], ['IsAproved', 'IsApproved', 'a'], ['UserName', 'EntryUser'], ['EntryDate', 'EntryDate', 't'],
        ['ModifyUserName', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 't'], ['RemarksHeader', 'Remarks'], ['NoOfAttachments', 'NoOfAttachments', 'n']];
    function hcell(h, c) {
        var v = ci(h, c[0]);
        if (c[2] === 'd') return C.gridDate(v);
        if (c[2] === 't') return C.gridDateTime(v);
        if (c[2] === 'a') return v === true || v === 1 || String(v).toLowerCase() === 'true' ? 'Approved' : 'Not Approved';
        return v === null || v === undefined ? '' : v;
    }
    function showHistory() {                                                    // btnshow_Click:1955 -> gridhistoryfill:2121
        if (!branchIds()) { alert('Select branch first'); $id('cmbBranchText').focus(); return; }
        var dt = document.querySelector('input[name="histDate"]:checked');
        C.getJson(api + '/history' + C.qs({
            dateType: dt ? dt.value : 'doc',
            fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value),
            supplierId: intOf($id('cmbSupplierNameHistory').value), branchIds: branchIds()
        })).then(function (list) {
            var cols = HCOLS.filter(function (c) { return c[2] !== 'b' || !look.branchImplemented; });          // :2290-2291 Visible = !BranchImplemented
            var t = $id('grdhistory');
            if (!list || !list.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr><th>Print</th><th>Edit</th><th>Save As</th>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = list.map(function (h) {
                var id = intOf(ci(h, 'Id'));
                return '<tr data-id="' + id + '" onclick="PoTrade.historyDetail(this)" ondblclick="PoTrade.open(' + id + ')">' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PoTrade.printId(' + id + ')">Print</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PoTrade.open(' + id + ')">Edit</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PoTrade.saveAsFrom(' + id + ')">Save As</button></td>' +
                    cols.map(function (c) { return '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + esc(hcell(h, c)) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = '';
        }).catch(function (e) { alert(e.message); });
    }
    var DCOLS = [['RefDocNo', 'RefDocNo'], ['ItemCode', 'ItemCode'], ['Item', 'Item'], ['JobLot', 'Job/Lot'], ['PackUom', 'PackUom'],
        ['ItemQty', 'ItemQty', 1], ['TotalQty', 'TotalQty', 1], ['Rate', 'Rate', 1], ['RateUom', 'RateUom'], ['Amount', 'Item Amount', 1], ['Remarks', 'Remarks']];
    function historyDetail(tr) {                                                // grdhistory_SelectionChanged:2373 -> HistoryGridDetailBind:2391
        document.querySelectorAll('#grdhistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (res) {
            var t = $id('grdDetail'), rows = res.lines || [];
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr>' + DCOLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) {
                return '<tr>' + DCOLS.map(function (c) {
                    var v = r[c[0]];
                    if (c[0] === 'Amount') v = roundEven(num(v));
                    else if (c[0] === 'ItemQty' || c[0] === 'TotalQty') v = fmt3(v);
                    return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function newHistory() {                                                     // btnNewHistory_Click:1991
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        $id('cmbSupplierNameHistory').value = '0';
        ['grdhistory', 'grdDetail'].forEach(function (id) { $id(id).tHead.innerHTML = ''; $id(id).tBodies[0].innerHTML = ''; });
        $id('rdpqtydoc').checked = true;
    }
    function saveAsFrom(id) {                                                   // grdhistory_ColumnButtonClick:2319 "SaveAs"
        reset().then(function () {
            return readById(id);
        }).then(function () {
            statusSet('Open');                                                   // Rows[1].Activate()
            show('btnUpdate', false); show('btnSaveAs', true);
        });
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ Purchase Demand loader (frmPendingPurchaseDemand, IsForPreBill) */

    var ldr = { head: [], rows: [], checked: {} };
    function openLoader() {                                                     // btnLoader_Click:3366
        C.openModal('ldrModal');
        C.getJson(api + '/loader/demand/lookups').then(function (l) {
            C.fillSelect('ldrItem', l.items, 'Id', 'Name');
            var el = $id('ldrItem'); if (el.options.length && el.options[0].value === '0') el.options[0].textContent = '-- Select --';
            var f = new Date(); f.setDate(f.getDate() - 7);                       // FromDate = Now - 7 days
            $id('ldrFrom').value = iso(f); $id('ldrTo').value = C.today();
            ldrShow();
        }).catch(function (e) { alert(e.message); });
    }
    function ldrShow() {                                                        // btngrnlod_Click
        C.getJson(api + '/loader/demand' + C.qs({ fromDate: $id('ldrFrom').value, toDate: $id('ldrTo').value,
            fromDocNo: intOf($id('ldrDocFrom').value), toDocNo: intOf($id('ldrDocTo').value), itemId: intOf($id('ldrItem').value) })).then(function (rows) {
            ldr.rows = rows || []; ldr.checked = {};
            var seen = {}; ldr.head = [];
            ldr.rows.forEach(function (r) { var id = intOf(ci(r, 'Id')); if (!seen[id]) { seen[id] = 1; ldr.head.push(r); } });
            ldrHeadRender(); ldrDetailRender();
        }).catch(function (e) { alert(e.message); });
    }
    function ldrHeadRender() {
        var t = $id('ldrGrd');
        t.tHead.innerHTML = '<tr><th></th><th>DocNo</th><th>DocDate</th><th>ParentCategory</th><th>Status</th><th>RemarksHeader</th><th>EntryUserName</th><th>EntryDate</th>' +
            '<th>ModifyUserName</th><th>ModifyDate</th><th>IsApproved</th><th>ApprovedDate</th><th>ApprovalUserName</th></tr>';
        t.tBodies[0].innerHTML = ldr.head.map(function (r) {
            var id = intOf(ci(r, 'Id'));
            return '<tr><td><input type="checkbox"' + (ldr.checked[id] ? ' checked' : '') + ' onchange="PoTrade.ldrTick(' + id + ',this.checked)"></td>' +
                '<td>' + esc(ci(r, 'DocNo')) + '</td><td>' + esc(C.gridDate(ci(r, 'DocDate'))) + '</td><td>' + esc(ci(r, 'ParentCategory')) + '</td><td>' + esc(ci(r, 'Status')) +
                '</td><td>' + esc(ci(r, 'RemarksHeader')) + '</td><td>' + esc(ci(r, 'EntryUserName')) + '</td><td>' + esc(C.gridDate(ci(r, 'EntryDate'))) +
                '</td><td>' + esc(ci(r, 'ModifyUserName')) + '</td><td>' + esc(C.gridDate(ci(r, 'ModifyDate'))) + '</td><td style="text-align:center"><input type="checkbox" disabled' +
                (ci(r, 'IsApproved') ? ' checked' : '') + '></td><td>' + esc(C.gridDate(ci(r, 'ApprovedDate'))) + '</td><td>' + esc(ci(r, 'ApprovalUserName')) + '</td></tr>';
        }).join('');
    }
    function ldrTick(id, on) { ldr.checked[id] = on; ldrDetailRender(); }
    var ldrDetailChecked = {};
    function ldrDetailRender() {                                                // DetailGridBind - all rows start checked (CheckAllRows)
        var t = $id('ldrDetail');
        var rows = ldr.rows.filter(function (r) { return ldr.checked[intOf(ci(r, 'Id'))]; });
        if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th></th><th>DepartmentName</th><th>RequestBy</th><th>ItemName</th><th>ItemCode</th><th>Uom</th><th>ItemCondition</th><th>JobLot</th>' +
            '<th>RequiredQty</th><th>ApprovedQty</th><th>QtyUsedInPreBill</th><th>QtyUsedInGrnDirectly</th><th>BalanceQty</th><th>RemarksDetail</th></tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            var d = intOf(ci(r, 'DetailId'));
            if (ldrDetailChecked[d] === undefined) ldrDetailChecked[d] = true;
            return '<tr><td><input type="checkbox"' + (ldrDetailChecked[d] ? ' checked' : '') + ' onchange="PoTrade.ldrDetailTick(' + d + ',this.checked)"></td>' +
                ['DepartmentName', 'RequestBy', 'ItemName', 'ItemCode', 'Uom', 'ItemCondition', 'JobLot'].map(function (k) { return '<td>' + esc(ci(r, k)) + '</td>'; }).join('') +
                ['RequiredQty', 'ApprovedQty', 'QtyUsedInPreBill', 'QtyUsedInGrnDirectly', 'BalanceQty'].map(function (k) { return '<td class="num">' + esc(fmt3(ci(r, k))) + '</td>'; }).join('') +
                '<td>' + esc(ci(r, 'RemarksDetail')) + '</td></tr>';
        }).join('');
    }
    function ldrDetailTick(d, on) { ldrDetailChecked[d] = on; }
    function ldrLoad() {                                                        // btnLoadOnInvoice_Click_1 then LoadInGridDetailForDemand:3381
        var ids = [];
        ldr.rows.forEach(function (r) {
            if (ldr.checked[intOf(ci(r, 'Id'))] && ldrDetailChecked[intOf(ci(r, 'DetailId'))]) ids.push(intOf(ci(r, 'DetailId')));
        });
        if (!ids.length) { alert('Check the row first in Detail Grid'); return; }
        var existing = {};
        table.forEach(function (r) { existing[intOf(r.RefDocInvoiceId)] = true; });
        ldr.rows.forEach(function (row) {
            var did = intOf(ci(row, 'DetailId'));
            if (ids.indexOf(did) < 0 || existing[did]) return;
            existing[did] = true;
            var eq = num(ci(row, 'UomEquivalent')), bal = num(ci(row, 'BalanceQty'));
            table.push({ Id: 0, RefDocumentTypeId: intOf(ci(row, 'DocumentTypeId')), RefDocId: intOf(ci(row, 'Id')), RefDocInvoiceId: did, RefDocNo: ci(row, 'DocNo'),
                ItemId: intOf(ci(row, 'ItemId')), ItemCode: ci(row, 'ItemCode'), Item: ci(row, 'ItemName'), JobLotId: intOf(ci(row, 'JobLotId')), JobLot: ci(row, 'JobLot'),
                PackUomId: intOf(ci(row, 'ItemSchuomId')), PackUom: ci(row, 'Uom'), PackEquivalent: eq,
                RateUomId: intOf(ci(row, 'ItemSchuomId')), RateUom: ci(row, 'Uom'), RateEquivalent: eq,
                ItemQty: bal, TotalQty: bal * eq, Rate: 0, Amount: 0, Remarks: '', BalQty: bal });
        });
        C.closeModal('ldrModal');
        render();
    }

    /* ------------------------------------------------------------------ shortcut keys (MakeShortCutKeys:3059) */

    var KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Supplier Name'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Item Combo in Detail Box'],
        ['Ctrl+Enter', 'For Update Record When Focus On Any Grid ']];
    function shortcuts() {
        $id('keysBody').innerHTML = KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        C.openModal('keysModal');
    }
    document.addEventListener('keydown', function (e) {                         // PurchsaeOrder_KeyDown:3089
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase(), done = true;
        if (k === 's') { if (!$id('btnSave').classList.contains('is-hidden')) save(); }
        else if (k === 'u') { if (!$id('btnUpdate').classList.contains('is-hidden')) update(); }
        else if (k === 'n') newDoc();
        else if (k === 'r') refresh();
        else if (k === 'p') printId(RECID);
        else if (k === 'e') { window.location.href = '/modules'; }
        else if (e.key === 'F5') $id('combsuppname').focus();
        else if (e.altKey) shortcuts();
        else done = false;
        if (done) e.preventDefault();
    });

    /* ------------------------------------------------------------------ load */

    function init() {                                                           // PurchsaeOrder_Load:401 / toolStripButton1_Click:1917
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update; $id('btnSaveAs').disabled = !r.save; $id('btnPrint').disabled = !r.print;
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Purchase Order Store.'; $id('rightsNote').classList.remove('is-hidden'); }
            fillDef('combsuppname', l.suppliers, 'CompanyName');
            fillDef('combpttrm', l.dueTerms, 'TermsDescription');
            fillDef('combdeliverytrm', l.deliveryTerms, 'Name');
            fillDef('CmbStatus', l.statuses, 'Name');
            fillDef('combjob', l.jobLots, 'JobLotDescription');
            itemBind();
            if (!loaded) {
                $id('ChkBoxPrint').checked = true;
                statusSet('Open');
                $id('CmbStatus').disabled = true;
                $id('txtdocno').value = l.docNo > 0 ? 'PO-' + l.docNo : '';
                $id('txtBranchSrNo').value = l.branchSrNo > 0 ? l.branchSrNo : '';
                var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;       // :526
                var f = new Date(); f.setDate(f.getDate() - days);
                $id('FromDateHistory').value = iso(f); $id('ToDateHistory').value = C.today();
                historyBranchFill();
                historyComboFill();
            }
            loaded = true;
            render();
            return l;
        }).catch(function (e) { alert(e.message); });
    }

    window.PoTrade = {
        newDoc: newDoc, refresh: refresh, save: save, update: update, saveAs: saveAs, tab: tab, open: readById, saveAsFrom: saveAsFrom,
        print: function () { printId(RECID); }, printId: printId, shortcuts: shortcuts,
        itemLeave: itemLeave, searchModeChanged: searchModeChanged, calcWeight: calcWeight, calcAmount: calcAmount,
        decimalKey: decimalKey, digitKey: digitKey, paymentChanged: paymentChanged, dueDaysChanged: dueDaysChanged,
        add: add, updateDetail: updateDetail, resetDetail: resetDetail, edit: edit, del: del, cellEdit: cellEdit,
        showHistory: showHistory, historyDetail: historyDetail, newHistory: newHistory, refreshHistory: refreshHistory,
        branchTick: branchTick, branchToggle: branchToggle,
        openLoader: openLoader, ldrShow: ldrShow, ldrTick: ldrTick, ldrDetailTick: ldrDetailTick, ldrLoad: ldrLoad,
        closeLoader: function () { C.closeModal('ldrModal'); }, closeKeys: function () { C.closeModal('keysModal'); }
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('DocDate').value = C.today();                                       // designer default: DateTime.Now
        $id('duedate').value = C.today(); $id('deliverystartdate').value = C.today();
        document.addEventListener('mousedown', function (e) {
            if (!e.target.closest('#cmbBranchBox')) $id('cmbBranchList').classList.add('is-hidden');
        });
        init().then(function () { resetDetail(); });
    });
})();
