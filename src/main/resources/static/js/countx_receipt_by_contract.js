/* Receipt By Contract - Architecture.WinApp.Account_Definition.ReceiptByContract (screen 18, DocumentTypeId 29).
   Event-for-event port of ReceiptByContract.cs. "Leave" events are fired from the combo's focusout, as the WinForms Leave event. */
(function ($) {
    'use strict';
    var API = '/accounts/receipt-by-contract';
    var RecId = 0, VoucherHeadId = 0, uidSeq = 0, updateRow = null, updateDetailIndexAmount = 0;
    var suppliers = [], hasSubsidiary = false, rights = null;
    var detail = [];                 // dtdetail (the rows behind grdDetail)
    var attSaved = [], attNew = [], docTime = '00:00:00', viewId = 0, lastCol = {};
    var TRANS_TYPES = [{ V: 1, T: 'Regular' }, { V: 2, T: 'Advance' }, { V: 3, T: 'Advance Utilize' }];
    var TRANS_AGAINST = [{ V: 1, T: 'Cash' }, { V: 2, T: 'Bank' }, { V: 3, T: 'Party' }];

    function busy(btn, on) { var $b = $(btn); if (on) $b.prop('disabled', true).addClass('btn-busy'); else $b.removeClass('btn-busy').prop('disabled', false); }
    function isBusy(btn) { return $(btn).hasClass('btn-busy'); }
    function say(m) { if (m != null && m !== '') window.alert(m); }
    function failText(x) {
        try { var j = x.responseJSON || JSON.parse(x.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return x && x.status ? 'HTTP ' + x.status : 'Request failed';
    }
    function call(method, path, data) {
        return new Promise(function (resolve, reject) {
            var o = { url: API + path, type: method, dataType: 'json', cache: false };
            if (method === 'POST') { o.contentType = 'application/json'; o.data = JSON.stringify(data == null ? {} : data); } else o.data = data || {};
            $.ajax(o).done(function (r) { if (r && r.success === false) reject(new Error(r.message || 'Error')); else resolve(r); })
                .fail(function (x) { reject(new Error(failText(x))); });
        });
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function ymd(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function nowParts() { var d = new Date(); return { date: ymd(d), time: pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()) }; }
    /* Conversion.ToDouble: thousands separators are accepted ("1,234" from the formatted total boxes) */
    function toDouble(s) {
        if (typeof s === 'number') return isFinite(s) ? s : 0;
        var t = String(s == null ? '' : s).replace(/,/g, '').trim(); if (t === '') return 0;
        var n = Number(t); return isFinite(n) ? n : 0;
    }
    /* Conversion.ToInt = Convert.ToInt32(string / value): whole numbers only, otherwise 0 */
    function toInt(s) {
        if (typeof s === 'number') return isFinite(s) ? Math.trunc(s) : 0;
        var t = String(s == null ? '' : s).trim(); if (!/^[+-]?\d+$/.test(t)) return 0;
        var n = parseInt(t, 10); return (n > 2147483647 || n < -2147483648) ? 0 : n;
    }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function rnd(n, d) { var f = Math.pow(10, d), x = Math.round(Math.abs(n) * f + 1e-9) / f; return n < 0 ? -x : x; }
    /* ToString("#,##0.#####") family: thousands separators, at most maxDec decimals, trailing zeros trimmed */
    function fmtNum(n, maxDec) {
        n = rnd(n, maxDec); var neg = n < 0, s = Math.abs(n).toFixed(maxDec);
        if (maxDec > 0) s = s.replace(/0+$/, '').replace(/\.$/, '');
        var p = s.split('.');
        return (neg ? '-' : '') + group(p[0]) + (p[1] ? '.' + p[1] : '');
    }
    /* "#,#;(#,#);0" */
    function fmtAmt(v) { var n = Math.round(toDouble(v)); if (n === 0) return '0'; return n < 0 ? '(' + group(String(-n)) + ')' : group(String(n)); }
    /* "#,#.##" / "##,#.##": zero shows nothing */
    function fmtTax(v) { var n = rnd(toDouble(v), 2); if (n === 0) return ''; return fmtNum(n, 2); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* dd-MMM-yyyy of 'yyyy-MM-dd...' or an already formatted text */
    function fmtDMY(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v == null ? '' : v));
        return m ? m[3] + '-' + MON[+m[2] - 1] + '-' + m[1] : (v == null ? '' : String(v));
    }
    /* DateTime.ToShortDateString() (en-US) */
    function fmtShort(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v == null ? '' : v)); return m ? (+m[2]) + '/' + (+m[3]) + '/' + m[1] : (v == null ? '' : String(v)); }
    function isoDate(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v == null ? '' : v)); return m ? m[1] : ''; }
    /* Math.Round(x, 2): banker's rounding as .NET does */
    function round2(x) {
        var y = x * 100, f = Math.floor(y), d = y - f, r;
        if (Math.abs(d - 0.5) < 1e-9) r = (f % 2 === 0) ? f : f + 1; else r = Math.round(y);
        return r / 100;
    }
    function sizeMb(bytes) { return Math.round(bytes / 1048576 * 100) / 100; }

    /* ---------------------------------------------------------------- combos */
    function fill(sel, rows, keyV, keyT) {
        AccF.fillSelect(sel, (rows || []).map(function (r) { return { V: CJG.pick(r, keyV), T: CJG.pick(r, keyT) }; }), 'V', 'T', null);
        clearCombo(sel);
    }
    function comboVal(sel) { var v = $(sel).val(); return v == null ? '' : String(v); }
    function comboText(sel) { return comboVal(sel) === '' ? '' : $(sel + ' option:selected').text(); }
    function setCombo(sel, v) {
        var s = v == null ? '' : String(v);
        if (!$(sel).find('option').filter(function () { return this.value === s; }).length) s = '';
        $(sel).val(s).trigger('change');
    }
    function clearCombo(sel) { $(sel).val('').trigger('change'); }
    function activateRow(sel, i) { var o = $(sel).find('option').eq(i); if (o.length) $(sel).val(o.val()).trigger('change'); }
    function focusCombo(sel) { try { AccF.focus(sel); } catch (e) { $(sel).focus(); } }
    function saveVisible() { return !$('#btnsave').prop('hidden'); }
    function saveEnabled() { return !$('#btnsave').prop('disabled'); }
    function saveMode() { return saveVisible() && saveEnabled(); }
    function setSaveMode(on) { $('#btnsave').prop('hidden', !on); $('#btnUpdate').prop('hidden', on); }
    function showBal(lbl, txt, on) { $(lbl + ',' + txt).css('display', on ? '' : 'none'); }
    function drCr(n) { return n === 0 ? 'Nill' : (n > 0 ? 'Dr' : 'Cr'); }

    /* SupplierCustomer / BindTaxTypes / AccountsComboBind */
    function applyLists(r) {
        if (r.suppliers && r.suppliers.length > 0) {
            suppliers = r.suppliers;
            fill('#CmbSupplierName', suppliers, 'Id', 'CompanyName'); fill('#CmbCustomerHistory', suppliers, 'Id', 'CompanyName');
        }
        if (r.taxTypes && r.taxTypes.length > 0) fill('#CmbTaxType', r.taxTypes, 'Id', 'TaxName');
    }
    function applyLoad(r) {
        applyLists(r);
        hasSubsidiary = !!r.hasSubsidiary;
        if (r.taxAccounts && r.taxAccounts.length > 0) { fill('#CmbTaxCrAc', r.taxAccounts, 'Id', 'AccountTitle'); fill('#CmbTaxDrAc', r.taxAccounts, 'Id', 'AccountTitle'); }
        if (r.docNo != null && r.docNo > 0) $('#txtdocno').val(String(r.docNo));          // VoucherNofill: only when Code > 0
        if (r.rights) { rights = r.rights; $('#btnsave').prop('disabled', !rights.canSave); $('#btnUpdate').prop('disabled', !rights.canUpdate); $('#print').prop('disabled', !rights.canPrint); }
    }
    function voucherNofill() {
        return call('GET', '/code').then(function (r) { if (r.docNo != null && r.docNo > 0) $('#txtdocno').val(String(r.docNo)); }).catch(function (e) { say(e.message); });
    }
    function supplierRow() {
        var id = toInt(comboVal('#CmbSupplierName'));
        for (var i = 0; i < suppliers.length; i++) if (toInt(suppliers[i].Id) === id) return suppliers[i];
        return null;
    }

    /* ---------------------------------------------------------------- balances */
    function accountCurrentBalance() {
        var id = toInt(comboVal('#CmbCreditAc'));
        return call('GET', '/balance/account', { id: id }).then(function (r) {
            if (!r.has) return;
            var n = rnd(r.balance, 5); $('#txtbalance').val(fmtNum(n, 5)); $('#lblAmount').text(drCr(n)); showBal('#lblAmount', '#txtbalance', true);
        }).catch(function (e) { say(e.message); });
    }
    function customerCurrentBalance() {
        return call('GET', '/balance/customer', { id: toInt(comboVal('#CmbSupplierName')) }).then(function (r) {
            if (r.has) {
                var n = rnd(r.balance, 5); $('#txtCustomerBalance').val(fmtNum(n, 5)); $('#lblCustomer').text(drCr(n)); showBal('#lblCustomer', '#txtCustomerBalance', true);
            } else { $('#lblCustomer').text(''); $('#txtCustomerBalance').val('0'); }
        }).catch(function (e) { say(e.message); });
    }
    function advanceAccountCurrentBalance() {
        return call('GET', '/balance/advance', { id: toInt(comboVal('#CmbSupplierName')) }).then(function (r) {
            if (!r.has) return;
            var n = rnd(r.balance, 5); $('#txtcustomerAdvBalance').val(fmtNum(n, 5)); $('#lblAdvance').text(drCr(n)); showBal('#lblAdvance', '#txtcustomerAdvBalance', true);
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- grids */
    var grdContract = CJG.create({
        table: '#grdContract', nav: '#navContract', autoResize: true,
        columns: [{ key: 'Select', caption: '', selector: true, width: 28 },
                  { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'PaymentTermsId', hidden: true },
                  { key: 'OrderDate', caption: 'OrderDate', width: 90 }, { key: 'OrderNo', caption: 'OrderNo', width: 75 },
                  { key: 'DueDate', caption: 'DueDate', width: 90, format: function (v) { return fmtDMY(v); } },
                  { key: 'PaymentTerm', caption: 'PaymentTerm', width: 100 },
                  { key: 'OrderAmount', caption: 'OrderAmount', width: 100, num: true, sum: true, format: fmtAmt, totalFormat: fmtAmt },
                  { key: 'ReceivedAmount', caption: 'ReceivedAmount', width: 115, num: true, sum: true, format: fmtAmt, totalFormat: fmtAmt },
                  { key: 'BalanceAmount', caption: 'BalanceAmount', width: 110, num: true, sum: true, format: fmtAmt, totalFormat: fmtAmt },
                  { key: 'TaxAmount', caption: 'TaxAmount', width: 100, num: true, sum: true, format: fmtTax, totalFormat: fmtTax }],
        onCheck: function () { grandTotal(); }                       // SelectionChanged: UpdateData, GrandTotal
    });
    function typeName(v) { var i = toInt(v); for (var k = 0; k < TRANS_TYPES.length; k++) if (TRANS_TYPES[k].V === i) return TRANS_TYPES[k].T; return v == null ? '' : String(v); }
    function againstName(v) { var i = toInt(v); for (var k = 0; k < TRANS_AGAINST.length; k++) if (TRANS_AGAINST[k].V === i) return TRANS_AGAINST[k].T; return v == null ? '' : String(v); }
    var grdDetail = CJG.create({
        table: '#grdDetail', nav: '#navDetail', autoResize: true,
        columns: [{ key: 'TransTypeId', caption: 'Trans Type', width: 100, format: typeName },
                  { key: 'TransAgainstId', caption: 'Trans Against', width: 100, format: againstName },
                  { key: 'GLAccountId', hidden: true }, { key: 'AccountTitle', caption: 'AccountTitle', width: 120 },
                  { key: 'Amount', caption: 'Amount', width: 80, num: true, sum: true, format: fmtAmt, totalFormat: fmtAmt },
                  { key: 'CheqNo', caption: 'CheqNo', width: 80 }, { key: 'CheqDate', caption: 'CheqDate', width: 90, format: function (v) { return fmtDMY(v); } },
                  { key: 'Paytitle', caption: 'Paytitle', width: 150 }, { key: 'Remarks', caption: 'Remarks', width: 150 },
                  { key: 'Delete', caption: 'X', button: 'X', width: 40, buttonTitle: 'Delete' }],
        onButton: function (col, row) { if (col === 'Delete') detailDelete(row); },
        onDblClick: function (row) { grdDetail_DoubleClick(row); }
    });
    var grdHistory = CJG.create({
        table: '#Gridhistory', nav: '#navHistory', autoResize: false,
        columns: [{ key: 'Edit', caption: 'Edit', button: 'Edit', width: 50, frozen: true }, { key: 'Print', caption: 'Print', button: 'Print', width: 50 },
                  { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
                  { key: 'DocDate', caption: 'DocDate', width: 80, format: function (v) { return fmtShort(v); } }, { key: 'DocNo', caption: 'DocNo', width: 60 },
                  { key: 'PartyName', caption: 'PartyName', width: 200 }, { key: 'TaxDebitAc', caption: 'TaxDebitAc', width: 200 }, { key: 'TaxCreditAc', caption: 'TaxCreditAc', width: 200 },
                  { key: 'TaxType', caption: 'TaxType', width: 90 },
                  { key: 'Tax%', caption: 'Tax%', width: 60, num: true, sum: true, format: fmtTax, totalFormat: fmtTax },
                  { key: 'TaxAmount', caption: 'TaxAmount', width: 90, num: true, sum: true, format: fmtTax, totalFormat: fmtTax },
                  { key: 'ReceivedAmount', caption: 'ReceivedAmount', width: 120, num: true, sum: true, format: fmtTax, totalFormat: fmtTax },
                  { key: 'NoOfAttachemtns', caption: 'NoOfAttachemtns', width: 120, link: true }],
        onButton: function (col, row) { if (col === 'Edit') readById(toInt(row.Id)); },          // Print: no handler on the desktop
        onLink: function (col, row) { attachmentsOf(toInt(row.Id)); },
        onDblClick: function (row) { readById(toInt(row.Id)); }
    });
    function plainRow(r) { var d = {}; for (var k in r) if (k.indexOf('__') !== 0) d[k] = r[k]; return d; }
    function bindDetail() { grdDetail.setRows(detail); }
    function contractRows() { return grdContract.rows(); }
    function checkedRows() { return contractRows().filter(function (r) { return r.__chk; }); }

    /* GrandTotal */
    function detailAmountNon2() {
        var s = 0; detail.forEach(function (r) { if (toInt(r.TransTypeId) !== 2) s += toDouble(r.Amount); }); return s;
    }
    function grandTotal() {
        var received = 0;
        if (contractRows().length > 0) checkedRows().forEach(function (r) { received += toDouble(r.ReceivedAmount); });
        var diff = received - detailAmountNon2();
        $('#txtgrandtotal').val(fmtNum(received, 6)); $('#txtdifference').val(fmtNum(diff, 5));
    }
    /* TotalReceivingAmtForContractGrid */
    function totalReceiving() {
        var total = detailAmountNon2(), rows = contractRows();
        if (total > 0) {
            rows.forEach(function (r) {
                if (total > 0) {
                    var bal = toDouble(r.BalanceAmount);
                    if (total > bal) { r.ReceivedAmount = bal; total -= bal; } else { r.ReceivedAmount = total; total = 0; }
                } else r.ReceivedAmount = total;
            });
        } else rows.forEach(function (r) { r.ReceivedAmount = 0; });
        grdContract.rerender();
    }
    /* TaxPropotionate */
    function taxOf(base) { var p = toDouble($('#txtTaxPercent').val()); return round2(base / (100 - p) * 100 * p / 100); }
    function taxPropotionate() {
        checkedRows().forEach(function (r) { r.TaxAmount = taxOf(toDouble(r.ReceivedAmount)); });
        grdContract.rerender();
    }
    /* grd_ColumnHeaderClick(Select): UpdateData, GrandTotal, TaxPropotionate */
    $('#grdContract').on('change', 'th input.hsel', function () { grandTotal(); taxPropotionate(); });
    /* ReceivedAmount is the one editable column (cell edit -> CellEdited/CellUpdated -> GrandTotal) */
    $('#grdContract').on('click', 'tbody tr.row td[data-k="ReceivedAmount"]', function () {
        var $td = $(this); if ($td.find('input').length) return;
        var row = grdContract.view()[+$td.closest('tr').data('i')]; if (!row) return;
        var $in = $('<input type="text" style="width:100%;height:100%;box-sizing:border-box;text-align:right;border:1px solid #008080;font:inherit">').val(String(toDouble(row.ReceivedAmount)));
        var done = false;
        function commit(ok) {
            if (done) return; done = true;
            if (ok) { var t = $in.val().trim(); if (t === '' || isFinite(Number(t.replace(/,/g, '')))) { row.ReceivedAmount = toDouble(t); } }
            grdContract.rerender(); if (ok) grandTotal();
        }
        $in.on('keydown', function (e) { if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); commit(true); } else if (e.key === 'Escape') { e.stopPropagation(); commit(false); } })
           .on('blur', function () { commit(true); });
        $td.empty().append($in); $in.focus().select();
    });

    /* ---------------------------------------------------------------- Leave events */
    /* SupplierCustomerAdvanceAc */
    function supplierAdvanceAc() {
        var row = supplierRow();
        if (suppliers.length > 0 && row) {
            fill('#cmbAdvanceAccount', [{ V: row.AdvanceGlAcId, T: row.AdvanceAccount }], 'V', 'T'); activateRow('#cmbAdvanceAccount', 0);
            return advanceAccountCurrentBalance();                                // cmbAdvanceAccount_Leave
        }
        fill('#cmbAdvanceAccount', [], 'V', 'T');
        return Promise.resolve();
    }
    /* GridContractFill */
    function gridContractFill() {
        if (toInt(comboVal('#CmbSupplierName')) === 0 || comboText('#CmbSupplierName').trim() === '') {
            grdContract.clear(); focusCombo('#CmbSupplierName'); say('Please select Customer Name first'); return Promise.resolve();
        }
        if (!saveMode()) return Promise.resolve();
        return call('GET', '/pending', { supplierId: toInt(comboVal('#CmbSupplierName')) }).then(function (r) {
            var rows = r.rows || [];
            if (rows.length > 0) {
                grdContract.setRows(rows.map(function (p) {
                    return { Id: p.Id, DocumentTypeId: p.DocumentTypeId, PaymentTermsId: p.PaymentTermsId, OrderDate: fmtDMY(p.OrderDate), OrderNo: p.OrderNo, DueDate: fmtDMY(p.DueDate),
                             PaymentTerm: p.PaymentTerm, OrderAmount: toDouble(p.OrderAmount), ReceivedAmount: toDouble(p.ReceivedAmount), BalanceAmount: toDouble(p.BalanceAmount), TaxAmount: 0 };
                }));
                taxPropotionate();
            } else grdContract.clear();
            grandTotal(); totalReceiving();
        }).catch(function (e) { say(e.message); });
    }
    /* cmbsupcust_Leave */
    function supplierLeave() { return supplierAdvanceAc().then(customerCurrentBalance).then(gridContractFill); }
    /* CmbTranType_Leave (CmbTranagainst.Leave): the credit accounts of the chosen against */
    function tranAgainstLeave() {
        var id = toInt(comboVal('#CmbCreditAc')), t = comboText('#CmbTranagainst');
        var type = t === 'Cash' ? 2 : t === 'Bank' ? 15 : t === 'Party' ? 3 : 0;
        if (t === 'Cash') { $('#txtCheqNo').prop('disabled', true); $('#datcheqdate').prop('disabled', true); }
        if (t === 'Bank') { $('#txtCheqNo').prop('disabled', false); $('#datcheqdate').prop('disabled', false); }
        clearCombo('#CmbCreditAc');
        if (!type) { fill('#CmbCreditAc', [], 'Id', 'AccountTitle'); return Promise.resolve(); }
        return call('GET', '/accounts', { type: type }).then(function (r) {
            var rows = r.rows || [];
            fill('#CmbCreditAc', rows, 'Id', 'AccountTitle');
            if (id > 0) { if (rows.some(function (a) { return toInt(a.Id) === id; })) setCombo('#CmbCreditAc', id); else clearCombo('#CmbCreditAc'); }
        }).catch(function (e) { say(e.message); });
    }
    /* cmbTransType_Leave */
    function transTypeLeave() {
        if (comboText('#cmbTransType') === 'Advance') {
            setCombo('#CmbTranagainst', 3);
            return tranAgainstLeave().then(function () {
                $('#CmbTranagainst').prop('disabled', true);
                var row = supplierRow();
                if (suppliers.length > 0 && row) { fill('#CmbCreditAc', [{ V: row.AdvanceGlAcId, T: row.AdvanceAccount }], 'V', 'T'); activateRow('#CmbCreditAc', 0); }
                else { say('Please Select party first...'); focusCombo('#CmbSupplierName'); }
            });
        }
        $('#CmbTranagainst').prop('disabled', false);
        return Promise.resolve();
    }
    function creditLeave() { return accountCurrentBalance(); }
    function advanceLeave() { return advanceAccountCurrentBalance(); }
    function ensureOption(sel, v, t) {
        var s = String(v); if (!$(sel).find('option').filter(function () { return this.value === s; }).length) $(sel).append($('<option>').val(s).text(t == null ? '' : t));
    }
    function fmtTotal(n, dec) {                       // ToString("0,0.00") / ToString("0,0")
        n = rnd(n, dec); var s = Math.abs(n).toFixed(dec).split('.'); var i = s[0]; while (i.length < 2) i = '0' + i;
        return (n < 0 ? '-' : '') + group(i) + (dec ? '.' + s[1] : '');
    }
    /* ChkBoxWthHolding_CheckedChanged (UpdateMode is never set on the desktop: always false) */
    function whtChanged() {
        if ($('#ChkBoxWthHolding').prop('checked')) {
            return call('GET', '/tax-schedule', { taxTypeId: toInt(comboVal('#CmbTaxType')), date: $('#datPaymentDate').val() || nowParts().date }).then(function (r) {
                var rows = r.rows || [];
                if (rows.length === 0) {
                    $('#CmbTaxCrAc, #CmbTaxDrAc').prop('disabled', true); clearCombo('#CmbTaxDrAc'); $('#txtTaxPercent').val('0'); $('#txtTaxAmount').val('0');
                } else {
                    $('#txtTaxPercent').val(String(rows[0].TaxPercent));
                    ensureOption('#CmbTaxDrAc', rows[0].TaxGLAccountId, rows[0].AccountTitle); setCombo('#CmbTaxDrAc', rows[0].TaxGLAccountId);
                    var grand = toDouble($('#txtgrandtotal').val()), tax = taxOf(grand);
                    $('#txtTaxAmount').val(String(tax)); $('#txtTotalAmount').val(fmtTotal(tax + grand, 2));
                }
                $('#CmbTaxCrAc, #CmbTaxDrAc').prop('disabled', false);
                taxPropotionate();
            }).catch(function (e) { say(e.message); });
        }
        $('#CmbTaxCrAc, #CmbTaxDrAc').prop('disabled', true); clearCombo('#CmbTaxDrAc');
        $('#txtTaxPercent').val('0'); $('#txtTaxAmount').val('0');
        $('#txtTotalAmount').val(fmtTotal(toDouble($('#txtgrandtotal').val()) + 0, 0));
        taxPropotionate();
        return Promise.resolve();
    }
    function taxTypeLeave() { return whtChanged().then(function () { taxPropotionate(); }); }
    var LEAVE = { CmbSupplierName: supplierLeave, CmbTranagainst: tranAgainstLeave, CmbCreditAc: creditLeave, cmbTransType: transTypeLeave, cmbAdvanceAccount: advanceLeave, CmbTaxType: taxTypeLeave };
    $(document).on('focusout', '.dtcombo-wrap', function () {
        var id = $(this).next('select').attr('id'), f = id && LEAVE[id];
        if (f) setTimeout(function () { f(); }, 0);
    });

    /* ---------------------------------------------------------------- detail box */
    function formValidationDetail() {
        if (comboVal('#cmbTransType') === '') { say('TransType field required'); focusCombo('#cmbTransType'); return false; }
        if (comboVal('#CmbTranagainst') === '') { say('Trans Against field required'); focusCombo('#CmbTranagainst'); return false; }
        if (comboVal('#CmbCreditAc') === '') { say('Credit Account field required'); focusCombo('#CmbCreditAc'); return false; }
        var t = $('#txtAmount').val();
        if (t === '') { say('Amount field required'); $('#txtAmount').focus(); return false; }
        if (!/^\s*[+-]?\d+\s*$/.test(t)) throw new Error('Input string was not in a correct format.');       // Convert.ToInt32(txtAmount.Text)
        var n = parseInt(t, 10);
        if (n > 2147483647 || n < -2147483648) throw new Error('Value was either too large or too small for an Int32.');
        if (n === 0) { say('Amount field required'); $('#txtAmount').focus(); return false; }
        var rm = $('#txtRemarks').val();
        if (rm === '0' || rm === '') { say('Remarks field required'); $('#txtRemarks').focus(); return false; }
        return true;
    }
    function validateAmountWithContractBalnceAmt() {
        var totalDetail = 0, totalBalance = 0, amount = toDouble($('#txtAmount').val());
        detail.forEach(function (r) { totalDetail += toDouble(r.Amount); });
        contractRows().forEach(function (r) { totalBalance += toDouble(r.BalanceAmount); });
        if (detail.length > 0 && contractRows().length > 0) {
            if (totalDetail + amount - updateDetailIndexAmount > totalBalance) { say('You Can not Add Amount Greater than Balance Total Amount'); return false; }
            return true;
        }
        if (amount > totalBalance) { say('You Can not Add Amount Greater than Balance Total Amount'); return false; }
        return true;
    }
    function rowOf(r) { for (var i = 0; i < detail.length; i++) if (detail[i].__uid === r.__uid) return detail[i]; return null; }
    function btnAdd_Click() {
        try {
            if (!(formValidationDetail() && validateAmountWithContractBalnceAmt())) return;
            var d = { TransTypeId: toInt(comboVal('#cmbTransType')), TransAgainstId: toInt(comboVal('#CmbTranagainst')), GLAccountId: toInt(comboVal('#CmbCreditAc')),
                      AccountTitle: comboText('#CmbCreditAc'), Amount: toDouble($('#txtAmount').val()), CheqNo: $('#txtCheqNo').val(),
                      CheqDate: $('#datcheqdate').val() || nowParts().date, Paytitle: $('#txtpaytitle').val(), Remarks: $('#txtRemarks').val() };
            if ($('#btnAdd').text() === 'Update' && updateRow) { for (var k in d) updateRow[k] = d[k]; }
            else { d.Paytitle = d.Paytitle.trim(); d.Remarks = d.Remarks.trim(); d.__uid = ++uidSeq; detail.push(d); }
            bindDetail(); resetDetail(); grandTotal(); totalReceiving();
        } catch (e) { say(e.message); }
    }
    function resetDetail() {
        clearCombo('#CmbCreditAc'); showBal('#lblAmount', '#txtbalance', false); $('#txtbalance').val('');
        $('#txtAmount').val(''); $('#txtCheqNo').val(''); $('#txtpaytitle').val('');
        focusCombo('#CmbTranagainst'); $('#btnAdd').text('Add'); updateRow = null;
    }
    function grdDetail_DoubleClick(row) {
        var item = row && rowOf(row);
        if (!item) return accountCurrentBalance();
        updateRow = item;
        setCombo('#cmbTransType', item.TransTypeId); setCombo('#CmbTranagainst', item.TransAgainstId);
        return tranAgainstLeave().then(function () {
            ensureOption('#CmbCreditAc', item.GLAccountId, item.AccountTitle); setCombo('#CmbCreditAc', item.GLAccountId);
            $('#txtAmount').val(String(item.Amount)); $('#txtCheqNo').val(item.CheqNo == null ? '' : item.CheqNo); $('#datcheqdate').val(isoDate(item.CheqDate) || nowParts().date);
            $('#txtpaytitle').val(item.Paytitle == null ? '' : item.Paytitle); $('#txtRemarks').val(item.Remarks == null ? '' : item.Remarks);
            updateDetailIndexAmount = toDouble(item.Amount);
            $('#btnAdd').text('Update'); focusCombo('#CmbTranagainst');
            return accountCurrentBalance();
        });
    }
    /* grdDetail_ColumnButtonClick(Delete) / Ctrl+Delete */
    function detailDelete(row) {
        try {
            if (saveMode() && row) { var i = detail.findIndex(function (r) { return r.__uid === row.__uid; }); if (i >= 0) { detail.splice(i, 1); bindDetail(); } }
            grandTotal(); totalReceiving();
        } catch (e) { say(e.message); }
    }

    /* ---------------------------------------------------------------- new / reset */
    function reset() {
        var n = nowParts();
        $('#datcheqdate').val(n.date); $('#datPaymentDate').val(n.date); docTime = n.time;
        attSaved = []; attNew = []; RecId = 0;
        $('#txtpaytitle, #txtdocno, #txtCheqNo, #txtAmount, #txtbalance, #txtCustomerBalance, #txtcustomerAdvBalance, #txtTaxPercent, #txtTaxAmount, #txtdifference, #txtgrandtotal, #txtRemarks').val('');
        clearCombo('#CmbCreditAc'); clearCombo('#CmbSupplierName'); clearCombo('#CmbTaxCrAc'); clearCombo('#CmbTaxDrAc'); clearCombo('#CmbTranagainst'); clearCombo('#CmbTaxType');
        setSaveMode(true); showBal('#lblCustomer', '#txtCustomerBalance', false); showBal('#lblAdvance', '#txtcustomerAdvBalance', false);
        grdContract.setRows([]); detail = []; bindDetail();
        $('#datPaymentDate').focus();
        return voucherNofill();
    }
    function btnNew_Click() { reset(); resetDetail(); }
    function btnRefresh_Click() { call('GET', '/load').then(function (r) { applyLists(r); }).catch(function (e) { say(e.message); }); }

    /* ---------------------------------------------------------------- Insert */
    function formvalidation() {
        if (comboText('#CmbSupplierName') === '' || toInt(comboVal('#CmbSupplierName')) === 0) { say('Please Select Supplier'); focusCombo('#CmbSupplierName'); return false; }
        if ($('#ChkBoxWthHolding').prop('checked')) {
            if (comboVal('#CmbTaxCrAc') === '') { say('Tax Credit Ac field required'); focusCombo('#CmbTaxCrAc'); return false; }
            if (comboVal('#CmbTaxDrAc') === '') { say('Tax Debit Ac field required'); focusCombo('#CmbTaxDrAc'); return false; }
            var p = $('#txtTaxPercent').val(); if (p === '' || p === '0') { say('Tax Percent field required'); $('#txtTaxPercent').focus(); return false; }
            var a = $('#txtTaxAmount').val(); if (a === '' || a === '0') { say('Tax Amount field required'); $('#txtTaxAmount').focus(); return false; }
        }
        var d = $('#txtdocno').val(); if (d === '' || d === '0') { say('DocNo field required'); $('#txtdocno').focus(); return false; }
        return true;
    }
    function insert(btn) {
        if (btn && isBusy(btn)) return;
        if (!formvalidation()) return;
        grandTotal();
        var diff = toDouble($('#txtdifference').val());
        if (diff > 0 || diff < 0) { say('Please Clear The Difference Amount'); return; }
        var amount = 0, totalReceived = 0;
        if (detail.length > 0) {
            amount = detailAmountNon2();
            if (amount !== toDouble($('#txtgrandtotal').val().trim())) { say('Please Check... Received & Total Amount Is Not Equal'); return; }
        }
        var chk = checkedRows();
        chk.forEach(function (r) { totalReceived += toDouble(r.ReceivedAmount); });
        if (totalReceived !== amount) { say('Please Check... Received & Total Amount Is Not Equal'); return; }
        var updating = RecId > 0;
        if (!window.confirm(updating ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        if (detail.length === 0 || contractRows().length === 0) { say('Outstanding Order Grid Can not be Empty'); return; }
        if (chk.length === 0) { say('Please Check any row in OutStanding Contract Grid First...'); return; }
        var req = {
            id: RecId, docDate: ($('#datPaymentDate').val() || nowParts().date) + ' ' + docTime, docNo: $('#txtdocno').val().trim(),
            supplierId: toInt(comboVal('#CmbSupplierName')), wht: !!$('#ChkBoxWthHolding').prop('checked'),
            taxTypeId: toInt(comboVal('#CmbTaxType')), taxDrId: toInt(comboVal('#CmbTaxDrAc')), taxCrId: toInt(comboVal('#CmbTaxCrAc')),
            taxPercent: $('#txtTaxPercent').val(), taxAmount: $('#txtTaxAmount').val(),
            others: detail.map(function (r) { var o = plainRow(r); o.CheqDate = isoDate(o.CheqDate) || nowParts().date; return o; }),
            contracts: contractRows().map(function (r) { return { Id: r.Id, DocumentTypeId: r.DocumentTypeId, ReceivedAmount: toDouble(r.ReceivedAmount), TaxAmount: toDouble(r.TaxAmount), checked: !!r.__chk }; }),
            keepAttachments: updating ? attSaved.map(function (a) { return a.Id; }) : [],
            addAttachments: attNew.map(function (a) { return { name: a.name, base64: '' }; })
        };
        if (btn) busy(btn, true);
        call('POST', '/save', req).then(function () {
            if (btn) busy(btn, false);
            say(updating ? 'Record Update Successfully' : 'Record Save Successfully');
            return reset();
        }).catch(function (e) { if (btn) busy(btn, false); say(e.message); });
    }
    function btnsave_Click() { RecId = 0; insert('#btnsave'); }
    function btnUpdate_Click() { insert('#btnUpdate'); }

    /* ---------------------------------------------------------------- ReadById */
    function readById(id) {
        RecId = toInt(id);
        return call('GET', '/' + RecId).then(function (r) {
            var h = r.header; if (!h) return;
            setSaveMode(false);
            $('#datPaymentDate').val(isoDate(h.DocDate)); var tm = /(\d{2}:\d{2}:\d{2})/.exec(String(h.DocDate || '')); docTime = tm ? tm[1] : '00:00:00';
            setCombo('#CmbSupplierName', h.SupplierCustomerId);
            $('#txtdocno').val(String(h.DocNo == null ? '' : h.DocNo)); $('#txtTaxPercent').val(String(h.TaxPrct == null ? '' : h.TaxPrct)); $('#txtTaxAmount').val(String(h.TaxAmount == null ? '' : h.TaxAmount));
            setCombo('#CmbTaxType', toInt(h.TaxTypeId)); setCombo('#CmbTaxDrAc', h.TaxDebitAcId); setCombo('#CmbTaxCrAc', h.TaxCreditAcId);
            var wasOn = $('#ChkBoxWthHolding').prop('checked');
            if (toDouble(h.TaxPrct) > 0) { $('#ChkBoxWthHolding').prop('checked', true); if (!wasOn) whtChanged(); }
            VoucherHeadId = toInt(r.voucherHeadId);
            grdContract.setRows((r.details || []).map(function (d) {
                return { Id: d.RefDocNoId, DocumentTypeId: d.RefDocumentTypeId, PaymentTermsId: d.PaymentTermsId, OrderDate: fmtDMY(d.InvoiceDate), OrderNo: d.InvoiceNo, DueDate: d.DueDate,
                         PaymentTerm: d.PaymentTerm, OrderAmount: toDouble(d.BillAmount), ReceivedAmount: toDouble(d.TotalReceivedAmount), BalanceAmount: toDouble(d.BalanceAmount), TaxAmount: 0 };
            }));
            detail = (r.others || []).map(function (o) {
                var ag = toInt(o.TransAgainst); if (!ag) for (var k = 0; k < TRANS_AGAINST.length; k++) if (TRANS_AGAINST[k].T === o.TransAgainst) ag = TRANS_AGAINST[k].V;
                return { __uid: ++uidSeq, TransTypeId: toInt(o.TransType), TransAgainstId: ag, GLAccountId: toInt(o.GLAccountId), AccountTitle: o.AccountTitle, Amount: toDouble(o.Amount),
                         CheqNo: o.CheqNo == null ? '' : o.CheqNo, CheqDate: isoDate(o.CheqDate) || nowParts().date, Paytitle: o.PayTitle == null ? '' : o.PayTitle, Remarks: o.Remarks == null ? '' : o.Remarks };
            });
            bindDetail(); grandTotal();
            attSaved = (r.attachments || []).map(function (a) { return { Id: a.Id, name: a.Attachment, size: a.UploadedFileSizeMb, date: a.EntryDate }; }); attNew = [];
            selectTab(0);
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- history / tabs */
    function selectTab(i) {
        $('#pageForm').toggleClass('on', i === 0); $('#pageHistory').toggleClass('on', i === 1);
        $('#tabF').toggleClass('on', i === 0); $('#tabH').toggleClass('on', i === 1);
        if (i === 1) {                                                // tabControl1_SelectedIndexChanged
            var d = new Date(); d.setDate(d.getDate() - 30); $('#BillDateFrom, #DueDateFrom').val(ymd(d));
            historyFill();
        } else { grdContract.rerender(); grdDetail.rerender(); }
    }
    function historyFill() {
        var q = { supplierId: toInt(comboVal('#CmbCustomerHistory')), fromDocNo: toInt($('#txtInvoiceNoFrom').val().trim()), toDocNo: toInt($('#txtInvoiceNoTo').val().trim()) };
        if ($('#BillDateFrom').val()) q.from = $('#BillDateFrom').val() + ' 00:00:00';
        if ($('#BillDateTo').val()) q.to = $('#BillDateTo').val() + ' 23:59:59';
        return call('GET', '/history', q).then(function (r) {
            var rows = r.rows || [];
            if (rows.length === 0) { grdHistory.clear(); return; }
            grdHistory.setRows(rows.map(function (x) {
                return { Id: x.Id, DocumentTypeId: x.DocumentTypeId, DocDate: x.DocDate, DocNo: x.DocNo, PartyName: x.CustomerName, TaxDebitAc: x.TaxDebitAccount, TaxCreditAc: x.TaxCreditAccount,
                         TaxType: x.TaxName, 'Tax%': toDouble(x.TaxPrct), TaxAmount: toDouble(x.TaxAmount), ReceivedAmount: toDouble(x.ReceivedAmount), NoOfAttachemtns: x.NoOfAttachemtns };
            }));
        }).catch(function (e) { say(e.message); });
    }

    /* ---------------------------------------------------------------- attachments */
    function cardHtml(name, meta, extra, delAttr) {
        var ext = (String(name).split('.').pop() || '').toUpperCase().substring(0, 4);
        return '<div class="card" ' + extra + '><div class="pv">' + CJG.esc(ext) + '</div><div class="nm">' + CJG.esc(name) + '</div><div class="mt">' + CJG.esc(meta) + '</div>'
            + (delAttr ? '<button type="button" class="del" ' + delAttr + '>Delete</button>' : '') + '</div>';
    }
    function renderAtt() {
        var h = '';
        attSaved.forEach(function (a, i) { h += cardHtml(a.name, (a.size == null ? '' : a.size + ' MB'), 'data-s="' + i + '"', 'data-s="' + i + '"'); });
        attNew.forEach(function (a, i) { h += cardHtml(a.name, sizeMb(a.size) + ' MB (new)', 'data-n="' + i + '"', 'data-n="' + i + '"'); });
        $('#attCards').html(h);
        $('#attNote').text(attSaved.length + attNew.length + ' file(s) - new files are not stored by this screen (as on the desktop)');
    }
    function btnAttachment_Click() { renderAtt(); $('#ovAtt').addClass('on'); }
    function attachmentsOf(id) {                                     // GetNoofAttachmentsByRefDocumentTypeID(id, 29)
        viewId = id;
        return call('GET', '/' + id + '/attachments').then(function (r) {
            var rows = r.rows || [], h = '', mb = 0;
            rows.forEach(function (a) { mb += toDouble(a.UploadedFileSizeMb); h += cardHtml(a.Attachment, (a.UploadedFileSizeMb == null ? '' : a.UploadedFileSizeMb + ' MB'), 'data-id="' + a.Id + '"', ''); });
            $('#viewCards').html(h); $('#vFiles').text(rows.length); $('#vSize').text(Math.round(mb * 100) / 100); $('#ovView').addClass('on');
        }).catch(function (e) { say(e.message); });
    }
    function pickFiles(files) {
        var list = Array.prototype.slice.call(files || []);
        if (attNew.length + list.length > 10) { say('Select at most ten files at once'); return; }
        list.forEach(function (f) {
            if (f.size === 0 || f.size > 5 * 1024 * 1024) { say(f.name + ': Attachment must be between 1 byte and 5 MB'); return; }
            attNew.push({ name: f.name, size: f.size }); renderAtt();
        });
    }

    /* ---------------------------------------------------------------- print (CommonServices.VoucherReport_118) */
    function openBlank() { try { return window.open('', '_blank'); } catch (e) { return null; } }
    function print_Click() {
        var w = openBlank();
        if (VoucherHeadId === 0) { try { if (w && !w.closed) w.close(); } catch (e) { /* ignore */ } say('VoucherId Not Found'); return; }
        var url = '/reports/print/by-key/acc-118?id=' + VoucherHeadId;
        if (w && !w.closed) w.location.href = url; else window.open(url, '_blank');
    }

    /* ---------------------------------------------------------------- keys */
    var KEYS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+P', 'For Print'],
                ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Tran Type in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
                ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function makeShortCutKeys() {
        $('#keysTbl tbody').html(KEYS.map(function (k) { return '<tr><td>' + CJG.esc(k[0]) + '</td><td>' + CJG.esc(k[1]) + '</td></tr>'; }).join(''));
        $('#ovKeys').addClass('on');
    }
    function enterTab(e) {
        var t = e.target;
        if (!t || t.tagName !== 'INPUT' || t.type === 'checkbox' || t.type === 'file') return false;
        var els = $('#pageForm').find('input:visible:not(:disabled):not([type=checkbox]):not([type=file]), button:visible:not(:disabled)').toArray();
        var i = els.indexOf(t);
        if (i >= 0 && i < els.length - 1) { e.preventDefault(); els[i + 1].focus(); return true; }
        return false;
    }
    function onForm() { return $('#pageForm').hasClass('on'); }
    function closeForm() { if (window.history.length > 1) window.history.back(); }

    /* ---------------------------------------------------------------- wiring */
    $(function () {
        var n = nowParts();
        $('#datPaymentDate, #datcheqdate, #BillDateFrom, #BillDateTo, #DueDateFrom, #DueDateTo').val(n.date); docTime = n.time;
        showBal('#lblCustomer', '#txtCustomerBalance', false); showBal('#lblAdvance', '#txtcustomerAdvBalance', false); showBal('#lblAmount', '#txtbalance', false);
        $('#txtCheqNo').prop('disabled', true); $('#datcheqdate').prop('disabled', true);
        $('#tabF').on('click', function () { selectTab(0); });
        $('#tabH').on('click', function () { selectTab(1); });
        $('#btnNew').on('click', btnNew_Click);
        $('#btnRefresh').on('click', btnRefresh_Click);
        $('#btnsave').on('click', btnsave_Click);
        $('#btnUpdate').on('click', btnUpdate_Click);
        $('#print').on('click', print_Click);
        $('#btnAttachment').on('click', btnAttachment_Click);
        $('#btnShortCutKey').on('click', makeShortCutKeys);
        $('#keysClose').on('click', function () { $('#ovKeys').removeClass('on'); });
        $('#btnAdd').on('click', btnAdd_Click);
        $('#btnsearchhistory').on('click', historyFill);
        $('#ChkBoxWthHolding').on('change', whtChanged);
        $('#txtAmount').on('keydown', function (e) { if (!e.ctrlKey && !e.metaKey && !e.altKey && e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); });
        $('#txtRemarks').on('focus', function () { this.select(); });
        $('#attChoose').on('click', function () { $('#attFile').val('').trigger('click'); });
        $('#attFile').on('change', function () { pickFiles(this.files); });
        $('#attClose').on('click', function () { $('#ovAtt').removeClass('on'); });
        $('#viewClose').on('click', function () { $('#ovView').removeClass('on'); });
        $('#attCards').on('click', '.del', function (e) {
            e.stopPropagation(); var s = $(this).data('s'), nn = $(this).data('n');
            if (s != null) attSaved.splice(+s, 1); else if (nn != null) attNew.splice(+nn, 1);
            renderAtt();
        }).on('click', '.card', function () {
            var s = $(this).data('s'); if (s != null && RecId > 0 && attSaved[+s]) window.open(API + '/' + RecId + '/attachments/' + attSaved[+s].Id, '_blank');
        });
        $('#viewCards').on('click', '.card', function () { window.open(API + '/' + viewId + '/attachments/' + $(this).data('id'), '_blank'); });
        /* the column the user last clicked in a grid (CurrentColumn) */
        $('#grdDetail').on('click', 'td', function () { lastCol.detail = $(this).data('k') || $(this).find('button').data('col'); });
        $('#Gridhistory').on('click', 'td', function () { lastCol.hist = $(this).data('k') || $(this).find('button').data('col'); });
        /* grdDetail_KeyDown */
        $('#grdDetail').on('keydown', function (e) {
            var item = grdDetail.current(); if (!item || $(e.target).is('input')) return;
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); grdDetail_DoubleClick(item); }
            else if (e.ctrlKey && e.key === ' ') { if (lastCol.detail === 'Delete' && saveMode()) { e.preventDefault(); detailDelete(item); } }
            else if (e.ctrlKey && e.key === 'Delete' && saveMode()) { e.preventDefault(); detailDelete(item); }
        });
        /* Gridhistory_KeyDown */
        $('#Gridhistory').on('keydown', function (e) {
            var item = grdHistory.current(); if (!item || $(e.target).is('input')) return;
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); readById(toInt(item.Id)); }
            else if (e.ctrlKey && e.key === ' ') {
                e.preventDefault();
                if (lastCol.hist === 'Edit') readById(toInt(item.Id));
                if (lastCol.hist === 'NoOfAttachemtns') attachmentsOf(toInt(item.Id));
            }
        });
        /* ReceiptByInvoiceAccount_KeyDown */
        $(document).on('keydown', function (e) {
            if ($('.ov.on').length) { if (e.key === 'Escape') $('.ov.on').removeClass('on'); return; }
            if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) { if (enterTab(e)) return; }
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey && onForm() && (k === 'control' || k === 'alt')) { makeShortCutKeys(); return; }
            if (e.ctrlKey && !e.altKey) {
                if (k === 'e') { e.preventDefault(); closeForm(); }
                else if (k === 'n') { e.preventDefault(); btnNew_Click(); }
                else if (k === 'r') { e.preventDefault(); btnRefresh_Click(); }
                else if (k === 's') { e.preventDefault(); if (onForm()) { if (saveMode()) btnsave_Click(); } else historyFill(); }
                else if (k === 'u' && !saveVisible() && !$('#btnUpdate').prop('disabled')) { e.preventDefault(); insert(null); }
                else if (k === 'p' && onForm() && rights && rights.canPrint) { e.preventDefault(); print_Click(); }
                else if (k === 'f10' && onForm()) { e.preventDefault(); btnAttachment_Click(); }
                else if (k === 'arrowdown' && onForm()) { e.preventDefault(); $('#grdDetail').focus(); }
                else if (k === 'arrowup' && onForm()) { e.preventDefault(); focusCombo('#CmbTranagainst'); }
                else if (k === 't') { e.preventDefault(); if (!onForm()) { selectTab(0); $('#datPaymentDate').focus(); } else { selectTab(1); $('#BillDateFrom').focus(); } }
            } else if (e.key === 'Escape') closeForm();
        });
        /* ReceiptByInvoice_Load */
        AccF.fillSelect('#cmbTransType', TRANS_TYPES, 'V', 'T', null); activateRow('#cmbTransType', 0);
        AccF.fillSelect('#CmbTranagainst', TRANS_AGAINST, 'V', 'T', null); activateRow('#CmbTranagainst', 0);
        setSaveMode(true);
        grdContract.setRows([]); bindDetail();
        call('GET', '/load').then(function (r) { applyLoad(r); $('#datPaymentDate').focus(); }).catch(function (e) { say(e.message); });
    });
})(window.jQuery);
