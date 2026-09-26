/* ============================================================================================
 * Item PM - screen 496, Packing Material module 54.
 * Desktop: Architecture.WinApp.StoreManagement.AddItemPM (AddItemPM.cs, 4,384 lines).
 *
 * Handler names follow the desktop's so each can be checked against its C# line:
 *
 *   InvDefrmAddItem_Load      :332   -> init()
 *   HistoryComboBind          :374   -> bindHistoryCombos()
 *   cmbItemCategory_Leave     :628   -> category change, only while Save is the active button
 *   cmbItemType_Leave         :1913  -> type change, ALWAYS (the desktop does not check the mode)
 *   FormValidationForAddItem  :787   -> formValidation()   (also enforced on the server)
 *   BtnSave_Click             :851   -> BtnSave_Click()
 *   btnupdate_Click           :989   -> btnupdate_Click()
 *   grdhistory_DoubleClick    :1105  -> openRecord()
 *   refresh                   :1263  -> refresh()
 *   btnnew_Click              :1301  -> btnnew_Click()
 *   toolStripButton1_Click    :1320  -> toolStripButton1_Click()  (Refresh)
 *   HistoryGridfill           :1359  -> btnsearch_Click()
 *   btnMasterItemsUpdate_Click:1855  -> btnMasterItemsUpdate_Click()
 *   InvDefrmAddItem_KeyDown   :1699  -> keyboard shortcuts
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/item-pm';
    var RecId = 0;
    var L = null;                 // lookups
    var masterItems = [];         // dtMasterItems, also the history grid's MasterItem combo
    var historyRows = [];
    var pics = { 1: null, 2: null };        // {id, upload:{name,base64}, url}
    var pendingSlot = 0;
    var newFiles = [];
    var removeAttachmentIds = [];
    var existingAttachments = [];

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    /* C#'s ToString("#,##0.###") / ("#,##0") */
    function fmt(v, dp) {
        var n = num(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dp });
    }
    function fmtDate(v) {
        if (!v) return '';
        var d = new Date(v);
        if (isNaN(d.getTime())) return String(v);
        var m = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][d.getMonth()];
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return String(d.getDate()).padStart(2, '0') + '-' + m + '-' + d.getFullYear() + ' '
             + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); applyRights(); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function http(method, url, body) {
        var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
                return b;
            });
        });
    }
    function getJson(url) { return http('GET', url); }

    // ------------------------------------------------------------------ combos

    function defineFamilies() {
        if (!window.DesktopCombo) return;
        /* BaseUnitFill :489-497 - Id, UomCode, Equivalent; BindDDL hides only column 0. */
        window.DesktopCombo.define('uomPm', [
            { caption: 'UOM Code', flex: 2 },
            { caption: 'Equivalent', flex: 2, key: 'eq', type: 'num' }
        ]);
        /* RackNameFillFromGlobal :616-619 - AllColumns, column 2 (invWarehouseId) hidden. */
        window.DesktopCombo.define('rackPm', [
            { caption: 'Rack Name', flex: 3 },
            { caption: 'WarehouseName', flex: 3, key: 'warehouse' }
        ]);
    }

    /**
     * BindDDL (DropDownBind.cs) hides column 0 and shows every other column of the DataTable it is
     * given. For Master Item and the three GL combos that DataTable is the procedure's result as
     * returned, so the column set is read off the response rather than written down here.
     */
    function defineDynamic(family, caption, displayKey, rows) {
        var cols = [{ caption: caption, flex: 3 }];
        var keys = [];
        if (rows && rows.length) {
            var names = Object.keys(rows[0]);
            names.slice(1).forEach(function (k) {
                if (k.toLowerCase() === displayKey.toLowerCase()) return;
                keys.push(k);
                cols.push({ caption: k, flex: 2, key: 'c-' + k.toLowerCase() });
            });
        }
        if (window.DesktopCombo) window.DesktopCombo.define(family, cols);
        return keys;
    }

    function fill(id, rows, valueKey, textKey, extra, blank) {
        var sel = $id(id); if (!sel) return;
        var keep = sel.value;
        var html = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var attrs = '';
            (extra || []).forEach(function (x) { attrs += ' data-' + x.attr + '="' + esc(col(r, x.key)) + '"'; });
            html += '<option value="' + esc(col(r, valueKey)) + '"' + attrs + '>' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + keep.replace(/"/g, '') + '"]')) sel.value = keep;
    }

    function dynFill(id, rows, keys, textKey) {
        fill(id, rows, 'Id', textKey, keys.map(function (k) { return { attr: 'c-' + k.toLowerCase(), key: k }; }));
    }

    // ------------------------------------------------------------------ load

    function init() {
        defineFamilies();
        bindKeys();
        $id('fileImage').addEventListener('change', onImagePicked);
        $id('fileAttachment').addEventListener('change', onAttachmentsPicked);
        $id('cmbItemCategory').addEventListener('change', cmbItemCategory_Leave);
        $id('cmbItemType').addEventListener('change', cmbItemType_Leave);
        return loadLookups(true).then(function () {
            showTab(0);
            say('');
        }).catch(function (e) { say('Not loaded.'); box(e.message); });
    }

    function loadLookups(first) {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d || {};
            masterItems = L.masterItems || [];

            fill('cmbItemCategory', L.categories, 'Id', 'CategoryDescription');           // ItemCategoryBind
            fill('cmbItemType', L.types, 'Id', 'TypeDescription');                         // ItemTypeFill
            fill('CmbItemTypeHistory', L.types, 'Id', 'TypeDescription');
            var mk = defineDynamic('masterItemPm', 'Master Item', 'MasterItem', masterItems);
            dynFill('CmbMasterItem', masterItems, mk, 'MasterItem');                       // MasterItemFill
            if (first) companiesBindInGrid(L.companies || []);                            // CompaniesBindInGrid
            var ak = defineDynamic('accountPm', 'Account Tile', 'AccountTitle', L.accounts || []);
            ['cmbPurchaseGL', 'cmbSaleGL', 'cmbCGSGL'].forEach(function (id) { dynFill(id, L.accounts, ak, 'AccountTitle'); });
            fill('CmbRackName', L.racks, 'Id', 'RackName', [{ attr: 'warehouse', key: 'WarehouseName' }]);
            fill('cmbTaxType', L.taxes, 'Id', 'TaxName');                                  // CmbTaxTypeFill
            fill('cmbBaseUnit', L.units, 'Id', 'UomCode', [{ attr: 'eq', key: 'Equivalent' }]);
            fill('CmbPackSizeId', L.packSizes, 'Id', 'type', null, false);                // PackSizeIdForItem
            var ps = $id('CmbPackSizeId');
            if (first && ps && ps.options.length) ps.selectedIndex = 0;                    // Rows[0].Activate() :762

            var f = L.features || {};
            $id('lblChkIsCompany').style.display = f.partyProcessing ? '' : 'none';      // :362-363
            $id('lblChkIsThirdParty').style.display = f.partyProcessing ? '' : 'none';
            $id('BtnAttributesAllocation').style.display = f.attributesForItem ? '' : 'none'; // :365

            if (first) bindHistoryCombos();
            applyRights();
            if (window.DesktopCombo) window.DesktopCombo.refresh();
            say('');
        });
    }

    /**
     * HistoryComboBind :374. NOTE - ported exactly, including what it binds: the Item Type list
     * built from existing PM items is bound into cmbItemType (:409), the ENTRY combo, not into
     * CmbItemTypeHistory. So once the company has any PM item, the entry Item Type offers only the
     * types already used by PM items. Reported for a decision; not changed.
     */
    function bindHistoryCombos() {
        var h = (L && L.history) || {};
        if (!h.types || !h.types.length) {
            if ((!h.categories || !h.categories.length) && (!h.masterItems || !h.masterItems.length)) return;   // :381 - no PM items
        }
        fill('cmbitemcathistory', h.categories, 'Id', 'Name');
        fill('cmbItemType', h.types, 'Id', 'Name');
        fill('CmbMasterItemHistory', h.masterItems, 'Id', 'Name');
    }

    function companiesBindInGrid(rows) {
        var html = '';
        rows.forEach(function (r) {
            html += '<tr><td>' + esc(col(r, 'Location')) + '</td>'
                  + '<td><input type="checkbox" class="alloc" data-id="' + esc(col(r, 'Id')) + '" checked/></td></tr>';
        });
        $id('grdAllocation').innerHTML = html;
        $id('chkAllocationHeader').checked = rows.length > 0;
    }

    function allocationHeader(on) {
        Array.prototype.forEach.call(document.querySelectorAll('#grdAllocation input.alloc'), function (c) { c.checked = on; });
    }

    // ------------------------------------------------------------------ rights / mode

    var saveMode = true;
    function applyRights() {
        var p = (L && L.permissions) || {};
        var s = $id('btnsave'), u = $id('btnupdate');
        s.style.display = saveMode ? '' : 'none';
        u.style.display = saveMode ? 'none' : '';
        if (!s.classList.contains('is-busy')) s.disabled = !p.Save;          // :340
        if (!u.classList.contains('is-busy')) u.disabled = !p.Update;        // :341
    }
    function isSaveButtonActive() { return saveMode && !$id('btnsave').disabled; }   // :645

    // ------------------------------------------------------------------ leave handlers

    function cmbItemCategory_Leave() {
        if (!isSaveButtonActive()) return Promise.resolve();
        return defaults(true);
    }
    /** :1913 - no mode check on the desktop, so it also runs while an item is open for update. */
    function cmbItemType_Leave() { return defaults(false); }

    function defaults(withAccounts) {
        var q = '?categoryId=' + int(val('cmbItemCategory')) + '&typeId=' + int(val('cmbItemType')) + '&withAccounts=' + withAccounts;
        return getJson(api + '/defaults' + q).then(function (d) {
            if (d && d.ItemCode !== undefined) {                              // GenerateItemCode :676
                setVal('txtItemCode', d.ItemCode);
                setVal('txtItemCodeNew', d.ItemCodeNew);
            }
            if (withAccounts && d && d.PurchaseGLAC !== undefined) {         // SetGLAccounts :693
                setVal('cmbPurchaseGL', String(col(d, 'PurchaseGLAC')));
                setVal('cmbSaleGL', String(col(d, 'SaleGLAC')));
                setVal('cmbCGSGL', String(col(d, 'COGSGLAC')));
            }
        }).catch(function () { box('Record Not Found'); });
    }

    // ------------------------------------------------------------------ validation :787

    function formValidation() {
        var p = (L && L.features) || {};
        var checks = [
            [function () { return val('txtItemName').trim() === ''; }, 'Item Name Field is Required', 'txtItemName'],
            [function () { return !int(val('cmbBaseUnit')); }, 'Base Unit Field is Required', 'cmbBaseUnit'],
            [function () { return !int(val('cmbItemCategory')); }, 'Item Category Field is Required', 'cmbItemCategory'],
            [function () { return !int(val('cmbItemType')); }, 'Item Type Field is Required', 'cmbItemType'],
            [function () { return !int(val('CmbRackName')); }, 'Rack Name Field is Required', 'CmbRackName'],
            [function () { return p.partyProcessing && !$id('ChkIsCompany').checked && !$id('ChkIsThirdParty').checked; },
                "Please Check of the Check box ('Is Company' Or 'Is third Party') ", 'ChkIsCompany'],
            [function () { return num(val('txtMinStock')) > num(val('txtMaxStock')); }, 'Min stock level cannot be greater than max stock level.', null],
            [function () { return !int(val('cmbPurchaseGL')); }, 'Stock A/c Field is Required', 'cmbPurchaseGL'],
            [function () { return !int(val('cmbSaleGL')); }, 'Sale A/c Field is Required', 'cmbSaleGL'],
            [function () { return !int(val('cmbCGSGL')); }, 'CGS A/c Filed is Required', 'cmbCGSGL']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i][0]()) { box(checks[i][1]); focus(checks[i][2]); return false; }
        }
        return true;
    }
    function focus(id) {
        if (!id) return;
        var e = $id(id); if (!e) return;
        var c = e.__dtcombo; if (c && c.input) c.input.focus(); else e.focus();
    }

    // ------------------------------------------------------------------ save / update

    function payload(isUpdate) {
        var images = [];
        [1, 2].forEach(function (slot) {
            var p = pics[slot];
            if (!p) return;
            images.push({ sortNo: slot, id: p.id || 0, upload: p.upload || null });
        });
        var companies = [];
        Array.prototype.forEach.call(document.querySelectorAll('#grdAllocation input.alloc'), function (c) {
            if (c.checked) companies.push(int(c.getAttribute('data-id')));
        });
        return {
            Id: isUpdate ? RecId : 0,
            ItemCode: val('txtItemCode'),
            ItemCodeNew: val('txtItemCodeNew'),
            BarcodeNo: val('txtBarcodeNo'),
            ItemName: val('txtItemName'),
            BaseUnitId: int(val('cmbBaseUnit')),
            ItemCategoryId: int(val('cmbItemCategory')),
            ItemTypeId: int(val('cmbItemType')),
            MasterItemId: int(val('CmbMasterItem')),
            RackId: int(val('CmbRackName')),
            EmptyBagWeight: num(val('txtEbWeight')),
            MinStockLevel: num(val('txtMinStock')),
            MaxStockLevel: num(val('txtMaxStock')),
            ReorderLevel: num(val('txtReOrderLevel')),
            ReOrderQty: num(val('txtReOrderQty')),
            LeadTimeDay: int(val('txtLeadTimeDay')),
            PackSize: num(val('txtPackSize')),
            PackSizeId: int(val('CmbPackSizeId')),
            AllowMultiUom: $id('chkAllowMultiUom').checked,
            ItemStatus: $id('chkstatus').checked,
            LocalSelected: $id('rdLocal').checked,
            IsCompany: $id('ChkIsCompany').checked,
            IsThirdParty: $id('ChkIsThirdParty').checked,
            ApplyGST: $id('chbGST').checked,
            TaxTypeId: int(val('cmbTaxType')),
            PurchaseGLAC: int(val('cmbPurchaseGL')),
            SaleGLAC: int(val('cmbSaleGL')),
            COGSGLAC: int(val('cmbCGSGL')),
            companies: companies,
            images: images,
            files: newFiles.slice(),
            removeAttachmentIds: removeAttachmentIds.slice()
        };
    }

    function BtnSave_Click() {
        if (!isSaveButtonActive()) return;
        if (!formValidation()) return;
        return busy('btnsave', function () {
            say('Saving...');
            return http('POST', api + '/save', payload(false)).then(function (d) {
                box((d && d.message) || 'Save Successfully');
                return refresh();
            }).catch(function (e) { say(''); box(e.message); });
        });
    }

    function btnupdate_Click() {
        if (saveMode || $id('btnupdate').disabled) return;
        if (!formValidation()) return;
        return busy('btnupdate', function () {
            say('Updating...');
            return http('POST', api + '/save', payload(true)).then(function (d) {
                box((d && d.message) || 'Update Successfully');
                return refresh();
            }).catch(function (e) { say(''); box(e.message); });
        });
    }

    // ------------------------------------------------------------------ refresh / new

    /** refresh() :1263 - clears only what the desktop clears; the rest keep their values. */
    function refresh() {
        RecId = 0;
        saveMode = true;
        setComboReadOnly('cmbItemCategory', false);
        $id('BrandImages').style.display = '';
        setVal('CmbRackName', ''); setVal('cmbCGSGL', ''); setVal('cmbSaleGL', '');
        setVal('txtMinStock', ''); setVal('txtMaxStock', ''); setVal('txtReOrderLevel', '');
        setVal('txtItemCode', ''); setVal('txtBarcodeNo', ''); setVal('txtItemName', '');
        $id('chbGST').checked = false;
        resetPic(1); resetPic(2);
        newFiles = []; removeAttachmentIds = []; existingAttachments = [];
        renderAttachments();
        $id('fileAttachment').value = '';
        applyRights();
        say('');
        focus('cmbItemCategory');
        return cmbItemCategory_Leave();                                           // :1285
    }

    function btnnew_Click() {                                                     // :1301
        var p = refresh();
        setVal('cmbBaseUnit', '');
        setVal('cmbItemType', '');
        return p;
    }

    /** toolStripButton1_Click :1320 - rebinds the lookups; the allocation grid and history combos stay. */
    function toolStripButton1_Click() {
        return busy('BtnRefresh', function () { return loadLookups(false).catch(function (e) { box(e.message); }); });
    }

    function setComboReadOnly(id, ro) {
        var e = $id(id); if (!e) return;
        e.disabled = !!ro;
        if (e.__dtcombo && e.__dtcombo.reflectDisabled) e.__dtcombo.reflectDisabled();
    }

    // ------------------------------------------------------------------ history

    function btnsearch_Click() {
        return busy('btnsearch', function () {
            var q = '?categoryId=' + int(val('cmbitemcathistory')) + '&typeId=' + int(val('CmbItemTypeHistory'))
                  + '&masterItemId=' + int(val('CmbMasterItemHistory'));
            return getJson(api + '/history' + q).then(function (rows) {
                historyRows = rows || [];
                renderHistory();
            }).catch(function (e) { box(e.message); });
        });
    }

    /** gridsetting :1431 - grouped by TypeDescription, the group column hidden; MasterItem and EmptyBagWeight editable. */
    function renderHistory() {
        var groups = {}, order = [];
        historyRows.forEach(function (r) {
            var g = String(col(r, 'TypeDescription') || '');
            if (!groups[g]) { groups[g] = []; order.push(g); }
            groups[g].push(r);
        });
        var opts = '<option value="0"></option>' + masterItems.map(function (m) {
            return '<option value="' + esc(col(m, 'Id')) + '">' + esc(col(m, 'MasterItem')) + '</option>';
        }).join('');
        var html = '';
        order.forEach(function (g, gi) {
            html += '<tr class="group-row" data-group="' + gi + '"><td colspan="14">TypeDescription: ' + esc(g) + ' (' + groups[g].length + ')</td></tr>';
            groups[g].forEach(function (r) {
                var id = int(col(r, 'Id'));
                html += '<tr class="data-row" data-g="' + gi + '" data-id="' + id + '">'
                     + '<td>' + esc(col(r, 'ItemCode')) + '</td>'
                     + '<td>' + esc(col(r, 'ItemCodeNew')) + '</td>'
                     + '<td>' + esc(col(r, 'ItemName')) + '</td>'
                     + '<td><select class="hist-master" data-id="' + id + '">' + opts.replace('value="' + esc(int(col(r, 'MasterItemId'))) + '"', 'value="' + esc(int(col(r, 'MasterItemId'))) + '" selected') + '</select></td>'
                     + '<td><input type="text" class="cell hist-eb" data-guard="decimal" data-id="' + id + '" value="' + esc(num(col(r, 'EmptyBagWeight'))) + '"/></td>'
                     + '<td>' + esc(col(r, 'CategoryDescription')) + '</td>'
                     + '<td>' + esc(col(r, 'ItemStatus')) + '</td>'
                     + '<td>' + esc(col(r, 'GlStockAccountTitle')) + '</td>'
                     + '<td>' + esc(col(r, 'GlSaleAccountTitle')) + '</td>'
                     + '<td>' + esc(col(r, 'GlCgsAccountTitle')) + '</td>'
                     + '<td>' + esc(fmtDate(col(r, 'EntryDate'))) + '</td>'
                     + '<td>' + esc(col(r, 'EntryUserName')) + '</td>'
                     + '<td class="num">' + (int(col(r, 'NoOfAttachments')) > 0
                            ? '<span class="win-link hist-att" data-id="' + id + '">' + int(col(r, 'NoOfAttachments')) + '</span>'
                            : '0') + '</td>'
                     + '<td class="num">' + esc(int(col(r, 'LeadTimeDay'))) + '</td>'
                     + '</tr>';
            });
        });
        var body = $id('grdhistory');
        body.innerHTML = html;
        $id('lblHistoryCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
        Array.prototype.forEach.call(body.querySelectorAll('tr.group-row'), function (tr) {
            tr.addEventListener('click', function () {
                var g = tr.getAttribute('data-group');
                Array.prototype.forEach.call(body.querySelectorAll('tr.data-row[data-g="' + g + '"]'), function (row) {
                    row.style.display = row.style.display === 'none' ? '' : 'none';
                });
            });
        });
        Array.prototype.forEach.call(body.querySelectorAll('tr.data-row'), function (tr) {
            tr.addEventListener('dblclick', function (e) {
                if (e.target && (e.target.tagName === 'SELECT' || e.target.tagName === 'INPUT')) return;
                openRecord(int(tr.getAttribute('data-id')));
            });
        });
        Array.prototype.forEach.call(body.querySelectorAll('.hist-att'), function (a) {
            a.addEventListener('click', function (e) {
                e.stopPropagation();
                openRecord(int(a.getAttribute('data-id'))).then(function () { btnattachment_Click(true); });
            });
        });
    }

    function btnNewHistory_Click() {
        setVal('cmbitemcathistory', ''); setVal('CmbItemTypeHistory', ''); setVal('CmbMasterItemHistory', '');
        historyRows = []; renderHistory();
    }

    /** btnRefreshHistory_Click :1489 - HistoryComboBind + the grid's Master Item list. */
    function btnRefreshHistory_Click() {
        return busy('btnRefreshHistory', function () {
            return getJson(api + '/lookups').then(function (d) {
                L.history = d.history; masterItems = d.masterItems || masterItems;
                bindHistoryCombos();
                renderHistory();
            }).catch(function (e) { box(e.message); });
        });
    }

    function btnMasterItemsUpdate_Click() {
        var rows = [];
        Array.prototype.forEach.call(document.querySelectorAll('#grdhistory tr.data-row'), function (tr) {
            var id = int(tr.getAttribute('data-id'));
            var m = int(tr.querySelector('.hist-master').value);
            var eb = num(tr.querySelector('.hist-eb').value);
            if (m > 0 || eb > 0) rows.push({ itemId: id, masterItemId: m, emptyBagWeight: eb });   // :1869
        });
        return busy('btnMasterItemsUpdate', function () {
            return http('POST', api + '/master-items', rows).then(function (d) {
                box((d && d.message) || 'Record Updated successfully...');
            }).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------ open a record :1105

    function openRecord(id) {
        if (!id) return Promise.resolve();
        say('Opening...');
        return getJson(api + '/' + id).then(function (d) {
            var it = d.item || {};
            RecId = id;
            saveMode = false;
            showTab(0);
            var f = (L && L.features) || {};
            if (f.partyProcessing) {
                $id('ChkIsThirdParty').checked = !!col(it, 'IsThirdParty');
                $id('ChkIsCompany').checked = !!col(it, 'IsCompany');
            }
            setVal('txtItemCode', col(it, 'ItemCode'));
            setVal('txtItemCodeNew', col(it, 'ItemCodeNew'));
            setVal('txtBarcodeNo', col(it, 'BarcodeNo'));
            setVal('txtItemName', col(it, 'ItemName'));
            setVal('cmbBaseUnit', String(int(col(it, 'BaseUnitId'))));
            setVal('cmbItemCategory', String(int(col(it, 'ItemCategoryId'))));
            setComboReadOnly('cmbItemCategory', true);                             // :1129
            setVal('cmbItemType', String(int(col(it, 'ItemTypeId'))));
            if (int(col(it, 'MasterItemId')) > 0) setVal('CmbMasterItem', String(int(col(it, 'MasterItemId'))));
            if (int(col(it, 'RackId')) > 0) setVal('CmbRackName', String(int(col(it, 'RackId'))));
            $id('chkstatus').checked = !!col(it, 'ItemStatus');
            setVal('cmbPurchaseGL', String(int(col(it, 'PurchaseGLAC'))));
            setVal('cmbSaleGL', String(int(col(it, 'SaleGLAC'))));
            setVal('cmbCGSGL', String(int(col(it, 'COGSGLAC'))));
            setVal('txtEbWeight', fmt(col(it, 'EmptyBagWeight'), 3));
            setVal('txtMinStock', fmt(col(it, 'MinStockLevel'), 3));
            setVal('txtMaxStock', fmt(col(it, 'MaxStockLevel'), 3));
            setVal('txtReOrderLevel', fmt(col(it, 'ReorderLevel'), 3));
            setVal('txtLeadTimeDay', fmt(col(it, 'LeadTimeDay'), 0));
            setVal('txtPackSize', fmt(col(it, 'PackSize'), 0));
            setVal('CmbPackSizeId', String(int(col(it, 'PackSizeId'))));
            $id('chkAllowMultiUom').checked = !!col(it, 'AllowMultiUom');
            $id('chbGST').checked = !!col(it, 'ApplyGST');

            existingAttachments = d.attachments || [];
            newFiles = []; removeAttachmentIds = [];
            renderAttachments();

            resetPic(1); resetPic(2);
            (d.images || []).forEach(function (img) {
                var slot = int(col(img, 'SortNo'));
                if (slot !== 1 && slot !== 2) return;
                pics[slot] = { id: int(col(img, 'Id')), url: api + '/' + id + '/images/' + int(col(img, 'Id')) };
                $id('picbox' + slot).innerHTML = '<img alt="" src="' + esc(pics[slot].url) + '"/>';
            });
            applyRights();
            say('Item ' + (col(it, 'ItemCodeNew') || id) + ' opened.');
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ images

    function browse(slot) { pendingSlot = slot; var f = $id('fileImage'); f.value = ''; f.click(); }
    function onImagePicked(e) {
        var file = e.target.files && e.target.files[0];
        if (!file || !pendingSlot) return;
        if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }
        var slot = pendingSlot;
        var reader = new FileReader();
        reader.onload = function () {
            var data = String(reader.result);
            pics[slot] = { id: 0, upload: { name: file.name, base64: data.substring(data.indexOf(',') + 1) } };
            $id('picbox' + slot).innerHTML = '<img alt="" src="' + esc(data) + '"/>';
        };
        reader.readAsDataURL(file);
    }
    function resetPic(slot) { pics[slot] = null; $id('picbox' + slot).innerHTML = ''; }       // btnPicNReset

    // ------------------------------------------------------------------ attachments

    function btnattachment_Click(forceOpen) {
        var g = $id('grpAttachments');
        g.style.display = (forceOpen === true || g.style.display === 'none') ? '' : 'none';
    }
    function onAttachmentsPicked(e) {
        var files = Array.prototype.slice.call(e.target.files || []);
        files.forEach(function (file) {
            if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }
            var reader = new FileReader();
            reader.onload = function () {
                var data = String(reader.result);
                newFiles.push({ name: file.name, base64: data.substring(data.indexOf(',') + 1) });
                renderAttachments();
            };
            reader.readAsDataURL(file);
        });
        e.target.value = '';
    }
    function renderAttachments() {
        var html = '';
        existingAttachments.forEach(function (a) {
            var id = int(col(a, 'Id'));
            if (removeAttachmentIds.indexOf(id) >= 0) return;
            html += '<div><a class="win-link" href="' + api + '/' + RecId + '/attachments/' + id + '">' + esc(col(a, 'Attachment')) + '</a>'
                  + ' <span class="win-link" data-remove="' + id + '">[remove]</span></div>';
        });
        newFiles.forEach(function (f, i) {
            html += '<div>' + esc(f.name) + ' <em>(new)</em> <span class="win-link" data-drop="' + i + '">[remove]</span></div>';
        });
        var box2 = $id('lstAttachments');
        box2.innerHTML = html;
        Array.prototype.forEach.call(box2.querySelectorAll('[data-remove]'), function (s) {
            s.addEventListener('click', function () { removeAttachmentIds.push(int(s.getAttribute('data-remove'))); renderAttachments(); });
        });
        Array.prototype.forEach.call(box2.querySelectorAll('[data-drop]'), function (s) {
            s.addEventListener('click', function () { newFiles.splice(int(s.getAttribute('data-drop')), 1); renderAttachments(); });
        });
    }

    // ------------------------------------------------------------------ tabs + keys

    var currentTab = 0;
    function showTab(i) {
        currentTab = i;
        $id('tpItemDefinitionAddItem').style.display = i === 0 ? '' : 'none';
        $id('tpItemDefinitionAddItemHistory').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0);
        $id('tabHistory').classList.toggle('active', i === 1);
        focus(i === 1 ? 'cmbitemcathistory' : 'txtItemName');                   // tabCAddItem_SelectedIndexChanged
    }

    /** InvDefrmAddItem_KeyDown :1699 */
    function bindKeys() {
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (saveMode && currentTab === 0) BtnSave_Click(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew_Click(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); return; }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!saveMode) btnupdate_Click(); return; }
            if (e.ctrlKey && k === 'e') { e.preventDefault(); window.location.href = '/dashboard'; return; }
            if (k === 'enter' && e.target && e.target.tagName === 'INPUT' && e.target.type !== 'file'
                    && !(e.target.classList && e.target.classList.contains('dtcombo-input'))) {
                e.preventDefault(); nextField(e.target);                          // SendKeys("{TAB}")
            }
        });
    }
    function nextField(el) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, button, textarea'), function (x) {
            return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null && x.type !== 'hidden';
        });
        var i = all.indexOf(el);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }

    window.ItemPm = {
        init: init, showTab: showTab,
        btnnew_Click: btnnew_Click, toolStripButton1_Click: toolStripButton1_Click,
        BtnSave_Click: BtnSave_Click, btnupdate_Click: btnupdate_Click, btnattachment_Click: btnattachment_Click,
        btnsearch_Click: btnsearch_Click, btnNewHistory_Click: btnNewHistory_Click,
        btnRefreshHistory_Click: btnRefreshHistory_Click, btnMasterItemsUpdate_Click: btnMasterItemsUpdate_Click,
        allocationHeader: allocationHeader, browse: browse, resetPic: resetPic
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
