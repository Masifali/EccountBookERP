/* ============================================================================================
 * countx_pp_define_item.js - 683 Add Item (Party Processing)
 * Architecture.WinApp.PartyProcessing.DefineItemPartyProcessing. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/define-item/{setup|refresh|allocation|history|code|by-id|save}
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/define-item';
    var P = {};
    window.PpDefineItem = P;
    var RecId = 0, rights = {}, tab = 0;

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    /* The forms the bottom buttons open (Form.Show()). A form without a web page says so. */
    var FORMS = {
        'item-category': '/party-processing/item-category',                 // DefItemCatagoryPartyProcessing
        'item-type': '/party-processing/item-type',                         // DefPartyProcessingItemType
        'uom-schedule': '/inventory/item-uom-schedule',                     // InvDeffrmItemUomSchedule
        'crop-year': '/inventory/crop-years',                               // DefineCropYear
        'item-group-schedule': '/inventory/item-group-schedules',           // DefineItemGroupUOMSchedule
        'uom': null,                                                        // InvDeffrmItemItemUom
        'item-group': null,                                                 // DefineItemGroup
        'job-lots': null,                                                   // AcfrmDefineJobLotPartyProcessing
        'warehouse': null                                                   // InvDeffrmWarehousePartyProcessing
    };
    P.open = function (k) {
        if (FORMS[k]) HRM.open(FORMS[k]);
        else HRM.box('This form is not available on the web yet.');
    };

    // ------------------------------------------------------------------ tabs (tabCAddItem)
    function selectTab(i) {
        tab = i;
        var btns = document.querySelectorAll('[data-tabs="tabCAddItem"] .hrm-tab');
        var pages = ['tpItemDefinitionAddItem', 'tpItemDefinitionAddItemHistory'];
        btns.forEach(function (b, x) { b.classList.toggle('is-active', x === i); });
        pages.forEach(function (p, x) { document.getElementById(p).classList.toggle('is-active', x === i); });
        if (i === 1) grdfrmfill(50);                                          // tabCAddItem_SelectedIndexChanged
    }
    document.querySelectorAll('[data-tabs="tabCAddItem"] .hrm-tab').forEach(function (b, x) {
        b.addEventListener('click', function () { if (tab !== x) selectTab(x); });
    });

    // ------------------------------------------------------------------ grids
    var grdAllocation = new HRM.Grid('grdAllocation', {
        columns: [
            { key: 'Id', hidden: true },
            { key: 'Location', caption: 'Location', width: 250 },
            { key: 'Value', caption: 'Value', type: 'edit-check', width: 60 }
        ],
        checkAll: 'Value'
    });

    var grdhistory = new HRM.Grid('grdhistory', {
        columns: [
            { key: 'RecordNo', hidden: true }, { key: 'Id', hidden: true },
            { key: 'ItemName', caption: 'ItemName', width: 240 },
            { key: 'ItemCode', caption: 'ItemCode', width: 100 },
            { key: 'ItemBaseUnitId', hidden: true }, { key: 'ItemCategoryId', hidden: true }, { key: 'ItemTypeId', hidden: true },
            { key: 'UomGroupId', hidden: true }, { key: 'EntryDate', hidden: true }, { key: 'EntryUser', hidden: true },
            { key: 'ModifyDate', hidden: true }, { key: 'ModifyUser', hidden: true }, { key: 'OrganizationId', hidden: true },
            { key: 'CompanyId', hidden: true }, { key: 'IsActive', hidden: true },
            { key: 'UOMCode', caption: 'UOMCode', width: 90 },
            { key: 'ItemGroupName', caption: 'ItemGroupName', width: 160 },
            { key: 'Category', caption: 'Category', width: 160 },
            { key: 'TypeName', caption: 'TypeName', width: 140 }
        ],
        filterRow: true,
        onDouble: function (r) { grdhistory_DoubleClick(r); }
    });

    /** grdfrmfill(NoOfRecords): rebinds only when rows came back. */
    function grdfrmfill(n) {
        return HRM.loading(HRM.get(API + '/history', { noOfRecords: n })).then(function (rows) {
            if (rows && rows.length) grdhistory.set(rows);
        }).catch(HRM.fail);
    }

    function combos(d) {
        HRM.fill('cmbItemType', d.types || [], 'Id', 'TypeDescription', { zero: '', keep: true });
        HRM.fill('cmbItemCategory', d.categories || [], 'Id', 'CategoryDescription', { zero: '', keep: true });
        HRM.fill('CmbItemGroup', d.groups || [], 'GroupId', 'ItemGroupName', { zero: '', keep: true });
    }

    /** cmbItemCategory_Leave: GenerateCode for the category -> txtItemCode (empty when no row). */
    document.getElementById('cmbItemCategory').addEventListener('change', function () {
        HRM.get(API + '/code', { categoryId: HRM.comboVal('cmbItemCategory') }).then(function (d) {
            HRM.setVal('txtItemCode', HRM.str(d.ItemCode));
        }).catch(HRM.fail);
    });

    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }

    /** refresh(): Save shown, Base Unit / Type / Code / Name / Category / Group cleared, RecId 0, CompaniesBindInGrid. */
    function refresh() {
        buttons(false);
        ['cmbBaseUnit', 'cmbItemType', 'cmbItemCategory', 'CmbItemGroup'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.setVal('txtItemCode', ''); HRM.setVal('txtItemName', '');
        HRM.focus('cmbItemCategory');
        RecId = 0;
        return HRM.get(API + '/allocation').then(function (rows) { grdAllocation.set(rows || []); }).catch(HRM.fail);
    }

    /**
     * grdhistory_DoubleClick: GetByID into the form, tab 0, Save hidden / Update shown. The form then activates
     * the Uom Group's first row, overwriting the item's own group (an update would store it); the web keeps the
     * item's group.
     */
    function grdhistory_DoubleClick(r) {
        var id = HRM.int(r.Id);
        HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            RecId = id;
            HRM.setVal('txtItemCode', HRM.str(o.ItemCode));
            HRM.setVal('txtItemName', HRM.str(o.ItemName));
            HRM.setCombo('cmbBaseUnit', o.ItemBaseUnitId);
            HRM.setCombo('CmbItemGroup', o.UomGroupId);
            HRM.setCombo('cmbItemCategory', o.ItemCategoryId);
            HRM.setCombo('cmbItemType', o.ItemTypeId);
            HRM.check('chkstatus', HRM.bool(o.IsActive));
            selectTab(0);
            buttons(true);
        }).catch(HRM.fail);
    }

    /** FormValidationForAddItem(): order and wording of the form. */
    function valid() {
        if (HRM.val('txtItemName').trim() === '') { HRM.box('Item Name Field is Required'); HRM.focus('txtItemName'); return false; }
        if (HRM.comboVal('cmbBaseUnit') === 0) { HRM.box('Base Unit Field is Required'); HRM.focus('cmbBaseUnit'); return false; }
        if (HRM.comboVal('cmbItemCategory') === 0) { HRM.box('Item Category Field is Required'); HRM.focus('cmbItemCategory'); return false; }
        if (HRM.comboVal('cmbItemType') === 0) { HRM.box('Item Type Field is Required'); HRM.focus('cmbItemType'); return false; }
        if (HRM.comboVal('CmbItemGroup') === 0) { HRM.box('Item Group Field is Required'); HRM.focus('CmbItemGroup'); return false; }
        return true;
    }

    /** Insert(): no confirmation on this form; the allocation rules run on the server with the form's messages. */
    function Insert(btn) {
        if (!valid()) return;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, itemCode: HRM.val('txtItemCode'), itemName: HRM.val('txtItemName'),
                baseUnitId: HRM.comboVal('cmbBaseUnit'), categoryId: HRM.comboVal('cmbItemCategory'),
                typeId: HRM.comboVal('cmbItemType'), groupId: HRM.comboVal('CmbItemGroup'), isActive: HRM.checked('chkstatus'),
                allocation: grdAllocation.rows().map(function (r) { return { Id: r.Id, Value: HRM.bool(r.Value) }; })
            }).then(function (d) { HRM.box(d.message); return refresh(); }).catch(HRM.fail);
        }, 'pp-define-item-save');
    }

    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };
    P.btnnew = function () { refresh(); };
    P.btnLoadAll = function (btn) { return HRM.busy(btn || 'btnLoadAll', function () { return grdfrmfill(0); }); };
    P.btnattachment = function () { HRM.box('Attachments are not available on the web page yet.'); };
    /** toolStripButton1_Click: ItemTypeFill, ItemCatagoryHistoryFill, ItemGroupNameFill. */
    P.refresh = function (btn) {
        return HRM.busy(btn || 'toolStripButton1', function () { return HRM.get(API + '/refresh').then(combos).catch(HRM.fail); });
    };

    HRM.keys({                                                  // InvDefrmAddItem_KeyDown (Enter -> {TAB})
        'ctrl+s': function () { if (HRM.visible('btnsave') && tab === 0 && !HRM.$('btnsave').disabled) P.btnsave(); },
        'ctrl+n': function () { P.btnnew(); },
        'ctrl+t': function () { selectTab(tab === 1 ? 0 : 1); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+u': function () { if (HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); }
    });
    HRM.footer(function () { selectTab(1); });

    /** InvDefrmAddItem_Load. */
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.enable('btnsave', rights.save !== false);
        HRM.enable('btnprint', rights.print !== false);
        HRM.enable('btnupdate', rights.update !== false);
        combos(d);
        HRM.fill('cmbBaseUnit', d.uoms || [], 'Id', 'UOMCode', { zero: '' });
        grdAllocation.set(d.allocation || []);
        buttons(false);
        HRM.focus('cmbItemCategory');
    }).catch(HRM.fail);
})();
