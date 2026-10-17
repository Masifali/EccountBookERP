/* InvDeffrmItemItemUom (Inventory_Definition/InvDeffrmItemItemUom.cs) - ScreenDefinition 114 "Define Item UOM".
   Line refs ":NNN" are that file. API: /inventory/define-item-uom/api.
     InvDeffrmItemItemUom_Load :79   ParentUOMFill (BindDDLNew true → default row), gridfill, btnupdate hidden, focus code
     Insert :182 / grdfrm_DoubleClick :318 (GetByID) / Reset :129 / btnnew_Click :146 */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4b, $id = SC.$id, API = '/inventory/define-item-uom/api';
    var DEFAULT_TEXT = '...Select Any Value...';
    var recId = 0;

    function q2(v) { return X.fmt(v, 2); }                          // "#,##0.##"
    /* gridsetting :296 — Id, ParentUOMId hidden; Status checkbox; numbers right-aligned "#,##0.##" */
    var grid = new X.Grid('grdfrm', [
        { key: 'Id', hidden: true },
        { key: 'UomCode', cap: 'UomCode' },
        { key: 'Equivalent', cap: 'Equivalent', n: true, fmt: q2 },
        { key: 'ParentUOMId', hidden: true },
        { key: 'ParentUOM', cap: 'ParentUOM' },
        { key: 'Status', cap: 'Status', check: true },
        { key: 'QtyEquivalent', cap: 'QtyEquivalent', n: true, fmt: q2 }
    ], { nav: 'lblNavigator', onDblClick: function (r) { readById(r.Id); } });

    function setMode(update) {
        $id('btnsave').classList.toggle('is-hidden', update);
        $id('btnupdate').classList.toggle('is-hidden', !update);
    }

    /* gridfill :258 — no rows → grid left as it was (:271) */
    function gridfill() { return SC.getJson(API + '/history').then(function (rows) { if (rows && rows.length) grid.set(rows); }).catch(function (e) { alert(e.message); }); }

    function body() {
        return { Id: recId, UOMCode: $id('txtItemUOMCode').value.trim(), Equivalent: $id('txtEquivalent').value.trim(),
            QtyEquivalent: $id('txtQtyEquivalent').value.trim(), UOMStatus: $id('chkstatus').checked, ParentUOMId: SC.num($id('CmbParentUOM').value) };
    }
    /* Insert :182 → Reset() */
    function insert() { X.insert(API, body(), function () { reset(); }); }
    function BtnSavetemUOM_Click() { recId = 0; insert(); }         // :233
    function btnupdate_Click() { insert(); }                        // :246

    /* Reset :129 — texts cleared (status / parent untouched), Save mode, gridfill */
    function reset() {
        recId = 0;
        $id('txtEquivalent').value = '';
        $id('txtQtyEquivalent').value = '';
        $id('txtItemUOMCode').value = '';
        setMode(false);
        gridfill();
    }

    /* grdfrm_DoubleClick :318 */
    function readById(id) {
        SC.getJson(API + '/' + id).then(function (c) {
            recId = c.Id;
            $id('txtItemUOMCode').value = c.UOMCode || '';
            $id('txtEquivalent').value = String(c.Equivalent);
            $id('chkstatus').checked = !!c.UOMStatus;
            $id('CmbParentUOM').value = String(c.ParentUOMId);
            $id('txtQtyEquivalent').value = String(c.QtyEquivalent);
            setMode(true);
        }).catch(function (e) { if (e.message !== 'Record Not Found') alert(e.message); });   // :327 "if (cat != null)"
    }

    /* InvDeffrmItemItemUom_Load :79 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            if (d.parentUoms && d.parentUoms.length) X.fillSelect('CmbParentUOM', d.parentUoms, DEFAULT_TEXT);   // ParentUOMFill :90
            if (d.history && d.history.length) grid.set(d.history);
            setMode(false);
            $id('txtItemUOMCode').focus();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnnew').addEventListener('click', reset);
        $id('btnsave').addEventListener('click', BtnSavetemUOM_Click);
        $id('btnupdate').addEventListener('click', btnupdate_Click);
        X.decimalOnly($id('txtEquivalent'));                                             // txtEquivalent_KeyPress :401
        X.decimalOnly($id('txtQtyEquivalent'));
        document.addEventListener('keydown', function (e) {                              // InvDeffrmItemItemUom_KeyDown :362
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (!$id('btnsave').classList.contains('is-hidden')) BtnSavetemUOM_Click(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!$id('btnupdate').classList.contains('is-hidden')) btnupdate_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && (k === 'f5' || k === 'arrowup')) { e.preventDefault(); $id('txtItemUOMCode').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); grid.focus(); }
        });
        load();
    });
})();
