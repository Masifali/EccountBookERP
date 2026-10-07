(function () {
    'use strict';
    var API = '/api/packing-material/flow-allocation';
    var lookupData = { transactionFlows: [], categories: [], types: [] };
    var busy = false;
    function byId(id) { return document.getElementById(id); }
    function esc(value) {
        return String(value == null ? '' : value).replace(/[&<>"']/g, function (c) {
            return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[c];
        });
    }
    function int(value) { var n = parseInt(value, 10); return Number.isFinite(n) ? n : 0; }
    function status(message, isError) {
        byId('status').textContent = message || '';
        byId('status').style.color = isError ? '#842029' : '#155724';
    }
    async function request(url, options) {
        var response = await fetch(url, Object.assign({ credentials: 'same-origin' }, options || {}));
        var body = await response.json();
        if (!response.ok) throw new Error(body.message || body.error || 'Unable to complete the request.');
        return body;
    }
    function populate(id, rows, label, key) {
        var el = byId(id), html = '<option value="0">' + esc(label || '') + '</option>';
        (rows || []).forEach(function (row) {
            html += '<option value="' + esc(row.Id || row.ID || row.id) + '">' + esc(row[key] || row.Name || row.name || row.TransactionFlow || '') + '</option>';
        });
        el.innerHTML = html;
    }
    function filters() {
        return { transactionFlowId: int(byId('transactionFlow').value),
            itemTypeId: int(byId('itemType').value), itemCategoryId: int(byId('itemCategory').value) };
    }
    function selected(tableId) {
        return Array.from(byId(tableId).querySelectorAll('tbody input[type=checkbox]:checked')).map(function (c) { return int(c.value); });
    }
    function render(tableId, countId, rows) {
        var body = byId(tableId).tBodies[0];
        body.innerHTML = (rows || []).map(function (row) {
            var id = int(row.ItemId || row.itemId), name = row.ItemName || row.itemName || '';
            return '<tr><td><input type="checkbox" value="' + id + '" aria-label="Select ' + esc(name) + '"></td><td>' + esc(name) + '</td></tr>';
        }).join('');
        byId(countId).textContent = (rows || []).length + ' records';
        byId(tableId).querySelector('thead input[type=checkbox]').checked = false;
    }
    async function show() {
        var f = filters();
        if (!f.transactionFlowId) { status('Select TransactionFlow First', true); return; }
        if (busy) return;
        busy = true; byId('showButton').disabled = true; status('Loading allocation lists…', false);
        try {
            var base = '?transactionFlowId=' + f.transactionFlowId + '&itemTypeId=' + f.itemTypeId + '&itemCategoryId=' + f.itemCategoryId;
            var result = await Promise.all([request(API + '/items' + base + '&actionId=1'), request(API + '/items' + base + '&actionId=2')]);
            render('pendingGrid', 'pendingCount', result[0]); render('allocatedGrid', 'allocatedCount', result[1]); status('', false);
        } catch (e) { status(e.message, true); }
        finally { busy = false; byId('showButton').disabled = false; }
    }
    async function move(kind) {
        if (busy) return;
        var f = filters(), table = kind === 'allocate' ? 'pendingGrid' : 'allocatedGrid', ids = selected(table);
        if (!f.transactionFlowId) { status('Select TransactionFlow First', true); return; }
        if (!ids.length) { status(kind === 'allocate' ? "Checked Row's first To Allocate Items" : "Checked Row's first To Un-Allocate Items", true); return; }
        if (kind === 'deallocate' && !window.confirm("Are you sure to UnAllocate Item's?")) return;
        var button = byId(kind === 'allocate' ? 'allocateButton' : 'deallocateButton');
        busy = true; button.disabled = true; status(kind === 'allocate' ? 'Allocating selected items…' : 'Removing selected allocations…', false);
        try {
            var result = await request(API + '/' + kind, { method: 'POST', headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ transactionFlowId: f.transactionFlowId, itemIds: ids }) });
            status(result.message || 'Completed.', false);
            busy = false; button.disabled = false;
            await show();
        } catch (e) { status(e.message, true); }
        finally { busy = false; button.disabled = false; }
    }
    function selectAll(tableId, checked) { byId(tableId).querySelectorAll('tbody input[type=checkbox]').forEach(function (c) { c.checked = checked; }); }
    async function load() {
        busy = true; byId('refreshButton').disabled = true; status('Loading form values…', false);
        try {
            lookupData = await request(API + '/lookups');
            populate('transactionFlow', lookupData.transactionFlows, '', 'TransactionFlow');
            populate('itemCategory', lookupData.categories, '', 'Name');
            populate('itemType', lookupData.types, '', 'Name');
            render('pendingGrid', 'pendingCount', []); render('allocatedGrid', 'allocatedCount', []); status('', false);
        } catch (e) { status(e.message, true); }
        finally { busy = false; byId('refreshButton').disabled = false; }
    }
    document.addEventListener('DOMContentLoaded', function () {
        byId('showButton').addEventListener('click', show);
        byId('transactionFlow').addEventListener('change', function () { if (int(this.value)) show(); });
        byId('allocateButton').addEventListener('click', function () { move('allocate'); });
        byId('deallocateButton').addEventListener('click', function () { move('deallocate'); });
        byId('refreshButton').addEventListener('click', load);
        byId('shortcutButton').addEventListener('click', function () { status('Ctrl+R: Refresh   F1: Transaction Flow   Ctrl+A: Allocate   Ctrl+D: De-Allocate   Esc: Back', false); });
        ['pendingGrid', 'allocatedGrid'].forEach(function (id) {
            byId(id).querySelector('thead input[type=checkbox]').addEventListener('change', function () { selectAll(id, this.checked); });
        });
        document.addEventListener('keydown', function (event) {
            if (event.key === 'Escape') { history.back(); return; }
            if (event.key === 'F1') { event.preventDefault(); byId('transactionFlow').focus(); }
            if (event.ctrlKey && event.key.toLowerCase() === 'r') { event.preventDefault(); load(); }
            if (event.ctrlKey && event.key.toLowerCase() === 'a') { event.preventDefault(); move('allocate'); }
            if (event.ctrlKey && event.key.toLowerCase() === 'd') { event.preventDefault(); move('deallocate'); }
        });
        load();
    });
})();
