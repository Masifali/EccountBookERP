/* ============================================================================================
 * Screen 285 "Receipe (Branding)" — Architecture.WinApp.Mfg.frmBillOfMaterial_Manufacturing
 * (base.Name = "frmBillOfMaterial_Manufacturing"), DocumentTypeId 1120. Line numbers below are the .cs.
 *
 *   frmBillOfMaterial_Manufacturing_Load:307   rights, ItemFill, ConsumptionItems, uomgetAll, AccountTitleFill,
 *                                               Datetypefill, AllCombobind, DocumentNoFill, the three grids (one blank row each)
 *   FromReset:1275                              (reset)            btnRefresh_Click:1320 (refresh)
 *   Insert():1353                               FormValidation → confirm → POST save → "Data Save Successfully....  n"
 *   ReadById:1714 / btnDelete_Click:1814 / BOMSlip:1874 (1120_Recipe_Slip.rpt)
 *   grdInputMaterial_CellUpdated:724, grdOutPutByProduct_CellUpdated:964, grdOverHead_CellUpdated:1171, Calculation:1335
 *   History :1886-2421, KeyDown :2423, btnGenerateAvgRate_Click:3243, BtnSaveAs_Click:3282
 *
 * The three grids are edited in place (GridEX AllowEdit): "+" (Add column) appends a blank row, "X" removes one; the
 * Item / Account cells are pickers, the UOM cells list the item's UOM schedule (the desktop's F1 GrdPopUp).
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/production/recipe-branding/api';
    var RPT = '1120_Recipe_Slip.rpt';

    var look = {}, rights = {}, rateDecimals = 2, amountDecimals = 2;
    var recId = 0, updateMode = false;
    var vis = { save: true, update: false, saveAs: false, del: false };
    var inputs = [], consumption = [], overheads = [];
    var removedInputs = [], removedConsumption = [];
    var current = { input: -1, consumption: -1, oh: -1 };
    var historyRows = [], historyCurrent = -1;

    /* ------------------------------------------------------------------ helpers */
    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function focusEl(id) { var e = $id(id); if (!e) return; try { e.focus(); } catch (x) { /* ignore */ } }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;'); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function intOf(v) { return Math.trunc(num(v)); }
    function cv(id) { return intOf(val(id)); }
    function selText(id) { var el = $id(id); if (!el || el.selectedIndex < 0) return ''; var o = el.options[el.selectedIndex]; return (o && o.value !== '' && o.value !== '0') ? o.text : ''; }
    function fmt(v, places) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places === undefined ? 3 : places }); }
    function fmtRate(v) { return num(v).toLocaleString('en-US', { minimumFractionDigits: rateDecimals, maximumFractionDigits: rateDecimals }); }
    /** "#,#.###" — blank for zero (Calculation:1342). */
    function fmtCalc(v) { var n = num(v); return n === 0 ? '' : n.toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    /** Math.Round(x, n, MidpointRounding.AwayFromZero). */
    function roundAway(v, n) { var m = Math.pow(10, n); return Math.sign(v) * Math.round(Math.abs(v) * m) / m; }
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parts(v) { var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || '')); return m ? { y: m[1], M: m[2], d: m[3], h: m[4], mi: m[5] } : null; }
    function fmtDMY(v) { var p = parts(v); return p ? p.d + '-' + MONTHS[+p.M - 1] + '-' + p.y.slice(2) : String(v || ''); }
    function fmtStamp(v) { var p = parts(v); if (!p) return String(v || ''); var h = +(p.h || 0), tt = h < 12 ? 'AM' : 'PM', h12 = h % 12 === 0 ? 12 : h % 12; return p.d + '-' + p.M + '-' + p.y + ' ' + String(h12).padStart(2, '0') + ':' + (p.mi || '00') + ' ' + tt; }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function isoOf(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[0] : ''; }
    function csrf(opt) {
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header && opt.method && opt.method.toUpperCase() !== 'GET') opt.headers[header.getAttribute('content')] = token.getAttribute('content');
    }
    function request(url, options) {
        var opt = options || {};
        opt.credentials = 'same-origin'; opt.headers = opt.headers || {}; opt.headers.Accept = 'application/json';
        csrf(opt);
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && body.message) || t || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    }
    function getJson(url) { return request(url, {}); }
    function postJson(url, body) { return request(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }); }
    function qs(o) { var a = []; Object.keys(o).forEach(function (k) { if (o[k] !== undefined && o[k] !== null && o[k] !== '') a.push(k + '=' + encodeURIComponent(o[k])); }); return a.length ? '?' + a.join('&') : ''; }
    function fail(e) { box(e && e.message ? e.message : String(e)); }

    /** DropDownBind.BindDDL(..., ZeroIndex) — "...Select Any Value..." when zeroIndex; selection kept when still present. */
    function fill(selectId, rows, zeroIndex, keepValue) {
        var el = $id(selectId); if (!el) return;
        var keep = keepValue === undefined ? el.value : String(keepValue);
        var h = zeroIndex ? '<option value="0">...Select Any Value...</option>' : '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.value = keep;
        if (el.value !== keep) el.selectedIndex = 0;
    }
    function fillIf(id, rows, zeroIndex) { if (rows && rows.length) fill(id, rows, zeroIndex); }
    function setCombo(id, v) { var el = $id(id); if (!el) return; el.value = String(v === null || v === undefined ? '' : v); if (el.value !== String(v) && el.options.length) el.selectedIndex = 0; }
    function clearCombo(id) { var el = $id(id); if (el && el.options.length) el.selectedIndex = 0; }
    function uomsOf(itemId) { return (look.allUoms || []).filter(function (u) { return intOf(u.ItemId) === itemId; }); }

    /* ------------------------------------------------------------------ load */

    function applyRights() {
        rights = look.rights || {};
        $id('btnsave').disabled = !rights.save; $id('btnUpdate').disabled = !rights.update;        // :315-318
        $id('btnPrint').disabled = !rights.print; $id('btnDelete').disabled = !rights.delete;
        var missing = [];
        if (!rights.save) missing.push('Save'); if (!rights.update) missing.push('Update'); if (!rights.print) missing.push('Print'); if (!rights.delete) missing.push('Delete');
        var note = $id('rightsNote');
        note.textContent = missing.length ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.' : '';
        note.classList.toggle('is-hidden', !missing.length);
    }
    function applyVisibility() { show('btnsave', vis.save); show('btnUpdate', vis.update); show('BtnSaveAs', vis.saveAs); show('btnDelete', vis.del); }

    function init() {
        ['datDocDate', 'txtEffectedDate', 'txtFromdateHistory', 'txtToDateHistory'].forEach(function (id) { setVal(id, today()); });
        wire();
        getJson(api + '/lookups').then(function (d) {
            look = d || {};
            applyRights();
            rateDecimals = look.rateDecimals === undefined ? 2 : intOf(look.rateDecimals);
            amountDecimals = look.amountDecimals === undefined ? 2 : intOf(look.amountDecimals);
            fillIf('cmbItem', look.items, true);                                            // ItemFill:438 (ZeroIndex: true)
            fill('cmbDateTypeHistory', look.dateTypes, true);                               // Datetypefill:1967
            var dt = $id('cmbDateTypeHistory'); if (dt.options.length > 2) { dt.selectedIndex = 2; datePreset(); }   // Rows[2].Activate
            fillIf('CmbBomNoHistory', look.bomNames, true); fillIf('cmbItemhistory', look.headerItems, true);         // AllCombobind:1924/1942
            setVal('txtdocno', look.docNo);                                                  // DocumentNoFill:408
            inputs = [blankDetail()]; consumption = [blankDetail()]; overheads = [blankOverhead()];   // :352/:367/:378
            applyVisibility();
            renderAll();
            focusEl('txtBom');
        }).catch(fail);
    }
    function blankDetail() { return { Id: 0, ItemId: 0, ItemUomId: 0, Uom: 0, UomCode: '', ItemQty: 0, NetWeight: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0 }; }
    function blankOverhead() { return { Id: 0, OverHeadTypeId: 0, OverHeadAccountId: 0, PercentOfTotalInput: 0, Qty: 0, Rate: 0, Amount: 0, OhRemarks: '' }; }

    /** btnRefresh_Click:1320. */
    function refresh() {
        getJson(api + '/refresh').then(function (d) {
            Object.keys(d || {}).forEach(function (k) { look[k] = d[k]; });
            fillIf('cmbItem', look.items, true);
            renderAll();
        }).catch(fail);
    }

    /** FromReset:1275. */
    function reset() {
        recId = 0;
        setVal('txtBom', ''); setVal('txtEffectedDate', today()); clearCombo('cmbItem'); $id('cmbUOM').innerHTML = '<option value=""></option>';
        setVal('txtQty', ''); setVal('txtRemarksHeader', '');
        inputs = [blankDetail()]; consumption = [blankDetail()]; overheads = [blankOverhead()];
        removedInputs = []; removedConsumption = [];
        current = { input: -1, consumption: -1, oh: -1 };
        vis = { save: true, update: false, saveAs: false, del: false }; updateMode = false; applyVisibility();
        $id('lblRecId').textContent = '';
        ['txtTotalInputs', 'txtOverHead', 'txtLessByProduct', 'txtNetValue'].forEach(function (id) { setVal(id, ''); });
        renderAll();
        focusEl('datDocDate');
        return getJson(api + '/doc-no').then(function (d) { setVal('txtdocno', d.docNo); }).catch(fail);   // DocumentNoFill:1282
    }

    /** cmbItem_Leave:530 — the header UOM list (UOMDescription) for the item, selection kept. */
    function itemLeave() {
        var prev = cv('cmbUOM');
        var list = uomsOf(cv('cmbItem')).map(function (u) { return { Id: u.Id, Name: u.UOMDescription }; });
        if (!list.length) { return; }
        fill('cmbUOM', list, false, prev > 0 ? prev : '');
    }

    /* ------------------------------------------------------------------ tabs */
    function currentTab() { return $id('tabHistory').classList.contains('is-active') ? 1 : 0; }
    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs1').querySelectorAll('.rc-tab1').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        if (id === 'tabHistory') {                                                             // tabControl1_SelectedIndexChanged:1977
            var dt = $id('cmbDateTypeHistory'); if (dt.options.length > 2) { dt.selectedIndex = 2; datePreset(); }
            getJson(api + '/history-combos').then(function (d) { fillIf('CmbBomNoHistory', d.bomNames, true); fillIf('cmbItemhistory', d.headerItems, true); }).catch(fail);
        }
    }

    /* ------------------------------------------------------------------ grids */

    function uomOptions(itemId, selectedId, defaultCode, defaultEq, blankAllowed) {
        var h = '';
        if (blankAllowed) h += '<option value=""' + (!selectedId && !defaultCode ? ' selected' : '') + '></option>';
        if (defaultCode) h += '<option value="0" data-eq="' + defaultEq + '" data-code="' + esc(defaultCode) + '"' + (!selectedId ? ' selected' : '') + '>' + esc(defaultCode) + '</option>';
        uomsOf(itemId).forEach(function (u) {
            h += '<option value="' + esc(u.Id) + '" data-eq="' + num(u.Equivalent) + '" data-code="' + esc(u.UOMCode) + '"' + (intOf(u.Id) === selectedId ? ' selected' : '') + '>' + esc(u.UOMCode) + '</option>';
        });
        return h;
    }
    function itemOptions(list, selectedId) {
        var h = '<option value="0"></option>';
        (list || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '"' + (intOf(r.Id) === selectedId ? ' selected' : '') + '>' + esc(r.Name) + '</option>'; });
        return h;
    }
    /** grdInputSettings:672 / grdConsumptionGridSetting:911 — X, +, Item Name, Uom, ItemQty, NetWeight, Rate, Rate Uom, Amount. */
    function renderDetail(tableId, rows, itemList, kind) {
        var t = $id(tableId);
        t.tHead.innerHTML = '<tr><th class="col-x">X</th><th class="col-x">+</th><th style="min-width:220px">Item Name</th><th>Uom</th><th>ItemQty</th><th>NetWeight</th><th>Rate</th><th>Rate Uom</th><th>Amount</th></tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            var def = kind === 'input' && intOf(r.ItemUomId) === 0 && intOf(r.ItemId) > 0;
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === current[kind] ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="rc-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="rc-add" tabindex="-1" data-act="add">+</button></td>'
                + '<td><select class="cell" data-k="ItemId" data-i="' + i + '">' + itemOptions(itemList, intOf(r.ItemId)) + '</select></td>'
                + '<td><select class="cell" data-k="UomCode" data-i="' + i + '">' + uomOptions(intOf(r.ItemId), intOf(r.ItemUomId), def ? (r.UomCode || '1Kg') : '', 1, !def) + '</select></td>'
                + '<td class="num"><input type="text" class="cell" data-k="ItemQty" data-i="' + i + '" value="' + esc(fmtPlain(r.ItemQty)) + '"></td>'
                + '<td class="num">' + esc(fmt(r.NetWeight, 3)) + '</td>'
                + '<td class="num"><input type="text" class="cell" data-k="Rate" data-i="' + i + '" value="' + esc(fmtPlain(r.Rate)) + '"></td>'
                + '<td><select class="cell" data-k="RateUomCode" data-i="' + i + '">' + uomOptions(intOf(r.ItemId), intOf(r.RateUomId), intOf(r.RateUomId) === 0 && intOf(r.ItemId) > 0 && kind === 'input' ? (r.RateUomCode || '1Kg') : '', num(r.RateUom) || 1, kind !== 'input' || intOf(r.ItemId) === 0) + '</select></td>'
                + '<td class="num">' + esc(kind === 'input' ? fmt(r.Amount, 3) : fmt(r.Amount, 2)) + '</td></tr>';
        }).join('');
        var sq = 0, sw = 0, sa = 0, sr = 0;
        rows.forEach(function (r) { sq += num(r.ItemQty); sw += num(r.NetWeight); sa += num(r.Amount); sr += num(r.Rate); });
        t.tFoot.innerHTML = rows.length ? '<tr><td></td><td></td><td></td><td></td><td>' + fmt(sq, 3) + '</td><td>' + fmt(sw, 3) + '</td><td>' + (kind === 'input' ? '' : fmtRate(sr)) + '</td><td></td><td>' + fmt(sa, 3) + '</td></tr>' : '';
    }
    function fmtPlain(v) { var n = num(v); return n === 0 ? '0' : String(n); }
    /** grdOverHeadGridSetting:1134 — X, +, AccountTitle, Percentage, Qty, Rate, Amount, OhRemarks. */
    function renderOverheads() {
        var t = $id('grdOverHead');
        t.tHead.innerHTML = '<tr><th class="col-x">X</th><th class="col-x">+</th><th style="min-width:220px">AccountTitle</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Amount</th><th>OhRemarks</th></tr>';
        t.tBodies[0].innerHTML = overheads.map(function (r, i) {
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === current.oh ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="rc-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="rc-add" tabindex="-1" data-act="add">+</button></td>'
                + '<td><select class="cell" data-k="OverHeadAccountId" data-i="' + i + '">' + itemOptions(look.accounts, intOf(r.OverHeadAccountId)) + '</select></td>'
                + '<td class="num"><input type="text" class="cell" data-k="PercentOfTotalInput" data-i="' + i + '" value="' + esc(fmtPlain(r.PercentOfTotalInput)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Qty" data-i="' + i + '" value="' + esc(fmtPlain(r.Qty)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Rate" data-i="' + i + '" value="' + esc(fmtPlain(r.Rate)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Amount" data-i="' + i + '" value="' + esc(fmtPlain(r.Amount)) + '"></td>'
                + '<td><input type="text" class="cell txt" data-k="OhRemarks" data-i="' + i + '" value="' + esc(r.OhRemarks) + '"></td></tr>';
        }).join('');
        var sp = 0, sq = 0, sa = 0;
        overheads.forEach(function (r) { sp += num(r.PercentOfTotalInput); sq += num(r.Qty); sa += num(r.Amount); });
        t.tFoot.innerHTML = overheads.length ? '<tr><td></td><td></td><td></td><td>' + fmt(sp, 2) + '</td><td>' + fmt(sq, 2) + '</td><td></td><td>' + fmt(sa, 2) + '</td><td></td></tr>' : '';
        $id('lblOverheadCount').textContent = overheads.length;
    }
    function renderAll() {
        renderDetail('grdInputMaterial', inputs, look.items, 'input'); $id('lblInputCount').textContent = inputs.length;
        renderDetail('grdConsumption', consumption, look.consumptionItems, 'consumption'); $id('lblConsumptionCount').textContent = consumption.length;
        renderOverheads();
    }

    /** CellUpdated :783 / :1009 — Amount = Qty × Uom / RateUom × Rate when Rate and RateUom are set, else 0. */
    function recalcAmount(r) {
        if (num(r.Rate) !== 0 && num(r.RateUom) !== 0) r.Amount = num(r.ItemQty) * num(r.Uom) / num(r.RateUom) * num(r.Rate); else r.Amount = 0;
    }
    /** grdInputMaterial_CellUpdated:724 (kind input) / grdOutPutByProduct_CellUpdated:964 (kind consumption). */
    function detailCellChanged(kind, i, key, el) {
        var rows = kind === 'input' ? inputs : consumption, r = rows[i]; if (!r) return;
        if (key === 'ItemId') {
            r.ItemId = intOf(el.value);
            if (kind === 'input') {
                getJson(api + '/avg-rate' + qs({ itemId: r.ItemId, docDate: val('datDocDate') })).then(function (d) {    // getAvgRateByItemId:734
                    r.Rate = num(d && d.rate); r.UomCode = '1Kg'; r.Uom = 1; r.RateUomCode = '1Kg'; r.RateUom = 1; r.ItemUomId = 0; r.RateUomId = 0;
                    r.NetWeight = num(r.ItemQty) * 1;
                    recalcAmount(r); calculation(); renderAll();
                }).catch(fail);
                return;
            }
            r.ItemUomId = 0; r.Uom = 0; r.UomCode = ''; r.RateUom = 0; r.RateUomId = 0; r.RateUomCode = ''; r.NetWeight = 0; r.Amount = 0;   // :974
        } else if (key === 'UomCode') {                                                        // F1 on UomCode :129-148
            var o = el.options[el.selectedIndex];
            if (o && o.value !== '') {
                r.ItemUomId = intOf(o.value); r.Uom = num(o.getAttribute('data-eq')); r.UomCode = o.getAttribute('data-code') || '';
                if (intOf(r.RateUomId) === 0 && kind === 'input') { r.RateUom = r.Uom; r.RateUomId = r.ItemUomId; r.RateUomCode = r.UomCode; }
                if (kind === 'consumption' && intOf(r.RateUomId) === 0) { r.RateUom = r.Uom; r.RateUomId = r.ItemUomId; r.RateUomCode = r.UomCode; }
            }
            r.NetWeight = num(r.ItemQty) * num(r.Uom);
            recalcAmount(r);
        } else if (key === 'RateUomCode') {                                                    // F1 on RateUomCode :150-164
            var o2 = el.options[el.selectedIndex];
            if (o2 && o2.value !== '') { r.RateUomId = intOf(o2.value); r.RateUom = num(o2.getAttribute('data-eq')); r.RateUomCode = o2.getAttribute('data-code') || ''; }
            recalcAmount(r);
        } else if (key === 'ItemQty') {
            r.ItemQty = num(el.value);
            r.NetWeight = num(r.ItemQty) * num(r.Uom);
            recalcAmount(r);
        } else if (key === 'Rate') {
            r.Rate = num(el.value);
            if (String(r.ItemQty) === '' || (kind === 'consumption' && intOf(r.ItemUomId) === 0 && num(r.Uom) === 0)) {
                renderAll(); box('ItemQty And ItemUom Should be greater than zero...'); return;
            }
            recalcAmount(r);
        }
        calculation();
        renderAll();
    }
    /** grdOverHead_CellUpdated:1171. */
    function overheadCellChanged(i, key, el) {
        var r = overheads[i]; if (!r) return;
        if (key === 'OverHeadAccountId') { r.OverHeadAccountId = intOf(el.value); r.OverHeadTypeId = r.OverHeadAccountId; }
        else if (key === 'Qty' || key === 'Rate') {
            r[key] = num(el.value);
            if (el.value.trim() === '' || (key === 'Qty' ? String(r.Rate) === '' : String(r.Qty) === '')) { renderAll(); box('Qty And Rate Should be greater than zero...'); return; }
            r.Amount = num(r.Qty) * num(r.Rate);
        } else if (key === 'PercentOfTotalInput') {
            r.PercentOfTotalInput = num(el.value);
            var total = overheads.reduce(function (a, x) { return a + num(x.PercentOfTotalInput); }, 0);
            if (total > 100) { r.PercentOfTotalInput = num(r.PercentOfTotalInput) - (total - 100); renderAll(); box('TotalPercentage Can not be Greater than 100...'); return; }
            if (el.value.trim() !== '') {
                var inputTotal = inputs.reduce(function (a, x) { return a + num(x.Amount); }, 0);
                var pct = inputTotal / 100 * num(r.PercentOfTotalInput);
                r.Amount = pct > 0 ? roundAway(pct, amountDecimals) : 0;
                r.Qty = 0; r.Rate = 0;
            }
        } else if (key === 'Amount') { r.Amount = num(el.value); r.Qty = 0; r.Rate = 0; }
        else if (key === 'OhRemarks') { r.OhRemarks = el.value; }
        calculation();
        renderAll();
    }
    /** grdInputMaterial_ColumnButtonClick:810 / grdOutPutByProduct_ColumnButtonClick:1033 (Delete / Add). */
    function detailButton(kind, i, act) {
        var rows = kind === 'input' ? inputs : consumption, removed = kind === 'input' ? removedInputs : removedConsumption;
        if (act === 'del') {
            var r = rows[i]; if (!r) return;
            if (intOf(r.Id) > 0) {
                if (!ask('Are you sure to Delete?')) return;
                removed.push({ Id: r.Id, ItemId: r.ItemId, ItemUomId: r.ItemUomId, ItemQty: r.ItemQty, NetWeight: r.NetWeight, Rate: r.Rate, RateUomId: r.RateUomId, Amount: r.Amount, ActionTypeId: 3 });
            }
            rows.splice(i, 1);
            if (!rows.length) rows.push(blankDetail());
        }
        if (act === 'add') rows.push(blankDetail());
        current[kind] = -1;
        calculation(); renderAll();
    }
    /** grdOverHead_ColumnButtonClick:1240 — no removed list for overheads. */
    function overheadButton(i, act) {
        if (act === 'del') { overheads.splice(i, 1); if (!overheads.length) overheads.push(blankOverhead()); }
        if (act === 'add') overheads.push(blankOverhead());
        current.oh = -1;
        calculation(); renderAll();
    }

    /** Calculation:1335 — Net Value = Inputs + OverHeads + Consumption (the desktop adds, as written). */
    function calculation() {
        var si = inputs.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        var sc = consumption.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        var so = overheads.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        setVal('txtTotalInputs', fmtCalc(Math.round(si * 100) / 100));
        setVal('txtOverHead', fmtCalc(Math.round(so * 100) / 100));
        setVal('txtLessByProduct', fmtCalc(Math.round(sc * 100) / 100));
        setVal('txtNetValue', fmtCalc(si + so + sc));
    }

    /** btnGenerateAvgRate_Click:3243. */
    function generateAvgRate() {
        if (!inputs.length) return;
        var docDate = val('datDocDate');
        inputs.reduce(function (p, r) {
            return p.then(function () {
                return getJson(api + '/avg-rate' + qs({ itemId: intOf(r.ItemId), docDate: docDate })).then(function (d) {
                    r.Rate = num(d && d.rate);
                    if (num(r.Rate) !== 0 && num(r.RateUom) !== 0) { r.NetWeight = num(r.ItemQty) * num(r.Uom); r.Amount = num(r.ItemQty) * num(r.Uom) / num(r.RateUom) * num(r.Rate); }
                    else r.Amount = 0;
                });
            });
        }, Promise.resolve()).then(function () { calculation(); renderAll(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ save / delete / print */

    /** FormValidation:593. */
    function headerOk() {
        if (intOf(val('txtdocno')) === 0) { box('doc No Field Required'); focusEl('txtdocno'); return false; }
        if (isoOf(val('datDocDate')) > isoOf(val('txtEffectedDate'))) { box('DocDate cannot be greater than Effected Date'); focusEl('txtEffectedDate'); return false; }
        var bom = val('txtBom').trim();
        if (bom === '' || bom === '0') { box('Recipe No Field Required'); focusEl('txtBom'); return false; }
        if (cv('cmbItem') === 0) { box('Item Field Required'); focusEl('cmbItem'); return false; }
        if (cv('cmbUOM') === 0) { box('IteUom Field Required'); focusEl('cmbUOM'); return false; }
        return true;
    }
    function body() {
        return { Id: recId, DocNo: intOf(val('txtdocno')), DocDate: val('datDocDate'), EffectedDate: val('txtEffectedDate'), BomName: val('txtBom'), ItemId: cv('cmbItem'), ItemUomId: cv('cmbUOM'),
            BomBatchQty: num(val('txtQty')), RemarksHeader: val('txtRemarksHeader'), IsActive: $id('ChkIsActive').checked, IsDefault: $id('chkIsDefault').checked,
            inputs: inputs.map(function (r) { return Object.assign({ ItemName: itemName(look.items, r.ItemId) }, r); }),
            consumption: consumption.map(function (r) { return Object.assign({ ItemName: itemName(look.consumptionItems, r.ItemId) }, r); }),
            overheads: overheads, removedInputs: removedInputs, removedConsumption: removedConsumption };
    }
    function itemName(list, id) { var r = (list || []).filter(function (x) { return intOf(x.Id) === intOf(id); })[0]; return r ? r.Name : ''; }
    /** Insert():1353. */
    function insert(btnId) {
        if (!headerOk()) return;
        if (!inputs.length) { box('Grid Record Not Found...\nGrid (Input Material) Should have atleast one record'); return; }
        var wasUpdate = recId > 0;
        if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var b = $id(btnId); if (b) b.classList.add('is-busy');
        postJson(api + '/save', body()).then(function (res) {
            box(res.message);
            var printId = intOf(res.id);
            return reset().then(function () { if ($id('ChkPreview').checked && rights.print) bomSlip(printId); });   // :1678
        }).catch(fail).then(function () { if (b) b.classList.remove('is-busy'); });
    }
    function save() { recId = 0; insert('btnsave'); }                       // btnsave_Click:1689
    function update() { insert('btnUpdate'); }                               // btnUpdate_Click:1702
    function saveAs() { recId = 0; insert('BtnSaveAs'); }                     // BtnSaveAs_Click:3282
    /** btnDelete_Click:1814. */
    function deleteClick() {
        if (recId > 0) {
            if (!ask('Are you sure to Delete?')) return;
            request(api + '/delete?id=' + recId, { method: 'POST' }).then(function (res) { box(res.message); reset(); }).catch(fail);
        } else box('No record found to Delete');
    }
    /** BOMSlip:1874 → CommonServices.Recipe_Slip1120(id) → 1120_Recipe_Slip.rpt. */
    function bomSlip(id) {
        if (!id) { box('Record Id Not Found'); return; }
        window.printRpt(RPT, { id: id, documentTypeId: 1120, branchesId: look.branchId || 0 });
    }
    function print() { bomSlip(recId); }                                      // btnPrint_Click:1862

    /* ------------------------------------------------------------------ read */

    /** ReadById:1714. */
    function readById(id) {
        return getJson(api + '/' + id).then(function (d) {
            var h = d.header || {};
            recId = id;
            tab('tabForm');
            setVal('datDocDate', isoOf(h.DocDate)); setVal('txtdocno', h.DocNo); setVal('txtRemarksHeader', h.RemarksHeader);
            setCombo('cmbItem', h.ItemId); itemLeave(); setCombo('cmbUOM', h.ItemUomId);
            setVal('txtBom', h.BomName); setVal('txtEffectedDate', isoOf(h.EffectedDate)); setVal('txtQty', h.BomBatchQty);
            $id('ChkIsActive').checked = !!h.IsActive; $id('chkIsDefault').checked = !!h.IsDefault;
            inputs = (d.inputs && d.inputs.length) ? d.inputs : [blankDetail()];
            consumption = (d.consumption && d.consumption.length) ? d.consumption : [blankDetail()];
            overheads = (d.overheads && d.overheads.length) ? d.overheads : [blankOverhead()];
            removedInputs = []; removedConsumption = [];
            updateMode = true;
            vis = { save: false, update: true, saveAs: false, del: true }; applyVisibility();   // :1794-1796
            $id('lblRecId').textContent = 'Record #' + id;
            calculation(); renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */

    /** cmbDateTypeHistory_ValueChanged:3206 — 1, 2 and 5 move only the From date. */
    function datePreset() {
        var v = cv('cmbDateTypeHistory'), d = new Date();
        if (v === 1) setVal('txtFromdateHistory', iso(d));
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); setVal('txtFromdateHistory', iso(w)); }
        else if (v === 3) { setVal('txtFromdateHistory', iso(new Date(d.getFullYear(), d.getMonth(), 1))); setVal('txtToDateHistory', iso(d)); }
        else if (v === 4) { setVal('txtFromdateHistory', iso(new Date(d.getFullYear(), 0, 1))); setVal('txtToDateHistory', iso(d)); }
        else if (v === 5) { if (look.financialYearStart) setVal('txtFromdateHistory', isoOf(look.financialYearStart)); }
    }
    /** btnHistoryRefresh_Click:2405 — clear the three pickers and the detail grids. */
    function refreshHistory() {
        clearCombo('cmbDateTypeHistory'); clearCombo('cmbItemhistory'); clearCombo('CmbBomNoHistory');
        ['grdHistoryOverHead', 'grdHistoryConsumption', 'grdHistoryInput'].forEach(function (g) { var t = $id(g); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; });
        focusEl('cmbDateTypeHistory');
    }
    function newHistory() { refreshHistory(); setVal('txtFromNoHistory', ''); setVal('txtToDocNoHistory', ''); }
    var HIST_COLS = [
        { key: 'RecordNo', w: 50, cls: 'ctr' }, { key: 'DocNo', w: 60, cls: 'ctr' }, { key: 'DocDate', date: fmtDMY, w: 80 }, { key: 'BomName', w: 150 }, { key: 'BomType', w: 90 }, { key: 'ItemName', w: 150 },
        { key: 'Uom', w: 60 }, { key: 'BomBatchQty', w: 70, num: true }, { key: 'EffectedDate', date: fmtDMY, w: 80 }, { key: 'IsActive', w: 40, check: true }, { key: 'IsDefault', w: 50, check: true },
        { key: 'RemarksHeader', w: 150 }, { key: 'EntryUser', w: 100 }, { key: 'EntryDate', date: fmtStamp, w: 110 }, { key: 'ModifyUser', w: 100 }, { key: 'ModifyDate', date: fmtStamp, w: 110 },
        { key: 'IsApproved', w: 60, check: true }, { key: 'NoOfAttachments', w: 70, link: true }
    ];
    /** HistoryGridFill:1998. */
    function history() {
        getJson(api + '/history' + qs({ fromDate: val('txtFromdateHistory'), toDate: val('txtToDateHistory'), fromDocNo: intOf(val('txtFromNoHistory')) || '',
            toDocNo: intOf(val('txtToDocNoHistory')) || '', bomId: cv('CmbBomNoHistory') || '', itemId: cv('cmbItemhistory') || '' })).then(function (rows) {
            historyRows = rows || []; historyCurrent = -1;
            $id('lblHistoryScope').textContent = rights.viewAll ? 'All users' : 'Your records only';
            renderHistory();
            if (historyRows.length) selectHistory(0);
        }).catch(fail);
    }
    function renderHistory() {
        var t = $id('DatagridHistory');
        t.tHead.innerHTML = '<tr><th style="width:50px">Print</th><th style="width:50px">Edit</th><th style="width:60px">SaveAs</th>' + HIST_COLS.map(function (c) { return '<th style="min-width:' + c.w + 'px">' + esc(c.key) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = historyRows.length ? historyRows.map(function (r, i) {
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === historyCurrent ? 'is-current' : '') + '">'
                + '<td class="ctr"><button type="button" class="rc-cellbtn" data-act="print">Print</button></td>'
                + '<td class="ctr"><button type="button" class="rc-cellbtn" data-act="edit">Edit</button></td>'
                + '<td class="ctr"><button type="button" class="rc-cellbtn" data-act="saveas">SaveAs</button></td>'
                + HIST_COLS.map(function (c) {
                    var v = r[c.key];
                    if (c.link) return '<td class="num"><button type="button" class="rc-link" data-act="attach">' + esc(v === null || v === undefined ? '0' : v) + '</button></td>';
                    if (c.check) return '<td class="ctr"><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';
                    if (c.date) return '<td>' + esc(c.date(v)) + '</td>';
                    if (c.num) return '<td class="num">' + esc(fmt(v, 2)) + '</td>';
                    return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '<tr class="rc-empty"><td colspan="' + (HIST_COLS.length + 3) + '">No records</td></tr>';
        var sq = historyRows.reduce(function (a, r) { return a + num(r.BomBatchQty); }, 0);
        t.tFoot.innerHTML = historyRows.length ? '<tr>' + '<td></td><td></td><td></td>' + HIST_COLS.map(function (c) { return '<td>' + (c.key === 'BomBatchQty' ? fmt(sq, 2) : '') + '</td>'; }).join('') + '</tr>' : '';
        $id('lblHistoryCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
    }
    /** DatagridHistory_SelectionChanged:2210 → DatagridHistoryDetailbind:2227. */
    function selectHistory(i) {
        historyCurrent = i;
        $id('DatagridHistory').tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        var r = historyRows[i]; if (!r) return;
        getJson(api + '/' + r.Id).then(function (d) {
            renderHistDetail('grdHistoryInput', d.inputs || [], 3);
            renderHistDetail('grdHistoryConsumption', d.consumption || [], 2);
            var t = $id('grdHistoryOverHead');
            t.tHead.innerHTML = '<tr><th>OverHeadAccount</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Amount</th><th>OhRemarks</th></tr>';
            t.tBodies[0].innerHTML = (d.overheads || []).map(function (x) {
                return '<tr><td>' + esc(x.OverHeadAccount) + '</td><td class="num">' + fmt(x.PercentOfTotalInput, 2) + '</td><td class="num">' + fmt(x.Qty, 2) + '</td><td class="num">' + fmtRate(x.Rate) + '</td><td class="num">' + fmt(x.Amount, 2) + '</td><td>' + esc(x.OhRemarks) + '</td></tr>';
            }).join('');
            var o = d.overheads || [];
            t.tFoot.innerHTML = o.length ? '<tr><td></td><td>' + fmt(o.reduce(function (a, x) { return a + num(x.PercentOfTotalInput); }, 0), 2) + '</td><td>' + fmt(o.reduce(function (a, x) { return a + num(x.Qty); }, 0), 2) + '</td><td></td><td>' + fmt(o.reduce(function (a, x) { return a + num(x.Amount); }, 0), 2) + '</td><td></td></tr>' : '';
        }).catch(fail);
    }
    function renderHistDetail(tableId, rows, amountPlaces) {
        var t = $id(tableId);
        t.tHead.innerHTML = '<tr><th>Item</th><th>Uom</th><th>ItemQty</th><th>NetWeight</th><th>Rate</th><th>RateUom</th><th>Amount</th></tr>';
        t.tBodies[0].innerHTML = rows.map(function (x) {
            return '<tr><td>' + esc(x.ItemName) + '</td><td>' + esc(x.UomCode) + '</td><td class="num">' + fmt(x.ItemQty, 2) + '</td><td class="num">' + fmt(x.NetWeight, 2) + '</td><td class="num">' + fmtRate(x.Rate) + '</td><td>' + esc(x.RateUomCode) + '</td><td class="num">' + fmt(x.Amount, amountPlaces) + '</td></tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr><td></td><td></td><td>' + fmt(rows.reduce(function (a, x) { return a + num(x.ItemQty); }, 0), 2) + '</td><td>' + fmt(rows.reduce(function (a, x) { return a + num(x.NetWeight); }, 0), 2) + '</td><td>' + fmtRate(rows.reduce(function (a, x) { return a + num(x.Rate); }, 0)) + '</td><td></td><td>' + fmt(rows.reduce(function (a, x) { return a + num(x.Amount); }, 0), amountPlaces) + '</td></tr>' : '';
    }
    /** DatagridHistory_ColumnButtonClick:2128 / _DoubleClick:2111 / _LinkClicked:2195. */
    function historyAction(i, act) {
        var r = historyRows[i]; if (!r) return;
        selectHistory(i);
        var id = intOf(r.Id);
        if (act === 'print') return bomSlip(id);
        if (act === 'edit' || act === 'dbl') { if (!rights.update) { box("You don't have update rights..."); return; } return readById(id); }
        if (act === 'attach') return openAttachments(id);
        if (act === 'saveas') {
            if (!rights.save) { box("you don't have save rights..."); return; }
            return readById(id).then(function () {
                vis = { save: false, update: false, saveAs: true, del: false }; applyVisibility();
                [inputs, consumption, overheads].forEach(function (rows) { rows.forEach(function (x) { x.Id = 0; }); });   // :2155-2184
                $id('lblRecId').textContent = 'Copy of #' + id;
                renderAll();
            });
        }
    }
    function openAttachments(id) {
        $id('attachBody').innerHTML = '<tr class="rc-empty"><td>Loading...</td></tr>';
        $id('attachModal').classList.add('is-open');
        getJson(api + '/attachments?id=' + id).then(function (rows) {
            rows = rows || [];
            $id('attachBody').innerHTML = rows.length ? rows.map(function (r) { return '<tr><td>' + esc(r.Attachment) + '</td></tr>'; }).join('') : '<tr class="rc-empty"><td>No attachments</td></tr>';
        }).catch(function (e) { $id('attachBody').innerHTML = ''; fail(e); });
    }
    function closeAttachments() { $id('attachModal').classList.remove('is-open'); }

    /* ------------------------------------------------------------------ keyboard */

    function shortcutKeys() {
        box(['Ctrl+S  For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh',
            'Ctrl+N  For New', 'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Doc Date', 'Ctrl+F10  For Open Attachments', 'F1  For Combo Lookup on Current Column of any Focused Grid',
            'Ctrl+T  For Tab Transfer', 'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid',
            'Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowUp  For Focus On on Doc Date', 'Ctrl+ArrowRight  to change focus from one grid to another',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }
    function focusNext(from) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button, [tabindex="0"]'), function (el) {
            return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null && !(el.tagName === 'SELECT' && el.classList.contains('dtcombo') && getComputedStyle(el).display === 'none');
        });
        var i = all.indexOf(from);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    function focusedGrid(t) {
        if (!t || !t.closest) return null;
        if (t.closest('#grdInputMaterial')) return 'input';
        if (t.closest('#grdConsumption')) return 'consumption';
        if (t.closest('#grdOverHead')) return 'oh';
        return null;
    }
    /** frmBillOfMaterial_Manufacturing_KeyDown:2423. */
    function onKey(e) {
        var t = e.target, k = e.key, ctrl = e.ctrlKey || e.metaKey;
        if (k === 'Escape') { if (e.defaultPrevented) return; if ($id('attachModal').classList.contains('is-open')) { closeAttachments(); return; } window.location.href = '/production'; return; }
        if (k === 'Enter' && !ctrl) {
            if (e.defaultPrevented || !t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.tagName === 'TR') return;
            e.preventDefault(); focusNext(t); return;
        }
        if (ctrl && e.altKey) { shortcutKeys(); return; }
        if (!ctrl) return;
        var key = k.length === 1 ? k.toLowerCase() : k, onHistory = currentTab() === 1;
        if (key === 't') { e.preventDefault(); tab(onHistory ? 'tabForm' : 'tabHistory'); if (!onHistory) focusEl('datDocDate'); return; }
        if (key === 'e') { e.preventDefault(); window.location.href = '/production'; return; }
        if (!onHistory) {
            var g = focusedGrid(t), tr = t && t.closest ? t.closest('tr[data-i]') : null, i = tr ? +tr.getAttribute('data-i') : -1;
            if (key === 'Delete' && e.shiftKey) { e.preventDefault(); if (rights.delete && vis.del) deleteClick(); return; }
            if (key === 'Delete' && g && i >= 0) { e.preventDefault(); if (g === 'oh') overheadButton(i, 'del'); else detailButton(g, i, 'del'); return; }
            if (key === 'd' && g) { e.preventDefault(); if (g === 'oh') overheadButton(i, 'add'); else detailButton(g, i, 'add'); return; }
            if (key === 's') { e.preventDefault(); if (vis.save && rights.save) save(); return; }
            if (key === 'F12') { e.preventDefault(); if (vis.saveAs && rights.save) saveAs(); return; }
            if (key === 'u') { e.preventDefault(); if (vis.update && rights.update && updateMode) update(); return; }
            if (key === 'p') { e.preventDefault(); if (rights.print) print(); return; }
            if (key === 'n') { e.preventDefault(); reset(); return; }
            if (key === 'r') { e.preventDefault(); refresh(); return; }
            if (key === 'F5') { e.preventDefault(); focusEl('datDocDate'); return; }
            if (key === 'ArrowDown') { e.preventDefault(); var fr = $id('grdInputMaterial').querySelector('tr[data-i]'); if (fr) fr.focus(); return; }
            if (key === 'ArrowUp') { e.preventDefault(); focusEl('datDocDate'); return; }
            if (key === 'ArrowRight') {
                e.preventDefault();
                var next = g === 'input' ? 'grdConsumption' : g === 'consumption' ? 'grdOverHead' : 'grdInputMaterial';
                var nr = $id(next).querySelector('tr[data-i]'); if (nr) nr.focus();
            }
        } else {
            if (key === 's') { e.preventDefault(); history(); return; }
            if (key === 'ArrowDown') { e.preventDefault(); var hr = $id('DatagridHistory').querySelector('tr[data-i]'); if (hr) hr.focus(); }
        }
    }

    /* ------------------------------------------------------------------ wiring */
    function wire() {
        document.addEventListener('keydown', onKey);
        /* OnlytextNumberFunction on txtQty (:1842) and the two Doc No boxes. */
        ['txtQty', 'txtFromNoHistory', 'txtToDocNoHistory'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !e.ctrlKey && !/[0-9]/.test(e.key)) e.preventDefault(); });
        });
        [['grdInputMaterial', 'input'], ['grdConsumption', 'consumption'], ['grdOverHead', 'oh']].forEach(function (g) {
            var body = $id(g[0]).tBodies[0];
            body.addEventListener('click', function (e) {
                var tr = e.target.closest('tr[data-i]'); if (!tr) return;
                var i = +tr.getAttribute('data-i');
                current[g[1]] = i;
                body.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', +x.getAttribute('data-i') === i); });
                var b = e.target.closest('button[data-act]'); if (!b) return;
                if (g[1] === 'oh') overheadButton(i, b.getAttribute('data-act')); else detailButton(g[1], i, b.getAttribute('data-act'));
            });
            body.addEventListener('change', function (e) {
                var el = e.target; if (!el.classList.contains('cell')) return;
                var i = +el.getAttribute('data-i'), k = el.getAttribute('data-k');
                if (g[1] === 'oh') overheadCellChanged(i, k, el); else detailCellChanged(g[1], i, k, el);
            });
            body.addEventListener('keypress', function (e) {
                var el = e.target; if (!el.classList.contains('cell') || el.tagName !== 'INPUT' || el.classList.contains('txt')) return;
                if (e.key.length !== 1 || e.ctrlKey) return;
                if (/[0-9]/.test(e.key) || (e.key === '.' && el.value.indexOf('.') < 0)) return;
                e.preventDefault();
            });
        });
        var hb = $id('DatagridHistory').tBodies[0];
        hb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'), b = e.target.closest('button[data-act]');
            if (b) historyAction(i, b.getAttribute('data-act')); else selectHistory(i);
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) historyAction(+tr.getAttribute('data-i'), 'dbl'); });
    }

    document.addEventListener('DOMContentLoaded', init);

    window.Recipe = {
        newClick: reset, refresh: refresh, save: save, update: update, saveAs: saveAs, deleteClick: deleteClick, print: print, generateAvgRate: generateAvgRate,
        shortcutKeys: shortcutKeys, tab: tab, itemLeave: itemLeave, datePreset: datePreset, newHistory: newHistory, refreshHistory: refreshHistory, history: history,
        closeAttachments: closeAttachments
    };
})();
