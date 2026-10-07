/* ============================================================================================
 * Screen 3 "Account Budget" - desktop Architecture.WinApp.Account_Definition.AccountBudget
 * (source map: AccountBudget.cs - Load :204, MonthFill :261, GenerateCode :295, ProjectFill :318, COABind :352,
 *  FormValidation :421, FormReset :485, btnRefresh :520, btnGenerate :539, grdDetailsetting :574,
 *  grdDetail_CellUpdated :603, Insert :643, ReadById :759, tab change :810, HistoryGridBind :872,
 *  HistoryGridSetting :933, ColumnButtonClick :968, LinkClicked :992, DoubleClick :1002, SelectionChanged :1019,
 *  grdHistoryDetailbind :1036, txtAmount_KeyPress :1091, attachment :1103, print :1115, KeyDown :1127,
 *  ShortCut keys :1234, DataGridHistory_KeyDown :1264).
 * API: /accounts/account-budget/... (AccountBudgetController).
 * ============================================================================================ */
(function ($) {
    'use strict';
    var API = '/accounts/account-budget';
    var DOC_TYPE = 11;
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var BAD_DATE = 'String was not recognized as a valid DateTime.';

    var S = {
        rights: { save: false, update: false, print: false, viewAll: false },
        dec: 0,                         /* clsGlobalVariables.DefaultNoofDecimalPointsForAmount (1..4, else 0) */
        recId: 0, updateMode: false, chkAmountFiled: false,
        rows: [],                       /* dtdetail: {id, month ("MMM-yyyy"), percent, amount} */
        months: [], projects: [], accounts: [],
        busy: false, tab: 0,
        att: { existing: [], added: [], removed: [] },       /* Attachment form (AT.lst / RemovedAttachmentListInUpdateCase) */
        histRows: [], histDetail: []
    };

    /* ------------------------------------------------------------------------------ helpers */
    function esc(v) { return String(v == null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function failText(x) {
        try { var j = x.responseJSON || JSON.parse(x.responseText); if (j && j.message) return j.message; } catch (e) { /* ignore */ }
        return x && x.status ? 'HTTP ' + x.status : 'Request failed';
    }
    function getJson(url) { return $.ajax({ url: url, type: 'GET', dataType: 'json', cache: false }); }
    function postJson(url, body) {
        return $.ajax({ url: url, type: 'POST', contentType: 'application/json', dataType: 'json', data: JSON.stringify(body) });
    }
    function msg(t) { window.alert(t); }                                  /* MessageBox.Show */
    /* Conversion.ToDouble: empty / unreadable -> 0, thousands separators allowed, +-Infinity -> 0 */
    function toD(v) {
        if (typeof v === 'number') return isFinite(v) ? v : (isNaN(v) ? v : 0);
        var s = String(v == null ? '' : v).trim().replace(/,/g, '');
        if (s === '') return 0;
        var n = Number(s);
        return isNaN(n) ? 0 : (isFinite(n) ? n : 0);
    }
    /* Conversion.ToInt on a string = Convert.ToInt32(string): digits only, anything else -> 0 */
    function toI(v) { var s = String(v == null ? '' : v).trim(); return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    /* Math.Round(double) - banker's rounding */
    function rint(x) { var f = Math.floor(x), d = x - f; if (d < 0.5) return f; if (d > 0.5) return f + 1; return f % 2 === 0 ? f : f + 1; }
    function group(n, minF, maxF) {
        return n.toLocaleString('en-US', { minimumFractionDigits: minF, maximumFractionDigits: maxF });
    }
    /* clsGlobalVariables.stringFormatsingle = "#,##0." + "0" x N */
    function fmtAmt(v) {
        var n = toD(v);
        return group(n, S.dec, S.dec);
    }
    /* "##,#.###": zero shows empty */
    function fmtPct(v) {
        var n = toD(v);
        if (n === 0) return '';
        var s = group(n, 0, 3);
        return s === '0' || s === '-0' ? '' : s;
    }
    function pad2(n) { return (n < 10 ? '0' : '') + n; }
    function todayIso() { var d = new Date(); return d.getFullYear() + '-' + pad2(d.getMonth() + 1) + '-' + pad2(d.getDate()); }
    function monthText(d) { return MON[d.getMonth()] + ' ' + d.getFullYear(); }               /* ToString("MMM yyyy") */
    /* DateTime.ParseExact(text, "MMM yyyy" | "MMM-yyyy", Invariant) -> Date at the 1st; null when it does not parse */
    function parseMonth(text, sep) {
        var m = new RegExp('^([A-Za-z]{3})' + sep + '(\\d{4})$').exec(String(text || '').trim());
        if (!m) return null;
        var i = -1;
        for (var k = 0; k < 12; k++) if (MON[k].toLowerCase() === m[1].toLowerCase()) i = k;
        return i < 0 ? null : new Date(Date.UTC(parseInt(m[2], 10), i, 1));
    }
    function daysBetween(a, b) { return Math.trunc((b.getTime() - a.getTime()) / 86400000); }
    function addMonth(d) { return new Date(Date.UTC(d.getUTCFullYear(), d.getUTCMonth() + 1, 1)); }
    function dashText(d) { return MON[d.getUTCMonth()] + '-' + d.getUTCFullYear(); }          /* ToString("MMM-yyyy") */

    /* ---- UltraCombo helpers (the native <select> is authoritative; value '' = no active row) ---- */
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function cval(id) { var v = $('#' + id).val(); return v == null ? '' : String(v); }
    function ctext(id) { return cval(id) === '' ? '' : $('#' + id + ' option:selected').text(); }       /* combo.Text */
    function cset(id, v) { $('#' + id).val(v == null ? '' : String(v)).trigger('change'); refreshCombos(); }
    function chas(id, v) { return $('#' + id + ' option').filter(function () { return this.value === String(v) && this.value !== ''; }).length > 0; }
    function ccount(id) { return $('#' + id + ' option').filter(function () { return this.value !== '' && this.value !== '0'; }).length; }
    function cfocus(id) {
        var $i = $('#' + id).closest('.ab-cb').find('.dtcombo-input');
        if ($i.length) $i.focus(); else $('#' + id).focus();
    }
    function bind(id, items, vKey, tKey, zero) {
        var h = '<option value=""></option>';
        if (zero) h += '<option value="0">...Select Any Value...</option>';
        items.forEach(function (r) { h += '<option value="' + esc(r[vKey]) + '">' + esc(r[tKey]) + '</option>'; });
        $('#' + id).html(h);
    }

    /* ------------------------------------------------------------------------------ rights / buttons */
    function applyRights() {
        $('#BtnSave').prop('disabled', !S.rights.save || S.busy);
        $('#btnUpdate').prop('disabled', !S.rights.update || S.busy);
        $('#BtnPrint').prop('disabled', !S.rights.print || S.busy);
    }
    function showSave(isSave) {
        $('#BtnSave').toggleClass('ab-hidden', !isSave);
        $('#btnUpdate').toggleClass('ab-hidden', isSave);
        applyRights();
    }
    function busy(on) {
        S.busy = on;
        $('#BtnSave, #btnUpdate, #BtnPrint, #btnGenerate, #BtnShowInputHistory').toggleClass('btn-busy', on);
        $('#btnGenerate, #BtnShowInputHistory').prop('disabled', on);
        applyRights();
    }

    /* ============================================================================================
     * Janus-style grid (filter row, sortable headers, total row, record navigator, frozen columns,
     * button / link / check cells, optional cell editing, keyboard navigation).
     * ============================================================================================ */
    function Grid(cfg) {
        this.c = cfg; this.rows = []; this.view = []; this.filters = {}; this.sort = null; this.dir = 1;
        this.cur = -1; this.col = 0; this.editing = null;
        var self = this;
        this.$g = $(cfg.grid); this.$t = $(cfg.total); this.$n = $(cfg.nav);
        this.$g.attr('tabindex', '0');
        this.$g.on('scroll', function () { self.$t.scrollLeft(self.$g.scrollLeft()); });
        this.$g.on('click', 'thead th', function () {
            var k = $(this).data('col'); if (!k) return;
            var col = self.colByKey(k); if (!col || col.type === 'btn') return;
            if (self.sort === k) self.dir = -self.dir; else { self.sort = k; self.dir = 1; }
            self.apply(true);
        });
        this.$g.on('input', 'tr.flt input', function () { self.filters[$(this).data('f')] = this.value; self.cur = 0; self.apply(true); });
        this.$g.on('click', 'tbody td', function (e) {
            var $tr = $(this).closest('tr'); if ($tr.data('i') == null) return;
            var ci = +$(this).data('c'), i = +$tr.data('i'), col = self.c.cols[ci];
            self.commit();
            self.setCur(i, ci);
            if ($(e.target).is('button.cellbtn') && col.type === 'btn') self.act('button', i, col);
            else if ($(e.target).is('a.lnk') && col.type === 'lnk') { e.preventDefault(); self.act('link', i, col); }
        });
        this.$g.on('dblclick', 'tbody td', function () {
            var $tr = $(this).closest('tr'); if ($tr.data('i') == null) return;
            var col = self.c.cols[+$(this).data('c')];
            if (col.edit) { self.startEdit(+$tr.data('i'), col); return; }
            if (self.c.onDbl) self.c.onDbl(self.view[+$tr.data('i')]);
        });
        this.$g.on('keydown', function (e) { self.key(e); });
        this.$g.on('keydown', 'input.ed', function (e) { self.editKey(e); });
        this.$g.on('blur', 'input.ed', function () { self.commit(); });
        this.$n.on('click', 'button', function () {
            var a = $(this).data('nav'), n = self.view.length;
            self.commit();
            self.setCur(a === 'first' ? 0 : a === 'last' ? n - 1 : a === 'prev' ? self.cur - 1 : self.cur + 1, self.col);
        });
    }
    Grid.prototype.vcols = function () { var r = []; this.c.cols.forEach(function (c, i) { if (!c.hidden) r.push(i); }); return r; };
    Grid.prototype.colByKey = function (k) { var r = null; this.c.cols.forEach(function (c) { if (c.k === k) r = c; }); return r; };
    Grid.prototype.text = function (col, row) {
        var v = row[col.k];
        if (col.type === 'bool') return v ? 'True' : 'False';
        if (col.type === 'btn') return col.cap;
        return col.f ? col.f(v) : (v == null ? '' : String(v));
    };
    Grid.prototype.setRows = function (rows) { this.rows = rows || []; this.filters = {}; this.cur = this.rows.length ? 0 : -1; this.apply(true); };
    Grid.prototype.clear = function () { this.setRows([]); };
    Grid.prototype.current = function () { return this.cur >= 0 ? this.view[this.cur] : null; };
    Grid.prototype.apply = function (notify) {
        var self = this, cols = this.c.cols;
        var f = this.filters, keys = Object.keys(f).filter(function (k) { return $.trim(f[k]) !== ''; });
        this.view = this.rows.filter(function (r) {
            return keys.every(function (k) {
                var col = self.colByKey(k); if (!col) return true;
                return self.text(col, r).toLowerCase().indexOf($.trim(f[k]).toLowerCase()) >= 0;
            });
        });
        if (this.sort) {
            var sc = this.colByKey(this.sort), dir = this.dir;
            var idx = this.view.map(function (r, i) { return { r: r, i: i }; });
            idx.sort(function (a, b) {
                var x = a.r[sc.k], y = b.r[sc.k], c;
                if (typeof x === 'number' && typeof y === 'number') c = x - y;
                else c = String(x == null ? '' : x).localeCompare(String(y == null ? '' : y), undefined, { numeric: true, sensitivity: 'base' });
                return (c || (a.i - b.i)) * dir;
            });
            this.view = idx.map(function (o) { return o.r; });
        }
        this.cur = this.view.length ? Math.min(Math.max(this.cur, 0), this.view.length - 1) : -1;
        this.render();
        if (notify && this.c.onCur) this.c.onCur(this.current());
    };
    Grid.prototype.widths = function () {
        var self = this, w = {};
        this.c.cols.forEach(function (col) {
            if (col.hidden) return;
            if (col.w) { w[col.k] = col.w; return; }
            var m = String(col.cap || col.k).length;
            self.rows.forEach(function (r) { var l = self.text(col, r).length; if (l > m) m = l; });
            w[col.k] = Math.max(40, Math.min(420, m * 7 + 20));
        });
        return w;
    };
    Grid.prototype.render = function () {
        var self = this, cols = this.c.cols, vc = this.vcols(), W = this.widths();
        var left = {}, off = 0;
        vc.forEach(function (ci) { var col = cols[ci]; if (col.fz) { left[col.k] = off; off += W[col.k]; } });
        var tot = 0; vc.forEach(function (ci) { tot += W[cols[ci].k]; });
        var cg = '<colgroup>' + vc.map(function (ci) { return '<col style="width:' + W[cols[ci].k] + 'px">'; }).join('') + '</colgroup>';
        var tw = 'width:100%;min-width:' + tot + 'px';
        var h = '<table style="' + tw + '">' + cg + '<thead><tr>';
        vc.forEach(function (ci) {
            var col = cols[ci];
            h += '<th data-col="' + esc(col.k) + '" class="' + (col.r ? 'r ' : '') + (col.fz ? 'fz' : '') + '"'
                + (col.fz ? ' style="left:' + left[col.k] + 'px"' : '') + '>' + esc(col.cap || col.k)
                + (self.sort === col.k ? (self.dir > 0 ? ' &#9650;' : ' &#9660;') : '') + '</th>';
        });
        h += '</tr><tr class="flt">';
        vc.forEach(function (ci) {
            var col = cols[ci];
            h += '<td class="' + (col.fz ? 'fz' : '') + '"' + (col.fz ? ' style="left:' + left[col.k] + 'px"' : '') + '>'
                + (col.type === 'btn' ? '' : '<input data-f="' + esc(col.k) + '" value="' + esc(self.filters[col.k] || '') + '">') + '</td>';
        });
        h += '</tr></thead><tbody>';
        if (!this.view.length) h += '<tr><td class="empty" colspan="' + vc.length + '"></td></tr>';
        this.view.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === self.cur ? ' class="cur"' : '') + '>';
            vc.forEach(function (ci) {
                var col = cols[ci], t = self.text(col, r), cls = [];
                if (col.r) cls.push('r'); if (col.fz) cls.push('fz');
                if (i === self.cur && ci === self.col) cls.push('cell-sel');
                h += '<td data-c="' + ci + '" class="' + cls.join(' ') + '"' + (col.fz ? ' style="left:' + left[col.k] + 'px"' : '') + ' title="' + (col.type === 'btn' ? '' : esc(t)) + '">';
                if (col.type === 'btn') h += '<button type="button" class="cellbtn" tabindex="-1">' + esc(col.cap) + '</button>';
                else if (col.type === 'lnk') h += '<a class="lnk" tabindex="-1">' + esc(t) + '</a>';
                else if (col.type === 'bool') h += '<input type="checkbox" disabled' + (r[col.k] ? ' checked' : '') + '>';
                else h += esc(t);
                h += '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody></table>';
        var sl = this.$g.scrollLeft(), st = this.$g.scrollTop();
        this.$g.html(h);
        this.$g.scrollLeft(sl); this.$g.scrollTop(st);
        /* total row (TotalRow = true; only the columns with an aggregate show a figure) */
        var th = '<table style="' + tw + '">' + cg + '<tbody><tr>';
        vc.forEach(function (ci) {
            var col = cols[ci], s = '';
            if (col.sum) { var sum = 0; self.view.forEach(function (r) { sum += toD(r[col.k]); }); s = (col.tf || col.f || String)(sum); }
            th += '<td>' + esc(s) + '</td>';
        });
        th += '</tr></tbody></table>';
        this.$t.html(th).scrollLeft(this.$g.scrollLeft());
        this.nav();
        if (this.c.onRender) this.c.onRender(this);
    };
    Grid.prototype.nav = function () {
        var n = this.view.length;
        if (!this.$n.children().length) {
            this.$n.html('<button type="button" data-nav="first" title="First">|&#9664;</button><button type="button" data-nav="prev" title="Previous">&#9664;</button>'
                + '<span class="nt"></span><button type="button" data-nav="next" title="Next">&#9654;</button><button type="button" data-nav="last" title="Last">&#9654;|</button>');
        }
        this.$n.find('.nt').text('Record ' + (this.cur >= 0 ? this.cur + 1 : 0) + ' of ' + n);
        this.$n.find('[data-nav=first],[data-nav=prev]').prop('disabled', this.cur <= 0);
        this.$n.find('[data-nav=next],[data-nav=last]').prop('disabled', this.cur < 0 || this.cur >= n - 1);
    };
    Grid.prototype.setCur = function (i, ci) {
        if (!this.view.length) { this.cur = -1; this.nav(); return; }
        var changed = i !== this.cur;
        this.cur = Math.max(0, Math.min(this.view.length - 1, i));
        if (ci != null) this.col = ci;
        var vc = this.vcols(); if (vc.indexOf(this.col) < 0) this.col = vc[0];
        this.$g.find('tbody tr').removeClass('cur').find('td').removeClass('cell-sel');
        var $tr = this.$g.find('tbody tr[data-i="' + this.cur + '"]').addClass('cur');
        $tr.find('td[data-c="' + this.col + '"]').addClass('cell-sel');
        if ($tr.length) $tr[0].scrollIntoView({ block: 'nearest', inline: 'nearest' });
        this.nav();
        if (changed && this.c.onCur) this.c.onCur(this.current());
    };
    Grid.prototype.act = function (kind, i, col) { if (this.c.onAct) this.c.onAct(kind, this.view[i], col); };
    Grid.prototype.key = function (e) {
        if ($(e.target).is('input')) return;
        var vc = this.vcols(), pos = vc.indexOf(this.col), self = this, col = this.c.cols[this.col];
        var row = this.current();
        if (e.ctrlKey && this.c.onKey) { if (this.c.onKey(e, row, col) === true) return; }
        switch (e.key) {
            case 'ArrowDown': e.preventDefault(); if (!e.ctrlKey) this.setCur(this.cur + 1); break;
            case 'ArrowUp': e.preventDefault(); if (!e.ctrlKey) this.setCur(this.cur - 1); break;
            case 'ArrowRight': if (e.ctrlKey) return; e.preventDefault(); this.setCur(this.cur, vc[Math.min(vc.length - 1, pos + 1)]); break;
            case 'ArrowLeft': if (e.ctrlKey) return; e.preventDefault(); this.setCur(this.cur, vc[Math.max(0, pos - 1)]); break;
            case 'PageDown': e.preventDefault(); this.setCur(this.cur + 10); break;
            case 'PageUp': e.preventDefault(); this.setCur(this.cur - 10); break;
            case 'Home': if (e.ctrlKey) { e.preventDefault(); this.setCur(0); } break;
            case 'End': if (e.ctrlKey) { e.preventDefault(); this.setCur(this.view.length - 1); } break;
            case 'F2': if (col && col.edit && row) { e.preventDefault(); this.startEdit(this.cur, col); } break;
            case 'Enter': if (!e.ctrlKey && col && col.edit && row) { e.preventDefault(); this.startEdit(this.cur, col); } break;
            default:
                if (col && col.edit && row && !e.ctrlKey && !e.altKey && !e.metaKey && e.key.length === 1) { e.preventDefault(); this.startEdit(this.cur, col, e.key); }
        }
    };
    /* ---- editing (Percent% / Amount of the detail grid) ---- */
    Grid.prototype.startEdit = function (i, col, first) {
        if (this.editing) return;
        var row = this.view[i]; if (!row || !col.edit) return;
        var $td = this.$g.find('tbody tr[data-i="' + i + '"] td[data-c="' + this.c.cols.indexOf(col) + '"]');
        if (!$td.length) return;
        var init = this.text(col, row).replace(/,/g, '');
        this.editing = { i: i, col: col, init: init };
        $td.addClass('edit').html('<input class="ed" value="' + esc(first != null ? first : init) + '">');
        var $in = $td.find('input'); $in.focus();
        if (first == null) $in[0].select(); else { var l = $in.val().length; $in[0].setSelectionRange(l, l); }
    };
    Grid.prototype.editKey = function (e) {
        if (e.key === 'Enter' || e.key === 'Tab') {
            e.preventDefault(); e.stopPropagation();
            var back = e.shiftKey && e.key === 'Tab';
            this.commit();
            this.moveEdit(back);
            this.$g.focus();
        } else if (e.key === 'Escape') {
            e.preventDefault(); e.stopPropagation();
            this.editing = null; this.render(); this.$g.focus();
        }
        e.stopPropagation();
    };
    Grid.prototype.moveEdit = function (back) {
        var ed = this.c.cols.map(function (c, i) { return c.edit && !c.hidden ? i : -1; }).filter(function (i) { return i >= 0; });
        if (!ed.length || this.cur < 0) return;
        var p = ed.indexOf(this.col);
        if (!back) { if (p >= 0 && p < ed.length - 1) this.setCur(this.cur, ed[p + 1]); else if (this.cur < this.view.length - 1) this.setCur(this.cur + 1, ed[0]); }
        else { if (p > 0) this.setCur(this.cur, ed[p - 1]); else if (this.cur > 0) this.setCur(this.cur - 1, ed[ed.length - 1]); }
    };
    Grid.prototype.commit = function () {
        var ed = this.editing; if (!ed) return;
        this.editing = null;
        var $in = this.$g.find('input.ed'), t = $.trim($in.val() == null ? ed.init : $in.val()).replace(/,/g, '');
        var row = this.view[ed.i];
        if (row && t !== ed.init) {
            var n = Number(t);
            if (t === '' || isNaN(n) || !isFinite(n)) { if (t === '') n = 0; else n = null; }
            if (n != null) {
                row[ed.col.k] = n;
                this.render();
                if (this.c.onCellUpdated) this.c.onCellUpdated(row, ed.col);
                return;
            }
        }
        this.render();
    };

    /* ============================================================================================
     * Grids
     * ============================================================================================ */
    var grdDetail, gridHistory, gridHistDetail;
    function detailCols() {
        return [
            { k: 'id', hidden: true },
            { k: 'month', cap: 'Month' },
            { k: 'percent', cap: 'Percent%', r: true, f: fmtPct, tf: fmtPct, sum: true, edit: true },
            { k: 'amount', cap: 'Amount', r: true, f: fmtAmt, tf: fmtAmt, sum: true, edit: true }
        ];
    }
    function historyCols() {
        return [
            { k: 'Id', hidden: true }, { k: 'DocumentTypeId', hidden: true },
            { k: 'Edit', cap: 'Edit', type: 'btn', w: 50, fz: true },
            { k: 'Print', cap: 'Print', type: 'btn', w: 50, fz: true },
            { k: 'DocNo', cap: 'DocNo', r: true }, { k: 'DocDate', cap: 'DocDate' },
            { k: 'FromMonth', cap: 'FromMonth' }, { k: 'ToMonth', cap: 'ToMonth' }, { k: 'ExpenseAccount', cap: 'ExpenseAccount' },
            { k: 'Amount', cap: 'Amount', r: true, f: fmtAmt, tf: fmtAmt, sum: true },
            { k: 'EntryUserName', cap: 'EntryUserName' }, { k: 'EntryDate', cap: 'EntryDate' },
            { k: 'ModifyUserName', cap: 'ModifyUserName' }, { k: 'ModifyDate', cap: 'ModifyDate' },
            { k: 'IsApproved', cap: 'IsApproved', type: 'bool' },
            { k: 'ApprovedUserName', cap: 'ApprovedUserName' }, { k: 'ApprovedDate', cap: 'ApprovedDate' },
            { k: 'NoOfAttachments', cap: 'NoOfAttachments', type: 'lnk', r: true }
        ];
    }
    function histDetailCols() {
        return [
            { k: 'Id', hidden: true }, { k: 'Month', cap: 'Month' },
            { k: 'Percent', cap: 'Percent%', r: true, f: fmtPct, tf: fmtPct, sum: true },
            { k: 'Amount', cap: 'Amount', r: true, f: fmtAmt, tf: fmtAmt, sum: true }
        ];
    }

    /* grdDetail_CellUpdated: grdDetail.UpdateData(); the other column is recomputed from the cell .Text (rounded display text) */
    function detailCellUpdated(row, col) {
        var amt = toD($('#txtAmount').val());
        if (col.k === 'percent') row.amount = amt * toD(fmtPct(row.percent)) / 100;
        if (col.k === 'amount') row.percent = toD(fmtAmt(row.amount)) / amt * 100;
        S.rows = grdDetail.rows;
        grdDetail.apply(false);
    }

    /* ============================================================================================
     * Load / lookups
     * ============================================================================================ */
    function projectFill(items) {
        var id = toI(cval('combproject'));
        S.projects = items;
        if (items.length > 0) {
            bind('combproject', items, 'Id', 'ProjectName', false);
            cset('combproject', items[0].Id);                           /* Rows[0].Activate() */
            if (id > 0) { if (chas('combproject', id)) cset('combproject', id); else cset('combproject', ''); }
        } else { bind('combproject', [], 'Id', 'ProjectName', false); cset('combproject', ''); }
    }
    function coaBind(items) {
        var id = toI(cval('cmbExpenseAcc')), id2 = toI(cval('CmbExpenseAccountHistory'));
        S.accounts = items;
        if (items.length > 0) {
            bind('cmbExpenseAcc', items, 'Id', 'AccountTitle', true);
            bind('CmbExpenseAccountHistory', items, 'Id', 'AccountTitle', true);
            cset('cmbExpenseAcc', '0'); cset('CmbExpenseAccountHistory', '0');      /* BindDDL ZeroIndex: Value = 0 */
            if (id > 0) { if (chas('cmbExpenseAcc', id)) cset('cmbExpenseAcc', id); else cset('cmbExpenseAcc', ''); }
            /* the desktop assigns CmbExpenseAccountHistory.Value = Id (the entry combo's id) - reproduced as is */
            if (id2 > 0) { if (chas('CmbExpenseAccountHistory', id2)) cset('CmbExpenseAccountHistory', id); else cset('CmbExpenseAccountHistory', ''); }
        } else {
            bind('cmbExpenseAcc', [], 'Id', 'AccountTitle', false); bind('CmbExpenseAccountHistory', [], 'Id', 'AccountTitle', false);
            cset('cmbExpenseAcc', ''); cset('CmbExpenseAccountHistory', '');
        }
    }
    function monthFill(list) {
        S.months = list;
        var items = list.map(function (m) { return { Id: m, Month: m }; });
        if (items.length > 0) {
            bind('cmbMonthFrom', items, 'Id', 'Month', false); bind('CmbMonthTo', items, 'Id', 'Month', false);
        } else { bind('cmbMonthFrom', [], 'Id', 'Month', false); bind('CmbMonthTo', [], 'Id', 'Month', false); }
        /* cmbMonthFrom.Value = DateTime.Now.Month (an int against "MMM yyyy" keys): no row matches, the combo shows nothing */
        cset('cmbMonthFrom', ''); cset('CmbMonthTo', '');
    }
    /* the "current month / next month" defaults of Load and FormReset */
    function defaultMonths() {
        if (ccount('cmbMonthFrom') > 0) {
            var now = new Date();
            var f = monthText(now);
            if (S.months.indexOf(f) >= 0) cset('cmbMonthFrom', f);
            var n = new Date(now.getFullYear(), now.getMonth() + 1, 1);
            var t = monthText(n);
            if (S.months.indexOf(t) >= 0) cset('CmbMonthTo', t);
        }
    }
    function generateCode() {
        return getJson(API + '/docno').done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            $('#txtdocno').val(String(r.docNo));
        }).fail(function (x) { msg(failText(x)); });
    }

    /* AccountBudget_Load */
    function load() {
        return getJson(API + '/init').done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            S.rights = r.rights || S.rights; S.dec = r.amountDecimals || 0;
            applyRights();
            S.rows = []; grdDetail.setRows([]);
            projectFill(r.projects || []);
            $('#txtdocno').val(String(r.docNo));
            coaBind(r.accounts || []);
            monthFill(r.months || []);
            $('#DocDate').focus();
            defaultMonths();
            grdDetail.render(); gridHistory.render(); gridHistDetail.render();
        }).fail(function (x) { msg(failText(x)); });
    }

    /* btnRefresh_Click: ProjectFill(); COABind(); MonthFill(); */
    function doRefresh() {
        return getJson(API + '/lookups').done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            projectFill(r.projects || []); coaBind(r.accounts || []); monthFill(r.months || []);
        }).fail(function (x) { msg(failText(x)); });
    }

    /* ============================================================================================
     * Form tab actions
     * ============================================================================================ */
    function resetAttachments() { S.att = { existing: [], added: [], removed: [] }; }
    /* FormReset() */
    function formReset() {
        cset('cmbExpenseAcc', '');
        $('#txtAmount').val('');
        S.updateMode = false; S.chkAmountFiled = false; S.recId = 0;
        generateCode();
        S.rows = []; grdDetail.setRows([]);
        resetAttachments();
        showSave(true);
        $('#DocDate').focus();
        defaultMonths();
    }

    /* btnGenerate_Click */
    function doGenerate() {
        if (S.busy) return;
        var a = $.trim($('#txtAmount').val());
        if (a === '' || toD($('#txtAmount').val()) === 0) { msg('Amount  Field Required'); $('#txtAmount').focus(); return; }
        S.rows = []; grdDetail.setRows([]);
        var from = parseMonth(ctext('cmbMonthFrom'), ' '), to = parseMonth(ctext('CmbMonthTo'), ' ');
        if (!from || !to) { msg(BAD_DATE); return; }
        var mdiff = Math.trunc(daysBetween(from, to) / 30.4) + 1;
        if (mdiff === 0) { msg('Attempted to divide by zero.'); return; }          /* decimal / 0 */
        var perMonth = toD($('#txtAmount').val()) / mdiff, pct = 100 / mdiff;
        var rows = [];
        for (var i = 0; i < mdiff; i++) {
            if (!(from.getTime() <= to.getTime())) break;
            rows.push({ id: 0, month: dashText(from), percent: pct, amount: perMonth });
            from = addMonth(from);
            S.chkAmountFiled = true;
        }
        S.rows = rows; grdDetail.setRows(rows);
    }

    /* FormValidation() - same order, message and focus */
    function formValidation() {
        if (cval('combproject') === '' || toI(cval('combproject')) === 0) { msg('Project  Field Required'); cfocus('combproject'); return false; }
        var d = $.trim($('#txtdocno').val());
        if (d === '' || d === '0') { msg('Doc No   Field Required'); $('#txtdocno').focus(); return false; }
        if (cval('cmbMonthFrom') === '' || ctext('cmbMonthFrom') === '') { msg('MonthFrom  Field Required'); cfocus('cmbMonthFrom'); return false; }
        if (cval('CmbMonthTo') === '' || ctext('CmbMonthTo') === '') { msg('Month To  Field Required'); cfocus('CmbMonthTo'); return false; }
        if (cval('cmbExpenseAcc') === '' || toI(cval('cmbExpenseAcc')) === 0) { msg('Exp Account Field Required'); cfocus('cmbExpenseAcc'); return false; }
        if (toD($.trim($('#txtAmount').val())) === 0 || toD($('#txtAmount').val()) === 0) { msg('Amount  Field Required'); $('#txtAmount').focus(); return false; }
        return true;
    }

    /* Insert() - Save and Update */
    function insert() {
        if (S.busy) return;
        grdDetail.commit();
        var rows = grdDetail.rows;
        if (rows.length === 0) { msg('Grid Record Not Found'); return; }
        var f = parseMonth(ctext('cmbMonthFrom'), ' '), t = parseMonth(ctext('CmbMonthTo'), ' ');
        if (!f || !t) { msg(BAD_DATE); return; }
        var expected = rint(daysBetween(f, t) / 30.4);                      /* Conversion.ToInt(double) = Convert.ToInt32 */
        if (Math.abs(expected - rows.length) > 1 || Math.abs(expected - rows.length) < 1) { msg('Month count does not match with Detail Record'); return; }
        if (!formValidation()) return;
        var sp = 0, sa = 0;
        rows.forEach(function (r) { sp += toD(r.percent); sa += toD(r.amount); });
        if (rint(sp) !== 100) { msg('Grid Total Percent Not Equal To 100'); return; }
        if (rint(sa) !== toD($.trim($('#txtAmount').val()))) { msg('Grid Total Amount and Header Amount Not Match '); return; }
        if (S.recId > 0) { if (!window.confirm('Are you sure to Update?')) return; }
        else if (!window.confirm('Are you sure to Save?')) return;

        var wasUpdate = S.recId > 0, print = $('#ChkPrint').prop('checked');
        var body = {
            id: S.recId, projectId: toI(cval('combproject')), docNo: $.trim($('#txtdocno').val()), docDate: $('#DocDate').val(),
            fromMonth: ctext('cmbMonthFrom'), toMonth: ctext('CmbMonthTo'), amount: $('#txtAmount').val(),
            expenseAccountId: toI(cval('cmbExpenseAcc')),
            rows: rows.map(function (r) { return { id: r.id, month: r.month, percent: toD(r.percent), amount: toD(r.amount) }; }),
            attachments: { files: S.att.added.map(function (a) { return { name: a.name, base64: a.b64 }; }), removeIds: S.att.removed.slice() }
        };
        busy(true);
        postJson(API + '/save', body).done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            msg(r.message);
            formReset();
            if (print) printSlip(r.id);
        }).fail(function (x) { msg(failText(x)); }).always(function () { busy(false); });
    }
    function doSave() { S.recId = 0; insert(); }                    /* BtnSave_Click: RecId = 0; Insert() */
    function doUpdate() { insert(); }                               /* btnUpdate_Click */

    /* ReadById(Id) */
    function readById(id) {
        return getJson(API + '/' + id).done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            if (!r.loaded) return;                                      /* header null or no detail rows: nothing happens */
            switchTab(0, true);
            S.recId = id;
            cset('combproject', chas('combproject', r.projectId) ? r.projectId : '');
            $('#txtdocno').val(String(r.docNo));
            if (r.docDate) $('#DocDate').val(r.docDate);
            if (S.months.indexOf(r.fromMonth) >= 0) cset('cmbMonthFrom', r.fromMonth);
            if (S.months.indexOf(r.toMonth) >= 0) cset('CmbMonthTo', r.toMonth);
            $('#txtAmount').val(String(r.amount));
            cset('cmbExpenseAcc', chas('cmbExpenseAcc', r.expenseAccountId) ? r.expenseAccountId : '');
            S.rows = (r.rows || []).map(function (x) { return { id: x.Id, month: x.Month, percent: x.Percent, amount: x.Amount }; });
            grdDetail.setRows(S.rows);
            S.att = { existing: (r.attachments || []).slice(), added: [], removed: [] };      /* FormHelper.LoadAttachmentsForObject */
            showSave(false);
            S.updateMode = true;
        }).fail(function (x) { msg(failText(x)); });
    }

    /* Print slip: CommonServices.AccountsBudgetSlip_11(PrintId) -> 11-AccountsBudgetSlip.rpt through the existing print contract */
    function printSlip(id) {
        if (!id) { msg('Record Not Found'); return; }
        var w = null; try { w = window.open('', '_blank'); } catch (e) { w = null; }
        if (w) { try { w.document.write('<p style="font:13px Segoe UI,sans-serif;padding:16px">Preparing report...</p>'); } catch (e) { /* ignore */ } }
        var h = { 'Content-Type': 'application/json', 'Accept': 'application/pdf, text/plain' };
        var tk = document.querySelector('meta[name="_csrf"]'), hd = document.querySelector('meta[name="_csrf_header"]');
        if (tk && hd) h[hd.getAttribute('content')] = tk.getAttribute('content');
        return fetch('/reports/print/11-accounts-budget-slip', { method: 'POST', credentials: 'same-origin', headers: h,
            body: JSON.stringify({ id: id, documentTypeId: DOC_TYPE }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                return r.text().then(function (t) { if (w) w.close(); msg(t || 'No Record Found For Display'); });
            })
            .catch(function (e) { if (w) w.close(); msg(e.message); });
    }

    /* ============================================================================================
     * Tabs / History
     * ============================================================================================ */
    function switchTab(i, silent) {
        S.tab = i;
        $('#pgForm').toggleClass('on', i === 0); $('#pgHistory').toggleClass('on', i === 1);
        $('#tabForm').toggleClass('on', i === 0); $('#tabHistory').toggleClass('on', i === 1);
        if (i === 0) { grdDetail.render(); } else { gridHistory.render(); gridHistDetail.render(); }
        if (!silent) { if (i === 0) $('#DocDate').focus(); else $('#txtFromdateHistory').focus(); }   /* tabControl1_SelectedIndexChanged */
        else $('#DocDate').focus();
    }

    /* Resethistory() */
    function resetHistory() {
        $('#txtFromNoHistory').val(''); $('#txtToDocNoHistory').val('');
        cset('CmbExpenseAccountHistory', '');
        cfocus('CmbExpenseAccountHistory');
    }

    /* HistoryGridBind() */
    function historyBind() {
        if (S.busy) return;
        busy(true);
        postJson(API + '/history', { fromDate: $('#txtFromdateHistory').val(), toDate: $('#txtToDateHistory').val(),
            fromDocNo: $('#txtFromNoHistory').val(), toDocNo: $('#txtToDocNoHistory').val(), accountText: ctext('CmbExpenseAccountHistory') })
            .done(function (r) {
                if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
                var rows = r.rows || [];
                if (rows.length > 0) {
                    S.histRows = rows;
                    gridHistory.setRows(rows);
                    gridHistory.col = gridHistory.vcols()[0];
                } else { gridHistory.clear(); gridHistDetail.clear(); }
            })
            .fail(function (x) { msg(failText(x)); }).always(function () { busy(false); });
    }
    /* DataGridHistory_SelectionChanged -> grdHistoryDetailbind(Id) */
    var detailSeq = 0;
    function historyDetail(row) {
        if (!row) return;
        var seq = ++detailSeq;
        getJson(API + '/' + row.Id + '/details').done(function (r) {
            if (seq !== detailSeq) return;
            if (!r || r.success === false) { window.alert((r && r.message) || 'Error'); return; }
            gridHistDetail.setRows((r.rows || []).map(function (x) { return { Id: x.Id, Month: x.Month, Percent: x.Percent, Amount: x.Amount }; }));
        }).fail(function (x) { window.alert(failText(x)); });
    }
    /* ColumnButtonClick / Link / Ctrl+Space on Edit / Print / NoOfAttachments */
    function histAction(row, colKey) {
        if (!row) return;
        if (colKey === 'Edit') readById(row.Id);
        if (colKey === 'Print') printSlip(row.Id);
        if (colKey === 'NoOfAttachments') viewAttachments(row.Id);
    }

    /* ============================================================================================
     * Attachment form (AT.Show()) / AttachmentView / ShortCutKeyPopUp
     * ============================================================================================ */
    function mb(n) { return (n || 0).toFixed(2) + ' Mb'; }
    function openAttach() { renderAttach(); $('#dlgAttach').addClass('on'); }
    function renderAttach() {
        var h = '';
        S.att.existing.forEach(function (a) {
            h += '<div class="ab-card" data-k="e" data-id="' + a.Id + '"><span class="nm">' + esc(a.Attachment) + '</span><span>' + mb(a.SizeMb) + '</span><span>'
                + esc(String(a.EntryDate || '').substring(0, 10)) + '</span><button type="button" class="del">Delete</button></div>';
        });
        S.att.added.forEach(function (a, i) {
            h += '<div class="ab-card" data-k="n" data-i="' + i + '"><span class="nm">' + esc(a.name) + '</span><span>' + mb(a.size / 1048576) + '</span><span>'
                + esc(a.date) + '</span><button type="button" class="del">Delete</button></div>';
        });
        $('#atCards').html(h);
        $('#atEmpty').toggle(!h);
    }
    function readFiles(files) {
        var list = Array.prototype.slice.call(files || []);
        if (!list.length) return;
        if (list.length > 10) { msg('Select at most ten files at once'); return; }
        var todo = list.length;
        list.forEach(function (f) {
            if (f.size === 0 || f.size > 5 * 1024 * 1024) { msg('Attachment must be between 1 byte and 5 MB'); if (--todo === 0) renderAttach(); return; }
            var fr = new FileReader();
            fr.onload = function () {
                var s = String(fr.result), b64 = s.substring(s.indexOf(',') + 1);
                S.att.added.push({ name: f.name, size: f.size, b64: b64, date: new Date().toLocaleDateString(), blob: URL.createObjectURL(f) });
                if (--todo === 0) renderAttach();
            };
            fr.onerror = function () { msg('Could not read ' + f.name); if (--todo === 0) renderAttach(); };
            fr.readAsDataURL(f);
        });
    }
    /* AttachmentView (History NoOfAttachments link): GetNoofAttachmentsByRefDocumentTypeID(id, 11); nothing opens when there are none */
    function viewAttachments(id) {
        getJson(API + '/' + id + '/attachments/by-document-type').done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            var rows = r.rows || [];
            if (!rows.length) return;
            var h = '', tot = 0;
            rows.forEach(function (a) {
                tot += a.SizeMb || 0;
                h += '<tr><td><a href="' + API + '/' + id + '/attachments/' + a.Id + '" target="_blank" rel="noopener">' + esc(a.Attachment) + '</a></td><td>' + esc(a.FileName) + '</td><td>'
                    + esc(String(a.EntryDate || '').substring(0, 10)) + '</td></tr>';
            });
            $('#vwTbl tbody').html(h);
            $('#vwTot').text('Total Files ' + rows.length + '    Total Size (Mbs) ' + tot.toFixed(3));
            $('#dlgView').addClass('on');
        }).fail(function (x) { msg(failText(x)); });
    }
    var KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print Slip 1853'], ['Ctrl+F5', 'For Focus on DocDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function showKeys() {
        $('#kyTbl tbody').html(KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join(''));
        $('#dlgKeys').addClass('on');
    }
    function anyModal() { return $('.ab-modal.on').length > 0; }

    /* ============================================================================================
     * Enter -> Tab (SendKeys {TAB}); explicit order because the combos hide their native select
     * ============================================================================================ */
    var ORDER = {
        0: ['combproject', 'txtdocno', 'cmbMonthFrom', 'CmbMonthTo', 'cmbExpenseAcc', 'txtAmount', 'btnGenerate'],
        1: ['txtFromdateHistory', 'txtToDateHistory', 'txtFromNoHistory', 'txtToDocNoHistory', 'CmbExpenseAccountHistory', 'BtnShowInputHistory']
    };
    function focusId(id) {
        var $e = $('#' + id);
        if ($e.is('select')) cfocus(id); else $e.focus();
    }
    function idOf(t) {
        var $c = $(t).closest('.ab-cb');
        if ($c.length) return $c.find('select').attr('id');
        return t.id;
    }
    function closeForm() { window.location.href = '/accounts/dashboard'; }          /* Close() */

    /* ============================================================================================
     * Wiring
     * ============================================================================================ */
    $(function () {
        grdDetail = new Grid({ grid: '#gridDetail', total: '#totDetail', nav: '#navDetail', cols: detailCols(), onCellUpdated: detailCellUpdated });
        gridHistory = new Grid({ grid: '#gridHistory', total: '#totHistory', nav: '#navHistory', cols: historyCols(),
            onCur: historyDetail,
            onDbl: function (row) { if (row) readById(row.Id); },                         /* DataGridHistory_DoubleClick */
            onAct: function (kind, row, col) { histAction(row, col.k); },                   /* ColumnButtonClick / LinkClicked */
            onKey: function (e, row, col) {                                                 /* DataGridHistory_KeyDown */
                if (!row) return false;
                if (e.key === ' ' && col && (col.k === 'Edit' || col.k === 'Print' || col.k === 'NoOfAttachments')) { e.preventDefault(); histAction(row, col.k); return true; }
                if (e.key === 'Enter') { e.preventDefault(); readById(row.Id); return true; }
                if ((e.key === 'p' || e.key === 'P') && !e.altKey) { e.preventDefault(); printSlip(row.Id); return true; }
                return false;
            } });
        gridHistDetail = new Grid({ grid: '#gridHistDetail', total: '#totHistDetail', nav: '#navHistDetail', cols: histDetailCols() });
        $('#wrapHistory').attr('data-gridbar-form', 'AccountBudget');

        $('#BtnNew').on('click', formReset);
        $('#btnRefresh').on('click', doRefresh);
        $('#BtnSave').on('click', doSave);
        $('#btnUpdate').on('click', doUpdate);
        $('#BtnPrint').on('click', function () { printSlip(S.recId); });
        $('#btnattachment').on('click', openAttach);
        $('#btnshortcutkeys').on('click', showKeys);
        $('#btnGenerate').on('click', doGenerate);
        $('#btnNewHistory').on('click', resetHistory);
        $('#BtnShowInputHistory').on('click', historyBind);
        $('#tabForm').on('click', function () { switchTab(0); });
        $('#tabHistory').on('click', function () { switchTab(1); });
        $('#ChkPrint').on('change', function () { /* ChkPrint_CheckedChanged: empty handler */ });

        /* txtAmount_KeyPress (OnlytextdecimelFunction) and the history doc-no boxes keep free text like the desktop */
        $('#txtAmount').on('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.key.length > 1) return;
            if (!/[0-9.]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') >= 0)) e.preventDefault();
        });

        /* Attachment form */
        $('#atChoose').on('click', function () { $('#atFile').click(); });
        $('#atFile').on('change', function () { readFiles(this.files); this.value = ''; });
        $('#atClose, #atX').on('click', function () { $('#dlgAttach').removeClass('on'); });
        $('#atCards').on('click', '.del', function (e) {
            e.stopPropagation();
            var $c = $(this).closest('.ab-card');
            if ($c.data('k') === 'e') {
                var id = +$c.data('id');
                S.att.existing = S.att.existing.filter(function (a) { return a.Id !== id; });
                S.att.removed.push(id);                              /* RemovedAttachmentListInUpdateCase */
            } else {
                var a = S.att.added.splice(+$c.data('i'), 1)[0];
                if (a && a.blob) URL.revokeObjectURL(a.blob);
            }
            renderAttach();
        });
        $('#atCards').on('click', '.nm', function () {
            var $c = $(this).closest('.ab-card');
            if ($c.data('k') === 'e') window.open(API + '/' + S.recId + '/attachments/' + $c.data('id'), '_blank');
            else { var a = S.att.added[+$c.data('i')]; if (a && a.blob) window.open(a.blob, '_blank'); }
        });
        $('#vwClose, #vwX').on('click', function () { $('#dlgView').removeClass('on'); });
        $('#kyClose, #kyX').on('click', function () { $('#dlgKeys').removeClass('on'); });
        if (!$('#vwTot').length) $('#dlgView .bd').append('<div id="vwTot" style="margin-top:6px;font-weight:bold"></div>');

        /* AccountBudget_KeyDown (KeyPreview) */
        $(document).on('keydown', function (e) {
            var k = e.key, ctrl = e.ctrlKey, t = e.target, tag = (t.tagName || '').toLowerCase();
            try {
                if (ctrl && e.altKey) { if (!anyModal()) { e.preventDefault(); showKeys(); } return; }
                if (anyModal()) { if (k === 'Escape') { e.preventDefault(); $('.ab-modal.on').removeClass('on'); } return; }
                if ($(t).is('input.ed')) return;
                /* Return -> SendKeys {TAB} */
                if (k === 'Enter' && !ctrl && !e.altKey && !e.shiftKey && tag !== 'button' && tag !== 'textarea' && !$(t).closest('#gridDetail,#gridHistory,#gridHistDetail,.ab-nav').length) {
                    var order = ORDER[S.tab], id = idOf(t), p = order.indexOf(id);
                    if (id === 'DocDate') {                                                    /* TabStop = false: the next stop after it */
                        setTimeout(function () { focusId('cmbMonthFrom'); }, 0);
                    } else if (p >= 0) {
                        setTimeout(function () { focusId(order[(p + 1) % order.length]); }, 0);      /* an open combo list finishes its pick first */
                    }
                }
                if (ctrl && (k === 't' || k === 'T')) {
                    e.preventDefault();
                    if (S.tab === 1) switchTab(0); else switchTab(1);
                    return;
                }
                if ((ctrl && (k === 'e' || k === 'E')) || k === 'Escape') {
                    if ($('.dtcombo-pop').filter(function () { return this.style.display === 'block'; }).length) return;     /* Esc belongs to an open combo list */
                    e.preventDefault(); closeForm(); return;
                }
                if (S.tab === 0) {
                    if (ctrl && (k === 's' || k === 'S')) { e.preventDefault(); if (!$('#BtnSave').hasClass('ab-hidden') && !$('#BtnSave').prop('disabled')) doSave(); }
                    if (ctrl && (k === 'u' || k === 'U')) { e.preventDefault(); if (!$('#btnUpdate').hasClass('ab-hidden') && !$('#btnUpdate').prop('disabled')) doUpdate(); }
                    if (ctrl && (k === 'n' || k === 'N')) { e.preventDefault(); formReset(); }
                    if (ctrl && (k === 'r' || k === 'R')) { e.preventDefault(); doRefresh(); }
                    if (ctrl && k === 'F5') { e.preventDefault(); $('#DocDate').focus(); }
                    if (ctrl && k === 'ArrowDown') { e.preventDefault(); $('#gridDetail').focus(); }
                    if (ctrl && k === 'F10') { e.preventDefault(); openAttach(); }
                    if (ctrl && (k === 'p' || k === 'P')) { e.preventDefault(); printSlip(S.recId); }
                    /* Alt accelerators of the tool strip (&New, &Save, &Update, 11-&Print, &Attachment) */
                    if (e.altKey && !ctrl) {
                        var a = k.toLowerCase();
                        if (a === 'n') { e.preventDefault(); formReset(); }
                        else if (a === 's') { e.preventDefault(); if (!$('#BtnSave').hasClass('ab-hidden') && !$('#BtnSave').prop('disabled')) doSave(); }
                        else if (a === 'u') { e.preventDefault(); if (!$('#btnUpdate').hasClass('ab-hidden') && !$('#btnUpdate').prop('disabled')) doUpdate(); }
                        else if (a === 'p') { e.preventDefault(); if (!$('#BtnPrint').prop('disabled')) printSlip(S.recId); }
                        else if (a === 'a') { e.preventDefault(); openAttach(); }
                    }
                    return;
                }
                /* History tab */
                if (e.altKey && !ctrl && k.toLowerCase() === 'n') { e.preventDefault(); resetHistory(); return; }
                if (ctrl && k === 'F5') { e.preventDefault(); $('#gridHistory').focus(); }
                if (ctrl && k === 'ArrowDown') { e.preventDefault(); $('#gridHistDetail').focus(); }
                if (ctrl && k === 'ArrowRight') {
                    e.preventDefault();
                    if (document.activeElement === $('#gridHistDetail')[0]) $('#gridHistory').focus(); else $('#gridHistDetail').focus();
                }
                if (ctrl && k === 'ArrowUp') { e.preventDefault(); $('#txtFromdateHistory').focus(); }
            } catch (ex) { msg(ex.message); }
        });

        /* defaults of the DateTimePickers (Value = Now) */
        $('#DocDate, #txtFromdateHistory, #txtToDateHistory').val(todayIso());
        showSave(true);
        load();
    });
})(window.jQuery);
