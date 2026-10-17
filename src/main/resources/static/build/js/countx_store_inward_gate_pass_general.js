/* ============================================================================================
 * Screen 342 "Inward Gate Pass General" - Architecture.WinApp.Inventory_Definition.GatePassGeneral
 * (form Name "GatePassGeneral", DocumentTypeId 52). Server side: StoreGatePassGeneralInwardService.java.
 *
 *   InwardGatePass_Load:190         combos, doc no, party / item lists, open list, history party combo
 *   CmbGatePassType_Leave:603       reference document combo for types 6 / 7 / 8, grid cleared on a type change
 *   RefDocNodtFillDbCall:645        FillData / Map*:691-745      RefDocNoBind:746   CmbDeliveryChallanNo_Leave:782
 *   PartiesAndItemdtFillByRefdoc:317  rows of the chosen document are copied into the grid
 *   formvalidation:832  FormValidationDetail:872  Add_Click:919  btnUpdateDetail_Click:938  grd_DoubleClick:999
 *   BtnUpdateSameValuesInGrid_Click:1195  ReadById:1245  Insert:1365  Reset:2086  btnPrint_Click:2217
 *   grdfrmfill:1581 (open list and history)  GetDetailGrdByHeadId:2052  InwardGatePass_KeyDown:2298
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf, ci = C.ci;
    var api = '/api/store/inward-gate-pass-general';
    var ZERO = '...Select Any Value...';

    var look = { rights: {}, warehouses: [], jobLots: [], vehicleTypes: [], wbRows: [], historyParties: [] };
    var GPTYPES = [[1, 'General'], [2, 'Store'], [3, 'SupplierStock'], [4, 'StockTransfer'], [5, 'PackingMaterial'], [6, 'Delivery_Challan'], [7, 'Purchase_Order'], [8, 'Purchase_Demand']];
    var STATUS = [[1, 'Open'], [2, 'Accepted'], [3, 'Rejected']];
    var WB = [[1, 'Non-Weighable'], [2, 'Weighable']];

    var ID = 0;                     // Id
    var gpTypeId = 0;               // GatePassTypeId (set by CmbGatePassType_Leave)
    var updateIdx = -1;             // updateDetailIndex
    var updateMode = false;
    var dtParty = [], dtItem = [];  // {Id, PartyName} / {Id, ItemName, ItemCode}
    var dtRefDocNo = [], dtRefData = [];
    var dtGrid = [];
    var openRows = [], histRows = [], histSel = -1;
    var att = { existing: [], files: [], removed: [] };

    var GCOLS = ['RefDocNo', 'ItemCode', 'ItemName', 'WareHouseId', 'JobLotId', 'PackUom', 'ItemQty', 'Remarks'];
    var GCAP = { WareHouseId: 'WareHouse', JobLotId: 'Job Lot', PackUom: 'UOM' };
    var HCOLS = [['GpType', 'GpType'], ['GpDate', 'GpDate', 'd'], ['GpNo', 'GpNo'], ['DeliveryChallanNo', 'DeliveryChallanNo'], ['ManualNo', 'ManualNo'],
        ['VehicleType', 'VehicleType'], ['VehicleNo', 'VehicleNo'], ['PartyName', 'PartyName'], ['ItemName', 'ItemName'], ['ItemQty', 'ItemQty', 'n'],
        ['Warehouse', 'Warehouse'], ['GpStatus', 'GpStatus'], ['WBStatus', 'WBStatus'], ['FactoryWeight', 'FactoryWeight', 'n'], ['PartyWeight', 'PartyWeight', 'n'],
        ['EntryUser', 'EntryUser'], ['EntryDate', 'EntryDate', 't'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate', 't'],
        ['ApprovedUser', 'ApprovedUser'], ['ApprovedDate', 'ApprovedDate', 't'], ['NoOfAttachments', 'NoOfAttachments', 'a'], ['Remarks', 'Remarks']];
    var HDCOLS = ['RefDocNo', 'ItemCode', 'ItemName', 'WareHouseId', 'JobLotId', 'PackUom', 'ItemQty', 'Remarks', 'WareHouseName', 'JobLot'];
    var KEYS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on GatePass Type'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Item Combo in Detail Box'], ['Ctrl+ArrowRight', 'To change Grid Tab Control,'],
        ['Ctrl+Enter', 'For Update Record When Focus On Any Grid'], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];

    /* ------------------------------------------------------------------ helpers */

    function selText(id) { var e = $id(id); return e && e.value !== '0' && e.value !== '' && e.selectedOptions && e.selectedOptions[0] ? e.selectedOptions[0].textContent.trim() : ''; }
    function selTextRaw(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] ? e.selectedOptions[0].textContent.trim() : ''; }
    function selByText(id, text) {                                      // ((Control)combo).Text = text
        var e = $id(id), t = String(text === null || text === undefined ? '' : text).trim();
        e.value = '0';
        if (!t) return false;
        for (var i = 0; i < e.options.length; i++) if (e.options[i].value !== '0' && e.options[i].textContent.trim() === t) { e.selectedIndex = i; return true; }
        return false;
    }
    function fillPairs(id, pairs, firstReal) {                           // BindAndRetainSelection(insertDefaultRow: true) + Rows[n].Activate()
        var h = '<option value="0">' + ZERO + '</option>';
        pairs.forEach(function (p) { h += '<option value="' + esc(p[0]) + '">' + esc(p[1]) + '</option>'; });
        var e = $id(id), keep = e.value;
        e.innerHTML = h;
        if (keep && keep !== '0') e.value = keep; else if (firstReal && pairs.length >= firstReal) e.selectedIndex = firstReal;
    }
    function alertErr(e) { alert(e && e.message ? e.message : String(e)); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function trimNum(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }
    function netStr(n) { return String(parseFloat(Number(n).toPrecision(15))); }
    function dateTxt(v) { var s = C.isoDay(v); return s || ''; }

    /* ------------------------------------------------------------------ Load */

    function init() {
        return C.getJson(api + '/initial').then(function (r) {
            look = r;
            if (r.rights && !r.rights.view) { var n = $id('rightsNote'); n.textContent = 'You do not have the View right for Inward Gate Pass General.'; n.classList.remove('is-hidden'); }
            $id('btnSave').disabled = !(r.rights && r.rights.save);
            $id('btnPrint').disabled = !(r.rights && r.rights.print);
            $id('btnUpdate').disabled = !(r.rights && r.rights.update);
            if (intOf(r.nextNo) > 0) $id('txtgpno').value = r.nextNo;
            fillPairs('CmbGatePassType', GPTYPES, 1);
            fillWarehouses();
            fillVehicleTypes();
            fillFromLookUp(null); bindParty(); itemNameBind();
            fillPairs('CmbStatus', STATUS, 1);
            fillPairs('CmbWeighBridge', WB, 2);
            factoryWeight();
            bindGrids();
            historyPartyBind();
            return openFill();
        });
    }
    function fillWarehouses() {
        var rows = look.warehouses || [];
        ['CmbWarehouse', 'CmbWareHouseToUpdate'].forEach(function (id) { C.fillSelect(id, rows, 'Id', 'Description'); relabel(id); });
        ['cmbJobLot', 'CmbJobLotToUpdate'].forEach(function (id) { C.fillSelect(id, look.jobLots || [], 'Id', 'Description'); relabel(id); });
    }
    function relabel(id) { var el = $id(id); if (el.options.length && el.options[0].value === '0') el.options[0].textContent = ZERO; }
    function fillVehicleTypes() {
        var el = $id('cmbvehicletype');
        C.fillSelect(el, look.vehicleTypes || [], 'Id', 'VehicleDescription'); relabel('cmbvehicletype');
        if (el.options.length > 1) el.selectedIndex = 1;                                       // Rows[1].Activate()
    }
    function historyPartyBind() { C.fillSelect('cmbPartyHistory', look.historyParties || [], 'Id', 'Name'); relabel('cmbPartyHistory'); }
    function refreshAll() {                                                                   // btnRefresh_Click:2137
        return C.getJson(api + '/warehouses').then(function (w) {
            look.warehouses = w;
            return C.getJson(api + '/initial');
        }).then(function (r) {
            look.vehicleTypes = r.vehicleTypes; look.warehouses = r.warehouses; look.jobLots = r.jobLots;
            fillVehicleTypes(); fillWarehouses();
            gpTypeLeave();
        }).catch(alertErr);
    }

    /* ------------------------------------------------------------------ party / item lists */

    function fillFromLookUp(actionIds) {                                // PartiesAndItemdtFillFromLookUp:263
        if (actionIds === null || actionIds.indexOf(1) >= 0) dtParty = [];
        if (actionIds === null || actionIds.indexOf(2) >= 0) dtItem = [];
        var rows = look.wbRows || [];
        if (!rows.length) return;
        var pIds = {}, iIds = {};
        rows.forEach(function (r) {
            var type = String(ci(r, 'WbType') || ''), id = intOf(ci(r, 'Id'));
            if (type === 'PartyName' && (actionIds === null || actionIds.indexOf(1) >= 0) && !pIds[id]) { pIds[id] = 1; dtParty.push({ Id: id, PartyName: String(ci(r, 'PartyName') || '') }); }
            if (type === 'Item' && (actionIds === null || actionIds.indexOf(2) >= 0) && !iIds[id]) { iIds[id] = 1; dtItem.push({ Id: id, ItemName: String(ci(r, 'ItemName') || ''), ItemCode: '' }); }
        });
    }
    function bindParty() {                                              // BindPartyName:410 (the combo's text is free)
        $id('dlParty').innerHTML = dtParty.map(function (p) { return '<option value="' + esc(p.PartyName) + '">'; }).join('');
    }
    function itemNameBind() {                                           // ItemNameBind:421
        var el = $id('cmbItem'), keep = intOf(el.value), byName = $id('rdbtnItemName').checked;
        var h = '<option value="0">' + ZERO + '</option>';
        dtItem.forEach(function (i) { h += '<option value="' + i.Id + '" data-code="' + esc(i.ItemCode) + '" data-name="' + esc(i.ItemName) + '">' + esc(byName ? i.ItemName : i.ItemCode) + '</option>'; });
        el.innerHTML = h;
        el.value = keep > 0 && dtItem.some(function (i) { return i.Id === keep; }) ? String(keep) : '0';
    }
    function itemLeave() {                                              // CmbItemName_Leave:824 -> ItemUomFromGlobalBind
        return uomBind(intOf($id('cmbItem').value), 0);
    }
    function uomBind(itemId, selectId) {
        return C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (rows) {
            C.fillSelectCols('cmbUOM', rows, 'Id', 'UOMCode', ['Equivalent'], ['UOM', 'Equivalent']);
            if (selectId) $id('cmbUOM').value = String(selectId);
        }).catch(function () { C.fillSelect('cmbUOM', [], 'Id', 'UOMCode'); });
    }

    /* ------------------------------------------------------------------ gate pass type / reference documents */

    function refLabel(t) { return t === 8 ? 'Demand #' : t === 7 ? 'Order No' : t === 6 ? 'D.C No' : ''; }
    function gpTypeLeave() {                                            // CmbGatePassType_Leave:603
        var v = intOf($id('CmbGatePassType').value);
        show('lblDeliveryChallanNo', false); show('CmbDeliveryChallanNo', false);
        $id('CmbDeliveryChallanNo').innerHTML = '<option value="0"></option>';
        $id('CmbGatePassType').style.width = '314px';
        var step;
        if (v >= 6 && v <= 8) {
            step = refDocFill(v).then(function () {
                refDocNoBind();
                show('lblDeliveryChallanNo', true); show('CmbDeliveryChallanNo', true);
                $id('lblDeliveryChallanNo').textContent = refLabel(v);
                $id('CmbGatePassType').style.width = '130px';
                show('PanelDetailInfo', false);
            });
        } else {
            show('PanelDetailInfo', true);
            fillFromLookUp(null); bindParty(); itemNameBind();
            step = Promise.resolve();
        }
        return step.then(function () {
            if (dtGrid.length > 0 && v !== gpTypeId) {
                if (!confirm('You are changing the GP Type. Grid data will be cleared. Are you sure?')) { $id('CmbGatePassType').value = String(gpTypeId); return; }
                dtGrid = [];
                renderGrid();
            }
            gpTypeId = v;
            gridLock();
        }).catch(alertErr);
    }
    function refDocFill(type) {                                         // RefDocNodtFillDbCall:645 + FillData:728
        dtRefDocNo = []; dtRefData = [];
        return C.getJson(api + '/ref-docs' + C.qs({ typeId: type, recId: ID })).then(function (rows) {
            dtRefData = rows || [];
            var ids = {};
            dtRefData.forEach(function (dr) {
                var id = type === 6 ? intOf(ci(dr, 'DeliveryChallanHeaderId')) : intOf(ci(dr, 'Id'));
                if (ids[id]) return;
                ids[id] = 1;
                var r = { Id: 0, DocNo: 0, CityId: 0, CityName: '', VehicleNo: '', BiltyNo: '', PartyId: 0, PartyName: '', ItemId: 0, ItemName: '', ItemCode: '', ItemQty: 0 };
                if (type === 6) {                                                           // MapDeliveryChallan
                    r.Id = ci(dr, 'DeliveryChallanHeaderId'); r.DocNo = ci(dr, 'ChallanNo'); r.CityId = ci(dr, 'CityId'); r.CityName = ci(dr, 'CityName');
                    r.VehicleNo = ci(dr, 'VehicleNo'); r.BiltyNo = ci(dr, 'BiltyNo'); r.PartyId = ci(dr, 'BillToPartyId'); r.PartyName = ci(dr, 'BillToPartyName');
                    r.ItemId = ci(dr, 'ItemId'); r.ItemName = ci(dr, 'ItemName'); r.ItemCode = ci(dr, 'ItemCode'); r.ItemQty = ci(dr, 'DeliveryChallanQty');
                } else if (type === 7) {                                                    // MapPurchaseOrder
                    r.Id = ci(dr, 'Id'); r.DocNo = ci(dr, 'DocNo'); r.CityId = ci(dr, 'CityId'); r.CityName = ci(dr, 'CityName');
                    r.PartyId = ci(dr, 'BillToPartyId'); r.PartyName = ci(dr, 'BillToPartyName');
                    r.ItemId = ci(dr, 'ItemId'); r.ItemName = ci(dr, 'ItemName'); r.ItemCode = ci(dr, 'ItemCode'); r.ItemQty = ci(dr, 'ItemQty');
                } else {                                                                    // MapPurchaseDemand
                    r.Id = ci(dr, 'Id'); r.DocNo = ci(dr, 'DocNo'); r.ItemId = ci(dr, 'ItemId'); r.ItemName = ci(dr, 'ItemName');
                    r.ItemCode = ci(dr, 'ItemCode'); r.ItemQty = ci(dr, 'BalanceQty');
                }
                dtRefDocNo.push(r);
            });
        });
    }
    function refDocNoBind() {                                           // RefDocNoBind:746
        var el = $id('CmbDeliveryChallanNo'), keep = intOf(el.value);
        var h = '<option value="0"></option>';
        dtRefDocNo.forEach(function (r) { h += '<option value="' + esc(r.Id) + '">' + esc(r.DocNo) + '</option>'; });
        el.innerHTML = h;
        el.value = keep > 0 && dtRefDocNo.some(function (r) { return intOf(r.Id) === keep; }) ? String(keep) : '0';
        if (dtRefDocNo.length > 0 && intOf(el.value) > 0) refDocLeave();
    }
    function refDocLeave() {                                            // CmbDeliveryChallanNo_Leave:782
        var id = intOf($id('CmbDeliveryChallanNo').value);
        if (id <= 0) return;
        var cells = dtRefDocNo.filter(function (r) { return intOf(r.Id) === id; })[0];
        if (!cells) return;
        var t = intOf($id('CmbGatePassType').value);
        if (t === 6 || t === 7) byRefDoc(id, t);
        else if (t === 8) { byRefDoc(id, t); fillFromLookUp([1]); }
        bindParty(); itemNameBind();
        $id('txtPartyName').value = cells.PartyName === null || cells.PartyName === undefined ? '' : String(cells.PartyName);
        selByText('cmbItem', cells.ItemName);
        itemLeaveSilent();
        $id('txtqty').value = cells.ItemQty === null || cells.ItemQty === undefined ? '' : String(cells.ItemQty);
        $id('txtvehicleno').value = cells.VehicleNo === null || cells.VehicleNo === undefined ? '' : String(cells.VehicleNo);
    }
    function itemLeaveSilent() { return itemLeave(); }
    function hasCol(name) { return dtRefData.length > 0 && Object.keys(dtRefData[0]).some(function (k) { return k.toLowerCase() === name.toLowerCase(); }); }
    function byRefDoc(id, type) {                                       // PartiesAndItemdtFillByRefdoc:317
        if (type === 8) dtItem = []; else { dtParty = []; dtItem = []; }
        if (!dtRefData.length) return;
        var hasDoc = hasCol('DocumentTypeId');
        var first = hasDoc ? intOf(ci(dtRefData[0], 'DocumentTypeId')) : 0;
        dtGrid = dtGrid.filter(function (r) { return intOf(r.RefDocumentTypeId) === first; });
        var pIds = {}, iIds = {}, exist = {};
        dtGrid.forEach(function (r) { exist[intOf(r.RefDocDetailId)] = 1; });
        var hasWh = hasCol('WareHouseId'), hasJob = hasCol('JobLotId'), hasParty = hasCol('BillToPartyId');
        dtRefData.forEach(function (row) {
            var itemId = intOf(ci(row, 'ItemId')), detailId = intOf(ci(row, 'DetailId'));
            var vendor = hasParty ? intOf(ci(row, 'BillToPartyId')) : 0;
            if (hasParty && !pIds[vendor]) { pIds[vendor] = 1; dtParty.push({ Id: vendor, PartyName: String(ci(row, 'BillToPartyName') || '') }); }
            if (!iIds[itemId]) { iIds[itemId] = 1; dtItem.push({ Id: itemId, ItemName: String(ci(row, 'ItemName') || ''), ItemCode: String(ci(row, 'ItemCode') || '') }); }
            var refDocId = type === 6 ? intOf(ci(row, 'DeliveryChallanHeaderId')) : intOf(ci(row, 'Id'));
            var isNew = !exist[detailId]; exist[detailId] = 1;
            if (isNew && refDocId === id) {
                var dr = { Id: 0, RefDocumentTypeId: hasDoc ? ci(row, 'DocumentTypeId') : null, RefDocId: refDocId,
                    RefDocNo: type === 6 ? ci(row, 'ChallanNo') : ci(row, 'DocNo'), RefDocDetailId: detailId, ItemId: itemId,
                    ItemCode: ci(row, 'ItemCode'), ItemName: ci(row, 'ItemName'),
                    WareHouseId: hasWh && ci(row, 'WareHouseId') !== null && ci(row, 'WareHouseId') !== '' ? ci(row, 'WareHouseId') : null,
                    JobLotId: hasJob && ci(row, 'JobLotId') !== null && ci(row, 'JobLotId') !== '' ? ci(row, 'JobLotId') : null,
                    PackUomId: ci(row, 'UomId'), PackUom: ci(row, 'Uom'), PackUomEquivalent: ci(row, 'UomEquivalent'),
                    ItemQty: type === 7 ? ci(row, 'ItemQty') : type === 6 ? ci(row, 'DeliveryChallanQty') : ci(row, 'BalanceQty'),
                    Remarks: ci(row, 'RemarksDetail') };
                dtGrid.push(dr);
            }
        });
        bindGrids();
    }

    /* ------------------------------------------------------------------ detail entry bar */

    function detailValid() {                                            // FormValidationDetail:872
        if (intOf($id('cmbItem').value) === 0) { alert('Item Field Required'); $id('cmbItem').focus(); return false; }
        if (intOf($id('CmbWarehouse').value) === 0) { alert('Warehouse Field Required'); $id('CmbWarehouse').focus(); return false; }
        if (intOf($id('cmbJobLot').value) === 0) { alert('Joblot Field Required'); $id('cmbJobLot').focus(); return false; }
        if (trimNum($id('txtqty').value.trim()) === 0) { alert('ItemQty Field Required'); $id('txtqty').focus(); return false; }
        return true;
    }
    function fillDetailRow(row) {                                       // FillDetailRow:954
        var opt = $id('cmbItem').selectedOptions[0];
        row.ItemId = intOf($id('cmbItem').value);
        row.ItemCode = opt ? opt.getAttribute('data-code') || '' : '';
        row.ItemName = opt ? opt.getAttribute('data-name') || '' : '';
        row.WareHouseId = intOf($id('CmbWarehouse').value);
        row.JobLotId = intOf($id('cmbJobLot').value);
        var uomId = intOf($id('cmbUOM').value);
        row.PackUomId = uomId;
        row.PackUom = uomId > 0 ? selTextRaw('cmbUOM') : '';
        var o = $id('cmbUOM').selectedOptions[0];
        row.PackUomEquivalent = uomId <= 0 ? 0 : num(((o && o.getAttribute('data-extra')) || '').split('|')[0]);
        row.ItemQty = trimNum($id('txtqty').value.trim());
        row.Remarks = $id('txtRemarksDetail').value.trim();
    }
    function add() {                                                    // Add_Click:919
        if (!detailValid()) return;
        var r = { Id: 0 };
        fillDetailRow(r);
        dtGrid.push(r);
        bindGrids(); resetDetail();
    }
    function updateDetail() {                                           // btnUpdateDetail_Click:938
        if (!detailValid()) return;
        fillDetailRow(dtGrid[updateIdx]);
        renderGrid(); resetDetail();
    }
    function resetDetail() {                                            // ResetDetail:2163 (Job Lot is not cleared)
        updateIdx = -1;
        $id('cmbItem').value = '0'; $id('CmbWarehouse').value = '0';
        C.fillSelect('cmbUOM', [], 'Id', 'UOMCode');
        $id('txtqty').value = ''; $id('txtRemarksDetail').value = '';
        $id('CmbWarehouse').focus();
        show('btnUpdateDetail', false); show('btnCancelDetail', false); show('Add', true);
    }
    function edit(i) {                                                  // grd_DoubleClick:999 (rows copied from a document cannot be edited here)
        var r = dtGrid[i];
        if (!r || intOf(r.RefDocumentTypeId) !== 0) return;
        updateIdx = i;
        $id('cmbItem').value = String(intOf(r.ItemId));
        $id('CmbWarehouse').value = String(intOf(r.WareHouseId));
        $id('cmbJobLot').value = String(intOf(r.JobLotId));
        uomBind(intOf(r.ItemId), intOf(r.PackUomId));
        $id('txtqty').value = r.ItemQty === null || r.ItemQty === undefined ? '' : String(r.ItemQty);
        $id('txtRemarksDetail').value = r.Remarks || '';
        show('Add', false); show('btnUpdateDetail', true); show('btnCancelDetail', true);
    }

    /* ------------------------------------------------------------------ grid */

    function gridLock() {                                               // BindGrids tail
        $id('CmbGatePassType').disabled = dtGrid.length > 0;
        $id('CmbDeliveryChallanNo').disabled = dtGrid.length > 0;
    }
    function bindGrids() { renderGrid(); gridLock(); }
    function whOptions(sel) {
        var h = '<option value="0"></option>';
        (look.warehouses || []).forEach(function (w) { h += '<option value="' + w.Id + '"' + (intOf(sel) === intOf(w.Id) ? ' selected' : '') + '>' + esc(w.Description) + '</option>'; });
        return h;
    }
    function jobOptions(sel) {
        var h = '<option value="0"></option>';
        (look.jobLots || []).forEach(function (w) { h += '<option value="' + w.Id + '"' + (intOf(sel) === intOf(w.Id) ? ' selected' : '') + '>' + esc(w.Description) + '</option>'; });
        return h;
    }
    function renderGrid() {
        var t = $id('grd'), noDel = gpTypeId === 6, qtyEdit = gpTypeId !== 6;
        if (!dtGrid.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr>' + (noDel ? '' : '<th style="width:24px"></th>') + GCOLS.map(function (c) { return '<th>' + esc(GCAP[c] || c) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = dtGrid.map(function (r, i) {
            var h = '<tr data-i="' + i + '" ondblclick="GpInward.edit(' + i + ')">';
            if (!noDel) h += '<td style="text-align:center"><button type="button" class="dz-mini" onclick="event.stopPropagation();GpInward.del(' + i + ')">X</button></td>';
            GCOLS.forEach(function (c) {
                if (c === 'WareHouseId') h += '<td><select class="dz-cell" onchange="GpInward.cell(' + i + ',\'WareHouseId\',this)">' + whOptions(r.WareHouseId) + '</select></td>';
                else if (c === 'JobLotId') h += '<td><select class="dz-cell" onchange="GpInward.cell(' + i + ',\'JobLotId\',this)">' + jobOptions(r.JobLotId) + '</select></td>';
                else if (c === 'ItemQty') h += '<td style="text-align:right">' + (qtyEdit ? '<input class="dz-cell r" value="' + esc(r.ItemQty === null || r.ItemQty === undefined ? '' : r.ItemQty) + '" onkeypress="return GpInward.decimalKey(event,this)" onchange="GpInward.cell(' + i + ',\'ItemQty\',this)">' : esc(r.ItemQty)) + '</td>';
                else if (c === 'Remarks') h += '<td><input class="dz-cell" value="' + esc(r.Remarks || '') + '" onchange="GpInward.cell(' + i + ',\'Remarks\',this)"></td>';
                else h += '<td>' + esc(r[c] === null || r[c] === undefined ? '' : r[c]) + '</td>';
            });
            return h + '</tr>';
        }).join('');
    }
    function cell(i, key, box) {
        var r = dtGrid[i]; if (!r) return;
        if (key === 'ItemQty') r.ItemQty = trimNum(box.value);
        else if (key === 'Remarks') r.Remarks = box.value;
        else r[key] = intOf(box.value);
    }
    function del(i) {                                                   // DeleteDetailRow:1113
        dtGrid.splice(i, 1);
        if (updateIdx === i) resetDetail();
        bindGrids();
    }
    function updateSame() {                                             // BtnUpdateSameValuesInGrid_Click:1195
        try {
            if (!dtGrid.length) throw new Error('Item Detail has no records.');
            var wh = intOf($id('CmbWareHouseToUpdate').value), job = intOf($id('CmbJobLotToUpdate').value);
            if (!(wh !== 0) && !(job !== 0)) {
                wh = intOf(dtGrid[0].WareHouseId); job = intOf(dtGrid[0].JobLotId);
                if (wh === 0 && job === 0) throw new Error('Warehouse and JobLot is empty in row 1.');
            }
            var all = $id('RadAll').checked;
            dtGrid.forEach(function (r) {
                if (wh !== 0 && (all || intOf(r.WareHouseId) === 0)) r.WareHouseId = wh;
                if (job !== 0 && (all || intOf(r.JobLotId) === 0)) r.JobLotId = job;
            });
            renderGrid();
        } catch (e) { alertErr(e); }
    }

    /* ------------------------------------------------------------------ Save (formvalidation + Insert) */

    function formValid() {                                              // formvalidation:832
        if (selText('CmbGatePassType') === '' || intOf($id('CmbGatePassType').value) === 0) { alert('Please Select CmbGatePassType'); $id('CmbGatePassType').focus(); return false; }
        var no = $id('txtgpno').value.trim();
        if (no === '' || no === '0') { alert('Gp No Filed Required'); $id('txtgpno').focus(); return false; }
        if ($id('txtvehicleno').value.trim() === '') { alert('Please Select Vehicle No'); $id('txtvehicleno').focus(); return false; }
        if (selText('cmbvehicletype') === '' || intOf($id('cmbvehicletype').value) === 0) { alert('Please Select Vehicle Type'); $id('cmbvehicletype').focus(); return false; }
        if (selText('CmbWeighBridge') === '' || intOf($id('CmbWeighBridge').value) === 0) { alert('Please Select WeighBridge'); $id('CmbWeighBridge').focus(); return false; }
        if (selText('CmbStatus') === '' || intOf($id('CmbStatus').value) === 0) { alert('Please Select Status'); $id('CmbStatus').focus(); return false; }
        return true;
    }
    function save() { ID = 0; insert(); }
    function update() { insert(); }
    function insert() {                                                 // Insert:1365
        if (!formValid()) return;
        var t = intOf($id('CmbGatePassType').value), docId = intOf($id('CmbDeliveryChallanNo').value);
        if ((t === 6 || t === 7 || t === 8) && docId === 0) {
            alert(t === 6 ? 'Please Select Delivery Challan No' : t === 7 ? 'Please Select Order No' : 'Please Select Demand No');
            $id('CmbDeliveryChallanNo').focus(); return;
        }
        if (!confirm(ID === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var body = {
            id: ID, gpDate: $id('txtgpdate').value, gpNo: $id('txtgpno').value.trim(),
            gatePassTypeId: t, gatePassTypeText: selTextRaw('CmbGatePassType'), refDocId: docId,
            partyName: $id('txtPartyName').value.trim(), manualNo: $id('txtManualNo').value.trim(),
            vehicleTypeId: intOf($id('cmbvehicletype').value), vehicleTypeText: selTextRaw('cmbvehicletype'), vehicleNo: $id('txtvehicleno').value.trim(),
            statusId: intOf($id('CmbStatus').value), statusText: selTextRaw('CmbStatus'),
            weighBridgeId: intOf($id('CmbWeighBridge').value), weighBridgeText: selTextRaw('CmbWeighBridge'),
            factoryWeight: $id('txtFactoryWeight').value.trim(), supplierWeight: $id('txtSupplierWeight').value.trim(), remarks: $id('txtremarks').value.trim(),
            entryItemText: selText('cmbItem') ? ($id('cmbItem').selectedOptions[0].getAttribute('data-name') || selText('cmbItem')) : '',
            entryQty: $id('txtqty').value.trim(), entryWarehouseId: intOf($id('CmbWarehouse').value),
            lines: dtGrid.map(function (r) {
                return { id: intOf(r.Id), refDocumentTypeId: intOf(r.RefDocumentTypeId), refDocId: intOf(r.RefDocId), refDocDetailId: intOf(r.RefDocDetailId),
                    refDocNo: intOf(r.RefDocNo), itemId: intOf(r.ItemId), itemCode: r.ItemCode || '', itemName: r.ItemName || '',
                    wareHouseId: intOf(r.WareHouseId), jobLotId: intOf(r.JobLotId), packUomId: intOf(r.PackUomId), packUom: r.PackUom || '',
                    packUomEquivalent: num(r.PackUomEquivalent), itemQty: num(r.ItemQty), remarks: r.Remarks || '' };
            }),
            attachments: { files: att.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: att.removed }
        };
        C.postJson(api + '/save', body).then(function (res) {
            alert(res.message);
            var printAfter = $id('ChkBox').checked;
            return reset().then(function () { if (printAfter) printId(res.id); });
        }).catch(function (e) { alert(e && e.message ? e.message : 'Database Error'); });
    }

    /* ------------------------------------------------------------------ ReadById / Reset */

    function readById(id) {                                             // ReadById:1245
        return C.getJson(api + '/' + id).then(function (res) {
            var h = res.header;
            return reset().then(function () {
                ID = id;
                tab('tabForm');
                show('btnUpdate', true); show('btnSave', false); updateMode = true;
                $id('txtgpdate').value = C.isoDay(ci(h, 'GpDate'));
                $id('txtgpno').value = ci(h, 'GpSrNo');
                selByText('CmbGatePassType', ci(h, 'GatepassType'));
                return gpTypeLeave().then(function () {
                    var t = intOf($id('CmbGatePassType').value);
                    gpTypeId = t;
                    if (t === 6) $id('CmbDeliveryChallanNo').value = String(intOf(ci(h, 'DeliveryChallanHeaderId')));
                    if (t === 7) $id('CmbDeliveryChallanNo').value = String(intOf(ci(h, 'PurchaseOrderId')));
                    if (t === 8) $id('CmbDeliveryChallanNo').value = String(intOf(ci(h, 'PurchaseDemandId')));
                    if ($id('CmbDeliveryChallanNo').value === null) $id('CmbDeliveryChallanNo').value = '0';
                    refDocLeave();
                    $id('txtremarks').value = ci(h, 'OtherRemarks') || '';
                    $id('txtvehicleno').value = ci(h, 'VehicleNo') || '';
                    selByText('cmbvehicletype', ci(h, 'VehicleType'));
                    $id('txtPartyName').value = ci(h, 'OtherSupCust') || '';
                    selByText('CmbStatus', ci(h, 'DocAttachment'));
                    selByText('CmbWeighBridge', ci(h, 'Status'));
                    $id('txtFactoryWeight').value = String(ci(h, 'FactoryWeight'));
                    $id('txtSupplierWeight').value = String(ci(h, 'SupplierWeight'));
                    $id('txtManualNo').value = ci(h, 'ManualNo') || '';
                    dtGrid = (res.details || []).map(function (d) {
                        return { Id: d.Id, RefDocumentTypeId: d.RefDocumentTypeId, RefDocId: d.RefDocId, RefDocDetailId: d.RefDocDetailId, RefDocNo: d.RefDocNo,
                            WareHouseId: d.WareHouseId, JobLotId: d.JobLotId, ItemId: d.ItemId, ItemCode: d.ItemCode, ItemName: d.ItemName,
                            PackUomId: d.PackUomId, PackUom: d.PackUom, PackUomEquivalent: d.PackUomEquivalent, ItemQty: d.ItemQty, Remarks: d.RemarksDetail !== undefined ? d.RemarksDetail : d.Remarks };
                    });
                    bindGrids();
                    var variety = dtGrid.length ? String(dtGrid[0].ItemName || '') : '';
                    if (!variety) variety = String(ci(h, 'VarietyName') || '');
                    selByText('cmbItem', variety);
                    var sum = 0; dtGrid.forEach(function (r) { sum += num(r.ItemQty); });
                    var pk = Math.trunc(sum);
                    $id('txtqty').value = pk === 0 ? String(ci(h, 'NoOfPackages')) : String(pk);
                    var wh = dtGrid.length ? intOf(dtGrid[0].WareHouseId) : 0;
                    $id('CmbWarehouse').value = String(wh === 0 ? intOf(ci(h, 'WarehouseId')) : wh);
                    att = { existing: res.attachments || [], files: [], removed: [] };
                    $id('txtFactoryWeight').value = res.wbFound ? netStr(res.wbNetWeight) : '0';
                    factoryWeight();
                    $id('txtManualNo').focus();
                });
            });
        }).catch(alertErr);
    }
    function reset() {                                                  // Reset:2086
        att = { existing: [], files: [], removed: [] };
        ID = 0; updateMode = false;
        show('btnUpdate', false); show('btnSave', true);
        gpTypeId = intOf($id('CmbGatePassType').value);
        $id('txtPartyName').value = ''; $id('txtManualNo').value = '';
        $id('cmbvehicletype').value = '0'; $id('txtvehicleno').value = '';
        $id('txtFactoryWeight').value = '0'; $id('txtremarks').value = '';
        openRows = []; renderOpen();
        dtGrid = [];
        resetDetail(); bindGrids();
        $id('txtgpdate').focus();
        return C.getJson(api + '/next-no').then(function (n) { if (intOf(n.nextNo) > 0) $id('txtgpno').value = n.nextNo; })
            .then(openFill)
            .then(gpTypeLeave)
            .catch(alertErr);
    }
    function newDoc() { reset(); }
    function factoryWeight() {                                          // FactoryWeight:507
        var non = selText('CmbWeighBridge') === 'Non-Weighable';
        show('txtFactoryWeight', !non); show('lblFactoryWt', !non);
    }

    /* ------------------------------------------------------------------ open list (Formgrdfill) and history */

    function histFilter(status) {
        var dt = document.querySelector('input[name="histDate"]:checked');
        return { status: status, dateMode: dt ? dt.value : 'doc', useFrom: $id('chkFromDate').checked, useTo: $id('chkToDate').checked,
            fromDate: $id('FromDateHistory').value, toDate: $id('ToDateHistory').value,
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value), partyId: intOf($id('cmbPartyHistory').value) };
    }
    function openFill() {
        return C.getJson(api + '/history' + C.qs(histFilter('Open'))).then(function (rows) { openRows = rows || []; renderOpen(); });
    }
    function cellText(r, c) {
        var v = r[c[0]];
        if (v === null || v === undefined) return '';
        if (c[2] === 'd') return esc(C.gridDate(v));
        if (c[2] === 't') return esc(C.gridDateTime(v, true));
        if (c[2] === 'n') return esc(num(v).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        return esc(v);
    }
    function listHtml(rows, kind) {
        var sums = { ItemQty: 0, FactoryWeight: 0, PartyWeight: 0 };
        var head = '<tr>' + (kind === 'open' ? '<th style="width:44px"></th>' : '<th style="width:44px"></th><th style="width:50px"></th>') +
            HCOLS.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        var body = rows.map(function (r, i) {
            Object.keys(sums).forEach(function (k) { sums[k] += num(r[k]); });
            var h = '<tr data-i="' + i + '" ondblclick="GpInward.openRow(\'' + kind + '\',' + i + ')" onclick="GpInward.pick(\'' + kind + '\',' + i + ')">';
            if (kind === 'open') h += '<td><button type="button" class="dz-mini" onclick="event.stopPropagation();GpInward.printId(' + intOf(r.Id) + ')">View</button></td>';
            else h += '<td><button type="button" class="dz-mini" onclick="event.stopPropagation();GpInward.openRow(\'hist\',' + i + ')">Edit</button></td>' +
                '<td><button type="button" class="dz-mini" onclick="event.stopPropagation();GpInward.printId(' + intOf(r.Id) + ')">Print</button></td>';
            HCOLS.forEach(function (c) {
                if (c[2] === 'a') h += '<td style="text-align:right">' + (intOf(r[c[0]]) > 0 ? '<a href="#" onclick="event.stopPropagation();GpInward.showAtt(' + intOf(r.Id) + ');return false;">' + intOf(r[c[0]]) + '</a>' : esc(r[c[0]] === null || r[c[0]] === undefined ? '' : r[c[0]])) + '</td>';
                else h += '<td' + (c[2] === 'n' ? ' style="text-align:right"' : '') + '>' + cellText(r, c) + '</td>';
            });
            return h + '</tr>';
        }).join('');
        var foot = rows.length ? '<tr class="dz-total">' + (kind === 'open' ? '<td></td>' : '<td></td><td></td>') + HCOLS.map(function (c) {
            return '<td style="text-align:right;font-weight:bold">' + (sums[c[0]] !== undefined ? esc(sums[c[0]].toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })) : '') + '</td>';
        }).join('') + '</tr>' : '';
        return { head: rows.length ? head : '', body: body + foot };
    }
    function renderOpen() {
        var t = $id('grdOpenGp'), o = listHtml(openRows, 'open');
        t.tHead.innerHTML = o.head; t.tBodies[0].innerHTML = o.body;
    }
    function showHistory() {                                            // btnshow_Click -> Historygrdfill:1699
        C.getJson(api + '/history' + C.qs(histFilter(''))).then(function (rows) {
            histRows = rows || []; histSel = -1;
            var t = $id('grdhistory'), o = listHtml(histRows, 'hist');
            t.tHead.innerHTML = o.head; t.tBodies[0].innerHTML = o.body;
            $id('grdHistoryDetail').tHead.innerHTML = ''; $id('grdHistoryDetail').tBodies[0].innerHTML = '';
            if (histRows.length) pick('hist', 0);
        }).catch(alertErr);
    }
    function pick(kind, i) {
        if (kind !== 'hist') return;
        histSel = i;
        document.querySelectorAll('#grdhistory tbody tr').forEach(function (x) { x.classList.toggle('is-selected', x.getAttribute('data-i') === String(i)); });
        var r = histRows[i]; if (!r) return;
        C.getJson(api + '/' + intOf(r.Id) + '/details').then(function (rows) {            // GetDetailGrdByHeadId:2052
            var t = $id('grdHistoryDetail');
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr>' + HDCOLS.map(function (c) { return '<th>' + esc(GCAP[c] || c) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (d) {
                return '<tr>' + HDCOLS.map(function (c) { var v = d[c]; return '<td' + (c === 'ItemQty' ? ' style="text-align:right"' : '') + '>' + esc(v === null || v === undefined ? '' : v) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(alertErr);
    }
    function openRow(kind, i) {
        var r = (kind === 'open' ? openRows : histRows)[i];
        if (r) readById(intOf(r.Id));
    }
    function newHistory() {                                             // btnNewHistory_Click:1865
        var d = new Date(); d.setDate(d.getDate() - 3);
        $id('FromDateHistory').value = d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
        $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        ['grdhistory', 'grdHistoryDetail'].forEach(function (id) { $id(id).tHead.innerHTML = ''; $id(id).tBodies[0].innerHTML = ''; });
        $id('rdpqtydoc').checked = true;
    }
    function refreshHistory() {                                         // btnRefreshHistory_Click:1881
        C.getJson(api + '/history-parties').then(function (rows) { look.historyParties = rows; historyPartyBind(); }).catch(alertErr);
    }
    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
        if (id === 'tabForm') openFill().catch(alertErr);                // tabControl1_SelectedIndexChanged:1922
    }

    /* ------------------------------------------------------------------ print / attachments / keys */

    function printId(id) {                                              // CommonServices.GatePassGeneralInward254(id, 52)
        if (!id) { alert('No Record Found For Display'); return; }
        if (typeof window.printRpt === 'function') window.printRpt('254-RptInwardGatePassSlipGeneral.rpt', { id: id, documentTypeId: 52 });
    }
    function attRender(readOnly, gpId) {
        var rows = readOnly ? readOnly : att.existing;
        var h = '<table class="win-grid"><thead><tr><th>Attachment</th><th style="width:150px"></th></tr></thead><tbody>';
        (rows || []).forEach(function (r) {
            var gone = att.removed.indexOf(intOf(r.Id)) >= 0;
            h += '<tr><td' + (gone && !readOnly ? ' style="text-decoration:line-through"' : '') + '>' + esc(r.Attachment) + '</td><td>' +
                '<a href="' + api + '/' + (gpId || ID) + '/attachments/' + intOf(r.Id) + '" target="_blank">Open</a>' +
                (readOnly ? '' : ' <button type="button" class="dz-mini" onclick="GpInward.attRemove(' + intOf(r.Id) + ')">' + (gone ? 'Undo' : 'Remove') + '</button>') + '</td></tr>';
        });
        if (!readOnly) att.files.forEach(function (f, i) {
            h += '<tr><td>' + esc(f.name) + ' (new)</td><td><button type="button" class="dz-mini" onclick="GpInward.attDrop(' + i + ')">Remove</button></td></tr>';
        });
        $id('attBody').innerHTML = h + '</tbody></table>';
        show('attPick', !readOnly);
    }
    function openAtt() { attRender(null); C.openModal('attModal'); }                         // btnAttachment_Click_1 -> AT.Show()
    function showAtt(id) {                                                                     // grdhistory_LinkClicked
        C.getJson(api + '/' + id + '/attachments').then(function (rows) { attRender(rows, id); C.openModal('attModal'); }).catch(alertErr);
    }
    function attRemove(id) { var k = att.removed.indexOf(id); if (k >= 0) att.removed.splice(k, 1); else att.removed.push(id); attRender(null); }
    function attDrop(i) { att.files.splice(i, 1); attRender(null); }
    function attPick(input) {
        var files = Array.prototype.slice.call(input.files || []);
        if (att.files.length + files.length > 10) { alert('Select at most ten files at once'); input.value = ''; return; }
        files.forEach(function (f) {
            var fr = new FileReader();
            fr.onload = function () { att.files.push({ name: f.name, base64: String(fr.result).split(',')[1] || '' }); attRender(null); };
            fr.readAsDataURL(f);
        });
        input.value = '';
    }
    function shortcuts() {                                                                    // MakeShortCutKeys:2394
        $id('keysBody').innerHTML = KEYS.map(function (k) { return '<tr><td>' + esc(k[0]) + '</td><td>' + esc(k[1]) + '</td></tr>'; }).join('');
        C.openModal('keysModal');
    }
    function decimalKey(e, box) {                                                             // OnlytextdecimelFunction
        if (e.key.length !== 1) return true;
        if (e.key === '.') return box.value.indexOf('.') < 0;
        return /\d/.test(e.key);
    }
    function digitKey(e) { return e.key.length !== 1 || /\d/.test(e.key); }
    function onTab1() { return $id('tabForm').classList.contains('is-active'); }
    document.addEventListener('keydown', function (e) {                                       // InwardGatePass_KeyDown:2298
        var k = e.key, form = onTab1();
        if (e.ctrlKey && e.altKey) { shortcuts(); return; }
        if (k === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not([disabled]):not([readonly]),select:not([disabled]),button.dz-btn'),
                function (x) { return x.offsetParent !== null; });
            var at = f.indexOf(e.target);
            if (at >= 0 && at < f.length - 1) { e.preventDefault(); f[at + 1].focus(); }
        }
        if (!e.ctrlKey) return;
        var key = k.toLowerCase();
        if (key === 's') { e.preventDefault(); if (form) { if (!$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) save(); } else showHistory(); }
        else if (key === 'n') { e.preventDefault(); if (form) newDoc(); else newHistory(); }
        else if (key === 't') { e.preventDefault(); if (!form) { tab('tabForm'); $id('CmbGatePassType').focus(); } else { tab('tabHistory'); $id('FromDateHistory').focus(); } }
        else if (key === 'u') { e.preventDefault(); if (form && !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) update(); }
        else if (key === 'p') { e.preventDefault(); if (form && !$id('btnPrint').disabled) printId(ID); }
        else if (key === 'r' && !form) { e.preventDefault(); refreshHistory(); }
        else if (k === 'F5') { e.preventDefault(); $id('CmbGatePassType').focus(); }
        else if (k === 'ArrowUp') { e.preventDefault(); (form ? $id('CmbGatePassType') : $id('FromDateHistory')).focus(); }
        else if (k === 'ArrowDown') { e.preventDefault(); (form ? $id('grdOpenGp') : $id('grdhistory')).focus(); }
        else if (k === 'Enter' && !form && histSel >= 0) { openRow('hist', histSel); }
    });

    window.GpInward = {
        newDoc: newDoc, refresh: refreshAll, save: save, update: update, print: function () { printId(ID); }, printId: printId, shortcuts: shortcuts,
        openAtt: openAtt, showAtt: showAtt, attRemove: attRemove, attDrop: attDrop, attPick: attPick, closeAtt: function () { C.closeModal('attModal'); },
        gpTypeLeave: gpTypeLeave, refDocLeave: refDocLeave, itemLeave: itemLeave, searchModeChanged: itemNameBind, factoryWeight: factoryWeight,
        add: add, updateDetail: updateDetail, resetDetail: resetDetail, edit: edit, del: del, cell: cell, updateSame: updateSame,
        decimalKey: decimalKey, digitKey: digitKey, tab: tab, showHistory: showHistory, newHistory: newHistory, refreshHistory: refreshHistory,
        pick: pick, openRow: openRow, closeKeys: function () { C.closeModal('keysModal'); }
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('txtgpdate').value = C.today();
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        init().then(function () { resetDetail(); }).catch(alertErr);
    });
})();
