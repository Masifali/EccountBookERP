/* ============================================================================================
 * countx_hrm_shift.js - HRM shift screens (AppModules 2022). One script, the page is chosen by
 * <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   define-shift     frmGenShift.cs (652)
 *   shift-location   frmShiftLocation.cs (653)
 *   shift-timing     frmGenShiftTiming.cs (654)
 *   holiday          frmHoliday.cs (655)
 *
 * Each handler follows its desktop form line by line: validation wording and order, messages,
 * what New / Save / Update / double-click do to the buttons and the grids, and every calculation.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/shift/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmShift
    window.HrmShift = P;
    var NO_VIEW = "You don't have the right to view this form...";

    /** Project() / braches(): BindDDLNew(ZeroIndex false) + Rows[0].Activate() - only when rows came back. */
    function bindFirst(id, rows, valueKey, textKey) {
        if (!rows || !rows.length) return;
        HRM.fill(id, rows, valueKey, textKey, { zero: false });
    }

    // ============================================================================ 652 frmGenShift
    function defineShift() {
        var RecId = 0, UpdateMode = false, canShiftTiming = false;
        var grdShift = new HRM.Grid('grdShift', {                              // GridBind + GRidSetting
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'Shift', caption: 'Shift', type: 'code', width: 150 },
                { key: 'ShortName', caption: 'ShortName', width: 100 }
            ],
            filterRow: true,
            onDouble: function (r) { ReadById(HRM.int(r.Id)); }                  // grdShift_DoubleClick
        });
        function GridBind(rows) { if (rows && rows.length) grdShift.set(rows); else grdShift.clear(); }   // else ClearStructure
        function buttons(update) {
            HRM.show('btnsave', !update); HRM.show('btnupdate', update);
            HRM.show('BtnSavef', !update); HRM.show('btnUpdateF', update);
        }
        function ResetForm() {                                                  // ResetForm()
            HRM.setCombo('combranches', 0); HRM.setCombo('comproject', 0);
            HRM.setVal('txtShortName', ''); HRM.setVal('txtShiftUrduName', ''); HRM.setVal('txtShiftName', '');
            buttons(false); RecId = 0; UpdateMode = false;
            return HRM.get(API + '/reload').then(function (d) {                  // Project(); braches(); GridBind();
                bindFirst('comproject', d.projects, 'Id', 'ProjectName');
                bindFirst('combranches', d.branches, 'Id', 'BranchName');
                GridBind(d.rows);
                HRM.focus('txtShortName');
            }).catch(HRM.fail);
        }
        function ReadById(id) {                                                 // ReadById(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = id;
                HRM.setCombo('combranches', HRM.col(o, 'BranchId'));
                HRM.setCombo('comproject', HRM.col(o, 'ProjectId'));
                HRM.setVal('txtShiftName', HRM.str(HRM.col(o, 'ShiftName')));
                HRM.setVal('txtShortName', HRM.str(HRM.col(o, 'ShiftShortName')));
                HRM.setVal('txtShiftUrduName', HRM.str(HRM.col(o, 'ShiftUrduName')));
                UpdateMode = true; buttons(true);
            }).catch(HRM.fail);
        }
        function Validation() {                                                 // Validation(): Text == string.Empty
            if (HRM.val('txtShiftName') === '') { HRM.box('Shift Name Filed Required!'); HRM.focus('txtShiftName'); return false; }
            return true;
        }
        function save(btn, update) {                                           // btnsave_Click / btnupdate_Click
            if (!Validation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: update ? RecId : 0, shiftName: HRM.val('txtShiftName'), shortName: HRM.val('txtShortName'),
                    urduName: HRM.val('txtShiftUrduName'), branchId: HRM.comboVal('combranches'), projectId: HRM.comboVal('comproject')
                }).then(function (d) {
                    if (HRM.int(d.id) > 0) { HRM.box(d.message || (update ? 'Update Successfully' : 'Save Successfully')); return ResetForm(); }
                }).catch(HRM.fail);
            }, 'shift-save');
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', false); };
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', true); };
        P.btnnew = function () { ResetForm(); };                                         // btnnew_Click
        P.btnShiftTiming = function () {                                                 // btnShiftTiming_Click: "ShiftTiming" view right
            if (canShiftTiming) HRM.open('/hrm/shift-timing'); else HRM.box(NO_VIEW);
        };
        HRM.keys({                                                                       // frmGenShift_KeyDown (single tab: Ctrl+T has no second page)
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (UpdateMode) P.btnupdate(); }
        });
        HRM.footer(function (b) {                                                        // History = the form's own grid, refreshed
            return HRM.busy(b, function () { return HRM.get(API + '/list').then(GridBind).catch(HRM.fail); });
        });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // frmGenShift_Load
            canShiftTiming = !!d.canShiftTiming;
            bindFirst('comproject', d.projects, 'Id', 'ProjectName');
            bindFirst('combranches', d.branches, 'Id', 'BranchName');
            GridBind(d.rows);
            HRM.focus('txtShortName');
            buttons(false);
        }).catch(HRM.fail);
    }

    // ============================================================================ 653 frmShiftLocation
    function shiftLocation() {
        var RecId = 0, UpdateMode = false;
        var grd = new HRM.Grid('grdShiftLocation', {                            // GridBind + grdShiftLocationSetting
            columns: [
                { key: 'ShiftLocationId', caption: 'ShiftLocationId', hidden: true },
                { key: 'Shift', caption: 'Shift', type: 'code', width: 100 },
                { key: 'Location', caption: 'Location', width: 200 },
                { key: 'IsActive', caption: 'IsActive', type: 'check', width: 100 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(HRM.int(r.ShiftLocationId)); }   // grdShiftLocation_DoubleClick (Cells[0])
        });
        function GridBind() { return HRM.get(API + '/list').then(function (rows) { grd.set(rows || []); }).catch(HRM.fail); }
        function buttons(update) {
            HRM.show('btnupdate', update); HRM.show('btnsave', !update);
            HRM.show('btnUpdateF', update); HRM.show('BtnSavef', !update);
        }
        function Reset() {                                                      // Reset(): Branch / Project keep their rows; RecId is kept
            HRM.setCombo('cmbLocation', 0); HRM.setCombo('cmbShift', 0);
            HRM.check('checkBoxActive', false);
            buttons(false); UpdateMode = false;
            HRM.focus('cmbShift');
            return GridBind();
        }
        function RetrivedData(id) {                                             // RetrivedData(Id)
            RecId = id;
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                HRM.setCombo('combranches', HRM.int(HRM.col(o, 'BranchId')));
                HRM.setCombo('comproject', HRM.int(HRM.col(o, 'ProjectId')));
                HRM.setCombo('cmbShift', HRM.int(HRM.col(o, 'ShiftId')));
                HRM.setCombo('cmbLocation', HRM.int(HRM.col(o, 'LocationId')));
                HRM.check('checkBoxActive', HRM.bool(HRM.col(o, 'IsActive')));
                buttons(true); UpdateMode = true;
            }).catch(HRM.fail);
        }
        function Validation() {                                                 // Validation()
            if (HRM.comboVal('cmbShift') === 0) { HRM.box('Shift Required!'); HRM.focus('cmbShift'); return false; }
            if (HRM.comboVal('cmbLocation') === 0) { HRM.box('Location Required!'); HRM.focus('cmbLocation'); return false; }
            return true;
        }
        function save(btn, update) {                                           // btnsave_Click / btnupdate_Click
            if (!Validation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: update ? RecId : 0, shiftId: HRM.comboVal('cmbShift'), locationId: HRM.comboVal('cmbLocation'),
                    branchId: HRM.comboVal('combranches'), projectId: HRM.comboVal('comproject'), isActive: HRM.checked('checkBoxActive')
                }).then(function (d) {
                    if (HRM.int(d.id) > 0) { HRM.box(d.message || (update ? 'Update Successfully' : 'Save Successfully')); return Reset(); }
                }).catch(HRM.fail);
            }, 'shift-location-save');
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', false); };
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', true); };
        P.btnnew = function () { Reset(); };                                             // btnnew_Click
        P.btnDefineShift = function () { HRM.open('/hrm/define-shift'); };              // btnDefineShift_Click: new frmGenShift().Show()
        P.btnDefinelocation = function () { HRM.open('/hrm/location'); };               // btnDefinelocation_Click: new frmgenLocation().Show()
        HRM.keys({                                                                       // frmShiftLocation_KeyDown
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (UpdateMode) P.btnupdate(); }
        });
        HRM.footer(function (b) { return HRM.busy(b, GridBind); });                     // History = the form's own grid, refreshed
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // frmShiftLocation_Load
            buttons(false);
            bindFirst('comproject', d.projects, 'Id', 'ProjectName');
            bindFirst('combranches', d.branches, 'Id', 'BranchName');
            HRM.fill('cmbShift', d.shifts, 'Id', 'SelectShift', { zero: '' });           // BindDDL(ZeroIndex false): no row active
            HRM.fill('cmbLocation', d.locations, 'Id', 'SelectLocation', { zero: '' });
            grd.set(d.rows || []);
            HRM.focus('cmbShift');
        }).catch(HRM.fail);
    }

    // ============================================================================ 654 frmGenShiftTiming
    function shiftTiming() {
        var RecId = 0, UpdateMode = false, rights = {}, canDutyRoaster = false;
        var DESIGNER_TIME = '18:41';                                            // designer Value 20-Jul-2023 18:41:54 of every time picker
        var TIMES = ['datStartTime', 'datEndTime', 'datStartBreakTime', 'datEndBreakTime', 'datLateStartTime', 'datLateEndTime',
            'datDepartureStarTime', 'datDepartureEndTime', 'datShortLeaveInStartTime', 'datShortLeaveInEndTime', 'datShortLeaveOutStartTime',
            'datShortLeaveOutEndTime', 'datHalfDayInStartTime', 'datHalfDayInEndTime', 'datHalfDayOutStartTime', 'datHalfDayOutEndTime',
            'datAbsentAfterInTime', 'datAbsentBeforeOutTime'];
        // RefreshForm sets these to DateTime.Now; Late Start/End and Departure Start/End keep their value
        var REFRESHED = ['datAbsentAfterInTime', 'datAbsentBeforeOutTime', 'datEndBreakTime', 'datEndTime', 'datHalfDayInEndTime',
            'datHalfDayInStartTime', 'datHalfDayOutEndTime', 'datHalfDayOutStartTime', 'datShortLeaveInEndTime', 'datShortLeaveInStartTime',
            'datShortLeaveOutEndTime', 'datShortLeaveOutStartTime', 'datStartBreakTime', 'datStartTime'];

        var grd = new HRM.Grid('grd', {                                         // WeekDaysFill + WeekDaySetting
            columns: [
                { key: 'Wds', caption: '', type: 'edit-check', width: 40 },
                { key: 'ProfileId', caption: 'ProfileId', hidden: true },
                { key: 'ProfileName', caption: 'Day Name', width: 150 }
            ],
            checkAll: 'Wds'
        });
        function WeekDaysFill(rows) { if (rows && rows.length) grd.set(rows.map(function (r) { r.Wds = false; return r; })); }

        function BindShift(rows) {                                             // BindShift(): BindDDLNew(ZeroIndex false) - nothing active
            if (rows && rows.length) HRM.fill('CmbShift', rows, 'ShiftId', 'ShiftName', { zero: '' });
        }
        function BindShiftLocation() {                                          // BindShiftLocation()
            HRM.fill('CmbShiftLocation', [], 'ShiftId', 'LocationName', { zero: '' });   // DataSource = null; Text = ""
            return HRM.get(API + '/locations', { shiftId: HRM.comboVal('CmbShift') }).then(function (rows) {
                if (rows && rows.length) {
                    HRM.fill('CmbShiftLocation', rows, 'ShiftId', 'LocationName', { zero: '' });
                    HRM.setCombo('CmbShiftLocation', HRM.col(rows[0], 'ShiftId'));       // Rows[0].Activate()
                }
            }).catch(HRM.fail);
        }

        // ------------------------------------------------------------ TotalShifthours() - hours only, as the desktop
        function toDouble(s) {                                                  // Convert.ToDouble(text)
            var t = String(s).trim().replace(/,/g, '');
            if (!/^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(t)) throw new Error('Input string was not in a correct format.');
            return parseFloat(t);
        }
        function hourOf(id) { var m = /^(\d{1,2})/.exec(HRM.val(id)); return m ? parseInt(m[1], 10) : 0; }   // DateTimePicker.Value.Hour
        function dstr(n) { return String(n); }                                  // double.ToString()
        function TotalShifthours() {
            try {
                var TotalDaysHours = 0, TotalShiftHours = 0, InHours = 0, OutHours = 0, ToalHours = 0;
                var eh = hourOf('datEndTime'), sh = hourOf('datStartTime');
                if (eh > sh) {
                    HRM.setVal('txtTotalShiftHours', dstr(eh - sh));
                    if (HRM.val('txtTotalShiftHours') !== '') TotalShiftHours = toDouble(HRM.val('txtTotalShiftHours'));
                    if (HRM.val('txtInHours') !== '') InHours = toDouble(HRM.val('txtInHours'));
                    if (HRM.val('txtOutHours') !== '') OutHours = toDouble(HRM.val('txtOutHours'));
                    if (InHours !== 0 || OutHours !== 0) {
                        ToalHours = OutHours + InHours;
                        HRM.setVal('txtTotalHours', dstr(ToalHours));
                        TotalDaysHours = TotalShiftHours + ToalHours;
                    }
                    if (HRM.val('txtTotalHours') !== '') HRM.setVal('txtTotalDaysHours', dstr(TotalDaysHours));
                } else {
                    var hour = eh - sh;
                    TotalShiftHours = 24 + hour;
                    HRM.setVal('txtTotalShiftHours', dstr(TotalShiftHours));
                    if (HRM.val('txtInHours') !== '') InHours = toDouble(HRM.val('txtInHours'));
                    if (HRM.val('txtOutHours') !== '') {
                        OutHours = toDouble(HRM.val('txtOutHours'));
                        HRM.setVal('txtTotalHours', dstr(OutHours + InHours));
                        TotalDaysHours = TotalShiftHours + InHours + OutHours;
                        HRM.setVal('txtTotalDaysHours', dstr(TotalDaysHours));
                    }
                    if (HRM.val('txtTotalHours') !== '') HRM.setVal('txtTotalDaysHours', dstr(TotalDaysHours));
                }
            } catch (e) { HRM.fail(e); }
        }
        HRM.$('datStartTime').addEventListener('blur', TotalShifthours);          // datStartTime_Leave
        HRM.$('datEndTime').addEventListener('blur', TotalShifthours);            // datEndTime_Leave
        HRM.$('txtInHours').addEventListener('input', TotalShifthours);           // txtInHours_TextChanged
        HRM.$('txtOutHours').addEventListener('input', TotalShifthours);          // txtOutHours_TextChanged
        function setText(id, v) {                                               // TextBox.Text = v (TextChanged only when it changes)
            v = HRM.str(v); if (HRM.val(id) === v) return;
            HRM.setVal(id, v); TotalShifthours();
        }
        HRM.$('CmbShift').addEventListener('change', function () { BindShiftLocation(); });   // CmbShift_Leave

        function buttons(mode) {                                                // 'new' | 'update' | 'saveas'
            HRM.show('btnsave', mode === 'new'); HRM.show('btnupdate', mode === 'update'); HRM.show('btnSaveAs', mode === 'saveas');
        }
        function RefreshForm() {                                                // RefreshForm()
            var now = HRM.nowTime(), today = HRM.today();
            HRM.setVal('datStartDate', today); HRM.setVal('datEndDate', today);
            REFRESHED.forEach(function (id) { HRM.setVal(id, now); });
            HRM.setCombo('CmbShiftLocation', 0);
            HRM.setVal('txtDescription', '');
            HRM.setCombo('CmbShift', 0);
            buttons('new'); UpdateMode = false; RecId = 0;
            grd.clear();                                                        // grd.ClearStructure(); GridBind(); WeekDaysFill()
            HRM.focus('CmbShift');
            return HRM.get(API + '/reload').then(function (d) { WeekDaysFill(d.weekDays); }).catch(HRM.fail);
        }
        function ReadById(id) {                                                 // ReadById(Id)
            RecId = id;
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (t) {
                if (history && history.modal) history.modal.close();            // tabControl1.SelectedIndex = 0
                HRM.setVal('txtDescription', HRM.str(HRM.col(t, 'TimingDescription')));
                HRM.setCombo('CmbShift', HRM.col(t, 'ShiftId'));
                return BindShiftLocation().then(function () {                   // CmbShift_Leave(null, null)
                    HRM.setCombo('CmbShiftLocation', HRM.col(t, 'ShiftLocationId'));
                    HRM.setVal('datStartDate', HRM.day(HRM.col(t, 'StartDate')));
                    HRM.setVal('datEndDate', HRM.day(HRM.col(t, 'EndDate')));
                    HRM.setVal('datStartTime', HRM.col(t, 'StartTime'));
                    HRM.setVal('datEndTime', HRM.col(t, 'EndTime'));
                    HRM.setVal('datStartBreakTime', HRM.col(t, 'StartBreakTime'));
                    HRM.setVal('datEndBreakTime', HRM.col(t, 'EndBreakTime'));
                    HRM.setVal('datLateStartTime', HRM.col(t, 'GraceTime'));
                    HRM.setVal('datLateEndTime', HRM.col(t, 'GraceInEndTime'));
                    setText('txtInHours', HRM.int(HRM.col(t, 'BeforeInHours')));
                    setText('txtOutHours', HRM.int(HRM.col(t, 'AfterOutHours')));
                    HRM.setVal('datDepartureStarTime', HRM.col(t, 'GraceOutStartTime'));
                    HRM.setVal('datDepartureEndTime', HRM.col(t, 'GraceOutEndTime'));
                    HRM.setVal('datShortLeaveInStartTime', HRM.col(t, 'ShortLeaveInStartTime'));
                    HRM.setVal('datShortLeaveInEndTime', HRM.col(t, 'ShortLeaveInEndTime'));
                    HRM.setVal('datShortLeaveOutStartTime', HRM.col(t, 'ShortLeaveOutStartTime'));
                    HRM.setVal('datShortLeaveOutEndTime', HRM.col(t, 'ShortLeaveOutEndTime'));
                    HRM.setVal('datHalfDayInStartTime', HRM.col(t, 'HalfDayInStartTime'));
                    HRM.setVal('datHalfDayOutStartTime', HRM.col(t, 'HalfDayOutStartTime'));
                    HRM.setVal('datHalfDayOutEndTime', HRM.col(t, 'HalfDayOutEndTime'));
                    HRM.setVal('datHalfDayInEndTime', HRM.col(t, 'HalfDayInEndTime'));
                    HRM.setVal('datAbsentAfterInTime', HRM.col(t, 'AbsentAfterInTime'));
                    HRM.setVal('datAbsentBeforeOutTime', HRM.col(t, 'AbsentBeforeOutTime'));
                    HRM.check('ChkBoxIsClosedNextDay', HRM.bool(HRM.col(t, 'IsNextDayClosed')));
                    buttons('update'); UpdateMode = true;                       // the week days are NOT re-ticked (desktop)
                });
            }).catch(HRM.fail);
        }
        function Validation() {                                                 // Validation()
            if (HRM.comboVal('CmbShift') === 0) { HRM.box('Shift Required!'); HRM.focus('CmbShift'); return false; }
            if (HRM.comboVal('CmbShiftLocation') === 0) { HRM.box('Shift Location Required!'); HRM.focus('CmbShiftLocation'); return false; }
            if (HRM.val('txtDescription') === '') { HRM.box('Description Required!'); HRM.focus('txtDescription'); return false; }
            return true;
        }
        function Insert(btn) {                                                  // Insert()
            if (!Validation()) return;
            var days = grd.checked('Wds');
            if (!days.length) { HRM.box('Please Select Week Days'); return; }
            // CheckShiftTiming(timing) runs on the still-empty model on the desktop and can never fail.
            try {
                if (toDouble(HRM.val('txtTotalDaysHours')) !== 24) { HRM.box('Total Days Hours not less or gratter than 24 hours'); return; }
            } catch (e) { HRM.fail(e); return; }
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var body = {
                id: RecId, shiftId: HRM.comboVal('CmbShift'), shiftLocationId: HRM.comboVal('CmbShiftLocation'),
                description: HRM.val('txtDescription'), startDate: HRM.val('datStartDate'), endDate: HRM.val('datEndDate'),
                startTime: HRM.val('datStartTime'), endTime: HRM.val('datEndTime'),
                startBreakTime: HRM.val('datStartBreakTime'), endBreakTime: HRM.val('datEndBreakTime'),
                lateStartTime: HRM.val('datLateStartTime'), lateEndTime: HRM.val('datLateEndTime'),
                departureStartTime: HRM.val('datDepartureStarTime'), departureEndTime: HRM.val('datDepartureEndTime'),
                shortLeaveInStartTime: HRM.val('datShortLeaveInStartTime'), shortLeaveInEndTime: HRM.val('datShortLeaveInEndTime'),
                shortLeaveOutStartTime: HRM.val('datShortLeaveOutStartTime'), shortLeaveOutEndTime: HRM.val('datShortLeaveOutEndTime'),
                halfDayInStartTime: HRM.val('datHalfDayInStartTime'), halfDayInEndTime: HRM.val('datHalfDayInEndTime'),
                halfDayOutStartTime: HRM.val('datHalfDayOutStartTime'), halfDayOutEndTime: HRM.val('datHalfDayOutEndTime'),
                absentAfterInTime: HRM.val('datAbsentAfterInTime'), absentBeforeOutTime: HRM.val('datAbsentBeforeOutTime'),
                inHours: HRM.val('txtInHours'), outHours: HRM.val('txtOutHours'), totalDaysHours: HRM.val('txtTotalDaysHours'),
                isNextDayClosed: HRM.checked('ChkBoxIsClosedNextDay'),
                weekDays: days.map(function (r) { return { profileId: HRM.int(r.ProfileId), name: HRM.str(r.ProfileName) }; })
            };
            var wasNew = RecId === 0;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (wasNew ? 'Record Save Successfully' : 'Record Update Successfully'));
                    return RefreshForm();
                }).catch(HRM.fail);
            }, 'shift-timing-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };             // btnupdate_Click
        P.btnSaveAs = function (btn) { RecId = 0; return Insert(btn || 'btnSaveAs'); };  // btnSaveAs_Click
        P.btnnew = function () { RefreshForm(); };                                       // btnnew_Click
        P.toolStripButton1 = function (btn) {                                            // Refresh: BindShift(); WeekDaysFill(); BindShiftLocation();
            return HRM.busy(btn || 'toolStripButton1', function () {
                return HRM.get(API + '/reload').then(function (d) { BindShift(d.shifts); WeekDaysFill(d.weekDays); return BindShiftLocation(); }).catch(HRM.fail);
            });
        };
        P.brnShift = function () { HRM.open('/hrm/define-shift'); };                      // brnShift_Click: new frmGenShift().Show()
        P.btnShiftLocation = function () { HRM.open('/hrm/shift-location'); };           // btnShiftLocation_Click: new frmShiftLocation().Show()
        P.btnDutyRoasterForm = function () {                                             // btnDutyRoasterForm_Click: "DutyRoaster" view right
            if (canDutyRoaster) HRM.open('/hrm/duty-roaster'); else HRM.box(NO_VIEW);
        };

        // ------------------------------------------------------------ History tab (GridBind + BindSetting) as a dialog
        var history = null;
        function btn(kind) { return function () { return '<button type="button" class="win-btn-action hrm-cell-btn" data-' + kind + '="1">' + (kind === 'edit' ? 'Edit' : 'SaveAs') + '</button>'; }; }
        function showHistory() {                                                // tabControl1_SelectedIndexChanged(1) -> GridBind()
            history = HRM.history({
                title: 'Shift Timing History',
                columns: [
                    { key: 'Edit', caption: 'Edit', width: 50, render: btn('edit') },
                    { key: 'SaveAs', caption: 'SaveAs', width: 60, render: btn('saveas') },
                    { key: 'Id', caption: 'Id', type: 'code' },
                    { key: 'LoacationName', caption: 'LoacationName' }, { key: 'ShiftName', caption: 'ShiftName' },
                    { key: 'ShiftTimingDesc', caption: 'ShiftTimingDesc' }, { key: 'Location', caption: 'Location' },
                    { key: 'WeekDay', caption: 'WeekDay' },
                    { key: 'StartDate', caption: 'StartDate', type: 'date' }, { key: 'EndDate', caption: 'EndDate', type: 'date' },
                    { key: 'StartTime', caption: 'StartTime' }, { key: 'EndTime', caption: 'EndTime' },
                    { key: 'startBreaktime', caption: 'startBreaktime' }, { key: 'EndBreaktime', caption: 'EndBreaktime' },
                    { key: 'GraceInTime', caption: 'GraceInTime' }, { key: 'GraceEndTime', caption: 'GraceEndTime' },
                    { key: 'BeforeInHourn', caption: 'BeforeInHourn', type: 'int' }, { key: 'BeforeInMinute', caption: 'BeforeInMinute', type: 'int' },
                    { key: 'AfterOutHourn', caption: 'AfterOutHourn', type: 'int' }, { key: 'AfterOutMinute', caption: 'AfterOutMinute', type: 'int' },
                    { key: 'ReprotTime', caption: 'ReprotTime' }, { key: 'ReportDay', caption: 'ReportDay', type: 'int' },
                    { key: 'ShortLeaveInStartTime', caption: 'ShortLeaveInStartTime' }, { key: 'ShortLeaveInEndTime', caption: 'ShortLeaveInEndTime' },
                    { key: 'ShortLeaveOutStartTime', caption: 'ShortLeaveOutStartTime' }, { key: 'ShortLeaveOutEndTime', caption: 'ShortLeaveOutEndTime' },
                    { key: 'HaldDayInStartTime', caption: 'HaldDayInStartTime' }, { key: 'HaldDayInEndTime', caption: 'HaldDayInEndTime' },
                    { key: 'HaldDayOutStartTime', caption: 'HaldDayOutStartTime' }, { key: 'HaldDayOutEndTime', caption: 'HaldDayOutEndTime' },
                    { key: 'GraceOutStartTime', caption: 'GraceOutStartTime' }, { key: 'GraceOutEndTime', caption: 'GraceOutEndTime' },
                    { key: 'AbsentAfterInStartTime', caption: 'AbsentAfterInStartTime' }, { key: 'AbsentBeforeOutTime', caption: 'AbsentBeforeOutTime' }
                ],
                load: function () { return HRM.get(API + '/history'); },
                onPick: function (r) { ReadById(HRM.int(r.Id)); },              // grdShift_DoubleClick
                onReady: function (body, g) {                                   // grdShift_ColumnButtonClick
                    body.addEventListener('click', function (e) {
                        var b = e.target.closest('[data-edit],[data-saveas]'); if (!b) return;
                        var tr = b.closest('tr[data-i]'); if (!tr) return;
                        var r = g.rows()[+tr.getAttribute('data-i')];
                        if (b.hasAttribute('data-edit')) { ReadById(HRM.int(r.Id)); return; }
                        ReadById(HRM.int(r.Id)).then(function () {
                            buttons('saveas'); UpdateMode = false;              // btnSaveAs visible, update / save hidden
                        });
                    });
                }
            });
        }
        P.history = showHistory;
        HRM.keys({                                                                       // frmGenShiftTiming_KeyDown
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': function () { showHistory(); },                                   // toggles Form / History tab
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (UpdateMode) P.btnupdate(); }
        });
        HRM.footer(function () { showHistory(); });

        // designer defaults: dates = today (DateTimePicker default), every time picker 18:41
        HRM.setVal('datStartDate', HRM.today()); HRM.setVal('datEndDate', HRM.today());
        TIMES.forEach(function (id) { HRM.setVal(id, DESIGNER_TIME); });
        buttons('new');
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // frmGenShiftTiming_Load
            rights = d.rights || {};
            canDutyRoaster = !!d.canDutyRoaster;
            HRM.applyRights(rights, { save: ['btnsave'], update: ['btnupdate'] });
            BindShift(d.shifts);
            WeekDaysFill(d.weekDays);
            HRM.focus('CmbShift');
        }).catch(HRM.fail);
    }

    // ============================================================================ 655 frmHoliday
    function holiday() {
        var DAYS = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];   // DayOfWeek.ToString()
        var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        function ddMMMyyyy(y, m, d) { return String(d).padStart(2, '0') + '-' + MON[m - 1] + '-' + y; }
        // grd_Click: IsOff wins - when IsOff is ticked IsGazette is cleared, else a ticked IsGazette clears IsOff
        function offOrGazette(r) {
            var changed = false;
            if (HRM.bool(r.IsOff)) { if (HRM.bool(r.IsGazette)) { r.IsGazette = false; changed = true; } }
            else if (HRM.bool(r.IsGazette)) { if (HRM.bool(r.IsOff)) { r.IsOff = false; changed = true; } }
            return changed;
        }
        var grd = new HRM.Grid('grd', {                                         // grdSetting
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'Sr#', caption: 'Sr#', hidden: true },
                { key: 'Date', caption: 'Date', width: 100 },
                { key: 'Day', caption: 'Day', width: 90 },
                { key: 'IsOff', caption: 'IsOff', type: 'edit-check', width: 60 },
                { key: 'IsGazette', caption: 'IsGazette', type: 'edit-check', width: 70 },
                { key: 'Description', caption: 'Description', type: 'edit', width: 350 }
            ],
            filterRow: true,
            onChange: function (r, k) { if (k === 'IsOff' || k === 'IsGazette') { offOrGazette(r); grd.draw(); } }
        });
        HRM.$('grd').addEventListener('click', function (e) {                   // grd_Click on any cell of a row
            if (e.target.closest('input,select,a')) return;
            var tr = e.target.closest('tbody tr[data-i]'); if (!tr) return;
            if (offOrGazette(grd.rows()[+tr.getAttribute('data-i')])) grd.draw();
        });
        function GenerateDatesOfMonth(year, month) {                            // GenerateDatesOfMonth(year, month)
            var rows = [], sr = 1, dt = new Date(year, month - 1, 1);
            while (dt.getMonth() === month - 1) {
                rows.push({ Id: 0, 'Sr#': sr++, Date: ddMMMyyyy(year, month, dt.getDate()), Day: DAYS[dt.getDay()], IsOff: false, IsGazette: false, Description: '' });
                dt.setDate(dt.getDate() + 1);
            }
            return rows;
        }
        function btnLoad(btn) {                                                 // btnLoad_Click
            var month = HRM.comboVal('cmbMonth'), year = HRM.comboVal('cmbYear');
            if (month === 0) { HRM.box('Please Select Month...'); HRM.focus('cmbMonth'); return; }
            if (year === 0) { HRM.box('Please Select Year...'); HRM.focus('cmbYear'); return; }
            var rows = GenerateDatesOfMonth(year, month);
            grd.set(rows);
            return HRM.busy(btn, function () {                                  // GetByMonthandYearForUpdate()
                return HRM.get(API + '/by-month', { month: month, year: year }).then(function (lst) {
                    (lst || []).forEach(function (h) {
                        var p = HRM.day(HRM.col(h, 'HolidayDate')).split('-');
                        var date = ddMMMyyyy(+p[0], +p[1], +p[2]);
                        rows.forEach(function (r, i) {
                            if (r.Date !== date) return;
                            rows[i] = { Id: HRM.int(HRM.col(h, 'HolidayId')), 'Sr#': i, Date: date, Day: HRM.str(HRM.col(h, 'Name')),
                                IsOff: HRM.bool(HRM.col(h, 'IsOffDay')), IsGazette: HRM.bool(HRM.col(h, 'IsGazetted')), Description: HRM.str(HRM.col(h, 'HolidayDetail')) };
                        });
                    });
                    grd.set(rows);
                }).catch(HRM.fail);
            }, 'holiday-load');
        }
        function Reset() {                                                      // Reset()
            HRM.setCombo('cmbMonth', 0); HRM.setCombo('cmbYear', 0);
            grd.clear();
        }
        function Insert(btn) {                                                  // Insert()
            if (!HRM.ask('Are you sure to Save?')) return;
            var rows = grd.rows();
            for (var i = 0; i < rows.length; i++) {
                if (HRM.bool(rows[i].IsOff) && HRM.bool(rows[i].IsGazette)) { HRM.box('On a Same Day you cannot check both IsOff and IsGazette'); return; }
            }
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    rows: rows.map(function (r) {
                        return { id: HRM.int(r.Id), date: r.Date, day: HRM.str(r.Day), isOff: HRM.bool(r.IsOff), isGazette: HRM.bool(r.IsGazette), description: HRM.str(r.Description) };
                    })
                }).then(function (d) { HRM.box(d.message || 'Save Successfully'); Reset(); }).catch(HRM.fail);
            }, 'holiday-save');
        }
        P.btnLoad = function (btn) { return btnLoad(btn || 'btnLoad'); };
        P.btnsave = function (btn) { return Insert(btn || 'btnsave'); };                 // btnsave_Click
        P.btnnew = function () { Reset(); };                                             // btnnew_Click
        HRM.keys({                                                                       // frmHoliday_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        HRM.footer(function (b) { return P.btnLoad(b); });                               // the form's own grid = the month loaded again
        HRM.fillFixed('cmbMonth', [[1, 'January'], [2, 'February'], [3, 'March'], [4, 'April'], [5, 'May'], [6, 'June'], [7, 'July'],
            [8, 'August'], [9, 'September'], [10, 'October'], [11, 'November'], [12, 'December']], { zero: '' });   // MonthFill (ZeroIndex false)
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // frmHoliday_Load: YearFill
            HRM.fill('cmbYear', d.years, 'Id', 'Year', { zero: '' });
        }).catch(HRM.fail);
    }

    var PAGES = { 'define-shift': defineShift, 'shift-location': shiftLocation, 'shift-timing': shiftTiming, holiday: holiday };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
