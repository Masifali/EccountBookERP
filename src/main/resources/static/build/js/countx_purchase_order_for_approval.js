/* ============================================================================================
 * Approvals - Purchase Order
 * Ported from Architecture.WinApp.ApprovalDashboard\PurchaseOrderForApproval.cs (variant "po", dashboard TypeID 20, 2,508 lines)
 * and Architecture.WinApp.FeedMill.Approval\PurchaseOrderForApproval.cs (variant "fm", dashboard TypeID 1100, 2,394 lines).
 *
 * URL: /dashboard/purchase-order-for-approval?variant=po|fm[&branchId=][&fromDate=][&toDate=][&popup=1]
 *   branchId / fromDate / toDate  the properties ApprovalDashboard sets on the form for TypeID 20 (:284-295):
 *                                 BranchesId, RequestedFromDate (only when From is ticked), RequestedToDate.
 *                                 TypeID 1100 (:1100-1105) passes nothing.
 *
 *   PurchaseOrderHistory_Load   settings, rights, combos, status Open, approved "Not Approved", dates, both grids
 *   AllDropDownBind / SupplierNameFill + ItemDetailFill     CmbSupplier, CmbItem
 *   GetDataForApproval          grd (+ GridDetail on selection)
 *   GetPendingPurchaseOrder / GetPendingForReviewPurchaseOrder    grdPendingViewVoucher (+ GridDetailForPendingView)
 *   GridSettings / GridSettingsForPending / GridDetailSetting*    column sets, widths, formats, totals
 *   grd_LinkClicked, grd_ColumnButtonClick_1, grd_KeyDown (Ctrl+Space), the pending grid's three, GridDetail_*
 *   ApproveOrder, btnApprovedOrder_Click, BtnCancel_Click, btnPendingForView_Click
 *   PurchaseOrderHistory_KeyDown (form keys)
 * ==========================================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/purchase-order-approval';
var ATT_API = '/api/dashboard/pending-approval-vouchers/attachments';   /* CommonServices.GetNoofAttachmentsByRefDocumentTypeID */
var PRINT_PO = '/reports/print/203-purchase-order-rice-slip';           /* CommonServices.PurchaseOrderSlipReport203 */
var PRINT_FM = '/reports/print/1100-purchase-order-slip';               /* CommonServices.PurchaseOrderSlip1100 */
var PRINT_LAB = '/reports/print/657-inv-lab-sample-analysis-slip-a';    /* 657-RptInvLabSampleAnalysisSlipA.rpt */

var qs = new URLSearchParams(location.search);
var S = {
    variant: qs.get('variant') === 'fm' ? 'fm' : 'po',
    branchId: toI(qs.get('branchId')),
    reqFrom: qs.get('fromDate') || '',
    reqTo: qs.get('toDate') || '',
    popup: qs.get('popup') === '1',
    rights: { complete: false, cancel: false },
    allowEdit: false,
    branchImplemented: false,
    amountDec: 0,
    rateDec: 2,
    hist: { rows: [], lines: [] },
    pend: { rows: [], lines: [] },
    gridAp: '', gridSt: '',          /* CmbApproved / CmbStatus text when grd was last drawn */
    busy: false
};
var FM = S.variant === 'fm';

/* ---------------------------------------------------------------- helpers */
function el(id) { return document.getElementById(id); }
function txt(v) { return v === null || v === undefined ? '' : String(v); }
function esc(v) { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function toI(v) { var n = parseInt(txt(v).trim(), 10); return isNaN(n) ? 0 : n; }
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
function fmtDate(v) {                     /* dd-MMM-yyyy */
    var d = parseDt(v); if (!d) return txt(v);
    return pad(d.d) + '-' + MON[d.mo - 1] + '-' + d.y;
}
function fmtShortDate(v) {                /* DateTime.ToShortDateString() */
    var d = parseDt(v); if (!d) return txt(v);
    return pad(d.d) + '/' + pad(d.mo) + '/' + d.y;
}
function fmtDateTime(v) {                 /* dd-MMM-yyyy hh:mm tt */
    var d = parseDt(v); if (!d) return txt(v);
    var h12 = d.h % 12 === 0 ? 12 : d.h % 12;
    return pad(d.d) + '-' + MON[d.mo - 1] + '-' + d.y + ' ' + pad(h12) + ':' + pad(d.mi) + ' ' + (d.h < 12 ? 'AM' : 'PM');
}
function group(intPart) { return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
function isNum(v) { return v !== null && v !== undefined && v !== '' && isFinite(Number(v)); }
/* "#,##0.##" - up to d decimals, zero shows 0 */
function fmtMax(d) {
    return function (v) {
        if (v === null || v === undefined || v === '') return '';
        if (!isNum(v)) return txt(v);
        var n = Number(v), neg = n < 0;
        var s = Math.abs(n).toFixed(d);
        if (d > 0) s = s.replace(/\.?0+$/, '');
        var p = s.split('.');
        return (neg && Number(s) !== 0 ? '-' : '') + group(p[0]) + (p[1] ? '.' + p[1] : '');
    };
}
/* clsGlobalVariables.stringFormatsingle / DecimalRateFormate - fixed decimals from the company configuration */
function fmtFix(getDec) {
    return function (v) {
        if (v === null || v === undefined || v === '') return '';
        if (!isNum(v)) return txt(v);                /* NaN / Infinity text of the desktop's double divide */
        var d = getDec(), n = Number(v), neg = n < 0;
        var s = Math.abs(n).toFixed(d), p = s.split('.');
        return (neg && Number(s) !== 0 ? '-' : '') + group(p[0]) + (p[1] ? '.' + p[1] : '');
    };
}
function fmtRaw(v) { return v === null || v === undefined ? '' : txt(v); }
var fmtAmount = fmtFix(function () { return S.amountDec; });
var fmtRate = fmtFix(function () { return S.rateDec; });

function csrfHeaders(h) {
    var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
    if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
    return h;
}
function getJson(url) {
    return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(readJson);
}
function postJson(url, body) {
    return fetch(url, { method: 'POST', credentials: 'same-origin',
        headers: csrfHeaders({ 'Accept': 'application/json', 'Content-Type': 'application/json' }),
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
    ['btnshow', 'btnNew', 'Btnrefresh', 'BtnCancel', 'btnApprovedOrder', 'btnPendingForView'].forEach(function (id) {
        var b = el(id); if (b) b.disabled = !!on;
    });
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
    if (this.opts.onClear) this.opts.onClear();
};
Grid.prototype.frozenLefts = function () {
    var n = this.opts.frozen || 0, left = 0, out = {};
    var vis = this.visibleCols();
    for (var i = 0; i < vis.length && i < n; i++) { out[vis[i].key] = left; left += vis[i].width || 100; }
    return out;
};
Grid.prototype.renderHead = function () {
    var vis = this.visibleCols(), fz = this.frozenLefts(), self = this;
    var hh = this.opts.hdr || 30;
    var total = 0;
    var cg = '<colgroup>' + vis.map(function (c) { var w = c.width || 100; total += w; return '<col style="width:' + w + 'px">'; }).join('') + '</colgroup>';
    var h = '<thead><tr>' + vis.map(function (c) {
        var isF = fz[c.key] !== undefined;
        var st = 'height:' + hh + 'px;' + (isF ? 'left:' + fz[c.key] + 'px;' : '');
        var attrs = ' data-col="' + esc(c.key) + '" style="' + st + '"';
        if (c.selector) return '<th class="c' + (isF ? ' fz' : '') + '"' + attrs + '><input type="checkbox" data-selall="1"></th>';
        return '<th' + (isF ? ' class="fz"' : '') + attrs + '>' + esc(c.caption) + '</th>';
    }).join('') + '</tr>';
    h += '<tr class="flt">' + vis.map(function (c) {
        var f = ' style="top:' + (hh + 1) + 'px' + (fz[c.key] !== undefined ? ';left:' + fz[c.key] + 'px' : '') + '"' + (fz[c.key] !== undefined ? ' class="fz"' : '');
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
    /* TotalRow: AggregateFunction 2 = Sum, 3 = Average */
    var any = vis.some(function (c) { return c.sum; });
    tf.innerHTML = any ? '<tr>' + vis.map(function (c) {
        var cls = c.align === 'r' ? 'r' : '', st = '';
        if (fz[c.key] !== undefined) { cls += ' fz'; st = ' style="left:' + fz[c.key] + 'px"'; }
        var t = '';
        if (c.sum) {
            var s = 0, n = 0;
            self.view.forEach(function (r) { var x = Number(col(r, c.key)); if (isFinite(x)) { s += x; n++; } });
            if (c.sum === 'avg') s = n ? s / n : 0;
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
function dnum(key, w, f, sum) {            /* a Double column: right aligned, optional total */
    return { key: key, caption: key, width: w, align: 'r', fmt: f, sum: sum || false, totalFmt: f };
}
function allowEditLink() { return S.allowEdit; }
function always() { return true; }

/* The button columns of grd, in the order the desktop's Position statements leave them (GridSettings). */
function gridLeadButtons() {
    var ap = S.gridAp, st = S.gridSt, c = [];
    if (FM) {
        if (ap === 'Not Approved') {
            c.push({ key: 'Select', caption: '', width: 40, selector: true });
            c.push({ key: 'IsApproved', caption: 'Approve', width: 60, button: 'Approve' });
            c.push({ key: 'Slip', caption: 'Slip-1100', width: 70, button: 'Slip-1100' });
            c.push({ key: 'Detail', caption: 'Detail', width: 60, button: 'Detail' });
        } else {
            c.push({ key: 'Slip', caption: 'Slip-1100', width: 70, button: 'Slip-1100' });
            c.push({ key: 'Detail', caption: 'Detail', width: 60, button: 'Detail' });
        }
        return c;
    }
    if (ap === 'Approved' && st === 'Open') {
        if (S.rights.complete) c.push({ key: 'Complete', caption: 'Complete', width: 80, button: 'Complete' });
        if (S.rights.cancel) c.push({ key: 'Cancel', caption: 'Cancel', width: 60, button: 'Cancel' });
        c.push({ key: 'Slip', caption: 'Slip203', width: 60, button: 'Slip203' });
        c.push({ key: 'Detail', caption: 'Detail', width: 60, button: 'Detail' });
    } else if (ap === 'Not Approved' && st === 'Open') {
        c.push({ key: 'Select', caption: '', width: 30, selector: true });
        c.push({ key: 'Slip', caption: 'Slip203', width: 60, button: 'Slip203' });
        c.push({ key: 'Detail', caption: 'Detail', width: 60, button: 'Detail' });
        c.push({ key: 'IsApproved', caption: 'Approve', width: 60, button: 'Approve' });
        c.push({ key: 'ApprovedAndCancel', caption: 'Approved & Cancel', width: 120, button: 'Approved & Cancel' });
    } else {
        c.push({ key: 'Slip', caption: 'Slip203', width: 60, button: 'Slip203' });
        c.push({ key: 'Detail', caption: 'Detail', width: 60, button: 'Detail' });
    }
    return c;
}
/* kind: 'grd' | 'pend' */
function masterCols(kind) {
    var pend = kind === 'pend';
    var c = pend ? [] : gridLeadButtons();
    if (!FM) {
        /* GridWrappingAndColumnSettings(grid, 3, 2): ints centred, doubles right; widths then set by the form.
           The pending grid has no button columns: its GridEX_Helper.AddButton calls name grd, which already has them. */
        c.push({ key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true });
        c.push({ key: 'DocNo', caption: 'DocNo', width: 60, align: 'c', link: allowEditLink });
        c.push({ key: 'DocDate', caption: 'DocDate', width: 93, fmt: fmtDate });
        if (pend) c.push({ key: 'SupplierCustomerId', hidden: true });
        else {
            c.push({ key: 'BranchSrNo', caption: 'BranchSrNo', width: 60, align: 'c', hidden: S.branchImplemented });
            c.push({ key: 'BranchName', caption: 'BranchName', width: 120, hidden: S.branchImplemented });
            c.push({ key: 'OrderSupCustId', hidden: true });
        }
        c.push(
            { key: 'SupplierName', caption: 'SupplierName', width: 170, link: always },
            { key: 'CommissionAgent', caption: 'CommissionAgent', width: 130 },
            { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 90 },
            { key: 'OrderExpiryDate', caption: 'OrderExpiryDate', width: 93, fmt: fmtDate },
            dnum('OrderQty', 70, fmtMax(2), true),
            dnum('OrderWeight', 80, fmtMax(2), true),
            dnum('AvgRate40Kg', 70, fmtRate, false),
            dnum('Amount', 90, fmtAmount, true),
            { key: 'EntryDate', caption: 'EntryDate', width: 130, fmt: fmtDateTime },
            { key: 'EntryUser', caption: 'EntryUser', width: 100 },
            { key: 'ModifyDate', caption: 'ModifyDate', width: 130, fmt: fmtDateTime },
            { key: 'ModifyUser', caption: 'ModifyUser', width: 100 },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', width: 50, link: always, align: pend ? 'c' : '' }
        );
        return c;
    }
    /* FeedMill: no helper, explicit widths 60/80/140/140/100/120/70/90/100/95/145/100/145/100/120 */
    c.push({ key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true });
    c.push({ key: pend ? 'SupplierCustomerId' : 'OrderSupCustId', hidden: true });
    c.push(
        { key: 'DocNo', caption: 'DocNo', width: 60, align: 'r', link: allowEditLink },
        { key: 'DocDate', caption: 'DocDate', width: 80, fmt: fmtDate },
        { key: 'SupplierName', caption: 'SupplierName', width: 140 },
        { key: 'CommissionAgent', caption: 'CommissionAgent', width: 140 },
        { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 100 },
        { key: 'OrderExpiryDate', caption: 'OrderExpiryDate', width: 120, fmt: fmtShortDate },
        dnum('OrderQty', 70, fmtMax(2), true),
        dnum('OrderWeight', 90, fmtMax(pend ? 4 : 3), true),
        dnum('AvgRate40Kg', 100, pend ? fmtRaw : fmtMax(3), 'avg'),
        dnum('Amount', 95, fmtMax(pend ? 4 : 3), true),
        { key: 'EntryDate', caption: 'EntryDate', width: 145, fmt: fmtDateTime },
        { key: 'EntryUser', caption: 'EntryUser', width: 100 },
        { key: 'ModifyDate', caption: 'ModifyDate', width: 145, fmt: fmtDateTime },
        { key: 'ModifyUser', caption: 'ModifyUser', width: 100 },
        { key: 'NoOfAttachments', caption: 'NoOfAttachments', width: 120, link: always }
    );
    if (pend) {
        /* GridSettingsForPending: Slip pos 0, Approve pos 1 */
        c.unshift({ key: 'Approve', caption: 'Approve', width: 60, button: 'Approve' });
        c.unshift({ key: 'Slip', caption: 'Slip-1100', width: 70, button: 'Slip-1100' });
    }
    return c;
}

/* The detail grids. kind: 'hist' | 'pend' */
function detailCols(kind) {
    var hist = kind === 'hist';
    if (!FM) {
        if (hist) {
            /* GridWrappingAndColumnSettings(GridDetail, 2, 2): default widths by type and name */
            return [
                { key: 'PurchaseGLAC', hidden: true }, { key: 'ItemId', hidden: true },
                { key: 'ItemName', caption: 'ItemName', width: 150, link: always },
                { key: 'CropYear', caption: 'CropYear', width: 100 },
                { key: 'PackUom', caption: 'PackUom', width: 60 },
                dnum('ItemQty', 90, fmtMax(2), true), dnum('NetWeight', 90, fmtMax(2), true),
                dnum('ItemRate', 90, fmtRate, false),
                { key: 'RateUom', caption: 'RateUom', width: 60 },
                dnum('ItemAmount', 90, fmtAmount, true),
                { key: 'PPBagPrice', caption: 'PPBagPrice', width: 100 }, { key: 'JuteBagPrice', caption: 'JuteBagPrice', width: 100 },
                { key: 'PPBagWeight', caption: 'PPBagWeight', width: 100 }, { key: 'JuteBagWeight', caption: 'JuteBagWeight', width: 100 },
                { key: 'JobLot', caption: 'JobLot', width: 100 },
                { key: 'LabSampleId', hidden: true },
                { key: 'LabSampleNo', caption: 'LabSampleNo', width: 100, link: always },
                { key: 'CityArea', caption: 'CityArea', width: 75 }
            ];
        }
        /* GridDetailSettingForPendingRecord */
        return [
            { key: 'PurchaseGLAC', hidden: true }, { key: 'ItemId', hidden: true },
            { key: 'ItemName', caption: 'ItemName', width: 150, link: always },
            { key: 'CropYear', caption: 'CropYear', width: 73 },
            { key: 'PackUom', caption: 'PackUom', width: 60 },
            dnum('ItemQty', 70, fmtMax(2), true), dnum('NetWeight', 80, fmtMax(2), true),
            dnum('ItemRate', 70, fmtMax(2), false),
            { key: 'RateUom', caption: 'RateUom', width: 60 },
            dnum('ItemAmount', 90, fmtMax(2), true),
            { key: 'PPBagPrice', caption: 'PPBagPrice', width: 70 }, { key: 'JuteBagPrice', caption: 'JuteBagPrice', width: 70 },
            { key: 'PPBagWeight', caption: 'PPBagWeight', width: 70 }, { key: 'JuteBagWeight', caption: 'JuteBagWeight', width: 70 },
            { key: 'JobLot', caption: 'JobLot', width: 80 },
            { key: 'CityArea', caption: 'CityArea', width: 75 }
        ];
    }
    if (hist) {
        return [
            { key: 'PurchaseGLAC', hidden: true }, { key: 'ItemId', hidden: true },
            { key: 'ItemName', caption: 'ItemName', width: 150 },
            { key: 'CropYear', caption: 'CropYear', width: 100 },
            { key: 'PackUom', caption: 'PackUom', width: 100 },
            dnum('ItemQty', 100, fmtMax(2), true), dnum('NetWeight', 100, fmtMax(2), true), dnum('ItemRate', 100, fmtMax(2), false),
            { key: 'RateUom', caption: 'RateUom', width: 100 },
            dnum('ItemAmount', 100, fmtMax(2), true),
            dnum('PPBagPrice', 100, fmtRaw), dnum('JuteBagPrice', 100, fmtRaw), dnum('PPBagWeight', 100, fmtRaw), dnum('JuteBagWeight', 100, fmtRaw),
            { key: 'JobLot', caption: 'JobLot', width: 100 },
            { key: 'LabSampleId', hidden: true },
            { key: 'LabSampleNo', caption: 'LabSampleNo', width: 100, link: always },
            { key: 'CityArea', caption: 'CityArea', width: 100 }
        ];
    }
    return [
        { key: 'PurchaseGLAC', hidden: true }, { key: 'ItemId', hidden: true },
        { key: 'ItemName', caption: 'ItemName', width: 150 },
        { key: 'CropYear', caption: 'CropYear', width: 100 },
        { key: 'PackUom', caption: 'PackUom', width: 100 },
        dnum('ItemQty', 100, fmtMax(2), true), dnum('NetWeight', 100, fmtMax(3), true), dnum('ItemRate', 100, fmtMax(3), false),
        { key: 'RateUom', caption: 'RateUom', width: 100 },
        dnum('ItemAmount', 100, fmtMax(3), true),
        dnum('PPBagPrice', 100, fmtRaw), dnum('JuteBagPrice', 100, fmtRaw), dnum('PPBagWeight', 100, fmtRaw), dnum('JuteBagWeight', 100, fmtRaw),
        { key: 'JobLot', caption: 'JobLot', width: 100 },
        { key: 'CityArea', caption: 'CityArea', width: 100 }
    ];
}

/* ---------------------------------------------------------------- grids */
var grd = new Grid('grd', {
    frozen: 4, hdr: FM ? 22 : 44,
    onSelect: function (r) { fillDetail(r, 'hist'); },                        /* grd_SelectionChanged */
    onClear: function () { GridDetail.clear(); },
    onButton: function (key, r) { masterButton(key, r); },                    /* grd_ColumnButtonClick_1 */
    onLink: function (key, r) { masterLink(key, r); },                        /* grd_LinkClicked */
    onCtrlSpace: function (key, r) { masterCtrlSpace(key, r); }              /* grd_KeyDown */
});
var GridDetail = new Grid('GridDetail', {
    hdr: FM ? 22 : 30,
    onLink: function (key, r) { detailLink(key, r); },
    onCtrlSpace: function (key, r) { detailCtrlSpace(key, r); }
});
var grdPend = new Grid('grdPendingViewVoucher', {
    frozen: FM ? 2 : 4, hdr: FM ? 22 : 44,
    onSelect: function (r) { fillDetail(r, 'pend'); },
    onClear: function () { GridDetailPend.clear(); },
    onButton: function (key, r) { masterButton(key, r); },
    onLink: function (key, r) { masterLink(key, r); },
    onCtrlSpace: function (key, r) { masterCtrlSpace(key, r); }
});
var GridDetailPend = new Grid('GridDetailForPendingView', {
    hdr: FM ? 22 : 30,
    onLink: function (key, r) { detailLink(key, r); },
    onCtrlSpace: function (key, r) { detailCtrlSpace(key, r); }
});

/* grd_SelectionChanged / grdPendingViewVoucher_SelectionChanged - the detail rows of the selected order. */
function fillDetail(r, kind) {
    var id = toI(col(r, 'Id'));
    var src = (kind === 'hist' ? S.hist : S.pend).lines || [];
    var rows = src.filter(function (l) { return toI(col(l, 'Id')) === id; });
    var g = kind === 'hist' ? GridDetail : GridDetailPend;
    if (!rows.length) { g.clear(); return; }
    g.setData(detailCols(kind), rows);
}

/* ---------------------------------------------------------------- combos */
function selText(id) {
    var s = el(id);
    if (!s || s.selectedIndex < 0) return '';
    return txt(s.options[s.selectedIndex].text).trim();
}
function setSel(id, v) { var s = el(id); if (s) s.value = v; }
function fillSelect(id, items, blank) {
    var s = el(id); if (!s) return;
    var h = blank ? '<option value=""></option>' : '';
    (items || []).forEach(function (x) { h += '<option value="' + esc(x.id) + '">' + esc(x.name) + '</option>'; });
    s.innerHTML = h;
    s.value = '';
}
/* ComboStatusFill / ComboApprovedfill, then Rows[0] / Rows[1].Activate() in the Load */
function fillStatusCombos() {
    fillSelect('CmbStatus', [{ id: 1, name: 'Open' }, { id: 2, name: 'Complete' }, { id: 3, name: 'Cancel' }], false);
    fillSelect('CmbApproved', [{ id: 1, name: 'Approved' }, { id: 2, name: 'Not Approved' }, { id: 3, name: 'All' }], false);
    setSel('CmbStatus', '1');
    setSel('CmbApproved', '2');
}
function fillPartyCombos(d) {
    fillSelect('CmbSupplier', d.suppliers, true);
    fillSelect('CmbItem', d.items, true);
}

/* ---------------------------------------------------------------- reads */
function filterQuery() {
    return 'variant=' + S.variant + '&branchId=' + S.branchId
        + '&fromDate=' + encodeURIComponent(el('fromdate').value || '')
        + '&toDate=' + encodeURIComponent(el('todate').value || '')
        + '&fromNo=' + encodeURIComponent(el('txtSrFrom').value.trim())
        + '&toNo=' + encodeURIComponent(el('txtSrTo').value.trim())
        + '&supplierId=' + toI(el('CmbSupplier').value)
        + '&itemId=' + toI(el('CmbItem').value)
        + '&status=' + encodeURIComponent(selText('CmbStatus'))
        + '&approved=' + encodeURIComponent(selText('CmbApproved'));
}
/* GetDataForApproval */
function GetDataForApproval() {
    var ap = selText('CmbApproved'), st = selText('CmbStatus');
    return getJson(API + '/history?' + filterQuery()).then(function (d) {
        S.hist = { rows: d.rows || [], lines: d.lines || [] };
        S.gridAp = ap;
        S.gridSt = st;
        if (S.hist.rows.length) grd.setData(masterCols('grd'), S.hist.rows);
        else { grd.clear(); GridDetail.clear(); }
    }).catch(function (e) { alert(e.message); });
}
/* GetPendingPurchaseOrder / GetPendingForReviewPurchaseOrder */
function GetPendingPurchaseOrder() {
    return getJson(API + '/pending?' + filterQuery()).then(function (d) {
        S.pend = { rows: d.rows || [], lines: d.lines || [] };
        if (S.pend.rows.length) grdPend.setData(masterCols('pend'), S.pend.rows);
        else { grdPend.clear(); GridDetailPend.clear(); }
    }).catch(function (e) { alert(e.message); });
}
function bothGrids() { return GetDataForApproval().then(GetPendingPurchaseOrder); }

/* ---------------------------------------------------------------- toolbar and filter strip */
/* btnshow_Click */
function btnshow_Click() {
    if (S.busy) return;
    setBusy(true);
    bothGrids().then(function () { setBusy(false); }, function () { setBusy(false); });
}
/* reset() - btnNew_Click */
function reset() {
    setSel('CmbSupplier', ''); setSel('CmbItem', '');
    el('txtSrFrom').value = ''; el('txtSrTo').value = '';
    el('fromdate').focus();
}
/* Btnrefresh_Click - AllDropDownBind */
function Btnrefresh_Click() {
    if (S.busy) return;
    setBusy(true);
    getJson(API + '/combos?variant=' + S.variant + '&branchId=' + S.branchId).then(function (d) {
        setBusy(false);
        fillPartyCombos(d);
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
function checkedIds() {
    return grd.checkedRows().map(function (r) { return toI(col(r, 'Id')); });
}
/* btnApprovedOrder_Click */
function btnApprovedOrder_Click() {
    if (S.busy) return;
    var ids = checkedIds();
    if (!ids.length) { alert('Please select check box first'); return; }
    if (selText('CmbApproved') !== 'Not Approved') { alert('Please Select Not Approved status first'); return; }
    if (!confirm('Are you sure to Approve Order?')) return;
    send('approve', '', ids, function () { return GetDataForApproval(); });
}
/* BtnCancel_Click (po) */
function BtnCancel_Click() {
    if (S.busy) return;
    var ids = checkedIds();
    if (!ids.length) { alert('Please select check box first'); return; }
    if (selText('CmbApproved') === 'Not Approved' && selText('CmbStatus') === 'Open'
        && confirm('Are you sure to Approve & cancel Purchase Order?')) {
        send('cancel', '', ids, function () { return GetDataForApproval(); });
    }
}
/* btnPendingForView_Click */
function btnPendingForView_Click() {
    if (S.busy) return;
    var ids = checkedIds();
    if (!ids.length) { alert('Please select check box first'); return; }
    if (selText('CmbApproved') !== 'Not Approved') { alert('Please Select Not Approved status first'); return; }
    if (!confirm('Are you sure to Move PurchaseOrder in PendingGrid?')) return;
    send('pending', '', ids, bothGrids);
}
/* The save chain: PurchaseOrder.UpdateStatusandIsApprovedbyOrderId / PurchaseOrderandVoucherApprove, then the message, then the refresh. */
function send(source, reqType, ids, after, ap, st) {
    setBusy(true);
    postJson(API + '/update', {
        variant: S.variant, source: source, reqType: reqType, ids: ids,
        approved: ap === undefined ? selText('CmbApproved') : ap,
        status: st === undefined ? selText('CmbStatus') : st
    }).then(function (d) {
        setBusy(false);
        if (d.message) alert(d.message);
        return after ? after() : null;
    }).catch(function (e) { setBusy(false); alert(e.message); });
}
/* ApproveOrder(Id, RequestType) */
function ApproveOrder(id, req) {
    if (S.busy) return;
    var r = FM ? 'Approve' : (req || 'Approve');
    if (!confirm('Are you sure to ' + r + ' Order?')) return;
    send('row', r, [id], function () {
        setBusy(true);
        return bothGrids().then(function () { setBusy(false); }, function () { setBusy(false); });
    }, S.gridAp, S.gridSt);
}

/* ---------------------------------------------------------------- grid buttons and links */
function masterButton(key, r) {
    var id = toI(col(r, 'Id'));
    if (key === 'Slip') GenerateReport(id);
    else if (key === 'Detail') activityDetails();
    else if (key === 'IsApproved' || key === 'Approve') ApproveOrder(id);
    else if (key === 'Complete') ApproveOrder(id, 'Status');
    else if (key === 'Cancel') ApproveOrder(id, 'Cancel');
    else if (key === 'ApprovedAndCancel') ApproveOrder(id, 'ApproveAndCancel');
}
/* new ActivityDetails(UserAccount, Id).Show() */
function activityDetails() {
    alert('Detail opens ActivityDetails (the approval history of the order) on the desktop. That screen is not ported yet, so nothing was opened.');
}
function supplierOf(r) { return toI(col(r, 'OrderSupCustId')) || toI(col(r, 'SupplierCustomerId')); }
function masterLink(key, r) {
    var id = toI(col(r, 'Id')), docType = toI(col(r, 'DocumentTypeId'));
    if (key === 'DocNo') editLinked(docType, id);
    else if (key === 'SupplierName') openCustomerLedger(supplierOf(r));
    else if (key === 'NoOfAttachments') attachments(id, docType);
}
/* grd_KeyDown / grdPendingViewVoucher_KeyDown, Ctrl+Space */
function masterCtrlSpace(key, r) {
    var id = toI(col(r, 'Id')), docType = toI(col(r, 'DocumentTypeId'));
    if (key === 'DocNo') { if (S.allowEdit) editLinked(docType, id); }
    else if (key === 'SupplierName') openCustomerLedger(supplierOf(r));
    else if (key === 'NoOfAttachments') attachments(id, docType);
    else if (key === 'Slip') GenerateReport(id);
    else if (key === 'Detail') activityDetails();
    else if (key === 'IsApproved' || key === 'Approve') ApproveOrder(id);
    else if (key === 'Complete') ApproveOrder(id, 'Status');
    else if (key === 'Cancel') ApproveOrder(id, 'Cancel');
    else if (key === 'ApprovedAndCancel') ApproveOrder(id, 'ApproveAndCancel');
}
function detailLink(key, r) {
    if (key === 'ItemName') openItemLedger(toI(col(r, 'ItemId')));
    else if (key === 'LabSampleNo') labSlip(toI(col(r, 'LabSampleId')));
}
function detailCtrlSpace(key, r) {
    if (key === 'ItemName') openItemLedger(toI(col(r, 'ItemId')));
}

/* CommonServices.EditMethodFromLinked(DocumentTypeId, Id) */
function editLinked(docType, id) {
    if (!window.DocLink) { alert('Document links are not available on this page.'); return; }
    window.DocLink.open(docType, id, { message: function (m) { alert(m); } });
}
/* CommonServices.GoToCustomerLedgerFromLinkedEvent(id, fromdate, todate) */
function openCustomerLedger(supplierCustomerId) {
    getJson(API + '/gl-account?supplierCustomerId=' + supplierCustomerId).then(function (d) {
        var q = new URLSearchParams();
        q.set('accountId', toI(d.accountId));
        if (el('fromdate').value) q.set('fromDate', el('fromdate').value);
        if (el('todate').value) q.set('toDate', el('todate').value);
        window.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
    }).catch(function (e) { alert(e.message); });
}
/* CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, fromdate, todate) */
function openItemLedger(itemId) {
    var q = new URLSearchParams();
    q.set('itemId', itemId);
    if (el('fromdate').value) q.set('fromDate', el('fromdate').value);
    if (el('todate').value) q.set('toDate', el('todate').value);
    window.open('/stocks/item-ledger?' + q.toString(), '_blank');
}
/* CommonServices.GetNoofAttachmentsByRefDocumentTypeID - AttachmentView opens only when there is a row. */
function attachments(refId, documentTypeId) {
    getJson(ATT_API + '?refId=' + refId + '&documentTypeId=' + documentTypeId).then(function (d) {
        var rows = d.rows || [];
        if (!rows.length) return;
        el('attBody').innerHTML = rows.map(function (a) {
            return '<tr><td>' + esc(a.AttachmentName) + '</td><td>' + esc(a.CustomName) + '</td><td>' + esc(fmtDateTime(a.EntryDate)) + '</td></tr>';
        }).join('');
        el('attModal').style.display = 'flex';
    }).catch(function (e) { alert(e.message); });
}
/* GenerateReport(Id) -> PurchaseOrderSlipReport203 / PurchaseOrderSlip1100 */
function GenerateReport(id) {
    if (!(id > 0)) { alert('No Record Found For Display'); return; }
    pdf(FM ? PRINT_FM : PRINT_PO, { id: id });
}
/* GridDetail_LinkClicked LabSampleNo -> 657-RptInvLabSampleAnalysisSlipA.rpt */
function labSlip(labId) {
    pdf(PRINT_LAB, { id: labId });
}
function pdf(url, body) {
    var w = window.open('about:blank', '_blank');
    fetch(url, { method: 'POST', credentials: 'same-origin',
        headers: csrfHeaders({ 'Content-Type': 'application/json', 'Accept': 'application/pdf' }),
        body: JSON.stringify(body) }).then(function (r) {
        var type = r.headers.get('Content-Type') || '';
        if (r.ok && type.indexOf('application/pdf') === 0) {
            return r.blob().then(function (b) { if (w) w.location = URL.createObjectURL(b); });
        }
        return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
    }).catch(function (e) { if (w) w.close(); alert(e.message); });
}

/* ---------------------------------------------------------------- tabs */
var T1 = ['tabPage1', 'tabPage2'];
function tab1(id) {
    T1.forEach(function (p) { el(p).classList.toggle('on', p === id); });
    Array.prototype.forEach.call(document.querySelectorAll('#t1Strip a'), function (a) { a.classList.toggle('on', a.getAttribute('data-tab') === id); });
}
function tab1Index() { return el('tabPage1').classList.contains('on') ? 0 : 1; }

/* ---------------------------------------------------------------- close (Esc / Ctrl+E) */
function closeForm() {
    if (S.popup && window.parent && window.parent !== window && typeof window.parent.PoaClose === 'function') {
        window.parent.PoaClose();
        return;
    }
    location.href = '/dashboard/approval-dashboard';
}

/* ---------------------------------------------------------------- PurchaseOrderHistory_KeyDown */
function onFormKey(e) {
    if (el('attModal').style.display === 'flex') {
        if (e.key === 'Escape') { el('attModal').style.display = 'none'; e.preventDefault(); }
        return;
    }
    if (e.defaultPrevented) return;
    var k = (e.key || '').toLowerCase();
    var t = e.target;
    if (e.key === 'Enter' && t && t.tagName === 'INPUT' && t.type !== 'checkbox' && !t.getAttribute('data-flt')) {
        /* SendKeys.Send("{TAB}") */
        var f = Array.prototype.filter.call(document.querySelectorAll('#panel5 input, #panel5 button'), function (x) { return !x.disabled && x.offsetParent !== null; });
        var i = f.indexOf(t); if (i >= 0 && i < f.length - 1) { f[i + 1].focus(); e.preventDefault(); }
        return;
    }
    if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); tab1('tabPage1'); el('fromdate').focus(); return; }
    if (e.ctrlKey && k === 'p') { e.preventDefault(); return; }                       /* print_Click_1 is empty */
    if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); closeForm(); return; }
    if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
    if (e.ctrlKey && k === 's') { e.preventDefault(); if (!S.busy) { setBusy(true); GetDataForApproval().then(function () { setBusy(false); }); } return; }
    if (e.ctrlKey && k === 'a') { e.preventDefault(); btnApprovedOrder_Click(); return; }
    if (e.ctrlKey && k === 'b') { e.preventDefault(); btnPendingForView_Click(); return; }
    if (e.ctrlKey && k === 't') { e.preventDefault(); tab1(tab1Index() === 0 ? 'tabPage2' : 'tabPage1'); return; }
    if (e.ctrlKey && e.key === 'ArrowDown') {
        e.preventDefault();
        if (tab1Index() === 0) {
            var m1 = t && t.closest && t.closest('#grd');
            (m1 ? el('GridDetail') : el('grd')).focus();
        } else {
            var m2 = t && t.closest && t.closest('#grdPendingViewVoucher');
            (m2 ? el('GridDetailForPendingView') : el('grdPendingViewVoucher')).focus();
        }
    }
}

/* ---------------------------------------------------------------- PurchaseOrderHistory_Load */
function boot() {
    el('btnshow').addEventListener('click', btnshow_Click);
    el('btnNew').addEventListener('click', reset);
    el('Btnrefresh').addEventListener('click', Btnrefresh_Click);
    el('BtnCancel').addEventListener('click', BtnCancel_Click);
    el('btnApprovedOrder').addEventListener('click', btnApprovedOrder_Click);
    el('btnPendingForView').addEventListener('click', btnPendingForView_Click);
    el('attClose').addEventListener('click', function () { el('attModal').style.display = 'none'; });
    var bc = el('btnClose'); if (bc) bc.addEventListener('click', closeForm);
    Array.prototype.forEach.call(document.querySelectorAll('#t1Strip a'), function (a) { a.addEventListener('click', function () { tab1(a.getAttribute('data-tab')); }); });
    document.addEventListener('keydown', onFormKey);
    fillStatusCombos();

    var q = 'variant=' + S.variant + '&branchId=' + S.branchId;
    if (S.reqFrom) q += '&fromDate=' + encodeURIComponent(S.reqFrom);
    if (S.reqTo) q += '&toDate=' + encodeURIComponent(S.reqTo);
    setBusy(true);
    getJson(API + '/setup?' + q).then(function (d) {
        el('label23').textContent = d.title || (FM ? 'Purchase Order Approval' : 'Purchase Order');
        S.branchImplemented = !!d.branchImplemented;
        if (d.branchesId) S.branchId = toI(d.branchesId);
        S.rights.complete = !!(d.rights && d.rights.canChangeOrderStatusToComplete);
        S.rights.cancel = !!(d.rights && d.rights.canChangeOrderStatusToCancel);
        S.allowEdit = !!d.allowEditOnVoucherNoinReports;
        S.amountDec = toI(d.amountDecimals);
        S.rateDec = toI(d.rateDecimals);
        fillPartyCombos(d);
        if (d.comboError) alert(d.comboError);
        el('fromdate').value = d.fromDate || '';
        el('todate').value = d.toDate || '';
        el('fromdate').focus();
        return bothGrids();
    }).then(function () { setBusy(false); }, function (e) { setBusy(false); alert(e.message); });
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
