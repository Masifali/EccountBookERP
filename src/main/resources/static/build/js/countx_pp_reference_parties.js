/* ============================================================================================
 * countx_pp_reference_parties.js - 689 Define LookUp Parties (Party Processing menu)
 * Architecture.WinApp.DefineReferenceParties. Built on countx_hrm.js (window.HRM).
 * API /api/party-processing/reference-parties/{setup|refresh|list|by-id|save}
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/reference-parties';
    var P = {};
    window.PpReference = P;
    var RecId = 0, UpdateMood = false;

    HRM.close = function () {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/party-processing'; } }, 150);
    };

    var grd = new HRM.Grid('grd', {
        columns: [
            { key: 'Id', hidden: true },
            { key: 'ReferencePartyTypeId', hidden: true },
            { key: 'ReferencePartyType', caption: 'ReferencePartyType', width: 150 },
            { key: 'ReferencePartyName', caption: 'ReferencePartyName', width: 220 },
            { key: 'IsActive', caption: 'IsActive', type: 'check', width: 70 },
            { key: 'OrganizationId', hidden: true },
            { key: 'CompanyId', hidden: true },
            { key: 'SupplierCustomerId', hidden: true },
            { key: 'SupplierCustomer', caption: 'SupplierCustomer', width: 220 }
        ],
        filterRow: true,
        onDouble: function (r) { grdcropyear_DoubleClick(r); }
    });

    /** bindGrid(): the rows, or an empty grid when there are none (ClearStructure). */
    function bindGrid() {
        return HRM.get(API + '/list').then(function (rows) { grd.set(rows || []); }).catch(HRM.fail);
    }

    /** SupplierNameFill() / TypeNameFill(): keep the picked value when it is still in the list, else empty. */
    function fillCombos(d) {
        HRM.fill('combsalesman', d.agents || [], 'Id', 'CompanyName', { zero: '', keep: true });
        HRM.fill('CmbReferencePartyType', d.types || [], 'ReferencePartyTypeId', 'ReferencePartyType', { zero: '', keep: true });
    }

    /** CmbReferencePartyType_Leave: type 3 enables the Agent, any other type clears and disables it. */
    function typeLeave() {
        if (HRM.comboVal('CmbReferencePartyType') === 3) { HRM.enable('combsalesman', true); return; }
        HRM.setCombo('combsalesman', 0);
        HRM.enable('combsalesman', false);
    }
    document.getElementById('CmbReferencePartyType').addEventListener('change', typeLeave);

    function grdcropyear_DoubleClick(r) {
        var id = HRM.int(r.Id);
        HRM.show('btnsave', false); HRM.show('btnUpdate', true);
        UpdateMood = true;
        HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            RecId = id;
            HRM.setVal('txtPartyName', HRM.str(o.ReferencePartyName));
            HRM.setCombo('CmbReferencePartyType', o.ReferencePartyTypeId);
            HRM.setCombo('combsalesman', o.SupplierCustomerId);
            HRM.check('chkIsActive', HRM.bool(o.IsActive));
        }).catch(HRM.fail);
    }

    function Insert(btn) {
        if (HRM.val('txtPartyName').trim() === '') { HRM.box('PartyName Field Required'); HRM.focus('txtPartyName'); return; }
        if (!HRM.ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, name: HRM.val('txtPartyName'), isActive: HRM.checked('chkIsActive'),
                typeId: HRM.comboVal('CmbReferencePartyType'), agentId: HRM.comboVal('combsalesman')
            }).then(function (d) { HRM.box(d.message); return formReFresh(); }).catch(HRM.fail);
        }, 'pp-reference-save');
    }

    /** formReFresh(): RecId 0, bindGrid, name cleared, Save shown / Update hidden (type, agent, Is Active and UpdateMood kept, as the form). */
    function formReFresh() {
        RecId = 0;
        HRM.setVal('txtPartyName', '');
        HRM.focus('txtPartyName');
        HRM.show('btnsave', true); HRM.show('btnUpdate', false);
        return bindGrid();
    }

    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnUpdate = function (btn) { return Insert(btn || 'btnUpdate'); };
    P.btnnew = function () { formReFresh(); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return HRM.get(API + '/refresh').then(fillCombos).catch(HRM.fail); });
    };

    HRM.keys({                                                  // DefineCropYear_KeyDown
        'ctrl+n': function () { P.btnnew(); },
        'ctrl+s': function () { if (!UpdateMood) P.btnsave(); },
        'ctrl+u': function () { if (UpdateMood) P.btnUpdate(); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        enterTab: false
    });
    HRM.footer(function (b) {
        return HRM.busy(b, function () {
            return bindGrid().then(function () { var x = document.getElementById('historyBox'); if (x) x.scrollIntoView({ block: 'start', behavior: 'smooth' }); });
        });
    });

    /** DefineCropYear_Load: feature 4, bindGrid, Update hidden, focus name, Is Active ticked, SupplierNameFill, TypeNameFill. */
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        grd.set(d.rows || []);
        fillCombos(d);
        HRM.check('chkIsActive', true);
        HRM.show('btnUpdate', false);
        HRM.focus('txtPartyName');
    }).catch(HRM.fail);
})();
