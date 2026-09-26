/* ============================================================================================
 * Screen 280 "Production Against (Job Order)" - tab PackingMaterial.
 * Desktop form: Architecture.WinApp.Production.frmProductionPackingMaterial
 * (src280/frmProductionPackingMaterial.cs; the :line numbers below are that file's).
 *
 * The desktop is synchronous: a Leave handler finishes its database round trip before the next
 * statement runs. Here every such handler is an async function and is AWAITED where the desktop
 * calls it inline, so the statement order - and therefore the final state of the form - is the
 * desktop's. The one place a late answer could overwrite a value the desktop would already have
 * replaced (the average rate) carries a token: any later assignment to the Rate box wins.
 *
 * FIFOCGSFlag is declared in this form (:47) and never assigned, so it is always false and the
 * FIFO branches of GetAvgRateByItemAndWarehouseForPackingMAterial and AverageCalculate are
 * unreachable. Only the GetAvgRateQtyAndStockInHand branch is ported.
 *
 * Embedding contract: window.P280ReadById(id) selects tab 0 and runs ReadByIdPackingMaterial(id)
 * (FoodProductionWithValues.SettlementForm_OnPackingMaterialReadById:3317).
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/production-against-job-order/packing-material';
    var K = window.ReportKit;
    var DOC_TYPE_ID = 111;

    /* ------------------------------------------------------------------ form state (:29-47) */
    var LK = null;                  // /init payload
    var R = { save: false, update: false, print: false, rateAndAmount: false };
    var RECID = 0;                  // RecpackingMaterial
    var VOUCHER_HEAD_ID = 0;        // VoucherHeadId
    var UPDATE_IDX = -1;            // updateDetailIndexPM
    var ROWS = [];                  // tablePackingMaterial
    var CUR = -1;                   // grddetailPackingMaterial.CurrentRow
    var SCHEDULES = [];             // dtScheduleData
    var SCHED_REC = 0;              // the RecId dtScheduleData was read with
    var RACKS = [];                 // racksWithWarehouseAndItems for the current item
    var RACKS_ITEM = null;
    var DOC_TIME = nowTime();       // txtdocdatePackingMaterial keeps a time of day (DateTimePicker.Value)
    var HIST_TIME = nowTime();
    var HIST_ROWS = [];
    var BOOTED = false, PENDING_READ = null;

    // ============================================================================ utilities

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblStatus'); if (e) e.textContent = m; }
    function box(m) { window.alert(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    function enabled(id) { var e = $id(id); return !!e && !e.disabled; }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return '';
    }
    /** Conversion.ToInt / ToDouble: lenient, 0 on anything unreadable. */
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function dbl(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    /** double.Parse (NumberStyles.Float | AllowThousands): NaN where .NET throws FormatException. */
    function netParse(t) {
        var s = String(t);
        if (!/^\s*[+-]?(\d[\d,]*\.?\d*|\.\d+)([eE][+-]?\d+)?\s*$/.test(s)) return NaN;
        return parseFloat(s.replace(/,/g, ''));
    }
    /** Math.Round(x) / Math.Round(x, d): MidpointRounding.ToEven. */
    function roundEven(x, d) {
        var f = Math.pow(10, d || 0), y = x * f, r = Math.round(y);
        if (Math.abs(y % 1) === 0.5 && r % 2 !== 0) r -= 1;
        return r / f;
    }
    /** double.ToString() - general format, no grouping. */
    function netStr(n) { return String(n); }
    /** "#,#" - 0 prints as an empty string, exactly like .NET. */
    function fmtHash(n) { var r = Math.round(Math.abs(n)) * (n < 0 ? -1 : 1); return r === 0 ? '' : K.fixed(r, 0); }
    function pad(n) { return String(n).padStart(2, '0'); }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function nowTime() { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function addDays(ymd, n) { var p = ymd.split('-'); var d = new Date(+p[0], +p[1] - 1, +p[2] + n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function dateOnly(v) {
        if (!v) return '';
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(v);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    function docDateTime() { return (val('txtdocdatePackingMaterial') || today()) + 'T' + DOC_TIME; }
    function rateDec() { return LK ? int(LK.rateDecimals) : 2; }
    function amountDec() { return LK ? int(LK.amountDecimals) : 2; }
    function fq(v) { return K.num(v, 3); }          // "#,##0.###"
    function fr(v) { return K.fixed(v, rateDec()); } // DecimalRateFormate
    function fa(v) { return K.fixed(v, amountDec()); } // stringFormatsingle

    function csrf(h) {
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse);
    }
    function postJson(url, body) {
        return fetch(url, { method: 'POST', credentials: 'same-origin',
            headers: csrf({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body) }).then(parse);
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function q(o) {
        return Object.keys(o).filter(function (k) { return o[k] !== undefined && o[k] !== null && o[k] !== ''; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }
    /** Button contract: disabled + spinner, duplicates ignored, re-enabled on success and failure. */
    function busy(id, fn) {
        var b = $id(id);
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.pmWas = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = b.dataset.pmWas === '1'; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        return p.then(done, function (e) { done(); if (e) box(e.message || String(e)); });
    }

    // ============================================================================ combos

    if (window.DesktopCombo) {
        /* ItemFillPackingMaterial:694 - Id hidden, ItemName, ItemCode (AllColumns). */
        window.DesktopCombo.define('p280pmItem', [
            { caption: 'Item Name', flex: 3 }, { caption: 'ItemCode', flex: 2, key: 'code' }]);
        /* RackBindFromGlobalRacksByItemId:769 - col 2 (WarehouseId) hidden. */
        window.DesktopCombo.define('p280pmRack', [
            { caption: 'Rack Name', flex: 2 }, { caption: 'WarehouseName', flex: 2, key: 'wh' },
            { caption: 'BaseRackId', flex: 1, key: 'base', type: 'num' }]);
        /* ScheduleNoBind:986 - cols 2 (DocumentTypeId) and 5 (InvoiceId) hidden. */
        window.DesktopCombo.define('p280pmSchedule', [
            { caption: 'Schedule / Invoice No', flex: 3 }, { caption: 'NoOfContainer', flex: 1, key: 'cont', type: 'num' },
            { caption: 'MTon', flex: 1, key: 'mton', type: 'num' }, { caption: 'ScheduleNo', flex: 2, key: 'sno' },
            { caption: 'InvoiceMTon', flex: 1, key: 'imton', type: 'num' }]);
    }

    /** Fill a <select>. `withDefault` is InfragisticsHelper's insertDefaultRow ("-- Select --", 0).
     *  `keep` retains the current value when it is still offered (BindAndRetainSelection). */
    function fill(id, rows, valueKey, textKey, withDefault, keep, attrs) {
        var s = $id(id);
        var prev = s.value;
        var h = withDefault ? '<option value="0">-- Select --</option>' : '';
        (rows || []).forEach(function (r) {
            var extra = '';
            if (attrs) for (var a in attrs) extra += ' data-' + a + '="' + esc(col(r, attrs[a])) + '"';
            h += '<option value="' + esc(col(r, valueKey)) + '"' + extra + '>' + esc(col(r, textKey)) + '</option>';
        });
        s.innerHTML = h;
        s.selectedIndex = -1;
        if (keep && prev !== '' && prev !== null) setCombo(id, prev);
    }
    /** combo.Value = v: selects the row when it exists, otherwise nothing is active. */
    function setCombo(id, v) {
        var s = $id(id), sv = String(v === null || v === undefined ? '' : v);
        for (var i = 0; i < s.options.length; i++) {
            if (s.options[i].value === sv) { s.selectedIndex = i; return true; }
        }
        s.selectedIndex = -1;
        return false;
    }
    /** combo.Text = "" - no active row. The list stays. */
    function clearCombo(id) { $id(id).selectedIndex = -1; }
    /** DataSource = null. */
    function emptyCombo(id) { $id(id).innerHTML = ''; }
    /** ActiveRow != null. */
    function active(id) { return $id(id).selectedIndex >= 0; }
    function cval(id) { return active(id) ? int($id(id).value) : 0; }
    function ctext(id) { var s = $id(id); return s.selectedIndex >= 0 ? s.options[s.selectedIndex].textContent : ''; }
    function focusCombo(id) {
        var s = $id(id), w = s.parentNode && s.parentNode.querySelector && s.parentNode.querySelector('.dtcombo-wrap input');
        (w || s).focus();
    }

    // ===================================================================== TextChanged plumbing

    var RATE_TOKEN = 0;             // bumped by every assignment to the Rate box
    var AVG_SEQ = 0;                // orders the average-rate calls themselves
    /** Assign a TextBox's Text; WinForms raises TextChanged only when the text actually changes. */
    function setText(id, v) {
        var e = $id(id), s = (v === null || v === undefined) ? '' : String(v);
        if (id === 'txtPackingMaterialRate') RATE_TOKEN++;
        if (e.value === s) return;
        e.value = s;
        if (id === 'txtPackingMaterialQTY') qtyTextChanged();
        else if (id === 'txtPackingMaterialRate') rateTextChanged();
    }
    /** txtPackingMaterialQTY_TextChanged:1553. */
    function qtyTextChanged() { getAvgRate(); calculation(); }
    /** txtPackingMaterialRate_TextChanged:1528. */
    function rateTextChanged() { calculation(); }

    /** CaluculatonofPackingMaterial:1533 - Math.Round(Rate x Qty), whole number. */
    function calculation() {
        var q = val('txtPackingMaterialQTY'), r = val('txtPackingMaterialRate');
        if (q !== '' && r !== '') {
            var rn = netParse(r), qn = netParse(q);
            if (isNaN(rn) || isNaN(qn)) { box('Input string was not in a correct format.'); return; }
            setVal('txtAmountPackingMaterial', netStr(roundEven(rn * qn, 0)));
        } else {
            setVal('txtAmountPackingMaterial', '0');
        }
    }

    /** GetAvgRateByItemAndWarehouseForPackingMAterial:1832 (non-FIFO branch). */
    function getAvgRate() {
        setText('txtPackingMaterialRate', '0');
        if (!R.rateAndAmount) return Promise.resolve();
        var rate = $id('txtPackingMaterialRate');
        rate.disabled = false;
        var token = RATE_TOKEN, seq = ++AVG_SEQ;
        return getJson(api + '/avg-rate?' + q({ itemId: cval('cmbItemPackingMaterial'), docDate: docDateTime(),
                itemConditionId: cval('cmbItemCondition'), recId: RECID }))
            .then(function (d) {
                if (seq !== AVG_SEQ) return;                 // a newer call owns the box
                var v = d ? dbl(d.avgRate) : 0;
                if (v > 0) {
                    /* On the desktop this ran before any later statement: the text may since
                       have been replaced, but the box stays disabled either way. */
                    if (token === RATE_TOKEN) setText('txtPackingMaterialRate', netStr(v));
                    rate.disabled = true;
                }
            }).catch(function (e) { box(e.message); });
    }

    // ================================================================================ Load

    /** frmFoodProduction_Load:272. */
    function load() {
        say('Loading...');
        return getJson(api + '/init').then(function (d) {
            LK = d || {};
            R = LK.rights || R;
            /* :287-290 */
            $id('btnsavepackingmaterial').disabled = !R.save;
            $id('btnUpdatepackingmaterial').disabled = !R.update;
            $id('btnVoucherPM').disabled = !R.print;
            $id('btnPackingMaterialSlip').disabled = !R.print;
            if (int(LK.docNo) > 0) setVal('txtDocNoPackingMaterial', LK.docNo);   // GenerateCodeOfPackingMaterial
            jobOrderNoFill(LK.jobOrders);
            itemFill(LK.items);
            var p = itemLeave();                                                   // ItemFillPackingMaterial:707
            itemConditionBind(LK.itemConditions);
            SCHEDULES = LK.schedules || []; SCHED_REC = int(LK.scheduleRecId);
            scheduleNoBind();
            /* :325-336 - no Rate right: Remarks moves to x=191 at 607px, Rate/Amount and their
               labels and the voucher button disappear. */
            if (!R.rateAndAmount) {
                show('wrapRate', false); show('wrapAmount', false); show('btnVoucherPM', false);
                $id('wrapRemarks').style.width = '607px';
            }
            $id('txtdocdatePackingMaterial').focus();
            historyComboBind(LK.historyJobOrders);
            show('btnUpdateDetailPm', false); show('btnCancelDetailPm', false);
            renderDetail();
            say('Ready - rights read for ' + (R.screenName || ''));
            return p;
        }).catch(function (e) { say('Load failed'); box(e.message); });
    }

    /** JobOrderNoFill:348 - Id / PlanCode, DocumentTypeId carried but hidden; value retained. */
    function jobOrderNoFill(rows) {
        var prev = cval('cmbJobOrderPackingMaterial');
        if (!rows || !rows.length) return;
        fill('cmbJobOrderPackingMaterial', rows, 'Id', 'PlanCode', false, false, { doctype: 'DocumentTypeId' });
        if (prev > 0) setCombo('cmbJobOrderPackingMaterial', prev);
    }
    /** ItemFillPackingMaterial:694 - ItemTypeOfTypeId == 14, default row, retained. */
    function itemFill(rows) {
        fill('cmbItemPackingMaterial', rows, 'Id', 'ItemName', true, true, { code: 'ItemCode' });
    }
    /** ItemConditionBindFromGlobal:651 - without Id 4 (server), default row, retained. */
    function itemConditionBind(rows) {
        fill('cmbItemCondition', rows, 'Id', 'Description', true, true);
    }
    /** ScheduleNoBind:986 - default row, retained, not activated. */
    function scheduleNoBind() {
        fill('CmbInvoiceNo', SCHEDULES, 'Id', 'ScheduleCode', true, true,
            { cont: 'NoOfContainer', mton: 'MTon', sno: 'ScheduleNo', imton: 'InvoiceMTon' });
    }
    /** HistoryComboBind:2104 - no default row, retained, not activated. */
    function historyComboBind(rows) {
        fill('CmbJobOrder', rows || [], 'Id', 'ReferenceName', false, true);
    }

    // ========================================================================= Leave events

    /** cmbJobOrderPackingMaterial_Leave:817. */
    function jobOrderLeave() {
        var jo = cval('cmbJobOrderPackingMaterial');
        if (jo <= 0) return Promise.resolve();
        return getPlantFeeder(jo).then(getTotals).then(brandItemsForPm);
    }

    /** GetPlantFeeder:391 - when the job order has no plants the OLD list stays and only the text
     *  is cleared (desktop quirk, reproduced). */
    function getPlantFeeder(jobOrderId) {
        var prev = cval('CmbPlantFeederPackingMaterial');
        return getJson(api + '/plants?' + q({ jobOrderId: jobOrderId })).then(function (dt) {
            if (dt && dt.length) {
                if (active('cmbJobOrderPackingMaterial')) fill('CmbPlantFeederPackingMaterial', dt, 'PlantId', 'PlantName', false, false);
                if (prev > 0) setCombo('CmbPlantFeederPackingMaterial', prev);
            } else {
                clearCombo('CmbPlantFeederPackingMaterial');
            }
        }).catch(function (e) { box(e.message); });
    }

    /** GetTotalInPutOutOnPM:834. */
    function getTotals() {
        return getJson(api + '/totals?' + q({ jobOrderId: cval('cmbJobOrderPackingMaterial'), plantId: cval('CmbPlantFeederPackingMaterial') }))
            .then(function (dt) {
                if (dt && dt.length) {
                    var inQ = 0, inW = 0, outQ = 0, outW = 0, fQ = 0, fW = 0;
                    dt.forEach(function (r) {
                        if (String(col(r, 'EntryTypeOutPut')) === 'FinishGoods') { fQ += dbl(col(r, 'ItemQty')); fW += dbl(col(r, 'StockWeight')); }
                        if (String(col(r, 'EntryType')) === 'Input') { inQ += dbl(col(r, 'ItemQty')); inW += dbl(col(r, 'StockWeight')); }
                        else { outQ += dbl(col(r, 'ItemQty')); outW += dbl(col(r, 'StockWeight')); }
                    });
                    setVal('txtTotalInputQtyPm', fmtHash(inQ)); setVal('txtTotalInputWeightPm', fmtHash(inW));
                    setVal('txtTotaloutputQtyPm', fmtHash(outQ)); setVal('txtTotalOutPutWeightPm', fmtHash(outW));
                    setVal('txtTotalFinishQtyPm', fmtHash(fQ)); setVal('txtTotalFinishWeightPm', fmtHash(fW));
                } else {
                    ['txtTotalInputQtyPm', 'txtTotalInputWeightPm', 'txtTotaloutputQtyPm', 'txtTotalOutPutWeightPm',
                     'txtTotalFinishQtyPm', 'txtTotalFinishWeightPm'].forEach(function (i) { setVal(i, '0'); });
                }
            }).catch(function (e) { box(e.message); });
    }

    function brandRows(withPmDoc) {
        return getJson(api + '/brand-items?' + q({ jobOrderId: cval('cmbJobOrderPackingMaterial'),
            itemId: cval('cmbItemPackingMaterial'), pmDocId: withPmDoc ? int(val('txtIdPMRec')) : 0 }));
    }

    /** BrandItemsForPackMaterialAgainstPmItem:610 - distinct ItemId / ItemName. */
    function brandItemsForPm() {
        return brandRows(true).then(function (lst) {
            if (lst && lst.length) {
                var seen = {}, dt = [];
                lst.forEach(function (r) { var k = int(col(r, 'ItemId')); if (!seen[k]) { seen[k] = 1; dt.push({ ItemId: k, ItemName: col(r, 'ItemName') }); } });
                fill('CmbBrandNamePackingMaterial', dt, 'ItemId', 'ItemName', false, false);
            } else {
                clearCombo('CmbBrandNamePackingMaterial'); emptyCombo('CmbBrandNamePackingMaterial');
            }
        }).catch(function (e) { box(e.message); });
    }

    /** BrandUomForPackMaterialAgainstPmItemAndBrandItem:559 - the chosen output item's UOMs;
     *  exactly one -> it is selected and CmbBrandUomPackingMaterial_Leave runs. */
    function brandUomForPm() {
        return brandRows(true).then(function (lst) {
            if (lst && lst.length) {
                var brand = cval('CmbBrandNamePackingMaterial'), seen = {}, dt = [];
                lst.forEach(function (r) {
                    var u = int(col(r, 'ItemUomId'));
                    if (!seen[u] && int(col(r, 'ItemId')) === brand) { seen[u] = 1; dt.push({ ItemUomId: u, PackUom: col(r, 'PackUom') }); }
                });
                fill('CmbBrandUomPackingMaterial', dt, 'ItemUomId', 'PackUom', false, false);
                if (dt.length === 1) {
                    setCombo('CmbBrandUomPackingMaterial', dt[0].ItemUomId);
                    return brandUomLeave();
                }
            } else {
                clearCombo('CmbBrandUomPackingMaterial'); emptyCombo('CmbBrandUomPackingMaterial');
            }
        }).catch(function (e) { box(e.message); });
    }

    /** CmbBrandNamePackingMaterial_Leave:900. */
    function brandLeave() {
        return brandUomForPm().then(function () {
            if (active('CmbBrandUomPackingMaterial')) return getOutPutBalQty();
        });
    }

    /** CmbBrandUomPackingMaterial_Leave:1883. */
    function brandUomLeave() {
        return Promise.resolve(getAvgRate()).then(getOutPutBalQty);
    }

    /** GetOutPutBalQtyForPMQty:916 - Save mode only. The first row whose UOM is the chosen one
     *  and whose balance exceeds the packing already issued; none -> the desktop's
     *  "There is no row at position 0." (dt.Rows[0] on an empty table), reproduced. */
    function getOutPutBalQty() {
        if (!visible('btnsavepackingmaterial') || !enabled('btnsavepackingmaterial')) return Promise.resolve();
        return brandRows(false).then(function (lst) {
            if (!lst || !lst.length || !active('CmbBrandUomPackingMaterial')) return;
            setText('txtPackingMaterialQTY', '0');
            var uom = cval('CmbBrandUomPackingMaterial'), hit = null;
            for (var i = 0; i < lst.length; i++) {
                var r = lst[i];
                if (dbl(col(r, 'BalQty')) > dbl(col(r, 'PmQty')) && int(col(r, 'ItemUomId')) === uom) { hit = r; break; }
            }
            if (!hit) { box('There is no row at position 0.'); return; }
            setText('txtPackingMaterialQTY', netStr(dbl(col(hit, 'BalQty'))));
        }).catch(function (e) { box(e.message); });
    }

    /** cmbItemPackingMaterial_Leave:1814. */
    function itemLeave() {
        var itemId = cval('cmbItemPackingMaterial');
        return getOutPutBalQty()
            .then(brandItemsForPm)
            .then(function () { return getAvgRate(); })
            .then(function () { calculation(); return bindWarehouse(itemId); })
            .then(function () { return rackBind(itemId, cval('CmbWarehousePackingMaterial')); })
            .catch(function (e) { box(e.message); });
    }

    function loadRacks(itemId) {
        if (RACKS_ITEM === itemId) return Promise.resolve(RACKS);
        return getJson(api + '/racks?' + q({ itemId: itemId })).then(function (d) {
            RACKS = d || []; RACKS_ITEM = itemId; return RACKS;
        });
    }

    /** BindWarehouseDropdown:715 - warehouses of the item's racks (first per warehouse); one ->
     *  selected; several -> config PackingMaterialDefaultWarehouse when it is among them. */
    function bindWarehouse(itemId) {
        return loadRacks(itemId).then(function (racks) {
            var seen = {}, dt = [];
            racks.forEach(function (r) { var w = int(r.WarehouseId); if (!seen[w]) { seen[w] = 1; dt.push({ Id: w, Warehouse: r.WareHouseName }); } });
            fill('CmbWarehousePackingMaterial', dt, 'Id', 'Warehouse', true, true);
            if (!dt.length) { clearCombo('CmbWarehousePackingMaterial'); emptyCombo('CmbWarehousePackingMaterial'); return; }
            if (cval('CmbWarehousePackingMaterial') !== 0) return;
            if (dt.length === 1) setCombo('CmbWarehousePackingMaterial', dt[0].Id);
            else if (dt.length > 1) {
                var cfg = LK ? int(LK.defaultWarehouseId) : 0;
                if (cfg !== 0 && dt.some(function (x) { return x.Id === cfg; })) setCombo('CmbWarehousePackingMaterial', cfg);
            }
        });
    }

    /** RackBindFromGlobalRacksByItemId:749. */
    function rackBind(itemId, warehouseId) {
        return loadRacks(itemId).then(function (racks) {
            var seen = {}, dt = [];
            racks.forEach(function (r) {
                if ((warehouseId === 0 || int(r.WarehouseId) === warehouseId) && !seen[r.Id]) { seen[r.Id] = 1; dt.push(r); }
            });
            fill('CmbRackName', dt, 'Id', 'RackName', true, true, { wh: 'WareHouseName', base: 'BaseRackId' });
            if (!dt.length) { clearCombo('CmbRackName'); emptyCombo('CmbRackName'); return; }
            if (cval('CmbRackName') !== 0) return;
            if (dt.length === 1) { setCombo('CmbRackName', dt[0].Id); rackLeave(); }
            else {
                var b = null;
                for (var i = 0; i < dt.length; i++) if (int(dt[i].BaseRackId) > 0) { b = dt[i]; break; }
                if (b) { setCombo('CmbRackName', b.Id); rackLeave(); }
            }
        }).catch(function (e) { box('Error loading Rack: ' + e.message); });
    }

    /** CmbRackName_Leave:799 - the rack's warehouse wins. */
    function rackLeave() {
        var id = cval('CmbRackName'), wh = cval('CmbWarehousePackingMaterial'), rack = null;
        for (var i = 0; i < RACKS.length; i++) if (int(RACKS[i].Id) === id) { rack = RACKS[i]; break; }
        if (rack && (wh === 0 || int(rack.WarehouseId) !== wh)) setCombo('CmbWarehousePackingMaterial', rack.WarehouseId);
    }

    /** CmbWarehousePackingMaterial_Leave:1952. */
    function warehouseLeave() { return rackBind(cval('cmbItemPackingMaterial'), cval('CmbWarehousePackingMaterial')); }
    /** cmbItemCondition_Leave:2055. */
    function itemConditionLeave() { return Promise.resolve(getAvgRate()).then(calculation); }
    /** CmbPlantFeederPackingMaterial_Leave:1940. */
    function plantLeave() { return getTotals(); }

    // ======================================================================= detail portion

    /** FormValidationOfDetailPortionPackingMaterial:500. */
    function detailValidation() {
        function stop(m, id) { box(m); focusCombo(id); return false; }
        if (!active('cmbJobOrderPackingMaterial')) return stop('job Order Field Required', 'cmbJobOrderPackingMaterial');
        if (!active('CmbPlantFeederPackingMaterial')) return stop('Plant Field Required', 'CmbPlantFeederPackingMaterial');
        if (!active('cmbItemPackingMaterial')) return stop('Packing Material Item Field Required', 'cmbItemPackingMaterial');
        if (!active('CmbWarehousePackingMaterial')) return stop('Warehouse Field Required', 'CmbWarehousePackingMaterial');
        if (!active('CmbRackName')) return stop('Rack Name Field Required', 'CmbRackName');
        if (active('cmbItemCondition') && cval('cmbItemCondition') <= 0) return stop('Item Condition Field Required', 'cmbItemCondition');
        if (active('CmbBrandNamePackingMaterial') && cval('CmbBrandUomPackingMaterial') <= 0) return stop('ItemUOM Field Required', 'CmbBrandUomPackingMaterial');
        var rt = val('txtPackingMaterialRate').trim(), am = val('txtAmountPackingMaterial').trim();
        if ((rt === '' || dbl(rt) === 0) && R.rateAndAmount) { box('Rate Field Required'); $id('txtPackingMaterialRate').focus(); return false; }
        if ((am === '' || dbl(am) === 0) && R.rateAndAmount) { box('Amount Field Required'); $id('txtAmountPackingMaterial').focus(); return false; }
        return true;
    }

    /** btnAddDetailPm_Click:1615. */
    function addDetail() {
        if (!detailValidation()) return Promise.resolve();
        var wh = cval('CmbWarehousePackingMaterial');
        ROWS.push({
            Id: 0, DocDate: val('txtdocdatePackingMaterial') || today(), DocNo: int(val('txtDocNoPackingMaterial').trim()),
            InvProductionJobOrderId: cval('cmbJobOrderPackingMaterial'),
            PlantId: cval('CmbPlantFeederPackingMaterial'), Plant: ctext('CmbPlantFeederPackingMaterial').trim(),
            PackingItemId: cval('cmbItemPackingMaterial'), PackingItem: ctext('cmbItemPackingMaterial').trim(),
            WarehouseId: wh, Warehouse: wh > 0 ? ctext('CmbWarehousePackingMaterial').trim() : '',
            RackId: cval('CmbRackName'), RackName: ctext('CmbRackName').trim(),
            ItemConditionId: cval('cmbItemCondition'), ItemCondition: ctext('cmbItemCondition').trim(),
            BrandId: cval('CmbBrandNamePackingMaterial'), Brand: ctext('CmbBrandNamePackingMaterial').trim(),
            BrandUomId: cval('CmbBrandUomPackingMaterial'), BrandUom: ctext('CmbBrandUomPackingMaterial').trim(),
            ItemQty: dbl(val('txtPackingMaterialQTY').trim()), ItemRate: dbl(val('txtPackingMaterialRate').trim()),
            Amount: dbl(val('txtAmountPackingMaterial').trim()), Remarks: val('txtremarksPackingMaterial').trim(),
            ContractScheduleId: cval('CmbInvoiceNo'), ContractScheduleNo: ctext('CmbInvoiceNo')
        });
        if (CUR < 0) CUR = 0;
        renderDetail();
        resetDetail();
        jobOrderLock();
        focusCombo('CmbPlantFeederPackingMaterial');
        return Promise.resolve();
    }

    /** btnUpdateDetailPm_Click:1701 - Plant text, RackId and RackName are NOT written back
     *  (desktop quirk), and CmbBrandNamePackingMaterial_Leave runs between Brand and BrandUom,
     *  so the UOM (and in Save mode the qty) can change under the user before they are read. */
    function updateDetail() {
        if (!detailValidation()) return Promise.resolve();
        var r = ROWS[UPDATE_IDX];
        if (!r) { box('There is no row at position ' + UPDATE_IDX + '.'); return Promise.resolve(); }
        var wh = cval('CmbWarehousePackingMaterial');
        r.PlantId = cval('CmbPlantFeederPackingMaterial');
        r.WarehouseId = wh;
        r.Warehouse = wh > 0 ? ctext('CmbWarehousePackingMaterial').trim() : '';
        r.PackingItemId = cval('cmbItemPackingMaterial');
        r.PackingItem = ctext('cmbItemPackingMaterial');
        r.BrandId = cval('CmbBrandNamePackingMaterial');
        r.Brand = ctext('CmbBrandNamePackingMaterial');
        return brandLeave().then(function () {
            r.BrandUomId = cval('CmbBrandUomPackingMaterial');
            r.BrandUom = ctext('CmbBrandUomPackingMaterial');
            r.ItemConditionId = cval('cmbItemCondition');
            r.ItemCondition = ctext('cmbItemCondition');
            r.ItemQty = dbl(val('txtPackingMaterialQTY').trim());
            r.ItemRate = dbl(val('txtPackingMaterialRate').trim());
            r.Amount = dbl(val('txtAmountPackingMaterial').trim());
            r.Remarks = val('txtremarksPackingMaterial').trim();
            r.ContractScheduleId = cval('CmbInvoiceNo');
            r.ContractScheduleNo = cval('CmbInvoiceNo') > 0 ? ctext('CmbInvoiceNo') : '';
            renderDetail();
            resetDetail();
            jobOrderLock();
        });
    }

    /** grddetailPackingMaterial_DoubleClick:1768. */
    function editDetailRow(idx) {
        var r = ROWS[idx];
        if (!r) return Promise.resolve();
        UPDATE_IDX = idx;
        setVal('txtdocdatePackingMaterial', dateOnly(r.DocDate)); DOC_TIME = '00:00:00';
        setVal('txtDocNoPackingMaterial', r.DocNo);
        setVal('txtIdPMRec', r.Id);
        setCombo('cmbJobOrderPackingMaterial', r.InvProductionJobOrderId);
        return jobOrderLeave().then(function () {
            setCombo('CmbPlantFeederPackingMaterial', r.PlantId);
            setCombo('cmbItemPackingMaterial', r.PackingItemId);
            return itemLeave();
        }).then(function () {
            setCombo('CmbWarehousePackingMaterial', r.WarehouseId);
            setCombo('CmbRackName', r.RackId);
            setCombo('cmbItemCondition', r.ItemConditionId);
            setCombo('CmbBrandNamePackingMaterial', r.BrandId);
            return brandLeave();
        }).then(function () {
            setCombo('CmbBrandUomPackingMaterial', r.BrandUomId);
            /* the grid cells' Text: formatted as the grid shows them */
            setText('txtPackingMaterialQTY', fq(r.ItemQty));
            setText('txtPackingMaterialRate', fr(r.ItemRate));
            setVal('txtAmountPackingMaterial', fa(r.Amount));
            setVal('txtremarksPackingMaterial', r.Remarks);
            setCombo('CmbInvoiceNo', String(int(r.ContractScheduleId)));
            show('btnAddDetailPm', false); show('btnUpdateDetailPm', true); show('btnCancelDetailPm', true);
            focusCombo('CmbPlantFeederPackingMaterial');
            jobOrderLock();
        });
    }

    /** grddetailPackingMaterial_ColumnButtonClick:1740 - "X". */
    function deleteDetailRow(idx) {
        if (!enabled('btnsavepackingmaterial') || !visible('btnsavepackingmaterial')) {
            box('You Can not Delete Record In Update Mode');
            return;
        }
        ROWS.splice(idx, 1);
        if (CUR >= ROWS.length) CUR = ROWS.length - 1;
        renderDetail();
        jobOrderLock();
    }

    /** JobOrderPackingMaterialLock:1681. */
    function jobOrderLock() {
        var s = $id('cmbJobOrderPackingMaterial');
        if (ROWS.length > 0) { s.disabled = true; setCombo('cmbJobOrderPackingMaterial', ROWS[0].InvProductionJobOrderId); }
        else s.disabled = false;
    }

    /** ResetdetailPackingMaterial:1068. */
    function resetDetail() {
        setVal('txtIdPMRec', '');
        clearCombo('CmbPlantFeederPackingMaterial');
        clearCombo('cmbItemPackingMaterial');
        clearCombo('CmbWarehousePackingMaterial'); emptyCombo('CmbWarehousePackingMaterial');
        clearCombo('CmbRackName'); emptyCombo('CmbRackName');
        clearCombo('CmbBrandNamePackingMaterial');
        clearCombo('CmbBrandUomPackingMaterial');
        setText('txtPackingMaterialQTY', '');
        setText('txtPackingMaterialRate', '');
        setVal('txtAmountPackingMaterial', '');
        setVal('txtremarksPackingMaterial', '');
        show('btnAddDetailPm', true); show('btnUpdateDetailPm', false); show('btnCancelDetailPm', false);
        focusCombo('CmbPlantFeederPackingMaterial');
    }

    /** FormResetOfPackingMaterial:1018. txtIdPMRec, Item Condition and Schedule are not cleared. */
    function formReset() {
        RECID = 0; VOUCHER_HEAD_ID = 0;
        show('btnsavepackingmaterial', true); show('btnUpdatepackingmaterial', false);
        $id('cmbJobOrderPackingMaterial').disabled = false;
        var p = getJson(api + '/next-code').then(function (d) { if (d && int(d.docNo) > 0) setVal('txtDocNoPackingMaterial', d.docNo); })
            .catch(function (e) { box(e.message); });
        clearCombo('CmbPlantFeederPackingMaterial');
        clearCombo('cmbItemPackingMaterial');
        clearCombo('CmbWarehousePackingMaterial'); emptyCombo('CmbWarehousePackingMaterial');
        clearCombo('CmbRackName'); emptyCombo('CmbRackName');
        clearCombo('CmbBrandNamePackingMaterial');
        clearCombo('CmbBrandUomPackingMaterial');
        setText('txtPackingMaterialQTY', '');
        setText('txtPackingMaterialRate', '');
        setVal('txtAmountPackingMaterial', '');
        setVal('txtremarksPackingMaterial', '');
        clearCombo('cmbJobOrderPackingMaterial');
        show('btnAddDetailPm', true); show('btnUpdateDetailPm', false); show('btnCancelDetailPm', false);
        ROWS = []; CUR = -1; UPDATE_IDX = -1;
        renderDetail();
        ['txtTotalFinishQtyPm', 'txtTotalFinishWeightPm', 'txtTotaloutputQtyPm', 'txtTotalOutPutWeightPm',
         'txtTotalInputQtyPm', 'txtTotalInputWeightPm'].forEach(function (i) { setVal(i, ''); });
        setVal('txtdocdatePackingMaterial', today()); DOC_TIME = nowTime();
        focusCombo('cmbJobOrderPackingMaterial');
        return p;
    }

    // ============================================================================ grid

    /* grdDetailPackingMaterialSettings:1636 - hidden: Id, PackingItemId, WarehouseId, RackId,
       ItemConditionId, PlantId, BrandUomId, BrandId, InvProductionJobOrderId, ContractScheduleId,
       DocDate, DocNo (+ ItemRate, Amount without the Rate right). "Delete" button "X" first. */
    var DETAIL_COLS = [
        { k: 'Plant', w: 130 }, { k: 'PackingItem', w: 180 }, { k: 'Warehouse', w: 130 }, { k: 'RackName', w: 100 },
        { k: 'ItemCondition', w: 100 }, { k: 'Brand', w: 180 }, { k: 'BrandUom', w: 70 },
        { k: 'ItemQty', w: 80, n: 'q', sum: true }, { k: 'ItemRate', w: 80, n: 'r', rate: true },
        { k: 'Amount', w: 100, n: 'a', sum: true, rate: true }, { k: 'Remarks', w: 200 },
        { k: 'ContractScheduleNo', w: 130, cap: 'Schedule / Invoice No' }
    ];
    function detailCols() { return DETAIL_COLS.filter(function (c) { return !c.rate || R.rateAndAmount; }); }
    function cellFmt(c, v) { return c.n === 'q' ? fq(v) : c.n === 'r' ? fr(v) : c.n === 'a' ? fa(v) : (v === null || v === undefined ? '' : v); }

    function renderDetail() {
        var cols = detailCols();
        $id('detailHead').innerHTML = '<th style="width:24px;">Delete</th>' + cols.map(function (c) {
            return '<th style="min-width:' + c.w + 'px;">' + esc(c.cap || c.k) + '</th>';
        }).join('');
        $id('detailBody').innerHTML = ROWS.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === CUR ? ' class="is-current"' : '') + '><td class="ctr"><button type="button" class="pm-x" data-del="' + i + '">X</button></td>'
                + cols.map(function (c) { return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(cellFmt(c, r[c.k])) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('detailFoot').innerHTML = ROWS.length ? '<tr><td></td>' + cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = 0; ROWS.forEach(function (r) { s += dbl(r[c.k]); });
            return '<td class="num">' + esc(cellFmt(c, s)) + '</td>';
        }).join('') + '</tr>' : '';
        $id('lblDetailCount').textContent = ROWS.length ? '(' + ROWS.length + ' rows)' : '';
        K.filterRow($id('grddetailPackingMaterial'));
    }

    /* GridSettingOfPackingmaterial:1430 - Id, JobOrderId hidden; Rate/Amount without the right;
       "Edit" button first. DocNo is also a link to the same Edit (page contract). */
    var HIST_COLS = [
        { k: 'DocNo', w: 60, link: true }, { k: 'DocDate', w: 90, d: true }, { k: 'JobOrder', w: 150 },
        { k: 'PlantName', w: 130 }, { k: 'Item', w: 180 }, { k: 'WareHouseName', w: 130 }, { k: 'RackName', w: 100 },
        { k: 'ItemCondition', w: 100 }, { k: 'BrandName', w: 180 }, { k: 'BrandUom', w: 70 },
        { k: 'Qty', w: 80, n: 'q', sum: true }, { k: 'Rate', w: 80, n: 'r', rate: true },
        { k: 'Amount', w: 100, n: 'a', sum: true, rate: true }, { k: 'Remarks', w: 200 },
        { k: 'ContractScheduleNo', w: 130 }, { k: 'IsApproved', w: 90 }, { k: 'JobPlanStatus', w: 100 }
    ];

    /** BindHistoryPMGrid:1386-1413 - the procedure's columns renamed as the desktop's table does. */
    function histRow(s) {
        return {
            Id: int(col(s, 'Id')), DocNo: col(s, 'DocNo'), DocDate: col(s, 'DocDate'),
            JobOrderId: int(col(s, 'InvProductionJobOrderId')), JobOrder: col(s, 'RefInvoiceNo'),
            PlantName: col(s, 'PlantName'), Item: col(s, 'ItemName'), WareHouseName: col(s, 'WareHouseName'),
            RackName: col(s, 'rackName'), ItemCondition: col(s, 'ItemCondition'), BrandName: col(s, 'BrandName'),
            BrandUom: col(s, 'BrandUom'), Qty: col(s, 'Qty'), Rate: col(s, 'Rate'), Amount: col(s, 'Amount'),
            Remarks: col(s, 'pmRemarks'), ContractScheduleNo: col(s, 'ContractScheduleNo'),
            IsApproved: col(s, 'JobOrderStatus'), JobPlanStatus: col(s, 'JobPlanStatus')
        };
    }

    function renderHistory() {
        var cols = HIST_COLS.filter(function (c) { return !c.rate || R.rateAndAmount; });
        $id('historyHead').innerHTML = '<th style="width:40px;">Edit</th>' + cols.map(function (c) {
            return '<th style="min-width:' + c.w + 'px;">' + esc(c.k) + '</th>';
        }).join('');
        $id('historyBody').innerHTML = HIST_ROWS.map(function (r, i) {
            return '<tr data-h="' + i + '"><td class="ctr"><button type="button" class="pm-edit" data-edit="' + i + '">Edit</button></td>'
                + cols.map(function (c) {
                    var v = c.d ? K.dMMMyyyy(r[c.k]) : cellFmt(c, r[c.k]);
                    if (c.link) return '<td class="ctr"><span class="win-doc-link" data-edit="' + i + '">' + esc(v) + '</span></td>';
                    return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('historyFoot').innerHTML = HIST_ROWS.length ? '<tr><td></td>' + cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            var s = 0; HIST_ROWS.forEach(function (r) { s += dbl(r[c.k]); });
            return '<td class="num">' + esc(cellFmt(c, s)) + '</td>';
        }).join('') + '</tr>' : '';
        K.filterRow($id('grdPackingMaterialHistory'));
    }

    // ============================================================================ save

    /** FormValidationOfPackingMaterial:483. */
    function formValidation() {
        if (val('txtDocNoPackingMaterial').trim() === '') { box('Doc No. Field Required'); $id('txtDocNoPackingMaterial').focus(); return false; }
        if (!active('cmbJobOrderPackingMaterial')) { box('Job Order No. Field Required'); focusCombo('cmbJobOrderPackingMaterial'); return false; }
        return true;
    }

    /** PackingMaterialInsert:1096 - the row loop and the BLL/DAL run on the server. */
    function packingMaterialInsert() {
        if (!formValidation()) return Promise.resolve();
        if (!window.confirm(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
        var body = {
            recId: RECID, jobOrderId: cval('cmbJobOrderPackingMaterial'), scheduleRecId: SCHED_REC,
            rows: ROWS.map(function (r) {
                return { id: r.Id, docDate: dateOnly(r.DocDate), docNo: r.DocNo, plantId: r.PlantId, packingItemId: r.PackingItemId,
                    warehouseId: r.WarehouseId, rackId: r.RackId, rackName: r.RackName, itemConditionId: r.ItemConditionId,
                    itemCondition: r.ItemCondition, brandId: r.BrandId, brandUomId: r.BrandUomId, itemQty: r.ItemQty,
                    itemRate: r.ItemRate, amount: r.Amount, remarks: r.Remarks, contractScheduleId: r.ContractScheduleId,
                    contractScheduleNo: r.ContractScheduleNo };
            })
        };
        return postJson(api + '/save', body).then(function (d) {
            if (!d || !d.success) { box((d && d.message) || 'Save failed.'); return; }
            box(d.message);
            say(d.message);
            return formReset();
        });
    }

    /** btnsavepackingmaterial_Click:1228 - RecpackingMaterial is zeroed first, whatever follows. */
    function btnSave() { RECID = 0; return busy('btnsavepackingmaterial', packingMaterialInsert); }
    /** btnUpdatepackingmaterial_Click:1241. */
    function btnUpdate() { return busy('btnUpdatepackingmaterial', packingMaterialInsert); }

    // ========================================================================== read by id

    /** ReadByIdPackingMaterial:1253. Save/Update swap and RecpackingMaterial are set BEFORE the
     *  approved/status checks, so a refused record still leaves the form in Update mode on the
     *  rows it already had (desktop behaviour, reproduced). */
    function readById(id) {
        RECID = int(id);
        show('btnsavepackingmaterial', false); show('btnUpdatepackingmaterial', true);
        say('Opening ' + RECID + '...');
        return getJson(api + '/' + RECID).then(function (d) {
            if (!d || !d.found) throw new Error('Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index');
            var o = d.row;
            var ap = col(o, 'IsApproved');
            if (ap === true || ap === 1 || String(ap).toLowerCase() === 'true') throw new Error("You Can't Update Approved Record");
            var st = col(o, 'PlanStatus'); st = st === null || st === undefined ? '' : String(st);
            if (st !== 'In Process') throw new Error("You Can't Update Record with " + st + ' JobPlanStatus');
            selectTab(0);
            ROWS = []; CUR = -1;
            setVal('txtdocdatePackingMaterial', dateOnly(col(o, 'DocDate'))); DOC_TIME = '00:00:00';
            setVal('txtDocNoPackingMaterial', col(o, 'DocNo'));
            setCombo('cmbJobOrderPackingMaterial', col(o, 'InvProductionJobOrderId'));
            return jobOrderLeave().then(function () {
                $id('cmbJobOrderPackingMaterial').disabled = true;
                ROWS.push({
                    Id: int(col(o, 'Id')), DocDate: dateOnly(col(o, 'DocDate')), DocNo: int(col(o, 'DocNo')),
                    InvProductionJobOrderId: int(col(o, 'InvProductionJobOrderId')), PlantId: int(col(o, 'PlantId')),
                    Plant: col(o, 'PlantName'), PackingItemId: int(col(o, 'ItemId')), PackingItem: col(o, 'ItemName'),
                    WarehouseId: int(col(o, 'WarehouseId')), Warehouse: col(o, 'WareHouseName'), RackId: int(col(o, 'RackId')),
                    RackName: col(o, 'rackName'), ItemConditionId: int(col(o, 'ItemConditionId')), ItemCondition: col(o, 'ItemCondition'),
                    BrandId: int(col(o, 'BrandId')), Brand: col(o, 'BrandName'), BrandUomId: int(col(o, 'BrandUomId')),
                    BrandUom: col(o, 'BrandUom'), ItemQty: dbl(col(o, 'Qty')), ItemRate: dbl(col(o, 'Rate')),
                    Amount: dbl(col(o, 'Amount')), Remarks: col(o, 'pmRemarks') || '',
                    ContractScheduleId: int(col(o, 'ContractScheduleId')), ContractScheduleNo: col(o, 'ContractScheduleNo') || ''
                });
                CUR = 0;
                renderDetail();
                VOUCHER_HEAD_ID = int(d.voucherHeadId);          // GetVoucherHeadId(RecpackingMaterial, 111)
                resetDetail();
                say('Doc No ' + col(o, 'DocNo') + ' opened for update');
            });
        }).catch(function (e) { say('Ready'); box(e.message); });
    }

    /** grdPackingMaterialHistory_ColumnButtonClick:1889 / grdPackingMaterial_DoubleClick:1469. */
    function editHistoryRow(i) {
        var r = HIST_ROWS[i];
        if (!r) return;
        show('btnsavepackingmaterial', false); show('btnUpdatepackingmaterial', true);
        if (String(r.IsApproved) === 'Approved') { box("You Can't Update Approved Record"); return; }
        if (String(r.JobPlanStatus === null || r.JobPlanStatus === undefined ? '' : r.JobPlanStatus) !== 'In Process') {
            box("You Can't Update Record with " + (r.JobPlanStatus || '') + ' JobPlanStatus'); return;
        }
        RECID = r.Id;
        readById(RECID);
    }

    // ============================================================================ history

    function dateMode() { var e = document.querySelector('input[name="pmDateMode"]:checked'); return e ? e.value : 'doc'; }

    /** BindHistoryPMGrid:1324. */
    function bindHistory() {
        var from = $id('chkFromDateHistory').checked && val('FromDateHistory') ? val('FromDateHistory') + 'T' + HIST_TIME : '';
        var to = $id('chkToDateHistory').checked && val('ToDateHistory') ? val('ToDateHistory') + 'T' + HIST_TIME : '';
        return getJson(api + '/history?' + q({ pendingForRates: $id('chkPendingForRates').checked, dateMode: dateMode(),
                fromDate: from, toDate: to, docNoFrom: val('txtFromDocNoHistory'), docNoTo: val('txtToDocNoHistory'),
                jobOrderId: cval('CmbJobOrder') }))
            .then(function (rows) {
                HIST_ROWS = (rows || []).map(histRow);
                renderHistory();
                say(HIST_ROWS.length + ' record(s)');
            }).catch(function (e) { box(e.message); });
    }

    /** btnNewHistory_Click:2120. */
    function newHistory() {
        var t = today();
        setVal('FromDateHistory', addDays(t, -3)); setVal('ToDateHistory', t); HIST_TIME = nowTime();
        setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', '');
        clearCombo('CmbJobOrder');
        HIST_ROWS = []; renderHistory();
        $id('drdocdate').checked = true;
    }

    /** btnRefreshHistory_Click:2139. */
    function refreshHistory() {
        return getJson(api + '/history-job-orders').then(historyComboBind);
    }

    // =========================================================================== toolbar

    /** btnRefreshPackingMaterial_Click:1000 - re-reads the globals, then the four binds. */
    function refresh() {
        return getJson(api + '/refresh?' + q({ recId: RECID })).then(function (d) {
            RACKS_ITEM = null;
            jobOrderNoFill(d.jobOrders);
            itemFill(d.items);
            var p = itemLeave();
            itemConditionBind(d.itemConditions);
            SCHEDULES = d.schedules || []; SCHED_REC = int(d.scheduleRecId);
            scheduleNoBind();
            return p;
        });
    }

    /** btnPackingMaterialSlip_Click:1492 - the CURRENT detail row's job order; no row is the
     *  desktop's NullReferenceException. */
    function print606() {
        var r = ROWS[CUR];
        if (!r) { box('Object reference not set to an instance of an object.'); return Promise.resolve(); }
        var jo = int(r.InvProductionJobOrderId);
        var win = window.CrystalPrint.reserve();
        return getJson(api + '/print-606-rows?' + q({ jobOrderId: jo })).then(function (d) {
            if (!d || int(d.rows) === 0) { window.CrystalPrint.release(win); box('Record Not Found For DisPlay'); return; }
            return window.CrystalPrint.open('prod-606-pm', { jobOrderId: jo }, null, win);
        }).catch(function (e) { window.CrystalPrint.release(win); throw e; });
    }

    /** btnVoucherPM_Click:2068 -> CommonServices.VoucherReport_118(VoucherHeadId). */
    function voucher118() {
        if (VOUCHER_HEAD_ID === 0) { box('VoucherId Not Found'); return Promise.resolve(); }
        return window.CrystalPrint.open('acc-118', { id: VOUCHER_HEAD_ID, documentTypeId: 0 });
    }

    /** btnGenerateRate_Click:2043 -> AverageCalculate:1964 (non-FIFO): every row takes the form's
     *  date and, when a rate > 0 comes back, Rate and Amount = Qty x Rate (not rounded). */
    function generateRate() {
        if (!ROWS.length) return Promise.resolve();
        var d = val('txtdocdatePackingMaterial') || today();
        var chain = Promise.resolve();
        ROWS.forEach(function (r) {
            chain = chain.then(function () {
                r.DocDate = d;
                return getJson(api + '/avg-rate?' + q({ itemId: r.PackingItemId, docDate: docDateTime(),
                        itemConditionId: r.ItemConditionId, recId: RECID }))
                    .then(function (x) {
                        var rate = x ? dbl(x.avgRate) : 0;
                        if (rate > 0) { r.ItemRate = rate; r.Amount = dbl(r.ItemQty) * rate; }
                    });
            });
        });
        return chain.then(renderDetail);
    }

    // ============================================================================ tabs

    function selectTab(i) {
        var was = $id('PmHistory').classList.contains('is-active') ? 1 : 0;
        $id('PmForm').classList.toggle('is-active', i === 0);
        $id('PmHistory').classList.toggle('is-active', i === 1);
        $id('tabPmForm').classList.toggle('is-active', i === 0);
        $id('tabPmHistory').classList.toggle('is-active', i === 1);
        if (i === 1 && was !== 1) tabChangedToHistory();
    }
    /** tabControlPm_SelectedIndexChanged:1595 - unchecking fires CheckedChanged only when it was on. */
    function tabChangedToHistory() {
        show('wrapPendingForRates', !!R.rateAndAmount);
        var c = $id('chkPendingForRates');
        if (c.checked) { c.checked = false; bindHistory(); }
    }
    function onForm() { return $id('PmForm').classList.contains('is-active'); }

    function toggleFullscreen(id) { var b = $id(id); if (b) b.classList.toggle('is-fullscreen'); }

    /** Close() - the form closes inside the shell's panel; standalone it returns to the hub. */
    function closeForm() {
        if (window.parent && window.parent !== window) {
            if (typeof window.parent.P280CloseTab === 'function') window.parent.P280CloseTab('PackingMaterial');
            return;
        }
        K.close();
    }

    // ============================================================================ wiring

    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    function wire() {
        on('btnNewpackingmaterial', 'click', function () { return busy('btnNewpackingmaterial', formReset); });
        on('btnRefreshPackingMaterial', 'click', function () { return busy('btnRefreshPackingMaterial', refresh); });
        on('btnsavepackingmaterial', 'click', btnSave);
        on('btnUpdatepackingmaterial', 'click', btnUpdate);
        on('btnPackingMaterialSlip', 'click', function () { return busy('btnPackingMaterialSlip', print606); });
        on('btnVoucherPM', 'click', function () { return busy('btnVoucherPM', voucher118); });
        on('btnGenerateRate', 'click', function () { return busy('btnGenerateRate', generateRate); });
        on('btnAddDetailPm', 'click', function () { return busy('btnAddDetailPm', addDetail); });
        on('btnUpdateDetailPm', 'click', function () { return busy('btnUpdateDetailPm', updateDetail); });
        on('btnCancelDetailPm', 'click', resetDetail);

        /* Leave handlers. A pick fires `change`; that is where the desktop's Leave work runs. */
        on('cmbJobOrderPackingMaterial', 'change', jobOrderLeave);
        on('CmbPlantFeederPackingMaterial', 'change', plantLeave);
        on('cmbItemPackingMaterial', 'change', itemLeave);
        on('CmbWarehousePackingMaterial', 'change', warehouseLeave);
        on('CmbRackName', 'change', rackLeave);
        on('cmbItemCondition', 'change', itemConditionLeave);
        on('CmbBrandNamePackingMaterial', 'change', brandLeave);
        on('CmbBrandUomPackingMaterial', 'change', brandUomLeave);
        on('txtPackingMaterialQTY', 'input', function () { RATE_TOKEN++; qtyTextChanged(); });
        on('txtPackingMaterialQTY', 'change', function () { getAvgRate(); });          // txtPackingMaterialQTY_Leave:1924
        on('txtPackingMaterialRate', 'input', function () { RATE_TOKEN++; rateTextChanged(); });
        on('txtdocdatePackingMaterial', 'change', function () { DOC_TIME = nowTime(); }); // ValueChanged:1936 is empty

        /* detail grid: current row, double click = edit, X = delete */
        on('detailBody', 'click', function (e) {
            var del = e.target.closest('[data-del]');
            if (del) { deleteDetailRow(int(del.getAttribute('data-del'))); return; }
            var tr = e.target.closest('tr[data-i]');
            if (tr) { CUR = int(tr.getAttribute('data-i')); renderDetail(); }
        });
        on('detailBody', 'dblclick', function (e) {
            if (e.target.closest('[data-del]')) return;
            var tr = e.target.closest('tr[data-i]');
            if (tr) editDetailRow(int(tr.getAttribute('data-i'))).catch(function (x) { box(x.message); });
        });

        on('historyBody', 'click', function (e) {
            var b = e.target.closest('[data-edit]');
            if (b) editHistoryRow(int(b.getAttribute('data-edit')));
        });
        on('historyBody', 'dblclick', function (e) {
            if (e.target.closest('[data-edit]')) return;
            var tr = e.target.closest('tr[data-h]');
            if (tr) editHistoryRow(int(tr.getAttribute('data-h')));
        });
        on('btnshow', 'click', function () { return busy('btnshow', bindHistory); });
        on('btnNewHistory', 'click', newHistory);
        on('btnRefreshHistory', 'click', function () { return busy('btnRefreshHistory', refreshHistory); });
        on('chkPendingForRates', 'change', bindHistory);
        on('tabPmForm', 'click', function () { selectTab(0); });
        on('tabPmHistory', 'click', function () { selectTab(1); });
        Array.prototype.forEach.call(document.querySelectorAll('[data-fullscreen]'), function (b) {
            b.addEventListener('click', function () { toggleFullscreen(b.getAttribute('data-fullscreen')); });
        });

        /* frmFoodProduction_KeyDown:445 */
        K.enterToTab();
        K.keys({
            'ctrl+e': closeForm, 'esc': closeForm,
            'ctrl+s': function () { if (onForm() && visible('btnsavepackingmaterial') && enabled('btnsavepackingmaterial')) btnSave(); },
            'ctrl+u': function () { if (onForm() && visible('btnUpdatepackingmaterial') && enabled('btnUpdatepackingmaterial')) btnUpdate(); },
            'ctrl+n': function () { if (onForm()) busy('btnNewpackingmaterial', formReset); },
            'ctrl+r': function () { if (onForm()) busy('btnRefreshPackingMaterial', refresh); }
        });
    }

    function boot() {
        setVal('txtdocdatePackingMaterial', today());
        /* DateTimePicker defaults: both history pickers start at "now", ticked; Doc Date radio. */
        setVal('FromDateHistory', today()); setVal('ToDateHistory', today());
        renderHistory();
        wire();
        load().then(function () {
            BOOTED = true;
            if (PENDING_READ !== null) { var id = PENDING_READ; PENDING_READ = null; window.P280ReadById(id); }
        });
    }

    /** Shell -> this page: PackingMaterialForm.tabControlPm.SelectedIndex = 0;
     *  PackingMaterialForm.ReadByIdPackingMaterial(e.Id). Queued until Load has finished. */
    window.P280ReadById = function (id) {
        if (!BOOTED) { PENDING_READ = id; return; }
        selectTab(0);
        return readById(id);
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
