/* ============================================================================================
 * Screen 914 JournalVoucher_New - "Journal Voucher (without Offset)".
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher_New (DocumentTypeId 17,
 * base.Name "JournalVoucher_New"). One grid row = one ledger line (Debit OR Credit, no offset line);
 * Insert() refuses when the two totals differ by 1.0 or more.
 *
 * The Form tab and the History tab are rendered here at the InitializeComponent coordinates (tab page 1208
 * wide, tabs at the bottom). Backend: /accounts/api/journal-no-offset (JournalNoOffsetVoucherController):
 * lookups from the desktop's own fill methods, Save / Update through the desktop procedure chain, ReadById
 * through Sp_Vouchers_GetMethods, history through USP_VoucherFormHistory. The page posts the grid ROWS and the
 * header controls; the ledger lines, cost-centre rows and VoucherAmount are built on the server from Insert().
 *
 * Layout numbers are the Location / Size values of InitializeComponent(); the 27 px toolstrip is in the
 * template, so designer y values here are measured from the bottom of the toolstrip.
 * ============================================================================================ */
(function (w, $) {
    'use strict';

    var BASE = '/accounts/api/journal-no-offset';
    var DOC_TYPE = 17;

    var S = {
        L: null, F: {}, rows: [], recId: 0, updIdx: -1, busy: false, subLen: 0, codeSeq: 0,
        amtSrc: '', fcySrc: '', remarksMouse: false, dupAck: [],
        hist: { rows: [], sel: -1, busy: false }
    };
    /* print-rpt.js reads the open voucher's id */
    try { Object.defineProperty(w, 'RecId', { get: function () { return S.recId; }, configurable: true });
          Object.defineProperty(w, 'VoucherHeadId', { get: function () { return S.recId; }, configurable: true }); } catch (e) { }

    if (w.DesktopCombo) {
        /* DebitAccountTitleFill: AccountTitle | AccountCode | ParentAccountTitle | AccountClass (Id hidden). */
        w.DesktopCombo.define('jnoAcc', [
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
    function fmt4(v) { return num(v).toLocaleString('en-US', { maximumFractionDigits: 4 }); }   // "#,##0.####"
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
                '" data-parent="' + esc(ci(a, 'ParentAccountTitle')) + '" data-cls="' + esc(ci(a, 'AccountClass')) + '">' + esc(ci(a, 'AccountTitle')) + '</option>';
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
    /* txtDebitBalance: Math.Round(balance).ToString(stringFormatboth); label Dr / Cr / Nill */
    function bal(v) { var b = Math.round(num(v)); return b < 0 ? '(' + Math.abs(b).toLocaleString('en-US') + ')' : b.toLocaleString('en-US'); }   // "#,#;(#,#);0"
    function drCr(v) { var b = num(v); return b > 0 ? 'Dr' : (b === 0 ? 'Nill' : 'Cr'); }

    function busyOn($b) {
        $b.each(function () {
            var b = $(this); if (b.data('jnoBusy')) return;
            b.data('jnoBusy', 1).data('jnoHtml', b.html()).prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> ' + $.trim(b.text()));
        });
    }
    function busyOff($b) {
        $b.each(function () {
            var b = $(this); if (!b.data('jnoBusy')) return;
            b.html(b.data('jnoHtml')).removeData('jnoBusy').prop('disabled', false);
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
    function pnl(id, inner) { return '<div class="prv-fp" id="' + id + '" style="position:absolute;left:0;top:0;height:47px;">' + inner + '</div>'; }

    // ------------------------------------------------------------------------------ Form tab markup

    /**
     * Dock Top panels of tabPage1: panel3 (teal, 28 high), panel4 (199 high, or 160 when none of Multi Currency /
     * Branch / Booking Office / Subsidiary is on - DayBookVoucher_Load), then panel5 (the grid, Fill).
     */
    function render() {
        var F = S.F, fcy = !!F.multiCurrencyFeature;
        var compact = !fcy && !F.branchFeature && !F.isBookingOffice && !F.subsidiaryFeature;
        var p4h = compact ? 160 : 199, gb2h = compact ? 75 : 119;

        /* ---- panel3: title, Location Type, Difference, and the Top|Right anchored check boxes */
        var strip = '<div class="prv-strip" style="height:28px;">' +
            '<span class="prv-strip-title" style="position:absolute;left:9px;top:4px;" id="jnoTitle">Journal Voucher</span>' +
            '<label class="prv-l" style="color:#fff;font:bold 9pt \'Segoe UI\',sans-serif;' + px(159, 8) + '">Location Type</label>' +
            cmb('jnoLocation', 248, 2, 212, 26) +
            '<label class="prv-l" style="color:#fff;font:9.75pt \'Microsoft Sans Serif\',sans-serif;' + px(526, 5) + '">Difference</label>' +
            tb('jnoDifference', 595, 4, 152, 20, ' readonly tabindex="-1"', 'prv-num') +
            '<div class="prv-ar" style="width:1204px;height:28px;">' +
            '<label style="' + px(986, 5, 120) + '"><input type="checkbox" id="jnoCustom"/>Custom Accounts</label>' +
            '<label style="' + px(1110, 5, 95) + '"><input type="checkbox" id="jnoChkPrint" checked/>Print Preview</label>' +
            '</div></div>';

        /* ---- groupBox1 "Main" (3,4) 1195x72 */
        var main =
            lab(6, 19, 'Cost Center') + cmb('jnoProject', 74, 14, 167, 24, '', 'prv-cmb-mss') +
            lab(6, 39, 'Doc No &amp; Date', null, null, 48, 32) + tb('jnoDocNo', 74, 40, 50, 23, ' readonly tabindex="-1"') + dtp('jnoDate', 125, 40, 116, 23) +
            lab(246, 17, 'Voucher Remarks', null, null, 56, 38) +
            '<textarea id="jnoRemarks" class="prv-tb" style="' + px(305, 17, fcy ? 437 : 846, 53) + '"></textarea>';
        if (fcy) {
            main += lab(745, 20, 'Fcy Code') + cmb('jnoCurrency', 832, 14, 111, 26) +
                lab(745, 46, 'Exchange Rate') + tb('jnoExchangeRate', 832, 42, 111, 23, '', 'prv-num') +
                lab(946, 19, 'Fcy Amount Dr') + tb('jnoFcyAmount', 1034, 15, 117, 23, ' readonly tabindex="-1"', 'prv-num prv-dis') +
                lab(946, 45, 'Fcy Amount Cr') + tb('jnoFcyAmountCr', 1034, 41, 117, 23, ' readonly tabindex="-1"', 'prv-num prv-dis');
        } else {
            /* the controls are hidden, not removed: Insert() and cmbCurrency_Leave still read their values */
            main += '<div style="display:none;">' + cmb('jnoCurrency', 832, 14, 111, 26) + tb('jnoExchangeRate', 832, 42, 111, 23, '', 'prv-num') +
                tb('jnoFcyAmount', 1034, 15, 117, 23, '', 'prv-num') + tb('jnoFcyAmountCr', 1034, 41, 117, 23, '', 'prv-num') + '</div>';
        }

        /* ---- groupBox2 "Detail" (3,77) 1195 x gb2h: flowLayoutPanel1 (3,19), panels laid out by layoutDetail() */
        var P = '';
        P += pnl('jnoPAcc', lab(3, 2, 'Account Title') +
            '<span id="jnoDrLbl" class="prv-bal" style="display:none;' + px(91, 2) + '"></span>' +
            '<input type="text" id="jnoDrBal" class="prv-tb prv-bal" readonly tabindex="-1" style="display:none;background:#fff;border:0;' + px(112, 3, 92, 16) + '"/>' +
            cmb('jnoAccount', 2, 21, 236, 26, ' data-dtcombo="jnoAcc" data-dtcombo-caption="Account Title"'));
        P += pnl('jnoPSub', lab(0, 3, 'Subsidiary') +
            '<span id="jnoSubLbl" class="prv-bal" style="display:none;' + px(75, 4) + '"></span>' +
            '<span id="jnoSubBal" class="prv-bal" style="display:none;' + px(92, 4) + '"></span>' +
            cmb('jnoSubsidiary', 0, 21, 168, 26));
        P += pnl('jnoPRem', lab(1, 4, 'Comments') + tb('jnoLineRemarks', 0, 22, 185, 23));
        P += pnl('jnoPJob', lab(1, 3, 'Job Lot ') + cmb('jnoJobLot', 1, 21, 134, 26));
        P += pnl('jnoPChq', lab(2, 6, 'Cheq / Ref') + tb('jnoCheq', 2, 23, 125, 23));
        P += pnl('jnoPFd', lab(1, 6, 'Fcy Debit') + tb('jnoFcyDebit', 1, 23, 110, 23, '', 'prv-num'));
        P += pnl('jnoPDr', lab(1, 6, 'Debit') + tb('jnoAmount', 1, 23, 110, 23, '', 'prv-num'));
        P += pnl('jnoPFc', lab(1, 6, 'Fcy Credit') + tb('jnoFcyCredit', 1, 23, 110, 23, '', 'prv-num'));
        P += pnl('jnoPCr', lab(1, 6, 'Credit') + tb('jnoCredit', 1, 23, 110, 23, '', 'prv-num'));
        P += pnl('jnoPBr', lab(5, 4, 'Branch') + cmb('jnoBranch', 2, 21, 143, 26));
        P += pnl('jnoPCc', lab(-1, 4, 'Cost Center') + cmb('jnoCostCenter', 0, 21, 123, 26));
        P += pnl('jnoPBtn',
            '<button type="button" id="jnoAdd" class="prv-fbtn" style="' + px(2, 21, 30, 24) + '">+</button>' +
            '<button type="button" id="jnoUpdateDetail" class="prv-fbtn" style="display:none;' + px(2, 20, 56, 27) + '">Update</button>' +
            '<button type="button" id="jnoCancelDetail" class="prv-fbtn" style="display:none;' + px(56, 20, 49, 27) + '">Clear</button>');
        var top = '<div class="prv-abs" id="jnoTop" style="height:' + (28 + p4h) + 'px;">' + strip +
            gbox('Main', 3, 32, 1195, 72, main) +
            gbox('Detail', 3, 105, 1195, gb2h, '<div class="prv-g" id="jnoFlow" style="' + px(3, 19) + '">' + P + '</div>', 'jnoDetailBox') +
            '</div>';

        /* ---- panel5: teal bar (30 high: "Detail", Update Job Lot button) and the grid */
        var bar = '<div class="prv-teal" style="height:30px;"><span style="left:7px;top:6px;">Detail</span>' +
            '<button type="button" id="jnoUpdateJobLot" class="prv-sysbtn" style="right:44px;top:3px;width:216px;height:24px;background:#004040;color:#fff;font:bold 8.25pt Verdana,sans-serif;">Update Job Lot to Detail Rows</button></div>';
        var grid = '<div class="prv-mid">' + bar + '<div class="prv-gridbox" style="right:0;top:30px;" id="jnoGridBox"><table class="prv-jg" id="jnoGrid" tabindex="0"><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
        /* History button: to the right of the footer, as on every ported voucher */
        var bot = '<div class="prv-bot prv-abs" style="height:30px;"><div class="prv-ar" style="width:1204px;height:30px;">' +
            '<button type="button" class="prv-sysbtn" id="jnoBtnHistory" style="left:1110px;top:4px;width:92px;height:22px;"><i class="fa fa-history"></i>History</button>' +
            '</div></div>';
        $('#prvForm').html(top + grid + bot);
        S.compact = compact;
    }

    /**
     * flowLayoutPanel1 (1186 wide): visible panels left to right in TabIndex order, wrapped at 1186.
     * Subsidiary / Fcy / Branch / Cost Center follow the Load-time features; without Subsidiary the comments
     * panel is 353 wide.
     */
    function layoutDetail() {
        var F = S.F, subs = !!F.subsidiaryFeature, fcy = !!F.multiCurrencyFeature, branch = !!F.branchFeature, booking = !!F.isBookingOffice;
        var remW = subs ? 185 : 353;
        var P = [['jnoPAcc', 238, true], ['jnoPSub', 168, subs], ['jnoPRem', remW, true], ['jnoPJob', 135, true], ['jnoPChq', 127, true],
                 ['jnoPFd', 113, fcy], ['jnoPDr', 112, true], ['jnoPFc', 113, fcy], ['jnoPCr', 113, true], ['jnoPBr', 147, branch],
                 ['jnoPCc', 123, booking], ['jnoPBtn', 106, true]];
        var x = 0, y = 0;
        P.forEach(function (p) {
            var $p = $('#' + p[0]);
            if (!p[2]) { $p.hide(); return; }
            if (x > 0 && x + p[1] > 1186) { x = 0; y += 47; }
            $p.css({ left: x + 'px', top: y + 'px', width: p[1] + 'px', display: 'block' });
            x += p[1];
        });
        $('#jnoLineRemarks').css('width', remW + 'px');
    }

    // ------------------------------------------------------------------------------ lookups

    var COMBOS = ['jnoProject', 'jnoLocation', 'jnoAccount', 'jnoSubsidiary', 'jnoJobLot', 'jnoCurrency', 'jnoBranch', 'jnoCostCenter'];
    function capture() { var o = {}; COMBOS.forEach(function (id) { o[id] = $('#' + id).val(); }); return o; }
    function restore(o, ids) { ids.forEach(function (id) { if (o[id] != null) put(id, o[id]); }); }

    function fillLists(L) {
        $('#jnoProject').html(opts(L.projects, 'Id', 'ProjectName', false));
        $('#jnoLocation').html(opts(L.locationTypes, 'Id', 'Location', true));        // ZeroIndex: true
        $('#jnoAccount').html(accOpts(L.accounts));                                   // DebitAccountTitleFill, ZeroIndex: true
        $('#jnoJobLot').html(opts(L.jobLots, 'Id', 'JobLotDescription', true));
        $('#jnoCurrency').html(opts(L.currencies, 'Id', 'CurrencyCode', true));
        $('#jnoBranch').html(opts(L.branches, 'BranchId', 'BranchName', false));
        $('#jnoCostCenter').html(opts(L.costCenters, 'Id', 'CostCenterName', false));
        bindSubsidiary(0);
    }

    function bindLookups(L) {
        S.L = L; S.F = L.flags || {};
        render();
        fillLists(L);
        layoutDetail();
        $('#btnPrint').prop('disabled', !rights().print);                             // Print.Enabled = DoHavePrintRights
        wireForm();
        reset(true);
    }

    /** DefaultConfigurations(): Job/Lot, Base Currency, BaseCurrencyRate (no Leave event is raised by Value =). */
    function defaults() {
        if (int(S.F.defaultJobLotId) > 0) put('jnoJobLot', S.F.defaultJobLotId);
        if (int(S.F.baseCurrencyId) > 0) put('jnoCurrency', S.F.baseCurrencyId);
        if (S.F.baseCurrencyRate != null) $('#jnoExchangeRate').val(fmtRate(S.F.baseCurrencyRate));
    }

    /** btnRefresh_Click: BranchesFill, combojoblotfill, DebitAccountTitleFill, BindAllSubsidiaryaccounts, DefaultConfigurations. */
    function refreshLists($b) {
        if (S.busy) return;
        busyOn($b);
        $.getJSON(BASE + '/lookups').then(function (L) {
            var keep = capture();
            S.L = L; S.F = L.flags || S.F;
            fillLists(L);
            restore(keep, ['jnoProject', 'jnoLocation', 'jnoAccount', 'jnoCurrency', 'jnoCostCenter']);
            if (S.F.defaultBranchId) setVal('jnoBranch', S.F.defaultBranchId);      // BranchesFill: Text = UserAccount.BranchName
            defaults();
            layoutDetail();
        }, function (x) { alert(errMsg(x, 'Could not refresh the lists.')); }).always(function () { busyOff($b); });
    }

    // ------------------------------------------------------------------------------ grid

    /** gridsetting(): visible columns, captions, order and widths. Delete is the last column added. */
    function cols() {
        var F = S.F;
        var c = [{ k: 'accountCode', t: 'AccountCode', w: 90 }, { k: 'accountTitle', t: 'AccountTitle', w: 260 }];
        if (F.subsidiaryFeature) c.push({ k: 'subsidiaryAccount', t: 'SubsidiaryAccount', w: 180 });
        c.push({ k: 'remarks', t: 'Remarks', ed: 'txt', w: 280 }, { k: 'jobLot', t: 'JobLot', w: 130 }, { k: 'cheqNo', t: 'CheqNo', ed: 'txt', w: 100 });
        if (F.multiCurrencyFeature) c.push({ k: 'fcyAmountDr', t: 'FcyAmountDr', n: 4, sum: 4, w: 110 });
        c.push({ k: 'amountDr', t: 'AmountDr', n: 4, sum: 1, ed: 'num', w: 110 });
        if (F.multiCurrencyFeature) c.push({ k: 'fcyAmountCr', t: 'FcyAmountCr', n: 4, sum: 5, w: 110 });
        c.push({ k: 'amountCr', t: 'AmountCr', n: 4, sum: 2, ed: 'num', w: 110 });
        if (F.branchFeature) c.push({ k: 'branchName', t: 'BranchName', w: 130 });
        if (F.isBookingOffice) c.push({ k: 'costCenter', t: 'CostCenter', w: 130 });
        c.push({ k: 'x', t: 'Delete', w: 50 });
        return c;
    }

    /* Janus GridEX auto-adjusts the columns over the grid width (CommonServices.GridAutoAdjustmentNew). */
    function renderGridHead() {
        var cs = cols(), sum = cs.reduce(function (s, c) { return s + (c.w || 100); }, 0);
        var boxW = $('#jnoGridBox').innerWidth() || 1200;
        var scale = sum > 0 ? Math.max(1, (boxW - 2) / sum) : 1;
        $('#jnoGrid').css('width', '100%');
        $('#jnoGrid colgroup').html(cs.map(function (c) { return '<col style="width:' + Math.round((c.w || 100) * scale) + 'px;"/>'; }).join(''));
        $('#jnoGrid thead').html('<tr>' + cs.map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + ' title="' + esc(c.t) + '">' + c.t + '</th>'; }).join('') + '</tr>');
    }

    function renderGrid() {
        renderGridHead();
        var cs = cols(), html = '';
        S.rows.forEach(function (r, i) {
            html += '<tr data-i="' + i + '">' + cs.map(function (c) {
                if (c.k === 'x') return '<td style="text-align:center;padding:0;"><button type="button" class="prv-cbtn prv-del" data-i="' + i + '">Delete</button></td>';
                if (c.ed === 'txt') return '<td><input type="text" class="prv-ce" data-i="' + i + '" data-k="' + c.k + '" value="' + esc(r[c.k]) + '"/></td>';
                if (c.ed === 'num') return '<td class="num"><input type="text" class="prv-ce num" data-i="' + i + '" data-k="' + c.k + '" value="' + esc(fmt4(r[c.k])) + '"/></td>';
                var v = r[c.k];
                if (c.n === 4) v = fmt4(v);
                return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $('#jnoGrid tbody').html(html || '<tr><td colspan="' + cs.length + '" class="prv-empty"></td></tr>');
        var t = totals();
        $('#jnoGrid tfoot').html(S.rows.length ? '<tr>' + cs.map(function (c) {
            var s = c.sum === 1 ? t.dr : c.sum === 2 ? t.cr : c.sum === 4 ? t.fdr : c.sum === 5 ? t.fcr : null;
            return '<td class="num" style="font-weight:bold;">' + (s == null ? '' : fmt4(s)) + '</td>';
        }).join('') + '</tr>' : '');
        calcTotals();
    }

    function totals() {
        var t = { dr: 0, cr: 0, fdr: 0, fcr: 0 };
        S.rows.forEach(function (r) { t.dr += num(r.amountDr); t.cr += num(r.amountCr); t.fdr += num(r.fcyAmountDr); t.fcr += num(r.fcyAmountCr); });
        return t;
    }

    /** CalculateTotalInformation(): Fcy Dr / Fcy Cr "#,##0.###", Difference = Dr - Cr "#,##0.####"; "0" with no rows. */
    function calcTotals() {
        if (S.rows.length) {
            var t = totals();
            $('#jnoFcyAmount').val(fmt3(t.fdr)); $('#jnoFcyAmountCr').val(fmt3(t.fcr)); $('#jnoDifference').val(fmt4(t.dr - t.cr));
        } else { $('#jnoFcyAmount').val('0'); $('#jnoDifference').val('0'); }
    }

    // ------------------------------------------------------------------------------ detail entry

    /** CmbDr_Leave: the balance of the selected account on the voucher date, then BindDebitSubsidiaryAccount / BindAllSubsidiaryaccounts. */
    function accountLeave() {
        var id = int($('#jnoAccount').val());
        if (id > 0 && $('#jnoAccount option:selected').text() !== '') {
            $.getJSON(BASE + '/balance', { accountId: id, date: $('#jnoDate').val() }).then(function (r) {
                var b = r && r.balance;
                $('#jnoDrBal').val(bal(b)).show(); $('#jnoDrLbl').text(drCr(Math.round(num(b)))).show();
            });
        } else { $('#jnoDrBal').val('0'); }
        bindSubsidiary(0);
    }

    /** BindDebitSubsidiaryAccount(): the subsidiaries whose parent GL account is the selected one (all of them with no account). */
    function bindSubsidiary(retainId) {
        var all = (S.L && S.L.subsidiaries) || [], list = all, acc = int($('#jnoAccount').val());
        var $s = $('#jnoSubsidiary');
        if (!all.length) { S.subLen = 0; $s.html('<option value="0"></option>'); return; }
        if ($('#jnoAccount option:selected').text() !== '' && acc > 0) {
            list = all.filter(function (r) { return int(ci(r, 'AccountId')) === acc; });
        } else if (acc !== 0) { list = []; }
        if (!list.length) { S.subLen = 0; $s.html('<option value="0"></option>'); return; }
        S.subLen = list.length;
        $s.html('<option value="0"></option>' + list.map(function (r) {
            return '<option value="' + esc(ci(r, 'Id')) + '" data-type="' + esc(ci(r, 'SubsidiaryTypeId')) + '" data-acc="' + esc(ci(r, 'AccountId')) +
                   '">' + esc(ci(r, 'SubsidiaryAccount')) + '</option>';
        }).join(''));
        put('jnoSubsidiary', retainId || 0);
    }

    /** CmbDrSubsidiaryAccount_ValueChanged: the subsidiary's own balance. */
    function subBalance() {
        var acc = int($('#jnoAccount').val()), sub = int($('#jnoSubsidiary').val());
        if (acc > 0 && sub > 0) {
            $.getJSON(BASE + '/subsidiary-balance', { accountId: acc, subsidiaryId: sub, date: $('#jnoDate').val() }).then(function (r) {
                var b = num(r && r.balance);
                $('#jnoSubBal').text(bal(b)).show(); $('#jnoSubLbl').text(drCr(b)).show();
            });
        } else { $('#jnoSubLbl, #jnoSubBal').hide(); $('#jnoSubBal').text('0'); }
    }

    /** CmbDrSubsidiaryAccount_AfterCloseUp: the subsidiary's parent GL account becomes the account, the subsidiary stays chosen. */
    function subsidiaryChosen() {
        var $o = $('#jnoSubsidiary option:selected'), sub = int($('#jnoSubsidiary').val());
        if (sub > 0) {
            var gl = int($o.attr('data-acc'));
            if (gl !== 0 && gl !== int($('#jnoAccount').val())) {
                put('jnoAccount', gl);
                bindSubsidiary(sub);
                $.getJSON(BASE + '/balance', { accountId: gl, date: $('#jnoDate').val() }).then(function (r) {
                    var b = r && r.balance;
                    $('#jnoDrBal').val(bal(b)).show(); $('#jnoDrLbl').text(drCr(Math.round(num(b)))).show();
                });
            }
        }
        subBalance();
    }

    /** FormValidation() - the same checks, order and wording for + and Update. */
    function detailValid() {
        var F = S.F;
        if (int($('#jnoAccount').val()) === 0) return ['Account Field is Required', 'jnoAccount'];
        if (F.subsidiaryFeature && S.subLen > 0 && int($('#jnoSubsidiary').val()) === 0) return ['Subsidiary Account Field Required', 'jnoSubsidiary'];
        if ($.trim($('#jnoLineRemarks').val()) === '') return ['Remarks Field is Required', 'jnoLineRemarks'];
        if (int($('#jnoJobLot').val()) === 0) return ['JobLot Field is Required', 'jnoJobLot'];
        if (num($('#jnoAmount').val()) === 0 && num($('#jnoCredit').val()) === 0) return ['Debit and Credit cannot be equal to zero....!', 'jnoAmount'];
        if (F.branchFeature && int($('#jnoBranch').val()) === 0) return ['Branch Field is Required', 'jnoBranch'];
        return null;
    }
    function fail(e) { alert(e[0]); if (/^jno(Account|Subsidiary|JobLot|Branch|CostCenter)$/.test(e[1])) comboFocus(e[1]); else $('#' + e[1]).focus(); }

    function rowFromEntry() {
        var accOpt = $('#jnoAccount option:selected'), sub = int($('#jnoSubsidiary').val()), subOpt = $('#jnoSubsidiary option:selected');
        var cc = int($('#jnoCostCenter').val());
        var r = {
            accountCode: accOpt.attr('data-code') || '', accountId: int($('#jnoAccount').val()), accountTitle: accOpt.attr('data-title') || accOpt.text(),
            subsidiaryAccountId: 0, subsidiaryAccount: '', subsidiaryAccountTypeId: 0,
            remarks: $.trim($('#jnoLineRemarks').val()), jobLotId: int($('#jnoJobLot').val()), jobLot: selText('jnoJobLot'), cheqNo: $.trim($('#jnoCheq').val()),
            fcyAmountDr: num($('#jnoFcyDebit').val()), amountDr: num($('#jnoAmount').val()),
            fcyAmountCr: num($('#jnoFcyCredit').val()), amountCr: num($('#jnoCredit').val()),
            branchId: S.F.branchFeature ? int($('#jnoBranch').val()) : 0, branchName: S.F.branchFeature ? selText('jnoBranch') : '',
            costCenterId: cc > 0 ? cc : 0, costCenter: cc > 0 ? selText('jnoCostCenter') : ''
        };
        if (S.F.subsidiaryFeature) {
            if (S.subLen > 0 && sub > 0) { r.subsidiaryAccountId = sub; r.subsidiaryAccount = subOpt.text(); r.subsidiaryAccountTypeId = int(subOpt.attr('data-type')); }
            else { r.subsidiaryAccountId = r.accountId; r.subsidiaryAccount = r.accountTitle; r.subsidiaryAccountTypeId = 4; }
        }
        return r;
    }

    /** btnplus_Click: after the add the remarks / amounts / cheq are cleared and the account keeps focus. */
    function add() {
        var e = detailValid();
        if (e) { fail(e); return; }
        S.rows.push(rowFromEntry());
        $('#jnoLineRemarks, #jnoFcyDebit, #jnoAmount, #jnoFcyCredit, #jnoCredit, #jnoCheq').val('');
        renderGrid();
        comboFocus('jnoAccount');
    }

    function clearEntry() {
        $('#jnoLineRemarks, #jnoAmount, #jnoCheq').val('');
        $('#jnoDrLbl').text('');
    }
    function detailButtons(upd) {
        $('#jnoAdd').toggle(!upd); $('#jnoUpdateDetail, #jnoCancelDetail').toggle(upd);
    }

    /** btnUpdateDetail_Click. */
    function updateDetail() {
        var e = detailValid();
        if (e) { fail(e); return; }
        if (S.updIdx > -1) { S.rows[S.updIdx] = rowFromEntry(); S.updIdx = -1; }
        put('jnoAccount', 0); accountLeave();
        clearEntry();
        detailButtons(false);
        comboFocus('jnoAccount');
        rateChangedHeader();                 // txtExchangeRate_TextChanged(null, null)
    }

    /** btnCancelDetail_Click ("Clear"). */
    function cancelDetail() {
        detailButtons(false);
        put('jnoAccount', 0); accountLeave();
        clearEntry();
    }

    /** grd_DoubleClick (and Ctrl+Enter on the grid): the row goes back into the entry box. */
    function editRow(i) {
        var r = S.rows[i]; if (!r) return;
        detailButtons(true);
        S.updIdx = i;
        put('jnoAccount', r.accountId);
        accountLeave();
        bindSubsidiary(0);
        if (S.subLen === 0 && S.F.subsidiaryFeature) {
            $('#jnoSubsidiary').html('<option value="0"></option><option value="' + esc(r.subsidiaryAccountId) + '" data-type="4" data-acc="' + esc(r.accountId) + '">' + esc(r.subsidiaryAccount) + '</option>');
        }
        if (int(r.subsidiaryAccountId) > 0) put('jnoSubsidiary', r.subsidiaryAccountId);
        $('#jnoCheq').val(r.cheqNo || '');
        $('#jnoLineRemarks').val(r.remarks || '');
        if (int(r.jobLotId) > 0) put('jnoJobLot', r.jobLotId);
        if (int(r.costCenterId) > 0) put('jnoCostCenter', r.costCenterId);
        $('#jnoAmount').val(fmt4(r.amountDr)); $('#jnoCredit').val(fmt4(r.amountCr));
        if (S.F.branchFeature) put('jnoBranch', r.branchId);
        comboFocus('jnoAccount');
    }

    /** grd_ColumnButtonClick "Delete" (and Ctrl+Space on that column). */
    function deleteRow(i) {
        if (!S.rows[i]) return;
        S.rows.splice(i, 1);
        renderGrid();
    }

    /** btnUpdateJoblot_Click: the entry box's job lot goes onto every detail row. */
    function updateJobLot() {
        var id = int($('#jnoJobLot').val());
        if (!id) { alert('Please Select Job Lot to update in Detail Rows'); return; }
        if (!S.rows.length) { alert('No Detail Rows Found'); return; }
        var t = selText('jnoJobLot');
        S.rows.forEach(function (r) { r.jobLotId = id; r.jobLot = t; });
        renderGrid();
    }

    /** btnGenerateRemarks_Click: fills the empty row remarks from the voucher remarks. */
    function generateRemarks() {
        var t = totals(), vr = $('#jnoRemarks').val() || '';
        if (!(t.dr > 0) || !(t.cr > 0) || vr === '') return;
        var drRows = S.rows.filter(function (r) { return num(r.amountDr) > 0; }), crRows = S.rows.filter(function (r) { return num(r.amountCr) > 0; });
        S.rows.forEach(function (r) {
            if ($.trim(r.remarks) !== '') return;
            if (crRows.length === 1) r.remarks = r.accountTitle + ' ' + $.trim(vr) + ' ' + crRows[0].accountTitle;
            else if (drRows.length === 1) r.remarks = r.accountTitle + ' ' + $.trim(vr) + ' ' + drRows[0].accountTitle;
            else r.remarks = $.trim(vr);
        });
        renderGrid();
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
     * txtAmount / txtCredit / txtFcyDebit / txtFcyCredit _TextChanged: a positive Debit zeroes the Credit and the
     * other way round (same for the Fcy pair); UpdateRelatedFieldsCalculation links an amount to its Fcy amount
     * with the header exchange rate (Fcy = amount / rate, amount = Fcy * rate).
     */
    function link(which) {
        var rate = num($('#jnoExchangeRate').val());
        var debitSide = which === 'dr' || which === 'fdr';
        var amt = debitSide ? '#jnoAmount' : '#jnoCredit', fcy = debitSide ? '#jnoFcyDebit' : '#jnoFcyCredit';
        var other = which === 'dr' ? '#jnoCredit' : which === 'cr' ? '#jnoAmount' : which === 'fdr' ? '#jnoFcyCredit' : '#jnoFcyDebit';
        var src = which === 'dr' || which === 'cr' ? amt : fcy;
        if (num($(src).val()) > 0) $(other).val('0');
        if ($.trim($(src).val()) === '') return;
        if (rate <= 0) { $(amt).val('0'); $(fcy).val('0'); return; }
        if (src === amt) { var a = num($(amt).val()); $(fcy).val(a > 0 ? String(round(a / rate, dec())) : '0'); }
        else { var f = num($(fcy).val()); $(amt).val(f > 0 ? String(round(f * rate, dec())) : '0'); }
    }

    /** txtExchangeRate_TextChanged: every row's Fcy = amount / rate (0 with no rate), then CalculateTotalInformation(). */
    function rateChangedHeader() {
        var rate = num($('#jnoExchangeRate').val());
        S.rows.forEach(function (r) {
            r.fcyAmountDr = rate > 0 ? num(r.amountDr) / rate : 0;
            r.fcyAmountCr = rate > 0 ? num(r.amountCr) / rate : 0;
        });
        renderGrid();
    }

    /** cmbCurrency_Leave: non-base currencies take the last rate used on a type-17 voucher, the base currency its configured rate. */
    function currencyLeave() {
        var cur = int($('#jnoCurrency').val());
        if (cur === 0) return;
        if (cur !== int(S.F.baseCurrencyId)) {
            $.getJSON(BASE + '/last-rate', { currencyId: cur }).then(function (r) {
                var lr = num(r && r.lastRate);
                $('#jnoExchangeRate').val(lr ? fmtRate(lr) : '0');
                rateChangedHeader();
            }, function (x) { alert(errMsg(x)); });
        } else {
            $('#jnoExchangeRate').val(fmtRate(S.F.baseCurrencyRate));
            rateChangedHeader();
        }
    }

    // ------------------------------------------------------------------------------ reset / load

    function showButtons(m) {
        var r = rights();
        $('#btnSave').toggle(m === 'save').prop('disabled', !r.save);
        $('#btnUpdate').toggle(m === 'update').prop('disabled', !r.update);
    }

    /** VoucherCode(): CommonServices.GenerateVoucherCode(17). */
    function nextCode(first) {
        if (first && S.L && S.L.nextCode != null) { $('#jnoDocNo').val(S.L.nextCode); return; }
        var seq = ++S.codeSeq;
        $.getJSON(BASE + '/next-code').then(function (r) { if (seq === S.codeSeq && !(S.recId > 0)) $('#jnoDocNo').val(r.voucherCode); });
    }

    /**
     * Reset(): the grid, the entry boxes, the remarks and the Fcy totals are cleared; the project, date, location type,
     * currency and rate are NOT touched (only Load sets them).
     */
    function reset(first) {
        S.rows = []; S.recId = 0; S.updIdx = -1; S.subLen = 0; S.dupAck = [];
        put('jnoAccount', 0); bindSubsidiary(0);
        $('#jnoRemarks').val('');
        $('#jnoDrLbl').text(''); $('#jnoDrBal').val('');
        $('#jnoCredit, #jnoFcyAmountCr, #jnoFcyAmount, #jnoLineRemarks, #jnoAmount, #jnoCheq, #jnoFcyDebit, #jnoFcyCredit').val('');
        if (first) {
            $('#jnoDate').val(today());
            firstRow('jnoProject'); firstRow('jnoLocation');                      // Rows[0] / Rows[1] activated
            if (S.F.defaultBranchId) setVal('jnoBranch', S.F.defaultBranchId);
            defaults();
        }
        nextCode(first);
        showButtons('save');
        detailButtons(false);
        renderGrid();
        $('#jnoDate').focus();
    }

    /** New_Click -> Reset(). */
    function newClick() { reset(false); }

    /** ReadById(ID). */
    function loadForEdit(id) {
        return $.getJSON(BASE + '/' + id).then(function (v) {
            if (!v) return;
            switchTab('form');
            reset(false);
            S.recId = int(v.id); S.codeSeq++; S.updIdx = -1;
            put('jnoProject', v.projectId);
            $('#jnoDate').val(day(v.voucherDate));
            $('#jnoCheq').val(v.chequeNo || '');
            $('#jnoDocNo').val(v.voucherCode);
            $('#jnoRemarks').val(v.remarks || '');
            put('jnoCurrency', v.multiCurrencyId);
            $('#jnoExchangeRate').val(fmtRate(v.exchangeCurrencyRate));
            $('#jnoFcyAmount').val(fmt3(v.fcAmount));
            $('#jnoCustom').prop('checked', !!v.customAccounts);
            put('jnoLocation', v.locationTypeId);
            if (int(v.multiCurrencyId) === 0 || num(v.exchangeCurrencyRate) === 0) defaults();
            S.rows = (v.rows || []).map(function (r) {
                return {
                    accountCode: r.accountCode || '', accountId: int(r.accountId), accountTitle: r.accountTitle || '',
                    subsidiaryAccountId: int(r.subsidiaryAccountId), subsidiaryAccount: r.subsidiaryAccount || '', subsidiaryAccountTypeId: int(r.subsidiaryAccountTypeId),
                    remarks: r.remarks || '', jobLotId: int(r.jobLotId), jobLot: r.jobLot || '', cheqNo: r.cheqNo || '',
                    fcyAmountDr: num(r.fcyAmountDr), amountDr: num(r.amountDr), fcyAmountCr: num(r.fcyAmountCr), amountCr: num(r.amountCr),
                    branchId: int(r.branchId), branchName: r.branchName || '', costCenterId: int(r.costCenterId), costCenter: r.costCenter || ''
                };
            });
            renderGrid();
            showButtons('update');
            $('#jnoDate').focus();
        }, function (x) { alert(errMsg(x, 'Voucher not found')); });
    }

    // ------------------------------------------------------------------------------ save

    function payload() {
        return {
            Id: S.recId, VoucherCode: int($('#jnoDocNo').val()), VoucherDate: $('#jnoDate').val(),
            ProjectId: int($('#jnoProject').val()), LocationTypeId: int($('#jnoLocation').val()),
            Remarks: $('#jnoRemarks').val(), ChequeNo: $('#jnoCheq').val(),
            MultiCurrencyId: int($('#jnoCurrency').val()), ExchangeCurrencyRate: num($('#jnoExchangeRate').val()),
            FcAmount: num($('#jnoFcyAmount').val()), CustomAccounts: $('#jnoCustom').is(':checked'),
            duplicateAcknowledgedAccounts: S.dupAck.slice(),
            rows: S.rows.map(function (r) {
                return {
                    AccountId: r.accountId, SubsidiaryAccountId: r.subsidiaryAccountId || 0, SubsidiaryAccountTypeId: r.subsidiaryAccountTypeId || 0,
                    Remarks: r.remarks, JobLotId: r.jobLotId, CheqNo: r.cheqNo || '', FcyAmountDr: num(r.fcyAmountDr), AmountDr: num(r.amountDr),
                    FcyAmountCr: num(r.fcyAmountCr), AmountCr: num(r.amountCr), BranchId: r.branchId || 0, CostCenterId: r.costCenterId || 0
                };
            })
        };
    }

    /** Save_Click / btnUpdate_Click -> Insert(): the checks before the "Are you sure" prompt, then the server's own. */
    function save(kind) {
        if (S.busy) return;
        var $b = kind === 'update' ? $('#btnUpdate') : $('#btnSave');
        if (kind !== 'update') S.recId = 0;
        if (kind === 'update' && !(S.recId > 0)) { alert('Record Not Update  ' + S.recId); return; }
        if (int($('#jnoProject').val()) === 0) { alert('Cost Center Field is Required'); comboFocus('jnoProject'); return; }
        if (!S.rows.length) { alert('Grid Fields Required'); return; }
        if (S.F.multiCurrencyFeature) {
            if (int($('#jnoCurrency').val()) === 0) { alert('Fcy Code Field is Required'); comboFocus('jnoCurrency'); return; }
            if (num($('#jnoExchangeRate').val()) === 0) { alert('Exchange Rate Field is Required'); $('#jnoExchangeRate').focus(); return; }
            if (num($('#jnoFcyAmount').val()) === 0) { alert('Fcy Amount Field is Required'); return; }
        } else {
            if (int($('#jnoCurrency').val()) === 0) { alert('Please Configure Your Base Currency In configurations'); return; }
            if ($.trim($('#jnoExchangeRate').val()) === '' || $.trim($('#jnoExchangeRate').val()) === '0') { alert('Please Configure Your Base Currency Rate In configurations'); return; }
        }
        if (!confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < S.rows.length; i++) if (num(S.rows[i].amountDr) > 0 && int(S.rows[i].accountId) === 0) { alert('Please Select Account Title First in Row#' + (i + 1)); return; }
        for (var j = 0; j < S.rows.length; j++) if (num(S.rows[j].amountCr) > 0 && int(S.rows[j].accountId) === 0) { alert('Please Select Account Title First in Row#' + (j + 1)); return; }
        var t = totals();
        if (Math.abs(t.dr - t.cr) >= 1) { alert('Debit & Credit side not equal'); return; }
        var print = $('#jnoChkPrint').is(':checked');
        var win = print && w.CrystalPrint ? w.CrystalPrint.reserve() : null;
        S.busy = true; busyOn($b); S.dupAck = [];
        (function post() {
            $.ajax({ url: BASE + '/save', type: 'POST', contentType: 'application/json', headers: csrf(), data: JSON.stringify(payload()) })
                .done(function (res) {
                    S.busy = false; busyOff($b);
                    alert(res.message);
                    reset(false);
                    if (print) printDoc(res.id, null, win);
                })
                .fail(function (x) {
                    var b = x.responseJSON || {};
                    if (x.status === 409 && b.confirm === 'duplicate') {
                        if (confirm(b.message)) { S.dupAck.push(int(b.accountId)); post(); return; }
                    } else alert(b.message || ('Save failed (' + x.status + ')'));
                    S.busy = false; busyOff($b);
                    if (win && w.CrystalPrint) w.CrystalPrint.release(win);
                });
        }());
    }

    // ------------------------------------------------------------------------------ print / shortcut keys

    /** CommonServices.AcRptPaymentReceiptsVoucherSlip_102(id, 17) = the acc-102 slip with DocumentTypeId 17. */
    function printDoc(id, btn, win) {
        if (!(int(id) > 0)) { if (win && w.CrystalPrint) w.CrystalPrint.release(win); alert('No Data Found For Display'); return; }
        if (!w.CrystalPrint) { if (win) win.close(); alert('Print is not available.'); return; }
        w.CrystalPrint.open('acc-102', { id: int(id), documentTypeId: DOC_TYPE }, btn, win);
    }
    function printButton(btn) {
        if (!rights().print) { alert("You don't have right"); return; }
        printDoc(S.recId, btn, null);
    }

    /* MakeShortCutKeys() */
    var KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        if (!$('#jnoKeys').length) {
            $('body').append('<div id="jnoKeys" style="display:none;position:fixed;inset:0;z-index:3000;background:rgba(0,0,0,.3);">' +
                '<div style="position:absolute;left:50%;top:70px;transform:translateX(-50%);width:520px;max-width:95vw;background:#fff;border:1px solid #00796B;box-shadow:0 4px 16px rgba(0,0,0,.3);">' +
                '<div style="background:#00796B;color:#fff;padding:5px 8px;font-weight:bold;display:flex;justify-content:space-between;"><span>ShortCut Keys</span>' +
                '<a href="javascript:void(0)" id="jnoKeysClose" style="color:#fff;">&#x2715;</a></div><div style="max-height:70vh;overflow:auto;">' +
                '<table class="win-grid-table"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody></tbody></table></div></div></div>');
            $('#jnoKeysClose').on('click', function () { $('#jnoKeys').hide(); });
            $('#jnoKeys').on('click', function (e) { if (e.target === this) $(this).hide(); });
        }
        $('#jnoKeys tbody').html(KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''));
        $('#jnoKeys').show();
    }
    function attachments() { alert('Attachments are not available on the web form yet.'); }
    /** btnLoadTrialBalance_Click opens the desktop TrialBalance dialog and copies its closing balances into the grid. */
    function loadTrialBalance() { alert('Load Trial Balance needs the desktop Trial Balance dialog and is not available on the web yet.'); }

    // ------------------------------------------------------------------------------ history tab (tabPage2)

    function H(n) { return $('#jnoH' + n); }

    function hcols() {
        var c = [{ b: 'p1', t: 'Print', w: 40 }, { b: 'edit', t: 'Edit', w: 40 }, { k: 'documentType', t: 'V.Type' }, { k: 'voucherCode', t: 'V.No' },
                 { k: 'voucherDate', t: 'V.Date' }, { k: 'voucherAmount', t: 'VoucherAmount', n: 1, sum: 1 }];
        if (S.F.multiCurrencyFeature) c.push({ k: 'fcyCode', t: 'FcyCode' }, { k: 'exchangeRate', t: 'ExchangeRate', n: 2 }, { k: 'fcyAmount', t: 'FcyAmount', n: 3, sum: 3 });
        c.push({ k: 'entryUser', t: 'EntryUser' }, { k: 'entryDate', t: 'EntryDate' }, { k: 'modifyUser', t: 'ModifyUser' }, { k: 'modifyDate', t: 'ModifyDate' },
               { k: 'approvedUser', t: 'ApprovedUser' }, { k: 'approvedDate', t: 'ApprovedDate' }, { k: 'attachment', t: 'Attachment', link: 2 },
               { b: 'att', t: 'Add Attachments', w: 110 }, { b: 'del', t: 'X', w: 20 });
        return c;
    }

    function renderHistory() {
        var h = '';
        /* toolStrip3: &Reset */
        h += '<div class="prv-ts"><div class="prv-ts-in" style="border-bottom-color:#c5cbd3;">' +
             '<button type="button" class="prv-ts-btn" id="jnoHReset"><i class="fa fa-undo" style="color:#2e7d32"></i><span><u>R</u>eset</span></button></div></div>';
        h += '<div class="prv-teal" style="height:28px;margin-top:-3px;"><span style="left:5px;top:4px;">Journal Voucher History</span></div>';
        /* panel7 (78 high): groupBox3 "Filters" (5,3) 926x71 and VoucherInfoBox (933,5) 242x69 */
        var filt =
            lab(5, 23, 'From Date') +
            '<div class="prv-dtp" style="' + px(5, 41, 135, 23) + '"><input type="checkbox" id="jnoHFromChk" checked title="Use this date"/><input type="date" id="jnoHFrom" class="prv-tb"/></div>' +
            lab(146, 22, 'To Date') +
            '<div class="prv-dtp" style="' + px(146, 41, 134, 23) + '"><input type="checkbox" id="jnoHToChk" checked title="Use this date"/><input type="date" id="jnoHTo" class="prv-tb"/></div>' +
            lab(288, 22, 'From Doc No') + tb('jnoHFromNo', 288, 41, 100, 23, ' maxlength="9"') +
            lab(393, 22, 'To Doc No') + tb('jnoHToNo', 393, 41, 100, 23, ' maxlength="9"') +
            lab(501, 22, 'Approved Status') +
            '<select id="jnoHStatus" class="prv-cmb" data-dtcombo="single" style="position:absolute;' + px(499, 39, 170, 26) + '">' +
            '<option value="notapproved" selected>Not Approved</option><option value="approved">Approved</option><option value="all">All</option></select>' +
            '<button type="button" class="prv-fbtn prv-show" id="jnoHShow" style="' + px(673, 39, 53, 26) + '">Show</button>' +
            '<label class="prv-rb" style="' + px(735, 28) + '"><input type="radio" name="jnoHDate" value="docdate" checked/>Doc Date</label>' +
            '<label class="prv-rb" style="' + px(735, 48) + '"><input type="radio" name="jnoHDate" value="entrydate"/>Entry Date</label>' +
            '<label class="prv-rb" style="' + px(816, 28) + '"><input type="radio" name="jnoHDate" value="modifydate"/>Modify Date</label>' +
            '<label class="prv-rb" style="' + px(816, 48) + '"><input type="radio" name="jnoHDate" value="approveddate"/>Approved Date</label>';
        var info = '<div id="jnoHInfo" class="prv-gb prv-info" style="display:none;' + px(933, 5, 242, 69) + '">' +
            lab(6, 11, 'Total Vouchers', 'prv-info') + '<span id="jnoHTotal" class="prv-iv" style="' + px(153, 11, 80, 17) + '">0</span>' +
            lab(6, 28, 'Approved Vouchers', 'prv-info') + '<span id="jnoHApproved" class="prv-iv" style="' + px(153, 28, 80, 17) + '">0</span>' +
            lab(6, 47, 'UnApproved Vouchers', 'prv-info') + '<span id="jnoHUnApproved" class="prv-iv" style="' + px(153, 47, 80, 17) + '">0</span></div>';
        h += '<div class="prv-abs" style="height:78px;">' + gbox('Filters', 5, 3, 926, 71, filt) + info + '</div>';
        h += '<div class="prv-hgrid" style="height:244px;"><table class="prv-jg prv-auto" id="jnoHGrid" tabindex="0"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        h += '<div class="prv-teal" style="height:26px;"><span style="left:6px;top:3px;">Detail of above selected row</span></div>';
        h += '<div class="prv-hdet"><table class="prv-jg prv-auto" id="jnoHDetail"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        $('#jnoHist').html(h);
        H('From').add(H('To')).val(today());
        H('Grid').find('thead').html('<tr>' + hcols().map(function (c) {
            return '<th' + (c.n ? ' class="num"' : '') + (c.w ? ' style="min-width:' + c.w + 'px;"' : '') + '>' + c.t + '</th>';
        }).join('') + '</tr>');
        wireHistory();
    }

    /** btnNewHistory_Click ("&Reset"): dates today, doc numbers cleared, status row 0, info box hidden, both grids cleared. */
    function historyReset() {
        H('From').add(H('To')).val(today());
        H('FromNo').add(H('ToNo')).val('');
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
            dateType: $('input[name="jnoHDate"]:checked').val(),
            fromDate: H('FromChk').is(':checked') ? H('From').val() : '', toDate: H('ToChk').is(':checked') ? H('To').val() : '',
            fromDocNo: int(H('FromNo').val()) || '', toDocNo: int(H('ToNo').val()) || '', approvedStatus: H('Status').val()
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
                        if (c.b === 'del') return '<td><button type="button" class="prv-cbtn" tabindex="-1">' + c.t + '</button></td>';   /* the desktop column has no handler */
                        return '<td><button type="button" class="prv-cbtn prv-hb" data-b="' + c.b + '" data-i="' + i + '">' + c.t + '</button></td>';
                    }
                    var x = v[c.k];
                    if (c.link === 2) return '<td><a class="prv-att" data-i="' + i + '">' + esc(int(x) > 0 ? x : '') + '</a></td>';
                    if (c.n === 1) x = fmt(x); else if (c.n === 3) x = fmt3(x); else if (c.n === 2) x = fmtRate(x);
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
        $.getJSON(BASE + '/' + v.id + '/lines').then(function (r) { if (hs.sel === i) renderLines(r); });
    }

    /** DataGridHistory_SelectionChanged -> grdDetail (gridDetailsetting): rows by SubNo, Subsidiary column hidden. */
    function renderLines(r) {
        var F = S.F, data = (r && r.rows) || [];
        var cs = [['accountCode', 'AccountCode'], ['accountTitle', 'Account Title'], ['jobLot', 'JobLot'], ['remarks', 'Remarks'], ['cheqNo', 'CheqNo'],
                  F.multiCurrencyFeature && ['debitFcyAmount', 'Fcy Debit', 3], ['amountDr', 'Debit', 1],
                  F.multiCurrencyFeature && ['creditFcyAmount', 'Fcy Credit', 3], ['amountCr', 'Credit', 1],
                  F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']].filter(Boolean);
        var sums = {};
        H('Detail').find('thead').html('<tr>' + cs.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + c[1] + '</th>'; }).join('') + '</tr>');
        H('Detail').find('tbody').html(data.map(function (x) {
            return '<tr>' + cs.map(function (c) {
                var v = x[c[0]];
                if (c[2]) { sums[c[0]] = (sums[c[0]] || 0) + num(v); v = c[2] === 3 ? fmt3(v) : fmt(v); }
                return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join(''));
        H('Detail').find('tfoot').html('<tr>' + cs.map(function (c) {
            return '<td class="num" style="font-weight:bold;">' + (c[2] ? (c[2] === 3 ? fmt3(sums[c[0]] || 0) : fmt(sums[c[0]] || 0)) : '') + '</td>';
        }).join('') + '</tr>');
    }

    /* DataGridHistory_ColumnButtonClick: Edit = Reset + ReadById; Print = AcRptPaymentReceiptsVoucherSlip_102(id, 17) */
    function historyButton(b, i, btn) {
        var v = S.hist.rows[i]; if (!v) return;
        if (b === 'edit') { loadForEdit(v.id); }
        else if (b === 'p1') printDoc(v.id, btn, null);
    }

    function wireHistory() {
        var hs = S.hist, $g = H('Grid');
        H('Reset').on('click', historyReset);
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

    function focusNext(from) {
        var $all = $('#tabFormContent, #tabHistContent').filter(':visible')
            .find('input,select,textarea,button').filter(':visible').filter(function () {
                return !this.disabled && (!this.readOnly || $(this).hasClass('dtcombo-input')) && this.type !== 'hidden' && this.tabIndex >= 0;
            });
        var i = $all.index(from);
        if (i >= 0 && i + 1 < $all.length) $all.eq(i + 1).focus();
    }

    /* DayBookVoucher_KeyDown */
    function onKey(e) {
        if ($('#jnoKeys').is(':visible')) return;
        var k = e.key || '', K = k.toUpperCase(), r = rights(), hd = onHist();
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey) {                  // Keys.Return -> SendKeys {TAB}
            var t = e.target;
            if (t && /^(INPUT|SELECT|TEXTAREA)$/.test(t.tagName) && t.type !== 'button' && t.type !== 'submit') {
                if ($(t).closest('#jnoGrid').length) { e.preventDefault(); $(t).trigger('change'); return; }
                if (t.tagName === 'TEXTAREA') return;
                e.preventDefault(); focusNext(t);
            }
            return;
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { shortcuts(); return; }
        if (e.altKey && e.shiftKey && !e.ctrlKey && K === 'T' && !hd) {                  // Alt+Shift+T shows the two hidden buttons
            e.preventDefault(); $('#btnLoadTrialBalance, #btnGenerateRemarks').show(); return;
        }
        if (e.ctrlKey && e.shiftKey && !e.altKey && K === 'L' && !hd) { e.preventDefault(); loadTrialBalance(); return; }
        if (e.ctrlKey && !e.altKey) {
            if (K === 'S') { e.preventDefault(); if (hd) historyShow(); else if ($('#btnSave').is(':visible') && !$('#btnSave').prop('disabled')) save('save'); }
            else if (K === 'U') { e.preventDefault(); if ($('#btnUpdate').is(':visible') && !$('#btnUpdate').prop('disabled')) save('update'); }
            else if (K === 'N') { e.preventDefault(); if (hd) historyReset(); else newClick(); }
            else if (K === 'E') { e.preventDefault(); w.location.href = '/accounts/dashboard'; }
            else if (K === 'T') { e.preventDefault(); if (hd) { switchTab('form'); $('#jnoDate').focus(); } else switchTab('history'); }
            else if (K === 'P') { e.preventDefault(); if (r.print) printButton($('#btnPrint')[0]); }
            else if (K === 'R') { e.preventDefault(); if (!hd) refreshLists($('#btnRefresh')); }
            else if (k === 'F5') { e.preventDefault(); if (hd) H('From').focus(); else $('#jnoDate').focus(); }
            else if (k === 'F10') { e.preventDefault(); attachments(); }
            else if (k === 'ArrowDown') { e.preventDefault(); (hd ? H('Grid') : $('#jnoGrid')).focus(); }
            else if (k === 'ArrowUp') { e.preventDefault(); if (hd) H('From').focus(); else comboFocus('jnoAccount'); }
        }
    }

    function wireForm() {
        $('#jnoAccount').on('change', accountLeave);                      // CmbDr_Leave
        $('#jnoSubsidiary').on('change', subsidiaryChosen);
        $('#jnoDate').on('change', function () { if (int($('#jnoAccount').val()) > 0) accountLeave(); subBalance(); });
        $('#jnoExchangeRate').on('keydown', decimalOnly).on('input', rateChangedHeader)
            .on('blur', function () { $(this).val(fmtRate($(this).val())); rateChangedHeader(); });
        $('#jnoCurrency').on('change', currencyLeave);
        $('#jnoAmount').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); link('dr'); });
        $('#jnoCredit').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); link('cr'); });
        $('#jnoFcyDebit').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); link('fdr'); });
        $('#jnoFcyCredit').on('keydown', decimalOnly).on('input', function () { commaTyping.call(this); link('fcr'); });
        $('#jnoLineRemarks').on('mousedown', function () { S.remarksMouse = true; })
            .on('focus', function () { var el = this; if (!S.remarksMouse) setTimeout(function () { el.select(); }, 0); })
            .on('blur', function () { S.remarksMouse = false; });
        $('#jnoAdd').on('click', add);
        $('#jnoUpdateDetail').on('click', updateDetail);
        $('#jnoCancelDetail').on('click', cancelDetail);
        $('#jnoUpdateJobLot').on('click', updateJobLot);
        $('#jnoGrid').on('click', '.prv-del', function (ev) { ev.stopPropagation(); deleteRow(int($(this).data('i'))); });
        $('#jnoGrid').on('click', 'tbody tr[data-i]', function () { $('#jnoGrid tbody tr').removeClass('prv-sel'); $(this).addClass('prv-sel'); });
        $('#jnoGrid').on('dblclick', 'tbody tr[data-i]', function (ev) { if ($(ev.target).is('input,select')) return; editRow(int($(this).data('i'))); });
        $('#jnoGrid').on('keydown', function (e) {
            var sel = $('#jnoGrid tbody tr.prv-sel'); if (!sel.length) sel = $('#jnoGrid tbody tr[data-i]').first();
            if (!sel.length) return;
            var i = int(sel.data('i'));
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRow(i); }
            else if (e.ctrlKey && e.key === ' ') { e.preventDefault(); deleteRow(i); }
            else if (!e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowUp') && !$(e.target).is('input,select')) {
                var n = S.rows.length, j = i + (e.key === 'ArrowDown' ? 1 : -1);
                if (j >= 0 && j < n) { e.preventDefault(); $('#jnoGrid tbody tr').removeClass('prv-sel').filter('[data-i="' + j + '"]').addClass('prv-sel'); }
            }
        });
        /* in-cell edits (the grid is AllowEdit): remarks, cheq no and the two amounts; totals and Difference follow */
        $('#jnoGrid').on('change', '.prv-ce', function () {
            var i = int($(this).data('i')), k = $(this).data('k'), r = S.rows[i]; if (!r) return;
            if (k === 'amountDr' || k === 'amountCr') {
                r[k] = num($(this).val());
                var rate = num($('#jnoExchangeRate').val());
                r.fcyAmountDr = rate > 0 ? num(r.amountDr) / rate : 0; r.fcyAmountCr = rate > 0 ? num(r.amountCr) / rate : 0;
                renderGrid();
            } else r[k] = $(this).val();
        });
        $('#jnoBtnHistory').on('click', function () { switchTab('history'); });
    }

    function wireToolbar() {
        $('#btnNew').on('click', newClick);
        $('#btnRefresh').on('click', function () { refreshLists($(this)); });
        $('#btnSave').on('click', function () { save('save'); });
        $('#btnUpdate').on('click', function () { save('update'); });
        $('#btnAttachment').on('click', attachments);
        $('#btnPrint').on('click', function () { printButton(this); });
        $('#btnShortcuts').on('click', shortcuts);
        $('#btnGenerateRemarks').on('click', generateRemarks);
        $('#btnLoadTrialBalance').on('click', loadTrialBalance);
        $('#tabBtnForm').on('click', function () { switchTab('form'); });
        $('#tabBtnHist').on('click', function () { switchTab('history'); });
        $(document).on('keydown', onKey);
    }

    w.JNO = {
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
        print: function () { printButton($('#btnPrint')[0]); },
        switchTab: switchTab
    };
}(window, jQuery));
