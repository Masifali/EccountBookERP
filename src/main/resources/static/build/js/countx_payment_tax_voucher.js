/* ============================================================================================
 * Screens 850 PaymentVoucherWithTax, 851 frmBankPaymentVoucherTax, 852 frmCashPaymentVoucherTax.
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew - one class opened
 * by its Tag (frmCashPaymentVoucherTax = Cash, frmBankPaymentVoucherTax = Bank, PaymentVoucherWithTax =
 * Voucher Type combo with both). Mode 0 = 850, 1 = 852 (cash), 2 = 851 (bank).
 *
 * The Form tab AND the CPV / BPV history tabs are rendered here. Backend:
 * /accounts/api/payment-tax/{mode}/... (PaymentTaxVoucherController): lookups from the desktop's own fill
 * methods, Save / Update through the desktop procedure chain (DesktopVoucherWriter), ReadById through
 * Sp_Vouchers_GetMethods, history through USP_VoucherFormHistory. The page posts grid ROWS and the header
 * controls; the ledger lines (vd, vd2, WHT vd3/vd4, SRB vd5/vd6, discount vd7/vd8) are built on the server.
 *
 * Layout numbers are the Location / Size values of InitializeComponent() (tab page 1545x783; the 30 px
 * toolstrip is in the template, so the designer y values are 30 less here).
 * ============================================================================================ */
(function (w, $) {
    'use strict';

    var BASE = '/accounts/api/payment-tax';
    var TITLE = { 1: 'Cash Payment Voucher', 2: 'Bank Payment Voucher' };
    /* CommonServices.PaymentAndReceiptVoucherSlip: the 102-III report per DocumentTypeId. */
    var RPT3 = { 1: '102-CashPaymentVoucher.rpt', 2: '102-BankPaymentVoucher.rpt' };
    /* ScreenId = CommonServices.GetScreenIdByName(base.Name) with base.Name = "PaymentVoucherNew". */
    var SCREEN_ID = 21;

    var S = {
        mode: 0, doc: 1, L: null, F: {}, rows: [], recId: 0, updateMode: false,
        editIndex: -1, updIdx: -1, editCheque: null, cheques: [], busy: false,
        accMode: 'title', refMode: 'title', remarksMouse: false, codeSeq: 0, chqSeq: 0,
        subLen: 0, discSrc: '', amtSrc: '', sched: {}, histTaxSum: 0,
        hist: { 1: { rows: [], sel: -1, busy: false }, 2: { rows: [], sel: -1, busy: false } }
    };
    /* print-rpt.js reads the open voucher's id */
    try { Object.defineProperty(w, 'RecId', { get: function () { return S.recId; }, configurable: true });
          Object.defineProperty(w, 'VoucherHeadId', { get: function () { return S.recId; }, configurable: true }); } catch (e) { }

    if (w.DesktopCombo) {
        /* DetailAccountFill / GetAccountsFromGlobalByTypeIds: AccountTitle | AccountCode | ParentAccountTitle | AccountClass (Id hidden). */
        w.DesktopCombo.define('ptvAcc4', [
            { caption: 'Account Title', flex: 4 }, { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' }, { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('ptvAcc4c', [
            { caption: 'Account Code', flex: 2 }, { caption: 'AccountTitle', flex: 4, key: 'title' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' }, { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('ptvAcc2', [{ caption: 'Account Title', flex: 4 }, { caption: 'AccountCode', flex: 2, key: 'code' }]);
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
            var b = $(this); if (b.data('ptvBusy')) return;
            b.data('ptvBusy', 1).data('ptvHtml', b.html()).prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> ' + $.trim(b.text()));
        });
    }
    function busyOff($b) {
        $b.each(function () {
            var b = $(this); if (!b.data('ptvBusy')) return;
            b.html(b.data('ptvHtml')).removeData('ptvBusy').prop('disabled', false);
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
        var title = S.mode === 1 ? 'Cash Payment Voucher' : S.mode === 2 ? 'Bank PaymentVoucher' : 'PAYMENT VOUCHER';
        /* ---- panel4 (Dock Top, 1541x36, BackColor 10,110,110); the check boxes are anchored Top|Right */
        var strip = '<div class="prv-strip" style="height:36px;">' +
            '<span class="prv-strip-title" style="position:absolute;left:7px;top:8px;" id="ptvTitle">' + title + '</span>' +
            '<label class="prv-l" style="color:#fff;font:bold 9pt \'Segoe UI\',sans-serif;' + px(255, 11) + '">Location Type</label>' +
            cmb('ptvLocation', 344, 5, 212, 26) +
            '<div class="prv-ar" style="width:1541px;height:36px;">' +
            '<label class="prv-v" id="ptvLblPrintOnSave" style="display:none;' + px(928, 6, 183) + '"><input type="checkbox" id="ptvCbPrintOnSave"/>Cheque Print On Save</label>' +
            '<label style="' + px(1128, 8, 102) + '"><input type="checkbox" id="ptvChkPrint1" checked/>Print Preview</label>' +
            '<label style="' + px(1236, 8, 126) + '"><input type="checkbox" id="ptvChkPrint2"/>Preview Format II</label>' +
            '<label style="' + px(1363, 8, 126) + '"><input type="checkbox" id="ptvChkPrint3"/>Preview Format II</label>' +
            '</div></div>';
        /* ---- groupBox2 "Main" (1,68) 1173x128 */
        var main =
            lab(14, 16, 'Cost Center') + cmb('ptvProject', 111, 11, 240, 24, '', 'prv-cmb-mss') +
            lab(14, 43, 'Voucher Type') + cmb('ptvVoucherType', 111, 37, 240, 26) +
            lab(14, 70, 'Voucher No') + tb('ptvVoucherNo', 111, 66, 74, 23, ' readonly') +
            lab(190, 70, 'Date') + dtp('ptvDate', 229, 66, 122, 23) +
            lab(14, 97, 'Credit Account') + cmb('ptvCredit', 111, 91, 240, 26, ' data-dtcombo="ptvAcc2" data-dtcombo-caption="Credit Account"') +
            '<span id="ptvCrBalance" class="prv-bal" style="display:none;color:#f00;' + px(354, 96) + '"></span>' +
            lab(373, 16, 'WHT Debit Ac') + cmb('ptvAgainstAc', 465, 10, 373, 26, ' data-dtcombo="ptvAcc2" disabled') +
            lab(373, 39, 'Remarks', 'prv-tah') + '<textarea id="ptvRemarks" class="prv-tb" style="' + px(465, 41, 373, 76) + '"></textarea>' +
            lab(843, 16, 'Tcy Code') + cmb('ptvCurrency', 924, 10, 111, 26) +
            lab(843, 36, 'Tcy Exchange<br/>Rate') + tb('ptvExchangeRate', 924, 40, 111, 23, '', 'prv-num') +
            lab(843, 70, 'Tcy Amount') + tb('ptvFcyAmount', 924, 66, 111, 23, ' readonly tabindex="-1"', 'prv-num prv-dis') +
            '<label class="prv-l" style="' + px(915, 97) + '"><input type="checkbox" id="ptvCustom" style="vertical-align:middle;margin:0 3px 0 0;"/>Custom Accounts</label>';

        /* ---- groupboxdetail "Detail" (2,167): flowLayoutPanel1 (1,15) 1165 wide; panels laid out by layoutDetail() */
        var P = '';
        P += pnl('ptvPPay', lab(1, 4, 'Payment Type') + cmb('ptvPaymentType', 1, 21, 86, 26));
        P += pnl('ptvPAcc', lab(1, 4, 'Debit Ac') +
            '<label class="prv-rb" style="' + px(54, 2) + '"><input type="radio" name="ptvAccMode" value="title" checked/>Title</label>' +
            '<label class="prv-rb" style="' + px(101, 2) + '"><input type="radio" name="ptvAccMode" value="code"/>Code</label>' +
            '<span id="ptvAccBalance" class="prv-bal" style="display:none;color:#f00;' + px(150, 4) + '"></span>' +
            cmb('ptvAccount', 0, 21, 263, 26, ' data-dtcombo="ptvAcc4" data-dtcombo-caption="Account Title"'));
        P += pnl('ptvPSub', lab(0, 3, 'Subsidiary A/c') + '<span id="ptvSubBalance" class="prv-bal" style="display:none;color:#f00;' + px(84, 3) + '"></span>' +
            cmb('ptvSubsidiary', 0, 21, 168, 26));
        P += pnl('ptvPJob', lab(0, 2, 'Job/Lot') + cmb('ptvJobLot', 0, 21, 135, 26));
        P += pnl('ptvPRem', lab(0, 3, 'Remarks') + tb('ptvLineRemarks', 0, 22, 278, 23));
        P += pnl('ptvPTcy', lab(0, 2, 'Tcy Code') + cmb('ptvTcyCode', 1, 21, 112, 26));
        P += pnl('ptvPRate', lab(1, 3, 'Tcy Exchange Rate') + tb('ptvTcyRate', 0, 22, 120, 23, '', 'prv-num'));
        P += pnl('ptvPFcy', lab(1, 3, 'Tcy Amount') + tb('ptvTcyAmount', 0, 22, 117, 23, '', 'prv-num'));
        P += pnl('ptvPAmt', lab(1, 3, 'Debit Amount') + tb('ptvAmount', 1, 22, 114, 23, '', 'prv-num'));
        P += pnl('ptvPRef', lab(4, 3, 'Reference Ac') +
            '<label class="prv-rb" style="' + px(75, 2) + '"><input type="radio" name="ptvRefMode" value="title" checked/>Title</label>' +
            '<label class="prv-rb" style="' + px(119, 2) + '"><input type="radio" name="ptvRefMode" value="code"/>Code</label>' +
            cmb('ptvReference', 2, 21, 168, 26, ' data-dtcombo="ptvAcc4" data-dtcombo-caption="Reference Account"'));
        P += pnl('ptvPFi', lab(1, 2, 'Financial Instrument') + cmb('ptvInstrument', 1, 19, 116, 26));
        P += pnl('ptvPCd', lab(4, 3, 'Cheque Date') + dtp('ptvChequeDate', 1, 21, 113, 23));
        P += pnl('ptvPCn', lab(0, 1, 'Cheque No') +
            '<input type="text" id="ptvChequeNo" class="prv-tb" list="ptvChequeList" autocomplete="off" maxlength="50" style="' + px(1, 19, 130, 26) + '"/><datalist id="ptvChequeList"></datalist>');
        P += pnl('ptvPPt', lab(0, 1, 'Payee Title') + tb('ptvPayeeTitle', 1, 19, 157, 26, ' maxlength="100"'));
        P += pnl('ptvPCt', lab(1, 1, 'Cheque Type') + cmb('ptvChequeType', 1, 19, 98, 26));
        P += pnl('ptvPBr', lab(1, 1, 'Branch Name') + cmb('ptvBranch', 1, 19, 142, 26));
        P += pnl('ptvPCc', lab(2, 1, 'Cost Center') + cmb('ptvCostCenter', 1, 19, 120, 26));
        P += pnl('ptvPBtn',
            '<button type="button" id="ptvAdd" class="prv-fbtn" style="' + px(1, 19, 26, 25) + '">+</button>' +
            '<button type="button" id="ptvUpdateDetail" class="prv-fbtn" style="display:none;' + px(1, 19, 59, 25) + '">Update</button>' +
            '<button type="button" id="ptvCancelDetail" class="prv-fbtn" style="display:none;' + px(59, 19, 57, 25) + '">Cancel</button>');
        var top = '<div class="prv-abs" id="ptvTop" style="height:282px;">' + strip +
            gbox('Main', 1, 38, 1173, 128, main) +
            gbox('Detail', 2, 167, 1172, 113, '<div class="prv-g" id="ptvFlow" style="' + px(1, 15) + '">' + P + '</div>', 'ptvDetailBox') +
            '</div>';
        var grid = '<div class="prv-mid"><div class="prv-gridbox" style="right:0;" id="ptvGridBox"><table class="prv-jg" id="ptvGrid" tabindex="0"><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
        /* ---- panel17 (Dock Bottom, 157 high), its controls anchored Top|Right of a 1541 wide panel */
        var bot = '<div class="prv-bot prv-abs" style="height:157px;"><div class="prv-ar" style="width:1541px;height:157px;">' +
            '<label class="prv-rb prv-v" style="' + px(1075, 8) + '"><input type="radio" name="ptvTaxMode" value="excluded" id="ptvRadExcluded" checked/>Excluded Tax</label>' +
            '<label class="prv-rb prv-v" style="' + px(1199, 8) + '"><input type="radio" name="ptvTaxMode" value="included" id="ptvRadIncluded"/>Included Tax</label>' +
            lab(1315, 9, 'Amount') + tb('ptvValue', 1396, 4, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
            lab(874, 35, 'SRB Tax Account (Cr)') + cmb('ptvSrbAcc', 1002, 30, 191, 24, ' data-dtcombo="ptvAcc2" data-dtcombo-caption="SRB Account"', 'prv-cmb-mss') +
            lab(1315, 35, 'SRB Amount') + tb('ptvSrbAmount', 1396, 30, 142, 25, '', 'prv-num prv-seg') +
            lab(873, 61, 'Discount Account (Cr)') + cmb('ptvDiscAcc', 1002, 56, 191, 24, ' data-dtcombo="ptvAcc2" data-dtcombo-caption="Discount Account"', 'prv-cmb-mss') +
            lab(1194, 60, 'Disc %', 'prv-seg') + tb('ptvDiscPct', 1240, 56, 72, 25, '', 'prv-seg') +
            lab(1315, 53, 'Discount<br/>Amount', null, null, 75) + tb('ptvDiscAmt', 1396, 56, 142, 25, '', 'prv-num prv-seg') +
            lab(667, 87, 'TaxType') + cmb('ptvTaxType', 724, 82, 92, 24, '', 'prv-cmb-mss') +
            '<label class="prv-rb prv-plain" style="' + px(821, 84) + '"><input type="checkbox" id="ptvWht"/>WHT</label>' +
            lab(874, 87, 'WHT Credit Account') + cmb('ptvWithHoldingAc', 1002, 82, 191, 24, ' data-dtcombo="ptvAcc2" disabled', 'prv-cmb-mss') +
            lab(1194, 86, 'Tax %', 'prv-seg') + tb('ptvTaxPercent', 1240, 82, 72, 25, ' readonly value="0" tabindex="-1"', 'prv-seg') +
            lab(1315, 87, 'Wht Amount') + tb('ptvTaxAmount', 1396, 82, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
            lab(1315, 113, 'Total Amount') + tb('ptvTotal', 1396, 108, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
            '<button type="button" class="prv-sysbtn" id="ptvBtnHistory" style="left:1446px;top:133px;width:92px;height:22px;"><i class="fa fa-history"></i>History</button>' +
            '</div></div>';
        $('#prvForm').html(top + grid + bot);
    }

    /**
     * flowLayoutPanel1: the visible panels left to right, wrapped at 1165. Bank-only panels follow the
     * Voucher Type (AccountTitleFill); Subsidiary / Branch / Cost Center follow the Load-time features.
     * groupboxdetail / ParentPanelOfMainDetail heights: 162 / 359 when (booking office or branch) on a
     * bank voucher, else 113 / 312 (the toolstrip row is not part of the page, so 30 less).
     */
    function layoutDetail() {
        var F = S.F, bank = S.doc === 2;
        var subs = !!F.subsidiaryFeature, branch = !!F.branchFeature, booking = !!F.isBookingOffice;
        var remW = subs ? 278 : 446;
        var P = [['ptvPPay', 87, true], ['ptvPAcc', 263, true], ['ptvPSub', 168, subs], ['ptvPJob', 135, true], ['ptvPRem', remW, true],
                 ['ptvPTcy', 112, true], ['ptvPRate', 120, true], ['ptvPFcy', 117, true], ['ptvPAmt', 117, true], ['ptvPRef', 170, true],
                 ['ptvPFi', 116, bank], ['ptvPCd', 114, bank], ['ptvPCn', 130, bank], ['ptvPPt', 158, bank], ['ptvPCt', 98, bank],
                 ['ptvPBr', 143, branch], ['ptvPCc', 121, booking], ['ptvPBtn', 117, true]];
        var x = 0, y = 0;
        P.forEach(function (p) {
            var $p = $('#' + p[0]);
            if (!p[2]) { $p.hide(); return; }
            if (x > 0 && x + p[1] > 1165) { x = 0; y += 47; }
            $p.css({ left: x + 'px', top: y + 'px', width: p[1] + 'px', display: 'block' });
            x += p[1];
        });
        $('#ptvLineRemarks').css('width', (remW - (subs ? 0 : 2)) + 'px');
        var tall = (booking || branch) && bank;
        $('#ptvFlow').css('height', (tall ? 144 : 96) + 'px');
        $('#ptvDetailBox').css('height', (tall ? 162 : 113) + 'px');
        $('#ptvTop').css('height', ((tall ? 359 : 312) - 30) + 'px');
    }

    // ------------------------------------------------------------------------------ lookups

    var COMBOS = ['ptvProject', 'ptvVoucherType', 'ptvLocation', 'ptvCredit', 'ptvAgainstAc', 'ptvWithHoldingAc', 'ptvPaymentType', 'ptvAccount',
                  'ptvSubsidiary', 'ptvJobLot', 'ptvTcyCode', 'ptvCurrency', 'ptvReference', 'ptvInstrument', 'ptvChequeType', 'ptvBranch',
                  'ptvCostCenter', 'ptvTaxType', 'ptvSrbAcc', 'ptvDiscAcc'];
    function capture() { var o = {}; COMBOS.forEach(function (id) { o[id] = $('#' + id).val(); }); return o; }
    function restore(o, ids) { ids.forEach(function (id) { if (o[id] != null) put(id, o[id]); }); }

    /** AccountTitleFill: the credit account list of the current voucher type. */
    function fillCredit() {
        var list = ((S.L && S.L.headerAccounts) || {})[String(S.doc)] || [];
        $('#ptvCredit').html(accOpts(list, true, 'title'));                         // DDL.BindDDL ZeroIndex: true
    }

    function fillLists(L) {
        $('#ptvProject').html(opts(L.projects, 'Id', 'ProjectName', false));          // BindDDLNew(..., false)
        $('#ptvVoucherType').html(opts(L.voucherTypes, 'Id', 'Name', false));
        $('#ptvLocation').html(opts(L.locationTypes, 'Id', 'Location', true));        // ZeroIndex: true
        fillCredit();
        $('#ptvAccount').html(accOpts(L.detailAccounts, true, S.accMode));
        $('#ptvReference').html(accOpts(L.referenceAccounts, true, S.refMode));
        $('#ptvAgainstAc, #ptvWithHoldingAc').html(accOpts(L.whtAccounts, true, 'title'));
        $('#ptvSrbAcc').html(accOpts(L.srbAccounts, true, 'title'));
        $('#ptvDiscAcc').html(accOpts(L.discountAccounts, true, 'title'));
        $('#ptvPaymentType').html(opts(L.paymentTypes, 'Id', 'PaymentType', false));
        $('#ptvJobLot').html(opts(L.jobLots, 'Id', 'JobLotDescription', true));       // ZeroIndex: true
        $('#ptvCurrency, #ptvTcyCode').html(opts(L.currencies, 'Id', 'CurrencyCode', true));
        $('#ptvTaxType').html(opts(L.taxTypes, 'Id', 'TaxName', true));
        $('#ptvBranch').html(opts(L.branches, 'BranchId', 'BranchName', false));
        $('#ptvCostCenter').html(opts(L.costCenters, 'Id', 'CostCenterName', false));
        $('#ptvInstrument').html(opts(L.instrumentTypes, 'Id', 'InstrumentType', false));
        $('#ptvChequeType').html(opts(L.chequeTypes, 'id', 'CheqType', true));
        bindSubsidiary(0);
    }

    function bindLookups(L) {
        S.L = L; S.F = L.flags || {};
        S.doc = int(L.startDoc) || 1;
        render();
        fillLists(L);
        put('ptvVoucherType', S.doc);
        layoutDetail();
        $('#ptvLblPrintOnSave').toggle(!!S.F.chequePrintingEnable && S.doc === 2);   // cbPrintOnSave.Visible
        $('#ptvCbPrintOnSave').prop('checked', false);
        $('#btnPrint').prop('disabled', !rights().print);                             // Print.Enabled = DoHavePrintRights
        wireForm();
        reset(true);
    }

    /** DefaultConfigurations(): Job/Lot, BaseCurrencyRate, Base Currency (-> cmbCurrency_Leave), default SRB / discount accounts. */
    function defaults() {
        if (S.F.defaultJobLotId != null && int(S.F.defaultJobLotId) > 0) setVal('ptvJobLot', S.F.defaultJobLotId);
        if (S.F.baseCurrencyRate != null) $('#ptvExchangeRate').val(fmtRate(S.F.baseCurrencyRate));
        if (int(S.F.baseCurrencyId) > 0) { put('ptvCurrency', S.F.baseCurrencyId); currencyLeave(); }
        if (int(S.F.defaultSrbAccountId) > 0) put('ptvSrbAcc', S.F.defaultSrbAccountId);
        if (int(S.F.defaultDiscountAccountId) > 0) put('ptvDiscAcc', S.F.defaultDiscountAccountId);
    }

    /** btnRefresh_Click: lists re-bound in place, DefaultConfigurations, SpecialRightsImplement, combos of the grid. */
    function refreshLists($b) {
        if (S.busy) return;
        busyOn($b);
        $.getJSON(api('/lookups')).then(function (L) {
            var keep = capture();
            S.L = L; S.F = L.flags || S.F; S.sched = {};
            fillLists(L);
            restore(keep, ['ptvProject', 'ptvVoucherType', 'ptvLocation', 'ptvCredit', 'ptvAccount', 'ptvReference', 'ptvAgainstAc', 'ptvWithHoldingAc',
                           'ptvPaymentType', 'ptvJobLot', 'ptvCurrency', 'ptvTcyCode', 'ptvCostCenter', 'ptvTaxType', 'ptvSrbAcc', 'ptvDiscAcc']);
            if (S.F.defaultBranchId) setVal('ptvBranch', S.F.defaultBranchId);      // BranchesFill: Text = UserAccount.BranchName
            firstRow('ptvInstrument');                                              // GetFinancialInstrumentTypes: Rows[0].Activate
            defaults();
            layoutDetail();
            specialRights();
            chequeFill();
            renderGrid();
        }, function (x) { alert(errMsg(x, 'Could not refresh the lists.')); }).always(function () { busyOff($b); });
    }

    // ------------------------------------------------------------------------------ grid

    /** grdSettings(): visible columns, captions, order and widths. JobLotId, ids and GlCurrency are hidden. */
    function cols() {
        var F = S.F, bank = S.doc === 2;
        var c = [{ k: 'x', t: 'X', w: 20 }, { k: 'paymentType', t: 'Payment Type', ed: 'pt', w: 90 }, { k: 'accountCode', t: 'AccountCode', w: 90 },
                 { k: 'accountTitle', t: 'AccountTitle', w: 220 }];
        if (F.subsidiaryFeature) c.push({ k: 'subsidiaryAccount', t: 'SubsidiaryAccount', w: 180 });
        c.push({ k: 'remarks', t: 'Remarks', ed: 'txt', w: 220 }, { k: 'tcyCode', t: 'TcyCode', w: 80 },
               { k: 'tcyExchangeRate', t: 'TcyExchangeRate', n: 2, w: 105 }, { k: 'fcyAmount', t: 'Tcy Amount', n: 3, sum: 3, w: 110 },
               { k: 'amount', t: 'Debit Amount', n: 1, sum: 1, w: 110 }, { k: 'referenceAccount', t: 'ReferenceAccount', w: 150 });
        if (bank) c.push({ k: 'financialInstrument', t: 'FinancialInstrument' }, { k: 'chequeDate', t: 'ChequeDate', dt: 1 },
                         { k: 'chequeNo', t: 'ChequeNo' }, { k: 'payeeTitle', t: 'PayeeTitle' });
        if (F.branchFeature) c.push({ k: 'branchName', t: 'BranchName', w: 130 });
        if (F.isBookingOffice) c.push({ k: 'costCenterName', t: 'CostCenterId', ed: 'cc', w: 130 });
        if ($('#ptvRadIncluded').is(':checked')) c.push({ k: 'taxAmount', t: 'TaxAmount', n: 1 });
        return c;
    }

    /* Janus GridEX: ColumnAutoResize = true for cash (columns spread over the grid), false for bank (designer widths, scrolls). */
    function renderGridHead() {
        var cs = cols(), sum = cs.reduce(function (s, c) { return s + (c.w || 100); }, 0);
        var auto = S.doc !== 2, boxW = $('#ptvGridBox').innerWidth() || 1500;
        var scale = auto && sum > 0 ? Math.max(1, (boxW - 2) / sum) : 1;
        $('#ptvGrid').css('width', auto ? '100%' : sum + 'px');
        $('#ptvGrid colgroup').html(cs.map(function (c) { return '<col style="width:' + Math.round((c.w || 100) * scale) + 'px;"/>'; }).join(''));
        $('#ptvGrid thead').html('<tr>' + cs.map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + ' title="' + esc(c.t) + '">' + c.t + '</th>'; }).join('') + '</tr>');
    }

    function cellEditor(c, r, i) {
        var L = S.L || {};
        if (c.ed === 'txt') return '<input type="text" class="prv-ce" data-i="' + i + '" data-k="remarks" value="' + esc(r.remarks) + '"/>';
        var list, idK, txK, cur, key;
        if (c.ed === 'pt') { list = L.paymentTypes; idK = 'Id'; txK = 'PaymentType'; cur = r.paymentTypeId; key = 'paymentTypeId'; }
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
                if (c.k === 'amount' || c.k === 'taxAmount') v = fmt(v);
                else if (c.k === 'fcyAmount') v = fmt3(v);
                else if (c.k === 'tcyExchangeRate') v = fmt3(v);
                else if (c.dt) v = dmy(day(v));
                return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $('#ptvGrid tbody').html(html || '<tr><td colspan="' + cs.length + '" class="prv-empty"></td></tr>');
        var sumA = baseAmount(), sumF = S.rows.reduce(function (s, r) { return s + num(r.fcyAmount); }, 0);
        $('#ptvGrid tfoot').html(S.rows.length ? '<tr>' + cs.map(function (c) {
            return '<td class="num" style="font-weight:bold;">' + (c.sum === 1 ? fmt(sumA) : c.sum === 3 ? fmt3(sumF) : '') + '</td>';
        }).join('') + '</tr>' : '');
        // CalculateTotalInformation(): txtFcyAmount = SUM(FcyAmount) "#,##0.###", "0" with no rows
        $('#ptvFcyAmount').val(S.rows.length ? fmt3(sumF) : '0');
    }

    // ------------------------------------------------------------------------------ Total() / WHT / SRB / discount

    function baseAmount() { return S.rows.reduce(function (s, r) { return s + num(r.amount); }, 0); }
    var T = { value: 0, tax: 0, total: 0 };
    function showBoxes() {
        $('#ptvValue').val(fmt(T.value)); $('#ptvTaxAmount').val(fmt(T.tax)); $('#ptvTotal').val(fmt(T.total));
    }

    /** Total(): Excluded = (base+srb+disc)/(100-rate)*rate on top; Included = the payable is split. Nothing changes when the grid sums to 0. */
    function ptotal() {
        var rate = num($('#ptvTaxPercent').val()), srb = num($('#ptvSrbAmount').val()), disc = num($('#ptvDiscAmt').val());
        var base = baseAmount(), tax, tot, val = base;
        if (base > 0) {
            if ($('#ptvRadExcluded').is(':checked')) { tax = (base + srb + disc) / (100 - rate) * rate; tot = base + tax + srb + disc; }
            else { var n = base + srb + disc; val = n / 100 * (100 - rate); tot = n; tax = tot - val; }
            T.value = round(val, dec()); T.tax = round(tax, dec()); T.total = round(tot, dec());
            showBoxes();
            proportion();
        }
    }

    /** TaxAmountProportion(): row TaxAmount only in Included mode (MidpointRounding.AwayFromZero). */
    function proportion() {
        var inc = $('#ptvRadIncluded').is(':checked');
        var totalTax = round(T.tax, dec()), totalDetail = baseAmount();
        S.rows.forEach(function (r) {
            r.taxAmount = (inc && totalTax > 0 && totalDetail > 0) ? round(totalTax / totalDetail * num(r.amount), dec()) : 0;
        });
        renderGrid();
    }

    function whtEnable(on) { $('#ptvAgainstAc, #ptvWithHoldingAc').prop('disabled', !on); }

    function schedule() {
        var key = int($('#ptvTaxType').val()) + '|' + $('#ptvDate').val();
        if (S.sched[key] !== undefined) return $.Deferred().resolve(S.sched[key]).promise();
        return $.getJSON(BASE + '/tax-schedule', { taxTypeId: int($('#ptvTaxType').val()), date: $('#ptvDate').val() })
            .then(function (s) { S.sched[key] = s; return s; });
    }

    /** checkBox1_CheckedChanged() (also textBox1 / txtSrbTaxAmount / txtDiscount TextChanged): the schedule, then Total(). */
    function whtChanged() {
        if (!$('#ptvWht').is(':checked')) {
            whtEnable(false);
            $('#ptvTaxPercent').val('0'); T.tax = 0;
            T.total = T.value + T.tax; showBoxes();
            ptotal();
            return $.Deferred().resolve().promise();
        }
        return schedule().then(function (s) {
            if (!s || !s.found) {
                whtEnable(false);
                put('ptvWithHoldingAc', 0); put('ptvAgainstAc', 0);
                $('#ptvTaxPercent').val('0'); T.tax = 0; showBoxes();
            } else {
                $('#ptvTaxPercent').val(String(s.taxPercent));
                if (!S.updateMode) put('ptvWithHoldingAc', s.taxGLAccountId);
                whtEnable(true);
                if (S.rows.length && int($('#ptvAgainstAc').val()) === 0) put('ptvAgainstAc', S.rows[0].accountId);
            }
            ptotal();
        }, function (x) { alert('Error applying withholding tax: ' + errMsg(x)); });
    }

    /** CommonServices.UpdateRelatedFieldsCalculation for Disc % <-> Discount Amount (factor = txtValue, factor2 = 100). */
    function discPctChanged() {
        S.discSrc = 'pct';
        var pct = num($('#ptvDiscPct').val());
        if (pct > 0) {
            if (T.value === 0) comboFocus('ptvPaymentType');
            if (T.value <= 0) { $('#ptvDiscAmt').val('0'); $('#ptvDiscPct').val('0'); }
            else $('#ptvDiscAmt').val(String(round(pct * T.value / 100, dec())));
        } else $('#ptvDiscAmt').val('0');
        whtChanged();
    }
    function discAmtChanged() {
        S.discSrc = 'amt';
        var amt = num($('#ptvDiscAmt').val());
        if (amt > 0) {
            if (T.value === 0) comboFocus('ptvPaymentType');
            if (T.value <= 0) { $('#ptvDiscAmt').val('0'); $('#ptvDiscPct').val('0'); }
            else $('#ptvDiscPct').val(String(round(amt * 100 / T.value, 3)));
        } else $('#ptvDiscPct').val('0');
        whtChanged();
    }
    function srbAmtChanged() { whtChanged(); }

    // ------------------------------------------------------------------------------ detail entry

    function balance(accountId, $lbl) {
        if (!(accountId > 0)) { $lbl.hide().text('0'); return; }
        $.getJSON(BASE + '/balance', { accountId: accountId, date: $('#ptvDate').val() }).then(function (r) { $lbl.text(bal(r && r.balance)).show(); });
    }
    function subBalance() {
        var acc = int($('#ptvAccount').val()), sub = int($('#ptvSubsidiary').val()), $l = $('#ptvSubBalance');
        if (acc > 0 && sub > 0) {
            $.getJSON(BASE + '/subsidiary-balance', { accountId: acc, subsidiaryId: sub, date: $('#ptvDate').val() }).then(function (r) { $l.text(bal(r && r.balance)).show(); });
        } else $l.hide().text('0');
    }

    /** BindSubsidiaryAccount(): the subsidiaries of the selected debit account (all of them with no account), retaining retainId. */
    function bindSubsidiary(retainId) {
        var all = (S.L && S.L.subsidiaries) || [], list = all, acc = int($('#ptvAccount').val());
        var $s = $('#ptvSubsidiary');
        if (!all.length) { S.subLen = 0; $s.html('<option value="0"></option>'); return; }
        if ($('#ptvAccount option:selected').text() !== '' && acc > 0) {
            list = all.filter(function (r) { return int(ci(r, 'AccountId')) === acc; });
        } else if (acc === 0 && $('#ptvAccount').val() !== null && $('#ptvAccount option:selected').text() === '') {
            list = all;
        }
        if (!list.length) { S.subLen = 0; $s.html('<option value="0"></option>'); return; }
        S.subLen = list.length;
        $s.html('<option value="0"></option>' + list.map(function (r) {
            return '<option value="' + esc(ci(r, 'Id')) + '" data-type="' + esc(ci(r, 'SubsidiaryTypeId')) + '" data-acc="' + esc(ci(r, 'AccountId')) +
                   '">' + esc(ci(r, 'SubsidiaryAccount')) + '</option>';
        }).join(''));
        put('ptvSubsidiary', retainId || 0);
    }

    /** CmbSubsidiaryAccount_Leave: the subsidiary's own GL account becomes the debit account. */
    function subsidiaryChanged() {
        var $o = $('#ptvSubsidiary option:selected'), sub = int($('#ptvSubsidiary').val());
        if (sub > 0) {
            var gl = int($o.attr('data-acc'));
            if (gl !== 0 && gl !== int($('#ptvAccount').val())) { put('ptvAccount', gl); balance(gl, $('#ptvAccBalance')); bindSubsidiary(sub); }
        }
        subBalance();
    }

    /** CheqNoFill(): BPV only, only with "CheqBook Enabled"; the leaves of the credit bank, plus this voucher's own when editing. */
    function chequeFill() {
        S.cheques = [];
        $('#ptvChequeList').empty();
        var bank = int($('#ptvCredit').val());
        if (S.doc !== 2 || !S.F.chequeBookEnabled) return;
        $('#ptvChequeNo').val('');
        if (!(bank > 0)) return;
        var seq = ++S.chqSeq;
        $.getJSON('/accounts/api/vouchers/outstanding-cheques', { bankId: bank, recId: S.recId }).then(function (list) {
            if (seq !== S.chqSeq) return;
            S.cheques = list || [];
            $('#ptvChequeList').html(S.cheques.map(function (c) { return '<option value="' + esc(c.cheqNo) + '"></option>'; }).join(''));
        });
    }
    function chequeIdFor(no) {
        for (var i = 0; i < S.cheques.length; i++) if (String(S.cheques[i].cheqNo) === String(no)) return int(S.cheques[i].id);
        if (S.editCheque && S.editCheque.id > 0 && String(S.editCheque.no) === String(no)) return S.editCheque.id;
        return 0;
    }

    /** CheckingChequeNoSerialWise() (uses updateDetailIndex, which only Reset() clears). */
    function chequeSerial(no) {
        if (!S.cheques.length) return null;
        var all = S.cheques.map(function (c) { return int(c.cheqNo); });
        var n = int(no);
        if (!S.rows.length) return n !== Math.min.apply(null, all) ? 'Please Insert Cheque No In Detail Grid Serial Wise' : null;
        var distinct = S.rows.map(function (r) { return int(r.chequeNo); }).filter(function (v, i, a) { return a.indexOf(v) === i; });
        if (distinct.length === 1) {
            var rest = all.filter(function (v) { return v !== distinct[0]; });
            if (!rest.length) return 'The source contains no DataRows.';
            return n > Math.min.apply(null, rest) ? 'Please Insert Cheque No In Detail Grid Serial Wise' : null;
        }
        var min = Math.min.apply(null, distinct), max = Math.max.apply(null, distinct);
        if (n < max && S.updIdx < 0) return 'Please Insert Cheque No In Detail Grid Serial Wise! You Inserting ' + no + ' ChequeNo And in Grid ' + max + ' ChequeNo Is Present';
        var outside = all.filter(function (v) { return v < min || v > max; });
        if (!outside.length) return 'Sequence contains no elements';
        var min3 = Math.min.apply(null, outside);
        if (n >= min && n < max && S.updIdx < 0) return 'Please Insert Cheque No In Detail Grid Serial Wise';
        if (n > min3) return 'Please Insert Cheque No In Detail Grid Serial Wise';
        return null;
    }

    /** FormValidationDetail() - Add and Update share it. */
    function detailValid() {
        var F = S.F, bank = S.doc === 2, ins = int($('#ptvInstrument').val());
        if (!$('#ptvPaymentType').val()) return ['PaymentType Field is Required', 'ptvPaymentType'];
        if (int($('#ptvCredit').val()) === 0) return ['Credit Account Field is Required', 'ptvCredit'];
        if (int($('#ptvAccount').val()) === 0) return ['Debit Account Field is Required', 'ptvAccount'];
        if (F.subsidiaryFeature && S.subLen > 0 && int($('#ptvSubsidiary').val()) === 0) return ['Subsidiary Account Field is Required', 'ptvSubsidiary'];
        if (int($('#ptvJobLot').val()) === 0) return ['Job/Lot Field is Required', 'ptvJobLot'];
        if (bank && !$('#ptvInstrument').val()) return ['Financial Instrument Field is Required', 'ptvInstrument'];
        if (bank && ins === 1 && F.chequeNoCompulsoryOnBpv && chequeIdFor($('#ptvChequeNo').val()) === 0) return ['Cheque_number Field is Required', 'ptvChequeNo'];
        if (int($('#ptvTcyCode').val()) === 0) return ['Tcy Code Field is Required', 'ptvTcyCode'];
        if ($.trim($('#ptvTcyRate').val()) === '' || num($('#ptvTcyRate').val()) === 0) return ['Tcy Exchange Rate Field is Required', 'ptvTcyRate'];
        if ($.trim($('#ptvAmount').val()) === '' || num($('#ptvAmount').val()) === 0) return ['Amount Field is Required', 'ptvAmount'];
        if (bank && ins === 1 && int($('#ptvChequeType').val()) === 0) return ['Cheque Type Field is Required', 'ptvChequeType'];
        if (F.branchFeature && int($('#ptvBranch').val()) === 0) return ['BranchName Field is Required', 'ptvBranch'];
        return null;
    }
    function fail(e) { alert(e[0]); comboFocus(e[1]); }

    function rowFromEntry() {
        var accOpt = $('#ptvAccount option:selected'), refId = int($('#ptvReference').val()), refOpt = $('#ptvReference option:selected');
        var sub = int($('#ptvSubsidiary').val()), subOpt = $('#ptvSubsidiary option:selected');
        var r = {
            paymentTypeId: int($('#ptvPaymentType').val()), paymentType: selText('ptvPaymentType'),
            accountId: int($('#ptvAccount').val()), accountCode: accOpt.attr('data-code') || '', accountTitle: accOpt.attr('data-title') || accOpt.text(),
            subsidiaryAccountId: 0, subsidiaryAccount: '', subsidiaryAccountTypeId: 0,
            jobLotId: int($('#ptvJobLot').val()), remarks: $('#ptvLineRemarks').val(),
            tcyCodeId: int($('#ptvTcyCode').val()), tcyCode: selText('ptvTcyCode'), tcyExchangeRate: num($('#ptvTcyRate').val()),
            fcyAmount: num($('#ptvTcyAmount').val()), amount: num($('#ptvAmount').val()), taxAmount: 0,
            referenceAccountId: refId, referenceAccount: refId > 0 ? (refOpt.attr('data-title') || refOpt.text()) : '',
            financialInstrumentId: int($('#ptvInstrument').val()), financialInstrument: S.doc === 2 ? selText('ptvInstrument') : '',
            chequeDate: $('#ptvChequeDate').val() || today(), chequeId: 0, chequeNo: '', payeeTitle: '', chequeTypeId: 0,
            branchId: int($('#ptvBranch').val()), branchName: selText('ptvBranch'),
            costCenterId: int($('#ptvCostCenter').val()), costCenterName: selText('ptvCostCenter')
        };
        if (S.F.subsidiaryFeature) {
            if (sub > 0) { r.subsidiaryAccountId = sub; r.subsidiaryAccount = subOpt.text(); r.subsidiaryAccountTypeId = int(subOpt.attr('data-type')); }
            else { r.subsidiaryAccountId = r.accountId; r.subsidiaryAccount = r.accountTitle; r.subsidiaryAccountTypeId = 4; }
        }
        if (S.doc === 2) {
            r.chequeNo = $('#ptvChequeNo').val(); r.chequeId = chequeIdFor(r.chequeNo);
            r.payeeTitle = $('#ptvPayeeTitle').val(); r.chequeTypeId = int($('#ptvChequeType').val());
        }
        return r;
    }

    function afterRowsChanged() {
        if (S.rows.length && int($('#ptvAgainstAc').val()) === 0) setVal('ptvAgainstAc', S.rows[0].accountId);
    }

    /** Add_Click_1. */
    function add() {
        var e = detailValid();
        if (e) { fail(e); return; }
        if (S.doc === 2 && S.F.chequePostingSerialWise && int($('#ptvInstrument').val()) === 1) {
            var se = chequeSerial($('#ptvChequeNo').val());
            if (se) { alert(se); return; }
        }
        var r = rowFromEntry();
        S.rows.push(r);
        $('#ptvAccBalance').hide();
        if (r.chequeId > 0) $('#ptvVoucherType, #ptvCredit').prop('disabled', true);
        renderGrid();
        resetDetail();
        ptotal();
        afterRowsChanged();
    }

    /** btnUpdateDetail_Click. */
    function updateDetail() {
        if (S.editIndex < 0) return;
        var e = detailValid();
        if (e) { fail(e); return; }
        if (S.doc === 2 && S.F.chequePostingSerialWise) {
            var typed = int($('#ptvChequeNo').val());
            if (S.editCheque && S.editCheque.no > 0 && typed !== S.editCheque.no) {
                var max = Math.max.apply(null, S.rows.map(function (r) { return int(r.chequeNo); }));
                var next = S.rows[S.editIndex + 1];
                if (next) {
                    if (S.editCheque.no < int(next.chequeNo)) { alert('You Cant Update Cheque No Serial To Other Serial No'); return; }
                } else if (typed > max) { alert('You Cant Update Cheque No Serial To Other Serial No'); return; }
            }
            var se = chequeSerial($('#ptvChequeNo').val());
            if (se) { alert(se); return; }
        }
        S.rows[S.editIndex] = rowFromEntry();
        $('#ptvAdd').show(); $('#ptvUpdateDetail, #ptvCancelDetail').hide();
        renderGrid();
        resetDetail();
        ptotal();
        rateChangedHeader();                // txtExchangeRate_TextChanged(null, null)
        afterRowsChanged();
    }

    function cancelDetail() { resetDetail(); }

    /** ResetDetail(): Add visible again, the subsidiary list emptied, amounts / cheque no / payee cleared, Total(), focus the debit account. */
    function resetDetail() {
        S.editIndex = -1; S.updIdx = -1; S.editCheque = null; S.amtSrc = '';
        $('#ptvAdd').show(); $('#ptvUpdateDetail, #ptvCancelDetail').hide();
        $('#ptvSubsidiary').html('<option value="0"></option>');
        $('#ptvTcyAmount').val(''); $('#ptvAmount').val('');
        $('#ptvChequeNo').val(''); $('#ptvPayeeTitle').val('');
        ptotal();
        comboFocus('ptvAccount');
    }

    /** grd_DoubleClick (and Ctrl+Enter on the grid). */
    function editRow(i) {
        var r = S.rows[i]; if (!r) return;
        S.editIndex = i; S.updIdx = i; S.editCheque = null;
        setVal('ptvPaymentType', r.paymentTypeId);
        put('ptvAccount', r.accountId);
        bindSubsidiary(0);
        if (S.subLen === 0 && S.F.subsidiaryFeature) {
            $('#ptvSubsidiary').html('<option value="0"></option><option value="' + esc(r.subsidiaryAccountId) + '" data-type="4" data-acc="' + esc(r.accountId) + '">' + esc(r.subsidiaryAccount) + '</option>');
        }
        put('ptvSubsidiary', r.subsidiaryAccountId);
        setVal('ptvJobLot', r.jobLotId);
        if (int(r.financialInstrumentId) > 0) setVal('ptvInstrument', r.financialInstrumentId);
        if (int(r.referenceAccountId) > 0) setVal('ptvReference', r.referenceAccountId);
        $('#ptvChequeDate').val(day(r.chequeDate) || today());
        if (int(r.chequeId) > 0 && String(r.chequeNo || '') !== '') S.editCheque = { id: int(r.chequeId), no: int(r.chequeNo) };
        $('#ptvChequeNo').val(r.chequeNo || '');
        $('#ptvPayeeTitle').val(r.payeeTitle || '');
        setVal('ptvChequeType', r.chequeTypeId);
        setVal('ptvBranch', r.branchId);
        $('#ptvLineRemarks').val(r.remarks);
        setVal('ptvTcyCode', r.tcyCodeId);
        $('#ptvTcyRate').val(fmt3(r.tcyExchangeRate));
        $('#ptvTcyAmount').val(fmt3(r.fcyAmount));
        $('#ptvAmount').val(String(r.amount));
        if (int(r.costCenterId) > 0) setVal('ptvCostCenter', r.costCenterId);
        comboFocus('ptvPaymentType');
        $('#ptvAdd').hide(); $('#ptvUpdateDetail, #ptvCancelDetail').show();
    }

    /** grd_ColumnButtonClick "Delete" (and Ctrl+Space). */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (S.F.chequePostingSerialWise) {
            var max = Math.max.apply(null, S.rows.map(function (x) { return int(x.chequeNo); }));
            if (int(r.chequeNo) !== max) { alert("You can't Delete This Row Because This Entry Is Against Serial Wise Cheque No"); return; }
        }
        S.rows.splice(i, 1);
        if (S.editIndex === i) resetDetail();
        if (S.doc === 2) {
            var used = S.rows.reduce(function (s, x) { return s + (int(x.chequeId) > 0 ? int(x.chequeId) : 0); }, 0);
            if (!S.rows.length || used === 0) $('#ptvCredit').prop('disabled', false);   // only the credit account is re-enabled
        }
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
        var rate = num($('#ptvTcyRate').val());
        if (S.amtSrc === 'fcy') {
            var f = num($('#ptvTcyAmount').val());
            if (f > 0 && rate > 0) $('#ptvAmount').val(String(round(f * rate, dec()))); else $('#ptvAmount').val('0');
        } else {
            var a = num($('#ptvAmount').val());
            if ($.trim($('#ptvAmount').val()) === '') return;
            if (a > 0 && rate > 0) $('#ptvTcyAmount').val(String(round(a / rate, fcyDec()))); else $('#ptvTcyAmount').val('0');
        }
    }

    /** txtExchangeRate_TextChanged: the Tcy detail rate follows, then CalculateTotalInformation(). */
    function rateChangedHeader() {
        $('#ptvTcyRate').val($('#ptvExchangeRate').val());
        linkAmounts();
        renderGrid();
    }

    /** cmbCurrency_Leave. */
    function currencyLeave() {
        var cur = int($('#ptvCurrency').val());
        if (cur === 0) return;
        put('ptvTcyCode', cur);
        if (cur !== int(S.F.baseCurrencyId)) {
            if (int($('#ptvVoucherType').val()) === 0) { alert('Please Select Voucher Type First'); comboFocus('ptvVoucherType'); return; }
            $.getJSON(api('/last-rate'), { doc: S.doc, currencyId: cur }).then(function (r) {
                var lr = num(r && r.lastRate);
                var t = lr ? fmtRate(lr) : '0';
                $('#ptvExchangeRate').val(t); $('#ptvTcyRate').val(t);
                rateChangedHeader();
            }, function (x) { alert(errMsg(x)); });
        } else {
            var b = fmtRate(S.F.baseCurrencyRate);
            $('#ptvExchangeRate').val(b); $('#ptvTcyRate').val(b);
            rateChangedHeader();
        }
    }

    // ------------------------------------------------------------------------------ voucher type / reset / load

    /** AccountTitleFill() + the cheque / instrument visibility that follows the Voucher Type. */
    function applyDoc() {
        var keep = int($('#ptvCredit').val());
        S.doc = int($('#ptvVoucherType').val()) || S.doc;
        fillCredit();
        put('ptvCredit', keep);
        if (S.doc === 1 && rights().save && !S.updateMode && int($('#ptvCredit').val()) === 0) firstRow('ptvCredit');   // Rows[1].Activate()
        layoutDetail();
        $('#ptvLblPrintOnSave').toggle(!!S.F.chequePrintingEnable && S.doc === 2);
        renderGrid();
        chequeFill();
        balance(int($('#ptvCredit').val()), $('#ptvCrBalance'));
        if (!S.updateMode) nextCode(false);
    }

    function showButtons(m) {
        var r = rights();
        $('#btnSave').toggle(m === 'save').prop('disabled', !r.save);
        $('#btnUpdate').toggle(m === 'update').prop('disabled', !r.update);
    }

    /** VoucherNofill(): only while Save is visible and enabled. */
    function nextCode(first) {
        if (!rights().save) { $('#ptvVoucherNo').val(''); return; }
        if (first && S.L && S.L.nextCode != null && S.doc === int(S.L.startDoc)) { $('#ptvVoucherNo').val(S.L.nextCode); return; }
        var seq = ++S.codeSeq;
        $.getJSON(api('/next-code'), { doc: S.doc }).then(function (r) {
            if (seq === S.codeSeq && !S.updateMode) $('#ptvVoucherNo').val(r.voucherCode);
        });
    }

    /** Reset(): the date, project, location, credit account, currency and rate are NOT touched (only Load / btnNew do). */
    function reset(first) {
        var wasWht = $('#ptvWht').is(':checked');
        S.rows = []; S.recId = 0; S.updateMode = false; S.editIndex = -1; S.updIdx = -1; S.editCheque = null; S.subLen = 0; S.amtSrc = ''; S.discSrc = '';
        $('#ptvVoucherType, #ptvCredit').prop('disabled', false);
        $('#ptvAccBalance').hide().text('0');
        $('#ptvLineRemarks').val(''); $('#ptvAmount').val(''); $('#ptvPayeeTitle').val(''); put('ptvChequeType', 0);
        $('#ptvRemarks').val('');
        put('ptvTaxType', 0); put('ptvAgainstAc', 0); put('ptvWithHoldingAc', 0);
        $('#ptvWht').prop('checked', false);
        $('#ptvValue, #ptvTaxAmount, #ptvTotal').val(''); T.value = 0; T.tax = 0; T.total = 0;
        firstRow('ptvPaymentType');
        put('ptvAccount', 0); $('#ptvChequeNo').val('');
        $('#ptvSrbAmount, #ptvDiscPct, #ptvDiscAmt').val('');
        if (first) {
            $('#ptvDate').val(today());
            firstRow('ptvProject');
            if (S.doc === 2) { firstRow('ptvInstrument'); firstRow('ptvChequeType'); }
            $('#ptvChequeDate').val(today());
            if (S.F.defaultBranchId) setVal('ptvBranch', S.F.defaultBranchId);
            if (S.doc === 1 && rights().save) firstRow('ptvCredit');
            defaults();
        }
        nextCode(first);
        showButtons('save');
        renderGrid();
        resetDetail();
        if (wasWht) whtChanged();
        else $('#ptvTaxPercent').val($('#ptvTaxPercent').val() || '0');
        balance(int($('#ptvCredit').val()), $('#ptvCrBalance'));     // combcreditac_Leave
        chequeFill();
        $('#ptvFcyAmount').val('');
        if (!first) currencyLeave();                                  // cmbCurrency_Leave(null, null)
    }

    /** btnNew_Click: Reset(), the credit account cleared, its balance hidden. */
    function newClick() {
        reset(false);
        put('ptvCredit', 0);
        $('#ptvCrBalance').hide();
    }

    /** ReadById(ID). */
    function loadForEdit(id) {
        return $.getJSON(api('/' + id)).then(function (v) {
            if (!v) return;
            var dt = int(v.documentTypeId);
            if (dt !== S.doc) {
                if (S.mode !== 0) { alert('This voucher belongs to the other voucher type.'); return; }
                put('ptvVoucherType', dt); S.doc = dt; applyDoc();
            }
            switchTab('form');
            S.recId = int(v.id); S.updateMode = true; S.codeSeq++; S.editIndex = -1; S.updIdx = -1; S.editCheque = null;
            $('#ptvVoucherNo').val(v.voucherCode);
            $('#ptvDate').val(day(v.voucherDate));
            setVal('ptvCredit', v.refAccountId);
            $('#ptvRemarks').val(v.remarks || '');
            setVal('ptvProject', v.projectId);
            put('ptvLocation', v.locationTypeId);
            put('ptvCurrency', v.multiCurrencyId);
            $('#ptvExchangeRate').val(fmtRate(v.exchangeCurrencyRate));
            $('#ptvTcyRate').val(fmtRate(v.exchangeCurrencyRate));
            if (int(v.multiCurrencyId) === 0 || num(v.exchangeCurrencyRate) === 0) defaults();
            $('#ptvCustom').prop('checked', !!v.customAccounts);
            $('#ptvRadIncluded').prop('checked', !!v.inclusiveTax); $('#ptvRadExcluded').prop('checked', !v.inclusiveTax);
            S.rows = (v.rows || []).map(function (r) {
                return {
                    paymentTypeId: int(r.paymentTypeId), paymentType: r.paymentType || '', accountId: int(r.accountId),
                    accountCode: r.accountCode || '', accountTitle: r.accountTitle || '',
                    subsidiaryAccountId: int(r.subsidiaryAccountId), subsidiaryAccount: r.subsidiaryAccountTitle || '', subsidiaryAccountTypeId: int(r.subsidiaryAccountTypeId),
                    jobLotId: int(r.jobLotId), remarks: r.remarks || '', tcyCodeId: int(r.tcyCodeId), tcyCode: r.tcyCode || '',
                    tcyExchangeRate: num(r.tcyExchangeRate), fcyAmount: num(r.fcyAmount), amount: num(r.amount), taxAmount: num(r.taxAmount),
                    referenceAccountId: int(r.referenceAccountId), referenceAccount: r.referenceAccount || '',
                    financialInstrumentId: int(r.financialInstrumentId), financialInstrument: r.financialInstrument || '',
                    chequeDate: day(r.chequeDate), chequeId: int(r.chequeId), chequeNo: r.chequeNo || '', payeeTitle: r.payeeTitle || '',
                    chequeTypeId: int(r.chequeTypeId), branchId: int(r.branchId), branchName: r.branchName || '',
                    costCenterId: int(r.costCenterId), costCenterName: r.costCenterName || ''
                };
            });
            var wh = v.wht || {};
            if (v.includeWHT) {
                $('#ptvWht').prop('checked', true);
                put('ptvTaxType', wh.taxTypeId);
                put('ptvAgainstAc', wh.againstAcId); put('ptvWithHoldingAc', wh.withHoldingAcId);
                if (wh.taxPercent != null) $('#ptvTaxPercent').val(String(wh.taxPercent));
            } else $('#ptvWht').prop('checked', false);
            var sb = v.srb || {}, ds = v.discount || {};
            put('ptvSrbAcc', sb.accountId || 0); $('#ptvSrbAmount').val(sb.amount != null ? String(num(sb.amount)) : '');
            put('ptvDiscAcc', ds.accountId || 0);
            $('#ptvDiscPct').val(ds.percent != null ? String(num(ds.percent)) : ''); $('#ptvDiscAmt').val(ds.amount != null ? String(num(ds.amount)) : '');
            if (v.lockHeader) $('#ptvVoucherType, #ptvCredit').prop('disabled', true);
            renderGrid();
            chequeFill();
            balance(int($('#ptvCredit').val()), $('#ptvCrBalance'));
            showButtons('update');
            return whtChanged().then(function () { ptotal(); $('#ptvDate').focus(); });
        }, function (x) { alert(errMsg(x, 'Voucher not found')); });
    }

    // ------------------------------------------------------------------------------ save

    /** FormValidation(). */
    function formValid() {
        if (int($('#ptvProject').val()) === 0) return ['Cost Center Field is Required', 'ptvProject'];
        if (!$('#ptvVoucherType').val() || int($('#ptvVoucherType').val()) === 0) return ['Voucher Type Field is Required', 'ptvVoucherType'];
        if (int($('#ptvLocation').val()) === 0) return ['Location Type Field is Required', 'ptvLocation'];
        if (int($('#ptvCredit').val()) === 0) return ['Credit Account Field is Required', 'ptvCredit'];
        if (int($('#ptvCurrency').val()) === 0) return ['Fcy Code Field is Required', 'ptvCurrency'];
        if (num($('#ptvExchangeRate').val()) === 0) return ['Exchange Rate Field is Required', 'ptvExchangeRate'];
        if (num($('#ptvFcyAmount').val()) === 0) return ['Fcy Amount Field is Required', 'ptvFcyAmount'];
        return null;
    }

    /** Insert(): the field checks that follow the "Are you sure" prompt. */
    function taxFieldsValid() {
        if ($('#ptvWht').is(':checked')) {
            if (int($('#ptvTaxType').val()) === 0) return ['TaxType Field is Required', 'ptvTaxType'];
            if (num($('#ptvTaxPercent').val()) === 0) return ['Tax Percent Field is Required', 'ptvTaxPercent'];
            if (int($('#ptvAgainstAc').val()) === 0) return ['Withholding Debit Account Field is Required', 'ptvAgainstAc'];
            if (int($('#ptvWithHoldingAc').val()) === 0) return ['Withholding Credit Account Field is Required', 'ptvWithHoldingAc'];
        }
        if (int($('#ptvSrbAcc').val()) === 0 && num($('#ptvSrbAmount').val()) > 0) return ['SRB Account Field is Required', 'ptvSrbAcc'];
        if (int($('#ptvDiscAcc').val()) === 0 && num($('#ptvDiscAmt').val()) > 0) return ['Discount Account Field is Required', 'ptvDiscAcc'];
        if (num($('#ptvDiscPct').val()) === 0 && num($('#ptvDiscAmt').val()) > 0) return ['Disc% Field is Required', 'ptvDiscPct'];
        for (var i = 0; i < S.rows.length; i++) if (S.rows[i].amount > 0 && int(S.rows[i].accountId) === 0) return ['Please Select Account Title First', null];
        return null;
    }

    function payload(ack) {
        return {
            Id: S.recId, VoucherCode: int($('#ptvVoucherNo').val()), VoucherDate: $('#ptvDate').val(), DocumentTypeId: S.doc,
            ProjectId: int($('#ptvProject').val()), LocationTypeId: int($('#ptvLocation').val()), RefAccountId: int($('#ptvCredit').val()),
            Remarks: $('#ptvRemarks').val(), MultiCurrencyId: int($('#ptvCurrency').val()),
            ExchangeCurrencyRate: num($('#ptvExchangeRate').val()), FcAmount: num($('#ptvFcyAmount').val()),
            CustomAccounts: $('#ptvCustom').is(':checked'), InclusiveTax: $('#ptvRadIncluded').is(':checked'),
            IncludeWHT: $('#ptvWht').is(':checked'), TaxTypeId: int($('#ptvTaxType').val()), TaxTypeName: selText('ptvTaxType'),
            TaxPercent: $('#ptvTaxPercent').val(), AgainstAcId: int($('#ptvAgainstAc').val()), WithHoldingAcId: int($('#ptvWithHoldingAc').val()),
            SrbAccountId: int($('#ptvSrbAcc').val()), SrbAccountName: selText('ptvSrbAcc'), SrbAmount: num($('#ptvSrbAmount').val()),
            DiscountAccountId: int($('#ptvDiscAcc').val()), DiscountPercent: num($('#ptvDiscPct').val()), DiscountAmount: num($('#ptvDiscAmt').val()),
            DiscountAmountText: $('#ptvDiscAmt').val(),
            acknowledged: ack,
            rows: S.rows.map(function (r) {
                return {
                    PaymentTypeId: r.paymentTypeId, AccountId: r.accountId, SubsidiaryAccountId: r.subsidiaryAccountId || 0,
                    SubsidiaryAccountTypeId: r.subsidiaryAccountTypeId || 0, JobLotId: r.jobLotId, Remarks: r.remarks,
                    TcyCodeId: r.tcyCodeId, TcyExchangeRate: r.tcyExchangeRate, FcyAmount: r.fcyAmount, Amount: r.amount,
                    ReferenceAccountId: r.referenceAccountId || 0, FinancialInstrumentId: r.financialInstrumentId || 0,
                    ChequeDate: r.chequeDate || null, ChequeId: r.chequeId || 0, ChequeNo: r.chequeNo || null, PayeeTitle: r.payeeTitle || null,
                    ChequeTypeId: r.chequeTypeId || 0, BranchId: r.branchId || 0, CostCenterId: r.costCenterId || 0
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
        var printCheque = S.F.chequePrintingEnable && S.doc === 2 && $('#ptvCbPrintOnSave').is(':checked');
        var p1 = $('#ptvChkPrint1').is(':checked'), p2 = $('#ptvChkPrint2').is(':checked'), p3 = $('#ptvChkPrint3').is(':checked');
        var printKind = p1 ? 1 : (p2 ? 2 : (p3 ? 3 : 0));
        var win = printKind && w.CrystalPrint ? w.CrystalPrint.reserve() : null;
        S.busy = true; busyOn($b);
        (function post() {
            $.ajax({ url: api('/save'), type: 'POST', contentType: 'application/json', headers: csrf(), data: JSON.stringify(payload(ack)) })
                .done(function (res) {
                    S.busy = false; busyOff($b);
                    alert(res.message);
                    var doc = S.doc;
                    reset(false);
                    if (printKind) printDoc(printKind, res.id, doc, null, win);
                    if (printCheque) w.open('/accounts/banking/cheque-printing', '_blank');
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

    /** kind 1 = hrm-102 slip, 2 = acc-102 (with DocumentTypeId), 3 = PaymentAndReceiptVoucherSlip (102-III per type). */
    function printDoc(kind, id, docType, btn, win) {
        if (!(int(id) > 0)) { if (win && w.CrystalPrint) w.CrystalPrint.release(win); alert('VoucherId Not Found'); return; }
        if (kind === 3) {
            var url = '/reports/print/by-template/' + encodeURIComponent(RPT3[docType || S.doc]) + '/pdf?id=' + encodeURIComponent(id);
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
        printDoc(kind, S.recId, S.doc, btn, null);
    }

    /* SpecialRightsImplement: RightId 2 -> ChkPrint1, 8 -> ChkPrint2, 9 -> ChkPrint3 (ScreenId of PaymentVoucherNew = 21). */
    function specialRights() {
        if (!w.SpecialRights) return;
        w.SpecialRights.mine(SCREEN_ID).then(function (rows) {
            $.each(rows || [], function (i, r) {
                var rid = int(r.RightId != null ? r.RightId : r.rightId);
                var a = r.IsActive != null ? r.IsActive : r.isActive;
                a = a === true || a === 1 || a === '1' || String(a).toLowerCase() === 'true';
                var id = ({ 2: '#ptvChkPrint1', 8: '#ptvChkPrint2', 9: '#ptvChkPrint3' })[rid];
                if (id) $(id).prop('checked', a);
            });
        }, function (x) { alert(errMsg(x, 'Could not load special rights.')); });
    }

    var KEYS = [['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+U', 'For Update'], ['Ctrl+S', 'For Save'], ['Ctrl+E', 'For Close'],
        ['Alt+1', 'For Print I'], ['Alt+2', 'For Print II'], ['Alt+3', 'For Print III'], ['Ctrl+F5', 'For Focus on Voucher Type'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        if (!$('#ptvKeys').length) {
            $('body').append('<div id="ptvKeys" style="display:none;position:fixed;inset:0;z-index:3000;background:rgba(0,0,0,.3);">' +
                '<div style="position:absolute;left:50%;top:70px;transform:translateX(-50%);width:460px;max-width:95vw;background:#fff;border:1px solid #00796B;box-shadow:0 4px 16px rgba(0,0,0,.3);">' +
                '<div style="background:#00796B;color:#fff;padding:5px 8px;font-weight:bold;display:flex;justify-content:space-between;"><span>ShortCut Keys</span>' +
                '<a href="javascript:void(0)" id="ptvKeysClose" style="color:#fff;">&#x2715;</a></div><div style="max-height:70vh;overflow:auto;">' +
                '<table class="win-grid-table"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody></tbody></table></div></div></div>');
            $('#ptvKeysClose').on('click', function () { $('#ptvKeys').hide(); });
            $('#ptvKeys').on('click', function (e) { if (e.target === this) $(this).hide(); });
        }
        $('#ptvKeys tbody').html(KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''));
        $('#ptvKeys').show();
    }
    function attachments() { alert('Attachments are not available on the web form yet.'); }

    // ------------------------------------------------------------------------------ History tabs (Cpv / Bpv)

    /* MultiCurrencyFeatureVisibilty is hard-wired true on the desktop: the Fcy columns are always visible. */
    function hid(d, n) { return 'ptvH' + d + n; }
    function H(d, n) { return $('#' + hid(d, n)); }

    function hcols(d) {
        var c = [{ b: 'edit', t: 'Edit', w: 35 }, { b: 'p1', t: 'Print', w: 40 }, { b: 'p2', t: 'Print-II', w: 60 }, { b: 'p3', t: 'Print-III', w: 60 },
                 { k: 'documentType', t: 'V.Type' }, { k: 'voucherDate', t: 'V.Date' }, { k: 'voucherCode', t: 'V.No', link: 1 }];
        if (d === 2) c.push({ k: 'chequeNo', t: 'ChequeNo' });
        c.push({ k: 'accountTitle', t: 'AccountTitle' }, { k: 'voucherAmount', t: 'VoucherAmount', n: 1, sum: 1 },
               { k: 'fcyCode', t: 'FcyCode' }, { k: 'exchangeRate', t: 'ExchangeRate', n: 2 }, { k: 'fcyAmount', t: 'FcyAmount', n: 3, sum: 3 },
               { k: 'remarks', t: 'Remarks', w: 350 }, { k: 'entryUser', t: 'EntryUser' }, { k: 'entryDate', t: 'EntryDate' },
               { k: 'modifyUser', t: 'ModifyUser' }, { k: 'modifyDate', t: 'ModifyDate' }, { k: 'approvedUser', t: 'ApprovedUser' },
               { k: 'approvedDate', t: 'ApprovedDate' }, { k: 'attachment', t: 'Attachment' }, { b: 'att', t: d === 2 ? 'Add Attachment' : 'Add Attachments', w: d === 1 ? 100 : 110 });
        return c;
    }

    /* Layout of the Cpv / Bpv tab pages (the designer numbers of PaymentVoucherNew's history tabs). */
    var HL = {
        1: { title: 'Cash Payment Voucher History', th: 29, fh: 79, gw: 1081, ly: 19, fy: 19, cy: 38, ky: 36, from: [5, 134], to: [144, 130], fno: 278, tno: 382,
             acc: [488, 216], st: [708, 118], show: [831, 36], rad: [[903, 30], [903, 50], [984, 30], [984, 50]], info: [1090, 5, 229, 69], iv: [158, 67], gh: 198, dh: 25 },
        2: { title: 'Bank Payment Voucher History', th: 28, fh: 78, gw: 1075, ly: 22, fy: 23, cy: 41, ky: 39, from: [5, 138], to: [146, 132], fno: 281, tno: 384,
             acc: [488, 216], st: [707, 118], show: [829, 39], rad: [[898, 29], [898, 49], [979, 29], [979, 49]], info: [1087, 1, 238, 73], iv: [158, 67], gh: 220, dh: 26 }
    };

    function renderHistory(d) {
        var G = HL[d], h = '';
        var I = function (n) { return hid(d, n); };
        h += '<div class="prv-ts"><div class="prv-ts-in" style="border-bottom-color:#c5cbd3;">' +
             '<button type="button" class="prv-ts-btn" id="' + I('Reset') + '"><i class="fa fa-undo" style="color:#2e7d32"></i><span><u>R</u>eset</span></button>' +
             '<button type="button" class="prv-ts-btn" id="' + I('Refresh') + '"><i class="fa fa-refresh" style="color:#2e7d32"></i><span>Refresh</span></button></div></div>';
        h += '<div class="prv-teal" style="height:' + G.th + 'px;margin-top:-3px;"><span style="left:4px;top:4px;">' + G.title + '</span></div>';
        var rn = 'ptvHDate' + d, radios =
            '<label class="prv-rb" style="' + px(G.rad[0][0], G.rad[0][1]) + '"><input type="radio" name="' + rn + '" value="docdate" checked/>Doc Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[1][0], G.rad[1][1]) + '"><input type="radio" name="' + rn + '" value="entrydate"/>Entry Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[2][0], G.rad[2][1]) + '"><input type="radio" name="' + rn + '" value="modifydate"/>Modify Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[3][0], G.rad[3][1]) + '"><input type="radio" name="' + rn + '" value="approveddate"/>Approved Date</label>';
        var filt =
            lab(5, G.fy, 'From Date') +
            '<div class="prv-dtp" style="' + px(G.from[0], G.cy, G.from[1], 23) + '"><input type="checkbox" id="' + I('FromChk') + '" checked title="Use this date"/>' +
            '<input type="date" id="' + I('From') + '" class="prv-tb"/></div>' +
            lab(G.to[0], G.ly, 'To Date') +
            '<div class="prv-dtp" style="' + px(G.to[0], G.cy, G.to[1], 23) + '"><input type="checkbox" id="' + I('ToChk') + '" checked title="Use this date"/>' +
            '<input type="date" id="' + I('To') + '" class="prv-tb"/></div>' +
            lab(G.fno, G.ly, 'From Doc No') + tb(I('FromNo'), G.fno, G.cy, 100, 23, ' maxlength="9"', d === 1 ? '' : 'prv-segb') +
            lab(G.tno, G.ly, 'To Doc No') + tb(I('ToNo'), G.tno, G.cy, 100, 23, ' maxlength="9"', d === 1 ? '' : 'prv-segb') +
            lab(G.acc[0], G.ly, 'Account Title') +
            '<select id="' + I('Account') + '" class="prv-cmb" data-dtcombo="single" style="position:absolute;' + px(G.acc[0], G.ky, G.acc[1], 26) + '"><option value="0"></option></select>' +
            lab(G.st[0], G.ly, 'Approved Status') +
            '<select id="' + I('Status') + '" class="prv-cmb" data-dtcombo="single" style="position:absolute;' + px(G.st[0], G.ky, G.st[1], 26) + '">' +
            '<option value="notapproved" selected>Not Apporved</option><option value="approved">Approved</option><option value="all">All</option></select>' +
            '<button type="button" class="prv-fbtn prv-show" id="' + I('Show') + '" style="' + px(G.show[0], G.show[1], 53, 26) + '">Show</button>';
        var info = '<div id="' + I('Info') + '" class="prv-gb prv-info" style="display:none;' + px(G.info[0], G.info[1], G.info[2], G.info[3]) + '">' +
            lab(6, 11, 'Total Vouchers', 'prv-info') + '<span id="' + I('Total') + '" class="prv-iv" style="' + px(G.iv[0], 11, G.iv[1], 17) + '">0</span>' +
            lab(6, 28, 'Approved Vouchers', 'prv-info') + '<span id="' + I('Approved') + '" class="prv-iv" style="' + px(G.iv[0], 28, G.iv[1], 17) + '">0</span>' +
            lab(6, 47, 'UnApproved Vouchers', 'prv-info') + '<span id="' + I('UnApproved') + '" class="prv-iv" style="' + px(G.iv[0], 47, G.iv[1], 17) + '">0</span></div>';
        h += '<div class="prv-abs" style="height:' + G.fh + 'px;">' + gbox('Filters', 5, 3, G.gw, 71, filt) + radios + info + '</div>';
        h += '<div class="prv-hgrid" style="height:' + G.gh + 'px;"><table class="prv-jg prv-auto" id="' + I('Grid') + '" tabindex="0"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        h += '<div class="prv-teal" style="height:' + G.dh + 'px;"><span style="left:3px;top:3px;">Detail of above selected row</span></div>';
        h += '<div class="prv-hdet"><table class="prv-jg prv-auto" id="' + I('Detail') + '"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        $('#ptvHist' + d).html(h);
        H(d, 'From').add(H(d, 'To')).val(today());
        H(d, 'Grid').find('thead').html('<tr>' + hcols(d).map(function (c) {
            return '<th' + (c.n ? ' class="num"' : '') + (c.w ? ' style="min-width:' + c.w + 'px;"' : '') + '>' + c.t + '</th>';
        }).join('') + '</tr>');
        wireHistory(d);
        historyAccounts(d);
    }

    function historyAccounts(d, $b) {
        if ($b) busyOn($b);
        return $.getJSON(api('/history/accounts'), { doc: d }).then(function (list) {
            var cur = H(d, 'Account').val();
            H(d, 'Account').html('<option value="0"></option>' + (list || []).map(function (a) {
                return '<option value="' + esc(a.Id) + '">' + esc(a.name) + '</option>';
            }).join(''));
            if (cur) H(d, 'Account').val(cur);
        }, function (x) { alert(errMsg(x, 'Could not load the accounts.')); }).always(function () { if ($b) busyOff($b); });
    }

    function historyReset(d) {
        H(d, 'From').add(H(d, 'To')).val(today());
        H(d, 'FromNo').add(H(d, 'ToNo')).val('');
        H(d, 'Account').val('0').trigger('change');
        H(d, 'Status').val('notapproved');
        H(d, 'Info').hide();
        S.hist[d].rows = []; S.hist[d].sel = -1;
        H(d, 'Grid').find('tbody, tfoot').empty();
        H(d, 'Detail').find('thead, tbody, tfoot').empty();
    }

    function historyShow(d) {
        var hs = S.hist[d]; if (hs.busy) return;
        var $b = H(d, 'Show');
        var params = {
            doc: d, dateType: $('input[name="ptvHDate' + d + '"]:checked').val(),
            fromDate: H(d, 'FromChk').is(':checked') ? H(d, 'From').val() : '', toDate: H(d, 'ToChk').is(':checked') ? H(d, 'To').val() : '',
            fromDocNo: int(H(d, 'FromNo').val()) || '', toDocNo: int(H(d, 'ToNo').val()) || '',
            accountId: int(H(d, 'Account').val()), approvedStatus: H(d, 'Status').val()
        };
        hs.busy = true; busyOn($b);
        $.getJSON(api('/history'), params).then(function (res) {
            var rows = (res && res.rows) || [];
            hs.rows = rows; hs.sel = -1;
            H(d, 'Detail').find('thead, tbody, tfoot').empty();
            if (!rows.length) { H(d, 'Info').hide(); H(d, 'Grid').find('tbody, tfoot').empty(); return; }
            H(d, 'Info').show();
            H(d, 'Total').text(res.totalVouchers || 0); H(d, 'Approved').text(res.totalApprovedVoucher || 0); H(d, 'UnApproved').text(res.totalUnApprovedVoucher || 0);
            var cs = hcols(d), sumA = 0, sumF = 0, html = '';
            rows.forEach(function (v, i) {
                sumA += num(v.voucherAmount); sumF += num(v.fcyAmount);
                html += '<tr data-i="' + i + '" data-id="' + esc(v.id) + '">' + cs.map(function (c) {
                    if (c.b) {
                        if (c.b === 'att') return '<td><button type="button" class="prv-cbtn" disabled title="Attachments are not on the web yet">' + c.t + '</button></td>';
                        return '<td><button type="button" class="prv-cbtn prv-hb" data-b="' + c.b + '" data-i="' + i + '">' + c.t + '</button></td>';
                    }
                    var x = v[c.k];
                    if (c.link) return '<td><a class="prv-vno" data-i="' + i + '">' + esc(x) + '</a></td>';
                    if (c.n === 1) x = fmt(x); else if (c.n === 2) x = fmtRate(x); else if (c.n === 3) x = fmt3(x);
                    if (c.k === 'attachment') x = int(x) > 0 ? x : '';
                    return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(x) + '</td>';
                }).join('') + '</tr>';
            });
            H(d, 'Grid').find('tbody').html(html);
            H(d, 'Grid').find('tfoot').html('<tr>' + cs.map(function (c) {
                return '<td class="num" style="font-weight:bold;">' + (c.sum === 1 ? fmt(sumA) : c.sum === 3 ? fmt3(sumF) : '') + '</td>';
            }).join('') + '</tr>');
            selectHistory(d, 0);
        }, function (x) { alert(errMsg(x, 'Could not load the history.')); }).always(function () { hs.busy = false; busyOff($b); });
    }

    function selectHistory(d, i) {
        var hs = S.hist[d], v = hs.rows[i]; if (!v) return;
        hs.sel = i;
        H(d, 'Grid').find('tbody tr').removeClass('prv-sel').filter('[data-i="' + i + '"]').addClass('prv-sel');
        $.getJSON(api('/' + v.id + '/lines')).then(function (r) { if (hs.sel === i) renderLines(d, r); });
    }

    /** CpvDetail / BpvDetail grids (VoucherDetailByHeaderId / VoucherDetailBPVByHeaderId). */
    function renderLines(d, r) {
        var list = (r && r.rows) || [], F = S.F, cs, data;
        if (d === 1) {
            var inc = $('#ptvRadIncluded').is(':checked');            // radioButton1 of the FORM, as the desktop reads it
            list = list.slice().sort(function (a, b) { return a.id - b.id; });
            cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['debit', 'Debit', 1], ['credit', 'Credit', 1], ['fcyD', 'Fcy Debit', 3], ['fcyC', 'Fcy Credit', 3],
                  ['referenceAccount', 'ReferenceAccount'], F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
            data = list.map(function (x) {
                return $.extend({}, x, { debit: (inc && x.debit > 0) ? x.debit + x.taxAmount : x.debit, credit: (inc && x.credit > 0) ? x.credit + x.taxAmount : x.credit,
                                         fcyD: x.debit > 0 ? x.fcy : 0, fcyC: x.credit > 0 ? x.fcy : 0 });
            });
            S.histTaxSum = data.reduce(function (s, x) { return s + num(x.taxAmount); }, 0);
            if (S.histTaxSum > 0) cs.push(['taxAmount', 'TaxAmount', 1]);
        } else {
            cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['debit', 'Debit', 1], ['credit', 'Credit', 1], ['chequeDate', 'ChequeDate'],
                  ['chequeNo', 'ChequeNo'], ['payeeTitle', 'PayeeTitle'], ['fcyD', 'Fcy Debit', 3], ['fcyC', 'Fcy Credit', 3],
                  ['referenceAccount', 'ReferenceAccount'], F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
            data = list.filter(function (x) { return x.debit > 0 || x.credit > 0; }).map(function (x) {
                var back = x.isTaxable === 'False' && r.inclusiveTax;
                return $.extend({}, x, { debit: (back && x.debit > 0) ? x.debit + x.taxAmount : x.debit, credit: (back && x.credit > 0) ? x.credit + x.taxAmount : x.credit,
                                         fcyD: x.debit > 0 ? x.fcy : 0, fcyC: x.credit > 0 ? x.fcy : 0 });
            });
            /* GridBankDetail's TaxAmount column follows the CPV detail grid's sum (grdDetail), as the desktop codes it */
            if (S.histTaxSum > 0) cs.push(['taxAmount', 'TaxAmount', 1]);
        }
        cs = cs.filter(Boolean);
        var sums = {};
        H(d, 'Detail').find('thead').html('<tr>' + cs.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + c[1] + '</th>'; }).join('') + '</tr>');
        H(d, 'Detail').find('tbody').html(data.map(function (x) {
            return '<tr>' + cs.map(function (c) {
                var v = x[c[0]];
                if (c[2]) { sums[c[0]] = (sums[c[0]] || 0) + num(v); v = c[2] === 3 ? fmt3(v) : fmt(v); }
                return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join(''));
        H(d, 'Detail').find('tfoot').html('<tr>' + cs.map(function (c) {
            var s = c[2] && c[0] !== 'taxAmount' ? (c[2] === 3 ? fmt3(sums[c[0]] || 0) : fmt(sums[c[0]] || 0)) : '';
            return '<td class="num" style="font-weight:bold;">' + s + '</td>';
        }).join('') + '</tr>');
    }

    function historyButton(d, b, i, btn) {
        var v = S.hist[d].rows[i]; if (!v) return;
        if (b === 'edit') { reset(false); loadForEdit(v.id); }
        else if (b === 'p1') printDoc(1, v.id, int(v.documentTypeId), btn, null);
        else if (b === 'p2') printDoc(2, v.id, int(v.documentTypeId), btn, null);
        else if (b === 'p3') printDoc(3, v.id, int(v.documentTypeId), btn, null);
    }

    function wireHistory(d) {
        var hs = S.hist[d], $g = H(d, 'Grid');
        H(d, 'Reset').on('click', function () { historyReset(d); });
        H(d, 'Refresh').on('click', function () { historyAccounts(d, $(this)); });
        H(d, 'Show').on('click', function () { historyShow(d); });
        H(d, 'FromNo').add(H(d, 'ToNo')).on('keydown', function (e) {
            if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length > 1) return;
            if (!/[0-9]/.test(e.key)) e.preventDefault();
        });
        $g.on('click', 'tbody tr[data-i]', function () { var i = int($(this).data('i')); if (i !== hs.sel) selectHistory(d, i); });
        $g.on('dblclick', 'tbody tr[data-i]', function () { var v = hs.rows[int($(this).data('i'))]; if (v) loadForEdit(v.id); });
        $g.on('click', 'a.prv-vno', function (e) { e.stopPropagation(); var v = hs.rows[int($(this).data('i'))]; if (v) loadForEdit(v.id); });
        $g.on('click', 'button.prv-hb', function (e) { e.stopPropagation(); historyButton(d, $(this).data('b'), int($(this).data('i')), this); });
        $g.on('keydown', function (e) {
            var n = hs.rows.length; if (!n) return;
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                e.preventDefault();
                var i = hs.sel < 0 ? 0 : hs.sel + (e.key === 'ArrowDown' ? 1 : -1);
                if (i >= 0 && i < n) selectHistory(d, i);
            } else if (e.ctrlKey && e.key === 'Enter') {
                e.preventDefault(); e.stopPropagation();
                var v = hs.rows[hs.sel]; if (v) { reset(false); loadForEdit(v.id); }
            }
        });
    }

    // ------------------------------------------------------------------------------ tabs / keys / wiring

    /** tab = 'form' | 1 (CPV history) | 2 (BPV history). */
    function switchTab(tab) {
        if (tab === 'history') tab = S.doc;
        $('#tabFormContent, #tabHistContent1, #tabHistContent2').hide();
        $('#tabBtnForm, #tabBtnHist1, #tabBtnHist2').removeClass('active');
        if (tab === 'form') { $('#tabFormContent').show(); $('#tabBtnForm').addClass('active'); }
        else { $('#tabHistContent' + tab).show(); $('#tabBtnHist' + tab).addClass('active'); H(tab, 'From').focus(); }
    }
    function histDoc() { return $('#tabHistContent1').is(':visible') ? 1 : ($('#tabHistContent2').is(':visible') ? 2 : 0); }

    function focusNext(from) {
        var $all = $('#tabFormContent, #tabHistContent1, #tabHistContent2').filter(':visible')
            .find('input,select,textarea,button').filter(':visible').filter(function () {
                return !this.disabled && (!this.readOnly || $(this).hasClass('dtcombo-input')) && this.type !== 'hidden' && this.tabIndex >= 0;
            });
        var i = $all.index(from);
        if (i >= 0 && i + 1 < $all.length) $all.eq(i + 1).focus();
    }

    function onKey(e) {
        if ($('#srModal').is(':visible') || $('#ptvKeys').is(':visible')) return;
        var k = e.key || '', K = k.toUpperCase(), r = rights(), hd = histDoc();
        if (k === 'Escape') { w.location.href = '/accounts/dashboard'; return; }
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey) {
            var t = e.target;
            if (t && /^(INPUT|SELECT|TEXTAREA)$/.test(t.tagName) && t.type !== 'button' && t.type !== 'submit') {
                if ($(t).closest('#ptvGrid').length) { e.preventDefault(); $(t).trigger('change'); return; }
                e.preventDefault(); focusNext(t);
            }
            return;
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { shortcuts(); return; }
        if (e.ctrlKey && !e.altKey) {
            if (K === 'S') { e.preventDefault(); if (hd) historyShow(hd); else if ($('#btnSave').is(':visible') && !$('#btnSave').prop('disabled')) save('save'); }
            else if (K === 'U') { e.preventDefault(); if ($('#btnUpdate').is(':visible') && !$('#btnUpdate').prop('disabled')) save('update'); }
            else if (K === 'N') { e.preventDefault(); if (hd) historyReset(hd); else newClick(); }
            else if (K === 'E') { e.preventDefault(); w.location.href = '/accounts/dashboard'; }
            else if (K === 'T') { e.preventDefault(); if (hd) { switchTab('form'); comboFocus('ptvCredit'); } else switchTab('history'); }
            else if (K === 'R') {
                e.preventDefault();
                /* desktop quirk: with the three tabs of PaymentVoucherWithTax, Ctrl+R on the Form tab is New; with two it is Refresh */
                if (hd) historyAccounts(hd, H(hd, 'Refresh')); else if (S.mode === 0) newClick(); else refreshLists($('#btnRefresh'));
            }
            else if (k === 'F5') { e.preventDefault(); comboFocus($('#ptvVoucherType').prop('disabled') ? 'ptvCredit' : 'ptvVoucherType'); }
            else if (k === 'F10') { e.preventDefault(); attachments(); }
            else if (k === 'ArrowDown') { e.preventDefault(); (hd ? H(hd, 'Grid') : $('#ptvGrid')).focus(); }
            else if (k === 'ArrowUp') { e.preventDefault(); if (hd) H(hd, 'From').focus(); else comboFocus('ptvPaymentType'); }
            return;
        }
        if (e.altKey && !e.ctrlKey) {
            if ((k === '1' || e.code === 'Digit1' || e.code === 'Numpad1') && r.print) { e.preventDefault(); printButton(1, $('#btnPrint')[0]); }
            else if ((k === '2' || e.code === 'Digit2' || e.code === 'Numpad2') && r.print) { e.preventDefault(); printButton(2, $('#btnPrint2')[0]); }
            else if ((k === '3' || e.code === 'Digit3' || e.code === 'Numpad3') && r.print) { e.preventDefault(); printButton(3, $('#btnPrint3')[0]); }
        }
    }

    function wireForm() {
        $('#ptvVoucherType').on('change', applyDoc);                      // CmbVoucherType_Leave -> AccountTitleFill
        $('#ptvCredit').on('change', function () {                        // combcreditac_Leave
            var id = int($(this).val());
            if (id > 0) balance(id, $('#ptvCrBalance')); else $('#ptvCrBalance').hide().text('0');
            chequeFill();
        });
        $('#ptvAccount').on('change', function () {                       // combactitle_Leave: balance and the account's subsidiaries
            var id = int($(this).val());
            if (id > 0) balance(id, $('#ptvAccBalance')); else $('#ptvAccBalance').hide().text('0');
            bindSubsidiary(0); subBalance();
        });
        $('#ptvSubsidiary').on('change', subsidiaryChanged);
        $('input[name="ptvAccMode"]').on('change', function () {
            S.accMode = $(this).val();
            var cur = $('#ptvAccount').val();
            $('#ptvAccount').attr('data-dtcombo', S.accMode === 'code' ? 'ptvAcc4c' : 'ptvAcc4')
                .attr('data-dtcombo-caption', S.accMode === 'code' ? 'Account Code' : 'Account Title')
                .html(accOpts(S.L ? S.L.detailAccounts : [], true, S.accMode));
            if (int(cur) > 0) { $('#ptvAccount').val(cur).trigger('change'); comboFocus('ptvAccount'); }
        });
        $('input[name="ptvRefMode"]').on('change', function () {
            S.refMode = $(this).val();
            var cur = $('#ptvReference').val();
            $('#ptvReference').attr('data-dtcombo', S.refMode === 'code' ? 'ptvAcc4c' : 'ptvAcc4')
                .attr('data-dtcombo-caption', S.refMode === 'code' ? 'Reference Code' : 'Reference Account')
                .html(accOpts(S.L ? S.L.referenceAccounts : [], true, S.refMode));
            if (int(cur) > 0) { $('#ptvReference').val(cur).trigger('change'); comboFocus('ptvReference'); }
        });
        $('#ptvDate').on('change', function () { balance(int($('#ptvCredit').val()), $('#ptvCrBalance')); if ($('#ptvWht').is(':checked')) whtChanged(); });
        $('#ptvExchangeRate').on('keydown', decimalOnly).on('input', rateChangedHeader)
            .on('blur', function () { $(this).val(fmtRate($(this).val())); rateChangedHeader(); });
        $('#ptvCurrency').on('change', currencyLeave);
        $('#ptvTcyRate').on('keydown', decimalOnly).on('input', linkAmounts)
            .on('blur', function () { $(this).val(fmtRate($(this).val())); });
        $('#ptvAmount').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); S.amtSrc = 'amt'; linkAmounts(); });
        $('#ptvTcyAmount').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); S.amtSrc = 'fcy'; linkAmounts(); });
        $('#ptvDiscPct').on('keydown', decimalOnly).on('input', discPctChanged);
        $('#ptvDiscAmt').on('keydown', decimalOnly).on('input', discAmtChanged);
        $('#ptvSrbAmount').on('keydown', decimalOnly).on('input', srbAmtChanged);
        $('#ptvLineRemarks').on('mousedown', function () { S.remarksMouse = true; })
            .on('focus', function () { var el = this; if (!S.remarksMouse) setTimeout(function () { el.select(); }, 0); })
            .on('blur', function () { S.remarksMouse = false; });
        $('#ptvAdd').on('click', add);
        $('#ptvUpdateDetail').on('click', updateDetail);
        $('#ptvCancelDetail').on('click', cancelDetail);
        /* btnCancelDetail_PreviewKeyDown / _Leave: Tab out of the buttons returns to Payment Type */
        $('#ptvAdd, #ptvCancelDetail').on('keydown', function (e) { if (e.key === 'Tab' && !e.shiftKey) { e.preventDefault(); comboFocus('ptvPaymentType'); } });
        $('#ptvGrid').on('click', '.prv-del', function (ev) { ev.stopPropagation(); deleteRow(int($(this).data('i'))); });
        $('#ptvGrid').on('click', 'tbody tr[data-i]', function () { $('#ptvGrid tbody tr').removeClass('prv-sel'); $(this).addClass('prv-sel'); });
        $('#ptvGrid').on('dblclick', 'tbody tr[data-i]', function (ev) { if ($(ev.target).is('input,select')) return; editRow(int($(this).data('i'))); });
        $('#ptvGrid').on('keydown', function (e) {
            var sel = $('#ptvGrid tbody tr.prv-sel'); if (!sel.length) sel = $('#ptvGrid tbody tr[data-i]').first();
            if (!sel.length) return;
            var i = int(sel.data('i'));
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRow(i); }
            else if (e.ctrlKey && e.key === ' ') { e.preventDefault(); deleteRow(i); }
            else if (!e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowUp') && !$(e.target).is('select')) {
                var n = S.rows.length, j = i + (e.key === 'ArrowDown' ? 1 : -1);
                if (j >= 0 && j < n) { e.preventDefault(); $('#ptvGrid tbody tr').removeClass('prv-sel').filter('[data-i="' + j + '"]').addClass('prv-sel'); }
            }
        });
        /* in-cell edits (Payment Type, Remarks, Cost Center are the editable grid columns) */
        $('#ptvGrid').on('change', '.prv-ce', function () {
            var i = int($(this).data('i')), k = $(this).data('k'), r = S.rows[i]; if (!r) return;
            if (k === 'remarks') { r.remarks = $(this).val(); return; }
            r[k] = int($(this).val());
            var txt = $(this).find('option:selected').text();
            if (k === 'paymentTypeId') r.paymentType = txt; else if (k === 'costCenterId') r.costCenterName = txt;
        });
        $('#ptvWht').on('change', whtChanged);                              // checkBox1_CheckedChanged
        $('#ptvTaxType').on('change', function () { whtChanged(); });
        $('input[name="ptvTaxMode"]').on('change', function () { ptotal(); renderGrid(); });   // RadExcluded_CheckedChanged
        $('#ptvBtnHistory').on('click', function () { switchTab('history'); });
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
        $('#tabBtnHist1').on('click', function () { switchTab(1); });
        $('#tabBtnHist2').on('click', function () { switchTab(2); });
        $(document).on('keydown', onKey);
    }

    w.PTV = {
        init: function (cfg) {
            S.mode = int(cfg.mode);
            wireToolbar();
            var ready = $.getJSON(BASE + '/' + S.mode + '/lookups').then(function (L) {
                bindLookups(L);
                (S.mode === 0 ? [1, 2] : [S.mode === 1 ? 1 : 2]).forEach(renderHistory);
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
