/* ============================================================================================
 * countx_hrm_policy.js - HRM "Policy Management" (AppModules 2019). One script, the page is chosen
 * by <body data-hrm="...">. Built on countx_hrm.js (window.HRM); handlers exported as window.HrmPolicy.
 *
 *   social-security          SocialSecurity.cs        (643)
 *   general-policy           GeneralPolicy.cs         (644)
 *   salary-breakup-policy    SalaryBreakupPolicy.cs   (645)
 *   leave-quota-policy       LeaveQuotaPolicy.cs      (646)
 *   eobi-policy              EOBIPolicy.cs            (647)
 *
 * Each handler follows its desktop form line by line: validation wording and order, messages,
 * what New / Save / Update / the grid buttons do to the buttons, the text boxes and the grids.
 * The desktop's Conversion.ToInt (Convert.ToInt32) is reproduced by cInt() because the values it
 * yields are what the form shows and saves.
 * ============================================================================================ */
(function () {
    'use strict';

    var PAGE = HRM.page();
    var API = '/api/hrm/policy/' + PAGE;
    var P = {};                       // handlers of the page, exported as window.HrmPolicy
    window.HrmPolicy = P;

    // ------------------------------------------------------------------------ desktop conversions
    /** Convert.ToInt32(double): nearest integer, halves to even. */
    function roundEven(n) {
        var f = Math.floor(n), d = n - f;
        if (d > 0.5) return f + 1;
        if (d < 0.5) return f;
        return f % 2 === 0 ? f : f + 1;
    }
    /** Conversion.ToInt: a string must be a whole number (int.Parse), else 0; a number is rounded half-to-even. */
    function cInt(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') { if (!isFinite(v)) return 0; var r = roundEven(v); return Math.abs(r) > 2147483647 ? 0 : r; }
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim();
        if (!/^[+-]?\d+$/.test(s)) return 0;
        var n = parseInt(s, 10);
        return Math.abs(n) > 2147483647 ? 0 : n;
    }
    /** int.Parse(s) - throws "Input string was not in a correct format." (the form's catch shows it). */
    function intParse(s) {
        var t = String(s === null || s === undefined ? '' : s).trim();
        if (!/^[+-]?\d+$/.test(t) || Math.abs(parseInt(t, 10)) > 2147483647) throw new Error('Input string was not in a correct format.');
        return parseInt(t, 10);
    }
    /** Conversion.ToDouble. */
    function cDbl(v) { return HRM.num(v); }
    /** double.ToString() for the grid / text boxes. */
    function dstr(v) { if (v === null || v === undefined || v === '') return ''; var n = Number(v); return isFinite(n) ? String(n) : String(v); }
    /** decimal.TryParse (NumberStyles.Number, en-US) - CommonServices.IsNumeric. */
    function isDecimal(s) { return /^\s*[+-]?(?=[\d,]*\.?\d)[\d,]*(\.\d*)?\s*$/.test(String(s === null || s === undefined ? '' : s)); }
    /** .NET custom format "0,0": grouped, whole number, at least two digits (5 -> "05"). */
    function fmt00(v) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v), neg = n < 0, a = Math.abs(n);
        var s = String(Math.floor(a + 0.5));
        if (s.length < 2) s = '0' + s;
        s = s.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (neg && s.replace(/[0,]/g, '') !== '' ? '-' : '') + s;
    }
    function dateGuard(id) {                                 // a DateTimePicker is never empty
        var e = HRM.$(id); if (!e) return;
        e.addEventListener('change', function () { if (!e.value) e.value = HRM.today(); });
    }
    function toolStripNoFocus() {                            // a ToolStrip button never takes the focus (no Leave)
        document.querySelectorAll('.win-tool-strip button').forEach(function (b) {
            b.addEventListener('mousedown', function (e) { e.preventDefault(); });
        });
    }
    function digitsOnly(id) {                                // txt*_KeyPress: control, digit or '.'
        var e = HRM.$(id); if (!e) return;
        e.addEventListener('keypress', function (ev) {
            if (ev.ctrlKey || ev.metaKey || !ev.key || ev.key.length !== 1) return;
            if (!/[0-9.]/.test(ev.key)) ev.preventDefault();
        });
    }
    function detailBox(title, id) {
        return '<div class="hrm-grid-box hrm-grid-short pol-hist-detail"><div class="hrm-subcaption"><span>' + HRM.esc(title) +
            '</span><button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button></div>' +
            '<div class="hrm-grid-wrap"><table id="' + id + '"></table></div></div>';
    }
    function detailLink(v) { return '<a href="#" class="pol-detail">Detail</a>'; }

    // ============================================================================ 643 Social Security
    function socialSecurity() {
        var RecId = 0, updateDetailIndex = -1, UpdateMode = false, rights = {};
        var table = [];                                      // DataTable table: FromSalary, ToSalary (string), Percent%, Amount (double)
        var grd = new HRM.Grid('grdfrm', {
            columns: [                                       // grdSettings()
                { key: 'FromSalary', caption: 'FromSalary', width: 120, align: 'right' },
                { key: 'ToSalary', caption: 'ToSalary', width: 120, align: 'right' },
                { key: 'Percent%', caption: 'Percent%', width: 150, align: 'right', sum: true, render: dstr },
                { key: 'Amount', caption: 'Amount', width: 150, align: 'right', sum: true, render: dstr },
                { key: 'Delete', caption: ' X', width: 20, align: 'center', render: function () { return '<button type="button" class="pol-del" title="Delete row">X</button>'; } }
            ],
            filterRow: true, totals: true,
            onDouble: function (r, i) { grdfrm_DoubleClick(r, i); }
        });
        HRM.$('grdfrm').addEventListener('click', function (e) {          // grdfrm_ColumnButtonClick (Delete)
            var b = e.target.closest('.pol-del'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            grdfrm_ColumnButtonClick(+tr.getAttribute('data-i'));
        });
        function bind() { grd.set(table); }                             // grdfrm.DataSource = table; RetrieveStructure; grdSettings
        function fromSalaryReadOnly(on) { HRM.readOnly('txtfromsalary', on); }
        function fromSalaryEnabled(on) { HRM.enable('txtfromsalary', on); }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function ProfileType(rows) {                                    // ProfileType(): BindDDLNew(..., "ProfileId", "ProfileName", false)
            if (rows && rows.length) HRM.fill('cmbSalaryFactor', rows, 'ProfileId', 'ProfileName', { zero: '', keep: true });
        }
        function resetdetail() {
            HRM.setVal('txttosalary', ''); HRM.setVal('txtpercent', ''); HRM.setVal('txtamount', '');
            HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        }
        function Resetmain() {
            HRM.setVal('txtpolicydescription', '');
            HRM.setCombo('cmbSalaryFactor', 0);
            HRM.setVal('txtminyearlimit', '');
            HRM.setVal('txtfromdate', HRM.today()); HRM.setVal('txttodate', HRM.today());
            table = []; grd.clear();                                    // table.Rows.Clear(); grdfrm.ClearStructure()
            fromSalaryReadOnly(false);                                  // (the desktop does not re-enable it nor clear its text)
            buttons(false);
            UpdateMode = false; RecId = 0;
            resetdetail();
        }
        function formvalidation() {
            if (HRM.comboText('cmbSalaryFactor').trim() === '') { HRM.box('Please Select Salary Factor'); HRM.focus('cmbSalaryFactor'); return false; }
            if (HRM.val('txtpolicydescription').trim() === '') { HRM.box('Please Insert Policy Descripton'); HRM.focus('txtpolicydescription'); return false; }
            return true;
        }
        function formvalidationDetail() {
            if (HRM.val('txtfromsalary').trim() === '') { HRM.box('Please Insert From Salary'); HRM.focus('txtfromsalary'); return false; }
            if (HRM.val('txttosalary').trim() === '') { HRM.box('Please Insert To Salary'); HRM.focus('txttosalary'); return false; }
            if (HRM.val('txtpercent').trim() === '' && HRM.val('txtamount').trim() === '') { HRM.box('Please Insert Percent Or Flat Amount'); HRM.focus('txtpercent'); return false; }
            if (intParse(HRM.val('txtfromsalary')) > intParse(HRM.val('txttosalary'))) { HRM.box('Please Check From Salary Is Greater Than To Salary'); HRM.focus('txtfromsalary'); return false; }
            return true;
        }
        function nextFromSalary() {                                     // lastRow.Cells["ToSalary"] + 1, txtfromsalary disabled
            var last = table.length ? table[table.length - 1] : null;
            var lastAmountValue = last ? cDbl(last.ToSalary) : 0;
            HRM.setVal('txtfromsalary', dstr(lastAmountValue + 1));
            fromSalaryEnabled(false);
            HRM.focus('txttosalary');
        }
        P.btnplus = function () {                                       // btnplus_Click
            try {
                if (!formvalidationDetail()) return;
                var from = cDbl(HRM.val('txtfromsalary'));
                for (var i = 0; i < table.length; i++) {
                    if (from < cDbl(table[i].FromSalary)) { HRM.box('From Salary Should Not Greater then Record that Already Exist'); return; }
                }
                table.push({ FromSalary: HRM.val('txtfromsalary').trim(), ToSalary: HRM.val('txttosalary').trim(),
                    'Percent%': cDbl(HRM.val('txtpercent').trim()), Amount: cDbl(HRM.val('txtamount').trim()) });
                bind();
                HRM.setVal('txtfromsalary', dstr(1 + cDbl(HRM.val('txttosalary'))));
                resetdetail();
                fromSalaryReadOnly(true);
                HRM.focus('txttosalary');
            } catch (e) { HRM.fail(e); }
        };
        function grdfrm_DoubleClick(r, i) {
            updateDetailIndex = i;
            HRM.setVal('txtfromsalary', HRM.str(r.FromSalary));
            HRM.setVal('txttosalary', HRM.str(r.ToSalary));
            HRM.setVal('txtpercent', dstr(r['Percent%']));
            HRM.setVal('txtamount', dstr(r.Amount));
            HRM.show('btnplus', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
        }
        P.btnCancelUpdateDetial = function () { resetdetail(); };      // btnCancelUpdateDetial_Click
        P.btnUpdateDetail = function () {                              // btnUpdateDetail_Click
            try {
                if (!formvalidationDetail()) return;
                var r = table[updateDetailIndex];
                if (!r) return;
                r.FromSalary = HRM.val('txtfromsalary');
                r.ToSalary = HRM.val('txttosalary');
                r['Percent%'] = cDbl(HRM.val('txtpercent'));
                r.Amount = cDbl(HRM.val('txtamount'));
                bind();
                resetdetail();
                nextFromSalary();
            } catch (e) { HRM.fail(e); }
        };
        function grdfrm_ColumnButtonClick(i) {                          // Delete: r.Delete() then the next From Salary
            if (i < 0 || i >= table.length) return;
            table.splice(i, 1);
            bind();
            nextFromSalary();
        }
        function Insert(btn) {                                          // Insert()
            if (!formvalidation()) return;
            if (HRM.val('txtfromdate') > HRM.val('txttodate')) { HRM.box('DateFrom Is Greater Than DateTo please Check: Thank You'); return; }
            if (!table.length) { HRM.box('Please Insert Record In Detail ThankYou'); return; }
            var tot = 0; table.forEach(function (r) { tot += HRM.num(r['Percent%']); });
            if (!/^-?\d+$/.test(dstr(tot))) { HRM.box('Input string was not in a correct format.'); return; }
            if (tot > 100) { HRM.box('Your Total Percent Is Greater Than 100 Please Check It'); return; }
            var body = {
                id: RecId,
                salaryFactorProfileId: HRM.comboVal('cmbSalaryFactor'),
                salaryFactorText: HRM.comboText('cmbSalaryFactor'),
                fromDate: HRM.val('txtfromdate'), toDate: HRM.val('txttodate'),
                minYearLimit: HRM.val('txtminyearlimit'),
                policyDescription: HRM.val('txtpolicydescription'),
                details: table.map(function (r) { return { fromSalary: HRM.str(r.FromSalary), toSalary: HRM.str(r.ToSalary), percent: HRM.num(r['Percent%']), amount: HRM.num(r.Amount) }; })
            };
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Save Successfully'));
                    Resetmain();
                }).catch(HRM.fail);
            }, 'social-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { Resetmain(); };                                     // btnnew_Click
        P.btnRefresh = function (btn) {                                              // btnRefresh_Click -> ProfileType()
            return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/factors').then(ProfileType).catch(HRM.fail); });
        };
        P.btnsalaryfactor = function () { HRM.open('/hrm/define-profile?profileTypeId=5'); };   // ProfileDefine { profileTypeId = 5 }.Show()
        function GetById(id) {                                          // GetById(Id)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = HRM.int(o.SocialPolicyId);
                HRM.setCombo('cmbSalaryFactor', o.SalaryFactorProfileId);
                HRM.setVal('txtfromdate', HRM.day(o.FromDate) || HRM.today());
                HRM.setVal('txttodate', HRM.day(o.ToDate) || HRM.today());
                HRM.setVal('txtminyearlimit', HRM.str(o.MinYearLimit));
                HRM.setVal('txtpolicydescription', HRM.str(o.PolicyDescription));
                var det = o.details || [];
                if (det.length > 0) {
                    // the desktop adds to the table without clearing it, and puts SecurityPercent into Amount too
                    det.forEach(function (s) { table.push({ FromSalary: dstr(s.FromSalary), ToSalary: dstr(s.ToSalary), 'Percent%': cDbl(s.SecurityPercent), Amount: cDbl(s.SecurityPercent) }); });
                    bind();
                    fromSalaryEnabled(false);
                }
                buttons(true);
                UpdateMode = true;
            }).catch(HRM.fail);
        }
        function HistoryDetailBind(id, g) {                             // HistoryDetailBind(Id)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                var det = o.details || [];
                if (!det.length) return;
                g.set(det.map(function (s) { return { FromSalary: dstr(s.FromSalary), ToSalary: dstr(s.ToSalary), 'Percent%': dstr(s.SecurityPercent), Amount: dstr(s.SecurityPercent) }; }));
            }).catch(HRM.fail);
        }
        HRM.footer(function () {                                        // History tab -> HistoryFill() (grdHistory Edit / Detail)
            HRM.history({
                title: 'Social Security - History',
                columns: [
                    { key: 'SocialPolicyId', caption: 'SocialPolicyId', hidden: true },
                    { key: 'ProfileName', caption: 'ProfileName' },
                    { key: 'FromDate', caption: 'FromDate' },
                    { key: 'ToDate', caption: 'ToDate' },
                    { key: 'MinYearLimit', caption: 'MinYearLimit', align: 'right' },
                    { key: 'PolicyDescription', caption: 'PolicyDescription' },
                    { key: 'Edit', caption: 'Edit', type: 'code', width: 50 },
                    { key: 'Detail', caption: 'Detail', width: 50, render: detailLink }
                ],
                load: function () { return HRM.get(API + '/history').then(function (rows) { return (rows || []).map(function (r) { r.Edit = 'Edit'; r.Detail = 'Detail'; return r; }); }); },
                onPick: function (r) { GetById(HRM.int(r.SocialPolicyId)); },
                onReady: function (body, hg) {
                    body.insertAdjacentHTML('beforeend', detailBox('Detail', 'grdHistoryDetail'));
                    HRM.wireFullscreen(body);
                    var dg = new HRM.Grid(body.querySelector('#grdHistoryDetail'), {
                        columns: [{ key: 'FromSalary', caption: 'FromSalary' }, { key: 'ToSalary', caption: 'ToSalary' },
                            { key: 'Percent%', caption: 'Percent%' }, { key: 'Amount', caption: 'Amount' }],
                        filterRow: true, totals: true
                    });
                    body.addEventListener('click', function (e) {
                        var a = e.target.closest('a.pol-detail'); if (!a) return;
                        e.preventDefault();
                        var tr = a.closest('tr[data-i]'); if (!tr) return;
                        HistoryDetailBind(HRM.int(hg.rows()[+tr.getAttribute('data-i')].SocialPolicyId), dg);
                    });
                }
            });
        });
        HRM.keys({                                                      // SocialSecurity_KeyDown
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        dateGuard('txtfromdate'); dateGuard('txttodate');
        HRM.setVal('txtfromdate', HRM.today()); HRM.setVal('txttodate', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {       // SocialSecurity_Load
            rights = d.rights || {};
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
            buttons(false);
            ProfileType(d.factors);
            HRM.focus('cmbSalaryFactor');
        }).catch(HRM.fail);
    }

    // ============================================================================ 644 General Policy
    function generalPolicy() {
        var RecId = 0, updatemode = false;
        var TEXT = ['txtpayrollcutoffday', 'txtadvanceafterday', 'txtadvancelimitpercent', 'txtotfactorrate', 'txtmonthlyshorthourslimit',
            'txtnoofshortleaveinmonth', 'txtnoofshorthoursinday', 'txtmarkabsentaftermin', 'txtdeductionleavenofoflate',
            'txtdeductionleavenoofshortleave', 'txtdeductionleavenoofhalfday', 'txtcplapplyminuteshours', 'txtCplCalculateHours', 'txtovertimeapplyaftermin'];
        var CHECK = ['chkismonthendpayroll', 'chkisseprateotpayroll', 'chkisallowmultishiftinoneday', 'chkshowlatearrivalwithgracetime'];
        function GetHeaderId(rows) {                                    // GetHeaderId(): BindDDLNew(dt, cmbid, "PolicyHeaderId", "DateFrom", "DateFrom", false)
            if (rows && rows.length) HRM.fill('cmbid', rows, 'PolicyHeaderId', 'DateFrom', { zero: '', keep: true });
        }
        function saveText(update) { var b = HRM.$('btnSave'); if (b) b.querySelector('span').innerHTML = update ? '<u>U</u>pdate' : '<u>S</u>ave'; }
        function cmbid_Leave() {                                        // cmbbeniftprofile_Leave
            RecId = HRM.comboVal('cmbid');
            return HRM.loading(HRM.get(API + '/by-id', { id: RecId })).then(function (d) {
                GetHeaderId(d.headers);
                var r = d.row;
                if (r) {
                    HRM.setVal('txtpayrollcutoffday', r.PayrollCutOffDay);
                    HRM.check('chkismonthendpayroll', r.IsMonthEndPayroll);
                    HRM.setVal('txtadvanceafterday', r.AdvanceAfterDay);
                    HRM.setVal('txtadvancelimitpercent', r.AdvanceLimitPercent);
                    HRM.setVal('txtotfactorrate', r.OTFactorRate);
                    HRM.check('chkisseprateotpayroll', r.IsSeparteOTPayroll);
                    HRM.setVal('txtmonthlyshorthourslimit', r.ShortNoOfHours);
                    HRM.setVal('txtnoofshortleaveinmonth', r.NoOfShortLeaveInMonth);
                    HRM.setVal('txtnoofshorthoursinday', r.NoOfShortHoursInDay);
                    HRM.setVal('txtdeductionleavenofoflate', r.LateNoOfDay);
                    HRM.setVal('txtdeductionleavenoofshortleave', r.NoOfShortLeave);
                    HRM.setVal('txtdeductionleavenoofhalfday', r.NoOfHalfDay);
                    HRM.setVal('txtcplapplyminuteshours', r.CPLApplyMinHours);
                    HRM.setVal('txtovertimeapplyaftermin', r.OTApplyAfterMin);
                    HRM.check('chkisallowmultishiftinoneday', r.AllowMultiShiftToEmployeeOnDay);
                    HRM.check('chkshowlatearrivalwithgracetime', r.LateArrivalWithGraceTime);
                    saveText(true);
                    updatemode = true;
                } else {
                    HRM.box('Record No Found');
                }
            }).catch(HRM.fail);
        }
        // UltraCombo.Leave: focus leaves the combo (its field, its popup) for another control
        document.addEventListener('focusout', function (e) {
            var sel = HRM.$('cmbid'); if (!sel) return;
            var wrap = sel.closest('.dtcombo-wrap') || sel;
            if (!wrap.contains(e.target)) return;
            if (e.relatedTarget && wrap.contains(e.relatedTarget)) return;
            if (!e.relatedTarget) return;                                // window lost focus: no Leave
            cmbid_Leave();
        });
        function Reset() {                                              // Reset()
            RecId = 0;
            HRM.setCombo('cmbid', 0);
            HRM.setVal('txtdatefrom', HRM.today()); HRM.setVal('txtdateto', HRM.today());
            TEXT.forEach(function (id) { HRM.setVal(id, ''); });
            CHECK.forEach(function (id) { HRM.check(id, false); });
            updatemode = false;
            saveText(false);
            HRM.focus('txtdatefrom');
        }
        P.btnsave = function (btn) {                                    // btnsave_Click
            var body = {
                id: RecId, updateMode: updatemode,
                dateFrom: HRM.val('txtdatefrom'), dateTo: HRM.val('txtdateto'),
                payrollCutOffDay: HRM.val('txtpayrollcutoffday'), isMonthEndPayroll: HRM.checked('chkismonthendpayroll'),
                advanceAfterDay: HRM.val('txtadvanceafterday'), advanceLimitPercent: HRM.val('txtadvancelimitpercent'),
                otFactorRate: HRM.val('txtotfactorrate'), isSeparteOTPayroll: HRM.checked('chkisseprateotpayroll'),
                monthlyShortHoursLimit: HRM.val('txtmonthlyshorthourslimit'), noOfShortLeaveInMonth: HRM.val('txtnoofshortleaveinmonth'),
                noOfShortHoursInDay: HRM.val('txtnoofshorthoursinday'), deductionLeaveNoOfLate: HRM.val('txtdeductionleavenofoflate'),
                deductionLeaveNoOfShortLeave: HRM.val('txtdeductionleavenoofshortleave'), deductionLeaveNoOfHalfDay: HRM.val('txtdeductionleavenoofhalfday'),
                cplApplyMinutesHours: HRM.val('txtcplapplyminuteshours'), cplCalculateHours: HRM.val('txtCplCalculateHours'),
                overtimeApplyAfterMin: HRM.val('txtovertimeapplyaftermin'),
                isAllowMultiShiftInOneDay: HRM.checked('chkisallowmultishiftinoneday'),
                showLateArrivalWithGraceTime: HRM.checked('chkshowlatearrivalwithgracetime')
            };
            return HRM.busy(btn || 'btnSave', function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    GetHeaderId(d.headers);
                    if (d.message) HRM.box(d.message);
                    Reset();
                }).catch(HRM.fail);
            }, 'general-save');
        };
        P.btnnew = function () { Reset(); };                            // btnnew_Click
        P.btnRefresh = function (btn) {                                 // btnRefresh_Click -> GetHeaderId()
            return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/headers').then(GetHeaderId).catch(HRM.fail); });
        };
        HRM.footer(function () {                                        // no History of its own: the Load Record list (same proc) as a dialog
            HRM.history({
                title: 'General Policy - History',
                columns: [
                    { key: 'PolicyHeaderId', caption: 'PolicyHeaderId', type: 'code', width: 90 },
                    { key: 'DateFromText', caption: 'DateFrom', width: 120 },
                    { key: 'DateToText', caption: 'DateTo', width: 120 }
                ],
                load: function () { return HRM.get(API + '/headers'); },
                onPick: function (r) { HRM.setCombo('cmbid', r.PolicyHeaderId); cmbid_Leave(); }
            });
        });
        HRM.keys({                                                      // GeneralPolicy_KeyDown (no Enter -> Tab on this form)
            enterTab: false,
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+s': function () { if (!updatemode) P.btnsave(); },
            'ctrl+u': function () { if (updatemode) P.btnsave(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        toolStripNoFocus();
        dateGuard('txtdatefrom'); dateGuard('txtdateto');
        HRM.setVal('txtdatefrom', HRM.today()); HRM.setVal('txtdateto', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {       // GeneralPolicy_Load
            GetHeaderId(d.headers);
            HRM.focus('cmbid');
        }).catch(HRM.fail);
    }

    // ============================================================================ 645 Salary Breakup Policy
    function salaryBreakupPolicy() {
        var RecId = 0, UpdateMode = false, locations = [];
        var grd = new HRM.Grid('grdfrm', {
            columns: [                                                  // grdfrmSetting()
                { key: 'Id', caption: 'Id', hidden: true },
                { key: 'SalaryId', caption: 'SalaryId', hidden: true },
                { key: 'SalaryType', caption: 'SalaryType', width: 240 },
                { key: 'Percentage', caption: 'Percentage', type: 'edit-num', width: 120, sum: true }
            ],
            filterRow: true, totals: true
        });
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function LocationFill(rows) {                                   // LocationFill(): BindDDLNew(..., "LocationId", "LocationName", false)
            if (rows && rows.length) { locations = rows; HRM.fill('cmbLocation', rows, 'LocationId', 'LocationName', { zero: '', keep: true }); }
        }
        function ExistRecordGridFillByLocationId(locationId) {          // cmbLocation_ValueChanged
            return HRM.loading(HRM.get(API + '/by-location', { locationId: locationId })).then(function (d) {
                grd.set(d.rows || []);
                if (d.exists) { RecId = HRM.int(d.recId); buttons(true); UpdateMode = true; }
                else { buttons(false); UpdateMode = false; }
            }).catch(HRM.fail);
        }
        HRM.$('cmbLocation').addEventListener('change', function () { ExistRecordGridFillByLocationId(HRM.comboVal('cmbLocation')); });
        function Reset() {                                              // Reset(): cmbLocation.Text = "" (ValueChanged -> no rows -> gridfill)
            buttons(false); UpdateMode = false;
            HRM.setCombo('cmbLocation', 0);
            return ExistRecordGridFillByLocationId(0);
        }
        function Insert(btn) {                                          // Insert()
            if (HRM.comboVal('cmbLocation') === 0) { HRM.box('Location Required'); return; }
            if (grd.sum('Percentage') !== 100.0) { HRM.box('Percent Total should be Equal to 100'); return; }
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var body = {
                id: RecId, locationId: HRM.comboVal('cmbLocation'),
                details: grd.rows().map(function (r) { return { id: HRM.int(r.Id), salaryId: HRM.int(r.SalaryId), percentage: HRM.num(r.Percentage) }; })
            };
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'breakup-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { Reset(); };                                         // btnnew_Click
        P.btnRefresh = function (btn) {                                              // btnRefresh_Click -> LocationFill()
            return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/locations').then(LocationFill).catch(HRM.fail); });
        };
        P.btnlocation = function () { HRM.open('/hrm/location'); };                  // new frmgenLocation(UserAccount).Show()
        HRM.footer(function () {                                        // no History tab: hrmSalaryBreakupPolicy.Getall (BLL ReadAll) as a dialog
            HRM.history({
                title: 'Salary Breakup Policy - History',
                columns: [
                    { key: 'LocationName', caption: 'Location', type: 'code', width: 200 },
                    { key: 'SalaryType', caption: 'SalaryType', width: 200 },
                    { key: 'SalaryTypePercent', caption: 'Percentage', type: 'num', decimals: 2, width: 100 }
                ],
                load: function () {
                    return HRM.get(API + '/history').then(function (rows) {
                        return (rows || []).map(function (r) {
                            var l = locations.filter(function (x) { return HRM.int(x.LocationId) === HRM.int(r.LocationId); })[0];
                            r.LocationName = l ? l.LocationName : HRM.str(r.LocationId);
                            return r;
                        });
                    });
                },
                onPick: function (r) { HRM.setCombo('cmbLocation', r.LocationId); ExistRecordGridFillByLocationId(HRM.comboVal('cmbLocation')); }
            });
        });
        HRM.keys({                                                      // SalaryBreakupPolicy_KeyDown
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+u': function () { if (UpdateMode) P.btnupdate(); },
            'ctrl+e': HRM.close, 'esc': HRM.close
        });
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {       // ProfileDefine_Load: LocationFill(); gridfill()
            LocationFill(d.locations);
            grd.set(d.rows || []);
            buttons(false);
            HRM.focus('cmbLocation');
        }).catch(HRM.fail);
    }

    // ============================================================================ 646 Leave Quota Policy
    function leaveQuotaPolicy() {
        var RecId = 0, UpdateMode = false, rights = {};
        function quotaColumn() {
            return {
                key: 'QuotaValue', caption: 'QuotaValue', type: 'edit', width: 100, align: 'right', sum: true,
                onChange: function (r, k, i, el) {                      // grd*_UpdatingCell: IsNumeric else the message (value not taken)
                    var v = el.value;
                    if (!isDecimal(v)) {
                        HRM.box('Please Type Only Numeric Value');
                        if (String(v).trim() === '') r.QuotaValue = null;
                        else { r.QuotaValue = r._q; el.value = dstr(r._q); }
                    } else {
                        r.QuotaValue = HRM.num(v); r._q = r.QuotaValue;
                    }
                }
            };
        }
        function leaveGrid(id) {
            return new HRM.Grid(id, {
                columns: [{ key: 'Id', caption: 'Id', hidden: true }, { key: 'LeaveType', caption: 'LeaveType', width: 300 }, quotaColumn()],
                filterRow: true, totals: true
            });
        }
        var grdcasualleave = leaveGrid('grdcasualleave'), grdspecialleave = leaveGrid('grdspecialleave');
        function rowsOf(list) { return (list || []).map(function (r) { r._q = HRM.num(r.QuotaValue); r.QuotaValue = HRM.num(r.QuotaValue); return r; }); }
        function fillGrids(d) { grdcasualleave.set(rowsOf(d.casual)); grdspecialleave.set(rowsOf(d.special)); }
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function Reset() {                                              // Reset()
            HRM.setVal('txtdatefrom', HRM.today()); HRM.setVal('txtdateto', HRM.today());
            HRM.setVal('txtdescription', '');
            HRM.focus('txtdatefrom');
            buttons(false); UpdateMode = false;
            return HRM.loading(HRM.get(API + '/grids')).then(fillGrids).catch(HRM.fail);   // gridCasualLeaveFill(); gridSpecialLeaveFill()
        }
        function quota(r) { var v = r.QuotaValue; return v === null || v === undefined || String(v).trim() === '' ? null : HRM.num(v); }
        function Insert(btn) {                                          // Insert()
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var body = {
                id: RecId, fromDate: HRM.val('txtdatefrom'), toDate: HRM.val('txtdateto'), description: HRM.val('txtdescription'),
                casual: grdcasualleave.rows().map(function (r) { return { id: HRM.int(r.Id), quotaValue: quota(r) }; }),
                special: grdspecialleave.rows().map(function (r) { return { id: HRM.int(r.Id), quotaValue: quota(r) }; })
            };
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return Reset();
                }).catch(HRM.fail);
            }, 'leavequota-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { Reset(); };                                         // btnnew_Click
        P.btnlookup = function () { HRM.open('/hrm/define-profile?profileTypeId=4'); };        // Define Special Leaves
        P.btnOfficialleave = function () { HRM.open('/hrm/define-profile?profileTypeId=3'); }; // Define Offical Leaves
        function GetById(id) {                                          // GetById(Id)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                RecId = HRM.int(o.LeaveQuotaId);
                HRM.setVal('txtdatefrom', HRM.day(o.FromDate) || HRM.today());
                HRM.setVal('txtdateto', HRM.day(o.ToDate) || HRM.today());
                HRM.setVal('txtdescription', HRM.str(o.Description));
                var det = o.details || [];
                if (det.length > 0) {
                    var c = [], s = [];
                    det.forEach(function (d) {
                        var row = { Id: d.LeaveTypeProfileId, LeaveType: d.ProfileName, QuotaValue: HRM.num(d.QuotaValue) };
                        if (d.ProfileTypeName === 'Casual Leave') c.push(row);
                        if (d.ProfileTypeName === 'Special Leave') s.push(row);
                    });
                    fillGrids({ casual: c, special: s });
                }
                buttons(true);
                UpdateMode = true;
            }).catch(HRM.fail);
        }
        function HistoryDetailBind(id, gc, gs) {                        // HistoryDetailBind(Id)
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                var det = o.details || [];
                if (det.length <= 0) return;
                var c = [], s = [];
                det.forEach(function (d) {
                    var row = { LeaveType: d.ProfileName, QuotaValue: d.QuotaValue };
                    if (d.ProfileTypeName === 'Casual Leave') c.push(row);
                    if (d.ProfileTypeName === 'Special Leave') s.push(row);
                });
                gc.set(c); gs.set(s);
            }).catch(HRM.fail);
        }
        HRM.footer(function () {                                        // History tab -> HistoryGridFill() (Detail / Edit buttons)
            HRM.history({
                title: 'Leave Quota Policy - History',
                columns: [
                    { key: 'Detail', caption: 'Detail', width: 50, render: detailLink },
                    { key: 'Edit', caption: 'Edit', type: 'code', width: 50 },
                    { key: 'LeaveQuotaId', caption: 'LeaveQuotaId', hidden: true },
                    { key: 'FromDate', caption: 'FromDate' },
                    { key: 'ToDate', caption: 'ToDate' },
                    { key: 'Description', caption: 'Description', width: 300 }
                ],
                load: function () { return HRM.get(API + '/history').then(function (rows) { return (rows || []).map(function (r) { r.Edit = 'Edit'; r.Detail = 'Detail'; return r; }); }); },
                onPick: function (r) { GetById(HRM.int(r.LeaveQuotaId)); },
                onReady: function (body, hg) {
                    body.insertAdjacentHTML('beforeend', '<div class="pol-two">' + detailBox('Offical Leave', 'grdCasualHistory') + detailBox('Special Leave', 'grdSpecialHistory') + '</div>');
                    HRM.wireFullscreen(body);
                    var cols = function () { return [{ key: 'LeaveType', caption: 'LeaveType', width: 300 }, { key: 'QuotaValue', caption: 'QuotaValue', width: 100, align: 'right', sum: true }]; };
                    var gc = new HRM.Grid(body.querySelector('#grdCasualHistory'), { columns: cols(), filterRow: true, totals: true });
                    var gs = new HRM.Grid(body.querySelector('#grdSpecialHistory'), { columns: cols(), filterRow: true, totals: true });
                    body.addEventListener('click', function (e) {
                        var a = e.target.closest('a.pol-detail'); if (!a) return;
                        e.preventDefault();
                        var tr = a.closest('tr[data-i]'); if (!tr) return;
                        HistoryDetailBind(HRM.int(hg.rows()[+tr.getAttribute('data-i')].LeaveQuotaId), gc, gs);
                    });
                }
            });
        });
        HRM.keys({                                                      // LeaveQuotaPolicy_KeyDown
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (UpdateMode) P.btnupdate(); }
        });
        dateGuard('txtdatefrom'); dateGuard('txtdateto');
        HRM.setVal('txtdatefrom', HRM.today()); HRM.setVal('txtdateto', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {       // LeaveQuotaPolicy_Load
            rights = d.rights || {};
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
            buttons(false);
            fillGrids(d);
            HRM.focus('txtdatefrom');
        }).catch(HRM.fail);
    }

    // ============================================================================ 647 E.O.B.I Policy
    function eobiPolicy() {
        var RecId = 0, UpdateMode = false, rights = {};
        var grd = new HRM.Grid('grdfrm', {
            columns: [                                                  // grdEmployeeSetting()
                { key: 'EmployeeId', caption: 'EmployeeId', hidden: true },
                { key: 'EmployeeName', caption: 'EmployeeName', width: 350 },
                { key: 'DOB', caption: 'DOB', width: 100 },
                { key: 'TotalSalary', caption: 'TotalSalary', align: 'right', sum: true, render: fmt00 },
                { key: 'EmployeeShare', caption: 'EmployeeShare', width: 120, align: 'right', sum: true, render: fmt00 },
                { key: 'CompanyShare', caption: 'CompanyShare', width: 120, align: 'right', sum: true, render: fmt00 },
                { key: 'TotalShare', caption: 'TotalShare', width: 120, align: 'right', sum: true, render: fmt00 }
            ],
            filterRow: true, totals: true,
            // FormatConditions: CompanyShare > 0 / EmployeeShare > 0 -> DarkRed, bold
            rowClass: function (r) { return (HRM.num(r.CompanyShare) > 0 || HRM.num(r.EmployeeShare) > 0) ? 'hrm-row-red hrm-row-bold' : ''; }
        });
        function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
        function DetailGridFill() {                                     // DetailGridFill()
            return HRM.loading(HRM.get(API + '/employees')).then(function (rows) { if (rows && rows.length) grd.set(rows); else grd.clear(); }).catch(HRM.fail);
        }
        function formvalidation() {
            if (HRM.val('txtpolicydescription').trim() === '') { HRM.box('Please Insert Policy Description'); HRM.focus('txtpolicydescription'); return false; }
            if (HRM.val('txtagelimit').trim() === '') { HRM.box('Please Insert Age Limit'); HRM.focus('txtagelimit'); return false; }
            if (HRM.val('txtemployeeshare').trim() === '') { HRM.box('Please Insert Employee Share'); HRM.focus('txtemployeeshare'); return false; }
            if (HRM.val('txtcompanyshare').trim() === '') { HRM.box('Please Insert Company Share'); HRM.focus('txtcompanyshare'); return false; }
            return true;
        }
        function clear() {                                              // clear()
            HRM.setVal('txtpolicydescription', '');
            HRM.setVal('txtagelimit', ''); HRM.setVal('txtcompanyshare', ''); HRM.setVal('txtemployeeshare', '');
            buttons(false); UpdateMode = false;
            HRM.enable('btnAddToEmployee', true);
            HRM.focus('txtpolicydescription');
            return DetailGridFill();
        }
        function dobYear(s) {                                           // Conversion.ToDateTime("dd-MMM-yyyy").Year (1900 when not a date)
            var m = String(s || '').match(/(\d{4})\s*$/);
            return m ? +m[1] : 1900;
        }
        P.btnAddToEmployee = function () {                              // btnAddToEmployee_Click
            if (!formvalidation()) return;
            var year = new Date().getFullYear();
            var AgeLimit = cInt(HRM.val('txtagelimit'));
            grd.rows().forEach(function (r) {
                var num = year - dobYear(r.DOB);
                if (num > AgeLimit) {
                    r.EmployeeShare = cDbl(HRM.val('txtemployeeshare'));
                    r.CompanyShare = cDbl(HRM.val('txtcompanyshare'));
                } else {
                    r.EmployeeShare = 0; r.CompanyShare = 0;
                }
                r.TotalShare = cInt(r.EmployeeShare) + cInt(r.CompanyShare);
            });
            grd.draw();
        };
        function Insert(btn) {                                          // Insert()
            if (!formvalidation()) return;
            if (HRM.val('txtdatefrom') > HRM.val('txtdateto')) { HRM.box('Date From Is Greater Than Date To Please Check'); HRM.focus('txtdatefrom'); return; }
            if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
            var body = {
                id: RecId, policyDescription: HRM.val('txtpolicydescription'),
                fromDate: HRM.val('txtdatefrom'), toDate: HRM.val('txtdateto'),
                ageLimit: HRM.val('txtagelimit'), employeeShare: HRM.val('txtemployeeshare'), companyShare: HRM.val('txtcompanyshare'),
                details: grd.rows().filter(function (r) { return cDbl(r.EmployeeShare) > 0; })
                    .map(function (r) { return { employeeId: HRM.int(r.EmployeeId), employeeShare: cDbl(r.EmployeeShare), companyShare: cDbl(r.CompanyShare) }; })
            };
            return HRM.busy(btn, function () {
                return HRM.post(API + '/save', body).then(function (d) {
                    HRM.box(d.message || (RecId > 0 ? 'Update Successfully' : 'Save Successfully'));
                    return clear();
                }).catch(HRM.fail);
            }, 'eobi-save');
        }
        P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };   // btnsave_Click
        P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };         // btnupdate_Click
        P.btnnew = function () { clear(); };                                         // btnnew_Click
        function DataHistoryGrrid_DoubleClick(id) {                     // DataHistoryGrrid_DoubleClick / Edit button
            RecId = id;
            return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
                HRM.setVal('txtpolicydescription', HRM.str(o.PolicyDescription));
                HRM.setVal('txtdatefrom', HRM.day(o.FromDate) || HRM.today());
                HRM.setVal('txtdateto', HRM.day(o.ToDate) || HRM.today());
                HRM.setVal('txtagelimit', String(cInt(o.AgeLimit)));
                HRM.setVal('txtemployeeshare', dstr(cDbl(o.EmployeeShare)));
                HRM.setVal('txtcompanyshare', dstr(cDbl(o.CompanyShare)));
                buttons(true); UpdateMode = true;
                P.btnAddToEmployee();
            }).catch(HRM.fail);
        }
        HRM.footer(function () {                                        // History tab -> gridfill() (Edit button + double-click)
            HRM.history({
                title: 'E.O.B.I Policy - History',
                columns: [
                    { key: 'Id', caption: 'Id', hidden: true },
                    { key: 'PolicyDescription', caption: 'PolicyDescription', width: 220 },
                    { key: 'FromDate', caption: 'FromDate', width: 100 },
                    { key: 'ToDate', caption: 'ToDate', width: 100 },
                    { key: 'AgeLimit', caption: 'AgeLimit', width: 100 },
                    { key: 'CompanyShare', caption: 'CompanyShare', width: 120 },
                    { key: 'EmployeeShare', caption: 'EmployeeShare', width: 120 },
                    { key: 'Edit', caption: 'Edit', type: 'code', width: 40 }
                ],
                load: function () { return HRM.get(API + '/history').then(function (rows) { return (rows || []).map(function (r) { r.Edit = 'Edit'; return r; }); }); },
                onPick: function (r) { DataHistoryGrrid_DoubleClick(HRM.int(r.Id)); }
            });
        });
        HRM.keys({                                                      // EOBIPolicy_KeyDown
            'ctrl+s': function () { if (!UpdateMode) P.btnsave(); },
            'ctrl+n': function () { P.btnnew(); },
            'ctrl+e': HRM.close, 'esc': HRM.close,
            'ctrl+u': function () { if (UpdateMode) P.btnupdate(); }
        });
        digitsOnly('txtagelimit'); digitsOnly('txtemployeeshare'); digitsOnly('txtcompanyshare');
        dateGuard('txtdatefrom'); dateGuard('txtdateto');
        HRM.setVal('txtdatefrom', HRM.today()); HRM.setVal('txtdateto', HRM.today());
        HRM.loading(HRM.get(API + '/setup')).then(function (d) {       // EOBIPolicy_Load
            rights = d.rights || {};
            HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
            if (d.employees && d.employees.length) grd.set(d.employees);
            buttons(false);
            HRM.focus('txtpolicydescription');
        }).catch(HRM.fail);
    }

    var PAGES = {
        'social-security': socialSecurity,
        'general-policy': generalPolicy,
        'salary-breakup-policy': salaryBreakupPolicy,
        'leave-quota-policy': leaveQuotaPolicy,
        'eobi-policy': eobiPolicy
    };
    if (PAGES[PAGE]) PAGES[PAGE]();
})();
