/* ============================================================================================
 * Screen 323 "Purchase Invoice Store Management" — PurchaseInvoiceStoreManagement.cs,
 * DocumentTypeId 64, with its Load-GRN dialog frmPendingGrnStoreLoader.cs (GRN type 48).
 *
 * The grid arithmetic is the form's own, event for event (line refs are PurchaseInvoiceStoreManagement.cs
 * unless marked "ldr:" for the loader). Math.Round on the desktop is banker's rounding (roundEven).
 * The server re-validates and builds the document; Doc No / Branch Sr No / Tax No shown here are
 * display only.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon;
    var $id = C.$id, esc = C.esc, num = C.num;
    var api = '/api/store/purchase-invoice-store-management';
    var DOC_TYPE = 64;

    var look = {};
    var st = null;             // form state (Reset)
    var lastOpenedId = 0;      // VoucherHeadId source — Reset does not clear it (D8)
    var firstLoad = true;
    var hist = { rows: [], branches: [], checked: {}, textOnly: false, selectedId: 0 };
    var ldr = null;            // the loader dialog — a new one per click

    /* ------------------------------------------------------------------ numbers */

    /** .NET Math.Round(double) — MidpointRounding.ToEven. */
    function roundEven(x) {
        var f = Math.floor(x), d = x - f;
        if (d > 0.5) return f + 1;
        if (d < 0.5) return f;
        return (f % 2 === 0) ? f : f + 1;
    }
    function netStr(v) {            // double.ToString()
        if (!isFinite(v)) return String(v);
        return String(parseFloat(Number(v).toPrecision(15)));
    }
    /** Conversion.ToInt(text) — Convert.ToInt32(string): an integer or 0. */
    function intText(s) { var t = String(s === null || s === undefined ? '' : s).trim(); return /^[-+]?\d+$/.test(t) ? parseInt(t, 10) : 0; }
    /** double.ToString("#,##.###") — "" for 0, no leading zero. */
    function fmtHashes(v) {
        var x = Math.abs(num(v));
        if (x === 0) return '';
        var s = (Math.round(x * 1000) / 1000).toString();
        var parts = s.split('.'), ip = parts[0] === '0' ? '' : parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ip + (parts[1] ? '.' + parts[1] : '');
        return (num(v) < 0 ? '-' : '') + out;
    }
    function sum(rows, key) { var t = 0; (rows || []).forEach(function (r) { t += num(r[key]); }); return t; }
    function caption(k) { return String(k).replace(/([a-z])([A-Z])/g, '$1 $2').replace(/([A-Z]+)([A-Z][a-z])/g, '$1 $2'); }
    function today() { return C.today(); }
    function addDays(n) { var d = new Date(); d.setDate(d.getDate() + n); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function fail(e) { alert(e && e.message ? e.message : e); }

    /* ------------------------------------------------------------------ state */

    function blankState() {
        return {
            id: 0, approved: false, grnBase: 0, supplierGlId: '',
            rows: [], freight: [], gl: [], exp: []
        };
    }
    function freightRow() { return { InvGrnId: 0, Transporter: 0, Freight: 0, Debit: 0 }; }      // :904
    function glRow() { return { AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }  // :796
    function expRow() { return { Id: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }  // :985

    /** BindGrids (:603) — the three side grids always show at least one row. */
    function bindGrids() {
        if (!st.freight.length) st.freight.push(freightRow());
        if (!st.gl.length) st.gl.push(glRow());
        if (!st.exp.length) st.exp.push(expRow());
        renderAll();
    }

    /* ------------------------------------------------------------------ item grid */

    /** DetailGridCommonSetting — visible columns, WareHouseName / RackName moved after ItemName. */
    function itemColumns(base) {
        var cols = ['PurchaseOrderNo', 'PurchaseDemandNo', 'PreBillNo', 'DeliveryChallanNo', 'GrnNo', 'ItemName',
            'WareHouseName', 'RackName', 'UOMCodeItem', 'ItemCondition', 'ItemQty', 'Rate', 'ItemAmountWithoutDiscount',
            'DiscountAmount', 'ItemAmount', 'TaxName', 'TaxPercent', 'TaxAmount', 'BillAmount', 'Freights',
            'PoAttachments', 'DemandAttachments', 'PreBillAttachments', 'DeliveryChallanAttachments', 'GrnAttachments'];
        var hide = {};
        if (base === 1) ['PurchaseDemandNo', 'PreBillNo', 'DeliveryChallanNo', 'DemandAttachments', 'PreBillAttachments', 'DeliveryChallanAttachments'].forEach(function (k) { hide[k] = 1; });
        if (base === 2) ['PurchaseOrderNo', 'PreBillNo', 'DeliveryChallanNo', 'PoAttachments', 'PreBillAttachments', 'DeliveryChallanAttachments'].forEach(function (k) { hide[k] = 1; });
        if (base === 3) ['PurchaseOrderNo', 'PoAttachments'].forEach(function (k) { hide[k] = 1; });
        return cols.filter(function (k) { return !hide[k]; });
    }
    var NUMERIC = { ItemQty: 1, Rate: 1, ItemAmountWithoutDiscount: 1, DiscountAmount: 1, ItemAmount: 1, TaxPercent: 1, TaxAmount: 1, BillAmount: 1, Freights: 1 };
    var TOTALS = { ItemQty: 1, ItemAmountWithoutDiscount: 1, DiscountAmount: 1, ItemAmount: 1, TaxAmount: 1, BillAmount: 1, Freights: 1 };

    function itemHeader(cols) {
        return '<tr>' + cols.map(function (k) { return '<th>' + esc(k === 'UOMCodeItem' ? 'UOM' : caption(k)) + '</th>'; }).join('') + '</tr>';
    }
    function cellText(k, v) { return NUMERIC[k] ? netStr(num(v)) : (v === null || v === undefined ? '' : v); }

    function renderItems(tableId, rows, base, editable) {
        var t = $id(tableId), cols = itemColumns(base);
        t.tHead.innerHTML = itemHeader(cols);
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr>' + cols.map(function (k) {
                var ed = editable && ((k === 'Rate' && st.grnBase === 2) || k === 'DiscountAmount');   // :676-681
                if (ed) return '<td class="num"><input type="text" class="num" data-grd="' + i + '" data-k="' + k + '" value="' + esc(netStr(num(r[k]))) + '"></td>';
                return '<td' + (NUMERIC[k] ? ' class="num"' : '') + '>' + esc(cellText(k, r[k])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr>' + cols.map(function (k) { return '<td class="num">' + (TOTALS[k] ? esc(netStr(sum(rows, k))) : '') + '</td>'; }).join('') + '</tr>' : '';
        if (editable) {
            t.querySelectorAll('input[data-grd]').forEach(function (inp) {
                inp.onchange = function () { itemCellUpdated(parseInt(inp.getAttribute('data-grd'), 10), inp.getAttribute('data-k'), inp.value); };
            });
        }
    }

    /** grd_CellUpdated (:696). */
    function itemCellUpdated(i, key, value) {
        var row = st.rows[i]; if (!row) return;
        row[key] = num(value);
        if (key === 'Rate') row.ItemAmountWithoutDiscount = num(row.ItemQty) * num(row.Rate);
        if (key === 'Rate' || key === 'DiscountAmount') recalculateRow(row);
        billAmount();
        renderAll();
    }

    /** RecalculateRow (:726). */
    function recalculateRow(row) {
        var itemAmount = num(row.ItemAmountWithoutDiscount) - num(row.DiscountAmount);
        row.ItemAmount = itemAmount;
        row.TaxAmount = itemAmount * num(row.TaxPercent) / 100;
        row.BillAmount = itemAmount + num(row.ExpenseAmount) + num(row.Freights);
    }

    /* ------------------------------------------------------------------ side grids */

    function accountOptions(list, sel) {
        return '<option value="0"></option>' + (list || []).map(function (a) {
            return '<option value="' + a.Id + '"' + (a.Id === sel ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>';
        }).join('');
    }
    function btns(grid, i) {
        return '<td><button type="button" class="cx-x" data-g="' + grid + '" data-a="del" data-i="' + i + '">X</button></td>' +
            '<td><button type="button" class="cx-link" data-g="' + grid + '" data-a="add" data-i="' + i + '">+</button></td>';
    }
    function numInput(grid, i, k, v, ro) {
        return '<td class="num"><input type="text" class="num" data-g="' + grid + '" data-i="' + i + '" data-k="' + k + '" value="' + esc(netStr(num(v))) + '"' + (ro ? ' disabled' : '') + '></td>';
    }

    /** grdFreightSettings (:880): InvGrnId / Debit hidden, Freight captioned "Credit". */
    function renderFreight() {
        var t = $id('grdFreight');
        t.tHead.innerHTML = '<tr><th></th><th></th><th>Transporter</th><th>Credit</th></tr>';
        t.tBodies[0].innerHTML = st.freight.map(function (r, i) {
            var ro = num(r.InvGrnId) !== 0;                                   // grdFreight_SelectionChanged (:965)
            return '<tr>' + btns('fr', i) +
                '<td><select class="win-combo" data-g="fr" data-i="' + i + '" data-k="Transporter"' + (ro ? ' disabled' : '') + '>' + accountOptions(look.glAccounts, C.intOf(r.Transporter)) + '</select></td>' +
                numInput('fr', i, 'Freight', r.Freight, ro) + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td class="num">' + esc(netStr(sum(st.freight, 'Freight'))) + '</td></tr>';
    }

    /** gridGLSettings (:775). */
    function renderGl() {
        var t = $id('grdGLedger');
        t.tHead.innerHTML = '<tr><th></th><th></th><th>Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th></tr>';
        t.tBodies[0].innerHTML = st.gl.map(function (r, i) {
            return '<tr>' + btns('gl', i) +
                '<td><select class="win-combo" data-g="gl" data-i="' + i + '" data-k="AccountId">' + accountOptions(look.glAccounts, C.intOf(r.AccountId)) + '</select></td>' +
                '<td><input type="text" data-g="gl" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks || '') + '"></td>' +
                numInput('gl', i, 'Percentage', r.Percentage) + numInput('gl', i, 'Qty', r.Qty) + numInput('gl', i, 'Rate', r.Rate) +
                numInput('gl', i, 'Debit', r.Debit) + numInput('gl', i, 'Credit', r.Credit) + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td colspan="7"></td><td class="num">' + esc(netStr(sum(st.gl, 'Debit'))) + '</td><td class="num">' + esc(netStr(sum(st.gl, 'Credit'))) + '</td></tr>';
    }

    /** grdInvExpSettings (:1018): Id hidden; ItemId is the "Other Item Name" combo. */
    function renderExp() {
        var t = $id('grdInvExp');
        t.tHead.innerHTML = '<tr><th></th><th></th><th>Other Item Name</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = st.exp.map(function (r, i) {
            var opts = '<option value="0"></option>' + (look.otherItems || []).map(function (o) {
                return '<option value="' + o.Id + '"' + (o.Id === C.intOf(r.ItemId) ? ' selected' : '') + '>' + esc(o.OtherItemName) + '</option>';
            }).join('');
            return '<tr>' + btns('ex', i) +
                '<td><select class="win-combo" data-g="ex" data-i="' + i + '" data-k="ItemId">' + opts + '</select></td>' +
                numInput('ex', i, 'Qty', r.Qty) + numInput('ex', i, 'Rate', r.Rate) + numInput('ex', i, 'Amount', r.Amount) +
                '<td><input type="text" data-g="ex" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks || '') + '"></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td colspan="3"></td><td class="num">' + esc(netStr(sum(st.exp, 'Qty'))) + '</td><td></td><td class="num">' + esc(netStr(sum(st.exp, 'Amount'))) + '</td><td></td></tr>';
    }

    function wireSideGrids() {
        ['grdFreight', 'grdGLedger', 'grdInvExp'].forEach(function (tid) {
            var t = $id(tid);
            t.onclick = function (e) {
                var b = e.target.closest('button[data-a]'); if (!b) return;
                sideButton(b.getAttribute('data-g'), b.getAttribute('data-a'), parseInt(b.getAttribute('data-i'), 10));
            };
            t.onchange = function (e) {
                var el = e.target; if (!el.hasAttribute('data-g')) return;
                sideCell(el.getAttribute('data-g'), parseInt(el.getAttribute('data-i'), 10), el.getAttribute('data-k'), el.value);
            };
        });
    }

    /** grdFreight_ColumnButtonClick (:908) / grdGLedger_ColumnButtonClick (:849) / grdInvExp_ColumnButtonClick (:1057). */
    function sideButton(g, a, i) {
        if (g === 'fr') {
            if (a === 'add') st.freight.push(freightRow());
            if (a === 'del') {
                if (num(st.freight[i].InvGrnId) > 0) { alert('Record cannot be deleted because this record againt grn'); return; }
                st.freight.splice(i, 1);
                if (!st.freight.length) st.freight.push(freightRow());
            }
            billAmount();
        } else if (g === 'gl') {
            if (a === 'add') st.gl.push(glRow());
            if (a === 'del') { st.gl.splice(i, 1); if (!st.gl.length) st.gl.push(glRow()); }
            billAmount();
        } else {
            if (a === 'del') { st.exp.splice(i, 1); if (!st.exp.length) st.exp.push(expRow()); }
            if (a === 'add') st.exp.push(expRow());
            billAmount();
        }
        renderAll();
    }

    function sideCell(g, i, k, v) {
        if (g === 'fr') {                                                         // grdFreight_CellUpdated (:944)
            var r = st.freight[i];
            r[k] = k === 'Transporter' ? C.intOf(v) : num(v);
            if (k === 'Freight' && num(r.Debit) > 0) { r.Freight = 0; alert('Debit Side is aleady added'); }
            if (k === 'Debit' && num(r.Freight) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); }
            billAmount();
            freightProportion();
        } else if (g === 'gl') {                                                  // grdGLedger_CellUpdated (:800)
            var item = st.gl[i];
            item[k] = k === 'AccountId' ? C.intOf(v) : (k === 'Remarks' ? v : num(v));
            if (k === 'Qty' || k === 'Rate') {
                item.Credit = num(item.Qty) * num(item.Rate); item.Debit = 0; item.Percentage = 0;
            }
            if (k === 'Percentage') {
                var tot = sum(st.rows, 'ItemAmount') / 100 * num(item.Percentage);
                if (tot > 0) { item.Credit = roundEven(tot); item.Debit = 0; }
                else { item.Debit = Math.abs(roundEven(tot)); item.Credit = 0; }
                item.Qty = 0; item.Rate = 0;
            }
            if (k === 'Credit' && num(item.Debit) > 0) { item.Credit = 0; alert('Debit Side is aleady added'); }
            if (k === 'Debit' && num(item.Credit) > 0) { item.Debit = 0; alert('Credit Side is aleady added'); }
            billAmount();
        } else {                                                                  // grdInvExp_CellUpdated (:1084)
            var row = st.exp[i];
            row[k] = k === 'ItemId' ? C.intOf(v) : (k === 'Remarks' ? v : num(v));
            if (k === 'Qty' || k === 'Rate') row.Amount = num(row.Qty) * num(row.Rate);
            else if (k === 'Amount') row.Rate = num(row.Qty) === 0 ? 0 : num(row.Amount) / num(row.Qty);
            else { renderAll(); return; }
            billAmount();
        }
        renderAll();
    }

    function renderAll() {
        renderItems('grd', st.rows, st.grnBase, true);
        renderFreight();
        renderGl();
        renderExp();
    }

    /* ------------------------------------------------------------------ totals */

    /** BillAmount (:1190). */
    function billAmount() {
        var itemAmountWithoutDiscount = roundEven(sum(st.rows, 'ItemAmountWithoutDiscount'));
        var itemDiscount = roundEven(sum(st.rows, 'DiscountAmount'));
        var itemNetAmount = roundEven(sum(st.rows, 'ItemAmount'));
        var taxAmount = roundEven(sum(st.rows, 'TaxAmount'));
        var itemPlusTax = itemNetAmount + taxAmount;
        $id('txtItemAmountWithoutDiscountFooter').value = netStr(itemAmountWithoutDiscount);
        if (document.activeElement !== $id('txtItemDiscountAmountFooter')) $id('txtItemDiscountAmountFooter').value = netStr(itemDiscount);
        $id('txtItemNetAmountFooter').value = netStr(itemNetAmount);
        $id('txtTaxAmountFooter').value = netStr(taxAmount);
        $id('txtItemAmountPlusTaxAmountFooter').value = netStr(itemPlusTax);
        var totalExpense = roundEven(sum(st.exp, 'Amount'));
        $id('txtExpenseAmount').value = netStr(totalExpense);
        var bill = itemPlusTax + totalExpense;
        var jd = 0, jc = 0;
        if (C.intOf($id('cmbsuppliername').value) > 0) {                          // cmbsuppliername.ActiveRow != null
            for (var i = 0; i < st.gl.length; i++) {
                var r = st.gl[i], acc = C.intOf(r.AccountId);
                if (acc > 0) {
                    if (String(st.supplierGlId) === String(acc)) {
                        r.AccountId = 0;
                        renderGl();
                        alert('You cannot select supplier Account');
                        return;
                    }
                    jd += num(r.Debit); jc += num(r.Credit);
                }
            }
        }
        var addLess = jd - jc;
        $id('txtPartyAddLessAmountFooter').value = netStr(addLess);
        bill += addLess;
        var supplierId = C.intOf($id('cmbsuppliername').value);
        var alt = look.erpFeature4 ? supplierId : intText(st.supplierGlId);
        var freight = 0;
        st.freight.forEach(function (f) {
            var t = C.intOf(f.Transporter);
            if (t !== 0 && t === alt) freight += num(f.Freight);
        });
        $id('txtFreightAmuontFooter').value = netStr(freight);
        bill += freight;
        bill -= num($id('txtBillDiscountAmountFooter').value);
        $id('txtBillAmount').value = netStr(roundEven(bill));
        billProportion();
    }

    /** FreightProportion (:1261). */
    function freightProportion() {
        var netWeight = sum(st.rows, 'ItemQty');
        var credit = sum(st.freight, 'Freight'), debit = sum(st.freight, 'Debit');
        if (debit < credit) {
            var diff = credit - debit;
            st.rows.forEach(function (r) { r.Freights = roundEven(diff) / netWeight * num(r.ItemQty); });
        } else {
            st.rows.forEach(function (r) { r.Freights = 0; });                    // equal → 0, debit > credit → 0
        }
        billProportion();
    }

    /** BillProportion (:1319). */
    function billProportion() {
        st.rows.forEach(function (r) { r.BillAmount = num(r.ItemAmount) + num(r.Freights) + num(r.ExpenseAmount); });
    }

    /** ItemDiscountProportion (:1337) — only while the footer box has the focus. */
    function itemDiscountProportion() {
        if (document.activeElement !== $id('txtItemDiscountAmountFooter')) return;
        var total = num($id('txtItemDiscountAmountFooter').value);
        var qty = sum(st.rows, 'ItemQty');
        var has = total > 0 && qty > 0;
        var per = has ? total / qty : 0;
        st.rows.forEach(function (r) { r.DiscountAmount = has ? per * num(r.ItemQty) : 0; recalculateRow(r); });
    }

    function itemDiscountFooterChanged() { itemDiscountProportion(); billAmount(); renderItems('grd', st.rows, st.grnBase, true); }
    function billDiscountChanged() { billAmount(); renderItems('grd', st.rows, st.grnBase, true); }

    /* ------------------------------------------------------------------ header events */

    /** txtDueDays_TextChanged (:2719). */
    function dueDaysChanged() {
        var t = $id('txtDueDays').value.trim();
        $id('DueDate').value = t !== '' ? addDays(Math.trunc(num(t))) : today();
    }

    /** cmbsuppliername_ValueChanged (:2799) — the GL id is only replaced when a supplier is chosen. */
    function supplierChanged() {
        var id = C.intOf($id('cmbsuppliername').value);
        if (id > 0) {
            var s = (look.suppliers || []).find(function (x) { return x.Id === id; });
            if (s) st.supplierGlId = String(s.GlAccountId);
        }
    }

    /** DocDate_Leave (:2738). */
    function docDateLeave() {
        if (!st.rows.length) return;
        var ids = st.rows.map(function (r) { return ',' + r.ItemId; }).join('');
        C.getJson(api + '/tax-schedule' + C.qs({ itemIds: ids, docDate: $id('DocDate').value, recId: st.id })).then(function (dt) {
            st.rows.forEach(function (item) {
                var taxPercent = 0, itemAmount = 0, taxAmount = 0, flag = false;
                dt.forEach(function (d) {
                    if (d.ItemId === C.intOf(item.ItemId)) {
                        flag = true;
                        item.TaxNameId = d.TaxNameId;
                        item.TaxName = d.TaxName;
                        taxPercent = num(d.TaxPercent);
                        item.TaxPercent = taxPercent;
                        itemAmount = num(item.ItemAmount);
                        if (itemAmount > 0 && taxPercent > 0) { taxAmount = itemAmount * taxPercent / 100; item.TaxAmount = taxAmount; }
                        item.BillAmount = itemAmount + taxAmount + num(item.Freights);
                    }
                });
                if (!flag) {
                    item.TaxNameId = 0; item.TaxName = '0'; item.TaxPercent = 0; item.TaxAmount = 0;
                    item.BillAmount = num(item.ItemAmount) + num(item.Freights);
                }
            });
            billAmount();
            renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ reset / open */

    function applyRights() {
        var r = look.rights || {};
        $id('btnSave').disabled = !r.save;
        $id('btnUpdate').disabled = !r.update;
        $id('btnDelete').disabled = !r.delete;
        $id('btnPrint').disabled = !r.print;
        var n = $id('rightsNote');
        if (!r.view) { n.textContent = 'You do not have the View right for this screen.'; n.classList.remove('is-hidden'); }
    }

    function bindCombos() {
        C.fillSelect('cmbsuppliername', look.suppliers, 'Id', 'CompanyName');
        C.fillSelect('CmbPaymentTerm', look.paymentTerms, 'Id', 'Description');
        C.fillSelect('CmbTaxAccount', look.taxAccounts, 'Id', 'AccountTitle');
        C.fillSelect('CmbDiscountAccount', look.discountAccounts, 'Id', 'AccountTitle');
    }

    /** Reset (:1846) — Doc Date and the two preview boxes are not touched. */
    function reset() {
        st = blankState();
        $id('btnSave').classList.remove('is-hidden');
        $id('btnUpdate').classList.add('is-hidden');
        $id('btnDelete').classList.add('is-hidden');
        $id('cmbsuppliername').disabled = false;
        $id('cmbsuppliername').value = '0';
        $id('txtbillno').value = '';
        $id('CmbPaymentTerm').value = '0';
        $id('txtDueDays').value = '0';
        dueDaysChanged();
        $id('txtRemarks').value = '';
        ['txtItemAmountWithoutDiscountFooter', 'txtItemDiscountAmountFooter', 'txtItemNetAmountFooter', 'txtTaxAmountFooter',
            'txtItemAmountPlusTaxAmountFooter', 'txtExpenseAmount', 'txtBillDiscountAmountFooter', 'txtFreightAmuontFooter',
            'txtPartyAddLessAmountFooter', 'txtBillAmount'].forEach(function (id) { $id(id).value = ''; });
        $id('CmbTaxAccount').value = '0';
        $id('CmbDiscountAccount').value = '0';
        bindGrids();
        return C.getJson(api + '/numbers').then(function (n) {
            $id('txtdocno').value = n.docNo;
            $id('txtInvoiceTaxNo').value = n.taxNo;
            $id('txtBranchSrNo').value = n.branchSrNo;
        }).catch(fail);
    }

    /** ReadById (:1721). */
    function readById(id) {
        return reset().then(function () {
            return C.getJson(api + '/' + id);
        }).then(function (o) {
            st.id = o.Id;
            lastOpenedId = o.Id;
            tab('tabForm');
            $id('btnSave').classList.add('is-hidden');
            $id('btnUpdate').classList.remove('is-hidden');
            $id('btnDelete').classList.remove('is-hidden');
            $id('txtBranchSrNo').value = o.BranchSrNo;
            $id('txtdocno').value = o.DocNo;
            $id('DocDate').value = C.isoDay(o.DocDate);
            $id('cmbsuppliername').value = String(o.SupplierCustomerId);
            supplierChanged();
            $id('txtbillno').value = o.ManualBillNo || '';
            $id('CmbTaxAccount').value = String(o.TaxAccountId);
            $id('txtInvoiceTaxNo').value = o.SalesTaxNo;
            $id('CmbPaymentTerm').value = String(o.PaymentTermsId);
            $id('txtDueDays').value = o.DueDays;
            dueDaysChanged();
            $id('DueDate').value = C.isoDay(o.DueDate);
            $id('txtBillAmount').value = netStr(num(o.BillAmount));
            $id('txtRemarks').value = o.RemarksHeader || '';
            $id('CmbDiscountAccount').value = String(o.DiscountAccountId);
            $id('txtBillDiscountAmountFooter').value = netStr(num(o.DiscountAmount));
            st.approved = !!o.IsApproved;
            st.grnBase = C.intOf(o.GrnBaseDocumentTypeId);
            st.rows = o.rows || [];
            st.freight = o.freight || [];
            st.gl = o.journal || [];
            st.exp = o.expenses || [];
            bindGrids();
            billAmount();
            freightProportion();
            billProportion();
            renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ save / delete / print */

    function payload(confirmed) {
        return {
            confirmed: confirmed,
            Id: st.id,
            DocDate: $id('DocDate').value,
            SupplierCustomerId: C.intOf($id('cmbsuppliername').value),
            ManualBillNo: $id('txtbillno').value,
            PaymentTermsId: C.intOf($id('CmbPaymentTerm').value),
            DueDays: $id('txtDueDays').value,
            DueDate: $id('DueDate').value,
            RemarksHeader: $id('txtRemarks').value,
            TaxAccountId: C.intOf($id('CmbTaxAccount').value),
            DiscountAccountId: C.intOf($id('CmbDiscountAccount').value),
            DiscountAmount: $id('txtBillDiscountAmountFooter').value,
            BillAmount: $id('txtBillAmount').value,
            GrnBaseDocumentTypeId: st.grnBase,
            rows: st.rows.map(function (r) {
                var o = {};
                ['Id', 'PurchaseOrderDocumentTypeId', 'PurchaseOrderId', 'PurchaseOrderNo', 'PurchaseDemandDocumentTypeId', 'PurchaseDemandId',
                    'PurchaseDemandNo', 'PreBillDocumentTypeId', 'PreBillId', 'PreBillNo', 'DeliveryChallanDocumentTypeId', 'DeliveryChallanId',
                    'DeliveryChallanNo', 'InvGrnDocumentTypeId', 'InvGrnId', 'InvGrnDetailId', 'GrnNo', 'WarehouseId', 'ItemId', 'ItemUomId',
                    'ItemConditionId', 'TaxNameId', 'PoAttachments', 'DemandAttachments', 'PreBillAttachments', 'DeliveryChallanAttachments',
                    'GrnAttachments', 'RackId'].forEach(function (k) { o[k] = C.intOf(r[k]); });
                ['ItemQty', 'Rate', 'ItemAmountWithoutDiscount', 'DiscountAmount', 'ItemAmount', 'TaxPercent', 'TaxAmount', 'BillAmount',
                    'Freights', 'ExpenseAmount'].forEach(function (k) { o[k] = num(r[k]); });
                ['WareHouseName', 'ItemName', 'UOMCodeItem', 'ItemCondition', 'TaxName', 'RackName'].forEach(function (k) {
                    o[k] = r[k] === null || r[k] === undefined ? null : String(r[k]);
                });
                return o;
            }),
            freight: st.freight.map(function (f) { return { InvGrnId: C.intOf(f.InvGrnId), Transporter: C.intOf(f.Transporter), Freight: num(f.Freight), Debit: num(f.Debit) }; }),
            journal: st.gl.map(function (j) {
                return { AccountId: C.intOf(j.AccountId), Remarks: j.Remarks || '', Percentage: num(j.Percentage), Qty: num(j.Qty), Rate: num(j.Rate), Debit: num(j.Debit), Credit: num(j.Credit) };
            }),
            expenses: st.exp.map(function (e) {
                var o = (look.otherItems || []).find(function (x) { return x.Id === C.intOf(e.ItemId); });
                return { Id: C.intOf(e.Id), ItemId: C.intOf(e.ItemId), ItemText: o ? o.OtherItemName : '', Qty: num(e.Qty), Rate: num(e.Rate), Amount: num(e.Amount), Remarks: e.Remarks || '' };
            })
        };
    }

    /** Insert (:1383) — the checks before the prompt run on the server first (W3). */
    function insert() {
        C.postJson(api + '/save', payload(false)).then(function (r) {
            if (!r || !r.confirm) return;
            if (!window.confirm(r.confirm + '?')) return;
            return C.postJson(api + '/save', payload(true)).then(function (res) {
                alert(res.message);
                var id = res.id;
                return reset().then(function () {
                    if ($id('ChkBok').checked) openSlip(id, DOC_TYPE);                    // :1615
                    if ($id('chkVoucherPreview').checked) voucher(id);                    // :1619
                });
            });
        }).catch(function (e) { alert(e.message); });
    }
    function save() { st.id = 0; insert(); }                                              // btnSave_Click sets Id = 0
    function update() { insert(); }

    /** btnDelete_Click (:1681). */
    function remove() {
        if (st.approved) { alert('Record Not Update because Record has approved'); return; }
        if (!(st.id > 0)) { alert('RecordId Not Found.....'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + st.id + '/delete', {}).then(function (r) { alert(r.message); return reset(); }).catch(fail);
    }

    /** OpenSlip (:2688) — 64 → 230, otherwise 238. */
    function openSlip(id, docType) {
        C.printSlip(api + '/' + (id || 0) + '/slip' + C.qs({ documentTypeId: docType }),
            docType === DOC_TYPE ? '230-Purchase Bill Store Slip' : '238-Purchase Invoice Store Bill With Tax');
    }
    function voucher(id) { C.printSlip(api + '/voucher-slip' + C.qs({ id: id || 0 }), '103-Purchase / Sales Voucher Slip'); }
    function printSlip() { openSlip(st.id, DOC_TYPE); }                                    // btnSlipDetail_Click
    function printVoucher() { voucher(lastOpenedId); }                                    // btnPrint_Click (D8)

    /** btnFrmRefresh_Click (:1902) — rebinds the globals, keeping the selections. */
    function refresh() {
        C.getJson(api + '/refresh').then(function (r) {
            Object.keys(r).forEach(function (k) { look[k] = r[k]; });
            bindCombos();
            renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load GRN */

    function branchBox(boxId, listId, textId, branches, state) {
        var list = $id(listId);
        list.innerHTML = branches.map(function (b) {
            return '<label><input type="checkbox" value="' + b.Id + '"' + (state.checked[b.Id] ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>';
        }).join('');
        list.onchange = function (e) {
            var cb = e.target; if (cb.type !== 'checkbox') return;
            if (cb.checked) state.checked[cb.value] = 1; else delete state.checked[cb.value];
            state.textOnly = false;
            branchText(textId, branches, state);
            if (state.onChange) state.onChange();
        };
        branchText(textId, branches, state);
    }
    function branchText(textId, branches, state) {
        var names = branches.filter(function (b) { return state.checked[b.Id]; }).map(function (b) { return b.BranchName; });
        $id(textId).value = names.length ? names.join(',') : (state.textOnly ? state.text : '');
    }
    function branchIds(state) { return Object.keys(state.checked).join(','); }
    function toggleBranches(boxId) { $id(boxId).classList.toggle('is-open'); }
    /** cmbBranchName.Text = UserAccount.BranchName — the user's branch is ticked (a name not in the list stays as text only). */
    function defaultBranch(state, branches, userId, userName) {
        state.checked = {};
        state.textOnly = false;
        if (!branches.length) return;
        var mine = branches.find(function (b) { return b.Id === userId; });
        if (mine) state.checked[mine.Id] = 1;
        else if (userName) { state.textOnly = true; state.text = userName; }
        else state.checked[branches[0].Id] = 1;
    }

    /** toolStripButton3_Click_1 (:1926) — a NEW dialog per click, so every filter starts fresh. */
    function openLoader() {
        ldr = { branches: [], branch: { checked: {} }, rows: [], checked: {}, current: -1 };
        C.getJson(api + '/loader/lookups').then(function (l) {
            ldr.branches = l.branches || [];
            ldr.fyStart = l.financialYearStart;
            /* BranchesFill (ldr:144): Text = UserAccount.BranchName, else the previous text, else the first row. */
            ldr.branch.checked = {};
            var mine = ldr.branches.find(function (b) { return b.Id === l.userBranchId; });
            if (mine) ldr.branch.checked[mine.Id] = 1; else if (ldr.branches.length) ldr.branch.checked[ldr.branches[0].Id] = 1;
            ldr.branch.onChange = loaderCombos;                                   // cmbBranchName_Leave (ldr:208)
            branchBox('ldrBranchBox', 'ldrBranchNameList', 'ldrBranchName', ldr.branches, ldr.branch);
            $id('ldrFromDate').value = ldr.fyStart || today();
            $id('ldrToDate').value = today();
            $id('txtDocNoFrom').value = '';
            $id('txtDocNoTo').value = '';
            $id('CmbBillToParty').innerHTML = '';
            $id('CmbItemName').innerHTML = '';
            $id('ldrGrd').tHead.innerHTML = ''; $id('ldrGrd').tBodies[0].innerHTML = '';
            $id('ldrGrdDetail').tHead.innerHTML = ''; $id('ldrGrdDetail').tBodies[0].innerHTML = '';
            C.openModal('dlgGrn');
            return loaderCombos().then(loaderShow);
        }).catch(fail);
    }

    function loaderCombos() {
        return C.getJson(api + '/loader/combos' + C.qs({ branchIds: branchIds(ldr.branch) })).then(function (c) {
            C.fillSelect('CmbBillToParty', c.suppliers, 'Id', 'Name');
            C.fillSelect('CmbItemName', c.items, 'Id', 'Name');
        }).catch(fail);
    }

    /** btngrnlod_Click (ldr:252) → PendingDataDbCall + GrdDataBind. */
    function loaderShow() {
        return C.getJson(api + '/loader/pending' + C.qs({
            branchIds: branchIds(ldr.branch), fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value,
            docNoFrom: intText($id('txtDocNoFrom').value), docNoTo: intText($id('txtDocNoTo').value),
            itemId: C.intOf($id('CmbItemName').value), billToPartyId: C.intOf($id('CmbBillToParty').value)
        })).then(function (rows) {
            ldr.rows = rows || [];
            ldr.checked = {};
            ldr.current = -1;
            renderLoaderMaster();
            renderLoaderDetail();
        }).catch(fail);
    }

    var LDR_COLS = ['BaseDocumentType', 'DocNo', 'DocDate', 'SupplierName', 'TransporterName', 'CarriageAmount', 'GpNo', 'GPDate',
        'VehicleType', 'VehicleNo', 'BiltyNo', 'EntryDate', 'EntryUserName', 'ModifyDate', 'ModifyUserName', 'RemarksHeader', 'NoOfAttachments'];

    /** GrdDataBind (ldr:298) — one row per GRN Id. */
    function loaderHeaders() {
        var seen = {}, out = [];
        ldr.rows.forEach(function (r) { if (!seen[r.Id]) { seen[r.Id] = 1; out.push(r); } });
        return out;
    }
    function renderLoaderMaster() {
        var t = $id('ldrGrd'), heads = loaderHeaders();
        if (!heads.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th>Select</th>' + LDR_COLS.map(function (k) { return '<th>' + esc(caption(k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = heads.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (ldr.current === i ? ' class="is-selected"' : '') + '><td><input type="checkbox" data-id="' + r.Id + '"' + (ldr.checked[r.Id] ? ' checked' : '') + '></td>' +
                LDR_COLS.map(function (k) {
                    var v = r[k];
                    if (k === 'DocDate' || k === 'GPDate') v = C.gridDate(v);
                    if (k === 'EntryDate' || k === 'ModifyDate') v = C.gridDate(v);
                    return '<td' + (k === 'CarriageAmount' ? ' class="num"' : '') + '>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].onclick = function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var cb = e.target.closest('input[type=checkbox]');
            if (cb) { if (cb.checked) ldr.checked[cb.getAttribute('data-id')] = 1; else delete ldr.checked[cb.getAttribute('data-id')]; }
            ldr.current = parseInt(tr.getAttribute('data-i'), 10);                   // grd_SelectionChanged / RowCheckStateChanged
            t.tBodies[0].querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-selected', x === tr); });
            renderLoaderDetail();
        };
    }

    var LDR_DETAIL_COLS = [['DocNo', 'Doc No'], ['PurchaseOrderNo', 'Purchase Order No'], ['PurchaseOrderDate', 'Purchase Order Date'],
        ['PurchaseDemandNo', 'Purchase Demand No'], ['PurchaseDemandDate', 'Purchase Demand Date'], ['PurchasePreBillNo', 'Purchase Pre Bill No'],
        ['PurchasePreBillDate', 'Purchase Pre Bill Date'], ['DeliveryChallanNo', 'Delivery Challan No'], ['DeliveryChallanDate', 'Delivery Challan Date'],
        ['WareHouseName', 'Ware House Name'], ['ItemCode', 'Item Code'], ['ItemName', 'Item Name'], ['Uom', 'Uom'], ['ItemCondition', 'Item Condition'],
        ['ItemQty', 'Item Qty'], ['PurchaseOrderQty', 'Purchase Order Qty'], ['PurchaseDemandQty', 'Purchase Demand Qty'],
        ['PurchasePreBillQty', 'Purchase Pre Bill Qty'], ['DeliveryChallanQty', 'Delivery Challan Qty'], ['RemarksDetail', 'Remarks Detail'],
        ['NoOfAttachmentsPO', 'PO Attachtments'], ['NoOfAttachmentsPD', 'Demand Attachtments'], ['NoOfAttachmentsPreBill', 'PreBill Attachtments'],
        ['NoOfAttachmentsDC', 'DC Attachtments']];

    /** DetailGridBind (ldr:471) — the checked GRNs, or the current row when none is checked. */
    function renderLoaderDetail() {
        var t = $id('ldrGrdDetail');
        var ids = Object.keys(ldr.checked);
        if (!ids.length) {
            var heads = loaderHeaders();
            if (ldr.current < 0 || !heads[ldr.current]) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
            ids = [String(heads[ldr.current].Id)];
        }
        var rows = ldr.rows.filter(function (r) { return ids.indexOf(String(r.Id)) >= 0; });
        t.tHead.innerHTML = '<tr>' + LDR_DETAIL_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr>' + LDR_DETAIL_COLS.map(function (c) {
                var v = r[c[0]];
                if (/Date$/.test(c[0])) v = (!v || /^1900-01-01/.test(String(v))) ? '' : C.gridDate(v);   // CheckDateTimeNull → ""
                return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }

    /** btnReset_Click (ldr:674) — the Supplier box is not cleared. */
    function loaderNew() {
        $id('ldrFromDate').value = ldr.fyStart || today();
        $id('txtDocNoFrom').value = '';
        $id('txtDocNoTo').value = '';
        $id('CmbItemName').value = '0';
        loaderShow();
    }

    /** btnRefresh_Click (ldr:692) — BranchesFill + CombosFill. */
    function loaderRefresh() {
        C.getJson(api + '/loader/lookups').then(function (l) {
            ldr.branches = l.branches || [];
            var mine = ldr.branches.find(function (b) { return b.Id === l.userBranchId; });
            if (mine) { ldr.branch.checked = {}; ldr.branch.checked[mine.Id] = 1; }
            branchBox('ldrBranchBox', 'ldrBranchNameList', 'ldrBranchName', ldr.branches, ldr.branch);
            return loaderCombos();
        }).catch(fail);
    }

    /** btnLoadOnInvoice_Click_1 (ldr:705). */
    function loaderLoad() {
        var ids = Object.keys(ldr.checked);
        if (!ids.length) { alert('Please select at least one record from the grid.'); return; }
        var detail = ldr.rows.filter(function (r) { return ids.indexOf(String(r.Id)) >= 0; });
        if (!detail.length) { alert('No detail records found for selected rows.'); return; }
        var party = null, base = null;
        for (var i = 0; i < detail.length; i++) {
            var p = C.intOf(detail[i].SupplierCustomerId), b = C.intOf(detail[i].BaseDocumentTypeId);
            if (party === null) party = p;
            if (base === null) base = b;
            if (p !== party) { alert('Sorry! You can only load records of the same Party.'); return; }
            if (b !== base) { alert('Sorry! You can only load records of the same Base Document.'); return; }
        }
        var seen = {}, dtLoader = [];
        detail.forEach(function (r) { if (!seen[r.DetailId]) { seen[r.DetailId] = 1; dtLoader.push(r); } });
        C.closeModal('dlgGrn');
        loadInGridDetailFromGrn(dtLoader);
    }

    /** The dialog's close box — FormClosed sets dtLoader = null (ldr:668). */
    function closeLoader() { C.closeModal('dlgGrn'); loadInGridDetailFromGrn(null); }

    var BASE_NAMES = { 1: 'Grn Against Purchase_Order', 2: 'Grn Against Purchase_Demand', 3: 'Grn Against DeliveryChallan' };

    /** LoadInGridDetailFromGrn (:1940). */
    function loadInGridDetailFromGrn(dtLoader) {
        if (!dtLoader || !dtLoader.length) { if (!st.rows.length) st.grnBase = 0; return; }
        var frt = C.distinct(dtLoader.filter(function (r) { return r.Id !== null && r.Id !== undefined; }), 'Id').map(function (r) { return C.intOf(r.Id); }).join(',');
        var dcSeen = {}, dc = [];
        dtLoader.forEach(function (r) {
            if (r.DeliveryChallanId === null || r.DeliveryChallanId === undefined) return;
            var v = C.intOf(r.DeliveryChallanId); if (!dcSeen[v]) { dcSeen[v] = 1; dc.push(v); }
        });
        var first = dtLoader[0];
        var party = C.intOf(first.SupplierCustomerId), newBase = C.intOf(first.BaseDocumentTypeId);
        if (st.grnBase > 0 && st.grnBase !== newBase) {
            alert('Data of ' + (BASE_NAMES[st.grnBase] || '') + ' already exists. Please Reset the Form before loading data for ' + (BASE_NAMES[newBase] || '') + '.');
            return;
        }
        st.grnBase = newBase;
        if (st.rows.length > 0) {
            /* D2 — :1979 reads Cells["DemondId"], a column this grid does not have; the desktop's load
               stops with an error every time the grid already has rows. */
            alert('Column \'DemondId\' does not belong to the detail grid. Please Reset the Form before loading another GRN.');
            return;
        }
        $id('cmbsuppliername').value = String(party);
        supplierChanged();
        $id('cmbsuppliername').disabled = true;
        $id('txtRemarks').value = first.RemarksHeader || '';
        $id('txtBillDiscountAmountFooter').value = fmtHashes(first.PurchasePreBillBillDiscount);
        var existing = {};
        st.rows.forEach(function (r) { existing[C.intOf(r.InvGrnDetailId)] = 1; });
        dtLoader.forEach(function (row) {
            var detailId = C.intOf(row.DetailId);
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var qty = num(row.ItemQty), disc = num(row.PurchasePreBillItemDiscount), gross = num(row.ItemAmount);
            var itemAmount = gross - disc, taxPercent = num(row.PurchaseOrderTaxPercent);
            st.rows.push({
                Id: 0, InvGrnId: row.Id, InvGrnDetailId: detailId, InvGrnDocumentTypeId: row.DocumentTypeId, GrnNo: row.DocNo,
                PurchaseOrderDocumentTypeId: row.PurchaseOrderDocumentTypeId, PurchaseOrderId: row.PurchaseOrderHeaderId, PurchaseOrderNo: row.PurchaseOrderNo,
                PurchaseDemandDocumentTypeId: row.PurchaseDemandDocumentTypeId, PurchaseDemandId: row.PurchaseDemandHeaderId, PurchaseDemandNo: row.PurchaseDemandNo,
                PreBillDocumentTypeId: row.PurchasePreBillDocumentTypeId, PreBillId: row.PurchasePreBillId, PreBillNo: row.PurchasePreBillNo,
                DeliveryChallanDocumentTypeId: row.DeliveryChallanDocumentTypeId, DeliveryChallanId: row.DeliveryChallanId, DeliveryChallanNo: row.DeliveryChallanNo,
                ItemId: row.ItemId, ItemName: row.ItemName, ItemUomId: row.UomId, UOMCodeItem: row.Uom, ItemQty: qty,
                WarehouseId: row.WareHouseId, WareHouseName: row.WareHouseName, ItemConditionId: row.ItemConditionId, ItemCondition: row.ItemCondition,
                RackId: row.RackId, RackName: row.rackName, Rate: num(row.ItemRate), DiscountAmount: disc, ItemAmountWithoutDiscount: gross,
                ItemAmount: itemAmount, TaxNameId: row.PurchaseOrderTaxNameId, TaxName: row.PurchaseOrderTaxName, TaxPercent: taxPercent,
                TaxAmount: itemAmount * taxPercent / 100, BillAmount: 0, Freights: 0, ExpenseAmount: 0,
                GrnAttachments: row.NoOfAttachments, PoAttachments: row.NoOfAttachmentsPO, DemandAttachments: row.NoOfAttachmentsPD,
                PreBillAttachments: row.NoOfAttachmentsPreBill, DeliveryChallanAttachments: row.NoOfAttachmentsDC
            });
        });
        C.getJson(api + '/loader/extras' + C.qs({ grnIds: frt, challanIds: dc.join(',') })).then(function (x) {
            if (x.freight && x.freight.length) st.freight = x.freight;           // LoadFreightData (:2077)
            if (x.expenses && x.expenses.length) st.exp = x.expenses;            // LoadExpData (:2094)
            bindGrids();
            billAmount();
            freightProportion();
            billProportion();
            renderAll();
        }).catch(function (e) { bindGrids(); fail(e); });
    }

    /* ------------------------------------------------------------------ history */

    function histDefaults() {
        var days = C.intOf(look.defaultDaysToLessFromHistoryFromDate);
        $id('FromDateHistory').value = addDays(-(days > 0 ? days : 3));          // :495
        $id('ToDateHistory').value = today();
    }

    function bindHistoryBranches(branches) {
        hist.branches = branches || [];
        defaultBranch(hist, hist.branches, look.userBranchId, look.userBranchName);
        branchBox('histBranchBox', 'cmbBranchNameList', 'cmbBranchName', hist.branches, hist);
    }

    /** GetAll (:2309). */
    function showHistory() {
        var dateType = (document.querySelector('input[name=rdHist]:checked') || {}).value || 'doc';
        C.getJson(api + '/history' + C.qs({
            branchChosen: $id('cmbBranchName').value !== '', branchIds: branchIds(hist), dateType: dateType,
            fromChecked: $id('chkFromDateHistory').checked, fromDate: $id('FromDateHistory').value,
            toChecked: $id('chkToDateHistory').checked, toDate: $id('ToDateHistory').value,
            fromDocNo: $id('FromDocNoHistory').value, toDocNo: $id('ToDocNoHistory').value,
            supplierId: C.intOf($id('CmbCustomerHistory').value)
        })).then(function (rows) {
            hist.rows = rows || [];
            renderHistory();
        }).catch(fail);
    }

    var HIST_DATES = { DocDate: 1, DueDate: 1 };
    var HIST_TIMES = { EntryDate: 1, ModifyDate: 1, ApprovedDate: 1 };
    function historyColumns() {
        var cols = ['DocNo', 'DocDate'];
        if (!look.branchImplemented) cols.push('BranchSrNo', 'BranchName');       // HistoryGridSettings (:2458)
        return cols.concat(['DueDays', 'DueDate', 'ManualBillNo', 'SupplierName', 'ReferencePartyName', 'BillAmount', 'CurrencyName',
            'BillType', 'ApprovedStatus', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedUser', 'ApprovedDate',
            'RemarksHeader', 'NoOfAttachments']);
    }

    function renderHistory() {
        var t = $id('grdHistory');
        $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = ''; $id('grdDetail').tFoot.innerHTML = '';
        if (!hist.rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        var cols = historyColumns();
        t.tHead.innerHTML = '<tr><th>Print</th><th>Edit</th><th>Voucher</th>' + cols.map(function (k) { return '<th>' + esc(caption(k)) + '</th>'; }).join('') + '<th>Add Attachment</th></tr>';
        t.tBodies[0].innerHTML = hist.rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' +
                '<td><button type="button" class="cx-link" data-a="print">Print</button></td>' +
                '<td><button type="button" class="cx-link" data-a="edit">Edit</button></td>' +
                '<td><button type="button" class="cx-link" data-a="voucher">Voucher</button></td>' +
                cols.map(function (k) {
                    var v = r[k];
                    if (k === 'DocNo') return '<td class="num"><a href="#" data-a="edit">' + esc(v) + '</a></td>';
                    if (HIST_DATES[k]) v = C.gridDate(v);
                    if (HIST_TIMES[k]) v = C.gridDateTime(v, true);
                    return '<td' + (k === 'BillAmount' ? ' class="num"' : '') + '>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') +
                '<td><button type="button" class="cx-link" disabled title="Attachments are not ported">Add Attachment</button></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td colspan="3"></td>' + cols.map(function (k) { return '<td class="num">' + (k === 'BillAmount' ? esc(netStr(sum(hist.rows, 'BillAmount'))) : '') + '</td>'; }).join('') + '<td></td></tr>';
        t.tBodies[0].onclick = function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var r = hist.rows[parseInt(tr.getAttribute('data-i'), 10)];
            t.tBodies[0].querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-selected', x === tr); });
            var a = e.target.closest('[data-a]');
            if (a) {
                e.preventDefault();
                var act = a.getAttribute('data-a');
                if (act === 'print') openSlip(r.Id, C.intOf(r.DocumentTypeId));           // grdHistory_ColumnButtonClick (:2523)
                else if (act === 'voucher') voucher(r.Id);
                else if (act === 'edit') readById(r.Id);
                return;
            }
            historySelection(r);
        };
        t.tBodies[0].ondblclick = function (e) {                                          // grdHistory_DoubleClick (:2506)
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            readById(hist.rows[parseInt(tr.getAttribute('data-i'), 10)].Id);
        };
    }

    /** grdHistory_SelectionChanged (:2590) — GetByID and the same detail layout, by DocumentTypeSrNo. */
    function historySelection(r) {
        C.getJson(api + '/' + r.Id).then(function (o) {
            renderItems('grdDetail', o.rows || [], C.intOf(o.GrnBaseDocumentTypeId), false);
        }).catch(function () {
            $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = ''; $id('grdDetail').tFoot.innerHTML = '';
        });
    }

    /** btnNewHistory_Click (:2263). */
    function newHistory() {
        $id('FromDateHistory').value = today();
        $id('ToDateHistory').value = today();
        $id('FromDocNoHistory').value = '';
        $id('ToDocNoHistory').value = '';
        $id('CmbCustomerHistory').value = '0';
        hist.rows = [];
        renderHistory();
        $id('drdocdate').checked = true;
    }

    /** btnRefreshHistory_Click (:2249). */
    function refreshHistory() {
        C.getJson(api + '/history-refresh').then(function (r) {
            bindHistoryBranches(r.historyBranches);
            C.fillSelect('CmbCustomerHistory', r.historySuppliers, 'Id', 'Name');
        }).catch(fail);
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ init */

    function init() {
        st = blankState();
        wireSideGrids();
        document.addEventListener('click', function (e) {
            document.querySelectorAll('.pism-branches.is-open').forEach(function (b) { if (!b.contains(e.target)) b.classList.remove('is-open'); });
        });
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            applyRights();
            bindCombos();
            if (firstLoad) {                                                       // Form_Load only
                $id('DocDate').value = today();
                $id('txtdocno').value = l.docNo;
                $id('txtBranchSrNo').value = l.branchSrNo;
                $id('txtInvoiceTaxNo').value = l.taxNo;
                $id('txtDueDays').value = '0';
                dueDaysChanged();
                bindHistoryBranches(l.historyBranches);
                C.fillSelect('CmbCustomerHistory', l.historySuppliers, 'Id', 'Name');
                histDefaults();
                firstLoad = false;
            }
            bindGrids();
        }).catch(fail);
    }

    window.PiStoreMgmt = {
        reset: reset, refresh: refresh, save: save, update: update, remove: remove,
        openLoader: openLoader, closeLoader: closeLoader, loaderNew: loaderNew, loaderRefresh: loaderRefresh,
        loaderLoad: loaderLoad, loaderShow: loaderShow, toggleBranches: toggleBranches,
        printVoucher: printVoucher, printSlip: printSlip, supplierChanged: supplierChanged,
        dueDaysChanged: dueDaysChanged, docDateLeave: docDateLeave,
        itemDiscountFooterChanged: itemDiscountFooterChanged, billDiscountChanged: billDiscountChanged,
        showHistory: showHistory, newHistory: newHistory, refreshHistory: refreshHistory,
        tab: tab, open: readById
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
