/* ============================================================================================
 * Screen 15 "Day Book" — Architecture.WinApp.Account_Definition.DayBook (ScreenName DayBook),
 * DocumentTypeId 9. Every handler below names the desktop method it reproduces.
 *
 * Desktop behaviour kept as it is (not corrected):
 *  - "+" leaves the account (and, on the Receipt side, the Page No) as they were; only Update
 *    clears them.
 *  - Leaving the Credit Account with nothing chosen clears the DEBIT balance caption
 *    (CmbCreditAccount_Leave :891).
 *  - Cancel does not reset the row being edited, and does not clear the Payment side's Cheq/Ref.
 *  - New keeps the cash account, date, remarks and page numbers.
 *  - History lists DocumentTypeId 8 AND 9 (the procedure reads IN (8,9)), cash lines included,
 *    grouped by Cash Account; only the Receipt grid shows group totals.
 *  - The history toolbar's 144-Print sends no date (the date combo's Value is a row number).
 * Not ported: attachments.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/accounts/api/day-book';
    var $ = function (id) { return document.getElementById(id); };

    var S = {
        rights: {}, subsidiaryFeature: false, subAll: [], detailAccounts: [], cashAccounts: [],
        recId: 0, loadedDate: null,
        cr: [], dr: [], upd: { cr: -1, dr: -1 }, subLen: { cr: 0, dr: 0 }, afterCloseUp: { cr: false, dr: false }
    };

    var SIDE = {
        cr: { acc: 'CmbCreditAccount', sub: 'CmbSubsidiaryAccountReceipt', amt: 'txtCreditAmount', page: 'txtCrPageNo',
              cdate: 'datCrChequeDate', ref: 'txtCrCheqRef', rem: 'txtCrRemarks', add: 'btnAddCrditRow',
              upd: 'btnUpdateCreditRow', cancel: 'btnCreditRowCancel', bal: 'lblCreditAccountBalance',
              cap: 'lblCreditAccountBalanceCaption', grid: 'ReceiptGrid', titleCol: 'CreditAccount' },
        dr: { acc: 'CmbDebitAccount', sub: 'CmbSubsidiaryAccountPayment', amt: 'txtDebitAmount', page: 'txtDrPageNo',
              cdate: 'datDrCheqDate', ref: 'txtDrCheqRef', rem: 'txtDrRemarks', add: 'btnAddDebitRow',
              upd: 'btnUpdateDebitRow', cancel: 'btnDebitRowCancel', bal: 'lblDebitAccountBalance',
              cap: 'lblDebitAccountBalanceCaption', grid: 'PaymentGrid', titleCol: 'DebitAccount' }
    };

    function val(id) { var e = $(id); return e ? e.value : ''; }
    function intOf(v) { var n = parseInt(String(v || '').replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function selText(id) { var e = $(id); return (e && e.selectedIndex >= 0 && e.value !== '') ? e.options[e.selectedIndex].text : ''; }
    function today() { return GD.iso(new Date()); }
    function fail(e) { alert(e && e.message ? e.message : e); }

    function fillAccounts(sel, rows, blank) {
        GD.fill(sel, rows, 'Id', 'AccountTitle', { code: 'AccountCode', parent: 'ParentAccountTitle', 'class': 'AccountClass' }, blank);
    }

    function fillSub(side, rows) {
        GD.fill(SIDE[side].sub, rows, 'Id', 'SubsidiaryAccount',
            { type: 'SubsidiaryTypeId', gl: 'AccountId', code: 'Code', account: 'AccountTitle' }, '');
    }

    // ================================================================= DayBook_Load
    function load() {
        GD.get(API + '/cash/init').then(function (d) {
            S.rights = d.rights || {};
            GD.setDecimals(d.amountDecimals, d.rateDecimals);
            S.subsidiaryFeature = !!d.subsidiaryFeature;
            S.subAll = d.subsidiaryAccounts || [];
            S.cashAccounts = d.cashAccounts || [];
            S.detailAccounts = d.detailAccounts || [];
            $('btnSave').disabled = !S.rights.canSave;
            $('btnUpdate').disabled = !S.rights.canUpdate;
            Array.prototype.forEach.call(document.querySelectorAll('.sub'), function (e) { e.hidden = !S.subsidiaryFeature; });
            $('txtDocNo').value = d.voucherCode || '';
            $('txtDocDate').value = today();
            $('datCrChequeDate').value = today();
            $('datDrCheqDate').value = today();
            fillAccounts('cmbCashAccount', S.cashAccounts, '');
            fillAccounts('CmbCashAccountForHistory', S.cashAccounts, '');
            fillAccounts('CmbCreditAccount', S.detailAccounts, '');
            fillAccounts('CmbDebitAccount', S.detailAccounts, '');
            if (S.subsidiaryFeature) { bindSub('cr'); bindSub('dr'); }
            fillVoucherDates(d.voucherDates);
            renderGrid('cr'); renderGrid('dr');
            $('cmbCashAccount').focus();
        }).catch(fail);
    }

    function fillVoucherDates(rows) {
        var sel = $('datVouchderDateForHistory');
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var v = GD.col(r, 'VoucherDate');
            h += '<option value="' + GD.esc(GD.iso(v)) + '">' + GD.esc(GD.fmtDate(v, 'dd-MMM-yyyy')) + '</option>';
        });
        sel.innerHTML = h;
    }

    // ============================================== BindSubsidiaryAccountReceipt / Payment
    function bindSub(side) {
        var d = SIDE[side];
        if (S.subAll.length > 0) {
            var acc = intOf(val(d.acc));
            if (val(d.acc) !== '') {
                if (acc !== 0) {
                    var match = S.subAll.filter(function (r) { return intOf(GD.col(r, 'AccountId')) === acc; });
                    var keep = val(d.sub) !== '' ? intOf(val(d.sub)) : 0;
                    if (match.length > 0) {
                        S.subLen[side] = match.length;
                        fillSub(side, match);
                        $(d.sub).value = '';
                        if (match.some(function (r) { return intOf(GD.col(r, 'Id')) === keep; })) $(d.sub).value = String(keep);
                    } else { fillSub(side, []); S.subLen[side] = 0; }
                } else { fillSub(side, []); S.subLen[side] = 0; }
            } else {
                S.subLen[side] = S.subAll.length;
                fillSub(side, S.subAll);
            }
        } else { fillSub(side, []); S.subLen[side] = 0; }
    }

    // ===================================================== CmbCreditAccount_Leave / CmbDebitAccount_Leave
    function detailLeave(side) {
        var d = SIDE[side];
        var p = Promise.resolve();
        if (val(d.acc) !== '') {
            if (intOf(val(d.acc)) === intOf(val('cmbCashAccount'))) {
                $(d.acc).value = '';
                $(d.acc).focus();
                alert("Cash Account And Detail Account Can't be Same! Please Select Other Account");
                return Promise.resolve();
            }
            p = GD.get(API + '/balance', { accountId: val(d.acc), date: val('txtDocDate') || today() }).then(function (r) {
                var b = GD.num(r.balance);
                $(d.bal).textContent = GD.fmtBoth(b);
                $(d.cap).textContent = GD.drCr(b);
            }).catch(fail);
        } else {
            $(d.bal).textContent = '0';
            $('lblDebitAccountBalanceCaption').textContent = '';     // :891 / :991 — always the Debit caption
        }
        bindSub(side);
        return p;
    }

    // ======================== CmbSubsidiaryAccount*_Leave / _AfterCloseUp: the GL behind the pick
    function subLeave(side) {
        var gl = intOf(GD.selData(SIDE[side].sub, 'gl'));
        if (val(SIDE[side].sub) !== '' && gl !== 0) $(SIDE[side].acc).value = String(gl);
    }

    // ======================================== FormValidationCreditDetail / FormValidationDebitDetail
    function validateDetail(side) {
        var d = SIDE[side];
        var cr = side === 'cr';
        if (val(d.acc) === '') { alert(cr ? 'Credit Account Field Required' : 'Debit Account Field is Required'); $(d.acc).focus(); return false; }
        if (S.subsidiaryFeature && S.subLen[side] > 0 && val(d.sub) === '') { alert('Subsidiary Account Field is Required'); $(d.sub).focus(); return false; }
        if (GD.num(val(d.amt)) === 0) { alert(cr ? 'Amount Field Is Required' : 'Amount Field is Required'); $(d.amt).focus(); return false; }
        if (val(d.rem) === '') { alert(cr ? 'Remarks Field Is Required' : 'Remarks Field is Required'); $(d.rem).focus(); return false; }
        return true;
    }

    function subsidiaryOf(side) {
        var d = SIDE[side];
        var subVisible = S.subsidiaryFeature;
        return {
            id: !S.subsidiaryFeature ? 0 : (S.subLen[side] > 0 ? intOf(val(d.sub)) : intOf(val(d.acc))),
            text: !S.subsidiaryFeature ? '' : (S.subLen[side] > 0 ? selText(d.sub) : selText(d.acc)),
            type: !subVisible ? 0 : (S.subLen[side] > 0 ? intOf(GD.selData(d.sub, 'type')) : 4)
        };
    }

    // ============================================== btnAddCrditRow_Click / btnAddDebitRow_Click
    function addRow(side) {
        if (!validateDetail(side)) return;
        var d = SIDE[side], rows = S[side];
        var lineId = 1;
        if (rows.length > 0) lineId = Math.max.apply(null, rows.map(function (r) { return intOf(r.LineId); })) + 1;
        var sub = subsidiaryOf(side);
        rows.push({
            AccountId: intOf(val(d.acc)), Title: selText(d.acc),
            SubsidiaryAccountId: sub.id, SubsidiaryAccount: sub.text, SubsidiaryAccountTypeId: sub.type,
            Amount: GD.num(val(d.amt)), PageNo: intOf(val(d.page)), ChequeDate: val(d.cdate),
            ChequeRef: val(d.ref), Remarks: val(d.rem), LineId: lineId, Attachments: 0
        });
        renderGrid(side);
        $(d.amt).value = '';
        fillSub(side, []);
        if (side === 'cr') { $(d.rem).value = ''; $(d.ref).value = ''; }
        else { $(d.rem).value = ''; $(d.ref).value = ''; }
        bindSub(side);
        $(d.acc).focus();
    }

    // ============================================== ReceiptGrid_DoubleClick / PaymentGrid_DoubleClick
    function editRow(side, idx) {
        var d = SIDE[side], r = S[side][idx];
        if (!r) return;
        S.upd[side] = idx;
        $(d.acc).value = String(r.AccountId);
        detailLeave(side).then(function () {
            if (S.subLen[side] === 0) {
                fillSub(side, [{ Id: r.SubsidiaryAccountId, SubsidiaryAccount: r.SubsidiaryAccount, SubsidiaryTypeId: 4, AccountId: r.AccountId, AccountTitle: r.Title }]);
            }
            $(d.sub).value = String(r.SubsidiaryAccountId);
            $(d.amt).value = GD.fmtSingle(r.Amount);
            $(d.page).value = String(r.PageNo);
            $(d.cdate).value = GD.iso(r.ChequeDate);
            $(d.ref).value = r.ChequeRef || '';
            $(d.rem).value = r.Remarks || '';
            $(d.add).hidden = true; $(d.upd).hidden = false; $(d.cancel).hidden = false;
            $(d.acc).focus();
        });
    }

    // ============================================== btnUpdateCreditRow_Click / btnUpdateDebitRow_Click
    function updateRow(side) {
        if (!validateDetail(side)) return;
        var d = SIDE[side], r = S[side][S.upd[side]];
        if (!r) { alert('Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index'); return; }
        r.AccountId = intOf(val(d.acc)); r.Title = selText(d.acc);
        if (S.subsidiaryFeature) {
            if (val(d.sub) !== '') {
                r.SubsidiaryAccountId = intOf(val(d.sub)); r.SubsidiaryAccount = selText(d.sub); r.SubsidiaryAccountTypeId = intOf(GD.selData(d.sub, 'type'));
            } else {
                r.SubsidiaryAccountId = intOf(val(d.acc)); r.SubsidiaryAccount = selText(d.acc); r.SubsidiaryAccountTypeId = 4;
            }
        } else { r.SubsidiaryAccountId = 0; r.SubsidiaryAccount = ''; r.SubsidiaryAccountTypeId = 0; }
        r.Amount = GD.num(val(d.amt).trim());
        r.PageNo = intOf(val(d.page).trim());
        r.ChequeDate = val(d.cdate);
        r.ChequeRef = val(d.ref).trim();
        r.Remarks = val(d.rem).trim();
        $(d.acc).value = ''; $(d.rem).value = ''; $(d.amt).value = ''; $(d.ref).value = '';
        if (side === 'cr') $(d.page).value = '';
        fillSub(side, []);
        $(d.add).hidden = false; $(d.upd).hidden = true; $(d.cancel).hidden = true;
        renderGrid(side);
        $(d.acc).focus();
    }

    // ============================================== btnCreditRowCancel_Click / btnDebitRowCancel_Click
    function cancelRow(side) {
        var d = SIDE[side];
        $(d.add).hidden = false; $(d.cancel).hidden = true; $(d.upd).hidden = true;
        $(d.amt).value = ''; $(d.rem).value = '';
        if (side === 'cr') $(d.ref).value = '';
        fillSub(side, []);
        $(d.acc).focus();
    }

    // ============================================== ReceiptGrdSetting / PaymentGrdSetting
    function renderGrid(side) {
        var d = SIDE[side], rows = S[side];
        var h = '<thead><tr><th>X</th><th>' + d.titleCol + '</th>' + (S.subsidiaryFeature ? '<th>SubsidiaryAccount</th>' : '')
            + '<th>Amount</th><th>PageNo</th><th>ChequeDate</th><th>ChequeRef</th><th>Remarks</th><th>Attachments</th></tr></thead><tbody>';
        var total = 0;
        rows.forEach(function (r, i) {
            total += GD.num(r.Amount);
            h += '<tr ondblclick="DB.editRow(\'' + side + '\',' + i + ')">'
                + '<td><button type="button" onclick="DB.deleteRow(\'' + side + '\',' + i + ')">X</button></td>'
                + '<td>' + GD.esc(r.Title) + '</td>'
                + (S.subsidiaryFeature ? '<td>' + GD.esc(r.SubsidiaryAccount) + '</td>' : '')
                + '<td class="num">' + GD.fmtSingle(r.Amount) + '</td>'
                + '<td class="num">' + r.PageNo + '</td>'
                + '<td>' + GD.fmtDate(r.ChequeDate, 'dd-MMM-yy') + '</td>'
                + '<td>' + GD.esc(r.ChequeRef) + '</td>'
                + '<td>' + GD.esc(r.Remarks) + '</td>'
                + '<td class="num">' + (r.Attachments || 0) + '</td></tr>';
        });
        h += '</tbody><tfoot><tr><td></td><td></td>' + (S.subsidiaryFeature ? '<td></td>' : '')
            + '<td class="num">' + GD.fmtSingle(total) + '</td><td colspan="5"></td></tr></tfoot>';
        $(d.grid).innerHTML = h;
    }

    /** ReceiptGrid_ColumnButtonClick / PaymentGrid_ColumnButtonClick "Delete" */
    function deleteRow(side, idx) {
        S[side].splice(idx, 1);
        renderGrid(side);
    }

    // ============================================================== cmbCashAccount_Leave
    function cashLeave() {
        var cash = val('cmbCashAccount');
        if (cash !== '') {
            var c = intOf(cash);
            if (S.cr.some(function (r) { return r.AccountId === c; })) {
                $('cmbCashAccount').value = '';
                alert("You Can't Select This Cash Account Because Receipt Grid Has Entry Against This Account");
                return;
            }
            if (S.dr.some(function (r) { return r.AccountId === c; })) {
                $('cmbCashAccount').value = '';
                alert("You Can't Select This Cash Account Because Payment Grid Has Entry Against This Account");
                return;
            }
            var date = val('txtDocDate') || today();
            var prev = GD.toDate(date); prev.setDate(prev.getDate() - 1);
            GD.get(API + '/balance', { accountId: c, date: GD.iso(prev) }).then(function (r) {
                var op = GD.num(r.balance);
                $('lblOpeningBalance').textContent = GD.fmtBoth(op);
                $('lblCreditOrDebit').textContent = GD.drCr(op);
                return GD.get(API + '/balance', { accountId: c, date: date });
            }).then(function (r) {
                var cl = GD.num(r.balance);
                $('lblClosingBalance').textContent = GD.fmtBoth(cl);
                $('lblCreditOrDebitForClosing').textContent = GD.drCr(cl);
            }).catch(fail);
        } else {
            $('lblOpeningBalance').textContent = '0'; $('lblCreditOrDebit').textContent = '';
            $('lblClosingBalance').textContent = '0'; $('lblCreditOrDebitForClosing').textContent = '';
        }
    }

    // =================================================================== Reset / btnnew_Click
    function newForm() {
        S.recId = 0; S.loadedDate = null;
        S.upd.cr = -1; S.upd.dr = -1;
        S.cr = []; S.dr = [];
        renderGrid('cr'); renderGrid('dr');
        GD.get(API + '/cash/code').then(function (r) { $('txtDocNo').value = r.voucherCode; }).catch(fail);
        $('CmbCreditAccount').value = ''; $('txtCreditAmount').value = '';
        $('CmbDebitAccount').value = ''; $('txtDebitAmount').value = '';
        $('cmbCashAccount').focus();
        $('btnSave').hidden = false; $('btnUpdate').hidden = true;
    }

    // ===================================================================== btnRefresh_Click
    function refresh() {
        GD.get(API + '/cash/accounts').then(function (d) {
            var keep = { a: val('cmbCashAccount'), h: val('CmbCashAccountForHistory'), c: val('CmbCreditAccount'), dd: val('CmbDebitAccount') };
            S.cashAccounts = d.cashAccounts || []; S.detailAccounts = d.detailAccounts || [];
            fillAccounts('cmbCashAccount', S.cashAccounts, ''); $('cmbCashAccount').value = keep.a;
            fillAccounts('CmbCashAccountForHistory', S.cashAccounts, ''); $('CmbCashAccountForHistory').value = keep.h;
            fillAccounts('CmbCreditAccount', S.detailAccounts, ''); $('CmbCreditAccount').value = keep.c;
            fillAccounts('CmbDebitAccount', S.detailAccounts, ''); $('CmbDebitAccount').value = keep.dd;
            if (S.subsidiaryFeature) { S.subAll = d.subsidiaryAccounts || []; bindSub('dr'); bindSub('cr'); }
        }).catch(fail);
    }

    // =============================================================== btnsave_Click / btnUpdate_Click -> Insert()
    function save(isUpdate) {
        if (!isUpdate) S.recId = 0;
        if (intOf(val('txtDocNo')) === 0) { alert('Doc No Field is Required'); return; }
        if (val('cmbCashAccount') === '') { alert('cash Account Field is Required'); $('cmbCashAccount').focus(); return; }
        if (S.cr.length === 0 && S.dr.length === 0) { alert('Add AtLeast One Row In Either Payment Side Or Receipt Side'); return; }
        if (!confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var date = val('txtDocDate');
        if (S.loadedDate && GD.iso(S.loadedDate) === date) date = GD.isoTime(S.loadedDate);
        var line = function (r) {
            return { accountId: r.AccountId, subsidiaryAccountId: r.SubsidiaryAccountId, subsidiaryAccountTypeId: r.SubsidiaryAccountTypeId,
                amount: r.Amount, pageNo: r.PageNo, chequeDate: r.ChequeDate, chequeRef: r.ChequeRef, remarks: r.Remarks, lineId: r.LineId };
        };
        var body = {
            id: S.recId, voucherCode: val('txtDocNo'), voucherDate: date, cashAccountId: intOf(val('cmbCashAccount')),
            remarks: val('txtHeaderRemarks'), receipts: S.cr.map(line), payments: S.dr.map(line)
        };
        var print = $('ChkPrintPreview').checked;
        GD.api('POST', API + '/cash/save', body).then(function (res) {
            alert(res.message);
            newForm();
            if (print) slip({ documentTypeId: 9, id: res.id });
        }).catch(fail);
    }

    // =============================================================================== History
    function tab(i) {
        $('tabForm').classList.toggle('on', i === 0); $('tabHistory').classList.toggle('on', i === 1);
        $('pageForm').hidden = i !== 0; $('pageHistory').hidden = i !== 1;
        if (i === 1) history();      // tabControl1_SelectedIndexChanged -> HistoryFill
    }

    function loadVoucherDates() {
        GD.get(API + '/cash/voucher-dates').then(fillVoucherDates).catch(fail);
    }

    /** HistoryFill (:1683) */
    function history() {
        GD.get(API + '/cash/history', { voucherDate: val('datVouchderDateForHistory'), cashAccountId: val('CmbCashAccountForHistory') || 0 })
            .then(function (rows) {
                var cr = [], dr = [];
                (rows || []).forEach(function (r) {
                    if (GD.num(GD.col(r, 'CreditAmount')) > 0) cr.push(r);
                    else if (GD.num(GD.col(r, 'DebitAmount')) > 0) dr.push(r);
                });
                if ((rows || []).length === 0) {
                    $('DataGridCreditHistory').innerHTML = ''; $('DataGridDebitHistory').innerHTML = '';
                    return;
                }
                renderHistory('DataGridCreditHistory', cr, 'CreditAmount', true);
                renderHistory('DataGridDebitHistory', dr, 'DebitAmount', false);
            }).catch(fail);
    }

    /** DataGridCreditHistorySetting / DataGridDebitHistorySetting: grouped by CashAccount. */
    function renderHistory(grid, rows, amountCol, groupTotals) {
        var groups = {}, keys = [];
        rows.forEach(function (r) {
            var k = GD.col(r, 'CashAccount') || '';
            if (!groups[k]) { groups[k] = []; keys.push(k); }
            groups[k].push(r);
        });
        keys.sort();
        var grand = 0;
        var sub = S.subsidiaryFeature;
        var cols = 14 + (sub ? 1 : 0);
        var h = '<thead><tr><th>VoucherCode</th><th>DetailAccount</th>' + (sub ? '<th>SubsidiaryAccount</th>' : '')
            + '<th>' + amountCol + '</th><th>Remarks</th><th>PageNo</th><th>ChequeDate</th><th>ChequeRef</th><th>EntryUserName</th>'
            + '<th>EntryDate</th><th>ModifyUserName</th><th>ModifyDate</th><th>NoOfAttachments</th><th>Edit</th><th>Print</th></tr></thead><tbody>';
        keys.forEach(function (k) {
            h += '<tr class="grp"><td colspan="' + cols + '">CashAccount: ' + GD.esc(k) + '</td></tr>';
            var t = 0;
            groups[k].forEach(function (r) {
                var id = GD.col(r, 'Id');
                t += GD.num(GD.col(r, amountCol));
                grand += GD.num(GD.col(r, amountCol));
                h += '<tr ondblclick="DB.read(' + id + ')"><td>' + GD.esc(GD.col(r, 'VoucherCode')) + '</td>'
                    + '<td>' + GD.esc(GD.col(r, 'DetailAccount')) + '</td>'
                    + (sub ? '<td>' + GD.esc(GD.col(r, 'SubsidiaryAccountTitle')) + '</td>' : '')
                    + '<td class="num">' + GD.fmtSingle(GD.col(r, amountCol)) + '</td>'
                    + '<td>' + GD.esc(GD.col(r, 'Comments')) + '</td>'
                    + '<td class="num">' + GD.esc(GD.col(r, 'SortNo')) + '</td>'
                    + '<td>' + GD.fmtDate(GD.col(r, 'DCheqDate'), 'dd-MMM-yy') + '</td>'
                    + '<td>' + GD.esc(GD.col(r, 'CheqNoDetail')) + '</td>'
                    + '<td>' + GD.esc(GD.col(r, 'EntryUserName')) + '</td>'
                    + '<td>' + GD.fmtDate(GD.col(r, 'EntryDate'), 'dd-MMM-yyyy hh:mm tt') + '</td>'
                    + '<td>' + GD.esc(GD.col(r, 'ModifyUserName')) + '</td>'
                    + '<td>' + GD.fmtDate(GD.col(r, 'ModifyDate'), 'dd-MMM-yyyy hh:mm tt') + '</td>'
                    + '<td class="num"><a href="#" onclick="alert(\'Attachments are not ported to the web.\');return false;">' + GD.esc(GD.col(r, 'NoOfAttachments')) + '</a></td>'
                    + '<td><button type="button" onclick="DB.read(' + id + ')">Edit</button></td>'
                    + '<td><button type="button" onclick="DB.slip({documentTypeId:9,id:' + id + '})">Print</button></td></tr>';
            });
            if (groupTotals) {
                h += '<tr class="grp"><td></td><td></td>' + (sub ? '<td></td>' : '') + '<td class="num">' + GD.fmtSingle(t) + '</td><td colspan="11"></td></tr>';
            }
        });
        // TotalRow = True on both history grids (InitializeComponent)
        h += '</tbody><tfoot><tr><td></td><td></td>' + (sub ? '<td></td>' : '') + '<td class="num">' + GD.fmtSingle(grand) + '</td><td colspan="11"></td></tr></tfoot>';
        $(grid).innerHTML = h;
    }

    /** ReadById (:1915) */
    function read(id) {
        GD.get(API + '/read/' + id).then(function (o) {
            var hd = o.head;
            S.recId = id;
            $('btnSave').hidden = true; $('btnUpdate').hidden = false;
            tab(0);
            S.loadedDate = GD.toDate(GD.col(hd, 'VoucherDate'));
            $('txtDocDate').value = GD.iso(S.loadedDate);
            $('txtDocNo').value = GD.col(hd, 'VoucherCode');
            $('cmbCashAccount').value = String(GD.col(hd, 'RefAccountId'));
            S.cr = []; S.dr = [];
            cashLeave();
            $('txtHeaderRemarks').value = GD.col(hd, 'Remarks') || '';
            (o.details || []).forEach(function (x) {
                var acc = intOf(GD.col(x, 'AccountId')), against = intOf(GD.col(x, 'AgainstAccountId'));
                if (acc === against) return;
                var type = intOf(GD.col(x, 'SubsidiaryTypeId'));
                var subId = type === 1 ? intOf(GD.col(x, 'SupplierCustomerId')) : type === 2 ? intOf(GD.col(x, 'EmployeeId'))
                    : (type === 3 || type === 4) ? intOf(GD.col(x, 'SubsidiaryAccountId')) : 0;
                var row = {
                    AccountId: acc, Title: GD.col(x, 'AccountTitle'), SubsidiaryAccountId: subId,
                    SubsidiaryAccount: GD.col(x, 'SubsidiaryAccountTitle') || '', SubsidiaryAccountTypeId: type,
                    PageNo: intOf(GD.col(x, 'SortNo')), ChequeDate: GD.iso(GD.col(x, 'DCheqDate')),
                    ChequeRef: GD.col(x, 'CheqNoDetail') || '', Remarks: GD.col(x, 'Comments') || '',
                    LineId: intOf(GD.col(x, 'LineId')), Attachments: 0
                };
                if (GD.num(GD.col(x, 'CreditAmount')) > 0) { row.Amount = GD.num(GD.col(x, 'CreditAmount')); S.cr.push(row); }
                if (GD.num(GD.col(x, 'DebitAmount')) > 0) { row = Object.assign({}, row, { Amount: GD.num(GD.col(x, 'DebitAmount')) }); S.dr.push(row); }
            });
            renderGrid('cr'); renderGrid('dr');
        }).catch(fail);
    }

    /** CommonServices.DayBookSlip / btnPrint144_Click -> Sp_DayBookSlip */
    function slip(p) {
        GD.get(API + '/slip144', p).then(function (rows) { GD.printRows('144-DayBookSlip', rows); }).catch(fail);
    }

    function print144() {
        slip({ documentTypeId: 9, accountId: val('CmbCashAccountForHistory') || 0 });
    }

    // ============================================================== key handling (DayBook_KeyDown)
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName)) {
            var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) {
                return !x.disabled && !x.hidden && x.offsetParent !== null && x.tabIndex >= 0;
            });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return;
        }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        var onForm = !$('pageForm').hidden;
        if (k === 'n' && onForm) { e.preventDefault(); newForm(); }
        else if (k === 's') { e.preventDefault(); if (!$('btnSave').hidden && !$('btnSave').disabled) save(false); }
        else if (k === 'u') { e.preventDefault(); if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) save(true); }
        else if (k === 't') { e.preventDefault(); tab(onForm ? 1 : 0); }
        else if (e.key === 'ArrowLeft') { e.preventDefault(); $('CmbCreditAccount').focus(); }
        else if (e.key === 'ArrowRight') { e.preventDefault(); $('CmbDebitAccount').focus(); }
    });

    window.DB = {
        tab: tab, newForm: newForm, save: save, refresh: refresh, cashLeave: cashLeave, detailLeave: detailLeave,
        subLeave: subLeave, addRow: addRow, updateRow: updateRow, cancelRow: cancelRow, editRow: editRow,
        deleteRow: deleteRow, history: history, read: read, slip: slip, print144: print144,
        loadVoucherDates: loadVoucherDates,
        onlyDecimal: function (el) { var v = el.value.replace(/[^0-9.,]/g, ''); if (v !== el.value) el.value = v; },
        amountLeave: function (el) { el.value = GD.fmtSingle(el.value); }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
}());
