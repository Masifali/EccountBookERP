/* ============================================================================
 * 1001 Order Management Dashboard
 * Ported from Architecture.WinApp.Dashboard\OrderManagementDashboard.cs (283 lines),
 * OrderManageMentCardHeader.cs and OrderManagementInfoCard.cs.
 *
 *   :53-63    Load -> GetDataAndGenerateCards()
 *   :65-128   SPU_Invetnory_OrderManagementDashBoards, split by TranType, grouped by CardCount
 *   header :78-99   the five numeric labels, all formatted "#,#;(#,#);0"
 *   header :107-148 the two clickable labels and the rights check behind StockWeight
 * ==========================================================================*/
(function () {
'use strict';

var API = '/api/dashboard/order-management';

var S = { purchase: [], sales: [], busy: false };

function el(id)  { return document.getElementById(id); }
function val(id) { var e = el(id); return e ? e.value : ''; }
function setVal(id, v) { var e = el(id); if (e) e.value = v === null || v === undefined ? '' : v; }
function txt(v)  { return v === null || v === undefined ? '' : String(v); }
function esc(v)  { return txt(v).replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
function num(v)  { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

/* Janus / .NET "#,#;(#,#);0" - thousands, no decimals, negatives in parentheses, bare 0 for
   zero. The five numeric labels on the card all use it (header :85-93). */
function fInt(v) {
    var n = num(v);
    if (n === 0) return '0';
    var a = Math.abs(n).toLocaleString(undefined, { minimumFractionDigits: 0, maximumFractionDigits: 0 });
    return n < 0 ? '(' + a + ')' : a;
}
/* NoOfParties and NoOfOrders use a plain ToString() (header :94-97). */
function fPlain(v) {
    var n = num(v);
    return (n === Math.floor(n)) ? String(n) : String(n);
}

function today() {
    var d = new Date();
    return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2)
                           + '-' + ('0' + d.getDate()).slice(-2);
}

function msg(text, ok) {
    var m = el('omMessage');
    if (!m) return;
    m.className = ok ? 'ok' : 'err';
    m.textContent = text;
    if (ok) setTimeout(function () { if (m.textContent === text) m.className = ''; }, 6000);
}
function clearMsg() { var m = el('omMessage'); if (m) { m.className = ''; m.textContent = ''; } }

function busy(btn, on) {
    S.busy = !!on;
    if (btn) { btn.disabled = !!on; btn.classList.toggle('btn-busy', !!on); }
}

function getJson(url) {
    return fetch(url, { headers: { 'Accept': 'application/json' } })
        .then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status + ' on ' + url);
            return r.json();
        });
}

/* The seven labels on OrderManagementInfoCard, in the desktop's own order and wording
   (InitializeComponent :212-276). */
var COLS = [
    { k: 'itemName',    c: 'Item Name',      w: 220, link: 'item' },
    { k: 'orderQty',    c: 'Order Qty',      w: 90,  fmt: fInt },
    { k: 'orderWeight', c: 'Order Weight',   w: 100, fmt: fInt },
    { k: 'stockQty',    c: 'Stock Qty',      w: 90,  fmt: fInt },
    { k: 'stockWeight', c: 'Stock Weight',   w: 100, fmt: fInt, link: 'stock' },
    { k: 'noOfParties', c: 'No. Of Parties', w: 85,  fmt: fPlain },
    { k: 'noOfOrders',  c: 'No. Of Orders',  w: 85,  fmt: fPlain }
];

window.omShow = function (btn) {
    if (S.busy) return;
    clearMsg();
    busy(btn, true);
    var empty = el('omEmpty');
    if (empty) { empty.style.display = ''; empty.textContent = 'Loading...'; }

    getJson(API + '/cards?toDate=' + encodeURIComponent(val('toDate')))
    .then(function (d) {
        busy(btn, false);
        if (d && d.error) msg(d.error, false);
        S.purchase = (d && d.purchase) || [];
        S.sales    = (d && d.sales) || [];
        render('omPurchase', S.purchase, 'purchase');
        render('omSales',    S.sales,    'sales');
        var total = S.purchase.length + S.sales.length;
        if (empty) {
            empty.style.display = total ? 'none' : '';
            empty.textContent = 'No order management rows as at this date.';
        }
    }).catch(function (e) {
        busy(btn, false);
        msg('Order management dashboard failed: ' + e.message, false);
        if (empty) { empty.style.display = ''; empty.textContent = 'Could not be read.'; }
    });
};

function render(wrapId, cards, which) {
    var wrap = el(wrapId);
    if (!wrap) return;
    if (!cards.length) { wrap.innerHTML = ''; return; }

    wrap.innerHTML = cards.map(function (card, ci) {
        var head = COLS.map(function (c) {
            return '<th style="width:' + c.w + 'px;">' + esc(c.c) + '</th>';
        }).join('');

        var body = card.items.map(function (it, ii) {
            return '<tr>' + COLS.map(function (c) {
                var v = it[c.k];
                if (c.link === 'item') {
                    return '<td><span class="om-link" onclick="omItemClick(\'' + which + '\','
                         + ci + ',' + ii + ')">' + esc(v) + '</span></td>';
                }
                var shown = c.fmt ? c.fmt(v) : esc(v);
                var cls = 'num' + (c.fmt === fInt && num(v) < 0 ? ' neg' : '');
                if (c.link === 'stock') {
                    return '<td class="' + cls + '"><span class="om-link" onclick="omStockClick(\''
                         + which + '\',' + ci + ',' + ii + ')">' + shown + '</span></td>';
                }
                return '<td class="' + cls + '">' + shown + '</td>';
            }).join('') + '</tr>';
        }).join('');

        return '<div class="om-card">'
             + '<div class="om-card-head"><span>' + esc(card.title) + '</span>'
             + '<span>' + card.items.length + '</span></div>'
             + '<div class="om-scroll"><table class="om-grid">'
             + '<thead><tr>' + head + '</tr></thead><tbody>' + body + '</tbody>'
             + '</table></div></div>';
    }).join('');
}

function pick(which, ci, ii) {
    var list = which === 'sales' ? S.sales : S.purchase;
    var card = list[ci];
    if (!card) return null;
    return card.items[ii] || null;
}

/* header :117-133 - ItemName opens OrderManagemantPopUpForPurchaseSale with InvoiceType 1 for
   Purchase and 2 for Sales, and does nothing at all when ItemId <= 0 (:113). That popup is a
   separate desktop form (628 lines, driven by PurchaseOrderHistory / SaleOrderHistory) and has
   not been ported, so nothing unrelated is opened. */
window.omItemClick = function (which, ci, ii) {
    var it = pick(which, ci, ii);
    if (!it) return;
    if (num(it.itemId) <= 0) return;                 /* :113 - no item, nothing happens */
    var invoiceType = (which === 'sales') ? 2 : 1;   /* :71 / :86 */
    omOpenOrders(invoiceType, num(it.itemId), txt(it.itemName));
};

/* ===========================================================================
 * OrderManagemantPopUpForPurchaseSale.cs (628 lines), opened maximised by the
 * desktop. One form, two grids, chosen by InvoiceType:
 *
 *   1 Purchase  [fed].[usp_PurchaseOrder_History_Rpt]  DocumentTypeId 41,
 *               Status Open, IsApproved 1. Slip button prints 203.
 *   2 Sales     Sp_SalesSaleOrder_RiceAndPaddyRegister_Rpt, Status Open, and
 *               NO IsApproved - the form sets ApprovedFilter "All" (:91).
 *               Slip button prints 273.
 *
 * Both grids group by ItemName with GroupTotals = 2 (:163, :286), and both make
 * DocNo, the party name and ItemName link columns (ColumnType 5).
 * ======================================================================== */

/* GridSettingsForPurchaseOrder :155-223 - hidden: Id, SupCustId, PurchaseGLAC, ItemId.
   Sums: Qty, RcvdQty, RejQty, BalQty, Weight, RcvdWeight, BalWeight.
   "0,0" for the quantity and weight columns, "#,0.00" for Rate. */
var OM_PURCHASE_COLS = [
    { key: 'Slip',         cap: 'Slip203',      slip: true },
    { key: 'DocDate',      cap: 'DocDate',      type: 'd' },
    { key: 'DocNo',        cap: 'DocNo',        type: 's', link: 'doc' },
    { key: 'SupplierName', cap: 'SupplierName', type: 's', link: 'party' },
    { key: 'DeliveryTerm', cap: 'DeliveryTerm', type: 's' },
    { key: 'ItemName',     cap: 'ItemName',     type: 's', link: 'item' },
    { key: 'Crop',         cap: 'Crop',         type: 's' },
    { key: 'PackUom',      cap: 'PackUom',      type: 's' },
    { key: 'Qty',          cap: 'Qty',          type: 'n0', sum: true },
    { key: 'RcvdQty',      cap: 'RcvdQty',      type: 'n0', sum: true },
    { key: 'RejQty',       cap: 'RejQty',       type: 'n0', sum: true },
    { key: 'BalQty',       cap: 'BalQty',       type: 'n0', sum: true },
    { key: 'Weight',       cap: 'Weight',       type: 'n0', sum: true },
    { key: 'RcvdWeight',   cap: 'RcvdWeight',   type: 'n0', sum: true },
    { key: 'BalWeight',    cap: 'BalWeight',    type: 'n0', sum: true },
    { key: 'Rate',         cap: 'Rate',         type: 'n2' },
    { key: 'ExpiryDate',   cap: 'ExpiryDate',   type: 'd' }
];

/* GridSettingsForSale :264-329 - hidden: Id, SupCustId, SaleGLAC, ItemId.
   Rate is summed here as well (:308 sets its TotalFormatString), unlike the purchase grid. */
var OM_SALE_COLS = [
    { key: 'Slip',           cap: 'Slip273',        slip: true },
    { key: 'DocDate',        cap: 'DocDate',        type: 'd' },
    { key: 'DocNo',          cap: 'DocNo',          type: 's', link: 'doc' },
    { key: 'CustomerName',   cap: 'CustomerName',   type: 's', link: 'party' },
    { key: 'DeliveryTerm',   cap: 'DeliveryTerm',   type: 's' },
    { key: 'ItemName',       cap: 'ItemName',       type: 's', link: 'item' },
    { key: 'Crop',           cap: 'Crop',           type: 's' },
    { key: 'PackUom',        cap: 'PackUom',        type: 's' },
    { key: 'Qty',            cap: 'Qty',            type: 'n0', sum: true },
    { key: 'DispatchQty',    cap: 'DispatchQty',    type: 'n0', sum: true },
    { key: 'BalQty',         cap: 'BalQty',         type: 'n0', sum: true },
    { key: 'Weight',         cap: 'Weight',         type: 'n0', sum: true },
    { key: 'DispatchWeight', cap: 'DispatchWeight', type: 'n0', sum: true },
    { key: 'BalWeight',      cap: 'BalWeight',      type: 'n0', sum: true },
    { key: 'Rate',           cap: 'Rate',           type: 'n2' },
    { key: 'ExpiryDate',     cap: 'ExpiryDate',     type: 'd' }
];

var OM_POPUP = { rows: [], cols: [], invoiceType: 1 };

var OM_MONTHS = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];

/* Purchase uses ToShortDateString for both dates (:141); Sales uses ToShortDateString for DocDate
   and "dd-MMM-yy" for ExpiryDate (:251). Rendered as dd-MMM-yy throughout, which is unambiguous
   and matches the desktop's own expiry format. */
function omDate(v) {
    var s = txt(v);
    if (!s) return '';
    var y = parseInt(s.substr(0, 4), 10),
        mo = parseInt(s.substr(5, 2), 10),
        d = parseInt(s.substr(8, 2), 10);
    if (!y || !mo || !d || y <= 1900) return '';
    return (d < 10 ? '0' + d : d) + '-' + OM_MONTHS[mo - 1] + '-' + String(y).slice(-2);
}
/* "0,0" - thousands, no decimals. */
function omN0(v) { return num(v).toLocaleString(undefined, { maximumFractionDigits: 0 }); }
/* "#,0.00" - thousands, always two decimals. */
function omN2(v) {
    return num(v).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
function omFmt(v, type) {
    if (type === 'n0') return omN0(v);
    if (type === 'n2') return omN2(v);
    if (type === 'd')  return omDate(v);
    return esc(txt(v));
}
function omIsNum(type) { return type === 'n0' || type === 'n2'; }

function omOpenOrders(invoiceType, itemId, itemName) {
    var modal = el('omModal'), grid = el('omModalGrid'),
        head = el('omModalTitle'), count = el('omModalCount');
    if (!modal || !grid) return;

    OM_POPUP.invoiceType = invoiceType;
    OM_POPUP.cols = (invoiceType === 1) ? OM_PURCHASE_COLS : OM_SALE_COLS;
    if (head) head.textContent = (invoiceType === 1 ? 'Purchase Orders - ' : 'Sale Orders - ') + itemName;
    if (count) count.textContent = '';
    grid.innerHTML = '<tbody><tr><td class="om-empty">Reading...</td></tr></tbody>';
    modal.classList.add('open');

    getJson(API + '/orders?invoiceType=' + invoiceType + '&itemId=' + itemId)
    .then(function (d) {
        d = d || {};
        if (d.error) {
            grid.innerHTML = '<tbody><tr><td class="om-empty">' + esc(d.error) + '</td></tr></tbody>';
            return;
        }
        OM_POPUP.rows = d.rows || [];
        if (count) count.textContent = OM_POPUP.rows.length + ' order(s)';
        omRenderPopup();
    }).catch(function (e) {
        grid.innerHTML = '<tbody><tr><td class="om-empty">' + esc(e.message) + '</td></tr></tbody>';
    });
}

function omRenderPopup() {
    var grid = el('omModalGrid');
    if (!grid) return;
    var cols = OM_POPUP.cols, rows = OM_POPUP.rows;

    if (!rows.length) {
        /* :83-89 and :105-110 - an empty read clears the grid; the desktop shows nothing else. */
        grid.innerHTML = '<tbody><tr><td class="om-empty">No open orders for this item.</td></tr></tbody>';
        return;
    }

    var head = '<thead><tr>' + cols.map(function (c) {
        return '<th>' + esc(c.cap) + '</th>';
    }).join('') + '</tr></thead>';

    /* Groups.Add("ItemName") with GroupTotals = 2 - a band per item, each with its own totals. */
    var body = '', lastGroup = null, bucket = [];

    function flush() {
        if (!bucket.length) return;
        body += '<tr class="om-subtotal">' + cols.map(function (c, idx) {
            if (!c.sum) return '<td>' + (idx === 0 ? 'Group Total' : '') + '</td>';
            var t = 0;
            bucket.forEach(function (r) { t += num(f(r, c.key)); });
            return '<td class="num">' + omFmt(t, c.type) + '</td>';
        }).join('') + '</tr>';
        bucket = [];
    }

    rows.forEach(function (row, i) {
        var g = txt(f(row, 'ItemName'));
        if (g !== lastGroup) { flush(); lastGroup = g;
            body += '<tr class="om-group"><td colspan="' + cols.length + '">' + esc(g) + '</td></tr>'; }
        bucket.push(row);
        body += '<tr>' + cols.map(function (c) {
            if (c.slip) {
                return '<td><button type="button" class="om-slip" onclick="omSlip(' + i + ')">'
                     + esc(c.cap) + '</button></td>';
            }
            var raw = f(row, c.key);
            var cls = omIsNum(c.type) ? 'num' : '';
            if (omIsNum(c.type) && num(raw) < 0) cls += ' neg';
            var inner = omFmt(raw, c.type);
            if (c.link) inner = '<span class="om-link" onclick="omPopupLink(\'' + c.link + '\',' + i + ')">'
                              + inner + '</span>';
            return '<td class="' + cls + '">' + inner + '</td>';
        }).join('') + '</tr>';
    });
    flush();

    var foot = '<tfoot class="om-foot"><tr>' + cols.map(function (c, idx) {
        if (!c.sum) return '<td>' + (idx === 0 ? 'Total' : '') + '</td>';
        var t = 0;
        rows.forEach(function (r) { t += num(f(r, c.key)); });
        return '<td class="num">' + omFmt(t, c.type) + '</td>';
    }).join('') + '</tr></tfoot>';

    grid.innerHTML = head + '<tbody>' + body + '</tbody>' + foot;
}

/* grd_LinkClicked, :331-368.
     DocNo        -> ActivityDetails (purchase) / SaleActivityDetails (sale)  - NOT ported
     party name   -> GoToCustomerLedgerFromLinkedEvent                        - ported
     ItemName     -> GoToItemEvaluationLedgerFromLinkedEvent                  - ported
   The two ported targets navigate; the unported one reports what it would open. */
window.omPopupLink = function (kind, i) {
    var row = OM_POPUP.rows[i];
    if (!row) return;

    if (kind === 'party') {
        var supCustId = num(f(row, 'SupCustId'));
        if (!supCustId) return;
        window.open('/accounts/reports/customer-ledger?supplierCustomerId=' + supCustId, '_blank');
        return;
    }
    if (kind === 'item') {
        var itemId = num(f(row, 'ItemId'));
        if (!itemId) return;
        window.open('/stocks/item-ledger?itemId=' + itemId, '_blank');
        return;
    }
    /* DocNo */
    var form = (OM_POPUP.invoiceType === 1) ? 'ActivityDetails.cs' : 'SaleActivityDetails.cs';
    msg('Document ' + txt(f(row, 'DocNo')) + ' opens ' + form + ' on the desktop (order Id '
      + num(f(row, 'Id')) + '). That screen is not ported yet, so nothing was opened.', false);
};

/* grd_ColumnButtonClick, :484-504 - Slip203 for purchase, Slip273 for sale. Neither report is
   ported, so the button names the report and the order it would print. */
window.omSlip = function (i) {
    var row = OM_POPUP.rows[i];
    if (!row) return;
    var report = (OM_POPUP.invoiceType === 1) ? '203 (Purchase Order Slip)' : '273 (Sale Order Slip)';
    msg('Slip ' + report + ' for order Id ' + num(f(row, 'Id')) + ' is not ported yet.', false);
};

window.omCloseModal = function () {
    var m = el('omModal');
    if (m) m.classList.remove('open');
};

document.addEventListener('keydown', function (e) {
    if (e.key !== 'Escape') return;
    var m = el('omModal');
    if (m && m.classList.contains('open')) { e.preventDefault(); omCloseModal(); }
});

/* header :134-146 - StockWeight opens frmStockReportWithValues, but only when the weight is
   above zero AND the user holds the View right on RptGenrateStocks or frmStockReportWithValues;
   otherwise the desktop says "You don't have the right". That form is not ported, and the
   rights check is not something to work around, so the click reports both facts. */
window.omStockClick = function (which, ci, ii) {
    var it = pick(which, ci, ii);
    if (!it) return;
    if (num(it.itemId) <= 0) return;
    if (num(it.stockWeight) <= 0) return;      // :134 - zero weight does nothing
    msg('Stock weight opens the desktop form frmStockReportWithValues.cs, which has not been '
      + 'ported yet. On the desktop it is also behind the View right on RptGenrateStocks or '
      + 'frmStockReportWithValues.', false);
};

window.omToggleFullscreen = function () {
    document.body.classList.toggle('om-full');
};

/* ---------------------------------------------------------------- boot */
function boot() {
    /* The desktop sends DateTime.Now (:78) and binds the cards on load (:53-63). */
    setVal('toDate', today());
    var b = el('btnShow');
    if (b) omShow(b);
}

if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
else boot();

})();
