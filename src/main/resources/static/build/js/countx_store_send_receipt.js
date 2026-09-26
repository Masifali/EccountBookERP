/* ============================================================================================
 * Store Send Receipt - screen 504, module 54, DocumentTypeId 177.
 * Desktop: Architecture.WinApp.PackingMaterial_Store.StoreSendReceipt
 *        + LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts.
 *
 *   Load :170   LoadDetailForGrid :433   Reset :492   FormValidation :542   Insert :590
 *   GridHistoryBind :748   Grd_CellUpdated :830   Grd_ColumnButtonClick :886   ReadById :946
 *   btnGenerateItemNameFromCurrentPosition :1140   AvgRateUpdateOnDocDateChange :1179
 * The server repeats every figure and validation; nothing here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/store-send-receipt';
    var L = null, perms = {};
    var dtDetail = [], RecId = 0, currentRow = -1;
    var loaderRows = [], historyRows = [];
    var newFiles = [], removeAttachmentIds = [], existingAttachments = [];
    var currentTab = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
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
    function even(x) { var r = Math.round(x); if (Math.abs(Math.abs(x % 1) - 0.5) < 1e-9) r = 2 * Math.round(x / 2); return r; }
    function fmt(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 3 : d }); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }
    function dateTime(v) { if (!v) return ''; var d = new Date(v); if (isNaN(d.getTime())) return esc(v); var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return ddmmm(iso(d)) + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap; }

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
    function options(rows, v, t, current) {
        var html = '<option value="0"></option>';
        (rows || []).forEach(function (r) { var x = int(col(r, v)); html += '<option value="' + x + '"' + (x === int(current) ? ' selected' : '') + '>' + esc(col(r, t)) + '</option>'; });
        return html;
    }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today());
        var f = new Date(); f.setDate(f.getDate() - 7); setVal('FromDateHistory', iso(f)); setVal('ToDateHistory', today());
        loadLookups().then(function () {
            var r = (L && L.receiverWarehouses) || [];
            if (r.length && !val('CmbReceiverWareHouse')) setVal('CmbReceiverWareHouse', r[0].Id);   // Rows[1].Activate()
            renderGrid(); refreshCombos();
        });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; perms = d.permissions || {};
            fill('CmbSenderWareHouse', d.senderWarehouses, 'Id', 'Description');
            fill('CmbReceiverWareHouse', d.receiverWarehouses, 'Id', 'Description');
            fill('CmbSenderWarehouseHistory', d.historySenders, 'Id', 'Description');
            fill('CmbReceiverWarehouseHistory', d.historyReceivers, 'Id', 'Description');
            if (RecId === 0) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'SSR-' + d.docNo; }
            applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    function applyRights() {
        var upd = RecId > 0;
        show('btnsave', !upd); show('btnupdate', upd);
        $id('btnsave').disabled = !perms.Save; $id('btnupdate').disabled = !perms.Update; $id('btnPrint').disabled = !perms.Print;
    }

    // ------------------------------------------------------------------ grid

    function renderGrid() {
        $id('grdHead').innerHTML = '<th>X</th><th>+</th><th>RecordNo</th><th>Item Name</th><th>Item Condition</th><th>PackingType</th><th>ItemQty</th><th>AvgRate</th><th>Amount</th>';
        var html = '', tq = 0, ta = 0;
        dtDetail.forEach(function (r, i) {
            tq += num(r.ItemQty); ta += num(r.Amount);
            html += '<tr class="data-row' + (i === currentRow ? ' sel' : '') + '" data-i="' + i + '">'
                + '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td class="num">' + int(r.RecordNo) + '</td>'
                + '<td><select class="cell-sel" data-f="ItemId" style="max-width:260px">' + options(L && L.items, 'ItemId', 'ItemName', r.ItemId) + '</select></td>'
                + '<td><select class="cell-sel" data-f="ItemConditionId">' + options(L && L.conditions, 'Id', 'Description', r.ItemConditionId) + '</select></td>'
                + '<td>' + esc(r.PackingType) + '</td>'
                + '<td><input class="cell" data-f="ItemQty" value="' + esc(r.ItemQty) + '"/></td>'
                + '<td class="num">' + fmt(r.AvgRate) + '</td><td class="num">' + fmt(r.Amount, 3) + '</td></tr>';
        });
        $id('Grd').innerHTML = html;
        $id('grdFoot').innerHTML = dtDetail.length ? '<td></td><td></td><td></td><td></td><td></td><td></td><td class="num">' + fmt(tq) + '</td><td></td><td class="num">' + fmt(ta, 3) + '</td>' : '';
    }

    function rateFor(r) {
        return getJson(api + '/avg-rate?itemId=' + int(r.ItemId) + '&conditionId=' + int(r.ItemConditionId) + '&docDate=' + encodeURIComponent(val('DocDate')) + '&recId=' + RecId)
            .then(function (d) { r.AvgRate = num(d.rate); r.Amount = num(r.ItemQty) * r.AvgRate; });
    }

    /* Grd_CellUpdated :830 */
    function cellUpdated(i, f, value) {
        var r = dtDetail[i]; if (!r) return;
        if (f === 'ItemId' || f === 'ItemConditionId') {
            r[f] = int(value);
            return rateFor(r).then(renderGrid).catch(function (e) { box(e.message); });
        }
        if (f === 'ItemQty') {
            r.ItemQty = num(value);
            if (RecId === 0 && perms.Save) {
                var others = 0;
                dtDetail.forEach(function (x, j) { if (j !== i && int(x.RecordNo) === int(r.RecordNo)) others += num(x.ItemQty); });
                var max = num(r.BalQty) - others;
                if (r.ItemQty <= 0) { r.ItemQty = max; box('Qty must be greater than 0. Reset to max allowed: ' + fmt(max)); }
                else if (r.ItemQty > max) { r.ItemQty = max; box('Qty exceeds allowed balance for RecordNo ' + r.RecordNo + '.\nOther rows already use ' + fmt(others) + ', max allowed is ' + fmt(max) + '.'); }
            }
            r.Amount = num(r.ItemQty) * num(r.AvgRate);
            renderGrid();
        }
    }

    /* Grd_ColumnButtonClick :886 */
    function buttonClick(i, act) {
        var r = dtDetail[i]; if (!r) return;
        if (act === 'del') {
            dtDetail.splice(i, 1);
            if (currentRow >= dtDetail.length) currentRow = -1;
        } else {
            var bal = num(r.BalQty), qty = num(r.ItemQty), add = 0;
            if (bal === qty) { box('Current Row can\'t be breakable. First Break The Qty of Row'); return; }
            var same = dtDetail.filter(function (x) { return int(x.RecordNo) === int(r.RecordNo); });
            if (same.length === 1) add = bal - qty;
            else {
                var tot = 0; same.forEach(function (x) { tot += num(x.ItemQty); });
                if (tot > bal) { box('Total Qty Of Record#' + r.RecordNo + ' Can\'t Be Greater Than Balance Qty Which Is ' + fmt(bal)); return; }
                add = bal - tot;
                if (add === 0) { box('Current Row can\'t be breakable. First Break The Qty of any Row of Record No' + r.RecordNo); return; }
            }
            var c = {}; for (var k in r) c[k] = r[k];
            c.ItemQty = add;
            if (RecId > 0) c.Id = 0;
            dtDetail.push(c);
        }
        if (!dtDetail.length) setVal('CmbSenderWareHouse', '');
        renderGrid();
    }

    /* btnGenerateItemNameFromCurrentPosition_Click :1140 */
    function fillItemDown() {
        var cur = dtDetail[currentRow]; if (!cur) return;
        dtDetail.forEach(function (r, j) { if (j > currentRow && !int(r.ItemId)) r.ItemId = cur.ItemId; });
        return Promise.all(dtDetail.map(rateFor)).then(renderGrid).catch(function (e) { box(e.message); });
    }

    /* DocDate_ValueChanged :1210 */
    function docDateChanged() {
        if (!dtDetail.length) return;
        return Promise.all(dtDetail.map(rateFor)).then(renderGrid).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ loader

    var loader = {
        open: function () {
            $id('dlgLoader').classList.add('open');
            setVal('LoaderFromDate', dateOnly(L && L.financialYearStart) || today()); setVal('LoaderToDate', today());
            return loader.load();
        },
        close: function () { $id('dlgLoader').classList.remove('open'); },
        load: function () {
            return getJson(api + '/pending').then(function (rows) {
                loaderRows = rows || [];
                $id('loaderHead').innerHTML = '<th><input type="checkbox" id="chkAllLoader"/></th><th>DocDate</th><th>DocNo</th><th>SenderWareHouse</th><th>PackingType</th><th>PackUom</th><th>IssueQty</th><th>ReceivedQty</th><th>BalQty</th>';
                var html = '';
                loaderRows.forEach(function (r, i) {
                    html += '<tr><td><input type="checkbox" data-pick="' + i + '"/></td><td>' + ddmmm(r.DocDate) + '</td><td>' + esc(r.DocNo) + '</td><td>' + esc(r.SenderWareHouse) + '</td><td>'
                        + esc(r.PackingType) + '</td><td>' + esc(r.PackUom) + '</td><td class="num">' + fmt(r.IssueQty) + '</td><td class="num">' + fmt(r.ReceivedQty) + '</td><td class="num">' + fmt(r.BalQty) + '</td></tr>';
                });
                $id('loaderBody').innerHTML = html; selectedQty();
            }).catch(function (e) { box(e.message); });
        },
        take: function () {
            var picked = Array.prototype.filter.call(document.querySelectorAll('#loaderBody input[data-pick]'), function (x) { return x.checked; })
                .map(function (x) { return loaderRows[int(x.getAttribute('data-pick'))]; });
            if (!picked.length) { box('Please Select Row first'); return; }
            var wid = 0;
            for (var i = 0; i < picked.length; i++) {
                var w = int(picked[i].SenderWareHouseId); if (!w) continue;
                if (!wid) wid = w;
                if (wid !== w) { box('Sorry Check Rows Which Have Same Warehouse'); return; }
            }
            loader.close(); LoadDetailForGrid(picked.filter(function (p) { return int(p.SenderWareHouseId) !== 0; }));
        }
    };
    function selectedQty() {
        var s = 0; Array.prototype.forEach.call(document.querySelectorAll('#loaderBody input[data-pick]'), function (x) { if (x.checked) s += num(loaderRows[int(x.getAttribute('data-pick'))].BalQty); });
        setVal('txtSelectedQty', fmt(even(s)));
    }

    /* LoadDetailForGrid :433 */
    function LoadDetailForGrid(dt) {
        if (!dt.length) return;
        if (int(val('CmbSenderWareHouse')) && dtDetail.length && int(val('CmbSenderWareHouse')) !== int(dt[0].SenderWareHouseId)) { box("You Can't Load Data Of Different Warehouse"); return; }
        setVal('CmbSenderWareHouse', dt[0].SenderWareHouseId);
        dt.forEach(function (p, i) {
            var max = 0;
            dtDetail.forEach(function (x) { max = Math.max(max, int(x.RecordNo)); });
            if (dtDetail.some(function (x) { return int(x.RefDocDetailId) === int(p.RefDocSubIdNo); })) return;
            dtDetail.push({ Id: 0, RefDocNoId: p.RefDocIdNo, RefDocDetailId: p.RefDocSubIdNo, RefDocumentTypeId: p.RefDocumentTypeId,
                RecordNo: max > 0 ? max + 1 : i + 1, ItemId: 0, ItemConditionId: 0, PackingTypeId: p.PackingTypeId, PackingType: p.PackingType,
                ItemQty: p.BalQty, BalQty: p.BalQty, AvgRate: 0, Amount: 0 });
        });
        refreshCombos(); renderGrid();
    }

    // ------------------------------------------------------------------ save

    function FormValidation() {
        if (!int(val('txtdocno'))) { box('Doc No is Required'); return false; }
        if (!int(val('CmbSenderWareHouse'))) { box('Sender Warehouse is Required'); return false; }
        if (!int(val('CmbReceiverWareHouse'))) { box('Receiver Warehouse is Required'); return false; }
        return true;
    }

    function Insert(btnId) {
        if (!FormValidation()) return;
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        if (!dtDetail.length) { box('Grid Record Not Found'); return; }
        for (var i = 0; i < dtDetail.length; i++) {
            var r = dtDetail[i], n = i + 1;
            if (!int(r.ItemId)) { box('Item Required In Row#' + n + ' In Detail Grid'); return; }
            if (!int(r.ItemConditionId)) { box('Item Condition Required In Row#' + n + ' In Detail Grid'); return; }
            if (!num(r.ItemQty)) { box('Item Qty Required In Row#' + n + ' In Detail Grid'); return; }
            if (!num(r.AvgRate)) { box('Avg Rate Required In Row#' + n + ' In Detail Grid'); return; }
        }
        var req = {
            Id: RecId, DocDate: val('DocDate'), SenderWarehouseId: int(val('CmbSenderWareHouse')), ReceiverWarehouseId: int(val('CmbReceiverWareHouse')),
            lines: dtDetail.map(function (r) {
                return { Id: int(r.Id), RefDocNoId: int(r.RefDocNoId), RefDocDetailId: int(r.RefDocDetailId), RefDocumentTypeId: int(r.RefDocumentTypeId),
                    RecordNo: int(r.RecordNo), ItemId: int(r.ItemId), ItemConditionId: int(r.ItemConditionId), PackingTypeId: int(r.PackingTypeId), ItemQty: num(r.ItemQty) };
            }),
            files: newFiles, removeAttachmentIds: removeAttachmentIds
        };
        return busy(btnId, function () {
            say('Saving...');
            return http('POST', api + '/save', req).then(function (d) {
                say(''); box(d.message);
                var id = d.id;
                return Reset().then(function () { if ($id('ChkPrint').checked) GeneratePrint(id); });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }

    function GeneratePrint(id) { if (!int(id)) { box('No Record Found For Display'); return; } window.open('/api/reports/ssr-465/data?id=' + encodeURIComponent(id), '_blank'); }

    /* Reset :492 - the receiver warehouse and the date stay. */
    function Reset() {
        RecId = 0; currentRow = -1;
        newFiles = []; removeAttachmentIds = []; existingAttachments = []; renderAttachments();
        setVal('txtdocno', ''); setVal('CmbSenderWareHouse', '');
        dtDetail = []; renderGrid(); applyRights();
        return getJson(api + '/numbers').then(function (d) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'SSR-' + d.docNo; refreshCombos(); })
            .catch(function (e) { box(e.message); });
    }

    /* ReadById :946 */
    function ReadById(id) {
        say('Loading...');
        return getJson(api + '/' + id).then(function (d) {
            say('');
            var h = d.header;
            RecId = int(col(h, 'InvStoreSendReceiptId')) || id;
            setVal('txtdocno', col(h, 'DocNo')); $id('txtDocNoShow').textContent = 'SSR-' + col(h, 'DocNo');
            setVal('DocDate', dateOnly(col(h, 'DocDate')));
            setVal('CmbSenderWareHouse', int(col(h, 'SenderWarehouseId')) || '');
            setVal('CmbReceiverWareHouse', int(col(h, 'ReceiverWarehouseId')) || '');
            dtDetail = (d.lines || []).map(function (l) { var r = {}; for (var k in l) r[k] = l[k]; return r; });
            currentRow = -1;
            existingAttachments = d.attachments || []; newFiles = []; removeAttachmentIds = []; renderAttachments();
            showTab(0); refreshCombos(); renderGrid(); applyRights();
        }).catch(function (e) { say(''); box(e.message); });
    }

    // ------------------------------------------------------------------ history

    function GridHistoryBind() {
        var q = '?fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory'))
            + '&senderId=' + int(val('CmbSenderWarehouseHistory')) + '&receiverId=' + int(val('CmbReceiverWarehouseHistory'))
            + ($id('chkFromDate').checked ? '&fromDate=' + encodeURIComponent(val('FromDateHistory')) : '')
            + ($id('chkToDate').checked ? '&toDate=' + encodeURIComponent(val('ToDateHistory')) : '');
        return getJson(api + '/history' + q).then(function (rows) {
            if (!rows || !rows.length) return;                                                     // the desktop leaves the old grid in place
            historyRows = rows;
            $id('histHead').innerHTML = '<th>Edit</th><th>Print</th><th>DocNo</th><th>DocDate</th><th>SenderWarehouse</th><th>ReceiverWarehouse</th><th>EntryDate</th><th>EntryUser</th><th>ModifyDate</th><th>ModifyUser</th><th>NoOfAttachments</th>';
            var html = '';
            historyRows.forEach(function (r, i) {
                html += '<tr class="data-row" data-h="' + i + '"><td><button type="button" class="win-btn-mini" data-h-act="edit">Edit</button></td><td><button type="button" class="win-btn-mini" data-h-act="print">Print</button></td>'
                    + '<td>' + esc(col(r, 'DocNo')) + '</td><td>' + ddmmm(col(r, 'DocDate')) + '</td><td>' + esc(col(r, 'SenderWareHouse')) + '</td><td>' + esc(col(r, 'ReceiverWareHouse')) + '</td>'
                    + '<td>' + dateTime(col(r, 'EntryDate')) + '</td><td>' + esc(col(r, 'EntryUser')) + '</td><td>' + dateTime(col(r, 'ModifyDate')) + '</td><td>' + esc(col(r, 'ModifyUser')) + '</td>'
                    + '<td class="num">' + esc(col(r, 'NoOfAttachments')) + '</td></tr>';
            });
            $id('GrdHistory').innerHTML = html; $id('GrdDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            $id('lblHistCount').textContent = historyRows.length + ' record(s)';
        }).catch(function (e) { box(e.message); });
    }
    function onHistoryClick(e) {
        var tr = e.target.closest('tr[data-h]'); if (!tr) return;
        var r = historyRows[int(tr.getAttribute('data-h'))]; if (!r) return;
        Array.prototype.forEach.call(document.querySelectorAll('#GrdHistory tr'), function (x) { x.classList.toggle('sel', x === tr); });
        var id = int(col(r, 'InvStoreSendReceiptId')), b = e.target.closest('button[data-h-act]');
        if (!b) { BindDetailOfHeaderId(id); return; }
        if (b.getAttribute('data-h-act') === 'edit') ReadById(id); else GeneratePrint(id);
    }
    function BindDetailOfHeaderId(id) {
        return getJson(api + '/' + id).then(function (d) {
            $id('histDetailHead').innerHTML = '<th>ItemName</th><th>ItemCondition</th><th>PackingType</th><th>ItemQty</th><th>AvgRate</th><th>Amount</th>';
            var html = '';
            (d.lines || []).forEach(function (l) {
                html += '<tr><td>' + esc(l.ItemName) + '</td><td>' + esc(l.ItemCondition) + '</td><td>' + esc(l.PackingType) + '</td><td class="num">' + fmt(l.ItemQty)
                    + '</td><td class="num">' + fmt(l.AvgRate) + '</td><td class="num">' + fmt(l.Amount) + '</td></tr>';
            });
            $id('GrdDetail').innerHTML = html;
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ attachments

    function onAttachmentsPicked(e) {
        Array.prototype.slice.call(e.target.files || []).forEach(function (file) {
            if (file.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: ' + file.name); return; }
            var reader = new FileReader();
            reader.onload = function () { var s = String(reader.result); newFiles.push({ name: file.name, base64: s.substring(s.indexOf(',') + 1) }); renderAttachments(); };
            reader.readAsDataURL(file);
        });
        e.target.value = '';
    }
    function renderAttachments() {
        var html = '';
        existingAttachments.forEach(function (a) {
            var id = int(col(a, 'Id')); if (removeAttachmentIds.indexOf(id) >= 0) return;
            html += '<div><a class="win-link" href="' + api + '/' + RecId + '/attachments/' + id + '">' + esc(col(a, 'Attachment')) + '</a> <span class="win-link" data-remove="' + id + '">[remove]</span></div>';
        });
        newFiles.forEach(function (f, i) { html += '<div>' + esc(f.name) + ' <em>(new)</em> <span class="win-link" data-drop="' + i + '">[remove]</span></div>'; });
        $id('lstAttachments').innerHTML = html;
    }

    // ------------------------------------------------------------------ events

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    function bindEvents() {
        on('Grd', 'change', function (e) {
            var t = e.target, f = t.getAttribute('data-f'), tr = t.closest('tr[data-i]'); if (!f || !tr) return;
            var i = int(tr.getAttribute('data-i')), v = t.value;
            setTimeout(function () { cellUpdated(i, f, v); }, 0);
        });
        on('Grd', 'click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = int(tr.getAttribute('data-i')), b = e.target.closest('button[data-act]');
            if (b) { buttonClick(i, b.getAttribute('data-act')); return; }
            if (currentRow !== i) { currentRow = i; Array.prototype.forEach.call(document.querySelectorAll('#Grd tr'), function (x) { x.classList.toggle('sel', x === tr); }); }
        });
        on('DocDate', 'change', docDateChanged);
        on('loaderBody', 'change', selectedQty);
        on('loaderHead', 'change', function (e) {
            if (e.target.id !== 'chkAllLoader') return;
            Array.prototype.forEach.call(document.querySelectorAll('#loaderBody input[data-pick]'), function (x) { x.checked = e.target.checked; });
            selectedQty();
        });
        on('GrdHistory', 'click', onHistoryClick);
        on('GrdHistory', 'dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var r = historyRows[int(tr.getAttribute('data-h'))]; if (r) ReadById(int(col(r, 'InvStoreSendReceiptId')));
        });
        on('fileAttachment', 'change', onAttachmentsPicked);
        on('lstAttachments', 'click', function (e) {
            var rm = e.target.getAttribute('data-remove'), dr = e.target.getAttribute('data-drop');
            if (rm) { removeAttachmentIds.push(int(rm)); renderAttachments(); }
            if (dr !== null && dr !== undefined) { newFiles.splice(int(dr), 1); renderAttachments(); }
        });
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 's') { e.preventDefault(); if (currentTab === 0) { if (RecId === 0 && perms.Save) Insert('btnsave'); } else GridHistoryBind(); }
            else if (k === 'u') { e.preventDefault(); if (RecId > 0 && perms.Update) Insert('btnupdate'); }
            else if (k === 'n') { e.preventDefault(); Reset(); }
            else if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
        });
    }

    window.Ssr = {
        btnnew_Click: function () { return busy('btnnew', Reset); },
        btnRefresh_Click: function () { return busy('btnRefresh', function () { return loadLookups().then(function () { refreshCombos(); renderGrid(); }); }); },
        btnsave_Click: function () { if (RecId !== 0 || $id('btnsave').disabled) return; return Insert('btnsave'); },
        btnupdate_Click: function () { if (RecId === 0 || $id('btnupdate').disabled) return; return Insert('btnupdate'); },
        btnattachment_Click: function () { var g = $id('grpAttachments'); g.style.display = g.style.display === 'none' ? '' : 'none'; },
        btnPrint_Click: function () { if (!perms.Print) return; if (RecId > 0) GeneratePrint(RecId); else box('No Record Selected'); },
        btnGenerateItemNameFromCurrentPosition_Click: fillItemDown,
        loader: loader, showTab: showTab,
        btnshow_Click: function () { return busy('btnshow', GridHistoryBind); },
        btnResetHistory_Click: function () { setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', ''); setVal('CmbSenderWarehouseHistory', ''); setVal('CmbReceiverWarehouseHistory', ''); },
        btnRefreshHistory_Click: function () { return loadLookups(); }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
