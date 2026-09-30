/* ============================================================================================
 * countx_hrm_employee.js - HRM "Employee Management" (AppModules 2020). One script, the page is
 * chosen by <body data-hrm="...">. Built on countx_hrm.js (window.HRM).
 *
 *   employee-registration   frmEmployeeRegistration.cs (648)   ?id=<EmployeeId> opens that employee
 *
 * Sections (in form order):
 *   1 state, tabs, masks            6 Week Days tab                     11 Bank Account tab
 *   2 combos (the *ComboFill set)   7 Address Detail tab                12 RetrivedDataEmployeeRegistration
 *   3 header panel4 + Reset         8 Experience tab                    13 Save / Update (btnsave_Click)
 *   4 Employee Information tab      9 Education tab                     14 Print, History, toolbar, define buttons
 *   5 Employee Salary + Benefits   10 Family + Reference tabs           15 keys and form Load
 *
 * Each handler names the desktop method it ports; validation wording and order, messages and the
 * Add / Update / Cancel button visibility are the form's.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/employee/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmEmployee
    window.HrmEmployee = P;

    // ============================================================================ 648 Employee Registration
    function employeeRegistration() {

        // ======================================================================== 1 state, tabs, masks
        var RecId = 0, UpdateMode = false, rights = {}, links = {}, L = {};
        var fresh = { directBenefits: [], assetBenefits: [], weekDays: [] };   // the last grdEmployee*BenefitFill / grdWeekDaysFill result
        var persons = [];                                                      // dtReportedPersone (Approval Person value list)
        var lastNoLookup = '';

        function clone(rows) { return (rows || []).map(function (r) { return Object.assign({}, r); }); }

        /** tabControl2 / 3 / 4: a click shows its page; showTab(pageId) also opens the parent tab (a Focus() on a hidden control). */
        function wireTabs() {
            document.querySelectorAll('[data-tabs]').forEach(function (bar) {
                bar.addEventListener('click', function (e) {
                    var b = e.target.closest('.hrm-tab'); if (!b) return;
                    showTab(b.getAttribute('data-tab'));
                });
            });
        }
        function showTab(pageId) {
            var page = document.getElementById(pageId); if (!page) return;
            var group = page.getAttribute('data-tabs-page');
            document.querySelectorAll('[data-tabs-page="' + group + '"]').forEach(function (p) { p.classList.toggle('is-active', p === page); });
            document.querySelectorAll('[data-tabs="' + group + '"] .hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === pageId); });
            var parent = page.parentElement && page.parentElement.closest('.hrm-tab-page');
            if (parent) showTab(parent.id);
        }
        function focusIn(pageId, id) { showTab(pageId); HRM.focus(id); }

        /** MaskedTextBox: txtCNIC "00000-0000000-0", txtMobile1 "0000 0000000" (digits only, literals inserted). */
        function mask(id, groups, sep) {
            var el = HRM.$(id); if (!el) return;
            var max = groups.reduce(function (a, b) { return a + b; }, 0);
            el.addEventListener('input', function () {
                var d = el.value.replace(/\D/g, '').slice(0, max), out = '', pos = 0;
                for (var g = 0; g < groups.length && pos < d.length; g++) {
                    if (g > 0) out += sep;
                    out += d.slice(pos, pos + groups[g]); pos += groups[g];
                }
                el.value = out;
            });
        }

        // ======================================================================== 2 combos
        /** DDL.BindDDL / BindDDLNew with ZeroIndex false: no row selected (a blank slot); only when the table has rows. */
        function bind(id, rows, v, t, keep) {
            if (!rows || !rows.length) return;
            HRM.fill(id, rows, v, t, { zero: '', keep: keep });
        }
        /** AccountBinds: BindDDLNew(..., true) -> "...Select Any Value..." row, Value = 0. */
        function bindAccount(id, rows) {
            if (!rows || !rows.length) return;
            HRM.fill(id, rows, 'Id', 'AccountTitle', { zero: '...Select Any Value...' });
        }
        function fillCombos(l, keep) {
            L = l || {};
            HRM.fill('comproject', L.projects || [], 'Id', 'ProjectName', { zero: false });         // Project(): Rows[0].Activate()
            HRM.fill('combranches', L.branches || [], 'Id', 'BranchName', { zero: false });         // braches(): Rows[0].Activate()
            bindAccount('CmbSalariesExpensesAc', L.expenseAccounts);                               // AccountBinds "11,12,20,21"
            bindAccount('CmbSalaryPayableAc', L.payableAccounts);                                  //              "5,8"
            bindAccount('CmbSalaryLoanAc', L.payableAccounts);
            persons = L.reportedPersons || [];                                                     // AllLookUpFill -> dtReportedPersone (type 34)
            bind('cmbEmployeeType', L.employeeTypes, 'Id', 'Name', keep);                          // EmployeeTypeComboFill (20)
            bind('cmbNationality', L.nationalities, 'Id', 'Name', keep);                           // NationalityComboFill (15)
            bind('cmbBloodGroup', L.bloodGroups, 'Id', 'Name', keep);                              // BloodGroupComboFill (22)
            bind('cmbEmployeeCatagory', L.employeeCategories, 'Id', 'EmployeeCategory', keep);     // EmployeeCatagoryComboFill
            bind('cmbTitle', L.titles, 'Id', 'Name', keep);                                        // EmployeeTitleComboFill (24)
            HRM.fill('cmbRelationTitle', L.relations || [], 'Id', 'Name', { zero: '', keep: keep });   // EmployeeRelationComboFill (25), no row guard
            HRM.fill('cmbRelationShip', L.relations || [], 'Id', 'Name', { zero: '', keep: keep });
            bind('cmbGender', L.genders, 'Id', 'Name', keep);                                      // EmployeeGenderComboFill (26)
            bind('cmbDepartment', L.departments, 'DepartmentId', 'DepartmentName', keep);          // DepartmentNameComboFill
            bind('cmbLocation', L.locations, 'LocationId', 'LocationName', keep);                  // LocationNameComboFill
            bind('cmbReportedBranch', L.reportedBranches, 'Id', 'Name', keep);                     // ReportedBranchComboFill (28)
            bind('cmbSection', L.sections, 'SectionId', 'SectionName', keep);                      // SectionNameComboFill
            bind('cmbStore', L.stores, 'Id', 'Name', keep);                                        // StoreNameComboFill (31, hidden)
            bind('cmbDesignation', L.designations, 'DesignationId', 'DesignationName', keep);      // DesignationNameComboFill (three combos)
            bind('cmbDesignationEmployeeExperience', L.designations, 'DesignationId', 'DesignationName', keep);
            bind('cmbDesignationEmployeeRefrence', L.designations, 'DesignationId', 'DesignationName', keep);
            bind('cmbShift', L.shifts, 'ShiftId', 'ShiftName', keep);                              // ShiftNameComboFill
            bind('cmbReportedPerson', persons, 'Id', 'Name', keep);                                // ReportedPersonComboFill (34)
            bind('cmbAddressType', L.addressTypes, 'Id', 'Name', keep);                            // AddressTypeComboFill (59)
            bind('cmbCity', L.cities, 'Id', 'CityName', keep);                                     // CityFill (three combos)
            bind('cmbCityEmployeeExperience', L.cities, 'Id', 'CityName', keep);
            bind('cmbCityEducation', L.cities, 'Id', 'CityName', keep);
            bind('cmbCountry', L.countries, 'Id', 'Description', keep);                            // CountryFill
            bind('cmbProvince', L.provinces, 'Id', 'Description', keep);                           // ProvinceFill
            bind('cmbDegree', L.degrees, 'Id', 'Name', keep);                                      // DegreeFill (57)
            bind('cmbRelationShip', L.relationships, 'Id', 'Name', keep);                          // RelationShipFill (47) replaces the 25 list when it has rows
            bind('cmbBankAccount', L.banks, 'Id', 'BranchName', keep);                             // BankName
            bind('cmbEmployeeGroup', L.employeeGroups, 'EmployeeGroupId', 'EmployeeGroupName', keep);  // EmployeeGroupComboFill
            if (grdDirect) { grdDirect.draw(); grdAssets.draw(); }                                 // the Approval Person value list
        }
        function firstRow(id) { HRM.setCombo(id, 0); }                                              // UltraCombo.Rows[0].Activate() (the zero row)
        function clearCombo(id) { HRM.setCombo(id, 0); }                                           // UltraCombo.Text = ""

        // ======================================================================== 3 header (panel4) + Reset
        function buttons(update) {
            HRM.show('btnsave', !update); HRM.show('btnupdate', update);
        }
        function picture(path, box) {
            var el = HRM.$(box); if (!el) return;
            var name = HRM.str(path).split(/[\\/]/).pop();
            el.textContent = name ? name : '';
            el.title = HRM.str(path);
        }
        /** Reset(): header fields, Employee No regenerated, pictures, all detail tables and the salary grid cleared. */
        function Reset(employeeNo) {
            RecId = 0;
            HRM.setVal('txtEmployeeNoForUpdate', ''); lastNoLookup = '';
            HRM.setVal('txtEmployeeNo', employeeNo === undefined ? '' : employeeNo);            // EmlpoyeeNoGenerate
            ['cmbEmployeeType', 'cmbEmployeeCatagory', 'cmbTitle', 'cmbRelationTitle', 'cmbGender', 'cmbNationality', 'cmbBloodGroup'].forEach(clearCombo);
            ['txtFirstName', 'txtMiddleName', 'txtLastName', 'txtRelationName', 'txtRelationMobileNo', 'txtEmail', 'txtMobile1', 'txtMobile2',
                'txtCNIC', 'txtPassportNo', 'txtLicenseNo'].forEach(function (id) { HRM.setVal(id, ''); });
            HRM.setVal('dtpDOB', HRM.today());
            HRM.setVal('dtpCNICIssueDate', HRM.today());
            HRM.setVal('dtpCNICExpiryDate', HRM.today());
            picture('', 'pictureBoxEmployeePhoto'); picture('', 'pictureBoxEmployeeSignature');
            buttons(false);
            UpdateMode = false;
            grdBank.clear(); grdAddress.clear(); grdExperience.clear(); grdFamily.clear(); grdReference.clear(); grdEducation.clear();
            grdSalary.clear();
            HRM.focus('txtEmployeeNo');
        }
        /** ValidationEmployeeRegistration (order and wording of the form). */
        function ValidationEmployeeRegistration() {
            function fail(m, page, id) { HRM.box(m); if (page) focusIn(page, id); else HRM.focus(id); return false; }
            if (HRM.int(HRM.val('txtEmployeeNo')) === 0) return fail('EmployeeNo Required!', null, 'txtEmployeeNo');
            if (HRM.comboVal('cmbEmployeeType') === 0) return fail('Employee Type Required!', 'tabPage5', 'cmbEmployeeType');
            if (HRM.comboVal('cmbEmployeeCatagory') === 0) return fail('Employee Catagory Required!', null, 'cmbEmployeeCatagory');
            if (HRM.comboVal('cmbTitle') === 0) return fail('Employee Title Required!', null, 'cmbTitle');
            if (HRM.val('txtFirstName').trim() === '') return fail('FirstName Required!', null, 'txtFirstName');
            if (HRM.val('txtLastName').trim() === '') return fail('LastName Required!', null, 'txtLastName');
            if (HRM.comboVal('cmbGender') === 0) return fail('Gender Required!', null, 'cmbGender');
            if (HRM.comboVal('cmbNationality') === 0) return fail('Nationality Required!', null, 'cmbNationality');
            if (HRM.val('txtCNIC').trim() === '') return fail('CNIC Required!', null, 'txtCNIC');
            if (HRM.val('txtMobile1').trim() === '') return fail('Mobile1 Required!', null, 'txtMobile1');
            if (HRM.comboVal('cmbDepartment') === 0) return fail('Department Required!', 'tabPage5', 'cmbDepartment');
            if (HRM.comboVal('CmbSalaryPayableAc') === 0) return fail('Payables Account Field Required', 'tabPage5', 'CmbSalaryPayableAc');
            if (HRM.comboVal('CmbSalariesExpensesAc') === 0) return fail('Expense Account Field Required', 'tabPage5', 'CmbSalariesExpensesAc');
            if (HRM.comboVal('CmbSalaryLoanAc') === 0) return fail('Loan Account Field Required', 'tabPage5', 'CmbSalaryLoanAc');
            return true;
        }
        /** txtEmployeeNoForUpdate_Leave: GetEmployeeIdByEmployeeNo -> RetrivedDataEmployeeRegistration, else "Record Not Found For this EmployeeNo". */
        function txtEmployeeNoForUpdate_Leave() {
            var no = HRM.val('txtEmployeeNoForUpdate').trim();
            if (no === '' || no === lastNoLookup) return;
            lastNoLookup = no;
            HRM.loading(HRM.get(API + '/by-no', { employeeNo: no })).then(function (d) {
                return RetrivedDataEmployeeRegistration(HRM.int(d.id));
            }).catch(HRM.fail);
        }

        // ======================================================================== 4 Employee Information tab (panel10)
        /** ResetEmployeeInformation(): dates = today, combos cleared, Is Active off, the three accounts on their first row. */
        function ResetEmployeeInformation() {
            HRM.setVal('txtStartDate', HRM.today());
            HRM.setVal('txtEndDate', HRM.today());
            HRM.setVal('txtProbationEndingDate', HRM.today());
            ['cmbDesignation', 'cmbShift', 'cmbDepartment', 'cmbLocation', 'cmbSection', 'cmbEmployeeType', 'cmbReportedBranch', 'cmbReportedPerson',
                'cmbStore', 'cmbEmployeeGroup'].forEach(clearCombo);
            HRM.setVal('txtSalary', '');
            HRM.check('IsActiveEmployeeInformation', false);
            firstRow('CmbSalaryPayableAc'); firstRow('CmbSalariesExpensesAc'); firstRow('CmbSalaryLoanAc');
        }
        /** EmployeeInformationValidation(). */
        function EmployeeInformationValidation() {
            function fail(m, id) { HRM.box(m); focusIn('tabPage5', id); return false; }
            if (HRM.comboVal('cmbDesignation') === 0) return fail('Designation Required', 'cmbDesignation');
            if (HRM.comboVal('cmbShift') === 0) return fail('Shift Required', 'cmbShift');
            if (HRM.comboVal('cmbDepartment') === 0) return fail('Department Required', 'cmbDepartment');
            if (HRM.comboVal('cmbLocation') === 0) return fail('Location Required', 'cmbLocation');
            if (HRM.comboVal('cmbSection') === 0) return fail('Section Required', 'cmbSection');
            if (HRM.comboVal('cmbEmployeeType') === 0) return fail('Employee Type Required', 'cmbEmployeeType');
            if (HRM.num(HRM.val('txtSalary')) === 0) return fail('Salary Required', 'txtSalary');
            return true;
        }
        /** cmbDepartment_Leave: GetCaoByDepartmentId -> Payable / Expense / Loan accounts, else their first rows. */
        function cmbDepartment_Leave() {
            var dep = HRM.comboVal('cmbDepartment');
            function first() { firstRow('CmbSalaryPayableAc'); firstRow('CmbSalariesExpensesAc'); firstRow('CmbSalaryLoanAc'); }
            if (dep === 0) { first(); return Promise.resolve(); }
            return HRM.loading(HRM.get(API + '/department-accounts', { departmentId: dep })).then(function (d) {
                if (d && d.found) {
                    HRM.setCombo('CmbSalaryPayableAc', d.PayableAcId);
                    HRM.setCombo('CmbSalariesExpensesAc', d.ExpenseAccountId);
                    HRM.setCombo('CmbSalaryLoanAc', d.LoanAcId);
                } else first();
            }).catch(HRM.fail);
        }
        /** cmbLocation_ValueChanged: a row is active -> GridBindEmployeeSalary(Location). */
        function cmbLocation_ValueChanged() {
            var v = HRM.val('cmbLocation');
            if (v !== '' && v !== '0') GridBindEmployeeSalary(HRM.comboVal('cmbLocation'));
        }

        // ======================================================================== 5 Employee Salary tab + Benefits tab
        /** .NET "0,0": grouped whole number, at least two digits. */
        function fmt00(v) {
            var n = HRM.num(v), neg = n < 0, a = Math.round(Math.abs(n));
            var s = String(a).padStart(2, '0').replace(/\B(?=(\d{3})+(?!\d))/g, ',');
            return (neg && a !== 0 ? '-' : '') + s;
        }
        /** .NET "#,##0.##" (TotalFormatString). */
        function fmtTotal(v) {
            var n = HRM.round(v, 2), p = Math.abs(n).toFixed(2).split('.');
            var s = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',') + (p[1] === '00' ? '' : '.' + p[1].replace(/0$/, ''));
            return (n < 0 ? '-' : '') + s;
        }
        var grdSalary = new HRM.Grid('grdEmployeeSalary', {                               // grdEmployeeSalarySetting
            columns: [
                { key: 'SalaryTypeId', caption: 'SalaryTypeId', hidden: true },
                { key: 'SalaryType', caption: 'SalaryType' },
                { key: 'TypePercent', caption: 'TypePercent', type: 'num', sum: true, align: 'right', render: function (v) { return HRM.esc(fmt00(v)); } },
                { key: 'Amount', caption: 'Amount', type: 'num', sum: true, align: 'right', render: function (v) { return HRM.esc(fmt00(v)); } }
            ],
            totals: true, filterRow: true,
            onDraw: function (g) {
                var tf = g.table.tFoot; if (!tf || !tf.rows[0]) return;
                tf.rows[0].cells[2].textContent = fmtTotal(g.sum('TypePercent', true));
                tf.rows[0].cells[3].textContent = fmtTotal(g.sum('Amount', true));
            }
        });
        /** GridBindEmployeeSalary(LocationId): the location's hrmSalaryBreakupPolicy rows with Amount 0, or an empty grid. */
        function GridBindEmployeeSalary(locationId) {
            if (locationId <= 0) return Promise.resolve();
            return HRM.loading(HRM.get(API + '/salary-breakup', { locationId: locationId })).then(function (rows) {
                grdSalary.set(rows && rows.length ? rows : []);
            }).catch(HRM.fail);
        }
        /** GenerateSalaryBreakup (btnGenerateBreakup_Click): Amount = TypePercent / 100 * Total Salary on every row. */
        P.btnGenerateBreakup = function () {
            var salary = HRM.num(HRM.val('txtSalary'));
            if (salary > 0) {
                grdSalary.rows().forEach(function (r) { r.Amount = HRM.num(r.TypePercent) / 100.0 * salary; });
                grdSalary.draw();
            } else {
                HRM.box('Please Enter Salary First In Employee Information');
                focusIn('tabPage5', 'txtSalary');
            }
        };

        function approvalOptions() { return [['', '']].concat(persons.map(function (p) { return [HRM.col(p, 'Id'), HRM.col(p, 'Name')]; })); }
        function benefitColumns() {                                                       // grdEmployee*BenefitSetting
            return [
                { key: 'BenefitId', caption: 'BenefitId', hidden: true },
                { key: 'BenefitName', caption: 'BenefitName' },
                { key: 'BenefitValue', caption: 'BenefitValue' },
                { key: 'ApprovalPersonId', caption: 'Approval Person', type: 'select', width: 250, options: approvalOptions },
                { key: 'Select', caption: 'Select', type: 'edit-check' }
            ];
        }
        var grdDirect = new HRM.Grid('grdEmployeeDirectBenefits', { columns: benefitColumns(), checkAll: 'Select', totals: true, filterRow: true });   // UseHeaderSelector
        var grdAssets = new HRM.Grid('grdEmployeeAssetsBenefit', { columns: benefitColumns(), totals: true, filterRow: true });
        /** ResetEmployeeBenefits + EmployeeBenefitsandWeekDaysCheckBoxNull: fresh benefit rows, none ticked. */
        function ResetEmployeeBenefits() {
            grdDirect.set(clone(fresh.directBenefits).map(function (r) { r.Select = false; return r; }));
            grdAssets.set(clone(fresh.assetBenefits).map(function (r) { r.Select = false; return r; }));
        }
        /** btnGenerateAllBenefits_Click: All -> every direct row ticked with the current row's Approval Person; else the ticked rows get it. */
        P.btnGenerateAllBenefits = function () {
            var rows = grdDirect.rows();
            var cur = grdDirect.current() || rows[0];                                     // GridEX: the first row is current after binding
            if (HRM.checked('ChkAllBenefits')) {
                if (rows.length > 0) rows.forEach(function (r) { r.Select = true; r.ApprovalPersonId = HRM.int(cur.ApprovalPersonId) || ''; });
            } else {
                if (!cur) return;
                rows.forEach(function (r) { if (HRM.bool(r.Select)) r.ApprovalPersonId = HRM.int(cur.ApprovalPersonId) || ''; });
            }
            grdDirect.draw();
        };

        // ======================================================================== 6 Week Days tab
        var grdWeekDays = new HRM.Grid('grdWeekDays', {                                   // grdWeekDaysSetting
            columns: [
                { key: 'WeekDaysProfileId', caption: 'WeekDaysProfileId', hidden: true },
                { key: 'WeekDays', caption: 'WeekDays' },
                { key: 'Select', caption: 'Select', type: 'edit-check' }
            ],
            totals: true, filterRow: true
        });
        /** ResetWeekDays -> grdWeekDaysFill: every week day (profile type 60) ticked. */
        function ResetWeekDays() { grdWeekDays.set(clone(fresh.weekDays)); }

        // ======================================================================== detail tab helper (Add / Update / Cancel / DoubleClick)
        /**
         * The six Employee Detail tabs share one pattern: *Validation, btnAdd* (Rows.Add then Reset*), btnUpdate*
         * (Rows[updateDetailIndex] = controls), btnCancel*, grd*_DoubleClick (controls = row, Add hidden, Update + Cancel shown).
         */
        function Detail(o) {
            var idx = -1;
            function mode(edit) { HRM.show(o.add, !edit); HRM.show(o.upd, edit); HRM.show(o.cancel, edit); }
            function reset() { o.clear(); mode(false); idx = -1; }
            var api = {
                reset: reset,
                add: function () { if (!o.validate()) return; o.grid.add(o.read()); reset(); HRM.focus(o.focus); },
                update: function () {
                    if (!o.validate()) return;
                    if (idx >= 0 && idx < o.grid.rows().length) o.grid.update(idx, o.read());
                    reset(); HRM.focus(o.focus);
                },
                cancel: function () { reset(); HRM.focus(o.cancelFocus || o.focus); },
                edit: function (r, i) { idx = i; o.write(r); mode(true); if (o.editFocus !== false) HRM.focus(o.focus); }
            };
            o.grid.opts.onDouble = api.edit;
            return api;
        }
        function need(id, m) { if (HRM.val(id) === '' || HRM.val(id) === '0') { HRM.box(m); HRM.focus(id); return false; } return true; }  // ActiveRow == null
        function needText(id, m) { if (HRM.val(id) === '') { HRM.box(m); HRM.focus(id); return false; } return true; }     // Text == string.Empty

        // ======================================================================== 7 Address Detail tab
        var grdAddress = new HRM.Grid('grdAddressDetail', {                               // grdAddressDetailSetting
            columns: [
                { key: 'AddressTypeId', hidden: true }, { key: 'AddressType' }, { key: 'CountryId', hidden: true }, { key: 'Country' },
                { key: 'ProvinceId', hidden: true }, { key: 'Province' }, { key: 'CityId', hidden: true }, { key: 'City' }, { key: 'AddressDetail' }
            ], totals: true, filterRow: true
        });
        P.address = Detail({
            grid: grdAddress, add: 'btnAddAddressDetail', upd: 'btnUpdateAddressDetail', cancel: 'btnCancelAddressDetail', focus: 'cmbAddressType',
            validate: function () {                                                        // AddressDetailValidation
                return need('cmbAddressType', 'Address Required') && need('cmbCountry', 'Country Required') && need('cmbProvince', 'Province Required') &&
                    need('cmbCity', 'City Required') && needText('txtAddressDetail', 'Address Detail Required');
            },
            read: function () {
                return { AddressTypeId: HRM.comboVal('cmbAddressType'), AddressType: HRM.comboText('cmbAddressType'), CountryId: HRM.comboVal('cmbCountry'),
                    Country: HRM.comboText('cmbCountry'), ProvinceId: HRM.comboVal('cmbProvince'), Province: HRM.comboText('cmbProvince'),
                    CityId: HRM.comboVal('cmbCity'), City: HRM.comboText('cmbCity'), AddressDetail: HRM.val('txtAddressDetail') };
            },
            write: function (r) {                                                          // grdAddressDetail_DoubleClick
                HRM.setCombo('cmbAddressType', r.AddressTypeId); HRM.setCombo('cmbCountry', r.CountryId); HRM.setCombo('cmbProvince', r.ProvinceId);
                HRM.setCombo('cmbCity', r.CityId); HRM.setVal('txtAddressDetail', r.AddressDetail);
            },
            clear: function () {                                                           // ResetAddressDetail
                ['cmbAddressType', 'cmbCountry', 'cmbProvince', 'cmbCity'].forEach(clearCombo); HRM.setVal('txtAddressDetail', '');
            }
        });

        // ======================================================================== 8 Experience tab
        var grdExperience = new HRM.Grid('grdEmployeeExperience', {                       // grdEmployeeExperienceSetting
            columns: [
                { key: 'DesignationId', hidden: true }, { key: 'Designation' }, { key: 'Organization' }, { key: 'CityId', hidden: true }, { key: 'City' },
                { key: 'FromDate', type: 'date' }, { key: 'ToDate', type: 'date' }, { key: 'Description' }
            ], totals: true, filterRow: true
        });
        P.experience = Detail({
            grid: grdExperience, add: 'btnAddExperience', upd: 'btnUpdateExperience', cancel: 'btnCancelExperience', focus: 'cmbDesignationEmployeeExperience',
            validate: function () {                                                        // EmployeeExperienceValidation (its City message says "Designation Required")
                return need('cmbDesignationEmployeeExperience', 'Designation Required') && needText('txtOrganization', 'Organization Required') &&
                    need('cmbCityEmployeeExperience', 'Designation Required');
            },
            read: function () {
                return { DesignationId: HRM.comboVal('cmbDesignationEmployeeExperience'), Designation: HRM.comboText('cmbDesignationEmployeeExperience'),
                    Organization: HRM.val('txtOrganization'), CityId: HRM.comboVal('cmbCityEmployeeExperience'), City: HRM.comboText('cmbCityEmployeeExperience'),
                    FromDate: HRM.val('dtpFrom') || HRM.today(), ToDate: HRM.val('dtpTo') || HRM.today(), Description: HRM.val('txtDescription') };
            },
            write: function (r) {                                                          // grdEmployeeExperience_DoubleClick
                HRM.setCombo('cmbDesignationEmployeeExperience', r.DesignationId); HRM.setVal('txtOrganization', r.Organization);
                HRM.setCombo('cmbCityEmployeeExperience', r.CityId); HRM.setVal('dtpFrom', HRM.day(r.FromDate)); HRM.setVal('dtpTo', HRM.day(r.ToDate));
                HRM.setVal('txtDescription', r.Description);
            },
            clear: function () {                                                           // ResetEmployeeExperience
                clearCombo('cmbDesignationEmployeeExperience'); HRM.setVal('txtOrganization', ''); clearCombo('cmbCityEmployeeExperience');
                HRM.setVal('dtpFrom', HRM.today()); HRM.setVal('dtpTo', HRM.today()); HRM.setVal('txtDescription', '');
            }
        });

        // ======================================================================== 9 Education tab
        var grdEducation = new HRM.Grid('grdEmployeeEducation', {                         // grdEmployeeEducationSetting
            columns: [
                { key: 'DegreeId', hidden: true }, { key: 'Degree' }, { key: 'DegreeTitle' }, { key: 'Institute' }, { key: 'CityId', hidden: true },
                { key: 'City' }, { key: 'StartYear', type: 'date' }, { key: 'PassingYear', type: 'date' }, { key: 'CGPA' }
            ], totals: true, filterRow: true
        });
        P.education = Detail({
            grid: grdEducation, add: 'btnAddEducation', upd: 'btnUpdateEducation', cancel: 'btnCancelEducation', focus: 'cmbDegree', editFocus: false,
            validate: function () {                                                        // EmployeeEducationValidation
                return need('cmbDegree', 'Degree Required') && needText('txtDegreeTitle', 'Degree Title Required') && needText('txtInstitute', 'Institute Required') &&
                    need('cmbCityEducation', 'City Required') && needText('txtCGPA', 'CGPA Required');
            },
            read: function () {
                return { DegreeId: HRM.comboVal('cmbDegree'), Degree: HRM.comboText('cmbDegree'), DegreeTitle: HRM.val('txtDegreeTitle'), Institute: HRM.val('txtInstitute'),
                    CityId: HRM.comboVal('cmbCityEducation'), City: HRM.comboText('cmbCityEducation'), StartYear: HRM.val('StartYearDate') || HRM.today(),
                    PassingYear: HRM.val('PassingYearDate') || HRM.today(), CGPA: HRM.val('txtCGPA') };
            },
            write: function (r) {                                                          // grdEmployeeEducation_DoubleClick
                HRM.setCombo('cmbDegree', r.DegreeId); HRM.setVal('txtDegreeTitle', r.DegreeTitle); HRM.setVal('txtInstitute', r.Institute);
                HRM.setCombo('cmbCityEducation', r.CityId); HRM.setVal('StartYearDate', HRM.day(r.StartYear)); HRM.setVal('PassingYearDate', HRM.day(r.PassingYear));
                HRM.setVal('txtCGPA', HRM.str(r.CGPA));
            },
            clear: function () {                                                           // ResetEmployeeEducation
                clearCombo('cmbDegree'); HRM.setVal('txtDegreeTitle', ''); HRM.setVal('txtInstitute', ''); clearCombo('cmbCityEducation');
                HRM.setVal('StartYearDate', HRM.today()); HRM.setVal('PassingYearDate', HRM.today()); HRM.setVal('txtCGPA', '');
            }
        });

        // ======================================================================== 10 Family + Reference tabs
        var grdFamily = new HRM.Grid('grdFamilyInfo', {                                   // grdEmployeeFamilyInfoSetting
            columns: [{ key: 'RelationShipId', hidden: true }, { key: 'RelationShip' }, { key: 'RelationName' }, { key: 'Description' }],
            totals: true, filterRow: true
        });
        P.family = Detail({
            grid: grdFamily, add: 'btnAddFamily', upd: 'btnUpdateFamily', cancel: 'btnCancelFamily', focus: 'cmbRelationShip', editFocus: false,
            validate: function () {                                                        // EmployeeFamilyInfoValidation
                return need('cmbRelationShip', 'Relationship Required') && needText('txtNameFamilyInfo', 'Relationship Name Required');
            },
            read: function () {
                return { RelationShipId: HRM.comboVal('cmbRelationShip'), RelationShip: HRM.comboText('cmbRelationShip'), RelationName: HRM.val('txtNameFamilyInfo'),
                    Description: HRM.val('txtDescriptionFamilyInfo') };
            },
            write: function (r) {                                                          // grdFamilyInfo_DoubleClick
                HRM.setCombo('cmbRelationShip', r.RelationShipId); HRM.setVal('txtNameFamilyInfo', r.RelationName); HRM.setVal('txtDescriptionFamilyInfo', r.Description);
            },
            clear: function () { clearCombo('cmbRelationShip'); HRM.setVal('txtNameFamilyInfo', ''); HRM.setVal('txtDescriptionFamilyInfo', ''); }
        });

        var grdReference = new HRM.Grid('grdReference', {                                 // grdEmployeeReferenceSetting
            columns: [{ key: 'ReferenceName' }, { key: 'OrganizationName' }, { key: 'DesignationId', hidden: true }, { key: 'Designation' }, { key: 'Mobile' },
                { key: 'Email' }, { key: 'Description' }],
            totals: true, filterRow: true
        });
        P.reference = Detail({
            grid: grdReference, add: 'btnAddReference', upd: 'btnUpdateReference', cancel: 'btnCancelReference', focus: 'txtRefrenceName', editFocus: false,
            validate: function () {                                                        // EmployeeReferenceValidation
                return need('cmbDesignationEmployeeRefrence', 'Designation Required') && needText('txtRefrenceName', 'Reference Name Required') &&
                    needText('txtOrganizationEmployeeRefrence', 'Organization Required') && needText('txtMobileEmployeeRefrence', 'Mobile Required');
            },
            read: function () {
                return { ReferenceName: HRM.val('txtRefrenceName'), OrganizationName: HRM.val('txtOrganizationEmployeeRefrence'),
                    DesignationId: HRM.comboVal('cmbDesignationEmployeeRefrence'), Designation: HRM.comboText('cmbDesignationEmployeeRefrence'),
                    Mobile: HRM.val('txtMobileEmployeeRefrence'), Email: HRM.val('txtEmailEmployeeRefrence'), Description: HRM.val('txtDescriptionEmployeeRefrence') };
            },
            write: function (r) {                                                          // grdReference_DoubleClick
                HRM.setVal('txtRefrenceName', r.ReferenceName); HRM.setVal('txtOrganizationEmployeeRefrence', r.OrganizationName);
                HRM.setCombo('cmbDesignationEmployeeRefrence', r.DesignationId); HRM.setVal('txtMobileEmployeeRefrence', r.Mobile);
                HRM.setVal('txtEmailEmployeeRefrence', r.Email); HRM.setVal('txtDescriptionEmployeeRefrence', r.Description);
            },
            clear: function () {                                                           // ResetReference
                ['txtRefrenceName', 'txtOrganizationEmployeeRefrence', 'txtMobileEmployeeRefrence', 'txtEmailEmployeeRefrence', 'txtDescriptionEmployeeRefrence']
                    .forEach(function (id) { HRM.setVal(id, ''); });
                clearCombo('cmbDesignationEmployeeRefrence');
            }
        });

        // ======================================================================== 11 Bank Account tab
        var grdBank = new HRM.Grid('grdBankAccount', {                                    // grdBankAccountSetting
            columns: [{ key: 'BankId', hidden: true }, { key: 'Bank' }, { key: 'AccountTitle' }, { key: 'AccountNo' }, { key: 'IsPayRoll', type: 'check' }],
            totals: true, filterRow: true
        });
        P.bank = Detail({
            grid: grdBank, add: 'btnAddBankAccount', upd: 'btnUpdateBankAccount', cancel: 'btnCancelBankAccount', focus: 'cmbBankAccount', editFocus: false,
            validate: function () {                                                        // EmployeeBankAccountValidation
                return need('cmbBankAccount', 'Bank Required') && needText('txtAccountTitle', 'Account Title Required') && needText('txtAccountNo', 'Account No Required');
            },
            read: function () {
                return { BankId: HRM.comboVal('cmbBankAccount'), Bank: HRM.comboText('cmbBankAccount'), AccountTitle: HRM.val('txtAccountTitle'),
                    AccountNo: HRM.val('txtAccountNo'), IsPayRoll: HRM.checked('IsForPayrollBankAccount') };
            },
            write: function (r) {                                                          // grdBankAccount_DoubleClick
                HRM.setCombo('cmbBankAccount', r.BankId); HRM.setVal('txtAccountTitle', r.AccountTitle); HRM.setVal('txtAccountNo', r.AccountNo);
                HRM.check('IsForPayrollBankAccount', HRM.bool(r.IsPayRoll));
            },
            clear: function () {                                                           // ResetBankAccount
                clearCombo('cmbBankAccount'); HRM.setVal('txtAccountTitle', ''); HRM.setVal('txtAccountNo', ''); HRM.check('IsForPayrollBankAccount', false);
            }
        });

        // ======================================================================== btnnew_Click
        /** btnnew_Click: Reset, ResetEmployeeInformation, ResetEmployeeSalary, ResetEmployeeBenefits, ResetWeekDays, the six detail resets. */
        function New(btn) {
            return HRM.busy(btn || 'btnnew', function () {
                return HRM.get(API + '/reset').then(function (d) {
                    fresh = { directBenefits: d.directBenefits || [], assetBenefits: d.assetBenefits || [], weekDays: d.weekDays || [] };
                    Reset(d.employeeNo);
                    ResetEmployeeInformation();
                    // ResetEmployeeSalary: GridBindEmployeeSalary(cmbLocation) - the location was just cleared, so nothing
                    ResetEmployeeBenefits();
                    ResetWeekDays();
                    P.address.reset(); P.experience.reset(); P.education.reset(); P.family.reset(); P.reference.reset(); P.bank.reset();
                }).catch(HRM.fail);
            }, 'employee-new');
        }
        P.btnnew = function (btn) { return New(btn); };

        // ======================================================================== 12 RetrivedDataEmployeeRegistration
        /**
         * RetrivedDataEmployeeRegistration(Id): GetByID, header, history[0] (cmbDepartment_Leave then the saved accounts,
         * cmbLocation -> GridBindEmployeeSalary), then each child list the form copies into its grid. The web starts from the
         * fresh (New) grids so one employee's rows are never carried into another (see report).
         */
        function RetrivedDataEmployeeRegistration(id) {
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (d) {
                var e = d.employee || {};
                RecId = HRM.int(id);
                grdBank.clear(); grdAddress.clear(); grdExperience.clear(); grdFamily.clear(); grdReference.clear(); grdEducation.clear(); grdSalary.clear();
                ResetEmployeeBenefits(); ResetWeekDays();
                HRM.setVal('txtEmployeeNo', HRM.str(e.EmployeeNo));
                HRM.setCombo('cmbEmployeeType', e.EmployeeTypeProfileId);
                HRM.setCombo('cmbEmployeeCatagory', e.EmployeeCategoryId);
                HRM.setCombo('cmbTitle', e.TitleId);
                HRM.setVal('txtFirstName', HRM.str(e.FistName));
                HRM.setVal('txtMiddleName', HRM.str(e.MiddleName));
                HRM.setVal('txtLastName', HRM.str(e.LastName));
                HRM.setCombo('cmbRelationTitle', e.SubTitleId);
                HRM.setVal('txtRelationName', HRM.str(e.RelationName));
                HRM.setVal('txtRelationMobileNo', HRM.str(e.RelationMobile));
                HRM.setCombo('cmbGender', e.GenderProfileId);
                HRM.setVal('dtpDOB', HRM.day(e.DOB) || '1900-01-01');
                HRM.setCombo('cmbNationality', e.NationalityProfileId);
                HRM.setVal('txtEmail', HRM.str(e.Email));
                HRM.setVal('txtMobile1', HRM.str(e.Mobile1));
                HRM.setVal('txtMobile2', HRM.str(e.Mobile2));
                HRM.setVal('txtCNIC', HRM.str(e.CNIC));
                HRM.setVal('dtpCNICIssueDate', HRM.day(e.CNICIssueDate) || '1900-01-01');
                HRM.setVal('dtpCNICExpiryDate', HRM.day(e.CNICExpiryDate) || '1900-01-01');
                HRM.setVal('txtPassportNo', HRM.str(e.PassportNo));
                HRM.setVal('txtLicenseNo', HRM.str(e.LicenseNo));
                HRM.setCombo('cmbBloodGroup', e.BloodGroupProfileId);
                if (HRM.hasOption('combranches', e.BranchesId)) HRM.setCombo('combranches', e.BranchesId);
                if (HRM.hasOption('comproject', e.ProjectId)) HRM.setCombo('comproject', e.ProjectId);
                picture(e.PictureFilePath, 'pictureBoxEmployeePhoto');
                picture(e.SignPictureFilePath, 'pictureBoxEmployeeSignature');

                var h = d.history;
                if (h) {
                    HRM.setVal('txtStartDate', HRM.day(h.FromDate));
                    HRM.setVal('txtEndDate', HRM.day(h.ToDate));
                    HRM.setVal('txtProbationEndingDate', HRM.day(h.ProbationEnding));
                    HRM.setCombo('cmbDesignation', h.DesignationId);
                    HRM.setCombo('cmbShift', h.ShiftId);
                    HRM.setCombo('cmbDepartment', h.DepartmentId);            // cmbDepartment_Leave runs next, then the saved accounts win
                    HRM.setCombo('CmbSalaryPayableAc', h.PayableAcId);
                    HRM.setCombo('CmbSalariesExpensesAc', h.ExpenseAccountId);
                    HRM.setCombo('CmbSalaryLoanAc', h.LoanAcId);
                    HRM.setCombo('cmbLocation', h.LocationId);                // cmbLocation_ValueChanged -> GridBindEmployeeSalary
                    if (HRM.int(h.LocationId) > 0 && HRM.hasOption('cmbLocation', h.LocationId)) grdSalary.set(d.locationBreakup || []);
                    HRM.setCombo('cmbSection', h.SectionId);
                    HRM.setCombo('cmbEmployeeType', h.EmployeeTypeProfileId);
                    HRM.setCombo('cmbReportedBranch', h.PartyLocationId);
                    HRM.setCombo('cmbReportedPerson', h.ReportToEmployeeId);
                    HRM.setCombo('cmbStore', h.StoreId);
                    HRM.setVal('txtSalary', HRM.str(h.TotalSalary));
                    HRM.setCombo('cmbEmployeeGroup', h.EmployeeGroupId);
                    HRM.check('IsActiveEmployeeInformation', HRM.bool(h.Active));
                }
                if ((d.salaries || []).length > 0) grdSalary.set(d.salaries);

                // HrmEmployeeBenefitslist: a saved benefit's row is deleted and re-added at the end, ticked, with its value and approver
                (d.benefits || []).forEach(function (b) {
                    [['Payroll Benefits', grdDirect], ['Assets Benefits', grdAssets]].forEach(function (pair) {
                        if (HRM.str(b.ProfileName) !== pair[0]) return;
                        var g = pair[1];
                        g.rows().slice().forEach(function (r) {
                            if (HRM.int(b.BenefitId) !== HRM.int(r.BenefitId)) return;
                            g.data.splice(g.data.indexOf(r), 1);
                            g.data.push({ BenefitId: b.BenefitId, BenefitName: b.ProfileName, BenefitValue: b.Description, ApprovalPersonId: b.ApprovalEmployeeId, Select: true });
                        });
                    });
                });
                grdDirect.draw(); grdAssets.draw();

                // EmployeeWeekDayslist: every day unticked, then each saved day re-added at the end, ticked
                if ((d.weekDays || []).length > 0) {
                    grdWeekDays.rows().forEach(function (r) { r.Select = false; });
                    d.weekDays.forEach(function (w) {
                        grdWeekDays.rows().slice().forEach(function (r) {
                            if (HRM.int(w.WeekDayProfileId) !== HRM.int(r.WeekDaysProfileId)) return;
                            grdWeekDays.data.splice(grdWeekDays.data.indexOf(r), 1);
                            grdWeekDays.data.push({ WeekDaysProfileId: w.WeekDayProfileId, WeekDays: w.WeekDayProfileName, Select: true });
                        });
                    });
                }
                grdWeekDays.draw();

                if ((d.addresses || []).length) grdAddress.set(d.addresses);
                if ((d.experiences || []).length) grdExperience.set(d.experiences);
                if ((d.educations || []).length) grdEducation.set(d.educations);
                if ((d.families || []).length) grdFamily.set(d.families);
                if ((d.references || []).length) grdReference.set(d.references);
                if ((d.banks || []).length) grdBank.set(d.banks);

                buttons(true);
                UpdateMode = true;
            }).catch(HRM.fail);
        }
        P.load = RetrivedDataEmployeeRegistration;

        // ======================================================================== 13 Save / Update (btnsave_Click)
        function benefitsCheck(g) {
            var rows = g.rows();
            for (var i = 0; i < rows.length; i++) {
                var r = rows[i];
                if (!HRM.bool(r.Select)) continue;
                if (HRM.int(r.BenefitId) === 0) { HRM.box('PLease Check Benefit'); return false; }
                if (HRM.str(r.BenefitValue).trim() === '') { HRM.box('Please Check Benefit Value'); return false; }
                if (HRM.int(r.ApprovalPersonId) === 0) { HRM.box('Please Check Approval Person'); return false; }
            }
            return true;
        }
        function benefitRows(g) {
            return g.rows().map(function (r) {
                return { benefitId: HRM.int(r.BenefitId), benefitValue: HRM.str(r.BenefitValue), approvalPersonId: HRM.int(r.ApprovalPersonId), select: HRM.bool(r.Select) };
            });
        }
        function payload() {
            return {
                id: RecId,
                employeeNo: HRM.val('txtEmployeeNo'), employeeTypeId: HRM.comboVal('cmbEmployeeType'), employeeCategoryId: HRM.comboVal('cmbEmployeeCatagory'),
                titleId: HRM.comboVal('cmbTitle'), firstName: HRM.val('txtFirstName'), middleName: HRM.val('txtMiddleName'), lastName: HRM.val('txtLastName'),
                relationTitleId: HRM.comboVal('cmbRelationTitle'), relationName: HRM.val('txtRelationName'), relationMobile: HRM.val('txtRelationMobileNo'),
                genderId: HRM.comboVal('cmbGender'), dob: HRM.val('dtpDOB'), nationalityId: HRM.comboVal('cmbNationality'), email: HRM.val('txtEmail'),
                mobile1: HRM.val('txtMobile1'), mobile2: HRM.val('txtMobile2'), cnic: HRM.val('txtCNIC'), cnicIssueDate: HRM.val('dtpCNICIssueDate'),
                cnicExpiryDate: HRM.val('dtpCNICExpiryDate'), passportNo: HRM.val('txtPassportNo'), licenseNo: HRM.val('txtLicenseNo'),
                bloodGroupId: HRM.comboVal('cmbBloodGroup'), branchesId: HRM.comboVal('combranches'), projectId: HRM.comboVal('comproject'),
                startDate: HRM.val('txtStartDate'), endDate: HRM.val('txtEndDate'), probationEndDate: HRM.val('txtProbationEndingDate'),
                designationId: HRM.comboVal('cmbDesignation'), shiftId: HRM.comboVal('cmbShift'), sectionId: HRM.comboVal('cmbSection'),
                departmentId: HRM.comboVal('cmbDepartment'), payableAcId: HRM.comboVal('CmbSalaryPayableAc'), expenseAccountId: HRM.comboVal('CmbSalariesExpensesAc'),
                loanAcId: HRM.comboVal('CmbSalaryLoanAc'), employeeGroupId: HRM.comboVal('cmbEmployeeGroup'), locationId: HRM.comboVal('cmbLocation'),
                reportedBranchId: HRM.comboVal('cmbReportedBranch'), reportedPersonId: HRM.comboVal('cmbReportedPerson'), storeId: HRM.comboVal('cmbStore'),
                totalSalary: HRM.val('txtSalary'), active: HRM.checked('IsActiveEmployeeInformation'),
                salaries: grdSalary.rows().map(function (r) { return { salaryTypeId: HRM.int(r.SalaryTypeId), typePercent: HRM.str(r.TypePercent), amount: HRM.str(r.Amount) }; }),
                directBenefits: benefitRows(grdDirect),
                assetBenefits: benefitRows(grdAssets),
                weekDays: grdWeekDays.rows().map(function (r, i) { return { weekDaysProfileId: HRM.int(r.WeekDaysProfileId), weekDays: HRM.str(r.WeekDays), select: HRM.bool(r.Select), rowIndex: i }; }),
                addresses: grdAddress.rows().map(function (r) {
                    return { addressTypeId: HRM.int(r.AddressTypeId), countryId: HRM.int(r.CountryId), provinceId: HRM.int(r.ProvinceId), cityId: HRM.int(r.CityId), addressDetail: HRM.str(r.AddressDetail) };
                }),
                experiences: grdExperience.rows().map(function (r, i) {
                    return { designationId: HRM.int(r.DesignationId), organization: HRM.str(r.Organization), cityId: HRM.int(r.CityId), fromDate: HRM.day(r.FromDate),
                        toDate: HRM.day(r.ToDate), description: HRM.str(r.Description), rowIndex: i };
                }),
                educations: grdEducation.rows().map(function (r) {
                    return { degreeId: HRM.int(r.DegreeId), degreeTitle: HRM.str(r.DegreeTitle), institute: HRM.str(r.Institute), cityId: HRM.int(r.CityId),
                        startYear: HRM.day(r.StartYear), passingYear: HRM.day(r.PassingYear), cgpa: HRM.str(r.CGPA) };
                }),
                families: grdFamily.rows().map(function (r, i) {
                    return { relationShipId: HRM.int(r.RelationShipId), relationName: HRM.str(r.RelationName), description: HRM.str(r.Description), rowIndex: i };
                }),
                references: grdReference.rows().map(function (r) {
                    return { referenceName: HRM.str(r.ReferenceName), organizationName: HRM.str(r.OrganizationName), designationId: HRM.int(r.DesignationId),
                        mobile: HRM.str(r.Mobile), email: HRM.str(r.Email), description: HRM.str(r.Description) };
                }),
                banks: grdBank.rows().map(function (r) {
                    return { bankId: HRM.int(r.BankId), accountTitle: HRM.str(r.AccountTitle), accountNo: HRM.str(r.AccountNo), isPayRoll: HRM.bool(r.IsPayRoll) };
                })
            };
        }
        /** btnsave_Click (btnupdate_Click calls it). */
        function Save(btn) {
            if (!ValidationEmployeeRegistration()) return;
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var issue = HRM.val('dtpCNICIssueDate') || HRM.today(), expiry = HRM.val('dtpCNICExpiryDate') || HRM.today();
            var plus10 = new Date(issue + 'T00:00:00'); plus10.setFullYear(plus10.getFullYear() + 10);
            if (plus10 < new Date(expiry + 'T00:00:00')) { HRM.box('CNIC Expired!'); HRM.focus('dtpCNICExpiryDate'); return; }
            if (!EmployeeInformationValidation()) return;
            if (grdSalary.rows().length === 0) { HRM.box('Employee Salary Breakup Required'); return; }
            if (!benefitsCheck(grdDirect) || !benefitsCheck(grdAssets)) return;
            var body = payload();
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d && d.message ? d.message : (body.id === 0 ? 'Information Saved' : 'Information Update'));
                    return New();                                                              // btnnew_Click(null, null)
                }).catch(HRM.fail);
            }, 'employee-save');
        }
        P.btnsave = function (btn) { return Save(btn || 'btnsave'); };                        // btnsave_Click
        P.btnupdate = function (btn) { return Save(btn || 'btnupdate'); };                    // btnupdate_Click -> btnsave_Click

        // ======================================================================== 14 Print, History, toolbar, define buttons
        /** EmployeeRegistrationSlipAndRegister rows first ("Not Record Found For Display"), then the viewer. */
        function printSlip(btn, employeeId, key, rpt) {
            var CP = window.CrystalPrint;
            var win = CP && CP.reserve ? CP.reserve() : null;
            return HRM.busy(btn, function () {
                return HRM.get(API + '/print-check', { employeeId: employeeId || 0 }).then(function (d) {
                    if (!d || HRM.int(d.rows) === 0) { if (CP) CP.release(win); HRM.box('Not Record Found For Display'); return; }
                    if (!key) { if (CP) CP.release(win); HRM.box('Report file ' + rpt + ' is not available.'); return; }
                    if (!CP) { HRM.box('Report file ' + rpt + ' is not available.'); return; }
                    return CP.open(key, { employeeId: employeeId > 0 ? employeeId : '' }, null, win);
                }).catch(function (e) { if (CP) CP.release(win); HRM.fail(e); });
            }, 'employee-print-' + (employeeId || 0) + '-' + (key || rpt));
        }
        /** btnPrint_Click: 1112 slip of RecId (0 -> the BLL omits @EmployeeId -> every employee). */
        P.btnPrint = function (btn) { return printSlip(btn || 'btnPrint', RecId, 'hrm-1112', '1112-Employee_Registration_Slip.rpt'); };

        /** tabPage2 History (GridBindEmployeeRegistration / grdEmployeeRegistrationSetting): Edit + Print columns, EmployeeNo opens the employee. */
        function History() {
            HRM.history({
                title: 'History',
                filters: '<button type="button" id="btnPrintRegister" class="win-btn-action" style="width:auto;padding:0 10px"><i class="fa fa-print"></i> 1111-Print Register</button>',
                columns: [
                    { key: 'Edit', caption: 'Edit', width: 50, render: function () { return '<a href="#" class="hrm-code" data-c="0">Edit</a>'; } },
                    { key: 'Print', caption: 'Print', width: 50, render: function () { return '<a href="#" class="emp-print">Print</a>'; } },
                    { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                    { key: 'EmployeeNo', caption: 'EmployeeNo', type: 'code' },
                    { key: 'EmployeeType', caption: 'EmployeeType', width: 100 },
                    { key: 'EmployeeCatagory', caption: 'EmployeeCatagory', width: 150 },
                    { key: 'Title', caption: 'Title', width: 80 },
                    { key: 'FirstName', caption: 'FirstName', width: 150 },
                    { key: 'MiddleName', caption: 'MiddleName', width: 150 },
                    { key: 'LastName', caption: 'LastName', width: 150 },
                    { key: 'RelationTitle', caption: 'RelationTitle', width: 80 },
                    { key: 'RelationName', caption: 'RelationName', width: 150 },
                    { key: 'RelationMobileNo', caption: 'RelationMobileNo', width: 150 },
                    { key: 'Gender', caption: 'Gender', width: 100 },
                    { key: 'DOB', caption: 'DOB', type: 'date', width: 100 },
                    { key: 'Nationality', caption: 'Nationality', width: 150 },
                    { key: 'Email', caption: 'Email', width: 150 },
                    { key: 'Mobile1', caption: 'Mobile1', width: 150 },
                    { key: 'Mobile2', caption: 'Mobile2', width: 150 },
                    { key: 'CNIC', caption: 'CNIC', width: 150 },
                    { key: 'CNIC Issue Date', caption: 'CNIC Issue Date', type: 'date', width: 150 },
                    { key: 'CNIC Expiry Date', caption: 'CNIC Expiry Date', type: 'date', width: 150 },
                    { key: 'PassportNo', caption: 'PassportNo', width: 100 },
                    { key: 'LicenseNo', caption: 'LicenseNo', width: 100 },
                    { key: 'BloodGroup', caption: 'BloodGroup', width: 50 },
                    { key: 'FlatSharePercent', caption: 'FlatSharePercent', width: 50 },
                    { key: 'IsAllowShare', caption: 'IsAllowShare', type: 'check', width: 50 },
                    { key: 'NeedLogin', caption: 'NeedLogin', width: 50 },
                    { key: 'SignPictureFilePath', caption: 'SignPictureFilePath', width: 200 },
                    { key: 'PictureFilePath', caption: 'PictureFilePath', width: 200 },
                    { key: 'Status', caption: 'Status' }
                ],
                load: function () { return HRM.get(API + '/history'); },
                onPick: function (r) { RetrivedDataEmployeeRegistration(HRM.int(r.EmployeeId)); },   // Edit button / grdEmployeeRegistration_DoubleClick
                onReady: function (body, g) {
                    body.addEventListener('click', function (e) {
                        var a = e.target.closest('a.emp-print'); if (!a) return;          // ColumnButtonClick "Print": 1112 slip of that row
                        e.preventDefault();
                        var tr = a.closest('tr[data-i]'); if (!tr) return;
                        var r = g.data[+tr.getAttribute('data-i')];
                        printSlip(a, HRM.int(r.EmployeeId), 'hrm-1112', '1112-Employee_Registration_Slip.rpt');
                    });
                    var pr = body.querySelector('#btnPrintRegister');                     // btnPrintRegister_Click (1111 .rpt is not on the server)
                    if (pr) pr.addEventListener('click', function () { printSlip(pr, 0, null, '1111-Employee_Registration_Register.rpt'); });
                }
            });
        }
        HRM.footer(function () { History(); });

        /** btnRefresh_Click: every combo again, then GridBindEmployeeSalary(cmbLocation). */
        P.btnRefresh = function (btn) {
            return HRM.busy(btn || 'btnRefresh', function () {
                return HRM.get(API + '/refresh').then(function (l) {
                    fillCombos(l, true);
                    return GridBindEmployeeSalary(HRM.comboVal('cmbLocation'));
                }).catch(HRM.fail);
            });
        };
        /** btnEmployeePicture_Click / btnEmployeeSignature_Click: OpenFileDialog + File.Copy to the "Attachment Folder Path" share. */
        P.btnEmployeePicture = function () {
            HRM.box('Employee Picture: the desktop picks the file with its OpenFileDialog and copies it to the "Attachment Folder Path" share, storing only the path. ' +
                'A browser cannot write to that share, so choosing a picture is not available here; the stored path is kept unchanged.');
        };
        P.btnEmployeeSignature = function () {
            HRM.box('Employee Signature: the desktop picks the file with its OpenFileDialog and copies it to the "Attachment Folder Path" share, storing only the path. ' +
                'A browser cannot write to that share, so choosing a signature is not available here; the stored path is kept unchanged.');
        };
        /** btnTitle / btnGender / ... _Click: new ProfileDefine { cmbprofiletype.Value = type }.Show(). */
        P.define = function (profileTypeId) { HRM.open('/hrm/define-profile?profileTypeId=' + profileTypeId); };
        /** btnEmployeeCategory / btnLocation / btnSection / btnShift / btnDesignation / btnDepartment / btnEmployeeGroup / btnCityName /
         *  btnSalaryBreakUp / btnDefineBenefits _Click: that define form .Show(). */
        P.open = function (url) { HRM.open(url); };
        /** btnCoa1 / btncoa2 / btncoa3 _Click: AcfrmDefCoa when the user has View on it. */
        P.coa = function () {
            if (links.coa) HRM.open('/accounts/chart_of_accounts');
            else HRM.box('You Dont Have rights View Of This Form..');
        };
        /** btnDutyRoasterForm_Click / BtnShifttiming_Click: the form when the user has View on it. */
        P.btnDutyRoasterForm = function () { if (links.dutyRoaster) HRM.open('/hrm/duty-roaster'); else HRM.box("You don't have the right to view this form..."); };
        P.BtnShifttiming = function () { if (links.shiftTiming) HRM.open('/hrm/shift-timing'); else HRM.box("You don't have the right to view this form..."); };

        // ======================================================================== 15 keys and form Load
        HRM.keys({                                                                   // frmEmployeeRegistration_KeyDown
            'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+p': function () { P.btnPrint(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled && UpdateMode) P.btnupdate(); }
        });
        wireTabs();
        mask('txtCNIC', [5, 7, 1], '-');
        mask('txtMobile1', [4, 7], ' ');
        HRM.$('cmbDepartment').addEventListener('change', cmbDepartment_Leave);
        HRM.$('cmbLocation').addEventListener('change', cmbLocation_ValueChanged);
        HRM.$('txtEmployeeNoForUpdate').addEventListener('blur', txtEmployeeNoForUpdate_Leave);

        HRM.loading(HRM.get(API + '/setup')).then(function (d) {                     // frmEmployeeRegistration_Load
            rights = d.rights || {};
            links = d.links || {};
            fillCombos(d.lookups || {}, false);
            fresh = { directBenefits: d.directBenefits || [], assetBenefits: d.assetBenefits || [], weekDays: d.weekDays || [] };
            HRM.setVal('txtEmployeeNo', HRM.str(d.employeeNo));
            ['dtpDOB', 'dtpCNICIssueDate', 'dtpCNICExpiryDate', 'txtStartDate', 'txtProbationEndingDate', 'dtpFrom', 'dtpTo', 'StartYearDate', 'PassingYearDate']
                .forEach(function (id) { HRM.setVal(id, HRM.today()); });
            HRM.setVal('txtEndDate', d.endDate ? HRM.day(d.endDate) : HRM.today());   // ActiveYr.End_Period
            ResetEmployeeBenefits();                                                  // fill + EmployeeBenefitsandWeekDaysCheckBoxNull
            ResetWeekDays();
            buttons(false);
            HRM.applyRights(rights, { save: ['btnsave'], update: ['btnupdate'] });
            var id = HRM.int(HRM.param('id'));
            if (id > 0) return RetrivedDataEmployeeRegistration(id);
            HRM.focus('txtEmployeeNo');
        }).catch(HRM.fail);
    }

    var PAGES = { 'employee-registration': employeeRegistration };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
