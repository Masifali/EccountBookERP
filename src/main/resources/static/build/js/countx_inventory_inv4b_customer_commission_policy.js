/* CustomerCommissionPolicy (WholeSale.Commission/CustomerCommissionPolicy.cs) - ScreenDefinition 95 "Customer Commission Policy".
   Line refs ":NNN" are that file. API: /inventory/customer-commission-policy/api.
     InvfrmPriceSchedule_Load :115   rights, CommissionTypeFill, CustomerBind, GridFill, BtnEdit hidden, dtpEffectedDate focused
     CommissionTypeFill :139         BindDDLNew(..., true) → default row "...Select Any Value..." + rows, selection kept
     CustomerBind :180               BindDDL(ZeroIndex: true) for cmbCustomer and CmbIncentiveAccount, selection kept
     cmbDiscountType_Leave :254      CommissionItemsByCommissionTypeId → CmbCommissionItem (ZeroIndex: true)
     cmbDiscItem_TextChanged :269    txtCommissionPercentage = item's CommissionPerncentage (stays disabled)
     Insert :343 / ReadById :414 / reset :592 / BtnRefresh_Click :625 */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4b, $id = SC.$id, API = '/inventory/customer-commission-policy/api';
    var DEFAULT_TEXT = '...Select Any Value...';
    var rId = 0, rights = {}, items = [];

    function pct(v) { return X.fmt(v, 3); }                        // FormatString "##,#.###"
    /* GrdSetting :507 — hidden Id, CustomerId, CommissionItemsId, CommissionTypeId, CommissionPerncentage; Edit at 0; sums on the rates */
    var grid = new X.Grid('grd', [
        { key: 'Edit', cap: 'Edit', btn: 'Edit', width: 40 },
        { key: 'Id', hidden: true }, { key: 'CustomerId', hidden: true },
        { key: 'CustomerName', cap: 'CustomerName', width: 250 },
        { key: 'EffectiveDate', cap: 'EffectiveDate', fmt: function (v) { return X.dmy(v); } },
        { key: 'CommissionType', cap: 'CommissionType' },
        { key: 'CommissionRate', cap: 'CommissionRate', n: true, sum: true, fmt: pct },
        { key: 'IsActive', cap: 'IsActive', check: true },
        { key: 'CommissionItemsId', hidden: true },
        { key: 'CommissionItemName', cap: 'CommissionItemName' },
        { key: 'CommissionTypeId', hidden: true },
        { key: 'CommissionTypeCode', cap: 'CommissionTypeCode' },
        { key: 'CommissionPerncentage', cap: 'CommissionPercentage', hidden: true, n: true, sum: true, fmt: pct },
        { key: 'IncentiveAccount', cap: 'IncentiveAccount', width: 250 },
        { key: 'EntryDate', cap: 'EntryDate', fmt: function (v) { return X.dmyHm(v); } },
        { key: 'EntryUserName', cap: 'EntryUserName' },
        { key: 'ModifyDate', cap: 'ModifyDate', fmt: function (v) { return X.dmyHm(v); } },
        { key: 'ModifyUserName', cap: 'ModifyUserName' }
    ], { nav: 'lblNavigator', onEdit: function (r) { readById(r.Id); }, onDblClick: function (r) { readById(r.Id); } });

    /* MakeShortCutKeys :685 */
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'],
        ['Ctrl+F5', 'For Focus on Discount Item Name'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Discount Item Name']];

    function setMode(update) {
        $id('BtnSave').classList.toggle('is-hidden', update);
        $id('BtnEdit').classList.toggle('is-hidden', !update);
    }

    /* CommissionTypeFill :139 + CustomerBind :180 (BtnRefresh_Click :625) — empty result clears the combo */
    function bindLookups(d) {
        X.fillSelect('CmbCommissionType', d.commissionTypes, d.commissionTypes && d.commissionTypes.length ? DEFAULT_TEXT : null);
        X.fillSelect('cmbCustomer', d.customers, d.customers && d.customers.length ? DEFAULT_TEXT : null);
        X.fillSelect('CmbIncentiveAccount', d.customers, d.customers && d.customers.length ? DEFAULT_TEXT : null);
    }
    function refresh() { return SC.getJson(API + '/lookups').then(bindLookups).catch(function (e) { alert(e.message); }); }

    /* CommissionItemsByCommissionTypeId :228 */
    function fillItems(typeId, thenSelect) {
        return SC.getJson(API + '/commission-items' + SC.qs({ commissionTypeId: typeId })).then(function (rows) {
            items = rows || [];
            X.fillSelect('CmbCommissionItem', items, items.length ? DEFAULT_TEXT : null);
            if (thenSelect !== undefined) $id('CmbCommissionItem').value = String(thenSelect);
            itemChanged();
        }).catch(function (e) { alert(e.message); });
    }
    /* cmbDiscountType_Leave :254 — only when a type is selected */
    function typeLeave() { var id = SC.num($id('CmbCommissionType').value); if (id > 0) return fillItems(id); return Promise.resolve(); }
    /* cmbDiscItem_TextChanged :269 / cmbDiscItem_Leave :285 */
    function itemChanged() {
        var id = SC.num($id('CmbCommissionItem').value);
        var it = items.filter(function (r) { return r.Id === id; })[0];
        if (it) { $id('txtCommissionPercentage').value = String(it.Percentage); $id('txtCommissionPercentage').disabled = true; }
    }

    /* GridFill :457 — no rows → grd.ClearStructure() */
    function gridFill() { return SC.getJson(API + '/history').then(function (rows) { grid.set(rows || []); }).catch(function (e) { alert(e.message); }); }

    function body() {
        return { Id: rId, CustomerId: SC.num($id('cmbCustomer').value), CustomerIncentivePartyId: SC.num($id('CmbIncentiveAccount').value),
            EffectiveDate: $id('dtpEffectedDate').value, CommissionTypeId: SC.num($id('CmbCommissionType').value),
            CommissionItemsId: SC.num($id('CmbCommissionItem').value), CommissionRate: $id('txtCommissionPercentage').value,
            IsActive: $id('chkIsActive').checked };
    }
    /* Insert :343 → reset() */
    function insert() { X.insert(API, body(), function () { reset(); }); }
    function BtnSave_Click() { rId = 0; insert(); }                 // :330
    function BtnEdit_Click() {                                      // :441
        if (rId === 0) { alert('Record not update because Id not found'); return; }
        insert();
    }

    /* reset :592 — texts cleared (no selection), Save mode, IsActive checked, GridFill, focus date */
    function reset() {
        rId = 0;
        ['cmbCustomer', 'CmbIncentiveAccount', 'CmbCommissionType', 'CmbCommissionItem'].forEach(function (id) { $id(id).value = '0'; });
        $id('txtCommissionPercentage').value = '';
        setMode(false);
        $id('chkIsActive').checked = true;
        gridFill();
        $id('dtpEffectedDate').focus();
    }

    /* ReadById :414 */
    function readById(id) {
        SC.getJson(API + '/' + id).then(function (d) {
            rId = d.Id;
            $id('cmbCustomer').value = String(d.CustomerId);
            $id('CmbIncentiveAccount').value = String(d.CustomerIncentivePartyId);
            $id('dtpEffectedDate').value = d.EffectiveDate || SC.today();
            $id('CmbCommissionType').value = String(d.CommissionTypeId);
            var typeId = SC.num($id('CmbCommissionType').value);
            var afterItems = function () {
                $id('CmbCommissionItem').value = String(d.CommissionItemsId);
                /* :427 then :428 — the stored rate is shown, then the item's TextChanged overwrites it with the item's % */
                $id('txtCommissionPercentage').value = String(d.CommissionRate);
                itemChanged();
                $id('chkIsActive').checked = !!d.IsActive;
                setMode(true);
                $id('dtpEffectedDate').focus();
            };
            if (typeId > 0) fillItems(typeId).then(afterItems); else afterItems();
        }).catch(function (e) { alert(e.message); });
    }

    /* InvfrmPriceSchedule_Load :115 */
    function load() {
        $id('dtpEffectedDate').value = SC.today();
        SC.getJson(API + '/init').then(function (d) {
            rights = d.rights || {};
            $id('BtnSave').disabled = !rights.save;
            $id('BtnEdit').disabled = !rights.update;
            bindLookups(d);
            grid.set(d.history || []);
            setMode(false);
            $id('dtpEffectedDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('BtnNew').addEventListener('click', reset);                                  // BtnNew_Click :580
        $id('BtnRefresh').addEventListener('click', refresh);
        $id('BtnSave').addEventListener('click', BtnSave_Click);
        $id('BtnEdit').addEventListener('click', BtnEdit_Click);
        $id('btnshortcut').addEventListener('click', function () { X.showShortcuts(SHORTCUTS); });
        $id('btnAllocateBrandToType').addEventListener('click', function () { alert('AllocateBrandItemToCommissionType is not ported to the web yet.'); });
        $id('btnAllocateDiscountItemToType').addEventListener('click', function () { alert('AllocateCommissionItemToCommissionType is not ported to the web yet.'); });
        $id('CmbCommissionType').addEventListener('change', typeLeave);         // the Leave of the desktop combo
        $id('CmbCommissionItem').addEventListener('change', itemChanged);
        X.decimalOnly($id('txtCommissionPercentage'));                                   // txtDiscountRate_KeyPress :740
        document.addEventListener('keydown', function (e) {                              // InvfrmPriceSchedule_KeyDown :638
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { e.preventDefault(); X.showShortcuts(SHORTCUTS); return; }
            if (k === 'enter' && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {   // Return → Tab
                e.preventDefault();
                var f = Array.prototype.filter.call(document.querySelectorAll('.fx-root input, .fx-root select, .fx-root button'), function (x) { return !x.disabled && x.offsetParent !== null; });
                var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) f[i + 1].focus();
                return;
            }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (!$id('BtnSave').classList.contains('is-hidden') && !$id('BtnSave').disabled) BtnSave_Click(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!$id('BtnEdit').classList.contains('is-hidden') && !$id('BtnEdit').disabled) BtnEdit_Click(); }
            else if (e.ctrlKey && (k === 'f5' || k === 'arrowup')) { e.preventDefault(); $id('dtpEffectedDate').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); grid.focus(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if (!X.closeShortcuts()) X.closeForm(); }
        });
        load();
    });
})();
