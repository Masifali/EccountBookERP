/* ============================================================================================
 * countx_fixedassets_define.js - 356 "Fixed Asset Items" (Architecture.WinApp.FixedAsset.frmDefineAssets).
 * Built on countx_hrm.js (window.HRM) + countx_fixedassets_common.js (window.FA); exports window.FaDef.
 *
 * The API is the existing complete port of this form (StoreDefineAssetsExtraController, which the Store
 * screens' "+" dialog already uses): /api/store/define/fixed-asset-item/{lookups|categories|category-leave|
 * history|{id}|save}. Rights there are the frmDefineAssets rows (View to read, Save / Update to write);
 * validation texts and save behaviour: StoreDefineAssetsExtraService notes 11-18.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/store/define/fixed-asset-item';
    var P = {};
    window.FaDef = P;

    var RecId = 0, rights = { save: false, update: false }, loaded = false;
    var ACC_COLS = ['AccountCode', 'ParentAccountTitle', 'AccountClass'];

    // ------------------------------------------------------------------------ grids
    var galloc = new HRM.Grid('grdAllocation', {                                   // CompaniesBindInGrid():443
        columns: [
            { key: 'Id', hidden: true },
            { key: 'Location', caption: 'Location', width: 250 },
            { key: 'Value', caption: 'Value', type: 'edit-check', width: 60 }
        ],
        checkAll: 'Value'
    });
    var grdhistory = new HRM.Grid('grdhistory', {                                  // grdfrmfill():925 + gridsetting():980
        columns: [
            { key: 'Id', hidden: true },
            { key: 'AssetCode', hidden: true },
            { key: 'AssetCodeNew', caption: 'Asset Code' },
            { key: 'AssetName', caption: 'AssetName', width: 220 },
            { key: 'ItemType', caption: 'ItemType' },
            { key: 'ItemCategory', caption: 'ItemCategory' },
            { key: 'ParentCategory', caption: 'ParentCategory' },
            { key: 'ClassGroupName', caption: 'ClassGroupName' },
            { key: 'ItemStatus', caption: 'ItemStatus' },
            { key: 'Assets_GL', caption: 'Assets_GL' },
            { key: 'Accumulated_Depreciation', caption: 'Accumulated_Depreciation' },
            { key: 'Depreciation_Expense', caption: 'Depreciation_Expense' },
            { key: 'EntryDate', caption: 'EntryDate', type: 'date' },
            { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' },
            { key: 'LeadTimeDay', caption: 'LeadTimeDay', type: 'int', width: 60 }
        ],
        filterRow: true,
        onDouble: function (r) { var id = HRM.int(HRM.col(r, 'Id')); refresh().then(function () { ReadById(id); }); }   // grdhistory_DoubleClick
    });

    // ------------------------------------------------------------------------ binds
    function bindAll(l) {
        HRM.fill('cmbItemCategory', l.categories, 'Id', 'CategoryDescription', { keep: true });   // ItemCatagoryHistoryFill():308
        HRM.fill('cmbItemType', l.itemTypes, 'Id', 'TypeDescription', { keep: true });            // ItemTypeFill():373
        FA.fillCols('cmbAccumolativeACC', l.assetAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });   // AssetAccountBind():270
        FA.fillCols('CmbAssetGL', l.assetAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });
        FA.fillCols('CmbExpenseMaintenanceAccount', l.expenseAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });   // ExpenseAccountFill():289
        FA.fillCols('cmbDepriGL', l.expenseAccounts, 'Id', 'AccountTitle', ACC_COLS, { keep: true });
        HRM.fill('cmbAssetsClass', l.classes, 'ClassId', 'ClassDescription', { keep: true });     // ItemClassFill():488
    }
    function saveVisibleEnabled() { return HRM.visible('btnsave') && !HRM.$('btnsave').disabled; }
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
    function pics(p1, p2) {
        HRM.text('picbox1', p1 ? p1 : 'No picture');
        HRM.text('picbox2', p2 ? p2 : 'No picture');
    }

    /** cmbItemCategory_Leave():402 - only while Save is visible and enabled; any failure -> "Record Not Found". */
    function cmbItemCategory_Leave() {
        if (!saveVisibleEnabled()) return Promise.resolve();
        return HRM.get(API + '/category-leave', { categoryId: HRM.comboVal('cmbItemCategory') }).then(function (r) {
            r = r || {};
            if (r.ItemCodeNew !== undefined) HRM.setVal('txtItemCodeNew', HRM.str(r.ItemCodeNew));
            if (r.CapitalWipAcId !== undefined) {
                HRM.setCombo('cmbAccumolativeACC', HRM.int(r.accumulatedDepreciationAcId));
                HRM.setCombo('cmbDepriGL', HRM.int(r.DepreciationExpenseAcId));
                HRM.setCombo('CmbAssetGL', HRM.int(r.CapitalWipAcId));
                HRM.setCombo('CmbExpenseMaintenanceAccount', HRM.int(r.ExpenseMaintenanceAccountId));
            }
        }).catch(function () { HRM.box('Record Not Found'); });
    }

    /** refresh():825 */
    function refresh() {
        HRM.enable('cmbItemCategory', true);                                         // cmbItemCategory.ReadOnly = false
        RecId = 0;
        buttons(false);
        HRM.setVal('txtBarcodeNo', '');
        HRM.setVal('txtItemName', '');
        HRM.focus('cmbItemCategory');
        pics('', '');
        return cmbItemCategory_Leave();
    }

    /** ReadById(Id):740 */
    function ReadById(id) {
        return HRM.loading(HRM.get(API + '/' + id)).then(function (it) {
            RecId = id;
            buttons(true);
            FA.showTab('form');
            HRM.setVal('txtItemCodeNew', HRM.str(it.ItemCodeNew));
            HRM.setVal('txtBarcodeNo', HRM.str(it.BarcodeNo));
            HRM.setVal('txtItemName', HRM.str(it.ItemName));
            HRM.setCombo('cmbBaseUnit', HRM.int(it.BaseUnitId));
            HRM.setCombo('cmbItemCategory', HRM.int(it.ItemCategoryId));
            HRM.enable('cmbItemCategory', false);                                     // cmbItemCategory.ReadOnly = true
            HRM.setCombo('cmbItemType', HRM.int(it.ItemTypeId));
            HRM.check('chkstatus', !!it.ItemStatus);
            HRM.setCombo('CmbAssetGL', HRM.int(it.CapitalWipAcId));
            HRM.setCombo('cmbDepriGL', HRM.int(it.DepreciationExpenseAcId));
            HRM.setCombo('cmbAccumolativeACC', HRM.int(it.accumulatedDepreciationAcId));
            if (HRM.int(it.ExpenseMaintenanceAccountId) > 0) HRM.setCombo('CmbExpenseMaintenanceAccount', HRM.int(it.ExpenseMaintenanceAccountId));
            HRM.setCombo('cmbAssetsClass', HRM.int(it.ItemClassId));
            HRM.setVal('txtLeadTimeDay', HRM.str(it.LeadTimeDay));
            pics(HRM.str(it.Pic1), HRM.str(it.Pic2));                                  // the radios are not restored (desktop)
        }).catch(HRM.fail);
    }

    /** FormValidationForAddItem():521 */
    function validate() {
        function fail(m, id) { HRM.box(m); HRM.focus(id); return false; }
        if (HRM.val('txtItemName').trim() === '') return fail('Item Name Field is Required', 'txtItemName');
        if (!HRM.comboVal('cmbBaseUnit')) return fail('Base Unit Field is Required', 'cmbBaseUnit');
        if (!HRM.comboVal('cmbItemCategory')) return fail('Item Category Field is Required', 'cmbItemCategory');
        if (!HRM.comboVal('cmbItemType')) return fail('Item Type Field is Required', 'cmbItemType');
        if (!HRM.comboVal('cmbAssetsClass')) return fail('Item Class Field is Required', 'cmbAssetsClass');
        if (!HRM.comboVal('CmbAssetGL')) return fail('Asset GL A/c Field is Required', 'CmbAssetGL');
        if (!HRM.comboVal('cmbDepriGL')) return fail('Depreciation_Expense A/c Field is Required', 'cmbDepriGL');
        if (!HRM.comboVal('cmbAccumolativeACC')) return fail('Accumulated_Depreciation A/c Filed is Required', 'cmbAccumolativeACC');
        if (!HRM.comboVal('CmbExpenseMaintenanceAccount')) return fail('Expense Maintenance A/c Filed is Required', 'CmbExpenseMaintenanceAccount');
        return true;
    }
    /** Insert():580 */
    function Insert(btn) {
        if (!validate()) return;
        if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            Id: RecId, ItemCategoryId: HRM.comboVal('cmbItemCategory'), ItemTypeId: HRM.comboVal('cmbItemType'),
            ItemName: HRM.val('txtItemName'), BaseUnitId: HRM.comboVal('cmbBaseUnit'), BarcodeNo: HRM.val('txtBarcodeNo'),
            ItemClassId: HRM.comboVal('cmbAssetsClass'), CapitalWipAcId: HRM.comboVal('CmbAssetGL'),
            accumulatedDepreciationAcId: HRM.comboVal('cmbAccumolativeACC'), DepreciationExpenseAcId: HRM.comboVal('cmbDepriGL'),
            ExpenseMaintenanceAccountId: HRM.comboVal('CmbExpenseMaintenanceAccount'), LeadTimeDay: HRM.val('txtLeadTimeDay'),
            ItemStatus: HRM.checked('chkstatus'), Local: HRM.checked('rdLocal'),
            Allocations: galloc.rows().map(function (r) { return { Id: HRM.int(r.Id), Value: HRM.bool(r.Value) }; })
        };
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (r) {
                HRM.box(r && r.message ? r.message : '');
                return refresh();
            }).catch(HRM.fail);
        }, 'def-save');
    }

    /** grdfrmfill(NoOfRecords):925 - no rows -> the grid structure is cleared. */
    function grdfrmfill(n, btn) {
        return HRM.busy(btn, function () {
            return HRM.get(API + '/history', { noOfRecords: n || 0 }).then(function (rows) { grdhistory.set(rows || []); }).catch(HRM.fail);
        }, 'def-history');
    }

    // ------------------------------------------------------------------------ toolbar
    P.btnnew = function () {                                                        // btnnew_Click:857
        var p = refresh();
        HRM.setCombo('cmbBaseUnit', 0);
        HRM.setCombo('cmbItemType', 0);
        return p;
    };
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };      // BtnSave_Click:711
    P.btnupdate = function (btn) {                                                  // btnupdate_Click:724
        if (RecId === 0) { HRM.box('Record Not Update because RecId Not Found'); return; }
        return Insert(btn || 'btnupdate');
    };
    P.BtnRefresh = function (btn) {                                                 // BtnRefresh_Click:871
        return HRM.busy(btn || 'BtnRefresh', function () {
            return HRM.get(API + '/lookups').then(function (l) { bindAll(l || {}); }).catch(HRM.fail);
        });
    };
    P.btnLoadAll = function (btn) { return grdfrmfill(0, btn || 'BtnLoadAllT'); };   // btnLoadAll_Click:1025
    /** btnitmcat_Click:1139 - frmItemCatagoryStore { FormTypeId = 3 }.Show(); the category list is re-read on close
        (web: the desktop never re-reads, the dialog is modal here). */
    P.btnitmcat = function () {
        FA.storeDialog('openAssetCategory', function () {
            HRM.get(API + '/categories').then(function (list) { HRM.fill('cmbItemCategory', list, 'Id', 'CategoryDescription', { keep: true }); }).catch(HRM.fail);
        });
    };
    P.BtnShortCutkeys = function () { MakeShortCutKeys(); };
    function MakeShortCutKeys() {                                                   // MakeShortCutKeys():1362
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on ItemName'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Company grid '], ['Ctrl+ArrowUp', 'For Focus on ItemName'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', 'When Focus On Any Grid To Call Function\'s On Button Or Link']];
        var m = HRM.modal({ title: 'ShortCut Keys', width: 'min(600px, 96vw)', html: '<div class="hrm-grid-wrap"><table class="fa-keys"></table></div>' });
        new HRM.Grid(m.body.querySelector('.fa-keys'), { columns: [{ key: 'k', caption: 'KeyCombination' }, { key: 'd', caption: 'Description' }] })
            .set(rows.map(function (r) { return { k: r[0], d: r[1] }; }));
    }

    // ------------------------------------------------------------------------ events
    FA.onLeave('cmbItemCategory', function () { if (loaded) cmbItemCategory_Leave(); });
    FA.tabs(function (t) {                                                           // tabCAddItem_SelectedIndexChanged:1017
        if (t === 'history') grdfrmfill(50);
    });
    function guard(fn) { return function (e) { if (!FA.dialogOpen()) fn(e); }; }
    function focusRow(tableId) { var r = HRM.$(tableId).querySelector('tbody tr[data-i]'); if (r) r.focus(); }
    HRM.keys({                                                                       // InvDefrmAddItem_KeyDown:1248
        'ctrl+e': guard(HRM.close), 'esc': guard(HRM.close),
        'ctrl+alt+control': guard(MakeShortCutKeys), 'ctrl+alt+alt': guard(MakeShortCutKeys),
        'ctrl+t': guard(function () {
            if (FA.activeTab() === 'history') { FA.showTab('form'); HRM.focus('txtItemName'); }
            else { FA.showTab('history'); grdfrmfill(50); focusRow('grdhistory'); }
        }),
        'ctrl+n': guard(function () { if (FA.activeTab() === 'form') P.btnnew(); else P.btnLoadAll(); }),
        'ctrl+l': guard(function () { if (FA.activeTab() === 'history') P.btnLoadAll(); }),
        'ctrl+s': guard(function () { if (FA.activeTab() === 'form' && saveVisibleEnabled()) P.btnsave(); }),
        'ctrl+r': guard(function () { if (FA.activeTab() === 'form') P.BtnRefresh(); }),
        'ctrl+u': guard(function () { if (FA.activeTab() === 'form' && HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); }),
        'ctrl+f5': guard(function () { if (FA.activeTab() === 'form') HRM.focus('txtItemName'); }),
        'ctrl+arrowdown': guard(function () { focusRow(FA.activeTab() === 'form' ? 'grdAllocation' : 'grdhistory'); }),
        'ctrl+arrowup': guard(function () { if (FA.activeTab() === 'form') HRM.focus('txtItemName'); }),
        'ctrl+enter': guard(function () {                                              // grdhistory_KeyDown Ctrl+Enter
            var r = grdhistory.current();
            if (r && document.activeElement && document.activeElement.closest('#grdhistory')) grdhistory.opts.onDouble(r);
        })
    });
    HRM.footer(function () { FA.showTab('history'); grdfrmfill(50); });              // History = the form's History tab

    // ------------------------------------------------------------------------ load
    HRM.loading(HRM.get(API + '/lookups')).then(function (l) {                        // InvDefrmAddItem_Load:243
        l = l || {};
        rights = l.rights || rights;
        if (!rights.save) { HRM.enable('btnsave', false); HRM.$('btnsave').title = 'You do not have the Save right for Fixed Asset Items'; }
        if (!rights.update) { HRM.enable('btnupdate', false); HRM.$('btnupdate').title = 'You do not have the Update right for Fixed Asset Items'; }
        bindAll(l);
        galloc.set(l.companies || []);                                                   // CompaniesBindInGrid():443
        FA.fillCols('cmbBaseUnit', l.uoms, 'Id', 'UomCode', ['Equivalent'], {});           // BaseUnitFill():341
        buttons(false);
        loaded = true;
        var id = HRM.int(HRM.param('id'));
        if (id > 0) ReadById(id);
    }).catch(HRM.fail);
})();
