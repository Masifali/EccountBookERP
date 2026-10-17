/* ============================================================================================
 * Screen 593 "Work Order" - Architecture.WinApp.Mfg.frmWorkOrder (base.Name "frmWorkOrder", DocumentTypeId 1614).
 * Desktop line numbers (.cs): _Load:353, ItemFill:445, DocumentNoFill:418, BomNoFill:501, cmbItem_Leave:621, CustomerFill:679, BindCustomerOrders:747,
 * FormValidation:918, cmbBOMNo_Leave:1016, GridFillByBomOperationDetail:1028, UpdateBOMInDetailGrid:1300, btnRefresh_Click:1809, Insert():1883,
 * ReadById:2125, btnDelete_Click:2223, HistoryGridFill:2437, SaveAs:2641, DatagridHistory_SelectionChanged:2658.
 * API /production/mfg/work-order/api. Not ported: Attachment dialog.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/work-order/api';
    var $ = W3B.$;
    var look = {}, rights = {}, wsoMap = {}, uoms = [];
    var recId = 0;
    var ops = [], items = [], custs = [], rmOps = [], rmItems = [], rmCusts = [], hist = [], orders = [];

    function applyRights() {
        $('btnSave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights.delete;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update'); if (!rights.delete) m.push('Delete');
        var n = $('rightsNote');
        n.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        n.classList.toggle('is-hidden', !m.length);
    }
    function mode() {                                          // new vs edit buttons
        var edit = recId > 0;
        W3B.show('btnSave', !edit); W3B.show('btnUpdate', edit); W3B.show('btnDelete', edit); W3B.show('btnPrint', edit);
    }
    function applyCombos(d) {
        W3B.fill('cmbItem', d.items, true); W3B.fill('cmbMatItem', d.items, true);
        W3B.fill('cmbWso', d.workStationOperations, true);
        wsoMap = {}; (d.workStationOperations || []).forEach(function (o) { wsoMap[o.Id] = o; });
        uoms = (d.allUoms || []).map(function (u) { return { Id: u.Id, ItemId: u.ItemId, Name: u.UOMCode || u.UOMDescription }; });
    }

    // ----------------------------------------------------------------------------- grids
    function cell(f, i, v, g) { return '<input class="cell" style="width:70px" inputmode="decimal" data-g="' + g + '" data-f="' + f + '" data-i="' + i + '" value="' + W3B.esc(v) + '">'; }
    function xbtn(g, i) { return '<button type="button" class="rc-x" data-g="' + g + '" data-act="del" data-i="' + i + '">X</button>'; }
    function render() {
        $('grdOperations').innerHTML = W3B.tableHtml([
            { h: '', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xbtn('ops', i); } },
            { h: 'WorkStation', k: 'WorkStationName', w: 150 }, { h: 'Operation', k: 'OperationName', w: 150 },
            { h: 'OpHours', html: true, cls: 'num', f: function (r, i) { return cell('OpHours', i, r.OpHours, 'ops'); } },
            { h: 'OpHoursPerDay', html: true, cls: 'num', f: function (r, i) { return cell('OpHoursPerDay', i, r.OpHoursPerDay, 'ops'); } },
            { h: 'OpDays', html: true, cls: 'num', f: function (r, i) { return cell('OpDays', i, r.OpDays, 'ops'); } },
            { h: 'DeadLine', html: true, f: function (r, i) { return '<input type="date" class="cell txt" style="width:120px" data-g="ops" data-f="DeadLine" data-i="' + i + '" value="' + W3B.esc(r.DeadLine) + '">'; } }
        ], ops, { empty: 'No operations' });
        $('grdItems').innerHTML = W3B.tableHtml([
            { h: '', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xbtn('items', i); } },
            { h: 'Item', k: 'ItemName', w: 220 }, { h: 'UOM', k: 'UOM', w: 80 },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return cell('Qty', i, r.Qty, 'items'); } }
        ], items, { empty: 'No items' });
        $('grdCustomers').innerHTML = W3B.tableHtml([
            { h: '', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xbtn('custs', i); } },
            { h: 'Customer', k: 'CustomerName', w: 200 }, { h: 'SaleOrder', k: 'SaleOrder', cls: 'num', w: 70 },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return cell('Qty', i, r.Qty, 'custs'); } }
        ], custs, { empty: 'No customers' });
    }
    function arr(g) { return g === 'ops' ? ops : g === 'items' ? items : custs; }
    function removed(g) { return g === 'ops' ? rmOps : g === 'items' ? rmItems : rmCusts; }
    document.addEventListener('input', function (e) {
        var t = e.target; if (!t.dataset || !t.dataset.g) return;
        var r = arr(t.dataset.g)[+t.dataset.i]; if (!r) return;
        r[t.dataset.f] = t.type === 'date' ? t.value : W3B.num(t.value);
    });
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('button[data-act="del"]') : null; if (!b) return;
        var g = b.dataset.g, a = arr(g), i = +b.dataset.i, r = a[i];
        if (!r) return;
        if (recId > 0 && r.Id > 0) removed(g).push(r);        // removed rows are sent with ActionTypeId 3 (:1956, :1999, :2036)
        a.splice(i, 1); render();
    });

    // ----------------------------------------------------------------------------- entry
    function addOp() {
        var id = W3B.int($('cmbWso').value); if (!id) { W3B.box('Select Work Station / Operation'); return; }
        if (ops.some(function (r) { return r.WorkStationOperationId === id; })) { W3B.box('Operation already added'); return; }
        var w = wsoMap[id] || {};
        ops.push({ Id: 0, BOMHeaderId: 0, BOMOperationDetailId: 0, WorkStationOperationId: id, WorkStationName: w.WorkStationName || '', OperationName: w.OperationName || '',
            OpHours: 0, OpHoursPerDay: w.OpHoursPerDay || 0, OpDays: 0, DeadLine: W3B.today() });
        render();
    }
    function addMat() {
        var id = W3B.int($('cmbMatItem').value), uom = W3B.int($('cmbMatUom').value), q = W3B.num($('txtMatQty').value);
        if (!id) { W3B.box('Select Item'); return; }
        if (!uom) { W3B.box('Uom Field Required'); return; }
        if (!q) { W3B.box('Qty Field Required'); return; }
        if (items.some(function (r) { return r.ItemId === id; })) { W3B.box('Item already added'); return; }
        items.push({ Id: 0, ItemId: id, ItemName: W3B.selText('cmbMatItem'), UOMId: uom, UOM: W3B.selText('cmbMatUom'), Qty: q });
        $('txtMatQty').value = ''; render();
    }
    function addCust() {
        var c = W3B.int($('cmbCustomer').value), so = W3B.int($('cmbSaleOrder').value), q = W3B.num($('txtCustQty').value);
        if (!c) { W3B.box('Select Customer'); return; }
        if (!so) { W3B.box('SaleOrder Field Required'); return; }
        if (!q) { W3B.box('Qty Field Required'); return; }
        var o = orders.filter(function (x) { return x.OrderId === so; })[0] || {};
        custs.push({ Id: 0, CustomerId: c, CustomerName: W3B.selText('cmbCustomer'), SaleOrderId: so, SaleOrder: o.OrderNo || so, Qty: q });
        $('txtCustQty').value = ''; render();
    }

    function uomOptions(id) {
        var rows = uoms.filter(function (u) { return String(u.ItemId) === String(id); });
        W3B.fill('cmbMatUom', rows.length ? rows : uoms, true);
    }
    function matItemLeave() { uomOptions($('cmbMatItem').value); }

    function itemLeave() {                                      // cmbItem_Leave:621
        var id = W3B.int($('cmbItem').value);
        uomOptions(id);
        W3B.fill('cmbItemUom', uoms.filter(function (u) { return String(u.ItemId) === String(id); }), true);
        if (!id) { W3B.fill('cmbBOMNo', [], true); W3B.fill('cmbCustomer', [], true); orders = []; return Promise.resolve(); }
        return W3B.getJson(api + '/item-info?itemId=' + id).then(function (d) {
            W3B.fill('cmbBOMNo', d.boms, true); W3B.fill('cmbCustomer', d.customers, true);
            orders = d.orders || []; W3B.fill('cmbSaleOrder', [], true);
        }).catch(W3B.fail);
    }
    function custLeave() {                                      // BindCustomerOrders:747
        var c = W3B.int($('cmbCustomer').value);
        var rows = orders.filter(function (o) { return o.CustomerId === c; }).map(function (o) { return { Id: o.OrderId, Name: o.OrderNo }; });
        W3B.fill('cmbSaleOrder', rows, true);
        var o = orders.filter(function (x) { return x.CustomerId === c; })[0];
        if (o) { $('cmbSaleOrder').value = String(o.OrderId); $('txtCustQty').value = o.OrderQty; }
    }
    function orderLeave() {
        var so = W3B.int($('cmbSaleOrder').value), o = orders.filter(function (x) { return x.OrderId === so; })[0];
        if (o) $('txtCustQty').value = o.OrderQty;
    }
    function bomLeave() {                                       // cmbBOMNo_Leave:1016 -> GridFillByBomOperationDetail:1028
        var id = W3B.int($('cmbBOMNo').value); if (!id) return;
        W3B.getJson(api + '/bom-operations?bomId=' + id).then(function (rows) {
            ops.forEach(function (r) { if (recId > 0 && r.Id > 0) rmOps.push(r); });
            ops = rows; render();
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- toolbar
    function resetForm() {
        recId = 0; ops = []; items = []; custs = []; rmOps = []; rmItems = []; rmCusts = []; orders = [];
        ['txtManualDocCode', 'txtBatchDesc', 'txtItemQty', 'txtRemarksHeader', 'txtMatQty', 'txtCustQty'].forEach(function (i) { $(i).value = ''; });
        ['cmbItem', 'cmbItemUom', 'cmbBOMNo', 'cmbWso', 'cmbMatItem', 'cmbMatUom', 'cmbCustomer', 'cmbSaleOrder'].forEach(W3B.clearCombo);
        var t = W3B.today(); $('dtpDocDate').value = t; $('dtpStartDate').value = t; $('dtpEndDate').value = t; $('dtpDeadLine').value = t;
        mode(); render();
    }
    function newClick() {
        resetForm();
        W3B.getJson(api + '/doc-no').then(function (d) { $('txtDocNo').value = d.docNo; }).catch(W3B.fail);
    }
    function refresh() {                                        // btnRefresh_Click:1809
        W3B.getJson(api + '/refresh').then(function (d) { applyCombos(d); }).catch(W3B.fail);
    }
    function body() {
        return { id: recId, docNo: $('txtDocNo').value, docDate: $('dtpDocDate').value, manualDocCode: $('txtManualDocCode').value,
            bomHeaderId: W3B.int($('cmbBOMNo').value), itemId: W3B.int($('cmbItem').value), itemUomId: W3B.int($('cmbItemUom').value),
            itemQty: W3B.num($('txtItemQty').value), startDate: $('dtpStartDate').value, endDate: $('dtpEndDate').value, deadLine: $('dtpDeadLine').value,
            batchDesc: $('txtBatchDesc').value, remarksHeader: $('txtRemarksHeader').value,
            operations: ops, items: items, customers: custs, removedOperations: rmOps, removedItems: rmItems, removedCustomers: rmCusts };
    }
    function save(asNew) {                                      // Insert():1883
        if (!ops.length) { W3B.box('Operation Grid Record Not Found...\nGrid Should have atleast one record'); return; }
        if (!items.length) { W3B.box('Item Grid Record Not Found...\nGrid Should have atleast one record'); return; }
        if (!W3B.ask(recId > 0 && !asNew ? 'Are you sure you want to Update?' : 'Are you sure you want to Save?')) return;
        var b = body(); if (asNew) { b.id = 0; b.removedOperations = []; b.removedItems = []; b.removedCustomers = []; }
        W3B.postJson(api + '/save', b).then(function (d) {
            W3B.box(d.message); resetForm();
            return W3B.getJson(api + '/doc-no').then(function (n) { $('txtDocNo').value = n.docNo; });
        }).catch(W3B.fail);
    }
    function saveAs() {                                         // SaveAs:2641 - load, then save as a new order
        if (recId <= 0) { W3B.box('Open a Work Order from History first'); return; }
        if (!rights.save) { W3B.box('You do not have the Save right on Work Order.'); return; }
        ops.forEach(function (r) { r.Id = 0; }); items.forEach(function (r) { r.Id = 0; }); custs.forEach(function (r) { r.Id = 0; });
        W3B.getJson(api + '/doc-no').then(function (n) { $('txtDocNo').value = n.docNo; recId = 0; mode(); save(true); }).catch(W3B.fail);
    }
    function del() {                                            // btnDelete_Click:2223
        if (recId <= 0) { W3B.box('No record found to Delete'); return; }
        if (!W3B.ask('Are you sure you want to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { W3B.box(d.message); newClick(); }).catch(W3B.fail);
    }
    function print() { if (recId > 0) window.open('/reports/print/1614-work-order-slip?id=' + recId, '_blank'); }

    function load(id) {                                         // ReadById:2125
        return W3B.getJson(api + '/read/' + id).then(function (d) {
            resetForm(); recId = d.id;
            $('txtDocNo').value = d.docNo; $('dtpDocDate').value = d.docDate; $('txtManualDocCode').value = d.manualDocCode;
            $('dtpStartDate').value = d.startDate; $('dtpEndDate').value = d.endDate; $('dtpDeadLine').value = d.deadLine;
            $('txtRemarksHeader').value = d.remarksHeader; $('txtBatchDesc').value = d.batchDesc; $('txtItemQty').value = d.itemQty;
            $('cmbItem').value = String(d.itemId);
            return itemLeave().then(function () {
                W3B.setCombo('cmbItemUom', d.itemUomId); W3B.setCombo('cmbBOMNo', d.bomHeaderId);
                ops = d.operations; items = d.items; custs = d.customers; mode(); render(); tab('form');
            });
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
    }
    function dateType() {                                       // Datetype selection (CommonServices date ranges)
        var v = W3B.int($('cmbDateType').value), n = new Date(), f = new Date(n), t = new Date(n);
        if (v === 2) { var d = n.getDay(); f.setDate(n.getDate() - d); t.setDate(f.getDate() + 6); }
        else if (v === 3) { f = new Date(n.getFullYear(), n.getMonth(), 1); t = new Date(n.getFullYear(), n.getMonth() + 1, 0); }
        else if (v === 4) { f = new Date(n.getFullYear(), 0, 1); t = new Date(n.getFullYear(), 11, 31); }
        else if (v === 5) { f = new Date(look.financialYearStart || n); t = new Date(f.getFullYear() + 1, f.getMonth(), f.getDate() - 1); }
        $('dtpFrom').value = W3B.iso(f); $('dtpTo').value = W3B.iso(t);
    }
    function history() {                                        // HistoryGridFill:2437
        W3B.getJson(api + '/history' + W3B.qs({ fromDate: $('dtpFrom').value, toDate: $('dtpTo').value, fromDocNo: W3B.int($('txtFromDocNo').value),
            toDocNo: W3B.int($('txtToDocNo').value), workOrderId: W3B.int($('cmbHistWo').value), itemId: W3B.int($('cmbHistItem').value) })).then(function (rows) {
            hist = rows;
            $('DatagridHistory').innerHTML = W3B.tableHtml([
                { h: '', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hist="' + i + '">Edit</button>'; } },
                { h: 'RecordNo', k: 'RecordNo', cls: 'num' }, { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (r) { return W3B.fmtDMY(r.DocDate); } },
                { h: 'ManualDocCode', k: 'ManualDocCode' }, { h: 'BomNo', k: 'BomNo' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'Uom' },
                { h: 'ItemQty', k: 'ItemQty', cls: 'num' }, { h: 'StartDate', f: function (r) { return W3B.fmtDMY(r.StartDate); } },
                { h: 'EndDate', f: function (r) { return W3B.fmtDMY(r.EndDate); } }, { h: 'DeadLine', f: function (r) { return W3B.fmtDMY(r.DeadLine); } },
                { h: 'BatchDesc', k: 'BatchDesc' }, { h: 'RemarksHeader', k: 'RemarksHeader' }, { h: 'EntryUser', k: 'EntryUserName' },
                { h: 'EntryDate', f: function (r) { return W3B.fmtStamp(r.EntryDate); } }, { h: 'ModifyUser', k: 'ModifyUserName' },
                { h: 'ModifyDate', f: function (r) { return W3B.fmtStamp(r.ModifyDate); } }, { h: 'IsApproved', bool: true, k: 'IsApproved', cls: 'ctr' },
                { h: 'Attachments', k: 'NoOfAttachments', cls: 'num' }
            ], rows, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('button[data-hist]') : null;
        if (b) {
            if (!rights.update) { W3B.box("you don't have update rights..."); return; }
            load(hist[+b.dataset.hist].Id); return;
        }
        var tr = e.target.closest ? e.target.closest('#DatagridHistory tbody tr[data-i]') : null;   // SelectionChanged:2658 - operations of the order
        if (tr) {
            var id = hist[+tr.dataset.i].Id;
            W3B.getJson(api + '/read/' + id).then(function (d) {
                $('grdHistOps').innerHTML = W3B.tableHtml([
                    { h: 'WorkStation', k: 'WorkStationName' }, { h: 'Operation', k: 'OperationName' }, { h: 'OpHours', k: 'OpHours', cls: 'num' },
                    { h: 'OpHoursPerDay', k: 'OpHoursPerDay', cls: 'num' }, { h: 'OpDays', k: 'OpDays', cls: 'num' }, { h: 'DeadLine', f: function (r) { return W3B.fmtDMY(r.DeadLine); } }
                ], d.operations, { empty: 'No operations' });
            }).catch(W3B.fail);
        }
    });

    window.Wo = { newClick: newClick, refresh: refresh, save: function () { save(false); }, saveAs: saveAs, del: del, print: print, tab: tab,
        addOp: addOp, addMat: addMat, addCust: addCust, itemLeave: itemLeave, matItemLeave: matItemLeave, custLeave: custLeave, orderLeave: orderLeave,
        bomLeave: bomLeave, history: history, dateType: dateType };

    W3B.keys({ n: newClick, s: function () { save(false); }, u: function () { if (recId > 0) save(false); }, r: refresh });

    W3B.getJson(api + '/lookups').then(function (d) {
        look = d; rights = d.rights || {}; applyRights(); applyCombos(d);
        W3B.fill('cmbDateType', (d.dateTypes || []), false);
        W3B.fill('cmbHistWo', d.bomNames, true); W3B.fill('cmbHistItem', d.headerItems, true);
        resetForm(); $('txtDocNo').value = d.docNo; dateType();
    }).catch(W3B.fail);
}());
