/* ============================================================================================
 * countx_hrm_profile.js - HRM "Profile Management" (AppModules 2018). One script, the page is
 * chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   employee-group     genEmployeeGroup.cs (634)
 *   employee-category  frmEmployeeCategory.cs (635)
 *   benefit            BenifitDefine.cs (636)
 *   designation        genDesignation.cs (637)
 *   department         genDepartment.cs (638)
 *   section            DefineSection.cs (639)
 *   location           frmgenLocation.cs (640)
 *   profile-type       PolicyManagment/ProfileTypes.cs (641)
 *   define-profile     PolicyManagment/ProfileDefine.cs (642)  (?profileTypeId= as the callers set profileTypeId)
 *
 * Each handler follows its desktop form line by line: validation wording and order, messages,
 * what New / Save / Update / double-click do to the buttons and the grid.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/profile/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmProfile
    window.HrmProfile = P;

    // ============================================================================ 637 Designation
    function designation() {
        var RecId = 0, UpdateMood = false, rights = {};
        var grd = new HRM.Grid('grd', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'Name', caption: 'Name', width: 500 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(HRM.int(r.Id)); }        // grd_DoubleClick
        });
        function GridFill() {                                                // GridFill: grid replaced only when rows came back
            return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); })
                .catch(HRM.fail);
        }
        function buttons(update) {
            HRM.show('btnsave', !update); HRM.show('btnupdate', update);
            HRM.show('BtnSavef', !update); HRM.show('btnUpdateF', update);
        }
        function Reset() {                                                   // Reset()
            HRM.setVal('txtDesignationName', '');
            buttons(false);
            UpdateMood = false; RecId = 0;
            HRM.focus('txtDesignationName');
            return GridFill();
        }
        function RetrivedData(id) {                                          // RetrivedData(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = id;
                HRM.setVal('txtDesignationName', HRM.str(HRM.col(o, 'DesignationName')));
                UpdateMood = true;
                buttons(true);
            }).catch(HRM.fail);
        }
        function formvalidation() {
            if (HRM.val('txtDesignationName').trim() === '') { HRM.box('Please Insert Designation Name'); HRM.focus('txtDesignationName'); return false; }
            return true;
        }
        function Insert(btn) {                                               // Insert()
            if (!formvalidation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', { id: RecId, name: HRM.val('txtDesignationName') }).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'designation-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { Reset(); };                                         // btnnew_Click
        HRM.keys({                                                                   // genDesignation_KeyDown
            'ctrl+s': function () { if (!UpdateMood) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (UpdateMood) P.btnupdate(); }
        });
        HRM.footer(function (b) { return HRM.busy(b, GridFill); });                  // History = the form's own History grid, refreshed
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // genDesignation_Load
            rights = d.rights || {};
            if (d.rows && d.rows.length) grd.set(d.rows);
            buttons(false);
            HRM.focus('txtDesignationName');
        }).catch(HRM.fail);
    }

    // ============================================================================ shared bits of the group
    /** The four Save / Update buttons of these forms (tool strip + bottom panel). */
    function buttons4(update) {
        HRM.show('btnsave', !update); HRM.show('btnupdate', update);
        HRM.show('BtnSavef', !update); HRM.show('btnUpdateF', update);
    }
    /** The footer History button: the form's own History grid (GridFill), refreshed and scrolled to. */
    function historyFooter(fill) {
        HRM.footer(function (b) {
            return HRM.busy(b, function () {
                return Promise.resolve(fill()).then(function () {
                    var box = document.getElementById('historyBox');
                    if (box && box.scrollIntoView) box.scrollIntoView({ block: 'start', behavior: 'smooth' });
                });
            });
        });
    }
    /** Form_KeyDown of every form here: Ctrl+S (!UpdateMode), Ctrl+N, Ctrl+E / Esc (Close), Ctrl+U (UpdateMode); Enter -> {TAB}. */
    function formKeys(isUpdate) {
        HRM.keys({
            'ctrl+s': function () { if (!isUpdate()) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (isUpdate()) P.btnupdate(); }
        });
    }
    P.BtnCancel = function () { HRM.close(); };                                      // BtnCancel_Click: Close()

    // ============================================================================ 634 Employee Group
    function employeeGroup() {
        var RecId = 0, UpdateMood = false;
        var grd = new HRM.Grid('grd', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'ShortName', caption: 'ShortName', width: 120 },
                { key: 'Name', caption: 'Name', width: 300 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(HRM.int(r.Id)); }                  // grd_DoubleClick
        });
        function GridFill() {                                                         // GridFill: replaced only when rows came back
            return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
        }
        function Reset() {                                                            // Reset()
            HRM.setVal('txtGroupName', ''); HRM.setVal('txtShortName', '');
            buttons4(false);
            UpdateMood = false; RecId = 0;
            HRM.focus('txtShortName');
            return GridFill();
        }
        function RetrivedData(id) {                                                   // RetrivedData(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = id;
                HRM.setVal('txtShortName', HRM.str(HRM.col(o, 'EmployeeGroupPrefix')));
                HRM.setVal('txtGroupName', HRM.str(HRM.col(o, 'EmployeeGroupName')));
                UpdateMood = true;
                buttons4(true);
            }).catch(HRM.fail);
        }
        function formvalidation() {
            if (HRM.val('txtShortName').trim() === '') { HRM.box('Please Insert Pre Name'); HRM.focus('txtShortName'); return false; }
            if (HRM.val('txtGroupName').trim() === '') { HRM.box('Please Insert Group Name'); HRM.focus('txtGroupName'); return false; }
            return true;
        }
        function Insert(btn) {                                                        // Insert()
            if (!formvalidation()) return;
            if (RecId <= 0) {                                                         // new record: the grid's Name column is checked first
                var name = HRM.val('txtGroupName'), rows = grd.rows();
                for (var i = 0; i < rows.length; i++) {
                    if (HRM.str(rows[i].Name) === name) { HRM.box('This Already already Exist'); return; }
                }
            }
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', { id: RecId, shortName: HRM.val('txtShortName'), name: HRM.val('txtGroupName') }).then(function (d) {
                    if (d && d.message) HRM.box(d.message);
                    return Reset();
                }).catch(HRM.fail);
            }, 'employee-group-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { Reset(); };                                         // btnnew_Click
        formKeys(function () { return UpdateMood; });                                // genDepartment_KeyDown (the form's handler name)
        historyFooter(GridFill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // genDepartment_Load (the form's handler name)
            if (d.rows && d.rows.length) grd.set(d.rows);
            HRM.show('BtnSavef', true); HRM.show('btnUpdateF', false);
            HRM.focus('txtShortName');
        }).catch(HRM.fail);
    }

    // ============================================================================ 635 Employee Category
    function employeeCategory() {
        var RecId = 0, UpdateMood = false;
        var grd = new HRM.Grid('grd', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'CategoryName', caption: 'CategoryName', width: 280 },
                { key: 'ShortName', caption: 'ShortName', width: 120 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(HRM.int(r.Id)); }                  // grd_DoubleClick
        });
        function GridFill() {                                                         // GridFill: rows or grd.ClearStructure()
            return HRM.get(API + '/list').then(function (rows) { grd.set(rows || []); }).catch(HRM.fail);
        }
        function toolButtons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }   // only the tool strip, as the form does
        function Reset() {                                                            // Reset()
            HRM.setVal('txtCategoryName', ''); HRM.setVal('txtShortName', '');
            toolButtons(false);
            UpdateMood = false; RecId = 0;
            HRM.focus('txtCategoryName');
            return GridFill();
        }
        function RetrivedData(id) {                                                   // RetrivedData(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = id;
                HRM.setVal('txtShortName', HRM.str(HRM.col(o, 'EmployeeCategoryPrefix')));
                HRM.setVal('txtCategoryName', HRM.str(HRM.col(o, 'EmployeeCategoryName')));
                UpdateMood = true;
                toolButtons(true);
            }).catch(HRM.fail);
        }
        function formvalidation() {
            if (HRM.val('txtShortName').trim() === '') { HRM.box('Please Insert Short Name'); HRM.focus('txtShortName'); return false; }
            if (HRM.val('txtCategoryName').trim() === '') { HRM.box('Please Insert Category Name'); HRM.focus('txtCategoryName'); return false; }
            return true;
        }
        function Insert(btn) {                                                        // Insert()
            if (!formvalidation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', { id: RecId, shortName: HRM.val('txtShortName'), name: HRM.val('txtCategoryName') }).then(function (d) {
                    if (d && d.message) HRM.box(d.message);
                    return Reset();
                }).catch(HRM.fail);
            }, 'employee-category-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click (also BtnSavef)
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click (also btnUpdateF)
        P.btnnew = function () { Reset(); };                                         // btnnew_Click
        formKeys(function () { return UpdateMood; });                                // genDepartment_KeyDown
        historyFooter(GridFill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // genDepartment_Load: GridFill()
            grd.set(d.rows || []);
            HRM.focus('txtCategoryName');
        }).catch(HRM.fail);
    }

    // ============================================================================ 636 Benefit
    function benefit() {
        var RecId = 0, UpdateMode = false;
        var grdfrm = new HRM.Grid('grdfrm', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'BenefitProfile', caption: 'BenefitProfile', width: 170 },
                { key: 'BenefitName', caption: 'BenefitName', width: 200 },
                { key: 'Prefix', caption: 'Prefix', width: 80 }
            ],
            filterRow: true,
            onDouble: function (r) { grdfrm_DoubleClick(r); }
        });
        function ProfileType(rows) {                                                  // ProfileType(): BindDDL(..., ZeroIndex false) when rows came back
            if (rows && rows.length) HRM.fill('cmbbeniftprofile', rows, 'Id', 'BenefitProfile', { zero: '', keep: true });
        }
        function gridfill() {                                                         // gridfill()
            return HRM.get(API + '/list').then(function (rows) { grdfrm.set(rows || []); }).catch(HRM.fail);
        }
        function clear() {                                                            // clear()
            buttons4(false);
            UpdateMode = false;
            HRM.setCombo('cmbbeniftprofile', 0);
            HRM.setVal('txtprefix', ''); HRM.setVal('txtname', '');
            HRM.focus('cmbbeniftprofile');
            return gridfill();
        }
        function formvalidation() {
            if (HRM.comboText('cmbbeniftprofile').trim() === '') { HRM.box('Please Select Benefit Profile'); HRM.focus('cmbbeniftprofile'); return false; }
            if (HRM.val('txtname').trim() === '') { HRM.box('Please Insert Benefit Name'); HRM.focus('txtname'); return false; }
            if (HRM.val('txtprefix').trim() === '') { HRM.box('Please Insert Prefix'); HRM.focus('txtprefix'); return false; }
            return true;
        }
        function save(btn, id) {
            if (!formvalidation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', { id: id, profileId: HRM.comboVal('cmbbeniftprofile'), name: HRM.val('txtname'), prefix: HRM.val('txtprefix') }).then(function (d) {
                    HRM.box(d && d.message ? d.message : (id > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return clear();
                }).catch(HRM.fail);
            }, 'benefit-save');
        }
        function grdfrm_DoubleClick(r) {                                              // grdfrm_DoubleClick
            HRM.show('btnsave', false);
            UpdateMode = true;
            RecId = HRM.int(r.Id);
            HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (cat) {
                HRM.setCombo('cmbbeniftprofile', HRM.int(HRM.col(cat, 'BenefitTypeProfileId')));
                HRM.setVal('txtname', HRM.str(HRM.col(cat, 'BenefitName')));
                HRM.setVal('txtprefix', HRM.str(HRM.col(cat, 'BenefitPrefix')));
                gridfill();
                HRM.show('btnupdate', true); HRM.show('BtnSavef', false); HRM.show('btnUpdateF', true);
                HRM.focus('cmbbeniftprofile');
            }).catch(HRM.fail);
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', 0); };                 // btnsave_Click
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', RecId); };         // btnupdate_Click
        P.btnnew = function () { clear(); };                                             // btnnew_Click
        P.btnbenifit = function () { HRM.open('/hrm/define-profile?profileTypeId=1'); };  // btnbenifit_Click: ProfileDefine { profileTypeId = 1 }.Show()
        P.btnRefresh = function (btn) {                                                  // btnRefresh_Click: ProfileType()
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/combos').then(function (d) { ProfileType(d.profiles); }).catch(HRM.fail);
            });
        };
        formKeys(function () { return UpdateMode; });                                    // BenifitDefine_KeyDown
        historyFooter(gridfill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                         // BenifitDefine_Load
            buttons4(false);
            grdfrm.set(d.rows || []);
            ProfileType(d.profiles);
            HRM.focus('cmbbeniftprofile');
        }).catch(HRM.fail);
    }

    // ============================================================================ 638 Department
    function department() {
        var RecId = 0, UpdateMood = false, canCoa = false;
        var grd = new HRM.Grid('grd', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'Type', caption: 'Type', width: 130 },
                { key: 'ShortName', caption: 'ShortName', width: 90 },
                { key: 'Name', caption: 'Name', width: 180 },
                { key: 'Payable Account', caption: 'Payable Account', width: 180 },
                { key: 'Expense Account', caption: 'Expense Account', width: 180 },
                { key: 'Loan Account', caption: 'Loan Account', width: 180 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(HRM.int(r.Id)); }                      // grd_DoubleClick (also Ctrl+Enter / Enter on the grid)
        });
        function AccountBinds(d) {                                                        // AccountBinds(): BindDDLNew(..., ZeroIndex true) when rows came back
            if (d.expense && d.expense.length) HRM.fill('CmbSalariesExpensesAc', d.expense, 'Id', 'AccountTitle', { zero: '...Select Any Value...' });
            if (d.payable && d.payable.length) {
                HRM.fill('CmbSalaryPayableAc', d.payable, 'Id', 'AccountTitle', { zero: '...Select Any Value...' });
                HRM.fill('CmbSalaryLoanAc', d.payable, 'Id', 'AccountTitle', { zero: '...Select Any Value...' });
            }
        }
        function DepartmentTypeFill(d) {                                                  // DepartmentTypeFill(): BindDDLNew(false) + Rows[0].Activate()
            if (d.types && d.types.length) HRM.fill('cmbDepartmentType', d.types, 'ProfileId', 'ProfileName', { zero: false });
        }
        function GridFill() {                                                             // GridFill: replaced only when rows came back
            return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
        }
        function Reset() {                                                                // Reset()
            buttons4(false);
            HRM.setVal('txtDepartmentName', ''); HRM.setVal('txtShortName', '');
            HRM.setCombo('CmbSalariesExpensesAc', 0);
            UpdateMood = false; RecId = 0;
            HRM.focus('cmbDepartmentType');
            return GridFill();
        }
        function RetrivedData(id) {                                                       // RetrivedData(Id)
            HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = id;
                HRM.setCombo('cmbDepartmentType', HRM.int(HRM.col(o, 'DepartmentTypeProfileId')));
                HRM.setVal('txtShortName', HRM.str(HRM.col(o, 'ShortName')));
                HRM.setVal('txtDepartmentName', HRM.str(HRM.col(o, 'DepartmentName')));
                HRM.setCombo('CmbSalaryLoanAc', HRM.int(HRM.col(o, 'LoanAcId')));
                HRM.setCombo('CmbSalaryPayableAc', HRM.int(HRM.col(o, 'PayableAcId')));
                HRM.setCombo('CmbSalariesExpensesAc', HRM.int(HRM.col(o, 'ExpenseAccountId')));
                UpdateMood = true;
                buttons4(true);
            }).catch(HRM.fail);
        }
        function formvalidation() {
            if (HRM.comboText('cmbDepartmentType').trim() === '' || HRM.comboVal('cmbDepartmentType') === 0) {
                HRM.box('Please Select Department Type'); HRM.focus('cmbDepartmentType'); return false;
            }
            if (HRM.val('txtShortName').trim() === '') { HRM.box('Please Insert Short Name'); HRM.focus('txtShortName'); return false; }
            if (HRM.val('txtDepartmentName').trim() === '') { HRM.box('Please Insert Department Name'); HRM.focus('txtDepartmentName'); return false; }
            return true;
        }
        function Insert(btn) {                                                            // Insert()
            if (!formvalidation()) return;
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: RecId, typeId: HRM.comboVal('cmbDepartmentType'),
                    shortName: HRM.val('txtShortName'), name: HRM.val('txtDepartmentName'),
                    expenseAccountId: HRM.comboVal('CmbSalariesExpensesAc'),
                    payableAcId: HRM.comboVal('CmbSalaryPayableAc'),
                    loanAcId: HRM.comboVal('CmbSalaryLoanAc')
                }).then(function (d) {
                    if (d && d.message) HRM.box(d.message);
                    return Reset();
                }).catch(HRM.fail);
            }, 'department-save');
        }
        function coa() {                                                                  // btnCoa1_Click (btnCoa1 / btncoa2 / btncoa3)
            if (canCoa) HRM.open('/accounts/chart_of_accounts');
            else HRM.box('You Dont Have rights View Of This Form..');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };     // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };           // btnupdate_Click
        P.btnnew = function () { Reset(); };                                           // btnnew_Click
        P.btnrefresh = function (btn) {                                                // btnrefresh_Click: AccountBinds(); GridFill(); DepartmentTypeFill()
            return HRM.busy(btn || 'btnrefresh', function () {
                return HRM.get(API + '/combos').then(function (d) {
                    AccountBinds(d);
                    return GridFill().then(function () { DepartmentTypeFill(d); });
                }).catch(HRM.fail);
            });
        };
        P.btnDepartmenTtype = function () { HRM.open('/hrm/define-profile?profileTypeId=65'); };   // ProfileDefine { profileTypeId = 65 }.Show()
        P.btnCoa1 = coa; P.btncoa2 = coa; P.btncoa3 = coa;
        formKeys(function () { return UpdateMood; });                                  // genDepartment_KeyDown
        historyFooter(GridFill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                       // genDepartment_Load
            canCoa = !!d.canCoa;
            AccountBinds(d);
            if (d.rows && d.rows.length) grd.set(d.rows);
            DepartmentTypeFill(d);
            buttons4(false);
        }).catch(HRM.fail);
    }

    // ============================================================================ 639 Section
    function section() {
        var RecId = 0, UpdateMode = false;
        var grdfrm = new HRM.Grid('grdfrm', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'DepartmentName', caption: 'DepartmentName', width: 150 },
                { key: 'ShortName', caption: 'ShortName', width: 90 },
                { key: 'SectionName', caption: 'SectionName', width: 160 },
                { key: 'Code', caption: 'Code', width: 80 }
            ],
            filterRow: true,
            onDouble: function (r) { grdfrm_DoubleClick(r); }
        });
        function DepartmentFill(rows) {                                                // DepartmentFill(): BindDDL(..., ZeroIndex false) when rows came back
            if (rows && rows.length) HRM.fill('cmbDepartment', rows, 'Id', 'DepartmentName', { zero: '' });
        }
        function gridfill() {                                                          // gridfill(): rows or grdfrm.ClearStructure()
            return HRM.get(API + '/list').then(function (rows) { grdfrm.set(rows || []); }).catch(HRM.fail);
        }
        function clear() {                                                             // clear()
            buttons4(false);
            UpdateMode = false;
            HRM.setCombo('cmbDepartment', 0);
            HRM.setVal('txtSectionName', ''); HRM.setVal('txtShortName', ''); HRM.setVal('txtCode', '');
            HRM.focus('cmbDepartment');
            return gridfill();
        }
        function formvalidation() {                                                    // the desktop's own (copied) texts
            if (HRM.comboText('cmbDepartment').trim() === '') { HRM.box('Please Select Benefit Profile'); HRM.focus('cmbDepartment'); return false; }
            if (HRM.val('txtShortName').trim() === '') { HRM.box('Please Insert Benefit Name'); HRM.focus('txtShortName'); return false; }
            if (HRM.val('txtSectionName').trim() === '') { HRM.box('Please Insert Prefix'); HRM.focus('txtSectionName'); return false; }
            return true;
        }
        function save(btn, id) {
            if (!formvalidation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: id, departmentId: HRM.comboVal('cmbDepartment'),
                    shortName: HRM.val('txtShortName'), name: HRM.val('txtSectionName'), code: HRM.val('txtCode')
                }).then(function (d) {
                    if (d && d.message) { HRM.box(d.message); return clear(); }        // only when Save(...) > 0
                }).catch(HRM.fail);
            }, 'section-save');
        }
        function grdfrm_DoubleClick(r) {                                               // grdfrm_DoubleClick
            buttons4(true);
            UpdateMode = true;
            RecId = HRM.int(r.Id);
            HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (cat) {
                HRM.setCombo('cmbDepartment', HRM.int(HRM.col(cat, 'DepartmentId')));
                HRM.setVal('txtShortName', HRM.str(HRM.col(cat, 'ShortName')));
                HRM.setVal('txtSectionName', HRM.str(HRM.col(cat, 'SectionName')));
                HRM.setVal('txtCode', HRM.str(HRM.col(cat, 'SectionCode')));
            }).catch(HRM.fail);
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', 0); };             // btnsave_Click
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', RecId); };     // btnupdate_Click
        P.btnnew = function () { clear(); };                                         // btnnew_Click
        P.btnLocation = function () { HRM.open('/hrm/department'); };                // btnLocation_Click: new genDepartment(...).Show()
        formKeys(function () { return UpdateMode; });                                // BenifitDefine_KeyDown (the form's handler name)
        historyFooter(gridfill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // BenifitDefine_Load: gridfill(); DepartmentFill()
            buttons4(false);
            grdfrm.set(d.rows || []);
            DepartmentFill(d.departments);
        }).catch(HRM.fail);
    }

    // ============================================================================ 640 Location
    function locationPage() {
        var RecId = 0, UpdateMode = false;
        var TEXTS = ['txtLocationName', 'txtShortName', 'txtSubLocationName', 'txtEmail', 'txtMobile1', 'txtMobile2', 'txtPortalURL', 'txtLicenseNo', 'txtAddressDetail'];
        var grd = new HRM.Grid('grd', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'LocationType', caption: 'LocationType', width: 120 },
                { key: 'LocationName', caption: 'LocationName', width: 160 },
                { key: 'ShortName', caption: 'ShortName', width: 90 },
                { key: 'SubName', caption: 'SubName', width: 120 },
                { key: 'Mobile1', caption: 'Mobile1', width: 100 },
                { key: 'Mobile2', caption: 'Mobile2', width: 100 },
                { key: 'Email', caption: 'Email', width: 150 },
                { key: 'PortalURL', caption: 'PortalURL', width: 150 },
                { key: 'AddressDetail', caption: 'AddressDetail', width: 200 },
                { key: 'LicenseNo', caption: 'LicenseNo', width: 100 }
            ],
            filterRow: true,
            onDouble: function (r) { RetrivedData(r); }                                 // grd_DoubleClick
        });
        function LocationTypeFill(rows) {                                               // LocationTypeFill(): BindDDLNew(false) when rows came back
            if (rows && rows.length) HRM.fill('cmbLocationType', rows, 'ProfileId', 'ProfileName', { zero: '', keep: true });
        }
        function gridFill() {                                                           // gridFill: replaced only when rows came back
            return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
        }
        function Reset() {                                                              // Reset()
            TEXTS.forEach(function (t) { HRM.setVal(t, ''); });
            HRM.setCombo('cmbLocationType', 0);
            HRM.focus('txtLocationName');
            RecId = 0;
            buttons4(false);
            UpdateMode = false;
        }
        function RetrivedData(r) {                                                      // RetrivedData(): the desktop keeps Save shown / Update hidden here
            buttons4(false);
            UpdateMode = true;
            RecId = HRM.int(r.Id);
            HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (o) {
                HRM.setVal('txtLocationName', HRM.str(HRM.col(o, 'LocationName')));
                HRM.setVal('txtShortName', HRM.str(HRM.col(o, 'LocationShortName')));
                HRM.setVal('txtSubLocationName', HRM.str(HRM.col(o, 'SubLocationName')));
                HRM.setVal('txtEmail', HRM.str(HRM.col(o, 'Email')));
                HRM.setCombo('cmbLocationType', HRM.int(HRM.col(o, 'LocationTypeProfileId')));
                HRM.setVal('txtMobile1', HRM.str(HRM.col(o, 'Mobile1')));
                HRM.setVal('txtMobile2', HRM.str(HRM.col(o, 'Mobile2')));
                HRM.setVal('txtPortalURL', HRM.str(HRM.col(o, 'PortalURL')));
                HRM.setVal('txtLicenseNo', HRM.str(HRM.col(o, 'LicenseNo')));
                HRM.setVal('txtAddressDetail', HRM.str(HRM.col(o, 'HeaderDetail')));
            }).catch(HRM.fail);
        }
        function formValidation() {
            var n = HRM.val('txtLocationName');
            if (n === '' || n === '0') { HRM.box('Location Name Required'); HRM.focus('txtLocationName'); return false; }
            if (HRM.comboText('cmbLocationType').trim() === '' || HRM.comboVal('cmbLocationType') === 0) { HRM.box('Location Type'); HRM.focus('cmbLocationType'); return false; }
            return true;
        }
        function save(btn, id, question) {
            if (!formValidation()) return;
            if (!HRM.ask(question)) return;
            var b = {
                id: id, name: HRM.val('txtLocationName'), shortName: HRM.val('txtShortName'), subName: HRM.val('txtSubLocationName'),
                email: HRM.val('txtEmail'), mobile1: HRM.val('txtMobile1'), mobile2: HRM.val('txtMobile2'), portalUrl: HRM.val('txtPortalURL'),
                licenseNo: HRM.val('txtLicenseNo'), addressDetail: HRM.val('txtAddressDetail'), typeId: HRM.comboVal('cmbLocationType')
            };
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', b).then(function (d) {
                    if (d && d.message) { HRM.box(d.message); Reset(); return gridFill(); }
                }).catch(HRM.fail);
            }, 'location-save');
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', 0, 'Are you sure to Save?'); };            // btnsave_Click
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', RecId, 'Are you sure to Update?'); }; // btnupdate_Click
        P.btnnew = function () { Reset(); };                                                                // btnnew_Click
        P.btnrefresh = function (btn) {                                                                     // btnrefresh_Click: LocationTypeFill()
            return HRM.busy(btn || 'btnrefresh', function () {
                return HRM.get(API + '/combos').then(function (d) { LocationTypeFill(d.types); }).catch(HRM.fail);
            });
        };
        formKeys(function () { return UpdateMode; });                                                       // LeaveQuotaPolicy_KeyDown (the form's handler name)
        historyFooter(gridFill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                                            // LeaveQuotaPolicy_Load
            LocationTypeFill(d.types);
            if (d.rows && d.rows.length) grd.set(d.rows);
            buttons4(false);
        }).catch(HRM.fail);
    }

    // ============================================================================ 641 Profile Type
    function profileType() {
        var RecId = 0, UpdateMode = false;
        var grdfrm = new HRM.Grid('grdfrm', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'ProfileType', caption: 'ProfileType', width: 260 },
                { key: 'Prefix', caption: 'Prefix', width: 100 }
            ],
            filterRow: true,
            onDouble: function (r) { grdfrm_DoubleClick(r); }
        });
        function gridfill() {                                                           // gridfill(): the table is always rebound
            return HRM.get(API + '/list').then(function (rows) { grdfrm.set(rows || []); }).catch(HRM.fail);
        }
        function clear() {                                                              // clear()
            buttons4(false);
            UpdateMode = false;
            HRM.setVal('txtprefix', ''); HRM.setVal('txtprofilename', '');
            HRM.focus('txtprofilename');
            return gridfill();
        }
        function formvalidation() {
            if (HRM.val('txtprofilename').trim() === '') { HRM.box('Please Insert ProfileName'); HRM.focus('txtprofilename'); return false; }
            if (HRM.val('txtprefix').trim() === '') { HRM.box('Please Insert Prefix'); HRM.focus('txtprefix'); return false; }
            return true;
        }
        function save(btn, id) {
            if (!formvalidation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', { id: id, shortName: HRM.val('txtprefix'), name: HRM.val('txtprofilename') }).then(function (d) {
                    if (d && d.message) { HRM.box(d.message); return clear(); }         // only when Save(...) > 0
                }).catch(HRM.fail);
            }, 'profile-type-save');
        }
        function grdfrm_DoubleClick(r) {                                                // grdfrm_DoubleClick
            UpdateMode = true;
            RecId = HRM.int(r.Id);
            HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (cat) {
                HRM.setVal('txtprefix', HRM.str(HRM.col(cat, 'Prefix')));
                HRM.setVal('txtprofilename', HRM.str(HRM.col(cat, 'ProfileTypeName')));
                gridfill();
                buttons4(true);
            }).catch(HRM.fail);
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', 0); };               // btnsave_Click
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', RecId); };       // btnupdate_Click
        P.btnnew = function () { clear(); };                                           // btnnew_Click
        formKeys(function () { return UpdateMode; });                                  // ProfileTypes_KeyDown
        historyFooter(gridfill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                       // ProfileTypes_Load
            grdfrm.set(d.rows || []);
            buttons4(false);
            HRM.focus('txtprofilename');
        }).catch(HRM.fail);
    }

    // ============================================================================ 642 Define Profile
    function defineProfile() {
        var RecId = 0, UpdateMode = false, dtPrfile = [];
        var grdfrm = new HRM.Grid('grdfrm', {
            columns: [
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'ProfileType', caption: 'ProfileType', width: 150 },
                { key: 'Profile', caption: 'Profile', width: 170 },
                { key: 'Prefix', caption: 'Prefix', width: 80 },
                { key: 'IsActive', caption: 'IsActive', width: 70, type: 'check' }
            ],
            filterRow: true,
            onDouble: function (r) { grdfrm_DoubleClick(r); }
        });
        function show(rows) { if (rows && rows.length) grdfrm.set(rows); }            // gridfill(): rebound only when dtPrfile has rows
        function gridfill() {                                                         // gridfill()
            return HRM.get(API + '/list').then(function (rows) { dtPrfile = rows || []; show(dtPrfile); }).catch(HRM.fail);
        }
        function cmbprofiletype_TextChanged() {                                       // cmbprofiletype_TextChanged: grid = dtPrfile rows of the chosen type
            var t = HRM.comboText('cmbprofiletype');
            if (HRM.comboVal('cmbprofiletype') === 0 || t === '') return;
            if (dtPrfile.length > 0) {
                grdfrm.set(dtPrfile.filter(function (r) { return HRM.str(r.ProfileType) === t.trim(); }));
            } else {
                gridfill();
            }
        }
        function clear() {                                                            // clear()
            UpdateMode = false;
            buttons4(false);
            HRM.setCombo('cmbprofiletype', 0);
            HRM.setVal('txtprefix', ''); HRM.setVal('txtprofilename', '');
            var p = gridfill();
            HRM.focus('cmbprofiletype');
            return p;
        }
        function formvalidation() {
            if (HRM.comboText('cmbprofiletype').trim() === '') { HRM.box('Please Select Profil Type'); HRM.focus('cmbprofiletype'); return false; }
            if (HRM.val('txtprofilename').trim() === '') { HRM.box('Please Insert Profile Name'); HRM.focus('txtprofilename'); return false; }
            if (HRM.val('txtprefix').trim() === '') { HRM.box('Please Insert Prefix'); HRM.focus('txtprefix'); return false; }
            return true;
        }
        function save(btn, id) {
            if (!formvalidation()) return;
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', {
                    id: id, profileTypeId: HRM.comboVal('cmbprofiletype'),
                    name: HRM.val('txtprofilename'), prefix: HRM.val('txtprefix'), isActive: HRM.checked('chkactive')
                }).then(function (d) {
                    HRM.box(d && d.message ? d.message : (id > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return clear();
                }).catch(HRM.fail);
            }, 'define-profile-save');
        }
        function grdfrm_DoubleClick(r) {                                              // grdfrm_DoubleClick
            UpdateMode = true;
            RecId = HRM.int(r.Id);
            HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (cat) {
                HRM.setCombo('cmbprofiletype', HRM.int(HRM.col(cat, 'ProfileTypeId')));
                cmbprofiletype_TextChanged();
                HRM.setVal('txtprefix', HRM.str(HRM.col(cat, 'ProfilePrefix')));
                HRM.setVal('txtprofilename', HRM.str(HRM.col(cat, 'ProfileName')));
                HRM.check('chkactive', HRM.bool(HRM.col(cat, 'IsActive')));
                gridfill();
                buttons4(true);
            }).catch(HRM.fail);
        }
        P.btnsave = function (btn) { return save(btn || 'btnsave', 0); };             // btnsave_Click
        P.btnupdate = function (btn) { return save(btn || 'btnupdate', RecId); };     // btnupdate_Click
        P.btnnew = function () { clear(); HRM.focus('cmbprofiletype'); };             // btnnew_Click
        P.btnbenifit = function () { HRM.open('/hrm/profile-type'); };               // btnbenifit_Click: new ProfileTypes(...).Show()
        var cmb = document.getElementById('cmbprofiletype');
        if (cmb) cmb.addEventListener('change', cmbprofiletype_TextChanged);
        formKeys(function () { return UpdateMode; });                                // ProfileDefine_KeyDown
        historyFooter(gridfill);
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // ProfileDefine_Load
            if (d.types && d.types.length) HRM.fill('cmbprofiletype', d.types, 'ProfileTypeId', 'ProfileTypeName', { zero: '' });   // ProfileType(): BindDDLNew(false)
            dtPrfile = d.rows || [];                                                 // gridfill()
            show(dtPrfile);
            buttons4(false);
            var profileTypeId = HRM.int(HRM.param('profileTypeId'));                 // opened with profileTypeId (BenifitDefine 1, genDepartment 65)
            if (profileTypeId > 0) { HRM.setCombo('cmbprofiletype', profileTypeId); cmbprofiletype_TextChanged(); }
        }).catch(HRM.fail);
    }

    var PAGES = {
        'employee-group': employeeGroup, 'employee-category': employeeCategory, benefit: benefit,
        designation: designation, department: department, section: section, location: locationPage,
        'profile-type': profileType, 'define-profile': defineProfile
    };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
