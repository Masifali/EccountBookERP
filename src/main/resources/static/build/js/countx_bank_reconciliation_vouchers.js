/* ============================================================================================
 * Screen 885 "Bank Reconciliation With Vouchers" —
 * AdjustmentVouchers.frmBankReconciliationWithVouchers (DocumentTypeId 920).
 * Detail grid = uploaded bank-statement rows not yet reconciled (tick the ones that match);
 * Pending grid = the bank account's ledger (Load picks the voucher they are matched to).
 * History's Voucher From / Voucher To boxes exist on the desktop but its history call never
 * sends them; they are kept and ignored the same way.
 * Not ported: attachments.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/accounts/api/bank-reconciliation-vouchers';
    var $ = function (id) { return document.getElementById(id); };
    var S = { rights: {}, pending: [], rows: [], recId: 0, approved: false, voucher: null, defaultDays: 0, yearStart: '', history: [] };

    function toInt(v) { var n = parseInt(String(v === undefined || v === null ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(r, c) { return GD.col(r, c); }
    function val(id) { return $(id) ? $(id).value : ''; }
    function fail(e) { alert(e && e.message ? e.message : e); }
    function ask(m) { return confirm(m + '?'); }      // FormHelper.ConfirmAction appends "?"

    function load() {
        GD.get(API + '/init').then(function (d) {
            S.rights = d.rights || {};
            S.defaultDays = d.defaultDaysToLessFromHistoryFromDate || 0;
            S.yearStart = d.financialYearStart || '';
            GD.setDecimals(d.amountDecimals, 2);
            $('btnSave').disabled = !S.rights.canSave;
            $('btnUpdate').disabled = !S.rights.canUpdate;
            $('btnDelete').disabled = !S.rights.canDelete;
            $('btnSlip').disabled = !S.rights.canPrint;
            $('ChkPrintslip').checked = !!S.rights.canPrint;
            fillBanks(d.bankAccounts);
            fillHistoryBanks(d.historyBanks);
            var from = new Date(); from.setDate(from.getDate() - (S.defaultDays > 0 ? S.defaultDays : 3));
            $('txtFromdateHistory').value = GD.iso(from);
            $('txtToDateHistory').value = GD.iso(new Date());
            render();
            $('CmbSupplier').focus();
        }).catch(function () { alert('Error occurred during database call.'); });
    }

    function fillBanks(rows) {
        var keep = val('CmbSupplier');
        // insertDefaultRow: true -> value 0
        GD.fill('CmbSupplier', rows, 'Id', 'AccountTitle', { code: 'AccountCode', parent: 'ParentAccountTitle', 'class': 'AccountClass' }, null);
        $('CmbSupplier').insertAdjacentHTML('afterbegin', '<option value="0"></option>');
        $('CmbSupplier').value = keep || '0';
    }

    function fillHistoryBanks(rows) {
        var keep = val('CmbCustomerHistory');
        GD.fill('CmbCustomerHistory', rows, 'Id', 'name', null, '');
        $('CmbCustomerHistory').value = keep;
    }

    // ===================================================================== btnShowRecords_Click
    function show() {
        if ($('btnSave').hidden || $('btnSave').disabled) { alert('Please Reset the Form First...'); return Promise.resolve(); }
        var id = toInt(val('CmbSupplier'));
        if (id <= 0) { alert('Please Select a Bank Account to Show Records'); $('CmbSupplier').focus(); return Promise.resolve(); }
        if ((S.rows.length > 0 || S.pending.length > 0) && !ask('Record Already Exists,Do you want to Refresh Previous And Load Again')) return Promise.resolve();
        reset();
        return GD.get(API + '/show', { bankAccountId: id }).then(function (d) {
            S.pending = (d.pending || []).map(function (r) {
                return { RecNo: col(r, 'RecNo'), Id: col(r, 'Id'), DetailId: col(r, 'DetailId'), DocumentTypeId: col(r, 'DocumentTypeId'),
                    DocumentType: col(r, 'DocumentTypeDescription'), VoucherDate: col(r, 'VoucherDate'), VoucherCode: col(r, 'VoucherCode'),
                    ChequeNo: col(r, 'ChequeNo'), ChequeDate: col(r, 'ChequeDate'), Remarks: col(r, 'Remarks'),
                    DebitAmount: col(r, 'DebitAmount'), CreditAmount: Math.abs(GD.num(col(r, 'CreditAmount'))), RunningBalance: col(r, 'RunningBalance') };
            });
            S.rows = (d.rows || []).map(rowOf);
            render();
        }).catch(function (e) { alert('Error while loading pending vouchers: ' + (e && e.message)); });
    }

    function rowOf(r) {
        return { Id: col(r, 'Id'), DocumentTypeId: col(r, 'DocumentTypeId'), BankAccountId: col(r, 'BankAccountId'),
            TransactionDate: col(r, 'TransactionDate'), ChequeNo: col(r, 'ChequeNo'), Particulars: col(r, 'Particulars'),
            Debit: col(r, 'Debit'), Credit: col(r, 'Credit'), Remarks: col(r, 'Remarks'), checked: false };
    }

    function render() {
        // grdSetting: Select | TransactionDate | ChequeNo | Particulars | Debit | Credit | Remarks
        var h = '<thead><tr><th><input type="checkbox" onclick="BR.checkAll(this.checked)"></th><th>TransactionDate</th><th>ChequeNo</th>'
            + '<th>Particulars</th><th>Debit</th><th>Credit</th><th>Remarks</th></tr></thead><tbody>';
        var td = 0, tc = 0;
        S.rows.forEach(function (r, i) {
            td += GD.num(r.Debit); tc += GD.num(r.Credit);
            h += '<tr><td><input type="checkbox" ' + (r.checked ? 'checked ' : '') + 'onchange="BR.check(' + i + ',this.checked)"></td>'
                + '<td>' + GD.fmtDate(r.TransactionDate, 'dd-MMM-yy') + '</td><td>' + GD.esc(r.ChequeNo) + '</td>'
                + '<td>' + GD.esc(r.Particulars) + '</td><td class="num">' + GD.fmtSingle(r.Debit) + '</td>'
                + '<td class="num">' + GD.fmtSingle(r.Credit) + '</td><td>' + GD.esc(r.Remarks) + '</td></tr>';
        });
        h += '</tbody><tfoot><tr><td colspan="4"></td><td class="num">' + GD.fmtSingle(td) + '</td><td class="num">' + GD.fmtSingle(tc) + '</td><td></td></tr></tfoot>';
        $('grd').innerHTML = h;
        // grdPendingVouchersSetting: Load | V.Type | VoucherDate | VoucherCode | ChequeNo | ChequeDate | Remarks | DebitAmount | CreditAmount
        var p = '<thead><tr><th>Load</th><th>V.Type</th><th>VoucherDate</th><th>VoucherCode</th><th>ChequeNo</th><th>ChequeDate</th>'
            + '<th>Remarks</th><th>DebitAmount</th><th>CreditAmount</th></tr></thead><tbody>';
        var pd = 0, pc = 0;
        S.pending.forEach(function (r, i) {
            pd += GD.num(r.DebitAmount); pc += GD.num(r.CreditAmount);
            p += '<tr><td><button type="button" onclick="BR.loadPending(' + i + ')">Load</button></td><td>' + GD.esc(r.DocumentType) + '</td>'
                + '<td>' + GD.fmtDate(r.VoucherDate, 'dd-MMM-yy') + '</td><td>' + GD.esc(r.VoucherCode) + '</td><td>' + GD.esc(r.ChequeNo) + '</td>'
                + '<td>' + GD.fmtDate(r.ChequeDate, 'dd-MMM-yy') + '</td><td>' + GD.esc(r.Remarks) + '</td>'
                + '<td class="num">' + GD.fmtSingle(r.DebitAmount) + '</td><td class="num">' + GD.fmtSingle(r.CreditAmount) + '</td></tr>';
        });
        p += '</tbody><tfoot><tr><td colspan="7"></td><td class="num">' + GD.fmtSingle(pd) + '</td><td class="num">' + GD.fmtSingle(pc) + '</td></tr></tfoot>';
        $('grdPendingVouchers').innerHTML = p;
    }

    /** grdPendingOrders_LoadClick (:455) */
    function loadPending(i) {
        var r = S.pending[i]; if (!r) return;
        if ($('btnSave').hidden || $('btnSave').disabled) { alert('Please Reset the Form First to Load New Data'); return; }
        if (toInt(r.DocumentTypeId) === 0) { alert('Please Select Valid row to Load Data'); return; }
        if (toInt(val('CmbVoucher')) > 0 && !ask('Voucher Is Already Selected,Do you Want To change??')) return;
        S.voucher = { Id: toInt(r.Id), VoucherNo: r.VoucherCode, DetailId: toInt(r.DetailId) };
        $('CmbVoucher').innerHTML = '<option value="' + S.voucher.Id + '">' + GD.esc(S.voucher.VoucherNo) + '</option>';
        $('txtVoucherDate').value = GD.iso(r.VoucherDate);
        $('txtVoucherAmountCr').value = GD.fmt3(Math.abs(GD.num(r.CreditAmount)));
        $('txtVoucherAmountDr').value = GD.fmt3(Math.abs(GD.num(r.DebitAmount)));
    }

    function reset() {
        S.recId = 0; S.approved = false;
        $('btnSave').hidden = false; $('btnUpdate').hidden = true; $('btnDelete').hidden = true;
        $('CmbSupplier').disabled = false;
        S.voucher = null; $('CmbVoucher').innerHTML = '';
        $('txtVoucherAmountCr').value = ''; $('txtVoucherAmountDr').value = '';
        S.rows = []; S.pending = [];
        render();
        $('CmbSupplier').focus();
    }

    function refresh() {
        GD.get(API + '/bank-accounts').then(fillBanks).catch(fail);
    }

    // ================================================================== Insert() (:701)
    function request(rows) {
        return {
            recId: S.recId, bankAccountId: toInt(val('CmbSupplier')), voucherHeadId: toInt(val('CmbVoucher')),
            voucherDetailId: S.voucher ? S.voucher.DetailId : 0,
            voucherNo: S.voucher ? String(S.voucher.VoucherNo) : '',
            voucherAmountDr: val('txtVoucherAmountDr'), voucherAmountCr: val('txtVoucherAmountCr'),
            gridRowCount: S.rows.length,
            rows: rows.map(function (r) { return { Id: toInt(r.Id), Debit: GD.num(r.Debit), Credit: GD.num(r.Credit) }; })
        };
    }

    function save(isUpdate) {
        if (isUpdate) {
            if (S.recId === 0) { alert('Record Not found'); return; }
            if (S.approved) { alert('Record Not Update because Record has approved'); return; }
        } else S.recId = 0;
        if (S.rows.length === 0) { alert('Grid Record Not Found'); return; }
        var checked = S.rows.filter(function (r) { return r.checked; });
        if (checked.length === 0) { alert('Please check Any Record'); return; }
        if (toInt(val('CmbSupplier')) === 0) { alert('Bank Account field is required'); return; }
        if (toInt(val('CmbVoucher')) === 0) { alert('Voucher No field is required'); return; }
        if (!ask(S.recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
        var req = request(checked);
        GD.api('POST', API + '/save', req).then(function (res) {
            alert(res.message);
            reset();
            if ($('ChkPrintslip').checked) generateSlip(req.bankAccountId, req.voucherHeadId);
            return show();
        }).catch(function (e) { alert(e && e.message); });
    }

    /** btnDelete_Click (:828) — every grid row's Id, StatusId 2. */
    function del() {
        if (S.recId === 0) { alert('Record Id not found for deletion...'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        GD.api('POST', API + '/delete', request(S.rows)).then(function (res) { alert(res.message); reset(); }).catch(fail);
    }

    // ================================================================== History
    function tab(i) {
        $('tabForm').classList.toggle('on', i === 0); $('tabHistory').classList.toggle('on', i === 1);
        $('pageForm').hidden = i !== 0; $('pageHistory').hidden = i !== 1;
    }

    function dateTypeChanged() {
        var v = toInt(val('cmbDateTypeHistory')), now = new Date();
        if (v === 1) $('txtFromdateHistory').value = GD.iso(now);
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); $('txtFromdateHistory').value = GD.iso(w); }
        else if (v === 3) { $('txtFromdateHistory').value = GD.iso(new Date(now.getUTCFullYear(), now.getUTCMonth(), 1)); $('txtToDateHistory').value = GD.iso(now); }
        else if (v === 4) { $('txtFromdateHistory').value = GD.iso(new Date(now.getFullYear(), 0, 1)); $('txtToDateHistory').value = GD.iso(now); }
        else if (v === 5) { $('txtFromdateHistory').value = GD.iso(S.yearStart) || '1900-01-01'; }
    }

    function resetHistory() {
        $('cmbDateTypeHistory').value = '3'; dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        $('CmbCustomerHistory').value = '';
    }

    function historyBanks() { GD.get(API + '/history-banks').then(fillHistoryBanks).catch(fail); }

    /** FillHistory (:1046) */
    function history() {
        var dt = (document.querySelector('input[name=histDate]:checked') || {}).value || 'doc';
        var p = { dateType: dt, bankAccountId: toInt(val('CmbCustomerHistory')) };
        if ($('chkFrom').checked) p.from = val('txtFromdateHistory');
        if ($('chkTo').checked) p.to = val('txtToDateHistory');
        GD.get(API + '/history', p).then(function (rows) {
            S.history = rows || [];
            if (S.history.length === 0) { $('grdHistory').innerHTML = ''; return; }
            var h = '<thead><tr><th>Edit</th><th>Print</th><th>VoucherType</th><th>VoucherNo</th><th>VoucherDate</th><th>AccountTitle</th>'
                + '<th>AccountCode</th><th>TransactionDate</th><th>ChequeNo</th><th>Particulars</th><th>Debit</th><th>Credit</th>'
                + '<th>IsReconciled</th><th>Remarks</th><th>EntryDate</th><th>EntryUser</th><th>ModifyDate</th><th>ModifyUser</th>'
                + '<th>IsApproved</th><th>ApprovedDate</th><th>ApprovedUser</th></tr></thead><tbody>';
            var td = 0, tc = 0;
            S.history.forEach(function (r, i) {
                td += GD.num(col(r, 'Debit')); tc += GD.num(col(r, 'Credit'));
                h += '<tr ondblclick="BR.edit(' + i + ')"><td><button type="button" onclick="BR.edit(' + i + ')">Edit</button></td>'
                    + '<td><button type="button" onclick="BR.printRow(' + i + ')">Print</button></td>'
                    + '<td>' + GD.esc(col(r, 'VoucherDocumentType')) + '</td><td>' + GD.esc(col(r, 'VoucherNo')) + '</td>'
                    + '<td>' + GD.fmtDate(col(r, 'VoucherDate'), 'dd-MMM-yy') + '</td><td>' + GD.esc(col(r, 'AccountTitle')) + '</td>'
                    + '<td>' + GD.esc(col(r, 'AccountCode')) + '</td><td>' + GD.fmtDate(col(r, 'TransactionDate'), 'dd-MMM-yy') + '</td>'
                    + '<td>' + GD.esc(col(r, 'ChequeNo')) + '</td><td>' + GD.esc(col(r, 'Particulars')) + '</td>'
                    + '<td class="num">' + GD.fmtSingle(col(r, 'Debit')) + '</td><td class="num">' + GD.fmtSingle(col(r, 'Credit')) + '</td>'
                    + '<td><input type="checkbox" disabled ' + (GD.num(col(r, 'IsReconciled')) || col(r, 'IsReconciled') === true ? 'checked' : '') + '></td>'
                    + '<td>' + GD.esc(col(r, 'Remarks')) + '</td>'
                    + '<td>' + GD.fmtDate(col(r, 'EntryDate'), 'dd-MM-yyyy hh:mm tt') + '</td><td>' + GD.esc(col(r, 'EntryUser')) + '</td>'
                    + '<td>' + GD.fmtDate(col(r, 'ModifyDate'), 'dd-MM-yyyy hh:mm tt') + '</td><td>' + GD.esc(col(r, 'ModifyUser')) + '</td>'
                    + '<td><input type="checkbox" disabled ' + (GD.num(col(r, 'IsApproved')) || col(r, 'IsApproved') === true ? 'checked' : '') + '></td>'
                    + '<td>' + GD.fmtDate(col(r, 'ApprovedDate'), 'dd-MM-yyyy hh:mm tt') + '</td><td>' + GD.esc(col(r, 'ApprovedUser')) + '</td></tr>';
            });
            h += '</tbody><tfoot><tr><td colspan="10"></td><td class="num">' + GD.fmtSingle(td) + '</td><td class="num">' + GD.fmtSingle(tc) + '</td><td colspan="9"></td></tr></tfoot>';
            $('grdHistory').innerHTML = h;
        }).catch(fail);
    }

    function edit(i) {
        var r = S.history[i]; if (!r) return;
        readById(toInt(col(r, 'BankAccountId')), toInt(col(r, 'VoucherHeadId')));
    }

    function printRow(i) {
        var r = S.history[i]; if (!r) return;
        generateSlip(toInt(col(r, 'BankAccountId')), toInt(col(r, 'VoucherHeadId')));
    }

    /** ReadById (:789) */
    function readById(bankAccountId, voucherHeadId) {
        reset();
        S.recId = voucherHeadId;
        GD.get(API + '/read', { bankAccountId: bankAccountId, voucherHeadId: voucherHeadId }).then(function (rows) {
            if (!rows || rows.length === 0) return;
            tab(0);
            $('btnSave').hidden = true; $('btnUpdate').hidden = false; $('btnDelete').hidden = false;
            var dr = rows.filter(function (x) { return toInt(col(x, 'VoucherHeadId')) === voucherHeadId; })[0];
            if (!dr) { alert('Object reference not set to an instance of an object.'); return; }
            $('CmbSupplier').disabled = true;
            $('CmbSupplier').value = String(col(dr, 'BankAccountId'));
            S.approved = !!(GD.num(col(dr, 'IsApproved')) || col(dr, 'IsApproved') === true);
            S.voucher = { Id: toInt(col(dr, 'VoucherHeadId')), VoucherNo: col(dr, 'VoucherCode'), DetailId: toInt(col(dr, 'VoucherDetailId')) };
            $('CmbVoucher').innerHTML = '<option value="' + S.voucher.Id + '">' + GD.esc(S.voucher.VoucherNo) + '</option>';
            $('txtVoucherDate').value = GD.iso(col(dr, 'VoucherDate'));
            $('txtVoucherAmountCr').value = String(col(dr, 'VoucherAmountCr') === null ? '' : col(dr, 'VoucherAmountCr'));
            $('txtVoucherAmountDr').value = String(col(dr, 'VoucherAmountDr') === null ? '' : col(dr, 'VoucherAmountDr'));
            S.rows = rows.map(rowOf);
            render();
        }).catch(fail);
    }

    // ================================================================== prints
    function generateSlip(bankAccountId, voucherHeadId) {
        GD.get(API + '/slip', { bankAccountId: bankAccountId, voucherHeadId: voucherHeadId }).then(function (rows) {
            if (!rows || rows.length === 0) { alert('Not Record Found For Display'); return; }
            GD.printRows('920_BankReconciliation_Slip', rows);
        }).catch(fail);
    }

    function slipClick() { generateSlip(toInt(val('CmbSupplier')), toInt(val('CmbVoucher'))); }

    function shortcuts() {
        alert(['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh',
            'Ctrl+N  For New', 'Ctrl+P  For Print Slip 920', 'Alt+1  For Print Slip 920', 'Ctrl+F5  For Focus on DocDate',
            'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form',
            'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid',
            'Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowRight  For Change Focus from one Grid To another Grid',
            "Ctrl+Space  When Focus On Any Grid To Call Function's On Button Or Link"].join('\n'));
    }

    document.addEventListener('keydown', function (e) {
        var onForm = !$('pageForm').hidden;
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (e.ctrlKey && e.key.toLowerCase() === 't') { e.preventDefault(); tab(onForm ? 1 : 0); return; }
        if (!onForm) return;
        if (e.altKey && e.key === '1') { e.preventDefault(); slipClick(); return; }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (k === 's') { e.preventDefault(); if (!$('btnSave').hidden && !$('btnSave').disabled) save(false); }
        else if (k === 'u') { e.preventDefault(); if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) save(true); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'p') { e.preventDefault(); slipClick(); }
        else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); $('CmbSupplier').focus(); }
    });

    window.BR = {
        tab: tab, newForm: reset, refresh: refresh, save: save, del: del, show: show, loadPending: loadPending,
        check: function (i, on) { if (S.rows[i]) S.rows[i].checked = on; },
        checkAll: function (on) { S.rows.forEach(function (r) { r.checked = on; }); render(); },
        slipClick: slipClick, shortcuts: shortcuts, dateTypeChanged: dateTypeChanged, resetHistory: resetHistory,
        historyBanks: historyBanks, history: history, edit: edit, printRow: printRow
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
}());
