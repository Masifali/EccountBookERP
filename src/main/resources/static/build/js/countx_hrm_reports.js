/* ============================================================================================
 * countx_hrm_reports.js - HRM reports (Architecture.WinApp.HRM_Reports, AppModules 29). One script,
 * the page is chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM) and
 * countx_crystal_print.js (window.CrystalPrint).
 *
 *   employee-register                 EmployeeHistoryRpt.cs            (371)
 *   employee-monthly-register         HRM_Reports.cs                   (372, two tabs)
 *   daily-attendance                  DailyAttendanceRpt.cs            (373)
 *   daily-late-and-early-departure    DailyLateandEarlyDeparture.cs    (374)
 *   duty-roster-employee-wise         genDutyRosterEmployeeWise.cs     (375)
 *   employee-attendence               genEmployeeAttendence.cs         (376)
 *   monthly-attendance-summary        MonthlyAttendanceSummary.cs      (377)
 *   salary-sheet                      PayRollSalarySheetRpt.cs         (378)
 *
 * Each form: the filter combos (same procedures, empty slot = BindDDL ZeroIndex:false), Show = the
 * form's GridHistory (the grid shows the form's dtcol with GridSetting's hidden columns, widths,
 * formats, totals and groups), Refresh = btnNew_Click, the toolbar prints (registered Crystal
 * contracts; a grid-backed print re-runs the last Show's arguments, as the desktop prints dtGrid of
 * the last Show), the grid bar's Export, Form_KeyDown (Ctrl+N, Ctrl+P, Ctrl+E / Esc, Enter = Tab).
 * The employee code of every grid opens /hrm/employee-registration?id=EmployeeId.
 * Reports have no History of their own: the footer's History button is hidden (HRM.footer(null)).
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/reports/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmReports
    window.HrmReports = P;

    var NO_REC = 'Not Record Found For Display';

    // ============================================================================ shared helpers

    /** Constants.InventoryConstants (Architecture.WinApp.Common.Constants). */
    var K = { DocNo: 60, Date: 73, SupplierName: 170, StringC: 130, WareHouseName: 150, DriverCnic: 90, CellNo: 100,
        Amount: 90, Status: 75, BranchName: 120, Days: 60, DiscountType: 80, AccountCode: 100 };

    /** BindDDL(..., ZeroIndex: false): the list with an empty "nothing picked" slot (Value 0). */
    function fill(id, rows, textKey, valueKey) {
        HRM.fill(id, rows || [], valueKey || 'Id', textKey || 'Name', { zero: '', keep: true });
    }
    /** UltraCombo.DataSource = null; Text = string.Empty. */
    function clearCombo(id) { HRM.fill(id, [], 'Id', 'Name', { zero: '' }); }
    function v(id) { return HRM.comboVal(id); }
    /** A "yyyy-MM-dd" date for a print argument (ISO with T: unambiguous for datetime parameters). */
    function dateArg(iso) { return iso ? iso + 'T00:00:00' : null; }
    function unavailable(file) { HRM.box('Report file ' + file + ' is not available.'); }
    function openEmployee(r) {
        var id = HRM.int(HRM.col(r, 'EmployeeId'));
        if (id > 0) HRM.open('/hrm/employee-registration?id=' + id);
    }
    function status(n) { HRM.text('lblStatus', n ? n + ' record(s)' : ''); }
    function monthOptions(id, full) {
        HRM.fillFixed(id, HRM.MONTHS.map(function (m, i) { return [i + 1, full ? m : m.substr(0, 3)]; }));
        HRM.setVal(id, String(new Date().getMonth() + 1));
    }

    /** .NET custom numeric formats the GridSetting code uses. */
    function fmt(val, f) {
        if (val === null || val === undefined || val === '') return '';
        var n = HRM.num(val);
        if (f === '0,0') {                                   // at least two integer digits, grouped
            var s = HRM.fmtNum(Math.round(n), 0), neg = s.charAt(0) === '-';
            var d = neg ? s.substr(1) : s;
            if (d.replace(/,/g, '').length < 2) d = '0' + d;
            return (neg ? '-' : '') + d;
        }
        if (f === '#,##0.##') {
            var t = HRM.fmtNum(n, 2);
            return t.replace(/\.?0+$/, '');
        }
        return HRM.str(val);
    }
    /** A number column: cell FormatString, total TotalFormatString (AggregateFunction.Sum). */
    function numCol(key, width, cellFmt, totalFmt, extra) {
        return Object.assign({
            key: key, caption: key, width: width, align: 'right', sum: !!totalFmt, totalFmt: totalFmt,
            render: function (x) { return HRM.esc(fmt(x, cellFmt)); }
        }, extra || {});
    }

    /**
     * The report GridEX: filter row, total row, the employee code as a link, GridEX groups
     * (RootTable.Groups.Add) drawn as group header rows, and the totals re-formatted with the
     * column's TotalFormatString.
     */
    function ReportGrid(id, columns, o) {
        o = o || {};
        var self = this;
        this.groups = [];
        this.grid = new HRM.Grid(id, {
            columns: columns, filterRow: true, totals: true, emptyText: '',
            onCode: function (r, i, c) { if (o.onCode) o.onCode(r, c); else openEmployee(r); },
            onDraw: function (g) { self._decorate(g); }
        });
    }
    ReportGrid.prototype.set = function (rows, groups) {
        this.groups = groups || [];
        var keys = this.groups;
        rows = (rows || []).slice();
        if (keys.length) {                                   // GridEX sorts the group values ascending
            rows = rows.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
                for (var k = 0; k < keys.length; k++) {
                    var x = HRM.str(HRM.col(a.r, keys[k])), y = HRM.str(HRM.col(b.r, keys[k]));
                    if (x !== y) return x < y ? -1 : 1;
                }
                return a.i - b.i;
            }).map(function (x) { return x.r; });
        }
        this.grid.set(rows);
        status(rows.length);
    };
    ReportGrid.prototype.rows = function () { return this.grid.rows(); };
    ReportGrid.prototype.clear = function () { this.set([]); };
    ReportGrid.prototype.column = function (k) { return this.grid.columnOf(k); };
    /** RootTable.Columns[k].Visible = !on after the grid was built (header, filter cell and body cells). */
    ReportGrid.prototype.hide = function (k, on) {
        var g = this.grid, c = g.columnOf(k); if (!c) return;
        c.hidden = !!on;
        var i = g.columns.indexOf(c);
        Array.prototype.forEach.call(g.table.tHead.rows, function (tr) { if (tr.cells[i]) tr.cells[i].style.display = on ? 'none' : ''; });
    };
    ReportGrid.prototype._decorate = function (g) {
        var keys = this.groups;
        if (g.table.tFoot) {                                 // TotalFormatString
            var tds = g.table.tFoot.querySelectorAll('td');
            g.columns.forEach(function (c, i) {
                if (c.sum && c.totalFmt && tds[i]) tds[i].textContent = fmt(g.sum(c.key, true), c.totalFmt);
            });
        }
        if (!keys.length || !g.view.length) return;
        var span = g.columns.filter(function (c) { return !c.hidden; }).length || 1;
        var prev = [];
        Array.prototype.forEach.call(g.body.querySelectorAll('tr[data-i]'), function (tr) {
            var r = g.data[+tr.getAttribute('data-i')];
            for (var lvl = 0; lvl < keys.length; lvl++) {
                var val = HRM.str(HRM.col(r, keys[lvl]));
                if (prev[lvl] === val) continue;
                for (var j = lvl; j < keys.length; j++) {
                    var gv = HRM.str(HRM.col(r, keys[j]));
                    prev[j] = gv;
                    var h = document.createElement('tr');
                    h.className = 'hrm-group-row hrm-group-l' + j;
                    h.innerHTML = '<td colspan="' + span + '">' + HRM.esc(keys[j]) + ': ' + HRM.esc(gv) + '</td>';
                    tr.parentNode.insertBefore(h, tr);
                }
                break;
            }
        });
    };

    /** CtrlGrdBar "Export": the grid as shown (visible columns, filtered rows, groups, totals) to an Excel file. */
    function exportGrid(tableId, fileName) {
        var t = HRM.$(tableId);
        if (!t || !t.tBodies[0] || !t.tBodies[0].querySelector('tr[data-i]')) { HRM.box(NO_REC); return; }
        var html = '';
        Array.prototype.forEach.call(t.rows, function (tr) {
            if (tr.classList.contains('hrm-filter-row') || tr.classList.contains('hrm-empty')) return;
            var cells = '';
            Array.prototype.forEach.call(tr.cells, function (c) {
                if (c.style.display === 'none') return;
                var box = c.querySelector('input[type=checkbox]');
                var txt = box ? (box.checked ? 'True' : 'False') : c.textContent;
                var tag = c.tagName === 'TH' ? 'th' : 'td';
                cells += '<' + tag + (c.colSpan > 1 ? ' colspan="' + c.colSpan + '"' : '') + '>' + HRM.esc(txt) + '</' + tag + '>';
            });
            html += '<tr>' + cells + '</tr>';
        });
        var doc = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel">' +
            '<head><meta charset="utf-8"></head><body><table border="1">' + html + '</table></body></html>';
        var blob = new Blob(['﻿' + doc], { type: 'application/vnd.ms-excel' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = (fileName || 'Export') + '.xls';
        document.body.appendChild(a); a.click();
        setTimeout(function () { URL.revokeObjectURL(a.href); a.remove(); }, 500);
    }
    P.exportGrid = function (tableId, fileName) { exportGrid(tableId, fileName); };

    /** ShowReportWithDataTable(dtGrid, rpt): rows of the last Show, else the form's message. */
    function printGrid(btn, key, rows, args, msg) {
        if (!rows || !rows.length) { HRM.box(msg || NO_REC); return; }
        if (!window.CrystalPrint) { HRM.box('The report viewer is not loaded.'); return; }
        return CrystalPrint.open(key, args, btn);
    }
    /**
     * A print that runs its own BLL call first (1002, 1005, 1110B): the viewer tab is reserved inside
     * the click, the row count decides "Not Record Found For Display", then the contract prints.
     */
    function printChecked(btn, url, body, key, args, missingRpt) {
        var win = missingRpt ? null : (window.CrystalPrint ? CrystalPrint.reserve() : null);
        return HRM.busy(btn, function () {
            return HRM.post(url, body).then(function (d) {
                if (!d || !HRM.int(d.rows)) { if (win) CrystalPrint.release(win); HRM.box(NO_REC); return; }
                if (missingRpt) { unavailable(missingRpt); return; }
                return CrystalPrint.open(key, args, null, win);
            }).catch(function (e) { if (win) CrystalPrint.release(win); HRM.fail(e); });
        });
    }

    function keys(map) {
        HRM.keys(Object.assign({ 'ctrl+e': HRM.close, 'esc': HRM.close }, map));
    }

    // ============================================================================ 371 Employee Register
    function employeeRegister() {
        var dtGrid = [], lastArgs = {};
        var grd = new ReportGrid('grd', [                                       // GridSetting
            { key: 'EmployeeId', hidden: true },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo, type: 'code' },
            { key: 'EmployeeName', caption: 'EmployeeName', width: K.SupplierName },
            { key: 'DOB', caption: 'DOB', width: K.Date + 20, type: 'date' },
            { key: 'LocationName', caption: 'LocationName', width: K.StringC + 30 },
            { key: 'DepartmentName', caption: 'DepartmentName', width: K.WareHouseName + 20 },
            { key: 'DesignationName', caption: 'DesignationName', width: K.StringC },
            { key: 'SectionName', caption: 'SectionName', width: K.StringC },
            { key: 'JoiningDate', caption: 'JoiningDate', width: K.Date + 20, type: 'date' },
            { key: 'CNIC', caption: 'CNIC', width: K.DriverCnic + 30 },
            { key: 'MobileNo', caption: 'MobileNo', width: K.CellNo },
            numCol('TotalSalary', K.Amount, '0,0', '0,0'),
            { key: 'CurrentGlBalance', caption: 'CurrentGlBalance', align: 'right' },
            { key: 'ProbationEnding', caption: 'ProbationEnding', width: K.Date + 20, type: 'date' },
            { key: 'Active', caption: 'Active', width: K.Status }
        ]);
        function load() {                                                   // DailyAttendanceRpt_Load
            return HRM.get(API + '/setup').then(function (d) {
                fill('cmbDepartment', d.departments);                           // DepartmentFill
                fill('cmbSection', d.sections);                                 // SectionFill
                fill('cmbShift', d.shifts);                                     // ShiftFill
                fill('cmbDesignation', d.designations);                         // DesignationFill
                fill('cmbEmployee', d.employees);                               // EmployeeFill (genEmployee.Getall)
            });
        }
        function IsMissingFill() {                                          // 1 Active / 2 InActive, Rows[0].Activate()
            HRM.fillFixed('cmbIsActive', [[1, 'Active'], [2, 'InActive']], { zero: '' });
            HRM.setVal('cmbIsActive', '1');
        }
        function filters() {
            return { departmentId: v('cmbDepartment'), sectionId: v('cmbSection'), shiftId: v('cmbShift'),
                designationId: v('cmbDesignation'), employeeId: v('cmbEmployee'), status: v('cmbIsActive') };
        }
        function GridHistory(btn) {                                         // GridHistory
            var f = filters();
            return HRM.busy(btn || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    lastArgs = { departmentId: f.departmentId, sectionId: f.sectionId, shiftId: f.shiftId, designationId: f.designationId,
                        employeeId: f.employeeId };
                    if (f.status) lastArgs.active = f.status === 1;             // ApprovedFilter != "All"
                    grd.set(dtGrid);
                }).catch(HRM.fail);
            }, 'show');
        }
        P.btnshow = function (b) { return GridHistory(b); };                 // btnshow_Click
        P.btnNew = function (b) {                                            // btnNew_Click
            ['cmbEmployee', 'cmbDepartment', 'cmbSection', 'cmbShift', 'cmbDesignation'].forEach(function (id) { HRM.setCombo(id, 0); });
            HRM.setVal('cmbIsActive', '1');
            return GridHistory(b || 'btnNew');
        };
        P.print = function (b) {                                             // print_Click (1101)
            return printGrid(b || 'print', 'hrm-1101', dtGrid, lastArgs);
        };
        P.btnPrint1101_01 = function () {                                    // btnPrint1101_01_Click
            if (!dtGrid.length) { HRM.box(NO_REC); return; }
            unavailable('1101_01-EmployeeListDepartmentWiseActiveInactive.rpt');
        };
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.print(); } });
        IsMissingFill();
        HRM.loading(load()).catch(HRM.fail);
    }

    // ============================================================================ 372 Employee Monthly Register
    function monthlyRegister() {
        var dtGrid = [], dtGridRegister = [], lastArgs = {}, lastRegArgs = {}, tab = 0;
        var sumCols = ['ShortHours', 'ShortMinutes', 'OTHours', 'OTMinutes', 'EmployeeHours', 'EmployeeMinutes', 'TotalShortHour',
            'TotalShortMinutes', 'TotalOTMinutes', 'TOTAL_HOUR', 'Actual_HOUR', 'EMPLOYEE_WORKINGHOUR', 'OverTime'];
        var hidden = ['EmployeeAttendanceId', 'DutyRoasterDetailId', 'EmployeeId', 'Sr#', 'RM', 'RY', 'ATD_DATE', 'TotalShortHour',
            'TotalShortMinutes', 'TotalOTMinutes', 'EMPLOYEE_WORKINGHOUR'];
        var dates = ['ATD_DATE', 'CurrentDate', 'RangeInDateTime', 'RangeOutDateTime', 'InDateTime', 'OutDateTime', 'SystemOutDateTime',
            'GraceInDateTime', 'GraceOutDateTime', 'StartTime', 'EndTime'];
        var bools = ['IsLateArrival', 'IsGazetted', 'IsOffDay', 'DRIsOffDuty', 'IsRestDay', 'IsHoliday', 'IsLeave', 'IsLeaveApproved',
            'IsSuspended', 'IsAbsent', 'IsPresent', 'IsArrivalShortLeave', 'IsDepartureShortLeave', 'IsArrivalHalfDay',
            'IsDepartureHalfDay', 'IsEarlyDeparture', 'IsCPL'];
        var order = ['Sr#', 'RM', 'RY', 'EmployeeAttendanceId', 'DutyRoasterDetailId', 'ATD_DATE', 'CurrentDate', 'RangeInDateTime',
            'RangeOutDateTime', 'DAYNAME', 'EmployeeId', 'EmployeeNo', 'EmployeeName', 'Mobile1', 'CNIC', 'Email', 'DepartmentName',
            'DesignationName', 'InEntryMode', 'OutEntryMode', 'InDateTime', 'OutDateTime', 'SystemOutDateTime', 'ShiftTiming', 'InTime',
            'OutTime', 'TimeDuration', 'Late', 'Early', 'Status', 'IsLateArrival', 'SelectedMonth', 'Timing', 'Shift', 'IsGazetted',
            'IsOffDay', 'DRIsOffDuty', 'DRShiftId', 'GraceInDateTime', 'GraceOutDateTime', 'StartTime', 'EndTime', 'Decsription', 'Short',
            'OT', 'ShortHours', 'ShortMinutes', 'OTHours', 'OTMinutes', 'EmployeeHours', 'EmployeeMinutes', 'TotalShortHour',
            'TotalShortMinutes', 'TotalOTMinutes', 'CPLQuota', 'CPLEntryMode', 'TOTAL_HOUR', 'Actual_HOUR', 'EMPLOYEE_WORKINGHOUR',
            'OverTime', 'LateMinutes', 'IsRestDay', 'IsHoliday', 'IsLeave', 'IsLeaveApproved', 'IsSuspended', 'IsAbsent', 'IsPresent',
            'IsArrivalShortLeave', 'IsDepartureShortLeave', 'IsArrivalHalfDay', 'IsDepartureHalfDay', 'IsEarlyDeparture', 'IsCPL',
            'AttendanceStatus', 'BenefitPrefix'];
        var DatagridHistory = new ReportGrid('DatagridHistory', order.map(function (k) {   // GridSetting (GridAutoAdjustment widths)
            if (sumCols.indexOf(k) >= 0) return numCol(k, null, '#,##0.##', '#,##0.##', { hidden: hidden.indexOf(k) >= 0 });
            var c = { key: k, caption: k, hidden: hidden.indexOf(k) >= 0 };
            if (k === 'EmployeeNo') c.type = 'code';
            else if (dates.indexOf(k) >= 0) c.type = 'datetime';
            else if (bools.indexOf(k) >= 0) c.type = 'check';
            return c;
        }));
        var grd = new ReportGrid('grd', [                                   // GridRegisterSetting
            { key: 'SRNo', hidden: true },
            { key: 'EmployeeId', hidden: true },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo + 10, type: 'code' },
            { key: 'EmployeeName', hidden: true },
            { key: 'DepartmentName', hidden: true },
            { key: 'CNIC', caption: 'CNIC', width: K.DriverCnic + 30 },
            { key: 'Email', caption: 'Email', width: K.StringC },
            { key: 'Mobile1', caption: 'Mobile1', width: K.CellNo },
            { key: 'Mobile2', caption: 'Mobile2', width: K.CellNo },
            { key: 'OutDateTime', caption: 'OutDateTime', width: K.Date + 20, type: 'date' },
            { key: 'InDateTime', caption: 'InDateTime', width: K.Date + 20, type: 'date' },
            { key: 'InTime', caption: 'InTime', width: K.Date },
            { key: 'OutTime', caption: 'OutTime', width: K.Date },
            { key: 'TimeDifference', caption: 'TimeDifference', width: K.Date }
        ]);
        function tabFilters() {
            if (tab === 1) return { tab: 1, departmentId: v('cmbDepartmentRegister') };
            return { tab: 0, departmentId: v('cmbDepartment'), shiftId: v('cmbShift'), sectionId: v('cmbSection'), partyLocationId: v('cmbPartyLocation') };
        }
        function Employee() {                                               // Employee()
            return HRM.post(API + '/employees', tabFilters()).then(function (rows) {
                if (rows && rows.length) { fill('cmbEmployee', rows); fill('cmbEmployeeRegister', rows); }
                else if (tab === 0) clearCombo('cmbEmployee');
                else clearCombo('cmbEmployeeRegister');
            }).catch(HRM.fail);
        }
        function Section(departmentId) {                                    // Section(departmentId)
            return HRM.get(API + '/sections', { departmentId: departmentId }).then(function (rows) {
                if (rows && rows.length) fill('cmbSection', rows); else clearCombo('cmbSection');
            }).catch(HRM.fail);
        }
        function combos(d, withEmployees) {
            if (withEmployees && d.employees && d.employees.length) { fill('cmbEmployee', d.employees); fill('cmbEmployeeRegister', d.employees); }
            if (d.departments && d.departments.length) { fill('cmbDepartment', d.departments); fill('cmbDepartmentRegister', d.departments); }
            if (d.partyLocations && d.partyLocations.length) fill('cmbPartyLocation', d.partyLocations);
            if (d.shifts && d.shifts.length) fill('cmbShift', d.shifts);
        }
        function filters() {
            var dt = new Date(HRM.today() + 'T00:00:00');
            return { date: HRM.val('txtDutyDate'), month: HRM.int(HRM.val('txtDutyMonth')) || dt.getMonth() + 1,
                year: HRM.int(HRM.val('txtDutyYear')) || dt.getFullYear(), employeeId: v('cmbEmployee'), departmentId: v('cmbDepartment'),
                sectionId: v('cmbSection'), partyLocationId: v('cmbPartyLocation'), shiftId: v('cmbShift') };
        }
        function args(f) {
            return { month: f.month, year: f.year, employeeId: f.employeeId, departmentId: f.departmentId, sectionId: f.sectionId,
                partyLocationId: f.partyLocationId, shiftId: f.shiftId };
        }
        P.btnShow = function (b) {                                          // btnShow_Click
            var f = filters();
            return HRM.busy(b || 'btnShow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || []; lastArgs = args(f);
                    DatagridHistory.set(dtGrid);
                }).catch(HRM.fail);
            });
        };
        P.btnShowRegister = function (b) {                                  // btnShowRegister_Click -> GridHistory
            var f = { fromDate: HRM.val('txtFromDateRegister'), toDate: HRM.val('txtDateToRegister'),
                employeeId: v('cmbEmployeeRegister'), departmentId: v('cmbDepartmentRegister') };
            return HRM.busy(b || 'btnShowRegister', function () {
                return HRM.post(API + '/show-register', f).then(function (d) {
                    dtGridRegister = d.rows || [];
                    lastRegArgs = { fromDate: dateArg(f.fromDate), toDate: dateArg(f.toDate), employeeId: f.employeeId, departmentId: f.departmentId };
                    grd.set(dtGridRegister, ['DepartmentName', 'EmployeeName']);
                }).catch(HRM.fail);
            });
        };
        P.btnNew = function (b) {                                           // btnNew_Click: Employee, Department, PartyLocation, Shift
            return HRM.busy(b || 'btnNew', function () {
                return Employee().then(function () { return HRM.get(API + '/setup'); }).then(function (d) { combos(d, false); }).catch(HRM.fail);
            });
        };
        P.btnNewRegister = function (b) {                                   // btnNewRegister_Click: Department, Employee, focus From Date
            return HRM.busy(b || 'btnNewRegister', function () {
                return HRM.get(API + '/setup').then(function (d) { combos(d, false); return Employee(); })
                    .then(function () { HRM.focus('txtFromDateRegister'); }).catch(HRM.fail);
            });
        };
        P.btnprint101 = function (b) { return printGrid(b || 'btnprint101', 'hrm-1001', dtGrid, lastArgs); };     // btnPrintRegister_Click
        P.btnPrint1002 = function (b) {                                     // btn1002Print_Click (current filters)
            var f = filters();
            return printChecked(b || 'btnPrint1002', API + '/print-1002-check', f, 'hrm-1002', args(f));
        };
        P.print = function (b) { return printGrid(b || 'print', 'hrm-1004', dtGridRegister, lastRegArgs); };     // print_Click (1004)
        P.tab = function (i) {                                              // tabControl1_SelectedIndexChanged (no work of its own)
            tab = i;
            document.querySelectorAll('.hrm-tab').forEach(function (t, k) { t.classList.toggle('is-active', k === i); });
            document.querySelectorAll('.hrm-tab-page').forEach(function (t, k) { t.classList.toggle('is-active', k === i); });
            status((i === 0 ? DatagridHistory : grd).rows().length);
        };
        // cmbDepartment_ValueChanged -> Section(); the Leave of Department / Section / Shift / PartyLocation -> Employee()
        HRM.$('cmbDepartment').addEventListener('change', function () { Section(v('cmbDepartment')).then(Employee); });
        ['cmbSection', 'cmbShift', 'cmbPartyLocation', 'cmbDepartmentRegister'].forEach(function (id) {
            HRM.$(id).addEventListener('change', function () { Employee(); });
        });
        keys({
            'ctrl+n': function () { if (tab === 0) P.btnNew(); else P.btnNewRegister(); },
            'ctrl+p': function () { if (tab === 0) P.btnprint101(); else P.print(); }
        });
        // HRM_Reports_Load: pickers = today, From Date = 1st of the month
        var now = new Date();
        HRM.setVal('txtDutyDate', HRM.today());
        monthOptions('txtDutyMonth', false);
        HRM.setVal('txtDutyYear', String(now.getFullYear()));
        HRM.setVal('txtFromDateRegister', HRM.firstOfMonth());
        HRM.setVal('txtDateToRegister', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) { combos(d, true); }).catch(HRM.fail);
    }

    // ============================================================================ 373 Daily Attendance
    function dailyAttendance() {
        var dtGrid = [], lastArgs = {};
        var grd = new ReportGrid('grd', [                                   // GridSetting
            { key: 'Sr#', caption: 'Sr#', width: K.DocNo },
            { key: 'DepartmentId', hidden: true },
            { key: 'EmployeeId', hidden: true },
            { key: 'PartyLocationId', hidden: true },
            { key: 'Party', hidden: true },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo + 10, type: 'code' },
            { key: 'Mobile1', caption: 'Mobile1', width: K.CellNo },
            { key: 'Email', hidden: true },
            { key: 'BranchName', hidden: true },
            { key: 'ShiftId', hidden: true },
            { key: 'Employee', caption: 'Employee', width: K.SupplierName + 30 },
            { key: 'Section', caption: 'Section', width: K.StringC },
            { key: 'Department', caption: 'Department', width: K.StringC },
            { key: 'Designation', caption: 'Designation', width: K.StringC },
            { key: 'Shift', caption: 'Shift', width: K.StringC },
            { key: 'DayName', caption: 'DayName', width: K.StringC - 40 },
            { key: 'SelectedMonth', hidden: true },
            { key: 'ShiftTiming', caption: 'ShiftTiming', width: K.StringC + 100 },
            { key: 'InTime', caption: 'InTime', width: K.Date },
            { key: 'OutTime', caption: 'OutTime', width: K.Date },
            { key: 'TimeDifference', caption: 'TimeDifference', width: K.Date },
            { key: 'Date', caption: 'Date', width: K.Date + 20, type: 'date' },
            { key: 'Status', caption: 'Status', width: K.Status + 20 }
        ]);
        function EmployeeFill() {                                           // EmployeeFill: rebinds only when rows came back
            return HRM.get(API + '/employees', { departmentId: v('cmbDepartment') }).then(function (rows) {
                if (rows && rows.length) fill('cmbEmployee', rows);
            }).catch(HRM.fail);
        }
        function GridHistory(b) {
            var f = { departmentId: v('cmbDepartment'), employeeId: v('cmbEmployee'), date: HRM.val('txtDate') };
            return HRM.busy(b || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    lastArgs = { departmentId: f.departmentId, employeeId: f.employeeId, date: dateArg(f.date) };
                    grd.set(dtGrid);
                }).catch(HRM.fail);
            });
        }
        P.btnshow = function (b) { return GridHistory(b); };
        P.btnNew = function (b) {                                           // btnNew_Click: DepartmentFill, EmployeeFill, txtDate.Focus
            return HRM.busy(b || 'btnNew', function () {
                return HRM.get(API + '/setup').then(function (d) { if (d.departments && d.departments.length) fill('cmbDepartment', d.departments); })
                    .then(EmployeeFill).then(function () { HRM.focus('txtDate'); }).catch(HRM.fail);
            });
        };
        P.Print = function (b) { return printGrid(b || 'Print', 'hrm-1003', dtGrid, lastArgs); };                 // print_Click
        P.btnprint = function (b) {                                                                             // btnprint_Click
            return printGrid(b || 'btnprint', 'hrm-1003-01', dtGrid, lastArgs, 'Record Not Found For Display');
        };
        HRM.$('cmbDepartment').addEventListener('change', EmployeeFill);    // cmbDepartment_Leave
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.Print(); } });
        HRM.setVal('txtDate', HRM.today());                                 // txtDate.Value = DateTime.Now
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {           // DepartmentFill, EmployeeFill
            if (d.departments && d.departments.length) fill('cmbDepartment', d.departments);
            if (d.employees && d.employees.length) fill('cmbEmployee', d.employees);
        }).catch(HRM.fail);
    }

    // ============================================================================ 374 Daily Late & Early Departure
    function dailyLateEarly() {
        var dtGrid = [], lastArgs = {};
        var grd = new ReportGrid('grd', [
            { key: 'SRNo', caption: 'SRNo', width: K.DocNo },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo + 10, type: 'code' },
            { key: 'Employee', caption: 'Employee', width: K.SupplierName },
            { key: 'PartyLocationId', hidden: true },
            { key: 'PartyName', hidden: true },
            { key: 'DepartmentId', hidden: true },
            { key: 'Department', caption: 'Department', width: K.StringC },
            { key: 'Designation', caption: 'Designation', width: K.StringC },
            { key: 'Shift', caption: 'Shift', width: K.StringC },
            { key: 'Timing', caption: 'Timing', width: K.StringC },
            { key: 'EmployeeInTime', caption: 'EmployeeInTime', width: K.Date },
            { key: 'InTimeDifference', caption: 'InTimeDifference', width: K.Date },
            { key: 'EmployeeOutTime', caption: 'EmployeeOutTime', width: K.Date },
            { key: 'OutTimeDifference', caption: 'OutTimeDifference', width: K.Date },
            { key: 'TotalHour', caption: 'TotalHour', width: K.Date },
            { key: 'Early', hidden: true },
            { key: 'Late', hidden: true },
            { key: 'Date', caption: 'Date', width: K.Date + 20, type: 'date' },
            { key: 'EmployeeId', hidden: true }
        ]);
        function GridHistory(b) {
            var f = { partyLocationId: v('cmbPartyLocation'), date: HRM.val('txtDate') };
            return HRM.busy(b || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    lastArgs = { partyLocationId: f.partyLocationId, date: dateArg(f.date) };
                    grd.set(dtGrid);
                }).catch(HRM.fail);
            }, 'show');
        }
        P.btnshow = function (b) { return GridHistory(b); };
        P.btnNew = function (b) {                                           // btnNew_Click: date = now, focus Location, GridHistory
            HRM.setVal('txtDate', HRM.today());
            HRM.focus('cmbPartyLocation');
            return GridHistory(b || 'btnNew');
        };
        P.print = function (b) { return printGrid(b || 'print', 'hrm-1113', dtGrid, lastArgs); };
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.print(); } });
        HRM.setVal('txtDate', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {           // DepartmentFill (genLocation)
            if (d.locations && d.locations.length) fill('cmbPartyLocation', d.locations);
        }).catch(HRM.fail);
    }

    // ============================================================================ 375 Duty Roster Employee Wise
    function dutyRoster() {
        var dtGrid = [], lastArgs = {};
        var cols = [
            { key: 'EmployeeId', hidden: true },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo + 10, type: 'code' },
            { key: 'EmployeeName', caption: 'EmployeeName', width: K.SupplierName + 20 },
            { key: 'CNIC', caption: 'CNIC', width: K.DriverCnic + 30 },
            { key: 'Mobile1', caption: 'Mobile1', width: K.CellNo },
            { key: 'EmployeeType', caption: 'EmployeeType', width: K.DiscountType + 20 },
            { key: 'DesignationName', caption: 'DesignationName', width: K.StringC },
            { key: 'DepartmentName', caption: 'DepartmentName', width: K.StringC },
            { key: 'ShiftName', caption: 'ShiftName', width: K.StringC },
            { key: 'ShiftShortName', caption: 'ShiftShortName', width: K.StringC },
            { key: 'Date', caption: 'Date', width: K.Date + 20, type: 'date' },
            { key: 'Year', caption: 'Year', width: K.DocNo },
            { key: 'Month', caption: 'Month', width: K.DocNo },
            { key: 'Timing', caption: 'Timing', width: K.StringC },
            { key: 'IsOffDuty', caption: 'IsOffDuty', width: K.Date, type: 'check' }
        ];
        var grd = new ReportGrid('grd', cols);
        function EmployeeFill() {
            return HRM.get(API + '/setup').then(function (d) { if (d.employees && d.employees.length) fill('cmbEmployee', d.employees); });
        }
        function pickedDate() {                                             // txtDate ("MMM"): its month and year
            return HRM.int(HRM.val('txtYear')) + '-' + String(HRM.int(HRM.val('txtMonth'))).padStart(2, '0') + '-01';
        }
        function GridHistory(b) {
            var f = { employeeId: v('cmbEmployee'), date: pickedDate() };
            var picked = f.employeeId > 0;                                  // cmbEmployee.ActiveRow != null
            return HRM.busy(b || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    lastArgs = { employeeId: f.employeeId, date: dateArg(f.date) };
                    grd.hide('EmployeeName', picked);                        // GridSetting: hidden + grouped by EmployeeName
                    grd.set(dtGrid, picked ? ['EmployeeName'] : []);
                }).catch(HRM.fail);
            });
        }
        P.btnshow = function (b) { return GridHistory(b); };
        P.btnNew = function (b) { return HRM.busy(b || 'btnNew', function () { return EmployeeFill().catch(HRM.fail); }); };
        P.print = function (b) { return printGrid(b || 'print', 'hrm-1114', dtGrid, lastArgs); };
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.print(); } });
        monthOptions('txtMonth', false);
        HRM.setVal('txtYear', String(new Date().getFullYear()));
        HRM.loading(EmployeeFill()).catch(HRM.fail);
    }

    // ============================================================================ 376 Employee Attendence
    function employeeAttendence() {
        var dtGrid = [], lastArgs = {};
        var grd = new ReportGrid('grd', [                                   // GridSetting
            { key: 'SRNo', hidden: true },
            { key: 'EmployeeId', hidden: true },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo + 10, type: 'code' },
            { key: 'EmployeeName', hidden: true },
            { key: 'DepartmentName', hidden: true },
            { key: 'CNIC', caption: 'CNIC', width: K.DriverCnic + 30 },
            { key: 'Email', caption: 'Email', width: K.StringC },
            { key: 'Mobile1', caption: 'Mobile1', width: K.CellNo },
            { key: 'Mobile2', caption: 'Mobile2', width: K.CellNo },
            { key: 'OutDateTime', caption: 'OutDateTime', width: K.Date + 20, type: 'date' },
            { key: 'InDateTime', caption: 'InDateTime', width: K.Date + 20, type: 'date' },
            { key: 'InTime', caption: 'InTime', width: K.Date },
            { key: 'OutTime', caption: 'OutTime', width: K.Date },
            { key: 'TimeDifference', caption: 'TimeDifference', width: K.Date }
        ]);
        function EmployeeFill() {
            return HRM.get(API + '/employees', { departmentId: v('cmbDepartment') }).then(function (rows) {
                if (rows && rows.length) fill('cmbEmployee', rows);
            }).catch(HRM.fail);
        }
        P.btnshow = function (b) {                                          // btnshow_Click -> GridHistory
            var f = { fromDate: HRM.val('txtFromDate'), toDate: HRM.val('txtDateTo'), employeeId: v('cmbEmployee'), departmentId: v('cmbDepartment') };
            return HRM.busy(b || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    lastArgs = { fromDate: dateArg(f.fromDate), toDate: dateArg(f.toDate), employeeId: f.employeeId, departmentId: f.departmentId };
                    grd.set(dtGrid, ['DepartmentName', 'EmployeeName']);
                }).catch(HRM.fail);
            });
        };
        P.btnNew = function (b) {                                           // DepartmentFill, EmployeeFill, txtFromDate.Focus
            return HRM.busy(b || 'btnNew', function () {
                return HRM.get(API + '/setup').then(function (d) { if (d.departments && d.departments.length) fill('cmbDepartment', d.departments); })
                    .then(EmployeeFill).then(function () { HRM.focus('txtFromDate'); }).catch(HRM.fail);
            });
        };
        P.print = function (b) { return printGrid(b || 'print', 'hrm-1004', dtGrid, lastArgs); };
        P.btnSummaryPrint = function () { /* btnSummaryPrint_Click: empty on the desktop - "1005-Print Summary" does nothing */ };
        HRM.$('cmbDepartment').addEventListener('change', EmployeeFill);    // cmbDepartment_Leave
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.print(); } });
        HRM.setVal('txtFromDate', HRM.firstOfMonth());                      // new DateTime(Now.Year, Now.Month, 1)
        HRM.setVal('txtDateTo', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {
            if (d.departments && d.departments.length) fill('cmbDepartment', d.departments);
            if (d.employees && d.employees.length) fill('cmbEmployee', d.employees);
        }).catch(HRM.fail);
    }

    // ============================================================================ 377 Monthly Attendance Summary
    function monthlySummary() {
        var dtGrid = [], lastArgs = {};
        var D = K.Days;
        var grd = new ReportGrid('grd', [                                   // GridSetting
            { key: 'EmployeeId', hidden: true },
            { key: 'ShiftId', hidden: true },
            { key: 'ShiftName', caption: 'ShiftName', width: K.StringC },
            { key: 'EmployeeNo', caption: 'EmployeeNo', width: K.DocNo + 10, type: 'code' },
            { key: 'EmployeeName', caption: 'EmployeeName', width: K.SupplierName + 20, type: 'code', link: 'print' },   // ColumnType.Link
            { key: 'DepartmentName', caption: 'DepartmentName', width: K.StringC },
            { key: 'DesignationName', caption: 'DesignationName', width: K.StringC },
            { key: 'Section', caption: 'Section', width: K.StringC },
            { key: 'AsOnDate', caption: 'AsOnDate', width: K.Date + 20, type: 'date' },
            { key: 'DiffDays', caption: 'DiffDays', width: D },
            { key: 'DutyMonth', caption: 'DutyMonth', width: D },
            { key: 'DutyYear', caption: 'DutyYear', width: D },
            { key: 'TotalDays', caption: 'TotalDays', width: D },
            { key: 'TotalHoliday', caption: 'TotalHoliday', width: D },
            { key: 'TotalOffDAys', caption: 'TotalOffDAys', width: D },
            { key: 'DutyOffDays', caption: 'DutyOffDays', width: D },
            { key: 'TotalPresent', caption: 'TotalPresent', width: D },
            { key: 'TotalAbsent', caption: 'TotalAbsent', width: D },
            { key: 'ApplyLeave', caption: 'ApplyLeave', width: D },
            { key: 'TotalLeaves', caption: 'TotalLeaves', width: D },
            { key: 'TotalLate', caption: 'TotalLate', width: D },
            { key: 'Month', caption: 'Month', width: K.Date },
            { key: 'Year', caption: 'Year', width: K.DocNo },
            { key: 'DutyDate', caption: 'DutyDate', width: K.Date + 20, type: 'date' },
            { key: 'TOTAL_HOUR', caption: 'TOTAL_HOUR', width: D },
            { key: 'Actual_HOUR', caption: 'Actual_HOUR', width: D },
            { key: 'EMPLOYEE_WORKINGHOUR', caption: 'EMPLOYEE_WORKINGHOUR', width: D + 20 },
            { key: 'OT', caption: 'OT', width: D + 10 }
        ], {
            onCode: function (r, c) {
                if (c && c.link === 'print') {                              // grd_LinkClicked (EmployeeName) -> PrintMonthlyAttendance
                    unavailable('EmployeeAttendenceMonthly.rpt');
                    return;
                }
                openEmployee(r);
            }
        });
        function filters() {
            return { departmentId: v('cmbDepartment'), employeeId: v('cmbEmployee'), sectionId: v('CmbSection'),
                month: HRM.int(HRM.val('datMonth')), year: HRM.int(HRM.val('datYear')) };
        }
        function EmployeeFill() {
            return HRM.get(API + '/employees', { departmentId: v('cmbDepartment') }).then(function (rows) {
                if (rows && rows.length) fill('cmbEmployee', rows);
            });
        }
        function GridHistory(b) {
            var f = filters();
            return HRM.busy(b || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    lastArgs = { month: f.month, year: f.year, departmentId: f.departmentId, employeeId: f.employeeId, sectionId: f.sectionId };
                    grd.set(dtGrid);
                }).catch(HRM.fail);
            }, 'show');
        }
        function combos(d) {
            if (d.departments && d.departments.length) fill('cmbDepartment', d.departments);
            if (d.employees && d.employees.length) fill('cmbEmployee', d.employees);
            fill('CmbSection', d.sections);                                 // BindAndRetainSelection, no default row
        }
        P.btnshow = function (b) { return GridHistory(b); };
        P.btnNew = function (b) {                                           // DepartmentFill, EmployeeFill, SectionFill, GridHistory
            return HRM.busy(b || 'btnNew', function () {
                return HRM.get(API + '/setup').then(function (d) {
                    if (d.departments && d.departments.length) fill('cmbDepartment', d.departments);
                    return EmployeeFill().then(function () { fill('CmbSection', d.sections); });
                }).catch(HRM.fail);
            }).then(function () { return GridHistory(); });
        };
        P.print = function (b) { return printGrid(b || 'print', 'hrm-1006', dtGrid, lastArgs); };              // print_Click (1006)
        P.btnMonthlyRegister = function (b) {                               // btnMonthlyRegister_Click (1005)
            var f = filters();
            return printChecked(b || 'btnMonthlyRegister', API + '/print-1005-check', f, 'hrm-1005',
                { month: f.month, year: f.year, employeeId: f.employeeId, departmentId: f.departmentId });
        };
        HRM.$('cmbDepartment').addEventListener('change', function () { EmployeeFill().catch(HRM.fail); });     // cmbDepartment_Leave
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.print(); } });
        monthOptions('datMonth', true);
        HRM.setVal('datYear', String(new Date().getFullYear()));
        HRM.loading(HRM.get(API + '/setup')).then(combos).catch(HRM.fail);
    }

    // ============================================================================ 378 Salary Sheet
    function salarySheet() {
        var dtGrid = [];
        var A = K.Amount;
        var sums = ['NetSalary', 'AddLessAmount', 'TotalAddition', 'ArrearAmount', 'FoodAmount', 'MobileAmount', 'MiscAddAmount',
            'MiscLessAmount', 'FuelAmount', 'MedicalAmount', 'TravellingAmount', 'OTAmount', 'TotalLessAmount', 'EOBIAmount', 'PFAmount',
            'NoOfInstallments', 'LoanAmount', 'AdvanceAmount', 'LeaveAmount', 'IncomeTaxAmount', 'LateDeductionAmount', 'GrossSalary',
            'BasicSalary', 'EmployeePayrollSalary', 'EmployeePerDaySalary', 'EmployeeDays', 'PresentDays', 'OffDays', 'LeaveDays',
            'SalaryDays', 'AbsentDays'];
        var widths = { AccountCode: K.AccountCode - 20, EmployeeNo: K.DocNo, EmployeeName: K.SupplierName + 30, JoinDate: K.Date + 20,
            DesignationName: 150, DepartmentName: 130, SectionName: 120, MONTH: 70, YEAR: 60, TotalYear: 65, PresentDays: K.DocNo,
            OffDays: K.DocNo, LeaveDays: K.DocNo, SalaryDays: K.DocNo, AbsentDays: K.DocNo, EmployeeDays: K.DocNo,
            EmployeePerDaySalary: A + 20, EmployeePayrollSalary: A + 20 };
        var order = ['EmployeeId', 'EmployeeHistoryId', 'AccountCode', 'EmployeeNo', 'EmployeeName', 'JoinDate', 'DesignationName',
            'DepartmentName', 'SectionName', 'DepartmentId', 'MONTH', 'YEAR', 'TotalYear', 'PresentDays', 'OffDays', 'LeaveDays',
            'SalaryDays', 'AbsentDays', 'EmployeeDays', 'EmployeePerDaySalary', 'EmployeePayrollSalary', 'BasicSalary', 'GrossSalary',
            'LateDeductionAmount', 'IncomeTaxAmount', 'LeaveAmount', 'AdvanceAmount', 'LoanAmount', 'NoOfInstallments', 'PFAmount',
            'EOBIAmount', 'TotalLessAmount', 'OTAmount', 'TravellingAmount', 'MedicalAmount', 'FuelAmount', 'MiscLessAmount',
            'MiscAddAmount', 'MobileAmount', 'FoodAmount', 'ArrearAmount', 'TotalAddition', 'AddLessAmount', 'NetSalary'];
        var grd = new ReportGrid('grd', order.map(function (k) {             // GridSetting
            var w = widths[k] || A;
            if (sums.indexOf(k) >= 0) return numCol(k, w, '0,0', '#,##0.##', k === 'ArrearAmount' ? { caption: 'Arear Amount' } : null);
            var c = { key: k, caption: k, width: w, hidden: k === 'EmployeeId' || k === 'DepartmentId' || k === 'EmployeeHistoryId' };
            if (k === 'EmployeeNo') { c.type = 'code'; c.link = 'slip'; }  // ColumnType.Link -> SalarySlipRpt(EmployeeId)
            if (k === 'EmployeeName') c.type = 'code';                      // web: opens the employee
            if (k === 'JoinDate') c.type = 'date';
            return c;
        }), {
            onCode: function (r, c) {
                if (c && c.link === 'slip') { SalarySlipRpt(null, HRM.int(r.EmployeeId), 0); return; }   // grd_LinkClicked
                openEmployee(r);
            }
        });
        function monthYearOk() { return v('cmbMonth') > 0 && v('cmbYear') > 0; }
        function common() {
            return { month: v('cmbMonth'), year: v('cmbYear'), debitAccountId: v('CmbDebitAccount') };
        }
        /** SalarySlipRpt(EmployeeId, DepartmentId): "Month Or Year Field Required", Sp_EmployeeSalarySlip_Rpt, 1110B. */
        function SalarySlipRpt(btn, employeeId, departmentId, missingRpt) {
            if (!monthYearOk()) { HRM.box('Month Or Year Field Required'); return; }
            var f = Object.assign(common(), { employeeId: employeeId, departmentId: departmentId });
            return printChecked(btn, API + '/slip-check', f, 'hrm-1110B', f, missingRpt);
        }
        function YearMonth(d) {
            fill('cmbYear', d.years, 'Year');                               // YearFill: ActiveYr.Start_Period.Year .. Now.Year
            HRM.fillFixed('cmbMonth', HRM.MONTHS.map(function (m, i) { return [i + 1, m]; }), { zero: '', keep: true });   // MonthFill
        }
        function load(first) {
            return HRM.get(API + '/setup').then(function (d) {
                YearMonth(d);
                if (d.departments && d.departments.length) fill('cmbDepartment', d.departments);   // BindDDLNew
                if (d.employees && d.employees.length) fill('cmbEmployee', d.employees);
                if (first && d.accounts && d.accounts.length) fill('CmbDebitAccount', d.accounts, 'AccountTitle');   // GetAccountsFromEmployee
            });
        }
        P.btnshow = function (b) {                                          // btnshow_Click -> GridHistory
            if (v('cmbMonth') <= 0) { HRM.box('Month field Required...'); HRM.focus('cmbMonth'); return; }
            if (v('cmbYear') <= 0) { HRM.box('Year field Required...'); HRM.focus('cmbYear'); return; }
            var f = Object.assign(common(), { departmentId: v('cmbDepartment'), sectionId: v('cmbSection'), employeeId: v('cmbEmployee'),
                monthName: HRM.comboText('cmbMonth'), posted: HRM.checked('RadPosted') });
            return HRM.busy(b || 'btnshow', function () {
                return HRM.post(API + '/show', f).then(function (d) {
                    dtGrid = d.rows || [];
                    grd.set(dtGrid);
                }).catch(HRM.fail);
            });
        };
        P.btnNew = function (b) {                                           // YearFill, MonthFill, DepartmentFill, EmployeeFill, focus
            return HRM.busy(b || 'btnNew', function () {
                return load(false).then(function () { HRM.focus('cmbDepartment'); }).catch(HRM.fail);
            });
        };
        P.tsDropDownPrint = function () {                                   // tsDropDownPrint_DropDownItemClicked
            if (!dtGrid.length) { HRM.box(NO_REC); return; }
            /* CommonServices.DynamicReportsLoad: the grid (dtGrid) goes into the SalarySheet report - Jasper */
            if (window.printRowsJasper) return window.printRowsJasper('1100-Employee Salary Sheet', dtGrid);
            HRM.box('Include /js/print-rpt.js?v=20261003erp to print.');
        };
        P.btn1110SalaryList = function (b) {                                // btn1110SalaryList_Click (cmbDepartment, cmbEmployee)
            /* 1110-EmployeeSalarySlip.rpt is not in the report folder; its procedure (Sp_EmployeeSalarySlip_Rpt,
               seeded contract 1110-employeesalaryslip, aligned with PayRollReports.EmployeeSalarySlip_Rpt: Month/Year
               always, EmployeeId / DepartmentId / DebitAccountId only when != 0) prints through a Jasper layout */
            if (!monthYearOk()) { HRM.box('Month Or Year Field Required'); return; }
            var f = Object.assign(common(), { employeeId: v('cmbEmployee'), departmentId: v('cmbDepartment') });
            return printChecked(b || 'btn1110SalaryList', API + '/slip-check', f, '1110-employeesalaryslip', f);
        };
        P.btn1110ASalarySlip = function (b) {                               // btn1110ASalarySlip_Click
            return SalarySlipRpt(b || 'btn1110ASalarySlip', v('cmbEmployee'), v('cmbDepartment'));
        };
        HRM.$('cmbDepartment').addEventListener('change', function () {     // cmbDepartment_Leave: EmployeeFill + Section
            HRM.get(API + '/department', { departmentId: v('cmbDepartment') }).then(function (d) {
                if (d.employees && d.employees.length) fill('cmbEmployee', d.employees);
                if (d.sections && d.sections.length) fill('cmbSection', d.sections); else clearCombo('cmbSection');
            }).catch(HRM.fail);
        });
        keys({ 'ctrl+n': function () { P.btnNew(); }, 'ctrl+p': function () { P.tsDropDownPrint(); } });
        HRM.loading(load(true)).then(function () { HRM.focus('cmbMonth'); }).catch(HRM.fail);
    }

    var PAGES = {
        'employee-register': employeeRegister,
        'employee-monthly-register': monthlyRegister,
        'daily-attendance': dailyAttendance,
        'daily-late-and-early-departure': dailyLateEarly,
        'duty-roster-employee-wise': dutyRoster,
        'employee-attendence': employeeAttendence,
        'monthly-attendance-summary': monthlySummary,
        'salary-sheet': salarySheet
    };
    HRM.footer(null);                                                     // reports have no History of their own
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
