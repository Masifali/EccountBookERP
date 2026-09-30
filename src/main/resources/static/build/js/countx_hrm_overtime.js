/* ============================================================================================
 * countx_hrm_overtime.js - HRM "Over Time Management" (AppModules 2025). One script, the page is
 * chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   over-time-request    OverTimeRequest.cs (665) + its ShowDialog popup EmpOverTimeLoadForRequest.cs
 *   employee-over-time   frmEmployeeOverTime.cs (666)
 *
 * Each handler follows its desktop form line by line: validation wording and order, messages,
 * what New / Save / Update / double-click do to the buttons and the grids. The forms' History tab
 * is the footer's History dialog (HRM.history) with the tab's own filters, toolbar and grids.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/overtime/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmOvertime
    window.HrmOvertime = P;

    // ============================================================================ shared helpers
    function pad(n) { return String(n).padStart(2, '0'); }
    /** FormatString "hh:mm tt" of a time / date-time value ("HH:mm", ISO, epoch). */
    function t12(v) {
        var t = HRM.time(v); if (!t) return '';
        var h = +t.substr(0, 2), m = t.substr(3, 2);
        return pad(h % 12 === 0 ? 12 : h % 12) + ':' + m + ' ' + (h < 12 ? 'AM' : 'PM');
    }
    /** "dd-MM-yyyy hh:mm tt" */
    function dmy12(v) {
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'); return p[2] + '-' + p[1] + '-' + p[0] + ' ' + t12(v);
    }
    /** "dd-MMM-yyyy hh:mm tt" */
    function dmmm12(v) { var d = HRM.fmtDate(v); return d ? d + ' ' + t12(v) : ''; }
    /** .NET "#,##0.###" (zero shows 0) and "#,#.###" (zero shows nothing). */
    function n3(v, blankZero) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.round(v, 3);
        if (blankZero && n === 0) return '';
        var s = String(Math.abs(n)).split('.');
        s[0] = s[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + s.join('.');
    }
    function nowTime() { return HRM.nowTime(); }
    function codeLink(ci, text) { return '<a href="#" class="hrm-code" data-c="' + ci + '">' + HRM.esc(text) + '</a>'; }
    /** A table element replaced by a fresh copy, so a grid can be rebuilt with new columns (RetrieveStructure). */
    function freshTable(id) {
        var t = HRM.$(id), n = t.cloneNode(false);
        t.parentNode.replaceChild(n, t);
        return n;
    }
    function focusGrid(tableId) {
        var tr = document.querySelector('#' + tableId + ' tbody tr[data-i]');
        if (tr) tr.focus(); else HRM.focus(tableId);
    }
    /** MakeShortCutKeys(): the ShortCutKeyPopUp form - KeyCombination / Description. */
    function shortcutPopup(rows) {
        var m = HRM.modal({ title: 'ShortCut Keys', width: 'min(720px, 96vw)',
            html: '<div class="hrm-grid-wrap"><table class="ot-keys"></table></div>' });
        var g = new HRM.Grid(m.body.querySelector('table'), {
            columns: [{ key: 'KeyCombination', caption: 'KeyCombination' }, { key: 'Description', caption: 'Description', cls: 'hrm-wrap' }]
        });
        g.set(rows.map(function (r) { return { KeyCombination: r[0], Description: r[1] }; }));
        return m;
    }
    /** Normalised key of a keydown inside a dialog (HRM.keys is paused while a dialog is open). */
    function keyOf(e) {
        return (e.ctrlKey ? 'ctrl+' : '') + (e.altKey ? 'alt+' : '') + (e.shiftKey ? 'shift+' : '') +
            (e.key === 'Escape' ? 'esc' : (e.key || '').toLowerCase());
    }
    function isCtrlAlt(e) { return e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt'); }
    /** Key handler of a dialog (the desktop form's KeyDown while that form is active): runs while the dialog is the topmost one. */
    function dialogKeys(el, fn) {
        function h(e) {
            if (!document.body.contains(el)) { document.removeEventListener('keydown', h); return; }
            var all = document.querySelectorAll('.hrm-modal');
            if (all[all.length - 1] !== el) return;
            fn(e);
        }
        document.addEventListener('keydown', h);
    }

    // ============================================================================ 665 Over Time Request
    function overTimeRequest() {
        var RecId = 0, updateDetailIndex = -1, rights = {}, defaultDays = 0;
        var dtEmployees = [], departments = [], sections = [];
        var dtdetail = [];                 // Id, EmployeeId, EmployeeName, FromDateTime, ToDateTime, OTHours, OTRate, IsAllowMeal, EntryByLoader
        var lstRemoveDetailRecord = [];
        var grd = null;
        var hist = null;                   // History tab state, kept between openings (the tab keeps its values)

        // ---------------------------------------------------------------- combos
        function fillCombos(d) {           // DepartmentFill / SectionFill / RequestedByFill (ZeroIndex true, value kept when still listed)
            departments = d.departments || []; sections = d.sections || []; dtEmployees = d.employees || [];
            HRM.fill('cmbDepartmnet', departments, 'Id', 'Name', { zero: '...Select Any Value...', keep: true });
            HRM.fill('cmbSection', sections, 'Id', 'Name', { zero: '...Select Any Value...', keep: true });
            HRM.fill('cmbRequestedBy', dtEmployees, 'EmployeeId', 'EmployeeName', { zero: '...Select Any Value...', keep: true });
        }
        function cmbDepartmnet_Leave() {   // employees of the department -> cmbEmployee (BindDDLNew, ZeroIndex false)
            var dep = HRM.comboVal('cmbDepartmnet');
            if (dep > 0) {
                var rows = dtEmployees.filter(function (r) { return HRM.int(r.DepartmentId) === dep; });
                if (rows.length > 0) HRM.fill('cmbEmployee', rows, 'EmployeeId', 'EmployeeName', { zero: '' });
            } else {
                HRM.fill('cmbEmployee', [], 'EmployeeId', 'EmployeeName', { zero: '' });
            }
        }

        // ---------------------------------------------------------------- detail grid
        function grdSettings() {           // grdSettings(): Edit column only when the rows are not from the loader
            var loader = dtdetail.length > 0 && HRM.bool(dtdetail[0].EntryByLoader);
            var t = freshTable('grdDetails');
            var cols = [];
            if (!loader) { var ce = cols.length; cols.push({ key: 'Edit', caption: 'Edit', width: 40, align: 'center', render: function () { return codeLink(ce, 'Edit'); } }); }
            var cx = cols.length;
            cols.push({ key: 'Delete', caption: 'X', width: 30, align: 'center', render: function () { return codeLink(cx, 'X'); } });
            cols.push({ key: 'Id', caption: 'Id', hidden: true });
            cols.push({ key: 'EmployeeId', caption: 'EmployeeId', hidden: true });
            cols.push({ key: 'EmployeeName', caption: 'EmployeeName' });
            cols.push({ key: 'FromDateTime', caption: 'From Time', render: function (v) { return HRM.esc(t12(v)); } });
            cols.push({ key: 'ToDateTime', caption: 'To Time', render: function (v) { return HRM.esc(t12(v)); } });
            cols.push({ key: 'OTHours', caption: 'OTHours', hidden: true });
            cols.push({ key: 'OTRate', caption: 'OTRate', hidden: true });
            cols.push({ key: 'IsAllowMeal', caption: 'IsAllowMeal', hidden: true });
            cols.push({ key: 'EntryByLoader', caption: 'EntryByLoader', hidden: true });
            grd = new HRM.Grid(t, {
                columns: cols, filterRow: true,
                onDouble: function (r, i) { grdDetails_DoubleClick(i); },
                onCode: function (r, i, c) { grdDetails_ColumnButtonClick(i, c.key); }
            });
            grd.data = dtdetail;           // the grid is bound to dtdetail itself
            grd.draw();
        }
        function grdDetails_ColumnButtonClick(i, key) {
            var r = dtdetail[i]; if (!r) return;
            if (key === 'Delete') {
                if (HRM.int(r.Id) > 0) {
                    if (!HRM.ask('Are you sure to Delete?')) return;
                    lstRemoveDetailRecord.push(r);              // OverTimeRequestDetail with ActionTypeId 3
                }
                dtdetail.splice(i, 1);
                if (updateDetailIndex === i) updateDetailIndex = -1; else if (updateDetailIndex > i) updateDetailIndex--;
                grd.cur = -1; grd.draw();
                if (dtdetail.length === 0) { HRM.enable('cmbDepartmnet', true); HRM.enable('txtOverTimeDate', true); }
            }
            if (key === 'Edit') grdDetails_DoubleClick(i);
        }
        function grdDetails_DoubleClick(i) {
            var item = dtdetail[i];
            if (item && !HRM.bool(item.EntryByLoader)) {
                updateDetailIndex = i;
                HRM.setCombo('cmbEmployee', item.EmployeeId);
                HRM.setVal('txtFromDateTime', HRM.time(item.FromDateTime));
                HRM.setVal('txtToDateTime', HRM.time(item.ToDateTime));
                HRM.setVal('txtOTHours', HRM.str(item.OTHours));
                HRM.setVal('txtOTRate', HRM.str(item.OTRate));
                HRM.check('IsMealAllow', HRM.bool(item.IsAllowMeal));
                detailButtons(true);
            }
        }
        function detailButtons(updating) {
            HRM.show('btnplus', !updating); HRM.show('btnUpdateDetail', updating); HRM.show('btnCancelUpdateDetial', updating);
        }
        function DetailFormValidation() {
            if (HRM.comboVal('cmbEmployee') === 0) { HRM.box('Employee Required'); HRM.focus('cmbEmployee'); return false; }
            return true;
        }
        function ResetDetail() {           // ResetDetail() - it also empties lstRemoveDetailRecord, as the desktop does
            lstRemoveDetailRecord = [];
            HRM.setCombo('cmbEmployee', 0);
            HRM.setVal('txtFromDateTime', nowTime());       // DateTimePicker.Text = "" -> Now
            HRM.setVal('txtToDateTime', nowTime());
            HRM.setVal('txtOTHours', ''); HRM.setVal('txtOTRate', '');
            HRM.check('IsMealAllow', false);
        }
        function AddtoGrid() {
            if (dtdetail.length > 0 && HRM.bool(dtdetail[0].EntryByLoader)) { HRM.box("U Can't add Direct Entry because Entry From Loader Exist"); return; }
            if (!DetailFormValidation()) return;
            dtdetail.push({ Id: 0, EmployeeId: HRM.comboVal('cmbEmployee'), EmployeeName: HRM.comboText('cmbEmployee'),
                FromDateTime: HRM.val('txtFromDateTime'), ToDateTime: HRM.val('txtToDateTime'), OTHours: 0, OTRate: 0,
                IsAllowMeal: HRM.checked('IsMealAllow'), EntryByLoader: false });
            grdSettings();
            ResetDetail();
            HRM.focus('cmbEmployee');
            HRM.enable('cmbDepartmnet', false);
        }
        P.btnplus = function () { AddtoGrid(); };                                       // btnplus_Click
        P.btnUpdateDetail = function () {                                               // btnUpdateDetail_Click
            if (!DetailFormValidation()) return;
            var r = dtdetail[updateDetailIndex];
            if (r) {
                r.EmployeeId = HRM.comboVal('cmbEmployee');
                r.EmployeeName = HRM.comboText('cmbEmployee');
                r.FromDateTime = HRM.val('txtFromDateTime');
                r.ToDateTime = HRM.val('txtToDateTime');
                r.OTHours = HRM.num(HRM.val('txtOTHours'));
                r.OTRate = HRM.num(HRM.val('txtOTRate'));
                r.IsAllowMeal = HRM.checked('IsMealAllow');
                grd.draw();
            }
            detailButtons(false);
            ResetDetail();
            HRM.focus('cmbEmployee');
        };
        P.btnCancelUpdateDetial = function () { detailButtons(false); ResetDetail(); };  // btnCancelUpdateDetial_Click

        // ---------------------------------------------------------------- header
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnUpdate', update); }
        function Reset() {                 // Reset() - the two dates keep their values
            RecId = 0;
            HRM.setCombo('cmbDepartmnet', 0); HRM.setCombo('cmbSection', 0);
            HRM.check('IsOffDuty', false);
            HRM.setVal('txtReason', '');
            HRM.setCombo('cmbRequestedBy', 0);
            HRM.enable('cmbDepartmnet', true); HRM.enable('txtOverTimeDate', true);
            dtdetail.length = 0;
            grdSettings();
            buttons(false);
            lstRemoveDetailRecord = [];
            HRM.setCombo('cmbEmployee', 0);
            HRM.setVal('txtFromDateTime', nowTime()); HRM.setVal('txtToDateTime', nowTime());
            HRM.setVal('txtOTHours', ''); HRM.setVal('txtOTRate', '');
            HRM.check('IsMealAllow', false);
            detailButtons(false);
            updateDetailIndex = -1;
            HRM.focus('txtRequestDate');
        }
        function FormValidation() {
            if (HRM.comboVal('cmbDepartmnet') === 0) { HRM.box('Department Required'); HRM.focus('cmbDepartmnet'); return false; }
            if (HRM.comboVal('cmbSection') === 0) { HRM.box('Section Required'); HRM.focus('cmbSection'); return false; }
            if (HRM.val('txtReason') === '') { HRM.box('Reason Required'); HRM.focus('txtReason'); return false; }
            if (HRM.comboVal('cmbRequestedBy') === 0) { HRM.box('Requested By Required'); HRM.focus('cmbRequestedBy'); return false; }
            return true;
        }
        function rowDto(r) {
            return { id: HRM.int(r.Id), employeeId: HRM.int(r.EmployeeId), employeeName: HRM.str(r.EmployeeName),
                fromDateTime: HRM.str(r.FromDateTime), toDateTime: HRM.str(r.ToDateTime), otHours: HRM.num(r.OTHours),
                otRate: HRM.num(r.OTRate), isAllowMeal: HRM.bool(r.IsAllowMeal), entryByLoader: HRM.bool(r.EntryByLoader) };
        }
        function save(btn) {               // btnsave_Click
            if (!FormValidation()) return;
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var body = {
                id: RecId, requestDate: HRM.val('txtRequestDate'), overTimeDate: HRM.val('txtOverTimeDate'),
                departmentId: HRM.comboVal('cmbDepartmnet'), sectionId: HRM.comboVal('cmbSection'),
                isOffDuty: HRM.checked('IsOffDuty'), reason: HRM.val('txtReason'), requestById: HRM.comboVal('cmbRequestedBy'),
                details: dtdetail.map(rowDto), removed: lstRemoveDetailRecord.map(rowDto)
            };
            var wasUpdate = RecId > 0;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (wasUpdate ? 'Data Update Successfully' : 'Data Save Successfully'));
                    Reset();
                    if (HRM.checked('ChkBox')) OverTimeSlip(HRM.int(d.id));
                }).catch(HRM.fail);
            }, 'ot-request-save');
        }
        P.btnsave = function (btn) {
            var b = HRM.$('btnsave');
            if (b && b.disabled && !btn) return;
            return save(btn || 'btnsave');
        };
        P.btnUpdate = function (btn) {                                                  // btnUpdate_Click
            if (RecId === 0) { HRM.box("Record Can't Update because no Id found"); return; }
            return save(btn || 'btnUpdate');
        };
        P.btnnew = function () { Reset(); ResetDetail(); };                             // btnnew_Click
        P.btnRefresh = function (btn) {                                                 // btnRefresh_Click_1
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/combos').then(fillCombos).catch(HRM.fail);
            });
        };
        P.attachment = function () {                                                    // toolStripButton2_Click -> AT.Show()
            HRM.box('Attachment is the desktop Attachment form (local file dialog and AttachmentFolderPath copy); it is not available in the browser.');
        };

        // ---------------------------------------------------------------- ReadById
        function ReadById(id, btn) {
            RecId = id;
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                buttons(true);
                HRM.setVal('txtRequestDate', HRM.day(o.RequestDate));
                HRM.setVal('txtOverTimeDate', HRM.day(o.OverTimeDate));
                HRM.setCombo('cmbDepartmnet', o.DepartmentId);
                cmbDepartmnet_Leave();
                HRM.setCombo('cmbSection', o.SectionId);
                HRM.check('IsOffDuty', HRM.bool(o.IsOffDuty));
                HRM.setVal('txtReason', HRM.str(o.Reason));
                HRM.setCombo('cmbRequestedBy', o.RequestById);
                dtdetail.length = 0;
                (o.details || []).forEach(function (r) { dtdetail.push(r); });
                grdSettings();
            }).catch(HRM.fail);
        }

        // ---------------------------------------------------------------- prints
        /** CommonServices.OverTimeSlip(PrintId): 0 or no rows -> "No Record Found For Display", else 1010-OverTime_Slip.rpt. */
        function OverTimeSlip(id, btn) {
            if (!id) { HRM.box('No Record Found For Display'); return Promise.resolve(); }
            var w = window.CrystalPrint ? CrystalPrint.reserve() : null;
            return HRM.busy(btn || null, function () {
                return HRM.get(API + '/slip-check', { id: id }).then(function () {
                    if (window.CrystalPrint) return CrystalPrint.open('hrm-1010', { id: id }, null, w);
                    HRM.box('Report file 1010-OverTime_Slip.rpt is not available.');
                }).catch(function (e) { if (window.CrystalPrint) CrystalPrint.release(w); HRM.fail(e); });
            }, btn ? null : 'ot-slip-' + id);
        }
        P.btnprint = function (btn) { return OverTimeSlip(RecId, btn || 'btnprint'); };  // btnprint_Click -> GenerateOTSlip(RecId)

        // ---------------------------------------------------------------- loader (EmpOverTimeLoadForRequest)
        P.btnLoader = function () {                                                     // btnLoader_Click
            if (dtdetail.length === 0 || HRM.bool(dtdetail[0].EntryByLoader)) {
                openLoader().then(LoadInGridDetail);
            } else {
                HRM.box("You Can't Load Because Direct Entry Already Exist");
            }
        };
        function LoadInGridDetail(dt) {                                                 // LoadInGridDetail()
            if (dt && dt.length > 0) {
                if (dtdetail.length > 0) {
                    if (HRM.val('txtOverTimeDate') !== HRM.day(dt[0].DutyDate)) { HRM.box('Already Loaded Rows have different overtime Date'); return; }
                    if (HRM.comboVal('cmbDepartmnet') !== HRM.int(dt[0].DepartmentId)) { HRM.box('Already Loaded Rows have different Department'); return; }
                }
                HRM.enable('txtOverTimeDate', false);
                HRM.setVal('txtOverTimeDate', HRM.day(dt[0].DutyDate));
                HRM.enable('cmbDepartmnet', false);
                HRM.setCombo('cmbDepartmnet', HRM.int(dt[0].DepartmentId));
                dt.forEach(function (x) {
                    var flag = dtdetail.some(function (r) {
                        return HRM.int(r.EmployeeId) === HRM.int(x.EmployeeId) && HRM.val('txtOverTimeDate') === HRM.day(x.DutyDate);
                    });
                    if (!flag) dtdetail.push({ Id: 0, EmployeeId: x.EmployeeId, EmployeeName: x.EmployeeName, FromDateTime: x.OTInTime,
                        ToDateTime: x.OTOutTime, OTHours: x.OverTime, OTRate: 0, IsAllowMeal: false, EntryByLoader: true });
                });
            }
            grdSettings();
        }
        /** The ShowDialog: resolves with dtLoader (the checked rows, as the procedure returned them) when it closes. */
        function openLoader() {
            return new Promise(function (resolve) {
                var dtGrid = [], dtLoader = [], lastArgs = null;
                var today = HRM.today();
                var m = HRM.modal({
                    title: 'EccountBook', width: 'min(1180px, 98vw)',
                    onClose: function () { resolve(dtLoader); },
                    html:
                        '<div class="win-tool-strip">' +
                        '<button type="button" class="win-btn-tool" id="ldNew"><i class="fa fa-plus"></i> <span><u>N</u>ew</span></button>' +
                        '<button type="button" class="win-btn-tool" id="ldRefresh"><i class="fa fa-refresh"></i> <span><u>R</u>efresh</span></button>' +
                        '<button type="button" class="win-btn-tool" id="ldPrint"><i class="fa fa-print"></i> <span>1009-Print</span></button>' +
                        '<button type="button" class="win-btn-tool" id="ldShortcut"><i class="fa fa-keyboard-o"></i> <span>ShortCut Keys</span></button></div>' +
                        '<div class="hrm-caption">Employee OverTime Register</div>' +
                        '<fieldset class="hrm-group"><legend>Filters</legend><div class="ot-loader-filters">' +
                        '<div class="hrm-field"><label for="lddatefrom">From Date</label><input type="date" id="lddatefrom" class="win-textbox" value="' + today + '"></div>' +
                        '<div class="hrm-field"><label for="lddateto">To Date</label><input type="date" id="lddateto" class="win-textbox" value="' + today + '"></div>' +
                        '<div class="hrm-field"><label for="ldcmbemployee">Employee</label><select id="ldcmbemployee" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                        '<div class="hrm-field"><label for="ldcmbDepartment">Department</label><select id="ldcmbDepartment" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                        '<div class="hrm-actions"><button type="button" class="win-btn-action" id="ldShow">Show</button>' +
                        '<button type="button" class="win-btn-action" id="ldLoad">Load</button></div></div></fieldset>' +
                        '<div class="hrm-grid-box hrm-grid-box-modal"><div class="hrm-subcaption"><span>History</span>' +
                        '<button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button></div>' +
                        '<div class="hrm-grid-wrap"><table id="ldgrd"></table></div></div>'
                });
                HRM.wireFullscreen(m.el);
                var grid = new HRM.Grid('ldgrd', {
                    filterRow: true, totals: true, checkAll: 'Select',
                    columns: [
                        { key: 'Select', caption: '', type: 'edit-check', width: 30 },
                        { key: 'Id', caption: 'Id', hidden: true },
                        { key: 'DutyDate', caption: 'DutyDate', type: 'date' },
                        { key: 'ShiftName', caption: 'ShiftName' },
                        { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'int' },
                        { key: 'EmployeeName', caption: 'EmployeeName' },
                        { key: 'DepartmentId', caption: 'DepartmentId', hidden: true },
                        { key: 'DepartmentName', caption: 'DepartmentName' },
                        { key: 'DesignationName', caption: 'DesignationName' },
                        { key: 'TotalSalary', caption: 'TotalSalary', align: 'right', render: function (v) { return HRM.esc(n3(v, true)); } },
                        { key: 'DutyInTime', caption: 'DutyInTime', render: function (v) { return HRM.esc(t12(v)); } },
                        { key: 'DutyOutTime', caption: 'DutyOutTime', render: function (v) { return HRM.esc(t12(v)); } },
                        { key: 'InTime', caption: 'InTime', render: function (v) { return HRM.esc(t12(v)); } },
                        { key: 'OutTime', caption: 'OutTime', render: function (v) { return HRM.esc(t12(v)); } },
                        { key: 'TimeDuration', caption: 'TimeDuration' },
                        { key: 'Actual_Hour', caption: 'Actual_Hour', type: 'int' },
                        { key: 'Working_Hour', caption: 'Working_Hour', type: 'int' },
                        { key: 'OT', caption: 'OT', align: 'right', sum: true, decimals: 0, render: function (v) { return HRM.esc(HRM.str(v)); } },
                        { key: 'OTHM', caption: 'OTHM' },
                        { key: 'OT Rate', caption: 'OT Rate', align: 'right', render: function (v) { return HRM.esc(n3(v, true)); } },
                        { key: 'OT Amount', caption: 'OT Amount', align: 'right', sum: true, decimals: 3, render: function (v) { return HRM.esc(n3(v, true)); } }
                    ]
                });
                function loaderFill(d, keepDept) {                              // DepartmentFill (ZeroIndex false) / EmployeeFill
                    if (!keepDept) HRM.fill('ldcmbDepartment', d.departments || [], 'Id', 'name', { zero: '' });
                    HRM.fill('ldcmbemployee', d.employees || [], 'EmployeeId', 'EmployeeName', { zero: '' });
                }
                function GridHistory(btn) {                                     // btnshow_Click -> GridHistory()
                    var args = { fromDate: HRM.val('lddatefrom'), toDate: HRM.val('lddateto'),
                        employeeId: HRM.comboVal('ldcmbemployee'), departmentId: HRM.comboVal('ldcmbDepartment') };
                    return HRM.busy(btn || 'ldShow', function () {
                        return HRM.get(API + '/loader-show', args).then(function (rows) {
                            dtGrid = rows || []; lastArgs = args;
                            grid.set(dtGrid.map(function (x) {
                                return { _src: x, Select: false, Id: x.DutyRoasterDetailId, DutyDate: x.DutyDate, ShiftName: x.ShiftName,
                                    EmployeeNo: x.EmployeeNo, EmployeeName: x.EmployeeName, DepartmentId: x.DepartmentId,
                                    DepartmentName: x.DepartmentName, DesignationName: x.DesignationName, TotalSalary: x.TotalSalary,
                                    DutyInTime: x.DutyInTime, DutyOutTime: x.DutyOutTime, InTime: x.InTime, OutTime: x.OutTime,
                                    TimeDuration: x.TimeDuration, Actual_Hour: x.Actual_HOUR, Working_Hour: x.EMPLOYEE_WORKINGHOUR,
                                    OT: x.OverTime, OTHM: x.OTHM, 'OT Rate': x['OT Rate'],
                                    'OT Amount': HRM.num(x['OT Rate']) * HRM.num(x.OverTime) };
                            }));
                        }).catch(HRM.fail);
                    });
                }
                function btnLoad_Click() {
                    var checkedRows = grid.rows().filter(function (r) { return HRM.bool(r.Select); });
                    if (checkedRows.length === 0) { HRM.box('Chek the row first'); return; }
                    var DutyDate = null, departmentId = 0, out = [];
                    for (var i = 0; i < checkedRows.length; i++) {
                        var r = checkedRows[i], date = HRM.day(r.DutyDate), depId = HRM.int(r.DepartmentId);
                        if (DutyDate === null) DutyDate = date;
                        if (departmentId === 0) departmentId = depId;
                        if (DutyDate !== date) { HRM.box('Sorry Check Rows Which Have Same Duty Date'); return; }
                        if (departmentId !== depId) { HRM.box('Sorry Check Rows Which Have Same Department'); return; }
                        out.push(r._src);
                    }
                    dtLoader = out;
                    m.close();                                                  // Hide() -> ShowDialog returns
                }
                function print(btn) {                                           // print_Click: 1009-EmployeeOverTimeRegister.rpt of dtGrid
                    if (!dtGrid.length || !lastArgs) { HRM.box('Record Not Found For Display'); return; }
                    if (window.CrystalPrint) CrystalPrint.open('hrm-1009', lastArgs, btn);
                    else HRM.box('Report file 1009-EmployeeOverTimeRegister.rpt is not available.');
                }
                function newFilters() { HRM.setCombo('ldcmbemployee', 0); HRM.setCombo('ldcmbDepartment', 0); }   // btnNew_Click
                function refresh(btn) {                                         // toolStripButton1_Click: DepartmentFill(); EmployeeFill()
                    return HRM.busy(btn || 'ldRefresh', function () {
                        return HRM.get(API + '/loader-setup', { departmentId: 0 }).then(function (d) { loaderFill(d); }).catch(HRM.fail);
                    });
                }
                function shortcuts() {
                    shortcutPopup([['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
                        ['Ctrl+F5', 'For Focus on DateType'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                        ['Ctrl+ArrowUp', 'For Focus On Date Type'], ['Ctrl+ArrowDown', 'For Focus On Grid']]);
                }
                m.el.querySelector('#ldShow').addEventListener('click', function () { GridHistory(this); });
                m.el.querySelector('#ldLoad').addEventListener('click', btnLoad_Click);
                m.el.querySelector('#ldPrint').addEventListener('click', function () { print(this); });
                m.el.querySelector('#ldNew').addEventListener('click', newFilters);
                m.el.querySelector('#ldRefresh').addEventListener('click', function () { refresh(this); });
                m.el.querySelector('#ldShortcut').addEventListener('click', shortcuts);
                dialogKeys(m.el, function (e) {                                 // EmployeeOverTimeRegister_KeyDown
                    var k = keyOf(e);
                    if (isCtrlAlt(e)) { e.preventDefault(); e.stopPropagation(); shortcuts(); }
                    else if (k === 'ctrl+s') { e.preventDefault(); e.stopPropagation(); GridHistory(); }
                    else if (k === 'ctrl+n') { e.preventDefault(); e.stopPropagation(); newFilters(); }
                    else if (k === 'ctrl+p') { e.preventDefault(); e.stopPropagation(); print(m.el.querySelector('#ldPrint')); }
                    else if (k === 'ctrl+f5' || k === 'ctrl+arrowup') { e.preventDefault(); e.stopPropagation(); HRM.focus('lddatefrom'); }
                    else if (k === 'ctrl+arrowdown') { e.preventDefault(); e.stopPropagation(); focusGrid('ldgrd'); }
                });
                HRM.loading(HRM.get(API + '/loader-setup', { departmentId: 0 })).then(function (d) {   // DailyAttendanceRpt_Load
                    loaderFill(d); HRM.focus('lddatefrom');
                }).catch(HRM.fail);
            });
        }

        // ---------------------------------------------------------------- History tab (dialog)
        function historyState() {
            if (hist) return hist;
            var days = defaultDays > 0 ? defaultDays : 3;
            hist = { from: HRM.addDays(HRM.today(), -days), to: HRM.today(), fromOn: true, toOn: true,
                mode: 'requested', sectionId: 0, departmentId: 0, requestedById: 0 };
            return hist;
        }
        function openHistory() {
            var h = historyState(), detailGrid = null, token = 0;
            var radios = [['requested', 'Requested Date'], ['overtime', 'Over Time Date'], ['entry', 'Entry Date'], ['modify', 'Modify Date'], ['approved', 'Approved Date']];
            var H = HRM.history({
                title: 'Over Time History',
                filters:
                    '<div class="win-tool-strip ot-history-tools">' +
                    '<button type="button" class="win-btn-tool" id="hNew"><i class="fa fa-plus"></i> <span>New</span></button>' +
                    '<button type="button" class="win-btn-tool" id="hRefresh"><i class="fa fa-refresh"></i> <span>Refresh</span></button></div>' +
                    '<fieldset class="hrm-group" style="width:100%;margin:0"><legend>Filters</legend><div class="ot-history-filters">' +
                    '<div class="hrm-field"><label for="Requesteddatefrom">Date From</label><span class="ot-datecheck"><input type="checkbox" id="RequesteddatefromOn" title="Use this date"><input type="date" id="Requesteddatefrom" class="win-textbox"></span></div>' +
                    '<div class="hrm-field"><label for="RequestedDateTo">Date To</label><span class="ot-datecheck"><input type="checkbox" id="RequestedDateToOn" title="Use this date"><input type="date" id="RequestedDateTo" class="win-textbox"></span></div>' +
                    '<div class="hrm-field"><label for="cmbsectionhistory">Section</label><select id="cmbsectionhistory" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                    '<div class="hrm-field"><label for="CmbDepartmentHistory">Department</label><select id="CmbDepartmentHistory" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                    '<div class="hrm-field"><label for="cmbrequestedbthistory">Requested By</label><select id="cmbrequestedbthistory" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                    '<div class="ot-radios">' + radios.map(function (r) {
                        return '<label class="hrm-check"><input type="radio" name="otHistMode" value="' + r[0] + '"> ' + r[1] + '</label>';
                    }).join('') + '</div></div></fieldset>',
                columns: [
                    { key: 'Edit', caption: 'Edit', width: 50, type: 'code', align: 'center' },
                    { key: 'Print', caption: 'Print', width: 50, align: 'center', render: function () { return '<button type="button" class="win-btn-action ot-print-btn">Print</button>'; } },
                    { key: 'OverTimeRequestId', caption: 'OverTimeRequestId', hidden: true },
                    { key: 'RequestDate', caption: 'RequestDate', type: 'date' },
                    { key: 'OverTimeDate', caption: 'OverTimeDate', type: 'date' },
                    { key: 'DepartmentName', caption: 'DepartmentName' },
                    { key: 'SectionName', caption: 'SectionName' },
                    { key: 'IsOffDuty', caption: 'IsOffDuty' },
                    { key: 'Reason', caption: 'Reason' },
                    { key: 'RequestedByPerson', caption: 'RequestedByPerson' },
                    { key: 'EntryUser', caption: 'EntryUser' },
                    { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(dmy12(v)); } },
                    { key: 'ModifyUser', caption: 'ModifyUser' },
                    { key: 'ModifyDate', caption: 'ModifyDate', render: function (v) { return HRM.esc(dmy12(v)); } },
                    { key: 'ApprovedUser', caption: 'ApprovedUser' },
                    { key: 'ApprovedDate', caption: 'ApprovedDate', render: function (v) { return HRM.esc(dmy12(v)); } },
                    { key: 'NoOfAttachment', caption: 'NoOfAttachment', type: 'int' }
                ],
                onReady: function (body, g) {
                    body.querySelector('.hrm-history-refresh').textContent = 'Show';     // btnShow
                    HRM.fill('cmbsectionhistory', sections, 'Id', 'Name', { zero: '...Select Any Value...' });
                    HRM.fill('CmbDepartmentHistory', departments, 'Id', 'Name', { zero: '...Select Any Value...' });
                    HRM.fill('cmbrequestedbthistory', dtEmployees, 'EmployeeId', 'EmployeeName', { zero: '...Select Any Value...' });
                    restore(body);
                    var box = document.createElement('div');                            // panel7 + grdHistoryDetail
                    box.className = 'hrm-grid-box hrm-grid-short';
                    box.innerHTML = '<div class="hrm-subcaption"><span>Detail Of Above Selected Row</span></div><div class="hrm-grid-wrap"><table id="grdHistoryDetail"></table></div>';
                    body.appendChild(box);
                    detailGrid = new HRM.Grid('grdHistoryDetail', {
                        filterRow: true,
                        columns: [
                            { key: 'EmployeeName', caption: 'EmployeeName' },
                            { key: 'FromDateTime', caption: 'From Time', render: function (v) { return HRM.esc(t12(v)); } },
                            { key: 'ToDateTime', caption: 'To Time', render: function (v) { return HRM.esc(t12(v)); } },
                            { key: 'OTHours', caption: 'OTHours', hidden: true },
                            { key: 'OTRate', caption: 'OTRate', hidden: true },
                            { key: 'IsAllowMeal', caption: 'IsAllowMeal', hidden: true }
                        ]
                    });
                    g.opts.onSelect = function (r) {                                     // DataGridHistory_SelectionChanged
                        var my = ++token;
                        HRM.get(API + '/history-detail', { id: r.OverTimeRequestId }).then(function (rows) {
                            if (my === token) detailGrid.set(rows || []);
                        }).catch(HRM.fail);
                    };
                    g.table.addEventListener('click', function (e) {                    // DataGridHistory_ColumnButtonClick "Print"
                        var b = e.target.closest('.ot-print-btn'); if (!b) return;
                        var tr = b.closest('tr[data-i]'); if (!tr) return;
                        var r = g.data[+tr.getAttribute('data-i')];
                        OverTimeSlip(HRM.int(r.OverTimeRequestId), b);
                    });
                    body.querySelector('#hNew').addEventListener('click', function () { historyNew(body, g); });
                    body.querySelector('#hRefresh').addEventListener('click', function () {   // btnhistoryrefresh_Click
                        var b = this;
                        HRM.busy(b, function () {
                            return HRM.get(API + '/combos').then(function (d) {
                                fillCombos(d);
                                HRM.fill('cmbsectionhistory', sections, 'Id', 'Name', { zero: '...Select Any Value...', keep: true });
                                HRM.fill('CmbDepartmentHistory', departments, 'Id', 'Name', { zero: '...Select Any Value...', keep: true });
                                HRM.fill('cmbrequestedbthistory', dtEmployees, 'EmployeeId', 'EmployeeName', { zero: '...Select Any Value...', keep: true });
                            }).catch(HRM.fail);
                        });
                    });
                    dialogKeys(body.closest('.hrm-modal'), function (e) {   // FrmExportSalesContract_KeyDown, History tab
                        var k = keyOf(e);
                        if (isCtrlAlt(e)) { e.preventDefault(); e.stopPropagation(); P.shortcuts(); }
                        else if (k === 'ctrl+s') { e.preventDefault(); e.stopPropagation(); H.reload(); }
                        else if (k === 'ctrl+n') { e.preventDefault(); e.stopPropagation(); historyNew(body, g); }
                        else if (k === 'ctrl+enter') { e.preventDefault(); e.stopPropagation(); var c = g.current(); if (c) { H.modal.close(); pick(c); } }
                        else if (k === 'ctrl+arrowdown') { e.preventDefault(); e.stopPropagation(); var tr = g.table.querySelector('tbody tr[data-i]'); if (tr) tr.focus(); }
                        else if (k === 'ctrl+arrowup') { e.preventDefault(); e.stopPropagation(); HRM.focus('Requesteddatefrom'); }
                        else if (k === 'ctrl+t') { e.preventDefault(); e.stopPropagation(); H.modal.close(); HRM.focus('txtRequestDate'); }
                    });
                    HRM.focus('Requesteddatefrom');                                      // tabControl1_SelectedIndexChanged
                },
                load: function (body) {                                                     // btnShow_Click -> HistoryFill
                    remember(body);
                    return HRM.get(API + '/history', historyArgs()).then(function (rows) {
                        if (detailGrid) detailGrid.set([]);
                        return (rows || []).map(function (r) { r.Edit = 'Edit'; return r; });
                    });
                },
                onPick: function (r) { pick(r); }
            });
            function pick(r) { ReadById(HRM.int(r.OverTimeRequestId)); }                   // DataGridHistory Edit / DoubleClick -> ReadById
            function restore(body) {
                HRM.setVal('Requesteddatefrom', h.from); HRM.setVal('RequestedDateTo', h.to);
                HRM.check('RequesteddatefromOn', h.fromOn); HRM.check('RequestedDateToOn', h.toOn);
                HRM.setCombo('cmbsectionhistory', h.sectionId); HRM.setCombo('CmbDepartmentHistory', h.departmentId);
                HRM.setCombo('cmbrequestedbthistory', h.requestedById);
                body.querySelectorAll('input[name=otHistMode]').forEach(function (x) { x.checked = x.value === h.mode; });
            }
            function remember(body) {
                h.from = HRM.val('Requesteddatefrom'); h.to = HRM.val('RequestedDateTo');
                h.fromOn = HRM.checked('RequesteddatefromOn'); h.toOn = HRM.checked('RequestedDateToOn');
                h.sectionId = HRM.comboVal('cmbsectionhistory'); h.departmentId = HRM.comboVal('CmbDepartmentHistory');
                h.requestedById = HRM.comboVal('cmbrequestedbthistory');
                var r = body.querySelector('input[name=otHistMode]:checked'); h.mode = r ? r.value : 'requested';
            }
            function historyArgs() {
                return { mode: h.mode, from: h.fromOn ? h.from : '', to: h.toOn ? h.to : '',
                    sectionId: h.sectionId, departmentId: h.departmentId, requestedById: h.requestedById };
            }
            function historyNew(body, g) {                                                   // toolStripButton1_Click
                h.from = HRM.today(); h.to = HRM.today(); h.fromOn = true; h.toOn = true;
                h.sectionId = 0; h.departmentId = 0; h.requestedById = 0; h.mode = 'requested';
                restore(body);
                g.set([]); if (detailGrid) detailGrid.set([]);
            }
            return H;
        }

        // ---------------------------------------------------------------- shortcuts / keys / load
        P.shortcuts = function () {                                                      // btnshortcutkeys_Click -> MakeShortCutKeys
            shortcutPopup([['Ctrl+S', 'For Save in Form Tab and For Show History in History Tab'], ['Ctrl+U', 'For Update'],
                ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on Requested Date'],
                ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                ['Ctrl+ArrowDown', 'For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab'],
                ['Ctrl+ArrowUp', "For Focus on Employee in Detail Grid when in Form tab and For focus on FromDate in history tab"],
                ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
                ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        };
        function canClick(id) { var b = HRM.$(id); return b && HRM.visible(id) && !b.disabled; }
        HRM.keys({                                                                       // FrmExportSalesContract_KeyDown, Form tab
            'ctrl+t': function () { openHistory(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+alt+control': P.shortcuts, 'ctrl+alt+alt': P.shortcuts,
            'ctrl+s': function () { if (canClick('btnsave')) P.btnsave(); },
            'ctrl+u': function () { if (canClick('btnUpdate')) P.btnUpdate(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+enter': function () { if (grd && grd.currentIndex() >= 0) grdDetails_DoubleClick(grd.currentIndex()); },
            'ctrl+p': function () { if (canClick('btnprint')) P.btnprint(); },
            'ctrl+f10': function () { P.attachment(); },
            'ctrl+r': function () { P.btnRefresh(); },
            'ctrl+f5': function () { HRM.focus('txtRequestDate'); },
            'ctrl+arrowup': function () { HRM.focus('cmbEmployee'); },
            'ctrl+arrowdown': function () { focusGrid('grdDetails'); }
        });
        HRM.$('cmbDepartmnet').addEventListener('change', cmbDepartmnet_Leave);          // cmbDepartmnet_Leave
        HRM.footer(function () { openHistory(); });                                      // History tab
        HRM.setVal('txtRequestDate', HRM.today()); HRM.setVal('txtOverTimeDate', HRM.today());
        HRM.setVal('txtFromDateTime', nowTime()); HRM.setVal('txtToDateTime', nowTime());
        grdSettings();
        buttons(false);
        HRM.fill('cmbEmployee', [], 'EmployeeId', 'EmployeeName', { zero: '' });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // FrmExportSalesContract_Load
            rights = d.rights || {};
            defaultDays = HRM.int(d.defaultDays);
            fillCombos(d);
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnUpdate', print: 'btnprint' });
            HRM.check('ChkBox', rights.print !== false);                                 // ChkBox.Checked = DoHavePrintRights
            HRM.focus('txtRequestDate');
        }).catch(HRM.fail);
    }

    // ============================================================================ 666 Employee Over Time
    var MONTHS = [[1, 'January'], [2, 'February'], [3, 'March'], [4, 'April'], [5, 'May'], [6, 'June'], [7, 'July'],
        [8, 'August'], [9, 'September'], [10, 'October'], [11, 'November'], [12, 'December']];   // CommonServices.GetMonths

    function employeeOverTime() {
        var RecId = 0, detailList = [], histCombos = { months: [], years: [], employees: [] };
        var hist = { month: 0, year: 0, employeeId: 0 };

        var grdEmployeeOt = new HRM.Grid('grdEmployeeOt', {
            filterRow: true,
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', type: 'code' },
                { key: 'Designation', caption: 'Designation' },
                { key: 'Branch', caption: 'Branch' },
                { key: 'Department', caption: 'Department' },
                { key: 'Section', caption: 'Section' },
                { key: 'Mobile', caption: 'Mobile' }
            ],
            onCode: function (r) { grdEmployeeOt_LinkClicked(r); }
        });
        var grdEmployeeDetail = new HRM.Grid('grdEmployeeDetail', {
            filterRow: true, checkAll: 'Select',
            columns: [
                { key: 'Select', caption: '', type: 'edit-check', width: 40 },
                { key: 'EmpOvertimeId', caption: 'EmpOvertimeId', hidden: true },
                { key: 'OvertimeDetailId', caption: 'OvertimeDetailId', hidden: true },
                { key: 'EmpAttendanceId', caption: 'EmpAttendanceId', hidden: true },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'OvertimeMonth', caption: 'OvertimeMonth', hidden: true },
                { key: 'OvertimeYear', caption: 'OvertimeYear', hidden: true },
                { key: 'Employee', caption: 'Employee' },
                { key: 'Date', caption: 'Date', type: 'int' },
                { key: 'Day', caption: 'Day' },
                { key: 'FromTime', caption: 'FromTime', render: function (v) { return HRM.esc(t12(v)); } },
                { key: 'ToTime', caption: 'ToTime', render: function (v) { return HRM.esc(t12(v)); } },
                { key: 'Timing', caption: 'Timing' },
                { key: 'WorkingHour', caption: 'WorkingHour', align: 'right', render: function (v) { return HRM.esc(HRM.str(v)); } },
                { key: 'ReqOTHours', caption: 'ReqOTHours', hidden: true },
                { key: 'ActualOTHour', caption: 'ActualOTHour', hidden: true },
                { key: 'ShortHours', caption: 'ShortHours', hidden: true },
                { key: 'OTRate', caption: 'OTRate', align: 'right', render: function (v) { return HRM.esc(n3(v)); } },
                { key: 'OTHours', caption: 'OTHours', align: 'right', render: function (v) { return HRM.esc(HRM.str(v)); } },
                { key: 'AddLess', caption: 'AddLess', type: 'edit', align: 'right', onChange: grdEmployeeDetail_CellUpdated },
                { key: 'NetOTHours', caption: 'NetOTHours', align: 'right', render: function (v) { return HRM.esc(HRM.str(v)); } },
                { key: 'Amount', caption: 'Amount', align: 'right', render: function (v) { return HRM.esc(n3(v)); } }
            ]
        });

        /** grdEmployeeDetail_UpdatingCell + grdEmployeeDetail_CellUpdated (AddLess). */
        function grdEmployeeDetail_CellUpdated(item, key, i, el) {
            var s = HRM.str(item.AddLess).trim();
            if (s !== '' && !/^[+-]?(\d+\.?\d*|\.\d+)$/.test(s)) {
                HRM.box('Please Type Only Numeric Value');
                item.AddLess = item._addLessPrev;                       // the Double cell refuses the text
                if (el) el.value = HRM.str(item._addLessPrev);
                return;
            }
            if (s === '') { item.AddLess = null; item._addLessPrev = null; return; }
            var AddLess = HRM.num(s), OTRate = HRM.num(item.OTRate), OTHours = HRM.num(item.OTHours);
            var NetOt = OTHours + AddLess;
            item.AddLess = AddLess; item._addLessPrev = AddLess;
            item.NetOTHours = NetOt;
            item.Amount = HRM.round(OTRate * NetOt, 3);
            grdEmployeeDetail.draw();
        }
        function MonthYearFill(years) {
            HRM.fillFixed('cmbMonth', MONTHS, { zero: '' });             // MonthFill: BindDDL ZeroIndex false
            HRM.fill('cmbYear', years || [], 'Id', 'Year', { zero: '' }); // YearFill
        }
        function Reset() {                                               // Reset()
            RecId = 0;
            HRM.setCombo('cmbMonth', 0); HRM.setCombo('cmbYear', 0);
            detailList = [];
            HRM.text('lblEmpName', '( )');
            HRM.show('btnsave', true); HRM.show('btnupdate', false);
            grdEmployeeDetail.clear();
        }
        function GetEmployeesData(btn) {
            return HRM.busy(btn || null, function () {
                return HRM.get(API + '/employees', { month: HRM.comboVal('cmbMonth'), year: HRM.comboVal('cmbYear') }).then(function (rows) {
                    if (rows && rows.length) grdEmployeeOt.set(rows);
                    else { grdEmployeeOt.clear(); grdEmployeeDetail.clear(); }
                }).catch(HRM.fail);
            }, btn ? null : 'ot-employees');
        }
        function grdEmployeeOt_LinkClicked(row) {
            if (!row) { HRM.text('lblEmpName', '( )'); return; }
            HRM.text('lblEmpName', '( ' + HRM.str(row.EmployeeName) + ' )');
            GetDetailDataByEmployee(HRM.int(row.Id));
        }
        function GetDetailDataByEmployee(empId) {
            HRM.loading(HRM.get(API + '/detail', { employeeId: empId, month: HRM.comboVal('cmbMonth'), year: HRM.comboVal('cmbYear') })).then(function (rows) {
                if (rows && rows.length) {
                    grdEmployeeDetail.set(rows.filter(function (r) {
                        return !detailList.some(function (d) { return d.overTimeRequestDetailId === HRM.int(r.OvertimeDetailId); });
                    }).map(function (r) { r.Select = false; r._addLessPrev = r.AddLess; return r; }));
                } else grdEmployeeDetail.clear();
            }).catch(HRM.fail);
        }
        function addDataToDetailList() {
            grdEmployeeDetail.checked('Select').forEach(function (row) {
                var id = HRM.int(row.OvertimeDetailId);
                if (detailList.some(function (d) { return d.overTimeRequestDetailId === id; })) return;
                detailList.push({ overTimeRequestDetailId: id, employeeAttendanceId: HRM.int(row.EmpAttendanceId), employeeId: HRM.int(row.EmployeeId),
                    month: HRM.int(row.OvertimeMonth), year: HRM.int(row.OvertimeYear), addLess: HRM.num(row.AddLess) });
            });
        }
        P.btnAddEmployeeDetail = function () {                           // btnAddEmployeeDetail_Click
            if (grdEmployeeDetail.checked('Select').length === 0) { HRM.box('No Rows Selected'); return; }
            if (!HRM.ask('Are you sure to Add?')) return;
            addDataToDetailList();
        };
        function Insert(btn) {
            if (detailList.length <= 0) { HRM.box('No Record Found'); return; }
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var wasUpdate = RecId > 0;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', { details: detailList.slice() }).then(function (d) {
                    HRM.box(wasUpdate ? 'Update Successfully' : (d.message || 'Saved Successfully'));
                    Reset();
                    return GetEmployeesData();
                }).catch(HRM.fail);
            }, 'ot-employee-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };             // btnupdate_Click
        P.btnnew = function () { Reset(); };                                             // btnnew_Click
        P.btnShow = function (btn) { return GetEmployeesData(btn || 'btnShow'); };       // btnShow_Click
        P.shortcuts = function () {                                                      // btnShortcutkeys_Click -> MakeShortCutKeys
            shortcutPopup([['Ctrl+S', 'For Save in Form Tab and For Show History in History Tab'], ['Ctrl+E', 'For Close'],
                ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+F5', 'For Focus on Month'], ['Ctrl+T', 'For Tab Transfer'],
                ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                ['Ctrl+ArrowDown', 'For Toggle in Grids in Form tab and for focus on history grid when in history tab'],
                ['Ctrl+ArrowUp', 'For Focus on Month when in Form tab and For focus on Month in history tab'],
                ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        };

        // ---------------------------------------------------------------- History tab (dialog)
        function fillHistoryCombos(keep) {                               // DropDownBindsForHistory (ZeroIndex false)
            HRM.fill('cmbMonthHistory', histCombos.months, 'Id', 'name', { zero: '', keep: keep });
            HRM.fill('cmbYearHistory', histCombos.years, 'Id', 'name', { zero: '', keep: keep });
            HRM.fill('cmbEmployeeHistory', histCombos.employees, 'Id', 'name', { zero: '', keep: keep });
        }
        function monthYearCheck() {
            if (HRM.comboVal('cmbMonthHistory') <= 0) { HRM.focus('cmbMonthHistory'); return 'Month Required'; }
            if (HRM.comboVal('cmbYearHistory') <= 0) { HRM.focus('cmbYearHistory'); return 'Year Required'; }
            return null;
        }
        function openHistory() {
            var first = true;
            var H = HRM.history({
                title: 'Employee Overtime History',
                filters:
                    '<div class="win-tool-strip ot-history-tools">' +
                    '<button type="button" class="win-btn-tool" id="btnNewHistory"><i class="fa fa-plus"></i> <span><u>N</u>ew</span></button>' +
                    '<button type="button" class="win-btn-tool" id="BtnRefreshHistory"><i class="fa fa-refresh"></i> <span>Refresh</span></button>' +
                    '<button type="button" class="win-btn-tool" id="btnPrint1011"><i class="fa fa-print"></i> <span>Print-1011</span></button></div>' +
                    '<div class="ot-filters">' +
                    '<div class="hrm-field"><label for="cmbMonthHistory">Month</label><select id="cmbMonthHistory" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                    '<div class="hrm-field"><label for="cmbYearHistory">Year</label><select id="cmbYearHistory" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                    '<div class="hrm-field ot-wide"><label for="cmbEmployeeHistory">Employee</label><select id="cmbEmployeeHistory" class="win-combo dtcombo" data-dtcombo="single"></select></div></div>',
                columns: [
                    { key: 'Id', caption: 'Id', hidden: true },
                    { key: 'EmployeeName', caption: 'EmployeeName' },
                    { key: 'OverTimeDate', caption: 'OverTimeDate', type: 'date' },
                    { key: 'InTime', caption: 'InTime' },
                    { key: 'OutTime', caption: 'OutTime' },
                    { key: 'TimeDuration', caption: 'TimeDuration' },
                    { key: 'ActualOTHours', caption: 'ActualOTHours', hidden: true },
                    { key: 'OTHours', caption: 'OTHours', align: 'right', render: function (v) { return HRM.esc(HRM.str(v)); } },
                    { key: 'AddLess', caption: 'AddLess', align: 'right', render: function (v) { return HRM.esc(HRM.str(v)); } },
                    { key: 'ApprovedHours', caption: 'ApprovedHours', align: 'right', render: function (v) { return HRM.esc(HRM.str(v)); } },
                    { key: 'OTRate', caption: 'OTRate', align: 'right', render: function (v) { return HRM.esc(n3(v)); } },
                    { key: 'ApprovedAmount', caption: 'ApprovedAmount', align: 'right', render: function (v) { return HRM.esc(n3(v)); } },
                    { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(dmmm12(v)); } },
                    { key: 'EntryUser', caption: 'EntryUser' }
                ],
                onReady: function (body, g) {
                    body.querySelector('.hrm-history-refresh').textContent = 'Show';     // btnShowHistory
                    fillHistoryCombos(false);
                    HRM.setCombo('cmbMonthHistory', hist.month); HRM.setCombo('cmbYearHistory', hist.year); HRM.setCombo('cmbEmployeeHistory', hist.employeeId);
                    function newHistory() {                                              // btnNewHistory_Click
                        HRM.setCombo('cmbMonthHistory', 0); HRM.setCombo('cmbYearHistory', 0); HRM.setCombo('cmbEmployeeHistory', 0);
                        g.set([]); HRM.focus('cmbMonthHistory');
                    }
                    function refresh(b) {                                                // BtnRefreshHistory_Click -> DropDownBindsForHistory
                        return HRM.busy(b || 'BtnRefreshHistory', function () {
                            return HRM.get(API + '/history-combos').then(function (d) {
                                if (d && ((d.months || []).length + (d.years || []).length + (d.employees || []).length) > 0) { histCombos = d; fillHistoryCombos(true); }
                            }).catch(HRM.fail);
                        });
                    }
                    function print1011(b) {                                              // btnPrint1011_Click
                        var msg = monthYearCheck();
                        if (msg) { HRM.box(msg); return; }
                        var args = { month: HRM.comboVal('cmbMonthHistory'), year: HRM.comboVal('cmbYearHistory') };
                        var w = window.CrystalPrint ? CrystalPrint.reserve() : null;
                        return HRM.busy(b, function () {
                            return HRM.get(API + '/slip-check', args).then(function () {
                                if (window.CrystalPrint) return CrystalPrint.open('hrm-1011', args, null, w);
                                HRM.box('Report file 1011-EmployeeOverTimeSlip.rpt is not available.');
                            }).catch(function (e) { if (window.CrystalPrint) CrystalPrint.release(w); HRM.fail(e); });
                        });
                    }
                    body.querySelector('#btnNewHistory').addEventListener('click', newHistory);
                    body.querySelector('#BtnRefreshHistory').addEventListener('click', function () { refresh(this); });
                    body.querySelector('#btnPrint1011').addEventListener('click', function () { print1011(this); });
                    dialogKeys(body.closest('.hrm-modal'), function (e) {   // EmployeeFamilyInfo_KeyDown, History tab
                        var k = keyOf(e);
                        if (isCtrlAlt(e)) { e.preventDefault(); e.stopPropagation(); P.shortcuts(); }
                        else if (k === 'ctrl+s') { e.preventDefault(); e.stopPropagation(); H.reload(); }
                        else if (k === 'ctrl+n') { e.preventDefault(); e.stopPropagation(); newHistory(); }
                        else if (k === 'ctrl+r') { e.preventDefault(); e.stopPropagation(); refresh(); }
                        else if (k === 'ctrl+arrowdown') { e.preventDefault(); e.stopPropagation(); var tr = g.table.querySelector('tbody tr[data-i]'); if (tr) tr.focus(); }
                        else if (k === 'ctrl+arrowup') { e.preventDefault(); e.stopPropagation(); HRM.focus('cmbMonthHistory'); }
                        else if (k === 'ctrl+t') { e.preventDefault(); e.stopPropagation(); H.modal.close(); HRM.focus('cmbMonth'); }
                    });
                    HRM.focus('cmbMonthHistory');                                        // tabControl1_SelectedIndexChanged
                },
                load: function () {                                                      // btnShowHistory_Click
                    if (first) { first = false; return Promise.resolve([]); }            // the tab opens empty until Show
                    var msg = monthYearCheck();
                    if (msg) return Promise.reject(new Error(msg));
                    hist.month = HRM.comboVal('cmbMonthHistory'); hist.year = HRM.comboVal('cmbYearHistory'); hist.employeeId = HRM.comboVal('cmbEmployeeHistory');
                    return HRM.get(API + '/history', { month: hist.month, year: hist.year, employeeId: hist.employeeId });
                }
            });
            return H;
        }

        function toggleGrids() {                                         // Ctrl+Down on the Form tab
            var a = document.activeElement;
            if (a && a.closest && a.closest('#grdEmployeeOt')) focusGrid('grdEmployeeDetail');
            else focusGrid('grdEmployeeOt');
        }
        HRM.keys({                                                       // EmployeeFamilyInfo_KeyDown, Form tab
            'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+arrowdown': toggleGrids,
            'ctrl+arrowup': function () { HRM.focus('cmbMonth'); },
            'ctrl+f5': function () { HRM.focus('cmbMonth'); },
            'ctrl+t': function () { openHistory(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+alt+control': P.shortcuts, 'ctrl+alt+alt': P.shortcuts,
            'ctrl+u': function () { /* the desktop reads btnupdate.Enabled and does nothing */ }
        });
        HRM.footer(function () { openHistory(); });                      // History tab
        MonthYearFill([]);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {         // EmployeeFamilyInfo_Load
            MonthYearFill(d.years || []);
            histCombos = d.history || histCombos;
            HRM.show('btnsave', true); HRM.show('btnupdate', false);
            HRM.focus('cmbMonth');
        }).catch(HRM.fail);
    }

    var PAGES = { 'over-time-request': overTimeRequest, 'employee-over-time': employeeOverTime };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
