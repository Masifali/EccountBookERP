/* DiscountPolicyForParty (Architecture.WinApp/DiscountPolicyForParty.cs) - ScreenDefinition 98 "Discount Policy For Party".
   Line refs ":NNN" are that file. API: /inventory/discount-policy-for-party/api.
     InvfrmPriceSchedule_Load :279   rights, GridFill, BtnEdit hidden, focus date, DiscountTypeFill
     DiscountTypeFill :199           Percent / Weight / Flat (in-code), Rows[0] activated; the saved value is the TEXT (:154)
     Insert :128 / grd_DoubleClick :316 (GetByID) / reset :221 / BtnNew_Click :302 */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4b, $id = SC.$id, API = '/inventory/discount-policy-for-party/api';
    var rId = 0, rights = {}, types = [];

    /* GridFill :253-269 — Id is NOT hidden on this form; EffectedDate "dd-mmm-yyyy"; no Edit column, double-click opens */
    var grid = new X.Grid('grd', [
        { key: 'Id', cap: 'Id', n: true },
        { key: 'EffectedDate', cap: 'EffectedDate', fmt: function (v) { return X.dmy(v); } },
        { key: 'PolicyName', cap: 'PolicyName' },
        { key: 'DiscountType', cap: 'DiscountType' },
        { key: 'DiscountRate', cap: 'DiscountRate', n: true, fmt: function (v) { return X.fmt(v, 3); } },
        { key: 'IsActive', cap: 'IsActive', check: true }
    ], { nav: 'lblNavigator', onDblClick: function (r) { readById(r.Id); } });

    function setMode(update) {
        $id('BtnSave').classList.toggle('is-hidden', update);
        $id('BtnEdit').classList.toggle('is-hidden', !update);
    }

    /* DiscountTypeFill :199 — BindDDLNew(..., false): no default row; Rows[0].Activate() */
    function discountTypeFill() {
        var el = $id('cmbDiscountType');
        el.innerHTML = '';
        types.forEach(function (t) { el.appendChild(new Option(t, t)); });
        if (types.length) el.selectedIndex = 0;
    }

    /* GridFill :238 — no rows → grid left as it was (:251) */
    function gridFill() { return SC.getJson(API + '/history').then(function (rows) { if (rows && rows.length) grid.set(rows); }).catch(function (e) { alert(e.message); }); }

    function body() {
        return { Id: rId, PolicyName: $id('txtPolicyname').value.trim(), EffectiveDate: $id('dtpEffectedDate').value,
            DiscountType: X.selText('cmbDiscountType'), DiscountRate: $id('txtDiscountRate').value.trim(), IsActive: $id('chkIsActive').checked };
    }
    /* Insert :128 → GridFill(); reset() */
    function insert() { X.insert(API, body(), function () { gridFill(); reset(); }); }
    function BtnSave_Click() { rId = 0; insert(); }                 // :115
    function BtnEdit_Click() {                                      // :183
        if (rId === 0) { alert('Record not update because Id not found'); return; }
        insert();
    }

    /* reset :221 — texts cleared, Save mode, focus policy name, IsActive checked (date and type untouched) */
    function reset() {
        rId = 0;
        $id('txtPolicyname').value = '';
        $id('txtDiscountRate').value = '';
        setMode(false);
        $id('txtPolicyname').focus();
        $id('chkIsActive').checked = true;
    }
    /* BtnNew_Click :302 */
    function BtnNew_Click() { reset(); gridFill(); discountTypeFill(); }

    /* grd_DoubleClick :316 */
    function readById(id) {
        SC.getJson(API + '/' + id).then(function (d) {
            rId = d.Id;
            $id('txtPolicyname').value = d.PolicyName || '';
            $id('dtpEffectedDate').value = d.EffectiveDate || SC.today();
            var el = $id('cmbDiscountType');                                              // cmbDiscountType.Text = DiscountType (:324)
            el.value = d.DiscountType;
            if (el.value !== d.DiscountType) { el.appendChild(new Option(d.DiscountType, d.DiscountType)); el.value = d.DiscountType; }
            $id('txtDiscountRate').value = String(d.DiscountRate);
            $id('chkIsActive').checked = !!d.IsActive;
            setMode(true);
        }).catch(function (e) { alert(e.message); });
    }

    /* InvfrmPriceSchedule_Load :279 */
    function load() {
        $id('dtpEffectedDate').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            rights = d.rights || {};
            types = d.discountTypes || [];
            $id('BtnSave').disabled = !rights.save;
            $id('BtnEdit').disabled = !rights.update;
            if (d.history && d.history.length) grid.set(d.history);
            setMode(false);
            $id('dtpEffectedDate').focus();
            discountTypeFill();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('BtnNew').addEventListener('click', BtnNew_Click);
        $id('BtnSave').addEventListener('click', BtnSave_Click);
        $id('BtnEdit').addEventListener('click', BtnEdit_Click);
        X.decimalOnly($id('txtDiscountRate'));                                           // txtDiscountRate_KeyPress :388
        document.addEventListener('keydown', function (e) {                              // InvfrmPriceSchedule_KeyDown :343
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); BtnNew_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (!$id('BtnSave').classList.contains('is-hidden') && !$id('BtnSave').disabled) BtnSave_Click(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!$id('BtnEdit').classList.contains('is-hidden') && !$id('BtnEdit').disabled) BtnEdit_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
        });
        load();
    });
})();
