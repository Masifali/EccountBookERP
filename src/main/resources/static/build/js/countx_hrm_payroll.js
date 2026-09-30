/* ============================================================================================
 * countx_hrm_payroll.js - HRM "Payroll" (AppModules 2027). One script, the page is chosen by
 * <body data-hrm="...">. Built on countx_hrm.js (window.HRM); exports window.HrmPayroll.
 *
 *   payroll-posting            frmPayrollPosting.cs (670)
 *   employee-allowance         frmEmployeeAllowance.cs (671)
 *   employee-late-adjustment   frmEmployeeLateAdjustment.cs (672)
 *   employee-loan-deduction    EmployeeLoanDeduction.cs (673)
 *   employee-short-adjustment  EmployeeShortAdjustment.cs (674)
 *
 * Each handler follows its desktop method line by line: validation wording and order, messages,
 * what New / Save / Update / double-click do to the buttons and the grids. Values typed into text
 * boxes are sent as typed; the server applies the desktop's Conversion.ToInt / ToDecimal to them.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/payroll/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmPayroll
    window.HrmPayroll = P;

    /* CommonServices.GetMonths / the forms' own MonthFill: Id 1..12, Name January..December. */
    var MONTHS = HRM.MONTHS.map(function (n, i) { return { Id: i + 1, Name: n }; });

    /** Conversion.ToDouble of a text box / cell (0 when not a number). */
    function dbl(v) { return HRM.num(v); }

    /** "#,#" (Janus FormatString): whole number with group separators, blank for 0. */
    function fmtHash(v) { var n = Math.round(HRM.num(v)); return n === 0 ? '' : HRM.fmtNum(n, 0); }

    /** "0,0": group separators, at least two digits (5 -> "05"), rounded half away from zero. */
    function fmt00(v) {
        var n = HRM.num(v), neg = n < 0, a = Math.round(Math.abs(n));
        var s = String(a); if (s.length < 2) s = '0' + s;
        s = s.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (neg && a !== 0 ? '-' : '') + s;
    }

    /** "#,#0.##": group separators, up to two decimals, no trailing zeros. */
    function fmtUpTo2(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = Math.round(HRM.num(v) * 100) / 100;
        var p = String(Math.abs(n)).split('.');
        p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + p.join('.');
    }

    /** A grid whose columns change (GridEX.RetrieveStructure + settings): a fresh <table> each time. */
    function freshTable(id) {
        var t = HRM.$(id); if (!t) return;
        var n = t.cloneNode(false);
        n.removeAttribute('class');
        t.parentNode.replaceChild(n, t);
    }

    /**
     * GridEX TotalFormatString: HRM.Grid prints totals with fixed decimals; a column carrying
     * totalFmt gets its footer cell re-printed with the desktop's own total format.
     */
    function fixTotals(g) {
        if (!g.table || !g.table.tFoot || !g.table.tFoot.rows.length) return;
        var cells = g.table.tFoot.rows[0].cells;
        g.columns.forEach(function (c, i) {
            if (c.sum && c.totalFmt && cells[i]) cells[i].textContent = c.totalFmt(g.sum(c.key, true));
        });
    }

    /** "#,##0.##" */
    function fmtHash0(v) { return fmtUpTo2(v) || '0'; }

    function scrollTo(id) { var e = HRM.$(id); if (e && e.scrollIntoView) e.scrollIntoView({ block: 'start', behavior: 'smooth' }); }

    // ============================================================================ 670 Payroll Posting
    function payrollPosting() {
        var RecId = 0, rights = {}, lstDelete = [], hideDept = false, decimals = 2, grd = null, reportsOpen = false;

        var AMOUNTS = ['GrossSalary', 'EmployeePerDaySalary', 'EmployeePayrollSalary', 'LateDeductionAmount', 'IncomeTaxAmount',
            'LeaveAmount', 'AdvanceAmount', 'LoanAmount'];
        var AMOUNTS2 = ['PFAmount', 'EOBIAmount', 'MiscLessAmount', 'TotalLessAmount', 'OTAmount', 'TravellingAmount', 'MedicalAmount',
            'MobileAmount', 'FuelAmount', 'FoodAmount', 'MiscAddAmount'];
        /* grdSettings(): dtdetail's columns in order; Id / AccountId / EmployeeId / EmployeeHistoryId / ArearAmount hidden;
           Designation / Department / Section hidden when RecId > 0; every amount Sum + stringFormatsingle; only
           AddLessAmount editable (all others EditType NoEdit); "Delete" button column "X" (20) last. */
        function columns() {
            var amt = function (k) { return { key: k, caption: k, type: 'num', decimals: decimals, sum: true }; };
            var c = [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'AccountId', caption: 'AccountId', hidden: true },
                { key: 'AccountCode', caption: 'AccountCode' },
                { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'int' },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 180 },
                { key: 'DesignationName', caption: 'DesignationName', hidden: hideDept },
                { key: 'DepartmentName', caption: 'DepartmentName', hidden: hideDept },
                { key: 'SectionName', caption: 'SectionName', hidden: hideDept },
                { key: 'EmployeeHistoryId', caption: 'EmployeeHistoryId', hidden: true },
                amt('GrossSalary'),
                { key: 'EmployeeDays', caption: 'EmployeeDays' }
            ];
            AMOUNTS.slice(1).forEach(function (k) { c.push(amt(k)); });
            c.push({ key: 'LoanNOS', caption: 'LoanNOS', type: 'int' });
            AMOUNTS2.forEach(function (k) { c.push(amt(k)); });
            c.push({ key: 'ArearAmount', caption: 'ArearAmount', hidden: true, type: 'num', decimals: decimals, sum: true });
            c.push(amt('Salary'));
            c.push({ key: 'AddLessAmount', caption: 'AddLessAmount', type: 'edit-num', decimals: decimals, sum: true, redraw: true });
            c.push(amt('NetSalary'));
            c.push({ key: 'Delete', caption: 'X', width: 20, align: 'center', render: function () {
                return '<button type="button" class="pr-x-btn" data-del="1" title="Delete">X</button>'; } });
            return c;
        }
        function bind(rows) {                                               // grdDetails.DataSource = dtdetail; RetrieveStructure; grdSettings
            hideDept = RecId > 0;
            freshTable('grdDetails');
            grd = new HRM.Grid('grdDetails', {
                columns: columns(), filterRow: true, totals: true, emptyText: '',
                onChange: function (r, k) {                                  // grdDetails_CellUpdated
                    if (k !== 'AddLessAmount') return;
                    var AddLessAmount = dbl(r.AddLessAmount), Salary = dbl(r.Salary);
                    r.NetSalary = parseFloat((Salary + AddLessAmount).toPrecision(15));   // (Salary + AddLessAmount).ToString()
                }
            });
            grd.set(rows || []);
        }
        HRM.$('grdDetailsWrap').addEventListener('click', function (e) {     // grdDetails_ColumnButtonClick "Delete"
            var b = e.target.closest('button[data-del]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            if (!HRM.ask('Are you sure to Delete?')) return;
            var r = grd.rows()[i];
            if (HRM.int(r.Id) > 0) lstDelete.push(Object.assign({}, r));
            grd.remove(i);
        });

        function fillCombos(d) {
            if (d.departments && d.departments.length) HRM.fill('cmbEmployeeDepartment', d.departments, 'Id', 'Name');   // only when rows > 0
            if (d.categories && d.categories.length) HRM.fill('cmbEmployeeCategory', d.categories, 'Id', 'Name');
            HRM.fill('cmbYear', d.years || [], 'Id', 'Year');
            HRM.fill('cmbMonth', MONTHS, 'Id', 'Name');
        }
        function buttons(update) {
            HRM.show('btnsave', !update); HRM.show('btnUpdate', update);
        }
        function GenerateCode() {
            return HRM.get(API + '/code').then(function (d) { HRM.setVal('txtDocNo', HRM.str(d.docNo)); }).catch(HRM.fail);
        }
        function Reset() {                                                  // Reset()
            HRM.setVal('txtDocDate', HRM.today());
            HRM.setCombo('cmbMonth', 0); HRM.setCombo('cmbYear', 0);
            HRM.setCombo('cmbEmployeeCategory', 0); HRM.setCombo('cmbEmployeeDepartment', 0);
            RecId = 0;
            grd.clear();
            buttons(false);
            HRM.enable('btnShowDetail', true);
            HRM.focus('txtDocDate');
            lstDelete = [];
            return GenerateCode();
        }
        function ReadById(id, btn) {                                        // ReadById(ID)
            return HRM.busy(btn || null, function () {
                return HRM.get(API + '/by-id', { id: id }).then(function (o) {
                    RecId = id;
                    buttons(true);
                    HRM.enable('btnShowDetail', false);
                    HRM.setVal('txtDocDate', HRM.day(o.PayrollDate));
                    HRM.setCombo('cmbMonth', HRM.int(o.PayrollMonth));
                    HRM.setCombo('cmbYear', HRM.int(o.PayrollYear));
                    HRM.setVal('txtDocNo', HRM.str(o.DocNo));
                    lstDelete = [];
                    bind(o.details || []);
                }).catch(HRM.fail);
            }, 'payroll-read');
        }
        function monthYearOk(mMsg, yMsg) {
            if (HRM.comboVal('cmbMonth') === 0) { HRM.box(mMsg); HRM.focus('cmbMonth'); return false; }
            if (HRM.comboVal('cmbYear') === 0) { HRM.box(yMsg); HRM.focus('cmbYear'); return false; }
            return true;
        }
        /** btnShowDetail_Click */
        P.btnShowDetail = function (btn) {
            if (!monthYearOk('Month feild is required...', 'Year feild is required...')) return;
            return HRM.busy(btn, function () {
                return HRM.get(API + '/show', {
                    departmentId: HRM.comboVal('cmbEmployeeDepartment'), categoryId: HRM.comboVal('cmbEmployeeCategory'),
                    month: HRM.comboText('cmbMonth'), year: HRM.comboVal('cmbYear'), monthValue: HRM.comboVal('cmbMonth'),
                    debitAccountId: HRM.comboVal('CmbDebitAccount'), employeeId: HRM.comboVal('cmbEmployee')
                }).then(function (rows) {
                    grd.clear();
                    bind(rows || []);                                      // rows == 0 -> ClearStructure
                }).catch(HRM.fail);
            });
        };
        function rowDto(r) {
            return {
                id: r.Id, employeeId: r.EmployeeId, employeeHistoryId: r.EmployeeHistoryId, grossSalary: r.GrossSalary,
                employeeDays: r.EmployeeDays, employeePerDaySalary: r.EmployeePerDaySalary, employeePayrollSalary: r.EmployeePayrollSalary,
                lateDeductionAmount: r.LateDeductionAmount, incomeTaxAmount: r.IncomeTaxAmount, leaveAmount: r.LeaveAmount,
                advanceAmount: r.AdvanceAmount, loanAmount: r.LoanAmount, loanNos: r.LoanNOS, pfAmount: r.PFAmount, eobiAmount: r.EOBIAmount,
                miscLessAmount: r.MiscLessAmount, totalLessAmount: r.TotalLessAmount, otAmount: r.OTAmount, travellingAmount: r.TravellingAmount,
                medicalAmount: r.MedicalAmount, mobileAmount: r.MobileAmount, fuelAmount: r.FuelAmount, foodAmount: r.FoodAmount,
                miscAddAmount: r.MiscAddAmount, arearAmount: r.ArearAmount, salary: r.Salary, addLessAmount: r.AddLessAmount, netSalary: r.NetSalary
            };
        }
        /** btnsave_Click (btnUpdate_Click calls it too) */
        P.btnsave = function (btn) {
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var body = {
                id: RecId, docDate: HRM.val('txtDocDate'), month: HRM.comboVal('cmbMonth'), year: HRM.comboVal('cmbYear'),
                docNo: HRM.val('txtDocNo'), details: grd.visibleRows().map(rowDto), deleted: lstDelete.map(rowDto)
            };
            return HRM.busy(btn || (RecId === 0 ? 'btnsave' : 'btnUpdate'), function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (RecId === 0 ? 'Record Save Successfully' : 'Record Update Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'payroll-save');
        };
        P.btnUpdate = function (btn) { return P.btnsave(btn || 'btnUpdate'); };
        P.btnnew = function () { Reset(); };                                              // btnnew_Click
        P.toolStripButton1 = function (btn) {                                            // toolStripButton1_Click (Refresh)
            return HRM.busy(btn, function () {
                return HRM.get(API + '/refresh').then(fillCombos).catch(HRM.fail);
            });
        };
        P.btnEmployeeCategory = function () { HRM.open('/hrm/employee-category'); };      // button1_Click -> frmEmployeeCategory.Show()
        P.btnEmployee = function () { HRM.open('/hrm/department'); };                     // btnEmployee_Click -> genDepartment.Show()

        /* ---------------- tsDropDownPrint */
        P.togglePrint = function () {
            reportsOpen = !reportsOpen;
            HRM.$('tsDropDownPrint').parentNode.classList.toggle('is-open', reportsOpen);
        };
        document.addEventListener('click', function (e) {
            if (reportsOpen && !e.target.closest('.pr-dd')) { reportsOpen = false; HRM.$('tsDropDownPrint').parentNode.classList.remove('is-open'); }
        });
        function fillReports(list) {
            var m = HRM.$('tsDropDownPrintMenu');
            m.innerHTML = (list || []).map(function (n) {
                return '<button type="button" class="pr-dd-item" data-rpt-own data-rpt="' + HRM.esc(n) + '">' + HRM.esc(n) + '</button>';
            }).join('') || '<div class="pr-dd-empty">No reports in the SalarySheet folder</div>';
        }
        HRM.$('tsDropDownPrintMenu').addEventListener('click', function (e) {             // tsDropDownPrint_DropDownItemClicked
            var it = e.target.closest('[data-rpt]'); if (!it) return;
            reportsOpen = false; HRM.$('tsDropDownPrint').parentNode.classList.remove('is-open');
            if (!monthYearOk('Month feild Required...', 'Year feild Required...')) return;
            var name = it.getAttribute('data-rpt');
            var args = { payrollId: RecId, month: HRM.comboText('cmbMonth').trim(), year: HRM.comboVal('cmbYear'), monthValue: HRM.comboVal('cmbMonth') };
            var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
            return HRM.busy('tsDropDownPrint', function () {
                return HRM.get(API + '/print-check', args).then(function () {
                    window.CrystalPrint.open('hrm-670-' + name.split('-')[0], args, null, win);
                }).catch(function (err) { if (window.CrystalPrint) window.CrystalPrint.release(win); HRM.fail(err); });
            });
        });

        /* ---------------- History tab (HistoryFill / DataGridHistory / grdHistoryDetail) as a dialog */
        function voucher(row, b) {                                                        // ColumnButtonClick "Voucher"
            var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
            return HRM.busy(b, function () {
                return HRM.get(API + '/voucher-head-id', { id: HRM.int(row.PayrollId) }).then(function (r) {
                    var vh = HRM.int(r && r.voucherHeadId);
                    if (vh === 0) { if (window.CrystalPrint) window.CrystalPrint.release(win); HRM.box('VoucherId Not Found'); return; }
                    window.CrystalPrint.open('hrm-670-102', { id: vh }, null, win);
                }).catch(function (err) { if (window.CrystalPrint) window.CrystalPrint.release(win); HRM.fail(err); });
            });
        }
        var DETAIL_DBL = ['GrossSalary', 'EmployeePayrollSalary', 'LateDeductionAmount', 'IncomeTaxAmount', 'LeaveAmount', 'AdvanceAmount',
            'LoanAmount', 'PFAmount', 'EOBIAmount', 'FoodAmount', 'MiscLessAmount', 'TotalLessAmount', 'OTAmount', 'TravellingAmount',
            'MedicalAmount', 'MobileAmount', 'FuelAmount', 'MiscAddAmount', 'ArrearAmount', 'Salary', 'AddLessAmount', 'NetSalary'];
        function historyDetailColumns() {                                                 // DataGridHistory_SelectionChanged dt
            var order = ['EmployeeNo', 'EmployeeName', 'GrossSalary', 'EmployeeDays', 'EmployeePerDaySalary', 'EmployeePayrollSalary',
                'LateDeductionAmount', 'IncomeTaxAmount', 'LeaveAmount', 'AdvanceAmount', 'LoanAmount', 'LoanNOS', 'PFAmount', 'EOBIAmount',
                'FoodAmount', 'MiscLessAmount', 'TotalLessAmount', 'OTAmount', 'TravellingAmount', 'MedicalAmount', 'MobileAmount', 'FuelAmount',
                'MiscAddAmount', 'ArrearAmount', 'Salary', 'AddLessAmount', 'NetSalary'];
            return order.map(function (k) {
                if (DETAIL_DBL.indexOf(k) >= 0) return { key: k, caption: k, type: 'num', sum: true, align: 'right', render: fmtUpTo2, totalFmt: fmtUpTo2 };
                if (k === 'EmployeePerDaySalary') return { key: k, caption: k, align: 'right', render: fmtUpTo2 };
                if (k === 'EmployeeNo') return { key: k, caption: k, type: 'int' };
                return { key: k, caption: k };
            });
        }
        function openHistory() {
            var detail = null, seq = 0;
            return HRM.history({
                title: 'HISTORY',
                columns: [
                    { key: 'Edit', caption: 'Edit', width: 50, render: function () { return '<button type="button" class="pr-mini-btn" data-act="edit">Edit</button>'; } },
                    { key: 'Voucher', caption: 'Voucher', width: 60, render: function () { return '<button type="button" class="pr-mini-btn" data-act="voucher">Voucher</button>'; } },
                    { key: 'PayrollId', caption: 'PayrollId', hidden: true },
                    { key: 'PayrollDocNo', caption: 'PayrollDocNo', type: 'code' },
                    { key: 'PayrollDate', caption: 'PayrollDate', type: 'date' },
                    { key: 'PayrollMonth', caption: 'PayrollMonth', type: 'int' },
                    { key: 'PayrollYear', caption: 'PayrollYear', type: 'int' },
                    { key: 'EntryUserName', caption: 'EntryUserName' }
                ],
                load: function () { return HRM.get(API + '/history'); },
                onPick: function (r) { RecId = HRM.int(r.PayrollId); ReadById(RecId); },   // DataGridHistory_DoubleClick
                onReady: function (body, g) {
                    var box = document.createElement('div');
                    box.className = 'hrm-grid-box hrm-grid-short pr-hist-detail';
                    box.innerHTML = '<div class="hrm-subcaption"><span>Detail</span><button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button></div>' +
                        '<div class="hrm-grid-wrap"><table id="grdHistoryDetail"></table></div>';
                    body.appendChild(box);
                    HRM.wireFullscreen(box);
                    detail = new HRM.Grid(box.querySelector('table'), { columns: historyDetailColumns(), filterRow: true, totals: true, onDraw: fixTotals });
                    g.opts.onSelect = function (r) {                                              // DataGridHistory_SelectionChanged
                        var my = ++seq;
                        HRM.get(API + '/by-id', { id: HRM.int(r.PayrollId) }).then(function (o) {
                            if (my !== seq) return;
                            detail.set((o.details || []).map(function (d) {
                                var x = Object.assign({}, d); x.ArrearAmount = d.ArearAmount; return x;
                            }));
                        }).catch(HRM.fail);
                    };
                    body.addEventListener('click', function (e) {                                  // DataGridHistory_ColumnButtonClick
                        var b = e.target.closest('button[data-act]'); if (!b) return;
                        var tr = b.closest('tr[data-i]'); if (!tr) return;
                        var row = g.rows()[+tr.getAttribute('data-i')];
                        if (b.getAttribute('data-act') === 'edit') {
                            var x = document.querySelector('.hrm-modal .hrm-modal-x'); if (x) x.click();
                            RecId = HRM.int(row.PayrollId); ReadById(RecId);
                        } else voucher(row, b);
                    });
                }
            });
        }

        HRM.keys({                                                                       // FrmExportSalesContract_KeyDown
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
            'ctrl+u': function () { if (HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled) P.btnUpdate(); },
            'ctrl+t': function () { openHistory(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        HRM.footer(function () { openHistory(); });                                       // History tab -> dialog
        bind([]);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                          // FrmExportSalesContract_Load
            rights = d.rights || {};
            decimals = HRM.int(d.amountDecimals);
            fillCombos(d);
            HRM.setVal('txtDocNo', HRM.str(d.docNo));
            if (d.debitAccounts && d.debitAccounts.length) HRM.fill('CmbDebitAccount', d.debitAccounts, 'Id', 'AccountTitle');
            if (d.employees && d.employees.length) HRM.fill('cmbEmployee', d.employees, 'EmployeeId', 'EmployeeName');
            fillReports(d.reports);
            HRM.setVal('txtDocDate', HRM.today());
            bind([]);
            buttons(false);
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnUpdate' });
            HRM.focus('txtDocDate');
            var id = HRM.int(HRM.param('id'));                                            // opened from a voucher drill-down (ReadById)
            if (id > 0) ReadById(id);
        }).catch(HRM.fail);
    }

    // ============================================================================ 671 Employee Allowance
    function employeeAllowance() {
        var RecId = 0, rights = {}, lstDelete = [], updateDetailIndex = -1, employees = [];
        var grd = new HRM.Grid('grdDetails', {
            columns: [                                                       // grdSettings(): Delete "X" at position 0, frozen
                { key: 'Delete', caption: 'X', width: 20, align: 'center', render: function () {
                    return '<button type="button" class="pr-x-btn" data-del="1" title="Delete">X</button>'; } },
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'Employee', caption: 'Employee', width: 200 },
                { key: 'EmployeeBenefitId', caption: 'EmployeeBenefitId', hidden: true },
                { key: 'EmployeeBenefit', caption: 'EmployeeBenefit', width: 160 },
                { key: 'AppliedOn', caption: 'AppliedOn', type: 'date' },
                { key: 'TotalAmount', caption: 'TotalAmount', align: 'right', sum: true, decimals: 2, render: fmt00, totalFmt: fmtHash0 },
                { key: 'IsForPayroll', caption: 'IsForPayroll', type: 'check' },
                { key: 'Remarks', caption: 'Remarks', width: 220 }
            ],
            filterRow: true, totals: true, onDraw: fixTotals,
            onDouble: function (r, i) { grdDetails_DoubleClick(r, i); }
        });
        HRM.$('grdDetails').addEventListener('click', function (e) {           // grdDetails_ColumnButtonClick "Delete"
            var b = e.target.closest('button[data-del]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            if (!HRM.ask('Are you sure to Delete?')) return;
            var i = +tr.getAttribute('data-i');
            lstDelete.push(Object.assign({}, grd.rows()[i]));
            grd.remove(i);
            if (updateDetailIndex === i) updateDetailIndex = -1; else if (updateDetailIndex > i) updateDetailIndex--;
        });
        function detailButtons(edit) {
            HRM.show('btnplus', !edit); HRM.show('btnUpdateDetail', edit); HRM.show('btnCancelUpdateDetial', edit);
        }
        function Reset() {                                                   // Reset()
            HRM.setVal('txtAppliedOn', HRM.today());
            HRM.setVal('txtTotalAmount', '');
            HRM.check('IsForPayroll', false);
            HRM.setVal('txtRemarks', '');
            HRM.focus('cmbEmployee');
        }
        function EmployeeFill() {
            return HRM.get(API + '/employees').then(function (rows) {
                if (rows && rows.length) { employees = rows; HRM.fill('cmbEmployee', rows, 'EmployeeId', 'EmployeeName', { keep: true }); }
            });
        }
        function EmployeeBenefitFill() {
            var id = HRM.comboVal('cmbEmployee');
            if (id === 0) return Promise.resolve();                            // ActiveRow == null: the list is left as it is
            return HRM.get(API + '/benefits', { employeeId: id }).then(function (rows) {
                if (rows && rows.length) HRM.fill('cmbEmployeeBenefit', rows, 'BenefitId', 'Description');
                else HRM.fill('cmbEmployeeBenefit', [], 'BenefitId', 'Description');   // Text = ""; DataSource = null
            });
        }
        function GetByEmployeeId(id) {
            return HRM.get(API + '/by-employee', { employeeId: id }).then(function (rows) {
                grd.set(rows || []);                                          // lst.Count == 0 -> ClearStructure
                updateDetailIndex = -1;
            });
        }
        /** cmbEmployee_ValueChanged / _TextChanged / _Leave: EmployeeBenefitFill + GetByEmployeeId. */
        function employeeChanged() {
            return HRM.loading(Promise.all([EmployeeBenefitFill(), GetByEmployeeId(HRM.comboVal('cmbEmployee'))])).catch(HRM.fail);
        }
        HRM.$('cmbEmployee').addEventListener('change', employeeChanged);
        function FormValidation() {
            if (HRM.comboVal('cmbEmployee') === 0) { HRM.box('Employee Required'); HRM.focus('cmbEmployee'); return false; }
            if (HRM.comboVal('cmbEmployeeBenefit') === 0) { HRM.box('Employee Benefit Required'); HRM.focus('cmbEmployeeBenefit'); return false; }
            if (HRM.val('txtTotalAmount') === '') { HRM.box('Total Amount Required'); HRM.focus('txtTotalAmount'); return false; }
            return true;
        }
        /** btnplus_Click -> AddtoGrid() */
        P.btnplus = function () {
            var rows = grd.visibleRows();
            for (var i = 0; i < rows.length; i++) {
                if (HRM.int(rows[i].EmployeeId) !== HRM.comboVal('cmbEmployee')) { HRM.box('You Cannot Insert Another Employee Record'); return; }
                if (HRM.int(rows[i].EmployeeBenefitId) === HRM.comboVal('cmbEmployeeBenefit')) { HRM.box('You Cannot Insert Duplicate Benefit Of an Employee'); return; }
            }
            if (!FormValidation()) return;
            grd.add({
                Id: 0, EmployeeId: HRM.comboVal('cmbEmployee'), Employee: HRM.comboText('cmbEmployee'),
                EmployeeBenefitId: HRM.comboVal('cmbEmployeeBenefit'), EmployeeBenefit: HRM.comboText('cmbEmployeeBenefit'),
                AppliedOn: HRM.val('txtAppliedOn'), TotalAmount: dbl(HRM.val('txtTotalAmount')), IsForPayroll: HRM.checked('IsForPayroll'),
                Remarks: HRM.val('txtRemarks')
            });
            Reset();
            HRM.focus('cmbEmployee');
        };
        /** grdDetails_DoubleClick */
        function grdDetails_DoubleClick(r, i) {
            updateDetailIndex = i;
            HRM.setCombo('cmbEmployee', r.EmployeeId);
            HRM.setCombo('cmbEmployeeBenefit', r.EmployeeBenefitId);
            HRM.setVal('txtAppliedOn', HRM.day(r.AppliedOn));
            HRM.setVal('txtTotalAmount', String(dbl(r.TotalAmount)));
            HRM.check('IsForPayroll', HRM.bool(r.IsForPayroll));
            HRM.setVal('txtRemarks', HRM.str(r.Remarks));
            detailButtons(true);
            if (HRM.int(r.Id) > 0) HRM.enable('cmbEmployee', false);           // cmbEmployee.ReadOnly = true
        }
        /** btnUpdateDetail_Click */
        P.btnUpdateDetail = function () {
            if (!FormValidation()) return;
            var r = grd.rows()[updateDetailIndex]; if (!r) return;
            r.EmployeeId = HRM.comboVal('cmbEmployee');
            r.Employee = HRM.comboText('cmbEmployee');
            r.EmployeeBenefitId = HRM.comboVal('cmbEmployeeBenefit');
            r.EmployeeBenefit = HRM.comboText('cmbEmployeeBenefit');
            r.AppliedOn = HRM.val('txtAppliedOn');
            r.TotalAmount = dbl(HRM.val('txtTotalAmount'));
            r.IsForPayroll = HRM.checked('IsForPayroll');
            r.Remarks = HRM.val('txtRemarks');
            grd.draw();
            detailButtons(false);
            Reset();
            HRM.focus('cmbEmployee');
        };
        /** btnCancelUpdateDetial_Click */
        P.btnCancelUpdateDetial = function () { detailButtons(false); Reset(); };
        function rowDto(r) {
            return { id: r.Id, employeeId: r.EmployeeId, employeeBenefitId: r.EmployeeBenefitId, appliedOn: HRM.day(r.AppliedOn),
                totalAmount: r.TotalAmount, isForPayroll: HRM.bool(r.IsForPayroll), remarks: HRM.str(r.Remarks) };
        }
        /** btnsave_Click (btnUpdate_Click calls it) */
        P.btnsave = function (btn) {
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var body = { rows: grd.visibleRows().map(rowDto), deleted: lstDelete.map(rowDto) };
            return HRM.busy(btn || 'btnsave', function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || 'Receod Save Successfully');
                    RecId = 0; lstDelete = [];
                    HRM.show('btnsave', true); HRM.show('btnUpdate', false);
                    grd.clear(); updateDetailIndex = -1;
                    Reset();
                }).catch(HRM.fail);
            }, 'allowance-save');
        };
        P.btnUpdate = function (btn) { return P.btnsave(btn || 'btnUpdate'); };
        /** btnnew_Click */
        P.btnnew = function () {
            RecId = 0; lstDelete = [];
            HRM.show('btnsave', true); HRM.show('btnUpdate', false);
            HRM.enable('cmbEmployee', true);
            grd.clear(); updateDetailIndex = -1;
            detailButtons(false);
            Reset();
        };
        /** btnRefresh_Click_1: EmployeeFill + EmployeeBenefitFill */
        P.btnRefresh = function (btn) {
            return HRM.busy(btn, function () { return EmployeeFill().then(EmployeeBenefitFill).catch(HRM.fail); });
        };
        P.btnEmployee = function () { HRM.open('/hrm/employee-registration'); };     // frmEmployeeRegistration.Show() (View right checked by that page)
        P.btnBenifits = function () { HRM.open('/hrm/benefit'); };                    // BenifitDefine.Show()
        HRM.keys({                                                                    // FrmExportSalesContract_KeyDown
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
            'ctrl+u': function () { if (HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled) P.btnUpdate(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        /* History = the form's own grid (the employee's allowances): reload it and scroll to it. */
        HRM.footer(function (b) {
            return HRM.busy(b, function () { return GetByEmployeeId(HRM.comboVal('cmbEmployee')).then(function () { scrollTo('grdBox'); }).catch(HRM.fail); });
        });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // FrmExportSalesContract_Load
            rights = d.rights || {};
            employees = d.employees || [];
            if (employees.length) HRM.fill('cmbEmployee', employees, 'EmployeeId', 'EmployeeName');
            HRM.fill('cmbEmployeeBenefit', [], 'BenefitId', 'Description');
            HRM.setVal('txtAppliedOn', HRM.today());
            HRM.show('btnsave', true); HRM.show('btnUpdate', false);
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnUpdate' });
            HRM.focus('cmbEmployee');
        }).catch(HRM.fail);
    }

    // ============================================================================ 672 / 674 Late & Short adjustment
    /**
     * frmEmployeeLateAdjustment and EmployeeShortAdjustment are the same form with different fields:
     * EmployeeName / cmbEmployeeName_Leave / Validation / Reset / GridBind / RetrivedData / Insert.
     */
    function adjustment(cfg) {
        var RecId = 0, UpdateMode = false, rights = {}, employees = [];
        var grd = new HRM.Grid('grd', {
            columns: cfg.columns, filterRow: true, totals: true, onDraw: fixTotals,
            onDouble: function (r) { open(r); }                                      // grdEmployeeFamilyInfo_DoubleClick
        });
        function open(r) { RetrivedData(HRM.int(r[cfg.idKey])); UpdateMode = true; }
        HRM.$('panel5').addEventListener('dblclick', function () {                   // panel5.DoubleClick -> the same handler (current row)
            var r = grd.current(); if (r) open(r);
        });
        function GridBind() {
            return HRM.get(API + '/list').then(function (rows) { grd.set(rows || []); }).catch(HRM.fail);
        }
        function EmployeeName() {
            return HRM.get(API + '/employees').then(function (rows) {
                employees = rows || [];
                HRM.fill('cmbEmployeeName', employees, 'EmployeeId', 'EmployeeName', { keep: true });
            });
        }
        /** cmbEmployeeName_Leave: the employee's row of dtemployee into the Employee Information box. */
        function employeeInfo() {
            var id = HRM.comboVal('cmbEmployeeName'), row = null;
            if (id !== 0) for (var i = 0; i < employees.length; i++) if (HRM.int(employees[i].EmployeeId) === id) row = employees[i];
            HRM.setVal('txtEmployeeNo', row ? HRM.str(row.EmployeeNo) : '');
            HRM.setVal('txtDesgination', row ? HRM.str(row.DesignationName) : '');
            HRM.setVal('txtDepartment', row ? HRM.str(row.DepartmentName) : '');
            HRM.setVal('txtSalary', row ? HRM.str(row.TotalSalary) : '');
            HRM.setVal('txtSection', row ? HRM.str(row.SectionName) : '');
        }
        HRM.$('cmbEmployeeName').addEventListener('change', employeeInfo);
        function Validation() {
            if (HRM.comboVal('cmbEmployeeName') === 0) { HRM.box('Employee Name Required!'); HRM.focus('cmbEmployeeName'); return false; }
            for (var i = 0; i < cfg.required.length; i++) {
                var f = cfg.required[i];
                if (dbl(HRM.val(f[0])) === 0) { HRM.box(f[1]); HRM.focus(f[0]); return false; }
            }
            return true;
        }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function Reset() {                                                            // Reset()
            RecId = 0;
            HRM.setCombo('cmbEmployeeName', 0);
            HRM.setCombo('cmbMonth', 0); HRM.setCombo('cmbYear', 0);
            cfg.texts.forEach(function (t) { HRM.setVal(t, ''); });
            HRM.check('IsDeduction', false); HRM.check('IsLeaveAdjust', false);
            ['txtSalary', 'txtEmployeeNo', 'txtDepartment', 'txtDesgination', 'txtSection'].forEach(function (t) { HRM.setVal(t, ''); });
            HRM.focus('cmbEmployeeName');
            buttons(false);
            UpdateMode = false;
            return GridBind();
        }
        function RetrivedData(id) {                                                   // RetrivedData(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = id;
                HRM.setCombo('cmbEmployeeName', HRM.int(o.EmployeeId));
                HRM.setCombo('cmbMonth', HRM.int(o[cfg.monthKey]));
                HRM.setCombo('cmbYear', HRM.int(o[cfg.yearKey]));
                cfg.load(o);
                HRM.check('IsDeduction', HRM.bool(o.IsDeduction));
                HRM.check('IsLeaveAdjust', HRM.bool(o.IsLeaveAdjust));
                employeeInfo();
                buttons(true);
            }).catch(HRM.fail);
        }
        function Insert(btn) {                                                        // Insert()
            if (!Validation()) return;
            var body = Object.assign({ id: RecId, employeeId: HRM.comboVal('cmbEmployeeName'), month: HRM.comboVal('cmbMonth'),
                year: HRM.comboVal('cmbYear'), deductionRate: HRM.val('txtDeductionRate'),
                isDeduction: HRM.checked('IsDeduction'), isLeaveAdjust: HRM.checked('IsLeaveAdjust') }, cfg.body());
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Saved Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, PAGE + '-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };          // btnupdate_Click
        P.btnnew = function () { Reset(); };                                          // btnnew_Click
        P.btnRefresh = function (btn) {                                               // btnRefresh_Click: EmployeeName()
            return HRM.busy(btn, function () { return EmployeeName().catch(HRM.fail); });
        };
        P.btnEmployee = function () { HRM.open('/hrm/employee-registration'); };      // btnEmployee_Click
        HRM.keys({                                                                    // EmployeeFamilyInfo_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled && !UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled && UpdateMode) P.btnupdate(); }
        });
        /* History = the form's own History grid: GridBind again and scroll to it. */
        HRM.footer(function (b) { return HRM.busy(b, function () { return GridBind().then(function () { scrollTo('grdBox'); }); }); });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // EmployeeFamilyInfo_Load
            rights = d.rights || {};
            HRM.fill('cmbMonth', MONTHS, 'Id', 'Name');
            HRM.fill('cmbYear', cfg.years(d), 'Id', 'Year');
            employees = d.employees || [];
            HRM.fill('cmbEmployeeName', employees, 'EmployeeId', 'EmployeeName');
            grd.set(d.rows || []);
            buttons(false);
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
            HRM.focus('cmbEmployeeName');
        }).catch(HRM.fail);
    }

    function flagCols(list) {
        return list.map(function (c) {
            if (c.fmt === 'hash') return { key: c.key, caption: c.key, align: 'right', sum: c.sum, decimals: 0, render: fmtHash, totalFmt: fmtHash, width: c.width };
            return Object.assign({ caption: c.key }, c);
        });
    }

    function lateAdjustment() {
        adjustment({
            idKey: 'EmployeeLateAdjustmentId', monthKey: 'LAMonth', yearKey: 'LAYear',
            columns: flagCols([                                                        // GridBind dt + grdSetting()
                { key: 'EmployeeLateAdjustmentId', hidden: true },
                { key: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', width: 250, type: 'code' },
                { key: 'LocationId', hidden: true },
                { key: 'LAMonth' }, { key: 'LAYear' },
                { key: 'TotalLates', fmt: 'hash', sum: true },
                { key: 'TotalLateDays', fmt: 'hash', sum: true },
                { key: 'DeductedDays', type: 'num', decimals: 2, noGroup: false, render: function (v) { return HRM.str(v === null ? '' : HRM.num(v)); } },
                { key: 'DeductionRate', fmt: 'hash' },
                { key: 'IsDeduction', type: 'check' }, { key: 'IsLeaveAdjust', type: 'check' }
            ]),
            texts: ['txtTotalLateDays', 'txtDeductionRate', 'txtDeductedDays', 'txtTotalLates'],
            required: [['txtTotalLates', 'Total Lates Field Required!'], ['txtTotalLateDays', 'Total Late Days Field Required!'],
                ['txtDeductedDays', 'Total Deducted Days Field Required!'], ['txtDeductionRate', 'Total Deduction Rate Field Required!']],
            years: function (d) { return d.years || []; },                             // CommonServices.GetYears
            load: function (o) {
                HRM.setVal('txtTotalLates', HRM.str(o.TotalLates));
                HRM.setVal('txtTotalLateDays', HRM.str(o.TotalLateDays));
                HRM.setVal('txtDeductedDays', HRM.str(o.DeductedDays));
                HRM.setVal('txtDeductionRate', HRM.str(o.DeductionRate));
            },
            body: function () {
                return { totalLates: HRM.val('txtTotalLates'), totalLateDays: HRM.val('txtTotalLateDays'), deductedDays: HRM.val('txtDeductedDays') };
            }
        });
    }

    /** YearFill of 673 / 674: 1900 .. DateTime.Now.Year, sorted "Id DESC". */
    function yearsDesc() {
        var out = [];
        for (var y = new Date().getFullYear(); y >= 1900; y--) out.push({ Id: y, Year: y });
        return out;
    }

    function shortAdjustment() {
        adjustment({
            idKey: 'EmployeeShortAdjustmentId', monthKey: 'SAMonth', yearKey: 'SAYear',
            columns: flagCols([
                { key: 'EmployeeShortAdjustmentId', hidden: true },
                { key: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', width: 250, type: 'code' },
                { key: 'LocationId', hidden: true },
                { key: 'SAMonth' }, { key: 'SAYear' },
                { key: 'TotalShortHours', fmt: 'hash', sum: true },
                { key: 'TotalShortMinutes', align: 'right', render: function (v) { return HRM.str(v === null ? '' : HRM.num(v)); } },
                { key: 'DeductedShortHours', fmt: 'hash', sum: true },
                { key: 'DeductionRate', fmt: 'hash' },
                { key: 'IsDeduction', type: 'check' }, { key: 'IsLeaveAdjust', type: 'check' }
            ]),
            texts: ['txtTotalHours', 'txtTotalMinutes', 'txtDeductionRate', 'txtDeductedShortHours'],
            required: [['txtTotalHours', 'Total Hours Field Required!'], ['txtDeductedShortHours', 'Total Deducted Hours Field Required!'],
                ['txtDeductionRate', 'Total Deduction Rate Field Required!']],
            years: function () { return yearsDesc(); },
            load: function (o) {
                HRM.setVal('txtTotalHours', HRM.str(o.TotalShortHours));
                HRM.setVal('txtTotalMinutes', HRM.str(o.TotalShortMinutes));
                HRM.setVal('txtDeductedShortHours', HRM.str(o.DeductedShortHours));
                HRM.setVal('txtDeductionRate', HRM.str(o.DeductionRate));
            },
            body: function () {
                return { totalHours: HRM.val('txtTotalHours'), totalMinutes: HRM.val('txtTotalMinutes'), deductedShortHours: HRM.val('txtDeductedShortHours') };
            }
        });
    }

    // ============================================================================ 673 Employee Loan Deduction
    function employeeLoanDeduction() {
        var rights = {}, UpdateMode = false;
        var grd = new HRM.Grid('grdPendingForDeduction', {
            columns: [                                                                // grdPendingForDeductionSetting()
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 250 },
                { key: 'RemaningNoOfInstallment', caption: 'RemaningNoOfInstallment', width: 200 },
                { key: 'InstallmentAmount', caption: 'InstallmentAmount', width: 150, align: 'right', sum: true, decimals: 2, render: fmt00, totalFmt: fmtHash0 },
                { key: 'NoOfInstallment', caption: 'NoOfInstallment', width: 150, type: 'edit', redraw: true },
                { key: 'Amount', caption: 'Amount', width: 150, align: 'right', sum: true, decimals: 2, render: fmt00, totalFmt: fmtHash0 }
            ],
            filterRow: true, totals: true, onDraw: fixTotals,
            onChange: function (r, k) {                                               // grdPendingForDeduction_CellUpdated
                if (k !== 'NoOfInstallment') return;
                if (dbl(r.NoOfInstallment) > dbl(r.RemaningNoOfInstallment)) {
                    r.NoOfInstallment = 0; r.Amount = 0;
                    setTimeout(function () { grd.draw(); HRM.box('NoOfInstallment cannot be greater than RemaningNoOfInstallment Please Check'); }, 0);
                    return;
                }
                r.Amount = dbl(r.NoOfInstallment) * dbl(r.InstallmentAmount);
            }
        });
        function CalculateAmount() {                                                  // CalculateAmount()
            grd.rows().forEach(function (r) {
                if (HRM.int(r.NoOfInstallment) <= HRM.int(r.RemaningNoOfInstallment)) {
                    if (HRM.int(r.NoOfInstallment) > 0 && dbl(r.InstallmentAmount) > 0) r.Amount = dbl(r.NoOfInstallment) * dbl(r.InstallmentAmount);
                } else r.Amount = 0;
            });
            grd.draw();
        }
        function monthYear(mMsg, yMsg) {
            if (HRM.comboVal('cmbMonth') === 0) { HRM.box(mMsg); HRM.focus('cmbMonth'); return false; }
            if (HRM.comboVal('cmbYear') === 0) { HRM.box(yMsg); HRM.focus('cmbYear'); return false; }
            return true;
        }
        /** btnLoad_Click -> PendingForDeductionFill() */
        P.btnLoad = function (btn) {
            if (!monthYear('Month Feild Required...', 'Year Feild Required...')) return;
            return HRM.busy(btn, function () {
                return HRM.get(API + '/pending').then(function (rows) {
                    if (rows && rows.length) { grd.set(rows); CalculateAmount(); } else grd.clear();
                }).catch(HRM.fail);
            });
        };
        function Reset() {                                                            // Reset()
            HRM.setCombo('cmbMonth', 0); HRM.setCombo('cmbYear', 0);
            HRM.focus('cmbMonth');
            HRM.show('btnsave', true);
            grd.clear();
        }
        /** btnsave_Click / btnupdate_Click -> Insert() */
        P.btnsave = function (btn) {
            if (!monthYear('Month Required', 'Year Required')) return;
            var body = { month: HRM.comboVal('cmbMonth'), year: HRM.comboVal('cmbYear'),
                rows: grd.visibleRows().map(function (r) {
                    return { id: r.Id, employeeId: r.EmployeeId, installmentAmount: r.InstallmentAmount, noOfInstallment: r.NoOfInstallment, amount: r.Amount };
                }) };
            return HRM.busy(btn || 'btnsave', function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || 'Saved Successfully');
                    Reset();
                }).catch(HRM.fail);
            }, 'loan-deduction-save');
        };
        P.btnUpdate = function (btn) { return P.btnsave(btn || 'btnUpdate'); };
        P.btnnew = function () { Reset(); };                                          // btnnew_Click
        HRM.keys({                                                                    // EmployeeFamilyInfo_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled && !UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        /* History tab (grd): the desktop never fills it (tabControl1_SelectedIndexChanged is empty); the
           dialog shows the BLL's own FormHistory (ReadAll), read-only as the desktop grid is. */
        HRM.footer(function () {
            HRM.history({
                title: 'History',
                columns: [
                    { key: 'EmployeeLoanDeductionId', caption: 'EmployeeLoanDeductionId', hidden: true },
                    { key: 'EmployeeName', caption: 'EmployeeName', width: 220 },
                    { key: 'PayrollMonth', caption: 'PayrollMonth', type: 'int' },
                    { key: 'PayrollYear', caption: 'PayrollYear', type: 'int' },
                    { key: 'NoOfInstallment', caption: 'NoOfInstallment', type: 'int', sum: true, decimals: 0 },
                    { key: 'InstallmentAmount', caption: 'InstallmentAmount', type: 'num', sum: true },
                    { key: 'TotalLoanAmount', caption: 'TotalLoanAmount', type: 'num', sum: true },
                    { key: 'LocationName', caption: 'LocationName' },
                    { key: 'CreatedByName', caption: 'CreatedByName' },
                    { key: 'CreatedOn', caption: 'CreatedOn', type: 'datetime' },
                    { key: 'AlteredByName', caption: 'AlteredByName' },
                    { key: 'AlteredOn', caption: 'AlteredOn', type: 'datetime' }
                ],
                totals: true,
                load: function () { return HRM.get(API + '/history'); }
            });
        });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // EmployeeFamilyInfo_Load
            rights = d.rights || {};
            HRM.fill('cmbYear', yearsDesc(), 'Id', 'Year');
            HRM.fill('cmbMonth', MONTHS, 'Id', 'Name');
            HRM.show('btnsave', true);
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnUpdate' });
        }).catch(HRM.fail);
    }

    var PAGES = {
        'payroll-posting': payrollPosting,
        'employee-allowance': employeeAllowance,
        'employee-late-adjustment': lateAdjustment,
        'employee-loan-deduction': employeeLoanDeduction,
        'employee-short-adjustment': shortAdjustment
    };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
