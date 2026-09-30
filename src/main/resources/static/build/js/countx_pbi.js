/* ============================================================================================
 * countx_pbi.js - screen 41 "Payment By Invoice Voucher New"
 *   Architecture.WinApp.Account_Definition.PaymentByInvoiceVoucherNew.cs + LoadPendingInvoicesByPayment.cs
 * Built on countx_hrm.js (window.HRM); exports window.Pbi (the toolbar / grid handlers).
 *
 * Every handler follows its desktop method line by line: Leave / ValueChanged / CheckedChanged / TextChanged
 * events, the grid's CellUpdated and ColumnButtonClick, the validation wording and order, what New / Save /
 * Update / Edit do to the buttons, and the number formats ("0,0", "#,#", double.ToString()).
 * A Leave that calls the server is awaited by Add / Save / Update, as the desktop runs Leave before the click.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/accounts/payment-by-invoice';
    var P = {};
    window.Pbi = P;

    // ------------------------------------------------------------------------ state (the form's fields)
    var Id = 0;                 // public static int Id
    var UpdateMode = false;
    var rights = {};
    var dtAccount = [];         // DetailAccountFill rows (grid AccountId value list)
    var dtJobLot = [];          // combojoblotfill rows (grid JobLotId value list)
    var dtInvoiceNos = [];      // InvoiceNoFill rows
    var startPeriod = null;
    var tabIndex = 0;
    var pending = Promise.resolve();       // the Leave handlers still running
    var grd, hist, detail;

    // ------------------------------------------------------------------------ .NET formats
    /** Math.Round(v, d) - MidpointRounding.ToEven. */
    function roundEven(v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p;
        var r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    }
    /** Round half away from zero (custom numeric formats). */
    function roundAway(v) { var n = HRM.num(v); return (n < 0 ? -1 : 1) * Math.round(Math.abs(n)); }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /** double.ToString("0,0"): whole number, grouped, at least two digits ("05", "00"). */
    function fmt00(v) {
        var r = roundAway(v), s = String(Math.abs(r));
        if (s.length < 2) s = ('00' + s).slice(-2);
        return (r < 0 ? '-' : '') + group(s);
    }
    /** "#,#" / "#,##": whole number, grouped, zero shows nothing. */
    function fmtHash(v) {
        if (v === null || v === undefined || v === '') return '';
        var r = roundAway(v);
        return r === 0 ? '' : (r < 0 ? '-' : '') + group(String(Math.abs(r)));
    }
    /** double.ToString(): the shortest text ("12.5", "50"). */
    function dbl(v) { var n = HRM.num(v); return isFinite(n) ? String(n) : (n > 0 ? '∞' : '-∞'); }
    function pctTax(amount, pct) { return roundEven(HRM.num(amount) / (100.0 - HRM.num(pct)) * 100.0 * HRM.num(pct) / 100.0, 2); }

    // ------------------------------------------------------------------------ small helpers
    function el(id) { return HRM.$(id); }
    function hasRow(id) { return HRM.comboVal(id) !== 0; }                       // UltraCombo.ActiveRow != null
    function vdate() { return HRM.val('voucherdatetime') || HRM.today(); }
    function docType() { return HRM.comboVal('combvtype') === 1 ? 1 : 2; }       // Text == "CPV" ? 1 : 2
    function saveOn() { return HRM.visible('Save') && !el('Save').disabled; }    // Save.Visible && Save.Enabled
    function numCol(key, caption, width) {
        return { key: key, caption: caption, width: width, type: 'num', decimals: 0, sum: true, align: 'right',
                 render: function (v) { return HRM.esc(fmtHash(v)); } };
    }
    function after(p) { pending = pending.then(function () { return p; }).catch(function () { }); return p; }
    function titleOf(rows, id, key) {
        for (var i = 0; i < rows.length; i++) if (HRM.int(HRM.col(rows[i], 'Id')) === HRM.int(id)) return HRM.str(HRM.col(rows[i], key));
        return '';
    }
    /** Fills a combo; `first` activates Rows[0] as the desktop does after BindDDL / BindDDLNew. */
    function fill(id, rows, v, t, first, keep) {
        HRM.fill(id, rows || [], v, t, { zero: '', keep: !!keep });
        if (first && rows && rows.length && !(keep && HRM.comboVal(id))) HRM.setCombo(id, HRM.col(rows[0], v));
    }
    /** UltraCombo.Leave: focus leaving the combo's wrap (built by countx_prod_combo.js after this runs). */
    function onLeave(id, fn) {
        document.addEventListener('focusout', function (e) {
            var w = e.target && e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
            if (!w || !w.querySelector('#' + id)) return;
            if (e.relatedTarget && w.contains(e.relatedTarget)) return;
            setTimeout(function () { after(fn()); }, 0);
        });
    }

    // ------------------------------------------------------------------------ grids
    function buildGrids() {
        // table columns (Load) + grdSettings(); Delete "X" appended last
        grd = new HRM.Grid('grd', {
            totals: true, emptyText: '',
            columns: [
                { key: 'RefDocumentTypeId', caption: 'RefDocumentTypeId', hidden: true },
                { key: 'InvoiceId', caption: 'InvoiceId', hidden: true },
                { key: 'InvoiceNo', caption: 'InvoiceNo', width: 80 },
                { key: 'AccountId', caption: 'Account Title', width: 250, render: function (v) { return HRM.esc(titleOf(dtAccount, v, 'AccountTitle')); } },
                { key: 'JobLotId', caption: 'JobLot', width: 80, type: 'select',
                  options: function () { return [['', '']].concat(dtJobLot.map(function (r) { return [HRM.str(HRM.col(r, 'Id')), HRM.str(HRM.col(r, 'JobLotDescription'))]; })); } },
                { key: 'Remarks', caption: 'Remarks', width: 240, type: 'edit' },
                numCol('WHT', 'WHT', 80),
                numCol('InvoiceAmount', 'InvoiceAmount', 100),
                numCol('TotalPaidAmount', 'TotalPaidAmount', 100),
                numCol('BalanceAmount', 'BalanceAmount', 100),
                { key: 'Amount', caption: 'Amount', width: 100, type: 'edit-num', sum: true, decimals: 0 },
                { key: 'Delete', caption: 'X', width: 20, render: function () { return '<button type="button" class="pbi-x" data-act="delete" tabindex="-1">X</button>'; } }
            ],
            onChange: function (r, k) {                                                  // grd_CellUpdated -> Total()
                if (k === 'Amount') r.Amount = HRM.num(r.Amount);
                if (k === 'JobLotId') r.JobLotId = HRM.int(r.JobLotId);
                after(Total());
            }
        });
        grd._totals = (function (orig) {                                               // TotalFormatString "#,##"
            return function () {
                orig.call(this);
                var tf = this.table.tFoot; if (!tf) return;
                var self = this;
                Array.prototype.forEach.call(tf.querySelectorAll('td'), function (td, i) {
                    var c = self.columns[i]; if (c && c.sum) td.textContent = fmtHash(self.sum(c.key, true));
                });
            };
        })(grd._totals);
        el('grd').addEventListener('click', function (e) {                             // grd_ColumnButtonClick "Delete"
            var b = e.target.closest('button[data-act="delete"]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            grd.remove(+tr.getAttribute('data-i'));
            after(Total());
        });

        // DataGridHistory: HistoryFill dtHistory + HistoryGridSettings (View / Print / Edit appended)
        hist = new HRM.Grid('DataGridHistory', {
            filterRow: true, totals: true, emptyText: '',
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
                { key: 'VoucherCode', caption: 'VoucherCode' },
                { key: 'DocumentTypeCode', caption: 'DocumentTypeCode' },
                { key: 'VoucherDate', caption: 'VoucherDate', type: 'date' },
                { key: 'AccountTitle', caption: 'AccountTitle', width: 300 },
                { key: 'Remarks', caption: 'Remarks', width: 150 },
                { key: 'VoucherAmount', caption: 'VoucherAmount', width: 120, type: 'num', sum: true, decimals: 0, align: 'right',
                  render: function (v) { return HRM.esc(fmt00(v)); } },
                { key: 'CheqNo', caption: 'CheqNo' },
                { key: 'UserName', caption: 'UserName' },
                { key: 'Attachment', caption: 'Attachment', type: 'code' },            // ColumnType Link -> LinkClicked
                { key: 'View', caption: 'View', width: 50, render: function () { return cellBtn('view', 'View'); } },
                { key: 'Print', caption: 'Print', width: 50, render: function () { return cellBtn('print', 'Print'); } },
                { key: 'Edit', caption: 'Edit', width: 50, render: function () { return cellBtn('edit', 'Edit'); } }
            ],
            onCode: function (r) { showAttachments(HRM.int(HRM.col(r, 'Id'))); }
        });
        hist._totals = (function (orig) {
            return function () {
                orig.call(this);
                var tf = this.table.tFoot; if (!tf) return;
                var self = this;
                Array.prototype.forEach.call(tf.querySelectorAll('td'), function (td, i) {
                    var c = self.columns[i]; if (c && c.sum) td.textContent = fmt00(self.sum(c.key, true));
                });
            };
        })(hist._totals);
        el('DataGridHistory').addEventListener('click', function (e) {                // DataGridHistory_ColumnButtonClick
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'); hist.select(i);
            var r = hist.rows()[i], id = HRM.int(HRM.col(r, 'Id')), act = b.getAttribute('data-act');
            if (act === 'edit') HRM.busy(b, function () { return Reset().then(function () { return HistoryGridFill(id); }); }, 'hist-edit').catch(HRM.fail);
            else if (act === 'view') HRM.busy(b, function () { return viewDetail(id); }, 'hist-view').catch(HRM.fail);
            else if (act === 'print') printSlip(id, b);
        });

        detail = new HRM.Grid('grdDetail', {
            filterRow: true, totals: true, emptyText: '',
            columns: [
                { key: 'InvoiceNo', caption: 'InvoiceNo', width: 100 },
                { key: 'AccountTitle', caption: 'AccountTitle', width: 250 },
                { key: 'Job/Lot', caption: 'Job/Lot', width: 120 },
                { key: 'Remarks', caption: 'Remarks', width: 560 },
                numCol('WHT', 'WHT'), numCol('DebitAmount', 'DebitAmount'), numCol('CreditAmount', 'CreditAmount')
            ]
        });
        detail._totals = grd._totals;
    }
    function cellBtn(act, text) { return '<button type="button" class="pbi-cellbtn" data-act="' + act + '">' + text + '</button>'; }

    // ------------------------------------------------------------------------ text boxes whose TextChanged runs code
    /** txtValue.TextChanged -> textBox1_TextChanged -> checkBox1_CheckedChanged. */
    function setValueText(s) {
        var e = el('txtValue');
        if (e.value === s) return Promise.resolve();
        e.value = s;
        return WhtChanged();
    }
    /** ChkBoxWthHolding.Checked = on (CheckedChanged fires only on a change). */
    function setWht(on) {
        var e = el('ChkBoxWthHolding');
        if (e.checked === !!on) return Promise.resolve();
        e.checked = !!on;
        return WhtChanged();
    }

    // ------------------------------------------------------------------------ Load
    function load() {
        buildGrids();
        HRM.fillFixed('combvtype', [[1, 'CPV'], [2, 'BPV']]);                          // DocumentTypeFill, Rows[0].Activate
        HRM.check('ChkBox', true);
        HRM.setVal('voucherdatetime', HRM.today());
        HRM.setVal('CheqDate', HRM.today());
        return HRM.loading(HRM.get(API + '/setup')).then(function (s) {
            rights = s.rights || {};
            // Save.Enabled / Print.Enabled / Update.Enabled = rights
            el('Save').disabled = !rights.save;
            el('Print').disabled = !rights.print;
            el('Update').disabled = !rights.update;
            if (!rights.save) el('Save').title = 'You do not have save rights on this screen';
            if (!rights.print) el('Print').title = 'You do not have print rights on this screen';
            if (!rights.update) el('Update').title = 'You do not have update rights on this screen';
            startPeriod = s.startPeriod;
            bindCombos(s, false);
            HRM.setVal('txtvoucherno', saveOn() ? HRM.str(s.voucherCode) : '');         // VoucherNofill
            HRM.focus('combvtype');
        }).catch(HRM.fail);
    }

    /** CombCompanyFill .. BindTaxTypes (Load and btnRefresh_Click). */
    function bindCombos(s, keep) {
        fill('combcomp', s.companies, 'Id', 'CompName', true, false);
        fill('combbranch', s.branches, 'Id', 'BranchName', true, false);
        fill('combproject', s.projects, 'Id', 'ProjectName', true, false);
        dtJobLot = s.jobLots || [];
        fill('CmbJobLot', dtJobLot, 'Id', 'JobLotDescription', true, false);
        fill('CmbAgainstAc', s.againstAccounts, 'Id', 'AccountTitle', false, keep);
        fill('CmbWithHoldingAc', s.againstAccounts, 'Id', 'AccountTitle', false, keep);
        dtAccount = s.detailAccounts || [];
        fill('combactitle', dtAccount, 'Id', 'AccountTitle', false, keep);
        fill('CmbTaxType', s.taxTypes, 'Id', 'TaxName', false, keep);
        grd.draw();
    }

    // ------------------------------------------------------------------------ header events
    /** combvtype_Leave: AccountTitleFill; VoucherNofill when Save is visible and enabled. */
    function combvtypeLeave() {
        return HRM.get(API + '/voucher-type', { documentTypeId: docType(), newVoucher: saveOn() }).then(function (r) {
            fill('combcreditac', r.accounts, 'Id', 'AccountTitle', false, true);
            if (saveOn() && r.voucherCode !== undefined && r.voucherCode !== null) HRM.setVal('txtvoucherno', HRM.str(r.voucherCode));
        }).catch(HRM.fail);
    }

    /** combcreditac_Leave: txtbalance visible, AccountCurrentBalance, CheqNoFill. */
    function combcreditacLeave() {
        HRM.show('txtbalance', true);
        return HRM.get(API + '/credit-account', { accountId: HRM.comboVal('combcreditac'), voucherDate: vdate(), documentTypeId: docType() }).then(function (r) {
            if (r.balance !== null && r.balance !== undefined) {
                var n = HRM.num(r.balance);
                HRM.text('txtbalance', dbl(r.balance));
                HRM.text('lbldrcr', n === 0 ? 'Nill' : n > 0 ? 'Dr' : 'Cr');
                HRM.show('lbldrcr', true);
            }
            if (r.cheques) fill('CmbCheqNo', r.cheques, 'Id', 'CheqNo', false, false);
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ detail events
    /** combactitle_Leave: DetailAccountCurrentBalance, InvoiceNoFill, GetInvoiceBalanceAmount. */
    function combactitleLeave() {
        HRM.show('lblbalancedetail', true);
        return HRM.get(API + '/detail-account', { accountId: HRM.comboVal('combactitle'), voucherDate: vdate(), invoiceId: HRM.comboVal('CmbInvoiceNo') }).then(function (r) {
            if (r.balance !== null && r.balance !== undefined) {
                var t = fmt00(r.balance), n = HRM.num(t);
                HRM.text('lblbalancedetail', t);
                HRM.text('lbldetaildrcr', n > 0 ? 'Dr' : n === 0 ? 'Nill' : 'Cr');
                HRM.show('lbldetaildrcr', true);
            }
            dtInvoiceNos = r.invoices || [];
            if (dtInvoiceNos.length) {
                fill('CmbInvoiceNo', dtInvoiceNos, 'Id', 'VoucherCode', false, false);
            } else {
                fill('CmbInvoiceNo', [], 'Id', 'VoucherCode', false, false);            // Text = "", DataSource = null
                HRM.setVal('txtBalanceAmount', '');
            }
            showInvoiceBalance(r.invoiceBalance);
        }).catch(HRM.fail);
    }

    /** GetInvoiceBalanceAmount: three read-only boxes, "0,0" or "0". */
    function showInvoiceBalance(b) {
        if (b) {
            HRM.setVal('txtInvoiceAmount', fmt00(b.InvoiceAmount));
            HRM.setVal('txtTotalPaidAmount', fmt00(b.TotalPaidAmount));
            HRM.setVal('txtBalanceAmount', fmt00(b.BalanceAmount));
        } else {
            HRM.setVal('txtInvoiceAmount', '0'); HRM.setVal('txtTotalPaidAmount', '0'); HRM.setVal('txtBalanceAmount', '0');
        }
    }
    function GetInvoiceBalanceAmount() {
        return HRM.get(API + '/invoice-balance', { accountId: HRM.comboVal('combactitle'), invoiceId: HRM.comboVal('CmbInvoiceNo') })
            .then(showInvoiceBalance).catch(HRM.fail);
    }

    /** CmbInvoiceNo_ValueChanged: txtRefDocumentTypeId from dtInvoiceNos, GetInvoiceBalanceAmount. */
    function cmbInvoiceNoChanged() {
        if (dtInvoiceNos.length) {
            var id = HRM.comboVal('CmbInvoiceNo');
            dtInvoiceNos.forEach(function (r) { if (HRM.int(HRM.col(r, 'Id')) === id) HRM.setVal('txtRefDocumentTypeId', HRM.str(HRM.col(r, 'DocumentTypeId'))); });
            return GetInvoiceBalanceAmount();
        }
        HRM.setVal('txtRefDocumentTypeId', '0');
        return Promise.resolve();
    }

    // ------------------------------------------------------------------------ totals and WHT
    /** Total(). */
    function Total() {
        var rows = grd.rows(), amount = 0;
        var p = Promise.resolve();
        if (rows.length) {
            for (var i = 0; i < rows.length; i++) {
                var num = HRM.num(rows[i].Amount), bal = HRM.num(rows[i].BalanceAmount);
                if (num > bal) {
                    rows[i].Amount = 0; grd.draw();
                    HRM.box('DebitAmount not greater than BalanceAmount!');
                    return p;
                }
                amount += HRM.num(rows[i].Amount);
            }
        } else {
            p = setValueText('').then(function () { HRM.setVal('txtTaxAmount', ''); HRM.setVal('txtTotalAmount', ''); });
        }
        return p.then(function () {
            if (amount > 0) {
                return setValueText(fmt00(amount)).then(function () {
                    var taxAmount = HRM.num(HRM.val('txtTaxAmount'));
                    var TotalAmt = roundEven(amount + taxAmount, 2);
                    HRM.setVal('txtTotalAmount', fmt00(TotalAmt));
                });
            }
        });
    }

    /** WHTTaxPropotion(). */
    function WHTTaxPropotion() {
        var pct = HRM.val('txtTaxPercent');
        grd.rows().forEach(function (r) { r.WHT = HRM.checked('ChkBoxWthHolding') ? pctTax(r.Amount, pct) : 0; });
        grd.draw();
    }

    /** checkBox1_CheckedChanged (also CmbTaxType_Leave and txtValue.TextChanged). */
    function WhtChanged() {
        if (HRM.checked('ChkBoxWthHolding')) {
            return HRM.get(API + '/tax-schedule', { taxTypeId: HRM.comboVal('CmbTaxType'), voucherDate: vdate() }).then(function (s) {
                if (!s || !s.found) {
                    HRM.enable('CmbAgainstAc', false); HRM.enable('CmbWithHoldingAc', false);
                    HRM.setCombo('CmbWithHoldingAc', 0);
                    HRM.setVal('txtTaxPercent', '0'); HRM.setVal('txtTaxAmount', '0');
                    WHTTaxPropotion();
                    return;
                }
                HRM.setVal('txtTaxPercent', HRM.str(s.TaxPercent));
                if (!UpdateMode) HRM.setCombo('CmbWithHoldingAc', s.TaxGLAccountId);
                var pct = HRM.num(HRM.val('txtTaxPercent'));
                var taxamount = roundEven(HRM.num(HRM.val('txtValue')) / (100.0 - pct) * 100.0 * pct / 100.0, 2);
                HRM.setVal('txtTaxAmount', dbl(taxamount));
                HRM.setVal('txtTotalAmount', fmt00(taxamount + HRM.num(HRM.val('txtValue'))));
                WHTTaxPropotion();
                HRM.enable('CmbAgainstAc', true); HRM.enable('CmbWithHoldingAc', true);
            }).catch(HRM.fail);
        }
        HRM.enable('CmbAgainstAc', false); HRM.enable('CmbWithHoldingAc', false);
        HRM.setVal('txtTaxPercent', '0'); HRM.setVal('txtTaxAmount', '0');
        HRM.setVal('txtTotalAmount', fmt00(HRM.num(HRM.val('txtValue')) + HRM.num(HRM.val('txtTaxAmount'))));
        WHTTaxPropotion();
        return Promise.resolve();
    }

    // ------------------------------------------------------------------------ Add
    /** Add_Click_1. */
    P.Add = function (btn) {
        return HRM.busy(btn, function () {
            return pending.then(function () {
                if (!hasRow('CmbInvoiceNo')) { HRM.box('InvoiceNo Field Required'); HRM.focus('CmbInvoiceNo'); return; }
                var amt = HRM.val('txtamount').trim();
                if (amt === '' || HRM.num(amt) === 0) { HRM.box('Amount Field Required'); HRM.focus('txtamount'); return; }
                if (!hasRow('combactitle')) { HRM.box('Account Title Field Required'); HRM.focus('combactitle'); return; }
                if (!hasRow('CmbJobLot')) { HRM.box('Job/Lot Field Required'); HRM.focus('CmbJobLot'); return; }
                var BalanceAmt = 0, Amount = 0;
                var b = HRM.val('txtBalanceAmount');
                if (b.trim() !== '' && b !== '0') BalanceAmt = HRM.num(b);
                if (HRM.num(amt) > 0) Amount = HRM.num(amt);
                if (Amount > BalanceAmt) { HRM.box('Debit Amount not greater than Balance Amount!'); return; }
                var InvoiceId = HRM.comboVal('CmbInvoiceNo'), AccountId = HRM.comboVal('combactitle');
                var rows = grd.rows();
                for (var i = 0; i < rows.length; i++) {
                    if (InvoiceId === HRM.int(rows[i].InvoiceId)) { HRM.box('InvoiceNo Already add in grid please check!'); return; }
                    if (AccountId !== HRM.int(rows[i].AccountId)) { HRM.box('You cannot entered another Party!'); return; }
                }
                grd.add({
                    RefDocumentTypeId: HRM.val('txtRefDocumentTypeId').trim(), InvoiceId: InvoiceId, InvoiceNo: HRM.comboText('CmbInvoiceNo'),
                    AccountId: AccountId, JobLotId: HRM.comboVal('CmbJobLot'), Remarks: HRM.val('txtremarks').trim(), WHT: 0,
                    InvoiceAmount: HRM.num(HRM.val('txtInvoiceAmount')), TotalPaidAmount: HRM.num(HRM.val('txtTotalPaidAmount')),
                    BalanceAmount: HRM.num(HRM.val('txtBalanceAmount')), Amount: HRM.num(amt)
                });
                HRM.show('lblbalancedetail', false); HRM.show('lbldetaildrcr', false);
                HRM.focus('combactitle');
                return Total().then(function () {
                    HRM.setVal('txtamount', '');
                    HRM.setCombo('CmbInvoiceNo', 0);
                    HRM.setVal('txtBalanceAmount', '0');
                    HRM.setVal('txtRefDocumentTypeId', '0');
                    WHTTaxPropotion();
                });
            });
        }).catch(HRM.fail);
    };

    // ------------------------------------------------------------------------ Save / Update
    /** FormValidation(). */
    function FormValidation() {
        if (!hasRow('combcreditac')) { HRM.box('Account Field is Required'); HRM.focus('combcreditac'); return false; }
        if (!hasRow('combcomp')) { HRM.box('Company Name Field is Required'); HRM.focus('combcomp'); return false; }
        if (!hasRow('combbranch')) { HRM.box('Branch Name Field is Required'); HRM.focus('combbranch'); return false; }
        if (!hasRow('combproject')) { HRM.box('Project Name Field is Required'); HRM.focus('combproject'); return false; }
        if (!hasRow('combvtype')) { HRM.box('Voucher Type Field is Required'); HRM.focus('combvtype'); return false; }
        return true;
    }
    /** The WHT checks and the per-row checks shared by Save_Click / Update_Click; false when a message was shown. */
    function detailChecks() {
        if (HRM.checked('ChkBoxWthHolding')) {
            if (!hasRow('CmbAgainstAc')) { HRM.box('Against Account Field is Required'); HRM.focus('CmbAgainstAc'); return false; }
            if (!hasRow('CmbWithHoldingAc')) { HRM.box('WithHolding Account Field is Required'); HRM.focus('CmbWithHoldingAc'); return false; }
            if (!hasRow('CmbTaxType')) { HRM.box('TaxType Field is Required'); HRM.focus('CmbTaxType'); return false; }
        }
        var rows = grd.rows();
        for (var i = 0; i < rows.length; i++) {
            var r = rows[i];
            if (HRM.num(r.Amount) > 0) {
                if (HRM.num(r.Amount) > HRM.num(r.InvoiceAmount)) { HRM.box('DebitAmount not greater than InvoiceAmount!'); return false; }
                if (HRM.int(r.AccountId) === 0) { HRM.box('Please Select Account Title First'); return false; }
                continue;
            }
            HRM.box('Debit Amount Field Required');
            return false;
        }
        return true;
    }
    /** The VoucherHead + voucherDetailList the form builds (the server rebuilds the lines from these values). */
    function payload(id) {
        return {
            Id: id,
            CompanyId: HRM.comboVal('combcomp'), BranchId: HRM.comboVal('combbranch'), ProjectId: HRM.comboVal('combproject'),
            DocumentTypeId: HRM.comboVal('combvtype'), VoucherCode: HRM.int(HRM.val('txtvoucherno').trim()), VoucherDate: vdate(),
            RefAccountId: HRM.comboVal('combcreditac'), ChequeDate: HRM.val('CheqDate') || HRM.today(),
            CheqId: HRM.comboVal('CmbCheqNo'), ChequeNo: HRM.comboText('CmbCheqNo'),
            PayTitle: HRM.val('txtPayTitle').trim(), Remarks: HRM.val('txtremarksmain').trim(),
            IncludeWHT: HRM.checked('ChkBoxWthHolding'),
            WhtAgainstAccountId: HRM.comboVal('CmbAgainstAc'), WhtAccountId: HRM.comboVal('CmbWithHoldingAc'),
            TaxTypeId: HRM.comboVal('CmbTaxType'), TaxPercent: HRM.val('txtTaxPercent').trim(),
            rows: grd.rows().map(function (r) {
                return { RefDocumentTypeId: HRM.int(r.RefDocumentTypeId), InvoiceId: HRM.int(r.InvoiceId), InvoiceNo: HRM.str(r.InvoiceNo),
                         AccountId: HRM.int(r.AccountId), JobLotId: HRM.int(r.JobLotId), Remarks: HRM.str(r.Remarks), WHT: HRM.num(r.WHT),
                         InvoiceAmount: HRM.num(r.InvoiceAmount), TotalPaidAmount: HRM.num(r.TotalPaidAmount),
                         BalanceAmount: HRM.num(r.BalanceAmount), Amount: HRM.num(r.Amount) };
            })
        };
    }
    P.payload = function () { return payload(Id); };

    /** Save_Click. */
    P.Save = function (btn) {
        return HRM.busy(btn || 'Save', function () {
            return pending.then(function () {
                Id = 0;
                if (!grd.rows().length) { HRM.box('Grid Record not found'); return; }
                if (!FormValidation()) return;
                if (!HRM.ask('Are you sure to Save?')) return;
                if (!detailChecks()) return;
                return HRM.post(API + '/save', payload(0)).then(function (r) {
                    HRM.box(r.message);
                    var success = HRM.int(r.id);
                    return Reset().then(function () {
                        Id = 0; grd.clear();
                        if (HRM.checked('ChkBox')) return printSlip(success, null);
                    });
                });
            });
        }, 'pbi-save').catch(HRM.fail);
    };

    /** Update_Click. */
    P.Update = function (btn) {
        return HRM.busy(btn || 'Update', function () {
            return pending.then(function () {
                if (Id === 0) { HRM.box('Record Not Found For Update  ' + Id); return; }
                if (!grd.rows().length) { HRM.box('Grid Record not found'); return; }
                if (!FormValidation()) return;
                if (!HRM.ask('Are you sure to Update?')) return;
                if (!detailChecks()) return;
                return HRM.post(API + '/save', payload(Id)).then(function (r) {
                    HRM.box(r.message);
                    var success = HRM.int(r.id);
                    return Reset().then(function () {
                        if (HRM.checked('ChkBox')) return printSlip(success, null);
                    });
                });
            });
        }, 'pbi-save').catch(HRM.fail);
    };

    // ------------------------------------------------------------------------ New / Reset / Refresh
    /** Reset(). */
    function Reset() {
        Id = 0;
        grd.clear();
        UpdateMode = false;
        HRM.setCombo('CmbInvoiceNo', 0);
        HRM.setVal('txtremarks', ''); HRM.setVal('txtamount', ''); HRM.setVal('txtPayTitle', ''); HRM.setVal('txtremarksmain', '');
        HRM.setCombo('CmbTaxType', 0);
        return setValueText('').then(function () {                                   // txtValue.Clear() -> TextChanged
            HRM.setVal('txtTaxAmount', ''); HRM.setVal('txtTotalAmount', '');
            HRM.setCombo('combactitle', 0);
            HRM.setCombo('CmbCheqNo', 0);
            HRM.show('Save', true); HRM.show('Update', false);
            HRM.focus('combactitle');
            return Total();
        }).then(function () {
            return combvtypeLeave();                                                   // AccountTitleFill + VoucherNofill
        }).then(function () {
            return combcreditacLeave();
        }).then(function () { grd.clear(); });
    }

    /** btnNew_Click. */
    P.New = function (btn) {
        return HRM.busy(btn || 'New', function () {
            return Reset().then(function () {
                HRM.setCombo('combcreditac', 0);
                HRM.show('txtbalance', false); HRM.show('lbldrcr', false);
            });
        }, 'pbi-new').catch(HRM.fail);
    };

    /** btnRefresh_Click. */
    P.Refresh = function (btn) {
        return HRM.busy(btn, function () {
            return HRM.get(API + '/refresh', { documentTypeId: docType() }).then(function (s) {
                bindCombos(s, true);
                if (saveOn()) HRM.setVal('txtvoucherno', HRM.str(s.voucherCode));
            });
        }).catch(HRM.fail);
    };

    // ------------------------------------------------------------------------ edit (History "Edit")
    /** HistoryGridFill(ID). */
    function HistoryGridFill(ID) {
        grd.clear();
        Id = ID;
        return HRM.get(API + '/by-id', { id: ID }).then(function (d) {
            var vh = d.head, list = d.details || [];
            HRM.show('Save', false); HRM.show('Update', true);
            UpdateMode = true;
            HRM.setCombo('combcomp', vh.CompanyId); HRM.setCombo('combbranch', vh.BranchId); HRM.setCombo('combproject', vh.ProjectId);
            P.Tab(0);
            HRM.setCombo('combvtype', HRM.int(vh.DocumentTypeId) === 1 ? 1 : 2);
            return combvtypeLeave().then(function () {
                HRM.setVal('voucherdatetime', HRM.day(vh.VoucherDate));
                HRM.setCombo('combcreditac', vh.RefAccountId);
                return combcreditacLeave();
            }).then(function () {
                HRM.setVal('txtPayTitle', HRM.str(vh.PayTitle));
                if (HRM.int(vh.CheqId) > 0 && HRM.str(vh.ChequeNo) !== '') {
                    fill('CmbCheqNo', [{ Id: vh.CheqId, CheqNo: vh.ChequeNo }], 'Id', 'CheqNo', true, false);
                }
                HRM.setVal('CheqDate', HRM.day(vh.ChequeDate) || HRM.today());
                HRM.setVal('txtremarksmain', HRM.str(vh.Remarks));
                HRM.setVal('txtvoucherno', HRM.str(vh.VoucherCode));
                var ref = HRM.int(vh.RefAccountId);
                var rows = [];
                list.forEach(function (x) {
                    if (HRM.str(x.IsTaxable) === 'False' && HRM.int(x.AccountId) !== ref) {
                        rows.push({ RefDocumentTypeId: x.DocumentTypeIdRef, InvoiceId: x.OrderNo, InvoiceNo: x.RefInvoiceNo, AccountId: x.AccountId,
                                    JobLotId: HRM.int(x.JobLotId), Remarks: HRM.str(x.Comments), WHT: HRM.num(x.WhtHolding), InvoiceAmount: HRM.num(x.QtyIn),
                                    TotalPaidAmount: HRM.num(x.QtyOut), BalanceAmount: HRM.num(x.ItemAmount), Amount: HRM.num(x.DebitAmount) });
                    }
                });
                grd.set(rows);
                return setValueText(dbl(vh.VoucherAmount)).then(function () {
                    // the tax loop, row by row as the form runs it
                    var chain = Promise.resolve();
                    list.forEach(function (x) {
                        chain = chain.then(function () {
                            if (HRM.str(x.IsTaxable) === 'True') {
                                if (HRM.num(x.DebitAmount) > 0) HRM.setCombo('CmbAgainstAc', x.AccountId);
                                if (HRM.num(x.CreditAmount) > 0) {
                                    HRM.setCombo('CmbWithHoldingAc', x.AccountId);
                                    HRM.setCombo('CmbTaxType', x.TaxTypeId);
                                    HRM.setVal('txtTaxPercent', dbl(x.TaxPrcnt));
                                    HRM.setVal('txtTaxAmount', dbl(x.TaxesTotalAmount));
                                }
                                return setWht(true).then(Total);
                            }
                            HRM.setVal('txtTaxPercent', '0'); HRM.setVal('txtTaxAmount', '0');
                            return setWht(false).then(WhtChanged).then(Total);
                        });
                    });
                    return chain;
                });
            }).then(function () { grd.draw(); return Total(); });
        });
    }

    // ------------------------------------------------------------------------ History tab
    /** HistoryFill(NoofRecords). */
    function HistoryFill(n) {
        return HRM.get(API + '/history', { noOfRecords: n || 0 }).then(function (rows) {
            if (rows && rows.length) hist.set(rows);
            else { detail.clear(); hist.clear(); }
        });
    }
    P.LoadAll = function (btn) { return HRM.busy(btn, function () { return HistoryFill(0); }).catch(HRM.fail); };

    /** DataGridHistory "View": the voucher's lines in grdDetail. */
    function viewDetail(id) {
        return HRM.get(API + '/by-id', { id: id }).then(function (d) {
            var t = [];
            (d.details || []).forEach(function (x) {
                var tax = HRM.str(x.IsTaxable), dr = HRM.num(x.DebitAmount), cr = HRM.num(x.CreditAmount);
                var base = { InvoiceNo: x.RefInvoiceNo, AccountTitle: x.AccountTitle, 'Job/Lot': x.JobLotDescription, Remarks: x.Comments };
                if (dr > 0 && tax === 'False') t.push(Object.assign({}, base, { WHT: HRM.num(x.WhtHolding), DebitAmount: dr, CreditAmount: 0 }));
                if (cr > 0 && tax === 'False') t.push(Object.assign({}, base, { WHT: 0, DebitAmount: 0, CreditAmount: cr }));
                if (tax === 'True') t.push(Object.assign({}, base, { WHT: 0, DebitAmount: dr, CreditAmount: cr }));
            });
            detail.set(t);
        });
    }

    /** tabControl1_SelectedIndexChanged: History -> HistoryFill(50). */
    P.Tab = function (i) {
        if (i === tabIndex) return;
        tabIndex = i;
        el('pageForm').classList.toggle('is-active', i === 0);
        el('pageHistory').classList.toggle('is-active', i === 1);
        el('tabForm').classList.toggle('is-active', i === 0);
        el('tabHistory').classList.toggle('is-active', i === 1);
        if (i === 1) HRM.busy('btnLoadAll', function () { return HistoryFill(50); }, 'pbi-hist').catch(HRM.fail);
    };

    // ------------------------------------------------------------------------ print (132-PaymentByInvoiceSlipNew_Report.rpt)
    function printSlip(id, btn) {
        var win = null;
        try { win = window.open('', '_blank'); if (win) win.document.write('<p style="font:13px Segoe UI,sans-serif;padding:16px">Preparing report...</p>'); } catch (e) { win = null; }
        var run = function () {
            return HRM.get(API + '/print-check', { id: id }).then(function () {
                return fetch('/reports/print/132-payment-by-invoice-slip-new-report', {
                    method: 'POST', credentials: 'same-origin',
                    headers: csrf({ 'Content-Type': 'application/json', 'Accept': 'application/pdf, text/plain' }),
                    body: JSON.stringify({ success: String(id) })
                });
            }).then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') >= 0) return r.blob().then(function (b) {
                    var url = URL.createObjectURL(b);
                    if (win) win.location.href = url; else window.open(url, '_blank');
                });
                return r.text().then(function (t) { throw new Error(t || ('Request failed (' + r.status + ')')); });
            }).catch(function (e) { try { if (win) win.close(); } catch (x) { } throw e; });
        };
        return btn ? HRM.busy(btn, run).catch(HRM.fail) : run().catch(HRM.fail);
    }
    function csrf(h) {
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    /** Print_Click -> GenerateReport(): the slip of the loaded voucher (Id). */
    P.Print = function (btn) { return printSlip(Id, btn); };

    // ------------------------------------------------------------------------ attachments
    function showAttachments(id) {
        return HRM.get(API + '/attachments', { id: id }).then(function (rows) {
            var html = rows && rows.length
                ? '<ol class="pbi-attach-list">' + rows.map(function (r) { return '<li>' + HRM.esc(HRM.col(r, 'UploadedFileCustomName') || HRM.col(r, 'Attachment')) + '</li>'; }).join('') + '</ol>'
                : '<p class="pbi-attach-list">No attachment.</p>';
            HRM.modal({ title: 'Attachments', width: 'min(520px, 96vw)', html: html });
        }).catch(HRM.fail);
    }
    /** toolStripButton10_Click -> AT.Show(): uploading is not available on the web page; the loaded voucher's files are listed. */
    P.Attachment = function () {
        if (Id > 0) return showAttachments(Id);
        HRM.box('Attachments can be added from the desktop application only.');
    };

    // ------------------------------------------------------------------------ Load Invoices popup
    /** btnLoadInvoices_Click: LoadPendingInvoicesByPayment.ShowDialog(), then LoadInGridDetail(). */
    P.LoadInvoices = function () {
        var dtInvoices = null;                                                         // null = closed without Load
        var m = HRM.modal({
            title: 'Purchase Invoices Load For Payment', width: 'min(1040px, 98vw)', backdropClose: false,
            onClose: function () {                                                     // ShowDialog() returned -> LoadInGridDetail()
                if (dtInvoices && dtInvoices.length && dtInvoices[0].Id !== 0) after(LoadInGridDetail(dtInvoices));
            },
            html: '<div class="pbi-canvas pbi-panel2" style="--h:62px">' +
                '<div class="pbi-f"><label class="pbi-a pbi-lbl" for="ldFromDate" style="--x:12px;--y:8px">From Date</label>' +
                '<input type="date" id="ldFromDate" class="pbi-a win-textbox" style="--x:15px;--y:32px;--w:95px;--h:21px"></div>' +
                '<div class="pbi-f"><label class="pbi-a pbi-lbl" for="ldToDate" style="--x:112px;--y:8px">To Date</label>' +
                '<input type="date" id="ldToDate" class="pbi-a win-textbox" style="--x:114px;--y:32px;--w:102px;--h:21px"></div>' +
                '<div class="pbi-f"><label class="pbi-a pbi-lbl" for="ldAccount" style="--x:216px;--y:8px">Account Title</label>' +
                '<div class="pbi-a" style="--x:219px;--y:29px;--w:323px"><select id="ldAccount" class="win-combo dtcombo" data-dtcombo="single" data-dtcombo-caption="AccountTitle"></select></div></div>' +
                '<div class="pbi-f pbi-buttons">' +
                '<button type="button" id="ldSearch" class="pbi-a pbi-btn" style="--x:548px;--y:31px;--w:61px">Search</button>' +
                '<button type="button" id="ldLoad" class="pbi-a pbi-btn" style="--x:612px;--y:31px;--w:58px">Load</button>' +
                '<button type="button" id="ldReset" class="pbi-a pbi-btn" style="--x:673px;--y:32px;--w:58px">Reset</button></div>' +
                '</div>' +
                '<div class="hrm-grid-box"><div class="hrm-grid-wrap"><table id="ldGrid"></table></div></div>'
        });
        m.el.classList.add('pbi-loader');
        var g = new HRM.Grid('ldGrid', {
            filterRow: true, totals: true, checkAll: 'Select', emptyText: '',
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'RefAccountId', caption: 'RefAccountId', hidden: true },
                { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
                { key: 'InvoiceDate', caption: 'InvoiceDate', type: 'date' },
                { key: 'InvoiceNo', caption: 'InvoiceNo' },
                numCol('InvoiceAmount', 'InvoiceAmount'), numCol('TotalPaidAmount', 'TotalPaidAmount'), numCol('BalanceAmount', 'BalanceAmount'),
                { key: 'Select', caption: 'Select', type: 'edit-check' }                 // ActAsSelector + UseHeaderSelector
            ]
        });
        g._totals = grd._totals;
        HRM.refreshCombos();
        HRM.setVal('ldFromDate', startPeriod || HRM.today());                          // FromDate = ActiveYr.Start_Period
        HRM.setVal('ldToDate', HRM.today());
        function search() {                                                            // PendingPurchaseInvoiceForLoad
            return HRM.get(API + '/loader/search', { accountId: HRM.comboVal('ldAccount'), fromDate: HRM.val('ldFromDate'), toDate: HRM.val('ldToDate') })
                .then(function (rows) { g.set(rows || []); });
        }
        // frmLoadGRN_Load_1: PendingPurchaseInvoiceForLoad(), then AccountTitleFill()
        HRM.loading(search().then(function () {
            return HRM.get(API + '/loader/setup').then(function (s) {
                HRM.fill('ldAccount', s.accounts || [], 'GlAccountId', 'AccountTitle', { zero: '' });
            });
        })).catch(HRM.fail);
        el('ldSearch').addEventListener('click', function () { HRM.busy(this, search).catch(HRM.fail); });
        el('ldReset').addEventListener('click', function () { HRM.setCombo('ldAccount', 0); g.clear(); });   // button1_Click
        el('ldLoad').addEventListener('click', function () {                          // btnLoadOnInvoice_Click_1
            var checked = g.checked('Select');
            if (!checked.length) { HRM.box('No Row is Selected'); return; }
            dtInvoices = checked.map(function (r) { return { Id: HRM.int(HRM.col(r, 'Id')), AccountId: HRM.int(HRM.col(r, 'RefAccountId')) }; });
            m.close();
        });
    };

    /** LoadInGridDetail(). */
    function LoadInGridDetail(dtInvoices) {
        var ids = dtInvoices.map(function (r) { return r.Id; });
        var AcId = dtInvoices[dtInvoices.length - 1].AccountId;
        return HRM.busy('btnLoadInvoices', function () {
            return HRM.post(API + '/loader/load', { ids: ids, accountId: AcId }).then(function (dt) {
                if (!dt || !dt.length) return;                                         // grd.ClearStructure() - see the report
                for (var j = 0; j < dt.length; j++) {
                    var rows = grd.rows();
                    for (var i = 0; i < rows.length; i++) {
                        if (HRM.int(rows[i].InvoiceId) === HRM.int(dt[j].InvoiceId)) { grd.draw(); HRM.box('InvoiceNo Already add in grid please check!'); return Total(); }
                        if (HRM.int(rows[i].AccountId) !== HRM.int(dt[j].AccountId)) { grd.draw(); HRM.box('You cannot entered another Party!'); return Total(); }
                    }
                    var x = dt[j];
                    grd.data.push({ RefDocumentTypeId: x.RefDocumentTypeId, InvoiceId: x.InvoiceId, InvoiceNo: x.InvoiceNo, AccountId: x.AccountId,
                                    JobLotId: HRM.int(x.JobLotId), Remarks: HRM.str(x.Remarks), WHT: HRM.num(x.WHT), InvoiceAmount: HRM.num(x.InvoiceAmount),
                                    TotalPaidAmount: HRM.num(x.TotalPaidAmount), BalanceAmount: HRM.num(x.BalanceAmount), Amount: HRM.num(x.Amount) });
                }
                grd.draw();
                return Total();
            });
        }, 'pbi-load').catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ wiring
    function wire() {
        onLeave('combvtype', combvtypeLeave);
        onLeave('combcreditac', combcreditacLeave);
        onLeave('combactitle', combactitleLeave);
        onLeave('CmbInvoiceNo', GetInvoiceBalanceAmount);                             // CmbInvoiceNo_Leave
        onLeave('CmbTaxType', WhtChanged);                                            // CmbTaxType_Leave
        el('CmbInvoiceNo').addEventListener('change', function () { after(cmbInvoiceNoChanged()); });
        el('ChkBoxWthHolding').addEventListener('change', function () { after(WhtChanged()); });
        el('txtamount').addEventListener('keypress', function (e) {                   // txtamount_KeyPress: digits only
            if (e.key && e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault();
        });
        HRM.keys({                                                                     // PaymentVoucherNew_KeyDown
            'ctrl+s': function () { if (saveOn() && tabIndex === 0) P.Save(); },
            'ctrl+n': function () { P.New(); },
            'ctrl+t': function () { P.Tab(tabIndex === 1 ? 0 : 1); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('Update') && !el('Update').disabled) P.Update(); },
            'ctrl+f10': function () { P.Attachment(); },
            'ctrl+l': function () { P.LoadInvoices(); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        wire();
        load();
    });
})();
