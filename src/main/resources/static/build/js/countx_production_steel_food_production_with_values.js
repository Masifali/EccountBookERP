/* ============================================================================================
 * Screen 601 "Food Production With Values New" (Production Steel) -
 * Architecture.WinApp.Steel.Production.FoodProductionWithValuesNew_St. Line numbers are that file's.
 *
 * Input (1512) / OutPut (1513) / Over Head (1514) each have Form + History; Transaction History,
 * Summery and Settlement read the job order. The grids are the desktop DataTables (tableInput :772,
 * tableByProduct :796, tableOverHeads :818) with the same column names; removed saved rows travel
 * with ActionTypeId 3 (RemovedInputList / RemovedOutputList / RemovedOverheadList).
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/production/steel/food-production-with-values/api';
    var DOC_INPUT = 1512, DOC_OUTPUT = 1513, DOC_OVERHEAD = 1514, DOC_SETTLEMENT_VOUCHER = 178;

    var rights = { save: false, update: false, print: false, rate: false, settlement: false };
    var rateDecimals = 2, amountDecimals = 0;
    var lists = {};
    var recId = 0, recIdOutput = 0, recOverHeadId = 0, voucherHeadId = { input: 0, output: 0, overhead: 0 };
    var vis = { save: true, update: false, saveOut: true, updateOut: false, saveOh: true, updateOh: false };
    var inputRows = [], outputRows = [], ohRows = [];
    var removed = { input: [], output: [], oh: [] };
    var current = { input: -1, output: -1, oh: -1 };
    var hist = { input: [], output: [], oh: [], th: [] }, histCur = { input: -1, output: -1, oh: -1, th: -1 };
    var settle = { rows: [], overheads: [], values: [], voucherHeadId: 0, avgRate: 0, avgRateWo: 0 };
    var loader = { kind: '', rows: [] };

    /* ------------------------------------------------------------------ helpers */
    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focusEl(id) { var e = $id(id); if (e) { try { e.focus(); } catch (x) { /* ignore */ } } }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;'); }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function pick(row, names) { for (var i = 0; i < names.length; i++) { var v = ci(row, names[i]); if (v !== '' && v !== null && v !== undefined) return v; } return ''; }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { return Math.trunc(num(v)); }
    function selText(id) { var el = $id(id); if (!el || el.selectedIndex < 0) return ''; var o = el.options[el.selectedIndex]; return (o && o.value !== '') ? o.text : ''; }
    function hasRow(id) { return val(id) !== ''; }
    var MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function parts(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? { y: m[1], M: m[2], d: m[3] } : null; }
    function fmtDMY(v) { var p = parts(v); return p ? p.d + '-' + MONTHS[+p.M - 1] + '-' + p.y : String(v || ''); }
    function fmtShort(v) { var p = parts(v); return p ? p.d + '/' + p.M + '/' + p.y : String(v || ''); }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function fmtInt(v) { var n = Math.round(num(v)); return n === 0 ? '' : group(String(n)); }                      // "#,#"
    function fmt00(v) { return group(String(Math.round(num(v)))); }                                                   // "0,0"
    function fmtDec(v, n) { var s = num(v).toFixed(n).replace(/0+$/, '').replace(/\.$/, ''); var p = s.split('.'); p[0] = group(p[0]); return p.join('.'); }
    function fmtRate(v) { var p = num(v).toFixed(rateDecimals).split('.'); p[0] = group(p[0]); return p.join('.'); }
    function fmtAmt(v) { var p = num(v).toFixed(amountDecimals).split('.'); p[0] = group(p[0]); return p.join('.'); }
    function round(v, n) { var f = Math.pow(10, n); return Math.sign(v) * Math.round(Math.abs(v) * f) / f; }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function isoOf(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[0] : ''; }
    function busy(btnId, fn) {
        var b = $id(btnId);
        if (b) { if (b.classList.contains('is-busy')) return; b.dataset.wasDisabled = b.disabled ? '1' : ''; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (!b) return; b.classList.remove('is-busy'); b.disabled = b.dataset.wasDisabled === '1'; applyButtons(); };
        var p; try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function csrf(opt) { var t = document.querySelector('meta[name="_csrf"]'), h = document.querySelector('meta[name="_csrf_header"]'); if (t && h && opt.method && opt.method !== 'GET') opt.headers[h.getAttribute('content')] = t.getAttribute('content'); }
    function request(url, options) {
        var opt = options || {}; opt.credentials = 'same-origin'; opt.headers = opt.headers || {}; opt.headers.Accept = 'application/json'; csrf(opt);
        return fetch(url, opt).then(function (r) { return r.text().then(function (t) {
            var body = null; try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((body && body.message) || t || ('Request failed (' + r.status + ')'));
            return body;
        }); });
    }
    function getJson(url) { return request(url, {}); }
    function postJson(url, body) { return request(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }); }
    function fill(id, rows, valueKey, textKey) {
        var el = $id(id); if (!el) return;
        var keep = el.value, html = '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(ci(r, valueKey)) + '">' + esc(ci(r, textKey)) + '</option>'; });
        el.innerHTML = html; if (keep !== '') el.value = keep; if (el.value !== keep) el.value = '';
    }
    function bindIf(id, rows, valueKey, textKey) { if (rows && rows.length) fill(id, rows, valueKey, textKey); }
    function options(rows, valueKey, textKey, selected) {
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) { var v = ci(r, valueKey); html += '<option value="' + esc(v) + '"' + (String(v) === String(selected) ? ' selected' : '') + '>' + esc(ci(r, textKey)) + '</option>'; });
        return html;
    }
    function copy(o) { return JSON.parse(JSON.stringify(o)); }

    /* ------------------------------------------------------------------ rights / load */
    function applyButtons() {
        show('btnsave', vis.save); show('btnUpdate', vis.update); show('btnSaveOutPut', vis.saveOut); show('btnUpdateOutPut', vis.updateOut);
        show('btnSaveOverHead', vis.saveOh); show('btnUpdateOverHead', vis.updateOh);
        [['btnsave', rights.save], ['btnSaveOutPut', rights.save], ['btnSaveOverHead', rights.save], ['btnUpdate', rights.update], ['btnUpdateOutPut', rights.update], ['btnUpdateOverHead', rights.update],
         ['Print', rights.print], ['btnVoucherReport', rights.print], ['btnVoucherOutPut', rights.print], ['btnPrintOutPut', rights.print]].forEach(function (p) {
            var b = $id(p[0]); if (b && !b.classList.contains('is-busy')) b.disabled = !p[1];
        });
        var missing = []; if (!rights.save) missing.push('Save'); if (!rights.update) missing.push('Update'); if (!rights.print) missing.push('Print');
        var note = $id('rightsNote');
        note.textContent = missing.length ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.' : '';
        note.classList.toggle('is-hidden', !missing.length);
    }
    function saveModeInput() { return vis.save && rights.save; }
    function saveModeOutput() { return vis.saveOut && rights.save; }
    function saveModeOh() { return vis.saveOh && rights.save; }

    /** frmFoodProduction_Load:726 */
    function loadAll() {
        return getJson(api + '/lookups').then(function (d) {
            d = d || {}; lists = d;
            rights.save = !!d.canSave; rights.update = !!d.canUpdate; rights.print = !!d.canPrint; rights.rate = !!d.canRateAndAmount; rights.settlement = !!d.canSettlementTab;
            rateDecimals = intOf(d.rateDecimals); amountDecimals = intOf(d.amountDecimals);
            if (!rights.settlement) {                                     // :759 - tabControl1.TabPages.Remove(Settlement); Remove(OverHead)
                document.querySelector('#tabs1 [data-tab="Settlement"]').classList.add('is-hidden');
                document.querySelector('#tabs1 [data-tab="OverHead"]').classList.add('is-hidden');
            }
            if (!rights.rate) { show('btnVoucherReport', false); show('btnVoucherOutPut', false); show('btnVoucherOH', false); }   // :846
            bindIf('CmbDifferenceAccountTitle', d.differenceAccounts, 'Id', 'AccountTitle');       // stockAccoutnFill:872
            bindJobOrders(d);
            setVal('txtdocnumber', intOf(d.docNoInput) > 0 ? d.docNoInput : '');                 // GenerateDocNumberInput:1318
            setVal('txtDocNoOutPut', intOf(d.docNoOutput) > 0 ? d.docNoOutput : '');
            setVal('txtDocNoOverHead', intOf(d.docNoOverhead) > 0 ? d.docNoOverhead : '');
            applyButtons(); renderInput(); renderOutput(); renderOh();
            focusEl('txtDocdate');
        });
    }
    function bindJobOrders(d) {
        bindIf('CmbJobOrderNoInput', d.jobOrders, 'Id', 'Name'); bindIf('CmbJobOrderNoOutput', d.jobOrders, 'Id', 'Name'); bindIf('cmbJobOrderOverHead', d.jobOrders, 'Id', 'Name');   // JobOrderNoFill:926
        if (d.summaryJobOrders && d.summaryJobOrders.length) { fill('cmbsummeryJobOrderNo', d.summaryJobOrders, 'Id', 'Name'); fill('CmbJobOrderSettlement', d.summaryJobOrders, 'Id', 'Name'); }
        else { fill('cmbsummeryJobOrderNo', [], 'Id', 'Name'); fill('CmbJobOrderSettlement', [], 'Id', 'Name'); }                                                                            // :996
    }
    /** btnRefresh_Click:1667 / btnRefreshOverHead_Click:3524 / btnRefreshSummary_Click:4456 - JobOrderNoFill (+ summery list). */
    function refreshJobOrders() {
        return getJson(api + '/next-codes').then(function (d) { bindJobOrders(d || {}); }).catch(function (e) { box(e.message); });
    }
    /** btnRefreshOutPut_Click:2224 - the stock account, UOM, job lot, warehouse, packing type and job order lists. */
    function refreshOutput() {
        return busy('btnRefreshOutPut', function () {
            return getJson(api + '/lookups').then(function (d) { d = d || {}; lists = d; bindIf('CmbDifferenceAccountTitle', d.differenceAccounts, 'Id', 'AccountTitle'); bindJobOrders(d); renderOutput(); }).catch(function (e) { box(e.message); });
        });
    }

    /* ------------------------------------------------------------------ tabs */
    var TABS = ['InPut', 'OutPut', 'OverHead', 'TransctionHistory', 'Summary', 'Settlement'];
    function tab(id) {
        TABS.forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs1').querySelectorAll('.fp-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        /* tabControl1_SelectedIndexChanged:1278 */
        if (id === 'OutPut' && saveModeOutput()) { nextOutputCode(); focusEl('DocDateOutPut'); }
        if (id === 'OverHead') { nextOverheadCode(); focusEl('txtDocDateOverHead'); }
        if (id === 'Settlement') focusEl('CmbJobOrderSettlement');
        if (id === 'TransctionHistory') transactionHistory(50);          // GridHistory(50) runs on every tab change on the desktop
    }
    function currentTab() { for (var i = 0; i < TABS.length; i++) if ($id(TABS[i]).classList.contains('is-active')) return i; return 0; }
    function sub(stripId, pageId) {
        var strip = $id(stripId);
        strip.querySelectorAll('.fp-tab').forEach(function (b) {
            var on = b.getAttribute('data-tab') === pageId;
            b.classList.toggle('is-active', on);
            $id(b.getAttribute('data-tab')).classList.toggle('is-active', on);
        });
        if (pageId === 'tabPage4') inputHistory(0);           // tabControl2_SelectedIndexChanged:1960
        if (pageId === 'tabPage6') outputHistory(50);         // tabControl3_SelectedIndexChanged:2901
        if (pageId === 'tabPage3') overheadHistory(50);       // tabControl4_SelectedIndexChanged:3896
    }
    function full(hostId) {
        var h = $id(hostId); if (!h) return;
        var on = !h.classList.contains('fp-full');
        document.querySelectorAll('.fp-full').forEach(function (x) { x.classList.remove('fp-full'); });
        if (on) h.classList.add('fp-full');
    }

    /* ------------------------------------------------------------------ job order -> WIP / plants / totals */
    /** CmbJobOrderNo_Leave:1348 / CmbJobOrderNoOutput_Leave:2306 / cmbJobOrderOverHead_Leave:3267 */
    function jobOrderChanged(kind) {
        var combo = kind === 'input' ? 'CmbJobOrderNoInput' : kind === 'output' ? 'CmbJobOrderNoOutput' : 'cmbJobOrderOverHead';
        var jobOrderId = intOf(val(combo));
        return getJson(api + '/job-order-info?jobOrderId=' + jobOrderId).then(function (d) {
            d = d || {};
            if (kind !== 'overhead') {
                if (d.wipAccounts && d.wipAccounts.length) {
                    fill('CmbWorkInProcess', d.wipAccounts, 'Id', 'Name'); fill('CmbWIPItem', d.wipItems, 'Id', 'Name');
                    setVal('CmbWorkInProcess', ci(d.wipAccounts[0], 'Id')); setVal('CmbWIPItem', ci(d.wipItems[0], 'Id'));   // Rows[0].Activate()
                } else { fill('CmbWorkInProcess', [], 'Id', 'Name'); fill('CmbWIPItem', [], 'Id', 'Name'); }
            }
            var plantCombo = kind === 'input' ? 'CmbPlantFeederInput' : kind === 'output' ? 'CmbPlantFeederOutput' : 'CmbPlantFeederOverHead';
            if (d.plants && d.plants.length && jobOrderId) fill(plantCombo, d.plants, 'Id', 'Name'); else fill(plantCombo, [], 'Id', 'Name');   // ReadPlantDetailByJobOrderId:1082
            var totals = d.totals || [];
            if (kind === 'output') {
                var inQ = 0, inW = 0, outQ = 0, outW = 0;
                totals.forEach(function (t) { if (intOf(t.EntryTypeId) === 3) { inQ += num(t.ItemQty); inW += num(t.StockWeight); } else { outQ += num(t.ItemQty); outW += num(t.StockWeight); } });
                var has = totals.length > 0;
                setVal('txtInPutTotalQty', has ? fmtInt(inQ) : '0'); setVal('txtInPutTotalWeight', has ? fmtInt(inW) : '0');
                setVal('txtOutQtyTotal', has ? fmtInt(outQ) : '0'); setVal('txtOutWeightTotal', has ? fmtInt(outW) : '0');
                setVal('txtBalQtyTotal', has ? fmtInt(inQ - outQ) : '0'); setVal('txtBalWeightTotal', has ? fmtInt(inW - outW) : '0');
                if (d.wipAccounts && d.wipAccounts.length) {
                    if (saveModeOutput() || num(val('txtAvgFinishGoodRate')) === 0) setVal('txtAvgFinishGoodRate', d.finishGoodRate);
                } else setVal('txtAvgFinishGoodRate', '0');
            }
            if (kind === 'overhead') {                                       // GetTotlInPutOut:3280
                var iq = 0, iw = 0, oq = 0, ow = 0, fq = 0, fw = 0;
                totals.forEach(function (t) {
                    var et = intOf(t.EntryTypeId);
                    if (et === 2) { fq += num(t.ItemQty); fw += num(t.StockWeight); }
                    if (et === 3) { iq += num(t.ItemQty); iw += num(t.StockWeight); } else { oq += num(t.ItemQty); ow += num(t.StockWeight); }
                });
                var h2 = totals.length > 0;
                setVal('txtInputQtyTotalOH', h2 ? fmtInt(iq) : '0'); setVal('txtInputWeightTotalOH', h2 ? fmtInt(iw) : '0');
                setVal('txtOutputQtyTotalOH', h2 ? fmtInt(oq) : '0'); setVal('txtOutputWeightTotalOH', h2 ? fmtInt(ow) : '0');
                setVal('txtfinishQtyOh', h2 ? fmtInt(fq) : '0'); setVal('txtfinishWeightOh', h2 ? fmtInt(fw) : '0');
            }
        }).catch(function (e) { box(e.message); });
    }

    /* ================================================================== INPUT (1512) */
    var IN_COLS = [['WareHouse', null], ['JobLot', null], ['Item', null], ['PackingType', null], ['PackUom', null], ['ItemQty', 'edit'], ['NetWeight', 'edit'],
        ['ItemRate', 'rate'], ['RateUom', 'rateuom'], ['ItemAmount', 'amount'], ['Remarks', 'text']];
    function renderInput() {
        var cols = IN_COLS.filter(function (c) { return rights.rate || (c[1] !== 'rate' && c[1] !== 'rateuom' && c[1] !== 'amount'); });   // gridsettings:1775
        $id('inHead').innerHTML = '<th class="col-x">X</th>' + cols.map(function (c) { return '<th' + (c[1] && c[1] !== 'text' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id('inBody').innerHTML = inputRows.length ? inputRows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="input" data-i="' + i + '" class="' + (i === current.input ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="fp-x" tabindex="-1" data-act="del">X</button></td>'
                + cols.map(function (c) {
                    var v = r[c[0]];
                    if (c[1] === 'edit') return '<td class="num"><input type="text" class="cellin" data-col="' + c[0] + '" data-i="' + i + '" value="' + esc(fmtDec(v, 2)) + '"></td>';
                    if (c[1] === 'text') return '<td><input type="text" class="cellin txt" data-col="Remarks" data-i="' + i + '" value="' + esc(v) + '"></td>';
                    if (c[1] === 'rate') return '<td class="num">' + esc(fmtRate(v)) + '</td>';
                    if (c[1] === 'amount') return '<td class="num">' + esc(fmtAmt(v)) + '</td>';
                    if (c[1] === 'rateuom') return '<td class="num">' + esc(v) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '';
        var q = 0, w = 0, a = 0; inputRows.forEach(function (r) { q += num(r.ItemQty); w += num(r.NetWeight); a += num(r.ItemAmount); });
        $id('inFoot').innerHTML = inputRows.length ? '<tr><td></td>' + cols.map(function (c) {
            if (c[0] === 'ItemQty') return '<td class="num">' + esc(fmtDec(q, 2)) + '</td>';
            if (c[0] === 'NetWeight') return '<td class="num">' + esc(fmtDec(w, 2)) + '</td>';
            if (c[0] === 'ItemAmount') return '<td class="num">' + esc(fmtAmt(a)) + '</td>';
            return '<td></td>';
        }).join('') + '</tr>' : '';
        $id('lblInputCount').textContent = inputRows.length;
        $id('CmbJobOrderNoInput').disabled = inputRows.length > 0 && !saveModeInput() ? true : $id('CmbJobOrderNoInput').disabled;
    }
    /** grdStockConversionProductionDetail_CellUpdated:1672 */
    function inputCell(i, col, value) {
        var r = inputRows[i]; if (!r) return;
        if (col === 'ItemQty') {
            var qty = num(value), uom = num(r.PackEquivalent); r.ItemQty = qty; r.NetWeight = qty * uom;
            var amt = (qty * uom) / num(r.RateEquivalent) * num(r.ItemRate); r.ItemAmount = isFinite(amt) ? amt : 0;
        } else if (col === 'NetWeight') {
            var w = num(value); r.NetWeight = w;
            var amt2 = w / num(r.RateEquivalent) * num(r.ItemRate); r.ItemAmount = isFinite(amt2) ? amt2 : 0;
        } else if (col === 'Remarks') { r.Remarks = value; return; }
        renderInput();
    }
    /** grdStockConversionProductionDetail_ColumnButtonClick:1704 */
    function deleteInput(i) {
        var r = inputRows[i]; if (!r) return;
        if (!ask('Are you sure to Delete?')) return;
        if (intOf(r.Id) > 0) { var c = copy(r); c.ActionTypeId = 3; removed.input.push(c); }
        inputRows.splice(i, 1); current.input = -1;
        if (!inputRows.length) $id('CmbJobOrderNoInput').disabled = false;
        renderInput();
    }
    /** FormValidation:1379 */
    function inputOk() {
        if (val('txtdocnumber').trim() === '' || val('txtdocnumber') === '0') { box('document Number Field Required'); focusEl('txtdocnumber'); return false; }
        if (!hasRow('CmbJobOrderNoInput')) { box('Job Order Number Field Required'); focusEl('CmbJobOrderNoInput'); return false; }
        if (!hasRow('CmbPlantFeederInput')) { box('Plant Name Field Required'); focusEl('CmbPlantFeederInput'); return false; }
        if (!hasRow('CmbWorkInProcess')) { box('WorkInProcess Field Required'); focusEl('CmbWorkInProcess'); return false; }
        if (!hasRow('CmbWIPItem')) { box('WIP Item Field Required'); focusEl('CmbWIPItem'); return false; }
        return true;
    }
    /** InsertInput:1414 */
    function insertInput(btnId) {
        return busy(btnId, function () {
            if (!inputOk()) return;
            var wasUpdate = recId > 0;
            if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            if (!inputRows.length) { box('Enter Detail First ...'); return; }
            var body = {
                Id: recId, DocumentTypeId: DOC_INPUT, DocCode: val('txtdocnumber'), DocDate: val('txtDocdate'), MainRemarks: val('txtRemarksInput'),
                WIPAccountId: intOf(val('CmbWorkInProcess')), WIPItemId: intOf(val('CmbWIPItem')), ProductionJobOrderId: intOf(val('CmbJobOrderNoInput')),
                ProductionJobOrderNo: selText('CmbJobOrderNoInput').trim(), PlantId: intOf(val('CmbPlantFeederInput')),
                details: removed.input.concat(inputRows.map(function (r) {
                    return { Id: r.Id, JobOrderInPutId: r.JobOrderInPutId, RefDocIdNo: r.RefDocIdNo, RefDocumentTypeId: r.RefDocumentTypeId, RefDocSubIdNo: r.RefDocSubIdNo,
                        WareHouseId: r.WareHouseId, ItemId: r.ItemId, JobLotId: r.JobLotId, PackingTypeId: r.PackingTypeId, PackUom: r.PackUom, PackUomId: r.PackUomId,
                        ItemQty: r.ItemQty, NetWeight: r.NetWeight, ItemRate: r.ItemRate, ItemAmount: r.ItemAmount, RateUomId: r.RateUomId, Remarks: r.Remarks };
                }))
            };
            return postJson(api + '/save', body).then(function (d) {
                box((d && d.message) || (wasUpdate ? 'Record Update Successfully' : 'Record Save Successfully'));
                resetInput();
            }).catch(function (e) { box(e.message); });
        });
    }
    function saveInput() { recId = 0; return insertInput('btnsave'); }           // btnsave_Click:1549
    function updateInput() { return insertInput('btnUpdate'); }                 // btnUpdate_Click:1562
    /** reset:1641 */
    function resetInput() {
        removed.input = []; recId = 0; voucherHeadId.input = 0; setVal('txtRemarksInput', '');
        vis.save = true; vis.update = false; $id('CmbJobOrderNoInput').disabled = false;
        inputRows = []; current.input = -1; $id('lblRecIdInput').textContent = '';
        setVal('CmbJobOrderNoInput', ''); setVal('CmbPlantFeederInput', ''); fill('CmbWorkInProcess', [], 'Id', 'Name'); fill('CmbWIPItem', [], 'Id', 'Name');
        applyButtons(); renderInput();
        return getJson(api + '/next-codes').then(function (d) { d = d || {}; if (intOf(d.docNoInput) > 0) setVal('txtdocnumber', d.docNoInput); bindJobOrders(d); }).catch(function (e) { box(e.message); });
    }
    /** ReadByIdInput:1574 */
    function readInput(id) {
        return getJson(api + '/' + id).then(function (d) {
            var h = d.header || {};
            recId = id; vis.save = false; vis.update = true;
            setVal('txtdocnumber', h.DocCode); setVal('txtDocdate', isoOf(h.DocDate)); setVal('txtRemarksInput', h.MainRemarks);
            fill('CmbJobOrderNoInput', [{ Id: h.ProductionJobOrderId, Name: h.ProductionJobOrderNo }], 'Id', 'Name'); setVal('CmbJobOrderNoInput', h.ProductionJobOrderId);
            $id('CmbJobOrderNoInput').disabled = true;
            return jobOrderChanged('input').then(function () {
                setVal('CmbPlantFeederInput', h.PlantId);
                inputRows = (d.details || []).map(function (x) {
                    return { Id: x.Id, JobOrderInPutId: x.JobOrderInPutId, RefDocumentTypeId: x.RefDocumentTypeId, RefDocIdNo: x.RefDocNoId, RefDocSubIdNo: x.RefDocSubIdNo,
                        WareHouseId: x.WarehouseId, WareHouse: x.WareHouseName, JobLotId: x.JobLotId, JobLot: x.JobLotDescription, ItemId: x.ItemId, Item: x.ItemName,
                        PackingTypeId: x.PackingtypeId, PackingType: x.PackingType, PackUomId: x.PackUomId, PackUom: x.PackUom, PackEquivalent: x.PackUomEquivalent,
                        ItemQty: x.Qty, NetWeight: x.Weight, ItemRate: round(num(x.NetRate), 2), RateUomId: x.RateUOMId, RateUom: x.RateUOM, RateEquivalent: x.RateUomEquivalent,
                        ItemAmount: x.Amount, Remarks: x.Remarks };
                });
                voucherHeadId.input = intOf(d.voucherHeadId);
                tab('InPut'); sub('tabs2', 'tabPage1');
                $id('lblRecIdInput').textContent = 'Record #' + id;
                applyButtons(); renderInput(); focusEl('txtDocdate');
            });
        }).catch(function (e) { box(e.message); });
    }
    /** btnLoadInvoices_Click:1845 -> LoadInputDataByJobOrder */
    function loadInput() {
        if (!hasRow('CmbJobOrderNoInput')) { focusEl('CmbJobOrderNoInput'); box('Select Job Order No First...'); return; }
        openLoader('input', 'Input Data Against Job Order', api + '/load-input?jobOrderId=' + intOf(val('CmbJobOrderNoInput')));
    }
    /** LoadDataDetailfromPurchaseInvoivce:1867 */
    function applyLoadInput(rows) {
        rows.forEach(function (r) {
            var id = intOf(ci(r, 'Id')), exists = false;
            inputRows.forEach(function (x) { if (intOf(x.JobOrderInPutId) === id) exists = true; });
            if (exists) return;
            inputRows.push({
                Id: 0, JobOrderInPutId: id, RefDocumentTypeId: intOf(ci(r, 'RefDocumentTypeId')), RefDocIdNo: intOf(ci(r, 'RefDocIdNo')), RefDocSubIdNo: intOf(ci(r, 'RefDocSubIdNo')),
                WareHouseId: intOf(pick(r, ['WareHouseId', 'WarehouseId'])), WareHouse: ci(r, 'WareHouseName'), JobLotId: intOf(ci(r, 'JobLotId')), JobLot: pick(r, ['JobLot', 'JobLotDescription']),
                ItemId: intOf(ci(r, 'ItemId')), Item: ci(r, 'ItemName'), PackingTypeId: intOf(ci(r, 'PackingTypeId')), PackingType: pick(r, ['PackingType', 'PackTypeDesc']),
                PackUomId: intOf(pick(r, ['UomSchIdQty', 'PackUomId'])), PackUom: pick(r, ['PackUom', 'PackUomCode']), PackEquivalent: num(pick(r, ['PackEquivalent', 'PackUomEquivalent'])),
                ItemQty: num(pick(r, ['BalQty', 'balQty'])), NetWeight: num(pick(r, ['BalWeight', 'balWeight'])), ItemRate: num(ci(r, 'ItemRate')),
                RateUomId: intOf(ci(r, 'RateUomId')), RateUom: pick(r, ['RateUom', 'RateUomCode']), RateEquivalent: num(pick(r, ['RateEquivalent', 'RateUomEquivalent'])),
                ItemAmount: num(pick(r, ['BalAmount', 'balAmount'])), Remarks: pick(r, ['Remarks', 'RemarksDetail'])
            });
        });
        $id('CmbJobOrderNoInput').disabled = true; renderInput();
    }
    /** InputGridHistory:1968 */
    var H_COLS = [['RecordNo', null], ['DocDate', fmtShort], ['DocCode', null], ['ProductionJobOrderNo', null], ['WorkInProcessAccountTitle', null], ['WipItemName', null], ['EntryUserName', null], ['EntryDate', fmtShort], ['IsApproved', null], ['MainRemarks', null]];
    function inputHistory(n) {
        return getJson(api + '/history?documentTypeId=' + DOC_INPUT + '&noOfRecords=' + n).then(function (d) {
            hist.input = (d && d.rows) || []; histCur.input = hist.input.length ? 0 : -1;
            renderHist('inHistHead', 'inHistBody', hist.input, histCur.input, 'hinput', 'lblInHistCount');
            if (histCur.input >= 0) histDetail(intOf(hist.input[0].Id), 'inHistDetHead', 'inHistDetBody', 'inHistDetFoot', 'input');
        }).catch(function (e) { box(e.message); });
    }
    function renderHist(headId, bodyId, rows, cur, kind, countId) {
        $id(headId).innerHTML = '<th style="width:40px;">Edit</th><th style="width:40px;">Print</th>' + H_COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('');
        $id(bodyId).innerHTML = rows.length ? rows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="' + kind + '" data-i="' + i + '" class="' + (i === cur ? 'is-current' : '') + '">'
                + '<td><button type="button" class="fp-cellbtn" tabindex="-1" data-act="edit">Edit</button></td><td><button type="button" class="fp-cellbtn" tabindex="-1" data-act="print">Print</button></td>'
                + H_COLS.map(function (c) { var v = r[c[0]]; if (c[0] === 'IsApproved') v = r.IsApproved ? 'True' : 'False'; return '<td>' + esc(c[1] ? c[1](v) : v) + '</td>'; }).join('') + '</tr>';
        }).join('') : '<tr class="fp-empty"><td colspan="' + (H_COLS.length + 2) + '">No records</td></tr>';
        $id(countId).textContent = rows.length + ' record(s)';
    }
    /** InputDetailByHeaderId:2097 / OutPutDetailByHeaderId:3148 / DatagridHistoryDetail:4322 */
    function histDetail(id, headId, bodyId, footId, kind) {
        return getJson(api + '/' + id).then(function (d) {
            var rows = d.details || [], cols;
            if (kind === 'input') cols = [['WareHouseName', null], ['JobLotDescription', 'JobLot'], ['ItemName', null], ['PackingType', null], ['PackUom', null], ['Qty', fmtInt], ['Weight', fmtInt], ['Rate', fmtRate, 'rate'], ['RateUOM', null, 'rate'], ['Amount', fmtAmt, 'rate'], ['Remarks', null]];
            else if (kind === 'output') cols = [['WareHouseName', null], ['JobLotDescription', 'JobLot'], ['ItemName', null], ['PackingType', null], ['PackUom', null], ['Qty', fmtInt], ['Weight', fmtInt], ['Rate', fmtRate, 'rate'], ['RateUOM', null, 'rate'], ['Amount', fmtAmt, 'rate'], ['NetRate', fmtRate, 'rate'], ['TotalAmount', fmtAmt, 'rate'], ['Remarks', null]];
            else cols = [['EntryType', null], ['WareHouseName', null], ['JobLotCode', 'JobLot'], ['ItemName', null], ['PackingType', null], ['PackUom', null], ['Qty', function (v) { return fmtDec(v, 3); }], ['Weight', function (v) { return fmtDec(v, 3); }], ['Rate', fmtRate], ['RateUOM', null], ['Amount', fmtAmt], ['NetRate', fmtRate], ['TotalAmount', fmtAmt], ['Remarks', null]];
            cols = cols.filter(function (c) { return rights.rate || c[2] !== 'rate'; });
            simpleTable(headId, bodyId, footId, cols, rows, { Qty: 1, Weight: 1, Amount: 1, TotalAmount: 1 });
        }).catch(function (e) { box(e.message); });
    }
    function simpleTable(headId, bodyId, footId, cols, rows, sums) {
        $id(headId).innerHTML = cols.map(function (c) { var t = typeof c[1] === 'string' ? c[1] : c[0]; return '<th' + (typeof c[1] === 'function' ? ' class="num"' : '') + '>' + esc(t) + '</th>'; }).join('');
        $id(bodyId).innerHTML = rows.length ? rows.map(function (r) {
            return '<tr>' + cols.map(function (c) { var f = typeof c[1] === 'function' ? c[1] : null; return '<td' + (f ? ' class="num"' : '') + '>' + esc(f ? f(ci(r, c[0])) : ci(r, c[0])) + '</td>'; }).join('') + '</tr>';
        }).join('') : '<tr class="fp-empty"><td colspan="' + cols.length + '">No records</td></tr>';
        if (footId) $id(footId).innerHTML = rows.length ? '<tr>' + cols.map(function (c) {
            if (!sums[c[0]]) return '<td></td>';
            var t = 0; rows.forEach(function (r) { t += num(ci(r, c[0])); });
            var f = typeof c[1] === 'function' ? c[1] : null; return '<td class="num">' + esc(f ? f(t) : t) + '</td>';
        }).join('') + '</tr>' : '';
    }
    /** grdinputhistory_ColumnButtonClick:2060 */
    function inputHistAction(i, act) {
        var r = hist.input[i]; if (!r) return;
        histCur.input = i; renderHist('inHistHead', 'inHistBody', hist.input, i, 'hinput', 'lblInHistCount');
        if (act === 'edit') { $id('CmbJobOrderNoInput').disabled = true; if (r.IsApproved) { box("You Can't Update Approved Record"); return; } readInput(intOf(r.Id)); }
        else if (act === 'print') printSlip(intOf(r.Id), DOC_INPUT, null);
        else histDetail(intOf(r.Id), 'inHistDetHead', 'inHistDetBody', 'inHistDetFoot', 'input');
    }
    /** Print_Click:1903 */
    function printInput() { if (recId > 0) return printSlip(recId, DOC_INPUT, 'Print'); box('No Record Found'); }
    /** GenerateSlipInput_Output:1920 - FoodProductionSlip pre-check then 1512-FoodProductionSlip.rpt */
    function printSlip(id, documentTypeId, btnId) {
        var run = function () {
            return getJson(api + '/slip-check?id=' + id + '&documentTypeId=' + documentTypeId).then(function (d) {
                if (d && d.message) { box(d.message); return; }
                if (typeof window.printRpt !== 'function') { box('print-rpt.js is not loaded'); return; }
                return window.printRpt('1512-FoodProductionSlip.rpt', { id: id, documentTypeId: documentTypeId });
            }).catch(function (e) { box(e.message); });
        };
        return btnId ? busy(btnId, run) : run();
    }
    /** VoucherReprot:1306 - CommonServices.VoucherReport_118(VoucherHeadId) */
    function voucher118(kind, btnId) {
        var id = voucherHeadId[kind] || 0;
        if (!id) { box('No Record Found'); return; }
        if (window.CrystalPrint) return window.CrystalPrint.open('acc-118', { id: id }, btnId);
        window.open('/reports/print/118a-voucher-report?id=' + id, '_blank');
    }

    /* ================================================================== OUTPUT (1513) */
    var OUT_COLS = [['EntryType', null], ['WareHouseId', 'wh'], ['JobLotId', 'jl'], ['PackingTypeId', 'pt'], ['Item', null], ['PackUom', null], ['ItemQty', 'edit'], ['NetWeight', 'edit'],
        ['ItemRate', 'rate'], ['RateUom', 'rateuom'], ['ItemAmount', 'amount'], ['NetRate', 'rate'], ['NetAmount', 'amount'], ['Remarks', 'text']];
    var OUT_CAPTION = { WareHouseId: 'WareHouse Name', JobLotId: 'JobLot', PackingTypeId: 'Packing Type' };
    function renderOutput() {
        var cols = OUT_COLS.filter(function (c) { return rights.rate || (c[1] !== 'rate' && c[1] !== 'rateuom' && c[1] !== 'amount'); });   // gridsettingsOutPut:2400
        $id('outHead').innerHTML = '<th class="col-x">X</th><th class="col-x">+</th>' + cols.map(function (c) { return '<th' + (c[1] && c[1] !== 'text' && c[1] !== 'wh' && c[1] !== 'jl' && c[1] !== 'pt' ? ' class="num"' : '') + '>' + esc(OUT_CAPTION[c[0]] || c[0]) + '</th>'; }).join('');
        $id('outBody').innerHTML = outputRows.length ? outputRows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="output" data-i="' + i + '" class="' + (i === current.output ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="fp-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="fp-cellbtn" tabindex="-1" data-act="add">+</button></td>'
                + cols.map(function (c) {
                    var v = r[c[0]];
                    if (c[1] === 'wh') return '<td><select class="cellsel" data-col="WareHouseId" data-i="' + i + '">' + options(lists.warehouses, 'Id', 'WareHouseName', v) + '</select></td>';
                    if (c[1] === 'jl') return '<td><select class="cellsel" data-col="JobLotId" data-i="' + i + '">' + options(lists.jobLots, 'Id', 'JobLotDescription', v) + '</select></td>';
                    if (c[1] === 'pt') return '<td><select class="cellsel" data-col="PackingTypeId" data-i="' + i + '">' + options(lists.packingTypes, 'Id', 'PackTypeDesc', v) + '</select></td>';
                    if (c[1] === 'edit') return '<td class="num"><input type="text" class="cellin" data-col="' + c[0] + '" data-i="' + i + '" value="' + esc(fmtDec(v, 2)) + '"></td>';
                    if (c[1] === 'text') return '<td><input type="text" class="cellin txt" data-col="Remarks" data-i="' + i + '" value="' + esc(v) + '"></td>';
                    if (c[1] === 'rate') return '<td class="num">' + esc(fmtRate(v)) + '</td>';
                    if (c[1] === 'amount') return '<td class="num">' + esc(fmtAmt(v)) + '</td>';
                    if (c[1] === 'rateuom') return '<td class="num">' + esc(v) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '';
        var q = 0, w = 0, a = 0; outputRows.forEach(function (r) { q += num(r.ItemQty); w += num(r.NetWeight); a += num(r.ItemAmount); });
        $id('outFoot').innerHTML = outputRows.length ? '<tr><td></td><td></td>' + cols.map(function (c) {
            if (c[0] === 'ItemQty') return '<td class="num">' + esc(fmtDec(q, 2)) + '</td>';
            if (c[0] === 'NetWeight') return '<td class="num">' + esc(fmtDec(w, 2)) + '</td>';
            if (c[0] === 'ItemAmount') return '<td class="num">' + esc(fmtAmt(a)) + '</td>';
            return '<td></td>';
        }).join('') + '</tr>' : '';
        $id('lblOutputCount').textContent = outputRows.length;
    }
    /** grdByProduct_CellUpdated:2506 */
    function outputCell(i, col, value) {
        var r = outputRows[i]; if (!r) return;
        if (col === 'ItemQty') {
            var qty = num(value), uom = num(r.PackEquivalent); r.ItemQty = qty; r.NetWeight = qty * uom;
            var a = r.NetWeight / num(r.RateEquivalent) * num(r.ItemRate), n = r.NetWeight / num(r.RateEquivalent) * num(r.NetRate);
            r.ItemAmount = isFinite(a) ? a : 0; r.NetAmount = isFinite(n) ? n : 0;
        } else if (col === 'NetWeight') {
            var w = num(value); r.NetWeight = w;
            /* :2531 reads a "Rate" column the table does not have (the desktop throws there); ItemRate is the rate the row carries */
            var a2 = w / num(r.RateEquivalent) * num(r.ItemRate), n2 = w / num(r.RateEquivalent) * num(r.NetRate);
            r.ItemAmount = isFinite(a2) ? a2 : 0; r.NetAmount = isFinite(n2) ? n2 : 0;
        } else if (col === 'Remarks') { r.Remarks = value; return; }
        else if (col === 'WareHouseId' || col === 'JobLotId' || col === 'PackingTypeId') { r[col] = intOf(value); return; }
        renderOutput();
    }
    /** grdByProduct_ColumnButtonClick:2551 */
    function outputAction(i, act) {
        var r = outputRows[i]; if (!r) return;
        if (act === 'add') {
            var c = copy(r); outputRows.push(c);
            if (!saveModeOutput()) { box('You can not add row in Update Mode'); renderOutput(); return; }    // the desktop adds the row, then refuses to zero its Id
            c.Id = 0; renderOutput(); return;
        }
        if (act === 'del') {
            if (!ask('Are you sure to Delete?')) return;
            if (intOf(r.Id) > 0) { var d = copy(r); d.ActionTypeId = 3; removed.output.push(d); }
            outputRows.splice(i, 1); current.output = -1;
            if (!outputRows.length) $id('CmbJobOrderNoOutput').disabled = false;
            renderOutput();
        }
    }
    /** FormValidationOutPut:2241 */
    function outputOk() {
        if (val('txtDocNoOutPut').trim() === '' || val('txtDocNoOutPut') === '0') { box('DocNo Field Required'); focusEl('txtDocNoOutPut'); return false; }
        if (!hasRow('CmbJobOrderNoOutput')) { box('Job Order Number Field Required'); focusEl('CmbJobOrderNoOutput'); return false; }
        if (!hasRow('CmbPlantFeederOutput')) { box('Plant Name Field Is Required'); focusEl('CmbPlantFeederOutput'); return false; }
        if (!hasRow('CmbWorkInProcess')) { box('WorkInProcess Field Required'); focusEl('CmbWorkInProcess'); return false; }
        if (!hasRow('CmbWIPItem')) { box('WIP Item Field Required'); focusEl('CmbWIPItem'); return false; }
        return true;
    }
    /** OutPutInsert:2731 */
    function insertOutput(btnId) {
        return busy(btnId, function () {
            if (!outputOk()) return;
            var wasUpdate = recIdOutput > 0;
            if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            if (!outputRows.length) { box('Enter Detail First ...'); return; }
            var body = {
                Id: recIdOutput, DocumentTypeId: DOC_OUTPUT, DocCode: val('txtDocNoOutPut'), DocDate: val('DocDateOutPut'), MainRemarks: val('txtRemarksHeaderOutPut'),
                WIPAccountId: intOf(val('CmbWorkInProcess')), WIPItemId: intOf(val('CmbWIPItem')), ProductionJobOrderId: intOf(val('CmbJobOrderNoOutput')),
                ProductionJobOrderNo: selText('CmbJobOrderNoOutput').trim(), PlantId: intOf(val('CmbPlantFeederOutput')),
                details: removed.output.concat(outputRows.map(function (r) {
                    return { Id: r.Id, EntryTypeId: r.EntryTypeId, OutPutIdFromJobOrder: r.OutPutIdFromJobOrder, WareHouseId: r.WareHouseId, ItemId: r.ItemId, PackUomId: r.PackUomId,
                        JobLotId: r.JobLotId, PackingTypeId: r.PackingTypeId, ItemQty: r.ItemQty, NetWeight: r.NetWeight, ItemRate: r.ItemRate, RateUomId: r.RateUomId,
                        ItemAmount: r.ItemAmount, NetRate: r.NetRate, NetAmount: r.NetAmount, Remarks: r.Remarks, ActionTypeId: r.ActionTypeId };
                }))
            };
            return postJson(api + '/save', body).then(function (d) {
                box((d && d.message) || (wasUpdate ? 'Record Update Successfully' : 'Record Save Successfully'));
                resetOutput();
            }).catch(function (e) { box(e.message); });
        });
    }
    function saveOutput() { recIdOutput = 0; return insertOutput('btnSaveOutPut'); }      // btnSaveOutPut_Click:2706
    function updateOutput() { return insertOutput('btnUpdateOutPut'); }                   // btnUpdateOutPut_Click:2719
    function nextOutputCode() { return getJson(api + '/next-codes').then(function (d) { if (d && intOf(d.docNoOutput) > 0) setVal('txtDocNoOutPut', d.docNoOutput); }).catch(function () { /* silent */ }); }
    function nextOverheadCode() { return getJson(api + '/next-codes').then(function (d) { if (d && intOf(d.docNoOverhead) > 0) setVal('txtDocNoOverHead', d.docNoOverhead); }).catch(function () { /* silent */ }); }
    /** resetOutPut:2863 */
    function resetOutput() {
        removed.output = []; recIdOutput = 0; voucherHeadId.output = 0;
        ['txtInPutTotalQty', 'txtInPutTotalWeight', 'txtOutQtyTotal', 'txtOutWeightTotal', 'txtBalQtyTotal', 'txtBalWeightTotal'].forEach(function (id) { setVal(id, '0'); });
        vis.saveOut = true; vis.updateOut = false; $id('CmbJobOrderNoOutput').disabled = false;
        outputRows = []; current.output = -1; $id('lblRecIdOutput').textContent = '';
        setVal('CmbJobOrderNoOutput', ''); setVal('txtAvgFinishGoodRate', ''); setVal('CmbPlantFeederOutput', ''); setVal('txtRemarksHeaderOutPut', '');
        applyButtons(); renderOutput();
        return getJson(api + '/next-codes').then(function (d) { d = d || {}; if (intOf(d.docNoOutput) > 0) setVal('txtDocNoOutPut', d.docNoOutput); bindJobOrders(d); }).catch(function (e) { box(e.message); });
    }
    /** ReadByIdOutPut:2628 */
    function readOutput(id) {
        if (id <= 0) return Promise.resolve();
        return getJson(api + '/' + id).then(function (d) {
            var h = d.header || {};
            recIdOutput = id; vis.saveOut = false; vis.updateOut = true;
            setVal('txtDocNoOutPut', h.DocCode); setVal('DocDateOutPut', isoOf(h.DocDate)); setVal('txtRemarksHeaderOutPut', h.MainRemarks);
            fill('CmbJobOrderNoOutput', [{ Id: h.ProductionJobOrderId, Name: h.ProductionJobOrderNo }], 'Id', 'Name'); setVal('CmbJobOrderNoOutput', h.ProductionJobOrderId);
            $id('CmbJobOrderNoOutput').disabled = true;
            applyButtons();
            return jobOrderChanged('output').then(function () {
                setVal('CmbPlantFeederOutput', h.PlantId);
                outputRows = (d.details || []).map(function (x) {
                    if (intOf(x.EntryTypeId) === 2 && num(x.NetRate) > 0) setVal('txtAvgFinishGoodRate', x.NetRate);
                    return { Id: x.Id, OutPutIdFromJobOrder: x.JobOrderOutPutId, EntryTypeId: x.EntryTypeId, EntryType: x.EntryType, WareHouseId: x.WarehouseId, JobLotId: x.JobLotId,
                        PackingTypeId: x.PackingtypeId, ItemId: x.ItemId, Item: x.ItemName, PackUomId: x.PackUomId, PackUom: x.PackUom, PackEquivalent: x.PackUomEquivalent,
                        ItemQty: x.Qty, NetWeight: x.Weight, ItemRate: round(num(x.Rate), 2), RateUomId: x.RateUOMId, RateUom: x.RateUOM, RateEquivalent: x.RateUomEquivalent,
                        ItemAmount: x.Amount, NetRate: round(num(x.NetRate), 2), NetAmount: x.TotalAmount, Remarks: x.Remarks };
                });
                voucherHeadId.output = intOf(d.voucherHeadId);
                tab('OutPut'); sub('tabs3', 'tabPage5');
                $id('lblRecIdOutput').textContent = 'Record #' + id;
                renderOutput(); focusEl('DocDateOutPut');
            });
        }).catch(function (e) { box(e.message); });
    }
    /** btnLoadOutPutFromjobOrder_Click:3090 */
    function loadOutput() {
        if (!hasRow('CmbJobOrderNoOutput')) { focusEl('CmbJobOrderNoOutput'); box('Select Job Order No First...'); return; }
        openLoader('output', 'Output Data Against Job Order', api + '/load-output?jobOrderId=' + intOf(val('CmbJobOrderNoOutput')));
    }
    /** LoadOutPutDataFromJobOrder:3112 */
    function applyLoadOutput(rows) {
        rows.forEach(function (r) {
            var id = intOf(ci(r, 'Id')), exists = false;
            outputRows.forEach(function (x) { if (intOf(x.OutPutIdFromJobOrder) === id) exists = true; });
            if (exists) return;
            outputRows.push({
                Id: 0, OutPutIdFromJobOrder: id, EntryTypeId: intOf(ci(r, 'EntryTypeId')), EntryType: ci(r, 'EntryType'), WareHouseId: intOf(pick(r, ['WareHouseId', 'WarehouseId'])),
                JobLotId: intOf(ci(r, 'JobLotId')), PackingTypeId: intOf(ci(r, 'InvPackingTypeId')), ItemId: intOf(ci(r, 'ItemId')), Item: ci(r, 'ItemName'),
                PackUomId: intOf(ci(r, 'PackUomId')), PackUom: ci(r, 'OuterPackUom'), PackEquivalent: num(ci(r, 'OuterPackUomEquivalent')),
                ItemQty: num(pick(r, ['BalQty', 'balQty'])), NetWeight: num(pick(r, ['BalWeight', 'balWeight'])), ItemRate: num(ci(r, 'ItemRate')),
                RateUomId: intOf(ci(r, 'RateUomId')), RateUom: ci(r, 'RateUOM'), RateEquivalent: num(ci(r, 'RateUomEquivalent')),
                ItemAmount: num(pick(r, ['BalAmount', 'balAmount'])), NetRate: num(ci(r, 'NetRate')), NetAmount: num(pick(r, ['BalNetAmount', 'balNetAmount'])), Remarks: ci(r, 'RemarksDetail')
            });
        });
        $id('CmbJobOrderNoOutput').disabled = true; renderOutput();
    }
    /** OutputGridHistory:2909 */
    function outputHistory(n) {
        return getJson(api + '/history?documentTypeId=' + DOC_OUTPUT + '&noOfRecords=' + n).then(function (d) {
            hist.output = (d && d.rows) || []; histCur.output = hist.output.length ? 0 : -1;
            renderHist('outHistHead', 'outHistBody', hist.output, histCur.output, 'houtput', 'lblOutHistCount');
            if (histCur.output >= 0) histDetail(intOf(hist.output[0].Id), 'outHistDetHead', 'outHistDetBody', 'outHistDetFoot', 'output');
        }).catch(function (e) { box(e.message); });
    }
    /** grdOutputHistory_ColumnButtonClick:3001 */
    function outputHistAction(i, act) {
        var r = hist.output[i]; if (!r) return;
        histCur.output = i; renderHist('outHistHead', 'outHistBody', hist.output, i, 'houtput', 'lblOutHistCount');
        if (act === 'edit') { if (r.IsApproved) { box("You Can't Update Approved Record"); return; } readOutput(intOf(r.Id)); }
        else if (act === 'print') printSlip(intOf(r.Id), DOC_OUTPUT, null);
        else histDetail(intOf(r.Id), 'outHistDetHead', 'outHistDetBody', 'outHistDetFoot', 'output');
    }
    /** btnPrintOutPut_Click:3061 */
    function printOutput() { if (recIdOutput > 0) return printSlip(recIdOutput, DOC_OUTPUT, 'btnPrintOutPut'); box('No Record Found'); }

    /* ================================================================== OVER HEAD (1514) */
    var OH_COLS = [['ChartOfAccount', null], ['Brand', null], ['BrandUom', 'r'], ['ItemQty', 'edit'], ['ItemRate', 'editrate'], ['Amount', 'editamt'], ['Remarks', 'text']];
    function renderOh() {
        $id('ohHead').innerHTML = '<th class="col-x">X</th><th class="col-x">+</th>' + OH_COLS.map(function (c) { return '<th' + (c[1] && c[1] !== 'text' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id('ohBody').innerHTML = ohRows.length ? ohRows.map(function (r, i) {
            return '<tr tabindex="0" data-kind="oh" data-i="' + i + '" class="' + (i === current.oh ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="fp-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="fp-cellbtn" tabindex="-1" data-act="add">+</button></td>'
                + OH_COLS.map(function (c) {
                    var v = r[c[0]];
                    if (c[1] === 'edit') return '<td class="num"><input type="text" class="cellin" data-col="ItemQty" data-i="' + i + '" value="' + esc(fmtInt(v)) + '"></td>';
                    if (c[1] === 'editrate') return '<td class="num"><input type="text" class="cellin" data-col="ItemRate" data-i="' + i + '" value="' + esc(fmtRate(v)) + '"></td>';
                    if (c[1] === 'editamt') return '<td class="num"><input type="text" class="cellin" data-col="Amount" data-i="' + i + '" value="' + esc(fmtAmt(v)) + '"></td>';
                    if (c[1] === 'text') return '<td><input type="text" class="cellin txt" data-col="Remarks" data-i="' + i + '" value="' + esc(v) + '"></td>';
                    if (c[1] === 'r') return '<td class="num">' + esc(v) + '</td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '';
        var q = 0, a = 0, rt = 0; ohRows.forEach(function (r) { q += num(r.ItemQty); a += num(r.Amount); rt += num(r.ItemRate); });
        $id('ohFoot').innerHTML = ohRows.length ? '<tr><td></td><td></td><td></td><td></td><td></td><td class="num">' + esc(fmtInt(q)) + '</td><td class="num">' + esc(fmtRate(rt / ohRows.length)) + '</td><td class="num">' + esc(fmtAmt(a)) + '</td><td></td></tr>' : '';
        $id('lblOhCount').textContent = ohRows.length;
        $id('cmbJobOrderOverHead').disabled = ohRows.length > 0;        // JobOrderOverHeadLock:3569
    }
    /** grdDetailOverhead_CellUpdated:3733 */
    function ohCell(i, col, value) {
        var r = ohRows[i]; if (!r) return;
        if (col === 'ItemQty' || col === 'ItemRate') {
            r[col] = num(value);
            r.Amount = (num(r.ItemQty) !== 0 && num(r.ItemRate) !== 0) ? round(num(r.ItemQty) * num(r.ItemRate), amountDecimals) : 0;
        } else if (col === 'Amount') { r.Amount = num(value); r.ItemQty = 0; r.ItemRate = 0; }
        else if (col === 'Remarks') { r.Remarks = value; return; }
        renderOh();
    }
    /** grdDetailOverhead_ColumnButtonClick:3653 */
    function ohAction(i, act) {
        var r = ohRows[i]; if (!r) return;
        if (act === 'add') {
            if (!saveModeOh()) { box('You can not add row in Update Mode'); return; }
            var c = copy(r); c.Id = 0; ohRows.push(c); renderOh(); return;
        }
        if (act === 'del') {
            if (!ask('Are you sure to Delete?')) return;
            if (intOf(r.Id) > 0) { var d = copy(r); d.ActionId = 3; d.ActionTypeId = 3; removed.oh.push(d); }
            ohRows.splice(i, 1); current.oh = -1; renderOh();
        }
    }
    /** FormValidationOfOverHead:4020 */
    function ohOk() {
        if (val('txtDocNoOverHead').trim() === '') { box('Doc No. Field Required'); focusEl('txtDocNoOverHead'); return false; }
        if (!hasRow('cmbJobOrderOverHead')) { box('Job Order No. Field Required'); focusEl('cmbJobOrderOverHead'); return false; }
        if (!hasRow('CmbPlantFeederOverHead')) { box('Plant Field Required'); focusEl('CmbPlantFeederOverHead'); return false; }
        return true;
    }
    /** OverHeadInsert:3344 */
    function insertOverhead(btnId) {
        return busy(btnId, function () {
            if (!ohOk()) return;
            var wasUpdate = recOverHeadId > 0;
            if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var body = { RecOverHeadId: recOverHeadId, DocNo: val('txtDocNoOverHead'), DocDate: val('txtDocDateOverHead'), ProductionJobOrderId: intOf(val('cmbJobOrderOverHead')),
                PlantId: intOf(val('CmbPlantFeederOverHead')), rows: removed.oh.concat(ohRows) };
            return postJson(api + '/overhead/save', body).then(function (d) {
                box((d && d.message) || (wasUpdate ? 'Update Successfully' : 'Save Successfully'));
                resetOverhead();
            }).catch(function (e) { box(e.message); });
        });
    }
    function saveOverhead() { recOverHeadId = 0; return insertOverhead('btnSaveOverHead'); }    // btnSaveOverHead_Click:3468
    function updateOverhead() { return insertOverhead('btnUpdateOverHead'); }                   // btnUpdateOverHead_Click:3481
    /** FormResetOverHead:3541 */
    function resetOverhead() {
        recOverHeadId = 0; voucherHeadId.overhead = 0; vis.updateOh = false; vis.saveOh = true; $id('cmbJobOrderOverHead').disabled = false;
        ['txtOutputQtyTotalOH', 'txtOutputWeightTotalOH', 'txtInputQtyTotalOH', 'txtInputWeightTotalOH', 'txtfinishQtyOh', 'txtfinishWeightOh'].forEach(function (id) { setVal(id, ''); });
        setVal('cmbJobOrderOverHead', ''); setVal('CmbPlantFeederOverHead', ''); ohRows = []; removed.oh = []; current.oh = -1;
        applyButtons(); renderOh(); focusEl('cmbJobOrderOverHead');
        return nextOverheadCode();
    }
    /** ReadByIdOverHeadJobOrderWise:3493 */
    function readOverheadByJobOrder(jobOrderId) {
        return getJson(api + '/overhead/by-job-order?jobOrderId=' + jobOrderId).then(function (rows) {
            rows = rows || [];
            if (!rows.length) { box('Record Not Found'); return; }
            vis.saveOh = false; vis.updateOh = true; recOverHeadId = jobOrderId;
            tab('OverHead'); sub('tabs4', 'tabPage2');
            setVal('cmbJobOrderOverHead', rows[0].ProductionJobOrderId);
            if (!hasRow('cmbJobOrderOverHead')) { fill('cmbJobOrderOverHead', [{ Id: rows[0].ProductionJobOrderId, Name: 'Job Order #' + rows[0].ProductionJobOrderId }], 'Id', 'Name'); setVal('cmbJobOrderOverHead', rows[0].ProductionJobOrderId); }
            return jobOrderChanged('overhead').then(function () {
                setVal('CmbPlantFeederOverHead', rows[0].PlantId);
                ohRows = rows.map(function (r) { return { Id: r.Id, JobOrderOverHeadId: r.JobOrderOverHeadId, DocDate: r.DocDate, DocNo: r.DocNo, InvProductionJobOrderId: r.ProductionJobOrderId,
                    ChartOfAccountId: r.ChartOfAccountId, ChartOfAccount: r.ChartOfAccount, BrandId: r.BrandId, Brand: r.Brand, BrandUomId: r.BrandUomId, BrandUom: r.BrandUom,
                    ItemQty: r.ItemQty, ItemRate: r.ItemRate, Amount: r.Amount, Remarks: r.Remarks }; });
                removed.oh = [];
                applyButtons(); renderOh();
                $id('cmbJobOrderOverHead').disabled = true;
                return getJson(api + '/voucher-head?id=' + recOverHeadId + '&documentTypeId=' + DOC_OVERHEAD).then(function (v) { voucherHeadId.overhead = intOf(v && v.voucherHeadId); });   // GetVoucherHeadId(RecOverHeadId, 1514)
            });
        }).catch(function (e) { box(e.message); });
    }
    /** btnLoadOverHeadsFromJobOrder_Click:4145 */
    function loadOverheads() {
        if (!hasRow('cmbJobOrderOverHead')) { focusEl('cmbJobOrderOverHead'); box('Select Job Order No First...'); return; }
        openLoader('overhead', 'Input Data Against Job Order', api + '/load-overheads?jobOrderId=' + intOf(val('cmbJobOrderOverHead')));
    }
    /** LoadOverHeadsDataFromJobOrder:4167 */
    function applyLoadOverheads(rows) {
        rows.forEach(function (r) {
            var id = intOf(ci(r, 'Id')), exists = false;
            ohRows.forEach(function (x) { if (intOf(x.JobOrderOverHeadId) === id) exists = true; });
            if (exists) return;
            ohRows.push({ Id: 0, JobOrderOverHeadId: id, DocDate: today(), DocNo: 0, InvProductionJobOrderId: intOf(ci(r, 'ProductionJobOrderId')),
                ChartOfAccountId: intOf(ci(r, 'CharOfAccountId')), ChartOfAccount: ci(r, 'OverHeadAccount'), BrandId: intOf(ci(r, 'BrandId')), Brand: ci(r, 'BrandName'),
                BrandUomId: intOf(ci(r, 'BrandUomId')), BrandUom: ci(r, 'BrandUom'), ItemQty: num(pick(r, ['BalQty', 'balQty'])), ItemRate: num(ci(r, 'ItemRate')),
                Amount: num(pick(r, ['BalAmount', 'balAmount'])), Remarks: ci(r, 'RemarksOH') });
        });
        $id('cmbJobOrderOverHead').disabled = true; renderOh();
    }
    /** BindGridHistoryOverHead:3792 / OverHeadGridSetting:3851 */
    function overheadHistory(n) {
        return getJson(api + '/overhead/history?noOfRecords=' + n).then(function (rows) {
            hist.oh = rows || []; histCur.oh = hist.oh.length ? 0 : -1;
            $id('ohHistHead').innerHTML = '<th style="width:50px;">Edit</th><th style="width:50px;">Print</th><th>ProductionJobOrder</th><th class="num">TotalItemQty</th><th class="num">TotalExpense</th><th class="num">AverageItemRate</th><th>IsApproved</th>';
            $id('ohHistBody').innerHTML = hist.oh.length ? hist.oh.map(function (r, i) {
                return '<tr tabindex="0" data-kind="hoh" data-i="' + i + '" class="' + (i === histCur.oh ? 'is-current' : '') + '">'
                    + '<td><button type="button" class="fp-cellbtn" tabindex="-1" data-act="edit">Edit</button></td><td><button type="button" class="fp-cellbtn" tabindex="-1" data-act="print">Print</button></td>'
                    + '<td>' + esc(r.ProductionJobOrder) + '</td><td class="num">' + esc(fmtDec(r.TotalItemQty, 2)) + '</td><td class="num">' + esc(fmtAmt(r.TotalExpense)) + '</td><td class="num">' + esc(fmtRate(r.AverageItemRate)) + '</td><td>' + esc(r.IsApproved) + '</td></tr>';
            }).join('') : '<tr class="fp-empty"><td colspan="7">No records</td></tr>';
            var q = 0, a = 0, rt = 0; hist.oh.forEach(function (r) { q += num(r.TotalItemQty); a += num(r.TotalExpense); rt += num(r.AverageItemRate); });
            $id('ohHistFoot').innerHTML = hist.oh.length ? '<tr><td></td><td></td><td></td><td class="num">' + esc(fmtDec(q, 2)) + '</td><td class="num">' + esc(fmtAmt(a)) + '</td><td class="num">' + esc(fmtRate(rt / hist.oh.length)) + '</td><td></td></tr>' : '';
            $id('lblOhHistCount').textContent = hist.oh.length + ' job order(s)';
            if (histCur.oh >= 0) ohHistDetail(intOf(hist.oh[0].ProductionJobOrderId));
        }).catch(function (e) { box(e.message); });
    }
    /** grdOverHeadHistory_SelectionChanged:3924 */
    function ohHistDetail(jobOrderId) {
        return getJson(api + '/overhead/by-job-order?jobOrderId=' + jobOrderId).then(function (rows) {
            var cols = [['DocNo', null], ['DocDate', fmtDMY], ['ChartOfAccount', 'GLAccount'], ['Brand', 'BrandName'], ['BrandUom', null], ['ItemQty', function (v) { return fmtDec(v, 2); }], ['ItemRate', fmtRate], ['Amount', fmtAmt], ['Remarks', null]];
            simpleTable('ohHistDetHead', 'ohHistDetBody', 'ohHistDetFoot', cols, rows || [], { ItemQty: 1, ItemRate: 1, Amount: 1 });
        }).catch(function (e) { box(e.message); });
    }
    /** grdOverHeadHistory_ColumnButtonClick:3990 / grdOverHead_DoubleClick:3904 */
    function ohHistAction(i, act) {
        var r = hist.oh[i]; if (!r) return;
        histCur.oh = i; $id('ohHistBody').querySelectorAll('tr').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        if (act === 'edit' || act === 'dbl') { if (String(r.IsApproved) === 'True') { box("You Can't Update Approved Record"); return; } readOverheadByJobOrder(intOf(r.ProductionJobOrderId)); }
        else if (act === 'print') printOverheadFor(intOf(r.ProductionJobOrderId), null);
        else ohHistDetail(intOf(r.ProductionJobOrderId));
    }
    /** btnPrintOverHead_Click:4072 */
    function printOverhead() { if (hasRow('cmbJobOrderOverHead')) return printOverheadFor(intOf(val('cmbJobOrderOverHead')), 'btnPrintOverHead'); box('No Record Found'); }
    /** GenerateOverHeadPrint:4089 - pre-check then 1514-FoodProductionOverHeadJobOrderWise_Slip.rpt */
    function printOverheadFor(jobOrderId, btnId) {
        var run = function () {
            return getJson(api + '/summary-print-check?jobOrderId=' + jobOrderId + '&which=1514').then(function (d) {
                if (d && d.message) { box(d.message); return; }
                return window.printRpt('1514-FoodProductionOverHeadJobOrderWise_Slip.rpt', { jobOrderId: jobOrderId });
            }).catch(function (e) { box(e.message); });
        };
        return btnId ? busy(btnId, run) : run();
    }

    /* ================================================================== TRANSACTION HISTORY */
    var TH_COLS = [['DocCode', 'DocNo'], ['DocDate', fmtDMY], ['EntryType', null], ['ProductionJobOrderNo', 'JobOrderNo'], ['EntryDate', fmtShort], ['EntryUserName', 'EntryUser'], ['ModifyDate', fmtShort], ['ModifyUserName', 'ModifyUser'], ['ApprovedStatus', null], ['MainRemarks', 'Remarks']];
    /** GridHistory:4203 */
    function transactionHistory(n) {
        return getJson(api + '/history?documentTypeId=0&noOfRecords=' + n).then(function (d) {
            hist.th = (d && d.rows) || []; histCur.th = hist.th.length ? 0 : -1;
            $id('thHead').innerHTML = '<th style="width:40px;">Edit</th>' + TH_COLS.map(function (c) { return '<th>' + esc(typeof c[1] === 'string' ? c[1] : c[0]) + '</th>'; }).join('');
            $id('thBody').innerHTML = hist.th.length ? hist.th.map(function (r, i) {
                return '<tr tabindex="0" data-kind="hth" data-i="' + i + '" class="' + (i === histCur.th ? 'is-current' : '') + '">'
                    + '<td><button type="button" class="fp-cellbtn" tabindex="-1" data-act="edit">Edit</button></td>'
                    + TH_COLS.map(function (c) {
                        var v = c[0] === 'ApprovedStatus' ? (r.IsApproved ? 'Approved' : 'Not Approved') : r[c[0]];
                        return '<td>' + esc(typeof c[1] === 'function' ? c[1](v) : v) + '</td>';
                    }).join('') + '</tr>';
            }).join('') : '<tr class="fp-empty"><td colspan="' + (TH_COLS.length + 1) + '">No records</td></tr>';
            $id('lblThCount').textContent = hist.th.length + ' record(s)';
            if (histCur.th >= 0) histDetail(intOf(hist.th[0].Id), 'thDetHead', 'thDetBody', 'thDetFoot', 'th');
        }).catch(function (e) { box(e.message); });
    }
    /** DatagridHistory_ColumnButtonClick:4280 */
    function thAction(i, act) {
        var r = hist.th[i]; if (!r) return;
        histCur.th = i; $id('thBody').querySelectorAll('tr').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        if (act === 'edit') {
            if (r.IsApproved) { box("You Can't Update Approved Record"); return; }
            if (String(r.EntryType) === 'Input' || String(r.EntryType) === 'InPut') { $id('CmbJobOrderNoInput').disabled = true; readInput(intOf(r.Id)); }
            else { $id('CmbJobOrderNoOutput').disabled = true; readOutput(intOf(r.Id)); }
        } else histDetail(intOf(r.Id), 'thDetHead', 'thDetBody', 'thDetFoot', 'th');
    }

    /* ================================================================== SUMMERY */
    /** cmbsummeryJobOrderNo_TextChanged:4594 */
    function summaryJobOrderChanged() {
        var id = intOf(val('cmbsummeryJobOrderNo')); if (!id) return;
        getJson(api + '/job-order-info?jobOrderId=' + id).then(function (d) { if (d && d.plants && d.plants.length) fill('CmbPlantNameSummary', d.plants, 'Id', 'Name'); }).catch(function (e) { box(e.message); });
    }
    /** toolStripButton1_Click:4441 */
    function summaryNew() { setVal('cmbsummeryJobOrderNo', ''); focusEl('cmbsummeryJobOrderNo'); ['sumHead', 'sumBody', 'sumRepHead', 'sumRepBody', 'sumRepFoot'].forEach(function (id) { $id(id).innerHTML = ''; }); }
    var SUM_REP_COLS = [['DocNo', null], ['DocDate', fmtShort], ['ProductionJobOrderNo', null], ['PlantName', null], ['SortNo', null], ['TranType', null], ['ItemName', null], ['UOMCode', null], ['Remarks', null], ['JobLotCode', null],
        ['NetQty', function (v) { return fmtDec(v, 2); }], ['NetWeight', function (v) { return fmtDec(v, 2); }], ['Amount', fmtAmt, 'rate'], ['WeightPrcnt', null], ['OutPutPrcnt', function (v) { return fmtDec(v, 2); }], ['BpGgPrcnt', null],
        ['RMAmountTotal', fmtAmt], ['RmAvgRate', fmtRate], ['BpAmountTotal', fmtAmt], ['BpAvgRate', fmtRate], ['FgItemRate', fmtAmt], ['FgAmountTotal', fmtAmt], ['BpFgQty', null], ['BpFgWeight', null], ['BpFgAmount', fmtAmt], ['OhAmount', fmtAmt], ['FgOhAmount', fmtAmt], ['FgOhRate', fmtRate], ['NetRate', fmtRate]];
    /** btnShow_Click:4468 */
    function summaryShow() {
        var id = intOf(val('cmbsummeryJobOrderNo'));
        return busy('btnShow', function () {
            return getJson(api + '/summary?jobOrderId=' + id + '&plantId=' + intOf(val('CmbPlantNameSummary'))).then(function (d) {
                d = d || {};
                var rep = d.report || [];
                var cols = SUM_REP_COLS.filter(function (c) { return rights.rate || c[2] !== 'rate'; });   // SummeryHistoryGridSetting:4617 (TranRate / TotalAmountBySortNo hidden always)
                $id('sumRepHead').innerHTML = cols.map(function (c) { return '<th' + (typeof c[1] === 'function' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
                /* grouped by TranType (GroupByBoxVisible / Groups.Add("TranType") :4623) with group totals */
                var groups = {}; var order = [];
                rep.forEach(function (r) { var k = String(ci(r, 'TranType')); if (!groups[k]) { groups[k] = []; order.push(k); } groups[k].push(r); });
                var html = '';
                order.forEach(function (k) {
                    html += '<tr><td colspan="' + cols.length + '" style="background:#e4ecf7; font-weight:bold;">TranType: ' + esc(k) + ' (' + groups[k].length + ')</td></tr>';
                    groups[k].forEach(function (r) { html += '<tr>' + cols.map(function (c) { var f = typeof c[1] === 'function' ? c[1] : null; return '<td' + (f ? ' class="num"' : '') + '>' + esc(f ? f(ci(r, c[0])) : ci(r, c[0])) + '</td>'; }).join('') + '</tr>'; });
                    var sums = { NetQty: 0, NetWeight: 0, Amount: 0, OutPutPrcnt: 0, RMAmountTotal: 0 };
                    groups[k].forEach(function (r) { Object.keys(sums).forEach(function (s) { sums[s] += num(ci(r, s)); }); });
                    html += '<tr style="background:#f3f3f3; font-weight:bold;">' + cols.map(function (c) { var f = typeof c[1] === 'function' ? c[1] : null; return '<td class="num">' + (sums[c[0]] !== undefined ? esc(f ? f(sums[c[0]]) : sums[c[0]]) : '') + '</td>'; }).join('') + '</tr>';
                });
                $id('sumRepBody').innerHTML = rep.length ? html : '<tr class="fp-empty"><td colspan="' + cols.length + '">No records</td></tr>';
                var tot = { NetQty: 0, NetWeight: 0, Amount: 0, OutPutPrcnt: 0, RMAmountTotal: 0 };
                rep.forEach(function (r) { Object.keys(tot).forEach(function (s) { tot[s] += num(ci(r, s)); }); });
                $id('sumRepFoot').innerHTML = rep.length ? '<tr>' + cols.map(function (c) { var f = typeof c[1] === 'function' ? c[1] : null; return '<td class="num">' + (tot[c[0]] !== undefined ? esc(f ? f(tot[c[0]]) : tot[c[0]]) : '') + '</td>'; }).join('') + '</tr>' : '';
                $id('lblSumRepCount').textContent = rep.length + ' row(s)';
                var vals = d.values || [];
                var vcols = [['SortNo', null], ['TranType', null], ['NetQty', fmtInt], ['NetWeight', fmtInt], ['Amount', fmtAmt, 'rate'], ['AvgRate', fmtRate, 'rate'], ['AvgRateWoExp', fmtRate], ['ByProductOverHeads', fmtInt]].filter(function (c) { return rights.rate || c[2] !== 'rate'; });
                simpleTable('sumHead', 'sumBody', null, vcols, vals, {});
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnProductionRecoveryReport_Click:4736 - 1513-FoodProductionSummery_WithOutValue.rpt */
    function printSummary1513() {
        var id = intOf(val('cmbsummeryJobOrderNo'));
        return busy('btnProductionRecoveryReport', function () {
            return getJson(api + '/summary-print-check?jobOrderId=' + id + '&which=1513').then(function (d) {
                if (d && d.message) { box(d.message); return; }
                return window.printRpt('1513-FoodProductionSummery_WithOutValue.rpt', { summeryJobOrderNo: id, jobOrderId: id });
            }).catch(function (e) { box(e.message); });
        });
    }

    /* ================================================================== SETTLEMENT */
    var ST_IN = [['DocDate', fmtDMY], ['DocNo', null], ['WareHouse', null], ['ItemName', null], ['PackUom', null], ['JobLot', null], ['PackingType', null], ['Quantity', fmtInt], ['Weight', fmtInt], ['Rate', fmtRate], ['RateUOM', null], ['ItemAmount', fmtAmt], ['Remarks', null]];
    var ST_OUT = [['EntryType', null], ['DocDate', fmtDMY], ['DocNo', null], ['WareHouse', null], ['ItemName', null], ['PackUom', null], ['JobLot', null], ['PackingType', null], ['Quantity', function (v) { return fmtDec(v, 3); }], ['Weight', function (v) { return fmtDec(v, 3); }],
        ['Rate', 'edit'], ['ItemAmount', fmtAmt], ['RateWithoutExp', fmtRate], ['AmountWithoutExp', fmtAmt], ['NetRate', fmtRate], ['RateUOM', null], ['ItemOhCost', fmtAmt], ['GeneralOhCost', fmtAmt], ['TotalAmount', fmtAmt], ['Remarks', null]];
    /** btnGeneralSettlement_Click:4776 */
    function settlementGenerate() {
        var id = intOf(val('CmbJobOrderSettlement'));
        return busy('btnGeneralSettlement', function () {
            return getJson(api + '/settlement?jobOrderId=' + id).then(function (d) {
                d = d || {};
                settle.rows = []; settle.overheads = d.overheads || []; settle.values = d.values || []; settle.voucherHeadId = intOf(d.voucherHeadId);
                (d.rows || []).forEach(function (r) {
                    var et = String(ci(r, 'EntryType'));
                    var o = { Id: ci(r, 'Id'), DocDate: ci(r, 'DocDate'), DocNo: ci(r, 'DocCode'), WareHouseId: intOf(ci(r, 'WarehouseId')), WareHouse: ci(r, 'WareHouseName'), ItemId: intOf(ci(r, 'ItemId')), ItemName: ci(r, 'ItemName'),
                        PackUomId: intOf(ci(r, 'PackUomId')), PackUom: ci(r, 'PackUom'), JobLotId: intOf(ci(r, 'JobLotId')), JobLot: ci(r, 'JobLotDescription'), PackingTypeId: intOf(ci(r, 'PackingtypeId')), PackingType: ci(r, 'PackTypeDesc'),
                        Quantity: num(ci(r, 'Qty')), Weight: num(ci(r, 'Weight')), Rate: num(ci(r, 'Rate')), RateUOMId: intOf(ci(r, 'RateUOMId')), RateUOM: ci(r, 'RateUom'), ItemAmount: num(ci(r, 'Amount')), Remarks: ci(r, 'Remarks'), EntryType: et };
                    if (et === 'Input') { o.RefDocumentTypeId = ci(r, 'RefDocumentTypeId'); o.RefDocIdNo = ci(r, 'RefDocNoId'); }
                    if (et === 'Output') {
                        o.DetailId = intOf(ci(r, 'DetailId')); o.WIPAccountId = intOf(ci(r, 'WIPAccountId')); o.EntryTypeId = intOf(ci(r, 'EntryTypeId')); o.EntryType = ci(r, 'EntryTypeDetail');
                        o.RateWithoutExp = num(ci(r, 'Rate')); o.AmountWithoutExp = num(ci(r, 'Amount')); o.NetRate = num(ci(r, 'NetRate'));
                        o.ItemOhCost = num(ci(r, 'ItemOhCost')); o.GeneralOhCost = num(ci(r, 'GeneralOhCost')); o.TotalAmount = num(ci(r, 'TotalAmount')); o.kind = 'out';
                    } else o.kind = 'in';
                    settle.rows.push(o);
                });
                renderSettlement();
                /* FG totals -> the value boxes (:4900) */
                var netWeight = 0, fgAgainstJo = 0;
                settle.rows.forEach(function (r) { if (r.kind === 'out' && r.EntryTypeId === 2) { netWeight += num(r.Weight); fgAgainstJo += num(r.ItemAmount); } });
                applySettlementValues(settle.values, fgAgainstJo);
            }).catch(function (e) { box(e.message); });
        });
    }
    function applySettlementValues(vals, fgAgainstJo) {
        vals.forEach(function (v) {
            var t = String(ci(v, 'TranType'));
            if (t === 'Input Raw Material') { setVal('txtItemQtyRM', fmt00(ci(v, 'NetQty'))); setVal('txtNetWeightRM', fmt00(ci(v, 'NetWeight'))); setVal('txtAmountRM', fmt00(ci(v, 'Amount'))); }
            if (t === 'Over Heads') setVal('txtOverHeadsSettlement', fmt00(ci(v, 'Amount')));
            if (t === 'OutPut ManualRate') {
                var bp = num(ci(v, 'Amount')), bpOh = num(ci(v, 'ByProductOverHeads'));
                setVal('txtItemQtyBP', fmt00(ci(v, 'NetQty'))); setVal('txtNetWeightBP', fmt00(ci(v, 'NetWeight'))); setVal('txtAmountBP', fmt00(bp));
                setVal('txtBPOverHead', fmt00(bpOh)); setVal('txtBPTotalAmountWithExpenses', fmt00(bp + bpOh));
            }
            if (t === 'Output AutoRate') {
                var fg = num(ci(v, 'Amount'));
                settle.avgRate = num(ci(v, 'AvgRate'));
                setVal('txtAvgRateFG', String(round(num(ci(v, 'AvgRate')), 3))); setVal('txtFinishGoodRateWithOutExp', String(round(num(ci(v, 'AvgRateWoExp')), 3)));
                setVal('txtItemQtyFG', fmt00(ci(v, 'NetQty'))); setVal('txtNetWeightFG', fmt00(ci(v, 'NetWeight'))); setVal('txtAmountFG', fmt00(fg)); setVal('txtActualFinishGoodsAmount', fmt00(fg));
                setVal('txtFinishAmountAgainstJobOrder', fmt00(fgAgainstJo)); setVal('txtDifferenceAmount', fmt00(fg - fgAgainstJo));
            }
        });
        show('btnApproveSettlement', num(val('txtAmountFG')) > 0);           // :4981
    }
    function renderSettlement() {
        var ins = settle.rows.filter(function (r) { return r.kind === 'in'; }), outs = settle.rows.filter(function (r) { return r.kind === 'out'; });
        simpleTable('stInHead', 'stInBody', 'stInFoot', ST_IN, ins, { Quantity: 1, Weight: 1, ItemAmount: 1 });
        $id('stOutHead').innerHTML = ST_OUT.map(function (c) { return '<th' + (c[1] ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>'; }).join('');
        $id('stOutBody').innerHTML = outs.length ? outs.map(function (r) {
            var idx = settle.rows.indexOf(r);
            return '<tr>' + ST_OUT.map(function (c) {
                if (c[1] === 'edit') return '<td class="num"><input type="text" class="cellin" data-col="Rate" data-i="' + idx + '" value="' + esc(fmtRate(r.Rate)) + '"></td>';   // grdOutPutSettlement_CellUpdated:5216
                var f = typeof c[1] === 'function' ? c[1] : null; return '<td' + (f ? ' class="num"' : '') + '>' + esc(f ? f(r[c[0]]) : r[c[0]]) + '</td>';
            }).join('') + '</tr>';
        }).join('') : '<tr class="fp-empty"><td colspan="' + ST_OUT.length + '">No records</td></tr>';
        var sums = { Quantity: 0, Weight: 0, ItemAmount: 0, AmountWithoutExp: 0, ItemOhCost: 0, GeneralOhCost: 0, TotalAmount: 0 };
        outs.forEach(function (r) { Object.keys(sums).forEach(function (k) { sums[k] += num(r[k]); }); });
        $id('stOutFoot').innerHTML = outs.length ? '<tr>' + ST_OUT.map(function (c) { var f = typeof c[1] === 'function' ? c[1] : null; return '<td class="num">' + (sums[c[0]] !== undefined ? esc(f ? f(sums[c[0]]) : sums[c[0]]) : '') + '</td>'; }).join('') + '</tr>' : '';
        var oh = settle.overheads;
        var ohCols = oh.length ? Object.keys(oh[0]).filter(function (k) { return ['Id', 'CharOfAccountId', 'DocumentTypeId', 'BrandId', 'BrandUomId'].indexOf(k) < 0; }) : ['DocNo', 'DocDate', 'AccountTitle', 'ItemQty', 'ItemRate', 'Amount'];
        $id('stOhHead').innerHTML = ohCols.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('');
        $id('stOhBody').innerHTML = oh.length ? oh.map(function (r) {
            return '<tr>' + ohCols.map(function (k) {
                var v = r[k];
                if (k === 'DocNo') return '<td><button type="button" class="fp-link" onclick="SteelFP.printOverheadSettlement()">' + esc(v) + '</button></td>';   // ColumnType Link -> GenerateOverHeadPrint(CmbJobOrderSettlement)
                if (k === 'DocDate') return '<td>' + esc(fmtDMY(v)) + '</td>';
                if (k === 'ItemQty') return '<td class="num">' + esc(fmtInt(v)) + '</td>';
                if (k === 'ItemRate') return '<td class="num">' + esc(fmtRate(v)) + '</td>';
                if (k === 'Amount') return '<td class="num">' + esc(fmtAmt(v)) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('') : '<tr class="fp-empty"><td colspan="' + ohCols.length + '">No records</td></tr>';
        var oq = 0, oa = 0, orr = 0; oh.forEach(function (r) { oq += num(r.ItemQty); oa += num(r.Amount); orr += num(r.ItemRate); });
        $id('stOhFoot').innerHTML = oh.length ? '<tr>' + ohCols.map(function (k) { if (k === 'ItemQty') return '<td class="num">' + esc(fmtInt(oq)) + '</td>'; if (k === 'Amount') return '<td class="num">' + esc(fmtAmt(oa)) + '</td>'; if (k === 'ItemRate') return '<td class="num">' + esc(fmtRate(orr / oh.length)) + '</td>'; return '<td></td>'; }).join('') + '</tr>' : '';
    }
    /** grdOutPutSettlement_CellUpdated:5216 - Rate typed: the 40-equivalent UOM of the item, then Amount = Weight / RateUOM * Rate. */
    function settlementRateCell(i, value) {
        var r = settle.rows[i]; if (!r) return;
        r.Rate = num(value);
        (lists.uomList || []).some(function (u) {
            if (intOf(ci(u, 'ItemId')) === r.ItemId && intOf(ci(u, 'Equivalent')) === 40) { r.RateUOMId = intOf(ci(u, 'Id')); r.RateUOM = intOf(ci(u, 'Equivalent')); return true; }
            return false;
        });
        var ru = num(r.RateUOM), a = r.Weight / ru * r.Rate; r.ItemAmount = isFinite(a) ? a : 0;
        renderSettlement();
    }
    /** btnApplyAvgRateFG_Click:5247 */
    function settlementApplyRate() {
        var id = intOf(val('CmbJobOrderSettlement'));
        var netWeight = 0, itemAmount = 0;
        settle.rows.forEach(function (r) {
            if (r.kind !== 'out') return;
            if (r.EntryTypeId === 2) netWeight += num(r.Weight);
            if (num(val('txtAmountBP')) === 0 && r.EntryTypeId !== 2) itemAmount += num(r.ItemAmount);
        });
        if (itemAmount > 0) setVal('txtAmountBP', String(itemAmount));
        return busy('btnApplyAvgRateFG', function () {
            return getJson(api + '/summary-values?jobOrderId=' + id + '&netWeight=' + netWeight).then(function (vals) {
                vals = vals || [];
                if (vals.length) {
                    vals.forEach(function (v) { if (String(ci(v, 'TranType')) === 'Output AutoRate') { settle.avgRate = num(ci(v, 'AvgRate')); settle.avgRateWo = num(ci(v, 'AvgRateWoExp')); setVal('txtAvgRateFG', String(round(settle.avgRate, 3))); } });
                    proportionFinishGoodsSettlement();
                } else { setVal('txtAvgRateFG', '0'); setVal('txtFinishGoodRateWithOutExp', '0'); }
                show('btnApproveSettlement', num(val('txtAmountFG')) > 0);
            }).catch(function (e) { box(e.message); });
        });
    }
    /** ProporationFinishGoodsSettlement:5319 */
    function proportionFinishGoodsSettlement() {
        if (val('txtAvgRateFG') === '' || val('txtAvgRateFG') === '0') { box('Finish Goods AvgRate Not Found Please check!'); return; }
        var stop = false;
        settle.rows.forEach(function (r) {
            if (r.kind !== 'out' || stop) return;
            if (intOf(r.RateUOMId) === 0) {
                var u40 = (lists.uomList || []).filter(function (u) { return intOf(ci(u, 'ItemId')) === r.ItemId && intOf(ci(u, 'Equivalent')) === 40; });
                if (!u40.length) { box('40KG RateUom not define please check'); stop = true; return; }
                r.RateUOMId = intOf(ci(u40[0], 'Id')); r.RateUOM = 40;
            }
            var w = num(r.Weight), ru = num(r.RateUOM), ohc = num(r.ItemOhCost), goh = num(r.GeneralOhCost);
            if (r.EntryTypeId === 2) {
                var rate = settle.avgRateWo, wo = w * rate, withExp = wo + ohc + goh, brand = w > 0 ? withExp / w * ru : 0;
                r.RateWithoutExp = rate * ru; r.AmountWithoutExp = wo; r.NetRate = brand; r.TotalAmount = withExp;
            } else {
                var rate2 = num(r.Rate), wo2 = ru > 0 ? w / ru * rate2 : 0, withExp2 = wo2 + ohc + goh, bpRate = w > 0 ? withExp2 / w * ru : 0;
                r.RateWithoutExp = rate2; r.AmountWithoutExp = wo2; r.NetRate = bpRate; r.TotalAmount = withExp2;
            }
        });
        renderSettlement();
    }
    /** ResetSettlement:5400 */
    function settlementReset() {
        show('btnApproveSettlement', false);
        ['txtItemQtyRM', 'txtItemQtyBP', 'txtItemQtyFG', 'txtNetWeightRM', 'txtNetWeightBP', 'txtNetWeightFG', 'txtAmountRM', 'txtAmountBP', 'txtAmountFG', 'txtActualFinishGoodsAmount', 'txtFinishAmountAgainstJobOrder', 'txtDifferenceAmount', 'txtBPOverHead', 'txtBPPMCharges', 'txtBPTotalAmountWithExpenses', 'txtOverHeadsSettlement', 'txtAvgRateFG', 'txtFinishGoodRateWithOutExp'].forEach(function (id) { setVal(id, ''); });
        setVal('CmbJobOrderSettlement', ''); settle = { rows: [], overheads: [], values: [], voucherHeadId: 0, avgRate: 0, avgRateWo: 0 };
        ['stInHead', 'stInBody', 'stInFoot', 'stOutHead', 'stOutBody', 'stOutFoot', 'stOhHead', 'stOhBody', 'stOhFoot'].forEach(function (id) { $id(id).innerHTML = ''; });
        refreshJobOrders();
    }
    /** btnUpdateSettlement_Click:5638 -> UpdateSettlement:5447 */
    function settlementUpdate() {
        if (!ask('Are you sure to Update Settlement?')) return;
        return busy('btnUpdateSettlement', function () {
            var rows = settle.rows.filter(function (r) { return r.kind === 'out'; });
            return postJson(api + '/settlement/update', { ProductionJobOrderId: intOf(val('CmbJobOrderSettlement')), rows: rows }).then(function (d) {
                box((d && d.message) || 'Record has updated Successfully'); settlementReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnApproveSettlement_Click:5693 */
    function settlementApprove() {
        if (!ask('Are you sure to Approve Settlement?')) return;
        return busy('btnApproveSettlement', function () {
            return request(api + '/settlement/approve?jobOrderId=' + intOf(val('CmbJobOrderSettlement')), { method: 'POST', headers: {} }).then(function (d) {
                box((d && d.message) || 'Record Approved Successfully'); settlementReset();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** btnRecoveryReport_Click:5653 - 1513A_ProductionSummeryWithValues.rpt */
    function printSummary1513A() {
        var id = intOf(val('CmbJobOrderSettlement'));
        return busy('btnRecoveryReport', function () {
            return getJson(api + '/summary-print-check?jobOrderId=' + id + '&which=1513A').then(function (d) {
                if (d && d.message) { box(d.message); return; }
                return window.printRpt('1513A_ProductionSummeryWithValues.rpt', { jobOrderId: id, plantId: 0 });
            }).catch(function (e) { box(e.message); });
        });
    }
    /** grdOverHeadSettlement_LinkClicked:5204 */
    function printOverheadSettlement() { return printOverheadFor(intOf(val('CmbJobOrderSettlement')), null); }
    /** btnSettlementVoucher_Click:5773 - AcRptPurchaseSalesVoucherSlip_103(VoucherId, 178) */
    function settlementVoucher() { if (settle.voucherHeadId > 0) window.open('/accounts/reports/voucher-slip/pdf/' + settle.voucherHeadId, '_blank'); }

    /* ================================================================== loader dialog (the three Load*DataByJobOrder forms) */
    function openLoader(kind, title, url) {
        loader.kind = kind; loader.url = url; loader.rows = [];
        $id('loaderTitle').textContent = title; $id('loaderModal').classList.add('is-open');
        renderLoader(); loaderLoad();
    }
    function closeLoader() { $id('loaderModal').classList.remove('is-open'); }
    function loaderReset() { loader.rows = []; renderLoader(); }
    function loaderLoad() {
        return busy('btnJobOrderDefineLoad', function () {
            return getJson(loader.url).then(function (rows) { loader.rows = rows || []; renderLoader(); }).catch(function (e) { box(e.message); });
        });
    }
    function renderLoader() {
        var rows = loader.rows, cols = rows.length ? Object.keys(rows[0]) : [];
        $id('loaderHead').innerHTML = '<th><input type="checkbox" id="loaderAll"></th>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
        $id('loaderBody').innerHTML = rows.length ? rows.map(function (r, i) {
            return '<tr><td><input type="checkbox" class="loader-sel" data-i="' + i + '"></td>' + cols.map(function (c) { var v = r[c]; if (/date/i.test(c)) v = fmtDMY(v); return '<td>' + esc(v) + '</td>'; }).join('') + '</tr>';
        }).join('') : '<tr class="fp-empty"><td colspan="' + (cols.length + 1) + '">No records</td></tr>';
        var all = $id('loaderAll'); if (all) all.addEventListener('change', function () { document.querySelectorAll('.loader-sel').forEach(function (c) { c.checked = all.checked; }); });
    }
    function loaderApply() {
        var sel = Array.prototype.map.call(document.querySelectorAll('.loader-sel:checked'), function (c) { return loader.rows[+c.getAttribute('data-i')]; }).filter(Boolean);
        if (!sel.length) { box('Please Select Row first'); return; }
        if (loader.kind === 'input') applyLoadInput(sel); else if (loader.kind === 'output') applyLoadOutput(sel); else applyLoadOverheads(sel);
        closeLoader();
    }

    /* ================================================================== grid events */
    function rowOf(e) { var tr = e.target.closest('tr[data-kind]'); return tr ? { kind: tr.getAttribute('data-kind'), i: +tr.getAttribute('data-i') } : null; }
    function wire() {
        [['inBody', 'input', inputCell, function (i, act) { if (act === 'del') deleteInput(i); }],
         ['outBody', 'output', outputCell, outputAction],
         ['ohBody', 'oh', ohCell, ohAction]].forEach(function (g) {
            var body = $id(g[0]);
            body.addEventListener('click', function (e) {
                var r = rowOf(e); if (!r) return;
                current[g[1]] = r.i;
                body.querySelectorAll('tr[data-kind]').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === r.i); });
                var btn = e.target.closest('button[data-act]'); if (btn) g[3](r.i, btn.getAttribute('data-act'));
            });
            body.addEventListener('change', function (e) {
                var t = e.target; if (!t || !(t.classList.contains('cellin') || t.classList.contains('cellsel'))) return;
                g[2](+t.getAttribute('data-i'), t.getAttribute('data-col'), t.value);
            });
        });
        $id('stOutBody').addEventListener('change', function (e) { var t = e.target; if (t && t.classList.contains('cellin')) settlementRateCell(+t.getAttribute('data-i'), t.value); });
        [['inHistBody', inputHistAction], ['outHistBody', outputHistAction], ['ohHistBody', ohHistAction], ['thBody', thAction]].forEach(function (g) {
            var body = $id(g[0]);
            body.addEventListener('click', function (e) { var r = rowOf(e); if (!r) return; var btn = e.target.closest('button[data-act]'); g[1](r.i, btn ? btn.getAttribute('data-act') : 'select'); });
            body.addEventListener('dblclick', function (e) { var r = rowOf(e); if (!r || e.target.closest('button')) return; g[1](r.i, g[0] === 'ohHistBody' ? 'dbl' : 'select'); });
        });
    }

    /* ================================================================== keyboard (frmFoodProduction_KeyDown:1133) */
    function focusNext(from) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button, [tabindex="0"]'), function (el) {
            return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null && !(el.tagName === 'SELECT' && el.classList.contains('dtcombo') && getComputedStyle(el).display === 'none');
        });
        var i = all.indexOf(from); if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    function onKey(e) {
        var t = e.target, k = e.key, ctrl = e.ctrlKey || e.metaKey, code = e.code || '';
        if (k === 'Escape' && document.querySelector('.cx-modal.is-open')) { closeLoader(); return; }
        if (k === 'Enter' && !ctrl) {
            if (e.defaultPrevented || !t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.tagName === 'TR') return;
            if (t.closest && t.closest('.cx-modal')) return;
            e.preventDefault(); focusNext(t); return;
        }
        if (!ctrl) return;
        var ti = currentTab(), key = k.length === 1 ? k.toLowerCase() : k;
        if (code === 'Numpad1') { e.preventDefault(); tab('InPut'); return; }
        if (code === 'Numpad2') { e.preventDefault(); tab('OutPut'); focusEl('DocDateOutPut'); return; }
        if (code === 'Numpad4' && rights.settlement) { e.preventDefault(); tab('OverHead'); focusEl('txtDocDateOverHead'); return; }
        if (code === 'Numpad5') { e.preventDefault(); tab('TransctionHistory'); return; }
        if (code === 'Numpad6') { e.preventDefault(); tab('Summary'); focusEl('cmbsummeryJobOrderNo'); return; }
        if (ti === 2) {
            if (key === 's' && saveModeOh()) { e.preventDefault(); saveOverhead(); }
            else if (key === 'u' && vis.updateOh && rights.update) { e.preventDefault(); updateOverhead(); }
            else if (key === 'n') { e.preventDefault(); resetOverhead(); }
            else if (key === 'r') { e.preventDefault(); refreshJobOrders(); }
            return;
        }
        if (ti === 4 && k === 'F5') { e.preventDefault(); focusEl('cmbsummeryJobOrderNo'); return; }
        if (ti === 3) {
            if (k === 'Enter') { e.preventDefault(); if (histCur.th >= 0) thAction(histCur.th, 'edit'); }
            else if (key === 'd') { e.preventDefault(); if (histCur.th >= 0) thAction(histCur.th, 'select'); }
            return;
        }
        if (ti === 0) {
            if (key === 's' && saveModeInput()) { e.preventDefault(); saveInput(); }
            else if (key === 'u' && vis.update && rights.update) { e.preventDefault(); updateInput(); }
            else if (key === 'l') { e.preventDefault(); loadInput(); }
            else if (k === 'F5') { e.preventDefault(); focusEl('txtDocdate'); }
            else if (k === 'ArrowDown') { e.preventDefault(); var tr = $id('inBody').querySelector('tr'); if (tr) tr.focus(); }
            else if (key === 'n') { e.preventDefault(); resetInput(); }
            else if (key === 'r') { e.preventDefault(); refreshJobOrders(); }
            return;
        }
        if (ti === 1) {
            if (key === 's' && saveModeOutput()) { e.preventDefault(); saveOutput(); }
            else if (key === 'u' && vis.updateOut && rights.update) { e.preventDefault(); updateOutput(); }
            else if (k === 'F5') { e.preventDefault(); focusEl('DocDateOutPut'); }
            else if (k === 'ArrowDown') { e.preventDefault(); var tr2 = $id('outBody').querySelector('tr'); if (tr2) tr2.focus(); }
            else if (key === 'n') { e.preventDefault(); resetOutput(); }
            else if (key === 'r') { e.preventDefault(); refreshOutput(); }
        }
    }

    /* ================================================================== boot */
    function boot() {
        ['txtDocdate', 'DocDateOutPut', 'txtDocDateOverHead', 'txtfromDate', 'txtToDate'].forEach(function (id) { setVal(id, today()); });
        document.addEventListener('keydown', onKey);
        wire(); applyButtons();
        loadAll().catch(function (e) { box(e.message); });
    }

    window.SteelFP = {
        tab: tab, sub: sub, full: full, refreshJobOrders: refreshJobOrders, refreshOutput: refreshOutput, jobOrderChanged: jobOrderChanged,
        resetInput: resetInput, saveInput: saveInput, updateInput: updateInput, loadInput: loadInput, inputHistory: inputHistory, printInput: printInput, voucher118: voucher118,
        resetOutput: resetOutput, saveOutput: saveOutput, updateOutput: updateOutput, loadOutput: loadOutput, outputHistory: outputHistory, printOutput: printOutput,
        resetOverhead: resetOverhead, saveOverhead: saveOverhead, updateOverhead: updateOverhead, loadOverheads: loadOverheads, overheadHistory: overheadHistory, printOverhead: printOverhead,
        transactionHistory: transactionHistory,
        summaryJobOrderChanged: summaryJobOrderChanged, summaryNew: summaryNew, summaryShow: summaryShow, printSummary1513: printSummary1513,
        settlementGenerate: settlementGenerate, settlementReset: settlementReset, settlementApplyRate: settlementApplyRate, settlementUpdate: settlementUpdate,
        settlementApprove: settlementApprove, printSummary1513A: printSummary1513A, printOverheadSettlement: printOverheadSettlement, settlementVoucher: settlementVoucher,
        closeLoader: closeLoader, loaderReset: loaderReset, loaderLoad: loaderLoad, loaderApply: loaderApply
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot); else boot();
}());
