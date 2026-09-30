/* ============================================================================================
 * countx_hrm_device.js - HRM "Device Management" (AppModules 2021). One script, the page is
 * chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   device-configuration        frmDeviceManagement.cs (649)
 *   pull-attendance-by-machine  PullAttendanceByMachine.cs (650 / 651)
 *
 * Neither desktop form talks to the ZKTeco SDK itself: frmDeviceManagement keeps the device list and
 * PullAttendanceByMachine queues jobs (mmProductDeviceJob, JobStatus 'Pending') for the attendance
 * service on the device network - so every button is ported. Only the grid's "Ping" button, which has
 * no handler on the desktop either, explains that a browser cannot reach the device.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/device/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmDevice
    window.HrmDevice = P;

    // ============================================================================ 649 frmDeviceManagement
    function deviceConfiguration() {
        var RecId = 0, UpdateMode = false, rights = {};
        var datagrid = new HRM.Grid('datagrid', {                               // GridFill + datagridSetting
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'LocationId', caption: 'LocationId', hidden: true },
                { key: 'LocationName', caption: 'LocationName', width: 120 },
                { key: 'DeviceName', caption: 'DeviceName', type: 'code', width: 120 },
                { key: 'NetworkPort', caption: 'NetworkPort', width: 80 },
                { key: 'NetworkIP', caption: 'NetworkIP', width: 110 },
                { key: 'IsActive', caption: 'IsActive', hidden: true },
                { key: 'Status', caption: 'Status', width: 70 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(HRM.int(r.Id)); }            // datagrid_DoubleClick
        });
        function GridFill() {                                                   // only replaced when rows came back
            return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) datagrid.set(rows); }).catch(HRM.fail);
        }
        function buttons(update) {
            HRM.show('BtnSavef', !update); HRM.show('btnUpdateF', update);
            HRM.show('btnsave', !update); HRM.show('btnupdate', update);
        }
        function Reset() {                                                      // Reset(): IsActive keeps its tick, as on the desktop
            HRM.setCombo('cmbLocation', 0);
            HRM.setVal('txtDeviceName', ''); HRM.setVal('txtNetworkIP', ''); HRM.setVal('txtNetworkPort', '');
            HRM.focus('cmbLocation');
            RecId = 0; buttons(false); UpdateMode = false;
            return GridFill();
        }
        function RetrivedData(id) {                                             // RetrivedData(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = HRM.int(HRM.col(o, 'ProductDeviceId'));
                HRM.setCombo('cmbLocation', HRM.int(HRM.col(o, 'LocationId')));
                HRM.setVal('txtDeviceName', HRM.str(HRM.col(o, 'DeviceName')));
                HRM.setVal('txtNetworkPort', HRM.str(HRM.col(o, 'NetworkPort')));
                HRM.setVal('txtNetworkIP', HRM.str(HRM.col(o, 'NetworkIP')));
                HRM.check('IsActive', HRM.bool(HRM.col(o, 'IsActive')));
                buttons(true); UpdateMode = true;
            }).catch(HRM.fail);
        }
        function FormValidation() {                                             // Text == string.Empty (not trimmed)
            if (HRM.comboVal('cmbLocation') === 0) { HRM.box('Location Required'); HRM.focus('cmbLocation'); return false; }
            if (HRM.val('txtDeviceName') === '') { HRM.box('Device Name Required'); HRM.focus('txtDeviceName'); return false; }
            if (HRM.val('txtNetworkPort') === '') { HRM.box('Network Port Required'); HRM.focus('txtNetworkPort'); return false; }
            if (HRM.val('txtNetworkIP') === '') { HRM.box('Network IP Required'); HRM.focus('txtNetworkIP'); return false; }
            return true;
        }
        function Insert(btn) {                                                  // Insert()
            if (!FormValidation()) return;
            if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId, locationId: HRM.comboVal('cmbLocation'), deviceName: HRM.val('txtDeviceName'),
                    networkPort: HRM.val('txtNetworkPort'), networkIP: HRM.val('txtNetworkIP'), isActive: HRM.checked('IsActive')
                }).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'device-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { Reset(); };                                         // btnnew_Click
        P.btnbenifit = function () { HRM.open('/hrm/location'); };                   // btnbenifit_Click: new frmgenLocation().Show()
        // txtNetworkIP_KeyPress (CommonServices.OnlytextdecimelFunction): digits, control keys and '.'.
        // The desktop also refuses a second '.', which makes an IPv4 address untypeable (it can only be
        // pasted); the working behaviour is kept here - any number of dots.
        HRM.$('txtNetworkIP').addEventListener('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
            if (!/[0-9.]/.test(e.key)) e.preventDefault();
        });
        HRM.keys({                                                                   // frmDeviceManagement_KeyDown
            'ctrl+s': function () { if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled && UpdateMode) P.btnupdate(); }
        });
        HRM.footer(function (b) { return HRM.busy(b, function () { return GridFill().then(function () { HRM.$('datagrid').scrollIntoView({ block: 'nearest' }); }); }); });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // frmDeviceManagement_Load
            rights = d.rights || {};
            HRM.fill('cmbLocation', d.locations, 'Id', 'Name', { zero: '' });      // BindDDL(ZeroIndex false): no row active
            if (d.rows && d.rows.length) datagrid.set(d.rows);
            buttons(false);
            HRM.focus('cmbLocation');
        }).catch(HRM.fail);
    }

    // ============================================================================ 650/651 PullAttendanceByMachine
    function pullAttendanceByMachine() {
        var grdMachineData = new HRM.Grid('grdMachineData', {                   // MachineDataGridBind + gridMachineSetting
            columns: [
                { key: 'AtPull', caption: 'AtPull', type: 'edit-check', width: 50 },      // ActAsSelector, no header selector
                { key: 'PingButton', caption: 'Ping', width: 50, align: 'center',
                  render: function () { return '<button type="button" class="win-btn-action hrm-ping" data-ping="1">Ping</button>'; } },
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'LocationId', caption: 'LocationId', hidden: true },
                { key: 'BranchName', caption: 'BranchName', width: 150 },
                { key: 'DeviceName', caption: 'DeviceName', width: 120 },
                { key: 'DeviceLocation', caption: 'DeviceLocation', width: 180 },
                { key: 'IPAddress', caption: 'IPAddress', width: 100 },
                { key: 'PortNo', caption: 'PortNo', width: 70 },
                { key: 'IsActive', caption: 'IsActive', width: 70 }
            ],
            filterRow: true
        });
        // biometric templates are long base64 strings: the cell shows the start, the tooltip the whole value
        function tpl(v) { var t = HRM.str(v); return '<span title="' + HRM.esc(t) + '">' + HRM.esc(t.length > 24 ? t.substring(0, 24) + '\u2026' : t) + '</span>'; }
        var grdEmployeeList = new HRM.Grid('grdEmployeeList', {                 // EmployeeGridbind + grdSetting
            columns: [
                { key: 'Active', caption: 'Active', type: 'edit-check', width: 50 },       // ActAsSelector + header selector
                { key: 'EmployeeHistoryId', caption: 'EmployeeHistoryId', hidden: true },
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'int' },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 220 },
                { key: 'Designation', caption: 'Designation' },
                { key: 'Department', caption: 'Department' },
                { key: 'ShiftName', caption: 'ShiftName' },
                { key: 'Category', caption: 'Category' },
                { key: 'Section', caption: 'Section' },
                { key: 'FingerTemplate', caption: 'FingerTemplate', width: 160, render: tpl },
                { key: 'FaceTemplate', caption: 'FaceTemplate', width: 160, render: tpl }
            ],
            filterRow: true, checkAll: 'Active'
        });
        // The desktop adds a "Ping" button column but wires no ColumnButtonClick handler, so the button does
        // nothing there; here it says why the browser cannot do it either.
        HRM.$('grdMachineData').addEventListener('click', function (e) {
            if (e.target.closest('[data-ping]')) HRM.box('Ping needs a direct network connection to the attendance device (ZKTeco zkemkeeper on the desktop). A browser cannot reach the device; use "Pull Attendance" to queue a request for the attendance service.');
        });
        function bind(d) {
            var m = (d && d.machines) || [];
            if (m.length) grdMachineData.set(m.map(function (r) { r.AtPull = false; return r; })); else grdMachineData.clear();   // ClearStructure
            var e = (d && d.employees) || [];
            if (e.length) grdEmployeeList.set(e.map(function (r) { r.Active = false; return r; })); else grdEmployeeList.clear(); // DataSource = null
        }
        function Reset(btn) {                                                   // Reset(): EmployeeGridbind + MachineDataGridBind
            return HRM.busy(btn || 'btnnew', function () { return HRM.get(API + '/reload').then(bind).catch(HRM.fail); });
        }
        function MakeProductDeviceData(JobTask, btn) {                         // MakeProductDeviceData(JobTask)
            var machines = grdMachineData.checked('AtPull');
            if (machines.length !== 1) { HRM.box('Only one row select'); return; }
            var employees = grdEmployeeList.checked('Active');
            return HRM.busy(btn, function () {
                return HRM.post(API + '/job', {
                    jobTask: JobTask,
                    devices: machines.map(function (r) { return HRM.int(r.Id); }),                 // item.Cells[0] = Id
                    employeeNos: employees.map(function (r) { return HRM.str(r.EmployeeNo); })
                }).then(function (d) { HRM.box(d.message || 'Request successfuly submit'); }).catch(HRM.fail);
            }, 'device-job');
        }
        P.btnnew = function (btn) { return Reset(btn); };                                          // btnnew_Click
        P.btnPullAttendance = function (btn) { return MakeProductDeviceData('PullLog', btn); };   // btnPullAttendance_Click
        P.btnDownloadTemplate = function (btn) { return MakeProductDeviceData('Download', btn); }; // btnDownloadTemplate_Click
        P.button2 = function (btn) { return MakeProductDeviceData('Upload', btn); };               // button2_Click (Upload Template)
        P.button1 = function (btn) { return MakeProductDeviceData('Delete', btn); };               // button1_Click (Delete Template)
        HRM.keys({                                                                                 // AddressDetail_KeyDown
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        HRM.footer(null);                                                        // a device utility: no history of its own
        HRM.loading(HRM.get(API + '/setup')).then(bind).catch(HRM.fail);          // PullAttendanceByMachine_Load
    }

    var PAGES = { 'device-configuration': deviceConfiguration, 'pull-attendance-by-machine': pullAttendanceByMachine };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
