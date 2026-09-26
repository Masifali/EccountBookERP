/* ============================================================================================
 * Screen 280 "Production Against (Job Order)" - Overhead tab.
 * Desktop form: Architecture.WinApp.Production/frmProductionOverhead.cs (DocumentTypeId 110).
 *
 * The page keeps tableOverHeads in memory exactly as the form does, runs every event the form
 * wires (Load, the Leave handlers, TextChanged, CheckedChanged, ValueChanged, KeyDown, grid
 * double-click / column buttons, tab change), and sends the grid to the server on Save/Update,
 * where OverHeadInsert's checks run again before DAL SetData posts the rows and their vouchers.
 *
 * Line numbers are frmProductionOverhead.cs's.
 *
 * Embedding contract: window.P280ReadById(id) selects the first inner tab and runs
 * ReadByIdOverHeadJobOrderWise(id) (FoodProductionWithValues.SettlementForm_OnOverHeadReadById).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/production-against-job-order/overhead';
    var K = window.ReportKit;

    /* ------------------------------------------------------------------ form-level fields */
    var UpdateMode = false;
    var RecOverHeadId = 0;
    var VoucherHeadId = 0;
    var updateDetailIndexOH = -1;
    var tableOverHeads = [];          // the DataTable of :237-257
    var currentDetail = -1;           // grdDetailOverhead.CurrentRow
    var OverHeadHistory = [];         // grouped rows of BindGridHistoryOverHead
    var currentHistory = -1;
    var RIGHTS = { save: false, update: false, print: false, rateAndAmount: false };
    var TOOL = {};                    // each toolbar button's own Enabled / Visible
    var LISTS = { jobOrders: [], plants: [], brands: [], brandUoms: [], rateUoms: [], coa: [], charges: [] };
    var pending = Promise.resolve();  // Leave handlers run to completion before a click acts

    /* ---------------------------------------------------------------------------- helpers */
    function $(id) { return document.getElementById(id); }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $('lblStatus'); if (e) e.textContent = m || ''; }
    function msg(m) { window.alert(m); }
    function esc(s) { return K.esc(s); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var ln = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === ln) return row[k];
        return null;
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    /** Conversion.ToDouble - unparsable is 0. */
    function toDouble(v) {
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var n = parseFloat(str(v).replace(/,/g, '').trim());
        return isNaN(n) ? 0 : n;
    }
    /** Conversion.ToInt - unparsable is 0; a double rounds half to even (Convert.ToInt32). */
    function toInt(v) { return Math.trunc(roundEven(toDouble(v))); }
    function roundEven(x) {
        if (!isFinite(x)) return x;
        var f = Math.abs(x % 1);
        if (f === 0.5) return 2 * Math.round(x / 2);
        return Math.round(x);
    }
    /** double.Parse(text) - NumberStyles.Float | AllowThousands; throws like the desktop. */
    function netParse(s) {
        var t = str(s);
        if (!/^\s*[+-]?(?=[\d.,]*\d)[\d,]*(\.\d*)?([eE][+-]?\d+)?\s*$/.test(t) || /^\s*[+-]?,/.test(t)) {
            throw new Error('Input string was not in a correct format.');
        }
        return parseFloat(t.replace(/,/g, '').trim());
    }
    /** double.ToString() for the rounded amounts (.NET Framework). */
    function netToString(n) {
        if (isNaN(n)) return 'NaN';
        if (n === Infinity) return 'Infinity';
        if (n === -Infinity) return '-Infinity';
        if (n === 0) return '0';
        if (Math.abs(n) < 1e15 && Number.isInteger(n)) return String(n);
        return String(Number(n.toPrecision(15))).replace('e+', 'E+').replace('e-', 'E-');
    }
    /**
     * .NET custom numeric formats used by this form: "#,#", "#,##0", "0,0", "#,#.##",
     * "#,##0.###", "#,##0.####". Midpoints round away from zero; "#" drops a zero integer part.
     */
    function netFormat(v, fmt) {
        var n = typeof v === 'number' ? v : toDouble(v);
        var parts = fmt.split('.');
        var ip = parts[0], dp = parts[1] || '';
        var minInt = (ip.match(/0/g) || []).length;
        var group = ip.indexOf(',') >= 0 && /[#0],[#0]/.test(ip);
        var minDec = (dp.match(/0/g) || []).length, maxDec = dp.length;
        var a = Number(Math.abs(n).toPrecision(15));
        var p = Math.pow(10, maxDec);
        var r = Math.floor(a * p + 0.5 + 1e-9) / p;
        var fixed = r.toFixed(maxDec);
        var ipart = fixed.split('.')[0], fpart = fixed.split('.')[1] || '';
        while (fpart.length > minDec && fpart.charAt(fpart.length - 1) === '0') fpart = fpart.slice(0, -1);
        if (ipart === '0') ipart = '';
        while (ipart.length < minInt) ipart = '0' + ipart;
        if (group && ipart) ipart = ipart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ipart + (fpart ? '.' + fpart : '');
        if (n < 0 && out !== '' && /[1-9]/.test(out)) out = '-' + out;
        return out;
    }
    function today() {
        var d = new Date();
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    function dateOnly(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v));
        return m ? m[1] + '-' + m[2] + '-' + m[3] : '';
    }

    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hh = document.querySelector('meta[name="_csrf_header"]');
        if (t && hh) h[hh.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(readJson);
    }
    function postJson(url, body) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body)
        }).then(readJson);
    }
    function readJson(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
            return body;
        });
    }
    function qs(o) {
        return Object.keys(o).map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }

    /* The button contract: disabled at once, spinner, duplicates ignored, re-enabled on
       success AND failure. */
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $(btn) : btn;
        if (b) {
            if (b.classList.contains('is-busy')) return Promise.resolve();
            b.dataset.ohWas = b.disabled ? '1' : '';
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = b.dataset.ohWas === '1'; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { done(); msg(e.message); return Promise.resolve(); }
        return p.then(done, function (e) { done(); if (e && e.message) msg(e.message); });
    }
    /** Run after any Leave work in flight, as the desktop's synchronous Leave runs before a Click. */
    function after(fn) {
        var p = pending.then(fn, fn);
        pending = p.catch(function () { });
        return p;
    }
    function chain(fn) { return after(fn); }

    /* ------------------------------------------------------------------------- combo fill */
    if (window.DesktopCombo) {
        /* UOMOverHead:633 - DDL.BindDDL hides only column 0, so UOM and Equivalent both show. */
        window.DesktopCombo.define('ohRateUom', [
            { caption: 'Rate UOM', flex: 2 },
            { caption: 'Equivalent', flex: 2, key: 'eq', type: 'num' }
        ]);
    }
    /** DDL.BindDDL / BindDDLNew with ZeroIndex false: no "Select" row. The blank option is the
        combo with no ActiveRow. */
    function fill(id, rows, valueKey, textKey, attrs) {
        var sel = $(id);
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            if (attrs) for (var a in attrs) extra += ' data-' + a + '="' + esc(col(r, attrs[a])) + '"';
            html += '<option value="' + esc(col(r, valueKey)) + '"' + extra + '>' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel.value = '';
    }
    function clear(id) { var s = $(id); s.innerHTML = '<option value=""></option>'; s.value = ''; }
    function hasRow(id) { return val(id) !== ''; }
    function text(id) {
        var s = $(id);
        return s && s.selectedIndex >= 0 && s.value !== '' ? s.options[s.selectedIndex].textContent.trim() : '';
    }
    function setCombo(id, v) {
        var s = $(id), want = str(v);
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === want) { s.value = want; return true; }
        s.value = '';
        return false;
    }
    function enable(id, on) { var e = $(id); if (e) e.disabled = !on; }
    function show(id, on) { var e = $(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focus(id) { var s = $(id); if (!s) return; var w = s.closest('.dtcombo-wrap'); var i = w ? w.querySelector('input') : s; try { i.focus(); } catch (e) { } }
    function docTypeOfSide() { return $('RadInputItemOH').checked ? 80 : 112; }

    /* ------------------------------------------------------------------ toolbar and rights */

    /** toolStrip5.Enabled = Print right (:234): a disabled strip disables every item in it,
        and a ToolStripItem reports Enabled false while its strip is disabled. */
    function paintToolbar() {
        var strip = !!RIGHTS.print;
        ['btnNewOverHead', 'btnRefreshOverHead', 'btnSaveOverHead', 'btnUpdateOverHead', 'btnPrintOverHead',
         'btnVoucherOH', 'BtnLoadFohOverHeads', 'btnOverHeadFormulas'].forEach(function (id) {
            var t = TOOL[id] || { enabled: true, visible: true };
            var b = $(id);
            if (!b.classList.contains('is-busy')) b.disabled = !(t.enabled && strip);
            b.classList.toggle('is-hidden', !t.visible);
        });
    }
    function tool(id, patch) { TOOL[id] = Object.assign(TOOL[id] || { enabled: true, visible: true }, patch); paintToolbar(); }
    function toolUsable(id) { var t = TOOL[id] || {}; return t.visible !== false && t.enabled !== false && !!RIGHTS.print; }

    /* ----------------------------------------------------------------- frmFoodProduction_Load */

    function load() {
        tool('btnUpdateOverHead', { visible: false });
        return getJson(API + '/state').then(function (s) {
            RIGHTS = s.rights || RIGHTS;
            tool('btnSaveOverHead', { enabled: !!RIGHTS.save });           // :231
            $('btnUpdateOH').disabled = !RIGHTS.update;                      // :232 - the DETAIL button
            tool('btnPrintOverHead', { enabled: !!RIGHTS.print });          // :233
            tool('btnVoucherOH', { enabled: !!RIGHTS.print });              // :235
            if (!RIGHTS.rateAndAmount) tool('btnVoucherOH', { visible: false }); // :269-272
            renderDetail();                                                  // grdDetailOverheadSettings
            if (s.docNo > 0) setVal('txtDocNoOverHead', s.docNo);            // GenerateCodeofOverHead
            LISTS.coa = s.chartOfAccounts || [];
            fill('cmbChartofAccountOverHead', LISTS.coa, 'Id', 'AccountTitle');
            LISTS.rateUoms = s.rateUoms || [];
            fill('cmbRateUomOverHeads', LISTS.rateUoms, 'Id', 'UOM', { eq: 'Equivalent' });
            LISTS.charges = s.chargesTypes || [];
            fill('cmbChargestypeOverheads', LISTS.charges, 'Id', 'Type');
            LISTS.jobOrders = s.jobOrders || [];
            fill('cmbJobOrderOverHead', LISTS.jobOrders, 'Id', 'PlanCode', { doctype: 'DocumentTypeId' });
            show('btnAddOH', true); show('btnUpdateOH', false); show('BtnCancelOH', false);
            $('txtDocDateOverHead').focus();
        }).catch(function (e) { msg(e.message); });
    }

    /* ------------------------------------------------------------------------- the lookups */

    /** GetBrandItemsForPackingAndOverHeads:281 */
    function getBrandItems(jobOrderId) {
        return getJson(API + '/brand-items?' + qs({ jobOrderId: jobOrderId, documentTypeId: docTypeOfSide() }))
            .then(function (rows) {
                LISTS.brands = rows || [];
                if (LISTS.brands.length > 0) fill('CmbBrandNameOverHead', LISTS.brands, 'ItemId', 'ItemName');
                else clear('CmbBrandNameOverHead');
            }).catch(function (e) { msg(e.message); });
    }
    /** GetItemUomByItemIdFromFinishGoods:311 - value ItemUomId, display Equivalent. */
    function getItemUoms(jobOrderId, itemId) {
        return getJson(API + '/brand-uoms?' + qs({ jobOrderId: jobOrderId, itemId: itemId }))
            .then(function (rows) {
                LISTS.brandUoms = rows || [];
                var keep = val('CmbBrandUomOverHead');
                if (LISTS.brandUoms.length > 0) {
                    fill('CmbBrandUomOverHead', LISTS.brandUoms, 'ItemUomId', 'Equivalent');
                    if (keep) setCombo('CmbBrandUomOverHead', keep);
                } else clear('CmbBrandUomOverHead');
            }).catch(function (e) { msg(e.message); });
    }
    /** GetPlantFeeder:385 - one row is activated; a previous value is kept. */
    function getPlantFeeder(jobOrderId) {
        var keep = toInt(val('CmbPlantFeederOverHead'));
        return getJson(API + '/plants?' + qs({ jobOrderId: jobOrderId })).then(function (rows) {
            LISTS.plants = rows || [];
            if (LISTS.plants.length > 0) {
                fill('CmbPlantFeederOverHead', LISTS.plants, 'PlantId', 'PlantName');
                if (LISTS.plants.length === 1) setCombo('CmbPlantFeederOverHead', col(LISTS.plants[0], 'PlantId'));
                if (keep > 0) setCombo('CmbPlantFeederOverHead', keep);
            } else {
                clear('CmbPlantFeederOverHead');
            }
        }).catch(function (e) { msg(e.message); });
    }
    /** JobOrderNoFill:342 - keeps the chosen job order. */
    function jobOrderNoFill(rows) {
        var keep = toInt(val('cmbJobOrderOverHead'));
        LISTS.jobOrders = rows || [];
        if (LISTS.jobOrders.length > 0) {
            fill('cmbJobOrderOverHead', LISTS.jobOrders, 'Id', 'PlanCode', { doctype: 'DocumentTypeId' });
            if (keep > 0) setCombo('cmbJobOrderOverHead', keep);
        }
    }
    /** GenerateCodeofOverHead:536 */
    function generateCode() {
        return getJson(API + '/next-code').then(function (r) {
            if (r && r.docNo > 0) setVal('txtDocNoOverHead', r.docNo);
        }).catch(function (e) { msg(e.message); });
    }

    /** GetTotlInPutOut:658 - "#,#" formats (a zero total shows as an empty box). */
    function getTotlInPutOut() {
        return getJson(API + '/totals?' + qs({ jobOrderId: toInt(val('cmbJobOrderOverHead')) })).then(function (rows) {
            rows = rows || [];
            if (rows.length > 0) {
                var InQty = 0, InWeight = 0, OutQty = 0, OutWeight = 0, FinQty = 0, FinWeight = 0;
                rows.forEach(function (r) {
                    if (str(col(r, 'EntryTypeOutPut')) === 'FinishGoods') {
                        FinQty += toDouble(col(r, 'ItemQty'));
                        FinWeight += toDouble(col(r, 'StockWeight'));
                    }
                    if (str(col(r, 'EntryType')) === 'Input') {
                        InQty += toDouble(col(r, 'ItemQty'));
                        InWeight += toDouble(col(r, 'StockWeight'));
                    } else {
                        OutQty += toDouble(col(r, 'ItemQty'));
                        OutWeight += toDouble(col(r, 'StockWeight'));
                    }
                });
                setVal('txtInputQtyTotalOH', netFormat(InQty, '#,#'));
                setVal('txtInputWeightTotalOH', netFormat(InWeight, '#,#'));
                setVal('txtOutputQtyTotalOH', netFormat(OutQty, '#,#'));
                setVal('txtOutputWeightTotalOH', netFormat(OutWeight, '#,#'));
                setVal('txtfinishQtyOh', netFormat(FinQty, '#,#'));
                setVal('txtfinishWeightOh', netFormat(FinWeight, '#,#'));
            } else {
                ['txtInputQtyTotalOH', 'txtInputWeightTotalOH', 'txtOutputQtyTotalOH', 'txtOutputWeightTotalOH',
                 'txtfinishQtyOh', 'txtfinishWeightOh'].forEach(function (id) { setVal(id, '0'); });
            }
        });
    }

    /** GetOutPutQtyByJobOrderandItemIdOverHeads:736 - Input side sums BalanceWeight, Output ItemQty; "0,0". */
    function getOutPutQty() {
        return getJson(API + '/output-qty?' + qs({
            jobOrderId: toInt(val('cmbJobOrderOverHead')), brandId: toInt(val('CmbBrandNameOverHead')),
            brandUomId: toInt(val('CmbBrandUomOverHead')), documentTypeId: docTypeOfSide()
        })).then(function (rows) {
            rows = rows || [];
            if (rows.length > 0) {
                var input = $('RadInputItemOH').checked, sum = 0;
                rows.forEach(function (r) { sum += toDouble(col(r, input ? 'BalanceWeight' : 'ItemQty')); });
                setVal('txtItemQtyOverHead', netFormat(sum, '0,0'));
            } else {
                setVal('txtItemQtyOverHead', '');
            }
            calculate();                                   // txtItemQtyOverHead_TextChanged
        });
    }

    /* ----------------------------------------------------------- CaluculatonOfOverHeads:491 */

    function rateUomEquivalent() {
        var s = $('cmbRateUomOverHeads');
        var o = s.options[s.selectedIndex];
        return o ? toDouble(o.getAttribute('data-eq')) : 0;
    }
    function calculate() {
        try {
            var qty = val('txtItemQtyOverHead'), rate = val('txtItemRateOverHeads');
            if (toInt(val('cmbChargestypeOverheads')) === 1) {
                if (toInt(val('CmbBrandNameOverHead')) > 0 && hasRow('CmbBrandNameOverHead')
                        && toInt(val('CmbBrandUomOverHead')) > 0 && hasRow('CmbBrandUomOverHead')) {
                    if (qty !== '' && rate !== '') {
                        setVal('txtExpAmountOverHead', netToString(roundEven(netParse(qty) * netParse(rate))));
                    }
                } else if (qty !== '' && rate !== '' && hasRow('cmbRateUomOverHeads')) {
                    /* Total InPut Weight / Rate UOM Equivalent x Rate. The weight box is "#,#"
                       formatted, so a zero total is an empty string and double.Parse throws. */
                    var w = netParse(val('txtInputWeightTotalOH'));
                    var eq = rateUomEquivalent();
                    var r = netParse(rate);
                    setVal('txtExpAmountOverHead', netToString(roundEven(w / eq * r)));
                }
            } else if (qty !== '' && rate !== '') {
                setVal('txtExpAmountOverHead', netToString(roundEven(netParse(qty) * netParse(rate))));
            }
        } catch (e) {
            msg(e.message);
        }
    }

    /* ----------------------------------------------------------------------- Leave events */

    /** cmbJobOrderOverHead_Leave:642 */
    function jobOrderLeave() {
        var j = toInt(val('cmbJobOrderOverHead'));
        return getBrandItems(j)
            .then(function () { return getPlantFeeder(j); })
            .then(function () { return getTotlInPutOut(); })
            .then(function () { calculate(); refreshPlantInOhGrid(); })
            .catch(function (e) { msg(e.message); });
    }
    /** CmbBrandNameOverHead_Leave:722 */
    function brandLeave() {
        return getItemUoms(toInt(val('cmbJobOrderOverHead')), toInt(val('CmbBrandNameOverHead')))
            .then(function () { return getOutPutQty(); })
            .then(function () { calculate(); })
            .catch(function (e) { msg(e.message); });
    }
    /** CmbBrandUomOverHead_Leave:773 - no try/catch on the desktop; the error still surfaces. */
    function brandUomLeave() {
        return getOutPutQty().then(function () { calculate(); }).catch(function (e) { msg(e.message); });
    }
    /** cmbChargestypeOverheads_Leave:1660 */
    function chargesTypeLeave() {
        enable('txtExpAmountOverHead', toInt(val('cmbChargestypeOverheads')) !== 1);
        calculate();
    }
    /** txtItemQtyOverHead_Leave:1685 - "#,##0" (and TextChanged recalculates). */
    function qtyLeave() {
        setVal('txtItemQtyOverHead', netFormat(toDouble(val('txtItemQtyOverHead')), '#,##0'));
        calculate();
    }

    var LEAVE = {
        cmbJobOrderOverHead: jobOrderLeave,
        CmbBrandNameOverHead: brandLeave,
        CmbBrandUomOverHead: brandUomLeave,
        cmbChargestypeOverheads: chargesTypeLeave,
        cmbRateUomOverHeads: function () { calculate(); }
    };
    /* A combo's Leave: focus leaving the combo (its text box or the native select). */
    document.addEventListener('focusout', function (e) {
        var t = e.target;
        var wrap = t && t.closest ? t.closest('.dtcombo-wrap') : null;
        var sel = t && t.tagName === 'SELECT' ? t : (wrap ? wrap.querySelector('select') : null);
        if (!sel || !LEAVE[sel.id]) return;
        if (wrap && e.relatedTarget && wrap.contains(e.relatedTarget)) return;
        var fn = LEAVE[sel.id];
        /* after the combo has written its pick back to the select */
        setTimeout(function () { chain(fn); }, 140);
    });

    /* --------------------------------------------------------------- the detail grid */

    var DETAIL_COLS = [
        { key: 'Plant' }, { key: 'ChargesType' }, { key: 'ChartOfAccount' }, { key: 'Brand' }, { key: 'BrandUom' },
        { key: 'ItemQty', num: true, fmt: '#,##0.##', total: true }, { key: 'ItemRate', num: true, fmt: '#,##0.##' },
        { key: 'RateUom' }, { key: 'Amount', num: true, fmt: '#,##0.##', total: true }, { key: 'Remarks', edit: true }
    ];

    /** grdDetailOverheadSettings:1259 - Delete button first (frozen), hidden id columns, Remarks editable. */
    function renderDetail() {
        var t = $('grdDetailOverhead');
        t.tHead.innerHTML = '<tr><th style="width:24px;">Delete</th>' + DETAIL_COLS.map(function (c) {
            return '<th' + (c.num ? ' class="num"' : '') + '>' + esc(c.key) + '</th>';
        }).join('') + '</tr>';
        if (currentDetail >= tableOverHeads.length) currentDetail = tableOverHeads.length - 1;
        if (currentDetail < 0 && tableOverHeads.length) currentDetail = 0;
        t.tBodies[0].innerHTML = tableOverHeads.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === currentDetail ? ' class="is-current"' : '') + '>'
                + '<td><button type="button" class="cellbtn" data-del="' + i + '">X</button></td>'
                + DETAIL_COLS.map(function (c) {
                    if (c.edit) return '<td><input type="text" class="cell-edit" data-edit="' + i + '" value="' + esc(r[c.key]) + '"></td>';
                    var v = c.num ? netFormat(toDouble(r[c.key]), c.fmt) : str(r[c.key]);
                    return '<td' + (c.num ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        var tot = '<tr><td></td>' + DETAIL_COLS.map(function (c) {
            if (!c.total) return '<td></td>';
            var s = 0; tableOverHeads.forEach(function (r) { s += toDouble(r[c.key]); });
            return '<td class="num">' + esc(netFormat(s, c.fmt)) + '</td>';
        }).join('') + '</tr>';
        t.tFoot.innerHTML = tableOverHeads.length ? tot : '';
        $('navDetail').textContent = tableOverHeads.length
            ? 'Record ' + (currentDetail + 1) + ' of ' + tableOverHeads.length : '';
    }

    $('grdDetailOverhead').addEventListener('click', function (e) {
        var tr = e.target.closest('tbody tr');
        if (tr) {
            currentDetail = +tr.getAttribute('data-i');
            Array.prototype.forEach.call(this.tBodies[0].rows, function (x) { x.classList.toggle('is-current', x === tr); });
            $('navDetail').textContent = 'Record ' + (currentDetail + 1) + ' of ' + tableOverHeads.length;
        }
        var del = e.target.closest('[data-del]');
        if (del) deleteDetailRow(+del.getAttribute('data-del'));
    });
    $('grdDetailOverhead').addEventListener('input', function (e) {
        var ed = e.target.closest('[data-edit]');
        if (ed) tableOverHeads[+ed.getAttribute('data-edit')].Remarks = ed.value;
    });
    $('grdDetailOverhead').addEventListener('dblclick', function (e) {
        if (e.target.closest('[data-del]') || e.target.closest('[data-edit]')) return;
        var tr = e.target.closest('tbody tr');
        if (tr) chain(function () { return detailDoubleClick(+tr.getAttribute('data-i')); });
    });

    /** grdDetailOverhead_ColumnButtonClick:1290 */
    function deleteDetailRow(i) {
        if (!toolUsable('btnSaveOverHead')) { msg('You Can not Delete Record In Update Mode'); return; }
        tableOverHeads.splice(i, 1);
        if (currentDetail >= tableOverHeads.length) currentDetail = tableOverHeads.length - 1;
        renderDetail();
        jobOrderOverHeadLock();
    }

    /** JobOrderOverHeadLock:1146 */
    function jobOrderOverHeadLock() {
        if (tableOverHeads.length > 0) {
            enable('cmbJobOrderOverHead', false);
            setCombo('cmbJobOrderOverHead', tableOverHeads[0].InvProductionJobOrderId);
        } else {
            enable('cmbJobOrderOverHead', true);
            enable('CmbPlantFeederOverHead', true);
            enable('txtDocDateOverHead', true);
        }
    }

    /** RefreshPlantInOhGrid:1843 - rows whose plant is not in the job order's plant list lose it. */
    function refreshPlantInOhGrid() {
        if (tableOverHeads.length <= 0 || LISTS.plants.length <= 0) return;
        var ok = {};
        LISTS.plants.forEach(function (p) { ok[toInt(col(p, 'PlantId'))] = true; });
        tableOverHeads.forEach(function (r) {
            if (!ok[toInt(r.PlantId)]) { r.PlantId = 0; r.Plant = ''; }
        });
        renderDetail();
    }

    /** FormValidationOfDetailPortionOverheads:1084 */
    function validateDetail() {
        if (!hasRow('cmbJobOrderOverHead')) { msg('job Order Field Required'); focus('cmbJobOrderOverHead'); return false; }
        if (!hasRow('CmbPlantFeederOverHead')) { msg('Plant Field Required'); focus('CmbPlantFeederOverHead'); return false; }
        if (!hasRow('cmbChartofAccountOverHead')) { msg('OverHead Account Field Required'); focus('cmbChartofAccountOverHead'); return false; }
        if (hasRow('CmbBrandNameOverHead') && !hasRow('CmbBrandUomOverHead')) { msg('outPut ItemUOM Field Required'); focus('CmbBrandUomOverHead'); return false; }
        if (!hasRow('cmbRateUomOverHeads') && !hasRow('CmbBrandNameOverHead') && !hasRow('CmbBrandUomOverHead')) {
            msg('RateUOM Field Required'); focus('cmbRateUomOverHeads'); return false;
        }
        var a = val('txtExpAmountOverHead').trim();
        if (a === '' || toDouble(a) === 0) { msg('Amount Field Required'); $('txtExpAmountOverHead').focus(); return false; }
        return true;
    }

    /** btnAddOH_Click:1125 */
    function addDetail() {
        if (!validateDetail()) return;
        tableOverHeads.push({
            Id: 0, DocDate: val('txtDocDateOverHead'), DocNo: toInt(val('txtDocNoOverHead').trim()),
            InvProductionJobOrderId: toInt(val('cmbJobOrderOverHead')),
            PlantId: toInt(val('CmbPlantFeederOverHead')), Plant: text('CmbPlantFeederOverHead'),
            ChargesTypeId: toInt(val('cmbChargestypeOverheads')), ChargesType: text('cmbChargestypeOverheads'),
            ChartOfAccountId: toInt(val('cmbChartofAccountOverHead')), ChartOfAccount: text('cmbChartofAccountOverHead'),
            BrandId: toInt(val('CmbBrandNameOverHead')), Brand: text('CmbBrandNameOverHead'),
            BrandUomId: toInt(val('CmbBrandUomOverHead')), BrandUom: text('CmbBrandUomOverHead'),
            ItemQty: toDouble(val('txtItemQtyOverHead').trim()), ItemRate: toDouble(val('txtItemRateOverHeads').trim()),
            RateUomId: toInt(val('cmbRateUomOverHeads')), RateUom: text('cmbRateUomOverHeads'),
            Amount: toDouble(val('txtExpAmountOverHead').trim()), Remarks: val('txtLedgerRemarksOverHead').trim(),
            DetailDocumentTypeId: docTypeOfSide()
        });
        currentDetail = 0;
        renderDetail();
        resetDetail();
        jobOrderOverHeadLock();
        focus('CmbPlantFeederOverHead');
    }

    /** btnUpdateOH_Click:1168 - the ChargesType TEXT column is not rewritten (:1176 sets only the id). */
    function updateDetail() {
        if (!validateDetail()) return Promise.resolve();
        var r = tableOverHeads[updateDetailIndexOH];
        if (!r) { msg('Index was out of range. Must be non-negative and less than the size of the collection.'); return Promise.resolve(); }
        r.PlantId = toInt(val('CmbPlantFeederOverHead'));
        r.Plant = text('CmbPlantFeederOverHead');
        r.ChargesTypeId = toInt(val('cmbChargestypeOverheads'));
        r.ChartOfAccountId = toInt(val('cmbChartofAccountOverHead'));
        r.ChartOfAccount = text('cmbChartofAccountOverHead');
        r.BrandId = toInt(val('CmbBrandNameOverHead'));
        r.Brand = text('CmbBrandNameOverHead');
        return getItemUoms(toInt(val('cmbJobOrderOverHead')), toInt(val('CmbBrandNameOverHead'))).then(function () {
            r.BrandUomId = toInt(val('CmbBrandUomOverHead'));
            r.BrandUom = text('CmbBrandUomOverHead');
            r.ItemQty = toDouble(val('txtItemQtyOverHead').trim());
            r.ItemRate = toDouble(val('txtItemRateOverHeads').trim());
            r.RateUomId = toInt(val('cmbRateUomOverHeads'));
            r.RateUom = text('cmbRateUomOverHeads');
            r.Amount = toDouble(val('txtExpAmountOverHead').trim());
            r.Remarks = val('txtLedgerRemarksOverHead').trim();
            r.DetailDocumentTypeId = docTypeOfSide();
            resetDetail();
            renderDetail();
        });
    }

    /** grdDetailOverhead_DoubleClick:1201 */
    function detailDoubleClick(i) {
        var item = tableOverHeads[i];
        if (!item) return Promise.resolve();
        if (toInt(item.ChargesTypeId) === 3) { msg("you can't Edit Foh Expense Row"); return Promise.resolve(); }
        updateDetailIndexOH = i;
        setCombo('CmbPlantFeederOverHead', item.PlantId);
        setCombo('cmbChargestypeOverheads', item.ChargesTypeId);
        setCombo('cmbChartofAccountOverHead', item.ChartOfAccountId);
        var side = toInt(item.DetailDocumentTypeId) === 80 ? 'RadInputItemOH' : 'RadOutputItemOH';
        var p = Promise.resolve();
        if (!$(side).checked) {
            $(side).checked = true;
            p = getBrandItems(toInt(val('cmbJobOrderOverHead')));        // RadOutputItemOH_CheckedChanged
        }
        return p.then(function () {
            setCombo('CmbBrandNameOverHead', item.BrandId);
            return brandLeave();                                            // CmbBrandNameOverHead_Leave(null, null)
        }).then(function () {
            setCombo('CmbBrandUomOverHead', item.BrandUomId);
            setVal('txtItemQtyOverHead', netFormat(toDouble(item.ItemQty), '#,##0.##')); calculate();
            setVal('txtItemRateOverHeads', netFormat(toDouble(item.ItemRate), '#,##0.##')); calculate();
            setCombo('cmbRateUomOverHeads', item.RateUomId);
            setVal('txtExpAmountOverHead', netFormat(toDouble(item.Amount), '#,##0.##'));
            setVal('txtLedgerRemarksOverHead', item.Remarks);
            show('btnAddOH', false); show('btnUpdateOH', true); show('BtnCancelOH', true);
            focus('CmbPlantFeederOverHead');
            jobOrderOverHeadLock();
        });
    }

    /** ResetdetailOverHead:1061 - Plant and Rate UOM are kept. */
    function resetDetail() {
        setVal('cmbChargestypeOverheads', '');
        setVal('cmbChartofAccountOverHead', '');
        setVal('CmbBrandNameOverHead', '');
        setVal('CmbBrandUomOverHead', '');
        setVal('txtItemQtyOverHead', '');
        setVal('txtItemRateOverHeads', '');
        setVal('txtExpAmountOverHead', '');
        setVal('txtLedgerRemarksOverHead', '');
        show('btnAddOH', true); show('btnUpdateOH', false); show('BtnCancelOH', false);
        focus('CmbPlantFeederOverHead');
    }

    /** FormResetOverHead:1031 - the two finish totals are NOT cleared on the desktop. */
    function formReset() {
        RecOverHeadId = 0;
        VoucherHeadId = 0;
        tool('btnUpdateOverHead', { visible: false });
        UpdateMode = false;
        tool('btnSaveOverHead', { visible: true });
        enable('cmbJobOrderOverHead', true);
        enable('CmbPlantFeederOverHead', true);
        enable('txtDocDateOverHead', true);
        setVal('cmbJobOrderOverHead', '');
        ['txtOutputQtyTotalOH', 'txtOutputWeightTotalOH', 'txtInputQtyTotalOH', 'txtInputWeightTotalOH']
            .forEach(function (id) { setVal(id, ''); });
        setVal('CmbPlantFeederOverHead', '');
        tableOverHeads = [];
        currentDetail = -1;
        renderDetail();
        resetDetail();
        return generateCode().then(function () { focus('cmbJobOrderOverHead'); });
    }

    /** txtDocDateOverHead_ValueChanged:1702 - every row takes the new date. */
    $('txtDocDateOverHead').addEventListener('change', function () {
        tableOverHeads.forEach(function (r) { r.DocDate = val('txtDocDateOverHead'); });
    });

    /* ------------------------------------------------------------------ Save / Update */

    /** FormValidationOfOverHead:1541 then OverHeadInsert:779 */
    function overHeadInsert() {
        if (val('txtDocNoOverHead').trim() === '') { msg('Doc No. Field Required'); $('txtDocNoOverHead').focus(); return Promise.resolve(); }
        if (!hasRow('cmbJobOrderOverHead')) { msg('Job Order No. Field Required'); focus('cmbJobOrderOverHead'); return Promise.resolve(); }
        var p;
        if (RecOverHeadId > 0) {
            if (!window.confirm('Are you sure to Update?')) return Promise.resolve();
            p = Promise.resolve();
        } else {
            if (!window.confirm('Are you sure to Save?')) return Promise.resolve();
            p = generateCode();
        }
        return p.then(function () {
            return postJson(API + '/save', {
                recOverHeadId: RecOverHeadId,
                docNo: val('txtDocNoOverHead').trim(),
                jobOrderId: toInt(val('cmbJobOrderOverHead')),
                rows: tableOverHeads
            });
        }).then(function (r) {
            msg(r.message);
            return formReset();
        });
    }
    function btnSave() {
        if (!toolUsable('btnSaveOverHead')) return Promise.resolve();
        return busy('btnSaveOverHead', function () {
            return after(function () { RecOverHeadId = 0; return overHeadInsert(); });   // :923
        });
    }
    function btnUpdate() {
        if (!toolUsable('btnUpdateOverHead')) return Promise.resolve();
        return busy('btnUpdateOverHead', function () { return after(overHeadInsert); });
    }

    /* ------------------------------------------------- ReadByIdOverHeadJobOrderWise:944 */

    function readById(jobOrderId) {
        tool('btnSaveOverHead', { visible: false });
        tool('btnUpdateOverHead', { visible: true });
        return getJson(API + '/by-job-order/' + encodeURIComponent(jobOrderId)).then(function (res) {
            var dt = (res && res.rows) || [];
            if (dt.length <= 0) return;
            if (str(col(dt[0], 'JobOrderStatus')) === 'Approved') throw new Error("You Can't Update Approved Record");
            if (str(col(dt[0], 'PlanStatus')) !== 'In Process') {
                throw new Error("You Can't Update Record with " + str(col(dt[0], 'PlanStatus')) + ' JobPlanStatus');
            }
            showTab('form');
            setVal('txtDocNoOverHead', toInt(col(dt[0], 'DocNo')));
            setVal('txtDocDateOverHead', dateOnly(col(dt[0], 'DocDate')));
            RecOverHeadId = toInt(col(dt[0], 'InvProductionJobOrderId'));
            setCombo('cmbJobOrderOverHead', RecOverHeadId);
            return jobOrderLeave().then(function () {
                tableOverHeads = dt.map(function (r) {
                    return {
                        Id: toInt(col(r, 'Id')), DocDate: dateOnly(col(r, 'DocDate')), DocNo: toInt(col(r, 'DocNo')),
                        InvProductionJobOrderId: toInt(col(r, 'InvProductionJobOrderId')),
                        PlantId: col(r, 'PlantId'), Plant: str(col(r, 'PlantName')),
                        ChargesTypeId: col(r, 'ChargesTypeId'), ChargesType: str(col(r, 'ChargesType')),
                        ChartOfAccountId: col(r, 'CharOfAccountId'), ChartOfAccount: str(col(r, 'AccountTitle')),
                        BrandId: col(r, 'BrandId'), Brand: str(col(r, 'BrandName')),
                        BrandUomId: col(r, 'BrandUomId'), BrandUom: str(col(r, 'BrandUom')),
                        ItemQty: col(r, 'ItemQty'), ItemRate: col(r, 'ItemRate'),
                        /* ReadByjobProductionId returns the rate UOM's Equivalent as "RateUom". */
                        RateUomId: col(r, 'RateUomId'), RateUom: str(col(r, 'RateUom')),
                        Amount: col(r, 'Amount'), Remarks: str(col(r, 'ohRemarks')),
                        DetailDocumentTypeId: col(r, 'DetailDocumentTypeId')
                    };
                });
                currentDetail = tableOverHeads.length ? 0 : -1;
                renderDetail();
                VoucherHeadId = toInt(res.voucherHeadId);                 // GetVoucherHeadId(RecOverHeadId, 110)
                focus('cmbChartofAccountOverHead');
                UpdateMode = true;
                refreshPlantInOhGrid();
            });
        }).catch(function (e) { msg(e.message); });
    }

    /* ------------------------------------------------------------------------- History */

    /** BindGridHistoryOverHead:1326 - grouped by job order, in first-seen order. */
    function bindHistory() {
        return getJson(API + '/history').then(function (rows) {
            rows = rows || [];
            var groups = [], byKey = {};
            rows.forEach(function (r) {
                var k = str(col(r, 'InvProductionJobOrderId'));
                if (!byKey[k]) { byKey[k] = { rows: [] }; groups.push(byKey[k]); }
                byKey[k].rows.push(r);
            });
            OverHeadHistory = groups.map(function (g) {
                var f = g.rows[0], q = 0, a = 0, rt = 0;
                g.rows.forEach(function (r) { q += toDouble(col(r, 'ItemQty')); a += toDouble(col(r, 'Amount')); rt += toDouble(col(r, 'ItemRate')); });
                return {
                    InvProductionJobOrderId: toInt(col(f, 'InvProductionJobOrderId')),
                    ProductionJobOrder: str(col(f, 'PlanCode')),
                    TotalItemQty: q, TotalExpense: a, AverageItemRate: rt / g.rows.length,
                    IsApproved: str(col(f, 'JobOrderStatus')), JobPlanStatus: str(col(f, 'JobPlanStatus'))
                };
            });
            currentHistory = OverHeadHistory.length ? 0 : -1;
            renderHistory();
            if (currentHistory >= 0) return historySelectionChanged();
            renderHistoryDetail([]);
        }).catch(function (e) { msg(e.message); });
    }

    var HIST_COLS = [
        { key: 'ProductionJobOrder', link: true }, { key: 'TotalItemQty', num: true, fmt: '#,##0.####', agg: 'sum' },
        { key: 'TotalExpense', num: true, fmt: '#,##0.###', agg: 'sum' }, { key: 'AverageItemRate', num: true, fmt: '#,#.##', agg: 'avg' },
        { key: 'IsApproved' }, { key: 'JobPlanStatus' }
    ];
    /** OverHeadGridSetting:1385 - Edit button at position 0, frozen; ClearStructure when empty. */
    function renderHistory() {
        var t = $('grdOverHeadHistory');
        if (!OverHeadHistory.length) {
            t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = '';
            $('navHistory').textContent = '';
            return;
        }
        t.tHead.innerHTML = '<tr><th style="width:50px;">Edit</th>' + HIST_COLS.map(function (c) {
            return '<th' + (c.num ? ' class="num"' : '') + '>' + esc(c.key) + '</th>';
        }).join('') + '</tr>';
        t.tBodies[0].innerHTML = OverHeadHistory.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === currentHistory ? ' class="is-current"' : '') + '>'
                + '<td><button type="button" class="cellbtn" data-edit-hist="' + i + '">Edit</button></td>'
                + HIST_COLS.map(function (c) {
                    if (c.link) return '<td><span class="win-doc-link" data-open-hist="' + i + '">' + esc(r[c.key]) + '</span></td>';
                    return '<td' + (c.num ? ' class="num"' : '') + '>' + esc(c.num ? netFormat(r[c.key], c.fmt) : r[c.key]) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr class="rk-keep"><td></td>' + HIST_COLS.map(function (c) {
            if (!c.agg) return '<td></td>';
            var s = 0; OverHeadHistory.forEach(function (r) { s += toDouble(r[c.key]); });
            if (c.agg === 'avg') s = s / OverHeadHistory.length;
            return '<td class="num">' + esc(netFormat(s, c.fmt)) + '</td>';
        }).join('') + '</tr>';
        K.filterRow(t);
        $('navHistory').textContent = 'Record ' + (currentHistory + 1) + ' of ' + OverHeadHistory.length;
    }

    /** grdOverHeadHistory_SelectionChanged:1468 */
    function historySelectionChanged() {
        var item = OverHeadHistory[currentHistory];
        if (!item) return Promise.resolve();
        return getJson(API + '/history-detail?' + qs({ jobOrderId: item.InvProductionJobOrderId }))
            .then(function (rows) { if ((rows || []).length > 0) renderHistoryDetail(rows); })
            .catch(function (e) { msg(e.message); });
    }

    var HD_COLS = [
        { key: 'DocNo', src: 'DocNo' }, { key: 'DocDate', src: 'DocDate', date: true }, { key: 'JobOrderNo', src: 'PlanCode' },
        { key: 'PlantName', src: 'PlantName' }, { key: 'ChargesType', src: 'ChargesType' }, { key: 'GLAccount', src: 'AccountTitle' },
        { key: 'BrandName', src: 'BrandName' }, { key: 'BrandUom', src: 'BrandUom' },
        { key: 'ItemQty', src: 'ItemQty', num: true, fmt: '#,#' }, { key: 'ItemRate', src: 'ItemRate', num: true, fmt: '#,#.##' },
        { key: 'Amount', src: 'Amount', num: true, fmt: '#,#' }, { key: 'Remarks', src: 'ohRemarks' },
        { key: 'IsApproved', src: 'JobOrderStatus' }
    ];
    /** GrddetailOhHistoryGridSetting:1508 - ItemQty, ItemRate and Amount are all summed. */
    function renderHistoryDetail(rows) {
        var t = $('grddetailOhHistory');
        if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; $('navHistoryDetail').textContent = ''; return; }
        t.tHead.innerHTML = '<tr>' + HD_COLS.map(function (c) { return '<th' + (c.num ? ' class="num"' : '') + '>' + esc(c.key) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr>' + HD_COLS.map(function (c) {
                var v = col(r, c.src);
                if (c.num) v = netFormat(toDouble(v), c.fmt);
                else if (c.date) v = K.dMMMyyyy(dateOnly(v));
                return '<td' + (c.num ? ' class="num"' : '') + '>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = '<tr class="rk-keep">' + HD_COLS.map(function (c) {
            if (!c.num) return '<td></td>';
            var s = 0; rows.forEach(function (r) { s += toDouble(col(r, c.src)); });
            return '<td class="num">' + esc(netFormat(s, c.fmt)) + '</td>';
        }).join('') + '</tr>';
        K.filterRow(t);
        $('navHistoryDetail').textContent = rows.length + ' record(s)';
    }

    /** grdOverHeadHistory_ColumnButtonClick:1422 and grdOverHead_DoubleClick:1445 (identical). */
    function editFromHistory(i) {
        tool('btnSaveOverHead', { visible: false });
        tool('btnUpdateOverHead', { visible: true });
        var r = OverHeadHistory[i];
        if (!r) return Promise.resolve();
        if (r.IsApproved === 'Approved') { msg("You Can't Update Approved Record"); return Promise.resolve(); }
        if (r.JobPlanStatus !== 'In Process') { msg("You Can't Update Record with " + r.JobPlanStatus + ' JobPlanStatus'); return Promise.resolve(); }
        RecOverHeadId = toInt(r.InvProductionJobOrderId);
        return readById(RecOverHeadId);
    }

    $('grdOverHeadHistory').addEventListener('click', function (e) {
        var tr = e.target.closest('tbody tr');
        if (tr && tr.hasAttribute('data-i')) {
            var i = +tr.getAttribute('data-i');
            if (i !== currentHistory) {
                currentHistory = i;
                Array.prototype.forEach.call(this.tBodies[0].rows, function (x) { x.classList.toggle('is-current', x === tr); });
                $('navHistory').textContent = 'Record ' + (i + 1) + ' of ' + OverHeadHistory.length;
                historySelectionChanged();
            }
        }
        var b = e.target.closest('[data-edit-hist]') || e.target.closest('[data-open-hist]');
        if (b) {
            var k = +(b.getAttribute('data-edit-hist') || b.getAttribute('data-open-hist'));
            busy(b.tagName === 'BUTTON' ? b : null, function () { return editFromHistory(k); });
        }
    });
    $('grdOverHeadHistory').addEventListener('dblclick', function (e) {
        if (e.target.closest('[data-edit-hist]') || e.target.closest('[data-open-hist]')) return;
        var tr = e.target.closest('tbody tr');
        if (tr && tr.hasAttribute('data-i')) editFromHistory(+tr.getAttribute('data-i'));
    });

    /* ------------------------------------------------------------------ Print / Voucher */

    /** btnPrintOverHead_Click:1570 - the job order of the grid's CURRENT row. */
    function btnPrint() {
        if (!toolUsable('btnPrintOverHead')) return;
        var row = tableOverHeads[currentDetail];
        if (!row) { msg('Object reference not set to an instance of an object.'); return; }
        var jobOrderId = toInt(row.InvProductionJobOrderId);
        var win = window.CrystalPrint.reserve();
        busy('btnPrintOverHead', function () {
            return getJson(API + '/print-606-rows?' + qs({ jobOrderId: jobOrderId })).then(function (r) {
                if (!r || r.rows === 0) { window.CrystalPrint.release(win); msg('Record Not Found For DisPlay'); return; }
                return window.CrystalPrint.open('pr-606', { jobOrderId: jobOrderId }, null, win);
            }).catch(function (e) { window.CrystalPrint.release(win); throw e; });
        });
    }
    /** btnVoucherOH_Click_1:1558 -> CommonServices.VoucherReport_118(VoucherHeadId). */
    function btnVoucher() {
        if (!toolUsable('btnVoucherOH')) return;
        if (VoucherHeadId === 0) { msg('VoucherId Not Found'); return; }
        window.CrystalPrint.open('acc-118', { id: VoucherHeadId }, $('btnVoucherOH'));
    }

    /* ------------------------------------------------------ FOH Expenses (GrdPopUpToReturndt) */

    var FOH = { rows: [], cols: [], current: -1 };
    var FOH_HIDDEN = { accountid: true, joborderid: true, plantid: true };

    /** BtnLoadFohOverHeads_Click:1754 */
    function btnLoadFoh() {
        if (!toolUsable('BtnLoadFohOverHeads')) return;
        if (!hasRow('cmbJobOrderOverHead') || toInt(val('cmbJobOrderOverHead')) === 0) {
            msg('Job Order No. Field Required'); focus('cmbJobOrderOverHead'); return;
        }
        if (!hasRow('CmbPlantFeederOverHead') || toInt(val('CmbPlantFeederOverHead')) === 0) {
            msg('Plant Field Required'); focus('CmbPlantFeederOverHead'); return;
        }
        busy('BtnLoadFohOverHeads', function () {
            return after(function () {
                return getJson(API + '/foh-expenses?' + qs({
                    jobOrderId: toInt(val('cmbJobOrderOverHead')), plantId: toInt(val('CmbPlantFeederOverHead')),
                    docDate: val('txtDocDateOverHead') || today()
                })).then(function (rows) {
                    rows = rows || [];
                    if (rows.length === 0) throw new Error('No Data Found For This JobOrder, Plant And DocDate');
                    openFoh(rows);
                });
            });
        });
    }
    function openFoh(rows) {
        FOH.rows = rows.map(function (r) { return { data: r, checked: false }; });
        FOH.cols = Object.keys(rows[0]).filter(function (k) { return !FOH_HIDDEN[k.toLowerCase()]; });
        FOH.current = 0;
        var t = $('grdFoh');
        t.tHead.innerHTML = '<tr><th style="width:30px;">Select</th>' + FOH.cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = FOH.rows.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === 0 ? ' class="is-current"' : '') + '><td style="text-align:center;"><input type="checkbox" data-chk="' + i + '"></td>'
                + FOH.cols.map(function (c) {
                    var v = r.data[c];
                    var isNum = typeof v === 'number';
                    var s = isNum ? netFormat(v, '#,##0.##') : (/^\d{4}-\d{2}-\d{2}/.test(str(v)) ? K.dMMMyyyy(dateOnly(v)) : str(v));
                    return '<td' + (isNum ? ' class="num"' : '') + '>' + esc(s) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        K.filterRow(t);
        $('dlgFoh').classList.add('is-open');
        setTimeout(function () { var c = t.querySelector('input[data-chk]'); if (c) c.focus(); }, 0);
    }
    function closeModal(id) { $(id).classList.remove('is-open'); }
    /** grd_DoubleClick: checked rows if any, else the current row; Enter does the same. */
    function returnFoh() {
        var picked = FOH.rows.filter(function (r) { return r.checked; }).map(function (r) { return r.data; });
        if (!picked.length && FOH.rows[FOH.current]) picked = [FOH.rows[FOH.current].data];
        closeModal('dlgFoh');
        if (picked.length > 0) loadInDetailOverHead(picked);
    }
    $('grdFoh').addEventListener('click', function (e) {
        var tr = e.target.closest('tbody tr');
        if (tr) {
            FOH.current = +tr.getAttribute('data-i');
            Array.prototype.forEach.call(this.tBodies[0].rows, function (x) { x.classList.toggle('is-current', x === tr); });
        }
        var c = e.target.closest('[data-chk]');
        if (c) FOH.rows[+c.getAttribute('data-chk')].checked = c.checked;
    });
    $('grdFoh').addEventListener('dblclick', function (e) { if (!e.target.closest('[data-chk]') && e.target.closest('tbody tr')) returnFoh(); });
    $('dlgFoh').addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !(e.target.closest && e.target.closest('tr.rk-filter'))) { e.preventDefault(); e.stopPropagation(); returnFoh(); }
        else if (e.key === 'Escape' || (e.ctrlKey && (e.key === 'e' || e.key === 'E'))) { e.preventDefault(); e.stopPropagation(); closeModal('dlgFoh'); }
    });

    /** LoadInDetailOverHead:1814 - one row per job order + plant + doc date + account, never twice. */
    function loadInDetailOverHead(dt) {
        function ymd(d) { return dateOnly(d).replace(/-/g, ''); }
        var existing = {};
        tableOverHeads.forEach(function (r) {
            existing[r.InvProductionJobOrderId + '_' + r.PlantId + '_' + ymd(r.DocDate) + '_' + r.ChartOfAccountId] = true;
        });
        var docDate = val('txtDocDateOverHead');
        var docNo = toInt(val('txtDocNoOverHead'));
        dt.forEach(function (dr) {
            var key = toInt(col(dr, 'JobOrderId')) + '_' + toInt(col(dr, 'PlantId')) + '_' + ymd(docDate) + '_' + toInt(col(dr, 'AccountId'));
            if (existing[key]) return;
            existing[key] = true;
            tableOverHeads.push({
                Id: 0, DocDate: docDate, DocNo: docNo, InvProductionJobOrderId: toInt(col(dr, 'JobOrderId')),
                PlantId: toInt(col(dr, 'PlantId')), Plant: str(col(dr, 'PlantName')),
                ChargesTypeId: 3, ChargesType: 'FOH Expenses',
                ChartOfAccountId: toInt(col(dr, 'AccountId')), ChartOfAccount: str(col(dr, 'AccountTitle')),
                BrandId: 0, Brand: '', BrandUomId: 0, BrandUom: '', ItemQty: 0, ItemRate: 0, RateUomId: 0, RateUom: '',
                Amount: col(dr, 'ConsumedFohAmount'), Remarks: '', DetailDocumentTypeId: 112
            });
        });
        if (currentDetail < 0 && tableOverHeads.length) currentDetail = 0;
        renderDetail();
        enable('txtDocDateOverHead', false);
        enable('cmbJobOrderOverHead', false);
        enable('CmbPlantFeederOverHead', false);
    }

    /* --------------------------------------------------------------- Formulas (MakeOHformulas) */

    function btnFormulas() {
        if (!toolUsable('btnOverHeadFormulas')) return;
        var rows = [
            ['Amount (Processing Charges)', 'if Brand Name,Brand Uom is selected and Item Qty and Item Rate has value greater than zero ---> Amount will be equal to (ItemQty * ItemRate) else if Item Qty and ItemRate has value grater than zero  and Rate Uom is selected Amount will be equal to((Total Input Weight / Rate UOm) *Item Rate)'],
            ['Amount (Other Charges)', 'if Item Qty and ItemRate has value grater than zero Amount will be equal to (Item Qty * Item Rate),and Amount Will be Editable']
        ];
        var t = $('grdFormulas');
        t.tHead.innerHTML = '<tr><th style="width:150px;">Formula For</th><th>Description</th></tr>';
        t.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr><td class="wrap">' + esc(r[0]) + '</td><td class="wrap">' + esc(r[1]) + '</td></tr>';
        }).join('');
        $('dlgFormulas').classList.add('is-open');
    }

    /* -------------------------------------------------------------------- Refresh / New */

    /** btnRefreshOverHead_Click:1011 */
    function btnRefresh() {
        if (!toolUsable('btnRefreshOverHead')) return;
        doRefresh();
    }
    /* Ctrl+R calls the handler directly (:467), without asking whether the button is enabled. */
    function doRefresh() {
        busy('btnRefreshOverHead', function () {
            return getJson(API + '/lookups').then(function (l) {
                LISTS.rateUoms = l.rateUoms || [];
                fill('cmbRateUomOverHeads', LISTS.rateUoms, 'Id', 'UOM', { eq: 'Equivalent' });
                LISTS.charges = l.chargesTypes || [];
                fill('cmbChargestypeOverheads', LISTS.charges, 'Id', 'Type');
                jobOrderNoFill(l.jobOrders);
                LISTS.coa = l.chartOfAccounts || [];
                if (LISTS.coa.length > 0) fill('cmbChartofAccountOverHead', LISTS.coa, 'Id', 'AccountTitle');
            });
        });
    }
    function btnNew() {
        if (!toolUsable('btnNewOverHead')) return;
        busy('btnNewOverHead', function () { return after(formReset); });
    }

    /* ---------------------------------------------------------------------- tabControl1 */

    function showTab(which) {
        var form = which === 'form';
        $('pageForm').classList.toggle('is-active', form);
        $('pageHistory').classList.toggle('is-active', !form);
        $('tabForm').classList.toggle('is-active', form);
        $('tabHistory').classList.toggle('is-active', !form);
    }
    /** tabControl4_SelectedIndexChanged:1318 - History rebinds every time it is selected. */
    $('tabForm').addEventListener('click', function () { showTab('form'); });
    $('tabHistory').addEventListener('click', function () {
        if ($('pageHistory').classList.contains('is-active')) return;
        showTab('history');
        bindHistory();
    });
    function formTabActive() { return $('pageForm').classList.contains('is-active'); }

    /* ------------------------------------------------------------------------- wiring */

    $('btnNewOverHead').addEventListener('click', btnNew);
    $('btnRefreshOverHead').addEventListener('click', btnRefresh);
    $('btnSaveOverHead').addEventListener('click', btnSave);
    $('btnUpdateOverHead').addEventListener('click', btnUpdate);
    $('btnPrintOverHead').addEventListener('click', btnPrint);
    $('btnVoucherOH').addEventListener('click', btnVoucher);
    $('BtnLoadFohOverHeads').addEventListener('click', btnLoadFoh);
    $('btnOverHeadFormulas').addEventListener('click', btnFormulas);
    $('btnAddOH').addEventListener('click', function () { chain(addDetail); });
    $('btnUpdateOH').addEventListener('click', function () { busy('btnUpdateOH', function () { return after(updateDetail); }); });
    $('BtnCancelOH').addEventListener('click', resetDetail);
    /* TextChanged */
    $('txtItemQtyOverHead').addEventListener('input', calculate);
    $('txtItemRateOverHeads').addEventListener('input', calculate);
    $('txtItemQtyOverHead').addEventListener('blur', qtyLeave);
    /* RadOutputItemOH_CheckedChanged:1873 - fires whichever way the pair flips. */
    ['RadOutputItemOH', 'RadInputItemOH'].forEach(function (id) {
        $(id).addEventListener('change', function () { chain(function () { return getBrandItems(toInt(val('cmbJobOrderOverHead'))); }); });
    });
    Array.prototype.forEach.call(document.querySelectorAll('[data-close]'), function (b) {
        b.addEventListener('click', function () { closeModal(b.getAttribute('data-close')); });
    });
    Array.prototype.forEach.call(document.querySelectorAll('[data-full]'), function (b) {
        b.addEventListener('click', function () { $(b.getAttribute('data-full')).classList.toggle('is-fullscreen'); });
    });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && $('dlgFormulas').classList.contains('is-open')) { e.preventDefault(); closeModal('dlgFormulas'); }
    });

    /** frmFoodProduction_KeyDown:441 - Enter -> Tab; Ctrl+E / Esc close; the rest on tab 0 only. */
    K.enterToTab();
    function closeForm() {
        if (document.querySelector('.oh-modal.is-open')) return;
        if (window.parent && window.parent !== window) {
            if (typeof window.parent.P280CloseTab === 'function') window.parent.P280CloseTab('Overhead');
            return;
        }
        K.close();
    }
    K.keys({
        'ctrl+e': closeForm,
        'esc': closeForm,
        'ctrl+s': function () { if (formTabActive() && toolUsable('btnSaveOverHead')) btnSave(); },
        'ctrl+u': function () { if (formTabActive() && toolUsable('btnUpdateOverHead')) btnUpdate(); },
        /* Ctrl+N / Ctrl+R call the handlers directly (:463-470), enabled or not. */
        'ctrl+n': function () { if (formTabActive()) busy('btnNewOverHead', function () { return after(formReset); }); },
        'ctrl+r': function () { if (formTabActive()) doRefresh(); }
    });

    /* ---------------------------------------------------------------------------- start */

    setVal('txtDocDateOverHead', today());
    renderDetail();
    var ready = load();

    /**
     * Shell -> Overhead (FoodProductionWithValues.SettlementForm_OnOverHeadReadById:3332):
     * OverHeadForm.tabControl1.SelectedIndex = 0; OverHeadForm.ReadByIdOverHeadJobOrderWise(id).
     */
    window.P280ReadById = function (id) {
        return ready.then(function () {
            showTab('form');
            return after(function () { return readById(toInt(id)); });
        });
    };
}());
