/* ============================================================================================
 * countx_fixedassets_purchase.js - 918 "Fixed Asset Purchase / Opening"
 * (Architecture.WinApp.Account_Definition.frmFixedAssetPurchase, DocumentTypeId 130).
 * Built on countx_hrm.js (window.HRM) + countx_fixedassets_common.js (window.FA); exports window.FaPur.
 * API /api/fixed-assets/fixed-asset-purchase/{setup|next-code|history-accounts|custom-accounts|last-rate|
 * tax-schedule|balance|save|by-id|history|history-detail|print-check}. The server (FaPurchaseService) repeats
 * every validation and builds the ledger lines; this page keeps the form's grid, calculations and flow.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/fixed-assets/fixed-asset-purchase';
    var P = {};
    window.FaPur = P;

    var L = {}, rights = {}, subFeature = false, loaded = false;
    var RecId = 0, UpdateMode = false, updateDetailIndex = -1;
    var SubsdiaryAccountDatasourceLengthDetail = 0, SubsdiaryAccountDatasourceLengthCr = 0;
    var BaseCurrency = 0, BaseRate = 0;
    var ACC3 = ['AccountCode', 'NoteTitle'];
    var SUBCOLS = ['SubsidiaryType', 'Code'];

    function AD() { return HRM.int(L.decimals && L.decimals.amount); }
    function RD() { var r = HRM.int(L.decimals && L.decimals.rate); return r > 0 ? r : 2; }
    function FD() { return HRM.int(L.decimals && L.decimals.fcyAmount); }
    function rateText(v) { return FA.fixed(v, RD()); }                         // clsGlobalVariables.DecimalRateFormate
    function hashes(v) { var n = FA.roundAway(v, 0); return n === 0 ? '' : HRM.fmtNum(n, 0); }   // .ToString("#,##")
    function guard(fn) { return function (e) { if (!FA.dialogOpen()) fn(e); }; }

    // ============================================================================ grids
    function btn(act, text) { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + act + '" style="min-width:0;width:auto;height:20px;padding:0 6px;font-size:8pt">' + text + '</button>'; }
    function onButtons(grid, tableId, fn) {
        HRM.$(tableId).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), i, b);
        });
    }
    /** DetailGridSetting():1702 */
    var grd = new HRM.Grid('grd', {
        columns: [
            { key: 'AccountCode', caption: 'AccountCode' },
            { key: 'AccId', hidden: true },
            { key: 'Account', caption: 'Account', width: 220 },
            { key: 'SubsidiaryAccountId', hidden: true },
            { key: 'SubsidiaryAccount', caption: 'SubsidiaryAccount' },
            { key: 'SubsidiaryAccountTypeId', hidden: true },
            { key: 'JobLotId', hidden: true },
            { key: 'JobLot', caption: 'JobLot' },
            { key: 'InvoiceNo', caption: 'InvoiceNo' },
            { key: 'Qty', caption: 'Qty', type: 'num', sum: true, decimals: 3, render: function (v) { return HRM.esc(FA.fmt(v, 3)); } },
            { key: 'Rate', caption: 'Rate', type: 'num', render: function (v) { return HRM.esc(rateText(v)); } },
            { key: 'Amount', caption: 'Amount', type: 'num', sum: true, render: function (v) { return HRM.esc(HRM.fmtNum(v, AD())); } },
            { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', render: function (v) { return HRM.esc(FA.fmt(v, 3)); } },
            { key: 'ReferenceAccountId', hidden: true },
            { key: 'ReferenceAccount', caption: 'ReferenceAccount' },
            { key: 'Remarks', caption: 'Remarks', width: 200 },
            { key: 'Delete', caption: 'X', width: 20, render: function () { return btn('del', 'X'); } }
        ],
        filterRow: true, totals: true,
        onDouble: function (r, i) { grdDetail_DoubleClick(i); }
    });
    onButtons(grd, 'grd', function (r, act, i) { if (act === 'del') grdDetail_Delete(i); });
    /** grd / grdDetailHistory: SubsidiaryAccount visible only with ERP feature 4 (DetailGridSetting / VoucherDetailByHeaderId). */
    function setHidden(grid, key, hidden) {
        var idx = -1;
        grid.columns.forEach(function (c, i) { if (c.key === key) { c.hidden = hidden; idx = i; } });
        if (idx < 0 || !grid.table || !grid.table.tHead) return;
        Array.prototype.forEach.call(grid.table.tHead.rows, function (tr) { if (tr.cells[idx]) tr.cells[idx].style.display = hidden ? 'none' : ''; });
        grid.draw();
    }
    function subsidiaryColumn() { setHidden(grd, 'SubsidiaryAccount', !subFeature); }

    /** ScheduleGridSetting():1859 - DueDays / DueDate (calendar) / Due% / Amount editable; X and +. */
    var grdSchedule = new HRM.Grid('grdSchedule', {
        columns: [
            { key: 'DueDays', caption: 'DueDays', type: 'edit', width: 70, redraw: true, onChange: function (r) {       // grdSchedule_CellUpdated DueDays
                r.DueDate = HRM.addDays(HRM.val('txtDocDate') || HRM.today(), HRM.int(r.DueDays));
            } },
            { key: 'DueDate', caption: 'DueDate', type: 'edit-date', width: 100 },
            { key: 'Due%', caption: 'Due%', type: 'edit-num', width: 50, redraw: true, onChange: function (r) {          // grdSchedule_CellUpdated Due%
                r.Amount = Math.round(gridTotal() * (HRM.num(r['Due%']) / 100) * 100) / 100;
            } },
            { key: 'Amount', caption: 'Amount', type: 'edit-num', width: 80, sum: true },
            { key: 'Delete', caption: 'X', width: 20, render: function () { return btn('del', 'X'); } },
            { key: 'Add', caption: '+', width: 20, render: function () { return btn('add', '+'); } }
        ],
        totals: true
    });
    onButtons(grdSchedule, 'grdSchedule', function (r, act, i) {                        // grdSchedule_ColumnButtonClick
        if (act === 'del') { grdSchedule.remove(i); if (!grdSchedule.rows().length) AddRowInScheduleGrid(); }
        if (act === 'add') AddRowInScheduleGrid();
    });

    function gridTotal() { return grd.sum('Amount'); }
    /** AddRowInScheduleGrid():1832 */
    function AddRowInScheduleGrid() {
        var rows = grdSchedule.rows();
        if (rows.length > 0) {
            var totalPrcnt = grdSchedule.sum('Due%');
            if (totalPrcnt !== 100) {
                var percent = 100 - totalPrcnt;
                grdSchedule.add({ DueDays: '', DueDate: '', 'Due%': percent, Amount: gridTotal() * percent / 100 });
            }
        } else grdSchedule.add({ DueDays: '', DueDate: '', 'Due%': 0, Amount: 0 });
    }
    /** UpdateDataInScheduleGrid():1809 */
    function UpdateDataInScheduleGrid() {
        var amount = gridTotal();
        grdSchedule.rows().forEach(function (r) { r.Amount = HRM.num(r['Due%']) * amount / 100; });
        grdSchedule.draw();
    }

    // ============================================================================ binds
    function LocationTypeBind(rows) {                                             // LocationTypeBind():633 - first real row active
        var keep = HRM.comboVal('cmbLocationType');
        HRM.fill('cmbLocationType', rows, 'Id', 'Location', {});
        if (keep && HRM.hasOption('cmbLocationType', keep)) HRM.setCombo('cmbLocationType', keep);
        else if ((rows || []).length) HRM.setCombo('cmbLocationType', HRM.col(rows[0], 'Id'));
    }
    function CostCenterBind(rows) {                                               // CostCenterBind():656 - Rows[0].Activate
        var keep = HRM.comboVal('CmbProjectId');
        HRM.fill('CmbProjectId', rows, 'Id', 'ProjectName', {});
        if (keep && HRM.hasOption('CmbProjectId', keep)) HRM.setCombo('CmbProjectId', keep);
        else if ((rows || []).length) HRM.setCombo('CmbProjectId', HRM.col(rows[0], 'Id'));
    }
    function byTypes(csv) {
        var set = {}; String(csv).split(',').forEach(function (x) { var n = parseInt(x, 10); if (!isNaN(n)) set[n] = true; });
        return (L.allAccounts || []).filter(function (a) { return set[HRM.int(a.AccountTypeId)]; });
    }
    function CreditAccountAccountBind() { FA.fillCols('cmbAccount', byTypes('3'), 'Id', 'AccountTitle', ACC3, { keep: true }); }
    function bindAll(first) {
        LocationTypeBind(L.locationTypes);
        CostCenterBind(L.costCenters);
        HRM.fill('CmbAccountType', L.accountTypes, 'Id', 'AccountType', { keep: true });                        // AccountTypeBind()
        if (!first && HRM.comboVal('CmbAccountType') > 0) CmbAccountType_Leave(); else CreditAccountAccountBind();
        FA.fillCols('CmbTaxAccount', L.taxAccounts, 'Id', 'AccountTitle', ACC3, { keep: true });                // SalesTaxAccountBind()
        HRM.fill('cmbCurrency', L.currencies, 'Id', 'CurrencyCode', { keep: true });                             // CurrencyBind()
        var g = HRM.comboVal('CmbCustomGroup');
        HRM.fill('CmbCustomGroup', L.customGroups, 'Id', 'CustomGroupName', {});                                 // CustomGroupBind() - first row active
        if (g && HRM.hasOption('CmbCustomGroup', g)) HRM.setCombo('CmbCustomGroup', g);
        else if ((L.customGroups || []).length) HRM.setCombo('CmbCustomGroup', HRM.col(L.customGroups[0], 'Id'));
        HRM.fill('cmbJobLotFile', L.jobLots, 'Id', 'JobLotDescription', { keep: true });                         // JobLotBind()
        FA.fillCols('CmbReferenceAccount', L.referenceAccounts, 'Id', 'AccountTitle', ACC3, { keep: true });    // ReferenceAccountBind()
        HRM.fill('CmbTaxType', L.taxTypes, 'Id', 'TaxName', { keep: true });                                     // TaxTypesBind()
        HRM.fill('CmbEntryType', L.entryTypes, 'Id', 'type', { keep: true });                                    // PurchaseTypesBind()
        FA.fillCols('CmbAdvanceTaxAccount', L.advanceTaxAccounts, 'Id', 'AccountTitle', ACC3, { keep: true });  // AdvanceTaxAccountBind()
        DefaultConfigurations();
        return HRM.comboVal('CmbCustomGroup') > 0 ? CmbCustomGroup_Leave() : Promise.resolve();
    }
    /** DefaultConfigurations():584 - Job/Lot, Base Currency, BaseCurrencyRate. */
    function DefaultConfigurations() {
        var d = L.defaults || {};
        if (HRM.int(d.jobLotId)) HRM.setCombo('cmbJobLotFile', HRM.int(d.jobLotId));
        BaseCurrency = HRM.int(d.currencyId);
        if (BaseCurrency) HRM.setCombo('cmbCurrency', BaseCurrency);
        BaseRate = HRM.num(d.exchangeRate);
        if (d.exchangeRate !== undefined && d.exchangeRate !== null) HRM.setVal('txtExchangeRate', rateText(BaseRate));
    }
    function StatusFillForHistory() {                                                 // StatusFillForHistory():2864
        HRM.fillFixed('cmbApproveStatus', [['1', 'Not Apporved'], ['2', 'Approved'], ['3', 'All']]);
        HRM.setCombo('cmbApproveStatus', 1);
    }
    function ComboBindForHistory(rows) {                                              // ComboBindForHistory():2909
        HRM.fill('CmbAccountTitleHistory', rows, 'Id', 'AccountTitle', { keep: true });
    }

    // ============================================================================ Leave handlers
    function balance(accountId, lbl, drcr) {                                          // Detail/CreditAccountCurrentBalance
        return HRM.get(API + '/balance', { accountId: accountId, date: HRM.val('txtDocDate') }).then(function (d) {
            if (!d || !d.found) return;
            var set = function (id, v) { var e = HRM.$(id); if (!e) return; if ('value' in e && e.tagName === 'INPUT') e.value = v; else e.textContent = v; };
            set(lbl, HRM.str(d.Balance));
            var n = HRM.num(d.Balance);
            set(drcr, n === 0 ? 'Nill' : (n > 0 ? 'Dr' : 'Cr'));
        }).catch(HRM.fail);
    }
    /** CmbCustomGroup_Leave():1189 -> DetailAccountsBind() (also the Other Charges account list). */
    function CmbCustomGroup_Leave() {
        var id = HRM.comboVal('CmbCustomGroup');
        if (id <= 0) return Promise.resolve();
        return HRM.get(API + '/custom-accounts', { customGroupId: id }).then(function (rows) {
            FA.fillCols('cmbAccountDetail', rows, 'Id', 'AccountTitle', ['AccountCode', 'AccountType', 'NoteTitle'], { keep: true });
            FA.fillCols('CmbOtherChargesAccount', rows, 'Id', 'AccountTitle', ['AccountCode', 'AccountType', 'NoteTitle'], { keep: true });
        }).catch(HRM.fail);
    }
    function subsidiaryRows(accountId) {
        var all = L.subsidiaries || [];
        return accountId ? all.filter(function (s) { return HRM.int(s.AccountId) === accountId; }) : all;
    }
    /** BindSubsidiaryAccountDrWithDbCall():958 */
    function BindSubsidiaryAccountDr() {
        var all = L.subsidiaries || [];
        if (!all.length) { FA.fillCols('CmbSubsidiaryAccount', [], 'Id', 'SubsidiaryAccount', SUBCOLS, {}); SubsdiaryAccountDatasourceLengthDetail = 0; return; }
        var acc = HRM.comboVal('cmbAccountDetail');
        if (acc !== 0) {
            var r = HRM.comboRow('cmbAccountDetail', 'Id'), t = r ? HRM.int(HRM.col(r, 'AccountTypeId')) : 0;
            HRM.show('PanelSubsidiaryDetail', t !== 2 && t !== 15 && subFeature);
            var keep = HRM.val('CmbSubsidiaryAccount');
            var m = subsidiaryRows(acc);
            SubsdiaryAccountDatasourceLengthDetail = m.length;
            FA.fillCols('CmbSubsidiaryAccount', m, 'Id', 'SubsidiaryAccount', SUBCOLS, {});
            if (m.length && keep && HRM.hasOption('CmbSubsidiaryAccount', keep)) HRM.setCombo('CmbSubsidiaryAccount', keep);
        } else {
            SubsdiaryAccountDatasourceLengthDetail = all.length;
            FA.fillCols('CmbSubsidiaryAccount', all, 'Id', 'SubsidiaryAccount', SUBCOLS, {});
        }
    }
    /** BindSubsidiaryAccountCrWithDbCall():1040 */
    function BindSubsidiaryAccountCr() {
        var all = L.subsidiaries || [];
        if (!all.length) { FA.fillCols('CmbSubsidiaryAccountCr', [], 'Id', 'SubsidiaryAccount', SUBCOLS, {}); SubsdiaryAccountDatasourceLengthCr = 0; return; }
        var acc = HRM.comboVal('cmbAccount');
        if (acc !== 0) {
            var keep = HRM.val('CmbSubsidiaryAccountCr');
            var m = subsidiaryRows(acc);
            SubsdiaryAccountDatasourceLengthCr = m.length;
            FA.fillCols('CmbSubsidiaryAccountCr', m, 'Id', 'SubsidiaryAccount', SUBCOLS, {});
            if (m.length && keep && HRM.hasOption('CmbSubsidiaryAccountCr', keep)) HRM.setCombo('CmbSubsidiaryAccountCr', keep);
        } else {
            SubsdiaryAccountDatasourceLengthCr = all.length;
            FA.fillCols('CmbSubsidiaryAccountCr', all, 'Id', 'SubsidiaryAccount', SUBCOLS, {});
        }
    }
    /** cmbAccountDetail_Leave():1110 */
    function cmbAccountDetail_Leave() {
        var note = '';
        var r = HRM.comboRow('cmbAccountDetail', 'Id');
        if (r && HRM.comboVal('cmbAccountDetail') > 0) {
            HRM.text('lblbalancedetail', ''); HRM.text('lbldetaildrcr', '');
            balance(HRM.comboVal('cmbAccountDetail'), 'lblbalancedetail', 'lbldetaildrcr');
            note = HRM.str(HRM.col(r, 'NoteTitle'));
        } else { HRM.text('lblbalancedetail', ''); HRM.text('lbldetaildrcr', ''); }
        HRM.text('label29', note);
        BindSubsidiaryAccountDr();
    }
    /** CmbSubsidiaryAccount_Leave():1137 - the subsidiary's GL account becomes the detail account. */
    function CmbSubsidiaryAccount_Leave() {
        var r = HRM.comboRow('CmbSubsidiaryAccount', 'Id');
        if (r && HRM.comboVal('CmbSubsidiaryAccount') !== 0) {
            var gl = HRM.int(HRM.col(r, 'AccountId'));
            if (gl !== 0) HRM.setCombo('cmbAccountDetail', gl);
        }
    }
    /** CmbSubsidiaryAccountDr_Leave():1156 (wired to the header Subsidiary A/C). */
    function CmbSubsidiaryAccountCr_Leave() {
        var r = HRM.comboRow('CmbSubsidiaryAccountCr', 'Id');
        if (r && HRM.comboVal('CmbSubsidiaryAccountCr') !== 0) {
            var gl = HRM.int(HRM.col(r, 'AccountId'));
            if (gl !== 0) HRM.setCombo('cmbAccount', gl);
        }
    }
    /** cmbAccount_Leave():1175 */
    function cmbAccount_Leave() {
        HRM.setVal('lblCreditAcBalance', '0'); HRM.text('lblDrCrS', '');
        if (HRM.comboVal('cmbAccount')) balance(HRM.comboVal('cmbAccount'), 'lblCreditAcBalance', 'lblDrCrS');
        BindSubsidiaryAccountCr();
    }
    /** CmbAccountType_Leave():1222 */
    function CmbAccountType_Leave() {
        var t = HRM.comboVal('CmbAccountType');
        if (t > 0) FA.fillCols('cmbAccount', byTypes(String(t)), 'Id', 'AccountTitle', ACC3, { keep: true });
        else CreditAccountAccountBind();
    }
    /** cmbCurrency_Leave():1319 */
    function cmbCurrency_Leave() {
        var c = HRM.comboVal('cmbCurrency');
        if (c === 0) return;
        if (c !== BaseCurrency) {
            HRM.get(API + '/last-rate', { currencyId: c }).then(function (rows) {
                HRM.setVal('txtExchangeRate', rows && rows.length ? rateText(HRM.num(HRM.col(rows[0], 'LastExchRate'))) : '0');
                txtExchangeRate_TextChanged();
            }).catch(HRM.fail);
        } else { HRM.setVal('txtExchangeRate', rateText(BaseRate)); txtExchangeRate_TextChanged(); }
    }
    /** txtExchangeRate_TextChanged():1417 - every row's FCY = Amount / rate (rounded), then the total. */
    function txtExchangeRate_TextChanged() {
        var rate = HRM.num(HRM.val('txtExchangeRate'));
        grd.rows().forEach(function (r) { r.FcyAmount = rate > 0 ? FA.roundEven(HRM.num(r.Amount) / rate, FD()) : 0; });
        grd.draw();
        CalculateTotalInformation();
    }
    function CalculateTotalInformation() { HRM.setVal('txtFcyAmount', FA.fmt(grd.sum('FcyAmount'), 3)); }   // "#,##0.###"

    // ============================================================================ calculations
    /** Calculation():2584 - Amount = Qty x Rate (both non-zero), Math.Round AwayFromZero. */
    function Calculation() {
        var q = HRM.num(HRM.val('txtQtyDetial')), r = HRM.num(HRM.val('txtRateDetail'));
        var a = (q !== 0 && r !== 0) ? q * r : 0;
        HRM.setVal('txtAmountDetail', FA.fixed(a, AD()));
        related('amount');
    }
    /** CommonServices.UpdateRelatedFieldsCalculation(txtAmountDetail, txtFcyAmountDetail, rate, ...). */
    function related(src) {
        var rate = HRM.num(HRM.val('txtExchangeRate'));
        if (rate === 0) { HRM.box('Please enter exchange Rate First'); HRM.focus('txtExchangeRate'); return; }
        if (src === 'amount') {
            var a = HRM.num(HRM.val('txtAmountDetail'));
            HRM.setVal('txtFcyAmountDetail', a !== 0 ? FA.fmt(FA.roundAway(a / rate, FD()), 3, false) : '0');
        } else {
            var f = HRM.num(HRM.val('txtFcyAmountDetail'));
            HRM.setVal('txtAmountDetail', f !== 0 ? FA.fixed(FA.roundAway(f * rate, AD()), AD()) : '0');
        }
    }
    /** salesTex():2749 */
    var taxSeq = 0;
    function salesTex() {
        var tt = HRM.comboVal('CmbTaxType'), seq = ++taxSeq;
        if (tt > 0) {
            return HRM.get(API + '/tax-schedule', { taxTypeId: tt, date: HRM.val('txtDocDate') }).then(function (rows) {
                if (seq !== taxSeq) return;
                if (rows && rows.length) {
                    var d = rows[0];
                    HRM.setVal('txtTaxPercent', HRM.str(HRM.col(d, 'TaxPercent')));
                    if (!UpdateMode || HRM.comboVal('CmbTaxAccount') === 0) {
                        var idd = HRM.int(HRM.col(d, 'TaxGLAccountId'));
                        if (idd > 0 && HRM.hasOption('CmbTaxAccount', idd)) HRM.setCombo('CmbTaxAccount', idd);
                    }
                    var gt = gridTotal(), pct = HRM.num(HRM.val('txtTaxPercent'));
                    HRM.setVal('txtTaxAmount', String(gt > 0 && pct > 0 ? Math.round(gt * pct / 100 * 100) / 100 : 0));
                } else { HRM.setVal('txtTaxPercent', '0'); HRM.setVal('txtTaxAmount', '0'); }
                TotalAmountInfooter();
            }).catch(HRM.fail);
        }
        HRM.setVal('txtTaxPercent', '0'); HRM.setVal('txtTaxAmount', '0');
        TotalAmountInfooter();
        return Promise.resolve();
    }
    /** TotalAmountInfooter():2805 */
    function TotalAmountInfooter() {
        var gt = gridTotal();
        HRM.setVal('txtVoucherAmountFooter', hashes(gt));
        HRM.setVal('txtTotalAmount', hashes(gt + HRM.num(HRM.val('txtTaxAmount')) + HRM.num(HRM.val('txtAdvanceTaxAmount')) + HRM.num(HRM.val('txtOtherChargesAmount'))));
    }

    // ============================================================================ detail
    /** FormDetailValidation():1521 */
    function FormDetailValidation() {
        if (!HRM.comboRow('cmbAccountDetail', 'Id')) { HRM.box('Account  Field Required'); HRM.focus('cmbAccountDetail'); return false; }
        if (HRM.visible('PanelSubsidiaryDetail') && SubsdiaryAccountDatasourceLengthDetail > 0 && !HRM.comboRow('CmbSubsidiaryAccount', 'Id')) {
            HRM.box('Subsidiary Account Title Field Required'); HRM.focus('CmbSubsidiaryAccount'); return false;
        }
        if (!HRM.comboRow('cmbJobLotFile', 'Id')) { HRM.box('JobLot  Field Required'); HRM.focus('cmbJobLotFile'); return false; }
        if (HRM.num(HRM.val('txtAmountDetail').trim()) === 0) { HRM.box('Amount  Field Required'); HRM.focus('txtAmountDetail'); return false; }
        return true;
    }
    function detailRow() {
        var acc = HRM.comboRow('cmbAccountDetail', 'Id'), subVis = HRM.visible('PanelSubsidiaryDetail');
        var sub = HRM.comboRow('CmbSubsidiaryAccount', 'Id'), ref = HRM.comboVal('CmbReferenceAccount');
        var useSub = SubsdiaryAccountDatasourceLengthDetail > 0 && sub;
        var refRow = ref > 0 ? HRM.comboRow('CmbReferenceAccount', 'Id') : null;
        return {
            AccountCode: HRM.str(HRM.col(acc, 'AccountCode')),
            AccId: HRM.comboVal('cmbAccountDetail'),
            Account: HRM.comboText('cmbAccountDetail'),
            SubsidiaryAccountId: !subVis ? 0 : (useSub ? HRM.comboVal('CmbSubsidiaryAccount') : HRM.comboVal('cmbAccountDetail')),
            SubsidiaryAccount: !subVis ? '' : (useSub ? HRM.comboText('CmbSubsidiaryAccount') : HRM.comboText('cmbAccountDetail')),
            SubsidiaryAccountTypeId: !subVis ? 0 : (useSub ? HRM.int(HRM.col(sub, 'SubsidiaryTypeId')) : 4),
            JobLotId: HRM.comboVal('cmbJobLotFile'),
            JobLot: HRM.comboText('cmbJobLotFile'),
            InvoiceNo: HRM.val('txtInvoiceNoDetail').trim(),
            Qty: HRM.num(HRM.val('txtQtyDetial')),
            Rate: HRM.num(HRM.val('txtRateDetail')),
            Amount: HRM.num(HRM.val('txtAmountDetail')),
            FcyAmount: HRM.num(HRM.val('txtFcyAmountDetail')),
            ReferenceAccountId: ref,
            ReferenceAccount: refRow ? HRM.str(HRM.col(refRow, 'AccountTitle')) : '',
            Remarks: HRM.val('txtRemarksDetail')
        };
    }
    function detailButtons(edit) { HRM.show('btnUpdateDetail', edit); HRM.show('btnCancelDetail', edit); HRM.show('btnAdd', !edit); }
    /** ResetFormDetail():1987 */
    function ResetFormDetail() {
        HRM.setCombo('cmbAccountDetail', 0);
        HRM.setVal('txtInvoiceNoDetail', '');
        HRM.setVal('txtQtyDetial', '0');
        HRM.setVal('txtRateDetail', '0');
        HRM.setVal('txtAmountDetail', '');
        HRM.setVal('txtFcyAmountDetail', '0');                                        // txtAmountDetail_TextChanged -> FCY 0
        HRM.setVal('txtRemarksDetail', '');
        detailButtons(false);
        updateDetailIndex = -1;
    }
    /** btnAdd_Click():1678 */
    P.btnAdd = function () {
        if (!FormDetailValidation()) return;
        grd.add(detailRow());
        HRM.text('lblbalancedetail', ''); HRM.text('lbldetaildrcr', '');
        salesTex();                                                                   // DetailGridSetting() -> salesTex()
        HRM.focus('cmbAccountDetail');
        ResetFormDetail();
        UpdateDataInScheduleGrid();
        txtExchangeRate_TextChanged();
    };
    /** grdDetail_DoubleClick():1550 */
    function grdDetail_DoubleClick(i) {
        var r = grd.rows()[i]; if (!r) return;
        updateDetailIndex = i;
        HRM.setCombo('cmbAccountDetail', HRM.int(r.AccId));
        cmbAccountDetail_Leave();
        if (SubsdiaryAccountDatasourceLengthDetail === 0) {
            FA.fillCols('CmbSubsidiaryAccount', [{ Id: HRM.int(r.SubsidiaryAccountId), SubsidiaryAccount: r.SubsidiaryAccount, SubsidiaryTypeId: 4, AccountId: HRM.int(r.AccId) }],
                'Id', 'SubsidiaryAccount', SUBCOLS, {});
        }
        HRM.setCombo('CmbSubsidiaryAccount', HRM.int(r.SubsidiaryAccountId));
        HRM.setCombo('cmbJobLotFile', HRM.int(r.JobLotId));
        HRM.setVal('txtRemarksDetail', HRM.str(r.Remarks));
        HRM.setVal('txtInvoiceNoDetail', HRM.str(r.InvoiceNo));
        HRM.setVal('txtQtyDetial', HRM.str(r.Qty));
        HRM.setVal('txtRateDetail', rateText(r.Rate));
        HRM.setVal('txtAmountDetail', HRM.fmtNum(r.Amount, AD()));
        HRM.setVal('txtFcyAmountDetail', FA.fmt(r.FcyAmount, 3));
        if (HRM.int(r.ReferenceAccountId) > 0) HRM.setCombo('CmbReferenceAccount', HRM.int(r.ReferenceAccountId));
        detailButtons(true);
        HRM.focus('cmbAccountDetail');
    }
    /** btnUpdateDetail_Click():1620 */
    P.btnUpdateDetail = function () {
        if (!FormDetailValidation()) return;
        if (updateDetailIndex < 0 || !grd.rows()[updateDetailIndex]) return;
        HRM.text('lblbalancedetail', ''); HRM.text('lbldetaildrcr', '');
        var r = detailRow();
        if (HRM.visible('PanelSubsidiaryDetail')) {
            var sub = HRM.comboRow('CmbSubsidiaryAccount', 'Id');
            if (sub && HRM.comboText('CmbSubsidiaryAccount') !== '') {
                r.SubsidiaryAccountId = HRM.comboVal('CmbSubsidiaryAccount'); r.SubsidiaryAccount = HRM.comboText('CmbSubsidiaryAccount');
                r.SubsidiaryAccountTypeId = HRM.int(HRM.col(sub, 'SubsidiaryTypeId'));
            } else {
                r.SubsidiaryAccountId = HRM.comboVal('cmbAccountDetail'); r.SubsidiaryAccount = HRM.comboText('cmbAccountDetail'); r.SubsidiaryAccountTypeId = 4;
            }
        }
        grd.update(updateDetailIndex, r);
        ResetFormDetail();
        HRM.focus('cmbAccountDetail');
        txtExchangeRate_TextChanged();
        UpdateDataInScheduleGrid();
    };
    P.btnCancelDetail = function () { ResetFormDetail(); };                          // btnCancelDetail_Click
    /** grdDetail_ColumnButtonClick():1596 (Delete) */
    function grdDetail_Delete(i) {
        grd.remove(i);
        UpdateDataInScheduleGrid();
        txtExchangeRate_TextChanged();
    }

    // ============================================================================ form
    function buttons(mode) {                                                         // 'save' | 'update' | 'saveas'
        HRM.show('btnsave', mode === 'save');
        HRM.show('btnUpdate', mode === 'update');
        HRM.show('btnSaveAs', mode === 'saveas');
    }
    /** ResetForm():1942 */
    function ResetForm() {
        RecId = 0;
        HRM.setVal('txtInvoiceNoDetail', '');
        HRM.setCombo('cmbAccount', 0);
        HRM.setVal('txtInvoiceNo', '');
        HRM.setVal('txtRemarks', '');
        HRM.setCombo('CmbTaxAccount', 0);
        HRM.setVal('txtTaxPercent', '');
        HRM.setVal('txtTaxAmount', '');
        HRM.setVal('lblCreditAcBalance', '0');
        HRM.focus('cmbAccount');
        grd.clear();
        grdSchedule.clear();
        AddRowInScheduleGrid();
        buttons('save');
        UpdateMode = false;
        HRM.setCombo('CmbAdvanceTaxAccount', 0);
        HRM.setCombo('CmbOtherChargesAccount', 0);
        HRM.setCombo('CmbTaxType', 0);
        HRM.setVal('txtVoucherAmountFooter', '');
        HRM.setVal('txtAdvanceTaxAmount', '');
        HRM.setVal('txtOtherChargesAmount', '');
        HRM.setVal('txtTotalAmount', '');
        CalculateTotalInformation();
        return HRM.get(API + '/next-code').then(function (d) { HRM.setVal('txtDocNo', HRM.str(d && d.voucherCode)); }).catch(HRM.fail);
    }
    /** FormValidation():1468 (Insert() checks the grid first). */
    function FormValidation() {
        function fail(m, id) { HRM.box(m); HRM.focus(id); return false; }
        if (!HRM.comboRow('cmbLocationType', 'Id') || !HRM.comboVal('cmbLocationType')) return fail('Location Type Field is Required', 'cmbLocationType');
        if (!HRM.comboRow('CmbProjectId', 'Id') || !HRM.comboVal('CmbProjectId')) return fail('Cost Center  Field Required', 'CmbProjectId');
        var doc = HRM.val('txtDocNo').trim();
        if (doc === '' || doc === '0') return fail('DocNo  Field Required', 'txtDocNo');
        if (!HRM.comboRow('cmbAccount', 'Id') || !HRM.comboVal('cmbAccount')) return fail('Account  Field Required', 'cmbAccount');
        if (subFeature && SubsdiaryAccountDatasourceLengthCr > 0 && (!HRM.comboRow('CmbSubsidiaryAccountCr', 'Id') || !HRM.comboVal('CmbSubsidiaryAccountCr')))
            return fail('Subsidiary Account Title Field Required', 'CmbSubsidiaryAccountCr');
        if (!HRM.comboVal('cmbCurrency')) return fail('Fcy Code Field is Required', 'cmbCurrency');
        if (HRM.num(HRM.val('txtExchangeRate')) === 0) return fail('Exchange Rate Field is Required', 'txtExchangeRate');
        if (HRM.num(HRM.val('txtFcyAmount')) === 0) return fail('Fcy Amount Field is Required', 'txtFcyAmount');
        return true;
    }
    /** Insert():2099 */
    function Insert(btn, mode) {
        if (!grd.rows().length) { HRM.box('Grid Record not found'); return; }
        if (!FormValidation()) return;
        if (!HRM.ask(mode === 'update' ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            mode: mode === 'update' ? 'update' : 'save', recId: mode === 'update' ? RecId : 0,
            locationTypeId: HRM.comboVal('cmbLocationType'), projectId: HRM.comboVal('CmbProjectId'), voucherCode: HRM.val('txtDocNo'),
            voucherDate: HRM.val('txtDocDate'), accountId: HRM.comboVal('cmbAccount'), headerSubsidiaryId: HRM.comboVal('CmbSubsidiaryAccountCr'),
            taxAccountId: HRM.comboVal('CmbTaxAccount'), invoiceNo: HRM.val('txtInvoiceNo'), invoiceDate: HRM.val('txtInvoiceDate'),
            remarks: HRM.val('txtRemarks'), entryTypeId: HRM.comboVal('CmbEntryType'), currencyId: HRM.comboVal('cmbCurrency'),
            exchangeRate: HRM.num(HRM.val('txtExchangeRate')), customAccounts: HRM.checked('chkCustomAccounts'),
            taxTypeId: HRM.comboVal('CmbTaxType'), taxPercent: HRM.num(HRM.val('txtTaxPercent')), taxPercentText: HRM.val('txtTaxPercent'),
            taxAmount: HRM.num(HRM.val('txtTaxAmount')), advanceTaxAccountId: HRM.comboVal('CmbAdvanceTaxAccount'),
            advanceTaxAmount: HRM.num(HRM.val('txtAdvanceTaxAmount')), otherChargesAccountId: HRM.comboVal('CmbOtherChargesAccount'),
            otherChargesAmount: HRM.num(HRM.val('txtOtherChargesAmount')),
            rows: grd.rows().map(function (r) {
                return { accountId: HRM.int(r.AccId), subsidiaryAccountId: HRM.int(r.SubsidiaryAccountId), subsidiaryAccountTypeId: HRM.int(r.SubsidiaryAccountTypeId),
                    jobLotId: HRM.int(r.JobLotId), invoiceNo: HRM.str(r.InvoiceNo), qty: HRM.num(r.Qty), rate: HRM.num(r.Rate), amount: HRM.num(r.Amount),
                    fcyAmount: HRM.num(r.FcyAmount), referenceAccountId: HRM.int(r.ReferenceAccountId), remarks: HRM.str(r.Remarks) };
            }),
            schedules: grdSchedule.rows().map(function (r) {
                return { dueDays: HRM.str(r.DueDays), dueDate: HRM.str(r.DueDate), duePercent: HRM.num(r['Due%']), amount: HRM.num(r.Amount) };
            })
        };
        var preview = HRM.checked('CHKPREVIEW');
        var win = preview ? window.open('about:blank', '_blank') : null;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (d) {
                HRM.box(d && d.message ? d.message : '');
                var id = HRM.int(d && d.id);
                return ResetForm().then(function () {
                    if (preview && id > 0) printInto(win, '102', id); else if (win) win.close();            // CHKPREVIEW -> 102 slip
                });
            }).catch(function (e) { if (win) win.close(); HRM.fail(e); });
        }, 'pur-save');
    }
    /** ReadById(ID):2463 */
    function ReadById(id, saveAs) {
        return ResetForm().then(function () {
            return HRM.loading(HRM.get(API + '/by-id', { id: id }));
        }).then(function (o) {
            RecId = id;
            UpdateMode = true;
            buttons(saveAs ? 'saveas' : 'update');
            FA.showTab('form');
            HRM.setVal('txtDocDate', HRM.str(o.voucherDate));
            HRM.setCombo('cmbAccount', HRM.int(o.accountId));
            HRM.setCombo('CmbProjectId', HRM.int(o.projectId));
            HRM.setVal('txtRemarks', HRM.str(o.remarks));
            HRM.setVal('txtInvoiceNo', HRM.str(o.invoiceNo));
            HRM.setVal('txtDocNo', HRM.str(o.voucherCode));
            HRM.setCombo('CmbEntryType', HRM.int(o.entryTypeId));
            HRM.setCombo('cmbCurrency', HRM.int(o.currencyId));
            HRM.check('chkCustomAccounts', !!o.customAccounts);
            HRM.setVal('txtExchangeRate', rateText(o.exchangeRate));
            HRM.setVal('txtFcyAmount', FA.fmt(o.fcAmount, 3));
            HRM.setCombo('CmbAdvanceTaxAccount', HRM.int(o.advanceTaxAccountId));
            HRM.setVal('txtAdvanceTaxAmount', FA.fmt(o.advanceTaxAmount, 3));
            HRM.setCombo('CmbOtherChargesAccount', HRM.int(o.otherChargesAccountId));
            HRM.setVal('txtOtherChargesAmount', FA.fmt(o.otherChargesAmount, 3));
            if (!HRM.comboVal('cmbLocationType')) HRM.setCombo('cmbLocationType', HRM.int(o.firstLocationTypeId));
            if (subFeature && (o.rows || []).length) { cmbAccount_Leave(); HRM.setCombo('CmbSubsidiaryAccountCr', HRM.int(o.headerSubsidiaryId)); }
            grd.set((o.rows || []).map(function (r) {
                return { AccountCode: r.accountCode, AccId: r.accountId, Account: r.account, SubsidiaryAccountId: r.subsidiaryAccountId,
                    SubsidiaryAccount: r.subsidiaryAccount, SubsidiaryAccountTypeId: r.subsidiaryAccountTypeId, JobLotId: r.jobLotId, JobLot: r.jobLot,
                    InvoiceNo: r.invoiceNo, Qty: r.qty, Rate: r.rate, Amount: r.amount, FcyAmount: r.fcyAmount,
                    ReferenceAccountId: r.referenceAccountId, ReferenceAccount: r.referenceAccount, Remarks: r.remarks };
            }));
            var t = o.tax || {};
            if (HRM.int(t.taxTypeId) > 0) HRM.setCombo('CmbTaxType', HRM.int(t.taxTypeId));
            if (HRM.int(t.taxAccountId) > 0) HRM.setCombo('CmbTaxAccount', HRM.int(t.taxAccountId));
            HRM.setVal('txtTaxPercent', HRM.str(t.taxPercent));
            HRM.setVal('txtTaxAmount', HRM.str(t.taxAmount));
            grdSchedule.set((o.schedules || []).map(function (s) { return { DueDays: HRM.str(s.dueDays), DueDate: s.dueDate, 'Due%': s.duePercent, Amount: s.amount }; }));
            if (!grdSchedule.rows().length) AddRowInScheduleGrid();
            txtExchangeRate_TextChanged();
            TotalAmountInfooter();
        }).catch(HRM.fail);
    }

    // ============================================================================ prints
    var PRINT = {
        '102': { url: '/reports/print/102-a-new-ac-payment-receipts-voucher-slip', body: function (id) { return { id: id }; } },          // ANewAcRptPaymentReceiptsVoucherSlip_102
        '102_01': { url: '/reports/print/102-01-bills-payables-or-receiveable-voucher', body: function (id) { return { voucherHeadId: id, documentTypeIds: '130' }; } }   // BillsPayablesOrReceiveableVoucher102_01(id, 130)
    };
    function printInto(win, kind, id) {
        var p = PRINT[kind];
        var h = { 'Content-Type': 'application/json', 'Accept': 'application/pdf' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(p.url, { method: 'POST', credentials: 'same-origin', headers: h, body: JSON.stringify(p.body(id)) }).then(function (r) {
            var type = r.headers.get('Content-Type') || '';
            if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { if (win) win.location = URL.createObjectURL(b); });
            return r.text().then(function (x) { if (win) win.close(); HRM.box(x || ('Print failed (' + r.status + ')')); });
        }).catch(function (e) { if (win) win.close(); HRM.fail(e); });
    }
    function print(kind, id, btnEl) {
        if (!(id > 0)) { HRM.box('Record Not Found For Display'); return; }
        var win = window.open('about:blank', '_blank');
        return HRM.busy(btnEl, function () {
            return HRM.get(API + '/print-check', { id: id }).then(function () { return printInto(win, kind, id); })
                .catch(function (e) { if (win) win.close(); HRM.fail(e); });
        }, 'pur-print');
    }

    // ============================================================================ history
    var DataGridHistory = new HRM.Grid('DataGridHistory', {                            // HistoryGridSettings():3147
        columns: [
            { key: 'Print', caption: 'Print', width: 50, render: function () { return btn('print', 'Print'); } },
            { key: 'PrintII', caption: 'PrintII', width: 60, render: function () { return btn('print2', 'PrintII'); } },
            { key: 'Edit', caption: 'Edit', width: 50, render: function () { return btn('edit', 'Edit'); } },
            { key: 'SaveAs', caption: 'SaveAs', width: 70, render: function () { return btn('saveas', 'SaveAs'); } },
            { key: 'DocumentTypeCode', caption: 'V.Type', width: 60 },
            { key: 'Id', hidden: true }, { key: 'AccountTypeId', hidden: true },
            { key: 'VoucherCode', caption: 'V.No', width: 60 },
            { key: 'DocumentTypeId', hidden: true },
            { key: 'VoucherDate', caption: 'V.Date', type: 'date', width: 90 },
            { key: 'ManualBillNo', hidden: true },
            { key: 'AccountTitle', caption: 'AccountTitle' },
            { key: 'AgainstAccount', hidden: true },
            { key: 'VoucherAmount', caption: 'VoucherAmount', type: 'num', sum: true, render: function (v) { return HRM.esc(HRM.fmtNum(v, AD())); } },
            { key: 'FcyCode', caption: 'FcyCode' },
            { key: 'ExchangeRate', caption: 'ExchangeRate', type: 'num', render: function (v) { return HRM.esc(rateText(v)); } },
            { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', sum: true, decimals: 3, render: function (v) { return HRM.esc(FA.fmt(v, 3)); } },
            { key: 'Remarks', caption: 'Remarks', width: 200 },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' },
            { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'ApprovedDate', caption: 'ApprovedDate', type: 'datetime' },
            { key: 'CheqNo', caption: 'CheqNo' },
            { key: 'Attachment', caption: 'Attachment', type: 'int' }
        ],
        filterRow: true, totals: true,
        onDouble: function (r) { ReadById(HRM.int(HRM.col(r, 'Id'))); },               // DataGridHistory_DoubleClick
        onSelect: function (r) { VoucherDetailByHeaderId(r); }                           // DataGridHistory_SelectionChanged
    });
    onButtons(DataGridHistory, 'DataGridHistory', function (r, act, i, b) {              // DataGridHistory_ColumnButtonClick
        var id = HRM.int(HRM.col(r, 'Id'));
        if (act === 'edit') ReadById(id);
        else if (act === 'saveas') { if (rights.update) ReadById(id, true); }
        else if (act === 'print') print('102', id, b);
        else if (act === 'print2') print('102_01', id, b);
    });
    var grdDetailHistory = new HRM.Grid('grdDetailHistory', {                          // VoucherDetailByHeaderId():3301
        columns: [
            { key: 'AccountCode', caption: 'AccountCode' }, { key: 'AccountTitle', caption: 'AccountTitle' },
            { key: 'SubsidiaryAccount', caption: 'SubsidiaryAccount' }, { key: 'Job/Lot', caption: 'Job/Lot' }, { key: 'InvoiceNo', caption: 'InvoiceNo' },
            { key: 'Qty', caption: 'Qty', type: 'num', sum: true, decimals: 3, render: function (v) { return HRM.esc(FA.fmt(v, 3)); } },
            { key: 'Rate', caption: 'Rate', type: 'num', render: function (v) { return HRM.esc(rateText(v)); } },
            { key: 'DebitAmount', caption: 'Debit', type: 'num', sum: true, render: function (v) { return HRM.esc(HRM.fmtNum(v, AD())); } },
            { key: 'CreditAmount', caption: 'Credit', type: 'num', sum: true, render: function (v) { return HRM.esc(HRM.fmtNum(v, AD())); } },
            { key: 'FcyAmount', caption: 'FcyAmount', type: 'num', sum: true, decimals: 3, render: function (v) { return HRM.esc(FA.fmt(v, 3)); } },
            { key: 'Remarks', caption: 'Remarks' }, { key: 'ReferenceAccount', caption: 'ReferenceAccount' }
        ],
        totals: true
    });
    var detSeq = 0;
    function VoucherDetailByHeaderId(r) {
        var seq = ++detSeq;
        HRM.get(API + '/history-detail', { id: HRM.int(HRM.col(r, 'Id')) }).then(function (rows) {
            if (seq !== detSeq) return;
            grdDetailHistory.set(rows || []);
        }).catch(HRM.fail);
    }
    /** HistoryFill():3019 */
    function HistoryFill(btnEl) {
        var dt = HRM.checked('rdexpentry') ? 'entry' : HRM.checked('rdexpmodify') ? 'modify' : HRM.checked('rdexpapproved') ? 'approved' : 'doc';
        var q = { dateType: dt, fromDocNo: HRM.int(HRM.val('txtFromDocNoHistory')), toDocNo: HRM.int(HRM.val('txtToDocNoHistory')),
            accountId: HRM.comboVal('CmbAccountTitleHistory'), status: HRM.comboText('cmbApproveStatus') || 'Not Apporved' };
        if (HRM.checked('chkFromDateHistory')) q.fromDate = HRM.val('FromDateHistory');
        if (HRM.checked('chkToDateHistory')) q.toDate = HRM.val('ToDateHistory');
        return HRM.busy(btnEl, function () {
            return HRM.get(API + '/history', q).then(function (d) {
                d = d || {};
                var rows = d.rows || [];
                DataGridHistory.set(rows);
                if (rows.length) {
                    HRM.show('VoucherInfoBox', true);
                    HRM.text('lblTotalVouchers', HRM.str(HRM.int(d.totalVouchers)));
                    HRM.text('lblApprovedVouchers', HRM.str(HRM.int(d.approvedVouchers)));
                    HRM.text('lblUnApprovedVouchers', HRM.str(HRM.int(d.unApprovedVouchers)));
                } else { HRM.show('VoucherInfoBox', false); grdDetailHistory.clear(); }
            }).catch(HRM.fail);
        }, 'pur-history');
    }

    // ============================================================================ toolbar
    P.btnnew = function () { ResetForm(); ResetFormDetail(); };                          // btnnew_Click
    P.btnsave = function (b) { RecId = 0; return Insert(b || 'btnsave', 'save'); };    // btnsave_Click
    P.btnSaveAs = function (b) { RecId = 0; return Insert(b || 'btnSaveAs', 'save'); };  // btnSaveAs_Click
    P.btnUpdate = function (b) {                                                        // btnUpdate_Click
        if (RecId === 0) { HRM.box('Record Not Update  ' + RecId); return; }
        return Insert(b || 'btnUpdate', 'update');
    };
    P.btnRefresh = function (b) {                                                       // btnRefresh_Click
        return HRM.busy(b || 'btnRefresh', function () {
            return HRM.get(API + '/setup').then(function (d) { L = d || {}; return bindAll(false); }).catch(HRM.fail);
        });
    };
    P.Print = function (b) { return print('102', RecId, b || 'Print'); };                // Print_Click -> GenerateReport()
    P.BtnPrintII = function (b) { return print('102_01', RecId, b || 'BtnPrintII'); };   // BtnPrintII_Click
    P.btnNewHistory = function () {                                                     // btnNewHistory_Click (Reset)
        HRM.setVal('FromDateHistory', HRM.today());
        HRM.setVal('ToDateHistory', HRM.today());
        HRM.setVal('txtFromDocNoHistory', '');
        HRM.setVal('txtToDocNoHistory', '');
        HRM.setCombo('CmbAccountTitleHistory', 0);
        HRM.setCombo('cmbApproveStatus', 1);
        HRM.show('VoucherInfoBox', false);
        DataGridHistory.clear();
        grdDetailHistory.clear();
    };
    P.btnRefreshHistory = function (b) {                                                // btnRefreshHistory_Click
        return HRM.busy(b || 'btnRefreshHistory', function () {
            return HRM.get(API + '/history-accounts').then(ComboBindForHistory).catch(HRM.fail);
        });
    };
    P.btnShowHistory = function (b) { return HistoryFill(b || 'btnShowHistory'); };    // btnShowHistory_Click
    P.btnShortCutKey = function () { MakeShortCutKeys(); };
    function MakeShortCutKeys() {                                                       // MakeShortCutKeys():3662
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+L', 'For Load All Records'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Grids'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Debit Account in Detail Box'],
            ['Ctrl+shift+ArrowUp', 'For Focus On Sale Tax Account int Sales Tax Tab'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', 'When Focus On Any Grid To Call Function\'s On Button Or Link']];
        var m = HRM.modal({ title: 'ShortCut Keys', width: 'min(600px, 96vw)', html: '<div class="hrm-grid-wrap"><table class="fa-keys"></table></div>' });
        new HRM.Grid(m.body.querySelector('.fa-keys'), { columns: [{ key: 'k', caption: 'KeyCombination' }, { key: 'd', caption: 'Description' }] })
            .set(rows.map(function (r) { return { k: r[0], d: r[1] }; }));
    }

    // ============================================================================ events
    FA.onLeave('CmbCustomGroup', function () { if (loaded) CmbCustomGroup_Leave(); });
    FA.onLeave('cmbAccountDetail', function () { if (loaded) cmbAccountDetail_Leave(); });
    FA.onLeave('CmbSubsidiaryAccount', function () { if (loaded) CmbSubsidiaryAccount_Leave(); });
    FA.onLeave('CmbSubsidiaryAccountCr', function () { if (loaded) CmbSubsidiaryAccountCr_Leave(); });
    FA.onLeave('cmbAccount', function () { if (loaded) cmbAccount_Leave(); });
    FA.onLeave('CmbAccountType', function () { if (loaded) CmbAccountType_Leave(); });
    FA.onLeave('cmbCurrency', function () { if (loaded) cmbCurrency_Leave(); });
    HRM.$('cmbAccountDetail').addEventListener('change', function () {               // cmbAccountDetail_TextChanged (Note Title)
        var r = HRM.comboRow('cmbAccountDetail', 'Id');
        HRM.text('label29', r && HRM.comboVal('cmbAccountDetail') > 0 ? HRM.str(HRM.col(r, 'NoteTitle')) : '');
    });
    HRM.$('CmbTaxType').addEventListener('change', function () { if (loaded) salesTex(); });   // CmbTaxType_TextChanged
    HRM.$('txtExchangeRate').addEventListener('input', txtExchangeRate_TextChanged);
    HRM.$('txtExchangeRate').addEventListener('blur', function () {                  // txtExchangeRate_Leave
        HRM.setVal('txtExchangeRate', rateText(HRM.num(HRM.val('txtExchangeRate'))));
        txtExchangeRate_TextChanged();
    });
    HRM.$('txtQtyDetial').addEventListener('input', Calculation);
    HRM.$('txtRateDetail').addEventListener('input', Calculation);
    HRM.$('txtAmountDetail').addEventListener('input', function () { related('amount'); });
    HRM.$('txtFcyAmountDetail').addEventListener('input', function () { related('fcy'); });
    HRM.$('txtOtherChargesAmount').addEventListener('input', TotalAmountInfooter);
    HRM.$('txtAdvanceTaxAmount').addEventListener('input', TotalAmountInfooter);
    function syncDateChecks() {
        HRM.enable('FromDateHistory', HRM.checked('chkFromDateHistory'));
        HRM.enable('ToDateHistory', HRM.checked('chkToDateHistory'));
    }
    HRM.$('chkFromDateHistory').addEventListener('change', syncDateChecks);
    HRM.$('chkToDateHistory').addEventListener('change', syncDateChecks);
    FA.tabs(function (t) { if (t === 'history') HRM.focus('FromDateHistory'); });      // tabControl1_SelectedIndexChanged
    function focusRow(id) { var r = HRM.$(id).querySelector('tbody tr[data-i]'); if (r) r.focus(); }
    HRM.keys({                                                                         // frmFixedAssetPurchase_KeyDown
        'ctrl+e': guard(HRM.close), 'esc': guard(HRM.close),
        'ctrl+s': guard(function () { if (FA.activeTab() === 'form' && HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); }),
        'ctrl+u': guard(function () { if (HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled && UpdateMode) P.btnUpdate(); }),
        'ctrl+n': guard(function () { P.btnnew(); }),
        'ctrl+t': guard(function () {
            if (FA.activeTab() === 'history') { FA.showTab('form'); HRM.focus('txtDocDate'); } else { FA.showTab('history'); focusRow('DataGridHistory'); }
        }),
        'ctrl+p': guard(function () { if (rights.print) P.Print(); }),
        'ctrl+r': guard(function () { P.btnRefresh(); }),
        'ctrl+f5': guard(function () { HRM.focus('txtDocDate'); }),
        'ctrl+arrowdown': guard(function () { focusRow('grd'); }),
        'ctrl+arrowup': guard(function () { HRM.focus('cmbAccountDetail'); }),
        'ctrl+shift+arrowup': guard(function () { HRM.focus('cmbAccountDetail'); }),
        'ctrl+arrowright': guard(function () {
            var a = document.activeElement;
            if (a && a.closest('#grd')) focusRow('grdSchedule');
            else if (a && a.closest('#grdSchedule')) HRM.focus('CmbTaxAccount');
            else focusRow('grd');
        }),
        'ctrl+alt+control': guard(MakeShortCutKeys), 'ctrl+alt+alt': guard(MakeShortCutKeys),
        'ctrl+enter': guard(function () {                                                // grdDetail_KeyDown / DataGridHistory_KeyDown Ctrl+Enter
            var a = document.activeElement;
            if (a && a.closest('#grd') && grd.currentIndex() >= 0) grdDetail_DoubleClick(grd.currentIndex());
            else if (a && a.closest('#DataGridHistory') && DataGridHistory.current()) ReadById(HRM.int(HRM.col(DataGridHistory.current(), 'Id')));
        })
    });
    HRM.footer(function () { FA.showTab('history'); HRM.focus('FromDateHistory'); });  // History = the form's History tab

    // ============================================================================ load
    HRM.setVal('txtDocDate', HRM.today());
    HRM.setVal('txtInvoiceDate', HRM.today());
    HRM.setVal('ToDateHistory', HRM.today());
    HRM.setVal('FromDateHistory', HRM.today());
    StatusFillForHistory();
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {                           // InitializeComponentMethod()
        L = d || {};
        rights = L.rights || {};
        subFeature = !!L.subsidiaryFeature;
        HRM.applyRights({ save: rights.save, update: rights.update, print: rights.print },
            { save: ['btnsave', 'btnSaveAs'], update: 'btnUpdate', print: 'Print' });
        HRM.show('PanelSubsidiaryDetail', subFeature);
        HRM.show('rowSubsidiaryCr', subFeature);
        subsidiaryColumn();
        setHidden(grdDetailHistory, 'SubsidiaryAccount', !subFeature);
        HRM.text('lblbalancedetail', ''); HRM.text('lbldetaildrcr', '');
        if (RecId === 0) HRM.setVal('txtDocNo', HRM.str(L.voucherCode));
        var p = bindAll(true);
        grdSchedule.clear();
        AddRowInScheduleGrid();
        ComboBindForHistory(L.historyAccounts || []);
        var back = HRM.int(L.defaults && L.defaults.historyFromDaysBack) || 3;
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -back));
        HRM.setVal('ToDateHistory', HRM.today());
        loaded = true;
        HRM.focus('txtDocDate');
        var id = HRM.int(HRM.param('id'));
        return p.then(function () { if (id > 0) ReadById(id); });
    }).catch(HRM.fail);
})();
