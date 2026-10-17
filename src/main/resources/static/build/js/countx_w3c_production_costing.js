/* ============================================================================================
 * 887 "Production Costing".
 * Desktop: Architecture.WinApp.Production/frmCreateProductionCostingReport.cs (DocumentTypeId 921) and its
 * History tab Architecture.WinApp.Production_Reports/frmProductionCostingReport.cs.
 *
 * Line numbers below are frmCreateProductionCostingReport.cs's unless stated. The grid arithmetic
 * (CalculateHeaderValues:1374, UpdateInvoiceGridValues:1405, CalculateTotalCostDetail:1436, grdInputWorking_CellUpdated:674,
 * the "+" / Update / X buttons) runs here as it runs in the desktop form; every database step and every refusal
 * Insert():957 makes runs on the server (W3CProductionCostingService).
 * Desktop re-parses the ROUNDED textbox text in its calculations; this page does the same (read(), fmt*()).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/production/production-costing';
    var K = window.ReportKit;

    var RecId = 0;
    var jobOrderId = 0;
    var rights = { save: false, update: false, print: false };
    var amtDec = 0, rateDec = 2, defDays = 0;
    var dtInput = [], dtBp = [], dtdetail = [];
    var lstDetailRemove = [];
    var updateDetailIndex = -1;
    var DataReset = false;
    var jobRows = [], invRows = [];
    var historyRows = null, histFrom = '', histTo = '';
    var fyStart = '', today = '';
    var icur = -1, bcur = -1, dcur = -1, hcur = -1;
    var bpPopupRows = [];

    function $id(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function say(m) { $id('lblStatus').textContent = m || ''; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function ymd(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function colAny(row, names) {
        for (var i = 0; i < names.length; i++) { var v = col(row, names[i]); if (v !== null && v !== undefined) return v; }
        return null;
    }
    function toD(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var n = Number(String(v).trim().replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function toI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function toS(v) { return v === null || v === undefined ? '' : String(v); }

    /* ---- .NET custom numeric formats: "#,##0.00" style; round half away from zero on the 15-digit value ---- */
    function roundTo(x, dec) {
        var neg = x < 0, a = Math.abs(Number(x.toPrecision(15)));
        var r = Number(Math.round(Number(a + 'e' + dec)) + 'e-' + dec);
        return neg && r !== 0 ? -r : r;
    }
    function grp(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmtFixed(x, dec) {
        if (!isFinite(x)) return String(x);
        var r = roundTo(x, dec), neg = r < 0, t = Math.abs(r).toFixed(dec).split('.');
        return (neg ? '-' : '') + grp(t[0]) + (t.length > 1 ? '.' + t[1] : '');
    }
    /* clsGlobalVariables.stringFormatsingle = "#,##0." + zeros (no point when 0 decimals) */
    function fmtAmt(x) { return fmtFixed(x, amtDec); }
    /* clsGlobalVariables.DecimalRateFormate = "#,#0." + zeros (0 / unset -> 2) */
    function fmtRate(x) { return fmtFixed(x, rateDec); }
    /* "#,##0.###" */
    function fmt3(x) {
        var s = fmtFixed(x, 3);
        return s.indexOf('.') >= 0 ? s.replace(/0+$/, '').replace(/\.$/, '') : s;
    }
    function read(id) { return toD($id(id).value.trim()); }
    function setTxt(id, v) { $id(id).value = v; }

    /* ------------------------------------------------------------------ MessageBox */
    var ICON = { error: 'fa-times-circle" style="color:#c00' };
    function dialog(text, caption, icon, buttons) {
        return new Promise(function (resolve) {
            var m = document.createElement('div');
            m.className = 'pa-modal pa-msg';
            m.innerHTML = '<div class="pa-dlg" role="dialog"><div class="cap"><span>' + esc(caption || '') + '</span></div>'
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
    function ask(text, caption) { return dialog(text, caption, null, ['Yes', 'No']).then(function (i) { return i === 0; }); }
    function modalOpen() { return !!document.querySelector('.pa-msg, .pa-modal:not(.is-hidden), .rk-modal.is-open'); }

    /* ------------------------------------------------------------------ requests */
    function headers(h) {
        var t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn && t.getAttribute('content')) h[hn.getAttribute('content')] = t.getAttribute('content');
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
    function qs(o) {
        return Object.keys(o).filter(function (k) { return o[k] !== null && o[k] !== undefined && o[k] !== ''; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(o[k]); }).join('&');
    }
    function busy(btn, fn) {
        var b = typeof btn === 'string' ? $id(btn) : btn;
        if (b && b.classList.contains('is-busy')) return Promise.resolve();
        var was = b ? b.disabled : false;
        if (b) { b.classList.add('is-busy'); b.disabled = true; }
        var done = function () { if (b) { b.classList.remove('is-busy'); b.disabled = was; } };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    }

    /* ------------------------------------------------------------------ combos */
    if (window.DesktopCombo) {
        /* InfragisticsHelper.BindAndRetainSelection(CmbJobOrderNo, dt, "JobOrderId", "JobOrderNo", "JobOrderNo", AllColumns:true):434 */
        window.DesktopCombo.define('w3cJobOrder', [
            { caption: 'JobOrderNo', flex: 3 },
            { caption: 'Finish Output %', flex: 2, key: 'fin', type: 'num' }
        ]);
        /* BindAndRetainSelection(CmbInvoiceNoDetail, dt, "Id", "InvoiceNo", "Invoice No", AllColumns:true):446 */
        window.DesktopCombo.define('w3cInvoice', [
            { caption: 'Invoice No', flex: 2 },
            { caption: 'Customer', flex: 3, key: 'cust' },
            { caption: 'Fcy', flex: 1, key: 'fcy' },
            { caption: 'Exchange Rate', flex: 1, key: 'xr', type: 'num' }
        ]);
    }
    function cssEsc(v) { return window.CSS && CSS.escape ? CSS.escape(v) : String(v).replace(/"/g, '\\"'); }
    function setValue(sel, v) {
        var s = String(v === null || v === undefined ? '' : v);
        Array.prototype.forEach.call(sel.querySelectorAll('option[data-orphan]'), function (o) { o.remove(); });
        if (s === '' || s === '0') { sel.value = ''; return; }
        if (!sel.querySelector('option[value="' + cssEsc(s) + '"]')) {
            var o = document.createElement('option');
            o.value = s; o.textContent = s; o.setAttribute('data-orphan', '1');
            sel.appendChild(o);
        }
        sel.value = s;
    }
    function activeRow(sel) {
        var o = sel.options[sel.selectedIndex];
        return o && o.value !== '' && !o.hasAttribute('data-orphan') ? o : null;
    }
    function setComboEnabled(sel, on) {
        sel.disabled = !on;
        var w = sel.closest('.dtcombo-wrap');
        if (w) Array.prototype.forEach.call(w.querySelectorAll('input, button'), function (x) { x.disabled = !on; });
    }
    function focusCombo(sel) {
        var w = sel.closest('.dtcombo-wrap');
        var inp = w ? w.querySelector('input') : null;
        (inp || sel).focus();
    }
    function comboText(sel) {
        var w = sel.closest('.dtcombo-wrap'), inp = w ? w.querySelector('input') : null;
        var o = sel.options[sel.selectedIndex];
        return inp && inp.value ? inp.value : (o && o.value !== '' ? o.textContent : '');
    }
    function onLeave(sel, fn) {
        setTimeout(function () {
            var wrap = sel.closest('.dtcombo-wrap') || sel;
            wrap.addEventListener('focusout', function () {
                setTimeout(function () {
                    var a = document.activeElement;
                    if (a && (wrap.contains(a) || (a.closest && a.closest('.dtcombo-pop')))) return;
                    fn();
                }, 0);
            });
        }, 0);
    }
    function fillJobOrders(rows) {
        var sel = $id('CmbJobOrderNo'), keep = sel.value;
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, 'JobOrderId')) + '" data-fin="' + esc(fmt3(toD(col(r, 'FinishOutPutPrcntObtained'))))
                + '">' + esc(col(r, 'JobOrderNo')) + '</option>';
        });
        sel.innerHTML = h;
        setValue(sel, keep);
    }
    function fillInvoices(rows) {
        var sel = $id('CmbInvoiceNoDetail'), keep = sel.value;
        var h = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, 'Id')) + '" data-cust="' + esc(col(r, 'CustomerName')) + '" data-fcy="' + esc(col(r, 'FcyCode'))
                + '" data-xr="' + esc(col(r, 'ExchangeRate')) + '" data-custid="' + esc(col(r, 'SupplierCustomerId'))
                + '" data-fcyid="' + esc(col(r, 'FcurrencyId')) + '">' + esc(col(r, 'InvoiceNo')) + '</option>';
        });
        sel.innerHTML = h;
        setValue(sel, keep);
    }

    /* ============================================================ InitializeComponentMethod / btnRefresh_Click:1283 */
    function applyFormats(d) {
        if (d.amountDecimals !== undefined) amtDec = toI(d.amountDecimals);
        if (d.rateDecimals !== undefined) rateDec = toI(d.rateDecimals);
        if (d.defaultHistoryDays !== undefined) defDays = toI(d.defaultHistoryDays);
    }
    function load() {
        return getJson(API + '/init').then(function (d) {
            rights = d.rights || rights;
            applyFormats(d);
            today = d.today || ymd(new Date());
            fyStart = d.financialYearStart || '';
            $id('btnsave').disabled = !rights.save;
            $id('btnUpdate').disabled = !rights.update;
            if (window.GridBar) {
                ['grdInputWorking', 'grdByProductRatio', 'grd', 'DataGridHistory'].forEach(function (id) {
                    window.GridBar.attach($id(id), null, {
                        canChooseFields: rights.fieldChooser !== false, canSaveLayout: rights.saveLayout !== false,
                        canPrint: !!rights.gridPrint, canExport: !!rights.gridExport,
                        canGroupCollapse: rights.groupCollapse !== false, canGroupExpand: rights.groupExpand !== false
                    });
                });
            }
            jobRows = d.jobOrders || []; invRows = d.invoices || [];
            fillJobOrders(jobRows); fillInvoices(invRows);
            $id('GRNfromdate').value = histDefaultFrom();
            $id('GRNToDate').value = today;
            renderAll();
        }).catch(function (e) { return msg(e.message || 'Error occurred during database call.'); });
    }
    function histDefaultFrom() {
        var d = new Date(today + 'T00:00:00');
        d.setDate(d.getDate() - (defDays > 0 ? defDays : 3));
        return ymd(d);
    }
    function refresh() {
        return busy('BtnRefresh', function () {
            return getJson(API + '/refresh').then(function (d) {
                jobRows = d.jobOrders || []; invRows = d.invoices || [];
                if (d.defaultHistoryDays !== undefined) defDays = toI(d.defaultHistoryDays);
                fillJobOrders(jobRows); fillInvoices(invRows);
            }, function (e) { return msg(e.message); });
        });
    }

    /* ============================================================ rendering */
    var INPUT_COLS = [
        ['RefDocumentType', 'RefDocumentType', 'l', 120], ['RefDocNo', 'RefDocNo', 'l', 110], ['ItemCode', 'ItemCode', 'l', 90],
        ['ItemName', 'ItemName', 'l', 220], ['Weight', 'Weight', 'w', 90], ['InputRate', 'InputRate', 'r', 90],
        ['Rate', 'Rate', 'e', 90], ['FinalAmount', 'FinalAmount', 'a', 110]
    ];
    var BP_COLS = [
        ['ItemCode', 'ItemCode', 'l', 90], ['ItemName', 'ItemName', 'l', 220], ['PercentOfTotalInput', 'PercentOfTotalInput', 'p', 130],
        ['Ratio', 'Ratio', 'r', 90], ['Rate', 'Rate', 'r', 90], ['Amount', 'Amount', 'a', 110]
    ];
    var DET_COLS = [
        ['InvoiceNo', 'InvoiceNo', 'l', 110], ['Customer', 'Customer', 'l', 200], ['CostAfterProcessing', 'CostAfterProcessing', 'a', 110],
        ['PackingCharges', 'PackingCharges', 'a', 110], ['FOBCharges', 'FOBCharges', 'a', 110], ['Tax', 'Tax', 'a', 110],
        ['Freight', 'Freight', 'a', 110], ['FinancialCharges', 'FinancialCharges', 'a', 110], ['TotalCost', 'TotalCost', 'a', 110],
        ['ExchangeRate', 'ExchangeRate', 'r', 90], ['FcyCode', 'FcyCode', 'l', 70], ['FcyAmount', 'FcyAmount', 'a', 110],
        ['CommissionAmount', 'CommissionAmount', 'a', 110], ['AmountAfterCommission', 'AmountAfterCommission', 'a', 110],
        ['ItemSaleAmount', 'ItemSaleAmount', 'a', 110], ['ContributionPerMTon', 'ContributionPerMTon', 'a', 110], ['Remarks', 'Remarks', 'ed', 200]
    ];
    function cell(kind, v) {
        if (kind === 'l') return '<td>' + esc(v) + '</td>';
        if (kind === 'w') return '<td class="num">' + fmtFixed(toD(v), 3) + '</td>';
        if (kind === 'p') return '<td class="num">' + fmtFixed(toD(v), 2) + '</td>';
        if (kind === 'r') return '<td class="num">' + fmtRate(toD(v)) + '</td>';
        return '<td class="num">' + fmtAmt(toD(v)) + '</td>';
    }
    function head(cols, extra) {
        return '<thead><tr>' + (extra || '') + cols.map(function (c) { return '<th style="min-width:' + c[3] + 'px">' + esc(c[0]) + '</th>'; }).join('') + '</tr></thead>';
    }
    function sumOf(rows, f) { var s = 0; rows.forEach(function (r) { s += toD(r[f]); }); return s; }

    function renderInput() {
        var h = head(INPUT_COLS);
        h += '<tbody>' + dtInput.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === icur ? ' class="cur"' : '') + '>' + INPUT_COLS.map(function (c) {
                if (c[2] === 'e') return '<td class="num" data-edit="Rate"><input class="edit" data-i="' + i + '" value="' + esc(fmtRate(toD(r.Rate))) + '"></td>';
                return cell(c[2], r[c[1]]);
            }).join('') + '</tr>';
        }).join('') + '</tbody>';
        $id('grdInputWorking').innerHTML = h;
    }
    function renderBp() {
        var h = head(BP_COLS);
        h += '<tbody>' + dtBp.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === bcur ? ' class="cur"' : '') + '>' + BP_COLS.map(function (c) { return cell(c[2], r[c[1]]); }).join('') + '</tr>';
        }).join('') + '</tbody>';
        $id('grdByProductRatio').innerHTML = h;
    }
    function renderDetail() {
        var h = head(DET_COLS, '<th style="min-width:40px">Edit</th><th style="min-width:20px">X</th>');
        h += '<tbody>' + dtdetail.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === dcur ? ' class="cur"' : '') + '>'
                + '<td><button type="button" class="cb" data-act="edit">Edit</button></td><td><button type="button" class="cb" data-act="del">X</button></td>'
                + DET_COLS.map(function (c) {
                    if (c[2] === 'ed') return '<td><input class="edit left" data-rem="' + i + '" value="' + esc(r.Remarks) + '"></td>';
                    return cell(c[2], r[c[1]]);
                }).join('') + '</tr>';
        }).join('') + '</tbody>';
        $id('grd').innerHTML = h;
    }
    function renderHistory() {
        var t = $id('DataGridHistory');
        if (!historyRows || !historyRows.length) { t.innerHTML = ''; return; }
        var H = [['JobOrderNo', 'JobOrderNo', 90], ['CostingDate', 'CostingDate', 85], ['Month', 'Month', 70],
            ['TotalCostBeforeProcessing', 'Total Cost (Before Processing)', 110], ['ProcessingCharges', 'ProcessingCharges', 100],
            ['TotalCostAfterProcessing', 'Total Cost (After Processing)', 115], ['RBP', 'RBP', 80], ['RAP', 'RAP', 80]];
        var h = '<thead><tr>' + H.map(function (c) { return '<th style="min-width:' + c[2] + 'px">' + esc(c[1]) + '</th>'; }).join('') + '</tr></thead><tbody>';
        historyRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === hcur ? ' class="cur"' : '') + '>';
            H.forEach(function (c) {
                var v = col(r, c[0]);
                if (c[0] === 'JobOrderNo') h += '<td><a class="lnk" data-link="1">' + esc(v) + '</a></td>';
                else if (c[0] === 'CostingDate') h += '<td>' + esc(v ? K.dMMMyy(v) : '') + '</td>';
                else if (c[0] === 'Month') h += '<td>' + esc(v) + '</td>';
                else h += '<td class="num">' + fmtAmt(toD(v)) + '</td>';
            });
            h += '</tr>';
        });
        t.innerHTML = h + '</tbody>';
    }
    function renderAll() { renderInput(); renderBp(); renderDetail(); renderHistory(); }

    /* ============================================================ CalculateHeaderValues:1374 */
    function calculateHeaderValues() {
        var totalFinal = sumOf(dtInput, 'FinalAmount'), totalWeight = sumOf(dtInput, 'Weight');
        var avg = (totalFinal > 0 && totalWeight > 0) ? totalFinal / totalWeight * 1000 : 0;
        setTxt('txtInputAvgRateHeader', fmtRate(avg));
        var outPct = read('txtFinishOutPutPrcntObtainedHeader');
        var toBe = outPct > 0 ? avg / outPct * 100 : 0;
        setTxt('txtInputToBeProcessedHeader', fmtAmt(toBe));
        var bpCost = sumOf(dtBp, 'Amount');
        setTxt('txtByProductCostHeader', fmtAmt(bpCost));
        var totalCost = toBe - bpCost;
        setTxt('txtTotalCostHeader', fmtAmt(totalCost));
        var proc = read('txtProcessingChargesHeader');
        var after = totalCost + proc;
        setTxt('txtTotalCostAfterProcessingPerMtonHeader', fmtAmt(after));
        setTxt('txtCostAfterProcessingDetail', fmtAmt(after));
        setTxt('txtRatePerMTonHeader', fmtAmt(after / 1000));
        calculateTotalCostDetail();
        updateInvoiceGridValues();
    }
    /* UpdateInvoiceGridValues:1405 - re-reads the rounded Total Cost After Processing text. */
    function updateInvoiceGridValues() {
        var after = read('txtTotalCostAfterProcessingPerMtonHeader');
        dtdetail.forEach(function (r) {
            r.CostAfterProcessing = after;
            var total = after + toD(r.PackingCharges) + toD(r.FOBCharges) + toD(r.Tax) + toD(r.Freight) + toD(r.FinancialCharges);
            r.TotalCost = total;
            var xr = toD(r.ExchangeRate);
            var fcy = xr > 0 ? total / xr : 0;
            r.FcyAmount = fcy;
            r.AmountAfterCommission = fcy - toD(r.CommissionAmount);
            r.ContributionPerMTon = toD(r.ItemSaleAmount) - r.AmountAfterCommission;
        });
        renderDetail();
    }
    /* CalculateTotalCostDetail:1436 */
    function calculateTotalCostDetail() {
        var total = read('txtCostAfterProcessingDetail') + read('txtPackingChargesDetail') + read('txtFobChargesDetail')
            + read('txtTaxDetail') + read('txtFreightDetail') + read('txtFinancialChargesDetail');
        setTxt('txtTotalCostDetail', fmtAmt(total));
        var xr = read('txtExchangeRateDetail');
        var fcy = xr > 0 ? total / xr : 0;
        setTxt('txtFcyAmountDetail', fmtAmt(fcy));
        var afterCom = fcy - read('txtCommissionAmountDetail');
        setTxt('txtamountAfterCommissionDetail', fmtAmt(afterCom));
        setTxt('txtContributionPerMTonDetail', fmtAmt(read('txtItemSaleAmountDetail') - afterCom));
    }

    /* ============================================================ btnShow_Click:554 */
    function btnShow() {
        var sel = $id('CmbJobOrderNo'), id = toI(sel.value);
        if (!activeRow(sel) || id === 0) { msg('Please Select JobOrder First'); focusCombo(sel); return Promise.resolve(); }
        return busy('btnShow', function () {
            return getJson(API + '/show?' + qs({ jobOrderId: id })).then(function (d) {
                jobOrderId = id;
                var fin = toD(d.finishOutPutPrcntObtained);
                if (d.exists) {
                    /* ReadById:1182 */
                    RecId = toI(d.recId);
                    $id('btnsave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden');
                    var h = d.header || {};
                    setTxt('txtInputAvgRateHeader', fmt3(toD(col(h, 'InputAvgRate'))));
                    setTxt('txtFinishOutPutPrcntObtainedHeader', fmt3(toD(col(h, 'FinishedOutputPercent'))));
                    setTxt('txtInputToBeProcessedHeader', fmt3(toD(col(h, 'InputToBeProcessed'))));
                    setTxt('txtByProductCostHeader', fmt3(toD(col(h, 'ByProductCost'))));
                    setTxt('txtTotalCostHeader', fmt3(toD(col(h, 'TotalCost'))));
                    setTxt('txtProcessingChargesHeader', fmt3(toD(col(h, 'ProcessingCharges'))));
                    setTxt('txtRatePerMTonHeader', fmt3(toD(col(h, 'RateInPMT'))));
                    dtdetail = (d.invoiceRows || []).map(invoiceRowFromDb);
                    dtInput = (d.inputRows || []).map(function (r) {
                        return { Id: toI(col(r, 'Id')), RefDocumentTypeId: toI(col(r, 'RefDocumentTypeId')), RefDocumentType: toS(col(r, 'RefDocumentType')),
                            RefDocId: toI(col(r, 'RefDocId')), RefDocNo: toS(col(r, 'RefDocNo')), ItemId: toI(col(r, 'ItemId')),
                            ItemCode: toS(col(r, 'ItemCode')), ItemName: toS(col(r, 'ItemName')), Qty: toD(col(r, 'Qty')), Weight: toD(col(r, 'Weight')),
                            InputAmount: toD(col(r, 'InputAmount')), InputRate: toD(col(r, 'InputRate')), Rate: toD(col(r, 'Rate')), FinalAmount: toD(col(r, 'FinalAmount')) };
                    });
                } else {
                    /* :568-572 */
                    RecId = 0;
                    setTxt('txtFinishOutPutPrcntObtainedHeader', fmt3(fin));
                    dtInput = (d.inputRows || []).map(function (r) {
                        var qty = toD(col(r, 'Qty')), wt = toD(col(r, 'Weight')), amt = toD(col(r, 'Amount'));
                        var rate = wt !== 0 ? amt / wt : 0;
                        return { Id: 0, RefDocumentTypeId: toI(col(r, 'RefDocumentTypeId')), RefDocumentType: toS(col(r, 'RefDocumentType')),
                            RefDocId: toI(col(r, 'RefDocNoId')), RefDocNo: toS(col(r, 'RefDocNo')), ItemId: toI(col(r, 'ItemId')),
                            ItemCode: toS(col(r, 'ItemCode')), ItemName: toS(col(r, 'ItemName')), Qty: qty, Weight: wt,
                            InputAmount: amt, InputRate: rate, Rate: rate, FinalAmount: amt };
                    });
                }
                /* ByProductBindByJobOrder:704 - always rebuilt from the summary (Id 0). */
                var totalW = sumOf(dtInput, 'Weight'), finPct = read('txtFinishOutPutPrcntObtainedHeader');
                var totalBpRatio = finPct > 0 ? 1000 / finPct : 0;
                dtBp = (d.byProductSummary || []).map(function (r) {
                    var rate = toD(col(r, 'RatePerKg')), ta = toD(col(r, 'TotalAmount')), tw = toD(col(r, 'Weight'));
                    var pct = totalW > 0 ? tw / totalW * 100 : 0, ratio = totalBpRatio * pct;
                    return { Id: 0, ItemId: toI(col(r, 'ItemId')), ItemCode: toS(col(r, 'ItemCode')), ItemName: toS(col(r, 'ItemName')),
                        PercentOfTotalInput: pct, Ratio: ratio, Rate: rate, ByProductAmount: ta, Amount: rate * ratio };
                });
                icur = bcur = dcur = -1;
                renderInput(); renderBp();
                calculateHeaderValues();
                setComboEnabled($id('CmbJobOrderNo'), false);
                $id('btnShow').disabled = true;
            }, function (e) { return msg(e.message); });
        });
    }
    function invoiceRowFromDb(r) {
        return { Id: toI(col(r, 'Id')), InvoiceId: toI(colAny(r, ['ExImInvoiceId', 'InvoiceId'])), InvoiceNo: toS(col(r, 'InvoiceNo')),
            SupplierCustomerId: toI(col(r, 'SupplierCustomerId')), Customer: toS(colAny(r, ['CustomerName', 'Customer'])),
            CostAfterProcessing: toD(col(r, 'CostAfterProcessing')), PackingCharges: toD(col(r, 'PackingCharges')),
            FOBCharges: toD(colAny(r, ['FOBCharges', 'FobCharges'])), Tax: toD(col(r, 'Tax')), Freight: toD(col(r, 'Freight')),
            FinancialCharges: toD(col(r, 'FinancialCharges')), TotalCost: toD(col(r, 'TotalCost')), ExchangeRate: toD(col(r, 'ExchangeRate')),
            FcyCodeId: toI(col(r, 'FcyCodeId')), FcyCode: toS(col(r, 'FcyCode')), FcyAmount: toD(col(r, 'FcyAmount')),
            CommissionAmount: toD(col(r, 'CommissionAmount')), AmountAfterCommission: toD(colAny(r, ['AmountAfterComm', 'AmountAfterCommission'])),
            ItemSaleAmount: toD(col(r, 'ItemSaleAmount')), ContributionPerMTon: toD(col(r, 'ContributionPerMTon')),
            Remarks: toS(colAny(r, ['RemarksDetail', 'Remarks'])) };
    }

    /* ============================================================ grdInputWorking_CellUpdated:674 */
    function inputRateEdited(i, text) {
        var row = dtInput[i];
        if (!row) return;
        var rate = toD(text);
        if (rate < 0) {
            row.Rate = 0;
            renderInput();
            msg('Rate cannot be negative.').then(function () { /* the desktop returns here: header is not recalculated */ });
            return;
        }
        row.Rate = rate;
        row.FinalAmount = rate > 0 ? rate * toD(row.Weight) : toD(row.InputRate) * toD(row.Weight);
        renderInput();
        calculateHeaderValues();
    }

    /* ============================================================ invoice combo Leave:455 */
    function invoiceLeave() {
        var sel = $id('CmbInvoiceNoDetail'), o = activeRow(sel), invoiceId = toI(sel.value);
        if (!o || invoiceId <= 0) {
            setPick('CmbCustomerDetail', 0, ''); setPick('CmbFcyCodeDetail', 0, '');
            setTxt('txtExchangeRateDetail', '0');
            return Promise.resolve();
        }
        setPick('CmbCustomerDetail', toI(o.getAttribute('data-custid')), o.getAttribute('data-cust'));
        setPick('CmbFcyCodeDetail', toI(o.getAttribute('data-fcyid')), o.getAttribute('data-fcy'));
        setTxt('txtExchangeRateDetail', String(toD(o.getAttribute('data-xr'))));
        calculateTotalCostDetail();
        var saved = dtdetail.some(function (r) { return r.InvoiceId === invoiceId && r.Id > 0; });
        if (saved && !DataReset) return Promise.resolve();
        return getJson(API + '/invoice-defaults?' + qs({ jobOrderId: toI($id('CmbJobOrderNo').value), invoiceId: invoiceId })).then(function (d) {
            var r = d && d.row;
            if (r) {
                setTxt('txtPackingChargesDetail', toS(col(r, 'PackingCharges')));
                setTxt('txtFobChargesDetail', toS(colAny(r, ['FOBCharges', 'FobCharges'])));
                setTxt('txtTaxDetail', toS(col(r, 'Tax')));
                setTxt('txtFreightDetail', toS(col(r, 'Freight')));
                setTxt('txtFinancialChargesDetail', toS(col(r, 'FinancialCharges')));
                setTxt('txtCommissionAmountDetail', toS(col(r, 'CommissionAmount')));
                setTxt('txtItemSaleAmountDetail', toS(col(r, 'ItemSaleAmount')));
            } else {
                DataReset = true;
                ['txtPackingChargesDetail', 'txtFobChargesDetail', 'txtTaxDetail', 'txtFreightDetail', 'txtFinancialChargesDetail',
                    'txtCommissionAmountDetail', 'txtItemSaleAmountDetail'].forEach(function (id) { setTxt(id, '0'); });
            }
            calculateTotalCostDetail();
        }, function (e) { return msg(e.message); });
    }
    function setPick(id, idv, text) { var el = $id(id); el.setAttribute('data-id', String(idv || 0)); el.value = text || ''; }

    /* ============================================================ detail entry: DetailFormValidation:535 */
    function detailValidation() {
        var checks = [
            ['Invoice No', 'INT', toI($id('CmbInvoiceNoDetail').value)],
            ['Customer', 'INT', toI($id('CmbCustomerDetail').getAttribute('data-id'))],
            ['Cost After Processing', 'D', read('txtCostAfterProcessingDetail')],
            ['Total Cost', 'D', read('txtTotalCostDetail')],
            ['Exchange Rate', 'D', read('txtExchangeRateDetail')],
            ['FCY Code', 'INT', toI($id('CmbFcyCodeDetail').getAttribute('data-id'))],
            ['FCY Amount', 'D', read('txtFcyAmountDetail')],
            ['Item Sale Amount', 'D', read('txtItemSaleAmountDetail')],
            ['Contribution Per M.Ton', 'D', read('txtContributionPerMTonDetail')]
        ];
        for (var i = 0; i < checks.length; i++) {
            var c = checks[i];
            if (c[2] === 0) {
                return msg(c[0] + (c[1] === 'INT' ? ' field is required' : ' must be a non-zero number')).then(function () { return false; });
            }
        }
        return Promise.resolve(true);
    }
    function detailFromForm(id) {
        return { Id: id || 0, InvoiceId: toI($id('CmbInvoiceNoDetail').value), InvoiceNo: comboText($id('CmbInvoiceNoDetail')),
            SupplierCustomerId: toI($id('CmbCustomerDetail').getAttribute('data-id')), Customer: $id('CmbCustomerDetail').value,
            CostAfterProcessing: read('txtCostAfterProcessingDetail'), PackingCharges: read('txtPackingChargesDetail'),
            FOBCharges: read('txtFobChargesDetail'), Tax: read('txtTaxDetail'), Freight: read('txtFreightDetail'),
            FinancialCharges: read('txtFinancialChargesDetail'), TotalCost: read('txtTotalCostDetail'), ExchangeRate: read('txtExchangeRateDetail'),
            FcyCodeId: toI($id('CmbFcyCodeDetail').getAttribute('data-id')), FcyCode: $id('CmbFcyCodeDetail').value,
            FcyAmount: read('txtFcyAmountDetail'), CommissionAmount: read('txtCommissionAmountDetail'),
            AmountAfterCommission: read('txtamountAfterCommissionDetail'), ItemSaleAmount: read('txtItemSaleAmountDetail'),
            ContributionPerMTon: read('txtContributionPerMTonDetail'), Remarks: $id('txtRemarksDetail').value };
    }
    function btnAddDetail() {
        return detailValidation().then(function (ok) {
            if (!ok) return;
            dtdetail.push(detailFromForm(0));
            renderDetail();
            resetDetail();
        });
    }
    function btnUpdateDetail() {
        return detailValidation().then(function (ok) {
            if (!ok) return;
            var keep = dtdetail[updateDetailIndex];
            if (!keep) return;
            /* desktop assigns every field except Id */
            var n = detailFromForm(keep.Id);
            dtdetail[updateDetailIndex] = n;
            renderDetail();
            resetDetail();
        });
    }
    /* grd_DoubleClick:773 / Edit button */
    function editDetail(i) {
        var r = dtdetail[i];
        if (!r) return Promise.resolve();
        updateDetailIndex = i;
        setValue($id('CmbInvoiceNoDetail'), r.InvoiceId);
        return invoiceLeave().then(function () {
            setPick('CmbCustomerDetail', r.SupplierCustomerId, r.Customer);
            setTxt('txtCostAfterProcessingDetail', toS(r.CostAfterProcessing));
            setTxt('txtPackingChargesDetail', toS(r.PackingCharges));
            setTxt('txtFobChargesDetail', toS(r.FOBCharges));
            setTxt('txtTaxDetail', toS(r.Tax));
            setTxt('txtFreightDetail', toS(r.Freight));
            setTxt('txtFinancialChargesDetail', toS(r.FinancialCharges));
            setTxt('txtTotalCostDetail', toS(r.TotalCost));
            setTxt('txtExchangeRateDetail', toS(r.ExchangeRate));
            setPick('CmbFcyCodeDetail', r.FcyCodeId, r.FcyCode);
            setTxt('txtFcyAmountDetail', toS(r.FcyAmount));
            setTxt('txtCommissionAmountDetail', toS(r.CommissionAmount));
            setTxt('txtamountAfterCommissionDetail', toS(r.AmountAfterCommission));
            setTxt('txtItemSaleAmountDetail', toS(r.ItemSaleAmount));
            setTxt('txtContributionPerMTonDetail', toS(r.ContributionPerMTon));
            setTxt('txtRemarksDetail', toS(r.Remarks));
            $id('btnAddDetail').classList.add('is-hidden');
            $id('btnUpdateDetail').classList.remove('is-hidden');
            $id('btnCancelDetail').classList.remove('is-hidden');
            focusCombo($id('CmbInvoiceNoDetail'));
        });
    }
    /* DeleteDetailRow:915 */
    function deleteDetail(i) {
        var r = dtdetail[i];
        if (!r) return Promise.resolve();
        if (updateDetailIndex !== -1) return msg('Reset Detail First');
        function remove() { dtdetail.splice(i, 1); dcur = -1; renderDetail(); }
        if (r.Id > 0) {
            return ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                lstDetailRemove.push(r);
                remove();
            });
        }
        remove();
        return Promise.resolve();
    }
    /* ResetDetail:1337 */
    function resetDetail() {
        updateDetailIndex = -1;
        var sel = $id('CmbInvoiceNoDetail');
        setValue(sel, '');
        var w = sel.closest('.dtcombo-wrap'), inp = w ? w.querySelector('input') : null;
        if (inp) inp.value = '';
        setPick('CmbCustomerDetail', 0, ''); setPick('CmbFcyCodeDetail', 0, '');
        ['txtPackingChargesDetail', 'txtFobChargesDetail', 'txtTaxDetail', 'txtFreightDetail', 'txtFinancialChargesDetail',
            'txtTotalCostDetail', 'txtExchangeRateDetail', 'txtFcyAmountDetail', 'txtCommissionAmountDetail',
            'txtamountAfterCommissionDetail', 'txtItemSaleAmountDetail', 'txtContributionPerMTonDetail'].forEach(function (id) { setTxt(id, ''); });
        $id('btnAddDetail').classList.remove('is-hidden');
        $id('btnUpdateDetail').classList.add('is-hidden');
        $id('btnCancelDetail').classList.add('is-hidden');
        focusCombo(sel);
    }

    /* ============================================================ Reset:1292 (btnnew) */
    function resetForm() {
        lstDetailRemove = [];
        setComboEnabled($id('CmbJobOrderNo'), true);
        $id('btnShow').disabled = false;
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        DataReset = false; RecId = 0; jobOrderId = 0;
        var sel = $id('CmbJobOrderNo');
        setValue(sel, '');
        var w = sel.closest('.dtcombo-wrap'), inp = w ? w.querySelector('input') : null;
        if (inp) inp.value = '';
        resetDetail();
        setTxt('txtRemarksDetail', '');
        dtdetail = []; dtInput = []; dtBp = []; icur = bcur = dcur = -1;
        ['txtInputAvgRateHeader', 'txtFinishOutPutPrcntObtainedHeader', 'txtInputToBeProcessedHeader', 'txtByProductCostHeader',
            'txtTotalCostHeader', 'txtProcessingChargesHeader', 'txtTotalCostAfterProcessingPerMtonHeader', 'txtCostAfterProcessingDetail',
            'txtRatePerMTonHeader'].forEach(function (id) { setTxt(id, ''); });
        showInnerTab(0);
        renderAll();
        focusCombo(sel);
    }

    /* ============================================================ Insert():957 */
    function save() {
        if (RecId > 0 && !rights.update) return msg('You do not have the Update right');
        if (RecId === 0 && !rights.save) return msg('You do not have the Save right');
        function nz(id, name) { if (read(id) === 0) return msg(name + ' must be a non-zero number').then(function () { return false; }); return Promise.resolve(true); }
        if (!dtInput.length) return msg('Grid Input Record Not Found', 'Error!', 'error');
        if (toI($id('CmbJobOrderNo').value) === 0) return msg('JobOrder No field is required', 'Error!', 'error');
        var chain = Promise.resolve(true);
        [['txtInputAvgRateHeader', 'InputAvg Rate'], ['txtFinishOutPutPrcntObtainedHeader', 'Finish OutPut % Obtained'],
            ['txtInputToBeProcessedHeader', 'Input To Be Processed Obtained'], ['txtTotalCostHeader', 'Total Cost'],
            ['txtRatePerMTonHeader', 'Rate In MPR']].forEach(function (c) {
            chain = chain.then(function (ok) { return ok ? nz(c[0], c[1]) : false; });
        });
        return chain.then(function (ok) {
            if (!ok) return;
            return ask(RecId === 0 ? 'Are you sure to Save' : 'Are you sure to Update', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    recId: RecId, jobOrderId: toI($id('CmbJobOrderNo').value),
                    header: { inputAvgRate: read('txtInputAvgRateHeader'), finishedOutputPercent: read('txtFinishOutPutPrcntObtainedHeader'),
                        inputToBeProcessed: read('txtInputToBeProcessedHeader'), byProductCost: read('txtByProductCostHeader'),
                        totalCost: read('txtTotalCostHeader'), processingCharges: read('txtProcessingChargesHeader'),
                        rateInPMT: read('txtRatePerMTonHeader') },
                    inputRows: dtInput.map(function (r) {
                        return { id: RecId !== 0 ? r.Id : 0, refDocumentTypeId: r.RefDocumentTypeId, refDocId: r.RefDocId, itemId: r.ItemId,
                            qty: r.Qty, weight: r.Weight, inputAmount: r.InputAmount, inputRate: r.InputRate, rate: r.Rate, finalAmount: r.FinalAmount };
                    }),
                    byProductRows: dtBp.map(function (r) {
                        return { id: RecId !== 0 ? r.Id : 0, itemId: r.ItemId, percentOfTotalInput: r.PercentOfTotalInput, ratio: r.Ratio,
                            rate: r.Rate, byProductAmount: r.ByProductAmount, amount: r.Amount };
                    }),
                    invoiceRows: dtdetail.map(invoicePayload),
                    removedInvoiceRows: RecId > 0 ? lstDetailRemove.map(invoicePayload) : []
                };
                return busy(RecId > 0 ? 'btnUpdate' : 'btnsave', function () {
                    return postJson(API + '/save', body).then(function (d) {
                        var wasNew = RecId === 0;
                        return msg(wasNew ? 'Record Save Successfully' : 'Record Update Successfully').then(function () { resetForm(); });
                    }, function (e) { return msg(e.message, 'Error!', 'error'); });
                });
            });
        });
    }
    function invoicePayload(r) {
        return { id: RecId !== 0 ? r.Id : 0, invoiceId: r.InvoiceId, supplierCustomerId: r.SupplierCustomerId,
            costAfterProcessing: r.CostAfterProcessing, packingCharges: r.PackingCharges, fobCharges: r.FOBCharges, tax: r.Tax,
            freight: r.Freight, financialCharges: r.FinancialCharges, totalCost: r.TotalCost, exchangeRate: r.ExchangeRate,
            fcyCodeId: r.FcyCodeId, fcyAmount: r.FcyAmount, commissionAmount: r.CommissionAmount,
            amountAfterCommission: r.AmountAfterCommission, itemSaleAmount: r.ItemSaleAmount,
            contributionPerMTon: r.ContributionPerMTon, remarks: r.Remarks };
    }
    function btnUpdate() {
        if (RecId === 0) return msg('RecId not Found');
        return save();
    }

    /* ============================================================ OpenPrint:1517 */
    function openPrint(key) {
        var sel = $id('CmbJobOrderNo'), id = toI(sel.value);
        if (!activeRow(sel) || id === 0) { msg('Please Select JobOrder First'); focusCombo(sel); return Promise.resolve(); }
        var w = window.CrystalPrint.reserve();
        return busy('BtnPrintDropDown', function () {
            return getJson(API + '/print-rows?' + qs({ jobOrderId: id })).then(function (d) {
                if (!d || !(d.rows > 0)) { window.CrystalPrint.release(w); return msg('Record Not Found For DisPlay'); }
                return window.CrystalPrint.open(key, { jobOrderId: id }, null, w);
            }, function (e) { window.CrystalPrint.release(w); return msg(e.message); });
        });
    }

    /* ============================================================ BtnByProductSettlementPopUp_Click:1830 */
    function byProductPopup() {
        var sel = $id('CmbJobOrderNo'), id = toI(sel.value);
        if (id === 0) { return msg('Please Select JobOrder First'); }
        return getJson(API + '/by-product-item-wise?' + qs({ jobOrderId: id })).then(function (rows) {
            bpPopupRows = (rows || []).map(function (r) { return { raw: r, rate: toD(colAny(r, ['RatePerKg', 'Rate'])) }; });
            renderBpPopup();
            $id('dlgBp').classList.remove('is-hidden');
        }, function (e) { return msg(e.message); });
    }
    function renderBpPopup() {
        var keys = [];
        if (bpPopupRows.length) {
            Object.keys(bpPopupRows[0].raw).forEach(function (k) {
                if (/^(id|itemid|jobOrderid|companyid)$/i.test(k)) return;
                keys.push(k);
            });
        }
        var h = '<thead><tr>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr></thead><tbody>';
        bpPopupRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">' + keys.map(function (k) {
                if (/^(rateperkg|rate)$/i.test(k)) return '<td class="num"><input class="edit" data-bp="' + i + '" value="' + esc(String(r.rate)) + '"></td>';
                var v = r.raw[k];
                return typeof v === 'number' ? '<td class="num">' + esc(fmtFixed(v, 3)) + '</td>' : '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        $id('grdBpPopup').innerHTML = h + '</tbody>';
    }
    function bpPopupOk() {
        var rows = bpPopupRows.map(function (r) {
            return { itemId: toI(colAny(r.raw, ['ItemId'])), rate: r.rate };
        });
        return busy('btnBpOk', function () {
            return postJson(API + '/by-product-item-wise', { jobOrderId: toI($id('CmbJobOrderNo').value), rows: rows }).then(function () {
                $id('dlgBp').classList.add('is-hidden');
                /* the desktop runs btnShow_Click again */
                return reshow();
            }, function (e) { return msg(e.message); });
        });
    }
    function reshow() {
        var sel = $id('CmbJobOrderNo');
        setComboEnabled(sel, true); $id('btnShow').disabled = false;
        return btnShow();
    }

    /* ============================================================ History (frmProductionCostingReport) */
    function dateTypeChanged() {
        var v = toI($id('cmbperemeter').value), t = new Date(today + 'T00:00:00');
        var f = $id('GRNfromdate'), to = $id('GRNToDate');
        if (v === 1) f.value = ymd(t);
        else if (v === 2) { var d = new Date(t); d.setDate(d.getDate() - 7); f.value = ymd(d); }
        else if (v === 3) { f.value = ymd(new Date(t.getFullYear(), t.getMonth(), 1)); to.value = ymd(t); }
        else if (v === 4) { f.value = ymd(new Date(t.getFullYear(), 0, 1)); to.value = ymd(t); }
        else if (v === 5) { if (fyStart) f.value = String(fyStart).substring(0, 10); }
    }
    function historyShow() {
        var f = $id('GRNfromdate').value, t = $id('GRNToDate').value;
        return busy('btnshow', function () {
            return getJson(API + '/history?' + qs({ fromDate: f, toDate: t })).then(function (rows) {
                historyRows = rows || []; histFrom = f; histTo = t; hcur = -1;
                renderHistory();
                say(historyRows.length + ' record(s)');
            }, function (e) { return msg(e.message); });
        });
    }
    function openFromHistory(i) {
        var r = historyRows && historyRows[i];
        if (!r) return;
        var jid = toI(col(r, 'ProductionJobOrderId'));
        showTab(0);
        resetForm();
        setValue($id('CmbJobOrderNo'), jid);
        return btnShow();
    }
    function historyPrint() {
        if (!historyRows || !historyRows.length) return msg('Not Record Found For Display');
        var w = window.CrystalPrint.reserve();
        return Promise.resolve(window.CrystalPrint.open('w3c-921h', { fromDate: histFrom, toDate: histTo }, null, w));
    }

    /* ============================================================ tabs / keys */
    function showTab(n) {
        $id('tabForm').classList.toggle('is-hidden', n !== 0);
        $id('tabHistory').classList.toggle('is-hidden', n !== 1);
        Array.prototype.forEach.call($id('tabControlMain').querySelectorAll('button'), function (b) {
            b.classList.toggle('on', toI(b.getAttribute('data-tab')) === n);
        });
    }
    function showInnerTab(n) {
        $id('tabInputWorking').classList.toggle('is-hidden', n !== 0);
        $id('tabByProductRatio').classList.toggle('is-hidden', n !== 1);
        Array.prototype.forEach.call(document.querySelectorAll('.itabs button'), function (b) {
            b.classList.toggle('on', toI(b.getAttribute('data-itab')) === n);
        });
    }
    function innerTabIndex() { return $id('tabInputWorking').classList.contains('is-hidden') ? 1 : 0; }
    function shortcutKeys() {
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on JobOrder'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowRight', 'For Moving In Detail Grids'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On First Column in Detail for Entry'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $id('tblKeys').innerHTML = '<tr><th>Key</th><th>Function</th></tr>' + rows.map(function (r) {
            return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>';
        }).join('');
        $id('dlgKeys').classList.remove('is-hidden');
    }

    function wire() {
        $id('toolStripButton1').addEventListener('click', resetForm);
        $id('BtnRefresh').addEventListener('click', refresh);
        $id('btnsave').addEventListener('click', save);
        $id('btnUpdate').addEventListener('click', btnUpdate);
        $id('btnShow').addEventListener('click', btnShow);
        $id('btnShortCutKey').addEventListener('click', shortcutKeys);
        $id('BtnPrintDropDown').addEventListener('click', function (e) { e.stopPropagation(); $id('printMenu').classList.toggle('is-hidden'); });
        document.addEventListener('click', function () { $id('printMenu').classList.add('is-hidden'); });
        Array.prototype.forEach.call($id('printMenu').querySelectorAll('button'), function (b) {
            b.addEventListener('click', function () { $id('printMenu').classList.add('is-hidden'); openPrint(b.getAttribute('data-rpt')); });
        });
        Array.prototype.forEach.call(document.querySelectorAll('.itabs button'), function (b) {
            b.addEventListener('click', function () { showInnerTab(toI(b.getAttribute('data-itab'))); });
        });
        Array.prototype.forEach.call($id('tabControlMain').querySelectorAll('button'), function (b) {
            b.addEventListener('click', function () { showTab(toI(b.getAttribute('data-tab'))); });
        });
        $id('BtnByProductSettlementPopUp').addEventListener('click', byProductPopup);
        $id('btnBpOk').addEventListener('click', bpPopupOk);
        Array.prototype.forEach.call(document.querySelectorAll('#dlgBp [data-close], #dlgKeys [data-close]'), function (b) {
            b.addEventListener('click', function () { b.closest('.pa-modal').classList.add('is-hidden'); });
        });
        $id('grdBpPopup').addEventListener('input', function (e) {
            var i = e.target.getAttribute && e.target.getAttribute('data-bp');
            if (i !== null && i !== undefined) bpPopupRows[toI(i)].rate = toD(e.target.value);
        });

        /* header: txtProcessingChargesHeader_TextChanged */
        $id('txtProcessingChargesHeader').addEventListener('input', calculateHeaderValues);
        /* detail text boxes: *_TextChanged -> CalculateTotalCostDetail */
        ['txtCostAfterProcessingDetail', 'txtPackingChargesDetail', 'txtFobChargesDetail', 'txtTaxDetail', 'txtFreightDetail',
            'txtFinancialChargesDetail', 'txtExchangeRateDetail', 'txtCommissionAmountDetail', 'txtItemSaleAmountDetail'].forEach(function (id) {
            $id(id).addEventListener('input', calculateTotalCostDetail);
        });
        onLeave($id('CmbInvoiceNoDetail'), invoiceLeave);
        $id('CmbInvoiceNoDetail').addEventListener('change', invoiceLeave);
        $id('btnAddDetail').addEventListener('click', btnAddDetail);
        $id('btnUpdateDetail').addEventListener('click', btnUpdateDetail);
        $id('btnCancelDetail').addEventListener('click', resetDetail);

        /* Input grid: Rate cell */
        $id('grdInputWorking').addEventListener('change', function (e) {
            var i = e.target.getAttribute && e.target.getAttribute('data-i');
            if (i !== null && i !== undefined && e.target.classList.contains('edit')) inputRateEdited(toI(i), e.target.value);
        });
        $id('grdInputWorking').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (tr) { icur = toI(tr.getAttribute('data-i')); }
        });
        $id('grdByProductRatio').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (tr) { bcur = toI(tr.getAttribute('data-i')); }
        });
        /* Detail grid: Edit / X buttons, double-click, Remarks editable */
        $id('grd').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            var i = toI(tr.getAttribute('data-i')); dcur = i;
            var b = e.target.closest('button[data-act]');
            if (b) { if (b.getAttribute('data-act') === 'edit') editDetail(i); else deleteDetail(i); }
        });
        $id('grd').addEventListener('dblclick', function (e) {
            if (e.target.closest('input,button')) return;
            var tr = e.target.closest('tbody tr'); if (tr) editDetail(toI(tr.getAttribute('data-i')));
        });
        $id('grd').addEventListener('change', function (e) {
            var i = e.target.getAttribute && e.target.getAttribute('data-rem');
            if (i !== null && i !== undefined && dtdetail[toI(i)]) dtdetail[toI(i)].Remarks = e.target.value;
        });

        /* History */
        $id('cmbperemeter').addEventListener('change', dateTypeChanged);
        $id('btnshow').addEventListener('click', historyShow);
        $id('btnHNew').addEventListener('click', function () { historyRows = null; hcur = -1; renderHistory(); });
        $id('btnHRefresh').addEventListener('click', function () { refresh(); });
        $id('BtnPrintHistory').addEventListener('click', historyPrint);
        $id('DataGridHistory').addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr'); if (!tr) return;
            var i = toI(tr.getAttribute('data-i')); hcur = i;
            if (e.target.closest('a[data-link]')) openFromHistory(i);
        });
        $id('DataGridHistory').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr'); if (tr) openFromHistory(toI(tr.getAttribute('data-i')));
        });

        /* Keys: Ctrl+S / U / E / R / N / P / F5 / T / Alt */
        document.addEventListener('keydown', function (e) {
            if (modalOpen()) return;
            var k = e.key;
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcutKeys(); return; }
            if (e.ctrlKey && (k === 's' || k === 'S') && !$id('btnsave').classList.contains('is-hidden')) { e.preventDefault(); save(); }
            else if (e.ctrlKey && (k === 'u' || k === 'U') && !$id('btnUpdate').classList.contains('is-hidden') && RecId > 0) { e.preventDefault(); btnUpdate(); }
            else if (e.ctrlKey && (k === 'e' || k === 'E')) { e.preventDefault(); if (K && K.close) K.close(); else window.location.href = '/production'; }
            else if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); resetForm(); }
            else if (e.ctrlKey && (k === 'p' || k === 'P' || k === '1')) { e.preventDefault(); openPrint('w3c-921'); }
            else if (e.ctrlKey && k === 'F5') { e.preventDefault(); focusCombo($id('CmbJobOrderNo')); }
            else if (e.ctrlKey && (k === 't' || k === 'T')) { e.preventDefault(); showInnerTab(innerTabIndex() === 0 ? 1 : 0); }
            else if (e.ctrlKey && k === 'ArrowUp') { e.preventDefault(); focusCombo($id('CmbInvoiceNoDetail')); }
            else if (e.ctrlKey && k === 'ArrowDown') { e.preventDefault(); $id('wrapGrd').focus(); }
            else if (e.ctrlKey && k === 'Enter' && document.activeElement === $id('wrapGrd') && dcur >= 0) { e.preventDefault(); editDetail(dcur); }
            else if (e.ctrlKey && k === 'Delete' && document.activeElement === $id('wrapGrd') && dcur >= 0) { e.preventDefault(); deleteDetail(dcur); }
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        wire();
        load();
    });
})();
