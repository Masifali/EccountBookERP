/* ============================================================================================
 * Screens 28-31 - Cash Payment (1), Bank Payment (2), Cash Receipt (3), Bank Receipt Voucher (4).
 *
 * Desktop: Architecture.WinApp.Account_Definition.PaymentVoucherNew (1/2) and
 *          Architecture.WinApp.Account_Definition.ReceiptsVoucherNew (3/4) - the NON-tax forms
 *          (ScreenDefinition TargetUrl; CommonServices.EditMethodFromLinked).
 *
 * One source for the four pages: the Form tab AND the History tab are rendered here.
 * Backend: /accounts/api/payment-receipt/{doc}/... (PaymentReceiptVoucherController) - lookups from the
 * desktop's own fill methods, Save/Update through the desktop procedure chain (DesktopVoucherWriter),
 * ReadById through Sp_Vouchers_GetMethods 'ReadByID' / 'VoucherDetail_ReadByVoucherHeadID', history
 * through USP_VoucherFormHistory.
 *
 * 2026-10-02 event-parity pass (every handler wired in InitializeComponent, see the page notes per
 * function): desktop layout (panel4 strip / Main / Detail flow / grid / WHT panel), Reset() and
 * btnNew_Click() exactly as each form resets, grid in-cell editing, serial-wise cheque rules on
 * update/delete, cmbCurrency_Leave last rate, txtExchangeRate_Leave format, amount key filter and
 * comma typing, Enter = Tab, every KeyDown shortcut, Refresh re-binds lists in place (btnRefresh_Click),
 * Save As, the three 102 prints, and the History tab with its own buttons, totals and detail grid.
 *
 * The page posts grid ROWS; the ledger lines (vd/vd2/vd3/vd4) are built on the server.
 *
 * 2026-10-02 round 3 (GUI): the Form and History tabs are drawn at the designer coordinates of
 * PaymentVoucherNew / ReceiptsVoucherNew (see render() / renderHistory()); every combo is
 * countx_prod_combo.js (the select sits INSIDE its .dtcombo-wrap, so focus goes through comboFocus()).
 * ============================================================================================ */
(function (w, $) {
    'use strict';

    var API = '/accounts/api/payment-receipt';
    var ROUTE = { 1: 'cash-payment', 2: 'bank-payment', 3: 'cash-receipt', 4: 'bank-receipt' };
    var TITLE = { 1: 'Cash Payment Voucher', 2: 'Bank Payment Voucher', 3: 'Cash Receipt Voucher', 4: 'Bank Receipt Voucher' };
    var TAG = { 1: 'CPV', 2: 'BPV', 3: 'CRV', 4: 'BRV' };
    /* CommonServices.PaymentAndReceiptVoucherSlip: the 102-III report per DocumentTypeId. */
    var RPT3 = { 1: '102-CashPaymentVoucher.rpt', 2: '102-BankPaymentVoucher.rpt', 3: '102-CashReceiptVoucher.rpt', 4: '102-BankReceiptVoucher.rpt' };
    /* Special Rights screen id (kept as published on 09-30; see the project note on GetScreenIdByName). */
    var SCREEN = { 1: 28, 2: 29, 3: 30, 4: 31 };

    var S = {
        doc: 0, pay: false, L: null, F: {}, rows: [], recId: 0, updateMode: false, saveAs: false,
        editIndex: -1,          // the row open in the detail box (Add hidden, Update/Cancel shown)
        updIdx: -1,             // the desktop's updateDetailIndex: set by a row double-click, cleared only by Reset()
        editCheque: null,       // CheqNoValidatingInUpdate + the leaf id of the row being edited
        cheques: [], busy: false, accMode: 'title', remarksMouse: false, codeSeq: 0, chqSeq: 0,
        hist: { rows: [], sel: -1, busy: false }
    };
    /* print-rpt.js reads the open voucher's id (S.recId == VoucherHeadId) */
    try { Object.defineProperty(w, 'RecId', { get: function () { return S.recId; }, configurable: true });
          Object.defineProperty(w, 'VoucherHeadId', { get: function () { return S.recId; }, configurable: true }); } catch (e) { }

    if (w.DesktopCombo) {
        /* DetailAccountFill -> GetAccountsFromGlobalByTypeIds: Id | AccountTitle | AccountCode |
           ParentAccountTitle | AccountClass, Id hidden. rdSearchByAccountName (Title) / rdSearchByAccountCode
           (Code) switch the display member, so the Code family shows the code first. */
        w.DesktopCombo.define('prvAcc4', [
            { caption: 'Account Title', flex: 4 },
            { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' },
            { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('prvAcc4c', [
            { caption: 'Account Code', flex: 2 },
            { caption: 'AccountTitle', flex: 4, key: 'title' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' },
            { caption: 'AccountClass', flex: 2, key: 'cls' }
        ]);
        w.DesktopCombo.define('prvAcc2', [
            { caption: 'Account Title', flex: 4 },
            { caption: 'AccountCode', flex: 2, key: 'code' }
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
    function dmy(v) {                                     // grid ChequeDate FormatString "dd-MMM-yyyy"
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
            var code = ci(a, 'accountCode'), title = ci(a, 'accountTitle');
            h += '<option value="' + esc(ci(a, 'id')) + '" data-code="' + esc(code) + '" data-title="' + esc(title) + '" data-parent="' +
                esc(ci(a, 'parentAccountTitle')) + '" data-cls="' + esc(ci(a, 'accountClass')) + '">' + esc(mode === 'code' ? code : title) + '</option>';
        });
        return h;
    }
    function selText(id) { var o = $('#' + id + ' option:selected'); return o.length && o.val() !== '0' ? o.text() : ''; }
    function setVal(id, v) { var $s = $('#' + id); $s.val(String(v == null ? 0 : v)); if ($s.val() == null) $s.val($s.find('option:first').val()); $s.trigger('change'); }
    function firstRow(id) { var $s = $('#' + id), o = $s.find('option').filter(function () { return this.value !== '0'; }).first(); if (o.length) setVal(id, o.val()); }
    function rights() { return S.F.rights || {}; }
    /** the visible field of a combo (countx_prod_combo puts the select INSIDE .dtcombo-wrap) */
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

    /* Server buttons: disabled at once + spinner, duplicates refused, re-enabled on success AND failure. */
    function busyOn($b) {
        $b.each(function () {
            var b = $(this); if (b.data('prvBusy')) return;
            b.data('prvBusy', 1).data('prvHtml', b.html()).prop('disabled', true)
             .html('<i class="fa fa-spinner fa-spin"></i> ' + $.trim(b.text()));
        });
    }
    function busyOff($b) {
        $b.each(function () {
            var b = $(this); if (!b.data('prvBusy')) return;
            b.html(b.data('prvHtml')).removeData('prvBusy').prop('disabled', false);
        });
    }

    // ------------------------------------------------------------------------------ markup (Form tab)
    /*
     * 2026-10-02 round 3 - the designer layout. Every number below is a Location / Size from
     * InitializeComponent() of PaymentVoucherNew.cs (form 1468x704, tab page 1460x674) or
     * ReceiptsVoucherNew.cs (form 1324x664, tab page 1316x638). The toolstrip (panel3, 30 px) is in the
     * template, so the y values here are the designer's minus 30.
     *   PaymentVoucherNew : panel4 teal 1456x36 | groupBox2 "Main" (1,68) 1104x128 | groupboxdetail
     *                       "Detail" (2,197) 1103x109 with flowLayoutPanel1 (1,12) 1100x97 | panel6: grd
     *                       docked left 1105 wide, panel17 (WHT / totals) docked bottom 86 high.
     *   ReceiptsVoucherNew: panel4 teal 1312x32 | groupBox2 "Main" (4,65) 1206x127 | groupboxdetail
     *                       "Detail" (4,194) 1206x114 with flowLayoutPanel1 (3,13) 1200x97 (Load shrinks it
     *                       to 63 / 49 and panel2 to 257 when no Subsidiary / Branch / Booking office) |
     *                       panel6: grd docked left 1210 wide, panel21 docked bottom 85 high.
     * The detail panels are a FlowLayoutPanel: the visible ones are laid out left to right and wrap
     * at the flow width, as Load() leaves them (Visible / RemarksPanel.Size per feature).
     */

    function px(x, y, w, h) {
        return 'left:' + x + 'px;top:' + y + 'px;' + (w != null ? 'width:' + w + 'px;' : '') + (h != null ? 'height:' + h + 'px;' : '');
    }
    function lab(x, y, text, cls, id, w) {
        return '<label class="prv-l' + (cls ? ' ' + cls : '') + '"' + (id ? ' id="' + id + '"' : '') + ' style="' + px(x, y, w) + '">' + text + '</label>';
    }
    /** an UltraCombo -> a select that countx_prod_combo.js turns into its searchable combo at this spot */
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
    function grp(id, inner) { return '<div class="prv-g" id="' + id + '">' + inner + '</div>'; }

    /** FlowLayoutPanel: [{id, w, show, inner}] -> absolutely placed 47 px high panels, wrapped at `width`. */
    function flow(panels, width) {
        var x = 0, y = 0, h = '', rows = 1;
        panels.forEach(function (p) {
            if (!p.show) {                       // Visible = false: kept in the page (ids), takes no room
                h += '<div class="prv-fp"' + (p.id ? ' id="' + p.id + '"' : '') + ' style="display:none;' + px(0, 0, p.w, 47) + '">' + p.inner + '</div>';
                return;
            }
            if (x > 0 && x + p.w > width) { x = 0; y += 47; rows++; }
            h += '<div class="prv-fp"' + (p.id ? ' id="' + p.id + '"' : '') + ' style="position:absolute;' + px(x, y, p.w, 47) + '">' + p.inner + '</div>';
            x += p.w;
        });
        return { html: h, rows: rows };
    }

    function render() {
        var d = S.doc, pay = S.pay, h = '';
        var subsidiary = !!S.F.subsidiaryFeature, branch = !!S.F.branchFeature, booking = !!S.F.isBookingOffice;
        /* Load(): PaymentVoucherLable.Text per DocumentTypeId (the desktop's own spelling) */
        var label = ({ 1: 'Cash Payment Voucher', 2: 'Bank PaymentVoucher', 3: 'Cash Receipt Voucher', 4: 'Bank Receipt Voucher' })[d];
        var btns, top, det, grid, bot;

        if (pay) {
            /* ---- panel4 (Dock Top, 1456x36, BackColor 10,110,110); the check boxes are anchored Top|Right */
            var strip = '<div class="prv-strip" style="height:36px;">' +
                '<span class="prv-strip-title" style="position:absolute;left:7px;top:8px;">' + label + '</span>' +
                '<div class="prv-ar" style="width:1456px;height:36px;">' +
                '<label class="prv-v" id="lblPrintOnSave" style="display:none;' + px(843, 6, 183) + '"><input type="checkbox" id="cbPrintOnSave"/>Cheque Print On Save</label>' +
                '<label style="' + px(1043, 8, 102) + '"><input type="checkbox" id="prvChkPrint1" checked/>Print Preview</label>' +
                '<label style="' + px(1151, 8, 126) + '"><input type="checkbox" id="prvChkPrint2"/>Preview Format II</label>' +
                '<label style="' + px(1278, 8, 126) + '"><input type="checkbox" id="prvChkPrint3"/>Preview Format II</label>' +
                '</div></div>';
            /* ---- groupBox2 "Main" (1,68) 1104x128 */
            var main =
                lab(14, 15, 'Cost Center') + cmb('prvProject', 111, 10, 240, 24, '', 'prv-cmb-mss') +
                lab(14, 43, 'Voucher Type') +
                '<select id="prvVoucherType" class="prv-cmb" data-dtcombo="single" disabled style="position:absolute;' + px(111, 37, 240, 26) + '"><option value="' + d + '">' + TITLE[d] + '</option></select>' +
                lab(14, 70, 'Voucher No') + tb('prvVoucherNo', 111, 66, 74, 23, ' readonly') +
                lab(190, 70, 'Date') + dtp('prvDate', 229, 66, 122, 23) +
                lab(14, 97, 'Credit Account') + cmb('prvRefAccount', 111, 91, 240, 26, ' data-dtcombo="prvAcc2" data-dtcombo-caption="Credit Account"') +
                lab(357, 15, 'WHT Debit Ac') + cmb('prvAgainstAc', 460, 9, 282, 26, ' data-dtcombo="prvAcc2" disabled') +
                lab(357, 39, 'Remarks', 'prv-tah') +
                '<textarea id="prvRemarks" class="prv-tb" style="' + px(460, 41, 282, 76) + '"></textarea>' +
                grp('prvFcyCodeWrap', lab(749, 15, 'Fcy Code') + cmb('prvCurrency', 834, 13, 111, 26)) +
                grp('prvRateWrap', lab(749, 45, 'Exchange Rate') + tb('prvExchangeRate', 834, 41, 111, 23, '', 'prv-num')) +
                grp('prvFcyAmtWrap', lab(749, 70, 'Fcy Amount') + tb('prvFcyAmount', 834, 66, 111, 23, ' readonly tabindex="-1"', 'prv-num prv-dis')) +
                '<span id="prvRefBalance" class="prv-bal" style="display:none;' + px(748, 98) + '"></span>';
            /* ---- groupboxdetail "Detail" (2,197) 1103x109 -> flowLayoutPanel1 (1,12) 1100 wide */
            var remW = subsidiary ? 330 : 498;                                    // Load: RemarksPanel.Size
            if (!booking && !branch && !subsidiary && d === 1) remW = 330;
            var panels = [
                { w: 87, show: true, inner: lab(1, 4, 'Payment Type') + cmb('prvPaymentType', 1, 21, 86, 26) },
                { w: 263, show: true, inner: lab(1, 4, 'Debit Acc') +
                    '<label class="prv-rb" style="' + px(58, 2) + '"><input type="radio" name="prvAccMode" value="title" checked/>Title</label>' +
                    '<label class="prv-rb" style="' + px(102, 2) + '"><input type="radio" name="prvAccMode" value="code"/>Code</label>' +
                    '<span id="prvAccBalance" class="prv-bal prv-r" style="display:none;' + px(152, 4, 107, 14) + '"></span>' +
                    cmb('prvAccount', 1, 21, 261, 26, ' data-dtcombo="prvAcc4" data-dtcombo-caption="Account Title"') },
                { w: 168, show: subsidiary, inner: lab(0, 3, 'Subsidiary A/c') +
                    '<select id="prvSubsidiary" class="prv-cmb" data-dtcombo="single" disabled title="Subsidiary A/c is not on the web form yet" style="position:absolute;' + px(0, 21, 168, 26) + '"></select>' },
                { w: 135, show: true, inner: lab(0, 2, 'Job/Lot') + cmb('prvJobLot', 0, 21, 134, 26) },
                { w: remW, show: true, inner: lab(0, 3, 'Remarks') + tb('prvLineRemarks', 0, 22, remW - 1, 23) },
                { w: 117, show: true, inner: lab(1, 3, 'Debit Amount') +
                    '<input type="text" id="prvAmount" class="prv-tb prv-num" autocomplete="off" style="' + px(1, 22, 114, 23) + '"/>' },
                { w: 133, show: d === 2, inner: lab(1, 2, 'Financial Instrument') + cmb('prvInstrument', 1, 19, 132, 26) },
                { w: 114, show: d === 2, inner: lab(4, 3, 'Cheque Date') + dtp('prvChequeDate', 1, 21, 113, 23) },
                { w: 140, show: d === 2, inner: lab(0, 1, 'Cheque No') +
                    '<input type="text" id="prvChequeNo" class="prv-tb" list="prvChequeList" autocomplete="off" maxlength="50" style="' + px(1, 19, 138, 26) + '"/><datalist id="prvChequeList"></datalist>' },
                { w: 178, show: d === 2, inner: lab(0, 1, 'Payee Title') + tb('prvPayeeTitle', 0, 19, 178, 26, ' maxlength="100"') },
                { w: 117, show: d === 2, inner: lab(1, 1, 'Cheque Type') + cmb('prvChequeType', 1, 19, 116, 26) },
                { id: 'prvBranchWrap', w: 163, show: branch, inner: lab(1, 1, 'Branch Name') + cmb('prvBranch', 1, 19, 161, 26) },
                { id: 'prvCostCenterWrap', w: 138, show: booking, inner: lab(2, 1, 'Cost Center') + cmb('prvCostCenter', 1, 19, 137, 26) },
                { w: 117, show: true, inner:
                    '<button type="button" id="prvAdd" class="prv-fbtn" style="' + px(0, 19, 26, 25) + '">+</button>' +
                    '<button type="button" id="prvUpdateDetail" class="prv-fbtn" style="display:none;' + px(0, 19, 59, 25) + '">Update</button>' +
                    '<button type="button" id="prvCancelDetail" class="prv-fbtn" style="display:none;' + px(58, 19, 57, 25) + '">Cancel</button>' }
            ];
            var fl = flow(panels, 1100);
            top = '<div class="prv-abs" style="height:279px;">' + strip +
                  gbox('Main', 1, 38, 1104, 128, main) +
                  gbox('Detail', 2, 167, 1103, 109, '<div class="prv-g" style="' + px(1, 12) + '">' + fl.html + '</div>') +
                  '</div>';
            grid = '<div class="prv-mid"><div class="prv-gridbox" style="width:1105px;" id="prvGridBox"><table class="prv-jg" id="prvGrid" tabindex="0"><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
            /* ---- panel17 (Dock Bottom, 86 high) */
            bot = '<div class="prv-bot prv-abs" style="height:86px;">' +
                '<label class="prv-rb prv-v" style="' + px(613, 8) + '"><input type="radio" name="prvTaxMode" value="excluded" id="prvRadExcluded" checked/>Excluded Tax</label>' +
                '<label class="prv-rb prv-v" style="' + px(737, 8) + '"><input type="radio" name="prvTaxMode" value="included" id="prvRadIncluded"/>Included Tax</label>' +
                lab(452, 36, 'TaxType') + cmb('prvTaxType', 580, 31, 92, 24, '', 'prv-cmb-mss') +
                '<label class="prv-rb prv-plain" style="' + px(679, 33) + '"><input type="checkbox" id="prvWht"/>WHT</label>' +
                lab(735, 35, 'Tax %', 'prv-seg') + tb('prvTaxPercent', 785, 31, 72, 25, ' readonly value="0" tabindex="-1"', 'prv-seg') +
                lab(452, 63, 'WHT Credit Account') + cmb('prvWithHoldingAc', 580, 58, 277, 24, ' data-dtcombo="prvAcc2" disabled', 'prv-cmb-mss') +
                lab(870, 9, 'Amount') + tb('prvValue', 963, 4, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
                lab(870, 36, 'Tax Amount') + tb('prvTaxAmount', 963, 31, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
                lab(874, 63, 'Total Amount') + tb('prvTotalAmount', 963, 58, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
                '<button type="button" class="prv-sysbtn" id="prvBtnHistory" style="left:auto;right:10px;top:56px;width:92px;height:26px;"><i class="fa fa-history"></i>History</button>' +
                '</div>';
        } else {
            var small = !booking && !branch && !subsidiary;                     // Load: groupboxdetail 63, flow 49, panel2 257
            var strip2 = '<div class="prv-strip" style="height:32px;">' +
                '<span class="prv-strip-title" style="position:absolute;left:3px;top:6px;">' + label + '</span>' +
                '<div class="prv-ar" style="width:1312px;height:32px;">' +
                '<label style="' + px(936, 6, 95) + '"><input type="checkbox" id="prvChkPrint1" checked/>Print Preview</label>' +
                '<label style="' + px(1042, 6, 104) + '"><input type="checkbox" id="prvChkPrint2"/>Print Preview II</label>' +
                '<label style="' + px(1146, 6, 110) + '"><input type="checkbox" id="prvChkPrint3"/>Print Preview III</label>' +
                '</div></div>';
            /* ---- groupBox2 "Main" (4,65) 1206x127 */
            var main2 =
                lab(6, 15, 'Project') + cmb('prvProject', 100, 10, 232, 24, '', 'prv-cmb-mss') +
                lab(6, 43, 'Voucher Type') +
                '<select id="prvVoucherType" class="prv-cmb" data-dtcombo="single" disabled style="position:absolute;' + px(99, 37, 232, 26) + '"><option value="' + d + '">' + TITLE[d] + '</option></select>' +
                lab(6, 70, 'Voucher No') + tb('prvVoucherNo', 100, 66, 86, 23, ' readonly') +
                lab(191, 70, 'Date', 'prv-mss') + dtp('prvDate', 231, 66, 101, 23) +
                lab(6, 97, 'Debit Account') + cmb('prvRefAccount', 100, 91, 232, 26, ' data-dtcombo="prvAcc2" data-dtcombo-caption="Debit Account"') +
                lab(341, 16, 'Against Ac') + cmb('prvAgainstAc', 407, 10, 276, 26, ' data-dtcombo="prvAcc2" disabled') +
                lab(339, 43, 'Remarks') +
                '<textarea id="prvRemarks" class="prv-tb" style="' + px(407, 39, 276, 78) + '"></textarea>';
            if (d === 4) {                                                       // CRV: Load hides the cheque trio
                main2 += lab(689, 20, 'Cheque Date') + dtp('prvHdrChequeDate', 771, 16, 213, 23) +
                         lab(689, 46, 'Cheque No') + tb('prvHdrChequeNo', 771, 42, 213, 23, ' maxlength="20"') +
                         lab(689, 71, 'PayTitle') + tb('prvHdrPayTitle', 771, 67, 213, 23, ' maxlength="100"');
            }
            main2 += '<span id="prvRefBalance" class="prv-bal" style="display:none;' + px(690, 101) + '"></span>' +
                grp('prvFcyCodeWrap', lab(991, 20, 'Fcy Code') + cmb('prvCurrency', 1076, 14, 111, 26)) +
                grp('prvRateWrap', lab(991, 46, 'Exchange Rate') + tb('prvExchangeRate', 1076, 42, 111, 23, '', 'prv-num')) +
                grp('prvFcyAmtWrap', lab(991, 71, 'Fcy Amount') + tb('prvFcyAmount', 1076, 67, 111, 23, ' readonly tabindex="-1"', 'prv-num prv-dis'));
            /* ---- groupboxdetail "Detail" (4,194) 1206x114 -> flowLayoutPanel1 (3,13) 1200 wide */
            var remR = small ? 330 : (subsidiary ? 462 : 480);
            var panels2 = [
                { w: 96, show: true, inner: lab(1, 2, 'PaymentType') + cmb('prvPaymentType', 2, 21, 94, 26) },
                { w: 280, show: true, inner: lab(1, 3, 'Credit Acc') +
                    '<label class="prv-rb" style="' + px(62, 1) + '"><input type="radio" name="prvAccMode" value="title" checked/>Title</label>' +
                    '<label class="prv-rb" style="' + px(105, 1) + '"><input type="radio" name="prvAccMode" value="code"/>Code</label>' +
                    '<span id="prvAccBalance" class="prv-bal prv-r" style="display:none;' + px(156, 5, 121, 14) + '"></span>' +
                    cmb('prvAccount', 0, 21, 278, 26, ' data-dtcombo="prvAcc4" data-dtcombo-caption="Credit Account"') },
                { w: 204, show: subsidiary, inner: lab(0, 2, 'Subsidiary A/c') +
                    '<select id="prvSubsidiary" class="prv-cmb" data-dtcombo="single" disabled title="Subsidiary A/c is not on the web form yet" style="position:absolute;' + px(0, 21, 203, 26) + '"></select>' },
                { w: 155, show: true, inner: lab(3, 2, 'Job/Lot') + cmb('prvJobLot', 0, 21, 153, 26) },
                { w: remR, show: true, inner: lab(0, 3, 'Remarks') + tb('prvLineRemarks', 0, 23, remR - 1, 23) },
                { w: 155, show: true, inner: lab(1, 4, 'Credit Amount') +
                    '<input type="text" id="prvAmount" class="prv-tb prv-num" autocomplete="off" style="' + px(1, 23, 153, 23) + '"/>' },
                { id: 'prvBranchWrap', w: 204, show: branch, inner: lab(0, 2, 'Branch Name') + cmb('prvBranch', 0, 21, 204, 26) },
                { id: 'prvCostCenterWrap', w: 221, show: booking, inner: lab(0, 2, 'Cost Center') + cmb('prvCostCenter', 0, 21, 219, 26) },
                { w: 155, show: true, inner:
                    '<button type="button" id="prvAdd" class="prv-fbtn prv-tah" style="' + px(0, 21, 26, 25) + '">+</button>' +
                    '<button type="button" id="prvUpdateDetail" class="prv-fbtn prv-tah" style="display:none;' + px(2, 21, 59, 25) + '">Update</button>' +
                    '<button type="button" id="prvCancelDetail" class="prv-fbtn prv-tah" style="display:none;' + px(60, 21, 59, 25) + '">Cancel</button>' }
            ];
            var fl2 = flow(panels2, 1200);
            var detH = small ? 63 : 114, panel2H = small ? 257 : 305;
            if (fl2.rows > 1 && detH < 13 + 47 * fl2.rows + 4) detH = 13 + 47 * fl2.rows + 4;
            top = '<div class="prv-abs" style="height:' + (panel2H - 30) + 'px;">' + strip2 +
                  gbox('Main', 4, 35, 1206, 127, main2) +
                  gbox('Detail', 4, 164, 1206, detH, '<div class="prv-g" style="' + px(3, 13) + '">' + fl2.html + '</div>') +
                  '</div>';
            grid = '<div class="prv-mid"><div class="prv-gridbox" style="width:1210px;" id="prvGridBox"><table class="prv-jg" id="prvGrid" tabindex="0"><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div></div>';
            /* ---- panel21 (Dock Bottom, 85 high) */
            bot = '<div class="prv-bot prv-abs" style="height:85px;">' +
                lab(598, 9, 'TaxType') + cmb('prvTaxType', 684, 4, 92, 24, '', 'prv-cmb-mss') +
                '<label class="prv-rb" style="' + px(782, 7) + '"><input type="checkbox" id="prvWht"/>WHT</label>' +
                lab(840, 9, 'Tax %') + '<input type="text" id="prvTaxPercent" class="prv-tb prv-seg" readonly value="0" tabindex="-1" style="' + px(890, 6, 72, 20) + '"/>' +
                lab(598, 36, 'WHT Account') + cmb('prvWithHoldingAc', 684, 31, 278, 24, ' data-dtcombo="prvAcc2" disabled', 'prv-cmb-mss') +
                lab(975, 9, 'Amount') + tb('prvValue', 1068, 4, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
                lab(975, 36, 'Tax Amount') + tb('prvTaxAmount', 1068, 31, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
                lab(975, 62, 'Total Amount') + tb('prvTotalAmount', 1068, 57, 142, 25, ' readonly tabindex="-1"', 'prv-num prv-seg') +
                '<button type="button" class="prv-sysbtn" id="prvBtnHistory" style="left:auto;right:10px;top:55px;width:92px;height:26px;"><i class="fa fa-history"></i>History</button>' +
                '</div>';
        }
        h = top + grid + bot;
        $('#prvForm').html(h);
    }

    // ------------------------------------------------------------------------------ lookups

    function capture() {
        var o = {};
        ['prvProject', 'prvRefAccount', 'prvAccount', 'prvAgainstAc', 'prvWithHoldingAc', 'prvPaymentType', 'prvJobLot', 'prvCurrency',
         'prvTaxType', 'prvBranch', 'prvCostCenter', 'prvInstrument', 'prvChequeType'].forEach(function (id) { o[id] = $('#' + id).val(); });
        return o;
    }
    /* A programmatic value (Value = ...) raises no Leave on the desktop, so no change event here. */
    function put(id, v) { var $s = $('#' + id); $s.val(String(v == null ? 0 : v)); if ($s.val() == null) $s.val($s.find('option:first').val()); }
    function restore(o, ids) { ids.forEach(function (id) { if (o[id] != null) put(id, o[id]); }); }

    function fillLists(L) {
        $('#prvProject').html(opts(L.projects, 'Id', 'ProjectName', false));                 // BindDDLNew(..., false)
        $('#prvRefAccount').html(accOpts(L.headerAccounts, true, 'title'));                 // ZeroIndex: true
        $('#prvAccount').html(accOpts(L.detailAccounts, true, S.accMode));
        $('#prvAgainstAc, #prvWithHoldingAc').html(accOpts(L.whtAccounts, true, 'title'));
        $('#prvPaymentType').html(opts(L.paymentTypes, 'Id', 'PaymentType', false));
        $('#prvJobLot').html(opts(L.jobLots, 'Id', 'JobLotDescription', S.pay));           // payment ZeroIndex true, receipt false
        $('#prvCurrency').html(opts(L.currencies, 'Id', 'CurrencyCode', true));
        $('#prvTaxType').html(opts(L.taxTypes, 'Id', 'TaxName', true));                     // BindTaxTypes: no row activated
        $('#prvBranch').html(opts(L.branches, 'BranchId', 'BranchName', false));
        $('#prvCostCenter').html(opts(L.costCenters, 'Id', 'CostCenterName', false));
        if (S.doc === 2) {
            $('#prvInstrument').html(opts(L.instrumentTypes, 'Id', 'InstrumentType', false));
            $('#prvChequeType').html(opts(L.chequeTypes, 'id', 'CheqType', true));          // Reset() clears its text -> a blank slot
        }
    }

    function bindLookups(L) {
        S.L = L; S.F = L.flags || {};
        render();
        wireForm();
        fillLists(L);
        /* MultiCurrencyFeature(): the Fcy trio is hidden when feature 6 is off (values still posted). */
        $('#prvFcyCodeWrap, #prvRateWrap, #prvFcyAmtWrap').toggle(!!S.F.multiCurrencyFeature);
        $('#lblPrintOnSave').toggle(!!S.F.chequePrintingEnable && S.pay);                   // cbPrintOnSave.Visible
        $('#cbPrintOnSave').prop('checked', false);
        $('#btnPrint').prop('disabled', !rights().print);                                   // Print.Enabled = DoHavePrintRights
        renderGridHead();
        reset(true);
    }

    /** DefaultConfigurations(): Job/Lot, Base Currency, BaseCurrencyRate (DecimalRateFormate). */
    function defaults() {
        if (S.F.defaultJobLotId != null && int(S.F.defaultJobLotId) > 0) setVal('prvJobLot', S.F.defaultJobLotId);
        if (int(S.F.baseCurrencyId) > 0) put('prvCurrency', S.F.baseCurrencyId);
        if (S.F.baseCurrencyRate != null) $('#prvExchangeRate').val(fmtRate(S.F.baseCurrencyRate));
    }

    /** btnRefresh_Click: lists re-bound in place (selection kept where the desktop keeps it), then
        DefaultConfigurations and SpecialRightsImplement. The form's entries are NOT cleared. */
    function refreshLists($b) {
        if (S.busy) return;
        busyOn($b);
        $.getJSON(API + '/' + S.doc + '/lookups').then(function (L) {
            var keep = capture();
            S.L = L; S.F = L.flags || S.F;
            fillLists(L);
            restore(keep, ['prvProject', 'prvRefAccount', 'prvAccount', 'prvAgainstAc', 'prvWithHoldingAc', 'prvPaymentType', 'prvJobLot',
                           'prvCurrency', 'prvCostCenter', 'prvChequeType']);
            if (S.F.defaultBranchId) setVal('prvBranch', S.F.defaultBranchId);             // BranchesFill: Text = UserAccount.BranchName
            if (S.doc === 2) firstRow('prvInstrument');                                     // GetFinancialInstrumentTypes: Rows[0].Activate
            defaults();
            specialRights();
            chequeFill();
            renderGrid();
        }, function (x) { alert(errMsg(x, 'Could not refresh the lists.')); }).always(function () { busyOff($b); });
    }

    // ------------------------------------------------------------------------------ grid

    /** grdSettings(): visible columns, captions and order of each form. */
    function cols() {
        var F = S.F, c;
        var edAmt = int(F.amountDecimals) <= 0;                 // Amount EditType = (DefaultNoofDecimalPointsForAmount <= 0)
        if (S.pay) {
            c = [{ k: 'x', t: 'X', w: 20 }, { k: 'paymentType', t: 'Payment Type', ed: 'pt', w: 90 }, { k: 'accountCode', t: 'AccountCode', w: 90 },
                 { k: 'accountTitle', t: 'AccountTitle', w: 220 }];
            if (F.subsidiaryFeature) c.push({ k: 'subsidiaryAccount', t: 'SubsidiaryAccount', w: 180 });
            c.push({ k: 'remarks', t: 'Remarks', ed: 'txt', w: 220 }, { k: 'amount', t: 'Debit Amount', n: 1, sum: 1, ed: edAmt ? 'amt' : '', w: 110 });
            if (S.doc === 2) c.push({ k: 'financialInstrument', t: 'FinancialInstrument' }, { k: 'chequeDate', t: 'ChequeDate', dt: 1 },
                                    { k: 'chequeNo', t: 'ChequeNo' }, { k: 'payeeTitle', t: 'PayeeTitle' });
            if (F.multiCurrencyFeature) c.push({ k: 'fcyAmount', t: 'FcyAmount', n: 1, sum: 3 });
            if (F.branchFeature) c.push({ k: 'branchName', t: 'BranchName' });
            if (F.isBookingOffice) c.push({ k: 'costCenterName', t: 'Cost Center', ed: 'cc' });
            if ($('#prvRadIncluded').is(':checked')) c.push({ k: 'taxAmount', t: 'TaxAmount', n: 1 });
        } else {
            c = [{ k: 'paymentType', t: 'PaymentType', ed: 'pt' }, { k: 'accountCode', t: 'AccountCode', w: 90 }, { k: 'accountTitle', t: 'Account Title', w: 250 }];
            if (F.subsidiaryFeature) c.push({ k: 'subsidiaryAccount', t: 'SubsidiaryAccount', w: 190 });
            c.push({ k: 'jobLot', t: 'JobLot', ed: 'jl' }, { k: 'remarks', t: 'Remarks', ed: 'txt', w: 293 },
                   { k: 'amount', t: 'Credit Amount', n: 1, sum: 1, ed: edAmt ? 'amt' : '', w: 115 });
            if (F.multiCurrencyFeature) c.push({ k: 'fcyAmount', t: 'FcyAmount', n: 1, sum: 3 });
            if (F.branchFeature) c.push({ k: 'branchName', t: 'BranchName' });
            if (F.isBookingOffice) c.push({ k: 'costCenterName', t: 'Cost Center', ed: 'cc' });
            c.push({ k: 'x', t: 'X', w: 20 });                  // ReceiptsVoucherNew adds Delete last, no Position
        }
        return c;
    }

    /* Janus GridEX: column widths from grdSettings (unset = the GridEX default 100); ColumnAutoResize = true
       spreads them over the grid width, except on BPV (false: the designer widths, scrolling sideways). */
    function renderGridHead() {
        var cs = cols(), sum = cs.reduce(function (s, c) { return s + (c.w || 100); }, 0);
        var auto = S.doc !== 2, boxW = $('#prvGridBox').innerWidth() || (S.pay ? 1103 : 1208);
        var scale = auto && sum > 0 ? Math.max(1, (boxW - 2) / sum) : 1;
        $('#prvGrid').css('width', auto ? '100%' : sum + 'px');
        $('#prvGrid colgroup').html(cs.map(function (c) { return '<col style="width:' + Math.round((c.w || 100) * scale) + 'px;"/>'; }).join(''));
        $('#prvGrid thead').html('<tr>' + cs.map(function (c) { return '<th' + (c.n ? ' class="num"' : '') + ' title="' + esc(c.t) + '">' + c.t + '</th>'; }).join('') + '</tr>');
    }

    function cellEditor(c, r, i) {
        var L = S.L || {};
        if (c.ed === 'amt') return '<input type="text" class="prv-ce" data-i="' + i + '" data-k="amount" value="' + esc(r.amount) + '" style="text-align:right;"/>';
        if (c.ed === 'txt') return '<input type="text" class="prv-ce" data-i="' + i + '" data-k="remarks" value="' + esc(r.remarks) + '"/>';
        var list, idK, txK, cur, key;
        if (c.ed === 'pt') { list = L.paymentTypes; idK = 'Id'; txK = 'PaymentType'; cur = r.paymentTypeId; key = 'paymentTypeId'; }
        else if (c.ed === 'jl') { list = L.jobLots; idK = 'Id'; txK = 'JobLotDescription'; cur = r.jobLotId; key = 'jobLotId'; }
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
        var cs = cols(), rate = num($('#prvExchangeRate').val());
        var html = '';
        S.rows.forEach(function (r, i) {
            r.fcyAmount = rate > 0 ? r.amount / rate : 0;           // txtExchangeRate_TextChanged
            html += '<tr data-i="' + i + '">' + cs.map(function (c) {
                if (c.k === 'x') return '<td style="text-align:center;padding:0;"><button type="button" class="prv-cbtn prv-del" data-i="' + i + '">X</button></td>';
                if (c.ed) { var e = cellEditor(c, r, i); if (e) return '<td' + (c.n ? ' class="num"' : '') + '>' + e + '</td>'; }
                var v = r[c.k];
                if (c.k === 'amount' || c.k === 'taxAmount') v = fmt(v);
                else if (c.k === 'fcyAmount') v = fmt3(v);
                else if (c.dt) v = dmy(day(v));
                return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $('#prvGrid tbody').html(html || '<tr><td colspan="' + cs.length + '" class="prv-empty"></td></tr>');
        var sumA = S.rows.reduce(function (s, r) { return s + r.amount; }, 0);
        var sumF = S.rows.reduce(function (s, r) { return s + r.fcyAmount; }, 0);
        $('#prvGrid tfoot').html(S.rows.length ? '<tr>' + cs.map(function (c) {
            return '<td class="num" style="font-weight:bold;">' + (c.sum === 1 ? fmt(sumA) : c.sum === 3 ? fmt3(sumF) : '') + '</td>';
        }).join('') + '</tr>' : '');
        // CalculateTotalInformation(): txtFcyAmount = SUM(FcyAmount) "#,##0.###", "0" with no rows
        $('#prvFcyAmount').val(S.rows.length ? fmt3(sumF) : '0');
    }

    // ------------------------------------------------------------------------------ totals / WHT

    function baseAmount() { return S.rows.reduce(function (s, r) { return s + r.amount; }, 0); }
    function setBox(id, v, raw) { $('#' + id).val(raw != null ? raw : fmt(v)).data('v', v); }
    function boxV(id) { var v = $('#' + id).data('v'); return v == null ? num($('#' + id).val()) : num(v); }

    /** Total() of each form. */
    function total() {
        if (S.pay) {
            var base = baseAmount(), rate = num($('#prvTaxPercent').val()), tax, tot;
            if ($('#prvRadExcluded').is(':checked')) { tax = base / (100 - rate) * rate; tot = base + tax; }
            else { var n = base; base = n / 100 * (100 - rate); tot = n; tax = tot - base; }
            showTotals(base, tax, tot);
            proportion();
        } else {
            var amt = baseAmount();
            if (amt > 0) {                                      // nothing changes when the grid sums to 0
                var t = boxV('prvTaxAmount');
                setBox('prvValue', round(amt, dec()));
                setBox('prvTotalAmount', round(amt + t, dec()));
            }
        }
    }

    function showTotals(value, tax, tot) {                      // payment: the boxes hold stringFormatsingle text
        setBox('prvValue', round(value, dec()));
        setBox('prvTaxAmount', round(tax, dec()));
        setBox('prvTotalAmount', round(tot, dec()));
    }

    /** TaxAmountProportion(): row TaxAmount only in Included mode (MidpointRounding.AwayFromZero). */
    function proportion() {
        var inc = $('#prvRadIncluded').is(':checked');
        var totalTax = round(boxV('prvTaxAmount'), dec()), totalDetail = baseAmount();
        S.rows.forEach(function (r) {
            r.taxAmount = (inc && totalTax > 0 && totalDetail > 0) ? round(totalTax / totalDetail * r.amount, dec()) : 0;
        });
        renderGrid();
    }

    function whtEnable(on) { $('#prvAgainstAc, #prvWithHoldingAc').prop('disabled', !on); }

    /** checkBox1_CheckedChanged() of each form (also CmbTaxType_Leave; receipt: txtValue.TextChanged). */
    function whtChanged() {
        var base = baseAmount();
        if (!$('#prvWht').is(':checked')) {
            whtEnable(false);
            $('#prvTaxPercent').val('0');
            if (S.pay) { showTotals(base, 0, base); proportion(); }
            else {
                setVal('prvWithHoldingAc', 0);
                setBox('prvTaxAmount', 0, '0');
                var v0 = boxV('prvValue');
                setBox('prvTotalAmount', round(v0, dec()));
            }
            return $.Deferred().resolve().promise();
        }
        return $.getJSON(API + '/tax-schedule', { taxTypeId: int($('#prvTaxType').val()), date: $('#prvDate').val() }).then(function (s) {
            if (!s || !s.found) {
                if (S.pay) {
                    whtEnable(false);
                    setVal('prvWithHoldingAc', 0); setVal('prvAgainstAc', 0);
                    $('#prvTaxPercent').val('0');
                    setBox('prvTaxAmount', 0, '0');
                    setBox('prvTotalAmount', boxV('prvValue'), $('#prvValue').val());   // txtTotalAmount.Text = txtValue.Text; no proportion
                } else {
                    setVal('prvWithHoldingAc', 0);
                    $('#prvTaxPercent').val('0');
                    setBox('prvTaxAmount', 0, '0');
                    whtEnable(true);                            // ReceiptsVoucherNew enables both combos after either branch
                }
                return;
            }
            var p = num(s.taxPercent);
            /* payment: taxPercent.ToString("#,##0.####"); receipt: Rows[0]["TaxPercent"].ToString() */
            $('#prvTaxPercent').val(S.pay ? p.toLocaleString('en-US', { maximumFractionDigits: 4 }) : String(p));
            if (!S.updateMode) setVal('prvWithHoldingAc', s.taxGLAccountId);
            if (S.pay) {
                var tax, tot, value = base;
                if ($('#prvRadExcluded').is(':checked')) { tax = round(base / (100 - p) * p, 2); tot = base + tax; }
                else { tot = base; value = round(tot / (1 + p / 100), 2); tax = tot - value; }
                showTotals(value, tax, tot);
                whtEnable(true);
                if (S.rows.length && int($('#prvAgainstAc').val()) === 0) setVal('prvAgainstAc', S.rows[0].accountId);
                proportion();
            } else {
                var v = boxV('prvValue');
                var t = v / (100 - p) * 100 * p / 100;
                setBox('prvTaxAmount', t, String(t));            // txtTaxAmount.Text = taxamount.ToString()
                setBox('prvTotalAmount', round(t + v, dec()));
                whtEnable(true);
            }
        }, function (x) { alert('Error applying withholding tax: ' + errMsg(x)); });
    }

    /** Receipt: txtValue.TextChanged -> checkBox1_CheckedChanged, only when the Amount text really changed. */
    function receiptValueChanged(before) {
        if (!S.pay && $('#prvValue').val() !== before) whtChanged();
    }

    // ------------------------------------------------------------------------------ detail entry

    function balance(accountId, $lbl) {
        if (!(accountId > 0)) { $lbl.hide().text('0'); return; }
        $.getJSON(API + '/balance', { accountId: accountId, date: $('#prvDate').val() }).then(function (r) {
            var b = Math.round(num(r && r.balance));            // "#,#;(#,#);0"
            $lbl.text(b < 0 ? '(' + Math.abs(b).toLocaleString('en-US') + ')' : b.toLocaleString('en-US')).show();
        });
    }

    /** CheqNoFill(): BPV only, only with "CheqBook Enabled"; the leaves of the bank, plus this voucher's own when editing. */
    function chequeFill() {
        S.cheques = [];
        $('#prvChequeList').empty();
        var bank = int($('#prvRefAccount').val());
        if (S.doc !== 2 || !S.F.chequeBookEnabled) return;
        $('#prvChequeNo').val('');
        if (!(bank > 0)) return;
        var seq = ++S.chqSeq;
        $.getJSON('/accounts/api/vouchers/outstanding-cheques', { bankId: bank, recId: S.recId }).then(function (list) {
            if (seq !== S.chqSeq) return;                                 // a later fill (another bank / voucher) wins
            S.cheques = list || [];
            $('#prvChequeList').html(S.cheques.map(function (c) { return '<option value="' + esc(c.cheqNo) + '"></option>'; }).join(''));
        });
    }
    function chequeIdFor(no) {
        for (var i = 0; i < S.cheques.length; i++) if (String(S.cheques[i].cheqNo) === String(no)) return int(S.cheques[i].id);
        /* update mode binds a one-row list holding the edited row's own leaf (grd_DoubleClick) */
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
        if (!outside.length) return 'Sequence contains no elements';      // Min() over an empty clone throws on the desktop
        var min3 = Math.min.apply(null, outside);
        if (n >= min && n < max && S.updIdx < 0) return 'Please Insert Cheque No In Detail Grid Serial Wise';
        if (n > min3) return 'Please Insert Cheque No In Detail Grid Serial Wise';
        return null;
    }

    /** FormValidationDetail() (payment: Add and Update share it). */
    function detailValidPay() {
        if (!$('#prvPaymentType').val()) return ['PaymentType Field is Required', 'prvPaymentType'];
        if (int($('#prvRefAccount').val()) === 0) return ['Credit Account Field is Required', 'prvRefAccount'];
        if (int($('#prvAccount').val()) === 0) return ['Debit Account Field is Required', 'prvAccount'];
        if (int($('#prvJobLot').val()) === 0) return ['Job/Lot Field is Required', 'prvJobLot'];
        if (S.doc === 2 && !$('#prvInstrument').val()) return ['Financial Instrument Field is Required', 'prvInstrument'];
        if (S.doc === 2 && int($('#prvInstrument').val()) === 1 && S.F.chequeNoCompulsoryOnBpv && chequeIdFor($('#prvChequeNo').val()) === 0) {
            return ['Cheque_number Field is Required', 'prvChequeNo'];
        }
        if ($.trim($('#prvAmount').val()) === '' || num($('#prvAmount').val()) === 0) return ['Amount Field is Required', 'prvAmount'];
        if (S.doc === 2 && int($('#prvInstrument').val()) === 1 && int($('#prvChequeType').val()) === 0) return ['Cheque Type Field is Required', 'prvChequeType'];
        if (S.F.branchFeature && int($('#prvBranch').val()) === 0) return ['BranchName Field is Required', 'prvBranch'];
        return null;
    }
    /** ReceiptsVoucherNew Add_Click_1 / btnUpdateDetail_Click checks (their wording differs). */
    function detailValidRec(update) {
        if (!$('#prvPaymentType').val()) return ['PaymentType Field Required', 'prvPaymentType'];
        if (int($('#prvAccount').val()) === 0) return [update ? 'Account Title Field Required' : 'Credit Account Field Required', 'prvAccount'];
        if (!$('#prvJobLot').val() || int($('#prvJobLot').val()) === 0) return ['Job/Lot Field Required', 'prvJobLot'];
        if ($.trim($('#prvAmount').val()) === '' || num($('#prvAmount').val()) === 0) return ['Amount Field Required', 'prvAmount'];
        if (S.F.branchFeature && int($('#prvBranch').val()) === 0) return ['BranchName Field is Required', 'prvBranch'];
        return null;
    }
    function fail(e) { alert(e[0]); comboFocus(e[1]); }

    function rowFromEntry() {
        var accOpt = $('#prvAccount option:selected');
        var r = {
            paymentTypeId: int($('#prvPaymentType').val()), paymentType: selText('prvPaymentType'),
            accountId: int($('#prvAccount').val()), accountCode: accOpt.attr('data-code') || '', accountTitle: accOpt.attr('data-title') || accOpt.text(),
            jobLotId: int($('#prvJobLot').val()), jobLot: selText('prvJobLot'),
            remarks: $('#prvLineRemarks').val(), amount: num($('#prvAmount').val()), taxAmount: 0,
            branchId: int($('#prvBranch').val()), branchName: selText('prvBranch'),
            costCenterId: int($('#prvCostCenter').val()), costCenterName: selText('prvCostCenter')
        };
        if (S.pay) {
            // Add_Click_1 always adds the (hidden on CPV) CheqDate picker's value.
            r.chequeDate = S.doc === 2 ? ($('#prvChequeDate').val() || today()) : today();
        }
        if (S.doc === 2) {
            r.financialInstrumentId = int($('#prvInstrument').val()); r.financialInstrument = selText('prvInstrument');
            r.chequeNo = $('#prvChequeNo').val(); r.chequeId = chequeIdFor(r.chequeNo);
            r.payeeTitle = $('#prvPayeeTitle').val(); r.chequeTypeId = int($('#prvChequeType').val());
        }
        return r;
    }

    function afterRowsChanged() {
        if (S.pay && S.rows.length && int($('#prvAgainstAc').val()) === 0) setVal('prvAgainstAc', S.rows[0].accountId);
    }

    /** Add_Click_1 of each form. */
    function add() {
        if (S.pay) {
            var e = detailValidPay();
            if (e) { fail(e); return; }
            if (S.doc === 2 && S.F.chequePostingSerialWise && int($('#prvInstrument').val()) === 1) {
                var se = chequeSerial($('#prvChequeNo').val());
                if (se) { alert(se); return; }
            }
            var r = rowFromEntry();
            S.rows.push(r);
            $('#prvAccBalance').hide();
            if (S.doc === 2 && r.chequeId > 0) $('#prvRefAccount').prop('disabled', true);   // CmbCheqNo.Value > 0
            renderGrid();
            resetDetail();
            afterRowsChanged();
        } else {
            var e2 = detailValidRec(false);
            if (e2) { fail(e2); return; }
            var before = $('#prvValue').val();
            S.rows.push(rowFromEntry());
            renderGrid();
            $('#prvAccBalance').hide();
            focusAccount();
            total();
            $('#prvAmount').val('');
            receiptValueChanged(before);
        }
    }

    /** btnUpdateDetail_Click of each form. */
    function updateDetail() {
        if (S.editIndex < 0) return;
        if (S.pay) {
            var e = detailValidPay();
            if (e) { fail(e); return; }
            if (S.doc === 2 && S.F.chequePostingSerialWise) {
                var typed = int($('#prvChequeNo').val());
                if (S.editCheque && S.editCheque.no > 0 && typed !== S.editCheque.no) {
                    var max = Math.max.apply(null, S.rows.map(function (r) { return int(r.chequeNo); }));
                    var next = S.rows[S.editIndex + 1];
                    if (next) {
                        if (S.editCheque.no < int(next.chequeNo)) { alert('You Cant Update Cheque No Serial To Other Serial No'); return; }
                    } else if (typed > max) { alert('You Cant Update Cheque No Serial To Other Serial No'); return; }
                }
                var se = chequeSerial($('#prvChequeNo').val());
                if (se) { alert(se); return; }
            }
            S.rows[S.editIndex] = rowFromEntry();
            renderGrid();
            resetDetail();
            afterRowsChanged();
        } else {
            var e2 = detailValidRec(true);
            if (e2) { fail(e2); return; }
            var before = $('#prvValue').val();
            S.rows[S.editIndex] = rowFromEntry();
            S.editIndex = -1;
            $('#prvAdd').show(); $('#prvUpdateDetail, #prvCancelDetail').hide();
            renderGrid();
            total();
            focusAccount();
            $('#prvAmount').val('');
            receiptValueChanged(before);
        }
    }

    /** btnCancelDetail_Click: payment = ResetDetail(); receipt hides Update/Cancel and clears the amount. */
    function cancelDetail() {
        if (S.pay) { resetDetail(); return; }
        S.editIndex = -1;
        $('#prvAdd').show(); $('#prvUpdateDetail, #prvCancelDetail').hide();
        $('#prvAmount').val('');
    }

    /** ResetDetail() (payment). */
    function resetDetail() {
        S.editIndex = -1;
        $('#prvAdd').show(); $('#prvUpdateDetail, #prvCancelDetail').hide();
        $('#prvAmount').val('');
        if (S.doc === 2) { $('#prvChequeNo').val(''); $('#prvPayeeTitle').val(''); }
        if (S.pay) total();
        focusAccount();
    }

    function focusAccount() { comboFocus('prvAccount'); }

    /** grd_DoubleClick (and Ctrl+Enter on the grid). */
    function editRow(i) {
        var r = S.rows[i]; if (!r) return;
        S.editIndex = i; S.updIdx = i;
        setVal('prvPaymentType', r.paymentTypeId); setVal('prvAccount', r.accountId); setVal('prvJobLot', r.jobLotId);
        if (S.doc === 2) {
            if (int(r.financialInstrumentId) > 0) setVal('prvInstrument', r.financialInstrumentId);
            $('#prvChequeDate').val(day(r.chequeDate) || today());
            if (int(r.chequeId) > 0 && String(r.chequeNo || '') !== '') S.editCheque = { id: int(r.chequeId), no: int(r.chequeNo) };
            $('#prvChequeNo').val(r.chequeNo || '');
            $('#prvPayeeTitle').val(r.payeeTitle || '');
            setVal('prvChequeType', r.chequeTypeId);
        }
        setVal('prvBranch', r.branchId);
        $('#prvLineRemarks').val(r.remarks);
        $('#prvAmount').val(r.amount);
        if (int(r.costCenterId) > 0) setVal('prvCostCenter', r.costCenterId);
        comboFocus('prvPaymentType');
        $('#prvAdd').hide(); $('#prvUpdateDetail, #prvCancelDetail').show();
    }

    /** grd_ColumnButtonClick "Delete" (and Ctrl+Space). */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (S.pay && S.F.chequePostingSerialWise) {
            var max = Math.max.apply(null, S.rows.map(function (x) { return int(x.chequeNo); }));
            if (int(r.chequeNo) !== max) { alert("You can't Delete This Row Beacuse This Entry Is Against Serial Wise Cheque No"); return; }
        }
        var before = $('#prvValue').val();
        S.rows.splice(i, 1);
        if (S.editIndex === i) cancelDetail();
        if (S.doc === 2) {
            var used = S.rows.reduce(function (s, x) { return s + (int(x.chequeId) > 0 ? int(x.chequeId) : 0); }, 0);
            if (!S.rows.length || used === 0) $('#prvRefAccount').prop('disabled', false);
        }
        renderGrid();
        total();
        receiptValueChanged(before);
    }

    // ------------------------------------------------------------------------------ reset / load

    function showButtons(mode) {         // 'save' | 'update' | 'saveas'  (Save.Visible / Update.Visible / btnSaveAs.Visible)
        var r = rights();
        $('#btnSave').toggle(mode === 'save').prop('disabled', !r.save);
        $('#btnUpdate').toggle(mode === 'update').prop('disabled', !r.update);
        $('#btnSaveAs').toggle(mode === 'saveas').prop('disabled', false);
    }

    function nextCode(first) {
        /* VoucherNofill: payment only while Save is visible AND enabled; receipt always. */
        if (S.pay && !rights().save) { $('#prvVoucherNo').val(''); return; }
        if (first && S.L && S.L.nextCode != null) { $('#prvVoucherNo').val(S.L.nextCode); return; }
        var seq = ++S.codeSeq;
        $.getJSON(API + '/' + S.doc + '/next-code').then(function (r) {
            if (seq === S.codeSeq && !S.updateMode) $('#prvVoucherNo').val(r.voucherCode);   // a voucher opened meanwhile keeps its code
        });
    }

    /** Reset() of each form (first = the Load-time state). The date, project, header account, job/lot,
        currency and rate are NOT touched by Reset() - only by Load. */
    function reset(first) {
        S.rows = []; S.recId = 0; S.updateMode = false; S.saveAs = false; S.editIndex = -1; S.updIdx = -1; S.editCheque = null;
        $('#prvRefAccount').prop('disabled', false);
        $('#prvFcyAmount').val('');
        $('#prvAccBalance').hide().text('0');
        $('#prvLineRemarks').val(''); $('#prvAmount').val('');
        $('#prvRemarks').val('');
        setVal('prvAgainstAc', 0);
        put('prvTaxType', 0);
        setBox('prvValue', 0, ''); setBox('prvTaxAmount', 0, ''); setBox('prvTotalAmount', 0, '');
        firstRow('prvPaymentType');                                // CmbPaymentType.Rows[0].Activate()
        setVal('prvAccount', 0);
        if (S.pay) {
            $('#prvPayeeTitle').val(''); setVal('prvChequeType', 0); $('#prvChequeNo').val('');
            setVal('prvWithHoldingAc', 0);
            $('#prvWht').prop('checked', false);
        } else {
            $('#prvHdrPayTitle').val(''); $('#prvHdrChequeNo').val('');
        }
        if (first) {
            $('#prvDate').val(today());
            firstRow('prvProject');                                // CmbProjectId.Rows[0].Activate()
            if (S.doc === 2) { firstRow('prvInstrument'); firstRow('prvChequeType'); $('#prvChequeDate').val(today()); }
            if (S.doc === 4) $('#prvHdrChequeDate').val(today());
            if (S.F.defaultBranchId) setVal('prvBranch', S.F.defaultBranchId);
            /* AccountTitleFill: CPV / CRV activate Rows[1] while Save is visible and enabled */
            if ((S.doc === 1 || S.doc === 3) && rights().save) {
                var o = $('#prvRefAccount option').eq(1);
                if (o.length) setVal('prvRefAccount', o.val());
            }
            defaults();
            $('#prvChkPrint1').prop('checked', true);
            $('#prvWht').prop('checked', false);
        }
        nextCode(first);
        showButtons('save');
        renderGrid();
        if (S.pay) { whtChanged(); resetDetail(); }               // ChkBoxWthHolding.Checked = false -> CheckedChanged; ResetDetail()
        else { total(); $('#prvAdd').show(); $('#prvUpdateDetail, #prvCancelDetail').hide(); }
        if (!first) { balance(int($('#prvRefAccount').val()), $('#prvRefBalance')); chequeFill(); }   // combcreditac_Leave
    }

    /** btnNew_Click: Reset(); payment also clears CmbCreditAccount; both hide the balance. */
    function newClick() {
        reset(false);
        if (S.pay) { setVal('prvRefAccount', 0); }
        $('#prvRefBalance').hide();
    }

    /** ReadById(ID, SaveAs) of each form. */
    function loadForEdit(id, saveAs) {
        return $.getJSON(API + '/' + S.doc + '/' + id, saveAs ? { saveAs: true } : {}).then(function (v) {
            if (!v) return;
            if (int(v.documentTypeId) !== S.doc) {                 // opened from a paired history -> the right screen
                w.location.href = '/accounts/vouchers/' + ROUTE[int(v.documentTypeId)] + '?id=' + id;
                return;
            }
            switchTab('form');
            S.recId = saveAs ? 0 : int(v.id); S.updateMode = true; S.saveAs = !!saveAs; S.codeSeq++;
            S.editIndex = -1; S.updIdx = -1;
            $('#prvVoucherNo').val(v.voucherCode);
            $('#prvDate').val(saveAs ? today() : day(v.voucherDate));
            setVal('prvRefAccount', v.refAccountId);
            $('#prvRemarks').val(v.remarks || '');
            setVal('prvProject', v.projectId);
            put('prvCurrency', v.multiCurrencyId);
            $('#prvExchangeRate').val(fmtRate(v.exchangeCurrencyRate));
            if (int(v.multiCurrencyId) === 0 || num(v.exchangeCurrencyRate) === 0) defaults();
            if (S.doc === 4) {
                $('#prvHdrPayTitle').val(saveAs ? '' : (v.payTitle || ''));
                $('#prvHdrChequeNo').val(saveAs ? '' : (v.chequeNo || ''));
                $('#prvHdrChequeDate').val(saveAs ? today() : (day(v.chequeDate) || today()));
            }
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
                if (wh.againstAcId != null) setVal('prvAgainstAc', wh.againstAcId);
                if (wh.withHoldingAcId != null) setVal('prvWithHoldingAc', wh.withHoldingAcId);
                if (wh.taxTypeId != null) put('prvTaxType', wh.taxTypeId);
                if (wh.taxPercent != null) $('#prvTaxPercent').val(String(wh.taxPercent));
                if (!S.pay) setBox('prvTaxAmount', num(wh.taxAmount), String(num(wh.taxAmount)));
            } else {
                $('#prvWht').prop('checked', false);
            }
            renderGrid();
            /* the CheckedChanged / textBox1_TextChanged chain, then Total() */
            if (S.pay) whtChanged().then(total);
            else { total(); whtChanged(); }
            chequeFill();
            showButtons(saveAs ? 'saveas' : 'update');
            if (!saveAs) $('#prvDate').focus();
        }, function (x) { alert(errMsg(x, 'Voucher not found')); });
    }

    // ------------------------------------------------------------------------------ save

    /** FormValidation(). */
    function formValid() {
        if (int($('#prvProject').val()) === 0) return ['Cost Center Field is Required', 'prvProject'];
        if (int($('#prvRefAccount').val()) === 0) return [S.pay ? 'Credit Account Field is Required' : 'Account Field is Required', 'prvRefAccount'];
        if (S.F.multiCurrencyFeature) {
            if (int($('#prvCurrency').val()) === 0) return ['Fcy Code Field is Required', 'prvCurrency'];
            if (num($('#prvExchangeRate').val()) === 0) return ['Exchange Rate Field is Required', 'prvExchangeRate'];
            if (num($('#prvFcyAmount').val()) === 0) return ['Fcy Amount Field is Required', 'prvFcyAmount'];
        } else {
            if (int($('#prvCurrency').val()) === 0) return ['Please Configure Your Base Currency In configurations', null];
            var t = $.trim($('#prvExchangeRate').val());
            if (t === '' || t === '0') return ['Please Configure Your Base Currency Rate In configurations', null];
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
            TaxPercent: $('#prvTaxPercent').val(), TaxAmount: boxV('prvTaxAmount'),
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

    /** Save_Click / btnSaveAs_Click (RecId = 0) and Update_Click -> Insert(). */
    function save(kind) {
        if (S.busy) return;
        var $b = kind === 'update' ? $('#btnUpdate') : kind === 'saveas' ? $('#btnSaveAs') : $('#btnSave');
        if (kind !== 'update') S.recId = 0;
        if (kind === 'update' && !(S.recId > 0)) { alert('Record Not Update  ' + S.recId); return; }
        if (!S.rows.length) { alert('Grid Record not found'); return; }
        var e = formValid();
        if (e) { alert(e[0]); if (e[1]) comboFocus(e[1]); return; }
        if (!confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var ack = [];
        var printCheque = S.pay && S.F.chequePrintingEnable && $('#cbPrintOnSave').is(':checked');
        var p1 = $('#prvChkPrint1').is(':checked'), p2 = $('#prvChkPrint2').is(':checked'), p3 = $('#prvChkPrint3').is(':checked');
        /* ChkPrint1 -> GenerateReport (102 slip); else ChkPrint2 -> AcRptPaymentReceiptsVoucherSlip_102; else ChkPrint3 -> 102-III */
        var printKind = p1 ? 1 : (p2 ? 2 : (p3 ? 3 : 0));
        var win = printKind && w.CrystalPrint ? w.CrystalPrint.reserve() : null;   // opened inside the click (popup blockers)
        S.busy = true; busyOn($b);
        (function post() {
            $.ajax({ url: API + '/' + S.doc + '/save', type: 'POST', contentType: 'application/json', headers: csrf(),
                     data: JSON.stringify(payload(ack)) })
                .done(function (res) {
                    S.busy = false; busyOff($b);
                    alert(res.message);
                    reset(false);
                    if (printKind) printDoc(printKind, res.id, S.doc, null, win);
                    if (printCheque) w.open('/accounts/banking/cheque-printing', '_blank');
                })
                .fail(function (x) {
                    var b = x.responseJSON || {};
                    if (x.status === 409 && b.confirm) {
                        if (confirm(b.message)) { ack.push(b.confirm); post(); return; }
                    } else {
                        alert(b.message || ('Save failed (' + x.status + ')'));
                    }
                    S.busy = false; busyOff($b);
                    if (win && w.CrystalPrint) w.CrystalPrint.release(win);
                });
        }());
    }

    // ------------------------------------------------------------------------------ prints / special rights

    /**
     * kind 1 = Print / GenerateReport -> CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102 (102 slip, key hrm-102)
     * kind 2 = btnPrintOld / btnPrint102Old / Print_102 -> AcRptPaymentReceiptsVoucherSlip_102(Id, DocumentTypeId) (key acc-102)
     * kind 3 = BtnPaymentAndReceiptVoucherSlip / PrintNew -> PaymentAndReceiptVoucherSlip(Id, DocumentTypeId) (102-III per type)
     */
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

    /* SpecialRightsImplement (Load / Refresh): RightId 2 -> ChkPrint1, 8 -> ChkPrint2, 9 -> ChkPrint3, Checked = IsActive. */
    function specialRights() {
        if (!w.SpecialRights) return;
        w.SpecialRights.mine(SCREEN[S.doc]).then(function (rows) {
            $.each(rows || [], function (i, r) {
                var rid = int(r.RightId != null ? r.RightId : r.rightId);
                var a = r.IsActive != null ? r.IsActive : r.isActive;
                a = a === true || a === 1 || a === '1' || String(a).toLowerCase() === 'true';
                var id = ({ 2: '#prvChkPrint1', 8: '#prvChkPrint2', 9: '#prvChkPrint3' })[rid];
                if (id) $(id).prop('checked', a);
            });
        }, function (x) { alert(errMsg(x, 'Could not load special rights.')); });
    }
    w.PRVSpecialRights = specialRights;

    // ------------------------------------------------------------------------------ shortcut keys (MakeShortCutKeys)

    var KEYS_PAY = [['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+U', 'For Update'], ['Ctrl+S', 'For Save'], ['Ctrl+E', 'For Close'],
        ['Alt+F1', 'For Open Payment By Invoice Voucher'], ['Alt+1', 'For Print I'], ['Alt+2', 'For Print II'], ['Ctrl+F5', 'For Focus on Doc Date'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var KEYS_REC = [['Ctrl+S', 'For Save in Form Tab And For Show History in History Tabs'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print I'], ['Alt+2', 'For Print II'], ['Alt+3', 'For Print III'],
        ['Ctrl+F5', 'For Focus on Voucher Type'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Payment Type in Detail Box'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() {
        if (!$('#prvKeys').length) {
            $('body').append('<div id="prvKeys" style="display:none;position:fixed;inset:0;z-index:3000;background:rgba(0,0,0,.3);">' +
                '<div style="position:absolute;left:50%;top:70px;transform:translateX(-50%);width:460px;max-width:95vw;background:#fff;border:1px solid #00796B;box-shadow:0 4px 16px rgba(0,0,0,.3);">' +
                '<div style="background:#00796B;color:#fff;padding:5px 8px;font-weight:bold;display:flex;justify-content:space-between;"><span>ShortCut Keys</span>' +
                '<a href="javascript:void(0)" id="prvKeysClose" style="color:#fff;">&#x2715;</a></div><div style="max-height:70vh;overflow:auto;">' +
                '<table class="win-grid-table"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody></tbody></table></div></div></div>');
            $('#prvKeysClose').on('click', function () { $('#prvKeys').hide(); });
            $('#prvKeys').on('click', function (e) { if (e.target === this) $(this).hide(); });
        }
        $('#prvKeys tbody').html((S.pay ? KEYS_PAY : KEYS_REC).map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''));
        $('#prvKeys').show();
    }

    // ------------------------------------------------------------------------------ amount / rate typing

    /** CommonServices.OnlytextdecimelFunction: digits, one '.', control keys. */
    function decimalOnly(e) {
        var k = e.key;
        if (e.ctrlKey || e.metaKey || e.altKey || !k || k.length > 1) return;
        if (/[0-9]/.test(k)) return;
        if (k === '.' && String(this.value).indexOf('.') < 0) return;
        e.preventDefault();
    }
    /** CommonServices.CommasApplyWhileTypingOnVouchers: thousands separators while typing, caret kept. */
    function commaTyping() {
        var el = this, v = el.value, pos = el.selectionStart || 0, before = v.length;
        var raw = v.replace(/,/g, '');
        if (raw === '' || raw === '.' || !/^\d*\.?\d*$/.test(raw)) return;
        var parts = raw.split('.');
        var ip = parts[0].replace(/^0+(?=\d)/, '').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var nv = ip + (parts.length > 1 ? '.' + parts[1] : '');
        if (nv !== v) { el.value = nv; var p = Math.max(0, pos + (nv.length - before)); try { el.setSelectionRange(p, p); } catch (x) { } }
    }

    /** cmbCurrency_Leave. */
    function currencyLeave() {
        var cur = int($('#prvCurrency').val());
        if (cur === 0) return;
        if (cur !== int(S.F.baseCurrencyId)) {
            $.getJSON(API + '/' + S.doc + '/last-rate', { currencyId: cur }).then(function (r) {
                var lr = num(r && r.lastRate);
                $('#prvExchangeRate').val(lr ? fmtRate(lr) : '0');
                renderGrid();                                   // txtExchangeRate_TextChanged
            }, function (x) { alert(errMsg(x)); });
        } else {
            $('#prvExchangeRate').val(fmtRate(S.F.baseCurrencyRate));
            renderGrid();
        }
    }

    // ------------------------------------------------------------------------------ History tab

    function hcols() {
        var c = [{ b: 'edit', t: 'Edit', w: 35 }, { b: 'p1', t: 'Print', w: 40 }, { b: 'p2', t: 'Print-II', w: 60 }, { b: 'p3', t: 'Print-III', w: 60 }, { b: 'saveas', t: 'Save As', w: 60 },
                 { k: 'documentType', t: 'V.Type' }, { k: 'voucherDate', t: 'V.Date' }, { k: 'voucherCode', t: 'V.No', link: 1 }];
        if (S.doc === 2 || S.doc === 4) c.push({ k: 'chequeNo', t: 'ChequeNo' });
        c.push({ k: 'accountTitle', t: 'AccountTitle' }, { k: 'voucherAmount', t: 'VoucherAmount', n: 1, sum: 1 });
        if (S.F.multiCurrencyFeature) c.push({ k: 'fcyCode', t: 'FcyCode' }, { k: 'exchangeRate', t: 'ExchangeRate', n: 2 }, { k: 'fcyAmount', t: 'FcyAmount', n: 3, sum: 3 });
        c.push({ k: 'remarks', t: 'Remarks', w: 350 }, { k: 'entryUser', t: 'EntryUser' }, { k: 'entryDate', t: 'EntryDate' },
               { k: 'modifyUser', t: 'ModifyUser' }, { k: 'modifyDate', t: 'ModifyDate' }, { k: 'approvedUser', t: 'ApprovedUser' },
               { k: 'approvedDate', t: 'ApprovedDate' }, { k: 'attachment', t: 'Attachment' }, { b: 'att', t: S.doc === 2 ? 'Add Attachment' : 'Add Attachments', w: S.doc === 1 ? 100 : 110 });
        return c;
    }

    /* History tab pages (Cpv / Bpv of PaymentVoucherNew, CrvTab / BrvTab of ReceiptsVoucherNew):
       toolStrip (Reset, Refresh; LoadAll is Visible = false) | teal title panel | Filters panel with the
       "Filters" GroupBox, the date-type radios and the (hidden until a Show) VouchersInfoBox | the
       history grid (Dock Top, designer height) | teal "Detail of above selected row" | detail grid (Fill). */
    var HL = {
        1: { title: 'Cash Payment Voucher History', th: 29, fh: 79, gw: 1081, ly: 19, fy: 19, cy: 38, ky: 36, from: [5, 134], to: [144, 130], fno: 278, tno: 382,
             acc: [488, 216], st: [708, 118], show: [831, 36], rIn: false, rad: [[903, 30], [903, 50], [984, 30], [984, 50]], info: [1090, 5, 229, 69], iv: [158, 67], gh: 198, dh: 25 },
        2: { title: 'Bank Payment Voucher History', th: 28, fh: 78, gw: 1075, ly: 22, fy: 23, cy: 41, ky: 39, from: [5, 138], to: [146, 132], fno: 281, tno: 384,
             acc: [488, 216], st: [707, 118], show: [829, 39], rIn: false, rad: [[898, 29], [898, 49], [979, 29], [979, 49]], info: [1087, 1, 238, 73], iv: [158, 67], gh: 220, dh: 26 },
        3: { title: 'Cash Receipt Voucher History', th: 29, fh: 78, gw: 1060, ly: 22, fy: 23, cy: 41, ky: 39, from: [4, 130], to: [140, 130], fno: 274, tno: 378,
             acc: [482, 210], st: [696, 118], show: [818, 39], rIn: false, rad: [[880, 33], [880, 53], [961, 33], [961, 53]], info: [1068, 4, 242, 69], iv: [158, 67], gh: 270, dh: 25 },
        4: { title: 'Bank Receipt Voucher History', th: 28, fh: 78, gw: 1064, ly: 22, fy: 23, cy: 41, ky: 39, from: [5, 130], to: [139, 130], fno: 273, tno: 374,
             acc: [476, 216], st: [694, 118], show: [814, 39], rIn: true, rad: [[871, 28], [871, 48], [952, 28], [952, 48]], info: [1069, 5, 242, 69], iv: [154, 80], gh: 258, dh: 26 }
    };

    function renderHistory() {
        var G = HL[S.doc], mss = S.pay ? '' : 'prv-cmb-mss10';
        var h = '<div class="prv-ts"><div class="prv-ts-in" style="border-bottom-color:#c5cbd3;">' +
                '<button type="button" class="prv-ts-btn" id="prvHReset"><i class="fa fa-undo" style="color:#2e7d32"></i><span><u>R</u>eset</span></button>' +
                '<button type="button" class="prv-ts-btn" id="prvHRefresh"><i class="fa fa-refresh" style="color:#2e7d32"></i><span>Refresh</span></button></div></div>';
        h += '<div class="prv-teal" style="height:' + G.th + 'px;margin-top:-3px;"><span style="left:4px;top:4px;">' + G.title + '</span></div>';
        var radios =
            '<label class="prv-rb" style="' + px(G.rad[0][0], G.rad[0][1]) + '"><input type="radio" name="prvHDateType" value="docdate" checked/>Doc Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[1][0], G.rad[1][1]) + '"><input type="radio" name="prvHDateType" value="entrydate"/>Entry Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[2][0], G.rad[2][1]) + '"><input type="radio" name="prvHDateType" value="modifydate"/>Modify Date</label>' +
            '<label class="prv-rb" style="' + px(G.rad[3][0], G.rad[3][1]) + '"><input type="radio" name="prvHDateType" value="approveddate"/>Approved Date</label>';
        var filt =
            lab(5, G.fy, 'From Date') +
            '<div class="prv-dtp" style="' + px(G.from[0], G.cy, G.from[1], 23) + '"><input type="checkbox" id="prvHFromChk" checked title="Use this date"/>' +
            '<input type="date" id="prvHFrom" class="prv-tb"/></div>' +
            lab(G.to[0], G.ly, 'To Date') +
            '<div class="prv-dtp" style="' + px(G.to[0], G.cy, G.to[1], 23) + '"><input type="checkbox" id="prvHToChk" checked title="Use this date"/>' +
            '<input type="date" id="prvHTo" class="prv-tb"/></div>' +
            lab(G.fno, G.ly, 'From Doc No') + tb('prvHFromNo', G.fno, G.cy, 100, 23, ' maxlength="9"', S.doc === 1 ? '' : 'prv-segb') +
            lab(G.tno, G.ly, 'To Doc No') + tb('prvHToNo', G.tno, G.cy, 100, 23, ' maxlength="9"', S.doc === 1 ? '' : 'prv-segb') +
            lab(G.acc[0], G.ly, 'Account Title') +
            '<select id="prvHAccount" class="prv-cmb ' + mss + '" data-dtcombo="single" style="position:absolute;' + px(G.acc[0], G.ky, G.acc[1], 26) + '"><option value="0"></option></select>' +
            lab(G.st[0], G.ly, 'Approved Status') +
            '<select id="prvHStatus" class="prv-cmb ' + mss + '" data-dtcombo="single" style="position:absolute;' + px(G.st[0], G.ky, G.st[1], 26) + '">' +
            '<option value="notapproved" selected>Not Apporved</option><option value="approved">Approved</option><option value="all">All</option></select>' +
            '<button type="button" class="prv-fbtn prv-show" id="prvHShow" style="' + px(G.show[0], G.show[1], 53, 26) + '">Show</button>' +
            (G.rIn ? radios : '');
        var info = '<div id="prvHInfo" class="prv-gb prv-info" style="display:none;' + px(G.info[0], G.info[1], G.info[2], G.info[3]) + '">' +
            lab(6, 11, 'Total Vouchers', 'prv-info') + '<span id="prvHTotal" class="prv-iv" style="' + px(G.iv[0], 11, G.iv[1], 17) + '">0</span>' +
            lab(6, 28, 'Approved Vouchers', 'prv-info') + '<span id="prvHApproved" class="prv-iv" style="' + px(G.iv[0], 28, G.iv[1], 17) + '">0</span>' +
            lab(6, 47, 'UnApproved Vouchers', 'prv-info') + '<span id="prvHUnApproved" class="prv-iv" style="' + px(G.iv[0], 47, G.iv[1], 17) + '">0</span></div>';
        h += '<div class="prv-abs" style="height:' + G.fh + 'px;">' + gbox('Filters', 5, 3, G.gw, 71, filt) + (G.rIn ? '' : radios) + info + '</div>';
        h += '<div class="prv-hgrid" style="height:' + G.gh + 'px;"><table class="prv-jg prv-auto" id="prvHGrid" tabindex="0"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        h += '<div class="prv-teal" style="height:' + G.dh + 'px;"><span style="left:3px;top:3px;">Detail of above selected row</span></div>';
        h += '<div class="prv-hdet"><table class="prv-jg prv-auto" id="prvHDetail"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>';
        $('#prvHistory').html(h);
        $('#prvHFrom, #prvHTo').val(today());                    // FromDate / ToDate = DateTime.Now, checked
        $('#prvHGrid thead').html('<tr>' + hcols().map(function (c) {
            return '<th' + (c.n ? ' class="num"' : '') + (c.w ? ' style="min-width:' + c.w + 'px;"' : '') + '>' + c.t + '</th>';
        }).join('') + '</tr>');
        wireHistory();
        historyAccounts();
    }

    /** ComboBindForXxxHistory. */
    function historyAccounts($b) {
        if ($b) busyOn($b);
        return $.getJSON(API + '/' + S.doc + '/history/accounts').then(function (list) {
            var cur = $('#prvHAccount').val();
            $('#prvHAccount').html('<option value="0"></option>' + (list || []).map(function (a) {
                return '<option value="' + esc(a.Id) + '">' + esc(a.name) + '</option>';
            }).join(''));
            if (cur) $('#prvHAccount').val(cur);
        }, function (x) { alert(errMsg(x, 'Could not load the accounts.')); }).always(function () { if ($b) busyOff($b); });
    }

    /** btnNewXxxHistory_Click. */
    function historyReset() {
        $('#prvHFrom, #prvHTo').val(today());
        $('#prvHFromNo, #prvHToNo').val('');
        $('#prvHAccount').val('0').trigger('change');
        $('#prvHStatus').val('notapproved');
        $('#prvHInfo').hide();
        S.hist.rows = []; S.hist.sel = -1;
        $('#prvHGrid tbody, #prvHGrid tfoot, #prvHDetail thead, #prvHDetail tbody, #prvHDetail tfoot').empty();
    }

    /** HistoryFillXxx. */
    function historyShow() {
        if (S.hist.busy) return;
        var $b = $('#prvHShow');
        var params = {
            dateType: $('input[name="prvHDateType"]:checked').val(),
            fromDate: $('#prvHFromChk').is(':checked') ? $('#prvHFrom').val() : '',
            toDate: $('#prvHToChk').is(':checked') ? $('#prvHTo').val() : '',
            fromDocNo: int($('#prvHFromNo').val()) || '', toDocNo: int($('#prvHToNo').val()) || '',
            accountId: int($('#prvHAccount').val()), approvedStatus: $('#prvHStatus').val()
        };
        S.hist.busy = true; busyOn($b);
        $.getJSON(API + '/' + S.doc + '/history', params).then(function (res) {
            var rows = (res && res.rows) || [];
            S.hist.rows = rows; S.hist.sel = -1;
            $('#prvHDetail thead, #prvHDetail tbody, #prvHDetail tfoot').empty();
            if (!rows.length) { $('#prvHInfo').hide(); $('#prvHGrid tbody, #prvHGrid tfoot').empty(); return; }
            $('#prvHInfo').show();
            $('#prvHTotal').text(res.totalVouchers || 0);
            $('#prvHApproved').text(res.totalApprovedVoucher || 0);
            $('#prvHUnApproved').text(res.totalUnApprovedVoucher || 0);
            var cs = hcols(), sumA = 0, sumF = 0, html = '';
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
            $('#prvHGrid tbody').html(html);
            $('#prvHGrid tfoot').html('<tr>' + cs.map(function (c) {
                return '<td class="num" style="font-weight:bold;">' + (c.sum === 1 ? fmt(sumA) : c.sum === 3 ? fmt3(sumF) : '') + '</td>';
            }).join('') + '</tr>');
            selectHistory(0);
        }, function (x) { alert(errMsg(x, 'Could not load the history.')); })
          .always(function () { S.hist.busy = false; busyOff($b); });
    }

    /** DataGridHistory_SelectionChanged -> VoucherDetailByHeaderId (and the bank variants). */
    function selectHistory(i) {
        var v = S.hist.rows[i]; if (!v) return;
        S.hist.sel = i;
        $('#prvHGrid tbody tr').removeClass('prv-sel').filter('[data-i="' + i + '"]').addClass('prv-sel');
        $.getJSON(API + '/' + S.doc + '/' + v.id + '/lines').then(function (r) {
            if (S.hist.sel !== i) return;
            renderLines(r);
        });
    }

    function renderLines(r) {
        var list = (r && r.rows) || [], F = S.F, cs, data;
        var inc = $('#prvRadIncluded').is(':checked');
        if (S.doc === 1 || S.doc === 3) list = list.slice().sort(function (a, b) { return a.id - b.id; });   // OrderBy(Id)
        var orig = (r && r.rows) || [];
        if (S.doc === 1) {
            cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['debit', 'Debit', 1], ['credit', 'Credit', 1],
                  F.multiCurrencyFeature && ['fcyD', 'Fcy Debit', 3], F.multiCurrencyFeature && ['fcyC', 'Fcy Credit', 3],
                  F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
            data = list.map(function (d) {
                return $.extend({}, d, { debit: (inc && d.debit > 0) ? d.debit + d.taxAmount : d.debit,      // radioButton1 of the FORM
                                         credit: (inc && d.credit > 0) ? d.credit + d.taxAmount : d.credit,
                                         fcyD: d.debit > 0 ? d.fcy : 0, fcyC: d.credit > 0 ? d.fcy : 0 });
            });
            if (data.reduce(function (s, d) { return s + num(d.taxAmount); }, 0) > 0) cs.push(['taxAmount', 'TaxAmount', 1]);
        } else if (S.doc === 2) {
            cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'Job/Lot'], ['remarks', 'Remarks'], ['debit', 'Debit', 1], ['credit', 'Credit', 1], ['chequeDate', 'ChequeDate'],
                  ['chequeNo', 'ChequeNo'], ['payeeTitle', 'PayeeTitle'], F.multiCurrencyFeature && ['fcyD', 'Fcy Debit', 3],
                  F.multiCurrencyFeature && ['fcyC', 'Fcy Credit', 3], F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
            data = list.filter(function (d) { return d.debit > 0 || d.credit > 0; }).map(function (d) {
                var addBack = d.isTaxable === 'False' && r.inclusiveTax;
                return $.extend({}, d, { debit: (addBack && d.debit > 0) ? d.debit + d.taxAmount : d.debit,
                                         credit: (addBack && d.credit > 0) ? d.credit + d.taxAmount : d.credit,
                                         fcyD: d.debit > 0 ? d.fcy : 0, fcyC: d.credit > 0 ? d.fcy : 0 });
            });
            if (data.reduce(function (s, d) { return s + num(d.taxAmount); }, 0) > 0) cs.push(['taxAmount', 'TaxAmount', 1]);
        } else if (S.doc === 3) {
            cs = [['accountCode', 'AccountCode'], ['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'],
                  ['jobLot', 'JobLot'], ['remarks', 'Remarks'], ['debit', 'Debit', 1], ['credit', 'Credit', 1],
                  F.multiCurrencyFeature && ['fcyD', 'Fcy Debit', 3], F.multiCurrencyFeature && ['fcyC', 'Fcy Credit', 3],
                  F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
            /* BranchName / CostCenterName come from the UNSORTED list at the same index, as the desktop reads them */
            data = list.map(function (d, i) {
                return $.extend({}, d, { fcyD: d.debit > 0 ? d.fcy : 0, fcyC: d.credit > 0 ? d.fcy : 0,
                                         branchName: orig[i] ? orig[i].branchName : '', costCenter: orig[i] ? orig[i].costCenter : '' });
            });
        } else {
            cs = [['accountTitle', 'AccountTitle'], F.subsidiaryFeature && ['subsidiaryAccount', 'SubsidiaryAccount'], ['jobLot', 'JobLot'],
                  ['remarks', 'Remarks'], ['credit', 'Credit', 1], ['debit', 'Debit', 1], F.multiCurrencyFeature && ['fcyC', 'Fcy Credit', 3],
                  F.multiCurrencyFeature && ['fcyD', 'Fcy Debit', 3], F.branchFeature && ['branchName', 'BranchName'], F.isBookingOffice && ['costCenter', 'CostCenter']];
            data = list.map(function (d) { return $.extend({}, d, { fcyD: d.debit > 0 ? d.fcy : 0, fcyC: d.credit > 0 ? d.fcy : 0 }); });
        }
        cs = cs.filter(Boolean);
        var sums = {};
        $('#prvHDetail thead').html('<tr>' + cs.map(function (c) { return '<th' + (c[2] ? ' class="num"' : '') + '>' + c[1] + '</th>'; }).join('') + '</tr>');
        $('#prvHDetail tbody').html(data.map(function (d) {
            return '<tr>' + cs.map(function (c) {
                var x = d[c[0]];
                if (c[2]) { sums[c[0]] = (sums[c[0]] || 0) + num(x); x = c[2] === 3 ? fmt3(x) : fmt(x); }
                return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(x) + '</td>';
            }).join('') + '</tr>';
        }).join(''));
        $('#prvHDetail tfoot').html('<tr>' + cs.map(function (c) {
            var s = c[2] && c[0] !== 'taxAmount' ? (c[2] === 3 ? fmt3(sums[c[0]] || 0) : fmt(sums[c[0]] || 0)) : '';
            return '<td class="num" style="font-weight:bold;">' + s + '</td>';
        }).join('') + '</tr>');
    }

    /** DataGridHistory_ColumnButtonClick. */
    function historyButton(b, i, btn) {
        var v = S.hist.rows[i]; if (!v) return;
        if (b === 'edit') { reset(false); loadForEdit(v.id); }                         // Reset(); ReadById(ID)
        else if (b === 'p1') printDoc(1, v.id, int(v.documentTypeId), btn, null);     // GenerateReport(ID, AccountId, Date)
        else if (b === 'p2') printDoc(2, v.id, S.pay ? int(v.documentTypeId) : 0, btn, null);   // receipt passes no DocumentTypeId
        else if (b === 'p3') printDoc(3, v.id, int(v.documentTypeId), btn, null);
        else if (b === 'saveas') { reset(false); loadForEdit(v.id, true); }            // DataGridHistory_SaveAs
    }

    function wireHistory() {
        $('#prvHReset').on('click', historyReset);
        $('#prvHRefresh').on('click', function () { historyAccounts($(this)); });     // btnRefreshXxxHistory_Click
        $('#prvHShow').on('click', historyShow);
        $('#prvHFromNo, #prvHToNo').on('keydown', function (e) {                       // OnlytextNumberFunction
            if (e.ctrlKey || e.metaKey || e.altKey || !e.key || e.key.length > 1) return;
            if (!/[0-9]/.test(e.key)) e.preventDefault();
        });
        $('#prvHGrid').on('click', 'tbody tr[data-i]', function () { var i = int($(this).data('i')); if (i !== S.hist.sel) selectHistory(i); });
        $('#prvHGrid').on('dblclick', 'tbody tr[data-i]', function () { var v = S.hist.rows[int($(this).data('i'))]; if (v) loadForEdit(v.id); });
        $('#prvHGrid').on('click', 'a.prv-vno', function (e) { e.stopPropagation(); var v = S.hist.rows[int($(this).data('i'))]; if (v) loadForEdit(v.id); });
        $('#prvHGrid').on('click', 'button.prv-hb', function (e) { e.stopPropagation(); historyButton($(this).data('b'), int($(this).data('i')), this); });
        $('#prvHGrid').on('keydown', function (e) {                                    // DataGridHistory_KeyDown
            var n = S.hist.rows.length; if (!n) return;
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                e.preventDefault();
                var i = S.hist.sel < 0 ? 0 : S.hist.sel + (e.key === 'ArrowDown' ? 1 : -1);
                if (i >= 0 && i < n) selectHistory(i);
            } else if (e.ctrlKey && e.key === 'Enter') {
                e.preventDefault(); e.stopPropagation();
                var v = S.hist.rows[S.hist.sel]; if (v) { reset(false); loadForEdit(v.id); }
            }
        });
    }

    // ------------------------------------------------------------------------------ tabs / keys / wiring

    function switchTab(tab) {
        if (tab === 'form') {
            $('#tabBtnForm').addClass('active'); $('#tabBtnHistory').removeClass('active');
            $('#tabFormContent').show(); $('#tabHistoryContent').hide();
        } else {
            $('#tabBtnHistory').addClass('active'); $('#tabBtnForm').removeClass('active');
            $('#tabFormContent').hide(); $('#tabHistoryContent').show();
            $('#prvHFrom').focus();                                                    // tabControl1_SelectedIndexChanged
        }
    }
    function onHistory() { return $('#tabHistoryContent').is(':visible'); }

    /** Enter = SendKeys("{TAB}") (form KeyDown, grids' KeyDown). */
    function focusNext(from) {
        var $all = $('#tabFormContent, #tabHistoryContent').filter(':visible')
            .find('input,select,textarea,button').filter(':visible').filter(function () {
                return !this.disabled && (!this.readOnly || $(this).hasClass('dtcombo-input')) && this.type !== 'hidden' && this.tabIndex >= 0;
            });
        var i = $all.index(from);
        if (i >= 0 && i + 1 < $all.length) $all.eq(i + 1).focus();
    }

    function onKey(e) {
        if ($('#srModal').is(':visible') || $('#prvKeys').is(':visible')) return;
        var k = e.key || '', K = k.toUpperCase(), r = rights();
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey) {
            var t = e.target;
            if (t && /^(INPUT|SELECT|TEXTAREA)$/.test(t.tagName) && t.type !== 'button' && t.type !== 'submit') {
                if ($(t).closest('#prvGrid').length) { e.preventDefault(); $(t).trigger('change'); return; }   // commit the cell
                e.preventDefault(); focusNext(t);
            }
            return;
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { shortcuts(); return; }
        if (e.ctrlKey && !e.altKey) {
            if (K === 'S') { e.preventDefault(); if (onHistory()) historyShow(); else if ($('#btnSave').is(':visible') && !$('#btnSave').prop('disabled')) save('save'); }
            else if (K === 'U') { e.preventDefault(); if ($('#btnUpdate').is(':visible') && !$('#btnUpdate').prop('disabled')) save('update'); }
            else if (K === 'N') { e.preventDefault(); if (onHistory()) historyReset(); else newClick(); }
            else if (K === 'E') { e.preventDefault(); w.location.href = '/accounts/dashboard'; }
            else if (K === 'T') { e.preventDefault(); if (onHistory()) { switchTab('form'); comboFocus('prvRefAccount'); } else switchTab('history'); }
            else if (K === 'R') { e.preventDefault(); if (onHistory()) historyAccounts($('#prvHRefresh')); else refreshLists($('#btnRefresh')); }
            else if (k === 'F5') { e.preventDefault(); comboFocus('prvRefAccount'); }   // CmbVoucherType disabled -> credit account
            else if (k === 'F10') { e.preventDefault(); attachments(); }
            else if (k === 'ArrowDown') { e.preventDefault(); (onHistory() ? $('#prvHGrid') : $('#prvGrid')).focus(); }
            else if (k === 'ArrowUp') { e.preventDefault(); if (onHistory()) $('#prvHFrom').focus(); else comboFocus('prvPaymentType'); }
            return;
        }
        if (e.altKey && !e.ctrlKey) {
            if (k === 'F1' && S.pay) { e.preventDefault(); w.location.href = '/accounts/vouchers/payment-by-invoice'; }
            else if ((k === '1' || e.code === 'Digit1' || e.code === 'Numpad1') && r.print) { e.preventDefault(); printButton(1, $('#btnPrint')[0]); }
            else if ((k === '2' || e.code === 'Digit2' || e.code === 'Numpad2') && r.print) { e.preventDefault(); printButton(2, $('#btnPrint2')[0]); }
            else if ((k === '3' || e.code === 'Digit3' || e.code === 'Numpad3') && r.print) { e.preventDefault(); printButton(3, $('#btnPrint3')[0]); }
        }
    }

    function attachments() { alert('Attachments are not available on the web form yet.'); }

    function wireForm() {
        $('#prvRefAccount').on('change', function () {
            var id = int($(this).val());                                    // combcreditac_Leave
            if (id > 0) balance(id, $('#prvRefBalance')); else $('#prvRefBalance').hide().text('0');
            chequeFill();
        });
        $('#prvAccount').on('change', function () {                         // combactitle_Leave
            var id = int($(this).val());
            if (id > 0) balance(id, $('#prvAccBalance')); else $('#prvAccBalance').hide().text('0');
        });
        $('input[name="prvAccMode"]').on('change', function () {           // rdSearchByAccountName_CheckedChanged
            S.accMode = $(this).val();
            var cur = $('#prvAccount').val();
            $('#prvAccount').attr('data-dtcombo', S.accMode === 'code' ? 'prvAcc4c' : 'prvAcc4')
                .attr('data-dtcombo-caption', S.accMode === 'code' ? 'Account Code' : (S.pay ? 'Account Title' : 'Credit Account'))
                .html(accOpts(S.L ? S.L.detailAccounts : [], true, S.accMode));
            if (int(cur) > 0) { $('#prvAccount').val(cur).trigger('change'); focusAccount(); }
        });
        $('#prvExchangeRate').on('keydown', decimalOnly)
            .on('input', function () { renderGrid(); })                     // txtExchangeRate_TextChanged
            .on('blur', function () { $(this).val(fmtRate($(this).val())); renderGrid(); });   // txtExchangeRate_Leave
        $('#prvCurrency').on('change', currencyLeave);
        $('#prvAmount').on('keydown', decimalOnly).on('input', commaTyping);
        $('#prvLineRemarks').on('mousedown', function () { S.remarksMouse = true; })
            .on('focus', function () { var el = this; if (!S.pay || !S.remarksMouse) setTimeout(function () { el.select(); }, 0); })
            .on('blur', function () { S.remarksMouse = false; });           // txtremarks_Enter / _MouseClick / _Leave
        $('#prvAdd').on('click', add);
        $('#prvUpdateDetail').on('click', updateDetail);
        $('#prvCancelDetail').on('click', cancelDetail);
        $('#prvGrid').on('click', '.prv-del', function (ev) { ev.stopPropagation(); deleteRow(int($(this).data('i'))); });
        $('#prvGrid').on('click', 'tbody tr[data-i]', function () { $('#prvGrid tbody tr').removeClass('prv-sel'); $(this).addClass('prv-sel'); });
        $('#prvGrid').on('dblclick', 'tbody tr[data-i]', function (ev) { if ($(ev.target).is('input,select')) return; editRow(int($(this).data('i'))); });
        $('#prvGrid').on('keydown', function (e) {                          // grd_KeyDown
            var sel = $('#prvGrid tbody tr.prv-sel'); if (!sel.length) sel = $('#prvGrid tbody tr[data-i]').first();
            if (!sel.length) return;
            var i = int(sel.data('i'));
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); editRow(i); }
            else if (e.ctrlKey && e.key === ' ') { e.preventDefault(); deleteRow(i); }
            else if (!e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowUp') && !$(e.target).is('select')) {
                var n = S.rows.length, j = i + (e.key === 'ArrowDown' ? 1 : -1);
                if (j >= 0 && j < n) { e.preventDefault(); $('#prvGrid tbody tr').removeClass('prv-sel').filter('[data-i="' + j + '"]').addClass('prv-sel'); }
            }
        });
        /* in-cell edits: grd_UpdatingCell (numeric check) -> CellUpdated / CellEdited -> Total() (+ receipt fcy) */
        $('#prvGrid').on('keydown', 'input.prv-ce[data-k="amount"]', function (e) {
            if (e.key === '.') e.preventDefault();                          // GridEX_Amount_KeyPress
        });
        $('#prvGrid').on('change', '.prv-ce', function () {
            var i = int($(this).data('i')), k = $(this).data('k'), r = S.rows[i]; if (!r) return;
            var before = $('#prvValue').val();
            if (k === 'amount') {
                var v = $.trim($(this).val()).replace(/,/g, '');
                if (v === '' || isNaN(Number(v))) { alert('Please Type Only Numeric Value'); $(this).val(r.amount); return; }
                r.amount = Number(v);
            } else if (k === 'remarks') r.remarks = $(this).val();
            else {
                r[k] = int($(this).val());
                var txt = $(this).find('option:selected').text();
                if (k === 'paymentTypeId') r.paymentType = txt; else if (k === 'jobLotId') r.jobLot = txt; else if (k === 'costCenterId') r.costCenterName = txt;
                return;
            }
            renderGrid(); total(); receiptValueChanged(before);
        });
        $('#prvWht').on('change', whtChanged);                               // checkBox1_CheckedChanged
        $('#prvTaxType').on('change', function () { whtChanged(); });        // CmbTaxType_Leave
        $('input[name="prvTaxMode"]').on('change', function () { total(); });   // RadExcluded_CheckedChanged -> Total() + TaxAmount column
        $('#prvBtnHistory').on('click', function () { switchTab('history'); });
    }

    function wireToolbar() {
        $('#btnNew').on('click', newClick);
        $('#btnRefresh').on('click', function () { refreshLists($(this)); });
        $('#btnSave').on('click', function () { save('save'); });
        $('#btnUpdate').on('click', function () { save('update'); });
        $('#btnSaveAs').on('click', function () { save('saveas'); });
        $('#btnAttachment').on('click', attachments);
        $('#btnPrint').on('click', function () { printButton(1, this); });
        $('#btnPrint2').on('click', function () { printButton(2, this); });
        $('#btnPrint3').on('click', function () { printButton(3, this); });
        $('#btnShortcuts').on('click', shortcuts);
        $('#btnSpecialRights').on('click', function () { if (w.SpecialRights) w.SpecialRights.open(SCREEN[S.doc]); });
        $('#tabBtnForm').on('click', function () { switchTab('form'); });
        $('#tabBtnHistory').on('click', function () { switchTab('history'); });
        $(document).on('keydown', onKey);
    }

    w.PRV = {
        init: function (cfg) {
            S.doc = int(cfg.doc); S.pay = S.doc === 1 || S.doc === 2;
            wireToolbar();
            var ready = $.getJSON(API + '/' + S.doc + '/lookups').then(function (L) {
                bindLookups(L);
                renderHistory();
            }, function (x) {
                alert(errMsg(x, 'Could not load the form lists.'));
            });
            var q = new URLSearchParams(w.location.search);
            var id = int(q.get('id') || q.get('Id'));
            var code = q.get('voucherCode') || q.get('VoucherCode') || q.get('fromDocNo');
            ready.then(function () {
                specialRights();
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
        save: function () { save($('#btnUpdate').is(':visible') ? 'update' : 'save'); },
        reset: newClick,
        print: function () { printButton(1, $('#btnPrint')[0]); },
        switchTab: switchTab
    };
    w.switchTab = switchTab;
}(window, jQuery));
