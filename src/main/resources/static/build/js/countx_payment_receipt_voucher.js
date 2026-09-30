/* ============================================================================================
 * Screens 28-31 - Cash Payment (1), Bank Payment (2), Cash Receipt (3), Bank Receipt Voucher (4).
 *
 * Desktop: Architecture.WinApp.Account_Definition.PaymentVoucherNew (1/2) and
 *          Architecture.WinApp.Account_Definition.ReceiptsVoucherNew (3/4) - the NON-tax forms
 *          (ScreenDefinition TargetUrl; CommonServices.EditMethodFromLinked).
 *
 * The Form tab is rendered by this file (one source for the four pages); the History tab stays in
 * each page (USP_VoucherFormHistory). Backend: /accounts/api/payment-receipt/{doc}/...
 * (PaymentReceiptVoucherController) - lookups from the desktop's own fill methods, Save/Update
 * through the desktop procedure chain (DesktopVoucherWriter), ReadById through
 * Sp_Vouchers_GetMethods 'ReadByID' / 'VoucherDetail_ReadByVoucherHeadID'.
 *
 * The page posts grid ROWS; the ledger lines (vd/vd2/vd3/vd4) are built on the server.
 * ============================================================================================ */
(function (w, $) {
    'use strict';

    var API = '/accounts/api/payment-receipt';
    var ROUTE = { 1: 'cash-payment', 2: 'bank-payment', 3: 'cash-receipt', 4: 'bank-receipt' };
    var TITLE = { 1: 'Cash Payment Voucher', 2: 'Bank Payment Voucher', 3: 'Cash Receipt Voucher', 4: 'Bank Receipt Voucher' };

    var S = {
        doc: 0, pay: false, L: null, F: {}, rows: [], recId: 0, updateMode: false,
        editIndex: -1, cheques: [], busy: false
    };
    /* print-rpt.js reads the open voucher's id (S.recId == VoucherHeadId) for the 102 formats */
    try { Object.defineProperty(w, 'RecId', { get: function () { return S.recId; }, configurable: true });
          Object.defineProperty(w, 'VoucherHeadId', { get: function () { return S.recId; }, configurable: true }); } catch (e) { }

    if (w.DesktopCombo) {
        /* DetailAccountFill -> GetAccountsFromGlobalByTypeIds: Id | AccountTitle | AccountCode |
           ParentAccountTitle | AccountClass, Id hidden. */
        w.DesktopCombo.define('prvAcc4', [
            { caption: 'Account Title', flex: 4 },
            { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' },
            { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('prvAcc2', [
            { caption: 'Account Title', flex: 4 },
            { caption: 'AccountCode', flex: 2, key: 'code' }
        ]);
    }

    function esc(v) { return $('<div>').text(v == null ? '' : String(v)).html(); }
    function ci(o, k) {
        if (!o) return undefined;
        if (k in o) return o[k];
        var lk = k.toLowerCase();
        for (var p in o) if (p.toLowerCase() === lk) return o[p];
        return undefined;
    }
    function num(v) { var n = parseFloat(String(v == null ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function dec() { return Math.max(int(S.F.amountDecimals), 0); }
    function fmt(v) { return num(v).toLocaleString('en-US', { minimumFractionDigits: dec(), maximumFractionDigits: dec() }); }
    function round(v, d) { var p = Math.pow(10, d); return Math.round((v + (v >= 0 ? 1e-9 : -1e-9)) * p) / p; }
    function today() { var d = new Date(); d.setMinutes(d.getMinutes() - d.getTimezoneOffset()); return d.toISOString().slice(0, 10); }
    function day(v) { return v ? String(v).slice(0, 10) : ''; }
    function opts(list, idKey, textKey, blank) {
        var h = blank ? '<option value="0"></option>' : '';
        (list || []).forEach(function (r) { h += '<option value="' + esc(ci(r, idKey)) + '">' + esc(ci(r, textKey)) + '</option>'; });
        return h;
    }
    function accOpts(list, blank) {
        var h = blank ? '<option value="0"></option>' : '';
        (list || []).forEach(function (a) {
            h += '<option value="' + esc(ci(a, 'id')) + '" data-code="' + esc(ci(a, 'accountCode')) + '" data-parent="' +
                esc(ci(a, 'parentAccountTitle')) + '" data-cls="' + esc(ci(a, 'accountClass')) + '">' + esc(ci(a, 'accountTitle')) + '</option>';
        });
        return h;
    }
    function selText(id) { var o = $('#' + id + ' option:selected'); return o.length && o.val() !== '0' ? o.text() : ''; }
    function setVal(id, v) { var $s = $('#' + id); $s.val(String(v == null ? 0 : v)); if ($s.val() == null) $s.val($s.find('option:first').val()); $s.trigger('change'); }

    // ------------------------------------------------------------------------------ markup

    function field(label, html, id, extra) {
        return '<div class="prv-f"' + (id ? ' id="' + id + '"' : '') + (extra || '') + '><label class="win-label">' + label + '</label>' + html + '</div>';
    }

    function render() {
        var d = S.doc, pay = S.pay;
        var h = '';
        h += '<div class="win-section-header">Main</div><div class="prv-grid" style="padding:8px 10px;">';
        h += field(pay ? 'Cost Center' : 'Project', '<select id="prvProject" class="win-input"></select>');
        h += field('Voucher Type', '<select id="prvVoucherType" class="win-input" disabled><option value="' + d + '">' + TITLE[d] + '</option></select>');
        h += field('Voucher No', '<input type="text" id="prvVoucherNo" class="win-input" readonly style="font-weight:bold;"/>');
        h += field('Date', '<input type="date" id="prvDate" class="win-input"/>');
        h += field(pay ? 'Credit Account' : 'Debit Account',
            '<select id="prvRefAccount" class="win-input" data-dtcombo="prvAcc2" data-dtcombo-caption="' + (pay ? 'Credit Account' : 'Debit Account') + '"></select>' +
            '<span id="prvRefBalance" style="font-weight:bold;color:#b71c1c;display:none;"></span>');
        if (!pay && d === 4) {
            h += field('Cheque No', '<input type="text" id="prvHdrChequeNo" class="win-input"/>');
            h += field('Cheque Date', '<input type="date" id="prvHdrChequeDate" class="win-input"/>');
            h += field('PayTitle', '<input type="text" id="prvHdrPayTitle" class="win-input"/>');
        }
        h += field('Fcy Code', '<select id="prvCurrency" class="win-input"></select>', 'prvFcyCodeWrap');
        h += field('Exchange Rate', '<input type="text" id="prvExchangeRate" class="win-input" style="text-align:right;"/>', 'prvRateWrap');
        h += field('Fcy Amount', '<input type="text" id="prvFcyAmount" class="win-input" readonly style="text-align:right;"/>', 'prvFcyAmtWrap');
        h += field('Remarks', '<textarea id="prvRemarks" class="win-textarea"></textarea>', null, ' style="grid-column: span 2;"');
        h += '</div>';

        h += '<div class="win-section-header">Detail</div><div style="padding:8px 10px;"><div class="detail-entry-bar"><div class="prv-grid">';
        h += field('Payment Type', '<select id="prvPaymentType" class="win-input"></select>');
        h += field(pay ? 'Debit Acc' : 'Credit Acc',
            '<select id="prvAccount" class="win-input" data-dtcombo="prvAcc4" data-dtcombo-caption="Account Title"></select>' +
            '<span id="prvAccBalance" style="font-weight:bold;color:#b71c1c;display:none;"></span>');
        h += field('Job/Lot', '<select id="prvJobLot" class="win-input"></select>');
        if (d === 2) {
            h += field('Financial Instrument', '<select id="prvInstrument" class="win-input"></select>');
            h += field('Cheque No', '<input type="text" id="prvChequeNo" class="win-input" list="prvChequeList" autocomplete="off"/><datalist id="prvChequeList"></datalist>');
            h += field('Cheque Date', '<input type="date" id="prvChequeDate" class="win-input"/>');
            h += field('Cheque Type', '<select id="prvChequeType" class="win-input"></select>');
            h += field('Payee Title', '<input type="text" id="prvPayeeTitle" class="win-input"/>');
        }
        h += field('Branch Name', '<select id="prvBranch" class="win-input"></select>', 'prvBranchWrap', ' style="display:none;"');
        h += field('Cost Center', '<select id="prvCostCenter" class="win-input"></select>', 'prvCostCenterWrap', ' style="display:none;"');
        h += field('Remarks', '<input type="text" id="prvLineRemarks" class="win-input"/>', null, ' style="grid-column: span 2;"');
        h += field(pay ? 'Debit Amount' : 'Credit Amount', '<input type="text" id="prvAmount" class="win-input" style="text-align:right;font-weight:bold;"/>');
        h += '<div class="prv-f" style="display:flex;align-items:flex-end;gap:4px;">' +
             '<button type="button" id="prvAdd" class="win-btn win-btn-teal">+</button>' +
             '<button type="button" id="prvUpdateDetail" class="win-btn" style="display:none;">Update</button>' +
             '<button type="button" id="prvCancelDetail" class="win-btn" style="display:none;">Cancel</button></div>';
        h += '</div><div id="prvSubsidiaryNote" style="display:none;color:#b71c1c;margin-top:4px;">' +
             'Subsidiary A/c (ERP feature 4) is switched on for this company; the web form does not offer it yet, so rows are saved without a subsidiary account.</div></div>';

        h += '<div style="max-height:300px;overflow:auto;margin-top:8px;"><table class="win-grid-table" id="prvGrid"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';

        // WHT + totals
        h += '<div class="wht-bar"><div class="prv-grid">';
        h += '<div class="prv-f"><label style="font-weight:bold;cursor:pointer;"><input type="checkbox" id="prvWht"/> WHT</label></div>';
        if (pay) {
            h += '<div class="prv-f"><label style="font-weight:normal;margin-right:8px;"><input type="radio" name="prvTaxMode" value="excluded" id="prvRadExcluded" checked/> Excluded Tax</label>' +
                 '<label style="font-weight:normal;"><input type="radio" name="prvTaxMode" value="included" id="prvRadIncluded"/> Included Tax</label></div>';
        }
        h += field('TaxType', '<select id="prvTaxType" class="win-input"></select>');
        h += field('Tax %', '<input type="text" id="prvTaxPercent" class="win-input" readonly value="0" style="text-align:right;"/>');
        h += field(pay ? 'WHT Debit Ac' : 'Against Ac', '<select id="prvAgainstAc" class="win-input" data-dtcombo="prvAcc2" disabled></select>');
        h += field(pay ? 'WHT Credit Account' : 'WHT Account', '<select id="prvWithHoldingAc" class="win-input" data-dtcombo="prvAcc2" disabled></select>');
        h += '</div></div>';
        h += '<div class="totals-bar"><div>Amount: <span id="prvValue">0</span></div><div>Tax Amount: <span id="prvTaxAmount">0</span></div>' +
             '<div>Total Amount: <span id="prvTotalAmount">0</span></div>' +
             '<div><label style="font-weight:normal;cursor:pointer;"><input type="checkbox" id="prvChkPrint1" checked/> Print Preview</label></div></div>';
        h += '</div>';
        h += '<style>.prv-grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(170px,1fr));gap:6px 10px;}' +
             '.prv-f .win-textarea{height:25px;}#prvGrid td.num,#prvGrid th.num{text-align:right;}</style>';
        $('#prvForm').html(h);
    }

    // ------------------------------------------------------------------------------ lookups

    function bindLookups(L) {
        S.L = L; S.F = L.flags || {};
        $('#prvProject').html(opts(L.projects, 'Id', 'ProjectName', false));
        $('#prvRefAccount').html(accOpts(L.headerAccounts, true));
        $('#prvAccount').html(accOpts(L.detailAccounts, true));
        $('#prvAgainstAc, #prvWithHoldingAc').html(accOpts(L.whtAccounts, true));
        $('#prvPaymentType').html(opts(L.paymentTypes, 'Id', 'PaymentType', false));
        $('#prvJobLot').html(opts(L.jobLots, 'Id', 'JobLotDescription', true));
        $('#prvCurrency').html(opts(L.currencies, 'Id', 'CurrencyCode', true));
        $('#prvTaxType').html(opts(L.taxTypes, 'Id', 'TaxName', false));
        $('#prvBranch').html(opts(L.branches, 'BranchId', 'BranchName', false));
        $('#prvCostCenter').html(opts(L.costCenters, 'Id', 'CostCenterName', false));
        if (S.doc === 2) {
            $('#prvInstrument').html(opts(L.instrumentTypes, 'Id', 'InstrumentType', false));
            $('#prvChequeType').html(opts(L.chequeTypes, 'id', 'CheqType', false));
        }
        // MultiCurrencyFeature(): the Fcy trio is hidden when feature 6 is off (values still posted).
        var mc = !!S.F.multiCurrencyFeature;
        $('#prvFcyCodeWrap, #prvRateWrap, #prvFcyAmtWrap').toggle(mc);
        $('#prvBranchWrap').toggle(!!S.F.branchFeature);
        $('#prvCostCenterWrap').toggle(!!S.F.isBookingOffice);
        $('#prvSubsidiaryNote').toggle(!!S.F.subsidiaryFeature);
        if (w.$ && $('#lblPrintOnSave').length) $('#lblPrintOnSave').toggle(!!S.F.chequePrintingEnable && S.pay);
        var r = S.F.rights || {};
        $saveBtn().prop('disabled', !r.save);
        renderGridHead();
        reset(true);
    }

    function $saveBtn() {
        var b = $('#btnSaveUpdate');
        return b.length ? b : $('button[onclick="submitVoucher()"]').first();
    }

    /** DefaultConfigurations(): Job/Lot, Base Currency, BaseCurrencyRate. */
    function defaults() {
        if (int(S.F.defaultJobLotId) > 0) setVal('prvJobLot', S.F.defaultJobLotId);
        if (int(S.F.baseCurrencyId) > 0) setVal('prvCurrency', S.F.baseCurrencyId);
        if (S.F.baseCurrencyRate != null) $('#prvExchangeRate').val(String(S.F.baseCurrencyRate));
    }

    // ------------------------------------------------------------------------------ grid

    function cols() {
        var c = [{ k: 'x', t: 'X' }, { k: 'paymentType', t: 'PaymentType' }, { k: 'accountCode', t: 'AccountCode' },
                 { k: 'accountTitle', t: S.pay ? 'AccountTitle' : 'AccountId' }, { k: 'jobLot', t: 'JobLotId' },
                 { k: 'remarks', t: 'Remarks' }, { k: 'amount', t: S.pay ? 'Debit Amount' : 'Credit Amount', n: 1 }];
        if (S.doc === 2) c = c.concat([{ k: 'financialInstrument', t: 'FinancialInstrument' }, { k: 'chequeDate', t: 'ChequeDate' },
                                       { k: 'chequeNo', t: 'ChequeNo' }, { k: 'payeeTitle', t: 'PayeeTitle' }]);
        if (S.F.multiCurrencyFeature) c.push({ k: 'fcyAmount', t: 'FcyAmount', n: 1 });
        if (S.F.branchFeature) c.push({ k: 'branchName', t: 'BranchName' });
        if (S.F.isBookingOffice) c.push({ k: 'costCenterName', t: 'CostCenterId' });
        if (S.pay && $('#prvRadIncluded').is(':checked')) c.push({ k: 'taxAmount', t: 'TaxAmount', n: 1 });
        if (!S.pay) { c.shift(); c.push({ k: 'x', t: 'X' }); }   // ReceiptsVoucherNew adds Delete last, no Position
        return c;
    }

    function renderGridHead() {
        $('#prvGrid thead').html('<tr>' + cols().map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + '>' + c.t + '</th>'; }).join('') + '</tr>');
    }

    function renderGrid() {
        renderGridHead();
        var cs = cols(), rate = num($('#prvExchangeRate').val());
        var html = '';
        S.rows.forEach(function (r, i) {
            r.fcyAmount = rate > 0 ? r.amount / rate : 0;           // txtExchangeRate_TextChanged
            html += '<tr data-i="' + i + '">' + cs.map(function (c) {
                if (c.k === 'x') return '<td style="text-align:center;"><button type="button" class="btn btn-xs btn-danger prv-del" data-i="' + i + '">X</button></td>';
                var v = r[c.k];
                if (c.k === 'amount' || c.k === 'taxAmount') v = fmt(v);
                else if (c.k === 'fcyAmount') v = num(v).toLocaleString('en-US', { maximumFractionDigits: 3 });
                return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $('#prvGrid tbody').html(html || '<tr><td colspan="' + cs.length + '" style="text-align:center;color:#888;padding:14px;">No rows</td></tr>');
        var sum = S.rows.reduce(function (s, r) { return s + r.amount; }, 0);
        $('#prvGrid tfoot').html('<tr>' + cs.map(function (c) { return '<td class="num" style="font-weight:bold;">' + (c.k === 'amount' ? fmt(sum) : '') + '</td>'; }).join('') + '</tr>');
        // CalculateTotalInformation(): txtFcyAmount = SUM(FcyAmount), "#,##0.###"
        var fcy = S.rows.reduce(function (s, r) { return s + r.fcyAmount; }, 0);
        $('#prvFcyAmount').val(S.rows.length ? round(fcy, 3) : 0);
    }

    // ------------------------------------------------------------------------------ totals / WHT

    function baseAmount() { return S.rows.reduce(function (s, r) { return s + r.amount; }, 0); }

    /** Total(). */
    function total() {
        if (S.pay) {
            var base = baseAmount(), rate = num($('#prvTaxPercent').val()), tax, tot;
            if (!$('#prvRadIncluded').is(':checked')) { tax = base / (100 - rate) * rate; tot = base + tax; }
            else { var n = base; base = n / 100 * (100 - rate); tot = n; tax = tot - base; }
            showTotals(base, tax, tot);
            proportion();
        } else {
            var amt = baseAmount();
            if (amt > 0) {
                var t = num($('#prvTaxAmount').data('v'));
                $('#prvValue').text(fmt(amt)).data('v', round(amt, dec()));
                $('#prvTotalAmount').text(fmt(amt + t)).data('v', round(amt + t, dec()));
            }
        }
    }

    function showTotals(value, tax, tot) {
        $('#prvValue').text(fmt(value)).data('v', round(value, dec()));
        $('#prvTaxAmount').text(fmt(tax)).data('v', round(tax, dec()));
        $('#prvTotalAmount').text(fmt(tot)).data('v', round(tot, dec()));
    }

    /** TaxAmountProportion(): row TaxAmount only in Included mode. */
    function proportion() {
        var inc = $('#prvRadIncluded').is(':checked');
        var totalTax = round(num($('#prvTaxAmount').data('v')), dec()), totalDetail = baseAmount();
        S.rows.forEach(function (r) {
            r.taxAmount = (inc && totalTax > 0 && totalDetail > 0) ? round(totalTax / totalDetail * r.amount, dec()) : 0;
        });
        renderGrid();
    }

    function whtEnable(on) { $('#prvAgainstAc, #prvWithHoldingAc').prop('disabled', !on); }

    /** checkBox1_CheckedChanged() of each form (also CmbTaxType_Leave). */
    function whtChanged() {
        var base = baseAmount();
        if (!$('#prvWht').is(':checked')) {
            whtEnable(false);
            $('#prvTaxPercent').val('0');
            if (S.pay) { showTotals(base, 0, base); proportion(); }
            else {
                setVal('prvWithHoldingAc', 0);
                $('#prvTaxAmount').text(fmt(0)).data('v', 0);
                $('#prvTotalAmount').text(fmt(num($('#prvValue').data('v')))).data('v', num($('#prvValue').data('v')));
            }
            return $.Deferred().resolve().promise();
        }
        return $.getJSON(API + '/tax-schedule', { taxTypeId: int($('#prvTaxType').val()), date: $('#prvDate').val() }).then(function (s) {
            if (!s || !s.found) {
                whtEnable(false);
                setVal('prvWithHoldingAc', 0);
                if (S.pay) setVal('prvAgainstAc', 0);
                $('#prvTaxPercent').val('0');
                $('#prvTaxAmount').text(fmt(0)).data('v', 0);
                if (S.pay) { $('#prvTotalAmount').text($('#prvValue').text()).data('v', num($('#prvValue').data('v'))); proportion(); }
                else whtEnable(true);   // ReceiptsVoucherNew enables both combos after either branch
                return;
            }
            var p = num(s.taxPercent);
            $('#prvTaxPercent').val(S.pay ? String(round(p, 4)) : String(p));
            if (!S.updateMode) setVal('prvWithHoldingAc', s.taxGLAccountId);
            if (S.pay) {
                var tax, tot, value = base;
                if (!$('#prvRadIncluded').is(':checked')) { tax = round(base / (100 - p) * p, 2); tot = base + tax; }
                else { tot = base; value = round(tot / (1 + p / 100), 2); tax = tot - value; }
                showTotals(value, tax, tot);
                whtEnable(true);
                if (S.rows.length && int($('#prvAgainstAc').val()) === 0) setVal('prvAgainstAc', S.rows[0].accountId);
                proportion();
            } else {
                var v = num($('#prvValue').data('v'));
                var t = v / (100 - p) * 100 * p / 100;
                $('#prvTaxAmount').text(fmt(t)).data('v', t);
                $('#prvTotalAmount').text(fmt(t + v)).data('v', t + v);
                whtEnable(true);
            }
        });
    }

    // ------------------------------------------------------------------------------ detail entry

    function balance(accountId, $lbl) {
        if (!(accountId > 0)) { $lbl.hide().text('0'); return; }
        $.getJSON(API + '/balance', { accountId: accountId, date: $('#prvDate').val() }).then(function (r) {
            var b = num(r && r.balance);
            $lbl.text(b < 0 ? '(' + Math.abs(Math.round(b)).toLocaleString('en-US') + ')' : Math.round(b).toLocaleString('en-US')).show();
        });
    }

    /** CheqNoFill(): BPV only, only with "CheqBook Enabled". */
    function chequeFill() {
        S.cheques = [];
        $('#prvChequeList').empty();
        var bank = int($('#prvRefAccount').val());
        if (S.doc !== 2 || !S.F.chequeBookEnabled || !(bank > 0)) return;
        $.getJSON('/accounts/api/vouchers/outstanding-cheques', { bankId: bank, recId: S.recId }).then(function (list) {
            S.cheques = list || [];
            $('#prvChequeList').html(S.cheques.map(function (c) { return '<option value="' + esc(c.cheqNo) + '"></option>'; }).join(''));
        });
    }
    function chequeIdFor(no) {
        for (var i = 0; i < S.cheques.length; i++) if (String(S.cheques[i].cheqNo) === String(no)) return int(S.cheques[i].id);
        return 0;
    }

    /** CheckingChequeNoSerialWise(). */
    function chequeSerial(no) {
        if (!S.cheques.length) return null;
        var all = S.cheques.map(function (c) { return int(c.cheqNo); });
        var n = int(no);
        if (!S.rows.length) return n !== Math.min.apply(null, all) ? 'Please Insert Cheque No In Detail Grid Serial Wise' : null;
        var distinct = S.rows.map(function (r) { return int(r.chequeNo); }).filter(function (v, i, a) { return a.indexOf(v) === i; });
        if (distinct.length === 1) {
            var rest = all.filter(function (v) { return v !== distinct[0]; });
            return rest.length && n > Math.min.apply(null, rest) ? 'Please Insert Cheque No In Detail Grid Serial Wise' : null;
        }
        var min = Math.min.apply(null, distinct), max = Math.max.apply(null, distinct);
        if (n < max && S.editIndex < 0) return 'Please Insert Cheque No In Detail Grid Serial Wise! You Inserting ' + no + ' ChequeNo And in Grid ' + max + ' ChequeNo Is Present';
        var outside = all.filter(function (v) { return v < min || v > max; });
        var min3 = outside.length ? Math.min.apply(null, outside) : Infinity;
        if (n >= min && n < max && S.editIndex < 0) return 'Please Insert Cheque No In Detail Grid Serial Wise';
        if (n > min3) return 'Please Insert Cheque No In Detail Grid Serial Wise';
        return null;
    }

    /** FormValidationDetail() (payment) / Add_Click_1 checks (receipt). */
    function detailValid() {
        var pay = S.pay;
        if (!$('#prvPaymentType').val()) return pay ? 'PaymentType Field is Required' : 'PaymentType Field Required';
        if (pay && int($('#prvRefAccount').val()) === 0) return 'Credit Account Field is Required';
        if (int($('#prvAccount').val()) === 0) return pay ? 'Debit Account Field is Required' : 'Credit Account Field Required';
        if (int($('#prvJobLot').val()) === 0) return pay ? 'Job/Lot Field is Required' : 'Job/Lot Field Required';
        if (S.doc === 2) {
            if (int($('#prvInstrument').val()) === 0) return 'Financial Instrument Field is Required';
            if (int($('#prvInstrument').val()) === 1 && S.F.chequeNoCompulsoryOnBpv && chequeIdFor($('#prvChequeNo').val()) === 0) return 'Cheque_number Field is Required';
        }
        if ($.trim($('#prvAmount').val()) === '' || num($('#prvAmount').val()) === 0) return pay ? 'Amount Field is Required' : 'Amount Field Required';
        if (S.doc === 2 && int($('#prvInstrument').val()) === 1 && int($('#prvChequeType').val()) === 0) return 'Cheque Type Field is Required';
        if (S.F.branchFeature && int($('#prvBranch').val()) === 0) return 'BranchName Field is Required';
        return null;
    }

    function rowFromEntry() {
        var accOpt = $('#prvAccount option:selected');
        var r = {
            paymentTypeId: int($('#prvPaymentType').val()), paymentType: selText('prvPaymentType'),
            accountId: int($('#prvAccount').val()), accountCode: accOpt.attr('data-code') || '', accountTitle: accOpt.text(),
            jobLotId: int($('#prvJobLot').val()), jobLot: selText('prvJobLot'),
            remarks: $('#prvLineRemarks').val(), amount: num($('#prvAmount').val()), taxAmount: 0,
            branchId: S.F.branchFeature ? int($('#prvBranch').val()) : 0, branchName: S.F.branchFeature ? selText('prvBranch') : '',
            costCenterId: S.F.isBookingOffice ? int($('#prvCostCenter').val()) : 0, costCenterName: S.F.isBookingOffice ? selText('prvCostCenter') : ''
        };
        if (S.pay) {
            // CPV rows carry the (hidden) cheque date picker's value too - Add_Click_1 always adds it.
            r.chequeDate = S.doc === 2 ? ($('#prvChequeDate').val() || today()) : today();
        }
        if (S.doc === 2) {
            r.financialInstrumentId = int($('#prvInstrument').val()); r.financialInstrument = selText('prvInstrument');
            r.chequeNo = $('#prvChequeNo').val(); r.chequeId = chequeIdFor(r.chequeNo);
            r.payeeTitle = $('#prvPayeeTitle').val(); r.chequeTypeId = int($('#prvChequeType').val());
        }
        return r;
    }

    function add() {
        var e = detailValid();
        if (e) { alert(e); return; }
        if (S.doc === 2 && S.F.chequePostingSerialWise && int($('#prvInstrument').val()) === 1) {
            var se = chequeSerial($('#prvChequeNo').val());
            if (se) { alert(se); return; }
        }
        var r = rowFromEntry();
        if (S.editIndex >= 0) S.rows[S.editIndex] = r; else S.rows.push(r);
        if (S.doc === 2 && r.chequeId > 0) $('#prvRefAccount').prop('disabled', true);   // Add_Click_1: lock CmbCreditAccount once a leaf is used
        $('#prvAccBalance').hide();
        resetDetail();
        if (S.pay && S.rows.length && int($('#prvAgainstAc').val()) === 0) setVal('prvAgainstAc', S.rows[0].accountId);
    }

    function resetDetail() {
        S.editIndex = -1;
        $('#prvAdd').show(); $('#prvUpdateDetail, #prvCancelDetail').hide();
        $('#prvAmount').val('');
        if (S.doc === 2) { $('#prvChequeNo').val(''); $('#prvPayeeTitle').val(''); }
        total();
        if (!S.pay) whtChanged();       // txtValue.TextChanged -> checkBox1_CheckedChanged
        $('#prvAccount').focus();
    }

    function editRow(i) {
        var r = S.rows[i];
        S.editIndex = i;
        setVal('prvPaymentType', r.paymentTypeId); setVal('prvAccount', r.accountId); setVal('prvJobLot', r.jobLotId);
        $('#prvLineRemarks').val(r.remarks); $('#prvAmount').val(r.amount);
        if (S.F.branchFeature) setVal('prvBranch', r.branchId);
        if (S.F.isBookingOffice) setVal('prvCostCenter', r.costCenterId);
        if (S.doc === 2) {
            setVal('prvInstrument', r.financialInstrumentId); $('#prvChequeNo').val(r.chequeNo); $('#prvChequeDate').val(day(r.chequeDate));
            setVal('prvChequeType', r.chequeTypeId); $('#prvPayeeTitle').val(r.payeeTitle);
        }
        $('#prvAdd').hide(); $('#prvUpdateDetail, #prvCancelDetail').show();
    }

    // ------------------------------------------------------------------------------ reset / load

    /** Reset() (and Load's initial state). */
    function reset(first) {
        S.rows = []; S.recId = 0; S.updateMode = false; S.editIndex = -1;
        $('#prvRefAccount').prop('disabled', false);
        $('#prvDate').val(today());
        if (S.doc === 4) { $('#prvHdrChequeNo').val(''); $('#prvHdrChequeDate').val(today()); $('#prvHdrPayTitle').val(''); }
        if (S.doc === 2) $('#prvChequeDate').val(today());
        $('#prvRemarks').val(''); $('#prvLineRemarks').val('');
        $('#prvWht').prop('checked', false);
        setVal('prvAgainstAc', 0); setVal('prvWithHoldingAc', 0);
        $('#prvTaxPercent').val('0');
        $('#prvTaxAmount').text(fmt(0)).data('v', 0); $('#prvValue').text(fmt(0)).data('v', 0); $('#prvTotalAmount').text(fmt(0)).data('v', 0);
        if (first) {
            $('#prvProject option:first').prop('selected', true); $('#prvProject').trigger('change');   // Rows[0].Activate()
            $('#prvPaymentType option:first').prop('selected', true); $('#prvPaymentType').trigger('change');
            if (S.doc === 1 || S.doc === 3) {                                  // Rows[1].Activate() on CPV / CRV
                var o = $('#prvRefAccount option').eq(1);
                if (o.length) setVal('prvRefAccount', o.val());
            }
            defaults();
        }
        $('#prvVoucherNo').val(S.L ? S.L.nextCode : '');
        if (!first) $.getJSON(API + '/' + S.doc + '/next-code').then(function (r) { $('#prvVoucherNo').val(r.voucherCode); });
        $saveBtn().html('<i class="fa fa-save"></i> Save').prop('disabled', !((S.F.rights || {}).save));
        whtEnable(false);
        resetDetail();
    }

    function loadForEdit(id) {
        return $.getJSON(API + '/' + S.doc + '/' + id).then(function (v) {
            if (!v) return;
            if (int(v.documentTypeId) !== S.doc) {                 // opened from a paired history -> the right screen
                w.location.href = '/accounts/vouchers/' + ROUTE[int(v.documentTypeId)] + '?id=' + id;
                return;
            }
            if (typeof w.switchTab === 'function') w.switchTab('form');
            S.recId = int(v.id); S.updateMode = true;
            $('#prvVoucherNo').val(v.voucherCode);
            $('#prvDate').val(day(v.voucherDate));
            setVal('prvRefAccount', v.refAccountId);
            $('#prvRemarks').val(v.remarks || '');
            setVal('prvProject', v.projectId);
            setVal('prvCurrency', v.multiCurrencyId);
            $('#prvExchangeRate').val(v.exchangeCurrencyRate);
            if (int(v.multiCurrencyId) === 0 || num(v.exchangeCurrencyRate) === 0) defaults();
            if (S.doc === 4) { $('#prvHdrPayTitle').val(v.payTitle || ''); $('#prvHdrChequeNo').val(v.chequeNo || ''); $('#prvHdrChequeDate').val(day(v.chequeDate)); }
            if (S.pay) { $('#prvRadIncluded').prop('checked', !!v.inclusiveTax); $('#prvRadExcluded').prop('checked', !v.inclusiveTax); }
            S.rows = (v.rows || []).map(function (r) {
                return {
                    paymentTypeId: int(r.paymentTypeId), paymentType: r.paymentType || '', accountId: int(r.accountId),
                    accountCode: r.accountCode || '', accountTitle: r.accountTitle || '', jobLotId: int(r.jobLotId), jobLot: r.jobLot || '',
                    remarks: r.remarks || '', amount: num(r.amount), taxAmount: num(r.taxAmount), branchId: int(r.branchId),
                    branchName: r.branchName || '', costCenterId: int(r.costCenterId), costCenterName: r.costCenterName || '',
                    financialInstrumentId: int(r.financialInstrumentId), financialInstrument: r.financialInstrument || '',
                    chequeDate: day(r.chequeDate), chequeId: int(r.chequeId), chequeNo: r.chequeNo || '', payeeTitle: r.payeeTitle || '',
                    chequeTypeId: int(r.chequeTypeId)
                };
            });
            var wh = v.wht || {};
            if (v.includeWHT) {
                $('#prvWht').prop('checked', true);
                whtEnable(true);
                if (wh.againstAcId != null) setVal('prvAgainstAc', wh.againstAcId);
                if (wh.withHoldingAcId != null) setVal('prvWithHoldingAc', wh.withHoldingAcId);
                if (wh.taxTypeId != null) setVal('prvTaxType', wh.taxTypeId);
                if (wh.taxPercent != null) $('#prvTaxPercent').val(String(wh.taxPercent));
                if (!S.pay) $('#prvTaxAmount').text(fmt(wh.taxAmount)).data('v', num(wh.taxAmount));
            } else {
                $('#prvWht').prop('checked', false);
                $('#prvTaxPercent').val('0'); $('#prvTaxAmount').text(fmt(0)).data('v', 0);
                whtEnable(false);
            }
            total();
            renderGrid();
            chequeFill();
            var r = S.F.rights || {};
            $saveBtn().html('<i class="fa fa-save"></i> Update').prop('disabled', !r.update);
        }, function () { alert('Voucher not found'); });
    }

    // ------------------------------------------------------------------------------ save

    /** FormValidation(). */
    function formValid() {
        if (!S.rows.length) return 'Grid Record not found';
        if (int($('#prvProject').val()) === 0) return 'Cost Center Field is Required';
        if (int($('#prvRefAccount').val()) === 0) return S.pay ? 'Credit Account Field is Required' : 'Account Field is Required';
        if (S.F.multiCurrencyFeature) {
            if (int($('#prvCurrency').val()) === 0) return 'Fcy Code Field is Required';
            if (num($('#prvExchangeRate').val()) === 0) return 'Exchange Rate Field is Required';
            if (num($('#prvFcyAmount').val()) === 0) return 'Fcy Amount Field is Required';
        } else {
            if (int($('#prvCurrency').val()) === 0) return 'Please Configure Your Base Currency In configurations';
            var t = $.trim($('#prvExchangeRate').val());
            if (t === '' || t === '0') return 'Please Configure Your Base Currency Rate In configurations';
        }
        return null;
    }

    function payload(ack) {
        return {
            Id: S.recId, VoucherCode: int($('#prvVoucherNo').val()), VoucherDate: $('#prvDate').val(),
            ProjectId: int($('#prvProject').val()), RefAccountId: int($('#prvRefAccount').val()),
            Remarks: $('#prvRemarks').val(), MultiCurrencyId: int($('#prvCurrency').val()),
            ExchangeCurrencyRate: num($('#prvExchangeRate').val()), FcAmount: num($('#prvFcyAmount').val()),
            IncludeWHT: $('#prvWht').is(':checked'), TaxTypeId: int($('#prvTaxType').val()), TaxTypeName: selText('prvTaxType'),
            TaxPercent: $('#prvTaxPercent').val(), TaxAmount: num($('#prvTaxAmount').data('v')),
            AgainstAcId: int($('#prvAgainstAc').val()), WithHoldingAcId: int($('#prvWithHoldingAc').val()),
            InclusiveTax: S.pay && $('#prvRadIncluded').is(':checked'),
            ChequeNo: S.doc === 4 ? $('#prvHdrChequeNo').val() : null,
            ChequeDate: S.doc === 4 ? $('#prvHdrChequeDate').val() : (S.doc === 3 ? today() : null),
            PayTitle: S.doc === 4 ? $('#prvHdrPayTitle').val() : null,
            acknowledged: ack,
            rows: S.rows.map(function (r) {
                return {
                    PaymentTypeId: r.paymentTypeId, AccountId: r.accountId, JobLotId: r.jobLotId, Remarks: r.remarks, Amount: r.amount,
                    BranchId: r.branchId, CostCenterId: r.costCenterId, FinancialInstrumentId: r.financialInstrumentId || 0,
                    ChequeDate: r.chequeDate || null, ChequeId: r.chequeId || 0, ChequeNo: r.chequeNo || null,
                    PayeeTitle: r.payeeTitle || null, ChequeTypeId: r.chequeTypeId || 0
                };
            })
        };
    }

    function csrf() {
        var t = $('meta[name="_csrf"]').attr('content'), h = $('meta[name="_csrf_header"]').attr('content');
        var o = {}; if (t && h) o[h] = t; return o;
    }

    function save() {
        if (S.busy) return;
        var e = formValid();
        if (e) { alert(e); return; }
        if (!confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var ack = [];
        var printCheque = S.pay && S.F.chequePrintingEnable && $('#cbPrintOnSave').is(':checked');
        var printSlip = $('#prvChkPrint1').is(':checked');
        var slipWin = printSlip && w.CrystalPrint ? w.CrystalPrint.reserve() : null;
        S.busy = true; $saveBtn().prop('disabled', true);
        (function post() {
            $.ajax({ url: API + '/' + S.doc + '/save', type: 'POST', contentType: 'application/json', headers: csrf(),
                     data: JSON.stringify(payload(ack)) })
                .done(function (res) {
                    S.busy = false;
                    alert(res.message);
                    if (printSlip && w.CrystalPrint) w.CrystalPrint.open('hrm-102', { id: res.id }, null, slipWin);
                    else if (slipWin) w.CrystalPrint.release(slipWin);
                    if (printCheque) w.open('/accounts/banking/cheque-printing', '_blank');
                    reset(false);
                })
                .fail(function (x) {
                    var b = x.responseJSON || {};
                    if (x.status === 409 && b.confirm) {
                        if (confirm(b.message)) { ack.push(b.confirm); post(); return; }
                    } else {
                        alert(b.message || ('Save failed (' + x.status + ')'));
                    }
                    S.busy = false;
                    if (slipWin && w.CrystalPrint) w.CrystalPrint.release(slipWin);
                    $saveBtn().prop('disabled', !((S.F.rights || {})[S.recId > 0 ? 'update' : 'save']));
                });
        }());
    }

    function print() {
        if (!(S.recId > 0)) { alert('VoucherId Not Found'); return; }   // ANewAcRptPaymentReceiptsVoucherSlip_102
        if (!((S.F.rights || {}).print)) { alert("You don't have right"); return; }
        if (w.CrystalPrint) w.CrystalPrint.open('hrm-102', { id: S.recId });
    }

    // ------------------------------------------------------------------------------ wiring

    function wire() {
        $('#prvRefAccount').on('change', function () {
            balance(int($(this).val()), $('#prvRefBalance'));   // combcreditac_Leave -> AccountCurrentBalance
            chequeFill();
        });
        $('#prvAccount').on('change', function () { balance(int($(this).val()), $('#prvAccBalance')); });
        $('#prvDate').on('change', function () { balance(int($('#prvRefAccount').val()), $('#prvRefBalance')); });
        $('#prvExchangeRate').on('input change', function () { renderGrid(); });
        $('#prvCurrency').on('change', function () {
            // cmbCurrency_Leave: back to the base currency restores the base rate
            if (int($(this).val()) === int(S.F.baseCurrencyId) && S.F.baseCurrencyRate != null) $('#prvExchangeRate').val(String(S.F.baseCurrencyRate));
            renderGrid();
        });
        $('#prvAdd, #prvUpdateDetail').on('click', add);
        $('#prvCancelDetail').on('click', resetDetail);
        $('#prvGrid').on('click', '.prv-del', function (ev) {
            ev.stopPropagation();
            S.rows.splice(int($(this).data('i')), 1);
            if (!S.rows.length) $('#prvRefAccount').prop('disabled', false);
            total(); renderGrid();
            if (!S.pay) whtChanged();
        });
        $('#prvGrid').on('dblclick', 'tbody tr[data-i]', function () { editRow(int($(this).data('i'))); });
        $('#prvWht').on('change', whtChanged);
        $('#prvTaxType').on('change', function () { if ($('#prvWht').is(':checked')) whtChanged(); });
        $('input[name="prvTaxMode"]').on('change', function () { total(); });   // RadExcluded_CheckedChanged -> Total()
        $(document).on('keydown', function (e) {
            if (e.ctrlKey && e.keyCode === 83) { e.preventDefault(); if (S.recId === 0) save(); }
            else if (e.ctrlKey && e.keyCode === 85) { e.preventDefault(); if (S.recId > 0) save(); }
            else if (e.ctrlKey && e.keyCode === 78) { e.preventDefault(); reset(false); }
            else if (e.ctrlKey && e.keyCode === 82) { e.preventDefault(); w.location.reload(); }
            else if (e.altKey && (e.keyCode === 49 || e.keyCode === 97)) { e.preventDefault(); print(); }
        });
    }

    w.PRV = {
        init: function (cfg) {
            S.doc = int(cfg.doc); S.pay = S.doc === 1 || S.doc === 2;
            render();
            wire();
            var ready = $.getJSON(API + '/' + S.doc + '/lookups').then(bindLookups, function (x) {
                alert((x.responseJSON && x.responseJSON.message) || 'Could not load the form lists.');
            });
            var q = new URLSearchParams(w.location.search);
            var id = int(q.get('id') || q.get('Id'));
            var code = q.get('voucherCode') || q.get('VoucherCode') || q.get('fromDocNo');
            ready.then(function () {
                if (id > 0) loadForEdit(id);
                else if (code) {
                    $.getJSON('/accounts/api/vouchers/by-code', { documentTypeId: S.doc, voucherCode: code }).then(function (v) {
                        if (v && v.header && v.header.id) loadForEdit(v.header.id);
                    });
                }
            });
            return ready;
        },
        loadForEdit: function (id) { return loadForEdit(id); },
        save: save,
        reset: function () { reset(false); },
        print: print
    };
}(window, jQuery));
