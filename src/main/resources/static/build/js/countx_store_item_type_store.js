/*
 * Screen 337 "Item Type Store" (ScreenName ItemTypeStore) — InvDeffrmItemType.cs, opened from the
 * menu with FormTypeId 0. Line refs are to that file.
 */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id;
    var api = '/api/store/item-type-store';
    var look = null, recId = 0;

    /* gridfill:500 — Id hidden (GridSetting:528). */
    var COLS = ['Code', 'Description', 'Type', 'ParentCategory', 'MotherStatus'];
    var gridRows = [], filters = {};

    function val(id) { return $id(id).value; }
    function selInt(id) { return C.intOf($id(id).value); }
    function setSel(id, v) {
        var el = $id(id), s = String(v == null ? 0 : v);
        el.value = s;
        if (el.value !== s) el.value = '0';
    }

    /* btnsave / btnupdate visibility (desktop Visible toggles), additionally hidden when the
       ScreenName's Save / Update right is missing. */
    function showButtons(isNew) {
        var r = (look && look.rights) || {};
        $id('btnsave').classList.toggle('is-hidden', !(isNew && r.save));
        $id('btnupdate').classList.toggle('is-hidden', !(!isNew && r.update));
    }

    /* The desktop has no TabControl: the history grid (panel2) sits under panel1 at all times.
       tab('tabForm') brings the form back into view (after opening a row); the footer History
       button scrolls to the grid. */
    function tab(id) {
        if (id === 'tabHistory') { gotoHistory(); return; }
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }
    function gotoHistory() {
        $id('histBox').scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    /* ------------------------------------------------------------------ history grid */
    function renderGrid(rows) {
        if (!rows || !rows.length) return;                   /* gridfill:496 — no rows: grid left as it was */
        gridRows = rows;
        var t = $id('grdhistory');
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
        var t = $id('grdhistory');
        var rows = gridRows.filter(function (r) {
            return COLS.every(function (c) {
                var f = (filters[c] || '').toLowerCase();
                return !f || String(C.ci(r, c)).toLowerCase().indexOf(f) >= 0;
            });
        });
        t.querySelector('tbody').innerHTML = rows.map(function (r) {
            return '<tr data-id="' + C.esc(C.ci(r, 'Id')) + '">' + COLS.map(function (c) {
                var v = C.esc(C.ci(r, c));
                if (c === 'Code') return '<td><a href="#" class="cx-link" data-open="1">' + v + '</a></td>';
                return '<td>' + v + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.querySelectorAll('tbody tr').forEach(function (tr) {
            var id = C.intOf(tr.getAttribute('data-id'));
            tr.ondblclick = function () { open(id); };                              /* grdfrm_DoubleClick:538 */
            tr.onclick = function () {
                t.querySelectorAll('tbody tr.is-selected').forEach(function (x) { x.classList.remove('is-selected'); });
                tr.classList.add('is-selected');
            };
            var a = tr.querySelector('a[data-open]');
            if (a) a.onclick = function (e) { e.preventDefault(); open(id); };
        });
    }
    function reloadHistory() { return C.getJson(api + '/history').then(renderGrid); }

    /* ------------------------------------------------------------------ combos */
    /* ItemParentCategoryFill:219 / CombTypeFill:275 — a value still in the new list is restored,
       otherwise the text is blanked; on the first bind Rows[0] is activated. */
    function bind(id, rows, text, first) {
        var el = $id(id), keep = C.intOf(el.value);
        C.fillSelect(el, rows, 'Id', text);
        /* DDL.BindDDL(..., ZeroIndex: false) adds no "...Select Any Value..." row: the empty
           option only stands for the combo's blank Text and is not offered in the list. */
        if (el.options.length && el.options[0].value === '0' && el.options[0].text === '') el.options[0].hidden = true;
        if (first) setSel(id, rows && rows.length ? C.ci(rows[0], 'Id') : 0);
        if (keep > 0) setSel(id, keep);
        if (!rows || !rows.length) setSel(id, 0);
    }

    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            $id('txtItemTypeCode').disabled = !!l.itemCodingEnable;           /* GetConfigurationsFromGlobal:191 */
            $id('btnMultiLingo').classList.toggle('is-hidden', !l.multiLanguage); /* :134 */
            bind('CmbParentCategory', l.parentCategories, 'ParentCategory', true);
            bind('cmbItemTypeType', l.types, 'ItemType', true);
            if (l.itemCodingEnable) { $id('txtItemTypeCode').value = l.code || ''; $id('txtItemTypeDescription').focus(); }
            else $id('txtItemTypeCode').focus();
            renderGrid(l.history);
            showButtons(true);                                                  /* :147 */
        }).catch(function (e) { alert(e.message); });
    }

    /* toolStripButton1_Click:394 — both combos only. Rows[0].Activate() runs on every bind, then a
       previous value still present is restored. */
    function refresh() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            bind('CmbParentCategory', l.parentCategories, 'ParentCategory', true);
            bind('cmbItemTypeType', l.types, 'ItemType', true);
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ New / Reset:333 */
    function reset() {
        recId = 0;
        $id('lblRecId').innerHTML = '&nbsp;';
        $id('txtItemTypeCode').value = '';
        $id('txtItemTypeDescription').value = '';
        showButtons(true);
        var p = Promise.resolve();
        if (look && look.itemCodingEnable) {
            p = C.getJson(api + '/code').then(function (r) { $id('txtItemTypeCode').value = r.code || ''; });
            $id('txtItemTypeDescription').focus();
        } else {
            $id('txtItemTypeCode').focus();
        }
        return p.then(reloadHistory).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ open */
    function open(id) {
        return C.getJson(api + '/' + id).then(function (t) {
            recId = C.intOf(t.Id);
            $id('lblRecId').textContent = 'Id: ' + recId;
            $id('txtItemTypeCode').value = t.TypeCode;
            $id('txtItemTypeDescription').value = t.TypeDescription;
            setSel('cmbItemTypeType', t.Type);
            setSel('CmbParentCategory', t.ParentCategoryId);
            $id('ChkIsMother').checked = !!t.IsMother;
            showButtons(false);
            tab('tabForm');
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ save / update */
    /* formvalidation:155 — the "0" checks are on the untrimmed text. */
    function validate() {
        var code = val('txtItemTypeCode'), d = val('txtItemTypeDescription');
        if (code === '' || code === '0') { alert('Please Insert Code'); $id('txtItemTypeCode').value = ''; $id('txtItemTypeCode').focus(); return false; }
        if (d === '' || d === '0') { alert('Please Insert Description'); $id('txtItemTypeDescription').value = ''; $id('txtItemTypeDescription').focus(); return false; }
        if (selInt('cmbItemTypeType') === 0) { alert('Please Select Type'); $id('cmbItemTypeType').focus(); return false; }
        if (selInt('CmbParentCategory') === 0) { alert('Please Select Parent Category'); $id('CmbParentCategory').focus(); return false; }
        return true;
    }

    function persist(id) {
        if (!validate()) return;
        if (!confirm(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        return C.postJson(api + '/save', {
            id: id,
            typeCode: val('txtItemTypeCode'),
            typeDescription: val('txtItemTypeDescription'),
            type: selInt('cmbItemTypeType'),
            parentCategoryId: selInt('CmbParentCategory'),
            isMother: $id('ChkIsMother').checked
        }).then(function (r) {
            alert(r.message);
            return reset();
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; return persist(0); }                          /* btnsave_Click:456 */
    function update() { return persist(recId); }                               /* btnUpdate_Click:469 */

    function multiLingo() { alert('Multi Lingo (Item Type Multi Language) is not available on the web yet.'); }

    document.addEventListener('DOMContentLoaded', function () { init(); });

    window.ItemTypeStore = { tab: tab, gotoHistory: gotoHistory, reset: reset, refresh: refresh, save: save, update: update, open: open, multiLingo: multiLingo };
})();
