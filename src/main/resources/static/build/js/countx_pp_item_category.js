/* ============================================================================================
 * countx_pp_item_category.js - 685 Item Category (Party Processing)
 * Architecture.WinApp.PartyProcessing.DefItemCatagoryPartyProcessing. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/item-category/{setup|list|by-id|save}
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/item-category';
    var P = {};
    window.PpItemCategory = P;
    var RecId = 0;

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    var grd = new HRM.Grid('grdfrm', {
        columns: [
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'CategoryCode', caption: 'CategoryCode', width: 130 },
            { key: 'CategoryDescription', caption: 'CategoryDescription', width: 260 },
            { key: 'SerialFrom', caption: 'SerialFrom', width: 100 },
            { key: 'SerialTo', caption: 'SerialTo', width: 100 },
            { key: 'CategoryStatus', caption: 'CategoryStatus', width: 110 }
        ],
        filterRow: true,
        onDouble: function (r) { grdfrm_DoubleClick(r); }
    });

    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }

    /** gridfill(): the grid is rebound only when rows came back. */
    function gridfill() {
        return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
    }

    /** Reset(): the four texts and the parent category cleared (Is Active keeps its tick), Save shown, gridfill. */
    function Reset() {
        RecId = 0;
        ['txtItemCategoryCode', 'txtItemCategoryDescription', 'txtItemCategorySerialFrom', 'txtItemCategorySerialTo'].forEach(function (id) { HRM.setVal(id, ''); });
        HRM.setCombo('CmbItemParentCategory', null);
        buttons(false);
        HRM.focus('txtItemCategoryCode');
        return gridfill();
    }

    /** grdfrm_DoubleClick: GetByID into the fields; Save hidden, Update shown. */
    function grdfrm_DoubleClick(r) {
        var id = HRM.int(r.Id);
        HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            RecId = id;
            HRM.setVal('txtItemCategoryCode', HRM.str(o.CategoryCode));
            HRM.setVal('txtItemCategoryDescription', HRM.str(o.CategoryDescription));
            HRM.setVal('txtItemCategorySerialFrom', HRM.str(o.SerialFrom));
            HRM.setVal('txtItemCategorySerialTo', HRM.str(o.SerialTo));
            HRM.setCombo('CmbItemParentCategory', o.ParentCategoriesId);
            HRM.check('chkstatus', HRM.bool(o.CategoryStatus));
            buttons(true);
        }).catch(HRM.fail);
    }

    /** formvalidation(): order and wording of the form (a text of "0" counts as empty). */
    function formvalidation() {
        var code = HRM.val('txtItemCategoryCode');
        if (code.trim() === '' || code === '0') { HRM.box('Please Insert Code'); HRM.focus('txtItemCategoryCode'); return false; }
        var d = HRM.val('txtItemCategoryDescription').trim();
        if (d === '' || d === '0') { HRM.box('Please Insert Description'); HRM.focus('txtItemCategoryDescription'); return false; }
        var f = HRM.val('txtItemCategorySerialFrom').trim();
        if (f === '' || f === '0') { HRM.box('Please Insert Serial From'); HRM.focus('txtItemCategorySerialFrom'); return false; }
        var t = HRM.val('txtItemCategorySerialTo').trim();
        if (t === '' || t === '0') { HRM.box('Please Insert Serial To'); HRM.focus('txtItemCategorySerialTo'); return false; }
        if (HRM.comboVal('CmbItemParentCategory') === 0) { HRM.box('Please Select Parent Category'); HRM.focus('CmbItemParentCategory'); return false; }
        return true;
    }

    function Insert(btn) {
        if (!formvalidation()) return;
        if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, code: HRM.val('txtItemCategoryCode'), description: HRM.val('txtItemCategoryDescription'),
                serialFrom: HRM.val('txtItemCategorySerialFrom'), serialTo: HRM.val('txtItemCategorySerialTo'),
                parentId: HRM.comboVal('CmbItemParentCategory'), status: HRM.checked('chkstatus')
            }).then(function (d) { HRM.box(d.message); return Reset(); }).catch(HRM.fail);
        }, 'pp-item-category-save');
    }

    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnupdate'); };
    P.btnnew = function () { Reset(); };

    HRM.keys({                                                  // InvDeffrmItemCatagory_KeyDown
        'ctrl+n': function () { P.btnnew(); },
        'ctrl+s': function () { if (HRM.visible('btnsave')) P.btnsave(); },
        'ctrl+u': function () { if (HRM.visible('btnupdate')) P.btnUpdate(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        enterTab: false
    });
    HRM.footer(function (b) {
        return HRM.busy(b, function () {
            return gridfill().then(function () { var x = document.getElementById('historyBox'); if (x) x.scrollIntoView({ block: 'start', behavior: 'smooth' }); });
        });
    });

    /** InvDeffrmItemCatagory_Load: gridfill(), ItemParentCategoryFill() (first row active), Update hidden. */
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        if (d.rows && d.rows.length) grd.set(d.rows);
        HRM.fill('CmbItemParentCategory', d.parents || [], 'Id', 'InvParentCateDescription', { zero: '' });
        if (d.parents && d.parents.length) HRM.setCombo('CmbItemParentCategory', d.parents[0].Id);
        buttons(false);
        HRM.focus('txtItemCategoryCode');
    }).catch(HRM.fail);
})();
