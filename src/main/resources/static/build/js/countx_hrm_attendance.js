/* ============================================================================================
 * countx_hrm_attendance.js - HRM "Attendance Management" (AppModules 2022). One script, the page
 * is chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   daily-attendance    frmDailyAttendance.cs  (658)
 *   manual-attendance   hrmManualAttendance.cs (659)
 *
 * Each handler follows its desktop form line by line: validation wording and order, messages,
 * what Load / Generate / Save / Update / New / Refresh do to the combos and the grid, and the
 * grid's CellUpdated rules (Auto entries, CPL against rest day, IsAbsent on rest day).
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/attendance/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmAttendance
    window.HrmAttendance = P;

    // ============================================================================ shared pieces

    /** Nothing selected: BindDDL / BindDDLNew with ZeroIndex false leave the UltraCombo empty. */
    var EMPTY = { zero: '' };

    /**
     * UltraCombo.Leave: focus leaving the combo altogether (its searchable wrap and popup included).
     * The desktop wires Leave on the seven filter combos to EmployeeName().
     */
    function onLeave(ids, fn) {
        document.addEventListener('focusout', function (e) {
            var t = e.target;
            if (!t || !t.closest) return;
            var wrap = t.closest('.dtcombo-wrap');
            var sel = wrap ? wrap.querySelector('select') : (t.tagName === 'SELECT' ? t : null);
            if (!sel || ids.indexOf(sel.id) < 0) return;
            if (wrap && e.relatedTarget && wrap.contains(e.relatedTarget)) return;
            setTimeout(function () {
                if (wrap && wrap.contains(document.activeElement)) return;
                fn(sel.id);
            }, 0);
        }, true);
    }

    /** The DateTimePicker with ShowCheckBox: the check box is .Checked, the time box is .Value ("HH:mm"). */
    function wireTimeCheck(chk, txt) {
        var c = HRM.$(chk);
        if (!c) return;
        var sync = function () { HRM.enable(txt, c.checked); };
        c.addEventListener('change', sync);
        HRM.setVal(txt, HRM.nowTime());                       // DateTimePicker default Value = DateTime.Now
        sync();
    }

    function t(v) { return HRM.str(v).trim(); }

    /** A grid row as AttendanceRowDto (the cells btnsave_Click / btnupdate_Click read). */
    function rowDto(r) {
        return {
            manualAttendanceId: HRM.int(r.ManualAttendanceId),
            employeeId: HRM.int(r.EmployeeId),
            locationId: HRM.int(r.LocationId),
            departmentId: HRM.int(r.DepartmentId),
            shiftId: HRM.int(r.ShiftId),
            dutyDate: HRM.str(r.DutyDate),
            inTime: HRM.str(r.InTime),
            outTime: HRM.str(r.OutTime),
            inEntryMode: HRM.str(r.InEntryMode),
            outEntryMode: HRM.str(r.OutEntryMode),
            status: HRM.str(r.Status),
            description: HRM.str(r.Description),
            cplQuota: HRM.str(r.CPLQuota),
            isAbsent: HRM.bool(r.IsAbsent)
        };
    }

    /** "29-Sep-2026" -> sortable number (the DutyDate sort key of the manual form). */
    var MON = { jan: 0, feb: 1, mar: 2, apr: 3, may: 4, jun: 5, jul: 6, aug: 7, sep: 8, oct: 9, nov: 10, dec: 11 };
    function dutyKey(s) {
        var m = String(s || '').match(/^(\d{1,2})-([A-Za-z]{3})-(\d{4})/);
        if (!m) { var d = HRM.day(s); return d ? +d.replace(/-/g, '') : 0; }
        return (+m[3]) * 10000 + ((MON[m[2].toLowerCase()] || 0) + 1) * 100 + (+m[1]);
    }

    /** Grid columns shared by both forms (keys = the form's dtgrid columns). */
    function col(key, o) { return Object.assign({ key: key, caption: key }, o || {}); }

    /**
     * grd_CellUpdated - the rules both forms share for InTime / OutTime: an Auto ("A") entry cannot be
     * changed ("You cannot change Auto Attendance Please Check"); otherwise the entry becomes Manual
     * ("M"), FlagIn/OutTime = 1 and IsAbsent = 0. Returns false when the edit was refused.
     */
    function timeEdited(grd, r, k, before) {
        var modeKey = k === 'InTime' ? 'InEntryMode' : 'OutEntryMode';
        var flagKey = k === 'InTime' ? 'FlagInTime' : 'FlagOutTime';
        if (!(t(r[modeKey]) !== 'A')) {
            // The desktop keeps the typed value in the cell (UpdateData runs first) but the procedure
            // discards an Auto entry's time anyway; the cell is put back so the grid shows what is saved.
            r[k] = before;
            grd.draw();
            HRM.box('You cannot change Auto Attendance Please Check');
            return false;
        }
        r[modeKey] = 'M';
        r[flagKey] = 1;
        r.IsAbsent = false;
        return true;
    }

    /**
     * btnGenerate_Click (both forms): for every row of the grid's view, when the picker is checked and
     * the entry is not Auto, the picker's time goes into the cell, the entry becomes "M", the flag 1 and
     * IsAbsent 0. (658 formats the time as "HH:mm tt", which fails to parse for an afternoon time on the
     * desktop; the working behaviour - the picked time - is kept.)
     */
    function generate(grd) {
        var rows = grd.visibleRows();
        if (rows.length > 0) {
            rows.forEach(function (r) {
                if (HRM.checked('chkInTime') && t(r.InEntryMode) !== 'A') {
                    r.InTime = HRM.val('txtInTime');
                    r.InEntryMode = 'M';
                    r.FlagInTime = 1;
                    r.IsAbsent = false;
                }
                if (HRM.checked('chkOutTime') && t(r.OutEntryMode) !== 'A') {
                    r.OutTime = HRM.val('txtOutTime');
                    r.OutEntryMode = 'M';
                    r.FlagOutTime = 1;
                    r.IsAbsent = false;
                }
            });
        }
        grd.draw();                                                          // grd.UpdateData()
    }

    /** Keeps the value each time cell had when it was loaded / last accepted (for a refused Auto edit). */
    function remember(rows) {
        (rows || []).forEach(function (r) { r._in = r.InTime; r._out = r.OutTime; });
        return rows;
    }

    function status(grd) {                                                   // RecordNavigator
        HRM.text('lblStatus', grd.rows().length ? ('Rows: ' + grd.visibleRows().length + ' of ' + grd.rows().length) : '');
    }

    // ============================================================================ 658 Daily Attendance
    function dailyAttendance() {
        var rights = {}, canEmployee = false, loaded = null, empSeq = 0;
        var FILTERS = ['cmbShift', 'CmbLocation', 'cmbDepartment', 'CmbDesignation', 'cmbSection', 'CmbEmployeeCategory', 'CmbEmployeeGroup'];

        // grdSetting(): the dtgrid columns in their order; Visible = false / EditType as set there.
        var grd = new HRM.Grid('grd', {
            columns: [
                col('ManualAttendanceId', { hidden: true }),
                col('Day', { width: 80 }),
                col('EmployeeNo', { width: 80 }),
                col('EmployeeId', { hidden: true }),
                col('LocationId', { hidden: true }),
                col('DepartmentId', { hidden: true }),
                col('ShiftId', { hidden: true }),
                col('EmployeeName', { width: 180 }),
                col('DutyDate', { width: 95 }),
                col('ShiftTiming', { width: 150 }),
                col('InTime', { type: 'edit-time', width: 110 }),              // EditType DropDown, "hh:mm tt"
                col('OutTime', { type: 'edit-time', width: 110 }),
                col('InEntryMode', { width: 80, align: 'center' }),
                col('OutEntryMode', { width: 85, align: 'center' }),
                col('Status', { hidden: true }),
                col('Description', { type: 'edit', width: 180 }),
                col('FlagInTime', { hidden: true }),
                col('FlagOutTime', { hidden: true }),
                col('IsPresent', { hidden: true }),
                col('IsLeave', { hidden: true }),
                col('IsHoliday', { hidden: true }),
                col('IsSuspended', { hidden: true }),
                col('IsTerminate', { hidden: true }),
                col('CPLQuota', { type: 'edit-num', width: 75 }),             // EditType TextBox
                col('IsRestDay', { type: 'check', width: 70 }),               // EditType NoEdit
                col('IsAbsent', { type: 'edit-check', width: 70 }),           // EditType CheckBox
                col('IsCPL', { type: 'edit-check', hidden: true })
            ],
            filterRow: true,                                                   // FilterMode Automatic
            emptyText: '',
            rowClass: function (r) {                                           // GridFormattingFuntion (the last colour set wins)
                if (HRM.bool(r.IsLeave)) return 'hrm-att-leave';
                if (HRM.bool(r.IsHoliday)) return 'hrm-att-holiday';
                if (HRM.bool(r.IsRestDay)) return 'hrm-att-rest';
                return '';
            },
            onChange: CellUpdated,
            onDraw: function () { status(grd); }
        });

        /** grd_CellUpdated (658). */
        function CellUpdated(r, k) {
            if (k === 'InTime' || k === 'OutTime') {
                var before = k === 'InTime' ? r._in : r._out;
                if (!timeEdited(grd, r, k, before)) return;
                if (k === 'InTime') r._in = r.InTime; else r._out = r.OutTime;
                grd.draw();
                return;
            }
            if (k === 'IsCPL') {                                               // column hidden by grdSetting - kept for completeness
                if (!HRM.bool(r.IsRestDay)) {
                    r.IsCPL = false; grd.draw();
                    HRM.box('CPL can only be Incremented against RestDay');
                    return;
                }
                if (!HRM.bool(r.IsCPL)) { r.CPLQuota = 0; grd.draw(); }
                return;
            }
            if (k === 'CPLQuota' && !HRM.bool(r.IsRestDay)) {
                r.CPLQuota = 0; r.IsCPL = false; grd.draw();
                HRM.box('CPL can only be Incremented against RestDay');
                return;
            }
            if (k === 'IsAbsent' && HRM.bool(r.IsRestDay)) {
                r.IsAbsent = false; grd.draw();
                HRM.box('You cannot Update IsAbsent because it is RestDay');
            }
        }

        /** The filter box as ReportsParameters (Conversion.ToInt(cmb.Value)). */
        function filter() {
            return {
                date: HRM.val('txtDate'),
                shiftId: HRM.comboVal('cmbShift'),
                sectionId: HRM.comboVal('cmbSection'),
                departmentId: HRM.comboVal('cmbDepartment'),
                designationId: HRM.comboVal('CmbDesignation'),
                locationId: HRM.comboVal('CmbLocation'),
                employeeGroupId: HRM.comboVal('CmbEmployeeGroup'),
                employeeCategoryId: HRM.comboVal('CmbEmployeeCategory'),
                employeeId: HRM.comboVal('cmbEmployee'),
                isRestDay: HRM.checked('ChkRestDay')
            };
        }

        /** ShiftNameComboFill .. EmployeeCategoryFill: BindDDLNew only when the table has rows. */
        function fillCombos(d, keep) {
            var o = { zero: '', keep: !!keep };
            if (d.shifts && d.shifts.length) HRM.fill('cmbShift', d.shifts, 'ShiftId', 'ShiftName', o);
            if (d.sections && d.sections.length) HRM.fill('cmbSection', d.sections, 'SectionId', 'SectionName', o);
            if (d.departments && d.departments.length) HRM.fill('cmbDepartment', d.departments, 'DepartmentId', 'DepartmentName', o);
            if (d.locations && d.locations.length) HRM.fill('CmbLocation', d.locations, 'LocationId', 'LocationName', o);
            if (d.designations && d.designations.length) HRM.fill('CmbDesignation', d.designations, 'DesignationId', 'DesignationName', o);
            if (d.employeeGroups && d.employeeGroups.length) HRM.fill('CmbEmployeeGroup', d.employeeGroups, 'EmployeeGroupId', 'EmployeeGroupName', o);
            if (d.employeeCategories && d.employeeCategories.length) HRM.fill('CmbEmployeeCategory', d.employeeCategories, 'EmployeeCategoryId', 'EmployeeCategoryName', o);
            if (d.employees) HRM.fill('cmbEmployee', d.employees, 'Id', 'EmployeeName', o);   // EmployeeName(): always bound
        }

        /** EmployeeName(): GetAllEmployeesActive with the filter values; the latest request wins. */
        function EmployeeName() {
            var seq = ++empSeq;
            return HRM.post(API + '/employees', filter()).then(function (rows) {
                if (seq === empSeq) HRM.fill('cmbEmployee', rows || [], 'Id', 'EmployeeName', { zero: '', keep: true });
            }).catch(HRM.fail);
        }

        /** grdFill(). */
        function grdFill() {
            var f = filter();
            return HRM.post(API + '/load', f).then(function (rows) {
                if (rows && rows.length) {
                    loaded = f;
                    grd.set(remember(rows));
                } else {
                    loaded = null;
                    grd.clear();                                               // grd.ClearStructure()
                    HRM.box('Record Not Found...');
                }
            }).catch(HRM.fail);
        }

        /** Reset(): date = now, every filter combo emptied, grid cleared. */
        function Reset() {
            HRM.setVal('txtDate', HRM.today());
            ['cmbDepartment', 'cmbSection', 'cmbShift', 'cmbEmployee', 'CmbDesignation', 'CmbLocation', 'CmbEmployeeCategory', 'CmbEmployeeGroup']
                .forEach(function (id) { HRM.setCombo(id, 0); });
            loaded = null;
            grd.clear();
        }

        P.btnLoad = function (btn) { return HRM.busy(btn || 'btnLoad', grdFill, 'daily-load'); };   // btnLoad_Click
        P.btnGenerate = function () { generate(grd); };                                             // btnGenerate_Click
        P.btnnew = function () { Reset(); };                                                        // btnnew_Click

        /** btnsave_Click. */
        P.btnsave = function (btn) {
            var b = HRM.$(btn || 'btnsave');
            if (b && b.disabled) return;
            var rows = grd.visibleRows();                                      // grd.GetRows()
            if (rows.length === 0) { HRM.box('Grid Record Not Found Please Check'); return; }
            if (!HRM.ask('Are you sure to Save?')) return;
            return HRM.busy(b, function () {
                return HRM.post(API + '/save', { loaded: loaded || filter(), details: rows.map(rowDto) }).then(function (d) {
                    HRM.box((d && d.message) || 'Saved Seccessfully');
                    Reset();
                }).catch(HRM.fail);
            }, 'daily-save');
        };

        /** btnRefresh_Click: every combo again, then EmployeeName(). */
        P.btnRefresh = function (btn) {
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.post(API + '/combos', filter()).then(function (d) { fillCombos(d || {}, true); }).catch(HRM.fail);
            }, 'daily-refresh');
        };

        /** btnPrint_Click: DailyAttendanceRpt ("Not Record Found For Display") -> 1003-DailyAttendance.rpt. */
        P.btnPrint = function (btn) {
            var b = HRM.$(btn || 'btnPrint');
            var f = filter();
            var args = {
                date: f.date, shiftId: f.shiftId, departmentId: f.departmentId, designationId: f.designationId,
                employeeId: f.employeeId, locationId: f.locationId, employeeCategoryId: f.employeeCategoryId, sectionId: f.sectionId
            };
            var CP = window.CrystalPrint;
            var w = CP && CP.reserve ? CP.reserve() : null;
            return HRM.busy(b, function () {
                return HRM.post(API + '/print-check', f).then(function () {
                    if (!CP) { if (w) w.close(); HRM.box('Report file 1003-DailyAttendance.rpt is not available.'); return; }
                    return CP.open('hrm-1003-658', args, null, w);
                }).catch(function (e) { if (CP && CP.release) CP.release(w); HRM.fail(e); });
            }, 'daily-print');
        };

        /** btnEmployeeDefine_Click: frmEmployeeRegistration.Show() when the user has View on it. */
        P.btnEmployeeDefine = function () {
            if (canEmployee) HRM.open('/hrm/employee-registration');
            else HRM.box('You Dont Have rights View Of This Form..');
        };

        onLeave(FILTERS, function () { EmployeeName(); });                 // cmbShift_Leave ... CmbEmployeeGroup_Leave
        wireTimeCheck('chkInTime', 'txtInTime');
        wireTimeCheck('chkOutTime', 'txtOutTime');

        HRM.keys({                                                            // frmDailyAttendance_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': function () { /* tabControl1 has one tab ("Form"): nothing to switch to */ },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+p': function () { P.btnPrint(); }
        });
        // The form has no history of its own: the Detail grid is it - History reloads it (grdFill) and shows it.
        HRM.footer(function (b) {
            return HRM.busy(b, function () {
                return grdFill().then(function () { var g = HRM.$('grdBox'); if (g && g.scrollIntoView) g.scrollIntoView({ block: 'nearest' }); });
            }, 'daily-load');
        });

        HRM.setVal('txtDate', HRM.today());                                   // DateTimePicker default = today
        HRM.fill('cmbEmployee', [], 'Id', 'EmployeeName', EMPTY);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {             // frmDailyAttendance_Load
            d = d || {};
            rights = d.rights || {};
            canEmployee = !!d.employeeRegistrationView;
            HRM.applyRights(rights, { save: ['btnsave'], print: ['btnPrint'] });   // btnsave.Enabled / btnPrint.Enabled
            fillCombos(d, false);
            grd.clear();
            HRM.focus('txtDate');
        }).catch(HRM.fail);
    }

    // ============================================================================ 659 Manual Attendance
    function manualAttendance() {
        var rights = {}, canEmployee = false, loaded = null, empSeq = 0, curMonth = new Date().getMonth() + 1, curYear = new Date().getFullYear();
        var FILTERS = ['cmbShift', 'CmbLocation', 'CmbDepartment', 'CmbSection', 'CmbDesignation', 'CmbEmployeeCategory', 'CmbEmployeeGroup'];

        // grdSetting(): the dtgrid columns in their order; Visible = false / EditType as set there.
        var grd = new HRM.Grid('grd', {
            columns: [
                col('ManualAttendanceId', { hidden: true }),
                col('Day', { width: 80 }),
                col('EmployeeNo', { width: 80 }),
                col('EmployeeId', { hidden: true }),
                col('LocationId', { hidden: true }),
                col('DepartmentId', { hidden: true }),
                col('ShiftId', { hidden: true }),
                col('Employee', { width: 180 }),
                col('DutyDate', { width: 95 }),
                col('ShiftTiming', { width: 150 }),
                col('InTime', { type: 'edit-time', width: 110 }),              // "hh:mm tt"
                col('OutTime', { type: 'edit-time', width: 110 }),
                col('InEntryMode', { width: 80, align: 'center' }),
                col('OutEntryMode', { width: 85, align: 'center' }),
                col('Status', { hidden: true }),
                col('Description', { type: 'edit', width: 180 }),             // no EditType set: editable
                col('FlagInTime', { hidden: true }),
                col('FlagOutTime', { hidden: true }),
                col('IsPresent', { hidden: true }),
                col('IsLeave', { hidden: true }),
                col('IsHoliday', { hidden: true }),
                col('IsRestDay', { hidden: true }),
                col('IsSuspended', { hidden: true }),
                col('IsTerminate', { hidden: true }),
                col('CPLQuota', { width: 75, align: 'right' }),               // EditType NoEdit
                col('IsAbsent', { type: 'edit-check', width: 70 })            // EditType CheckBox
            ],
            filterRow: true,
            emptyText: '',
            onChange: CellUpdated,
            onDraw: function () { status(grd); }
        });

        /** grd_CellUpdated (659): InTime / OutTime only. */
        function CellUpdated(r, k) {
            if (k === 'InTime' || k === 'OutTime') {
                var before = k === 'InTime' ? r._in : r._out;
                if (!timeEdited(grd, r, k, before)) return;
                if (k === 'InTime') r._in = r.InTime; else r._out = r.OutTime;
                grd.draw();
            }
        }

        function filter() {
            return {
                month: HRM.comboVal('cmbMonth'),
                year: HRM.comboVal('cmbYear'),
                shiftId: HRM.comboVal('cmbShift'),
                sectionId: HRM.comboVal('CmbSection'),
                departmentId: HRM.comboVal('CmbDepartment'),
                designationId: HRM.comboVal('CmbDesignation'),
                locationId: HRM.comboVal('CmbLocation'),
                employeeGroupId: HRM.comboVal('CmbEmployeeGroup'),
                employeeCategoryId: HRM.comboVal('CmbEmployeeCategory'),
                employeeId: HRM.comboVal('cmbEmployee'),
                isRestDay: HRM.checked('chkIsRestDay')
            };
        }

        function fillCombos(d, keep) {
            var o = { zero: '', keep: !!keep };
            if (d.employees) HRM.fill('cmbEmployee', d.employees, 'Id', 'EmployeeName', o);   // EmployeeName()
            if (d.shifts && d.shifts.length) HRM.fill('cmbShift', d.shifts, 'ShiftId', 'ShiftName', o);
            if (d.employeeCategories && d.employeeCategories.length) HRM.fill('CmbEmployeeCategory', d.employeeCategories, 'EmployeeCategoryId', 'EmployeeCategoryName', o);
            if (d.departments && d.departments.length) HRM.fill('CmbDepartment', d.departments, 'DepartmentId', 'DepartmentName', o);
            if (d.sections && d.sections.length) HRM.fill('CmbSection', d.sections, 'SectionId', 'SectionName', o);
            if (d.locations && d.locations.length) HRM.fill('CmbLocation', d.locations, 'LocationId', 'LocationName', o);
            if (d.designations && d.designations.length) HRM.fill('CmbDesignation', d.designations, 'DesignationId', 'DesignationName', o);
            if (d.employeeGroups && d.employeeGroups.length) HRM.fill('CmbEmployeeGroup', d.employeeGroups, 'EmployeeGroupId', 'EmployeeGroupName', o);
        }

        function EmployeeName() {
            var seq = ++empSeq;
            return HRM.post(API + '/employees', filter()).then(function (rows) {
                if (seq === empSeq) HRM.fill('cmbEmployee', rows || [], 'Id', 'EmployeeName', { zero: '', keep: true });
            }).catch(HRM.fail);
        }

        /** grdFill(): "Month Field Required" / "Year Field Required", rows sorted by DutyDate (SortKeys). */
        function grdFill() {
            var f = filter();
            if (f.month === 0) { HRM.focus('cmbMonth'); HRM.box('Month Field Required'); return Promise.resolve(); }
            if (f.year === 0) { HRM.focus('cmbYear'); HRM.box('Year Field Required'); return Promise.resolve(); }
            return HRM.post(API + '/load', f).then(function (rows) {
                if (rows && rows.length) {
                    loaded = f;
                    rows = rows.map(function (r, i) { r._ord = i; return r; });
                    rows.sort(function (a, b) { return (dutyKey(a.DutyDate) - dutyKey(b.DutyDate)) || (a._ord - b._ord); });
                    grd.set(remember(rows));
                } else {
                    loaded = null;
                    grd.clear();
                    HRM.box('Record Not Found...');
                }
            }).catch(HRM.fail);
        }

        /** btnLoad_Click: "Shift Required" when cmbShift has no active row. */
        function load() {
            if (HRM.comboVal('cmbShift') === 0) { HRM.box('Shift Required'); HRM.focus('cmbShift'); return Promise.resolve(); }
            return grdFill();
        }

        /** Reset(): month / year = today's, the other filters emptied (cmbShift is not), grid cleared. */
        function Reset() {
            HRM.focus('cmbMonth');
            HRM.setCombo('cmbMonth', curMonth);
            HRM.setCombo('cmbYear', curYear);
            ['cmbEmployee', 'CmbDepartment', 'CmbDesignation', 'CmbSection', 'CmbLocation', 'CmbEmployeeCategory', 'CmbEmployeeGroup']
                .forEach(function (id) { HRM.setCombo(id, 0); });
            loaded = null;
            grd.clear();
        }

        P.btnLoad = function (btn) { return HRM.busy(btn || 'btnLoad', load, 'manual-load'); };    // btnLoad_Click
        P.btnGenerate = function () { generate(grd); };                                         // btnGenerate_Click
        P.btnnew = function () { Reset(); };                                                    // btnnew_Click

        /** btnupdate_Click. */
        P.btnupdate = function (btn) {
            var b = HRM.$(btn || 'btnupdate');
            if (b && b.disabled) return;
            var rows = grd.visibleRows();
            if (rows.length === 0) { HRM.box('Grid Record Not Found Please Check'); return; }
            if (!HRM.ask('Are you sure to Update?')) return;
            return HRM.busy(b, function () {
                return HRM.post(API + '/update', { loaded: loaded || filter(), details: rows.map(rowDto) }).then(function (d) {
                    HRM.box((d && d.message) || 'Update Seccessfully');
                    Reset();
                }).catch(HRM.fail);
            }, 'manual-update');
        };

        /** btnRefresh_Click. */
        P.btnRefresh = function (btn) {
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.post(API + '/combos', filter()).then(function (d) { fillCombos(d || {}, true); }).catch(HRM.fail);
            }, 'manual-refresh');
        };

        /** brnEmployee_Click. */
        P.brnEmployee = function () {
            if (canEmployee) HRM.open('/hrm/employee-registration');
            else HRM.box("You don't have the right to view this form...");
        };

        onLeave(FILTERS, function () { EmployeeName(); });
        wireTimeCheck('chkInTime', 'txtInTime');
        wireTimeCheck('chkOutTime', 'txtOutTime');

        HRM.keys({                                                            // hrmManualAttendance_KeyDown
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': function () { /* one tab only */ },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); },
            'ctrl+r': function () { P.btnRefresh(); }
        });
        HRM.footer(function (b) {                                            // no history of its own: History reloads the grid
            return HRM.busy(b, function () {
                return load().then(function () { var g = HRM.$('grdBox'); if (g && g.scrollIntoView) g.scrollIntoView({ block: 'nearest' }); });
            }, 'manual-load');
        });

        HRM.fillFixed('cmbMonth', [[1, 'January'], [2, 'February'], [3, 'March'], [4, 'April'], [5, 'May'], [6, 'June'],
            [7, 'July'], [8, 'August'], [9, 'September'], [10, 'October'], [11, 'November'], [12, 'December']], EMPTY);  // MonthFill
        HRM.setCombo('cmbMonth', curMonth);                                   // cmbMonth.Value = DateTime.Now.Month
        HRM.fill('cmbYear', [], 'Id', 'Year', EMPTY);
        HRM.fill('cmbEmployee', [], 'Id', 'EmployeeName', EMPTY);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {             // frmDailyAttendance_Load (hrmManualAttendance)
            d = d || {};
            rights = d.rights || {};
            canEmployee = !!d.employeeRegistrationView;
            HRM.applyRights(rights, { update: ['btnupdate'] });               // btnupdate.Enabled (btnPrint stays hidden)
            if (d.month) { curMonth = HRM.int(d.month); HRM.setCombo('cmbMonth', curMonth); }
            if (d.year) curYear = HRM.int(d.year);
            HRM.fill('cmbYear', d.years || [], 'Id', 'Year', EMPTY);          // YearFill: bound, no value selected
            fillCombos(d, false);
            grd.clear();
            HRM.focus('cmbMonth');
        }).catch(HRM.fail);
    }

    var PAGES = { 'daily-attendance': dailyAttendance, 'manual-attendance': manualAttendance };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
