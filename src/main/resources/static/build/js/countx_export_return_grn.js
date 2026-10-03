/* ============================================================================================
 * countx_export_return_grn.js - ExportReturn_Grn.cs (Architecture.WinApp.Export), screen 190 "Export Return GRN",
 * DocumentTypeId 241, with the reverse-flow popup frmLoadExportReurnInvoiceForGrn. Every button, TextChanged / Leave,
 * grid edit / button / key, history action and shortcut of the desktop form has its counterpart here with the desktop's
 * messages and order. Data: /api/export/return-grn (ExportReturnController -> ExportReturnGrnService). Print 215:
 * POST /reports/print/215-export-return-grn-slip { id } (215-ExportReturnGrnSlip, USp_ExportReturnGrn_SlipAndRegister).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var X = global.ExM;
    var $id = X.$id, box = X.box, ask = X.ask, netD = X.netD, netI = X.netI, str = X.str;
    var API = '/api/export/return-grn';

    var PERM = { Save: true, Update: true, Print: true, Delete: true };
    var CFG = { enableExportReturnReverseFlow: false, defaultDaysToLessFromHistoryFromDate: 0, decAmount: 0 };
    var L = { customers: [], salesmen: [], ports: [], warehouses: [], cropYears: [], jobLots: [], packTypes: [], gatePasses: [], forwardings: [],
        items: [], uoms: [], otherItemList: [], containers: [] };
    var S = { recId: 0, detail: [], removed: [], others: [], updateIndex: -1, forwardingIdCombo: 0, forwardingData: [] };
    var H = { rows: [], cur: -1, detail: [] };
    var LD = { rows: [], main: [], detail: [], checked: {}, mainCur: -1 };

    function reverse() { return !!CFG.enableExportReturnReverseFlow; }
    function fA(v) { return X.fixed(v, CFG.decAmount); }
    function roundEven(v, d) { var f = Math.pow(10, d || 0), x = v * f, r = Math.round(x); if (Math.abs(x % 1) === 0.5 && r % 2 !== 0) r -= 1; return r / f; }

    // ------------------------------------------------------------------------------ columns

    var DETAIL_COLS = [
        { k: 'Id', hide: true }, { k: 'ContractId', hide: true }, { k: 'ContractDetailId', hide: true }, { k: 'ContractNo', t: 'ContractNo' },
        { k: 'ContractDate', t: 'ContractDate', date: 'short' }, { k: 'InvoiceId', hide: true }, { k: 'InvoiceDetailId', hide: true }, { k: 'InvoiceNo', t: 'InvoiceNo' },
        { k: 'ExImForwardingDetailId', hide: true }, { k: 'ReturnInvoiceId', hide: true }, { k: 'ReturnInvoiceDetailId', hide: true }, { k: 'ReturnInvoiceNo', t: 'ReturnInvoiceNo' },
        { k: 'WarehouseId', hide: true }, { k: 'Warehouse', t: 'Warehouse' }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode' },
        { k: 'ItemName', t: 'ItemName', w: 160 }, { k: 'CropYearId', hide: true }, { k: 'CropYear', t: 'CropYear' }, { k: 'JobLotId', hide: true },
        { k: 'JobLot', t: 'JobLot' }, { k: 'PackingTypeId', hide: true }, { k: 'PackingType', t: 'PackingType' }, { k: 'PackUomId', hide: true },
        { k: 'PackUomCode', t: 'Pack Uom' }, { k: 'PackUomEquivalent', hide: true }, { k: 'NoOfBags', t: 'NoOfBags', num: 3 },
        { k: 'GrossWeight', t: 'GrossWeight', num: 3 }, { k: 'EbUnit', t: 'EbUnit', num: 3 }, { k: 'EbTotal', t: 'EbTotal', num: 3 },
        { k: 'AddLess', t: 'AddLess', num: 3 }, { k: 'NetWeight', t: 'NetWeight', num: 3 }, { k: 'StockWeight', t: 'StockWeight', num: 3 },
        { k: 'ItemDescription', t: 'ItemDescription', edit: 'text', w: 160 }, { k: 'Container#', t: 'Container#' }, { k: 'Seal#', t: 'Seal#' }, { k: 'CostRate', hide: true }
    ];
    /* GridDetailSetting(grid). */
    function detailCols(forGrid) {
        return DETAIL_COLS.map(function (c) {
            var o = {}; for (var k in c) o[k] = c[k];
            if (o.k === 'ReturnInvoiceNo') o.hide = !reverse();
            if (!forGrid) { delete o.edit; return o; }
            if (reverse() && (o.k === 'NoOfBags' || o.k === 'GrossWeight' || o.k === 'EbUnit' || o.k === 'AddLess')) o.edit = 'num';
            if (['Warehouse', 'JobLot', 'CropYear', 'PackingType'].indexOf(o.k) >= 0) o.code = true;            /* F1 pickers */
            if (reverse() && (o.k === 'Container#' || o.k === 'Seal#')) o.code = true;
            return o;
        });
    }
    var HIST_COLS = [
        { k: 'Id', hide: true }, { k: 'DocNo', t: 'DocNo', code: true }, { k: 'DocDate', t: 'DocDate', date: 'short' }, { k: 'DocumentTypeId', hide: true },
        { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 150 }, { k: 'ExImForwardingId', hide: true }, { k: 'ForwardingNo', t: 'ForwardingNo' },
        { k: 'GpId', hide: true }, { k: 'GpNo', t: 'GpNo' }, { k: 'GPDate', t: 'GPDate', date: 'short' }, { k: 'Transporter', t: 'Transporter' },
        { k: 'FreightAmount', t: 'FreightAmount', num: 2 }, { k: 'VehicleNo', t: 'VehicleNo' }, { k: 'BiltyNo', t: 'BiltyNo' }, { k: 'DriverName', t: 'DriverName' },
        { k: 'DriverCellNo', t: 'DriverCellNo' }, { k: 'DriverCnicNo', t: 'DriverCnicNo' }, { k: 'LoadingPort', t: 'LoadingPort' }, { k: 'DestinationPort', t: 'DestinationPort' },
        { k: 'NoOfContainer', t: 'NoOfContainer', num: 0 }, { k: 'RemarksHeader', t: 'RemarksHeader' }, { k: 'EntryDate', t: 'EntryDate', date: 'dt' },
        { k: 'EntryUser', t: 'EntryUser' }, { k: 'ModifyDate', t: 'ModifyDate', date: 'dt' }, { k: 'ModifyUser', t: 'ModifyUser' }
    ];

    // ------------------------------------------------------------------------------ tabs / load

    function onHistory() { return $id('grnHistory').classList.contains('is-active'); }
    function tabChanged(p) {
        var hist = p === 'grnHistory';
        var fb = $id('btnGrnFooterHistory');
        fb.querySelector('span').textContent = hist ? 'Form' : 'History';
        fb.querySelector('i').className = hist ? 'fa fa-file-text-o' : 'fa fa-history';
        /* tabControl1_SelectedIndexChanged: the History tab resets From Date to today - 3 every time it is selected. */
        if (hist) { X.focus('fromdateHistory'); X.setText('fromdateHistory', X.daysAgo(3)); } else X.focus('DocDate');
    }
    function toggleHistory() { var p = onHistory() ? 'grnForm' : 'grnHistory'; X.selectTab('grn', p); tabChanged(p); }

    function bindGatePasses(rows) {
        L.gatePasses = rows || [];
        if (!L.gatePasses.length) {
            X.setText('txtGPDate', X.today()); X.setText('txtBilityNo', ''); X.setText('txtVehicleNo', ''); X.setText('txtFreight', '');
            X.setText('txtconainer1', ''); X.setText('txtContainer2', ''); X.setText('txtFactoryWeight', '');
        }
        X.bind('cmbGpNo', L.gatePasses, 'Id', 'GpSrNo', ['OrderType']);
    }
    function bindForwardings(rows) {
        L.forwardings = rows || [];
        X.bind('cmbForwardingNo', L.forwardings, 'ForwardingId', 'GpNo', ['GatePassOutwardId', 'GpDate', 'ForwardingNo', 'ForwardingDate']);
    }
    /* SupplierDtFillFromGlobal + BindSupplierName + ShippingAgentBind + PortsBind + the global detail combos. */
    function bindGlobals(d) {
        if (d.customers) { L.customers = d.customers; X.bind('cmbCustomer', L.customers, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'MobileNo']); }
        if (d.salesmen) {
            L.salesmen = d.salesmen;
            X.bind('cmbShippingAgent', L.salesmen, 'Id', 'CompanyName'); X.bind('cmbShippingLIne', L.salesmen, 'Id', 'CompanyName');
            X.bind('cmbtransporter', L.salesmen, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'MobileNo']);
        }
        if (d.ports) { L.ports = d.ports; X.bind('cmbLoadingPort', L.ports, 'Id', 'PortName'); X.bind('cmbDestinationPort', L.ports, 'Id', 'PortName'); }
        if (d.warehouses) { L.warehouses = d.warehouses; X.bind('CmbWareHouse', L.warehouses, 'Id', 'WareHouseName'); }
        if (d.cropYears) { L.cropYears = d.cropYears; X.bind('CmbCropYear', L.cropYears, 'Id', 'CropYear'); }
        if (d.jobLots) { L.jobLots = d.jobLots; X.bind('CmbJobLot', L.jobLots, 'Id', 'JobLotDescription'); }
        if (d.packTypes) { L.packTypes = d.packTypes; X.bind('cmbPackType', L.packTypes, 'Id', 'PackTypeDesc'); }
        itemNameBind();
    }
    /* ImplementConfigurations. */
    function applyConfig() {
        X.show('btnInvoiceLoader', reverse());
        X.show('panel5', !reverse());
        X.show('forwardingWrap', !reverse());
        $id('grnDetailNote').textContent = reverse() ? 'Bags / Gross / EbUnit / AddLess edit in the grid - click Warehouse, JobLot, CropYear, PackingType, Container#, Seal# = F1 picker'
            : 'Double-click / Edit loads a row into the panel - click Warehouse, JobLot, CropYear, PackingType = F1 picker';
    }
    function load() {
        return X.getJson(API + '/setup').then(function (d) {
            d = d || {};
            PERM = d.permissions || PERM; CFG = d.config || CFG;
            $id('btnSave').disabled = !PERM.Save; $id('btnUpdate').disabled = !PERM.Update; $id('BtnPrint').disabled = !PERM.Print;
            $id('ChkPrintPreview').disabled = !PERM.Print; $id('ChkPrintPreview').checked = !!PERM.Print; $id('btnDelete').disabled = !PERM.Delete;
            if (S.recId === 0) X.setText('txtdocno', d.docNo);
            applyConfig();
            bindGatePasses(d.gatePasses);
            if (!reverse()) bindForwardings(d.forwardings);
            bindGlobals(d);
            L.otherItemList = d.otherItemList || [];
            drawDetail();
            historyCombos(d.history || {});
            X.setText('fromdateHistory', CFG.defaultDaysToLessFromHistoryFromDate > 0 ? X.daysAgo(CFG.defaultDaysToLessFromHistoryFromDate) : X.daysAgo(3));
            X.setText('ToDateHistory', X.today());
            X.setText('DocDate', X.today());
            X.setText('txtGPDate', X.today());
            showMode(false);
            detailButtons(false);
            $id('grnFooterInfo').textContent = 'ExportReturn_Grn  -  Document Type 241';
            X.focus('DocDate');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function showMode(update) { X.show('btnSave', !update); X.show('btnUpdate', update); X.show('btnDelete', update); }
    function detailButtons(editing) { X.show('btnDetailUpdate', editing); X.show('btnDetailCancel', editing); X.show('btnAddinGrid', !editing); }

    // ------------------------------------------------------------------------------ detail grid

    function drawDetail() {
        var lead = [{ t: 'X', html: function () { return X.btnHtml('Delete', 'X', 'win-x'); } }];
        if (!reverse()) {
            lead.push({ t: '+', html: function () { return X.btnHtml('Add', '+'); } });
            lead.push({ t: 'Edit', html: function () { return X.btnHtml('Edit', 'Edit'); } });
        }
        X.grid('grdDetail', {
            cols: detailCols(true), rows: S.detail, lead: lead,
            onButton: function (i, act) { if (act === 'Delete') deleteDetailRow(i); if (act === 'Edit') editRow(i); if (act === 'Add') addDetailRow(i); },
            onDbl: function (i) { editRow(i); },
            onEdit: function (i, k, v) { cellUpdated(i, k, v); },
            onCode: function (i, k) { gridPicker(i, k); },
            onKey: function (i, e) {
                if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteDetailRow(i); }
                else if (e.ctrlKey && (e.key === 'd' || e.key === 'D')) { e.preventDefault(); addDetailRow(i); }
                else if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); editRow(i); }
            }
        });
    }
    /* grdDetail_CellUpdated. */
    function cellUpdated(i, k, v) {
        var r = S.detail[i]; if (!r) return;
        if (k === 'ItemDescription') { r.ItemDescription = v; return; }
        r[k] = netD(v);
        var bags = netD(r.NoOfBags), gross = netD(r.GrossWeight), eb = netD(r.EbUnit), ebTotal = netD(r.EbTotal), al = netD(r.AddLess);
        if (k === 'NoOfBags') {
            gross = bags * netD(r.PackUomEquivalent); r.GrossWeight = roundEven(gross);
            ebTotal = bags * eb; r.EbTotal = ebTotal;
            r.NetWeight = roundEven(gross);
            r.StockWeight = roundEven(gross - ebTotal + al);
        }
        if (k === 'GrossWeight') r.StockWeight = roundEven(gross - ebTotal + al);
        if (k === 'EbUnit') { ebTotal = bags * eb; r.EbTotal = ebTotal; r.StockWeight = roundEven(gross - ebTotal + al); }
        if (k === 'AddLess') r.StockWeight = roundEven(gross - ebTotal + al);
        drawDetail();
    }
    /* AddDetailRow ("+" / Ctrl+D): a copy of the row with Id 0 (not in the reverse flow). */
    function addDetailRow(i) {
        if (reverse()) return;
        var r = S.detail[i]; if (!r) return;
        var c = {}; for (var k in r) c[k] = r[k]; c.Id = 0;
        S.detail.push(c); drawDetail();
    }
    /* DeleteDetailRow. */
    function deleteDetailRow(i, silent) {
        var r = S.detail[i]; if (!r) return;
        if (S.updateIndex !== -1) { box('Please Reset the Detail First...'); return; }
        if (netI(r.Id) > 0) {
            if (!silent && !ask('Are you sure to Delete?')) return false;
            S.removed.push(r);
        }
        S.detail.splice(i, 1);
        drawDetail();
        return true;
    }
    /* grdDetail_KeyDown F1 - GrdPopUp over the warehouse / job lot / crop / packing lists, container / seal (reverse flow). */
    function gridPicker(i, k) {
        var r = S.detail[i]; if (!r) return;
        var spec = { 'Warehouse': [L.warehouses, 'WareHouseName', 'WarehouseId'], 'JobLot': [L.jobLots, 'JobLotDescription', 'JobLotId'],
            'CropYear': [L.cropYears, 'CropYear', 'CropYearId'], 'PackingType': [L.packTypes, 'PackTypeDesc', 'PackingTypeId'] }[k];
        if (spec) {
            X.pick(spec[1], spec[0], 'Id', spec[1]).then(function (p) { r[spec[2]] = p ? netI(p.Id) : 0; r[k] = p ? str(X.col(p, spec[1])) : ''; drawDetail(); });
            return;
        }
        if (reverse() && (k === 'Container#' || k === 'Seal#')) {
            var field = k === 'Container#' ? 'ContainerNo' : 'SealNo', seen = {}, list = [];
            S.forwardingData.forEach(function (f) { var v = str(f[field]); if (!seen[v]) { seen[v] = 1; list.push(f); } });
            X.pick(field, list, 'ContainerId', field).then(function (p) { r[k] = p ? str(p[field]) : ''; drawDetail(); });
        }
    }

    // ------------------------------------------------------------------------------ detail panel (normal flow)

    function itemNameBind() {
        var byName = $id('rdbtnItemName').checked;
        X.bind('cmbItem', L.items, 'Id', byName ? 'ItemName' : 'ItemCode');
    }
    function uomEquivalent() {
        var id = X.vint('cmbPackUOM'); if (id <= 0) return null;
        var u = L.uoms.filter(function (x) { return netI(x.Id) === id; })[0];
        return u ? netD(u.Equivalent) : null;
    }
    /* GrossWtCalculation. */
    function grossWtCalculation() {
        var eq = uomEquivalent(); if (eq === null) return;
        var bags = netD(X.val('txtNoOfBags'));
        fire('txtGrossWeight', bags > 0 && eq > 0 ? String(roundEven(bags * eq)) : '0');
    }
    /* NetWtCalculation. */
    function netWtCalculation() {
        var gross = netD(X.val('txtGrossWeight')), al = netD(X.val('txtAddLess')), eb = netD(X.val('txtEbUnit')), bags = netD(X.val('txtNoOfBags'));
        var eq = uomEquivalent() || 0, ebTotal = 0;
        if (eb > 0 && bags > 0) { ebTotal = bags * eb; X.setText('txtEbTotal', X.raw(ebTotal)); } else X.setText('txtEbTotal', '0');
        if (gross > 0) X.setText('txtStockWeight', X.raw(roundEven(gross - ebTotal + al, 2))); else X.setText('txtStockWeight', X.raw(gross));
        X.setText('txtNetWeight', X.raw(roundEven(bags * eq, 2)));
    }
    var HANDLERS = {
        txtNoOfBags: function () { grossWtCalculation(); netWtCalculation(); },
        txtEbUnit: netWtCalculation, txtAddLess: netWtCalculation, txtGrossWeight: netWtCalculation
    };
    /* Text assignment that raises TextChanged when the text really changes. */
    function fire(id, v) {
        var e = $id(id); if (!e) return;
        var changed = e.value !== str(v);
        e.value = str(v);
        if (changed && HANDLERS[id]) HANDLERS[id]();
    }
    /* cmbItem_Leave -> ItemUomFromGlobalBind (keeps the pack UOM by its code). */
    function itemUomBind(itemId) {
        var keepCode = X.text('cmbPackUOM');
        return X.getJson(API + '/item-uoms?itemId=' + itemId).then(function (rows) {
            L.uoms = rows || [];
            X.bind('cmbPackUOM', L.uoms, 'Id', 'UOMCode', ['Equivalent'], 0);
            var m = L.uoms.filter(function (u) { return str(u.UOMCode) === keepCode; })[0];
            if (m) X.setVal('cmbPackUOM', m.Id);
        });
    }
    /* grdDetail_DoubleClick / Edit (normal flow only). */
    function editRow(i) {
        if (reverse()) return;
        var r = S.detail[i]; if (!r) return;
        S.updateIndex = i;
        X.setVal('CmbWareHouse', r.WarehouseId);
        L.items = [{ Id: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode }];
        itemNameBind();
        X.setVal('cmbItem', r.ItemId);
        X.setVal('CmbJobLot', r.JobLotId);
        X.setVal('CmbCropYear', r.CropYearId);
        X.setVal('cmbPackType', r.PackingTypeId);
        itemUomBind(r.ItemId).then(function () {
            X.setVal('cmbPackUOM', r.PackUomId);
            fire('txtNoOfBags', X.raw(r.NoOfBags));
            X.setText('txtNetWeight', X.raw(r.NetWeight));
            fire('txtEbUnit', X.raw(r.EbUnit));
            X.setText('txtEbTotal', X.raw(r.EbTotal));
            fire('txtAddLess', X.raw(r.AddLess));
            fire('txtGrossWeight', X.raw(r.GrossWeight));
            X.setText('txtStockWeight', X.raw(r.StockWeight));
            X.setText('txtItemDesc', r.ItemDescription);
            X.setVal('cmbcontainerNo', str(r['Container#']));
            X.setText('txtSeal', r['Seal#']);
            detailButtons(true);
        }).catch(function (e) { box(e.message); });
    }
    /* formdetailvalidation (FormHelper.ValidateControls). */
    function detailValidation() {
        var combos = [['CmbWareHouse', 'Warehouse'], ['cmbItem', 'Item'], ['CmbCropYear', 'CropYear'], ['CmbJobLot', 'Job Lot'], ['cmbPackType', 'Pack Type'], ['cmbPackUOM', 'Pack UOM']];
        for (var j = 0; j < combos.length; j++) if (X.vint(combos[j][0]) === 0) { box(combos[j][1] + ' field is required'); X.focus(combos[j][0]); return false; }
        var nums = [['txtNoOfBags', 'No Of Bags'], ['txtNetWeight', 'Net Weight'], ['txtGrossWeight', 'Gross Weight']];
        for (var n = 0; n < nums.length; n++) if (netD(X.val(nums[n][0])) === 0) { box(nums[n][1] + ' must be a non-zero number'); X.focus(nums[n][0]); return false; }
        return true;
    }
    function btnDetailUpdate() {
        if (!detailValidation()) return;
        var r = S.detail[S.updateIndex]; if (!r) return;
        var item = L.items.filter(function (x) { return netI(x.Id) === X.vint('cmbItem'); })[0] || {};
        var uom = L.uoms.filter(function (x) { return netI(x.Id) === X.vint('cmbPackUOM'); })[0] || {};
        r.WarehouseId = X.vint('CmbWareHouse'); r.Warehouse = X.text('CmbWareHouse');
        r.ItemId = X.vint('cmbItem'); r.ItemName = str(item.ItemName); r.ItemCode = str(item.ItemCode);
        r.CropYearId = X.vint('CmbCropYear'); r.CropYear = X.text('CmbCropYear');
        r.JobLotId = X.vint('CmbJobLot'); r.JobLot = X.text('CmbJobLot');
        r.PackingTypeId = X.vint('cmbPackType'); r.PackingType = X.text('cmbPackType');
        r.PackUomId = X.vint('cmbPackUOM'); r.PackUomCode = str(uom.UOMCode); r.PackUomEquivalent = netD(uom.Equivalent);
        r.NoOfBags = netD(X.val('txtNoOfBags')); r.NetWeight = netD(X.val('txtNetWeight')); r.EbUnit = netD(X.val('txtEbUnit'));
        r.EbTotal = netD(X.val('txtEbTotal')); r.AddLess = netD(X.val('txtAddLess')); r.GrossWeight = netD(X.val('txtGrossWeight'));
        r.StockWeight = netD(X.val('txtStockWeight')); r.ItemDescription = X.val('txtItemDesc');
        r['Container#'] = X.text('cmbcontainerNo'); r['Seal#'] = X.val('txtSeal');
        drawDetail();
        formDetailReset();
    }
    /* FormDetailReset (CropYear and No of Bags are not cleared on the desktop). */
    function formDetailReset() {
        S.updateIndex = -1;
        ['CmbWareHouse', 'cmbItem', 'CmbJobLot', 'cmbPackType', 'cmbPackUOM', 'cmbcontainerNo'].forEach(function (c) { X.setVal(c, 0); });
        ['txtItemDesc', 'txtNetWeight', 'txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtAddLess', 'txtStockWeight', 'txtSeal'].forEach(function (c) { X.setText(c, ''); });
        detailButtons(false);
        X.focus('CmbWareHouse');
    }
    function btnAddinGrid() { box('You Can Not Add Record..please Use Detail Grid Add Button For Adding Row'); }

    // ------------------------------------------------------------------------------ gate pass / forwarding

    /* cmbGpNo TextChanged / Leave -> GatePassDataForExportReturnGrn. */
    function gatePassChanged() {
        var gp = X.vint('cmbGpNo'); if (gp <= 0) return Promise.resolve();
        return X.getJson(API + '/gate-pass?gpId=' + gp + '&recId=' + S.recId).then(function (d) {
            if (d.found) {
                X.setText('txtVehicleNo', d.VehicleNo); X.setText('txtBilityNo', d.BiltyNo); X.setText('txtGPDate', X.isoDate(d.GpDate));
                X.setText('txtFreight', d.NetPaid); X.setText('txtFactoryWeight', d.FactoryWeight);
                X.setText('txtconainer1', d.Container); X.setText('txtContainer2', d.Container1);
                L.containers = [{ Id: d.Container, Container: d.Container }, { Id: d.Container1, Container: d.Container1 }];
                X.bind('cmbcontainerNo', L.containers, 'Id', 'Container');
            } else {
                X.setText('txtVehicleNo', ''); X.setText('txtBilityNo', ''); X.setText('txtconainer1', ''); X.setText('txtContainer2', '');
                X.setText('txtFactoryWeight', '0'); X.setText('txtFreight', '0');
            }
        }).catch(function (e) { box(e.message); });
    }
    function selectedForwarding() {
        var id = X.vint('cmbForwardingNo'); if (id <= 0) return null;
        return L.forwardings.filter(function (f) { return netI(f.ForwardingId) === id; })[0] || null;
    }
    /* cmbInvoiceNo_Leave (the Outward Gp # combo). */
    function forwardingLeave() {
        var f = selectedForwarding(); if (!f) return Promise.resolve();
        return fillDetailFromExportForwarding(netI(f.ForwardingId)).then(function () { S.forwardingIdCombo = netI(f.ForwardingId); });
    }
    /* FillDetailFromExportForwarding. */
    function fillDetailFromExportForwarding(fid, keepRows) {
        if (!keepRows) {
            if (S.detail.length === 0) { S.detail = []; L.items = []; }
            else {
                if (S.forwardingIdCombo === fid) return Promise.resolve();
                if (!ask('Grid Detail Already have Record,Do You Want To Reset and Load Again???')) { X.setVal('cmbForwardingNo', S.forwardingIdCombo); return Promise.resolve(); }
                while (S.detail.length) { if (deleteDetailRow(0) === false || S.updateIndex !== -1) break; }
                S.detail = []; L.items = [];
            }
        }
        return X.getJson(API + '/forwarding?id=' + fid).then(function (d) {
            if (!d.found) {
                X.bind('cmbCustomer', [], 'Id', 'CompanyName'); X.bind('cmbLoadingPort', [], 'Id', 'PortName'); X.bind('cmbDestinationPort', [], 'Id', 'PortName');
                drawDetail(); return;
            }
            var h = d.header;
            /* The customer / port combos are re-bound to the forwarding's single values - and the loading-port combo gets the
               DESTINATION port while the destination-port combo gets the LOADING port (desktop quirk, kept). */
            X.bind('cmbCustomer', [{ Id: h.SupplierCustomerId, CompanyName: h.Customer }], 'Id', 'CompanyName', null, h.SupplierCustomerId);
            X.bind('cmbLoadingPort', [{ Id: h.DestinationPortId, PortName: h.DestinationPort }], 'Id', 'PortName', null, h.DestinationPortId);
            X.bind('cmbDestinationPort', [{ Id: h.LoadingPortId, PortName: h.LoadingPort }], 'Id', 'PortName', null, h.LoadingPortId);
            (d.rows || []).forEach(function (r) { L.items.push({ Id: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode }); S.detail.push(r); });
            drawDetail();
            itemNameBind();
            if ((d.otherItems || []).length) {
                L.otherItemList = d.otherItemList || [];
                S.others = d.otherItems.map(function (o) { o.ContainerNo = X.val('txtconainer1'); return o; });
            }
        });
    }

    // ------------------------------------------------------------------------------ reset / new / refresh

    /* FormReset (Remarks, Doc Date and Inward Gp Date are not cleared on the desktop). */
    function formReset() {
        S.removed = []; S.forwardingData = []; S.recId = 0; showMode(false);
        ['cmbGpNo', 'cmbForwardingNo', 'cmbCustomer', 'cmbShippingAgent', 'cmbShippingLIne', 'cmbLoadingPort', 'cmbDestinationPort'].forEach(function (c) { X.setEnabled(c, true); });
        S.forwardingIdCombo = 0;
        ['cmbGpNo', 'cmbForwardingNo', 'cmbCustomer', 'cmbShippingLIne', 'cmbShippingAgent', 'cmbtransporter', 'cmbLoadingPort', 'cmbDestinationPort'].forEach(function (c) { X.setVal(c, 0); });
        ['txtFreight', 'txtNoOfContainer', 'txtVehicleNo', 'txtBilityNo', 'txtDriverName', 'txtDriverCellNo', 'txtCNICNO', 'txtconainer1', 'txtContainer2'].forEach(function (c) { X.setText(c, ''); });
        X.setText('txtFactoryWeight', '0');
        formDetailReset();
        S.detail = []; S.others = []; L.otherItemList = [];
        drawDetail();
        return X.getJson(API + '/reset').then(function (d) {
            X.setText('txtdocno', d.docNo);
            if (!reverse()) bindForwardings(d.forwardings);
            bindGatePasses(d.gatePasses);
            X.focus('cmbGpNo');
        });
    }
    function btnNew(btn) { return X.busy(btn, function () { return formReset().then(formDetailReset); }); }
    function btnRefresh(btn) {
        return X.busy(btn, function () {
            return X.getJson(API + '/refresh?recId=' + S.recId).then(function (d) {
                CFG = d.config || CFG; applyConfig();
                bindGatePasses(d.gatePasses);
                if (!reverse()) bindForwardings(d.forwardings);
                bindGlobals(d);
                drawDetail();
            });
        });
    }

    // ------------------------------------------------------------------------------ save / update / delete

    function fieldCheck(v, name, i) {
        var bad = v === null || v === undefined || (typeof v === 'number' && v <= 0) || (typeof v === 'string' && v.trim() === '');
        if (bad) throw new Error(name + ' is required in Detail Grid at row No: ' + (i + 1));
    }
    /* Insert(). */
    function insert(btn) {
        if (!S.detail.length) { box('Grid Record Not Found'); return Promise.resolve(); }
        var fail = function (m, id) { box(m); X.focus(id); return Promise.resolve(); };
        if (!X.val('txtdocno').trim()) return fail('Doc No field is required', 'txtdocno');
        if (X.vint('cmbGpNo') === 0) return fail('GatePass No field is required', 'cmbGpNo');
        if (!reverse() && X.vint('cmbForwardingNo') === 0) return fail('Forwarding No field is required', 'cmbForwardingNo');
        if (X.vint('cmbCustomer') === 0) return fail('Customer field is required', 'cmbCustomer');
        if (!X.val('txtVehicleNo').trim()) return fail('Vehicle No field is required', 'txtVehicleNo');
        if (X.vint('cmbLoadingPort') === 0) return fail('Loading Port field is required', 'cmbLoadingPort');
        if (X.vint('cmbDestinationPort') === 0) return fail('Destination Port field is required', 'cmbDestinationPort');
        if (netD(X.val('txtNoOfContainer')) === 0) return fail('No Of Container must be a non-zero number', 'txtNoOfContainer');
        if (netD(X.val('txtFactoryWeight')) === 0) return fail('Factory Weight must be a non-zero number', 'txtFactoryWeight');
        var trans = X.vint('cmbtransporter'), freight = netD(X.val('txtFreight'));
        if (trans === 0 && freight > 0) return fail('Transporter is Required when Freight is greater than Zero', 'cmbtransporter');
        if (trans !== 0 && freight === 0) return fail('Freight is Required when Transporter is Selected', 'txtFreight');
        var gross = 0; S.detail.forEach(function (r) { gross += netD(r.GrossWeight); });
        if (netD(X.val('txtFactoryWeight')) !== gross) return fail('Factory Weight and Total Gross Weight In Grid Should Be Equal', 'txtFactoryWeight');
        if (!ask(S.recId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return Promise.resolve();
        try {
            S.detail.forEach(function (r, i) {
                fieldCheck(netI(r.ContractId), 'ContractId', i); fieldCheck(netI(r.InvoiceDetailId), 'InvoiceDetailId', i);
                if (!reverse()) fieldCheck(netI(r.ExImForwardingDetailId), 'ExImForwardingDetailId', i);
                fieldCheck(netI(r.WarehouseId), 'Warehouse', i); fieldCheck(netI(r.ItemId), 'Item', i); fieldCheck(netI(r.CropYearId), 'CropYear', i);
                fieldCheck(netI(r.JobLotId), 'Job Lot', i); fieldCheck(netI(r.PackingTypeId), 'Packing Type', i); fieldCheck(netI(r.PackUomId), 'Pack UOM', i);
                fieldCheck(netD(r.NoOfBags), 'No Of Bags', i); fieldCheck(netD(r.GrossWeight), 'Gross Weight', i); fieldCheck(netD(r.NetWeight), 'Net Weight', i);
                fieldCheck(netD(r.StockWeight), 'Stock Weight', i);
            });
        } catch (e) { box(e.message); return Promise.resolve(); }
        var f = selectedForwarding();
        var body = {
            recId: S.recId,
            header: {
                DocNo: X.val('txtdocno'), DocDate: X.val('DocDate'), GPId: X.vint('cmbGpNo'), GpNo: X.text('cmbGpNo'), GPDate: X.val('txtGPDate'),
                ExImForwardingId: f ? netI(f.ForwardingId) : 0, GatePassOutwardId: f ? netI(f.GatePassOutwardId) : 0, RemarksHeader: X.val('txtRemarksHeader'),
                SupplierCustomerId: X.vint('cmbCustomer'), LoadingPortId: X.vint('cmbLoadingPort'), DestinationPortId: X.vint('cmbDestinationPort'),
                TransporterId: trans, ShippingLineId: X.vint('cmbShippingLIne'), ShippingAgentId: X.vint('cmbShippingAgent'), FreightAmount: X.val('txtFreight'),
                VehicleNo: X.val('txtVehicleNo'), BiltyNo: X.val('txtBilityNo'), NoOfContainer: X.val('txtNoOfContainer'), DriverName: X.val('txtDriverName'),
                DriverCellNo: X.val('txtDriverCellNo'), DriverCnicNo: X.val('txtCNICNO'), Container: X.val('txtconainer1'), Container1: X.val('txtContainer2'),
                FactoryWeight: X.val('txtFactoryWeight')
            },
            rows: S.detail, removed: S.removed, otherItems: S.others
        };
        var preview = $id('ChkPrintPreview').checked;
        return X.busy(btn, function () {
            return X.postJson(API + '/save', body).then(function (d) {
                box(d.message);
                return formReset().then(function () { if (preview) return printSlip(null, d.id); });
            });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    function update(btn) { return insert(btn); }
    function btnDelete(btn) {
        if (S.recId === 0) { box('Record Not Delete because RecId No Found'); return; }
        if (!ask('Are you sure to Delete?')) return;
        return X.busy(btn, function () { return X.postJson(API + '/delete', { recId: S.recId }).then(function (d) { box(d.message); return formReset(); }); });
    }

    // ------------------------------------------------------------------------------ read

    /* ReadById. */
    function readById(id) {
        return formReset().then(function () {
            S.recId = id;
            return X.getJson(API + '/by-id?id=' + id);
        }).then(function (d) {
            var h = d.header, rows = d.rows || [];
            X.selectTab('grn', 'grnForm'); tabChanged('grnForm');
            showMode(true);
            X.setText('DocDate', X.isoDate(h.DocDate));
            X.setText('txtdocno', h.DocNo);
            return X.getJson(API + '/edit-data?id=' + id + '&forwardingId=' + h.ExImForwardingId + '&invoiceId=' + (rows.length ? netI(rows[0].InvoiceId) : 0)).then(function (e) {
                bindGatePasses(e.gatePasses);
                X.setVal('cmbGpNo', h.GPId);
                return gatePassChanged().then(function () {
                    X.setText('txtGPDate', X.isoDate(h.GPDate));
                    var next = Promise.resolve();
                    if (!reverse()) {
                        bindForwardings(e.forwardings);
                        X.setVal('cmbForwardingNo', h.ExImForwardingId);
                        if (selectedForwarding()) { next = fillDetailFromExportForwarding(netI(h.ExImForwardingId)).then(function () { S.forwardingIdCombo = netI(h.ExImForwardingId); }); }
                    } else {
                        S.forwardingData = e.forwardingData || [];
                        X.bind('cmbCustomer', L.customers, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'MobileNo']);
                        X.bind('cmbShippingAgent', L.salesmen, 'Id', 'CompanyName'); X.bind('cmbShippingLIne', L.salesmen, 'Id', 'CompanyName');
                        X.bind('cmbtransporter', L.salesmen, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'MobileNo']);
                    }
                    return next.then(function () {
                        ['cmbCustomer', 'cmbShippingLIne', 'cmbShippingAgent', 'cmbLoadingPort', 'cmbDestinationPort'].forEach(function (c) { X.setEnabled(c, !reverse()); });
                        X.setVal('cmbCustomer', h.SupplierCustomerId); X.setVal('cmbDestinationPort', h.DestinationPortId); X.setVal('cmbLoadingPort', h.LoadingPortId);
                        X.setVal('cmbShippingLIne', h.ShippingLineId); X.setVal('cmbShippingAgent', h.ShippingAgentId); X.setVal('cmbtransporter', h.TransporterId);
                        X.setText('txtRemarksHeader', h.RemarksHeader);
                        X.setText('txtFreight', fA(h.FreightAmount));
                        X.setText('txtVehicleNo', h.VehicleNo); X.setText('txtBilityNo', h.BiltyNo);
                        X.setText('txtFactoryWeight', X.opt(h.GpFactoryWeight, 3)); X.setText('txtNoOfContainer', X.opt(h.NoOfContainer, 3));
                        X.setText('txtDriverCellNo', h.DriverCellNo); X.setText('txtDriverName', h.DriverName); X.setText('txtCNICNO', h.DriverCnicNo);
                        X.setText('txtconainer1', h.Container); X.setText('txtContainer2', h.Container1);
                        S.detail = rows;
                        L.otherItemList = e.otherItemList || [];
                        S.others = d.otherItems || [];
                        drawDetail();
                    });
                });
            });
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ print / attachment

    function printSlip(btn, id) {
        id = id === undefined ? S.recId : id;
        if (netI(id) <= 0) { box('Not Record Found For Display'); return; }
        return X.printPdf('/reports/print/215-export-return-grn-slip', { id: id }, btn);
    }
    function attachment() { box('Attachments are not available in the web version.'); }

    // ------------------------------------------------------------------------------ history

    function historyCombos(d) {
        X.bind('CmbCustomerHistory', d.customers || [], 'Id', 'Name');
        X.bind('CmbInvoiceNoHistory', d.invoices || [], 'Id', 'Name');
    }
    function historyShow(btn) {
        return X.busy(btn, function () {
            return X.postJson(API + '/history', {
                fromChecked: $id('fromdateHistoryChk').checked, fromDate: X.val('fromdateHistory'),
                toChecked: $id('ToDateHistoryChk').checked, toDate: X.val('ToDateHistory'),
                fromDocNo: X.val('txtFromNoHistory'), toDocNo: X.val('txtToDocNoHistory'),
                exImInvoiceId: X.vint('CmbInvoiceNoHistory'), supplierCustomerId: X.vint('CmbCustomerHistory')
            }).then(function (rows) { H.rows = rows || []; H.cur = -1; drawHistory(); if (!H.rows.length) { H.detail = []; drawHistoryDetail(); } });
        });
    }
    function drawHistory() {
        X.grid('DataGridHistory', {
            cols: HIST_COLS, rows: H.rows, current: H.cur, empty: 'DataGridHistoryEmpty',
            lead: [{ t: 'Edit', html: function () { return X.btnHtml('Edit', 'Edit'); } },
                   { t: 'Print', html: function () { return X.btnHtml('Print', 'Print'); } },
                   { t: 'NoOfAttachments', html: function (r) { return '<a class="win-code" data-code="NoOfAttachments">' + X.esc(r.NoOfAttachments) + '</a>'; } }],
            onRow: function (i) { H.cur = i; historySelect(i); },
            onDbl: function (i) { readById(netI(H.rows[i].Id)); },
            onCode: function (i, k) { if (k === 'NoOfAttachments') attachment(); else readById(netI(H.rows[i].Id)); },
            onButton: function (i, act, b) { var id = netI(H.rows[i].Id); if (act === 'Edit') readById(id); if (act === 'Print') printSlip(b, id); }
        });
    }
    function historySelect(i) {
        return X.getJson(API + '/by-id?id=' + netI(H.rows[i].Id)).then(function (d) { H.detail = (d && d.rows) || []; drawHistoryDetail(); }).catch(function (e) { box(e.message); });
    }
    function drawHistoryDetail() { X.grid('DataGridHistoryDetail', { cols: detailCols(false), rows: H.detail }); }
    function historyReset() {
        X.setText('fromdateHistory', X.daysAgo(3)); X.setText('ToDateHistory', X.today());
        X.setText('txtFromNoHistory', ''); X.setText('txtToDocNoHistory', '');
        X.setVal('CmbInvoiceNoHistory', 0); X.setVal('CmbCustomerHistory', 0);
        H.rows = []; H.detail = []; drawHistory(); drawHistoryDetail();
    }
    function historyRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/history-combos').then(historyCombos); }); }

    // ------------------------------------------------------------------------------ frmLoadExportReurnInvoiceForGrn

    var LD_MAIN_COLS = [
        { k: 'Id', hide: true }, { k: 'DocNo', t: 'DocNo' }, { k: 'DocDate', t: 'DocDate', date: 'short' }, { k: 'SupplierCustomerId', hide: true },
        { k: 'CustomerName', t: 'CustomerName', w: 160 }, { k: 'InvoiceQty', t: 'InvoiceQty', num: 3 }, { k: 'UsedQty', t: 'UsedQty', num: 3 }, { k: 'BalQty', t: 'BalQty', num: 3 },
        { k: 'InvoiceWeight', t: 'InvoiceWeight', num: 3 }, { k: 'UsedWeight', t: 'UsedWeight', num: 3 }, { k: 'BalWeight', t: 'BalWeight', num: 3 }
    ];
    var LD_DETAIL_COLS = [
        { k: 'Id', hide: true }, { k: 'DetailId', hide: true }, { k: 'DocNo', t: 'DocNo' }, { k: 'DocDate', t: 'DocDate', date: 'short' }, { k: 'SupplierCustomerId', hide: true },
        { k: 'CustomerName', t: 'CustomerName', w: 150 }, { k: 'ContractId', hide: true }, { k: 'ContractNo', t: 'ContractNo' }, { k: 'ExImInvoiceId', hide: true },
        { k: 'InvoiceNo', t: 'InvoiceNo' }, { k: 'ExImInvoiceDetailId', hide: true }, { k: 'WareHouseId', hide: true }, { k: 'WareHouseName', t: 'WareHouseName' },
        { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 150 }, { k: 'ItemCode', t: 'ItemCode' }, { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot' },
        { k: 'CropYearId', hide: true }, { k: 'CropYear', t: 'CropYear' }, { k: 'PackingTypeId', hide: true }, { k: 'PackingType', t: 'PackingType' },
        { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom' }, { k: 'InvoiceQty', t: 'InvoiceQty', num: 3 }, { k: 'UsedQty', t: 'UsedQty', num: 3 },
        { k: 'BalQty', t: 'BalQty', num: 3 }, { k: 'InvoiceWeight', t: 'InvoiceWeight', num: 3 }, { k: 'UsedWeight', t: 'UsedWeight', num: 3 },
        { k: 'BalWeight', t: 'BalWeight', num: 3 }, { k: 'CostRate', hide: true }
    ];
    function loaderOpen(btn) {
        if (!reverse()) return;
        return X.busy(btn, function () {
            return X.getJson(API + '/loader/setup').then(function (d) {
                X.setText('ldFromDate', X.isoDate(d.fromDate) || X.today()); X.setText('ldTodate', X.today());
                loaderCombos(d.combos || {});
                X.modal('loaderModal', true);
                return loaderRows();
            });
        });
    }
    function loaderCombos(c) {
        X.bind('ldCmbInvoiceNo', c.InvoiceNo || [], 'Id', 'name'); X.bind('ldCmbCustomer', c.Customer || [], 'Id', 'name');
        X.bind('ldCmbItem', c.Item || [], 'Id', 'name'); X.bind('ldCmbJobLot', c.JobLot || [], 'Id', 'name'); X.bind('ldCmbCrop', c.Crop || [], 'Id', 'name');
    }
    function loaderRows() {
        return X.postJson(API + '/loader/rows', {
            fromDate: X.val('ldFromDate'), toDate: X.val('ldTodate'), exImInvoiceId: X.vint('ldCmbInvoiceNo'), supplierCustomerId: X.vint('ldCmbCustomer'),
            itemId: X.vint('ldCmbItem'), jobLotId: X.vint('ldCmbJobLot'), cropYearId: X.vint('ldCmbCrop')
        }).then(function (rows) {
            LD.rows = rows || []; LD.main = []; LD.detail = []; LD.checked = {}; LD.mainCur = -1;
            var map = {};
            LD.rows.forEach(function (r) {
                var k = netI(r.Id), g = map[k];
                if (!g) { g = map[k] = { Id: k, DocNo: r.DocNo, DocDate: r.DocDate, SupplierCustomerId: r.SupplierCustomerId, CustomerName: r.CustomerName,
                    InvoiceQty: 0, UsedQty: 0, BalQty: 0, InvoiceWeight: 0, UsedWeight: 0, BalWeight: 0 }; LD.main.push(g); }
                ['InvoiceQty', 'UsedQty', 'BalQty', 'InvoiceWeight', 'UsedWeight', 'BalWeight'].forEach(function (c) { g[c] += netD(r[c]); });
            });
            drawLoader();
        });
    }
    function drawLoader() {
        X.grid('ldGrdMain', { cols: LD_MAIN_COLS, rows: LD.main, current: LD.mainCur, onRow: function (i) { LD.mainCur = i; loaderMainSelect(i); } });
        X.grid('ldGrdDetail', { cols: LD_DETAIL_COLS, rows: LD.detail, lead: [{ t: 'Select', html: function (r, i) { return X.checkHtml('Select', !!LD.checked[i]); } }] });
        var tb = $id('ldGrdDetail').tBodies[0];
        if (tb) tb.addEventListener('change', function (e) {
            if (e.target.getAttribute('data-act') !== 'Select') return;
            LD.checked[parseInt(e.target.closest('tr[data-i]').getAttribute('data-i'), 10)] = e.target.checked; loaderTotals();
        });
        loaderTotals();
    }
    function loaderMainSelect(i) {
        var id = netI(LD.main[i].Id);
        LD.detail = LD.rows.filter(function (r) { return netI(r.Id) === id; }).map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; o.JobLot = r.JobLotDescription; return o; });
        LD.checked = {}; LD.detail.forEach(function (r, j) { LD.checked[j] = true; });
        drawLoader();
    }
    function loaderTotals() {
        var t = { InvoiceQty: 0, InvoiceWeight: 0, UsedQty: 0, UsedWeight: 0, BalQty: 0, BalWeight: 0 };
        LD.detail.forEach(function (r, i) { if (LD.checked[i]) for (var k in t) t[k] += netD(r[k]); });
        X.setText('txtOrderQtyHeader', fA(t.InvoiceQty)); X.setText('txtQrderWeightHeader', fA(t.InvoiceWeight));
        X.setText('txtUsedQtyHeader', fA(t.UsedQty)); X.setText('txtUsedWeightHeader', fA(t.UsedWeight));
        X.setText('txtBalanceQtyHeader', fA(t.BalQty)); X.setText('txtBalanceWeightHeader', fA(t.BalWeight));
    }
    function loaderShow(btn) { return X.busy(btn, loaderRows); }
    function loaderReset(btn) { ['ldCmbCustomer', 'ldCmbInvoiceNo', 'ldCmbItem', 'ldCmbJobLot', 'ldCmbCrop'].forEach(function (c) { X.setVal(c, 0); }); return X.busy(btn, loaderRows); }
    function loaderRefresh(btn) { return X.busy(btn, function () { return X.getJson(API + '/loader/setup').then(function (d) { loaderCombos(d.combos || {}); }); }); }
    function loaderClose() { X.modal('loaderModal', false); }
    function loaderLoad(btn) {
        var picked = LD.detail.filter(function (r, i) { return LD.checked[i]; });
        if (!picked.length) { box('Please check at least one row.'); return; }
        var ids = {}; picked.forEach(function (r) { ids[netI(r.Id)] = 1; });
        if (Object.keys(ids).length > 1) { box('You can only select rows from the same Invoice.'); return; }
        var loader = picked.map(function (r) { return LD.rows.filter(function (x) { return netI(x.DetailId) === netI(r.DetailId); })[0]; }).filter(Boolean);
        X.modal('loaderModal', false);
        return X.busy(btn, function () { return loadInGridDetail(loader); });
    }
    /* LoadInGridDetail. */
    function loadInGridDetail(rows) {
        S.forwardingData = [];
        if (!rows.length) return Promise.resolve();
        var dr = rows[0], invoiceId = netI(dr.Id), exImInvoiceId = netI(dr.ExImInvoiceId);
        for (var j = 0; j < S.detail.length; j++) if (netI(S.detail[j].ReturnInvoiceId) !== invoiceId) { box('Data against another Invoice Already Exist in Detail'); return Promise.resolve(); }
        ['cmbCustomer', 'cmbShippingAgent', 'cmbShippingLIne', 'cmbLoadingPort', 'cmbDestinationPort'].forEach(function (c) { X.setEnabled(c, false); });
        X.setVal('cmbCustomer', netI(dr.SupplierCustomerId)); X.setVal('cmbLoadingPort', netI(dr.LoadingPortId)); X.setVal('cmbDestinationPort', netI(dr.DestinationPortId));
        return X.getJson(API + '/forwarding-data?invoiceId=' + exImInvoiceId).then(function (fd) {
            S.forwardingData = fd || [];
            if (!S.forwardingData.length) throw new Error('There is no row at position 0.');
            X.setVal('cmbShippingAgent', netI(S.forwardingData[0].ShippingAgentId)); X.setVal('cmbShippingLIne', netI(S.forwardingData[0].ShippingLineId));
            X.setVal('cmbtransporter', netI(S.forwardingData[0].TransporterId));
            for (var i = 0; i < rows.length; i++) {
                var r = rows[i], flag = false;
                for (var k = 0; k < S.detail.length; k++) {
                    var g = S.detail[k];
                    if (netI(g.ReturnInvoiceId) !== netI(r.Id)) { box('Data against another Invoice Already Exist in Detail'); drawDetail(); return; }
                    if (netI(g.ReturnInvoiceDetailId) === netI(r.DetailId)) { flag = true; break; }
                }
                if (!flag) S.detail.push({ Id: 0, ContractId: r.ContractId, ContractDetailId: r.ContractDetailId, ContractNo: r.ContractNo, ContractDate: r.LcOrderDate,
                    InvoiceId: r.ExImInvoiceId, InvoiceDetailId: r.ExImInvoiceDetailId, InvoiceNo: r.InvoiceNo, ExImForwardingDetailId: 0, ReturnInvoiceId: r.Id,
                    ReturnInvoiceDetailId: r.DetailId, ReturnInvoiceNo: r.DocNo, WarehouseId: r.WareHouseId, Warehouse: r.WareHouseName, ItemId: r.ItemId,
                    ItemCode: r.ItemCode, ItemName: r.ItemName, CropYearId: r.CropYearId, CropYear: r.CropYear, JobLotId: r.JobLotId, JobLot: r.JobLotDescription,
                    PackingTypeId: r.PackingTypeId, PackingType: r.PackingType, PackUomId: r.PackUomId, PackUomCode: r.PackUom, PackUomEquivalent: r.PackEquivalent,
                    NoOfBags: r.BalQty, GrossWeight: r.BalWeight, EbUnit: 0, EbTotal: 0, AddLess: 0, NetWeight: r.BalWeight, StockWeight: r.BalWeight,
                    ItemDescription: '', 'Container#': '', 'Seal#': '', CostRate: r.CostRate });
            }
            drawDetail();
        });
    }

    // ------------------------------------------------------------------------------ wiring / shortcuts

    var SHORTCUTS = [['Ctrl+S', 'For Save and for Show on history'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'],
        ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print 241 Slip'], ['Alt+1', 'For Print 241 Slip'], ['Ctrl+F5', 'For Focus on DocDate'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'],
        ['Ctrl+Delete', 'For Deleting an row of Focused Grid'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On GpNo'],
        ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() { X.shortcuts(SHORTCUTS); }
    function loaderShortcuts() {
        X.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function vis(id) { var e = $id(id); return e && !e.classList.contains('is-hidden') && !e.disabled; }
    function keyDown(e) {
        var k = (e.key || '').toLowerCase();
        if (!$id('loaderModal').classList.contains('is-hidden')) {
            if (k === 'escape' || (e.ctrlKey && k === 'e')) { e.preventDefault(); loaderClose(); }
            else if (e.ctrlKey && e.altKey) loaderShortcuts();
            else if (e.ctrlKey && k === 's') { e.preventDefault(); loaderShow($id('ldBtnShow')); }
            else if (e.ctrlKey && k === 'l') { e.preventDefault(); loaderLoad($id('ldBtnLoad')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); loaderReset($id('ldBtnNew')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); loaderRefresh($id('ldBtnRefresh')); }
            return;
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
        if (k === 'escape' || (e.ctrlKey && k === 'e')) { e.preventDefault(); X.cancel(); return; }
        if (e.ctrlKey && e.altKey) { shortcuts(); return; }
        if (!onHistory()) {
            if (e.ctrlKey && e.shiftKey && k === 'delete' && vis('btnDelete')) { e.preventDefault(); btnDelete($id('btnDelete')); }
            else if (e.ctrlKey && k === 's' && vis('btnSave')) { e.preventDefault(); save($id('btnSave')); }
            else if (e.ctrlKey && k === 'u' && vis('btnUpdate')) { e.preventDefault(); update($id('btnUpdate')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew($id('BtnNew')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); X.focus('DocDate'); }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); X.focus('cmbGpNo'); }
            else if (e.ctrlKey && k === 'f10') { e.preventDefault(); attachment(); }
            else if ((e.ctrlKey && k === 'p') || (e.altKey && k === '1')) { e.preventDefault(); printSlip($id('BtnPrint')); }
            return;
        }
        if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnshowHistory')); }
        else if (e.ctrlKey && k === 'f5') { e.preventDefault(); X.focus('fromdateHistory'); }
        else if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
        else if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('BtnRefreshHistory')); }
        else if (e.ctrlKey && k === 'enter' && H.cur >= 0 && PERM.Update) { e.preventDefault(); readById(netI(H.rows[H.cur].Id)); }
        else if (e.ctrlKey && k === 'p' && H.cur >= 0 && PERM.Print) { e.preventDefault(); printSlip(null, netI(H.rows[H.cur].Id)); }
    }
    function wire() {
        X.tabs('grn', tabChanged);
        X.tabs('grnDet');
        X.fullscreen();
        X.on('cmbGpNo', 'change', gatePassChanged);
        X.onLeave('cmbGpNo', gatePassChanged);
        X.onLeave('cmbForwardingNo', forwardingLeave);
        X.on('cmbPackUOM', 'change', function () { grossWtCalculation(); netWtCalculation(); });
        ['txtNoOfBags', 'txtEbUnit', 'txtAddLess', 'txtGrossWeight'].forEach(function (id) { X.on(id, 'input', function () { HANDLERS[id](); }); });
        document.addEventListener('keydown', keyDown);
    }

    global.ExportReturnGrn = {
        btnNew: btnNew, btnRefresh: btnRefresh, save: save, update: update, btnDelete: btnDelete, attachment: attachment,
        print: function (b) { return printSlip(b); }, shortcuts: shortcuts, toggleHistory: toggleHistory,
        btnAddinGrid: btnAddinGrid, btnDetailUpdate: btnDetailUpdate, btnDetailCancel: formDetailReset,
        historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh,
        loaderOpen: loaderOpen, loaderClose: loaderClose, loaderShow: loaderShow, loaderReset: loaderReset, loaderRefresh: loaderRefresh,
        loaderLoad: loaderLoad, loaderShortcuts: loaderShortcuts
    };
    document.addEventListener('DOMContentLoaded', function () { wire(); load(); });
}(window));
