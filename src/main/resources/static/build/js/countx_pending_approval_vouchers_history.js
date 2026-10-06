/* ============================================================================================
 * Approvals - vouchers
 * Ported from Architecture.WinApp.ApprovalDashboard\PendingApprovalVouchersHistory.cs (1,777 lines).
 *
 * URL: /dashboard/pending-approval-vouchers-history?id=<card TypeID>&fromDate=&toDate=[&unApprove=1][&popup=1]
 *   id         PendingApprovalVouchersHistory.Id (static) - the server maps it to the DocumentTypeId (:183-197)
 *   fromDate   RequestedFromDate - only when the dashboard's From tick box is set
 *   toDate     RequestedToDate   - always
 *   unApprove  flagAproved       - set by UnApprovedInvoicesAndVouchers (:170)
 *
 *   :177-226  Load                     dates, mode, PendingForViewVouchers + HistoryFill / PendingVoucherForUnApprovalDashboard
 *   :228-275  HistoryFill              grd
 *   :277-322  PendingVoucherForUnApprovalDashboard
 *   :324-383  PendingForViewVouchers   grdPendingViewVoucher
 *   :385-505  Gridsetting              :507-575 GridPendingViewsetting   :577-605 GridDetailsetting
 *   :607-633  grd_ColumnButtonClick    :635-663 grd_LinkClicked
 *   :665-694  ApproveVoucher           :696-715 UnApproveVoucher
 *   :717-741  GridDetailByVoucherHeadId (grd_SelectionChanged)
 *   :809-882  btnApproved_Click        :884-911 btnShow_Click
 *   :913-950  grdDetail_LinkClicked / grdPendingViewVoucher_ColumnButtonClick
 *   :952-962  Print -> CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(id)
 *   :964-990  ApproveVoucherForPendingGrid   :992-1027 btnPendingForView_Click
 *   :1029-1050 grdPendingViewVoucher_LinkClicked   :1052-1080 tabControl2_SelectedIndexChanged
 *   :1082-1219 Ctrl+Space on grid cells       :1237-1300 form KeyDown
 * ==========================================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/pending-approval-vouchers';
var PRINT_102 = '/reports/print/102-a-new-ac-payment-receipts-voucher-slip';

var qs = new URLSearchParams(location.search);
var S = {
    id: toI(qs.get('id')),
    reqFrom: qs.get('fromDate') || '',
    reqTo: qs.get('toDate') || '',
    flagAproved: qs.get('unApprove') === '1',
    popup: qs.get('popup') === '1',
    documentTypeId: 0,
    allowEdit: false,
    busy: false
};

/* ---------------------------------------------------------------- helpers */
function el(id) { return document.getElementById(id); }
function txt(v) { return v === null || v === undefined ? '' : String(v); }
function esc(v) { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function toI(v) { var n = parseInt(txt(v).trim(), 10); return isNaN(n) ? 0 : n; }
function toN(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }
/* Conversion.ToInt(text): anything that is not a whole number is 0. */
function convInt(v) { var s = txt(v).trim(); return /^-?\d+$/.test(s) ? parseInt(s, 10) : 0; }
function col(row, name) {
    if (!row) return null;
    if (row[name] !== undefined) return row[name];
    var low = name.toLowerCase();
    for (var k in row) { if (k.toLowerCase() === low) return row[k]; }
    return null;
}

var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
function parseDt(v) {
    var s = txt(v).trim();
    var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(s);
    if (!m) return null;
    return { y: +m[1], mo: +m[2], d: +m[3], h: m[4] ? +m[4] : 0, mi: m[5] ? +m[5] : 0 };
}
function pad(n) { return n < 10 ? '0' + n : String(n); }
function fmtDate(v) {                     /* dd-MMM-yy */
    var d = parseDt(v); if (!d) return txt(v);
    return pad(d.d) + '-' + MON[d.mo - 1] + '-' + String(d.y).slice(-2);
}
function fmtDateTime(v) {                 /* dd-MMM-yyyy hh:mm tt */
    var d = parseDt(v); if (!d) return txt(v);
    var h12 = d.h % 12 === 0 ? 12 : d.h % 12;
    return pad(d.d) + '-' + MON[d.mo - 1] + '-' + d.y + ' ' + pad(h12) + ':' + pad(d.mi) + ' ' + (d.h < 12 ? 'AM' : 'PM');
}
function group(intPart) { return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
/* "#,##0.###" */
function fmt3(v) {
    if (v === null || v === undefined || v === '') return '';
    var n = toN(v), neg = n < 0;
    var s = Math.abs(n).toFixed(3).replace(/\.?0+$/, '');
    var p = s.split('.');
    return (neg ? '-' : '') + group(p[0]) + (p[1] ? '.' + p[1] : '');
}
/* "#,#" - whole number with grouping, zero shows nothing */
function fmtHash(v) {
    if (v === null || v === undefined || v === '') return '';
    var n = Math.round(toN(v));
    if (n === 0) return '';
    return (n < 0 ? '-' : '') + group(String(Math.abs(n)));
}

function getJson(url) {
    return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(readJson);
}
function postJson(url, body) {
    return fetch(url, { method: 'POST', credentials: 'same-origin',
        headers: { 'Accept': 'application/json', 'Content-Type': 'application/json' },
        body: JSON.stringify(body || {}) }).then(readJson);
}
function readJson(r) {
    if (r.redirected || r.status === 401) throw new Error('Please sign in to continue');
    return r.text().then(function (t) {
        var d = null;
        try { d = t ? JSON.parse(t) : {}; } catch (e) { d = { message: t }; }
        if (!r.ok) throw new Error((d && (d.message || d.error)) || ('HTTP ' + r.status));
        return d || {};
    });
}

function setBusy(on) {
    S.busy = !!on;
    ['btnShow', 'btnApproved', 'btnPendingForView'].forEach(function (id) { var b = el(id); if (b) b.disabled = !!on; });
}

/* ---------------------------------------------------------------- Janus GridEX (subset) */
function Grid(id, opts) {
    this.id = id;
    this.table = el(id);
    this.opts = opts;
    this.columns = [];
    this.rows = [];
    this.view = [];
    this.filters = {};
    this.cur = -1;
    this.curCol = null;
    this.checked = {};
    var self = this;
    this.nav = document.querySelector('.jg-nav[data-for="' + id + '"]');
    this.table.addEventListener('click', function (e) { self.onClick(e); });
    this.table.addEventListener('change', function (e) { self.onChange(e); });
    this.table.addEventListener('input', function (e) {
        var t = e.target;
        if (t && t.getAttribute('data-flt')) { self.filters[t.getAttribute('data-flt')] = t.value; self.applyFilter(); }
    });
    this.table.addEventListener('keydown', function (e) { self.onKey(e); });
    if (this.nav) {
        this.nav.innerHTML = '<button type="button" data-n="first">|&lt;</button><button type="button" data-n="prev">&lt;</button>'
            + '<span></span><button type="button" data-n="next">&gt;</button><button type="button" data-n="last">&gt;|</button>';
        this.nav.addEventListener('click', function (e) {
            var b = e.target.closest('button'); if (!b) return;
            var n = b.getAttribute('data-n'), len = self.view.length;
            if (!len) return;
            var i = self.cur < 0 ? 0 : self.cur;
            if (n === 'first') i = 0; else if (n === 'prev') i = Math.max(0, i - 1);
            else if (n === 'next') i = Math.min(len - 1, i + 1); else if (n === 'last') i = len - 1;
            self.setCurrent(i, true);
        });
    }
}
Grid.prototype.visibleCols = function () { return this.columns.filter(function (c) { return !c.hidden; }); };
/* DataSource = dt; RetrieveStructure(); ...setting() */
Grid.prototype.setData = function (columns, rows) {
    this.columns = columns || [];
    this.rows = rows || [];
    this.filters = {};
    this.checked = {};
    this.cur = -1;
    this.renderHead();
    this.applyFilter();
};
/* DataSource = null / ClearStructure() */
Grid.prototype.clear = function () {
    this.columns = []; this.rows = []; this.view = []; this.filters = {}; this.checked = {}; this.cur = -1;
    this.table.innerHTML = '';
    this.table.style.width = '';
    this.updateNav();
};
/* FrozenColumns */
Grid.prototype.frozenLefts = function () {
    var n = this.opts.frozen || 0, left = 0, out = {};
    var vis = this.visibleCols();
    for (var i = 0; i < vis.length && i < n; i++) { out[vis[i].key] = left; left += vis[i].width || 100; }
    return out;
};
Grid.prototype.renderHead = function () {
    var vis = this.visibleCols(), fz = this.frozenLefts(), self = this;
    var total = 0;
    var cg = '<colgroup>' + vis.map(function (c) { var w = c.width || 100; total += w; return '<col style="width:' + w + 'px">'; }).join('') + '</colgroup>';
    var h = '<thead><tr>' + vis.map(function (c) {
        var isF = fz[c.key] !== undefined;
        var attrs = ' data-col="' + esc(c.key) + '"' + (isF ? ' style="left:' + fz[c.key] + 'px"' : '');
        if (c.selector) return '<th class="c' + (isF ? ' fz' : '') + '"' + attrs + '><input type="checkbox" data-selall="1"></th>';
        return '<th' + (isF ? ' class="fz"' : '') + attrs + '>' + esc(c.caption) + '</th>';
    }).join('') + '</tr>';
    /* the filter row carries rk-filter so countx_grid_bar keys its columns from the caption row */
    h += '<tr class="flt rk-filter">' + vis.map(function (c) {
        var f = fz[c.key] !== undefined ? ' class="fz" style="left:' + fz[c.key] + 'px"' : '';
        if (c.selector || c.button) return '<td' + f + '></td>';
        return '<td' + f + '><input type="text" data-flt="' + esc(c.key) + '" value="' + esc(self.filters[c.key] || '') + '"></td>';
    }).join('') + '</tr></thead>';
    this.table.style.width = total + 'px';
    this.table.innerHTML = cg + h + '<tbody></tbody><tfoot></tfoot>';
};
Grid.prototype.display = function (c, row) {
    var v = col(row, c.key);
    if (c.fmt) return c.fmt(v, row);
    return txt(v);
};
/* FilterMode Automatic, filter row */
Grid.prototype.applyFilter = function () {
    var self = this, vis = this.visibleCols();
    var active = vis.filter(function (c) { return txt(self.filters[c.key]).trim() !== ''; });
    this.view = this.rows.filter(function (r) {
        return active.every(function (c) {
            return self.display(c, r).toLowerCase().indexOf(txt(self.filters[c.key]).trim().toLowerCase()) >= 0;
        });
    });
    this.renderBody();
    if (this.view.length) this.setCurrent(0, false); else { this.cur = -1; this.updateNav(); }
};
Grid.prototype.renderBody = function () {
    var self = this, vis = this.visibleCols(), fz = this.frozenLefts();
    var tb = this.table.tBodies[0], tf = this.table.tFoot;
    if (!tb) return;
    tb.innerHTML = this.view.map(function (r, i) {
        var chk = !!self.checked[txt(col(r, 'Id'))];
        return '<tr data-i="' + i + '"' + (chk ? ' class="chk"' : '') + '>' + vis.map(function (c) {
            var cls = (c.align === 'r' ? 'r' : c.align === 'c' ? 'c' : '');
            var st = '';
            if (fz[c.key] !== undefined) { cls += ' fz'; st = ' style="left:' + fz[c.key] + 'px"'; }
            var inner;
            if (c.selector) { inner = '<input type="checkbox" data-sel="1"' + (chk ? ' checked' : '') + '>'; cls += ' c'; }
            else if (c.button) inner = '<button type="button" class="cb" data-btn="' + esc(c.key) + '">' + esc(c.button) + '</button>';
            else {
                var t = self.display(c, r);
                inner = (c.link && c.link(r)) ? '<a class="lnk" data-lnk="' + esc(c.key) + '">' + esc(t) + '</a>' : esc(t);
            }
            return '<td class="' + cls.trim() + '" data-c="' + esc(c.key) + '"' + st + '>' + inner + '</td>';
        }).join('') + '</tr>';
    }).join('');
    /* TotalRow, AggregateFunction Sum */
    var any = vis.some(function (c) { return c.sum; });
    tf.innerHTML = any ? '<tr>' + vis.map(function (c) {
        var cls = c.align === 'r' ? 'r' : '', st = '';
        if (fz[c.key] !== undefined) { cls += ' fz'; st = ' style="left:' + fz[c.key] + 'px"'; }
        var t = '';
        if (c.sum) {
            var s = 0; self.view.forEach(function (r) { s += toN(col(r, c.key)); });
            t = c.totalFmt ? c.totalFmt(s) : String(s);
        }
        return '<td class="' + cls.trim() + '"' + st + '>' + esc(t) + '</td>';
    }).join('') + '</tr>' : '';
    this.syncSelAll();
};
Grid.prototype.syncSelAll = function () {
    var self = this, all = this.table.querySelector('input[data-selall]');
    if (all) all.checked = this.view.length > 0 && this.view.every(function (r) { return self.checked[txt(col(r, 'Id'))]; });
};
Grid.prototype.setCurrent = function (i, scroll) {
    var tb = this.table.tBodies[0];
    if (!tb) return;
    var old = tb.querySelector('tr.cur'); if (old) old.classList.remove('cur');
    var oc = tb.querySelector('td.cc'); if (oc) oc.classList.remove('cc');
    this.cur = i;
    var tr = tb.querySelector('tr[data-i="' + i + '"]');
    if (tr) {
        tr.classList.add('cur');
        if (scroll) tr.scrollIntoView({ block: 'nearest' });
        if (this.curCol) { var td = tr.querySelector('td[data-c="' + this.curCol + '"]'); if (td) td.classList.add('cc'); }
    }
    this.updateNav();
    if (this.opts.onSelect && this.view[i]) this.opts.onSelect(this.view[i]);
};
Grid.prototype.updateNav = function () {
    if (!this.nav) return;
    var sp = this.nav.querySelector('span');
    if (sp) sp.textContent = this.view.length ? ('Record ' + (this.cur + 1) + ' of ' + this.view.length) : '';
};
Grid.prototype.currentRow = function () { return this.cur >= 0 ? this.view[this.cur] : null; };
/* GetCheckedRows() - in grid order */
Grid.prototype.checkedRows = function () {
    var self = this;
    return this.rows.filter(function (r) { return self.checked[txt(col(r, 'Id'))]; });
};
Grid.prototype.onClick = function (e) {
    var t = e.target;
    if (t.getAttribute && (t.getAttribute('data-flt') || t.getAttribute('data-sel') || t.getAttribute('data-selall'))) return;
    var tr = t.closest('tbody tr[data-i]');
    if (!tr) return;
    var i = +tr.getAttribute('data-i');
    var td = t.closest('td[data-c]');
    if (td) this.curCol = td.getAttribute('data-c');
    if (i !== this.cur) this.setCurrent(i, false);
    else {
        var cc = this.table.querySelector('td.cc'); if (cc) cc.classList.remove('cc');
        if (td) td.classList.add('cc');
    }
    var row = this.view[i];
    var b = t.closest('button[data-btn]');
    if (b && this.opts.onButton) { this.opts.onButton(b.getAttribute('data-btn'), row); return; }
    var a = t.closest('a[data-lnk]');
    if (a && this.opts.onLink) this.opts.onLink(a.getAttribute('data-lnk'), row);
};
Grid.prototype.onChange = function (e) {
    var t = e.target, self = this;
    if (t.getAttribute('data-selall')) {
        /* UseHeaderSelector */
        this.view.forEach(function (r) { var k = txt(col(r, 'Id')); if (t.checked) self.checked[k] = true; else delete self.checked[k]; });
        Array.prototype.forEach.call(this.table.querySelectorAll('tbody tr[data-i]'), function (tr) {
            var cb = tr.querySelector('input[data-sel]'); if (cb) cb.checked = t.checked;
            tr.classList.toggle('chk', t.checked);
        });
        return;
    }
    if (t.getAttribute('data-sel')) {
        var tr = t.closest('tr[data-i]'); if (!tr) return;
        var r = this.view[+tr.getAttribute('data-i')], k = txt(col(r, 'Id'));
        if (t.checked) this.checked[k] = true; else delete this.checked[k];
        tr.classList.toggle('chk', t.checked);
        this.syncSelAll();
    }
};
Grid.prototype.onKey = function (e) {
    if (e.target && e.target.getAttribute && e.target.getAttribute('data-flt')) return;
    if (e.key === 'ArrowDown' && !e.ctrlKey) { if (this.cur < this.view.length - 1) this.setCurrent(this.cur + 1, true); e.preventDefault(); }
    else if (e.key === 'ArrowUp' && !e.ctrlKey) { if (this.cur > 0) this.setCurrent(this.cur - 1, true); e.preventDefault(); }
    else if (e.ctrlKey && (e.key === ' ' || e.code === 'Space')) {
        e.preventDefault();
        var r = this.currentRow();
        if (r && this.curCol && this.opts.onCtrlSpace) this.opts.onCtrlSpace(this.curCol, r);
    }
};

/* ---------------------------------------------------------------- column sets */
function always() { return true; }
/* CreateApprovalTable() order with Gridsetting (grd) / GridPendingViewsetting (grdPendingViewVoucher). */
function voucherCols(kind) {
    var is203 = S.documentTypeId === 203;
    var main = kind === 'grd';
    var c = [];
    if (main) c.push({ key: 'Select', caption: '', width: 25, selector: true });      /* ActAsSelector, Position 0 */
    c.push({ key: 'Print', caption: 'Print', width: 40, button: 'Print' });
    if (main && S.flagAproved) c.push({ key: 'UnPost', caption: 'UnApprove', width: 60, button: 'UnApprove' });
    else c.push({ key: 'Post', caption: 'Approve', width: 60, button: 'Approve' });
    c.push(
        { key: 'Id', hidden: true },
        { key: 'DocumentTypeId', hidden: true },
        { key: 'DocumentTypeCode', caption: 'V Type', width: 60 },
        { key: 'DocumentTypeSrNo', hidden: true },
        { key: 'VoucherCode', caption: 'V Code', width: 50, link: function () { return S.allowEdit; } },
        { key: 'VoucherDate', caption: 'V Date', width: main ? 90 : 88, fmt: fmtDate },
        { key: 'RefAccountId', hidden: true },
        { key: 'AccountTitle', caption: 'AccountTitle', width: 250, link: always },
        { key: 'Remarks', caption: 'Remarks', width: 300 },
        { key: 'VoucherAmount', caption: 'VoucherAmount', width: 130, align: 'r', fmt: fmt3, sum: true, totalFmt: fmt3 },
        { key: 'ChequeNo', caption: 'ChequeNo', width: 75, hidden: is203 },
        { key: 'ChequeDate', caption: 'ChequeDate', width: 80, hidden: is203, fmt: fmtDate },
        { key: 'PayTitle', caption: 'PayTitle', width: 120, hidden: is203 },
        { key: 'CurrencyCode', caption: 'CurrencyCode', width: 75, hidden: !is203 },
        { key: 'ExchangeCurrencyRate', caption: main ? 'Exchange Rate' : 'ExchangeCurrencyRate', width: 70, hidden: !is203, align: 'r', fmt: fmt3 },
        { key: 'FcAmount', caption: 'FcAmount', width: 100, hidden: !is203, align: 'r', fmt: fmt3, sum: true, totalFmt: fmt3 },
        { key: 'EntryUser', caption: 'EntryUser', width: main ? 100 : 110 },
        { key: 'EntryDate', caption: 'EntryDate', width: 145, fmt: fmtDateTime },
        { key: 'IsApproved', hidden: true },
        { key: 'NoOfAttachments', caption: 'NoOfAttachments', width: 85, align: 'r', link: always }
    );
    return c;
}
/* dtApproval.Rows.Add(...) - Vouchercode -> VoucherCode, UserName -> EntryUser. */
function toApprovalRows(rows) {
    return (rows || []).map(function (r) {
        return {
            Id: col(r, 'Id'), DocumentTypeId: col(r, 'DocumentTypeId'), DocumentTypeCode: col(r, 'DocumentTypeCode'),
            DocumentTypeSrNo: col(r, 'DocumentTypeSrNo'), VoucherCode: col(r, 'Vouchercode'), VoucherDate: col(r, 'VoucherDate'),
            RefAccountId: col(r, 'RefAccountId'), AccountTitle: col(r, 'AccountTitle'), Remarks: col(r, 'Remarks'),
            VoucherAmount: col(r, 'VoucherAmount'), ChequeNo: col(r, 'ChequeNo'), ChequeDate: col(r, 'ChequeDate'),
            PayTitle: col(r, 'PayTitle'), CurrencyCode: col(r, 'CurrencyCode'), ExchangeCurrencyRate: col(r, 'ExchangeCurrencyRate'),
            FcAmount: col(r, 'FcAmount'), EntryUser: col(r, 'UserName'), EntryDate: col(r, 'EntryDate'),
            IsApproved: col(r, 'IsApproved'), NoOfAttachments: col(r, 'NoOfAttachments')
        };
    });
}
/* GridDetailsetting :577-605 - the procedure's own column order (AccountTitle +50, Comments Remarks+100, Amount). */
var DETAIL_COLS = [
    { key: 'AccountTitle', caption: 'AccountTitle', width: 200, link: always },
    { key: 'OffsetAccount', caption: 'OffsetAccount', width: 200, link: always },
    { key: 'Comments', caption: 'Comments', width: 200 },
    { key: 'DebitAmount', caption: 'DebitAmount', width: 90, align: 'r', fmt: fmtHash, sum: true, totalFmt: fmtHash },
    { key: 'CreditAmount', caption: 'CreditAmount', width: 90, align: 'r', fmt: fmtHash, sum: true, totalFmt: fmtHash },
    { key: 'JobLot', caption: 'JobLot', width: 100 },
    { key: 'AccountId', hidden: true },
    { key: 'AgainstAccountId', hidden: true }
];

/* ---------------------------------------------------------------- grids */
var grd = new Grid('grd', {
    frozen: 3,
    onSelect: function (r) { GridDetailByVoucherHeadId(r); },              /* grd_SelectionChanged */
    onButton: function (key, r) { grdColumnButton(key, r); },
    onLink: function (key, r) { grdLink(key, r); },
    onCtrlSpace: function (key, r) {                                         /* grd_KeyDown */
        if (key === 'VoucherCode') { if (S.allowEdit) editLinked(r, false); }
        else if (key === 'NoOfAttachments') attachments(toI(col(r, 'Id')), toI(col(r, 'DocumentTypeId')));
        else if (key === 'AccountTitle') openLedger(col(r, 'RefAccountId'));
        else if (key === 'Print') Print(toI(col(r, 'Id')));
        else if (key === 'UnPost') UnApproveVoucher(toI(col(r, 'Id')));
        else if (key === 'Post') ApproveVoucher(toI(col(r, 'Id')));
    }
});
var grdDetail = new Grid('grdDetail', {
    onLink: function (key, r) { detailLink(key, r); },
    onCtrlSpace: function (key, r) { detailLink(key, r); }                   /* grdDetail_KeyDown */
});
var grdPending = new Grid('grdPendingViewVoucher', {
    frozen: 2,
    onButton: function (key, r) { pendingColumnButton(key, r); },
    onLink: function (key, r) { pendingLink(key, r); },
    onCtrlSpace: function (key, r) {                                         /* grdPendingViewVoucher_KeyDown */
        if (key === 'VoucherCode') { if (S.allowEdit) editLinked(r, true); }
        else if (key === 'NoOfAttachments') attachments(toI(col(r, 'Id')), toI(col(r, 'DocumentTypeId')));
        else if (key === 'AccountTitle') openLedger(col(r, 'RefAccountId'));
        else if (key === 'Print') Print(toI(col(r, 'Id')));
        else if (key === 'Post') ApproveVoucherForPendingGrid(toI(col(r, 'Id')));
    }
});

/* ---------------------------------------------------------------- reads */
function filterQuery() {
    return 'id=' + S.id
        + '&fromDate=' + encodeURIComponent(el('fromdate').value || '')
        + '&toDate=' + encodeURIComponent(el('todate').value || '')
        + '&docNoFrom=' + convInt(el('txtdocnofrom').value)
        + '&docNoTo=' + convInt(el('txtdocnoto').value);
}
/* HistoryFill :228-275 */
function HistoryFill() {
    return getJson(API + '/history?' + filterQuery()).then(function (d) {
        var rows = d.rows || [];
        if (rows.length) grd.setData(voucherCols('grd'), toApprovalRows(rows));
        else grd.clear();
    }).catch(function (e) { alert(e.message); });
}
/* PendingVoucherForUnApprovalDashboard :277-322 */
function PendingVoucherForUnApprovalDashboard() {
    return getJson(API + '/history?' + filterQuery() + '&unApprove=1').then(function (d) {
        var rows = d.rows || [];
        if (rows.length) grd.setData(voucherCols('grd'), toApprovalRows(rows));
        else grd.clear();
    }).catch(function (e) { alert(e.message); });
}
/* PendingForViewVouchers :324-383 */
function PendingForViewVouchers() {
    return getJson(API + '/pending-for-view?id=' + S.id).then(function (d) {
        var rows = d.rows || [];
        if (rows.length) grdPending.setData(voucherCols('pending'), toApprovalRows(rows));
        else grdPending.clear();
    }).catch(function (e) { alert(e.message); });
}
/* GridDetailByVoucherHeadId :717-741 */
var detailSeq = 0;
function GridDetailByVoucherHeadId(r) {
    var id = toI(col(r, 'Id')), seq = ++detailSeq;
    getJson(API + '/detail?voucherHeadId=' + id).then(function (d) {
        if (seq !== detailSeq) return;
        var rows = d.rows || [];
        if (rows.length) grdDetail.setData(DETAIL_COLS, rows);
        else grdDetail.clear();
    }).catch(function (e) { if (seq === detailSeq) alert(e.message); });
}

/* ---------------------------------------------------------------- actions */
function update(path, ids) { return postJson(API + '/' + path, { ids: ids }); }

/* grd_ColumnButtonClick :607-633 */
function grdColumnButton(key, r) {
    var id = toI(col(r, 'Id'));
    if (key === 'Print') Print(id);
    if (S.flagAproved) { if (key === 'UnPost') UnApproveVoucher(id); }
    else if (key === 'Post') ApproveVoucher(id);
}
/* ApproveVoucher :665-694 */
function ApproveVoucher(id) {
    if (S.busy) return;
    if (!confirm('Are you sure to Approve Voucher?')) return;
    setBusy(true);
    update('approve', [id]).then(function () {
        setBusy(false);
        alert('Record Approved Successfully');
        if (el('ChkPrint').checked) Print(id);
        return HistoryFill();
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
/* UnApproveVoucher :696-715 */
function UnApproveVoucher(id) {
    if (S.busy) return;
    if (!confirm('Are you sure to UnApprove Voucher?')) return;
    setBusy(true);
    update('unapprove', [id]).then(function () {
        setBusy(false);
        return PendingVoucherForUnApprovalDashboard().then(function () { alert('Record UnApproved Successfully'); });
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
/* btnApproved_Click :809-882 */
function btnApproved_Click() {
    if (S.busy) return;
    var checkedRows = grd.checkedRows();
    if (!checkedRows.length) { alert('Please select check box first'); return; }
    var ids = checkedRows.map(function (r) { return toI(col(r, 'Id')); });
    if (S.flagAproved) {
        if (!confirm('Are you sure to UnApprove Voucher?')) return;
        setBusy(true);
        update('unapprove', ids).then(function () {
            setBusy(false);
            alert('Record UnApproved Successfully');
            return PendingVoucherForUnApprovalDashboard();
        }).catch(function (e) { setBusy(false); alert(e.message); });
        return;
    }
    if (!confirm('Are you sure to Approve Voucher?')) return;
    setBusy(true);
    update('approve', ids).then(function () {
        setBusy(false);
        alert('Record Approved Successfully');
        return HistoryFill();
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
/* btnPendingForView_Click :992-1027 */
function btnPendingForView_Click() {
    if (S.busy) return;
    var checkedRows = grd.checkedRows();
    if (!checkedRows.length) { alert('Please select check box first'); return; }
    if (!confirm('Are you sure to Move Voucher in PendingGrid?')) return;
    var ids = checkedRows.map(function (r) { return toI(col(r, 'Id')); });
    setBusy(true);
    update('move-to-pending', ids).then(function () {
        setBusy(false);
        alert('Record Move on PendingGrid Successfully');
        return HistoryFill().then(PendingForViewVouchers);
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
/* grdPendingViewVoucher_ColumnButtonClick :931-950 */
function pendingColumnButton(key, r) {
    var id = toI(col(r, 'Id'));
    if (key === 'Print') Print(id);
    else if (key === 'Post') ApproveVoucherForPendingGrid(id);
}
/* ApproveVoucherForPendingGrid :964-990 */
function ApproveVoucherForPendingGrid(id) {
    if (S.busy) return;
    if (!confirm('Are you sure to Approve Voucher?')) return;
    setBusy(true);
    update('approve', [id]).then(function () {
        setBusy(false);
        return PendingForViewVouchers().then(function () { alert('Record Approved Successfully'); });
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
/* btnShow_Click :884-911 */
function btnShow_Click() {
    if (S.busy) return;
    setBusy(true);
    var p = S.flagAproved ? PendingVoucherForUnApprovalDashboard() : PendingForViewVouchers().then(HistoryFill);
    p.then(function () { setBusy(false); }, function () { setBusy(false); });
}

/* Print :952-962 -> CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(id) */
function Print(id) {
    if (!(id > 0)) { alert('VoucherId Not Found'); return; }
    var w = window.open('about:blank', '_blank');
    fetch(PRINT_102, { method: 'POST', credentials: 'same-origin',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/pdf' },
        body: JSON.stringify({ id: id }) }).then(function (r) {
        var type = r.headers.get('Content-Type') || '';
        if (r.ok && type.indexOf('application/pdf') === 0) {
            return r.blob().then(function (b) { if (w) w.location = URL.createObjectURL(b); });
        }
        return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
    }).catch(function (e) { if (w) w.close(); alert(e.message); });
}

/* ---------------------------------------------------------------- links */
/* CommonServices.GoToGeneralLedgerFromLinkedEvent(AccountId, fromdate.Value, todate.Value) */
function openLedger(accountId) {
    var q = new URLSearchParams();
    q.set('accountId', toI(accountId));
    if (el('fromdate').value) q.set('fromDate', el('fromdate').value);
    if (el('todate').value) q.set('toDate', el('todate').value);
    window.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
}
/* CommonServices.EditMethodFromLinked - grd passes DocumentTypeSrNo, grdPendingViewVoucher does not. */
function editLinked(r, pendingGrid) {
    if (!window.DocLink) { alert('Document links are not available on this page.'); return; }
    var opts = { message: function (m) { alert(m); } };
    if (!pendingGrid) opts.srNo = toI(col(r, 'DocumentTypeSrNo'));
    window.DocLink.open(toI(col(r, 'DocumentTypeId')), toI(col(r, 'Id')), opts);
}
/* grd_LinkClicked :635-663 */
function grdLink(key, r) {
    var id = toI(col(r, 'Id')), docType = toI(col(r, 'DocumentTypeId')), srNo = toI(col(r, 'DocumentTypeSrNo'));
    if (key === 'VoucherCode') editLinked(r, false);
    if (key === 'NoOfAttachments') attachments(docType === 203 ? srNo : id, docType);
    if (key === 'AccountTitle') openLedger(col(r, 'RefAccountId'));
}
/* grdPendingViewVoucher_LinkClicked :1029-1050 */
function pendingLink(key, r) {
    if (key === 'VoucherCode') editLinked(r, true);
    if (key === 'NoOfAttachments') attachments(toI(col(r, 'Id')), toI(col(r, 'DocumentTypeId')));
    if (key === 'AccountTitle') openLedger(col(r, 'RefAccountId'));
}
/* grdDetail_LinkClicked :913-929 */
function detailLink(key, r) {
    if (key === 'AccountTitle') openLedger(col(r, 'AccountId'));
    if (key === 'OffsetAccount') openLedger(col(r, 'AgainstAccountId'));
}
/* CommonServices.GetNoofAttachmentsByRefDocumentTypeID - AttachmentView opens only when there is a row. */
function attachments(refId, documentTypeId) {
    getJson(API + '/attachments?refId=' + refId + '&documentTypeId=' + documentTypeId).then(function (d) {
        var rows = d.rows || [];
        if (!rows.length) return;
        el('attBody').innerHTML = rows.map(function (a) {
            return '<tr><td>' + esc(a.AttachmentName) + '</td><td>' + esc(a.CustomName) + '</td><td>' + esc(fmtDateTime(a.EntryDate)) + '</td></tr>';
        }).join('');
        el('attModal').style.display = 'flex';
    }).catch(function (e) { alert(e.message); });
}

/* ---------------------------------------------------------------- tabs */
var T2 = ['tabPage2', 'tabPage3', 'tabPage4'];
function tab2(id) {
    T2.forEach(function (p) { el(p).classList.toggle('on', p === id); });
    Array.prototype.forEach.call(document.querySelectorAll('#t2Strip a'), function (a) { a.classList.toggle('on', a.getAttribute('data-tab') === id); });
}
function tab2Index() { var on = document.querySelector('#t2Strip a.on'); return Math.max(0, T2.indexOf(on ? on.getAttribute('data-tab') : 'tabPage2')); }
function tab1(id) {
    ['tabPage1', 'tabPendingVouchersForView'].forEach(function (p) { var e = el(p); if (e) e.classList.toggle('on', p === id); });
    Array.prototype.forEach.call(document.querySelectorAll('#t1Strip a'), function (a) { a.classList.toggle('on', a.getAttribute('data-tab') === id); });
}
function tab1Index() { var on = document.querySelector('#t1Strip a.on'); return on && on.getAttribute('data-tab') === 'tabPage1' ? 0 : 1; }

/* ---------------------------------------------------------------- close (Esc / Ctrl+E) */
function closeForm() {
    if (S.popup && window.parent && window.parent !== window && typeof window.parent.PavhClose === 'function') {
        window.parent.PavhClose();
        return;
    }
    location.href = S.flagAproved ? '/dashboard/un-approved-invoices-and-vouchers' : '/dashboard/approval-dashboard';
}

/* ---------------------------------------------------------------- PendingApprovalVouchersHistory_KeyDown :1237-1300 */
function onFormKey(e) {
    if (el('attModal').style.display === 'flex') {
        if (e.key === 'Escape') { el('attModal').style.display = 'none'; e.preventDefault(); }
        return;
    }
    var k = (e.key || '').toLowerCase();
    var t = e.target;
    if (e.key === 'Enter' && t && t.tagName === 'INPUT' && t.type !== 'checkbox' && !t.getAttribute('data-flt')) {
        /* SendKeys.Send("{TAB}") */
        var f = Array.prototype.filter.call(document.querySelectorAll('#panel3 input, #panel3 button'), function (x) { return !x.disabled && x.offsetParent !== null; });
        var i = f.indexOf(t); if (i >= 0 && i < f.length - 1) { f[i + 1].focus(); e.preventDefault(); }
        return;
    }
    if (e.ctrlKey && e.key === 'ArrowDown' && tab1Index() === 0) {
        e.preventDefault();
        var inMaster = t && t.closest && t.closest('#grd');
        (inMaster ? el('grdDetail') : el('grd')).focus();
        return;
    }
    if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); tab1('tabPage1'); el('fromdate').focus(); return; }
    if (e.ctrlKey && k === 't') { e.preventDefault(); tab2(T2[(tab2Index() + 1) % 3]); return; }
    if (e.ctrlKey && k === 'a') { e.preventDefault(); btnApproved_Click(); return; }
    if (e.ctrlKey && k === 'p') { e.preventDefault(); if (!S.flagAproved) btnPendingForView_Click(); return; }
    if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); closeForm(); }
}

/* ---------------------------------------------------------------- PendingApprovalVouchersHistory_Load :177-226 */
function boot() {
    el('btnShow').addEventListener('click', btnShow_Click);
    el('btnApproved').addEventListener('click', btnApproved_Click);
    el('btnPendingForView').addEventListener('click', btnPendingForView_Click);
    el('attClose').addEventListener('click', function () { el('attModal').style.display = 'none'; });
    var bc = el('btnClose'); if (bc) bc.addEventListener('click', closeForm);
    Array.prototype.forEach.call(document.querySelectorAll('#t2Strip a'), function (a) { a.addEventListener('click', function () { tab2(a.getAttribute('data-tab')); }); });
    Array.prototype.forEach.call(document.querySelectorAll('#t1Strip a'), function (a) { a.addEventListener('click', function () { tab1(a.getAttribute('data-tab')); }); });
    document.addEventListener('keydown', onFormKey);

    var q = 'id=' + S.id + '&unApprove=' + (S.flagAproved ? 1 : 0);
    if (S.reqFrom) q += '&fromDate=' + encodeURIComponent(S.reqFrom);
    if (S.reqTo) q += '&toDate=' + encodeURIComponent(S.reqTo);
    getJson(API + '/setup?' + q).then(function (d) {
        S.documentTypeId = toI(d.documentTypeId);
        S.allowEdit = !!d.allowEditOnVoucherNoinReports;
        el('fromdate').value = d.fromDate || '';
        el('todate').value = d.toDate || '';
        el('fromdate').focus();
        setBusy(true);
        if (S.flagAproved) {
            el('btnPendingForView').style.display = 'none';
            el('btnApproved').textContent = 'UnApprove';
            el('label23').textContent = 'Vouchers For UnApproval';
            /* tabControl1.TabPages.Remove(tabPendingVouchersForView) */
            var tl = el('tabPendingLink'); if (tl) tl.parentNode.removeChild(tl);
            var tp = el('tabPendingVouchersForView'); if (tp) tp.parentNode.removeChild(tp);
            PendingVoucherForUnApprovalDashboard().then(function () { setBusy(false); });
        } else {
            el('btnPendingForView').style.display = '';
            el('btnApproved').textContent = 'Approve';
            el('label23').textContent = 'Vouchers For Approval';
            PendingForViewVouchers().then(HistoryFill).then(function () { setBusy(false); });
        }
    }).catch(function (e) { alert(e.message); });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
