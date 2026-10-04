/* ============================================================================================
 * Production dropdowns in the STORE design - the Transporter combo on /store/grn-store.
 *
 * Production pages only. Loaded INSTEAD of countx_desktop_combo.js (which Sale, Purchase and CMAGT
 * keep using unchanged) and exposes the same window.DesktopCombo API - init, enhance, refresh,
 * define, families, instances - so every production page's DesktopCombo.define(...) and
 * DesktopCombo.refresh() calls keep working without an edit.
 *
 * What the store design is (countx_store_common.js "Searchable combos" + countx_store.css .cx-combo-*):
 *   - the field is a plain combo box: it shows the chosen text and cannot be typed into;
 *   - a click, F4, Alt+Down, Space or any printable key opens a popup whose FIRST row is a yellow
 *     "Search..." box (a printable key is pre-typed into it);
 *   - one column: rows whose text STARTS WITH the search first, then the ones that CONTAIN it;
 *     many columns: a table with a sticky light-blue header, filtered on CONTAINS over every
 *     column, list order kept, the empty "nothing selected" slot not drawn;
 *   - the active row is teal with white text, hover is light blue; at most 500 rows are drawn;
 *   - Up/Down move, Enter picks, Tab picks the active row (or just closes), Esc closes and puts
 *     focus back on the field; a scroll outside the popup or a window resize closes it.
 *
 * What is kept from the production component, because the production pages depend on it:
 *   - the native <select> stays hidden and authoritative; a pick writes it and fires a bubbling
 *     `input` + `change` only when the value changed;
 *   - option rebuilds, `.value =` / `.selectedIndex =` and jQuery .val() refresh the field;
 *   - the .dtcombo-wrap / .dtcombo-input / .dtcombo-pop markup, because page code focuses
 *     `wrap .dtcombo-input`, tests `.dtcombo-pop[style*="block"]` and treats focus inside the wrap
 *     or the popup as "still in the combo" (the UltraCombo Leave events). The popup therefore lives
 *     INSIDE the wrap, so moving focus into its search box is not a Leave;
 *   - 2026-10-02: a CHECKED multi-select mode, opt-in only: <select multiple data-dtcombo="single"
 *     data-dtcombo-checked> (the desktop's checked UltraCombo - see CheckedCombo below).
 *   - the column families (data-dtcombo="prJobOrder" etc.) for the multi-column combos, hidden
 *     options, disabled options, rows added to the document later.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var doc = global.document;
    var INSTANCES = [];
    var openInstance = null;
    var seq = 0;
    var MAX_ROWS = 500;

    /* 2026-10-03 (additive): CHECKED-ONLY companion mode, for a page that keeps ANOTHER dropdown
       library for its single combos (countx_desktop_combo.js on Sale / Purchase / CMAGT, select2 on
       the dashboards) and loads this file only for its checked multi-selects. Switched on by
       <script src=".../countx_prod_combo.js" data-dtcombo-checked-only>, by window.DTCOMBO_CHECKED_ONLY,
       or automatically when countx_desktop_combo.js already defined window.DesktopCombo. In this mode
       only <select multiple data-dtcombo-checked> is enhanced, nothing else is claimed, and the existing
       window.DesktopCombo is left alone (this API is window.DesktopCheckedCombo). A page that loads this
       file on its own behaves exactly as before. */
    var CUR_SCRIPT = doc.currentScript;
    var CHECKED_ONLY = !!(global.DTCOMBO_CHECKED_ONLY
        || (CUR_SCRIPT && CUR_SCRIPT.hasAttribute && CUR_SCRIPT.hasAttribute('data-dtcombo-checked-only'))
        || (global.DesktopCombo && global.DesktopCombo.design !== 'store'));
    var CHECKED_SELECTOR = 'select[multiple][data-dtcombo-checked]';

    function esc(v) {
        return String(v === undefined || v === null ? '' : v)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    function truthy(v) {
        if (v === undefined || v === null || v === '') return false;
        var s = String(v).trim().toLowerCase();
        return s === '1' || s === 'true' || s === 'yes' || s === 'y';
    }

    /* The column families, copied from countx_desktop_combo.js so a production combo keeps the
       columns its desktop form draws. type: 'text' | 'num' (right-aligned) | 'check' (a tick). */
    var FAMILIES = {
        /* PurchsaeOrder.cs - dtSupplier with GlAccountId and CityId hidden (:1031-1032). */
        party4: [
            { caption: 'Name',      flex: 3 },
            { caption: 'PartyCode', flex: 2, key: 'code' },
            { caption: 'CityName',  flex: 2, key: 'city' },
            { caption: 'MobileNo',  flex: 2, key: 'mobile' }
        ],
        /* SaleOrder.cs - CompanyNameBind, cols 3 (GlAccountId) and 4 (CityId) hidden. */
        party3: [
            { caption: 'Name',      flex: 3 },
            { caption: 'PartyCode', flex: 2, key: 'code' },
            { caption: 'CityName',  flex: 2, key: 'city' }
        ],
        /* dtitem cols 3, 4, 6, 8 hidden -> five visible. Same on both forms. */
        item: [
            { caption: 'Item',             flex: 4 },
            { caption: 'ItemCode',         flex: 2, key: 'item-code' },
            { caption: 'ItemCategory',     flex: 3, key: 'item-category' },
            { caption: 'ItemType',         flex: 2, key: 'item-type' },
            { caption: 'ProductionStage',  flex: 2, key: 'production-stage' }
        ],
        /* PurchsaeOrder.cs - dtUOM col 2 (Equivalent) hidden (:1396, :1398).
           Note :1388 populates the column NAMED "UOM" from UOMCode and the one named "UOMCode"
           from Equivalent, so reading the column names rather than the row population would show
           the wrong data. */
        uom2: [
            { caption: 'UOM',         flex: 3 },
            { caption: 'BaseRateUom', flex: 2, key: 'base', type: 'check' }
        ],
        /* SaleOrder.cs - all four visible. */
        uom4: [
            { caption: 'UOM',           flex: 2 },
            { caption: 'Equivalent',    flex: 2, key: 'eq',     type: 'num' },
            { caption: 'QtyEquivalent', flex: 2, key: 'qty-eq', type: 'num' },
            { caption: 'BaseRateUom',   flex: 2, key: 'base',   type: 'check' }
        ],
        /* CMAGT - CommonBindings.ItemdtFillFromGlobal builds a SIX column dtItem
           (Id, ItemName, ItemCode, InventoryParentCategoriesId, ItemCategoryId, ItemCategory) and
           ItemNameBind hides columns 3 and 4, leaving three. NOT the five-column `item` family
           the Purchase and Sale Order forms use - their dtitem carries ItemType and
           ProductionStage as well, which the CMAGT table does not even have. */
        itemCmagt: [
            { caption: 'Item',         flex: 4 },
            { caption: 'ItemCode',     flex: 2, key: 'item-code' },
            { caption: 'ItemCategory', flex: 3, key: 'item-category' }
        ],

        /* CMAGT - CommonServices.dtUomFromGloablUomScheduleByItemId (:2159) builds
           Id, UOMCode, Equivalent, BaseRateUom, BasePackUom [, BaseSecondaryUom], and
           CommonBindings.ItemUomFromGlobalBind (:201-217) binds it with AllColumns:true and hides
           NOTHING - so unlike Purchase Order's two columns and Sale Order's four, this one shows
           Equivalent AND both base flags. Caption follows the combo: Pack Uom / Rate Uom /
           Secondary Uom. */
        uomCmagt: [
            { caption: 'Uom',         flex: 3 },
            { caption: 'Equivalent',  flex: 2, key: 'eq',        type: 'num' },
            { caption: 'BaseRateUom', flex: 2, key: 'base',      type: 'check' },
            { caption: 'BasePackUom', flex: 2, key: 'base-pack', type: 'check' }
        ],

        /* InvFrmGDN.cs TransporterAcFill (:970-982) - dtTransporter is
           Id, AccountTitle, AccountCode, SupplierCustomerId and column 3 is hidden, leaving two.
           A chart-of-accounts row, NOT a party row, so it does not share the party families. */
        transporter: [
            { caption: 'Transporter Name', flex: 3 },
            { caption: 'AccountCode',      flex: 2, key: 'code' }
        ],

        /* Grid-cell combos on the Purchase Order tabs. Both are two-column on the desktop:
           the title, then its code. They live inside rows built by "+ Add Row", which is why
           they need the document observer below rather than the initial scan. */
        account2: [
            { caption: 'Account Title', flex: 4 },
            { caption: 'AccountCode',   flex: 2, key: 'code' }
        ],
        term2: [
            { caption: 'Payment Term', flex: 4 },
            { caption: 'Due Days',     flex: 2, key: 'code', type: 'num' }
        ],

        /* ProductionSummaryReport.cs:284 - InfragisticsHelper.BindAndRetainSelection(..., AllColumns:true),
           so the desktop drop grid shows EVERY column
           USP_InvProductionJobOrder_GetJobOrderNoWithInfo returns. That full list was read off a
           live response rather than guessed: Id plus the eleven below. Id is the value member and
           is not drawn, exactly as a bound value column is not drawn on the desktop. */
        jobOrderInfo: [
            { caption: 'JobOrderNo',      flex: 2 },
            { caption: 'JobOrderDate',    flex: 2, key: 'order-date' },
            { caption: 'JobOrderDocNo',   flex: 1, key: 'doc-no',          type: 'num' },
            { caption: 'JobStartDate',    flex: 2, key: 'start-date' },
            { caption: 'JobOrderStatus',  flex: 2, key: 'job-status' },
            { caption: 'SettledStatus',   flex: 2, key: 'settled-status' },
            { caption: 'SettlementDate',  flex: 2, key: 'settlement-date' },
            { caption: 'ApprovalStatus',  flex: 2, key: 'approval-status' },
            { caption: 'ApprovedDate',    flex: 2, key: 'approved-date' },
            { caption: 'LotCode',         flex: 2, key: 'lot-code' }
        ],

        /* PurchsaeOrder.cs BookingPersonFill - ReferenceParties bound through
           InfragisticsHelper.BindAndRetainSelection(..., "Id", "ReferencePartyName",
           "Booking Person", AllColumns: true, insertDefaultRow: true). The procedure returns
           Id, ReferencePartyName and ReferencePartyType, so with Id hidden the desktop draws
           exactly TWO columns - not the four of the party families, which describe dtSupplier
           and have no counterpart here (no PartyCode, no CityName, no MobileNo). */
        refParty2: [
            { caption: 'Booking Person',     flex: 3 },
            { caption: 'ReferencePartyType', flex: 2, key: 'code' }
        ],

        /* CMAGT - CommonBindings.ItemUomFromGlobalBind (:201-217) calls
           dtUomFromGloablUomScheduleByItemId(itemId, addBaseSecondaryUom: TRUE), so the bound
           table is Id, UOMCode, Equivalent, BaseRateUom, BasePackUom, BaseSecondaryUom and,
           with nothing hidden, FIVE columns show. (uomCmagt above stops at four.) */
        uomCmagt5: [
            { caption: 'Uom',              flex: 3 },
            { caption: 'Equivalent',       flex: 2, key: 'eq',             type: 'num' },
            { caption: 'BaseRateUom',      flex: 2, key: 'base',           type: 'check' },
            { caption: 'BasePackUom',      flex: 2, key: 'base-pack',      type: 'check' },
            { caption: 'BaseSecondaryUom', flex: 2, key: 'base-secondary', type: 'check' }
        ],

        /* CMAGT - CommonBindings.BindShipToAddressAgainstBuyer (:296-321): Id, Address,
           PartyId, PartyName with Columns[2] (PartyId) hidden -> Address | PartyName. */
        shipTo2: [
            { caption: 'Ship To Address', flex: 4 },
            { caption: 'PartyName',       flex: 3, key: 'party-name' }
        ],

        /* frmBuyerInquiryBooking AnalysisGroupFill (:903-912): Id, AnalysisGroupDescription,
           GroupTypeId, GroupType, nothing hidden. The desktop fills the column NAMED GroupTypeId
           with dtLAG.GroupType and the one NAMED GroupType with InvParentCateDescription; the
           captions are kept as drawn, the data as populated. */
        analysisGroup3: [
            { caption: 'Analysis Group', flex: 3 },
            { caption: 'GroupTypeId',    flex: 2, key: 'group-type' },
            { caption: 'GroupType',      flex: 2, key: 'parent-category' }
        ],

        /* CMAGT Tax Name - TaxTypeBind binds Sp_ItemTaxSchedule_GetAllMethod
           'GetItemTaxScheduleForItemId' (TaxNameId, TaxName, TaxScheduleId, EffectedDate,
           TaxPercent, TaxGLAccountId) with AllColumns:true and nothing hidden -> five columns. */
        taxCmagt: [
            { caption: 'Tax Name',       flex: 3 },
            { caption: 'TaxScheduleId',  flex: 1, key: 'tax-schedule-id', type: 'num' },
            { caption: 'EffectedDate',   flex: 2, key: 'effected-date' },
            { caption: 'TaxPercent',     flex: 1, key: 'tax-percent',     type: 'num' },
            { caption: 'TaxGLAccountId', flex: 1, key: 'tax-gl-account',  type: 'num' }
        ],

        /* One captioned column - the desktop still draws a header band over these. */
        single: [ { caption: '', flex: 1 } ]
    };

    function columnsFor(sel) {
        var fam = FAMILIES[sel.getAttribute('data-dtcombo')] || FAMILIES.single;
        /* The store format also works: data-columns="A|B|C" on the select, data-extra="b|c" on
           each option (StoreCommon.fillSelectCols). */
        var storeCols = sel.getAttribute('data-columns');
        if (storeCols && !sel.getAttribute('data-dtcombo')) {
            return storeCols.split('|').filter(Boolean).map(function (c, i) {
                return { caption: c, flex: 1, extra: i };
            });
        }
        var caption = sel.getAttribute('data-dtcombo-caption');
        if (!caption) return fam;
        var copy = fam.slice();
        copy[0] = { caption: caption, flex: fam[0].flex, key: fam[0].key, type: fam[0].type };
        return copy;
    }

    // ------------------------------------------------------------------ one control

    function Combo(sel) {
        this.sel = sel;
        this.id = 'pcx' + (++seq);
        this.cols = columnsFor(sel);
        this.rows = [];
        this.filtered = [];
        this.active = -1;
        this.open = false;
        this.build();
        this.syncFromSelect();
        this.watch();
    }

    Combo.prototype.multi = function () { return this.cols.length > 1; };

    Combo.prototype.build = function () {
        var sel = this.sel;

        /* Read the select's placement BEFORE it is moved into the wrap. */
        var attrStyle = sel.getAttribute('style') || '';
        function inline(prop) {
            return sel.style[prop] || (attrStyle.match(new RegExp('(?:^|;)\\s*' + prop + '\\s*:\\s*([^;]+)', 'i')) || [])[1];
        }
        var inlineLeft = inline('left'), inlineTop = inline('top'), inlineRight = inline('right');
        var inlineBottom = inline('bottom'), inlineWidth = inline('width'), inlineHeight = inline('height');
        var inlinePos = inline('position');
        var cs = global.getComputedStyle ? global.getComputedStyle(sel) : null;
        var isAbsolute = (inlinePos === 'absolute') || (cs && cs.position === 'absolute') || (!!inlineLeft && !!inlineTop);

        sel.classList.add('dtcombo-native');
        sel.setAttribute('aria-hidden', 'true');
        sel.setAttribute('tabindex', '-1');

        var wrap = doc.createElement('div');
        wrap.className = ('dtcombo-wrap pcx-wrap ' + sel.className.replace('dtcombo-native', '')).trim();
        if (isAbsolute) {
            wrap.style.position = 'absolute';
            if (inlineLeft) wrap.style.left = inlineLeft.trim();
            if (inlineTop) wrap.style.top = inlineTop.trim();
            if (inlineRight) wrap.style.right = inlineRight.trim();
            if (inlineBottom) wrap.style.bottom = inlineBottom.trim();
        } else if (inlinePos) {
            wrap.style.position = inlinePos.trim();
        }
        if (inlineWidth) wrap.style.width = inlineWidth.trim();
        if (inlineHeight) wrap.style.height = inlineHeight.trim();

        /* The field: shows the chosen text, not typed into (a <select> in the store design). */
        var input = doc.createElement('input');
        input.type = 'text';
        input.readOnly = true;
        input.className = 'dtcombo-input pcx-field ' + (sel.getAttribute('data-dtcombo-class') || '');
        input.setAttribute('autocomplete', 'off');
        input.setAttribute('role', 'combobox');
        input.setAttribute('aria-expanded', 'false');
        input.placeholder = sel.getAttribute('data-dtcombo-placeholder') || '';

        var caret = doc.createElement('span');
        caret.className = 'dtcombo-caret pcx-caret';

        wrap.appendChild(input);
        wrap.appendChild(caret);
        sel.parentNode.insertBefore(wrap, sel);
        wrap.appendChild(sel);

        /* The popup - after the field in document order, so wrap.querySelector('input') still
           finds the field. Inside the wrap, so focus in its search box is still "in the combo". */
        var pop = doc.createElement('div');
        pop.className = 'dtcombo-pop cx-combo-pop pcx-pop';
        pop.style.display = 'none';
        pop.setAttribute('role', 'listbox');
        pop.innerHTML = '<input type="text" class="cx-combo-search" placeholder="Search..." autocomplete="off" tabindex="-1">'
                      + '<div class="cx-combo-list"></div>';
        wrap.appendChild(pop);

        this.wrap = wrap;
        this.input = input;
        this.caret = caret;
        this.pop = pop;
        this.search = pop.querySelector('.cx-combo-search');
        this.list = pop.querySelector('.cx-combo-list');

        this.bindEvents();
        this.reflectDisabled();
    };

    /* Reads the <option> list into rows. */
    Combo.prototype.readOptions = function () {
        var rows = [];
        var opts = this.sel.options;
        for (var i = 0; i < opts.length; i++) {
            var o = opts[i];
            var extra = null;
            var cells = [];
            for (var c = 0; c < this.cols.length; c++) {
                var col = this.cols[c];
                if (c === 0) { cells.push(o.textContent); continue; }
                var v = null;
                if (col.extra !== undefined) {
                    if (extra === null) { var e = o.getAttribute('data-extra'); extra = e ? e.split('|') : []; }
                    v = extra[col.extra - 1];
                } else if (col.key) {
                    v = o.getAttribute('data-' + col.key);
                }
                cells.push(v === null || v === undefined ? '' : v);
            }
            rows.push({
                opt: o, index: i, value: o.value, text: o.textContent,
                cells: cells, disabled: o.disabled,
                hidden: o.hidden || o.style.display === 'none'
            });
        }
        this.rows = rows;
    };

    var NATIVE_SELECT_DESC = (function () {
        var out = {};
        try {
            var proto = global.HTMLSelectElement && global.HTMLSelectElement.prototype;
            if (proto) {
                out.value = Object.getOwnPropertyDescriptor(proto, 'value');
                out.selectedIndex = Object.getOwnPropertyDescriptor(proto, 'selectedIndex');
            }
        } catch (e) { /* the hook then simply does not install */ }
        return out;
    })();

    Combo.prototype.syncFromSelect = function () {
        if (this.__inSync) return;
        this.__inSync = true;
        try {
            this.cols = columnsFor(this.sel);
            this.readOptions();
            var o = this.sel.selectedIndex >= 0 ? this.sel.options[this.sel.selectedIndex] : null;
            this.input.value = o ? o.textContent.trim() : '';
            this.input.title = this.input.value;
        } finally { this.__inSync = false; }
    };

    Combo.prototype.reflectDisabled = function () {
        var d = this.sel.disabled;
        this.input.disabled = d;
        this.wrap.classList.toggle('dtcombo-disabled', !!d);
        if (d && this.open) this.closePop(false);
    };

    // ------------------------------------------------------------------ the store popup

    /* countx_store_common.js renderPop, over this combo's rows. */
    Combo.prototype.applyFilter = function (q) {
        q = String(q || '').toLowerCase().trim();
        var cols = this.cols, out = [];
        if (this.multi()) {
            /* multi-column: contains over every column, list order kept */
            for (var i = 0; i < this.rows.length && out.length < MAX_ROWS; i++) {
                var r = this.rows[i];
                if (r.hidden) continue;
                if ((r.value === '0' || r.value === '') && !String(r.text).trim()) continue;   /* the "nothing selected" slot */
                if (!q) { out.push(r); continue; }
                for (var c = 0; c < r.cells.length; c++) {
                    if (cols[c] && cols[c].type === 'check') continue;
                    if (String(r.cells[c]).toLowerCase().indexOf(q) >= 0) { out.push(r); break; }
                }
            }
        } else {
            /* one column: starts-with first, then contains */
            var starts = [], contains = [];
            for (var j = 0; j < this.rows.length; j++) {
                var s = this.rows[j];
                if (s.hidden) continue;
                var t = String(s.text).toLowerCase();
                if (!q || t.indexOf(q) === 0) starts.push(s); else if (t.indexOf(q) >= 0) contains.push(s);
            }
            out = starts.concat(contains).slice(0, MAX_ROWS);
        }
        this.filtered = out;
        this.active = -1;
        var cur = this.sel.value;
        for (var k = 0; k < out.length; k++) if (out[k].value === cur && !out[k].disabled) { this.active = k; break; }
        if (this.active < 0 && q && out.length) this.active = 0;
    };

    Combo.prototype.cellHtml = function (c, v) {
        if (c.type === 'check') return '<td class="pcx-check"><input type="checkbox" disabled' + (truthy(v) ? ' checked' : '') + '></td>';
        return '<td' + (c.type === 'num' ? ' class="pcx-num"' : '') + '>' + (v !== '' && v !== null && v !== undefined ? esc(v) : '&nbsp;') + '</td>';
    };

    Combo.prototype.render = function () {
        var self = this, rows = this.filtered, cols = this.cols;
        this.pop.classList.toggle('is-cols', this.multi());
        if (!rows.length) { this.list.innerHTML = '<div class="cx-combo-empty">No match</div>'; return; }
        var h;
        if (this.multi()) {
            h = '<table class="cx-combo-table"><thead><tr>'
              + cols.map(function (c) { return '<th' + (c.type === 'num' ? ' class="pcx-num"' : '') + '>' + esc(c.caption) + '</th>'; }).join('')
              + '</tr></thead><tbody>'
              + rows.map(function (r, i) {
                    return '<tr class="cx-combo-item' + (i === self.active ? ' is-active' : '') + (r.disabled ? ' is-disabled' : '')
                         + '" data-i="' + i + '">' + cols.map(function (c, j) { return self.cellHtml(c, r.cells[j]); }).join('') + '</tr>';
                }).join('')
              + '</tbody></table>';
        } else {
            h = rows.map(function (r, i) {
                return '<div class="cx-combo-item' + (i === self.active ? ' is-active' : '') + (r.disabled ? ' is-disabled' : '')
                     + '" data-i="' + i + '">' + (String(r.text) ? esc(r.text) : '&nbsp;') + '</div>';
            }).join('');
        }
        this.list.innerHTML = h;
        var a = this.list.querySelector('.is-active'); if (a) a.scrollIntoView({ block: 'nearest' });
    };

    Combo.prototype.move = function (d) {
        var n = this.filtered.length;
        if (!n) return;
        this.active = Math.max(0, Math.min(n - 1, this.active + d));
        var items = this.list.querySelectorAll('.cx-combo-item');
        for (var i = 0; i < items.length; i++) items[i].classList.toggle('is-active', i === this.active);
        var a = items[this.active]; if (a) a.scrollIntoView({ block: 'nearest' });
    };

    /* Below the field, flipped above when it would run off the bottom (store openPop). Fixed, so a
       panel's overflow never clips it; corrected afterwards in case an ancestor with a transform
       (the centred dialogs) makes "fixed" relative to that ancestor instead of the viewport. */
    Combo.prototype.position = function () {
        var r = this.wrap.getBoundingClientRect();
        var pop = this.pop;
        pop.style.minWidth = Math.min(Math.max(r.width, this.multi() ? 560 : 220), global.innerWidth - 16) + 'px';
        var left = r.left, top = r.bottom;
        pop.style.left = left + 'px';
        pop.style.top = top + 'px';
        var pr = pop.getBoundingClientRect();
        if (pr.bottom > global.innerHeight && r.top > pr.height) top = r.top - pr.height;
        if (left + pr.width > global.innerWidth - 8) left = Math.max(8, global.innerWidth - 8 - pr.width);
        pop.style.left = left + 'px';
        pop.style.top = top + 'px';
        pr = pop.getBoundingClientRect();
        var dx = pr.left - left, dy = pr.top - top;
        if (Math.abs(dx) > 1 || Math.abs(dy) > 1) {
            pop.style.left = (left - dx) + 'px';
            pop.style.top = (top - dy) + 'px';
        }
    };

    Combo.prototype.openPop = function (firstChar) {
        if (this.sel.disabled) return;
        if (openInstance && openInstance !== this) openInstance.closePop(false);
        this.cols = columnsFor(this.sel);       /* the caption may follow a Name/Code radio */
        this.readOptions();
        this.search.value = firstChar || '';
        this.applyFilter(this.search.value);
        this.render();
        this.pop.style.display = 'block';
        this.open = true;
        openInstance = this;
        this.position();
        this.input.setAttribute('aria-expanded', 'true');
        this.wrap.classList.add('dtcombo-open');
        var s = this.search;
        s.focus({ preventScroll: true });       /* a focus scroll would fire 'scroll' and close the box */
        try { var n = s.value.length; s.setSelectionRange(n, n); } catch (e) { /* ignore */ }
    };

    Combo.prototype.closePop = function (refocus) {
        if (!this.open) return;
        this.pop.style.display = 'none';
        this.open = false;
        if (openInstance === this) openInstance = null;
        this.input.setAttribute('aria-expanded', 'false');
        this.wrap.classList.remove('dtcombo-open');
        if (refocus && doc.body.contains(this.input)) this.input.focus();
    };

    /* The one place the native select is written. */
    Combo.prototype.choose = function (row) {
        this.closePop(false);
        if (!row || row.disabled) { this.input.focus(); return; }
        var changed = this.sel.value !== row.value || this.sel.selectedIndex !== row.index;
        this.sel.selectedIndex = row.index;
        this.syncFromSelect();
        if (doc.body.contains(this.input)) this.input.focus();
        if (changed) this.fire();
    };

    Combo.prototype.fire = function () {
        var sel = this.sel;
        ['input', 'change'].forEach(function (type) {
            var ev;
            try { ev = new Event(type, { bubbles: true }); }
            catch (e) { ev = doc.createEvent('HTMLEvents'); ev.initEvent(type, true, false); }
            sel.dispatchEvent(ev);
        });
    };

    Combo.prototype.bindEvents = function () {
        var self = this, input = this.input, search = this.search;

        function toggle(e) {
            if (e.button !== 0 || self.sel.disabled) return;
            e.preventDefault();
            if (self.open) { self.closePop(true); return; }
            input.focus({ preventScroll: true });
            self.openPop('');
        }
        input.addEventListener('mousedown', toggle);
        this.caret.addEventListener('mousedown', toggle);

        /* The field: a printable key opens the box with that key typed; F4 / Alt+Down / Space open it. */
        input.addEventListener('keydown', function (e) {
            if (self.sel.disabled || e.ctrlKey || e.metaKey) return;
            if (e.key && e.key.length === 1 && !e.altKey && e.key !== ' ') { e.preventDefault(); self.openPop(e.key); }
            else if (e.key === 'F4' || (e.altKey && e.key === 'ArrowDown') || e.key === ' ') { e.preventDefault(); self.openPop(''); }
        });

        search.addEventListener('input', function () { self.applyFilter(search.value); self.render(); });
        search.addEventListener('keydown', function (e) {
            var k = e.key;
            /* Handled here and stopped here, so a page's Enter-to-Tab or Esc-closes-form handler
               does not act on the same key press. */
            if (k === 'ArrowDown') { e.preventDefault(); e.stopPropagation(); self.move(1); }
            else if (k === 'ArrowUp') { e.preventDefault(); e.stopPropagation(); self.move(-1); }
            else if (k === 'Enter') {
                e.preventDefault(); e.stopPropagation();
                if (self.active >= 0) self.choose(self.filtered[self.active]);
            }
            else if (k === 'Escape') { e.preventDefault(); e.stopPropagation(); self.closePop(true); }
            else if (k === 'Tab') {
                if (self.active >= 0) self.choose(self.filtered[self.active]); else self.closePop(true);
            }
        });

        this.pop.addEventListener('mousedown', function (e) {
            if (e.target === search) return;                       /* let the search box take the caret */
            e.preventDefault();
            var it = e.target.closest ? e.target.closest('.cx-combo-item') : null;
            if (it && !it.classList.contains('is-disabled')) self.choose(self.filtered[parseInt(it.getAttribute('data-i'), 10)]);
        });
        this.pop.addEventListener('mousemove', function (e) {
            var it = e.target.closest ? e.target.closest('.cx-combo-item') : null;
            if (!it) return;
            var i = parseInt(it.getAttribute('data-i'), 10);
            if (i !== self.active) {
                self.active = i;
                var items = self.list.querySelectorAll('.cx-combo-item');
                for (var j = 0; j < items.length; j++) items[j].classList.toggle('is-active', j === i);
            }
        });

        /* Focus leaving the combo altogether (click elsewhere, Shift+Tab out of the search box). */
        this.wrap.addEventListener('focusout', function (e) {
            if (!self.open) return;
            if (e.relatedTarget && self.wrap.contains(e.relatedTarget)) return;
            setTimeout(function () {
                if (self.open && !self.wrap.contains(doc.activeElement)) self.closePop(false);
            }, 0);
        });
    };

    // ------------------------------------------------- staying in step with the screen's code

    Combo.prototype.watch = function () {
        var self = this;
        if (global.MutationObserver) {
            this.mo = new MutationObserver(function (recs) {
                var optionsChanged = false, attrChanged = false;
                for (var i = 0; i < recs.length; i++) {
                    if (recs[i].type === 'childList') optionsChanged = true;
                    else if (recs[i].type === 'attributes') attrChanged = true;
                }
                if (optionsChanged) {
                    self.syncFromSelect();
                    if (self.open) { self.applyFilter(self.search.value); self.render(); }
                }
                if (attrChanged) {
                    self.reflectDisabled();
                    self.syncFromSelect();
                }
            });
            this.mo.observe(this.sel, {
                childList: true, subtree: true,
                attributes: true,
                attributeFilter: ['disabled', 'data-dtcombo', 'data-dtcombo-caption', 'data-columns']
            });
        }
        /* `el.value = x` and `el.selectedIndex = n` fire nothing: hook them on THIS element. */
        try {
            if (!self.sel.__dtcomboHooked) {
                self.sel.__dtcomboHooked = true;
                ['value', 'selectedIndex'].forEach(function (prop) {
                    var d = NATIVE_SELECT_DESC[prop];
                    if (!d || !d.get || !d.set) return;
                    Object.defineProperty(self.sel, prop, {
                        configurable: true,
                        enumerable: false,
                        get: function () { return d.get.call(this); },
                        set: function (v) {
                            d.set.call(this, v);
                            if (self.__syncing) return;
                            self.__syncing = true;
                            try { self.syncFromSelect(); } finally { self.__syncing = false; }
                        }
                    });
                });
            }
        } catch (e) { /* the field then refreshes on the next rebuild or open */ }
    };

    /* jQuery .val() on a select sets option.selected - neither hooked accessor sees it. */
    function hookJQueryVal(jq) {
        if (!jq || !jq.valHooks || jq.__dtcomboValHooked) return false;
        var sh = jq.valHooks.select = jq.valHooks.select || {};
        var orig = sh.set;
        sh.set = function (elem) {
            var out = orig ? orig.apply(this, arguments) : undefined;
            var c = elem && elem.__dtcombo;
            if (c) { try { c.syncFromSelect(); } catch (e) { /* never break .val() */ } }
            return out;
        };
        jq.__dtcomboValHooked = true;
        return true;
    }
    function tryHookJQuery() {
        if (hookJQueryVal(global.jQuery)) return;
        if (doc.readyState === 'loading') {
            doc.addEventListener('DOMContentLoaded', function () { hookJQueryVal(global.jQuery); });
        }
    }

    Combo.prototype.destroy = function () {
        if (this.mo) this.mo.disconnect();
        if (this.pop && this.pop.parentNode) this.pop.parentNode.removeChild(this.pop);
    };

    // ------------------------------------------------- checked (multi-select) mode, 2026-10-02
    /* The desktop's CHECKED UltraCombo (VoucherValidation.cs VouchertypeFill, cmbdoctype): a bool
       "Selected" column at VisiblePosition 0 with a header check box (HeaderCheckBoxVisibility.Always),
       CheckedListSettings.EditorValueSource = CheckedItems, ListSeparator ",", ItemCheckArea = Item
       (a click anywhere on the row toggles it). The field shows the checked texts joined by ",".
       OPT-IN ONLY: <select multiple data-dtcombo="single" data-dtcombo-checked>. A single select never
       reaches this code, and a multiple select without data-dtcombo-checked is still left alone.
       The native <select multiple> stays authoritative: option.selected is what a pick toggles,
       jQuery .val() returns the array, .val([...]) / .val([]) refresh the field, and every toggle
       fires a bubbling input + change (the UltraCombo's ValueChanged per check). */
    function CheckedCombo(sel) { Combo.call(this, sel); }
    CheckedCombo.prototype = Object.create(Combo.prototype);
    CheckedCombo.prototype.constructor = CheckedCombo;
    CheckedCombo.prototype.checkedMode = true;
    CheckedCombo.prototype.multi = function () { return this.cols.length > 1; };

    CheckedCombo.prototype.build = function () {
        Combo.prototype.build.call(this);
        this.wrap.classList.add('pcx-checked');
        this.pop.classList.add('pcx-checked-pop');
        this.search.placeholder = 'Search...';
    };

    CheckedCombo.prototype.checkedTexts = function () {
        var out = [], opts = this.sel.options;
        for (var i = 0; i < opts.length; i++) if (opts[i].selected && (opts[i].value !== '' || opts[i].textContent.trim())) out.push(opts[i].textContent.trim());
        return out;
    };

    /** The checked values, in list order (what jQuery .val() returns, without the empty slot). */
    CheckedCombo.prototype.values = function () {
        var out = [], opts = this.sel.options;
        for (var i = 0; i < opts.length; i++) if (opts[i].selected && opts[i].value !== '') out.push(opts[i].value);
        return out;
    };

    CheckedCombo.prototype.syncFromSelect = function () {
        if (this.__inSync) return;
        this.__inSync = true;
        try {
            this.cols = columnsFor(this.sel);
            this.readOptions();
            this.input.value = this.checkedTexts().join(',');
            this.input.title = this.input.value;
        } finally { this.__inSync = false; }
    };

    CheckedCombo.prototype.applyFilter = function (q) {
        Combo.prototype.applyFilter.call(this, q);
        /* the "nothing selected" slot has nothing to check */
        this.filtered = this.filtered.filter(function (r) { return !((r.value === '0' || r.value === '') && !String(r.text).trim()); });
        if (this.active >= this.filtered.length) this.active = this.filtered.length ? 0 : -1;
        if (this.active < 0 && this.filtered.length) this.active = 0;
    };

    CheckedCombo.prototype.render = function () {
        var self = this, rows = this.filtered, cols = this.cols;
        this.pop.classList.toggle('is-cols', this.multi());
        if (!rows.length) { this.list.innerHTML = '<div class="cx-combo-empty">No match</div>'; return; }
        var all = rows.every(function (r) { return r.disabled || r.opt.selected; });
        var some = rows.some(function (r) { return r.opt.selected; });
        var keep = this.list.scrollTop;
        this.list.innerHTML = '<table class="cx-combo-table pcx-check-table"><thead><tr>'
            + '<th class="pcx-check"><input type="checkbox" class="pcx-check-all" tabindex="-1"' + (all ? ' checked' : '') + ' title="Check / uncheck all"></th>'
            + cols.map(function (col) { return '<th>' + esc(col.caption || '') + '</th>'; }).join('') + '</tr></thead><tbody>'
            + rows.map(function (r, i) {
                return '<tr class="cx-combo-item' + (i === self.active ? ' is-active' : '') + (r.disabled ? ' is-disabled' : '')
                     + (r.opt.selected ? ' is-checked' : '') + '" data-i="' + i + '">'
                     + '<td class="pcx-check"><input type="checkbox" tabindex="-1"' + (r.opt.selected ? ' checked' : '') + (r.disabled ? ' disabled' : '') + '></td>'
                     + cols.map(function (col, j) { return self.cellHtml(col, r.cells[j]); }).join('') + '</tr>';
            }).join('')
            + '</tbody></table>';
        var hc = this.list.querySelector('.pcx-check-all');
        if (hc) hc.indeterminate = some && !all;
        this.list.scrollTop = keep;
    };

    /** ItemCheckArea = Item: a click anywhere on the row toggles it; the box stays open. */
    CheckedCombo.prototype.toggle = function (row) {
        if (!row || row.disabled) return;
        row.opt.selected = !row.opt.selected;
        this.syncFromSelect();
        this.render();
        this.fire();
    };

    /** The header check box: checks every row the search shows, or unchecks them when all are checked. */
    CheckedCombo.prototype.toggleAll = function () {
        var rows = this.filtered.filter(function (r) { return !r.disabled; });
        if (!rows.length) return;
        var to = !rows.every(function (r) { return r.opt.selected; });
        rows.forEach(function (r) { r.opt.selected = to; });
        this.syncFromSelect();
        this.render();
        this.fire();
    };

    /** Programmatic: setValues(['5','7']) / setValues([]) - checks exactly those values, fires change once. */
    CheckedCombo.prototype.setValues = function (vals, silent) {
        var want = {};
        (vals || []).forEach(function (v) { want[String(v)] = true; });
        var opts = this.sel.options, changed = false;
        for (var i = 0; i < opts.length; i++) {
            var on = !!want[opts[i].value];
            if (opts[i].selected !== on) { opts[i].selected = on; changed = true; }
        }
        this.syncFromSelect();
        if (this.open) this.render();
        if (changed && !silent) this.fire();
    };

    /* Enter / a click toggle without closing (a choose() would close the box), Tab and Esc close. */
    CheckedCombo.prototype.choose = function (row) { this.toggle(row); };

    CheckedCombo.prototype.bindEvents = function () {
        var self = this, input = this.input, search = this.search;

        function toggleOpen(e) {
            if (e.button !== 0 || self.sel.disabled) return;
            e.preventDefault();
            if (self.open) { self.closePop(true); return; }
            input.focus({ preventScroll: true });
            self.openPop('');
        }
        input.addEventListener('mousedown', toggleOpen);
        this.caret.addEventListener('mousedown', toggleOpen);

        input.addEventListener('keydown', function (e) {
            if (self.sel.disabled || e.ctrlKey || e.metaKey) return;
            if (e.key && e.key.length === 1 && !e.altKey && e.key !== ' ') { e.preventDefault(); self.openPop(e.key); }
            else if (e.key === 'F4' || (e.altKey && e.key === 'ArrowDown') || e.key === ' ') { e.preventDefault(); self.openPop(''); }
            else if (e.key === 'Delete') { e.preventDefault(); self.setValues([]); }
        });

        search.addEventListener('input', function () { self.applyFilter(search.value); self.list.scrollTop = 0; self.render(); });
        search.addEventListener('keydown', function (e) {
            var k = e.key;
            if (k === 'ArrowDown') { e.preventDefault(); e.stopPropagation(); self.move(1); }
            else if (k === 'ArrowUp') { e.preventDefault(); e.stopPropagation(); self.move(-1); }
            else if (k === 'Enter') {
                e.preventDefault(); e.stopPropagation();
                if (self.active >= 0) self.toggle(self.filtered[self.active]);
            }
            else if (k === 'Escape') { e.preventDefault(); e.stopPropagation(); self.closePop(true); }
            else if (k === 'Tab') { self.closePop(false); }
        });

        this.pop.addEventListener('mousedown', function (e) {
            if (e.target === search) return;
            e.preventDefault();
            if (e.target.closest && e.target.closest('.pcx-check-all, thead')) { self.toggleAll(); return; }
            var it = e.target.closest ? e.target.closest('.cx-combo-item') : null;
            if (it && !it.classList.contains('is-disabled')) {
                self.active = parseInt(it.getAttribute('data-i'), 10);
                self.toggle(self.filtered[self.active]);
            }
        });
        /* the check boxes are drawn, not used: their own click must not flip them a second time */
        this.pop.addEventListener('click', function (e) {
            if (e.target && e.target.type === 'checkbox') e.preventDefault();
        });
        this.pop.addEventListener('mousemove', function (e) {
            var it = e.target.closest ? e.target.closest('.cx-combo-item') : null;
            if (!it) return;
            var i = parseInt(it.getAttribute('data-i'), 10);
            if (i !== self.active) {
                self.active = i;
                var items = self.list.querySelectorAll('.cx-combo-item');
                for (var j = 0; j < items.length; j++) items[j].classList.toggle('is-active', j === i);
            }
        });

        this.wrap.addEventListener('focusout', function (e) {
            if (!self.open) return;
            if (e.relatedTarget && self.wrap.contains(e.relatedTarget)) return;
            setTimeout(function () {
                if (self.open && !self.wrap.contains(doc.activeElement)) self.closePop(false);
            }, 0);
        });
    };

    // ------------------------------------------------------------------ public API

    var CLAIMED = [
        'select[data-dtcombo]',
        '.dtcombo',
        '.so-select2', '.select2', '.select2-2col', '.select2-4col',
        '.searchable', '.search-select',
        'select.win-combo', 'select.win-input', 'select.form-select', 'select.form-control',
        'select.ctl'
    ].join(',');

    function enhance(sel) {
        if (!sel || sel.tagName !== 'SELECT') return null;
        if (sel.__dtcombo) return sel.__dtcombo;
        if (CHECKED_ONLY && !(sel.multiple && sel.hasAttribute('data-dtcombo-checked'))) return null;
        if (sel.multiple || sel.size > 1) {
            /* 2026-10-02: the checked mode is opt-in (data-dtcombo-checked); every other multiple /
               list-box select is left alone, exactly as before. */
            if (!sel.multiple || !sel.hasAttribute('data-dtcombo-checked') || sel.hasAttribute('data-dtcombo-skip')) return null;
            var cc = new CheckedCombo(sel);
            sel.__dtcombo = cc;
            INSTANCES.push(cc);
            return cc;
        }
        if (sel.hasAttribute('data-dtcombo-skip')) return null;
        if (sel.hasAttribute('data-select2-id')) return null;
        if (sel.previousElementSibling && sel.previousElementSibling.classList
            && sel.previousElementSibling.classList.contains('select2-container')) return null;
        var c = new Combo(sel);
        sel.__dtcombo = c;
        INSTANCES.push(c);
        return c;
    }

    function init(selector) {
        var list = doc.querySelectorAll(selector || (CHECKED_ONLY ? CHECKED_SELECTOR : CLAIMED));
        var n = 0;
        for (var i = 0; i < list.length; i++) if (enhance(list[i])) n++;
        return n;
    }

    /* Store behaviour: a click outside, a scroll outside the popup or a resize closes it. */
    doc.addEventListener('mousedown', function (e) {
        if (!openInstance) return;
        if (openInstance.wrap.contains(e.target)) return;
        openInstance.closePop(false);
    }, true);
    doc.addEventListener('scroll', function (e) {
        /* 2026-10-03: ticking a row in checked mode toggles option.selected on the hidden native <select multiple>,
           which makes the browser scroll that select - that scroll must not close the popup. */
        var t = e.target;
        if (openInstance && t && (t === openInstance.sel || (t.classList && t.classList.contains('dtcombo-native')))) return;
        /* 2026-10-04: a text box scrolls its own text when it loses focus with a value wider than itself (a long
           combo caption, e.g. "GANJI TO HODI ROOM KATIE"): clicking the next combo blurred the previous field, its
           text scrolled back, and that scroll closed the popup that had just opened. Field-internal scrolls are ignored. */
        if (t && (t.tagName === 'INPUT' || t.tagName === 'TEXTAREA')) return;
        if (openInstance && !openInstance.pop.contains(t)) openInstance.closePop(false);
    }, true);
    global.addEventListener('resize', function () { if (openInstance) openInstance.closePop(false); });

    tryHookJQuery();

    var API = {
        init: init,
        enhance: enhance,
        claimed: CLAIMED,
        families: FAMILIES,
        instances: INSTANCES,
        design: 'store',
        define: function (name, cols) { FAMILIES[name] = cols; },
        refresh: function () { return init(); }
    };
    global.DesktopCheckedCombo = API;
    if (!CHECKED_ONLY) global.DesktopCombo = API;   /* checked-only: keep the page's own DesktopCombo */

    /* Rows built after load ("+ Add Row", re-rendered grids) are enhanced too, one pass per batch. */
    var rescanQueued = false;
    function queueRescan() {
        if (rescanQueued) return;
        rescanQueued = true;
        setTimeout(function () { rescanQueued = false; init(); }, 16);
    }
    function watchDocument() {
        if (!global.MutationObserver) return;
        new MutationObserver(function (recs) {
            for (var i = 0; i < recs.length; i++) {
                var added = recs[i].addedNodes;
                for (var j = 0; j < added.length; j++) {
                    var n = added[j];
                    if (n.nodeType !== 1) continue;
                    if (n.classList && (n.classList.contains('dtcombo-pop') || n.classList.contains('dtcombo-wrap'))) continue;
                    if (n.tagName === 'SELECT' || (n.querySelector && n.querySelector('select'))) { queueRescan(); return; }
                }
            }
        }).observe(doc.body, { childList: true, subtree: true });
    }

    function boot() { init(); watchDocument(); }
    if (doc.readyState === 'loading') doc.addEventListener('DOMContentLoaded', boot);
    else boot();
}(window));
