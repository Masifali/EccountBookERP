/* ============================================================================================
 * countx_hrm_leave.js - HRM "Leave Management". One script, the page is chosen by
 * <body data-hrm="...">. Built on countx_hrm.js (window.HRM); exports window.HrmLeave.
 *
 *   leave-opening       frmEmployeeLeaveOpening.cs     (660)
 *   employee-leave      frmEmployeeLeaveRequest.cs     (661)
 *   cpl-leave-opening   frmEmployeeCPLLeaveOpening.cs  (662 / 463)
 *   cpl-attendance      frmEmployeeCPLAttendance.cs    (461)
 *   cpl-request         frmEmployeeCPLRequest.cs       (462)
 *
 * Each handler follows its desktop form line by line: validation wording and order, confirmations,
 * messages, what New / Save / Update / Add / double-click do to the buttons and the grids.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/leave/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmLeave
    window.HrmLeave = P;

    // ------------------------------------------------------------------------ shared helpers
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function pad(n) { return String(n).padStart(2, '0'); }

    /** GridEX FormatString "#,#.##": grouped, at most 2 decimals, blank for 0. */
    function fmtHash(v) {
        var n = HRM.round(v, 2);
        if (!n) return '';
        var neg = n < 0, s = String(Math.abs(n)).split('.');
        s[0] = s[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (neg ? '-' : '') + s.join('.');
    }
    /** double / decimal ToString(): the number as it is, no forced decimals. */
    function plain(v) { return v === null || v === undefined || v === '' ? '' : String(HRM.num(v)); }
    /** "HH:mm" of an "HH:mm" string or an ISO date-time. */
    function hm(v) {
        if (v === null || v === undefined || v === '') return '';
        var m = String(v).match(/(\d{1,2}):(\d{2})/);
        return m ? pad(m[1]) + ':' + m[2] : '';
    }
    /** DateTime.ToString("HH:mm tt") - 24-hour clock followed by AM / PM, as the desktop grids show it. */
    function hmtt(v) {
        var t = hm(v); if (!t) return '';
        return t + (HRM.int(t.split(':')[0]) < 12 ? ' AM' : ' PM');
    }
    /** "dd-MMM-yy" (DateTimePicker CustomFormat of the CPL forms). */
    function ddMMMyy(v) {
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0].substring(2);
    }
    function nowHm() { return HRM.nowTime(); }

    /** The desktop's grid button columns (Delete / X / Edit): a link-styled button that calls fn(row, index). */
    function btnCell(text) {
        return function () { return '<a href="#" class="hrm-row-btn" role="button" style="display:inline-block;min-width:24px;padding:0 6px;border:1px solid #8aa;border-radius:2px;background:#f4f4f4;color:#a00;font-weight:bold;text-decoration:none;text-align:center">' + HRM.esc(text) + '</a>'; };
    }
    function onRowButton(grid, fn) {
        grid.table.addEventListener('click', function (e) {
            var b = e.target.closest('a.hrm-row-btn'); if (!b) return;
            e.preventDefault();
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            fn(grid.data[i], i);
        });
    }
    /** GridEX AllowDelete: the Delete key removes the current row. */
    function deleteKey(grid) {
        grid.table.addEventListener('keydown', function (e) {
            if (e.key !== 'Delete' || e.target.closest('input,select')) return;
            var i = grid.currentIndex(); if (i < 0) return;
            e.preventDefault(); grid.remove(i);
        });
    }
    function onChange(id, fn) { var e = HRM.$(id); if (e) e.addEventListener('change', fn); }
    /** A read-only UltraCombo bound to the one row GetEmployeeHistory found (Rows[0].Activate()). */
    function roCombo(id, v, t) {
        var s = HRM.$(id); if (!s) return;
        s.innerHTML = (v === null || v === undefined) ? '' : '<option value="' + HRM.esc(v) + '">' + HRM.esc(t) + '</option>';
        HRM.refreshCombos();
    }
    /** GetEmployeeHistory(EmployeeId): txtEmployeeNo + Designation / Department / Section / Location. */
    function fillEmployeeInfo(o) {
        if (o && HRM.bool(o.found)) {
            HRM.setVal('txtEmployeeNo', HRM.str(o.EmployeeNo));
            roCombo('cmbDesignation', o.DesignationId, HRM.str(o.DesignationName));
            roCombo('cmbDepartment', o.DepartmentId, HRM.str(o.DepartmentName));
            roCombo('cmbSection', o.SectionId, HRM.str(o.SectionName));
            roCombo('cmbLocation', o.LocationId, HRM.str(o.LocationName));
        } else {
            HRM.setVal('txtEmployeeNo', '');
            ['cmbDesignation', 'cmbDepartment', 'cmbSection', 'cmbLocation'].forEach(function (c) { roCombo(c, null); });
        }
    }
    /** btnEmployeeRegister_Click / BtnEmploye_Click: frmEmployeeRegistration.Show() (its page checks View). */
    P.employeeRegistration = function () { HRM.open('/hrm/employee-registration'); };

    // ============================================================================ 660 Leave Opening
    function leaveOpening() {
        var RecId = 0;
        var grd = new HRM.Grid('grd', {
            columns: [                                                          // GridBindByLeaveProfileTypeId + GrdSetting
                { key: 'EmployeeId', hidden: true },
                { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'code', width: 80 },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 180 },
                { key: 'LeaveQuotaDetailId', hidden: true },
                { key: 'JoiningDate', caption: 'JoiningDate', type: 'date', width: 90 },
                { key: 'DepartmentName', caption: 'DepartmentName', width: 140 },
                { key: 'DesignationName', caption: 'DesignationName', width: 140 },
                { key: 'QuotaValue', caption: 'QuotaValue', align: 'right', render: plain, width: 70 },
                { key: 'QuotaFromDate', hidden: true },
                { key: 'QuotaToDate', hidden: true },
                { key: 'TotalMonth', caption: 'TotalMonth', align: 'right', render: plain, width: 70 },
                { key: 'Assign', caption: 'Assign', align: 'right', render: plain, width: 70 },
                { key: 'Availed', caption: 'Availed', type: 'edit-num', width: 80, redraw: true },
                { key: 'Balance', caption: 'Balance', align: 'right', render: plain, width: 70 }
            ],
            filterRow: true,
            onCode: function (r) { HRM.open('/hrm/employee-registration?id=' + HRM.int(r.EmployeeId)); },
            onChange: function (r, k) {                                         // grd_CellUpdated
                if (k !== 'Availed') return;
                var Assign = HRM.num(r.Assign), Availed = HRM.num(r.Availed);
                if (Availed > Assign) {
                    HRM.box('Availd Leave cannot be greater than AssignLeave Please Check');
                    r.Availed = 0; r.Balance = 0;
                } else {
                    r.Balance = Math.round((Assign - Availed) * 1e6) / 1e6;
                }
            }
        });
        deleteKey(grd);
        function fillCombos(d) {                                                // EmployeeNameFill, LeaveQuotaBind, ProfileTypeFill
            if (d.employees && d.employees.length) HRM.fill('cmbEmployee', d.employees, 'Id', 'Name', { zero: '' });
            if (d.quotas && d.quotas.length) HRM.fill('CmbLeaveQuota', d.quotas, 'LeaveQuotaId', 'LeaveQuotaDesription', { zero: '' });
            if (d.leaveTypes && d.leaveTypes.length) HRM.fill('CmbLeaveType', d.leaveTypes, 'ProfileId', 'ProfileName', { zero: '' });
        }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnUpdate', update); }
        function Reset() {                                                      // Reset()
            buttons(false);
            RecId = 0;
            HRM.focus('CmbLeaveQuota');
            HRM.setCombo('CmbLeaveType', 0);
            HRM.setCombo('cmbEmployee', 0);
            grd.clear();
        }
        function FormValiadation() {
            if (HRM.comboVal('CmbLeaveQuota') === 0) { HRM.focus('CmbLeaveQuota'); HRM.box('LeaveQuota Field Required'); return false; }
            if (HRM.comboVal('CmbLeaveType') === 0) { HRM.focus('CmbLeaveType'); HRM.box('LeaveType Field Required'); return false; }
            return true;
        }
        function Insert(btn) {                                                  // Insert()
            if (!FormValiadation()) return;
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId,
                    leaveQuotaId: HRM.comboVal('CmbLeaveQuota'),
                    leaveTypeProfileId: HRM.comboVal('CmbLeaveType'),
                    details: grd.visibleRows().map(function (r) {               // grd.GetRows()
                        return { employeeId: HRM.int(r.EmployeeId), leaveQuotaDetailId: HRM.int(r.LeaveQuotaDetailId),
                            assign: HRM.num(r.Assign), availed: HRM.num(r.Availed) };
                    })
                }).then(function (d) {
                    HRM.box(d.message || (RecId === 0 ? 'Saved Successfully' : 'Update Successfully'));
                    Reset();
                }).catch(HRM.fail);
            }, 'leave-opening-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // btnsave_Click
        P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };            // btnUpdate_Click
        P.btnnew = function () { Reset(); };                                            // btnnew_Click
        P.btnLoad = function (btn) {                                                    // btnLoad_Click -> GridBindByLeaveProfileTypeId
            return HRM.busy(btn || 'btnLoad', function () {
                return HRM.get(API + '/load', {
                    leaveQuotaId: HRM.comboVal('CmbLeaveQuota'),
                    leaveTypeProfileId: HRM.comboVal('CmbLeaveType'),
                    employeeId: HRM.comboVal('cmbEmployee')
                }).then(function (rows) {
                    if (rows && rows.length) grd.set(rows);
                    else { grd.clear(); HRM.box('No record found...\n Try By Applying Leave Quota And Leave Type'); }
                }).catch(HRM.fail);
            });
        };
        P.btnRefresh = function (btn) {                                                 // btnRefresh_Click
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/combos').then(fillCombos).catch(HRM.fail);
            });
        };
        P.btnQuata = function () { HRM.open('/hrm/leave-quota-policy'); };              // btnQuata_Click: new LeaveQuotaPolicy().Show()
        P.btnLeaveType = function () { HRM.open('/hrm/define-profile?profileTypeId=3'); }; // btnLeaveType_Click: ProfileDefine { profileTypeId = 3 }
        HRM.keys({                                                                      // frmItemPricingSchedule_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave')) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('btnUpdate')) P.btnUpdate(); }
        });
        HRM.footer(null);                                                               // tabPage2 (history) is removed at Load and never filled
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // frmItemPricingSchedule_Load
            fillCombos(d);
            buttons(false);
            HRM.focus('CmbLeaveQuota');
        }).catch(HRM.fail);
    }

    // ============================================================================ 661 Employee Leave
    function employeeLeave() {
        var RecId = 0, updateDetailIndex = -1, Deletelst = [], balSeq = 0, empSeq = 0;
        var datagrid = new HRM.Grid('datagrid', {
            columns: [
                { key: 'Delete', caption: 'X', width: 35, render: btnCell('X'), align: 'center' },
                { key: 'EmployeeLeaveDetailId', hidden: true },
                { key: 'LeaveDate', caption: 'LeaveDate', type: 'date', width: 140 }
            ],
            filterRow: true,
            onDouble: function (r, i) {                                                  // datagrid_DoubleClick
                updateDetailIndex = i;
                HRM.setVal('datLeaveDate', HRM.day(r.LeaveDate));
                HRM.text('btnAdd', 'Update');
            }
        });
        onRowButton(datagrid, function (r, i) {                                          // datagrid_ColumnButtonClick "Delete"
            if (!HRM.ask('Are you sure to Delete?')) return;
            if (HRM.int(r.EmployeeLeaveDetailId) > 0) Deletelst.push({ employeeLeaveDetailId: HRM.int(r.EmployeeLeaveDetailId), leaveDate: HRM.day(r.LeaveDate) });
            datagrid.remove(i);
        });
        function LeaveBalance() {                                                       // LeaveBalance()
            var seq = ++balSeq;
            return HRM.get(API + '/balance', {
                employeeId: HRM.comboVal('cmbEmployee'), leaveQuotaId: HRM.comboVal('CmbLeaveQuota'), leaveTypeProfileId: HRM.comboVal('cmbLeaveType')
            }).then(function (d) { if (seq === balSeq) HRM.setVal('txtLeaveBalance', HRM.str(d && d.balance)); }).catch(HRM.fail);
        }
        function GetEmployeeHistory(id) {                                               // GetEmployeeHistory(EmployeeId)
            if (id <= 0) return Promise.resolve();
            var seq = ++empSeq;
            return HRM.get(API + '/employee', { employeeId: id }).then(function (o) { if (seq === empSeq) fillEmployeeInfo(o); }).catch(HRM.fail);
        }
        function cmbEmployee_Leave() { return Promise.all([LeaveBalance(), GetEmployeeHistory(HRM.comboVal('cmbEmployee'))]); }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnUpdate', update); }
        function Reset() {                                                              // Reset()
            HRM.setCombo('cmbEmployee', 0);
            HRM.setVal('datLeaveDate', HRM.today());
            HRM.setVal('datFromDate', HRM.today());
            HRM.setVal('datToDate', HRM.today());
            HRM.setVal('txtReason', '');
            HRM.setVal('txtRemarks', '');
            HRM.setVal('txtEmployeeNo', '');
            roCombo('cmbDepartment', null);                                             // (the desktop clears Department twice and leaves Designation)
            roCombo('cmbSection', null);
            roCombo('cmbLocation', null);
            HRM.setVal('txtLeaveBalance', '');
            buttons(false);
            RecId = 0;
            Deletelst = [];
            datagrid.clear();
            HRM.text('btnAdd', 'Add');
            HRM.focus('cmbEmployee');
        }
        function FormValiadation() {
            if (HRM.comboVal('cmbEmployee') === 0) { HRM.focus('cmbEmployee'); HRM.box('Employee Field Required'); return false; }
            if (HRM.comboVal('cmbLeaveType') === 0) { HRM.focus('cmbLeaveType'); HRM.box('LeaveType Field Required'); return false; }
            if (HRM.comboVal('CmbLeaveQuota') === 0) { HRM.focus('CmbLeaveQuota'); HRM.box('LeaveQuota Field Required'); return false; }
            if (HRM.val('txtReason') === '') { HRM.focus('txtReason'); HRM.box('Reason Field Required'); return false; }
            return true;
        }
        function Insert(btn) {                                                          // Insert()
            if (!FormValiadation()) return;
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            if (!datagrid.rows().length) { HRM.box('Please Add Detail Record'); return; }
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId,
                    employeeId: HRM.comboVal('cmbEmployee'),
                    leaveTypeProfileId: HRM.comboVal('cmbLeaveType'),
                    leaveQuotaId: HRM.comboVal('CmbLeaveQuota'),
                    reason: HRM.val('txtReason'),
                    remarks: HRM.val('txtRemarks'),
                    details: datagrid.rows().map(function (r) { return { employeeLeaveDetailId: HRM.int(r.EmployeeLeaveDetailId), leaveDate: HRM.day(r.LeaveDate) }; }),
                    deletes: Deletelst.slice()
                }).then(function (d) {
                    HRM.box(d.message || (RecId === 0 ? 'Save Successfully' : 'Update Successfully'));
                    Reset();
                }).catch(HRM.fail);
            }, 'employee-leave-save');
        }
        function GetByItemId(id) {                                                      // GetByItemId(EmployeeLeaveId)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = HRM.int(o.EmployeeLeaveId);
                HRM.setCombo('cmbEmployee', o.EmployeeId);
                HRM.setCombo('cmbLeaveType', o.LeaveTypeProfileId);
                HRM.setCombo('CmbLeaveQuota', o.EmployeeLeaveQuotaId);
                HRM.setVal('datFromDate', HRM.day(o.FromDate));
                HRM.setVal('datToDate', HRM.day(o.ToDate));
                HRM.setVal('txtReason', HRM.str(o.Reason));
                HRM.setVal('txtRemarks', HRM.str(o.Remarks));
                cmbEmployee_Leave();
                Deletelst = [];
                datagrid.set((o.details || []).map(function (d) { return { EmployeeLeaveDetailId: d.EmployeeLeaveDetailId, LeaveDate: d.LeaveDate }; }));
                buttons(true);
            }).catch(HRM.fail);
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // btnsave_Click
        P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };            // btnUpdate_Click
        P.btnnew = function () { Reset(); };                                            // btnnew_Click
        P.btnAdd = function () {                                                        // btnAdd_Click
            var LeaveDate = HRM.val('datLeaveDate') || HRM.today();
            var rows = datagrid.rows();
            for (var i = 0; i < rows.length; i++) {
                if (HRM.day(rows[i].LeaveDate) === LeaveDate) { HRM.box('Same Date Already Exist in grid....'); return; }
            }
            if (HRM.$('btnAdd').textContent === 'Add') datagrid.add({ EmployeeLeaveDetailId: 0, LeaveDate: LeaveDate });
            else if (HRM.$('btnAdd').textContent === 'Update') {
                if (updateDetailIndex >= 0 && rows[updateDetailIndex]) { rows[updateDetailIndex].LeaveDate = LeaveDate; datagrid.draw(); }
                HRM.text('btnAdd', 'Add');
            }
            HRM.focus('datLeaveDate');
            HRM.setVal('datLeaveDate', HRM.addDays(LeaveDate, 1));
        };
        function openHistory() {                                                        // tabControl1 -> "history" tab: FormHistory()
            var detail;
            HRM.history({
                title: 'Leave Request History',
                columns: [
                    { key: 'Edit', caption: 'Edit', type: 'code', width: 50 },
                    { key: 'EmployeeLeaveId', hidden: true },
                    { key: 'EmployeeName', caption: 'EmployeeName', width: 200 },
                    { key: 'LeaveType', caption: 'LeaveType', width: 120 },
                    { key: 'NoOfDays', caption: 'NoOfDays', type: 'int', width: 70 },
                    { key: 'Reason', caption: 'Reason', width: 220 },
                    { key: 'Remarks', caption: 'Remarks', width: 220 }
                ],
                load: function () {
                    return HRM.get(API + '/history').then(function (rows) {
                        return (rows || []).map(function (r) { r.Edit = 'Edit'; return r; });
                    });
                },
                onPick: function (r) { GetByItemId(HRM.int(r.EmployeeLeaveId)); },       // DataGridHistory_ColumnButtonClick "Edit"
                onReady: function (body, g) {
                    var box = document.createElement('div');
                    box.className = 'hrm-grid-box hrm-grid-short';
                    box.innerHTML = '<div class="hrm-subcaption"><span>Leave Dates</span></div><div class="hrm-grid-wrap"><table id="grdDetailHistory"></table></div>';
                    body.appendChild(box);
                    detail = new HRM.Grid(box.querySelector('table'), {
                        columns: [{ key: 'Id', hidden: true }, { key: 'LeaveDate', caption: 'LeaveDate', type: 'date', width: 140 }], filterRow: true
                    });
                    g.opts.onSelect = function (r) {                                     // DataGridHistory_SelectionChanged
                        HRM.get(API + '/history-detail', { id: HRM.int(r.EmployeeLeaveId) }).then(function (rows) { detail.set(rows || []); }).catch(HRM.fail);
                    };
                }
            });
        }
        P.history = openHistory;
        onChange('cmbEmployee', cmbEmployee_Leave);                                     // cmbEmployee TextChanged / Leave
        onChange('cmbLeaveType', LeaveBalance);                                         // cmbLeaveType TextChanged / Leave
        onChange('CmbLeaveQuota', LeaveBalance);                                        // CmbLeaveQuota TextChanged / Leave
        HRM.keys({                                                                      // frmItemPricingSchedule_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave')) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': openHistory,
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('btnUpdate')) P.btnUpdate(); }
        });
        HRM.footer(function () { openHistory(); });
        HRM.setVal('datLeaveDate', HRM.today());
        HRM.setVal('datFromDate', HRM.today());
        HRM.setVal('datToDate', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // frmItemPricingSchedule_Load
            if (d.employees && d.employees.length) HRM.fill('cmbEmployee', d.employees, 'EmployeeId', 'EmployeeName', { zero: '' });
            if (d.leaveTypes && d.leaveTypes.length) HRM.fill('cmbLeaveType', d.leaveTypes, 'ProfileId', 'ProfileName', { zero: '' });
            if (d.quotas && d.quotas.length) HRM.fill('CmbLeaveQuota', d.quotas, 'LeaveQuotaId', 'LeaveQuotaDesription', { zero: '' });
            buttons(false);
            HRM.focus('CmbLeaveQuota');
            var qid = HRM.int(HRM.param('id'));                                         // opened with a record (Leave Approval 668): GetByItemId
            if (qid > 0) GetByItemId(qid);
        }).catch(HRM.fail);
    }

    // ============================================================================ shared: CPL history dialog (662/463, 461)
    function cplHistory(title, onPick) {
        return HRM.history({
            title: title,
            columns: [
                { key: 'CPLAttendanceId', hidden: true },
                { key: 'EmployeeId', hidden: true },
                { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'code', width: 90 },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 200 },
                { key: 'CPLDate', caption: 'CPLDate', type: 'date', width: 100 },
                { key: 'InDateTime', caption: 'InDateTime', width: 90 },
                { key: 'OutDateTime', caption: 'OutDateTime', width: 90 },
                { key: 'CPLQuota', caption: 'CPLQuota', align: 'right', render: fmtHash, sum: true, width: 80 }
            ],
            totals: true,
            load: function () { return HRM.get(API + '/history'); },
            onPick: onPick,                                                             // DataGridHistory_DoubleClick
            onReady: function (body, g) { g.opts.onDraw = function () { hashTotal(g, 'CPLQuota'); }; }
        });
    }
    /** TotalFormatString "#,#.##" on the grid's total row. */
    function hashTotal(g, key) {
        var tf = g.table.tFoot; if (!tf || !tf.rows.length) return;
        var ci = -1; g.columns.forEach(function (c, i) { if (c.key === key) ci = i; });
        if (ci >= 0 && tf.rows[0].cells[ci]) tf.rows[0].cells[ci].textContent = fmtHash(g.sum(key, true));
    }

    // ============================================================================ 662 / 463 CPL Leave Opening
    function cplLeaveOpening() {
        var updateDetailIndex = -1, Deletelst = [];
        var grd = new HRM.Grid('grd', {
            columns: [                                                                  // dtGrid + GrdSetting
                { key: 'Id', hidden: true },
                { key: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 220 },
                { key: 'CPLDate', caption: 'CPLDate', render: ddMMMyy, width: 100 },
                { key: 'InTime', caption: 'InTime', render: hmtt, width: 90 },
                { key: 'OutTime', caption: 'OutTime', render: hmtt, width: 90 },
                { key: 'CPLQuota', caption: 'CPLQuota', align: 'right', render: fmtHash, sum: true, width: 90 },
                { key: 'Delete', caption: 'Delete', width: 35, render: btnCell('✖'), align: 'center' }
            ],
            filterRow: true, totals: true,
            onDraw: function (g) { hashTotal(g, 'CPLQuota'); },
            onDouble: function (r, i) {                                                  // grd_DoubleClick
                updateDetailIndex = i;
                HRM.setCombo('cmbEmployee', r.EmployeeId);
                HRM.setVal('datDate', HRM.day(r.CPLDate));
                HRM.setVal('datInTime', hm(r.InTime));
                HRM.setVal('datOutTime', hm(r.OutTime));
                HRM.setVal('txtCplQuota', String(HRM.num(r.CPLQuota)));
                addButtons(false);
                HRM.focus('cmbEmployee');
            }
        });
        onRowButton(grd, function (r, i) {                                               // grd_ColumnButtonClick "Delete"
            if (!HRM.ask('Are you sure to Delete?')) return;
            if (HRM.int(r.Id) > 0) Deletelst.push(cplRow(r));
            grd.remove(i);
        });
        function cplRow(r) {
            return { id: HRM.int(r.Id), employeeId: HRM.int(r.EmployeeId), employeeAttendanceId: 0, cplDate: HRM.day(r.CPLDate),
                inTime: hm(r.InTime), outTime: hm(r.OutTime), cplQuota: HRM.num(r.CPLQuota) };
        }
        function addButtons(add) { HRM.show('btnAdd', add); HRM.show('btnUpdateRow', !add); HRM.show('btnCancel', !add); }
        function fillEmployees(rows) {                                                  // EmployeeNameFill
            if (rows && rows.length) HRM.fill('cmbEmployee', rows, 'EmployeeId', 'EmployeeName', { zero: '' });
        }
        function Reset() {                                                              // Reset()
            HRM.focus('cmbEmployee');
            HRM.setCombo('cmbEmployee', 0);
            HRM.setVal('txtCplQuota', '');
            HRM.setVal('datDate', HRM.today());
            grd.clear();
            addButtons(true);
            Deletelst = [];
        }
        function FormValiadation() {
            if (HRM.comboVal('cmbEmployee') === 0) { HRM.focus('cmbEmployee'); HRM.box('EmployeeName Field Required'); return false; }
            if (HRM.num(HRM.val('txtCplQuota')) === 0) { HRM.focus('txtCplQuota'); HRM.box('CPL Quota Field Required'); return false; }
            return true;
        }
        P.btnAdd = function () {                                                        // btnLoad_Click (the "Add" button)
            if (!FormValiadation()) return;
            grd.add({ Id: 0, EmployeeId: HRM.comboVal('cmbEmployee'), EmployeeName: HRM.comboText('cmbEmployee').trim(),
                CPLDate: HRM.val('datDate'), InTime: HRM.val('datInTime'), OutTime: HRM.val('datOutTime'), CPLQuota: HRM.num(HRM.val('txtCplQuota').trim()) });
            HRM.setCombo('cmbEmployee', 0);
            HRM.focus('cmbEmployee');
        };
        P.btnUpdateRow = function () {                                                  // btnUpdate_Click (row update, no validation)
            var r = grd.rows()[updateDetailIndex];
            if (r) {
                r.EmployeeId = HRM.comboVal('cmbEmployee');
                r.EmployeeName = HRM.comboText('cmbEmployee');
                r.CPLDate = HRM.val('datDate');
                r.InTime = HRM.val('datInTime');
                r.OutTime = HRM.val('datOutTime');
                r.CPLQuota = HRM.num(HRM.val('txtCplQuota'));
                grd.draw();
            }
            addButtons(true);
            HRM.focus('cmbEmployee');
        };
        P.btnCancel = function () {                                                     // btnCancel_Click: clears the two boxes only
            HRM.setCombo('cmbEmployee', 0);
            HRM.setVal('txtCplQuota', '');
        };
        P.btnsave = function (btn) {                                                    // btnsave_Click -> Insert()
            if (!HRM.ask('Are you sure to Save?')) return;
            return HRM.busy(btn || 'btnsave', function () {
                return HRM.post(API + '/save', { rows: grd.rows().map(cplRow), deletes: Deletelst.slice() }).then(function (d) {
                    HRM.box(d.message || 'Saved Successfully');
                    Reset();
                }).catch(HRM.fail);
            }, 'cpl-opening-save');
        };
        P.btnnew = function () { Reset(); };                                            // btnnew_Click
        P.btnRefresh = function (btn) {                                                 // btnRefresh_Click
            return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/employees').then(fillEmployees).catch(HRM.fail); });
        };
        function ReadById(id) {                                                         // ReadById(Id)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                grd.set([{ Id: o.CPLAttendanceId, EmployeeId: o.EmployeeId, EmployeeName: o.EmployeeName, CPLDate: o.CPLDate,
                    InTime: hm(o.InTime), OutTime: hm(o.OutTime), CPLQuota: o.CPLQuota }]);
            }).catch(HRM.fail);
        }
        function openHistory() { cplHistory('CPL Opening History', function (r) { ReadById(HRM.int(r.CPLAttendanceId)); }); }
        P.history = openHistory;
        HRM.keys({                                                                      // frmItemPricingSchedule_KeyDown
            'ctrl+s': function () { P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': openHistory,
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        HRM.footer(function () { openHistory(); });
        HRM.setVal('datDate', HRM.today());
        HRM.setVal('datInTime', nowHm());
        HRM.setVal('datOutTime', nowHm());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // frmItemPricingSchedule_Load
            fillEmployees(d.employees);
            addButtons(true);
            HRM.focus('cmbEmployee');
        }).catch(HRM.fail);
    }

    // ============================================================================ 461 CPL Attendance
    function cplAttendance() {
        var Deletelst = [], grd = null, fromRecord = false;
        function makeGrid(withName) {                                                   // RetrieveStructure of the load table / of ReadById's table
            fromRecord = withName;
            var old = HRM.$('grd'), fresh = old.cloneNode(false);                       // a new table: no listeners of the previous structure
            old.parentNode.replaceChild(fresh, old);
            var cols = [
                { key: 'CPLAttendanceId', hidden: true },
                { key: 'EmployeeId', hidden: true },
                { key: 'EmployeeAttendanceId', hidden: true }
            ];
            if (withName) cols.push({ key: 'EmployeeName', caption: 'EmployeeName', width: 200 });
            cols.push(
                { key: 'DutyDate', caption: 'DutyDate', type: 'date', width: 100 },
                { key: 'InDateTime', caption: 'InDateTime', width: 150, render: function (v) { return HRM.esc(withName ? hmtt(v) : HRM.fmtDateTime(v)); } },
                { key: 'OutDateTime', caption: 'OutDateTime', width: 150, render: function (v) { return HRM.esc(withName ? hmtt(v) : HRM.fmtDateTime(v)); } },
                { key: 'CPLQuota', caption: 'CPLQuota', align: 'right', render: fmtHash, sum: true, width: 90 },
                { key: 'Delete', caption: 'Delete', width: 35, render: btnCell('✖'), align: 'center' }
            );
            grd = new HRM.Grid('grd', { columns: cols, filterRow: true, totals: true, onDraw: function (g) { hashTotal(g, 'CPLQuota'); } });
            onRowButton(grd, function (r, i) {                                           // grd_ColumnButtonClick "Delete"
                if (!HRM.ask('Are you sure to Delete?')) return;
                if (HRM.int(r.CPLAttendanceId) > 0) Deletelst.push(row(r));
                grd.remove(i);
            });
            return grd;
        }
        function row(r) {
            return { id: HRM.int(r.CPLAttendanceId), employeeId: HRM.int(r.EmployeeId), employeeAttendanceId: HRM.int(r.EmployeeAttendanceId),
                cplDate: HRM.day(r.DutyDate), inTime: HRM.str(r.InDateTime), outTime: HRM.str(r.OutDateTime), cplQuota: HRM.num(r.CPLQuota) };
        }
        function Reset() {                                                              // Reset()
            HRM.focus('cmbEmployee');
            HRM.setCombo('cmbEmployee', 0);
            makeGrid(fromRecord).clear();
            Deletelst = [];
        }
        P.btnnew = function () {                                                        // btnnew_Click
            HRM.setCombo('cmbMonth', 0);
            HRM.setCombo('cmbYear', 0);
            Reset();
        };
        P.btnShow = function (btn) {                                                    // btnLoad_Click (the "Show" button)
            if (HRM.comboVal('cmbEmployee') === 0) { HRM.focus('cmbEmployee'); HRM.box('EmployeeName Field Required'); return; }
            if (HRM.comboVal('cmbMonth') === 0) { HRM.focus('cmbMonth'); HRM.box('Month Field Required'); return; }
            if (HRM.comboVal('cmbYear') === 0) { HRM.focus('cmbYear'); HRM.box('Year Field Required'); return; }
            return HRM.busy(btn || 'btnShow', function () {
                return HRM.get(API + '/load', { employeeId: HRM.comboVal('cmbEmployee'), month: HRM.comboVal('cmbMonth'), year: HRM.comboVal('cmbYear') })
                    .then(function (rows) { makeGrid(false).set(rows || []); }).catch(HRM.fail);
            });
        };
        P.btnsave = function (btn) {                                                    // btnsave_Click -> Insert()
            if (!HRM.ask('Are you sure to Save?')) return;
            return HRM.busy(btn || 'btnsave', function () {
                return HRM.post(API + '/save', { rows: grd.rows().map(row), deletes: Deletelst.slice() }).then(function (d) {
                    HRM.box(d.message || 'Saved Successfully');
                    Reset();
                }).catch(HRM.fail);
            }, 'cpl-attendance-save');
        };
        function ReadById(id) {                                                         // ReadById(Id)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                makeGrid(true).set([{ CPLAttendanceId: o.CPLAttendanceId, EmployeeAttendanceId: o.EmployeeAttendanceId, EmployeeId: o.EmployeeId,
                    EmployeeName: o.EmployeeName, DutyDate: o.CPLDate, InDateTime: o.InTime, OutDateTime: o.OutTime, CPLQuota: o.CPLQuota }]);
            }).catch(HRM.fail);
        }
        function openHistory() { cplHistory('CPL Attendance History', function (r) { ReadById(HRM.int(r.CPLAttendanceId)); }); }
        P.history = openHistory;
        HRM.keys({                                                                      // frmItemPricingSchedule_KeyDown
            'ctrl+s': function () { P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': openHistory,
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        HRM.footer(function () { openHistory(); });
        makeGrid(false);
        HRM.fillFixed('cmbMonth', HRM.MONTHS.map(function (m, i) { return [i + 1, m]; }), { zero: '' });      // MonthFill (CommonServices.GetMonths)
        var years = [], y = new Date().getFullYear();
        for (var i = y; i >= 1900; i--) years.push([i, String(i)]);                    // YearFill: 1900..now, "Id DESC"
        HRM.fillFixed('cmbYear', years, { zero: '' });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // frmItemPricingSchedule_Load
            if (d.employees && d.employees.length) HRM.fill('cmbEmployee', d.employees, 'EmployeeId', 'EmployeeName', { zero: '' });
            HRM.focus('cmbEmployee');
        }).catch(HRM.fail);
    }

    // ============================================================================ 462 CPL Request
    function cplRequest() {
        var RecId = 0, updateDetailIndex = -1, Deletelst = [], showDelete = false, empSeq = 0;
        var datagrid = new HRM.Grid('datagrid', {
            columns: [
                { key: 'EmployeeLeaveDetailId', hidden: true },
                { key: 'LeaveDate', caption: 'LeaveDate', type: 'date', width: 140 },
                { key: 'Delete', caption: 'Delete', width: 35, align: 'center', render: function () { return showDelete ? btnCell('✖')() : ''; } }
            ],
            filterRow: true,
            onDouble: function (r, i) {                                                  // datagrid_DoubleClick
                updateDetailIndex = i;
                HRM.setVal('datLeaveDate', HRM.day(r.LeaveDate));
                HRM.text('btnAdd', 'Update');
            }
        });
        onRowButton(datagrid, function (r, i) {                                          // datagrid_ColumnButtonClick "Delete"
            if (!HRM.ask('Are you sure to Delete?')) return;
            if (HRM.int(r.EmployeeLeaveDetailId) > 0) Deletelst.push({ employeeLeaveDetailId: HRM.int(r.EmployeeLeaveDetailId), leaveDate: HRM.day(r.LeaveDate) });
            datagrid.remove(i);
        });
        var grd = new HRM.Grid('grd', {
            columns: [                                                                  // GridSetting()
                { key: 'CPLEmployeeLeaveId', hidden: true },
                { key: 'EmployeeLeaveId', hidden: true },
                { key: 'CPLAttendanceId', hidden: true },
                { key: 'ActionTypeId', hidden: true },
                { key: 'CPLDate', caption: 'CPLDate', type: 'date', width: 100 },
                { key: 'WeekDayName', caption: 'WeekDayName', width: 110 },
                { key: 'CPLQuota', caption: 'CPLQuota', align: 'right', render: plain, width: 80 },
                { key: 'Active', caption: 'Active', type: 'edit-check', width: 60 }
            ],
            checkAll: 'Active', filterRow: true
        });
        function GridSetting(rows) {                                                    // CPLBalance = total of CPLQuota
            grd.set(rows);
            HRM.setVal('txtCPLBalance', String(HRM.round(grd.sum('CPLQuota'), 10)));
        }
        function NoOfDays() {                                                           // txtFromTime_ValueChanged / txtToTime_ValueChanged
            var f = HRM.val('txtFromTime'), t = HRM.val('txtToTime');
            if (!f || !t) return;
            HRM.setVal('txtNoofDays', String(HRM.int(t.split('-')[2]) - HRM.int(f.split('-')[2]) + 1));
        }
        function GetEmployeeHistory(id) {                                               // cmbEmployee_ValueChanged -> GetEmployeeHistory
            if (id <= 0) return Promise.resolve();
            var seq = ++empSeq;
            return HRM.get(API + '/employee', { employeeId: id }).then(function (o) { if (seq === empSeq) fillEmployeeInfo(o); }).catch(HRM.fail);
        }
        function CplRecords(id) {                                                       // cmbEmployee_TextChanged -> GetCPLRecordForEmployeeLeaveRequest
            return HRM.get(API + '/cpl', { employeeId: id }).then(function (rows) {
                if (rows && rows.length) GridSetting(rows); else grd.clear();
            }).catch(HRM.fail);
        }
        function employeeChanged() {
            var id = HRM.comboVal('cmbEmployee');
            return Promise.all([GetEmployeeHistory(id), CplRecords(id)]);
        }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnUpdate', update); }
        function Reset() {                                                              // Reset()
            HRM.setCombo('cmbEmployee', 0);
            HRM.setVal('txtFromTime', HRM.today());
            HRM.setVal('txtToTime', HRM.today());
            HRM.setVal('txtReason', '');
            HRM.setVal('txtEmployeeNo', '');
            ['cmbDesignation', 'cmbDepartment', 'cmbSection', 'cmbLocation'].forEach(function (c) { roCombo(c, null); });
            HRM.setVal('txtNoofDays', '');
            HRM.setVal('txtCPLBalance', '');
            buttons(false);
            RecId = 0;
            Deletelst = [];
            showDelete = false;
            datagrid.clear();
            grd.clear();
            HRM.focus('cmbEmployee');
        }
        function FormValiadation() {
            if (HRM.comboVal('cmbEmployee') === 0) { HRM.focus('cmbEmployee'); HRM.box('Employee Required'); return false; }
            if (HRM.val('txtReason') === '') { HRM.focus('txtReason'); HRM.box('Reason Required'); return false; }
            return true;
        }
        function Insert(btn) {                                                          // Insert()
            var CplQuota = 0;
            grd.rows().forEach(function (r) { if (HRM.bool(r.Active)) CplQuota += HRM.num(r.CPLQuota); });
            var num = datagrid.rows().length;
            if (num === 0) { HRM.box('Please add record first in grid'); return; }
            if (num !== CplQuota) { HRM.box('Noofleaves cannot be equal to sum of CPL of grid'); return; }
            if (!FormValiadation()) return;
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId,
                    employeeId: HRM.comboVal('cmbEmployee'),
                    fromDate: HRM.val('txtFromTime'),
                    toDate: HRM.val('txtToTime'),
                    reason: HRM.val('txtReason'),
                    details: datagrid.rows().map(function (r) { return { employeeLeaveDetailId: HRM.int(r.EmployeeLeaveDetailId), leaveDate: HRM.day(r.LeaveDate) }; }),
                    deletes: Deletelst.slice(),
                    cpl: grd.rows().map(function (r) { return { cplAttendanceId: HRM.int(r.CPLAttendanceId), active: HRM.bool(r.Active) }; })
                }).then(function (d) {
                    HRM.box(d.message || (RecId === 0 ? 'Save Successfully' : 'Update Successfully'));
                    Reset();
                }).catch(HRM.fail);
            }, 'cpl-request-save');
        }
        function GetByItemId(id) {                                                      // GetByItemId(EmployeeLeaveId)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = HRM.int(o.EmployeeLeaveId);
                HRM.setCombo('cmbEmployee', o.EmployeeId);
                var cplLoaded = employeeChanged();
                HRM.setVal('txtFromTime', HRM.day(o.FromDate));
                HRM.setVal('txtToTime', HRM.day(o.ToDate));
                NoOfDays();
                HRM.setVal('txtReason', HRM.str(o.Reason));
                Deletelst = [];
                showDelete = false;                                                     // the Delete column is added by btnAdd only
                datagrid.set((o.details || []).map(function (d) { return { EmployeeLeaveDetailId: d.EmployeeLeaveDetailId, LeaveDate: d.LeaveDate }; }));
                return cplLoaded.then(function () {
                    if (o.cpl && o.cpl.length) GridSetting(o.cpl);                      // grd.DataSource = CPLEmployeeLeaveDetailslist
                    buttons(true);
                });
            }).catch(HRM.fail);
        }
        P.btnsave = function (btn) { return Insert(btn || 'btnsave'); };                // btnsave_Click
        P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };            // btnUpdate_Click
        P.btnnew = function () { Reset(); };                                            // btnnew_Click
        P.btnAdd = function () {                                                        // btnAdd_Click
            var FromDate = HRM.val('txtFromTime'), ToDate = HRM.val('txtToTime'), LeaveDate = HRM.val('datLeaveDate');
            if (!(LeaveDate >= FromDate) || !(LeaveDate <= ToDate)) { HRM.box('LeaveDate must between FromDate And ToDate Please Check'); return; }
            if (HRM.$('btnAdd').textContent === 'Add') datagrid.add({ EmployeeLeaveDetailId: 0, LeaveDate: LeaveDate });
            else if (HRM.$('btnAdd').textContent === 'Update') {
                var r = datagrid.rows()[updateDetailIndex];
                if (r) r.LeaveDate = LeaveDate;
                HRM.text('btnAdd', 'Add');
            }
            showDelete = true;
            datagrid.draw();
            HRM.focus('datLeaveDate');
        };
        function openHistory() {                                                        // tabControl1 -> "history" tab: FormHistory()
            HRM.history({
                title: 'CPL Request History',
                columns: [
                    { key: 'EmployeeLeaveId', hidden: true },
                    { key: 'EmployeeName', caption: 'EmployeeName', width: 200 },
                    { key: 'LocationName', caption: 'LocationName', width: 140 },
                    { key: 'LeaveType', caption: 'LeaveType', width: 90 },
                    { key: 'FromDate', caption: 'FromDate', type: 'date', width: 100 },
                    { key: 'ToDate', caption: 'ToDate', type: 'date', width: 100 },
                    { key: 'NoOfDays', caption: 'NoOfDays', align: 'right', render: plain, width: 70 },
                    { key: 'Reason', caption: 'Reason', width: 220 },
                    { key: 'Edit', caption: 'Edit', type: 'code', width: 50 }
                ],
                load: function () {
                    return HRM.get(API + '/history').then(function (rows) { return (rows || []).map(function (r) { r.Edit = 'Edit'; return r; }); });
                },
                onPick: function (r) { GetByItemId(HRM.int(r.EmployeeLeaveId)); }        // DataGridHistory_ColumnButtonClick "Edit"
            });
        }
        P.history = openHistory;
        onChange('cmbEmployee', employeeChanged);
        onChange('txtFromTime', NoOfDays);
        onChange('txtToTime', NoOfDays);
        HRM.keys({                                                                      // frmItemPricingSchedule_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave')) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+t': openHistory,
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('btnUpdate')) P.btnUpdate(); }
        });
        HRM.footer(function () { openHistory(); });
        HRM.setVal('txtFromTime', HRM.today());
        HRM.setVal('txtToTime', HRM.today());
        HRM.setVal('datLeaveDate', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                        // frmItemPricingSchedule_Load
            if (d.employees && d.employees.length) HRM.fill('cmbEmployee', d.employees, 'Id', 'Name', { zero: '' });
            buttons(false);
            HRM.focus('cmbEmployee');
            var qid = HRM.int(HRM.param('id'));                                         // opened with a record: GetByItemId
            if (qid > 0) GetByItemId(qid);
        }).catch(HRM.fail);
    }

    var PAGES = {
        'leave-opening': leaveOpening,
        'employee-leave': employeeLeave,
        'cpl-leave-opening': cplLeaveOpening,
        'cpl-attendance': cplAttendance,
        'cpl-request': cplRequest
    };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
