/* ============================================================================
 * Order Management grids - 363 frmOrderManagement, 366 OrderManagementDashboardForPurchase,
 * 367 OrderManagementDashboardForSale.
 *
 *   frmOrderManagement.cs            :31-86   Load -> two grids, rows split by TranType
 *                                    :88-148  GridSettingPurchase / GridSettingSale (layout from the .resx)
 *   OrderManagementDashboardFor*.cs  :64-107  GetDataAndGenerateCards, one OrderManageMentGrids per CardCount
 *                                    :109-119 BtnRefresh_Click
 *   DynamicCards\OrderManageMentGrids.cs
 *                                    :60-90   dt columns, caption = column name (RetrieveStructure)
 *                                    :92-140  GridSetting (formats and aggregates)
 *                                    :142-190 grd_LinkClicked: ItemName -> frmStockReport,
 *                                             OrderCount -> frmSaleOrderHistory / PurchaseOrderHistory
 * Grid features carried: FilterMode automatic (filter row), TotalRow (Sum / Average), column sort.
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/w4-order-management/data';
var ORDERS = '/api/dashboard/order-management/orders';   // OrderManagemantPopUpForPurchaseSale data (already ported)
var MODE = document.body.getAttribute('data-w4-mode') || 'tabs';
var S = { purchase: [], sales: [], tab: 'purchase', busy: false, grids: [] };

function el(id) { return document.getElementById(id); }
function txt(v) { return v === null || v === undefined ? '' : String(v); }
function esc(v) { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* .NET "#,##0.##" / "#,##0.###": grouped, at most N decimals, no trailing zeros, zero is "0" */
function fmtN(v, dec) {
    var n = num(v);
    var s = Math.abs(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec });
    return (n < 0 && s !== '0') ? '-' + s : s;
}
/* .NET "#,#": integer, grouped; a zero formats to the empty string */
function fmtCount(v) {
    var n = Math.round(num(v));
    return n === 0 ? '' : n.toLocaleString('en-US');
}

function msg(text, ok) {
    var m = el('w4Message');
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
}
function clearMsg() { var m = el('w4Message'); m.className = ''; m.textContent = ''; }
function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } }).then(function (r) {
        if (!r.ok) throw new Error('HTTP ' + r.status + ' on ' + url);
        return r.json();
    });
}

/* ------------------------------------------------------------------ the grid */
/* cols: { k, c (caption), f (formatter), agg: 'sum'|'avg', link: 'item'|'orders' } */
function Grid(host, cols, rows, onLink) {
    var self = this;
    this.host = host; this.cols = cols; this.rows = rows; this.onLink = onLink;
    this.filters = {}; this.sort = null; this.dir = 1; this.view = rows.slice();
    host.addEventListener('click', function (e) {
        var th = e.target.closest ? e.target.closest('th[data-k]') : null;
        if (th) { var k = th.getAttribute('data-k'); if (self.sort === k) self.dir = -self.dir; else { self.sort = k; self.dir = 1; } self.apply(); return; }
        var a = e.target.closest ? e.target.closest('[data-link]') : null;
        if (a && self.onLink) self.onLink(a.getAttribute('data-link'), self.view[parseInt(a.getAttribute('data-i'), 10)]);
    });
    host.addEventListener('input', function (e) {
        if (e.target && e.target.getAttribute('data-f')) {
            self.filters[e.target.getAttribute('data-f')] = e.target.value;
            self.apply(true);
        }
    });
    this.build();
    this.apply();
}
Grid.prototype.shown = function (c, r) {
    var v = r[c.k];
    return c.f ? c.f(v) : txt(v);
};
Grid.prototype.build = function () {
    var head = this.cols.map(function (c) { return '<th data-k="' + c.k + '">' + esc(c.c) + '</th>'; }).join('');
    var flt = this.cols.map(function (c) { return '<td><input type="text" data-f="' + c.k + '"></td>'; }).join('');
    this.host.innerHTML = '<div class="w4-scroll"><table class="w4-grid"><thead><tr>' + head + '</tr><tr class="w4-flt">' + flt
        + '</tr></thead><tbody></tbody><tfoot></tfoot></table></div><div class="w4-nav"></div>';
};
Grid.prototype.apply = function (keepFocus) {
    var self = this;
    var keys = Object.keys(this.filters).filter(function (k) { return txt(self.filters[k]).trim() !== ''; });
    this.view = this.rows.filter(function (r) {
        return keys.every(function (k) {
            var c = self.cols.filter(function (x) { return x.k === k; })[0];
            return self.shown(c, r).toLowerCase().indexOf(txt(self.filters[k]).trim().toLowerCase()) >= 0;
        });
    });
    if (this.sort) {
        var k = this.sort, d = this.dir;
        this.view = this.view.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
            var x = a.r[k], y = b.r[k], c;
            c = (typeof x === 'number' && typeof y === 'number') ? x - y : txt(x).localeCompare(txt(y), undefined, { numeric: true, sensitivity: 'base' });
            return (c || a.i - b.i) * d;
        }).map(function (o) { return o.r; });
    }
    this.paint();
};
Grid.prototype.paint = function () {
    var cols = this.cols, view = this.view, tb = this.host.querySelector('tbody'), tf = this.host.querySelector('tfoot');
    tb.innerHTML = view.map(function (r, i) {
        return '<tr>' + cols.map(function (c) {
            var v = r[c.k], shown = c.f ? c.f(v) : esc(v);
            var numeric = !!c.f;
            var cls = (numeric ? 'w4-num' : '') + (numeric && num(v) < 0 ? ' w4-neg' : '');
            if (c.link) shown = '<span class="w4-link" data-link="' + c.link + '" data-i="' + i + '">' + (shown === '' ? '&nbsp;&nbsp;' : shown) + '</span>';
            return '<td class="' + cls + '">' + shown + '</td>';
        }).join('') + '</tr>';
    }).join('');
    var any = cols.some(function (c) { return c.agg; });
    tf.innerHTML = any ? '<tr>' + cols.map(function (c, idx) {
        if (!c.agg) return '<td style="text-align:left;">' + (idx === 0 ? 'Total' : '') + '</td>';
        var t = 0;
        view.forEach(function (r) { t += num(r[c.k]); });
        if (c.agg === 'avg') t = view.length ? t / view.length : 0;
        return '<td>' + (c.f ? c.f(t) : t) + '</td>';
    }).join('') + '</tr>' : '';
    this.host.querySelector('.w4-nav').textContent = 'Record ' + (view.length ? 1 : 0) + ' of ' + view.length;
};

/* ------------------------------------------------------------------ column sets */
function f2(v) { return fmtN(v, 2); }
function f3(v) { return fmtN(v, 3); }

/* OrderManageMentGrids.cs :60-140 - the caption is the column name, RetrieveStructure() */
var CARD_COLS = [
    { k: 'itemName',    c: 'ItemName',    link: 'item' },
    { k: 'orderCount',  c: 'OrderCount',  f: fmtCount, agg: 'sum', link: 'orders' },
    { k: 'orderQty',    c: 'OrderQty',    f: f2, agg: 'sum' },
    { k: 'orderWeight', c: 'OrderWeight', f: f3, agg: 'sum' },
    { k: 'avgRate40Kg', c: 'AvgRate40Kg', f: f3, agg: 'avg' },
    { k: 'orderAmount', c: 'OrderAmount', f: f3, agg: 'sum' },
    { k: 'stockQty',    c: 'StockQty',    f: f2, agg: 'sum' },
    { k: 'stockWeight', c: 'StockWeight', f: f3, agg: 'sum' }
];
/* frmOrderManagement.resx layout captions; TranType, CardCount, SortNo, ActivityDescription, CategoryDescription,
   OrderCount and AvgRate40Kg are hidden (GridSettingPurchase :55-61). All aggregates are Sum, "#,##0.###". */
var TAB_COLS = [
    { k: 'itemName',    c: 'Item Name' },
    { k: 'orderQty',    c: 'Order Qty',    f: f3, agg: 'sum' },
    { k: 'orderWeight', c: 'Order Weight', f: f3, agg: 'sum' },
    { k: 'stockQty',    c: 'Stock Qty',    f: f3, agg: 'sum' },
    { k: 'stockWeight', c: 'Stock Weight', f: f3, agg: 'sum' },
    { k: 'orderAmount', c: 'Order Amount', f: f3, agg: 'sum' }
];

/* ------------------------------------------------------------------ render */
function groupCards(rows) {
    /* one card per CardCount (dense rank within TranType), title = that group's first ActivityDescription (:73-96) */
    var out = [], idx = {};
    rows.forEach(function (r) {
        var k = r.cardCount;
        if (idx[k] === undefined) { idx[k] = out.length; out.push({ title: r.title, rows: [] }); }
        out[idx[k]].rows.push(r);
    });
    out.sort(function (a, b) { return a.rows[0].cardCount - b.rows[0].cardCount; });
    return out;
}

function render() {
    var body = el('w4Body'), empty = el('w4Empty');
    S.grids = [];
    if (MODE === 'tabs') {
        var rows = S.tab === 'sales' ? S.sales : S.purchase;
        body.innerHTML = '<div class="w4-card" style="margin:8px;"><div class="w4-card-title" style="text-align:left;">'
            + (S.tab === 'sales' ? 'Sale Orders' : 'Purchase Orders') + '</div><div id="w4Tab"></div></div>';
        S.grids.push(new Grid(el('w4Tab'), TAB_COLS, rows, null));
        empty.style.display = rows.length ? 'none' : '';
        empty.textContent = 'No rows.';
        return;
    }
    var list = MODE === 'sales' ? S.sales : S.purchase;
    var cards = groupCards(list);
    body.innerHTML = '<div class="w4-wrap">' + cards.map(function (c, i) {
        return '<div class="w4-card"><div class="w4-card-title">' + esc(c.title) + '</div><div id="w4Card' + i + '"></div></div>';
    }).join('') + '</div>';
    cards.forEach(function (c, i) {
        S.grids.push(new Grid(el('w4Card' + i), CARD_COLS, c.rows, onLink));
    });
    empty.style.display = cards.length ? 'none' : '';
    empty.textContent = 'No rows.';
}

/* OrderManageMentGrids.grd_LinkClicked :142-190 */
function onLink(kind, row) {
    if (!row || !(row.itemId > 0)) return;                       // ItemId > 0 guard on both links
    if (kind === 'item') {                                       // frmStockReport with the item preselected
        window.open('/inventory/stock-report?itemId=' + row.itemId, '_blank');
        return;
    }
    openOrders(MODE === 'sales' ? 2 : 1, row.itemId, row.itemName);
}

/* The desktop opens frmSaleOrderHistory (Open / All) or PurchaseOrderHistory (Open / approved only) with the item set.
   Those two history forms are not ported; the open orders of the item come from the already-ported order pop-up. */
var P_COLS = [['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo'], ['SupplierName', 'SupplierName'], ['DeliveryTerm', 'DeliveryTerm'],
    ['ItemName', 'ItemName'], ['Crop', 'Crop'], ['PackUom', 'PackUom'], ['Qty', 'Qty', 'n'], ['RcvdQty', 'RcvdQty', 'n'], ['RejQty', 'RejQty', 'n'],
    ['BalQty', 'BalQty', 'n'], ['Weight', 'Weight', 'n'], ['RcvdWeight', 'RcvdWeight', 'n'], ['BalWeight', 'BalWeight', 'n'], ['Rate', 'Rate', 'r'], ['ExpiryDate', 'ExpiryDate', 'd']];
var S_COLS = [['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo'], ['CustomerName', 'CustomerName'], ['DeliveryTerm', 'DeliveryTerm'],
    ['ItemName', 'ItemName'], ['Crop', 'Crop'], ['PackUom', 'PackUom'], ['Qty', 'Qty', 'n'], ['DispatchQty', 'DispatchQty', 'n'],
    ['BalQty', 'BalQty', 'n'], ['Weight', 'Weight', 'n'], ['DispatchWeight', 'DispatchWeight', 'n'], ['BalWeight', 'BalWeight', 'n'], ['Rate', 'Rate', 'r'], ['ExpiryDate', 'ExpiryDate', 'd']];
var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
function fd(v) {
    var s = txt(v), y = parseInt(s.substr(0, 4), 10), m = parseInt(s.substr(5, 2), 10), d = parseInt(s.substr(8, 2), 10);
    if (!y || !m || !d || y <= 1900) return '';
    return (d < 10 ? '0' + d : d) + '-' + MON[m - 1] + '-' + String(y).slice(-2);
}
function pick(row, key) {
    if (row[key] !== undefined) return row[key];
    var l = key.toLowerCase();
    for (var k in row) if (k.toLowerCase() === l) return row[k];
    return null;
}
function openOrders(invoiceType, itemId, itemName) {
    var modal = el('w4Modal'), grid = el('w4ModalGrid');
    el('w4ModalTitle').textContent = (invoiceType === 1 ? 'Purchase Orders - ' : 'Sale Orders - ') + itemName;
    el('w4ModalCount').textContent = '';
    grid.innerHTML = '<tbody><tr><td class="w4-empty">Reading...</td></tr></tbody>';
    modal.classList.add('open');
    getJson(ORDERS + '?invoiceType=' + invoiceType + '&itemId=' + itemId).then(function (d) {
        if (d && d.error) { grid.innerHTML = '<tbody><tr><td class="w4-empty">' + esc(d.error) + '</td></tr></tbody>'; return; }
        var rows = (d && d.rows) || [], cols = invoiceType === 1 ? P_COLS : S_COLS;
        el('w4ModalCount').textContent = rows.length + ' order(s)';
        if (!rows.length) { grid.innerHTML = '<tbody><tr><td class="w4-empty">No open orders for this item.</td></tr></tbody>'; return; }
        var head = '<thead><tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr></thead>';
        var tb = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) {
                var v = pick(r, c[0]);
                if (c[2] === 'd') return '<td>' + fd(v) + '</td>';
                if (c[2] === 'n') return '<td class="w4-num">' + fmtN(v, 0) + '</td>';
                if (c[2] === 'r') return '<td class="w4-num">' + num(v).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        grid.innerHTML = head + '<tbody>' + tb + '</tbody>';
    }).catch(function (e) { grid.innerHTML = '<tbody><tr><td class="w4-empty">' + esc(e.message) + '</td></tr></tbody>'; });
}

/* ------------------------------------------------------------------ load */
function load() {
    if (S.busy) return;
    S.busy = true;
    clearMsg();
    var b = el('w4Refresh');
    b.disabled = true; b.classList.add('w4-btn-busy');
    var empty = el('w4Empty'); empty.style.display = ''; empty.textContent = 'Loading...';
    getJson(API).then(function (d) {
        S.busy = false; b.disabled = false; b.classList.remove('w4-btn-busy');
        if (d && d.error) msg(d.error, false);
        S.purchase = (d && d.purchase) || [];
        S.sales = (d && d.sales) || [];
        render();
    }).catch(function (e) {
        S.busy = false; b.disabled = false; b.classList.remove('w4-btn-busy');
        msg('Order management failed: ' + e.message, false);
        empty.style.display = ''; empty.textContent = 'Could not be read.';
    });
}

document.addEventListener('DOMContentLoaded', function () {
    el('w4Refresh').addEventListener('click', load);               // BtnRefresh_Click / toolbar refresh
    el('w4ModalClose').addEventListener('click', function () { el('w4Modal').classList.remove('open'); });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') el('w4Modal').classList.remove('open');
    });
    var tabs = el('w4Tabs');
    if (tabs) tabs.addEventListener('click', function (e) {
        var t = e.target.getAttribute && e.target.getAttribute('data-tab');
        if (!t) return;
        S.tab = t;
        Array.prototype.forEach.call(tabs.querySelectorAll('button'), function (x) { x.classList.toggle('on', x === e.target); });
        render();
    });
    load();                                                         // Load -> GetDataAndGenerateCards
});

})();
