/* ============================================================================================
 * Screen 24 "Day Book (Off Set)" — Architecture.WinApp.Account_Definition.frmDayBook
 * (ScreenName frmDayBook), DocumentTypeId 8. One grid of credit/debit PAIRS: the credit row takes
 * an odd LineId n, its debit row n+1, both carrying the same Sub Code. A "Temporary Account"
 * (configuration DayBookAcTemporary) switches on Sub Code and the Cr/Dr Total Amount fields that
 * let one total be split over several pairs.
 *
 * The desktop's own quirks are reproduced, not corrected:
 *  - the grid's AccountCode column is filled from the account combo's 4th column
 *    (ParentAccountTitle) on "+", and becomes Conversion.ToInt of it (normally 0) on "Update";
 *  - the Cheque No list is bound only when the credit account's AccountCode reads as 15;
 *  - Credit/Debit balances are as of NOW, not the voucher date; the cash opening/closing are
 *    read on leaving the Cash Account only (changing the date does not refresh them);
 *  - editing a debit row puts the credit row's subsidiary into the DEBIT subsidiary combo;
 *  - the Debit-total branch of Update compares the Credit total with the pair's credit total;
 *  - deleting a pair re-totals the remaining rows of that Sub Code from TotalAmountDr and decides
 *    which total to write by the SubsidiaryAccountTypeId / Remarks columns (ItemArray[7] / [8]).
 * Not ported: attachments.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/accounts/api/day-book';
    var $ = function (id) { return document.getElementById(id); };

    var S = {
        rights: {}, subsidiaryFeature: false, multiCurrency: false, chequeBook: false, tempAccountId: 0,
        baseCurrency: 0, baseRate: 0, subAll: [], accounts: [], cashAccounts: [],
        recId: 0, loadedDate: null, table: [], unique: 1, updateRowIndex: 0, cur: -1,
        crLen: 0, drLen: 0
    };

    function val(id) { var e = $(id); return e ? e.value : ''; }
    function set(id, v) { var e = $(id); if (e) e.value = (v === null || v === undefined) ? '' : String(v); }
    function toInt(v) {                      // Conversion.ToInt: Convert.ToInt32, 0 on failure
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return Math.round(v);
        var s = String(v).trim();
        if (!/^[-+]?\d+(\.\d+)?$/.test(s)) return 0;
        return Math.round(parseFloat(s));
    }
    function toDbl(v) { return GD.num(v); }  // Conversion.ToDouble (thousand separators allowed)
    function str(v) { return (v === null || v === undefined) ? '' : String(v); }
    function clr(n) { n = Number(n) || 0; return String(n); }   // C# double.ToString()
    function selText(id) { var e = $(id); return (e && e.selectedIndex >= 0 && e.value !== '') ? e.options[e.selectedIndex].text : ''; }
    function today() { return GD.iso(new Date()); }
    function fail(e) { alert(e && e.message ? e.message : e); }
    function has(id) { return val(id) !== '' && val(id) !== '0'; }

    // Server buttons (user mandate 2026-10-02): disabled at once with a spinner, a second request
    // refused while one is running, and put back to the state they had on success AND on failure.
    function lock(b) {
        if (typeof b === 'string') b = $(b);
        if (!b) return true;
        if (b.getAttribute('data-busy')) return false;
        b.setAttribute('data-busy', b.disabled ? 'd' : 'e');
        b.setAttribute('data-html', b.innerHTML);
        b.disabled = true;
        b.innerHTML = '<i class="fa fa-spinner fa-spin"></i> ' + b.innerHTML;
        return true;
    }
    function unlock(b) {
        if (typeof b === 'string') b = $(b);
        if (!b || !b.getAttribute('data-busy')) return;
        b.disabled = b.getAttribute('data-busy') === 'd';
        b.innerHTML = b.getAttribute('data-html');
        b.removeAttribute('data-busy'); b.removeAttribute('data-html');
    }
    function isBusy(id) { var b = $(id); return !!(b && b.getAttribute('data-busy')); }
    function busy(b, fn) {
        if (!lock(b)) return Promise.resolve();
        var p;
        try { p = Promise.resolve(fn()); } catch (x) { p = Promise.reject(x); }
        return p.catch(fail).then(function () { unlock(b); });
    }
    function firstRow(tableId) {
        var t = $(tableId); if (!t) return;
        var r = t.querySelector('tbody tr');
        if (r) r.focus(); else if (t.parentNode && t.parentNode.focus) t.parentNode.focus();
    }

    function fillAccounts(sel, rows) {
        GD.fill(sel, rows, 'Id', 'AccountTitle', { code: 'AccountCode', parent: 'ParentAccountTitle', 'class': 'AccountClass' }, '');
    }
    function fillSub(id, rows) {
        GD.fill(id, rows, 'Id', 'SubsidiaryAccount', { type: 'SubsidiaryTypeId', gl: 'AccountId', code: 'Code', account: 'AccountTitle' }, '');
    }

    // ==================================================================== frmDayBook_Load
    function load() {
        GD.get(API + '/offset/init').then(function (d) {
            S.rights = d.rights || {};
            GD.setDecimals(d.amountDecimals, d.rateDecimals);
            S.subsidiaryFeature = !!d.subsidiaryFeature;
            S.multiCurrency = !!d.multiCurrencyFeature;
            S.chequeBook = !!d.chequeBookEnabled;
            S.tempAccountId = d.temporaryAccountId || 0;
            S.baseCurrency = d.baseCurrencyId || 0;
            S.baseRate = d.baseCurrencyRate || 0;
            S.subAll = d.subsidiaryAccounts || [];
            S.accounts = d.detailAccounts || [];
            S.cashAccounts = d.cashAccounts || [];
            $('btnsave').disabled = !S.rights.canSave;
            $('btnUpdate').disabled = !S.rights.canUpdate;
            $('btnPrint144').disabled = !S.rights.canPrint;
            set('txtDocNo', d.voucherCode);
            set('txtDocDate', today());
            fillAccounts('cmbCashAccount', S.cashAccounts);
            fillAccounts('CmbCashAccountForHistory', S.cashAccounts);
            set('txtSubCode', S.unique);
            fillAccounts('cmbDebitAccount', S.accounts);
            fillAccounts('cmbCreditAccount', S.accounts);
            fillAccounts('CmbTemporaryAccount', S.accounts);
            if (S.tempAccountId > 0) set('CmbTemporaryAccount', S.tempAccountId);
            GD.fill('cmbCurrency', d.currencies, 'Id', 'CurrencyCode', null, '');
            // MultiCurrencyFeature()
            Array.prototype.forEach.call(document.querySelectorAll('.mc'), function (e) { e.hidden = !S.multiCurrency; });
            // Cheque book
            $('pnlChequeNo').hidden = !S.chequeBook;
            $('pnlChequeRef').hidden = S.chequeBook;
            // Temporary account panels
            $('rowTemporary').hidden = !(S.tempAccountId > 0);
            Array.prototype.forEach.call(document.querySelectorAll('.tmp'), function (e) { e.hidden = !(S.tempAccountId > 0); });
            Array.prototype.forEach.call(document.querySelectorAll('.sub'), function (e) { e.hidden = !S.subsidiaryFeature; });
            if (S.subsidiaryFeature) bindAllSub();
            fillVoucherDates(d.voucherDates);
            defaultConfigurations();
            renderGrid();
            $('txtDocDate').focus();
            // ?id= in the URL loads that voucher (ReadById)
            var qid = parseInt(new URLSearchParams(location.search).get('id') || '0', 10);
            if (qid > 0) read(qid);
        }).catch(fail);
    }

    /** DefaultConfigurations(): Base Currency and BaseCurrencyRate (the rate read through ToInt). */
    function defaultConfigurations() {
        if (S.baseCurrency > 0) set('cmbCurrency', S.baseCurrency);
        if (S.baseRate > 0) set('txtExchangeRate', GD.fmtRate(S.baseRate));
    }

    function fillVoucherDates(rows) {
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var v = GD.col(r, 'VoucherDate');
            h += '<option value="' + GD.esc(GD.iso(v)) + '">' + GD.esc(GD.fmtDate(v, 'dd-MMM-yyyy')) + '</option>';
        });
        $('datVouchderDateForHistory').innerHTML = h;
    }

    // ===================================================== subsidiary binding
    function bindAllSub() {                   // BindAllSubsidiaryaccounts (lengths untouched)
        if (S.subAll.length > 0) { fillSub('CmbCrSubsidiaryAccount', S.subAll); fillSub('CmbDrSubsidiaryAccount', S.subAll); }
    }

    /** BindCreditSubsidiaryAccount / BindDebitSubsidiaryAccount */
    function bindSub(side) {
        var acc = side === 'cr' ? 'cmbCreditAccount' : 'cmbDebitAccount';
        var sub = side === 'cr' ? 'CmbCrSubsidiaryAccount' : 'CmbDrSubsidiaryAccount';
        var len = 0;
        if (has(acc)) {
            if (S.subAll.length > 0) {
                var id = toInt(val(acc));
                if (id !== 0) {
                    var match = S.subAll.filter(function (r) { return toInt(GD.col(r, 'AccountId')) === id; });
                    var keep = val(sub) !== '' ? toInt(val(sub)) : 0;
                    if (match.length > 0) {
                        len = match.length;
                        fillSub(sub, match);
                        set(sub, '');
                        if (match.some(function (r) { return toInt(GD.col(r, 'Id')) === keep; })) set(sub, keep);
                    } else fillSub(sub, []);
                } else fillSub(sub, []);
            } else fillSub(sub, []);
        } else fillSub(sub, []);
        if (side === 'cr') S.crLen = len; else S.drLen = len;
    }

    // ========================================================== cmbCreditAccount_Leave
    function creditLeave() {
        GD.get(API + '/balance', { accountId: toInt(val('cmbCreditAccount')), date: GD.isoTime(new Date()) }).then(function (r) {
            $('CrBalance').hidden = false;
            $('CrBalance').textContent = GD.fmtBoth(r.balance);
        }).catch(fail);
        if (S.chequeBook) {
            if (has('cmbCreditAccount') && toInt(GD.selData('cmbCreditAccount', 'code')) === 15) bindChequeNo();
            else $('CmbChequeNo').innerHTML = '';
        }
        if (has('cmbCreditAccount')) bindSub('cr'); else bindAllSub();
    }

    function bindChequeNo() {
        GD.get(API + '/cheques', { bankId: toInt(val('cmbCreditAccount')) }).then(function (rows) {
            GD.fill('CmbChequeNo', rows, 'Id', 'CheqNo', null, '');
        }).catch(fail);
    }

    // =========================================================== cmbDebitAccount_Leave
    function debitLeave() {
        GD.get(API + '/balance', { accountId: toInt(val('cmbDebitAccount')), date: GD.isoTime(new Date()) }).then(function (r) {
            $('DrBalance').hidden = false;
            $('DrBalance').textContent = GD.fmtBoth(r.balance);
        }).catch(fail);
        if (has('cmbDebitAccount')) bindSub('dr'); else bindAllSub();
    }

    /** Cmb*SubsidiaryAccount_AfterCloseUp: the GL behind the pick becomes the account. */
    function subPicked(side) {
        var sub = side === 'cr' ? 'CmbCrSubsidiaryAccount' : 'CmbDrSubsidiaryAccount';
        var gl = toInt(GD.selData(sub, 'gl'));
        if (val(sub) !== '' && gl !== 0) {
            if (side === 'cr') { set('cmbCreditAccount', gl); creditLeave(); }
            else { set('cmbDebitAccount', gl); debitLeave(); }
        }
    }

    function crRemarksLeave() { set('txtDrRemarks', val('txtCrRemarks')); }

    // ============================================================= cmbCashAccount_Leave
    function cashLeave() {
        var date = val('txtDocDate') || today();
        var prev = GD.toDate(date); prev.setDate(prev.getDate() - 1);
        var cash = toInt(val('cmbCashAccount'));
        GD.get(API + '/balance', { accountId: cash, date: GD.iso(prev) }).then(function (r) {
            set('txtOpening', GD.fmtBoth(r.balance));
            return GD.get(API + '/balance', { accountId: cash, date: date });
        }).then(function (r) { set('txtClosing', GD.fmtBoth(r.balance)); }).catch(fail);
    }

    // ============================================================ currency / exchange rate
    function currencyLeave() {
        var c = toInt(val('cmbCurrency'));
        if (c === 0) return;
        if (c !== S.baseCurrency) {
            GD.get(API + '/last-rate', { currencyId: c }).then(function (r) {
                set('txtExchangeRate', r.found ? GD.fmtRate(r.lastExchRate) : '0');
                rateChanged();
            }).catch(fail);
        } else { set('txtExchangeRate', GD.fmtRate(S.baseRate)); rateChanged(); }
    }

    function rateLeave() { set('txtExchangeRate', GD.fmtRate(val('txtExchangeRate'))); }

    /** txtExchangeRate_TextChanged -> Fcy amounts per row, then CalculateTotalInformation. */
    function rateChanged() {
        var rate = toDbl(val('txtExchangeRate'));
        S.table.forEach(function (r) {
            if (S.table.length > 0 && rate > 0) { r.DebitFcyAmount = r.AmountDr / rate; r.CreditFcyAmount = r.AmountCr / rate; }
            else { r.DebitFcyAmount = 0; r.CreditFcyAmount = 0; }
        });
        renderGrid();
        calcTotals();
    }

    function calcTotals() {
        if (S.table.length > 0) {
            var t = 0; S.table.forEach(function (r) { t += toDbl(r.DebitFcyAmount); });
            set('txtFcyAmount', GD.fmt3(t));
        } else set('txtFcyAmount', '0');
    }

    // ============================================================= FormValidationDetail
    function validateDetail() {
        if (!has('cmbCreditAccount')) { alert('Credit Account Field Required'); $('cmbCreditAccount').focus(); return false; }
        if (S.subsidiaryFeature && S.crLen > 0 && val('CmbCrSubsidiaryAccount') === '') { alert('Credit Subsidiary Account Field Required'); return false; }
        if (!has('cmbDebitAccount')) { alert('Debit Account Field Required'); $('cmbDebitAccount').focus(); return false; }
        if (S.subsidiaryFeature && S.drLen > 0 && val('CmbDrSubsidiaryAccount') === '') { alert('Debit Subsidiary Account Field Required'); return false; }
        if (toDbl(val('txtAmount').trim()) === 0) { alert('Amount Field Required'); $('txtAmount').focus(); return false; }
        return true;
    }

    function subOf(side) {
        var acc = side === 'cr' ? 'cmbCreditAccount' : 'cmbDebitAccount';
        var sub = side === 'cr' ? 'CmbCrSubsidiaryAccount' : 'CmbDrSubsidiaryAccount';
        var len = side === 'cr' ? S.crLen : S.drLen;
        if (!S.subsidiaryFeature) return { id: 0, text: '', type: 0 };
        return len > 0 ? { id: toInt(val(sub)), text: selText(sub), type: toInt(GD.selData(sub, 'type')) }
                       : { id: toInt(val(acc)), text: selText(acc), type: 4 };
    }

    function chequeNo() { return S.chequeBook ? selText('CmbChequeNo') : val('txtChequeRef').trim(); }
    function chequeId() { return S.chequeBook ? toInt(val('CmbChequeNo')) : 0; }

    function totals() {
        if (val('txtTotalCrAmount') === '' && val('txtTotalDrAmount') === '') { set('txtTotalCrAmount', '0'); set('txtTotalDrAmount', '0'); }
    }

    // ================================================================= btnAddDetail_Click
    function addDetail() {
        if (!validateDetail()) return;
        var Amount = 0, TotalCredit = 0, TotalDebit = 0, Remaining = 0;
        var amt = toDbl(val('txtAmount'));
        totals();
        var cr = toDbl(val('txtTotalCrAmount')), dr = toDbl(val('txtTotalDrAmount'));
        if (cr > 0 && dr > 0) { alert("You Can't Enter Total Credit Amount And Total Debit Amount At Same Time"); return; }
        if (cr > 0) { set('txtTotalDrAmount', '0'); dr = 0; } else if (dr > 0) { set('txtTotalCrAmount', '0'); cr = 0; }
        if (cr > 0) { if (amt > cr) { alert("Amount Shouldn't Greater Than Total Credit Amount"); return; } }
        else if (dr > 0 && amt > dr) { alert("Amount Shouldn't Greater Than Total Debit Amount"); return; }
        var unique = 1;
        if (S.table.length > 0) {
            unique = Math.max.apply(null, S.table.map(function (r) { return toInt(r.LineId); })) + 1;
            if (cr > 0) { if (amt > cr) { alert("Amount Shouldn't Greater Than Total Credit Amount"); return; } }
            else if (dr > 0 && amt > dr) { alert("Amount Shouldn't Greater Than Total Debit Amount"); return; }
            var CreditAccountId = 0, DebitAccountId = 0, sc = toInt(val('txtSubCode'));
            for (var i = 0; i < S.table.length; i++) {
                var t = S.table[i];
                if (toInt(t.SubCode) === sc) {
                    if ((i + 1) % 2 !== 0) CreditAccountId = toInt(t.AccountId); else DebitAccountId = toInt(t.AccountId);
                    TotalCredit = toDbl(t.TotalAmountCr);
                    TotalDebit = toDbl(t.TotalAmountDr);
                    Amount += toInt(t.AmountCr);
                }
            }
            if (TotalCredit > 0 && Amount !== TotalCredit) {
                Remaining = TotalCredit - Amount;
                if (dr > 0) { alert("You Can't Change The Credit Total Amount To Debit Total Amount"); set('txtTotalCrAmount', clr(TotalCredit)); return; }
                if (cr !== TotalCredit) { set('txtTotalCrAmount', clr(TotalCredit)); alert("You Can't Change The Credit Total Amount"); return; }
                if (toInt(val('cmbCreditAccount')) !== CreditAccountId) { alert("You Can't Change The Credit Account"); set('cmbCreditAccount', CreditAccountId); return; }
                if (amt > Remaining) { alert("Amount Shouldn't Greater Than The Remaining Amount: " + clr(Remaining) + " Of Total Credit Amount"); return; }
            } else if (TotalDebit > 0 && Amount !== TotalDebit) {
                Remaining = TotalDebit - Amount;
                if (cr > 0) { set('txtTotalDrAmount', clr(TotalDebit)); alert("You Can't Change The Debit Total Amount To Credit Total Amount"); return; }
                if (cr !== TotalCredit) { set('txtTotalDrAmount', clr(TotalDebit)); alert("You Can't Change The Debit Total Amount"); return; }
                if (toInt(val('cmbDebitAccount')) !== DebitAccountId) { alert("You Can't Change The Debit Account"); return; }
                if (amt > Remaining) { alert("Amount Shouldn't Greater Than The Remaining Amount: " + clr(Remaining) + " Of Total Debit Amount"); return; }
            }
        }
        var rate = toDbl(val('txtExchangeRate'));
        var fcy = (rate > 0 && amt > 0) ? amt / rate : 0;
        var crs = subOf('cr'), drs = subOf('dr');
        S.table.push({
            SubCode: toInt(val('txtSubCode')), AccountCode: GD.selData('cmbCreditAccount', 'parent') || '',
            AccountId: toInt(val('cmbCreditAccount')), AccountTitle: selText('cmbCreditAccount'), AgainstAccountId: toInt(val('cmbDebitAccount')),
            SubsidiaryAccountId: crs.id, SubsidiaryAccount: crs.text, SubsidiaryAccountTypeId: crs.type,
            Remarks: val('txtCrRemarks').trim(), CheqNo: chequeNo(), TotalAmountDr: toDbl(val('txtTotalDrAmount')),
            TotalAmountCr: toDbl(val('txtTotalCrAmount')), AmountDr: 0, AmountCr: toDbl(val('txtAmount').trim()),
            LineId: unique, CheqId: chequeId(), DebitFcyAmount: 0, CreditFcyAmount: fcy
        });
        S.table.push({
            SubCode: toInt(val('txtSubCode')), AccountCode: GD.selData('cmbDebitAccount', 'parent') || '',
            AccountId: toInt(val('cmbDebitAccount')), AccountTitle: selText('cmbDebitAccount'), AgainstAccountId: toInt(val('cmbCreditAccount')),
            SubsidiaryAccountId: drs.id, SubsidiaryAccount: drs.text, SubsidiaryAccountTypeId: drs.type,
            Remarks: val('txtDrRemarks').trim(), CheqNo: chequeNo(), TotalAmountDr: toDbl(val('txtTotalDrAmount')),
            TotalAmountCr: toDbl(val('txtTotalCrAmount')), AmountDr: toDbl(val('txtAmount').trim()), AmountCr: 0,
            LineId: unique + 1, CheqId: chequeId(), DebitFcyAmount: fcy, CreditFcyAmount: 0
        });
        if (amt === Remaining) { set('txtTotalCrAmount', '0'); set('txtTotalDrAmount', '0'); }
        if (toDbl(val('txtTotalCrAmount')) === 0 && toDbl(val('txtTotalDrAmount')) === 0) {
            S.unique = Math.max.apply(null, S.table.map(function (r) { return toInt(r.SubCode); })) + 1;
            set('cmbCreditAccount', ''); set('cmbDebitAccount', '');
        } else if (toDbl(val('txtTotalCrAmount')) > 0) set('cmbDebitAccount', '');
        else if (toDbl(val('txtTotalDrAmount')) > 0) set('cmbCreditAccount', '');
        set('txtSubCode', S.unique);
        renderGrid();
        resetDetail();
        $('cmbCreditAccount').focus();
        calcTotals();
    }

    // =================================================================== grdDetail_DoubleClick
    function editRow(idx) {
        var item = S.table[idx];
        if (!item) return;
        S.cur = idx;
        $('btnAddDetail').hidden = true; $('btnUpdateDetail').hidden = false; $('btnCancelDetail').hidden = false;
        $('txtTotalCrAmount').disabled = true; $('txtTotalDrAmount').disabled = true;
        var id = toInt(item.LineId);
        var byLine = function (l) { for (var i = 0; i < S.table.length; i++) if (toInt(S.table[i].LineId) === l) return S.table[i]; return null; };
        if (id % 2 === 0) {
            var c = byLine(id - 1);
            if (!c) { alert('Index was outside the bounds of the array.'); return; }
            set('cmbCreditAccount', c.AccountId); creditLeave();
            set('cmbDebitAccount', c.AgainstAccountId);
            if (S.crLen === 0) fillSub('CmbCrSubsidiaryAccount', [{ Id: toInt(c.SubsidiaryAccountId), SubsidiaryAccount: c.SubsidiaryAccount, SubsidiaryTypeId: 4, AccountId: c.AccountId, AccountTitle: c.AccountTitle }]);
            if (toInt(c.SubsidiaryAccountId) > 0) set('CmbDrSubsidiaryAccount', toInt(c.SubsidiaryAccountId));
            if (S.chequeBook) setChequeText(c.CheqNo); else set('txtChequeRef', c.CheqNo);
            set('txtCrRemarks', c.Remarks); set('txtTotalCrAmount', str(c.TotalAmountCr)); set('txtAmount', str(c.AmountCr));
            set('txtSubCode', item.SubCode);
            set('cmbCreditAccount', item.AgainstAccountId);
            set('cmbDebitAccount', item.AccountId); debitLeave();
            if (S.drLen === 0) fillSub('CmbDrSubsidiaryAccount', [{ Id: toInt(item.SubsidiaryAccountId), SubsidiaryAccount: item.SubsidiaryAccount, SubsidiaryTypeId: 4, AccountId: item.AccountId, AccountTitle: item.AccountTitle }]);
            if (toInt(item.SubsidiaryAccountId) > 0) set('CmbDrSubsidiaryAccount', toInt(item.SubsidiaryAccountId));
            if (S.chequeBook) setChequeText(item.CheqNo); else set('txtChequeRef', item.CheqNo);
            set('txtDrRemarks', item.Remarks); set('txtTotalDrAmount', str(item.TotalAmountDr)); set('txtAmount', str(item.AmountDr));
        } else {
            var d = byLine(id + 1);
            if (!d) { alert('Index was outside the bounds of the array.'); return; }
            set('cmbDebitAccount', d.AccountId); debitLeave();
            set('cmbCreditAccount', d.AgainstAccountId);
            if (S.drLen === 0) fillSub('CmbDrSubsidiaryAccount', [{ Id: toInt(d.SubsidiaryAccountId), SubsidiaryAccount: d.SubsidiaryAccount, SubsidiaryTypeId: 4, AccountId: d.AccountId, AccountTitle: d.AccountTitle }]);
            if (toInt(d.SubsidiaryAccountId) > 0) set('CmbDrSubsidiaryAccount', toInt(d.SubsidiaryAccountId));
            set('txtDrRemarks', d.Remarks);
            if (S.chequeBook) setChequeText(d.CheqNo); else set('txtChequeRef', d.CheqNo);
            set('txtTotalDrAmount', str(d.TotalAmountDr)); set('txtAmount', str(d.AmountDr));
            set('txtSubCode', item.SubCode);
            set('cmbCreditAccount', item.AccountId); creditLeave();
            set('cmbDebitAccount', item.AgainstAccountId);
            if (S.crLen === 0) fillSub('CmbCrSubsidiaryAccount', [{ Id: toInt(item.SubsidiaryAccountId), SubsidiaryAccount: item.SubsidiaryAccount, SubsidiaryTypeId: 4, AccountId: item.AccountId, AccountTitle: item.AccountTitle }]);
            if (toInt(item.SubsidiaryAccountId) > 0) set('CmbCrSubsidiaryAccount', toInt(item.SubsidiaryAccountId));
            if (S.chequeBook) setChequeText(item.CheqNo); else set('txtChequeRef', item.CheqNo);
            set('txtCrRemarks', item.Remarks); set('txtTotalCrAmount', str(item.TotalAmountCr)); set('txtAmount', str(item.AmountCr));
        }
        S.updateRowIndex = id;
        renderGrid();
        $('cmbCreditAccount').focus();
    }

    function setChequeText(t) {
        var sel = $('CmbChequeNo');
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].text === t) { sel.selectedIndex = i; return; }
        sel.innerHTML = '<option value="">' + GD.esc(t) + '</option>';
    }

    // ================================================================= btnUpdateDetail_Click
    function updateDetail() {
        if (!validateDetail()) return;
        var Remaining = 0;
        var item = S.table[S.cur];
        if (!item) { alert('Object reference not set to an instance of an object.'); return; }
        var curSub = toInt(item.SubCode);
        var curAmt = toDbl(item.AmountDr) > 0 ? toDbl(item.AmountDr) : (toDbl(item.AmountCr) > 0 ? toDbl(item.AmountCr) : 0);
        var arr = S.table.filter(function (r) { return toInt(r.SubCode) === curSub; });
        var amt = toDbl(val('txtAmount'));
        if (arr.length !== 0) {
            var Amount = 0, TotalCredit = 0, TotalDebit = 0;
            totals();
            var cr = toDbl(val('txtTotalCrAmount')), dr = toDbl(val('txtTotalDrAmount'));
            if (cr > 0 && dr > 0) { alert("You Can't Enter Total Credit Amount And Total Debit Amount At Same Time"); return; }
            if (cr > 0) { set('txtTotalDrAmount', '0'); dr = 0; } else if (dr > 0) { set('txtTotalCrAmount', '0'); cr = 0; }
            if (cr > 0) { if (amt > cr) { alert("Amount Shouldn't Greater Than Total Credit Amount"); return; } }
            else if (dr > 0 && amt > dr) { alert("Amount Shouldn't Greater Than Total Debit Amount"); return; }
            var CreditAccountId = 0, DebitAccountId = 0;
            for (var i = 0; i < arr.length; i++) {
                if ((i + 1) % 2 !== 0) CreditAccountId = toInt(arr[i].AccountId); else DebitAccountId = toInt(arr[i].AccountId);
                TotalDebit = toDbl(arr[i].TotalAmountDr);
                TotalCredit = toDbl(arr[i].TotalAmountCr);
                Amount += toInt(arr[i].AmountCr);
            }
            if (TotalCredit > 0 && Amount - curAmt !== TotalCredit) {
                Remaining = TotalCredit - (Amount - curAmt);
                if (dr > 0) { set('txtTotalCrAmount', clr(TotalCredit)); set('txtTotalDrAmount', '0'); alert("You Can't Change The Credit Total Amount To Debit Total Amount"); return; }
                if (cr > TotalCredit) { set('txtTotalCrAmount', clr(TotalCredit)); alert("You Can't Change The Credit Total Amount"); return; }
                if (toInt(val('cmbCreditAccount')) !== CreditAccountId) { alert("You Can't Change The Credit Account"); return; }
                if (amt > Remaining) { alert("Amount Shouldn't Greater Than The Remaining Amount: " + clr(Remaining) + " Of Total Credit Amount"); return; }
            } else if (TotalDebit > 0 && Amount !== TotalDebit) {
                Remaining = TotalDebit - Amount;
                if (cr > 0) { set('txtTotalDrAmount', clr(TotalDebit)); set('txtTotalCrAmount', '0'); alert("You Can't Change The Debit Total Amount To Credit Total Amount"); return; }
                if (cr > TotalCredit) { set('txtTotalDrAmount', clr(TotalDebit)); alert("You Can't Change The Debit Total Amount"); return; }
                if (toInt(val('cmbDebitAccount')) !== DebitAccountId) { alert("You Can't Change The Debit Account"); return; }
                if (amt > Remaining) { alert("Amount Shouldn't Greater Than The Remaining Amount: " + clr(Remaining) + " Of Total Debit Amount"); return; }
            }
        }
        if (S.updateRowIndex > -1) {
            var isDebit = S.updateRowIndex % 2 === 0;
            var subVal = function (side) {
                var sub = side === 'cr' ? 'CmbCrSubsidiaryAccount' : 'CmbDrSubsidiaryAccount';
                var acc = side === 'cr' ? 'cmbCreditAccount' : 'cmbDebitAccount';
                if (!S.subsidiaryFeature) return { id: 0, text: '', type: 0 };
                if (val(sub) !== '' && selText(sub) !== '') return { id: toInt(val(sub)), text: selText(sub), type: toInt(GD.selData(sub, 'type')) };
                return { id: toInt(val(acc)), text: selText(acc), type: 4 };
            };
            var ds = subVal('dr'), cs = subVal('cr');
            var drLine = isDebit ? S.updateRowIndex : S.updateRowIndex + 1;
            var crLine = !isDebit ? S.updateRowIndex : S.updateRowIndex - 1;
            var drRow = S.table.filter(function (r) { return toInt(r.LineId) === drLine; })[0] || S.table[0];
            var crRow = S.table.filter(function (r) { return toInt(r.LineId) === crLine; })[0] || S.table[0];
            Object.assign(drRow, {
                SubCode: toInt(val('txtSubCode')), AccountCode: toInt(GD.selData('cmbDebitAccount', 'parent')),
                AccountId: toInt(val('cmbDebitAccount')), AccountTitle: selText('cmbDebitAccount'), AgainstAccountId: toInt(val('cmbCreditAccount')),
                SubsidiaryAccountId: ds.id, SubsidiaryAccount: ds.text, SubsidiaryAccountTypeId: ds.type,
                Remarks: val('txtDrRemarks'), CheqNo: chequeNo(), CheqId: chequeId(),
                TotalAmountDr: toDbl(val('txtTotalDrAmount')), TotalAmountCr: toDbl(val('txtTotalCrAmount')),
                AmountDr: toDbl(val('txtAmount')), AmountCr: 0, LineId: drLine
            });
            Object.assign(crRow, {
                SubCode: toInt(val('txtSubCode')), AccountCode: toInt(GD.selData('cmbCreditAccount', 'parent')),
                AccountId: toInt(val('cmbCreditAccount')), AccountTitle: selText('cmbCreditAccount'), AgainstAccountId: toInt(val('cmbDebitAccount')),
                SubsidiaryAccountId: cs.id, SubsidiaryAccount: cs.text, SubsidiaryAccountTypeId: cs.type,
                Remarks: val('txtCrRemarks'), CheqNo: chequeNo(), CheqId: chequeId(),
                TotalAmountDr: toDbl(val('txtTotalDrAmount')), TotalAmountCr: toDbl(val('txtTotalCrAmount')),
                AmountDr: 0, AmountCr: toDbl(val('txtAmount')), LineId: crLine
            });
            S.updateRowIndex = -1;
        }
        if (amt === Remaining) { set('txtTotalCrAmount', '0'); set('txtTotalDrAmount', '0'); }
        if (toDbl(val('txtTotalCrAmount')) > 0 || toDbl(val('txtTotalDrAmount')) > 0) set('txtSubCode', curSub);
        else set('txtSubCode', S.unique);
        if (toDbl(val('txtTotalCrAmount')) === 0 && toDbl(val('txtTotalDrAmount')) === 0) { set('cmbCreditAccount', ''); set('cmbDebitAccount', ''); }
        else if (toDbl(val('txtTotalCrAmount')) > 0) set('cmbDebitAccount', '');
        else if (toDbl(val('txtTotalDrAmount')) > 0) set('cmbCreditAccount', '');
        resetDetail();
        $('txtTotalCrAmount').disabled = false; $('txtTotalDrAmount').disabled = false;
        $('btnAddDetail').hidden = false; $('btnUpdateDetail').hidden = true; $('btnCancelDetail').hidden = true;
        rateChanged();
    }

    // ================================================================= btnCancelDetail_Click
    function cancelDetail() {
        $('txtTotalCrAmount').disabled = false; $('txtTotalDrAmount').disabled = false;
        $('btnAddDetail').hidden = false; $('btnUpdateDetail').hidden = true; $('btnCancelDetail').hidden = true;
        var num = toInt(val('txtSubCode'));
        set('txtSubCode', S.unique);
        if (toDbl(val('txtTotalCrAmount')) > 0) set('cmbDebitAccount', '');
        else if (toDbl(val('txtTotalDrAmount')) > 0) set('cmbCreditAccount', '');
        if (num !== S.unique) { set('txtTotalCrAmount', ''); set('txtTotalDrAmount', ''); }
        resetDetail();
    }

    function resetDetail() {
        set('txtCrRemarks', ''); set('txtDrRemarks', ''); set('txtChequeRef', '');
        var c = $('CmbChequeNo'); if (c) c.selectedIndex = -1;
        set('txtAmount', '');
        $('CrBalance').textContent = ''; $('DrBalance').textContent = '';
    }

    // ============================================================ grdDetail_ColumnButtonClick "Delete"
    function deleteRow(idx) {
        if (!confirm('Are you sure! You Want To Delete The Record ?')) return;
        var item = S.table[idx];
        if (!item) return;
        var subCode = toInt(item.SubCode), id = toInt(item.LineId);
        var pair = id % 2 === 0 ? id - 1 : id + 1;
        for (var i = 0; i < S.table.length; i++) if (toInt(S.table[i].LineId) === pair) { S.table.splice(i, 1); break; }
        S.table.splice(S.table.indexOf(item), 1);
        var arr = S.table.filter(function (r) { return toInt(r.SubCode) === subCode; });
        if (arr.length !== 0 && toInt(val('txtSubCode')) !== subCode) {
            var amount = 0;
            arr.forEach(function (r) { amount += toDbl(r.TotalAmountDr); });            // ItemArray[10]
            if (toDbl(arr[0].SubsidiaryAccountTypeId) > 0) {                           // ItemArray[7]
                S.table.forEach(function (r) { if (toInt(r.SubCode) === subCode) r.TotalAmountDr = amount; });
            } else if (toDbl(arr[0].Remarks) > 0) {                                     // ItemArray[8]
                S.table.forEach(function (r) { if (toInt(r.SubCode) === subCode) r.TotalAmountCr = amount; });
            }
        }
        renderGrid();
    }

    // =================================================================== gridsetting
    function renderGrid() {
        var tmp = S.tempAccountId > 0, sub = S.subsidiaryFeature, mc = S.multiCurrency;
        var h = '<thead><tr>' + (tmp ? '<th>SubCode</th>' : '') + '<th>AccountCode</th><th>AccountTitle</th>'
            + (sub ? '<th>SubsidiaryAccount</th>' : '') + '<th>Remarks</th><th>CheqNo</th>'
            + (tmp ? '<th>TotalAmountDr</th><th>TotalAmountCr</th>' : '') + '<th>AmountDr</th><th>AmountCr</th>'
            + (mc ? '<th>DebitFcyAmount</th><th>CreditFcyAmount</th>' : '') + '<th>Delete</th></tr></thead><tbody>';
        var tDr = 0, tCr = 0, fDr = 0, fCr = 0;
        S.table.forEach(function (r, i) {
            tDr += toDbl(r.AmountDr); tCr += toDbl(r.AmountCr); fDr += toDbl(r.DebitFcyAmount); fCr += toDbl(r.CreditFcyAmount);
            h += '<tr' + (i === S.cur ? ' class="cur"' : '') + ' tabindex="-1" data-i="' + i + '" ondblclick="DBO.editRow(' + i + ')">'
                + (tmp ? '<td class="num">' + r.SubCode + '</td>' : '')
                + '<td>' + GD.esc(r.AccountCode) + '</td><td>' + GD.esc(r.AccountTitle) + '</td>'
                + (sub ? '<td>' + GD.esc(r.SubsidiaryAccount) + '</td>' : '')
                + '<td>' + GD.esc(r.Remarks) + '</td><td>' + GD.esc(r.CheqNo) + '</td>'
                + (tmp ? '<td class="num">' + GD.esc(r.TotalAmountDr) + '</td><td class="num">' + GD.esc(r.TotalAmountCr) + '</td>' : '')
                + '<td class="num">' + GD.fmtSingle(r.AmountDr) + '</td><td class="num">' + GD.fmtSingle(r.AmountCr) + '</td>'
                + (mc ? '<td class="num">' + GD.fmt3(r.DebitFcyAmount) + '</td><td class="num">' + GD.fmt3(r.CreditFcyAmount) + '</td>' : '')
                + '<td><button type="button" onclick="DBO.deleteRow(' + i + ')">Delete</button></td></tr>';
        });
        var lead = (tmp ? 1 : 0) + 2 + (sub ? 1 : 0) + 2 + (tmp ? 2 : 0);
        h += '</tbody><tfoot><tr><td colspan="' + lead + '"></td><td class="num">' + GD.fmtSingle(tDr) + '</td><td class="num">' + GD.fmtSingle(tCr) + '</td>'
            + (mc ? '<td class="num">' + GD.fmt3(fDr) + '</td><td class="num">' + GD.fmt3(fCr) + '</td>' : '') + '<td></td></tr></tfoot>';
        $('grdDetail').innerHTML = h;
    }

    // ============================================================================= Reset
    function newForm() {
        S.recId = 0; S.loadedDate = null; S.unique = 1;
        set('txtSubCode', S.unique);
        set('txtDocNo', '');
        set('txtDocDate', today());
        set('cmbCashAccount', '');
        set('txtClosing', ''); set('txtOpening', '');
        set('txtFcyAmount', '');
        set('txtVoucherRemarks', '');
        set('cmbCreditAccount', ''); bindSub('cr');
        set('cmbDebitAccount', ''); bindSub('dr');
        set('txtTotalCrAmount', ''); set('txtTotalDrAmount', '');
        resetDetail();
        $('btnsave').hidden = false; $('btnUpdate').hidden = true;
        $('btnAddDetail').hidden = false; $('btnUpdateDetail').hidden = true; $('btnCancelDetail').hidden = true;
        $('txtDocDate').focus();
        S.table = []; S.cur = -1;
        renderGrid();
        busy('btnNew', function () { return GD.get(API + '/offset/code').then(function (r) { set('txtDocNo', r.voucherCode); }); });
    }

    // =========================================================================== btnRefresh
    function refresh() {
        busy('btnRefresh', function () { return GD.get(API + '/offset/accounts').then(function (d) {
            var keep = { cash: val('cmbCashAccount'), h: val('CmbCashAccountForHistory'), cr: val('cmbCreditAccount'), dr: val('cmbDebitAccount'), cur: val('cmbCurrency') };
            S.cashAccounts = d.cashAccounts || []; S.accounts = d.detailAccounts || [];
            GD.fill('cmbCurrency', d.currencies, 'Id', 'CurrencyCode', null, ''); set('cmbCurrency', keep.cur);
            fillAccounts('cmbCashAccount', S.cashAccounts); set('cmbCashAccount', keep.cash);
            fillAccounts('CmbCashAccountForHistory', S.cashAccounts); set('CmbCashAccountForHistory', keep.h);
            fillAccounts('cmbDebitAccount', S.accounts); set('cmbDebitAccount', keep.dr);
            fillAccounts('cmbCreditAccount', S.accounts); set('cmbCreditAccount', keep.cr);
            fillAccounts('CmbTemporaryAccount', S.accounts);
            S.tempAccountId = d.temporaryAccountId || 0;
            if (S.tempAccountId > 0) set('CmbTemporaryAccount', S.tempAccountId);
            S.baseCurrency = d.baseCurrencyId || 0; S.baseRate = d.baseCurrencyRate || 0;
            defaultConfigurations();
        }); });
    }

    // ================================================================= btnsave_Click / btnUpdate_Click
    function save(isUpdate, acknowledged) {
        if (isBusy('btnsave') || isBusy('btnUpdate')) return;      // a save is already running
        if (isUpdate && S.recId === 0) { alert('Record Id Not Found'); return; }
        if (val('txtDocNo') === '') { alert('DocNo Required'); return; }
        if (!has('cmbCashAccount')) { alert('Cash Account Required'); $('cmbCashAccount').focus(); return; }
        if (S.multiCurrency) {
            if (!has('cmbCurrency')) { alert('Fcy Code Field is Required'); return; }
            if (toDbl(val('txtExchangeRate')) === 0) { alert('Exchange Rate Field is Required'); return; }
            if (toDbl(val('txtFcyAmount')) === 0) { alert('Fcy Amount Field is Required'); return; }
        } else {
            if (!has('cmbCurrency')) { alert('Please Configure Your Base Currency In configurations'); return; }
            if (val('txtExchangeRate').trim() === '' || val('txtExchangeRate').trim() === '0') { alert('Please Configure Your Base Currency Rate In configurations'); return; }
        }
        if (toDbl(val('txtTotalCrAmount')) > 0 || toDbl(val('txtTotalDrAmount')) > 0) {
            alert('There Is A Stand Entry In Detail Record. Please Complete that Entry First'); return;
        }
        if (S.table.length === 0) { alert('Grid Fields Required'); return; }
        if (!acknowledged && !confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var date = val('txtDocDate');
        if (S.loadedDate && GD.iso(S.loadedDate) === date) date = GD.isoTime(S.loadedDate);
        var body = {
            id: S.recId, voucherCode: val('txtDocNo').trim(), voucherDate: date, cashAccountId: toInt(val('cmbCashAccount')),
            remarks: val('txtVoucherRemarks'), currencyId: toInt(val('cmbCurrency')), exchangeRate: val('txtExchangeRate'),
            fcyAmount: val('txtFcyAmount'), temporaryAccountId: toInt(val('CmbTemporaryAccount')),
            duplicateAcknowledged: !!acknowledged,
            rows: S.table.map(function (r) {
                return { subCode: toInt(r.SubCode), accountId: toInt(r.AccountId), againstAccountId: toInt(r.AgainstAccountId),
                    subsidiaryAccountId: toInt(r.SubsidiaryAccountId), subsidiaryAccountTypeId: toInt(r.SubsidiaryAccountTypeId),
                    remarks: str(r.Remarks), cheqNo: str(r.CheqNo), totalAmountDr: toDbl(r.TotalAmountDr), totalAmountCr: toDbl(r.TotalAmountCr),
                    amountDr: toDbl(r.AmountDr), amountCr: toDbl(r.AmountCr), lineId: toInt(r.LineId), cheqId: toInt(r.CheqId),
                    debitFcyAmount: toDbl(r.DebitFcyAmount), creditFcyAmount: toDbl(r.CreditFcyAmount) };
            })
        };
        var preview = $('ChkBoxPrintPreview').checked, format2 = $('chkformat2').checked;
        var again = false;
        busy(isUpdate ? 'btnUpdate' : 'btnsave', function () {
            return GD.api('POST', API + '/offset/save', body).then(function (res) {
                alert(res.message);
                newForm();
                if (preview) slip({ id: res.id });
                else if (format2) GD.get(API + '/slip102/' + res.id).then(function (rows) { GD.printRows('102-AcRptPaymentReceiptsVoucherSlip', rows); }).catch(fail);
            }).catch(function (e) {
                if (e && e.status === 409 && e.confirm === 'duplicate') { again = confirm(e.message); return; }
                fail(e);
            });
        }).then(function () { if (again) save(isUpdate, true); });
    }

    // =============================================================================== History
    function tab(i) {
        $('tabForm').classList.toggle('on', i === 0); $('tabHistory').classList.toggle('on', i === 1);
        $('pageForm').hidden = i !== 0; $('pageHistory').hidden = i !== 1;
        if (i === 1) history();
    }

    function loadVoucherDates() { busy('btnRefreshHistory', function () { return GD.get(API + '/offset/voucher-dates').then(fillVoucherDates); }); }

    /** HistoryFill (:1980) */
    function history() {
        busy('BtnShow', function () { return GD.get(API + '/offset/history', { voucherDate: val('datVouchderDateForHistory'), cashAccountId: val('CmbCashAccountForHistory') || 0 })
            .then(function (rows) {
                if (!rows || rows.length === 0) { $('DataGridCreditHistory').innerHTML = ''; $('DataGridDebitHistory').innerHTML = ''; return; }
                var cr = [], dr = [];
                rows.forEach(function (r) {
                    if (GD.num(GD.col(r, 'CreditAmount')) > 0) cr.push(r);
                    else if (GD.num(GD.col(r, 'DebitAmount')) > 0) dr.push(r);
                });
                renderHistory('DataGridCreditHistory', cr, 'CreditAmount', true);
                renderHistory('DataGridDebitHistory', dr, 'DebitAmount', false);
            }); });
    }

    function renderHistory(grid, rows, amountCol, withUsers) {
        var mc = S.multiCurrency;
        var h = '<thead><tr><th>Edit</th><th>Print</th><th>VoucherCode</th><th>CashAccount</th><th>AccountCode</th><th>DetailAccount</th>'
            + '<th>' + amountCol + '</th>' + (mc ? '<th>FcyAmount</th>' : '') + '<th>Comments</th><th>CheqNoDetail</th>'
            + (withUsers ? '<th>EntryUserName</th><th>EntryDate</th><th>ModifyUserName</th><th>ModifyDate</th>' : '')
            + '<th>NoOfAttachments</th></tr></thead><tbody>';
        rows.forEach(function (r) {
            var id = GD.col(r, 'Id');
            // ToShortDateString() then "dd-MMM-yyyy hh:mm tt": the time always reads 12:00 AM
            var ed = GD.toDate(GD.col(r, 'EntryDate')), md = GD.toDate(GD.col(r, 'ModifyDate'));
            if (ed) ed.setHours(0, 0, 0, 0);
            if (md) md.setHours(0, 0, 0, 0);
            h += '<tr tabindex="-1" data-id="' + id + '"><td><button type="button" onclick="DBO.read(' + id + ')">Edit</button></td>'
                + '<td><button type="button" onclick="DBO.slip({id:' + id + '})">Print</button></td>'
                + '<td><a href="#" class="code" title="Load this voucher" onclick="DBO.read(' + id + ');return false;">' + GD.esc(GD.col(r, 'VoucherCode')) + '</a></td><td>' + GD.esc(GD.col(r, 'CashAccount')) + '</td>'
                + '<td>' + GD.esc(GD.col(r, 'DeailAccountCode')) + '</td><td>' + GD.esc(GD.col(r, 'DetailAccount')) + '</td>'
                + '<td class="num">' + GD.fmtSingle(GD.col(r, amountCol)) + '</td>'
                + (mc ? '<td class="num">' + GD.fmt3(GD.col(r, 'DCurrencyAmount')) + '</td>' : '')
                + '<td>' + GD.esc(GD.col(r, 'Comments')) + '</td><td>' + GD.esc(GD.col(r, 'CheqNoDetail')) + '</td>'
                + (withUsers ? '<td>' + GD.esc(GD.col(r, 'EntryUserName')) + '</td><td>' + (ed ? GD.fmtDate(ed, 'dd-MMM-yyyy hh:mm tt') : '01-Jan-1900 12:00 AM') + '</td>'
                    + '<td>' + GD.esc(GD.col(r, 'ModifyUserName')) + '</td><td>' + (md ? GD.fmtDate(md, 'dd-MMM-yyyy hh:mm tt') : '01-Jan-1900 12:00 AM') + '</td>' : '')
                + '<td class="num"><a href="#" onclick="alert(\'Attachments are not ported to the web.\');return false;">' + GD.esc(GD.col(r, 'NoOfAttachments')) + '</a></td></tr>';
        });
        // No TotalRow on either history grid of this form (only grdDetail has one).
        $(grid).innerHTML = h + '</tbody>';
    }

    /** ReadById (:2145) */
    function read(id) {
        GD.get(API + '/read/' + id).then(function (o) {
            var hd = o.head;
            S.recId = id;
            $('btnsave').hidden = true; $('btnUpdate').hidden = false;
            tab(0);
            S.loadedDate = GD.toDate(GD.col(hd, 'VoucherDate'));
            set('txtDocDate', GD.iso(S.loadedDate));
            set('txtDocNo', GD.col(hd, 'VoucherCode'));
            set('cmbCashAccount', GD.col(hd, 'RefAccountId'));
            cashLeave();
            set('txtVoucherRemarks', GD.col(hd, 'Remarks'));
            set('cmbCurrency', GD.col(hd, 'MultiCurrencyId'));
            set('txtExchangeRate', GD.fmtRate(GD.col(hd, 'ExchangeCurrencyRate')));
            set('txtFcyAmount', GD.fmt3(GD.col(hd, 'FcAmount')));
            if (toInt(GD.col(hd, 'MultiCurrencyId')) === 0 || toDbl(GD.col(hd, 'ExchangeCurrencyRate')) === 0) defaultConfigurations();
            S.table = (o.details || []).map(function (x) {
                var dr = toDbl(GD.col(x, 'DebitAmount')), crA = toDbl(GD.col(x, 'CreditAmount'));
                return {
                    SubCode: toInt(GD.col(x, 'SubNo')), AccountCode: str(GD.col(x, 'AccountCode')), AccountId: toInt(GD.col(x, 'AccountId')),
                    AccountTitle: str(GD.col(x, 'AccountTitle')), AgainstAccountId: toInt(GD.col(x, 'AgainstAccountId')),
                    SubsidiaryAccountId: toInt(GD.col(x, 'SubsidiaryAccountId')), SubsidiaryAccount: str(GD.col(x, 'SubsidiaryAccountTitle')),
                    SubsidiaryAccountTypeId: toInt(GD.col(x, 'SubsidiaryTypeId')), Remarks: str(GD.col(x, 'Comments')),
                    CheqNo: str(GD.col(x, 'CheqNoDetail')), TotalAmountDr: toDbl(GD.col(x, 'TotalDebitAmount')),
                    TotalAmountCr: toDbl(GD.col(x, 'TotalCreditAmount')), AmountDr: dr, AmountCr: crA,
                    LineId: toInt(GD.col(x, 'LineId')), CheqId: toInt(GD.col(x, 'InvoiceNoRefId')),
                    DebitFcyAmount: dr > 0 ? toDbl(GD.col(x, 'DCurrencyAmount')) : 0,
                    CreditFcyAmount: crA > 0 ? toDbl(GD.col(x, 'DCurrencyAmount')) : 0
                };
            });
            S.cur = -1;
            S.unique = S.table.length ? Math.max.apply(null, S.table.map(function (r) { return r.SubCode; })) + 1 : 1;
            set('txtSubCode', S.unique);
            rateChanged();
        }).catch(fail);
    }

    /** CommonServices.DayBookSlip(id) / btnPrint144_Click -> Sp_DayBookSlip */
    function slip(p) {
        GD.get(API + '/slip144', p).then(function (rows) { GD.printRows('144-DayBookSlip', rows); }).catch(fail);
    }

    function print144() {
        slip({ accountId: val('CmbCashAccountForHistory') || 0, skipCash: $('chkZkipCashTransaction').checked });
    }

    function shortcuts() {
        alert(['Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh', 'Ctrl+N  For New',
            'Ctrl+P  For Print in History', 'Ctrl+F5  For Focus on Voucher Date', 'Ctrl+F10  For Open Attachments',
            'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+T  For Tab Transfer', 'Ctrl+ArrowDown  For Focus On Detail Grid',
            "Ctrl+ArrowRight  For Focus On History Grid's", 'Ctrl+ArrowUp  For Focus On Credit Account in Detail Box',
            'Ctrl+Enter  When Focus On Any Grid For Update Record',
            "Ctrl+Space  When Focus On Any Grid To Call Function's On Button Or Link"].join('\n'));
    }

    // ============================================================== frmDayBook_KeyDown
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName)) {
            var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) {
                return !x.disabled && !x.hidden && x.offsetParent !== null && x.tabIndex >= 0;
            });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return;
        }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase(), onForm = !$('pageForm').hidden;
        if (k === 'n' && onForm) { e.preventDefault(); newForm(); }
        else if (k === 's') { e.preventDefault(); if (!$('btnsave').hidden && !$('btnsave').disabled) save(false); }
        else if (k === 'u') { e.preventDefault(); if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) save(true); }
        else if (k === 't') { e.preventDefault(); tab(onForm ? 1 : 0); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'p') { e.preventDefault(); if (!$('btnPrint144').disabled) print144(); }
        else if (e.key === 'F5') { e.preventDefault(); (onForm ? $('txtDocDate') : $('datVouchderDateForHistory')).focus(); }
        else if (e.key === 'ArrowUp') { e.preventDefault(); $('cmbCreditAccount').focus(); }
        else if (e.key === 'ArrowDown') { e.preventDefault(); if (onForm) firstRow('grdDetail'); }          // focus on the detail grid
        else if (e.key === 'ArrowRight' && !onForm) {                                                       // history grids, one then the other
            e.preventDefault();
            var inCr = $('DataGridCreditHistory').contains(document.activeElement);
            firstRow(inCr ? 'DataGridDebitHistory' : 'DataGridCreditHistory');
        }
        else if (e.key === 'Enter' || e.key === ' ' || e.code === 'Space') {
            // grdDetail_KeyDown / DataGrid*History_KeyDown: Ctrl+Enter = edit / load the row,
            // Ctrl+Space = the button of the current column (Delete asks first, as on the desktop).
            var tr = e.target && e.target.closest ? e.target.closest('tbody tr') : null;
            var tb = tr ? tr.closest('table') : null;
            if (!tb || ['grdDetail', 'DataGridCreditHistory', 'DataGridDebitHistory'].indexOf(tb.id) < 0) return;
            e.preventDefault();
            if (e.key === 'Enter') {
                if (tb.id === 'grdDetail') editRow(+tr.getAttribute('data-i'));
                else read(+tr.getAttribute('data-id'));
            } else {
                var b = e.target.closest('button, a');
                if (b) b.click();
            }
        }
    });

    window.DBO = {
        tab: tab, newForm: newForm, refresh: refresh, save: save, cashLeave: cashLeave, creditLeave: creditLeave,
        debitLeave: debitLeave, subPicked: subPicked, crRemarksLeave: crRemarksLeave, currencyLeave: currencyLeave,
        rateChanged: rateChanged, rateLeave: rateLeave, addDetail: addDetail, updateDetail: updateDetail,
        cancelDetail: cancelDetail, editRow: editRow, deleteRow: deleteRow, history: history, read: read,
        slip: slip, print144: print144, loadVoucherDates: loadVoucherDates, shortcuts: shortcuts,
        commas: function (el) {
            var raw = el.value.replace(/[^0-9.]/g, '');
            var parts = raw.split('.');
            var intPart = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            var v = parts.length > 1 ? intPart + '.' + parts.slice(1).join('') : intPart;
            if (v !== el.value) el.value = v;
        }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', load); else load();
}());
