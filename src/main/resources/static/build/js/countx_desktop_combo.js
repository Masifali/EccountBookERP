/* ============================================================================================
 * Desktop-style multi-column dropdowns - every Sale, Purchase and CMAGT screen, one component.
 *
 * NO jQuery. NO select2. NO other dependency.
 *
 * The desktop's combos are Infragistics UltraCombo / Janus GridEX controls: a GRID in the drop
 * area with a header band, several columns, boolean columns drawn as checkboxes, and type-ahead
 * filtering (AutoCompleteMode). A plain <select> shows one column and no header, which is why the
 * web screens did not look like the desktop.
 *
 * Before this file there were TWO multi-column implementations - one in countx_sale_order_full.js,
 * another in countx_purchase_order_full.js - that disagreed about markup, CSS and column sets, and
 * both needed jQuery + select2. The other 32 Sale/Purchase/CMAGT screens load neither, so their
 * 150-odd dropdowns were plain native selects. Depending on select2 would have meant injecting
 * jQuery into every one of those screens; this renderer avoids that entirely.
 *
 * --------------------------------------------------------------------------------------------
 * THE NATIVE <select> STAYS THE SOURCE OF TRUTH
 * --------------------------------------------------------------------------------------------
 * The screens' own code reads and writes these controls in every way imaginable:
 *
 *     el.value              el.value = '7'         el.selectedOptions[0].textContent
 *     el.selectedIndex      el.options             el.innerHTML = '<option>...'
 *     $(el).val(...)        $(el).trigger('change')
 *
 * so the <select> is kept in the DOM and kept authoritative. This component only *hides* it and
 * draws a grid beside it. Every pick writes back to the select and fires a bubbling `change`
 * (and `input`), so existing onchange="..." handlers and listeners keep working untouched.
 *
 * Three kinds of external change are picked up automatically:
 *   - options rebuilt  -> MutationObserver on childList
 *   - value assigned   -> a per-element accessor hook over `value` / `selectedIndex`
 *   - disabled toggled -> MutationObserver on attributes
 *
 * If this script fails to load, the native <select> is still there and still works. The
 * enhancement is additive.
 *
 * --------------------------------------------------------------------------------------------
 * COLUMN SETS ARE PER DESKTOP FORM, NOT GLOBAL
 * --------------------------------------------------------------------------------------------
 * The same-looking combo does NOT have the same columns on every form:
 *
 *   Purchase Order (PurchsaeOrder.cs)            Sale Order (SaleOrder.cs)
 *   dtSupplier, cols 2 and 4 hidden (:1031)      dtsuppcus, cols 3 and 4 hidden
 *     -> Name | PartyCode | CityName | MobileNo    -> Name | PartyCode | CityName
 *   dtUOM, col 2 hidden (:1396, :1398)           dtUom, all four visible
 *     -> UOM | BaseRateUom                         -> UOM | Equivalent | QtyEquivalent | BaseRateUom
 *
 * Forcing one column set across both would be the same mistake as forcing two different desktop
 * forms through one dropdown endpoint. Each family below names the form it came from.
 *
 * --------------------------------------------------------------------------------------------
 * USAGE
 * --------------------------------------------------------------------------------------------
 *   <select id="cmbSupplier" class="dtcombo"
 *           data-dtcombo="party4" data-dtcombo-caption="Supplier Name">
 *     <option value="12" data-code="SUP-1" data-city="LAHORE" data-mobile="0300…">ACME</option>
 *
 * The first column is the option's own text; later columns read the data- attributes named in the
 * family. A missing attribute renders an EMPTY cell - never a placeholder, so a missing column
 * stays visible as missing.
 *
 * data-dtcombo-caption overrides the first column's caption and is re-read on every open, so a
 * caption that follows a Name/Code radio only has to update the attribute.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var doc = global.document;
    var INSTANCES = [];
    var openInstance = null;
    var seq = 0;

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

    /* type: 'text' (default) | 'num' (right-aligned) | 'check' (boolean -> read-only tick) */
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

        /* One captioned column - the desktop still draws a header band over these. */
        single: [ { caption: '', flex: 1 } ]
    };

    function columnsFor(sel) {
        var fam = FAMILIES[sel.getAttribute('data-dtcombo')] || FAMILIES.single;
        var caption = sel.getAttribute('data-dtcombo-caption');
        if (!caption) return fam;
        var copy = fam.slice();
        copy[0] = { caption: caption, flex: fam[0].flex, key: fam[0].key, type: fam[0].type };
        return copy;
    }

    // ------------------------------------------------------------------ one control

    function Combo(sel) {
        this.sel = sel;
        this.id = 'dtc' + (++seq);
        this.cols = columnsFor(sel);
        this.rows = [];          /* [{opt, cells[], text, value, disabled}] */
        this.filtered = [];
        this.active = -1;
        this.open = false;
        this.build();
        this.syncFromSelect();
        this.watch();
    }

    Combo.prototype.build = function () {
        var sel = this.sel;

        /* Inspect native select's positioning, sizing and inline styles BEFORE modifying sel */
        var attrStyle = sel.getAttribute('style') || '';
        var inlineLeft = sel.style.left || (attrStyle.match(/left\s*:\s*([^;]+)/i) || [])[1];
        var inlineTop = sel.style.top || (attrStyle.match(/top\s*:\s*([^;]+)/i) || [])[1];
        var inlineRight = sel.style.right || (attrStyle.match(/right\s*:\s*([^;]+)/i) || [])[1];
        var inlineBottom = sel.style.bottom || (attrStyle.match(/bottom\s*:\s*([^;]+)/i) || [])[1];
        var inlineWidth = sel.style.width || (attrStyle.match(/width\s*:\s*([^;]+)/i) || [])[1];
        var inlineHeight = sel.style.height || (attrStyle.match(/height\s*:\s*([^;]+)/i) || [])[1];
        var inlinePos = sel.style.position || (attrStyle.match(/position\s*:\s*([^;]+)/i) || [])[1];

        var cs = global.getComputedStyle ? global.getComputedStyle(sel) : null;
        var isAbsolute = (inlinePos === 'absolute') || (cs && cs.position === 'absolute') || sel.classList.contains('ctl') || (!!inlineLeft && !!inlineTop);

        /* Hide native select */
        sel.classList.add('dtcombo-native');
        sel.setAttribute('aria-hidden', 'true');
        sel.setAttribute('tabindex', '-1');

        /* Build wrapper element */
        var wrap = doc.createElement('div');
        wrap.className = ('dtcombo-wrap ' + sel.className.replace('dtcombo-native', '')).trim();

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

        var input = doc.createElement('input');
        input.type = 'text';
        input.className = 'dtcombo-input ' + (sel.getAttribute('data-dtcombo-class') || '');
        input.setAttribute('autocomplete', 'off');
        input.setAttribute('role', 'combobox');
        input.setAttribute('aria-expanded', 'false');
        /* The desktop combos are type-ahead (AutoCompleteMode), so this is a real text box. */
        input.placeholder = sel.getAttribute('data-dtcombo-placeholder') || '';

        var caret = doc.createElement('span');
        caret.className = 'dtcombo-caret';
        caret.innerHTML = '&#9662;';

        wrap.appendChild(input);
        wrap.appendChild(caret);
        sel.parentNode.insertBefore(wrap, sel);
        wrap.appendChild(sel);

        this.wrap = wrap;
        this.input = input;
        this.caret = caret;

        /* The popup lives on <body> so it is never clipped by a panel's overflow, and is
           positioned in viewport coordinates on each open. */
        var pop = doc.createElement('div');
        pop.className = 'dtcombo-pop';
        pop.style.display = 'none';
        pop.setAttribute('role', 'listbox');
        doc.body.appendChild(pop);
        this.pop = pop;

        this.bindEvents();
        this.reflectDisabled();
    };

    /* Reads the <option> list into rows. Called on build and whenever the options change. */
    Combo.prototype.readOptions = function () {
        var rows = [];
        var opts = this.sel.options;
        for (var i = 0; i < opts.length; i++) {
            var o = opts[i];
            var cells = [];
            for (var c = 0; c < this.cols.length; c++) {
                var col = this.cols[c];
                if (c === 0) { cells.push(o.textContent); continue; }
                var v = col.key ? o.getAttribute('data-' + col.key) : null;
                cells.push(v === null || v === undefined ? '' : v);   /* absent -> empty cell */
            }
            rows.push({
                opt: o, index: i, value: o.value, text: o.textContent,
                cells: cells, disabled: o.disabled,
                /* hidden options are honoured - the GRN screen hides UOM rows that do not
                   belong to the chosen item rather than removing them */
                hidden: o.hidden || o.style.display === 'none'
            });
        }
        this.rows = rows;
    };

    /* Captured ONCE, by name, from the real HTMLSelectElement prototype - before any instance
       can patch an element. See the accessor hook below for why this matters. */
    var NATIVE_SELECT_DESC = (function () {
        var out = {};
        try {
            var proto = global.HTMLSelectElement && global.HTMLSelectElement.prototype;
            if (proto) {
                out.value = Object.getOwnPropertyDescriptor(proto, 'value');
                out.selectedIndex = Object.getOwnPropertyDescriptor(proto, 'selectedIndex');
            }
        } catch (e) { /* leave empty; the hook then simply does not install */ }
        return out;
    })();

    Combo.prototype.syncFromSelect = function () {
        if (this.__inSync) return;          /* never re-enter, whatever called us */
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
    };

    // ------------------------------------------------------------------ rendering

    Combo.prototype.headerHtml = function () {
        var h = '<div class="dtcombo-header">';
        for (var i = 0; i < this.cols.length; i++) {
            var c = this.cols[i];
            h += '<span class="dtcombo-cell' + this.cellClass(c) + '" style="flex:' + c.flex + ';">'
               + esc(c.caption) + '</span>';
        }
        return h + '</div>';
    };

    Combo.prototype.cellClass = function (c) {
        return c.type === 'num' ? ' dtcombo-num' : c.type === 'check' ? ' dtcombo-check' : '';
    };

    Combo.prototype.rowHtml = function (row, idx) {
        var h = '<div class="dtcombo-row' + (row.disabled ? ' dtcombo-row-disabled' : '')
              + (idx === this.active ? ' dtcombo-active' : '')
              + '" data-i="' + row.index + '" role="option">';
        for (var i = 0; i < this.cols.length; i++) {
            var c = this.cols[i];
            var body;
            if (c.type === 'check') {
                /* The desktop draws the boolean column as a read-only tick, not a toggle. */
                body = '<input type="checkbox" disabled' + (truthy(row.cells[i]) ? ' checked' : '') + '>';
            } else {
                body = esc(row.cells[i]);
            }
            h += '<span class="dtcombo-cell' + this.cellClass(c) + '" style="flex:' + c.flex + ';">'
               + body + '</span>';
        }
        return h + '</div>';
    };

    /* Type-ahead across EVERY visible column, which is what the desktop's AutoSuggestFilterMode
       does - typing a party code matches the PartyCode column, not just the name. */
    Combo.prototype.applyFilter = function (q) {
        var needle = String(q || '').trim().toLowerCase();
        var out = [];
        for (var i = 0; i < this.rows.length; i++) {
            var r = this.rows[i];
            if (r.hidden) continue;
            if (!needle) { out.push(r); continue; }
            var hit = false;
            for (var c = 0; c < r.cells.length; c++) {
                if (this.cols[c] && this.cols[c].type === 'check') continue;
                if (String(r.cells[c]).toLowerCase().indexOf(needle) >= 0) { hit = true; break; }
            }
            if (hit) out.push(r);
        }
        this.filtered = out;
    };

    Combo.prototype.render = function () {
        var h = this.headerHtml() + '<div class="dtcombo-body">';
        if (!this.filtered.length) {
            h += '<div class="dtcombo-empty">No matching rows.</div>';
        } else {
            for (var i = 0; i < this.filtered.length; i++) h += this.rowHtml(this.filtered[i], i);
        }
        this.pop.innerHTML = h + '</div>';

        var self = this;
        var body = this.pop.querySelector('.dtcombo-body');
        body.addEventListener('mousedown', function (e) {
            /* mousedown, not click: the input's blur would close the popup first. */
            var row = e.target.closest ? e.target.closest('.dtcombo-row') : null;
            if (!row || row.classList.contains('dtcombo-row-disabled')) return;
            e.preventDefault();
            self.choose(parseInt(row.getAttribute('data-i'), 10));
        });
        body.addEventListener('mousemove', function (e) {
            var row = e.target.closest ? e.target.closest('.dtcombo-row') : null;
            if (!row) return;
            var i = self.filtered.findIndex(function (r) {
                return r.index === parseInt(row.getAttribute('data-i'), 10);
            });
            if (i >= 0 && i !== self.active) { self.active = i; self.paintActive(); }
        });
    };

    Combo.prototype.paintActive = function () {
        var rows = this.pop.querySelectorAll('.dtcombo-row');
        for (var i = 0; i < rows.length; i++) rows[i].classList.toggle('dtcombo-active', i === this.active);
        var el = rows[this.active];
        if (el && el.scrollIntoView) el.scrollIntoView({ block: 'nearest' });
    };

    Combo.prototype.position = function () {
        var r = this.wrap.getBoundingClientRect();
        var pop = this.pop;

        /* The desktop's drop grid is wider than its field - it has to be, to fit four or five
           columns without clipping. Width is derived from the column weights (roughly 46px per
           flex unit), floored at the field's own width and capped to the viewport, so a 5-column
           item grid gets room while a single-column combo stays field-width. */
        var weight = 0;
        for (var i = 0; i < this.cols.length; i++) weight += (this.cols[i].flex || 1);
        var want = Math.max(r.width, this.cols.length > 1 ? weight * 46 : 0, 200);
        pop.style.minWidth = Math.min(want, global.innerWidth - 16) + 'px';
        pop.style.maxWidth = (global.innerWidth - 16) + 'px';

        /* Keep it on screen when the field sits near the right edge. */
        var left = Math.round(r.left);
        var w = pop.offsetWidth || want;
        if (left + w > global.innerWidth - 8) left = Math.max(8, global.innerWidth - 8 - w);
        pop.style.left = left + 'px';
        /* Flip above when there is not enough room below, as a native dropdown does. */
        var below = global.innerHeight - r.bottom;
        var h = pop.offsetHeight || 260;
        if (below < h && r.top > below) {
            pop.style.top = Math.max(0, Math.round(r.top - h)) + 'px';
        } else {
            pop.style.top = Math.round(r.bottom) + 'px';
        }
    };

    // ------------------------------------------------------------------ open / close / choose

    Combo.prototype.openPop = function (selectAll) {
        if (this.sel.disabled || this.open) return;
        if (openInstance && openInstance !== this) openInstance.closePop();
        this.cols = columnsFor(this.sel);       /* caption may follow a Name/Code radio */
        this.readOptions();
        this.applyFilter('');
        var cur = this.sel.selectedIndex;
        this.active = this.filtered.findIndex(function (r) { return r.index === cur; });
        this.render();
        this.pop.style.display = 'block';
        this.position();
        this.paintActive();
        this.open = true;
        openInstance = this;
        this.input.setAttribute('aria-expanded', 'true');
        this.wrap.classList.add('dtcombo-open');
        if (selectAll) this.input.select();
    };

    Combo.prototype.closePop = function (restoreText) {
        if (!this.open) return;
        this.pop.style.display = 'none';
        this.open = false;
        if (openInstance === this) openInstance = null;
        this.input.setAttribute('aria-expanded', 'false');
        this.wrap.classList.remove('dtcombo-open');
        if (restoreText !== false) this.syncFromSelect();   /* discard a half-typed filter */
    };

    /* The one place the native select is written. Everything else reads it. */
    Combo.prototype.choose = function (optIndex) {
        var o = this.sel.options[optIndex];
        if (!o || o.disabled) return;
        var changed = this.sel.selectedIndex !== optIndex;
        this.sel.selectedIndex = optIndex;
        this.syncFromSelect();
        this.closePop(false);
        this.input.focus();
        if (changed) this.fire();
    };

    /* Bubbling native events, so inline onchange="..." attributes and addEventListener both run.
       jQuery's .on('change') is bound as a native listener, so screens that use jQuery hear this
       too - no jQuery dependency is needed to notify them. */
    Combo.prototype.fire = function () {
        var sel = this.sel;
        ['input', 'change'].forEach(function (type) {
            var ev;
            try {
                ev = new Event(type, { bubbles: true });
            } catch (e) {                       /* older engines */
                ev = doc.createEvent('HTMLEvents');
                ev.initEvent(type, true, false);
            }
            sel.dispatchEvent(ev);
        });

        /* No extra jQuery trigger here, deliberately. Screens bind their handlers with a jQuery
           NAMESPACE - $('#cmbSupplier').off('change.sync').on('change.sync', …) - and jQuery
           listens for the base type natively, so the dispatch above already runs them. Adding
           jQuery(sel).trigger('change') on top fired every such handler TWICE, which for the
           supplier combo meant running onSupplierSelectedEventChain twice per pick.
           Verified in a browser: native dispatch alone reaches both the addEventListener and the
           .on('change.sync') handler, exactly once each. */
    };

    Combo.prototype.bindEvents = function () {
        var self = this;
        var input = this.input;

        this.caret.addEventListener('mousedown', function (e) {
            e.preventDefault();
            if (self.open) self.closePop(); else { input.focus(); self.openPop(true); }
        });

        input.addEventListener('mousedown', function () {
            if (!self.open) setTimeout(function () { self.openPop(true); }, 0);
        });

        input.addEventListener('input', function () {
            if (!self.open) self.openPop(false);
            self.applyFilter(input.value);
            self.active = self.filtered.length ? 0 : -1;
            self.render();
            self.position();
            self.paintActive();
        });

        input.addEventListener('keydown', function (e) {
            var k = e.key;
            if (k === 'ArrowDown' || k === 'ArrowUp') {
                e.preventDefault();
                if (!self.open) { self.openPop(false); return; }
                var n = self.filtered.length;
                if (!n) return;
                self.active = k === 'ArrowDown'
                    ? (self.active + 1 >= n ? n - 1 : self.active + 1)
                    : (self.active - 1 < 0 ? 0 : self.active - 1);
                self.paintActive();
            } else if (k === 'Enter') {
                if (!self.open) return;
                /* While the grid is open Enter belongs to the grid. preventDefault stops an
                   implicit form submit; stopPropagation stops a page-level key handler acting on
                   the same press and undoing the pick. */
                e.preventDefault();
                e.stopPropagation();
                var pickIdx = self.active;
                /* Typed a filter and never moved? Commit the single obvious match. */
                if (pickIdx < 0 && self.filtered.length === 1) pickIdx = 0;
                if (pickIdx < 0 && self.filtered.length) pickIdx = 0;
                if (pickIdx >= 0 && self.filtered[pickIdx]) {
                    self.choose(self.filtered[pickIdx].index);
                }
            } else if (k === 'Escape') {
                if (self.open) { e.preventDefault(); self.closePop(); }
            } else if (k === 'Tab') {
                self.closePop();                /* Tab commits nothing, as on the desktop */
            }
        });

        input.addEventListener('blur', function () {
            /* Delayed so a mousedown on a row is processed first. */
            setTimeout(function () {
                if (self.open && !self.pop.contains(doc.activeElement)) self.closePop();
            }, 120);
        });
    };

    // ------------------------------------------------- staying in step with the screen's code

    /* Screens rebuild their option lists constantly (bindCombo, bindItemUom, loadSuppliers…),
       assign .value directly, and toggle .disabled. None of that fires an event, so each is
       observed rather than hoped for. */
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
                    if (self.open) { self.applyFilter(self.input.value); self.render(); self.paintActive(); }
                }
                if (attrChanged) {
                    self.reflectDisabled();
                    self.syncFromSelect();
                }
            });
            this.mo.observe(this.sel, {
                childList: true, subtree: true,
                attributes: true,
                attributeFilter: ['disabled', 'data-dtcombo', 'data-dtcombo-caption']
            });
        }

        /* `el.value = x` and `el.selectedIndex = n` are silent. Hook them on THIS element only,
           delegating to the prototype so the select still behaves exactly like a select - the
           hook just refreshes the display afterwards. Per-element, so nothing global is patched. */
        try {
            /* The descriptors come from NATIVE_SELECT_DESC, captured once at module load from
               HTMLSelectElement.prototype. They used to be read here with
               Object.getOwnPropertyDescriptor(Object.getPrototypeOf(this.sel), prop), which is
               fragile: if anything up that element's prototype chain already carried a patched
               accessor, `d.get` WAS this very getter and reading .value recursed until the stack
               blew - "Maximum call stack size exceeded" pointing at this line. Reading the native
               descriptor once, from the prototype by name, cannot capture a patched accessor.

               The element is also marked so a second enhance() on the same select cannot install
               a second layer, and the setter carries a re-entrancy flag so syncFromSelect can
               never drive the setter back into itself. */
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
        } catch (e) {
            /* If the engine will not allow it the control still works; the display then refreshes
               on the next option rebuild or open rather than instantly. */
        }
    };

    /* ------------------------------------------------------------------------------------------
     * jQuery .val() is the one path the per-element accessor hook above CANNOT see.
     *
     * For a <select>, jQuery does not assign elem.value or elem.selectedIndex at all - its
     * valHooks.select.set walks the options and sets option.selected = true on the match. Neither
     * hooked accessor fires, and neither does any event, so the native select ends up correct
     * while the visible input still shows the previous caption.
     *
     * Measured, jQuery 1.9.1 + Chromium:
     *     el.value = '11'            -> display follows          (accessor hook)
     *     el.selectedIndex = 1       -> display follows          (accessor hook)
     *     jQuery(el).val('12')       -> native 12, display STALE (this bug)
     *
     * That is the path these screens use to load a saved document, so opening an existing record
     * showed "-- Select --" over a combo that was in fact set. Wrapping the hook once - delegating
     * to the original and then refreshing whatever instance owns the element - fixes every screen
     * without touching a single call site, and leaves jQuery's own return value untouched.
     *
     * Also covers `.val(x).trigger('change.select2')`, which a few pages still carry: a namespaced
     * trigger fires only handlers in that namespace, so listening for 'change' would not help.
     * ---------------------------------------------------------------------------------------- */
    function hookJQueryVal(jq) {
        if (!jq || !jq.valHooks || jq.__dtcomboValHooked) return false;
        var sh = jq.valHooks.select = jq.valHooks.select || {};
        var orig = sh.set;
        sh.set = function (elem, value) {
            var out = orig ? orig.apply(this, arguments) : undefined;
            var c = elem && elem.__dtcombo;
            if (c) { try { c.syncFromSelect(); } catch (e) { /* never break .val() */ } }
            return out;
        };
        jq.__dtcomboValHooked = true;
        return true;
    }

    /* jQuery may load after this file, so try now and once more when the DOM is ready. */
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

    // ------------------------------------------------------------------ public API

    /* Every dropdown marker used anywhere in Sale, Purchase and CMAGT. One implementation now
       serves all of them, so a combo on Purchase Order looks like the matching one on Sale Order.
       `.dtcombo` is the explicit opt-in for new markup. */
    var CLAIMED = [
        /* The attribute itself claims the element. Without this line a select is enhanced only if
           it also happens to carry one of the class names below - and cmagt/purchase_order_cmagt.html
           uses its own w### width classes, so all 21 of its data-dtcombo attributes were inert.
           Keying on the attribute makes the tag authoritative and cannot over-claim, since only a
           tagged select matches it. */
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
        if (sel.multiple || sel.size > 1) return null;      /* not a drop-down */
        if (sel.hasAttribute('data-dtcombo-skip')) return null;
        /* A screen that still initialises select2 on this element keeps it - double-wrapping
           would leave two controls over one select. Sale Order and Purchase Order have had their
           own initialisers turned into no-ops so their combos come here instead, but any screen
           that has not been converted degrades to its previous behaviour rather than breaking. */
        if (sel.hasAttribute('data-select2-id')) return null;
        if (sel.previousElementSibling
            && sel.previousElementSibling.classList
            && sel.previousElementSibling.classList.contains('select2-container')) return null;
        var c = new Combo(sel);
        sel.__dtcombo = c;
        INSTANCES.push(c);
        return c;
    }

    function init(selector) {
        var list = doc.querySelectorAll(selector || CLAIMED);
        var n = 0;
        for (var i = 0; i < list.length; i++) if (enhance(list[i])) n++;
        return n;
    }

    /* An open grid must follow its field when the page scrolls, and close on a real outside
       click. Registered once, not per control. */
    doc.addEventListener('mousedown', function (e) {
        if (!openInstance) return;
        if (openInstance.wrap.contains(e.target) || openInstance.pop.contains(e.target)) return;
        openInstance.closePop();
    }, true);
    global.addEventListener('scroll', function () { if (openInstance) openInstance.position(); }, true);
    global.addEventListener('resize', function () { if (openInstance) openInstance.position(); });

        tryHookJQuery();

global.DesktopCombo = {
        init: init,
        enhance: enhance,
        claimed: CLAIMED,
        families: FAMILIES,
        instances: INSTANCES,
        /* Lets a screen declare a form-specific column set without editing this file. */
        define: function (name, cols) { FAMILIES[name] = cols; },
        /* For screens that inject markup after load. */
        refresh: function () { return init(); }
    };

    /* --------------------------------------------------------------------------------------
       Grid rows are built AFTER load - "+ Add Row" on the Account Credit, Supplier Expense,
       Empty Bags and Payment Detail tabs each inject a <tr> containing their own <select>. A
       one-off scan at DOMContentLoaded misses every one of them, which is why an Account Title
       cell stayed a plain native select while the header combos were grids.

       So the document is watched and anything inserted later is enhanced too. Batched on a
       microtask-ish timer so adding twenty rows costs one pass, not twenty.
       -------------------------------------------------------------------------------------- */
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
                    /* The popup and wrapper this component creates must not retrigger a scan. */
                    if (n.classList && (n.classList.contains('dtcombo-pop')
                                     || n.classList.contains('dtcombo-wrap'))) continue;
                    if (n.tagName === 'SELECT' || (n.querySelector && n.querySelector('select'))) {
                        queueRescan();
                        return;
                    }
                }
            }
        }).observe(doc.body, { childList: true, subtree: true });
    }

    function boot() { init(); watchDocument(); }

    if (doc.readyState === 'loading') {
        doc.addEventListener('DOMContentLoaded', boot);
    } else {
        boot();
    }
}(window));
