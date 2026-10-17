/* 599 Production Release - ProductionRelease.cs (pcc.Production). Pending productions -> tick -> Save (release 1857). */
(function () {
    'use strict';
    var API = '/production/production-release/api';
    var S = { pending: [], hist: [], init: null, sel: -1 };
    function $(id) { return document.getElementById(id); }
    function esc(v) { return String(v == null ? '' : v).replace(/[&<>"]/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' }[c]; }); }
    function msg(t, ok) { var m = $('prMsg'); m.textContent = t || ''; m.className = 'pr-msg ' + (ok ? 'ok' : 'err'); m.style.display = t ? 'block' : 'none'; }
    function fetchJson(url, opt) {
        return fetch(url, opt).then(function (r) { return r.json().catch(function () { return {}; }).then(function (d) { if (!r.ok) throw new Error((d && d.message) || 'Request failed.'); return d; }); });
    }
    function post(url, body) { return fetchJson(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body || {}) }); }
    function iso(d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
    function fmt(v) { if (v == null || v === '') return ''; var n = Number(v); return isNaN(n) ? esc(v) : n.toLocaleString(undefined, { maximumFractionDigits: 4 }); }

    function combo(id, label, list) {
        return '<div class="pr-f"><label>' + label + '</label><input id="' + id + '" list="' + id + 'L" autocomplete="off"><datalist id="' + id + 'L">' +
            (list || []).map(function (x) { return '<option value="' + esc(x.Name) + '" data-id="' + esc(x.Id) + '">'; }).join('') + '</datalist></div>';
    }
    function pick(id, list) {
        var v = ($(id).value || '').trim(); if (!v) return 0;
        for (var i = 0; i < list.length; i++) if (String(list[i].Name) === v) return Number(list[i].Id) || 0;
        return 0;
    }
    function table(cols, rows, opt) {
        opt = opt || {};
        var h = '<table class="pr-grid"><thead><tr>' + (opt.check ? '<th><input type="checkbox" id="chkAll"></th>' : '') +
            cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + (opt.extra ? opt.extra.map(function (e) { return '<th>' + esc(e[0]) + '</th>'; }).join('') : '') + '</tr></thead><tbody>';
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (i === S.sel && opt.selectable ? ' class="sel"' : '') + '>';
            if (opt.check) h += '<td><input type="checkbox" class="chk" data-i="' + i + '"></td>';
            cols.forEach(function (c) {
                var v = r[c[0]];
                if (c[2] === 'date') h += '<td>' + (opt.editDate ? '<input type="date" class="dd" data-i="' + i + '" value="' + esc(v) + '">' : esc(v)) + '</td>';
                else if (c[2] === 'n') h += '<td class="n">' + fmt(v) + '</td>';
                else h += '<td>' + esc(v) + '</td>';
            });
            if (opt.extra) opt.extra.forEach(function (e) { h += '<td><a data-act="' + e[1] + '" data-i="' + i + '">' + esc(e[2]) + '</a></td>'; });
            h += '</tr>';
        });
        return h + '</tbody></table>';
    }

    var PCOLS = [['DocDate', 'Doc Date', 'date'], ['RefDoNo', 'Ref DoNo'], ['ProductionDate', 'Production Date'], ['ProductionNo', 'Production No'], ['ProductionType', 'Production Type'],
        ['PlantName', 'Plant'], ['BomHeaderNo', 'BOM No'], ['ItemName', 'Item Name'], ['ItemCode', 'Item Code'], ['UOM', 'UOM'], ['ItemQty', 'Item Qty', 'n'],
        ['WipWareHouseName', 'WIP Warehouse'], ['WIPAccountTitle', 'WIP Account'], ['RemarksHeader', 'Remarks'], ['EntryDate', 'Entry Date'], ['EntryUserName', 'Entry User'],
        ['ModifyDate', 'Modify Date'], ['ModifyUserName', 'Modify User'], ['NoOfAttachments', 'Attachments']];
    var HCOLS = [['DocNo', 'Doc No'], ['DocDate', 'Doc Date'], ['RefDocNo', 'Ref DocNo'], ['RefDocDate', 'Ref Doc Date'], ['ProductionNo', 'Production No'], ['ProductionType', 'Production Type'],
        ['PlantName', 'Plant'], ['BomHeaderNo', 'BOM No'], ['ItemName', 'Item Name'], ['ItemCode', 'Item Code'], ['UOM', 'UOM'], ['ItemQty', 'Item Qty', 'n'],
        ['WipWareHouseName', 'WIP Warehouse'], ['WIPAccountTitle', 'WIP Account'], ['RemarksHeader', 'Remarks'], ['EntryDate', 'Entry Date'], ['EntryUserName', 'Entry User'],
        ['ModifyDate', 'Modify Date'], ['ModifyUserName', 'Modify User']];
    var INCOLS = [['WarehouseName', 'Warehouse'], ['ItemName', 'Item Name'], ['Uom', 'Uom'], ['ItemQty', 'Item Qty', 'n'], ['Rate', 'Rate', 'n'], ['Amount', 'Amount', 'n']];
    var OUTCOLS = [['SaleOrder', 'Sale Order'], ['WarehouseName', 'Warehouse'], ['ItemName', 'Item Name'], ['AttributeVarient', 'Attribute Varient'], ['VarientUnit', 'Varient Unit'],
        ['ItemQty', 'Item Qty', 'n'], ['VarientQty', 'Varient Qty', 'n'], ['Rate', 'Rate', 'n'], ['Amount', 'Amount', 'n']];

    function buildFilters(d) {
        var now = new Date(), first = d.fyStart || iso(now);
        $('fP').innerHTML =
            '<div class="pr-f"><label>From Date</label><input type="date" id="pFrom" value="' + iso(new Date(now.getFullYear(), now.getMonth(), 1)) + '"></div>' +
            '<div class="pr-f"><label>To Date</label><input type="date" id="pTo" value="' + iso(now) + '"></div>' +
            '<div class="pr-f"><label>From DocNo</label><input class="sm" id="pFD" type="number" min="0"></div>' +
            '<div class="pr-f"><label>To DocNo</label><input class="sm" id="pTD" type="number" min="0"></div>' +
            combo('pPN', 'Production No', d.productionNos) + combo('pBom', 'BOM Name', d.boms) + combo('pItem', 'Item', d.items) +
            combo('pPlant', 'Plant', d.plants) + combo('pType', 'Production Type', d.productionTypes) +
            '<button class="pr-btn" id="btnShowP" type="button">Show</button>';
        $('fH').innerHTML =
            '<div class="pr-f"><label>From Date</label><input type="date" id="hFrom" value="' + iso(new Date(now.getTime() - 7 * 864e5)) + '"></div>' +
            '<div class="pr-f"><label>To Date</label><input type="date" id="hTo" value="' + iso(now) + '"></div>' +
            '<button class="pr-btn" id="btnShowH" type="button">Show</button>';
        $('btnShowP').onclick = loadPending; $('btnShowH').onclick = loadHistory;
    }

    function loadPending() {
        msg('');
        var d = S.init;
        post(API + '/pending', {
            fromDate: $('pFrom').value, toDate: $('pTo').value, fromDocNo: $('pFD').value, toDocNo: $('pTD').value,
            productionNo: ($('pPN').value || '').trim(), bomId: pick('pBom', d.boms), itemId: pick('pItem', d.items),
            plantId: pick('pPlant', d.plants), productionTypeId: pick('pType', d.productionTypes)
        }).then(function (rows) {
            S.pending = rows; S.sel = -1;
            $('gridP').innerHTML = table(PCOLS, rows, { check: true, editDate: true, selectable: true, extra: [['Print', 'print', 'Print']] });
            $('gridIn').innerHTML = ''; $('gridOut').innerHTML = '';
            if (!rows.length) msg('No record found.');
        }).catch(function (e) { msg(e.message); });
    }
    function loadDetail(i) {
        var r = S.pending[i]; if (!r) return;
        S.sel = i;
        [].forEach.call($('gridP').querySelectorAll('tbody tr'), function (tr) { tr.classList.toggle('sel', Number(tr.getAttribute('data-i')) === i); });
        fetchJson(API + '/detail/' + r.Id).then(function (d) {
            $('gridIn').innerHTML = table(INCOLS, d.input); $('gridOut').innerHTML = table(OUTCOLS, d.output);
        }).catch(function (e) { msg(e.message); });
    }
    function loadHistory() {
        msg('');
        post(API + '/history', { fromDate: $('hFrom').value, toDate: $('hTo').value }).then(function (rows) {
            S.hist = rows;
            $('gridH').innerHTML = table(HCOLS, rows, { extra: [['', 'del', 'X'], ['Voucher', 'voucher', 'Voucher'], ['Ref Print', 'refprint', 'Print']] });
            if (!rows.length) msg('No record found.');
        }).catch(function (e) { msg(e.message); });
    }
    function slip(id) {
        if (window.printRpt) window.printRpt('1850_Production_Slip.rpt', { id: id, branchesId: 0, documentTypeId: 1850 });
        else msg('Print is not available.');
    }
    function save() {
        var rows = [];
        [].forEach.call($('gridP').querySelectorAll('.chk'), function (c) {
            if (!c.checked) return;
            var i = Number(c.getAttribute('data-i')), dd = $('gridP').querySelector('.dd[data-i="' + i + '"]');
            rows.push({ id: S.pending[i].Id, docDate: dd ? dd.value : '' });
        });
        if (!rows.length) { msg('Please select check box first'); return; }
        $('btnSave').disabled = true;
        post(API + '/save', { rows: rows }).then(function (d) { msg(d.message, true); loadPending(); })
            .catch(function (e) { msg(e.message); }).then(function () { $('btnSave').disabled = false; });
    }

    function tab(h) {
        $('paneP').classList.toggle('is-hidden', h); $('paneH').classList.toggle('is-hidden', !h);
        $('tabPending').classList.toggle('on', !h); $('tabHistory').classList.toggle('on', h);
        if (h) loadHistory();
    }

    document.addEventListener('DOMContentLoaded', function () {
        fetchJson(API + '/init').then(function (d) { S.init = d; buildFilters(d); $('btnSave').style.display = d.canSave === false ? 'none' : ''; })
            .catch(function (e) { msg(e.message); });
        $('tabPending').onclick = function () { tab(false); }; $('tabHistory').onclick = function () { tab(true); };
        $('btnSave').onclick = save;
        $('gridP').addEventListener('change', function (e) {
            if (e.target.id === 'chkAll') [].forEach.call($('gridP').querySelectorAll('.chk'), function (c) { c.checked = e.target.checked; });
        });
        $('gridP').addEventListener('click', function (e) {
            var a = e.target.closest('a[data-act]');
            if (a) { var r = S.pending[Number(a.getAttribute('data-i'))]; if (r) slip(r.Id); return; }
            var tr = e.target.closest('tbody tr'); if (tr && !e.target.closest('input')) loadDetail(Number(tr.getAttribute('data-i')));
        });
        $('gridH').addEventListener('click', function (e) {
            var a = e.target.closest('a[data-act]'); if (!a) return;
            var r = S.hist[Number(a.getAttribute('data-i'))]; if (!r) return;
            var act = a.getAttribute('data-act');
            if (act === 'del') {
                if (!window.confirm('Are you sure to Delete?')) return;
                post(API + '/delete/' + r.Id).then(function (d) { msg(d.message, true); loadHistory(); }).catch(function (x) { msg(x.message); });
            } else if (act === 'refprint') slip(r.RefDocId);
            else if (act === 'voucher') {
                var win = window.CrystalPrint ? CrystalPrint.reserve() : null;
                fetchJson(API + '/voucher-head/' + r.Id).then(function (d) {
                    var vh = Number(d && d.voucherHeadId) || 0;
                    if (!vh) { if (window.CrystalPrint) CrystalPrint.release(win); msg('VoucherId Not Found'); return; }
                    CrystalPrint.open('acc-118', { id: vh, documentTypeId: 1857 }, null, win);
                }).catch(function (x) { if (window.CrystalPrint) CrystalPrint.release(win); msg(x.message); });
            }
        });
    });
}());
