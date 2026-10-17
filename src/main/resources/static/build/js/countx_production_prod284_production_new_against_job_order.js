/* =====================================================================================================
 * Screen 284 "ProductionNew Against (Pre Costing Job Order)" - Architecture.WinApp.Production/FoodProductionWithValuesNew.cs
 * Page script: /production/production-new-against-job-order (API /production/production-new-against-job-order/api).
 *
 * One page = the seven tabs of tabControl1. Every handler below names the desktop method it reproduces
 * (line numbers of FoodProductionWithValuesNew.cs). MessageBox texts are reproduced verbatim.
 * ===================================================================================================== */
(function () {
    'use strict';
    var C = window.StoreCommon;
    var $id = C.$id, esc = C.esc, ci = C.ci, num = C.num;
    var API = '/production/production-new-against-job-order/api';
    var DOC = { INPUT: 80, OUTPUT: 112, PM: 111, OH: 110, SETTLEMENT: 142 };

    var st = {
        init: null, rights: {}, amountDec: 0, rateDec: 2,
        jobOrders: [], jobOrdersAll: [], warehouses: [], warehousesAll: [], jobLots: [], cropYears: [], packingTypes: [],
        uomSchedules: [], uomStatic: [], departments: [], differenceAccounts: [],
        wagesStatus: false, loaderStatus: false
    };
    var tabName = 'InPut';

    // ------------------------------------------------------------------------------------------- formats
    function thousands(s) { var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    /* "#,##0.##" */
    function f2(v) { v = num(v); var s = (Math.round(v * 100) / 100).toFixed(2).replace(/\.?0+$/, ''); return thousands(s); }
    /* "#,#.###" */
    function f3(v) { v = num(v); if (v === 0) return ''; var s = (Math.round(v * 1000) / 1000).toFixed(3).replace(/\.?0+$/, ''); return thousands(s); }
    /* "#,#" - .NET prints an EMPTY string for zero */
    function fi(v) { v = Math.round(num(v)); return v === 0 ? '' : thousands(String(v)); }
    /* "0,0" - at least one digit */
    function f00(v) { return thousands(String(Math.round(num(v)))); }
    /* "#,0.##" */
    function fp(v) { v = num(v); var s = (Math.round(v * 100) / 100).toFixed(2).replace(/\.?0+$/, ''); return thousands(s === '' ? '0' : s); }
    /* clsGlobalVariables.stringFormatsingle / DecimalRateFormate */
    function fAmt(v) { return thousands(num(v).toFixed(st.amountDec)); }
    function fRate(v) { return thousands(num(v).toFixed(st.rateDec)); }
    function f2d(v) { return thousands(num(v).toFixed(2)); }
    function fmtPlain(v) { return v == null ? '' : String(v); }
    function roundAway(v, d) { var f = Math.pow(10, d); var x = Math.abs(v) * f; var r = Math.floor(x + 0.5) / f; return v < 0 ? -r : r; }
    /* Math.Round(x, 0, MidpointRounding.AwayFromZero).ToString("0,0") */
    function r0(v) { return f00(roundAway(num(v), 0)); }
    /* Conversion.ToInt(double) - Convert.ToInt32 rounds half to even */
    function netInt(v) { v = num(v); var r = Math.round(v); if (Math.abs(v % 1) === 0.5 && r % 2 !== 0) r -= 1; return r; }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dMMMyy(v) { var d = parseDate(v); return d ? pad(d.getDate()) + '-' + MON[d.getMonth()] + '-' + String(d.getFullYear()).slice(-2) : ''; }
    function dMMMyyyy(v) { var d = parseDate(v); return d ? pad(d.getDate()) + '-' + MON[d.getMonth()] + '-' + d.getFullYear() : ''; }
    function dShort(v) { var d = parseDate(v); return d ? pad(d.getMonth() + 1) + '/' + pad(d.getDate()) + '/' + d.getFullYear() : ''; }
    function dTime(v) { var d = parseDate(v); return d ? dMMMyyyy(d) + ' ' + pad(d.getHours()) + ':' + pad(d.getMinutes()) : ''; }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function parseDate(v) {
        if (!v) return null;
        if (v instanceof Date) return v;
        var s = String(v);
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(s);
        if (m) return new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0));
        var d = new Date(s); return isNaN(d.getTime()) ? null : d;
    }
    function isoOf(v) { var d = parseDate(v); return d ? d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()) : ''; }
    function today() { return isoOf(new Date()); }
    function rateRight() { return !!st.rights.rateAndAmount; }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function show(id, on) { var el = $id(id); if (el) el.classList.toggle('is-hidden', !on); }
    function visible(id) { var el = $id(id); return !!el && !el.classList.contains('is-hidden') && !el.disabled; }
    function sel(id, rows, vk, tk) { C.fillSelect($id(id), rows || [], vk, tk, ''); }
    function selText(id) { var e = $id(id); return e && e.selectedIndex >= 0 && e.value !== '' ? e.options[e.selectedIndex].text : ''; }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = v == null ? '' : v; }
    function intOf(id) { return C.intOf(val(id)); }

    // ------------------------------------------------------------------------------------------- grid
    /**
     * A GridEX look-alike: cols [{key, cap, hide, num, fmt, total:'sum'|'avg', edit:'num'|'text'|'select', opts, ok, tk,
     * link(row), pick(row)}], opts {buttons:[{text, cls, click(row,i)}], onChange(row,key,i), onSelect(row,i),
     * groupBy, selectable}. Editable cells write back to the row object and call onChange; the grid re-renders.
     */
    function renderGrid(tableId, cols, rows, o) {
        o = o || {};
        var t = $id(tableId), thead = t.tHead, tbody = t.tBodies[0], tfoot = t.tFoot;
        var vis = cols.filter(function (c) { return !c.hide; });
        var btns = o.buttons || [];
        var h = '<tr>' + btns.map(function (b) { return '<th style="width:35px">' + esc(b.text) + '</th>'; }).join('')
              + vis.map(function (c) { return '<th' + (c.width ? ' style="min-width:' + c.width + 'px"' : '') + '>' + esc(c.cap || c.key) + '</th>'; }).join('') + '</tr>';
        thead.innerHTML = h;
        var body = '', lastGroup = null;
        (rows || []).forEach(function (r, i) {
            if (o.groupBy && r[o.groupBy] !== lastGroup) {
                lastGroup = r[o.groupBy];
                body += '<tr class="grp"><td colspan="' + (vis.length + btns.length) + '">' + esc(o.groupBy) + ': ' + esc(lastGroup) + '</td></tr>';
            }
            body += '<tr data-i="' + i + '"' + (r.__sel ? ' class="is-selected"' : '') + '>';
            btns.forEach(function (b, bi) { body += '<td class="' + b.cls + '" data-b="' + bi + '">' + esc(b.text) + '</td>'; });
            vis.forEach(function (c) {
                var v = r[c.key];
                if (c.edit === 'num') body += '<td class="num"><input class="ed" data-k="' + c.key + '" value="' + esc(v == null ? '' : v) + '"></td>';
                else if (c.edit === 'text') body += '<td><input class="ed txt" data-k="' + c.key + '" value="' + esc(v == null ? '' : v) + '"></td>';
                else if (c.edit === 'select') {
                    body += '<td><select class="ed" data-k="' + c.key + '"><option value=""></option>' + (c.opts || []).map(function (x) {
                        var ov = ci(x, c.ok || 'Id'); return '<option value="' + esc(ov) + '"' + (String(ov) === String(v == null ? '' : v) ? ' selected' : '') + '>' + esc(ci(x, c.tk || 'Name')) + '</option>';
                    }).join('') + '</select></td>';
                }
                else if (c.link) body += '<td class="lnk" data-l="' + c.key + '">' + esc(c.fmt ? c.fmt(v, r) : fmtPlain(v)) + '</td>';
                else if (c.pick) body += '<td class="lnk" data-p="' + c.key + '" title="F1 / click: choose">' + esc(c.fmt ? c.fmt(v, r) : fmtPlain(v)) + '</td>';
                else body += '<td' + (c.num ? ' class="num"' : '') + '>' + esc(c.fmt ? c.fmt(v, r) : fmtPlain(v)) + '</td>';
            });
            body += '</tr>';
        });
        tbody.innerHTML = body;
        if (tfoot) {
            var hasTotal = vis.some(function (c) { return c.total; });
            if (hasTotal && rows && rows.length) {
                tfoot.innerHTML = '<tr>' + btns.map(function () { return '<td></td>'; }).join('') + vis.map(function (c) {
                    if (!c.total) return '<td></td>';
                    var s = 0; rows.forEach(function (r) { s += num(r[c.key]); });
                    if (c.total === 'avg') s = s / rows.length;
                    return '<td class="num">' + esc((c.tfmt || c.fmt || f2)(s)) + '</td>';
                }).join('') + '</tr>';
            } else tfoot.innerHTML = '';
        }
        tbody.onclick = function (e) {
            var td = e.target.closest('td'), tr = e.target.closest('tr[data-i]');
            if (!tr) return;
            var i = +tr.getAttribute('data-i'), r = rows[i];
            if (td && td.hasAttribute('data-b')) { btns[+td.getAttribute('data-b')].click(r, i); return; }
            if (td && td.hasAttribute('data-l')) { var c = cols.filter(function (x) { return x.key === td.getAttribute('data-l'); })[0]; if (c && c.link) c.link(r, i); return; }
            if (td && td.hasAttribute('data-p')) { var pc = cols.filter(function (x) { return x.key === td.getAttribute('data-p'); })[0]; if (pc && pc.pick) pc.pick(r, i); return; }
            if (o.selectable !== false) {
                rows.forEach(function (x) { delete x.__sel; }); r.__sel = true;
                tbody.querySelectorAll('tr.is-selected').forEach(function (x) { x.classList.remove('is-selected'); });
                tr.classList.add('is-selected');
                if (o.onSelect && !(e.target.tagName === 'INPUT' || e.target.tagName === 'SELECT')) o.onSelect(r, i);
            }
        };
        tbody.onchange = function (e) {
            var el = e.target, tr = el.closest('tr[data-i]');
            if (!tr || !el.hasAttribute('data-k')) return;
            var i = +tr.getAttribute('data-i'), r = rows[i], k = el.getAttribute('data-k');
            if (el.tagName === 'SELECT') r[k] = el.value === '' ? 0 : (isNaN(+el.value) ? el.value : +el.value);
            else if (el.classList.contains('txt')) r[k] = el.value;
            else r[k] = num(el.value);
            if (o.onChange) o.onChange(r, k, i);
            renderGrid(tableId, cols, rows, o);
        };
        tbody.onkeydown = function (e) {
            if (e.key === 'F1') {
                var tr = e.target.closest('tr[data-i]'); if (!tr) return;
                var pc = cols.filter(function (x) { return x.pick; })[0];
                if (pc) { e.preventDefault(); pc.pick(rows[+tr.getAttribute('data-i')], +tr.getAttribute('data-i')); }
            }
        };
    }
    function clearGrid(id) { var t = $id(id); if (!t) return; t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; }
    function allCols(rows, hidden, fmts) {
        if (!rows || !rows.length) return [];
        return Object.keys(rows[0]).filter(function (k) { return k !== '__sel'; }).map(function (k) {
            var c = { key: k, cap: k, hide: (hidden || []).indexOf(k) >= 0 };
            if (fmts && fmts[k]) { c.num = true; c.fmt = fmts[k].fmt; c.total = fmts[k].total; c.tfmt = fmts[k].tfmt; }
            else if (/date/i.test(k)) c.fmt = dMMMyyyy;
            return c;
        });
    }

    // ------------------------------------------------------------------------------------------- tabs
    /** tabControl1_SelectedIndexChanged:1550 */
    function tab(name) {
        tabName = name;
        document.querySelectorAll('#tabControl1 .win-tab').forEach(function (x) { x.classList.toggle('is-active', x.getAttribute('data-tab') === name); });
        ['InPut', 'OutPut', 'PackingMaterial', 'OverHead', 'TransctionHistory', 'Summary', 'Settlement'].forEach(function (p) { $id(p).classList.toggle('is-active', p === name); });
        if (name === 'OutPut' && visible('btnSaveOutPut')) { out.serial(); $id('DocDateOutPut').focus(); }
        if (name === 'PackingMaterial') { pm.nextCode(); $id('txtdocdatePackingMaterial').focus(); }
        if (name === 'OverHead') { oh.nextCode(); $id('txtDocDateOverHead').focus(); }
        if (name === 'Settlement') $id('CmbJobOrderSettlement').focus();
        if (name === 'Summary') $id('cmbsummeryJobOrderNo').focus();
        th.gridHistory();   // GridHistory() runs on every tab change (:1575)
    }
    var SUB = { InPut: ['tabPage1', 'tabPage4'], OutPut: ['tabPage5', 'tabPage6'], PackingMaterial: ['PmForm', 'PmHistory'], OverHead: ['tabPage2', 'tabPage3'], Summary: ['tabPage7', 'tabPage9'] };
    function subTab(page, name) {
        SUB[page].forEach(function (p) { $id(p).classList.toggle('is-active', p === name); });
        var strip = { InPut: 'tabControl2', OutPut: 'tabControl3', PackingMaterial: 'tabControlPm', OverHead: 'tabControl4', Summary: 'tabControl5' }[page];
        document.querySelectorAll('#' + strip + ' .win-tab').forEach(function (x) { x.classList.toggle('is-active', x.getAttribute('data-tab') === name); });
        if (name === 'tabPage4') inp.history();           // tabControl2_SelectedIndexChanged:2226
        if (name === 'tabPage6') out.history();           // tabControl3_SelectedIndexChanged:3065
        if (name === 'PmHistory') { show('chkPendingForRatesWrap', rateRight()); $id('chkPendingForRates').checked = false; pm.history(); }  // :4049
        if (name === 'tabPage3') oh.history();            // tabControl4_SelectedIndexChanged:4828
    }

    // ------------------------------------------------------------------------------------------- load
    /** frmFoodProduction_Load:893 */
    function load() {
        return C.getJson(API + '/init').then(function (d) {
            st.init = d; st.rights = d.rights || {}; st.amountDec = d.amountDecimals || 0; st.rateDec = d.rateDecimals || 2;
            st.wagesStatus = !!d.wagesStatus; st.loaderStatus = !!d.loaderStatus;
            st.jobOrders = d.jobOrders || []; st.jobOrdersAll = d.jobOrdersAll || []; st.warehouses = d.warehouses || [];
            st.jobLots = d.jobLots || []; st.cropYears = d.cropYears || []; st.packingTypes = d.packingTypes || [];
            st.uomSchedules = d.uomSchedules || []; st.uomStatic = d.uomStatic || []; st.departments = d.departments || [];
            st.differenceAccounts = d.differenceAccounts || [];
            var r = st.rights;
            /* :921-928 */
            $id('btnsave').disabled = !r.save; $id('btnSaveOutPut').disabled = !r.save;
            $id('btnUpdate').disabled = !r.update; $id('btnUpdateOutPut').disabled = !r.update;
            $id('Print').disabled = !r.print; $id('btnVoucherReport').disabled = !r.print; $id('btnVoucherOutPut').disabled = !r.print; $id('btnPrintOutPut').disabled = !r.print;
            /* :945 - the Settlement and Over Head tabs go when the ProductionSettlement right is missing */
            if (!r.settlementTab) { $id('tabSettlement').remove(); $id('tabOverHead').remove(); }
            /* :1069 - no Rate right: the pending-rates and voucher buttons are not visible */
            if (!r.rateAndAmount) { ['btnPenForRateEntries', 'btnVoucherReport', 'btnVoucherPM', 'btnVoucherOutPut', 'btnVoucherOH'].forEach(function (id) { show(id, false); }); }
            bindLists();
            sel('CmbProductionDepartment', st.departments, 'Id', 'WareHouseName');
            setVal('txtdocnumber', d.docNoInput || '');
            ['txtDocdate', 'DocDateOutPut', 'txtdocdatePackingMaterial', 'txtDocDateOverHead', 'txtfromDate', 'txtToDate'].forEach(function (id) { setVal(id, d.today); });
            inp.renderGrid(); out.renderGrid(); pm.renderGrid(); oh.renderGrid();
            if (d.errors && d.errors.length) alert(d.errors.join('\n'));   /* each failed lookup is reported, never an empty list (:1081) */
            return C.getJson(API + '/warehouses-all').then(function (w) { st.warehousesAll = w || []; }).catch(function () { st.warehousesAll = st.warehouses; });
        }).then(function () { $id('txtDocdate').focus(); }).catch(fail);
    }
    function bindLists() {
        ['CmbJobOrderNo', 'CmbJobOrderNoOutput', 'cmbJobOrderPackingMaterial', 'cmbJobOrderOverHead'].forEach(function (id) { var v = val(id); sel(id, st.jobOrders, 'Id', 'PlanCode'); setVal(id, v); });
        ['cmbsummeryJobOrderNo', 'CmbJobOrderSettlement', 'cmbPendJobOrder'].forEach(function (id) { var v = val(id); sel(id, st.jobOrdersAll, 'Id', 'PlanCode'); setVal(id, v); });
        var v = val('CmbDifferenceAccountTitle'); sel('CmbDifferenceAccountTitle', st.differenceAccounts, 'Id', 'AccountTitle'); setVal('CmbDifferenceAccountTitle', v);
    }
    /** JobOrderNoFill:1180 (btnRefresh_Click:1953 / btnRefreshPackingMaterial_Click:3484 / btnRefreshOverHead_Click:4462) */
    function refreshJobOrders() {
        return C.getJson(API + '/job-orders').then(function (j) { st.jobOrders = j || []; bindLists(); }).catch(fail);
    }
    /** JobOrderNoFillForSummery:1223 (btnRefreshSummary_Click:5341) */
    function refreshJobOrdersAll() {
        return C.getJson(API + '/job-orders-all').then(function (j) { st.jobOrdersAll = j || []; bindLists(); }).catch(fail);
    }
    /** GetPlantFeeder:1327 - the plant list of one job order into the given combo */
    function plants(jobOrderId, comboId) {
        if (!jobOrderId) { sel(comboId, [], 'PlantId', 'PlantName'); return Promise.resolve([]); }
        return C.getJson(API + '/plants?jobOrderId=' + jobOrderId).then(function (p) { sel(comboId, p || [], 'PlantId', 'PlantName'); return p || []; });
    }
    /** GetGlAccountsByJobOrderId -> CmbWorkInProcess / CmbWIPItem (:1635 / :2525) */
    function glAccounts(jobOrderId) {
        return C.getJson(API + '/gl-accounts?jobOrderId=' + jobOrderId).then(function (rows) {
            rows = rows || [];
            sel('CmbWorkInProcess', rows, 'WorkInProccessAcId', 'WorkInProcessAc');
            sel('CmbWIPItem', rows, 'WipItemId', 'ItemName');
            if (rows.length) { $id('CmbWorkInProcess').selectedIndex = 1; $id('CmbWIPItem').selectedIndex = 1; }
            return rows;
        });
    }
    /** GetInPutTotalQtyandWeightByJobOrderId sums (:2494 / :3438 / :4234) */
    function totals(jobOrderId) {
        return C.getJson(API + '/totals?jobOrderId=' + jobOrderId).then(function (rows) {
            var s = { inQty: 0, inW: 0, outQty: 0, outW: 0, fgQty: 0, fgW: 0, any: !!(rows && rows.length) };
            (rows || []).forEach(function (r) {
                if (String(ci(r, 'EntryTypeOutPut')) === 'FinishGoods') { s.fgQty += num(ci(r, 'ItemQty')); s.fgW += num(ci(r, 'StockWeight')); }
                if (String(ci(r, 'EntryType')) === 'Input') { s.inQty += num(ci(r, 'ItemQty')); s.inW += num(ci(r, 'StockWeight')); }
                else { s.outQty += num(ci(r, 'ItemQty')); s.outW += num(ci(r, 'StockWeight')); }
            });
            return s;
        });
    }
    function voucherHeadId(id, docType) {
        if (!id) return Promise.resolve(0);
        return C.getJson(API + '/voucher-head?id=' + id + '&documentTypeId=' + docType).then(function (r) { return C.intOf(r.voucherHeadId); }).catch(function () { return 0; });
    }
    /** CommonServices.VoucherReport_118(VoucherHeadId) (:1587) */
    function voucher118(which) {
        var v = { input: inp.voucherHeadId, output: out.voucherHeadId, pm: pm.voucherHeadId, oh: oh.voucherHeadId }[which] || 0;
        if (!v) { alert('Voucher Not Found'); return; }
        window.CrystalPrint.open('acc-118', { id: v });
    }
    /** btnJobOrderDefine_Click:2354 - ProductionPreCostingAndJobOrder is a separate screen; the web has the
     *  DocumentTypeId-401 job order page (frmProductionJobOrder) at /production/job-order-not-use, so that opens. */
    function defineJobOrder() { window.open('/production/job-order-not-use', '_blank'); }

    /** Reporting.ShowReportWithDataTable(dt, rpt) -> POST /reports/print/grid with the rows the server read. */
    function printRows(url, rpt) {
        var w = window.open('', '_blank');
        C.getJson(url).then(function (rows) {
            if (!rows || !rows.length) { if (w) w.close(); alert('Record Not Found For Display'); return; }
            return fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: null, rows: rows }) })
                .then(function (r) {
                    if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); });
                    return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
                });
        }).catch(function (e) { if (w) w.close(); fail(e); });
    }
    /** frmwagesBillHeader (ShowDialog) - /production/wages-bill in an overlay (:1836 / :3008) */
    function wagesOpen(refDocTypeId, refDocId, gross) {
        $id('wagesFrame').src = '/production/wages-bill?' + new URLSearchParams({ refDocTypeId: refDocTypeId, refDocId: refDocId, grossWeightTotal: gross }).toString();
        C.openModal('dlgWages');
    }
    function wagesClose() { C.closeModal('dlgWages'); $id('wagesFrame').src = 'about:blank'; }

    // =========================================================================================== INPUT (80)
    var inp = (function () {
        var rows = [], recId = 0, vhId = 0;
        function cols() {
            var rr = rateRight();
            return [
                { key: 'Id', hide: true }, { key: 'JobOrderInPutId', hide: true }, { key: 'RefDocumentTypeId', hide: true }, { key: 'RefDocIdNo', hide: true }, { key: 'RefDocSubIdNo', hide: true },
                { key: 'DocDate', hide: true }, { key: 'DocNo', hide: true }, { key: 'WareHouseId', hide: true },
                { key: 'WareHouse', cap: 'WareHouse', width: 150 }, { key: 'ItemId', hide: true }, { key: 'Item', cap: 'Item', width: 300 },
                { key: 'ItemUOMId', hide: true }, { key: 'ItemUOM', cap: 'ItemUOM' }, { key: 'ItemEquivalent', hide: true },
                { key: 'CropYear', cap: 'CropYear' }, { key: 'JobLotId', hide: true }, { key: 'JobLot', cap: 'JobLot' },
                { key: 'PackingTypeId', hide: true }, { key: 'PackingType', cap: 'PackingType' },
                { key: 'BalanceQty', cap: 'BalanceQty', num: true, fmt: f2, total: 'sum' }, { key: 'BalanceWeight', cap: 'BalanceWeight', num: true, fmt: f2, total: 'sum' },
                { key: 'Quantity', cap: 'Quantity', edit: 'num', fmt: f2, total: 'sum' }, { key: 'Weight', cap: 'Weight', edit: 'num', fmt: f2, total: 'sum' },
                { key: 'Rate', cap: 'Rate', num: true, fmt: fRate, hide: !rr }, { key: 'RateUOMId', hide: true }, { key: 'RateUOM', cap: 'RateUOM', hide: !rr }, { key: 'RateEquivalent', hide: true },
                { key: 'ItemAmount', cap: 'ItemAmount', num: true, fmt: fAmt, total: 'sum', hide: !rr },
                { key: 'Remarks', cap: 'Remarks', edit: 'text' }
            ];
        }
        /** gridsettings:2016 - the X column only while Save is visible and enabled (:2111) */
        function renderGrid() {
            var canDelete = visible('btnsave');
            renderGrid_('grdStockConversionProductionDetail', cols(), rows, {
                buttons: canDelete ? [{ text: 'X', cls: 'xbtn', click: function (r, i) { rows.splice(i, 1); renderGrid(); } }] : [],
                onChange: cellUpdated
            });
        }
        /** grdStockConversionProductionDetail_CellUpdated:1958 */
        function cellUpdated(r, k) {
            if (k === 'Quantity') {
                r.Weight = num(r.Quantity) * num(r.ItemEquivalent);
                r.ItemAmount = num(r.Weight) / num(r.RateEquivalent) * num(r.Rate);
            }
            if (k === 'Weight') r.ItemAmount = num(r.Weight) / num(r.RateEquivalent) * num(r.Rate);
            if (!isFinite(r.ItemAmount)) r.ItemAmount = 0;
        }
        /** GenerateDocNumberInput:1595 */
        function serial() { return C.getJson(API + '/serial?documentTypeId=80').then(function (r) { if (C.intOf(r.docNo) > 0) setVal('txtdocnumber', r.docNo); }).catch(fail); }
        /** CmbJobOrderNo_Leave:1625 */
        function jobOrderLeave() {
            var jo = intOf('CmbJobOrderNo');
            return glAccounts(jo).then(function () { return plants(jo, 'CmbPlantFeeder'); }).catch(fail);
        }
        /** reset:1928 */
        function reset() {
            recId = 0; vhId = 0; rows = [];
            setVal('txtRemarks', '');
            show('btnsave', true); show('btnUpdate', false);
            $id('CmbJobOrderNo').disabled = false;
            setVal('CmbJobOrderNo', '');
            serial();
            refreshJobOrders().then(function () { setVal('CmbJobOrderNo', ''); setVal('CmbPlantFeeder', ''); });
            renderGrid();
        }
        function body() {
            return { id: recId, docNumber: val('txtdocnumber'), docDate: val('txtDocdate'), remarks: val('txtRemarks'),
                jobOrderId: intOf('CmbJobOrderNo'), jobOrderText: selText('CmbJobOrderNo'), plantId: intOf('CmbPlantFeeder'),
                wipAccountId: intOf('CmbWorkInProcess'), wipItemId: intOf('CmbWIPItem'), departmentId: intOf('CmbProductionDepartment'), rows: rows };
        }
        /** FormValidation:1656 */
        function validate() {
            var d = val('txtdocnumber').trim();
            if (d === '' || d === '0') { alert('document Number Field Required'); return false; }
            if (!intOf('CmbJobOrderNo')) { alert('Job Order Number Field Required'); $id('CmbJobOrderNo').focus(); return false; }
            if (!intOf('CmbPlantFeeder')) { alert('Plant Name Field Required'); $id('CmbPlantFeeder').focus(); return false; }
            if (!intOf('CmbWorkInProcess')) { alert('WorkInProcess Field Required'); $id('CmbWorkInProcess').focus(); return false; }
            if (!intOf('CmbWIPItem')) { alert('WIP Item Field Required'); $id('CmbWIPItem').focus(); return false; }
            return true;
        }
        /** InsertInput:1691 */
        function insert() {
            if (!validate()) return;
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            if (!rows.length) { alert('Enter Detail First ...'); return; }
            C.postJson(API + '/input/save', body()).then(function (res) {
                alert(res.message);
                if (res.openWages) wagesOpen(DOC.INPUT, res.id, res.grossWeightTotal);
                reset();
            }).catch(fail);
        }
        function save() { recId = 0; insert(); }    // btnsave_Click:1856
        function update() { insert(); }             // btnUpdate_Click:1869
        /** ReadByIdInput:1881 */
        function readById(id) {
            recId = id;
            show('btnsave', false); show('btnUpdate', true);
            return C.getJson(API + '/production/' + id).then(function (h) {
                var p = Promise.resolve();
                if (String(h.EntryType) === 'Input') {
                    setVal('txtdocnumber', h.DocCode); setVal('txtDocdate', isoOf(h.DocDate)); setVal('txtRemarks', h.MainRemarks || '');
                    setVal('CmbProductionDepartment', h.EBDepartmentId);
                    setVal('CmbJobOrderNo', h.InvJobOrderId);
                    p = jobOrderLeave().then(function () { setVal('CmbPlantFeeder', h.PlantId); });
                }
                rows = (h.details || []).filter(function (d) { return String(ci(d, 'EntryType')) === 'Issue'; }).map(function (d) {
                    return { Id: ci(d, 'Id'), JobOrderInPutId: ci(d, 'JobOrderInPutId'), RefDocumentTypeId: ci(d, 'RefDocumentTypeId'), RefDocIdNo: ci(d, 'RefDocNoId'), RefDocSubIdNo: ci(d, 'RefDocSubIdNo'),
                        DocDate: dMMMyy(ci(d, 'DocDate')), DocNo: ci(d, 'DocNo'), WareHouseId: ci(d, 'WarehouseId'), WareHouse: ci(d, 'WareHouseName'), ItemId: ci(d, 'ItemId'), Item: ci(d, 'ItemName'),
                        ItemUOMId: ci(d, 'ItemUomId'), ItemUOM: ci(d, 'PackUom'), ItemEquivalent: ci(d, 'Equivalent'), CropYear: ci(d, 'CropBatch'), JobLotId: ci(d, 'JobLotId'), JobLot: ci(d, 'JobLotDescription'),
                        PackingTypeId: ci(d, 'PackingtypeId'), PackingType: ci(d, 'PackTypeDesc'), BalanceQty: ci(d, 'Qty'), BalanceWeight: ci(d, 'Weight'), Quantity: ci(d, 'Qty'), Weight: ci(d, 'Weight'),
                        Rate: Math.round(num(ci(d, 'Rate')) * 100) / 100, RateUOMId: ci(d, 'RateUOMId'), RateUOM: ci(d, 'RateUOM'), RateEquivalent: ci(d, 'RateEquivalent'), ItemAmount: ci(d, 'Amount'), Remarks: ci(d, 'Remarks') };
                });
                if (rows.length) { tab('InPut'); subTab('InPut', 'tabPage1'); }
                vhId = C.intOf(h.voucherHeadId);
                renderGrid();
                return p;
            }).catch(fail);
        }
        /** btnLoadInvoices_Click:2130 -> LoadInputDataByJobOrder -> LoadDataDetailfromPurchaseInvoivce:2152 */
        function loadInput() {
            var jo = intOf('CmbJobOrderNo');
            if (!jo) { $id('CmbJobOrderNo').focus(); alert('Select Job Order No First...'); return; }
            loader.open('input', jo, 'Load InPut - Pending Job Order Input Rows', function (picked) {
                picked.forEach(function (r) {
                    if (rows.some(function (x) { return C.intOf(x.JobOrderInPutId) === C.intOf(ci(r, 'Id')); })) return;   // :2167
                    rows.push({ Id: 0, JobOrderInPutId: ci(r, 'Id'), RefDocumentTypeId: ci(r, 'RefDocumentTypeId'), RefDocIdNo: ci(r, 'RefDocIdNo'), RefDocSubIdNo: ci(r, 'RefDocSubIdNo'),
                        DocDate: dMMMyyyy(ci(r, 'RefDocDate')), DocNo: ci(r, 'RefDocNo'), WareHouseId: ci(r, 'WareHouseId'), WareHouse: ci(r, 'WareHouseName'), ItemId: ci(r, 'ItemId'), Item: ci(r, 'ItemName'),
                        ItemUOMId: ci(r, 'PackUomId'), ItemUOM: ci(r, 'PackUom'), ItemEquivalent: ci(r, 'PackUomEquivalent'), CropYear: ci(r, 'CropYear'), JobLotId: ci(r, 'JobLotId'), JobLot: ci(r, 'JobLotDescription'),
                        PackingTypeId: ci(r, 'PackingTypeId'), PackingType: ci(r, 'PackTypeDesc'), BalanceQty: ci(r, 'balQty'), BalanceWeight: ci(r, 'balWeight'), Quantity: ci(r, 'balQty'), Weight: ci(r, 'balWeight'),
                        Rate: ci(r, 'ItemRate'), RateUOMId: ci(r, 'RateUomId'), RateUOM: ci(r, 'RateUom'), RateEquivalent: ci(r, 'RateUomEquivalent'), ItemAmount: ci(r, 'balAmount'), Remarks: '' });
                });
                renderGrid();
                $id('CmbJobOrderNo').disabled = true;   // :2180
            });
        }
        /** Print_Click:2188 -> 601-InvFoodProductionSlip.rpt */
        function print() { printRows(API + '/print/601?id=' + recId + '&entryType=Input&jobOrderId=' + intOf('CmbJobOrderNo'), '601-InvFoodProductionSlip.rpt'); }
        /** InputGridHistory:2234 */
        var hrows = [];
        function history() {
            C.getJson(API + '/history?documentTypeId=80').then(function (rs) {
                hrows = (rs || []).map(histRow);
                renderGrid_('grdinputhistory', histCols(true), hrows, {
                    buttons: [{ text: 'Edit', cls: 'editbtn', click: function (r) {
                        /* grdinputhistory_ColumnButtonClick:2312 */
                        if (String(r.EntryType) !== 'Input') return;
                        if (String(r.IsApproved) === 'Approved') { alert("You Can't Update Approved Record"); return; }
                        readById(C.intOf(r.Id));
                    } }],
                    onSelect: function (r) { detail(C.intOf(r.Id)); }
                });
            }).catch(fail);
        }
        /** InputDetailByHeaderId:6947 */
        function detail(id) {
            C.getJson(API + '/production/' + id).then(function (h) {
                var rr = rateRight();
                var dr = (h.details || []).filter(function (d) { return String(ci(d, 'EntryType')) === 'Issue'; }).map(function (d) {
                    return { DocDate: dMMMyy(ci(d, 'DocDate')), DocNo: ci(d, 'DocNo'), WareHouseName: ci(d, 'WareHouseName'), ItemName: ci(d, 'ItemName'), PackUom: ci(d, 'Equivalent'), CropYear: ci(d, 'CropBatch'),
                        JobLot: ci(d, 'JobLotDescription'), PackingType: ci(d, 'PackTypeDesc'), Qty: ci(d, 'Qty'), Weight: ci(d, 'Weight'), Rate: Math.round(num(ci(d, 'Rate')) * 100) / 100, RateUom: ci(d, 'RateUOM'), Amount: ci(d, 'Amount'), Remarks: ci(d, 'Remarks') };
                });
                renderGrid_('grdInputDetail', [
                    { key: 'DocDate' }, { key: 'DocNo' }, { key: 'WareHouseName' }, { key: 'ItemName' }, { key: 'PackUom' }, { key: 'CropYear' }, { key: 'JobLot' }, { key: 'PackingType' },
                    { key: 'Qty', num: true, fmt: fi, total: 'sum' }, { key: 'Weight', num: true, fmt: fi, total: 'sum' }, { key: 'Rate', num: true, hide: !rr }, { key: 'RateUom', hide: !rr },
                    { key: 'Amount', num: true, fmt: fi, total: 'sum', hide: !rr }, { key: 'Remarks' }], dr, { selectable: false });
            }).catch(fail);
        }
        return { renderGrid: renderGrid, serial: serial, jobOrderLeave: jobOrderLeave, reset: reset, save: save, update: update, readById: readById,
            loadInput: loadInput, print: print, history: history, get voucherHeadId() { return vhId; }, get recId() { return recId; },
            lockJobOrder: function () { $id('CmbJobOrderNo').disabled = true; } };
    })();
    var renderGrid_ = renderGrid;

    /** the Input/Output history grid rows (InputGridHistory:2255 / OutputGridHistory:3094) */
    function histRow(r) {
        return { Id: ci(r, 'Id'), DocSrNo: ci(r, 'DocCode'), DocDate: dMMMyyyy(ci(r, 'DocDate')), JobOrderNo: ci(r, 'InvJobOrderNo'), PlantName: ci(r, 'PlantName'), EntryType: ci(r, 'EntryType'),
            Remarks: ci(r, 'MainRemarks'), IsApproved: ci(r, 'JobOrderApprovedStatus'), StartDate: dMMMyyyy(ci(r, 'StartDate')), EndDate: dMMMyyyy(ci(r, 'EndDate')),
            EntryDate: dTime(ci(r, 'EntryDate')), EntryUser: ci(r, 'EntryUser'), ModifyDate: dTime(ci(r, 'ModifyDate')), ModifyUser: ci(r, 'ModifyUser'), PlanStatus: ci(r, 'PlanStatus') };
    }
    function histCols(withPlant) {
        var c = [{ key: 'Id', hide: true }, { key: 'DocSrNo' }, { key: 'DocDate' }, { key: 'JobOrderNo' }];
        if (withPlant) c.push({ key: 'PlantName' });
        return c.concat([{ key: 'EntryType' }, { key: 'Remarks' }, { key: withPlant ? 'IsApproved' : 'JobOrderStatus' }, { key: 'StartDate' }, { key: 'EndDate' }, { key: 'EntryDate' }, { key: 'EntryUser' }, { key: 'ModifyDate' }, { key: 'ModifyUser' }, { key: 'PlanStatus' }]);
    }

    // =========================================================================================== OUTPUT (112)
    var out = (function () {
        var rows = [], recId = 0, vhId = 0, avgRate = 0;
        function cols() {
            var rr = rateRight();
            return [
                { key: 'Id', hide: true }, { key: 'OutPutIdFromJobOrder', hide: true }, { key: 'EntryType', cap: 'EntryType' },
                { key: 'WareHouseId', cap: 'WareHouse Name', edit: 'select', opts: st.warehousesAll, ok: 'Id', tk: 'WareHouseName' },
                { key: 'ItemId', hide: true }, { key: 'Item', cap: 'Item' }, { key: 'ItemUOMId', hide: true },
                { key: 'ItemUOM', cap: 'ItemUOM', pick: pickUom }, { key: 'ItemEquivalent', hide: true }, { key: 'CropYearId', hide: true }, { key: 'CropYear', cap: 'CropYear' },
                { key: 'JobLotId', cap: 'JobLot', edit: 'select', opts: st.jobLots, ok: 'Id', tk: 'JobLotDescription' },
                { key: 'PackingTypeId', cap: 'Packing Type', edit: 'select', opts: st.packingTypes, ok: 'Id', tk: 'PackTypeDesc' },
                { key: 'Quantity', cap: 'Quantity', edit: 'num', fmt: f2, total: 'sum' }, { key: 'Weight', cap: 'Weight', edit: 'num', fmt: f2, total: 'sum' },
                { key: 'Rate', cap: 'Rate', num: true, fmt: fRate, hide: !rr }, { key: 'RateUOMId', hide: true }, { key: 'RateUOM', cap: 'RateUOM', hide: !rr }, { key: 'RateEquivalent', hide: true },
                { key: 'ItemAmount', cap: 'ItemAmount', num: true, fmt: fAmt, total: 'sum', hide: !rr },
                { key: 'NetRate', cap: 'NetRate', edit: 'num', fmt: fRate }, { key: 'NetAmount', cap: 'NetAmount', num: true, fmt: fAmt },
                { key: 'Remarks', cap: 'Remarks', edit: 'text' }, { key: 'IssueWeight', cap: 'IssueWeight', num: true, fmt: f2 }
            ];
        }
        /** gridsettingsOutPut:2558 - X / + columns only while Save is visible and enabled (:2645) */
        function renderGrid() {
            var canEdit = visible('btnSaveOutPut');
            renderGrid_('grdByProduct', cols(), rows, {
                buttons: canEdit ? [
                    { text: 'X', cls: 'xbtn', click: function (r, i) { if (!visible('btnSaveOutPut')) { alert('You can not Delete row in Update Mode'); return; } rows.splice(i, 1); renderGrid(); } },
                    { text: '+', cls: 'addbtn', click: function (r) { if (!visible('btnSaveOutPut')) { alert('You can not add row in Update Mode'); return; } var c = JSON.parse(JSON.stringify(r)); delete c.__sel; c.Id = 0; rows.push(c); renderGrid(); } }
                ] : [],
                onChange: cellUpdated
            });
        }
        /** grdByProduct_CellUpdated:2671 */
        function cellUpdated(r, k) {
            if (k === 'Quantity') { r.Weight = num(r.Quantity) * num(r.ItemEquivalent); r.ItemAmount = num(r.Weight) / num(r.RateEquivalent) * num(r.Rate); }
            if (k === 'Weight') r.ItemAmount = num(r.Weight) / num(r.RateEquivalent) * num(r.Rate);
            if (!isFinite(r.ItemAmount)) r.ItemAmount = 0;
        }
        /** grdByProduct_KeyDown:2749 (F1 on ItemUOM) - GetUomScheduleByItemId -> GrdPopUp */
        function pickUom(r) {
            C.getJson(API + '/uom-by-item?itemId=' + C.intOf(r.ItemId)).then(function (uoms) {
                return C.pickFrom('UOM', uoms || [], [{ key: 'Id', caption: 'Id' }, { key: 'UOMCode', caption: 'UOMCode' }, { key: 'Equivalent', caption: 'Equivalent' }]).then(function (u) {
                    if (!u) return;
                    var eq = num(ci(u, 'Equivalent'));
                    r.ItemUOMId = ci(u, 'Id'); r.ItemUOM = ci(u, 'UOMCode'); r.ItemEquivalent = eq;
                    r.Weight = Math.round(num(r.Quantity) * eq * 1000) / 1000;
                    r.ItemAmount = Math.round(num(r.Weight) / num(r.RateEquivalent) * num(r.Rate) * 1000) / 1000;
                    if (!isFinite(r.ItemAmount)) r.ItemAmount = 0;
                    renderGrid();
                });
            }).catch(fail);
        }
        /** GenerateDocNumberOutPut:2438 */
        function serial() { return C.getJson(API + '/serial?documentTypeId=112').then(function (r) { if (C.intOf(r.docNo) > 0) setVal('txtDocNoOutPut', r.docNo); }).catch(fail); }
        /** CmbJobOrderNoOutput_Leave:2468 */
        function jobOrderLeave() {
            var jo = intOf('CmbJobOrderNoOutput');
            return totals(jo).then(function (s) {
                if (s.any) {
                    setVal('txtInPutTotalQty', fi(s.inQty)); setVal('txtInPutTotalWeight', fi(s.inW)); setVal('txtOutQtyTotal', fi(s.outQty)); setVal('txtOutWeightTotal', fi(s.outW));
                    setVal('txtBalQtyTotal', fi(s.inQty - s.outQty)); setVal('txtBalWeightTotal', fi(s.inW - s.outW));
                } else ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtOutQtyTotal', 'txtOutWeightTotal', 'txtBalQtyTotal', 'txtBalWeightTotal'].forEach(function (id) { setVal(id, '0'); });
                return glAccounts(jo);
            }).then(function (gl) {
                if (gl.length) {
                    if (visible('btnSaveOutPut') || num(val('txtAvgFinishGoodRate')) === 0) setVal('txtAvgFinishGoodRate', ci(gl[0], 'FinishGoodRate'));
                } else setVal('txtAvgFinishGoodRate', '0');
                return plants(jo, 'CmbPlantFeederOutput');
            }).catch(fail);
        }
        /** btnRefreshOutPut_Click:2385 - stockAccoutnFill, UOMFill, combojoblotfill, CropYear, Warehouse, bagType, JobOrderNoFill */
        function refresh() {
            C.getJson(API + '/lists').then(function (d) {
                st.differenceAccounts = d.differenceAccounts || st.differenceAccounts; st.jobLots = d.jobLots || st.jobLots; st.warehouses = d.warehouses || st.warehouses;
                st.warehousesAll = d.warehousesAll || st.warehousesAll; st.jobOrders = d.jobOrders || st.jobOrders; bindLists(); renderGrid();
            }).catch(fail);
        }
        /** resetOutPut:3028 */
        function reset() {
            recId = 0; vhId = 0; rows = [];
            ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtOutQtyTotal', 'txtOutWeightTotal', 'txtBalQtyTotal', 'txtBalWeightTotal'].forEach(function (id) { setVal(id, '0'); });
            show('btnSaveOutPut', true); show('btnUpdateOutPut', false);
            $id('CmbJobOrderNoOutput').disabled = false;
            serial();
            setVal('txtAvgFinishGoodRate', '');
            refreshJobOrders().then(function () { setVal('CmbJobOrderNoOutput', ''); setVal('CmbPlantFeederOutput', ''); });
            renderGrid();
        }
        /** FormValidationOutPut:2403 */
        function validate() {
            var d = val('txtDocNoOutPut').trim();
            if (d === '' || d === '0') { alert('DocNo Field Required'); return false; }
            if (!intOf('CmbJobOrderNoOutput')) { alert('Job Order Number Field Required'); $id('CmbJobOrderNoOutput').focus(); return false; }
            if (!intOf('CmbPlantFeederOutput')) { alert('Plant Name Field Is Required'); $id('CmbPlantFeederOutput').focus(); return false; }
            if (!intOf('CmbWorkInProcess')) { alert('WorkInProcess Field Required'); return false; }
            if (!intOf('CmbWIPItem')) { alert('WIP Item Field Required'); return false; }
            return true;
        }
        /** OutPutInsert:2891 */
        function insert() {
            if (!validate()) return;
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            if (!rows.length) { alert('Enter Detail First ...'); return; }
            C.postJson(API + '/output/save', { id: recId, docNumber: val('txtDocNoOutPut'), docDate: val('DocDateOutPut'), remarks: val('txtRemarksHeaderOutPut'),
                jobOrderId: intOf('CmbJobOrderNoOutput'), jobOrderText: selText('CmbJobOrderNoOutput'), plantId: intOf('CmbPlantFeederOutput'),
                wipAccountId: intOf('CmbWorkInProcess'), wipItemId: intOf('CmbWIPItem'), rows: rows }).then(function (res) {
                alert(res.message);
                if (res.openWages) wagesOpen(DOC.OUTPUT, res.id, res.grossWeightTotal);
                reset();
            }).catch(fail);
        }
        function save() { recId = 0; insert(); }   // btnSaveOutPut_Click:2866
        function update() { insert(); }            // btnUpdateOutPut_Click:2879
        /** ReadByIdOutPut:2791 */
        function readById(id) {
            if (id <= 0) return Promise.resolve();
            recId = id;
            show('btnSaveOutPut', false); show('btnUpdateOutPut', true);
            return C.getJson(API + '/production/' + id).then(function (h) {
                var p = Promise.resolve();
                if (String(h.EntryType) !== 'Input') {
                    setVal('txtDocNoOutPut', h.DocCode); setVal('DocDateOutPut', isoOf(h.DocDate)); setVal('txtRemarksHeaderOutPut', h.MainRemarks || '');
                    setVal('CmbJobOrderNoOutput', h.InvJobOrderId);
                    p = jobOrderLeave().then(function () { setVal('CmbPlantFeederOutput', h.PlantId); });
                }
                rows = (h.details || []).filter(function (d) { return String(ci(d, 'EntryType')) !== 'Issue'; }).map(function (d) {
                    if (String(ci(d, 'EntryType')) === 'FinishGoods' && num(ci(d, 'NetRate')) > 0) setVal('txtAvgFinishGoodRate', ci(d, 'NetRate'));
                    return { Id: ci(d, 'Id'), OutPutIdFromJobOrder: ci(d, 'OutPutIdFromJobOrder'), EntryType: ci(d, 'EntryType'), WareHouseId: ci(d, 'WarehouseId'), ItemId: ci(d, 'ItemId'), Item: ci(d, 'ItemName'),
                        ItemUOMId: ci(d, 'ItemUomId'), ItemUOM: ci(d, 'PackUom'), ItemEquivalent: ci(d, 'Equivalent'), CropYearId: ci(d, 'CropBatch'), CropYear: ci(d, 'CropBatch'), JobLotId: ci(d, 'JobLotId'), PackingTypeId: ci(d, 'PackingtypeId'),
                        Quantity: ci(d, 'Qty'), Weight: ci(d, 'Weight'), Rate: Math.round(num(ci(d, 'NetRate')) * 100) / 100, RateUOMId: ci(d, 'RateUOMId'), RateUOM: ci(d, 'RateUOM'), RateEquivalent: ci(d, 'RateEquivalent'),
                        ItemAmount: ci(d, 'TotalAmount'), NetRate: Math.round(num(ci(d, 'NetRate')) * 100) / 100, NetAmount: ci(d, 'TotalAmount'), Remarks: ci(d, 'Remarks'), IssueWeight: ci(d, 'IssueWeight') };
                });
                if (rows.length) { tab('OutPut'); subTab('OutPut', 'tabPage5'); }
                vhId = C.intOf(h.voucherHeadId);
                renderGrid();
                return p;
            }).catch(fail);
        }
        /** btnLoadOutPutFromjobOrder_Click:3270 -> LoadOutPutDataFromJobOrder:3292 */
        function loadOutput() {
            var jo = intOf('CmbJobOrderNoOutput');
            if (!jo) { $id('CmbJobOrderNoOutput').focus(); alert('Select Job Order No First...'); return; }
            loader.open('output', jo, 'Load OutPut - Pending Job Order Output Rows', function (picked) {
                picked.forEach(function (r) {
                    if (rows.some(function (x) { return C.intOf(x.OutPutIdFromJobOrder) === C.intOf(ci(r, 'Id')); })) return;   // :3307
                    rows.push({ Id: 0, OutPutIdFromJobOrder: ci(r, 'Id'), EntryType: ci(r, 'EntryType'), WareHouseId: ci(r, 'WareHouseId'), ItemId: ci(r, 'ItemId'), Item: ci(r, 'ItemName'),
                        ItemUOMId: ci(r, 'PackUomId'), ItemUOM: ci(r, 'OuterPackUom'), ItemEquivalent: ci(r, 'OuterPackUomEquivalent'), CropYearId: ci(r, 'CropYearId'), CropYear: ci(r, 'CropYear'), JobLotId: ci(r, 'JobLotId'),
                        PackingTypeId: ci(r, 'InvPackingTypeId'), Quantity: ci(r, 'balQty'), Weight: ci(r, 'balWeight'), Rate: ci(r, 'ItemRate'), RateUOMId: ci(r, 'RateUomId'), RateUOM: ci(r, 'RateUom'), RateEquivalent: ci(r, 'RateUomEquivalent'),
                        ItemAmount: ci(r, 'balAmount'), NetRate: ci(r, 'NetRate'), NetAmount: ci(r, 'balNetAmount'), Remarks: ci(r, 'RemarksDetail'), IssueWeight: ci(r, 'balWeight') });
                });
                renderGrid();
                $id('CmbJobOrderNoOutput').disabled = true;   // :3320
            });
        }
        /** btnPenForRateEntries_Click:3255 -> LoadOutPutPendingforRates; ReadByIdOutPut(OutPutDocId) */
        function pendingForRates() { pend.open(function (id) { readById(id); }); }
        /** btnPrintOutPut_Click:3205 */
        function print() { printRows(API + '/print/601?id=' + recId + '&entryType=Output&jobOrderId=' + intOf('CmbJobOrderNoOutput'), '601-InvFoodProductionSlip.rpt'); }
        /** OutputGridHistory:3073 */
        function history() {
            C.getJson(API + '/history?documentTypeId=112').then(function (rs) {
                var hr = (rs || []).map(histRow);
                renderGrid_('grdOutputHistory', histCols(true), hr, {
                    buttons: [{ text: 'Edit', cls: 'editbtn', click: function (r) {
                        if (String(r.IsApproved) === 'Approved') { alert("You Can't Update Approved Record"); return; }   // :3155
                        readById(C.intOf(r.Id));
                    } }],
                    onSelect: function (r) { detail(C.intOf(r.Id)); }
                });
            }).catch(fail);
        }
        /** OutPutDetailByHeaderId:7010 */
        function detail(id) {
            C.getJson(API + '/production/' + id).then(function (h) {
                var rr = rateRight();
                var dr = (h.details || []).filter(function (d) { return String(ci(d, 'EntryType')) !== 'Issue'; }).map(function (d) {
                    return { WareHouseName: ci(d, 'WareHouseName'), ItemName: ci(d, 'ItemName'), PackUom: ci(d, 'Equivalent'), CropYear: ci(d, 'CropBatch'), JobLot: ci(d, 'JobLotDescription'), PackingType: ci(d, 'PackTypeDesc'),
                        Qty: ci(d, 'Qty'), Weight: ci(d, 'Weight'), Rate: Math.round(num(ci(d, 'Rate')) * 100) / 100, RateUom: ci(d, 'RateUOM'), Amount: ci(d, 'Amount'), Remarks: ci(d, 'Remarks') };
                });
                renderGrid_('grdOutPutDetail', [{ key: 'WareHouseName' }, { key: 'ItemName' }, { key: 'PackUom' }, { key: 'CropYear' }, { key: 'JobLot' }, { key: 'PackingType' },
                    { key: 'Qty', num: true, fmt: fi, total: 'sum' }, { key: 'Weight', num: true, fmt: fi, total: 'sum' }, { key: 'Rate', num: true, hide: !rr }, { key: 'RateUom', hide: !rr },
                    { key: 'Amount', num: true, fmt: fi, total: 'sum', hide: !rr }, { key: 'Remarks' }], dr, { selectable: false });
            }).catch(fail);
        }
        return { renderGrid: renderGrid, serial: serial, jobOrderLeave: jobOrderLeave, refresh: refresh, reset: reset, save: save, update: update, readById: readById,
            loadOutput: loadOutput, pendingForRates: pendingForRates, print: print, history: history, get voucherHeadId() { return vhId; }, get recId() { return recId; },
            lockJobOrder: function () { $id('CmbJobOrderNoOutput').disabled = true; } };
    })();

    // =========================================================================================== PACKING MATERIAL (111)
    var pm = (function () {
        var rows = [], recId = 0, vhId = 0;
        function cols() {
            var rr = rateRight();
            return [
                { key: 'Id', hide: true }, { key: 'JobOrderPackingMaterialId', hide: true }, { key: 'DocDate', hide: true }, { key: 'DocNo', hide: true }, { key: 'InvProductionJobOrderId', hide: true },
                { key: 'PlantId', hide: true }, { key: 'Plant', cap: 'Plant' },
                { key: 'WarehouseId', cap: 'WareHouse Name', edit: 'select', opts: st.warehousesAll, ok: 'Id', tk: 'WareHouseName' },
                { key: 'PackingItemId', hide: true }, { key: 'PackingItem', cap: 'PackingItem' }, { key: 'BrandId', hide: true }, { key: 'Brand', cap: 'Brand' }, { key: 'BrandUomId', hide: true }, { key: 'BrandUom', cap: 'BrandUom', num: true },
                { key: 'ItemQty', cap: 'ItemQty', edit: 'num', fmt: fi, total: 'sum' }, { key: 'ItemRate', cap: 'ItemRate', edit: 'num', fmt: fi, total: 'avg', hide: !rr },
                { key: 'Amount', cap: 'Amount', num: true, fmt: fi, total: 'sum', hide: !rr }, { key: 'Remarks', cap: 'Remarks', edit: 'text' }
            ];
        }
        /** grdDetailPackingMaterialSettings:3811 - X and + always present; the handler refuses them in update mode (:3718 / :3731) */
        function renderGrid() {
            renderGrid_('grddetailPackingMaterial', cols(), rows, {
                buttons: [
                    { text: 'X', cls: 'xbtn', click: function (r, i) { if (!visible('btnsavepackingmaterial')) { alert('You Can not Delete Record In Update Mode'); return; } rows.splice(i, 1); renderGrid(); lock(); } },
                    { text: '+', cls: 'addbtn', click: function (r) { if (!visible('btnsavepackingmaterial')) { alert('You can not add row in Update Mode'); return; } var c = JSON.parse(JSON.stringify(r)); delete c.__sel; c.Id = 0; rows.push(c); renderGrid(); lock(); } }
                ],
                onChange: function (r, k) { if (k === 'ItemQty' || k === 'ItemRate') r.Amount = num(r.ItemQty) * num(r.ItemRate); }   // :4131
            });
        }
        /** JobOrderPackingMaterialLock:3351 */
        function lock() {
            if (rows.length) { $id('cmbJobOrderPackingMaterial').disabled = true; setVal('cmbJobOrderPackingMaterial', rows[0].InvProductionJobOrderId); }
            else $id('cmbJobOrderPackingMaterial').disabled = false;
        }
        /** GenerateCodeOfPackingMaterial:3371 */
        function nextCode() { return C.getJson(API + '/pm/next-code').then(function (r) { if (C.intOf(r.docNo) > 0) setVal('txtDocNoPackingMaterial', r.docNo); }).catch(fail); }
        /** cmbJobOrderPackingMaterial_Leave:3399 -> GetPlantFeeder + GetTotalInPutOutOnPM:3415 */
        function jobOrderLeave() {
            var jo = intOf('cmbJobOrderPackingMaterial');
            if (!jo) return Promise.resolve();
            return plants(jo, 'CmbPlantFeederPackingMaterial').then(function () { return totals(jo); }).then(function (s) {
                if (s.any) { setVal('txtTotalInputQtyPm', fi(s.inQty)); setVal('txtTotalInputWeightPm', fi(s.inW)); setVal('txtTotaloutputQtyPm', fi(s.outQty)); setVal('txtTotalOutPutWeightPm', fi(s.outW)); setVal('txtTotalFinishQtyPm', fi(s.fgQty)); setVal('txtTotalFinishWeightPm', fi(s.fgW)); }
                else ['txtTotalInputQtyPm', 'txtTotalInputWeightPm', 'txtTotaloutputQtyPm', 'txtTotalOutPutWeightPm', 'txtTotalFinishQtyPm', 'txtTotalFinishWeightPm'].forEach(function (id) { setVal(id, '0'); });
            }).catch(fail);
        }
        /** UpdatePlantAndJobOrderInPMGrid:3909 (CmbPlantFeederPackingMaterial_Leave:3479) */
        function plantLeave() {
            var jo = intOf('cmbJobOrderPackingMaterial'), pl = intOf('CmbPlantFeederPackingMaterial'), pt = selText('CmbPlantFeederPackingMaterial');
            rows.forEach(function (r) { r.InvProductionJobOrderId = jo; r.PlantId = pl; r.Plant = pt; });
            renderGrid();
        }
        /** FormResetOfPackingMaterial:3496 */
        function reset() {
            recId = 0; vhId = 0; rows = [];
            show('btnsavepackingmaterial', true); show('btnUpdatepackingmaterial', false);
            $id('cmbJobOrderPackingMaterial').disabled = false;
            nextCode();
            setVal('CmbPlantFeederPackingMaterial', ''); setVal('cmbJobOrderPackingMaterial', '');
            ['txtTotalFinishQtyPm', 'txtTotalFinishWeightPm', 'txtTotaloutputQtyPm', 'txtTotalOutPutWeightPm', 'txtTotalInputQtyPm', 'txtTotalInputWeightPm'].forEach(function (id) { setVal(id, ''); });
            renderGrid();
            $id('cmbJobOrderPackingMaterial').focus();
        }
        /** FormValidationOfPackingMaterial:3328 */
        function validate() {
            if (val('txtDocNoPackingMaterial').trim() === '') { alert('Doc No. Field Required'); return false; }
            if (!intOf('cmbJobOrderPackingMaterial')) { alert('Job Order No. Field Required'); $id('cmbJobOrderPackingMaterial').focus(); return false; }
            if (!intOf('CmbPlantFeederPackingMaterial')) { alert('Plant Name Field Required'); $id('CmbPlantFeederPackingMaterial').focus(); return false; }
            return true;
        }
        /** PackingMaterialInsert:3531 */
        function insert() {
            if (!validate()) return;
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var go = recId > 0 ? Promise.resolve() : nextCode();   // :3559
            go.then(function () {
                return C.postJson(API + '/pm/save', { id: recId, docNo: val('txtDocNoPackingMaterial'), docDate: val('txtdocdatePackingMaterial'), jobOrderId: intOf('cmbJobOrderPackingMaterial'), plantId: intOf('CmbPlantFeederPackingMaterial'), rows: rows });
            }).then(function (res) { alert(res.message); reset(); }).catch(fail);
        }
        function save() { recId = 0; insert(); }   // btnsavepackingmaterial_Click:3652
        function update() { insert(); }            // btnUpdatepackingmaterial_Click:3665
        /** ReadByIdPackingMaterial:3677 */
        function readById(id) {
            show('btnsavepackingmaterial', false); show('btnUpdatepackingmaterial', true);
            recId = id;
            return C.getJson(API + '/pm/' + id).then(function (o) {
                tab('PackingMaterial'); subTab('PackingMaterial', 'PmForm');
                setVal('cmbJobOrderPackingMaterial', ci(o, 'InvProductionJobOrderId'));
                return jobOrderLeave().then(function () {
                    setVal('CmbPlantFeederPackingMaterial', ci(o, 'PlantId'));
                    rows = [{ Id: ci(o, 'Id'), JobOrderPackingMaterialId: ci(o, 'JobOrderPackingMaterialId'), DocDate: isoOf(ci(o, 'DocDate')), DocNo: ci(o, 'DocNo'), InvProductionJobOrderId: ci(o, 'InvProductionJobOrderId'),
                        PlantId: ci(o, 'PlantId'), Plant: ci(o, 'PlantName'), WarehouseId: ci(o, 'WarehouseId'), PackingItemId: ci(o, 'ItemId'), PackingItem: ci(o, 'ItemName'), BrandId: ci(o, 'BrandId'), Brand: ci(o, 'BrandName'),
                        BrandUomId: ci(o, 'BrandUomId'), BrandUom: ci(o, 'BrandUom'), ItemQty: ci(o, 'Qty'), ItemRate: ci(o, 'Rate'), Amount: ci(o, 'Amount'), Remarks: ci(o, 'pmRemarks') || '' }];
                    vhId = C.intOf(o.voucherHeadId);
                    renderGrid(); lock();
                    $id('cmbJobOrderPackingMaterial').focus();
                });
            }).catch(fail);
        }
        /** btnLoadPMFromJobOrder_Click:3752 -> LoadPackingMaterialDataFromJobOrder:3774 */
        function load() {
            var jo = intOf('cmbJobOrderPackingMaterial');
            if (!jo) { $id('cmbJobOrderPackingMaterial').focus(); alert('Select Job Order No First...'); return; }
            loader.open('pm', jo, 'Load Packing Material - Pending Job Order Packing Material Rows', function (picked) {
                picked.forEach(function (r) {
                    if (rows.some(function (x) { return C.intOf(x.JobOrderPackingMaterialId) === C.intOf(ci(r, 'Id')); })) return;   // :3787
                    rows.push({ Id: 0, JobOrderPackingMaterialId: ci(r, 'Id'), DocDate: val('txtdocdatePackingMaterial'), DocNo: 0, InvProductionJobOrderId: ci(r, 'InvProductionJobOrderId'), PlantId: 0, Plant: '',
                        WarehouseId: ci(r, 'WareHouseId'), PackingItemId: ci(r, 'ItemId'), PackingItem: ci(r, 'PMItemName'), BrandId: ci(r, 'BrandId'), Brand: ci(r, 'BrandName'), BrandUomId: ci(r, 'BrandUomId'), BrandUom: ci(r, 'BrandUom'),
                        ItemQty: ci(r, 'balQty'), ItemRate: ci(r, 'Rate'), Amount: ci(r, 'balAmount'), Remarks: ci(r, 'pmRemarks') || '' });
                });
                renderGrid();
                $id('cmbJobOrderPackingMaterial').disabled = true;   // :3800
                lock(); plantLeave();   // :3802-3803
            });
        }
        /** btnPackingMaterialSlip_Click:4095 -> 606 ("PackingMaterial") for the current row's job order */
        function print() {
            var jo = rows.length ? C.intOf((rows.filter(function (r) { return r.__sel; })[0] || rows[0]).InvProductionJobOrderId) : intOf('cmbJobOrderPackingMaterial');
            printRows(API + '/print/606?reportType=PackingMaterial&jobOrderId=' + jo, '606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt');
        }
        /** BindHistoryPMGrid:3937 */
        function history() {
            var pending = $id('chkPendingForRates').checked;
            C.getJson(API + '/pm/history?pendingForRates=' + pending).then(function (rs) {
                var rr = rateRight();
                var hr = (rs || []).map(function (r) {
                    return { Id: ci(r, 'Id'), DocNo: ci(r, 'DocNo'), DocDate: dMMMyyyy(ci(r, 'DocDate')), JobOrderId: ci(r, 'InvProductionJobOrderId'), JobOrder: ci(r, 'RefInvoiceNo'), PlantName: ci(r, 'PlantName'),
                        WareHouseName: ci(r, 'WareHouseName'), Item: ci(r, 'ItemName'), Qty: ci(r, 'Qty'), Rate: ci(r, 'Rate'), Amount: ci(r, 'Amount'), BrandName: ci(r, 'BrandName'), BrandUom: ci(r, 'BrandUom'), Remarks: ci(r, 'pmRemarks'), IsApproved: ci(r, 'JobOrderStatus') };
                });
                if (!hr.length) { clearGrid('grdPackingMaterialHistory'); return; }
                renderGrid_('grdPackingMaterialHistory', [{ key: 'Id', hide: true }, { key: 'DocNo' }, { key: 'DocDate' }, { key: 'JobOrderId', hide: true }, { key: 'JobOrder' }, { key: 'PlantName' }, { key: 'WareHouseName' }, { key: 'Item' },
                    { key: 'Qty', num: true, fmt: f00, total: 'sum' }, { key: 'Rate', num: true, fmt: f00, total: 'sum', hide: !rr }, { key: 'Amount', num: true, fmt: f00, total: 'sum', hide: !rr }, { key: 'BrandName' }, { key: 'BrandUom' }, { key: 'Remarks' }, { key: 'IsApproved' }], hr, {
                    buttons: [{ text: 'Edit', cls: 'editbtn', click: function (r) {
                        /* grdPackingMaterialHistory_ColumnButtonClick:4075 */
                        show('btnsavepackingmaterial', false); show('btnUpdatepackingmaterial', true);
                        if (String(r.IsApproved) !== 'Approved') { readById(C.intOf(r.Id)); return; }
                        alert("You Can't Update Approved Record");
                    } }]
                });
            }).catch(fail);
        }
        return { renderGrid: renderGrid, nextCode: nextCode, jobOrderLeave: jobOrderLeave, plantLeave: plantLeave, reset: reset, save: save, update: update, readById: readById, load: load, print: print, history: history,
            get voucherHeadId() { return vhId; } };
    })();

    // =========================================================================================== OVER HEAD (110)
    var oh = (function () {
        var rows = [], recId = 0, vhId = 0;
        function cols() {
            return [
                { key: 'Id', hide: true }, { key: 'JobOrderOverHeadId', hide: true }, { key: 'DocDate', hide: true }, { key: 'DocNo', hide: true }, { key: 'InvProductionJobOrderId', hide: true },
                { key: 'PlantId', hide: true }, { key: 'Plant', cap: 'Plant' },
                { key: 'ChargesTypeId', cap: 'Charges Type', edit: 'select', opts: st.init ? st.init.chargesTypes : [], ok: 'Id', tk: 'Type' },
                { key: 'ChartOfAccountId', hide: true }, { key: 'ChartOfAccount', cap: 'ChartOfAccount' }, { key: 'BrandId', hide: true }, { key: 'Brand', cap: 'Brand' }, { key: 'BrandUomId', hide: true }, { key: 'BrandUom', cap: 'BrandUom', num: true },
                { key: 'ItemQty', cap: 'ItemQty', edit: 'num', fmt: fi, total: 'sum' }, { key: 'ItemRate', cap: 'ItemRate', edit: 'num', fmt: fRate, total: 'avg' },
                { key: 'RateUomId', cap: 'Rate Uom', edit: 'select', opts: st.uomStatic, ok: 'Id', tk: 'UOM' },
                { key: 'Amount', cap: 'Amount', edit: 'num', fmt: fAmt, total: 'sum' }, { key: 'Remarks', cap: 'Remarks', edit: 'text' }
            ];
        }
        /** grdDetailOverheadSettings:4526 */
        function renderGrid() {
            var c = cols();
            /* Amount is read-only for Processing Charges (:4688) and editable for Other Charges (:4697) - per row; the
               grid-wide column follows the selected / first row as the desktop's single column does */
            var first = rows.filter(function (r) { return r.__sel; })[0] || rows[0];
            if (first && C.intOf(first.ChargesTypeId) === 1) { var a = c.filter(function (x) { return x.key === 'Amount'; })[0]; delete a.edit; a.num = true; }
            renderGrid_('grdDetailOverhead', c, rows, {
                buttons: [
                    { text: 'X', cls: 'xbtn', click: function (r, i) { if (!visible('btnSaveOverHead')) { alert('You Can not Delete Record In Update Mode'); return; } rows.splice(i, 1); renderGrid(); lock(); } },
                    { text: '+', cls: 'addbtn', click: function (r) { if (!visible('btnSaveOverHead')) { alert('You can not add row in Update Mode'); return; } var x = JSON.parse(JSON.stringify(r)); delete x.__sel; x.Id = 0; rows.push(x); renderGrid(); lock(); } }
                ],
                onChange: cellUpdated
            });
        }
        /** grdDetailOverhead_CellUpdated:4656 */
        function cellUpdated(r, k) {
            if (k !== 'ChargesTypeId' && k !== 'ItemQty' && k !== 'ItemRate' && k !== 'RateUomId') return;
            var qty = num(r.ItemQty), rate = num(r.ItemRate);
            if (C.intOf(r.ChargesTypeId) === 1) {
                if (C.intOf(r.BrandId) > 0 && C.intOf(r.BrandUomId) > 0) {
                    if (qty !== 0 && rate !== 0) r.Amount = Math.round(qty * rate);
                } else if (qty !== 0 && rate !== 0 && num(r.RateUomId) !== 0) {
                    var inW = num(String(val('txtInputWeightTotalOH')).replace(/,/g, ''));
                    var u = st.uomStatic.filter(function (x) { return C.intOf(ci(x, 'Id')) === C.intOf(r.RateUomId); })[0];
                    var eq = u ? num(ci(u, 'Equivalent')) : 0;
                    r.Amount = Math.round(inW / eq * rate);
                    if (!isFinite(r.Amount)) r.Amount = 0;
                }
            } else if (qty !== 0 && rate !== 0) r.Amount = Math.round(qty * rate);
        }
        /** JobOrderOverHeadLock:4506 */
        function lock() {
            if (rows.length) { $id('cmbJobOrderOverHead').disabled = true; setVal('cmbJobOrderOverHead', rows[0].InvProductionJobOrderId); }
            else $id('cmbJobOrderOverHead').disabled = false;
        }
        /** GenerateCodeofOverHead:4170 */
        function nextCode() { return C.getJson(API + '/oh/next-code').then(function (r) { if (C.intOf(r.docNo) > 0) setVal('txtDocNoOverHead', r.docNo); }).catch(fail); }
        /** cmbJobOrderOverHead_Leave:4198 -> GetPlantFeeder + GetTotlInPutOut:4211 */
        function jobOrderLeave() {
            var jo = intOf('cmbJobOrderOverHead');
            return plants(jo, 'CmbPlantFeederOverHead').then(function () { return totals(jo); }).then(function (s) {
                if (s.any) { setVal('txtInputQtyTotalOH', fi(s.inQty)); setVal('txtInputWeightTotalOH', fi(s.inW)); setVal('txtOutputQtyTotalOH', fi(s.outQty)); setVal('txtOutputWeightTotalOH', fi(s.outW)); setVal('txtfinishQtyOh', fi(s.fgQty)); setVal('txtfinishWeightOh', fi(s.fgW)); }
                else ['txtInputQtyTotalOH', 'txtInputWeightTotalOH', 'txtOutputQtyTotalOH', 'txtOutputWeightTotalOH', 'txtfinishQtyOh', 'txtfinishWeightOh'].forEach(function (id) { setVal(id, '0'); });
            }).catch(fail);
        }
        /** CmbPlantFeederOverHead_Leave:4949 -> UpdatePlantAndJobOrderInOHGrid:4708 */
        function plantLeave() {
            var jo = intOf('cmbJobOrderOverHead'), pl = intOf('CmbPlantFeederOverHead'), pt = selText('CmbPlantFeederOverHead');
            rows.forEach(function (r) { r.InvProductionJobOrderId = jo; r.PlantId = pl; r.Plant = pt; });
            renderGrid();
        }
        /** FormResetOverHead:4479 */
        function reset() {
            recId = 0; vhId = 0; rows = [];
            show('btnUpdateOverHead', false); show('btnSaveOverHead', true);
            $id('cmbJobOrderOverHead').disabled = false;
            ['txtOutputQtyTotalOH', 'txtOutputWeightTotalOH', 'txtInputQtyTotalOH', 'txtInputWeightTotalOH'].forEach(function (id) { setVal(id, ''); });
            setVal('cmbJobOrderOverHead', ''); nextCode(); setVal('CmbPlantFeederOverHead', '');
            renderGrid();
            $id('cmbJobOrderOverHead').focus();
        }
        /** FormValidationOfOverHead:4954 */
        function validate() {
            if (val('txtDocNoOverHead').trim() === '') { alert('Doc No. Field Required'); return false; }
            if (!intOf('cmbJobOrderOverHead')) { alert('Job Order No. Field Required'); $id('cmbJobOrderOverHead').focus(); return false; }
            if (!intOf('CmbPlantFeederOverHead')) { alert('Plant Field Required'); $id('CmbPlantFeederOverHead').focus(); return false; }
            return true;
        }
        /** OverHeadInsert:4275 */
        function insert() {
            if (!validate()) return;
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var go = recId > 0 ? Promise.resolve() : nextCode();   // :4303
            go.then(function () {
                return C.postJson(API + '/oh/save', { id: recId, docNo: val('txtDocNoOverHead'), jobOrderId: intOf('cmbJobOrderOverHead'), plantId: intOf('CmbPlantFeederOverHead'), rows: rows });
            }).then(function (res) { alert(res.message); reset(); }).catch(fail);
        }
        function save() { recId = 0; insert(); }   // btnSaveOverHead_Click:4407
        function update() { insert(); }            // btnUpdateOverHead_Click:4420
        /** ReadByIdOverHeadJobOrderWise:4432 - RecOverHeadId is the job order id (:4844) */
        function readByJobOrder(jobOrderId) {
            show('btnSaveOverHead', false); show('btnUpdateOverHead', true);
            recId = jobOrderId;
            return C.getJson(API + '/oh/by-job-order/' + jobOrderId).then(function (o) {
                var rs = o.rows || [];
                if (!rs.length) throw new Error('Record Not Found');
                tab('OverHead'); subTab('OverHead', 'tabPage2');
                setVal('cmbJobOrderOverHead', ci(rs[0], 'InvProductionJobOrderId'));
                return jobOrderLeave().then(function () {
                    setVal('CmbPlantFeederOverHead', ci(rs[0], 'PlantId'));
                    rows = rs.map(function (d) {
                        return { Id: ci(d, 'Id'), JobOrderOverHeadId: ci(d, 'JobOrderOverHeadId'), DocDate: isoOf(ci(d, 'DocDate')), DocNo: ci(d, 'DocNo'), InvProductionJobOrderId: ci(d, 'InvProductionJobOrderId'),
                            PlantId: ci(d, 'PlantId'), Plant: ci(d, 'PlantName'), ChargesTypeId: ci(d, 'ChargesTypeId'), ChartOfAccountId: ci(d, 'CharOfAccountId'), ChartOfAccount: ci(d, 'AccountTitle'), BrandId: ci(d, 'BrandId'), Brand: ci(d, 'BrandName'),
                            BrandUomId: ci(d, 'BrandUomId'), BrandUom: ci(d, 'BrandUom'), ItemQty: ci(d, 'ItemQty'), ItemRate: ci(d, 'ItemRate'), RateUomId: ci(d, 'RateUomId'), Amount: ci(d, 'Amount'), Remarks: ci(d, 'ohRemarks') || '' };
                    });
                    vhId = C.intOf(o.voucherHeadId);
                    renderGrid();
                    $id('cmbJobOrderOverHead').focus();
                });
            }).catch(fail);
        }
        /** btnLoadOverHeadsFromJobOrder_Click:5060 -> LoadOverHeadsDataFromJobOrder:5082 */
        function load() {
            var jo = intOf('cmbJobOrderOverHead');
            if (!jo) { $id('cmbJobOrderOverHead').focus(); alert('Select Job Order No First...'); return; }
            loader.open('oh', jo, 'Load Over Heads - Job Order Over Head Rows', function (picked) {
                picked.forEach(function (r) {
                    if (rows.some(function (x) { return C.intOf(x.JobOrderOverHeadId) === C.intOf(ci(r, 'Id')); })) return;   // :5097
                    rows.push({ Id: 0, JobOrderOverHeadId: ci(r, 'Id'), DocDate: today(), DocNo: 0, InvProductionJobOrderId: ci(r, 'InvProductionJobOrderId'), PlantId: 0, Plant: '', ChargesTypeId: 0,
                        ChartOfAccountId: ci(r, 'CharOfAccountId'), ChartOfAccount: ci(r, 'OverHeadAccount'), BrandId: ci(r, 'BrandId'), Brand: ci(r, 'BrandName'), BrandUomId: ci(r, 'BrandUomId'), BrandUom: ci(r, 'BrandUom'),
                        ItemQty: ci(r, 'ItemQty'), ItemRate: ci(r, 'ItemRate'), RateUomId: 0, Amount: ci(r, 'Amount'), Remarks: ci(r, 'ohRemarks') || '' });
                });
                renderGrid();
                $id('cmbJobOrderOverHead').disabled = true;   // :5110
            });
        }
        /** btnPrintOverHead_Click:5006 -> 606 ("OverHead") */
        function print() {
            var jo = rows.length ? C.intOf((rows.filter(function (r) { return r.__sel; })[0] || rows[0]).InvProductionJobOrderId) : intOf('cmbJobOrderOverHead');
            printRows(API + '/print/606?reportType=OverHead&jobOrderId=' + jo, '606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt');
        }
        /** BindGridHistoryOverHead:4736 */
        function history() {
            C.getJson(API + '/oh/history').then(function (hr) {
                hr = hr || [];
                if (!hr.length) { clearGrid('grdOverHeadHistory'); clearGrid('grddetailOhHistory'); return; }
                renderGrid_('grdOverHeadHistory', [{ key: 'InvProductionJobOrderId', hide: true }, { key: 'ProductionJobOrder' }, { key: 'TotalItemQty', num: true, fmt: fi, total: 'sum' }, { key: 'TotalExpense', num: true, fmt: fi, total: 'sum' },
                    { key: 'AverageItemRate', num: true, fmt: fp, total: 'avg' }, { key: 'IsApproved' }], hr, {
                    buttons: [{ text: 'Edit', cls: 'editbtn', click: function (r) {
                        /* grdOverHeadHistory_ColumnButtonClick:4929 */
                        show('btnSaveOverHead', false); show('btnUpdateOverHead', true);
                        if (String(r.IsApproved) !== 'Approved') { readByJobOrder(C.intOf(r.InvProductionJobOrderId)); return; }
                        alert("You Can't Update Approved Record");
                    } }],
                    onSelect: function (r) { detail(C.intOf(r.InvProductionJobOrderId)); }
                });
            }).catch(fail);
        }
        /** grdOverHeadHistory_SelectionChanged:4856 */
        function detail(jobOrderId) {
            C.getJson(API + '/oh/by-job-order/' + jobOrderId).then(function (o) {
                var dr = (o.rows || []).map(function (d) {
                    return { Id: ci(d, 'Id'), InvProductionJobOrderId: ci(d, 'InvProductionJobOrderId'), DocNo: ci(d, 'DocNo'), DocDate: dMMMyyyy(ci(d, 'DocDate')), JobOrderNo: ci(d, 'PlanCode'), PlantName: ci(d, 'PlantName'),
                        ChargesType: ci(d, 'ChargesType'), GLAccount: ci(d, 'AccountTitle'), BrandName: ci(d, 'BrandName'), BrandUom: ci(d, 'BrandUom'), ItemQty: ci(d, 'ItemQty'), ItemRate: ci(d, 'ItemRate'), Amount: ci(d, 'Amount'), Remarks: ci(d, 'ohRemarks'), IsApproved: ci(d, 'JobOrderStatus') };
                });
                renderGrid_('grddetailOhHistory', [{ key: 'Id', hide: true }, { key: 'InvProductionJobOrderId', hide: true }, { key: 'DocNo' }, { key: 'DocDate' }, { key: 'JobOrderNo' }, { key: 'PlantName' }, { key: 'ChargesType' }, { key: 'GLAccount' }, { key: 'BrandName' }, { key: 'BrandUom' },
                    { key: 'ItemQty', num: true, fmt: fi, total: 'sum' }, { key: 'ItemRate', num: true, fmt: fp, total: 'sum' }, { key: 'Amount', num: true, fmt: fi, total: 'sum' }, { key: 'Remarks' }, { key: 'IsApproved' }], dr, { selectable: false });
            }).catch(fail);
        }
        return { renderGrid: renderGrid, nextCode: nextCode, jobOrderLeave: jobOrderLeave, plantLeave: plantLeave, reset: reset, save: save, update: update, readByJobOrder: readByJobOrder, load: load, print: print, history: history,
            get voucherHeadId() { return vhId; } };
    })();

    // =========================================================================================== TRANSACTION HISTORY
    var th = (function () {
        var hrows = [], loading = false;
        /** GridHistory:5118 */
        function gridHistory() {
            if (loading) return;
            loading = true;
            C.getJson(API + '/history?documentTypeId=0').then(function (rs) {
                hrows = (rs || []).map(function (r) {
                    return { Id: ci(r, 'Id'), DocSrNo: ci(r, 'DocCode'), DocDate: dMMMyyyy(ci(r, 'DocDate')), JobOrderNo: ci(r, 'InvJobOrderNo'), EntryType: ci(r, 'EntryType'), Remarks: ci(r, 'MainRemarks'),
                        JobOrderStatus: ci(r, 'JobOrderApprovedStatus'), StartDate: dMMMyyyy(ci(r, 'StartDate')), EndDate: dMMMyyyy(ci(r, 'EndDate')), EntryDate: dTime(ci(r, 'EntryDate')), EntryUser: ci(r, 'EntryUser'),
                        ModifyDate: dTime(ci(r, 'ModifyDate')), ModifyUser: ci(r, 'ModifyUser'), PlanStatus: ci(r, 'PlanStatus') };
                });
                if (!hrows.length) { clearGrid('DatagridHistory'); return; }
                /* GridSetting:5172 - the Edit column is appended (no Position) */
                var c = histCols(false).concat([{ key: '__edit', cap: 'Edit', fmt: function () { return 'Edit'; }, link: edit }]);
                renderGrid_('DatagridHistory', c, hrows, { onSelect: function (r) { detail(C.intOf(r.Id)); } });
            }).catch(fail).then(function () { loading = false; });
        }
        /** DatagridHistory_ColumnButtonClick:5193 */
        function edit(r) {
            if (String(r.JobOrderStatus) === 'Approved') { alert("You Can't Update Approved Record"); return; }
            if (String(r.EntryType) === 'Input') { inp.lockJobOrder(); inp.readById(C.intOf(r.Id)); }
            else { out.lockJobOrder(); out.readById(C.intOf(r.Id)); }
        }
        /** DatagridHistoryDetail:5223 */
        function detail(id) {
            C.getJson(API + '/production/' + id).then(function (h) {
                var dr = (h.details || []).map(function (d) {
                    return { EntryType: ci(d, 'EntryType'), WareHouse: ci(d, 'WareHouseName'), Item: ci(d, 'ItemName'), JobLot: ci(d, 'JobLotCode'), CropBatch: ci(d, 'CropBatch'), PackType: ci(d, 'PackTypeCode'),
                        Qty: ci(d, 'Qty'), Weight: ci(d, 'Weight'), Order: ci(d, 'InvProductionJobOrderNo'), Remarks: ci(d, 'Remarks') };
                });
                renderGrid_('grdDetail', [{ key: 'EntryType' }, { key: 'WareHouse' }, { key: 'Item' }, { key: 'JobLot' }, { key: 'CropBatch' }, { key: 'PackType' },
                    { key: 'Qty', num: true, fmt: fi, total: 'sum' }, { key: 'Weight', num: true, fmt: fi, total: 'sum' }, { key: 'Order' }, { key: 'Remarks' }], dr, { selectable: false });
            }).catch(fail);
        }
        function currentId() { var r = hrows.filter(function (x) { return x.__sel; })[0]; return r ? C.intOf(r.Id) : 0; }
        function current() { return hrows.filter(function (x) { return x.__sel; })[0]; }
        return { gridHistory: gridHistory, edit: edit, detail: detail, currentId: currentId, current: current };
    })();

    // =========================================================================================== SUMMERY
    var sum = (function () {
        /** toolStripButton1_Click:5326 */
        function reset() { setVal('cmbsummeryJobOrderNo', ''); clearGrid('grdSummery'); clearGrid('grdSummeryReport'); $id('cmbsummeryJobOrderNo').focus(); }
        /** cmbsummeryJobOrderNo_TextChanged:5491 (GetPlantByJobOrderId -> CmbPlantNameSummary) */
        function jobOrderChanged() { var jo = intOf('cmbsummeryJobOrderNo'); if (jo) plants(jo, 'CmbPlantNameSummary').catch(fail); }
        /** btnShow_Click:5353 - GrdSummaryValuesFill, GrdSummaryReportFill, grdDocWiseSummeryFill */
        function show_() {
            var jo = intOf('cmbsummeryJobOrderNo'), pl = intOf('CmbPlantNameSummary'), rr = rateRight();
            C.getJson(API + '/summary-values?jobOrderId=' + jo).then(function (rs) {
                if (rs && rs.length) renderGrid_('grdSummery', allCols(rs, rr ? [] : ['AvgRate', 'Amount'], { NetQty: { fmt: f2 }, NetWeight: { fmt: f2 }, AvgRate: { fmt: fRate }, Amount: { fmt: fAmt } }), rs, { selectable: false });
                else clearGrid('grdSummery');
            }).catch(fail);
            C.getJson(API + '/summary/report?jobOrderId=' + jo + '&plantId=' + pl).then(function (rs) {
                if (!rs || !rs.length) { clearGrid('grdSummeryReport'); return; }
                var hidden = ['TotalAmountBySortNo', 'TranRate', 'TotalWeightBySortNo', 'RmWeightTotal', 'BpWeightTotal', 'FgWeightTotal'];
                if (!rr) hidden.push('Amount');
                renderGrid_('grdSummeryReport', allCols(rs, hidden, { NetQty: { fmt: f00, total: 'sum', tfmt: fp }, NetWeight: { fmt: f00, total: 'sum', tfmt: fp }, Amount: { fmt: f00, total: 'sum', tfmt: fp }, OutPutPrcnt: { fmt: fp, total: 'sum' } }), rs, { selectable: false, groupBy: 'TranType' });
            }).catch(fail);
            C.getJson(API + '/summary/doc-wise?jobOrderId=' + jo + '&plantId=' + pl).then(function (rs) {
                if (!rs || !rs.length) { clearGrid('grdDocWiseSummeryReport'); return; }
                var dr = rs.map(function (r) {
                    return { SortNo: ci(r, 'SortNo'), JO_PlanCode: ci(r, 'PlanCode'), JO_PlanDate: dShort(ci(r, 'PlanDate')), ProductionJobOrderNo: ci(r, 'ReferenceReportNo'), PlantName: ci(r, 'PlantName'), DocNo: ci(r, 'DocCode'), DocDate: dShort(ci(r, 'DocDate')),
                        TranType: ci(r, 'TranType'), ItemName: ci(r, 'ItemName'), PackUom: ci(r, 'UOMCode'), CropYear: ci(r, 'CropBatch'), Remarks: ci(r, 'remarks'), JobLotCode: ci(r, 'JobLotCode'), NetQty: ci(r, 'NetQty'), NetWeight: ci(r, 'NetWeight'),
                        Amount: ci(r, 'Amount'), TranRate: ci(r, 'TranRate'), WeightPrcnt: ci(r, 'WeightPrcnt'), NetRate: ci(r, 'NetRate') };
                });
                renderGrid_('grdDocWiseSummeryReport', [{ key: 'SortNo' }, { key: 'JO_PlanCode' }, { key: 'JO_PlanDate' }, { key: 'ProductionJobOrderNo' }, { key: 'PlantName' }, { key: 'DocNo' }, { key: 'DocDate' }, { key: 'TranType' }, { key: 'ItemName' }, { key: 'PackUom' }, { key: 'CropYear' }, { key: 'Remarks' }, { key: 'JobLotCode' },
                    { key: 'NetQty', num: true, fmt: f2, total: 'sum' }, { key: 'NetWeight', num: true, fmt: f2, total: 'sum' }, { key: 'Amount', num: true, fmt: fAmt, total: 'sum', hide: !rr }, { key: 'TranRate', num: true, fmt: fRate, total: 'avg', hide: !rr },
                    { key: 'WeightPrcnt', num: true, fmt: f2, total: 'sum' }, { key: 'NetRate', num: true, fmt: fRate, total: 'avg', hide: !rr }], dr, { selectable: false });
            }).catch(fail);
        }
        /** btnProductionRecoveryReport_Click:5756 -> CommonServices.ProductionRecoveryReport602(JobOrderId) (602 + its hours sub-report) */
        function print602() { var jo = intOf('cmbsummeryJobOrderNo'); if (!jo) { alert('Record Not Found For DisPlay'); return; } window.CrystalPrint.open('pc-602-row', { id: jo }, 'btnProductionRecoveryReport'); }
        /** btn607ProductionIssuanceGrnWiseReport_Click:5723 */
        function print607() { var jo = intOf('cmbsummeryJobOrderNo'); if (!jo) return; printRows(API + '/print/607?jobOrderId=' + jo, '607-FoodProductionIssuanceGrnWiseByJobOrderId.rpt'); }
        return { reset: reset, jobOrderChanged: jobOrderChanged, show: show_, print602: print602, print607: print607 };
    })();

    // =========================================================================================== SETTLEMENT (142)
    var stl = (function () {
        var dtInput = [], dtOutput = [], avgRate = 0, avgRateWoExp = 0;
        var OUT_COLS = function () { return [
            { key: 'Id', hide: true }, { key: 'DetailId', hide: true }, { key: 'WIPAccountId', hide: true }, { key: 'EntryType' }, { key: 'DocDate' }, { key: 'DocNo', link: function (r) { out.lockJobOrder(); out.readById(C.intOf(r.Id)).then(function () { tab('OutPut'); $id('DocDateOutPut').focus(); }); } },
            { key: 'WareHouseId', hide: true }, { key: 'WareHouse' }, { key: 'ItemId', hide: true }, { key: 'ItemName' }, { key: 'ItemUOMId', hide: true }, { key: 'ItemUOM' }, { key: 'CropYear' }, { key: 'JobLotId', hide: true }, { key: 'JobLot' }, { key: 'PackingTypeId', hide: true }, { key: 'PackingType' },
            { key: 'Quantity', num: true, fmt: f3, total: 'sum' }, { key: 'Weight', num: true, fmt: f3, total: 'sum' }, { key: 'Rate', edit: 'num', fmt: fRate, total: 'avg' }, { key: 'ItemAmount', num: true, fmt: fAmt, total: 'sum' },
            { key: 'RateWithoutExp', num: true, fmt: fRate, total: 'avg' }, { key: 'AmountWithoutExp', num: true, fmt: fAmt, total: 'sum' }, { key: 'NetRate', num: true, fmt: fRate, total: 'avg' }, { key: 'RateUOMId', hide: true }, { key: 'RateUOM' },
            { key: 'ItemPmCost', num: true, fmt: fAmt, total: 'sum' }, { key: 'GeneralPmCost', num: true, fmt: fAmt, total: 'sum' }, { key: 'ItemOhCost', num: true, fmt: fAmt, total: 'sum' }, { key: 'GeneralOhCost', num: true, fmt: fAmt, total: 'sum' }, { key: 'TotalAmount', num: true, fmt: fAmt, total: 'sum' }, { key: 'Remarks' }
        ]; };
        function renderOut() { renderGrid_('grdOutPutSettlement', OUT_COLS(), dtOutput, { selectable: false, onChange: outCellUpdated }); }
        /** grdOutPutSettlement_CellUpdated:6341 - Rate: the 40-KG rate UOM of the item, Amount = Weight / RateUOM * Rate */
        function outCellUpdated(r, k) {
            if (k !== 'Rate') return;
            var u = st.uomSchedules.filter(function (x) { return C.intOf(ci(x, 'ItemId')) === C.intOf(r.ItemId) && C.intOf(ci(x, 'Equivalent')) === 40; })[0];
            if (u) { r.RateUOMId = C.intOf(ci(u, 'Id')); r.RateUOM = C.intOf(ci(u, 'Equivalent')); }
            r.ItemAmount = num(r.Weight) / num(r.RateUOM) * num(r.Rate);
            if (!isFinite(r.ItemAmount)) r.ItemAmount = 0;
        }
        /** btnGeneralSettlement_Click:5768 */
        function generate() {
            var jo = intOf('CmbJobOrderSettlement');
            if (!jo) { alert('JobOrder Settlement Field is Required'); $id('CmbJobOrderSettlement').focus(); return; }
            C.getJson(API + '/settlement?jobOrderId=' + jo + '&activity=InPutOutPut').then(function (rs) {
                dtInput = []; dtOutput = [];
                (rs || []).forEach(function (r) {
                    if (String(ci(r, 'EntryType')) === 'Input') dtInput.push({ Id: ci(r, 'Id'), RefDocumentTypeId: ci(r, 'RefDocumentTypeId'), RefDocIdNo: ci(r, 'RefDocNoId'), DocDate: dMMMyy(ci(r, 'DocDate')), DocNo: ci(r, 'DocCode'), WareHouseId: ci(r, 'WarehouseId'), WareHouse: ci(r, 'WareHouseName'),
                        ItemId: ci(r, 'ItemId'), ItemName: ci(r, 'ItemName'), ItemUOMId: ci(r, 'ItemUomId'), ItemUOM: ci(r, 'ItemUom'), CropYear: ci(r, 'CropBatch'), JobLotId: ci(r, 'JobLotId'), JobLot: ci(r, 'JobLotDescription'), PackingTypeId: ci(r, 'PackingtypeId'), PackingType: ci(r, 'PackTypeDesc'),
                        Quantity: ci(r, 'Qty'), Weight: ci(r, 'Weight'), Rate: ci(r, 'Rate'), RateUOMId: ci(r, 'RateUOMId'), RateUOM: ci(r, 'RateUom'), ItemAmount: ci(r, 'Amount'), Remarks: ci(r, 'Remarks') });
                    if (String(ci(r, 'EntryType')) === 'Output') dtOutput.push({ Id: ci(r, 'Id'), DetailId: ci(r, 'DetailId'), WIPAccountId: ci(r, 'WIPAccountId'), EntryType: ci(r, 'EntryTypeDetail'), DocDate: dMMMyy(ci(r, 'DocDate')), DocNo: ci(r, 'DocCode'), WareHouseId: ci(r, 'WarehouseId'), WareHouse: ci(r, 'WareHouseName'),
                        ItemId: ci(r, 'ItemId'), ItemName: ci(r, 'ItemName'), ItemUOMId: ci(r, 'ItemUomId'), ItemUOM: ci(r, 'ItemUom'), CropYear: ci(r, 'CropBatch'), JobLotId: ci(r, 'JobLotId'), JobLot: ci(r, 'JobLotDescription'), PackingTypeId: ci(r, 'PackingtypeId'), PackingType: ci(r, 'PackTypeDesc'),
                        Quantity: ci(r, 'Qty'), Weight: ci(r, 'Weight'), Rate: ci(r, 'Rate'), ItemAmount: ci(r, 'Amount'), RateWithoutExp: ci(r, 'Rate'), AmountWithoutExp: ci(r, 'Amount'), NetRate: ci(r, 'NetRate'), RateUOMId: ci(r, 'RateUOMId'), RateUOM: ci(r, 'RateUom'),
                        ItemPmCost: ci(r, 'ItemPmCost'), GeneralPmCost: ci(r, 'GeneralPmCost'), ItemOhCost: ci(r, 'ItemOhCost'), GeneralOhCost: ci(r, 'GeneralOhCost'), TotalAmount: ci(r, 'TotalAmount'), Remarks: ci(r, 'Remarks') });
                });
                if (rs && rs.length) {
                    renderGrid_('grdInputsettlement', [{ key: 'Id', hide: true }, { key: 'RefDocumentTypeId', hide: true }, { key: 'RefDocIdNo', hide: true }, { key: 'DocDate' }, { key: 'DocNo', link: function (r) { inp.lockJobOrder(); inp.readById(C.intOf(r.Id)).then(function () { tab('InPut'); $id('txtDocdate').focus(); }); } },
                        { key: 'WareHouseId', hide: true }, { key: 'WareHouse' }, { key: 'ItemId', hide: true }, { key: 'ItemName' }, { key: 'ItemUOMId', hide: true }, { key: 'ItemUOM' }, { key: 'CropYear' }, { key: 'JobLotId', hide: true }, { key: 'JobLot' }, { key: 'PackingTypeId', hide: true }, { key: 'PackingType' },
                        { key: 'Quantity', num: true, fmt: fi, total: 'sum' }, { key: 'Weight', num: true, fmt: fi, total: 'sum' }, { key: 'Rate', num: true, fmt: fRate, total: 'avg' }, { key: 'RateUOMId', hide: true }, { key: 'RateUOM' }, { key: 'ItemAmount', num: true, fmt: fAmt, total: 'sum' }, { key: 'Remarks' }], dtInput, { selectable: false });
                    renderOut();
                } else { clearGrid('grdInputsettlement'); clearGrid('grdOutPutSettlement'); }
                return C.getJson(API + '/settlement?jobOrderId=' + jo + '&activity=OverHeads');
            }).then(function (ohr) {
                if (ohr && ohr.length) renderGrid_('grdOverHeadSettlement', allCols(ohr, ['Id', 'CharOfAccountId', 'DocumentTypeId', 'BrandId', 'BrandUomId'], { ItemQty: { fmt: fi, total: 'sum' }, Amount: { fmt: fAmt, total: 'sum' }, ItemRate: { fmt: fRate, total: 'avg' } })
                    .map(function (c) { if (c.key === 'DocNo') c.link = function () { oh.readByJobOrder(jo); }; return c; }), ohr, { selectable: false });   // grdOverHeadSettlement_LinkClicked:6315
                return C.getJson(API + '/settlement?jobOrderId=' + jo + '&activity=PackingMaterial');
            }).then(function (pmr) {
                if (pmr && pmr.length) renderGrid_('grdPackingMaterialSettlement', allCols(pmr, ['Id', 'ItemId', 'itemUomSchId', 'DocumentTypeId', 'ItemUom', 'BrandId', 'BrandUomId'], { Qty: { fmt: fi, total: 'sum' }, Amount: { fmt: fAmt, total: 'sum' }, Rate: { fmt: fRate, total: 'avg' } })
                    .map(function (c) { if (c.key === 'DocNo') c.link = function (r) { pm.readById(C.intOf(ci(r, 'Id'))); }; return c; }), pmr, { selectable: false });   // grdPackingMaterialSettlement_LinkClicked:6328
                else clearGrid('grdPackingMaterialSettlement');
                /* :5916 - FG weight and FG amount at job-order rates */
                var netWeight = 0, fgAgainstJo = 0;
                dtOutput.forEach(function (r) { if (String(r.EntryType) === 'FinishGoods') { netWeight += num(r.Weight); fgAgainstJo += num(r.ItemAmount); } });
                return C.getJson(API + '/summary-values?jobOrderId=' + jo + '&netWeight=' + netWeight).then(function (sv) { fillSummary(sv || [], fgAgainstJo); });
            }).catch(fail);
        }
        /** :5934-6011 */
        function fillSummary(sv, fgAgainstJo) {
            sv.forEach(function (r) {
                var t = String(ci(r, 'TranType'));
                if (t === 'Input Raw Material') { setVal('txtItemQtyRM', r0(ci(r, 'NetQty'))); setVal('txtNetWeightRM', r0(ci(r, 'NetWeight'))); setVal('txtAmountRM', r0(ci(r, 'Amount'))); }
                if (t === 'Packing Material') setVal('txtPackingMaterialSettlement', r0(ci(r, 'Amount')));
                if (t === 'Over Heads') setVal('txtOverHeadsSettlement', r0(ci(r, 'Amount')));
                if (t === 'OutPut By Product') {
                    var bp = num(ci(r, 'Amount')), bpOh = num(ci(r, 'ByProductOverHeads')), bpPm = num(ci(r, 'ByProductPackingMaterial'));
                    setVal('txtItemQtyBP', r0(ci(r, 'NetQty'))); setVal('txtNetWeightBP', r0(ci(r, 'NetWeight'))); setVal('txtAmountBP', r0(bp));
                    setVal('txtBPOverHead', r0(bpOh)); setVal('txtBPPMCharges', r0(bpPm)); setVal('txtBPTotalAmountWithExpenses', r0(bp + bpOh + bpPm));
                }
                if (t === 'Output Finish Goods') {
                    var fg = num(ci(r, 'Amount'));
                    avgRate = num(ci(r, 'AvgRate'));
                    setVal('txtAvgRateFG', String(Math.round(avgRate * 1000) / 1000)); setVal('txtFinishGoodRateWithOutExp', String(Math.round(num(ci(r, 'AvgRateWoExp')) * 1000) / 1000));
                    setVal('txtItemQtyFG', r0(ci(r, 'NetQty'))); setVal('txtNetWeightFG', r0(ci(r, 'NetWeight'))); setVal('txtAmountFG', r0(fg)); setVal('txtActualFinishGoodsAmount', r0(fg));
                    setVal('txtFinishAmountAgainstJobOrder', r0(fgAgainstJo)); setVal('txtDifferenceAmount', r0(fg - fgAgainstJo));
                }
            });
            show('btnApproveSettlement', num(String(val('txtAmountFG')).replace(/,/g, '')) > 0);   // :6004
        }
        /** btnApplyAvgRateFG_Click:6372 */
        function applyAvgRate() {
            var jo = intOf('CmbJobOrderSettlement');
            var netWeight = 0, itemAmount = 0;
            dtOutput.forEach(function (r) {
                if (String(r.EntryType) === 'FinishGoods') netWeight += num(r.Weight);
                if (num(String(val('txtAmountBP')).replace(/,/g, '')) === 0 && String(r.EntryType) !== 'FinishGoods') itemAmount += num(r.ItemAmount);
            });
            if (itemAmount > 0) setVal('txtAmountBP', String(itemAmount));
            C.getJson(API + '/summary-values?jobOrderId=' + jo + '&netWeight=' + netWeight).then(function (sv) {
                if (sv && sv.length) {
                    sv.forEach(function (r) { if (String(ci(r, 'TranType')) === 'Output Finish Goods') { avgRate = num(ci(r, 'AvgRate')); avgRateWoExp = num(ci(r, 'AvgRateWoExp')); setVal('txtAvgRateFG', String(Math.round(avgRate * 1000) / 1000)); } });
                    return proportion();
                }
                setVal('txtAvgRateFG', '0'); setVal('txtFinishGoodRateWithOutExp', '0');
            }).then(function () { show('btnApproveSettlement', num(String(val('txtAmountFG')).replace(/,/g, '')) > 0); }).catch(fail);
        }
        /** ProporationFinishGoodsSettlement:6440 */
        function proportion() {
            var a = val('txtAvgRateFG');
            if (a === '' || a === '0') { alert('Finish Goods AvgRate Not Found Please check!'); return Promise.resolve(); }
            var chain = Promise.resolve();
            dtOutput.forEach(function (r) {
                chain = chain.then(function () {
                    if (C.intOf(r.RateUOMId) === 0) {
                        return C.getJson(API + '/uom-by-item?itemId=' + C.intOf(r.ItemId)).then(function (uoms) {
                            if (uoms && uoms.length) {
                                var u = uoms.filter(function (x) { return C.intOf(ci(x, 'Equivalent')) === 40; })[0];
                                if (!u) { alert('40KG RateUom not define please check'); throw new Error('__stop'); }
                                r.RateUOMId = C.intOf(ci(u, 'Id')); r.RateUOM = C.intOf(ci(u, 'Equivalent'));
                            }
                        });
                    }
                }).then(function () {
                    var w = num(r.Weight), ru = num(r.RateUOM), costs = num(r.ItemPmCost) + num(r.GeneralPmCost) + num(r.ItemOhCost) + num(r.GeneralOhCost);
                    if (String(r.EntryType) === 'FinishGoods') {
                        var rate = avgRateWoExp, woExp = w * rate, withExp = woExp + costs;
                        r.RateWithoutExp = rate * ru; r.AmountWithoutExp = woExp; r.NetRate = withExp / w * ru; r.TotalAmount = withExp;
                    } else {
                        var rt = num(r.Rate), wo = w / ru * rt, bpWith = wo + costs;
                        r.NetRate = bpWith / w * ru; r.RateWithoutExp = rt; r.AmountWithoutExp = wo; r.TotalAmount = bpWith;
                    }
                    ['NetRate', 'RateWithoutExp', 'AmountWithoutExp', 'TotalAmount'].forEach(function (k) { if (!isFinite(r[k])) r[k] = 0; });
                });
            });
            return chain.then(renderOut).catch(function (e) { if (e && e.message !== '__stop') fail(e); renderOut(); });
        }
        /** ResetSettlement:6527 */
        function reset() {
            show('btnApproveSettlement', false);
            ['txtItemQtyRM', 'txtItemQtyBP', 'txtItemQtyFG', 'txtNetWeightRM', 'txtNetWeightBP', 'txtNetWeightFG', 'txtAmountRM', 'txtAmountBP', 'txtAmountFG', 'txtActualFinishGoodsAmount', 'txtFinishAmountAgainstJobOrder', 'txtDifferenceAmount',
                'txtBPOverHead', 'txtBPPMCharges', 'txtBPTotalAmountWithExpenses', 'txtOverHeadsSettlement', 'txtPackingMaterialSettlement', 'txtAvgRateFG', 'txtFinishGoodRateWithOutExp'].forEach(function (id) { setVal(id, ''); });
            setVal('CmbDifferenceAccountTitle', ''); setVal('CmbJobOrderSettlement', '');
            dtInput = []; dtOutput = [];
            ['grdInputsettlement', 'grdOutPutSettlement', 'grdOverHeadSettlement', 'grdPackingMaterialSettlement'].forEach(clearGrid);
        }
        /** btnResetSettlement_Click:6564 */
        function resetClick() { reset(); refreshJobOrdersAll(); }
        /** btnUpdateSettlement_Click:6682 -> SettlementFinancials:6577 */
        function updateSettlement() {
            if (!confirm('Are you sure to Update Settlement?')) return;
            if (!intOf('CmbJobOrderSettlement')) { alert('JobOrder Settlement Field is Required'); $id('CmbJobOrderSettlement').focus(); return; }
            if (!intOf('CmbDifferenceAccountTitle')) { alert('Difference Account Field is Required'); $id('CmbDifferenceAccountTitle').focus(); return; }
            C.postJson(API + '/settlement/financials', { jobOrderId: intOf('CmbJobOrderSettlement'), differenceAccountId: intOf('CmbDifferenceAccountTitle'),
                actualFinishGoodsText: val('txtActualFinishGoodsAmount'), finishAmountAgainstJobOrderText: val('txtFinishAmountAgainstJobOrder'), differenceAmountText: val('txtDifferenceAmount') })
                .then(function (res) { alert(res.message); }).catch(fail);
        }
        /** btnApproveSettlement_Click:6796 */
        function approve() {
            if (!confirm('Are you sure to Approve Settlement?')) return;
            C.postJson(API + '/settlement/approve', { jobOrderId: intOf('CmbJobOrderSettlement') }).then(function (res) { alert(res.message); reset(); }).catch(fail);
        }
        /** btnSettlementVoucher_Click:7095 - VoucherHeadIdGet(JobOrderId, 142) -> AcRptPurchaseSalesVoucherSlip_103 */
        function voucher() {
            var jo = intOf('CmbJobOrderSettlement');
            voucherHeadId(jo, DOC.SETTLEMENT).then(function (v) { if (v > 0) window.CrystalPrint.open('acc-103', { id: v, documentTypeId: DOC.SETTLEMENT }, 'btnSettlementVoucher'); });
        }
        /** btn613SummaryReport_Click:6697 / btn615Report_Click:6730 / btnRecoveryReport_Click:6763 */
        function print(rpt) {
            var jo = intOf('CmbJobOrderSettlement'), wages = $id('chkIncludeWages').checked;
            var tpl = { '613': '613-InvRptProductionSummeryWithExpValues.rpt', '615': '615-InvRptProductionSummery.rpt', '602A': '602A-InvRptProductionSummeryWithValues.rpt' }[rpt];
            printRows(API + '/print/summary?rpt=' + rpt + '&jobOrderId=' + jo + '&includeWages=' + wages, tpl);
        }
        return { generate: generate, applyAvgRate: applyAvgRate, resetClick: resetClick, updateSettlement: updateSettlement, approve: approve, voucher: voucher, print: print };
    })();

    // =========================================================================================== dialogs
    /** Load{Input|OutPut|PackingMaterial|OverHeads}DataByJobOrder - one modal, four sources; checked rows are handed back. */
    var loader = (function () {
        var kind = '', jobOrderId = 0, rows = [], done = null;
        function open(k, jo, caption, cb) {
            kind = k; jobOrderId = jo; done = cb; rows = [];
            $id('dlgLoaderTitle').textContent = 'EccountBookERP';
            $id('dlgLoaderCaption').textContent = caption;
            $id('ldAll').checked = false;
            C.openModal('dlgLoader');
            refresh();
        }
        function refresh() {
            C.getJson(API + '/load/' + kind + '?jobOrderId=' + jobOrderId).then(function (rs) {
                rows = (rs || []).map(function (r) { r.__chk = false; return r; });
                render();
            }).catch(fail);
        }
        function render() {
            var t = $id('grdLoader');
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = '<tr><td class="pn-note">No pending rows for this job order.</td></tr>'; t.tFoot.innerHTML = ''; return; }
            var keys = Object.keys(rows[0]).filter(function (k) { return k !== '__chk' && k !== '__sel'; });
            t.tHead.innerHTML = '<tr><th>Select</th>' + keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r, i) {
                return '<tr><td class="ctr"><input type="checkbox" data-i="' + i + '"' + (r.__chk ? ' checked' : '') + '></td>' + keys.map(function (k) {
                    var v = r[k]; var txt = /date/i.test(k) ? dMMMyyyy(v) : (typeof v === 'number' ? f2(v) : fmtPlain(v));
                    return '<td' + (typeof v === 'number' ? ' class="num"' : '') + '>' + esc(txt) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '';
            t.tBodies[0].onchange = function (e) { if (e.target.type === 'checkbox') rows[+e.target.getAttribute('data-i')].__chk = e.target.checked; };
        }
        function checkAll(on) { rows.forEach(function (r) { r.__chk = on; }); render(); }
        function load() {
            var picked = rows.filter(function (r) { return r.__chk; });
            if (!picked.length) { alert('Select at least one row to load'); return; }
            close();
            if (done) done(picked);
        }
        function close() { C.closeModal('dlgLoader'); }
        return { open: open, refresh: refresh, load: load, close: close, checkAll: checkAll };
    })();

    /** LoadOutPutPendingforRates - job orders (DocumentTypeId 403) + USP_GetProductionOutPutDataForRatesInput; a row click returns its Id. */
    var pend = (function () {
        var done = null, rows = [];
        function open(cb) {
            done = cb;
            C.getJson(API + '/pending-rates/job-orders').then(function (j) { sel('cmbPendJobOrder', j || [], 'Id', 'PlanCode'); }).catch(fail);
            if (!val('pendFrom')) setVal('pendFrom', today());
            if (!val('pendTo')) setVal('pendTo', today());
            clearGrid('grdPending');
            C.openModal('dlgPending');
        }
        function search() {
            C.getJson(API + '/pending-rates?from=' + val('pendFrom') + '&to=' + val('pendTo') + '&jobOrderId=' + intOf('cmbPendJobOrder')).then(function (rs) {
                rows = rs || [];
                if (!rows.length) { clearGrid('grdPending'); alert('Record Not Found'); return; }
                renderGrid_('grdPending', allCols(rows, []), rows, { onSelect: function (r) {
                    var id = C.intOf(ci(r, 'Id')) || C.intOf(ci(r, 'InvFoodProductionId')) || C.intOf(ci(r, 'OutPutDocId'));
                    close(); if (done) done(id);
                } });
            }).catch(fail);
        }
        function close() { C.closeModal('dlgPending'); }
        return { open: open, search: search, close: close };
    })();

    // =========================================================================================== keyboard (frmFoodProduction_KeyDown:1381)
    document.addEventListener('keydown', function (e) {
        var tag = (e.target.tagName || '').toUpperCase();
        if (e.key === 'Enter' && !e.ctrlKey && tag !== 'TEXTAREA' && tag !== 'BUTTON' && !e.target.closest('table')) {
            /* SendKeys.Send("{TAB}") */
            var f = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button'), function (x) { return !x.disabled && x.tabIndex >= 0 && x.offsetParent !== null; });
            var i = f.indexOf(e.target); if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return;
        }
        if (!e.ctrlKey) return;
        var k = e.key, code = e.code || '';
        var pads = { Numpad1: 'InPut', Numpad2: 'OutPut', Numpad3: 'PackingMaterial', Numpad4: 'OverHead', Numpad5: 'TransctionHistory', Numpad6: 'Summary' };
        if (pads[code] && $id(pads[code])) { e.preventDefault(); tab(pads[code]); return; }
        var K = k.toUpperCase();
        if (tabName === 'InPut') {
            if (K === 'S' && visible('btnsave')) { e.preventDefault(); inp.save(); }
            else if (K === 'U' && visible('btnUpdate')) { e.preventDefault(); inp.update(); }
            else if (K === 'L') { e.preventDefault(); inp.loadInput(); }
            else if (K === 'N') { e.preventDefault(); inp.reset(); }
            else if (K === 'R') { e.preventDefault(); refreshJobOrders(); }
            else if (k === 'F5') { e.preventDefault(); $id('txtDocdate').focus(); }
            else if (k === 'ArrowDown') { e.preventDefault(); var g = $id('grdStockConversionProductionDetail').querySelector('input,select'); if (g) g.focus(); }
        } else if (tabName === 'OutPut') {
            if (K === 'S' && visible('btnSaveOutPut')) { e.preventDefault(); out.save(); }
            else if (K === 'U' && visible('btnUpdateOutPut')) { e.preventDefault(); out.update(); }
            else if (K === 'N') { e.preventDefault(); out.reset(); }
            else if (K === 'R') { e.preventDefault(); out.refresh(); }
            else if (k === 'F5') { e.preventDefault(); $id('DocDateOutPut').focus(); }
            else if (k === 'ArrowDown') { e.preventDefault(); var g2 = $id('grdByProduct').querySelector('input,select'); if (g2) g2.focus(); }
        } else if (tabName === 'PackingMaterial') {
            if (K === 'S' && visible('btnsavepackingmaterial')) { e.preventDefault(); pm.save(); }
            else if (K === 'U' && visible('btnUpdatepackingmaterial')) { e.preventDefault(); pm.update(); }
            else if (K === 'N') { e.preventDefault(); pm.reset(); }
            else if (K === 'R') { e.preventDefault(); refreshJobOrders(); }
        } else if (tabName === 'OverHead') {
            if (K === 'S' && visible('btnSaveOverHead')) { e.preventDefault(); oh.save(); }
            else if (K === 'U' && visible('btnUpdateOverHead')) { e.preventDefault(); oh.update(); }
            else if (K === 'N') { e.preventDefault(); oh.reset(); }
            else if (K === 'R') { e.preventDefault(); refreshJobOrders(); }
        } else if (tabName === 'TransctionHistory') {
            if (k === 'Enter') { e.preventDefault(); var cur = th.current(); if (cur) th.edit(cur); }
            else if (K === 'D') { e.preventDefault(); var id = th.currentId(); if (id) th.detail(id); }
        } else if (tabName === 'Summary' && k === 'F5') { e.preventDefault(); $id('cmbsummeryJobOrderNo').focus(); }
    });

    window.PN = { tab: tab, subTab: subTab, inp: inp, out: out, pm: pm, oh: oh, th: th, sum: sum, st: stl, loader: loader, pend: pend,
        refreshJobOrders: refreshJobOrders, refreshJobOrdersAll: refreshJobOrdersAll, voucher118: voucher118, defineJobOrder: defineJobOrder, wagesClose: wagesClose };
    document.addEventListener('DOMContentLoaded', load);
})();
