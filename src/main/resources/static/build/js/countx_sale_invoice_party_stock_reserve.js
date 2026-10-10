/* ============================================================================================
 * Sale Invoice Party Stock Reserve - screen 143, SaleInvoicepartyStockReserve, DocumentTypeId 99
 * Desktop: Architecture.WinApp.Sale.InvfrmInvSaleInvoiceDirect with FlagForm = 1
 *   Load :622  bindWareHouse :885  ItemdtFillFromAll :1148  ConfigurationDefault :1826  cmbCurrency_Leave :1905  txtExchangeRate_TextChanged :1981
 *   AvailableStockGetByItem :1993  grdInvExp_* :2069-2200  grdGLedger_* :2200-2400  btnAdd :2474  grd_DoubleClick :2531  btnUpdateDetail :2589
 *   grdFreight_* :2929-3132  FormValidation :3222  FormValidationDetila :3307  Reset :3461  ResetDetail :3549  Insert :3664  ReadById :4128
 *   btnDelete :4270  btnRecordsUpdate :4310  GetAll :4470  BillAmount :5195  ExpProportion :5256  BillProportion :5277  CommissionProportion :5313
 *   FreightProportion :5345  Total :5382  TotalWeight :5430  AmountCaluculation :5496  TotalCommissionAmount :5541  comsupplier_ValueChanged :5714
 *   KeyDown :6268
 * The server re-validates and recomputes everything Insert posts; nothing here is trusted for the amounts.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/sale/api/sale-invoice-party-stock-reserve';
    var DOC = 99;
    var L = {}, perms = {}, cfg = { amountDecimals: 0, rateDecimals: 2 };
    var dtGrid = [], dtExp = [], dtFr = [], dtGl = [], historyRows = [], uoms = [], whItems = null, histBranches = [], histBranchSel = {};
    var Id = 0, VoucherHeadId = 0, Approved = false, updateIndex = -1, selHist = -1, tab = 0, gtab = 0, ledgerBalance = 0, busy = false;
    var STOCKDATE_VISIBLE = false;

    function $id(x) { return document.getElementById(x); }
    function val(x) { var e = $id(x); return e ? e.value : ''; }
    function setVal(x, v) { var e = $id(x); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function first(row, names) { for (var i = 0; i < names.length; i++) { var v = col(row, names[i]); if (v !== '' && v !== null && v !== undefined) return v; } return ''; }
    /* .NET Math.Round(x, n, MidpointRounding.AwayFromZero) */
    function away(x, n) {
        x = num(x); var f = Math.pow(10, n || 0), y = Math.abs(x) * f;
        return (x < 0 ? -1 : 1) * Math.floor(y + 0.5 + 1e-9) / f;
    }
    /* .NET Math.Round(x) - banker's rounding */
    function bankers(x) {
        x = num(x); var f = Math.floor(x), d = x - f;
        if (Math.abs(d - 0.5) < 1e-12) return f % 2 === 0 ? f : f + 1;
        return Math.round(x);
    }
    function truthy(v) { return /^(true|1|yes|y)$/i.test(String(v === null || v === undefined ? '' : v).trim()); }
    function fmt(n, d, zeroText) {
        n = num(n);
        if (n === 0 && zeroText !== undefined) return zeroText;
        return away(n, d).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d || 0 });
    }
    function amt(n) { return fmt(n, cfg.amountDecimals, '0'); }          /* clsGlobalVariables.stringFormatsingle */
    function rateF(n) { return fmt(n, cfg.rateDecimals, '0'); }          /* clsGlobalVariables.DecimalRateFormate */
    function qtyF(n) { return fmt(n, 2, '0'); }                           /* "#,##0.##" */
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    function shortDate(v) { var p = dateOnly(v); return p ? p.slice(8, 10) + '/' + p.slice(5, 7) + '/' + p.slice(0, 4) : ''; }
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
    function fill(x, rows, v, t) {
        var sel = $id(x); if (!sel) return;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(typeof v === 'function' ? v(r) : col(r, v)) + '">' + esc(typeof t === 'function' ? t(r) : col(r, t)) + '</option>'; });
        sel.innerHTML = html;
    }
    function text(x) { var s = $id(x); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function show(x, on) { var e = $id(x); if (!e) return; var w = e.closest ? e.closest('.dtcombo-wrap') : null; (w || e).style.display = on ? '' : 'none'; }
    function focus(x) { var e = $id(x); if (e) try { e.focus(); } catch (z) { /* ignore */ } }
    function on(x, ev, fn) { var e = $id(x); if (e) e.addEventListener(ev, fn); }
    function custName(r) { return first(r, ['CompanyName', 'CustomerName', 'Name']); }
    function nf(n, d) { return fmt(n, d, '0'); }
    function isMulti() { return !!cfg.multiCurrency; }
    function acctKey() { return cfg.subsidiary ? 'SupplierCustomerId' : 'Id'; }

    // ------------------------------------------------------------------ load (Load :622)

    function itemText(r) { return $id('rdbtnItemCode').checked ? col(r, 'ItemCode') : col(r, 'ItemName'); }
    function brandText(r) { return $id('RadBrandCode').checked ? col(r, 'ItemCode') : col(r, 'ItemName'); }
    function whName(r) { return col(r, 'WareHouseName'); }

    function categoryBind() {                                                                              /* ItemCategoryOrTypeBind :1090 */
        var keep = val('CmbCategory'), isCat = $id('RadCategory').checked, seen = {}, rows = [];
        (L.items || []).forEach(function (r) {
            var d = String(col(r, isCat ? 'ItemCategory' : 'ItemType'));
            if (d !== '' && !seen[d]) { seen[d] = 1; rows.push({ Id: col(r, isCat ? 'ItemCategoryId' : 'ItemTypeId'), Description: d }); }
        });
        fill('CmbCategory', rows, 'Id', 'Description');
        setVal('CmbCategory', keep); $id('CmbCategory').setAttribute('data-dtcombo-caption', isCat ? 'Item Category' : 'ItemType');
    }
    function itemFill() {                                                                                  /* ItemdtFillFromAll :1148 + ItemDetailFill */
        var keep = val('CmbItemName'), rows;
        if (cfg.itemsAgainstWarehouse) rows = whItems || [];
        else {
            var cid = int(val('CmbCategory')), isCat = $id('RadCategory').checked;
            rows = (L.items || []).filter(function (r) { return cid === 0 || (isCat ? int(col(r, 'ItemCategoryId')) === cid : int(col(r, 'ItemTypeId')) === cid); });
        }
        fill('CmbItemName', rows, 'Id', itemText);
        setVal('CmbItemName', keep);
        if (int(val('CmbItemName')) === 0) setVal('CmbItemName', '');
    }
    function brandFill() { var keep = val('CmbBrandItemName'); fill('CmbBrandItemName', L.brands, 'Id', brandText); setVal('CmbBrandItemName', keep); }
    function loadWarehouseItems() {
        if (!cfg.itemsAgainstWarehouse) { itemFill(); refreshCombos(); return Promise.resolve(); }
        var w = int(val('comWarehouse'));
        if (!w) { whItems = []; itemFill(); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/items-by-warehouse?warehouseId=' + w).then(function (rows) { whItems = rows || []; itemFill(); refreshCombos(); }, function (e) { box(e.message); });
    }

    function bindLists() {
        fill('comsupplier', L.customers, 'Id', custName);
        fill('combpttrm', L.dueTerms, 'Id', 'TermsDescription');
        if ((L.dueTerms || []).length > 1) setVal('combpttrm', col(L.dueTerms[1], 'Id'));                  /* Rows[2].Activate() */
        fill('combdeliverytrm', L.deliveryTerms, 'Id', 'DeliveryTerm');
        if ((L.deliveryTerms || []).length) setVal('combdeliverytrm', col(L.deliveryTerms[0], 'Id'));
        fill('combCommType', L.commissionTypes, 'Id', 'CommissionType');
        if ((L.commissionTypes || []).length) setVal('combCommType', col(L.commissionTypes[0], 'Id'));
        fill('combcommUOM', L.commissionUoms, 'type', 'type');
        if ((L.commissionUoms || []).length) setVal('combcommUOM', col(L.commissionUoms[0], 'type'));
        fill('combcommAgent', L.customers, 'Id', custName);
        fill('CmbCommissionDebitAccount', L.commissionDebitAccounts, 'GlAccountId', 'AccountTitle');
        fill('CmbTransporterAc', L.accounts, acctKey, 'AccountTitle');
        fill('comWarehouse', L.warehouses, 'Id', whName);
        if ((L.warehouses || []).length) setVal('comWarehouse', col(L.warehouses[0], 'Id'));                 /* Rows[1].Activate() */
        fill('CmbReserveWarehouse', L.warehouses, 'Id', whName);
        fill('comCropYear', L.cropYears, 'Id', 'CropYear');
        fill('CmbJobLot', L.jobLots, 'Id', 'JobLotDescription');
        fill('comPackingType', L.packingTypes, 'Id', 'PackTypeDesc');
        fill('txtCity', L.cities, 'Id', 'CityName');
        fill('cmbOrderCategory2', L.otherCategories, 'Id', 'LookupName');
        fill('cmbCurrency', L.currencies, 'Id', 'CurrencyCode');
        categoryBind(); itemFill(); brandFill();
        refreshCombos();
    }
    function configurationDefault() {                                                                      /* ConfigurationDefault :1826 */
        if (cfg.defaultCropYearId) setVal('comCropYear', cfg.defaultCropYearId);
        if (cfg.defaultPackingTypeId) setVal('comPackingType', cfg.defaultPackingTypeId);
        if (cfg.defaultWarehouseId) setVal('comWarehouse', cfg.defaultWarehouseId);
        if (cfg.defaultCityId && int(val('txtCity')) === 0) setVal('txtCity', cfg.defaultCityId);
        if (cfg.defaultJobLotId && (L.jobLots || []).some(function (j) { return int(col(j, 'Id')) === cfg.defaultJobLotId; })) setVal('CmbJobLot', cfg.defaultJobLotId);
        if (cfg.baseCurrencyId) { if (int(val('cmbCurrency')) === 0) setVal('cmbCurrency', cfg.baseCurrencyId); }
        if (cfg.baseCurrencyRate !== null && cfg.baseCurrencyRate !== undefined && num(val('txtExchangeRate')) === 0) setVal('txtExchangeRate', rateF(cfg.baseCurrencyRate));
        var dr = !!cfg.commissionDebitToExpenses;
        show('CmbCommissionDebitAccount', dr); show('LabelCommissionDrCaption', dr);
        $id('txtCommissionRemarks').style.height = dr ? '51px' : '78px';
        var fr = int(cfg.outwardFreightAccountId) > 0;
        show('CmbTransporterAc', fr); show('lblTransporterAc', fr); show('txtFreightAmount', fr); show('lblFreightHdr', fr);
        $id('cmbCurrency').disabled = !isMulti(); $id('txtExchangeRate').disabled = !isMulti();
        refreshCombos();
    }
    function applyRights() {
        $id('btnSave').disabled = !perms.save;
        $id('btnUpdate').disabled = !perms.update;
        $id('btnPrint').disabled = !perms.print;
        $id('btnDelete').disabled = !perms.delete;
        ['btnSlip', 'btn294APrint', 'btn294BPrint'].forEach(function (x) { $id(x).disabled = !perms.print; });
    }
    function init() {
        bindEvents();
        setVal('DocDate', today()); setVal('duedate', today()); setVal('txtduedays', '0'); setVal('txtgpdate', today()); setVal('datStockDate', today());
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today());
        $id('ChkPrintslip').checked = false;
        http('GET', api + '/lookups').then(function (d) {
            L = d || {}; perms = L.rights || {}; cfg = L.configuration || cfg;
            setVal('txtdocno', L.docNo); setVal('txtBranchSrNo', L.branchSrNo);
            resetFr(); resetExp(); resetGl(); bindLists(); applyRights(); renderAll(); configurationDefault();
            setVal('txtLedgerBalance', '0'); setVal('txtBillAmount', '0'); setVal('txtLedgerBalanceAfterBill', '0');
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
            show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            dueTermChanged();
            if (cfg.itemsAgainstWarehouse) loadWarehouseItems();
            focus('DocDate'); say('');
        }, function (e) { say(e.message); box(e.message); });
    }

    // ------------------------------------------------------------------ entry-bar calculations

    function packEq() { var t = text('comPackUOM').trim(); return t === '' ? 0 : num(t); }          /* Cells[2] of the selected Pack UOM row */
    function uomEq(id) { var u = uoms.filter(function (x) { return int(col(x, 'Id')) === int(id); })[0]; return u ? num(col(u, 'Equivalent')) : 0; }
    function rateEq() { var id = int(val('comRateUOM')); return id ? uomEq(id) : 0; }

    /* TotalWeight() :5430 */
    function TotalWeight() {
        var p = int(val('comPackUOM')) !== 0 ? packEq() : 0, q = num(val('txtQty'));
        setVal('txtGrossWeight', fmt(p * q, 3, '0'));
    }
    /* Total() :5382 - the active control decides which of E.B Unit / E.B Total drives the other */
    function Total() {
        var q = num(val('txtQty')), gross = num(val('txtGrossWeight')), unit = num(val('txtEmptybagsUnit')), tot = num(val('txtEmptyBagsTotal'));
        var ae = document.activeElement ? document.activeElement.id : '';
        if (ae === 'txtEmptybagsUnit') { tot = unit * q; setVal('txtEmptyBagsTotal', fmt(tot, 3, '0')); }
        else { unit = q !== 0 ? tot / q : 0; if (!isFinite(unit)) unit = 0; setVal('txtEmptybagsUnit', fmt(unit, 4, '0')); }
        var cutTot = 0;
        if (val('txtwtcut').trim() !== '') { cutTot = q * num(val('txtwtcut')); setVal('txtWeightCutTotal', fmt(cutTot, 3, '0')); } else setVal('txtWeightCutTotal', '0');
        var addl = val('txtAddLss').trim();
        var weight = gross - tot - cutTot + (addl === '-' || addl === '' ? 0 : num(addl));
        var w = Math.round(weight * 100) / 100;
        setVal('txtNetBillWeight', fmt(w, 2, '0')); setVal('txtStockWeight', fmt(w, 2, '0'));
    }
    /* AmountCaluculation() :5496 */
    function AmountCaluculation() {
        var net = num(val('txtNetBillWeight')), ru = rateEq(), rate = num(val('txtRate')), cut = num(val('txtratecut'));
        if (net > 0 && ru > 0 && rate > 0) {
            var a = away(net / ru * rate, cfg.amountDecimals), c = away(net / ru * cut, cfg.amountDecimals);
            setVal('txtratecuttotal', amt(c)); setVal('txtAmount', amt(away(a - c, cfg.amountDecimals)));
        } else { setVal('txtratecuttotal', '0'); setVal('txtAmount', '0'); }
    }

    // ------------------------------------------------------------------ header totals (BillAmount :5195 and the proportions)

    function sum(rows, key) { return rows.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    function commType() { return text('combCommType').trim(); }
    /* TotalCommissionAmount() :5541 */
    function commissionAmount() {
        var t = commType(), rt = val('txtcommrate').trim(), rate = num(rt), uom = num(text('combcommUOM'));
        var totalItem = sum(dtGrid, 'ItemAmount'), totalNet = sum(dtGrid, 'NetBillWeight');
        if (t === 'Flat' && rt !== '') rate = away(rate, 0);
        var a = 0;
        if (rt !== '') {
            if (t === 'Flat') a = away(rate, cfg.amountDecimals);
            else if (t === 'Percent' || t === 'Percentage') a = away(totalItem * rate / 100, cfg.amountDecimals);
            else if (t === 'Comm Weight') a = away(totalNet / uom * rate, cfg.amountDecimals);
            if (!isFinite(a)) a = 0;
        }
        return { amount: a, rate: rate, type: t, has: rt !== '' };
    }
    function TotalCommissionAmount() {
        var c = commissionAmount();
        setVal('txtcommamount', c.has ? amt(c.amount) : '');
        return c;
    }
    function frGridTotal() { return dtFr.reduce(function (a, r) { return a + num(r.Freight); }, 0); }
    /* ExpProportion / CommissionProportion / FreightProportion / BillProportion + BillAmount + exchange rate */
    function recalc() {
        var amtDec = cfg.amountDecimals, c = TotalCommissionAmount();
        var totalNet = sum(dtGrid, 'NetBillWeight'), totalExp = 0, rateX = num(val('txtExchangeRate'));
        dtExp.forEach(function (e) {
            var q = num(e.Qty), r = num(e.Rate), a = num(e.Amount);
            totalExp += (q > 0 && r > 0) ? away(q * r, amtDec) : away(a, amtDec);
        });
        var fgt = frGridTotal(), credit = String(cfg.creditAmountInItemSaleGL === undefined || cfg.creditAmountInItemSaleGL === null ? '' : cfg.creditAmountInItemSaleGL).trim();
        dtGrid.forEach(function (r) {
            r.Expense = totalNet === 0 ? 0 : away(totalExp / totalNet * num(r.NetBillWeight), amtDec);
            r.Commission = c.type === 'Percent' ? num(r.ItemAmount) * c.rate / 100 : (totalNet === 0 ? 0 : c.amount / totalNet * num(r.NetBillWeight));
            if (!cfg.freightDebitToExpenses) r.Freight = fgt > 0 && totalNet !== 0 ? away(fgt / totalNet * num(r.NetBillWeight), amtDec) : 0;
            var item = away(r.ItemAmount, amtDec), com = away(r.Commission, amtDec), exp = away(r.Expense, amtDec);
            if (credit !== '') r.BillAmount = truthy(credit) ? away(item - com, amtDec) : item;
            else r.BillAmount = away(item + exp - com, amtDec);
            r.FcyAmount = rateX > 0 ? num(r.ItemAmount) / rateX : 0;
        });
        var itemSum = away(sum(dtGrid, 'ItemAmount'), amtDec), expSum = away(totalExp, amtDec);
        var jd = 0, jc = 0;
        dtGl.forEach(function (g) { if (int(g.AccountId) !== 0) { jd += num(g.Debit); jc += num(g.Credit); } });
        var freightText = int(cfg.outwardFreightAccountId) > 0 ? away(num(val('txtFreightAmount')), amtDec) : 0;
        var bill = itemSum + expSum - freightText;
        bill = bill + away(jc, amtDec) - away(jd, amtDec);
        if (int(val('comsupplier')) !== 0 && int(val('comsupplier')) === int(val('combcommAgent'))) bill -= c.amount;
        var billR = away(bill, amtDec);
        setVal('txtBillAmount', amt(billR));
        setVal('txtFcyAmount', rateX === 0 ? '0' : fmt(bill / rateX, 4, '0'));
        setVal('txtInvoiceQty', qtyF(sum(dtGrid, 'ItemQty'))); setVal('txtInvoiceWeight', qtyF(totalNet));
        setVal('txtLedgerBalanceAfterBill', fmt(away(ledgerBalance + billR, amtDec), amtDec, '0'));
        renderGrid(); renderFooters();
    }

    // ------------------------------------------------------------------ item / uom / stock

    /* PackUOM() :1442 */
    function PackUOM() {
        var item = int(val('CmbItemName')), keepPack = text('comPackUOM'), keepRate = text('comRateUOM');
        setVal('comPackUOM', ''); setVal('comRateUOM', '');
        if (!item) { uoms = []; fill('comPackUOM', [], 'Id', 'Equivalent'); fill('comRateUOM', [], 'Id', 'Equivalent'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/uoms?itemId=' + item).then(function (rows) {
            uoms = rows || [];
            fill('comPackUOM', uoms, 'Id', 'Equivalent'); fill('comRateUOM', uoms, 'Id', 'Equivalent');
            var hit = uoms.filter(function (x) { return String(col(x, 'Equivalent')) === keepPack; })[0];
            setVal('comPackUOM', hit ? col(hit, 'Id') : '');
            var h2 = uoms.filter(function (x) { return String(col(x, 'Equivalent')) === keepRate; })[0];
            setVal('comRateUOM', h2 ? col(h2, 'Id') : '');
            refreshCombos();
        }, function (e) { box(e.message); });
    }
    /* AvailableStockGetByItem() :1993 */
    function AvailableStockGetByItem() {
        var item = int(val('CmbItemName'));
        if (!item) { $id('lblBalance').textContent = '0'; return Promise.resolve(); }
        return http('GET', api + '/stock?itemId=' + item + '&docDate=' + encodeURIComponent(val('DocDate')) + '&warehouseId=' + int(val('comWarehouse'))
            + '&jobLotId=' + int(val('CmbJobLot')) + '&cropYear=' + encodeURIComponent(text('comCropYear').trim()) + '&packingTypeId=' + int(val('comPackingType'))
            + '&stockUom=' + int(val('comPackUOM'))).then(function (d) {
            var s = num(d && d.stock);
            $id('lblBalance').textContent = s > 0 ? String(s) : '0';
        }, function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ entry events

    function entryChanged() { Total(); AmountCaluculation(); recalc(); }
    function qtyChanged() { TotalWeight(); entryChanged(); }
    function packUomLeave() { TotalWeight(); Total(); AmountCaluculation(); recalc(); AvailableStockGetByItem(); }
    function itemLeave() { PackUOM().then(AvailableStockGetByItem); }
    function dueDateGenerate() {                                                                            /* DueDateGenerate */
        var t = val('txtduedays').trim(), d = val('DocDate') ? new Date(val('DocDate') + 'T00:00:00') : new Date();
        if (t !== '') d.setDate(d.getDate() + num(t));
        setVal('duedate', iso(d));
    }
    function dueTermChanged() { $id('txtduedays').disabled = false; dueDateGenerate(); }                    /* the due date follows the due days */
    function ledgerOf() {                                                                                   /* comsupplier_ValueChanged :5714 */
        setVal('txtLedgerBalance', '0'); ledgerBalance = 0;
        var c = int(val('comsupplier'));
        if (!c) { recalc(); return Promise.resolve(); }
        return http('GET', api + '/ledger?customerId=' + c + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (d) {
            ledgerBalance = num(d && d.balance);
            var r = bankers(ledgerBalance);
            setVal('txtLedgerBalance', r === 0 ? '0' : (r < 0 ? '(' + Math.abs(r).toLocaleString('en-US') + ')' : r.toLocaleString('en-US')));
            recalc();
        }, function (e) { box(e.message); recalc(); });
    }
    function currencyLeave() {                                                                              /* cmbCurrency_Leave :1905 */
        var c = int(val('cmbCurrency'));
        if (c === 0 || num(val('txtExchangeRate')) !== 0) return;
        if (c !== int(cfg.baseCurrencyId)) {
            http('GET', api + '/exchange-rate?currencyId=' + c).then(function (d) { setVal('txtExchangeRate', d && d.rate ? rateF(d.rate) : '0'); recalc(); }, function (e) { box(e.message); });
        } else { setVal('txtExchangeRate', rateF(cfg.baseCurrencyRate)); recalc(); }
    }

    function needCombo(x, m) { if (val(x) === '' || int(val(x)) === 0) { box(m); focus(x); return false; } return true; }
    function needText(x, m) { var t = val(x).trim(); if (t === '' || t === '0') { box(m); focus(x); return false; } return true; }
    /* FormValidationDetila() :3307 */
    function FormValidationDetila() {
        if (!needCombo('CmbItemName', 'Item Name Field is Required')) return false;
        if (!needCombo('comCropYear', 'Crop Year Field is Required')) return false;
        if (!needCombo('CmbJobLot', 'Job/Lot Field is Required')) return false;
        if (!needCombo('comPackingType', 'Packing Type Field is Required')) return false;
        if (!needText('txtQty', 'Qty Field is Required')) return false;
        if (!needCombo('comPackUOM', 'Pack Unit Field is Required')) return false;
        if (!needText('txtGrossWeight', 'Gross Weight Field is Required')) return false;
        if (!needText('txtNetBillWeight', 'Net Bill Weight Field is Required')) return false;
        if (!needText('txtStockWeight', 'Stock Weight Field is Required')) return false;
        if (!needText('txtRate', 'Rate Field is Required')) return false;
        if (!needCombo('comRateUOM', 'Rate UOM Field is Required')) return false;
        if (val('txtAmount').trim() === '' || val('txtAmount').trim() === '0') { box('Please Check Item Amount'); return false; }
        if (!needCombo('CmbReserveWarehouse', 'Reserve Warehouse Field is Required')) return false;
        if (!needCombo('comWarehouse', 'Warehouse Field is Required')) return false;
        if (!needCombo('txtCity', 'city Field is Required')) return false;
        return true;
    }
    function lotRow(id) { return (L.jobLots || []).filter(function (x) { return int(col(x, 'Id')) === int(id); })[0] || {}; }
    function whRow(id) { return (L.warehouses || []).filter(function (x) { return int(col(x, 'Id')) === int(id); })[0] || {}; }
    /* ItemIdExistsInJobLot :2458 */
    function itemInJobLot() {
        if (!cfg.saleCostingJobOrderWise) return true;
        var item = int(val('CmbItemName')), lot = int(val('CmbJobLot'));
        if (!item || !lot) return true;
        var ok = (L.jobLotItems || []).some(function (r) { return int(col(r, 'Id')) === lot && int(col(r, 'ItemId')) === item; });
        if (!ok) box("The selected item '" + text('CmbItemName') + "' does not exist against the job lot '" + text('CmbJobLot') + "'.");
        return ok;
    }
    function entryRow() {
        var itRow = ((cfg.itemsAgainstWarehouse ? whItems : L.items) || []).filter(function (x) { return int(col(x, 'Id')) === int(val('CmbItemName')); })[0] || {};
        var br = (L.brands || []).filter(function (x) { return int(col(x, 'Id')) === int(val('CmbBrandItemName')); })[0] || {};
        var wh = whRow(val('comWarehouse')), lot = lotRow(val('CmbJobLot'));
        var crop = text('comCropYear');
        var wcRaw = val('txtwtcut').trim();
        return {
            Id: 0, InvGdnId: 0, InvGdnDetailId: 0, GdnNo: '', ItemId: int(val('CmbItemName')), ItemCode: col(itRow, 'ItemCode'), Item: col(itRow, 'ItemName'),
            BrandItemId: int(val('CmbBrandItemName')), BrandItemCode: int(val('CmbBrandItemName')) > 0 ? col(br, 'ItemCode') : '', BrandItemName: int(val('CmbBrandItemName')) > 0 ? col(br, 'ItemName') : '',
            CropYear: crop, CropYearId: int(val('comCropYear')), JobLotId: int(val('CmbJobLot')), JobLot: text('CmbJobLot'), PackingTypeId: int(val('comPackingType')), PackingType: text('comPackingType'),
            PackUOMId: int(val('comPackUOM')), PackUOM: text('comPackUOM'), PackEquivalent: packEq(),
            ItemQty: num(val('txtQty')), GrossWeight: num(val('txtGrossWeight')), EmptyBags: num(val('txtEmptybagsUnit')), EmptyBagsTotal: num(val('txtEmptyBagsTotal')),
            WeightCut: num(val('txtwtcut')), WeightCutRaw: wcRaw, WeightCutTotal: num(val('txtWeightCutTotal')), AddLss: num(val('txtAddLss')),
            NetBillWeight: num(val('txtNetBillWeight')), StockWeight: num(val('txtStockWeight')), Rate: num(val('txtRate')), RateUOMId: int(val('comRateUOM')), RateUOM: text('comRateUOM'), RateEquivalent: rateEq(),
            RateCut: num(val('txtratecut')), RateCutTotal: num(val('txtratecuttotal')), ItemAmount: num(val('txtAmount')),
            WarehouseId: int(val('comWarehouse')), Warehouse: text('comWarehouse'), GpDate: val('txtgpdate') || today(), GpNo: int(val('txtgatepassno')), VehicleNo: val('txtvehicleno'),
            BillAmount: 0, Expense: 0, Journal: 0, Commission: 0, Freight: 0, FcyAmount: 0,
            ReserveWarehouseId: int(val('CmbReserveWarehouse')), ReserveWarehouse: text('CmbReserveWarehouse'), CityId: int(val('txtCity')), CityName: text('txtCity'),
            BranchId: int(col(wh, 'BranchId')), BranchName: col(wh, 'BranchName'), _lotBranchId: int(col(lot, 'BranchId')), _lotBranchName: col(lot, 'BranchName'), Dirty: true
        };
    }
    /* btnAdd_Click :2474 */
    function btnAdd_Click() {
        if (!itemInJobLot()) return;
        if (dtGrid.some(function (r) { return int(r.InvGdnId) > 0; })) { box('You can not add manual Record because record against Gdn exist in Grid'); return; }
        if (!FormValidationDetila()) return;
        var r = entryRow();
        if (cfg.branchFeature && !cfg.branchImplemented && r.BranchId !== r._lotBranchId) {
            box("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + r.BranchName + "' and JobLot is of Branch '" + r._lotBranchName + "'"); return;
        }
        dtGrid.push(r);
        recalc(); ResetDetail();
    }
    /* ResetDetail() :3549 */
    function ResetDetail() {
        ['comPackUOM', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight',
            'txtRate', 'comRateUOM', 'txtratecut', 'txtratecuttotal', 'txtAmount', 'CmbReserveWarehouse'].forEach(function (x) { setVal(x, ''); });
        if ($id('ChkResetOnSave').checked) OptionResetFields();
        refreshCombos(); focus('CmbItemName');
    }
    function OptionResetFields() {                                                                          /* OptionResetFields */
        ['CmbItemName', 'comCropYear', 'CmbJobLot', 'comPackingType', 'comWarehouse', 'CmbReserveWarehouse', 'txtgatepassno', 'txtvehicleno', 'txtCity'].forEach(function (x) { setVal(x, ''); });
        refreshCombos(); focus('CmbItemName');
    }
    var ENTRY_LOCK = ['comWarehouse', 'CmbCategory', 'CmbItemName', 'comCropYear', 'comPackUOM', 'comPackingType', 'CmbJobLot', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtAddLss'];
    function lockFields(lock) {                                                                              /* DisableFields / EnablesFields */
        ENTRY_LOCK.forEach(function (x) { var e = $id(x); if (e) e.disabled = lock; });
        refreshCombos();
    }
    function cropIdOf(t) { return (L.cropYears || []).filter(function (c) { return String(col(c, 'CropYear')) === String(t); }).map(function (c) { return col(c, 'Id'); })[0] || ''; }
    /* grd_DoubleClick :2531 */
    function grdDoubleClick(i) {
        var r = dtGrid[i]; if (!r) return;
        setVal('CmbCategory', ''); updateIndex = i;
        lockFields(int(r.InvGdnId) > 0);
        setVal('comWarehouse', r.WarehouseId);
        loadWarehouseItems().then(function () {
            itemFill(); setVal('CmbItemName', r.ItemId); setVal('CmbBrandItemName', r.BrandItemId || '');
            return PackUOM();
        }).then(function () {
            setVal('comCropYear', cropIdOf(r.CropYear)); setVal('CmbJobLot', r.JobLotId); setVal('comPackingType', r.PackingTypeId);
            setVal('txtQty', fmt(r.ItemQty, 2, '0')); setVal('comPackUOM', r.PackUOMId);
            setVal('txtGrossWeight', fmt(r.GrossWeight, 2, '0')); setVal('txtEmptybagsUnit', fmt(r.EmptyBags, 2, '0')); setVal('txtEmptyBagsTotal', fmt(r.EmptyBagsTotal, 2, '0'));
            setVal('txtwtcut', fmt(r.WeightCut, 2, '0')); setVal('txtWeightCutTotal', fmt(r.WeightCutTotal, 2, '0')); setVal('txtAddLss', fmt(r.AddLss, 2, '0'));
            setVal('txtNetBillWeight', fmt(r.NetBillWeight, 2, '0')); setVal('txtStockWeight', fmt(r.StockWeight, 2, '0'));
            setVal('txtRate', rateF(r.Rate)); if (int(r.RateUOMId) > 0) setVal('comRateUOM', r.RateUOMId);
            setVal('txtratecut', rateF(r.RateCut)); setVal('txtratecuttotal', amt(r.RateCutTotal)); setVal('txtAmount', amt(r.ItemAmount));
            setVal('txtgpdate', dateOnly(r.GpDate) || today()); setVal('txtgatepassno', r.GpNo); setVal('txtvehicleno', r.VehicleNo);
            setVal('CmbReserveWarehouse', r.ReserveWarehouseId || ''); setVal('txtCity', r.CityId || '');
            refreshCombos();
            show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        });
    }
    /* btnUpdateDetail_Click :2589 */
    function btnUpdateDetail_Click() {
        if (!itemInJobLot()) return;
        if (!FormValidationDetila()) return;
        var r = dtGrid[updateIndex]; if (!r) return;
        var n = entryRow();
        n.Id = r.Id; n.InvGdnId = r.InvGdnId; n.InvGdnDetailId = r.InvGdnDetailId; n.GdnNo = r.GdnNo; n.Freight = r.Freight;
        delete n._lotBranchId; delete n._lotBranchName;
        if (int(r.InvGdnId) > 0) { n.BranchId = r.BranchId; n.BranchName = r.BranchName; }
        dtGrid[updateIndex] = n; updateIndex = -1;
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        lockFields(false);
        recalc(); ResetDetail();
    }
    function btnCancelUpdateDetial_Click() {
        updateIndex = -1; show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        lockFields(false); ResetDetail();
    }
    function gridDelete(i) {                                                                                 /* grd_ColumnButtonClick */
        if (!window.confirm('Are you sure to Delete?')) return;
        dtGrid.splice(i, 1); recalc();
    }

    // ------------------------------------------------------------------ grids

    function gcols() {
        var c = [['GdnNo', 'GdnNo', 't'], ['ItemCode', 'ItemCode', 't'], ['Item', 'Item', 't'], ['BrandItemCode', 'BrandCode', 't'], ['BrandItemName', 'BrandName', 't'], ['CropYear', 'CropYear', 't'],
            ['JobLot', 'JobLot', 't'], ['PackingType', 'PackingType', 't'], ['PackUOM', 'PackUOM', 'q'], ['ItemQty', 'ItemQty', 'q', 1], ['GrossWeight', 'GrossWeight', 'q', 1], ['EmptyBags', 'EmptyBags', 'q'],
            ['EmptyBagsTotal', 'EmptyBagsTotal', 'q', 1], ['WeightCut', 'WeightCut', 'q'], ['WeightCutTotal', 'WeightCutTotal', 'q', 1], ['AddLss', 'AddLss', 'q'], ['NetBillWeight', 'NetBillWeight', 'q', 1],
            ['StockWeight', 'StockWeight', 'q', 1], ['Rate', 'Rate', 'r'], ['RateUOM', 'RateUOM', 'q'], ['RateCut', 'RateCut', 'r'], ['RateCutTotal', 'RateCutTotal', 'a', 1], ['ItemAmount', 'ItemAmount', 'a', 1]];
        if (cfg.multiCurrency) c.push(['FcyAmount', 'FcyAmount', 'f', 1]);
        c.push(['Warehouse', 'Warehouse', 't'], ['GpDate', 'GpDate', 'd'], ['GpNo', 'GpNo', 'i'], ['VehicleNo', 'VehicleNo', 't'], ['BillAmount', 'Item Net Amount', 'a', 1], ['Expense', 'Expense', 'a', 1],
            ['Journal', 'Journal', 'a', 1], ['Commission', 'Commission', 'a', 1], ['Freight', 'Freight', 'a', 1], ['ReserveWarehouse', 'ReserveWarehouse', 't'], ['CityName', 'CityName', 't']);
        if (cfg.branchFeature && !cfg.branchImplemented) c.push(['BranchName', 'BranchName', 't']);
        return c;
    }
    function fmtCell(kind, v) {
        if (kind === 'a') return amt(v);
        if (kind === 'f') return fmt(v, 4, '0');
        if (kind === 'r') return rateF(v);
        if (kind === 'q') return qtyF(v);
        if (kind === 'd') return shortDate(v);
        if (kind === 'i') return esc(v === 0 || v === '' ? '0' : v);
        return esc(v);
    }
    function renderGrid() {
        var gc = gcols();
        $id('grdHead').innerHTML = '<th style="width:20px">X</th>' + gc.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('');
        $id('grdBody').innerHTML = dtGrid.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="ctr"><button type="button" class="bt" data-del="' + i + '" style="width:20px;height:17px;">X</button></td>'
                + gc.map(function (c) { var n = c[2] !== 't' && c[2] !== 'd'; return '<td class="' + (n ? 'n' : '') + '">' + fmtCell(c[2], r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = '<td></td>' + gc.map(function (c) {
            if (!c[3]) return '<td></td>';
            var t = sum(dtGrid, c[0]);
            return '<td class="n">' + (c[2] === 'a' ? amt(t) : (c[2] === 'f' ? fmt(t, 4, '0') : qtyF(t))) + '</td>';
        }).join('');
    }
    function accountOptions(selected) {
        var k = acctKey();
        return '<option value=""></option>' + (L.accounts || []).map(function (r) {
            var v = String(col(r, k));
            return '<option value="' + esc(v) + '"' + (String(selected) === v && v !== '' && v !== '0' ? ' selected' : '') + '>' + esc(col(r, 'AccountTitle')) + '</option>';
        }).join('');
    }
    function nz(v) { return String(v) === '' ? '' : (num(v) === 0 ? '0' : fmt(v, 4)); }
    function resetFr() { dtFr = [{ InvGdnId: 0, Transporter: 0, Freight: 0, Debit: 0, Remarks: '' }]; }         /* AddRowInFreightGrid */
    function resetExp() { dtExp = [{ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }]; }                     /* AddRowInvExpGrid */
    function resetGl() { dtGl = [{ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }]; }   /* AddRowInGLGrid */
    function renderFr() {
        var dr = !!cfg.freightDebitToExpenses;
        $id('frHead').innerHTML = '<th style="width:56px">Delete</th><th style="width:48px">Add New</th><th style="width:250px">Transporter</th><th>Freight</th>' + (dr ? '<th>Debit</th>' : '') + '<th>Remarks</th>';
        $id('frBody').innerHTML = dtFr.map(function (r, i) {
            return '<tr><td class="ctr"><button type="button" class="bt" data-fdel="' + i + '" style="height:17px;">Delete</button></td><td class="ctr"><button type="button" class="bt" data-fadd="' + i + '" style="height:17px;">Add</button></td>'
                + '<td><select class="ct" data-fr="' + i + '" data-f="Transporter" style="width:100%;height:17px;font-size:12px;">' + accountOptions(r.Transporter) + '</select></td>'
                + '<td><input class="ce" data-fr="' + i + '" data-f="Freight" value="' + esc(num(r.Freight) ? amt(r.Freight) : '0') + '"/></td>'
                + (dr ? '<td><input class="ce" data-fr="' + i + '" data-f="Debit" value="' + esc(num(r.Debit) ? amt(r.Debit) : '0') + '"/></td>' : '')
                + '<td><input class="ct" data-fr="' + i + '" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td></tr>';
        }).join('');
    }
    function renderExp() {
        $id('expHead').innerHTML = '<th style="width:56px">Delete</th><th style="width:48px">Add New</th><th style="width:250px">Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th>';
        var opts = '<option value=""></option>' + (L.otherItems || []).map(function (r) { return '<option value="' + esc(col(r, 'Id')) + '">' + esc(col(r, 'OtherItemName')) + '</option>'; }).join('');
        $id('expBody').innerHTML = dtExp.map(function (r, i) {
            return '<tr><td class="ctr"><button type="button" class="bt" data-edel="' + i + '" style="height:17px;">Delete</button></td><td class="ctr"><button type="button" class="bt" data-eadd="' + i + '" style="height:17px;">Add</button></td>'
                + '<td><select class="ct" data-e="' + i + '" data-f="ItemId" style="width:100%;height:17px;font-size:12px;">' + opts.replace('value="' + r.ItemId + '"', 'value="' + r.ItemId + '" selected') + '</select></td>'
                + '<td><input class="ce" data-e="' + i + '" data-f="Qty" value="' + esc(r.Qty ? qtyF(r.Qty) : (r.Qty === '' ? '' : '0')) + '"/></td>'
                + '<td><input class="ce" data-e="' + i + '" data-f="Rate" value="' + esc(r.Rate === '' ? '' : (r.Rate ? rateF(r.Rate) : '0')) + '"/></td>'
                + '<td><input class="ce" data-e="' + i + '" data-f="Amount" value="' + esc(r.Amount ? amt(r.Amount) : '0') + '"/></td>'
                + '<td><input class="ct" data-e="' + i + '" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td></tr>';
        }).join('');
    }
    function renderGl() {
        $id('glHead').innerHTML = '<th style="width:56px">Delete</th><th style="width:48px">Add New</th><th style="width:250px">Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th>';
        $id('glBody').innerHTML = dtGl.map(function (r, i) {
            return '<tr><td class="ctr"><button type="button" class="bt" data-gdel="' + i + '" style="height:17px;">Delete</button></td><td class="ctr"><button type="button" class="bt" data-gadd="' + i + '" style="height:17px;">Add</button></td>'
                + '<td><select class="ct" data-g="' + i + '" data-f="AccountId" style="width:100%;height:17px;font-size:12px;">' + accountOptions(r.AccountId) + '</select></td>'
                + '<td><input class="ct" data-g="' + i + '" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td>'
                + ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].map(function (f) { return '<td><input class="ce" data-g="' + i + '" data-f="' + f + '" value="' + esc(num(r[f]) ? (f === 'Rate' ? rateF(r[f]) : (f === 'Debit' || f === 'Credit' ? amt(r[f]) : qtyF(r[f]))) : '0') + '"/></td>'; }).join('')
                + '</tr>';
        }).join('');
    }
    function renderFooters() {
        $id('frFoot').innerHTML = '<td></td><td></td><td></td><td class="n">' + amt(frGridTotal()) + '</td>' + (cfg.freightDebitToExpenses ? '<td class="n">' + amt(sum(dtFr, 'Debit')) + '</td>' : '') + '<td></td>';
        $id('expFoot').innerHTML = '<td></td><td></td><td></td><td class="n">' + qtyF(sum(dtExp, 'Qty')) + '</td><td></td><td class="n">' + amt(sum(dtExp, 'Amount')) + '</td><td></td>';
        $id('glFoot').innerHTML = '<td></td><td></td><td></td><td></td><td></td><td class="n">' + qtyF(sum(dtGl, 'Qty')) + '</td><td></td><td class="n">' + amt(sum(dtGl, 'Debit')) + '</td><td class="n">' + amt(sum(dtGl, 'Credit')) + '</td>';
    }
    function renderAll() { renderGrid(); renderFr(); renderExp(); renderGl(); renderFooters(); }
    function gridTab(t) {
        gtab = t;
        for (var i = 0; i < 4; i++) { show('gpage' + i, i === t); $id('gtab' + i).className = i === t ? 'on' : ''; }
    }

    /* grdInvExp_CellUpdated :2144 */
    function expCell(i, f, v) {
        var r = dtExp[i]; if (!r) return;
        if (f === 'Remarks') { r.Remarks = v; return; }
        if (f === 'ItemId') { r.ItemId = int(v); recalc(); return; }
        if (v.trim() !== '' && isNaN(num(v))) { box('Please Type Only Numeric Value'); renderExp(); return; }
        r[f] = v.trim() === '' ? '' : num(v);
        if (f === 'Qty' || f === 'Rate') { if (String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = away(num(r.Qty) * num(r.Rate), cfg.amountDecimals); }
        else if (f === 'Amount') r.Amount = away(num(r.Amount), cfg.amountDecimals);
        recalc(); renderExp();
    }
    /* grdFreight_CellUpdated :3090 */
    function frCell(i, f, v) {
        var r = dtFr[i]; if (!r) return;
        if (f === 'Remarks') { r.Remarks = v; return; }
        if (f === 'Transporter') { r.Transporter = int(v); recalc(); return; }
        if (v.trim() !== '' && isNaN(num(v))) { box('Please Type Only Numeric Value'); renderFr(); return; }
        r[f] = away(num(v), cfg.amountDecimals);
        if (f === 'Freight' && num(r.Debit) > 0) { r.Freight = 0; box('Debit Side is aleady added'); }
        if (f === 'Debit' && num(r.Freight) > 0) { r.Debit = 0; box('Credit Side is aleady added'); }
        recalc(); renderFr();
    }
    /* grdGLedger_CellUpdated :2303 */
    function glCell(i, f, v) {
        var r = dtGl[i]; if (!r) return;
        if (f === 'Remarks') { r.Remarks = v; return; }
        if (f === 'AccountId') {
            r.AccountId = int(v);
            if (!cfg.subsidiary) {
                var cu = (L.customers || []).filter(function (c) { return int(col(c, 'Id')) === int(val('comsupplier')); })[0];
                if (cu && r.AccountId !== 0 && int(col(cu, 'GlAccountId')) === r.AccountId) { box('Customer Account Not select'); r.AccountId = 0; renderGl(); }
            }
            recalc(); return;
        }
        if (v.trim() !== '' && isNaN(num(v))) { box('Please Type Only Numeric Value'); renderGl(); return; }
        r[f] = num(v);
        if ((f === 'Qty' || f === 'Rate') && num(r.Qty) !== 0 && num(r.Rate) !== 0) { r.Credit = away(num(r.Qty) * num(r.Rate), cfg.amountDecimals); r.Debit = 0; r.Percentage = 0; }
        if (f === 'Percentage' && v.trim() !== '') {
            var t = sum(dtGrid, 'ItemAmount') / 100 * num(r.Percentage);
            if (t > 0) { r.Credit = away(t, cfg.amountDecimals); r.Debit = 0; } else { r.Debit = Math.abs(away(t, cfg.amountDecimals)); r.Credit = 0; }
            r.Qty = 0; r.Rate = 0;
        }
        if (f === 'Credit') { r.Credit = away(r.Credit, cfg.amountDecimals); if (num(r.Debit) > 0) { r.Credit = 0; box('Debit Side is aleady added'); } }
        if (f === 'Debit') { r.Debit = away(r.Debit, cfg.amountDecimals); if (num(r.Credit) > 0) { r.Debit = 0; box('Credit Side is aleady added'); } }
        recalc(); renderGl();
    }

    // ------------------------------------------------------------------ save (Insert :3664)

    function collect(autoUpdate) {
        var h = {
            Id: Id, AutoUpdate: !!autoUpdate, DocDate: val('DocDate'), SupplierCustomerId: int(val('comsupplier')), ManualBillNo: val('txtbillno').trim(), SupplierReferenceNo: val('txtSupplierReference').trim(),
            OtherCategoryId: int(val('cmbOrderCategory2')), PaymentTermId: int(val('combpttrm')), DueDays: val('txtduedays'), DueDate: val('duedate'), DeliveryTerm: text('combdeliverytrm'),
            CurrencyId: int(val('cmbCurrency')), ExchangeRate: num(val('txtExchangeRate')), CustomAccounts: $id('chkCustomAccounts').checked,
            StockDate: STOCKDATE_VISIBLE ? val('datStockDate') : '',
            TransporterId: int(val('CmbTransporterAc')), FreightAmount: num(val('txtFreightAmount')),
            CommissionAgentId: int(val('combcommAgent')), CommissionType: commType(), CommRate: val('txtcommrate').trim(), UomScheduleIdCmRate: text('combcommUOM'),
            CommissionRemarks: val('txtCommissionRemarks').trim(), CommissionDebitAccountId: int(val('CmbCommissionDebitAccount')), RemarksHeader: val('txtremarks').trim()
        };
        h.details = dtGrid.map(function (r) {
            return { Id: r.Id, Dirty: !!r.Dirty, ItemId: r.ItemId, BrandItemId: r.BrandItemId, CropYear: r.CropYear, CropYearId: r.CropYearId || int(cropIdOf(r.CropYear)), JobLotId: r.JobLotId, PackingTypeId: r.PackingTypeId,
                PackUOMId: r.PackUOMId, ItemQty: r.ItemQty, GrossWeight: r.GrossWeight, EmptyBags: r.EmptyBags, EmptyBagsTotal: r.EmptyBagsTotal, EbMode: 'total',
                WeightCut: r.WeightCutRaw === '' ? '' : r.WeightCut, AddLss: r.AddLss, RateUOMId: r.RateUOMId, Rate: r.Rate, RateCut: r.RateCut, WarehouseId: r.WarehouseId,
                ReserveWarehouseId: r.ReserveWarehouseId, CityId: r.CityId, GpDate: dateOnly(r.GpDate), GpNo: r.GpNo, VehicleNo: r.VehicleNo };
        });
        h.expenses = dtExp.filter(function (r) { return int(r.ItemId) > 0 || num(r.Amount) > 0 || num(r.Qty) > 0; }).map(function (r) {
            return { ItemId: int(r.ItemId), Remarks: r.Remarks, Qty: String(r.Qty) === '' ? 0 : num(r.Qty), Rate: String(r.Rate) === '' ? 0 : num(r.Rate), Amount: num(r.Amount) };
        });
        h.freights = dtFr.filter(function (r) { return int(r.Transporter) > 0 || num(r.Freight) > 0 || num(r.Debit) > 0; }).map(function (r) {
            return { InvGdnId: r.InvGdnId || 0, Transporter: int(r.Transporter), Freight: num(r.Freight), Debit: num(r.Debit), Remarks: r.Remarks };
        });
        h.journals = dtGl.filter(function (r) { return int(r.AccountId) > 0 || num(r.Debit) > 0 || num(r.Credit) > 0; }).map(function (r) {
            return { AccountId: int(r.AccountId), Remarks: r.Remarks, Percentage: num(r.Percentage), Qty: num(r.Qty), Rate: num(r.Rate), Debit: num(r.Debit), Credit: num(r.Credit) };
        });
        return h;
    }
    function openWages(refDocId, gross, onClosed) {
        var ov = document.createElement('div'); ov.id = 'wagesOverlay';
        var fr = document.createElement('iframe'); fr.title = 'Contractor Wages Bill';
        fr.src = '/production/wages-bill?refDocTypeId=' + DOC + '&refDocId=' + encodeURIComponent(refDocId) + '&grossWeightTotal=' + encodeURIComponent(gross);
        var close = document.createElement('button'); close.type = 'button'; close.textContent = 'Close';
        ov.appendChild(fr); ov.appendChild(close); document.body.appendChild(ov);
        var done = function () { if (ov.parentNode) ov.parentNode.removeChild(ov); window.P280WagesClosed = null; onClosed(); };
        close.addEventListener('click', done);
        window.P280WagesClosed = done;
    }
    function Insert() {
        if (busy) return;
        if (dtGrid.length === 0) { box('Grid Record Not Found'); return; }
        if (int(val('comsupplier')) === 0) { box('CustomerName Field is Required'); focus('comsupplier'); return; }
        var dn = val('txtdocno').trim();
        if (dn === '' || dn === '0') { box('DocNo Field is Required'); focus('txtdocno'); return; }
        if (val('txtduedays').trim() === '') { box('Due Days Field is Required'); focus('txtduedays'); return; }
        if (!window.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < dtExp.length; i++)
            if (num(dtExp[i].Amount) > 0 && int(dtExp[i].ItemId) === 0) { box('Please Select an Item Against Expense First'); return; }
        var pr = { v: $id('ChkBok').checked, s: $id('ChkPrintslip').checked, a: $id('chkPrintSlip294A').checked, b: $id('chkPrint294B').checked };
        busy = true;
        return http('POST', api + '/save', collect(false)).then(function (d) {
            busy = false;
            box(d.message);
            var after = function () {
                Reset();
                if (pr.v) VoucherReport_103(d.voucherHeadId);
                if (pr.s) printSlip(294, d.id);
                if (pr.a) printSlip('294A', d.id);
                if (pr.b) printSlip('294B', d.id);
            };
            if (d.openWages) openWages(d.id, num(d.grossWeightTotal), after); else after();
        }, function (e) { busy = false; box(e.message); });
    }
    function btnSave_Click() { Id = 0; return Insert(); }
    function btnUpdate_Click() {
        if (Approved) { box('Record Not Update because Record has approved'); return; }
        return Insert();
    }
    function btnDelete_Click() {
        if (Approved) { box('Record Not Delete because Record has approved'); return; }
        if (!Id) { box('Record Not Found'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        http('DELETE', api + '/' + Id).then(function (d) { box(d.message); Reset(); }, function (e) { box(e.message); });
    }

    /* Reset() :3461 */
    function Reset(skipNumbers) {
        Id = 0; VoucherHeadId = 0; Approved = false; updateIndex = -1; ledgerBalance = 0;
        ['comsupplier', 'txtSupplierReference', 'txtbillno', 'combcommAgent', 'txtExchangeRate', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks', 'CmbTransporterAc', 'txtFreightAmount',
            'CmbCommissionDebitAccount', 'txtInvoiceQty', 'txtInvoiceWeight', 'CmbItemName', 'comPackUOM', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal',
            'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtRate', 'comRateUOM', 'txtratecut', 'txtratecuttotal', 'txtAmount', 'txtFcyAmount', 'CmbReserveWarehouse', 'combCommType', 'combcommUOM'].forEach(function (x) { setVal(x, ''); });
        setVal('txtduedays', '0'); setVal('DocDate', today()); setVal('datStockDate', today()); setVal('txtgpdate', today()); setVal('txtgatepassno', ''); setVal('txtvehicleno', '');
        $id('chkCustomAccounts').checked = false; $id('lblBalance').textContent = '0';
        dtGrid = []; resetExp(); resetFr(); resetGl();
        if ($id('ChkResetOnSave').checked) OptionResetFields();
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        lockFields(false);
        configurationDefault();
        setVal('txtLedgerBalance', '0'); setVal('txtBillAmount', '0'); setVal('txtLedgerBalanceAfterBill', '0');
        dueTermChanged(); renderAll(); refreshCombos();
        if (!skipNumbers) http('GET', api + '/numbers').then(function (n) { setVal('txtdocno', n.docNo); setVal('txtBranchSrNo', n.branchSrNo); });
        focus('DocDate');
    }
    function btnNew_Click() { Reset(); }

    // ------------------------------------------------------------------ read (ReadById :4128)

    function mapDetail(r) {
        var pe = num(col(r, 'PackEquivalent')), re = num(col(r, 'RateUOM'));
        return {
            Id: int(col(r, 'Id')), InvGdnId: int(col(r, 'InvGdnId')), InvGdnDetailId: int(col(r, 'InvGdnDetailId')), GdnNo: col(r, 'GdnNo'), ItemId: int(col(r, 'ItemId')), ItemCode: col(r, 'ItemCode'), Item: col(r, 'ItemName'),
            BrandItemId: int(col(r, 'BrandItemId')), BrandItemCode: col(r, 'BrandItemCode'), BrandItemName: col(r, 'BrandItemName'), CropYear: col(r, 'CropYear'), CropYearId: 0,
            JobLotId: int(col(r, 'JobLotId')), JobLot: col(r, 'JobLotDescription'), PackingTypeId: int(col(r, 'PackingTypeId')), PackingType: col(r, 'PackTypeDesc'),
            PackUOMId: int(col(r, 'ItemUOMId')), PackUOM: pe, PackEquivalent: pe, ItemQty: num(col(r, 'ItemQty')), GrossWeight: num(col(r, 'GrossWeight')), EmptyBags: num(col(r, 'EBWeight')),
            EmptyBagsTotal: bankers(col(r, 'EBTotalWt')), WeightCut: num(col(r, 'WeightCut')), WeightCutRaw: String(col(r, 'WeightCut')), WeightCutTotal: bankers(col(r, 'WeightCutTotal')), AddLss: num(col(r, 'AdLsWeight')),
            NetBillWeight: num(col(r, 'NetBillWeight')), StockWeight: num(col(r, 'NetStockWeight')), Rate: num(col(r, 'ItemRate')), RateUOMId: int(col(r, 'UomScheduleIdRate')), RateUOM: re, RateEquivalent: re,
            RateCut: num(col(r, 'RateCut')), RateCutTotal: num(col(r, 'RateCutAmount')), ItemAmount: num(col(r, 'ItemAmount')), WarehouseId: int(col(r, 'WarehouseId')), Warehouse: first(r, ['WareHouseName', 'Warehouse']),
            GpDate: col(r, 'GpDate'), GpNo: int(col(r, 'GpNo')), VehicleNo: col(r, 'VehicleNo'), BillAmount: num(col(r, 'BillAmount')), Expense: num(col(r, 'ExpenseAmount')), Journal: num(col(r, 'JournalAmount')),
            Commission: num(col(r, 'CommissionAmount')), Freight: num(col(r, 'FreightAmount')), FcyAmount: num(col(r, 'FcyAmount')),
            ReserveWarehouseId: int(col(r, 'ReserveWareHouse')), ReserveWarehouse: col(r, 'ReserveWareHouseName'), CityId: int(col(r, 'CityId')), CityName: col(r, 'CityName'),
            BranchId: int(col(r, 'BranchId')), BranchName: col(r, 'BranchName'), Dirty: false
        };
    }
    function ReadById(id) {
        return http('GET', api + '/' + id).then(function (m) {
            Reset(true);
            Id = int(id);
            setVal('txtdocno', col(m, 'DocNo')); setVal('txtBranchSrNo', col(m, 'BranchSrNo')); setVal('DocDate', dateOnly(col(m, 'DocDate')));
            showTab(0);
            setVal('comsupplier', col(m, 'SupplierCustomerId'));
            var tp = int(col(m, 'TransporterId'));
            if (tp > 0) setVal('CmbTransporterAc', cfg.subsidiary ? col(m, 'TransporterCreditPartyId') : tp);
            setVal('txtFreightAmount', String(col(m, 'FreightAmount')) === '' ? '' : amt(col(m, 'FreightAmount')));
            setVal('txtSupplierReference', col(m, 'SupplierReferenceNo')); setVal('txtbillno', col(m, 'ManualBillNo'));
            setVal('combcommAgent', int(col(m, 'CommissionAgentId')) || '');
            var cm = (m.commissions || [])[0];
            if (cm) setVal('CmbCommissionDebitAccount', col(cm, 'DebitAccountId'));
            var ct = (L.commissionTypes || []).filter(function (x) { return String(col(x, 'CommissionType')) === String(col(m, 'CommissionType')); })[0];
            setVal('combCommType', ct ? col(ct, 'Id') : '');
            setVal('txtcommrate', col(m, 'CommRate') === '' ? '' : rateF(col(m, 'CommRate')));
            var cu = (L.commissionUoms || []).filter(function (x) { return String(col(x, 'type')) === String(col(m, 'UomScheduleIdCmRate')); })[0];
            setVal('combcommUOM', cu ? col(cu, 'type') : '');
            setVal('txtcommamount', amt(col(m, 'CommAmount'))); setVal('txtCommissionRemarks', col(m, 'CommissionRemarks'));
            setVal('txtremarks', col(m, 'OtherRemarks')); setVal('txtBillAmount', amt(col(m, 'BillAmount')));
            if (int(col(m, 'PaymentTermId')) > 0) setVal('combpttrm', col(m, 'PaymentTermId'));
            setVal('txtduedays', col(m, 'SupplierInvoiceNo')); setVal('duedate', dateOnly(col(m, 'SupplierInvoiceDate'))); setVal('datStockDate', dateOnly(col(m, 'SupplierInvoiceDate')));
            var dt = String(col(m, 'DeliveryTerm') || '');
            if (dt !== '') { var dh = (L.deliveryTerms || []).filter(function (x) { return String(col(x, 'DeliveryTerm')) === dt; })[0]; if (dh) setVal('combdeliverytrm', col(dh, 'Id')); }
            Approved = !!col(m, 'IsApproved');
            VoucherHeadId = int(col(m, 'VoucherHeadId'));
            setVal('cmbCurrency', int(col(m, 'CurrencyId')) || '');
            if (int(col(m, 'CurrencyId')) > 0) setVal('txtExchangeRate', col(m, 'ExchangeRate')); else configurationDefault();
            $id('chkCustomAccounts').checked = !!col(m, 'CustomAccounts');
            setVal('cmbOrderCategory2', int(col(m, 'OtherCategoryId')) || '');
            dtGrid = (m.details || []).map(mapDetail);
            lockFields(dtGrid.some(function (r) { return int(r.InvGdnId) > 0; }));
            dtExp = (m.expenses || []).map(function (r) { return { ItemId: int(col(r, 'InvRevExpItemId')), Qty: num(col(r, 'Qty')), Rate: num(col(r, 'Rate')), Amount: num(col(r, 'Amount')), Remarks: col(r, 'CustomRemarks') }; });
            if (dtExp.length === 0) resetExp();
            dtGl = (m.journals || []).map(function (r) {
                return { AccountId: cfg.subsidiary ? int(col(r, 'TransporterSupCustId')) : int(col(r, 'ChartofAccountId')), Remarks: first(r, ['JvRemarks', 'Remarks']), Percentage: num(col(r, 'JvPrcnt')),
                    Qty: num(col(r, 'JvQty')), Rate: num(col(r, 'JvRate')), Debit: num(col(r, 'JvDebit')), Credit: num(col(r, 'JvCredit')) };
            });
            if (dtGl.length === 0) resetGl();
            dtFr = (m.freights || []).map(function (r) {
                return { InvGdnId: int(col(r, 'InvGdnId')), Transporter: cfg.subsidiary ? int(col(r, 'TransporterSupCustId')) : int(col(r, 'TansporterId')), Freight: num(col(r, 'FreightAmount')), Debit: num(col(r, 'Debit')), Remarks: col(r, 'Remarks') };
            });
            if (dtFr.length === 0) resetFr();
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            renderAll(); refreshCombos();
            return ledgerOf().then(function () { recalc(); renderAll(); });
        }, function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ records update (btnRecordsUpdate_Click :4310)

    function recordsUpdate() {
        if (busy) return;
        busy = true;
        http('GET', api + '/auto-update-ids').then(function (d) {
            var ids = (d && d.ids) || [], n = 0;
            var step = function () {
                if (n >= ids.length) { busy = false; say(''); box('Records Update Successfully'); Reset(); return; }
                var id = ids[n++];
                say('Updating ' + n + ' of ' + ids.length + ' ...');
                busy = false;
                ReadById(id).then(function () {
                    if (Id !== id) throw new Error('Record ' + id + ' could not be read');
                    busy = true;
                    return http('POST', api + '/save', collect(true));
                }).then(step, function (e) { busy = false; say(''); box(e.message); });
            };
            step();
        }, function (e) { busy = false; box(e.message); });
    }

    // ------------------------------------------------------------------ history (GetAll :4470)

    var HIST_COLS = ['DocNo', 'DocDate', 'BranchSrNo', 'BranchName', 'CustomerName', 'ManualBillNo', 'DueDate', 'CommAgent', 'CommType', 'CommRate', 'CommAmount', 'CommRemarks', 'BillAmount', 'OtherCategory',
        'EntryDate', 'EntryUser', 'ModifyUser', 'ModifyDate', 'ApprovedUser', 'ApprovedDate', 'NoOfAttachments', 'Remarks'];
    var DATE_COLS = { DocDate: 1, DueDate: 1, EntryDate: 1, ModifyDate: 1, ApprovedDate: 1 };
    var NUM_COLS = { CommRate: 1, CommAmount: 1, BillAmount: 1 };
    function dateMode() {
        if ($id('rdentrydate').checked) return 'entry';
        if ($id('rdmodifydate').checked) return 'modify';
        if ($id('rdapproveddate').checked) return 'approved';
        return 'doc';
    }
    function branchIds() { return histBranches.filter(function (b) { return histBranchSel[col(b, 'Id')]; }).map(function (b) { return col(b, 'Id'); }).join(','); }
    function branchCaption() {
        var sel = histBranches.filter(function (b) { return histBranchSel[col(b, 'Id')]; });
        $id('cmbBranchName').textContent = sel.length === 0 ? '' : (sel.length === 1 ? col(sel[0], 'BranchName') : sel.length + ' selected');
    }
    function renderBranchList() {
        $id('branchList').innerHTML = histBranches.map(function (b) {
            return '<label><input type="checkbox" data-br="' + esc(col(b, 'Id')) + '"' + (histBranchSel[col(b, 'Id')] ? ' checked' : '') + '/> ' + esc(col(b, 'BranchName')) + '</label>';
        }).join('');
        branchCaption();
    }
    function GetAll() {
        var ids = branchIds();
        if (!ids) { box('Select branch first'); return; }
        var qs = '?dateMode=' + dateMode() + '&from=' + encodeURIComponent(val('FromDateHistory')) + '&to=' + encodeURIComponent(val('ToDateHistory'))
            + '&fromDocNo=' + int(val('FromDocNoHistory')) + '&toDocNo=' + int(val('ToDocNoHistory')) + '&customerId=' + int(val('CmbCustomerHistory')) + '&branchIds=' + encodeURIComponent(ids);
        return http('GET', api + '/history' + qs).then(function (rows) {
            historyRows = rows || []; selHist = -1; renderHistory(); renderDetail(null);
        }, function (e) { box(e.message); });
    }
    function renderHistory() {
        var head = $id('histHead'), body = $id('histBody');
        if (!historyRows.length) { head.innerHTML = ''; body.innerHTML = ''; $id('histFoot').innerHTML = ''; return; }
        head.innerHTML = '<th>Print</th><th>Party Slip</th><th>Item Slip</th><th>Edit</th><th>Voucher</th><th>Attachment</th>' + HIST_COLS.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('');
        body.innerHTML = historyRows.map(function (r, i) {
            return '<tr data-h="' + i + '" class="' + (i === selHist ? 'sel' : '') + '">'
                + '<td><button type="button" class="bt" data-b="Print" data-i="' + i + '">Print</button></td>'
                + '<td><button type="button" class="bt" data-b="PartySlip" data-i="' + i + '">Party Slip</button></td>'
                + '<td><button type="button" class="bt" data-b="ItemSlip" data-i="' + i + '">Item Slip</button></td>'
                + '<td><button type="button" class="bt" data-b="Edit" data-i="' + i + '">Edit</button></td>'
                + '<td><button type="button" class="bt" data-b="Voucher" data-i="' + i + '">Voucher</button></td>'
                + '<td><button type="button" class="bt" data-b="Attach" data-i="' + i + '">Add Attachment</button></td>'
                + HIST_COLS.map(function (k) {
                    var v = col(r, k);
                    if (DATE_COLS[k]) v = shortDate(v) || v;
                    if (NUM_COLS[k]) return '<td class="n">' + (k === 'CommRate' ? rateF(v) : amt(v)) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('histFoot').innerHTML = '<td></td><td></td><td></td><td></td><td></td><td></td>' + HIST_COLS.map(function (k) {
            return '<td class="n">' + (k === 'BillAmount' ? amt(historyRows.reduce(function (a, r) { return a + num(col(r, k)); }, 0)) : '') + '</td>';
        }).join('');
    }
    var detCols = [['GdnNo', 't'], ['ItemCode', 't'], ['Item', 't'], ['BrandItemCode', 't'], ['BrandItemName', 't'], ['CropYear', 't'], ['JobLot', 't'], ['PackingType', 't'], ['PackUOM', 'q'], ['ItemQty', 'q', 1],
        ['GrossWeight', 'q', 1], ['EmptyBags', 'q'], ['EmptyBagsTotal', 'q', 1], ['WeightCut', 'q'], ['WeightCutTotal', 'q', 1], ['AddLss', 'q'], ['NetBillWeight', 'q', 1], ['StockWeight', 'q', 1], ['Rate', 'r'],
        ['RateUOM', 'q'], ['RateCut', 'r'], ['RateCutTotal', 'a', 1], ['ItemAmount', 'a', 1], ['Warehouse', 't'], ['GpDate', 'd'], ['GpNo', 'i'], ['VehicleNo', 't'], ['BillAmount', 'a', 1], ['Expense', 'a', 1],
        ['Journal', 'a', 1], ['Commission', 'a', 1], ['Freight', 'a', 1], ['ReserveWarehouse', 't'], ['CityName', 't']];
    function renderDetail(rows) {
        $id('detHead').innerHTML = rows ? detCols.map(function (c) { return '<th>' + (c[0] === 'BillAmount' ? 'Item Net Amount' : c[0]) + '</th>'; }).join('') : '';
        $id('detBody').innerHTML = (rows || []).map(function (r) {
            return '<tr>' + detCols.map(function (c) { return '<td class="' + (c[1] === 't' || c[1] === 'd' ? '' : 'n') + '">' + fmtCell(c[1], r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('detFoot').innerHTML = rows ? detCols.map(function (c) {
            return '<td class="n">' + (c[2] ? (c[1] === 'a' ? amt : qtyF)(rows.reduce(function (a, r) { return a + num(r[c[0]]); }, 0)) : '') + '</td>';
        }).join('') : '';
    }
    /* GetDetailGrdByHeadId */
    function GetDetailGrdByHeadId(i) {
        var h = historyRows[i]; if (!h) return;
        http('GET', api + '/' + int(col(h, 'Id'))).then(function (m) {
            var d = (m.details || []).map(mapDetail);
            renderDetail(d.length ? d : null);
        }, function () { renderDetail(null); });
    }
    function historyComboBind() {                                                                           /* HistoryComboBind */
        http('GET', api + '/history-customers').then(function (rows) {
            var keep = val('CmbCustomerHistory');
            fill('CmbCustomerHistory', rows, 'Id', 'Customer'); setVal('CmbCustomerHistory', keep); refreshCombos();
        }, function (e) { box(e.message); });
        http('GET', api + '/history-branches').then(function (rows) {
            var had = histBranches.length > 0;
            histBranches = rows || [];
            if (!had) histBranches.forEach(function (b) { histBranchSel[col(b, 'Id')] = true; });
            renderBranchList();
        }, function (e) { box(e.message); });
    }
    function Resethistory() {
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today()); setVal('FromDocNoHistory', ''); setVal('ToDocNoHistory', ''); setVal('CmbCustomerHistory', ''); refreshCombos();
    }
    function showTab(t) {
        tab = t;
        $id('tabPage1').style.display = t === 0 ? '' : 'none';
        $id('tabPage2').style.display = t === 1 ? '' : 'none';
        $id('tabForm').className = t === 0 ? 'on' : '';
        $id('tabHistory').className = t === 1 ? 'on' : '';
        if (t === 1) historyComboBind();
    }

    // ------------------------------------------------------------------ prints

    function VoucherReport_103(vh) {
        if (!int(vh)) { box('VoucherId Not Found'); return; }
        window.open('/api/print/acc-103/pdf?id=' + encodeURIComponent(vh) + '&documentTypeId=' + DOC, '_blank');
    }
    var PRINT_ROUTES = { '294': '294-sale-bill-direct-without-so', '294A': '294a-sale-bill-direct-without-so', '294B': '294b-sale-bill-direct-without-so', '294C': '294c-sale-bill-direct-item-slip' };
    function printSlip(kind, id) {
        if (!int(id)) { box('Record Not Found'); return; }
        window.open('/reports/print/' + PRINT_ROUTES[String(kind)] + '?id=' + encodeURIComponent(id), '_blank');
    }
    function btnPrint_Click() { VoucherReport_103(VoucherHeadId); }
    function btnSlip_Click() { printSlip(294, Id); }
    function btn294APrint_Click() { printSlip('294A', Id); }
    function btn294BPrint_Click() { printSlip('294B', Id); }
    function btnAttachment_Click() { box('Attachments are not available on this screen yet.'); }
    function btnRefresh_Click() {                                                                           /* btnRefresh_Click */
        http('GET', api + '/lookups').then(function (d) {
            var ids = ['comsupplier', 'CmbItemName', 'comWarehouse', 'CmbReserveWarehouse', 'comCropYear', 'comPackingType', 'CmbJobLot', 'combpttrm', 'combcommAgent', 'txtCity', 'cmbCurrency', 'combdeliverytrm', 'combCommType', 'combcommUOM', 'cmbOrderCategory2', 'CmbTransporterAc', 'CmbCommissionDebitAccount'];
            var keep = {}; ids.forEach(function (x) { keep[x] = val(x); });
            L = d || {}; perms = L.rights || {}; cfg = L.configuration || cfg; bindLists();
            ids.forEach(function (x) { setVal(x, keep[x]); });
            refreshCombos(); applyRights(); configurationDefault(); renderAll(); recalc();
        }, function (e) { box(e.message); });
    }
    function shortcutKeys() {
        box(['Ctrl+N  For New', 'Ctrl+R  For Refresh', 'Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete', 'Alt+1  For Print Slip 294', 'Alt+2  For Print Slip 294A',
            'Alt+3  For Print Voucher 103', 'Ctrl+F5  For Focus on Doc Date', 'Ctrl+Shift+F  Shows Stock Date and Records Update'].join('\n'));
    }

    // ------------------------------------------------------------------ keys (KeyDown :6268)

    function setHiddenTools(on) {
        STOCKDATE_VISIBLE = on && tab === 0;
        show('btnRecordsUpdate', on); show('datStockDate', STOCKDATE_VISIBLE);
    }
    function onKey(ev) {
        var k = ev.key, ctrl = ev.ctrlKey, shift = ev.shiftKey, alt = ev.altKey;
        if (ctrl && shift && (k === 'F' || k === 'f')) { ev.preventDefault(); setHiddenTools(true); return; }
        setHiddenTools(false);
        if (ctrl && !shift && (k === 'n' || k === 'N')) { ev.preventDefault(); btnNew_Click(); }
        else if (ctrl && !shift && (k === 'r' || k === 'R')) { ev.preventDefault(); btnRefresh_Click(); }
        else if (ctrl && !shift && (k === 's' || k === 'S')) { ev.preventDefault(); if (Id === 0) btnSave_Click(); }
        else if (ctrl && !shift && (k === 'u' || k === 'U')) { ev.preventDefault(); if (Id > 0) btnUpdate_Click(); }
        else if (ctrl && shift && k === 'Delete') { ev.preventDefault(); if (Id > 0) btnDelete_Click(); }
        else if (alt && k === '1') { ev.preventDefault(); btnSlip_Click(); }
        else if (alt && k === '2') { ev.preventDefault(); btn294APrint_Click(); }
        else if (alt && k === '3') { ev.preventDefault(); btnPrint_Click(); }
        else if (ctrl && k === 'F5') { ev.preventDefault(); focus('DocDate'); }
    }

    // ------------------------------------------------------------------ events

    function bindEvents() {
        on('btnAdd', 'click', btnAdd_Click); on('btnUpdateDetail', 'click', btnUpdateDetail_Click); on('btnCancelUpdateDetial', 'click', btnCancelUpdateDetial_Click);
        on('btnRecordsUpdate', 'click', recordsUpdate);
        on('comsupplier', 'change', ledgerOf); on('DocDate', 'change', function () { dueDateGenerate(); ledgerOf(); });
        on('combpttrm', 'change', dueTermChanged); on('txtduedays', 'input', dueDateGenerate);
        on('combcommAgent', 'change', recalc); on('combCommType', 'change', function () {
            if (commType() === 'Flat') setVal('txtcommrate', val('txtcommrate').trim() === '' ? '' : fmt(num(val('txtcommrate')), 0, '0'));
            recalc();
        });
        on('txtcommrate', 'input', function () {
            if (commType() === '') setVal('txtcommrate', ''); else if (commType() === 'Flat' && val('txtcommrate').trim() !== '') setVal('txtcommrate', fmt(num(val('txtcommrate')), 0, '0'));
            recalc();
        });
        on('combcommUOM', 'change', recalc); on('txtFreightAmount', 'input', recalc);
        on('cmbCurrency', 'change', currencyLeave); on('txtExchangeRate', 'input', recalc);
        on('txtExchangeRate', 'blur', function () { setVal('txtExchangeRate', rateF(val('txtExchangeRate'))); });
        on('comWarehouse', 'change', function () { loadWarehouseItems().then(AvailableStockGetByItem); });
        on('RadCategory', 'change', function () { categoryBind(); itemFill(); refreshCombos(); }); on('RadType', 'change', function () { categoryBind(); itemFill(); refreshCombos(); });
        on('CmbCategory', 'change', function () { itemFill(); refreshCombos(); });
        on('rdbtnItemName', 'change', function () { itemFill(); refreshCombos(); }); on('rdbtnItemCode', 'change', function () { itemFill(); refreshCombos(); });
        on('RadBrandName', 'change', function () { brandFill(); refreshCombos(); }); on('RadBrandCode', 'change', function () { brandFill(); refreshCombos(); });
        on('CmbItemName', 'change', itemLeave);
        ['comCropYear', 'CmbJobLot', 'comPackingType'].forEach(function (x) { on(x, 'change', AvailableStockGetByItem); });
        on('comPackUOM', 'change', packUomLeave);
        on('comRateUOM', 'change', function () { AmountCaluculation(); recalc(); });
        on('txtQty', 'input', qtyChanged);
        ['txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtAddLss'].forEach(function (x) { on(x, 'input', entryChanged); });
        ['txtRate', 'txtratecut'].forEach(function (x) { on(x, 'input', function () { AmountCaluculation(); recalc(); }); });
        on('grdBody', 'click', function (ev) { var b = ev.target.closest ? ev.target.closest('[data-del]') : null; if (b) gridDelete(int(b.getAttribute('data-del'))); });
        on('grdBody', 'dblclick', function (ev) { var tr = ev.target.closest ? ev.target.closest('tr[data-i]') : null; if (tr) grdDoubleClick(int(tr.getAttribute('data-i'))); });
        on('expBody', 'click', function (ev) {
            var t = ev.target, d = t.getAttribute && t.getAttribute('data-edel'), a = t.getAttribute && t.getAttribute('data-eadd');
            if (d !== null && d !== undefined && d !== '') { if (!window.confirm('Are you sure to Delete?')) return; dtExp.splice(int(d), 1); if (!dtExp.length) resetExp(); recalc(); renderExp(); }
            else if (a !== null && a !== undefined && a !== '') { dtExp.push({ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); renderExp(); renderFooters(); }
        });
        on('expBody', 'change', function (ev) { var t = ev.target; if (t.getAttribute('data-e') !== null) expCell(int(t.getAttribute('data-e')), t.getAttribute('data-f'), t.value); });
        on('frBody', 'click', function (ev) {
            var t = ev.target, d = t.getAttribute && t.getAttribute('data-fdel'), a = t.getAttribute && t.getAttribute('data-fadd');
            if (d !== null && d !== undefined && d !== '') { if (!window.confirm('Are you sure to Delete?')) return; dtFr.splice(int(d), 1); if (!dtFr.length) resetFr(); recalc(); renderFr(); }
            else if (a !== null && a !== undefined && a !== '') { dtFr.push({ InvGdnId: 0, Transporter: 0, Freight: 0, Debit: 0, Remarks: '' }); renderFr(); renderFooters(); }
        });
        on('frBody', 'change', function (ev) { var t = ev.target; if (t.getAttribute('data-fr') !== null) frCell(int(t.getAttribute('data-fr')), t.getAttribute('data-f'), t.value); });
        on('glBody', 'click', function (ev) {
            var t = ev.target, d = t.getAttribute && t.getAttribute('data-gdel'), a = t.getAttribute && t.getAttribute('data-gadd');
            if (d !== null && d !== undefined && d !== '') { if (!window.confirm('Are you sure to Delete?')) return; dtGl.splice(int(d), 1); if (!dtGl.length) resetGl(); recalc(); renderGl(); }
            else if (a !== null && a !== undefined && a !== '') { dtGl.push({ AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }); renderGl(); renderFooters(); }
        });
        on('glBody', 'change', function (ev) { var t = ev.target; if (t.getAttribute('data-g') !== null) glCell(int(t.getAttribute('data-g')), t.getAttribute('data-f'), t.value); });
        on('btnShowHistory', 'click', GetAll);
        on('cmbBranchName', 'click', function () { var l = $id('branchList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        on('branchList', 'change', function (ev) { var t = ev.target; if (t.getAttribute('data-br') !== null) { histBranchSel[t.getAttribute('data-br')] = t.checked; branchCaption(); } });
        on('histBody', 'click', function (ev) {
            var b = ev.target.closest ? ev.target.closest('button[data-b]') : null, tr = ev.target.closest ? ev.target.closest('tr[data-h]') : null;
            if (tr) { selHist = int(tr.getAttribute('data-h')); Array.prototype.forEach.call($id('histBody').children, function (x) { x.className = int(x.getAttribute('data-h')) === selHist ? 'sel' : ''; }); GetDetailGrdByHeadId(selHist); }
            if (!b) return;
            var r = historyRows[int(b.getAttribute('data-i'))], k = b.getAttribute('data-b'), id = int(col(r, 'Id'));
            if (k === 'Edit') ReadById(id);
            else if (k === 'Print') printSlip(294, id);
            else if (k === 'PartySlip') printSlip('294A', id);
            else if (k === 'ItemSlip') printSlip('294C', id);
            else if (k === 'Voucher') VoucherReport_103(col(r, 'VoucherHeadId'));
            else if (k === 'Attach') btnAttachment_Click();
        });
        document.addEventListener('keydown', onKey);
    }

    window.SPSR = {
        showTab: showTab, gridTab: gridTab, btnNew_Click: btnNew_Click, btnRefresh_Click: btnRefresh_Click, btnSave_Click: btnSave_Click, btnUpdate_Click: btnUpdate_Click, btnDelete_Click: btnDelete_Click,
        btnAttachment_Click: btnAttachment_Click, btnSlip_Click: btnSlip_Click, btn294APrint_Click: btn294APrint_Click, btn294BPrint_Click: btn294BPrint_Click, btnPrint_Click: btnPrint_Click,
        btnShortcutKeys_Click: shortcutKeys, BtnNewHistory_Click: function () { Resethistory(); historyRows = []; renderHistory(); renderDetail(null); Reset(); showTab(0); },
        BtnRefreshHistory_Click: function () { historyComboBind(); Resethistory(); }
    };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
}());
