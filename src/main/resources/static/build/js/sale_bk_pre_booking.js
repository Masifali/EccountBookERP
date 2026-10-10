/* ============================================================================================
 * Screen 761 PreBookingOrder - Architecture.WinApp.PreBookingAndDelivery.PreBookingOrder (.cs), DocumentTypeId 129
 * Page: templates/sale/bk/pre_booking_order.html (route /sale/booking/pre-booking-order). Data: /api/sale/bk/pre-booking/*.
 *   PreBookingOrder_Load :362      locks, cost center (AppId 5), rights (Save / Update / Print), RateEditableOnPrebookingOrder -> txtItemRate, then
 *                                  GenerateDocNo, SuppCustomerFill, PaymentTerms (Rows[2]), DeliveryTerms, CommissionTypeFill, CommissionUOMFill (Rows[0]), ItemFill, HistoryCombosFill
 *   CmbPaymentTerm_Leave :737      term 1 or 3 -> txtDueDays cleared + disabled, else enabled + focus
 *   CmbItem_Leave :822             bindRateUomAndItemPackUom :758 (keeps the pack / rate uom text when it still exists), CropYear :645, getRateRateUom :846
 *   FormValidationDetail :937 / btnAddDetail_Click :984 / ResetDetail :1007 / grd_DoubleClick :1029 / btnUpdateDetail_Click :1130
 *   grd_ColumnButtonClick :1061 / grd_KeyDown :1166   Delete: update-mode check, confirm, removed rows kept (ActionTypeId 3) when the order is being updated
 *   CalculateWeight :2164 / CalculateAmount :2221 / DueDaysCalculation :2523 / CalculateExpiryDate :2542 / TotalCommissionAmount :2561 / TotalOtherCommissionAmount :2750
 *   Reset :1313 / Insert :1423 / FormValidation :896      messages and order as the form; the server repeats the validations and runs the save transaction
 *   HistoryFill :1691 / HistoryGridSettings :1806 / ReadById :1886 / GridDetailBind :2021 / DataGridHistory_* :1958-2005
 *   PreBookingOrder_KeyDown :2344 / MakeShortCutKeys :2464
 * Not ported: the Attachment dialog (AT / FormHelper), see the report.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/bk/pre-booking';
    var st = { recId: 0, canSave: false, canUpdate: false, canPrint: false, appId: 0, supplierCustomerId: 0, rateEditable: false, historyDays: 3,
        rows: [], removed: [], updateRow: null, scheduleId: 0, uoms: [], tab: 0, amtDec: 2, rateDec: 2, histDetail: [] };
    var quiet = 0;

    var grid = new S.Grid({ tableId: 'detail', gridId: 'grd', navId: 'navD', navTextId: 'navTextD', headerLines: 1, noFilter: true, frozen: 1, autosize: true,
        onButton: function (row, col) { if (col.key === '_Del') deleteRow(row, false); } });
    var hist = new S.Grid({ tableId: 'history', gridId: 'gridH', navId: 'navH', navTextId: 'navTextH', headerLines: 2, noFilter: true, frozen: 3,
        onButton: historyButton, onLink: function () { attachmentsGap(); }, onSelect: function (r) { gridDetailBind(A.toInt(A.ci(r, 'Id'))); } });
    var hd = new S.Grid({ tableId: 'historyDetail', gridId: 'gridD2', navId: 'navD2', navTextId: 'navTextD2', headerLines: 1, noFilter: true, autosize: true });

    function say(m) { w.alert(m); }
    function num(t) { var x = Number(String(t == null ? '' : t).replace(/,/g, '').trim()); return isFinite(x) ? x : 0; }
    function n3(x) { return S.fmtNum(x, '#,##0.###'); }
    function amtText(x) { return S.fmtNum(x, S.fmtSingle(st.amtDec)); }
    function fail(e) { say(e && e.message ? e.message : String(e)); }
    function q(fn) { quiet++; try { fn(); } finally { quiet--; } }
    function fillQ(id, rows, zero, keep) { q(function () { S.fill(el(id), rows, zero, keep); }); }
    function on(id, ev, fn) { el(id).addEventListener(ev, function (e) { if (!quiet) return fn(e); }); }
    /* set a select to a value (Value = x); a value that is not in the list leaves it empty */
    function setVal(id, v) {
        var s = el(id); if (!s) return;
        var i = -1, k;
        for (k = 0; k < s.options.length; k++) if (s.options[k].value === String(v) && String(v) !== '') { i = k; break; }
        q(function () { s.selectedIndex = i >= 0 ? i : 0; s.dispatchEvent(new Event('change', { bubbles: true })); });
    }
    function clearSel(id) { var s = el(id); if (!s) return; q(function () { s.selectedIndex = 0; s.dispatchEvent(new Event('change', { bubbles: true })); }); }
    /* Text = x on a combo: the row with that caption, else empty */
    function setText(id, t) {
        var s = el(id), i = -1, k;
        if (!s) return;
        for (k = 0; k < s.options.length; k++) if (s.options[k].value !== '' && s.options[k].textContent.trim() === t) { i = k; break; }
        q(function () { s.selectedIndex = i >= 0 ? i : 0; s.dispatchEvent(new Event('change', { bubbles: true })); });
    }
    function hasText(id, t) {
        var s = el(id), k;
        for (k = 0; k < s.options.length; k++) if (s.options[k].value !== '' && s.options[k].textContent.trim() === t) return true;
        return false;
    }
    function saveVisible() { return !el('btnsave').hidden; }
    function saveEnabled() { return !el('btnsave').disabled; }
    function setDis(id, on_) { var f = el(id); if (f) f.disabled = !!on_; var wrap = S.wrapOf(id); if (wrap && wrap !== f) wrap.classList.toggle('disabled', !!on_); }
    function dayOf(id) { return el(id).value || A.today(); }
    function attachmentsGap() { say('Attachments are not available in the web version of this screen.'); }

    // ------------------------------------------------------------------ detail grid
    function gridCols() {
        return [{ key: '_Del', caption: 'X', width: 20, button: 'X' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'ScheduleId', caption: 'ScheduleId', hidden: true }, { key: 'ItemId', caption: 'ItemId', hidden: true },
            { key: 'ItemName', caption: 'ItemName' }, { key: 'CropYearId', caption: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'PackUomId', caption: 'PackUomId', hidden: true }, { key: 'PackUom', caption: 'PackUom' },
            { key: 'ItemQty', caption: 'ItemQty', fmt: '#,##0.###', totalFmt: '#,##0.###', sum: true, align: 'r' },
            { key: 'Weight', caption: 'Weight', fmt: '#,##0.###', totalFmt: '#,##0.###', sum: true, align: 'r' },
            { key: 'ItemRate', caption: 'ItemRate', num: true, fmt: '#,##0.##########' },
            { key: 'RateUomId', caption: 'RateUomId', hidden: true }, { key: 'RateUom', caption: 'RateUom' },
            { key: 'ItemAmount', caption: 'ItemAmount', fmt: S.fmtSingle(st.amtDec), totalFmt: S.fmtSingle(st.amtDec), sum: true, align: 'r' },
            { key: 'Remarks', caption: 'Remarks' }];
    }
    function renderGrid() { grid.setData(gridCols(), st.rows); }
    function sumOf(k) { var t = 0; st.rows.forEach(function (r) { t += A.toDouble(r[k]); }); return t; }

    // ------------------------------------------------------------------ calculations
    function round0(x) { return x < 0 ? -Math.round(-x) : Math.round(x); }
    function commission(typeId, rateId, uomId, amtId) {
        var rate = el(rateId).value;
        if (rate !== '') {
            var type = S.selText(typeId);
            if (type === 'Flat') el(amtId).value = n3(num(rate));
            else if (type === 'Percent') el(amtId).value = n3(round0(sumOf('ItemAmount') * num(rate) / 100));
            else if (type === 'Comm Weight') {
                var uom = num(S.selText(uomId)), wt = sumOf('Weight'), r = num(rate);
                el(amtId).value = (wt > 0 && r > 0 && uom > 0) ? n3(round0(wt / uom * r)) : n3(0);
            }
        } else el(amtId).value = '0';
    }
    function totalCommission() { commission('cmbCommType', 'txtCommRate', 'cmbCommRateUom', 'txtCommAmount'); }
    function totalOtherCommission() { commission('CmbOtherCommissionType', 'txtOtherCommissionRate', 'CmbOtherCommissionUom', 'txtOtherCommissionAmount'); }
    function uomRow(id) { var r = null; st.uoms.forEach(function (u) { if (String(A.ci(u, 'Id')) === String(id)) r = u; }); return r; }
    function calcWeight() {
        var id = S.selInt('cmbPackUom'), qty = num(el('txtqty').value), u = uomRow(id);
        el('txtWeight').value = (u && id > 0 && qty > 0) ? n3(A.toDouble(A.ci(u, 'equivalent')) * qty) : '0';
        calcAmount();
    }
    function calcAmount() {
        var id = S.selInt('CmbRateUom'), rate = num(el('txtItemRate').value), wt = num(el('txtWeight').value), u = uomRow(id);
        el('txtItemAmount').value = (id > 0 && rate > 0 && wt > 0 && u) ? amtText(wt / A.toDouble(A.ci(u, 'equivalent')) * rate) : '0';
    }
    function dueDays() { var t = el('txtDueDays').value.trim(); el('DueDate').value = t !== '' ? A.addDays(dayOf('DocDate'), Math.trunc(num(t))) : dayOf('DocDate'); }
    function expiry() { var n = A.toInt(el('txtDeliveryDays').value), s = dayOf('deliveryStartDate'); el('datExpiryDate').value = n === 0 ? s : A.addDays(s, Math.trunc(num(el('txtDeliveryDays').value))); }

    // ------------------------------------------------------------------ item box
    function fillPackRate(uoms) {
        var pack = S.selText('cmbPackUom'), rate = S.selText('CmbRateUom');
        st.uoms = uoms || [];
        if (st.uoms.length) {
            fillQ('cmbPackUom', st.uoms, false); fillQ('CmbRateUom', st.uoms, false);
            if (hasText('cmbPackUom', pack)) setText('cmbPackUom', pack); else clearSel('cmbPackUom');
            if (hasText('CmbRateUom', rate)) setText('CmbRateUom', rate); else clearSel('CmbRateUom');
        } else { fillQ('cmbPackUom', [], false); fillQ('CmbRateUom', [], false); }
    }
    function fillCrop(rows) {
        var old = S.selText('CmbCropyr');
        if (rows && rows.length) {
            fillQ('CmbCropyr', rows, false);
            if (old && hasText('CmbCropyr', old)) setText('CmbCropyr', old); else q(function () { S.activate('CmbCropyr', 0, false); });
        } else fillQ('CmbCropyr', [], false);
    }
    function getRate() {
        var item = S.selInt('CmbItem'), crop = S.selInt('CmbCropyr');
        function none() { st.scheduleId = 0; clearSel('CmbRateUom'); el('txtItemRate').value = '0'; calcAmount(); }
        if (item > 0 && crop > 0) {
            return A.getJson(API + '/rate?' + S.qs({ itemId: item, cropYearId: crop, docDate: dayOf('DocDate') })).then(function (r) {
                if (r.found) { st.scheduleId = A.toInt(r.scheduleId); setVal('CmbRateUom', A.toInt(r.rateUomId)); el('txtItemRate').value = n3(A.toDouble(r.rate)); calcAmount(); }
                else none();
            });
        }
        none(); return Promise.resolve();
    }
    function itemLeave() {
        var item = S.selInt('CmbItem');
        return A.getJson(API + '/item?' + S.qs({ itemId: item, docDate: dayOf('DocDate') })).then(function (r) {
            if (item > 0) fillPackRate(r.uoms);
            else { st.uoms = []; fillQ('cmbPackUom', [], false); fillQ('CmbRateUom', [], false); }
            fillCrop(r.cropYears);
            return getRate();
        });
    }
    function resetDetail() {
        st.scheduleId = 0;
        clearSel('CmbItem'); fillQ('CmbCropyr', [], false); clearSel('cmbPackUom');
        el('txtqty').value = ''; el('txtWeight').value = ''; el('txtItemRate').value = ''; clearSel('CmbRateUom'); el('txtItemAmount').value = ''; el('txtRemarksDetail').value = '';
    }
    function formValidationDetail() {
        if (S.selInt('CmbItem') <= 0) { say('ItemName Field is Required'); S.focus('CmbItem'); return false; }
        if (S.selInt('CmbCropyr') <= 0) { say('Crop Year Field is Required'); S.focus('CmbCropyr'); return false; }
        if (S.selInt('cmbPackUom') <= 0) { say('PackUom Field is Required'); S.focus('cmbPackUom'); return false; }
        var t = el('txtqty').value.trim(); if (t === '' || t === '0') { say('Item Qty Field is Required'); el('txtqty').focus(); return false; }
        t = el('txtWeight').value.trim(); if (t === '' || t === '0') { say('Weight Field is Required'); el('txtWeight').focus(); return false; }
        t = el('txtItemRate').value.trim(); if (t === '' || t === '0') { say('Item Rate Field is Required'); el('txtItemRate').focus(); return false; }
        t = el('txtItemAmount').value.trim(); if (t === '' || t === '0') { say('Item Amount Field is Required'); el('txtItemAmount').focus(); return false; }
        return true;
    }
    function rowFromBox(r) {
        r.ScheduleId = st.scheduleId; r.ItemId = S.selInt('CmbItem'); r.ItemName = S.selText('CmbItem');
        r.CropYearId = S.selInt('CmbCropyr'); r.CropYear = S.selText('CmbCropyr');
        r.PackUomId = S.selInt('cmbPackUom'); r.PackUom = S.selText('cmbPackUom');
        r.ItemQty = num(el('txtqty').value); r.Weight = num(el('txtWeight').value); r.ItemRate = num(el('txtItemRate').value);
        r.RateUomId = S.selInt('CmbRateUom'); r.RateUom = S.selText('CmbRateUom'); r.ItemAmount = num(el('txtItemAmount').value);
        r.Remarks = el('txtRemarksDetail').value.trim();
        return r;
    }
    function btnAddDetail() {
        if (!formValidationDetail()) return;
        var r = rowFromBox({ Id: 0 });
        st.rows.push(r); renderGrid();
        S.focus('CmbItem'); resetDetail();
        el('DocDate').disabled = true;
        totalCommission(); totalOtherCommission();
    }
    function editRow(r) {
        st.updateRow = r; st.scheduleId = A.toInt(r.ScheduleId);
        setVal('CmbItem', r.ItemId);
        return itemLeave().then(function () {
            setVal('CmbCropyr', A.toInt(r.CropYearId)); setVal('cmbPackUom', r.PackUomId);
            el('txtqty').value = String(r.ItemQty); el('txtWeight').value = String(r.Weight); el('txtItemRate').value = String(r.ItemRate);
            setVal('CmbRateUom', r.RateUomId); el('txtItemAmount').value = String(r.ItemAmount); el('txtRemarksDetail').value = r.Remarks == null ? '' : r.Remarks;
            el('btnAddDetail').hidden = true; el('btnUpdateDetail').hidden = false; el('btnCancelUpdateDetial').hidden = false;
            S.focus('CmbItem');
        }).catch(fail);
    }
    function btnUpdateDetail() {
        if (!formValidationDetail()) return;
        rowFromBox(st.updateRow);
        renderGrid();
        el('btnAddDetail').hidden = false; el('btnUpdateDetail').hidden = true; el('btnCancelUpdateDetial').hidden = true;
        st.updateRow = null;
        resetDetail(); S.focus('CmbItem');
        totalCommission(); totalOtherCommission();
    }
    function deleteRow(r, viaKey) {
        if (st.updateRow === r) { say("You can't Delete this Record Because This Record is In Update Mode"); return; }
        if (!w.confirm('Are you sure to Delete?')) return;
        var newMode = (saveVisible() && saveEnabled()) || (!el('btnSaveAs').hidden && !el('btnSaveAs').disabled);
        if (!newMode && A.toInt(r.Id) > 0) {
            st.removed.push({ Id: r.Id, ScheduleId: r.ScheduleId, ItemId: r.ItemId, CropYear: r.CropYear, CropYearId: r.CropYearId, PackUomId: r.PackUomId, ItemQty: r.ItemQty,
                Weight: r.Weight, ItemRate: r.ItemRate, RateUomId: r.RateUomId, ItemAmount: r.ItemAmount, Remarks: r.Remarks, viaKey: !!viaKey });
        }
        st.rows.splice(st.rows.indexOf(r), 1);
        renderGrid();
        totalCommission();
        if (st.rows.length === 0) el('DocDate').disabled = false;
    }

    // ------------------------------------------------------------------ load
    function costCenterId() { return st.appId === 5 ? S.selInt('CmbCostCenter') : 0; }
    function load() {
        return A.getJson(API + '/init').then(function (data) {
            st.canSave = !!data.canSave; st.canUpdate = !!data.canUpdate; st.canPrint = !!data.canPrint;
            st.appId = A.toInt(data.appId); st.supplierCustomerId = A.toInt(data.supplierCustomerId); st.rateEditable = !!data.rateEditable;
            st.historyDays = A.toInt(data.historyDays) || 3;
            st.amtDec = A.toInt(data.amountDecimals); st.rateDec = A.toInt(data.rateDecimals);
            if (st.supplierCustomerId > 0) { setDis('CmbCustomer', true); setDis('CmbCustomerHistory', true); }
            if (st.appId === 5) {
                el('lblCostCenter').hidden = false;
                fillQ('CmbCostCenter', data.costCenters, false);
                if (data.costCenters && data.costCenters.length) q(function () { S.activate('CmbCostCenter', 0, false); });
                setDis('CmbCostCenter', true);
            } else { el('lblCostCenter').hidden = true; var cw = S.wrapOf('CmbCostCenter'); if (cw) cw.style.display = 'none'; }
            el('btnsave').disabled = !st.canSave; el('btnPrint').disabled = !st.canPrint; el('btnupdate').disabled = !st.canUpdate;
            el('txtItemRate').disabled = !st.rateEditable;
            if (A.toInt(data.docNo) > 0) el('txtdocno').value = String(data.docNo);
            bindCustomers(data.customers);
            fillQ('CmbPaymentTerm', data.paymentTerms, true); q(function () { S.setIndex('CmbPaymentTerm', 2); });
            fillQ('CmbDeliveryTerm', [{ Id: 1, name: 'Load' }, { Id: 2, name: 'Ponch' }], false); q(function () { S.activate('CmbDeliveryTerm', 0, false); });
            var types = [{ Id: 1, name: 'Flat' }, { Id: 2, name: 'Percent' }, { Id: 3, name: 'Comm Weight' }];
            fillQ('cmbCommType', types, false); fillQ('CmbOtherCommissionType', types, false);
            q(function () { S.activate('cmbCommType', 0, false); S.activate('CmbOtherCommissionType', 0, false); });
            var uoms = [{ Id: 1, name: '40' }, { Id: 2, name: '50' }, { Id: 3, name: '60' }, { Id: 4, name: '100' }];
            fillQ('cmbCommRateUom', uoms, false); q(function () { S.activate('cmbCommRateUom', 0, false); });
            fillQ('CmbOtherCommissionUom', uoms, false); q(function () { S.activate('CmbOtherCommissionUom', 0, false); });
            fillQ('CmbItem', data.items, false);
            fillQ('CmbCustomerHistory', data.historyCustomers, false);
            if (st.supplierCustomerId > 0) { setVal('CmbCustomer', st.supplierCustomerId); setVal('CmbCustomerHistory', st.supplierCustomerId); }
            el('FromDateHistory').value = A.addDays(A.today(), -st.historyDays); el('ToDateHistory').value = A.today();
            renderGrid();
            S.focus('DocDate');
        }).catch(fail);
    }
    function bindCustomers(rows) {
        fillQ('CmbCustomer', rows, false); fillQ('cmbCommissionAgent', rows, false); fillQ('CmbOtherCommissionAgent', rows, false);
    }

    // ------------------------------------------------------------------ Reset / refresh / Insert
    function generateDocNo() {
        return A.getJson(API + '/doc-no').then(function (r) { if (A.toInt(r.docNo) > 0) el('txtdocno').value = String(r.docNo); });
    }
    function reset() {
        st.removed = []; st.recId = 0; st.updateRow = null;
        el('DocDate').disabled = false;
        clearSel('CmbCustomer'); var dueWas = el('txtDueDays').value; el('txtDueDays').value = ''; if (dueWas !== '') dueDays();
        el('txtDeliveryDays').value = '';
        el('deliveryStartDate').value = dayOf('DocDate'); expiry();
        clearSel('cmbCommissionAgent'); clearSel('cmbCommType'); clearSel('cmbCommRateUom'); el('txtCommRate').value = ''; el('txtCommAmount').value = ''; el('txtCommRemarks').value = '';
        clearSel('CmbOtherCommissionAgent'); clearSel('CmbOtherCommissionType'); clearSel('CmbOtherCommissionUom'); el('txtOtherCommissionRate').value = '';
        el('txtOtherCommissionAmount').value = ''; el('txtOtherCommissionRemarks').value = '';
        el('txtRemarks').value = '';
        clearSel('CmbItem'); clearSel('cmbPackUom'); el('txtqty').value = ''; el('txtWeight').value = ''; el('txtItemRate').value = ''; clearSel('CmbRateUom');
        el('txtItemAmount').value = ''; el('txtRemarksDetail').value = '';
        el('btnsave').hidden = false; el('btnupdate').hidden = true; el('btnSaveAs').hidden = true;
        el('btnAddDetail').hidden = false; el('btnUpdateDetail').hidden = true; el('btnCancelUpdateDetial').hidden = true;
        st.rows = []; renderGrid();
        S.focus('CmbCustomer');
        return generateDocNo().catch(fail);
    }
    function btnRefresh() {
        return A.getJson(API + '/refresh?' + S.qs({ costCenterId: costCenterId() })).then(function (r) {
            bindCustomers(r.customers);
            var prev = S.selInt('CmbPaymentTerm');
            fillQ('CmbPaymentTerm', r.paymentTerms, true); q(function () { S.setIndex('CmbPaymentTerm', 2); });
            if (prev > 0) setVal('CmbPaymentTerm', prev);
            fillQ('CmbItem', r.items, false);
        }).catch(fail);
    }
    function formValidation() {
        var dn = el('txtdocno').value.trim();
        if (dn === '' || dn === '0') { say('Doc No Field is Required'); el('txtdocno').focus(); return false; }
        if (st.appId === 5 && S.selInt('CmbCostCenter') === 0) { say('Cost Center Field is Required'); S.focus('CmbCostCenter'); return false; }
        if (S.selInt('CmbCustomer') <= 0) { say('Customer Name Field is Required'); S.focus('CmbCustomer'); return false; }
        if (S.selInt('CmbPaymentTerm') === 0) { say('Payment Term Field is Required'); S.focus('CmbPaymentTerm'); return false; }
        if (S.selInt('CmbPaymentTerm') === 2 && A.toInt(el('txtDueDays').value) === 0) { say('Due Days Field is Required'); el('txtDueDays').focus(); return false; }
        if (S.selInt('CmbDeliveryTerm') === 0) { say('Delivery Term Field is Required'); S.focus('CmbDeliveryTerm'); return false; }
        return true;
    }
    function insert() {
        if (!st.rows.length) { say('Grid Record Not Found'); return; }
        if (!formValidation()) return;
        if (!w.confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            recId: st.recId, docNo: el('txtdocno').value.trim(), docDate: dayOf('DocDate'), costCenterId: costCenterId(), customerId: S.selInt('CmbCustomer'),
            remarks: el('txtRemarks').value, paymentTermId: S.selInt('CmbPaymentTerm'), dueDays: el('txtDueDays').value, dueDate: dayOf('DueDate'),
            deliveryTermId: S.selInt('CmbDeliveryTerm'), deliveryTerm: S.selText('CmbDeliveryTerm'), deliveryStartDate: dayOf('deliveryStartDate'),
            deliveryDays: el('txtDeliveryDays').value, expiryDate: dayOf('datExpiryDate'),
            comm: { agentId: S.selInt('cmbCommissionAgent'), type: S.selText('cmbCommType'), rate: el('txtCommRate').value, uom: S.selText('cmbCommRateUom'),
                amount: el('txtCommAmount').value.trim(), remarks: el('txtCommRemarks').value },
            other: { agentId: S.selInt('CmbOtherCommissionAgent'), type: S.selText('CmbOtherCommissionType'), rate: el('txtOtherCommissionRate').value,
                uom: S.selText('CmbOtherCommissionUom'), amount: el('txtOtherCommissionAmount').value.trim(), remarks: el('txtOtherCommissionRemarks').value },
            rows: st.rows.map(function (r) {
                return { Id: r.Id, ScheduleId: r.ScheduleId, ItemId: r.ItemId, CropYearId: r.CropYearId, CropYear: r.CropYear, PackUomId: r.PackUomId, ItemQty: r.ItemQty,
                    Weight: r.Weight, ItemRate: r.ItemRate, RateUomId: r.RateUomId, ItemAmount: r.ItemAmount, Remarks: r.Remarks };
            }),
            removed: st.recId > 0 ? st.removed : []
        };
        var print = el('ChkBox').checked;
        return S.post(API + '/save', body).then(function (res) {
            say(res.message);
            return reset().then(function () { if (print) printSlip(A.toInt(res.id)); });
        }).catch(fail);
    }
    function btnSave() { st.recId = 0; return insert(); }
    function btnUpdate() { if (st.recId === 0) { say("Can't Update Record Because Id not Found"); return; } return insert(); }
    function printSlip(id) {
        if (id === 0) { say('No Record Found For Display'); return; }
        A.openPdf('/sale/bk/print/prebooking-slip?id=' + id);
    }
    function btnPrint() { if (st.recId > 0) printSlip(st.recId); else say('No Record Found for Display'); }

    // ------------------------------------------------------------------ history
    var HCOL = [['Id', null, true], ['DocNo', 60], ['DocDate', 73], ['CustomerName', 170], ['PaymentTerm', 70], ['DueDays', 60], ['DueDate', 73], ['DeliveryTerm', 70],
        ['DeliveryStartDate', 73], ['DeliveryDays', 60], ['ExpiryDate', 73], ['CommissionAgent', 150], ['CommType', 80], ['CommRate', 70], ['CommAmount', 90],
        ['OtherCommissionAgent', 150], ['OtherCommType', 80], ['OtherCommRate', 70], ['OtherCommAmount', 90], ['EntryUser', 80], ['EntryDate', 135], ['ModifyUser', 80],
        ['ModifyDate', 135], ['NoOfAttachments', 100], ['Remarks', 250]];
    function histCols() {
        var cols = [{ key: '_Edit', caption: 'Edit', width: 40, button: 'Edit' }, { key: '_SaveAs', caption: 'SaveAs', width: 60, button: 'SaveAs' }, { key: '_Print', caption: 'Print', width: 50, button: 'Print' }];
        HCOL.forEach(function (h) {
            var c = { key: h[0], caption: h[0], width: h[1], hidden: !!h[2] };
            if (/Date$/.test(h[0])) c.date = /^(Entry|Modify)Date$/.test(h[0]) ? 'dd-MM-yy hh:mm tt' : 'dd-MMM-yy';
            if (h[0] === 'CommRate' || h[0] === 'OtherCommRate') { c.fmt = '#,##0.####'; c.align = 'r'; }
            if (h[0] === 'CommAmount' || h[0] === 'OtherCommAmount') { c.fmt = '#,##0.####'; c.totalFmt = '#,##0.####'; c.sum = true; c.align = 'r'; }
            if (h[0] === 'NoOfAttachments') c.link = true;
            cols.push(c);
        });
        return cols;
    }
    function historyFill() {
        var by = d.querySelector('input[name="dateBy"]:checked');
        var args = { fromDate: el('FromDateHistory').value, toDate: el('ToDateHistory').value, dateBy: by ? by.value : 'doc', fromDocNo: A.toInt(el('FromDocNoHistory').value),
            toDocNo: A.toInt(el('ToDocNoHistory').value), customerId: S.selInt('CmbCustomerHistory'), costCenterId: costCenterId() };
        return A.getJson(API + '/history?' + S.qs(args)).then(function (rows) {
            if (rows && rows.length) hist.setData(histCols(), rows); else { hist.clear(); hd.clear(); }
        }).catch(fail);
    }
    function gridDetailBind(id) {
        return A.getJson(API + '/get?' + S.qs({ id: id })).then(function (po) {
            var rows = (po.details || []).map(function (x) {
                return { ItemName: x.ItemName, PackUom: x.UOMCode, ItemQty: x.OrderItemQty, Weight: x.NetWeight, ItemRate: x.OrderItemRate, RateUom: x.RateUom, ItemAmount: x.Amount, Remarks: x.OrderRemarks };
            });
            hd.setData([{ key: 'ItemName', caption: 'ItemName' }, { key: 'PackUom', caption: 'PackUom' },
                { key: 'ItemQty', caption: 'ItemQty', fmt: '#,##0.###', totalFmt: '#,##0.###', sum: true, align: 'r' },
                { key: 'Weight', caption: 'Weight', fmt: '#,##0.###', totalFmt: '#,##0.###', sum: true, align: 'r' },
                { key: 'ItemRate', caption: 'ItemRate', fmt: S.fmtRate(st.rateDec), align: 'r' }, { key: 'RateUom', caption: 'RateUom' },
                { key: 'ItemAmount', caption: 'ItemAmount', fmt: S.fmtSingle(st.amtDec), totalFmt: S.fmtSingle(st.amtDec), sum: true, align: 'r' },
                { key: 'Remarks', caption: 'Remarks' }], rows);
        }).catch(fail);
    }
    function readById(id) {
        st.removed = [];
        st.recId = A.toInt(id);
        return A.getJson(API + '/get?' + S.qs({ id: id })).then(function (po) {
            selectTab(0);
            var det = po.details || [], cc = det.length ? A.toInt(det[0].CostCenterId) : 0;
            el('DocDate').value = S.isoDay(po.DocDate); el('txtdocno').value = String(po.DocNo == null ? '' : po.DocNo);
            if (cc > 0) setVal('CmbCostCenter', cc);
            setVal('CmbCustomer', A.toInt(po.OrderSupCustId)); setVal('CmbPaymentTerm', A.toInt(po.PaymentTermsId));
            el('txtDueDays').value = String(po.OrderDueDays == null ? '' : po.OrderDueDays); el('DueDate').value = S.isoDay(po.OrderDueDate);
            setText('CmbDeliveryTerm', po.DeliveryTerm == null ? '' : String(po.DeliveryTerm));
            el('deliveryStartDate').value = S.isoDay(po.DeliveryStartDate); el('txtDeliveryDays').value = String(po.DeliveryDays == null ? '' : po.DeliveryDays);
            el('datExpiryDate').value = S.isoDay(po.OrderExpiryDate);
            if (A.toInt(po.BrokerAgentSupCustId) > 0) setVal('cmbCommissionAgent', A.toInt(po.BrokerAgentSupCustId));
            setText('cmbCommType', po.CommissionType == null ? '' : String(po.CommissionType));
            el('txtCommRate').value = String(po.CommRate == null ? '' : po.CommRate);
            if (A.toInt(po.UomScheduleIdCmRate) > 0) setText('cmbCommRateUom', String(po.UomScheduleIdCmRate));
            el('txtCommAmount').value = String(po.CommAmount == null ? '' : po.CommAmount); el('txtCommRemarks').value = po.CommissionRemarks == null ? '' : po.CommissionRemarks;
            if (A.toInt(po.OtherCommissionAgentId) > 0) setVal('CmbOtherCommissionAgent', A.toInt(po.OtherCommissionAgentId));
            setText('CmbOtherCommissionType', po.OtherCommissionType == null ? '' : String(po.OtherCommissionType));
            el('txtOtherCommissionRate').value = String(po.OtherCommissionRate == null ? '' : po.OtherCommissionRate);
            if (A.toDouble(po.OtherCommissionUom) > 0) setText('CmbOtherCommissionUom', String(po.OtherCommissionUom));
            el('txtOtherCommissionAmount').value = String(po.OtherCommissionAmount == null ? '' : po.OtherCommissionAmount);
            el('txtOtherCommissionRemarks').value = po.OtherCommissionRemarks == null ? '' : po.OtherCommissionRemarks;
            el('txtRemarks').value = po.RemarksHeader == null ? '' : po.RemarksHeader;
            el('DocDate').disabled = true;
            st.rows = det.map(function (x) {
                return { Id: x.Id, ScheduleId: x.PriceScheduleId, ItemId: x.OrderItemId, ItemName: x.ItemName, CropYearId: x.CropYearId, CropYear: x.Crop, PackUomId: x.OrderItemUOMId,
                    PackUom: x.UOMCode, ItemQty: x.OrderItemQty, Weight: x.NetWeight, ItemRate: x.OrderItemRate, RateUomId: x.OrderItemRateUOMId, RateUom: x.RateUom,
                    ItemAmount: x.Amount, Remarks: x.OrderRemarks };
            });
            renderGrid();
            el('btnsave').hidden = true; el('btnSaveAs').hidden = true; el('btnupdate').hidden = false;
            totalCommission();
        }).catch(fail);
    }
    function historyButton(row, col) {
        var id = A.toInt(A.ci(row, 'Id'));
        if (col.key === '_Edit') { return reset().then(function () { return readById(id); }); }
        if (col.key === '_Print') { printSlip(id); return; }
        if (col.key === '_SaveAs') { return reset().then(function () { return readById(id); }).then(function () { el('btnSaveAs').hidden = false; el('btnsave').hidden = true; el('btnupdate').hidden = true; }); }
    }
    function resetHistory() {
        el('FromDateHistory').value = A.addDays(A.today(), -st.historyDays); el('ToDateHistory').value = A.today();
        el('FromDocNoHistory').value = ''; el('ToDocNoHistory').value = '';
        clearSel('CmbCustomerHistory'); hist.clear(); hd.clear();
        el('drdocdate').checked = true; S.focus('FromDateHistory');
    }
    function refreshHistory() {
        return A.getJson(API + '/history-customers?' + S.qs({ costCenterId: costCenterId() })).then(function (rows) { fillQ('CmbCustomerHistory', rows, false); }).catch(fail);
    }

    // ------------------------------------------------------------------ tabs / keys
    function selectTab(n) {
        st.tab = n;
        el('page1').hidden = n !== 0; el('page2').hidden = n !== 1;
        el('tab1').classList.toggle('on', n === 0); el('tab2').classList.toggle('on', n === 1);
        S.focus(n === 0 ? 'CmbCustomer' : 'FromDateHistory');
    }
    function shortcutKeys() {
        A.shortcuts([['Ctrl+S', 'For Save when in Form Tab And For History When in History Tab'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
            ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+F12', 'For Save As'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus on Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Item Name in Detail Box'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function closeForm() { if (w.history.length > 1) w.history.back(); else w.location.href = '/dashboard'; }
    function visible(id) { return !el(id).hidden && !el(id).disabled; }
    function formKey(f0, f1) { return function () { if (st.tab === 0) { if (f0) return f0(); } else if (f1) return f1(); }; }

    S.keys({
        s: function () { if (st.tab === 0) { if (visible('btnsave')) btnSave(); } else historyFill(); },
        u: formKey(function () { if (visible('btnupdate')) btnUpdate(); }),
        n: formKey(reset, resetHistory), r: formKey(btnRefresh, refreshHistory),
        F5: formKey(function () { S.focus('DocDate'); }),
        ArrowDown: formKey(function () { grid.focus(); }, function () { hist.focus(); }),
        ArrowUp: formKey(function () { S.focus('CmbItem'); }, function () { S.focus('FromDateHistory'); }),
        ArrowRight: formKey(null, function () { var f = d.activeElement; if (f && f.closest && f.closest('#gridD2')) hist.focus(); else hd.focus(); }),
        F10: formKey(attachmentsGap), F12: formKey(function () { if (visible('btnSaveAs')) btnSave(); }),
        p: formKey(btnPrint),
        e: closeForm,
        t: function () { if (st.tab === 1) { selectTab(0); S.focus('DocDate'); } else selectTab(1); },
        alt: shortcutKeys
    }, closeForm);

    var order = ['DocDate', 'CmbCustomer', 'CmbPaymentTerm', 'txtDueDays', 'txtRemarks', 'CmbDeliveryTerm', 'deliveryStartDate', 'txtDeliveryDays',
        'cmbCommissionAgent', 'cmbCommType', 'txtCommRate', 'cmbCommRateUom', 'txtCommRemarks',
        'CmbOtherCommissionAgent', 'CmbOtherCommissionType', 'txtOtherCommissionRate', 'CmbOtherCommissionUom', 'txtOtherCommissionRemarks',
        'CmbItem', 'CmbCropyr', 'cmbPackUom', 'txtqty', 'txtItemRate', 'txtRemarksDetail', 'btnAddDetail'];
    var orderH = ['FromDateHistory', 'ToDateHistory', 'FromDocNoHistory', 'ToDocNoHistory', 'CmbCustomerHistory', 'btnShowHistory'];
    d.addEventListener('keydown', function (e) {
        if (e.key !== 'Enter' || e.ctrlKey || e.altKey || A.comboOpen()) return;
        var t = e.target; if (t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.closest('.grid')) return;
        var list = st.tab === 0 ? order : orderH, cur = -1;
        list.forEach(function (id, i) { var f = A.focusable(id); if (f === t || (f && f.contains && f.contains(t))) cur = i; });
        if (cur < 0) return;
        e.preventDefault();
        for (var k = cur + 1; k <= list.length; k++) {
            var id = list[k % list.length], f = A.focusable(id);
            if (f && !f.disabled && f.offsetParent !== null) { f.focus(); return; }
        }
    });

    // grid keys: Ctrl+Space / Ctrl+Delete delete the current row, Ctrl+Enter edits it (the web grid has no current column, so Ctrl+Space acts as the Delete column)
    el('grd').addEventListener('keydown', function (e) {
        var r = grid.current(); if (!r || !e.ctrlKey) return;
        if (e.code === 'Space' || e.key === ' ' || e.key === 'Delete') { e.preventDefault(); deleteRow(r, true); totalCommission(); }
        else if (e.key === 'Enter') { e.preventDefault(); editRow(r); }
    });
    el('grd').addEventListener('dblclick', function (e) { if (e.target.closest('button')) return; var r = grid.current(); if (r) editRow(r); });
    el('gridH').addEventListener('dblclick', function (e) {
        if (e.target.closest('button') || e.target.closest('a')) return;
        var r = hist.current(); if (r) reset().then(function () { return readById(A.toInt(A.ci(r, 'Id'))); });
    });
    el('gridH').addEventListener('keydown', function (e) {
        var r = hist.current(); if (!r || !e.ctrlKey || e.key !== 'Enter' || !st.canUpdate) return;
        e.preventDefault(); readById(A.toInt(A.ci(r, 'Id')));
    });

    // ------------------------------------------------------------------ events
    on('CmbItem', 'change', function () { itemLeave().catch(fail); });
    on('CmbCropyr', 'change', function () { getRate().catch(fail); });
    on('CmbPaymentTerm', 'change', function () {
        var t = S.selInt('CmbPaymentTerm');
        if (t === 1 || t === 3) { el('txtDueDays').value = ''; el('txtDueDays').disabled = true; dueDays(); }
        else { el('txtDueDays').disabled = false; el('txtDueDays').focus(); }
    });
    on('cmbPackUom', 'change', calcWeight); on('txtqty', 'input', calcWeight);
    on('CmbRateUom', 'change', calcAmount); on('txtItemRate', 'input', calcAmount);
    on('txtDueDays', 'input', dueDays);
    on('txtDeliveryDays', 'input', expiry);
    on('deliveryStartDate', 'change', function () {
        if (dayOf('deliveryStartDate') < dayOf('DocDate')) { el('deliveryStartDate').value = dayOf('DocDate'); say("Delivery Start Date Can't Be Less Than Doc Date"); }
        expiry();
    });
    on('cmbCommType', 'change', totalCommission); on('txtCommRate', 'input', totalCommission); on('cmbCommRateUom', 'change', totalCommission);
    on('CmbOtherCommissionType', 'change', totalOtherCommission); on('txtOtherCommissionRate', 'input', totalOtherCommission); on('CmbOtherCommissionUom', 'change', totalOtherCommission);
    S.digitsOnly('txtqty', true); S.digitsOnly('txtCommRate', true); S.digitsOnly('txtOtherCommissionRate', true);
    S.digitsOnly('txtDueDays', false); S.digitsOnly('txtDeliveryDays', false); S.digitsOnly('FromDocNoHistory', false); S.digitsOnly('ToDocNoHistory', false);

    el('btnnew').addEventListener('click', reset);
    el('btnRefresh').addEventListener('click', btnRefresh);
    el('btnsave').addEventListener('click', btnSave);
    el('btnSaveAs').addEventListener('click', btnSave);
    el('btnupdate').addEventListener('click', btnUpdate);
    el('btnattachment').addEventListener('click', attachmentsGap);
    el('btnPrint').addEventListener('click', btnPrint);
    el('btnshortcutkeys').addEventListener('click', shortcutKeys);
    el('btnAddDetail').addEventListener('click', btnAddDetail);
    el('btnUpdateDetail').addEventListener('click', btnUpdateDetail);
    el('btnCancelUpdateDetial').addEventListener('click', function () { /* the form has no btnCancelUpdateDetial_Click handler */ });
    el('btnShowHistory').addEventListener('click', historyFill);
    el('btnResetHistory').addEventListener('click', resetHistory);
    el('btnRefreshHistory').addEventListener('click', refreshHistory);
    el('tab1').addEventListener('click', function () { selectTab(0); });
    el('tab2').addEventListener('click', function () { selectTab(1); });
    S.closeShortcuts();

    var t0 = A.today();
    el('DocDate').value = t0; el('DueDate').value = t0; el('deliveryStartDate').value = t0; el('datExpiryDate').value = t0;
    el('FromDateHistory').value = A.addDays(t0, -3); el('ToDateHistory').value = t0;
    load();
}(window, document));
