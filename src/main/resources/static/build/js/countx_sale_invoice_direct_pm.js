/* ============================================================================================
 * Sale Invoice Packing Material - screen 507, module 54, DocumentTypeId 126, BaseDocumentTypeId 2.
 * Desktop: InvfrmInvSaleInvoiceDirectPackingMaterial (ScreenName FrmSaleInvoiceDirectPackingMaterial)
 *          + frmLoadGdnStoreAndPm (OrderCategoryId 9).
 *
 *   InitializeComponentMethod :538   comItem_Leave :976   WareHouseBind :856   RackBind :908   TaxTypeDbCall :1011
 *   FormValidationDetila :1106   btnAdd :1200   FillDetailRow :1244   grd_DoubleClick :1310   grdSettings :1368
 *   DeleteDetailRow :1469   HandleF1Actions (tax) :1555   freight grid :1717-1835   GL grid :1837-1995
 *   Insert :2037   ReadById :2330   Reset :2504   BillAmount :2650   FreightProportion :2729   BillProportion :2762
 *   CommissionProportion :2791   CalculateAmount :2823   secondary UOM :2926-3044   TotalCommissionAmount :3093
 *   AvailableStockGetByItem :3182   HistoryGridFill :3351   DocDate_Leave :3702   btnLoadGdn :4018   LoadInGridDetail :4046
 * The server re-reads every GDN line, re-prices manual lines and recomputes every proportion.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/sale-invoice';
    var L = null, cfg = {}, perms = {};
    var dtGrid = [], dtFreight = [], dtGrdGL = [];
    var Id = 0, VoucherHeadId = 0, Approved = false, updateDetailIndex = -1;
    var taxRows = [], historyRows = [], currentTab = 0;
    var gdnData = [], gdnHeaders = [];
    /* the form's static flags */
    var CalculateBySecondaryUomRate = true, CalculateByItemRate = false;
    var _isUpdatingRates = false;

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
    /* Conversion.ToInt of a text: Convert.ToInt32(string) - 0 unless it is a whole number without separators. */
    function toIntText(t) { t = String(t === null || t === undefined ? '' : t).trim(); return /^[-+]?\d+$/.test(t) ? parseInt(t, 10) : 0; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function even(x, n) {
        var f = Math.pow(10, n || 0), y = x * f, r = Math.round(y);
        if (Math.abs(Math.abs(y % 1) - 0.5) < 1e-9) r = 2 * Math.round(y / 2);
        return r / f;
    }
    function away(x, n) { var f = Math.pow(10, n || 0); var s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function dp() { return int(cfg.amountDecimals); }
    function fmt(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 4 : d }); }
    /* "#,##0.###" and "#,##.##" */
    function f3(n) { return num(n).toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    function f2blank(n) { n = num(n); return n === 0 ? '' : n.toLocaleString('en-US', { maximumFractionDigits: 2 }).replace(/^0\./, '.'); }
    function f3blank(n) { n = num(n); return n === 0 ? '' : n.toLocaleString('en-US', { maximumFractionDigits: 3 }).replace(/^0\./, '.'); }
    function amt(n) { return num(n).toLocaleString('en-US', { minimumFractionDigits: dp(), maximumFractionDigits: dp() }); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function addDays(s, n) { var d = new Date((s || today()) + 'T00:00:00'); d.setDate(d.getDate() + n); return iso(d); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0].substring(2); }

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
    function setText(id, t) {
        var s = $id(id); if (!s) return;
        var o = Array.prototype.filter.call(s.options, function (x) { return x.text === String(t); })[0];
        s.value = o ? o.value : '';
    }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function byId(list, id) { id = int(id); return (list || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0] || null; }
    function acctKey() { return cfg.subsidiary ? 'SupplierCustomerId' : 'Id'; }
    function accountOptions(current) {
        var html = '<option value="0"></option>', k = acctKey();
        (L && L.accounts || []).forEach(function (a) {
            var v = int(col(a, k));
            html += '<option value="' + v + '"' + (v === int(current) ? ' selected' : '') + '>' + esc(col(a, 'AccountTitle')) + '</option>';
        });
        return html;
    }
    function customerGl() { var c = byId(L && L.customers, val('CmbSupplierName')); return c ? int(c.GlAccountId) : 0; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today()); setVal('txtGpdate', today()); setVal('duedate', today()); setVal('txtduedays', '0');
        loadLookups().then(function () {
            AddRowInFreightGrid(); AddRowInGLGrid();
            var back = int(cfg.defaultDaysToLessFromHistoryFromDate);
            setVal('FromDateHistory', addDays(today(), -(back > 0 ? back : 3))); setVal('ToDateHistory', today());
            renderAll(); refreshCombos(); focus('DocDate');
        });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; cfg = d.configuration || {}; perms = d.rights || {};
            fill('CmbSupplierName', d.customers, 'Id', 'CompanyName');
            fill('CmbCommissionAgent', d.customers, 'Id', 'CompanyName');
            fill('CmbTaxAccount', d.taxAccounts, 'Id', 'AccountTitle');
            fill('CmbPaymentTerm', d.paymentTerms, 'Id', 'Description');
            fill('CmbCommType', d.commissionTypes, 'Id', 'Description');
            fill('CmbCommUom', d.commissionUoms, 'Id', 'Description');
            fill('CmbJobLot', d.jobLots, 'Id', 'Description');
            fill('CmbItemCondition', d.conditions, 'Id', 'Description');
            fill('CmbCustomerHistory', d.historyCustomers, 'Id', 'Name');
            ItemNameBind();
            if (Id === 0) { setVal('txtdocno', d.docNo); setVal('txtTaxInvoiceNo', d.taxNo); $id('txtDocNoShow').textContent = 'SI-' + d.docNo; }
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    function ItemNameBind() {
        fill('CmbItemName', (L && L.items) || [], 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode');
        refreshCombos();
    }

    function applyRights() {
        var upd = Id > 0;
        show('btnSave', !upd); show('btnUpdate', upd); show('btnDelete', upd);
        $id('btnSave').disabled = !perms.save; $id('btnUpdate').disabled = !perms.update; $id('btnDelete').disabled = !perms['delete'];
        $id('btnPrint').disabled = !perms.print;
    }

    // ------------------------------------------------------------------ entry bar

    function itemRacks(itemId) { return (L.racks || []).filter(function (r) { return int(r.ItemId) === int(itemId); }); }
    function itemUoms(itemId) { return (L.uoms || []).filter(function (u) { return int(u.ItemId) === int(itemId); }); }

    /* comItem_Leave :976 */
    function comItem_Leave() {
        var itemId = int(val('CmbItemName'));
        ItemUomFromGlobalBind(itemId);
        WareHouseBindFromGlobalRacksByItemId(itemId);
        RackBindFromGlobalRacksByItemId(itemId, int(val('CmbWarehouse')));
        if (itemId > 0) return TaxTypeBind(itemId);
        taxRows = []; fill('CmbTaxType', [], 'Id', 'TaxType'); setVal('txtTaxPercent', ''); setVal('txtTaxAmount', '');
        return Promise.resolve();
    }

    /* CommonBindings.ItemUomFromGlobalBind - the texts are kept, then the base rows win. */
    function ItemUomFromGlobalBind(itemId) {
        var list = itemUoms(itemId);
        [['CmbPackUOM', 'BasePackUom'], ['CmbRateUOM', 'BaseRateUom'], ['CmbSecondaryUom', 'BaseSecondaryUom']].forEach(function (p) {
            var t = text(p[0]);
            fill(p[0], list, 'Id', 'UOMCode');
            setText(p[0], t);
            var base = list.filter(function (u) { return u[p[1]] === true; })[0];
            if (base) setVal(p[0], base.Id);
        });
    }

    function WareHouseBindFromGlobalRacksByItemId(itemId) {
        var seen = {}, list = [];
        itemRacks(itemId).forEach(function (r) { if (!seen[r.WarehouseId]) { seen[r.WarehouseId] = 1; list.push({ Id: r.WarehouseId, WareHouseName: r.WareHouseName }); } });
        fill('CmbWarehouse', list, 'Id', 'WareHouseName');
        if (int(val('CmbRackName')) !== 0) return;
        if (list.length === 1) setVal('CmbWarehouse', list[0].Id);
        else if (list.length > 1 && int(cfg.defaultWarehouseForStoreFlow) && list.some(function (w) { return int(w.Id) === int(cfg.defaultWarehouseForStoreFlow); }))
            setVal('CmbWarehouse', cfg.defaultWarehouseForStoreFlow);
    }

    function RackBindFromGlobalRacksByItemId(itemId, whId) {
        var seen = {}, list = [];
        itemRacks(itemId).forEach(function (r) { if ((!whId || int(r.WarehouseId) === whId) && !seen[r.Id]) { seen[r.Id] = 1; list.push(r); } });
        fill('CmbRackName', list, 'Id', 'RackName');
        if (int(val('CmbRackName')) === 0 && list.length === 1) { setVal('CmbRackName', list[0].Id); CmbRackName_Leave(); }
    }

    function CmbRackName_Leave() {
        var rack = byId(L.racks, val('CmbRackName'));
        if (rack && int(rack.WarehouseId) > 0) {
            if (!has('CmbWarehouse', rack.WarehouseId)) WareHouseBindFromGlobalRacksByItemId(int(val('CmbItemName')));
            setVal('CmbWarehouse', rack.WarehouseId);
        }
        AvailableStockGetByItem();
    }

    /* AvailableStockGetByItem :3182 */
    function AvailableStockGetByItem() {
        $id('lblBalance').textContent = '0';
        var itemId = int(val('CmbItemName')); if (!itemId) return;
        var q = '?itemId=' + itemId + '&docDate=' + encodeURIComponent(val('DocDate')) + '&conditionId=' + int(val('CmbItemCondition'))
            + '&warehouseId=' + int(val('CmbWarehouse')) + '&rackId=' + int(val('CmbRackName'));
        return getJson(api + '/stock' + q).then(function (d) { $id('lblBalance').textContent = String(d.qtyInHand); }).catch(function () { /* the label stays 0 */ });
    }

    /* TaxTypeDbCall / TaxTypeBind :1011-1044 */
    function taxOptions(itemId) {
        return getJson(api + '/tax-options?itemId=' + int(itemId) + '&docDate=' + encodeURIComponent(val('DocDate')));
    }
    function TaxTypeBind(itemId) {
        return taxOptions(itemId).then(function (rows) {
            taxRows = rows || [];
            var keep = int(val('CmbTaxType'));
            fill('CmbTaxType', taxRows, 'Id', 'TaxType');
            if (updateDetailIndex > -1 && dtGrid[updateDetailIndex] && int(dtGrid[updateDetailIndex].TaxNameId) > 0) keep = int(dtGrid[updateDetailIndex].TaxNameId);
            setVal('CmbTaxType', keep && has('CmbTaxType', keep) ? keep : '');
            CalculateTaxAmount();
        }).catch(function (e) { box(e.message); });
    }

    /* CalculateAmount :2823 - Math.Round(qty / RateUom.Equivalent * rate, 3) */
    function CalculateAmount() {
        var u = byId(L.uoms, val('CmbRateUOM')), eq = u ? num(u.Equivalent) : 0, rate = num(val('txtRate')), qty = num(val('txtQty'));
        setVal('txtAmount', String(qty > 0 && eq > 0 && rate > 0 ? even(qty / eq * rate, 3) : 0));
        CalculateTaxAmount();
    }
    /* CalculateTaxAmount :2834 - the percent is always the selected schedule row's. */
    function CalculateTaxAmount() {
        var amount = num(val('txtAmount'));
        var t = byId(taxRows, val('CmbTaxType')), pct = t ? num(t.TaxPrcnt) : 0;
        setVal('txtTaxPercent', f3blank(pct));
        var tax = amount * pct / 100;
        setVal('txtTaxAmount', amt(away(tax, dp()))); setVal('txtTotalAmount', amt(away(amount + tax, dp())));
    }

    /* CalculateDetailWeights :2926 + txtNetBillWeight_TextChanged :2937 */
    function CalculateDetailWeights() {
        var qty = num(val('txtQty')), net = num(val('txtSecondaryUomQty')) + num(val('txtAddLessWeight'));
        setVal('txtNetBillWeight', f2blank(net));
        setVal('txtPerItemWeight', f2blank(qty > 0 ? net / qty : 0));
    }
    function txtNetBillWeight_TextChanged() {
        var qty = num(val('txtQty')), net = num(val('txtNetBillWeight'));
        setVal('txtPerItemWeight', f2blank(qty > 0 ? net / qty : 0));
    }
    /* ItemRateAndSecondaryRateCalculations :2995 - ActiveControl.AccessibleName; txtRate has none, so any control
       without one counts as txtRate. */
    var ACCESSIBLE = { txtPerItemWeight: 'PerItemWeight', txtNetBillWeight: 'NetBillWeight', txtAddLessWeight: '', txtSecondaryUomQty: 'SecondaryUomQty', txtSecondaryUomRate: 'SecondaryUomRate' };
    function activeName() { var a = document.activeElement; return a && Object.prototype.hasOwnProperty.call(ACCESSIBLE, a.id) ? ACCESSIBLE[a.id] : null; }
    function ItemRateAndSecondaryRateCalculations() {
        if (_isUpdatingRates) return;
        _isUpdatingRates = true;
        try {
            var itemRate = num(val('txtRate')), secRate = num(val('txtSecondaryUomRate')), secQty = num(val('txtSecondaryUomQty')), qty = num(val('txtQty'));
            if (qty <= 0 && secQty <= 0) { setVal('txtRate', '0'); setVal('txtSecondaryUomRate', '0'); return; }
            var a = activeName();
            if (a === null) {
                setVal('txtSecondaryUomRate', f3(secQty > 0 ? even(itemRate * (qty / secQty), 3) : 0));
                CalculateByItemRate = true; CalculateBySecondaryUomRate = false;
            } else if (a === 'SecondaryUomRate') {
                setVal('txtRate', f3(qty > 0 ? even(secRate * secQty / qty, 3) : 0));
                CalculateByItemRate = false; CalculateBySecondaryUomRate = true;
            } else if (CalculateBySecondaryUomRate) {
                setVal('txtRate', f3(qty > 0 ? even(secRate * secQty / qty, 3) : 0));
            } else if (CalculateByItemRate) {
                setVal('txtSecondaryUomRate', f3(secQty > 0 ? even(itemRate * (qty / secQty), 3) : 0));
            }
        } finally { _isUpdatingRates = false; }
    }

    /* FormValidationDetila :1106 */
    function FormValidationDetila() {
        if (int(val('CmbTaxType')) > 0 || num(val('txtTaxPercent')) > 0 || num(val('txtTaxAmount')) > 0) {
            if (!int(val('CmbTaxType'))) { box('Tax Name Field is Required'); focus('CmbTaxType'); return false; }
            if (num(val('txtTaxPercent')) === 0) { box('Tax Percent Field is Required'); return false; }
            if (num(val('txtTaxAmount')) === 0) { box('Tax Amount Field is Required'); return false; }
        }
        var checks = [['CmbWarehouse', 'Warehouse', 'i'], ['CmbJobLot', 'Job/Lot', 'i'], ['CmbItemName', 'Item Name', 'i'], ['txtQty', 'Qty', 'd'],
            ['CmbPackUOM', 'Pack Unit', 'i'], ['CmbItemCondition', 'Item Condition', 'i'], ['txtRate', 'Rate', 'd'], ['CmbRateUOM', 'Rate UOM', 'i'], ['txtAmount', 'Amount', 'd']];
        for (var i = 0; i < checks.length; i++) {
            var v = checks[i][2] === 'i' ? int(val(checks[i][0])) : num(val(checks[i][0]));
            if (!v) { box(checks[i][1] + ' field is required'); focus(checks[i][0]); return false; }
        }
        return true;
    }

    /* FillDetailRow :1244 */
    function FillDetailRow(r) {
        var item = byId(L.items, val('CmbItemName'));
        r.ItemId = int(val('CmbItemName')); r.ItemCode = item ? item.ItemCode : ''; r.Item = item ? item.ItemName : '';
        r.WarehouseId = int(val('CmbWarehouse')); r.Warehouse = text('CmbWarehouse');
        r.RackId = int(val('CmbRackName')); r.RackName = text('CmbRackName');
        r.ItemConditionId = int(val('CmbItemCondition')); r.ItemCondition = text('CmbItemCondition').trim();
        r.JobLotId = int(val('CmbJobLot')); r.JobLot = text('CmbJobLot');
        r.PackUOMId = int(val('CmbPackUOM')); r.PackUOM = text('CmbPackUOM').trim();
        r.ItemQty = num(val('txtQty'));
        r.SecondaryUomId = int(val('CmbSecondaryUom')); r.SecondaryUom = text('CmbSecondaryUom');
        r.SecondaryUomQty = num(val('txtSecondaryUomQty')); r.AddLessWeight = num(val('txtAddLessWeight'));
        r.NetBillWeight = num(val('txtNetBillWeight')); r.PerItemWeight = num(val('txtPerItemWeight'));
        r.SecondaryUomItemRate = num(val('txtSecondaryUomRate'));
        r.ItemRate = num(val('txtRate')); r.RateUOMId = int(val('CmbRateUOM')); r.RateUOM = text('CmbRateUOM');
        r.ItemAmount = num(val('txtAmount'));
        var taxId = int(val('CmbTaxType')), pct = toIntText(val('txtTaxPercent')), tax = r.ItemAmount * pct / 100;
        r.TaxNameId = taxId; r.TaxName = taxId > 0 ? text('CmbTaxType') : ''; r.TaxPercent = pct; r.TaxAmount = tax; r.TotalAmount = r.ItemAmount + tax;
        r.GpDate = val('txtGpdate'); r.GpNo = toIntText(val('txtGpNo')); r.VehicleNo = val('txtvehicleno'); r.Remarks = val('txtRemarksDetail');
        return r;
    }
    function newRow() {
        return { Id: 0, GdnId: 0, GdnDetailId: 0, GdnNo: 0, SoId: 0, SoDetailId: 0, BillAmount: 0, Commission: 0, Freights: 0 };
    }

    function btnAdd_Click() {
        if (!FormValidationDetila()) return;
        dtGrid.push(FillDetailRow(newRow()));
        TotalCommissionAmount(); FreightProportion(); BillAmount(); ResetDetail();
    }
    function btnUpdateDetail_Click() {
        if (!FormValidationDetila() || updateDetailIndex < 0 || !dtGrid[updateDetailIndex]) return;
        FillDetailRow(dtGrid[updateDetailIndex]);
        TotalCommissionAmount(); CommissionProportion(); FreightProportion(); BillAmount(); ResetDetail();
    }

    /* grd_DoubleClick :1310 - manual rows only */
    function grd_DoubleClick(i) {
        var r = dtGrid[i]; if (!r || int(r.GdnId) > 0) return;
        updateDetailIndex = i;
        setVal('CmbItemName', r.ItemId); refreshCombos();
        setVal('CmbRackName', ''); setVal('CmbWarehouse', '');
        var p = comItem_Leave();
        setVal('CmbWarehouse', r.WarehouseId); RackBindFromGlobalRacksByItemId(r.ItemId, int(r.WarehouseId)); setVal('CmbRackName', r.RackId);
        setVal('CmbJobLot', r.JobLotId); setVal('CmbItemCondition', r.ItemConditionId); setVal('CmbPackUOM', r.PackUOMId);
        setVal('txtQty', r.ItemQty); setVal('CmbSecondaryUom', r.SecondaryUomId);
        setVal('txtSecondaryUomQty', r.SecondaryUomQty); setVal('txtAddLessWeight', r.AddLessWeight); setVal('txtNetBillWeight', r.NetBillWeight);
        setVal('txtPerItemWeight', r.PerItemWeight); setVal('txtSecondaryUomRate', r.SecondaryUomItemRate);
        setVal('txtRate', r.ItemRate); setVal('CmbRateUOM', r.RateUOMId); setVal('txtAmount', r.ItemAmount);
        setVal('txtGpdate', dateOnly(r.GpDate) || today()); setVal('txtGpNo', r.GpNo); setVal('txtvehicleno', r.VehicleNo); setVal('txtRemarksDetail', r.Remarks);
        show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancel', true);
        p.then(function () { setVal('CmbTaxType', int(r.TaxNameId) || ''); CalculateAmount(); });
        refreshCombos(); renderAll(); focus('CmbWarehouse');
    }

    /* ResetDetail :2550 */
    function ResetDetail() {
        ['CmbWarehouse', 'CmbJobLot', 'CmbItemName', 'CmbPackUOM', 'txtQty', 'CmbSecondaryUom', 'txtSecondaryUomQty', 'txtAddLessWeight', 'txtNetBillWeight',
            'txtPerItemWeight', 'txtSecondaryUomRate', 'txtRate', 'CmbRateUOM', 'txtAmount', 'CmbTaxType', 'txtTaxPercent', 'txtTaxAmount', 'txtTotalAmount',
            'txtGpNo', 'txtvehicleno'].forEach(function (k) { setVal(k, ''); });
        taxRows = []; fill('CmbTaxType', [], 'Id', 'TaxType');
        updateDetailIndex = -1;
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancel', false);
        refreshCombos(); renderAll(); focus('CmbItemName');
    }

    /* DeleteDetailRow :1469 - manual rows only */
    function DeleteDetailRow(i) {
        var r = dtGrid[i]; if (!r || int(r.GdnId) !== 0) return;
        if (updateDetailIndex !== -1) { box('Please Reset the Detail first..'); return; }
        if (int(r.Id) !== 0 && !window.confirm('Are you sure to Delete?')) return;
        dtGrid.splice(i, 1);
        TotalCommissionAmount(); FreightProportion(); BillAmount();
    }

    /* HandleF1Actions, tax column :1592 */
    function rowTax(i) {
        var r = dtGrid[i]; if (!r) return;
        if (!int(r.ItemId)) { box('Service Item Not Found In Current Row'); return; }
        taxOptions(r.ItemId).then(function (rows) {
            var t = (rows || [])[0];
            if (t && window.confirm('Apply ' + t.TaxType + ' (' + t.TaxPrcnt + ' %) to this row?')) {
                r.TaxNameId = int(t.Id); r.TaxName = t.TaxType; r.TaxPercent = num(t.TaxPrcnt);
            } else if (int(r.TaxNameId) > 0 && window.confirm('No tax selected. Do you want to clear the current Tax fields?')) {
                r.TaxNameId = 0; r.TaxName = ''; r.TaxPercent = 0;
            } else return;
            /* Detail_TaxUpdateInRow :1696 */
            var tax = num(r.ItemAmount) * num(r.TaxPercent) / 100;
            r.TaxAmount = even(tax, 3); r.TotalAmount = even(num(r.ItemAmount) + tax, 3);
            TotalCommissionAmount(); FreightProportion(); BillAmount();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ proportions

    function sum(list, k) { var t = 0; list.forEach(function (r) { t += num(r[k]); }); return t; }
    /* TotalCommissionAmount :3093 */
    function TotalCommissionAmount() {
        var rateText = val('txtCommRate').trim(), type = text('CmbCommType');
        if (rateText !== '') {
            var rate = num(rateText);
            if (type === 'Flat') setVal('txtCommissionAmount', String(rate));
            else if (type === 'Percent' || type === 'Percentage') setVal('txtCommissionAmount', String(even(sum(dtGrid, 'ItemAmount') * rate / 100, 3)));
            else if (type === 'Comm Weight') { var u = num(text('CmbCommUom')); setVal('txtCommissionAmount', String(even(u > 0 ? sum(dtGrid, 'NetBillWeight') / u * rate : 0, 3))); }
        } else setVal('txtCommissionAmount', '0');
        CommissionProportion(); BillAmount();
    }
    /* CommissionProportion :2791 */
    function CommissionProportion() {
        var total = num(val('txtCommissionAmount')), pct = num(val('txtCommRate')), net = sum(dtGrid, 'NetBillWeight');
        dtGrid.forEach(function (r) { r.Commission = text('CmbCommType') === 'Percent' ? num(r.ItemAmount) * pct / 100 : (net ? total / net * num(r.NetBillWeight) : 0); });
        BillProportion();
    }
    /* FreightProportion :2729 */
    function FreightProportion() {
        var net = sum(dtGrid, 'NetBillWeight'), credit = sum(dtFreight, 'Freight');
        dtGrid.forEach(function (r) { r.Freights = credit > 0 && net ? even(credit, 0) / net * num(r.NetBillWeight) : 0; });
        BillProportion();
    }
    /* BillProportion :2762 */
    function BillProportion() {
        var c = String(cfg.creditAmountInItemSaleGL || '');
        dtGrid.forEach(function (r) {
            if (c !== '' && c.toLowerCase() !== 'true') r.BillAmount = num(r.ItemAmount) - num(r.Commission) + num(r.TaxAmount);
            else if (c === '') r.BillAmount = num(r.ItemAmount) - num(r.Freights) - num(r.Commission) + num(r.TaxAmount);
        });
    }
    /* BillAmount :2650 */
    function BillAmount() {
        var jd = 0, jc = 0, tc = 0, cust = cfg.subsidiary ? int(val('CmbSupplierName')) : customerGl();
        for (var i = 0; i < dtGrdGL.length; i++) {
            var g = dtGrdGL[i]; if (int(g.AccountId) <= 0) continue;
            if (int(g.AccountId) === cust && cust) { g.AccountId = 0; renderAll(); box('You cannot select Customer Account in Customer Add Less Grid at row#' + (i + 1)); return; }
            jd += num(g.Debit); jc += num(g.Credit);
        }
        dtFreight.forEach(function (f) { if (int(f.Transporter) > 0 && int(f.Transporter) === cust) tc += num(f.Freight); });
        var bill = sum(dtGrid, 'ItemAmount') + sum(dtGrid, 'TaxAmount');
        if (tc > 0) bill -= Math.abs(tc);
        var diff = jc - jd;
        bill = diff < 0 ? bill - Math.abs(diff) : bill + diff;
        if (int(val('CmbSupplierName')) === int(val('CmbCommissionAgent'))) bill -= num(val('txtCommissionAmount'));
        setVal('txtBillAmount', String(even(bill, 0)));
        BillProportion(); renderAll();
    }

    /* DueDateCalculate :3777 / CmbPaymentTerm_TextChanged :1085 */
    function CmbPaymentTerm_TextChanged() {
        var t = text('CmbPaymentTerm');
        if (t === 'Cash' || t === 'Advance') { $id('txtduedays').disabled = true; setVal('txtduedays', ''); setVal('duedate', val('DocDate')); }
        else $id('txtduedays').disabled = false;
    }
    function DueDateCalculate() {
        if (val('txtduedays').trim() !== '') setVal('duedate', addDays(val('DocDate'), num(val('txtduedays'))));
        else setVal('duedate', today());
    }

    /* DocDate_Leave :3702 */
    function DocDate_Leave() {
        if (!dtGrid.length) return;
        var ids = dtGrid.map(function (r) { return ',' + r.ItemId; }).join('');
        return getJson(api + '/tax-by-items?itemIds=' + encodeURIComponent(ids) + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (rows) {
            dtGrid.forEach(function (r) {
                var found = false;
                (rows || []).forEach(function (x) {
                    if (int(col(x, 'ItemId')) !== int(r.ItemId)) return;
                    found = true;
                    r.TaxNameId = int(col(x, 'TaxNameId')); r.TaxName = col(x, 'TaxName'); r.TaxPercent = num(col(x, 'TaxPercent'));
                    var tax = num(r.ItemAmount) > 0 && r.TaxPercent > 0 ? num(r.ItemAmount) * r.TaxPercent / 100 : 0;
                    r.TaxAmount = tax; r.TotalAmount = num(r.ItemAmount) + tax; r.BillAmount = r.TotalAmount + num(r.Freights);
                });
                if (!found) { r.TaxNameId = 0; r.TaxName = '0'; r.TaxPercent = 0; r.TaxAmount = 0; r.TotalAmount = 0; r.BillAmount = num(r.ItemAmount) + num(r.Freights); }
            });
            BillAmount();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ grids

    function AddRowInFreightGrid() { dtFreight.push({ Transporter: 0, Freight: 0 }); }
    function AddRowInGLGrid() { dtGrdGL.push({ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }); }

    var G = [['GdnNo', 'GdnNo'], ['ItemCode', 'ItemCode'], ['Item', 'Item'], ['Warehouse', 'Warehouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'],
        ['JobLot', 'JobLot'], ['PackUOM', 'PackUOM'], ['ItemQty', 'ItemQty', 'q'], ['SecondaryUom', 'SecondaryUom'], ['SecondaryUomQty', 'Secondary Uom Weight', 'q'],
        ['AddLessWeight', 'AddLessWeight', 'q'], ['NetBillWeight', 'NetBillWeight', 'q'], ['PerItemWeight', 'PerItemWeight', 'q'],
        ['SecondaryUomItemRate', 'SecondaryUomItemRate', 'q'], ['ItemRate', 'ItemRate', 'q'], ['RateUOM', 'RateUOM'], ['ItemAmount', 'ItemAmount', 'a'],
        ['TaxName', 'TaxName'], ['TaxPercent', 'TaxPercent', 'q'], ['TaxAmount', 'TaxAmount', 'a'], ['TotalAmount', 'TotalAmount', 'a'],
        ['GpDate', 'GpDate', 'd'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'], ['BillAmount', 'Total Item Amount', 'a'], ['Commission', 'Commission', 'a'],
        ['Freights', 'Freights', 'a'], ['Remarks', 'Remarks']];

    function manualGrid() { return !dtGrid.length || int(dtGrid[0].GdnId) === 0; }
    function cellHtml(c, v) {
        return '<td' + (c[2] && c[2] !== 'd' ? ' class="num"' : '') + '>' + (c[2] === 'a' ? amt(v) : c[2] === 'q' ? fmt(v) : c[2] === 'd' ? ddmmm(v) : esc(v)) + '</td>';
    }
    function renderAll() {
        var manual = manualGrid();
        /* grdSettings :1368 */
        show('panel4', manual);
        $id('CmbSupplierName').disabled = !manual;
        var cols = G.filter(function (c) { return !(manual && c[0] === 'GdnNo'); });
        var h = (manual ? '<th>X</th><th>Edit</th>' : '') + '<th>Tax</th>'; cols.forEach(function (c) { h += '<th>' + c[1] + '</th>'; }); $id('grdHead').innerHTML = h;
        var html = '';
        dtGrid.forEach(function (r, i) {
            html += '<tr class="data-row' + (i === updateDetailIndex ? ' editing' : '') + (int(r.GdnId) > 0 ? ' gdn-row' : '') + '" data-i="' + i + '">'
                + (manual ? '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>' : '')
                + '<td><button type="button" class="win-btn-mini" data-act="tax">Tax</button></td>';
            cols.forEach(function (c) {
                if (c[0] === 'Remarks') html += '<td><input class="cell-text" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td>';
                else html += cellHtml(c, r[c[0]]);
            });
            html += '</tr>';
        });
        $id('grd').innerHTML = html;
        var f = (manual ? '<td></td><td></td>' : '') + '<td></td>';
        cols.forEach(function (c) { f += '<td class="num">' + (['ItemQty', 'NetBillWeight', 'SecondaryUomQty'].indexOf(c[0]) >= 0 ? fmt(sum(dtGrid, c[0])) : c[2] === 'a' ? amt(sum(dtGrid, c[0])) : '') + '</td>'; });
        $id('grdFoot').innerHTML = dtGrid.length ? f : '';

        $id('frHead').innerHTML = '<th>X</th><th>+</th><th>Credit Ac</th><th>Credit</th>';
        var fr = '';
        dtFreight.forEach(function (r, i) {
            fr += '<tr data-fr="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td><select class="cell-sel" data-f="Transporter">' + accountOptions(r.Transporter) + '</select></td>'
                + '<td><input class="cell" data-f="Freight" value="' + esc(r.Freight) + '"/></td></tr>';
        });
        $id('grdFreight').innerHTML = fr;
        $id('frFoot').innerHTML = '<td></td><td></td><td></td><td class="num">' + amt(sum(dtFreight, 'Freight')) + '</td>';

        $id('glHead').innerHTML = '<th>Delete</th><th>Add</th><th>Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th>';
        var gl = '';
        dtGrdGL.forEach(function (r, i) {
            gl += '<tr data-gl="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">Delete</button></td><td><button type="button" class="win-btn-mini" data-act="add">Add</button></td>'
                + '<td><select class="cell-sel" data-f="AccountId">' + accountOptions(r.AccountId) + '</select></td>'
                + '<td><input class="cell-text" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td>'
                + ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].map(function (k) { return '<td><input class="cell-sm" data-f="' + k + '" value="' + esc(r[k]) + '"/></td>'; }).join('') + '</tr>';
        });
        $id('grdGLedger').innerHTML = gl;
        $id('glFoot').innerHTML = '<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td class="num">' + amt(sum(dtGrdGL, 'Debit')) + '</td><td class="num">' + amt(sum(dtGrdGL, 'Credit')) + '</td>';
    }

    // ------------------------------------------------------------------ GDN loader (frmLoadGdnStoreAndPm)

    /* btnLoadGdn_Click :4018 - with rows in the grid it looks at the HISTORY detail grid (grdDetail) - ported as written. */
    function btnLoadGdn_Click() {
        if (!dtGrid.length) { openGdn(); return; }
        if ($id('grdDetail').rows.length) box("You Can't Load Because another Entry Already Exist");
    }
    function openGdn() {
        $id('gdnModal').classList.add('open');
        return getJson(api + '/gdn-branches').then(function (d) {
            setVal('gdnFromDate', dateOnly(d.fromDate) || today()); setVal('gdnToDate', today());
            var bl = '';
            (d.branches || []).forEach(function (b) {
                bl += '<label class="win-check" style="display:block"><input type="checkbox" checked value="' + int(col(b, 'BranchId')) + '"/> ' + esc(col(b, 'BranchName')) + '</label>';
            });
            $id('gdnBranches').innerHTML = bl;
            return gdnShow();
        }).catch(function (e) { box(e.message); });
    }
    function closeGdn() { $id('gdnModal').classList.remove('open'); }
    function gdnBranchIds() {
        return Array.prototype.filter.call(document.querySelectorAll('#gdnBranches input'), function (x) { return x.checked; }).map(function (x) { return ',' + x.value; }).join('');
    }
    var GH = [['DocumentType', 'DocumentType'], ['BranchName', 'BranchName'], ['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo'], ['CustomerName', 'CustomerName'],
        ['GpNo', 'GpNo'], ['BiltyNo', 'BiltyNo'], ['VehicleNo', 'VehicleNo'], ['ItemQty', 'ItemQty', 'q'], ['TicketNos', 'TicketNos']];
    /* PendingGdnLoad - grouped by Id with the ItemQty sum. */
    function gdnShow() {
        var b = gdnBranchIds();
        if (!b) { box('Select branch first'); return; }
        return getJson(api + '/gdn-pending?fromDate=' + encodeURIComponent(val('gdnFromDate')) + '&toDate=' + encodeURIComponent(val('gdnToDate')) + '&branchIds=' + encodeURIComponent(b)).then(function (rows) {
            gdnData = rows || []; gdnHeaders = [];
            var seen = {};
            gdnData.forEach(function (r) {
                var id = int(col(r, 'Id'));
                if (!seen[id]) {
                    seen[id] = { Id: id, DocumentType: col(r, 'DocumentType'), BranchName: col(r, 'BranchName'), DocDate: col(r, 'DocDate'), DocNo: col(r, 'DocNo'),
                        CustomerName: col(r, 'SupplierCustomer'), GpNo: col(r, 'GpNo'), BiltyNo: col(r, 'BiltyNo'), VehicleNo: col(r, 'VehicleNo'), ItemQty: 0, TicketNos: col(r, 'TicketNos') };
                    gdnHeaders.push(seen[id]);
                }
                seen[id].ItemQty += num(col(r, 'ItemQty'));
            });
            var h = '<th>Select</th>'; GH.forEach(function (c) { h += '<th>' + c[1] + '</th>'; }); $id('gdnHead').innerHTML = gdnHeaders.length ? h : '';
            var html = '';
            gdnHeaders.forEach(function (g, i) {
                html += '<tr class="data-row" data-g="' + i + '"><td><input type="checkbox" data-chk="' + g.Id + '"/></td>';
                GH.forEach(function (c) { if (c[0] === 'BranchName' && cfg.saleInvoiceBranchWise) { html += '<td></td>'; return; } html += cellHtml(c, g[c[0]]); });
                html += '</tr>';
            });
            $id('gdnGrid').innerHTML = html; $id('gdnDetail').innerHTML = ''; $id('gdnDetailHead').innerHTML = '';
            $id('lblGdnCount').textContent = gdnHeaders.length ? gdnHeaders.length + ' GDN(s)' : '';
        }).catch(function (e) { box(e.message); });
    }
    /* grd_SelectionChanged */
    function gdnSelect(i) {
        var g = gdnHeaders[i]; if (!g) return;
        var cols = [['SaleOrderNo', 'SaleOrderNo'], ['ItemName', 'ItemName'], ['Warehouse', 'WareHouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'],
            ['JobLot', 'JobLot'], ['PackUom', 'PackUom'], ['ItemQty', 'ItemQty', 'q'], ['SecondaryUomCode', 'SecondaryUom'], ['SecondaryUomQty', 'SecondaryUomQty', 'q'],
            ['AdLsWeight', 'AddLessWeight', 'q'], ['NetBillWeight', 'NetBillWeight', 'q'], ['PerItemNetWeight', 'PerItemWeight', 'q'], ['AreaCity', 'CityName']];
        var h = ''; cols.forEach(function (c) { h += '<th>' + c[1] + '</th>'; }); $id('gdnDetailHead').innerHTML = h;
        var html = '';
        gdnData.filter(function (r) { return int(col(r, 'Id')) === g.Id; }).forEach(function (r) { html += '<tr>'; cols.forEach(function (c) { html += cellHtml(c, col(r, c[0])); }); html += '</tr>'; });
        $id('gdnDetail').innerHTML = html;
    }
    /* BtnLoad */
    function gdnLoad() {
        var ids = {};
        Array.prototype.forEach.call(document.querySelectorAll('#gdnGrid input[data-chk]'), function (x) { if (x.checked) ids[x.getAttribute('data-chk')] = 1; });
        var list = Object.keys(ids);
        if (!list.length) { box('Check the row first'); return; }
        if (list.length > 1) { box('Sorry! You can only load a single GDN at a time.'); return; }
        return busy('btnGdnLoad', function () {
            return getJson(api + '/gdn/' + int(list[0])).then(function (rows) { closeGdn(); LoadInGridDetail(rows || []); }).catch(function (e) { box(e.message); });
        });
    }
    /* LoadInGridDetail :4046 */
    function LoadInGridDetail(dt) {
        if (dt.length) {
            var h = dt[0];
            setVal('CmbSupplierName', int(col(h, 'SupplierCustomerId')));
            setVal('CmbPaymentTerm', int(col(h, 'PaymentTermsId')) || ''); CmbPaymentTerm_TextChanged();
            setVal('txtduedays', String(int(col(h, 'OrderDueDays')))); DueDateCalculate();
            setVal('CmbCommissionAgent', int(col(h, 'BrokerAgentSupCustId')) || '');
            setText('CmbCommType', String(col(h, 'CommissionType') || ''));
            setVal('txtCommRate', String(num(col(h, 'CommRate'))));
            setText('CmbCommUom', String(num(col(h, 'UomScheduleIdCmRate'))));
            var existing = {};
            dtGrid.forEach(function (r) { existing[int(r.GdnDetailId)] = 1; });
            dt.forEach(function (dr) {
                var detailId = int(col(dr, 'GdnDetailId'));
                if (existing[detailId]) return; existing[detailId] = 1;
                var itemId = int(col(dr, 'ItemId')), amount = num(col(dr, 'Amount'));
                dtGrid.push({ Id: 0, GdnId: int(col(dr, 'Id')), GdnDetailId: detailId, GdnNo: col(dr, 'DocNo'), SoId: int(col(dr, 'SaleOrderId')), SoDetailId: int(col(dr, 'SaleOrderDetailId')),
                    ItemId: itemId, ItemCode: itemId > 0 ? col(dr, 'ItemCode') : '', Item: itemId > 0 ? col(dr, 'ItemName') : '',
                    WarehouseId: int(col(dr, 'WarehouseId')), Warehouse: col(dr, 'Warehouse'), RackId: int(col(dr, 'RackId')), RackName: col(dr, 'RackName'),
                    ItemConditionId: int(col(dr, 'ItemConditionId')), ItemCondition: col(dr, 'ItemCondition'), JobLotId: int(col(dr, 'JobLotId')), JobLot: col(dr, 'JobLot'),
                    PackUOMId: int(col(dr, 'ItemUomId')), PackUOM: col(dr, 'PackUom'), ItemQty: num(col(dr, 'ItemQty')),
                    SecondaryUomId: int(col(dr, 'SecondaryUomId')), SecondaryUom: col(dr, 'SecondaryUomCode'), SecondaryUomQty: num(col(dr, 'SecondaryUomQty')),
                    AddLessWeight: num(col(dr, 'AdLsWeight')), NetBillWeight: num(col(dr, 'NetBillWeight')), PerItemWeight: num(col(dr, 'PerItemNetWeight')),
                    SecondaryUomItemRate: num(col(dr, 'SecondaryUomItemRate')), ItemRate: num(col(dr, 'ItemRate')), RateUOMId: int(col(dr, 'ItemUomId')), RateUOM: col(dr, 'PackUom'),
                    ItemAmount: amount, TaxNameId: int(col(dr, 'TaxNameId')), TaxName: col(dr, 'TaxName'), TaxPercent: num(col(dr, 'TaxPercent')), TaxAmount: num(col(dr, 'TaxAmount')),
                    TotalAmount: amount + num(col(dr, 'TaxAmount')), GpDate: col(dr, 'GpDate'), GpNo: int(col(dr, 'GpNo')), VehicleNo: col(dr, 'VehicleNo'),
                    BillAmount: 0, Commission: 0, Freights: 0, Remarks: '' });
            });
            if (num(col(h, 'CarriageAmount')) > 0 && sum(dtFreight, 'Freight') === 0) dtFreight = [{ Transporter: int(col(h, 'TransporterId')), Freight: num(col(h, 'CarriageAmount')) }];
        }
        refreshCombos();
        TotalCommissionAmount(); FreightProportion(); BillAmount();
    }

    // ------------------------------------------------------------------ save

    function Insert(btnId) {
        if (!dtGrid.length) { box('Detail Record Not Found'); return; }
        if (!int(val('CmbSupplierName'))) { box('Customer field is required'); focus('CmbSupplierName'); return; }
        if (!int(val('txtdocno'))) { box('Doc No field is required'); return; }
        if (!int(val('CmbPaymentTerm'))) { box('Payment Term field is required'); focus('CmbPaymentTerm'); return; }
        if (int(val('CmbPaymentTerm')) === 2 && toIntText(val('txtduedays')) === 0) { box('Due Days Field is Required'); focus('txtduedays'); return; }
        var hasTax = dtGrid.some(function (r) { return int(r.TaxNameId) > 0; });
        if (hasTax && !int(val('CmbTaxAccount'))) { box('Tax Account field is required'); focus('CmbTaxAccount'); return; }
        for (var j = 0; j < dtGrdGL.length; j++) if ((num(dtGrdGL[j].Credit) > 0 || Math.round(num(dtGrdGL[j].Debit)) > 0) && !int(dtGrdGL[j].AccountId)) { box('Please Select an Account Against JL First'); return; }
        for (var i = 0; i < dtFreight.length; i++) if (num(dtFreight[i].Freight) > 0 && !int(dtFreight[i].Transporter)) { box('Please Select an Account Against Freight First'); return; }
        if (num(val('txtCommissionAmount')) > 0 && !int(val('CmbCommissionAgent'))) { box('Please Select Commission Agent Account First'); return; }
        if (!window.confirm(Id === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
        var req = {
            Id: Id, DocDate: val('DocDate'), SupplierCustomerId: int(val('CmbSupplierName')), ManualBillNo: val('txtManualbillno'),
            TaxAccountId: int(val('CmbTaxAccount')), PaymentTermId: int(val('CmbPaymentTerm')), DueDays: val('txtduedays'), RemarksHeader: val('txtremarks'),
            CommissionAgentId: int(val('CmbCommissionAgent')), CommissionType: text('CmbCommType'), CommRate: val('txtCommRate'), CommUom: text('CmbCommUom'),
            CommAmount: val('txtCommissionAmount'), CommissionRemarks: val('txtCommissionRemarks'),
            details: dtGrid.map(function (r) {
                return { Id: int(r.Id), GdnId: int(r.GdnId), GdnDetailId: int(r.GdnDetailId), ItemId: int(r.ItemId), WarehouseId: int(r.WarehouseId), RackId: int(r.RackId),
                    ItemConditionId: int(r.ItemConditionId), JobLotId: int(r.JobLotId), PackUOMId: int(r.PackUOMId), ItemQty: num(r.ItemQty),
                    SecondaryUomId: int(r.SecondaryUomId), SecondaryUomQty: num(r.SecondaryUomQty), AddLessWeight: num(r.AddLessWeight), NetBillWeight: num(r.NetBillWeight),
                    PerItemWeight: num(r.PerItemWeight), SecondaryUomItemRate: num(r.SecondaryUomItemRate), ItemRate: num(r.ItemRate), RateUOMId: int(r.RateUOMId),
                    TaxNameId: int(r.TaxNameId), TaxPercent: num(r.TaxPercent), TaxAmount: num(r.TaxAmount), BillAmount: num(r.BillAmount),
                    GpDate: dateOnly(r.GpDate), GpNo: r.GpNo, VehicleNo: r.VehicleNo || '' };
            }),
            freights: dtFreight.map(function (f) { return { Transporter: int(f.Transporter), Freight: num(f.Freight) }; }),
            journals: dtGrdGL.map(function (g) { return { AccountId: int(g.AccountId), Remarks: g.Remarks || '', Percentage: num(g.Percentage), Qty: num(g.Qty), Rate: num(g.Rate), Debit: num(g.Debit), Credit: num(g.Credit) }; })
        };
        return busy(btnId, function () {
            say('Saving...');
            return http('POST', api + '/save', req).then(function (d) {
                say(''); box(d.message);
                var id = d.id, vh = d.voucherHeadId;
                return Reset().then(function () {
                    if ($id('ChkBok').checked) VoucherReport_118(vh);
                    if ($id('ChkPrintslip').checked) SaleInvoiceStorePMSlip(id);
                });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }

    function btnDelete_Click() {
        if (Approved) { box('Record cannot be  Delete because Record has approved'); return; }
        if (Id <= 0) { box('RecordId Not Found.....'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        return busy('btnDelete', function () {
            return http('POST', api + '/' + Id + '/delete').then(function (d) { box(d.message); return Reset(); }).catch(function (e) { box(e.message); });
        });
    }

    function VoucherReport_118(vh) { if (!int(vh)) { box('VoucherId Not Found'); return; } window.open('/api/print/acc-118/pdf?id=' + encodeURIComponent(vh) + '&documentTypeId=126', '_blank'); }
    function SaleInvoiceStorePMSlip(id) { if (!int(id)) { box('No Record Found For Display'); return; } window.open('/api/print/sidpm-294/pdf?id=' + encodeURIComponent(id), '_blank'); }

    // ------------------------------------------------------------------ reset / read

    function Reset() {
        Id = 0; VoucherHeadId = 0; Approved = false;
        ['CmbSupplierName', 'txtManualbillno', 'CmbTaxAccount', 'txtremarks', 'CmbPaymentTerm', 'CmbCommissionAgent', 'CmbCommType', 'txtCommissionRemarks',
            'txtCommRate', 'txtCommissionAmount', 'CmbCommUom', 'txtBillAmount'].forEach(function (k) { setVal(k, ''); });
        setVal('txtduedays', '0'); $id('txtduedays').disabled = false; DueDateCalculate();
        dtGrid = []; dtFreight = []; dtGrdGL = []; AddRowInFreightGrid(); AddRowInGLGrid();
        ResetDetail();
        applyRights();
        return getJson(api + '/numbers').then(function (d) {
            setVal('txtdocno', d.docNo); setVal('txtTaxInvoiceNo', d.taxNo); $id('txtDocNoShow').textContent = 'SI-' + d.docNo;
            refreshCombos(); focus('DocDate');
        }).catch(function (e) { box(e.message); });
    }
    function btnNew_Click() { return busy('btnNew', Reset); }
    function btnRefresh_Click() { return busy('btnRefresh', function () { return loadLookups().then(renderAll); }); }

    function gridRowFromSaved(x) {
        return { Id: int(col(x, 'Id')), GdnId: int(col(x, 'InvGdnId')), GdnDetailId: int(col(x, 'InvGdnDetailId')), GdnNo: col(x, 'GdnNo'),
            SoId: int(col(x, 'SaleOrderId')), SoDetailId: int(col(x, 'SaleOrderDetailId')), ItemId: int(col(x, 'ItemId')), ItemCode: col(x, 'ItemCode'), Item: col(x, 'ItemName'),
            WarehouseId: int(col(x, 'WarehouseId')), Warehouse: col(x, 'WareHouseName'), RackId: int(col(x, 'RackId')), RackName: col(x, 'rackName'),
            ItemConditionId: int(col(x, 'ItemConditionId')), ItemCondition: col(x, 'ItemCondition'), JobLotId: int(col(x, 'JobLotId')), JobLot: col(x, 'JobLotDescription'),
            PackUOMId: int(col(x, 'ItemUOMId')), PackUOM: col(x, 'UOMCodeItem'), ItemQty: num(col(x, 'ItemQty')),
            SecondaryUomId: int(col(x, 'SecondaryUomId')), SecondaryUom: col(x, 'SecondaryUomCode'), SecondaryUomQty: num(col(x, 'SecondaryUomQty')),
            AddLessWeight: num(col(x, 'AdLsWeight')), NetBillWeight: num(col(x, 'NetBillWeight')), PerItemWeight: num(col(x, 'PerItemSecondaryUomQty')),
            SecondaryUomItemRate: num(col(x, 'SecondaryUomItemRate')), ItemRate: num(col(x, 'ItemRate')), RateUOMId: int(col(x, 'UomScheduleIdRate')), RateUOM: col(x, 'UOMCodeRate'),
            ItemAmount: num(col(x, 'ItemAmount')), TaxNameId: int(col(x, 'TaxNameId')), TaxName: col(x, 'TaxDescriptions'), TaxPercent: num(col(x, 'TaxPercent')),
            TaxAmount: num(col(x, 'TaxAmount')), TotalAmount: num(col(x, 'ItemAmount')) + num(col(x, 'TaxAmount')), GpDate: col(x, 'GpDate'), GpNo: int(col(x, 'GpNo')),
            VehicleNo: col(x, 'VehicleNo'), BillAmount: num(col(x, 'BillAmount')), Commission: num(col(x, 'CommissionAmount')), Freights: num(col(x, 'FreightAmount')),
            Remarks: col(x, 'RemarksDetail') };
    }

    /* ReadById :2330 */
    function ReadById(id) {
        say('Loading...');
        return getJson(api + '/' + id).then(function (h) {
            say('');
            Id = int(col(h, 'Id')); Approved = !!col(h, 'IsApproved'); VoucherHeadId = int(col(h, 'VoucherHeadId'));
            showTab(0);
            setVal('txtdocno', col(h, 'DocNo')); $id('txtDocNoShow').textContent = 'SI-' + col(h, 'DocNo');
            setVal('DocDate', dateOnly(col(h, 'DocDate')));
            setVal('CmbSupplierName', int(col(h, 'SupplierCustomerId')));
            setVal('txtManualbillno', col(h, 'ManualBillNo'));
            if (int(col(h, 'ReferencePartyId')) > 0) setVal('CmbTaxAccount', int(col(h, 'ReferencePartyId')));
            setVal('txtTaxInvoiceNo', col(h, 'SalesTaxNo'));
            if (int(col(h, 'PaymentTermId')) > 0) setVal('CmbPaymentTerm', int(col(h, 'PaymentTermId')));
            CmbPaymentTerm_TextChanged();
            setVal('duedate', dateOnly(col(h, 'DueDate')));
            if (!$id('txtduedays').disabled) { setVal('txtduedays', int(col(h, 'DueDays')) > 0 ? String(int(col(h, 'DueDays'))) : String(col(h, 'SupplierInvoiceNo'))); DueDateCalculate(); }
            setVal('txtBillAmount', col(h, 'BillAmount'));
            setVal('txtremarks', col(h, 'RemarksHeader'));
            if (int(col(h, 'CommissionAgentId')) > 0) setVal('CmbCommissionAgent', int(col(h, 'CommissionAgentId')));
            setText('CmbCommType', String(col(h, 'CommissionType') || ''));
            setVal('txtCommRate', col(h, 'CommRate'));
            setText('CmbCommUom', String(col(h, 'UomScheduleIdCmRate') || ''));
            setVal('txtCommissionAmount', col(h, 'CommAmount'));
            setVal('txtCommissionRemarks', col(h, 'CommissionRemarks'));
            dtGrid = (h.details || []).map(gridRowFromSaved);
            var fl = h.freights || [];
            dtFreight = fl.map(function (f) { return { Transporter: cfg.subsidiary ? int(col(f, 'TransporterSupCustId')) : int(col(fl[0], 'TansporterId')), Freight: num(col(f, 'FreightAmount')) }; });
            dtGrdGL = (h.journals || []).map(function (j) {
                return { AccountId: cfg.subsidiary ? int(col(j, 'TransporterSupCustId')) : int(col(j, 'ChartofAccountId')), Remarks: col(j, 'JvRemarks'), Percentage: num(col(j, 'JvPrcnt')),
                    Qty: num(col(j, 'JvQty')), Rate: num(col(j, 'JvRate')), Debit: num(col(j, 'JvDebit')), Credit: num(col(j, 'JvCredit')) };
            });
            if (!dtFreight.length) AddRowInFreightGrid();
            if (!dtGrdGL.length) AddRowInGLGrid();
            ResetDetail();
            FreightProportion(); TotalCommissionAmount();
            refreshCombos(); renderAll(); applyRights();
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ history

    var H = [['DocNo', 'DocNo'], ['DocDate', 'DocDate', 'd'], ['ManualBillNo', 'ManualBillNo'], ['CustomerName', 'CustomerName'], ['BillAmount', 'BillAmount', 'a'],
        ['EntryDate', 'EntryDate', 'd'], ['UserName', 'EntryUser'], ['ModifyUserName', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 'd'],
        ['ApprovedUserName', 'ApprovedUser'], ['ApprovedDate', 'ApprovedDate', 'd'], ['RemarksHeader', 'Remarks'], ['NoOfAttachments', 'NoOfAttachments']];
    function HistoryGridFill() {
        var dt = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var q = '?dateType=' + dt
            + ($id('chkFromDate').checked ? '&fromDate=' + encodeURIComponent(val('FromDateHistory')) : '')
            + ($id('chkToDate').checked ? '&toDate=' + encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + toIntText(val('FromDocNoHistory')) + '&toDocNo=' + toIntText(val('ToDocNoHistory')) + '&customerId=' + int(val('CmbCustomerHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            var head = '<th>Edit</th><th>View</th><th>Voucher</th>'; H.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
            $id('histHead').innerHTML = historyRows.length ? head : '';
            var html = '';
            historyRows.forEach(function (r, i) {
                html += '<tr class="data-row" data-h="' + i + '">' + ['Edit', 'View', 'Voucher'].map(function (a) {
                    return '<td><button type="button" class="win-btn-mini" data-h-act="' + a + '">' + a + '</button></td>';
                }).join('');
                H.forEach(function (c) { html += cellHtml(c, col(r, c[0])); });
                html += '</tr>';
            });
            $id('grdHistory').innerHTML = html; $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            $id('lblHistCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
        }).catch(function (e) { box(e.message); });
    }
    function onHistoryClick(e) {
        var tr = e.target.closest('tr[data-h]'); if (!tr) return;
        var r = historyRows[int(tr.getAttribute('data-h'))]; if (!r) return;
        Array.prototype.forEach.call(document.querySelectorAll('#grdHistory tr'), function (x) { x.classList.toggle('sel', x === tr); });
        var id = int(col(r, 'Id')), b = e.target.closest('button[data-h-act]');
        if (!b) { GetDetailGrdByHeadId(id); return; }
        var a = b.getAttribute('data-h-act');
        if (a === 'Edit') ReadById(id);
        else if (a === 'View') SaleInvoiceStorePMSlip(id);
        else if (a === 'Voucher') VoucherReport_118(int(col(r, 'VoucherHeadId')));
    }
    function GetDetailGrdByHeadId(id) {
        return getJson(api + '/' + id).then(function (h) {
            var lines = (h.details || []).map(gridRowFromSaved);
            var cols = G;
            var head = ''; cols.forEach(function (c) { head += '<th>' + c[1] + '</th>'; }); $id('histDetailHead').innerHTML = lines.length ? head : '';
            var html = ''; lines.forEach(function (l) { html += '<tr>'; cols.forEach(function (c) { html += cellHtml(c, l[c[0]]); }); html += '</tr>'; });
            $id('grdDetail').innerHTML = html;
        }).catch(function (e) { box(e.message); });
    }
    function btnNewHistory_Click() {
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today()); setVal('FromDocNoHistory', ''); setVal('ToDocNoHistory', ''); setVal('CmbCustomerHistory', '');
        historyRows = []; $id('grdHistory').innerHTML = ''; $id('histHead').innerHTML = ''; $id('grdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
        var d = document.querySelector('input[name="rdDate"][value="doc"]'); if (d) d.checked = true;
        refreshCombos();
    }
    function btnRefreshHistory_Click() {
        return getJson(api + '/history-customers').then(function (rows) { fill('CmbCustomerHistory', rows, 'Id', 'Name'); refreshCombos(); }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ events

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
        if (i === 1) focus('FromDateHistory');
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function later(fn) { return function (e) { var t = e.target; setTimeout(function () { fn(t, e); }, 0); }; }

    function bindEvents() {
        on('rdbtnItemName', 'change', ItemNameBind);
        on('rdbtnItemCode', 'change', ItemNameBind);
        on('CmbItemName', 'change', function () { setVal('CmbRackName', ''); comItem_Leave(); refreshCombos(); });
        on('CmbWarehouse', 'change', function () { RackBindFromGlobalRacksByItemId(int(val('CmbItemName')), int(val('CmbWarehouse'))); AvailableStockGetByItem(); });
        on('CmbRackName', 'change', CmbRackName_Leave);
        on('CmbItemCondition', 'change', AvailableStockGetByItem);
        on('CmbJobLot', 'change', AvailableStockGetByItem);
        on('CmbPackUOM', 'change', function () { CalculateAmount(); AvailableStockGetByItem(); });
        on('txtQty', 'input', function () { CalculateDetailWeights(); ItemRateAndSecondaryRateCalculations(); CalculateAmount(); });
        on('txtSecondaryUomQty', 'input', function () { ItemRateAndSecondaryRateCalculations(); CalculateDetailWeights(); CalculateAmount(); });
        on('txtSecondaryUomRate', 'input', function () { ItemRateAndSecondaryRateCalculations(); CalculateAmount(); });
        on('txtRate', 'input', function () { ItemRateAndSecondaryRateCalculations(); CalculateAmount(); });
        on('txtAddLessWeight', 'input', CalculateDetailWeights);
        on('txtNetBillWeight', 'input', txtNetBillWeight_TextChanged);
        on('CmbRateUOM', 'change', CalculateAmount);
        on('CmbTaxType', 'change', CalculateTaxAmount);
        on('CmbSupplierName', 'change', BillAmount);
        on('CmbCommissionAgent', 'change', TotalCommissionAmount);
        on('CmbCommType', 'change', TotalCommissionAmount);
        on('CmbCommUom', 'change', TotalCommissionAmount);
        on('txtCommRate', 'input', TotalCommissionAmount);
        on('CmbPaymentTerm', 'change', CmbPaymentTerm_TextChanged);
        on('txtduedays', 'input', DueDateCalculate);
        on('DocDate', 'change', function () { DueDateCalculate(); AvailableStockGetByItem(); DocDate_Leave(); });

        on('grd', 'dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button,input')) grd_DoubleClick(int(tr.getAttribute('data-i'))); });
        on('grd', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-i')), a = b.getAttribute('data-act');
            if (a === 'del') DeleteDetailRow(i); else if (a === 'edit') grd_DoubleClick(i); else if (a === 'tax') rowTax(i);
        });
        on('grd', 'change', function (e) {
            var t = e.target; if (t.getAttribute('data-f') !== 'Remarks') return;
            var r = dtGrid[int(t.closest('tr').getAttribute('data-i'))]; if (r) r.Remarks = t.value;
        });

        on('grdFreight', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtFreight[int(tr.getAttribute('data-fr'))]; if (!r) return;
            if (f === 'Transporter') r.Transporter = int(t.value); else r.Freight = num(t.value);
            FreightProportion(); BillAmount();
        }));
        on('grdFreight', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-fr'));
            if (b.getAttribute('data-act') === 'del') { dtFreight.splice(i, 1); if (!dtFreight.length) AddRowInFreightGrid(); } else AddRowInFreightGrid();
            FreightProportion(); BillAmount();
        });
        on('grdGLedger', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr'); if (!f || !tr) return;
            var r = dtGrdGL[int(tr.getAttribute('data-gl'))]; if (!r) return;
            if (f === 'Remarks') { r.Remarks = t.value; return; }
            if (f === 'AccountId') r.AccountId = int(t.value);
            else {
                r[f] = t.value;
                if (f === 'Qty' || f === 'Rate') { if (String(r.Qty) !== '' && String(r.Rate) !== '') { r.Credit = num(r.Qty) * num(r.Rate); r.Debit = 0; r.Percentage = 0; } }
                else if (f === 'Percentage') {
                    if (String(r.Percentage) !== '') {
                        var v = sum(dtGrid, 'ItemAmount') / 100 * num(r.Percentage);
                        if (v > 0) { r.Credit = even(v, 0); r.Debit = 0; } else { r.Debit = Math.abs(even(v, 0)); r.Credit = 0; }
                        r.Qty = 0; r.Rate = 0;
                    }
                } else if (f === 'Credit') { if (num(r.Debit) > 0) { r.Credit = 0; box('Debit Side is aleady added'); } }
                else if (f === 'Debit') { if (num(r.Credit) > 0) { r.Debit = 0; box('Credit Side is aleady added'); } }
            }
            BillAmount();
        }));
        on('grdGLedger', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-gl'));
            if (b.getAttribute('data-act') === 'del') { dtGrdGL.splice(i, 1); if (!dtGrdGL.length) AddRowInGLGrid(); } else AddRowInGLGrid();
            BillAmount();
        });
        on('grdHistory', 'click', onHistoryClick);
        on('grdHistory', 'dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var r = historyRows[int(tr.getAttribute('data-h'))]; if (r) ReadById(int(col(r, 'Id')));
        });
        on('gdnGrid', 'click', function (e) { var tr = e.target.closest('tr[data-g]'); if (tr) gdnSelect(int(tr.getAttribute('data-g'))); });
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && $id('gdnModal').classList.contains('open')) { closeGdn(); return; }
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 's') { e.preventDefault(); if (currentTab === 0) { if (Id === 0 && perms.save) Insert('btnSave'); } else HistoryGridFill(); }
            else if (k === 'u') { e.preventDefault(); if (Id > 0 && perms.update) btnUpdate(); }
            else if (k === 'n') { e.preventDefault(); if (currentTab === 0) btnNew_Click(); else btnNewHistory_Click(); }
            else if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
        });
    }
    /* btnUpdate_Click :2314 */
    function btnUpdate() { if (Approved) { box('Record Not Update because Record has approved'); return; } return Insert('btnUpdate'); }

    window.Sip = {
        btnNew_Click: btnNew_Click, btnRefresh_Click: btnRefresh_Click,
        btnSave_Click: function () { if (Id !== 0 || $id('btnSave').disabled) return; return Insert('btnSave'); },
        btnUpdate_Click: function () { if (Id === 0 || $id('btnUpdate').disabled) return; return btnUpdate(); },
        btnDelete_Click: btnDelete_Click,
        btnPrint_Click: function () { if (perms.print) VoucherReport_118(VoucherHeadId); },
        btnSlip_Click: function () { SaleInvoiceStorePMSlip(Id); },
        btnLoadGdn_Click: btnLoadGdn_Click, closeGdn: closeGdn, gdnShow: function () { return busy('btnGdnShow', gdnShow); }, gdnLoad: gdnLoad,
        btnAdd_Click: btnAdd_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, ResetDetail: ResetDetail,
        showTab: showTab, btnShowHistory_Click: function () { return busy('btnShowHistory', HistoryGridFill); },
        btnNewHistory_Click: btnNewHistory_Click, btnRefreshHistory_Click: btnRefreshHistory_Click
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
