/* ============================================================================================
 * countx_hrm_approval.js - HRM "Approval Management" (AppModules 2026). One script, the page is
 * chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM); exports window.HrmApproval.
 *
 *   loan-approval      ApprovalManagement/LoanApproval.cs     (667)
 *   leave-approval     ApprovalManagement/LeaveApproval.cs    (668) + PendingLeaveDetailForApproval.cs (the Detail modal)
 *   advance-approval   ApprovalManagement/AdvanceApproval.cs  (669)
 *
 * Each handler follows its desktop form line by line. The grids' document column is a link that opens
 * the source document (Employee Loan / Employee Advance / Employee Leave with ?id=) in a new window.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/approval/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmApproval
    window.HrmApproval = P;

    // ------------------------------------------------------------------------ .NET number text
    /** Convert.ToInt32(decimal / double): rounds half to even. */
    function toIntEven(v) {
        var x = HRM.num(v), r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r;
    }
    /**
     * .NET custom numeric format with grouping: maxDec optional decimals, minInt integer digits (0 -> "" when the
     * whole result is zero, as "#,#" / "#,#.###" print nothing for 0). Custom formats round half away from zero.
     */
    function netFmt(v, maxDec, minInt) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v), p = Math.pow(10, maxDec), neg = n < 0;
        var a = Math.round(Math.abs(n) * p + 1e-9) / p;
        var s = a.toFixed(maxDec);
        if (maxDec > 0) s = s.replace(/0+$/, '').replace(/\.$/, '');
        var parts = s.split('.'), ip = parts[0];
        if (ip === '0') ip = '';
        while (ip.length < minInt) ip = '0' + ip;
        ip = ip.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ip + (parts[1] ? '.' + parts[1] : '');
        return out === '' ? '' : (neg && a !== 0 ? '-' : '') + out;
    }
    function btnCell(act, text) { return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    /** GridEX ColumnButtonClick: a click on a button cell of the table. */
    function onButton(grid, table, fn) {
        HRM.$(table).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), b);
        });
    }
    function fillCombo(id, rows, v, t) { HRM.fill(id, rows, v, t, { zero: '' }); }   // BindDDLNew(..., false): no zero row, nothing active

    // ============================================================================ 667 Loan Approval
    function loanApproval() {
        var RecId = 0, UpdateMode = false;
        var LOADED = null;                                                    // cmbEmployeeName bound to the Load row (Id, Name, LoanId)
        var grd = new HRM.Grid('grd', {                                       // GenerateApprovedLoan dt + grdSetting()
            columns: [
                { key: 'EmployeeLoanInstallmentId', caption: 'EmployeeLoanInstallmentId', hidden: true },
                { key: 'Installments', caption: 'Installments' },
                { key: 'Month', caption: 'Month' },
                { key: 'Year', caption: 'Year' },
                { key: 'Amount', caption: 'Amount', type: 'num', align: 'right', sum: true, decimals: 2,
                    render: function (v) { return HRM.esc(netFmt(v, 3, 0)); } }                    // "#,#.###", total "#,##0.##"
            ],
            filterRow: true, totals: true
        });
        var money = function (v) { return HRM.esc(HRM.fmtNum(v, 2)); };                         // "#,##0.00"
        var grdPendingForLoan = new HRM.Grid('grdPendingForLoan', {           // PendingLoanForApproval + grdPendingForLoanSetting()
            columns: [
                { key: 'RecordNo', caption: 'RecordNo', hidden: true },
                { key: 'EmployeeLoanId', caption: 'EmployeeLoanId', hidden: true },
                { key: 'DocNo', caption: 'DocNo', type: 'code' },
                { key: 'DocumentTypeId', caption: 'DocumentTypeId' },
                { key: 'EmployeeName', caption: 'EmployeeName' },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'NoOfInstallment', caption: 'NoOfInstallment' },
                { key: 'AppliedOn', caption: 'AppliedOn', type: 'datetime' },
                { key: 'LoanAmount', caption: 'LoanAmount', type: 'num', align: 'right', sum: true, decimals: 2, render: money },
                { key: 'ApprovedAmount', caption: 'ApprovedAmount', type: 'num', align: 'right', sum: true, decimals: 2, render: money },
                { key: 'Reason', caption: 'Reason' },
                { key: 'DepartmentName', caption: 'DepartmentName' },
                { key: 'DesignationName', caption: 'DesignationName' },
                { key: 'CreatedBy', caption: 'CreatedBy' },
                { key: 'AlteredBy', caption: 'AlteredBy' },
                { key: 'ApprovedBy', caption: 'ApprovedBy' },
                { key: 'ApprovedOn', caption: 'ApprovedOn', type: 'datetime' },
                { key: 'IsApproved', caption: 'IsApproved' },
                { key: 'CreatedOn', caption: 'CreatedOn', type: 'datetime' },
                { key: 'NoOfAttachments', caption: 'NoOfAttachments' },
                { key: 'DeductNoOfInstallment', caption: 'DeductNoOfInstallment' },
                { key: 'RemaningNoOfInstallment', caption: 'RemaningNoOfInstallment' },
                { key: 'InstallmentAmount', caption: 'InstallmentAmount', type: 'num' },
                { key: 'Load', caption: 'Load', width: 80, render: function () { return btnCell('load', 'Load'); } }
            ],
            filterRow: true, totals: true,
            onCode: function (r) { HRM.open('/hrm/employee-loan?id=' + HRM.int(HRM.col(r, 'EmployeeLoanId'))); }   // the source document
        });
        onButton(grdPendingForLoan, 'grdPendingForLoan', function (r, act) { if (act === 'load') ColumnButtonClick(r); });
        var employees = [];

        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function PendingLoanForApproval() {                                  // PendingLoanForApproval(): ClearStructure when empty
            return HRM.get(API + '/list').then(function (rows) { grdPendingForLoan.set(rows || []); }).catch(HRM.fail);
        }
        /** grdPendingForLoan_ColumnButtonClick (Load): the combo re-bound to that one row, the date, installments and amount. */
        function ColumnButtonClick(item) {
            if (!item) return;
            LOADED = { Id: HRM.int(HRM.col(item, 'EmployeeId')), Name: HRM.str(HRM.col(item, 'EmployeeName')), LoanId: HRM.int(HRM.col(item, 'EmployeeLoanId')) };
            HRM.fill('cmbEmployeeName', [LOADED], 'Id', 'Name', { zero: false });   // BindDDL(dtEmployee, ..., ZeroIndex false); Rows[0].Activate()
            HRM.setVal('datAppliedOn', HRM.day(HRM.col(item, 'AppliedOn')));
            HRM.setVal('txtNoofInstallments', String(toIntEven(HRM.col(item, 'NoOfInstallment'))));
            HRM.setVal('txtLoanAmount', String(toIntEven(HRM.col(item, 'LoanAmount'))));
            HRM.focus('txtApprovedAmount');
        }
        function clearInstallments() { grd.clear(); }
        /** GenerateApprovedLoan(): validations, then NoOfInstallments rows of ApprovedAmount / n from Applied On month by month. */
        function GenerateApprovedLoan() {
            if (!HRM.comboVal('cmbEmployeeName')) { clearInstallments(); HRM.box('EmployeeName Field Required....'); HRM.focus('cmbEmployeeName'); return; }
            if (HRM.num(HRM.val('txtApprovedAmount')) === 0) { clearInstallments(); HRM.box('ApprovedAmount Field Required....'); HRM.focus('txtApprovedAmount'); return; }
            if (HRM.num(HRM.val('txtApprovedAmount')) > HRM.num(HRM.val('txtLoanAmount'))) { clearInstallments(); HRM.box('ApprovedAmount Can not greater than LoanAmount....'); return; }
            if (HRM.int(HRM.val('txtNoofInstallments')) > 60) { clearInstallments(); HRM.box('No Of Installments Can not greater than 60....'); return; }
            var rows = [];
            var n = HRM.int(HRM.val('txtNoofInstallments'));
            if (HRM.num(HRM.val('txtApprovedAmount')) > 0 && n > 0) {
                var amount = HRM.num(HRM.val('txtApprovedAmount')) / n;
                var base = HRM.val('datAppliedOn') || HRM.today();
                var y0 = +base.slice(0, 4), m0 = +base.slice(5, 7) - 1;
                for (var i = 1, month = 0; i <= n; i++, month++) {                 // datAppliedOn.Value.AddMonths(month)
                    var d = new Date(y0, m0 + month, 1);
                    rows.push({ EmployeeLoanInstallmentId: 0, Installments: i, Month: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][d.getMonth()],
                        Year: d.getFullYear(), Amount: amount });
                }
                grd.set(rows);
            }
        }
        function Validation() {
            if (!HRM.comboVal('cmbEmployeeName')) { HRM.box('Employee Name Required!'); HRM.focus('cmbEmployeeName'); return false; }
            if (HRM.num(HRM.val('txtApprovedAmount')) === 0) { HRM.box('LoanAmount Field Required!'); HRM.focus('txtApprovedAmount'); return false; }
            if (HRM.num(HRM.val('txtNoofInstallments')) === 0) { HRM.box('No Of Installments Field Required!'); HRM.focus('txtNoofInstallments'); return false; }
            return true;
        }
        function Reset() {                                                    // Reset()
            RecId = 0;
            LOADED = null;
            HRM.fill('cmbEmployeeName', [], 'Id', 'Name', { zero: '' });     // Text = ""; DataSource = null
            HRM.setVal('txtApprovedAmount', ''); HRM.setVal('txtLoanAmount', ''); HRM.setVal('txtNoofInstallments', '');
            HRM.focus('txtApprovedAmount');
            buttons(false);
            grd.clear();
            return PendingLoanForApproval();
        }
        function Insert(btn) {                                                // Insert()
            if (!Validation() || !HRM.ask('Are you sure to Save?')) return;
            var row = LOADED && HRM.comboVal('cmbEmployeeName') === LOADED.Id ? LOADED : null;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    employeeId: HRM.comboVal('cmbEmployeeName'),
                    loanId: row ? row.LoanId : 0,                                  // SelectedRow.Cells[2] - absent on the full employee list
                    loanAmount: HRM.val('txtLoanAmount'), approvedAmount: HRM.val('txtApprovedAmount'),
                    noOfInstallments: HRM.val('txtNoofInstallments'), appliedOn: HRM.val('datAppliedOn'),
                    details: grd.rows().map(function (r) {
                        return { employeeLoanInstallmentId: HRM.int(r.EmployeeLoanInstallmentId), installments: HRM.int(r.Installments),
                            month: HRM.str(r.Month), year: HRM.int(r.Year), amount: HRM.num(r.Amount) };
                    })
                }).then(function (d) {
                    HRM.box(RecId > 0 ? 'Update Successfully' : (d && d.message) || 'Saved Successfully');
                    return Reset();
                }).catch(HRM.fail);
            }, 'loan-approval-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };     // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };           // btnupdate_Click
        P.btnnew = function () { return Reset(); };                                    // btnnew_Click
        P.btnGenerate = function () { GenerateApprovedLoan(); };                       // btnGenerate_Click
        HRM.keys({                                                                      // EmployeeFamilyInfo_KeyDown
            'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled && !UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled && UpdateMode) P.btnupdate(); }
        });
        HRM.footer(function (b) {                                                       // History = the pending grid, refreshed
            return HRM.busy(b, function () { return PendingLoanForApproval().then(function () { HRM.$('boxPending').scrollIntoView({ block: 'nearest' }); }); });
        });
        HRM.setVal('datAppliedOn', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // EmployeeFamilyInfo_Load
            employees = d.employees || [];
            if (employees.length) fillCombo('cmbEmployeeName', employees.map(function (e) { return { Id: e.EmployeeId, Name: e.EmployeeName }; }), 'Id', 'Name');
            grdPendingForLoan.set(d.rows || []);
            buttons(false);
            HRM.enable('datAppliedOn', false);
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ Leave / Advance: filter combos
    /** DepartmentFill / DesignationFill / EmployeeFill + cmbDepartment_Leave / cmbDesignation_Leave -> EmployeeFill(). */
    function filterCombos(d) {
        if ((d.departments || []).length) fillCombo('cmbDepartment', d.departments, 'DepartmentId', 'DepartmentName');
        if ((d.designations || []).length) fillCombo('cmbDesignation', d.designations, 'DesignationId', 'DesignationName');
        if ((d.employees || []).length) fillCombo('cmbEmployeeName', d.employees, 'EmployeeId', 'EmployeeName');
    }
    function EmployeeFill() {
        return HRM.get(API + '/employees', { departmentId: HRM.comboVal('cmbDepartment'), designationId: HRM.comboVal('cmbDesignation') })
            .then(function (rows) { if (rows && rows.length) HRM.fill('cmbEmployeeName', rows, 'EmployeeId', 'EmployeeName', { zero: '', keep: true }); })
            .catch(HRM.fail);
    }
    function wireFilterLeave() {
        ['cmbDepartment', 'cmbDesignation'].forEach(function (id) {
            HRM.$(id).addEventListener('change', EmployeeFill);
        });
    }
    function filters() {
        return { employeeId: HRM.comboVal('cmbEmployeeName'), departmentId: HRM.comboVal('cmbDepartment'), designationId: HRM.comboVal('cmbDesignation') };
    }
    function clearFilters() {                                                  // Reset(): the three combos' Text = ""
        ['cmbEmployeeName', 'cmbDepartment', 'cmbDesignation'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.focus('cmbEmployeeName');
    }

    // ============================================================================ 668 Leave Approval
    function leaveApproval() {
        var grdHistory = new HRM.Grid('grdHistory', {                         // HistoryForApproval + grdPendingForLoanSetting()
            columns: [
                { key: 'EmployeeLeaveId', caption: 'EmployeeLeaveId', hidden: true },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'EmployeeNo', caption: 'EmployeeNo', width: 120, type: 'code' },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 180 },
                { key: 'DepartmentName', caption: 'DepartmentName', width: 180 },
                { key: 'DesignationName', caption: 'DesignationName', width: 180 },
                { key: 'LeaveTypeProfileId', caption: 'LeaveTypeProfileId', hidden: true },
                { key: 'LeaveType', caption: 'LeaveType', width: 180 },
                { key: 'EmployeeLeaveQuotaId', caption: 'EmployeeLeaveQuotaId', hidden: true },
                { key: 'Reason', caption: 'Reason', width: 220 },
                { key: 'NoofLeaves', caption: 'NoofLeaves' },
                { key: 'Select', caption: 'Select', width: 40, type: 'edit-check' },          // ActAsSelector + UseHeaderSelector
                { key: 'Detail', caption: 'Detail', width: 50, render: function () { return btnCell('detail', 'Detail'); } }
            ],
            filterRow: true, checkAll: 'Select',
            onCode: function (r) { HRM.open('/hrm/employee-leave?id=' + HRM.int(HRM.col(r, 'EmployeeLeaveId'))); }   // the source leave request
        });
        onButton(grdHistory, 'grdHistory', function (r, act) { if (act === 'detail') showDetail(r); });   // grdHistory_ColumnButtonClick

        function HistoryForApproval() {                                        // HistoryForApproval()
            return HRM.get(API + '/list', filters()).then(function (rows) { grdHistory.set(rows || []); }).catch(HRM.fail);
        }
        function Reset() { clearFilters(); return HistoryForApproval(); }

        /** PendingLeaveDetailForApproval (EmployeeId, LeaveTypeProfileId) - a modal here; the desktop Show()s it. */
        function showDetail(item) {
            var EmployeeId = HRM.int(HRM.col(item, 'EmployeeId')), LeaveTypeProfileId = HRM.int(HRM.col(item, 'LeaveTypeProfileId'));
            var m = HRM.modal({
                title: 'Detail', width: 'min(1200px, 96vw)',
                html: '<div class="hrm-grid-box hrm-grid-box-modal"><div class="hrm-subcaption"><span>Detail</span>' +
                    '<button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button></div>' +
                    '<div class="hrm-grid-wrap"><table id="grdLeaveDetail"></table></div></div>'
            });
            var g = new HRM.Grid(m.body.querySelector('#grdLeaveDetail'), {   // grdPendingForLoanSetting() of the popup
                columns: [
                    { key: 'Approve', caption: 'Approve', width: 50, render: function () { return btnCell('approve', 'Approve'); } },   // Position 0
                    { key: 'Reject', caption: 'Reject', width: 50, render: function () { return btnCell('reject', 'Reject'); } },       // Position 1
                    { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                    { key: 'EmployeeNo', caption: 'EmployeeNo' },
                    { key: 'EmployeeName', caption: 'EmployeeName' },
                    { key: 'DepartmentName', caption: 'DepartmentName' },
                    { key: 'DesignationName', caption: 'DesignationName' },
                    { key: 'LeaveTypeProfileId', caption: 'LeaveTypeProfileId', hidden: true },
                    { key: 'LeaveType', caption: 'LeaveType' },
                    { key: 'EmployeeLeaveQuotaId', caption: 'EmployeeLeaveQuotaId', hidden: true },
                    { key: 'LeaveReson', caption: 'LeaveReson' },
                    { key: 'EmployeeLeaveDetailId', caption: 'EmployeeLeaveDetailId', hidden: true },
                    { key: 'LeaveDate', caption: 'LeaveDate', type: 'date' },
                    { key: 'Reason', caption: 'Reason', width: 200, type: 'edit' }                  // the grid allows edit (designer default)
                ],
                filterRow: true, totals: true, emptyText: ''
            });
            function load() {                                                   // HistoryForApproval(EmployeeId) / Reset()
                return HRM.get(API + '/details', { employeeId: EmployeeId, leaveTypeProfileId: LeaveTypeProfileId })
                    .then(function (rows) { g.set(rows || []); }).catch(HRM.fail);
            }
            onButton(g, m.body.querySelector('#grdLeaveDetail'), function (r, act, b) {   // grdHistory_ColumnButtonClick
                if (HRM.str(HRM.col(r, 'Reason')) === '') { HRM.box('Reason Field Required'); return; }
                var reject = act === 'reject';
                if (!HRM.ask(reject ? 'Are you sure to Reject?' : 'Are you sure to Approve?')) return;
                return HRM.busy(b, function () {
                    return HRM.post(API + (reject ? '/detail-reject' : '/detail-approve'), {
                        employeeId: EmployeeId, leaveTypeProfileId: LeaveTypeProfileId,
                        employeeLeaveDetailId: HRM.int(HRM.col(r, 'EmployeeLeaveDetailId')), reason: HRM.str(HRM.col(r, 'Reason'))
                    }).then(load).catch(HRM.fail);
                }, 'leave-detail-action');
            });
            HRM.loading(load());
            return { modal: m, grid: g };
        }

        P.btnnew = function () { return Reset(); };                            // btnnew_Click
        P.btnSearch = function (btn) { return HRM.busy(btn || 'btnSearch', HistoryForApproval); };   // btnSearch_Click
        P.btnApprove = function (btn) {                                         // btnApprove_Click
            if (!HRM.ask('Are you sure to Approve?')) return;
            var ids = grdHistory.checked('Select').map(function (r) { return HRM.int(HRM.col(r, 'EmployeeLeaveId')); });
            return HRM.busy(btn || 'btnApprove', function () {
                return HRM.post(API + '/approve', { employeeLeaveIds: ids }).then(function () {
                    HRM.box('Approve Successfully');
                    return Reset();
                }).catch(HRM.fail);
            }, 'leave-approve');
        };
        P._showDetail = showDetail;
        wireFilterLeave();
        HRM.keys({ 'ctrl+n': function () { P.btnnew(); }, 'ctrl+e': HRM.close, 'esc': HRM.close });   // EmployeeFamilyInfo_KeyDown
        HRM.footer(function (b) {                                                // History = the pending grid, refreshed
            return HRM.busy(b, function () { return HistoryForApproval().then(function () { HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); }); });
        });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                 // EmployeeFamilyInfo_Load
            filterCombos(d);
            grdHistory.set(d.rows || []);
        }).catch(HRM.fail);
    }

    // ============================================================================ 669 Advance Approval
    function advanceApproval() {
        var fmt00 = function (v) { return HRM.esc(netFmt(v, 0, 2)); };        // FormatString "0,0"
        var grdHistory = new HRM.Grid('grdHistory', {                         // HistoryForApproval + grdPendingForLoanSetting()
            columns: [
                { key: 'Approve', caption: 'Approve', width: 100, render: function () { return btnCell('approve', 'Approve'); } },   // Position 0, frozen
                { key: 'RequestNo', caption: 'RequestNo', type: 'code' },
                { key: 'RequestDate', caption: 'RequestDate', type: 'datetime' },
                { key: 'EmployeeAdvanceId', caption: 'EmployeeAdvanceId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName' },
                { key: 'AppliedOn', caption: 'AppliedOn', type: 'datetime' },
                { key: 'AdvanceAmount', caption: 'AdvanceAmount', type: 'num', align: 'right', sum: true, decimals: 2, render: fmt00 },
                { key: 'ApprovedAmount', caption: 'ApprovedAmount', type: 'edit-num', align: 'right', sum: true, decimals: 2 },   // EditType TextBox
                { key: 'ApprovalRemarks', caption: 'ApprovalRemarks' },
                { key: 'Reason', caption: 'Reason' },
                { key: 'DepartmentName', caption: 'DepartmentName' },
                { key: 'DesignationName', caption: 'DesignationName' },
                { key: 'CreatedBy', caption: 'CreatedBy' },
                { key: 'AlteredBy', caption: 'AlteredBy' },
                { key: 'ApprovedBy', caption: 'ApprovedBy' },
                { key: 'ApprovedOn', caption: 'ApprovedOn', type: 'datetime' },
                { key: 'IsApproved', caption: 'IsApproved', type: 'check' },
                { key: 'CreatedOn', caption: 'CreatedOn', type: 'datetime' }
            ],
            filterRow: true, totals: true,
            onCode: function (r) { HRM.open('/hrm/employee-advance?id=' + HRM.int(HRM.col(r, 'EmployeeAdvanceId'))); }   // the source document
        });
        function HistoryForApproval() {                                        // HistoryForApproval()
            return HRM.get(API + '/list', filters()).then(function (rows) { grdHistory.set(rows || []); }).catch(HRM.fail);
        }
        function Reset() { clearFilters(); return HistoryForApproval(); }
        /** grdPendingForLoan_ColumnButtonClick (Approve). */
        onButton(grdHistory, 'grdHistory', function (item, act, b) {
            if (act !== 'approve' || !item) return;
            if (!HRM.ask('Are you sure to Approve?')) return;
            var approved = HRM.num(HRM.col(item, 'ApprovedAmount'));
            if (approved === 0) { HRM.box('Approved Amount Field Required'); return; }
            if (approved > HRM.num(HRM.col(item, 'AdvanceAmount'))) { HRM.box('Approved Amount Can not Greater than Advance Amount'); return; }
            if (toIntEven(approved) === 0) { HRM.box('Approve Amount Required'); return; }
            return HRM.busy(b, function () {
                return HRM.post(API + '/approve', { employeeAdvanceId: HRM.int(HRM.col(item, 'EmployeeAdvanceId')), approvedAmount: HRM.str(HRM.col(item, 'ApprovedAmount')) })
                    .then(function () { HRM.box('Approve Successfully'); return Reset(); }).catch(HRM.fail);
            }, 'advance-approve');
        });
        P.btnnew = function () { return Reset(); };                            // btnnew_Click
        P.btnSearch = function (btn) { return HRM.busy(btn || 'btnSearch', HistoryForApproval); };   // btnSearch_Click
        wireFilterLeave();
        HRM.keys({ 'ctrl+n': function () { P.btnnew(); }, 'ctrl+e': HRM.close, 'esc': HRM.close });   // EmployeeFamilyInfo_KeyDown
        HRM.footer(function (b) {
            return HRM.busy(b, function () { return HistoryForApproval().then(function () { HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); }); });
        });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                 // EmployeeFamilyInfo_Load
            filterCombos(d);
            grdHistory.set(d.rows || []);
        }).catch(HRM.fail);
    }

    P._netFmt = netFmt;
    P._toIntEven = toIntEven;
    var PAGES = { 'loan-approval': loanApproval, 'leave-approval': leaveApproval, 'advance-approval': advanceApproval };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
