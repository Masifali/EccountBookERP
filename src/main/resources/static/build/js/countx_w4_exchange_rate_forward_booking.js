/* Screen 199 Exchange Rate Forward Booking - desktop ExchangeRateForwardBooking.cs (class prefix W4). */
(function ($) {
    'use strict';
    var API = '/export/exchange-rate-forward-booking';
    var S = { recId: 0, fmt: { amountDecimals: 0, rateDecimals: 2 }, rights: {}, rows: [], att: { existing: [], added: [], removed: [] }, selId: 0 };
    var COLS = [
        { k: 'Slip', t: 'Slip', btn: true }, { k: 'Edit', t: 'Edit', btn: true },
        { k: 'DocNo', t: 'DocNo' }, { k: 'BankName', t: 'BankName' }, { k: 'ContractRefNo', t: 'ContractRefNo' }, { k: 'DealDate', t: 'DealDate' },
        { k: 'Client', t: 'Client' }, { k: 'FcyCode', t: 'FcyCode' }, { k: 'FcyAmount', t: 'FcyAmount', n: 1, sum: 1 },
        { k: 'BookingRate', t: 'BookingRate', n: 1 }, { k: 'HcyCode', t: 'HcyCode' }, { k: 'HcyAmount', t: 'HcyAmount', n: 1, sum: 1 },
        { k: 'FixedDays', t: 'FixedDays', n: 1, sum: 1 }, { k: 'FixedMaturity', t: 'FixedMaturity' }, { k: 'OptionalDays', t: 'OptionalDays', n: 1, sum: 1 },
        { k: 'StartOfOptianlPeriod', t: 'StartOfOptianlPeriod' }, { k: 'FinalMaturity', t: 'FinalMaturity' }, { k: 'TotalDays', t: 'TotalDays', n: 1, sum: 1 },
        { k: 'EntryDate', t: 'EntryDate' }, { k: 'EntryUser', t: 'EntryUser' }, { k: 'ModifyDate', t: 'ModifyDate' }, { k: 'ModifyUser', t: 'ModifyUser' },
        { k: 'NoOfAttachments', t: 'NoOfAttachments', link: true }
    ];

    function msg(t) { window.alert(t); }
    function esc(s) { return String(s == null ? '' : s).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }
    function pad2(n) { return (n < 10 ? '0' : '') + n; }
    function iso(d) { return d.getFullYear() + '-' + pad2(d.getMonth() + 1) + '-' + pad2(d.getDate()); }
    function toD(v) { var n = parseFloat(String(v == null ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function group(n, dec) { return n.toLocaleString('en-US', { minimumFractionDigits: dec, maximumFractionDigits: dec }); }
    function csrf() {
        var h = { 'Content-Type': 'application/json', 'Accept': 'application/json, application/pdf, text/plain' };
        var tk = document.querySelector('meta[name="_csrf"]'), hd = document.querySelector('meta[name="_csrf_header"]');
        if (tk && hd) h[hd.getAttribute('content')] = tk.getAttribute('content');
        return h;
    }
    function post(url, body) { return $.ajax({ url: url, method: 'POST', contentType: 'application/json', data: JSON.stringify(body), headers: csrf() }); }
    function failText(x) { return (x && x.responseJSON && x.responseJSON.message) || (x && x.statusText) || 'Error'; }
    function say(t, ok) { $('#w4Message').attr('class', ok ? 'ok' : 'err').text(t); }

    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function cval(id) { var v = $('#' + id).val(); return v == null ? '' : String(v); }
    function cset(id, v) { $('#' + id).val(v == null || v === 0 ? '' : String(v)).trigger('change'); refreshCombos(); }
    function bind(id, items, vKey, tKey) {
        var h = '<option value=""></option>';
        items.forEach(function (r) { h += '<option value="' + esc(r[vKey]) + '">' + esc(r[tKey]) + '</option>'; });
        $('#' + id).html(h);
    }
    function fillLookups(r) {
        bind('cmbBank', r.banks || [], 'Id', 'BranchName'); bind('cmbClient', r.branches || [], 'Id', 'BranchName');
        bind('cmbFcy', r.currencies || [], 'Id', 'CurrencyCode'); bind('cmbHcy', r.currencies || [], 'Id', 'CurrencyCode');
        bind('cmbHBank', r.historyBanks || [], 'Id', 'name'); bind('cmbHClient', r.historyBranches || [], 'Id', 'name');
        refreshCombos();
    }

    /* calculateHcyAmount :1146, date arithmetic and CalculateTotalDays :1270 */
    function addDays(isoDate, n) {
        if (!isoDate) return '';
        var p = isoDate.split('-'), d = new Date(Date.UTC(+p[0], +p[1] - 1, +p[2] + n));
        return d.getUTCFullYear() + '-' + pad2(d.getUTCMonth() + 1) + '-' + pad2(d.getUTCDate());
    }
    function recalc() {
        var f = toD($('#txtFcyAmount').val()), r = toD($('#txtRate').val()), m = Math.pow(10, S.fmt.amountDecimals);
        $('#txtHcyAmount').val(f > 0 && r > 0 ? group(Math.round(f * r * m) / m, S.fmt.amountDecimals) : '');
        var fx = parseInt($('#txtFixedDays').val(), 10) || 0, op = parseInt($('#txtOptionalDays').val(), 10) || 0;
        var deal = $('#datDeal').val();
        var fm = deal ? addDays(deal, fx) : '';
        $('#datFixedMaturity').val(fm); $('#datStartOptional').val(fm);
        $('#datFinalMaturity').val(fm ? addDays(fm, op) : '');
        $('#txtTotalDays').val(fx > 0 || op > 0 ? fx + op : '');
    }

    function setMode(update) {
        $('#BtnSave').toggleClass('fb-hidden', update);
        $('#btnUpdate').toggleClass('fb-hidden', !update);
    }
    function applyRights() {
        $('#BtnSave').prop('disabled', !S.rights.save); $('#btnUpdate').prop('disabled', !S.rights.update);
    }
    function formReset() {
        S.recId = 0; S.att = { existing: [], added: [], removed: [] };
        ['txtContract', 'txtFcyAmount', 'txtRate', 'txtHcyAmount', 'txtFixedDays', 'txtOptionalDays', 'txtTotalDays'].forEach(function (i) { $('#' + i).val(''); });
        ['cmbBank', 'cmbClient', 'cmbFcy', 'cmbHcy'].forEach(function (i) { cset(i, ''); });
        $('#datDeal').val(iso(new Date())); recalc(); setMode(false);
        $.getJSON(API + '/docno').done(function (r) { if (r && r.success !== false) $('#txtDocNo').val(r.docNo); });
    }

    function load(id) {
        $.getJSON(API + '/' + id).done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            S.recId = r.id; $('#txtDocNo').val(r.docNo); cset('cmbBank', r.bankId); $('#txtContract').val(r.contractRef);
            $('#datDeal').val(r.docDate); cset('cmbClient', r.branchId); cset('cmbFcy', r.fcyId);
            $('#txtFcyAmount').val(r.fcyAmount); $('#txtRate').val(r.bookingRate); cset('cmbHcy', r.hcyId);
            $('#txtFixedDays').val(r.fixedDays); $('#txtOptionalDays').val(r.optionalDays);
            recalc();
            S.att = { existing: r.attachments || [], added: [], removed: [] };
            setMode(true);
        }).fail(function (x) { msg(failText(x)); });
    }

    /* ---- save / update (Insert() :584) */
    function attachPayload() {
        return { files: S.att.added.map(function (a) { return { name: a.name, base64: a.b64 }; }), removeIds: S.att.removed };
    }
    function doSave(update) {
        if (update ? !S.rights.update : !S.rights.save) { msg('You do not have the ' + (update ? 'Update' : 'Save') + ' right for this screen.'); return; }
        if (!window.confirm(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            id: update ? S.recId : 0, docNo: $('#txtDocNo').val(), bankId: cval('cmbBank') || 0, contractRef: $('#txtContract').val(),
            docDate: $('#datDeal').val(), branchId: cval('cmbClient') || 0, fcyId: cval('cmbFcy') || 0, fcyAmount: $('#txtFcyAmount').val(),
            bookingRate: $('#txtRate').val(), hcyId: cval('cmbHcy') || 0, fixedDays: $('#txtFixedDays').val(),
            fixedMaturity: $('#datFixedMaturity').val(), optionalDays: $('#txtOptionalDays').val(), startOptional: $('#datStartOptional').val(),
            finalMaturity: $('#datFinalMaturity').val(), attachments: attachPayload()
        };
        post(API + '/save', body).done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            say(r.message, true); window.alert(r.message); formReset(); historyBind();
        }).fail(function (x) { msg(failText(x)); });
    }

    /* ---- history grid (GridFill :719) */
    function head() {
        $('#gridHist thead').html('<tr>' + COLS.map(function (c) { return '<th data-col="' + c.k + '">' + esc(c.t) + '</th>'; }).join('') + '</tr>');
    }
    function renderGrid() {
        var h = '', sums = {};
        S.rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (r.Id === S.selId ? ' class="sel"' : '') + '>';
            COLS.forEach(function (c) {
                if (c.btn) { h += '<td><a data-act="' + c.k + '">' + c.k + '</a></td>'; return; }
                var v = r[c.k];
                if (c.n) {
                    var nv = toD(v), dec = c.k === 'BookingRate' ? S.fmt.rateDecimals : (c.k.indexOf('Days') >= 0 ? 0 : S.fmt.amountDecimals);
                    if (c.sum) sums[c.k] = (sums[c.k] || 0) + nv;
                    h += '<td class="n">' + group(nv, dec) + '</td>';
                } else if (c.link) {
                    h += '<td class="n">' + (v ? '<a data-act="NoOfAttachments">' + esc(v) + '</a>' : esc(v)) + '</td>';
                } else h += '<td>' + esc(v) + '</td>';
            });
            h += '</tr>';
        });
        $('#gridHist tbody').html(h);
        var f = '<tr>' + COLS.map(function (c) {
            if (!c.sum) return '<td></td>';
            var dec = c.k.indexOf('Days') >= 0 ? 0 : S.fmt.amountDecimals;
            return '<td class="n">' + group(sums[c.k] || 0, dec) + '</td>';
        }).join('') + '</tr>';
        $('#gridHist tfoot').html(S.rows.length ? f : '');
    }
    function historyBind() {
        var b = {
            dateBy: $('input[name=dateBy]:checked').val(), fromDate: $('#chkFrom').is(':checked') ? $('#datFrom').val() : '',
            toDate: $('#chkTo').is(':checked') ? $('#datTo').val() : '', fromDocNo: $('#txtFromDocNo').val(), toDocNo: $('#txtToDocNo').val(),
            bankId: cval('cmbHBank') || 0, branchId: cval('cmbHClient') || 0
        };
        post(API + '/history', b).done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            S.rows = r.rows || []; renderGrid();
        }).fail(function (x) { msg(failText(x)); });
    }

    /* ---- print (existing slip: /reports/print/01-fcy-ex-rate-forwward-booking-slip) */
    function printSlip(id) {
        if (!id) { msg('Record Not Found'); return; }
        var w = null; try { w = window.open('', '_blank'); } catch (e) { w = null; }
        return fetch('/reports/print/01-fcy-ex-rate-forwward-booking-slip', { method: 'POST', credentials: 'same-origin', headers: csrf(), body: JSON.stringify({ id: id }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                return r.text().then(function (t) { if (w) w.close(); msg(t || 'No Record Found For Display'); });
            }).catch(function (e) { if (w) w.close(); msg(e.message); });
    }

    /* ---- attachments */
    function renderAttach() {
        var h = '';
        S.att.existing.forEach(function (a) {
            h += '<tr><td><a href="' + API + '/' + S.recId + '/attachments/' + a.Id + '">' + esc(a.Attachment) + '</a></td><td>' + (+a.SizeMb).toFixed(2) + '</td><td><a data-k="e" data-id="' + a.Id + '">remove</a></td></tr>';
        });
        S.att.added.forEach(function (a, i) {
            h += '<tr><td>' + esc(a.name) + '</td><td>' + (a.size / 1048576).toFixed(2) + '</td><td><a data-k="n" data-i="' + i + '">remove</a></td></tr>';
        });
        $('#atRows').html(h);
    }
    function readFiles(files) {
        var list = Array.prototype.slice.call(files || []);
        if (!list.length) return;
        if (list.length > 10) { msg('Select at most ten files at once'); return; }
        var todo = list.length;
        list.forEach(function (f) {
            if (f.size === 0 || f.size > 5 * 1024 * 1024) { msg('Attachment must be between 1 byte and 5 MB'); if (--todo === 0) renderAttach(); return; }
            var fr = new FileReader();
            fr.onload = function () { var s = String(fr.result); S.att.added.push({ name: f.name, size: f.size, b64: s.substring(s.indexOf(',') + 1) }); if (--todo === 0) renderAttach(); };
            fr.onerror = function () { msg('Could not read ' + f.name); if (--todo === 0) renderAttach(); };
            fr.readAsDataURL(f);
        });
    }
    function viewAttachments(id) {
        $.getJSON(API + '/' + id + '/attachments').done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            var rows = r.rows || []; if (!rows.length) return;
            $('#vwRows').html(rows.map(function (a) {
                return '<tr><td><a href="' + API + '/' + id + '/attachments/' + a.Id + '">' + esc(a.Attachment) + '</a></td><td>' + (+a.SizeMb).toFixed(2) + '</td><td>' + esc(a.EntryUserName) + '</td></tr>';
            }).join(''));
            $('#dlgView').addClass('on');
        }).fail(function (x) { msg(failText(x)); });
    }

    function init() {
        $.getJSON(API + '/init').done(function (r) {
            if (!r || r.success === false) { msg((r && r.message) || 'Error'); return; }
            S.rights = r.rights || {}; S.fmt = { amountDecimals: r.amountDecimals || 0, rateDecimals: r.rateDecimals || 2 };
            fillLookups(r); applyRights();
            $('#txtDocNo').val(r.docNo); $('#datDeal').val(iso(new Date()));
            var back = new Date(); back.setDate(back.getDate() - (r.historyDaysBack || 3));
            $('#datFrom').val(iso(back)); $('#datTo').val(iso(new Date()));
            recalc(); historyBind();
        }).fail(function (x) { msg(failText(x)); });
    }

    $(function () {
        head();
        $('#BtnNew').on('click', formReset);
        $('#btnRefresh').on('click', function () {
            $.getJSON(API + '/lookups').done(function (r) { if (r && r.success !== false) fillLookups(r); }).fail(function (x) { msg(failText(x)); });
        });
        $('#BtnSave').on('click', function () { doSave(false); });
        $('#btnUpdate').on('click', function () { doSave(true); });
        $('#BtnPrint').on('click', function () { printSlip(S.recId); });
        $('#btnattachment').on('click', function () { renderAttach(); $('#dlgAttach').addClass('on'); });
        $('#btnshortcutkeys').on('click', function () { $('#dlgKeys').addClass('on'); });
        $('#btnShow').on('click', historyBind);
        $('#txtFcyAmount, #txtRate, #txtFixedDays, #txtOptionalDays, #datDeal').on('input change', recalc);
        $('#txtFcyAmount, #txtRate').on('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.key.length > 1) return;
            if (!/[0-9.]/.test(e.key) || (e.key === '.' && this.value.indexOf('.') >= 0)) e.preventDefault();
        });
        $('#txtFixedDays, #txtOptionalDays').on('keypress', function (e) { if (!e.ctrlKey && e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        $('#atChoose').on('click', function () { $('#atFile').click(); });
        $('#atFile').on('change', function () { readFiles(this.files); this.value = ''; });
        $('#atClose, #atX').on('click', function (e) { e.preventDefault(); $('#dlgAttach').removeClass('on'); });
        $('#vwX').on('click', function (e) { e.preventDefault(); $('#dlgView').removeClass('on'); });
        $('#kyX').on('click', function (e) { e.preventDefault(); $('#dlgKeys').removeClass('on'); });
        $('#atRows').on('click', 'a[data-k]', function () {
            var $a = $(this);
            if ($a.data('k') === 'e') { var id = +$a.data('id'); S.att.existing = S.att.existing.filter(function (a) { return a.Id !== id; }); S.att.removed.push(id); }
            else S.att.added.splice(+$a.data('i'), 1);
            renderAttach();
        });
        $('#gridHist tbody').on('click', 'tr', function () { S.selId = S.rows[+$(this).data('i')].Id; $('#gridHist tbody tr').removeClass('sel'); $(this).addClass('sel'); });
        $('#gridHist tbody').on('click', 'a[data-act]', function (e) {
            e.stopPropagation(); var row = S.rows[+$(this).closest('tr').data('i')], act = $(this).data('act');
            if (act === 'Slip') printSlip(row.Id); else if (act === 'Edit') load(row.Id); else viewAttachments(row.Id);
        });
        $('#gridHist tbody').on('dblclick', 'tr', function () { load(S.rows[+$(this).data('i')].Id); });
        $(document).on('keydown', function (e) {
            if (e.key === 'Escape') { $('.fb-dlg').removeClass('on'); return; }
            if (e.key === 'F5') { e.preventDefault(); $('#cmbBank').closest('.fb-f').find('.dtcombo-input').focus(); return; }
            if (!e.ctrlKey || e.altKey) return;
            var k = e.key.toLowerCase(), map = { s: '#BtnSave', u: '#btnUpdate', e: '#btnattachment', r: '#btnRefresh', n: '#BtnNew', p: '#BtnPrint' };
            if (map[k]) { e.preventDefault(); if (!$(map[k]).hasClass('fb-hidden')) $(map[k]).click(); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); $('#gridHist tbody tr:first').click(); }
        });
        init();
    });
}(jQuery));
