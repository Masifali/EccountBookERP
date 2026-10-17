/* ============================================================================================
 * W5B - Bill Of Material family, one page script for three desktop screens (variant = data-variant on #w5b):
 *   mfg       774 Architecture.WinApp.Mfg.frmBillOfMaterial          DocumentTypeId 1617  [Mfg] schema
 *   casting   845 Architecture.WinApp.Mfg.frmBillOfMaterialCasting   DocumentTypeId 1664  [Mfg] schema
 *   concrete  598 Architecture.WinApp.pcc.frmBillOfMaterialConcrete  DocumentTypeId 1851  [pcc] schema
 * Line numbers in comments are the .cs of the screen named (774 = frmBillOfMaterial.cs, 598 = frmBillOfMaterialConcrete.cs).
 * Modelled on countx_production_prod273_recipe_branding.js (screen 285).
 *
 *   Load 774:387 / 598:405       rights, lookups, doc no, blank rows            FromReset 774:2640    btnRefresh 774:1900
 *   Insert() 774:2002 / 598:2150 FormValidation -> confirm -> save              ReadById 774:2467 / 598:2518
 *   Calculation 774:1964 / 598:2123   grid CellUpdated 774:1083.. / 598:1083..  History 774:2813 / 598:2919
 *
 * The grids are edited in place (GridEX AllowEdit): "+" appends a blank row, "X" removes one; Item / Account cells are
 * type-ahead pickers, UOM cells list the item's UOM schedule (the desktop's F1 GrdPopUp).
 * ============================================================================================ */
(function () {
    'use strict';

    var root = document.getElementById('w5b');
    var variant = root.getAttribute('data-variant');
    var api = root.getAttribute('data-route') + '/api';
    var RPT = root.getAttribute('data-rpt');
    var SCREEN = root.getAttribute('data-screen');

    var look = {}, cfg = {}, rights = {}, rateDecimals = 2, amountDecimals = 2;
    var recId = 0, updateMode = false;
    var vis = { save: true, update: false, del: false };
    var inputs = [], byproducts = [], overheads = [], wages = [], operations = [];
    var removedInputs = [], removedByproducts = [], removedOperations = [];
    var current = { input: -1, output: -1, oh: -1, wages: -1, ops: -1 };
    var historyRows = [], historyCurrent = -1;
    var pendingRender = null;

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
    function fmtPlain(v) { var n = num(v); return n === 0 ? '0' : String(n); }
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

    /** DropDownBind.BindDDL(..., ZeroIndex) - "...Select Any Value..." when zeroIndex; selection kept when still present. */
    function fill(selectId, rows, zeroIndex, keepValue) {
        var el = $id(selectId); if (!el) return;
        var keep = keepValue === undefined ? el.value : String(keepValue);
        var h = zeroIndex ? '<option value="0">...Select Any Value...</option>' : '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.value = keep;
        if (el.value !== keep) el.selectedIndex = 0;
    }
    /** DDL.BindDDLNew(..., false) - no blank row, the first row stays active. */
    function fillNoBlank(selectId, rows, keepText) {
        var el = $id(selectId); if (!el) return;
        var h = '';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        if (keepText) setTypeByText(selectId, keepText); else if (el.options.length) el.selectedIndex = 0;
    }
    function fillIf(id, rows, zeroIndex) { if (rows && rows.length) fill(id, rows, zeroIndex); }
    function setCombo(id, v) { var el = $id(id); if (!el) return; el.value = String(v === null || v === undefined ? '' : v); if (el.value !== String(v) && el.options.length) el.selectedIndex = 0; }
    function clearCombo(id) { var el = $id(id); if (el && el.options.length) el.selectedIndex = 0; }
    function setTypeByText(id, text) {
        var el = $id(id); if (!el) return;
        for (var i = 0; i < el.options.length; i++) if (el.options[i].text === text) { el.selectedIndex = i; return; }
    }
    function uomsOf(itemId) { return (look.allUoms || []).filter(function (u) { return intOf(u.ItemId) === itemId; }); }
    function byName(list, name) {
        var n = String(name === null || name === undefined ? '' : name).trim().toLowerCase();
        if (!n) return null;
        for (var i = 0; i < (list || []).length; i++) if (String(list[i].Name).toLowerCase() === n) return list[i];
        return null;
    }
    function byId(list, id) { for (var i = 0; i < (list || []).length; i++) if (intOf(list[i].Id) === intOf(id)) return list[i]; return null; }
    function setOptions(dlId, list) {
        var dl = $id(dlId); if (!dl) return;
        dl.innerHTML = (list || []).map(function (r) { return '<option value="' + esc(r.Name) + '"></option>'; }).join('');
    }
    /** Item lists: 774/845 use one list; 598 uses ItemGetAllServiceBind for inputs and the 45/46/47 (or production type) list for the header and outputs. */
    function inItems() { return look.items || []; }
    function outItems() { return cfg.concrete ? (look.headerItems || []) : (look.items || []); }

    /* ------------------------------------------------------------------ load */

    function applyRights() {
        rights = look.rights || {};
        $id('btnsave').disabled = !rights.save; $id('btnUpdate').disabled = !rights.update;        // Load 774:397-401
        $id('btnPrint').disabled = !rights.print; $id('btnDelete').disabled = !rights.delete;
        var missing = [];
        if (!rights.save) missing.push('Save'); if (!rights.update) missing.push('Update'); if (!rights.print) missing.push('Print'); if (!rights.delete) missing.push('Delete');
        var note = $id('rightsNote');
        note.textContent = missing.length ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.' : '';
        note.classList.toggle('is-hidden', !missing.length);
    }
    function applyVisibility() { show('btnsave', vis.save); show('btnUpdate', vis.update); show('btnDelete', vis.del); }

    /** Controls that exist only on some variants carry data-v="mfg casting ..."; the others are removed. */
    function pruneVariant() {
        Array.prototype.slice.call(root.querySelectorAll('[data-v]')).forEach(function (el) {
            if ((' ' + el.getAttribute('data-v') + ' ').indexOf(' ' + variant + ' ') < 0 && el.parentNode) el.parentNode.removeChild(el);
        });
        Array.prototype.slice.call(root.querySelectorAll('[data-vt]')).forEach(function (el) {     // variant-specific text: data-vt-<variant>
            var t = el.getAttribute('data-vt-' + variant); if (t) el.textContent = t;
        });
    }

    function init() {
        pruneVariant();
        ['datDocDate', 'txtEffectedDate', 'txtFromdateHistory', 'txtToDateHistory'].forEach(function (id) { setVal(id, today()); });
        wire();
        getJson(api + '/lookups').then(function (d) {
            look = d || {};
            cfg = look.config || {};
            applyRights();
            rateDecimals = look.rateDecimals === undefined ? 2 : intOf(look.rateDecimals);
            amountDecimals = look.amountDecimals === undefined ? 2 : intOf(look.amountDecimals);
            bindLists();
            fill('cmbDateTypeHistory', look.dateTypes, true);                               // Datetypefill
            var dt = $id('cmbDateTypeHistory'); if (dt.options.length > 2) { dt.selectedIndex = 2; datePreset(); }   // Rows[2].Activate
            bindHistoryCombos(look);
            setVal('txtdocno', look.docNo);                                                  // DocumentNoFill
            blankGrids();
            applyVisibility();
            renderAll();
            focusEl('txtBom');
        }).catch(fail);
    }

    /** ItemFill / uomgetAll / AccountTitleFill / TypeBind / FillUOM / BindCastingType / ProductionTypeFill / plants. */
    function bindLists() {
        setOptions('dlInItems', inItems()); setOptions('dlOutItems', outItems()); setOptions('dlAccounts', look.accounts);
        fillNoBlank('cmbBOMType', look.bomTypes, selText('cmbBOMType'));
        fillIf('cmbItem', look.headerItems, true);
        if (cfg.hasCastingType) { fill('CmbCastingType', look.castingTypes, true); fill('cmbUOM', look.uoms, true); }
        if (cfg.concrete) {
            fill('CmbProductionType', look.productionTypes, true);
            var pt = $id('CmbProductionType'); if (pt.options.length > 1 && !cv('CmbProductionType')) pt.selectedIndex = 1;   // Rows[1].Activate (598:774)
            fill('CmbPlantName', look.plants, true);
        }
        if (cfg.hasOperations) fillOperationCombo();
        if (cfg.hasRouting) fill('CmbRoutingName', look.routingNames, true);
    }
    function bindHistoryCombos(d) {
        fillIf('CmbBomNoHistory', d.bomNames, true); fillIf('cmbbomtypeHistory', d.historyTypes, true);
        if (cfg.hasItem) fillIf('cmbItemhistory', d.historyItems, true);
    }
    function blankDetail() { return { Id: 0, ItemId: 0, ItemName: '', ItemUomId: 0, Uom: 0, UomCode: '', ItemQty: 0, NetWeight: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0, MinQty: 0, MaxQty: 0, MainItemId: 0, MainItemName: '', MainItemUomId: 0, MainUomCode: '' }; }
    function blankOverhead() { return { Id: 0, OverHeadTypeId: 0, OverHeadAccountId: 0, OverHeadAccount: '', PercentOfTotalInput: 0, Qty: 0, Rate: 0, Amount: 0, OhRemarks: '' }; }
    function blankWage() { return { Id: 0, ContractorWagesRateScheduleId: 0, ServiceActivity: '', Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function blankOp() { return { Id: 0, WorkStationOperationId: 0, OperationName: '', WorkStationName: '', CapacityQty: 0, Hours: 0, Minutes: 0, SeqNo: 0, HourlyCost: 0, OperationCost: 0, OperationRemarks: '' }; }
    function blankGrids() {
        inputs = [blankDetail()]; byproducts = [blankDetail()]; overheads = [blankOverhead()];
        wages = [blankWage()]; operations = [blankOp()];
        removedInputs = []; removedByproducts = []; removedOperations = [];
        current = { input: -1, output: -1, oh: -1, wages: -1, ops: -1 };
    }
    function fillOperationCombo() { /* the operation cell lists look.operationCombo directly */ }

    /** btnRefresh_Click (774:1900). */
    function refresh() {
        getJson(api + '/refresh').then(function (d) {
            Object.keys(d || {}).forEach(function (k) { look[k] = d[k]; });
            bindLists();
            renderAll();
        }).catch(fail);
    }

    /** FromReset (774:2640). */
    function reset() {
        recId = 0;
        setVal('txtBom', ''); setVal('txtEffectedDate', today()); setVal('datDocDate', today()); clearCombo('cmbItem');
        if (cfg.hasCastingType) { clearCombo('CmbCastingType'); clearCombo('cmbUOM'); } else $id('cmbUOM').innerHTML = '<option value=""></option>';
        if (cfg.concrete) { var pt = $id('CmbProductionType'); if (pt.options.length > 1) pt.selectedIndex = 1; clearCombo('CmbPlantName'); setVal('txtTopForce', ''); setVal('txtBottomForce', ''); look.headerItems = look.headerItemsAll || look.headerItems; fillIf('cmbItem', look.headerItems, true); setOptions('dlOutItems', outItems()); }
        var bt = $id('cmbBOMType'); if (bt.options.length) bt.selectedIndex = 0;
        setVal('txtQty', ''); setVal('txtRemarksHeader', '');
        if ($id('ChkIsActive')) $id('ChkIsActive').checked = true;
        $id('chkIsDefault').checked = true;
        if (cfg.hasRouting) { clearCombo('CmbRoutingName'); show('rowRouting', true); }
        setCaption();
        blankGrids();
        vis = { save: true, update: false, del: false }; updateMode = false; applyVisibility();
        lockItem();
        $id('lblRecId').textContent = '';
        ['txtTotalInputs', 'txtOverHead', 'txtWagesAmount', 'txtLessByProduct', 'txtNetValue', 'txtValuePerQty'].forEach(function (id) { setVal(id, ''); });
        renderAll();
        focusEl('datDocDate');
        return getJson(api + '/doc-no').then(function (d) { setVal('txtdocno', d.docNo); }).catch(fail);
    }

    /** cmbItem_Leave (774:cmbItem leave, 598:665). 774 / 845 only set the wages caption; 598 also lists the item's UOM schedule (UOMDescription). */
    function itemLeave() {
        if (!cfg.hasCastingType) {
            var prev = cv('cmbUOM');
            var list = uomsOf(cv('cmbItem')).map(function (u) { return { Id: u.Id, Name: u.UOMDescription }; });
            if (list.length) fill('cmbUOM', list, false, prev > 0 ? prev : '');
        }
        setCaption();
    }
    function setCaption() { var e = $id('labItemCaption'); if (e) e.textContent = selText('cmbItem'); }

    /** CmbProductionType_Leave (598:846). */
    function productionTypeLeave() {
        var t = cv('CmbProductionType');
        if (!look.headerItemsAll) look.headerItemsAll = look.headerItems;
        var keep = cv('cmbItem');
        var done = function (items) {
            look.headerItems = items || [];
            fill('cmbItem', look.headerItems, true, byId(look.headerItems, keep) ? keep : '');
            setOptions('dlOutItems', outItems());
            renderAll();
        };
        if (t > 0) return getJson(api + '/items-by-type' + qs({ typeId: t })).then(done).catch(fail);
        done(look.headerItemsAll);
        return Promise.resolve();
    }

    /** EnableDisableItemName (598:1849): with a wages row the item and production type are fixed. */
    function lockItem() {
        if (!cfg.hasWages) return;
        var locked = wages.length > 0 && intOf(wages[0].ContractorWagesRateScheduleId) > 0;
        ['cmbItem', 'CmbProductionType'].forEach(function (id) { var e = $id(id); if (e) e.disabled = locked; });
    }

    /* ------------------------------------------------------------------ tabs */
    function currentTab() { return $id('tabHistory').classList.contains('is-active') ? 1 : 0; }
    function tab(id) {
        ['tabForm', 'tabHistory'].forEach(function (t) { $id(t).classList.toggle('is-active', t === id); });
        $id('tabs1').querySelectorAll('.rc-tab1').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === id); });
        if (id === 'tabHistory') {                                                             // tabControl1_SelectedIndexChanged
            var dt = $id('cmbDateTypeHistory'); if (dt.options.length > 2) { dt.selectedIndex = 2; datePreset(); }
            getJson(api + '/history-combos').then(bindHistoryCombos).catch(fail);
        }
    }

    /* ------------------------------------------------------------------ grids */

    function rowsOf(kind) { return kind === 'input' ? inputs : byproducts; }
    function removedOf(kind) { return kind === 'input' ? removedInputs : removedByproducts; }

    function uomOptions(itemId, selectedId, selectedCode) {
        var h = '<option value=""></option>', found = false;
        uomsOf(itemId).forEach(function (u) {
            var sel = intOf(u.Id) === selectedId;
            if (sel) found = true;
            h += '<option value="' + esc(u.Id) + '" data-eq="' + num(u.Equivalent) + '" data-code="' + esc(u.UOMCode) + '"' + (sel ? ' selected' : '') + '>' + esc(u.UOMCode) + '</option>';
        });
        if (!found && selectedId) h += '<option value="' + esc(selectedId) + '" data-eq="0" data-code="' + esc(selectedCode) + '" selected>' + esc(selectedCode) + '</option>';
        return h;
    }
    function mainUomOptions(r) {
        var h = '<option value=""></option>', seen = {};
        inputs.forEach(function (x) {
            if (intOf(x.ItemId) !== intOf(r.MainItemId) || !intOf(r.MainItemId) || seen[x.ItemUomId]) return;
            seen[x.ItemUomId] = true;
            h += '<option value="' + esc(x.ItemUomId) + '" data-code="' + esc(x.UomCode) + '"' + (intOf(x.ItemUomId) === intOf(r.MainItemUomId) ? ' selected' : '') + '>' + esc(x.UomCode) + '</option>';
        });
        if (intOf(r.MainItemUomId) && !seen[r.MainItemUomId]) h += '<option value="' + esc(r.MainItemUomId) + '" data-code="' + esc(r.MainUomCode) + '" selected>' + esc(r.MainUomCode) + '</option>';
        return h;
    }

    /** grdInputSettings (774:672) / grdOutPutGridSetting - X, +, Item Name, Uom, ItemQty, Rate, Amount (598 input: + MinQty, MaxQty, Main Item, Main Uom). */
    function renderDetail(tableId, kind) {
        var rows = rowsOf(kind), extras = cfg.concrete && kind === 'input', g = kind;
        var t = $id(tableId);
        t.tHead.innerHTML = '<tr><th class="col-x">X</th><th class="col-x">+</th><th style="min-width:220px">Item Name</th><th>Uom</th><th>ItemQty</th><th>Rate</th><th>Amount</th>'
            + (extras ? '<th>MinQty</th><th>MaxQty</th><th style="min-width:160px">Main Item</th><th>Main Uom</th>' : '') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            var a = ' data-g="' + g + '" data-i="' + i + '"';
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === current[kind] ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="rc-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="rc-add" tabindex="-1" data-act="add">+</button></td>'
                + '<td><input type="text" class="cell txt" list="' + (kind === 'input' ? 'dlInItems' : 'dlOutItems') + '" data-k="ItemName"' + a + ' value="' + esc(r.ItemName) + '"></td>'
                + '<td><select class="cell" data-k="UomCode"' + a + '>' + uomOptions(intOf(r.ItemId), intOf(r.ItemUomId), r.UomCode) + '</select></td>'
                + '<td class="num"><input type="text" class="cell" data-k="ItemQty"' + a + ' value="' + esc(fmtPlain(r.ItemQty)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Rate"' + a + ' value="' + esc(fmtPlain(r.Rate)) + '"></td>'
                + '<td class="num">' + esc(fmt(r.Amount, kind === 'input' ? 3 : 2)) + '</td>'
                + (extras ? '<td class="num"><input type="text" class="cell" data-k="MinQty"' + a + ' value="' + esc(fmtPlain(r.MinQty)) + '"></td>'
                    + '<td class="num"><input type="text" class="cell" data-k="MaxQty"' + a + ' value="' + esc(fmtPlain(r.MaxQty)) + '"></td>'
                    + '<td><input type="text" class="cell txt" list="dlMainItems" data-k="MainItemName"' + a + ' value="' + esc(r.MainItemName) + '"></td>'
                    + '<td><select class="cell" data-k="MainUomCode"' + a + '>' + mainUomOptions(r) + '</select></td>' : '')
                + '</tr>';
        }).join('');
        var sq = 0, sa = 0;
        rows.forEach(function (r) { sq += num(r.ItemQty); sa += num(r.Amount); });
        t.tFoot.innerHTML = rows.length ? '<tr><td></td><td></td><td></td><td></td><td>' + fmt(sq, 3) + '</td><td></td><td>' + fmt(sa, 3) + '</td>' + (extras ? '<td></td><td></td><td></td><td></td>' : '') + '</tr>' : '';
        $id(kind === 'input' ? 'lblInputCount' : 'lblOutputCount').textContent = rows.length;
    }

    /** grdOverHeadGridSetting - X, +, AccountTitle, Percentage, Qty, Rate, Amount, OhRemarks. */
    function renderOverheads() {
        var t = $id('grdOverHead');
        t.tHead.innerHTML = '<tr><th class="col-x">X</th><th class="col-x">+</th><th style="min-width:220px">AccountTitle</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Amount</th><th>OhRemarks</th></tr>';
        t.tBodies[0].innerHTML = overheads.map(function (r, i) {
            var a = ' data-g="oh" data-i="' + i + '"';
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === current.oh ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="rc-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="rc-add" tabindex="-1" data-act="add">+</button></td>'
                + '<td><input type="text" class="cell txt" list="dlAccounts" data-k="OverHeadAccount"' + a + ' value="' + esc(r.OverHeadAccount) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="PercentOfTotalInput"' + a + ' value="' + esc(fmtPlain(r.PercentOfTotalInput)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Qty"' + a + ' value="' + esc(fmtPlain(r.Qty)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Rate"' + a + ' value="' + esc(fmtPlain(r.Rate)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Amount"' + a + ' value="' + esc(fmtPlain(r.Amount)) + '"></td>'
                + '<td><input type="text" class="cell txt" data-k="OhRemarks"' + a + ' value="' + esc(r.OhRemarks) + '"></td></tr>';
        }).join('');
        var sp = 0, sq = 0, sa = 0;
        overheads.forEach(function (r) { sp += num(r.PercentOfTotalInput); sq += num(r.Qty); sa += num(r.Amount); });
        t.tFoot.innerHTML = overheads.length ? '<tr><td></td><td></td><td></td><td>' + fmt(sp, 2) + '</td><td>' + fmt(sq, 2) + '</td><td></td><td>' + fmt(sa, 2) + '</td><td></td></tr>' : '';
        $id('lblOverheadCount').textContent = overheads.length;
    }

    /** grdContractorWagesGridSetting (598:1755) - X, +, ServiceActivity (F1 picker), Qty, Rate, Amount, Remarks. */
    function renderWages() {
        var t = $id('grdContractorWages'); if (!t) return;
        t.tHead.innerHTML = '<tr><th class="col-x">X</th><th class="col-x">+</th><th style="min-width:220px">ServiceActivity</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = wages.map(function (r, i) {
            var a = ' data-g="wages" data-i="' + i + '"';
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === current.wages ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="rc-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="rc-add" tabindex="-1" data-act="add">+</button></td>'
                + '<td>' + esc(r.ServiceActivity) + ' <button type="button" class="rc-cellbtn" tabindex="-1" data-act="pick" title="F1">...</button></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Qty"' + a + ' value="' + esc(fmtPlain(r.Qty)) + '"></td>'
                + '<td class="num">' + esc(fmtRate(r.Rate)) + '</td>'
                + '<td class="num">' + esc(fmt(r.Amount, 2)) + '</td>'
                + '<td><input type="text" class="cell txt" data-k="Remarks"' + a + ' value="' + esc(r.Remarks) + '"></td></tr>';
        }).join('');
        var sq = 0, sa = 0;
        wages.forEach(function (r) { sq += num(r.Qty); sa += num(r.Amount); });
        t.tFoot.innerHTML = wages.length ? '<tr><td></td><td></td><td></td><td>' + fmt(sq, 4) + '</td><td></td><td>' + fmt(sa, 2) + '</td><td></td></tr>' : '';
        $id('lblWagesCount').textContent = wages.length;
    }

    /** grdProcessSetting (774:1684) - X, +, WorkStation/Operation, OperationName, WorkStationName, Hours, Minutes, SeqNo, HourlyCost, OperationCost, OperationRemarks. */
    function renderOperations() {
        var t = $id('grdOperatinalDetail'); if (!t) return;
        t.tHead.innerHTML = '<tr><th class="col-x">X</th><th class="col-x">+</th><th style="min-width:220px">WorkStation/Operation</th><th>OperationName</th><th>WorkStationName</th><th>Hours</th><th>Minutes</th><th>SeqNo</th><th>HourlyCost</th><th>OperationCost</th><th>OperationRemarks</th></tr>';
        t.tBodies[0].innerHTML = operations.map(function (r, i) {
            var a = ' data-g="ops" data-i="' + i + '"';
            var opts = '<option value="0"></option>' + (look.operationCombo || []).map(function (o) {
                return '<option value="' + esc(o.Id) + '"' + (intOf(o.Id) === intOf(r.WorkStationOperationId) ? ' selected' : '') + '>' + esc(o.Name) + '</option>';
            }).join('');
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === current.ops ? 'is-current' : '') + '">'
                + '<td class="col-x"><button type="button" class="rc-x" tabindex="-1" data-act="del">X</button></td>'
                + '<td class="col-x"><button type="button" class="rc-add" tabindex="-1" data-act="add">+</button></td>'
                + '<td><select class="cell" data-k="WorkStationOperationId"' + a + '>' + opts + '</select></td>'
                + '<td>' + esc(r.OperationName) + '</td><td>' + esc(r.WorkStationName) + '</td>'
                + '<td class="num"><input type="text" class="cell" data-k="Hours"' + a + ' value="' + esc(fmtPlain(r.Hours)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="Minutes"' + a + ' value="' + esc(fmtPlain(r.Minutes)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="SeqNo"' + a + ' value="' + esc(fmtPlain(r.SeqNo)) + '"></td>'
                + '<td class="num"><input type="text" class="cell" data-k="HourlyCost"' + a + ' value="' + esc(fmtPlain(r.HourlyCost)) + '"></td>'
                + '<td class="num">' + esc(fmt(r.OperationCost, 2)) + '</td>'
                + '<td><input type="text" class="cell txt" data-k="OperationRemarks"' + a + ' value="' + esc(r.OperationRemarks) + '"></td></tr>';
        }).join('');
        var sc = 0;
        operations.forEach(function (r) { sc += num(r.OperationCost); });
        t.tFoot.innerHTML = operations.length ? '<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td>' + fmt(sc, 2) + '</td><td></td></tr>' : '';
        $id('lblOperationCount').textContent = operations.length;
    }

    function renderAll() {
        renderDetail('grdInputMaterial', 'input');
        renderDetail('grdOutPutByProduct', 'output');
        renderOverheads();
        if (cfg.hasWages) renderWages();
        if (cfg.hasOperations) renderOperations();
    }
    /** Re-render after a cell edit without losing the focused cell (the browser has already moved focus by the time the timer fires). */
    function schedule() {
        if (pendingRender) return;
        pendingRender = setTimeout(function () {
            pendingRender = null;
            var a = document.activeElement, key = null;
            if (a && a.getAttribute && a.getAttribute('data-k') && a.getAttribute('data-g')) key = { g: a.getAttribute('data-g'), i: a.getAttribute('data-i'), k: a.getAttribute('data-k') };
            renderAll();
            if (key) {
                var el = root.querySelector('[data-g="' + key.g + '"][data-i="' + key.i + '"][data-k="' + key.k + '"]');
                if (el) { try { el.focus(); if (el.tagName === 'INPUT') el.select(); } catch (x) { /* ignore */ } }
            }
        }, 0);
    }

    /** CellUpdated 774:1083.. / 598:1083 - Amount = ItemQty x Uom / RateUom x Rate when Rate and RateUomId are set, else 0. */
    function recalc(r) {
        r.NetWeight = num(r.ItemQty) * num(r.Uom);
        if (num(r.Rate) !== 0 && intOf(r.RateUomId) !== 0) {
            if (num(r.RateUom) === 0) { r.Amount = 0; box('Attempted to divide by zero.'); return; }
            r.Amount = num(r.ItemQty) * num(r.Uom) / num(r.RateUom) * num(r.Rate);
        } else r.Amount = 0;
    }
    function clearUom(r) {
        r.ItemUomId = 0; r.Uom = 0; r.UomCode = ''; r.RateUom = 0; r.RateUomId = 0; r.RateUomCode = ''; r.NetWeight = 0; r.Amount = 0;
    }

    /** grdInputMaterial_CellUpdated / grdOutPutByProduct_CellUpdated + the F1 pickers (KeyDown). */
    function detailCellChanged(kind, i, key, el) {
        var rows = rowsOf(kind), r = rows[i]; if (!r) return;
        if (key === 'ItemName') {                                                            // F1 on ItemName
            var f = byName(kind === 'input' ? inItems() : outItems(), el.value), prev = intOf(r.ItemId);
            r.ItemId = f ? intOf(f.Id) : 0; r.ItemName = f ? f.Name : '';
            if (r.ItemId !== prev) clearUom(r);
        } else if (key === 'UomCode') {                                                      // F1 on UomCode: RateUom follows
            var o = el.options[el.selectedIndex];
            if (o && o.value !== '') {
                r.ItemUomId = intOf(o.value); r.Uom = num(o.getAttribute('data-eq')); r.UomCode = o.getAttribute('data-code') || '';
                r.RateUomId = r.ItemUomId; r.RateUom = r.Uom; r.RateUomCode = r.UomCode;
            } else clearUom(r);
            recalc(r);
        } else if (key === 'ItemQty') {
            r.ItemQty = num(el.value);
            recalc(r);
        } else if (key === 'Rate') {
            r.Rate = num(el.value);
            if (el.value.trim() === '') { r.Rate = 0; box('ItemQty And ItemUom Should be greater than zero...'); schedule(); return; }
            recalc(r);
        } else if (key === 'MinQty') {                                                       // 598:1160
            r.MinQty = num(el.value);
            if (intOf(r.ItemQty) === 0) { r.MinQty = 0; box('ItemQty Required...Please Add that First'); }
            else if (intOf(r.MinQty) > intOf(r.ItemQty)) { r.MinQty = 0; box('MinQty Should be less than or equal to ItemQty...'); }
        } else if (key === 'MaxQty') {                                                       // 598:1172
            r.MaxQty = num(el.value);
            if (intOf(r.ItemQty) === 0) { r.MaxQty = 0; box('ItemQty Required...Please Add that First'); }
            else if (intOf(r.MaxQty) < intOf(r.ItemQty)) { r.MaxQty = 0; box('MaxQty Should be greater than or equal to ItemQty...'); }
        } else if (key === 'MainItemName') {                                                 // 598 F1 MainItemId / MainItemName
            var others = [];
            inputs.forEach(function (x, ix) { if (ix !== i && intOf(x.ItemId) > 0) others.push({ Id: x.ItemId, Name: x.ItemName }); });
            var m = byName(others, el.value);
            if (m && intOf(m.Id) === intOf(r.ItemId)) { m = null; box("MainItem Can't be Same As Item"); }
            r.MainItemId = m ? intOf(m.Id) : 0; r.MainItemName = m ? m.Name : '';
            r.MainItemUomId = 0; r.MainUomCode = '';
            if (m) {
                for (var k = 0; k < inputs.length; k++) {                                    // GetInputMainItemsUom -> first row of that item
                    if (intOf(inputs[k].ItemId) === r.MainItemId) { r.MainItemUomId = intOf(inputs[k].ItemUomId); r.MainUomCode = inputs[k].UomCode; break; }
                }
            }
        } else if (key === 'MainUomCode') {
            if (intOf(r.MainItemId) === 0) { box('Select Main Item First...'); }
            else { var o2 = el.options[el.selectedIndex]; if (o2 && o2.value !== '') { r.MainItemUomId = intOf(o2.value); r.MainUomCode = o2.getAttribute('data-code') || ''; } }
        }
        calculation();
        schedule();
    }

    /** grdOverHead_CellUpdated. */
    function overheadCellChanged(i, key, el) {
        var r = overheads[i]; if (!r) return;
        if (key === 'OverHeadAccount') {
            var f = byName(look.accounts, el.value);
            r.OverHeadAccountId = f ? intOf(f.Id) : 0; r.OverHeadAccount = f ? f.Name : ''; r.OverHeadTypeId = r.OverHeadAccountId;
        } else if (key === 'Qty' || key === 'Rate') {
            r[key] = num(el.value);
            if (el.value.trim() === '') { r[key] = 0; box('Qty And Rate Should be greater than zero...'); schedule(); return; }
            r.Amount = num(r.Qty) * num(r.Rate);
        } else if (key === 'PercentOfTotalInput') {
            r.PercentOfTotalInput = num(el.value);
            var total = overheads.reduce(function (a, x) { return a + num(x.PercentOfTotalInput); }, 0);
            if (total > 100) { r.PercentOfTotalInput = num(r.PercentOfTotalInput) - (total - 100); box('TotalPercentage Can not be Greater than 100...'); schedule(); return; }
            if (el.value.trim() !== '') {
                var pct = totalInputs() / 100 * num(r.PercentOfTotalInput);
                r.Amount = pct > 0 ? roundAway(pct, amountDecimals) : 0;
                r.Qty = 0; r.Rate = 0;
            }
        } else if (key === 'Amount') { r.Amount = num(el.value); r.Qty = 0; r.Rate = 0; }
        else if (key === 'OhRemarks') { r.OhRemarks = el.value; }
        calculation();
        schedule();
    }

    /** grdContractorWages_CellUpdated (598:1787). */
    function wagesCellChanged(i, key, el) {
        var r = wages[i]; if (!r) return;
        if (key === 'Qty') {
            if (el.value.trim() === '') { r.Qty = 0; box('Qty And Rate Should be greater than zero...'); schedule(); return; }
            r.Qty = num(el.value);
            r.Amount = num(r.Qty) * num(r.Rate);
        } else if (key === 'Remarks') r.Remarks = el.value;
        calculation();
        schedule();
    }

    /** grdProcess_CellUpdated (774:1832). */
    function opCellChanged(i, key, el) {
        var r = operations[i]; if (!r) return;
        if (key === 'WorkStationOperationId') {
            var id = intOf(el.value);
            if (id > 0) {
                for (var k = 0; k < operations.length; k++) {
                    if (k !== i && intOf(operations[k].WorkStationOperationId) === id) { r.WorkStationOperationId = 0; r.OperationName = ''; r.WorkStationName = ''; box('Operation Already Exist in Grid!'); schedule(); return; }
                }
                var f = byId(look.operationCombo, id);
                r.WorkStationOperationId = id; r.OperationName = f ? f.OperationName : ''; r.WorkStationName = f ? f.WorkStationName : '';
            } else { r.WorkStationOperationId = 0; r.OperationName = ''; r.WorkStationName = ''; }
        } else if (key === 'Hours' || key === 'Minutes' || key === 'HourlyCost') {
            var blankCell = String(el.value).trim() === '';
            r[key] = num(el.value);
            if (blankCell && (key === 'HourlyCost' || (key === 'Hours' ? String(r.Minutes) === '' : String(r.Hours) === ''))) {
                r.OperationCost = 0; box('Hours And HourlyCost Should be greater than zero...'); schedule(); return;
            }
            r.OperationCost = (num(r.Hours) + num(r.Minutes) / 60) * num(r.HourlyCost);
        } else if (key === 'SeqNo') r.SeqNo = intOf(el.value);
        else if (key === 'OperationRemarks') r.OperationRemarks = el.value;
        calculation();
        schedule();
    }

    /** ColumnButtonClick (Delete / Add) of the five grids. */
    function gridButton(g, i, act) {
        if (act === 'pick') { pickService(i); return; }
        var rows = g === 'input' ? inputs : g === 'output' ? byproducts : g === 'oh' ? overheads : g === 'wages' ? wages : operations;
        var blank = g === 'oh' ? blankOverhead : g === 'wages' ? blankWage : g === 'ops' ? blankOp : blankDetail;
        if (act === 'del') {
            var r = rows[i]; if (!r) return;
            if ((g === 'input' || g === 'output') && intOf(r.Id) > 0) {
                if (!ask('Are you sure to Delete?')) return;
                removedOf(g).push({ Id: r.Id, ItemId: r.ItemId, ItemUomId: r.ItemUomId, ItemQty: r.ItemQty, NetWeight: r.NetWeight, Rate: r.Rate, RateUomId: r.RateUomId, Amount: r.Amount, ActionTypeId: 3 });
            }
            if (g === 'ops' && intOf(r.Id) !== 0) {
                if (!ask('Are you sure to Delete?')) return;
                removedOperations.push({ Id: r.Id, WorkStationOperationId: r.WorkStationOperationId, Hours: r.Hours, Minutes: r.Minutes, HourlyCost: r.HourlyCost, OperationCost: r.OperationCost, SeqNo: r.SeqNo, OperationRemarks: r.OperationRemarks });
            }
            rows.splice(i, 1);
            if (!rows.length) rows.push(blank());
        }
        if (act === 'add') rows.push(blank());
        current[g] = -1;
        lockItem(); calculation(); renderAll();
    }

    /** F1 on ServiceActivity (598:4157, 774) - GetServiceActivityByItemId -> picker. */
    function pickService(i) {
        var r = wages[i]; if (!r) return;
        getJson(api + '/wages-activities' + qs({ itemId: cv('cmbItem'), docDate: val('datDocDate') })).then(function (list) {
            openPicker('Service Activity', list || [], [['WagesActivity', 'Wages Activity'], ['Qty', 'Qty'], ['Rate', 'Rate']], function (f) {
                r.ContractorWagesRateScheduleId = intOf(f.ContractorWagesRateScheduleId); r.ServiceActivity = f.WagesActivity;
                if (num(f.Qty) === 0) { box('Attempted to divide by zero.'); return; }
                r.Rate = num(f.Rate) / num(f.Qty);
                r.Amount = r.Rate * intOf(r.Qty);                                        // CurrRowQty = ToInt(Qty)
                lockItem(); calculation(); renderAll();
            });
        }).catch(fail);
    }
    function openPicker(title, rows, cols, onPick) {
        $id('pickTitle').textContent = title;
        $id('pickHead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        $id('pickBody').innerHTML = rows.length ? rows.map(function (r, i) {
            return '<tr data-i="' + i + '" tabindex="0" style="cursor:pointer">' + cols.map(function (c) { return '<td>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('') : '<tr class="rc-empty"><td colspan="' + cols.length + '">No records</td></tr>';
        $id('pickBody').onclick = function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            closePicker(); onPick(rows[+tr.getAttribute('data-i')]);
        };
        $id('pickModal').classList.add('is-open');
    }
    function closePicker() { $id('pickModal').classList.remove('is-open'); }

    /** Sum of the input Amounts (598 counts only rows with no MainItem: Calculation:2123). */
    function totalInputs() {
        return inputs.reduce(function (a, r) { return a + ((cfg.concrete && intOf(r.MainItemId) !== 0) ? 0 : num(r.Amount)); }, 0);
    }

    /** Calculation (774:1964, 845, 598:2123). */
    function calculation() {
        var si = totalInputs();
        var so = overheads.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        var sb = byproducts.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        var sx = 0, net;
        if (cfg.hasOperations) sx = operations.reduce(function (a, r) { return a + num(r.OperationCost); }, 0);        // Total OP
        if (cfg.concrete) sx = wages.reduce(function (a, r) { return a + num(r.Amount); }, 0);                           // Total Wages
        net = si + so + sx - sb;
        setVal('txtTotalInputs', String(roundAway(si, 2)));
        setVal('txtOverHead', String(roundAway(so, 2)));
        setVal('txtWagesAmount', String(roundAway(sx, 2)));
        setVal('txtLessByProduct', String(roundAway(sb, 2)));
        setVal('txtNetValue', cfg.concrete ? String(net) : String(roundAway(net, 2)));
        var q = num(val('txtQty'));
        setVal('txtValuePerQty', q !== 0 ? String(roundAway(net / q, 2)) : '');
    }

    /* ------------------------------------------------------------------ save / delete / print */

    function body() {
        var act = $id('ChkIsActive');
        return { Id: recId, DocNo: intOf(val('txtdocno')), DocDate: val('datDocDate'), EffectedDate: val('txtEffectedDate'), BomName: val('txtBom'),
            BomType: selText('cmbBOMType'), ItemId: cv('cmbItem'), ItemUomId: cv('cmbUOM'), CastingTypeId: cv('CmbCastingType'),
            ProductionTypeId: cv('CmbProductionType'), PlantId: cv('CmbPlantName'), TopForce: num(val('txtTopForce')), BottomForce: num(val('txtBottomForce')),
            BomBatchQty: num(val('txtQty')), RemarksHeader: val('txtRemarksHeader'), IsActive: act ? act.checked : true, IsDefault: $id('chkIsDefault').checked,
            inputs: inputs, byproducts: byproducts, overheads: overheads, wages: cfg.hasWages ? wages : [], operations: cfg.hasOperations ? operations : [],
            removedInputs: removedInputs, removedByproducts: removedByproducts, removedOperations: removedOperations };
    }
    /** Insert() (774:2002, 598:2150): FormValidation + grid test (server precheck) -> confirm -> save. */
    function insert(btnId) {
        var wasUpdate = recId > 0;
        var b = $id(btnId); if (b) b.classList.add('is-busy');
        postJson(api + '/precheck', body()).then(function () {
            if (!ask(wasUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return null;
            return postJson(api + '/save', body()).then(function (res) {
                box(res.message);
                var printId = intOf(res.id);
                return reset().then(function () { if ($id('ChkPreview').checked && rights.print) bomSlip(printId); });
            });
        }).catch(fail).then(function () { if (b) b.classList.remove('is-busy'); });
    }
    function save() { recId = 0; insert('btnsave'); }                       // btnsave_Click: RecId = 0
    function update() { insert('btnUpdate'); }                               // btnUpdate_Click
    /** btnDelete_Click (774:2600, 598:2688). */
    function deleteClick() {
        if (recId > 0) {
            if (!ask('Are you sure to Delete?')) return;
            request(api + '/delete?id=' + recId, { method: 'POST' }).then(function (res) { box(res.message); reset(); }).catch(fail);
        } else box('No record found to Delete');
    }
    /** BOMSlip (774 -> BillOfMaterial_Slip1617, 845 -> BOM_Casting_Slip1664, 598 -> BillOfMaterial_SlipAndRegister 1851). */
    function bomSlip(id) {
        if (!id) { box('Record Id Not Found'); return; }
        window.printRpt(RPT, { id: id, documentTypeId: cfg.docType, branchesId: look.branchId || 0 });
    }
    function print() { bomSlip(recId); }                                      // btnPrint_Click

    /** CmbRoutingName_Leave (774:4273): replaces the Operations grid with the routing's operations. */
    function routingLeave() {
        var id = cv('CmbRoutingName');
        if (id <= 0) return;
        if (!rights.update) { box('You do not have the Update right for this screen.'); clearCombo('CmbRoutingName'); return; }
        if (intOf(operations[0] && operations[0].WorkStationOperationId) > 0 && !ask('Are you sure to Reset Operations Grid Of New Routing?')) return;
        getJson(api + '/routing' + qs({ routingNameId: id })).then(function (rows) {
            operations = (rows && rows.length) ? rows : [blankOp()];
            calculation(); renderAll();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ read */

    /** ReadById (774:2467, 598:2518). */
    function readById(id) {
        return getJson(api + '/' + id).then(function (d) {
            var h = d.header || {};
            recId = id;
            tab('tabForm');
            setVal('datDocDate', isoOf(h.DocDate)); setVal('txtdocno', h.DocNo); setVal('txtRemarksHeader', h.RemarksHeader);
            if (cfg.hasCastingType) setCombo('CmbCastingType', h.CastingTypeId);
            if (cfg.concrete) setCombo('CmbPlantName', h.PlantId);
            var after = Promise.resolve();
            if (cfg.concrete) { setCombo('CmbProductionType', h.ProductionTypeId); after = productionTypeLeave(); }
            return after.then(function () {
                setCombo('cmbItem', h.ItemId); itemLeave(); setCombo('cmbUOM', h.ItemUomId);
                setVal('txtBom', h.BomName); setTypeByText('cmbBOMType', h.BomType);
                setVal('txtEffectedDate', isoOf(h.EffectedDate)); setVal('txtQty', h.BomBatchQty);
                if ($id('ChkIsActive')) $id('ChkIsActive').checked = !!h.IsActive;
                $id('chkIsDefault').checked = !!h.IsDefault;
                if (cfg.concrete) { setVal('txtTopForce', h.TopForce); setVal('txtBottomForce', h.BottomForce); }
                if (cfg.hasRouting) show('rowRouting', false);                         // Visible = false while editing
                inputs = (d.inputs && d.inputs.length) ? d.inputs : [blankDetail()];
                byproducts = (d.byproducts && d.byproducts.length) ? d.byproducts : [blankDetail()];
                overheads = (d.overheads && d.overheads.length) ? d.overheads : [blankOverhead()];
                wages = (d.wages && d.wages.length) ? d.wages : [blankWage()];
                operations = (d.operations && d.operations.length) ? d.operations : [blankOp()];
                removedInputs = []; removedByproducts = []; removedOperations = [];
                updateMode = true;
                vis = { save: false, update: true, del: true }; applyVisibility();
                lockItem();
                $id('lblRecId').textContent = 'Record #' + id;
                calculation(); renderAll();
            });
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */

    /** cmbDateTypeHistory_ValueChanged (598:cmbDateTypeHistory) - 1 and 2 move only the From date. */
    function datePreset() {
        var v = cv('cmbDateTypeHistory'), d = new Date();
        if (v === 1) setVal('txtFromdateHistory', iso(d));
        else if (v === 2) { var w = new Date(); w.setDate(w.getDate() - 7); setVal('txtFromdateHistory', iso(w)); }
        else if (v === 3) { setVal('txtFromdateHistory', iso(new Date(d.getFullYear(), d.getMonth(), 1))); setVal('txtToDateHistory', iso(d)); }
        else if (v === 4) { setVal('txtFromdateHistory', iso(new Date(d.getFullYear(), 0, 1))); setVal('txtToDateHistory', iso(d)); }
        else if (v === 5) { if (look.financialYearStart) setVal('txtFromdateHistory', isoOf(look.financialYearStart)); }
    }
    var HISTORY_DETAIL_GRIDS = ['grdHistoryInput', 'grdHistoryOutput', 'grdHistoryOverHead', 'grdWagesHistory', 'grdHistoryOperations'];
    function clearHistoryDetail() {
        HISTORY_DETAIL_GRIDS.forEach(function (g) { var t = $id(g); if (t) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; } });
    }
    /** btnNewHistory / refresh - clear the pickers and the detail grids. */
    function refreshHistory() {
        ['cmbDateTypeHistory', 'cmbItemhistory', 'CmbBomNoHistory', 'cmbbomtypeHistory'].forEach(clearCombo);
        clearHistoryDetail();
        focusEl('cmbDateTypeHistory');
    }
    function newHistory() { refreshHistory(); setVal('txtFromNoHistory', ''); setVal('txtToDocNoHistory', ''); }

    /** HistoryGridFill: the dtcol columns (Id / DocumentTypeId always hidden; 774/845 also hide IsActive / IsDefault; 845 hides RecordNo). */
    function historyCols() {
        var c = [];
        if (variant !== 'casting') c.push({ key: 'RecordNo', cap: 'RecordNo', w: 50, cls: 'ctr' });
        c.push({ key: 'DocNo', cap: 'DocNo', w: 60, cls: 'ctr' }, { key: 'DocDate', cap: 'DocDate', date: fmtDMY, w: 80 }, { key: 'BomName', cap: 'BomName', w: 150 });
        if (variant === 'concrete') c.push({ key: 'ProductionType', cap: 'BomType', w: 110 }, { key: 'PlantName', cap: 'PlantName', w: 110 }, { key: 'BomType', cap: 'FinishType', w: 100 });
        else c.push({ key: 'CastingType', cap: 'CastingType', w: 110 }, { key: 'BomType', cap: 'BomType', w: 100 });
        c.push({ key: 'ItemName', cap: 'ItemName', w: 150 }, { key: 'Uom', cap: 'Uom', w: 60 }, { key: 'BomBatchQty', cap: 'BomBatchQty', w: 70, num: true }, { key: 'EffectedDate', cap: 'EffectedDate', date: fmtDMY, w: 80 });
        if (variant === 'concrete') c.push({ key: 'IsActive', cap: 'IsActive', w: 40, check: true }, { key: 'IsDefault', cap: 'IsDefault', w: 50, check: true });
        c.push({ key: 'RemarksHeader', cap: 'RemarksHeader', w: 150 }, { key: 'EntryUser', cap: 'EntryUser', w: 100 }, { key: 'EntryDate', cap: 'EntryDate', date: fmtStamp, w: 110 },
            { key: 'ModifyUser', cap: 'ModifyUser', w: 100 }, { key: 'ModifyDate', cap: 'ModifyDate', date: fmtStamp, w: 110 },
            { key: 'IsApproved', cap: 'IsApproved', w: 60, check: true }, { key: 'NoOfAttachments', cap: 'NoOfAttachments', w: 70, num0: true });
        return c;
    }
    /** HistoryGridFill (774:2813, 598:2919). */
    function history() {
        getJson(api + '/history' + qs({ fromDate: val('txtFromdateHistory'), toDate: val('txtToDateHistory'), fromDocNo: intOf(val('txtFromNoHistory')) || '',
            toDocNo: intOf(val('txtToDocNoHistory')) || '', bomId: cv('CmbBomNoHistory') || '', bomType: cv('cmbbomtypeHistory') > 0 ? selText('cmbbomtypeHistory') : '',
            itemId: cfg.hasItem ? (cv('cmbItemhistory') || '') : '' })).then(function (rows) {
            historyRows = rows || []; historyCurrent = -1;
            $id('lblHistoryScope').textContent = rights.viewAll ? 'All users' : 'Your records only';
            renderHistory();
            if (historyRows.length) selectHistory(0); else clearHistoryDetail();
        }).catch(fail);
    }
    function renderHistory() {
        var cols = historyCols(), t = $id('DatagridHistory');
        t.tHead.innerHTML = '<tr><th style="width:50px">Print</th><th style="width:50px">Edit</th>' + cols.map(function (c) { return '<th style="min-width:' + c.w + 'px">' + esc(c.cap) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = historyRows.length ? historyRows.map(function (r, i) {
            return '<tr tabindex="0" data-i="' + i + '" class="' + (i === historyCurrent ? 'is-current' : '') + '">'
                + '<td class="ctr"><button type="button" class="rc-cellbtn" data-act="print">Print</button></td>'
                + '<td class="ctr"><button type="button" class="rc-cellbtn" data-act="edit">Edit</button></td>'
                + cols.map(function (c) {
                    var v = r[c.key];
                    if (c.num0) return '<td class="num">' + esc(v === null || v === undefined ? '0' : v) + '</td>';
                    if (c.check) return '<td class="ctr"><input type="checkbox" disabled' + (v ? ' checked' : '') + '></td>';
                    if (c.date) return '<td>' + esc(c.date(v)) + '</td>';
                    if (c.num) return '<td class="num">' + esc(fmt(v, 2)) + '</td>';
                    return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('') : '<tr class="rc-empty"><td colspan="' + (cols.length + 2) + '">No records</td></tr>';
        var sq = historyRows.reduce(function (a, r) { return a + num(r.BomBatchQty); }, 0);
        t.tFoot.innerHTML = historyRows.length ? '<tr><td></td><td></td>' + cols.map(function (c) { return '<td>' + (c.key === 'BomBatchQty' ? fmt(sq, 2) : '') + '</td>'; }).join('') + '</tr>' : '';
        $id('lblHistoryCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
    }
    /** DatagridHistory_SelectionChanged -> DatagridHistoryDetailbind (774:2925, 598:3099). */
    function selectHistory(i) {
        historyCurrent = i;
        $id('DatagridHistory').tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
        var r = historyRows[i]; if (!r) return;
        getJson(api + '/' + r.Id).then(function (d) {
            renderHistDetail('grdHistoryInput', d.inputs || [], 3, cfg.concrete);
            renderHistDetail('grdHistoryOutput', d.byproducts || [], 2, false);
            var t = $id('grdHistoryOverHead'), o = d.overheads || [];
            t.tHead.innerHTML = '<tr>' + (cfg.concrete ? '<th>OverHeadType</th>' : '') + '<th>OverHeadAccount</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Amount</th><th>OhRemarks</th></tr>';
            t.tBodies[0].innerHTML = o.map(function (x) {
                return '<tr>' + (cfg.concrete ? '<td>' + esc(x.OverHeadTypeId) + '</td>' : '') + '<td>' + esc(x.OverHeadAccount) + '</td><td class="num">' + fmt(x.PercentOfTotalInput, 2) + '</td><td class="num">' + fmt(x.Qty, 2)
                    + '</td><td class="num">' + fmtRate(x.Rate) + '</td><td class="num">' + fmt(x.Amount, 2) + '</td><td>' + esc(x.OhRemarks) + '</td></tr>';
            }).join('');
            t.tFoot.innerHTML = o.length ? '<tr>' + (cfg.concrete ? '<td></td>' : '') + '<td></td><td>' + fmt(o.reduce(function (a, x) { return a + num(x.PercentOfTotalInput); }, 0), 2) + '</td><td>' + fmt(o.reduce(function (a, x) { return a + num(x.Qty); }, 0), 2)
                + '</td><td></td><td>' + fmt(o.reduce(function (a, x) { return a + num(x.Amount); }, 0), 2) + '</td><td></td></tr>' : '';
            if (cfg.hasWages) {
                var w = d.wages || [], tw = $id('grdWagesHistory');
                tw.tHead.innerHTML = '<tr><th>ItemName</th><th>ServiceActivity</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
                tw.tBodies[0].innerHTML = w.map(function (x) {
                    return '<tr><td>' + esc(x.ItemName) + '</td><td>' + esc(x.ServiceActivity) + '</td><td class="num">' + fmt(x.Qty, 4) + '</td><td class="num">' + fmtRate(x.Rate) + '</td><td class="num">' + fmt(x.Amount, 2) + '</td><td>' + esc(x.Remarks) + '</td></tr>';
                }).join('');
                tw.tFoot.innerHTML = w.length ? '<tr><td></td><td></td><td>' + fmt(w.reduce(function (a, x) { return a + num(x.Qty); }, 0), 4) + '</td><td></td><td>' + fmt(w.reduce(function (a, x) { return a + num(x.Amount); }, 0), 2) + '</td><td></td></tr>' : '';
            }
            if (cfg.hasOperations) {
                var ops = d.operations || [], to = $id('grdHistoryOperations');
                to.tHead.innerHTML = '<tr><th>OperationName</th><th>WorkStationName</th><th>Hours</th><th>Minutes</th><th>SeqNo</th><th>HourlyCost</th><th>OperationCost</th><th>OperationRemarks</th></tr>';
                to.tBodies[0].innerHTML = ops.map(function (x) {
                    return '<tr><td>' + esc(x.OperationName) + '</td><td>' + esc(x.WorkStationName) + '</td><td class="num">' + x.Hours + '</td><td class="num">' + x.Minutes + '</td><td class="num">' + x.SeqNo
                        + '</td><td class="num">' + fmtRate(x.HourlyCost) + '</td><td class="num">' + fmt(x.OperationCost, 2) + '</td><td>' + esc(x.OperationRemarks) + '</td></tr>';
                }).join('');
                to.tFoot.innerHTML = ops.length ? '<tr><td></td><td></td><td></td><td></td><td></td><td></td><td>' + fmt(ops.reduce(function (a, x) { return a + num(x.OperationCost); }, 0), 2) + '</td><td></td></tr>' : '';
            }
        }).catch(fail);
    }
    function renderHistDetail(tableId, rows, amountPlaces, extras) {
        var t = $id(tableId);
        t.tHead.innerHTML = '<tr><th>Item</th><th>Uom</th><th>ItemQty</th><th>NetWeight</th><th>Rate</th><th>RateUom</th><th>Amount</th>' + (extras ? '<th>MinQty</th><th>MaxQty</th><th>MainItemName</th>' : '') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (x) {
            return '<tr><td>' + esc(x.ItemName) + '</td><td>' + esc(x.UomCode) + '</td><td class="num">' + fmt(x.ItemQty, 2) + '</td><td class="num">' + fmt(x.NetWeight, 2) + '</td><td class="num">' + fmtRate(x.Rate)
                + '</td><td>' + esc(x.RateUomCode) + '</td><td class="num">' + fmt(x.Amount, amountPlaces) + '</td>'
                + (extras ? '<td class="num">' + fmt(x.MinQty, 2) + '</td><td class="num">' + fmt(x.MaxQty, 2) + '</td><td>' + esc(x.MainItemName) + '</td>' : '') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = rows.length ? '<tr><td></td><td></td><td>' + fmt(rows.reduce(function (a, x) { return a + num(x.ItemQty); }, 0), 2) + '</td><td>' + fmt(rows.reduce(function (a, x) { return a + num(x.NetWeight); }, 0), 2)
            + '</td><td></td><td></td><td>' + fmt(rows.reduce(function (a, x) { return a + num(x.Amount); }, 0), amountPlaces) + '</td>' + (extras ? '<td></td><td></td><td></td>' : '') + '</tr>' : '';
    }
    /** DatagridHistory_ColumnButtonClick / _DoubleClick (774:2935, 4140; 598:4211). */
    function historyAction(i, act) {
        var r = historyRows[i]; if (!r) return;
        selectHistory(i);
        var id = intOf(r.Id);
        if (act === 'print') return bomSlip(id);
        if (act === 'edit' || act === 'dbl') {
            if (!rights.update) { box((variant === 'concrete' ? "You don't" : "you don't") + ' have update rights...'); return; }
            return readById(id);
        }
    }

    /* ------------------------------------------------------------------ keyboard */

    function shortcutKeys() {
        box(['Ctrl+S  For Save When on Entry form and For Show Data when on History Form', 'Ctrl+U  For Update', 'Ctrl+Shift+Delete  For Delete', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh',
            'Ctrl+N  For New', 'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Doc Date', 'F1  For Combo Lookup on Current Column of any Focused Grid',
            'Ctrl+T  For Tab Transfer', 'Ctrl+D  For Adding an row in Focused Grid', 'Ctrl+Delete  For Deleting an row of Focused Grid',
            'Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowUp  For Focus On on Doc Date', 'Ctrl+ArrowRight  to change focus from one grid to another'].join('\n'));
    }
    function focusNext(from) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button, [tabindex="0"]'), function (el) {
            return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null && !(el.tagName === 'SELECT' && el.classList.contains('dtcombo') && getComputedStyle(el).display === 'none');
        });
        var i = all.indexOf(from);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    var GRID_KIND = { grdInputMaterial: 'input', grdOutPutByProduct: 'output', grdOverHead: 'oh', grdContractorWages: 'wages', grdOperatinalDetail: 'ops' };
    function focusedGrid(t) {
        if (!t || !t.closest) return null;
        var tb = t.closest('table.rc-grid');
        return tb && GRID_KIND[tb.id] ? GRID_KIND[tb.id] : null;
    }
    function gridOrder() { return ['grdInputMaterial', 'grdOutPutByProduct', 'grdOverHead'].concat(cfg.hasWages ? ['grdContractorWages'] : []).concat(cfg.hasOperations ? ['grdOperatinalDetail'] : []); }
    /** frmBillOfMaterial KeyDown (774:3320, 598:3400). */
    function onKey(e) {
        var t = e.target, k = e.key, ctrl = e.ctrlKey || e.metaKey;
        if (k === 'Escape') {
            if (e.defaultPrevented) return;
            if ($id('pickModal').classList.contains('is-open')) { closePicker(); return; }
            window.location.href = '/production'; return;
        }
        if (k === 'F1') {
            var gk = focusedGrid(t), trr = t && t.closest ? t.closest('tr[data-i]') : null;
            if (gk === 'wages' && trr) { e.preventDefault(); pickService(+trr.getAttribute('data-i')); }
            return;
        }
        if (k === 'Enter' && !ctrl) {
            if (e.defaultPrevented || !t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.tagName === 'TR') return;
            e.preventDefault(); focusNext(t); return;
        }
        if (!ctrl) return;
        var key = k.length === 1 ? k.toLowerCase() : k, onHistory = currentTab() === 1;
        if (key === 't') { e.preventDefault(); tab(onHistory ? 'tabForm' : 'tabHistory'); if (!onHistory) focusEl('datDocDate'); return; }
        if (key === 'e') { e.preventDefault(); window.location.href = '/production'; return; }
        if (!onHistory) {
            var g = focusedGrid(t), tr = t && t.closest ? t.closest('tr[data-i]') : null, i = tr ? +tr.getAttribute('data-i') : -1;
            if (key === 'Delete' && e.shiftKey) { e.preventDefault(); if (rights.delete && vis.del) deleteClick(); return; }
            if (key === 'Delete' && g && i >= 0) { e.preventDefault(); gridButton(g, i, 'del'); return; }
            if (key === 'd' && g) { e.preventDefault(); gridButton(g, i, 'add'); return; }
            if (key === 's') { e.preventDefault(); if (vis.save && rights.save) save(); return; }
            if (key === 'u') { e.preventDefault(); if (vis.update && rights.update && updateMode) update(); return; }
            if (key === 'p') { e.preventDefault(); if (rights.print) print(); return; }
            if (key === 'n') { e.preventDefault(); reset(); return; }
            if (key === 'r') { e.preventDefault(); refresh(); return; }
            if (key === 'F5') { e.preventDefault(); focusEl('datDocDate'); return; }
            if (key === 'ArrowDown') { e.preventDefault(); var fr = $id('grdInputMaterial').querySelector('tr[data-i]'); if (fr) fr.focus(); return; }
            if (key === 'ArrowUp') { e.preventDefault(); focusEl('datDocDate'); return; }
            if (key === 'ArrowRight') {
                e.preventDefault();
                var order = gridOrder(), cur = order.indexOf(t && t.closest && t.closest('table.rc-grid') ? t.closest('table.rc-grid').id : '');
                var nr = $id(order[(cur + 1) % order.length]).querySelector('tr[data-i]'); if (nr) nr.focus();
            }
        } else {
            if (key === 's') { e.preventDefault(); history(); return; }
            if (key === 'ArrowDown') { e.preventDefault(); var hr = $id('DatagridHistory').querySelector('tr[data-i]'); if (hr) hr.focus(); }
        }
    }

    /* ------------------------------------------------------------------ wiring */
    function rebuildMainItems(i) {
        var others = [], seen = {};
        inputs.forEach(function (x, ix) { if (ix !== i && intOf(x.ItemId) > 0 && !seen[x.ItemId]) { seen[x.ItemId] = true; others.push({ Id: x.ItemId, Name: x.ItemName }); } });
        setOptions('dlMainItems', others);
    }
    function wire() {
        document.addEventListener('keydown', onKey);
        ['txtQty', 'txtTopForce', 'txtBottomForce'].forEach(function (id) {                          // OnlytextNumberFunction
            var el = $id(id); if (!el) return;
            el.addEventListener('keypress', function (e) { if (e.key.length === 1 && !e.ctrlKey && !/[0-9.]/.test(e.key)) e.preventDefault(); });
        });
        ['txtFromNoHistory', 'txtToDocNoHistory'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !e.ctrlKey && !/[0-9]/.test(e.key)) e.preventDefault(); });
        });
        $id('txtQty').addEventListener('change', calculation);
        Object.keys(GRID_KIND).forEach(function (tid) {
            var tbl = $id(tid); if (!tbl) return;
            var kind = GRID_KIND[tid], body = tbl.tBodies[0];
            body.addEventListener('click', function (e) {
                var tr = e.target.closest('tr[data-i]'); if (!tr) return;
                var i = +tr.getAttribute('data-i');
                current[kind] = i;
                body.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', +x.getAttribute('data-i') === i); });
                var b = e.target.closest('button[data-act]'); if (!b) return;
                gridButton(kind, i, b.getAttribute('data-act'));
            });
            body.addEventListener('change', function (e) {
                var el = e.target; if (!el.classList.contains('cell')) return;
                var i = +el.getAttribute('data-i'), k = el.getAttribute('data-k');
                if (kind === 'oh') overheadCellChanged(i, k, el);
                else if (kind === 'wages') wagesCellChanged(i, k, el);
                else if (kind === 'ops') opCellChanged(i, k, el);
                else detailCellChanged(kind, i, k, el);
            });
            body.addEventListener('focusin', function (e) {
                var el = e.target;
                if (kind === 'input' && el.getAttribute && el.getAttribute('data-k') === 'MainItemName') rebuildMainItems(+el.getAttribute('data-i'));
            });
            body.addEventListener('mousedown', function (e) {
                var el = e.target;
                if (kind === 'input' && el.getAttribute && el.getAttribute('data-k') === 'MainUomCode') {
                    var r = inputs[+el.getAttribute('data-i')];
                    if (r && intOf(r.MainItemId) === 0) { e.preventDefault(); box('Select Main Item First...'); }
                }
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

    window.BomW5B = {
        newClick: reset, refresh: refresh, save: save, update: update, deleteClick: deleteClick, print: print, shortcutKeys: shortcutKeys, tab: tab,
        itemLeave: itemLeave, productionTypeLeave: productionTypeLeave, routingLeave: routingLeave, datePreset: datePreset,
        newHistory: newHistory, refreshHistory: refreshHistory, history: history, closePicker: closePicker, calculation: calculation
    };
})();
