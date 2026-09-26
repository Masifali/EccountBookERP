/*
 * Screen 336 "Item Category Store" (ScreenName ItemCategoryStore) — InvDeffrmItemCatagory.cs,
 * opened from the menu with ParentCategoryId 0. Line refs are to that file.
 */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id;
    var api = '/api/store/item-category-store';
    var look = null, recId = 0;

    /* gridfill:915 — visible columns in table order (Id and the *Id columns are hidden, gridsetting:984). */
    var COLS = ['CategoryCode', 'CategoryDescription', 'SerialFrom', 'SerialTo', 'CategoryStatus', 'ParentCategory',
        'CGSAccountTitle', 'RevenueAccountTitle', 'InventoryAccountTitle', 'ClassGroupName', 'ProductionStage', 'VarietyNature'];
    var NUM = { SerialFrom: 1, SerialTo: 1 };
    var gridRows = [], filters = {};

    function val(id) { return $id(id).value; }
    function selInt(id) { return C.intOf($id(id).value); }
    function setSel(id, v) {
        var el = $id(id), s = String(v == null ? 0 : v);
        el.value = s;
        if (el.value !== s) el.value = '0';                  /* not in the list: LimitToList shows nothing */
    }
    function firstRow(id, rows) { setSel(id, rows && rows.length ? C.ci(rows[0], 'Id') : 0); }   /* Rows[0].Activate() */
    function fillKeep(id, rows, text) {                      /* BindDDL: a value still in the list survives */
        var el = $id(id), keep = el.value;
        C.fillSelect(el, rows, 'Id', text);
        setSel(id, keep || 0);
    }

    /* ------------------------------------------------------------------ tabs */
    /* btnsave / btnupdate visibility (desktop Visible toggles), additionally hidden when the
       ScreenName's Save / Update right is missing. */
    function showButtons(isNew) {
        var r = (look && look.rights) || {};
        $id('btnsave').classList.toggle('is-hidden', !(isNew && r.save));
        $id('btnupdate').classList.toggle('is-hidden', !(!isNew && r.update));
    }

    /* The desktop has no TabControl: panel4 (history) sits under panel3 at all times.
       tab('tabForm') brings the form back into view (after opening a row); the footer History
       button scrolls to the grid. */
    function tab(id) {
        if (id === 'tabHistory') { gotoHistory(); return; }
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
    function gotoHistory() {
        $id('histBox').scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
    /* Every combo is bound with DDL.BindDDL(..., ZeroIndex: false): no "...Select Any Value..." row.
       The empty option only stands for the combo's blank Text and is not offered in the list. */
    function hideBlank(id) {
        var el = $id(id);
        if (el.options.length && el.options[0].value === '0' && el.options[0].text === '') el.options[0].hidden = true;
    }

    /* ------------------------------------------------------------------ history grid */
    function renderGrid(rows) {
        if (!rows || !rows.length) return;                   /* gridfill:913 — no rows: grid left as it was */
        gridRows = rows;
        var t = $id('grdfrm');
        t.querySelector('thead').innerHTML = '<tr>' + COLS.map(function (c) { return '<th>' + C.esc(c) + '</th>'; }).join('') + '</tr>' +
            '<tr class="cx-filter">' + COLS.map(function (c) {
                return '<th style="padding:1px;"><input type="text" data-f="' + c + '" value="' + C.esc(filters[c] || '') + '" style="width:100%;box-sizing:border-box;font-size:8pt;"></th>';
            }).join('') + '</tr>';
        t.querySelectorAll('thead input[data-f]').forEach(function (inp) {
            inp.oninput = function () { filters[inp.getAttribute('data-f')] = inp.value; renderBody(); };
        });
        renderBody();
    }
    function renderBody() {
        var t = $id('grdfrm');
        var rows = gridRows.filter(function (r) {
            return COLS.every(function (c) {
                var f = (filters[c] || '').toLowerCase();
                return !f || String(C.ci(r, c)).toLowerCase().indexOf(f) >= 0;
            });
        });
        t.querySelector('tbody').innerHTML = rows.map(function (r) {
            return '<tr data-id="' + C.esc(C.ci(r, 'Id')) + '">' + COLS.map(function (c) {
                var v = C.esc(C.ci(r, c));
                if (c === 'CategoryCode') return '<td><a href="#" class="cx-link" data-open="1">' + v + '</a></td>';
                return '<td' + (NUM[c] ? ' class="num"' : '') + '>' + v + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelectorAll('tbody tr').forEach(function (tr) {
            var id = C.intOf(tr.getAttribute('data-id'));
            tr.ondblclick = function () { open(id); };                              /* grdfrm_DoubleClick:1032 */
            tr.onclick = function () {
                t.querySelectorAll('tbody tr.is-selected').forEach(function (x) { x.classList.remove('is-selected'); });
                tr.classList.add('is-selected');
            };
            var a = tr.querySelector('a[data-open]');
            if (a) a.onclick = function (e) { e.preventDefault(); open(id); };
        });
    }
    function reloadHistory() { return C.getJson(api + '/history').then(renderGrid); }

    /* ------------------------------------------------------------------ code generation */
    function genCode(parentId) {
        return C.getJson(api + '/code' + C.qs({ parentCategoryId: parentId })).then(function (r) {
            $id('txtItemCategoryCode').value = r.code || '';
        });
    }
    /* CmbItemParentCategory_Leave:1212 — regenerates the code on EVERY leave (coding on or off, new or opened). */
    function parentLeave() {
        genCode(selInt('CmbItemParentCategory')).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ load / refresh */
    function applyLists(l) {
        C.fillSelect('CmbItemParentCategory', l.parentCategories, 'Id', 'InvParentCateDescription');
        firstRow('CmbItemParentCategory', l.parentCategories);                  /* ItemParentCategoryFill:369 */
        C.fillSelect('CmbItemClassGroup', l.classGroups, 'Id', 'ClassGroupName');
        firstRow('CmbItemClassGroup', l.classGroups);                           /* GetItemClassGroup:395 */
        fillKeep('CmbProductionStag', l.productionStages, 'productionStageName');
        fillKeep('CmbVarityNature', l.varietyNatures, 'VarietyNatureName');
        fillKeep('cmbItemCategoryInventoryAccount', l.inventoryAccounts, 'AccountTitle');
        fillKeep('cmbItemCategoryRevenueAccount', l.revenueAccounts, 'AccountTitle');
        fillKeep('cmbItemCategoryCGSAccount', l.cgsAccounts, 'AccountTitle');
        ['CmbItemParentCategory', 'CmbItemClassGroup', 'CmbProductionStag', 'CmbVarityNature', 'cmbItemCategoryInventoryAccount',
            'cmbItemCategoryRevenueAccount', 'cmbItemCategoryCGSAccount'].forEach(hideBlank);
        $id('txtItemCategoryCode').disabled = !!l.itemCodingEnable;           /* GetConfigurationsFromGlobal:299 */
        $id('btnMultiLingo').classList.toggle('is-hidden', !l.multiLanguage); /* :175 */
    }

    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            applyLists(l);
            renderGrid(l.history);
            showButtons(true);                                                  /* :209 */
            if (l.itemCodingEnable) { $id('txtItemCategoryCode').value = l.code || ''; $id('txtItemCategoryDescription').focus(); }
            else $id('txtItemCategoryCode').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* btnRefresh_Click:1122 — configurations and every list; history and code untouched. */
    function refresh() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            applyLists(l);
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ New / Reset:1050 */
    function reset() {
        recId = 0;
        $id('lblRecId').innerHTML = '&nbsp;';
        ['txtItemCategoryCode', 'txtItemCategoryDescription', 'txtItemCategorySerialFrom', 'txtItemCategorySerialTo']
            .forEach(function (id) { $id(id).value = ''; });
        ['cmbItemCategoryCGSAccount', 'cmbItemCategoryInventoryAccount', 'cmbItemCategoryRevenueAccount', 'CmbItemClassGroup']
            .forEach(function (id) { setSel(id, 0); });
        /* Production Stage, Variety Nature and Is Active are not reset. */
        var p = reloadHistory().catch(function (e) { alert(e.message); });
        $id('CmbItemParentCategory').disabled = false;
        setSel('CmbItemParentCategory', 0);
        if (look && look.itemCodingEnable) {
            p = p.then(function () { return genCode(0); });                    /* GenerateCode(Value of an emptied combo) */
            $id('txtItemCategoryDescription').focus();
        } else {
            $id('txtItemCategoryCode').focus();
        }
        showButtons(true);
        return p;
    }

    /* ------------------------------------------------------------------ open (ReadById:852) */
    function open(id) {
        return C.getJson(api + '/' + id).then(function (c) {
            recId = C.intOf(c.Id);
            $id('lblRecId').textContent = 'Id: ' + recId;
            showButtons(false);
            $id('txtItemCategoryCode').value = c.CategoryCode;
            $id('txtItemCategoryDescription').value = c.CategoryDescription;
            $id('txtItemCategorySerialFrom').value = c.SerialFrom;
            $id('txtItemCategorySerialTo').value = c.SerialTo;
            setSel('cmbItemCategoryRevenueAccount', c.RevenueAccountId);
            setSel('cmbItemCategoryCGSAccount', c.CGSAccountId);
            setSel('cmbItemCategoryInventoryAccount', c.InventoryAccountId);
            setSel('CmbItemParentCategory', c.InventoryParentCategoriesId);
            setSel('CmbItemClassGroup', c.ItemClassGroupId);
            setSel('CmbProductionStag', c.ItemProductionStageId);
            setSel('CmbVarityNature', c.ItemVarietyNatureId);
            $id('chkstatus').checked = !!c.CategoryStatus;
            renderGrid(c.history);                                              /* gridfill() */
            $id('CmbItemParentCategory').disabled = false;
            tab('tabForm');
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ save / update */
    /* formvalidation:664 — the same checks, texts and order (the server repeats them). */
    function validate() {
        var code = val('txtItemCategoryCode'), d = val('txtItemCategoryDescription').trim();
        var sf = val('txtItemCategorySerialFrom').trim(), st = val('txtItemCategorySerialTo').trim();
        var checks = [
            [code.trim() === '' || code === '0', 'Please Insert Code', 'txtItemCategoryCode'],
            [d === '' || d === '0', 'Please Insert Description', 'txtItemCategoryDescription'],
            [sf === '' || sf === '0', 'Please Insert Serial From', 'txtItemCategorySerialFrom'],
            [st === '' || st === '0', 'Please Insert Serial To', 'txtItemCategorySerialTo'],
            [selInt('cmbItemCategoryRevenueAccount') === 0, 'Please Select Revenue Account', 'cmbItemCategoryRevenueAccount'],
            [selInt('cmbItemCategoryInventoryAccount') === 0, 'Please Select Inventory Account', 'cmbItemCategoryInventoryAccount'],
            [selInt('cmbItemCategoryCGSAccount') === 0, 'Please Select Revenue Account', 'cmbItemCategoryCGSAccount'],
            [selInt('CmbItemParentCategory') === 0, 'Please Select Parent Category', 'CmbItemParentCategory'],
            [selInt('CmbProductionStag') === 0, 'Please Select Production Stage', 'CmbProductionStag'],
            [selInt('CmbVarityNature') === 0, 'Please Select Variety Nature', 'CmbVarityNature']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i][0]) { alert(checks[i][1]); $id(checks[i][2]).focus(); return false; }
        }
        return true;
    }

    function persist(id) {
        if (!validate()) return;
        if (!confirm(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return C.postJson(api + '/save', {
            id: id,
            categoryCode: val('txtItemCategoryCode'),
            categoryDescription: val('txtItemCategoryDescription'),
            serialFrom: val('txtItemCategorySerialFrom'),
            serialTo: val('txtItemCategorySerialTo'),
            revenueAccountId: selInt('cmbItemCategoryRevenueAccount'),
            inventoryAccountId: selInt('cmbItemCategoryInventoryAccount'),
            cgsAccountId: selInt('cmbItemCategoryCGSAccount'),
            parentCategoryId: selInt('CmbItemParentCategory'),
            classGroupId: selInt('CmbItemClassGroup'),
            productionStageId: selInt('CmbProductionStag'),
            varietyNatureId: selInt('CmbVarityNature'),
            categoryStatus: $id('chkstatus').checked
        }).then(function (r) {
            alert(r.message);
            return reset();
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; return persist(0); }                          /* btnsave_Click:827 */
    function update() { return persist(recId); }                               /* btnUpdate_Click:840 */

    function multiLingo() { alert('Multi Lingo (Item Category Multi Language) is not available on the web yet.'); }

    /* ------------------------------------------------------------------ wiring */
    document.addEventListener('DOMContentLoaded', function () {
        $id('CmbItemParentCategory').addEventListener('blur', parentLeave);
        /* txtItemCategorySerialFrom/To_KeyPress:1190 — digits and control keys only. */
        ['txtItemCategorySerialFrom', 'txtItemCategorySerialTo'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) {
                if (e.key && e.key.length === 1 && !/[0-9]/.test(e.key) && !e.ctrlKey && !e.metaKey) e.preventDefault();
            });
        });
        init();
    });

    window.ItemCatStore = { tab: tab, gotoHistory: gotoHistory, reset: reset, refresh: refresh, save: save, update: update, open: open, multiLingo: multiLingo };
})();
