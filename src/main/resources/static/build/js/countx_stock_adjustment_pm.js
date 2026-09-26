/* ============================================================================================
 * Stock Adjustment For PM - screen 492, module 54, DocumentTypeId 215.
 * Desktop: Architecture.WinApp.StoreManagement.StockAdjustmentForPM.
 *
 *   Load :233   EntryTypeBind :330   CmbEntryType_Leave :428   CmbItemName_Leave :453
 *   BindWarehouseDropdown :470   RackBindFromGlobalRacksByItemId :504   CmbRackName_Leave :554
 *   Reset :603   ResetDetail :649   FormValidationDetail :719   grd_ColumnButtonClick :772   btnplus :815
 *   btnUpdateDetail :835   grd_DoubleClick :876   Insert :933   ReadById :1060   gridhistoryfill :1142
 *   grdhistory_ColumnButtonClick :1275   CalculateAmount :1502   BindAvgRateForLossCase :1550
 *   AvailableStock :1614   AvgRateUpdateOnDocDateChange :1992
 * The server repeats every validation and the amounts; nothing here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/stock-adjustment';
    var L = null, R = {};
    var table = [], RecId = 0, VoucherHeadId = 0, updateDetailIndex = -1, historyRows = [], currentTab = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function fmt(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 3 : d }); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }
    function dt(v) { if (!v) return ''; var d = new Date(String(v).replace(' ', 'T')); if (isNaN(d.getTime())) return String(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return ddmmm(iso(d)) + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap; }
    /* double.ToString() - up to 15 significant digits, no grouping. */
    function clr(n) { n = num(n); return String(parseFloat(n.toPrecision(15))); }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
        var p; try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function http(method, url, body) {
        var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
                return b;
            });
        });
    }
    function getJson(url) { return http('GET', url); }
    function fill(id, rows, v, t) {
        var sel = $id(id); if (!sel) return; var keep = sel.value;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function has(id, v) { var s = $id(id); return !!(s && v !== '' && s.querySelector('option[value="' + String(v).replace(/"/g, '') + '"]')); }
    function text(id) { var s = $id(id); return s && s.selectedIndex > 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function byId(list, id) { id = int(id); return (list || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0] || null; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today());
        var f = new Date(); f.setDate(f.getDate() - 3); setVal('FromDateHistory', iso(f)); setVal('ToDateHistory', today());
        loadLookups().then(function () { rateLock(); renderGrid(); });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; R = d.rights || {};
            fill('CmbEntryType', d.entryTypes, 'Id', 'Name');
            fill('CmbAdjustmentTypeHistory', d.entryTypes, 'Id', 'Name');
            fill('CmbItemName', d.items, 'Id', 'ItemName');
            fill('CmbItemCondition', d.conditions, 'Id', 'Description');
            if (RecId === 0) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'SA-' + d.docNo; }
            $id('ChkPrint').checked = !!R.print;
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    function applyRights() {
        var upd = RecId > 0;
        show('btnsave', !upd); show('btnupdate', upd);
        $id('btnsave').disabled = !R.save; $id('btnupdate').disabled = !R.update;
        $id('btnprint').disabled = !R.print; $id('ChkPrint').disabled = !R.print;
    }

    // ------------------------------------------------------------------ entry bar

    function entryType() { return int(val('CmbEntryType')); }
    function rateLock() { var ed = entryType() === 1; $id('txtItemRate').readOnly = !ed; $id('txtItemRate').disabled = !ed; }

    /* CmbEntryType_Leave :428 */
    function entryTypeLeave() {
        rateLock();
        return getJson(api + '/accounts?entryTypeId=' + entryType()).then(function (rows) {
            fill('CmbAccountTitle', rows, 'Id', 'AccountTitle'); refreshCombos();
        }).catch(function (e) { box(e.message); });
    }

    /* BindWarehouseDropdown :470 */
    function bindWarehouseDropdown(itemId) {
        var seen = {}, list = [];
        (L.racks || []).forEach(function (r) { if (int(r.ItemId) === itemId && !seen[r.WarehouseId]) { seen[r.WarehouseId] = 1; list.push({ Id: r.WarehouseId, Warehouse: r.WareHouseName }); } });
        fill('CmbWareHouse', list, 'Id', 'Warehouse');
        if (int(val('CmbWareHouse'))) return;
        if (list.length === 1) setVal('CmbWareHouse', list[0].Id);
        else if (list.length > 1) {
            var cfg = int(L.packingMaterialDefaultWarehouse);
            if (cfg && list.some(function (w) { return int(w.Id) === cfg; })) setVal('CmbWareHouse', cfg);
        }
    }
    /* RackBindFromGlobalRacksByItemId :504 */
    function rackBind(itemId, whId) {
        var seen = {}, list = [];
        (L.racks || []).forEach(function (r) { if (int(r.ItemId) === itemId && (!whId || int(r.WarehouseId) === whId) && !seen[r.Id]) { seen[r.Id] = 1; list.push(r); } });
        fill('CmbRackName', list, 'Id', 'RackName');
        if (int(val('CmbRackName'))) return null;
        var pick = null;
        if (list.length === 1) pick = list[0];
        else if (list.length > 1) pick = list.filter(function (r) { return int(r.BaseRackId) > 0; })[0] || null;
        if (pick) { setVal('CmbRackName', pick.Id); return rackLeave(); }
        return null;
    }
    /* CmbRackName_Leave :554 */
    function rackLeave() {
        var r = byId(L.racks, val('CmbRackName'));
        if (!r) return null;
        if (!int(val('CmbWareHouse')) || int(val('CmbWareHouse')) !== int(r.WarehouseId)) {
            if (!has('CmbWareHouse', r.WarehouseId)) fill('CmbWareHouse', [{ Id: r.WarehouseId, Warehouse: r.WareHouseName }], 'Id', 'Warehouse');
            setVal('CmbWareHouse', r.WarehouseId);
        }
        return AvailableStock();
    }
    /* CmbItemName_Leave :453 */
    function itemLeave() {
        var itemId = int(val('CmbItemName'));
        var p = [BindAvgRateForLossCase(), AvailableStock()];
        CalculateAmount();
        bindWarehouseDropdown(itemId);
        p.push(rackBind(itemId, int(val('CmbWareHouse'))));
        refreshCombos();
        return Promise.all(p);
    }

    function stockQuery(withLocation) {
        return '?recId=' + RecId + '&itemId=' + int(val('CmbItemName')) + '&conditionId=' + int(val('CmbItemCondition'))
            + '&warehouseId=' + (withLocation ? int(val('CmbWareHouse')) : 0) + '&rackId=' + (withLocation ? int(val('CmbRackName')) : 0)
            + '&entryTypeId=' + entryType() + '&docDate=' + encodeURIComponent(val('DocDate'));
    }
    /* AvailableStock :1614 */
    function AvailableStock() {
        return getJson(api + '/stock' + stockQuery(true)).then(function (d) {
            var l = $id('lblStockBalance');
            l.style.display = d.visible ? '' : 'none';
            l.textContent = num(d.stock) === 0 ? '' : fmt(d.stock, 2);
        }).catch(function (e) { box(e.message); });
    }
    /* BindAvgRateForLossCase :1550 - Loss only; FIFOCgs is never set, so always the average rate. */
    function BindAvgRateForLossCase() {
        if (entryType() !== 2) return Promise.resolve();
        return getJson(api + '/stock' + stockQuery(false)).then(function (d) {
            setVal('txtItemRate', num(d.rate) !== 0 ? clr(d.rate) : '0'); CalculateAmount();
        }).catch(function (e) { box(e.message); });
    }
    /* CalculateAmount :1502 */
    function CalculateAmount() {
        if (val('txtqty') !== '' && val('txtItemRate') !== '') setVal('txtItemAmount', clr(num(val('txtqty')) * num(val('txtItemRate'))));
        else setVal('txtItemAmount', '0');
    }

    /* FormValidationDetail :719 */
    function FormValidationDetail() {
        var checks = [['CmbItemName', 'ItemName field is required'], ['CmbWareHouse', 'WareHouseName field is required'],
            ['CmbRackName', 'Rack field is required'], ['CmbItemCondition', 'Item Condition field is required']];
        for (var i = 0; i < checks.length; i++) if (!int(val(checks[i][0]))) { box(checks[i][1]); focus(checks[i][0]); return false; }
        if (num(val('txtqty')) === 0) { box('ItemQty field is required'); focus('txtqty'); return false; }
        if (num(val('txtItemRate')) === 0) { box('ItemRate field is required'); focus('txtItemRate'); return false; }
        if (num(val('txtItemAmount')) === 0) { box('ItemAmount field is required'); focus('txtItemAmount'); return false; }
        if (!int(val('CmbAccountTitle'))) { box('Account Title field is required'); focus('CmbAccountTitle'); return false; }
        return true;
    }
    function rowFromBar(id) {
        return { Id: id, ItemId: int(val('CmbItemName')), Item: text('CmbItemName'), WareHouseId: int(val('CmbWareHouse')), WareHouse: text('CmbWareHouse'),
            RackId: int(val('CmbRackName')), RackName: text('CmbRackName'), ItemConditionId: int(val('CmbItemCondition')), ItemCondition: text('CmbItemCondition'),
            ItemQty: num(val('txtqty')), ItemRate: num(val('txtItemRate')), ItemAmount: num(val('txtItemAmount')),
            AccountId: int(val('CmbAccountTitle')), AccountTitle: text('CmbAccountTitle'), Comments: val('txtremarksdetail').trim() };
    }
    function btnplus_Click() {
        if (!FormValidationDetail()) return;
        table.push(rowFromBar(0));
        ResetDetail(); $id('CmbEntryType').disabled = true; renderGrid();
    }
    function btnUpdateDetail_Click() {
        if (updateDetailIndex < 0 || !FormValidationDetail()) return;
        table[updateDetailIndex] = rowFromBar(int(table[updateDetailIndex].Id));
        ResetDetail(); renderGrid();
    }
    /* ResetDetail :649 (condition kept) */
    function ResetDetail() {
        updateDetailIndex = -1;
        ['CmbItemName', 'txtqty', 'txtremarksdetail', 'CmbAccountTitle'].forEach(function (k) { setVal(k, ''); });
        fill('CmbWareHouse', [], 'Id', 'Warehouse'); fill('CmbRackName', [], 'Id', 'RackName');
        setVal('txtItemRate', '0'); setVal('txtItemAmount', '0');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        refreshCombos(); renderGrid(); focus('CmbItemName');
    }
    /* grd_DoubleClick :876 */
    function editRow(i) {
        var r = table[i]; if (!r) return;
        updateDetailIndex = i;
        setVal('CmbItemName', r.ItemId); setVal('CmbWareHouse', ''); setVal('CmbRackName', '');
        bindWarehouseDropdown(int(r.ItemId));
        setVal('CmbWareHouse', r.WareHouseId); rackBind(int(r.ItemId), int(r.WareHouseId));
        setVal('CmbRackName', r.RackId); setVal('CmbItemCondition', r.ItemConditionId);
        setVal('txtqty', clr(r.ItemQty)); setVal('txtItemRate', clr(r.ItemRate)); setVal('txtItemAmount', clr(r.ItemAmount));
        setVal('CmbAccountTitle', r.AccountId); setVal('txtremarksdetail', r.Comments);
        AvailableStock();
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        refreshCombos(); renderGrid(); focus('CmbWareHouse');
    }
    /* grd_ColumnButtonClick :772 "Delete" */
    function deleteRow(i) {
        var r = table[i]; if (!r) return;
        if (int(r.Id) > 0 && !ask('Are you sure to Delete?')) return;
        table.splice(i, 1);
        if (updateDetailIndex === i) ResetDetail(); else if (updateDetailIndex > i) updateDetailIndex--;
        if (table.length === 0) $id('CmbEntryType').disabled = false;
        renderGrid();
    }

    var GRID_COLS = [['Item', 'Item'], ['WareHouse', 'WareHouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'],
        ['ItemQty', 'ItemQty', 1], ['ItemRate', 'ItemRate', 1], ['ItemAmount', 'ItemAmount', 1], ['AccountTitle', 'AccountTitle'], ['Comments', 'Comments']];
    function renderGrid() {
        $id('grdHead').innerHTML = '<th>X</th><th>Edit</th>' + GRID_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
        var q = 0, a = 0;
        $id('grd').innerHTML = table.map(function (r, i) {
            q += num(r.ItemQty); a += num(r.ItemAmount);
            return '<tr class="data-row' + (i === updateDetailIndex ? ' editing' : '') + '" data-i="' + i + '">'
                + '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>'
                + GRID_COLS.map(function (c) { return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[2] ? fmt(r[c[0]]) : r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = table.length ? '<td colspan="6">' + table.length + ' row(s)</td><td class="num">' + fmt(q) + '</td><td></td><td class="num">' + fmt(a) + '</td><td colspan="2"></td>' : '';
    }

    // ------------------------------------------------------------------ save / read

    /* Insert :933 */
    function Insert(btn) {
        if (table.length === 0) { box('Grid Record Not Found'); return; }
        if (!int(val('txtdocno'))) { box('DocNo Field is Required'); focus('txtdocno'); return; }
        if (!entryType()) { box('EntryType Field is Required'); focus('CmbEntryType'); return; }
        if (!ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { Id: RecId, DocDate: val('DocDate'), AdjustmentTypeId: entryType(), RemarksHeader: val('txtremarks'),
            rows: table.map(function (r) { return { Id: int(r.Id), ItemId: int(r.ItemId), WareHouseId: int(r.WareHouseId), RackId: int(r.RackId),
                ItemConditionId: int(r.ItemConditionId), ItemQty: num(r.ItemQty), ItemRate: num(r.ItemRate), ItemAmount: num(r.ItemAmount),
                AccountId: int(r.AccountId), Comments: r.Comments || '' }; }) };
        return busy(btn, function () {
            return http('POST', api + '/save', body).then(function (d) {
                box(d.message);
                var print = $id('ChkPrint').checked;
                return Reset().then(function () { if (print) GenerateReport(d.id); });
            }).catch(function (e) { box(e.message); });
        });
    }
    /* Reset :603 */
    function Reset() {
        RecId = 0; VoucherHeadId = 0; updateDetailIndex = -1; table = [];
        $id('CmbEntryType').disabled = false; setVal('CmbEntryType', '');
        setVal('txtremarks', ''); ResetDetail(); rateLock();
        fill('CmbAccountTitle', [], 'Id', 'AccountTitle');
        applyRights(); renderGrid(); focus('DocDate');
        return getJson(api + '/doc-no').then(function (d) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'SA-' + d.docNo; })
            .catch(function (e) { box(e.message); });
    }
    /* ReadById :1060 */
    function ReadById(id) {
        return getJson(api + '/' + id).then(function (d) {
            RecId = int(d.Id); VoucherHeadId = int(d.VoucherHeadId) || VoucherHeadId;
            showTab(0);
            setVal('DocDate', dateOnly(d.DocDate)); setVal('txtdocno', d.DocNo); $id('txtDocNoShow').textContent = 'SA-' + d.DocNo;
            setVal('CmbEntryType', d.AdjustmentTypeId); $id('CmbEntryType').disabled = true;
            setVal('txtremarks', d.RemarksHeader);
            table = (d.rows || []).map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; return o; });
            return entryTypeLeave().then(function () { ResetDetail(); applyRights(); renderGrid(); });
        }).catch(function (e) { box(e.message); });
    }

    function GenerateReport(id) {
        if (!int(id)) { box('RecordId Not Found'); return; }
        window.open('/api/reports/sa-215/data?id=' + encodeURIComponent(id), '_blank');
    }
    function VoucherReport(id) {
        if (!int(id)) { box('VoucherId Not Found'); return; }
        window.open('/api/reports/acc-118/data?id=' + encodeURIComponent(id), '_blank');
    }

    /* AvgRateUpdateOnDocDateChange :1992 (Generate Rate) */
    function btnGenerateRate_Click() {
        if (table.length === 0 || entryType() !== 2) return Promise.resolve();
        var chain = Promise.resolve();
        table.forEach(function (r) {
            chain = chain.then(function () {
                var q = '?recId=' + RecId + '&itemId=' + int(r.ItemId) + '&conditionId=' + int(r.ItemConditionId) + '&warehouseId=0&rackId=0&entryTypeId=2&docDate=' + encodeURIComponent(val('DocDate'));
                return getJson(api + '/stock' + q).then(function (d) {
                    var rate = num(d.rate);
                    if (rate > 0) { r.ItemRate = rate; r.ItemAmount = num(r.ItemQty) * rate; } else { r.ItemRate = 0; r.ItemAmount = 0; }
                });
            });
        });
        return chain.then(renderGrid).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history

    var HIST_COLS = [['DocNo', 'DocNo'], ['DocDate', 'DocDate', 'd'], ['Entrytype', 'Entrytype'], ['EntryUser', 'EntryUser'], ['EntryDate', 'EntryDate', 't'],
        ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 't'], ['Remarks', 'Remarks'], ['NoOfAttachments', 'NoOfAttachments']];
    /* gridhistoryfill :1142 */
    function gridhistoryfill() {
        var q = '?fromDate=' + ($id('chkFromDate').checked ? encodeURIComponent(val('FromDateHistory')) : '')
            + '&toDate=' + ($id('chkToDate').checked ? encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('FromDocNoHistory')) + '&toDocNo=' + int(val('ToDocNoHistory'))
            + '&adjustmentTypeId=' + int(val('CmbAdjustmentTypeHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            $id('histDetailHead').innerHTML = ''; $id('grdHisttoyDetail').innerHTML = '';
            if (!historyRows.length) { $id('histHead').innerHTML = ''; $id('grdhistory').innerHTML = ''; $id('lblHistCount').textContent = ''; return; }
            $id('histHead').innerHTML = '<th>Edit</th><th>Print</th><th>Voucher</th>' + HIST_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
            $id('grdhistory').innerHTML = historyRows.map(function (r, i) {
                return '<tr class="data-row" data-i="' + i + '"><td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>'
                    + '<td><button type="button" class="win-btn-mini" data-act="print">Print</button></td>'
                    + '<td><button type="button" class="win-btn-mini" data-act="voucher">Voucher</button></td>'
                    + HIST_COLS.map(function (c) { var v = r[c[0]]; return '<td>' + esc(c[2] === 'd' ? ddmmm(v) : c[2] === 't' ? dt(v) : v) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('lblHistCount').textContent = historyRows.length + ' record(s)';
        }).catch(function (e) { box(e.message); });
    }
    /* grdhistory_SelectionChanged :1305 */
    function historySelect(i) {
        var r = historyRows[i]; if (!r) return;
        Array.prototype.forEach.call($id('grdhistory').rows, function (tr) { tr.classList.toggle('sel', int(tr.getAttribute('data-i')) === i); });
        getJson(api + '/' + int(r.Id)).then(function (d) {
            var cols = [['Item', 'Item'], ['WareHouse', 'WareHouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'], ['ItemQty', 'ItemQty', 1],
                ['ItemRate', 'ItemRate', 1], ['ItemAmount', 'ItemAmount', 1], ['AccountTitle', 'AccountTitle'], ['Comments', 'Comments']];
            $id('histDetailHead').innerHTML = cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
            $id('grdHisttoyDetail').innerHTML = (d.rows || []).map(function (x) {
                return '<tr>' + cols.map(function (c) { return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[2] ? fmt(x[c[0]]) : x[c[0]]) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }
    function historyOpen(i) { var r = historyRows[i]; if (!r) return; RecId = int(r.Id); VoucherHeadId = int(r.VoucherHeadId); return ReadById(RecId); }

    // ------------------------------------------------------------------ wiring

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    function bindEvents() {
        on('CmbEntryType', 'change', entryTypeLeave);
        on('CmbItemName', 'change', function () { setVal('CmbWareHouse', ''); setVal('CmbRackName', ''); itemLeave(); });
        on('CmbWareHouse', 'change', function () { AvailableStock(); BindAvgRateForLossCase(); setVal('CmbRackName', ''); rackBind(int(val('CmbItemName')), int(val('CmbWareHouse'))); refreshCombos(); });
        on('CmbRackName', 'change', rackLeave);
        on('CmbItemCondition', 'change', function () { AvailableStock(); BindAvgRateForLossCase(); });
        on('txtqty', 'input', function () { BindAvgRateForLossCase(); CalculateAmount(); });
        on('txtItemRate', 'input', CalculateAmount);
        on('grd', 'click', function (e) {
            var b = e.target.closest('button'), tr = e.target.closest('tr'); if (!b || !tr) return;
            var i = int(tr.getAttribute('data-i'));
            if (b.getAttribute('data-act') === 'del') deleteRow(i); else editRow(i);
        });
        on('grd', 'dblclick', function (e) { var tr = e.target.closest('tr'); if (tr) editRow(int(tr.getAttribute('data-i'))); });
        on('grdhistory', 'click', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            var i = int(tr.getAttribute('data-i')), b = e.target.closest('button');
            if (!b) { historySelect(i); return; }
            var r = historyRows[i], act = b.getAttribute('data-act');
            if (act === 'edit') historyOpen(i);
            else if (act === 'print') GenerateReport(r.Id);
            else if (act === 'voucher') VoucherReport(r.VoucherHeadId);
        });
        on('grdhistory', 'dblclick', function (e) { var tr = e.target.closest('tr'); if (tr) historyOpen(int(tr.getAttribute('data-i'))); });
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
            else if (k === 's') { e.preventDefault(); if (currentTab === 1) window.Sap.btnShowHistory_Click(); else window.Sap.btnsave_Click(); }
            else if (k === 'u' && currentTab === 0) { e.preventDefault(); window.Sap.btnupdate_Click(); }
        });
    }

    window.Sap = {
        btnnew_Click: function () { return busy('btnnew', Reset); },
        btnRefresh_Click: function () { return busy('btnRefresh', loadLookups); },
        btnsave_Click: function () { if (RecId !== 0 || $id('btnsave').disabled) return; return Insert('btnsave'); },
        btnupdate_Click: function () { if (RecId === 0) { box('Record Not Update because RecId Not Found'); return; } if ($id('btnupdate').disabled) return; return Insert('btnupdate'); },
        btnprint_Click: function () { GenerateReport(RecId); },
        btnVoucher118_Click: function () { VoucherReport(VoucherHeadId); },
        btnGenerateRate_Click: function () { return busy('btnGenerateRate', btnGenerateRate_Click); },
        btnplus_Click: btnplus_Click, btnUpdateDetail_Click: btnUpdateDetail_Click,
        btnCancelUpdateDetial_Click: ResetDetail,
        showTab: showTab, btnShowHistory_Click: function () { return busy('btnShowHistory', gridhistoryfill); },
        btnNewHistory_Click: function () {
            var f = new Date(); f.setDate(f.getDate() - 3); setVal('FromDateHistory', iso(f));
            setVal('FromDocNoHistory', ''); setVal('ToDocNoHistory', ''); focus('FromDateHistory');
        }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
