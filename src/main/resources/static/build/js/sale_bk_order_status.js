/* ============================================================================================
 * Screen 762 OrderDashboard - Architecture.WinApp.PreBookingAndDelivery.MyOrdersStatus (.cs), "My Order's Status"
 * Page: templates/sale/bk/order_status.html (route /sale/reports/order-dashboard).
 *   MyOrdersStatus_Load :120         fromdate = Start_Period; AppId 5: cost center label + combo shown, first row activated, combo disabled; else hidden; then btnShow_Click
 *   btnShow_Click :184               cards: Caption = Activity, Value = Total "#,##0.##" (usp_PreBookingOrder_Dashboard)
 *   UserControl_Clicked :210         Booking / Demand and Pending Orders show the summary boxes + tabs; Delivery In Transit and Sales Bill zero the boxes and show the grid
 *   GetBookingDemandData :258 / GetPendingOrdersData :538   six grids (Item, Customer, Item_Customer, Customer_Item, Item_Pack, Pack_Item); first tab selected; group totals Always
 *   GetDeliveryInTransitData :819 / GetSaleBillsData :930   grdSaleOrTransit with Slip + Confirm buttons at positions 0 / 1, FrozenColumns 2, CustomerRemarks editable
 *   grdSaleOrTransit_ColumnButtonClick :1035   Slip -> InvGdnSlip260(Id); Confirm -> remarks required, GdnStatusUpdate, reload; SlipBill 95 -> SaleInvoicetSlip_301, 99 -> SaleInvoiceDirectPartySlip_294;
 *                                    ConfirmBill -> InvoiceStatusUpdate, reload
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/bk/order-status';
    var info = { amountDecimals: 0, yearStart: null, appId: 0, costCenters: [] };
    var openedAt = null;                       // the time of day the DateTimePickers hold (Value keeps its time part)
    var Q3 = '#,##0.###';

    function two(n) { return ('0' + n).slice(-2); }
    function nowTime() { var u = new Date(); return two(u.getHours()) + ':' + two(u.getMinutes()) + ':' + two(u.getSeconds()); }

    // ------------------------------------------------------------------ the six tab grids (A.Grid = Janus GridEX look)
    function numCol(key, fmtFn, width) { return { key: key, caption: key, width: width, type: 'amount', sum: true, align: 'r', fmt: function (v) { return v === null || v === undefined || v === '' ? '' : S.fmtNum(v, fmtFn()); } }; }
    function q() { return Q3; }
    function single() { return S.fmtSingle(info.amountDecimals); }
    function tail() { return [numCol('Qty', q, 100), numCol('Weight', q, 110), numCol('Amount', single, 110)]; }
    function hid(key) { return { key: key, caption: key, hidden: true }; }
    function txt(key, width) { return { key: key, caption: key, width: width }; }
    var DEFS = {
        Item: { group: null, cols: function () { return [hid('Id'), txt('ItemName', 350)].concat(tail()); } },
        Customer: { group: null, cols: function () { return [hid('Id'), txt('CustomerName', 350)].concat(tail()); } },
        ItemCustomer: { group: 'ItemName', cols: function (booking) { return [hid('ItemId'), hid('ItemName'), hid('SuppCustId'), txt('CustomerName', 350)].concat(tail()); } },
        CustomerItem: { group: 'CustomerName', cols: function () { return [hid('ItemId'), hid('SuppCustId'), hid('CustomerName'), txt('ItemName', 350)].concat(tail()); } },
        ItemPack: { group: 'ItemName', cols: function () { return [hid('ItemId'), hid('ItemName'), hid('UomId'), txt('PackUom', 350)].concat(tail()); } },
        PackItem: { group: 'PackUom', cols: function () { return [hid('ItemId'), txt('ItemName', 350), hid('UomId'), hid('PackUom')].concat(tail()); } }
    };
    var grids = {};
    Object.keys(DEFS).forEach(function (n) {
        var g = new A.Grid({ tableId: 'tbl' + n, gridId: 'grd' + n, navId: 'nav' + n, navTextId: 'navText' + n, groupBy: DEFS[n].group, groupTotals: !!DEFS[n].group, decimals: function () { return info.amountDecimals; } });
        g.totalsRow = function (cls, rows, tag) {                          // AggregateFunction Sum with TotalFormatString of the column
            var tr = d.createElement('tr'); tr.className = cls;
            this.visibleCols().forEach(function (c, i) {
                var td = d.createElement('td');
                if (c.sum) {
                    var s = 0; rows.forEach(function (r) { s += A.toDouble(A.ci(r, c.key)); });
                    td.className = 'num'; td.textContent = S.fmtNum(s, c.key === 'Amount' ? single() : Q3);
                } else if (i === 0 && tag) td.textContent = tag;
                tr.appendChild(td);
            });
            return tr;
        };
        grids[n] = g;
    });
    function fillGrid(n, rows) {
        if (rows && rows.length) grids[n].setData(DEFS[n].cols(), rows); else grids[n].clear();
    }
    function clearAll() { Object.keys(grids).forEach(function (n) { grids[n].clear(); }); }

    function selectTab(i) {
        Array.prototype.forEach.call(d.querySelectorAll('#tabStrip button'), function (b) { b.classList.toggle('on', Number(b.getAttribute('data-tab')) === i); });
        Array.prototype.forEach.call(d.querySelectorAll('.tabpage'), function (p) { p.hidden = Number(p.getAttribute('data-page')) !== i; });
    }
    el('tabStrip').addEventListener('click', function (e) { var b = e.target.closest('button[data-tab]'); if (b) selectTab(Number(b.getAttribute('data-tab'))); });

    // ------------------------------------------------------------------ the transit / bills grid
    var kind = '';                                          // 'transit' | 'bills'
    var tg = new S.Grid({ tableId: 'tblTransit', gridId: 'transitGrid', navId: 'navTransit', navTextId: 'navTextTransit', headerLines: 2, autosize: true, frozen: 2,
        onButton: function (row, col) { buttonClick(row, col); } });
    var baseRender = S.Grid.prototype.render;
    tg.render = function () {                                // CustomerRemarks is the one editable column
        baseRender.call(this);
        if (!this.columns) return;
        var cols = this.visibleCols(), idx = -1;
        cols.forEach(function (c, i) { if (c.key === 'CustomerRemarks') idx = i; });
        if (idx < 0) return;
        this.flat.forEach(function (f) {
            var td = f.tr.cells[idx]; if (!td) return;
            td.textContent = '';
            var inp = d.createElement('input'); inp.type = 'text'; inp.className = 'rem'; inp.value = f.row.CustomerRemarks || '';
            inp.addEventListener('input', function () { f.row.CustomerRemarks = inp.value; });
            inp.addEventListener('focus', function () { var i = tg.flat.indexOf(f); if (i >= 0 && i !== tg.selected) tg.select(i); });
            td.appendChild(inp);
        });
    };
    function btn(key, caption, text, width, k) { return { key: key, caption: caption, width: width, button: text, kind: k }; }
    function transitCols() {
        var D = 'dd-MMM-yy hh:mm tt';
        return [btn('_Slip', 'Slip', 'Slip', 40, 'Slip'), btn('_Confirm', 'Confirm', 'Confirm', 60, 'Confirm'),
            hid('Id'), hid('SupplierCustomerId'), hid('DocDate'), { key: 'OutDatetime', caption: 'OutDatetime', date: D }, { key: 'GpNo', caption: 'GpNo', num: true, align: 'r' },
            txt('DriverName'), txt('DriverCell'), { key: 'CarriageAmount', caption: 'CarriageAmount', fmt: single(), sum: true, num: true },
            txt('VehicleNo'), { key: 'Qty', caption: 'Qty', fmt: Q3, sum: true, num: true }, { key: 'Weight', caption: 'Weight', fmt: Q3, sum: true, num: true },
            txt('GdnStatus'), { key: 'CustomerRemarks', caption: 'CustomerRemarks', width: 200, auto: false }];
    }
    function billCols() {
        var D = 'dd/MM/yyyy';
        return [btn('_SlipBill', 'Slip', 'Slip', 40, 'SlipBill'), btn('_ConfirmBill', 'Confirm', 'Confirm', 60, 'ConfirmBill'),
            hid('Id'), hid('DocumentTypeId'), { key: 'BillNo', caption: 'BillNo', num: true, align: 'r' }, { key: 'BillDate', caption: 'BillDate', date: D },
            { key: 'DueDays', caption: 'DueDays', num: true, align: 'r' }, { key: 'DueDate', caption: 'DueDate', date: D },
            { key: 'Qty', caption: 'Qty', fmt: Q3, sum: true, num: true }, { key: 'Weight', caption: 'Weight', fmt: Q3, sum: true, num: true },
            { key: 'BillAmount', caption: 'BillAmount', fmt: single(), sum: true, num: true }, txt('ApprovedStatus'), { key: 'CustomerRemarks', caption: 'CustomerRemarks', width: 200, auto: false }];
    }

    function buttonClick(row, col) {
        var id = A.toInt(A.ci(row, 'Id'));
        if (col.kind === 'Slip') {
            if (id === 0) { w.alert('No Record Found For Display'); return; }
            A.openPdf('/reports/print/260-gdn-rice-slip?id=' + id);
        } else if (col.kind === 'Confirm' || col.kind === 'ConfirmBill') {
            var remarks = A.ci(row, 'CustomerRemarks') || '';
            if (!remarks) { w.alert('Remarks Required!'); return; }
            S.post(API + '/confirm', { kind: col.kind === 'ConfirmBill' ? 'bill' : 'transit', id: id, remarks: remarks }).then(function (r) {
                w.alert(r.message);
                return col.kind === 'ConfirmBill' ? getSaleBills() : getTransit();
            }).catch(function (e) { w.alert(e.message); });
        } else if (col.kind === 'SlipBill') {
            var t = A.toInt(A.ci(row, 'DocumentTypeId'));
            if (t === 95) A.openPdf('/reports/print/301-inv-rep-sale-bill-customer?id=' + id);
            else if (t === 99) A.openPdf('/reports/print/294a-sale-bill-direct-without-so?id=' + id);
        }
    }

    // ------------------------------------------------------------------ data
    function args() {
        return { fromDate: el('fromdate').value, fromTime: '00:00:00', toDate: el('ToDate').value, toTime: openedAt, costCenterId: S.selInt('CmbCostCenter') };
    }
    function setBoxes(h) {
        el('lblCustomers').textContent = S.fmtNum(h.customers, Q3);
        el('lblQty').textContent = S.fmtNum(h.qty, Q3);
        el('lblWeight').textContent = S.fmtNum(h.weight, Q3);
        el('lblAmount').textContent = S.fmtNum(h.amount, Q3);
    }
    function zeroBoxes() { ['lblCustomers', 'lblQty', 'lblWeight', 'lblAmount'].forEach(function (id) { el(id).textContent = '0'; }); }
    function getDetail(booking) {
        clearAll();
        return A.getJson(API + '/' + (booking ? 'booking' : 'pending') + '?' + S.qs(args())).then(function (r) {
            if (!r || r.empty) return;
            if (r.header) setBoxes(r.header);
            selectTab(0);
            fillGrid('Item', r.item); fillGrid('Customer', r.customer); fillGrid('ItemCustomer', r.itemCustomer);
            fillGrid('CustomerItem', r.customerItem); fillGrid('ItemPack', r.itemPack); fillGrid('PackItem', r.packItem);
        }).catch(function (e) { w.alert(e.message); });
    }
    function getTransit() {
        kind = 'transit';
        return A.getJson(API + '/transit?' + S.qs(args())).then(function (rows) {
            if (rows && rows.length) tg.setData(transitCols(), rows); else tg.clear();
        }).catch(function (e) { w.alert(e.message); });
    }
    function getSaleBills() {
        kind = 'bills';
        return A.getJson(API + '/bills?' + S.qs(args())).then(function (rows) {
            if (rows && rows.length) tg.setData(billCols(), rows); else tg.clear();
        }).catch(function (e) { w.alert(e.message); });
    }

    // ------------------------------------------------------------------ cards
    var ICONS = {
        'Booking / Demand': '<svg class="ico" viewBox="0 0 34 34"><rect x="4" y="5" width="26" height="6" fill="#0a6e6e"/><rect x="4" y="14" width="26" height="6" fill="#0a6e6e"/><rect x="4" y="23" width="26" height="6" fill="#0a6e6e"/></svg>',
        'Pending Orders': '<svg class="ico" viewBox="0 0 34 34"><circle cx="17" cy="17" r="13" fill="none" stroke="#e29a00" stroke-width="3"/><path d="M17 8v9l6 4" fill="none" stroke="#e29a00" stroke-width="3"/></svg>',
        'Delivery In Transit': '<svg class="ico" viewBox="0 0 34 34"><rect x="2" y="9" width="19" height="14" fill="#2a6fb8"/><path d="M21 13h7l4 5v5H21z" fill="#2a6fb8"/><circle cx="9" cy="25" r="3" fill="#333"/><circle cx="25" cy="25" r="3" fill="#333"/></svg>',
        'Sales Bill': '<svg class="ico" viewBox="0 0 34 34"><path d="M6 3h22v28l-4-3-4 3-4-3-4 3-4-3-2 2z" fill="#fff" stroke="#2e9b45" stroke-width="2"/><path d="M11 10h12M11 16h12M11 22h8" stroke="#2e9b45" stroke-width="2"/></svg>'
    };
    function cardClicked(caption) {
        var booking = caption === 'Booking / Demand', pending = caption === 'Pending Orders';
        if (booking || pending) {
            el('PanelForCaptionIncludingBooking').hidden = false; el('panelTransitSale').hidden = true; el('CardsPanel').hidden = false;
            getDetail(booking);
        } else if (caption === 'Delivery In Transit' || caption === 'Sales Bill') {
            zeroBoxes();
            el('PanelForCaptionIncludingBooking').hidden = true; el('CardsPanel').hidden = true; el('panelTransitSale').hidden = false;
            if (caption === 'Sales Bill') getSaleBills(); else getTransit();
        }
    }
    function btnShow() {
        var host = el('CardsFlowLayoutPanel'), b = el('btnShow');
        host.innerHTML = '';
        A.busy(b, true);
        return A.getJson(API + '/cards?' + S.qs({ costCenterId: S.selInt('CmbCostCenter') })).then(function (rows) {
            (rows || []).forEach(function (r) {
                var c = d.createElement('div'); c.className = 'mos-card';
                var cap = d.createElement('span'); cap.className = 'cap'; cap.textContent = r.caption;
                cap.addEventListener('click', function () { cardClicked(r.caption); });          // only lblCaption.Click is wired
                var ic = d.createElement('span'); ic.innerHTML = ICONS[r.caption] || '';
                var v = d.createElement('span'); v.className = 'val'; v.textContent = S.fmtNum(r.value, '#,##0.##');
                c.appendChild(cap); if (ic.firstChild) c.appendChild(ic.firstChild); c.appendChild(v);
                host.appendChild(c);
            });
        }).catch(function (e) { w.alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ Load
    function load() {
        openedAt = nowTime();
        el('ToDate').value = A.today();
        return A.getJson(API + '/init').then(function (data) {
            info = data || info;
            if (info.yearStart) el('fromdate').value = info.yearStart;
            var show = info.appId === 5, wrap = S.wrapOf('CmbCostCenter');
            el('lblCostCenter').hidden = !show;
            if (wrap) wrap.style.display = show ? '' : 'none';
            if (show) {
                S.fill(el('CmbCostCenter'), info.costCenters, false);
                if (info.costCenters.length > 0) {
                    S.activate('CmbCostCenter', 0, false);
                    if (wrap) wrap.classList.add('dis');
                    el('CmbCostCenter').disabled = true;
                }
            }
            return btnShow();
        }).catch(function (e) { w.alert(e.message); });
    }
    el('btnShow').addEventListener('click', btnShow);
    load();
}(window, document));
