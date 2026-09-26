/* ============================================================================================
 * Screen 961 "Store Purchase Pre Bill" — frmPurchasePreBill.cs, DocumentTypeId 147, with its
 * Load-Demand dialog frmPendingPurchaseDemand.cs (IsForPreBill = true, Purchase Demand type 141).
 *
 * The grid arithmetic is the form's own, event for event (line refs are frmPurchasePreBill.cs unless
 * marked "ldr:" for the loader). Math.Round on the desktop is banker's rounding (roundEven).
 * The server re-validates every row against the pending loader / the stored bill and builds the
 * document; the Doc No shown here is display only.
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon;
    var $id = C.$id, esc = C.esc, num = C.num;
    var api = '/api/store/purchase-pre-bill';

    var look = {};
    var st = null;             // form state (FromReset)
    var firstLoad = true;
    var hist = { rows: [], selectedId: 0 };
    var ldr = null;            // the loader dialog — a new one per click (btnPurchaseOrderLoader_Click :2266)
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    /* ------------------------------------------------------------------ numbers / text */

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
    /** x.ToString(clsGlobalVariables.stringFormatsingle) — "#,##0." + N zeros, rounded away from zero. */
    function fmtSingle(v) {
        var n = C.intOf(look.amountDecimals), x = num(v), f = Math.pow(10, n);
        var r = Math.sign(x) * Math.round(Math.abs(x) * f + 1e-9) / f;
        return r.toLocaleString('en-US', { minimumFractionDigits: n, maximumFractionDigits: n });
    }
    function qtyStr(v) {            // "#,##0.###"
        return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });
    }
    function sum(rows, key) { var t = 0; (rows || []).forEach(function (r) { t += num(r[key]); }); return t; }
    function pad(n) { return String(n).padStart(2, '0'); }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return C.today(); }
    function addDays(n) { var d = new Date(); d.setDate(d.getDate() + n); return isoOf(d); }
    /** "dd-MMM-yy" */
    function dMMMyy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) : String(v || '');
    }
    /** "dd-MMM-yy hh:mm tt" */
    function dMMMyyTime(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dMMMyy(v);
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) + ' ' + pad(h) + ':' + m[5] + ' ' + ap;
    }
    function fail(e) { alert(e && e.message ? e.message : e); }
    function rights() { return look.rights || {}; }

    /** A combo with the "-- Select --" row InfragisticsHelper.InsertDefaultRow puts at index 0. */
    function fillDefault(id, rows, valueKey, textKey, defaultText) {
        var el = $id(id);
        el.innerHTML = '<option value="0">' + esc(defaultText || '-- Select --') + '</option>' + (rows || []).map(function (r) {
            return '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>';
        }).join('');
    }
    /** A combo without a default row (BindSupplierName: insertDefaultRow false). */
    function fillPlain(id, rows, valueKey, textKey) {
        var el = $id(id);
        el.innerHTML = (rows || []).map(function (r) {
            return '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>';
        }).join('');
    }
    function has(id, v) { return Array.prototype.some.call($id(id).options, function (o) { return o.value === String(v); }); }
    /** combo.Value = v — a value not in the list leaves the combo without a selection. */
    function setVal(id, v) { var el = $id(id); el.value = String(v); if (el.value !== String(v)) el.selectedIndex = -1; }
    function clearCombo(id) { $id(id).selectedIndex = -1; }                   // combo.Text = string.Empty
    function comboInt(id) { return C.intOf($id(id).value); }

    /**
     * BindAndRetainSelection's RetainComboSelection with insertDefaultRow: keep the previous value
     * when the new list has it, otherwise activate row [activateIndex] of the bound rows (the
     * default row counts as row 0), otherwise leave the text empty.
     */
    function retain(id, prev, activate, activateIndex) {
        var el = $id(id);
        if (prev !== null && prev !== undefined && has(id, prev) && String(prev) !== '0') { el.value = String(prev); return; }
        if (activate && el.options.length > activateIndex) { el.selectedIndex = activateIndex; return; }
        el.selectedIndex = -1;
    }

    /* ------------------------------------------------------------------ state */

    function blankState() { return { id: 0, rows: [], exp: [], removed: [] }; }
    function expRow() { return { Id: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }      // AddRowInvExpGrid :808

    /** BindGrids (:626) — the expense grid always shows at least one row. */
    function bindGrids() {
        if (!st.exp.length) st.exp.push(expRow());
        renderAll();
    }

    /* ------------------------------------------------------------------ detail grid */

    /** DetailGridCommonSetting (helper) + grdDetailSetting (:735): visible columns and captions. */
    var DETAIL_COLS = [
        ['PurchaseDemandNo', 'Purchase Demand No'], ['ItemCode', 'Item Code'], ['ItemName', 'Item Name'], ['Uom', 'Uom'],
        ['ItemConditionId', 'Item Condition'], ['DemandQty', 'Approved Demand Qty'], ['PurchasedQty', 'Already Purchased Qty'],
        ['BalanceQty', 'Balance Demand Qty'], ['ThisQty', 'Purchased Qty'], ['Rate', 'Rate'],
        ['ItemAmountWithoutDiscount', 'Gross Amount'], ['DiscountAmount', 'Discount Amount'], ['ItemAmount', 'Item Amount'],
        ['ExpenseAmount', 'Expense Amount'], ['ItemNetAmount', 'Item Net Amount'], ['Remarks', 'Remarks']
    ];
    var QTY = { DemandQty: 1, PurchasedQty: 1, BalanceQty: 1, ThisQty: 1 };
    var AMT = { ItemAmountWithoutDiscount: 1, DiscountAmount: 1, ItemAmount: 1, ExpenseAmount: 1, ItemNetAmount: 1 };
    var EDIT = { ThisQty: 1, Rate: 1, DiscountAmount: 1, Remarks: 1 };                      // :739-757

    function cellText(k, v) {
        if (k === 'Rate') return v === null || v === undefined || v === '' ? '' : netStr(num(v));
        if (QTY[k]) return qtyStr(v);
        if (AMT[k]) return fmtSingle(v);
        return v === null || v === undefined ? '' : v;
    }
    function conditionName(id) {
        var c = (look.itemConditions || []).find(function (x) { return x.Id === C.intOf(id); });
        return c ? c.Description : '';
    }

    function renderDetail() {
        var t = $id('grd');
        t.tHead.innerHTML = '<tr><th>X</th>' + DETAIL_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = st.rows.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-x" data-a="del" data-i="' + i + '">X</button></td>' + DETAIL_COLS.map(function (c) {
                var k = c[0];
                if (k === 'ItemConditionId') {                                                           // GridComboBind :722
                    return '<td><select class="win-combo" data-i="' + i + '" data-k="ItemConditionId">' +
                        (!conditionName(r.ItemConditionId) ? '<option value="' + C.intOf(r.ItemConditionId) + '" selected>' + (C.intOf(r.ItemConditionId) ? esc(r.ItemCondition || r.ItemConditionId) : '') + '</option>' : '') +
                        (look.itemConditions || []).map(function (o) {
                            return '<option value="' + o.Id + '"' + (o.Id === C.intOf(r.ItemConditionId) ? ' selected' : '') + '>' + esc(o.Description) + '</option>';
                        }).join('') + '</select></td>';
                }
                if (EDIT[k]) {
                    var v = k === 'Remarks' ? (r.Remarks || '') : (r[k] === null || r[k] === undefined || r[k] === '' ? '' : netStr(num(r[k])));
                    return '<td' + (k === 'Remarks' ? '' : ' class="num"') + '><input type="text"' + (k === 'Remarks' ? '' : ' class="num"') +
                        ' data-i="' + i + '" data-k="' + k + '" value="' + esc(v) + '"></td>';
                }
                return '<td' + (QTY[k] || AMT[k] ? ' class="num"' : '') + '>' + esc(cellText(k, r[k])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        if (!st.rows.length) { t.tFoot.innerHTML = ''; return; }
        t.tFoot.innerHTML = '<tr><td></td>' + DETAIL_COLS.map(function (c) {
            var k = c[0];
            if (QTY[k]) return '<td class="num">' + esc(qtyStr(sum(st.rows, k))) + '</td>';
            if (AMT[k]) return '<td class="num">' + esc(fmtSingle(sum(st.rows, k))) + '</td>';
            return '<td></td>';
        }).join('') + '</tr>';
    }

    function wireDetail() {
        var t = $id('grd');
        t.onclick = function (e) {
            var b = e.target.closest('button[data-a="del"]'); if (!b) return;
            deleteDetailRow(parseInt(b.getAttribute('data-i'), 10));                                 // grd_ColumnButtonClick :655
        };
        t.onchange = function (e) {
            var el = e.target; if (!el.hasAttribute('data-k')) return;
            cellUpdated(parseInt(el.getAttribute('data-i'), 10), el.getAttribute('data-k'), el.value);
        };
    }

    /** grd_CellUpdated (:674). */
    function cellUpdated(i, key, value) {
        var row = st.rows[i]; if (!row) return;
        if (key === 'Remarks') row.Remarks = value;
        else if (key === 'ItemConditionId') row.ItemConditionId = C.intOf(value);
        else row[key] = num(value);
        if (key === 'ThisQty' || key === 'Rate' || key === 'DiscountAmount') recalculateRow(row);
        expProportion();
        calculateFooterAmounts();
        renderAll();
    }

    /** RecalculateRow (:702). */
    function recalculateRow(row) {
        var allowed = num(row.BalanceQty), thisQty = num(row.ThisQty), rate = num(row.Rate);
        if (thisQty > allowed) {
            thisQty = allowed;
            row.ThisQty = thisQty;
            alert("You can't add Qty more than Balance Qty");
        }
        var gross = thisQty * rate;
        row.ItemAmountWithoutDiscount = gross;
        var itemAmount = gross - num(row.DiscountAmount);
        row.ItemAmount = itemAmount;
        row.ItemNetAmount = itemAmount + num(row.ExpenseAmount);
    }

    /** DeleteDetailRow (:770). */
    function deleteDetailRow(i) {
        var r = st.rows[i]; if (!r) return;
        if (C.intOf(r.Id) !== 0) {
            if (!window.confirm('Are you sure to Delete?')) return;
            st.removed.push({ Id: C.intOf(r.Id), PurchaseDemandHeaderId: C.intOf(r.PurchaseDemandHeaderId),
                PurchaseDemandDetailId: C.intOf(r.PurchaseDemandDetailId) });                        // lstRemoveRecord, ActionTypeId 3
        }
        st.rows.splice(i, 1);
        expProportion();
        calculateFooterAmounts();
        renderAll();
    }

    /* ------------------------------------------------------------------ expense grid */

    /** grdInvExpSettings (:841): X, +, Other Item Name, Qty, Rate, Amount, Remarks. */
    function renderExp() {
        var t = $id('grdInvExp');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th><th>Other Item Name</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = st.exp.map(function (r, i) {
            var opts = '<option value="0"></option>' + (look.otherItems || []).map(function (o) {
                return '<option value="' + o.Id + '"' + (o.Id === C.intOf(r.ItemId) ? ' selected' : '') + '>' + esc(o.OtherItemName) + '</option>';
            }).join('');
            return '<tr><td><button type="button" class="cx-x" data-a="del" data-i="' + i + '">X</button></td>' +
                '<td><button type="button" class="cx-link" data-a="add" data-i="' + i + '">+</button></td>' +
                '<td><select class="win-combo" data-i="' + i + '" data-k="ItemId">' + opts + '</select></td>' +
                ['Qty', 'Rate', 'Amount'].map(function (k) {
                    return '<td class="num"><input type="text" class="num" data-i="' + i + '" data-k="' + k + '" value="' + esc(netStr(num(r[k]))) + '"></td>';
                }).join('') +
                '<td><input type="text" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks || '') + '"></td></tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td colspan="3"></td><td class="num">' + esc(qtyStr(sum(st.exp, 'Qty'))) + '</td><td></td><td class="num">' +
            esc(fmtSingle(sum(st.exp, 'Amount'))) + '</td><td></td></tr>';
    }

    function wireExp() {
        var t = $id('grdInvExp');
        t.onclick = function (e) {                                                                   // grdInvExp_ColumnButtonClick :877
            var b = e.target.closest('button[data-a]'); if (!b) return;
            var i = parseInt(b.getAttribute('data-i'), 10);
            if (b.getAttribute('data-a') === 'del') {                                              // DeleteRowInExpenseGrid :812
                st.exp.splice(i, 1);
                if (!st.exp.length) st.exp.push(expRow());
            } else {
                st.exp.push(expRow());
            }
            expProportion();
            calculateFooterAmounts();
            renderAll();
        };
        t.onchange = function (e) {                                                                  // grdInvExp_CellUpdated :904
            var el = e.target; if (!el.hasAttribute('data-k')) return;
            var row = st.exp[parseInt(el.getAttribute('data-i'), 10)], k = el.getAttribute('data-k');
            if (!row) return;
            if (k === 'ItemId') { row.ItemId = C.intOf(el.value); return; }
            if (k === 'Remarks') { row.Remarks = el.value; return; }
            row[k] = num(el.value);
            if (k === 'Qty' || k === 'Rate') row.Amount = num(row.Qty) * num(row.Rate);            // UpdateAmount :935
            else row.Rate = num(row.Qty) === 0 ? 0 : num(row.Amount) / num(row.Qty);                // UpdateRate :942
            expProportion();
            calculateFooterAmounts();
            renderAll();
        };
    }

    function renderAll() { renderDetail(); renderExp(); }

    /* ------------------------------------------------------------------ totals */

    /** ExpProportion (:1351). */
    function expProportion() {
        var totalExp = sum(st.exp, 'Amount'), totalQty = sum(st.rows, 'ThisQty');
        var hasExp = totalExp > 0 && totalQty > 0, per = hasExp ? totalExp / totalQty : 0;
        st.rows.forEach(function (r) { r.ExpenseAmount = hasExp ? per * num(r.ThisQty) : 0; });
        billProportion();
    }

    /** BillProportion (:1331). */
    function billProportion() {
        st.rows.forEach(function (r) { r.ItemNetAmount = num(r.ItemAmount) + num(r.ExpenseAmount); });
    }

    function itemDiscountFocused() { return document.activeElement === $id('txtItemDiscountAmountFooter'); }

    /**
     * CalculateFooterAmounts (:1305). Writing the Item Discount box raises its TextChanged
     * (ItemDiscountProportion + CalculateFooterAmounts again) whenever the text changes (D8).
     */
    function calculateFooterAmounts(depth) {
        depth = depth || 0;
        if (!itemDiscountFocused()) $id('txtItemAmountWithoutDiscountFooter').value = netStr(roundEven(sum(st.rows, 'ItemAmountWithoutDiscount')));
        setText('txtItemDiscountAmountFooter', netStr(roundEven(sum(st.rows, 'DiscountAmount'))), depth);
        var itemAmount = sum(st.rows, 'ItemAmount');
        $id('txtItemAmountFooter').value = fmtSingle(itemAmount);
        var expAmount = sum(st.exp, 'Amount');
        $id('textBox1').value = fmtSingle(expAmount);
        var footerDiscount = num($id('txtDiscountAmountFooter').value);
        $id('txtNetBillAmountFooter').value = fmtSingle(itemAmount + expAmount - footerDiscount);
        billProportion();
    }

    /** A programmatic Text assignment with the box's TextChanged handler, when the text changes. */
    function setText(id, v, depth) {
        var el = $id(id);
        if (el.value === v) return;
        el.value = v;
        if (depth > 4) return;
        if (id === 'txtItemDiscountAmountFooter') { itemDiscountProportion(); calculateFooterAmounts(depth + 1); }
        else if (id === 'txtDiscountAmountFooter') calculateFooterAmounts(depth + 1);
    }

    /** ItemDiscountProportion (:1380) — only while the footer box has the focus. */
    function itemDiscountProportion() {
        if (!itemDiscountFocused()) return;
        var total = num($id('txtItemDiscountAmountFooter').value), qty = sum(st.rows, 'ThisQty');
        var hasD = total > 0 && qty > 0, per = hasD ? total / qty : 0;
        st.rows.forEach(function (r) { r.DiscountAmount = hasD ? per * num(r.ThisQty) : 0; recalculateRow(r); });
    }

    /** txtItemDiscountAmountFooter_TextChanged (:1408). */
    function itemDiscountFooterChanged() { itemDiscountProportion(); calculateFooterAmounts(); renderDetail(); }
    /** txtDiscountAmountFooter_TextChanged (:1375). */
    function billDiscountChanged() { calculateFooterAmounts(); renderDetail(); }

    /* ------------------------------------------------------------------ combos */

    function applyRights() {
        var r = rights();
        $id('btnsave').disabled = !r.save;                                                        // :392-395
        $id('btnupdate').disabled = !r.update;
        $id('btnDelete').disabled = !r.delete;
        $id('btnprint').disabled = !r.print;
        var n = $id('rightsNote');
        if (!r.view) { n.textContent = 'You do not have the View right for this screen.'; n.classList.remove('is-hidden'); }
    }

    /** BindSupplierName / ReferancePartyBind / DeliveryTerm / CityDtFillFromGlobalAndBind, retaining values. */
    function bindCombos(isLoad) {
        var bill = $id('CmbBillToParty').value, vend = $id('CmbVendorSupplier').value;
        fillPlain('CmbBillToParty', look.suppliers, 'Id', 'CompanyName');
        fillPlain('CmbVendorSupplier', look.suppliers, 'Id', 'CompanyName');
        if (bill && has('CmbBillToParty', bill)) $id('CmbBillToParty').value = bill; else clearCombo('CmbBillToParty');
        if (vend && has('CmbVendorSupplier', vend)) $id('CmbVendorSupplier').value = vend; else clearCombo('CmbVendorSupplier');
        var ref = isLoad ? 0 : comboInt('CmbRefParty');
        fillDefault('CmbRefParty', look.referenceParties, 'Id', 'ReferencePartyName');
        if ((look.referenceParties || []).length) retain('CmbRefParty', ref, true, 1); else clearCombo('CmbRefParty');   // :555 (D7)
        var term = isLoad ? 0 : comboInt('CmbDeliveryTerm');
        fillDefault('CmbDeliveryTerm', look.deliveryTerms, 'Id', 'Description');
        if ((look.deliveryTerms || []).length) retain('CmbDeliveryTerm', term, true, 2); else clearCombo('CmbDeliveryTerm'); // :567 (D7)
        var city = isLoad ? 0 : comboInt('CmbCityName');
        fillDefault('CmbCityName', look.cities, 'Id', 'Description');
        if ((look.cities || []).length) retain('CmbCityName', city, true, 0); else clearCombo('CmbCityName');
    }

    /** GetConfigurationsFromGlobalAndBindValuesInColumns (:431) — City Area, only when > 0. */
    function applyCityArea(cityArea) {
        if (C.intOf(cityArea) > 0) setVal('CmbCityName', C.intOf(cityArea));
    }

    /** HistoryComboBind (:1453) — nothing is rebound when the procedure returns no rows. */
    function bindHistoryCombos(parties, items) {
        if (!(parties || []).length && !(items || []).length) return;                                 // :1461 — nothing rebound
        bindHistoryCombo('CmbBillToPartyHistory', parties);
        bindHistoryCombo('CmbItemHistory', items);
    }
    /** BindAndRetainSelection(insertDefaultRow, ActivateRow false): an empty list clears the combo
        (no "-- Select --" row); otherwise the previous value is kept, else the default row. */
    function bindHistoryCombo(id, rows) {
        var el = $id(id), prev = comboInt(id);
        if (!(rows || []).length) { el.innerHTML = ''; el.selectedIndex = -1; return; }
        fillDefault(id, rows, 'Id', 'Name');
        el.value = has(id, prev) ? String(prev) : '0';
    }

    /** Datetypefill → InfragisticsHelper.BindComboDateType (default row "Select..."). */
    function dateTypeFill() {
        fillDefault('cmbDateTypeHistory', [{ Id: 1, P: 'This Day' }, { Id: 2, P: 'This Week' }, { Id: 3, P: 'This Month' },
            { Id: 4, P: 'This Year' }, { Id: 5, P: 'Financial Year' }], 'Id', 'P', 'Select...');
        $id('cmbDateTypeHistory').value = '0';
    }

    /* ------------------------------------------------------------------ reset / open */

    /** FromReset (:1228) — Doc Date, Vendor Bill Date, Delivery Term, City (unless City Area) and Print Preview kept. */
    function reset() {
        st.removed = [];
        st.id = 0;
        $id('btnsave').classList.remove('is-hidden');
        $id('btnupdate').classList.add('is-hidden');
        $id('btnDelete').classList.add('is-hidden');
        clearCombo('CmbBillToParty');
        clearCombo('CmbVendorSupplier');
        clearCombo('CmbRefParty');
        $id('txtVendorBillNo').value = '';
        $id('txtVehicleNo').value = '';
        $id('txtBiltyNo').value = '';
        $id('txtRemarksMain').value = '';
        /* the footer boxes are emptied one by one, BEFORE the grid is cleared — the two with a
           TextChanged handler recompute every footer from the rows still in the grid (:1250-1255) */
        $id('txtItemAmountWithoutDiscountFooter').value = '';
        setText('txtItemDiscountAmountFooter', '', 0);
        $id('txtItemAmountFooter').value = '';
        $id('textBox1').value = '';
        setText('txtDiscountAmountFooter', '', 0);
        $id('txtNetBillAmountFooter').value = '';
        st.rows = [];
        st.exp = [];
        bindGrids();
        return C.getJson(api + '/numbers').then(function (n) {
            $id('txtDocNo').value = n.docNo;                                                       // DocumentNoDbCall
            look.defaultDaysToLessFromHistoryFromDate = n.defaultDaysToLessFromHistoryFromDate;
            applyCityArea(n.cityArea);
        }).catch(fail);
    }

    /** ReadById (:1136). */
    function readById(id) {
        return reset().then(function () {
            return C.getJson(api + '/' + id + C.qs({ edit: true }));
        }).then(function (o) {
            if (!o) return;
            st.id = o.Id;
            tab('tabForm');
            $id('btnsave').classList.add('is-hidden');
            $id('btnupdate').classList.remove('is-hidden');
            $id('btnDelete').classList.remove('is-hidden');
            $id('txtDocDate').value = C.isoDay(o.DocDate);
            $id('txtDocNo').value = o.DocNo;
            setVal('CmbBillToParty', o.BillToPartyId);
            if (o.VendorSupplierId !== undefined) {                                                  // first detail row (:1157)
                setVal('CmbVendorSupplier', o.VendorSupplierId);
                setVal('CmbRefParty', o.ReferencePartyId);
                if (C.isoDay(o.VendorBillDate)) $id('txtVendorBillDate').value = C.isoDay(o.VendorBillDate);
                $id('txtVendorBillNo').value = o.VendorBillNo || '';
                $id('txtVehicleNo').value = o.VehicleNo || '';
                $id('txtBiltyNo').value = o.BiltyNo || '';
            }
            setVal('CmbDeliveryTerm', o.DeliveryTermId);
            setVal('CmbCityName', o.CityId);
            setText('txtDiscountAmountFooter', fmtSingle(o.DiscountAmount), 0);
            $id('txtRemarksMain').value = o.RemarksHeader || '';
            st.rows = o.rows || [];
            st.exp = o.expenses || [];
            bindGrids();
            calculateFooterAmounts();
            renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ save / delete / print */

    function payload(confirmed) {
        return {
            confirmed: confirmed,
            Id: st.id,
            DocDate: $id('txtDocDate').value,
            BillToPartyId: comboInt('CmbBillToParty'),
            VendorSupplierId: comboInt('CmbVendorSupplier'),
            ReferencePartyId: comboInt('CmbRefParty'),
            DeliveryTermId: comboInt('CmbDeliveryTerm'),
            CityId: comboInt('CmbCityName'),
            VendorBillDate: $id('txtVendorBillDate').value,
            VendorBillNo: $id('txtVendorBillNo').value,
            VehicleNo: $id('txtVehicleNo').value,
            BiltyNo: $id('txtBiltyNo').value,
            RemarksHeader: $id('txtRemarksMain').value,
            DiscountAmount: $id('txtDiscountAmountFooter').value,
            rows: st.rows.map(function (r) {
                var o = {};
                ['Id', 'PurchaseDemandHeaderId', 'PurchaseDemandDetailId', 'PurchaseDemandNo', 'ItemId', 'UomId', 'ItemConditionId']
                    .forEach(function (k) { o[k] = C.intOf(r[k]); });
                ['DemandQty', 'PurchasedQty', 'BalanceQty', 'ThisQty', 'Rate', 'ItemAmountWithoutDiscount', 'DiscountAmount',
                    'ItemAmount', 'ExpenseAmount', 'ItemNetAmount'].forEach(function (k) { o[k] = num(r[k]); });
                o.Remarks = r.Remarks === null || r.Remarks === undefined ? '' : String(r.Remarks);
                return o;
            }),
            removed: st.id > 0 ? st.removed.map(function (r) {
                return { Id: C.intOf(r.Id), PurchaseDemandHeaderId: C.intOf(r.PurchaseDemandHeaderId), PurchaseDemandDetailId: C.intOf(r.PurchaseDemandDetailId) };
            }) : [],
            expenses: st.exp.map(function (e) {
                return { Id: C.intOf(e.Id), ItemId: C.intOf(e.ItemId), Qty: num(e.Qty), Rate: num(e.Rate), Amount: num(e.Amount), Remarks: e.Remarks || '' };
            })
        };
    }

    /** Insert (:1007) — the checks before the prompt run on the server first (W6). */
    function insert() {
        if (!st.rows.length) { alert('Detail Record Not Found'); return; }                       // :1017
        calculateFooterAmounts();                                                                    // :1021
        C.postJson(api + '/save', payload(false)).then(function (r) {
            if (!r || !r.confirm) return;
            if (!window.confirm(r.confirm + '?')) return;
            return C.postJson(api + '/save', payload(true)).then(function (res) {
                alert(res.message);
                var id = res.id;
                return reset().then(function () {
                    if ($id('ChkBox').checked && rights().print) showPrint(id);                     // :1125
                });
            });
        }).catch(fail);
    }
    function save() { st.id = 0; insert(); }                                                       // btnsave_Click :953
    function update() {                                                                              // btnupdate_Click :967
        if (!(st.id > 0)) { alert('Record Not Update because RecId Not Found'); return; }
        insert();
    }

    /** btnDelete_Click (:983). */
    function remove() {
        if (!(st.id > 0)) { alert('No record found to Delete'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + st.id + '/delete', {}).then(function (r) { alert(r.message); return reset(); }).catch(fail);
    }

    /**
     * ShowPrint → StorePurchasePreBillSlip147: the report procedure's rows and its expense
     * sub-report, as tables (the Crystal layout 147_PurchasePreBillSlip.rpt is not reproduced).
     */
    function showPrint(id) {
        if (!(id > 0)) { alert('PrintId not found...'); return; }
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        Promise.all([C.getJson(api + '/' + id + '/slip'), C.getJson(api + '/' + id + '/slip-sub')]).then(function (res) {
            function table(rows) {
                if (!rows || !rows.length) return '<p>No rows</p>';
                var cols = Object.keys(rows[0]);
                return '<table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr></thead><tbody>' +
                    rows.map(function (r) { return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>'; }).join('') +
                    '</tbody></table>';
            }
            w.document.write('<html><head><title>147-Purchase Pre Bill Slip</title><style>body{font-family:Verdana;font-size:10px}' +
                'table{border-collapse:collapse;margin-bottom:12px}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
                '<h3>147-Purchase Pre Bill Slip</h3>' + table(res[0]) + '<h4>Expenses</h4>' + table(res[1]) +
                '<script>window.print()<\/script></body></html>');
            w.document.close();
        }).catch(function (e) { w.close(); fail(e); });
    }
    function print() { showPrint(st.id); }                                                          // btnprint_Click :1919

    /** btnRefresh_Click (:1270) — the lists rebound, selections kept; City Area applied. */
    function refresh() {
        C.getJson(api + '/refresh').then(function (r) {
            Object.keys(r).forEach(function (k) { look[k] = r[k]; });
            bindCombos(false);
            applyCityArea(r.cityArea);
            renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load Demand (frmPendingPurchaseDemand) */

    var LDR_COLS = [['DocNo', 'Doc No'], ['DocDate', 'Doc Date'], ['ParentCategory', 'Parent Category'], ['Status', 'Status'],
        ['RemarksHeader', 'Remarks Header'], ['EntryDate', 'Entry Date'], ['EntryUserName', 'Entry User Name'],
        ['ModifyDate', 'Modify Date'], ['ModifyUserName', 'Modify User Name'], ['IsApproved', 'Is Approved'],
        ['ApprovedDate', 'Approved Date'], ['ApprovalUserName', 'Approval User Name']];          // grdSettings ldr:271 (IsForPreBill)
    var LDR_DETAIL_COLS = [['DepartmentName', 'Department Name'], ['RequestBy', 'Request By'], ['ItemName', 'Item Name'],
        ['ItemCode', 'Item Code'], ['Uom', 'Uom'], ['ItemCondition', 'Item Condition'], ['JobLot', 'Job Lot'],
        ['RequiredQty', 'Required Qty'], ['ApprovedQty', 'Approved Qty'], ['QtyUsedInPreBill', 'Qty Used In Pre Bill'],
        ['QtyUsedInGrnDirectly', 'Qty Used In Grn Directly'], ['BalanceQty', 'Balance Qty'], ['RemarksDetail', 'Remarks Detail']];  // GridDetailSetting ldr:432

    /** btnPurchaseOrderLoader_Click (:2262) — a NEW dialog per click, so every filter starts fresh. */
    function openLoader() {
        ldr = { rows: [], checked: {}, detail: [], detailChecked: {} };
        $id('ldrFromDate').value = addDays(-7);                                                     // ldr:131 / ldr:206
        $id('ldrToDate').value = today();
        $id('txtDocNoFrom').value = '';
        $id('txtDocNoTo').value = '';
        $id('CmbItemName').innerHTML = '';
        renderLoader();
        renderLoaderDetail();
        C.openModal('dlgDemand');
        var dlg = ldr;
        C.getJson(api + '/loader/lookups').then(function (l) {
            if (ldr !== dlg) return;                                                                 // closed / reopened meanwhile
            dlg.fyStart = l.financialYearStart;
            loaderItems(l.items);
            return pendingCall();
        }).catch(function (e) { if (ldr === dlg) fail(e); });
    }

    /** CombosFill (ldr:152) — only bound when the procedure returns rows. */
    function loaderItems(items) {
        if (!(items || []).length) return;
        var prev = comboInt('CmbItemName');
        fillDefault('CmbItemName', items, 'Id', 'Name');
        $id('CmbItemName').value = has('CmbItemName', prev) ? String(prev) : '0';
    }

    /** PendingDataDbCall + GrdDataBind (ldr:195 / ldr:219). */
    function pendingCall() {
        var dlg = ldr;
        if (!dlg) return Promise.resolve();
        return C.getJson(api + '/loader/pending' + C.qs({
            fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value,
            docNoFrom: $id('txtDocNoFrom').value, docNoTo: $id('txtDocNoTo').value,
            itemId: comboInt('CmbItemName')
        })).then(function (rows) {
            if (ldr !== dlg) return;                                                                 // a late answer for a closed dialog
            dlg.rows = rows || [];
            dlg.checked = {};
            renderLoader();
        }, function (e) { if (ldr === dlg) throw e; });
    }

    function renderLoader() {
        var t = $id('ldrGrd'), seen = {}, heads = [];
        if (!ldr) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        (ldr.rows || []).forEach(function (r) { var id = C.intOf(r.Id); if (!seen[id]) { seen[id] = 1; heads.push(r); } });   // ldr:247
        if (!heads.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th><input type="checkbox" id="ldrGrdAll" title="Select all"' +
            (heads.every(function (r) { return ldr.checked[C.intOf(r.Id)]; }) ? ' checked' : '') + '></th>' + LDR_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = heads.map(function (r) {
            var id = C.intOf(r.Id);
            return '<tr><td><input type="checkbox" data-h="' + id + '"' + (ldr.checked[id] ? ' checked' : '') + '></td>' + LDR_COLS.map(function (c) {
                var k = c[0], v = r[k];
                if (k === 'DocNo') return '<td class="num"><a href="#" data-slip="' + id + '">' + esc(v) + '</a></td>';
                if (k === 'IsApproved') return '<td><input type="checkbox" disabled' + (v === true || v === 1 || v === '1' ? ' checked' : '') + '></td>';
                if (k === 'DocDate') v = C.gridDate(v);
                if (k === 'EntryDate' || k === 'ModifyDate' || k === 'ApprovedDate') v = C.gridDate(v);
                return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].onchange = function (e) {                                                      // grd_RowCheckStateChanged ldr:353
            var cb = e.target; if (!cb.hasAttribute('data-h')) return;
            var id = C.intOf(cb.getAttribute('data-h'));
            if (cb.checked) ldr.checked[id] = 1; else delete ldr.checked[id];
            $id('ldrGrdAll').checked = heads.every(function (r) { return ldr.checked[C.intOf(r.Id)]; });
            detailBind();
        };
        $id('ldrGrdAll').onchange = function () {
            var on = this.checked;
            heads.forEach(function (r) { var id = C.intOf(r.Id); if (on) ldr.checked[id] = 1; else delete ldr.checked[id]; });
            t.tBodies[0].querySelectorAll('input[data-h]').forEach(function (cb) { cb.checked = on; });
            detailBind();
        };
        t.tBodies[0].onclick = function (e) {                                                       // grd_LinkClicked ldr:308 → PurchaseDemandSlip454
            var a = e.target.closest('a[data-slip]'); if (!a) return;
            e.preventDefault();
            C.printSlip('/api/store/purchase-demand/' + C.intOf(a.getAttribute('data-slip')) + '/slip', '454-Purchase Demand Slip');
        };
    }

    /** DetailGridBind (ldr:374) — every row of the checked documents, all rows checked (CheckAllRows). */
    function detailBind() {
        if (!ldr) return;
        var ids = ldr.checked;
        if (!Object.keys(ids).length) { ldr.detail = []; ldr.detailChecked = {}; renderLoaderDetail(); return; }
        ldr.detail = (ldr.rows || []).filter(function (r) { return ids[C.intOf(r.Id)]; });
        ldr.detailChecked = {};
        ldr.detail.forEach(function (r, i) { ldr.detailChecked[i] = 1; });
        renderLoaderDetail();
    }

    function renderLoaderDetail() {
        var t = $id('ldrGrdDetail');
        if (!ldr || !ldr.detail.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        var all = ldr.detail.every(function (r, i) { return ldr.detailChecked[i]; });
        t.tHead.innerHTML = '<tr><th><input type="checkbox" id="ldrDetailAll" title="Select all"' + (all ? ' checked' : '') + '></th>' + LDR_DETAIL_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = ldr.detail.map(function (r, i) {
            return '<tr><td><input type="checkbox" data-d="' + i + '"' + (ldr.detailChecked[i] ? ' checked' : '') + '></td>' + LDR_DETAIL_COLS.map(function (c) {
                var k = c[0], v = r[k], n = /Qty$/.test(k);
                return '<td' + (n ? ' class="num"' : '') + '>' + esc(n ? qtyStr(v) : (v === null || v === undefined ? '' : v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].onchange = function (e) {
            var cb = e.target; if (!cb.hasAttribute('data-d')) return;
            var i = C.intOf(cb.getAttribute('data-d'));
            if (cb.checked) ldr.detailChecked[i] = 1; else delete ldr.detailChecked[i];
            $id('ldrDetailAll').checked = ldr.detail.every(function (r, j) { return ldr.detailChecked[j]; });
        };
        $id('ldrDetailAll').onchange = function () {
            var on = this.checked;
            ldr.detail.forEach(function (r, i) { if (on) ldr.detailChecked[i] = 1; else delete ldr.detailChecked[i]; });
            t.tBodies[0].querySelectorAll('input[data-d]').forEach(function (cb) { cb.checked = on; });
        };
    }

    /** btngrnlod_Click (ldr:182). */
    function loaderShow() { if (ldr) pendingCall().catch(fail); }

    /** btnReset_Click (ldr:461) — From Date = financial year start. */
    function loaderNew() {
        if (!ldr) return;
        $id('ldrFromDate').value = ldr.fyStart ? ldr.fyStart : $id('ldrFromDate').value;
        $id('txtDocNoFrom').value = '';
        $id('txtDocNoTo').value = '';
        if ($id('CmbItemName').options.length) clearCombo('CmbItemName');
        pendingCall().catch(fail);
    }

    /** btnRefresh_Click (ldr:479). */
    function loaderRefresh() {
        var dlg = ldr;
        if (!dlg) return;
        C.getJson(api + '/loader/lookups').then(function (l) { if (ldr === dlg) loaderItems(l.items); })
            .catch(function (e) { if (ldr === dlg) fail(e); });
    }

    function closeLoader() { C.closeModal('dlgDemand'); ldr = null; }                             // Esc / Ctrl+E: dtLoader = null

    /** btnLoadOnInvoice_Click_1 (ldr:491) — IsForPreBill: no gate-pass checks. */
    function loaderLoad() {
        if (!ldr) return;
        var picked = ldr.detail.filter(function (r, i) { return ldr.detailChecked[i]; });
        if (!picked.length) { alert('Check the row first in Detail Grid'); return; }
        var detailIds = {};
        picked.forEach(function (r) { detailIds[C.intOf(r.DetailId)] = 1; });
        var dtLoader = (ldr.rows || []).filter(function (r) { return detailIds[C.intOf(r.DetailId)]; });
        C.closeModal('dlgDemand');
        ldr = null;
        loadInGridDetail(dtLoader);
    }

    /** LoadInGridDetail (:2278). */
    function loadInGridDetail(dtLoader) {
        if (!dtLoader || !dtLoader.length) return;
        var existing = {};
        st.rows.forEach(function (r) { existing[C.intOf(r.PurchaseDemandDetailId)] = 1; });
        dtLoader.forEach(function (row) {
            var detailId = C.intOf(row.DetailId);
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var balance = num(row.BalanceQty);
            st.rows.push({
                Id: 0, PurchaseDemandHeaderId: C.intOf(row.Id), PurchaseDemandDetailId: detailId, PurchaseDemandNo: row.DocNo,
                ItemId: C.intOf(row.ItemId), ItemCode: row.ItemCode, ItemName: row.ItemName, UomId: C.intOf(row.ItemSchuomId),
                Uom: row.Uom, UomEquivalent: num(row.UomEquivalent), ItemConditionId: C.intOf(row.ItemConditionId), ItemCondition: row.ItemCondition,
                DemandQty: num(row.ApprovedQty), PurchasedQty: num(row.QtyUsedInPreBill) + num(row.QtyUsedInGrnDirectly),
                BalanceQty: balance, ThisQty: balance, Rate: null, ItemAmountWithoutDiscount: 0, DiscountAmount: 0,
                ItemAmount: 0, ExpenseAmount: 0, ItemNetAmount: 0, Remarks: row.RemarksDetail || ''
            });
        });
        bindGrids();
        calculateFooterAmounts();
        renderAll();
    }

    /* ------------------------------------------------------------------ history */

    /** Form_Load (:421) / btnResetHistory_Click (:1539). */
    function histDates(days) {
        $id('FromDateHistory').value = addDays(-(days > 0 ? days : 3));
        $id('ToDateHistory').value = today();
    }

    /** cmbDateTypeHistory_ValueChanged (:1497). */
    function dateTypeChanged() {
        var v = comboInt('cmbDateTypeHistory'), d = new Date();
        if (v === 1) $id('FromDateHistory').value = today();
        else if (v === 2) $id('FromDateHistory').value = addDays(-7);
        else if (v === 3) { $id('FromDateHistory').value = isoOf(new Date(d.getFullYear(), d.getMonth(), 1)); $id('ToDateHistory').value = today(); }
        else if (v === 4) { $id('FromDateHistory').value = isoOf(new Date(d.getFullYear(), 0, 1)); $id('ToDateHistory').value = today(); }
        else if (v === 5 && look.financialYearStart) $id('FromDateHistory').value = look.financialYearStart;
    }

    /** HistoryGridFill (:1574). */
    function showHistory() {
        var dateType = (document.querySelector('input[name=rdHist]:checked') || {}).value || 'doc';
        C.getJson(api + '/history' + C.qs({
            dateType: dateType,
            fromChecked: $id('chkFromDateHistory').checked, fromDate: $id('FromDateHistory').value,
            toChecked: $id('chkToDateHistory').checked, toDate: $id('ToDateHistory').value,
            fromDocNo: $id('FromDocNoHistory').value, toDocNo: $id('ToDocNoHistory').value,
            billToPartyId: comboInt('CmbBillToPartyHistory'), itemId: comboInt('CmbItemHistory')
        })).then(function (rows) {
            hist.rows = rows || [];
            renderHistory();
        }).catch(fail);
    }

    /** HistoryGridSetting (:1716): Id, DocumentTypeId, BillToPartyId, DeliveryTermId, CityId hidden; Edit, Print before DocNo. */
    var HIST_COLS = [['DocNo', 'Doc No'], ['DocDate', 'Doc Date'], ['BillToPartyName', 'Bill To Party Name'], ['ReferenceNo', 'Reference No'],
        ['DiscountAmount', 'Discount Amount'], ['BillAmount', 'Bill Amount'], ['DeliveryTerm', 'Delivery Term'], ['CityName', 'City Name'],
        ['RemarksHeader', 'Remarks Header'], ['EntryDate', 'Entry Date'], ['EntryUserName', 'Entry User Name'], ['ModifyDate', 'Modify Date'],
        ['ModifyUserName', 'Modify User Name'], ['IsApproved', 'Is Approved'], ['ApprovedDate', 'Approved Date'],
        ['ApprovalUserName', 'Approval User Name'], ['NoOfAttachments', 'No Of Attachments']];

    function clearHistoryDetail() {
        var d = $id('grdhistoryDetail');
        d.tHead.innerHTML = ''; d.tBodies[0].innerHTML = ''; d.tFoot.innerHTML = '';
    }

    function renderHistory() {
        var t = $id('grdhistory');
        clearHistoryDetail();
        if (!hist.rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + HIST_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = hist.rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' +
                '<td><button type="button" class="cx-link" data-a="edit">Edit</button></td>' +               // :1739
                '<td><button type="button" class="cx-link" data-a="print">Print</button></td>' +             // :1740
                HIST_COLS.map(function (c) {
                    var k = c[0], v = r[k];
                    if (k === 'DocNo') return '<td class="num"><a href="#" data-a="edit">' + esc(v) + '</a></td>';
                    if (k === 'IsApproved') return '<td><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';
                    if (k === 'NoOfAttachments') return '<td class="num"><span title="Attachments are not ported">' + esc(v) + '</span></td>';
                    if (k === 'DiscountAmount' || k === 'BillAmount') return '<td class="num">' + esc(fmtSingle(v)) + '</td>';
                    if (k === 'DocDate') v = dMMMyy(v);
                    if (k === 'EntryDate' || k === 'ModifyDate' || k === 'ApprovedDate') v = v ? dMMMyyTime(v) : '';
                    return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr><td></td><td></td>' + HIST_COLS.map(function (c) {
            var k = c[0];
            if (k === 'DiscountAmount' || k === 'BillAmount') return '<td class="num">' + esc(fmtSingle(sum(hist.rows, k))) + '</td>';
            return '<td></td>';
        }).join('') + '</tr>';
        t.tBodies[0].onclick = function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var r = hist.rows[parseInt(tr.getAttribute('data-i'), 10)];
            t.tBodies[0].querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-selected', x === tr); });
            var a = e.target.closest('[data-a]');
            if (a) {                                                                                  // grdhistory_ColumnButtonClick :1771
                e.preventDefault();
                if (a.getAttribute('data-a') === 'print') {
                    if (!rights().print) { alert("you don't have print rights..."); return; }
                    showPrint(r.Id);
                } else {
                    if (!rights().update) { alert("you don't have update rights..."); return; }
                    readById(r.Id);
                }
                return;
            }
            historySelection(r);
        };
        t.tBodies[0].ondblclick = function (e) {                                                     // grdhistory_DoubleClick :1750
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            if (!rights().update) { alert("ypu don't have updae rights..."); return; }
            readById(hist.rows[parseInt(tr.getAttribute('data-i'), 10)].Id);
        };
    }

    /** grddetailhistorySettings (:1861): PurchasedQty, BalanceQty, ItemConditionId hidden; ItemCondition in its place. */
    var HIST_DETAIL_COLS = [['PurchaseDemandNo', 'Purchase Demand No'], ['ItemCode', 'Item Code'], ['ItemName', 'Item Name'], ['Uom', 'Uom'],
        ['ItemCondition', 'Item Condition'], ['DemandQty', 'Approved Demand Qty'], ['ThisQty', 'Purchased Qty'], ['Rate', 'Rate'],
        ['ItemAmountWithoutDiscount', 'Gross Amount'], ['DiscountAmount', 'Discount Amount'], ['ItemAmount', 'Item Amount'],
        ['ExpenseAmount', 'Expense Amount'], ['ItemNetAmount', 'Item Net Amount'], ['Remarks', 'Remarks']];

    /** grdhistory_SelectionChanged (:1821) → GetDetailGrdByHeadId (:1838). */
    function historySelection(r) {
        if (hist.selectedId === r.Id && $id('grdhistoryDetail').tBodies[0].innerHTML) return;
        hist.selectedId = r.Id;
        C.getJson(api + '/' + r.Id).then(function (o) {
            var rows = o.rows || [], t = $id('grdhistoryDetail');
            if (!rows.length) { clearHistoryDetail(); return; }
            t.tHead.innerHTML = '<tr>' + HIST_DETAIL_COLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (d) {
                return '<tr>' + HIST_DETAIL_COLS.map(function (c) {
                    var k = c[0];
                    return '<td' + (QTY[k] || AMT[k] || k === 'Rate' ? ' class="num"' : '') + '>' + esc(cellText(k, d[k])) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr>' + HIST_DETAIL_COLS.map(function (c) {
                var k = c[0];
                if (QTY[k]) return '<td class="num">' + esc(qtyStr(sum(rows, k))) + '</td>';
                if (AMT[k]) return '<td class="num">' + esc(fmtSingle(sum(rows, k))) + '</td>';
                return '<td></td>';
            }).join('') + '</tr>';
        }).catch(function () { clearHistoryDetail(); });
    }

    /** btnResetHistory_Click (:1534) — From Date goes back to Now-3 (D10). */
    function resetHistory() {
        $id('cmbDateTypeHistory').selectedIndex = -1;
        $id('FromDateHistory').value = addDays(-3);
        $id('ToDateHistory').value = today();
        $id('FromDocNoHistory').value = '';
        $id('ToDocNoHistory').value = '';
        clearCombo('CmbBillToPartyHistory');
        clearCombo('CmbItemHistory');
        hist.rows = [];
        renderHistory();
        $id('drdocdate').checked = true;
    }

    /** btnRefreshHistory_Click (:1556). */
    function refreshHistory() {
        dateTypeFill();
        C.getJson(api + '/history-refresh').then(function (r) {
            bindHistoryCombos(r.historyBillToParties, r.historyItems);
        }).catch(fail);
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ init */

    function init() {
        st = blankState();
        wireDetail();
        wireExp();
        $id('txtDocDate').value = today();                                                          // designer default Now
        $id('txtVendorBillDate').value = today();
        bindGrids();
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            applyRights();
            bindCombos(true);
            if (firstLoad) {                                                                         // InitializeComponentMethod only
                $id('txtDocNo').value = l.docNo;
                bindHistoryCombos(l.historyBillToParties, l.historyItems);
                dateTypeFill();
                histDates(C.intOf(l.defaultDaysToLessFromHistoryFromDate));
                firstLoad = false;
            }
            bindGrids();
        }).catch(fail);
    }

    window.PreBill = {
        reset: reset, refresh: refresh, save: save, update: update, remove: remove, print: print,
        openLoader: openLoader, closeLoader: closeLoader, loaderNew: loaderNew, loaderRefresh: loaderRefresh,
        loaderLoad: loaderLoad, loaderShow: loaderShow,
        itemDiscountFooterChanged: itemDiscountFooterChanged, billDiscountChanged: billDiscountChanged,
        showHistory: showHistory, resetHistory: resetHistory, refreshHistory: refreshHistory,
        dateTypeChanged: dateTypeChanged, tab: tab, open: readById
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
