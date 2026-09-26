/* ============================================================================================
 * Screen 322 "Store Issuance" — Architecture.WinApp.StoreManagement.frmGSIssuance, DocumentTypeId 451.
 *
 * The grid is never typed into from scratch: it is filled by one of two loaders and the operator
 * then adjusts Issue Qty, Item Condition, Warehouse/Rack and Remarks.
 *
 *   btnLoadRequest_Click:844       Delivery Order (PM) loader — refused once Department Request rows are in
 *   BtnLoadDepRequest_Click:956    Department Request loader (DocumentTypeId 450) — the reverse
 *   LoadInGridDetail*:868 / :981   a detail id already in the grid is skipped; warehouse/rack resolved
 *   UpdateBalanceStock:1072        QtyInHand per row WITH warehouse and rack
 *   txtDocdate_ValueChanged:1491   QtyInHand WITH warehouse (no rack) + AvgRate, every row
 *   grdDetail_CellUpdated:695      Issue Qty > BalanceStock warning; condition change re-reads stock/rate
 *   grdDetail_KeyDown:1731         F1 on Warehouse / Rack — pick from the item's racks
 *   Insert():296                   every save-time rule is enforced again on the server
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/issuance/issuance';

    var look = { rights: {}, itemConditions: [], racks: [] };
    var recId = 0;
    var loaded = false;              // first Load done — chkPrint is set from the Print right once
    var requestType = '';            // "", "DeliveryOrder", "DepartmentRequest"
    var rows = [];                   // dtgrddetail
    var removed = [];                // lstRemoveRecord
    var doRows = [], deptRows = [];  // the two loaders' current result sets

    /* DetailGridSettings:747 — visible columns per request type, in DataTable order. */
    function columns() {
        var dept = requestType !== 'DeliveryOrder';
        var c = [];
        if (dept) c.push({ k: 'DepartmentRequestNo', h: 'Department Request No' });
        if (!dept) c.push({ k: 'DoNo', h: 'Do No' }, { k: 'SupplierName', h: 'Supplier Name' });
        c.push({ k: 'ItemCode', h: 'Item Code' }, { k: 'ItemName', h: 'Item' },
               { k: 'WarehouseName', h: 'Warehouse Name', pick: 'wh' }, { k: 'RackName', h: 'Rack Name', pick: 'rack' },
               { k: 'ItemConditionId', h: 'Item Condition', combo: true });
        if (!dept) c.push({ k: 'BagType', h: 'Bag Type' });
        c.push({ k: 'Unit', h: 'Unit' }, { k: 'IssueQty', h: 'Issue Qty', edit: 'num' },
               { k: 'RowBalanceQty', h: 'Request / Do Balance Qty', n: true },
               { k: 'BalanceStock', h: 'Balance Stock', n: true });
        if (dept) c.push({ k: 'Asset', h: 'Asset' });
        c.push({ k: 'Remarks', h: 'Remarks', edit: 'text' });
        return c;
    }

    function render() {
        var cols = columns();
        var t = $id('grdDetail');
        t.tHead.innerHTML = '<tr><th></th>' + cols.map(function (c) { return '<th>' + esc(c.h) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td><button type="button" class="cx-x" onclick="GsIssuance.del(' + i + ')">X</button></td>' +
                cols.map(function (c) {
                    var v = r[c.k];
                    if (c.combo) {
                        return '<td><select class="win-combo" onchange="GsIssuance.cond(' + i + ',this.value)">' +
                            '<option value="0"></option>' +
                            look.itemConditions.map(function (o) {
                                return '<option value="' + o.Id + '"' + (intOf(v) === o.Id ? ' selected' : '') + '>' + esc(o.Description) + '</option>';
                            }).join('') + '</select></td>';
                    }
                    if (c.edit === 'num') return '<td><input class="num" value="' + esc(v) + '" onchange="GsIssuance.qty(' + i + ',this.value)"></td>';
                    if (c.edit === 'text') return '<td><input value="' + esc(v) + '" onchange="GsIssuance.rem(' + i + ',this.value)"></td>';
                    if (c.pick) return '<td class="pick" tabindex="0" onclick="GsIssuance.pick(' + i + ',\'' + c.pick + '\')" ' +
                        'onkeydown="if(event.key===\'F1\'){event.preventDefault();GsIssuance.pick(' + i + ',\'' + c.pick + '\')}">' + esc(v || '…') + '</td>';
                    return '<td' + (c.n ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('lblRequestType').textContent = requestType ? requestType : '';
    }

    /* ---------------------------------------------------------------------- load */

    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l || look;
            var r = look.rights || {};
            $id('btnSave').disabled = !r.save;
            $id('btnUpdate').disabled = !r.update;
            $id('btnPrint').disabled = !r.print;
            if (!loaded) $id('chkPrint').checked = !!r.print;                    // Load:198 only
            loaded = true;
            $id('chkPrint').disabled = !r.print;
            if (!r.view) {
                $id('rightsNote').textContent = 'You do not have the View right for Store Issuance.';
                $id('rightsNote').classList.remove('is-hidden');
            }
            if (!recId) { $id('txtDocNo').value = look.docNo || ''; $id('txtBranchSrNo').value = look.branchSrNo || ''; }
            render();
        }).catch(function (e) { alert(e.message); });
    }

    function reset() {                                                          // formReset:625
        recId = 0; requestType = ''; rows = []; removed = [];
        $id('txtRemarks').value = '';
        $id('btnSave').classList.remove('is-hidden');
        $id('btnUpdate').classList.add('is-hidden');
        $id('btnDelete').classList.add('is-hidden');
        $id('lblRecId').textContent = '';
        /* formReset:625 never touches txtDocdate — the last date carries over. */
        render();
        return init();
    }

    /* ---------------------------------------------------------------------- stock */

    function stock(r, withRack, withWarehouse) {
        return C.getJson(api + '/stock' + C.qs({
            recId: recId, itemId: r.ItemId, docDate: $id('txtDocdate').value, itemConditionId: r.ItemConditionId,
            warehouseId: withWarehouse ? r.WarehouseId : 0, rackId: withRack ? r.RackId : 0
        }));
    }

    function updateBalanceStock() {                                             // :1072
        return Promise.all(rows.map(function (r) {
            r.BalanceStock = 0;
            return stock(r, true, true).then(function (s) { r.BalanceStock = s.qtyInHand; });
        })).then(render);
    }

    function docDateChanged() {                                                 // :1491
        Promise.all(rows.map(function (r) {
            return stock(r, false, true).then(function (s) { r.BalanceStock = s.qtyInHand; r.ItemRate = s.avgRate; });
        })).then(render).catch(function (e) { alert(e.message); });
    }

    /* ---------------------------------------------------------------------- grid edits */

    function qty(i, v) {
        var r = rows[i]; r.IssueQty = num(v);
        if (requestType !== 'DeliveryOrder' && r.IssueQty > num(r.BalanceStock)) {   // :702
            alert('IssueQty greater than BalanceQty Please Check');
        }
    }
    function rem(i, v) { rows[i].Remarks = v; }
    function cond(i, v) {                                                       // :711
        var r = rows[i]; r.ItemConditionId = intOf(v); r.BalanceStock = 0; r.ItemRate = 0;
        stock(r, true, true).then(function (s) {
            r.BalanceStock = s.qtyInHand;
            return stock(r, false, false);
        }).then(function (s) { r.ItemRate = s.avgRate; render(); }).catch(function (e) { alert(e.message); });
    }

    function pick(i, what) {                                                    // grdDetail_KeyDown:1731
        var r = rows[i];
        var itemRacks = look.racks.filter(function (x) { return x.ItemId === r.ItemId; });
        if (what === 'wh') {
            var whs = C.distinct(itemRacks, 'WarehouseId').map(function (w) { return { Id: w.WarehouseId, WareHouseName: w.WareHouseName }; });
            C.pickFrom('Warehouse', whs, [{ key: 'WareHouseName', caption: 'WareHouse Name' }]).then(function (w) {
                /* GrdPopUp returns 0 / "" when closed without a pick, and the form writes that back. */
                var selId = w ? w.Id : 0;
                r.WarehouseId = selId; r.WarehouseName = w ? w.WareHouseName : '';
                var racks = itemRacks.filter(function (x) { return x.WarehouseId === selId; });
                if (intOf(r.RackId) > 0) {
                    if (!racks.some(function (x) { return x.Id === r.RackId; })) {
                        if (racks.length === 1) { r.RackId = racks[0].Id; r.RackName = racks[0].RackName; }
                        else { r.RackId = 0; r.RackName = ''; }
                    }
                } else if (racks.length === 1) { r.RackId = racks[0].Id; r.RackName = racks[0].RackName; }
                getStock(r);
            });
        } else {
            var wid = intOf(r.WarehouseId);
            var list = C.distinct(itemRacks.filter(function (x) { return wid === 0 || x.WarehouseId === wid; }), 'Id');
            if (!list.length) return;
            C.pickFrom('Rack', list, [{ key: 'RackName', caption: 'Rack Name' }, { key: 'WareHouseName', caption: 'Warehouse Name' }]).then(function (k) {
                r.RackId = k ? k.Id : 0; r.RackName = k ? k.RackName : '';
                if (wid === 0 && k) { r.WarehouseId = k.WarehouseId; r.WarehouseName = k.WareHouseName; }
                getStock(r);
            });
        }
    }
    function getStock(r) {                                                      // GetStock:1868
        r.BalanceStock = 0;
        stock(r, true, true).then(function (s) { r.BalanceStock = s.qtyInHand; render(); })
            .catch(function (e) { alert(e.message); render(); });
    }

    function del(i) {                                                           // DeleteDetailRow:808
        var r = rows[i];
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            removed.push(r);
        }
        rows.splice(i, 1);
        render();
    }

    /* ---------------------------------------------------------------------- DO loader */

    function openDoLoader() {                                                   // :844
        if (!(requestType === '' || requestType === 'DeliveryOrder')) {
            alert('Data Against Department Request Loaded,Please Reset That First If You Want to Load Delivery Order');
            return;
        }
        /* :851 a new PendingDoPmForIssuance each click — every filter starts fresh. */
        $id('doFrom').value = C.isoDay(look.financialYearStart) || C.today();
        $id('doTo').value = C.today();
        $id('doNoFrom').value = ''; $id('doNoTo').value = '';
        C.openModal('dlgDo');
        searchDo();
    }
    function searchDo() {
        C.getJson(api + '/pending-delivery-orders' + C.qs({
            fromDate: $id('doFrom').value, toDate: $id('doTo').value,
            docNoFrom: intOf($id('doNoFrom').value), docNoTo: intOf($id('doNoTo').value)
        })).then(function (list) {
            doRows = list || [];
            var t = $id('grdDo');
            t.tHead.innerHTML = '<tr><th>Select</th><th>Doc Date</th><th>Doc No</th><th>Bag Type</th><th>Item Name</th>' +
                '<th>Uom</th><th>Item Condition</th><th>Item Qty</th><th>Issue Qty</th><th>Bal Qty</th><th>Party Name</th></tr>';
            t.tBodies[0].innerHTML = doRows.map(function (r, i) {
                var bag = intOf(ci(r, 'BagTypeId')) === 1 ? 'Normal' : 'Retain';
                return '<tr><td><input type="checkbox" data-i="' + i + '"></td><td>' + esc(C.gridDate(ci(r, 'DocDate'))) + '</td><td>' +
                    esc(ci(r, 'DocNo')) + '</td><td>' + bag + '</td><td>' + esc(ci(r, 'ItemName')) + '</td><td>' + esc(ci(r, 'UOMCode')) +
                    '</td><td>' + esc(ci(r, 'ItemCondition')) + '</td><td class="num">' + esc(ci(r, 'ItemQty')) + '</td><td class="num">' +
                    esc(ci(r, 'IssuedQty')) + '</td><td class="num">' + esc(ci(r, 'BalQty')) + '</td><td>' + esc(ci(r, 'CompanyName')) + '</td></tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function checked(tableId, source) {
        var out = [];
        document.querySelectorAll('#' + tableId + ' tbody input[type=checkbox]:checked').forEach(function (c) {
            out.push(source[parseInt(c.getAttribute('data-i'), 10)]);
        });
        return out;
    }
    function loadDo() {                                                         // LoadInGridDetailDoPM:868
        var picked = checked('grdDo', doRows);
        if (!picked.length) { alert('Check the row first'); return; }
        requestType = 'DeliveryOrder';
        var existing = {};
        rows.forEach(function (r) { existing[intOf(r.DoDetailId)] = 1; });
        picked.forEach(function (row) {
            var detailId = intOf(ci(row, 'DetailId'));
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var r = {
                Id: 0, DepartmentRequestId: 0, DepartmentRequestDetailId: 0, DepartmentRequestNo: 0,
                DoDocumentTypeId: intOf(ci(row, 'DocumentTypeId')), DoId: intOf(ci(row, 'Id')), DoDetailId: detailId,
                DoNo: intOf(ci(row, 'DocNo')), SupplierCustomerId: intOf(ci(row, 'SupplierCustomerId')),
                SupplierName: ci(row, 'CompanyName'), ItemId: intOf(ci(row, 'ItemId')), ItemCode: ci(row, 'ItemCode'),
                ItemName: ci(row, 'ItemName'), ItemConditionId: intOf(ci(row, 'ItemConditionId')),
                BagTypeId: intOf(ci(row, 'BagTypeId')), BagType: ci(row, 'BagType'),
                UnitId: intOf(ci(row, 'PackUOMId')), Unit: ci(row, 'UOMCode'), Equivalent: num(ci(row, 'Equivalent')),
                ItemRate: 0, IssueQty: num(ci(row, 'BalQty')), RowBalanceQty: num(ci(row, 'BalQty')),
                BalanceStock: num(ci(row, 'BalQty')), DepartmentId: 0, Department: '', AssetId: 0, Asset: '',
                WarehouseId: 0, WarehouseName: '', RackId: 0, RackName: '', Remarks: ci(row, 'LoadingRemarks')
            };
            place(r, ci(row, 'ParentCategoryId'));
            rows.push(r);
        });
        C.closeModal('dlgDo');
        if (!rows.length) requestType = '';
        updateBalanceStock().then(docDateChanged);
    }

    function place(r, parentCategoryId) {
        var itemRacks = look.racks.filter(function (x) { return x.ItemId === r.ItemId; });
        var res = C.resolveWarehouseAndRack(itemRacks, C.configWarehouse(parentCategoryId, look));
        if (res.WarehouseId > 0) { r.WarehouseId = res.WarehouseId; r.WarehouseName = res.WarehouseName; }
        if (res.RackId > 0) { r.RackId = res.RackId; r.RackName = res.RackName; }
    }

    /* ---------------------------------------------------------------------- Dept loader */

    var deptLooked = false;
    function openDeptLoader() {                                                 // :956
        if (!(requestType === '' || requestType === 'DepartmentRequest')) {
            alert('Data Against Delivery Order Loaded,Please Reset That First If You Want to Load Department Request');
            return;
        }
        var p = deptLooked ? Promise.resolve() : C.getJson(api + '/department-request-lookups').then(function (l) {
            C.fillSelect('cmbDepartmentFrom', l.DepartmentFrom, 'Id', 'Name');
            C.fillSelect('cmbDepartmentTo', l.DepartmentTo, 'Id', 'Name');
            C.fillSelect('cmbDepItem', l.Item, 'Id', 'Name');
            deptLooked = true;
        });
        p.then(function () {
            /* :963 a new LoadDepRequestToConsumableStore each click — pickers and doc nos empty.
               Load: Datetypefill activates "This Month", then FromDate = ActiveYr.Start_Period. */
            $id('cmbDepartmentFrom').value = '0'; $id('cmbDepartmentTo').value = '0'; $id('cmbDepItem').value = '0';
            $id('depNoFrom').value = ''; $id('depNoTo').value = '';
            $id('depTo').value = C.today();
            $id('depPeremeter').value = '3';
            deptPeremeter();
            $id('depFrom').value = C.isoDay(look.financialYearStart) || $id('depFrom').value;
            C.openModal('dlgDept');
            searchDept();
        }).catch(function (e) { alert(e.message); });
    }
    function deptPeremeter() {                                                  // cmbperemeter_ValueChanged
        var v = $id('depPeremeter').value, d = new Date(), f;
        function iso(x) { return x.getFullYear() + '-' + String(x.getMonth() + 1).padStart(2, '0') + '-' + String(x.getDate()).padStart(2, '0'); }
        if (v === '1') { $id('depFrom').value = C.today(); }
        else if (v === '2') { f = new Date(d); f.setDate(f.getDate() - 7); $id('depFrom').value = iso(f); }
        else if (v === '3') { $id('depFrom').value = iso(new Date(d.getFullYear(), d.getMonth(), 1)); $id('depTo').value = C.today(); }
        else if (v === '4') { $id('depFrom').value = d.getFullYear() + '-01-01'; $id('depTo').value = C.today(); }
        else if (v === '5') { $id('depFrom').value = C.isoDay(look.financialYearStart); }
        if (!$id('depTo').value) $id('depTo').value = C.today();
    }
    function searchDept() {
        C.getJson(api + '/pending-department-requests' + C.qs({
            fromDate: $id('depFrom').value, toDate: $id('depTo').value,
            docNoFrom: intOf($id('depNoFrom').value), docNoTo: intOf($id('depNoTo').value),
            itemId: intOf($id('cmbDepItem').value), departmentFromId: intOf($id('cmbDepartmentFrom').value),
            departmentToId: intOf($id('cmbDepartmentTo').value)
        })).then(function (list) {
            deptRows = list || [];
            var t = $id('grdDept');
            t.tHead.innerHTML = '<tr><th>Select</th><th>Doc Date</th><th>Doc No</th><th>From Department</th><th>To Department</th>' +
                '<th>Item Name</th><th>Item Code</th><th>UOM</th><th>Asset Name</th><th>Requested Qty</th><th>Issued Qty</th><th>Qty Bal</th><th>Qty In Stock</th></tr>';
            t.tBodies[0].innerHTML = deptRows.map(function (r, i) {
                return '<tr><td><input type="checkbox" data-i="' + i + '"></td><td>' + esc(C.gridDate(ci(r, 'DocDate'))) + '</td><td>' +
                    esc(ci(r, 'DocNo')) + '</td><td>' + esc(ci(r, 'FromDepartment')) + '</td><td>' + esc(ci(r, 'ToDepartment')) + '</td><td>' +
                    esc(ci(r, 'ItemName')) + '</td><td>' + esc(ci(r, 'ItemCode')) + '</td><td>' + esc(ci(r, 'UOMCode')) + '</td><td>' +
                    esc(ci(r, 'AssetName')) + '</td><td class="num">' + esc(ci(r, 'RequestedQty')) + '</td><td class="num">' +
                    esc(ci(r, 'IssuedQty')) + '</td><td class="num">' + esc(ci(r, 'BalQty')) + '</td><td class="num">' + esc(ci(r, 'StockQtyBal')) + '</td></tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function closeDept() {                                                      // :1061
        C.closeModal('dlgDept');
        if (!rows.length) { requestType = ''; reset(); }
    }
    function loadDept() {                                                       // btnLoadOnInvoice + :981
        var picked = checked('grdDept', deptRows);
        if (!picked.length) { alert('Check the row first'); return; }
        var docId = null;
        for (var k = 0; k < picked.length; k++) {                               // DocumentTypeId != 1615
            var cur = intOf(ci(picked[k], 'Id'));
            if (cur !== 0) {
                if (docId === null) docId = cur;
                else if (docId !== cur) { alert('Sorry! The selected invoices are not of the same Doc'); return; }
            }
        }
        requestType = 'DepartmentRequest';
        var existing = {};
        rows.forEach(function (r) { existing[intOf(r.DepartmentRequestDetailId)] = 1; });
        picked.forEach(function (row) {
            var detailId = intOf(ci(row, 'DetailId'));
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var r = {
                Id: 0, DepartmentRequestId: intOf(ci(row, 'Id')), DepartmentRequestDetailId: detailId,
                DepartmentRequestNo: intOf(ci(row, 'DocNo')), DoDocumentTypeId: 0, DoId: 0, DoDetailId: 0, DoNo: 0,
                SupplierCustomerId: 0, SupplierName: '', ItemId: intOf(ci(row, 'ItemId')), ItemCode: ci(row, 'ItemCode'),
                ItemName: ci(row, 'ItemName'), ItemConditionId: intOf(ci(row, 'ItemConditionId')), BagTypeId: 0, BagType: '',
                UnitId: intOf(ci(row, 'ItemUOMId')), Unit: ci(row, 'UOMCode'), Equivalent: num(ci(row, 'Equivalent')),
                ItemRate: 0, IssueQty: num(ci(row, 'BalQty')), RowBalanceQty: num(ci(row, 'BalQty')),
                BalanceStock: num(ci(row, 'StockQtyBal')), DepartmentId: intOf(ci(row, 'FromDepartmentId')),
                Department: ci(row, 'FromDepartment'), AssetId: intOf(ci(row, 'AssetId')), Asset: ci(row, 'AssetName'),
                Remarks: '', WarehouseId: 0, WarehouseName: '', RackId: 0, RackName: ''
            };
            place(r, ci(row, 'ParentCategoryId'));
            rows.push(r);
        });
        C.closeModal('dlgDept');
        if (!rows.length) { requestType = ''; }
        updateBalanceStock().then(docDateChanged);
    }

    /* ---------------------------------------------------------------------- save */

    function payload() {
        return {
            Id: recId, DocNo: intOf($id('txtDocNo').value), BranchSrNo: intOf($id('txtBranchSrNo').value),
            DocDate: $id('txtDocdate').value, Remarks: $id('txtRemarks').value,
            rows: rows, removed: removed
        };
    }
    function save() { return C.withBusy('btnSave', function () { recId = 0; return insert(); }); }
    function update() {
        if (!recId) { alert('Record id not found...'); return Promise.resolve(); }
        return C.withBusy('btnUpdate', function () { return insert(); });
    }
    function insert() {
        if (!rows.length) { alert('Grid record not found'); return Promise.resolve(); }
        var d = $id('txtDocNo').value.trim();
        if (d === '' || d === '0') { alert('document Number Field Required'); $id('txtDocNo').focus(); return Promise.resolve(); }
        if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
        var wasNew = recId === 0, reqT = requestType, shouldPrint = $id('chkPrint').checked;
        return C.postJson(api + '/save', payload()).then(function (res) {
            alert(res.message);
            var code = res.id;
            reset();
            if ($id('chkStoreReturnLink').checked && wasNew && reqT !== 'DeliveryOrder') {
                /* :410 — open Store Return with this issuance loaded; the slip prints when it closes. */
                window.open('/store/store-return?issuanceId=' + code + (shouldPrint ? '&printIssuance=' + code : ''), '_blank');
            } else if (shouldPrint) {
                printId(code);
            }
        }).catch(function (e) { alert(e.message); });
    }
    function remove() {                                                         // btnDelete_Click:578
        if (!(recId > 0)) { alert('Record Not Found'); return Promise.resolve(); }
        if (!confirm('Are you sure to Delete?')) return Promise.resolve();
        return C.withBusy('btnDelete', function () {
            return C.postJson(api + '/' + recId + '/delete', {}).then(function (res) { alert(res.message); reset(); })
                .catch(function (e) { alert(e.message); });
        });
    }
    function printId(id) {
        if (!id) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/' + id + '/slip', 'Store Issuance Slip');
    }

    /* ---------------------------------------------------------------------- open / history */

    function readById(id) {                                                     // ReadById:511
        C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id; requestType = h.RequestType; removed = [];
            $id('txtDocdate').value = C.isoDay(h.DocDate);
            $id('txtDocNo').value = h.DocNo; $id('txtBranchSrNo').value = h.BranchSrNo;
            $id('txtRemarks').value = h.Remarks || '';
            rows = (h.rows || []).map(function (r) { r.Unit = r.Unit; return r; });
            $id('btnSave').classList.add('is-hidden');
            $id('btnUpdate').classList.remove('is-hidden');
            $id('btnDelete').classList.toggle('is-hidden', !(look.rights || {}).delete);
            $id('lblRecId').textContent = 'Record #' + recId;
            tab('tabForm');
            render();
            docDateChanged();                                                   // :541
        }).catch(function (e) { alert(e.message); });
    }

    function newHistory() {
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('FromDocNoHistory').value = ''; $id('ToDocNoHistory').value = '';
        $id('DataGridHistory').tBodies[0].innerHTML = ''; $id('grddetailHistory').tBodies[0].innerHTML = '';
    }
    function showHistory(btn) {                                                 // HistoryFill:1197
        return C.withBusy(btn || document.querySelector('#tabHistory .win-btn-action') || $id('btnFooterHistory'), function () {
            return C.getJson(api + '/history' + C.qs({
                fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
                toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
                fromDocNo: intOf($id('FromDocNoHistory').value), toDocNo: intOf($id('ToDocNoHistory').value)
            })).then(function (list) {
                var r = look.rights || {};
                var t = $id('DataGridHistory');
                t.tHead.innerHTML = '<tr>' + (r.update ? '<th>Edit</th>' : '') + (r.print ? '<th>Print</th>' : '') +
                    '<th>Doc No</th><th>Doc Date</th><th>Issuance Type</th><th>Remarks</th><th>Entry Date</th><th>Entry User</th>' +
                    '<th>Modify Date</th><th>Modify User</th><th>No Of Attachments</th><th>Branch Sr No</th></tr>';
                t.tBodies[0].innerHTML = (list || []).map(function (h) {
                    return '<tr data-id="' + h.Id + '" data-type="' + esc(h.IssuanceType) + '" onclick="GsIssuance.historyDetail(this)">' +
                        (r.update ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();GsIssuance.open(' + h.Id + ')">Edit</button></td>' : '') +
                        (r.print ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();GsIssuance.printId(' + h.Id + ')">Print</button></td>' : '') +
                        '<td><a class="cx-link" href="javascript:void(0)" onclick="event.stopPropagation();GsIssuance.open(' + h.Id + ')">' + esc(h.DocNo) + '</a></td><td>' + esc(C.gridDate(h.DocDate)) + '</td><td>' + esc(h.IssuanceType) + '</td><td>' +
                        esc(h.Remarks) + '</td><td>' + esc(C.gridDateTime(h.EntryDate)) + '</td><td>' + esc(h.EntryUser) + '</td><td>' +
                        esc(C.gridDateTime(h.ModifyDate)) + '</td><td>' + esc(h.ModifyUser) + '</td><td class="num">' + esc(h.NoOfAttachments) +
                        '</td><td class="num">' + esc(h.BranchSrNo) + '</td></tr>';
                }).join('');
                $id('grddetailHistory').tBodies[0].innerHTML = '';
            }).catch(function (e) { alert(e.message); });
        });
    }

    function historyDetail(tr) {                                                // SelectionChanged:1357
        document.querySelectorAll('#DataGridHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        var type = tr.getAttribute('data-type');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var dept = type !== 'DeliveryOrder';
            var cols = [];
            if (dept) cols.push(['DepartmentRequestNo', 'Department Request No']); else cols.push(['DoNo', 'Do No'], ['SupplierName', 'Supplier Name']);
            cols.push(['ItemCode', 'Item Code'], ['ItemName', 'Item'], ['WarehouseName', 'Warehouse Name'], ['RackName', 'Rack Name'], ['ItemCondition', 'Item Condition']);
            if (!dept) cols.push(['BagType', 'Bag Type']);
            cols.push(['Unit', 'Unit'], ['IssueQty', 'Issue Qty']);
            if (dept) cols.push(['Asset', 'Asset']);
            cols.push(['Remarks', 'Remarks']);
            var t = $id('grddetailHistory');
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (h.rows || []).map(function (r) {
                return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }

    /* PendingDoPmForIssuance / LoadDepRequestToConsumableStore btnReset_Click (also their toolbar New):
       FromDate.Focus(); grd.DataSource = null. */
    function doReset() { $id('grdDo').tBodies[0].innerHTML = ''; doRows = []; $id('doFrom').focus(); }
    function deptReset() { $id('grdDept').tBodies[0].innerHTML = ''; deptRows = []; $id('depFrom').focus(); }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    function refresh() {                                                        // btnRefresh_Click:606
        C.getJson(api + '/lookups').then(function (l) { look.itemConditions = l.itemConditions; look.racks = l.racks; render(); })
            .catch(function (e) { alert(e.message); });
    }

    document.addEventListener('keydown', function (e) {                        // frmGSIssuance_KeyDown:1533
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase(), onForm = $id('tabForm').classList.contains('is-active');
        if (k === 's') { e.preventDefault(); if (onForm) { if ($id('btnUpdate').classList.contains('is-hidden')) save(); } else showHistory(); }
        else if (k === 'n') { e.preventDefault(); if (onForm) reset(); else newHistory(); }
        else if (k === 't') { e.preventDefault(); tab(onForm ? 'tabHistory' : 'tabForm'); }
        else if (k === 'u') { e.preventDefault(); if (!$id('btnUpdate').classList.contains('is-hidden')) update(); }
        else if (k === 'p') { e.preventDefault(); if ((look.rights || {}).print) printId(recId); }
        else if (k === 'l') { e.preventDefault(); if (onForm) { if (requestType === 'DeliveryOrder') openDoLoader(); else openDeptLoader(); } }
    });

    window.GsIssuance = {
        reset: reset, save: save, update: update, remove: remove, refresh: refresh, tab: tab,
        print: function () { printId(recId); }, printId: printId, open: readById,
        openDoLoader: openDoLoader, searchDo: searchDo, loadDo: loadDo, doReset: doReset, deptReset: deptReset,
        openDeptLoader: openDeptLoader, deptPeremeter: deptPeremeter, searchDept: searchDept, loadDept: loadDept, closeDept: closeDept,
        qty: qty, rem: rem, cond: cond, pick: pick, del: del, docDateChanged: docDateChanged,
        showHistory: showHistory, newHistory: newHistory, historyDetail: historyDetail
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('txtDocdate').value = C.today();                                    // designer: DateTime.Now, once
        reset();
    });
})();
