/* ============================================================================================
 * Screen 339 "Stock Transfer" — Architecture.WinApp.StoreManagement.frmStockTransfer, DocumentTypeId 68.
 *
 * Flow as on the desktop:
 *   - Transfer Type / Gate Pass are disabled (Load:365); "Load" on a Pending Entries row
 *     (GetDataFromGPGrid:1521) sets them, the ticket list and the item list.
 *   - Outward / MoveOrder rows are typed in the Detail box ("+", btnplus_Click:1625); Inward rows come
 *     from the branch transfer (BindDetailFromLoader:3482) or from the "Loader" dialog
 *     (LoadavailableTransactionsForIssuance → LoadDataDetailfromPurchaseInvoivce:3429).
 *   - Weights: txtQty/CmbPackUom → GrossWeightCalculation:3162 + WeightCalculation:3118.
 *   - Avg rate: GetAvgRate:3319 (AvgRateOnlyForCGS * 40), amount = net / 40 * rate formatted "#,#".
 *   - Other Charges (only with StockTransferFinancialEffectIsActive) spread over NetWeight as Expense.
 * Every server rule is repeated on the server; the client repeats the entry-bar checks the desktop
 * makes before anything reaches the BLL.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/stock-transfer';

    var look = { rights: {}, items: [], accounts: [], warehouses: [], cropYears: [], jobLots: [], packingTypes: [] };
    var Id = 0;                    // frmStockTransfer.Id
    var RefDocumentTypeId = 0;     // form field
    var IsApproved = false;
    var table = [];                // "table" — grd
    var dtExp = [];                // grdOtherCharges
    var dtItemsFromDO = [];
    var updateDetailIndex = -1;
    var currentRow = -1;           // grd.CurrentRow
    var loaderRows = [], loaderChecked = {};

    /* ------------------------------------------------------------------ small helpers */

    /** .NET Framework double.ToString(): 15 significant digits. */
    function clr(v) { var n = num(v); if (n === 0) return '0'; return String(Number(n.toPrecision(15))); }
    /** Custom format "#,#" / "#,#.##": zero renders empty. */
    function hash(v, dec) {
        var n = num(v), f = Math.pow(10, dec || 0), r = Math.sign(n) * Math.round(Math.abs(n) * f) / f;
        if (r === 0) return '';
        return r.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec || 0 });
    }
    function fmt0(v, dec) {                                   // "#,##0.##"
        return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec });
    }
    function selText(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] && intOf(e.value) !== 0 ? e.selectedOptions[0].textContent.trim() : ''; }
    function transferType() { var e = $id('CmbTransferType'); return e.selectedOptions[0] ? e.selectedOptions[0].textContent : ''; }
    function setOptions(id, rows, valueKey, textKey, blank) { C.fillSelect(id, rows, valueKey, textKey, blank); }
    function clearCombo(id) { $id(id).innerHTML = '<option value="0"></option>'; }
    function keepOr(id, value, rows, key) {
        var el = $id(id);
        if (value > 0) el.value = rows.some(function (r) { return intOf(ci(r, key)) === value; }) ? String(value) : '0';
    }
    function isEditMode() { return !$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled; }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }
    function fail(e) { alert(e && e.message ? e.message : e); }

    /* ------------------------------------------------------------------ load (frmStockTransfer_Load:357) */

    function init() {
        $id('DocDate').value = C.today();
        $id('txtQty').addEventListener('keypress', function (e) {            // txtqty_KeyPress:3511
            if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault();
        });
        ['txtFromDocNoHistory', 'txtToDocNoHistory'].forEach(function (id) {
            $id(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
        });
        $id('cmbItemName').addEventListener('blur', itemLeave);
        C.getJson(api + '/lookups').then(function (l) {
            look = l;
            $id('btnsave').disabled = !l.rights.save;
            $id('btnupdate').disabled = !l.rights.update;
            document.querySelectorAll('.fin').forEach(function (e) { e.classList.toggle('is-hidden', !l.financialEffect); });
            show('tabHeadOther', !!l.financialEffect);
            setOptions('CmbWareHouseFrom', l.warehouses, 'Id', 'WareHouseName');
            setOptions('cmbWareHouseTo', l.warehouses, 'Id', 'WareHouseName');
            setOptions('cmbPackingType', l.packingTypes, 'Id', 'PackTypeDesc');
            setOptions('cmbCropYear', l.cropYears, 'Id', 'CropYear');
            setOptions('cmbJobLot', l.jobLots, 'Id', 'JobLotDescription');
            setOptions('CmbJobLotTo', l.jobLots, 'Id', 'JobLotDescription');
            setOptions('CmbTransferType', [{ Id: 1, T: 'Inward' }, { Id: 2, T: 'Outward' }, { Id: 3, T: 'MoveOrder' }], 'Id', 'T', false);
            $id('CmbTransferType').value = '1';                                  // Rows[0].Activate()
            clearCombo('cmbGatePass'); clearCombo('cmbTicketNo'); clearCombo('cmbItemName'); clearCombo('CmbPackUom');
            $id('txtdocno').value = l.docNo > 0 ? String(l.docNo) : '';
            dtExp = [blankExp()];
            renderExp();
            renderGrid();
            renderPending(l.pending);
            $id('ChkBoxPrintPreview').checked = true;
            transferLeave();
            setOptions('cmbTransactiontypeHistory', l.historyTransferTypes, 'TransferType', 'TransferType');
            var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;
            var d = new Date(); d.setDate(d.getDate() - days);
            $id('FromDateHistory').value = d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
            $id('ToDateHistory').value = C.today();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ header combos */

    function transferLeave() {                                                   // CmbTransferType_Leave:1020
        var t = transferType();
        if (t === 'Inward' || t === 'Outward') return gatePassFill();
        return moveOrderTickets().then(itemFillAll);
    }

    function gatePassFill() {                                                    // CombGatePassNoFill:470
        var keep = intOf($id('cmbGatePass').value);
        return C.getJson(api + '/gate-passes' + C.qs({ transferType: transferType(), refDocumentTypeId: RefDocumentTypeId })).then(function (dt) {
            if (dt.length > 0) {
                setOptions('cmbGatePass', dt, 'Id', 'GpSrNo');
                keepOr('cmbGatePass', keep, dt, 'Id');
            } else { clearCombo('cmbGatePass'); clearCombo('cmbTicketNo'); }
        }).catch(fail);
    }

    function ticketsFill(gpId) {                                                 // combTicketNo:519
        var keep = intOf($id('cmbTicketNo').value);
        return C.getJson(api + '/tickets' + C.qs({ gatePassId: gpId, transferType: transferType(), refDocumentTypeId: RefDocumentTypeId })).then(function (dt) {
            if (dt.length > 0) {
                $id('txtOtherWeight').value = String(ci(dt[0], 'OtherWeight') === null ? '' : ci(dt[0], 'OtherWeight'));
                setOptions('cmbTicketNo', dt, 'Id', 'TicketNo');
                keepOr('cmbTicketNo', keep, dt, 'Id');
            } else {
                $id('txtOtherWeight').value = '';
                if (transferType() !== 'MoveOrder') clearCombo('cmbTicketNo');
            }
        }).catch(fail);
    }

    function moveOrderTickets() {                                                // GetSlipNoForMoveOrder:895
        var keep = intOf($id('cmbTicketNo').value);
        return C.getJson(api + '/move-order-tickets').then(function (dt) {
            if (dt.length > 0) { setOptions('cmbTicketNo', dt, 'Id', 'TicketNo'); keepOr('cmbTicketNo', keep, dt, 'Id'); }
            else { clearCombo('cmbGatePass'); clearCombo('cmbTicketNo'); }
        }).catch(fail);
    }

    function itemsFromDO(gpId) {                                                 // GetItemIdFromDeliveryOrderForStockTransfer:571
        var keep = intOf($id('cmbItemName').value);
        dtItemsFromDO = [];
        return C.getJson(api + '/delivery-order-items' + C.qs({ gatePassId: gpId })).then(function (dt) {
            dtItemsFromDO = dt;
            if (dt.length > 0) { setOptions('cmbItemName', dt, 'ItemId', 'ItemName'); keepOr('cmbItemName', keep, dt, 'ItemId'); }
            else clearCombo('cmbItemName');
        }).catch(fail);
    }

    function itemFillAll() {                                                     // ItemDetailFillForInwardandMoveOrder:694
        var keep = intOf($id('cmbItemName').value);
        clearCombo('cmbItemName');
        if (!look.items.length) return;
        setOptions('cmbItemName', look.items, 'Id', 'ItemName');
        keepOr('cmbItemName', keep, look.items, 'Id');
    }

    /** cmbGatePass_Leave:963, refDocId = the selected gate pass row's RefDocId. */
    function gatePassLeave(refDocId) {
        var t = transferType(), gp = intOf($id('cmbGatePass').value), p = Promise.resolve();
        if (t === 'Inward' || t === 'Outward') {
            $id('cmbTicketNo').value = '0';
            if (gp > 0) p = p.then(function () { return ticketsFill(gp); });
        } else clearCombo('cmbGatePass');
        if (t === 'Outward') p = p.then(function () { clearCombo('cmbItemName'); return itemsFromDO(gp); });
        if (t === 'Inward' && gp > 0) {
            if (RefDocumentTypeId === 51) p = p.then(function () { clearCombo('cmbItemName'); return itemsFromDO(refDocId); });
            else if (RefDocumentTypeId === 52) p = p.then(function () { itemFillAll(); show('panel5', true); });
        }
        return p;
    }

    /** Setting CmbTransferType.Value — CmbTransferType_ValueChanged:3263 fires when it changes. */
    function setType(v) {
        var el = $id('CmbTransferType');
        if (el.value === String(v)) return Promise.resolve();
        el.value = String(v);
        var p = Promise.resolve();
        if (transferType() === 'Outward') {
            clearCombo('cmbItemName');
            var gp = intOf($id('cmbGatePass').value);
            if (gp > 0) p = itemsFromDO(gp);
        }
        return p.then(handleLoaderVisibility);
    }

    function handleLoaderVisibility() {                                          // :3284
        show('btnLoadInvoices', RefDocumentTypeId !== 51 && selText('cmbTicketNo') !== '');
    }

    function ticketLeave() {                                                     // cmbTicketNo_Leave:1038
        handleLoaderVisibility();
        return C.getJson(api + '/ticket-weight' + C.qs({ ticketId: intOf($id('cmbTicketNo').value) })).then(function (r) {
            $id('txtHeadNetWeight').value = r.NetWbWeight;
            $id('txtWorkingReportNo').value = r.WorkingReportNo;
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ pending grid (grdDetail) */

    var PENDING_COLS = ['Doc_Type', 'WB_DocDate', 'TicketNo', 'GpNo', 'WorkingReportNo', 'DoNo', 'VehicleNo', 'ItemQty',
        'FirstWeight', 'SecondWeight', 'NetWeight', 'ItemName', 'Remarks', 'Status', 'RefGpNo', 'TransferNo'];
    var pendingRows = [];
    function renderPending(rows) {
        pendingRows = rows || [];
        var t = $id('grdDetail');
        if (!pendingRows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th>Load</th>' + PENDING_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        var tot = { FirstWeight: 0, SecondWeight: 0, NetWeight: 0 };
        var body = pendingRows.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-link" onclick="StockTr.loadPending(' + i + ')">Load</button></td>' +
                PENDING_COLS.map(function (c) {
                    var v = ci(r, c);
                    if (tot[c] !== undefined) { tot[c] += num(v); return '<td class="num">' + esc(hash(v) || '0') + '</td>'; }
                    if (c === 'WB_DocDate') return '<td>' + esc(C.gridDate(v)) + '</td>';
                    return '<td>' + esc(v === null ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        body += '<tr style="font-weight:bold"><td></td>' + PENDING_COLS.map(function (c) {
            return tot[c] !== undefined ? '<td class="num">' + esc(hash(tot[c])) + '</td>' : '<td></td>';
        }).join('') + '</tr>';
        t.tBodies[0].innerHTML = body;
    }

    /** GetDataFromGPGrid:1521. */
    function loadPending(i) {
        var item = pendingRows[i];
        if (!item) return;
        RefDocumentTypeId = intOf(ci(item, 'DocumentTypeId'));
        if (RefDocumentTypeId !== 52 && String(ci(item, 'Status')) !== 'Accepted') { alert('Record cannot be loaded because record status not Accepted'); return; }
        if (table.length > 0) {
            if (!confirm('Are you sure to Load? If You Load, Already Loaded Data Will Erased')) return;
            table = [];
        }
        var nb = intOf(ci(item, 'NoofBranches'));
        $id('txtNoofBranches').value = String(ci(item, 'NoofBranches'));
        var p = Promise.resolve();
        if (RefDocumentTypeId === 51 && nb > 1) { $id('CmbWareHouseFrom').disabled = true; $id('CmbWareHouseFrom').value = '0'; }
        else if (RefDocumentTypeId === 91 && nb > 1) { $id('cmbWareHouseTo').disabled = true; $id('cmbWareHouseTo').value = '0'; }
        else if (RefDocumentTypeId === 52) {
            p = setType(1).then(function () {
                $id('CmbWareHouseFrom').disabled = false; $id('cmbWareHouseTo').disabled = false;
                $id('cmbGatePass').value = '0'; $id('txtOtherWeight').value = '';
                itemFillAll();
            });
        } else if (RefDocumentTypeId === 74) {
            p = setType(3).then(function () {
                $id('CmbWareHouseFrom').disabled = false; $id('cmbWareHouseTo').disabled = false;
                $id('cmbGatePass').value = '0'; $id('txtOtherWeight').value = '';
                return moveOrderTickets();
            }).then(function () { itemFillAll(); $id('cmbTicketNo').value = String(intOf(ci(item, 'Id'))); });
        }
        function bindGp(type) {
            return setType(type).then(function () {
                setOptions('cmbGatePass', [{ Id: ci(item, 'Id'), GpNo: ci(item, 'GpNo') }], 'Id', 'GpNo', false);
                return gatePassLeave(intOf(ci(item, 'RefDocId')));
            });
        }
        if (RefDocumentTypeId === 51) p = p.then(function () { return bindGp(1); }).then(function () { if (nb > 1) return bindDetailFromLoader(); });
        if (RefDocumentTypeId === 52) p = p.then(function () { return bindGp(1); });
        if (RefDocumentTypeId === 91) p = p.then(function () { return bindGp(2); });
        p.then(function () { renderGrid(); $id('cmbTicketNo').focus(); }).catch(fail);
    }

    /** BindDetailFromLoader:3482. */
    function bindDetailFromLoader() {
        return C.getJson(api + '/branch-data' + C.qs({ gatePassId: intOf($id('cmbGatePass').value) })).then(function (dt) {
            if (!dt.length) { alert('Detail Record Not Found.Because Stock From Transaction is Incomplete!'); return; }
            table = dt.map(function (r) {
                return row(true, 0, 0, 0, 0, ci(r, 'ItemId'), ci(r, 'ItemName'), ci(r, 'InvPackingTypeId'), ci(r, 'PackingType'), ci(r, 'CropYear'),
                    ci(r, 'JobLotId'), ci(r, 'JobLotFrom'), ci(r, 'JobLotIdTo'), ci(r, 'JobLotTo'), ci(r, 'Qty'), ci(r, 'PackUomId'), clr(ci(r, 'PEquivalent')),
                    ci(r, 'NetWeight'), 0, 0, 0, ci(r, 'NetWeight'), ci(r, 'Qty'), ci(r, 'NetWeight'), 0, '', 0, '', '',
                    ci(r, 'ItemRate'), ci(r, 'RateUomId'), ci(r, 'REquivalent'), ci(r, 'ItemAmount'), 0,
                    ci(r, 'DocumentTypeId'), ci(r, 'InvStockTransferHeaderId'), ci(r, 'TransferDetailId'));
            });
            renderGrid();
        });
    }

    /** One "table" row, in the DataTable's column order (Load:391). */
    function row(flag, id, rdt, rno, rsub, itemId, itemName, packTypeId, packType, crop, jl, jlName, jlTo, jlToName, qty, puomId, puom,
                 gross, ebUnit, ebTotal, adls, net, balQty, balWt, whf, whfName, wht, whtName, remarks, rate, rateUomId, rateUom, amount,
                 expense, tdt, tid, tdid) {
        return {
            FlagForSplitAndDelete: !!flag, Id: intOf(id), RefDocumentTypeId: intOf(rdt), RefDocNoId: intOf(rno), RefDocSubIdNo: intOf(rsub),
            ItemId: intOf(itemId), ItemName: itemName === null || itemName === undefined ? '' : String(itemName),
            PackTypeId: intOf(packTypeId), PackType: packType || '', CropYear: crop === null || crop === undefined ? '' : String(crop),
            JobLotId: intOf(jl), JobLot: jlName || '', JobLotIdTo: intOf(jlTo), JobLotTo: jlToName || '',
            QTY: num(qty), PackUOMId: intOf(puomId), PackUOM: puom === null || puom === undefined ? '' : String(puom),
            GrossWeight: num(gross), EbUnit: num(ebUnit), EbTotal: num(ebTotal), AdLsWeight: num(adls), NetWeight: num(net),
            BalQty: num(balQty), BalWeight: num(balWt), WareHouseFromId: intOf(whf), WareHouseFrom: whfName || '',
            WareHouseToId: intOf(wht), WareHouseTo: whtName || '', Remarks: remarks || '',
            ItemRate: num(rate), RateUOMId: intOf(rateUomId), RateUOM: num(rateUom), ItemAmount: num(amount), Expense: num(expense),
            TransferDocumentTypeId: intOf(tdt), TransferId: intOf(tid), TransferDetailId: intOf(tdid)
        };
    }

    /* ------------------------------------------------------------------ entry bar */

    function itemLeave() {                                                       // cmbItemName_Leave:1006
        return bindUoms(intOf($id('cmbItemName').value)).then(stockAndRate);
    }

    function bindUoms(itemId) {                                                  // bindRateUomAndItemPackUom:734
        var prev = selText('CmbPackUom');
        return C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (dt) {
            if (dt.length > 0) {
                setOptions('CmbPackUom', dt.map(function (u) { return { Id: u.Id, Equivalent: clr(u.Equivalent) }; }), 'Id', 'Equivalent');
                var m = dt.find(function (u) { return prev !== '' && u.Equivalent === num(prev); });
                $id('CmbPackUom').value = m ? String(m.Id) : '0';
            } else clearCombo('CmbPackUom');
        }).catch(fail);
    }

    /** AvailableStockGetByItem:3220 + GetAvgRate:3319 (warehouse / crop / job lot Leave). */
    function stockAndRate() {
        var p1 = C.getJson(api + '/available-stock' + C.qs({
            recId: Id, itemId: intOf($id('cmbItemName').value), warehouseId: intOf($id('CmbWareHouseFrom').value),
            jobLotId: intOf($id('cmbJobLot').value), cropYear: selText('cmbCropYear'), docDate: $id('DocDate').value
        })).then(function (r) { $id('txtAvailableStock').value = r.availableStock; }).catch(fail);
        return Promise.all([p1, getAvgRate()]);
    }

    function getAvgRate() {
        if (transferType() === 'Inward' && updateDetailIndex > -1) {
            var r = table[currentRow];
            if (r && r.TransferDocumentTypeId > 0 && r.TransferId > 0 && r.TransferDetailId > 0) return Promise.resolve();
        }
        if (look.fifo) { $id('txtAvgRate').value = '0'; $id('txtAmount').value = '0'; return Promise.resolve(); }
        return C.getJson(api + '/avg-rate' + C.qs({
            recId: Id, itemId: intOf($id('cmbItemName').value), docDate: $id('DocDate').value,
            jobLotId: intOf($id('cmbJobLot').value), cropYearId: intOf($id('cmbCropYear').value),
            warehouseId: intOf($id('CmbWareHouseFrom').value)
        })).then(function (r) {
            if (num(r.rate) > 0) { $id('txtAvgRate').value = clr(r.rate); avgAmount(); }
            else { $id('txtAvgRate').value = '0'; $id('txtAmount').value = '0'; }
        }).catch(fail);
    }

    function avgAmount() {                                                       // AvgAmountCalculation:3369
        var net = num($id('txtDetailNetWeight').value), rate = num($id('txtAvgRate').value);
        $id('txtAmount').value = (net > 0 && rate > 0) ? hash(net / 40.0 * rate) : '0';
    }

    function grossCalc() {                                                       // GrossWeightCalculation:3162
        if (num($id('txtQty').value) !== 0 && intOf($id('CmbPackUom').value) !== 0) {
            $id('txtGrossWeight').value = clr(num($id('txtQty').value) * num(selText('CmbPackUom')));
        }
        avgAmount();
    }

    function weightCalc() {                                                      // WeightCalculation:3118
        var qtyT = $id('txtQty').value, grT = $id('txtGrossWeight').value;
        var qty = (qtyT.trim() !== '' && qtyT !== '0') ? num(qtyT) : 0;
        var gross = (grT.trim() !== '' && grT !== '0') ? num(grT) : 0;
        var addless = $id('txtAdLsWeight').value.trim() !== '' ? num($id('txtAdLsWeight').value) : 0;
        var ebUnit = 0, ebTotal = 0;
        if ($id('txtEbUnit').value.trim() !== '') ebUnit = num($id('txtEbUnit').value);
        else $id('txtEbTotal').value = '0';
        if (ebUnit > 0) { ebTotal = qty * ebUnit; $id('txtEbTotal').value = clr(ebTotal); }       // D6
        if (qtyT.trim() !== '' && intOf($id('CmbPackUom').value) !== 0 && grT.trim() !== '') {
            $id('txtDetailNetWeight').value = clr(gross - ebTotal + addless);
        }
        avgAmount();
    }

    function qtyChanged() { grossCalc(); weightCalc(); }                         // txtQty / CmbPackUom TextChanged

    /** FormValidationDetail:1117 — same messages, same order. */
    function validateDetail() {
        var type = intOf($id('CmbTransferType').value), nb = intOf($id('txtNoofBranches').value);
        function need(id, msg) { if (intOf($id(id).value) === 0) { alert(msg); $id(id).focus(); return false; } return true; }
        function needNum(id, msg) { if ($id(id).value.trim() === '' || num($id(id).value) === 0) { alert(msg); $id(id).focus(); return false; } return true; }
        if (type === 1 && nb > 1 && !need('cmbWareHouseTo', 'WarehouseTo field is required')) return false;
        if (type === 2 && nb > 1 && !need('CmbWareHouseFrom', 'WarehouseFrom field is required')) return false;
        if (nb <= 1) {
            if (!need('CmbWareHouseFrom', 'WarehouseFrom field is required')) return false;
            if (!need('cmbWareHouseTo', 'WarehouseTo field is required')) return false;
        }
        if (!need('cmbItemName', 'Item Name field is required')) return false;
        if (!need('cmbPackingType', 'PackingType field is required')) return false;
        if (!need('cmbCropYear', 'ItemName field is required')) return false;           // the desktop's own text
        if (!need('CmbPackUom', 'PackUOM field is required')) return false;
        if (!need('cmbJobLot', 'JobLot From field is required')) return false;
        if (!need('CmbJobLotTo', 'JobLotTo field is required')) return false;
        if (!needNum('txtQty', 'Qty field is required')) return false;
        if (!needNum('txtGrossWeight', 'GrossWeight field is required')) return false;
        if (look.financialEffect && !look.fifo) {
            if (!needNum('txtAvgRate', 'AvgRate field is required')) return false;
            if (!needNum('txtAmount', 'Amount field is required')) return false;
        }
        if (!needNum('txtDetailNetWeight', 'NetWeight field is required')) return false;
        return true;
    }

    /** A DataTable double column refuses "" (and anything unparseable), as the desktop's Rows.Add does. */
    function dbl(text, col) {
        var t = String(text).replace(/,/g, '').trim();
        if (t === '' || isNaN(Number(t))) throw new Error('Input string was not in a correct format.Couldn\'t store <' + text + '> in ' + col + ' Column.  Expected type is Double.');
        return Number(t);
    }

    function add() {                                                             // btnplus_Click:1625
        try {
            if (transferType() === 'Inward') throw new Error("You Can't Add Direct Record");
            if (!validateDetail()) return;
            if (table.some(function (r) { return r.RefDocumentTypeId > 0 || r.RefDocNoId > 0 || r.RefDocSubIdNo > 0; })) {
                throw new Error('Record cannot be add in grid because some rows already exist in grid of Loader');
            }
            var r = row(false, 0, 0, 0, 0, $id('cmbItemName').value, selText('cmbItemName'), $id('cmbPackingType').value, selText('cmbPackingType'),
                selText('cmbCropYear'), $id('cmbJobLot').value, selText('cmbJobLot'), $id('CmbJobLotTo').value, selText('CmbJobLotTo'),
                num($id('txtQty').value), $id('CmbPackUom').value, selText('CmbPackUom'), num($id('txtGrossWeight').value),
                num($id('txtEbUnit').value), num($id('txtEbTotal').value), num($id('txtAdLsWeight').value), num($id('txtDetailNetWeight').value),
                0, 0, $id('CmbWareHouseFrom').value, selText('CmbWareHouseFrom'), $id('cmbWareHouseTo').value, selText('cmbWareHouseTo'),
                $id('txtRemarksDetail').value.trim(), dbl($id('txtAvgRate').value, 'ItemRate'), 0, 40, dbl($id('txtAmount').value, 'ItemAmount'), 0);
            table.push(r);
            currentRow = table.length - 1;
            otherExpensesProportion();
            resetDetail();
            renderGrid();
        } catch (e) { fail(e); }
    }

    /** grd_DoubleClick:1784. */
    function editRow(i) {
        var item = table[i];
        if (!item) return;
        updateDetailIndex = i; currentRow = i;
        var loader = item.RefDocumentTypeId > 0;
        ['CmbWareHouseFrom', 'cmbItemName', 'CmbPackUom', 'cmbCropYear', 'cmbJobLot'].forEach(function (id) { $id(id).disabled = loader; });
        if (!loader) { $id('txtAvgRate').value = clr(item.ItemRate); $id('txtAmount').value = clr(item.ItemAmount); }
        $id('CmbWareHouseFrom').value = String(item.WareHouseFromId);
        if (item.WareHouseFromId === 0) { $id('CmbWareHouseFrom').disabled = true; $id('CmbWareHouseFrom').value = '0'; }
        $id('cmbWareHouseTo').value = String(item.WareHouseToId);
        if (transferType() === 'MoveOrder' || RefDocumentTypeId === 52) itemFillAll();
        $id('cmbItemName').value = String(item.ItemId);
        if (isEditMode() && transferType() !== 'MoveOrder' && RefDocumentTypeId !== 52) {
            setOptions('cmbItemName', [{ ItemId: item.ItemId, ItemName: item.ItemName }], 'ItemId', 'ItemName', false);
        }
        $id('cmbPackingType').value = String(item.PackTypeId);
        var crop = look.cropYears.find(function (c) { return String(c.CropYear) === item.CropYear; });
        $id('cmbCropYear').value = crop ? String(crop.Id) : '0';
        $id('cmbJobLot').value = String(item.JobLotId);
        $id('CmbJobLotTo').value = String(item.JobLotIdTo);
        $id('txtQty').value = clr(item.QTY);
        bindUoms(item.ItemId).then(function () {
            $id('CmbPackUom').value = String(item.PackUOMId);
            $id('txtGrossWeight').value = clr(item.GrossWeight);
            $id('txtEbUnit').value = clr(item.EbUnit);
            $id('txtEbTotal').value = clr(item.EbTotal);
            $id('txtAdLsWeight').value = clr(item.AdLsWeight);
            weightCalc();                                                        // txtAdLsWeight_TextChanged
            $id('txtDetailNetWeight').value = clr(item.NetWeight);
            $id('txtRemarksDetail').value = item.Remarks;
            if (transferType() === 'Inward') $id('cmbWareHouseTo').focus();
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        });
    }

    function updateDetail() {                                                    // btnUpdateDetail_Click:1865
        try {
            if (!validateDetail()) return;
            var r = table[updateDetailIndex];
            if (!r) return;
            r.ItemId = intOf($id('cmbItemName').value); r.ItemName = selText('cmbItemName');
            r.PackTypeId = intOf($id('cmbPackingType').value); r.PackType = selText('cmbPackingType');
            r.CropYear = selText('cmbCropYear');
            r.JobLotId = intOf($id('cmbJobLot').value); r.JobLot = selText('cmbJobLot');
            r.JobLotIdTo = intOf($id('CmbJobLotTo').value); r.JobLotTo = selText('CmbJobLotTo');
            r.QTY = num($id('txtQty').value);
            r.PackUOMId = intOf($id('CmbPackUom').value); r.PackUOM = selText('CmbPackUom');
            r.GrossWeight = num($id('txtGrossWeight').value);
            r.EbUnit = num($id('txtEbUnit').value); r.EbTotal = num($id('txtEbTotal').value);
            r.AdLsWeight = num($id('txtAdLsWeight').value); r.NetWeight = num($id('txtDetailNetWeight').value);
            r.WareHouseFromId = intOf($id('CmbWareHouseFrom').value); r.WareHouseFrom = selText('CmbWareHouseFrom');
            r.WareHouseToId = intOf($id('cmbWareHouseTo').value); r.WareHouseTo = selText('cmbWareHouseTo');
            r.Remarks = $id('txtRemarksDetail').value;
            if (r.RefDocumentTypeId === 0) {
                r.ItemRate = dbl($id('txtAvgRate').value, 'ItemRate');
                r.ItemAmount = dbl($id('txtAmount').value, 'ItemAmount');
            }
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            itemAmountCalc();
            resetDetail();
            otherChargesAmountUpdate();
            renderGrid();
        } catch (e) { fail(e); }
    }

    /** btnCancelUpdateDetial_Click:1951 — buttons only; the box and updateDetailIndex are left as they are. */
    function cancelDetail() { show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }

    function itemAmountCalc() {                                                  // ItemAmountCalculation:3085
        table.forEach(function (r) {
            var a = r.NetWeight / r.RateUOM * r.ItemRate;
            r.ItemAmount = (r.NetWeight > 0 && r.RateUOM > 0 && r.ItemRate > 0) ? a : 0;
        });
    }

    function resetDetail() {                                                     // ResetDetail:2330
        updateDetailIndex = -1;
        ['cmbCropYear', 'CmbPackUom', 'cmbItemName', 'cmbJobLot', 'CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbPackingType', 'CmbJobLotTo']
            .forEach(function (id) { $id(id).value = '0'; });
        ['txtDetailNetWeight', 'txtGrossWeight', 'txtAdLsWeight', 'txtQty', 'txtRemarksDetail', 'txtAvgRate', 'txtAmount']
            .forEach(function (id) { $id(id).value = ''; });
        weightCalc();
        $id('txtAvgRate').value = ''; $id('txtAmount').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        $id('cmbItemName').focus();
    }

    /* ------------------------------------------------------------------ detail grid (grd) */

    function gridCols() {                                                        // grdSettings:1661
        var type = intOf($id('CmbTransferType').value), nb = intOf($id('txtNoofBranches').value);
        var cr = table[currentRow >= 0 ? currentRow : 0];
        var cols = [['ItemName', 'ItemName'], ['PackType', 'PackType'], ['CropYear', 'CropYear'], ['JobLot', 'JobLot'], ['JobLotTo', 'JobLotTo'],
            ['QTY', 'QTY', 0], ['PackUOM', 'PackUOM'], ['GrossWeight', 'GrossWeight', 2], ['EbUnit', 'EbUnit', 2], ['EbTotal', 'EbTotal', 2],
            ['AdLsWeight', 'Ad/LsWeight', 2], ['NetWeight', 'NetWeight', 2]];
        if (!(cr && cr.RefDocumentTypeId === 0)) cols.push(['BalQty', 'BalQty', 0], ['BalWeight', 'BalWeight', 2]);
        var hideFrom = type === 1 && nb > 1, hideTo = type === 2 && nb > 1;
        if (nb <= 1) { hideFrom = false; hideTo = false; }
        if (!hideFrom) cols.push(['WareHouseFrom', 'WareHouseFrom']);
        if (!hideTo) cols.push(['WareHouseTo', 'WareHouseTo']);
        cols.push(['Remarks', 'Remarks']);
        if (look.financialEffect) cols.push(['ItemRate', 'ItemRate', 3, true], ['RateUOM', 'RateUOM'], ['ItemAmount', 'ItemAmount', 3], ['Expense', 'Expense', 3]);
        return cols;
    }

    function renderGrid() {
        var t = $id('grd'), inward = transferType() === 'Inward', cols = gridCols();
        t.tHead.innerHTML = '<tr><th>X</th>' + (inward ? '<th>+</th>' : '') + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        var tot = {};
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr data-i="' + i + '" ondblclick="StockTr.editRow(' + i + ')" onclick="StockTr.current(' + i + ')"' + (i === currentRow ? ' class="is-selected"' : '') + '>' +
                '<td><button type="button" class="cx-x" onclick="event.stopPropagation();StockTr.delRow(' + i + ')">X</button></td>' +
                (inward ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockTr.splitRow(' + i + ')">+</button></td>' : '') +
                cols.map(function (c) {
                    var v = r[c[0]];
                    if (c.length > 2) { if (!c[3]) tot[c[0]] = (tot[c[0]] || 0) + num(v); return '<td class="num">' + esc(hash(v, c[2])) + '</td>'; }
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = table.length ? '<tr style="font-weight:bold"><td></td>' + (inward ? '<td></td>' : '') + cols.map(function (c) {
            return tot[c[0]] !== undefined ? '<td class="num">' + esc(hash(tot[c[0]], c[2])) + '</td>' : '<td></td>';
        }).join('') + '</tr>' : '';
    }

    function delRow(i) {                                                         // grd_ColumnButtonClick:1932
        var r = table[i];
        if (!r) return;
        if (r.FlagForSplitAndDelete || r.Id !== 0) { alert("You Can't Delete This Row"); return; }
        table.splice(i, 1);
        if (currentRow >= table.length) currentRow = table.length - 1;
        renderGrid();
    }

    function splitRow(i) {                                                       // :1925 — ItemArray copied, Id included (D7)
        var r = table[i];
        if (!r) return;
        var c = JSON.parse(JSON.stringify(r));
        c.FlagForSplitAndDelete = false;
        table.push(c);
        renderGrid();
    }

    function sameInAllRows() {                                                   // BtnSameUpdateWarehouse_Click:3540
        try {
            var wh = $id('chkUpdateWareHouse').checked, jl = $id('chkUpdateJobLot').checked;
            if (!wh && !jl) throw new Error('Please Checked atleast one value (warehouse,JobLot) value to Update');
            if (!table.length) throw new Error('Grid Record not Found');
            var f = table[0];
            if (f.WareHouseToId === 0) throw new Error('WareHouseTo Field is Empty in Grid Row No: 01');
            table.forEach(function (r) {
                if (wh) { r.WareHouseToId = f.WareHouseToId; r.WareHouseTo = f.WareHouseTo; }
                if (jl) { r.JobLotIdTo = f.JobLotIdTo; r.JobLotTo = f.JobLotTo; }
            });
            renderGrid();
        } catch (e) { fail(e); }
    }

    /* ------------------------------------------------------------------ other charges (grdOtherCharges) */

    function blankExp() { return { Account: 0, Percentage: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }

    function renderExp() {
        var t = $id('grdOtherCharges');
        t.tHead.innerHTML = '<tr><th>AccountTitle</th><th>%</th><th>Qty</th><th>Rate</th><th>Credit</th><th>Remarks</th><th>X</th><th>+</th></tr>';
        var opts = '<option value="0"></option>' + look.accounts.map(function (a) { return '<option value="' + a.Id + '">' + esc(a.AccountTitle) + '</option>'; }).join('');
        t.tBodies[0].innerHTML = dtExp.map(function (r, i) {
            return '<tr><td><select class="win-combo" data-i="' + i + '" data-k="Account" onchange="StockTr.expCell(this)">' + opts + '</select></td>' +
                ['Percentage', 'Qty', 'Rate', 'Amount'].map(function (k) {
                    return '<td><input type="text" class="num" data-i="' + i + '" data-k="' + k + '" value="' + esc(clr(r[k])) + '" onchange="StockTr.expCell(this)"' + (k === 'Amount' ? '' : '') + '></td>';
                }).join('') +
                '<td><input type="text" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks) + '" onchange="StockTr.expCell(this)"></td>' +
                '<td><button type="button" class="cx-x" onclick="StockTr.expDel(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="StockTr.expAdd()">+</button></td></tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('select[data-k="Account"]').forEach(function (s) { s.value = String(dtExp[intOf(s.getAttribute('data-i'))].Account); });
        var total = dtExp.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        t.tFoot.innerHTML = '<tr style="font-weight:bold"><td></td><td></td><td></td><td></td><td class="num">' + esc(hash(total, 3)) + '</td><td colspan="3"></td></tr>';
    }

    function itemAmountTotal() { return table.reduce(function (a, r) { return a + num(r.ItemAmount); }, 0); }

    function expCell(el) {                                                       // grdOtherCharges_CellUpdated:1365
        var i = intOf(el.getAttribute('data-i')), k = el.getAttribute('data-k'), r = dtExp[i];
        if (!r) return;
        r[k] = (k === 'Remarks') ? el.value : (k === 'Account' ? intOf(el.value) : num(el.value));
        if ((k === 'Qty' || k === 'Rate')) { r.Amount = r.Qty * r.Rate; r.Percentage = 0; }
        if (k === 'Percentage') {
            var tp = itemAmountTotal() / 100.0 * r.Percentage;
            if (tp > 0) r.Amount = tp;
            r.Qty = 0; r.Rate = 0;
        }
        otherExpensesProportion();
        renderExp(); renderGrid();
    }

    function expDel(i) { dtExp.splice(i, 1); if (!dtExp.length) dtExp.push(blankExp()); otherExpensesProportion(); renderExp(); renderGrid(); }
    function expAdd() { dtExp.push(blankExp()); otherExpensesProportion(); renderExp(); renderGrid(); }

    function otherExpensesProportion() {                                         // :1265
        if (!table.length) return;
        var net = table.reduce(function (a, r) { return a + num(r.NetWeight); }, 0);
        var credit = dtExp.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        table.forEach(function (r) { r.Expense = credit > 0 ? credit / net * r.NetWeight : 0; });
    }

    function otherChargesAmountUpdate() {                                        // :1302
        dtExp.forEach(function (r) {
            if (num(r.Amount) > 0) {                                             // Percentage cell always has text
                var tp = itemAmountTotal() / 100.0 * num(r.Percentage);
                if (tp > 0) r.Amount = tp;
                r.Qty = 0; r.Rate = 0;
            }
        });
        otherExpensesProportion();
        renderExp();
    }

    /* ------------------------------------------------------------------ doc date (DocDate_ValueChanged:3630) */

    function docDateChanged() {
        if (!table.length) return;
        var newRateGet = true, p = Promise.resolve();
        table.forEach(function (r) {
            p = p.then(function () {
                if (transferType() === 'Inward' && r.TransferDocumentTypeId > 0 && r.TransferId > 0 && r.TransferDetailId > 0) newRateGet = false; // D13
                if (look.fifo || r.RefDocumentTypeId !== 0 || !newRateGet) return;
                return C.getJson(api + '/avg-rate' + C.qs({ recId: Id, itemId: r.ItemId, docDate: $id('DocDate').value, jobLotId: r.JobLotId,
                    cropYear: r.CropYear, warehouseId: r.WareHouseFromId })).then(function (x) {
                    var rate = num(x.rate);
                    if (rate > 0) { r.ItemRate = rate; r.ItemAmount = r.NetWeight / 40.0 * rate; }
                    else { r.ItemRate = 0; r.ItemAmount = 0; }
                });
            });
        });
        p.then(renderGrid).catch(fail);
    }

    /* ------------------------------------------------------------------ save / update / delete / print */

    /** FormValidation:1071 (client copy; the server repeats it). */
    function formValidation() {
        var t = transferType();
        if ($id('txtdocno').value.trim() === '' || $id('txtdocno').value.trim() === '0') { alert('DocNo Field is Required'); return false; }
        if (intOf($id('CmbTransferType').value) === 0) { alert('Transfer Type Field is Required'); return false; }
        if (intOf($id('cmbGatePass').value) === 0) {
            if (t !== 'MoveOrder') { alert('GatePassNo Field is Required'); return false; }
            return true;
        }
        if (intOf($id('cmbTicketNo').value) === 0) { alert('TicketNo Field is Required'); $id('cmbTicketNo').focus(); return false; }
        return true;
    }

    function insert() {                                                          // Insert:1958
        if (!formValidation()) return;
        if (!table.length) { alert('Grid Record Not Found'); return; }
        if (Id > 0) { if (!confirm('Are you sure to Update?')) return; }
        else if (!confirm('Are you sure to Save?')) return;
        var body = {
            Id: Id, DocDate: $id('DocDate').value, TransferType: transferType(),
            GatePassId: intOf($id('cmbGatePass').value), WbTicketId: intOf($id('cmbTicketNo').value),
            WbNetWeight: num($id('txtHeadNetWeight').value), OtherWeight: num($id('txtOtherWeight').value),
            NoOfBranches: intOf($id('txtNoofBranches').value), RefDocumentTypeId: RefDocumentTypeId,
            RemarksHeader: $id('txtRemarksHead').value, rows: table, expenses: dtExp
        };
        C.postJson(api + '/save', body).then(function (r) {
            alert(r.message);
            var printId = r.id;
            return reset().then(function () { if ($id('ChkBoxPrintPreview').checked) C.printSlip(api + '/' + printId + '/slip', 'Stock Transfer Slip (406)'); });
        }).catch(fail);
    }

    function save() { Id = 0; insert(); }                                        // btnsave_Click:2154
    function update() { if (IsApproved) { alert('Approved Record Not Update'); return; } insert(); }

    function del() {                                                             // btnDelete_Click:3594
        if (Id <= 0) { alert('RecordId Not Found.....'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + Id + '/delete', {}).then(function (r) { alert(r.message); return reset(); }).catch(fail);
    }

    function print() {                                                           // btnprint_Click → StockTransferSlip406
        if (Id === 0) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/' + Id + '/slip', 'Stock Transfer Slip (406)');
    }

    /* ------------------------------------------------------------------ new / reset / refresh / read */

    function reset() {                                                           // Reset:2279
        Id = 0; RefDocumentTypeId = 0; IsApproved = false;
        ['cmbCropYear', 'CmbPackUom', 'cmbGatePass', 'cmbPackingType'].forEach(function (id) { $id(id).value = '0'; });
        clearCombo('cmbTicketNo');
        ['txtHeadNetWeight', 'txtOtherWeight', 'txtRemarksHead', 'txtWorkingReportNo', 'txtAvgRate', 'txtAmount', 'txtNoofBranches']
            .forEach(function (id) { $id(id).value = ''; });
        show('btnsave', true); show('btnupdate', false); show('btnplus', true);
        show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); show('btnDelete', false);
        dtExp = [blankExp()]; renderExp();
        ['CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbItemName', 'CmbPackUom', 'cmbCropYear', 'cmbJobLot'].forEach(function (id) { $id(id).disabled = false; });
        clearCombo('cmbItemName');
        table = []; currentRow = -1;
        $id('lblRecId').innerHTML = '&nbsp;';
        return C.getJson(api + '/doc-no').then(function (r) { if (r.docNo > 0) $id('txtdocno').value = String(r.docNo); })
            .then(transferLeave)
            .then(function () { return C.getJson(api + '/pending'); }).then(renderPending)
            .then(renderGrid)
            .catch(fail);
    }

    function btnNew() { reset().then(resetDetail); }

    function refresh() {                                                         // btnRefresh_Click:2361
        C.getJson(api + '/refresh').then(function (l) {
            look.items = l.items;
            function keep(id, rows, textKey) { var v = intOf($id(id).value); setOptions(id, rows, 'Id', textKey); keepOr(id, v, rows, 'Id'); }
            keep('CmbWareHouseFrom', l.warehouses, 'WareHouseName'); keep('cmbWareHouseTo', l.warehouses, 'WareHouseName');
            keep('cmbPackingType', l.packingTypes, 'PackTypeDesc'); keep('cmbCropYear', l.cropYears, 'CropYear');
            keep('cmbJobLot', l.jobLots, 'JobLotDescription'); keep('CmbJobLotTo', l.jobLots, 'JobLotDescription');
            look.cropYears = l.cropYears;
            return gatePassFill();
        }).then(function () { return bindUoms(intOf($id('cmbItemName').value)); }).catch(fail);
    }

    function readById(id) {                                                      // ReadById:2186
        return reset().then(function () {
            Id = id;
            return C.getJson(api + '/' + id);
        }).then(function (s) {
            tab('tabForm');
            RefDocumentTypeId = intOf(s.RefDocumentTypeId);
            $id('DocDate').value = C.isoDay(s.DocDate);
            $id('txtdocno').value = String(s.DocNo);
            $id('txtNoofBranches').value = String(s.NoOfBranches);
            $id('txtRemarksHead').value = s.RemarksHeader;
            if (s.TransferType !== 'MoveOrder') {
                setOptions('cmbGatePass', [{ Id: s.GatePassId, T: s.GpSrNo }], 'Id', 'T', false);
                setOptions('cmbTicketNo', [{ Id: s.WbTicketId, T: s.TicketNo }], 'Id', 'T', false);
            } else {
                setOptions('cmbTicketNo', [{ Id: s.WbTicketId, T: s.TicketNo }], 'Id', 'T', false);
                clearCombo('cmbGatePass');
            }
            $id('txtHeadNetWeight').value = s.WbNetWeight;
            $id('txtOtherWeight').value = s.OtherWeight;
            var t = { Inward: 1, Outward: 2, MoveOrder: 3 }[s.TransferType] || 0;
            return setType(t).then(function () {
                $id('txtWorkingReportNo').value = s.WorkingReportNo;
                IsApproved = !!s.IsApproved;
                table = s.rows; currentRow = table.length ? 0 : -1;
                dtExp = s.expenses.length ? s.expenses : [blankExp()];
                renderGrid(); renderExp();
                show('btnsave', false); show('btnupdate', true); show('btnDelete', !!look.rights['delete']);
                $id('lblRecId').textContent = 'Record Id: ' + Id;
            });
        }).catch(function (e) { Id = 0; fail(e); });
    }

    /* ------------------------------------------------------------------ loader dialog */

    function openLoader() {                                                      // btnLoadInvoices_Click:3414 — a new dialog each time
        loaderRows = []; loaderChecked = {};
        $id('grdLoader').tHead.innerHTML = ''; $id('grdLoader').tBodies[0].innerHTML = '';
        $id('txtSelectedQty').value = ''; $id('txtSelectedStock').value = '';
        C.getJson(api + '/loader/lookups').then(function (l) {
            $id('ldFrom').value = l.fromDate || C.today();
            $id('ldTo').value = C.today();
            setOptions('ldParent', l.ParentCategories, 'Id', 'name'); setOptions('ldCategory', l.ItemCategories, 'Id', 'name');
            setOptions('ldType', l.ItemTypes, 'Id', 'name'); setOptions('ldJobLot', l.JobLot, 'Id', 'name');
            setOptions('ldCrop', l.CropYear, 'Id', 'name'); setOptions('ldWarehouse', l.Warehouse, 'Id', 'name');
            setOptions('ldDocType', l.DocumentType, 'Id', 'name'); setOptions('ldParty', l.Supplier_Customer, 'Id', 'name');
            setOptions('ldItem', l.Items, 'Id', 'name');
            C.openModal('dlgLoader');
            loaderSearch();
        }).catch(fail);
    }

    var LD_COLS = ['RefDocumentType', 'DocDate', 'DocCodeNo', 'ManualNo', 'GrnNo', 'SupplierCustomerName', 'VehicleNo', 'BiltyNo', 'GpNo',
        'WareHouseCode', 'RefWarehouse', 'ItemName', 'ItemCode', 'CropBatch', 'JobLotCode', 'PackingType', 'PackUom', 'QtyIn', 'QtyOut',
        'QtyBalance', 'WeightIn', 'WeightOut', 'WeightBalance', 'RateUom', 'Remarks'];
    var LD_NUM = { QtyIn: 1, QtyOut: 1, QtyBalance: 1, WeightIn: 1, WeightOut: 1, WeightBalance: 1 };

    function loaderSearch() {                                                    // PendingInventoryTransactionsForIssuanceLoad:319
        C.getJson(api + '/loader/search' + C.qs({
            fromDate: $id('ldFrom').value, toDate: $id('ldTo').value, parentCategoryId: intOf($id('ldParent').value),
            itemCategoryId: intOf($id('ldCategory').value), itemTypeId: intOf($id('ldType').value), jobLotId: intOf($id('ldJobLot').value),
            cropYear: selText('ldCrop'), warehouseId: intOf($id('ldWarehouse').value), refDocumentTypeId: intOf($id('ldDocType').value),
            supplierId: intOf($id('ldParty').value), itemId: intOf($id('ldItem').value)
        })).then(function (rows) {
            loaderRows = rows; loaderChecked = {};
            var t = $id('grdLoader');
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; selectedCalc(); return; }
            t.tHead.innerHTML = '<tr><th><input type="checkbox" onclick="StockTr.loaderAll(this.checked)"></th>' + LD_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r, i) {
                return '<tr><td><input type="checkbox" data-i="' + i + '" onclick="StockTr.loaderPick(' + i + ',this.checked)"></td>' + LD_COLS.map(function (c) {
                    var v = ci(r, c);
                    if (LD_NUM[c]) return '<td class="num">' + esc(fmt0(v, 2)) + '</td>';
                    if (c === 'DocDate') return '<td>' + esc(C.gridDate(v)) + '</td>';
                    return '<td>' + esc(v === null ? '' : v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            selectedCalc();
        }).catch(fail);
    }

    function loaderPick(i, on) { if (on) loaderChecked[i] = true; else delete loaderChecked[i]; selectedCalc(); }
    function loaderAll(on) {
        loaderChecked = {};
        $id('grdLoader').querySelectorAll('tbody input[type=checkbox]').forEach(function (c) { c.checked = on; if (on) loaderChecked[c.getAttribute('data-i')] = true; });
        selectedCalc();
    }
    function fmt00(v) { var s = Math.round(v).toLocaleString('en-US'); return Math.abs(Math.round(v)) < 10 ? (Math.round(v) < 0 ? '-0' + Math.abs(Math.round(v)) : '0' + Math.round(v)) : s; }
    function selectedCalc() {                                                    // SelectedWeightCalculation:636 — "0,0"
        var w = 0, q = 0;
        Object.keys(loaderChecked).forEach(function (k) { var r = loaderRows[k]; w += num(ci(r, 'WeightBalance')); q += num(ci(r, 'QtyBalance')); });
        $id('txtSelectedStock').value = fmt00(w); $id('txtSelectedQty').value = fmt00(q);
    }
    function loaderReset() {                                                     // btnReset_Click:552
        $id('ldParty').value = '0'; $id('ldItem').value = '0';
        loaderRows = []; loaderChecked = {};
        $id('grdLoader').tHead.innerHTML = ''; $id('grdLoader').tBodies[0].innerHTML = '';
    }

    function loaderLoad() {                                                      // btnLoadOnInvoice_Click_1:566
        var picked = Object.keys(loaderChecked).map(function (k) { return loaderRows[k]; });
        if (!picked.length) { alert('Please Select Row first'); return; }
        var dtIssuance = picked.map(function (r) {
            return {
                RefDocumentTypeId: intOf(ci(r, 'RefDocumentTypeId')), RefDocIdNo: intOf(ci(r, 'RefDocIdNo')), RefDocSubIdNo: intOf(ci(r, 'RefDocSubIdNo')),
                WareHouseId: intOf(ci(r, 'WarehouseId')), WareHouse: ci(r, 'WareHouseCode'), ItemId: intOf(ci(r, 'ItemId')), ItemName: ci(r, 'ItemName'),
                ItemUOMId: intOf(ci(r, 'ItemUom')), ItemUOM: clr(ci(r, 'PackSize')), CropYear: ci(r, 'CropBatch'), JobLotId: intOf(ci(r, 'JobLotId')),
                JobLotCode: ci(r, 'JobLotCode'), PackingTypeId: intOf(ci(r, 'InvPackingTypeId')), PackTypeCode: ci(r, 'PackingType'),
                ItemQty: num(ci(r, 'QtyBalance')), Weight: num(ci(r, 'WeightBalance')), ItemRate: num(ci(r, 'AVgRate')),
                RateUOMId: intOf(ci(r, 'RateUomId')), Equivalent: num(ci(r, 'Equivalent')), ItemAmount: num(ci(r, 'ItemAmount'))
            };
        });
        C.closeModal('dlgLoader');
        /* LoadDataDetailfromPurchaseInvoivce:3429 (D8) */
        dtIssuance.forEach(function (d) {
            var flag = false;
            if (table.length > 0) {
                table.slice().forEach(function (item) {
                    if (item.RefDocumentTypeId === 0 || item.RefDocNoId === 0) table.splice(table.indexOf(item), 1);
                    if (table.length !== 0 && d.RefDocumentTypeId === item.RefDocumentTypeId && d.RefDocIdNo === item.RefDocNoId && d.RefDocSubIdNo === item.RefDocSubIdNo) {
                        item.QTY = num(item.QTY) + d.ItemQty;
                        item.BalWeight = num(item.BalWeight) + d.Weight;
                        flag = true;
                    }
                });
            }
            if (!flag) {
                table.push(row(true, 0, d.RefDocumentTypeId, d.RefDocIdNo, d.RefDocSubIdNo, d.ItemId, d.ItemName, d.PackingTypeId, d.PackTypeCode,
                    d.CropYear, d.JobLotId, d.JobLotCode, d.JobLotId, d.JobLotCode, d.ItemQty, d.ItemUOMId, d.ItemUOM, d.Weight, 0, 0, 0, d.Weight,
                    d.ItemQty, d.Weight, d.WareHouseId, d.WareHouse, 0, '', '', d.ItemRate, d.RateUOMId, d.Equivalent, d.ItemAmount, 0, 0, 0, 0));
            }
        });
        if (currentRow < 0 && table.length) currentRow = 0;
        otherChargesAmountUpdate();
        renderGrid();
    }

    /* ------------------------------------------------------------------ history */

    var H_COLS = [['DocNo'], ['DocDate', 'd'], ['TransferType'], ['WorkingReportNo'], ['GpSrNo'], ['TicketNo'], ['WbNetWeight', 'w'],
        ['OtherWeight', 'w'], ['DiffWeight', 'w'], ['EntryDate', 'd'], ['EntryUser'], ['ModifyDate', 'd'], ['ModifyUser'], ['NoOfAttachments'], ['RemarksHeader']];
    var historyRows = [];

    function showHistory() {                                                     // gridhistoryfill:2717
        var dt = document.querySelector('input[name="hd"]:checked').value;
        C.getJson(api + '/history' + C.qs({
            dateType: dt,
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value),
            transferType: $id('cmbTransactiontypeHistory').value === '0' ? '' : $id('cmbTransactiontypeHistory').value
        })).then(function (rows) {
            historyRows = rows;
            var t = $id('grdhistory'), d = $id('GridDetailHistory');
            if (!rows.length) { t.tHead.innerHTML = t.tBodies[0].innerHTML = t.tFoot.innerHTML = ''; d.tHead.innerHTML = d.tBodies[0].innerHTML = d.tFoot.innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + H_COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
            var tot = { WbNetWeight: 0, OtherWeight: 0, DiffWeight: 0 };
            t.tBodies[0].innerHTML = rows.map(function (r, i) {
                return '<tr onclick="StockTr.historySelect(' + i + ',this)" ondblclick="StockTr.readById(' + r.Id + ')">' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockTr.readById(' + r.Id + ')">Edit</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockTr.printId(' + r.Id + ')">Print</button></td>' +
                    H_COLS.map(function (c) {
                        var v = r[c[0]];
                        if (c[1] === 'w') { tot[c[0]] += num(v); return '<td class="num">' + esc(fmt0(v, 3)) + '</td>'; }
                        if (c[1] === 'd') return '<td>' + esc(v ? C.gridDate(v) : '') + '</td>';
                        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                    }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr style="font-weight:bold"><td></td><td></td>' + H_COLS.map(function (c) {
                return c[1] === 'w' ? '<td class="num">' + esc(fmt0(tot[c[0]], 3)) + '</td>' : '<td></td>';
            }).join('') + '</tr>';
        }).catch(fail);
    }

    var HD_COLS = [['FromWarehouse'], ['ToWarehouse'], ['ItemName'], ['PackingType'], ['CropYear'], ['JobLotFrom'], ['JobLotTo'], ['Qty', 2],
        ['PackUom'], ['GrossWeight', 2], ['EbUnit', 2], ['EbTotal', 2], ['AddLessWt', 2], ['StockWeight', 2], ['ItemRate', 3, true],
        ['ItemAmount', 2], ['ExpenseAmount', 2], ['RemarksDetail']];

    function historySelect(i, tr) {                                              // grdhistory_SelectionChanged:2910
        $id('grdhistory').querySelectorAll('tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        var r = historyRows[i];
        C.getJson(api + '/' + r.Id + '/history-detail').then(function (rows) {
            if (!rows.length) return;                                            // the desktop leaves the previous detail
            var d = $id('GridDetailHistory'), tot = {};
            d.tHead.innerHTML = '<tr>' + HD_COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
            d.tBodies[0].innerHTML = rows.map(function (x) {
                return '<tr>' + HD_COLS.map(function (c) {
                    var v = x[c[0]];
                    if (c.length > 1) { if (!c[2]) tot[c[0]] = (tot[c[0]] || 0) + num(v); return '<td class="num">' + esc(fmt0(v, c[1])) + '</td>'; }
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            d.tFoot.innerHTML = '<tr style="font-weight:bold">' + HD_COLS.map(function (c) {
                return tot[c[0]] !== undefined ? '<td class="num">' + esc(fmt0(tot[c[0]], c[1])) + '</td>' : '<td></td>';
            }).join('') + '</tr>';
        }).catch(fail);
    }

    function printId(id) { C.printSlip(api + '/' + id + '/slip', 'Stock Transfer Slip (406)'); }

    /* ------------------------------------------------------------------ tabs */

    /* toolStrip2 (History tab) — btnNewHistory_Click:2626. The desktop clears grdhistory AND grdDetail
       (the pending-entries grid on the Form tab), not GridDetailHistory: reproduced as is. */
    function resetHistory() {
        $id('FromDateHistory').value = C.today();
        $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = '';
        $id('txtToDocNoHistory').value = '';
        $id('cmbTransactiontypeHistory').value = '0';
        var h = $id('grdhistory');
        h.tHead.innerHTML = ''; h.tBodies[0].innerHTML = ''; h.tFoot.innerHTML = '';
        renderPending([]);
        $id('drdocdate').checked = true;
    }
    /* btnRefreshHistory_Click:2645 → HistoryComboFill:2669 (transfer types of DocumentTypeIds "68"). */
    function refreshHistory() {
        C.getJson(api + '/lookups').then(function (l) {
            setOptions('cmbTransactiontypeHistory', l.historyTransferTypes, 'TransferType', 'TransferType');
        }).catch(fail);
    }

    function tab(id) {
        document.querySelectorAll('.win-tab[data-tab]').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }
    function subTab(id) {
        document.querySelectorAll('.win-tab[data-sub]').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-sub') === id); });
        ['tabPage3', 'tabPage4'].forEach(function (p) { show(p, p === id); });
    }
    function current(i) {
        currentRow = i;
        $id('grd').querySelectorAll('tbody tr').forEach(function (x) { x.classList.toggle('is-selected', intOf(x.getAttribute('data-i')) === i); });
    }

    window.StockTr = {
        btnNew: btnNew, refresh: refresh, save: save, update: update, print: print, del: del, openLoader: openLoader,
        tab: tab, subTab: subTab, current: current, ticketLeave: ticketLeave, loadPending: loadPending,
        itemLeave: itemLeave, stockAndRate: stockAndRate, qtyChanged: qtyChanged, weightCalc: weightCalc,
        add: add, editRow: editRow, updateDetail: updateDetail, cancelDetail: cancelDetail, delRow: delRow, splitRow: splitRow,
        sameInAllRows: sameInAllRows, expCell: expCell, expDel: expDel, expAdd: expAdd, docDateChanged: docDateChanged,
        loaderSearch: loaderSearch, loaderLoad: loaderLoad, loaderReset: loaderReset, loaderPick: loaderPick, loaderAll: loaderAll,
        showHistory: showHistory, historySelect: historySelect, readById: readById, printId: printId,
        resetHistory: resetHistory, refreshHistory: refreshHistory
    };
    document.addEventListener('DOMContentLoaded', init);
})();
