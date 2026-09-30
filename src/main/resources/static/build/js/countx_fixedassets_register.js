/* ============================================================================================
 * countx_fixedassets_register.js - 353 "Assets Register" (Architecture.WinApp.AssetSchema.frmAssetsRegister).
 * Built on countx_hrm.js (window.HRM) + countx_fixedassets_common.js (window.FA); exports window.FaReg.
 * API /api/fixed-assets/assets-register/{setup|items|serial-no|history|by-id|save}.
 * Every handler follows its desktop method (named in the comment): validation wording and order are the
 * server's (FaAssetsRegisterService), the confirmations, resets and focus moves are the form's.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/fixed-assets/assets-register';
    var P = {};
    window.FaReg = P;

    var RECID = 0, UpdateMode = false, rights = {}, L = {}, loaded = false;
    var ACC_COLS = ['AccountCode', 'ParentAccountTitle', 'AccountClass'];

    // ------------------------------------------------------------------------ binds (Load)
    function ItemCategory() {                                                   // ItemCategory():309
        FA.fillCols('CmbAssetCategory', L.categories, 'Id', 'categoryDescription', ['categoryCode'], { keep: true });
        HRM.fill('cmbCategoryHistory', L.categories, 'Id', 'categoryDescription', { keep: true });
    }
    function AssetAccountBind() {                                               // AssetAccountBind():353 - AccountTypeId {1}
        FA.fillCols('CmbAccomolativeAcc', L.assetAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });
        FA.fillCols('CmbGlAccount', L.assetAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });
    }
    function ExpenseAccountFill() {                                             // ExpenseAccountFill():372 - {11,12,14,20,21,23}
        FA.fillCols('CmbExpenseMaintenanceAccount', L.expenseAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });
        FA.fillCols('cmbDepriciationAcc', L.expenseAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });
    }
    function bindAll() {
        ItemCategory();
        AssetAccountBind();
        ExpenseAccountFill();
        HRM.fill('CmbAssetStatus', L.statuses, 'Id', 'StatusName', { keep: true });          // AssetStatusBind()
        HRM.fill('CmbDepartment', L.departments, 'Id', 'DepartmentName', { keep: true });    // DepartmentBind()
        HRM.fill('CmbAssetCondition', L.conditions, 'Id', 'ConditionName', { keep: true });  // AssetConditionBind()
        HRM.fill('CmbDepreciationMethod', L.methods, 'depreciationMethodScheduleId', 'DepreciatonMethodName', { keep: true });
    }
    /** AssetItemFill(categoryId):333 */
    function AssetItemFill(rows) { FA.fillCols('CmbAssetItem', rows || [], 'Id', 'ItemName', ['ItemCode'], { keep: true }); }
    function loadItems(categoryId) {
        return HRM.get(API + '/items', { categoryId: categoryId }).then(function (rows) { AssetItemFill(rows); }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ Leave handlers
    function ClearAccountFields() {                                             // ClearAccountFields():526
        if (HRM.comboVal('CmbAssetCategory') === 0) {
            ['CmbGlAccount', 'CmbAccomolativeAcc', 'cmbDepriciationAcc', 'CmbExpenseMaintenanceAccount'].forEach(function (i) { HRM.setCombo(i, 0); });
        }
    }
    function ClearDepreciationAndAccounts() {                                   // ClearDepreciationAndAccounts():518
        HRM.setCombo('CmbDepreciationMethod', 0);
        HRM.setVal('txtDepreciationRate', '');
        HRM.setVal('txtUseableLifeMonth', '');
        ClearAccountFields();
    }
    /** CmbAssetCategory_Leave():443 */
    function CmbAssetCategory_Leave() {
        var categoryId = HRM.comboVal('CmbAssetCategory');
        var itemId = HRM.comboVal('CmbAssetItem');
        if (categoryId <= 0) { ClearDepreciationAndAccounts(); return Promise.resolve(); }
        var r = HRM.comboRow('CmbAssetCategory', 'Id');
        if (!r) { ClearDepreciationAndAccounts(); return Promise.resolve(); }
        var chain = Promise.resolve();
        var save = HRM.$('btnSave');
        if (HRM.visible('btnSave') && !save.disabled) {                             // btnSave.Visible && btnSave.Enabled
            chain = HRM.get(API + '/serial-no', { categoryId: categoryId }).then(function (d) {
                if (d && d.SerialNo !== null && d.SerialNo !== undefined) HRM.setVal('txtAssetSerialNo', HRM.str(d.SerialNo));
            });
        }
        return chain.then(function () { return loadItems(categoryId); }).then(function () {
            HRM.setCombo('CmbDepreciationMethod', HRM.int(HRM.col(r, 'depreciationMethodScheduleId')));
            HRM.setVal('txtDepreciationRate', HRM.str(HRM.col(r, 'depriciationrate')));
            HRM.setVal('txtUseableLifeMonth', HRM.str(HRM.col(r, 'usefullLifeInMonths')));
            CalculateExpiryDate();                                                    // txtUseableLifeMonth_TextChanged
            if (itemId === 0) {
                HRM.setCombo('CmbGlAccount', HRM.int(HRM.col(r, 'GLAccountId')));
                HRM.setCombo('CmbAccomolativeAcc', HRM.int(HRM.col(r, 'accumulatedAccountId')));
                HRM.setCombo('cmbDepriciationAcc', HRM.int(HRM.col(r, 'depriciationAccountId')));
                HRM.setCombo('CmbExpenseMaintenanceAccount', HRM.int(HRM.col(r, 'expenseAccountId')));
            } else ClearAccountFields();
        }).catch(function (e) { HRM.box('Error loading asset category details: ' + (e && e.message ? e.message : e)); });
    }
    /** CmbAssetItem_Leave():491 */
    function CmbAssetItem_Leave() {
        if (HRM.comboVal('CmbAssetItem') <= 0) { ClearAccountFields(); return; }
        var r = HRM.comboRow('CmbAssetItem', 'Id');
        if (!r) { ClearAccountFields(); return; }
        HRM.setCombo('CmbGlAccount', HRM.int(HRM.col(r, 'GLAccountId')));
        HRM.setCombo('CmbAccomolativeAcc', HRM.int(HRM.col(r, 'accumulatedDepreciationAcId')));
        HRM.setCombo('cmbDepriciationAcc', HRM.int(HRM.col(r, 'DepreciationExpenseAcId')));
        HRM.setCombo('CmbExpenseMaintenanceAccount', HRM.int(HRM.col(r, 'ExpenseMaintenanceAccountId')));
    }
    /** CalculateExpiryDate():1352 - Purchase Date + Useable Life months (DateTime.AddMonths clamps the day). */
    function CalculateExpiryDate() {
        var t = HRM.val('txtUseableLifeMonth');
        if (t === '') return;
        var p = HRM.val('txtPurchaseDate'); if (!p) return;
        var n = HRM.int(t), d = new Date(p + 'T00:00:00');
        var day = d.getDate();
        d.setDate(1); d.setMonth(d.getMonth() + n);
        var last = new Date(d.getFullYear(), d.getMonth() + 1, 0).getDate();
        d.setDate(Math.min(day, last));
        HRM.setVal('txtExpiryDate', HRM.iso(d));
    }

    // ------------------------------------------------------------------------ reset / read
    function buttons(update) { HRM.show('btnSave', !update); HRM.show('btnUpdate', update); }
    function pics(p1, p2) {
        HRM.text('picbox1', p1 ? p1 : 'No picture');
        HRM.text('picbox2', p2 ? p2 : 'No picture');
        HRM.$('picbox1').title = p1 ? 'Stored path (the picture itself is on the desktop share): ' + p1 : '';
        HRM.$('picbox2').title = p2 ? 'Stored path (the picture itself is on the desktop share): ' + p2 : '';
    }
    /** FormReset():794 */
    function FormReset() {
        buttons(false);
        UpdateMode = false;
        RECID = 0;
        HRM.enable('CmbAssetCategory', true);
        HRM.setVal('txtUseableLifeMonth', '');
        HRM.setCombo('CmbAssetCondition', 0);
        ['txtAssetLocation', 'txtAssetSerialNo', 'txtAssetName', 'txtBrandName', 'txtCurrentValue', 'txtMakeDesc', 'txtManufacturer',
            'txtModelDesc', 'txtPurchasePrice', 'txtVendor', 'txtDepreciationRate'].forEach(function (i) { HRM.setVal(i, ''); });
        HRM.setVal('txtExpiryDate', HRM.today());
        HRM.setVal('txtPurchaseDate', HRM.today());
        ['CmbAccomolativeAcc', 'CmbAssetStatus', 'CmbDepartment', 'cmbDepriciationAcc', 'CmbExpenseMaintenanceAccount',
            'CmbDepreciationMethod', 'CmbGlAccount'].forEach(function (i) { HRM.setCombo(i, 0); });
        pics('', '');
        HRM.focus('txtAssetName');
    }
    /** ReadById(Id):685 */
    function ReadById(id) {
        RECID = id;
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            buttons(true);
            UpdateMode = true;
            FA.showTab('form');
            HRM.setCombo('CmbAssetCategory', HRM.int(o.categoryId));
            AssetItemFill(o.items);                                                  // D5: the record's item list
            if (HRM.int(o.itemId) > 0) HRM.setCombo('CmbAssetItem', HRM.int(o.itemId));
            HRM.setVal('txtBrandName', HRM.str(o.brandName));
            HRM.setVal('txtAssetName', HRM.str(o.assetName));
            HRM.setVal('txtModelDesc', HRM.str(o.modelDesc));
            HRM.setCombo('CmbAssetCondition', HRM.int(o.conditionId));
            HRM.setVal('txtAssetSerialNo', HRM.str(o.serialNo));
            HRM.setCombo('CmbDepartment', HRM.int(o.departmentId));
            HRM.setVal('txtManufacturer', HRM.str(o.manufacturerName));
            HRM.setVal('txtMakeDesc', HRM.str(o.makeDesc));
            HRM.setVal('txtAssetLocation', HRM.str(o.locationBranch));
            HRM.setVal('txtVendor', HRM.str(o.vendorName));
            HRM.setVal('txtPurchaseDate', HRM.day(o.purchaseDate));
            HRM.setVal('txtPurchasePrice', HRM.str(o.purchasePrice));
            HRM.setVal('txtUseableLifeMonth', HRM.str(o.useableLifeMonth));
            HRM.setVal('txtExpiryDate', HRM.day(o.expiryDate));                       // set after the life (TextChanged recomputes first)
            HRM.setVal('txtCurrentValue', HRM.str(o.currentValue));
            HRM.setCombo('CmbDepreciationMethod', HRM.int(o.depreciationMethodScheduleId));
            HRM.setVal('txtDepreciationRate', HRM.str(o.depriciationrate));
            HRM.setComboText('CmbAssetStatus', HRM.str(o.assetStatus));
            if (HRM.int(o.accumulateddepriciationAcId) > 0) HRM.setCombo('CmbAccomolativeAcc', HRM.int(o.accumulateddepriciationAcId));
            if (HRM.int(o.depriciationExpenseAcId) > 0) HRM.setCombo('cmbDepriciationAcc', HRM.int(o.depriciationExpenseAcId));
            if (HRM.int(o.glAccountId) > 0) HRM.setCombo('CmbGlAccount', HRM.int(o.glAccountId));
            if (HRM.int(o.ExpenseMaintenanceAccountId) > 0) HRM.setCombo('CmbExpenseMaintenanceAccount', HRM.int(o.ExpenseMaintenanceAccountId));
            pics(HRM.str(o.pic1Path), HRM.str(o.pic2Path));
            HRM.enable('CmbAssetCategory', false);
            HRM.focus('txtAssetName');
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------------ save
    /** Insert():559 - FormValidation, then the confirm, then AssetRegister.Save (server), then FormReset. */
    /** FormValidation():537 - FormHelper.ValidateControls: combo "X field is required", String box "X field is required",
        INT / Double box "X must be a non-zero number". The server repeats every check. */
    function FormValidation() {
        function num(id, ints) {
            var t = HRM.val(id).trim().replace(/,/g, '');
            if (t === '' || isNaN(Number(t)) || (ints && !/^[+-]?\d+$/.test(t))) return false;
            return Number(t) !== 0;
        }
        var list = [
            ['CmbAssetCategory', 'Asset Category', 'c'], ['txtAssetName', 'Asset Name', 's'], ['CmbAssetCondition', 'Asset Condition', 'c'],
            ['txtAssetSerialNo', 'Asset Serial No', 's'], ['CmbDepartment', 'Asset Department', 'c'], ['txtPurchasePrice', 'Purchase Price', 'd'],
            ['txtUseableLifeMonth', 'Useable Life Month', 'i'], ['txtCurrentValue', 'Current Value', 'd'],
            ['CmbDepreciationMethod', 'Depreciation Method', 'c'], ['txtDepreciationRate', 'Depreciation Rate', 'd'],
            ['CmbAssetStatus', 'Asset Status', 'c'], ['CmbGlAccount', 'Asset GL A/c', 'c'], ['cmbDepriciationAcc', 'Depreciation_Expense A/c', 'c'],
            ['CmbAccomolativeAcc', 'Accumulated_Depreciation A/c', 'c'], ['CmbExpenseMaintenanceAccount', 'Expense Maintenance A/c', 'c']];
        for (var i = 0; i < list.length; i++) {
            var id = list[i][0], name = list[i][1], t = list[i][2], ok;
            if (t === 'c') ok = HRM.comboVal(id) !== 0;
            else if (t === 's') ok = HRM.val(id).trim() !== '';
            else ok = num(id, t === 'i');
            if (!ok) {
                HRM.box(name + (t === 'c' || t === 's' ? ' field is required' : ' must be a non-zero number'));
                HRM.focus(id);
                return false;
            }
        }
        return true;
    }
    function Insert(btn, update) {
        if (!FormValidation()) return;
        if (!HRM.ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var st = HRM.$('CmbAssetStatus');
        var body = {
            mode: update ? 'update' : 'save', id: update ? RECID : 0,
            categoryId: HRM.comboVal('CmbAssetCategory'), itemId: HRM.comboVal('CmbAssetItem'),
            brandName: HRM.val('txtBrandName'), assetName: HRM.val('txtAssetName'), conditionId: HRM.comboVal('CmbAssetCondition'),
            modelDesc: HRM.val('txtModelDesc'), departmentId: HRM.comboVal('CmbDepartment'), manufacturerName: HRM.val('txtManufacturer'),
            makeDesc: HRM.val('txtMakeDesc'), locationBranch: HRM.val('txtAssetLocation'), vendorName: HRM.val('txtVendor'),
            purchaseDate: HRM.val('txtPurchaseDate'), purchasePrice: HRM.val('txtPurchasePrice'), useableLifeMonth: HRM.val('txtUseableLifeMonth'),
            expiryDate: HRM.val('txtExpiryDate'), currentValue: HRM.val('txtCurrentValue'),
            assetStatus: HRM.comboText('CmbAssetStatus') || (st && st.value !== '0' && st.selectedIndex >= 0 ? st.options[st.selectedIndex].textContent : ''),
            depreciationMethodScheduleId: HRM.comboVal('CmbDepreciationMethod'), depriciationrate: HRM.val('txtDepreciationRate'),
            glAccountId: HRM.comboVal('CmbGlAccount'), depriciationExpenseAcId: HRM.comboVal('cmbDepriciationAcc'),
            accumulateddepriciationAcId: HRM.comboVal('CmbAccomolativeAcc'), ExpenseMaintenanceAccountId: HRM.comboVal('CmbExpenseMaintenanceAccount')
        };
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (d) {
                HRM.box(d && d.message ? d.message : '');
                FormReset();
            }).catch(HRM.fail);
        }, 'reg-save');
    }

    // ------------------------------------------------------------------------ history
    var grid = new HRM.Grid('DatagridHistory', {
        columns: [
            { key: 'Id', hidden: true }, { key: 'CategoryId', hidden: true },
            { key: 'CategoryDescription', caption: 'CategoryDescription', width: 150 }, { key: 'CategoryCode', caption: 'CategoryCode' },
            { key: 'SerialNo', caption: 'SerialNo' }, { key: 'AssetName', caption: 'AssetName', width: 180 },
            { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 180 }, { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'MakeDesc', caption: 'MakeDesc' }, { key: 'ModelDesc', caption: 'ModelDesc' },
            { key: 'PurchaseDate', caption: 'PurchaseDate', render: function (v) { return HRM.esc(ddMMMyy(v)); } },
            { key: 'PurchasePrice', caption: 'PurchasePrice', type: 'num', decimals: 2 },
            { key: 'UseableLifeMonth', caption: 'UseableLifeMonth', type: 'int' },
            { key: 'ExpiryDate', caption: 'ExpiryDate', render: function (v) { return HRM.esc(ddMMMyy(v)); } },
            { key: 'CurrentValue', caption: 'CurrentValue', type: 'num', decimals: 2 },
            { key: 'AssetStatus', caption: 'AssetStatus' }, { key: 'DepartmentName', caption: 'DepartmentName' },
            { key: 'AssetCondition', caption: 'AssetCondition' },
            { key: 'AccumulatedDepreciationAcId', hidden: true }, { key: 'AccumulatedDepreciationAccount', caption: 'AccumulatedDepreciationAccount' },
            { key: 'DepreciationExpenseAcId', hidden: true }, { key: 'DepreciationExpenseAccount', caption: 'DepreciationExpenseAccount' },
            { key: 'CapitalWIPAccountId', hidden: true }, { key: 'GlAccountAccount', caption: 'GlAccountAccount' },
            { key: 'ExpenseMaintenanceAccount', caption: 'ExpenseMaintenanceAccount' },
            { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(ddMMMyyTime(v)); } },
            { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'LastModifyDate', caption: 'LastModifyDate', render: function (v) { return HRM.esc(ddMMMyyTime(v)); } },
            { key: 'LastModifyUserName', caption: 'LastModifyUserName' }
        ],
        filterRow: true,
        onDouble: function (r) { ReadById(HRM.int(HRM.col(r, 'Id'))); }                 // DatagridHistory_DoubleClick
    });
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parts(v) {
        if (v === null || v === undefined || v === '') return null;
        var d = new Date(typeof v === 'number' ? v : String(v).replace(' ', 'T'));
        return isNaN(d.getTime()) ? null : d;
    }
    function p2(n) { return (n < 10 ? '0' : '') + n; }
    function ddMMMyy(v) { var d = parts(v); return d ? p2(d.getDate()) + '-' + MON[d.getMonth()] + '-' + p2(d.getFullYear() % 100) : ''; }
    function ddMMMyyTime(v) {                                                            // "dd-MMM-yy hh:mm tt"
        var d = parts(v); if (!d) return '';
        var h = d.getHours(), ap = h < 12 ? 'AM' : 'PM', hh = h % 12 === 0 ? 12 : h % 12;
        return ddMMMyy(v) + ' ' + p2(hh) + ':' + p2(d.getMinutes()) + ' ' + ap;
    }
    /** Gridbind():907 */
    function Gridbind(btn) {
        var q = { dateType: HRM.checked('rdmodifydate') ? 'modify' : 'entry' };
        if (HRM.checked('chkFromDateHistory')) q.fromDate = HRM.val('FromDateHistory');
        if (HRM.checked('chkToDateHistory')) q.toDate = HRM.val('ToDateHistory');
        return HRM.busy(btn, function () {
            return HRM.get(API + '/history', q).then(function (rows) { grid.set(rows || []); }).catch(HRM.fail);
        }, 'reg-history');
    }
    function syncDateChecks() {
        HRM.enable('FromDateHistory', HRM.checked('chkFromDateHistory'));
        HRM.enable('ToDateHistory', HRM.checked('chkToDateHistory'));
    }

    // ------------------------------------------------------------------------ toolbar
    P.btnNew = function () { FormReset(); };                                             // btnNew_Click
    P.btnSave = function (btn) { RECID = 0; return Insert(btn || 'btnSave', false); };   // btnSave_Click
    P.btnUpdate = function (btn) {                                                       // btnUpdate_Click
        if (RECID === 0) { HRM.box('Record Not Update because RecId Not Found'); return; }
        return Insert(btn || 'btnUpdate', true);
    };
    P.BtnRefresh = function (btn) {                                                      // BtnRefresh_Click
        return HRM.busy(btn || 'BtnRefresh', function () {
            return HRM.get(API + '/setup').then(function (d) {
                L = d || {};
                bindAll();
                return loadItems(HRM.comboVal('CmbAssetCategory'));
            }).catch(HRM.fail);
        });
    };
    P.BtnNewHistory = function () {                                                      // BtnNewHistory_Click
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -3));
        HRM.setVal('ToDateHistory', HRM.today());
        grid.clear();
        HRM.check('rdentrydate', true);
    };
    P.btnRefreshHistory = function (btn) {                                               // btnRefreshHistory_Click -> ItemCategory()
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return HRM.get(API + '/setup').then(function (d) { L.categories = (d || {}).categories || []; ItemCategory(); }).catch(HRM.fail);
        });
    };
    P.btnShow = function (btn) { return Gridbind(btn || 'btnShow'); };                  // btnShow_Click_1
    P.btnAssetCategory = function () { FA.storeDialog('openAssetCategory'); };          // new frmItemCatagoryStore { FormTypeId = 3 }.Show()
    P.BtnDefineAssetDepartment = function () { FA.storeDialog('openDepartment'); };     // new Define_Department().Show()
    P.BtnAssetItemDefine = function () { HRM.open('/fixed-assets/define-assets'); };    // new frmDefineAssets().Show()
    P.BtnShortCutkeys = function () { MakeShortCutKeys(); };
    /** MakeShortCutKeys():1228 - the form's own list (it names keys this form does not handle; shown as is). */
    function MakeShortCutKeys() {
        var rows = [['Ctrl+S', 'For Save in Form Tab and For Show History in History Tab'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'],
            ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print 273'], ['Alt+1', 'For Print 273'],
            ['Ctrl+F1', 'For Open Stock Report'], ['Ctrl+F2', 'For Open Stock Report With Value'], ['Ctrl+F5', 'For Focus on Doc Date'],
            ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+F12', 'For SaveAs'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab'],
            ['Ctrl+ArrowUp', 'For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', 'When Focus On Any Grid To Call Function\'s On Button Or Link']];
        var m = HRM.modal({ title: 'ShortCut Keys', width: 'min(640px, 96vw)', html: '<div class="hrm-grid-wrap"><table class="fa-keys"></table></div>' });
        var g = new HRM.Grid(m.body.querySelector('.fa-keys'), { columns: [{ key: 'k', caption: 'KeyCombination' }, { key: 'd', caption: 'Description' }] });
        g.set(rows.map(function (r) { return { k: r[0], d: r[1] }; }));
    }

    // ------------------------------------------------------------------------ events
    FA.onLeave('CmbAssetCategory', function () { if (loaded) CmbAssetCategory_Leave(); });
    FA.onLeave('CmbAssetItem', function () { if (loaded) CmbAssetItem_Leave(); });
    HRM.$('txtPurchaseDate').addEventListener('change', CalculateExpiryDate);             // txtPurchaseDate_ValueChanged
    HRM.$('chkFromDateHistory').addEventListener('change', syncDateChecks);
    HRM.$('chkToDateHistory').addEventListener('change', syncDateChecks);
    FA.tabs(function (t) { if (t === 'history') HRM.focus('FromDateHistory'); else HRM.focus('CmbAssetItem'); });   // tabControl1_SelectedIndexChanged
    function guard(fn) { return function (e) { if (!FA.dialogOpen()) fn(e); }; }
    HRM.keys({                                                                          // frmAssetsRegister_KeyDown
        'ctrl+e': guard(HRM.close), 'esc': guard(HRM.close),
        'ctrl+alt+control': guard(MakeShortCutKeys), 'ctrl+alt+alt': guard(MakeShortCutKeys),
        'ctrl+t': guard(function () {
            if (FA.activeTab() === 'history') { FA.showTab('form'); HRM.focus('CmbAssetItem'); }
            else { FA.showTab('history'); HRM.focus('FromDateHistory'); }
        }),
        'ctrl+n': guard(function () { if (FA.activeTab() === 'form') P.btnNew(); else P.BtnNewHistory(); }),
        'ctrl+s': guard(function () {
            if (FA.activeTab() === 'form') { if (HRM.visible('btnSave') && !HRM.$('btnSave').disabled) P.btnSave(); }
            else P.btnShow();
        }),
        'ctrl+r': guard(function () { if (FA.activeTab() === 'form') P.BtnRefresh(); else P.btnRefreshHistory(); }),
        'ctrl+u': guard(function () { if (FA.activeTab() === 'form' && HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled) P.btnUpdate(); }),
        'ctrl+f5': guard(function () { HRM.focus(FA.activeTab() === 'form' ? 'txtBrandName' : 'FromDateHistory'); }),
        'ctrl+arrowdown': guard(function () { if (FA.activeTab() === 'form') HRM.focus('btnImageBrowse1'); else { var r = HRM.$('DatagridHistory').querySelector('tbody tr[data-i]'); if (r) r.focus(); } }),
        'ctrl+arrowup': guard(function () { HRM.focus(FA.activeTab() === 'form' ? 'CmbAssetItem' : 'FromDateHistory'); }),
        'ctrl+enter': guard(function () {                                                 // DatagridHistory_KeyDown Ctrl+Enter
            var r = grid.current();
            if (r && document.activeElement && document.activeElement.closest('#DatagridHistory')) { FormReset(); ReadById(HRM.int(HRM.col(r, 'Id'))); }
        })
    });
    HRM.footer(function () { FA.showTab('history'); HRM.focus('FromDateHistory'); });  // History = the form's History tab

    // ------------------------------------------------------------------------ load
    HRM.setVal('txtPurchaseDate', HRM.today());
    HRM.setVal('txtExpiryDate', HRM.today());
    HRM.setVal('FromDateHistory', HRM.today());
    HRM.setVal('ToDateHistory', HRM.today());
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {                            // frmAssetsRegister_Load
        L = d || {};
        rights = L.rights || {};
        HRM.applyRights(rights, { save: 'btnSave', update: 'btnUpdate' });
        bindAll();
        AssetItemFill([]);
        buttons(false);
        loaded = true;
        HRM.focus('CmbAssetCategory');
    }).catch(HRM.fail);
})();
