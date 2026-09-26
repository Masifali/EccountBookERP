/* ============================================================================================
 * 1002 Purchsae Order (Packing Material) - screen 498, module 54, DocumentTypeId 700.
 * Desktop: Architecture.WinApp.PackingMaterial_Store.PurchsaeOrderPmNew (6,854 lines).
 *
 * Handler names follow the desktop's so each can be checked against its C# line:
 *   InitializeComponentMethod :473   init / loadLookups
 *   CalculateWeight :2622, CalculateItemAount :2643, CalculateTaxAmount :2686   entry-row maths
 *   CalculateWeight/Amount/TaxAmount(GridEXRow) :3638-3707                     grid-cell maths
 *   txtExchangeRate_TextChanged :557 + CalculateTotalInformation :628          FcyAmount + totals
 *   btnplus_Click :1408, grd_DoubleClick :1591, btnUpdateDetail_Click :1653, btnCancelUpdateDetial :1715
 *   grd_ColumnButtonClick :1441 (Delete)   Insert :1722   ReadById :1931   Reset :1990   ResetDetail :2032
 *   gridhistoryfill :2181, grdhistory_ColumnButtonClick :2406, grdhistory_SelectionChanged :2514
 *   PurchsaeOrder_KeyDown :3151
 * The server repeats every validation and every sum; nothing typed here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/purchase-order';
    var L = null, cfg = {}, feat = {}, perms = {};
    var table = [];                 // the grid's DataTable
    var updateDetailIndex = -1;
    var RecId = 0;
    var removedIds = [];            // OrderDetailRemoveIds
    var mode = 'save';              // save | update | saveas
    var refDocRows = [];
    var taxRows = [];
    var newFiles = [], removeAttachmentIds = [], existingAttachments = [];
    var historyRows = [];

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    /* Math.Round(x, n, MidpointRounding.AwayFromZero) */
    function roundAway(x, n) {
        var f = Math.pow(10, n || 0);
        var s = x < 0 ? -1 : 1;
        return s * Math.round(Math.abs(x) * f + 1e-9) / f;
    }
    function fixed(n, dp) { return num(n).toLocaleString('en-US', { minimumFractionDigits: dp, maximumFractionDigits: dp }); }
    function upTo(n, dp) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dp }); }
    function amt(n) { return fixed(n, cfg.amountFormatDecimals || 0); }
    function rate(n) { return fixed(n, cfg.rateFormatDecimals === undefined ? 2 : cfg.rateFormatDecimals); }
    function fcyf(n) { return fixed(n, cfg.fcyFormatDecimals || 0); }
    function today() { var d = new Date(); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function addDays(iso, days) {
        var p = String(iso || today()).split('-'); var d = new Date(+p[0], +p[1] - 1, +p[2]); d.setDate(d.getDate() + days);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function dateOnly(v) {
        if (!v) return '';
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(v); if (isNaN(d.getTime())) return '';
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function mmmyyyy(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return MON[+p[1] - 1] + '-' + p[0]; }
    function ddmmmyyyy(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }
    function stamp(v) {
        if (!v) return ''; var d = new Date(v); if (isNaN(d.getTime())) return String(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return String(d.getDate()).padStart(2, '0') + '-' + MON[d.getMonth()] + '-' + d.getFullYear() + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap;
    }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
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
        /* dtitem :425-429, AllColumns - Id is the value member. The first column follows the
           Name / Code radio (ItemNameBind :1045). */
        window.DesktopCombo.define('itemPm', [
            { caption: 'Item Name', flex: 4 },
            { caption: 'ItemCode', flex: 2, key: 'code' },
            { caption: 'LeadTimeDay', flex: 1, key: 'lead', type: 'num' },
            { caption: 'WeightCapacity', flex: 2, key: 'cap' }
        ]);
        /* CreateRefDocOrInvoiceTable :1157 - Id, No, RelatedId with AllColumns. */
        window.DesktopCombo.define('refPm', [
            { caption: 'No', flex: 3 },
            { caption: 'RelatedId', flex: 1, key: 'rel', type: 'num' }
        ]);
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
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function selText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o && o.value ? o.textContent : ''; }
    function selData(id, key) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o ? (o.getAttribute('data-' + key) || '') : ''; }

    // ------------------------------------------------------------------ load

    function init() {
        defineFamilies();
        bindEvents();
        setVal('DocDate', today());
        setVal('deliverystartdate', today());
        setVal('duedate', today());
        setVal('txtPackingDate', today());
        setVal('txtExpiryDateDetail', today());
        renderGrid();
        return loadLookups(true).then(function () { showTab(0); }).catch(function (e) { say('Not loaded.'); box(e.message); });
    }

    function loadLookups(first) {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d || {}; cfg = L.configuration || {}; feat = L.features || {}; perms = L.permissions || {};
            fill('combsuppname', L.suppliers, 'Id', 'CompanyName', [{ attr: 'code', key: 'PartyCode' }, { attr: 'city', key: 'CityName' }, { attr: 'mobile', key: 'MobileNo' }]);
            fill('combpttrm', L.paymentTerms, 'Id', 'Description');
            fill('cmbCurrency', L.currencies, 'Id', 'Description');
            fill('CmbCropYear', L.cropYears, 'Id', 'Description');
            fill('CmbRefDocumentType', L.refDocumentTypes, 'Id', 'type');
            ItemCategoryOrTypeBind();
            ItemDtsFillAndBind();
            multiCurrencyFeature();
            if (first) {
                $id(cfg.itemSearchByCode ? 'rdSearchByCode' : 'rdSearchByName').checked = true;
                setVal('txtdocno', L.docNo); setVal('txtBranchSrNo', L.branchSrNo);
                $id('txtDocNoShow').textContent = ' PO-' + (L.docNo || '');
                HistoryComboBind(L.history);
                setVal('FromDateHistory', addDays(today(), -(cfg.defaultDaysToLessFromHistoryFromDate > 0 ? cfg.defaultDaysToLessFromHistoryFromDate : 3)));
                setVal('ToDateHistory', today());
                setVal('combdeliverytrm', '1');                             // Rows[0].Activate() :920
                setVal('CmbBillType', '1');                                 // :939
                GetConfigurationsFromGlobalAndBindValuesInColumns();
            }
            $id('btnLoader').style.display = feat.mrpPlanning ? '' : 'none';
            $id('ChkBoxPrint').checked = !!perms.Print;                    // :506
            applyRights();
            if (window.DesktopCombo) window.DesktopCombo.refresh();
            say('');
        });
    }

    /** multiCurrencyFeature :590 */
    function multiCurrencyFeature(fromRead) {
        Array.prototype.forEach.call(document.querySelectorAll('.fcy'), function (e) { e.style.display = feat.multiCurrency ? '' : 'none'; });
        if (!feat.multiCurrency && fromRead) {
            if (cfg.configuredCurrencyId) setVal('cmbCurrency', String(cfg.configuredCurrencyId));
            if (cfg.configuredCurrencyRate) { setVal('txtExchangeRate', cfg.configuredCurrencyRate); txtExchangeRate_TextChanged(); }
        }
    }

    /** GetConfigurationsFromGlobalAndBindValuesInColumns :783 - also what Reset() re-applies. */
    function GetConfigurationsFromGlobalAndBindValuesInColumns() {
        if (cfg.defaultCropYearId) setVal('CmbCropYear', String(cfg.defaultCropYearId));
        setVal('cmbCurrency', cfg.baseCurrencyId ? String(cfg.baseCurrencyId) : '');
        setVal('txtExchangeRate', rate(cfg.baseCurrencyRate || 0));
        $id('txtTaxPercnt').disabled = !cfg.taxPercentEditable;
        txtExchangeRate_TextChanged();
    }

    function applyRights() {
        var s = $id('btnsave'), u = $id('btnUpdate'), a = $id('BtnSaveAs');
        s.style.display = mode === 'save' ? '' : 'none';
        u.style.display = mode === 'update' ? '' : 'none';
        a.style.display = mode === 'saveas' ? '' : 'none';
        if (!s.classList.contains('is-busy')) s.disabled = !perms.Save;
        if (!u.classList.contains('is-busy')) u.disabled = !perms.Update;
        if (!a.classList.contains('is-busy')) a.disabled = !perms.Save;
        $id('btnprint').disabled = !perms.Print;
    }

    // ------------------------------------------------------------------ items

    function pmItems() { return (L && L.items) || []; }

    /** ItemCategoryOrTypeBind :960 */
    function ItemCategoryOrTypeBind() {
        var byCat = $id('RadCategory').checked, seen = {}, rows = [];
        pmItems().forEach(function (it) {
            var id = byCat ? it.ItemCategoryId : it.ItemTypeId, d = byCat ? it.ItemCategory : it.ItemType;
            if (!d || seen[d]) return; seen[d] = 1; rows.push({ Id: id, Description: d });
        });
        $id('CmbCategory').setAttribute('data-dtcombo-caption', byCat ? 'Item Category' : 'Item Type');
        fill('CmbCategory', rows, 'Id', 'Description');
    }

    /** ItemDtsFillFromGlobal :1014 + ItemNameBind :1040 */
    function ItemDtsFillAndBind() {
        var cat = int(val('CmbCategory')), byCat = $id('RadCategory').checked, byName = $id('rdSearchByName').checked;
        var rows = pmItems().filter(function (it) {
            return cat === 0 || (byCat && it.ItemCategoryId === cat) || (!byCat && it.ItemTypeId === cat);
        });
        $id('combitem').setAttribute('data-dtcombo-caption', byName ? 'Item Name' : 'Item Code');
        if (window.DesktopCombo) window.DesktopCombo.define('itemPm', [
            { caption: byName ? 'Item Name' : 'Item Code', flex: 4 },
            { caption: byName ? 'ItemCode' : 'ItemName', flex: 3, key: 'code' },
            { caption: 'LeadTimeDay', flex: 1, key: 'lead', type: 'num' },
            { caption: 'WeightCapacity', flex: 2, key: 'cap' }
        ]);
        fill('combitem', rows, 'Id', byName ? 'ItemName' : 'ItemCode',
            [{ attr: 'code', key: byName ? 'ItemCode' : 'ItemName' }, { attr: 'lead', key: 'LeadTimeDay' }, { attr: 'cap', key: 'WeightCapacity' }]);
        UomFromGlobalBind(int(val('combitem')));
    }
    function itemById(id) { for (var i = 0; i < pmItems().length; i++) if (pmItems()[i].Id === id) return pmItems()[i]; return null; }

    /** UomFromGlobalBind :1069 - keeps the previously chosen UOM by its code. */
    function UomFromGlobalBind(itemId) {
        var rows = ((L && L.uoms) || []).filter(function (u) { return u.ItemId === itemId; });
        var packText = selText('CmbUomDetail'), rateText = selText('combrateuom');
        var extra = [{ attr: 'eq', key: 'Equivalent' }, { attr: 'base', key: 'BaseRateUom' }, { attr: 'base-pack', key: 'BasePackUom' }];
        fill('CmbUomDetail', rows, 'Id', 'UOMCode', extra);
        fill('combrateuom', rows, 'Id', 'UOMCode', extra);
        [['CmbUomDetail', packText], ['combrateuom', rateText]].forEach(function (p) {
            if (!p[1]) return;
            var s = $id(p[0]);
            for (var i = 0; i < s.options.length; i++) if (s.options[i].textContent === p[1]) { s.selectedIndex = i; break; }
        });
    }
    function uomEq(itemId, uomId) {
        var u = ((L && L.uoms) || []).filter(function (x) { return x.ItemId === itemId && x.Id === uomId; })[0];
        return u ? num(u.Equivalent) : 0;
    }

    /** combitem_Leave_1 :2839 */
    function combitem_Leave() {
        var itemId = int(val('combitem'));
        if (itemId > 0) {
            var q = '?itemId=' + itemId + '&supplierId=' + int(val('combsuppname')) + '&docDate=' + encodeURIComponent(val('DocDate') || today());
            return getJson(api + '/item-defaults' + q).then(function (d) {
                taxRows = (d && d.taxes) || [];
                fill('CmbTaxName', taxRows, 'Id', 'TaxType');
                if (!taxRows.length) { setVal('txtTaxPercnt', ''); setVal('txtTaxAmount', ''); }
                if (updateDetailIndex > -1 && table[updateDetailIndex] && table[updateDetailIndex].TaxNameId > 0) {
                    setVal('CmbTaxName', String(table[updateDetailIndex].TaxNameId));      // :2900
                }
                var lead = int(d && d.leadTime);
                if (int(val('txtLeadtimeDetail')) === 0) {
                    setVal('txtLeadtimeDetail', lead > 0 ? lead : (itemById(itemId) || {}).LeadTimeDay);
                }
                $id('label7').textContent = 'LeadTime    ' + lead;
                setVal('txtWeightCapacity', (itemById(itemId) || {}).WeightCapacity || '');
                UomFromGlobalBind(itemId);
                CalculateTaxAmount();
            }).catch(function (e) { box(e.message); });
        }
        setVal('CmbTaxName', ''); fill('CmbTaxName', [], 'Id', 'TaxType'); setVal('txtTaxPercnt', ''); setVal('txtTaxAmount', '');
        UomFromGlobalBind(itemId);
        return Promise.resolve();
    }

    /** combsuppname_Leave :2911 */
    function combsuppname_Leave() {
        var q = '?itemId=' + int(val('combitem')) + '&supplierId=' + int(val('combsuppname')) + '&docDate=' + encodeURIComponent(val('DocDate') || today());
        getJson(api + '/item-defaults' + q).then(function (d) {
            var lead = int(d && d.leadTime);
            if (int(val('txtLeadtimeDetail')) === 0) setVal('txtLeadtimeDetail', lead);
            $id('label7').textContent = 'LeadTime    ' + lead;
        }).catch(function () { /* the desktop shows nothing here either on an empty item */ });
    }

    // ------------------------------------------------------------------ entry-row maths

    function CalculateWeight() {                                            // :2622
        var q = num(val('txtqty')), w = num(val('txtWtPerQty'));
        setVal('txtTotalWeight', (q > 0 && w > 0) ? upTo(w * q, 3) : '');
    }
    function CalculateItemAount() {                                          // :2643
        var rateUomId = int(val('combrateuom')), bill = int(val('CmbBillType'));
        if (rateUomId && bill) {
            var q = num(val('txtqty')), w = num(val('txtTotalWeight')), r = num(val('txtRate')), eq = num(selData('combrateuom', 'eq'));
            if (q > 0 && eq > 0 && r > 0 && w > 0) {
                var a = bill === 1 ? w / eq * r : (bill === 2 ? q / eq * r : 0);
                a = roundAway(a, cfg.amountDecimals || 0);
                setVal('txtItemAmount', amt(a)); setVal('txtTotalAmount', amt(a));
            } else { setVal('txtItemAmount', '0'); setVal('txtTotalAmount', '0'); }
            CalculateTaxAmount();
        } else { setVal('txtItemAmount', '0'); setVal('txtTotalAmount', '0'); }
    }
    function CalculateTaxAmount() {                                          // :2686
        var a = num(val('txtItemAmount')), taxId = int(val('CmbTaxName'));
        if (taxId) {
            var sched = num((taxRows.filter(function (t) { return t.Id === taxId; })[0] || {}).TaxPrcnt), pct;
            if (!cfg.taxPercentEditable || num(val('txtTaxPercnt')) === 0) { setVal('txtTaxPercnt', upTo(sched, 3)); pct = sched; }
            else pct = num(val('txtTaxPercnt'));
            var t = a * pct / 100;
            setVal('txtTaxAmount', amt(roundAway(t, cfg.amountDecimals || 0)));
            setVal('txtTotalAmount', amt(roundAway(a + t, cfg.amountDecimals || 0)));
        } else if (mode === 'save' && perms.Save) {
            setVal('txtTaxPercnt', '0'); setVal('txtTaxAmount', '0'); setVal('txtTotalAmount', amt(a));
        }
    }
    function recalcEntry() { CalculateWeight(); CalculateItemAount(); }

    // ------------------------------------------------------------------ grid-row maths :3638-3707

    function gridCalcWeight(r) { r.TotalWeight = (r.ItemQty > 0 && r.WeightPerQty > 0) ? r.WeightPerQty * r.ItemQty : 0; }
    function gridCalcAmount(r) {
        var bill = int(val('CmbBillType')), a = 0;
        if (r.ItemQty > 0 && r.RateEquivalent > 0 && r.ItemRate > 0 && r.TotalWeight > 0) {
            if (bill === 1) a = r.TotalWeight / r.RateEquivalent * r.ItemRate; else if (bill === 2) a = r.ItemQty / r.RateEquivalent * r.ItemRate;
        }
        r.Amount = roundAway(a, cfg.amountDecimals || 0);
        r.TotalAmount = r.Amount;
    }
    function gridCalcTax(r) {
        var p = num(r.TaxPercent), t = 0, tot = r.Amount;
        if (p > 100) { p = 0; box('Tax% cannot be greater than 100. It has been reset to 0.'); }
        if (p > 0 && r.Amount > 0) { t = r.Amount * p / 100; tot = r.Amount + t; } else { p = 0; t = 0; }
        r.TaxPercent = p; r.TaxAmount = roundAway(t, cfg.amountDecimals || 0); r.TotalAmount = roundAway(tot, cfg.amountDecimals || 0);
    }

    /** txtExchangeRate_TextChanged :557 then CalculateTotalInformation :628 */
    function txtExchangeRate_TextChanged() {
        var ex = num(val('txtExchangeRate'));
        table.forEach(function (r) { r.FcyAmount = ex > 0 ? roundHalfEven(r.Amount / ex, cfg.fcyDecimals || 0) : 0; });
        CalculateTotalInformation();
        renderGrid();
    }
    function roundHalfEven(x, n) {
        var f = Math.pow(10, n), v = x * f, fl = Math.floor(v), d = v - fl;
        var r = (Math.abs(d - 0.5) < 1e-9) ? (fl % 2 === 0 ? fl : fl + 1) : Math.round(v);
        return r / f;
    }
    function CalculateTotalInformation() {
        if (!table.length) { setVal('txtOrderQty', '0'); setVal('txtOrderAmount', '0'); setVal('txtFcyAmount', '0'); return; }
        var q = 0, a = 0, f = 0;
        table.forEach(function (r) { q += num(r.ItemQty); a += num(r.Amount); f += num(r.FcyAmount); });
        setVal('txtOrderQty', upTo(roundAway(q, 2), 2));
        setVal('txtOrderAmount', amt(a));
        setVal('txtFcyAmount', fcyf(f));
    }

    // ------------------------------------------------------------------ the grid

    var GRID_COLS = [
        ['ItemCode', 't'], ['ItemName', 't'], ['CropYear', 't'], ['WeightCapacity', 't'], ['PackingDate', 'date'], ['ExpiryDate', 'date'],
        ['PackUom', 't'], ['ItemQty', 'edit-num'], ['WeightPerQty', 'edit-num'], ['TotalWeight', 'n3'], ['ItemRate', 'edit-rate'],
        ['RateUom', 't'], ['Amount', 'amt'], ['FcyAmount', 'fcy'], ['TaxName', 't'], ['Tax%', 'tax'], ['TaxAmount', 'amt'],
        ['TotalAmount', 'amt'], ['LeadTime', 'edit-int'], ['RefDocumentType', 't'], ['RefDocNo', 't'], ['RefDocInvoiceNo', 't'], ['Remarks', 'edit-text']
    ];
    function gridKey(c) { return c === 'Tax%' ? 'TaxPercent' : c; }

    function renderGrid() {
        var cols = GRID_COLS.filter(function (c) { return c[0] !== 'FcyAmount' || feat.multiCurrency; });
        $id('grdHead').innerHTML = cols.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '<th>Delete</th>';
        var html = '';
        table.forEach(function (r, i) {
            html += '<tr class="data-row' + (i === updateDetailIndex ? ' sel' : '') + '" data-i="' + i + '">';
            cols.forEach(function (c) {
                var k = gridKey(c[0]), v = r[k];
                switch (c[1]) {
                    case 'date': html += '<td><input type="date" class="cell-date" data-k="' + k + '" value="' + esc(dateOnly(v)) + '" title="' + esc(mmmyyyy(v)) + '"/></td>'; break;
                    case 'edit-num': html += '<td><input type="text" class="cell" data-guard="decimal" data-k="' + k + '" value="' + esc(upTo(v, 3)) + '"/></td>'; break;
                    case 'edit-rate': html += '<td><input type="text" class="cell" data-guard="decimal" data-k="' + k + '" value="' + esc(rate(v)) + '"/></td>'; break;
                    case 'edit-int': html += '<td><input type="text" class="cell" data-guard="integer" data-k="' + k + '" value="' + esc(int(v)) + '"/></td>'; break;
                    case 'edit-text': html += '<td><input type="text" class="cell-text" data-k="' + k + '" value="' + esc(v) + '"/></td>'; break;
                    case 'tax': html += cfg.taxPercentEditable
                            ? '<td><input type="text" class="cell" data-guard="decimal" data-k="TaxPercent" value="' + esc(upTo(v, 2)) + '"/></td>'
                            : '<td class="num">' + esc(upTo(v, 2)) + '</td>'; break;
                    case 'n3': html += '<td class="num">' + esc(upTo(v, 3)) + '</td>'; break;
                    case 'amt': html += '<td class="num">' + esc(amt(v)) + '</td>'; break;
                    case 'fcy': html += '<td class="num">' + esc(fcyf(v)) + '</td>'; break;
                    default: html += '<td>' + esc(v) + '</td>';
                }
            });
            html += '<td><button type="button" class="win-btn-mini" data-del="' + i + '">X</button></td></tr>';
        });
        $id('grd').innerHTML = html;
        // footer - AggregateFunction.Sum on amount/qty columns (ConfigureNumericalColumn :1484)
        var sums = {};
        table.forEach(function (r) { ['ItemQty', 'WeightPerQty', 'TotalWeight', 'Amount', 'FcyAmount', 'TaxAmount', 'TotalAmount'].forEach(function (k) { sums[k] = (sums[k] || 0) + num(r[k]); }); });
        $id('grdFoot').innerHTML = cols.map(function (c) {
            var k = gridKey(c[0]);
            if (!(k in sums)) return '<td></td>';
            return '<td class="num">' + esc(/Amount/.test(k) ? (k === 'FcyAmount' ? fcyf(sums[k]) : amt(sums[k])) : upTo(sums[k], 2)) + '</td>';
        }).join('') + '<td></td>';
    }

    function onGridInput(e) {
        var t = e.target; if (!t || !t.getAttribute) return;
        var k = t.getAttribute('data-k'); if (!k) return;
        var tr = t.closest('tr'); var r = table[int(tr.getAttribute('data-i'))]; if (!r) return;
        if (k === 'PackingDate' || k === 'ExpiryDate') { r[k] = t.value; return; }
        if (k === 'Remarks') { r[k] = t.value; return; }
        if (k === 'LeadTime') { r[k] = int(t.value); return; }
        r[k] = num(t.value);
        switch (k) {                                                           // grd_CellUpdated :3603
            case 'ItemQty': case 'WeightPerQty': gridCalcWeight(r); gridCalcAmount(r); gridCalcTax(r); break;
            case 'ItemRate': gridCalcAmount(r); gridCalcTax(r); break;
            case 'TaxPercent': gridCalcTax(r); break;
        }
        txtExchangeRate_TextChanged();
    }

    /** grd_ColumnButtonClick :1441 */
    function deleteRow(i) {
        var r = table[i]; if (!r) return;
        if (updateDetailIndex === i) { box("You can't delete this detail because it is in Update Mode"); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        if (r.Id > 0) {
            var saved = table.filter(function (x) { return x.Id > 0; }).length;
            if (saved <= 1) { box('All rows Can not be Deleted in Update Mode'); return; }
            removedIds.push(r.Id);
        }
        table.splice(i, 1);
        if (updateDetailIndex > i) updateDetailIndex--;
        txtExchangeRate_TextChanged();
    }

    // ------------------------------------------------------------------ add / edit a row

    function FormValidationDetail() {                                       // :1350
        var c = [
            [!int(val('combitem')), 'Item Name Field is Required', 'combitem'],
            [!int(val('CmbUomDetail')), 'Pack Uom Field is Required', 'CmbUomDetail'],
            [num(val('txtqty')) === 0, 'Item Qty Field is Required', 'txtqty'],
            [num(val('txtRate')) === 0, 'Item Rate Field is Required', 'txtRate'],
            [!int(val('combrateuom')), 'Rate UOM Field is Required', 'combrateuom'],
            [num(val('txtItemAmount')) === 0, 'ItemAmount Field is Required', 'txtItemAmount']
        ];
        for (var i = 0; i < c.length; i++) if (c[i][0]) { box(c[i][1]); focus(c[i][2]); return false; }
        return true;
    }
    function focus(id) { var e = $id(id); if (!e) return; if (e.__dtcombo && e.__dtcombo.input) e.__dtcombo.input.focus(); else e.focus(); }

    function rowFromEntry(base) {
        var itemId = int(val('combitem')), it = itemById(itemId) || {};
        var refType = int(val('CmbRefDocumentType'));
        var r = base || { Id: 0 };
        r.ItemId = itemId; r.ItemCode = it.ItemCode; r.ItemName = it.ItemName;
        r.CropYearId = int(val('CmbCropYear')); r.CropYear = selText('CmbCropYear');
        r.WeightCapacity = val('txtWeightCapacity'); r.PackingDate = val('txtPackingDate'); r.ExpiryDate = val('txtExpiryDateDetail');
        r.PackUomId = int(val('CmbUomDetail')); r.PackUom = selText('CmbUomDetail'); r.PackEquivalent = num(selData('CmbUomDetail', 'eq'));
        r.ItemQty = num(val('txtqty')); r.WeightPerQty = num(val('txtWtPerQty')); r.TotalWeight = num(val('txtTotalWeight'));
        r.ItemRate = num(val('txtRate')); r.RateUomId = int(val('combrateuom')); r.RateUom = selText('combrateuom'); r.RateEquivalent = num(selData('combrateuom', 'eq'));
        r.Amount = num(val('txtItemAmount')); r.FcyAmount = num(val('txtItemAmount'));
        r.TaxNameId = int(val('CmbTaxName')); r.TaxName = selText('CmbTaxName'); r.TaxPercent = num(val('txtTaxPercnt'));
        r.TaxAmount = num(val('txtTaxAmount')); r.TotalAmount = num(val('txtTotalAmount')); r.LeadTime = int(val('txtLeadtimeDetail'));
        r.RefDocumentTypeId = refType; r.RefDocumentType = selText('CmbRefDocumentType');
        r.RefDocId = refType > 0 ? int(val('CmbRefDocNo')) : 0; r.RefDocNo = refType > 0 ? selText('CmbRefDocNo') : '';
        r.RefDocInvoiceId = refType > 0 ? int(val('CmbRefDocInvoiceNo')) : 0; r.RefDocInvoiceNo = refType > 0 ? selText('CmbRefDocInvoiceNo') : '';
        r.Remarks = val('txtremarksdetail').trim();
        return r;
    }

    function btnplus_Click() {                                              // :1408
        if (!FormValidationDetail()) return;
        table.push(rowFromEntry(null));
        ResetDetail();
        txtExchangeRate_TextChanged();
    }

    function grd_DoubleClick(i) {                                          // :1591
        var r = table[i]; if (!r) return;
        setVal('CmbCategory', '');
        ItemDtsFillAndBind();
        updateDetailIndex = i;
        setVal('combitem', String(r.ItemId));
        combitem_Leave().then(function () {
            setVal('CmbCropYear', String(r.CropYearId));
            setVal('txtWeightCapacity', r.WeightCapacity);
            setVal('CmbUomDetail', String(r.PackUomId));
            setVal('txtqty', upTo(r.ItemQty, 3)); setVal('txtWtPerQty', upTo(r.WeightPerQty, 3)); setVal('txtTotalWeight', upTo(r.TotalWeight, 3));
            setVal('combrateuom', String(r.RateUomId));
            setVal('txtRate', rate(r.ItemRate)); setVal('txtItemAmount', amt(r.Amount));
            if (r.TaxNameId > 0) setVal('CmbTaxName', String(r.TaxNameId));
            setVal('txtTaxPercnt', upTo(r.TaxPercent, 2)); setVal('txtTaxAmount', amt(r.TaxAmount)); setVal('txtTotalAmount', amt(r.TotalAmount));
            setVal('txtPackingDate', dateOnly(r.PackingDate) || today()); setVal('txtExpiryDateDetail', dateOnly(r.ExpiryDate) || today());
            setVal('txtLeadtimeDetail', r.LeadTime);
            setVal('txtremarksdetail', r.Remarks || '');
            var afterRef = Promise.resolve();
            if (r.RefDocumentTypeId > 0) {
                setVal('CmbRefDocumentType', String(r.RefDocumentTypeId));
                afterRef = CmbRefDocumentType_Leave().then(function () {
                    if (r.RefDocId > 0) setVal('CmbRefDocNo', String(r.RefDocId));
                    if (r.RefDocInvoiceId > 0) setVal('CmbRefDocInvoiceNo', String(r.RefDocInvoiceId));
                });
            }
            return afterRef;
        }).then(function () {
            $id('btnplus').style.display = 'none';
            $id('btnUpdateDetail').style.display = '';
            $id('btnCancelUpdateDetial').style.display = '';
            renderGrid();
            focus('combitem');
        });
    }

    function btnUpdateDetail_Click() {                                      // :1653
        if (updateDetailIndex < 0 || !FormValidationDetail()) return;
        rowFromEntry(table[updateDetailIndex]);
        ResetDetail();
        txtExchangeRate_TextChanged();
    }

    function btnCancelUpdateDetial_Click() {                               // :1715 - buttons only, as written
        $id('btnplus').style.display = '';
        $id('btnUpdateDetail').style.display = 'none';
        $id('btnCancelUpdateDetial').style.display = 'none';
    }

    function ResetDetail() {                                                // :2032
        updateDetailIndex = -1;
        ['combitem', 'CmbUomDetail', 'CmbCropYear', 'txtqty', 'txtWeightCapacity', 'txtLeadtimeDetail', 'combrateuom', 'txtRate',
         'txtItemAmount', 'txtWtPerQty', 'txtTotalWeight', 'CmbTaxName', 'txtTaxPercnt', 'CmbRefDocumentType'].forEach(function (id) { setVal(id, ''); });
        refDocRows = []; fill('CmbRefDocNo', [], 'Id', 'No'); fill('CmbRefDocInvoiceNo', [], 'Id', 'No');
        btnCancelUpdateDetial_Click();
        renderGrid();
        focus('combitem');
    }

    // ------------------------------------------------------------------ ref docs :1099-1231

    function CmbRefDocumentType_Leave() {
        var t = int(val('CmbRefDocumentType'));
        refDocRows = [];
        if (t <= 0) { bindRefDocs(); return Promise.resolve(); }
        return getJson(api + '/ref-docs?refDocumentTypeId=' + t + '&recId=' + RecId).then(function (rows) {
            refDocRows = rows || []; bindRefDocs();
        }).catch(function (e) { box(e.message); });
    }
    function refTable(idF, noF, relF) {
        var seen = {}, out = [];
        refDocRows.forEach(function (r) { var id = int(col(r, idF)); if (id > 0 && !seen[id]) { seen[id] = 1; out.push({ Id: id, No: col(r, noF), RelatedId: int(col(r, relF)) }); } });
        return out;
    }
    function bindRefDocs() {
        fill('CmbRefDocNo', refTable('RefDocId', 'RefDocNo', 'RefDocInvoiceId'), 'Id', 'No', [{ attr: 'rel', key: 'RelatedId' }]);
        fill('CmbRefDocInvoiceNo', refTable('RefDocInvoiceId', 'RefDocInvoiceNo', 'RefDocId'), 'Id', 'No', [{ attr: 'rel', key: 'RelatedId' }]);
    }
    function syncRef(from, to) {                                            // SyncComboSelection :1213
        if (int(val(from)) <= 0) return;
        var rel = int(selData(from, 'rel'));
        var s = $id(to);
        if (s.querySelector('option[value="' + rel + '"]')) s.value = String(rel); else s.value = '';
    }

    // ------------------------------------------------------------------ header events

    function combpttrm_TextChanged() {                                     // :3018
        var t = int(val('combpttrm'));
        if (t === 1) { setVal('txtduedays', ''); $id('txtduedays').disabled = true; setVal('duedate', val('DocDate')); }
        else if (t === 2) $id('txtduedays').disabled = false;
    }
    function txtduedays_TextChanged() {                                     // :2928
        var d = val('txtduedays').trim();
        setVal('duedate', d ? addDays(val('DocDate'), int(d)) : val('DocDate'));
    }
    function DocDate_Leave() {                                              // :2947 - both branches set the start date to DocDate
        var start = val('deliverystartdate'), doc = val('DocDate');
        if (start && doc && start < doc) {
            setVal('deliverystartdate', doc); txtduedays_TextChanged();
            box("Delivery Start Date Can't Be Less Than Doc Date");
        } else setVal('deliverystartdate', doc);
    }
    function deliverystartdate_ValueChanged() {                            // :3039
        if (val('deliverystartdate') < val('DocDate')) { setVal('deliverystartdate', val('DocDate')); box("Delivery Start Date Can't Be Less Than Doc Date"); }
    }
    function CmbBillType_TextChanged() {                                    // :2968
        if (!int(val('CmbBillType'))) return;
        CalculateItemAount();
        table.forEach(function (r) {
            var bill = int(val('CmbBillType')), a = 0;
            if (bill === 1) a = r.TotalWeight / r.RateEquivalent * r.ItemRate; else if (bill === 2) a = r.ItemQty / r.RateEquivalent * r.ItemRate;
            if (!isFinite(a)) a = 0;
            r.Amount = roundAway(a, cfg.amountDecimals || 0);
            var t = 0;
            if (a > 0 && num(r.TaxPercent) > 0) { t = a * num(r.TaxPercent) / 100; r.TaxAmount = roundAway(t, cfg.amountDecimals || 0); }
            r.TotalAmount = roundAway(a + t, cfg.amountDecimals || 0);
        });
        txtExchangeRate_TextChanged();
    }
    function cmbCurrency_Leave() {                                          // :655
        var c = int(val('cmbCurrency')); if (!c) return;
        getJson(api + '/exchange-rate?currencyId=' + c).then(function (d) {
            if (d && d.exchangeRate !== undefined) { setVal('txtExchangeRate', rate(d.exchangeRate)); txtExchangeRate_TextChanged(); }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ save :1722

    function FormValidation() {                                            // :1285
        var c = [[!int(val('combsuppname')), 'Supplier Name Field is Required', 'combsuppname'],
                 [!int(val('combpttrm')), 'Payment Term Field is Required', 'combpttrm'],
                 [!int(val('combdeliverytrm')), 'Delivery Term Field is Required', 'combdeliverytrm']];
        if (feat.multiCurrency) {
            c.push([!int(val('cmbCurrency')), 'Fcy Code Field is Required', 'cmbCurrency']);
            c.push([num(val('txtExchangeRate')) === 0, 'Exchange Rate Field is Required', 'txtExchangeRate']);
            c.push([num(val('txtFcyAmount')) === 0, 'Fcy Amount Rate Field is Required', 'txtFcyAmount']);
        } else {
            c.push([!int(val('cmbCurrency')), 'Please Configure Your Base Currency In configurations', 'cmbCurrency']);
            c.push([num(val('txtExchangeRate')) === 0, 'Please Configure Your Base Currency Rate In configurations', 'txtExchangeRate']);
        }
        c.push([!int(val('CmbBillType')), 'Bill Type Field is Required', 'CmbBillType']);
        for (var i = 0; i < c.length; i++) if (c[i][0]) { box(c[i][1]); focus(c[i][2]); return false; }
        return true;
    }

    function Insert(btnId) {
        if (!table.length) { box('Grid Record Not Found'); return; }
        if (!FormValidation()) return;
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        txtExchangeRate_TextChanged();
        var payload = {
            Id: RecId, DocDate: val('DocDate'), DocNo: int(val('txtdocno')), BranchSrNo: int(val('txtBranchSrNo')),
            OrderSupCustId: int(val('combsuppname')), SupplierRefNo: val('txtsupprefno'), RemarksHeader: val('txtremarks'),
            PaymentTermsId: int(val('combpttrm')), OrderDueDays: val('txtduedays'), DeliveryTermId: int(val('combdeliverytrm')),
            DeliveryStartDate: val('deliverystartdate'), DeliveryDays: val('txtdeliverydays'), BillCalculateTypeId: int(val('CmbBillType')),
            CurrencyId: int(val('cmbCurrency')), ExchangeRate: val('txtExchangeRate'),
            removedDetailIds: RecId > 0 ? removedIds.slice() : [],
            lines: table.map(function (r) {
                return { Id: RecId > 0 ? int(r.Id) : 0, ItemId: r.ItemId, CropYearId: r.CropYearId, WeightCapacity: r.WeightCapacity || '',
                    PackingDate: dateOnly(r.PackingDate), ExpiryDate: dateOnly(r.ExpiryDate), PackUomId: r.PackUomId, ItemQty: num(r.ItemQty),
                    WeightPerQty: num(r.WeightPerQty), RateUomId: r.RateUomId, ItemRate: num(r.ItemRate), TaxNameId: int(r.TaxNameId),
                    TaxPercent: num(r.TaxPercent), LeadTime: int(r.LeadTime), RefDocumentTypeId: int(r.RefDocumentTypeId),
                    RefDocId: int(r.RefDocId), RefDocInvoiceId: int(r.RefDocInvoiceId), Remarks: r.Remarks || '' };
            }),
            files: newFiles.slice(), removeAttachmentIds: removeAttachmentIds.slice()
        };
        return busy(btnId, function () {
            say(RecId > 0 ? 'Updating...' : 'Saving...');
            return http('POST', api + '/save', payload).then(function (d) {
                box(d.message);
                var id = d.id;
                return Reset().then(function () { if ($id('ChkBoxPrint').checked) GenerateReport(id); });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }
    function btnsave_Click() { if (mode !== 'save' || $id('btnsave').disabled) return; RecId = 0; return Insert('btnsave'); }
    function BtnSaveAs_Click() { if (mode !== 'saveas') return; RecId = 0; return Insert('BtnSaveAs'); }
    function btnupdate_Click() { if (mode !== 'update' || $id('btnUpdate').disabled) return; return Insert('btnUpdate'); }

    /** GenerateReport :2815 - CommonServices.PurchaseOrderPackingMaterialSlip215. The Crystal layout
        is not rendered on the web; the report's own rows are opened instead. */
    function GenerateReport(id) {
        if (!id) { box('No Record Found For Display'); return; }
        window.open('/api/reports/po-pm-215/data?id=' + encodeURIComponent(id), '_blank');
    }
    function btnprint_Click() { if (!perms.Print) return; GenerateReport(RecId); }

    // ------------------------------------------------------------------ new / reset / read

    function Reset() {                                                     // :1990
        removedIds = []; RecId = 0; mode = 'save';
        newFiles = []; removeAttachmentIds = []; existingAttachments = []; renderAttachments();
        ['txtdocno', 'txtBranchSrNo', 'combsuppname', 'txtsupprefno', 'txtduedays', 'txtdeliverydays', 'txtOrderAmount', 'txtOrderQty',
         'txtFcyAmount', 'txtExchangeRate'].forEach(function (id) { setVal(id, ''); });
        setVal('combdeliverytrm', '1');
        $id('label7').textContent = 'LeadTime';
        ResetDetail();
        table = []; renderGrid();
        applyRights();
        return getJson(api + '/numbers').then(function (n) {
            setVal('txtdocno', n.docNo); setVal('txtBranchSrNo', n.branchSrNo);
            $id('txtDocNoShow').textContent = ' PO-' + n.docNo;
            GetConfigurationsFromGlobalAndBindValuesInColumns();
            focus('DocDate');
        }).catch(function (e) { box(e.message); });
    }
    function btnnew_Click() { return Reset(); }

    /** toolStripButton1_Click :2068 - re-reads every global list. */
    function toolStripButton1_Click() { return busy('toolStripButton1', function () { return loadLookups(false).catch(function (e) { box(e.message); }); }); }

    /** ReadById :1931 */
    function ReadById(id, asSaveAs) {
        return getJson(api + '/' + id).then(function (d) {
            var h = d.header || {};
            RecId = id; removedIds = [];
            mode = asSaveAs ? 'saveas' : 'update';
            showTab(0);
            setVal('txtdocno', col(h, 'DocNo')); $id('txtDocNoShow').textContent = 'PO-' + col(h, 'DocNo');
            setVal('txtBranchSrNo', col(h, 'BranchSrNo'));
            setVal('DocDate', dateOnly(col(h, 'DocDate')));
            setVal('combsuppname', String(int(col(h, 'OrderSupCustId'))));
            setVal('txtsupprefno', col(h, 'SupplierRefNo'));
            setVal('txtremarks', col(h, 'RemarksHeader'));
            setVal('combpttrm', String(int(col(h, 'PaymentTermsId'))));
            setVal('txtduedays', col(h, 'OrderDueDays'));
            setVal('duedate', dateOnly(col(h, 'OrderDueDate')));
            var dt = String(col(h, 'DeliveryTerm') || '');
            setVal('combdeliverytrm', dt === 'Load' ? '1' : (dt === 'Ponch' ? '2' : ''));
            setVal('deliverystartdate', dateOnly(col(h, 'DeliveryStartDate')));
            setVal('txtdeliverydays', col(h, 'DeliveryDays'));
            setVal('CmbBillType', String(int(col(h, 'BillCalculateTypeId'))));
            setVal('txtqty', upTo(col(h, 'OrderQty'), 3));      // :1958 writes OrderQty into the DETAIL qty box, as the desktop does
            setVal('txtOrderAmount', amt(col(h, 'OrderAmount')));
            setVal('cmbCurrency', String(int(col(h, 'CurrencyId'))));
            setVal('txtExchangeRate', rate(col(h, 'ExchangeRate')));
            setVal('txtFcyAmount', fcyf(col(h, 'FcyAmount')));
            if (int(col(h, 'CurrencyId')) === 0) multiCurrencyFeature(true);
            table = (d.lines || []).map(function (l) {
                var r = {}; for (var k in l) r[k] = l[k];
                ['ItemQty', 'WeightPerQty', 'TotalWeight', 'ItemRate', 'RateEquivalent', 'Amount', 'FcyAmount', 'TaxPercent', 'TaxAmount', 'TotalAmount']
                    .forEach(function (k) { r[k] = num(r[k]); });
                r.Amount = roundAway(r.Amount, cfg.amountDecimals || 0);
                if (!r.RateEquivalent) r.RateEquivalent = uomEq(r.ItemId, r.RateUomId);
                if (asSaveAs) r.Id = 0;                                        // grdhistory SaveAs :2424-2434
                return r;
            });
            existingAttachments = asSaveAs ? [] : (d.attachments || []);
            newFiles = []; removeAttachmentIds = []; renderAttachments();
            txtExchangeRate_TextChanged();
            applyRights();
            say('PO-' + col(h, 'DocNo') + ' opened.');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history

    function HistoryComboBind(h) {                                         // :722
        h = h || {};
        fill('cmbSupplierNameHistory', h.suppliers, 'Id', 'Description');
        var html = '';
        (h.branches || []).forEach(function (b) {
            html += '<label class="win-check" style="display:flex;"><input type="checkbox" class="hist-branch" value="' + esc(b.Id) + '"'
                  + (b.Id === (L && L.userBranchId) ? ' checked' : '') + '/> ' + esc(b.Description) + '</label>';
        });
        $id('cmbBranchName').innerHTML = html;
    }

    function gridhistoryfill() {                                           // :2181
        var branches = Array.prototype.map.call(document.querySelectorAll('.hist-branch:checked'), function (c) { return c.value; });
        if (!branches.length) { box('Select Branch First'); return; }
        var mode2 = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var q = '?dateMode=' + mode2
              + ($id('chkFromDate').checked && val('FromDateHistory') ? '&fromDate=' + val('FromDateHistory') : '')
              + ($id('chkToDate').checked && val('ToDateHistory') ? '&toDate=' + val('ToDateHistory') : '')
              + '&fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory'))
              + '&supplierId=' + int(val('cmbSupplierNameHistory')) + '&branchIds=' + encodeURIComponent(',' + branches.join(','));
        return busy('btnshow', function () {
            return getJson(api + '/history' + q).then(function (rows) { historyRows = rows || []; renderHistory(); })
                .catch(function (e) { box(e.message); });
        });
    }

    var HIST_COLS = [['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo'], ['BranchSrNo', 'BranchSrNo', 'br'], ['BranchName', 'BranchName', 'br'],
        ['SupplierName', 'SupplierName'], ['OrderQty', 'OrderQty', 'q'], ['PaymentTerm', 'TermsDescription'], ['DueDays', 'DueDays'],
        ['DueDate', 'DueDate', 'd'], ['DeliveryTerm', 'DeliveryTerm'], ['DeliveryDays', 'DeliveryDays'], ['DeliveryStartDate', 'DeliveryStartDate', 'd'],
        ['OrderExpiryDate', 'OrderExpiryDate', 'd'], ['RemarksHeader', 'RemarksHeader'], ['EntryDate', 'EntryDate', 's'], ['EntryUser', 'UserName'],
        ['ModifyDate', 'ModifyDate', 's'], ['ModifyUser', 'ModifyUserName'], ['ApprovedDate', 'PostDate', 's'], ['ApprovedUser', 'ApprovedUserName'],
        ['OrderStatus', 'OrderStatus'], ['NoOfAttachments', 'NoOfAttachments', 'att']];

    function renderHistory() {                                              // HistoryGridSettings :2326
        var cols = HIST_COLS.filter(function (c) { return !(c[2] === 'br' && cfg.purchaseOrderBranchWise); });
        $id('histHead').innerHTML = '<th>Print</th><th>SaveAs</th><th>Edit</th>' + cols.map(function (c) { return '<th>' + c[0] + '</th>'; }).join('');
        var html = '';
        historyRows.forEach(function (r, i) {
            var id = int(col(r, 'Id'));
            html += '<tr class="data-row" data-i="' + i + '" data-id="' + id + '">'
                  + '<td><button type="button" class="win-btn-mini" data-act="print">Print</button></td>'
                  + '<td><button type="button" class="win-btn-mini" data-act="saveas">SaveAs</button></td>'
                  + '<td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>';
            cols.forEach(function (c) {
                var v = col(r, c[1]);
                if (c[2] === 'd') v = ddmmmyyyy(v); else if (c[2] === 's') v = stamp(v); else if (c[2] === 'q') v = upTo(v, 2);
                html += c[2] === 'att' && int(v) > 0 ? '<td class="num"><span class="win-link" data-act="att">' + int(v) + '</span></td>' : '<td>' + esc(v) + '</td>';
            });
            html += '</tr>';
        });
        $id('grdhistory').innerHTML = html;
        $id('label11').textContent = historyRows.length ? 'Filtered Records ' + historyRows.length : '';
        $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
    }

    function onHistoryClick(e) {
        var tr = e.target.closest('tr.data-row'); if (!tr) return;
        var id = int(tr.getAttribute('data-id')), act = e.target.getAttribute && e.target.getAttribute('data-act');
        Array.prototype.forEach.call($id('grdhistory').querySelectorAll('tr.sel'), function (x) { x.classList.remove('sel'); });
        tr.classList.add('sel');
        if (act === 'edit') { if (!perms.Update) { box("ypu don't have updae rights..."); return; } Reset().then(function () { ReadById(id, false); }); return; }
        if (act === 'saveas') { Reset().then(function () { ReadById(id, true); }); return; }
        if (act === 'print') { if (perms.Print) GenerateReport(id); return; }
        if (act === 'att') { ReadById(id, false).then(function () { btnattachment_Click(true); }); return; }
        getUpdateForHistory(id);                                           // grdhistory_SelectionChanged :2514
    }

    function getUpdateForHistory(id) {                                     // :2117
        getJson(api + '/' + id).then(function (d) {
            var cols = [['ItemCode'], ['ItemName'], ['CropYear'], ['WeightCapacity'], ['PackingDate', 'm'], ['ExpiryDate', 'm'], ['PackUom'],
                ['ItemQty', 'q'], ['WeightPerQty', 'q'], ['TotalWeight', 'q'], ['ItemRate', 'r'], ['RateUom'], ['Amount', 'a'],
                ['FcyAmount', 'f'], ['TaxName'], ['TaxPercent', 'q'], ['TaxAmount', 'a'], ['TotalAmount', 'a'], ['LeadTime'],
                ['RefDocumentType'], ['RefDocNo'], ['RefDocInvoiceNo'], ['Remarks']]
                .filter(function (c) { return c[0] !== 'FcyAmount' || feat.multiCurrency; });
            $id('histDetailHead').innerHTML = cols.map(function (c) { return '<th>' + (c[0] === 'TaxPercent' ? 'Tax%' : c[0]) + '</th>'; }).join('');
            $id('grdDetail').innerHTML = (d.lines || []).map(function (l) {
                return '<tr>' + cols.map(function (c) {
                    var v = l[c[0]];
                    switch (c[1]) { case 'm': v = mmmyyyy(v); break; case 'q': v = upTo(v, 2); break; case 'r': v = rate(v); break;
                        case 'a': v = amt(v); break; case 'f': v = fcyf(v); break; }
                    return '<td' + (c[1] && c[1] !== 'm' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }

    function btnNewHistory_Click() {                                       // :2561
        setVal('FromDateHistory', addDays(today(), -(cfg.defaultDaysToLessFromHistoryFromDate > 0 ? cfg.defaultDaysToLessFromHistoryFromDate : 3)));
        setVal('ToDateHistory', today()); setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', ''); setVal('cmbSupplierNameHistory', '');
        historyRows = []; renderHistory(); $id('drdocdate').checked = true;
    }
    function btnRefreshHistory_Click() {
        return busy('btnRefreshHistory', function () {
            return getJson(api + '/lookups').then(function (d) { HistoryComboBind(d.history); }).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------ attachments

    function btnattachment_Click(force) {
        var g = $id('grpAttachments');
        g.style.display = (force === true || g.style.display === 'none') ? '' : 'none';
    }
    function onAttachmentsPicked(e) {
        Array.prototype.slice.call(e.target.files || []).forEach(function (file) {
            if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }
            var reader = new FileReader();
            reader.onload = function () { var s = String(reader.result); newFiles.push({ name: file.name, base64: s.substring(s.indexOf(',') + 1) }); renderAttachments(); };
            reader.readAsDataURL(file);
        });
        e.target.value = '';
    }
    function renderAttachments() {
        var html = '';
        existingAttachments.forEach(function (a) {
            var id = int(col(a, 'Id')); if (removeAttachmentIds.indexOf(id) >= 0) return;
            html += '<div><a class="win-link" href="' + api + '/' + RecId + '/attachments/' + id + '">' + esc(col(a, 'Attachment')) + '</a> <span class="win-link" data-remove="' + id + '">[remove]</span></div>';
        });
        newFiles.forEach(function (f, i) { html += '<div>' + esc(f.name) + ' <em>(new)</em> <span class="win-link" data-drop="' + i + '">[remove]</span></div>'; });
        $id('lstAttachments').innerHTML = html;
    }

    // ------------------------------------------------------------------ tabs, events, keys

    var currentTab = 0;
    function showTab(i) {                                                  // tabControl1_SelectedIndexChanged :2314
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none';
        $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0);
        $id('tabHistory').classList.toggle('active', i === 1);
        focus(i === 1 ? 'FromDateHistory' : 'DocDate');
    }

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function bindEvents() {
        on('combitem', 'change', combitem_Leave);
        on('combsuppname', 'change', combsuppname_Leave);
        on('RadCategory', 'change', function () { ItemCategoryOrTypeBind(); ItemDtsFillAndBind(); });
        on('RadType', 'change', function () { ItemCategoryOrTypeBind(); ItemDtsFillAndBind(); });
        on('CmbCategory', 'change', ItemDtsFillAndBind);
        on('rdSearchByName', 'change', ItemDtsFillAndBind);
        on('rdSearchByCode', 'change', ItemDtsFillAndBind);
        ['txtqty', 'txtWtPerQty', 'txtRate'].forEach(function (id) { on(id, 'input', recalcEntry); });
        on('combrateuom', 'change', recalcEntry);
        on('CmbUomDetail', 'change', recalcEntry);
        on('CmbTaxName', 'change', function () {
            if (int(val('CmbTaxName'))) CalculateTaxAmount();
            else { setVal('txtTaxPercnt', ''); setVal('txtTaxAmount', ''); setVal('txtTotalAmount', amt(num(val('txtItemAmount')))); }
        });
        on('txtTaxPercnt', 'input', CalculateTaxAmount);
        on('CmbRefDocumentType', 'change', CmbRefDocumentType_Leave);
        on('CmbRefDocNo', 'change', function () { syncRef('CmbRefDocNo', 'CmbRefDocInvoiceNo'); });
        on('CmbRefDocInvoiceNo', 'change', function () { syncRef('CmbRefDocInvoiceNo', 'CmbRefDocNo'); });
        on('combpttrm', 'change', combpttrm_TextChanged);
        on('txtduedays', 'input', txtduedays_TextChanged);
        on('DocDate', 'change', function () { DocDate_Leave(); txtduedays_TextChanged(); });
        on('deliverystartdate', 'change', deliverystartdate_ValueChanged);
        on('CmbBillType', 'change', CmbBillType_TextChanged);
        on('cmbCurrency', 'change', cmbCurrency_Leave);
        on('txtExchangeRate', 'input', txtExchangeRate_TextChanged);
        on('fileAttachment', 'change', onAttachmentsPicked);
        $id('grd').addEventListener('change', onGridInput);
        $id('grd').addEventListener('click', function (e) {
            var d = e.target.getAttribute && e.target.getAttribute('data-del'); if (d !== null && d !== undefined) deleteRow(int(d));
        });
        $id('grd').addEventListener('dblclick', function (e) {
            if (e.target.tagName === 'INPUT' || e.target.tagName === 'BUTTON') return;
            var tr = e.target.closest('tr.data-row'); if (tr) grd_DoubleClick(int(tr.getAttribute('data-i')));
        });
        $id('grdhistory').addEventListener('click', onHistoryClick);
        $id('grdhistory').addEventListener('dblclick', function (e) {           // grdhistory_DoubleClick_1 :2105
            var tr = e.target.closest('tr.data-row'); if (tr && e.target.tagName !== 'BUTTON') ReadById(int(tr.getAttribute('data-id')), false);
        });
        $id('lstAttachments').addEventListener('click', function (e) {
            var r = e.target.getAttribute('data-remove'), d = e.target.getAttribute('data-drop');
            if (r) { removeAttachmentIds.push(int(r)); renderAttachments(); }
            if (d !== null && d !== undefined) { newFiles.splice(int(d), 1); renderAttachments(); }
        });
        document.addEventListener('keydown', function (e) {                    // PurchsaeOrder_KeyDown :3151
            var k = (e.key || '').toLowerCase();
            if (!e.ctrlKey) {
                if (k === 'enter' && e.target && e.target.tagName === 'INPUT' && !(e.target.classList && e.target.classList.contains('dtcombo-input'))
                        && e.target.type !== 'file' && e.target.type !== 'checkbox' && e.target.type !== 'radio') { e.preventDefault(); nextField(e.target); }
                return;
            }
            var handled = true;
            if (k === 'e') window.location.href = '/dashboard';
            else if (k === 'n') { if (currentTab === 0) btnnew_Click(); else btnNewHistory_Click(); }
            else if (k === 'r') { if (currentTab === 0) toolStripButton1_Click(); else btnRefreshHistory_Click(); }
            else if (k === 's') { if (currentTab === 0) { if (mode === 'save') btnsave_Click(); } else gridhistoryfill(); }
            else if (k === 'u') { if (mode === 'update') btnupdate_Click(); }
            else if (k === 'p') { if (perms.Print) btnprint_Click(); }
            else if (k === 'f5') focus('DocDate');
            else if (k === 'f10') btnattachment_Click();
            else if (k === 'f12') BtnSaveAs_Click();
            else if (k === 't') showTab(currentTab === 1 ? 0 : 1);
            else if (k === 'arrowup') focus(currentTab === 0 ? 'combitem' : 'FromDateHistory');
            else handled = false;
            if (handled) e.preventDefault();
        });
    }
    function nextField(el) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, button, textarea'), function (x) {
            return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null && x.type !== 'hidden';
        });
        var i = all.indexOf(el); if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }

    window.PoPm = {
        showTab: showTab, btnnew_Click: btnnew_Click, toolStripButton1_Click: toolStripButton1_Click,
        btnsave_Click: btnsave_Click, BtnSaveAs_Click: BtnSaveAs_Click, btnupdate_Click: btnupdate_Click,
        btnprint_Click: btnprint_Click, btnattachment_Click: btnattachment_Click,
        btnplus_Click: btnplus_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, btnCancelUpdateDetial_Click: btnCancelUpdateDetial_Click,
        btnshow_Click: gridhistoryfill, btnNewHistory_Click: btnNewHistory_Click, btnRefreshHistory_Click: btnRefreshHistory_Click
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
