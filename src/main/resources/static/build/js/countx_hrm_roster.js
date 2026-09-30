/* ============================================================================================
 * countx_hrm_roster.js - HRM duty roster screens (AppModules 2022 "Attendance Management").
 * One script, the page is chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   duty-roaster       frmDutyRoasterNew.cs  (656)
 *   employee-roaster   frmEmployeeRoaster.cs (657)
 *
 * The page holds the form's lstDutyRoasterDetail (one row per employee x shift x date from
 * SP_GetDutyDatesForDutyRoaster). Clicking a date (grdDutyDate_Click) shows that date's shifts in the
 * employee grid; ticking a shift (grdEmployee_Click) writes the ticks of that employee back to the list
 * for the date on the label; Apply (btnApply_Click) and Apply all (CheckAgainstAllEmployee) change the
 * list in bulk. Save sends the rows still on duty (the form's RemoveAll(!IsOnDuty)) with the grdGroup
 * rows; the server adds the off days and saves header + details + groups in one transaction.
 * Each handler names the desktop method it ports.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/roster/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmRoster
    window.HrmRoster = P;

    // ------------------------------------------------------------------------ helpers
    /** Convert.ToInt32(double): nearest integer, a .5 goes to the even neighbour. */
    function toInt32(x) {
        var f = Math.floor(x), d = x - f;
        if (Math.abs(d - 0.5) < 1e-9) return (f % 2 === 0) ? f : f + 1;
        return Math.round(x);
    }
    /** Time of day of a DateTimePicker value as a fraction of a day. */
    function dayFrac(d) { return (d.getHours() * 3600 + d.getMinutes() * 60 + d.getSeconds()) / 86400; }
    function lblText(iso) { return iso ? HRM.fmtDate(iso) : ''; }
    /** A fresh copy of a table element (drops the listeners of a previous HRM.Grid on it). */
    function freshTable(id) {
        var t = HRM.$(id); if (!t) return null;
        var n = t.cloneNode(false);
        n.removeAttribute('class');
        t.parentNode.replaceChild(n, t);
        return n;
    }
    /** GridEX.ClearStructure: no columns, no rows. */
    function clearStructure(id) { var t = freshTable(id); if (t) t.innerHTML = ''; }
    /** "dd-MMM-yyyy hh:mm tt" of the History grid's EntryDate / ModifyDate. */
    function fmt12(v) {
        var d = HRM.fmtDate(v); if (!d) return '';
        var t = HRM.time(v); if (!t) return d;
        var h = +t.slice(0, 2), m = t.slice(3, 5);
        var ap = h < 12 ? 'AM' : 'PM'; h = h % 12; if (h === 0) h = 12;
        return d + ' ' + String(h).padStart(2, '0') + ':' + m + ' ' + ap;
    }
    function onlyDigits(el) {                                             // CommonServices.OnlytextNumberFunction
        if (!el) return;
        el.addEventListener('keypress', function (e) { if (e.key && e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        el.addEventListener('input', function () { var v = el.value.replace(/[^0-9]/g, ''); if (v !== el.value) el.value = v; });
        el.addEventListener('paste', function (e) { e.preventDefault(); });   // ShortcutsEnabled = false
    }

    // ============================================================================ roster core
    /**
     * The three grids of both forms and lstDutyRoasterDetail.
     *   o.shifts()  -> dtShift rows {ShiftId, ShiftName}; o.week() -> dtWeekProfile rows {ProfileId, ProfileName}
     */
    function RosterCore(o) {
        var C = this;
        C.lst = [];                 // lstDutyRoasterDetail
        C.idx = {};                 // EmployeeId|DutyDate|ShiftShortName -> rows (List.Find = [0], Where = all)
        C.lbl = '';                 // lblDutyDate (yyyy-MM-dd)
        C.empCols = [];             // grdEmployee columns (Index >= 8 are the shift check boxes)
        C.loadFilter = null;        // the filter the grids were loaded with
        C.gDates = null; C.gEmp = null; C.gGroup = null;

        C.setLbl = function (iso) { C.lbl = iso || ''; HRM.text('lblDutyDate', lblText(C.lbl)); };

        C.reindex = function () {
            C.idx = {};
            C.lst.forEach(function (x) {
                var k = x.EmployeeId + '|' + x.DutyDate + '|' + x.ShiftShortName;
                (C.idx[k] || (C.idx[k] = [])).push(x);
            });
        };
        C.rows = function (emp, date, shortName) { return C.idx[emp + '|' + date + '|' + shortName] || []; };

        C.shiftCols = function () { return C.empCols.slice(8); };

        /** Reset(): grdDutyDate / grdGroup / grdEmployee.ClearStructure(), lstDutyRoasterDetail.Clear(), lblDutyDate = "". */
        C.clear = function () {
            clearStructure('grdDutyDate'); clearStructure('grdGroup'); clearStructure('grdEmployee');
            C.gDates = C.gEmp = C.gGroup = null;
            C.lst = []; C.idx = {}; C.empCols = []; C.loadFilter = null;
            C.setLbl('');
        };

        /** MannualDataSetInDutyRoaster after GetDutyDatesForDutyRoaster returned: binds the four tables. */
        C.bind = function (d, filter) {
            if (!d || !d.tables) { clearStructure('grdGroup'); C.gGroup = null; return; }   // ds.Tables.Count == 0
            C.loadFilter = filter;
            // grdGroup: EmployeeGroupId hidden, EmployeeGroupName read-only, ShiftId "Shift" / WeekDayName "RestDay" value lists
            C.gGroup = new HRM.Grid(freshTable('grdGroup'), {
                columns: [
                    { key: 'EmployeeGroupId', caption: 'EmployeeGroupId', hidden: true },
                    { key: 'EmployeeGroupName', caption: 'EmployeeGroupName', width: 150 },
                    { key: 'ShiftId', caption: 'Shift', type: 'select', width: 120,
                        options: function () { return [['', '']].concat(o.shifts().map(function (s) { return [HRM.str(HRM.col(s, 'ShiftId')), HRM.str(HRM.col(s, 'ShiftName'))]; })); } },
                    { key: 'WeekDayName', caption: 'RestDay', type: 'select', width: 120,
                        options: function () { return [['', '']].concat(o.week().map(function (s) { return [HRM.str(HRM.col(s, 'ProfileId')), HRM.str(HRM.col(s, 'ProfileName'))]; })); } }
                ],
                emptyText: ''
            });
            C.gGroup.set(d.groups || []);
            // grdDutyDate: HolidayDetail / IsOffDay / IsGazetted hidden; FormatCondition HolidayDetail = "Holiday" -> bold red
            C.gDates = new HRM.Grid(freshTable('grdDutyDate'), {
                columns: [
                    { key: 'Rno', caption: 'Rno', type: 'int', width: 40 },
                    { key: 'DutyDate', caption: 'DutyDate', type: 'date', width: 90 },
                    { key: 'WeekDayName', caption: 'WeekDayName', width: 90 },
                    { key: 'HolidayDetail', caption: 'HolidayDetail', hidden: true },
                    { key: 'IsOffDay', caption: 'IsOffDay', hidden: true },
                    { key: 'IsGazetted', caption: 'IsGazetted', hidden: true }
                ],
                rowClass: function (r) { return HRM.str(HRM.col(r, 'HolidayDetail')) === 'Holiday' ? 'hrm-row-red hrm-row-bold' : ''; },
                emptyText: ''
            });
            C.gDates.set(d.dates || []);
            // grdEmployee: columns of Tables[2] in order; DefaultShift / EmployeeId / EmployeeGroupId hidden; Index >= 8 check boxes
            C.empCols = (d.empCols || []).slice();
            var widths = { EmployeeName: 250, DepartmentName: 120, DesignationName: 120 };
            var hide = { DefaultShift: 1, EmployeeId: 1, EmployeeGroupId: 1 };
            C.gEmp = new HRM.Grid(freshTable('grdEmployee'), {
                columns: C.empCols.map(function (c, i) {
                    return { key: c, caption: c, hidden: !!hide[c], width: widths[c], type: i >= 8 ? 'edit-check' : 'text' };
                }),
                emptyText: ''
            });
            C.gEmp.set(d.emp || []);
            // lstDutyRoasterDetail, DutyRoasterDetailLineId = row index + 1, ActionTypeId 2 when it has a DutyRoasterDetailId
            var cols = d.allCols || [];
            C.lst = (d.all || []).map(function (a, j) {
                var x = {};
                cols.forEach(function (c, i) { x[c] = a[i]; });
                x.DutyRoasterDetailLineId = j + 1;
                x.ActionTypeId = HRM.int(x.DutyRoasterDetailId) > 0 ? 2 : 1;
                return x;
            });
            C.reindex();
            // lblDutyDate = grdDutyDate.CurrentRow (the first row)
            if (C.gDates.data.length) { C.gDates.select(0); C.setLbl(HRM.day(HRM.col(C.gDates.data[0], 'DutyDate'))); }
            else C.setLbl('');
        };

        /** grdDutyDate_Click: the label takes the current date and each shift cell shows that date's IsOnDuty (List.Find). */
        C.dutyDateClick = function () {
            if (!C.gDates) return;
            var item = C.gDates.current(); if (!item) return;
            var date = HRM.day(HRM.col(item, 'DutyDate'));
            C.setLbl(date);
            if (!C.gEmp) return;
            var sc = C.shiftCols();
            C.gEmp.rows().forEach(function (r) {
                sc.forEach(function (c) {
                    var hit = C.rows(HRM.int(HRM.col(r, 'EmployeeId')), date, c)[0];
                    if (hit) r[c] = hit.IsOnDuty;
                });
            });
            C.gEmp.draw();
        };

        /** grdEmployee_Click: the current row's shift ticks go to every list row of that employee, the label's date and that shift. */
        C.employeeClick = function () {
            if (!C.gEmp) return;
            var item = C.gEmp.current(); if (!item) return;
            var emp = HRM.int(HRM.col(item, 'EmployeeId'));
            if (C.lbl === '') { HRM.box('First Select Date'); return; }
            C.shiftCols().forEach(function (c) {
                var v = HRM.bool(item[c]);
                C.rows(emp, C.lbl, c).forEach(function (x) { x.IsOnDuty = v; });
            });
        };

        function validGroup(r) {
            return HRM.int(r.EmployeeGroupId) > 0 && HRM.int(r.ShiftId) > 0 && HRM.int(r.WeekDayName) > 0;
        }
        function weekText(v) {                                             // item.Cells["WeekDayName"].Text
            var s = String(v === null || v === undefined ? '' : v), w = o.week();
            for (var i = 0; i < w.length; i++) if (HRM.str(HRM.col(w[i], 'ProfileId')) === s) return HRM.str(HRM.col(w[i], 'ProfileName'));
            return '';
        }

        /** btnApply_Click */
        C.apply = function () {
            var rows = C.gGroup ? C.gGroup.rows() : [];
            if (rows.length === 0) { HRM.box('Grid Record Not Found'); return; }
            var count = 0;
            for (var i = 0; i < rows.length; i++) {
                if (validGroup(rows[i])) {
                    C.lst.forEach(function (x) { if (!HRM.bool(x.IsHoliday)) x.IsOnDuty = false; });
                    count++;
                    break;
                }
            }
            if (count === 0) { HRM.box('EmployeeGroup,ShiftName And WeekDayName must be select'); return; }
            rows.forEach(function (r) {
                if (!validGroup(r)) return;
                var g = HRM.int(r.EmployeeGroupId), s = HRM.int(r.ShiftId), w = weekText(r.WeekDayName);
                C.lst.forEach(function (x) { if (HRM.int(x.EmployeeGroupId) === g && HRM.int(x.ShiftId) === s && !HRM.bool(x.IsHoliday)) x.IsOnDuty = true; });
                C.lst.forEach(function (x) { if (HRM.int(x.EmployeeGroupId) === g && x.WeekDayName === w && !HRM.bool(x.IsHoliday)) x.IsOnDuty = false; });
            });
            C.dutyDateClick();                                              // grdDutyDate.Focus(); grdDutyDate_Click(null, null)
        };

        /** CheckAgainstAllEmployee (btnApplyAll_Click): the label date's shifts copied to that date and every later row of grdDutyDate. */
        C.applyAll = function () {
            if (!C.gDates || !C.gEmp) return;
            var d1 = C.lbl, start = false, sc = C.shiftCols();
            C.gDates.visibleRows().forEach(function (item) {
                var d2 = HRM.day(HRM.col(item, 'DutyDate'));
                if (!start) {
                    if (!(d1 === '' || d2 >= d1)) return;                  // DateTime.MinValue when the label is empty
                    start = true;
                }
                C.gEmp.rows().forEach(function (r) {
                    var emp = HRM.int(HRM.col(r, 'EmployeeId'));
                    sc.forEach(function (c) {
                        var obj = C.rows(emp, d1, c)[0];
                        if (!obj) return;
                        r[c] = obj.IsOnDuty;
                        var v = HRM.bool(obj.IsOnDuty);
                        C.rows(emp, d2, c).forEach(function (x) { x.IsOnDuty = v; });
                    });
                });
            });
            C.gEmp.draw();
        };

        /** Insert(): lstDutyRoasterDetail.RemoveAll(val => !val.IsOnDuty) */
        C.prune = function () {
            C.lst = C.lst.filter(function (x) { return HRM.bool(x.IsOnDuty); });
            C.reindex();
        };

        /** The rows Insert() hands genDutyRoaster.Save as GenDutyRoasterDetailsList (before the off days are added). */
        C.details = function () {
            return C.lst.map(function (x) {
                return {
                    line: x.DutyRoasterDetailLineId, employeeId: HRM.int(x.EmployeeId), employeeHistoryId: HRM.int(x.EmployeeHistoryId),
                    shiftId: HRM.int(x.ShiftId), dutyDate: x.DutyDate, weekDayName: x.WeekDayName, employeeGroupId: HRM.int(x.EmployeeGroupId),
                    dutyRoasterDetailId: HRM.int(x.DutyRoasterDetailId), dutyRoasterId: HRM.int(x.DutyRoasterId)
                };
            });
        };
        /** grdGroup.GetRows(): EmployeeGroupId, ShiftId, WeekDayName (the server keeps the rows where all three are > 0). */
        C.groups = function () {
            return (C.gGroup ? C.gGroup.rows() : []).map(function (r) {
                return { employeeGroupId: HRM.int(r.EmployeeGroupId), shiftId: HRM.int(r.ShiftId), weekDayId: HRM.int(r.WeekDayName) };
            });
        };

        /**
         * datagridHistory_DoubleClick's group loop, as written: for every saved group, EVERY grdGroup row takes
         * that group's EmployeeGroupId / ShiftId / WeekDayId (so all rows end with the last group).
         */
        C.applySavedGroups = function (groups) {
            if (!groups || !groups.length || !C.gGroup) return;
            groups.forEach(function (g) {
                C.gGroup.rows().forEach(function (r) {
                    r.EmployeeGroupId = HRM.int(g.EmployeeGroupId);
                    r.ShiftId = HRM.int(g.ShiftId);
                    r.WeekDayName = HRM.int(g.WeekDayId);
                });
            });
            C.gGroup.draw();                                                // grdGroup.UpdateData()
        };

        /** TotalDaysDiff = Conversion.ToInt((txtTodate.Value - txtFromDate.Value).TotalDays) - the pickers keep their time of day. */
        C.daysDiff = function (fromFrac, toFrac) {
            var f = HRM.val('txtFromDate'), t = HRM.val('txtTodate');
            if (!f || !t) return 0;
            return toInt32(HRM.daysBetween(f, t) + toFrac - fromFrac);
        };

        // grdDutyDate Click / grdEmployee Click (the wrappers stay, the tables are rebuilt on every load)
        var wd = HRM.$('wrapDutyDate');
        if (wd) wd.addEventListener('click', function (e) { if (e.target.closest('tr[data-i]')) C.dutyDateClick(); });
        var we = HRM.$('wrapEmployee');
        if (we) we.addEventListener('click', function (e) {
            if (!e.target.closest('tr[data-i]')) return;
            setTimeout(C.employeeClick, 0);                                  // after the check box's change reached the row (grdEmployee.UpdateData)
        });
    }

    /** History dialog detail grid (datagridhistoryDetail) under the History grid. */
    function detailBox(body, caption) {
        body.classList.add('ro-hist');
        var div = document.createElement('div');
        div.innerHTML = '<div class="hrm-subcaption"><span>' + HRM.esc(caption) + '</span></div>' +
            '<div class="hrm-grid-box hrm-grid-box-modal"><button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button>' +
            '<div class="hrm-grid-wrap"><table class="ro-hist-detail"></table></div></div>';
        while (div.firstChild) body.appendChild(div.firstChild);
        HRM.wireFullscreen(body);
        return new HRM.Grid(body.querySelector('.ro-hist-detail'), {
            columns: [
                { key: 'ShiftName', caption: 'ShiftName' }, { key: 'ShiftTiming', caption: 'ShiftTiming' },
                { key: 'EmployeeType', caption: 'EmployeeType' }, { key: 'Designation', caption: 'Designation' },
                { key: 'Department', caption: 'Department' }, { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'int' },
                { key: 'EmployeeName', caption: 'EmployeeName' }, { key: 'CNIC', caption: 'CNIC' }, { key: 'Mobile', caption: 'Mobile' },
                { key: 'Location', caption: 'Location' }, { key: 'WeekDay', caption: 'WeekDay' }, { key: 'DutyDate', caption: 'DutyDate', type: 'date' },
                { key: 'IsOffDuty', caption: 'IsOffDuty' }, { key: 'IsOnDuty', caption: 'IsOnDuty' }
            ],
            filterRow: true, emptyText: ''
        });
    }
    function cellButton(act, text) { return function () { return '<button type="button" class="ro-cell-btn" data-act="' + act + '">' + text + '</button>'; }; }

    // ============================================================================ 656 Duty Roaster
    function dutyRoaster() {
        var RecId = 0, rights = {}, setup = {};
        var shifts = [], week = [], departments = [];
        var fromFrac = 0, toFrac = dayFrac(new Date());                   // txtTodate keeps the time the form opened; txtFromDate = 1st of month 00:00
        var C = new RosterCore({ shifts: function () { return shifts; }, week: function () { return week; } });
        var histDlg = null;
        var H = { fromChecked: true, fromDate: HRM.today(), toChecked: true, toDate: HRM.today(), mode: 'doc', rosterFrom: '', rosterTo: '', rows: null, detail: null };

        // ---------------------------------------------------------------- cmbDepartment (UltraCombo with CheckedListSettings)
        var deptChecked = {};
        function deptNames() {                                            // cmbDepartment.Text (ListSeparator ",")
            return departments.filter(function (d) { return deptChecked[String(d.DepartmentId)]; }).map(function (d) { return HRM.str(d.DepartmentName); }).join(',');
        }
        function deptShow() { HRM.setVal('cmbDepartmentText', deptNames()); }
        function deptDraw() {
            var q = (HRM.val('cmbDepartmentFind') || '').toLowerCase();
            var html = departments.filter(function (d) { return !q || HRM.str(d.DepartmentName).toLowerCase().indexOf(q) >= 0; }).map(function (d) {
                var id = HRM.str(d.DepartmentId);
                return '<label class="ro-cc-item"><input type="checkbox" data-id="' + HRM.esc(id) + '"' + (deptChecked[id] ? ' checked' : '') + '> ' + HRM.esc(d.DepartmentName) + '</label>';
            }).join('');
            HRM.$('cmbDepartmentList').innerHTML = html || '<div class="ro-cc-empty">No record</div>';
            var all = HRM.$('cmbDepartmentAll');
            all.checked = departments.length > 0 && departments.every(function (d) { return deptChecked[String(d.DepartmentId)]; });
        }
        function deptOpen(on) {
            var pop = HRM.$('cmbDepartmentPop'), was = !pop.classList.contains('is-hidden');
            pop.classList.toggle('is-hidden', !on);
            if (on) { HRM.setVal('cmbDepartmentFind', ''); deptDraw(); HRM.$('cmbDepartmentFind').focus(); }
            else if (was) EmployeeName();                                 // cmbDepartment_Leave
        }
        /** DepartmentFill: Text = "", DataSource = null, then BindDDLNew with the "Selected" check column. */
        function DepartmentFill(rows) {
            deptChecked = {};
            departments = rows && rows.length ? rows : [];
            deptShow();
        }
        /** cmbDepartment.Value = DepartmentId (history): that department alone is ticked. */
        function deptSetValue(id) {
            deptChecked = {};
            if (HRM.int(id) > 0 && departments.some(function (d) { return HRM.int(d.DepartmentId) === HRM.int(id); })) deptChecked[String(HRM.int(id))] = true;
            deptShow();
        }
        /** The DepartmentIds string of EmployeeName / MannualDataSetInDutyRoaster: DataTable.Select("DepartmentName='x'") for each name of the text. */
        function departmentIds() {
            var text = deptNames();
            if (text === '') return null;
            var ids = '';
            (text + ',').split(',').forEach(function (name) {
                var n = name.toLowerCase();
                for (var i = 0; i < departments.length; i++) {
                    if (HRM.str(departments[i].DepartmentName).toLowerCase() === n) { ids += ',' + HRM.str(departments[i].DepartmentId); break; }
                }
            });
            return ids;
        }
        (function wireDept() {
            var box = HRM.$('cmbDepartment');
            HRM.$('cmbDepartmentText').addEventListener('click', function () { deptOpen(HRM.$('cmbDepartmentPop').classList.contains('is-hidden')); });
            HRM.$('cmbDepartmentText').addEventListener('keydown', function (e) {
                if (e.key === 'ArrowDown' || e.key === 'F4' || e.key === ' ') { e.preventDefault(); deptOpen(true); }
            });
            HRM.$('cmbDepartmentFind').addEventListener('input', deptDraw);
            HRM.$('cmbDepartmentList').addEventListener('change', function (e) {
                var id = e.target.getAttribute('data-id'); if (id === null) return;
                if (e.target.checked) deptChecked[id] = true; else delete deptChecked[id];
                deptShow();
                HRM.$('cmbDepartmentAll').checked = departments.length > 0 && departments.every(function (d) { return deptChecked[String(d.DepartmentId)]; });
            });
            HRM.$('cmbDepartmentAll').addEventListener('change', function (e) {          // header check box of the "Selected" column
                deptChecked = {};
                if (e.target.checked) departments.forEach(function (d) { deptChecked[String(d.DepartmentId)] = true; });
                deptShow();
                HRM.$('cmbDepartmentList').querySelectorAll('input[data-id]').forEach(function (x) { x.checked = !!deptChecked[x.getAttribute('data-id')]; });
            });
            document.addEventListener('mousedown', function (e) { if (!box.contains(e.target)) deptOpen(false); });
            box.addEventListener('keydown', function (e) { if (e.key === 'Escape' && !HRM.$('cmbDepartmentPop').classList.contains('is-hidden')) { e.stopPropagation(); e.preventDefault(); deptOpen(false); HRM.$('cmbDepartmentText').focus(); } }, true);
            box.addEventListener('focusout', function () { setTimeout(function () { if (!box.contains(document.activeElement)) deptOpen(false); }, 0); });
        })();

        // ---------------------------------------------------------------- fills
        function fillIf(id, rows, v, t) {                                 // BindDDLNew(..., false) only when dt.Rows.Count > 0
            if (rows && rows.length) HRM.fill(id, rows, v, t, { zero: '', keep: true });
        }
        function ShiftFill(rows) { if (rows && rows.length) shifts = rows; if (C.gGroup) C.gGroup.draw(); }
        /** EmployeeName(): active employees of the Section / Location / Department filter. */
        function EmployeeName() {
            return HRM.get(API + '/employees', {
                sectionId: HRM.comboVal('cmbSection'), locationId: HRM.comboVal('CmbLocation'), departmentIds: departmentIds() || undefined
            }).then(function (rows) { fillIf('CmbEmployeeName', rows, 'EmployeeId', 'EmployeeName'); }).catch(HRM.fail);
        }
        function fillLists(d) {
            fillIf('cmbEmployeeCategory', d.categories, 'EmployeeCategoryId', 'EmployeeCategoryName');
            DepartmentFill(d.departments);
            fillIf('cmbSection', d.sections, 'SectionId', 'SectionName');
            ShiftFill(d.shifts);
        }

        function buttons(save, update, saveAs) {
            HRM.show('btnsave', save); HRM.show('btnupdate', update); HRM.show('btnSaveAs', saveAs);
        }

        /** MannualDataSetInDutyRoaster */
        function filter() {
            return {
                fromDate: HRM.val('txtFromDate'), toDate: HRM.val('txtTodate'),
                employeeId: HRM.comboVal('CmbEmployeeName'), sectionId: HRM.comboVal('cmbSection'),
                employeeCategoryId: HRM.comboVal('cmbEmployeeCategory'), locationId: HRM.comboVal('CmbLocation'),
                dutyRoasterId: RecId, departmentIds: departmentIds()
            };
        }
        function MannualDataSetInDutyRoaster() {
            var f = filter();
            C.lst = []; C.idx = {};                                         // lstDutyRoasterDetail = new List<...>()
            if (!f.fromDate || !f.toDate) { HRM.box('Select the From Date and the To Date.'); return Promise.resolve(); }
            return HRM.loading(HRM.post(API + '/load', f)).then(function (d) { C.bind(d, f); }).catch(HRM.fail);
        }

        /** Reset() */
        function Reset() {
            RecId = 0;
            HRM.setCombo('cmbEmployeeCategory', 0);
            deptChecked = {}; deptShow();
            HRM.setCombo('cmbSection', 0);
            HRM.setVal('txtDescription', '');
            HRM.focus('txtFromDate');
            buttons(true, false, false);
            HRM.setCombo('CmbEmployeeName', 0);
            C.clear();
        }

        /** Insert() */
        function Insert(btn) {
            if (RecId > 0) { if (!HRM.ask('Are you sure to Update?')) return; }
            else if (!HRM.ask('Are you sure to Save?')) return;
            C.prune();
            var diff = C.daysDiff(fromFrac, toFrac);
            if (diff === 0 || diff < 0) {
                return HRM.busy(btn, function () {
                    return MannualDataSetInDutyRoaster().then(function () { HRM.box('DutyRoasterDetail not found Please Check'); });
                }, 'roster-save');
            }
            var id = RecId;
            var body = {
                recId: id, fromDate: HRM.val('txtFromDate'), toDate: HRM.val('txtTodate'), description: HRM.val('txtDescription'),
                employeeCategoryId: HRM.comboVal('cmbEmployeeCategory'), sectionId: HRM.comboVal('cmbSection'), locationId: HRM.comboVal('CmbLocation'),
                load: C.loadFilter, details: C.details(), groups: C.groups()
            };
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d && d.message ? d.message : (id > 0 ? 'Record Update' : 'Record Saved'));
                    Reset();
                }).catch(HRM.fail);
            }, 'roster-save');
        }

        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };           // btnsave_Click
        P.btnSaveAs = function (btn) { RecId = 0; return Insert(btn || 'btnSaveAs'); };       // btnSaveAs_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };                  // btnupdate_Click
        P.btnnew = function () { Reset(); };                                                  // btnnew_Click
        P.btnLoad = function (btn) { return HRM.busy(btn || 'btnLoad', MannualDataSetInDutyRoaster); };   // btnLoad_Click
        P.btnApply = function () { C.apply(); };                                              // btnApply_Click
        P.btnApplyAll = function () { C.applyAll(); };                                        // btnApplyAll_Click
        P.btnRefresh = function (btn) {                                                       // btnRefresh_Click
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/lists').then(function (d) {
                    fillLists(d);
                    return EmployeeName().then(function () { fillIf('CmbLocation', d.locations, 'LocationId', 'LocationName'); });
                }).catch(HRM.fail);
            });
        };
        P.btnEmployeeCategory = function () { HRM.open('/hrm/employee-category'); };          // new frmEmployeeCategory().Show()
        P.btnDepartment = function () { HRM.open('/hrm/department'); };                      // new genDepartment().Show()
        P.btnSection = function () { HRM.open('/hrm/section'); };                            // new DefineSection().Show()
        P.btnLocation = function () { HRM.open('/hrm/location'); };                          // new frmgenLocation().Show()
        P.btnEmployeeDefine = function () {                                                   // frmEmployeeRegistration, View right "EmployeeRegistration"
            if (setup.canEmployeeRegistration) HRM.open('/hrm/employee-registration');
            else HRM.box("You don't have the right to view this form...");
        };
        P.btnShifttimingorm = function () {                                                   // frmGenShiftTiming, View right "ShiftTiming"
            if (setup.canShiftTiming) HRM.open('/hrm/shift-timing');
            else HRM.box("You don't have the right to view this form...");
        };

        // cmbEmployeeCategory / cmbSection / CmbLocation _Leave -> EmployeeName()
        ['cmbEmployeeCategory', 'cmbSection', 'CmbLocation'].forEach(function (id) {
            var e = HRM.$(id); if (e) e.addEventListener('change', function () { EmployeeName(); });
        });

        // ---------------------------------------------------------------- History tab (tabPage1) as a dialog
        /** datagridHistory_DoubleClick (saveAs: the SaveAs column button) */
        function historyLoad(r, saveAs) {
            buttons(false, true, false);
            RecId = HRM.int(HRM.col(r, 'DutyRoaster'));
            return HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (o) {
                if (!o) return;
                HRM.setVal('txtFromDate', HRM.day(o.FromDate));
                HRM.setVal('txtTodate', HRM.day(o.ToDate));
                fromFrac = 0; toFrac = 0;
                HRM.setVal('txtDescription', HRM.str(o.Description));
                HRM.setCombo('cmbEmployeeCategory', HRM.int(o.EmployeeCategoryId));
                deptSetValue(o.DepartmentId);
                HRM.setCombo('cmbSection', HRM.int(o.SectionId));
                HRM.setCombo('CmbLocation', HRM.int(o.LocationId));
                return MannualDataSetInDutyRoaster().then(function () { C.applySavedGroups(o.groups); });
            }).catch(HRM.fail).then(function () {
                if (saveAs) buttons(false, false, true);                    // datagridHistory_ColumnButtonClick "SaveAs"
            });
        }
        function historyFilters() {
            return '<div class="hrm-field"><label class="hrm-check"><input type="checkbox" id="FromDateHistoryChk"> From Date</label><input type="date" id="FromDateHistory" class="win-textbox"></div>' +
                '<div class="hrm-field"><label class="hrm-check"><input type="checkbox" id="ToDateHistoryChk"> To Date</label><input type="date" id="ToDateHistory" class="win-textbox"></div>' +
                '<div class="hrm-field"><label for="txtFromDocNoHistory">Roaster From</label><input type="text" id="txtFromDocNoHistory" class="win-textbox" inputmode="numeric" autocomplete="off"></div>' +
                '<div class="hrm-field"><label for="txtToDocNoHistory">Roaster To</label><input type="text" id="txtToDocNoHistory" class="win-textbox" inputmode="numeric" autocomplete="off"></div>' +
                '<span class="ro-radios"><label class="hrm-check"><input type="radio" name="histMode" value="doc" id="drdocdate"> Duter Roaster Dates</label>' +
                '<label class="hrm-check"><input type="radio" name="histMode" value="entry" id="rdentrydate"> Entry Date</label>' +
                '<label class="hrm-check"><input type="radio" name="histMode" value="modify" id="rdmodifydate"> Modify Date</label></span>' +
                '<button type="button" class="win-btn-action" id="btnNewHistory">Reset</button>';
        }
        function readH(body) {
            H.fromChecked = body.querySelector('#FromDateHistoryChk').checked; H.fromDate = body.querySelector('#FromDateHistory').value;
            H.toChecked = body.querySelector('#ToDateHistoryChk').checked; H.toDate = body.querySelector('#ToDateHistory').value;
            H.rosterFrom = body.querySelector('#txtFromDocNoHistory').value; H.rosterTo = body.querySelector('#txtToDocNoHistory').value;
            var m = body.querySelector('input[name=histMode]:checked'); H.mode = m ? m.value : 'doc';
        }
        function writeH(body) {
            body.querySelector('#FromDateHistoryChk').checked = H.fromChecked; body.querySelector('#FromDateHistory').value = H.fromDate;
            body.querySelector('#ToDateHistoryChk').checked = H.toChecked; body.querySelector('#ToDateHistory').value = H.toDate;
            body.querySelector('#txtFromDocNoHistory').value = H.rosterFrom; body.querySelector('#txtToDocNoHistory').value = H.rosterTo;
            var m = body.querySelector('input[name=histMode][value="' + H.mode + '"]'); if (m) m.checked = true;
        }
        function openHistory() {
            var auto = true, dg = null;
            histDlg = HRM.history({
                title: 'Duty Roaster History', filters: historyFilters(),
                columns: [
                    { key: 'Edit', caption: 'Edit', width: 50, render: cellButton('edit', 'Edit') },
                    { key: 'SaveAs', caption: 'SaveAs', width: 65, render: cellButton('saveas', 'SaveAs') },
                    { key: 'DutyRoaster', caption: 'DutyRoaster', type: 'code' },
                    { key: 'ShiftTimingName', caption: 'ShiftTimingName' },
                    { key: 'FromDate', caption: 'FromDate', type: 'date' },
                    { key: 'ToDate', caption: 'ToDate', type: 'date' },
                    { key: 'Remarks', caption: 'Remarks' },
                    { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(fmt12(v)); } },
                    { key: 'EntryUserName', caption: 'EntryUserName' },
                    { key: 'ModifyDate', caption: 'ModifyDate', render: function (v) { return HRM.esc(fmt12(v)); } },
                    { key: 'ModifyUserName', caption: 'ModifyUserName' }
                ],
                load: function (body) {                                    // btnshowHistory_Click -> HistoryGridFill
                    readH(body);
                    if (auto) { auto = false; return Promise.resolve(H.rows || []); }   // the tab shows what it showed last
                    return HRM.post(API + '/history', {
                        fromChecked: H.fromChecked, fromDate: H.fromDate, toChecked: H.toChecked, toDate: H.toDate,
                        mode: H.mode, rosterFrom: H.rosterFrom, rosterTo: H.rosterTo
                    }).then(function (rows) { H.rows = rows || []; return H.rows; });
                },
                onPick: function (r) { historyLoad(r, false); },          // datagridHistory_DoubleClick / the DutyRoaster link
                onReady: function (body, g) {
                    writeH(body);
                    body.querySelector('.hrm-history-refresh').textContent = 'Show';
                    onlyDigits(body.querySelector('#txtFromDocNoHistory'));
                    onlyDigits(body.querySelector('#txtToDocNoHistory'));
                    body.querySelector('#FromDateHistory').focus();        // tabControl1_SelectedIndexChanged
                    dg = detailBox(body, 'This Grid Show The Detail Of seleced Row of Main grid');
                    if (H.detail) dg.set(H.detail);
                    body.querySelector('#btnNewHistory').addEventListener('click', function () {     // btnNewHistory_Click
                        body.querySelector('#FromDateHistory').value = HRM.addDays(HRM.today(), -30);
                        body.querySelector('#ToDateHistory').value = HRM.today();
                        body.querySelector('#txtFromDocNoHistory').value = '';
                        body.querySelector('#txtToDocNoHistory').value = '';
                        readH(body);
                        g.set([]); dg.set([]); H.rows = null; H.detail = null;
                    });
                    g.table.addEventListener('click', function (e) {
                        var tr = e.target.closest('tr[data-i]'); if (!tr) return;
                        var r = g.data[+tr.getAttribute('data-i')]; if (!r) return;
                        var b = e.target.closest('button[data-act]');
                        if (b) {                                             // datagridHistory_ColumnButtonClick
                            histDlg.modal.close();
                            historyLoad(r, b.getAttribute('data-act') === 'saveas');
                            return;
                        }
                        HRM.loading(HRM.get(API + '/history-detail', { id: HRM.int(r.DutyRoaster) })).then(function (rows) {   // datagridHistory_Click
                            H.detail = rows || []; dg.set(H.detail);
                        }).catch(HRM.fail);
                    });
                }
            });
            return histDlg;
        }
        function historyOpen() { return histDlg && document.body.contains(histDlg.modal.el); }
        function toggleHistory() { if (historyOpen()) histDlg.modal.close(); else openHistory(); }     // Ctrl+T switches the tab
        HRM.footer(function () { if (!historyOpen()) openHistory(); });
        document.addEventListener('keydown', function (e) {
            if (e.ctrlKey && (e.key || '').toLowerCase() === 't' && historyOpen()) { e.preventDefault(); e.stopImmediatePropagation(); toggleHistory(); }
        }, true);

        // frmDutyRoasterNew_KeyDown
        HRM.keys({
            'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': toggleHistory,
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled) P.btnupdate(); }
        });

        // frmDutyRoaster_Load
        HRM.setVal('txtFromDate', HRM.firstOfMonth());
        HRM.setVal('txtTodate', HRM.today());
        buttons(true, false, false);
        C.setLbl('');
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {
            setup = d || {};
            rights = setup.rights || {};
            HRM.applyRights(rights, { save: ['btnsave'], update: ['btnupdate'] });
            fillLists(setup);
            week = setup.weekProfiles || [];
            fillIf('CmbEmployeeName', setup.employees, 'EmployeeId', 'EmployeeName');
            fillIf('CmbLocation', setup.locations, 'LocationId', 'LocationName');
            HRM.focus('txtFromDate');
        }).catch(HRM.fail);
    }

    // ============================================================================ 657 Employee Roaster
    function employeeRoaster() {
        var RecId = 0, rights = {}, setup = {};
        var shifts = [], week = [];
        var openFrac = dayFrac(new Date()), fromFrac = openFrac, toFrac = openFrac;   // both pickers keep the time the form opened
        var C = new RosterCore({ shifts: function () { return shifts; }, week: function () { return week; } });

        function fillEmployees(rows) { if (rows && rows.length) HRM.fill('CmbEmployeeName', rows, 'EmployeeId', 'EmployeeName', { zero: '', keep: true }); }
        function ShiftFill(rows) { if (rows && rows.length) shifts = rows; if (C.gGroup) C.gGroup.draw(); }

        function filter() {
            return { fromDate: HRM.val('txtFromDate'), toDate: HRM.val('txtTodate'), employeeId: HRM.comboVal('CmbEmployeeName'), dutyRoasterId: RecId };
        }
        /** MannualDataSetInDutyRoaster (frmEmployeeRoaster): dates, EmployeeId and DutyRoasterId only */
        function MannualDataSetInDutyRoaster() {
            var f = filter();
            C.lst = []; C.idx = {};
            if (!f.fromDate || !f.toDate) { HRM.box('Select the From Date and the To Date.'); return Promise.resolve(); }
            return HRM.loading(HRM.post(API + '/load', f)).then(function (d) { C.bind(d, f); }).catch(HRM.fail);
        }

        /** Reset() */
        function Reset() {
            RecId = 0;
            HRM.setVal('txtDescription', '');
            HRM.focus('txtFromDate');
            HRM.show('btnupdate', false); HRM.show('btnSaveAs', false);
            HRM.setCombo('CmbEmployeeName', 0);
            C.clear();
        }

        /** Insert() - an update of the roster picked from History only */
        function Insert(btn) {
            if (RecId > 0) {
                if (!HRM.ask('Are you sure to Update?')) return;
                C.prune();
                var diff = C.daysDiff(fromFrac, toFrac);
                if (diff === 0 || diff < 0) {
                    return HRM.busy(btn, function () {
                        return MannualDataSetInDutyRoaster().then(function () { HRM.box('DutyRoasterDetail not found Please Check'); });
                    }, 'roster-save');
                }
                var body = {
                    recId: RecId, fromDate: HRM.val('txtFromDate'), toDate: HRM.val('txtTodate'), description: HRM.val('txtDescription'),
                    employeeCategoryId: 0, sectionId: 0, locationId: 0,
                    load: C.loadFilter, details: C.details(), groups: C.groups()
                };
                return HRM.busy(btn, function () {
                    return HRM.post(API + '/save', body).then(function (d) {
                        HRM.box(d && d.message ? d.message : 'Record Update');
                        Reset();
                    }).catch(HRM.fail);
                }, 'roster-save');
            }
            return HRM.busy(btn, function () {
                return MannualDataSetInDutyRoaster().then(function () { HRM.box('RecId Not Found'); });
            }, 'roster-save');
        }

        P.btnsave = function (btn) { return Insert(btn || 'btnsave'); };          // btnsave_Click (the button is never shown)
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };      // btnupdate_Click
        P.btnSaveAs = function () { };                                            // btnSaveAs_Click is empty on the desktop
        P.btnnew = function () { Reset(); };                                      // btnnew_Click
        P.btnRefresh = function (btn) {                                           // btnRefresh_Click: ShiftFill, EmployeeName
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/lists').then(function (d) { ShiftFill(d.shifts); fillEmployees(d.employees); }).catch(HRM.fail);
            });
        };
        P.btnLoad = function (btn) {                                              // btnLoad_Click
            if (RecId > 0) {
                if (HRM.comboVal('CmbEmployeeName') === 0) { HRM.box('Please Select Employee...'); HRM.focus('CmbEmployeeName'); return; }
                HRM.show('btnupdate', true);
                return HRM.busy(btn || 'btnLoad', MannualDataSetInDutyRoaster);
            }
            HRM.box('RecId Not Found Please GetMaster Record');
        };
        P.btnApply = function () { C.apply(); };                                  // btnApply_Click
        P.btnApplyAll = function () { C.applyAll(); };                            // btnApplyAll_Click
        P.btnEmployeeDefine = function () {                                       // frmEmployeeRegistration, View right "EmployeeRegistration"
            if (setup.canEmployeeRegistration) HRM.open('/hrm/employee-registration');
            else HRM.box("You don't have the right to view this form...");
        };

        // ---------------------------------------------------------------- History tab (tabPage1) as a dialog
        /** datagridHistory_DoubleClick */
        function historyLoad(r) {
            RecId = HRM.int(HRM.col(r, 'Id'));
            return HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (o) {
                if (!o) return;
                HRM.setVal('txtFromDate', HRM.day(o.FromDate));
                HRM.setVal('txtTodate', HRM.day(o.ToDate));
                fromFrac = 0; toFrac = 0;
                HRM.setVal('txtDescription', HRM.str(o.Description));
                C.applySavedGroups(o.groups);
                fillEmployees(o.employees);                                   // GetEmployeesByDutyRosterId -> CmbEmployeeName
            }).catch(HRM.fail);
        }
        function openHistory() {
            var dg = null, lastSel = -1;
            var dlg = HRM.history({
                title: 'Employee Duty Roaster History',
                columns: [
                    { key: 'Edit', caption: 'Edit', width: 50, render: cellButton('edit', 'Edit') },
                    { key: 'Id', caption: 'Id', type: 'code' },
                    { key: 'ShiftTimingName', caption: 'ShiftTimingName' },
                    { key: 'FromDate', caption: 'FromDate', type: 'date' },
                    { key: 'ToDate', caption: 'ToDate', type: 'date' },
                    { key: 'Remarks', caption: 'Remarks' }
                ],
                load: function () { return HRM.get(API + '/history'); },     // tabControl1_SelectedIndexChanged -> HistoryGridFill
                onPick: function (r) { historyLoad(r); },
                onReady: function (body, g) {
                    dg = detailBox(body, 'Employee Duty Roaster History');
                    g.opts.onSelect = function (r, i) {                        // datagridHistory SelectionChanged -> datagridHistory_Click
                        if (i === lastSel) return; lastSel = i;
                        HRM.loading(HRM.get(API + '/history-detail', { id: HRM.int(r.Id) })).then(function (rows) { dg.set(rows || []); }).catch(HRM.fail);
                    };
                    g.table.addEventListener('click', function (e) {
                        var b = e.target.closest('button[data-act]'); if (!b) return;
                        var tr = e.target.closest('tr[data-i]'); if (!tr) return;
                        var r = g.data[+tr.getAttribute('data-i')]; if (!r) return;
                        dlg.modal.close();                                    // datagridHistory_ColumnButtonClick "Edit"
                        historyLoad(r);
                    });
                }
            });
            return dlg;
        }
        HRM.footer(function () { if (!document.querySelector('.hrm-modal')) openHistory(); });
        HRM.keys({ enterTab: false });                                            // the form has no KeyDown handler

        // frmDutyRoaster_Load (frmEmployeeRoaster)
        HRM.setVal('txtFromDate', HRM.today());
        HRM.setVal('txtTodate', HRM.today());
        HRM.show('btnupdate', false); HRM.show('btnsave', false); HRM.show('btnSaveAs', false);
        C.setLbl('');
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {
            setup = d || {};
            rights = setup.rights || {};
            HRM.applyRights(rights, { save: ['btnsave'], update: ['btnupdate'] });
            ShiftFill(setup.shifts);
            week = setup.weekProfiles || [];
            fillEmployees(setup.employees);
            HRM.focus('txtFromDate');
        }).catch(HRM.fail);
    }

    var PAGES = { 'duty-roaster': dutyRoaster, 'employee-roaster': employeeRoaster };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
