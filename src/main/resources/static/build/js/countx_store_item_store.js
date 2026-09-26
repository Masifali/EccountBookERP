/* ============================================================================================
 * Item Store - screen 329, Store Management module 24.
 * Desktop: Architecture.WinApp.StoreManagement.AddItemStore (AddItemStore.cs, base.Name "AddItemStore").
 *
 *   InvDefrmAddItem_Load          :307   -> init()
 *   ItemCatagoryHistoryFill       :347   -> categories into cmbItemCategory AND cmbitemcathistory
 *   ItemTypeFill                  :414   -> types into cmbItemType AND CmbItemTypeHistory
 *   RackNameFillFromGlobal        :445   -> "-- Select --" row (0) inserted and activated
 *   cmbItemCategory_Leave         :536   -> code + GL accounts, only while Save is the active button
 *   cmbItemType_Leave             :1690  -> code, ALWAYS (no mode check on the desktop)
 *   FormValidationForAddItem      :695   -> formValidation()  (also enforced on the server)
 *   BtnSave_Click                 :759   -> BtnSave_Click()
 *   btnupdate_Click               :896   -> btnupdate_Click()
 *   grdhistory_DoubleClick        :1010  -> openRecord()
 *   refresh                       :1160  -> refresh()
 *   btnnew_Click                  :1197  -> btnnew_Click()
 *   toolStripButton1_Click        :1216  -> toolStripButton1_Click()  (Refresh)
 *   grdfrmfill / gridsetting      :1254  -> history()
 *   tabCAddItem_SelectedIndexChanged :1342 -> tab('tabHistory') loads 50 rows
 *   InvDefrmAddItem_KeyDown       :1546  -> keyboard shortcuts
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon;
    var api = '/api/store/item-store';
    var L = null;                         // lookups
    var RecId = 0;
    var saveMode = true;                  // btnsave.Visible / btnupdate.Visible
    var refreshedLookups = false;         // toolStripButton1_Click ran (:1219 switches to the PM auto-COA key)
    var pics = { 1: null, 2: null };      // {id, upload:{name,base64}}
    var pendingSlot = 0;
    var currentTab = 'tabForm';
    var defaultsSeq = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : String(v); }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function col(r, k) { var v = C.ci(r, k); return v === null || v === undefined ? '' : v; }
    function say(m) { var e = $id('lblStatus'); if (e) e.textContent = m || ''; }
    function box(m) { if (m) window.alert(m); }
    function esc(s) { return C.esc(s); }
    function hasOption(sel, v) {
        return Array.prototype.some.call(sel.options, function (o) { return o.value === String(v); });
    }
    /** Combo.Value = x: selects x when listed, otherwise leaves the combo with no active row. */
    function setCombo(id, v) {
        var sel = $id(id); if (!sel) return;
        if (v !== null && v !== undefined && hasOption(sel, v)) sel.value = String(v);
        else if (hasOption(sel, '')) sel.value = '';
        else sel.selectedIndex = -1;
    }
    /** C# ToString("#,##0.###") / ("#,##0"). */
    function fmt(v, dp) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dp }); }
    function fmtDateTime(v) {                                           // "dd-MMM-yyyy hh:mm tt" (:1333)
        if (!v) return '';
        var d = new Date(v);
        if (isNaN(d.getTime())) return String(v);
        var m = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][d.getMonth()];
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return String(d.getDate()).padStart(2, '0') + '-' + m + '-' + d.getFullYear() + ' '
             + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }

    /** DropDownBind.BindDDL(..., ZeroIndex:false): no blank "select" row; the combo starts with none active. */
    function fill(id, rows, valueKey, textKey, textFn, blank) {
        var sel = $id(id); if (!sel) return;
        var keep = sel.value, hadRow = sel.selectedIndex >= 0 && keep !== '';
        /* blank: an empty option so the operator can clear the combo (UltraCombo text cleared ->
           Value null -> the filter / value is 0). It is never a data row. */
        var h = blank ? '<option value=""></option>' : '';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, valueKey)) + '">' + esc(textFn ? textFn(r) : col(r, textKey)) + '</option>';
        });
        sel.innerHTML = h;
        if (hadRow && hasOption(sel, keep)) sel.value = keep; else sel.selectedIndex = blank ? 0 : -1;
    }

    // ------------------------------------------------------------------ load :307

    function init() {
        $id('fileImage').addEventListener('change', onImagePicked);
        // Leave (designer :3074 / :3210): fires whenever focus leaves the combo, changed or not.
        $id('cmbItemCategory').addEventListener('blur', cmbItemCategory_Leave);
        $id('cmbItemType').addEventListener('blur', cmbItemType_Leave);
        bindKeys();
        say('Loading...');
        return C.getJson(api + '/lookups').then(function (d) {
            L = d || {};
            bindCategories(); bindTypes();                                         // :328-329
            bindRacks(true);                                                       // :330
            companiesBindInGrid(L.companies || []);                                // :331
            bindAccounts();                                                        // :332
            fill('cmbTaxType', L.taxes, 'Id', 'TaxName', null, true);              // :333
            fill('cmbBaseUnit', L.units, 'Id', 'UomCode');                         // :334
            bindPackSizes();                                                       // :335
            var f = L.features || {};
            $id('lblChkIsCompany').classList.toggle('is-hidden', !f.partyProcessing);      // :339-340
            $id('lblChkIsThirdParty').classList.toggle('is-hidden', !f.partyProcessing);
            $id('BtnAttributesAllocation').classList.toggle('is-hidden', !f.attributesForItem);   // :342
            saveMode = true;                                                       // :336-337
            applyRights();
            history(50);
            say('');
        }).catch(function (e) { say('Not loaded.'); box(e.message); });
    }

    function bindCategories() {
        fill('cmbItemCategory', L.categories, 'Id', 'CategoryDescription');
        fill('cmbitemcathistory', L.categories, 'Id', 'CategoryDescription', null, true);
    }
    function bindTypes() {
        fill('cmbItemType', L.types, 'Id', 'TypeDescription');
        fill('CmbItemTypeHistory', L.types, 'Id', 'TypeDescription', null, true);
    }
    function bindAccounts() {
        ['cmbPurchaseGL', 'cmbSaleGL', 'cmbCGSGL'].forEach(function (id) { fill(id, L.accounts, 'Id', 'AccountTitle'); });
    }
    /**
     * RackNameFillFromGlobal :445 - BindAndRetainSelection(..., previous Id, insertDefaultRow:true):
     * "-- Select --" (0) is inserted first; the previous rack is kept when still listed, otherwise
     * row 0 is activated. The warehouse column is shown next to the rack name. With no racks at
     * all nothing is bound (source.Rows.Count == 0 -> Text "", DataSource null): no "-- Select --",
     * no active row.
     */
    function bindRacks() {
        var sel = $id('CmbRackName');
        var prev = sel.selectedIndex >= 0 ? sel.value : '0';
        if (!(L.racks || []).length) { sel.innerHTML = ''; sel.selectedIndex = -1; return; }
        var h = '<option value="0">-- Select --</option>';
        (L.racks || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, 'Id')) + '">' + esc(col(r, 'RackName'))
               + (col(r, 'WarehouseName') ? ' — ' + esc(col(r, 'WarehouseName')) : '') + '</option>';
        });
        sel.innerHTML = h;
        sel.value = hasOption(sel, prev) ? prev : '0';
    }
    /** PackSizeIdForItem :660 - Rows[0].Activate(); a previous id is kept when still listed, else cleared. */
    function bindPackSizes() {
        var sel = $id('CmbPackSizeId');
        var prev = int(sel.value);
        fill('CmbPackSizeId', L.packSizes, 'Id', 'type', null, true);
        if (sel.options.length > 1) sel.selectedIndex = 1;                        // Rows[0] = first data row
        if (prev > 0) { if (hasOption(sel, prev)) sel.value = String(prev); else sel.selectedIndex = 0; }
    }

    /** CompaniesBindInGrid :615 - Id hidden, Location, Value (checkbox, all ticked, header selector). */
    function companiesBindInGrid(rows) {
        var h = '';
        rows.forEach(function (r) {
            h += '<tr><td>' + esc(col(r, 'Location')) + '</td>'
               + '<td style="text-align:center;"><input type="checkbox" class="alloc" data-id="' + esc(col(r, 'Id')) + '" checked></td></tr>';
        });
        $id('grdAllocation').querySelector('tbody').innerHTML = h;
        $id('chkAllocationHeader').checked = rows.length > 0;
    }
    function allocationHeader(on) {
        document.querySelectorAll('#grdAllocation input.alloc').forEach(function (c) { c.checked = on; });
    }

    // ------------------------------------------------------------------ rights / mode :314-316

    function applyRights() {
        var r = (L && L.rights) || {};
        var s = $id('btnsave'), u = $id('btnupdate');
        s.classList.toggle('is-hidden', !saveMode);
        u.classList.toggle('is-hidden', saveMode);
        s.disabled = !r.save;
        u.disabled = !r.update;
    }
    function isSaveButtonActive() { return saveMode && !!(L && L.rights && L.rights.save); }   // :553

    // ------------------------------------------------------------------ leave handlers

    function cmbItemCategory_Leave() {                                                 // :536
        if (!isSaveButtonActive()) return Promise.resolve();
        return defaults(true);
    }
    function cmbItemType_Leave() { return defaults(false); }                           // :1690 - always

    function defaults(withAccounts) {
        var seq = ++defaultsSeq;
        var q = C.qs({ categoryId: int(val('cmbItemCategory')), typeId: int(val('cmbItemType')), withAccounts: withAccounts });
        return C.getJson(api + '/defaults' + q).then(function (d) {
            if (seq !== defaultsSeq) return;                                              // a newer leave already ran
            d = d || {};
            if (d.ItemCode !== undefined) {                                               // GenerateItemCode :579
                setVal('txtItemCode', d.ItemCode);
                setVal('txtItemCodeNew', d.ItemCodeNew);
            }
            if (withAccounts && d.PurchaseGLAC !== undefined) {                           // SetGLAccounts :596
                setCombo('cmbPurchaseGL', int(d.PurchaseGLAC));
                setCombo('cmbSaleGL', int(d.SaleGLAC));
                setCombo('cmbCGSGL', int(d.COGSGLAC));
            }
        }).catch(function () { box('Record Not Found'); });
    }

    // ------------------------------------------------------------------ validation :695

    function hasRow(id) { var e = $id(id); return !!e && e.selectedIndex >= 0 && e.value !== ''; }
    function formValidation() {
        var f = (L && L.features) || {};
        var checks = [
            [function () { return val('txtItemName').trim() === ''; }, 'Item Name Field is Required', 'txtItemName'],
            [function () { return !hasRow('cmbBaseUnit'); }, 'Base Unit Field is Required', 'cmbBaseUnit'],
            [function () { return !hasRow('cmbItemCategory'); }, 'Item Category Field is Required', 'cmbItemCategory'],
            [function () { return !hasRow('cmbItemType'); }, 'Item Type Field is Required', 'cmbItemType'],
            [function () { return f.partyProcessing && !$id('ChkIsCompany').checked && !$id('ChkIsThirdParty').checked; },
                "Please Check of the Check box ('Is Company' Or 'Is third Party') ", 'ChkIsCompany'],
            [function () { return num(val('txtMinStock').trim()) > num(val('txtMaxStock').trim()); }, 'Min stock level cannot be greater than max stock level.', null],
            [function () { return !hasRow('CmbRackName'); }, 'Rack Name Field is Required', 'CmbRackName'],
            [function () { return !hasRow('cmbPurchaseGL'); }, 'Stock A/c Field is Required', 'cmbPurchaseGL'],
            [function () { return !hasRow('cmbSaleGL'); }, 'Sale A/c Field is Required', 'cmbSaleGL'],
            [function () { return !hasRow('cmbCGSGL'); }, 'CGS A/c Filed is Required', 'cmbCGSGL']
        ];
        for (var i = 0; i < checks.length; i++) {
            if (checks[i][0]()) { box(checks[i][1]); if (checks[i][2]) $id(checks[i][2]).focus(); return false; }
        }
        return true;
    }

    // ------------------------------------------------------------------ save / update

    function payload(isUpdate) {
        var images = [];
        [1, 2].forEach(function (slot) {
            var p = pics[slot];
            if (p) images.push({ sortNo: slot, id: p.id || 0, upload: p.upload || null });
        });
        var companies = [];
        document.querySelectorAll('#grdAllocation input.alloc').forEach(function (c) {
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
            RackId: hasRow('CmbRackName') ? int(val('CmbRackName')) : null,
            MinStockLevel: val('txtMinStock'),
            MaxStockLevel: val('txtMaxStock'),
            ReorderLevel: val('txtReOrderLevel'),
            ReOrderQty: val('txtReOrderQty'),
            LeadTimeDay: val('txtLeadTimeDay'),
            PackSize: val('txtPackSize'),
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
            RefreshedLookups: refreshedLookups,
            companies: companies,
            images: images
        };
    }

    function BtnSave_Click() {                                                         // :759
        if (!saveMode || $id('btnsave').disabled) return;
        if (!formValidation()) return;
        say('Saving...');
        return C.postJson(api + '/save', payload(false)).then(function (d) {
            say('');
            box(d && d.message);                                                          // "Save Successfully" when >= 1
            return refresh();
        }).catch(function (e) { say(''); box(e.message); });
    }

    function btnupdate_Click() {                                                       // :896
        if (saveMode || $id('btnupdate').disabled) return;
        if (!formValidation()) return;
        say('Updating...');
        return C.postJson(api + '/save', payload(true)).then(function (d) {
            say('');
            box(d && d.message);                                                          // "Update Successfully"
            return refresh();
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ refresh / new

    /**
     * refresh :1160 - clears only what the desktop clears. Not cleared (kept as they are): Item
     * Category, Item Type, Base UOM, Rack, Stock A/c, ReOrder Qty, LeadTime Day, Pack Size, Pack
     * Size id, Tax Type, Local/Import, Company/Third Party, Status. Sale / CGS are cleared and the
     * category leave then puts the category's GL accounts back.
     */
    function refresh() {
        $id('cmbItemCategory').disabled = false;                                          // ReadOnly = false
        RecId = 0;
        $id('lblRecId').innerHTML = '&nbsp;';
        $id('BrandImages').classList.remove('is-hidden');
        saveMode = true;
        applyRights();
        $id('cmbCGSGL').selectedIndex = -1;
        $id('cmbSaleGL').selectedIndex = -1;
        setVal('txtMinStock', ''); setVal('txtMaxStock', ''); setVal('txtReOrderLevel', '');
        setVal('txtItemCode', ''); setVal('txtBarcodeNo', ''); setVal('txtItemName', '');
        $id('chbGST').checked = false;
        $id('cmbItemCategory').focus();
        var p = cmbItemCategory_Leave();                                                  // :1183
        resetPic(1); resetPic(2);
        return p;
    }

    function btnnew_Click() {                                                          // :1197
        var p = refresh();
        $id('cmbBaseUnit').selectedIndex = -1;
        $id('cmbItemType').selectedIndex = -1;
        return p;
    }

    /**
     * toolStripButton1_Click :1216 - reloads the auto-COA switch (from the PM key), rebinds Item Type,
     * Item Category (entry + history), Racks, the three GL combos, Tax Type and Pack Size. Base UOM
     * and the allocation grid are not rebound.
     */
    function toolStripButton1_Click() {
        refreshedLookups = true;
        return C.getJson(api + '/lookups').then(function (d) {
            d = d || {};
            L.types = d.types; L.categories = d.categories; L.racks = d.racks;
            L.accounts = d.accounts; L.taxes = d.taxes; L.packSizes = d.packSizes;
            bindTypes(); bindCategories(); bindRacks(); bindAccounts();
            fill('cmbTaxType', L.taxes, 'Id', 'TaxName', null, true);
            bindPackSizes();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history :1254

    var HIST_COLS = ['ItemCode', 'ItemCodeNew', 'ItemName', 'TypeDescription', 'CategoryDescription', 'ItemStatus',
                     'StockAc', 'SaleAc', 'CgsAc', 'EntryDate', 'EntryUserName', 'NoOfAttachments', 'LeadTimeDay'];

    function history(noOfRecords) {
        var q = C.qs({ noOfRecords: noOfRecords || 0, categoryId: int(val('cmbitemcathistory')), typeId: int(val('CmbItemTypeHistory')) });
        return C.getJson(api + '/history' + q).then(function (rows) {
            renderHistory(rows || []);
        }).catch(function (e) { box(e.message); });
    }

    /** gridsetting :1312 - Id, ParentCategory and ClassGroupName hidden; captions are the column keys. */
    function renderHistory(rows) {
        var t = $id('grdhistory');
        var tb = t.querySelector('tbody');
        $id('lblHistoryCount').textContent = rows.length ? '(' + rows.length + ')' : '';
        if (!rows.length) {                                                               // ClearStructure
            t.querySelector('thead').innerHTML = '';
            tb.innerHTML = '';
            return;
        }
        t.querySelector('thead').innerHTML = '<tr>' + HIST_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        tb.innerHTML = rows.map(function (r) {
            var id = int(col(r, 'Id'));
            return '<tr data-id="' + id + '">' + HIST_COLS.map(function (c) {
                if (c === 'ItemCode') return '<td><a href="#" class="is-open-item" data-id="' + id + '">' + esc(col(r, c)) + '</a></td>';
                if (c === 'EntryDate') return '<td>' + esc(fmtDateTime(col(r, c))) + '</td>';
                if (c === 'NoOfAttachments' || c === 'LeadTimeDay') return '<td class="num">' + esc(int(col(r, c))) + '</td>';
                return '<td>' + esc(col(r, c)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        tb.querySelectorAll('tr[data-id]').forEach(function (tr) {
            tr.addEventListener('dblclick', function () { openRecord(int(tr.getAttribute('data-id'))); });
        });
        tb.querySelectorAll('a.is-open-item').forEach(function (a) {
            a.addEventListener('click', function (e) { e.preventDefault(); openRecord(int(a.getAttribute('data-id'))); });
        });
    }

    function btnsearch_Click() { return history(0); }                                  // :1249
    function btnLoadAll_Click() { return history(0); }                                 // :1350

    // ------------------------------------------------------------------ open :1010

    function openRecord(id) {
        if (!id) return Promise.resolve();
        say('Opening...');
        return C.getJson(api + '/' + id).then(function (d) {
            var it = (d && d.item) || {};
            saveMode = false;                                                             // :1015-1016
            applyRights();
            RecId = id;
            $id('lblRecId').textContent = 'Item ' + (col(it, 'ItemCodeNew') || id);
            tab('tabForm');
            var f = (L && L.features) || {};
            if (f.partyProcessing) {
                $id('ChkIsThirdParty').checked = !!col(it, 'IsThirdParty');
                $id('ChkIsCompany').checked = !!col(it, 'IsCompany');
            }
            setVal('txtItemCode', col(it, 'ItemCode'));
            setVal('txtItemCodeNew', col(it, 'ItemCodeNew'));
            setVal('txtBarcodeNo', col(it, 'BarcodeNo'));
            setVal('txtItemName', col(it, 'ItemName'));
            setCombo('cmbBaseUnit', int(col(it, 'BaseUnitId')));
            setCombo('cmbItemCategory', int(col(it, 'ItemCategoryId')));
            $id('cmbItemCategory').disabled = true;                                        // ReadOnly :1034
            setCombo('cmbItemType', int(col(it, 'ItemTypeId')));
            $id('chkstatus').checked = !!col(it, 'ItemStatus');
            setCombo('CmbRackName', int(col(it, 'RackId')));
            setCombo('cmbPurchaseGL', int(col(it, 'PurchaseGLAC')));
            setCombo('cmbSaleGL', int(col(it, 'SaleGLAC')));
            setCombo('cmbCGSGL', int(col(it, 'COGSGLAC')));
            setVal('txtMinStock', fmt(col(it, 'MinStockLevel'), 3));
            setVal('txtMaxStock', fmt(col(it, 'MaxStockLevel'), 3));
            setVal('txtReOrderLevel', fmt(col(it, 'ReorderLevel'), 3));
            setVal('txtLeadTimeDay', fmt(col(it, 'LeadTimeDay'), 0));
            setVal('txtPackSize', fmt(col(it, 'PackSize'), 0));
            setCombo('CmbPackSizeId', int(col(it, 'PackSizeId')));
            $id('chkAllowMultiUom').checked = !!col(it, 'AllowMultiUom');
            $id('chbGST').checked = !!col(it, 'ApplyGST');
            // Not restored by the desktop: ReOrder Qty, Local/Import, Tax Type.
            resetPic(1); resetPic(2);                                                     // web deviation: slots cleared first
            ((d && d.images) || []).forEach(function (img) {
                var slot = int(col(img, 'SortNo'));
                if (slot !== 1 && slot !== 2) return;
                var imgId = int(col(img, 'Id'));
                pics[slot] = { id: imgId };
                $id('picbox' + slot).innerHTML = '<img alt="" src="' + esc(api + '/' + id + '/images/' + imgId) + '">';
            });
            say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ images :1396 / :1652

    function browse(slot) { pendingSlot = slot; var f = $id('fileImage'); f.value = ''; f.click(); }
    function onImagePicked(e) {
        var file = e.target.files && e.target.files[0];
        if (!file || !pendingSlot) return;
        if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }   // DAL :396
        var slot = pendingSlot;
        var reader = new FileReader();
        reader.onload = function () {
            var data = String(reader.result);
            pics[slot] = { id: 0, upload: { name: file.name, base64: data.substring(data.indexOf(',') + 1) } };
            $id('picbox' + slot).innerHTML = '<img alt="" src="' + esc(data) + '">';
        };
        reader.readAsDataURL(file);
    }
    function resetPic(slot) { pics[slot] = null; $id('picbox' + slot).innerHTML = ''; }

    // ------------------------------------------------------------------ tabs + keys

    /** tabCAddItem_SelectedIndexChanged :1342 - switching to History loads the latest 50. */
    function tab(name) {
        var el = $id(name);
        if (el) el.scrollIntoView({ behavior: 'smooth' });
    }

    /** InvDefrmAddItem_KeyDown :1546 */
    function bindKeys() {
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (saveMode && currentTab === 'tabForm') BtnSave_Click(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew_Click(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); tab(currentTab === 'tabHistory' ? 'tabForm' : 'tabHistory'); return; }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!saveMode) btnupdate_Click(); return; }
            if (e.ctrlKey && k === 'e') { e.preventDefault(); window.location.href = '/modules'; return; }
            if (k === 'enter' && e.target && e.target.tagName === 'INPUT' && e.target.type !== 'file') {
                e.preventDefault(); nextField(e.target);                                  // SendKeys("{TAB}")
            }
        });
        // CommonServices.OnlytextdecimelFunction / OnlytextNumberFunction (:1592-1650)
        document.querySelectorAll('input[data-keys]').forEach(function (inp) {
            inp.addEventListener('keypress', function (e) {
                if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
                var decimal = inp.getAttribute('data-keys') === 'decimal';
                if (/\d/.test(e.key)) return;
                if (decimal && e.key === '.' && inp.value.indexOf('.') < 0) return;
                e.preventDefault();
            });
        });
    }
    function nextField(el) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, button, textarea'), function (x) {
            return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null && x.type !== 'hidden';
        });
        var i = all.indexOf(el);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }

    window.ItemStore = {
        init: init, tab: tab,
        btnnew_Click: btnnew_Click, toolStripButton1_Click: toolStripButton1_Click,
        BtnSave_Click: BtnSave_Click, btnupdate_Click: btnupdate_Click,
        btnsearch_Click: btnsearch_Click, btnLoadAll_Click: btnLoadAll_Click,
        allocationHeader: allocationHeader, browse: browse, resetPic: resetPic, openRecord: openRecord
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
