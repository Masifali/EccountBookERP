/* ============================================================================================
 * Screen 280 "Production Against (Job Order)" - Settlement tab.
 * Desktop: Architecture.WinApp.Production/frmProductionSettlement.cs (3,962 lines).
 *
 * Line numbers below are frmProductionSettlement.cs's. The grid arithmetic runs here, as it runs
 * in the desktop's grids; every database step (and every refusal it guards) runs on the server.
 *
 * OpenInProductionTab: the shell constructs the form with OpenInProductionTab = true and handles
 * its OverHeadReadById / PackingMaterialReadById events. Here "inside the 280 frame" is that flag:
 * the two events become window.parent.P280OpenTab('Overhead'|'PackingMaterial', id). Standalone,
 * the desktop's other branch runs: the Overhead / Packing Material page opens on its own and
 * loads the record through its window.P280ReadById.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/production-against-job-order/settlement';
    var BASE = '/production/production-against-job-order/';
    var K = window.ReportKit;

    /* ---------------------------------------------------------------- form state (fields) */
    var cfg = { jobOrderCreatewithoutRates: false, saleCostingJobOrderWise: false, fifoCgs: false };
    var OpenInProductionTab = window.parent && window.parent !== window;
    var inputRows = null;       /* grdInputsettlement.DataSource (null = no DataSource) */
    var outputRows = null;      /* grdOutPutSettlement.DataSource */
    var ohRows = null;          /* grdOverHeadSettlement.DataSource */
    var pmRows = null;          /* grdPackingMaterialSettlement.DataSource */
    var AvgRate = 0, AvgRateWithoutExp = 0;
    var settlementDate = nowIso();          /* txtSettlementDate.Value, time of day kept */
    var outFilter = {};                     /* grdOutPutSettlement filter row (FilterMode automatic) */
    var outCurrent = -1;
    var uomCache = {};

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function say(m) { $id('lblStatus').textContent = m || ''; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function nowIso() {
        var d = new Date();
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) + 'T'
            + pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds());
    }

    /* ------------------------------------------------------ Architecture.Common.Conversion */

    /** Conversion.ToDouble: null/""->0, thousands allowed, Infinity->0, NaN passes through. */
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) || isNaN(v) ? v : 0;
        var s = String(v).trim().replace(/,/g, '');
        if (s === '') return 0;
        if (s === 'NaN') return NaN;
        var n = Number(s);
        if (isNaN(n)) return 0;
        return isFinite(n) ? n : 0;
    }
    /** Conversion.ToInt: Convert.ToInt32 - banker's rounding for numbers, strict text, else 0. */
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        if (typeof v === 'number') {
            if (!isFinite(v)) return 0;
            var f = Math.floor(v), d = v - f;
            if (d > 0.5 || (d === 0.5 && f % 2 !== 0)) f += 1;
            return f;
        }
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function toS(v) { return v === null || v === undefined ? '' : String(v); }
    /** double.ToString() (.NET Framework, 15 significant digits) and its round trip. */
    function str15(x) {
        if (typeof x !== 'number') return toS(x);
        if (isNaN(x)) return 'NaN';
        if (!isFinite(x)) return x > 0 ? 'Infinity' : '-Infinity';
        return String(Number(x.toPrecision(15)));
    }
    function r15(x) { return typeof x === 'number' && isFinite(x) ? Number(x.toPrecision(15)) : toD(x); }

    /* ------------------------------------------------------------- .NET number formats */
    function roundAway(x, d) {
        var p = Math.pow(10, d || 0);
        var a = Math.abs(Number(x.toPrecision(15)));
        var r = Math.round(a * p + 1e-9) / p;
        return x < 0 ? -r : r;
    }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmt(v, f) {
        if (v === null || v === undefined || v === '') return '';
        var x = typeof v === 'number' ? v : toD(v);
        if (isNaN(x)) return 'NaN';
        if (!isFinite(x)) return x > 0 ? 'Infinity' : '-Infinity';
        var neg = x < 0, r, p;
        switch (f) {
            case '#,#':
                r = roundAway(x, 0);
                return r === 0 ? '' : (r < 0 ? '-' : '') + group(String(Math.abs(r)));
            case '#,#.##': case '#,#.###': case '#,##0.###':
                var dec = f === '#,#.##' ? 2 : 3;
                r = roundAway(x, dec);
                if (r === 0) return f === '#,##0.###' ? '0' : '';
                p = Math.abs(r).toFixed(dec).replace(/0+$/, '').replace(/\.$/, '').split('.');
                var ip = p[0] === '0' ? (f === '#,##0.###' ? '0' : '') : group(p[0]);
                return (r < 0 ? '-' : '') + ip + (p[1] ? '.' + p[1] : '');
            case 'N3':
                r = roundAway(x, 3);
                p = Math.abs(r).toFixed(3).split('.');
                return (r < 0 ? '-' : '') + group(p[0]) + '.' + p[1];
            case '0':
                return String(roundAway(x, 0));
            case '0,0':
                return K.pad2(roundAway(x, 0));
            default:
                return str15(x);
        }
    }
    /** DateTime columns (no FormatString on the desktop) are shown dd-MMM-yy. */
    function dshow(v) { return v ? K.dMMMyy(v) : ''; }
    function dMMMyyText(v) {
        /* Conversion.ToDateTime(row["DocDate"]).ToString("dd-MMM-yy") - an untyped column keeps it as text. */
        return v ? K.dMMMyy(v) : '01-Jan-00';
    }

    /* ------------------------------------------------------------------ MessageBox */
    var ICON = { error: 'fa-times-circle" style="color:#c00', question: 'fa-question-circle" style="color:#1565c0',
                 info: 'fa-info-circle" style="color:#1565c0' };
    function dialog(text, caption, icon, buttons) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'st-modal';
            m.innerHTML = '<div class="st-dlg" role="dialog"><div class="cap"><span>' + esc(caption || '') + '</span></div>'
                + '<div class="msg">' + (icon ? '<i class="fa ' + ICON[icon] + '"></i>' : '') + '<div>' + esc(text) + '</div></div>'
                + '<div class="btns">' + buttons.map(function (b, i) {
                    return '<button type="button" data-i="' + i + '">' + esc(b) + '</button>';
                }).join('') + '</div></div>';
            document.body.appendChild(m);
            var first = m.querySelector('button');
            if (first) first.focus();
            function done(i) { document.removeEventListener('keydown', key, true); m.remove(); resolve(i); }
            function key(e) {
                if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); done(buttons.length === 1 ? 0 : 1); }
            }
            document.addEventListener('keydown', key, true);
            m.addEventListener('click', function (e) {
                var b = e.target.closest('button[data-i]');
                if (b) done(parseInt(b.getAttribute('data-i'), 10));
            });
        });
    }
    function msg(text, caption, icon) { return dialog(text, caption, icon, ['OK']); }
    function ask(text, caption) {
        return dialog(text, caption, 'question', ['Yes', 'No']).then(function (i) { return i === 0; });
    }

    /* ------------------------------------------------------------------ requests */
    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn) h[hn.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function handle(r) {
        return r.text().then(function (t) {
            var body = null;
            try { body = t ? JSON.parse(t) : null; } catch (e) { body = null; }
            if (!r.ok) throw new Error(body && body.message ? body.message : (t || ('Request failed (' + r.status + ')')));
            return body;
        });
    }
    function getJson(url) {
        return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(handle);
    }
    function postJson(url, body) {
        return fetch(url, { method: 'POST', credentials: 'same-origin',
            headers: headers({ 'Content-Type': 'application/json', 'Accept': 'application/json' }),
            body: JSON.stringify(body) }).then(handle);
    }
    /** Button contract: disabled + spinner while running, duplicates ignored, restored on both outcomes. */
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $id(btn) : btn;
        if (b && b.classList.contains('is-busy')) return Promise.resolve();
        if (b) { b.classList.add('is-busy'); b.disabled = true; }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = false; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* ------------------------------------------------------------------ combos */
    if (window.DesktopCombo) {
        /* DropDownBind.BindDDL: column 0 (Id) hidden, column 1 captioned "JobOrderNo" (350 wide),
           every other column GetJobOrderNoAll returns stays visible. */
        window.DesktopCombo.define('p280SettleJobOrder', [
            { caption: 'JobOrderNo',     flex: 3 },
            { caption: 'ApprovalStatus', flex: 2, key: 'approval' },
            { caption: 'SettledStatus',  flex: 2, key: 'settled' },
            { caption: 'SettlementDate', flex: 3, key: 'sdate' },
            { caption: 'JobDocNo',       flex: 1, key: 'docno', type: 'num' }
        ]);
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function fillJobOrders(rows, keep) {
        var sel = $id('CmbJobOrderSettlement');
        var prev = keep ? sel.value : '';
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var sd = toS(col(r, 'SettlementDate'));
            h += '<option value="' + esc(col(r, 'Id')) + '" data-approval="' + esc(col(r, 'ApprovalStatus'))
                + '" data-settled="' + esc(col(r, 'SettledStatus')) + '" data-sdate="' + esc(sd ? K.dmyhm(sd) : '')
                + '" data-sdate-raw="' + esc(sd) + '" data-docno="' + esc(col(r, 'JobDocNo')) + '">'
                + esc(col(r, 'PlanCode')) + '</option>';
        });
        sel.innerHTML = h;
        if (prev && sel.querySelector('option[value="' + CSS.escape(prev) + '"]')) sel.value = prev;
        else sel.value = '';
    }
    function jobOrderId() { return toI($id('CmbJobOrderSettlement').value); }
    function jobOrderText() {
        var sel = $id('CmbJobOrderSettlement');
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' ? o.textContent : '';
    }
    function jobOrderActiveRow() {
        var sel = $id('CmbJobOrderSettlement');
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' ? o : null;
    }
    function diffAccountId() { return toI($id('CmbDifferenceAccountTitle').value); }

    /** JobOrderNoFillForSettlement(ActionId):317 - bound only when rows came back. */
    function jobOrderNoFill(actionId) {
        return getJson(API + '/job-orders?all=' + (actionId === 0)).then(function (rows) {
            if (rows && rows.length > 0) fillJobOrders(rows, true);
        }).catch(function (e) { return msg(e.message); });
    }

    /** CmbJobOrderSettlement_Leave:2010. */
    function jobOrderLeave() {
        var o = jobOrderActiveRow();
        var d = nowIso();
        if (jobOrderText() !== '' && o && toI(o.value) !== 0) {
            var raw = o.getAttribute('data-sdate-raw');
            d = raw ? (raw.length === 10 ? raw + 'T00:00:00' : raw.substring(0, 19)) : '1900-01-01T00:00:00';
        }
        settlementDate = d;
        $id('txtSettlementDate').value = d.substring(0, 10);
    }

    /* ------------------------------------------------------------------ grids */
    var INPUT_COLS = [
        { k: 'EntryType', w: 55 }, { k: 'DocDate', w: 73, date: true }, { k: 'DocNo', w: 60 },
        { k: 'WareHouse', w: 120 }, { k: 'ItemName', w: 150 }, { k: 'ItemUOM', w: 50 }, { k: 'CropYear', w: 60 },
        { k: 'JobLot', w: 80 }, { k: 'PackingType', w: 70 },
        { k: 'Quantity', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Weight', w: 80, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Rate', w: 70, f: '#,#.##' }, { k: 'RateUOM', w: 50 },
        { k: 'ItemAmount', w: 90, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Remarks', w: 100 }, { k: 'WipAccount', cap: 'Wip/Lot Account', w: 100 },
        { k: 'RefJobOrder', w: 70, sc: true }, { k: 'RefJobStatus', w: 70, sc: true },
        { k: 'InvoiceNo', w: 70, sc: true }, { k: 'InvoiceApprovedStatus', w: 70, sc: true }
    ];
    var OUTPUT_COLS = [
        { k: 'EntryType', w: 80 }, { k: 'DocDate', w: 73 }, { k: 'DocNo', w: 60 }, { k: 'WareHouse', w: 120 },
        { k: 'ItemName', w: 150 }, { k: 'ItemUOM', w: 50 }, { k: 'CropYear', w: 60 }, { k: 'JobLot', w: 80 },
        { k: 'PackingType', w: 70 },
        { k: 'Quantity', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Weight', w: 80, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Rate', cap: 'Job Order Rate', w: 85, f: '#,#.###', right: true, rateEdit: true },
        { k: 'ItemAmount', w: 90, f: '#,##0.###', agg: 'sum', tf: '#,##0.###', right: true },
        { k: 'RateWithoutExp', w: 90, f: '#,#', right: true },
        { k: 'AmountWithoutExp', w: 90, f: '#,##0.###', agg: 'sum', tf: '#,##0.###', right: true },
        { k: 'NetRate', w: 85, f: '#,#.###', right: true },
        { k: 'RateUOM', w: 50 },
        { k: 'ItemPmCost', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'GeneralPmCost', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'ItemOhCost', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'GeneralOhCost', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'TotalAmount', w: 90, f: '#,##0.###', agg: 'sum', tf: '#,##0.###', right: true },
        { k: 'Remarks', w: 100 }, { k: 'Status', w: 100 }
    ];
    var OH_COLS = [
        { k: 'TransRef', w: 90 }, { k: 'DocDate', w: 80, date: true }, { k: 'DocNo', w: 65, link: 'oh' },
        { k: 'AccountTitle', w: 120 },
        { k: 'ItemQty', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'ItemRate', w: 70, f: '#,#.##', agg: 'avg', tf: '#,#.##', right: true },
        { k: 'Amount', w: 90, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'BrandName', w: 100 }, { k: 'BrandUom', w: 80, dbl: true }
    ];
    var PM_COLS = [
        { k: 'DocDate', w: 80, date: true }, { k: 'DocNo', w: 60, link: 'pm' }, { k: 'ItemName', w: 125 },
        { k: 'Qty', w: 70, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Rate', w: 70, f: '#,#.##', agg: 'avg', tf: '#,#.##', right: true },
        { k: 'Amount', w: 90, f: '#,#', agg: 'sum', tf: '#,#', right: true },
        { k: 'Remarks', w: 130 }, { k: 'BrandName', w: 120 }, { k: 'BrandUom', w: 80, dbl: true }
    ];

    function cellText(c, v) {
        if (c.date) return dshow(v);
        if (c.f) return fmt(v, c.f);
        if (c.dbl) return v === null || v === undefined ? '' : str15(toD(v));
        return toS(v);
    }
    function colsOf(cols) {
        return cols.filter(function (c) { return !c.sc || cfg.saleCostingJobOrderWise; });
    }
    function totalOf(c, rows) {
        if (!c.agg || !rows.length) return c.agg ? fmt(0, c.tf) : '';
        var s = 0;
        rows.forEach(function (r) { s += toD(r[c.k]); });
        if (c.agg === 'avg') s = s / rows.length;
        return fmt(s, c.tf);
    }
    function head(cols, filter) {
        var h = '<colgroup>' + cols.map(function (c) { return '<col style="width:' + c.w + 'px">'; }).join('') + '</colgroup>';
        h += '<thead><tr>' + cols.map(function (c) { return '<th>' + esc(c.cap || c.k) + '</th>'; }).join('') + '</tr>';
        if (filter) {
            h += '<tr class="flt">' + cols.map(function (c) {
                return '<th><input type="text" data-k="' + c.k + '" value="' + esc(outFilter[c.k] || '') + '"></th>';
            }).join('') + '</tr>';
        }
        return h + '</thead>';
    }
    function tableWidth(cols) { return cols.reduce(function (s, c) { return s + c.w; }, 0); }

    /** A grid with no DataSource is blank; one with an empty DataSource still shows its header. */
    function renderSimple(id, cols, rows) {
        var t = $id(id);
        if (!rows) { t.innerHTML = ''; return; }
        var vc = colsOf(cols);
        t.style.width = tableWidth(vc) + 'px';
        var h = head(vc, false) + '<tbody>';
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">' + vc.map(function (c) {
                var txt = esc(cellText(c, r[c.k]));
                if (c.link) txt = '<a class="lnk" data-link="' + c.link + '" data-i="' + i + '">' + txt + '</a>';
                return '<td class="' + (c.right ? 'num' : '') + '">' + txt + '</td>';
            }).join('') + '</tr>';
        });
        h += '</tbody><tfoot><tr>' + vc.map(function (c) {
            return '<td class="' + (c.right ? 'num' : '') + '">' + esc(totalOf(c, rows)) + '</td>';
        }).join('') + '</tr></tfoot>';
        t.innerHTML = h;
    }

    /** grdOutPutSettlement.GetRows() - the rows the filter row lets through. */
    function outputVisible() {
        if (!outputRows) return [];
        var keys = Object.keys(outFilter).filter(function (k) { return outFilter[k]; });
        if (!keys.length) return outputRows.slice();
        return outputRows.filter(function (r) {
            return keys.every(function (k) {
                var c = OUTPUT_COLS.filter(function (x) { return x.k === k; })[0];
                return cellText(c, r[k]).toLowerCase().indexOf(outFilter[k].toLowerCase()) >= 0;
            });
        });
    }
    function rateEditable(r) {
        /* grdOutPutSettelmentSetting:796 - Rate is a TextBox only when SaleCostingJobOrderWise && !FIFO;
           grdOutPutSettlement_Click / _KeyDown then allow editing only while a ByProduct row is current. */
        return cfg.saleCostingJobOrderWise && !cfg.fifoCgs && toS(r.EntryType) === 'ByProduct';
    }
    function renderOutput() {
        var t = $id('grdOutPutSettlement');
        if (!outputRows) { t.innerHTML = ''; $id('navOutput').textContent = ''; return; }
        var vc = OUTPUT_COLS;
        var vis = outputVisible();
        t.style.width = tableWidth(vc) + 'px';
        var h = head(vc, true) + '<tbody>';
        vis.forEach(function (r, i) {
            var idx = outputRows.indexOf(r);
            h += '<tr data-row="' + idx + '" class="' + (idx === outCurrent ? 'cur' : '') + '">' + vc.map(function (c) {
                if (c.rateEdit && rateEditable(r)) {
                    return '<td class="num"><input class="edit" data-edit="' + idx + '" value="' + esc(str15(toD(r.Rate))) + '"></td>';
                }
                return '<td class="' + (c.right ? 'num' : '') + '">' + esc(cellText(c, r[c.k])) + '</td>';
            }).join('') + '</tr>';
        });
        h += '</tbody><tfoot><tr>' + vc.map(function (c) {
            return '<td class="' + (c.right ? 'num' : '') + '">' + esc(totalOf(c, vis)) + '</td>';
        }).join('') + '</tr></tfoot>';
        var focusKey = document.activeElement && document.activeElement.closest && document.activeElement.closest('#grdOutPutSettlement tr.flt')
            ? document.activeElement.getAttribute('data-k') : null;
        t.innerHTML = h;
        if (focusKey) {
            var inp = t.querySelector('tr.flt input[data-k="' + focusKey + '"]');
            if (inp) { inp.focus(); inp.setSelectionRange(inp.value.length, inp.value.length); }
        }
        var pos = vis.indexOf(outputRows[outCurrent]);
        $id('navOutput').textContent = 'Record ' + (pos >= 0 ? pos + 1 : 0) + ' of ' + vis.length;
    }
    function renderAll() {
        renderSimple('grdInputsettlement', INPUT_COLS, inputRows);
        renderOutput();
        renderSimple('grdOverHeadSettlement', OH_COLS, ohRows);
        renderSimple('grdPackingMaterialSettlement', PM_COLS, pmRows);
    }

    /* =============================================================== btnGeneralSettlement */

    /** btnGeneralSettlement_Click:376. */
    function generate() {
        return getJson(API + '/generate?jobOrderId=' + jobOrderId()).then(function (d) {
            var io = d.inputOutput || [];
            if (io.length > 0) {
                inputRows = []; outputRows = [];
                io.forEach(function (row) {
                    var qty = toD(col(row, 'Qty')), weight = toD(col(row, 'Weight'));
                    var rate = toD(col(row, 'Rate')), amount = toD(col(row, 'Amount'));
                    var entryType = toS(col(row, 'EntryType')), entryTypeDetail = toS(col(row, 'EntryTypeDetail'));
                    if (entryType === 'Input' || entryTypeDetail === 'ReturnToGodown') {
                        if (entryTypeDetail === 'ReturnToGodown') { qty = 0 - qty; weight = 0 - weight; amount = 0 - amount; }
                        inputRows.push({
                            DetailId: col(row, 'DetailId'), Id: col(row, 'Id'), RefDocumentTypeId: col(row, 'RefDocumentTypeId'),
                            RefDocIdNo: col(row, 'RefDocNoId'), RefDocSubIdNo: col(row, 'RefDocSubIdNo'),
                            WIPAccountId: col(row, 'WIPAccountId'), EntryType: col(row, 'EntryTypeDetail'),
                            /* formatted "dd-MMM-yy", then parsed back into a DateTime column: the time is lost */
                            DocDate: col(row, 'DocDate') ? toS(col(row, 'DocDate')).substring(0, 10) : '1900-01-01',
                            DocNo: col(row, 'DocCode'), WareHouseId: col(row, 'WarehouseId'), WareHouse: col(row, 'WareHouseName'),
                            ItemId: col(row, 'ItemId'), ItemName: col(row, 'ItemName'), ItemUOMId: col(row, 'ItemUomId'),
                            ItemUOM: col(row, 'ItemUom'), CropYear: col(row, 'CropBatch'), JobLotId: col(row, 'JobLotId'),
                            JobLot: col(row, 'JobLotDescription'), PackingTypeId: col(row, 'PackingtypeId'),
                            PackingType: col(row, 'PackTypeDesc'), Quantity: qty, Weight: weight, Rate: rate,
                            RateUOMId: col(row, 'RateUOMId'), RateUOM: col(row, 'RateUom'), ItemAmount: amount,
                            Remarks: col(row, 'Remarks'), WipAccount: col(row, 'AccountTitle'),
                            RefJobOrderId: col(row, 'RefJobOrderId'), RefJobOrder: col(row, 'RefJobOrder'),
                            RefJobStatus: col(row, 'RefJobStatus'), InvoiceNo: col(row, 'InvoiceNo'),
                            InvoiceApproved: col(row, 'InvoiceApproved'), InvoiceApprovedStatus: col(row, 'InvoiceApprovedStatus')
                        });
                    }
                    if (entryType === 'Output' && entryTypeDetail !== 'ReturnToGodown') {
                        var rUom = col(row, 'RateUom');
                        outputRows.push({
                            Id: toS(col(row, 'Id')), DetailId: toS(col(row, 'DetailId')), WIPAccountId: toS(col(row, 'WIPAccountId')),
                            EntryType: toS(col(row, 'EntryTypeDetail')), DocDate: dMMMyyText(col(row, 'DocDate')),
                            DocNo: toS(col(row, 'DocCode')), WareHouseId: toS(col(row, 'WarehouseId')),
                            WareHouse: toS(col(row, 'WareHouseName')), ItemId: toS(col(row, 'ItemId')),
                            ItemName: toS(col(row, 'ItemName')), ItemUOMId: toS(col(row, 'ItemUomId')),
                            ItemUOM: rUomText(col(row, 'ItemUom')), CropYear: toS(col(row, 'CropBatch')),
                            JobLotId: toS(col(row, 'JobLotId')), JobLot: toS(col(row, 'JobLotDescription')),
                            PackingTypeId: toS(col(row, 'PackingtypeId')), PackingType: toS(col(row, 'PackTypeDesc')),
                            Quantity: col(row, 'Qty'), Weight: col(row, 'Weight'), Rate: col(row, 'Rate'),
                            ItemAmount: col(row, 'Amount'), RateWithoutExp: col(row, 'Rate'), AmountWithoutExp: col(row, 'Amount'),
                            NetRate: col(row, 'NetRate'), RateUOMId: toS(col(row, 'RateUOMId')), RateUOM: rUomText(rUom),
                            ItemPmCost: col(row, 'ItemPmCost'), GeneralPmCost: col(row, 'GeneralPmCost'),
                            ItemOhCost: col(row, 'ItemOhCost'), GeneralOhCost: col(row, 'GeneralOhCost'),
                            TotalAmount: col(row, 'TotalAmount'), Remarks: toS(col(row, 'Remarks')),
                            Status: toS(col(row, 'Status')), ProductionApprovalStatus: toS(col(row, 'ProductionApprovalStatus'))
                        });
                    }
                });
            } else {
                inputRows = null; outputRows = null;
            }
            outCurrent = -1;
            var oh = d.overHeads || [];
            ohRows = oh.length > 0 ? oh.map(function (r) {
                return { Id: col(r, 'Id'), CharOfAccountId: col(r, 'CharOfAccountId'), DocumentTypeId: col(r, 'DocumentTypeId'),
                         TransRef: col(r, 'TransRef'), DocDate: col(r, 'DocDate'), DocNo: col(r, 'DocNo'),
                         AccountTitle: col(r, 'AccountTitle'), ItemQty: col(r, 'ItemQty'), ItemRate: col(r, 'ItemRate'),
                         Amount: col(r, 'Amount'), BrandId: col(r, 'BrandId'), BrandName: col(r, 'BrandName'),
                         BrandUomId: col(r, 'BrandUomId'), BrandUom: col(r, 'BrandUom') };
            }) : null;
            var pm = d.packingMaterial || [];
            pmRows = pm.length > 0 ? pm.map(function (r) {
                return { Id: col(r, 'Id'), ItemId: col(r, 'ItemId'), itemUomSchId: col(r, 'itemUomSchId'),
                         DocumentTypeId: col(r, 'DocumentTypeId'), ItemUom: col(r, 'ItemUom'), BrandId: col(r, 'BrandId'),
                         BrandUomId: col(r, 'BrandUomId'), DocDate: col(r, 'DocDate'), DocNo: col(r, 'DocNo'),
                         ItemName: col(r, 'ItemName'), Qty: col(r, 'Qty'), Rate: col(r, 'Rate'), Amount: col(r, 'Amount'),
                         Remarks: col(r, 'Remarks'), BrandName: col(r, 'BrandName'), BrandUom: col(r, 'BrandUom') };
            }) : null;
            renderAll();
            return calcFinishGoods().then(totalActualFinish);
        }).catch(function (e) { return msg(e.message); });
    }
    /** An equivalent stored into an untyped (string) grid column: 40.0 -> "40". */
    function rUomText(v) { return v === null || v === undefined ? '' : str15(toD(v)); }

    /* =============================================== TotalActualFinishAndJobOrderAmountsCalculate */
    function totalActualFinish() {
        var jobOrderAmount = 0, finishGoodsAmount = 0;
        outputVisible().forEach(function (r) {
            if (toS(r.EntryType) === 'FinishGoods' && toS(r.Status) === 'Used') {
                jobOrderAmount += toD(r.ItemAmount);
                finishGoodsAmount += toD(r.TotalAmount);
            }
        });
        $id('txtActualFinishGoodsAmount').value = fmt(roundAway(finishGoodsAmount, 0), '0,0');
        $id('txtFinishAmountAgainstJobOrder').value = fmt(roundAway(jobOrderAmount, 0), '0,0');
        $id('txtDifferenceAmount').value = fmt(roundAway(finishGoodsAmount - jobOrderAmount, 0), '0,0');
    }

    /* ==================================================== CalcualteFinishGoodsRateAndAmounts:1086 */
    function calcFinishGoods() {
        try {
            var RMQty = 0, RMWeight = 0, RMAmount = 0, RTGQty = 0, RTGWeight = 0, RTGAmount = 0;
            var BPQty = 0, BPWeight = 0, BPAmount = 0, BPOverHeads = 0, BPPacking = 0;
            var FGQty = 0, FGWeight = 0, FGAmount = 0;
            (inputRows || []).forEach(function (r) {
                if (toS(r.EntryType) === 'Issue') {
                    RMQty += toD(r.Quantity); RMWeight += toD(r.Weight); RMAmount += toD(r.ItemAmount);
                } else {
                    RTGQty += Math.abs(toD(r.Quantity)); RTGWeight += Math.abs(toD(r.Weight)); RTGAmount += Math.abs(toD(r.ItemAmount));
                }
            });
            $id('txtItemQtyRM').value = fmt(RMQty - RTGQty, '#,#');
            $id('txtNetWeightRM').value = fmt(RMWeight - RTGWeight, '#,#');
            $id('txtAmountRM').value = fmt(RMAmount - RTGAmount, '#,#');
            outputVisible().forEach(function (r) {
                if (toS(r.EntryType) === 'FinishGoods') {
                    FGQty += toD(r.Quantity); FGWeight += toD(r.Weight); FGAmount += toD(r.ItemAmount);
                } else {
                    BPQty += toD(r.Quantity); BPWeight += toD(r.Weight); BPAmount += toD(r.ItemAmount);
                    BPOverHeads += toD(r.GeneralOhCost) + toD(r.ItemOhCost);
                    BPPacking += toD(r.GeneralPmCost) + toD(r.ItemPmCost);
                }
            });
            var bpTotal = BPAmount + BPOverHeads + BPPacking;
            $id('txtItemQtyBP').value = fmt(BPQty, '#,#');
            $id('txtNetWeightBP').value = fmt(BPWeight, '#,#');
            $id('txtAmountBP').value = fmt(BPAmount, '#,#');
            $id('txtBPOverHead').value = fmt(BPOverHeads, '#,#');
            $id('txtBPPMCharges').value = fmt(BPPacking, '#,#');
            $id('txtBPTotalAmountWithExpenses').value = fmt(bpTotal, '#,#');
            var PMAmount = (pmRows || []).reduce(function (s, r) { return s + toD(r.Amount); }, 0);
            $id('txtPackingMaterialSettlement').value = fmt(PMAmount, '#,#');
            var OHAmount = (ohRows || []).reduce(function (s, r) { return s + toD(r.Amount); }, 0);
            $id('txtOverHeadsSettlement').value = fmt(OHAmount, '#,#');
            var withExp = RMAmount + PMAmount + OHAmount - (RTGAmount + bpTotal);
            var withoutExp = RMAmount - (RTGAmount + bpTotal);
            AvgRate = FGWeight > 0 ? withExp / FGWeight : 0;
            AvgRateWithoutExp = FGWeight > 0 ? withoutExp / FGWeight : 0;
            $id('txtAvgRateFG').value = fmt(AvgRate, 'N3');
            $id('txtFinishGoodRateWithOutExp').value = fmt(roundAway(AvgRateWithoutExp, 0), '0');
            $id('txtItemQtyFG').value = fmt(FGQty, '#,#');
            $id('txtNetWeightFG').value = fmt(FGWeight, '#,#');
            $id('txtAmountFG').value = fmt(roundAway(withExp, 0), '0,0');
        } catch (e) {
            return msg(e.message);
        }
        return proportion().then(function () {
            $id('btnApproveSettlement').classList.toggle('is-hidden', !(toD($id('txtAmountFG').value) > 0));
        }).catch(function (e) { return msg(e.message); });
    }

    /* ============================================================ ProporationFinishGoodsSettlement:1179 */
    function proportion() {
        var avgText = $id('txtAvgRateFG').value;
        if (avgText === '' || avgText === '0') {
            return msg('Finish Goods AvgRate Not Found Please check!');
        }
        var rows = outputVisible();
        /* CommonServices.GetUomScheduleByItemId is read for each row whose RateUOMId is 0 -
           fetched up front here so the loop below runs in the desktop's order. */
        var need = {};
        rows.forEach(function (r) { if (toI(r.RateUOMId) === 0) need[toI(r.ItemId)] = true; });
        var ids = Object.keys(need).filter(function (i) { return !uomCache[i]; });
        return Promise.all(ids.map(function (i) {
            return getJson(API + '/uom-by-item?itemId=' + i).then(function (l) { uomCache[i] = l || []; });
        })).then(function () {
            var total = 0;
            for (var i = 0; i < rows.length; i++) {
                var r = rows[i];
                if (toI(r.RateUOMId) === 0) {
                    var dt = uomCache[toI(r.ItemId)] || [];
                    if (dt.length > 0) {
                        var dr = dt.filter(function (u) { return toD(col(u, 'Equivalent')) === 40; });
                        if (dr.length === 0) {
                            renderOutput();
                            return msg('40KG RateUom not define please check');
                        }
                        r.RateUOMId = String(toI(col(dr[0], 'Id')));
                        r.RateUOM = String(toI(col(dr[0], 'Equivalent')));
                    }
                }
                var Weight, Rate, RateUOM, ItemPmCost, GeneralPmCost, ItemOhCost, GeneralOhCost, rice;
                if (toS(r.EntryType) === 'FinishGoods') {
                    Weight = r15(toD(r.Weight));
                    var JobOrderRate = toD(r.Rate);
                    Rate = AvgRateWithoutExp;
                    RateUOM = toD(r.RateUOM);
                    ItemPmCost = r15(toD(r.ItemPmCost)); GeneralPmCost = r15(toD(r.GeneralPmCost));
                    ItemOhCost = r15(toD(r.ItemOhCost)); GeneralOhCost = r15(toD(r.GeneralOhCost));
                    rice = Weight * Rate;
                    var finishWithExp = rice + ItemPmCost + GeneralPmCost + ItemOhCost + GeneralOhCost;
                    var brandRate = finishWithExp / Weight * RateUOM;
                    if (toD(r.ItemAmount) === 0) r.ItemAmount = Weight / RateUOM * JobOrderRate;
                    r.RateWithoutExp = Rate * RateUOM;
                    r.AmountWithoutExp = toD(str15(rice));
                    r.NetRate = brandRate;
                    r.TotalAmount = toD(str15(finishWithExp));
                    total += finishWithExp;
                } else {
                    Weight = r15(toD(r.Weight));
                    Rate = r15(toD(r.Rate));
                    RateUOM = toD(r.RateUOM);
                    ItemPmCost = r15(toD(r.ItemPmCost)); GeneralPmCost = r15(toD(r.GeneralPmCost));
                    ItemOhCost = r15(toD(r.ItemOhCost)); GeneralOhCost = r15(toD(r.GeneralOhCost));
                    rice = Weight / RateUOM * Rate;
                    var bpWithExp = rice + ItemPmCost + GeneralPmCost + ItemOhCost + GeneralOhCost;
                    r.RateWithoutExp = Rate;
                    r.AmountWithoutExp = toD(str15(rice));
                    r.NetRate = bpWithExp / Weight * RateUOM;
                    r.TotalAmount = toD(str15(bpWithExp));
                }
            }
            renderOutput();
            $id('txtAmountFG').value = fmt(roundAway(total, 0), '0,0');
        });
    }

    /* ============================================================ grdOutPutSettlement_CellUpdated:1038 */
    function rateCellUpdated(idx, text) {
        var item = outputRows[idx];
        if (!item) return;
        item.Rate = toD(text);
        if (toS(item.EntryType) === 'ByProduct') {
            var Weight = r15(toD(item.Weight));
            var Rate = r15(toD(item.Rate));
            item.RateWithoutExp = Rate;
            item.NetRate = Rate;
            var RateUOM = toD(item.RateUOM);
            if (Weight > 0 && RateUOM > 0 && Rate > 0) {
                var Amount = Weight / RateUOM * Rate;
                item.ItemAmount = Amount;
                item.TotalAmount = Amount;
            } else {
                item.ItemAmount = 0;
                item.TotalAmount = 0;
            }
        }
        renderOutput();
    }

    /* ============================================================ ResetSettlement:1277 */
    function resetSettlement() {
        $id('btnApproveSettlement').classList.add('is-hidden');
        ['txtItemQtyRM', 'txtItemQtyBP', 'txtItemQtyFG', 'txtNetWeightRM', 'txtNetWeightBP', 'txtNetWeightFG',
         'txtAmountRM', 'txtAmountBP', 'txtAmountFG', 'txtBPOverHead', 'txtBPPMCharges', 'txtActualFinishGoodsAmount',
         'txtFinishAmountAgainstJobOrder', 'txtDifferenceAmount', 'txtBPTotalAmountWithExpenses',
         'txtOverHeadsSettlement', 'txtPackingMaterialSettlement', 'txtAvgRateFG', 'txtFinishGoodRateWithOutExp']
            .forEach(function (id) { $id(id).value = ''; });
        $id('CmbDifferenceAccountTitle').value = '';
        $id('CmbJobOrderSettlement').value = '';
        /* ClearStructure: the grids lose their columns; the DataSource stays assigned. */
        inputRows = inputRows ? [] : null;
        outputRows = outputRows ? [] : null;
        ohRows = ohRows ? [] : null;
        pmRows = pmRows ? [] : null;
        outCurrent = -1;
        ['grdInputsettlement', 'grdOutPutSettlement', 'grdOverHeadSettlement', 'grdPackingMaterialSettlement']
            .forEach(function (id) { $id(id).innerHTML = ''; });
        $id('navOutput').textContent = '';
    }

    /* ============================================================ btnUpdateSettlement_Click:1659 */
    function outputPayload(rows) {
        function n(v) { return typeof v === 'number' && !isFinite(v) ? String(v) : v; }
        return rows.map(function (r) {
            return { Id: r.Id, DetailId: r.DetailId, WIPAccountId: r.WIPAccountId, EntryType: r.EntryType,
                     DocNo: r.DocNo, WareHouseId: toS(r.WareHouseId), ItemId: toS(r.ItemId), ItemName: r.ItemName,
                     ItemUOMId: r.ItemUOMId, CropYear: toS(r.CropYear), JobLotId: toS(r.JobLotId),
                     PackingTypeId: r.PackingTypeId, Quantity: n(r.Quantity), Weight: n(r.Weight), Rate: n(r.Rate),
                     ItemAmount: n(r.ItemAmount), RateWithoutExp: n(r.RateWithoutExp),
                     AmountWithoutExp: n(r.AmountWithoutExp), NetRate: n(r.NetRate), RateUOMId: r.RateUOMId,
                     ItemPmCost: n(r.ItemPmCost), GeneralPmCost: n(r.GeneralPmCost), ItemOhCost: n(r.ItemOhCost),
                     GeneralOhCost: n(r.GeneralOhCost), TotalAmount: n(r.TotalAmount), Remarks: toS(r.Remarks),
                     Status: r.Status, ProductionApprovalStatus: r.ProductionApprovalStatus };
        });
    }
    /** UpdateSettlement():1327 - true, false (a MessageBox was shown), or throws. */
    function updateSettlement() {
        return postJson(API + '/update-output', { jobOrderId: jobOrderId(), rows: outputPayload(outputVisible()) })
            .then(function (r) {
                if (r && r.result === true) return true;
                return msg(r.message, r.caption || '', r.icon || null).then(function () { return false; });
            });
    }
    /** InPutUpdateSettlement():1431 - throws on any failure. */
    function inputUpdateSettlement() {
        var rows = (inputRows || []).map(function (r) {
            return { Id: r.Id, DetailId: r.DetailId, EntryType: toS(r.EntryType), RefDocumentTypeId: r.RefDocumentTypeId,
                     RefDocIdNo: r.RefDocIdNo, RefDocSubIdNo: r.RefDocSubIdNo, WareHouseId: r.WareHouseId,
                     ItemId: r.ItemId, ItemUOMId: r.ItemUOMId, CropYear: toS(r.CropYear), JobLotId: r.JobLotId,
                     PackingTypeId: r.PackingTypeId, Quantity: r.Quantity, Weight: r.Weight, Rate: r.Rate,
                     RateUOMId: r.RateUOMId, ItemAmount: r.ItemAmount, WIPAccountId: r.WIPAccountId,
                     Remarks: toS(r.Remarks), RefJobOrderId: r.RefJobOrderId };
        });
        return postJson(API + '/update-input', { jobOrderId: jobOrderId(), rows: rows });
    }

    /** SettlementFinancials():1505 - reports its own errors, never throws to the caller. */
    function settlementFinancials() {
        var diff = toD($id('txtDifferenceAmount').value);
        if (diff === 0) { resetSettlement(); return Promise.resolve(); }
        if (!jobOrderActiveRow()) {
            return msg('JobOrder Settlement Field is Required').then(function () { focusCombo('CmbJobOrderSettlement'); });
        }
        if (diffAccountId() === 0) {
            return msg('Difference Account Field is Required').then(function () { focusCombo('CmbDifferenceAccountTitle'); });
        }
        var gateBody = { jobOrderId: jobOrderId(), differenceAccountId: diffAccountId() };
        return postJson(API + '/financials/gate', gateBody).then(function (g) {
            if (g.action === 'confirm') {
                return ask(g.message, 'Confirm').then(function (yes) {
                    if (!yes) return null;
                    return postJson(API + '/financials/request-special-approval', gateBody).then(function () {
                        throw new Error('Job Order Sent for special approval.');
                    });
                });
            }
            return postJson(API + '/financials/save', {
                jobOrderId: jobOrderId(), jobOrderText: jobOrderText(), differenceAccountId: diffAccountId(),
                settlementDate: settlementDate, differenceAmountText: $id('txtDifferenceAmount').value,
                actualFinishGoodsText: $id('txtActualFinishGoodsAmount').value,
                finishAmountAgainstJobOrderText: $id('txtFinishAmountAgainstJobOrder').value
            }).then(function (s) {
                return msg(toI(s.result) > 0 ? 'Record Save Successfully' : 'Record Update Successfully')
                    .then(resetSettlement);
            });
        }).catch(function (e) { return msg(e.message, 'Message', 'error'); });
    }
    function focusCombo(id) {
        var w = $id(id).closest('.dtcombo-wrap');
        var i = w ? w.querySelector('input') : $id(id);
        if (i) i.focus();
    }

    function saleCostingBlocks(dt) {
        if (dt.some(function (x) { return toI(x.RefJobOrderId) > 0 && toS(x.RefJobStatus) === 'UnSettled'; })) {
            return 'Some related Job Orders have already been used in Production Input but are still pending settlement. Please settle and approve them first to continue.';
        }
        if (dt.some(function (x) { return toI(x.InvoiceNo) > 0 && toI(x.InvoiceApproved) === 0; })) {
            return 'You cannot proceed because some invoices have not been approved yet. Please review and approve them first.';
        }
        return null;
    }

    function btnUpdate() {
        return Promise.resolve().then(function () {
            if (outputVisible().length === 0) throw new Error('OutPut grid records not found please check....');
            if (!outputRows.some(function (r) { return r.EntryType === 'FinishGoods'; })) {
                throw new Error('Finish Goods records not found. Please check.');
            }
            return ask('Are you sure to Update?', 'Confirm').then(function (yes) {
                if (!yes) return null;
                var chain;
                if (cfg.fifoCgs && !cfg.saleCostingJobOrderWise) {
                    var diff = toD($id('txtDifferenceAmount').value);
                    if (outputRows.some(function (x) { return x.EntryType === 'FinishGoods' && x.Status === 'NotUsed'; })) {
                        chain = updateSettlement().then(function (ok) { if (ok) return settlementFinancials(); });
                    } else if (diff !== 0) {
                        chain = settlementFinancials();
                    } else {
                        resetSettlement();
                        chain = Promise.resolve();
                    }
                } else if (cfg.saleCostingJobOrderWise) {
                    if (!inputRows) throw new Error('Value cannot be null.\nParameter name: source');
                    var block = saleCostingBlocks(inputRows);
                    if (block) return msg(block, 'Message', 'error').then(function () { return 'stop'; });
                    var needInput = inputRows.some(function (x) {
                        var t = toI(x.RefDocumentTypeId);
                        return (t === 112 && toI(x.RefJobOrderId) > 0) || t === 56 || t === 57 || t === 40;
                    });
                    chain = (needInput ? inputUpdateSettlement() : Promise.resolve())
                        .then(updateSettlement).then(resetSettlement);
                } else {
                    chain = updateSettlement().then(resetSettlement);
                }
                return chain.then(function () {
                    return jobOrderNoFill($id('chkAllJobOrderOnSettlement').checked ? 0 : 1);
                });
            });
        }).catch(function (e) { return msg(e.message); });
    }

    /* ============================================================ btnApproveSettlement_Click:1905 */
    function btnApprove() {
        return Promise.resolve().then(function () {
            if (cfg.saleCostingJobOrderWise) {
                if (!inputRows) throw new Error('Value cannot be null.\nParameter name: source');
                var block = saleCostingBlocks(inputRows);
                if (block) return msg(block, 'Message', 'error');
            }
            if (!outputRows) throw new Error('Value cannot be null.\nParameter name: source');
            var pending = outputRows.filter(function (x) { return x.ProductionApprovalStatus === 'UnApproved'; })[0];
            if (pending) {
                return msg('Settlement cannot be processed because the Production Output transaction is still pending approval.\n\nItem: '
                    + toS(pending.ItemName) + '\nDocument No: ' + toS(pending.DocNo), 'Approval Pending', 'error');
            }
            return ask('Are you sure to Approve Settlement?', 'Confirm').then(function (yes) {
                if (!yes) return null;
                return postJson(API + '/approve', { jobOrderId: jobOrderId() }).then(function () {
                    return msg('Record Approved Successfully').then(resetSettlement);
                });
            });
        }).catch(function (e) { return msg(e.message, 'Error Message', 'error'); });
    }

    /* ============================================================ prints */
    function actionId() { return $id('chkIncludeWages').checked ? 1 : 0; }
    /** Report buttons :1740-1903, :2218 - rows first ("Record Not Found For DisPlay"), then the viewer. */
    function printReport(code, key, btnId) {
        var btn = $id(btnId);
        if (btn.classList.contains('is-busy')) return;
        var jo = jobOrderId(), act = actionId();
        var w = window.CrystalPrint.reserve();
        btn.classList.add('is-busy'); btn.disabled = true;
        var restore = function () { btn.classList.remove('is-busy'); btn.disabled = false; };
        getJson(API + '/print-rows?report=' + code + '&jobOrderId=' + jo + '&actionId=' + act).then(function (r) {
            restore();
            if (r && r.rows > 0) return window.CrystalPrint.open(key, { id: jo, actionId: act }, btn, w);
            window.CrystalPrint.release(w);
            return msg('Record Not Found For DisPlay');
        }).catch(function (e) { restore(); window.CrystalPrint.release(w); return msg(e.message); });
    }

    /** btnSettlementVoucher_Click:1975 -> AcRptPurchaseSalesVoucherSlip_103(VoucherId, 142) when VoucherId > 0. */
    function btnVoucher() {
        var btn = $id('btnSettlementVoucher');
        if (btn.classList.contains('is-busy')) return;
        btn.classList.add('is-busy'); btn.disabled = true;
        var restore = function () { btn.classList.remove('is-busy'); btn.disabled = false; };
        var w = null;
        getJson(API + '/voucher-head?jobOrderId=' + jobOrderId()).then(function (v) {
            var id = toI(v && v.voucherHeadId);
            if (id <= 0) { restore(); return null; }
            w = window.CrystalPrint.reserve();
            return getJson(API + '/print-rows?report=103&voucherHeadId=' + id).then(function (r) {
                restore();
                if (r && r.rows > 0) return window.CrystalPrint.open('p280s-103', { id: id, documentTypeId: 142 }, btn, w);
                window.CrystalPrint.release(w);
                return msg('No Record Found For Display');
            });
        }).catch(function (e) { restore(); window.CrystalPrint.release(w); return msg(e.message); });
    }

    /* ============================================================ BtnByProductSettlementPopUp_Click:2102 */
    var popupRows = null;
    function averagedByProduct() {
        /* GetAveragedByProductData:2134 */
        var dt = [];
        outputVisible().forEach(function (row) {
            if (toS(row.EntryType) !== 'ByProduct') return;
            var itemId = toI(row.ItemId), qty = toD(row.Quantity), weight = toD(row.Weight);
            var rate = toD(row.Rate), rateUom = toD(row.RateUOM);
            var perKg = rateUom > 0 ? rate / rateUom : 0;
            var rateTotal = perKg * weight;
            var ex = dt.filter(function (r) { return r.ItemId === itemId; })[0];
            if (ex) {
                ex.Qty += qty; ex.Weight += weight; ex.RateTotal += rateTotal;
                ex['AvgRate/Kg'] = ex.Weight > 0 ? ex.RateTotal / ex.Weight : 0;
                ex['Rate/Kg'] = ex.Weight > 0 ? ex.RateTotal / ex.Weight : 0;
            } else {
                dt.push({ ItemId: itemId, ItemName: toS(row.ItemName), Qty: qty, Weight: weight,
                          'AvgRate/Kg': perKg, 'Rate/Kg': perKg, RateTotal: rateTotal });
            }
        });
        dt.forEach(function (r) { delete r.RateTotal; });
        return dt;
    }
    function openByProductPopup() {
        popupRows = averagedByProduct();
        var cols = ['ItemName', 'Qty', 'Weight', 'AvgRate/Kg', 'Rate/Kg'];
        var h = '<thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr></thead><tbody>';
        popupRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                if (c === 'Rate/Kg') return '<td class="num"><input class="edit" data-i="' + i + '" value="' + esc(str15(r[c])) + '"></td>';
                return '<td class="' + (c === 'ItemName' ? '' : 'num') + '">' + esc(c === 'ItemName' ? r[c] : str15(r[c])) + '</td>';
            }).join('') + '</tr>';
        });
        $id('grdByProductPopup').innerHTML = h + '</tbody>';
        $id('dlgByProduct').classList.remove('is-hidden');
        var first = $id('grdByProductPopup').querySelector('input.edit');
        if (first) first.focus();
    }
    function closeByProductPopup(returnRows) {
        $id('dlgByProduct').classList.add('is-hidden');
        var dtReturn = returnRows ? popupRows.slice() : [];
        popupRows = null;
        if (dtReturn.length > 0) {
            applyUpdatedRates(dtReturn);
            return calcFinishGoods();
        }
        return Promise.resolve();
    }
    /** ApplyUpdatedRatesToGrid:2178. */
    function applyUpdatedRates(dtReturn) {
        outputVisible().forEach(function (row) {
            if (toS(row.EntryType) !== 'ByProduct') return;
            var itemId = toI(row.ItemId);
            var m = dtReturn.filter(function (r) { return r.ItemId === itemId; })[0];
            if (!m) return;
            var perKg = toD(m['Rate/Kg']);
            var rateUOM = toD(row.RateUOM);
            var rate = perKg * rateUOM;
            var weight = toD(row.Weight);
            var amount = weight > 0 && rateUOM > 0 && rate > 0 ? weight / rateUOM * rate : 0;
            row.Rate = rate; row.RateWithoutExp = rate; row.NetRate = rate;
            row.ItemAmount = amount; row.TotalAmount = amount;
        });
        renderOutput();
    }

    /* ============================================================ grid links */
    /** grdOverHeadSettlement_LinkClicked:972. */
    function overHeadLink(i) {
        var r = ohRows && ohRows[i];
        if (!r) return;
        var id = jobOrderId();
        switch (toI(r.DocumentTypeId)) {
            case 80: case 112: {
                /* [wages-register wiring, round 2] new frmEvaulationDetailWagesReports(UserAccount).Show();
                   CmbJobOrderNo.Value = Id; GridFill() - the page replays the last two from ?jobOrderId. */
                var ww = window.open('/production/reports/evaluation-detail-wages?jobOrderId=' + encodeURIComponent(id), '_blank');
                if (!ww) msg('The browser blocked the new window.');
                break;
            }
            case 110:
                if (OpenInProductionTab) {
                    if (window.parent.P280OpenTab) window.parent.P280OpenTab('Overhead', id);
                    break;
                }
                openStandalone('overhead', id);
                break;
            default: break;
        }
    }
    /** grdPackingMaterialSettlement_LinkClicked:1017. */
    function packingLink(i) {
        var r = pmRows && pmRows[i];
        if (!r) return;
        var id = toI(r.Id);
        if (OpenInProductionTab) {
            if (window.parent.P280OpenTab) window.parent.P280OpenTab('PackingMaterial', id);
            return;
        }
        openStandalone('packing-material', id);
    }
    /** new frmProductionOverhead / frmProductionPackingMaterial, Show(), then ReadById(Id). */
    function openStandalone(slug, id) {
        var w = window.open(BASE + slug, '_blank');
        if (!w) { msg('The browser blocked the new window.'); return; }
        var tries = 0;
        var t = setInterval(function () {
            tries++;
            try {
                if (w.closed || tries > 150) { clearInterval(t); return; }
                if (typeof w.P280ReadById === 'function') { clearInterval(t); w.P280ReadById(id); }
            } catch (e) { /* not ready yet */ }
        }, 200);
    }

    /* ============================================================ events */
    function wire() {
        var cmbJo = $id('CmbJobOrderSettlement');
        cmbJo.addEventListener('change', jobOrderLeave);
        var wrap = cmbJo.closest('.dtcombo-wrap') || cmbJo;
        wrap.addEventListener('focusout', function (e) {
            if (!wrap.contains(e.relatedTarget)) jobOrderLeave();
        });

        $id('txtSettlementDate').addEventListener('change', function () {
            var v = $id('txtSettlementDate').value;
            if (v) settlementDate = v + settlementDate.substring(10);
        });

        /* chkIncludeWages.CheckedChanged is wired to chkAllJobOrderOnSettlement_CheckedChanged (designer). */
        function refill() { jobOrderNoFill($id('chkAllJobOrderOnSettlement').checked ? 0 : 1); }
        $id('chkAllJobOrderOnSettlement').addEventListener('change', refill);
        $id('chkIncludeWages').addEventListener('change', refill);

        $id('btnGeneralSettlement').addEventListener('click', function () { busy('btnGeneralSettlement', generate); });
        $id('btnApplyAvgRateFG').addEventListener('click', function () {
            busy('btnApplyAvgRateFG', function () {
                if (outputVisible().length === 0) return msg('OutPut grid records not found please check....');
                return calcFinishGoods();
            });
        });
        $id('btnUpdateSettlement').addEventListener('click', function () { busy('btnUpdateSettlement', btnUpdate); });
        $id('btnApproveSettlement').addEventListener('click', function () { busy('btnApproveSettlement', btnApprove); });
        $id('btnResetSettlement').addEventListener('click', function () {
            busy('btnResetSettlement', function () { resetSettlement(); return jobOrderNoFill(1); });
        });
        $id('btnSettlementVoucher').addEventListener('click', btnVoucher);
        $id('btnRecoveryReport').addEventListener('click', function () { printReport('602A', 'p280s-602A', 'btnRecoveryReport'); });
        $id('btn613SummaryReport').addEventListener('click', function () { printReport('613', 'p280s-613', 'btn613SummaryReport'); });
        $id('BtnPrint613A').addEventListener('click', function () { printReport('613A', 'p280s-613A', 'BtnPrint613A'); });
        $id('BtnPrint613B').addEventListener('click', function () { printReport('613B', 'p280s-613B', 'BtnPrint613B'); });
        $id('btn615Report').addEventListener('click', function () { printReport('615', 'p280s-615', 'btn615Report'); });
        $id('BtnPrint615A').addEventListener('click', function () { printReport('615A', 'p280s-615A', 'BtnPrint615A'); });
        $id('btnAllocateExportInvoiceToJobOrder').addEventListener('click', function () {
            /* [allocation wiring, round 2] new ProductionOutputAllocationWithExportInvoice(UserAccount).Show(). */
            var wa = window.open('/production/output-allocation-export-invoice', '_blank');
            if (!wa) msg('The browser blocked the new window.');
        });
        $id('BtnByProductSettlementPopUp').addEventListener('click', function () {
            try { openByProductPopup(); } catch (e) { msg(e.message); }
        });

        /* GrdPopUpToReturndt: double-click returns every row; Esc / Ctrl+E / close returns nothing. */
        var dlg = $id('dlgByProduct');
        dlg.addEventListener('dblclick', function (e) {
            if (e.target.closest('tbody tr')) closeByProductPopup(true);
        });
        dlg.addEventListener('change', function (e) {
            var inp = e.target.closest('input.edit');
            if (inp && popupRows) popupRows[toI(inp.getAttribute('data-i'))]['Rate/Kg'] = toD(inp.value);
        });
        dlg.querySelector('[data-close]').addEventListener('click', function () { closeByProductPopup(false); });

        /* grids */
        $id('grdOverHeadSettlement').addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link="oh"]');
            if (a) overHeadLink(toI(a.getAttribute('data-i')));
        });
        $id('grdPackingMaterialSettlement').addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link="pm"]');
            if (a) { try { packingLink(toI(a.getAttribute('data-i'))); } catch (ex) { msg(ex.message); } }
        });
        var out = $id('grdOutPutSettlement');
        out.addEventListener('input', function (e) {
            var f = e.target.closest('tr.flt input');
            if (f) { outFilter[f.getAttribute('data-k')] = f.value; renderOutput(); }
        });
        out.addEventListener('change', function (e) {
            var inp = e.target.closest('input.edit');
            if (inp) rateCellUpdated(toI(inp.getAttribute('data-edit')), inp.value);
        });
        out.addEventListener('mousedown', function (e) {
            var tr = e.target.closest('tbody tr[data-row]');
            if (!tr) return;
            var idx = toI(tr.getAttribute('data-row'));
            if (idx !== outCurrent) {
                outCurrent = idx;
                Array.prototype.forEach.call(out.querySelectorAll('tbody tr'), function (x) {
                    x.classList.toggle('cur', x === tr);
                });
                var vis = outputVisible();
                $id('navOutput').textContent = 'Record ' + (vis.indexOf(outputRows[idx]) + 1) + ' of ' + vis.length;
            }
        });
        $id('wrapOutput').addEventListener('keydown', function (e) {
            if (e.key !== 'ArrowUp' && e.key !== 'ArrowDown') return;
            if (e.target.closest && e.target.closest('input')) return;
            var vis = outputVisible();
            if (!vis.length) return;
            e.preventDefault();
            var pos = vis.indexOf(outputRows[outCurrent]);
            pos = e.key === 'ArrowUp' ? Math.max(0, pos - 1) : Math.min(vis.length - 1, pos + 1);
            outCurrent = outputRows.indexOf(vis[pos]);
            renderOutput();
        });

        /* frmFoodProduction_KeyDown:345 - Enter -> Tab; Ctrl+E / Esc -> Close(). */
        K.enterToTab();
        document.addEventListener('keydown', function (e) {
            var dlgOpen = !$id('dlgByProduct').classList.contains('is-hidden');
            if ((e.ctrlKey && (e.key === 'e' || e.key === 'E')) || e.key === 'Escape') {
                if (document.querySelector('.st-modal:not(.is-hidden):not(#dlgByProduct)')) return;
                if (document.querySelector('.dtcombo-pop[style*="block"]')) return;
                e.preventDefault();
                if (dlgOpen) { closeByProductPopup(false); return; }
                closeForm();
            }
        });
    }

    /** Close(): standalone -> back to Production; in the 280 frame the hosted form just goes away. */
    function closeForm() {
        if (OpenInProductionTab) { $id('frmProductionSettlement').style.display = 'none'; return; }
        K.close();
    }

    /* ============================================================ frmFoodProduction_Load:262 */
    function load() {
        return getJson(API + '/load').then(function (d) {
            cfg.jobOrderCreatewithoutRates = !!d.jobOrderCreatewithoutRates;
            cfg.saleCostingJobOrderWise = !!d.saleCostingJobOrderWise;
            cfg.fifoCgs = !!d.fifoCgs;
            var showBp = cfg.jobOrderCreatewithoutRates || cfg.saleCostingJobOrderWise;
            $id('BtnByProductSettlementPopUp').classList.toggle('is-hidden', !showBp);
            $id('label171').classList.toggle('is-hidden', !showBp);
            /* CmbDifferenceAccountTitle.Visible = FIFOCGSFlag; its label stays visible either way. */
            var diff = $id('CmbDifferenceAccountTitle');
            (diff.closest('.dtcombo-wrap') || diff).classList.toggle('is-hidden', !cfg.fifoCgs);
            focusCombo('CmbJobOrderSettlement');
            var acc = d.differenceAccounts || [];
            if (acc.length > 0) {
                diff.innerHTML = '<option value=""></option>' + acc.map(function (a) {
                    return '<option value="' + esc(col(a, 'Id')) + '">' + esc(col(a, 'AccountTitle')) + '</option>';
                }).join('');
            }
            if (d.jobOrders && d.jobOrders.length > 0) fillJobOrders(d.jobOrders, false);
            jobOrderLeave();
            /* RequestedByOtherDocument && JobOrderId > 0 - another document opened this form. */
            var q = new URLSearchParams(window.location.search);
            var reqId = toI(q.get('jobOrderId'));
            if (reqId > 0) {
                $id('CmbJobOrderSettlement').value = String(reqId);
                jobOrderLeave();
                return busy('btnGeneralSettlement', generate).then(function () {
                    if (outputVisible().length === 0) return msg('OutPut grid records not found please check....');
                    return calcFinishGoods();
                });
            }
        }).catch(function (e) { return msg(e.message); });
    }

    function boot() {
        wire();
        $id('txtSettlementDate').value = settlementDate.substring(0, 10);
        load();
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
