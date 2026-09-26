/* ============================================================================================
 * Screen 280 - the Consumption tab page of FoodProductionWithValues.cs (DocumentTypeId 181):
 * tabPage9 "Form", tabPage10 "History", and the two loader forms the page opens:
 *   LoadConsumptionPendingforRates.cs                     "Pending For Rates "
 *   LoadavailableTransactionsForIssuanceOnConsumption.cs  "Load Data for Consumption Entry"
 *
 * Line numbers are FoodProductionWithValues.cs unless another file is named. Messages are the
 * desktop's own text, spelling included.
 * ============================================================================================ */
(function (global) {
    'use strict';

    /* frmwagesBillHeader as a modal (ShowDialog): /production/wages-bill in an overlay iframe; the
       caller continues (reset) only after the dialog closes, as the desktop blocks until then. */
    function p280OpenWages(refDocTypeId, refDocId, grossWeightTotal, onClosed) {
        var ov = document.createElement('div');
        ov.style.cssText = 'position:fixed;inset:0;z-index:9800;background:rgba(0,0,0,.35);';
        var fr = document.createElement('iframe');
        fr.src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: refDocTypeId, refDocId: refDocId,
                                                                  grossWeightTotal: grossWeightTotal || 0 });
        fr.style.cssText = 'position:absolute;inset:12px;width:calc(100% - 24px);height:calc(100% - 24px);border:1px solid #555;background:#fff;';
        ov.appendChild(fr); document.body.appendChild(ov);
        window.P280WagesClosed = function (r) { ov.remove(); window.P280WagesClosed = null; if (onClosed) onClosed(r); };
    }


    var P = global.P280, K = global.ReportKit, doc = global.document;
    var $ = P.$, esc = P.esc, box = P.box;
    var C = P.api + '/consumption';
    var DOC_TYPE = 181;

    /* ------------------------------------------------------------------------------ helpers */

    /** Conversion.ToDouble - text with thousands separators parses; anything else is 0. */
    function num(v) {
        if (v === null || v === undefined) return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v).replace(/,/g, '').trim();
        if (s === '' || !/^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(s)) return 0;
        return parseFloat(s);
    }
    /** Conversion.ToInt - Convert.ToInt32: numbers round half to even, strings must be integers. */
    function toInt(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return roundEven(v, 0);
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    /** Math.Round - half to even. */
    function roundEven(x, d) {
        var m = Math.pow(10, d || 0), y = x * m, f = Math.floor(y), r = y - f;
        var n;
        if (Math.abs(r - 0.5) < 1e-9) n = (f % 2 === 0) ? f : f + 1; else n = Math.round(y);
        return n / m;
    }
    /** .NET Framework double.ToString() - "G", 15 significant digits. */
    function netG(n) {
        if (typeof n !== 'number') n = num(n);
        if (!isFinite(n)) return isNaN(n) ? 'NaN' : (n > 0 ? 'Infinity' : '-Infinity');
        if (n === 0) return '0';
        var v = Number(n.toPrecision(15)), a = Math.abs(v);
        if (a >= 1e15 || a < 1e-5) {
            var e = v.toExponential(14).split('e');
            var mant = e[0].replace(/0+$/, '').replace(/\.$/, '');
            var ex = parseInt(e[1], 10);
            return mant + 'E' + (ex < 0 ? '-' : '+') + (Math.abs(ex) < 10 ? '0' : '') + Math.abs(ex);
        }
        return String(v);
    }
    /** "#,#.##" - zero shows nothing and a fraction has no leading zero. */
    function hashFmt(v) {
        var n = num(v);
        if (roundEven(Math.abs(n), 2) === 0) return '';
        var s = K.num(Math.abs(n), 2);
        if (s.indexOf('0.') === 0) s = s.slice(1);
        return (n < 0 ? '-' : '') + s;
    }
    function f2(v) { return (v === '' || v === null || v === undefined) ? '' : K.num(v, 2); }        /* "#,##0.##" */
    function fRate(v) { return (v === '' || v === null || v === undefined) ? '' : K.fixed(num(v), S.rateDec); }
    function fAmt(v) { return (v === '' || v === null || v === undefined) ? '' : K.fixed(num(v), S.amountDec); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function nowTime() { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); }
    function addDays(ymd, n) {
        var p = ymd.split('-'), d = new Date(+p[0], +p[1] - 1, +p[2] + n);
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    function datePart(v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v || '')); return m ? m[1] : ''; }
    function timePart(v) { var m = /[T ](\d{2}:\d{2}(:\d{2})?)/.exec(String(v || '')); return m ? (m[1].length === 5 ? m[1] + ':00' : m[1]) : '00:00:00'; }
    function ci(row, name) {
        if (!row) return undefined;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return undefined;
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    /** DateTime.ToString("dd-MMM-yy") / ("dd-MMM-yyyy"); an empty value is DateTime.MinValue. */
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dmy(v, longYear) {
        var d = datePart(v);
        if (!d) return longYear ? '01-Jan-0001' : '01-Jan-01';
        var p = d.split('-');
        return p[2] + '-' + MON[+p[1] - 1] + '-' + (longYear ? p[0] : p[0].slice(2));
    }
    /** DateTime.ToShortDateString() - dd/MM/yyyy, the culture the desktop runs under. */
    function shortDate(v) {
        var d = datePart(v);
        if (!d) return '01/01/0001';
        var p = d.split('-');
        return p[2] + '/' + p[1] + '/' + p[0];
    }

    /* ----------------------------------------------------------------------------- pickers */

    /** DDL.BindDDL. ZeroIndex adds a "0" row; otherwise the combo simply starts empty. */
    function fill(id, rows, valueKey, textKey, zeroIndex, extra) {
        var sel = $(id);
        if (!sel) return;
        var html = '<option value="' + (zeroIndex ? '0' : '') + '"></option>';
        (rows || []).forEach(function (r) {
            html += '<option value="' + esc(ci(r, valueKey)) + '"' + (extra ? extra(r) : '') + '>' + esc(ci(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel.selectedIndex = 0;
    }
    function clearList(id) { var s = $(id); if (s) { s.innerHTML = '<option value=""></option>'; s.selectedIndex = 0; } }
    function selVal(id) { var s = $(id); return s ? toInt(s.value) : 0; }
    function hasRow(id) { var s = $(id); return !!(s && s.selectedIndex > 0); }        /* ActiveRow != null */
    function selText(id) { var s = $(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent : ''; }
    function selData(id, key) { var s = $(id); return s && s.selectedIndex > 0 ? (s.options[s.selectedIndex].getAttribute('data-' + key) || '') : ''; }
    function setSel(id, v) {
        var s = $(id);
        if (!s) return false;
        var val = String(v === null || v === undefined ? '' : v);
        for (var i = 0; i < s.options.length; i++) {
            if (i > 0 && s.options[i].value === val) { s.selectedIndex = i; return true; }
        }
        return false;
    }
    function setSelText(id, t) {
        var s = $(id);
        if (!s) return;
        for (var i = 1; i < s.options.length; i++) if (s.options[i].textContent === t) { s.selectedIndex = i; return; }
        s.selectedIndex = 0;
    }
    function clearText(id) { var s = $(id); if (s) s.selectedIndex = 0; }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function setv(id, v) { var e = $(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function focus(id) { var e = $(id); if (!e) return; var w = e.__dtcombo; if (w && w.input) w.input.focus(); else e.focus(); }

    if (global.DesktopCombo) {
        /* dtitemConsumption: Id hidden, ItemName / ItemCode drawn (col 1 width 300, :1396). */
        global.DesktopCombo.define('p280item', [
            { caption: 'Item', flex: 3 },
            { caption: 'ItemCode', flex: 2, key: 'alt' }
        ]);
    }

    /* ------------------------------------------------------------------------------ state */

    var S = {
        sh: null, rights: {}, fifo: false, amountDec: 0, rateDec: 2, fyStart: null,
        recId: 0, voucherHeadId: 0, updateMode: false, updIdx: -1,
        rows: [], cur: -1,
        uoms: [], items: [],
        docTime: nowTime(),          /* the time part DateTimePicker.Value carries */
        pickTime: nowTime(),         /* ditto for the other pickers (set when the form loads) */
        consumptionSelected: false
    };

    function docDateTime() { return (val('txtDocDateConsumption') || today()) + 'T' + S.docTime; }
    function pickerDT(id) { var v = val(id); return v ? v + 'T' + S.pickTime : ''; }

    function isSaveVisible() { return !$('btnSaveConsumption').classList.contains('is-hidden'); }
    function isUpdateVisible() { return !$('BtnUpdateconsumption').classList.contains('is-hidden'); }
    function showSave(on) {
        $('btnSaveConsumption').classList.toggle('is-hidden', !on);
        $('BtnUpdateconsumption').classList.toggle('is-hidden', on);
    }
    function detailButtons(editing) {
        $('BtnAddConsumption').classList.toggle('is-hidden', editing);
        $('BtnUpdateDetailConsumption').classList.toggle('is-hidden', !editing);
        $('BtnCancelConsumption').classList.toggle('is-hidden', !editing);
    }

    /* ======================================================================== Load (:452) */

    function onLoad(sh) {
        S.sh = sh;
        S.rights = sh.rights || {};
        S.fifo = !!sh.fifoCgs;
        var R = S.rights;

        /* :488 - IsManualEntryOnInputNotAllowed hides the whole Detail panel. */
        if (sh.isManualEntryOnInputNotAllowed) $('panelManualEntryConsumption').classList.add('is-hidden');
        /* :493-496 */
        $('btnSaveConsumption').disabled = !R.save;
        $('BtnUpdateconsumption').disabled = !R.update;
        $('btnPrintVoucherConsumption').disabled = !R.print;
        $('BtnPrintSlipConsumption').disabled = !R.print;
        /* :498 - no Rate right: the field chooser of both consumption grids is disabled. */
        if (!R.rateAndAmount && global.GridBar) {
            global.GridBar.attach($('grdConsumptionHost'), null, { canChooseFields: false });
            global.GridBar.attach($('grdHistoryConsumptionMaiHost'), null, { canChooseFields: false });
        }
        /* :516 */
        $('BtnPendingForRatesFormConsumption').classList.toggle('is-hidden', S.fifo);
        detailButtons(false);
        /* :572-584 */
        if (!R.rateAndAmount) {
            $('txtRemarksDetailConsumption').style.width = '640px';
            ['txtRateConsumption', 'wrapRateUom', 'txtAmountConsumption', 'lblRateConsumption', 'lblRateUomConsumption',
             'lblAmountConsumption', 'BtnPendingForRatesFormConsumption', 'BtnGenerateRates', 'btnPrintVoucherConsumption']
                .forEach(function (id) { $(id).classList.add('is-hidden'); });
        }
        /* :585-588 */
        setv('txtFromDateConsupmtionHistory', addDays(today(), -3));
        setv('txtToDateConsupmtionHistory', today());
        jobOrderConsumptionHCombobind();
        /* :589-598 - Rate UOM is disabled in BOTH branches; Rate only under FIFO. */
        $('txtRateConsumption').disabled = S.fifo;
        $('CmbRateUomConsumption').disabled = true;

        setv('txtDocDateConsumption', today());
        return Promise.all([
            P.getJson(C + '/init').then(function (d) {
                S.amountDec = toInt(d.amountDecimals);
                S.rateDec = toInt(d.rateDecimals);
                S.fyStart = d.financialYearStart;
            }).catch(function (e) { box(e.message); }),
            uomFill(), jobOrderNoFill(), bindProductionDepartment()
        ]);
    }

    function bindProductionDepartment() {
        return P.getJson(P.api + '/departments').then(function (rows) {
            if (rows && rows.length) fill('CmbDepartmentConsumption', rows, 'id', 'name', false);
        }).catch(function (e) { box(e.message); });
    }

    /** JobOrderNoFill:636 - Id / PlanCode, DocumentTypeId carried hidden (BaseDocumentTypeId on save). */
    function jobOrderNoFill() {
        return P.getJson(P.api + '/job-orders').then(function (rows) {
            if (!rows || !rows.length) return;
            var keep = val('cmbJobOrderConsumption');
            fill('cmbJobOrderConsumption', rows, 'Id', 'PlanCode', false, function (r) {
                return ' data-dt="' + esc(ci(r, 'DocumentTypeId')) + '"';
            });
            if (keep) setSel('cmbJobOrderConsumption', keep);
        }).catch(function (e) { box(e.message); });
    }

    /** UOMFill:675 - UOMSchedule.Getall, kept as dtuomlst. */
    function uomFill() {
        return P.getJson(P.api + '/uom-schedules').then(function (rows) { S.uoms = rows || []; })
            .catch(function (e) { box(e.message); });
    }

    /** GenerateDocConsumption:1300. */
    function generateDoc() {
        return P.getJson(C + '/serial').then(function (d) {
            if (toInt(d.docNo) > 0) setv('txtDocNoConsumption', String(d.docNo));
        }).catch(function (e) { box(e.message); });
    }

    /** ConsumptionDetailComboBind:1330 - one call, split on ActivityType. */
    function consumptionDetailComboBind() {
        return P.getJson(C + '/detail-combos').then(function (all) {
            if (!all || !all.length) return;
            var ware = [], crop = [], lot = [], pack = [];
            S.items = [];
            all.forEach(function (r) {
                var t = str(ci(r, 'ActivityType'));
                var row = { Id: ci(r, 'Id'), name: ci(r, 'name') };
                if (t === 'Warehouse') ware.push(row);
                if (t === 'Items') S.items.push({ Id: ci(r, 'Id'), ItemName: ci(r, 'name'), ItemCode: ci(r, 'Code') });
                if (t === 'CropYear') crop.push(row);
                if (t === 'JobLot') lot.push(row);
                if (t === 'PackingType') pack.push(row);
            });
            rebind('cmbWarehouseConsumption', ware);
            if (S.items.length) {
                var id = selVal('CmbItemNameConsumption');
                bindItems();
                if (id > 0) setSel('CmbItemNameConsumption', id);
            }
            rebind('CmbCropYearConsumption', crop);
            rebind('CmbJobLotConsumption', lot);
            rebind('CmbPackingTypeConsumption', pack);
        }).catch(function (e) { box(e.message); });
    }
    function rebind(id, rows) {
        if (!rows.length) return;
        var keep = selVal(id);
        fill(id, rows, 'Id', 'name', true);
        if (keep > 0) setSel(id, keep);
    }
    function bindItems() {
        var byName = $('rdSearchByName').checked;
        fill('CmbItemNameConsumption', S.items, 'Id', byName ? 'ItemName' : 'ItemCode', true, function (r) {
            return ' data-code="' + esc(r.ItemCode) + '" data-alt="' + esc(byName ? r.ItemCode : r.ItemName) + '"';
        });
    }

    /** rdSearchByName_CheckedChanged:1552. */
    function searchModeChanged() {
        if (!S.items.length) return;
        var itemId = selVal('CmbItemNameConsumption') > 0 ? selVal('CmbItemNameConsumption') : 0;
        bindItems();
        $('CmbItemNameConsumption').setAttribute('data-dtcombo-caption', 'Item Name');
        setSel('CmbItemNameConsumption', itemId);
        focus('CmbItemNameConsumption');
    }

    /* ----------------------------------------------------------- Main group events (:1494) */

    var joSeq = 0;
    /** cmbJobOrderConsumption_Leave:1494 then GetPlantFeeder:692. */
    function jobOrderLeave() {
        var id = selVal('cmbJobOrderConsumption');
        var seq = ++joSeq;
        var gl = P.getJson(P.api + '/gl-accounts?jobOrderId=' + id).then(function (rows) {
            if (seq !== joSeq) return;
            if (rows && rows.length) {
                fill('cmbWIPAcConsumption', rows, 'WorkInProccessAcId', 'WorkInProcessAc', false);
                fill('cmbWIPItemConsumption', rows, 'WipItemId', 'ItemName', false);
                $('cmbWIPAcConsumption').selectedIndex = 1;
                $('cmbWIPItemConsumption').selectedIndex = 1;
            } else {
                clearList('cmbWIPAcConsumption');
                clearList('cmbWIPItemConsumption');
            }
        });
        var plantKeep = selVal('CmbPlantConsumption');
        var pl = P.getJson(P.api + '/plants?jobOrderId=' + id).then(function (rows) {
            if (seq !== joSeq) return;
            if (rows && rows.length) {
                if (hasRow('cmbJobOrderConsumption')) fill('CmbPlantConsumption', rows, 'PlantId', 'PlantName', false);
                if (plantKeep > 0) setSel('CmbPlantConsumption', plantKeep);
            } else {
                clearList('CmbPlantConsumption');
            }
        });
        return Promise.all([gl, pl]).catch(function (e) { box(e.message); });
    }

    /* ------------------------------------------------------------- Detail calculations */

    function uomRowsFor(itemId) {
        return S.uoms.filter(function (r) { return toInt(ci(r, 'ItemId')) === itemId; });
    }
    /** bindUomForConsumtionItems:1439. */
    function bindUomForItems() {
        if (!S.uoms.length) return;
        var packUom = selText('cmbPackUomConsumption');
        clearText('cmbPackUomConsumption');
        clearText('CmbRateUomConsumption');
        var rows = uomRowsFor(selVal('CmbItemNameConsumption'));
        if (rows.length) {
            var ex = function (r) { return ' data-eq="' + esc(ci(r, 'Equivalent')) + '"'; };
            fill('cmbPackUomConsumption', rows, 'Id', 'UOMCode', false, ex);
            fill('CmbRateUomConsumption', rows, 'Id', 'UOMCode', false, ex);
            setSelText('cmbPackUomConsumption', packUom);
            /* dtUom.Select("Equivalent='40'") */
            var hit = null;
            rows.forEach(function (r) { if (!hit && num(ci(r, 'Equivalent')) === 40) hit = r; });
            if (hit) setSel('CmbRateUomConsumption', ci(hit, 'Id')); else clearText('CmbRateUomConsumption');
        }
        packUomChanged(true);
        amountCalculation();
    }

    var stockSeq = 0;
    /** GetAvailableStockForConsumption:2546. */
    function getAvailableStock() {
        var seq = ++stockSeq;
        return P.getJson(C + '/stock' + P.qs({
            warehouseId: selVal('cmbWarehouseConsumption'), itemId: selVal('CmbItemNameConsumption'),
            jobLotId: selVal('CmbJobLotConsumption'), cropYear: selText('CmbCropYearConsumption').trim(),
            toDate: docDateTime()
        })).then(function (rows) {
            if (seq !== stockSeq) return;
            if (rows && rows.length) {
                setv('txtBalanceQtyConsumption', K.num(num(ci(rows[0], 'AvailableQty')), 3));
                setv('txtBalanceWeightConsumption', K.num(num(ci(rows[0], 'AvailableStock')), 4));
            } else {
                setv('txtBalanceQtyConsumption', '0');
                setv('txtBalanceWeightConsumption', '0');
            }
        }).catch(function (e) { box(e.message); });
    }

    function fifoRate(p) { return P.getJson(C + '/fifo-rate' + P.qs(p)).then(function (d) { return num(d.rate); }); }
    function cgsRate(p) { return P.getJson(C + '/cgs-rate' + P.qs(p)).then(function (d) { return num(d.rate); }); }

    var rateSeq = 0;
    /** GetAvgRateByItemAndJoblotForConsumption:2612. */
    function getAvgRate() {
        if (!S.rights.rateAndAmount) return Promise.resolve();
        var seq = ++rateSeq;
        var q = S.fifo
            ? fifoRate({ docDate: docDateTime(), itemId: selVal('CmbItemNameConsumption'), stockUom: selVal('cmbPackUomConsumption'),
                         warehouseId: selVal('cmbWarehouseConsumption'), jobLotId: selVal('CmbJobLotConsumption'),
                         packingTypeId: selVal('CmbPackingTypeConsumption'), cropYear: selText('CmbCropYearConsumption'),
                         itemQty: num(val('txtItemQtyConsumption')), netWeight: num(val('txtNetWeightConsumption')) })
            : cgsRate({ itemId: selVal('CmbItemNameConsumption'), docDate: docDateTime(), documentTypeId: DOC_TYPE,
                        recId: S.recId, jobLotId: selVal('CmbJobLotConsumption'), cropYearId: selVal('CmbCropYearConsumption'),
                        warehouseId: selVal('cmbWarehouseConsumption') });
        return q.then(function (rate) {
            if (seq !== rateSeq) return;
            setRate(rate > 0 ? netG(rate * 40) : '0');
        }).catch(function (e) { box(e.message); });
    }

    function setRate(t) { setv('txtRateConsumption', t); amountCalculation(); }
    function setNetWeight(t) { setv('txtNetWeightConsumption', t); amountCalculation(); }

    /** CalculateTotalWeightConsumption:2689. */
    function calculateTotalWeight() {
        if (hasRow('cmbPackUomConsumption') && selVal('cmbPackUomConsumption') > 0 && val('txtItemQtyConsumption') !== '') {
            var q = num(val('txtItemQtyConsumption').trim());
            var uom = num(selData('cmbPackUomConsumption', 'eq'));
            setNetWeight(netG(q * uom));
        } else {
            setNetWeight('0');
        }
    }
    /** AmountCalculationConsumption:2582. */
    function amountCalculation() {
        var nw = num(val('txtNetWeightConsumption')), rate = num(val('txtRateConsumption'));
        if (nw > 0 && selText('CmbRateUomConsumption') !== '' && rate > 0) {
            var amount = nw / num(selData('CmbRateUomConsumption', 'eq')) * rate;
            setv('txtAmountConsumption', amount > 0 ? netG(amount) : '0');
        } else {
            setv('txtAmountConsumption', '0');
        }
    }
    /** cmbPackUomConsumption_TextChanged:2727. */
    function packUomChanged(noRate) {
        calculateTotalWeight();
        if (S.fifo && !noRate) getAvgRate();
    }

    /* ---------------------------------------------------------------------------- grid */

    /* tableConsumption:535-564 - thirty columns, in this order. */
    var COLS = ['Id', 'RefDocumentTypeId', 'RefDocIdNo', 'RefDocSubIdNo', 'DocDate', 'DocNo', 'WareHouseId', 'WareHouse',
        'ItemId', 'Item', 'ItemCode', 'ItemUOMId', 'ItemUOM', 'ItemEquivalent', 'CropYear', 'JobLotId', 'JobLot',
        'PackingTypeId', 'PackingType', 'BalanceQty', 'BalanceWeight', 'Quantity', 'Weight', 'Rate', 'RateForCheck',
        'RateUOMId', 'RateUOM', 'RateUOMForCheck', 'ItemAmount', 'Remarks'];
    var INT_COLS = { Id: 1, RefDocumentTypeId: 1, RefDocIdNo: 1, RefDocSubIdNo: 1, DocNo: 1, WareHouseId: 1, ItemId: 1,
        ItemUOMId: 1, JobLotId: 1, PackingTypeId: 1, RateUOMId: 1 };
    var DBL_COLS = { ItemEquivalent: 1, BalanceQty: 1, BalanceWeight: 1, Quantity: 1, Weight: 1, Rate: 1, RateForCheck: 1 };
    /* grdConsumptionsettings:1861-1875 - hidden. */
    var HIDDEN = { Id: 1, DocDate: 1, DocNo: 1, ItemId: 1, RefDocumentTypeId: 1, RefDocIdNo: 1, RefDocSubIdNo: 1, JobLotId: 1,
        WareHouseId: 1, PackingTypeId: 1, ItemUOMId: 1, RateUOMId: 1, RateForCheck: 1, RateUOMForCheck: 1, ItemEquivalent: 1 };

    /** DataTable.Rows.Add - positional, typed columns converted as ADO.NET converts them. */
    function newRow(vals) {
        var r = {};
        COLS.forEach(function (c, i) { r[c] = typed(c, vals[i]); });
        return r;
    }
    function typed(c, v) {
        if (INT_COLS[c]) return toInt(v);
        if (DBL_COLS[c] || c === 'ItemAmount') return num(v);
        return v === null || v === undefined ? '' : (typeof v === 'number' ? netG(v) : String(v));
    }
    /** Assigning text to a typed DataTable cell - ADO.NET refuses what it cannot parse. */
    function assign(r, c, text) {
        if (INT_COLS[c] || DBL_COLS[c] || c === 'ItemAmount') {
            var s = String(text === null || text === undefined ? '' : text).trim();
            if (typeof text === 'number') { r[c] = text; return; }
            if (!/^[+-]?(\d{1,3}(,\d{3})*|\d+)?(\.\d+)?$/.test(s) || s === '' || s === '.') {
                throw new Error('Input string was not in a correct format.Couldn\'t store <' + s + '> in ' + c
                    + ' Column.  Expected type is ' + (INT_COLS[c] ? 'Int32' : (c === 'ItemAmount' ? 'Decimal' : 'Double')) + '.');
            }
            r[c] = INT_COLS[c] ? toInt(s.replace(/,/g, '')) : num(s);
            return;
        }
        r[c] = text === null || text === undefined ? '' : String(text);
    }
    function isRef(r) { return r.RefDocumentTypeId > 0 || r.RefDocIdNo > 0 || r.RefDocSubIdNo > 0; }
    function isRefAll(r) { return r.RefDocumentTypeId > 0 && r.RefDocIdNo > 0 && r.RefDocSubIdNo > 0; }

    function colVisible(c) {
        if (HIDDEN[c]) return false;
        if (!S.rights.rateAndAmount && (c === 'Rate' || c === 'RateUOM' || c === 'ItemAmount')) return false;
        return true;
    }
    function cellText(c, v) {
        if (c === 'BalanceQty' || c === 'BalanceWeight' || c === 'Quantity' || c === 'Weight') return f2(v);
        if (c === 'Rate') return fRate(v);
        if (c === 'ItemAmount') return fAmt(v);
        if (typeof v === 'number') return netG(v);
        return str(v);
    }
    var RIGHT = { BalanceQty: 1, BalanceWeight: 1, Quantity: 1, Weight: 1, ItemAmount: 1 };
    var SUM = { Quantity: 1, Weight: 1, ItemAmount: 1, BalanceQty: 1, BalanceWeight: 1 };

    /** grdConsumption bound to tableConsumption, grdConsumptionsettings:1803. */
    function renderGrid() {
        var host = $('grdConsumptionHost');
        if (!S.rows.length && !S.bound) { host.innerHTML = ''; $('grdConsumptionNav').textContent = ''; return; }
        var cols = COLS.filter(colVisible);
        var h = '<table class="gx" id="grdConsumption"><thead><tr><th style="width:35px;">X</th>';
        cols.forEach(function (c) { h += '<th' + (RIGHT[c] ? ' class="num"' : '') + '>' + esc(c) + '</th>'; });
        h += '</tr></thead><tbody>';
        S.rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === S.cur ? ' class="cur"' : '') + '><td><button type="button" class="cellbtn" data-del="' + i + '">X</button></td>';
            cols.forEach(function (c) {
                if (c === 'Quantity' || c === 'Weight') {
                    h += '<td class="num"><input class="cell" data-edit="' + c + '" data-i="' + i + '" value="' + esc(f2(r[c])) + '"></td>';
                } else {
                    h += '<td' + (RIGHT[c] ? ' class="num"' : '') + '>' + esc(cellText(c, r[c])) + '</td>';
                }
            });
            h += '</tr>';
        });
        h += '</tbody><tfoot><tr><td></td>';
        cols.forEach(function (c) {
            if (SUM[c]) {
                var t = 0;
                S.rows.forEach(function (r) { t += num(r[c]); });
                h += '<td class="num">' + esc(c === 'ItemAmount' ? fAmt(t) : f2(t)) + '</td>';
            } else h += '<td></td>';
        });
        h += '</tr></tfoot></table>';
        host.innerHTML = h;
        if (K && K.filterRow) K.filterRow($('grdConsumption'));
        nav();
    }
    function nav() {
        var n = visibleRows().length;
        $('grdConsumptionNav').textContent = 'Record: ' + (S.cur >= 0 ? (S.cur + 1) : 0) + ' of ' + n;
    }
    /** grdConsumption.GetRows() - the rows the current filter shows, in view order. */
    function visibleRows() {
        var t = $('grdConsumption');
        if (!t) return S.rows.map(function (r, i) { return i; });
        var out = [];
        Array.prototype.forEach.call(t.tBodies[0].rows, function (tr) {
            if (tr.style.display !== 'none') out.push(toInt(tr.getAttribute('data-i')));
        });
        return out;
    }
    function setCur(i) {
        S.cur = i;
        var t = $('grdConsumption');
        if (t) Array.prototype.forEach.call(t.tBodies[0].rows, function (tr) { tr.classList.toggle('cur', toInt(tr.getAttribute('data-i')) === i); });
        nav();
    }

    /** grdConsumption_ColumnButtonClick:2010. */
    function deleteRow(i) {
        var r = S.rows[i];
        if (!r) return;
        if (r.Id !== 0) { box('Can not Delete Saved Record...'); return; }
        if (!global.confirm('Are you sure to Delete?')) return;
        S.rows.splice(i, 1);
        if (S.cur >= S.rows.length) S.cur = S.rows.length - 1;
        renderGrid();
    }

    /** grdConsumption_DoubleClick:1916 - the row into the Detail group for editing. */
    function editRow() {
        var r = S.rows[S.cur];
        if (!r) return Promise.resolve();
        if (toInt(r.RefDocumentTypeId) > 0 || toInt(r.RefDocIdNo) > 0 || toInt(r.RefDocSubIdNo) > 0) {
            box('The record was not updated as it was loaded directly from the Loader.');
            return Promise.resolve();
        }
        S.updIdx = S.cur;
        setSel('cmbWarehouseConsumption', r.WareHouseId);
        setSel('CmbItemNameConsumption', r.ItemId);
        return itemLeave().then(function () {
            setSel('cmbPackUomConsumption', r.ItemUOMId);
            setSelText('CmbCropYearConsumption', r.CropYear);
            setSel('CmbJobLotConsumption', r.JobLotId);
            setSel('CmbPackingTypeConsumption', r.PackingTypeId);
            setv('txtBalanceQtyConsumption', f2(r.BalanceQty));
            setv('txtBalanceWeightConsumption', f2(r.BalanceWeight));
            setv('txtItemQtyConsumption', f2(r.Quantity));
            setv('txtNetWeightConsumption', f2(r.Weight));
            setv('txtRateConsumption', netG(num(r.Rate)));
            setSel('CmbRateUomConsumption', r.RateUOMId);
            setv('txtAmountConsumption', S.rights.rateAndAmount ? fAmt(r.ItemAmount) : fAmt(r.ItemAmount));
            setv('txtRemarksDetailConsumption', r.Remarks);
            detailButtons(true);
            focus('cmbWarehouseConsumption');
        });
    }

    /** grdConsumption_CellUpdated:2041 - Quantity and Weight are the only editable cells. */
    function cellUpdated(i, col, text) {
        var r = S.rows[i];
        if (!r) return Promise.resolve();
        var n = String(text).replace(/,/g, '').trim();
        if (n === '' || isNaN(Number(n))) { renderGrid(); return Promise.resolve(); }
        r[col] = Number(n);
        var ref = isRef(r), refAll = isRefAll(r);
        var p = Promise.resolve();
        if (col === 'Quantity') {
            var qty = num(r.Quantity), uom = num(r.ItemEquivalent), weight = 0;
            if (r.BalanceQty > 0 && r.BalanceQty < qty && ref) {
                r.Quantity = 0; r.Weight = 0; qty = 0;
                box('Qty cannot greater than BalanceQty Please check!');
            }
            if (refAll) {
                weight = r.BalanceWeight / r.BalanceQty * qty;
                if (r.BalanceWeight < weight) { weight = 0; r.Weight = 0; box('Weight cannot greater than Balance Weight Please check!'); }
            } else {
                weight = qty * uom;
            }
            r.Weight = weight;
            if (S.fifo && r.RefDocumentTypeId <= 0 && r.RefDocIdNo <= 0 && r.RefDocSubIdNo <= 0) {
                p = fifoRate({ docDate: docDateTime(), warehouseId: r.WareHouseId, itemId: r.ItemId, jobLotId: r.JobLotId,
                               packingTypeId: r.PackingTypeId, cropYear: r.CropYear, itemQty: r.Quantity, netWeight: num(f2(r.Weight)) })
                    .then(function (rate) { r.Rate = rate * 40; });
            }
            return p.then(function () { setAmount(r, weight / num(r.RateUOMForCheck) * num(r.Rate)); })
                .catch(function (e) { box(e.message); }).then(renderGrid);
        }
        if (col === 'Weight') {
            var w = num(r.Weight);
            if (r.BalanceWeight < w && ref) { r.Weight = 0; w = 0; box('Weight cannot greater than BalanceWeight Please check!'); }
            if (S.fifo && r.RefDocumentTypeId <= 0 && r.RefDocIdNo <= 0 && r.RefDocSubIdNo <= 0) {
                p = fifoRate({ docDate: docDateTime(), warehouseId: r.WareHouseId, itemId: r.ItemId, jobLotId: r.JobLotId,
                               packingTypeId: r.PackingTypeId, cropYear: r.CropYear, itemQty: r.Quantity, netWeight: num(f2(r.Weight)) })
                    .then(function (rate) { r.Rate = rate * 40; });
            }
            return p.then(function () { setAmount(r, w / num(r.RateUOMForCheck) * num(r.Rate)); })
                .catch(function (e) { box(e.message); }).then(renderGrid);
        }
        return p;
    }
    /** ItemAmount is a decimal column: Infinity / NaN cannot be stored. */
    function setAmount(r, v) {
        if (!isFinite(v)) throw new Error('Value was either too large or too small for a Decimal.');
        r.ItemAmount = v;
    }

    /* -------------------------------------------------------------------- Detail buttons */

    /** FormValidationOfDetailConsumption:1698. */
    function validateDetail() {
        var chk = [
            ['cmbWarehouseConsumption', 'Warehouse Field Required'],
            ['CmbItemNameConsumption', 'ItemName Field Required'],
            ['cmbPackUomConsumption', 'UOM Field Required'],
            ['CmbCropYearConsumption', 'CropYear Field Required'],
            ['CmbJobLotConsumption', 'JobLot Field Required'],
            ['CmbPackingTypeConsumption', 'Packing Type Field Required']
        ];
        for (var i = 0; i < chk.length; i++) {
            if (!hasRow(chk[i][0]) || selVal(chk[i][0]) === 0) { box(chk[i][1]); focus(chk[i][0]); return false; }
        }
        if (val('txtItemQtyConsumption').trim() === '' || num(val('txtItemQtyConsumption').trim()) === 0) {
            box('Item Qty Field Required'); focus('txtItemQtyConsumption'); return false;
        }
        if (S.rights.rateAndAmount) {
            if (val('txtRateConsumption').trim() === '' || num(val('txtRateConsumption').trim()) === 0) {
                box('Rate Field Required'); focus('txtRateConsumption'); return false;
            }
            if (!hasRow('CmbRateUomConsumption') || num(val('CmbRateUomConsumption')) === 0) {
                box('RateUom Field Required'); focus('CmbRateUomConsumption'); return false;
            }
            if (val('txtAmountConsumption').trim() === '' || num(val('txtAmountConsumption').trim()) === 0) {
                box('Amount Field Required'); focus('txtAmountConsumption'); return false;
            }
        }
        if (val('txtNetWeightConsumption').trim() === '' || num(val('txtNetWeightConsumption').trim()) === 0) {
            box('NetWeight Field Required'); focus('txtNetWeightConsumption'); return false;
        }
        return true;
    }

    /** BtnAddConsumption_Click:1772. */
    function addDetail() {
        try {
            if (!validateDetail()) return;
            for (var i = 0; i < S.rows.length; i++) {
                if (toInt(S.rows[i].RefDocumentTypeId) > 0) throw new Error('Record cannot be add in grid because record already add from loader');
            }
            if (!hasRow('CmbRateUomConsumption')) throw new Error('Object reference not set to an instance of an object.');
            S.rows.push(newRow([0, 0, 0, 0, 0, 0,
                selVal('cmbWarehouseConsumption'), selText('cmbWarehouseConsumption'),
                selVal('CmbItemNameConsumption'), selText('CmbItemNameConsumption'), selData('CmbItemNameConsumption', 'code'),
                selVal('cmbPackUomConsumption'), selText('cmbPackUomConsumption'), num(selData('cmbPackUomConsumption', 'eq')),
                selText('CmbCropYearConsumption'), selVal('CmbJobLotConsumption'), selText('CmbJobLotConsumption'),
                selVal('CmbPackingTypeConsumption'), selText('CmbPackingTypeConsumption'),
                num(val('txtBalanceQtyConsumption')), num(val('txtBalanceWeightConsumption')),
                num(val('txtItemQtyConsumption')), num(val('txtNetWeightConsumption')),
                num(val('txtRateConsumption')), num(val('txtRateConsumption')),
                selVal('CmbRateUomConsumption'),
                /* RateUOM gets Conversion.ToDouble of the combo TEXT - "0" unless the code is a number */
                num(selText('CmbRateUomConsumption')),
                num(selData('CmbRateUomConsumption', 'eq')),
                num(val('txtAmountConsumption')), val('txtRemarksDetailConsumption')]));
            S.bound = true;
            renderGrid();
            resetDetail();
            focus('cmbWarehouseConsumption');
        } catch (e) { box(e.message); }
    }

    /** BtnUpdateDetailConsumption_Click:1955. */
    function updateDetail() {
        try {
            if (!validateDetail()) return;
            var r = S.rows[S.updIdx];
            if (!r) throw new Error('There is no row at position ' + S.updIdx + '.');
            r.WareHouseId = selVal('cmbWarehouseConsumption');
            r.WareHouse = selText('cmbWarehouseConsumption').trim();
            r.JobLotId = selVal('CmbJobLotConsumption');
            r.JobLot = selText('CmbJobLotConsumption').trim();
            r.PackingTypeId = selVal('CmbPackingTypeConsumption');
            r.PackingType = selText('CmbPackingTypeConsumption').trim();
            r.ItemId = selVal('CmbItemNameConsumption');
            r.Item = selText('CmbItemNameConsumption').trim();
            r.ItemCode = selData('CmbItemNameConsumption', 'code');
            r.ItemUOMId = selVal('cmbPackUomConsumption');
            r.ItemUOM = selText('cmbPackUomConsumption').trim();
            r.ItemEquivalent = selVal('cmbPackUomConsumption') > 0 ? num(selData('cmbPackUomConsumption', 'eq')) : 0;
            r.CropYear = selText('CmbCropYearConsumption').trim();
            assign(r, 'Quantity', val('txtItemQtyConsumption').trim());
            assign(r, 'BalanceQty', val('txtBalanceQtyConsumption').trim());
            assign(r, 'BalanceWeight', val('txtBalanceWeightConsumption').trim());
            assign(r, 'Weight', val('txtNetWeightConsumption').trim());
            assign(r, 'Rate', val('txtRateConsumption').trim());
            assign(r, 'RateForCheck', val('txtRateConsumption').trim());
            /* RateUOM gets the combo's TEXT here - unlike Add, which stores ToDouble(text). */
            r.RateUOM = selText('CmbRateUomConsumption').trim();
            r.RateUOMForCheck = netG(selVal('CmbRateUomConsumption') > 0 ? num(selData('CmbRateUomConsumption', 'eq')) : 0);
            r.RateUOMId = selVal('CmbRateUomConsumption');
            assign(r, 'ItemAmount', val('txtAmountConsumption').trim());
            r.Remarks = val('txtRemarksDetailConsumption').trim();
            renderGrid();
            resetDetail();
            focus('cmbWarehouseConsumption');
        } catch (e) { box(e.message); renderGrid(); }
    }

    /** BtnCancelConsumption_Click:1995. */
    function cancelDetail() { detailButtons(false); resetDetail(); }

    /** ResetConsumptionDetail:1633 - note it clears the HEADER remarks and keeps the detail remarks. */
    function resetDetail() {
        clearText('cmbWarehouseConsumption');
        clearText('CmbItemNameConsumption');
        clearList('cmbPackUomConsumption');
        clearText('CmbCropYearConsumption');
        clearText('CmbJobLotConsumption');
        clearText('CmbPackingTypeConsumption');
        ['txtBalanceQtyConsumption', 'txtBalanceWeightConsumption', 'txtItemQtyConsumption', 'txtNetWeightConsumption',
         'txtRemarksHeaderConsumption', 'txtRateConsumption', 'txtAmountConsumption'].forEach(function (id) { setv(id, ''); });
        clearList('CmbRateUomConsumption');
        detailButtons(false);
    }

    /* ------------------------------------------------------------------ Main commands */

    /** ResetConsumptionMain:1608. */
    function resetMain() {
        S.recId = 0;
        S.voucherHeadId = 0;
        setv('txtRemarksHeaderConsumption', '');
        showSave(true);
        S.updateMode = false;
        $('cmbJobOrderConsumption').disabled = false;
        S.rows = [];
        S.cur = -1;
        S.bound = false;
        renderGrid();
        var g = generateDoc();
        clearList('cmbJobOrderConsumption');
        var j = jobOrderNoFill();
        clearText('CmbPlantConsumption');
        return Promise.all([g, j]);
    }

    /** BtnRefreshConsumption_Click:1593. */
    function refreshMain() {
        return Promise.all([consumptionDetailComboBind(), jobOrderNoFill()]);
    }

    /** FormValidationConsumption:1663. */
    function validateMain() {
        if (val('txtDocNoConsumption').trim() === '' || val('txtDocNoConsumption') === '0') {
            box('document Number Field Required'); focus('txtDocNoConsumption'); return false;
        }
        var chk = [
            ['cmbJobOrderConsumption', 'Job Order Number Field Required'],
            ['CmbPlantConsumption', 'Plant Name Field Required'],
            ['cmbWIPAcConsumption', 'WorkInProcess Ac Field Required'],
            ['cmbWIPItemConsumption', 'WIP Item Field Required']
        ];
        for (var i = 0; i < chk.length; i++) {
            if (!hasRow(chk[i][0]) || selVal(chk[i][0]) === 0) { box(chk[i][1]); focus(chk[i][0]); return false; }
        }
        return true;
    }

    /** InsertConsumption:2174. */
    function insertConsumption(btn) {
        if (!validateMain()) return Promise.resolve();
        if (!global.confirm(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
        var vis = visibleRows();
        if (!vis.length) { box('Enter Detail First ...'); return Promise.resolve(); }
        var rows = [];
        for (var k = 0; k < vis.length; k++) {
            var r = S.rows[vis[k]];
            if (!(num(f2(r.Quantity)) > 0) || !(num(f2(r.Weight)) > 0)) continue;
            if (String(r.WareHouseId) === '' || r.WareHouseId === 0) { box('WareHouse Field Require in Consumption Detail'); return Promise.resolve(); }
            if (String(r.ItemId) === '' || r.ItemId === 0) { box('Item Field Require in Consumption Detail'); return Promise.resolve(); }
            if (str(r.CropYear) === '' || str(r.CropYear) === '0') { box('CropYear Field Require in Consumption Detail'); return Promise.resolve(); }
            if (String(r.JobLotId) === '') { box('JobLot Field Require in Consumption Detail'); return Promise.resolve(); }
            rows.push({
                rowIndex: k, id: r.Id, refDocumentTypeId: r.RefDocumentTypeId, refDocIdNo: r.RefDocIdNo,
                refDocSubIdNo: r.RefDocSubIdNo, warehouseId: String(r.WareHouseId), itemId: String(r.ItemId),
                itemUomId: r.ItemUOMId, cropYear: str(r.CropYear), jobLotId: String(r.JobLotId), packingTypeId: r.PackingTypeId,
                quantity: r.Quantity, weight: r.Weight, rate: r.Rate, itemAmount: r.ItemAmount, rateUomId: r.RateUOMId,
                rateUom: str(r.RateUOM), rateUomForCheck: str(r.RateUOMForCheck), remarks: str(r.Remarks)
            });
        }
        if (!rows.length) { box('Grid Record Not Found'); return Promise.resolve(); }
        var jo = $('cmbJobOrderConsumption');
        var body = {
            id: S.recId, docCode: toInt(val('txtDocNoConsumption')), docDate: docDateTime(),
            mainRemarks: val('txtRemarksHeaderConsumption'), plantId: selVal('CmbPlantConsumption'),
            wipAccountId: selVal('cmbWIPAcConsumption'), wipItemId: selVal('cmbWIPItemConsumption'),
            departmentId: selVal('CmbDepartmentConsumption'), jobOrderId: selVal('cmbJobOrderConsumption'),
            jobOrderNo: selText('cmbJobOrderConsumption').trim(),
            baseDocumentTypeId: toInt(jo.options[jo.selectedIndex].getAttribute('data-dt')),
            rows: rows
        };
        return P.busy(btn, function () {
            return P.postJson(C + '/save', body).then(function (d) {
                box(d.message);
                if (d.openWages) {
                    /* :2318-2324 - bill removed on the server, then frmwagesBillHeader (181) as a dialog. */
                    return new Promise(function (done) {
                        p280OpenWages(181, d.id, d.grossWeight, function () { done(); });
                    }).then(function () { return resetMain(); });
                }
                return resetMain();
            }).catch(function (e) { box(e.message); });
        });
    }

    /** ReadByIdConsumption:2341. */
    function readById(id) {
        S.recId = id;
        showSave(false);
        return P.getJson(C + '/' + id).then(function (d) {
            if (!d || !d.found) return;
            var h = d.header;
            if (str(h.EntryType) !== 'Consumption') return;
            if (h.IsApproved === true || h.IsApproved === 1 || h.IsApproved === '1' || h.IsApproved === 'true') {
                throw new Error("You Can't Update Approved Record");
            }
            if (str(h.PlanStatus) !== 'In Process') {
                throw new Error("You Can't Update Record with " + str(h.PlanStatus) + ' JobPlanStatus');
            }
            setv('txtDocNoConsumption', str(h.DocCode));
            setv('txtDocDateConsumption', datePart(h.DocDate));
            S.docTime = timePart(h.DocDate);
            setv('txtRemarksHeaderConsumption', str(h.MainRemarks));
            setSel('CmbDepartmentConsumption', h.EBDepartmentId);
            setSel('cmbJobOrderConsumption', h.InvJobOrderId);
            return jobOrderLeave().then(function () {
                setSel('CmbPlantConsumption', h.PlantId);
                S.rows = [];
                (d.consumption || []).forEach(function (x) {
                    P.selectTab(2);
                    showSub('form');
                    S.rows.push(newRow([ci(x, 'Id'), ci(x, 'RefDocumentTypeId'), ci(x, 'RefDocNoId'), ci(x, 'RefDocSubIdNo'),
                        dmy(ci(x, 'RefDocDate'), false), ci(x, 'RefDocNo'), ci(x, 'WarehouseId'), ci(x, 'WareHouseName'),
                        ci(x, 'ItemId'), ci(x, 'ItemName'), ci(x, 'ItemCode'), ci(x, 'ItemUomId'), ci(x, 'PackUom'),
                        ci(x, 'Equivalent'), ci(x, 'CropBatch'), ci(x, 'JobLotId'), ci(x, 'JobLotDescription'),
                        ci(x, 'PackingtypeId'), ci(x, 'PackTypeDesc'), ci(x, 'Qty'), ci(x, 'Weight'), ci(x, 'Qty'),
                        ci(x, 'Weight'), roundEven(num(ci(x, 'Rate')), 2), roundEven(num(ci(x, 'Rate')), 2),
                        ci(x, 'RateUOMId'), ci(x, 'RateUOM'), ci(x, 'RateEquivalent'), ci(x, 'Amount'), ci(x, 'Remarks')]));
                });
                S.cur = S.rows.length ? 0 : -1;
                S.bound = true;
                renderGrid();
                return P.getJson(C + '/voucher-head?id=' + S.recId).then(function (v) { S.voucherHeadId = toInt(v.voucherHeadId); });
            });
        }).catch(function (e) { box(e.message); });
    }

    /** UpdateUomForLoadingPendingForRateEntriesConsumption:2525 - RateUom 40 where it is not an integer. */
    function updateUomForPendingRates() {
        visibleRows().forEach(function (i) {
            var r = S.rows[i];
            if (toInt(r.RateUOM) === 0) r.RateUOM = '40';
        });
        renderGrid();
    }

    /** AvgRateUpdateOnDocDateChange:3345 - "Generate Rates". */
    function generateRates() {
        var list = S.rows.slice();
        var i = 0;
        function next() {
            if (i >= list.length) { renderGrid(); return Promise.resolve(); }
            var r = list[i++];
            if (r.RefDocumentTypeId !== 0 || r.RefDocIdNo !== 0 || r.RefDocSubIdNo !== 0) { renderGrid(); return Promise.resolve(); }
            var eq = num(r.RateUOMForCheck), weight = num(r.Weight);
            var p;
            if (S.fifo) {
                var q = { docDate: docDateTime(), stockUom: r.ItemUOMId, itemId: r.ItemId, warehouseId: r.WareHouseId,
                          packingTypeId: r.PackingTypeId, jobLotId: r.JobLotId, cropYear: r.CropYear, itemQty: r.Quantity,
                          netWeight: weight };
                if (S.recId > 0) { q.documentTypeId = DOC_TYPE; q.id = S.recId; }
                p = fifoRate(q).then(function (x) {
                    var rate = roundEven(x, 3) * 40;
                    if (rate > 0) { r.Rate = rate; setAmount(r, weight / eq * rate); }
                    else { r.Rate = 0; r.ItemAmount = 0; }
                    return rate;
                });
            } else {
                p = cgsRate({ itemId: r.ItemId, docDate: docDateTime(), documentTypeId: DOC_TYPE, recId: S.recId,
                              jobLotId: r.JobLotId, cropYearId: 0, cropYear: r.CropYear, warehouseId: r.WareHouseId })
                    .then(function (x) { return x * 40; });
            }
            return p.then(function (rate) {
                if (rate > 0) { r.Rate = rate; setAmount(r, weight / eq * rate); }
                return next();
            });
        }
        return next().catch(function (e) { renderGrid(); box(e.message); });
    }

    function printSlip() {
        /* ProductionConsumptionReport_623: an Id of 0 is "No Record Found For Display". */
        if (S.recId === 0) { box('No Record Found For Display'); return; }
        return global.CrystalPrint.open('prod-623', { id: S.recId }, 'BtnPrintSlipConsumption');
    }
    function printVoucher() {
        var win = global.CrystalPrint.reserve();
        return P.busy('btnPrintVoucherConsumption', function () {
            return P.getJson(C + '/voucher-head?id=' + S.recId).then(function (d) {
                var vh = toInt(d.voucherHeadId);
                if (vh === 0) { global.CrystalPrint.release(win); box('VoucherId Not Found'); return; }
                return global.CrystalPrint.open('acc-118', { id: vh, documentTypeId: 0 }, null, win);
            }).catch(function (e) { global.CrystalPrint.release(win); box(e.message); });
        });
    }

    /* ============================================================ Consumption History (tabPage10) */

    function showSub(which) {
        $('subForm').classList.toggle('on', which === 'form');
        $('subHistory').classList.toggle('on', which === 'history');
        $('tabPage9Btn').classList.toggle('active', which === 'form');
        $('tabPage10Btn').classList.toggle('active', which === 'history');
    }

    /** jobOrderConsumptionHCombobind:2858. */
    function jobOrderConsumptionHCombobind() {
        return P.getJson(C + '/history/job-orders').then(function (rows) {
            if (rows && rows.length) fill('CmbJobOrderNoConsupmtionHistory', rows, 'Id', 'ReferenceName', false);
            else clearList('CmbJobOrderNoConsupmtionHistory');
        }).catch(function (e) { box(e.message); });
    }

    /** ResetConsumptionhistory:2908 - To date and the grids are left as they are. */
    function resetHistory() {
        setv('txtDocNofromConsupmtionHistory', '');
        setv('txtDocNoToConsupmtionHistory', '');
        clearText('CmbJobOrderNoConsupmtionHistory');
        setv('txtFromDateConsupmtionHistory', addDays(today(), -3));
        $('txtFromDateConsupmtionHistory').focus();
    }

    var H = { rows: [], cur: -1, detail: null, hidden: {} };
    var HCOLS = ['Id', 'DocSrNo', 'DocDate', 'JobOrderNo', 'PlantName', 'EntryType', 'Remarks', 'IsApproved', 'StartDate',
                 'EndDate', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser', 'PlanStatus', 'WagesId'];

    /** GrdHistoryConsumptionMain:2928 - nothing found leaves the grid as it was. */
    function showHistory() {
        return P.busy('BtnShowConsupmtionHistory', function () {
            return P.getJson(C + '/history' + P.qs({
                from: pickerDT('txtFromDateConsupmtionHistory'), to: pickerDT('txtToDateConsupmtionHistory'),
                docFrom: val('txtDocNofromConsupmtionHistory'), docTo: val('txtDocNoToConsupmtionHistory'),
                jobOrderId: selVal('CmbJobOrderNoConsupmtionHistory')
            })).then(function (rows) {
                if (!rows || !rows.length) return;
                H.rows = rows.map(function (x) {
                    return { Id: ci(x, 'Id'), DocSrNo: ci(x, 'DocCode'), DocDate: ci(x, 'DocDate'), JobOrderNo: ci(x, 'InvJobOrderNo'),
                        PlantName: ci(x, 'PlantName'), EntryType: ci(x, 'EntryType'), Remarks: ci(x, 'MainRemarks'),
                        IsApproved: ci(x, 'JobOrderApprovedStatus'), StartDate: ci(x, 'StartDate'), EndDate: ci(x, 'EndDate'),
                        EntryDate: ci(x, 'EntryDate'), EntryUser: ci(x, 'EntryUser'), ModifyDate: ci(x, 'ModifyDate'),
                        ModifyUser: ci(x, 'ModifyUser'), PlanStatus: ci(x, 'PlanStatus'), WagesId: ci(x, 'WagesId') };
                });
                H.cur = -1;
                renderHistory();
            }).catch(function (e) { box(e.message); });
        });
    }

    function historyColVisible(c) {
        if (Object.prototype.hasOwnProperty.call(H.hidden, c)) return !H.hidden[c];
        return c !== 'Id' && c !== 'WagesId';
    }
    function dateCell(c, v) {
        if (c === 'DocDate' || c === 'StartDate' || c === 'EndDate') return v ? K.dMMMyyyy(v) : '';
        if (c === 'EntryDate' || c === 'ModifyDate') return v ? K.dmyhm(v) : '';
        return str(v);
    }
    /** HistoryConsumptionMainSetting:2994 - button columns by rights, in the desktop's positions. */
    function renderHistory() {
        var R = S.rights, btns = [];
        if (R.update) btns.push('Edit');
        if (R.print) { btns = btns.length ? ['Edit', 'Print', 'WagesPrint'] : ['WagesPrint', 'Print']; }
        var cols = HCOLS.filter(historyColVisible);
        var h = '<table class="gx alt" id="grdHistoryConsumptionMai"><thead><tr>';
        btns.forEach(function (b) { h += '<th style="width:' + (b === 'Edit' ? 40 : 60) + 'px;">' + b + '</th>'; });
        cols.forEach(function (c) { h += '<th>' + esc(c) + '</th>'; });
        h += '</tr></thead><tbody>';
        H.rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === H.cur ? ' class="cur"' : '') + '>';
            btns.forEach(function (b) { h += '<td><button type="button" class="cellbtn" data-hb="' + b + '" data-i="' + i + '">' + b + '</button></td>'; });
            cols.forEach(function (c) {
                if (c === 'DocSrNo') h += '<td><span class="doclink" data-open="' + i + '" title="Open this consumption">' + esc(str(r[c])) + '</span></td>';
                else h += '<td>' + esc(dateCell(c, r[c])) + '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody></table>';
        $('grdHistoryConsumptionMaiHost').innerHTML = h;
        if (K && K.filterRow) K.filterRow($('grdHistoryConsumptionMai'));
        /* Print right without Update right: Columns["Edit"].Position throws (:3035). */
        if (R.print && !R.update) box('Object reference not set to an instance of an object.');
    }

    /** grdHistoryConsumptionMai_SelectionChanged:3048 -> GrdHistoryConsumptionDetailHeaderId:3060. */
    function historySelect(i) {
        H.cur = i;
        Array.prototype.forEach.call($('grdHistoryConsumptionMai').tBodies[0].rows, function (tr) {
            tr.classList.toggle('cur', toInt(tr.getAttribute('data-i')) === i);
        });
        var r = H.rows[i];
        if (!r) return;
        P.getJson(C + '/' + toInt(r.Id)).then(function (d) {
            if (!d || !d.found) return;
            var list = (d.consumption || []);
            if (!list.length) return;       /* the grid is re-bound inside the loop - no rows, no re-bind */
            var rows = [];
            list.forEach(function (x) {
                if (str(ci(x, 'EntryType')) !== 'Consumption') return;
                rows.push(newRow([ci(x, 'Id'), ci(x, 'RefDocumentTypeId'), ci(x, 'RefDocNoId'), ci(x, 'RefDocSubIdNo'),
                    dmy(ci(x, 'RefDocDate'), false), ci(x, 'RefDocNo'), ci(x, 'WarehouseId'), ci(x, 'WareHouseName'),
                    ci(x, 'ItemId'), ci(x, 'ItemName'), ci(x, 'ItemCode'), ci(x, 'ItemUomId'), ci(x, 'PackUom'),
                    ci(x, 'Equivalent'), ci(x, 'CropBatch'), ci(x, 'JobLotId'), ci(x, 'JobLotDescription'),
                    ci(x, 'PackingtypeId'), ci(x, 'PackTypeDesc'), ci(x, 'Qty'), ci(x, 'Weight'), ci(x, 'Qty'), ci(x, 'Weight'),
                    roundEven(num(ci(x, 'Rate')), 2), roundEven(num(ci(x, 'Rate')), 2), ci(x, 'RateUOMId'), ci(x, 'RateUOM'),
                    ci(x, 'RateEquivalent'), ci(x, 'Amount'), ci(x, 'Remarks')]));
            });
            renderHistoryDetail(rows);
        }).catch(function (e) { box(e.message); });
    }

    /** grdHistoryConsumptionDetailSettings:3127. */
    function renderHistoryDetail(rows) {
        var cols = COLS.filter(function (c) {
            if (HIDDEN[c]) return false;
            if (!S.rights.rateAndAmount && (c === 'Rate' || c === 'RateUOM' || c === 'ItemAmount')) return false;
            return true;
        });
        var h = '<table class="gx alt" id="grdHistoryConsumptionDetail"><thead><tr>';
        cols.forEach(function (c) { h += '<th' + (RIGHT[c] ? ' class="num"' : '') + '>' + esc(c) + '</th>'; });
        h += '</tr></thead><tbody>';
        rows.forEach(function (r) {
            h += '<tr>';
            cols.forEach(function (c) { h += '<td' + (RIGHT[c] ? ' class="num"' : '') + '>' + esc(cellText(c, r[c])) + '</td>'; });
            h += '</tr>';
        });
        h += '</tbody><tfoot><tr>';
        cols.forEach(function (c) {
            if (SUM[c]) {
                var t = 0;
                rows.forEach(function (r) { t += num(r[c]); });
                h += '<td class="num">' + esc(c === 'ItemAmount' ? fAmt(t) : f2(t)) + '</td>';
            } else h += '<td></td>';
        });
        h += '</tr></tfoot></table>';
        $('grdHistoryConsumptionDetailHost').innerHTML = h;
        if (K && K.filterRow) K.filterRow($('grdHistoryConsumptionDetail'));
    }

    /** grdHistoryConsumptionMai_ColumnButtonClick:3228 ("Edit") and _DoubleClick:3260. */
    function historyEdit(i) {
        var r = H.rows[i];
        if (!r) return;
        if (str(r.IsApproved) === 'Approved') { box("You Can't Update Approved Record"); return; }
        if (str(r.PlanStatus) !== 'In Process') { box("You Can't Update Record with " + str(r.PlanStatus) + ' JobPlanStatus'); return; }
        S.recId = toInt(str(r.Id));
        readById(S.recId);
    }
    function historyButton(b, i) {
        var r = H.rows[i];
        if (!r) return;
        H.cur = i;
        if (b === 'Edit') historyEdit(i);
        if (b === 'WagesPrint') {
            /* ContractorWagesBill_SlipandRegister_002 - no wages bill is "No Record Found For Display". */
            var w = toInt(r.WagesId);
            if (w <= 0) { box('No Record Found For Display'); return; }
            global.CrystalPrint.open('wages-002', { id: w });
        }
        if (b === 'Print') {
            var id = toInt(r.Id);
            if (id === 0) { box('No Record Found For Display'); return; }
            global.CrystalPrint.open('prod-623', { id: id });
        }
    }

    /* ======================================================== LoadConsumptionPendingforRates */

    var PR = { rows: [], raw: [], cur: -1, outPutDocId: 0, resolve: null };

    function openPendingRates() {
        PR.outPutDocId = 0;
        PR.rows = []; PR.raw = []; PR.cur = -1;
        $('pGridHost').innerHTML = '';
        $('pDetailHost').innerHTML = '';
        $('dlgPendingRates').classList.add('open');
        /* LoadInvoices_Load:95 */
        setv('pFromDate', S.fyStart ? datePart(S.fyStart) : today());
        setv('pToDate', today());
        setv('pDocFrom', ''); setv('pDocTo', '');
        clearList('pJobOrder');
        $('pFromDate').focus();
        var jo = P.getJson(C + '/pending-rates/job-orders').then(function (rows) {
            if (rows && rows.length) fill('pJobOrder', rows, 'Id', 'PlanCode', false);
        }).catch(function (e) { box(e.message); });
        jo.then(pendingSearch);
        return new Promise(function (resolve) { PR.resolve = resolve; });
    }
    function closePendingRates() {
        $('dlgPendingRates').classList.remove('open');
        var r = PR.resolve; PR.resolve = null;
        if (r) r(PR.outPutDocId);
    }

    /** OutputGridHistory:189 - grouped by header Id; TotalWeight is an AVERAGE, as the desktop sums it. */
    function pendingSearch() {
        return P.busy('pSearch', function () {
            return P.getJson(C + '/pending-rates' + P.qs({
                from: val('pFromDate') ? val('pFromDate') + 'T' + S.pickTime : '',
                to: val('pToDate') ? val('pToDate') + 'T' + S.pickTime : '',
                jobOrderId: selVal('pJobOrder')
            })).then(function (raw) {
                if (!raw || !raw.length) return;
                PR.raw = raw;
                var order = [], by = {};
                raw.forEach(function (x) {
                    var k = String(ci(x, 'Id'));
                    if (!by[k]) { by[k] = []; order.push(k); }
                    by[k].push(x);
                });
                PR.rows = order.map(function (k) {
                    var g = by[k], f = g[0], q = 0, w = 0;
                    g.forEach(function (x) { q += num(ci(x, 'Qty')); w += num(ci(x, 'Weight')); });
                    return { Id: ci(f, 'Id'), JobOrderNo: ci(f, 'JobOrderNo'), DocDate: shortDate(ci(f, 'DocDate')),
                             DocCode: ci(f, 'DocCode'), MainRemarks: ci(f, 'MainRemarks'), TotalQty: q, TotalWeight: w / g.length,
                             EntryDate: shortDate(ci(f, 'EntryDate')), EntryUser: ci(f, 'EntryUser'),
                             ModifyDate: shortDate(ci(f, 'ModifyDate')), ModifyUser: ci(f, 'ModifyUser') };
                });
                renderPending();
            }).catch(function (e) { box(e.message); });
        });
    }
    function renderPending() {
        var cols = ['JobOrderNo', 'DocDate', 'DocCode', 'MainRemarks', 'TotalQty', 'TotalWeight', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser'];
        var h = '<table class="gx" id="pGrid"><thead><tr><th style="width:40px;">Load</th>';
        cols.forEach(function (c) { h += '<th>' + c + '</th>'; });
        h += '</tr></thead><tbody>';
        PR.rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === PR.cur ? ' class="cur"' : '') + '><td><button type="button" class="cellbtn" data-load="' + i + '">Load</button></td>';
            cols.forEach(function (c) { h += '<td>' + esc(typeof r[c] === 'number' ? netG(r[c]) : str(r[c])) + '</td>'; });
            h += '</tr>';
        });
        h += '</tbody></table>';
        $('pGridHost').innerHTML = h;
        if (K && K.filterRow) K.filterRow($('pGrid'));
    }
    /** grd_SelectionChanged:297. */
    function pendingSelect(i) {
        PR.cur = i;
        Array.prototype.forEach.call($('pGrid').tBodies[0].rows, function (tr) { tr.classList.toggle('cur', toInt(tr.getAttribute('data-i')) === i); });
        var r = PR.rows[i];
        if (!r || !PR.raw.length) return;
        var cols = ['EntryType', 'ItemName', 'Cropyear', 'WareHouseName', 'JobLotDescription', 'PackType', 'PackUom', 'Qty', 'Weight'];
        var h = '<table class="gx" id="pDetail"><thead><tr>';
        cols.forEach(function (c) { h += '<th>' + c + '</th>'; });
        h += '</tr></thead><tbody>';
        PR.raw.forEach(function (x) {
            if (toInt(r.Id) !== toInt(ci(x, 'Id'))) return;
            var v = { EntryType: ci(x, 'EntryType'), ItemName: ci(x, 'ItemName'), Cropyear: ci(x, 'CropBatch'),
                      WareHouseName: ci(x, 'WareHouseName'), JobLotDescription: ci(x, 'JobLotDescription'),
                      PackType: ci(x, 'PackTypeDesc'), PackUom: ci(x, 'PackUom'), Qty: netG(num(ci(x, 'Qty'))), Weight: netG(num(ci(x, 'Weight'))) };
            h += '<tr>';
            cols.forEach(function (c) { h += '<td>' + esc(str(v[c])) + '</td>'; });
            h += '</tr>';
        });
        h += '</tbody></table>';
        $('pDetailHost').innerHTML = h;
    }
    /** btnReset_Click:275 - searches, then clears. */
    function pendingReset() {
        return pendingSearch().then(function () {
            setv('pFromDate', S.fyStart ? datePart(S.fyStart) : today());
            $('pFromDate').focus();
            clearText('pJobOrder');
            PR.rows = []; PR.cur = -1;
            $('pGridHost').innerHTML = '';
            $('pDetailHost').innerHTML = '';
        });
    }

    /** BtnPendingForRatesFormConsumption_Click:2450. */
    function pendingForRates() {
        openPendingRates().then(function (id) {
            var p = id > 0 ? readById(id) : Promise.resolve();
            return p.then(updateUomForPendingRates);
        }).catch(function (e) { box(e.message); });
    }

    /* ============================================= LoadavailableTransactionsForIssuanceOnConsumption */

    var IS = { rows: [], checked: {}, issuance: null, resolve: null };

    function openIssuance() {
        IS.rows = []; IS.checked = {}; IS.issuance = null;
        $('iGridHost').innerHTML = '';
        setv('iSelWeight', ''); setv('iSelQty', '');
        $('dlgIssuance').classList.add('open');
        var done = new Promise(function (resolve) { IS.resolve = resolve; });
        /* LoadInvoices_Load:520 */
        P.getJson(C + '/issuance-loader/init').then(function (d) {
            if (d.loadError) { box(d.loadError); return; }
            setv('iFromDate', S.fyStart ? datePart(S.fyStart) : today());
            setv('iToDate', today());
            IS.issuance = [];                      /* dtIssuance with its columns */
            bindIssuanceCombos(d.combos || []);
            return issuanceSearch().then(function () { $('iFromDate').focus(); });
        }).catch(function (e) { box(e.message); });
        return done;
    }
    function closeIssuance() {
        $('dlgIssuance').classList.remove('open');
        var r = IS.resolve; IS.resolve = null;
        if (r) r(IS.issuance);
    }

    /** ConsumptionDetailComboBind:121 of the loader. */
    function bindIssuanceCombos(all) {
        if (!all.length) return;
        var g = { ParentCategories: [], ItemCategories: [], ItemTypes: [], JobLot: [], CropYear: [], Warehouse: [],
                  RefDocumentType: [], Supplier: [], Items: [] };
        all.forEach(function (r) {
            var t = str(ci(r, 'ActivityType'));
            if (g[t]) g[t].push({ Id: ci(r, 'Id'), name: ci(r, 'name') });
        });
        rebind('iParent', g.ParentCategories);
        rebind('iCategory', g.ItemCategories);
        rebind('iType', g.ItemTypes);
        rebind('iJobLot', g.JobLot);
        rebind('iCropYear', g.CropYear);
        rebind('iWarehouse', g.Warehouse);
        rebind('iRefDoc', g.RefDocumentType);
        rebind('iSupplier', g.Supplier);
        rebind('iItem', g.Items);
    }

    var IGCOLS = ['RefDocumentTypeId', 'RefDocIdNo', 'RefDocSubIdNo', 'RefDocumentType', 'DocDate', 'DocCodeNo', 'ManualNo', 'GrnNo',
        'SupplierCustomerName', 'VehicleNo', 'GpNo', 'WarehouseId', 'WareHouseCode', 'ItemId', 'ItemName', 'ItemCode', 'CropYearId',
        'CropBatch', 'JobLotId', 'JobLotCode', 'InvPackingTypeId', 'PackingType', 'ItemUom', 'PackUom', 'PackSize', 'QtyIn', 'QtyOut',
        'QtyBalance', 'WeightIn', 'WeightOut', 'WeightBalance', 'ReserveWeight', 'AVgRate', 'RateUom', 'Equivalent', 'RateUomId', 'ItemAmount'];
    var IG_HIDDEN = { RefDocumentTypeId: 1, CropYearId: 1, RefDocIdNo: 1, RefDocSubIdNo: 1, ItemId: 1, ItemUom: 1, JobLotId: 1,
        InvPackingTypeId: 1, WarehouseId: 1, RateUomId: 1, ReserveWeight: 1, PackSize: 1, Equivalent: 1 };
    var IG_HASH = { AVgRate: 1, QtyIn: 1, QtyOut: 1, QtyBalance: 1, WeightIn: 1, WeightOut: 1, WeightBalance: 1, ItemAmount: 1 };
    var IG_SUM = { QtyIn: 1, QtyOut: 1, QtyBalance: 1, ItemAmount: 1, WeightIn: 1, WeightOut: 1, WeightBalance: 1 };

    /** PendingInventoryTransactionsForIssuanceLoad:289. */
    function issuanceSearch() {
        return P.busy('iSearch', function () {
            return P.getJson(C + '/issuance-loader/search' + P.qs({
                from: val('iFromDate') ? val('iFromDate') + 'T' + S.pickTime : '',
                to: val('iToDate') ? val('iToDate') + 'T' + S.pickTime : '',
                parentCategoryId: selVal('iParent'), itemCategoryId: selVal('iCategory'), itemTypeId: selVal('iType'),
                jobLotId: selVal('iJobLot'), cropYear: selVal('iCropYear') > 0 ? selText('iCropYear') : '',
                warehouseId: selVal('iWarehouse'), refDocumentTypeId: selVal('iRefDoc'),
                supplierCustomerId: selVal('iSupplier'), itemId: selVal('iItem')
            })).then(function (rows) {
                IS.rows = rows || [];
                IS.checked = {};
                if (!IS.rows.length) { $('iGridHost').innerHTML = ''; return; }
                renderIssuance();
            }).catch(function (e) { box(e.message); });
        });
    }
    function renderIssuance() {
        var cols = IGCOLS.filter(function (c) {
            if (IG_HIDDEN[c]) return false;
            if (!S.rights.rateAndAmount && (c === 'ItemAmount' || c === 'AVgRate')) return false;
            return true;
        });
        var h = '<table class="gx" id="iGrid"><thead><tr><th style="width:40px;"><input type="checkbox" id="iAll" title="Select all"></th>';
        cols.forEach(function (c) { h += '<th' + (IG_HASH[c] ? ' class="num"' : '') + (c === 'ItemName' ? ' style="min-width:250px;"' : '') + '>' + c + '</th>'; });
        h += '</tr></thead><tbody>';
        IS.rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"><td><input type="checkbox" data-chk="' + i + '"' + (IS.checked[i] ? ' checked' : '') + '></td>';
            cols.forEach(function (c) {
                var v = ci(r, c);
                var t = IG_HASH[c] ? hashFmt(v) : (c === 'DocDate' ? (v ? K.dMMMyyyy(v) : '') : (typeof v === 'number' ? netG(v) : str(v)));
                h += '<td' + (IG_HASH[c] ? ' class="num"' : '') + '>' + esc(t) + '</td>';
            });
            h += '</tr>';
        });
        h += '</tbody><tfoot><tr><td></td>';
        cols.forEach(function (c) {
            if (IG_SUM[c]) {
                var t = 0;
                IS.rows.forEach(function (r) { t += num(ci(r, c)); });
                h += '<td class="num">' + esc(hashFmt(t)) + '</td>';
            } else h += '<td></td>';
        });
        h += '</tr></tfoot></table>';
        $('iGridHost').innerHTML = h;
        if (K && K.filterRow) K.filterRow($('iGrid'));
    }
    /** SelectedWeightCalculation:623 - Math.Round then "0,0". */
    function selectedWeight() {
        var w = 0, q = 0;
        Object.keys(IS.checked).forEach(function (i) {
            if (!IS.checked[i]) return;
            w += num(ci(IS.rows[i], 'WeightBalance'));
            q += num(ci(IS.rows[i], 'QtyBalance'));
        });
        setv('iSelWeight', K.pad2(roundEven(w, 0)));
        setv('iSelQty', K.pad2(roundEven(q, 0)));
    }
    /** btnLoadOnInvoice_Click_1:496. */
    function issuanceLoad() {
        var idx = Object.keys(IS.checked).filter(function (i) { return IS.checked[i]; }).map(Number).sort(function (a, b) { return a - b; });
        if (IS.issuance === null) {
            box('Input array is longer than the number of columns in this table.');
            return;
        }
        if (!idx.length) { box('Please Select Row first'); return; }
        idx.forEach(function (i) {
            var r = IS.rows[i];
            IS.issuance.push({
                RefDocumentTypeId: str(ci(r, 'RefDocumentTypeId')), RefDocIdNo: str(ci(r, 'RefDocIdNo')), RefDocSubIdNo: str(ci(r, 'RefDocSubIdNo')),
                DocDate: str(ci(r, 'DocDate')), DocNo: str(ci(r, 'DocCodeNo')), WareHouseId: toInt(ci(r, 'WarehouseId')),
                WareHouse: str(ci(r, 'WareHouseCode')), ItemId: toInt(ci(r, 'ItemId')), ItemName: str(ci(r, 'ItemName')),
                ItemCode: str(ci(r, 'ItemCode')), ItemUOMId: toInt(ci(r, 'ItemUom')), PackUom: ci(r, 'PackUom'),
                ItemUOM: num(ci(r, 'PackSize')), CropYearId: toInt(ci(r, 'CropYearId')), CropYear: str(ci(r, 'CropBatch')),
                JobLotId: toInt(ci(r, 'JobLotId')), JobLotCode: str(ci(r, 'JobLotCode')), PackingTypeId: toInt(ci(r, 'InvPackingTypeId')),
                PackTypeCode: str(ci(r, 'PackingType')), BalQty: num(ci(r, 'QtyBalance')), BalWeight: num(ci(r, 'WeightBalance')),
                ItemQty: num(ci(r, 'QtyBalance')), Weight: num(ci(r, 'WeightBalance')), ItemRate: num(ci(r, 'AVgRate')),
                RateUOMId: toInt(ci(r, 'RateUomId')), RateUOM: ci(r, 'RateUom'), Equivalent: num(ci(r, 'Equivalent')),
                ItemAmount: num(ci(r, 'ItemAmount')), Remarks: ''
            });
        });
        closeIssuance();
    }
    /** btnReset_Click:470. */
    function issuanceReset() {
        focus('iParent');
        clearText('iSupplier');
        clearText('iItem');
        $('iGridHost').innerHTML = '';
        IS.rows = []; IS.checked = {};
    }

    /** BtnLoadDataForConsumptionEntry_Click:2469 -> LoadDataDetailforConsumption:2484. */
    function loadDataForConsumption() {
        openIssuance().then(function (dt) {
            if (!dt || !dt.length) return;
            var vis = visibleRows();
            dt.forEach(function (x) {
                var found = false;
                vis.forEach(function (i) {
                    var g = S.rows[i];
                    if (toInt(x.RefDocumentTypeId) === toInt(g.RefDocumentTypeId) && toInt(x.RefDocIdNo) === toInt(g.RefDocIdNo)
                        && toInt(x.RefDocSubIdNo) === toInt(g.RefDocSubIdNo)) {
                        g.BalanceWeight = num(g.BalanceWeight) + num(x.Weight);
                        g.BalanceQty = num(g.BalanceQty) + num(x.ItemQty);
                        found = true;
                    }
                });
                if (!found) {
                    S.rows.push(newRow([0, x.RefDocumentTypeId, x.RefDocIdNo, x.RefDocSubIdNo, dmy(x.DocDate, true), x.DocNo,
                        x.WareHouseId, x.WareHouse, x.ItemId, x.ItemName, x.ItemCode, x.ItemUOMId, x.PackUom, x.ItemUOM, x.CropYear,
                        x.JobLotId, x.JobLotCode, x.PackingTypeId, x.PackTypeCode, x.ItemQty, x.Weight, x.ItemQty, x.Weight,
                        x.ItemRate, x.ItemRate, x.RateUOMId, x.RateUOM, x.Equivalent, x.ItemAmount, '']));
                }
            });
            S.bound = true;
            renderGrid();
        }).catch(function (e) { box(e.message); });
    }

    /* ============================================================================== wiring */

    function on(id, ev, fn) { var e = $(id); if (e) e.addEventListener(ev, fn); }

    doc.addEventListener('DOMContentLoaded', function () {
        on('BtnNewConsumption', 'click', function () { resetMain(); });
        on('BtnRefreshConsumption', 'click', function () { P.busy('BtnRefreshConsumption', refreshMain); });
        on('btnSaveConsumption', 'click', function () { S.recId = 0; insertConsumption('btnSaveConsumption'); });
        on('BtnUpdateconsumption', 'click', function () { insertConsumption('BtnUpdateconsumption'); });
        on('BtnPrintSlipConsumption', 'click', printSlip);
        on('btnPrintVoucherConsumption', 'click', printVoucher);
        on('BtnGenerateRates', 'click', function () { P.busy('BtnGenerateRates', generateRates); });
        on('BtnPendingForRatesFormConsumption', 'click', pendingForRates);
        on('BtnLoadDataForConsumptionEntry', 'click', loadDataForConsumption);

        on('cmbJobOrderConsumption', 'change', jobOrderLeave);
        on('cmbWarehouseConsumption', 'change', function () { getAvailableStock(); getAvgRate(); });
        on('CmbItemNameConsumption', 'change', function () { itemLeave(); });
        on('CmbCropYearConsumption', 'change', function () { getAvailableStock(); getAvgRate(); });
        on('CmbJobLotConsumption', 'change', function () { getAvailableStock(); getAvgRate(); });
        on('CmbPackingTypeConsumption', 'change', function () { if (S.fifo) getAvgRate(); });
        on('cmbPackUomConsumption', 'change', function () { packUomChanged(false); });
        on('txtItemQtyConsumption', 'input', calculateTotalWeight);
        on('txtItemQtyConsumption', 'blur', function () { calculateTotalWeight(); if (S.fifo) getAvgRate(); });
        on('txtNetWeightConsumption', 'input', amountCalculation);
        on('txtNetWeightConsumption', 'blur', function () { if (S.fifo) getAvgRate(); });
        on('txtRateConsumption', 'input', amountCalculation);
        on('CmbRateUomConsumption', 'change', amountCalculation);
        on('rdSearchByName', 'change', searchModeChanged);
        on('rdSearchByCode', 'change', searchModeChanged);
        on('BtnAddConsumption', 'click', addDetail);
        on('BtnUpdateDetailConsumption', 'click', updateDetail);
        on('BtnCancelConsumption', 'click', cancelDetail);

        var host = $('grdConsumptionHost');
        host.addEventListener('click', function (e) {
            var d = e.target.getAttribute('data-del');
            if (d !== null) { setCur(toInt(d)); deleteRow(toInt(d)); return; }
            var tr = e.target.closest('tbody tr');
            if (tr) setCur(toInt(tr.getAttribute('data-i')));
        });
        host.addEventListener('dblclick', function (e) {
            if (e.target.getAttribute('data-del') !== null) return;
            var tr = e.target.closest('tbody tr');
            if (!tr) return;
            setCur(toInt(tr.getAttribute('data-i')));
            editRow();
        });
        host.addEventListener('change', function (e) {
            var c = e.target.getAttribute('data-edit');
            if (c) cellUpdated(toInt(e.target.getAttribute('data-i')), c, e.target.value);
        });
        host.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target.getAttribute('data-edit')) { e.preventDefault(); e.target.blur(); }
        });

        /* tabControl6 */
        on('tabPage9Btn', 'click', function () { showSub('form'); });
        on('tabPage10Btn', 'click', function () { showSub('history'); });
        on('BtnShowConsupmtionHistory', 'click', showHistory);
        on('btnRefreshConsupmtionHistory', 'click', function () { P.busy('btnRefreshConsupmtionHistory', jobOrderConsumptionHCombobind); });
        on('BtnNewHistoryConsumption', 'click', resetHistory);
        var hh = $('grdHistoryConsumptionMaiHost');
        hh.addEventListener('click', function (e) {
            var b = e.target.getAttribute('data-hb');
            if (b) { historyButton(b, toInt(e.target.getAttribute('data-i'))); return; }
            var o = e.target.getAttribute('data-open');
            if (o !== null) { historySelect(toInt(o)); historyEdit(toInt(o)); return; }
            var tr = e.target.closest('tbody tr');
            if (tr) historySelect(toInt(tr.getAttribute('data-i')));
        });
        hh.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr');
            if (tr && !e.target.getAttribute('data-hb')) historyEdit(toInt(tr.getAttribute('data-i')));
        });

        /* ctrlGrdBar16 / ctrlGrdBar17: the shared GridBar (countx_grid_bar.js, data-gridbar on the hosts). */

        /* Pending For Rates dialog */
        var pd = $('dlgPendingRates');
        pd.querySelector('[data-close]').addEventListener('click', closePendingRates);
        on('pSearch', 'click', pendingSearch);
        on('pReset', 'click', pendingReset);
        $('pGridHost').addEventListener('click', function (e) {
            var l = e.target.getAttribute('data-load');
            if (l !== null) {
                PR.outPutDocId = toInt(ci(PR.rows[toInt(l)], 'Id'));
                closePendingRates();
                return;
            }
            var tr = e.target.closest('tbody tr');
            if (tr) pendingSelect(toInt(tr.getAttribute('data-i')));
        });

        /* Issuance loader dialog */
        var dl = $('dlgIssuance');
        dl.querySelector('[data-close]').addEventListener('click', closeIssuance);
        on('iSearch', 'click', issuanceSearch);
        on('iLoad', 'click', issuanceLoad);
        on('iReset', 'click', issuanceReset);
        on('iRefresh', 'click', function () {
            P.busy('iRefresh', function () {
                return P.getJson(C + '/issuance-loader/combos').then(bindIssuanceCombos).catch(function (e) { box(e.message); });
            });
        });
        $('iGridHost').addEventListener('change', function (e) {
            if (e.target.id === 'iAll') {
                var on_ = e.target.checked;
                Array.prototype.forEach.call($('iGrid').tBodies[0].rows, function (tr) {
                    if (tr.style.display === 'none') return;
                    var i = toInt(tr.getAttribute('data-i'));
                    IS.checked[i] = on_;
                    var cb = tr.querySelector('input[data-chk]');
                    if (cb) cb.checked = on_;
                });
                selectedWeight();
                return;
            }
            var c = e.target.getAttribute('data-chk');
            if (c !== null) { IS.checked[toInt(c)] = e.target.checked; selectedWeight(); }
        });
        /* LoadavailableTransactionsForIssuance_KeyDown:584 */
        dl.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && !e.ctrlKey && e.target.tagName !== 'BUTTON' && e.target.tagName !== 'TEXTAREA') {
                e.preventDefault();
                var all = Array.prototype.filter.call(dl.querySelectorAll('input, select, button'), function (x) {
                    return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null;
                });
                var i = all.indexOf(e.target);
                if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
            }
            if ((e.ctrlKey && (e.key === 'e' || e.key === 'E')) || e.key === 'Escape') { e.preventDefault(); closeIssuance(); }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); $('iFromDate').focus(); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $('iGridHost').focus(); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('iParent'); }
            if (e.ctrlKey && (e.key === 'l' || e.key === 'L')) { e.preventDefault(); issuanceLoad(); }
            if (e.ctrlKey && (e.key === 's' || e.key === 'S')) { e.preventDefault(); issuanceSearch(); }
        });
        pd.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') { e.preventDefault(); closePendingRates(); }
        });
    });

    /** CmbItemNameConsumption_Leave:1538. */
    function itemLeave() {
        bindUomForItems();
        return Promise.all([getAvailableStock(), getAvgRate()]);
    }

    /* frmFoodProduction_KeyDown:785-815 - only while tab index 2 is selected. */
    P.onKey(function (e, index) {
        if (index !== 2) return;
        var k = e.key.length === 1 ? e.key.toLowerCase() : e.key;
        if (!e.ctrlKey) return;
        if (k === 's' && isSaveVisible() && !$('btnSaveConsumption').disabled) { e.preventDefault(); S.recId = 0; insertConsumption('btnSaveConsumption'); }
        else if (k === 'u' && isUpdateVisible() && !$('BtnUpdateconsumption').disabled) { e.preventDefault(); insertConsumption('BtnUpdateconsumption'); }
        else if (k === 'F5') { e.preventDefault(); $('txtDocDateConsumption').focus(); }
        else if (k === 'ArrowDown') { e.preventDefault(); $('grdConsumptionHost').focus(); }
        else if (k === 'Enter') { e.preventDefault(); editRow(); }
        else if (k === 'n') { e.preventDefault(); resetMain(); }
        else if (k === 'r') { e.preventDefault(); refreshMain(); }
    });

    /* tabControl1_SelectedIndexChanged:899 - index 2. */
    P.onSelect(function (index) {
        if (index !== 2) return;
        if (isSaveVisible() && !$('btnSaveConsumption').disabled) generateDoc();
        consumptionDetailComboBind();
        $('txtDocDateConsumption').focus();
    });

    P.ready.then(onLoad).catch(function () { /* the shell has shown the error */ });

    global.P280Consumption = { readById: readById };
}(window));
