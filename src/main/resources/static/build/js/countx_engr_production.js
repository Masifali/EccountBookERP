/* ============================================================================================
 * Screen 795 "Production (Casting)" - Architecture.WinApp.Mfg.frmProductionEngr (DocumentTypeId 1670).
 * Desktop line numbers (.cs): BindJobLot:567, BindCastingType:622, GenerateCode:667, GetHeatCode:698, GetAllBomHeader:742, BindBomHeader:755, BindItemInputScrap:822,
 * GetWarehouses:891, BindOverHeadAccounts:937, FormValidation:980, BtnLoadInputFromBom_Click:1021, FillInputDetailForMultiBom:1043, DeleteInputDetailRow:1225,
 * DeleteOutputDetailRow:1575, DeleteScrapDetailRow:1781, Insert:1820, ReadById:2168, BtnDelete_Click:2259, Reset:2296, BtnRefresh_Click:2361, Calculation:2421,
 * BtnGenerate_Click:2501, UpdateOutputRates:2517, HistoryGridFill:2615, BindAllDetailHistories:2876, grdInput/grdScrap/grdOutPut/grdOverHead cell updates and KeyDown,
 * BtnLoadOutPut_Click:3943. API /production/engr-production/api.
 * Not ported: Attachment dialog (DMS), SaveAs, shortcut-keys dialog, grid layout files. In-cell pack/rate UOM is a drop-down of the item's UOMs (desktop: F1 picker of the same list).
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/engr-production/api';
    var $ = W3B.$, n = W3B.num, I = W3B.int;

    var look = {}, rights = {};
    var recId = 0, isApproved = false;
    var L = { wh: [], uoms: [], inputItems: [], scrapItems: [], accounts: [], boms: [], casting: [], stages: [] };
    var G = { inp: [], oh: [], scr: [], out: [] };
    var removedOut = [], removedIn = [];
    var hist = [];
    var cfg = { whDefault: 0, stageId: 0, stageName: '', decimals: 2 };

    // ----------------------------------------------------------------------------- helpers
    function alertMsg(m) { W3B.box(m); }
    function v(id) { return $(id).value; }
    function sel(id) { return I(v(id)); }
    function fmt(x, p) { return n(x).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: p === undefined ? 3 : p }); }
    function fmtPct(x) { var r = Math.round(n(x) * 100) / 100; return r === 0 ? '' : r.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }); }   // "#,##.##"
    function roundAway(x, d) { var s = x < 0 ? -1 : 1, f = Math.pow(10, d); return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function nameOf(list, id) { for (var i = 0; i < list.length; i++) if (String(list[i].Id) === String(id)) return list[i].Name; return ''; }
    function uomsOf(itemId) { return L.uoms.filter(function (u) { return I(u.ItemId) === I(itemId); }); }
    function uomName(u) { return u.UOMCode || u.UOMDescription || ''; }

    var BLANK = {
        inp: function () { return { Id: 0, BomHeaderId: 0, BomInputDetailId: 0, WarehouseId: cfg.whDefault, ItemId: 0, ItemName: '', PackUomId: 0, PackUom: '', PackEquivalent: 0, NetWeight: 0, Rate: 0, RateUomId: 0, RateUom: '', RateEquivalent: 0, Amount: 0 }; },
        scr: function () { return { Id: 0, WarehouseId: 0, ItemId: 0, ItemName: '', PackUomId: 0, PackUom: '', PackEquivalent: 0, NetWeight: 0, Rate: 0, RateUomId: 0, RateUom: '', RateEquivalent: 0, Amount: 0 }; },
        oh: function () { return { Id: 0, OverHeadAccountId: 0, OverHeadAccount: '', PercentOfTotalInput: 0, Qty: 0, Rate: 0, Amount: 0, OhRemarks: '' }; }
    };

    // ----------------------------------------------------------------------------- picker (GrdPopUp)
    var pickRows = [], pickCb = null, pickMulti = false, pickCols = null;
    function pick(title, rows, cb, multi, cols) {
        pickRows = rows || []; pickCb = cb; pickMulti = !!multi; pickCols = cols || [{ h: 'Id', k: 'Id' }, { h: 'Name', k: 'Name' }];
        $('pickTitle').textContent = title; $('pickSearch').value = '';
        W3B.show('pickOk', pickMulti);
        pickRender(); $('pickModal').classList.add('is-open'); W3B.focus('pickSearch');
    }
    function pickRender() {
        var q = $('pickSearch').value.toLowerCase(), h = '<thead><tr>' + (pickMulti ? '<th style="width:26px"></th>' : '') +
            pickCols.map(function (c) { return '<th>' + W3B.esc(c.h) + '</th>'; }).join('') + '</tr></thead><tbody>', c = 0;
        pickRows.forEach(function (r, i) {
            var hay = pickCols.map(function (x) { return String(r[x.k] === undefined || r[x.k] === null ? '' : r[x.k]); }).join(' ').toLowerCase();
            if (c < 400 && (!q || hay.indexOf(q) >= 0)) {
                c++;
                h += '<tr data-pi="' + i + '" class="pk-row">' + (pickMulti ? '<td><input type="checkbox" data-pc="' + i + '"' + (r.__chk ? ' checked' : '') + '></td>' : '') +
                    pickCols.map(function (x) { return '<td>' + W3B.esc(r[x.k]) + '</td>'; }).join('') + '</tr>';
            }
        });
        if (!c) h += '<tr class="rc-empty"><td colspan="' + (pickCols.length + 1) + '">No records</td></tr>';
        $('pickGrid').innerHTML = h + '</tbody>';
    }
    function pickClose() { $('pickModal').classList.remove('is-open'); pickCb = null; }
    function pickOk() { var cb = pickCb, rows = pickRows.filter(function (r) { return r.__chk; }); pickClose(); if (cb && rows.length) cb(rows); }
    function pickChoose(i) { var r = pickRows[i], cb = pickCb; if (pickMulti) { r.__chk = !r.__chk; pickRender(); return; } pickClose(); if (cb && r) cb(r); }
    $('pickSearch').addEventListener('input', pickRender);
    $('pickSearch').addEventListener('keydown', function (e) {
        if (e.key === 'Escape') pickClose();
        if (e.key === 'Enter') { var f = document.querySelector('#pickGrid tr.pk-row'); if (f) pickChoose(+f.dataset.pi); }
    });
    $('pickGrid').addEventListener('click', function (e) {
        var c = e.target.closest('input[data-pc]'); if (c) { pickRows[+c.dataset.pc].__chk = c.checked; return; }
        var tr = e.target.closest('tr.pk-row'); if (tr) pickChoose(+tr.dataset.pi);
    });

    // ----------------------------------------------------------------------------- lists
    function applyLookups(d) {
        cfg.whDefault = I(d.defaultCastingWarehouseId); cfg.stageId = I(d.defaultProductionStageId); cfg.stageName = d.defaultProductionStageName || '';
        cfg.decimals = d.amountDecimals > 0 ? I(d.amountDecimals) : 2;
        L.wh = d.warehouses || []; L.uoms = d.uoms || []; L.inputItems = d.inputItems || []; L.scrapItems = d.scrapItems || []; L.accounts = d.overheadAccounts || [];
        L.boms = d.boms || []; L.casting = d.castingTypes || [];
        W3B.fill('CmbCastingType', L.casting, true);
        W3B.fill('CmbHeatCode', d.heatCodes || [], true);
        W3B.fill('CmbJobLot', d.jobLots || [], true);
        bindBom();
    }
    function bindBom() {                                                  // BindBomHeader:755 (filtered by the casting type)
        var c = sel('CmbCastingType'), prev = sel('CmbBillOfMaterial');
        var rows = c > 0 ? L.boms.filter(function (b) { return I(b.CastingTypeId) === c; }).map(function (b) { return { Id: b.Id, Name: b.BomName }; }) : [];
        W3B.fill('CmbBillOfMaterial', rows, true, prev);
    }
    function castingLeave() { bindBom(); if (sel('CmbCastingType') === 0) W3B.clearCombo('CmbBillOfMaterial'); }     // CmbCastingType_Leave:961

    // ----------------------------------------------------------------------------- grid definitions
    function whOpts(cur) { return '<option value="0"></option>' + L.wh.map(function (w) { return '<option value="' + W3B.esc(w.Id) + '"' + (String(w.Id) === String(cur) ? ' selected' : '') + '>' + W3B.esc(w.Name) + '</option>'; }).join(''); }
    function uomOpts(itemId, cur, name) {
        var us = uomsOf(itemId), h = '<option value="0"></option>', found = false;
        us.forEach(function (u) { var s = String(u.Id) === String(cur); if (s) found = true; h += '<option value="' + W3B.esc(u.Id) + '"' + (s ? ' selected' : '') + '>' + W3B.esc(uomName(u)) + '</option>'; });
        if (!found && I(cur) > 0) h += '<option value="' + W3B.esc(cur) + '" selected>' + W3B.esc(name) + '</option>';
        return h;
    }
    function cellInput(g, i, f, val, cls, ro) { return '<input class="cell ' + (cls || 'num') + '" data-g="' + g + '" data-i="' + i + '" data-f="' + f + '" value="' + W3B.esc(val) + '"' + (ro ? ' readonly tabindex="-1"' : '') + '>'; }
    function cellText(g, i, f, val) { return '<span data-g="' + g + '" data-i="' + i + '" data-f="' + f + '">' + W3B.esc(val) + '</span>'; }
    function btns(g, i, add) { return '<button type="button" class="rc-x" data-act="del" data-g="' + g + '" data-i="' + i + '">X</button>' + (add ? ' <button type="button" class="rc-add" data-act="add" data-g="' + g + '" data-i="' + i + '">+</button>' : ''); }

    function itemCols(g) {
        return [
            { h: 'WareHouse', html: true, w: 150, f: function (r, i) { return '<select class="cell" data-g="' + g + '" data-i="' + i + '" data-f="WarehouseId">' + whOpts(r.WarehouseId) + '</select>'; } },
            { h: 'Item Name', html: true, cls: 'nowrap', w: 220, f: function (r, i) { return '<span data-g="' + g + '" data-i="' + i + '" data-f="ItemName">' + W3B.esc(r.ItemName) + '</span><button type="button" class="pk" data-pk="item" data-g="' + g + '" data-i="' + i + '">F1</button>'; } },
            { h: 'Uom', html: true, w: 90, f: function (r, i) { return '<select class="cell" data-g="' + g + '" data-i="' + i + '" data-f="PackUomId">' + uomOpts(r.ItemId, r.PackUomId, r.PackUom) + '</select>'; } },
            { h: g === 'scr' ? 'Weight' : 'Qty', html: true, w: 90, f: function (r, i) { return cellInput(g, i, 'NetWeight', r.NetWeight ? fmt(r.NetWeight) : ''); } },
            { h: 'Rate', html: true, w: 90, f: function (r, i) { return cellInput(g, i, 'Rate', r.Rate ? fmt(r.Rate) : ''); } },
            { h: 'RateUom', html: true, w: 90, f: function (r, i) { return '<select class="cell" data-g="' + g + '" data-i="' + i + '" data-f="RateUomId">' + uomOpts(r.ItemId, r.RateUomId, r.RateUom) + '</select>'; } },
            { h: 'Amount', html: true, cls: 'num', w: 100, f: function (r, i) { return cellText(g, i, 'Amount', fmt(r.Amount)); } },
            { h: '', html: true, w: 60, f: function (r, i) { return btns(g, i, true); } }
        ];
    }
    function render(which) {
        var all = !which;
        if (all || which === 'inp') $('grdInput').innerHTML = W3B.tableHtml(itemCols('inp'), G.inp, { empty: 'No rows' });
        if (all || which === 'scr') $('grdScrap').innerHTML = W3B.tableHtml(itemCols('scr'), G.scr, { empty: 'No rows' });
        if (all || which === 'oh') $('grdOverHead').innerHTML = W3B.tableHtml([
            { h: 'Account', html: true, cls: 'nowrap', w: 260, f: function (r, i) { return '<span data-g="oh" data-i="' + i + '" data-f="OverHeadAccount">' + W3B.esc(r.OverHeadAccount) + '</span><button type="button" class="pk" data-pk="acct" data-g="oh" data-i="' + i + '">F1</button>'; } },
            { h: 'Percentage', html: true, w: 90, f: function (r, i) { return cellInput('oh', i, 'PercentOfTotalInput', r.PercentOfTotalInput ? fmt(r.PercentOfTotalInput) : ''); } },
            { h: 'Qty', html: true, w: 90, f: function (r, i) { return cellInput('oh', i, 'Qty', r.Qty ? fmt(r.Qty) : ''); } },
            { h: 'Rate', html: true, w: 90, f: function (r, i) { return cellInput('oh', i, 'Rate', r.Rate ? fmt(r.Rate) : ''); } },
            { h: 'Amount', html: true, w: 110, f: function (r, i) { return cellInput('oh', i, 'Amount', r.Amount ? fmt(r.Amount) : ''); } },
            { h: 'ohRemarks', html: true, f: function (r, i) { return cellInput('oh', i, 'OhRemarks', r.OhRemarks, 'txt'); } },
            { h: '', html: true, w: 60, f: function (r, i) { return btns('oh', i, true); } }
        ], G.oh, { empty: 'No rows' });
        if (all || which === 'out') $('grdOutPut').innerHTML = W3B.tableHtml([
            { h: 'WorkOrderNo', html: true, cls: 'nowrap', w: 120, f: function (r, i) { return '<span data-g="out" data-i="' + i + '" data-f="WorkOrderNo">' + W3B.esc(r.WorkOrderNo) + '</span><button type="button" class="pk" data-pk="wo" data-g="out" data-i="' + i + '">F1</button>'; } },
            { h: 'WareHouse', html: true, w: 140, f: function (r, i) { return '<select class="cell" data-g="out" data-i="' + i + '" data-f="WarehouseId">' + whOpts(r.WarehouseId) + '</select>'; } },
            { h: 'ItemName', html: true, cls: 'nowrap', f: function (r, i) { return cellText('out', i, 'ItemName', r.ItemName); } },
            { h: 'Uom', html: true, f: function (r, i) { return cellText('out', i, 'PackUom', r.PackUom); } },
            { h: 'ModalDescription', html: true, f: function (r, i) { return cellText('out', i, 'ModalDescription', r.ModalDescription); } },
            { h: 'Production Stage', html: true, f: function (r, i) { return cellText('out', i, 'ProcessStatus', r.ProcessStatus); } },
            { h: 'Qty', html: true, w: 80, f: function (r, i) { return cellInput('out', i, 'NetWeight', r.NetWeight ? fmt(r.NetWeight) : ''); } },
            { h: 'BalQty', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'BalQty', fmt(r.BalQty)); } },
            { h: 'WeightPerQty', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'WeightPerQty', fmt(r.WeightPerQty)); } },
            { h: 'CastingWeight', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'CastingWeight', fmtPct(r.CastingWeight)); } },
            { h: 'Rate', html: true, w: 80, f: function (r, i) { return cellInput('out', i, 'Rate', r.Rate ? fmt(r.Rate) : ''); } },
            { h: 'RateUom', html: true, w: 90, f: function (r, i) { return '<select class="cell" data-g="out" data-i="' + i + '" data-f="RateUomId">' + uomOpts(r.ItemId, r.RateUomId, r.RateUom) + '</select>'; } },
            { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'Amount', fmt(r.Amount)); } },
            { h: '', html: true, w: 40, f: function (r, i) { return btns('out', i, false); } }
        ], G.out, { empty: 'No rows' });
        calculation();
        castingEnable();
    }
    function ensureBlanks() {                                             // empty input / scrap / overhead grids get their blank row back (not the output grid)
        ['inp', 'scr', 'oh'].forEach(function (g) { if (!G[g].length) G[g].push(BLANK[g]()); });
    }
    function castingEnable() { $('CmbCastingType').disabled = G.out.some(function (r) { return I(r.WorkOrderId) > 0; }); }

    // ----------------------------------------------------------------------------- cell updates (grdOutPut/grdInput/grdScrap/grdOverHead _CellUpdated)
    function calcAmount(r) {                                              // Amount = Ruom > 0 ? NetWeight / Ruom * rate : 0
        var ru = n(r.RateEquivalent); r.Amount = ru > 0 ? n(r.NetWeight) / ru * n(r.Rate) : 0;
    }
    function refreshRow(g, i) {
        var r = G[g][i]; if (!r) return;
        document.querySelectorAll('[data-g="' + g + '"][data-i="' + i + '"][data-f]').forEach(function (el) {
            var f = el.dataset.f, val = r[f];
            if (el.tagName === 'SELECT') return;
            if (document.activeElement === el) return;
            var txt;
            if (f === 'CastingWeight') txt = fmtPct(val);
            else if (f === 'OhRemarks' || f === 'ItemName' || f === 'OverHeadAccount' || f === 'WorkOrderNo' || f === 'PackUom' || f === 'ModalDescription' || f === 'ProcessStatus') txt = val === undefined || val === null ? '' : String(val);
            else txt = (val === 0 || val === '' || val === undefined) && el.tagName === 'INPUT' ? '' : fmt(val);
            if (el.tagName === 'INPUT') el.value = txt; else el.textContent = txt;
        });
    }
    $('grdInput').addEventListener('input', cellInputEvt); $('grdScrap').addEventListener('input', cellInputEvt);
    $('grdOverHead').addEventListener('input', cellInputEvt); $('grdOutPut').addEventListener('input', cellInputEvt);
    function cellInputEvt(e) {
        var t = e.target; if (!t.dataset || !t.dataset.g || t.tagName !== 'INPUT') return;
        var r = G[t.dataset.g][+t.dataset.i]; if (!r) return;
        r[t.dataset.f] = t.dataset.f === 'OhRemarks' ? t.value : (t.value === '' ? 0 : n(t.value));
    }
    ['grdInput', 'grdScrap', 'grdOverHead', 'grdOutPut'].forEach(function (id) { $(id).addEventListener('change', cellChange); });
    function cellChange(e) {
        var t = e.target; if (!t.dataset || !t.dataset.g || (t.tagName !== 'INPUT' && t.tagName !== 'SELECT')) return;
        var g = t.dataset.g, i = +t.dataset.i, f = t.dataset.f, r = G[g][i]; if (!r) return;
        if (t.tagName === 'SELECT') {
            r[f] = I(t.value);
            if (f === 'PackUomId') { var u = uomsOf(r.ItemId).filter(function (x) { return I(x.Id) === r.PackUomId; })[0]; r.PackUom = u ? uomName(u) : ''; r.PackEquivalent = u ? n(u.Equivalent) : 0; }
            if (f === 'RateUomId') { var ru = uomsOf(r.ItemId).filter(function (x) { return I(x.Id) === r.RateUomId; })[0]; r.RateUom = ru ? uomName(ru) : ''; r.RateEquivalent = ru ? n(ru.Equivalent) : 0; calcAmount(r); }
            refreshRow(g, i); calculation(); return;
        }
        if (g === 'oh') { ohChanged(i, f); return; }
        if (f === 'NetWeight' || f === 'Rate') {
            if (g === 'out') r.CastingWeight = n(r.NetWeight) * n(r.WeightPerQty);
            calcAmount(r);
        }
        t.value = r[f] ? fmt(r[f]) : '';
        refreshRow(g, i); calculation();
    }
    function ohChanged(i, f) {                                            // grdOverHead_CellUpdated
        var r = G.oh[i];
        try {
            if (f === 'Qty' || f === 'Rate') {
                if (n(r.Qty) <= 0 || n(r.Rate) <= 0) throw new Error('Qty And Rate Should be greater than zero...');
                r.Amount = n(r.Qty) * n(r.Rate);
            } else if (f === 'PercentOfTotalInput') {
                var total = G.oh.reduce(function (s, x) { return s + n(x.PercentOfTotalInput); }, 0);
                if (total > 100) {
                    r.PercentOfTotalInput = n(r.PercentOfTotalInput) - (total - 100);
                    refreshRow('oh', i);
                    throw new Error('TotalPercentage Can not be Greater than 100...');
                }
                var inputAmount = G.inp.reduce(function (s, x) { return s + n(x.Amount); }, 0);
                r.Amount = roundAway(inputAmount / 100 * n(r.PercentOfTotalInput), cfg.decimals); r.Qty = 0; r.Rate = 0;
            } else if (f === 'Amount') { r.Qty = 0; r.Rate = 0; }
        } catch (ex) { alertMsg(ex.message); }
        var ids = document.querySelectorAll('#grdOverHead input[data-i="' + i + '"]'); ids.forEach(function (el) { el.blur(); });
        refreshOhRow(i); calculation();
    }
    function refreshOhRow(i) {
        var r = G.oh[i];
        document.querySelectorAll('#grdOverHead input[data-i="' + i + '"]').forEach(function (el) {
            var f = el.dataset.f; if (f === 'OhRemarks') return; el.value = r[f] ? fmt(r[f]) : '';
        });
    }

    // ----------------------------------------------------------------------------- row buttons / pickers
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('button[data-act]') : null;
        if (b) { if (b.dataset.act === 'del') deleteRow(b.dataset.g, +b.dataset.i); else addRow(b.dataset.g, +b.dataset.i); return; }
        var p = e.target.closest ? e.target.closest('button.pk') : null;
        if (p) { openPick(p.dataset.pk, p.dataset.g, +p.dataset.i); return; }
        var h = e.target.closest ? e.target.closest('button[data-hact]') : null;
        if (h) {
            var hi = +h.dataset.hi, row = hist[hi];
            if (h.dataset.hact === 'edit') load(row.Id);
            else if (h.dataset.hact === 'print') printSlip(row.Id);
            else if (h.dataset.hact === 'voucher') { if (I(row.IsSettled) !== 1) alertMsg('This Document Is not settled yet'); else printVoucherFor(row.Id); }
            return;
        }
        var tr = e.target.closest ? e.target.closest('#grdhistory tbody tr[data-i]') : null;
        if (tr) historyDetail(hist[+tr.dataset.i].Id);
    });
    document.addEventListener('dblclick', function (e) {
        var th = e.target.closest ? e.target.closest('#grdhistory tbody tr[data-i]') : null;
        if (th && !e.target.closest('button')) load(hist[+th.dataset.i].Id);
    });
    document.addEventListener('keydown', function (e) {
        var cell = e.target.closest ? e.target.closest('#grdInput,#grdScrap,#grdOverHead,#grdOutPut') : null;
        if (!cell) return;
        var g = { grdInput: 'inp', grdScrap: 'scr', grdOverHead: 'oh', grdOutPut: 'out' }[cell.id];
        var tr = e.target.closest('tr'); var any = tr ? tr.querySelector('[data-i]') : null; var i = any ? +any.dataset.i : -1;
        if (i < 0) return;
        if (e.key === 'F1') { var p = tr.querySelector('button.pk'); if (p) { e.preventDefault(); openPick(p.dataset.pk, g, i); } }
        if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteRow(g, i); }
        if (e.ctrlKey && (e.key === 'd' || e.key === 'D') && g !== 'out') { e.preventDefault(); addRow(g, i); }
    });
    function addRow(g, i) { if (g === 'out') return; G[g].splice(i + 1, 0, BLANK[g]()); render(g); }
    function removedCopy(g, r) {
        var o = JSON.parse(JSON.stringify(r)); if (g === 'out') o.OutPutTypeId = 1; if (g === 'scr') o.OutPutTypeId = 2; return o;
    }
    function deleteRow(g, i) {                                            // DeleteInputDetailRow:1225 / DeleteOutputDetailRow:1575 / DeleteScrapDetailRow:1781
        var r = G[g][i]; if (!r) return;
        if (I(r.Id) > 0) {
            if (g === 'inp') removedIn.push(removedCopy(g, r));
            else if (g === 'out' || g === 'scr') removedOut.push(removedCopy(g, r));
        }
        G[g].splice(i, 1);
        if (g !== 'out') ensureBlanks();
        render(g);
    }
    function openPick(kind, g, i) {
        var r = G[g][i]; if (!r) return;
        if (kind === 'item') {                                            // grdInput_KeyDown:3533 / grdScrap_KeyDown - both use the scrap item list (desktop dtitemScrap)
            var rows = L.scrapItems.map(function (x) { return { Id: x.Id, ItemName: x.ItemName, ItemCode: x.ItemCode }; });
            pick('Item', rows, function (picked) {
                picked.forEach(function (x, k) {
                    var row = k === 0 ? r : BLANK[g]();
                    if (k > 0) { row.WarehouseId = r.WarehouseId; G[g].splice(i + k, 0, row); }
                    row.ItemId = I(x.Id); row.ItemName = x.ItemName; row.PackUomId = 0; row.PackUom = ''; row.PackEquivalent = 0; row.RateUomId = 0; row.RateUom = ''; row.RateEquivalent = 0; row.Amount = 0;
                });
                render(g);
            }, true, [{ h: 'ItemName', k: 'ItemName' }, { h: 'ItemCode', k: 'ItemCode' }]);
        } else if (kind === 'acct') {                                     // grdOverHead_KeyDown - multi select, distinct accounts
            var accts = L.accounts.map(function (x) { return { Id: x.Id, AccountTitle: x.AccountTitle, AccountCode: x.AccountCode }; });
            pick('Over Head Account', accts, function (picked) {
                var k = 0;
                picked.forEach(function (x) {
                    if (G.oh.some(function (o) { return I(o.OverHeadAccountId) === I(x.Id); })) return;
                    var row = (k === 0 && I(r.OverHeadAccountId) === 0) ? r : BLANK.oh();
                    if (row !== r) G.oh.splice(i + 1 + k, 0, row);
                    row.OverHeadAccountId = I(x.Id); row.OverHeadAccount = x.AccountTitle; k++;
                });
                render('oh');
            }, true, [{ h: 'AccountTitle', k: 'AccountTitle' }, { h: 'AccountCode', k: 'AccountCode' }]);
        } else if (kind === 'wo') {                                       // grdOutPut_KeyDown F1:3427
            if (sel('CmbCastingType') === 0) { alertMsg('Casting type Field Required'); return; }
            W3B.getJson(api + '/work-orders' + W3B.qs({ castingTypeId: sel('CmbCastingType'), recId: recId })).then(function (rows) {
                var cols = woCols();
                pick('Work Order', rows, function (x) { setOutputFromWo(r, x); render('out'); }, false, cols);
            }).catch(W3B.fail);
        }
    }
    function woCols() {
        return [{ h: 'WorkOrderNo', k: 'WorkOrderNo' }, { h: 'ItemName', k: 'ItemName' }, { h: 'ItemCode', k: 'ItemCode' }, { h: 'ItemWeight', k: 'ItemWeight' }, { h: 'ItemUom', k: 'ItemUom' },
            { h: 'ModelDescription', k: 'ModelDescription' }, { h: 'CastingType', k: 'CastingType' }, { h: 'Qty', k: 'Qty' }, { h: 'UsedQtyInProduction', k: 'UsedQtyInProduction' }, { h: 'BalQty', k: 'BalQty' }];
    }
    function setOutputFromWo(r, x) {
        r.WorkOrderId = I(x.WorkOrderId); r.workOrderItemDetailId = I(x.workOrderItemDetailId); r.WorkOrderNo = x.WorkOrderNo;
        r.ItemId = I(x.ItemId); r.ItemName = x.ItemName; r.PackUomId = I(x.UomId); r.PackUom = x.ItemUom; r.PackEquivalent = n(x.ItemEquivalent);
        r.ModalId = I(x.ItemVariantId); r.ModalDescription = x.ModelDescription; r.BalQty = n(x.BalQty); r.WeightPerQty = n(x.ItemWeight);
        r.NetWeight = n(x.BalQty); r.CastingWeight = n(x.BalQty) * n(x.ItemWeight);
        r.RateUomId = I(x.UomId); r.RateUom = x.ItemUom; r.RateEquivalent = n(x.ItemEquivalent); calcAmount(r);
    }

    // ----------------------------------------------------------------------------- Calculation:2421
    function sum(a, f) { return a.reduce(function (s, x) { return s + n(x[f]); }, 0); }
    function calculation() {
        var iq = sum(G.inp, 'NetWeight'), ia = sum(G.inp, 'Amount'), oq = sum(G.oh, 'Qty'), oa = sum(G.oh, 'Amount'), sq = sum(G.scr, 'NetWeight'), sa = sum(G.scr, 'Amount'),
            uq = sum(G.out, 'NetWeight'), ua = sum(G.out, 'Amount');
        function pct(x) { return ia > 0 ? x / ia * 100 : 0; }
        var exp = ia + oa - sa;
        var rows = [['Total Inputs', iq, ia, fmtPct(100)], ['Total OverHead', oq, oa, fmtPct(pct(oa))], ['Total Scrap', sq, sa, fmtPct(pct(sa))],
            ['Total Output', uq, ua, fmtPct(pct(ua))], ['Total Expected Output', uq, exp, fmtPct(pct(exp))]];
        $('grdSummary').innerHTML = '<thead><tr><th>Description</th><th>Qty</th><th>Amount</th><th>%</th></tr></thead><tbody>' + rows.map(function (r) {
            return '<tr><td>' + W3B.esc(r[0]) + '</td><td class="num">' + fmt(r[1]) + '</td><td class="num">' + fmt(r[2]) + '</td><td class="num">' + W3B.esc(r[3]) + '</td></tr>';
        }).join('') + '</tbody>';
        var gl = sa + ua - ia, glp = ia > 0 ? gl / ia * 100 : 0, color = gl >= 0 ? 'green' : 'red';
        $('labelGainLossCaption').textContent = gl >= 0 ? 'Gain' : 'Loss'; $('labelGainLossPercent').textContent = glp.toLocaleString('en-US', { minimumFractionDigits: 3, maximumFractionDigits: 3 }) + ' %';
        $('labelGainLossCaption').style.color = color; $('labelGainLossPercent').style.color = color;
    }

    // ----------------------------------------------------------------------------- Generate:2501
    function generate() {
        if (!G.out.length) { alertMsg('Output Grid Record not Found'); return; }
        if (G.out.length === 1 && I(G.out[0].WorkOrderId) === 0 && I(G.out[0].ItemId) === 0) { alertMsg('Output Grid Record not Filled Completely'); return; }
        var net = sum(G.inp, 'Amount') + sum(G.oh, 'Amount') - sum(G.scr, 'Amount'), qty = sum(G.out, 'NetWeight');
        G.out.forEach(function (r) {
            r.Amount = qty > 0 ? net / qty * n(r.NetWeight) : 0;
            r.Rate = n(r.NetWeight) > 0 ? r.Amount / n(r.NetWeight) : 0;
        });
        render('out');
    }

    // ----------------------------------------------------------------------------- Load Input From BOM:1021
    function loadFromBom() {
        var bom = sel('CmbBillOfMaterial');
        if (bom === 0) { alertMsg('BillOfMaterial Field is Required'); W3B.focus('CmbBillOfMaterial'); return; }
        if (G.inp.length > 0 && !W3B.ask('Input Grid already have Records.Do you want To Refresh Data?')) return;
        var wh = 0; for (var k = 0; k < L.wh.length; k++) if (I(L.wh[k].Id) > 0) { wh = I(L.wh[k].Id); break; }
        W3B.getJson(api + '/bom-inputs?id=' + bom).then(function (rows) {
            if (!rows.length) return;                                     // desktop: grdInput.ClearStructure() (gap)
            rows.forEach(function (x) {
                if (G.inp.some(function (r) { return I(r.BomInputDetailId) === I(x.BomInputDetailId); })) return;
                x.WarehouseId = wh; G.inp.push(x);
            });
            render('inp');
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- Load Output:3943
    var woRows = [], woChk = {};
    function loadOutput() {
        if (sel('CmbCastingType') === 0) { alertMsg('Casting type Field Required'); return; }
        W3B.getJson(api + '/work-orders' + W3B.qs({ castingTypeId: sel('CmbCastingType'), recId: recId })).then(function (rows) {
            woRows = rows; woChk = {}; woRender(); $('woModal').classList.add('is-open');
        }).catch(W3B.fail);
    }
    function woRender() {
        var cols = [{ h: '', html: true, w: 26, f: function (r, i) { return '<input type="checkbox" data-wo="' + i + '"' + (woChk[i] ? ' checked' : '') + '>'; } }].concat(woCols().map(function (c) {
            return { h: c.h, k: c.k, cls: ['ItemWeight', 'Qty', 'UsedQtyInProduction', 'BalQty'].indexOf(c.k) >= 0 ? 'num' : '' };
        }));
        $('woGrid').innerHTML = W3B.tableHtml(cols, woRows, { empty: 'No records' });
    }
    $('woGrid').addEventListener('change', function (e) { var t = e.target; if (t.dataset && t.dataset.wo !== undefined) woChk[+t.dataset.wo] = t.checked; });
    function woClose() { $('woModal').classList.remove('is-open'); }
    function woPick() {
        Object.keys(woChk).filter(function (k) { return woChk[k]; }).forEach(function (k) {
            var x = woRows[+k];
            if (G.out.some(function (r) { return I(r.workOrderItemDetailId) === I(x.workOrderItemDetailId); })) return;
            var r = { Id: 0, WarehouseId: cfg.whDefault, ProcessStatusId: cfg.stageId, ProcessStatus: cfg.stageName, Rate: 0, Amount: 0 };
            setOutputFromWo(r, x); r.Rate = 0; r.Amount = 0; G.out.push(r);
        });
        woClose(); render('out');
    }

    // ----------------------------------------------------------------------------- toolbar
    function applyRights() {
        $('btnSave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights['delete'];
        $('btnPrint').disabled = !rights.print; $('ChkBox').disabled = !rights.print; $('ChkBox').checked = !!rights.print;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update'); if (!rights['delete']) m.push('Delete'); if (!rights.print) m.push('Print');
        var nt = $('rightsNote');
        nt.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        nt.classList.toggle('is-hidden', !m.length);
    }
    function mode() { var edit = recId > 0; W3B.show('btnSave', !edit); W3B.show('btnUpdate', edit); W3B.show('btnDelete', edit); }
    function timeChanged() {                                              // TotalTime = "{d} Days And {hh}:{mm}"
        var a = v('InTime'), b = v('OutTime');
        if (!a || !b) { $('txtTotalTime').value = ''; return; }
        var ms = new Date(b) - new Date(a); if (isNaN(ms)) { $('txtTotalTime').value = ''; return; }
        var neg = ms < 0; ms = Math.abs(ms);
        var d = Math.floor(ms / 86400000), h = Math.floor(ms / 3600000) % 24, mi = Math.floor(ms / 60000) % 60;
        $('txtTotalTime').value = (neg ? '-' : '') + d + ' Days And ' + String(h).padStart(2, '0') + ':' + String(mi).padStart(2, '0');
    }
    function reset() {                                                    // Reset:2296
        recId = 0; isApproved = false; removedOut = []; removedIn = [];
        G.inp = [BLANK.inp()]; G.scr = [BLANK.scr()]; G.oh = [BLANK.oh()]; G.out = [];
        $('txtremarks').value = ''; $('txtProductionNo').value = ''; $('txtTotalTime').value = ''; $('ChkSettlement').checked = false;
        ['CmbJobLot', 'CmbHeatCode', 'CmbCastingType', 'CmbBillOfMaterial'].forEach(function (id) { W3B.clearCombo(id); });
        bindBom(); mode(); render();
        return W3B.getJson(api + '/doc-no').then(function (d) { $('txtdocno').value = d.docNo; }).catch(W3B.fail);
    }
    function newClick() { reset(); }
    function refresh() { W3B.getJson(api + '/refresh').then(function (d) { applyLookups(d); render(); }).catch(W3B.fail); }
    function formValidation() {                                           // FormValidation:980
        var d = v('txtdocno').trim();
        if (d === '' || d === '0') { alertMsg('DocNo Field is Required'); return false; }
        var pn = v('txtProductionNo').trim();
        if (pn === '' || pn === '0') { alertMsg('Production No Field is Required'); W3B.focus('txtProductionNo'); return false; }
        if (sel('CmbJobLot') === 0) { alertMsg('Batch Code Field is Required'); W3B.focus('CmbJobLot'); return false; }
        if (sel('CmbHeatCode') === 0) { alertMsg('Heat Code Field is Required'); W3B.focus('CmbHeatCode'); return false; }
        var a = v('InTime'), b = v('OutTime');
        if (!a || !b || new Date(a) >= new Date(b)) { alertMsg('In time cannot be equal to or greater than out time.'); W3B.focus('InTime'); return false; }
        return true;
    }
    var saving = false;
    function save() {                                                     // Insert:1820
        if (saving) return;
        if (!formValidation()) return;
        if (!G.inp.length) { alertMsg('Grid Record Not Found,Input Grid Should have a row'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            id: recId, docDate: v('DocDate'), docNo: v('txtdocno'), productionNo: v('txtProductionNo'), jobLotId: sel('CmbJobLot'), batchCode: W3B.selText('CmbJobLot'),
            heatCode: sel('CmbHeatCode'), inTime: v('InTime'), outTime: v('OutTime'), castingTypeId: sel('CmbCastingType'), castingTypeText: W3B.selText('CmbCastingType'),
            bomHeaderId: sel('CmbBillOfMaterial'), remarks: v('txtremarks'), settled: $('ChkSettlement').checked,
            output: G.out, scrap: G.scr, input: G.inp, overhead: G.oh, removedOutput: removedOut, removedInput: removedIn
        };
        saving = true;
        W3B.postJson(api + '/save', body).then(function (d) {
            saving = false; alertMsg(d.message);
            var id = d.id, doPrint = $('ChkBox').checked;
            return reset().then(function () { if (doPrint) printSlip(id); });
        }).catch(function (e) { saving = false; W3B.fail(e); });
    }
    function del() {                                                      // BtnDelete_Click:2259
        if (recId <= 0) { alertMsg('No record found to Delete'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { alertMsg(d.message); return reset(); }).catch(W3B.fail);
    }
    function printSlip(id) {                                              // CommonServices.ProductionSlipEngr_1670(id) -> 1670_ProductionEngr_Slip.rpt
        if (!id) { alertMsg('No Data Found'); return; }
        window.open('/reports/print/1670-production-engr-slip?branchesId=' + encodeURIComponent(look.branchId || '') + '&id=' + encodeURIComponent(id), '_blank');
    }
    function print() { printSlip(recId); }
    function printVoucherFor(id) {                                        // VoucherReprot:3176
        if (!id) return;
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        W3B.getJson(api + '/voucher-head?id=' + id).then(function (d) {
            var vh = I(d.voucherHeadId);
            if (!vh) { if (win) window.CrystalPrint.release(win); alertMsg('VoucherId Not Found'); return; }
            return window.CrystalPrint.open('acc-118', { id: vh, documentTypeId: 0 }, null, win);
        }).catch(function (e) { if (win) window.CrystalPrint.release(win); W3B.fail(e); });
    }
    function printVoucher() { printVoucherFor(recId); }

    // ----------------------------------------------------------------------------- ReadById:2168
    function load(id) {
        return reset().then(function () { return W3B.getJson(api + '/read/' + id); }).then(function (d) {
            recId = d.id; isApproved = !!d.isApproved; mode(); tab('form');
            $('txtdocno').value = d.docNo; $('DocDate').value = d.docDate; $('txtProductionNo').value = d.productionNo;
            W3B.setCombo('CmbJobLot', d.jobLotId); W3B.setCombo('CmbHeatCode', d.heatCode);
            $('InTime').value = String(d.inTime || '').slice(0, 16); $('OutTime').value = String(d.outTime || '').slice(0, 16); timeChanged();
            W3B.setCombo('CmbCastingType', d.castingTypeId); bindBom(); W3B.setCombo('CmbBillOfMaterial', d.bomHeaderId);
            $('txtremarks').value = d.remarks;
            G.out = d.output || []; G.scr = d.scrap || []; G.inp = d.input || []; G.oh = d.overhead || [];
            ensureBlanks(); render();
        }).catch(function (e) { recId = 0; mode(); W3B.fail(e); });
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
    }
    function dateType() {
        var t = I(v('cmbperemeter')), now = new Date(), today = W3B.iso(now);
        if (t === 1) $('fromdate').value = today;
        else if (t === 2) { var d = new Date(now); d.setDate(now.getDate() - 7); $('fromdate').value = W3B.iso(d); }
        else if (t === 3) { $('fromdate').value = W3B.iso(new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), 1))); $('ToDate').value = today; }
        else if (t === 4) { $('ToDate').value = now.getFullYear() + '-01-01'; $('fromdate').value = today; }
        else if (t === 5) { if (look.financialYearStart) $('fromdate').value = look.financialYearStart; }
    }
    function btnCell(act, i, cap) { return '<button type="button" class="rc-cellbtn" data-hact="' + act + '" data-hi="' + i + '">' + cap + '</button>'; }
    function history() {                                                  // HistoryGridFill:2615
        var q = { fromDocNo: I(v('txtDocNoFrom')), toDocNo: I(v('txtDocNoTo')) };
        if ($('chkFrom').checked) q.fromDate = v('fromdate');
        if ($('chkTo').checked) q.toDate = v('ToDate');
        W3B.getJson(api + '/history' + W3B.qs(q)).then(function (rows) {
            hist = rows; clearHistDetail();
            if (!rows.length) { $('grdhistory').innerHTML = ''; return; }
            $('grdhistory').innerHTML = W3B.tableHtml([
                { h: 'Edit', html: true, w: 40, f: function (r, i) { return btnCell('edit', i, 'Edit'); } },
                { h: 'Print', html: true, w: 40, f: function (r, i) { return btnCell('print', i, 'Print'); } },
                { h: 'Voucher', html: true, w: 50, f: function (r, i) { return btnCell('voucher', i, 'Voucher'); } },
                { h: 'DocDate', f: function (r) { return W3B.fmtDMY(r.DocDate); } }, { h: 'DocNo', k: 'DocNo' }, { h: 'Manual No', k: 'ProductionNo' }, { h: 'BatchCode', k: 'BatchCode' },
                { h: 'heatCode', k: 'heatCode' }, { h: 'InTime', f: function (r) { return W3B.fmtStamp(r.InTime); } }, { h: 'OutTime', f: function (r) { return W3B.fmtStamp(r.OutTime); } },
                { h: 'TotalTime', k: 'TotalTime' }, { h: 'CastingType', k: 'CastingType' }, { h: 'BomNo', k: 'BomNo' }, { h: 'SettlementStatus', k: 'SettlementStatus' },
                { h: 'RemarksHeader', k: 'RemarksHeader' }, { h: 'EntryDate', f: function (r) { return W3B.fmtStamp(r.EntryDate); } }, { h: 'EntryUser', k: 'EntryUser' },
                { h: 'ModifyDate', f: function (r) { return W3B.fmtStamp(r.ModifyDate); } }, { h: 'ModifyUser', k: 'ModifyUser' }, { h: 'NoOfAttachments', k: 'NoOfAttachments', cls: 'num' }
            ], rows, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function clearHistDetail() { ['hInput', 'hOutput', 'hScrap', 'hOverHead'].forEach(function (id) { $(id).innerHTML = ''; }); }
    function historyDetail(id) {                                          // BindAllDetailHistories:2876
        W3B.getJson(api + '/read/' + id).then(function (d) {
            var amt = function (k) { return { h: k, k: k, cls: 'num' }; };
            $('hInput').innerHTML = W3B.tableHtml([{ h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'PackUom' }, { h: 'Qty', k: 'NetWeight', cls: 'num' },
                amt('Rate'), { h: 'RateUom', k: 'RateUom' }, amt('Amount')], d.input || [], { empty: 'No rows' });
            $('hOutput').innerHTML = W3B.tableHtml([{ h: 'WorkOrderNo', k: 'WorkOrderNo' }, { h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'PackUom' },
                { h: 'ModalDescription', k: 'ModalDescription' }, { h: 'Production Stage', k: 'ProcessStatus' }, { h: 'Qty', k: 'NetWeight', cls: 'num' }, { h: 'WeightPerQty', k: 'WeightPerQty', cls: 'num' },
                { h: 'CastingWeight', f: function (r) { return fmtPct(r.CastingWeight); }, cls: 'num' }, amt('Rate'), { h: 'RateUom', k: 'RateUom' }, amt('Amount')], d.output || [], { empty: 'No rows' });
            $('hScrap').innerHTML = W3B.tableHtml([{ h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'PackUom' }, { h: 'Weight', k: 'NetWeight', cls: 'num' },
                amt('Rate'), { h: 'RateUom', k: 'RateUom' }, amt('Amount')], d.scrap || [], { empty: 'No rows' });
            $('hOverHead').innerHTML = W3B.tableHtml([{ h: 'AccountTitle', k: 'OverHeadAccount' }, { h: 'Percentage', k: 'PercentOfTotalInput', cls: 'num' }, { h: 'Qty', k: 'Qty', cls: 'num' },
                amt('Rate'), amt('Amount'), { h: 'ohRemarks', k: 'OhRemarks' }], d.overhead || [], { empty: 'No rows' });
        }).catch(W3B.fail);
    }
    function newHistory() {                                               // new-history defaults
        var now = new Date(), f = new Date(now); f.setDate(now.getDate() - 3);
        $('fromdate').value = W3B.iso(f); $('ToDate').value = W3B.iso(now); $('txtDocNoFrom').value = ''; $('txtDocNoTo').value = '';
        $('grdhistory').innerHTML = ''; clearHistDetail(); hist = [];
    }

    window.Ep = { newClick: newClick, refresh: refresh, save: save, del: del, print: print, voucher: printVoucher, generate: generate, tab: tab, history: history, newHistory: newHistory,
        dateType: dateType, castingLeave: castingLeave, timeChanged: timeChanged, loadFromBom: loadFromBom, loadOutput: loadOutput, woClose: woClose, woPick: woPick,
        pickClose: pickClose, pickOk: pickOk };

    W3B.keys({ n: newClick, s: save, u: function () { if (recId > 0) save(); }, r: refresh, p: print });

    // ----------------------------------------------------------------------------- boot (InitializeComponentMethod:454)
    W3B.getJson(api + '/lookups').then(function (d) {
        look = d; rights = d.rights || {};
        applyRights(); applyLookups(d);
        W3B.fill('cmbperemeter', d.dateTypes || [], false);
        $('DocDate').value = W3B.today();
        var now = new Date(), f = new Date(now); f.setDate(now.getDate() - 3);
        $('fromdate').value = W3B.iso(f); $('ToDate').value = W3B.iso(now);
        $('txtdocno').value = d.docNo;
        G.inp = [BLANK.inp()]; G.scr = [BLANK.scr()]; G.oh = [BLANK.oh()]; G.out = [];
        mode(); render();
    }).catch(W3B.fail);
}());
