/* ============================================================================================
 * Screen 862 ExpenseVoucherNew - "Expense Voucher New".
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.ExpenseVoucherNew (DocumentTypeId 26,
 * base.Name "ExpenseVoucherNew"). One grid row = one debit line (detail account); Insert() adds the credit
 * line against the header's single Credit Account for every row. Rows may carry a cost-centre breakup
 * (Apply Multi Cost Center / Cost Center Breakup / Cost Center Summary).
 *
 * The Form tab and the History tab are rendered here at the InitializeComponent coordinates (tab page 1418
 * wide, tabs at the bottom). Backend: /accounts/api/expense-tax (ExpenseTaxVoucherController): lookups from the
 * desktop's own fill methods, Save / Update through the desktop procedure chain, ReadById through
 * Sp_Vouchers_GetMethods, history through USP_VoucherFormHistory. The page posts the grid ROWS, the cost-centre
 * breakup rows and the header controls; the debit / credit line pairs and VoucherAmount are built on the server.
 *
 * Layout numbers are the Location / Size values of InitializeComponent(); the 27 px toolstrip is in the
 * template, so designer y values here are measured from the bottom of the toolstrip.
 * ============================================================================================ */
(function (w, $) {
    'use strict';

    var BASE = '/accounts/api/expense-tax';
    var DOC_TYPE = 26;

    var S = {
        L: null, F: {}, rows: [], breaks: [], cc: [], recId: 0, updIdx: -1, busy: false, subLen: 0, codeSeq: 0,
        creditType: 0, subSeq: 0, cheques: [], debitAmtGet: 0, remarksMouse: false, ack: { neg: false, dup: false },
        hist: { rows: [], sel: -1, busy: false }
    };
    /* print-rpt.js reads the open voucher's id */
    try { Object.defineProperty(w, 'RecId', { get: function () { return S.recId; }, configurable: true });
          Object.defineProperty(w, 'VoucherHeadId', { get: function () { return S.recId; }, configurable: true }); } catch (e) { }

    if (w.DesktopCombo) {
        /* AccountTitleFill / DetailAccountFill: Account Title | AccountCode | ParentAccountTitle | AccountClass | CurrencyCode (Id, type, CurrencyId hidden). */
        w.DesktopCombo.define('exnAcc', [
            { caption: 'Account Title', flex: 4 }, { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' }, { caption: 'AccountClass', flex: 2, key: 'cls' },
            { caption: 'CurrencyCode', flex: 2, key: 'curcode' }
        ]);
        /* BindReferenceAccounts: Id hidden; AccountTitle | AccountCode | ParentAccountTitle | AccountClass */
        w.DesktopCombo.define('exnRef', [
            { caption: 'Account Title', flex: 4 }, { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' }, { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
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
    /* DecimalRateFormate: points = config > 0 ? config : 2; 1-4 zeros, anything else -> none. */
    function rdec() { var n = int(S.F.rateDecimals); n = n > 0 ? n : 2; return n <= 4 ? n : 0; }
    function fmtN(v, d) { return num(v).toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function fmtRate(v) { return fmtN(v, rdec()); }
    function fmt3(v) { return num(v).toLocaleString('en-US', { maximumFractionDigits: 3 }); }   // "#,##0.###"
    function adec() { var n = int(S.F.amountDecimals); return n >= 1 && n <= 4 ? n : 0; }
    function fmt(v) { return fmtN(v, adec()); }            // clsGlobalVariables.stringFormatsingle (history grids)
    function round(v, d) { var p = Math.pow(10, d); return Math.round((v + (v >= 0 ? 1e-9 : -1e-9)) * p) / p; }
    function today() { var d = new Date(); d.setMinutes(d.getMinutes() - d.getTimezoneOffset()); return d.toISOString().slice(0, 10); }
    function day(v) { return v ? String(v).slice(0, 10) : ''; }
    function opts(list, idKey, textKey, blank) {
        var h = blank ? '<option value="0"></option>' : '';
        (list || []).forEach(function (r) { h += '<option value="' + esc(ci(r, idKey)) + '">' + esc(ci(r, textKey)) + '</option>'; });
        return h;
    }
    function accOpts(list) {
        var h = '<option value="0"></option>';
        (list || []).forEach(function (a) {
            h += '<option value="' + esc(ci(a, 'Id')) + '" data-code="' + esc(ci(a, 'AccountCode')) + '" data-title="' + esc(ci(a, 'AccountTitle')) +
                '" data-parent="' + esc(ci(a, 'ParentAccountTitle')) + '" data-cls="' + esc(ci(a, 'AccountClass')) + '" data-type="' + esc(ci(a, 'AccountTypeId')) +
                '" data-cur="' + esc(ci(a, 'CurrencyId')) + '" data-curcode="' + esc(ci(a, 'CurrencyCode')) + '">' + esc(ci(a, 'AccountTitle')) + '</option>';
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
    /* txtbalance / lblbalancedetail: ToString("#,#;(#,#);0") */
    function bal(v) { var b = Math.round(num(v)); return b < 0 ? '(' + Math.abs(b).toLocaleString('en-US') + ')' : b.toLocaleString('en-US'); }
    function drCr(v) { var b = num(v); return b > 0 ? 'Dr' : (b === 0 ? 'Nill' : 'Cr'); }
    /* C# double.ToString() for the messages */
    function net(v) { var n = num(v); return String(Math.abs(n - Math.round(n)) < 1e-9 ? Math.round(n) : parseFloat(n.toFixed(10))); }

    function busyOn($b) {
        $b.each(function () {
            var b = $(this); if (b.data('exnBusy')) return;
            b.data('exnBusy', 1).data('exnHtml', b.html()).prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> ' + $.trim(b.text()));
        });
    }
    function busyOff($b) {
        $b.each(function () {
            var b = $(this); if (!b.data('exnBusy')) return;
            b.html(b.data('exnHtml')).removeData('exnBusy').prop('disabled', false);
        });
    }

    // ------------------------------------------------------------------------------ markup helpers

    function px(x, y, w, h) {
        return 'left:' + x + 'px;top:' + y + 'px;' + (w != null ? 'width:' + w + 'px;' : '') + (h != null ? 'height:' + h + 'px;' : '');
    }
    function lab(x, y, text, cls, id, w, h) {
        return '<label class="prv-l' + (cls ? ' ' + cls : '') + '"' + (id ? ' id="' + id + '"' : '') + ' style="' + px(x, y, w, h) + '">' + text + '</label>';
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
    function pnl(id, inner, h) { return '<div class="prv-fp" id="' + id + '" style="position:absolute;left:0;top:0;height:' + (h || 47) + 'px;">' + inner + '</div>'; }

    // ------------------------------------------------------------------------------ Form tab markup

    function render() {
        /* ---- panel4: title, Location Type and the Top|Right anchored check boxes (1414 wide) */
        var strip = '<div class="prv-strip" style="height:27px;">' +
            '<span class="prv-strip-title" style="position:absolute;left:3px;top:4px;">EXPENSE VOUCHER</span>' +
            '<label class="prv-l" style="color:#fff;font:bold 9pt \'Segoe UI\',sans-serif;' + px(318, 6) + '">Location Type</label>' +
            cmb('exnLocation', 406, 0, 243, 26) +
            '<div class="prv-ar" style="width:1414px;height:27px;">' +
            '<label style="font:bold 8pt Verdana,sans-serif;' + px(994, 6, 137) + '"><input type="checkbox" id="exnCustom"/>Custom Accounts</label>' +
            '<label class="prv-v" style="' + px(1132, 4, 122) + '"><input type="checkbox" id="exnChkPrint" checked/>Print Preview</label>' +
            '<label class="prv-v" style="' + px(1255, 4, 157) + '"><input type="checkbox" id="exnChkFmt2"/>Preview Format II</label>' +
            '</div></div>';

        /* ---- groupBox2 "Main" (3,3) 1027x93 */
        var main =
            lab(5, 19, 'Project') + cmb('exnProject', 100, 13, 197, 26, '', 'prv-cmb-mss') +
            lab(5, 45, 'Voucher No,Date') + tb('exnDocNo', 100, 41, 72, 23, ' readonly tabindex="-1"') + dtp('exnDate', 173, 41, 123, 22) +
            lab(6, 68, 'Remarks') + tb('exnRemarks', 100, 65, 519, 23) +
            lab(302, 19, 'Credit Account') + cmb('exnCredit', 388, 13, 231, 26, ' data-dtcombo="exnAcc" data-dtcombo-caption="Account Title"') +
            lab(302, 45, 'Balance') + tb('exnBal', 388, 41, 231, 23, ' readonly tabindex="-1"', 'prv-num prv-dis') +
            lab(623, 19, 'Tcy Code') + cmb('exnCurrency', 705, 13, 124, 26) +
            lab(623, 37, 'Tcy Exchange <br/>Rate', null, null, 82, 30) + tb('exnRate', 705, 41, 124, 23, '', 'prv-num') +
            lab(623, 69, 'Tcy Amount') + tb('exnFcyAmount', 705, 65, 124, 23, ' readonly tabindex="-1"', 'prv-num prv-dis');

        /* ---- groupboxdetail "Detail" (3,99) 1027x114: flowLayoutPanel1 (2,15) 1019x94, panels laid out by layoutDetail() */
        var P = '';
        P += pnl('exnPAcc', lab(1, 3, 'Debit Account') +
            '<span id="exnDrLbl" class="prv-bal" style="display:none;color:#f00;' + px(95, 3) + '"></span>' +
            cmb('exnAccount', 1, 20, 209, 26, ' data-dtcombo="exnAcc" data-dtcombo-caption="Account Title"'));
        P += pnl('exnPSub', lab(1, 3, 'Subsidiary Account') +
            '<span id="exnSubBal" class="prv-bal" style="display:none;' + px(108, 3) + '"></span>' +
            cmb('exnSubsidiary', 1, 20, 203, 26));
        P += pnl('exnPJob', lab(1, 3, 'Job/Lot') + cmb('exnJobLot', 0, 20, 128, 26));
        P += pnl('exnPRem', lab(1, 5, 'Remarks') + tb('exnComments', 1, 22, 179, 23));
        P += pnl('exnPTcy', lab(0, 2, 'Tcy Code') + cmb('exnTcyCode', 1, 21, 86, 26));
        P += pnl('exnPRate', lab(-1, 3, 'Tcy Exchange Rate') + tb('exnTcyRate', 2, 22, 101, 23, '', 'prv-num'));
        P += pnl('exnPFcy', lab(1, 4, 'Tcy Amount') + tb('exnFcyDetail', 1, 22, 95, 23, '', 'prv-num'), 45);
        P += pnl('exnPAmt', lab(3, 4, 'Debit Amount') + tb('exnAmount', 1, 22, 108, 23, '', 'prv-num'));
        P += pnl('exnPRef', lab(2, 2, 'Reference Account') + cmb('exnRefAccount', 1, 19, 153, 26, ' data-dtcombo="exnRef" data-dtcombo-caption="Account Title"'));
        P += pnl('exnPCc', lab(1, 2, 'Cost Center') + cmb('exnCostCenter', 1, 18, 122, 26), 45);
        P += pnl('exnPBr', lab(2, 1, 'Branch Name') + cmb('exnBranch', 1, 18, 148, 26), 45);
        P += pnl('exnPChq', lab(1, 2, 'Cheque Date') + dtp('exnChqDate', 1, 19, 107, 22) +
            lab(108, 2, 'Cheque No') + '<div id="exnChqSelBox" style="position:absolute;left:109px;top:17px;width:113px;height:26px;">' + cmb('exnChqNo', 0, 0, 113, 26) + '</div>' + tb('exnChqText', 109, 18, 113, 23, ' style="display:none"') +
            lab(220, 1, 'PayTitle') + tb('exnPayTitle', 223, 18, 182, 23), 45);
        P += pnl('exnPBtn',
            '<button type="button" id="exnAdd" class="prv-fbtn" style="' + px(1, 22, 36, 23) + '">+</button>' +
            '<button type="button" id="exnUpdateDetail" class="prv-fbtn" style="display:none;' + px(1, 23, 61, 21) + '">Update</button>' +
            '<button type="button" id="exnCancelDetail" class="prv-fbtn" style="display:none;' + px(1, 0, 61, 22) + '">Cancel</button>');
        var top = '<div class="prv-abs" id="exnTop" style="height:214px;">' +
            gbox('Main', 3, 3, 1027, 93, main) +
            gbox('Detail', 3, 99, 1027, 114, '<div class="prv-g" id="exnFlow" style="' + px(2, 15) + '">' + P + '</div>', 'exnDetailBox') +
            '</div>';

        /* ---- tabControl2: "Detail" (grd) and "Cost Center Breakup" (grdCostCenterDetail) */
        var tabs = '<div class="exn-tabbar"><button type="button" class="win-tab-btn active" id="exnTabDet">Detail</button>' +
            '<button type="button" class="win-tab-btn" id="exnTabCc">Cost Center Breakup</button></div>';
        var det = '<div class="prv-mid exn-pane" id="exnPaneDet"><div class="prv-teal" style="height:21px;"><span style="left:1px;top:2px;font:bold 10pt Tahoma,sans-serif;">VOUCHER Detail</span></div>' +
            '<div class="prv-gridbox" style="right:0;top:21px;" id="exnGridBox"><table class="prv-jg" id="exnGrid" tabindex="0"><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
        var ccd = '<div class="prv-mid exn-pane" id="exnPaneCc" style="display:none;"><div class="prv-teal" style="height:22px;"><span style="left:1px;top:2px;font:bold 9.75pt Tahoma,sans-serif;">Cost Center BreakUp</span></div>' +
            '<div class="prv-gridbox" style="right:0;top:22px;"><table class="prv-jg prv-auto" id="exnCcDetail"><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
        var left = '<div class="exn-left">' + top + tabs + det + ccd + '</div>';

        /* ---- panel24: Apply Multi Cost Center (183) over Cost Center Summary */
        var right = '<div class="exn-right">' +
            '<div class="exn-white" style="height:22px;"><span>Apply Multi Cost Center</span></div>' +
            '<div class="exn-ccbox" style="height:161px;"><table class="prv-jg prv-auto" id="exnCcGrid"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>' +
            '<div class="prv-teal" style="height:21px;"><span style="left:3px;top:2px;font:bold 10pt Tahoma,sans-serif;">Cost Center Summary</span></div>' +
            '<div class="exn-ccbox" style="flex:1 1 auto;"><table class="prv-jg prv-auto" id="exnCcSum"><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';

        var strip0 = '<div class="prv-abs" id="exnStripBox" style="height:27px;">' + strip + '</div>';
        var body = '<div class="exn-body">' + left + right + '</div>';
        /* History button: to the right of the footer, as on every ported voucher */
        var bot = '<div class="prv-bot prv-abs" style="height:30px;"><div class="prv-ar" style="width:1414px;height:30px;">' +
            '<button type="button" class="prv-sysbtn" id="exnBtnHistory" style="left:1320px;top:4px;width:92px;height:22px;"><i class="fa fa-history"></i>History</button>' +
            '</div></div>';
        $('#prvForm').html(strip0 + body + bot);
    }

    /**
     * flowLayoutPanel1 (1019 wide): visible panels left to right in TabIndex order, wrapped at 1019.
     * Subsidiary / Branch follow the Load-time features, the cheque panel the credit account type (15);
     * without Subsidiary the comments panel is 384 wide (txtComments 382).
     */
    function layoutDetail() {
        var F = S.F, subs = !!F.subsidiaryFeature, branch = !!F.branchFeature, chq = S.creditType === 15;
        var remW = subs ? 180 : 384;
        var P = [['exnPAcc', 210, true], ['exnPSub', 204, subs], ['exnPJob', 128, true], ['exnPRem', remW, true], ['exnPTcy', 89, true],
                 ['exnPRate', 105, true], ['exnPFcy', 96, true], ['exnPAmt', 110, true], ['exnPRef', 155, true], ['exnPCc', 125, true],
                 ['exnPBr', 150, branch], ['exnPChq', 405, chq], ['exnPBtn', 67, true]];
        var x = 0, y = 0;
        P.forEach(function (p) {
            var $p = $('#' + p[0]);
            if (!p[2]) { $p.hide(); return; }
            if (x > 0 && x + p[1] > 1019) { x = 0; y += 47; }
            $p.css({ left: x + 'px', top: y + 'px', width: p[1] + 'px', display: 'block' });
            x += p[1];
        });
        $('#exnComments').css('width', (subs ? 179 : 382) + 'px');
    }

    // ------------------------------------------------------------------------------ lookups

    var COMBOS = ['exnProject', 'exnLocation', 'exnCredit', 'exnAccount', 'exnSubsidiary', 'exnJobLot', 'exnCurrency', 'exnTcyCode', 'exnRefAccount', 'exnBranch', 'exnCostCenter'];
    function capture() { var o = {}; COMBOS.forEach(function (id) { o[id] = $('#' + id).val(); }); return o; }
    function restore(o, ids) { ids.forEach(function (id) { if (o[id] != null) put(id, o[id]); }); }

    function ccOpts(list) {
        var h = '<option value="0"></option>';
        (list || []).forEach(function (r) {
            h += '<option value="' + esc(ci(r, 'Id')) + '" data-parent="' + esc(ci(r, 'ParentCostCenterId')) + '">' + esc(ci(r, 'CostCenterName')) + '</option>';
        });
        return h;
    }

    function fillLists(L) {
        $('#exnProject').html(opts(L.projects, 'Id', 'ProjectName', false));
        $('#exnLocation').html(opts(L.locationTypes, 'Id', 'Location', true));        // ZeroIndex: true
        $('#exnCredit').html(accOpts(L.creditAccounts));                               // AccountTitleFill
        $('#exnAccount').html(accOpts(L.accounts));                                    // DetailAccountFill
        $('#exnRefAccount').html(accOpts(L.refAccounts));                              // BindReferenceAccounts
        $('#exnJobLot').html(opts(L.jobLots, 'Id', 'JobLotDescription', true));
        var cur = opts(L.currencies, 'Id', 'CurrencyCode', true);                      // CurrencyFill: both Tcy combos
        $('#exnCurrency').html(cur); $('#exnTcyCode').html(cur);
        $('#exnBranch').html(opts(L.branches, 'BranchId', 'BranchName', true));
        $('#exnCostCenter').html(ccOpts(L.costCenters));                               // GetAllSubCostCenters
        bindSubsidiary(0);
    }

    function bindLookups(L) {
        S.L = L; S.F = L.flags || {};
        render();
        fillLists(L);
        layoutDetail();
        $('#btnPrint, #btnPrint2').prop('disabled', !rights().print);                  // Print.Enabled = DoHavePrintRights
        wireForm();
        reset(true);
    }

    /** DefaultConfigurations(): Job/Lot, Base Currency (+ cmbCurrency_Leave), BaseCurrencyRate. */
    function defaults() {
        if (int(S.F.defaultJobLotId) > 0) put('exnJobLot', S.F.defaultJobLotId);
        if (int(S.F.baseCurrencyId) > 0) { put('exnCurrency', S.F.baseCurrencyId); currencyLeave(); }
        if (S.F.baseCurrencyRate != null) $('#exnRate').val(fmtRate(S.F.baseCurrencyRate));
    }

    /** btnRefresh_Click: location, branches, credit / detail / reference accounts, job lot, defaults, sub cost centres, CheqBook Enabled. */
    function refreshLists($b) {
        if (S.busy) return;
        busyOn($b);
        $.getJSON(BASE + '/lookups').then(function (L) {
            var keep = capture();
            S.L = L; S.F = L.flags || S.F;
            fillLists(L);
            restore(keep, ['exnProject', 'exnLocation', 'exnCredit', 'exnAccount', 'exnCurrency', 'exnTcyCode', 'exnRefAccount', 'exnCostCenter']);
            if (S.F.defaultBranchId) setVal('exnBranch', S.F.defaultBranchId);        // BranchesFill: Text = UserAccount.BranchName
            defaults();
            layoutDetail();
        }, function (x) { alert(errMsg(x, 'Could not refresh the lists.')); }).always(function () { busyOff($b); });
    }

    // ------------------------------------------------------------------------------ grid (grd)

    /** grdSettings(): visible columns, captions, order and widths. The Delete "X" button column is first and frozen. */
    function cols() {
        var F = S.F, subs = !!F.subsidiaryFeature, c = [{ k: 'x', t: 'X', w: 20 }, { k: 'accountCode', t: 'AccountCode', w: 80 },
            { k: 'accountTitle', t: 'Account Title', w: subs ? 150 : 200 }];
        if (F.multiCurrencyFeature) c.push({ k: 'glCurrency', t: 'GlCurrency', w: 75 });
        if (subs) c.push({ k: 'subsidiaryAccount', t: 'SubsidiaryAccount', w: 130 });
        c.push({ k: 'jobLot', t: 'JobLot', w: subs ? 90 : 105 }, { k: 'remarks', t: 'Remarks', w: F.branchFeature ? 120 : 180 },
               { k: 'tcyCode', t: 'TcyCode', w: 60 }, { k: 'tcyExchangeRate', t: 'TcyExchangeRate', n: 3, w: 85 },
               { k: 'fcyAmount', t: 'Tcy Amount', n: 3, sum: 3, w: 90 }, { k: 'amount', t: 'Debit Amount', n: 1, sum: 1, w: 90 },
               { k: 'referenceAccount', t: 'ReferenceAccount', w: 120 }, { k: 'costCenterName', t: 'CostCenterName', w: 110 });
        if (F.branchFeature) c.push({ k: 'branchName', t: 'BranchName', w: 100 });
        if (S.creditType === 15) c.push({ k: 'chequeDate', t: 'ChequeDate', w: 90 }, { k: 'chequeNo', t: 'ChequeNo', w: 110 }, { k: 'payTitle', t: 'PayTitle', w: 130 });
        return c;
    }

    /* Janus GridEX columns keep their designer widths (ColumnAutoResize = false). */
    function renderGridHead() {
        var cs = cols(), sum = cs.reduce(function (s, c) { return s + (c.w || 100); }, 0);
        var boxW = $('#exnGridBox').innerWidth() || 1000;
        var scale = sum > 0 ? Math.max(1, (boxW - 2) / sum) : 1;
        $('#exnGrid').css('width', '100%');
        $('#exnGrid colgroup').html(cs.map(function (c) { return '<col style="width:' + Math.round((c.w || 100) * scale) + 'px;"/>'; }).join(''));
        $('#exnGrid thead').html('<tr>' + cs.map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + ' title="' + esc(c.t) + '">' + c.t + '</th>'; }).join('') + '</tr>');
    }

    function renderGrid() {
        renderGridHead();
        var cs = cols(), html = '';
        S.rows.forEach(function (r, i) {
            html += '<tr data-i="' + i + '">' + cs.map(function (c) {
                if (c.k === 'x') return '<td style="text-align:center;padding:0;"><button type="button" class="prv-cbtn prv-del" data-i="' + i + '">X</button></td>';
                var v = r[c.k];
                if (c.n === 3) v = fmt3(v); else if (c.n === 1) v = fmt(v);
                if (c.k === 'chequeDate') v = day(v);
                return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $('#exnGrid tbody').html(html || '<tr><td colspan="' + cs.length + '" class="prv-empty"></td></tr>');
        var t = totals();
        $('#exnGrid tfoot').html(S.rows.length ? '<tr>' + cs.map(function (c) {
            var s = c.sum === 1 ? fmt(t.amt) : c.sum === 3 ? fmt3(t.fcy) : '';
            return '<td class="num" style="font-weight:bold;">' + s + '</td>';
        }).join('') + '</tr>' : '');
        calcTotals();
    }

    function totals() {
        var t = { amt: 0, fcy: 0 };
        S.rows.forEach(function (r) { t.amt += num(r.amount); t.fcy += num(r.fcyAmount); });
        return t;
    }

    /** CalculateTotalInformation(): txtFcyAmount = sum of the rows' Tcy Amount "#,##0.###"; "0" with no rows. */
    function calcTotals() { $('#exnFcyAmount').val(S.rows.length ? fmt3(totals().fcy) : '0'); }

    // ------------------------------------------------------------------------------ cost centres

    /** GridCostCenterFill(): every sub cost centre, unchecked, Prcnt 0, Amount 0. */
    function ccReset() {
        S.cc = ((S.L && S.L.costCenters) || []).map(function (r) {
            return { id: int(ci(r, 'Id')), name: ci(r, 'CostCenterName') || '', parentId: int(ci(r, 'ParentCostCenterId')), parent: ci(r, 'ParentCostCenter') || '', prcnt: 0, amount: 0, checked: false };
        });
        renderCc();
    }
    function ccChecked() { return S.cc.filter(function (c) { return c.checked; }); }

    function renderCc() {
        var $g = $('#exnCcGrid');
        $g.find('thead').html('<tr><th style="width:30px;"><input type="checkbox" id="exnCcAll" title="Select all"/></th><th>CostCenter</th><th class="num" style="width:50px;">Prcnt</th><th class="num" style="width:100px;">Amount</th></tr>');
        var groups = [], map = {};
        S.cc.forEach(function (c, i) {
            var g = map[c.parent]; if (!g) { g = map[c.parent] = { name: c.parent, items: [] }; groups.push(g); }
            g.items.push(i);
        });
        var h = '';
        groups.forEach(function (g) {
            h += '<tr class="exn-grp"><td colspan="4">' + esc(g.name) + '</td></tr>';
            g.items.forEach(function (i) {
                var c = S.cc[i];
                h += '<tr data-i="' + i + '"><td style="text-align:center;"><input type="checkbox" class="exn-cck" data-i="' + i + '"' + (c.checked ? ' checked' : '') + '/></td>' +
                    '<td>' + esc(c.name) + '</td>' +
                    '<td class="num"><input type="text" class="prv-ce num exn-cp" data-i="' + i + '" value="' + esc(fmt3(c.prcnt)) + '"/></td>' +
                    '<td class="num"><input type="text" class="prv-ce num exn-ca" data-i="' + i + '" value="' + esc(fmt3(c.amount)) + '"/></td></tr>';
            });
        });
        $g.find('tbody').html(h);
        $g.find('tfoot').html('<tr><td></td><td></td><td class="num exn-tp" style="font-weight:bold;"></td><td class="num exn-ta" style="font-weight:bold;"></td></tr>');
        ccTotals();
    }
    /* in-place refresh of the cost-centre grid values (the grid keeps focus while the operator tabs through it) */
    function ccSync() {
        S.cc.forEach(function (c, i) {
            var $r = $('#exnCcGrid tbody tr[data-i="' + i + '"]');
            $r.find('.exn-cck').prop('checked', c.checked);
            $r.find('.exn-cp').val(fmt3(c.prcnt)); $r.find('.exn-ca').val(fmt3(c.amount));
        });
        ccTotals();
    }
    function ccTotals() {
        var p = 0, a = 0; S.cc.forEach(function (c) { p += num(c.prcnt); a += num(c.amount); });
        $('#exnCcGrid .exn-tp').text(fmt3(p)); $('#exnCcGrid .exn-ta').text(fmt3(a));
    }

    /** grdCostCenter_CellUpdated: Prcnt / Amount edits against the Debit Amount; the row is checked while Prcnt > 0. */
    function ccEdit(i, key, raw) {
        var c = S.cc[i]; if (!c) return;
        var debit = num($('#exnAmount').val()) > 0 ? num($('#exnAmount').val()) : 0;
        if (key === 'prcnt') {
            c.prcnt = num(raw);
            var tp = 0; S.cc.forEach(function (x) { if (x.checked && x !== c) tp += num(x.prcnt); });
            var totalPrcnt = tp + (c.checked ? c.prcnt : 0);
            /* TotalPrcnt sums the CHECKED rows (the edited row included only if it was already checked) */
            if (c.prcnt > 100 || totalPrcnt > 100) { c.prcnt = 0; alert("Total Percentage Can't greater than 100"); }
            c.amount = debit * (c.prcnt / 100);
        } else {
            c.amount = num(raw);
            var ta = 0; S.cc.forEach(function (x) { if (x.checked && x !== c) ta += num(x.amount); });
            var totalAmount = ta + (c.checked ? c.amount : 0);
            if (c.amount > debit || totalAmount > debit) { c.amount = 0; alert("Total Amount Can't greater than DebitAmount " + net(debit)); }
            c.prcnt = round(debit > 0 ? c.amount / debit * 100 : 0, 3);
        }
        c.checked = num(c.prcnt) > 0;
        ccSync();
    }

    /** ChangeAmountsOfCostCenter (txtDebitAmount_Leave): the checked rows' Amount = Debit * Prcnt / 100. */
    function ccAmounts() {
        var debit = num($('#exnAmount').val());
        ccChecked().forEach(function (c) { c.amount = debit * (num(c.prcnt) / 100); });
        ccSync();
    }

    // ------------------------------------------------------------------------------ cost-centre breakup (dtGridCostCenterDetail)

    function ccParentOfCombo() { return int($('#exnCostCenter option:selected').attr('data-parent')); }

    /** InsertCostCenterBreakUp(LineId) */
    function breakInsert(lineId) {
        var chk = ccChecked(), title = selText('exnAccount');
        if (chk.length > 1) {
            chk.forEach(function (c) {
                S.breaks.push({ id: 0, lineId: lineId, parentId: c.parentId, debitAccount: title, costCenterId: c.id, costCenter: c.name, percent: num(c.prcnt), amount: num(c.amount) });
            });
        } else if (int($('#exnCostCenter').val()) > 0) {
            S.breaks.push({ id: 0, lineId: lineId, parentId: ccParentOfCombo(), debitAccount: title, costCenterId: int($('#exnCostCenter').val()),
                costCenter: selText('exnCostCenter'), percent: 100, amount: num($('#exnAmount').val()) });
        }
        renderBreaks();
    }
    function breakCount(lineId) { return S.breaks.filter(function (b) { return b.lineId === lineId; }).length; }
    /** DeleteCostCenterDetail(LineId) */
    function breakDelete(lineId) {
        if (lineId <= 0) return;
        S.breaks = S.breaks.filter(function (b) { return b.lineId !== lineId; });
    }
    /** UpdateCostCenterBreakUp(LineId) */
    function breakUpdate(lineId) {
        var chk = ccChecked(), cmbCc = int($('#exnCostCenter').val()), title = selText('exnAccount'), has = breakCount(lineId) > 0;
        if (chk.length === 0 && cmbCc === 0) {
            if (has) { breakDelete(lineId); summary(); }
        } else if (chk.length > 1) {
            if (!has) return;
            if (breakCount(lineId) !== chk.length) { breakDelete(lineId); breakInsert(lineId); summary(); return; }
            S.cc.forEach(function (c) {
                S.breaks.forEach(function (b) {
                    if (b.lineId === lineId && b.costCenter === c.name) {
                        b.parentId = c.parentId; b.debitAccount = title; b.costCenterId = c.id; b.costCenter = c.name; b.percent = num(c.prcnt); b.amount = num(c.amount);
                    }
                });
            });
            renderBreaks(); summary();
        } else {
            if (cmbCc <= 0 || !has) return;
            if (breakCount(lineId) !== 1) { breakDelete(lineId); breakInsert(lineId); summary(); return; }
            S.breaks.forEach(function (b) {
                if (b.lineId === lineId) {
                    b.parentId = ccParentOfCombo(); b.debitAccount = title; b.costCenterId = cmbCc; b.costCenter = selText('exnCostCenter');
                    b.percent = 100; b.amount = num($('#exnAmount').val());
                }
            });
            renderBreaks(); summary();
        }
    }

    function renderBreaks() {
        var $t = $('#exnCcDetail'), sum = 0;
        $t.find('thead').html('<tr><th style="width:40px;">LineId</th><th>DebitAccount</th><th style="width:150px;">CostCenter</th><th class="num" style="width:60px;">Percent</th><th class="num" style="width:90px;">Amount</th></tr>');
        $t.find('tbody').html(S.breaks.map(function (b) {
            sum += num(b.amount);
            return '<tr><td>' + esc(b.lineId) + '</td><td>' + esc(b.debitAccount) + '</td><td>' + esc(b.costCenter) + '</td><td class="num">' + esc(fmt3(b.percent)) + '</td><td class="num">' + esc(fmt3(b.amount)) + '</td></tr>';
        }).join(''));
        $t.find('tfoot').html(S.breaks.length ? '<tr><td></td><td></td><td></td><td></td><td class="num" style="font-weight:bold;">' + fmt3(sum) + '</td></tr>' : '');
    }

    /** SummaryForCostCenterBreakUp(): grouped by CostCenterId, amounts summed; cleared when there are no rows. */
    function summary() {
        var $t = $('#exnCcSum'), ids = [], by = {}, total = 0;
        S.breaks.forEach(function (b) {
            if (!(b.costCenterId in by)) { by[b.costCenterId] = { name: b.costCenter, amt: 0 }; ids.push(b.costCenterId); }
            by[b.costCenterId].amt += num(b.amount);
        });
        if (!ids.length) { $t.find('thead, tbody, tfoot').empty(); return; }
        $t.find('thead').html('<tr><th>CostCenter</th><th class="num" style="width:100px;">Amount</th></tr>');
        $t.find('tbody').html(ids.map(function (id) { total += by[id].amt; return '<tr><td>' + esc(by[id].name) + '</td><td class="num">' + esc(fmt3(by[id].amt)) + '</td></tr>'; }).join(''));
        $t.find('tfoot').html('<tr><td></td><td class="num" style="font-weight:bold;">' + fmt3(total) + '</td></tr>');
    }

    // ------------------------------------------------------------------------------ credit account / cheque

    function chqOn() { return !!S.F.chequeBookEnabled; }
    function chqVal() {
        if (chqOn()) { var id = int($('#exnChqNo').val()); return { id: id, no: id > 0 ? selText('exnChqNo') : '' }; }
        return { id: 0, no: $.trim($('#exnChqText').val()) };
    }
    function chqMode() { $('#exnChqSelBox').toggle(chqOn()); $('#exnChqText').toggle(!chqOn()); }

    /** CheqNoFill(): the credit (bank) account's outstanding cheques; nothing happens unless "CheqBook Enabled" is on. */
    function cheqFill() {
        if (!chqOn()) return $.Deferred().resolve().promise();
        var bank = int($('#exnCredit').val());
        $('#exnChqNo').html('<option value="0"></option>'); put('exnChqNo', 0);
        return $.getJSON(BASE + '/cheques', { bankId: bank, recId: S.recId }).then(function (list) {
            S.cheques = list || [];
            $('#exnChqNo').html(opts(S.cheques, 'Id', 'CheqNo', true)); put('exnChqNo', 0);
        });
    }

    /** CmbAccountId_ValueChanged (called from combcreditac_Leave): type 15 shows the cheque panel and its grid columns. */
    function creditTypeChanged() {
        var id = int($('#exnCredit').val()), t = id > 0 ? int($('#exnCredit option:selected').attr('data-type')) : 0;
        S.creditType = t;
        if (t === 15) cheqFill();
        else { $('#exnChqNo').html('<option value="0"></option>'); put('exnChqNo', 0); $('#exnChqText').val(''); }
        layoutDetail();
        renderGrid();
    }

    /** combcreditac_Leave: AccountCurrentBalance, then the cheque panel. */
    function creditLeave() {
        var id = int($('#exnCredit').val());
        if (id > 0 && $('#exnCredit option:selected').text() !== '') {
            $.getJSON(BASE + '/balance', { accountId: id, date: $('#exnDate').val() }).then(function (r) { $('#exnBal').val(bal(r && r.balance)); });
        } else { $('#exnBal').val('0'); }
        creditTypeChanged();
    }

    // ------------------------------------------------------------------------------ detail entry

    /** combactitle_Leave: the balance of the selected detail account on the voucher date, then BindSubsidiaryAccount. */
    function accountLeave() {
        var id = int($('#exnAccount').val());
        if (id > 0 && $('#exnAccount option:selected').text() !== '') {
            $.getJSON(BASE + '/balance', { accountId: id, date: $('#exnDate').val() }).then(function (r) {
                $('#exnDrLbl').text(bal(r && r.balance)).show();
            });
        } else { $('#exnDrLbl').text('0').hide(); }
        return bindSubsidiary(0);
    }

    /** BindSubsidiaryAccount(): VoucherHead.BindSubsidiaryAccount(CompanyId, account, "3"); a single row is activated. */
    function bindSubsidiary(retainId) {
        var $s = $('#exnSubsidiary'), acc = int($('#exnAccount').val());
        function empty() { S.subLen = 0; $s.html('<option value="0"></option>'); put('exnSubsidiary', 0); $('#exnSubBal').hide(); }
        if (!S.F.subsidiaryFeature || acc <= 0 || $('#exnAccount option:selected').text() === '') { empty(); return $.Deferred().resolve().promise(); }
        var seq = ++S.subSeq;
        return $.getJSON(BASE + '/subsidiaries', { accountId: acc }).then(function (list) {
            if (seq !== S.subSeq || acc !== int($('#exnAccount').val())) return;
            if (!list || !list.length) { empty(); return; }
            S.subLen = list.length;
            $s.html('<option value="0"></option>' + list.map(function (r) {
                return '<option value="' + esc(ci(r, 'Id')) + '" data-type="' + esc(ci(r, 'SubsidiaryTypeId')) + '">' + esc(ci(r, 'SubsidiaryAccount')) + '</option>';
            }).join(''));
            put('exnSubsidiary', retainId || (list.length === 1 ? ci(list[0], 'Id') : 0));
            subBalance();
        }, function () { empty(); });
    }

    /** CmbSubsidiaryAccount_ValueChanged: the subsidiary's own balance. */
    function subBalance() {
        var acc = int($('#exnAccount').val()), sub = int($('#exnSubsidiary').val());
        if (acc > 0 && sub > 0) {
            $('#exnSubBal').text('0').show();
            $.getJSON(BASE + '/subsidiary-balance', { accountId: acc, subsidiaryId: sub, date: $('#exnDate').val() }).then(function (r) { $('#exnSubBal').text(bal(r && r.balance)); });
        } else { $('#exnSubBal').hide().text('0'); }
    }

    /** btnplus_Click / btnUpdateDetail_Click FormValidation: the same checks and order, "Account Title Field Required" on Update. */
    function detailValid(upd) {
        var F = S.F;
        if (int($('#exnAccount').val()) === 0) return [upd ? 'Account Title Field Required' : 'Debit Account Title Field Required', 'exnAccount'];
        if (F.multiCurrencyFeature && int($('#exnAccount option:selected').attr('data-cur')) === 0) return ['Debit Account Currency not set Please Check', 'exnAccount'];
        if (F.subsidiaryFeature && S.subLen > 0 && int($('#exnSubsidiary').val()) === 0) return ['Subsidiary Account Title Field Required', 'exnSubsidiary'];
        if (int($('#exnJobLot').val()) === 0) return ['Job/Lot Field Required', 'exnJobLot'];
        if (int($('#exnTcyCode').val()) === 0) return ['Tcy Code Field is Required', 'exnTcyCode'];
        if ($.trim($('#exnTcyRate').val()) === '' || num($('#exnTcyRate').val()) === 0) return ['Tcy Exchange Rate Field is Required', 'exnTcyRate'];
        if (num($('#exnAmount').val()) === 0) return ['Amount Field Required', 'exnAmount'];
        var chk = ccChecked();
        if (chk.length > 1) {
            var tp = 0, ta = 0; chk.forEach(function (c) { tp += num(c.prcnt); ta += num(c.amount); });
            if (Math.round(tp) !== 100 || round(ta, 6) !== round(num($('#exnAmount').val()), 6)) return ['Percent not equal to 100 Or Amount not Equal To Debit Amount', null];
        }
        if (F.branchFeature && int($('#exnBranch').val()) === 0) return ['BranchName Field is Required', 'exnBranch'];
        return null;
    }
    function fail(e) {
        alert(e[0]);
        if (!e[1]) return;
        if (/^exn(Account|Subsidiary|JobLot|TcyCode|Branch)$/.test(e[1])) comboFocus(e[1]); else $('#' + e[1]).focus();
    }

    function rowFromEntry(lineId) {
        var accOpt = $('#exnAccount option:selected'), sub = int($('#exnSubsidiary').val()), subOpt = $('#exnSubsidiary option:selected');
        var multi = ccChecked().length > 1, ref = int($('#exnRefAccount').val()), cq = chqVal();
        var r = {
            accountCode: accOpt.attr('data-code') || '', accountId: int($('#exnAccount').val()), accountTitle: accOpt.attr('data-title') || accOpt.text(),
            glCurrencyId: int(accOpt.attr('data-cur')), glCurrency: accOpt.attr('data-curcode') || '',
            subsidiaryAccountId: 0, subsidiaryAccount: '', subsidiaryAccountTypeId: 0,
            jobLotId: int($('#exnJobLot').val()), jobLot: selText('exnJobLot'), remarks: $.trim($('#exnComments').val()),
            tcyCodeId: int($('#exnTcyCode').val()), tcyCode: selText('exnTcyCode'), tcyExchangeRate: num($('#exnTcyRate').val()),
            fcyAmount: num($('#exnFcyDetail').val()), amount: num($('#exnAmount').val()),
            referenceAccountId: ref, referenceAccount: ref > 0 ? $('#exnRefAccount option:selected').attr('data-title') || selText('exnRefAccount') : '',
            lineId: lineId, costCenterId: multi ? 0 : int($('#exnCostCenter').val()), costCenterName: multi ? '' : selText('exnCostCenter'),
            branchId: int($('#exnBranch').val()), branchName: selText('exnBranch'),
            chequeId: cq.id, chequeDate: $('#exnChqDate').val(), chequeNo: cq.no, payTitle: $('#exnPayTitle').val() || ''
        };
        if (S.F.subsidiaryFeature) {
            if (S.subLen > 0 && sub > 0) { r.subsidiaryAccountId = sub; r.subsidiaryAccount = subOpt.text(); r.subsidiaryAccountTypeId = int(subOpt.attr('data-type')); }
            else { r.subsidiaryAccountId = r.accountId; r.subsidiaryAccount = r.accountTitle; r.subsidiaryAccountTypeId = 4; }
        }
        return r;
    }

    function clearEntry() {
        put('exnAccount', 0); $('#exnDrLbl').text('').hide();
        $('#exnFcyDetail, #exnComments, #exnAmount').val('');
    }

    /** Add_Click_1: LineId = max + 1, the breakup rows, GridCostCenterFill, summary, then the entry boxes are cleared. */
    function add() {
        var e = detailValid(false);
        if (e) { fail(e); return; }
        var lineId = 1;
        if (S.rows.length) lineId = Math.max.apply(null, S.rows.map(function (r) { return int(r.lineId); })) + 1;
        S.rows.push(rowFromEntry(lineId));
        breakInsert(lineId);
        ccReset();
        summary();
        clearEntry();
        bindSubsidiary(0);
        renderGrid();
        comboFocus('exnAccount');
    }

    function detailButtons(upd) { $('#exnAdd').toggle(!upd); $('#exnUpdateDetail, #exnCancelDetail').toggle(upd); }

    /** btnUpdateDetail_Click: the row at updateDetailIndex is rewritten, then the breakup rows follow. */
    function updateDetail() {
        var e = detailValid(true);
        if (e) { fail(e); return; }
        if (S.updIdx < 0 || !S.rows[S.updIdx]) return;
        var lineId = int(S.rows[S.updIdx].lineId);
        S.rows[S.updIdx] = rowFromEntry(lineId);
        bindSubsidiary(0);
        rateChangedHeader();                                  // txtExchangeRate_TextChanged(null, null)
        if (breakCount(lineId) === 0) breakInsert(lineId); else breakUpdate(lineId);
        summary();
        ccReset();
        S.updIdx = -1;
        detailButtons(false);
        clearEntry();
        renderGrid();
        comboFocus('exnAccount');
    }

    /** btnCancelUpdateDetial_Click */
    function cancelDetail() {
        S.updIdx = -1;
        detailButtons(false);
        comboFocus('exnAccount');
        $('#exnComments, #exnAmount').val('');
        put('exnAccount', 0); $('#exnDrLbl').hide();
        ccReset();
    }

    /** grd_DoubleClick (and Ctrl+Enter on the grid): the row goes back into the entry box and its breakup into the cost-centre grid. */
    function editRow(i) {
        var r = S.rows[i]; if (!r) return;
        S.updIdx = i;
        put('exnAccount', r.accountId);
        accountLeave().always(function () {
            if (S.F.subsidiaryFeature) {
                if (S.subLen === 0) {
                    $('#exnSubsidiary').html('<option value="0"></option><option value="' + esc(r.subsidiaryAccountId) + '" data-type="4">' + esc(r.subsidiaryAccount) + '</option>');
                }
                put('exnSubsidiary', r.subsidiaryAccountId);
                subBalance();
            }
        });
        put('exnJobLot', r.jobLotId);
        $('#exnComments').val(r.remarks || '');
        put('exnTcyCode', r.tcyCodeId);
        $('#exnTcyRate').val(fmt3(r.tcyExchangeRate));
        $('#exnFcyDetail').val(fmt3(r.fcyAmount));
        $('#exnAmount').val(fmt(r.amount));
        if (int(r.referenceAccountId) > 0) put('exnRefAccount', r.referenceAccountId);
        put('exnBranch', r.branchId);
        put('exnCostCenter', r.costCenterId);
        if (chqOn()) put('exnChqNo', r.chequeId); else $('#exnChqText').val(r.chequeNo || '');
        if (r.chequeDate) $('#exnChqDate').val(day(r.chequeDate));
        $('#exnPayTitle').val(r.payTitle || '');
        comboFocus('exnAccount');
        detailButtons(true);
        var mine = S.breaks.filter(function (b) { return b.lineId === int(r.lineId); });
        if (mine.length) {
            S.cc.forEach(function (c) {
                var b = mine.filter(function (x) { return x.costCenterId === c.id; })[0];
                c.checked = !!b; c.prcnt = b ? num(b.percent) : 0; c.amount = b ? num(b.amount) : 0;
            });
            ccSync();
        } else ccReset();
    }

    /** grd_ColumnButtonClick "Delete" (and Ctrl+Space on that column). */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        var lineId = int(r.lineId);
        S.rows.splice(i, 1);
        renderGrid();
        breakDelete(lineId);
        renderBreaks();
        summary();
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
    function dec() { var n = int(S.F.amountDecimals); return n > 0 ? n : 2; }

    /**
     * txtDebitAmount_TextChanged / txtFcyAmountDetail_TextChanged: UpdateRelatedFieldsCalculation links the Debit
     * Amount to the Tcy Amount with the detail rate (Tcy = amount / rate, amount = Tcy * rate).
     */
    function link(which) {
        var rate = num($('#exnTcyRate').val()), amt = '#exnAmount', fcy = '#exnFcyDetail';
        var src = which === 'amt' ? amt : fcy;
        if ($.trim($(src).val()) === '') return;
        if (rate <= 0) { $(amt).val('0'); $(fcy).val('0'); return; }
        if (src === amt) { var a = num($(amt).val()); $(fcy).val(a > 0 ? String(round(a / rate, dec())) : '0'); }
        else { var f = num($(fcy).val()); $(amt).val(f > 0 ? String(round(f * rate, dec())) : '0'); }
    }

    /** txtExchangeRate_TextChanged: the detail rate follows the header rate, then CalculateTotalInformation(). */
    function rateChangedHeader() { $('#exnTcyRate').val($('#exnRate').val()); calcTotals(); }

    /** cmbCurrency_Leave: the Tcy detail code follows; non-base currencies take the last rate used on a type-26 voucher, the base currency its configured rate. */
    function currencyLeave() {
        var cur = int($('#exnCurrency').val());
        if (cur === 0) return;
        put('exnTcyCode', cur);
        if (cur !== int(S.F.baseCurrencyId)) {
            $.getJSON(BASE + '/last-rate', { currencyId: cur }).then(function (r) {
                var lr = num(r && r.lastRate), t = lr ? fmtRate(lr) : '0';
                $('#exnRate').val(t); $('#exnTcyRate').val(t); calcTotals();
            }, function (x) { alert(errMsg(x)); });
        } else {
            var t = fmtRate(S.F.baseCurrencyRate);
            $('#exnRate').val(t); $('#exnTcyRate').val(t);
        }
    }

    // ------------------------------------------------------------------------------ reset / load

    function showButtons(m) {
        var r = rights();
        $('#btnSave').toggle(m === 'save').prop('disabled', !r.save);
        $('#btnUpdate').toggle(m === 'update').prop('disabled', !r.update);
    }

    /** VoucherNofill(): CommonServices.GenerateVoucherCode(26). */
    function nextCode(first) {
        if (first && S.L && S.L.nextCode != null) { $('#exnDocNo').val(S.L.nextCode); return; }
        var seq = ++S.codeSeq;
        $.getJSON(BASE + '/next-code').then(function (r) { if (seq === S.codeSeq && !(S.recId > 0)) $('#exnDocNo').val(r.voucherCode); });
    }

    /**
     * Reset(): the grid, entry boxes, cheque boxes, remarks, Tcy amount, cost-centre rows and summary are cleared, the
     * voucher number refreshed, combcreditac_Leave and DefaultConfigurations re-run. The project, date, location type
     * and credit account stay (btnNew clears the credit account text).
     */
    function reset(first, isNew) {
        S.rows = []; S.breaks = []; S.recId = 0; S.updIdx = -1; S.subLen = 0; S.debitAmtGet = 0; S.ack = { neg: false, dup: false };
        $('#exnComments, #exnAmount, #exnFcyDetail, #exnPayTitle, #exnRemarks, #exnChqText, #exnFcyAmount').val('');
        put('exnAccount', 0); bindSubsidiary(0); $('#exnDrLbl').text('').hide();
        put('exnChqNo', 0);
        if (first) {
            $('#exnDate').val(today()); $('#exnChqDate').val(today());
            firstRow('exnProject'); firstRow('exnLocation');
            if (S.F.defaultBranchId) setVal('exnBranch', S.F.defaultBranchId);
            if (S.F.isBookingOffice && ((S.L && S.L.costCenters) || []).length) firstRow('exnCostCenter');   // Rows[0].Activate() when AppId == 5
            chqMode();
        }
        if (isNew) put('exnCredit', 0);
        nextCode(first);
        showButtons('save');
        detailButtons(false);
        creditLeave();
        renderBreaks(); summary(); ccReset();
        renderGrid();
        $('#exnFcyAmount').val('');
        defaults();
        $('#exnDate').focus();
    }

    /** New_Click -> Reset() then the credit account text cleared. */
    function newClick() { reset(false, true); }

    /** ReadById(ID). */
    function loadForEdit(id) {
        return $.getJSON(BASE + '/' + id).then(function (v) {
            if (!v) return;
            switchTab('form');
            reset(false, false);
            S.recId = int(v.id); S.codeSeq++; S.updIdx = -1;
            $('#exnDate').val(day(v.voucherDate));
            put('exnCredit', v.refAccountId);
            S.creditType = int($('#exnCredit option:selected').attr('data-type')); layoutDetail(); renderGrid();
            $.getJSON(BASE + '/balance', { accountId: int(v.refAccountId), date: day(v.voucherDate) }).then(function (r) { $('#exnBal').val(bal(r && r.balance)); });
            $('#exnPayTitle').val(v.payTitle || '');
            var firstChq = (v.rows || []).length ? int(v.rows[0].chequeId) : 0;
            $.when(S.creditType === 15 ? cheqFill() : null).always(function () {
                if (chqOn()) {
                    var $o = $('#exnChqNo option').filter(function () { return $(this).text() === (v.chequeNo || ''); }).first();
                    if (!$o.length && v.chequeNo) { $('#exnChqNo').append('<option value="' + firstChq + '">' + esc(v.chequeNo) + '</option>'); $o = $('#exnChqNo option').last(); }
                    put('exnChqNo', $o.length ? $o.val() : 0);
                } else $('#exnChqText').val(v.chequeNo || '');
            });
            if (v.chequeDate) $('#exnChqDate').val(day(v.chequeDate));
            $('#exnRemarks').val(v.remarks || '');
            $('#exnDocNo').val(v.voucherCode);
            put('exnCurrency', v.multiCurrencyId);
            if (int(v.multiCurrencyId) > 0) put('exnTcyCode', v.multiCurrencyId);          // cmbCurrency_Leave sets the detail code; the stored rate follows
            $('#exnCustom').prop('checked', !!v.customAccounts);
            $('#exnRate').val(fmtRate(v.exchangeCurrencyRate));
            $('#exnFcyAmount').val(fmt3(v.fcAmount));
            if (int(v.multiCurrencyId) === 0 || num(v.exchangeCurrencyRate) === 0) defaults();
            put('exnLocation', v.locationTypeId);
            put('exnProject', v.projectId);
            S.rows = (v.rows || []).map(function (r) {
                return {
                    accountCode: r.accountCode || '', accountId: int(r.accountId), accountTitle: r.accountTitle || '',
                    glCurrencyId: int(r.glCurrencyId), glCurrency: r.glCurrency || '',
                    subsidiaryAccountId: int(r.subsidiaryAccountId), subsidiaryAccount: r.subsidiaryAccount || '', subsidiaryAccountTypeId: int(r.subsidiaryAccountTypeId),
                    jobLotId: int(r.jobLotId), jobLot: selTextOf('exnJobLot', r.jobLotId), remarks: r.remarks || '',
                    tcyCodeId: int(r.tcyCodeId), tcyCode: r.tcyCode || '', tcyExchangeRate: num(r.tcyExchangeRate), fcyAmount: num(r.fcyAmount), amount: num(r.amount),
                    referenceAccountId: int(r.referenceAccountId), referenceAccount: r.referenceAccount || '', lineId: int(r.lineId),
                    costCenterId: int(r.costCenterId), costCenterName: r.costCenterName || '', branchId: int(r.branchId), branchName: r.branchName || '',
                    chequeId: int(r.chequeId), chequeDate: day(r.chequeDate), chequeNo: r.chequeNo || '', payTitle: r.payTitle || ''
                };
            });
            S.breaks = (v.costCenters || []).map(function (c) {
                return { id: int(c.id), lineId: int(c.lineId), parentId: int(c.parentId), debitAccount: c.debitAccount || '', costCenterId: int(c.costCenterId),
                         costCenter: c.costCenter || '', percent: num(c.percent), amount: num(c.amount) };
            });
            renderGrid(); renderBreaks(); summary(); ccReset();
            showButtons('update');
            $('#exnDate').focus();
        }, function (x) { alert(errMsg(x, 'Voucher not found')); });
    }
    function selTextOf(id, val) { var o = $('#' + id + ' option[value="' + int(val) + '"]'); return o.length && int(val) > 0 ? o.text() : ''; }

    // ------------------------------------------------------------------------------ save

    function payload() {
        return {
            Id: S.recId, VoucherCode: int($('#exnDocNo').val()), VoucherDate: $('#exnDate').val(),
            ProjectId: int($('#exnProject').val()), LocationTypeId: int($('#exnLocation').val()), RefAccountId: int($('#exnCredit').val()),
            Remarks: $('#exnRemarks').val(), MultiCurrencyId: int($('#exnCurrency').val()), ExchangeCurrencyRate: num($('#exnRate').val()),
            FcAmount: num($('#exnFcyAmount').val()), CustomAccounts: $('#exnCustom').is(':checked'),
            NegativeBalanceAcknowledged: S.ack.neg, DuplicateAcknowledged: S.ack.dup,
            rows: S.rows.map(function (r) {
                return {
                    AccountId: r.accountId, SubsidiaryAccountId: r.subsidiaryAccountId || 0, SubsidiaryAccountTypeId: r.subsidiaryAccountTypeId || 0,
                    JobLotId: r.jobLotId, Remarks: r.remarks, TcyCodeId: r.tcyCodeId, TcyExchangeRate: num(r.tcyExchangeRate), FcyAmount: num(r.fcyAmount),
                    Amount: num(r.amount), ReferenceAccountId: r.referenceAccountId || 0, LineId: r.lineId, CostCenterId: r.costCenterId || 0, BranchId: r.branchId || 0,
                    ChequeId: r.chequeId || 0, ChequeDate: r.chequeDate || '', ChequeNo: r.chequeNo || '', PayTitle: r.payTitle || ''
                };
            }),
            costCenters: S.breaks.map(function (b) { return { Id: b.id || 0, LineId: b.lineId, CostCenterId: b.costCenterId, Percent: num(b.percent), Amount: num(b.amount) }; })
        };
    }

    /** Save_Click / Update_Click -> Insert(): the checks before the "Are you sure" prompt, then the server's own. */
    function save(kind) {
        if (S.busy) return;
        var $b = kind === 'update' ? $('#btnUpdate') : $('#btnSave');
        if (kind !== 'update') S.recId = 0;
        if (kind === 'update' && !(S.recId > 0)) { alert('Record Not Update  ' + S.recId); return; }
        if (!S.rows.length) { alert('Grid Record not found'); return; }
        if (int($('#exnLocation').val()) === 0) { alert('Location Type Field is Required'); comboFocus('exnLocation'); return; }
        if (int($('#exnProject').val()) === 0) { alert('Cost Center Field is Required'); comboFocus('exnProject'); return; }
        if (int($('#exnCredit').val()) === 0) { alert('Credit Account Field is Required'); comboFocus('exnCredit'); return; }
        if (int($('#exnCurrency').val()) === 0) { alert('Fcy Code Field is Required'); comboFocus('exnCurrency'); return; }
        if (num($('#exnRate').val()) === 0) { alert('Exchange Rate Field is Required'); $('#exnRate').focus(); return; }
        if (num($('#exnFcyAmount').val()) === 0) { alert('Fcy Amount Field is Required'); return; }
        if (S.F.multiCurrencyFeature) { alert('Multi Currency Grid is not available on the web yet. This voucher cannot be saved while the Multi Currency feature is on.'); return; }
        if (!confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < S.rows.length; i++) if (num(S.rows[i].amount) > 0 && int(S.rows[i].accountId) === 0) { alert('Please Select Account Title First'); return; }
        var p1 = $('#exnChkPrint').is(':checked'), p2 = $('#exnChkFmt2').is(':checked');
        var win = (p1 || p2) && w.CrystalPrint ? w.CrystalPrint.reserve() : null;
        S.busy = true; busyOn($b); S.ack = { neg: false, dup: false };
        (function post() {
            $.ajax({ url: BASE + '/save', type: 'POST', contentType: 'application/json', headers: csrf(), data: JSON.stringify(payload()) })
                .done(function (res) {
                    S.busy = false; busyOff($b);
                    (res.warnings || []).forEach(function (m) { alert(m); });
                    alert(res.message);
                    reset(false, false);
                    S.recId = 0;
                    if (p1) printDoc(1, res.id, null, null, win);
                    else if (p2) printDoc(2, res.id, DOC_TYPE, null, win);
                    else if (win && w.CrystalPrint) w.CrystalPrint.release(win);
                })
                .fail(function (x) {
                    var b = x.responseJSON || {};
                    if (x.status === 409 && (b.confirm === 'negativeBalance' || b.confirm === 'duplicate')) {
                        if (confirm(b.message)) { if (b.confirm === 'negativeBalance') S.ack.neg = true; else S.ack.dup = true; post(); return; }
                    } else alert(b.message || ('Save failed (' + x.status + ')'));
                    S.busy = false; busyOff($b);
                    if (win && w.CrystalPrint) w.CrystalPrint.release(win);
                });
        }());
    }

    // ------------------------------------------------------------------------------ print / shortcut keys

    /** kind 1 = CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(id) = hrm-102; kind 2 = AcRptPaymentReceiptsVoucherSlip_102(id, 26) = acc-102 with DocumentTypeId. */
    function printDoc(kind, id, docType, btn, win) {
        if (!(int(id) > 0)) { if (win && w.CrystalPrint) w.CrystalPrint.release(win); alert(kind === 2 ? 'No Record Selected' : 'VoucherId Not Found'); return; }
        if (!w.CrystalPrint) { if (win) win.close(); alert('Print is not available.'); return; }
        var args = { id: int(id) };
        if (kind === 2 && docType) args.documentTypeId = docType;
        w.CrystalPrint.open(kind === 1 ? 'hrm-102' : 'acc-102', args, btn, win);
    }
    function printButton(kind, btn) {
        if (!rights().print) { alert("You don't have right"); return; }
        printDoc(kind, S.recId, kind === 2 ? DOC_TYPE : null, btn, null);
    }

    /* MakeShortCutKeys() */
    var KEYS = [['Ctrl+S', 'For Save on form tab and for show data in history tab'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
        ['Ctrl+N', 'For New'], ['Alt+1', 'For Print I'], ['Alt+2', 'For Print II'], ['Ctrl+F5', 'For Focus on Voucher Date'], ['Ctrl+F10', 'For Open Attachments'],
        ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        if (!$('#exnKeys').length) {
            $('body').append('<div id="exnKeys" style="display:none;position:fixed;inset:0;z-index:3000;background:rgba(0,0,0,.3);">' +
                '<div style="position:absolute;left:50%;top:70px;transform:translateX(-50%);width:560px;max-width:95vw;background:#fff;border:1px solid #00796B;box-shadow:0 4px 16px rgba(0,0,0,.3);">' +
                '<div style="background:#00796B;color:#fff;padding:5px 8px;font-weight:bold;display:flex;justify-content:space-between;"><span>ShortCut Keys</span>' +
                '<a href="javascript:void(0)" id="exnKeysClose" style="color:#fff;">&#x2715;</a></div><div style="max-height:70vh;overflow:auto;">' +
                '<table class="win-grid-table"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody></tbody></table></div></div></div>');
            $('#exnKeysClose').on('click', function () { $('#exnKeys').hide(); });
            $('#exnKeys').on('click', function (e) { if (e.target === this) $(this).hide(); });
        }
        $('#exnKeys tbody').html(KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''));
        $('#exnKeys').show();
    }
    function attachments() { alert('Attachments are not available on the web form yet.'); }

    // ------------------------------------------------------------------------------ history tab (tabPage2)

    function H(n) { return $('#exnH' + n); }

    function hcols() {
        return [{ b: 'p1', t: 'Print', w: 40 }, { b: 'edit', t: 'Edit', w: 40 }, { k: 'documentType', t: 'V.Type' }, { k: 'voucherCode', t: 'V.No' },
                { k: 'voucherDate', t: 'V.Date' }, { k: 'accountTitle', t: 'AccountTitle' }, { k: 'remarks', t: 'Remarks', w: 350 },
                { k: 'voucherAmount', t: 'VoucherAmount', n: 1, sum: 1 }, { k: 'fcyAmount', t: 'FcyAmount', n: 3, sum: 3 },
                { k: 'entryUser', t: 'EntryUser' }, { k: 'entryDate', t: 'EntryDate' }, { k: 'modifyUser', t: 'ModifyUser' }, { k: 'modifyDate', t: 'ModifyDate' },
                { k: 'approvedUser', t: 'ApprovedUser' }, { k: 'approvedDate', t: 'ApprovedDate' }, { k: 'cheqNo', t: 'CheqNo' },
                { k: 'attachment', t: 'Attachment', link: 2 }, { b: 'att', t: 'Add Attachments', w: 110 }];
    }

    function renderHistory() {
        var h = '';
        /* toolStrip3: &Reset, Refresh (LoadAll is hidden) */
        h += '<div class="prv-ts"><div class="prv-ts-in" style="border-bottom-color:#c5cbd3;">' +
             '<button type="button" class="prv-ts-btn" id="exnHReset"><i class="fa fa-undo" style="color:#2e7d32"></i><span><u>R</u>eset</span></button>' +
             '<button type="button" class="prv-ts-btn" id="exnHRefresh"><i class="fa fa-refresh" style="color:#2e7d32"></i><span>Refresh</span></button></div></div>';
        h += '<div class="prv-teal" style="height:26px;"><span style="left:5px;top:3px;">Expense Voucher History</span></div>';
        /* panel16 (77 high): groupBox3 "Filters" (5,3) 917x72 and VoucherInfoBox (922,3) 242x72 */
        var filt =
            lab(4, 19, 'From Date') +
            '<div class="prv-dtp" style="' + px(70, 15, 135, 23) + '"><input type="checkbox" id="exnHFromChk" checked title="Use this date"/><input type="date" id="exnHFrom" class="prv-tb"/></div>' +
            lab(4, 47, 'To Date') +
            '<div class="prv-dtp" style="' + px(70, 43, 135, 23) + '"><input type="checkbox" id="exnHToChk" checked title="Use this date"/><input type="date" id="exnHTo" class="prv-tb"/></div>' +
            lab(213, 19, 'From Doc No') + tb('exnHFromNo', 294, 15, 100, 23, ' maxlength="9"') +
            lab(213, 47, 'To Doc No') + tb('exnHToNo', 294, 43, 100, 23, ' maxlength="9"') +
            lab(397, 19, 'Account Title') + cmb('exnHAccount', 493, 13, 234, 26) +
            lab(397, 47, 'Approved Status') +
            '<select id="exnHStatus" class="prv-cmb" data-dtcombo="single" style="position:absolute;' + px(493, 41, 158, 26) + '">' +
            '<option value="notapproved" selected>Not Apporved</option><option value="approved">Approved</option><option value="all">All</option></select>' +
            '<button type="button" class="prv-fbtn prv-show" id="exnHShow" style="' + px(656, 41, 71, 26) + '">Show</button>' +
            '<label class="prv-rb" style="' + px(732, 16) + '"><input type="radio" name="exnHDate" value="docdate" checked/>Doc Date</label>' +
            '<label class="prv-rb" style="' + px(732, 36) + '"><input type="radio" name="exnHDate" value="entrydate"/>Entry Date</label>' +
            '<label class="prv-rb" style="' + px(813, 16) + '"><input type="radio" name="exnHDate" value="modifydate"/>Modify Date</label>' +
            '<label class="prv-rb" style="' + px(813, 36) + '"><input type="radio" name="exnHDate" value="approveddate"/>Approved Date</label>';
        var info = '<div id="exnHInfo" class="prv-gb prv-info" style="display:none;' + px(922, 3, 242, 72) + '">' +
            lab(6, 11, 'Total Vouchers', 'prv-info') + '<span id="exnHTotal" class="prv-iv" style="' + px(153, 11, 80, 17) + '">0</span>' +
            lab(6, 28, 'Approved Vouchers', 'prv-info') + '<span id="exnHApproved" class="prv-iv" style="' + px(153, 28, 80, 17) + '">0</span>' +
            lab(6, 47, 'UnApproved Vouchers', 'prv-info') + '<span id="exnHUnApproved" class="prv-iv" style="' + px(153, 47, 80, 17) + '">0</span></div>';
        h += '<div class="prv-abs" style="height:77px;">' + gbox('Filters', 5, 3, 917, 72, filt) + info + '</div>';
        h += '<div class="prv-hgrid" style="height:248px;"><table class="prv-jg prv-auto" id="exnHGrid" tabindex="0"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        h += '<div class="prv-teal" style="height:25px;"><span style="left:4px;top:3px;font:bold 10pt Tahoma,sans-serif;">Detail of above selected row</span></div>';
        h += '<div class="prv-hdet"><table class="prv-jg prv-auto" id="exnHDetail"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        $('#exnHist').html(h);
        H('From').add(H('To')).val(today());
        H('Account').html(opts(S.L && S.L.historyAccounts, 'Id', 'AccountTitle', true));
        H('Grid').find('thead').html('<tr>' + hcols().map(function (c) {
            return '<th' + (c.n ? ' class="num"' : '') + (c.w ? ' style="min-width:' + c.w + 'px;"' : '') + '>' + c.t + '</th>';
        }).join('') + '</tr>');
        wireHistory();
    }

    /** btnNewHistory_Click ("&Reset"): dates today, doc numbers and account cleared, status row 0, info box hidden, both grids cleared. */
    function historyReset() {
        H('From').add(H('To')).val(today());
        H('FromNo').add(H('ToNo')).val('');
        put('exnHAccount', 0);
        H('Status').val('notapproved');
        H('Info').hide();
        S.hist.rows = []; S.hist.sel = -1;
        H('Grid').find('tbody, tfoot').empty();
        H('Detail').find('thead, tbody, tfoot').empty();
    }
    /** btnRefreshHistory_Click: ComboBindForHistory(). */
    function historyRefresh() {
        $.getJSON(BASE + '/lookups').then(function (L) {
            var keep = H('Account').val();
            S.L.historyAccounts = L.historyAccounts;
            H('Account').html(opts(L.historyAccounts, 'Id', 'AccountTitle', true)); put('exnHAccount', keep);
        }, function (x) { alert(errMsg(x)); });
    }

    function historyShow() {
        var hs = S.hist; if (hs.busy) return;
        var $b = H('Show');
        var params = {
            dateType: $('input[name="exnHDate"]:checked').val(),
            fromDate: H('FromChk').is(':checked') ? H('From').val() : '', toDate: H('ToChk').is(':checked') ? H('To').val() : '',
            fromDocNo: int(H('FromNo').val()) || '', toDocNo: int(H('ToNo').val()) || '', accountId: int(H('Account').val()) || '', approvedStatus: H('Status').val()
        };
        hs.busy = true; busyOn($b);
        $.getJSON(BASE + '/history', params).then(function (res) {
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
        $.getJSON(BASE + '/' + v.id + '/lines').then(function (r) { if (hs.sel === i) renderLines(r, int(v.accountTypeId)); });
    }

    /** VoucherDetailByHeaderId -> grdDetail: the cheque columns show for credit account type 15. */
    function renderLines(r, typeId) {
        var F = S.F, data = (r && r.rows) || [];
        var cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.multiCurrencyFeature && ['glCurrency', 'GlCurrency'],
                  F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'], ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['tcyCode', 'TcyCode'],
                  ['tcyExchangeRate', 'TcyExchangeRate', 3], ['tcyDebit', 'TcyDebit', 3], ['debitAmount', 'Debit', 1], ['tcyCredit', 'TcyCredit', 3], ['creditAmount', 'Credit', 1],
                  ['referenceAccount', 'ReferenceAccount'], F.branchFeature && ['branchName', 'BranchName'],
                  typeId === 15 && ['chequeDate', 'ChequeDate'], typeId === 15 && ['chequeNo', 'ChequeNo'], typeId === 15 && ['payTitle', 'PayTitle']].filter(Boolean);
        var sums = {};
        H('Detail').find('thead').html('<tr>' + cs.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + c[1] + '</th>'; }).join('') + '</tr>');
        H('Detail').find('tbody').html(data.map(function (x) {
            return '<tr>' + cs.map(function (c) {
                var v = x[c[0]];
                if (c[0] === 'chequeDate') v = day(v);
                if (c[2]) { if (c[0] !== 'tcyExchangeRate') sums[c[0]] = (sums[c[0]] || 0) + num(v); v = c[2] === 3 ? fmt3(v) : fmt(v); }
                return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join(''));
        H('Detail').find('tfoot').html('<tr>' + cs.map(function (c) {
            return '<td class="num" style="font-weight:bold;">' + (c[2] && c[0] !== 'tcyExchangeRate' ? (c[2] === 3 ? fmt3(sums[c[0]] || 0) : fmt(sums[c[0]] || 0)) : '') + '</td>';
        }).join('') + '</tr>');
    }

    /* DataGridHistory_ColumnButtonClick: Edit = Reset + ReadById; Print = ANewAcRptPaymentReceiptsVoucherSlip_102(id) */
    function historyButton(b, i, btn) {
        var v = S.hist.rows[i]; if (!v) return;
        if (b === 'edit') loadForEdit(v.id);
        else if (b === 'p1') printDoc(1, v.id, null, btn, null);
    }

    function wireHistory() {
        var hs = S.hist, $g = H('Grid');
        H('Reset').on('click', historyReset);
        H('Refresh').on('click', historyRefresh);
        H('Show').on('click', historyShow);
        H('FromNo').add(H('ToNo')).on('keydown', function (e) {
            if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length > 1) return;
            if (!/[0-9]/.test(e.key)) e.preventDefault();
        });
        $g.on('click', 'tbody tr[data-i]', function () { var i = int($(this).data('i')); if (i !== hs.sel) selectHistory(i); });
        $g.on('dblclick', 'tbody tr[data-i]', function () { var v = hs.rows[int($(this).data('i'))]; if (v) loadForEdit(v.id); });   // DataGridHistory_DoubleClick
        $g.on('click', 'a.prv-att', function (e) { e.stopPropagation(); attachments(); });
        $g.on('click', 'button.prv-hb', function (e) { e.stopPropagation(); historyButton($(this).data('b'), int($(this).data('i')), this); });
        $g.on('keydown', function (e) {
            var n = hs.rows.length; if (!n) return;
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                e.preventDefault();
                var i = hs.sel < 0 ? 0 : hs.sel + (e.key === 'ArrowDown' ? 1 : -1);
                if (i >= 0 && i < n) selectHistory(i);
            } else if (e.ctrlKey && e.key === 'Enter') {
                e.preventDefault(); e.stopPropagation();
                var v = hs.rows[hs.sel]; if (v) loadForEdit(v.id);
            }
        });
    }

    // ------------------------------------------------------------------------------ tabs / keys / wiring

    /** tab = 'form' | 'history' (tabControl1). */
    function switchTab(tab) {
        $('#tabFormContent, #tabHistContent').hide();
        $('#tabBtnForm, #tabBtnHist').removeClass('active');
        if (tab === 'form') { $('#tabFormContent').show(); $('#tabBtnForm').addClass('active'); }
        else { $('#tabHistContent').show(); $('#tabBtnHist').addClass('active'); H('From').focus(); }
    }
    function onHist() { return $('#tabHistContent').is(':visible'); }
    /** tabControl2: 'det' = Detail, 'cc' = Cost Center Breakup. */
    function innerTab(t) {
        $('#exnPaneDet').toggle(t === 'det'); $('#exnPaneCc').toggle(t === 'cc');
        $('#exnTabDet').toggleClass('active', t === 'det'); $('#exnTabCc').toggleClass('active', t === 'cc');
        if (t === 'det') renderGridHead();
    }

    function focusNext(from) {
        var $all = $('#tabFormContent, #tabHistContent').filter(':visible')
            .find('input,select,textarea,button').filter(':visible').filter(function () {
                return !this.disabled && (!this.readOnly || $(this).hasClass('dtcombo-input')) && this.type !== 'hidden' && this.tabIndex >= 0;
            });
        var i = $all.index(from);
        if (i >= 0 && i + 1 < $all.length) $all.eq(i + 1).focus();
    }

    /* ExpenseVoucher_KeyDown */
    function onKey(e) {
        if ($('#exnKeys').is(':visible')) return;
        var k = e.key || '', K = k.toUpperCase(), r = rights(), hd = onHist();
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey) {                  // Keys.Return -> SendKeys {TAB}
            var t = e.target;
            if (t && /^(INPUT|SELECT|TEXTAREA)$/.test(t.tagName) && t.type !== 'button' && t.type !== 'submit') {
                if ($(t).closest('#exnGrid').length) { e.preventDefault(); return; }
                if (t.tagName === 'TEXTAREA') return;
                e.preventDefault(); focusNext(t);
            }
            return;
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { shortcuts(); return; }
        if (k === 'Escape' && (!e.target || e.target === document.body)) { w.location.href = '/accounts/dashboard'; return; }   // Keys.Escape -> Close()
        if (e.altKey && !e.ctrlKey && (k === '1' || k === '2') && r.print) { e.preventDefault(); printButton(k === '1' ? 1 : 2, null); return; }
        if (e.ctrlKey && !e.altKey) {
            if (K === 'S') { e.preventDefault(); if (hd) historyShow(); else if ($('#btnSave').is(':visible') && !$('#btnSave').prop('disabled')) save('save'); }
            else if (K === 'U') { e.preventDefault(); if ($('#btnUpdate').is(':visible') && !$('#btnUpdate').prop('disabled')) save('update'); }
            else if (K === 'N') { e.preventDefault(); if (hd) historyReset(); else newClick(); }
            else if (K === 'E') { e.preventDefault(); w.location.href = '/accounts/dashboard'; }
            else if (K === 'T') { e.preventDefault(); if (hd) { switchTab('form'); $('#exnDate').focus(); } else switchTab('history'); }
            else if (K === 'R') { e.preventDefault(); if (hd) historyRefresh(); else refreshLists($('#btnRefresh')); }
            else if (k === 'F5') { e.preventDefault(); $('#exnDate').focus(); }
            else if (k === 'F10') { e.preventDefault(); attachments(); }
            else if (k === 'ArrowDown') { e.preventDefault(); (hd ? H('Grid') : $('#exnGrid')).focus(); }
            else if (k === 'ArrowUp') { e.preventDefault(); if (hd) H('From').focus(); else comboFocus('exnAccount'); }
        }
    }

    function wireForm() {
        $('#exnCredit').on('change', creditLeave);                       // combcreditac_Leave
        $('#exnAccount').on('change', accountLeave);                     // combactitle_Leave
        $('#exnSubsidiary').on('change', subBalance);                    // CmbSubsidiaryAccount_ValueChanged
        $('#exnCurrency').on('change', currencyLeave);                   // cmbCurrency_Leave
        $('#exnRate').on('keydown', decimalOnly).on('input', rateChangedHeader)
            .on('blur', function () { $(this).val(fmtRate($(this).val())); rateChangedHeader(); });   // txtExchangeRate_Leave
        $('#exnAmount').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); link('amt'); }).on('blur', ccAmounts);   // txtDebitAmount_Leave
        $('#exnFcyDetail').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); link('fcy'); });
        $('#exnTcyRate').on('keydown', decimalOnly);
        $('#exnAdd').on('click', add);
        $('#exnUpdateDetail').on('click', updateDetail);
        $('#exnCancelDetail').on('click', cancelDetail);
        $('#exnGrid').on('click', '.prv-del', function (ev) { ev.stopPropagation(); deleteRow(int($(this).data('i'))); });
        $('#exnGrid').on('click', 'tbody tr[data-i]', function () { $('#exnGrid tbody tr').removeClass('prv-sel'); $(this).addClass('prv-sel'); });
        $('#exnGrid').on('dblclick', 'tbody tr[data-i]', function () { editRow(int($(this).data('i'))); });
        $('#exnGrid').on('keydown', function (e) {                       // grd_KeyDown
            var sel = $('#exnGrid tbody tr.prv-sel'); if (!sel.length) sel = $('#exnGrid tbody tr[data-i]').first();
            if (!sel.length) return;
            var i = int(sel.data('i'));
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRow(i); }
            else if (e.ctrlKey && e.key === ' ') { e.preventDefault(); deleteRow(i); }
            else if (!e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowUp')) {
                var n = S.rows.length, j = i + (e.key === 'ArrowDown' ? 1 : -1);
                if (j >= 0 && j < n) { e.preventDefault(); $('#exnGrid tbody tr').removeClass('prv-sel').filter('[data-i="' + j + '"]').addClass('prv-sel'); }
            }
        });
        /* grdCostCenter: Select column, Prcnt and Amount edits */
        $('#exnCcGrid').on('change', '.exn-cp', function () { ccEdit(int($(this).data('i')), 'prcnt', $(this).val()); });
        $('#exnCcGrid').on('change', '.exn-ca', function () { ccEdit(int($(this).data('i')), 'amount', $(this).val()); });
        $('#exnCcGrid').on('change', '.exn-cck', function () { var c = S.cc[int($(this).data('i'))]; if (c) { c.checked = $(this).is(':checked'); ccSync(); } });
        $('#exnCcGrid').on('change', '#exnCcAll', function () { var on = $(this).is(':checked'); S.cc.forEach(function (c) { c.checked = on; }); ccSync(); });
        $('#exnTabDet').on('click', function () { innerTab('det'); });
        $('#exnTabCc').on('click', function () { innerTab('cc'); });
        $('#exnBtnHistory').on('click', function () { switchTab('history'); });
    }

    function wireToolbar() {
        $('#btnNew').on('click', newClick);
        $('#btnRefresh').on('click', function () { refreshLists($(this)); });
        $('#btnSave').on('click', function () { save('save'); });
        $('#btnUpdate').on('click', function () { save('update'); });
        $('#btnAttachment').on('click', attachments);
        $('#btnPrint').on('click', function () { printButton(1, this); });
        $('#btnPrint2').on('click', function () { printButton(2, this); });
        $('#btnShortcuts').on('click', shortcuts);
        $('#tabBtnForm').on('click', function () { switchTab('form'); });
        $('#tabBtnHist').on('click', function () { switchTab('history'); });
        $(document).on('keydown', onKey);
    }

    w.EXN = {
        init: function () {
            wireToolbar();
            var ready = $.getJSON(BASE + '/lookups').then(function (L) {
                bindLookups(L);
                renderHistory();
            }, function (x) { alert(errMsg(x, 'Could not load the form lists.')); });
            var id = int(new URLSearchParams(w.location.search).get('id'));
            ready.then(function () { if (id > 0) loadForEdit(id); });
            return ready;
        },
        loadForEdit: function (id) { return loadForEdit(id); },
        save: function () { save($('#btnUpdate').is(':visible') ? 'update' : 'save'); },
        reset: newClick,
        print: function () { printButton(1, $('#btnPrint')[0]); },
        switchTab: switchTab
    };
}(window, jQuery));
