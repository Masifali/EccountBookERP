/* ============================================================================================
 * countx_pp_item_type.js - 686 Item Type (Party Processing)
 * Architecture.WinApp.PartyProcessing.DefPartyProcessingItemType. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/item-type/{setup|list|by-id|save}
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/item-type';
    var P = {};
    window.PpItemType = P;
    var RecId = 0;

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    var grd = new HRM.Grid('grdhistory', {
        columns: [
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'Code', caption: 'Code', width: 160 },
            { key: 'Description', caption: 'Description', width: 260 },
            { key: 'Type', caption: 'Type', width: 180 }
        ],
        filterRow: true,
        emptyText: '',
        onDouble: function (r) { grdfrm_DoubleClick(r); }
    });

    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }

    /** gridfill(): the grid is replaced only when rows came back. */
    function gridfill() {
        return HRM.get(API + '/list').then(function (rows) { if (rows && rows.length) grd.set(rows); }).catch(HRM.fail);
    }

    /** clear(): Save shown, Code / Description cleared (the Type stays), Update hidden, gridfill. */
    function clear() {
        RecId = 0;
        buttons(false);
        HRM.setVal('txtItemTypeCode', ''); HRM.setVal('txtItemTypeDescription', '');
        HRM.focus('txtItemTypeCode');
        return gridfill();
    }

    /** grdfrm_DoubleClick: GetByID -> Code, Description, Type; Save hidden, Update shown. */
    function grdfrm_DoubleClick(r) {
        var id = HRM.int(r.Id);
        HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            RecId = id;
            HRM.setVal('txtItemTypeCode', HRM.str(o.TypeCode));
            HRM.setVal('txtItemTypeDescription', HRM.str(o.TypeDescription));
            HRM.setCombo('cmbItemTypeType', o.LookUpId);
            buttons(true);
        }).catch(HRM.fail);
    }

    /** formvalidation(): the form's order and wording. */
    function formvalidation() {
        if (HRM.val('txtItemTypeCode').trim() === '') { HRM.box('Please Insert Code'); HRM.focus('txtItemTypeCode'); return false; }
        if (HRM.val('txtItemTypeDescription').trim() === '') { HRM.box('Please Insert Description'); HRM.focus('txtItemTypeDescription'); return false; }
        if (HRM.comboVal('cmbItemTypeType') === 0) { HRM.box('Please select type'); HRM.focus('cmbItemTypeType'); return false; }
        return true;
    }

    /** Insert(): validation, the Save / Update confirmation, ItemTypePartyProcessing.Save, message, clear(). */
    function Insert(btn) {
        if (!formvalidation()) return;
        if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, code: HRM.val('txtItemTypeCode'), description: HRM.val('txtItemTypeDescription'),
                typeId: HRM.comboVal('cmbItemTypeType')
            }).then(function (d) {
                HRM.box(d.message);
                return clear();
            }).catch(HRM.fail);
        }, 'pp-item-type-save');
    }

    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnupdate'); };
    P.btnnew = function () { clear(); };

    HRM.keys({                                                  // InvDeffrmItemType_KeyDown
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

    /** InvDeffrmItemType_Load: gridfill(), CombTypeFill() (first row active), focus Code, Update hidden. */
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        if (d.rows && d.rows.length) grd.set(d.rows);
        HRM.fill('cmbItemTypeType', d.types || [], 'Id', 'LookupName', { zero: false });
        buttons(false);
        HRM.focus('txtItemTypeCode');
    }).catch(HRM.fail);
})();
