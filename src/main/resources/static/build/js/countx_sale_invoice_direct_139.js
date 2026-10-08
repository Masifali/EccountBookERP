/* ============================================================================================
 * Sale Invoice Direct 139 - screen 145, Sale hub, DocumentTypeId 139.
 * Desktop: Architecture.WinApp.Sale.frmSaleInvoiceDirect139
 *   Load :364  suppliercustomer :436  item :455  PaymentTerms :472  DeliveryTerm :489  PackingType :507  JobLotFill :523
 *   WareHouseFill :539  OtherItemsBind :555  PackUOM :586  DocumentNo :623  AvailableStockGetByItem :663  grdSettings :705
 *   Insert :743  Reset :945  ResetDetail :992  BillAmount :1026  grdInvExp_CellUpdated :1072  BillProportion :1100
 *   CommissionProportion :1118  btnAdd :1141  ReadById :1163  Total :1217  TotalWeight :1252  AmountCaluculation :1279
 *   comPackUOM_Leave :1316  txtQty_TextChanged :1338  GetAll :1390  HistoryGridSettings :1425  TotalCommissionAmount :1492
 *   KeyDown :1578  grd_DoubleClick :1636  btnUpdateDetail :1669  grdHistory_ColumnButtonClick :1807  GetDetailGrdByHeadId :1848
 * The server re-validates and recomputes everything Insert posts (net weight, amount, commission, bill amount).
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/sale/api/sale-invoice-direct-139';
    var L = {}, perms = {};
    var dtGrid = [], dtExp = [], historyRows = [];
    var Id = 0, VoucherHeadId = 0, Approved = false, updateDetailIndex = -1, selRow = -1, selHist = -1, tab = 0, uoms = [];

    function $id(x) { return document.getElementById(x); }
    function val(x) { var e = $id(x); return e ? e.value : ''; }
    function setVal(x, v) { var e = $id(x); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    /* .NET Math.Round(x, n): midpoint to even */
    function even(x, n) {
        var f = Math.pow(10, n || 0), y = x * f, r = Math.round(y);
        if (Math.abs(Math.abs(y % 1) - 0.5) < 1e-9) r = 2 * Math.round(y / 2);
        return r / f;
    }
    /* custom format "#,#" / "#,#.##": thousands separators, zero shows nothing */
    function hash(n, d) {
        n = num(n); if (n === 0) return '';
        var s = Math.abs(n), f = Math.pow(10, d || 0);
        s = Math.round(s * f + 1e-9) / f;
        var t = s.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d || 0 });
        return (n < 0 ? '-' : '') + t;
    }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
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
    function fill(x, rows, v, t) {
        var sel = $id(x); if (!sel) return;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
    }
    function text(x) { var s = $id(x); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function show(x, on) { var e = $id(x); if (e) e.style.display = on ? '' : 'none'; }
    function focus(x) { var e = $id(x); if (e) try { e.focus(); } catch (z) { /* ignore */ } }

    // ------------------------------------------------------------------ load (InvfrmPurchasedirectInvoice_Load :364)

    function bindLists() {
        fill('comsupplier', L.customers, 'Id', 'CompanyName');
        fill('combcommAgent', L.customers, 'Id', 'CompanyName');
        fill('comItem', L.items, 'Id', 'ItemName');
        fill('combpttrm', L.dueTerms, 'Id', 'TermsDescription');
        if ((L.dueTerms || []).length > 1) setVal('combpttrm', col(L.dueTerms[1], 'Id'));      /* Rows[1].Activate() */
        fill('combdeliverytrm', L.deliveryTerms, 'Id', 'DeliveryTerm');
        setVal('combdeliverytrm', col(L.deliveryTerms[0], 'Id'));                                  /* Rows[0].Activate() */
        fill('CmbWareHouse', L.warehouses, 'Id', 'WareHouseName');
        fill('CmbJobLot', L.jobLots, 'Id', 'JobLotDescription');
        fill('comPackingType', L.packingTypes, 'Id', 'PackTypeDesc');
        refreshCombos();
    }

    function otherItemsBind() {
        dtExp = (L.otherItems || []).map(function (r) {
            return { ItemId: int(col(r, 'Id')), ItemName: col(r, 'OtherItemName'), Remarks: '', Qty: 0, Rate: 0, Amount: 0 };
        });
    }

    function applyRights() {
        $id('btnSave').disabled = !perms.save;
        $id('btnPrint').disabled = !perms.print;
        $id('btnUpdate').disabled = !perms.update;
    }

    function init() {
        bindEvents();
        setVal('DocDate', today()); setVal('duedate', today()); setVal('txtduedays', '0');
        $id('ChkPrintslip').checked = true;
        http('GET', api + '/lookups').then(function (d) {
            L = d || {}; perms = L.rights || {};
            setVal('txtdocno', L.docNo);
            otherItemsBind(); bindLists(); applyRights(); renderAll();
            show('btnSave', true); show('btnUpdate', false);
            focus('DocDate'); say('');
        }, function (e) { say(e.message); box(e.message); });
    }

    // ------------------------------------------------------------------ calculations

    /* Total() :1217 */
    function Total() {
        var g = val('txtGrossWeight').trim() === '' ? 0 : num(val('txtGrossWeight'));
        var a = val('txtAddLss').trim() === '' ? 0 : num(val('txtAddLss'));
        setVal('txtNetBillWeight', String(even(g + a, 2)));
    }
    /* AmountCaluculation() :1279 */
    function AmountCaluculation() {
        var ru = text('comRateUOM').trim() === '' ? 0 : num(text('comRateUOM'));
        var r = val('txtRate').trim() === '' ? 0 : num(val('txtRate'));
        var nw = val('txtNetBillWeight').trim() === '' ? 0 : num(val('txtNetBillWeight'));
        if (nw > 0 && ru > 0 && r > 0) setVal('txtAmount', String(even(nw / ru * r, 0)));
        else { setVal('txtAmount', '0'); setVal('txtBillAmount', '0'); }
    }
    function gridTotal(key) { return dtGrid.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    function expTotal(key) { return dtExp.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    /* BillAmount() :1026 */
    function BillAmount() {
        var bill = gridTotal('ItemAmount') + expTotal('Amount');
        if (int(val('comsupplier')) === int(val('combcommAgent'))) bill -= num(val('txtcommamount'));
        setVal('txtBillAmount', hash(even(bill, 0)));
        BillProportion();
    }
    /* BillProportion() :1100 */
    function BillProportion() {
        dtGrid.forEach(function (r) { r.BillAmount = num(r.ItemAmount) - num(r.Commission); });
        renderGrid();
    }
    /* CommissionProportion() :1118 */
    function CommissionProportion() {
        var p = num(val('txtcommrate'));
        dtGrid.forEach(function (r) { r.Commission = num(r.ItemAmount) * p / 100; });
        BillProportion();
    }
    /* TotalCommissionAmount() :1492 */
    function TotalCommissionAmount() {
        var p = num(val('txtcommrate'));
        setVal('txtcommamount', hash(even(gridTotal('ItemAmount') * p / 100, 0)));
        CommissionProportion();
    }

    // ------------------------------------------------------------------ item / stock

    function PackUOM() {
        var item = int(val('comItem'));
        if (!item) { uoms = []; fill('comPackUOM', [], 'Id', 'Equivalent'); fill('comRateUOM', [], 'Id', 'Equivalent'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/uoms?itemId=' + item).then(function (rows) {
            uoms = rows || [];
            if (uoms.length > 0) { fill('comPackUOM', uoms, 'Id', 'Equivalent'); fill('comRateUOM', uoms, 'Id', 'Equivalent'); refreshCombos(); }
        }, function (e) { box(e.message); });
    }
    function AvailableStockGetByItem() {
        var item = int(val('comItem'));
        if (!item) { $id('lblBalance').textContent = '0'; return Promise.resolve(); }
        return http('GET', api + '/stock?itemId=' + item + '&docDate=' + encodeURIComponent(val('DocDate')) + '&packingTypeId=' + int(val('comPackingType'))
            + '&stockUom=' + int(val('comPackUOM'))).then(function (d) {
            var s = num(d && d.stock);
            $id('lblBalance').textContent = s > 0 ? String(s) : '0';
        }, function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ entry events

    function setGross(v) { setVal('txtGrossWeight', v); Total(); }                                     /* txtGrossWeight_TextChanged */
    function qtyChanged() {                                                                              /* txtQty_TextChanged :1338 */
        if (val('txtQty') !== '' && text('comPackUOM') !== '') setGross(String(num(val('txtQty')) * num(text('comPackUOM'))));
        else setGross('0');
        Total(); AmountCaluculation(); TotalCommissionAmount(); BillAmount();
    }
    function packUomLeave() {                                                                            /* comPackUOM_Leave :1316 */
        if (val('txtQty') !== '' && text('comPackUOM') !== '') setGross(String(num(val('txtQty')) * num(text('comPackUOM'))));
        Total(); AmountCaluculation(); TotalCommissionAmount(); BillAmount(); AvailableStockGetByItem();
    }
    function addLssChanged() { Total(); AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }
    function rateUomLeave() { AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }
    function rateChanged() { AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }
    function commRateChanged() { TotalCommissionAmount(); BillAmount(); }
    function dueDaysChanged() {                                                                          /* txtduedays_TextChanged :1788 */
        var t = val('txtduedays').trim();
        var d = new Date(); if (t !== '') d.setDate(d.getDate() + num(t));
        setVal('duedate', iso(d));
    }
    function supplierChanged() {
        BillAmount();
        var c = (L.customers || []).filter(function (x) { return int(col(x, 'Id')) === int(val('comsupplier')); })[0];
        if (c) setVal('txtSupplierGLId', col(c, 'GlAccountId'));
        setVal('combcommAgent', int(val('comsupplier')) || '');                                          /* comsupplier_Leave */
        refreshCombos();
        BillAmount();
    }

    /* FormValidationDetila() :293 */
    function FormValidationDetila() {
        function need(x, m) { if (val(x) === '' || int(val(x)) === 0) { box(m); focus(x); return false; } return true; }
        if (!need('CmbWareHouse', 'WareHouse Field is Required')) return false;
        if (!need('CmbJobLot', 'JobLot Field is Required')) return false;
        if (!need('comItem', 'Item Name Field is Required')) return false;
        if (!need('comPackingType', 'Packing Type Field is Required')) return false;
        if (num(val('txtQty').trim()) <= 0) { box('Qty Field is Required'); focus('txtQty'); return false; }
        if (!need('comPackUOM', 'Pack Unit Field is Required')) return false;
        if (num(val('txtGrossWeight').trim()) <= 0) { box('Gross Weight Field is Required'); focus('txtGrossWeight'); return false; }
        if (num(val('txtNetBillWeight').trim()) <= 0) { box('Net Bill Weight Field is Required'); focus('txtNetBillWeight'); return false; }
        if (num(val('txtRate').trim()) <= 0) { box('Rate Field is Required'); focus('txtRate'); return false; }
        if (!need('comRateUOM', 'Rate UOM Field is Required')) return false;
        if (num(val('txtAmount').trim()) <= 0) { box('Net Bill Weight Field is Required'); focus('txtNetBillWeight'); return false; }
        return true;
    }
    function rowFromEntry() {
        return {
            WarehouseId: int(val('CmbWareHouse')), Warehouse: text('CmbWareHouse'), JobLotId: int(val('CmbJobLot')), JobLot: text('CmbJobLot'),
            VehicleNo: val('txtvehicleno'), ItemId: int(val('comItem')), ItemName: text('comItem'),
            PackingTypeId: int(val('comPackingType')), PackingType: text('comPackingType'), PackUOMId: int(val('comPackUOM')), PackUOM: text('comPackUOM'),
            ItemQty: num(val('txtQty').trim()), GrossWeight: num(val('txtGrossWeight')), AddLss: num(val('txtAddLss')), NetBillWeight: num(val('txtNetBillWeight')),
            Rate: num(val('txtRate')), RateUOMId: int(val('comRateUOM')), RateUOM: text('comRateUOM'), ItemAmount: num(val('txtAmount'))
        };
    }
    /* btnAdd_Click :1141 */
    function btnAdd_Click() {
        if (!FormValidationDetila()) return;
        var r = rowFromEntry(); r.BillAmount = 0; r.Commission = 0;
        dtGrid.push(r);
        TotalCommissionAmount(); BillAmount(); ResetDetail();
    }
    /* ResetDetail() :992 */
    function ResetDetail() {
        ['comPackUOM', 'txtQty', 'txtGrossWeight', 'txtAddLss', 'txtNetBillWeight', 'txtRate', 'comRateUOM', 'txtAmount', 'CmbWareHouse', 'CmbJobLot'].forEach(function (x) { setVal(x, ''); });
        if ($id('ChkResetOnSave').checked) OptionResetFields();
        refreshCombos(); focus('CmbWareHouse');
    }
    function OptionResetFields() { setVal('comItem', ''); setVal('comPackingType', ''); setVal('txtvehicleno', ''); refreshCombos(); focus('comItem'); }
    /* grd_DoubleClick :1636 */
    function grdDoubleClick(i) {
        var r = dtGrid[i]; if (!r) return;
        updateDetailIndex = i;
        setVal('CmbWareHouse', r.WarehouseId); setVal('CmbJobLot', r.JobLotId); setVal('comItem', r.ItemId);
        PackUOM().then(function () {
            setVal('comPackingType', r.PackingTypeId); setVal('txtQty', r.ItemQty); setVal('comPackUOM', r.PackUOMId);
            setVal('txtGrossWeight', r.GrossWeight); setVal('txtAddLss', r.AddLss); setVal('txtNetBillWeight', r.NetBillWeight);
            setVal('txtRate', r.Rate); setVal('comRateUOM', r.RateUOMId); setVal('txtAmount', r.ItemAmount); setVal('txtvehicleno', r.VehicleNo);
            refreshCombos();
            show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        });
    }
    /* btnUpdateDetail_Click :1669 */
    function btnUpdateDetail_Click() {
        if (!FormValidationDetila()) return;
        var r = dtGrid[updateDetailIndex]; if (!r) return;
        var n = rowFromEntry();
        for (var k in n) r[k] = n[k];
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        TotalCommissionAmount(); BillAmount();
    }
    function btnCancelUpdateDetial_Click() { show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }

    // ------------------------------------------------------------------ grids

    var gridCols = [
        ['Warehouse', 'Warehouse', 't'], ['JobLot', 'JobLot', 't'], ['VehicleNo', 'VehicleNo', 't'], ['ItemName', 'ItemName', 't'], ['PackingType', 'PackingType', 't'],
        ['PackUOM', 'PackUOM', 't'], ['ItemQty', 'ItemQty', 'q'], ['GrossWeight', 'GrossWeight', 'h'], ['AddLss', 'AddLss', 'q'], ['NetBillWeight', 'NetBillWeight', 'h'],
        ['Rate', 'Rate', 'r'], ['RateUOM', 'RateUOM', 't'], ['ItemAmount', 'ItemAmount', 'h'], ['BillAmount', 'ItemNetAmount', 'h'], ['Commission', 'Brokery', 'h']
    ];
    var sumCols = { ItemQty: 1, GrossWeight: 1, NetBillWeight: 1, ItemAmount: 1, BillAmount: 1, Commission: 1 };
    function fmtCell(kind, v) {
        if (kind === 'h') return hash(v, 0);
        if (kind === 'r') return hash(v, 2);
        if (kind === 'q') return esc(v === null || v === undefined || v === '' ? '' : v);
        return esc(v);
    }
    function renderGrid() {
        $id('grdHead').innerHTML = gridCols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('');
        $id('grdBody').innerHTML = dtGrid.map(function (r, i) {
            return '<tr data-i="' + i + '" class="' + (i === selRow ? 'sel' : '') + '">' + gridCols.map(function (c) {
                return '<td class="' + (c[2] === 't' ? '' : 'n') + '">' + fmtCell(c[2], r[c[0]]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = gridCols.map(function (c) {
            return '<td class="' + (c[2] === 't' ? '' : 'n') + '">' + (sumCols[c[0]] ? (c[2] === 'q' ? String(gridTotal(c[0])) : hash(gridTotal(c[0]), 0)) : '') + '</td>';
        }).join('');
    }
    function expFooter() {
        var q = expTotal('Qty'), a = expTotal('Amount'), n = dtExp.length, rt = n ? expTotal('Rate') / n : 0;
        $id('expFoot').innerHTML = '<td></td><td></td><td class="n">' + hash(q, 0) + '</td><td class="n">' + hash(rt, 0) + '</td><td class="n">' + hash(a, 0) + '</td>';
    }
    function renderExp() {
        $id('expHead').innerHTML = '<th style="width:250px">ItemName</th><th>Remarks</th><th>Qty</th><th>Rate</th><th>Amount</th>';
        $id('expBody').innerHTML = dtExp.map(function (r, i) {
            return '<tr data-e="' + i + '"><td>' + esc(r.ItemName) + '</td>'
                + '<td><input class="ct" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td>'
                + '<td><input class="ce" data-f="Qty" value="' + esc(r.Qty) + '"/></td>'
                + '<td><input class="ce" data-f="Rate" value="' + esc(r.Rate) + '"/></td>'
                + '<td class="n">' + hash(r.Amount, 0) + '</td></tr>';
        }).join('');
        expFooter();
    }
    function renderAll() { renderGrid(); renderExp(); }

    /* grdInvExp_CellUpdated :1072 */
    function expCell(i, f, v) {
        var r = dtExp[i]; if (!r) return;
        if (f === 'Remarks') { r.Remarks = v; return; }
        r[f] = v;
        if (String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = num(r.Qty) * num(r.Rate);
        BillAmount();
        var cells = $id('expBody').querySelectorAll('tr[data-e="' + i + '"] td');
        if (cells[4]) cells[4].textContent = hash(r.Amount, 0);
        expFooter();
    }

    // ------------------------------------------------------------------ save (Insert :743)

    function collect() {
        return {
            Id: Id, DocNo: val('txtdocno'), DocDate: val('DocDate'), SupplierCustomerId: int(val('comsupplier')), ManualBillNo: val('txtbillno'),
            DueDays: val('txtduedays'), DueDate: val('duedate'), RemarksHeader: val('txtremarks'), CommRate: num(val('txtcommrate')),
            CommissionAgentId: int(val('combcommAgent')),
            details: dtGrid.map(function (r) {
                return { WarehouseId: r.WarehouseId, JobLotId: r.JobLotId, ItemId: r.ItemId, PackingTypeId: r.PackingTypeId, PackUOMId: r.PackUOMId,
                    ItemQty: r.ItemQty, GrossWeight: r.GrossWeight, AddLss: r.AddLss, Rate: r.Rate, RateUOMId: r.RateUOMId, VehicleNo: r.VehicleNo };
            }),
            expenses: dtExp.map(function (r) { return { ItemId: r.ItemId, Remarks: r.Remarks, Qty: num(r.Qty), Rate: num(r.Rate), Amount: num(r.Amount) }; })
        };
    }
    function Insert() {
        if (dtGrid.length === 0) { box('Grid Record Not Found'); return; }
        if (int(val('comsupplier')) === 0) { box('CustomerName Field is Required'); focus('comsupplier'); return; }
        var dn = val('txtdocno').trim();
        if (dn === '' || dn === '0') { box('DocNo Field is Required'); focus('txtdocno'); return; }
        if (val('txtduedays').trim() === '') { box('Due Days Field is Required'); focus('txtduedays'); return; }
        if (!window.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < dtExp.length; i++)
            if (num(dtExp[i].Amount) > 0 && int(dtExp[i].ItemId) === 0) { box('Please Select an Item Against Expense First'); return; }
        if (num(val('txtcommamount')) > 0 && int(val('combcommAgent')) === 0) { box('Please Select Broker Account First'); return; }
        var chkBok = $id('ChkBok').checked, chkSlip = $id('ChkPrintslip').checked;
        return http('POST', api + '/save', collect()).then(function (d) {
            box(d.message);
            Reset();
            if (chkBok) VoucherReport_118(d.voucherHeadId);
            if (chkSlip) window.open('/api/print/sidpm-294/pdf?id=' + encodeURIComponent(d.id), '_blank');
        }, function (e) { box(e.message); });
    }
    function btnSave_Click() { Id = 0; return Insert(); }
    function btnUpdate_Click() {
        if (Approved) { box('Record Not Update beacause Record has approved'); return; }
        return Insert();
    }

    /* Reset() :945 */
    function Reset() {
        Id = 0; VoucherHeadId = 0; Approved = false;
        ['comsupplier', 'txtbillno', 'combcommAgent', 'txtcommrate', 'txtcommamount', 'txtremarks', 'comItem', 'comPackUOM', 'txtQty', 'txtGrossWeight',
            'txtAddLss', 'txtNetBillWeight', 'txtRate', 'comRateUOM', 'txtAmount', 'txtBillAmount'].forEach(function (x) { setVal(x, ''); });
        setVal('txtduedays', '0');
        dtGrid = []; selRow = -1;
        if ($id('ChkResetOnSave').checked) OptionResetFields();
        show('btnSave', true); show('btnUpdate', false);
        dtExp.forEach(function (r) { r.Qty = '0'; r.Rate = '0'; r.Amount = '0'; });       /* OtherItemReset :2068 */
        renderAll(); refreshCombos();
        http('GET', api + '/numbers').then(function (n) { setVal('txtdocno', n.docNo); });
        focus('DocDate');
    }
    function btnNew_Click() { Reset(); }

    // ------------------------------------------------------------------ read (ReadById :1163)

    function ReadById(id) {
        return http('GET', api + '/' + id).then(function (m) {
            Id = int(id);
            setVal('txtdocno', col(m, 'DocNo')); setVal('DocDate', dateOnly(col(m, 'DocDate')));
            showTab(0);
            setVal('comsupplier', col(m, 'SupplierCustomerId')); setVal('txtbillno', col(m, 'ManualBillNo'));
            setVal('combcommAgent', col(m, 'CommissionAgentId')); setVal('txtcommrate', col(m, 'CommRate')); setVal('txtcommamount', col(m, 'CommAmount'));
            setVal('txtremarks', col(m, 'RemarksHeader')); setVal('txtBillAmount', col(m, 'BillAmount'));
            setVal('txtduedays', col(m, 'SupplierInvoiceNo')); setVal('duedate', dateOnly(col(m, 'SupplierInvoiceDate')));
            Approved = !!col(m, 'IsApproved');
            VoucherHeadId = int(col(m, 'VoucherHeadId'));
            dtGrid = (m.details || []).map(function (r) {
                return { WarehouseId: int(col(r, 'WarehouseId')), Warehouse: col(r, 'WareHouseName'), JobLotId: int(col(r, 'JobLotId')), JobLot: col(r, 'JobLotDescription'),
                    VehicleNo: col(r, 'VehicleNo'), ItemId: int(col(r, 'ItemId')), ItemName: col(r, 'ItemName'), PackingTypeId: int(col(r, 'PackingTypeId')),
                    PackingType: col(r, 'PackTypeDesc'), PackUOMId: int(col(r, 'ItemUOMId')), PackUOM: col(r, 'UOMCodeItem'), ItemQty: num(col(r, 'ItemQty')),
                    GrossWeight: num(col(r, 'GrossWeight')), AddLss: num(col(r, 'AdLsWeight')), NetBillWeight: num(col(r, 'NetBillWeight')), Rate: num(col(r, 'ItemRate')),
                    RateUOMId: int(col(r, 'UomScheduleIdRate')), RateUOM: col(r, 'RateUOM'), ItemAmount: num(col(r, 'ItemAmount')), BillAmount: num(col(r, 'BillAmount')),
                    Commission: num(col(r, 'CommissionAmount')) };
            });
            dtExp = (m.expenses || []).map(function (r) {
                return { ItemId: int(col(r, 'InvRevExpItemId')), ItemName: col(r, 'OtherItemName'), Remarks: col(r, 'Remarks'), Qty: num(col(r, 'Qty')), Rate: num(col(r, 'Rate')), Amount: num(col(r, 'Amount')) };
            });
            show('btnSave', false); show('btnUpdate', true);
            CommissionProportion(); renderAll(); refreshCombos();
        }, function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history

    var hiddenHist = { id: 1, recordno: 1, voucherheadid: 1, isreserve: 1, gdnid: 1, gdnno: 1 };
    function GetAll(n) {
        return http('GET', api + '/history?noOfRecords=' + (n || 0)).then(function (rows) {
            historyRows = rows || []; selHist = -1; renderHistory(); renderDetail(null);
        }, function (e) { box(e.message); });
    }
    function renderHistory() {
        var head = $id('histHead'), body = $id('histBody');
        if (!historyRows.length) { head.innerHTML = ''; body.innerHTML = ''; return; }
        var keys = Object.keys(historyRows[0]).filter(function (k) { return !hiddenHist[k.toLowerCase()]; });
        head.innerHTML = keys.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('') + '<th>View</th><th>Detail</th><th>Edit</th><th>Voucher</th>';
        body.innerHTML = historyRows.map(function (r, i) {
            return '<tr data-h="' + i + '" class="' + (i === selHist ? 'sel' : '') + '">' + keys.map(function (k) {
                var v = r[k]; if (/date$/i.test(k)) v = dateOnly(v) || v;
                return '<td>' + (k === 'NoOfAttachments' ? '<span class="glink" data-att="' + i + '">' + esc(v) + '</span>' : esc(v)) + '</td>';
            }).join('')
                + '<td><button type="button" data-b="View" data-i="' + i + '">View</button></td><td><button type="button" data-b="Detail" data-i="' + i + '">Detail</button></td>'
                + '<td><button type="button" data-b="Edit" data-i="' + i + '">Edit</button></td><td><button type="button" data-b="Voucher" data-i="' + i + '">Voucher</button></td></tr>';
        }).join('');
    }
    var detCols = ['ItemName', 'ItemUOM', 'PackingType', 'VehicleNo', 'ItemQty', 'GrossWeight', 'NetBillWeight', 'ItemRate', 'RateUOM', 'ItemAmount', 'BrokeryAmount'];
    var detSum = { ItemQty: 1, GrossWeight: 1, NetBillWeight: 1, ItemAmount: 1, BrokeryAmount: 1 };
    var detNum = { ItemQty: 1, GrossWeight: 1, NetBillWeight: 1, ItemRate: 1, ItemAmount: 1, BrokeryAmount: 1 };
    /* GetDetailGrdByHeadId :1848 */
    function renderDetail(rows) {
        $id('detHead').innerHTML = rows ? detCols.map(function (c) { return '<th>' + c + '</th>'; }).join('') : '';
        $id('detBody').innerHTML = (rows || []).map(function (r) {
            return '<tr>' + detCols.map(function (c) { return '<td class="' + (detNum[c] ? 'n' : '') + '">' + (detNum[c] ? hash(r[c], 0) : esc(r[c])) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('detFoot').innerHTML = rows ? detCols.map(function (c) {
            return '<td class="n">' + (detSum[c] ? num(rows.reduce(function (a, r) { return a + num(r[c]); }, 0)).toLocaleString('en-US', { maximumFractionDigits: 2 }) : '') + '</td>';
        }).join('') : '';
    }
    function GetDetailGrdByHeadId(i) {
        var h = historyRows[i]; if (!h) return;
        http('GET', api + '/' + int(col(h, 'Id'))).then(function (m) {
            var d = (m.details || []).map(function (r) {
                return { ItemName: col(r, 'ItemName'), ItemUOM: col(r, 'UOMCodeItem'), PackingType: col(r, 'PackTypeDesc'), VehicleNo: col(r, 'VehicleNo'), ItemQty: col(r, 'ItemQty'),
                    GrossWeight: col(r, 'GrossWeight'), NetBillWeight: col(r, 'NetBillWeight'), ItemRate: col(r, 'ItemRate'), RateUOM: col(r, 'RateUOM'),
                    ItemAmount: col(r, 'ItemAmount'), BrokeryAmount: col(r, 'CommissionAmount') };
            });
            renderDetail(d.length ? d : null);
        }, function () { renderDetail(null); });
    }
    function VoucherReport_118(vh) { if (!int(vh)) { box('VoucherId Not Found'); return; } window.open('/api/print/acc-118/pdf?id=' + encodeURIComponent(vh) + '&documentTypeId=139', '_blank'); }
    function Slip294(id) { if (!int(id)) { box('No Record Found For Display'); return; } window.open('/api/print/sidpm-294/pdf?id=' + encodeURIComponent(id), '_blank'); }

    function showTab(t) {
        tab = t;
        $id('tabPage1').style.display = t === 0 ? '' : 'none';
        $id('tabPage2').style.display = t === 1 ? '' : 'none';
        $id('tabForm').className = t === 0 ? 'on' : '';
        $id('tabHistory').className = t === 1 ? 'on' : '';
        if (t === 1) GetAll(50);                                                                          /* tabControl1_SelectedIndexChanged :1570 */
    }

    // ------------------------------------------------------------------ toolbar

    function btnPrint_Click() { VoucherReport_118(VoucherHeadId); }
    function btnSlip_Click() { Slip294(Id); }
    function btn294APrint_Click() { box('Report 294A-InvRptSaleBillDirectWithoutSO is not available on the web yet.'); }
    function btnAttachment_Click() { box('Attachments are not available on this screen yet.'); }
    function btnRefresh_Click() {
        http('GET', api + '/lookups').then(function (d) { L = d || {}; perms = L.rights || {}; bindLists(); applyRights(); }, function (e) { box(e.message); });
    }
    function btnLoadAll_Click() { GetAll(0); }

    // ------------------------------------------------------------------ wiring

    function decOnly(e) { if (e.key && e.key.length === 1 && !/[0-9.]/.test(e.key) && !e.ctrlKey && !e.metaKey) e.preventDefault(); }
    function intOnly(e) { if (e.key && e.key.length === 1 && !/[0-9]/.test(e.key) && !e.ctrlKey && !e.metaKey) e.preventDefault(); }
    function on(x, ev, fn) { var e = $id(x); if (e) e.addEventListener(ev, fn); }

    function bindEvents() {
        on('txtQty', 'input', qtyChanged); on('txtGrossWeight', 'input', Total); on('txtAddLss', 'input', addLssChanged);
        on('comPackUOM', 'change', packUomLeave); on('comRateUOM', 'change', rateUomLeave); on('txtRate', 'input', rateChanged);
        on('txtcommrate', 'input', commRateChanged); on('txtduedays', 'input', dueDaysChanged);
        on('comsupplier', 'change', supplierChanged); on('combcommAgent', 'change', function () { TotalCommissionAmount(); BillAmount(); });
        on('comItem', 'change', function () { PackUOM().then(AvailableStockGetByItem); });
        on('comPackingType', 'change', AvailableStockGetByItem);
        on('txtQty', 'keypress', decOnly); on('txtGrossWeight', 'keypress', decOnly); on('txtRate', 'keypress', decOnly); on('txtcommrate', 'keypress', decOnly);
        on('txtduedays', 'keypress', intOnly);
        on('btnAdd', 'click', btnAdd_Click); on('btnUpdateDetail', 'click', btnUpdateDetail_Click); on('btnCancelUpdateDetial', 'click', btnCancelUpdateDetial_Click);
        $id('grdBody').addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) { selRow = int(tr.getAttribute('data-i')); renderGrid(); } });
        $id('grdBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) grdDoubleClick(int(tr.getAttribute('data-i'))); });
        $id('expBody').addEventListener('change', function (e) {
            var t = e.target, tr = t.closest('tr[data-e]'); if (!tr || !t.getAttribute('data-f')) return;
            expCell(int(tr.getAttribute('data-e')), t.getAttribute('data-f'), t.value);
        });
        $id('histBody').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-b]'), tr = e.target.closest('tr[data-h]');
            if (tr && !b) { selHist = int(tr.getAttribute('data-h')); renderHistory(); GetDetailGrdByHeadId(selHist); }
            if (!b) return;
            var i = int(b.getAttribute('data-i')), h = historyRows[i]; if (!h) return;
            if (b.getAttribute('data-b') === 'Edit') ReadById(int(col(h, 'Id')));
            if (b.getAttribute('data-b') === 'View') Slip294(int(col(h, 'Id')));
            if (b.getAttribute('data-b') === 'Voucher') VoucherReport_118(int(col(h, 'VoucherHeadId')));
            if (b.getAttribute('data-b') === 'Detail') { selHist = i; GetDetailGrdByHeadId(i); }
        });
        $id('histBody').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var h = historyRows[int(tr.getAttribute('data-h'))]; if (h) ReadById(int(col(h, 'Id')));
        });
        document.addEventListener('keydown', function (e) {
            var tg = e.target, tn = tg && tg.tagName;
            if (e.key === 'Delete' && tab === 0 && selRow >= 0 && tn !== 'INPUT' && tn !== 'TEXTAREA' && tn !== 'SELECT') {   /* grd_RecordsDeleted :2041 */
                dtGrid.splice(selRow, 1); selRow = -1; TotalCommissionAmount(); BillAmount(); return;
            }
            if (e.key === 'Enter' && (tn === 'INPUT' || tn === 'SELECT') && tg.type !== 'checkbox' && tg.type !== 'button') {                 /* SendKeys TAB */
                var all = Array.prototype.filter.call(document.querySelectorAll('#frm input:not([type=hidden]), #frm select, #frm textarea, #frm button'),
                    function (x) { return x.offsetParent !== null && !x.disabled && !x.readOnly && !x.classList.contains('dtcombo-native'); });
                var k = all.indexOf(tg); if (k >= 0 && k < all.length - 1) { e.preventDefault(); all[k + 1].focus(); }
            }
            if (e.ctrlKey && (e.key === 's' || e.key === 'S') && tab === 0 && !$id('btnSave').disabled && $id('btnSave').style.display !== 'none') { e.preventDefault(); btnSave_Click(); }
            if (e.ctrlKey && (e.key === 'n' || e.key === 'N') && tab === 0) { e.preventDefault(); btnNew_Click(); }
            if (e.ctrlKey && (e.key === 't' || e.key === 'T')) { e.preventDefault(); showTab(tab === 1 ? 0 : 1); }
            if (e.ctrlKey && (e.key === 'u' || e.key === 'U') && !$id('btnUpdate').disabled && $id('btnUpdate').style.display !== 'none') { e.preventDefault(); btnUpdate_Click(); }
            if (e.ctrlKey && (e.key === 'p' || e.key === 'P') && !$id('btnPrint').disabled) { e.preventDefault(); btnPrint_Click(); }
            if (e.ctrlKey && e.key === 'F10') btnAttachment_Click();
            if (e.ctrlKey && e.key === 'F5') focus('CmbWareHouse');
            if (e.ctrlKey && e.key === 'ArrowUp' && tab === 0) { var f = document.querySelector('#expBody input'); if (f) f.focus(); }
        });
    }

    window.S139 = { showTab: showTab, btnNew_Click: btnNew_Click, btnSave_Click: btnSave_Click, btnUpdate_Click: btnUpdate_Click, btnPrint_Click: btnPrint_Click,
        btnSlip_Click: btnSlip_Click, btn294APrint_Click: btn294APrint_Click, btnAttachment_Click: btnAttachment_Click, btnRefresh_Click: btnRefresh_Click,
        btnLoadAll_Click: btnLoadAll_Click };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
