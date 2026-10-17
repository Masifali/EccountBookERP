/* CustomerDiscountPolicy (Architecture.WinApp/CustomerDiscountPolicy.cs) - ScreenDefinition 96 "Customer Discount Policy".
   Line refs ":NNN" are that file. API: /inventory/customer-discount-policy/api.
     InvfrmPriceSchedule_Load :436   rights, grid structure, GridFill, BtnEdit hidden, focus date, DiscountTypeFill, CustomerBind
     DiscountTypeFill :263           BindDDLNew(..., false) → no default row
     CustomerBind :287               BindDDL(ZeroIndex: false) → no default row
     cmbDiscountType_Leave :337      FillDiscountItems → cmbDiscItem (ZeroIndex: false; empty → cleared)
     cmbDiscItem_TextChanged :342    txtDiscountPercentage = item's DiscPerncentage (stays disabled)
     Insert :149 / ReadById :233 / reset :491 / BtnNew_Click :476 */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4b, $id = SC.$id, API = '/inventory/customer-discount-policy/api';
    var rId = 0, rights = {}, items = [];

    /* GrdSetting :389 — Id, CustomerId, Item% hidden; EntryDate / ModifyDate "dd-MMM-yyyy hh:mm tt"; Edit at 0 */
    var grid = new X.Grid('grd', [
        { key: 'Edit', cap: 'Edit', btn: 'Edit', width: 40 },
        { key: 'Id', hidden: true },
        { key: 'EffectedDate', cap: 'EffectedDate', fmt: function (v) { return X.dmy(v); } },
        { key: 'CustomerId', hidden: true },
        { key: 'CustomerName', cap: 'CustomerName' },
        { key: 'DiscountType', cap: 'DiscountType' },
        { key: 'DiscountItem', cap: 'DiscountItem' },
        { key: 'DiscountPct', cap: 'Discount%', n: true, fmt: function (v) { return X.fmt(v, 3); } },
        { key: 'ItemPct', cap: 'Item%', hidden: true },
        { key: 'IsActive', cap: 'IsActive', check: true },
        { key: 'EntryUser', cap: 'EntryUser' },
        { key: 'EntryDate', cap: 'EntryDate', fmt: function (v) { return X.dmyHm(v); } },
        { key: 'ModifyUser', cap: 'ModifyUser' },
        { key: 'ModifyDate', cap: 'ModifyDate', fmt: function (v) { return X.dmyHm(v); } }
    ], { nav: 'lblNavigator', onEdit: function (r) { readById(r.Id); }, onDblClick: function (r) { readById(r.Id); } });

    function setMode(update) {
        $id('BtnSave').classList.toggle('is-hidden', update);
        $id('BtnEdit').classList.toggle('is-hidden', !update);
    }

    /* DiscountTypeFill :263 + CustomerBind :287 — only rebound when rows exist */
    function bindLookups(d) {
        if (d.discountTypes && d.discountTypes.length) X.fillSelect('cmbDiscountType', d.discountTypes, null);
        if (d.customers && d.customers.length) X.fillSelect('cmbCustomer', d.customers, null);
    }
    function lookups() { return SC.getJson(API + '/lookups').then(bindLookups).catch(function (e) { alert(e.message); }); }

    /* FillDiscountItems :303 */
    function fillItems(typeId) {
        return SC.getJson(API + '/discount-items' + SC.qs({ discountTypeId: typeId })).then(function (rows) {
            items = rows || [];
            if (items.length) X.fillSelect('cmbDiscItem', items, null);
            else { $id('cmbDiscItem').innerHTML = ''; }                       // Text = "", DataSource = null (:327)
            itemChanged();
        }).catch(function (e) { alert(e.message); });
    }
    /* cmbDiscItem_TextChanged :342 / cmbDiscItem_Leave :584 */
    function itemChanged() {
        var id = SC.num($id('cmbDiscItem').value);
        var it = items.filter(function (r) { return r.Id === id; })[0];
        if (it) { $id('txtDiscountPercentage').value = String(it.Percentage); $id('txtDiscountPercentage').disabled = true; }
    }

    /* GridFill :358 — no rows → grid left as it was (:371) */
    function gridFill() { return SC.getJson(API + '/history').then(function (rows) { if (rows && rows.length) grid.set(rows); }).catch(function (e) { alert(e.message); }); }

    function body() {
        return { Id: rId, CustomerId: SC.num($id('cmbCustomer').value), EffectiveDate: $id('dtpEffectedDate').value,
            DiscountTypeId: SC.num($id('cmbDiscountType').value), DiscountItemId: SC.num($id('cmbDiscItem').value),
            DiscountRate: $id('txtDiscountPercentage').value, IsActive: $id('chkIsActive').checked };
    }
    /* Insert :149 → GridFill(); reset() */
    function insert() { X.insert(API, body(), function () { gridFill(); reset(); }); }
    function BtnSave_Click() { rId = 0; insert(); }                 // :136
    function BtnEdit_Click() {                                      // :217
        if (rId === 0) { alert('Record not update because Id not found'); return; }
        insert();
    }

    /* reset :491 — combo texts cleared, rate cleared, Save mode, focus date, IsActive checked */
    function reset() {
        rId = 0;
        ['cmbCustomer', 'cmbDiscountType', 'cmbDiscItem'].forEach(function (id) { $id(id).selectedIndex = -1; });
        $id('txtDiscountPercentage').value = '';
        setMode(false);
        $id('dtpEffectedDate').focus();
        $id('chkIsActive').checked = true;
    }
    /* BtnNew_Click :476 */
    function BtnNew_Click() { reset(); gridFill(); lookups(); }

    /* ReadById :233 */
    function readById(id) {
        SC.getJson(API + '/' + id).then(function (d) {
            rId = d.Id;
            $id('cmbCustomer').value = String(d.CustomerId);
            $id('dtpEffectedDate').value = d.EffectiveDate || SC.today();
            $id('cmbDiscountType').value = String(d.DiscountTypeId);
            fillItems(SC.num($id('cmbDiscountType').value)).then(function () {      // cmbDiscountType_Leave :242
                $id('txtDiscountPercentage').value = String(d.DiscountRate);          // :243
                $id('cmbDiscItem').value = String(d.DiscountItemId);                  // :244 (TextChanged overwrites the rate)
                itemChanged();
                $id('chkIsActive').checked = !!d.IsActive;
                setMode(true);
                $id('dtpEffectedDate').focus();
            });
        }).catch(function (e) { alert(e.message); });
    }

    /* InvfrmPriceSchedule_Load :436 */
    function load() {
        $id('dtpEffectedDate').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            rights = d.rights || {};
            $id('BtnSave').disabled = !rights.save;
            $id('BtnEdit').disabled = !rights.update;
            if (d.history && d.history.length) grid.set(d.history);
            setMode(false);
            $id('dtpEffectedDate').focus();
            bindLookups(d);
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('BtnNew').addEventListener('click', BtnNew_Click);
        $id('BtnSave').addEventListener('click', BtnSave_Click);
        $id('BtnEdit').addEventListener('click', BtnEdit_Click);
        $id('btnAllocateBrandToType').addEventListener('click', function () { alert('InvAllocateBrandItemToDiscType is not ported to the web yet.'); });
        $id('btnAllocateDiscountItemToType').addEventListener('click', function () { alert('InvAllocateDiscItemToDiscType is not ported to the web yet.'); });
        $id('cmbDiscountType').addEventListener('change', function () { fillItems(SC.num($id('cmbDiscountType').value)); });   // Leave :337
        $id('cmbDiscItem').addEventListener('change', itemChanged);
        X.decimalOnly($id('txtDiscountPercentage'));                                     // txtDiscountRate_KeyPress :555
        document.addEventListener('keydown', function (e) {                              // InvfrmPriceSchedule_KeyDown :510
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); BtnNew_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (!$id('BtnSave').classList.contains('is-hidden') && !$id('BtnSave').disabled) BtnSave_Click(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!$id('BtnEdit').classList.contains('is-hidden') && !$id('BtnEdit').disabled) BtnEdit_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
        });
        load();
    });
})();
