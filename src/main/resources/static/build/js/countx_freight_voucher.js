/* ============================================================================================
 * Freight Payment Voucher — Architecture.WinApp.Account_Definition.FreightVoucher
 * (FreightVoucher.cs), DocumentTypeId 28, ScreenName "FreightVoucher", with its pop-up
 * FreightVoucherPaymentBreakUp.cs. Each function names the desktop method it reproduces.
 *
 * The desktop form is a gate-pass driven voucher: nothing is typed into the detail grid from
 * scratch. A row is LOADED from "Pending Records" (usp_getGpDataForFreightVoucher), the gate
 * pass fields fill from it, and only Discount / Other Deduction / Deduction / Remarks (and, for
 * 105/106 gate passes with no rate, Shortage) are editable — grd_CellUpdated keeps the rest in
 * step. Saving posts one freight voucher plus its accounting voucher (Dr freight account, Cr the
 * credit account or the payment break-up rows).
 *
 * Print buttons (241-Slip, 102-Voucher, 240-Register) produce browser output of the SAME
 * procedures the Crystal reports read; the .rpt layouts themselves cannot be rendered here.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/accounts/api/freight';

    /* ------------------------------------------------------------------ state */
    var cfg = { DefaultFreightVoucherCreditAccountId: 0, DefaultDaysToLessFromHistoryFromDate: 0,
                TolerancePercentForDiscountonFreightVoucher: 0, IncludeBankAccountsInFreightVoucherCreditAccount: false,
                ChequeBookEnableStatus: false };
    var rights = { canSave: false, canUpdate: false, canPrint: false };
    var accounts = [], cities = [], drivers = [], instrumentTypes = [];
    var table = [];                 // grd's DataTable
    var dtFreightPaymentBreakUp = [];
    var pendingRows = [], previousRows = [], dtRegister = [];
    var RecId = 0, VoucherHeadId = 0, DriverBioId = 0;
    var gridSet = { refDocumentTypeId: 0, rate: 0, customGroupId: 0 };
    var currentTab = 'tabForm';
    var buUpdateIndex = -1;

    /* ------------------------------------------------------------------ helpers */
    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : String(v); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var lower = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === lower) return row[k];
        return '';
    }
    function str(v) { return (v === null || v === undefined) ? '' : String(v); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { return roundHalfEven(num(v)); }
    /* Math.Round / Convert.ToInt32 — banker's rounding. */
    function roundHalfEven(x) {
        var r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r;
    }
    /* Conversion.ToString(double) — C# prints integers without a point and floats shortest. */
    function cs(v) {
        if (v === '' || v === null || v === undefined) return '';
        var n = Number(v);
        if (isNaN(n)) return str(v);
        return String(Math.round(n * 1e10) / 1e10);
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoDay(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function parseDate(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(str(v));
        return m ? new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0)) : null;
    }
    function dayOf(v) { var d = parseDate(v); return d ? isoDay(d) : ''; }
    function shortDate(v) { var d = parseDate(v); return d ? pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear() : str(v); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function longDateTime(v) {
        var d = parseDate(v); if (!d) return str(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return pad(d.getDate()) + '-' + MON[d.getMonth()] + '-' + d.getFullYear() + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + ap;
    }
    function group(intStr) { return intStr.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* "#,##0.###"-style: grouped, up to `dec` decimals, trailing zeros dropped. */
    function fmt(v, dec) {
        if (v === '' || v === null || v === undefined) return '';
        var n = Number(v); if (!isFinite(n)) return '';
        var neg = n < 0; n = Math.abs(n);
        var s = n.toFixed(dec === undefined ? 2 : dec);
        var parts = s.split('.');
        var frac = parts[1] ? parts[1].replace(/0+$/, '') : '';
        return (neg ? '-' : '') + group(parts[0]) + (frac ? '.' + frac : '');
    }
    /* "#,#;(#,#);0" */
    function fmtBal(v) {
        var n = Number(v) || 0;
        if (n === 0) return '0';
        var r = Math.round(Math.abs(n));
        var s = r === 0 ? '' : group(String(r));
        return n < 0 ? '(' + s + ')' : s;
    }

    function fetchJson(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opts || {}))
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
                    if (!r.ok) throw new Error(body && body.message ? body.message : ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }
    function get(path, params) { return fetchJson(api + path + (params ? '?' + new URLSearchParams(params).toString() : '')); }
    function post(path, body) {
        return fetchJson(api + path, { method: 'POST', headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
    }

    /* ------------------------------------------------------------------ selects */
    function bindSelect(id, rows, valueKey, textKey, opts) {
        opts = opts || {};
        var keep = opts.retain === false ? '' : val(id);
        var h = opts.blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            (opts.attrs || []).forEach(function (a) { extra += ' data-' + a[0] + '="' + esc(ci(r, a[1])) + '"'; });
            h += '<option value="' + esc(ci(r, valueKey)) + '"' + extra + '>' + esc(ci(r, textKey)) + '</option>';
        });
        $id(id).innerHTML = h;
        /* InfragisticsHelper.BindAndRetainSelection — keep the value when it is still in the list */
        if (keep !== '') { $id(id).value = keep; if ($id(id).value !== keep) $id(id).value = ''; }
    }
    function selectValue(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = ''; }
    function selectedText(id) {
        var s = $id(id); if (!s || s.selectedIndex < 0) return '';
        var o = s.options[s.selectedIndex]; return o && o.value !== '' ? o.textContent : '';
    }
    function selectedAttr(id, attr) {
        var s = $id(id); if (!s || s.selectedIndex < 0) return '';
        var o = s.options[s.selectedIndex]; return o ? (o.getAttribute('data-' + attr) || '') : '';
    }
    function focusCtl(id) {
        var e = $id(id); if (!e) return;
        var w = e.closest ? e.closest('.dtcombo-wrap') : null;
        var t = w ? w.querySelector('input') : e;
        if (t && t.focus) t.focus();
    }
    /* accounts de-duplicated by ChartOfAccountId after a filter, as every desktop binder does */
    function distinctAccounts(pred) {
        var seen = {}, out = [];
        accounts.forEach(function (a) {
            if (!pred(a)) return;
            if (seen[a.Id]) return;
            seen[a.Id] = true; out.push(a);
        });
        return out;
    }

    /* ------------------------------------------------------------------ masked text boxes */
    function maskApply(el) {
        var mask = el.getAttribute('data-mask'); if (!mask) return;
        var digits = el.value.replace(/\D/g, '');
        var out = '', di = 0;
        for (var i = 0; i < mask.length && di < digits.length; i++) {
            if (mask[i] === '0') out += digits[di++];
            else out += mask[i];
        }
        el.value = out;
    }
    function maskFull(el) {
        var mask = el.getAttribute('data-mask') || '';
        var need = (mask.match(/0/g) || []).length;
        return el.value.replace(/\D/g, '').length === need;
    }
    function initMasks() {
        Array.prototype.forEach.call(document.querySelectorAll('input[data-mask]'), function (el) {
            el.addEventListener('input', function () { maskApply(el); });
            el.setAttribute('maxlength', el.getAttribute('data-mask').length);
        });
    }

    /* ------------------------------------------------------------------ grids */
    /* Column spec: key, cap, w, t ('num'|'int'|'text'|'date'|'dt'|'btn'), dec, sum, edit, cls */
    function renderGrid(hostId, cols, rows, opts) {
        opts = opts || {};
        var h = '<table class="g"><thead><tr>';
        cols.forEach(function (c) { h += '<th style="min-width:' + (c.w || 70) + 'px;">' + esc(c.cap) + '</th>'; });
        h += '</tr></thead><tbody>';
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">';
            cols.forEach(function (c) { h += cell(c, r, i, opts); });
            h += '</tr>';
        });
        h += '</tbody>';
        if (opts.totals && rows.length) {
            h += '<tfoot><tr>';
            cols.forEach(function (c) {
                if (c.sum) {
                    var s = 0; rows.forEach(function (r) { s += num(r[c.key]); });
                    h += '<td>' + esc(fmt(s, c.dec)) + '</td>';
                } else h += '<td></td>';
            });
            h += '</tr></tfoot>';
        }
        h += '</table>';
        $id(hostId).innerHTML = h;
        if (opts.rec) $id(opts.rec).textContent = 'Record: ' + (rows.length ? 1 : 0) + ' Of ' + rows.length;
    }
    function cell(c, r, i, opts) {
        var v = r[c.key];
        if (c.t === 'btn') return '<td class="c"><button type="button" class="gbtn" data-act="' + c.key + '" data-i="' + i + '">' + esc(c.cap) + '</button></td>';
        if (c.edit && opts.editable) {
            if (c.t === 'combo') {
                var o = '<option value=""></option>';
                (c.options || []).forEach(function (a) { o += '<option value="' + a.Id + '"' + (String(a.Id) === String(v) ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>'; });
                return '<td class="ed t"><select data-k="' + c.key + '" data-i="' + i + '">' + o + '</select></td>';
            }
            return '<td class="ed' + (c.t === 'text' ? ' t' : '') + '"><input type="text" data-k="' + c.key + '" data-i="' + i + '" value="' + esc(c.t === 'text' ? str(v) : cs(v)) + '"></td>';
        }
        var cls = c.cls || '', txt;
        if (c.t === 'num') { cls += ' n'; txt = fmt(v, c.dec); }
        else if (c.t === 'int') { cls += ' c'; txt = str(v); }
        else if (c.t === 'date') txt = shortDate(v);
        else if (c.t === 'dt') txt = longDateTime(v);
        else if (c.t === 'combo') { var a = accounts.filter(function (x) { return String(x.Id) === String(v); })[0]; txt = a ? a.AccountTitle : ''; }
        else txt = str(v);
        return '<td class="' + cls + '">' + esc(txt) + '</td>';
    }

    /* grdSettings (:795) — which detail columns show and which can be edited */
    function detailCols() {
        var tol = num(cfg.TolerancePercentForDiscountonFreightVoucher) !== 0;
        var shortageEdit = (gridSet.refDocumentTypeId === 105 || gridSet.refDocumentTypeId === 106) && gridSet.rate === 0;
        var cols = [];
        if (gridSet.customGroupId > 0) {
            var grp = gridSet.customGroupId;
            cols.push({ key: 'DebitAccountId', cap: 'Debit Account', w: 160, t: 'combo', edit: true,
                        options: distinctAccounts(function (a) { return a.CustomGroupId === grp; }) });
        } else {
            cols.push({ key: 'AccountTitle', cap: 'Account Title (Dr)', w: 160, t: 'text' });
        }
        cols.push(
            { key: 'BiltyFreight', cap: 'Bilty Freight', w: 70, t: 'num', dec: 2, sum: true },
            { key: 'FreightPer100Kg', cap: 'Freight Per100Kg', w: 70, t: 'num', dec: 2, sum: true },
            { key: 'FreightPerMton', cap: 'Freight Per Mton', w: 70, t: 'num', dec: 2, sum: true },
            { key: 'WbCharges', cap: 'Scale Charges', w: 65, t: 'num', dec: 3, sum: true },
            { key: 'AdvanceByParty', cap: 'Advance By Supplier', w: 80, t: 'num', dec: 3, sum: true },
            { key: 'AdvanceByFactory', cap: 'Advance By Factory', w: 80, t: 'num', dec: 3, sum: true },
            { key: 'OtherDeduction', cap: 'Other Deduction', w: 80, t: 'num', dec: 3, sum: true, edit: true },
            { key: 'TotalFreight', cap: 'Total Freight', w: 80, t: 'num', dec: 2, sum: true },
            { key: 'ShortageAmount', cap: 'Shortage Amount', w: 80, t: 'num', dec: 2, sum: true, edit: shortageEdit },
            { key: 'DiscountAmount', cap: 'Discount Amount', w: 80, t: 'num', dec: 2, sum: true, edit: tol },
            { key: 'RemainingAmount', cap: 'Remaining Amount', w: 80, t: 'num', dec: 2, sum: true },
            { key: 'DeductionAmount', cap: 'Deduction From Freight', w: 80, t: 'num', dec: 2, sum: true, edit: true },
            { key: 'ChargeToParty', cap: 'Charge To Supplier', w: 80, t: 'num', dec: 3, sum: true },
            { key: 'PaidAmount', cap: 'Net Payable Amount', w: 80, t: 'num', dec: 2, sum: true, cls: 'pay' },
            { key: 'Remarks', cap: 'Remarks', w: 110, t: 'text', edit: true },
            { key: 'VarietyName', cap: 'Item Name', w: 100, t: 'text' },
            { key: 'ItemQty', cap: 'Item Qty', w: 60, t: 'num', dec: 3, sum: true },
            { key: 'RateForShortage', cap: 'Rate For Shortage / 40Kg', w: 90, t: 'num', dec: 2 }
        );
        return cols;
    }
    function renderDetail() {
        renderGrid('grd', detailCols(), table, { totals: true, editable: true, rec: 'recDetail' });
    }

    var PENDING_COLS = [
        { key: 'Load', cap: 'Load', w: 40, t: 'btn' },
        { key: 'GpDate', cap: 'Gp Date', w: 75, t: 'date' },
        { key: 'GpNo', cap: 'Gp No', w: 50, t: 'int' },
        { key: 'VehicleNo', cap: 'Vehicle No', w: 80, t: 'text' },
        { key: 'BiltyNo', cap: 'Bilty No', w: 70, t: 'text' },
        { key: 'BiltyFreight', cap: 'Bilty Freight', w: 70, t: 'num', dec: 2, sum: true },
        { key: 'WbCharges', cap: 'Wb Charges', w: 65, t: 'num', dec: 3, sum: true },
        { key: 'AdvanceByParty', cap: 'Advance By Party', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'AdvanceByFactory', cap: 'Advance By Factory', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'TotalFreight', cap: 'Total Freight', w: 75, t: 'num', dec: 2, sum: true },
        { key: 'ItemQty', cap: 'Item Qty', w: 60, t: 'num', dec: 3, sum: true },
        { key: 'SupplierWeight', cap: 'Supplier Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'NetWbWeight', cap: 'Net Wb Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'ShortWeight', cap: 'Short Weight', w: 65, t: 'num', dec: 3, sum: true },
        { key: 'AllowShortWeight', cap: 'Allow Short Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'ShortageApplyWeight', cap: 'Shortage Apply Weight', w: 75, t: 'num', dec: 3, sum: true },
        { key: 'ShortageAmount', cap: 'Shortage Amount', w: 75, t: 'num', dec: 2, sum: true },
        { key: 'Status', cap: 'Status', w: 80, t: 'text' },
        { key: 'CityName', cap: 'City Name', w: 100, t: 'text' },
        { key: 'VarietyName', cap: 'Item Name', w: 150, t: 'text' },
        { key: 'SupplierName', cap: 'Supplier Name', w: 150, t: 'text' },
        { key: 'AccountTitle', cap: 'Account Title', w: 150, t: 'text' },
        { key: 'OrderRate', cap: 'Order Rate', w: 70, t: 'num', dec: 2 },
        { key: 'Remarks', cap: 'Remarks', w: 120, t: 'text' }
    ];
    var PREV_COLS = [
        { key: 'GpDate', cap: 'Gp Date', w: 70, t: 'date' },
        { key: 'SupplierName', cap: 'Supplier Name', w: 90, t: 'text' },
        { key: 'SupplierWeight', cap: 'Supplier Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'FactoryWeight', cap: 'Factory Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'DiffWeight', cap: 'Diff Weight', w: 50, t: 'num', dec: 3, sum: true },
        { key: 'Freight', cap: 'Freight', w: 70, t: 'num', dec: 2, sum: true },
        { key: 'FreightPerMton', cap: 'Freight Per Mton', w: 63, t: 'num', dec: 2, sum: true },
        { key: 'NoOfBags', cap: 'No Of Bags', w: 54, t: 'num', dec: 3, sum: true },
        { key: 'FreightPer40Kg', cap: 'Freight Per40Kg', w: 60, t: 'num', dec: 2, sum: true }
    ];
    var REGISTER_COLS = [
        { key: 'Edit', cap: 'Edit', w: 50, t: 'btn' },
        { key: 'Print', cap: 'Print', w: 50, t: 'btn' },
        { key: 'Voucher', cap: 'Voucher', w: 70, t: 'btn' },
        { key: 'DocDate', cap: 'Doc Date', w: 80, t: 'date' },
        { key: 'DocNo', cap: 'Doc No', w: 55, t: 'int' },
        { key: 'GpDate', cap: 'Gp Date', w: 80, t: 'date' },
        { key: 'GpNo', cap: 'Gp No', w: 55, t: 'int' },
        { key: 'SupplierName', cap: 'Supplier Name', w: 200, t: 'text' },
        { key: 'VehicleNo', cap: 'Vehicle No', w: 75, t: 'text' },
        { key: 'BiltyNo', cap: 'Bilty No', w: 70, t: 'text' },
        { key: 'BiltyDate', cap: 'Bilty Date', w: 80, t: 'date' },
        { key: 'DeliveryTerm', cap: 'Delivery Term', w: 65, t: 'text' },
        { key: 'SupplierWeight', cap: 'Supplier Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'FactoryWeight', cap: 'Factory Weight', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'ShortWeight', cap: 'Short Weight', w: 65, t: 'num', dec: 3, sum: true },
        { key: 'AllowShortage', cap: 'Against Policy Discount/Allowed Weight', w: 100, t: 'num', dec: 3, sum: true },
        { key: 'ShortageApply', cap: 'Shortage Applied Weight', w: 65, t: 'num', dec: 3, sum: true },
        { key: 'ShortageAmount', cap: 'Shortage Applied Amount', w: 85, t: 'num', dec: 2, sum: true },
        { key: 'BiltyFreight', cap: 'Bilty Freight', w: 70, t: 'num', dec: 2, sum: true },
        { key: 'DeductedAmount', cap: 'Deduction From Freight', w: 80, t: 'num', dec: 2, sum: true },
        { key: 'DiscountAmount', cap: 'Discount Amount', w: 80, t: 'num', dec: 2, sum: true },
        { key: 'ToleranceWeight', cap: 'Weigh Bridge Tolerance', w: 70, t: 'text', cls: 'n' },
        { key: 'WbCharges', cap: 'Wb Charges', w: 60, t: 'num', dec: 3, sum: true },
        { key: 'AdvanceByParty', cap: 'Advance By Supplier', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'AdvanceByFactory', cap: 'Advance By Factory', w: 70, t: 'num', dec: 3, sum: true },
        { key: 'TotalFreight', cap: 'Total Freight', w: 80, t: 'num', dec: 2, sum: true },
        { key: 'ChargeToParty', cap: 'Shortage Charge To Supplier', w: 80, t: 'num', dec: 3, sum: true },
        { key: 'PaidAmount', cap: 'Net Payable Amount', w: 80, t: 'num', dec: 2, sum: true },
        { key: 'PerMtonFreight', cap: 'Per Mton Freight', w: 75, t: 'num', dec: 2, sum: true },
        { key: 'PerMaundFreight', cap: 'Per Maund Freight', w: 75, t: 'num', dec: 2, sum: true },
        { key: 'Per100KgFreight', cap: 'Per100Kg Freight', w: 75, t: 'num', dec: 2, sum: true },
        { key: 'CityName', cap: 'City Name', w: 100, t: 'text' },
        { key: 'CashAccountTitle', cap: 'Cash Account Title', w: 150, t: 'text' },
        { key: 'DebitAccountTitle', cap: 'Debit Account Title', w: 150, t: 'text' },
        { key: 'EntryDate', cap: 'Entry Date', w: 145, t: 'dt' },
        { key: 'EntryUser', cap: 'Entry User', w: 90, t: 'text' },
        { key: 'ModifyDate', cap: 'Modify Date', w: 145, t: 'dt' },
        { key: 'ModifyUser', cap: 'Modify User', w: 90, t: 'text' },
        { key: 'ApprovedDate', cap: 'Approved Date', w: 145, t: 'dt' },
        { key: 'ApprovalUser', cap: 'Approval User', w: 90, t: 'text' },
        { key: 'ApprovalStatus', cap: 'Approval Status', w: 80, t: 'text' },
        { key: 'NoOfAttachments', cap: 'No Of Attachments', w: 90, t: 'int' },
        { key: 'RemarksDetail', cap: 'Remarks Detail', w: 150, t: 'text' },
        { key: 'RemarksHeader', cap: 'Remarks Header', w: 150, t: 'text' },
        { key: 'AddAttachment', cap: 'Add Attachment', w: 105, t: 'btn' }
    ];
    var BREAKUP_COLS = [
        { key: 'Delete', cap: 'X', w: 20, t: 'btn' },
        { key: 'TransactionType', cap: 'Transaction Type', w: 90, t: 'text' },
        { key: 'InstrumentType', cap: 'Instrument Type', w: 90, t: 'text' },
        { key: 'AccountTitle', cap: 'Account Title', w: 200, t: 'text' },
        { key: 'ChequeNo', cap: 'Cheque No', w: 90, t: 'text' },
        { key: 'ChequeDate', cap: 'Cheque Date', w: 80, t: 'date' },
        { key: 'Amount', cap: 'Amount', w: 90, t: 'num', dec: 2, sum: true },
        { key: 'PayeeTitle', cap: 'Payee Title', w: 150, t: 'text' },
        { key: 'Remarks', cap: 'Remarks', w: 200, t: 'text' }
    ];

    /* ================================================================== form load */

    /* InitializeComponentMethod (:415) */
    function init() {
        initMasks();
        var today = new Date();
        setVal('voucherdatetime', isoDay(today));
        setVal('txtGPDate', isoDay(today));
        setVal('datChequeDate', isoDay(today));
        setVal('txtDriverCellNo', '092-3'); setVal('txtWhatsAppNo', '092-3'); setVal('txtAlternateCellNo', '092-3');
        wireEvents();
        renderDetail();
        Promise.all([get('/init'), get('/accounts'), get('/cities'), get('/drivers')]).then(function (res) {
            var d = res[0];
            accounts = res[1] || []; cities = res[2] || []; drivers = res[3] || [];
            rights = d.rights || rights;
            cfg = d.config || cfg;
            applyRights();
            if (RecId === 0) setVal('txtvoucherno', d.documentNo);
            getConfigurationsFromGlobal();
            cashAccountFillFromGlobal();
            cityDtFillFromGlobalAndBind();
            renderDetail();
            pendingRecordBind(d.pending || []);
            historyComboBind(d.historyCombos || []);
            var from = new Date();
            from.setDate(from.getDate() - (num(cfg.DefaultDaysToLessFromHistoryFromDate) > 0 ? num(cfg.DefaultDaysToLessFromHistoryFromDate) : 3));
            setVal('datFromDate', isoDay(from));
            setVal('datToDate', isoDay(new Date()));
            if (num(val('CmbCashAccount')) > 0) accountCurrentBalance();
            focusCtl('CmbCashAccount');
        }).catch(function (e) { box('Error occurred during database call.\n' + e.message); });
    }

    function applyRights() {
        $id('btnSave').disabled = !rights.canSave;
        $id('btnUpdate').disabled = !rights.canUpdate;
        $id('btnPrint').disabled = !rights.canPrint;
        var missing = [];
        if (!rights.canSave) missing.push('Save');
        if (!rights.canUpdate) missing.push('Update');
        if (!rights.canPrint) missing.push('Print');
        if (missing.length) { $id('rightsNote').textContent = 'Your user rights on Freight Voucher do not include: ' + missing.join(', ') + '.'; show('rightsNote', true); }
    }

    /* GetConfigurationsFromGlobal (:468) — the default credit account is applied before the
       list is bound, and BindAndRetainSelection then keeps it. */
    function getConfigurationsFromGlobal() {
        if (num(cfg.DefaultFreightVoucherCreditAccountId) > 0) pendingCashDefault = String(cfg.DefaultFreightVoucherCreditAccountId);
    }
    var pendingCashDefault = '';

    /* CashAccountFillFromGlobal (:550) */
    function cashAccountFillFromGlobal() {
        var types = $id('rdCashAccount').checked
            ? (cfg.IncludeBankAccountsInFreightVoucherCreditAccount ? [2, 15] : [2])
            : [3, 6, 8];
        var rows = distinctAccounts(function (a) { return types.indexOf(a.AccountTypeId) >= 0; });
        var keep = pendingCashDefault || val('CmbCashAccount');
        pendingCashDefault = '';
        bindSelect('CmbCashAccount', rows, 'Id', 'AccountTitle', { retain: false, attrs: [['code', 'AccountCode'], ['type', 'AccountTypeId']] });
        if (keep) selectValue('CmbCashAccount', keep);
    }

    /* CityDtFillFromGlobalAndBind (:619) */
    function cityDtFillFromGlobalAndBind() {
        bindSelect('CmbCity', cities, 'Id', 'Description');
    }

    /* HistoryComboBind (:492) */
    function historyComboBind(dt) {
        var supp = [], cr = [], dr = [];
        (dt || []).forEach(function (r) {
            var a = str(ci(r, 'Activity')), row = { Id: ci(r, 'Id'), Name: ci(r, 'ReferenceName') };
            if (a === 'Supplier') supp.push(row);
            else if (a === 'CreditAccount') cr.push(row);
            else if (a === 'DebitAccount') dr.push(row);
        });
        bindSelect('CmbSupplierName', supp, 'Id', 'Name');
        bindSelect('CmbCreditAcRegister', cr, 'Id', 'Name');
        bindSelect('CmbDebitAcRegister', dr, 'Id', 'Name');
    }

    /* AccountCurrentBalance (:645) */
    function accountCurrentBalance() {
        var id = intOf(val('CmbCashAccount'));
        get('/balance', { accountId: id }).then(function (r) {
            if (r.found) {
                var b = num(r.balance);
                setVal('txtbalance', fmtBal(b));
                $id('lbldrcr').textContent = b === 0 ? 'Nill' : (b > 0 ? 'Dr' : 'Cr');
                show('lbldrcr', true);
            } else {
                setVal('txtbalance', '0');
                $id('lbldrcr').textContent = 'Nill';
            }
        }).catch(function (e) { box(e.message); });
    }

    /* ================================================================== pending records */

    /* PendingRecordBind (:987) — the desktop copies the procedure's rows into its own table,
       renaming GpSrNo -> GpNo, PolicyShortWeight -> AllowShortWeight and
       ShortWeightAfterDeductionPolicy -> ShortageApplyWeight. */
    function pendingRecordBind(dt) {
        pendingRows = (dt || []).map(function (r) {
            return {
                Id: ci(r, 'Id'), GpDate: dayOf(ci(r, 'GpDate')), GpNo: ci(r, 'GpSrNo'), VehicleNo: ci(r, 'VehicleNo'),
                BiltyNo: ci(r, 'BiltyNo'), BiltyFreight: ci(r, 'BiltyFreight'), WbCharges: ci(r, 'WbCharges'),
                AdvanceByParty: ci(r, 'AdvanceByParty'), AdvanceByFactory: ci(r, 'AdvanceByFactory'),
                TotalFreight: ci(r, 'TotalFreight'), ItemQty: ci(r, 'ItemQty'), SupplierWeight: ci(r, 'SupplierWeight'),
                NetWbWeight: ci(r, 'NetWbWeight'), ToleranceWeight: ci(r, 'ToleranceWeight'), ShortWeight: ci(r, 'ShortWeight'),
                AllowShortWeight: ci(r, 'PolicyShortWeight'), ShortageApplyWeight: ci(r, 'ShortWeightAfterDeductionPolicy'),
                ShortageAmount: ci(r, 'ShortageAmount'), Status: ci(r, 'Status'), CityId: ci(r, 'CityId'),
                CityName: ci(r, 'CityName'), VarietyName: ci(r, 'VarietyName'), SupplierName: ci(r, 'SupplierName'),
                FreightCustomAccountsGroupId: ci(r, 'FreightCustomAccountsGroupId'), DebitAccountId: ci(r, 'DebitAccountId'),
                AccountTitle: ci(r, 'AccountTitle'), OrderRate: ci(r, 'OrderRate'), Remarks: ci(r, 'Remarks'),
                WeightToleranceAllowPerMTon: ci(r, 'WeightToleranceAllowPerMTon'), PolicyTypeId: ci(r, 'PolicyTypeId'),
                RefDocumentTypeId: ci(r, 'RefDocumentTypeId'), DriverName: ci(r, 'DriverName'), CnicNo: ci(r, 'CnicNo'),
                DriverCellNo: ci(r, 'DriverCellNo'), whatsappNo: ci(r, 'whatsappNo'), AlternateCellNo: ci(r, 'AlternateCellNo'),
                FatherName: ci(r, 'FatherName'), fatherCnicNo: ci(r, 'fatherCnicNo'), driverBiodataId: ci(r, 'driverBiodataId')
            };
        });
        renderGrid('grdPendingRecord', PENDING_COLS, pendingRows, { totals: true, rec: 'recPending' });
    }

    /* LoadRecord (:1098) */
    function loadRecord(item) {
        try {
            if (RecId > 0) throw new Error("Record can't be loaded because another record is in update mode.");
            table = [];
            dtFreightPaymentBreakUp = [];
            var debitAccountId = intOf(item.DebitAccountId);
            if (debitAccountId === 0) throw new Error('FreightAccount not found. Please check!');
            setVal('txtGpId', cs(item.Id));
            setVal('txtGPDate', dayOf(item.GpDate));
            setVal('txtGPNo', cs(item.GpNo));
            setVal('txtVehicleNo', item.VehicleNo);
            setVal('txtBiltyNo', item.BiltyNo);
            setVal('txtBiltyFreight', cs(item.BiltyFreight));
            setVal('txtAllowShortage', cs(item.AllowShortWeight));
            setVal('txtShortageApply', cs(item.ShortageApplyWeight));
            setVal('txtWeightToleranceAllowPerMTon', cs(item.WeightToleranceAllowPerMTon));
            setVal('txtSupplierWeight', cs(item.SupplierWeight));
            setVal('txtFactoryWeight', cs(item.NetWbWeight));
            setVal('txtToleranceWeight', cs(item.ToleranceWeight));
            setVal('txtShortWeight', cs(item.ShortWeight));
            label17();
            setVal('txtremarksmain', item.Remarks);
            var cityId = intOf(item.CityId);
            selectValue('CmbCity', cityId);
            setVal('txtPartyName', item.SupplierName);
            var biltyFreight = num(item.BiltyFreight);
            var advP = num(item.AdvanceByParty), advF = num(item.AdvanceByFactory);
            var net = num(item.NetWbWeight);
            var perMTon = net > 0 ? biltyFreight / net * 1000 : 0;
            var per100 = net > 0 ? biltyFreight / net * 100 : 0;
            var wb = num(item.WbCharges);
            var totalFreight = biltyFreight + wb - advF - advP;
            var shortage = num(item.ShortageAmount);
            var policyTypeId = intOf(item.PolicyTypeId);
            var refDoc = intOf(item.RefDocumentTypeId);
            var rate = num(item.OrderRate);
            var groupId = intOf(item.FreightCustomAccountsGroupId);
            DriverBioId = intOf(item.driverBiodataId);
            if (DriverBioId > 0) {
                setVal('txtCNIC', item.CnicNo); setVal('txtDriverCellNo', item.DriverCellNo);
                setVal('txtWhatsAppNo', item.whatsappNo); setVal('txtAlternateCellNo', item.AlternateCellNo);
                setVal('txtDriverName', item.DriverName); setVal('txtFatherName', item.FatherName);
                setVal('txtFatherCNIC', item.fatherCnicNo);
                driverFieldsDisableOrEnable(false);
            }
            table.push({
                Id: 0, FreightCustomAccountsGroupId: groupId, DebitAccountId: debitAccountId, AccountTitle: item.AccountTitle,
                BiltyFreight: biltyFreight, FreightPer100Kg: per100, FreightPerMton: perMTon, WbCharges: wb,
                AdvanceByParty: advP, AdvanceByFactory: advF, OtherDeduction: 0, TotalFreight: totalFreight,
                ShortageAmount: shortage, DiscountAmount: 0, RemainingAmount: shortage, DeductionAmount: 0,
                ChargeToParty: shortage, PaidAmount: totalFreight, Remarks: '', VarietyName: item.VarietyName,
                ItemQty: item.ItemQty, RateForShortage: item.OrderRate
            });
            gridSet = { refDocumentTypeId: refDoc, rate: rate, customGroupId: groupId };
            renderDetail();
            previousCityDataBind(cityId);
            setAllowShortageEditable(policyTypeId === 3);
        } catch (e) { box(e.message); }
    }

    function label17() {
        $id('label17').textContent = num(val('txtShortWeight')) < 0 ? 'Excess Weight' : 'Short Weight';
    }
    function setAllowShortageEditable(on) {
        var e = $id('txtAllowShortage');
        e.readOnly = !on; e.tabIndex = on ? 0 : -1;
    }

    /* PreviousCityDataBind (:865) */
    function previousCityDataBind(cityId) {
        get('/previous-city', { cityId: cityId, recId: RecId }).then(function (dt) {
            previousRows = (dt || []).map(function (r) {
                return { GpNo: ci(r, 'GatePassNo'), GpDate: dayOf(ci(r, 'GatePassDate')), SupplierName: ci(r, 'SupplierName'),
                         SupplierWeight: ci(r, 'SupplierWeight'), FactoryWeight: ci(r, 'FactoryWeight'), DiffWeight: ci(r, 'DiffWeight'),
                         Freight: ci(r, 'Freight'), FreightPerMton: ci(r, 'PerMtonFreight'), NoOfBags: ci(r, 'NoOfPackages'),
                         FreightPer40Kg: ci(r, 'FreightPer40Kg') };
            });
            renderGrid('grdPreviousInformation', PREV_COLS, previousRows, { totals: true, rec: 'recPrev' });
        }).catch(function (e) { box(e.message); });
    }
    function clearPrevious() { previousRows = []; $id('grdPreviousInformation').innerHTML = ''; $id('recPrev').textContent = 'Record: 0 Of 0'; }

    /* ================================================================== detail grid edits */

    /* grd_CellUpdated (:694) — reproduced branch for branch, messages included. */
    function cellUpdated(i, columnName) {
        var item = table[i]; if (!item) return;
        var shortageAmount = num(item.ShortageAmount);
        if (columnName === 'ShortageAmount') item.RemainingAmount = shortageAmount;
        var discountAmount = num(item.DiscountAmount);
        var chargeToParty = num(item.ChargeToParty);
        var totalFreight = num(item.TotalFreight);
        var deductionAmount = num(item.DeductionAmount);
        var remainingAmount = num(item.RemainingAmount);
        if (columnName === 'OtherDeduction') {
            var bilty = num(item.BiltyFreight), wb = num(item.WbCharges);
            var advP = num(item.AdvanceByParty), advF = num(item.AdvanceByFactory);
            var other = num(item.OtherDeduction);
            var gross = bilty + wb - (advF + advP);
            var netPaid = gross - deductionAmount;
            if (other > netPaid) {
                other = netPaid;
                item.OtherDeduction = other;
                box('OtherDeduction can not be greater than PaidAmount.');
            }
            totalFreight = gross - other;
            item.TotalFreight = totalFreight;
            if (deductionAmount > totalFreight) { deductionAmount = totalFreight; item.DeductionAmount = deductionAmount; }
        }
        if (columnName === 'DiscountAmount' && shortageAmount === 0) {
            item.DiscountAmount = 0; item.ChargeToParty = 0; item.DeductionAmount = 0;
            box('There is no shortage.');
            renderDetail();
            return;
        }
        if (columnName === 'DeductionAmount' && deductionAmount > totalFreight) {
            deductionAmount = totalFreight; item.DeductionAmount = deductionAmount;
            box('DeductionAmount can not be greater than totalFreight.');
        }
        if (columnName === 'DeductionAmount' && deductionAmount > remainingAmount) {
            deductionAmount = remainingAmount; item.DeductionAmount = deductionAmount;
            box('DeductionAmount can not be greater than RemainingAmount.');
        }
        if (columnName === 'DiscountAmount' || columnName === 'ShortageAmount') {
            if (discountAmount > shortageAmount) {
                discountAmount = shortageAmount; item.DiscountAmount = discountAmount;
                box('DiscountAmount can not be greater than ShortageAmount.');
            }
            remainingAmount = shortageAmount - discountAmount;
            item.RemainingAmount = remainingAmount;
            if (deductionAmount > remainingAmount) { deductionAmount = remainingAmount; item.DeductionAmount = deductionAmount; }
            chargeToParty = remainingAmount - deductionAmount;
            item.ChargeToParty = chargeToParty;
        }
        if (columnName === 'DeductionAmount' || columnName === 'ShortageAmount') {
            chargeToParty = remainingAmount - deductionAmount;
            item.ChargeToParty = chargeToParty;
        }
        if (columnName !== 'Remarks') item.PaidAmount = totalFreight - deductionAmount;
        renderDetail();
    }

    /* txtAllowShortage_TextChanged (:2332) */
    function allowShortageChanged() {
        if ($id('txtAllowShortage').readOnly) return;
        var sw = num(val('txtShortWeight')), allow = num(val('txtAllowShortage'));
        if (sw > 0) {
            if (allow > sw) {
                setVal('txtAllowShortage', '0');
                box('Allow shortage cannot be greater than short weight please check....');
            } else {
                setVal('txtShortageApply', fmt(sw - allow, 3));
                recalculateShortageAmount();
            }
        }
    }

    /* RecalculateShortageAmount (:2362) */
    function recalculateShortageAmount() {
        table.forEach(function (item) {
            var apply = num(val('txtShortageApply'));
            var orderRate = num(item.RateForShortage);
            var deduction = num(item.DeductionAmount);
            var totalFreight = num(item.TotalFreight);
            var shortage = roundHalfEven(apply / 40 * orderRate);
            item.ShortageAmount = shortage;
            if (shortage === 0) { item.DiscountAmount = 0; item.RemainingAmount = 0; item.DeductionAmount = 0; }
            var remaining = shortage - num(item.DiscountAmount);
            item.RemainingAmount = remaining;
            item.ChargeToParty = remaining - deduction;
            item.PaidAmount = totalFreight - deduction;
        });
        renderDetail();
    }

    /* ================================================================== driver information */

    function driverFieldsDisableOrEnable(flag) {
        ['txtCNIC', 'txtDriverCellNo', 'txtWhatsAppNo', 'txtAlternateCellNo', 'txtDriverName', 'txtFatherName', 'txtFatherCNIC']
            .forEach(function (id) { $id(id).disabled = !flag; });
    }
    function resetDriverFields() {
        DriverBioId = 0;
        ['txtCNIC', 'txtDriverName', 'txtFatherName', 'txtFatherCNIC'].forEach(function (id) { setVal(id, ''); });
        setVal('txtDriverCellNo', '092-3'); setVal('txtWhatsAppNo', '092-3'); setVal('txtAlternateCellNo', '092-3');
    }
    function fillDriver(obj) {
        DriverBioId = intOf(obj.Id);
        setVal('txtCNIC', obj.CnicNo); setVal('txtDriverCellNo', obj.DriverCellNo);
        setVal('txtWhatsAppNo', obj.WhatsappNo); setVal('txtAlternateCellNo', obj.AlternateCellNo);
        setVal('txtDriverName', obj.DriverName); setVal('txtFatherName', obj.FatherName);
        setVal('txtFatherCNIC', obj.FatherCnicNo);
        driverFieldsDisableOrEnable(false);
    }
    /* txtCNIC_Leave (:2440) */
    function cnicLeave() {
        var t = val('txtCNIC').trim();
        var obj = drivers.filter(function (r) { return str(r.CnicNo) === t; })[0];
        if (obj) fillDriver(obj); else driverFieldsDisableOrEnable(true);
    }
    /* txtDriverCellNo_Leave (:2467) */
    function cellLeave() {
        var t = val('txtDriverCellNo').trim();
        var obj = drivers.filter(function (r) { return str(r.DriverCellNo) === t; })[0];
        if (obj) fillDriver(obj); else driverFieldsDisableOrEnable(true);
    }
    /* btnResetDriverInfo_Click (:2494) */
    function resetDriverInfo() {
        get('/drivers').then(function (d) { drivers = d || []; }).catch(function () { });
        resetDriverFields();
        driverFieldsDisableOrEnable(true);
    }

    /* ================================================================== save / update */

    function sum(key) { var s = 0; table.forEach(function (r) { s += num(r[key]); }); return s; }
    function isNonZeroInt(t) { return /^\s*[-+]?\d+\s*$/.test(str(t)) && parseInt(t, 10) !== 0; }
    function isNonZeroDouble(t) { var s = str(t).trim(); return s !== '' && !isNaN(Number(s)) && Number(s) !== 0; }

    /* Insert (:1258) — the operator-facing half: the same checks, questions and messages. The
       server repeats every refusal before it writes. */
    function insert() {
        try {
            if (table.length === 0) throw new Error('Grid Record Not Found');
            if (val('txtDriverCellNo').trim() === '' || !maskFull($id('txtDriverCellNo'))) { box('Cell No. Field Required'); focusCtl('txtDriverCellNo'); return; }
            if (val('txtDriverName').trim() === '') { box('Driver Name  Field Required'); focusCtl('txtDriverName'); return; }
            if (dtFreightPaymentBreakUp.length === 0 && intOf(val('CmbCashAccount')) === 0) { box('Cash Account Field Required'); focusCtl('CmbCashAccount'); return; }
            if (!isNonZeroInt(val('txtGPNo'))) { box('GpNo must be a non-zero number'); return; }
            if (!isNonZeroInt(val('txtBiltyFreight'))) { box('BiltyFreight must be a non-zero number'); return; }
            if (!isNonZeroDouble(val('txtSupplierWeight'))) { box('SupplierWeight must be a non-zero number'); return; }
            if (!isNonZeroDouble(val('txtFactoryWeight'))) { box('FactoryWeight must be a non-zero number'); return; }
            if (!ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;

            var tol = num(cfg.TolerancePercentForDiscountonFreightVoucher);
            var sumDiscount = sum('DiscountAmount'), shortage = sum('ShortageAmount');
            var toleranceAmount = shortage * tol / 100;
            if (sumDiscount > toleranceAmount)
                throw new Error('Total Discount Amount ' + cs(sumDiscount) + ' cannot be greater than Tolerance Amount ' + cs(toleranceAmount) + ' for Shortage Amount ' + cs(shortage) + '.');
            var allow = num(val('txtAllowShortage')), sw = num(val('txtShortWeight'));
            var tolW = sw * tol / 100;
            if (allow > tolW)
                throw new Error('Total Allow shortage ' + cs(allow) + ' cannot be greater than Tolerance Weight ' + cs(tolW) + ' for Short weight ' + cs(sw) + '.');
            var sumPerMTon = sum('FreightPerMton');
            var latest = previousRows.slice().sort(function (a, b) { return str(b.GpDate).localeCompare(str(a.GpDate)); })[0];
            var firstPerMTon = latest ? num(latest.FreightPerMton) : 0;
            if (sumPerMTon > 0 && firstPerMTon > 0 && sumPerMTon > firstPerMTon + 100
                && !ask('Total Freight Per Metric Ton (' + fmt(sumPerMTon, 2).replace(/,/g, '') + ') exceeds the previous freight rate ('
                        + fmt(firstPerMTon, 2).replace(/,/g, '') + ') by more than 100. Do you want to continue??')) return;

            /* per-row refusals (:1376-1385) are checked here too so nothing is posted in vain */
            for (var i = 0; i < table.length; i++) {
                var r = table[i];
                if (intOf(r.DebitAccountId) === 0) throw new Error('Debit Account is required in Detail Grid at row No: ' + (i + 1));
                if (intOf(r.BiltyFreight) === 0) throw new Error('BiltyFreight is required in Detail Grid at row No: ' + (i + 1));
                if (roundHalfEven(num(r.DiscountAmount)) + roundHalfEven(num(r.ChargeToParty)) + roundHalfEven(num(r.DeductionAmount)) > roundHalfEven(num(r.ShortageAmount)))
                    throw new Error("Sum of 'DiscountAmount,ChargeToParty and DeductionAmount' cannot exceed 'ShortageAmount'. in Row No : " + (i + 1));
                if (num(r.PaidAmount) < 0) throw new Error('TotalPayable Amount can not less than 0 in Row No : ' + (i + 1));
            }
            if (dtFreightPaymentBreakUp.length > 0) {
                var bu = 0; dtFreightPaymentBreakUp.forEach(function (b) { bu += num(b.Amount); });
                var paid = sum('PaidAmount');
                if (Math.abs(bu - paid) > 0.00005) throw new Error('BreakUp Amount:' + cs(bu) + ' not equal to TotalPaidAmount:' + cs(paid));
            }

            var preview = $id('ChkBox').checked ? window.open('', '_blank') : null;
            if (preview) preview.document.write('<p style="font-family:Segoe UI;padding:20px;">Saving…</p>');
            post('/save', payload()).then(function (res) {
                box(res.message);
                reset();
                if (preview) printSlip241(res.id, preview);
            }).catch(function (e) {
                if (preview) preview.close();
                box('Database Error\n\n' + e.message);
            });
        } catch (e) { box('Database Error\n\n' + e.message); }
    }

    function payload() {
        return {
            id: RecId,
            documentNo: val('txtvoucherno').trim(),
            docDate: val('voucherdatetime'),
            creditAccount: $id('rdCreditAccount').checked,
            cashAccountId: intOf(val('CmbCashAccount')),
            manualNo: val('txtManualNumber'),
            remarksHeader: val('txtremarksmain'),
            gpId: val('txtGpId'), gpNo: val('txtGPNo'), gpDate: val('txtGPDate'),
            vehicleNo: val('txtVehicleNo'), biltyNo: val('txtBiltyNo'), biltyFreight: val('txtBiltyFreight'),
            partyName: val('txtPartyName'), supplierWeight: val('txtSupplierWeight'), factoryWeight: val('txtFactoryWeight'),
            shortWeight: val('txtShortWeight'), allowShortage: val('txtAllowShortage'), shortageApply: val('txtShortageApply'),
            toleranceWeight: val('txtToleranceWeight'), cityId: intOf(val('CmbCity')),
            driverBiodataId: DriverBioId,
            cnicNo: val('txtCNIC'), driverCellNo: val('txtDriverCellNo'), driverCellMaskFull: maskFull($id('txtDriverCellNo')),
            whatsappNo: val('txtWhatsAppNo'), alternateCellNo: val('txtAlternateCellNo'),
            driverName: val('txtDriverName'), fatherName: val('txtFatherName'), fatherCnicNo: val('txtFatherCNIC'),
            rows: table.map(function (r) {
                return { id: intOf(r.Id), freightCustomAccountsGroupId: intOf(r.FreightCustomAccountsGroupId),
                         debitAccountId: intOf(r.DebitAccountId), accountTitle: str(r.AccountTitle),
                         biltyFreight: num(r.BiltyFreight), freightPer100Kg: num(r.FreightPer100Kg), freightPerMton: num(r.FreightPerMton),
                         wbCharges: num(r.WbCharges), advanceByParty: num(r.AdvanceByParty), advanceByFactory: num(r.AdvanceByFactory),
                         otherDeduction: num(r.OtherDeduction), totalFreight: num(r.TotalFreight), shortageAmount: num(r.ShortageAmount),
                         discountAmount: num(r.DiscountAmount), remainingAmount: num(r.RemainingAmount), deductionAmount: num(r.DeductionAmount),
                         chargeToParty: num(r.ChargeToParty), paidAmount: num(r.PaidAmount), remarks: str(r.Remarks),
                         varietyName: str(r.VarietyName), itemQty: cs(r.ItemQty), rateForShortage: num(r.RateForShortage) };
            }),
            breakUps: dtFreightPaymentBreakUp.map(function (b) {
                return { id: intOf(b.Id), transactionTypeId: intOf(b.TransactionTypeId), transactionType: str(b.TransactionType),
                         instrumentTypeId: intOf(b.InstrumentTypeId), instrumentType: str(b.InstrumentType),
                         accountTitleId: intOf(b.AccountTitleId), accountTitle: str(b.AccountTitle), chequeId: intOf(b.ChequeId),
                         chequeNo: str(b.ChequeNo), chequeDate: dayOf(b.ChequeDate), amount: num(b.Amount),
                         payeeTitle: str(b.PayeeTitle), remarks: str(b.Remarks) };
            })
        };
    }

    /* Save_Click (:1456) / Update_Click (:1469) */
    function save() { if (!rights.canSave || $id('btnSave').classList.contains('is-hidden')) return; RecId = 0; insert(); }
    function update() {
        if (!rights.canUpdate || $id('btnUpdate').classList.contains('is-hidden')) return;
        if (RecId === 0) { box('Database Error\n\nRecord Id not found please check'); return; }
        insert();
    }

    /* ReadById (:1485) */
    function readById(id) {
        reset().then(function () {
            RecId = id;
            return get('/read/' + id);
        }).then(function (d) {
            var h = d.header;
            show('btnSave', false); show('btnUpdate', true);
            tab('tabForm');
            setVal('voucherdatetime', dayOf(ci(h, 'DocDate')));
            setVal('txtvoucherno', cs(ci(h, 'DocumentNo')));
            if (intOf(ci(h, 'FreightOn')) === 1) $id('rdCreditAccount').checked = true; else $id('rdCashAccount').checked = true;
            cashAccountFillFromGlobal();
            if (intOf(ci(h, 'CashAccountId')) > 0) selectValue('CmbCashAccount', intOf(ci(h, 'CashAccountId')));
            setVal('txtremarksmain', ci(h, 'RemarksHeader'));
            setVal('txtGpId', cs(ci(h, 'GatePassId')));
            setVal('txtGPNo', cs(ci(h, 'GatePassNo')));
            setVal('txtGPDate', dayOf(ci(h, 'GatePassDate')));
            setVal('txtManualNumber', ci(h, 'ManualNo'));
            setVal('txtVehicleNo', ci(h, 'VehicleNo'));
            setVal('txtBiltyNo', ci(h, 'BiltyNo'));
            setVal('txtSupplierWeight', str(ci(h, 'SupplierWeight')));
            setVal('txtFactoryWeight', str(ci(h, 'FactoryWeight')));
            setVal('txtToleranceWeight', str(ci(h, 'ToleranceWeight')));
            setVal('txtShortWeight', str(ci(h, 'DifferenceWeight')));
            setVal('txtAllowShortage', cs(ci(h, 'AllowShortage')));
            setVal('txtShortageApply', cs(ci(h, 'ShortageApply')));
            label17();
            selectValue('CmbCity', intOf(ci(h, 'CityId')));
            setVal('txtPartyName', ci(h, 'PartyName'));
            DriverBioId = intOf(ci(h, 'driverBiodataId'));
            if (DriverBioId > 0) {
                setVal('txtCNIC', ci(h, 'CnicNo')); setVal('txtDriverCellNo', ci(h, 'DriverCellNo'));
                setVal('txtWhatsAppNo', ci(h, 'whatsappNo')); setVal('txtAlternateCellNo', ci(h, 'AlternateCellNo'));
                setVal('txtDriverName', ci(h, 'DriverName')); setVal('txtFatherName', ci(h, 'FatherName'));
                setVal('txtFatherCNIC', ci(h, 'fatherCnicNo'));
                driverFieldsDisableOrEnable(false);
            }
            VoucherHeadId = intOf(d.voucherHeadId);
            table = [];
            var freight = 0, customGroupId = 0, rateForShortage = 0;
            var factory = num(ci(h, 'FactoryWeight'));
            (d.details || []).forEach(function (x) {
                rateForShortage = num(ci(x, 'RateForShortage'));
                customGroupId = intOf(ci(x, 'CustomGroupId'));
                var bilty = num(ci(x, 'BiltyFreight'));
                var perMTon = factory > 0 ? bilty / factory * 1000 : 0;
                var per100 = factory > 0 ? bilty / factory * 100 : 0;
                var total = bilty + num(ci(x, 'ScaleCharges')) - num(ci(x, 'AdvanceByFactory')) - num(ci(x, 'AdvanceByParty')) - num(ci(x, 'OtherDeduction'));
                table.push({
                    Id: ci(x, 'Id'), FreightCustomAccountsGroupId: customGroupId, DebitAccountId: ci(x, 'DebitAccountId'),
                    AccountTitle: ci(x, 'AccountTitle'), BiltyFreight: bilty, FreightPer100Kg: per100, FreightPerMton: perMTon,
                    WbCharges: num(ci(x, 'ScaleCharges')), AdvanceByParty: num(ci(x, 'AdvanceByParty')), AdvanceByFactory: num(ci(x, 'AdvanceByFactory')),
                    OtherDeduction: num(ci(x, 'OtherDeduction')), TotalFreight: total, ShortageAmount: num(ci(x, 'ShortageAmount')),
                    DiscountAmount: num(ci(x, 'AddLessAmount')), RemainingAmount: num(ci(x, 'ShortageAmount')) - num(ci(x, 'AddLessAmount')),
                    DeductionAmount: num(ci(x, 'DeductedAmount')), ChargeToParty: num(ci(x, 'ChargeToParty')),
                    PaidAmount: num(ci(x, 'TotalPayableAmount')), Remarks: ci(x, 'RemarksDetail'),
                    VarietyName: ci(h, 'VarietyName'), ItemQty: ci(h, 'GpQty'), RateForShortage: num(ci(x, 'RateForShortage'))
                });
                freight += intOf(ci(x, 'BiltyFreight'));
            });
            setVal('txtBiltyFreight', String(freight));
            gridSet = { refDocumentTypeId: intOf(ci(h, 'RefDocumentTypeId')), rate: rateForShortage, customGroupId: customGroupId };
            renderDetail();
            dtFreightPaymentBreakUp = (d.breakUps || []).map(function (b) {
                return { Id: ci(b, 'Id'), TransactionTypeId: ci(b, 'TransTypeId'), TransactionType: ci(b, 'TransactionType'),
                         InstrumentTypeId: ci(b, 'InstrumentTypeId'), InstrumentType: ci(b, 'InstrumentType'),
                         AccountTitleId: ci(b, 'AccountId'), AccountTitle: ci(b, 'AccountTitle'), ChequeId: ci(b, 'CheqId'),
                         ChequeNo: ci(b, 'CheqNo'), ChequeDate: dayOf(ci(b, 'CheqDate')), Amount: num(ci(b, 'Amount')),
                         PayeeTitle: ci(b, 'PayeeTitle'), Remarks: ci(b, 'Remarks') };
            });
            previousCityDataBind(intOf(ci(h, 'CityId')));
            setAllowShortageEditable(intOf(ci(h, 'PolicyTypeId')) === 3);
            if (intOf(val('CmbCashAccount')) > 0) accountCurrentBalance();
        }).catch(function (e) { box(e.message); });
    }

    /* Reset (:1586) — note it does not clear the credit account, the Cash/Credit choice,
       Shortage Apply or the city's hidden per-MTon tolerance, exactly as the desktop. */
    function reset() {
        RecId = 0; VoucherHeadId = 0;
        show('btnSave', true); show('btnUpdate', false);
        ['txtremarksmain', 'txtManualNumber', 'txtGpId', 'txtGPNo', 'txtVehicleNo', 'txtBiltyNo', 'txtBiltyFreight',
         'txtAllowShortage', 'txtSupplierWeight', 'txtFactoryWeight', 'txtToleranceWeight', 'txtShortWeight', 'txtPartyName']
            .forEach(function (id) { setVal(id, ''); });
        label17();
        selectValue('CmbCity', '');
        table = [];
        gridSet = { refDocumentTypeId: 0, rate: 0, customGroupId: 0 };
        renderDetail();
        dtFreightPaymentBreakUp = [];
        clearPrevious();
        ['txtCNIC', 'txtDriverName', 'txtFatherName', 'txtFatherCNIC'].forEach(function (id) { setVal(id, ''); });
        setVal('txtDriverCellNo', '092-3'); setVal('txtWhatsAppNo', '092-3'); setVal('txtAlternateCellNo', '092-3');
        driverFieldsDisableOrEnable(true);
        focusCtl('CmbCashAccount');
        return Promise.all([get('/document-no'), get('/pending'), get('/drivers')]).then(function (res) {
            setVal('txtvoucherno', res[0].documentNo);
            pendingRecordBind(res[1] || []);
            drivers = res[2] || [];
        }).catch(function (e) { box(e.message); });
    }

    /* btnNew_Click (:1639) */
    function newForm() {
        reset();
        selectValue('CmbCashAccount', '');
        show('lbldrcr', false);
    }

    /* BtnRefresh_Click (:1658) — reload the account and city caches, re-apply configuration */
    function refresh() {
        Promise.all([get('/accounts'), get('/cities'), get('/config')]).then(function (res) {
            accounts = res[0] || []; cities = res[1] || []; cfg = res[2] || cfg;
            getConfigurationsFromGlobal();
            cashAccountFillFromGlobal();
            cityDtFillFromGlobalAndBind();
            renderDetail();
        }).catch(function (e) { box(e.message); });
    }

    /* ================================================================== register tab */

    /* btnshow_Click (:1716) */
    function showRegister() {
        var f = {
            fromDate: val('datFromDate'), toDate: val('datToDate'),
            gpNoFrom: intOf(val('txtGpNoFrom')), gpNoTo: intOf(val('txtGpNoTo')),
            vehicleNo: val('txtVehicleRegister'),
            supplierCustomerId: intOf(val('CmbSupplierName')),
            creditAccountId: intOf(val('CmbCreditAcRegister')),
            debitAccountId: intOf(val('CmbDebitAcRegister')),
            onlyDiscountedRows: $id('chkOnlyDiscountedRows').checked,
            freightAuditByCity: $id('chkFreightAuditbyCity').checked
        };
        post('/register', f).then(function (rows) {
            dtRegister = rows || [];
            var grid = dtRegister.map(function (r) {
                var bilty = num(ci(r, 'BiltyFreight')), fac = num(ci(r, 'FactoryWeight'));
                return {
                    Id: ci(r, 'Id'), VoucherHeadId: ci(r, 'VoucherHeadId'), DocDate: ci(r, 'DocDate'), DocNo: ci(r, 'DocNo'),
                    GpDate: ci(r, 'GatePassDate'), GpNo: ci(r, 'GatePassNo'), SupplierName: ci(r, 'SupplierName'),
                    VehicleNo: ci(r, 'VehicleNo'), BiltyNo: ci(r, 'BiltyNo'), BiltyDate: ci(r, 'BiltyDate'),
                    DeliveryTerm: ci(r, 'DeliveryTerm'), SupplierWeight: ci(r, 'SupplierWeight'), FactoryWeight: ci(r, 'FactoryWeight'),
                    ShortWeight: ci(r, 'ShortWeight'), AllowShortage: ci(r, 'AllowShortage'), ShortageApply: ci(r, 'ShortageApply'),
                    ShortageAmount: ci(r, 'ShortageAmount'), BiltyFreight: bilty, DeductedAmount: ci(r, 'DeductedAmount'),
                    DiscountAmount: ci(r, 'DiscountAmount'), ToleranceWeight: cs(ci(r, 'ToleranceWeight')), WbCharges: ci(r, 'ScaleCharges'),
                    AdvanceByParty: ci(r, 'AdvanceByParty'), AdvanceByFactory: ci(r, 'AdvanceByFactory'), TotalFreight: ci(r, 'TotalFreight'),
                    ChargeToParty: ci(r, 'ChargeToParty'), PaidAmount: ci(r, 'PaidAmount'), RemainingAmount: ci(r, 'RemainingAmount'),
                    PerMtonFreight: ci(r, 'PerMtonFreight'), PerMaundFreight: bilty / fac * 40, Per100KgFreight: bilty / fac * 100,
                    CityName: ci(r, 'CityName'), CashAccountCode: ci(r, 'CashAcCode'), CashAccountTitle: ci(r, 'CashAcTitle'),
                    DebitAccountCode: ci(r, 'DebAcCode'), DebitAccountTitle: ci(r, 'DebAcTitle'), EntryDate: ci(r, 'EntryDate'),
                    EntryUser: ci(r, 'EntryUser'), ModifyDate: ci(r, 'ModifyDate'), ModifyUser: ci(r, 'ModifyUser'),
                    ApprovedDate: ci(r, 'ApprovedDate'), ApprovalUser: ci(r, 'ApprovalUser'), ApprovalStatus: ci(r, 'ApprovalStatus'),
                    NoOfAttachments: ci(r, 'NoOfAttachments'), RemarksDetail: ci(r, 'RemarksDetail'), RemarksHeader: ci(r, 'RemarksHeader')
                };
            });
            registerGrid = grid;
            renderGrid('grdRegister', REGISTER_COLS, grid, { totals: true, rec: 'recRegister' });
        }).catch(function (e) { box(e.message); });
    }
    var registerGrid = [];

    /* btnResetRegister_Click (:1687) */
    function resetRegister() {
        setVal('txtGpNoFrom', ''); setVal('txtGpNoTo', ''); setVal('txtVehicleRegister', '');
        selectValue('CmbSupplierName', '');
        dtRegister = []; registerGrid = [];
        $id('grdRegister').innerHTML = ''; $id('recRegister').textContent = 'Record: 0 Of 0';
    }
    /* btnRefreshRegister_Click (:1704) */
    function refreshRegister() {
        get('/history-combos').then(historyComboBind).catch(function (e) { box(e.message); });
    }

    /* ================================================================== prints */

    function openWin(pre) { return pre || window.open('', '_blank'); }
    function writeDoc(w, title, body) {
        w.document.open();
        w.document.write('<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + esc(title) + '</title><style>'
            + 'body{font-family:Segoe UI,Verdana,sans-serif;font-size:11px;margin:16px;color:#000}'
            + 'h2{margin:0 0 4px;font-size:15px}h3{margin:12px 0 4px;font-size:12px}'
            + '.muted{color:#666;font-size:10px}table{border-collapse:collapse;width:100%;margin-top:4px}'
            + 'th,td{border:1px solid #999;padding:2px 5px;text-align:left}th{background:#eee}td.n{text-align:right}'
            + '.kv{display:grid;grid-template-columns:repeat(4,max-content 1fr);gap:2px 10px;margin-top:6px}.kv b{font-weight:600}'
            + '@media print{.noprint{display:none}}</style></head><body>'
            + '<div class="noprint" style="margin-bottom:8px;"><button onclick="window.print()">Print</button></div>'
            + body + '</body></html>');
        w.document.close();
    }
    function kv(pairs) {
        var h = '<div class="kv">';
        pairs.forEach(function (p) { h += '<b>' + esc(p[0]) + '</b><span>' + esc(p[1]) + '</span>'; });
        return h + '</div>';
    }

    /* FreightSlip241 (:2020) -> CommonServices.FreightVoucherSlip241 */
    function printSlip241(id, pre) {
        if (!id) { box('No Record Found For Display'); if (pre) pre.close(); return; }
        var w = openWin(pre);
        get('/slip241/' + id).then(function (rows) {
            if (!rows || !rows.length) { w.close(); box('No Record Found For Display'); return; }
            var r = rows[0];
            var body = '<h2>241 - Freight Voucher Slip</h2><div class="muted">Browser output of SP_FreightVoucherSlipAndRegister — not the Crystal layout.</div>'
                + kv([['Doc No', ci(r, 'DocNo')], ['Doc Date', shortDate(ci(r, 'DocDate'))], ['Gp No', ci(r, 'GatePassNo')], ['Gp Date', shortDate(ci(r, 'GatePassDate'))],
                      ['Supplier', ci(r, 'SupplierName')], ['City', ci(r, 'CityName')], ['Vehicle No', ci(r, 'VehicleNo')], ['Bilty No', ci(r, 'BiltyNo')],
                      ['Item', ci(r, 'VarietyName')], ['Bags', ci(r, 'ItemQty')], ['Delivery Term', ci(r, 'DeliveryTerm')], ['Policy', ci(r, 'PolicyName')],
                      ['Supplier Weight', fmt(ci(r, 'SupplierWeight'), 3)], ['Factory Weight', fmt(ci(r, 'FactoryWeight'), 3)],
                      ['Short Weight', fmt(ci(r, 'ShortWeight'), 3)], ['WB Tolerance', cs(ci(r, 'ToleranceWeight'))],
                      ['Allow Shortage', fmt(ci(r, 'AllowShortage'), 3)], ['Shortage Apply', fmt(ci(r, 'ShortageApply'), 3)],
                      ['Credit Account', ci(r, 'CashAcTitle')], ['Approval', ci(r, 'ApprovalStatus')],
                      ['Driver', ci(r, 'DriverName')], ['CNIC', ci(r, 'CnicNo')], ['Driver Cell', ci(r, 'DriverCellNo')], ['Father', ci(r, 'FatherName')]]);
            body += '<h3>Detail</h3><table><tr><th>Debit Account</th><th>Bilty Freight</th><th>Per MTon</th><th>Scale Charges</th><th>Adv. Supplier</th><th>Adv. Factory</th><th>Other Ded.</th><th>Total Freight</th><th>Shortage</th><th>Discount</th><th>Deduction</th><th>Charge To Supplier</th><th>Net Payable</th></tr>';
            var seenDetail = {};
            rows.forEach(function (x) {
                var k = ci(x, 'DetailId'); if (seenDetail[k]) return; seenDetail[k] = true;
                body += '<tr><td>' + esc(ci(x, 'DebAcTitle')) + '</td>' + ['BiltyFreight', 'PerMtonFreight', 'ScaleCharges', 'AdvanceByParty', 'AdvanceByFactory', 'OtherDeduction', 'TotalFreight', 'ShortageAmount', 'DiscountAmount', 'DeductedAmount', 'ChargeToParty', 'PaidAmount']
                    .map(function (c) { return '<td class="n">' + esc(fmt(ci(x, c), 2)) + '</td>'; }).join('') + '</tr>';
            });
            body += '</table><p>' + esc(ci(r, 'RemarksDetail')).replace(/\n/g, '<br>') + '</p>';
            var cityRows = rows.filter(function (x) { return str(ci(x, 'GatePassNoCity')) !== ''; });
            if (cityRows.length) {
                body += '<h3>Previous Data By City</h3><table><tr><th>Gp No</th><th>Gp Date</th><th>Supplier</th><th>Supplier Wt</th><th>Factory Wt</th><th>Diff</th><th>Freight</th><th>Per MTon</th></tr>';
                var seenCity = {};
                cityRows.forEach(function (x) {
                    var k = ci(x, 'GatePassNoCity'); if (seenCity[k]) return; seenCity[k] = true;
                    body += '<tr><td>' + esc(k) + '</td><td>' + esc(shortDate(ci(x, 'GatePassDateCity'))) + '</td><td>' + esc(ci(x, 'SupplierNameCity')) + '</td>'
                        + ['SupplierWeightCity', 'FactoryWeightCity', 'DiffWeightCity', 'FreightCity', 'PerMtonFreightCity'].map(function (c) { return '<td class="n">' + esc(fmt(ci(x, c), 2)) + '</td>'; }).join('') + '</tr>';
                });
                body += '</table>';
            }
            body += '<p class="muted">Entry: ' + esc(ci(r, 'EntryUser')) + ' ' + esc(longDateTime(ci(r, 'EntryDate'))) + '</p>';
            writeDoc(w, 'Freight Voucher Slip ' + ci(r, 'DocNo'), body);
        }).catch(function (e) { w.close(); box(e.message); });
    }

    /* Print_Click (:1996) -> ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId) */
    function printVoucher102(voucherHeadId) {
        if (!voucherHeadId) { box('VoucherId Not Found'); return; }
        var w = openWin();
        get('/voucher102/' + voucherHeadId).then(function (rows) {
            if (!rows || !rows.length) { w.close(); box('No Record Found For Display'); return; }
            var r = rows[0], dr = 0, cr = 0;
            var body = '<h2>102 - ' + esc(ci(r, 'DocumentTypeDescription')) + '</h2><div class="muted">Browser output of SpVouchers_PaymentReceiptVoucherSlipNew_Rpt — not the Crystal layout.</div>'
                + kv([['Voucher No', ci(r, 'DocumentTypeCode') + '-' + ci(r, 'VoucherCode')], ['Voucher Date', shortDate(ci(r, 'VoucherDate'))],
                      ['Status', ci(r, 'EntryStatus')], ['Manual Bill No', ci(r, 'ManualBillNo')]]);
            body += '<table><tr><th>Account Code</th><th>Account Title</th><th>Narration</th><th>Debit</th><th>Credit</th></tr>';
            rows.forEach(function (x) {
                dr += num(ci(x, 'DebitAmount')); cr += num(ci(x, 'CreditAmount'));
                body += '<tr><td>' + esc(ci(x, 'AccountCode')) + '</td><td>' + esc(ci(x, 'AccountTitle')) + '</td><td>' + esc(ci(x, 'Comments')).replace(/\n/g, '<br>')
                    + '</td><td class="n">' + esc(fmt(ci(x, 'DebitAmount'), 2)) + '</td><td class="n">' + esc(fmt(ci(x, 'CreditAmount'), 2)) + '</td></tr>';
            });
            body += '<tr><th colspan="3" style="text-align:right">Total</th><th class="n">' + esc(fmt(dr, 2)) + '</th><th class="n">' + esc(fmt(cr, 2)) + '</th></tr></table>';
            body += '<p>' + esc(ci(r, 'Remarks')) + '</p><p class="muted">Prepared by: ' + esc(ci(r, 'EntryUserName')) + ' · Approved by: ' + esc(ci(r, 'ApprovedUserName')) + '</p>';
            writeDoc(w, 'Voucher ' + ci(r, 'VoucherCode'), body);
        }).catch(function (e) { w.close(); box(e.message); });
    }

    /* btnRegister_Click (:2032) — 240-FreightVoucherRegister over dtRegister */
    function print240() {
        if (!dtRegister || !dtRegister.length) { box('Not Record Found For Display'); return; }
        var cols = REGISTER_COLS.filter(function (c) { return c.t !== 'btn' && ['EntryDate', 'ModifyDate', 'ApprovedDate', 'ModifyUser', 'ApprovalUser', 'RemarksDetail', 'RemarksHeader', 'NoOfAttachments', 'CashAccountTitle', 'DebitAccountTitle'].indexOf(c.key) < 0; });
        var body = '<h2>240 - Freight Voucher Register</h2><div class="muted">Browser output of SP_FreightVoucherSlipAndRegister — not the Crystal layout.</div><table><tr>';
        cols.forEach(function (c) { body += '<th>' + esc(c.cap) + '</th>'; });
        body += '</tr>';
        registerGrid.forEach(function (r) {
            body += '<tr>';
            cols.forEach(function (c) {
                var v = r[c.key];
                body += c.t === 'num' ? '<td class="n">' + esc(fmt(v, c.dec)) + '</td>' : '<td>' + esc(c.t === 'date' ? shortDate(v) : str(v)) + '</td>';
            });
            body += '</tr>';
        });
        writeDoc(openWin(), 'Freight Voucher Register', body + '</table>');
    }

    /* btnSlip241_Click (:2008) is not rights-gated on the desktop; only Alt+1 checks Print. */
    function slip241() { printSlip241(RecId); }
    function voucher102() { if (rights.canPrint) printVoucher102(VoucherHeadId); }
    function attachment() {
        box('Attachments are not available on the web page yet. Use the desktop Attachment button for this voucher.');
    }

    /* ================================================================== payment break-up */

    /* btnPaymentBreakUp_Click (:2528) — opens only when the detail grid has rows */
    function openBreakUp() {
        if (table.length === 0) return;
        buUpdateIndex = -1;
        show('wrapCmbChequeNo', !!cfg.ChequeBookEnableStatus);
        show('txtChequeNo', !cfg.ChequeBookEnableStatus);
        bindSelect('CmbTransactionType', [{ Id: 1, Name: 'Cash' }, { Id: 2, Name: 'Bank' }, { Id: 3, Name: 'Other' }], 'Id', 'Name', { blank: true });
        var p = instrumentTypes.length ? Promise.resolve(instrumentTypes) : get('/instrument-types');
        p.then(function (rows) {
            instrumentTypes = rows || [];
            bindSelect('CmbInstrumentType', instrumentTypes, 'Id', 'InstrumentType');
        }).catch(function (e) { box(e.message); });
        buResetDetails();
        renderBreakUp();
        $id('breakUpModal').classList.add('on');
        focusCtl('CmbTransactionType');
    }
    function closeBreakUp() { $id('breakUpModal').classList.remove('on'); }
    function renderBreakUp() { renderGrid('grdBreakUp', BREAKUP_COLS, dtFreightPaymentBreakUp, { totals: true }); }

    /* CmbTransactionType_Leave (:384) */
    function buTransactionTypeLeave() {
        if (intOf(val('CmbTransactionType')) === 2 && instrumentTypes.length > 0) selectValue('CmbInstrumentType', ci(instrumentTypes[0], 'Id'));
        buBindAccounts();
    }
    /* BindAccounts (:400) */
    function buBindAccounts() {
        var id = intOf(val('CmbTransactionType'));
        if (id === 0) {
            $id('CmbAccountTitle').innerHTML = '';
            $id('CmbChequeNo').innerHTML = '';
            return;
        }
        var pred = id === 1 ? function (a) { return a.AccountTypeId === 2; }
                 : id === 2 ? function (a) { return a.AccountTypeId === 15; }
                 : id === 3 ? function (a) { return a.AccountTypeId === 3 || a.AccountTypeId === 6 || a.AccountTypeId === 8; }
                 : function () { return true; };
        bindSelect('CmbAccountTitle', distinctAccounts(pred), 'Id', 'AccountTitle', { attrs: [['code', 'AccountCode']] });
    }
    /* CmbAccountTitle_Leave (:450) -> AccountCurrentBalance + CheqNoFill */
    function buAccountLeave() {
        var acc = intOf(val('CmbAccountTitle'));
        get('/balance', { accountId: acc }).then(function (r) {
            if (r.found) {
                var b = num(r.balance);
                $id('BalanceValue').textContent = fmtBal(b) + (b > 0 ? ' Dr' : (b < 0 ? ' Cr' : ' Nill'));
            } else $id('BalanceValue').textContent = '0';
        }).catch(function (e) { box(e.message); });
        buCheqNoFill();
    }
    function buCheqNoFill() {
        if (intOf(val('CmbTransactionType')) === 2 && cfg.ChequeBookEnableStatus) {
            get('/cheques', { bankId: intOf(val('CmbAccountTitle')), voucherId: VoucherHeadId }).then(function (rows) {
                bindSelect('CmbChequeNo', rows || [], 'Id', 'CheqNo', { blank: false });
                if (!(rows || []).length) $id('CmbChequeNo').innerHTML = '';
                else $id('CmbChequeNo').value = '';
            }).catch(function (e) { box(e.message); });
        } else {
            $id('CmbChequeNo').innerHTML = '';
        }
    }

    /* FormValidation (:257) */
    function buValidation() {
        var tt = selectedText('CmbTransactionType');
        if (!tt) { box('Transaction Type field is required'); focusCtl('CmbTransactionType'); return false; }
        if (tt === 'Bank' && intOf(val('CmbInstrumentType')) === 0) { box('Instrument Type field is required'); focusCtl('CmbInstrumentType'); return false; }
        if (intOf(val('CmbAccountTitle')) === 0) { box('Account Title field is required'); focusCtl('CmbAccountTitle'); return false; }
        if (intOf(val('CmbTransactionType')) === 2 && intOf(val('CmbInstrumentType')) === 1) {
            if (cfg.ChequeBookEnableStatus) {
                if (val('CmbChequeNo') === '') { box('Cheque No Field is Required'); focusCtl('CmbChequeNo'); return false; }
            } else if (val('txtChequeNo') === '') { box('Cheque No field is required'); focusCtl('txtChequeNo'); return false; }
        }
        if (num(val('txtAmount').trim()) <= 0) { box('Net Amount field is Required'); focusCtl('txtAmount'); return false; }
        return true;
    }
    function buRowFromForm(id) {
        return {
            Id: id || 0,
            TransactionTypeId: intOf(val('CmbTransactionType')), TransactionType: selectedText('CmbTransactionType').trim(),
            InstrumentTypeId: intOf(val('CmbInstrumentType')), InstrumentType: selectedText('CmbInstrumentType'),
            AccountTitleId: intOf(val('CmbAccountTitle')), AccountTitle: selectedText('CmbAccountTitle'),
            ChequeId: cfg.ChequeBookEnableStatus ? intOf(val('CmbChequeNo')) : 0,
            ChequeNo: cfg.ChequeBookEnableStatus ? selectedText('CmbChequeNo') : val('txtChequeNo'),
            ChequeDate: val('datChequeDate'), Amount: num(val('txtAmount')),
            PayeeTitle: val('txtPayeeTitle').trim(), Remarks: val('txtRemarks').trim()
        };
    }
    /* btnAdd_Click (:551) */
    function breakUpAdd() {
        try {
            if (!buValidation()) return;
            var amount = 0; dtFreightPaymentBreakUp.forEach(function (b) { amount += num(b.Amount); });
            if (amount + num(val('txtAmount')) > sum('PaidAmount')) throw new Error('Amount is greater than total paid amount');
            dtFreightPaymentBreakUp.push(buRowFromForm(0));
            renderBreakUp();
            buResetDetails();
        } catch (e) { box(e.message); }
    }
    /* btnUpdate_Click (:604) */
    function breakUpUpdate() {
        try {
            if (!buValidation() || buUpdateIndex < 0) return;
            var amount = 0; dtFreightPaymentBreakUp.forEach(function (b) { amount += num(b.Amount); });
            amount -= num(dtFreightPaymentBreakUp[buUpdateIndex].Amount);
            if (amount + num(val('txtAmount')) > sum('PaidAmount')) throw new Error('Amount is greater than total paid amount');
            dtFreightPaymentBreakUp[buUpdateIndex] = buRowFromForm(dtFreightPaymentBreakUp[buUpdateIndex].Id);
            renderBreakUp();
            buResetDetails();
        } catch (e) { box(e.message); }
    }
    function breakUpCancel() { buResetDetails(); }
    /* ResetDetails (:579) */
    function buResetDetails() {
        selectValue('CmbInstrumentType', '');
        $id('CmbAccountTitle').innerHTML = '';
        $id('BalanceValue').textContent = '0';
        selectValue('CmbChequeNo', '');
        setVal('txtChequeNo', ''); setVal('datChequeDate', isoDay(new Date()));
        setVal('txtAmount', ''); setVal('txtPayeeTitle', ''); setVal('txtRemarks', '');
        show('btnAdd', true); show('btnBuUpdate', false); show('btnBuCancel', false);
        buUpdateIndex = -1;
    }
    /* Reset (:199) — the break-up's own New clears every row */
    function breakUpNew() {
        dtFreightPaymentBreakUp = [];
        renderBreakUp();
        selectValue('CmbTransactionType', '');
        buResetDetails();
        focusCtl('CmbTransactionType');
    }
    /* btnRefresh_Click (:700) */
    function breakUpRefresh() {
        Promise.all([get('/accounts'), get('/instrument-types')]).then(function (res) {
            accounts = res[0] || []; instrumentTypes = res[1] || [];
            bindSelect('CmbInstrumentType', instrumentTypes, 'Id', 'InstrumentType');
            buBindAccounts();
            buCheqNoFill();
        }).catch(function (e) { box(e.message); });
    }
    /* grdGdBreakUp_DoubleClick (:304) */
    function breakUpEdit(i) {
        var b = dtFreightPaymentBreakUp[i]; if (!b) return;
        buUpdateIndex = i;
        selectValue('CmbTransactionType', intOf(b.TransactionTypeId));
        buTransactionTypeLeave();
        if (intOf(b.InstrumentTypeId) > 0) selectValue('CmbInstrumentType', intOf(b.InstrumentTypeId));
        selectValue('CmbAccountTitle', intOf(b.AccountTitleId));
        buAccountLeave();
        if (cfg.ChequeBookEnableStatus) setTimeout(function () { selectValue('CmbChequeNo', intOf(b.ChequeId)); }, 400);
        else setVal('txtChequeNo', b.ChequeNo);
        setVal('datChequeDate', dayOf(b.ChequeDate) || isoDay(new Date()));
        setVal('txtAmount', fmt(b.Amount, 3));
        setVal('txtPayeeTitle', b.PayeeTitle); setVal('txtRemarks', b.Remarks);
        show('btnAdd', false); show('btnBuUpdate', true); show('btnBuCancel', true);
        focusCtl('CmbTransactionType');
    }
    /* DeleteDetailRow (:672) */
    function breakUpDelete(i) {
        var b = dtFreightPaymentBreakUp[i]; if (!b) return;
        if (intOf(b.Id) > 0 && !ask('Are you sure to Delete?')) return;
        dtFreightPaymentBreakUp.splice(i, 1);
        renderBreakUp();
    }

    /* ================================================================== tabs, keys */

    function tab(id) {
        currentTab = id;
        ['tabForm', 'tabRegister'].forEach(function (t) { $id(t).classList.toggle('on', t === id); });
        Array.prototype.forEach.call(document.querySelectorAll('.tab'), function (t) { t.classList.toggle('on', t.getAttribute('data-tab') === id); });
        show('tsForm', id === 'tabForm');
    }

    var SHORTCUTS = [
        ['Ctrl+S', 'For Save in Form Tab And For Show History in History Tabs'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print Slip'], ['Alt+2', 'For Print Voucher'],
        ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On Doc Date in Form tab and on From Date in history tab'],
        ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
    ];
    function shortcutKeys() {
        var h = '<thead><tr><th>Key Combination</th><th>Description</th></tr></thead><tbody>';
        SHORTCUTS.forEach(function (s) { h += '<tr><td>' + esc(s[0]) + '</td><td style="white-space:normal">' + esc(s[1]) + '</td></tr>'; });
        $id('shortcutTable').innerHTML = h + '</tbody>';
        $id('shortcutModal').classList.add('on');
    }
    function closeShortcutKeys() { $id('shortcutModal').classList.remove('on'); }

    /* PaymentVoucherNew_KeyDown (:2092) */
    function onKey(e) {
        var k = e.key;
        if ($id('breakUpModal').classList.contains('on')) {
            if (k === 'Escape' || (e.ctrlKey && (k === 'e' || k === 'E'))) { e.preventDefault(); closeBreakUp(); }
            if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); breakUpNew(); }
            return;
        }
        if (e.ctrlKey && e.altKey) { shortcutKeys(); return; }
        if (e.ctrlKey && (k === 'e' || k === 'E')) { e.preventDefault(); window.location.href = '/accounts/dashboard'; return; }
        if (e.ctrlKey && (k === 's' || k === 'S')) {
            e.preventDefault();
            if (currentTab === 'tabForm') { if (!$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) save(); }
            else showRegister();
        }
        if (e.ctrlKey && (k === 'u' || k === 'U')) { e.preventDefault(); if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) update(); }
        if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); if (currentTab === 'tabForm') newForm(); else resetRegister(); }
        if (e.ctrlKey && (k === 'r' || k === 'R') && currentTab === 'tabRegister') { e.preventDefault(); refreshRegister(); }
        if (e.ctrlKey && (k === 't' || k === 'T')) {
            e.preventDefault();
            if (currentTab === 'tabRegister') { tab('tabForm'); $id('voucherdatetime').focus(); } else { tab('tabRegister'); $id('datFromDate').focus(); }
        }
        if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); if (currentTab === 'tabForm') $id('grdPendingRecord').focus(); else $id('grdRegister').focus(); }
        if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); if (currentTab === 'tabForm') $id('voucherdatetime').focus(); else $id('datFromDate').focus(); }
        if (e.ctrlKey && k === 'F5') { e.preventDefault(); $id('voucherdatetime').focus(); }
        if (e.ctrlKey && k === 'F10') { e.preventDefault(); attachment(); }
        if (e.altKey && (k === '1') && rights.canPrint) { e.preventDefault(); slip241(); }
        if (e.altKey && (k === '2') && rights.canPrint) { e.preventDefault(); voucher102(); }
    }

    function wireEvents() {
        document.addEventListener('keydown', onKey);
        $id('rdCashAccount').addEventListener('change', cashAccountFillFromGlobal);
        $id('rdCreditAccount').addEventListener('change', cashAccountFillFromGlobal);
        $id('CmbCashAccount').addEventListener('change', accountCurrentBalance);
        $id('txtAllowShortage').addEventListener('keypress', function (e) {
            if (!/[0-9.]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') >= 0)) e.preventDefault();
        });
        $id('txtAllowShortage').addEventListener('input', allowShortageChanged);
        $id('txtCNIC').addEventListener('blur', cnicLeave);
        $id('txtDriverCellNo').addEventListener('blur', cellLeave);
        ['txtGpNoFrom', 'txtGpNoTo'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) { if (!/[0-9]/.test(e.key)) e.preventDefault(); });
        });
        $id('txtAmount').addEventListener('keypress', function (e) {
            if (!/[0-9.]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') >= 0)) e.preventDefault();
        });
        $id('CmbTransactionType').addEventListener('change', buTransactionTypeLeave);
        $id('CmbAccountTitle').addEventListener('change', buAccountLeave);

        /* detail grid: commit an edited cell -> grd_CellUpdated */
        $id('grd').addEventListener('change', function (e) {
            var t = e.target, k = t.getAttribute('data-k'); if (!k) return;
            var i = +t.getAttribute('data-i'), row = table[i]; if (!row) return;
            if (k === 'Remarks') row.Remarks = t.value;
            else if (k === 'DebitAccountId') row.DebitAccountId = intOf(t.value);
            else row[k] = num(t.value);
            cellUpdated(i, k);
        });
        $id('grd').addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target.getAttribute('data-k')) { e.preventDefault(); e.target.blur(); }
        });
        $id('grdPendingRecord').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act="Load"]'); if (!b) return;
            loadRecord(pendingRows[+b.getAttribute('data-i')]);
        });
        $id('grdRegister').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var r = registerGrid[+b.getAttribute('data-i')]; if (!r) return;
            var act = b.getAttribute('data-act');
            if (act === 'Edit') { readById(intOf(r.Id)); }
            else if (act === 'Print') printSlip241(intOf(r.Id));
            else if (act === 'Voucher') printVoucher102(intOf(r.VoucherHeadId));
            else if (act === 'AddAttachment') attachment();
        });
        $id('grdRegister').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr || e.target.closest('button')) return;
            var r = registerGrid[+tr.getAttribute('data-i')]; if (r) readById(intOf(r.Id));
        });
        $id('grdBreakUp').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act="Delete"]'); if (b) breakUpDelete(+b.getAttribute('data-i'));
        });
        $id('grdBreakUp').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr || e.target.closest('button')) return;
            breakUpEdit(+tr.getAttribute('data-i'));
        });
        ['grdPendingRecord', 'grdRegister', 'grdBreakUp', 'grdPreviousInformation', 'grd'].forEach(function (id) {
            $id(id).addEventListener('click', function (e) {
                var tr = e.target.closest('tbody tr'); if (!tr) return;
                Array.prototype.forEach.call(this.querySelectorAll('tbody tr.cur'), function (x) { x.classList.remove('cur'); });
                tr.classList.add('cur');
            });
        });
    }

    window.FreightVoucher = {
        newForm: newForm, refresh: refresh, save: save, update: update, slip241: slip241, voucher102: voucher102,
        attachment: attachment, shortcutKeys: shortcutKeys, closeShortcutKeys: closeShortcutKeys,
        openBreakUp: openBreakUp, closeBreakUp: closeBreakUp, breakUpNew: breakUpNew, breakUpRefresh: breakUpRefresh,
        breakUpAdd: breakUpAdd, breakUpUpdate: breakUpUpdate, breakUpCancel: breakUpCancel,
        resetDriverInfo: resetDriverInfo, tab: tab,
        showRegister: showRegister, resetRegister: resetRegister, refreshRegister: refreshRegister, print240: print240
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
