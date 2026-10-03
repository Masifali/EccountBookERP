/* ============================================================================================
 * countx_export_delivery_order_new.js - ExportDeliveryOrderNew.cs (Architecture.WinApp.Export), screen 201
 * "Export Delivery Order" (InvDeliveryOrder, DocumentTypeId 84, DeliveryOrderType "Export"). Form | History;
 * API /api/export/delivery-order-new.
 * Every desktop event has a counterpart with the desktop's messages and order. The desktop's TextChanged chains that fire
 * when the code itself sets a text box (txtqty -> CalculaterPackingWeight + CalculateWeight, txtTotalPackingWeight ->
 * gross + CalculaterPackingWeight, txtPackingWt -> CalculaterPackingWeight, txtOtherWeight -> CalculateTotalInformation)
 * are replayed by setFire(), which - like a TextBox - only fires when the text really changes.
 * Quirks kept on purpose:
 *   - CalculaterPackingWeight decides by the focused control's Tag (Pack Weight -> T.Pack = PW * Qty; T.Pack Weight ->
 *     PW = T.Pack / Qty; anything else -> T.Pack = PW * Qty), so selecting an item overwrites the invoice's
 *     TotalPackingWeight with PackingWeight * Qty;
 *   - NetWeight (the form field) is only changed by CalculateWeight and Reset;
 *   - ReadById writes the invoice ID into the invoice combo TEXT (shown as typed when no Invoice No matches);
 *   - the removed-rows list (lstRemoveRecord) is never cleared - it goes with every later save (a new document then fails
 *     with "Record cannot be inserted because ActionTypeId not equal to 1");
 *   - Reset blanks Other Weight before the grid, so Item / Packing Weight are recomputed from the old rows;
 *   - grd DoubleClick does not restore Container Remarks; the transporter combo is never filled (TransporterId 0);
 *   - History "New" resets From Date to today - 3 (not the configured days) and keeps the Invoice filter.
 * Button contract: disabled + spinner while a request runs, no duplicates, re-enabled on success and failure.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExR2;
    var $id = X.$id, box = X.box, esc = X.esc, str = X.str, netI = X.netI, netD = X.netD, clr = X.clr;
    var API = '/api/export/delivery-order-new';
    var DOC_TYPE = 84;
    var PERM = { Save: true, Update: true, Print: true };
    var CFG = { DefaultDaysToLessFromHistoryFromDate: 0, ContainerNoCompulsoryOnDeliveryOrderExport: false, DefaultJobLotId: 0, DefaultWarehouseId: 0 };
    var S = { recId: 0, rows: [], removed: [], updateIdx: -1, cur: -1, netWeight: 0, branchCount: 0, userBranchId: 0, contFromReadById: [], items: [], uoms: [], isApproved: false };
    var HIST = [], HCUR = -1;
    var invSeq = 0, itemSeq = 0, orderSeq = 0;

    function f2(n) { return isFinite(n) ? X.fmtNet(n, '#,##0.##') : clr(n); }

    // ------------------------------------------------------------------------------ tabs

    function tab(panelId) {
        document.querySelectorAll('.win-tabs[data-tabs="edon"] .win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        ['tabPage1', 'tabPage2'].forEach(function (p) { $id(p).classList.toggle('is-active', p === panelId); });
        var onHist = panelId === 'tabPage2';
        var fb = $id('btnEdonFooterHistory');
        fb.querySelector('span').textContent = onHist ? 'Form' : 'History';
        fb.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged */
        if (onHist) X.focus('FromDateHistory'); else X.focus('DocDate');
    }
    function onHistory() { return $id('tabPage2').classList.contains('is-active'); }
    function toggleHistory() { tab(onHistory() ? 'tabPage1' : 'tabPage2'); }

    // ------------------------------------------------------------------------------ text-changed replay

    var HANDLERS = {};
    function setFire(id, v) {
        var e = $id(id); if (!e) return;
        v = str(v);
        if (e.value === v) return;
        e.value = v;
        if (HANDLERS[id]) HANDLERS[id]();
    }
    function activeTag() { var a = document.activeElement; return a && a.getAttribute ? str(a.getAttribute('data-tag')) : ''; }

    /** CalculaterPackingWeight. */
    function calcPackingWeight() {
        var pw = netD(X.val('txtPackingWt')), tpw = netD(X.val('txtTotalPackingWeight')), qty = netD(X.val('txtqty'));
        var tag = activeTag();
        if (tag === 'TotalPackingWeight') setFire('txtPackingWt', f2(tpw / qty));
        else setFire('txtTotalPackingWeight', f2(pw * qty));
    }
    /** txtPackingWeight_TextChanged (handler of txtTotalPackingWeight). */
    function totalPackingChanged() {
        if (S.netWeight > 0) X.setText('txtGrossWeight', f2(S.netWeight + netD(X.val('txtTotalPackingWeight'))));
        else X.setText('txtGrossWeight', f2(S.netWeight));
        calcPackingWeight();
    }
    function selectedUom() { var v = X.val('CmbPackUom'); if (v === '') return null; return X.find(S.uoms, 'PackUomId', v); }
    /** CalculateWeight (CmbPackUom Leave, txtqty TextChanged). */
    function calculateWeight() {
        X.setText('txtweight', '0');
        X.setText('txtGrossWeight', '0');
        var u = selectedUom();
        if (u && netD(X.val('txtqty')) !== 0) {
            S.netWeight = netD(u.Equivalent) * netD(X.val('txtqty'));
            X.setText('txtweight', f2(S.netWeight));
            X.setText('txtGrossWeight', f2(S.netWeight));
            totalPackingChanged();
        }
    }
    /** txtqty_TextChanged. */
    function qtyChanged() { calcPackingWeight(); calculateWeight(); }

    HANDLERS.txtqty = qtyChanged;
    HANDLERS.txtTotalPackingWeight = totalPackingChanged;
    HANDLERS.txtPackingWt = calcPackingWeight;
    HANDLERS.txtOtherWeight = function () { calculateTotalInformation(); };

    // ------------------------------------------------------------------------------ load

    function activateFirst(id) { var s = $id(id); if (s && s.options.length > 1) { s.selectedIndex = 1; X.refreshCombos(); } }
    function clearCombo(id) { X.bind(id, [], 'Id', 'Id'); }
    function bindInvoices(rows) {
        clearRaw('cmbInvoiceNo');
        if (rows && rows.length) X.bind('cmbInvoiceNo', rows, 'Id', 'InvoiceNo', { keep: true });
        else clearCombo('cmbInvoiceNo');
    }
    function applyBranches(rows) {
        S.branchCount = (rows || []).length;
        if (S.branchCount > 0) { X.bind('CmbBranch', rows, 'Id', 'BranchName'); activateFirst('CmbBranch'); }
        var vis = S.branchCount > 1;
        X.show('lblBranchName', vis); X.show('CmbBranchWrap', vis);
    }
    /** defaultConfiquration: Job/Lot and Warehouse from the configuration. */
    function defaultConfiguration() {
        if (CFG.DefaultJobLotId) X.setVal('CmbJobLot', CFG.DefaultJobLotId);
        if (CFG.DefaultWarehouseId) X.setVal('CmbWareHouse', CFG.DefaultWarehouseId);
    }
    function bindHistoryCombos(h) {
        h = h || {};
        X.bind('CmbCustomerHistory', h.customers, 'Id', 'Customer');
        X.bind('CmbInvoiceNoHistory', h.invoices, 'Id', 'InvoiceNo');
    }

    /** PurchsaeOrder_Load. */
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('btnupdate').disabled = !PERM.Update;
            if (d.config) CFG = d.config;
            S.userBranchId = netI(d.userBranchId);
            bindInvoices(d.invoices);
            if (d.packingTypes && d.packingTypes.length) { X.bind('CmbPackingType', d.packingTypes, 'Id', 'PackTypeDesc'); activateFirst('CmbPackingType'); }
            if (d.docNo) X.setText('txtdocno', d.docNo);
            if (d.warehouses && d.warehouses.length) X.bind('CmbWareHouse', d.warehouses, 'Id', 'WareHouseName');
            if (d.jobLots && d.jobLots.length) X.bind('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription');
            clearCombo('cmbtransporter');
            X.setText('DocDate', X.today());
            X.setText('txtInvoiceDate', X.today());
            showEntryButtons(false);
            X.show('btnupdate', false); X.show('btnsave', true);
            applyBranches(d.branches);
            X.setVal('CmbBranch', S.userBranchId);
            defaultConfiguration();
            bindHistoryCombos(d.historyCombos);
            var days = netI(CFG.DefaultDaysToLessFromHistoryFromDate);
            X.setText('FromDateHistory', X.addDays(X.today(), days > 0 ? -days : -3));
            X.setText('ToDateHistory', X.today());
            render();
            $id('edonFooterInfo').textContent = 'ExportDeliveryOrderNew  -  Document Type 84';
            X.focus('DocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    // ------------------------------------------------------------------------------ raw combo texts

    /** UltraCombo.Text / Value set to something that is not in the list: the text stays, Value = that text. */
    function setRaw(id, v) {
        clearRaw(id);
        var s = $id(id); if (!s) return;
        v = str(v);
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === v && v !== '') { s.selectedIndex = i; X.refreshCombos(); return; }
        if (v === '' || v === '0') { s.value = ''; X.refreshCombos(); return; }
        var o = document.createElement('option'); o.value = v; o.text = v; o.setAttribute('data-raw', '1');
        s.appendChild(o); s.value = v; X.refreshCombos();
    }
    function clearRaw(id) { var s = $id(id); if (!s) return; s.querySelectorAll('option[data-raw]').forEach(function (o) { o.parentNode.removeChild(o); }); }

    // ------------------------------------------------------------------------------ invoice / order / item chain

    /** GetInvoicewiseDoWeightandBalanceWeight (cmbInvoiceNo_TextChanged). */
    function applyWeights(w) {
        w = w || {};
        X.setText('txtInvoiceWeight', str(w.InvoiceWeight)); X.setText('txtDoWeight', str(w.DoWeight));
        X.setText('txtrejectedWeight', str(w.GpRejectedWeight)); X.setText('txtBalanceWeight', str(w.WeightAvailableForDo));
    }
    function invoiceTextChanged() {
        return X.getJson(API + '/invoice-weights?invoiceId=' + netI(X.val('cmbInvoiceNo'))).then(applyWeights).catch(function (e) { box(e.message); });
    }
    /** GetContainerNoFromCROByInvoiceId: keeps the selected container, adds the containers of the loaded record. */
    function bindContainers(list) {
        var conNo = netI(X.val('cmbContainerNoDetail'));
        if (list && list.length) {
            var rows = list.slice();
            S.contFromReadById.forEach(function (c) {
                if (!rows.some(function (r) { return netI(r.ContainerId) === netI(c.ContainerId); })) rows.push({ ContainerId: c.ContainerId, ContainerNo: c.ContainerNo });
            });
            X.bind('cmbContainerNoDetail', rows, 'ContainerId', 'ContainerNo');
        } else clearCombo('cmbContainerNoDetail');
        if (conNo > 0) X.setVal('cmbContainerNoDetail', conNo);
    }
    /** cmbInvoiceNo_Leave: GetDatabyInvoiceId + GetInvoicewiseDoWeightandBalanceWeight + containers. */
    function invoiceLeave() {
        var seq = ++invSeq;
        X.setVal('CmbSupplierCustomer', ''); clearCombo('CmbSupplierCustomer');
        X.setVal('CmbOrderNo', ''); clearCombo('CmbOrderNo');
        var inv = netI(X.val('cmbInvoiceNo'));
        return X.getJson(API + '/invoice?invoiceId=' + inv + '&recId=' + S.recId).then(function (d) {
            if (seq !== invSeq) return;
            if (d.orders && d.orders.length) { X.bind('CmbOrderNo', d.orders, 'OrderId', 'OrderNo'); activateFirst('CmbOrderNo'); }
            if (d.customers && d.customers.length) {
                X.setText('txtInvoiceDate', d.invoiceDate || X.today());
                X.bind('CmbSupplierCustomer', d.customers, 'Id', 'CustomerName'); activateFirst('CmbSupplierCustomer');
            }
            applyWeights(d.weights);
            if (inv !== 0) bindContainers(d.containers); else clearCombo('cmbContainerNoDetail');
            return orderTextChanged();
        }).catch(function (e) { box(e.message); });
    }
    /** CmbOrderNo_TextChanged: items and crop years of the invoice + contract. */
    function orderTextChanged() {
        var seq = ++orderSeq;
        var inv = netI(X.val('cmbInvoiceNo')), ord = netI(X.val('CmbOrderNo'));
        if (!(inv > 0 && ord > 0)) { clearItems(); return Promise.resolve(); }
        return X.getJson(API + '/contract-items?invoiceId=' + inv + '&orderId=' + ord).then(function (rows) {
            if (seq !== orderSeq) return;
            rows = rows || [];
            if (!rows.length) { clearItems(); return; }
            S.items = rows;
            X.bind('CmbItemName', rows, 'InvoiceDetailId', 'ItemName');
            if (rows.length === 1) activateFirst('CmbItemName');
            var crops = rows.map(function (r) { return { CropYearId: r.CropYearId, CropYear: r.CropYear }; });
            X.bind('CmbCropYear', crops, 'CropYearId', 'CropYear'); activateFirst('CmbCropYear');
        }).catch(function (e) { box(e.message); });
    }
    function clearItems() { S.items = []; X.setVal('CmbItemName', ''); clearCombo('CmbItemName'); X.setVal('CmbCropYear', ''); clearCombo('CmbCropYear'); }
    function selectedItem() { var v = X.val('CmbItemName'); return v === '' ? null : X.find(S.items, 'InvoiceDetailId', v); }

    /** CmbItemName Leave -> BindPackUomAndPackingType(InvoiceDetailId). */
    function itemLeave() {
        var it = selectedItem();
        if (!it || netI(it.InvoiceDetailId) <= 0) return Promise.resolve();
        var seq = ++itemSeq;
        return X.getJson(API + '/item-detail?invoiceDetailId=' + netI(it.InvoiceDetailId)).then(function (d) {
            if (seq !== itemSeq) return;
            if (d && d.found) {
                setFire('txtTotalPackingWeight', d.TotalPackingWeight);
                setFire('txtPackingWt', d.PackingWeight);
                if (netI(d.JobLotId) > 0) X.setVal('CmbJobLot', d.JobLotId);
                S.uoms = d.uoms || [];
                if (S.uoms.length) { X.bind('CmbPackUom', S.uoms, 'PackUomId', 'PackUom'); activateFirst('CmbPackUom'); }
            } else { S.uoms = []; clearCombo('CmbPackUom'); }
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ totals

    function sum(list, c) { var s = 0; list.forEach(function (r) { s += netD(r[c]); }); return s; }
    /** CalculateTotalInformation + OtherWeightProportion. */
    function calculateTotalInformation() {
        if (S.rows.length) {
            var item = sum(S.rows, 'LoadWeight'), pack = sum(S.rows, 'TotalPackingWeight'), gross = sum(S.rows, 'GrossWeight');
            if (netD(X.val('txtOtherWeight')) > 0) gross += netD(X.val('txtOtherWeight'));
            X.setText('txtItemWeight', X.fmtNet(X.roundEven(item, 2), '#,##0.##'));
            X.setText('txtPackingWeight', X.fmtNet(X.roundEven(pack, 2), '#,##0.##'));
            X.setText('txtTotalGrossWeight', X.fmtNet(X.roundEven(gross, 2), '#,##0.##'));
        } else {
            X.setText('txtItemWeight', '0'); X.setText('txtPackingWeight', '0'); X.setText('txtTotalGrossWeight', '0');
        }
        otherWeightProportion();
    }
    function otherWeightProportion() {
        var net = S.rows.length ? sum(S.rows, 'Weight') : 0;
        var other = netD(X.val('txtOtherWeight'));
        S.rows.forEach(function (r) { r.OtherWeight = other > 0 ? other / net * netD(r.Weight) : 0; });
        render();
    }

    // ------------------------------------------------------------------------------ detail grid

    var COLS = [
        { key: 'InvoiceNo' }, { key: 'SupplierCustomer' }, { key: 'OrderNo' }, { key: 'WareHouse' }, { key: 'JobLot' }, { key: 'CropYear' },
        { key: 'Item' }, { key: 'ItemUOM' }, { key: 'PackingType' },
        { key: 'LoadQty', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'LoadWeight', fmt: '#,##0.##', sum: '#,##0.##' },
        { key: 'PackingWeight', fmt: '#,##0.##' }, { key: 'TotalPackingWeight', fmt: '#,##0.##', sum: '#,##0.##' },
        { key: 'GrossWeight', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'OtherWeight', fmt: '#,##0.##', sum: '#,##0.##' },
        { key: 'ContainerNo' }, { key: 'ContainerRemarks' }, { key: 'InspectionRemarks' }, { key: 'Remarks' }
    ];
    function render() {
        X.drawGrid('edonBody', 'edonFoot', S.rows, COLS, {
            current: S.cur, leadCount: 2,
            lead: function (r, i) {
                return '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '">X</button></td>'
                    + '<td class="win-cell-btn"><button type="button" class="win-edit" data-edit="' + i + '">Edit</button></td>';
            }
        });
    }
    function showEntryButtons(editing) { X.show('btnplus', !editing); X.show('btnUpdateDetail', editing); X.show('btnCancelUpdateDetial', editing); }

    /** FormValidationDetail. */
    function detailValidation() {
        var checks = [
            [netI(X.val('cmbInvoiceNo')) <= 0, 'InvoiceNo field is required', 'cmbInvoiceNo'],
            [netI(X.val('CmbSupplierCustomer')) <= 0, 'Customer field is required', 'CmbSupplierCustomer'],
            [netI(X.val('CmbOrderNo')) <= 0, 'OrderNo field is required', 'CmbOrderNo'],
            [netI(X.val('CmbItemName')) <= 0, 'ItemName field is required', 'CmbItemName'],
            [netI(X.val('CmbPackUom')) <= 0, 'PackUOM field is required', 'CmbPackUom'],
            [netI(X.val('CmbPackingType')) <= 0, 'Packingtype field is required', 'CmbPackingType'],
            [netI(X.val('CmbJobLot')) <= 0, 'JobLot field is required', 'CmbJobLot'],
            [netI(X.val('CmbCropYear')) <= 0, 'CropYear field is required', 'CmbCropYear'],
            [netI(X.val('CmbWareHouse')) <= 0, 'Warehouse field is required', 'CmbWareHouse'],
            [X.val('txtqty').trim() === '' || netD(X.val('txtqty').trim()) === 0, 'Qty field is required', 'txtqty'],
            [X.val('txtweight').trim() === '' || netD(X.val('txtweight').trim()) === 0, 'Weight field is required', 'txtweight'],
            [CFG.ContainerNoCompulsoryOnDeliveryOrderExport && netI(X.val('cmbContainerNoDetail')) === 0, 'ContainerNo field is required', 'cmbContainerNoDetail']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }
    function containerPick() {
        var id = netI(X.val('cmbContainerNoDetail'));
        return id !== 0 ? { id: id, no: X.txt('cmbContainerNoDetail') } : { id: 0, no: '' };
    }

    /** btnplus_Click. */
    function btnplus() {
        if (!detailValidation()) return;
        var it = selectedItem() || {};
        var c = containerPick();
        S.rows.push({
            Id: 0, InvoiceDetailId: netI(X.val('CmbItemName')) > 0 ? netI(it.InvoiceDetailId) : 0,
            InvoiceId: X.val('cmbInvoiceNo'), InvoiceNo: X.txt('cmbInvoiceNo').trim(),
            SupplierCustomerId: X.val('CmbSupplierCustomer'), SupplierCustomer: X.txt('CmbSupplierCustomer').trim(),
            OrderId: X.val('CmbOrderNo'), OrderNo: X.txt('CmbOrderNo').trim(),
            WareHouseId: X.val('CmbWareHouse'), WareHouse: X.txt('CmbWareHouse').trim(),
            JobLotId: X.val('CmbJobLot'), JobLot: X.txt('CmbJobLot').trim(),
            CropYearId: X.val('CmbCropYear'), CropYear: X.txt('CmbCropYear').trim(),
            ItemId: it.ItemId, Item: X.txt('CmbItemName').trim(), ItemUOM: X.txt('CmbPackUom').trim(), ItemUOMId: X.val('CmbPackUom'),
            PackingTypeId: X.val('CmbPackingType'), PackingType: X.txt('CmbPackingType').trim(),
            QTY: netD(X.val('txtqty').trim()), Weight: netD(X.val('txtweight').trim()), LoadQty: netD(X.val('txtqty').trim()), LoadWeight: netD(X.val('txtweight').trim()),
            PackingWeight: netD(X.val('txtPackingWt')), TotalPackingWeight: netD(X.val('txtTotalPackingWeight')), GrossWeight: netD(X.val('txtGrossWeight')),
            OtherWeight: 0, ContainerId: c.id, ContainerNo: c.no,
            ContainerRemarks: X.val('txtContainerRemarks').trim(), InspectionRemarks: X.val('txtInspectionRemarks').trim(), Remarks: X.val('txtremarksdetail').trim()
        });
        render();
        resetDetail();
        calculateTotalInformation();
    }

    /** grd_DoubleClick. */
    function rowToEntry(i) {
        var r = S.rows[i]; if (!r) return Promise.resolve();
        S.updateIdx = i; S.cur = i; render();
        setRaw('cmbInvoiceNo', netI(r.InvoiceId));
        invoiceTextChanged();
        return invoiceLeave().then(function () {
            X.setVal('CmbSupplierCustomer', netI(r.SupplierCustomerId));
            var before = X.val('CmbOrderNo');
            X.setVal('CmbOrderNo', netI(r.OrderId));
            return before !== X.val('CmbOrderNo') ? orderTextChanged() : null;
        }).then(function () {
            X.setVal('CmbItemName', netI(r.InvoiceDetailId));
            return itemLeave();
        }).then(function () {
            X.setVal('CmbPackUom', str(r.ItemUOMId));
            X.setVal('CmbPackingType', str(r.PackingTypeId));
            X.setByText('CmbPackingType', r.PackingType);
            X.setVal('CmbCropYear', str(r.CropYearId));
            setFire('txtqty', clr(r.LoadQty));
            X.setText('txtweight', clr(r.LoadWeight));
            setFire('txtqty', clr(r.QTY));
            X.setText('txtweight', clr(r.Weight));
            setFire('txtPackingWt', clr(r.PackingWeight));
            setFire('txtTotalPackingWeight', clr(r.TotalPackingWeight));
            X.setText('txtGrossWeight', clr(r.GrossWeight));
            X.setVal('CmbWareHouse', str(r.WareHouseId));
            X.setByText('CmbWareHouse', r.WareHouse);
            X.setVal('CmbJobLot', str(r.JobLotId));
            X.setByText('CmbJobLot', r.JobLot);
            if (netI(r.ContainerId) > 0) X.setVal('cmbContainerNoDetail', str(r.ContainerId));
            X.setText('txtInspectionRemarks', str(r.InspectionRemarks));
            X.setText('txtremarksdetail', str(r.Remarks));
            showEntryButtons(true);
            X.focus('CmbWareHouse');
        });
    }

    /** btnUpdateDetail_Click. */
    function btnUpdateDetail() {
        if (!detailValidation()) return;
        var r = S.rows[S.updateIdx]; if (!r) return;
        var it = selectedItem() || {};
        var c = containerPick();
        r.InvoiceId = X.val('cmbInvoiceNo'); r.InvoiceNo = X.txt('cmbInvoiceNo');
        r.SupplierCustomerId = X.val('CmbSupplierCustomer'); r.SupplierCustomer = X.txt('CmbSupplierCustomer');
        r.InvoiceDetailId = netI(it.InvoiceDetailId);
        r.OrderId = X.val('CmbOrderNo'); r.OrderNo = X.txt('CmbOrderNo');
        r.WareHouseId = X.val('CmbWareHouse'); r.WareHouse = X.txt('CmbWareHouse');
        r.JobLotId = X.val('CmbJobLot'); r.JobLot = X.txt('CmbJobLot').trim();
        r.CropYearId = X.val('CmbCropYear'); r.CropYear = X.txt('CmbCropYear').trim();
        r.ItemId = it.ItemId; r.Item = X.txt('CmbItemName');
        r.ItemUOMId = X.val('CmbPackUom'); r.ItemUOM = X.txt('CmbPackUom');
        r.PackingTypeId = X.val('CmbPackingType'); r.PackingType = X.txt('CmbPackingType').trim();
        r.QTY = netD(X.val('txtqty')); r.LoadQty = netD(X.val('txtqty'));
        r.Weight = netD(X.val('txtweight')); r.LoadWeight = netD(X.val('txtweight'));
        r.PackingWeight = netD(X.val('txtPackingWt')); r.TotalPackingWeight = netD(X.val('txtTotalPackingWeight')); r.GrossWeight = netD(X.val('txtGrossWeight'));
        r.ContainerId = c.id; r.ContainerNo = c.no;
        r.ContainerRemarks = X.val('txtContainerRemarks'); r.InspectionRemarks = X.val('txtInspectionRemarks'); r.Remarks = X.val('txtremarksdetail');
        showEntryButtons(false);
        resetDetail();
        reloadInvoices();
        calculateTotalInformation();
    }
    function btnCancelUpdateDetial() { resetDetail(); reloadInvoices(); }

    /** CmbInvoiceNoFill (obj.Id = the form's Id). */
    function reloadInvoices() {
        return X.getJson(API + '/invoices?recId=' + S.recId).then(bindInvoices).catch(function (e) { box(e.message); });
    }

    /** grd_ColumnButtonClick "Delete". */
    function deleteRow(i) {
        var r = S.rows[i]; if (!r) return;
        if (netI(r.Id) > 0) {
            if (!X.ask('Are you sure to Delete?')) return;
            S.removed.push(r);
        }
        S.rows.splice(i, 1);
        S.cur = -1;
        calculateTotalInformation();
    }

    /** ResetDetail (with the TextChanged chains its text clears fire). */
    function resetDetail() {
        X.focus('cmbInvoiceNo');
        X.setVal('CmbItemName', ''); clearCombo('CmbItemName'); S.items = [];
        X.setVal('CmbPackUom', ''); clearCombo('CmbPackUom'); S.uoms = [];
        setFire('txtqty', '');
        X.setText('txtweight', '');
        X.setVal('CmbOrderNo', '');
        clearItems();                         /* CmbOrderNo_TextChanged with an empty order */
        X.setVal('CmbPackingType', '');
        X.setText('txtremarksdetail', '');
        X.setText('txtGrossWeight', '');
        setFire('txtPackingWt', '');
        setFire('txtTotalPackingWeight', '');
        X.setText('txtInspectionRemarks', '');
        X.setText('txtContainerRemarks', '');
        X.setText('txtGrossWeight', '');
        X.setVal('CmbSupplierCustomer', '');
        X.setVal('cmbContainerNoDetail', ''); clearCombo('cmbContainerNoDetail');
        showEntryButtons(false);
    }

    // ------------------------------------------------------------------------------ header

    /** Reset() - the local part; resetData() fetches DocumentNoFill / CmbInvoiceNoFill / defaultConfiquration. */
    function resetLocal() {
        S.recId = 0; S.isApproved = false;
        X.focus('DocDate');
        X.setText('txtItemWeight', ''); X.setText('txtPackingWeight', '');
        setFire('txtOtherWeight', '');        /* CalculateTotalInformation over the rows still in the grid */
        X.setText('txtTotalGrossWeight', ''); X.setText('txtOtherRemarks', '');
        X.setVal('CmbItemName', ''); X.setVal('CmbPackUom', '');
        setFire('txtqty', ''); X.setText('txtweight', '');
        X.setVal('CmbSupplierCustomer', ''); X.setVal('CmbOrderNo', ''); X.setVal('CmbPackingType', '');
        X.setText('txtremarks', '');
        clearRaw('cmbInvoiceNo'); X.setVal('cmbInvoiceNo', '');
        X.setText('txtVehicleNo', '');
        X.setText('txtInvoiceDate', X.today());
        S.contFromReadById = [];
        clearCombo('CmbSupplierCustomer'); clearCombo('CmbOrderNo');
        X.show('btnsave', true); X.show('btnupdate', false);
        showEntryButtons(false);
        S.rows = []; S.cur = -1; S.updateIdx = -1;
        render();
        $id('cmbInvoiceNo').disabled = false; X.refreshCombos();
        S.netWeight = 0;
        ['txtInvoiceWeight', 'txtDoWeight', 'txtBalanceWeight', 'txtrejectedWeight'].forEach(function (id) { X.setText(id, ''); });
    }
    function applyReset(d) {
        d = d || {};
        if (d.docNo) X.setText('txtdocno', d.docNo);
        bindInvoices(d.invoices);
        if (d.config) CFG = d.config;
        defaultConfiguration();
    }
    function reset() {
        resetLocal();
        return X.getJson(API + '/reset').then(applyReset).catch(function (e) { box(e.message); });
    }
    function btnnew() { return reset(); }

    /** btnRefresh_Click. */
    function btnRefresh(btn) {
        return X.busy(btn || 'btnRefresh', function () {
            return X.getJson(API + '/refresh?recId=' + S.recId).then(function (d) {
                S.branchCount = (d.branches || []).length;
                if (d.packingTypes && d.packingTypes.length) { X.bind('CmbPackingType', d.packingTypes, 'Id', 'PackTypeDesc'); activateFirst('CmbPackingType'); }
                if (d.warehouses && d.warehouses.length) X.bind('CmbWareHouse', d.warehouses, 'Id', 'WareHouseName', { keep: true });
                if (d.jobLots && d.jobLots.length) X.bind('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription', { keep: true });
                bindInvoices(d.invoices);
            });
        });
    }

    /** FormValidation. */
    function formValidation() {
        var branchVisible = X.visible('lblBranchName');
        var checks = [
            [branchVisible && X.val('CmbBranch') === '', 'Brnach field is required', 'CmbBranch'],
            [X.val('txtdocno').trim() === '' || X.val('txtdocno').trim() === '0', 'DocNo Field is Required', 'txtdocno'],
            [netD(X.val('txtItemWeight')) === 0, 'Item Weight Field is Required', 'txtItemWeight'],
            [netD(X.val('txtPackingWeight')) === 0, 'Packing Weight Field is Required', 'txtPackingWeight'],
            [netD(X.val('txtTotalGrossWeight')) === 0, 'Total Gross Weight Field is Required', 'txtTotalGrossWeight'],
            [netD(X.val('txtOtherWeight')) > 0 && X.val('txtOtherRemarks') === '', 'Other Remarks Field is Required', 'txtOtherRemarks']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][0]) { box(checks[i][1]); X.focus(checks[i][2]); return false; }
        return true;
    }

    /** Insert(). */
    function insert(btn) {
        if (!formValidation()) return;
        if (!S.rows.length) { box('Grid Record Not Found'); return; }
        if (S.recId > 0) { if (!X.ask('Are you sure to Update?')) return; }
        else if (!X.ask('Are you sure to Save?')) return;
        otherWeightProportion();
        var body = {
            recId: S.recId, docDate: X.val('DocDate'), docNo: X.val('txtdocno'), remarks: X.val('txtremarks'),
            invoiceId: netI(X.val('cmbInvoiceNo')), branchId: netI(X.val('CmbBranch')), vehicleNo: X.val('txtVehicleNo'),
            transporterId: netI(X.val('cmbtransporter')), itemWeight: X.val('txtItemWeight'), packingWeight: X.val('txtPackingWeight'),
            otherWeight: X.val('txtOtherWeight'), totalGrossWeight: X.val('txtTotalGrossWeight'), otherRemarks: X.val('txtOtherRemarks'),
            rows: S.rows, removed: S.removed
        };
        return X.busy(btn, function () {
            return X.postJson(API + '/save', body).then(function (r) {
                if (r && r.success) { box(r.message); return reset(); }
            });
        });
    }
    function btnsave(btn) { S.recId = 0; return insert(btn || $id('btnsave')); }
    function btnupdate(btn) { return insert(btn || $id('btnupdate')); }

    /** Print_Click: CommonServices.InvDeliveryOrderSlip(Id, 84) -> 262-DeliveryOrderSlip.rpt. */
    function printSlip(id, btn) {
        if (netI(id) === 0) { box('PrintId not found...'); return; }
        if (!global.CrystalPrint) { box('Print is not available.'); return; }
        return global.CrystalPrint.open('262-deliveryorderslip', { id: id, documentTypeId: DOC_TYPE }, btn);
    }
    function print(btn) { return printSlip(S.recId, btn); }

    // ------------------------------------------------------------------------------ read

    /** ReadById. */
    function readById(id) {
        resetLocal();
        return Promise.all([X.getJson(API + '/reset'), X.getJson(API + '/by-id?id=' + encodeURIComponent(id))]).then(function (res) {
            applyReset(res[0]);
            var d = res[1], h = d.header;
            S.recId = netI(id);
            tab('tabPage1');
            X.setText('DocDate', h.DocDate);
            X.setText('txtdocno', h.DocNo);
            X.setText('txtremarks', h.LoadingInstructions);
            setRaw('cmbInvoiceNo', h.EximInvoiceId);
            invoiceTextChanged();
            X.setText('txtVehicleNo', h.VehicleNo);
            X.setVal('CmbBranch', h.BranchesId);
            X.setText('txtItemWeight', X.fmtNet(h.NetWeight, '#,##0.##'));
            X.setText('txtPackingWeight', X.fmtNet(h.PackingWeight, '#,##0.##'));
            setFire('txtOtherWeight', X.fmtNet(h.OtherWeight, '#,##0.##'));
            X.setText('txtTotalGrossWeight', X.fmtNet(h.GrossWeight, '#,##0.##'));
            X.setText('txtOtherRemarks', h.OtherWeightRemarks);
            if (netI(h.TransporterId) > 0) X.setVal('cmbtransporter', h.TransporterId);
            S.isApproved = !!h.IsApproved;
            S.rows = d.rows || [];
            S.contFromReadById = S.rows.map(function (r) { return { ContainerId: r.ContainerId, ContainerNo: r.ContainerNo }; });
            S.cur = -1;
            calculateTotalInformation();
            X.show('btnsave', false); X.show('btnupdate', true);
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = ['DoDate', 'DoNo', 'InvoiceDate', 'InvoiceNo', 'InvoiceNetWeight', 'InvoiceGrossWeight', 'CustomerName', 'OrderNo', 'VehicleNo', 'Transporter',
        'GpDate', 'GpSrNo', 'GpStatus', 'ForwardingNo', 'FactoryWeight', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser', 'ApprovedDate', 'ApprovedUser', 'BranchName', 'NoOfAttachments'];
    var HNUM = { InvoiceNetWeight: 1, InvoiceGrossWeight: 1, FactoryWeight: 1 };
    function histCols() { return HCOLS.filter(function (c) { return c !== 'BranchName' || S.branchCount > 1; }); }
    function renderHistory() {
        var cols = histCols();
        $id('edonHistHead').innerHTML = '<tr><th class="ctr">Edit</th><th class="ctr">Print</th>' + cols.map(function (c) { return '<th' + (HNUM[c] || c === 'DoNo' ? ' class="num"' : '') + '>' + esc(c) + '</th>'; }).join('') + '</tr>';
        var defs = cols.map(function (c) {
            if (c === 'DoNo') return { key: c, num: true, render: function (r, i) { return '<a href="#" class="win-code" data-i="' + i + '">' + esc(r.DoNo) + '</a>'; } };
            if (c === 'DoDate') return { key: c, render: function (r) { return esc(X.shortDate(r.DoDate)); } };
            if (c === 'NoOfAttachments') return { key: c, render: function (r) { return '<span title="Attachments are not part of this web screen">' + esc(r.NoOfAttachments) + '</span>'; } };
            if (HNUM[c]) return { key: c, fmt: '#,##0.##', sum: '#,##0.##' };
            return { key: c };
        });
        X.drawGrid('edonHistBody', 'edonHistFoot', HIST, defs, {
            current: HCUR, leadCount: 2,
            lead: function (r, i) {
                return '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-hedit="' + i + '">Edit</button></td>'
                    + '<td class="win-cell-btn"><button type="button" class="win-btn-small" data-hprint="' + i + '">Print</button></td>';
            }
        });
        X.show('edonHistEmpty', !HIST.length);
    }
    /** gridhistoryfill. */
    function btnShowHistory(btn) {
        var body = {
            fromChecked: X.checked('FromDateHistoryChk'), fromDate: X.val('FromDateHistory'),
            toChecked: X.checked('ToDateHistoryChk'), toDate: X.val('ToDateHistory'),
            dateMode: (document.querySelector('input[name="edonDateBy"]:checked') || {}).value || 'doc',
            fromDocNo: netI(X.val('FromDocNoHistory')), toDocNo: netI(X.val('ToDocNoHistory')),
            customerId: netI(X.val('CmbCustomerHistory')), invoiceId: netI(X.val('CmbInvoiceNoHistory'))
        };
        return X.busy(btn || 'btnShowHistory', function () {
            return X.postJson(API + '/history', body).then(function (rows) {
                HIST = rows || []; HCUR = -1;
                renderHistory();
                $id('edonHistDetailBody').innerHTML = ''; $id('edonHistDetailFoot').innerHTML = '';
            });
        });
    }
    var DCOLS = [
        { key: 'InvocieNo' }, { key: 'CustomerName' }, { key: 'OrderNo' }, { key: 'WareHouse' }, { key: 'JobLot' }, { key: 'Item' }, { key: 'ItemUOM' }, { key: 'PackingType' },
        { key: 'QTY', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'Weight', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'PackingWeight', fmt: '#,##0.##' },
        { key: 'TotalPackingWeight', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'GrossWeight', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'OtherWeight', fmt: '#,##0.##', sum: '#,##0.##' },
        { key: 'LoadQty', fmt: '#,##0.##', sum: '#,##0.##' }, { key: 'LoadWeight', fmt: '#,##0.##', sum: '#,##0.##' },
        { key: 'ContainerNo' }, { key: 'ContainerRemarks' }, { key: 'InspectionRemarks' }, { key: 'Remarks' }
    ];
    /** GetDetailByHeaderId (grdhistory_SelectionChanged). */
    function historySelect(i) {
        HCUR = i;
        var r = HIST[i]; if (!r) return;
        X.getJson(API + '/history-detail?id=' + r.Id).then(function (rows) {
            if (HCUR !== i) return;
            if (rows && rows.length) X.drawGrid('edonHistDetailBody', 'edonHistDetailFoot', rows, DCOLS);
        }).catch(function (e) { box(e.message); });
    }
    /** btnResetHistory_Click. */
    function btnResetHistory() {
        X.setText('FromDateHistory', X.addDays(X.today(), -3));
        X.setText('ToDateHistory', X.today());
        X.setText('FromDocNoHistory', ''); X.setText('ToDocNoHistory', '');
        X.setVal('CmbCustomerHistory', '');
        HIST = []; HCUR = -1;
        $id('edonHistHead').innerHTML = ''; $id('edonHistBody').innerHTML = ''; $id('edonHistFoot').innerHTML = '';
        $id('edonHistDetailBody').innerHTML = ''; $id('edonHistDetailFoot').innerHTML = '';
        X.show('edonHistEmpty', false);
    }
    function btnRefreshHistory(btn) {
        return X.busy(btn || 'btnRefreshHistory', function () { return X.getJson(API + '/history-combos').then(bindHistoryCombos); });
    }

    // ------------------------------------------------------------------------------ shortcut keys

    var KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Invoice no in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortCutKeys() {
        $id('edonShortcutBody').innerHTML = KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        X.show('edonShortcutModal', true);
    }
    function closeShortCutKeys() { X.show('edonShortcutModal', false); }

    // ------------------------------------------------------------------------------ wiring

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.win-tabs[data-tabs="edon"] .win-tab').forEach(function (b) { b.addEventListener('click', function () { tab(b.getAttribute('data-tab')); }); });
        X.wireFullscreen();
        X.on('cmbInvoiceNo', 'change', function () { if (!$id('cmbInvoiceNo').querySelector('option[data-raw]:checked')) clearRaw('cmbInvoiceNo'); invoiceLeave(); });
        X.on('CmbOrderNo', 'change', orderTextChanged);
        X.on('CmbItemName', 'change', itemLeave);
        X.on('CmbPackUom', 'change', calculateWeight);
        X.on('txtqty', 'input', qtyChanged);
        X.on('txtTotalPackingWeight', 'input', totalPackingChanged);
        X.on('txtPackingWt', 'input', calcPackingWeight);
        X.on('txtOtherWeight', 'input', calculateTotalInformation);
        var gb = $id('edonBody');
        gb.addEventListener('click', function (e) {
            var d = e.target.closest('button[data-del]'); if (d) { deleteRow(+d.getAttribute('data-del')); return; }
            var ed = e.target.closest('button[data-edit]'); if (ed) { rowToEntry(+ed.getAttribute('data-edit')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            S.cur = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) rowToEntry(+tr.getAttribute('data-i')); });
        var hb = $id('edonHistBody');
        hb.addEventListener('click', function (e) {
            var be = e.target.closest('button[data-hedit]'); if (be) { readById(HIST[+be.getAttribute('data-hedit')].Id); return; }
            var bp = e.target.closest('button[data-hprint]'); if (bp) { printSlip(HIST[+bp.getAttribute('data-hprint')].Id, bp); return; }
            var a = e.target.closest('a.win-code'); if (a) { e.preventDefault(); readById(HIST[+a.getAttribute('data-i')].Id); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            hb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            historySelect(+tr.getAttribute('data-i'));
        });
        hb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) readById(HIST[+tr.getAttribute('data-i')].Id); });
        /* PurchsaeOrder_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (e.ctrlKey && e.key === 'Enter') {
                e.preventDefault();
                if (!onHistory()) { if (S.cur >= 0) rowToEntry(S.cur); }
                else if (HCUR >= 0 && HIST[HCUR]) readById(HIST[HCUR].Id);
                return;
            }
            if (X.enterAsTab(e)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's' && X.visible('btnsave') && !$id('btnsave').disabled && !onHistory()) { e.preventDefault(); btnsave($id('btnsave')); return; }
            if (e.ctrlKey && k === 'u' && X.visible('btnupdate') && !$id('btnupdate').disabled) { e.preventDefault(); btnupdate($id('btnupdate')); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && k === 'n' && !onHistory()) { e.preventDefault(); btnnew(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') {
                if (X.visible('edonShortcutModal')) { closeShortCutKeys(); return; }
                e.preventDefault(); X.closeForm(); return;
            }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); return; }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); X.focus('DocDate'); return; }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var g = onHistory() ? $id('grdDetail') : $id('grd'); if (g) g.scrollIntoView({ block: 'nearest' }); return; }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); if (onHistory()) $id('grdhistory').scrollIntoView({ block: 'nearest' }); else X.focus('cmbInvoiceNo'); return; }
            /* Ctrl+P: CommonServices.BookOrderReports273A(Id) - 273-InvRptSaleOrderSlip(A).rpt with the DO id (desktop quirk) */
            if (e.ctrlKey && k === 'p') {
                e.preventDefault();
                if (S.recId === 0) { box('PrintId not found...'); return; }
                if (global.CrystalPrint) global.CrystalPrint.open('273a-invrptsaleorderslip', { id: S.recId, documentTypeId: 0 }); else box('Print is not available.');
                return;
            }
            if (e.ctrlKey && e.altKey) { shortCutKeys(); }
        });
        load();
    });

    global.ExportEdon = {
        btnnew: btnnew, btnRefresh: btnRefresh, btnsave: btnsave, btnupdate: btnupdate, print: print, toggleHistory: toggleHistory,
        btnplus: btnplus, btnUpdateDetail: btnUpdateDetail, btnCancelUpdateDetial: btnCancelUpdateDetial,
        btnShowHistory: btnShowHistory, btnResetHistory: btnResetHistory, btnRefreshHistory: btnRefreshHistory,
        shortCutKeys: shortCutKeys, closeShortCutKeys: closeShortCutKeys
    };
}(window));
