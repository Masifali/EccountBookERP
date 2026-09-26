/* ============================================================================================
 * Purchase Invoice Direct PM - screen 495, module 54, DocumentTypeId 245.
 * Desktop: Architecture.WinApp.PackingMaterial_Store.frmPurchaseInvoiceDirectPM.
 *
 *   Load :392   FormValidation :525   FormValidationDetail :584   DefaultConfigurations :665
 *   comItem_Leave :1049   WareHouseBind :1077   RackBind :1132   CmbRackName_Leave :1168   BalanceStock :1202
 *   GetTaxTypeIdAndTaxPercent :1246   CmbRefDocumentType_Leave :1280   cmbCurrency_Leave :1505
 *   grdFreight :1723-1850   grdGLedger :1857-2022   btnAdd :2024   grd_DoubleClick :2152   btnUpdateDetail :2202
 *   DeleteDetailrow :2325   Insert :2356   ReadById :2642   Reset :2756   ResetDetail :2823
 *   BillAmount :2899   AmountCaluculation :2943   FreightProportion :2991   BillProportion :3025
 *   CalculateTaxAmount :3072   GetAll :3240   DocDate_Leave :3770
 * The server repeats every figure and validation; nothing here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/purchase-invoice-direct';
    var L = null, cfg = {}, perms = {};
    var dtGrid = [], dtFreight = [], dtGrdGL = [];
    var RecId = 0, VoucherHeadId = 0, Approved = false, updateDetailIndex = -1;
    var taxRow = null, dtRefDoc = [];
    var newFiles = [], removeAttachmentIds = [], existingAttachments = [];
    var historyRows = [];
    var currentTab = 0;

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
    /* Math.Round(x, n, AwayFromZero) */
    function away(x, n) { var f = Math.pow(10, n || 0); var s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    /* Math.Round(x, n) - to even */
    function even(x, n) {
        var f = Math.pow(10, n || 0), y = x * f, r = Math.round(y);
        if (Math.abs(Math.abs(y % 1) - 0.5) < 1e-9) r = 2 * Math.round(y / 2);
        return r / f;
    }
    function dp() { return int(cfg.amountDecimals); }
    function fmt(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 4 : d }); }
    function fixed(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function amt(n) { return fixed(away(num(n), dp()), dp()); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
        var p; try { p = fn(); } catch (e) { done(); throw e; }
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
    function fill(id, rows, v, t, blank) {
        var sel = $id(id); if (!sel) return; var keep = sel.value;
        var html = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function has(id, v) { var s = $id(id); return !!(s && s.querySelector('option[value="' + String(v).replace(/"/g, '') + '"]')); }
    function text(id) { var s = $id(id); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function byId(list, id) { id = int(id); return (list || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0] || null; }
    /* dtAccountlst - the combo value is SuppliercustomerId (the account id itself when feature 4 is off). */
    function account(v) { v = int(v); return (L && L.accounts || []).filter(function (a) { return int(col(a, 'SupplierCustomerId')) === v; })[0] || null; }
    function accountOptions(current) {
        var html = '<option value="0"></option>';
        (L && L.accounts || []).forEach(function (a) {
            var v = int(col(a, 'SupplierCustomerId'));
            html += '<option value="' + v + '"' + (v === int(current) ? ' selected' : '') + '>' + esc(col(a, 'AccountTitle')) + '</option>';
        });
        return html;
    }
    function supplierGl() { var s = byId(L && L.suppliers, val('comsupplier')); return s ? int(s.GlAccountId) : 0; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today()); setVal('txtPackingDate', today()); setVal('txtExpiryDateDetail', today());
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today());
        loadLookups().then(function () { AddRowInGLGrid(); AddRowInFreightGrid(); DefaultConfigurations(); renderAll(); refreshCombos(); });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; cfg = d.configuration || {}; perms = d.permissions || {};
            fill('comsupplier', d.suppliers, 'Id', 'CompanyName');
            fill('CmbTaxAccount', d.taxAccounts, 'Id', 'AccountTitle');
            ItemNameBind();
            fill('CmbItemCondition', d.conditions, 'Id', 'Description');
            fill('CmbCropYear', d.cropYears, 'Id', 'Description');
            fill('comjobLot', d.jobLots, 'Id', 'JobLotDescription');
            fill('cmbCurrency', d.currencies, 'Id', 'CurrencyCode');
            fill('CmbRefDocumentType', d.refDocumentTypes, 'Id', 'type');
            Array.prototype.forEach.call(document.querySelectorAll('.fcy'), function (e) { e.style.display = cfg.multiCurrency ? '' : 'none'; });
            $id('txtTaxPercnt').disabled = !cfg.taxPercentEditable;
            if (RecId === 0) { setVal('txtdocno', d.docNo); setVal('txtBranchSrNo', d.branchSrNo); $id('txtDocNoShow').textContent = 'PI-' + d.docNo; }
            var bl = '';
            (d.branches || []).forEach(function (b) {
                bl += '<label class="win-check" style="display:block"><input type="checkbox" value="' + int(b.Id) + '"' + (int(b.Id) === int(d.userBranchId) ? ' checked' : '') + '/> ' + esc(b.Description) + '</label>';
            });
            $id('cmbBranchName').innerHTML = bl;
            fill('cmbSupplierNameHistory', d.historySuppliers, 'Id', 'Description');
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    /* DefaultConfigurations :665 */
    function DefaultConfigurations() {
        if (int(cfg.defaultJobLotId) && has('comjobLot', cfg.defaultJobLotId)) setVal('comjobLot', cfg.defaultJobLotId);
        if (int(cfg.baseCurrencyId)) setVal('cmbCurrency', cfg.baseCurrencyId);
        setVal('txtExchangeRate', fixed(cfg.baseCurrencyRate, int(cfg.rateDecimals)));
    }

    function ItemNameBind() {
        var byName = $id('rdbtnItemName').checked;
        fill('comItem', (L && L.items) || [], 'Id', byName ? 'ItemName' : 'ItemCode');
        refreshCombos();
    }

    function applyRights() {
        var upd = RecId > 0;
        show('btnSave', !upd); show('btnUpdate', upd); show('btnDelete', upd);
        $id('btnSave').disabled = !perms.Save; $id('btnUpdate').disabled = !perms.Update; $id('btnDelete').disabled = !perms.Delete;
        ['btnPrint', 'btnSlip', 'ChkBok', 'ChkPrintSlip'].forEach(function (b) { $id(b).disabled = !perms.Print; });
    }

    // ------------------------------------------------------------------ detail editors

    function itemRacks(itemId) { return (L.racks || []).filter(function (r) { return int(r.ItemId) === int(itemId); }); }

    /* comItem_Leave :1049 */
    function comItem_Leave() {
        var itemId = int(val('comItem'));
        var p = Promise.resolve();
        if (itemId > 0) { p = GetTaxTypeIdAndTaxPercent(); BalanceStock(); }
        else { taxRow = null; fill('CmbTaxName', [], 'Id', 'Name'); setVal('txtTaxPercnt', ''); setVal('txtTaxAmount', ''); }
        WareHouseBindFromGlobalRacksByItemId(itemId);
        RackBindFromGlobalRacksByItemId(itemId, int(val('comWarehouse')));
        PackUOM();
        return p;
    }

    function WareHouseBindFromGlobalRacksByItemId(itemId) {
        var seen = {}, list = [];
        itemRacks(itemId).forEach(function (r) { if (!seen[r.WarehouseId]) { seen[r.WarehouseId] = 1; list.push({ Id: r.WarehouseId, WareHouseName: r.WareHouseName, BranchId: r.BranchId, BranchName: r.BranchName }); } });
        fill('comWarehouse', list, 'Id', 'WareHouseName');
        if (int(val('CmbRackName')) !== 0) return;
        if (list.length === 1) setVal('comWarehouse', list[0].Id);
        else if (list.length > 1 && int(cfg.defaultWarehouseId) && list.some(function (w) { return int(w.Id) === int(cfg.defaultWarehouseId); })) setVal('comWarehouse', cfg.defaultWarehouseId);
    }

    function RackBindFromGlobalRacksByItemId(itemId, whId) {
        var seen = {}, list = [];
        itemRacks(itemId).forEach(function (r) { if ((!whId || int(r.WarehouseId) === whId) && !seen[r.Id]) { seen[r.Id] = 1; list.push(r); } });
        fill('CmbRackName', list, 'Id', 'RackName');
        if (int(val('CmbRackName')) === 0 && list.length === 1) { setVal('CmbRackName', list[0].Id); CmbRackName_Leave(); }
    }

    function CmbRackName_Leave() {
        var rack = byId(L.racks, val('CmbRackName'));
        if (rack) {
            if (!int(val('comWarehouse')) || int(rack.WarehouseId) !== int(val('comWarehouse'))) {
                if (!has('comWarehouse', rack.WarehouseId)) WareHouseBindFromGlobalRacksByItemId(int(val('comItem')));
                setVal('comWarehouse', rack.WarehouseId);
            }
            BalanceStock();
        }
    }

    /* PackUOM :974 - both combos keep their text; else the only row, else the Equivalent 1 row. */
    function PackUOM() {
        var packText = text('cmbPackUom'), rateText = text('comRateUOM');
        var list = (L.uoms || []).filter(function (u) { return int(u.ItemId) === int(val('comItem')); });
        fill('cmbPackUom', list, 'Id', 'UOMCode'); fill('comRateUOM', list, 'Id', 'UOMCode');
        [['cmbPackUom', packText], ['comRateUOM', rateText]].forEach(function (p) {
            var m = list.filter(function (u) { return String(u.UOMCode) === p[1]; })[0];
            if (m && p[1]) { setVal(p[0], m.Id); return; }
            setVal(p[0], '');
            if (list.length === 1) setVal(p[0], list[0].Id);
            else if (list.length > 1) { var e = list.filter(function (u) { return num(u.Equivalent) === 1; })[0]; if (e) setVal(p[0], e.Id); }
        });
    }

    function BalanceStock() {
        var itemId = int(val('comItem')); if (!itemId) return;
        var q = '?itemId=' + itemId + '&docDate=' + encodeURIComponent(val('DocDate')) + '&conditionId=' + int(val('CmbItemCondition'))
            + '&recId=' + RecId + '&warehouseId=' + int(val('comWarehouse')) + '&rackId=' + int(val('CmbRackName'));
        return getJson(api + '/stock' + q).then(function (d) { $id('lblStockQty').textContent = fmt(d.qtyInHand, 2); }).catch(function () { $id('lblStockQty').textContent = ''; });
    }

    /* GetTaxTypeIdAndTaxPercent :1246 - one row: the item's schedule on the doc date. */
    function GetTaxTypeIdAndTaxPercent(keepId) {
        return getJson(api + '/tax-options?itemId=' + int(val('comItem')) + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (rows) {
            taxRow = rows && rows.length ? rows[0] : null;
            if (taxRow) {
                fill('CmbTaxName', [{ Id: col(taxRow, 'TaxNameId'), Name: col(taxRow, 'TaxName') }], 'Id', 'Name');
                setVal('CmbTaxName', keepId === undefined ? col(taxRow, 'TaxNameId') : (int(keepId) > 0 && has('CmbTaxName', keepId) ? keepId : ''));
            } else { fill('CmbTaxName', [], 'Id', 'Name'); setVal('txtTaxPercnt', ''); setVal('txtTaxAmount', ''); }
            CalculateTaxAmount();
        }).catch(function (e) { box(e.message); });
    }

    /* AmountCaluculation :2943 */
    function AmountCaluculation() {
        var u = byId(L.uoms, val('comRateUOM')), eq = u ? num(u.Equivalent) : 0, rate = num(val('txtRate')), qty = num(val('txtQty'));
        setVal('txtAmount', qty > 0 && eq > 0 && rate > 0 ? String(even(qty / eq * rate, 2)) : '0');
        CalculateTaxAmount();
    }

    /* CalculateTaxAmount :3072 */
    function CalculateTaxAmount() {
        var amount = num(val('txtAmount'));
        if (int(val('CmbTaxName')) > 0 && taxRow) {
            var pct;
            if (!(num(val('txtTaxPercnt')) > 0)) { pct = num(col(taxRow, 'TaxPercent')); setVal('txtTaxPercnt', fmt(pct, 3)); }
            else pct = num(val('txtTaxPercnt'));
            var t = amount * pct / 100;
            setVal('txtTaxAmount', amt(t)); setVal('txtTotalAmount', amt(amount + t));
        } else {
            setVal('txtTaxPercnt', int(val('CmbTaxName')) > 0 ? val('txtTaxPercnt') : '');
            setVal('txtTaxAmount', ''); setVal('txtTotalAmount', amt(amount));
        }
    }

    /* CmbRefDocumentType_Leave :1280 */
    function CmbRefDocumentType_Leave() {
        var t = int(val('CmbRefDocumentType'));
        dtRefDoc = [];
        if (t <= 0) { fill('CmbRefDocNo', []); fill('CmbRefDocInvoiceNo', []); return Promise.resolve(); }
        return getJson(api + '/ref-docs?refDocumentTypeId=' + t + '&recId=' + RecId).then(function (rows) {
            dtRefDoc = rows || [];
            var inv = [], doc = [], si = {}, sd = {};
            dtRefDoc.forEach(function (r) {
                var i = int(col(r, 'RefDocInvoiceId')), d = int(col(r, 'RefDocId'));
                if (i > 0 && !si[i]) { si[i] = 1; inv.push({ Id: i, No: col(r, 'RefDocInvoiceNo'), RefDocId: d }); }
                if (d > 0 && !sd[d]) { sd[d] = 1; doc.push({ Id: d, No: col(r, 'RefDocNo'), InvoiceId: i }); }
            });
            fill('CmbRefDocInvoiceNo', inv, 'Id', 'No'); fill('CmbRefDocNo', doc, 'Id', 'No');
        }).catch(function (e) { box(e.message); });
    }
    function CmbRefDocNo_Leave() {
        var d = int(val('CmbRefDocNo')); if (d <= 0) return;
        var r = dtRefDoc.filter(function (x) { return int(col(x, 'RefDocId')) === d; })[0], i = r ? int(col(r, 'RefDocInvoiceId')) : 0;
        setVal('CmbRefDocInvoiceNo', has('CmbRefDocInvoiceNo', i) ? i : '');
    }
    function CmbRefDocInvoiceNo_Leave() {
        var i = int(val('CmbRefDocInvoiceNo')); if (i <= 0) return;
        var r = dtRefDoc.filter(function (x) { return int(col(x, 'RefDocInvoiceId')) === i; })[0], d = r ? int(col(r, 'RefDocId')) : 0;
        setVal('CmbRefDocNo', has('CmbRefDocNo', d) ? d : '');
    }

    /* FormValidationDetail :584 */
    function FormValidationDetail() {
        var checks = [['comItem', 'Item Name Field is Required'], ['comWarehouse', 'Warehouse Field is Required'], ['CmbRackName', 'Rack Name Field is Required'],
            ['CmbItemCondition', 'Item Condition Field is Required'], ['comjobLot', 'Job/Lot Field is Required'], ['cmbPackUom', 'UOM Field is Required']];
        for (var i = 0; i < checks.length; i++) if (!int(val(checks[i][0]))) { box(checks[i][1]); focus(checks[i][0]); return false; }
        if (num(val('txtQty')) === 0) { box('Qty Field is Required'); focus('txtQty'); return false; }
        if (num(val('txtRate')) === 0) { box('Rate Field is Required'); focus('txtRate'); return false; }
        if (!int(val('comRateUOM'))) { box('Rate UOM Field is Required'); focus('comRateUOM'); return false; }
        if (num(val('txtAmount')) === 0) { box('ItemAmount Field is Required'); focus('txtAmount'); return false; }
        if (num(val('txtTaxAmount')) > 0) {
            if (!num(val('txtTaxPercnt'))) { box('Tax Percent is Required'); focus('txtTaxPercnt'); return false; }
            if (!int(val('CmbTaxName'))) { box('Tax Name is Required'); focus('CmbTaxName'); return false; }
        }
        return true;
    }

    function detailRow(existing) {
        var wh = byId(itemRacks(val('comItem')).map(function (r) { return { Id: r.WarehouseId, BranchId: r.BranchId, BranchName: r.BranchName }; }), val('comWarehouse'));
        var lot = byId(L.jobLots, val('comjobLot'));
        var branchId = wh ? int(wh.BranchId) : 0, branchName = wh ? wh.BranchName : '';
        if (cfg.branchFeature && branchId !== (lot ? int(lot.BranchId) : 0)) {
            box("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + (branchName || '') + "' and JobLot is of Branch '" + (lot ? lot.BranchName : '') + "'");
            return null;
        }
        var taxNameId = int(val('CmbTaxName')), amount = num(val('txtAmount')), pct = 0, tax = 0, taxName = '';
        if (taxNameId > 0) { taxName = text('CmbTaxName'); pct = num(val('txtTaxPercnt')); tax = amount * pct / 100; }
        var refType = int(val('CmbRefDocumentType')), item = byId(L.items, val('comItem'));
        var r = existing || { Id: 0, Freights: 0 };
        r.ItemId = int(val('comItem')); r.ItemCode = item ? item.ItemCode : ''; r.Item = item ? item.ItemName : '';
        r.WarehouseId = int(val('comWarehouse')); r.Warehouse = text('comWarehouse');
        r.RackId = int(val('CmbRackName')); r.RackName = text('CmbRackName');
        r.ItemConditionId = int(val('CmbItemCondition')); r.ItemCondition = text('CmbItemCondition');
        r.CropYearId = int(val('CmbCropYear')); r.CropYear = int(val('CmbCropYear')) ? text('CmbCropYear') : '';
        r.JobLotId = int(val('comjobLot')); r.JobLot = text('comjobLot');
        r.PackingDate = val('txtPackingDate'); r.ExpiryDate = val('txtExpiryDateDetail');
        r.UOMId = int(val('cmbPackUom')); r.UOM = text('cmbPackUom');
        r.ItemQty = num(val('txtQty')); r.Rate = num(val('txtRate')); r.RateUOMId = int(val('comRateUOM')); r.RateUOM = text('comRateUOM');
        r.ItemAmount = amount; r.TaxNameId = taxNameId; r.TaxName = taxName; r.TaxPercent = pct; r.TaxAmount = tax;
        r.RemarksDetail = val('txtRemarksdetail').trim(); r.GpNo = int(val('txtGpNo')); r.VehicleNo = val('txtVehicleNo').trim();
        r.BranchId = branchId; r.BranchName = branchName;
        r.RefDocumentTypeId = refType; r.RefDocumentType = refType ? text('CmbRefDocumentType') : '';
        r.RefDocId = refType ? int(val('CmbRefDocNo')) : 0; r.RefDocNo = refType ? text('CmbRefDocNo') : '';
        r.RefDocInvoiceId = refType ? int(val('CmbRefDocInvoiceNo')) : 0; r.RefDocInvoiceNo = refType ? text('CmbRefDocInvoiceNo') : '';
        return r;
    }

    function btnAdd_Click() {
        if (!FormValidationDetail()) return;
        var r = detailRow(null); if (!r) return;
        dtGrid.push(r);
        FreightProportion(); BillAmount(); txtExchangeRate_TextChanged(); ResetDetail(); renderAll();
    }
    function btnUpdateDetail_Click() {
        if (updateDetailIndex < 0 || !dtGrid[updateDetailIndex]) return;
        if (!FormValidationDetail()) return;
        var r = detailRow(dtGrid[updateDetailIndex]); if (!r) return;
        FreightProportion(); BillAmount(); txtExchangeRate_TextChanged(); ResetDetail(); renderAll();
    }

    /* grd_DoubleClick :2152 */
    function grd_DoubleClick(i) {
        var r = dtGrid[i]; if (!r) return;
        updateDetailIndex = i;
        setVal('comItem', r.ItemId); refreshCombos();
        taxRow = null;
        setVal('CmbRackName', ''); setVal('comWarehouse', '');
        comItem_Leave();
        setVal('comWarehouse', r.WarehouseId); RackBindFromGlobalRacksByItemId(r.ItemId, r.WarehouseId); setVal('CmbRackName', r.RackId);
        setVal('CmbItemCondition', r.ItemConditionId); setVal('CmbCropYear', r.CropYearId || ''); setVal('comjobLot', r.JobLotId);
        setVal('txtPackingDate', dateOnly(r.PackingDate)); setVal('txtExpiryDateDetail', dateOnly(r.ExpiryDate));
        setVal('cmbPackUom', r.UOMId); setVal('txtQty', r.ItemQty); setVal('txtRate', r.Rate); setVal('comRateUOM', r.RateUOMId);
        setVal('txtAmount', r.ItemAmount);
        GetTaxTypeIdAndTaxPercent(r.TaxNameId).then(function () {
            setVal('txtTaxPercnt', fmt(r.TaxPercent, 2)); setVal('txtTaxAmount', amt(r.TaxAmount)); setVal('txtTotalAmount', amt(num(r.ItemAmount) + num(r.TaxAmount)));
        });
        setVal('CmbRefDocumentType', r.RefDocumentTypeId || '');
        CmbRefDocumentType_Leave().then(function () { setVal('CmbRefDocNo', r.RefDocId || ''); setVal('CmbRefDocInvoiceNo', r.RefDocInvoiceId || ''); });
        setVal('txtRemarksdetail', r.RemarksDetail); setVal('txtGpNo', r.GpNo || ''); setVal('txtVehicleNo', r.VehicleNo);
        show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        refreshCombos(); renderAll(); focus('comWarehouse');
    }

    /* ResetDetail :2823 - condition, crop year, job lot, pack uom, dates, remarks, GP and vehicle stay. */
    function ResetDetail() {
        setVal('comItem', ''); fill('comWarehouse', []); fill('CmbRackName', []);
        ['txtQty', 'txtRate', 'txtAmount', 'txtTaxPercnt', 'txtTaxAmount', 'txtTotalAmount'].forEach(function (k) { setVal(k, ''); });
        setVal('comRateUOM', ''); taxRow = null; fill('CmbTaxName', [], 'Id', 'Name');
        setVal('CmbRefDocumentType', ''); fill('CmbRefDocNo', []); fill('CmbRefDocInvoiceNo', []); dtRefDoc = [];
        updateDetailIndex = -1;
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        refreshCombos(); renderAll(); focus('comItem');
    }

    /* DeleteDetailrow :2325 */
    function DeleteDetailrow(i) {
        var r = dtGrid[i]; if (!r) return;
        if (updateDetailIndex !== -1) { box('Please Reset the Detail first..'); return; }
        var p = Promise.resolve();
        if (int(r.Id) > 0) {
            if (!window.confirm('Are you sure you want to delete this record?')) return;
            p = http('POST', api + '/' + RecId + '/detail/' + int(r.Id) + '/check-delete');
        }
        return p.then(function () { dtGrid.splice(i, 1); FreightProportion(); BillAmount(); txtExchangeRate_TextChanged(); renderAll(); })
            .catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ totals

    function totals() {
        var t = { qty: 0, amount: 0, tax: 0, fcy: 0, freights: 0, bill: 0 };
        dtGrid.forEach(function (r) { t.qty += num(r.ItemQty); t.amount += num(r.ItemAmount); t.tax += num(r.TaxAmount); t.fcy += num(r.FcyAmount); t.freights += num(r.Freights); t.bill += num(r.BillAmount); });
        return t;
    }
    /* BillAmount :2899 - TransporterCredit is never assigned in the desktop, so it adds 0. */
    function BillAmount() {
        var t = totals(), jd = 0, jc = 0;
        dtGrdGL.forEach(function (g) { if (int(g.AccountId) > 0) { jd += num(g.Debit); jc += num(g.Credit); } });
        setVal('txtBillAmount', String(even(t.amount + t.tax + jd - jc)));
        BillProportion();
    }
    /* FreightProportion :2991 */
    function FreightProportion() {
        var t = totals(), credit = 0;
        dtFreight.forEach(function (f) { credit += num(f.Freight); });
        dtGrid.forEach(function (r) { r.Freights = credit > 0 ? even(credit) / t.qty * num(r.ItemQty) : 0; });
        BillProportion();
    }
    function BillProportion() { dtGrid.forEach(function (r) { r.BillAmount = num(r.ItemAmount) + num(r.Freights) + num(r.TaxAmount); }); }
    /* txtExchangeRate_TextChanged :1560 / CalculateTotalInformation :1593 */
    function txtExchangeRate_TextChanged() {
        var rate = num(val('txtExchangeRate'));
        dtGrid.forEach(function (r) { r.FcyAmount = dtGrid.length && rate > 0 ? num(r.ItemAmount) / rate : 0; });
        var t = totals();
        setVal('txtOrderQty', dtGrid.length ? fmt(even(t.qty, 2), 2) : '0');
        setVal('txtFcyAmount', dtGrid.length ? fixed(away(t.fcy, int(cfg.fcyDecimals)), int(cfg.fcyDecimals)) : '0');
    }

    /* DocDate_Leave :3770 */
    function DocDate_Leave() {
        if (!dtGrid.length) return;
        var ids = dtGrid.map(function (r) { return r.ItemId; }).join(',');
        return getJson(api + '/tax-by-items?itemIds=' + encodeURIComponent(',' + ids) + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (rows) {
            dtGrid.forEach(function (r) {
                var m = (rows || []).filter(function (x) { return int(col(x, 'ItemId')) === int(r.ItemId); });
                if (m.length) {
                    var s = m[m.length - 1];
                    r.TaxNameId = int(col(s, 'TaxNameId')); r.TaxName = col(s, 'TaxName'); r.TaxPercent = num(col(s, 'TaxPercent'));
                    if (num(r.ItemAmount) > 0 && r.TaxPercent > 0) r.TaxAmount = num(r.ItemAmount) * r.TaxPercent / 100;
                    r.BillAmount = num(r.ItemAmount) + num(r.TaxAmount) + num(r.Freights);
                } else { r.TaxNameId = 0; r.TaxName = ''; r.TaxPercent = 0; r.TaxAmount = 0; r.BillAmount = num(r.ItemAmount) + num(r.Freights); }
            });
            BillAmount(); renderAll();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ grids

    function AddRowInFreightGrid() { dtFreight.push({ Transporter: 0, Freight: 0, Remarks: '' }); }
    function AddRowInGLGrid() { dtGrdGL.push({ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }); }

    var G = [['ItemCode', 'Item Code'], ['Item', 'Item'], ['Warehouse', 'Warehouse'], ['RackName', 'Rack'], ['ItemCondition', 'Condition'], ['CropYear', 'Crop Year'],
        ['JobLot', 'Job Lot'], ['PackingDate', 'Packing Date', 'd'], ['ExpiryDate', 'Expiry Date', 'd'], ['UOM', 'UOM'], ['ItemQty', 'Qty', 'q'], ['Rate', 'Rate', 'q'],
        ['RateUOM', 'Rate UOM'], ['ItemAmount', 'Item Amount', 'a'], ['FcyAmount', 'Fcy Amount', 'q'], ['TaxName', 'Tax Name'], ['TaxPercent', 'Tax %', 'q'],
        ['TaxAmount', 'Tax Amount', 'a'], ['Freights', 'Freights', 'a'], ['BillAmount', 'Bill Amount', 'a'], ['RemarksDetail', 'Remarks'], ['GpNo', 'GP No'],
        ['VehicleNo', 'Vehicle No'], ['BranchName', 'Branch'], ['RefDocumentType', 'Ref Doc Type'], ['RefDocNo', 'Ref Doc No'], ['RefDocInvoiceNo', 'Ref Invoice No']];

    function renderAll() {
        var h = '<th>X</th>'; G.forEach(function (c) { h += '<th>' + c[1] + '</th>'; }); $id('grdHead').innerHTML = h;
        var html = '';
        dtGrid.forEach(function (r, i) {
            html += '<tr class="data-row' + (i === updateDetailIndex ? ' editing' : '') + '" data-i="' + i + '"><td><button type="button" class="win-btn-mini" data-del="' + i + '">X</button></td>';
            G.forEach(function (c) {
                var v = r[c[0]];
                html += '<td' + (c[2] && c[2] !== 'd' ? ' class="num"' : '') + '>' + (c[2] === 'a' ? amt(v) : c[2] === 'q' ? fmt(v) : c[2] === 'd' ? ddmmm(v) : esc(v)) + '</td>';
            });
            html += '</tr>';
        });
        $id('grd').innerHTML = html;
        var t = totals(), f = '<td></td>';
        G.forEach(function (c) { f += '<td class="num">' + (c[0] === 'ItemQty' ? fmt(t.qty) : c[0] === 'ItemAmount' ? amt(t.amount) : c[0] === 'TaxAmount' ? amt(t.tax)
            : c[0] === 'Freights' ? amt(t.freights) : c[0] === 'BillAmount' ? amt(t.bill) : c[0] === 'FcyAmount' ? fmt(t.fcy, 3) : '') + '</td>'; });
        $id('grdFoot').innerHTML = dtGrid.length ? f : '';

        $id('frHead').innerHTML = '<th>X</th><th>+</th><th>Transporter</th><th>Credit</th><th>Remarks</th>';
        var fr = '', fcr = 0;
        dtFreight.forEach(function (r, i) {
            fcr += num(r.Freight);
            fr += '<tr data-fr="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td><select class="cell-sel" data-f="Transporter">' + accountOptions(r.Transporter) + '</select></td>'
                + '<td><input class="cell" data-f="Freight" value="' + esc(r.Freight) + '"/></td>'
                + '<td><input class="cell-text" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td></tr>';
        });
        $id('grdFreight').innerHTML = fr;
        $id('frFoot').innerHTML = '<td></td><td></td><td></td><td class="num">' + amt(fcr) + '</td><td></td>';

        $id('glHead').innerHTML = '<th>X</th><th>+</th><th>Account</th><th>Remarks</th><th>%</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th>';
        var gl = '', gd = 0, gc = 0;
        dtGrdGL.forEach(function (r, i) {
            gd += num(r.Debit); gc += num(r.Credit);
            gl += '<tr data-gl="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td><select class="cell-sel" data-f="AccountId">' + accountOptions(r.AccountId) + '</select></td>'
                + '<td><input class="cell-text" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td>'
                + ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].map(function (k) { return '<td><input class="cell-sm" data-f="' + k + '" value="' + esc(r[k]) + '"/></td>'; }).join('') + '</tr>';
        });
        $id('grdGLedger').innerHTML = gl;
        $id('glFoot').innerHTML = '<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td class="num">' + amt(gd) + '</td><td class="num">' + amt(gc) + '</td>';
    }

    // ------------------------------------------------------------------ save

    /* FormValidation :525 */
    function FormValidation() {
        if (!val('txtdocno').trim() || val('txtdocno').trim() === '0') { box('DocNo Field is Required'); return false; }
        if (!int(val('comsupplier'))) { box('Supplier Field is Required'); focus('comsupplier'); return false; }
        if (cfg.multiCurrency) {
            if (!int(val('cmbCurrency'))) { box('Fcy Code Field is Required'); focus('cmbCurrency'); return false; }
            if (!num(val('txtExchangeRate'))) { box('Exchange Rate Field is Required'); focus('txtExchangeRate'); return false; }
            if (!num(val('txtFcyAmount'))) { box('Fcy Amount Rate Field is Required'); return false; }
        } else {
            if (!int(val('cmbCurrency'))) { box('Please Configure Your Base Currency In configurations'); return false; }
            if (!num(val('txtExchangeRate'))) { box('Please Configure Your Base Currency Rate In configurations'); return false; }
        }
        return true;
    }

    function Insert(btnId) {
        if (!dtGrid.length) { box('Grid Record Not Found'); return; }
        if (!FormValidation()) return;
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < dtFreight.length; i++) if (num(dtFreight[i].Freight) > 0 && !int(dtFreight[i].Transporter)) { box('Please Select an Account Against Freight First'); return; }
        for (var j = 0; j < dtGrdGL.length; j++) if ((num(dtGrdGL[j].Credit) > 0 || Math.round(num(dtGrdGL[j].Debit)) > 0) && !int(dtGrdGL[j].AccountId)) { box('Please Select an Account Against JL First'); return; }
        if (totals().tax > 0 && !int(val('CmbTaxAccount'))) { box('TaxAccount Field is Required'); focus('CmbTaxAccount'); return; }
        var req = {
            Id: RecId, DocDate: val('DocDate'), SupplierId: int(val('comsupplier')), ManualBillNo: val('txtbillno'), RemarksHeader: val('txtremarks'),
            TaxAccountId: int(val('CmbTaxAccount')), CurrencyId: int(val('cmbCurrency')), ExchangeRate: num(val('txtExchangeRate')),
            lines: dtGrid.map(function (r) {
                return { Id: int(r.Id), ItemId: r.ItemId, WarehouseId: r.WarehouseId, RackId: r.RackId, ItemConditionId: r.ItemConditionId, CropYearId: r.CropYearId,
                    JobLotId: r.JobLotId, PackingDate: dateOnly(r.PackingDate), ExpiryDate: dateOnly(r.ExpiryDate), UOMId: r.UOMId, ItemQty: num(r.ItemQty), Rate: num(r.Rate),
                    RateUOMId: r.RateUOMId, TaxNameId: int(r.TaxNameId), TaxPercent: num(r.TaxPercent), RemarksDetail: r.RemarksDetail || '', GpNo: String(r.GpNo || ''),
                    VehicleNo: r.VehicleNo || '', RefDocumentTypeId: int(r.RefDocumentTypeId), RefDocId: int(r.RefDocId), RefDocInvoiceId: int(r.RefDocInvoiceId) };
            }),
            freight: dtFreight.map(function (f) { return { Transporter: int(f.Transporter), Freight: num(f.Freight), Remarks: f.Remarks || '' }; }),
            journal: dtGrdGL.map(function (g) { return { AccountId: int(g.AccountId), Remarks: g.Remarks || '', Percentage: num(g.Percentage), Qty: num(g.Qty), Rate: num(g.Rate), Debit: num(g.Debit), Credit: num(g.Credit) }; }),
            files: newFiles, removeAttachmentIds: removeAttachmentIds
        };
        return busy(btnId, function () {
            say('Saving...');
            return http('POST', api + '/save', req).then(function (d) {
                say(''); box(d.message);
                var id = d.id, vh = d.voucherHeadId;
                return Reset().then(function () {
                    if ($id('ChkBok').checked) VoucherReport_118(vh);
                    if ($id('ChkPrintSlip').checked) GeneratePrint(id);
                });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }

    function btnDelete_Click() {
        if (Approved) { box('Record Not Delete because Record has approved'); return; }
        if (RecId <= 0) { box('Record Not Found'); return; }
        if (!perms.Delete || !window.confirm('Are you sure to Delete?')) return;
        return busy('btnDelete', function () {
            return http('POST', api + '/' + RecId + '/delete').then(function (d) { box(d.message); return Reset(); }).catch(function (e) { box(e.message); });
        });
    }

    function VoucherReport_118(vh) { if (!int(vh)) { box('VoucherId Not Found'); return; } window.open('/api/reports/acc-118/data?id=' + encodeURIComponent(vh) + '&documentTypeId=245', '_blank'); }
    function GeneratePrint(id) { if (!int(id)) { box('No Record Found For Display'); return; } window.open('/api/reports/pi-dpm-245/data?id=' + encodeURIComponent(id), '_blank'); }
    /* grdHistory "Voucher" :3467 passes the invoice Id (not the voucher head id) to AcRptPurchaseSalesVoucherSlip_103 - ported as written. */
    function VoucherSlip103(id) { if (!int(id)) { box('No Record Found For Display'); return; } window.open('/api/reports/acc-103/data?id=' + encodeURIComponent(id) + '&documentTypeId=245', '_blank'); }

    // ------------------------------------------------------------------ reset / read

    function Reset() {
        RecId = 0; VoucherHeadId = 0; Approved = false;
        newFiles = []; removeAttachmentIds = []; existingAttachments = []; renderAttachments();
        ['comsupplier', 'txtbillno', 'txtremarks', 'CmbTaxAccount', 'txtOrderQty', 'txtBillAmount', 'txtGpNo', 'txtVehicleNo', 'cmbPackUom', 'CmbCropYear', 'comjobLot'].forEach(function (k) { setVal(k, ''); });
        dtGrid = []; dtFreight = []; dtGrdGL = []; AddRowInFreightGrid(); AddRowInGLGrid();
        ResetDetail();
        renderAll(); applyRights();
        return getJson(api + '/numbers').then(function (d) {
            setVal('txtdocno', d.docNo); setVal('txtBranchSrNo', d.branchSrNo); $id('txtDocNoShow').textContent = 'PI-' + d.docNo;
            refreshCombos(); focus('DocDate');
        }).catch(function (e) { box(e.message); });
    }
    function btnNew_Click() { return busy('btnNew', Reset); }
    function btnRefresh_Click() { return busy('btnRefresh', function () { return loadLookups().then(renderAll); }); }

    /* ReadById :2642 */
    function ReadById(id) {
        say('Loading...');
        return getJson(api + '/' + id).then(function (d) {
            say('');
            var h = d.header;
            RecId = int(col(h, 'Id')); VoucherHeadId = int(d.voucherHeadId); Approved = !!d.approved;
            showTab(0);
            setVal('txtdocno', col(h, 'DocNo')); $id('txtDocNoShow').textContent = 'PI-' + col(h, 'DocNo');
            setVal('txtBranchSrNo', col(h, 'BranchSrNo'));
            setVal('DocDate', dateOnly(col(h, 'DocDate')));
            setVal('comsupplier', int(col(h, 'SupplierCustomerId')));
            setVal('CmbTaxAccount', int(col(h, 'TaxAccountId')) || '');
            setVal('txtbillno', col(h, 'ManualBillNo'));
            setVal('txtremarks', col(h, 'RemarksHeader'));
            setVal('cmbCurrency', int(col(h, 'CurrencyId')) || '');
            setVal('txtExchangeRate', col(h, 'ExchangeRate'));
            dtGrid = (d.lines || []).map(function (l) {
                return { Id: l.Id, ItemId: l.ItemId, ItemCode: l.ItemCode, Item: l.ItemName, WarehouseId: l.WarehouseId, Warehouse: l.WareHouseName, RackId: l.RackId,
                    RackName: l.RackName, ItemConditionId: l.ItemConditionId, ItemCondition: l.ItemCondition, CropYearId: l.CropYearId, CropYear: l.CropYear,
                    JobLotId: l.JobLotId, JobLot: l.JobLot, PackingDate: dateOnly(l.PackingDate), ExpiryDate: dateOnly(l.ExpiryDate), UOMId: l.UOMId, UOM: l.UOM,
                    ItemQty: l.ItemQty, Rate: l.Rate, RateUOMId: l.RateUOMId, RateUOM: l.RateUOM, ItemAmount: l.ItemAmount, FcyAmount: l.FcyAmount,
                    TaxNameId: l.TaxNameId, TaxName: l.TaxName, TaxPercent: l.TaxPercent, TaxAmount: l.TaxAmount, BillAmount: l.BillAmount, Freights: l.Freights,
                    RemarksDetail: l.RemarksDetail, GpNo: l.GpNo, VehicleNo: l.VehicleNo, BranchId: l.BranchId, BranchName: l.BranchName,
                    RefDocumentTypeId: l.RefDocumentTypeId, RefDocumentType: l.RefDocumentType, RefDocId: l.RefDocId, RefDocNo: l.RefDocNo,
                    RefDocInvoiceId: l.RefDocInvoiceId, RefDocInvoiceNo: l.RefDocInvoiceNo };
            });
            dtGrdGL = (d.journal || []).map(function (j) {
                return { AccountId: int(col(j, 'SupplierCustomerId')), Remarks: col(j, 'JvRemarks'), Percentage: num(col(j, 'JvPrcnt')), Qty: num(col(j, 'JvQty')),
                    Rate: num(col(j, 'JvRate')), Debit: num(col(j, 'JvDebit')), Credit: num(col(j, 'JvCredit')) };
            });
            dtFreight = (d.freight || []).map(function (f) { return { Transporter: int(col(f, 'SupplierCustomerId')), Freight: num(col(f, 'FreightAmount')), Remarks: col(f, 'Remarks') }; });
            if (!dtGrdGL.length) AddRowInGLGrid();
            if (!dtFreight.length) AddRowInFreightGrid();
            existingAttachments = d.attachments || []; newFiles = []; removeAttachmentIds = []; renderAttachments();
            txtExchangeRate_TextChanged(); BillAmount();
            refreshCombos(); renderAll(); applyRights();
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ history

    function branchIds() {
        return Array.prototype.filter.call(document.querySelectorAll('#cmbBranchName input'), function (x) { return x.checked; }).map(function (x) { return x.value; }).join(',');
    }
    var H = [['DocDate', 'DocDate', 'date'], ['DocNo', 'DocNo'], ['BranchSrNo', 'BranchSrNo'], ['BranchName', 'BranchName'], ['DueDays', 'DueDays'], ['DueDate', 'DueDate', 'date'],
        ['ManualBillNo', 'ManualBillNo'], ['SupplierName', 'SupplierName'], ['BillAmount', 'BillAmount', 'n'], ['ApprovedStatus', 'ApprovedStatus'],
        ['EntryUser', 'EntryUser'], ['EntryDate', 'EntryDate', 'date'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 'date'],
        ['NoOfAttachments', 'NoOfAttachments'], ['RemarksHeader', 'Remarks']];
    function GetAll() {
        var b = branchIds();
        if (!b) { box('Select branch first'); return; }
        var dt = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var q = '?branchIds=' + encodeURIComponent(b) + '&dateType=' + dt
            + ($id('chkFromDate').checked ? '&fromDate=' + encodeURIComponent(val('FromDateHistory')) : '')
            + ($id('chkToDate').checked ? '&toDate=' + encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory')) + '&supplierId=' + int(val('cmbSupplierNameHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            var head = '<th>Edit</th><th>Voucher</th><th>Slip</th>'; H.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
            $id('histHead').innerHTML = historyRows.length ? head : '';
            var html = '';
            historyRows.forEach(function (r, i) {
                html += '<tr class="data-row" data-h="' + i + '">' + ['edit', 'voucher', 'slip'].map(function (a) {
                    return '<td><button type="button" class="win-btn-mini" data-h-act="' + a + '">' + a.charAt(0).toUpperCase() + a.slice(1) + '</button></td>';
                }).join('');
                H.forEach(function (c) { var v = col(r, c[0]); html += '<td' + (c[2] === 'n' ? ' class="num"' : '') + '>' + (c[2] === 'date' ? ddmmm(v) : c[2] === 'n' ? amt(v) : esc(v)) + '</td>'; });
                html += '</tr>';
            });
            $id('grdHistory').innerHTML = html; $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            $id('lblHistCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
        }).catch(function (e) { box(e.message); });
    }
    function editFromHistory(id) { if (!perms.Update) { box("You don't Have Update Rights..."); return; } ReadById(id); }
    function onHistoryClick(e) {
        var tr = e.target.closest('tr[data-h]'); if (!tr) return;
        var r = historyRows[int(tr.getAttribute('data-h'))]; if (!r) return;
        Array.prototype.forEach.call(document.querySelectorAll('#grdHistory tr'), function (x) { x.classList.toggle('sel', x === tr); });
        var id = int(col(r, 'Id')), b = e.target.closest('button[data-h-act]');
        if (!b) { detail(id); return; }
        var a = b.getAttribute('data-h-act');
        if (a === 'edit') editFromHistory(id);
        else if (a === 'voucher') { if (perms.Print) VoucherSlip103(id); }
        else if (a === 'slip') { if (perms.Print) GeneratePrint(id); }
    }
    function detail(id) {
        return getJson(api + '/' + id).then(function (d) {
            var cols = [['ItemCode', 'ItemCode'], ['ItemName', 'Item'], ['WareHouseName', 'Warehouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'],
                ['CropYear', 'CropYear'], ['JobLot', 'JobLot'], ['UOM', 'UOM'], ['ItemQty', 'ItemQty', 'n'], ['Rate', 'Rate', 'n'], ['RateUOM', 'RateUOM'],
                ['ItemAmount', 'ItemAmount', 'n'], ['FcyAmount', 'FcyAmount', 'n'], ['TaxName', 'TaxName'], ['TaxPercent', 'TaxPercent', 'n'], ['TaxAmount', 'TaxAmount', 'n'],
                ['BillAmount', 'BillAmount', 'n'], ['Freights', 'Freights', 'n'], ['RemarksDetail', 'RemarksDetail'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'],
                ['BranchName', 'BranchName'], ['RefDocumentType', 'RefDocumentType'], ['RefDocNo', 'RefDocNo'], ['RefDocInvoiceNo', 'RefDocInvoiceNo']];
            var head = ''; cols.forEach(function (c) { head += '<th>' + c[1] + '</th>'; }); $id('histDetailHead').innerHTML = head;
            var html = ''; (d.lines || []).forEach(function (l) { html += '<tr>'; cols.forEach(function (c) { html += '<td' + (c[2] ? ' class="num"' : '') + '>' + (c[2] ? fmt(l[c[0]]) : esc(l[c[0]])) + '</td>'; }); html += '</tr>'; });
            $id('grdDetail').innerHTML = html;
        }).catch(function (e) { box(e.message); });
    }
    function btnNewHistory_Click() {
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today()); setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', ''); setVal('cmbSupplierNameHistory', '');
        historyRows = []; $id('grdHistory').innerHTML = ''; $id('histHead').innerHTML = ''; $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
        refreshCombos();
    }
    function btnRefreshHistory_Click() {
        var b = branchIds(); if (!b) { box('Select branch first'); return; }
        return getJson(api + '/history-suppliers?branchIds=' + encodeURIComponent(b)).then(function (rows) { fill('cmbSupplierNameHistory', rows, 'Id', 'Description'); refreshCombos(); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ attachments

    function btnAttachment_Click() { var g = $id('grpAttachments'); g.style.display = g.style.display === 'none' ? '' : 'none'; }
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

    // ------------------------------------------------------------------ events

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function later(fn) { return function (e) { var t = e.target; setTimeout(function () { fn(t, e); }, 0); }; }

    function bindEvents() {
        on('rdbtnItemName', 'change', ItemNameBind);
        on('rdbtnItemCode', 'change', ItemNameBind);
        on('comItem', 'change', function () { setVal('CmbRackName', ''); comItem_Leave(); AmountCaluculation(); refreshCombos(); });
        on('comWarehouse', 'change', function () { RackBindFromGlobalRacksByItemId(int(val('comItem')), int(val('comWarehouse'))); });
        on('CmbRackName', 'change', CmbRackName_Leave);
        on('CmbItemCondition', 'change', BalanceStock);
        on('txtQty', 'input', AmountCaluculation);
        on('txtRate', 'input', AmountCaluculation);
        on('comRateUOM', 'change', function () { AmountCaluculation(); BillAmount(); });
        on('CmbTaxName', 'change', function () { if (!int(val('CmbTaxName'))) { setVal('txtTaxPercnt', ''); setVal('txtTaxAmount', ''); setVal('txtTotalAmount', amt(num(val('txtAmount')))); } else CalculateTaxAmount(); });
        on('txtTaxPercnt', 'input', CalculateTaxAmount);
        on('CmbRefDocumentType', 'change', CmbRefDocumentType_Leave);
        on('CmbRefDocNo', 'change', CmbRefDocNo_Leave);
        on('CmbRefDocInvoiceNo', 'change', CmbRefDocInvoiceNo_Leave);
        on('cmbCurrency', 'change', function () {
            if (!int(val('cmbCurrency'))) return;
            getJson(api + '/exchange-rate?currencyId=' + int(val('cmbCurrency'))).then(function (d) { setVal('txtExchangeRate', fixed(d.rate, int(cfg.rateDecimals))); txtExchangeRate_TextChanged(); renderAll(); })
                .catch(function (e) { box(e.message); });
        });
        on('txtExchangeRate', 'input', function () { txtExchangeRate_TextChanged(); renderAll(); });
        on('comsupplier', 'change', BillAmount);
        on('DocDate', 'change', DocDate_Leave);
        on('grd', 'dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) grd_DoubleClick(int(tr.getAttribute('data-i'))); });
        on('grd', 'click', function (e) { var b = e.target.closest('button[data-del]'); if (b) DeleteDetailrow(int(b.getAttribute('data-del'))); });

        on('grdFreight', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtFreight[int(tr.getAttribute('data-fr'))]; if (!r) return;
            if (f === 'Remarks') { r.Remarks = t.value; return; }
            if (f === 'Transporter') r.Transporter = int(t.value); else r.Freight = num(t.value);
            BillAmount(); FreightProportion(); renderAll();
        }));
        on('grdFreight', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-fr')), r = dtFreight[i];
            if (b.getAttribute('data-act') === 'del') { dtFreight.splice(i, 1); if (!dtFreight.length) AddRowInFreightGrid(); }
            else if (r) dtFreight.push({ Transporter: r.Transporter, Freight: 0, Remarks: r.Remarks });
            BillAmount(); renderAll();
        });
        on('grdGLedger', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtGrdGL[int(tr.getAttribute('data-gl'))]; if (!r) return;
            if (f === 'Remarks') { r.Remarks = t.value; return; }
            if (f === 'AccountId') {
                r.AccountId = int(t.value);
                if (r.AccountId && supplierGl() === r.AccountId) { box('Supplier Account Not select'); r.AccountId = 0; renderAll(); return; }
            } else {
                r[f] = num(t.value);
                if (f === 'Qty' || f === 'Rate') { if (t.value !== '') { r.Credit = num(r.Qty) * num(r.Rate); r.Debit = 0; r.Percentage = 0; } }
                else if (f === 'Percentage') {
                    var v = totals().amount / 100 * num(r.Percentage);
                    if (v > 0) { r.Credit = even(v); r.Debit = 0; } else { r.Debit = Math.abs(even(v)); r.Credit = 0; }
                    r.Qty = 0; r.Rate = 0;
                } else if (f === 'Credit') { if (num(r.Debit) > 0) { r.Credit = 0; box('Debit Side is aleady added'); } }
                else if (f === 'Debit') { if (num(r.Credit) > 0) { r.Debit = 0; box('Credit Side is aleady added'); } }
            }
            BillAmount(); renderAll();
        }));
        on('grdGLedger', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-gl'));
            if (b.getAttribute('data-act') === 'del') { dtGrdGL.splice(i, 1); if (!dtGrdGL.length) AddRowInGLGrid(); } else AddRowInGLGrid();
            BillAmount(); renderAll();
        });
        on('grdHistory', 'click', onHistoryClick);
        on('grdHistory', 'dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var r = historyRows[int(tr.getAttribute('data-h'))]; if (r) editFromHistory(int(col(r, 'Id')));
        });
        on('fileAttachment', 'change', onAttachmentsPicked);
        on('lstAttachments', 'click', function (e) {
            var rm = e.target.getAttribute('data-remove'), dr = e.target.getAttribute('data-drop');
            if (rm) { removeAttachmentIds.push(int(rm)); renderAttachments(); }
            if (dr !== null && dr !== undefined) { newFiles.splice(int(dr), 1); renderAttachments(); }
        });
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 's') { e.preventDefault(); if (currentTab === 0) { if (RecId === 0 && perms.Save) Insert('btnSave'); } else GetAll(); }
            else if (k === 'u') { e.preventDefault(); if (RecId > 0 && perms.Update) btnUpdate(); }
            else if (k === 'n') { e.preventDefault(); if (currentTab === 0) btnNew_Click(); else btnNewHistory_Click(); }
            else if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
            else if (k === 'e') { e.preventDefault(); window.location.href = '/dashboard'; }
        });
    }
    /* btnUpdate_Click :2626 */
    function btnUpdate() { if (Approved) { box('Record Not Update because Record has approved'); return; } return Insert('btnUpdate'); }

    window.PiDpm = {
        btnNew_Click: btnNew_Click, btnRefresh_Click: btnRefresh_Click,
        btnSave_Click: function () { if (RecId !== 0 || $id('btnSave').disabled) return; return Insert('btnSave'); },
        btnUpdate_Click: function () { if (RecId === 0 || $id('btnUpdate').disabled) return; return btnUpdate(); },
        btnDelete_Click: btnDelete_Click,
        btnPrint_Click: function () { if (perms.Print) VoucherReport_118(VoucherHeadId); },
        btnSlip_Click: function () { if (perms.Print) GeneratePrint(RecId); },
        btnAttachment_Click: btnAttachment_Click,
        btnAdd_Click: btnAdd_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, ResetDetail: ResetDetail,
        showTab: showTab, btnshow_Click: function () { return busy('btnshow', GetAll); },
        btnNewHistory_Click: btnNewHistory_Click, btnRefreshHistory_Click: btnRefreshHistory_Click
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
