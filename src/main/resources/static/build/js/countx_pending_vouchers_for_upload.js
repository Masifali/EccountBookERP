/* ==========================================================================================
 * Vouchers (Upload) - desktop Architecture.WinApp.DataSyncing.frmPendingVouchersForUpload (ScreenId 957)
 *   :120 InitializeComponentCustom   :137 InitializeComponentMethod   :178 ComboBind   :201 StatusFill
 *   :207 btnRefresh   :218 btnNew   :250 BtnShow   :431 grid link   :445 selection changed
 *   :456 VoucherDetailByHeaderId   :550 BtnUploadVouchers   :651 MakeShortCutKeys
 *   :687 form KeyDown   :733 DataGridHistory KeyDown (Ctrl+Space)
 * ==========================================================================================*/
(function () {
    'use strict';
    var C = window.StoreCommon;
    var API = '/api/accounts/pending-vouchers-for-upload';
    var PRINT_102 = '/reports/print/102-a-new-ac-payment-receipts-voucher-slip';
    var S = { multi: false, sub: false, rows: [], cur: -1, busy: false };

    function $(id) { return document.getElementById(id); }
    function esc(v) { return C.esc(v); }
    function txt(v) { return v === null || v === undefined ? '' : String(v); }
    function amt(v) { var n = parseFloat(v); return isNaN(n) ? '0.00' : n.toFixed(2); }
    function rate(v) { var n = parseFloat(v); return isNaN(n) ? '0.00' : n.toFixed(4); }
    function csrf(h) {
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }

    /* ---------------------------------------------------------------- combos (ComboBind / StatusFill) */
    function fillCombos(d) {
        C.fillSelectCols('cmbAccountTitleCpv', d.accounts, 'Id', 'Name', [], ['Account Title'], false);
        C.fillSelectCols('CmbDocumentType', d.documentTypes, 'Id', 'Name', [], ['DocumentType'], false);
        C.fillSelect('cmbApprovedStatusCpv', d.statuses, 'Id', 'Status', false);
        var st = $('cmbApprovedStatusCpv');
        if (st && !st.value && st.options.length) st.selectedIndex = 0;
    }

    function load() {
        return C.getJson(API + '/init').then(function (d) {
            S.multi = !!d.multiCurrency; S.sub = !!d.subsidiary;
            fillCombos(d);
            $('txtFromDate').value = C.isoDay(d.yearStart);
            $('txtToDate').value = C.today();
            $('chkFromDate').checked = true; $('chkToDate').checked = true;
            renderHistory([]); renderDetail([]);
        }).catch(function (e) {
            var n = $('rightsNote');
            if (n) { n.textContent = e.message; n.classList.remove('is-hidden'); }
            fail(e);
        });
    }

    /* ---------------------------------------------------------------- history grid */
    function histColumns() {
        var c = [
            { k: 'Select', t: 'Select' }, { k: 'Print', t: 'Print' }, { k: 'Print_102', t: 'Print_102' },
            { k: 'DocumentType', t: 'V.Type' }, { k: 'VoucherDate', t: 'V.Date' }, { k: 'VoucherCode', t: 'V.No' },
            { k: 'AccountTitle', t: 'AccountTitle' }, { k: 'VoucherAmount', t: 'VoucherAmount', n: 1 }
        ];
        if (S.multi) c.push({ k: 'FcyCode', t: 'FcyCode' }, { k: 'ExchangeRate', t: 'ExchangeRate', n: 1 }, { k: 'FcyAmount', t: 'FcyAmount', n: 1 });
        c.push({ k: 'Remarks', t: 'Remarks' }, { k: 'EntryUser', t: 'EntryUser' }, { k: 'EntryDate', t: 'EntryDate' },
            { k: 'ModifyUser', t: 'ModifyUser' }, { k: 'ModifyDate', t: 'ModifyDate' }, { k: 'ApprovedUser', t: 'ApprovedUser' },
            { k: 'ApprovedDate', t: 'ApprovedDate' }, { k: 'NoOfAttachments', t: 'NoOfAttachments' });
        return c;
    }
    var STICKY = { Select: 0, Print: 30, Print_102: 70, DocumentType: 140 };

    function cell(r, c, i) {
        var v = r[c.k];
        switch (c.k) {
            case 'Select': return '<input type="checkbox" class="pvu-sel" data-i="' + i + '" aria-label="Select">';
            case 'Print': return '<button type="button" class="fx-btn pvu-p" data-i="' + i + '" style="padding:0 4px">Print</button>';
            case 'Print_102': return '<button type="button" class="fx-btn pvu-p102" data-i="' + i + '" style="padding:0 4px">Print_102</button>';
            case 'VoucherDate': return esc(C.gridDate(v));
            case 'EntryDate': case 'ModifyDate': case 'ApprovedDate': return esc(v ? C.gridDateTime(v, true) : '');
            case 'VoucherAmount': case 'FcyAmount': return amt(v);
            case 'ExchangeRate': return rate(v);
            case 'NoOfAttachments': return '<a href="#" class="pvu-link pvu-att" data-i="' + i + '">' + esc(txt(v)) + '</a>';
            default: return esc(txt(v));
        }
    }

    function renderHistory(rows) {
        S.rows = rows || []; S.cur = -1;
        var cols = histColumns(), t = $('DataGridHistory');
        var h = '<tr>' + cols.map(function (c) {
            var st = STICKY[c.k] !== undefined ? ' class="fz" style="left:' + STICKY[c.k] + 'px"' : '';
            return '<th' + st + '>' + (c.k === 'Select' ? '<input type="checkbox" id="pvuAll" aria-label="Select all">' : esc(c.t)) + '</th>';
        }).join('') + '</tr>';
        t.querySelector('thead').innerHTML = h;
        t.querySelector('tbody').innerHTML = S.rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + cols.map(function (c) {
                var st = STICKY[c.k] !== undefined ? ' class="fz" style="left:' + STICKY[c.k] + 'px"' : (c.n ? ' class="num"' : '');
                return '<td tabindex="-1" data-k="' + c.k + '"' + st + '>' + cell(r, c, i) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var all = $('pvuAll');
        if (all) all.addEventListener('change', function () {
            Array.prototype.forEach.call(t.querySelectorAll('.pvu-sel'), function (b) { b.checked = all.checked; });
        });
    }

    function selectRow(i) {
        var t = $('DataGridHistory'), trs = t.querySelectorAll('tbody tr');
        Array.prototype.forEach.call(trs, function (tr) { tr.classList.remove('is-selected'); });
        if (i < 0 || i >= S.rows.length) { S.cur = -1; renderDetail([]); return; }
        S.cur = i; trs[i].classList.add('is-selected');
        var id = parseInt(S.rows[i].Id, 10) || 0;
        C.getJson(API + '/voucher/' + id).then(function (d) { if (S.cur === i) renderDetail(d.rows); }).catch(function (e) { renderDetail([]); fail(e); });
    }

    /* ---------------------------------------------------------------- detail grid (VoucherDetailByHeaderId) */
    function renderDetail(rows) {
        var cols = [['AccountCode', 'AccountCode'], ['AccountTitle', 'AccountTitle']];
        if (S.sub) cols.push(['SubsidiaryAccount', 'SubsidiaryAccount']);
        cols.push(['JobLot', 'Job/Lot'], ['Remarks', 'Remarks'], ['DebitAmount', 'Debit', 1], ['CreditAmount', 'Credit', 1]);
        if (S.multi) cols.push(['DebitFcyAmount', 'Fcy Debit', 1], ['CreditFcyAmount', 'Fcy Credit', 1]);
        cols.push(['ChequeDate', 'ChequeDate'], ['ChequeNo', 'ChequeNo']);
        var t = $('grdDetail');
        t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = (rows || []).map(function (r) {
            return '<tr>' + cols.map(function (c) {
                var v = r[c[0]];
                if (c[2]) return '<td class="num">' + amt(v) + '</td>';
                if (c[0] === 'ChequeDate') return '<td>' + esc(v ? C.gridDate(v) : '') + '</td>';
                return '<td>' + esc(txt(v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
    }

    /* ---------------------------------------------------------------- Show (BtnShow_Click :250) */
    function radio(name, fallback) {
        var r = document.querySelector('input[name="' + name + '"]:checked');
        return r ? r.value : fallback;
    }
    function show() {
        var body = {
            dateType: radio('dtype', 'doc'),
            fromDate: $('chkFromDate').checked ? $('txtFromDate').value : '',
            toDate: $('chkToDate').checked ? $('txtToDate').value : '',
            fromDocNo: $('txtFromDocNoCpv').value.trim(), toDocNo: $('txtToDocNoCpv').value.trim(),
            accountId: $('cmbAccountTitleCpv').value, documentTypeId: $('CmbDocumentType').value,
            status: ($('cmbApprovedStatusCpv').selectedOptions[0] || {}).text || '',
            actionId: radio('acct', '0')
        };
        return C.withBusy('btnShow', function () {
            return C.postJson(API + '/rows', body).then(function (d) {
                var rows = d.rows || [];
                renderHistory(rows);
                if (rows.length) {
                    $('lblTotalVouchers').textContent = d.totalVouchers;
                    $('lblApprovedVouchers').textContent = d.approvedVouchers;
                    $('lblUnApprovedVouchers').textContent = d.unApprovedVouchers;
                    selectRow(0);
                } else renderDetail([]);
            }).catch(fail);
        });
    }

    /* ---------------------------------------------------------------- New / Refresh */
    function resetNew() {
        C.getJson(API + '/init').then(function (d) {
            $('txtFromDate').value = C.isoDay(d.yearStart); $('txtToDate').value = C.today();
        }).catch(function () { /* keep the current dates */ });
        $('chkFromDate').checked = true; $('chkToDate').checked = true;
        $('txtFromDocNoCpv').value = ''; $('txtToDocNoCpv').value = '';
        var a = $('cmbAccountTitleCpv'), d = $('CmbDocumentType'), st = $('cmbApprovedStatusCpv');
        if (a.options.length) a.selectedIndex = 0; if (d.options.length) d.selectedIndex = 0; if (st.options.length) st.selectedIndex = 0;
        $('drdocdate').checked = true; $('radAll').checked = true;
        $('VouchersInfoBoxCash').style.display = 'none';
        renderHistory([]); renderDetail([]);
        $('txtFromDate').focus();
    }
    function refresh() {
        return C.withBusy('btnRefresh', function () { return C.getJson(API + '/refresh').then(fillCombos).catch(fail); });
    }

    /* ---------------------------------------------------------------- Upload (BtnUploadVouchers_Click :550) */
    function upload() {
        var picked = [];
        Array.prototype.forEach.call(document.querySelectorAll('#DataGridHistory .pvu-sel'), function (b) {
            if (b.checked) { var r = S.rows[parseInt(b.getAttribute('data-i'), 10)]; picked.push({ Id: r.Id, VoucherCode: r.VoucherCode, DocumentTypeId: r.DocumentTypeId }); }
        });
        if (!picked.length) { alert('Check any row first'); return Promise.resolve(); }
        if (!confirm('Are you sure to Upload Selected vouchers?')) return Promise.resolve();
        return C.withBusy('BtnUploadVouchers', function () {
            return C.postJson(API + '/upload', { rows: picked }).then(function () {
                alert('Record Uploaded Successfully');
                return show();
            }).catch(fail);
        });
    }

    /* ---------------------------------------------------------------- print / attachments */
    function print102(id) {
        if (!(id > 0)) { alert('VoucherId Not Found'); return; }
        var w = window.open('about:blank', '_blank');
        fetch(PRINT_102, { method: 'POST', credentials: 'same-origin',
            headers: csrf({ 'Content-Type': 'application/json', 'Accept': 'application/pdf' }), body: JSON.stringify({ id: id }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { if (w) w.location = URL.createObjectURL(b); });
                return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); fail(e); });
    }
    function printAcc102(r) { window.CrystalPrint.open('acc-102', { id: parseInt(r.Id, 10) || 0, documentTypeId: parseInt(r.DocumentTypeId, 10) || 0 }); }
    function attachments(r) {
        var id = parseInt(r.Id, 10) || 0, dt = parseInt(r.DocumentTypeId, 10) || 0;
        C.getJson(API + '/attachments' + C.qs({ id: id, documentTypeId: dt })).then(function (list) {
            var b = $('tblAtt').querySelector('tbody');
            b.innerHTML = (list || []).map(function (a) {
                var name = a.AttachmentName || a.CustomName || '';
                return '<tr><td><a class="pvu-link" href="' + API + '/attachments/' + a.Id + C.qs({ id: id, documentTypeId: dt }) + '" target="_blank">' + esc(name) + '</a></td><td>' + esc(a.EntryDate ? C.gridDateTime(a.EntryDate, true) : '') + '</td></tr>';
            }).join('') || '<tr><td colspan="2">No attachments</td></tr>';
            C.openModal('dlgAtt');
        }).catch(fail);
    }
    function onGridKey(r, key) {
        if (key === 'NoOfAttachments') attachments(r);
        if (key === 'Print') print102(parseInt(r.Id, 10) || 0);
        if (key === 'Print_102') printAcc102(r);
    }

    /* ---------------------------------------------------------------- shortcut popup (MakeShortCutKeys :651) */
    function shortcuts() {
        var rows = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'],
            ['Ctrl+S', 'For Search'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $('tblKeys').querySelector('tbody').innerHTML = rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        C.openModal('dlgKeys');
    }
    function closeForm() { window.location.href = '/modules'; }

    /* ---------------------------------------------------------------- events */
    function wire() {
        $('btnShow').addEventListener('click', show);
        $('btnRefresh').addEventListener('click', refresh);
        $('btnNew').addEventListener('click', resetNew);
        $('btnShortcutKeys').addEventListener('click', shortcuts);
        $('BtnUploadVouchers').addEventListener('click', upload);

        var t = $('DataGridHistory');
        t.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            var i = parseInt(tr.getAttribute('data-i'), 10), r = S.rows[i];
            if (e.target.classList.contains('pvu-p')) { print102(parseInt(r.Id, 10) || 0); }
            else if (e.target.classList.contains('pvu-p102')) { printAcc102(r); }
            else if (e.target.classList.contains('pvu-att')) { e.preventDefault(); attachments(r); }
            if (i !== S.cur) selectRow(i);
        });
        t.addEventListener('keydown', function (e) {
            if (!(e.ctrlKey && (e.key === ' ' || e.code === 'Space'))) return;
            var tr = e.target.closest('tbody tr'), td = e.target.closest('td');
            if (!tr || !td) return;
            e.preventDefault();
            onGridKey(S.rows[parseInt(tr.getAttribute('data-i'), 10)], td.getAttribute('data-k'));
        });

        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) {
                var a = document.activeElement;
                if (a && (a.tagName === 'INPUT' && a.type !== 'checkbox' && a.type !== 'radio' || a.tagName === 'SELECT')) {
                    e.preventDefault();
                    var f = Array.prototype.filter.call(document.querySelectorAll('input,select,button'), function (x) { return !x.disabled && x.offsetParent !== null && x.tabIndex >= 0; });
                    var n = f[f.indexOf(a) + 1]; if (n) n.focus();
                }
            } else if ((e.ctrlKey && k === 'e') || e.key === 'Escape') {
                var open = document.querySelector('.cx-modal.is-open');
                if (open) { open.classList.remove('is-open'); return; }
                e.preventDefault(); closeForm();
            } else if (e.ctrlKey && e.altKey) { shortcuts(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); upload(); }
            else if (e.ctrlKey && (k === 'n' || k === 'r')) { e.preventDefault(); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); $('txtFromDate').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') {
                e.preventDefault();
                var c = t.querySelector('tbody td'); if (c) { c.tabIndex = 0; c.focus(); }
            }
        });
    }

    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
})();
