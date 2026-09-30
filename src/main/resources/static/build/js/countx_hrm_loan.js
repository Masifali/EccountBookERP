/* ============================================================================================
 * countx_hrm_loan.js - HRM "Loan Management" (AppModules 2024). One script, the page is chosen by
 * <body data-hrm="...">. Built on countx_hrm.js (window.HRM); exports window.HrmLoan.
 *
 *   employee-loan      LoanManagement/frmEmployeeLoan.cs    (663)
 *   employee-advance   LoanManagement/frmEmployeeAdvance.cs (664)
 *
 * Each handler follows its desktop form line by line: validation wording and order, messages, what
 * New / Save / Update / double-click do to the buttons and the grid. Both pages load a record from
 * ?id= (opened from Loan Approval / Advance Approval).
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/loan/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmLoan
    window.HrmLoan = P;

    // ------------------------------------------------------------------------ .NET number text
    /** Math.Round(v, d) - MidpointRounding.ToEven (also Convert.ToInt32(decimal) with d = 0). */
    function roundEven(v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p;
        var r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    }
    /** .NET custom format "#,#": whole number, grouped, zero shows nothing. */
    function fmtHash(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v), r = (n < 0 ? -1 : 1) * Math.round(Math.abs(n));
        return r === 0 ? '' : HRM.fmtNum(r, 0);
    }
    function numCol(key, caption) {
        return { key: key, caption: caption, type: 'num', decimals: 0, sum: true, render: function (v) { return HRM.esc(fmtHash(v)); } };
    }
    function dateCol(key, caption) { return { key: key, caption: caption, type: 'date' }; }
    function btnCell(act, text) { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    /** GridEX ColumnButtonClick: a click on a button cell of the table. */
    function onButton(grid, tableId, fn) {
        HRM.$(tableId).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), b);
        });
    }

    // ------------------------------------------------------------------------ shared: employee combo
    /** EmployeeName(): BindDDLNew(dtemployee, cmbEmployeeName, "EmployeeId", "EmployeeName", "Employee Name", false). */
    function fillEmployees(rows, keep) {
        HRM.fill('cmbEmployeeName', rows, 'EmployeeId', 'EmployeeName', { zero: '', keep: !!keep });
    }
    /** cmbEmployeeName_Leave: the dtemployee row of the selected employee -> the Employee Information group. */
    function employeeInfo(rows) {
        var id = HRM.comboVal('cmbEmployeeName'), hit = null;
        if (id) (rows || []).forEach(function (r) { if (HRM.int(HRM.col(r, 'EmployeeId')) === id) hit = r; });
        HRM.setVal('txtEmployeeNo', hit ? HRM.str(HRM.col(hit, 'EmployeeNo')) : '');
        HRM.setVal('txtDesgination', hit ? HRM.str(HRM.col(hit, 'DesignationName')) : '');
        HRM.setVal('txtDepartment', hit ? HRM.str(HRM.col(hit, 'DepartmentName')) : '');
        HRM.setVal('txtSalary', hit ? HRM.str(HRM.col(hit, 'TotalSalary')) : '');
        HRM.setVal('txtSection', hit ? HRM.str(HRM.col(hit, 'SectionName')) : '');
    }
    /** btnEmployeeRegister_Click: frmEmployeeRegistration.Show() when its View right is there. */
    function employeeRegister(can) {
        if (can) HRM.open('/hrm/employee-registration');
        else HRM.box("You don't have the right to view this form...");
    }
    /**
     * UltraCombo.Leave: a pick fires `change` on the hidden <select> (countx_prod_combo.js); focus leaving the
     * combo's wrap (built after this script runs, so the listener is delegated) is the Leave itself.
     */
    function onComboLeave(fn) {
        HRM.$('cmbEmployeeName').addEventListener('change', fn);
        document.addEventListener('focusout', function (e) {
            var w = e.target && e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
            if (!w || !w.querySelector('#cmbEmployeeName')) return;
            if (e.relatedTarget && w.contains(e.relatedTarget)) return;
            setTimeout(fn, 0);
        });
    }

    // ============================================================================ 663 Employee Loan
    function employeeLoan() {
        var RecId = 0, UpdateMode = false, rights = {}, dtemployee = [], canRegister = false;
        var grd = new HRM.Grid('grd', {
            columns: [                                                        // GridBind dtEmp + grdSetting()
                { key: 'LoanId', caption: 'LoanId', hidden: true },
                { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
                { key: 'DocNo', caption: 'DocNo', type: 'code' },             // ColumnType Link -> grd_LinkClicked
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 250 },
                { key: 'Department', caption: 'Department' },
                { key: 'Designation', caption: 'Designation' },
                dateCol('AppliedDate', 'AppliedDate'),
                numCol('LoanAmount', 'LoanAmount'),
                numCol('ApprovedAmount', 'ApprovedAmount'),
                { key: 'NoOfInstallments', caption: 'NoOfInstallments' },
                { key: 'Reason', caption: 'Reason' },
                { key: 'EntryUser', caption: 'EntryUser' },
                dateCol('EntryDate', 'EntryDate'),
                { key: 'ModifyUser', caption: 'ModifyUser' },
                { key: 'ApprovedUser', caption: 'ApprovedUser' },
                { key: 'Approve_Status', caption: 'Approve_Status' },
                dateCol('ApprovedDate', 'ApprovedDate')
            ],
            filterRow: true, totals: true,
            rowClass: function (r) { return HRM.col(r, 'Approve_Status') === 'Approved' ? 'hrm-row-green' : ''; },
            onDouble: function (r) { ReadById(HRM.int(HRM.col(r, 'LoanId'))); },          // grdEmployeeFamilyInfo_DoubleClick
            onCode: function (r) { printVoucher(HRM.int(HRM.col(r, 'LoanId')), null); }     // grd_LinkClicked (DocNo)
        });
        function GridBind() {                                                 // GridBind(): ClearStructure when no row
            return HRM.get(API + '/list').then(function (rows) { grd.set(rows || []); }).catch(HRM.fail);
        }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function clearInfo() { ['txtSalary', 'txtEmployeeNo', 'txtDepartment', 'txtDesgination', 'txtSection'].forEach(function (i) { HRM.setVal(i, ''); }); }
        function Reset() {                                                    // Reset()
            RecId = 0;
            HRM.setCombo('cmbEmployeeName', 0);
            HRM.setVal('txtLoanAmount', ''); HRM.setVal('txtNoofInstallments', ''); HRM.setVal('txtReason', '');
            clearInfo();
            HRM.focus('cmbEmployeeName');
            buttons(false);
            UpdateMode = false;
            return GridBind();
        }
        function cmbEmployeeName_Leave() { employeeInfo(dtemployee); }        // (SalaryAmountCalculation() is empty on this form)
        function ReadById(id) {                                               // ReadById(Id)
            RecId = id;
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                HRM.setVal('datAppliedOn', HRM.day(HRM.col(o, 'AppliedOn')));
                HRM.setCombo('cmbEmployeeName', HRM.int(HRM.col(o, 'EmployeeId')));
                cmbEmployeeName_Leave();
                HRM.setVal('txtReason', HRM.str(HRM.col(o, 'Reason')));
                HRM.setVal('txtLoanAmount', HRM.str(HRM.col(o, 'LoanAmount')));
                HRM.setVal('txtNoofInstallments', HRM.str(HRM.col(o, 'NoOfInstallment')));
                buttons(true);
                UpdateMode = true;
            }).catch(HRM.fail);
        }
        function Validation() {
            if (!HRM.comboVal('cmbEmployeeName')) { HRM.box('Employee Name Required!'); HRM.focus('cmbEmployeeName'); return false; }
            if (HRM.num(HRM.val('txtLoanAmount')) === 0) { HRM.box('LoanAmount Field Required!'); HRM.focus('txtLoanAmount'); return false; }
            if (HRM.num(HRM.val('txtNoofInstallments')) === 0) { HRM.box('No Of Installments Field Required!'); HRM.focus('txtNoofInstallments'); return false; }
            if (HRM.num(HRM.val('txtNoofInstallments')) > 60) { HRM.box('No Of Installments Can not greater than 60...!'); HRM.focus('txtNoofInstallments'); return false; }
            return true;
        }
        function Insert(btn) {                                                // Insert()
            if (!Validation()) return;
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var upd = RecId > 0;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId, employeeId: HRM.comboVal('cmbEmployeeName'), appliedOn: HRM.val('datAppliedOn'),
                    loanAmount: HRM.val('txtLoanAmount'), noOfInstallments: HRM.val('txtNoofInstallments'), reason: HRM.val('txtReason')
                }).then(function (d) {
                    HRM.box(d && d.message ? d.message : (upd ? 'Update Successfully' : 'Saved Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'loan-save');
        }
        /** CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(CommonServices.VoucherHeadIdGet(id, 1001)). */
        function printVoucher(id, btn) {
            var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
            return HRM.busy(btn, function () {
                return HRM.get(API + '/voucher-id', { id: id }).then(function (d) {
                    var vh = HRM.int(d && d.voucherHeadId);
                    if (!vh) { if (win) window.CrystalPrint.release(win); HRM.box('VoucherId Not Found'); return; }
                    if (!window.CrystalPrint) { HRM.box('Report file 102-ANewAcRptPaymentReceiptsVoucherSlip.rpt is not available.'); return; }
                    return window.CrystalPrint.open('hrm-102', { id: vh }, null, win);
                }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
            }, 'loan-print');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };            // btnupdate_Click
        P.btnnew = function () { return Reset(); };                                     // btnnew_Click
        P.btnRefresh = function (btn) {                                                  // btnRefresh_Click -> EmployeeName()
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/employees').then(function (rows) { dtemployee = rows || []; fillEmployees(dtemployee, true); }).catch(HRM.fail);
            });
        };
        P.btnPrint = function (btn) {                                                    // btnPrint_Click
            if (RecId === 0) { HRM.box('Record Not Found For Display'); return; }
            return printVoucher(RecId, btn || 'btnPrint');
        };
        P.btnEmployeeRegister = function () { employeeRegister(canRegister); };          // btnEmployeeRegister_Click
        onComboLeave(cmbEmployeeName_Leave);                                             // cmbEmployeeName_Leave
        HRM.$('datAppliedOn').addEventListener('blur', function () { /* datAppliedOn_Leave -> SalaryAmountCalculation(): empty */ });
        HRM.keys({                                                                       // EmployeeFamilyInfo_KeyDown
            'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled && !UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled && UpdateMode) P.btnupdate(); },
            'ctrl+enter': function () { var r = grd.current(); if (r && document.activeElement && document.activeElement.closest('#grd')) ReadById(HRM.int(HRM.col(r, 'LoanId'))); }
        });
        HRM.footer(function (b) {                                                        // History = the form's own History grid
            return HRM.busy(b, function () { return GridBind().then(function () { HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); }); });
        });
        HRM.setVal('datAppliedOn', HRM.today());                                         // DateTimePicker default: now
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // EmployeeFamilyInfo_Load
            rights = d.rights || {};
            canRegister = !!d.canRegister;
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
            dtemployee = d.employees || [];
            fillEmployees(dtemployee);
            grd.set(d.rows || []);
            buttons(false);
            HRM.focus('cmbEmployeeName');
            var id = HRM.int(HRM.param('id'));
            if (id > 0) ReadById(id);
        }).catch(HRM.fail);
    }

    // ============================================================================ 664 Employee Advance
    function employeeAdvance() {
        var RecId = 0, UpdateMode = false, rights = {}, dtemployee = [], canRegister = false;
        var grd = new HRM.Grid('grd', {
            columns: [                                                        // GridBind dtEmp + grdSetting()
                { key: 'Print', caption: 'Print', width: 50, render: function () { return btnCell('print', 'Print'); } },   // Position 0, frozen
                dateCol('RequestDate', 'RequestDate'),
                { key: 'RequestNo', caption: 'RequestNo', type: 'code' },
                { key: 'AdvanceId', caption: 'AdvanceId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 250 },
                { key: 'Department', caption: 'Department' },
                { key: 'Designation', caption: 'Designation' },
                dateCol('AppliedDate', 'AppliedDate'),
                numCol('AdvanceAmount', 'AdvanceAmount'),
                numCol('ApprovedAmount', 'ApprovedAmount'),
                { key: 'Reason', caption: 'Reason' },
                { key: 'EntryUser', caption: 'EntryUser' },
                dateCol('EntryDate', 'EntryDate'),
                { key: 'ModifyUser', caption: 'ModifyUser' },
                { key: 'ApprovedUser', caption: 'ApprovedUser' },
                { key: 'IsApproved', caption: 'IsApproved', type: 'check' },
                dateCol('ApprovedDate', 'ApprovedDate')
            ],
            filterRow: true, totals: true,
            rowClass: function (r) { return HRM.bool(HRM.col(r, 'IsApproved')) ? 'hrm-row-green' : ''; },
            onDouble: function (r) { RetrivedData(HRM.int(HRM.col(r, 'AdvanceId'))); UpdateMode = true; }   // grdEmployeeFamilyInfo_DoubleClick
        });
        onButton(grd, 'grd', function (r, act, b) {                           // grd_ColumnButtonClick (Print)
            if (act === 'print') GenerateSlip(HRM.int(HRM.col(r, 'AdvanceId')), b);
        });
        function GridBind() { return HRM.get(API + '/list').then(function (rows) { grd.set(rows || []); }).catch(HRM.fail); }
        function GenerateCode() {                                             // GenerateCode()
            return HRM.get(API + '/code').then(function (d) { HRM.setVal('txtDocNo', HRM.str(d && d.docNo)); }).catch(HRM.fail);
        }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function Reset() {                                                    // Reset()
            RecId = 0;
            HRM.setCombo('cmbEmployeeName', 0);
            ['txtAmount', 'txtCurrentAmount', 'txtReason', 'txtSalary', 'txtEmployeeNo', 'txtDepartment', 'txtDesgination', 'txtSection']
                .forEach(function (i) { HRM.setVal(i, ''); });
            HRM.focus('cmbEmployeeName');
            buttons(false);
            UpdateMode = false;
            return GridBind().then(GenerateCode);
        }
        /** SalaryAmountCalculation(): Math.Round(Salary / 30 * datAppliedOn.Day, 2). */
        function SalaryAmountCalculation() {
            var day = HRM.val('datAppliedOn') ? +HRM.val('datAppliedOn').slice(8, 10) : new Date().getDate();
            if (day > 0) HRM.setVal('txtCurrentAmount', String(roundEven(HRM.num(HRM.val('txtSalary').trim()) / 30.0 * day, 2)));
            else HRM.setVal('txtCurrentAmount', '0');
        }
        /** PreviousAdvanceAmount(): USP_GetPreviousAdvanceAmount for the employee and the Applied On month / year. */
        var prevSeq = 0;
        function PreviousAdvanceAmount() {
            var seq = ++prevSeq;
            return HRM.get(API + '/previous', { employeeId: HRM.comboVal('cmbEmployeeName'), appliedOn: HRM.val('datAppliedOn') }).then(function (d) {
                if (seq !== prevSeq) return;
                if (d && d.found) {
                    HRM.setVal('txtPreviousAdvance', String(HRM.num(d.AdvanceAmount)));
                    HRM.setVal('txtPreivousApprovedAmount', String(HRM.num(d.ApprovedAmount)));
                } else { HRM.setVal('txtPreviousAdvance', '0'); HRM.setVal('txtPreivousApprovedAmount', '0'); }
            }).catch(HRM.fail);
        }
        function cmbEmployeeName_Leave() {                                    // cmbEmployeeName_Leave
            employeeInfo(dtemployee);
            SalaryAmountCalculation();
            return PreviousAdvanceAmount();
        }
        function RetrivedData(id) {                                           // RetrivedData(Id)
            RecId = id;
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                HRM.setVal('txtDocNo', HRM.str(HRM.col(o, 'DocNo')));
                HRM.setVal('datRequestDate', HRM.day(HRM.col(o, 'DocDate')));
                HRM.setVal('datAppliedOn', HRM.day(HRM.col(o, 'AppliedOn')));
                HRM.setCombo('cmbEmployeeName', HRM.int(HRM.col(o, 'EmployeeId')));
                HRM.setVal('txtReason', HRM.str(HRM.col(o, 'Reason')));
                HRM.setVal('txtAmount', HRM.str(HRM.col(o, 'AdvanceAmount')));
                HRM.setVal('txtPreviousAdvance', HRM.str(HRM.col(o, 'PreviousAdvanceAmount')));
                HRM.setVal('txtPreivousApprovedAmount', HRM.str(HRM.col(o, 'PreviousApprovedAmount')));
                cmbEmployeeName_Leave();
                buttons(true);
                HRM.focus('cmbEmployeeName');
            }).catch(HRM.fail);
        }
        function Validation() {
            if (!HRM.comboVal('cmbEmployeeName')) { HRM.box('Employee Name Required!'); HRM.focus('cmbEmployeeName'); return false; }
            if (HRM.num(HRM.val('txtAmount')) === 0) { HRM.box('Advance Amount Field Required!'); HRM.focus('txtAmount'); return false; }
            return true;
        }
        function Insert(btn) {                                                // Insert() - no confirmation on this form
            if (!Validation()) return;
            var upd = RecId > 0;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId, employeeId: HRM.comboVal('cmbEmployeeName'), docNo: HRM.val('txtDocNo'),
                    requestDate: HRM.val('datRequestDate'), appliedOn: HRM.val('datAppliedOn'), advanceAmount: HRM.val('txtAmount'),
                    previousAdvance: HRM.val('txtPreviousAdvance'), previousApprovedAmount: HRM.val('txtPreivousApprovedAmount'),
                    reason: HRM.val('txtReason')
                }).then(function (d) {
                    HRM.box(d && d.message ? d.message : (upd ? 'Update Successfully' : 'Saved Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'advance-save');
        }
        /** GenerateSlip(PrintId): USP_EmployeeAdvanceSlip rows, else "Not Record Found For Display"; 1008-EmployeeAdvanceSlip.rpt. */
        function GenerateSlip(id, btn) {
            var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
            return HRM.busy(btn, function () {
                return HRM.get(API + '/slip-check', { id: id }).then(function () {
                    if (!window.CrystalPrint) { HRM.box('Report file 1008-EmployeeAdvanceSlip.rpt is not available.'); return; }
                    return window.CrystalPrint.open('hrm-1008', { id: id }, null, win);
                }).catch(function (e) { if (win) window.CrystalPrint.release(win); HRM.fail(e); });
            }, 'advance-print');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };            // btnupdate_Click
        P.btnnew = function () { return Reset(); };                                     // btnnew_Click
        P.btnRefresh = function (btn) {                                                  // btnRefresh_Click -> EmployeeName()
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/employees').then(function (rows) { dtemployee = rows || []; fillEmployees(dtemployee, true); }).catch(HRM.fail);
            });
        };
        P.btnPrint = function (btn) { return GenerateSlip(RecId, btn || 'btnPrint'); };  // btnPrint_Click -> GenerateSlip(RecId)
        P.btnEmployeeRegister = function () { employeeRegister(canRegister); };          // btnEmployeeRegister_Click
        onComboLeave(cmbEmployeeName_Leave);                                             // cmbEmployeeName_Leave
        HRM.$('datAppliedOn').addEventListener('change', function () { PreviousAdvanceAmount(); });   // datAppliedOn_ValueChanged
        HRM.$('datAppliedOn').addEventListener('blur', function () { SalaryAmountCalculation(); PreviousAdvanceAmount(); });  // datAppliedOn_Leave
        HRM.keys({                                                                       // EmployeeFamilyInfo_KeyDown
            'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled && !UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled && UpdateMode) P.btnupdate(); },
            'ctrl+enter': function () {
                var r = grd.current();
                if (r && document.activeElement && document.activeElement.closest('#grd')) { RetrivedData(HRM.int(HRM.col(r, 'AdvanceId'))); UpdateMode = true; }
            }
        });
        HRM.footer(function (b) {                                                        // History = the form's own History grid
            return HRM.busy(b, function () { return GridBind().then(function () { HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); }); });
        });
        HRM.setVal('datRequestDate', HRM.today());                                       // DateTimePicker defaults: now
        HRM.setVal('datAppliedOn', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // EmployeeFamilyInfo_Load
            rights = d.rights || {};
            canRegister = !!d.canRegister;
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
            HRM.setVal('txtDocNo', HRM.str(d.docNo));
            dtemployee = d.employees || [];
            fillEmployees(dtemployee);
            grd.set(d.rows || []);
            buttons(false);
            HRM.focus('cmbEmployeeName');
            var id = HRM.int(HRM.param('id'));
            if (id > 0) { RetrivedData(id); UpdateMode = true; }
        }).catch(HRM.fail);
    }

    P._roundEven = roundEven;
    P._fmtHash = fmtHash;
    var PAGES = { 'employee-loan': employeeLoan, 'employee-advance': employeeAdvance };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
