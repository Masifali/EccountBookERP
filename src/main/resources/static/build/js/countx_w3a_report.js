/* W3A - Production report screens (307 GrnsPendingOrUsedinProduction, 311 frmProductionPendingPurchaseInvoice,
   820 frmConcreteProductionReport, 591 ProductionRegisterSalt). One script, one config per screen; API:
   /production/reports/w3a-api/{key}/init | combos | show. Line refs ":NNN" are the desktop .cs files. */
(function () {
    'use strict';
    var root = document.getElementById('w3Root');
    if (!root) return;
    var KEY = root.getAttribute('data-key');
    var API = '/production/reports/w3a-api/' + KEY;
    function $(id) { return document.getElementById(id); }
    function esc(s) { return String(s == null ? '' : s).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }
    function ci(r, k) { if (!r) return undefined; if (k in r) return r[k]; var l = k.toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return undefined; }
    function msg(t) { var m = $('w3Msg'); m.textContent = t || ''; m.style.display = t ? 'block' : 'none'; if (t) window.scrollTo(0, 0); }
    function iso(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dmy(s) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s || ''); return m ? m[3] + '-' + MON[+m[2] - 1] + '-' + m[1].slice(2) : (s || ''); }
    function api(url, opt) {
        return fetch(url, Object.assign({ credentials: 'same-origin' }, opt || {})).then(function (r) {
            return r.text().then(function (t) {
                var j = null; try { j = t ? JSON.parse(t) : null; } catch (e) { }
                if (!r.ok) throw new Error((j && j.message) || t || ('Request failed (' + r.status + ')'));
                return j;
            });
        });
    }
    function post(url, body) {
        var h = { 'Content-Type': 'application/json' }, t = document.querySelector('meta[name="_csrf"]'), hn = document.querySelector('meta[name="_csrf_header"]');
        if (t && hn) h[hn.getAttribute('content')] = t.getAttribute('content');
        return api(url, { method: 'POST', headers: h, body: JSON.stringify(body) });
    }

    /* number formats (Janus FormatString) as [minFraction, maxFraction] */
    var F = { n0: [0, 0], n2: [0, 2], n3: [0, 3], n4: [0, 4], amt: [0, 2], rate: [0, 4] };
    function fmt(v, f) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v); if (isNaN(n)) return esc(v);
        var p = F[f] || F.n2;
        return n.toLocaleString('en-US', { minimumFractionDigits: p[0], maximumFractionDigits: p[1] });
    }

    /* screen configs: columns exactly as the desktop grid (hidden = h, date = d, format = f, total = s) */
    var CFG = {
        grns: {   /* GrnsPendingOrUsedinProduction.cs :118-141 dtGrid, :441 GridSetting */
            printBtn: '663-Register',
            grids: [{ id: 'g1', cols: [
                { k: 'BranchName', c: 'BranchName', w: 150 }, { k: 'GrnStatus', c: 'GrnStatus', w: 120 }, { k: 'Id', h: 1 },
                { k: 'DocDate', c: 'DocDate', w: 80, d: 1 }, { k: 'DocNo', c: 'DocNo', w: 100 }, { k: 'PartyName', c: 'PartyName', w: 200 },
                { k: 'ItemName', c: 'ItemName', w: 200 }, { k: 'CropYear', c: 'CropYear', w: 80 }, { k: 'JobLot', c: 'JobLot', w: 80 },
                { k: 'PackType', c: 'PackType', w: 80 }, { k: 'PackUom', c: 'PackUom', w: 70 }, { k: 'GatePassNo', c: 'GatePassNo', w: 50 },
                { k: 'BiltyNo', c: 'BiltyNo', w: 80 }, { k: 'VehicleNo', c: 'VehicleNo', w: 90 }, { k: 'WareHouse', c: 'WareHouse', w: 150 },
                { k: 'CityName', c: 'CityName', w: 150 },
                { k: 'FreightAmount', c: 'FreightAmount', w: 90, f: 'n0', s: 'sum' }, { k: 'GrnQty', c: 'GrnQty', w: 80, f: 'n0', s: 'sum' },
                { k: 'OutQty', c: 'OutQty', w: 80, f: 'n0', s: 'sum' }, { k: 'BalQty', c: 'BalQty', w: 80, f: 'n0', s: 'sum' },
                { k: 'GrnWeight', c: 'GrnWeight', w: 80, f: 'n2', s: 'sum' }, { k: 'OutWeight', c: 'OutWeight', w: 80, f: 'n2', s: 'sum' },
                { k: 'BalWeight', c: 'BalWeight', w: 80, f: 'n2', s: 'sum' },
                { k: '_print', c: 'Print', w: 50, btn: 'Print' }] }]
        },
        pendinginv: {   /* frmProductionPendingPurchaseInvoice.cs :258-300 dt, :326 grdSettings */
            printBtn: 'Print',
            grids: [{ id: 'g1', cols: [
                { k: 'Id', h: 1 }, { k: 'BranchName', c: 'BranchName', w: 230 }, { k: 'DocNo', c: 'DocNo', w: 40 }, { k: 'DocDate', c: 'DocDate', w: 70, d: 1 },
                { k: 'SupplierName', c: 'SupplierName', w: 220 }, { k: 'ManualBillNo', c: 'ManualBillNo', w: 60 }, { k: 'WareHouse', c: 'WareHouse', w: 150 },
                { k: 'ItemName', c: 'ItemName', w: 220 }, { k: 'CropYear', c: 'CropYear', w: 70 }, { k: 'JobLot', c: 'JobLot', w: 70 },
                { k: 'UOM', c: 'UOM', w: 40 }, { k: 'PackingType', c: 'PackingType', w: 90 }, { k: 'RateUOM', c: 'RateUOM', w: 50 },
                { k: 'ItemQty', c: 'ItemQty', w: 80, f: 'n3', s: 'sum' }, { k: 'NetBillWeight', c: 'NetBillWeight', w: 100, f: 'n3', s: 'sum' },
                { k: 'ItemRate', c: 'ItemRate', w: 90, f: 'n2' }, { k: 'ItemAmount', c: 'ItemAmount', w: 100, f: 'amt', s: 'sum' }] }]
        },
        concrete: {   /* frmConcreteProductionReport.cs :219-340 + HistoryGridSetting/grdHistoryInputSetting/grdHistoryOutputSetting */
            printBtn: '1850_01-Register',
            grids: [
                { id: 'g1', title: 'Production', cols: [
                    { k: 'ProductionTypeId', h: 1 }, { k: 'ProductionType', c: 'ProductionType' }, { k: 'ItemId', h: 1 }, { k: 'ItemName', c: 'ItemName' }, { k: 'ItemCode', c: 'ItemCode' },
                    { k: 'ItemUomId', h: 1 }, { k: 'ItemUom', c: 'ItemUom' }, { k: 'ItemQty', c: 'ItemQty', f: 'n4', s: 'sum' },
                    { k: 'TotalInputAmount', c: 'TotalInputAmount', f: 'amt', s: 'sum' }, { k: 'TotalAutoRateOutputAmount', c: 'TotalAutoRateOutputAmount', f: 'amt', s: 'sum' },
                    { k: 'TotalManualRateOutputAmount', c: 'TotalManualRateOutputAmount', f: 'amt', s: 'sum' }, { k: 'TotalOverHeadAmount', c: 'TotalOverHeadAmount', f: 'amt', s: 'sum' },
                    { k: 'TotalWagesAmount', c: 'TotalWagesAmount', f: 'amt', s: 'sum' }, { k: 'NetFinishAmount', c: 'NetFinishAmount', f: 'amt', s: 'sum' }] },
                { id: 'g2', title: 'Input', half: 1, cols: [
                    { k: 'ItemId', h: 1 }, { k: 'ItemName', c: 'ItemName' }, { k: 'ItemCode', c: 'ItemCode' }, { k: 'ItemUomId', h: 1 }, { k: 'ItemUom', c: 'ItemUom' },
                    { k: 'ItemQty', c: 'ItemQty', f: 'n4', s: 'sum' }, { k: 'NetWeight', h: 1, f: 'n4', s: 'sum' }, { k: 'AvgRate', c: 'AvgRate', f: 'rate', s: 'avg' },
                    { k: 'Amount', c: 'Amount', f: 'amt', s: 'sum' }] },
                { id: 'g3', title: 'Output', half: 1, group: 'ProductionType', cols: [
                    { k: 'ProductionTypeId', h: 1 }, { k: 'ProductionType', h: 1 }, { k: 'ItemId', h: 1 }, { k: 'ItemName', c: 'ItemName' }, { k: 'ItemCode', c: 'ItemCode' },
                    { k: 'ItemAttributeVarientId', h: 1 }, { k: 'Variant', c: 'Variant' }, { k: 'ItemQty', c: 'ItemQty', f: 'n4', s: 'sum' },
                    { k: 'VariantQty', c: 'VariantQty', f: 'n4', s: 'sum' }, { k: 'NetWeight', h: 1, f: 'n4', s: 'sum' },
                    { k: 'Rate/VariantQty', c: 'Rate/VariantQty', f: 'n4', s: 'sum' }, { k: 'AvgRate', c: 'AvgRate', f: 'rate', s: 'sum' }, { k: 'Amount', c: 'Amount', f: 'amt', s: 'sum' }] }
            ]
        },
        salt: {   /* ProductionRegisterSalt.cs :246-264 dt, :289 grdsetting */
            printBtn: '1818-Print',
            grids: [{ id: 'g1', cols: [
                { k: 'Id', h: 1 }, { k: 'DocumentTypeId', h: 1 }, { k: 'DocNo', c: 'DocNo' }, { k: 'DocDate', c: 'DocDate', d: 1 }, { k: 'ProductionNo', c: 'ProductionNo' },
                { k: 'ItemName', c: 'ItemName' }, { k: 'PackingType', c: 'PackingType' }, { k: 'JobLotDescription', c: 'JobLotDescription' },
                { k: 'ItemQty', c: 'ItemQty', f: 'n2', s: 'sum' }, { k: 'PackUom', c: 'PackUom' }, { k: 'NetWeight', c: 'NetWeight', f: 'n3', s: 'sum' },
                { k: 'ItemRate', c: 'ItemRate', f: 'n3', s: 'avg' }, { k: 'RateUom', c: 'RateUom' }, { k: 'Amount', c: 'Amount', f: 'n3', s: 'avg' }] }]
        }
    };
    var C = CFG[KEY];
    var st = { init: null, rows: {}, filt: {}, sel: {}, fyStart: null, last: null, lastData: null };

    /* ---- widgets ---- */
    function fld(label, inner) { return '<div class="w3-f"><label>' + esc(label) + '</label>' + inner + '</div>'; }
    function dateField(id, label, val) { return fld(label, '<input type="date" id="' + id + '" value="' + esc(val || '') + '">'); }
    /* searchable combo = input + datalist; the Id is resolved from the exact name (UltraCombo DisplayMember) */
    var lists = {};
    function comboField(id, label, rows, nameKey) {
        lists[id] = (rows || []).map(function (r) { var n = ci(r, nameKey || 'Name'); return { id: ci(r, 'Id'), name: String(n == null ? '' : n) }; });
        return fld(label, '<input class="cmb" id="' + id + '" list="' + id + 'L" autocomplete="off"><datalist id="' + id + 'L">' +
            lists[id].map(function (o) { return '<option value="' + esc(o.name) + '">'; }).join('') + '</datalist>');
    }
    function comboVal(id) {
        var el = $(id); if (!el) return 0; var t = el.value.trim(); if (!t) return 0;
        var l = lists[id] || []; for (var i = 0; i < l.length; i++) if (l[i].name === t) return Number(l[i].id) || 0;
        return 0;
    }
    function comboText(id) { var el = $(id); return el ? el.value.trim() : ''; }
    function selectField(id, label, opts, selected) {
        return fld(label, '<select id="' + id + '">' + opts.map(function (o) { return '<option value="' + o[0] + '"' + (String(o[0]) === String(selected) ? ' selected' : '') + '>' + esc(o[1]) + '</option>'; }).join('') + '</select>');
    }
    var DATE_TYPES = [[0, ''], [1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];   /* CommonServices.DateType */
    function applyDateType(v, fromId, toId) {
        v = Number(v); var t = new Date(), f = $(fromId), to = $(toId);
        if (v === 1) f.value = iso(t);
        else if (v === 2) { var w = new Date(t); w.setDate(w.getDate() - 7); f.value = iso(w); }
        else if (v === 3) { f.value = iso(new Date(t.getFullYear(), t.getMonth(), 1)); to.value = iso(t); }
        else if (v === 4) { f.value = iso(new Date(t.getFullYear(), 0, 1)); to.value = iso(t); }
        else if (v === 5 && st.fyStart) f.value = st.fyStart;
    }
    /* UltraCombo checked list (branches), ListSeparator "," */
    function multiField(id, label, rows, idKey, nameKey, defaultId) {
        var html = '<label class="all"><input type="checkbox" class="all"> </label>' + (rows || []).map(function (r) {
            var rid = ci(r, idKey);
            return '<label><input type="checkbox" data-id="' + esc(rid) + '"' + (Number(rid) === Number(defaultId) ? ' checked' : '') + '> ' + esc(ci(r, nameKey)) + '</label>';
        }).join('');
        return fld(label, '<div class="w3-multi" id="' + id + '"><button type="button" class="w3-multi-btn"><span id="' + id + 'T"></span><i class="fa fa-caret-down"></i></button><div class="w3-multi-list" id="' + id + 'L">' + html + '</div></div>');
    }
    function multiIds(id) { var o = []; $(id + 'L').querySelectorAll('input[data-id]:checked').forEach(function (i) { o.push(Number(i.getAttribute('data-id'))); }); return o; }
    function multiSync(id) {
        var names = []; $(id + 'L').querySelectorAll('input[data-id]:checked').forEach(function (i) { names.push(i.parentNode.textContent.trim()); });
        $(id + 'T').textContent = names.join(',');
        var boxes = $(id + 'L').querySelectorAll('input[data-id]'), all = $(id + 'L').querySelector('input.all');
        all.checked = boxes.length > 0 && names.length === boxes.length;
    }
    function wireMulti(id, onClose) {
        var box = $(id), list = $(id + 'L'), btn = box.querySelector('.w3-multi-btn');
        btn.addEventListener('click', function () { var open = list.style.display === 'block'; list.style.display = open ? 'none' : 'block'; if (open && onClose) onClose(); });
        document.addEventListener('click', function (e) { if (list.style.display === 'block' && !box.contains(e.target)) { list.style.display = 'none'; if (onClose) onClose(); } });
        list.addEventListener('change', function (e) {
            if (e.target.classList.contains('all')) list.querySelectorAll('input[data-id]').forEach(function (i) { i.checked = e.target.checked; });
            multiSync(id);
        });
        multiSync(id);
    }

    /* ---- grid (Janus GridEX look: filter row, totals row, optional group by) ---- */
    function gridHtml(g) {
        return (g.title ? '<div class="w3-gridtitle">' + esc(g.title) + '</div>' : '') +
            '<div class="w3-grid-host' + (g.half ? ' half' : '') + '"><table class="w3-grid" id="' + g.id + '"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>' +
            '<div class="w3-nav" id="' + g.id + 'N"></div>';
    }
    function cellText(col, v) {
        if (col.d) return esc(dmy(v));
        if (col.f) return fmt(v, col.f);
        return esc(v == null ? '' : v);
    }
    function renderGrid(g) {
        var rows = st.rows[g.id] || [], fl = st.filt[g.id] = st.filt[g.id] || {}, cols = g.cols.filter(function (c) { return !c.h; });
        var tbl = $(g.id);
        var vis = rows.filter(function (r) {
            return cols.every(function (c) {
                var q = (fl[c.k] || '').toLowerCase(); if (!q || c.btn) return true;
                return String(cellText(c, r[c.k])).toLowerCase().indexOf(q) >= 0;   /* DefaultFilterRowComparison = Contains */
            });
        });
        tbl.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th style="min-width:' + (c.w || 80) + 'px">' + esc(c.c) + '</th>'; }).join('') + '</tr>' +
            '<tr class="flt">' + cols.map(function (c) { return '<td>' + (c.btn ? '' : '<input data-g="' + g.id + '" data-k="' + esc(c.k) + '" value="' + esc(fl[c.k] || '') + '">') + '</td>'; }).join('') + '</tr>';
        var html = '', lastGrp = null;
        vis.forEach(function (r) {
            if (g.group && r[g.group] !== lastGrp) { lastGrp = r[g.group]; html += '<tr class="grp"><td colspan="' + cols.length + '">' + esc(g.group) + ': ' + esc(lastGrp) + '</td></tr>'; }
            var ix = rows.indexOf(r);
            html += '<tr data-i="' + ix + '"' + (st.sel[g.id] === ix ? ' class="sel"' : '') + '>' + cols.map(function (c) {
                if (c.btn) return '<td><a data-btn="' + c.btn + '">' + c.btn + '</a></td>';
                return '<td class="' + (c.f ? 'n' : '') + '">' + cellText(c, r[c.k]) + '</td>';
            }).join('') + '</tr>';
        });
        tbl.querySelector('tbody').innerHTML = html;
        var any = cols.some(function (c) { return c.s; });
        tbl.querySelector('tfoot').innerHTML = any ? '<tr>' + cols.map(function (c, i) {
            if (!c.s) return '<td>' + (i === 0 ? 'Totals' : '') + '</td>';
            var sum = 0, n = 0; vis.forEach(function (r) { var v = Number(r[c.k]); if (!isNaN(v)) { sum += v; n++; } });
            return '<td>' + fmt(c.s === 'avg' ? (n ? sum / n : 0) : sum, c.f) + '</td>';
        }).join('') + '</tr>' : '';
        $(g.id + 'N').textContent = 'Records: ' + vis.length;
        tbl.querySelectorAll('input[data-g]').forEach(function (inp) {
            inp.addEventListener('input', function () {
                fl[inp.getAttribute('data-k')] = inp.value; var pos = inp.selectionStart; renderGrid(g);
                var again = tbl.querySelector('input[data-k="' + inp.getAttribute('data-k') + '"]'); if (again) { again.focus(); again.setSelectionRange(pos, pos); }
            });
        });
        tbl.querySelectorAll('tbody tr[data-i]').forEach(function (tr) {
            tr.addEventListener('click', function () { st.sel[g.id] = Number(tr.getAttribute('data-i')); tbl.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); });
        });
        tbl.querySelectorAll('a[data-btn]').forEach(function (a) {
            a.addEventListener('click', function () { var tr = a.closest('tr'); onRowButton(rows[Number(tr.getAttribute('data-i'))]); });
        });
    }
    function setRows(g, rows) { st.rows[g.id] = rows || []; st.sel[g.id] = -1; renderGrid(g); }
    function clearGrid(g) { st.rows[g.id] = []; renderGrid(g); }

    /* ---- screen building ---- */
    var HOST = '<div class="w3-f" id="w3Host" style="flex-direction:row;gap:6px 14px;flex-wrap:wrap;align-items:flex-end"></div>';
    function by(rows, a) { return rows.filter(function (r) { return String(ci(r, 'ActivityType')) === a; }); }
    /* 307 ComboFill :182 - Warehouse / JobLot / CropYear / Supplier_Customer / Items / PackingType (ZeroIndex => blank first) */
    function renderGrnCombos(rows) {
        $('w3Host').innerHTML = comboField('cmbPartyName', 'Party Name', by(rows, 'Supplier_Customer'), 'name') + comboField('cmbItemName', 'Item Name', by(rows, 'Items'), 'name') +
            comboField('cmbJobLot', 'Job Lot', by(rows, 'JobLot'), 'name') + comboField('cmbWarehouse', 'Warehouse', by(rows, 'Warehouse'), 'name') +
            comboField('cmbPackingType', 'Packing Type', by(rows, 'PackingType'), 'name') + comboField('cmbCropYear', 'Crop Year', by(rows, 'CropYear'), 'name') +
            selectField('cmbPendingUsed', 'Pending / Used', [[1, 'Pending'], [2, 'Used']], 1);   /* PendingUsedFill :407 */
    }
    /* 311 ComboFill :122 */
    function renderPendCombos(rows) {
        $('w3Host').innerHTML = comboField('CmbSupplier', 'Supplier Name', by(rows, 'Supplier_Customer'), 'name') + comboField('CmbItem', 'Item Name', by(rows, 'Items'), 'name') +
            comboField('cmbJobLot', 'JobLot', by(rows, 'JobLot'), 'name') + comboField('CmbWareHouse', 'WarehouseName', by(rows, 'Warehouse'), 'name');
    }
    /* cmbBranchName_Leave: text non-empty -> ComboFill by the ticked branches; empty -> clear the combos */
    function branchLeave() {
        var ids = multiIds('cmbBranchName');
        if (!ids.length) { ['cmbPartyName', 'cmbItemName', 'cmbJobLot', 'cmbWarehouse', 'cmbPackingType', 'cmbCropYear', 'CmbSupplier', 'CmbItem', 'CmbWareHouse'].forEach(function (i) { if ($(i)) $(i).value = ''; }); return; }
        post(API + '/combos', { branchIds: ids }).then(function (rows) {
            var pu = $('cmbPendingUsed') ? $('cmbPendingUsed').value : null;
            if (KEY === 'grns') renderGrnCombos(rows); else renderPendCombos(rows);
            if (pu !== null && $('cmbPendingUsed')) $('cmbPendingUsed').value = pu;
        }).catch(function (e) { msg(e.message); });
    }

    function build() {
        $('w3Strip').innerHTML = '<button type="button" class="w3-tool" id="btnReset"><i class="fa fa-plus-circle" style="color:#2e9b45"></i> <span>Reset</span></button>' +
            '<button type="button" class="w3-tool" id="btnRefresh"><i class="fa fa-repeat"></i> <span>Refresh</span></button>' +
            '<button type="button" class="w3-tool" id="btnPrint"><i class="fa fa-print"></i> <span>' + esc(C.printBtn) + '</span></button>' +
            '<button type="button" class="w3-tool" id="btnCsv"><i class="fa fa-file-excel-o"></i> <span>Excel (CSV)</span></button>';
        $('w3Grids').innerHTML = C.grids.map(gridHtml).join('');
        var d = st.init, today = iso(new Date());
        if (KEY === 'grns') {
            $('w3Filters').innerHTML = multiField('cmbBranchName', 'Branch Name', d.branches, 'BranchId', 'BranchName', d.userBranchId) +
                dateField('txtfromdate', 'From Date', today) + dateField('txttodate', 'To Date', today) + HOST;
            renderGrnCombos(d.combos);
            wireMulti('cmbBranchName', branchLeave);
        } else if (KEY === 'pendinginv') {
            $('w3Filters').innerHTML = selectField('cmbDateType', 'Parameters', DATE_TYPES, 3) +
                multiField('cmbBranchName', 'Branch Name', d.branches, 'BranchId', 'BranchName', d.userBranchId) +
                dateField('txtfromdate', 'From Date', today) + dateField('txttodate', 'To Date', today) + HOST;
            renderPendCombos(d.combos);
            wireMulti('cmbBranchName', branchLeave);
            applyDateType(3, 'txtfromdate', 'txttodate');
            $('cmbDateType').addEventListener('change', function () { applyDateType(this.value, 'txtfromdate', 'txttodate'); if (Number(this.value) > 0) show(); });   /* :cmbDateType_ValueChanged -> btnshow_Click */
        } else if (KEY === 'concrete') {
            $('w3Filters').innerHTML = selectField('cmbDateType', 'Parameters', DATE_TYPES, 3) +
                dateField('txtFromdateHistory', 'From Date', today) + dateField('txtToDateHistory', 'To Date', today) +
                fld('From Doc No', '<input class="sm" id="txtFromNoHistory" inputmode="numeric">') + fld('To Doc No', '<input class="sm" id="txtToDocNoHistory" inputmode="numeric">') +
                comboField('CmbProductionType', 'Production Type', d.productionTypes) + comboField('CmbPlant', 'Plant Name', d.plants);
            ['txtFromNoHistory', 'txtToDocNoHistory'].forEach(function (id) { $(id).addEventListener('input', function () { this.value = this.value.replace(/\D/g, ''); }); });   /* OnlytextNumberFunction */
            applyDateType(3, 'txtFromdateHistory', 'txtToDateHistory');
            $('cmbDateType').addEventListener('change', function () { applyDateType(this.value, 'txtFromdateHistory', 'txtToDateHistory'); });
        } else {
            $('w3Filters').innerHTML = selectField('cmbDateType', 'Parameters', DATE_TYPES, 3) +
                dateField('datFromDate', 'From Date', today) + dateField('datToDate', 'To Date', today) +
                comboField('cmbitemtype', 'Item Type', d.itemTypes) + comboField('cmbWarehouse', 'Warehouse', d.warehouses) +
                comboField('cmbitem', 'Item', d.items) + comboField('cmbjoblot', 'Job Lot', d.jobLots) +
                comboField('cmbprodno', 'Production No', d.productionNos) + comboField('cmbitemcat', 'Item Category', d.itemCategories);
            applyDateType(3, 'datFromDate', 'datToDate');
            $('cmbDateType').addEventListener('change', function () { applyDateType(this.value, 'datFromDate', 'datToDate'); });
        }
        var sh = document.createElement('div'); sh.className = 'w3-f'; sh.innerHTML = '<label>&nbsp;</label><button type="button" class="w3-btn" id="btnshow" title="Ctrl+S">Show</button>';
        $('w3Filters').appendChild(sh);
        C.grids.forEach(renderGrid);
        $('btnshow').addEventListener('click', show);
        $('btnReset').addEventListener('click', reset);
        $('btnRefresh').addEventListener('click', refresh);
        $('btnPrint').addEventListener('click', printIt);
        $('btnCsv').addEventListener('click', csv);
        if (KEY === 'pendinginv') show();   /* Datetypefill activates "This Month" -> ValueChanged -> btnshow_Click */
    }
    document.addEventListener('keydown', function (e) {
        if (!e.ctrlKey || !st.init) return;
        var k = (e.key || '').toLowerCase();
        if (k === 's') { e.preventDefault(); show(); } else if (k === 'p') { e.preventDefault(); printIt(); }
        else if (k === 'r') { e.preventDefault(); refresh(); } else if (k === 'n') { e.preventDefault(); reset(); }
    });

    function show() {
        msg('');
        var body;
        if (KEY === 'grns') {
            body = { branchIds: multiIds('cmbBranchName'), fromDate: $('txtfromdate').value, toDate: $('txttodate').value, partyId: comboVal('cmbPartyName'), itemId: comboVal('cmbItemName'),
                jobLotId: comboVal('cmbJobLot'), warehouseId: comboVal('cmbWarehouse'), packingTypeId: comboVal('cmbPackingType'), pendingUsed: Number($('cmbPendingUsed').value) || 0,
                cropYearId: comboVal('cmbCropYear'), cropYear: comboText('cmbCropYear') };
        } else if (KEY === 'pendinginv') {
            body = { branchIds: multiIds('cmbBranchName'), fromDate: $('txtfromdate').value, toDate: $('txttodate').value, supplierId: comboVal('CmbSupplier'), itemId: comboVal('CmbItem'),
                jobLotId: comboVal('cmbJobLot'), warehouseId: comboVal('CmbWareHouse') };
        } else if (KEY === 'concrete') {
            body = { fromDate: $('txtFromdateHistory').value, toDate: $('txtToDateHistory').value, fromDocNo: $('txtFromNoHistory').value.trim(), toDocNo: $('txtToDocNoHistory').value.trim(),
                productionTypeId: comboVal('CmbProductionType'), plantId: comboVal('CmbPlant') };
        } else {
            body = { fromDate: $('datFromDate').value, toDate: $('datToDate').value, warehouseId: comboVal('cmbWarehouse'), itemId: comboVal('cmbitem'), jobLotId: comboVal('cmbjoblot'),
                itemTypeId: comboVal('cmbitemtype'), itemCategoryId: comboVal('cmbitemcat'), productionNo: comboText('cmbprodno') };
        }
        post(API + '/show', body).then(function (res) {
            st.last = body; st.lastData = res;
            if (KEY === 'concrete') { setRows(C.grids[0], res.main); setRows(C.grids[1], res.input); setRows(C.grids[2], res.output); }
            else setRows(C.grids[0], res);
        }).catch(function (e) { msg(e.message); });
    }

    function reset() {
        msg('');
        if (KEY === 'grns') {   /* Reset :670 */
            $('txtfromdate').value = st.fyStart || iso(new Date()); $('txttodate').value = iso(new Date());
            ['cmbItemName', 'cmbJobLot', 'cmbPartyName', 'cmbWarehouse', 'cmbPackingType', 'cmbCropYear'].forEach(function (i) { $(i).value = ''; });
            $('cmbPendingUsed').selectedIndex = -1;
        } else if (KEY === 'pendinginv') {   /* Reset() */
            ['CmbItem', 'cmbJobLot', 'CmbSupplier', 'CmbWareHouse'].forEach(function (i) { $(i).value = ''; });
            $('cmbDateType').value = 3; applyDateType(3, 'txtfromdate', 'txttodate');
        } else if (KEY === 'concrete') {   /* btnNewHistory/btnHistoryRefresh_Click */
            $('cmbDateType').value = 0; $('CmbPlant').value = ''; $('CmbProductionType').value = '';
            C.grids.slice(1).forEach(clearGrid);
        } else {   /* Reset() :333 */
            ['cmbWarehouse', 'cmbitem', 'cmbjoblot', 'cmbitemtype'].forEach(function (i) { $(i).value = ''; });
        }
    }
    function refresh() {   /* 307 btnAdd=Reset; 311 ComboFill+Datetypefill; 820/591 AllCombobind */
        msg('');
        if (KEY === 'grns') { reset(); return; }
        api(API + '/init').then(function (d) {
            st.init = d; st.fyStart = d.fyStart || st.fyStart;
            if (KEY === 'pendinginv') { branchLeave(); $('cmbDateType').value = 3; applyDateType(3, 'txtfromdate', 'txttodate'); }
            else build();
        }).catch(function (e) { msg(e.message); });
    }

    /* ---- printing through the registered Jasper endpoints (print-rpt.js printRpt) ---- */
    function noData() { var d = st.lastData; if (!d) return true; if (KEY === 'concrete') return !(d.main && d.main.length); return !d.length; }
    function printIt() {
        msg('');
        if (noData()) { alert(KEY === 'salt' ? 'Record Not Found For Display' : 'Not Record Found For Display'); return; }
        var b = st.last || {}, ids = (b.branchIds || []).join(',');
        if (KEY === 'grns') {   /* btn663Register_Click :620 */
            window.printRpt('663-InvRptPurchaseOrderDetailRegisterRice.rpt', { actionId: b.pendingUsed, fromDate: b.fromDate, toDate: b.toDate, supplierCustomerId: b.partyId || '', warehouseId: b.warehouseId || '',
                itemId: b.itemId || '', jobLotId: b.jobLotId || '', packingTypeId: b.packingTypeId || '', cropYear: b.cropYearId > 0 ? b.cropYear : '', branchesIds: ids });
        } else if (KEY === 'pendinginv') {   /* PrintRegister */
            window.printRpt('603-ProductionPendingInvoice.rpt', { fromDate: b.fromDate, toDate: b.toDate, supplierCustomerId: b.supplierId || '', itemId: b.itemId || '', documentTypeId: 56,
                jobLotId: b.jobLotId || '', warehouseId: b.warehouseId || '', branchesIds: ids });
        } else if (KEY === 'salt') {   /* btnPrintCurrent_Click :492 */
            window.printRpt('1818-ProductionSalt_Register.rpt', { branchesId: st.init.userBranchId, fromDate: b.fromDate, toDate: b.toDate, warehouseId: b.warehouseId || '', jobLotId: b.jobLotId || '',
                itemId: b.itemId || '', itemTypeId: b.itemTypeId || '', itemCategoryId: b.itemCategoryId || '', productionNo: b.productionNo || '' });
        } else {   /* btnRegister_Click: 1850_01_Production_Report.rpt + 2 sub reports - no registered print action yet */
            alert('1850_01_Production_Report.rpt (with the Production_Input / Production_Output sub reports) has no registered print action in the web app yet. Use Excel (CSV).');
        }
    }
    function onRowButton(row) {   /* 307 DataGridHistory_ColumnButtonClick :655 -> CommonServices.GrnSlipWithSubReports(Id) -> 211 slip */
        if (KEY === 'grns' && row) window.printRpt('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: row.Id });
    }

    function csv() {
        var out = [];
        C.grids.forEach(function (g) {
            var rows = st.rows[g.id] || [], cols = g.cols.filter(function (c) { return !c.h && !c.btn; });
            if (!rows.length) return;
            if (g.title) out.push('"' + g.title + '"');
            out.push(cols.map(function (c) { return '"' + c.c + '"'; }).join(','));
            rows.forEach(function (r) { out.push(cols.map(function (c) { var v = r[c.k]; return '"' + String(v == null ? '' : (c.d ? dmy(v) : v)).replace(/"/g, '""') + '"'; }).join(',')); });
            out.push('');
        });
        if (!out.length) { alert('Not Record Found For Display'); return; }
        var a = document.createElement('a'); a.href = URL.createObjectURL(new Blob(['﻿' + out.join('\r\n')], { type: 'text/csv;charset=utf-8' }));
        a.download = KEY + '_report.csv'; document.body.appendChild(a); a.click(); a.remove();
    }

    api(API + '/init').then(function (d) { st.init = d; st.fyStart = d.fyStart; build(); }).catch(function (e) { msg(e.message); });
})();
