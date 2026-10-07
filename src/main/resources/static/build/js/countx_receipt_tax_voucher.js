/* ============================================================================================
 * Screens 853 frmCashReceiptVoucherTax and 854 frmBankReceiptVoucherTax.
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew - one class opened
 * by its Tag (frmCashReceiptVoucherTax -> DocumentTypeId 3, frmBankReceiptVoucherTax -> 4). mode = that
 * DocumentTypeId.
 *
 * The Form tab AND the history tab (CRV History on 853, BRV History on 854: the other tab page is removed
 * by AcfrmPaymentVoucher_Load) are rendered here. Backend: /accounts/api/receipt-tax/{mode}/...
 * (ReceiptTaxVoucherController): lookups from the desktop's own fill methods, Save / Update through the
 * desktop procedure chain (DesktopVoucherWriter), ReadById through Sp_Vouchers_GetMethods, history through
 * USP_VoucherFormHistory. The page posts grid ROWS and the header controls; the ledger lines (vd debit /
 * vd2 credit per row, WHT vd3/vd4, discount vd5/vd6) are built on the server from Insert().
 *
 * Layout numbers are the Location / Size values of InitializeComponent() (tab page 1444 wide; the 30 px
 * toolstrip is in the template, so the designer y values are 30 less here).
 * ============================================================================================ */
(function (w, $) {
    'use strict';

    var BASE = '/accounts/api/receipt-tax';
    var TITLE = { 3: 'Cash Receipt Voucher', 4: 'Bank Receipt Voucher' };
    /* CommonServices.PaymentAndReceiptVoucherSlip: the 102-III report per DocumentTypeId. */
    var RPT3 = { 3: '102-CashReceiptVoucher.rpt', 4: '102-BankReceiptVoucher.rpt' };
    /* ScreenId = CommonServices.GetScreenIdByName(base.Name) with base.Name = "ReceiptsVoucherNew". */
    var SCREEN_ID = 20;

    var S = {
        mode: 3, L: null, F: {}, rows: [], recId: 0, updateMode: false,
        updIdx: -1, busy: false, accMode: 'title', refMode: 'title', remarksMouse: false, codeSeq: 0,
        subLen: 0, discSrc: '', amtSrc: '', sched: {},
        hist: { rows: [], sel: -1, busy: false }
    };
    /* print-rpt.js reads the open voucher's id */
    try { Object.defineProperty(w, 'RecId', { get: function () { return S.recId; }, configurable: true });
          Object.defineProperty(w, 'VoucherHeadId', { get: function () { return S.recId; }, configurable: true }); } catch (e) { }

    if (w.DesktopCombo) {
        /* DetailAccountFill / GetAccountsFromGlobalByTypeIds: AccountTitle | AccountCode | ParentAccountTitle | AccountClass (Id hidden). */
        w.DesktopCombo.define('rtvAcc4', [
            { caption: 'Credit Account', flex: 4 }, { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' }, { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('rtvAcc4c', [
            { caption: 'Account Code', flex: 2 }, { caption: 'AccountTitle', flex: 4, key: 'title' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' }, { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('rtvAcc2', [{ caption: 'Account Title', flex: 4 }, { caption: 'AccountCode', flex: 2, key: 'code' }]);
    }

    // ------------------------------------------------------------------------------ helpers

    function esc(v) { return $('<div>').text(v == null ? '' : String(v)).html(); }
    function ci(o, k) {
        if (!o) return undefined;
        if (k in o) return o[k];
        var lk = k.toLowerCase();
        for (var p in o) if (p.toLowerCase() === lk) return o[p];
        return undefined;
    }
    function num(v) { var n = parseFloat(String(v == null ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v == null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    /* GetDecimalConfiguration: stringFormatsingle "#,##0." + 1-4 zeros (anything else -> none). */
    function dec() { var n = int(S.F.amountDecimals); return n >= 1 && n <= 4 ? n : 0; }
    /* DecimalRateFormate: points = config > 0 ? config : 2; 1-4 zeros, anything else -> none. */
    function rdec() { var n = int(S.F.rateDecimals); n = n > 0 ? n : 2; return n <= 4 ? n : 0; }
    function fmtN(v, d) { return num(v).toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function fmt(v) { return fmtN(v, dec()); }
    function fmtRate(v) { return fmtN(v, rdec()); }
    function fmt3(v) { return num(v).toLocaleString('en-US', { maximumFractionDigits: 3 }); }   // "#,##0.###"
    function round(v, d) { var p = Math.pow(10, d); return Math.round((v + (v >= 0 ? 1e-9 : -1e-9)) * p) / p; }
    /* Math.Round(double, digits): to even on an exact half. */
    function roundEven(v, d) {
        var p = Math.pow(10, d), x = v * p, f = Math.floor(x), diff = x - f;
        if (Math.abs(diff - 0.5) < 1e-9) return (f % 2 === 0 ? f : f + 1) / p;
        return Math.round(x) / p;
    }
    function today() { var d = new Date(); d.setMinutes(d.getMinutes() - d.getTimezoneOffset()); return d.toISOString().slice(0, 10); }
    function day(v) { return v ? String(v).slice(0, 10) : ''; }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dmy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(v || ''); return m ? m[3] + '-' + MON[int(m[2]) - 1] + '-' + m[1] : (v || '');
    }
    function opts(list, idKey, textKey, blank) {
        var h = blank ? '<option value="0"></option>' : '';
        (list || []).forEach(function (r) { h += '<option value="' + esc(ci(r, idKey)) + '">' + esc(ci(r, textKey)) + '</option>'; });
        return h;
    }
    function accOpts(list, blank, mode) {
        var h = blank ? '<option value="0"></option>' : '';
        (list || []).forEach(function (a) {
            var code = ci(a, 'AccountCode'), title = ci(a, 'AccountTitle');
            h += '<option value="' + esc(ci(a, 'Id')) + '" data-code="' + esc(code) + '" data-title="' + esc(title) + '" data-parent="' +
                esc(ci(a, 'ParentAccountTitle')) + '" data-cls="' + esc(ci(a, 'AccountClass')) + '">' + esc(mode === 'code' ? code : title) + '</option>';
        });
        return h;
    }
    function selText(id) { var o = $('#' + id + ' option:selected'); return o.length && o.val() !== '0' ? o.text() : ''; }
    function setVal(id, v) { var $s = $('#' + id); $s.val(String(v == null ? 0 : v)); if ($s.val() == null) $s.val($s.find('option:first').val()); $s.trigger('change'); }
    /* A programmatic value (Value = ...) raises no Leave on the desktop, so no change event here. */
    function put(id, v) { var $s = $('#' + id); $s.val(String(v == null ? 0 : v)); if ($s.val() == null) $s.val($s.find('option:first').val()); }
    function firstRow(id) { var $s = $('#' + id), o = $s.find('option').filter(function () { return this.value !== '0'; }).first(); if (o.length) setVal(id, o.val()); }
    function rights() { return S.F.rights || {}; }
    function comboField(id) {
        var $s = $('#' + id), $w = $s.closest('.dtcombo-wrap');
        if (!$w.length) $w = $s.prev('.dtcombo-wrap');
        var $i = $w.find('.dtcombo-input').first();
        return $i.length ? $i : $s;
    }
    function comboFocus(id) { comboField(id).focus(); }
    function csrf() {
        var t = $('meta[name="_csrf"]').attr('content'), h = $('meta[name="_csrf_header"]').attr('content');
        var o = {}; if (t && h) o[h] = t; return o;
    }
    function errMsg(x, dflt) { return (x && x.responseJSON && x.responseJSON.message) || dflt || ('Request failed (' + (x ? x.status : '?') + ')'); }
    function api(p) { return BASE + '/' + S.mode + p; }
    function bal(v) { var b = Math.round(num(v)); return b < 0 ? '(' + Math.abs(b).toLocaleString('en-US') + ')' : b.toLocaleString('en-US'); }   // "#,#;(#,#);0"

    function busyOn($b) {
        $b.each(function () {
            var b = $(this); if (b.data('rtvBusy')) return;
            b.data('rtvBusy', 1).data('rtvHtml', b.html()).prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> ' + $.trim(b.text()));
        });
    }
    function busyOff($b) {
        $b.each(function () {
            var b = $(this); if (!b.data('rtvBusy')) return;
            b.html(b.data('rtvHtml')).removeData('rtvBusy').prop('disabled', false);
        });
    }

    // ------------------------------------------------------------------------------ markup helpers

    function px(x, y, w, h) {
        return 'left:' + x + 'px;top:' + y + 'px;' + (w != null ? 'width:' + w + 'px;' : '') + (h != null ? 'height:' + h + 'px;' : '');
    }
    function lab(x, y, text, cls, id, w) {
        return '<label class="prv-l' + (cls ? ' ' + cls : '') + '"' + (id ? ' id="' + id + '"' : '') + ' style="' + px(x, y, w) + '">' + text + '</label>';
    }
    function cmb(id, x, y, w, h, attrs, cls) {
        attrs = attrs || '';
        if (attrs.indexOf('data-dtcombo') < 0) attrs += ' data-dtcombo="single"';
        return '<select id="' + id + '" class="prv-cmb' + (cls ? ' ' + cls : '') + '"' + attrs + ' style="position:absolute;' + px(x, y, w, h) + '"></select>';
    }
    function tb(id, x, y, w, h, attrs, cls) {
        return '<input type="text" id="' + id + '" class="prv-tb' + (cls ? ' ' + cls : '') + '" autocomplete="off"' + (attrs || '') + ' style="' + px(x, y, w, h) + '"/>';
    }
    function dtp(id, x, y, w, h, cls) {
        return '<input type="date" id="' + id + '" class="prv-tb' + (cls ? ' ' + cls : '') + '" style="' + px(x, y, w, h) + '"/>';
    }
    function gbox(caption, x, y, w, h, inner, id, cls) {
        return '<div class="prv-gb' + (cls ? ' ' + cls : '') + '"' + (id ? ' id="' + id + '"' : '') + ' style="' + px(x, y, w, h) + '">' +
               (caption ? '<span class="prv-gbc">' + caption + '</span>' : '') + inner + '</div>';
    }
    function pnl(id, inner) { return '<div class="prv-fp" id="' + id + '" style="position:absolute;left:0;top:0;height:47px;">' + inner + '</div>'; }

    // ------------------------------------------------------------------------------ Form tab markup

    function render() {
        /* ---- panel4 (Dock Top, 1440x32, BackColor 10,110,110); the check boxes are anchored Top|Right */
        var strip = '<div class="prv-strip" style="height:32px;">' +
            '<span class="prv-strip-title" style="position:absolute;left:3px;top:6px;" id="rtvTitle">' + TITLE[S.mode] + '</span>' +
            '<label class="prv-l" style="color:#fff;font:bold 9pt \'Segoe UI\',sans-serif;' + px(247, 9) + '">Location Type</label>' +
            cmb('rtvLocation', 336, 3, 212, 26) +
            '<div class="prv-ar" style="width:1440px;height:32px;">' +
            '<label style="' + px(1064, 6, 95) + '"><input type="checkbox" id="rtvChkPrint1" checked/>Print Preview</label>' +
            '<label style="' + px(1170, 6, 106) + '"><input type="checkbox" id="rtvChkPrint2"/>Print Preview II</label>' +
            '<label style="' + px(1274, 6, 110) + '"><input type="checkbox" id="rtvChkPrint3"/>Print Preview III</label>' +
            '</div></div>';
        /* ---- groupBox2 "Main" (4,65) 1206x127 */
        var main =
            lab(6, 18, 'Project') + cmb('rtvProject', 100, 13, 232, 24, '', 'prv-cmb-mss') +
            lab(6, 44, 'Voucher Type') + cmb('rtvVoucherType', 100, 38, 232, 26) +
            lab(6, 70, 'Voucher No') + tb('rtvVoucherNo', 100, 66, 86, 23, ' readonly') +
            lab(191, 70, 'Date', 'rtv-lreg') + dtp('rtvDate', 231, 66, 101, 23) +
            lab(6, 97, 'Debit Account') + cmb('rtvDebit', 100, 91, 232, 26, ' data-dtcombo="rtvAcc2" data-dtcombo-caption="Debit Account"') +
            '<span id="rtvDrBalance" class="prv-bal" style="display:none;color:#f00;' + px(336, 98) + '"></span>' +
            lab(375, 18, 'Against Ac') + cmb('rtvAgainstAc', 441, 12, 276, 26, ' data-dtcombo="rtvAcc2" disabled') +
            lab(373, 43, 'Remarks', 'prv-tah') + '<textarea id="rtvRemarks" class="prv-tb" style="' + px(441, 39, 276, 78) + '"></textarea>' +
            lab(722, 18, 'Tcy Code') + cmb('rtvCurrency', 807, 12, 111, 26) +
            lab(722, 37, 'Tcy Exchange<br/>Rate') + tb('rtvExchangeRate', 807, 41, 111, 23, '', 'prv-num') +
            lab(722, 71, 'Tcy Amount') + tb('rtvFcyAmount', 807, 67, 111, 23, ' readonly tabindex="-1"', 'prv-num prv-dis') +
            '<label class="prv-l" style="' + px(721, 98) + '"><input type="checkbox" id="rtvCustom" style="vertical-align:middle;margin:0 3px 0 0;"/>Custom Accounts</label>';

        /* ---- groupboxdetail "Detail" (4,194): flowLayoutPanel1 (3,13) 1200 wide; panels laid out by layoutDetail() */
        var P = '';
        P += pnl('rtvPPay', lab(1, 2, 'PaymentType') + cmb('rtvPaymentType', 2, 21, 94, 26));
        P += pnl('rtvPAcc', lab(2, 3, 'Credit Ac') +
            '<label class="prv-rb" style="' + px(56, 1) + '"><input type="radio" name="rtvAccMode" value="title" checked/>Title</label>' +
            '<label class="prv-rb" style="' + px(103, 1) + '"><input type="radio" name="rtvAccMode" value="code"/>Code</label>' +
            '<span id="rtvAccBalance" class="prv-bal" style="display:none;color:#f00;' + px(152, 4) + '"></span>' +
            cmb('rtvAccount', 0, 21, 256, 26, ' data-dtcombo="rtvAcc4" data-dtcombo-caption="Credit Account"'));
        P += pnl('rtvPSub', lab(0, 2, 'Subsidiary A/c') + '<span id="rtvSubBalance" class="prv-bal" style="display:none;color:#f00;' + px(83, 2) + '"></span>' +
            cmb('rtvSubsidiary', 0, 21, 171, 26));
        P += pnl('rtvPJob', lab(3, 2, 'Job/Lot') + cmb('rtvJobLot', 0, 21, 137, 26));
        P += pnl('rtvPRem', lab(0, 3, 'Remarks') + tb('rtvLineRemarks', 0, 23, 280, 23));
        P += pnl('rtvPTcy', lab(2, 2, 'Tcy Code') + cmb('rtvTcyCode', 1, 21, 112, 26));
        P += pnl('rtvPRate', lab(1, 3, 'Tcy Exchange Rate') + tb('rtvTcyRate', 0, 22, 120, 23, '', 'prv-num'));
        P += pnl('rtvPFcy', lab(1, 4, 'Tcy Amount') + tb('rtvTcyAmount', 0, 23, 96, 23, '', 'prv-num'));
        P += pnl('rtvPAmt', lab(1, 4, 'Credit Amount') + tb('rtvAmount', 0, 23, 117, 23, '', 'prv-num'));
        P += pnl('rtvPRef', lab(1, 2, 'Reference Ac') +
            '<label class="prv-rb" style="' + px(77, 1) + '"><input type="radio" name="rtvRefMode" value="title" checked/>Title</label>' +
            '<label class="prv-rb" style="' + px(121, 1) + '"><input type="radio" name="rtvRefMode" value="code"/>Code</label>' +
            cmb('rtvReference', 0, 21, 173, 26, ' data-dtcombo="rtvAcc4" data-dtcombo-caption="Reference Account"'));
        P += pnl('rtvPBr', lab(0, 2, 'Branch Name') + cmb('rtvBranch', 0, 21, 164, 26));
        P += pnl('rtvPCc', lab(0, 2, 'Cost Center') + cmb('rtvCostCenter', 0, 21, 145, 26));
        P += pnl('rtvPChq', lab(3, 2, 'Cheque Date') + dtp('rtvChequeDate', 3, 23, 120, 23) +
            lab(129, 2, 'Cheque No') + '<input type="text" id="rtvChequeNo" class="prv-tb" autocomplete="off" maxlength="50" style="' + px(124, 23, 113, 23) + '"/>' +
            lab(239, 2, 'PayTitle') + tb('rtvPayTitle', 238, 23, 175, 23, ' maxlength="100"'));
        P += pnl('rtvPBtn',
            '<button type="button" id="rtvAdd" class="prv-fbtn" style="' + px(2, 21, 39, 25) + '">+</button>' +
            '<button type="button" id="rtvUpdateDetail" class="prv-fbtn" style="display:none;' + px(4, 21, 60, 25) + '">Update</button>' +
            '<button type="button" id="rtvCancelDetail" class="prv-fbtn" style="display:none;' + px(2, -1, 62, 25) + '">Cancel</button>');
        var top = '<div class="prv-abs" id="rtvTop" style="height:279px;">' + strip +
            gbox('Main', 4, 35, 1206, 127, main) +
            gbox('Detail', 4, 164, 1206, 112, '<div class="prv-g" id="rtvFlow" style="' + px(3, 13) + '">' + P + '</div>', 'rtvDetailBox') +
            '</div>';
        var grid = '<div class="prv-mid"><div class="prv-gridbox" style="right:0;" id="rtvGridBox"><table class="prv-jg" id="rtvGrid" tabindex="0"><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
        /* ---- panel21 (Dock Bottom, 105 high once the multi currency grid is off), controls anchored Top|Right of a 1440 wide panel */
        var bot = '<div class="prv-bot prv-abs" style="height:132px;"><div class="prv-ar" style="width:1440px;height:105px;">' +
            lab(1216, 9, 'Amount') + tb('rtvValue', 1296, 4, 142, 23, ' readonly tabindex="-1"', 'prv-num') +
            lab(573, 32, 'TaxType') + cmb('rtvTaxType', 627, 29, 92, 24, '', 'prv-cmb-mss') +
            '<label class="prv-rb prv-plain" style="' + px(721, 32) + '"><input type="checkbox" id="rtvWht"/>WHT</label>' +
            lab(773, 34, 'WHT Account') + cmb('rtvWithHoldingAc', 902, 29, 191, 24, ' data-dtcombo="rtvAcc2" disabled', 'prv-cmb-mss') +
            lab(1094, 34, 'Tax %') + tb('rtvTaxPercent', 1140, 30, 72, 23, ' readonly value="0" tabindex="-1"') +
            lab(1216, 34, 'Tax Amount') + tb('rtvTaxAmount', 1296, 30, 142, 23, ' readonly tabindex="-1"', 'prv-num') +
            lab(773, 59, 'Discount Account (Cr)') + cmb('rtvDiscAcc', 902, 54, 191, 24, ' data-dtcombo="rtvAcc2" data-dtcombo-caption="Discount Account"', 'prv-cmb-mss') +
            lab(1094, 60, 'Disc %', 'rtv-lreg') + tb('rtvDiscPct', 1140, 55, 72, 23, '') +
            lab(1215, 51, 'Discount<br/>Amount', null, null, 75) + tb('rtvDiscAmt', 1296, 55, 142, 23, '', 'prv-num') +
            lab(1215, 83, 'Total Amount') + tb('rtvTotal', 1296, 79, 142, 23, ' readonly tabindex="-1"', 'prv-num') +
            '<button type="button" class="prv-sysbtn" id="rtvBtnHistory" style="left:1346px;top:107px;width:92px;height:22px;"><i class="fa fa-history"></i>History</button>' +
            '</div></div>';
        $('#prvForm').html(top + grid + bot);
    }

    /**
     * flowLayoutPanel1 (1200 wide): the visible panels left to right in TabIndex order, wrapped at 1200.
     * Cheque panel: bank voucher only (ChequeDetailPanel.Visible = false for document type 3);
     * Subsidiary / Branch / Cost Center follow the Load-time features; without Subsidiary the remarks
     * panel is 451 wide.
     */
    function layoutDetail() {
        var F = S.F, bank = S.mode === 4;
        var subs = !!F.subsidiaryFeature, branch = !!F.branchFeature, booking = !!F.isBookingOffice;
        var remW = subs ? 280 : 451;
        var P = [['rtvPPay', 96, true], ['rtvPAcc', 256, true], ['rtvPSub', 171, subs], ['rtvPJob', 138, true], ['rtvPRem', remW, true],
                 ['rtvPTcy', 112, true], ['rtvPRate', 120, true], ['rtvPFcy', 96, true], ['rtvPAmt', 118, true], ['rtvPRef', 175, true],
                 ['rtvPBr', 164, branch], ['rtvPCc', 147, booking], ['rtvPChq', 415, bank], ['rtvPBtn', 70, true]];
        var x = 0, y = 0;
        P.forEach(function (p) {
            var $p = $('#' + p[0]);
            if (!p[2]) { $p.hide(); return; }
            if (x > 0 && x + p[1] > 1200) { x = 0; y += 47; }
            $p.css({ left: x + 'px', top: y + 'px', width: p[1] + 'px', display: 'block' });
            x += p[1];
        });
        $('#rtvLineRemarks').css('width', remW + 'px');
    }

    // ------------------------------------------------------------------------------ lookups

    var COMBOS = ['rtvProject', 'rtvVoucherType', 'rtvLocation', 'rtvDebit', 'rtvAgainstAc', 'rtvWithHoldingAc', 'rtvPaymentType', 'rtvAccount',
                  'rtvSubsidiary', 'rtvJobLot', 'rtvTcyCode', 'rtvCurrency', 'rtvReference', 'rtvBranch', 'rtvCostCenter', 'rtvTaxType', 'rtvDiscAcc'];
    function capture() { var o = {}; COMBOS.forEach(function (id) { o[id] = $('#' + id).val(); }); return o; }
    function restore(o, ids) { ids.forEach(function (id) { if (o[id] != null) put(id, o[id]); }); }

    /** AccountTitleFill: the debit (cash / bank) account list of this voucher type. */
    function fillDebit() { $('#rtvDebit').html(accOpts((S.L && S.L.debitAccounts) || [], true, 'title')); }   // DDL.BindDDL ZeroIndex: true

    function fillLists(L) {
        $('#rtvProject').html(opts(L.projects, 'Id', 'ProjectName', false));
        $('#rtvVoucherType').html(opts(L.voucherTypes, 'Id', 'Name', false));
        $('#rtvLocation').html(opts(L.locationTypes, 'Id', 'Location', true));        // ZeroIndex: true
        fillDebit();
        $('#rtvAccount').html(accOpts(L.creditAccounts, true, S.accMode));
        $('#rtvReference').html(accOpts(L.referenceAccounts, true, S.refMode));
        $('#rtvAgainstAc, #rtvWithHoldingAc').html(accOpts(L.whtAccounts, true, 'title'));
        $('#rtvDiscAcc').html(accOpts(L.discountAccounts, true, 'title'));
        $('#rtvPaymentType').html(opts(L.paymentTypes, 'Id', 'PaymentType', false));
        $('#rtvJobLot').html(opts(L.jobLots, 'Id', 'JobLotDescription', true));       // ZeroIndex: true
        $('#rtvCurrency, #rtvTcyCode').html(opts(L.currencies, 'Id', 'CurrencyCode', true));
        $('#rtvTaxType').html(opts(L.taxTypes, 'Id', 'TaxName', true));
        $('#rtvBranch').html(opts(L.branches, 'BranchId', 'BranchName', false));
        $('#rtvCostCenter').html(opts(L.costCenters, 'Id', 'CostCenterName', false));
        bindSubsidiary(0);
    }

    function bindLookups(L) {
        S.L = L; S.F = L.flags || {};
        render();
        fillLists(L);
        put('rtvVoucherType', S.mode);
        layoutDetail();
        $('#btnPrint').prop('disabled', !rights().print);                             // Print.Enabled = DoHavePrintRights
        wireForm();
        reset(true);
    }

    /** DefaultConfigurations(): Job/Lot, Base Currency (-> cmbCurrency_Leave), BaseCurrencyRate, default discount account. */
    function defaults() {
        if (S.F.defaultJobLotId != null && int(S.F.defaultJobLotId) > 0) put('rtvJobLot', S.F.defaultJobLotId);
        if (int(S.F.baseCurrencyId) > 0) { put('rtvCurrency', S.F.baseCurrencyId); currencyLeave(); }
        if (S.F.baseCurrencyRate != null) $('#rtvExchangeRate').val(fmtRate(S.F.baseCurrencyRate));
        if (int(S.F.defaultDiscountAccountId) > 0) put('rtvDiscAcc', S.F.defaultDiscountAccountId);
    }

    /** btnRefresh_Click: lists re-bound in place, DefaultConfigurations, SpecialRightsImplement, combos of the grid. */
    function refreshLists($b) {
        if (S.busy) return;
        busyOn($b);
        $.getJSON(api('/lookups')).then(function (L) {
            var keep = capture();
            S.L = L; S.F = L.flags || S.F; S.sched = {};
            fillLists(L);
            restore(keep, ['rtvProject', 'rtvVoucherType', 'rtvLocation', 'rtvDebit', 'rtvAccount', 'rtvReference', 'rtvAgainstAc', 'rtvWithHoldingAc',
                           'rtvPaymentType', 'rtvJobLot', 'rtvCurrency', 'rtvTcyCode', 'rtvCostCenter', 'rtvTaxType', 'rtvDiscAcc']);
            if (S.F.defaultBranchId) setVal('rtvBranch', S.F.defaultBranchId);      // BranchesFill: Text = UserAccount.BranchName
            defaults();
            layoutDetail();
            specialRights();
            renderGrid();
        }, function (x) { alert(errMsg(x, 'Could not refresh the lists.')); }).always(function () { busyOff($b); });
    }

    // ------------------------------------------------------------------------------ grid

    /** grdSettings(): visible columns, captions, order and widths. Delete is the last column added. */
    function cols() {
        var F = S.F, bank = S.mode === 4;
        var c = [{ k: 'paymentType', t: 'PaymentType', ed: 'pt', w: 90 }, { k: 'accountCode', t: 'AccountCode', w: 90 },
                 { k: 'accountTitle', t: 'Account Title', w: 250 }];
        if (F.subsidiaryFeature) c.push({ k: 'subsidiaryAccount', t: 'SubsidiaryAccount', w: 190 });
        c.push({ k: 'jobLot', t: 'JobLot', ed: 'job', w: 150 }, { k: 'remarks', t: 'Remarks', ed: 'txt', w: 293 }, { k: 'tcyCode', t: 'TcyCode', w: 80 },
               { k: 'tcyExchangeRate', t: 'TcyExchangeRate', n: 2, w: 105 }, { k: 'fcyAmount', t: 'Tcy Amount', n: 3, sum: 3, w: 115 },
               { k: 'amount', t: 'Credit Amount', n: 1, sum: 1, w: 115 }, { k: 'referenceAccount', t: 'ReferenceAccount', w: 150 });
        if (F.branchFeature) c.push({ k: 'branchName', t: 'BranchName', w: 130 });
        if (F.isBookingOffice) c.push({ k: 'costCenterName', t: 'Cost Center', ed: 'cc', w: 130 });
        if (bank) c.push({ k: 'chequeDate', t: 'ChequeDate', ed: 'dt', w: 90 }, { k: 'chequeNo', t: 'ChequeNo', ed: 'txt', w: 110 },
                         { k: 'payTitle', t: 'PayTitle', ed: 'txt', w: 130 });
        c.push({ k: 'x', t: 'X', w: 20 });
        return c;
    }

    /* Janus GridEX: ColumnAutoResize = true for cash (columns spread over the grid), false for bank (designer widths, scrolls). */
    function renderGridHead() {
        var cs = cols(), sum = cs.reduce(function (s, c) { return s + (c.w || 100); }, 0);
        var auto = S.mode !== 4, boxW = $('#rtvGridBox').innerWidth() || 1400;
        var scale = auto && sum > 0 ? Math.max(1, (boxW - 2) / sum) : 1;
        $('#rtvGrid').css('width', auto ? '100%' : sum + 'px');
        $('#rtvGrid colgroup').html(cs.map(function (c) { return '<col style="width:' + Math.round((c.w || 100) * scale) + 'px;"/>'; }).join(''));
        $('#rtvGrid thead').html('<tr>' + cs.map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + ' title="' + esc(c.t) + '">' + c.t + '</th>'; }).join('') + '</tr>');
    }

    function cellEditor(c, r, i) {
        var L = S.L || {};
        if (c.ed === 'txt') return '<input type="text" class="prv-ce" data-i="' + i + '" data-k="' + c.k + '" value="' + esc(r[c.k]) + '"/>';
        if (c.ed === 'dt') return '<input type="date" class="prv-ce" data-i="' + i + '" data-k="chequeDate" value="' + esc(day(r.chequeDate)) + '"/>';
        var list, idK, txK, cur, key;
        if (c.ed === 'pt') { list = L.paymentTypes; idK = 'Id'; txK = 'PaymentType'; cur = r.paymentTypeId; key = 'paymentTypeId'; }
        else if (c.ed === 'job') { list = L.jobLots; idK = 'Id'; txK = 'JobLotDescription'; cur = r.jobLotId; key = 'jobLotId'; }
        else if (c.ed === 'cc') { list = L.costCenters; idK = 'Id'; txK = 'CostCenterName'; cur = r.costCenterId; key = 'costCenterId'; }
        else return null;
        var h = '<select class="prv-ce" data-dtcombo="single" data-i="' + i + '" data-k="' + key + '">' + (int(cur) ? '' : '<option value="0"></option>');
        (list || []).forEach(function (o) {
            var v = ci(o, idK); h += '<option value="' + esc(v) + '"' + (String(v) === String(cur) ? ' selected' : '') + '>' + esc(ci(o, txK)) + '</option>';
        });
        return h + '</select>';
    }

    function renderGrid() {
        renderGridHead();
        var cs = cols(), html = '';
        S.rows.forEach(function (r, i) {
            html += '<tr data-i="' + i + '">' + cs.map(function (c) {
                if (c.k === 'x') return '<td style="text-align:center;padding:0;"><button type="button" class="prv-cbtn prv-del" data-i="' + i + '">X</button></td>';
                if (c.ed) { var e = cellEditor(c, r, i); if (e) return '<td>' + e + '</td>'; }
                var v = r[c.k];
                if (c.k === 'amount') v = fmt(v);
                else if (c.k === 'fcyAmount') v = fmt3(v);
                else if (c.k === 'tcyExchangeRate') v = fmt3(v);
                return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $('#rtvGrid tbody').html(html || '<tr><td colspan="' + cs.length + '" class="prv-empty"></td></tr>');
        var sumA = baseAmount(), sumF = S.rows.reduce(function (s, r) { return s + num(r.fcyAmount); }, 0);
        $('#rtvGrid tfoot').html(S.rows.length ? '<tr>' + cs.map(function (c) {
            return '<td class="num" style="font-weight:bold;">' + (c.sum === 1 ? fmt(sumA) : c.sum === 3 ? fmt3(sumF) : '') + '</td>';
        }).join('') + '</tr>' : '');
        // CalculateTotalInformation(): txtFcyAmount = SUM(FcyAmount) "#,##0.###", "0" with no rows
        $('#rtvFcyAmount').val(S.rows.length ? fmt3(sumF) : '0');
    }

    // ------------------------------------------------------------------------------ Total() / WHT / discount

    function baseAmount() { return S.rows.reduce(function (s, r) { return s + num(r.amount); }, 0); }
    var T = { value: 0, tax: 0, total: 0 };
    function showBoxes() {
        $('#rtvValue').val(fmt(T.value)); $('#rtvTaxAmount').val(String(T.tax)); $('#rtvTotal').val(fmt(T.total));
    }

    /**
     * Total() + checkBox1_CheckedChanged(): Value = sum of the Credit Amounts; with WHT ticked
     * Tax = Math.Round((Value + Discount) / (100 - rate) * 100 * rate / 100, 2); Total = Amount + Tax + Discount.
     * Nothing changes while the grid sums to 0 (Total() only acts when Amount > 0).
     */
    function ptotal() {
        var base = baseAmount();
        if (base <= 0) return;
        var rate = $('#rtvWht').is(':checked') ? num($('#rtvTaxPercent').val()) : 0, disc = num($('#rtvDiscAmt').val());
        T.value = round(base, dec());
        discSync();
        disc = num($('#rtvDiscAmt').val());
        T.tax = (rate > 0 && rate < 100) ? roundEven((T.value + disc) / (100 - rate) * 100 * rate / 100, 2) : 0;
        T.total = round(base + T.tax + disc, dec());
        showBoxes();
    }

    function whtEnable(on) { $('#rtvAgainstAc, #rtvWithHoldingAc').prop('disabled', !on); }

    function schedule() {
        var key = int($('#rtvTaxType').val()) + '|' + $('#rtvDate').val();
        if (S.sched[key] !== undefined) return $.Deferred().resolve(S.sched[key]).promise();
        return $.getJSON(BASE + '/tax-schedule', { taxTypeId: int($('#rtvTaxType').val()), date: $('#rtvDate').val() })
            .then(function (s) { S.sched[key] = s; return s; });
    }

    /** checkBox1_CheckedChanged(): WHT ticked reads the tax schedule; the Against / WHT combos are enabled only while it is ticked. */
    function whtChanged() {
        if (!$('#rtvWht').is(':checked')) {
            whtEnable(false);
            put('rtvWithHoldingAc', 0);
            $('#rtvTaxPercent').val('0'); T.tax = 0;
            if (baseAmount() <= 0) $('#rtvTaxAmount').val('0');
            ptotal();
            return $.Deferred().resolve().promise();
        }
        return schedule().then(function (s) {
            if (!s || !s.found) {
                put('rtvWithHoldingAc', 0);
                $('#rtvTaxPercent').val('0'); T.tax = 0;
            } else {
                $('#rtvTaxPercent').val(String(s.taxPercent));
                if (!S.updateMode) put('rtvWithHoldingAc', s.taxGLAccountId);
            }
            whtEnable(true);
            if (baseAmount() <= 0) $('#rtvTaxAmount').val('0');
            ptotal();
        }, function (x) { alert('Error applying withholding tax: ' + errMsg(x)); });
    }

    /** CommonServices.UpdateRelatedFieldsCalculation for Discount Amount <-> Disc % (factor = txtValue, factor2 = 100). */
    function discSync() {
        if (!S.discSrc) return;
        if (T.value <= 0) { $('#rtvDiscAmt').val('0'); $('#rtvDiscPct').val('0'); return; }
        if (S.discSrc === 'amt') { var a = num($('#rtvDiscAmt').val()); $('#rtvDiscPct').val(a > 0 ? String(round(a * 100 / T.value, 3)) : '0'); }
        else { var p = num($('#rtvDiscPct').val()); $('#rtvDiscAmt').val(p > 0 ? String(round(p * T.value / 100, dec())) : '0'); }
    }
    function discPctChanged() {
        S.discSrc = 'pct';
        if (num($('#rtvDiscPct').val()) > 0 && T.value === 0) comboFocus('rtvPaymentType');
        whtChanged();
    }
    function discAmtChanged() {
        S.discSrc = 'amt';
        if (num($('#rtvDiscAmt').val()) > 0 && T.value === 0) comboFocus('rtvPaymentType');
        whtChanged();
    }

    // ------------------------------------------------------------------------------ detail entry

    function balance(accountId, $lbl) {
        if (!(accountId > 0)) { $lbl.hide().text('0'); return; }
        $.getJSON(BASE + '/balance', { accountId: accountId, date: $('#rtvDate').val() }).then(function (r) { $lbl.text(bal(r && r.balance)).show(); });
    }
    function subBalance() {
        var acc = int($('#rtvAccount').val()), sub = int($('#rtvSubsidiary').val()), $l = $('#rtvSubBalance');
        if (acc > 0 && sub > 0) {
            $.getJSON(BASE + '/subsidiary-balance', { accountId: acc, subsidiaryId: sub, date: $('#rtvDate').val() }).then(function (r) { $l.text(bal(r && r.balance)).show(); });
        } else $l.hide().text('0');
    }

    /** BindSubsidiaryAccount(): the subsidiaries of the selected credit account (all of them with no account), retaining retainId when it is in the list. */
    function bindSubsidiary(retainId) {
        var all = (S.L && S.L.subsidiaries) || [], list = all, acc = int($('#rtvAccount').val());
        var $s = $('#rtvSubsidiary');
        if (!all.length) { S.subLen = 0; $s.html('<option value="0"></option>'); return; }
        if ($('#rtvAccount option:selected').text() !== '' && acc > 0) {
            list = all.filter(function (r) { return int(ci(r, 'AccountId')) === acc; });
        } else if (acc === 0 && $('#rtvAccount').val() !== null && $('#rtvAccount option:selected').text() === '') {
            list = all;
        }
        if (!list.length) { S.subLen = 0; $s.html('<option value="0"></option>'); return; }
        S.subLen = list.length;
        $s.html('<option value="0"></option>' + list.map(function (r) {
            return '<option value="' + esc(ci(r, 'Id')) + '" data-type="' + esc(ci(r, 'SubsidiaryTypeId')) + '" data-acc="' + esc(ci(r, 'AccountId')) +
                   '">' + esc(ci(r, 'SubsidiaryAccount')) + '</option>';
        }).join(''));
        put('rtvSubsidiary', retainId || 0);
    }

    /** CmbSubsidiaryAccount_Leave: the subsidiary's own GL account becomes the credit account. */
    function subsidiaryChanged() {
        var $o = $('#rtvSubsidiary option:selected'), sub = int($('#rtvSubsidiary').val());
        if (sub > 0) {
            var gl = int($o.attr('data-acc'));
            if (gl !== 0 && gl !== int($('#rtvAccount').val())) { put('rtvAccount', gl); balance(gl, $('#rtvAccBalance')); bindSubsidiary(sub); }
        }
        subBalance();
    }

    /** Add_Click_1 checks, in the desktop's order; upd = btnUpdateDetail_Click (its own wording for two of them). */
    function detailValid(upd) {
        var F = S.F;
        if (!$('#rtvPaymentType').val()) return ['PaymentType Field Required', 'rtvPaymentType'];
        if (upd) { if (int($('#rtvAccount').val()) === 0) return ['Account Title Field Required', 'rtvAccount']; }
        else if (int($('#rtvAccount').val()) === 0) return ['Credit Account Field Required', 'rtvAccount'];
        if (int($('#rtvJobLot').val()) === 0) return ['Job/Lot Field Required', 'rtvJobLot'];
        if (int($('#rtvTcyCode').val()) === 0) return ['Tcy Code Field is Required', 'rtvTcyCode'];
        if ($.trim($('#rtvTcyRate').val()) === '' || num($('#rtvTcyRate').val()) === 0) return ['Tcy Exchange Rate Field is Required', 'rtvTcyRate'];
        if ($.trim($('#rtvAmount').val()) === '' || num($('#rtvAmount').val()) === 0) return ['Amount Field Required', 'rtvAmount'];
        if (F.subsidiaryFeature && S.subLen > 0 && int($('#rtvSubsidiary').val()) === 0) {
            return [upd ? 'Subsidiary Account Title Field Required' : 'Subsidiary Account Title Field Is Required', 'rtvSubsidiary'];
        }
        if (F.branchFeature && int($('#rtvBranch').val()) === 0) return ['BranchName Field is Required', 'rtvBranch'];
        return null;
    }
    function fail(e) { alert(e[0]); comboFocus(e[1]); }

    function rowFromEntry() {
        var accOpt = $('#rtvAccount option:selected'), refId = int($('#rtvReference').val()), refOpt = $('#rtvReference option:selected');
        var sub = int($('#rtvSubsidiary').val()), subOpt = $('#rtvSubsidiary option:selected');
        var r = {
            paymentTypeId: int($('#rtvPaymentType').val()), paymentType: selText('rtvPaymentType'),
            accountId: int($('#rtvAccount').val()), accountCode: accOpt.attr('data-code') || '', accountTitle: accOpt.attr('data-title') || accOpt.text(),
            subsidiaryAccountId: 0, subsidiaryAccount: '', subsidiaryAccountTypeId: 0,
            jobLotId: int($('#rtvJobLot').val()), jobLot: selText('rtvJobLot'), remarks: $('#rtvLineRemarks').val(),
            tcyCodeId: int($('#rtvTcyCode').val()), tcyCode: selText('rtvTcyCode'), tcyExchangeRate: num($('#rtvTcyRate').val()),
            fcyAmount: num($('#rtvTcyAmount').val()), amount: num($('#rtvAmount').val()),
            referenceAccountId: refId, referenceAccount: refId > 0 ? (refOpt.attr('data-title') || refOpt.text()) : '',
            branchId: int($('#rtvBranch').val()), branchName: selText('rtvBranch'),
            costCenterId: int($('#rtvCostCenter').val()), costCenterName: selText('rtvCostCenter'),
            chequeDate: $('#rtvChequeDate').val() || today(), chequeNo: $('#rtvChequeNo').val() || '', payTitle: $('#rtvPayTitle').val() || ''
        };
        if (S.F.subsidiaryFeature) {
            if (S.subLen > 0 && sub > 0) { r.subsidiaryAccountId = sub; r.subsidiaryAccount = subOpt.text(); r.subsidiaryAccountTypeId = int(subOpt.attr('data-type')); }
            else { r.subsidiaryAccountId = r.accountId; r.subsidiaryAccount = r.accountTitle; r.subsidiaryAccountTypeId = 4; }
        }
        return r;
    }

    /** The tail of Add / Update: CmbAgainstAc takes the first row's account while it is empty. */
    function afterRowsChanged() {
        if (S.rows.length && int($('#rtvAgainstAc').val()) === 0) put('rtvAgainstAc', S.rows[0].accountId);
    }

    /** Add_Click_1. */
    function add() {
        var e = detailValid(false);
        if (e) { fail(e); return; }
        S.rows.push(rowFromEntry());
        $('#rtvAccBalance').hide();
        renderGrid();
        comboFocus('rtvAccount');
        ptotal();
        $('#rtvAmount').val(''); $('#rtvTcyAmount').val(''); S.amtSrc = '';
        afterRowsChanged();
    }

    /** btnUpdateDetail_Click. */
    function updateDetail() {
        if (S.updIdx < 0) return;
        var e = detailValid(true);
        if (e) { fail(e); return; }
        S.rows[S.updIdx] = rowFromEntry();
        $('#rtvAdd').show(); $('#rtvUpdateDetail, #rtvCancelDetail').hide();
        renderGrid();
        ptotal();
        comboFocus('rtvAccount');
        S.updIdx = -1;
        $('#rtvSubsidiary').html('<option value="0"></option>'); S.subLen = 0;
        $('#rtvAmount').val(''); $('#rtvTcyAmount').val(''); S.amtSrc = '';
        rateChangedHeader();                // txtExchangeRate_TextChanged(null, null)
        afterRowsChanged();
    }

    /** btnCancelDetail_Click. */
    function cancelDetail() {
        $('#rtvAdd').show(); $('#rtvUpdateDetail, #rtvCancelDetail').hide();
        $('#rtvSubsidiary').html('<option value="0"></option>'); S.subLen = 0;
        $('#rtvTcyAmount').val(''); $('#rtvAmount').val('');
        S.updIdx = -1; S.amtSrc = '';
    }

    /** grd_DoubleClick (and Ctrl+Enter on the grid): the row goes back into the entry box. */
    function editRow(i) {
        var r = S.rows[i]; if (!r) return;
        S.updIdx = i;
        setVal('rtvPaymentType', r.paymentTypeId);
        put('rtvAccount', r.accountId);
        bindSubsidiary(0);
        if (S.subLen === 0 && S.F.subsidiaryFeature) {
            $('#rtvSubsidiary').html('<option value="0"></option><option value="' + esc(r.subsidiaryAccountId) + '" data-type="4" data-acc="' + esc(r.accountId) + '">' + esc(r.subsidiaryAccount) + '</option>');
        }
        put('rtvSubsidiary', r.subsidiaryAccountId);
        put('rtvJobLot', r.jobLotId);
        $('#rtvLineRemarks').val(r.remarks);
        $('#rtvTcyAmount').val(fmt3(r.fcyAmount));
        $('#rtvAmount').val(fmt3(r.amount));
        put('rtvBranch', r.branchId);
        if (int(r.referenceAccountId) > 0) put('rtvReference', r.referenceAccountId);
        if (int(r.costCenterId) > 0) put('rtvCostCenter', r.costCenterId);
        put('rtvTcyCode', r.tcyCodeId);
        $('#rtvTcyRate').val(fmt3(r.tcyExchangeRate));
        $('#rtvChequeDate').val(day(r.chequeDate) || today());
        $('#rtvChequeNo').val(r.chequeNo || '');
        $('#rtvPayTitle').val(r.payTitle || '');
        comboFocus('rtvPaymentType');
        $('#rtvAdd').hide(); $('#rtvUpdateDetail, #rtvCancelDetail').show();
    }

    /** grd_ColumnButtonClick "Delete" (and Ctrl+Space). */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (S.updIdx >= 0) { alert('Reset the detail first to delete'); return; }
        S.rows.splice(i, 1);
        renderGrid();
        ptotal();
    }

    // ------------------------------------------------------------------------------ amount / rate / currency

    function decimalOnly(e) {
        var k = e.key;
        if (e.ctrlKey || e.metaKey || e.altKey || !k || k.length > 1) return;
        if (/[0-9]/.test(k)) return;
        if (k === '.' && String(this.value).indexOf('.') < 0) return;
        e.preventDefault();
    }
    /** CommonServices.CommasApplyWhileTypingOnVouchers. */
    function commaTyping() {
        var el = this, v = el.value, pos = el.selectionStart || 0, before = v.length;
        var raw = v.replace(/,/g, '');
        if (raw === '' || raw === '.' || !/^\d*\.?\d*$/.test(raw)) return;
        var parts = raw.split('.');
        var ip = parts[0].replace(/^0+(?=\d)/, '').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var nv = ip + (parts.length > 1 ? '.' + parts[1] : '');
        if (nv !== v) { el.value = nv; var p = Math.max(0, pos + (nv.length - before)); try { el.setSelectionRange(p, p); } catch (x) { } }
    }
    function fcyDec() { return S.F.fcyDecimals != null ? int(S.F.fcyDecimals) : 3; }

    /** txtamount_TextChanged / txtFcyAmountDetail_TextChanged / txtTcyExchangeRateDetail_TextChanged:
        UpdateRelatedFieldsCalculation(txtamount, txtFcyAmountDetail, rate, ...) - the last edited field is the source. */
    function linkAmounts() {
        var rate = num($('#rtvTcyRate').val());
        if (S.amtSrc === 'fcy') {
            var f = num($('#rtvTcyAmount').val());
            if (rate <= 0) { $('#rtvAmount').val('0'); $('#rtvTcyAmount').val('0'); }
            else if (f > 0) $('#rtvAmount').val(String(round(f * rate, dec()))); else $('#rtvAmount').val('0');
        } else {
            var a = num($('#rtvAmount').val());
            if ($.trim($('#rtvAmount').val()) === '') return;
            if (rate <= 0) { $('#rtvAmount').val('0'); $('#rtvTcyAmount').val('0'); }
            else if (a > 0) $('#rtvTcyAmount').val(String(round(a / rate, fcyDec()))); else $('#rtvTcyAmount').val('0');
        }
    }

    /** txtExchangeRate_TextChanged: the Tcy detail rate follows, then CalculateTotalInformation(). */
    function rateChangedHeader() {
        $('#rtvTcyRate').val($('#rtvExchangeRate').val());
        linkAmounts();
        renderGrid();
    }

    /** cmbCurrency_Leave. */
    function currencyLeave() {
        var cur = int($('#rtvCurrency').val());
        if (cur === 0) return;
        put('rtvTcyCode', cur);
        if (cur !== int(S.F.baseCurrencyId)) {
            $.getJSON(api('/last-rate'), { currencyId: cur }).then(function (r) {
                var lr = num(r && r.lastRate);
                var t = lr ? fmtRate(lr) : '0';
                $('#rtvExchangeRate').val(t); $('#rtvTcyRate').val(t);
                rateChangedHeader();
            }, function (x) { alert(errMsg(x)); });
        } else {
            var b = fmtRate(S.F.baseCurrencyRate);
            $('#rtvExchangeRate').val(b); $('#rtvTcyRate').val(b);
            rateChangedHeader();
        }
    }

    // ------------------------------------------------------------------------------ voucher type / reset / load

    /** combvtype_Leave -> AccountTitleFill() (+ VoucherNofill while Save is visible and enabled). */
    function applyDoc() {
        var keep = int($('#rtvDebit').val());
        fillDebit();
        put('rtvDebit', keep);
        if (S.mode === 3 && rights().save && !S.updateMode) firstRow('rtvDebit');     // Rows[1].Activate()
        balance(int($('#rtvDebit').val()), $('#rtvDrBalance'));
        if (!S.updateMode && rights().save) nextCode(false);
    }

    function showButtons(m) {
        var r = rights();
        $('#btnSave').toggle(m === 'save').prop('disabled', !r.save);
        $('#btnUpdate').toggle(m === 'update').prop('disabled', !r.update);
    }

    /** VoucherNofill(): CommonServices.GenerateVoucherCode(combvtype.Value). */
    function nextCode(first) {
        if (first && S.L && S.L.nextCode != null) { $('#rtvVoucherNo').val(S.L.nextCode); return; }
        var seq = ++S.codeSeq;
        $.getJSON(api('/next-code')).then(function (r) {
            if (seq === S.codeSeq && !S.updateMode) $('#rtvVoucherNo').val(r.voucherCode);
        });
    }

    /**
     * Reset(): the date, project, location, debit account, currency and rate are NOT touched (only Load does).
     * The WHT tick stays; clearing the value box re-runs checkBox1_CheckedChanged, so a ticked WHT keeps its
     * combos enabled with the schedule of the (now empty) Tax Type.
     */
    function reset(first) {
        var wasWht = $('#rtvWht').is(':checked');
        S.rows = []; S.recId = 0; S.updateMode = false; S.updIdx = -1; S.subLen = 0; S.amtSrc = '';
        $('#rtvAccBalance').hide().text('0');
        $('#rtvLineRemarks').val(''); $('#rtvAmount').val(''); $('#rtvPayTitle').val(''); $('#rtvChequeNo').val('');
        $('#rtvRemarks').val('');
        put('rtvAgainstAc', 0); put('rtvTaxType', 0);
        $('#rtvValue, #rtvTaxAmount, #rtvTotal').val(''); T.value = 0; T.tax = 0; T.total = 0;
        if (first) $('#rtvDiscPct, #rtvDiscAmt').val(''); else $('#rtvDiscPct, #rtvDiscAmt').val('0');
        firstRow('rtvPaymentType');
        put('rtvAccount', 0);
        if (first) {
            $('#rtvDate').val(today());
            firstRow('rtvProject');
            $('#rtvChequeDate').val(today());
            if (S.F.defaultBranchId) setVal('rtvBranch', S.F.defaultBranchId);
            if (S.mode === 3 && rights().save) firstRow('rtvDebit');
            defaults();
        }
        nextCode(first);
        showButtons('save');
        $('#rtvAdd').show(); $('#rtvUpdateDetail, #rtvCancelDetail').hide();
        $('#rtvSubsidiary').html('<option value="0"></option>');
        renderGrid();
        if (wasWht) { whtEnable(true); whtChanged(); }
        else $('#rtvTaxPercent').val($('#rtvTaxPercent').val() || '0');
        $('#rtvTaxAmount, #rtvTotal, #rtvValue').val('');
        $('#rtvFcyAmount').val('');
        if (!first) currencyLeave();                                  // cmbCurrency_Leave(null, null)
    }

    /** btnNew_Click: Reset(), the debit account balance hidden. */
    function newClick() {
        reset(false);
        $('#rtvDrBalance').hide();
    }

    /** ReadById(ID). */
    function loadForEdit(id) {
        return $.getJSON(api('/' + id)).then(function (v) {
            if (!v) return;
            switchTab('form');
            S.recId = int(v.id); S.updateMode = true; S.codeSeq++; S.updIdx = -1;
            $('#rtvVoucherNo').val(v.voucherCode);
            $('#rtvDate').val(day(v.voucherDate));
            fillDebit();
            put('rtvDebit', v.refAccountId);
            put('rtvProject', v.projectId);
            $('#rtvRemarks').val(v.remarks || '');
            put('rtvLocation', v.locationTypeId);
            put('rtvCurrency', v.multiCurrencyId);
            put('rtvTcyCode', v.multiCurrencyId);
            $('#rtvExchangeRate').val(fmtRate(v.exchangeCurrencyRate));
            $('#rtvTcyRate').val(fmtRate(v.exchangeCurrencyRate));
            if (int(v.multiCurrencyId) === 0 || num(v.exchangeCurrencyRate) === 0) defaults();
            $('#rtvCustom').prop('checked', !!v.customAccounts);
            S.rows = (v.rows || []).map(function (r) {
                var jl = ((S.L && S.L.jobLots) || []).filter(function (j) { return int(ci(j, 'Id')) === int(r.jobLotId); })[0];
                return {
                    paymentTypeId: int(r.paymentTypeId), paymentType: r.paymentType || '', accountId: int(r.accountId),
                    accountCode: r.accountCode || '', accountTitle: r.accountTitle || '',
                    subsidiaryAccountId: int(r.subsidiaryAccountId), subsidiaryAccount: r.subsidiaryAccountTitle || '', subsidiaryAccountTypeId: int(r.subsidiaryAccountTypeId),
                    jobLotId: int(r.jobLotId), jobLot: jl ? ci(jl, 'JobLotDescription') : '', remarks: r.remarks || '', tcyCodeId: int(r.tcyCodeId), tcyCode: r.tcyCode || '',
                    tcyExchangeRate: num(r.tcyExchangeRate), fcyAmount: num(r.fcyAmount), amount: num(r.amount),
                    referenceAccountId: int(r.referenceAccountId), referenceAccount: r.referenceAccount || '',
                    branchId: int(r.branchId), branchName: r.branchName || '', costCenterId: int(r.costCenterId), costCenterName: '',
                    chequeDate: day(r.chequeDate), chequeNo: r.chequeNo || '', payTitle: r.payeeTitle || ''
                };
            });
            S.rows.forEach(function (r) {
                var cc = ((S.L && S.L.costCenters) || []).filter(function (c) { return int(ci(c, 'Id')) === r.costCenterId; })[0];
                if (cc) r.costCenterName = ci(cc, 'CostCenterName');
            });
            renderGrid();
            var wh = v.wht || {}, ds = v.discount;
            if (ds) {
                S.discSrc = 'pct';
                put('rtvDiscAcc', ds.accountId); $('#rtvDiscPct').val(String(num(ds.percent))); $('#rtvDiscAmt').val(String(num(ds.amount)));
            } else {
                put('rtvDiscAcc', S.F.defaultDiscountAccountId || 0); $('#rtvDiscPct').val('0'); $('#rtvDiscAmt').val('0');
            }
            var done;
            if (v.includeWHT) {
                $('#rtvWht').prop('checked', true);
                put('rtvTaxType', wh.taxTypeId);
                done = whtChanged().then(function () {
                    put('rtvAgainstAc', wh.againstAcId); put('rtvWithHoldingAc', wh.withHoldingAcId);
                    if (wh.taxPercent != null) $('#rtvTaxPercent').val(String(wh.taxPercent));
                    ptotal();
                });
            } else {
                $('#rtvWht').prop('checked', false);
                $('#rtvTaxPercent').val('0'); $('#rtvTaxAmount').val('0');
                done = whtChanged();
            }
            showButtons('update');
            balance(int($('#rtvDebit').val()), $('#rtvDrBalance'));
            return done.then(function () { ptotal(); $('#rtvDate').focus(); });
        }, function (x) { alert(errMsg(x, 'Voucher not found')); });
    }

    // ------------------------------------------------------------------------------ save

    /** FormValidation(). */
    function formValid() {
        if (int($('#rtvProject').val()) === 0) return ['Cost Center Field is Required', 'rtvProject'];
        if (!$('#rtvVoucherType').val() || int($('#rtvVoucherType').val()) === 0) return ['Voucher Type Field is Required', 'rtvVoucherType'];
        if (int($('#rtvLocation').val()) === 0) return ['Location Type Field is Required', 'rtvLocation'];
        if (int($('#rtvDebit').val()) === 0) return ['Debit Account Field is Required', 'rtvDebit'];
        if (int($('#rtvCurrency').val()) === 0) return ['Fcy Code Field is Required', 'rtvCurrency'];
        if (num($('#rtvExchangeRate').val()) === 0) return ['Exchange Rate Field is Required', 'rtvExchangeRate'];
        if (num($('#rtvFcyAmount').val()) === 0) return ['Fcy Amount Field is Required', 'rtvFcyAmount'];
        return null;
    }

    /** Insert(): the field checks that follow the "Are you sure" prompt. */
    function taxFieldsValid() {
        if ($('#rtvWht').is(':checked')) {
            if (int($('#rtvTaxType').val()) === 0) return ['TaxType Account Field is Required', 'rtvTaxType'];
            if (num($('#rtvTaxPercent').val()) === 0) return ['Tax Percent Field is Required', 'rtvTaxPercent'];
            if (int($('#rtvAgainstAc').val()) === 0) return ['Withholding Credit Account Field is Required', 'rtvAgainstAc'];
            if (int($('#rtvWithHoldingAc').val()) === 0) return ['Withholding Debit Account Field is Required', 'rtvWithHoldingAc'];
        }
        if (int($('#rtvDiscAcc').val()) === 0 && num($('#rtvDiscAmt').val()) > 0) return ['Discount Account Field is Required', 'rtvDiscAcc'];
        if (num($('#rtvDiscPct').val()) === 0 && num($('#rtvDiscAmt').val()) > 0) return ['Disc% Field is Required', 'rtvDiscPct'];
        for (var i = 0; i < S.rows.length; i++) if (S.rows[i].amount > 0 && int(S.rows[i].accountId) === 0) return ['Please Select Account Title First', null];
        return null;
    }

    function payload(ack) {
        return {
            Id: S.recId, VoucherCode: int($('#rtvVoucherNo').val()), VoucherDate: $('#rtvDate').val(),
            ProjectId: int($('#rtvProject').val()), LocationTypeId: int($('#rtvLocation').val()), RefAccountId: int($('#rtvDebit').val()),
            Remarks: $('#rtvRemarks').val(), MultiCurrencyId: int($('#rtvCurrency').val()),
            ExchangeCurrencyRate: num($('#rtvExchangeRate').val()), FcAmount: num($('#rtvFcyAmount').val()),
            CustomAccounts: $('#rtvCustom').is(':checked'),
            IncludeWHT: $('#rtvWht').is(':checked'), TaxTypeId: int($('#rtvTaxType').val()),
            TaxPercent: $('#rtvTaxPercent').val(), AgainstAcId: int($('#rtvAgainstAc').val()), WithHoldingAcId: int($('#rtvWithHoldingAc').val()),
            DiscountAccountId: int($('#rtvDiscAcc').val()), DiscountPercent: num($('#rtvDiscPct').val()), DiscountAmount: num($('#rtvDiscAmt').val()),
            DiscountAmountText: $('#rtvDiscAmt').val(),
            acknowledged: ack,
            rows: S.rows.map(function (r) {
                return {
                    PaymentTypeId: r.paymentTypeId, AccountId: r.accountId, SubsidiaryAccountId: r.subsidiaryAccountId || 0,
                    SubsidiaryAccountTypeId: r.subsidiaryAccountTypeId || 0, JobLotId: r.jobLotId, Remarks: r.remarks,
                    TcyCodeId: r.tcyCodeId, TcyExchangeRate: r.tcyExchangeRate, FcyAmount: r.fcyAmount, Amount: r.amount,
                    ReferenceAccountId: r.referenceAccountId || 0,
                    ChequeDate: r.chequeDate || null, ChequeNo: r.chequeNo || '', PayeeTitle: r.payTitle || '',
                    BranchId: r.branchId || 0, CostCenterId: r.costCenterId || 0
                };
            })
        };
    }

    /** Save_Click / Update_Click -> Insert(). */
    function save(kind) {
        if (S.busy) return;
        var $b = kind === 'update' ? $('#btnUpdate') : $('#btnSave');
        if (kind !== 'update') S.recId = 0;
        if (kind === 'update' && !(S.recId > 0)) { alert('Record Not Update  ' + S.recId); return; }
        if (!S.rows.length) { alert('Grid Record not found'); return; }
        var e = formValid();
        if (e) { alert(e[0]); comboFocus(e[1]); return; }
        if (!confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        e = taxFieldsValid();
        if (e) { alert(e[0]); if (e[1]) comboFocus(e[1]); return; }
        var ack = [];
        var p1 = $('#rtvChkPrint1').is(':checked'), p2 = $('#rtvChkPrint2').is(':checked'), p3 = $('#rtvChkPrint3').is(':checked');
        var printKind = p1 ? 1 : (p2 ? 2 : (p3 ? 3 : 0));
        var win = printKind && w.CrystalPrint ? w.CrystalPrint.reserve() : null;
        S.busy = true; busyOn($b);
        (function post() {
            $.ajax({ url: api('/save'), type: 'POST', contentType: 'application/json', headers: csrf(), data: JSON.stringify(payload(ack)) })
                .done(function (res) {
                    S.busy = false; busyOff($b);
                    alert(res.message);
                    reset(false);
                    if (printKind) printDoc(printKind, res.id, S.mode, null, win);
                })
                .fail(function (x) {
                    var b = x.responseJSON || {};
                    if (x.status === 409 && b.confirm) {
                        if (confirm(b.message)) { ack.push(b.confirm); post(); return; }
                    } else alert(b.message || ('Save failed (' + x.status + ')'));
                    S.busy = false; busyOff($b);
                    if (win && w.CrystalPrint) w.CrystalPrint.release(win);
                });
        }());
    }

    // ------------------------------------------------------------------------------ prints / special rights / shortcut keys

    /** kind 1 = hrm-102 slip, 2 = acc-102 (with DocumentTypeId when given), 3 = PaymentAndReceiptVoucherSlip (102-III per type). */
    function printDoc(kind, id, docType, btn, win) {
        if (!(int(id) > 0)) { if (win && w.CrystalPrint) w.CrystalPrint.release(win); alert('VoucherId Not Found'); return; }
        if (kind === 3) {
            var url = '/reports/print/by-template/' + encodeURIComponent(RPT3[docType || S.mode]) + '/pdf?id=' + encodeURIComponent(id);
            if (win) win.location.href = url; else w.open(url, '_blank');
            return;
        }
        if (!w.CrystalPrint) { if (win) win.close(); alert('Print is not available.'); return; }
        var args = { id: int(id) };
        if (kind === 2 && docType) args.documentTypeId = docType;
        w.CrystalPrint.open(kind === 1 ? 'hrm-102' : 'acc-102', args, btn, win);
    }
    function printButton(kind, btn) {
        if (kind === 1 && !rights().print) { alert("You don't have right"); return; }
        if (kind === 3 && !(S.mode > 0)) { alert('Please Select Voucher type First'); return; }
        printDoc(kind, S.recId, S.mode, btn, null);
    }

    /* SpecialRightsImplement: RightId 2 -> ChkPrint1, 8 -> ChkPrint2, 9 -> ChkPrint3 (ScreenId of ReceiptsVoucherNew = 20). */
    function specialRights() {
        if (!w.SpecialRights) return;
        w.SpecialRights.mine(SCREEN_ID).then(function (rows) {
            $.each(rows || [], function (i, r) {
                var rid = int(r.RightId != null ? r.RightId : r.rightId);
                var a = r.IsActive != null ? r.IsActive : r.isActive;
                a = a === true || a === 1 || a === '1' || String(a).toLowerCase() === 'true';
                var id = ({ 2: '#rtvChkPrint1', 8: '#rtvChkPrint2', 9: '#rtvChkPrint3' })[rid];
                if (id) $(id).prop('checked', a);
            });
        }, function (x) { alert(errMsg(x, 'Could not load special rights.')); });
    }

    /* MakeShortCutKeys() */
    var KEYS = [['Ctrl+S', 'For Save in Form Tab And For Show History in History Tabs'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print I'], ['Alt+2', 'For Print II'], ['Alt+3', 'For Print III'],
        ['Ctrl+F5', 'For Focus on Voucher Type'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Payment Type in Detail Box'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        if (!$('#rtvKeys').length) {
            $('body').append('<div id="rtvKeys" style="display:none;position:fixed;inset:0;z-index:3000;background:rgba(0,0,0,.3);">' +
                '<div style="position:absolute;left:50%;top:70px;transform:translateX(-50%);width:520px;max-width:95vw;background:#fff;border:1px solid #00796B;box-shadow:0 4px 16px rgba(0,0,0,.3);">' +
                '<div style="background:#00796B;color:#fff;padding:5px 8px;font-weight:bold;display:flex;justify-content:space-between;"><span>ShortCut Keys</span>' +
                '<a href="javascript:void(0)" id="rtvKeysClose" style="color:#fff;">&#x2715;</a></div><div style="max-height:70vh;overflow:auto;">' +
                '<table class="win-grid-table"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody></tbody></table></div></div></div>');
            $('#rtvKeysClose').on('click', function () { $('#rtvKeys').hide(); });
            $('#rtvKeys').on('click', function (e) { if (e.target === this) $(this).hide(); });
        }
        $('#rtvKeys tbody').html(KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''));
        $('#rtvKeys').show();
    }
    function attachments() { alert('Attachments are not available on the web form yet.'); }


    // ------------------------------------------------------------------------------ history tab (CrvTab / BrvTab)

    function H(n) { return $('#rtvH' + n); }

    /* Receipt designer numbers of the history tab (T.layout: CrvTab 122-160, BrvTab 171-205). */
    var HL = {
        3: { title: 'Cash Receipt Voucher History', th: 29, fh: 78, gw: 1060, inner: false, ly: 22, fy: 23, cy: 41, ky: 39,
             from: [4, 130], to: [140, 130], fno: 274, tno: 378, acc: [482, 210], st: [696, 118], show: [818, 39],
             rad: [[880, 33], [880, 53], [961, 33], [961, 53]], info: [1068, 4, 242, 69], iv: [158, 67], gh: 270, dh: 25 },
        4: { title: 'Bank Receipt Voucher History', th: 28, fh: 78, gw: 1064, inner: true, ly: 22, fy: 23, cy: 41, ky: 39,
             from: [5, 130], to: [139, 130], fno: 273, tno: 374, acc: [476, 216], st: [694, 118], show: [814, 39],
             rad: [[871, 28], [871, 48], [952, 28], [952, 48]], info: [1069, 5, 242, 69], iv: [154, 80], gh: 258, dh: 26 }
    };

    function hcols() {
        var c = [{ b: 'edit', t: 'Edit', w: 35 }, { b: 'p1', t: 'Print', w: 40 }, { b: 'p2', t: 'Print-II', w: 60 }, { b: 'p3', t: 'Print-III', w: 60 },
                 { k: 'documentType', t: 'V.Type' }, { k: 'voucherDate', t: 'V.Date' }, { k: 'voucherCode', t: 'V.No' }];
        if (S.mode === 4) c.push({ k: 'chequeNo', t: 'ChequeNo' });
        c.push({ k: 'accountTitle', t: 'AccountTitle' }, { k: 'voucherAmount', t: 'VoucherAmount', n: 1, sum: 1 },
               { k: 'fcyAmount', t: 'FcyAmount', n: 3, sum: 3 },
               { k: 'remarks', t: 'Remarks', w: 350 }, { k: 'entryUser', t: 'EntryUser' }, { k: 'entryDate', t: 'EntryDate' },
               { k: 'modifyUser', t: 'ModifyUser' }, { k: 'modifyDate', t: 'ModifyDate' }, { k: 'approvedUser', t: 'ApprovedUser' },
               { k: 'approvedDate', t: 'ApprovedDate' }, { k: 'attachment', t: 'Attachment', link: 2 }, { b: 'att', t: 'Add Attachments', w: 110 });
        return c;
    }

    function renderHistory() {
        var G = HL[S.mode], h = '';
        h += '<div class="prv-ts"><div class="prv-ts-in" style="border-bottom-color:#c5cbd3;">' +
             '<button type="button" class="prv-ts-btn" id="rtvHReset"><i class="fa fa-undo" style="color:#2e7d32"></i><span><u>R</u>eset</span></button>' +
             '<button type="button" class="prv-ts-btn" id="rtvHRefresh"><i class="fa fa-refresh" style="color:#2e7d32"></i><span>Refresh</span></button></div></div>';
        h += '<div class="prv-teal" style="height:' + G.th + 'px;margin-top:-3px;"><span style="left:4px;top:4px;">' + G.title + '</span></div>';
        var radios =
            '<label class="prv-rb" style="' + px(G.rad[0][0], G.rad[0][1]) + '"><input type="radio" name="rtvHDate" value="docdate" checked/>Doc Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[1][0], G.rad[1][1]) + '"><input type="radio" name="rtvHDate" value="entrydate"/>Entry Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[2][0], G.rad[2][1]) + '"><input type="radio" name="rtvHDate" value="modifydate"/>Modify Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[3][0], G.rad[3][1]) + '"><input type="radio" name="rtvHDate" value="approveddate"/>Approved Date</label>';
        var filt =
            lab(G.from[0] - 1, G.fy, 'From Date') +
            '<div class="prv-dtp" style="' + px(G.from[0], G.cy, G.from[1], 23) + '"><input type="checkbox" id="rtvHFromChk" checked title="Use this date"/>' +
            '<input type="date" id="rtvHFrom" class="prv-tb"/></div>' +
            lab(G.to[0], G.ly, 'To Date') +
            '<div class="prv-dtp" style="' + px(G.to[0], G.cy, G.to[1], 23) + '"><input type="checkbox" id="rtvHToChk" checked title="Use this date"/>' +
            '<input type="date" id="rtvHTo" class="prv-tb"/></div>' +
            lab(G.fno, G.ly, 'From Doc No') + tb('rtvHFromNo', G.fno, G.cy, 100, 23, ' maxlength="9"') +
            lab(G.tno, G.ly, 'To Doc No') + tb('rtvHToNo', G.tno, G.cy, 100, 23, ' maxlength="9"') +
            lab(G.acc[0], G.ly, 'Account Title') +
            '<select id="rtvHAccount" class="prv-cmb" data-dtcombo="single" style="position:absolute;' + px(G.acc[0], G.ky, G.acc[1], 26) + '"><option value="0"></option></select>' +
            lab(G.st[0], G.ly, 'Approved Status') +
            '<select id="rtvHStatus" class="prv-cmb" data-dtcombo="single" style="position:absolute;' + px(G.st[0], G.ky, G.st[1], 26) + '">' +
            '<option value="notapproved" selected>Not Apporved</option><option value="approved">Approved</option><option value="all">All</option></select>' +
            '<button type="button" class="prv-fbtn prv-show" id="rtvHShow" style="' + px(G.show[0], G.show[1], 53, 26) + '">Show</button>';
        if (G.inner) filt += radios;
        var info = '<div id="rtvHInfo" class="prv-gb prv-info" style="display:none;' + px(G.info[0], G.info[1], G.info[2], G.info[3]) + '">' +
            lab(6, 11, 'Total Vouchers', 'prv-info') + '<span id="rtvHTotal" class="prv-iv" style="' + px(G.iv[0], 11, G.iv[1], 17) + '">0</span>' +
            lab(6, 28, 'Approved Vouchers', 'prv-info') + '<span id="rtvHApproved" class="prv-iv" style="' + px(G.iv[0], 28, G.iv[1], 17) + '">0</span>' +
            lab(6, 47, 'UnApproved Vouchers', 'prv-info') + '<span id="rtvHUnApproved" class="prv-iv" style="' + px(G.iv[0], 47, G.iv[1], 17) + '">0</span></div>';
        h += '<div class="prv-abs" style="height:' + G.fh + 'px;">' + gbox('Filters', 5, 3, G.gw, 71, filt) + (G.inner ? '' : radios) + info + '</div>';
        h += '<div class="prv-hgrid" style="height:' + G.gh + 'px;"><table class="prv-jg prv-auto" id="rtvHGrid" tabindex="0"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        h += '<div class="prv-teal" style="height:' + G.dh + 'px;"><span style="left:3px;top:3px;">Detail of above selected row</span></div>';
        h += '<div class="prv-hdet"><table class="prv-jg prv-auto" id="rtvHDetail"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        $('#rtvHist').html(h);
        H('From').add(H('To')).val(today());
        H('Grid').find('thead').html('<tr>' + hcols().map(function (c) {
            return '<th' + (c.n ? ' class="num"' : '') + (c.w ? ' style="min-width:' + c.w + 'px;"' : '') + '>' + c.t + '</th>';
        }).join('') + '</tr>');
        wireHistory();
        historyAccounts();
    }

    function historyAccounts($b) {
        if ($b) busyOn($b);
        return $.getJSON(api('/history/accounts')).then(function (list) {
            var cur = H('Account').val();
            H('Account').html('<option value="0"></option>' + (list || []).map(function (a) {
                return '<option value="' + esc(a.Id) + '">' + esc(a.name) + '</option>';
            }).join(''));
            if (cur) H('Account').val(cur);
        }, function (x) { alert(errMsg(x, 'Could not load the accounts.')); }).always(function () { if ($b) busyOff($b); });
    }

    function historyReset() {
        H('From').add(H('To')).val(today());
        H('FromNo').add(H('ToNo')).val('');
        H('Account').val('0').trigger('change');
        H('Status').val('notapproved');
        H('Info').hide();
        S.hist.rows = []; S.hist.sel = -1;
        H('Grid').find('tbody, tfoot').empty();
        H('Detail').find('thead, tbody, tfoot').empty();
    }

    function historyShow() {
        var hs = S.hist; if (hs.busy) return;
        var $b = H('Show');
        var params = {
            dateType: $('input[name="rtvHDate"]:checked').val(),
            fromDate: H('FromChk').is(':checked') ? H('From').val() : '', toDate: H('ToChk').is(':checked') ? H('To').val() : '',
            fromDocNo: int(H('FromNo').val()) || '', toDocNo: int(H('ToNo').val()) || '',
            accountId: int(H('Account').val()), approvedStatus: H('Status').val()
        };
        hs.busy = true; busyOn($b);
        $.getJSON(api('/history'), params).then(function (res) {
            var rows = (res && res.rows) || [];
            hs.rows = rows; hs.sel = -1;
            H('Detail').find('thead, tbody, tfoot').empty();
            if (!rows.length) { H('Info').hide(); H('Grid').find('tbody, tfoot').empty(); return; }
            H('Info').show();
            H('Total').text(res.totalVouchers || 0); H('Approved').text(res.totalApprovedVoucher || 0); H('UnApproved').text(res.totalUnApprovedVoucher || 0);
            var cs = hcols(), sumA = 0, sumF = 0, html = '';
            rows.forEach(function (v, i) {
                sumA += num(v.voucherAmount); sumF += num(v.fcyAmount);
                html += '<tr data-i="' + i + '" data-id="' + esc(v.id) + '">' + cs.map(function (c) {
                    if (c.b) {
                        if (c.b === 'att') return '<td><button type="button" class="prv-cbtn" disabled title="Attachments are not on the web yet">' + c.t + '</button></td>';
                        return '<td><button type="button" class="prv-cbtn prv-hb" data-b="' + c.b + '" data-i="' + i + '">' + c.t + '</button></td>';
                    }
                    var x = v[c.k];
                    if (c.link === 2) return '<td><a class="prv-att" data-i="' + i + '">' + esc(int(x) > 0 ? x : '') + '</a></td>';
                    if (c.n === 1) x = fmt(x); else if (c.n === 3) x = fmt3(x);
                    return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(x) + '</td>';
                }).join('') + '</tr>';
            });
            H('Grid').find('tbody').html(html);
            H('Grid').find('tfoot').html('<tr>' + cs.map(function (c) {
                return '<td class="num" style="font-weight:bold;">' + (c.sum === 1 ? fmt(sumA) : c.sum === 3 ? fmt3(sumF) : '') + '</td>';
            }).join('') + '</tr>');
            selectHistory(0);
        }, function (x) { alert(errMsg(x, 'Could not load the history.')); }).always(function () { hs.busy = false; busyOff($b); });
    }

    function selectHistory(i) {
        var hs = S.hist, v = hs.rows[i]; if (!v) return;
        hs.sel = i;
        H('Grid').find('tbody tr').removeClass('prv-sel').filter('[data-i="' + i + '"]').addClass('prv-sel');
        $.getJSON(api('/' + v.id + '/lines')).then(function (r) { if (hs.sel === i) renderLines(r); });
    }

    /** CrvDetail / BrvDetail grids (VoucherDetailByHeaderId 2907 / VoucherDetailBRVByHeaderId 3576), sorted by id. */
    function renderLines(r) {
        var list = ((r && r.rows) || []).slice().sort(function (a, b) { return a.id - b.id; }), F = S.F, cs;
        var data = list.map(function (x) { return $.extend({}, x, { fcyD: x.debit > 0 ? x.fcy : 0, fcyC: x.credit > 0 ? x.fcy : 0 }); });
        if (S.mode === 3) {
            cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['tcyCode', 'TcyCode'], ['tcyExchangeRate', 'TcyExchangeRate', 2],
                  ['debit', 'Debit', 1], ['fcyD', 'Fcy Debit', 3], ['credit', 'Credit', 1], ['fcyC', 'Fcy Credit', 3],
                  ['referenceAccount', 'ReferenceAccount'], F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
        } else {
            cs = [['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['tcyExchangeRate', 'TcyExchangeRate', 2],
                  ['debit', 'Debit', 1], ['fcyD', 'Fcy Debit', 3], ['credit', 'Credit', 1], ['fcyC', 'Fcy Credit', 3],
                  ['referenceAccount', 'ReferenceAccount'], F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter'],
                  ['chequeDate', 'ChequeDate'], ['chequeNo', 'ChequeNo'], ['payeeTitle', 'PayTitle']];
        }
        cs = cs.filter(Boolean);
        var sums = {};
        H('Detail').find('thead').html('<tr>' + cs.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + c[1] + '</th>'; }).join('') + '</tr>');
        H('Detail').find('tbody').html(data.map(function (x) {
            return '<tr>' + cs.map(function (c) {
                var v = x[c[0]];
                if (c[2]) { if (c[2] !== 2) sums[c[0]] = (sums[c[0]] || 0) + num(v); v = c[2] === 3 ? fmt3(v) : c[2] === 2 ? fmtRate(v) : fmt(v); }
                return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join(''));
        H('Detail').find('tfoot').html('<tr>' + cs.map(function (c) {
            var s = c[2] && c[2] !== 2 ? (c[2] === 3 ? fmt3(sums[c[0]] || 0) : fmt(sums[c[0]] || 0)) : '';
            return '<td class="num" style="font-weight:bold;">' + s + '</td>';
        }).join('') + '</tr>');
    }

    /* DataGridHistory_ColumnButtonClick (2872): Edit = Reset + ReadById; Print = hrm-102 (id only); Print-II = acc-102; Print-III = PaymentAndReceiptVoucherSlip */
    function historyButton(b, i, btn) {
        var v = S.hist.rows[i]; if (!v) return;
        if (b === 'edit') { reset(false); loadForEdit(v.id); }
        else if (b === 'p1') printDoc(1, v.id, int(v.documentTypeId), btn, null);
        else if (b === 'p2') printDoc(2, v.id, int(v.documentTypeId), btn, null);
        else if (b === 'p3') printDoc(3, v.id, int(v.documentTypeId), btn, null);
    }

    function wireHistory() {
        var hs = S.hist, $g = H('Grid');
        H('Reset').on('click', historyReset);
        H('Refresh').on('click', function () { historyAccounts($(this)); });
        H('Show').on('click', historyShow);
        H('FromNo').add(H('ToNo')).on('keydown', function (e) {
            if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length > 1) return;
            if (!/[0-9]/.test(e.key)) e.preventDefault();
        });
        $g.on('click', 'tbody tr[data-i]', function () { var i = int($(this).data('i')); if (i !== hs.sel) selectHistory(i); });
        $g.on('dblclick', 'tbody tr[data-i]', function () { var v = hs.rows[int($(this).data('i'))]; if (v) loadForEdit(v.id); });
        $g.on('click', 'a.prv-att', function (e) { e.stopPropagation(); alert('Attachments are not available on the web form yet.'); });
        $g.on('click', 'button.prv-hb', function (e) { e.stopPropagation(); historyButton($(this).data('b'), int($(this).data('i')), this); });
        $g.on('keydown', function (e) {
            var n = hs.rows.length; if (!n) return;
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                e.preventDefault();
                var i = hs.sel < 0 ? 0 : hs.sel + (e.key === 'ArrowDown' ? 1 : -1);
                if (i >= 0 && i < n) selectHistory(i);
            } else if (e.ctrlKey && e.key === 'Enter') {
                e.preventDefault(); e.stopPropagation();
                var v = hs.rows[hs.sel]; if (v) { reset(false); loadForEdit(v.id); }
            }
        });
    }

    // ------------------------------------------------------------------------------ tabs / keys / wiring

    /** tab = 'form' | 'history' (CrvTab / BrvTab). */
    function switchTab(tab) {
        $('#tabFormContent, #tabHistContent').hide();
        $('#tabBtnForm, #tabBtnHist').removeClass('active');
        if (tab === 'form') { $('#tabFormContent').show(); $('#tabBtnForm').addClass('active'); }
        else { $('#tabHistContent').show(); $('#tabBtnHist').addClass('active'); H('From').focus(); }
    }
    function onHist() { return $('#tabHistContent').is(':visible'); }

    function focusNext(from) {
        var $all = $('#tabFormContent, #tabHistContent').filter(':visible')
            .find('input,select,textarea,button').filter(':visible').filter(function () {
                return !this.disabled && (!this.readOnly || $(this).hasClass('dtcombo-input')) && this.type !== 'hidden' && this.tabIndex >= 0;
            });
        var i = $all.index(from);
        if (i >= 0 && i + 1 < $all.length) $all.eq(i + 1).focus();
    }

    /* ReceiptsVoucherNew_KeyDown (3193) */
    function onKey(e) {
        if ($('#srModal').is(':visible') || $('#rtvKeys').is(':visible')) return;
        var k = e.key || '', K = k.toUpperCase(), r = rights(), hd = onHist();
        if (k === 'Escape') { w.location.href = '/accounts/dashboard'; return; }
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey) {
            var t = e.target;
            if (t && /^(INPUT|SELECT|TEXTAREA)$/.test(t.tagName) && t.type !== 'button' && t.type !== 'submit') {
                if ($(t).closest('#rtvGrid').length) { e.preventDefault(); $(t).trigger('change'); return; }
                e.preventDefault(); focusNext(t);
            }
            return;
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { shortcuts(); return; }
        if (e.ctrlKey && !e.altKey) {
            if (K === 'S') { e.preventDefault(); if (hd) historyShow(); else if ($('#btnSave').is(':visible') && !$('#btnSave').prop('disabled')) save('save'); }
            else if (K === 'U') { e.preventDefault(); if ($('#btnUpdate').is(':visible') && !$('#btnUpdate').prop('disabled')) save('update'); }
            else if (K === 'N') { e.preventDefault(); if (hd) historyReset(); else newClick(); }
            else if (K === 'E') { e.preventDefault(); w.location.href = '/accounts/dashboard'; }
            else if (K === 'T') { e.preventDefault(); if (hd) { switchTab('form'); comboFocus('rtvPaymentType'); } else switchTab('history'); }
            else if (K === 'R') { e.preventDefault(); if (hd) historyAccounts(H('Refresh')); else refreshLists($('#btnRefresh')); }
            else if (k === 'F5') { e.preventDefault(); comboFocus($('#rtvVoucherType').prop('disabled') ? 'rtvPaymentType' : 'rtvVoucherType'); }
            else if (k === 'F10') { e.preventDefault(); attachments(); }
            else if (k === 'ArrowDown') { e.preventDefault(); (hd ? H('Grid') : $('#rtvGrid')).focus(); }
            else if (k === 'ArrowUp') { e.preventDefault(); if (hd) H('From').focus(); else comboFocus('rtvPaymentType'); }
            return;
        }
        if (e.altKey && !e.ctrlKey) {
            if ((k === '1' || e.code === 'Digit1' || e.code === 'Numpad1') && r.print) { e.preventDefault(); printButton(1, $('#btnPrint')[0]); }
            else if ((k === '2' || e.code === 'Digit2' || e.code === 'Numpad2') && r.print) { e.preventDefault(); printButton(2, $('#btnPrint2')[0]); }
            else if ((k === '3' || e.code === 'Digit3' || e.code === 'Numpad3') && r.print) { e.preventDefault(); printButton(3, $('#btnPrint3')[0]); }
        }
    }

    function wireForm() {
        $('#rtvVoucherType').on('change', applyDoc);
        $('#rtvDebit').on('change', function () {
            var id = int($(this).val());
            if (id > 0) balance(id, $('#rtvDrBalance')); else $('#rtvDrBalance').hide().text('0');
        });
        $('#rtvAccount').on('change', function () {                       // combactitle_Leave: balance and the account's subsidiaries
            var id = int($(this).val());
            if (id > 0) balance(id, $('#rtvAccBalance')); else $('#rtvAccBalance').hide().text('0');
            bindSubsidiary(0); subBalance();
        });
        $('#rtvSubsidiary').on('change', subsidiaryChanged);
        $('input[name="rtvAccMode"]').on('change', function () {
            S.accMode = $(this).val();
            var cur = $('#rtvAccount').val();
            $('#rtvAccount').attr('data-dtcombo', S.accMode === 'code' ? 'rtvAcc4c' : 'rtvAcc4')
                .attr('data-dtcombo-caption', S.accMode === 'code' ? 'Account Code' : 'Credit Account')
                .html(accOpts(S.L ? S.L.creditAccounts : [], true, S.accMode));
            if (int(cur) > 0) { $('#rtvAccount').val(cur).trigger('change'); comboFocus('rtvAccount'); }
        });
        $('input[name="rtvRefMode"]').on('change', function () {
            S.refMode = $(this).val();
            var cur = $('#rtvReference').val();
            $('#rtvReference').attr('data-dtcombo', S.refMode === 'code' ? 'rtvAcc4c' : 'rtvAcc4')
                .attr('data-dtcombo-caption', S.refMode === 'code' ? 'Reference Code' : 'Reference Account')
                .html(accOpts(S.L ? S.L.referenceAccounts : [], true, S.refMode));
            if (int(cur) > 0) { $('#rtvReference').val(cur).trigger('change'); comboFocus('rtvReference'); }
        });
        $('#rtvDate').on('change', function () {
            balance(int($('#rtvDebit').val()), $('#rtvDrBalance'));
            if ($('#rtvWht').is(':checked')) whtChanged();
        });
        $('#rtvExchangeRate').on('keydown', decimalOnly).on('input', rateChangedHeader)
            .on('blur', function () { $(this).val(fmtRate($(this).val())); rateChangedHeader(); });
        $('#rtvCurrency').on('change', currencyLeave);
        $('#rtvTcyRate').on('keydown', decimalOnly).on('input', linkAmounts)
            .on('blur', function () { $(this).val(fmtRate($(this).val())); });
        $('#rtvAmount').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); S.amtSrc = 'amt'; linkAmounts(); });
        $('#rtvTcyAmount').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); S.amtSrc = 'fcy'; linkAmounts(); });
        $('#rtvDiscPct').on('keydown', decimalOnly).on('input', discPctChanged);
        $('#rtvDiscAmt').on('keydown', decimalOnly).on('input', discAmtChanged);
        $('#rtvLineRemarks').on('mousedown', function () { S.remarksMouse = true; })
            .on('focus', function () { var el = this; if (!S.remarksMouse) setTimeout(function () { el.select(); }, 0); })
            .on('blur', function () { S.remarksMouse = false; });
        $('#rtvAdd').on('click', add);
        $('#rtvUpdateDetail').on('click', updateDetail);
        $('#rtvCancelDetail').on('click', cancelDetail);
        $('#rtvAdd, #rtvCancelDetail').on('keydown', function (e) { if (e.key === 'Tab' && !e.shiftKey) { e.preventDefault(); comboFocus('rtvPaymentType'); } });
        $('#rtvGrid').on('click', '.prv-del', function (ev) { ev.stopPropagation(); deleteRow(int($(this).data('i'))); });
        $('#rtvGrid').on('click', 'tbody tr[data-i]', function () { $('#rtvGrid tbody tr').removeClass('prv-sel'); $(this).addClass('prv-sel'); });
        $('#rtvGrid').on('dblclick', 'tbody tr[data-i]', function (ev) { if ($(ev.target).is('input,select')) return; editRow(int($(this).data('i'))); });
        $('#rtvGrid').on('keydown', function (e) {
            var sel = $('#rtvGrid tbody tr.prv-sel'); if (!sel.length) sel = $('#rtvGrid tbody tr[data-i]').first();
            if (!sel.length) return;
            var i = int(sel.data('i'));
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRow(i); }
            else if (e.ctrlKey && e.key === ' ') { e.preventDefault(); deleteRow(i); }
            else if (!e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowUp') && !$(e.target).is('select')) {
                var n = S.rows.length, j = i + (e.key === 'ArrowDown' ? 1 : -1);
                if (j >= 0 && j < n) { e.preventDefault(); $('#rtvGrid tbody tr').removeClass('prv-sel').filter('[data-i="' + j + '"]').addClass('prv-sel'); }
            }
        });
        /* in-cell edits (Payment Type, Job/Lot, Remarks, Cost Center, cheque fields are the editable grid columns) */
        $('#rtvGrid').on('change', '.prv-ce', function () {
            var i = int($(this).data('i')), k = $(this).data('k'), r = S.rows[i]; if (!r) return;
            if (k === 'remarks' || k === 'chequeNo' || k === 'payTitle' || k === 'chequeDate') { r[k] = $(this).val(); return; }
            r[k] = int($(this).val());
            var txt = $(this).find('option:selected').text();
            if (k === 'paymentTypeId') r.paymentType = txt; else if (k === 'costCenterId') r.costCenterName = txt; else if (k === 'jobLotId') r.jobLot = txt;
        });
        $('#rtvWht').on('change', whtChanged);                              // checkBox1_CheckedChanged
        $('#rtvTaxType').on('change', function () { whtChanged(); });
        $('#rtvBtnHistory').on('click', function () { switchTab('history'); });
    }

    function wireToolbar() {
        $('#btnNew').on('click', newClick);
        $('#btnRefresh').on('click', function () { refreshLists($(this)); });
        $('#btnSave').on('click', function () { save('save'); });
        $('#btnUpdate').on('click', function () { save('update'); });
        $('#btnAttachment').on('click', attachments);
        $('#btnPrint').on('click', function () { printButton(1, this); });
        $('#btnPrint2').on('click', function () { printButton(2, this); });
        $('#btnPrint3').on('click', function () { printButton(3, this); });
        $('#btnShortcuts').on('click', shortcuts);
        $('#btnSpecialRights').on('click', function () { if (w.SpecialRights) w.SpecialRights.open(SCREEN_ID); });
        $('#tabBtnForm').on('click', function () { switchTab('form'); });
        $('#tabBtnHist').on('click', function () { switchTab('history'); });
        $(document).on('keydown', onKey);
    }

    w.RTV = {
        init: function (cfg) {
            S.mode = int(cfg.mode);
            wireToolbar();
            var ready = $.getJSON(BASE + '/' + S.mode + '/lookups').then(function (L) {
                bindLookups(L);
                renderHistory();
            }, function (x) { alert(errMsg(x, 'Could not load the form lists.')); });
            var id = int(new URLSearchParams(w.location.search).get('id'));
            ready.then(function () { specialRights(); if (id > 0) loadForEdit(id); });
            return ready;
        },
        loadForEdit: function (id) { return loadForEdit(id); },
        save: function () { save($('#btnUpdate').is(':visible') ? 'update' : 'save'); },
        reset: newClick,
        print: function () { printButton(1, $('#btnPrint')[0]); },
        switchTab: switchTab
    };
}(window, jQuery));
