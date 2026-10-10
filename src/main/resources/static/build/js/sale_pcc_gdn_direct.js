/*
 * Screen 552  GdnDirectConcrete  (Architecture.WinApp.pcc.Sale.GdnDirectConcrete, document type 1866)
 * Page script. Desktop methods are named in the comments (GdnDirectConcrete.cs). Server: /sale/pcc/gdn-direct-concrete/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/pcc/gdn-direct-concrete/api', PRINT = '/sale/pcc/print/gdn-direct-slip';
    var S = {
        id: 0, approved: false, rights: {}, remove: [], depth: 0, files: [], removedAtt: [], existing: [], editIndex: -1, tok: 0,
        fmt: { amountRound: 0, amount: 0, rate: 2, fcy: 0, rateRound: 2 }, def: {}, search: {}, custs: [], contractors: [], items: [], wh: [], job: [], city: [],
        uoms: [], hCusts: [], hist: [], dateTypes: [], fyStart: ''
    };
    var cb = {}, G = {}, tabs, tabs2;

    function fail(e) { return SE.dbError(e); }
    function msg(t, c) { return SE.alert(t, c); }
    function field(id) { return ($(id).value || '').trim(); }
    function vid(c) { return +c.value() || 0; }
    function n(v) { return SE.toNum(v); }
    function num(id) { return SE.toNum($(id).value); }
    function nameOr(v) { return v == null ? '' : String(v); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function setEnabled(id, on) { $(id).disabled = !on; }
    /* WinForms TextChanged: fires only when the text really changes */
    function setv(id, t) {
        var e = $(id); t = t == null ? '' : String(t);
        if (e.value !== t) { e.value = t; e.dispatchEvent(new Event('input', { bubbles: true })); }
    }
    /* case-insensitive column read (DataRow["name"]) */
    function g(row, name) {
        if (!row) return undefined;
        if (row[name] !== undefined) return row[name];
        var l = String(name).toLowerCase();
        for (var k in row) if (String(k).toLowerCase() === l) return row[k];
        return undefined;
    }
    function gi(row, name) { return Math.trunc(n(g(row, name))) || 0; }
    function toBool(v) { return v === true || v === 1 || /^(true|1|yes)$/i.test(String(v)); }          // Conversion.ToBool
    /* every desktop handler wraps its body in try / catch (MessageBox.Show(ex.Message)); a depth guard stands in for WinForms' re-entrancy */
    function guard(fn) {
        return function () {
            if (S.depth > 30) return;
            S.depth++;
            try { return fn.apply(this, arguments); } catch (x) { msg(x.message); } finally { S.depth--; }
        };
    }
    function stop(m, f) { return SE.alert(m).then(function () { if (f) f(); return false; }); }
    function fmtAmt(v) { var d = S.fmt.amount; return SE.num(v, d, d); }                       // stringFormatsingle
    function f2z(v) { return v === 0 || v === '' || v == null ? '' : SE.num(n(v), 2, 0); }       // "##,#.##" (zero prints nothing)
    function f2(v) { return SE.num(n(v), 2, 0); }                                              // "#,##0.##"
    function keep(c, rows) { var id = vid(c); c.setData(rows || []); if (id > 0 && !c.setValue(id)) c.clear(); }
    function custRows(list, byName) { return (list || []).map(function (r) { return Object.assign({ _t: byName ? r.CompanyName : r.PartyCode }, r); }); }
    function itemRows(list, byName) { return (list || []).map(function (r) { return Object.assign({ _t: byName ? r.ItemName : r.ItemCode }, r); }); }
    function partyByName() { return $('RdPartyByName').checked; }
    function itemByName() { return $('rdbtnItemName').checked; }
    function histByName() { return $('RdCustomerHistorySearchByName').checked; }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('comsupplier', { columns: [{ key: '_t', caption: 'Party Name' }], textKey: '_t', popupWidth: 420 });
        cb.trans = XCombo('CmbTransporterAc', { columns: [{ key: 'AccountTitle', caption: 'Account Title' }], textKey: 'AccountTitle', popupWidth: 380 });
        cb.dterm = XCombo('combdeliverytrm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm', onSelect: guard(termChanged) });
        cb.vtype = XCombo('CmbVehicleType', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Type' }], textKey: 'VehicleDescription' });
        cb.wh = XCombo('comWarehouse', { columns: [{ key: 'WareHouseName', caption: 'Warehouse' }], textKey: 'WareHouseName', popupWidth: 280, onSelect: guard(stockLabel), onLeave: guard(stockLabel) });
        cb.item = XCombo('comItem', { columns: [{ key: '_t', caption: 'Item Name' }], textKey: '_t', popupWidth: 420, onSelect: guard(itemLeave), onLeave: guard(itemLeave) });
        cb.pack = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], valueKey: 'Id', textKey: 'UOMCode', popupWidth: 200, onSelect: guard(packChanged) });
        cb.varient = XCombo('CmbAttributeVarient', { columns: [{ key: 'ItemAttribute', caption: 'Varient' }], valueKey: 'ItemAttributeVarientId', textKey: 'ItemAttribute', popupWidth: 260,
            onSelect: guard(varientLeave), onLeave: guard(varientLeave) });
        cb.job = XCombo('comjobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', popupWidth: 300, onSelect: guard(stockLabel), onLeave: guard(stockLabel) });
        cb.city = XCombo('txtCity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName', popupWidth: 300 });
        cb.dateType = XCombo('cmbDateTypeHistory', { columns: [{ key: 'Parameters', caption: 'Parameters' }], textKey: 'Parameters', onSelect: dateTypeChanged });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: '_t', caption: 'Party Name' }], textKey: '_t', popupWidth: 380 });
        cb.actW = XCombo('CmbActivityforContractorUpdateInWagesGrid', { columns: [{ key: 'ServiceActivity', caption: 'ServiceActivity' }], valueKey: 'ContractorWagesRateScheduleId', textKey: 'ServiceActivity', popupWidth: 260 });
        cb.conW = XCombo('CmbContractorForUpdateInWagesGrid', { columns: [{ key: 'CompanyName', caption: 'ContractorName' }], textKey: 'CompanyName', popupWidth: 300 });
        cb.dterm.setData([{ Id: 1, DeliveryTerm: 'Factory Loading' }, { Id: 2, DeliveryTerm: 'Delivery' }]);
    }
    /* combdeliverytrm_TextChanged: clears the transporter and sets the carriage amount to "0" (the "Ponch" branch is dead on the desktop) */
    function termChanged() { cb.trans.clear(); setv('txtFreightAmountHeader', '0'); }
    function dateTypeChanged() { SPC.setRangeFromDateType(vid(cb.dateType), $('txtFromdateHistory'), $('txtToDateHistory'), S.fyStart); }
    function fillCustomers() { keep(cb.cust, custRows(S.custs, partyByName())); }                      // suppliercustomer
    function fillItems() { keep(cb.item, itemRows(S.items, itemByName())); }                           // item
    function applyLists(d) {
        if (d.customers) S.custs = d.customers;
        S.contractors = d.contractors || S.contractors;
        S.wh = d.warehouses || S.wh; S.job = d.jobLots || S.job; S.city = d.cities || S.city;
        fillCustomers();
        SPC.fillDatalist('CmbVehicleNo', d.vehicles || [], 'VehicleNo');                       // BindVehicles
        keep(cb.trans, d.transporters); keep(cb.vtype, d.vehicleTypes);
        keep(cb.wh, S.wh); keep(cb.job, S.job); keep(cb.city, S.city);
    }
    /* ConfigurationDefault: Warehouse (always), City Area / Job/Lot (only while empty) */
    function applyDefaults() {
        var d = S.def || {};
        if (+d.warehouseId > 0) cb.wh.setValue(+d.warehouseId);
        if (+d.cityId > 0 && vid(cb.city) === 0) cb.city.setValue(+d.cityId);
        if (+d.jobLotId > 0 && vid(cb.job) === 0) cb.job.setValue(+d.jobLotId);
    }
    function loadItems() {
        return SE.api(API + '/items' + SE.q({ docDate: $('DocDate').value })).then(function (r) { S.items = r || []; fillItems(); }).catch(function () { S.items = []; fillItems(); });
    }

    /* ------------------------------------------------------------------ grids */
    function blankWage() {
        return { Id: 0, OrderId: 0, OrderWagesId: 0, ItemId: 0, ItemName: '', AttributeVarientId: 0, AttributeVarient: '', VarientEquivalent: 0, ContractorId: 0, ContractorName: '',
            ContractorWagesRateScheduleId: 0, ServiceActivity: '', ParentUomId: 0, Qty: 0, ItemNetWeight: 0, Rate: 0, Amount: 0, AddLess: 0, NetAmount: 0, Remarks: '' };
    }
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, frozen: 2, dec: 0,
            onBtn: function (k, r, i) { guard(function () { if (k === 'Delete') deleteRow(i); else if (k === 'Edit') editRow(i); })(); },
            onEdit: function (r, k, v) { guard(gridEdit)(r, k, v); },
            onDbl: function (r) { guard(function () { editRow(G.grd.curIndex()); })(); },
            cols: [
                { k: 'Delete', t: 'X', w: 28, btn: 'X' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' },
                { k: 'Id', hide: true }, { k: 'WarehouseId', hide: true }, { k: 'Warehouse', t: 'Warehouse', w: 130 }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 },
                { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ScheduleId', hide: true }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'Uom', w: 70 }, { k: 'PackUomEquivalent', hide: true },
                { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 110 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: f2, sum: true, cls: 'num' },
                { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 110 },
                { k: 'ItemQty', t: 'ItemQty', w: 90, edit: true, render: f2, sum: true, cls: 'num' },
                { k: 'AvailableQty', t: 'AvailableQty', w: 100, render: f2, sum: true, cls: 'num' },
                { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: f2, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: f2, sum: true, cls: 'num' },
                { k: 'BalWeight', t: 'BalWeight', w: 90, render: f2, sum: true, cls: 'num' },
                { k: 'CityId', hide: true }, { k: 'CityName', t: 'CityName', w: 110 },
                { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk', render: function (v) { return '<input type="checkbox" data-foc' + (v ? ' checked' : '') + '>'; } },
                { k: 'RemarksDetail', t: 'RemarksDetail', w: 160, edit: true }] });
        $('grd').addEventListener('keydown', function (e) { guard(gridKey)(e); });
        $('grd').addEventListener('click', function (e) {                                       // grd_Click: IsFOC is a check-box column
            if (e.target && e.target.hasAttribute && e.target.hasAttribute('data-foc')) {
                var tr = e.target.closest('tr[data-i]'), r = tr && G.grd.rows()[+tr.getAttribute('data-i')];
                if (r) { r.IsFOC = e.target.checked; G.grd.refresh(); }
            }
        });
        G.focMain = SPC.focRows($('grd'), G.grd);

        G.wg = SE.grid('grdContractorWages', { footer: true, frozen: 3, dec: 0,
            onBtn: function (k, r, i) { guard(function () { wageButton(k, r, i); })(); },
            onEdit: function (r, k, v) { guard(wageEdit)(r, k, v); },
            cols: [
                { k: 'DeleteAll', t: 'DeleteAll', w: 80, btn: 'DeleteAll' }, { k: 'Delete', t: 'X', w: 40, btn: 'X' }, { k: 'Add', t: '+', w: 40, btn: '+' },
                { k: 'Id', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderWagesId', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 170 },
                { k: 'AttributeVarientId', hide: true }, { k: 'AttributeVarient', t: 'Varient', w: 100 }, { k: 'VarientEquivalent', t: 'Unit', w: 60, render: f2z, sum: true, cls: 'num' },
                { k: 'ContractorId', hide: true }, { k: 'ContractorName', t: 'ContractorName', w: 170 }, { k: 'ContractorWagesRateScheduleId', hide: true },
                { k: 'ServiceActivity', t: 'ServiceActivity', w: 150 }, { k: 'ParentUomId', hide: true },
                { k: 'Qty', t: 'Qty', w: 80, edit: true, render: f2z, sum: true, cls: 'num' }, { k: 'ItemNetWeight', t: 'Weight', w: 80, render: f2z, sum: true, cls: 'num' },
                { k: 'Rate', t: 'Rate', w: 80, render: function (v) { return n(v) === 0 ? '' : SE.num(n(v), 3, 0); }, cls: 'num' },
                { k: 'Amount', t: 'Amount', w: 100, f: 'amt', sum: true, cls: 'num' }, { k: 'AddLess', t: 'AddLess', w: 80, edit: true, f: 'amt', sum: true, cls: 'num' },
                { k: 'NetAmount', t: 'NetAmount', w: 100, f: 'amt', sum: true, cls: 'num' }, { k: 'Remarks', t: 'Remarks', w: 130, edit: true }] });
        G.wg.setRows([blankWage()]);
        $('grdContractorWages').addEventListener('keydown', function (e) { guard(wageKey)(e); });

        G.hist = SE.grid('grdHistory', { frozen: 3, dec: 0, onDbl: function (r) { guard(readById)(+r.Id); },
            onBtn: function (k, r) { if (k === 'Edit') histRead(r); else if (k === 'Print') print(+r.Id); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 90, link: true },
                { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'SupplierCustomerId', hide: true },
                { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'PartyName', t: 'PartyName', w: 170 }, { k: 'PartyCode', t: 'PartyCode', w: 100 },
                { k: 'ReferenceNo', t: 'ReferenceNo', w: 100 }, { k: 'Distance', t: 'Distance', w: 70 },
                { k: 'ReferencPartyName', t: 'ReferencPartyName', w: 140 }, { k: 'ReferencPartyAddress', t: 'ReferencPartyAddress', w: 180 }, { k: 'ReferencPartyCellNo', t: 'ReferencPartyCellNo', w: 110 },
                { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 }, { k: 'TransporterName', t: 'TransporterName', w: 140 }, { k: 'CarriageAmount', t: 'CarriageAmount', w: 100, f: 'amt', sum: true, cls: 'num' },
                { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'GpDate', t: 'GpDate', w: 90, f: 'sdate' }, { k: 'VehicleType', t: 'VehicleType', w: 110 }, { k: 'VehicleNo', t: 'VehicleNo', w: 100 },
                { k: 'DriverName', t: 'DriverName', w: 110 }, { k: 'DriverCNICNo', t: 'DriverCNICNo', w: 120 }, { k: 'DriverCellNo', t: 'DriverCellNo', w: 110 },
                { k: 'IsApproved', t: 'IsApproved', w: 70, f: 'chk' }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 100 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 100 }, { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 }] });
        $('grdHistory').addEventListener('keydown', function (e) { guard(histKey)(e); });
        G.hd = SE.grid('grdDetailHistory', { footer: true, dec: 0, cols: [
            { k: 'Id', hide: true }, { k: 'ItemId', hide: true },
            { k: 'Warehouse', t: 'Warehouse', w: 130 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'Uom', t: 'Uom', w: 70 },
            { k: 'AttributeVarient', t: 'AttributeVarient', w: 110 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: f2, sum: true, cls: 'num' }, { k: 'JobLot', t: 'JobLot', w: 110 },
            { k: 'ItemQty', t: 'ItemQty', w: 90, render: f2, sum: true, cls: 'num' }, { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: f2, sum: true, cls: 'num' },
            { k: 'NetWeight', t: 'NetWeight', w: 90, render: f2, sum: true, cls: 'num' }, { k: 'CityName', t: 'CityName', w: 110 },
            { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }, { k: 'RemarksDetail', t: 'RemarksDetail', w: 160 }] });
        G.focHd = SPC.focRows($('grdDetailHistory'), G.hd);
    }

    /* ------------------------------------------------------------------ entry group (groupBox1) */
    /* Conversion.ToString(double): no thousands separators, no trailing zeros */
    function plain(v) { return v == null || v === '' ? '' : String(n(v)); }
    function withVariant(row) { return toBool(g(row, 'WithVariant')); }
    /* AvailableStockGetByItem / AvailableStockGetForLabel / comWarehouse.Leave / comjobLot.Leave -> lblBalance */
    function stockLabel() {
        var item = vid(cb.item), lbl = $('lblBalance');
        if (!(item > 0)) { lbl.textContent = '0'; return Promise.resolve(); }
        return SE.api(API + '/stock' + SE.q({ itemId: item, warehouseId: vid(cb.wh), jobLotId: vid(cb.job), varientId: vid(cb.varient), docDate: $('DocDate').value })).then(function (r) {
            lbl.textContent = String(n(r && r.qty));
        }).catch(fail);
    }
    /* bindPackUom: the previous pack code is kept when the new item has it */
    function bindPackUom(itemId) {
        var prev = cb.pack.text(), tok = ++S.tok;
        cb.pack.clear();
        if (!(itemId > 0)) { S.uoms = []; cb.pack.setData([]); return Promise.resolve(); }
        return SE.api(API + '/pack-uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            if (tok !== S.tok) return;
            S.uoms = rows || []; cb.pack.setData(S.uoms);
            var hit = S.uoms.filter(function (x) { return String(g(x, 'UOMCode')) === prev; })[0];
            if (S.uoms.length > 0 && hit) cb.pack.setValue(+hit.Id); else cb.pack.clear();
            if (S.uoms.length === 0) cb.pack.setData([]);
        }).catch(fail);
    }
    /* bindvarientunit (ItemAttributeVarient.GetAllForCombo): the previous varient text is kept when the new item has it */
    function bindVarientUnit(itemId) {
        var prev = cb.varient.text();
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            rows = rows || [];
            if (rows.length > 0) {
                cb.varient.setData(rows);
                if (prev !== '') {
                    var hit = rows.filter(function (x) { return String(g(x, 'ItemAttribute')) === prev; })[0];
                    if (hit) cb.varient.setValue(gi(hit, 'ItemAttributeVarientId')); else cb.varient.clear();
                } else cb.varient.clear();
            } else { cb.varient.clear(); setv('txtVarientEquivalent', ''); cb.varient.setData([]); }
        }).catch(fail);
    }
    /* comItem_Leave */
    function itemLeave() {
        var id = vid(cb.item), before = cb.pack.text();
        var jobs = [bindPackUom(id)];
        if (id > 0) { setv('txtItemWeight', nameOr(g(cb.item.row(), 'ItemWeight'))); jobs.push(bindVarientUnit(id)); }
        else setv('txtItemWeight', '0');
        return Promise.all(jobs).then(function () {
            if (cb.pack.text() !== before) calcWeight();                                   // CmbPackUom_TextChanged
            return stockLabel();
        });
    }
    /* CmbPackUom_TextChanged */
    function packChanged() { calcWeight(); return stockLabel(); }
    /* CmbAttributeVarient_Leave */
    function varientLeave() {
        var eq = vid(cb.varient) > 0 ? n(g(cb.varient.row(), 'VarientEquivalent')) : 0;
        setv('txtVarientEquivalent', String(eq));
        calcWeight();
        return stockLabel();
    }
    /* CalculateWeight (txtQty_TextChanged, CmbPackUom_TextChanged, CmbAttributeVarient_Leave) */
    function calcWeight() {
        var out = '0';
        if (vid(cb.varient) > 0 && $('txtQty').value !== '') {
            var qty = num('txtQty'), wt = num('txtItemWeight'), unit = num('txtVarientEquivalent'), net = 0;
            if (qty > 0 && vid(cb.item) > 0) {
                if (!withVariant(cb.item.row())) { unit = vid(cb.pack) <= 0 ? 0 : n(g(cb.pack.row(), 'Equivalent')); net = unit * qty; }
                else net = unit * qty * wt;
                out = f2z(net);                                                              // "##,#.##"
            }
        }
        setv('txtNetWeight', out);
    }
    /* FormValidationDetail */
    function formValidDetail() {
        if (!cb.cust.row() || vid(cb.cust) === 0) return stop('Supplier Field is Required', function () { cb.cust.focus(); });
        if (!cb.wh.row() || vid(cb.wh) === 0) return stop('Warehouse Field is Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || vid(cb.item) === 0) return stop('Item Name Field is Required', function () { cb.item.focus(); });
        if (!cb.varient.row() || vid(cb.varient) === 0) return stop('Attribute Varient Field is Required', function () { cb.varient.focus(); });
        var q = field('txtQty'); if (q === '' || q === '0') return stop('Qty Field is Required', function () { $('txtQty').focus(); });
        var w = field('txtItemWeight'); if (w === '' || w === '0') return stop('Item Weight Field is Required', function () { $('txtItemWeight').focus(); });
        if (!cb.job.row() || vid(cb.job) === 0) return stop('Job/Lot Field is Required', function () { cb.job.focus(); });
        if (!cb.city.row() || vid(cb.city) === 0) return stop('City Field is Required', function () { cb.city.focus(); });
        return Promise.resolve(true);
    }
    /* the duplicate test of btnAdd_Click / btnUpdateDetail_Click (every grid row is compared, the edited one included, as on the desktop) */
    function isDuplicate() {
        return G.grd.rows().some(function (r) {
            return vid(cb.wh) === (+r.WarehouseId || 0) && vid(cb.item) === (+r.ItemId || 0) && vid(cb.varient) === (+r.AttributeVarientId || 0) && vid(cb.job) === (+r.JobLotId || 0) &&
                vid(cb.city) === (+r.CityId || 0) && vid(cb.pack) === (+r.PackUomId || 0) && $('chkisFreeOfCost').checked === !!r.IsFOC;
        });
    }
    function entryValues() {                                                                 // the cell values shared by Add and Update
        var it = cb.item.row(), pk = cb.pack.row();
        if (!pk) throw new Error('Object reference not set to an instance of an object.');     // CmbPackUom.SelectedRow is null
        var wv = withVariant(it);
        return { WarehouseId: vid(cb.wh), Warehouse: cb.wh.text(), ItemId: vid(cb.item), ItemName: nameOr(g(it, 'ItemName')), ItemCode: nameOr(g(it, 'ItemCode')),
            ScheduleId: nameOr(g(it, 'ScheduleId')), PackUomId: vid(cb.pack), PackUom: nameOr(g(pk, 'UOMCode')), PackUomEquivalent: n(g(pk, 'Equivalent')),
            AttributeVarientId: wv ? vid(cb.varient) : 0, AttributeVarient: wv ? cb.varient.text().trim() : '', VarientUnit: wv ? num('txtVarientEquivalent') : 0,
            JobLotId: vid(cb.job), JobLot: cb.job.text(), ItemQty: num('txtQty'), ItemWeight: num('txtItemWeight'), NetWeight: num('txtNetWeight'),
            CityId: vid(cb.city), CityName: cb.city.text(), IsFOC: $('chkisFreeOfCost').checked, RemarksDetail: $('txtRemarksDetail').value };
    }
    /* btnAdd_Click */
    function btnAdd() {
        return formValidDetail().then(function (ok) {
            if (!ok) return;
            if (isDuplicate()) return msg('Duplicate items are not added to the grid. An item with the same warehouse,Uom, AttributeVarient,job lot and City already exists');
            var v = entryValues();
            v.Id = 0; v.AvailableQty = v.ItemQty; v.BalWeight = v.NetWeight;
            G.grd.addRow(v);
            resetDetail();
        }).catch(function (x) { return msg(x.message); });
    }
    /* btnUpdateDetail_Click (the PackUom cell keeps the pack code; the desktop converts the code text with Conversion.ToDecimal) */
    function btnUpdateDetail() {
        return formValidDetail().then(function (ok) {
            if (!ok) return;
            if (isDuplicate()) return msg('Duplicate items are not added to the grid. An item with the same warehouse,Uom, AttributeVarient,job lot and City already exists');
            var r = G.grd.rows()[S.editIndex];
            if (!r) throw new Error('Index was out of range. Must be non-negative and less than the size of the collection.');
            var v = entryValues();
            ['WarehouseId', 'Warehouse', 'ItemId', 'ItemName', 'ItemCode', 'ScheduleId', 'PackUomId', 'PackUom', 'PackUomEquivalent', 'AttributeVarientId', 'AttributeVarient', 'VarientUnit',
                'JobLotId', 'JobLot', 'ItemQty', 'ItemWeight', 'NetWeight', 'CityId', 'CityName', 'IsFOC', 'RemarksDetail'].forEach(function (k) { r[k] = v[k]; });
            G.grd.refresh();
            show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            resetDetail();
        }).catch(function (x) { return msg(x.message); });
    }
    /* btnCancelUpdateDetial_Click */
    function btnCancelUpdate() { show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); S.editIndex = -1; }
    /* ResetDetail */
    function resetDetail() {
        $('chkisFreeOfCost').checked = false;
        setv('txtQty', ''); setv('txtItemWeight', ''); setv('txtNetWeight', '');
        if ($('ChkResetOnSave').checked) optionResetFields();
        cb.item.focus();
    }
    /* OptionResetFields */
    function optionResetFields() {
        cb.item.clear(); cb.job.clear(); cb.wh.clear(); setv('txtgatepassno', ''); setv('CmbVehicleNo', ''); cb.city.clear();
        cb.item.focus();
    }
    /* grd_DoubleClick: the grid row goes back to the entry group */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return Promise.resolve();
        S.editIndex = i;
        cb.wh.setValue(+r.WarehouseId || 0);
        cb.item.setValue(+r.ItemId || 0);
        return Promise.all([bindVarientUnit(+r.ItemId || 0), bindPackUom(+r.ItemId || 0)]).then(guard(function () {
            if ((+r.PackUomId || 0) > 0) cb.pack.setValue(+r.PackUomId);
            if ((+r.AttributeVarientId || 0) > 0) cb.varient.setValue(+r.AttributeVarientId);
            setv('txtVarientEquivalent', nameOr(r.VarientUnit));
            cb.job.setValue(+r.JobLotId || 0);
            setv('txtQty', f2(r.ItemQty));
            setv('txtItemWeight', f2(r.ItemWeight));
            setv('txtNetWeight', f2(r.NetWeight));
            cb.city.setValue(+r.CityId || 0);
            $('chkisFreeOfCost').checked = !!r.IsFOC;
            show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            cb.item.focus();
        }));
    }

    /* ------------------------------------------------------------------ detail grid (grd) */
    function lineOf(r, removed) {
        return { id: +r.Id || 0, wareHouseId: +r.WarehouseId || 0, itemId: +r.ItemId || 0, scheduleId: removed ? 0 : (Math.trunc(n(r.ScheduleId)) || 0), packUomId: removed ? 0 : (+r.PackUomId || 0),
            attributeVarientId: +r.AttributeVarientId || 0, jobLotId: +r.JobLotId || 0, cityId: +r.CityId || 0,
            wareHouseName: nameOr(r.Warehouse), itemCode: nameOr(r.ItemCode), itemName: nameOr(r.ItemName), packUom: nameOr(r.PackUom), attributeVarient: nameOr(r.AttributeVarient),
            jobLot: nameOr(r.JobLot), cityName: nameOr(r.CityName), remarksDetail: nameOr(r.RemarksDetail), varientUnit: n(r.VarientUnit), itemQty: n(r.ItemQty),
            itemWeight: n(r.ItemWeight), netWeight: n(r.NetWeight), packUomEquivalent: n(r.PackUomEquivalent), isFoc: !!r.IsFOC };
    }
    /* GetStockInHandAndAvgRateFromEvaluationConcrete -> AvailableQty (ItemId > 0 || WareHouseId > 0, else 0) */
    function stockRows(rows) {
        var lines = rows.map(function (r) { return { itemId: +r.ItemId || 0, wareHouseId: +r.WarehouseId || 0, jobLotId: +r.JobLotId || 0, varientId: +r.AttributeVarientId || 0 }; });
        if (!lines.length) return Promise.resolve();
        return SE.api(API + '/stock-lines' + SE.q({ docDate: $('DocDate').value }), { method: 'POST', body: lines }).then(function (q) {
            rows.forEach(function (r, i) { r.AvailableQty = q && q[i] != null ? q[i] : 0; });
            G.grd.refresh();
        }).catch(fail);
    }
    /* grd_CellUpdated (ItemQty: NetWeight from the pack equivalent and the varient unit) */
    function gridEdit(r, k, v) {
        if (k === 'ItemQty') {
            var qty = n(v), unit = n(r.VarientUnit), pk = n(r.PackUomEquivalent), wt = n(r.ItemWeight);
            r.ItemQty = qty;
            if (pk > 0 && unit > 0) r.NetWeight = unit * qty * pk * wt;
            else if (pk > 0 && unit === 0) r.NetWeight = qty * pk * wt;
            else if (pk === 0 && unit > 0) r.NetWeight = qty * unit * wt;
        } else r[k] = v;
        G.grd.refresh();
    }
    /* grd_ColumnButtonClick "Delete" / Ctrl+Space / Ctrl+Delete */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return Promise.resolve();
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push(lineOf(r, true));
                G.grd.removeAt(i);
            });
        }
        G.grd.removeAt(i);
        return Promise.resolve();
    }
    function gridKey(e) {
        var r = G.grd.cur(), i = G.grd.curIndex(), k = G.grd.curKey();
        if (!r || i < 0 || !k) return;
        if (e.ctrlKey && e.code === 'Space') {
            if (k === 'Delete') { e.preventDefault(); deleteRow(i); } else if (k === 'Edit') { e.preventDefault(); editRow(i); }
        }
        else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteRow(i); }
        else if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); editRow(i); }
        else if (e.key === 'F1') { e.preventDefault(); pickGrid(r, k); }
    }
    /* grd_KeyDown F1 */
    function pickGrid(r, k) {
        if (k === 'WarehouseId' || k === 'Warehouse') {
            return pick('Warehouse', S.wh, 'Id', 'WareHouseName').then(function (p) { r.WarehouseId = p.id; r.Warehouse = p.name; return stockRows([r]); });
        }
        if (k === 'AttributeVarientId' || k === 'AttributeVarient') {
            if ((+r.ItemId || 0) <= 0) { msg('Please Select an Item First'); return; }
            return SE.api(API + '/varients' + SE.q({ itemId: +r.ItemId })).then(function (rows) {
                return pick('Varient', rows, 'ItemAttributeVarientId', 'ItemAttribute').then(function (p) {
                    r.AttributeVarientId = p.id; r.AttributeVarient = p.name;
                    var f = rows.filter(function (x) { return +x.ItemAttributeVarientId === p.id; })[0];
                    r.VarientUnit = f ? n(f.VarientEquivalent) : 0;
                    return stockRows([r]);
                });
            }).catch(fail);
        }
        if (k === 'JobLotId' || k === 'JobLot') {
            return pick('Job Lot', S.job, 'Id', 'JobLotDescription').then(function (p) { r.JobLotId = p.id; r.JobLot = p.name; return stockRows([r]); });
        }
        if (k === 'CityName' || k === 'CityId') {
            return pick('City', S.city, 'Id', 'CityName').then(function (p) { r.CityId = p.id; r.CityName = p.name; G.grd.refresh(); });
        }
    }
    /* GrdPopUp(dt, idKey, nameKey): a modal list with a search box; closing without a choice returns 0 / "" */
    function pick(title, rows, idKey, nameKey) {
        return new Promise(function (resolve) {
            var done = false;
            function finish(v) { if (done) return; done = true; resolve(v || { id: 0, name: '' }); }
            var pop = SE.pop(title, '<input class="f" id="wpq" placeholder="Search" style="position:static;width:100%;margin-bottom:4px"><div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>' +
                SE.esc(nameKey) + '</th></tr></thead><tbody id="wpb"></tbody></table></div>', [{ t: 'Close' }]);
            var tb = pop.body.querySelector('#wpb'), q = pop.body.querySelector('#wpq'), sel = 0, shown = [];
            function paint() {
                var t = q.value.toLowerCase();
                shown = rows.filter(function (r) { return String(r[nameKey]).toLowerCase().indexOf(t) >= 0; });
                if (sel >= shown.length) sel = 0;
                tb.innerHTML = shown.map(function (r, i) { return '<tr data-i="' + i + '"' + (i === sel ? ' class="sel"' : '') + '><td>' + SE.esc(r[nameKey]) + '</td></tr>'; }).join('');
            }
            function choose(i) { var r = shown[i]; if (!r) return; finish({ id: +r[idKey] || 0, name: String(r[nameKey]), row: r }); pop.close(); }
            q.addEventListener('input', paint);
            q.addEventListener('keydown', function (e) {
                if (e.key === 'ArrowDown' && sel < shown.length - 1) { sel++; paint(); } else if (e.key === 'ArrowUp' && sel > 0) { sel--; paint(); }
                else if (e.key === 'Enter') { e.preventDefault(); e.stopPropagation(); choose(sel); } else if (e.key === 'Escape') { e.stopPropagation(); pop.close(); }
            });
            tb.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) choose(+tr.getAttribute('data-i')); });
            tb.addEventListener('click', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) { sel = +tr.getAttribute('data-i'); paint(); } });
            pop.open(); paint(); q.focus();
            var mo = new MutationObserver(function () { if (!document.body.contains(pop.el)) { mo.disconnect(); finish(null); } });
            mo.observe(document.body, { childList: true });
        });
    }
    /* ------------------------------------------------------------------ contractor wages */
    /* WagesItemFillFromDetail: dtVarientForWages = one row per item + varient of the detail grid with the summed ItemQty / NetWeight */
    function varientGroups() {
        var out = [];
        G.grd.rows().forEach(function (r) {
            var iid = +r.ItemId || 0, vid_ = +r.AttributeVarientId || 0;
            if (out.some(function (x) { return x.ItemId === iid && x.ItemAttributeId === vid_; })) return;
            var qty = 0, wt = 0;
            G.grd.rows().forEach(function (x) { if ((+x.ItemId || 0) === iid && (+x.AttributeVarientId || 0) === vid_) { qty += n(x.ItemQty); wt += n(x.NetWeight); } });
            out.push({ ItemId: iid, ItemName: nameOr(r.ItemName), ItemAttributeId: vid_, ItemAttribute: nameOr(r.AttributeVarient), VarientEquivalent: n(r.VarientUnit), Qty: qty, NetWeight: wt });
        });
        return out;
    }
    function addWage() { G.wg.addRow(blankWage()); }                                           // AddRowInContractorWagesGrid
    function itemWeightOf(id) { for (var i = 0; i < S.items.length; i++) if (+S.items[i].Id === +id) return n(S.items[i].ItemWeight); return 0; }
    /* amount rules shared by every rate update: ParentUomId 2 = Rate * Qty, 3 = Weight / 1000 * Rate (weight int-truncated), else 0 */
    function applyRate(r, found) {
        var rate = found ? n(g(found, 'Rate')) : 0, uom = found ? gi(found, 'ParentUOMId') : 0;
        var addLess = Math.trunc(n(r.AddLess)) || 0, qty = n(r.Qty), amt = 0;
        r.ContractorWagesRateScheduleId = found ? gi(found, 'ContractorWagesRateScheduleId') : 0;
        r.Rate = rate; r.ParentUomId = uom;
        if (uom === 2) amt = rate * qty;
        else if (uom === 3) amt = (Math.trunc(n(r.ItemNetWeight)) || 0) / 1000 * rate;
        else { r.ParentUomId = 0; r.Rate = 0; amt = 0; }
        r.Amount = amt; r.NetAmount = amt + addLess;
    }
    function activities(itemId, contractorId, cache) {
        var key = itemId + '|' + contractorId;
        if (cache && cache[key]) return cache[key];
        var p = SE.api(API + '/wage-activities' + SE.q({ itemId: itemId, date: $('DocDate').value, contractorId: contractorId }));
        if (cache) cache[key] = p;
        return p;
    }
    function actFound(list, activity) { for (var i = 0; i < list.length; i++) if (String(g(list[i], 'WagesActivity')) === String(activity)) return list[i]; return null; }
    /* GenerateRowsInWagesGrid */
    function generateRows() {
        var groups = varientGroups();
        if (G.grd.count() <= 0 || groups.length <= 0) return Promise.resolve();
        var cache = {}, chain = Promise.resolve();
        groups.forEach(function (d) {
            chain = chain.then(function () { return activities(d.ItemId, 0, cache); }).then(function (acts) {
                acts.forEach(function (a) {
                    var hit = false, rows = G.wg.rows();
                    for (var i = 0; i < rows.length; i++) {
                        var w = rows[i];
                        if ((+w.ItemId || 0) === d.ItemId && (+w.AttributeVarientId || 0) === d.ItemAttributeId && (+w.ContractorWagesRateScheduleId || 0) === gi(a, 'ContractorWagesRateScheduleId')) {
                            w.Qty = d.Qty; w.ItemNetWeight = d.NetWeight;
                            var uom = gi(a, 'ParentUomId'), rate = n(g(a, 'Rate')), amount = uom === 2 ? rate * d.Qty : uom === 3 ? d.NetWeight / 1000 * rate : 0;
                            w.Amount = amount; w.NetAmount = amount + n(w.AddLess);
                            hit = true; break;
                        }
                    }
                    if (!hit) {
                        var u2 = gi(a, 'ParentUomId'), r2 = n(g(a, 'Rate')), am2 = u2 === 2 ? r2 * d.Qty : u2 === 3 ? d.NetWeight / 1000 * r2 : 0;
                        var row = { Id: 0, OrderId: 0, OrderWagesId: 0, ItemId: d.ItemId, ItemName: d.ItemName, AttributeVarientId: d.ItemAttributeId, AttributeVarient: d.ItemAttribute,
                            VarientEquivalent: d.VarientEquivalent, ContractorId: 0, ContractorName: '', ContractorWagesRateScheduleId: gi(a, 'ContractorWagesRateScheduleId'),
                            ServiceActivity: nameOr(g(a, 'WagesActivity')), ParentUomId: u2, Qty: d.Qty, ItemNetWeight: d.NetWeight, Rate: r2, Amount: am2, AddLess: 0, NetAmount: am2, Remarks: '' };
                        G.wg.addRow(row);
                    }
                });
            });
        });
        return chain.then(function () { G.wg.refresh(); });
    }
    /* BtnGenerateWagesRows_Click */
    function btnGenerate() {
        return generateRows().then(fillWagesFilter).catch(function (x) { return msg(x.message); });
    }
    /* grdContractorWages_ColumnButtonClick / KeyDown (Ctrl+Space) */
    function wageButton(k, r, i) {
        if (k === 'Delete') { G.wg.removeAt(i); if (G.wg.count() === 0) addWage(); }
        else if (k === 'DeleteAll') {
            var act = nameOr(r.ServiceActivity);
            return SE.ask('Are you sure to Delete All Rows With Activity ' + act + '?', 'Confirm').then(function (yes) {
                if (!yes) return;
                for (var j = G.wg.count() - 1; j >= 0; j--) if (nameOr(G.wg.rows()[j].ServiceActivity) === act) G.wg.removeAt(j);
                if (G.wg.count() === 0) addWage();
                fillWagesFilter();
            });
        }
        else if (k === 'Add') addWage();
        fillWagesFilter();
    }
    /* grdContractorWages_CellUpdated */
    function wageEdit(r, k, v) {
        if (k === 'Qty') {
            if (String(v).trim() === '') throw new Error('Qty And Rate Should be greater than zero...');
            var qty = n(v); r.Qty = qty;
            var weight = itemWeightOf(r.ItemId) * qty * n(r.VarientEquivalent);
            r.ItemNetWeight = weight;
            var rate = n(r.Rate), addLess = n(r.AddLess), uom = Math.trunc(n(r.ParentUomId)) || 0;
            if (uom === 2) { r.Amount = rate * qty; r.NetAmount = r.Amount + addLess; }
            else if (uom === 3) { r.Amount = weight / 1000 * rate; r.NetAmount = r.Amount + addLess; }
            else { r.Amount = 0; r.NetAmount = addLess; G.wg.refresh(); msg('ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check'); return; }
        } else if (k === 'AddLess') {
            r.AddLess = n(v); r.NetAmount = n(r.Amount) + n(r.AddLess);
        } else r[k] = v;
        G.wg.refresh();
    }
    function wageKey(e) {
        var i = G.wg.curIndex(), r = G.wg.cur(), k = G.wg.curKey();
        if (!r || !k) return;
        if (e.ctrlKey && e.code === 'Space') {
            if (k === 'Add' || k === 'DeleteAll' || k === 'Delete') { e.preventDefault(); guard(function () { wageButton(k, r, i); })(); }
        }
        else if (e.ctrlKey && e.key.toLowerCase() === 'd') { e.preventDefault(); addWage(); }
        else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); G.wg.removeAt(i); if (G.wg.count() === 0) addWage(); }
        else if (e.key === 'F1') { e.preventDefault(); pickWage(r, k); }
    }
    /* grdContractorWages_KeyDown F1 on the contractor cell: the contractor is set, then the rate of the row's activity is looked up for that contractor */
    function pickWage(r, k) {
        if (k === 'ContractorId' || k === 'ContractorName') {
            return pick('Contractor', S.contractors, 'Id', 'CompanyName').then(function (p) {
                r.ContractorId = p.id; r.ContractorName = p.name;
                return activities(gi(r, 'ItemId'), p.id).then(function (dt) {
                    if (dt.length > 0) applyRate(r, actFound(dt, r.ServiceActivity));
                    G.wg.refresh();
                });
            }).catch(function (x) { return msg(x.message); });
        }
        if (k === 'ItemId' || k === 'ItemName') {                                            // dtItemForWages: the items of the detail grid
            var prev = gi(r, 'ItemId'), items = [];
            G.grd.rows().forEach(function (x) { var id = +x.ItemId || 0; if (!items.some(function (y) { return y.Id === id; })) items.push({ Id: id, ItemName: nameOr(x.ItemName), ItemCode: nameOr(x.ItemCode) }); });
            return pick('Item', items, 'Id', 'ItemName').then(function (p) {
                r.ItemId = p.id; r.ItemName = p.name;
                if (p.id !== prev) {
                    r.AttributeVarientId = 0; r.AttributeVarient = ''; r.VarientEquivalent = 0; r.Qty = 0; r.ItemNetWeight = 0; r.ContractorWagesRateScheduleId = 0;
                    r.ServiceActivity = ''; r.ParentUomId = 0; r.Rate = 0; r.Amount = 0;
                }
                G.wg.refresh();
            });
        }
        if (k === 'AttributeVarientId' || k === 'AttributeVarient') {
            var iid = gi(r, 'ItemId');
            if (iid <= 0) { msg('Please Select an Item First'); return; }
            var match = varientGroups().filter(function (x) { return x.ItemId === iid; });
            if (match.length === 0) { msg('Please Select an Item First'); return; }
            return pick('Varient', match, 'ItemAttributeId', 'ItemAttribute').then(function (p) {
                r.AttributeVarientId = p.id; r.AttributeVarient = p.name;
                if (p.id > 0) {
                    var f = match.filter(function (x) { return x.ItemAttributeId === p.id; })[0];
                    var qty = f ? f.Qty : 0, wt = f ? f.NetWeight : 0;
                    r.Qty = qty; r.ItemNetWeight = wt; r.VarientEquivalent = f ? f.VarientEquivalent : 0;
                    var rate2 = Math.trunc(n(r.Rate)) || 0, addLess = Math.trunc(n(r.AddLess)) || 0, uom = Math.trunc(n(r.ParentUomId)) || 0;
                    if (uom === 2) r.Amount = rate2 * qty;
                    else if (uom === 3) r.Amount = wt / 1000 * rate2;
                    else { r.Amount = 0; r.NetAmount = addLess; }
                } else { r.Qty = 0; r.ItemNetWeight = 0; r.Amount = 0; r.NetAmount = 0; }
                G.wg.refresh();
            });
        }
        if (k === 'ServiceActivity' || k === 'ContractorWagesRateScheduleId') {
            if (gi(r, 'ItemId') <= 0) { msg('Please Select an Item First'); return; }
            return activities(gi(r, 'ItemId'), gi(r, 'ContractorId')).then(function (dt2) {
                return pick('Service Activity', dt2.map(function (x) { return { ContractorWagesRateScheduleId: gi(x, 'ContractorWagesRateScheduleId'), WagesActivity: nameOr(g(x, 'WagesActivity')), _row: x }; }), 'ContractorWagesRateScheduleId', 'WagesActivity').then(function (p) {
                    r.ContractorWagesRateScheduleId = p.id; r.ServiceActivity = p.name;
                    if (p.id > 0) {
                        var f = p.row ? p.row._row : null;
                        var rate = f ? n(g(f, 'Rate')) : 0, uom = f ? gi(f, 'ParentUOMId') : 0, qty = n(r.Qty), addLess = Math.trunc(n(r.AddLess)) || 0;
                        r.Rate = rate; r.ParentUomId = uom;
                        if (uom === 2) { r.Amount = rate * qty; r.NetAmount = r.Amount + addLess; }
                        else if (uom === 3) { r.Amount = (Math.trunc(n(r.ItemNetWeight)) || 0) / 1000 * rate; r.NetAmount = r.Amount + addLess; }
                        else {
                            r.ContractorWagesRateScheduleId = 0; r.ServiceActivity = ''; r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; r.NetAmount = addLess;
                            G.wg.refresh(); msg('ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check'); return;
                        }
                    } else { r.ContractorWagesRateScheduleId = 0; r.ServiceActivity = ''; r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; r.NetAmount = 0; }
                    G.wg.refresh();
                });
            }).catch(function (x) { return msg(x.message); });
        }
    }
    /* BtnUpdateConctratorInWages_Click (4 modes) */
    function updateContractor() {
        var rows = G.wg.rows();
        if (rows.length <= 0) throw new Error('Wages Grid Record not Found');
        var hasC = vid(cb.conW) !== 0, hasA = vid(cb.actW) !== 0, contrId, contrName, filter = null;
        if (!hasC && !hasA) {
            if (gi(rows[0], 'ContractorId') === 0) throw new Error('ContractorName Field is Empty in Wages Grid Row No: 01');
            contrId = gi(rows[0], 'ContractorId'); contrName = nameOr(rows[0].ContractorName);
        } else if (hasC && !hasA) {
            contrId = vid(cb.conW); contrName = cb.conW.text();
        } else if (!hasC && hasA) {
            cb.conW.focus(); throw new Error('ContractorName Field is Empty');
        } else {
            contrId = vid(cb.conW); contrName = cb.conW.text(); filter = cb.actW.text();
        }
        var cache = {}, chain = Promise.resolve();
        rows.slice().forEach(function (r) {
            if (filter !== null && nameOr(r.ServiceActivity) !== filter) return;
            r.ContractorId = contrId; r.ContractorName = contrName;
            chain = chain.then(function () { return activities(gi(r, 'ItemId'), contrId, cache); }).then(function (dt) {
                if (dt.length > 0) applyRate(r, actFound(dt, r.ServiceActivity));
            });
        });
        return chain.then(function () { G.wg.refresh(); });
    }
    function btnUpdateContractor() {
        var p;
        try { p = updateContractor(); } catch (x) { return msg(x.message); }
        return p.catch(function (x) { return msg(x.message); });
    }
    /* UpdateWagesRateForAllRows (DocDate_ValueChanged) */
    function updateRatesAll() {
        var rows = G.wg.rows();
        if (rows.length === 0) return Promise.resolve();
        var cache = {}, chain = Promise.resolve();
        rows.forEach(function (r) {
            var iid = gi(r, 'ItemId');
            if (iid <= 0) return;
            chain = chain.then(function () { return activities(iid, gi(r, 'ContractorId'), cache); }).then(function (dt) {
                if (dt.length > 0) applyRate(r, actFound(dt, r.ServiceActivity));
                else { r.ParentUomId = 0; r.Rate = 0; r.Amount = 0; r.NetAmount = 0 + (Math.trunc(n(r.AddLess)) || 0); }
            });
        });
        return chain.then(function () { G.wg.refresh(); }).catch(function (x) { return msg(x.message); });
    }
    /* FillWagesGridFilterCombo (the desktop restores the contractor by the activity Id and the activity by the contractor text; reproduced) */
    function fillWagesFilter() {
        var id1 = vid(cb.actW), id2 = cb.conW.text();
        var rows = G.wg.rows();
        if (rows.length > 0) {
            var seen = {}, acts = [];
            rows.forEach(function (r) { var a = nameOr(r.ServiceActivity); if (!seen[a]) { seen[a] = 1; acts.push(r); } });
            if (S.contractors.length > 0) {
                cb.conW.setData(S.contractors);
                if (id1 > 0) { if (S.contractors.some(function (x) { return +x.Id === id1; })) cb.conW.setValue(id1); else cb.conW.clear(); } else cb.conW.clear();
            } else { cb.conW.clear(); cb.conW.setData([]); }
            if (acts.length > 0) {
                cb.actW.setData(acts.map(function (r) { return { ContractorWagesRateScheduleId: r.ContractorWagesRateScheduleId, ServiceActivity: nameOr(r.ServiceActivity) }; }));
                if (id2) { var hit = acts.filter(function (r) { return nameOr(r.ServiceActivity) === id2; })[0]; if (hit) cb.actW.setValue(+hit.ContractorWagesRateScheduleId || 0); else cb.actW.clear(); } else cb.actW.clear();
            } else { cb.actW.clear(); cb.actW.setData([]); }
        } else { cb.conW.clear(); cb.conW.setData([]); cb.actW.clear(); cb.actW.setData([]); }
    }
    function wageRowsOf(list) {
        return (list || []).map(function (w) {
            return { Id: w.Id, OrderId: w.SaleOrderId, OrderWagesId: w.SaleOrderWagesId, ItemId: w.ItemId, ItemName: w.ItemName, AttributeVarientId: w.ItemAttributeVarientId, AttributeVarient: w.ItemAttributeVarient,
                VarientEquivalent: n(w.VarientEquivalent), ContractorId: w.ContractorId, ContractorName: w.ContractorName, ContractorWagesRateScheduleId: w.ContractorWagesRateScheduleId,
                ServiceActivity: nameOr(w.ServiceActivity), ParentUomId: w.ParentUomId, Qty: n(w.Qty), ItemNetWeight: n(w.ItemNetWeight), Rate: n(w.Rate), Amount: n(w.Amount), AddLess: n(w.AddLess),
                NetAmount: n(w.NetAmount), Remarks: nameOr(w.Remarks) };
        });
    }

    /* ------------------------------------------------------------------ Reset / Refresh */
    function docNoFill() { return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail); }
    /* Reset */
    function reset() {
        guard(function () {
            S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.approved = false; S.id = 0;
            $('chkisFreeOfCost').checked = false;
            cb.trans.clear();
            ['txtManualNo', 'txtSupplierReference', 'TxtDistance', 'txtremarks'].forEach(function (id) { setv(id, ''); });
            setv('txtFreightAmountHeader', '');
            setv('txtRefPartyName', ''); setv('txtRefPartyAddress', ''); setv('txtRefPartyCellNo', ''); setv('txtgatepassno', ''); setv('CmbVehicleNo', '');
            var had = vid(cb.dterm) > 0; cb.dterm.clear(); if (had) termChanged();
            setv('txtDriverName', ''); setv('txtDriverCellNo', ''); setv('txtCNICNO', '');
            $('txtgpdate').value = SE.today();
            cb.cust.clear(); cb.cust.el.disabled = false;
            G.wg.setRows([blankWage()]);
            cb.item.clear(); setv('txtQty', '');
            $('DocDate').focus();
            G.grd.setRows([]);
            if ($('ChkResetOnSave').checked) optionResetFields();
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
            show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            applyDefaults();
        })();
        return docNoFill().then(fillWagesFilter);
    }
    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/lists').then(function (d) { applyLists(d); if (d.defaults) S.def = d.defaults; return loadItems(); }).then(function () { applyDefaults(); fillWagesFilter(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ ReadById_Update */
    function detailRows(lines) {
        return (lines || []).map(function (l) {
            return { Id: l.Id, WarehouseId: gi(l, 'WarehouseId'), Warehouse: nameOr(g(l, 'WareHouseName')), ItemId: l.ItemId, ItemName: l.ItemName, ItemCode: l.ItemCode, ScheduleId: nameOr(g(l, 'ScheduleId')),
                PackUomId: gi(l, 'ItemUomId'), PackUom: nameOr(g(l, 'PackUom')), PackUomEquivalent: n(g(l, 'PackEquivalent')), AttributeVarientId: gi(l, 'ItemAttributeVarientId'),
                AttributeVarient: nameOr(g(l, 'ItemAttributeVarient')), VarientUnit: n(g(l, 'VarientEquivalent')), JobLotId: l.JobLotId, JobLot: nameOr(g(l, 'JobLotDescription')),
                ItemQty: n(l.ItemQty), AvailableQty: n(l.ItemQty), ItemWeight: n(l.ItemWeight), NetWeight: n(g(l, 'ItemNetWeight')), BalWeight: n(g(l, 'ItemNetWeight')),
                CityId: l.CityId, CityName: nameOr(l.CityName), IsFOC: toBool(l.IsFOC), RemarksDetail: nameOr(l.RemarksDetail) };
        });
    }
    /* combdeliverytrm.Text = value: selects the matching term (TextChanged clears the transporter and sets the carriage amount to 0) */
    function setTerm(text) {
        var f = cb.dterm.rows().filter(function (x) { return x.DeliveryTerm === text; })[0];
        var before = cb.dterm.text();
        if (f) cb.dterm.setValue(f.Id); else cb.dterm.clear();
        if (before !== cb.dterm.text()) termChanged();
    }
    function readById(id) {
        return SE.api(API + '/' + id).then(guard(function (d) {
            var h = d.head;
            S.id = id; S.remove = [];
            G.wg.setRows([]);
            tabs.select('tabPage1');
            $('DocDate').value = SE.dateInput(h.DocDate);
            setv('txtdocno', nameOr(h.DocNo)); setv('txtSupplierReference', nameOr(h.ReferenceNo)); setv('TxtDistance', nameOr(h.Distance)); setv('txtManualNo', nameOr(g(h, 'ManualNo')));
            cb.cust.setValue(+h.SupplierCustomerId || 0);
            setv('txtremarks', nameOr(h.RemarksHeader));
            setTerm(nameOr(h.DeliveryTerm));
            setv('txtRefPartyName', nameOr(h.ReferencPartyName)); setv('txtRefPartyAddress', nameOr(h.ReferencPartyAddress)); setv('txtRefPartyCellNo', nameOr(h.ReferencPartyCellNo));
            cb.trans.setValue(+h.TransporterId || 0);
            setv('txtFreightAmountHeader', fmtAmt(n(h.CarriageAmount)));
            setv('txtgatepassno', nameOr(h.GpNo)); $('txtgpdate').value = SE.dateInput(h.GpDate);
            cb.vtype.setValue(+h.VehicleTypeId || 0); setv('CmbVehicleNo', nameOr(h.VehicleNo));
            setv('txtDriverName', nameOr(h.DriverName)); setv('txtDriverCellNo', nameOr(h.DriverCellNo)); setv('txtCNICNO', nameOr(h.DriverCNICNo));
            S.approved = !!h.IsApproved;
            G.grd.setRows(detailRows(d.lines));
            var w = wageRowsOf(d.wages);
            G.wg.setRows(w.length ? w : [blankWage()]);
            S.existing = d.attachments || [];
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            setEnabled('btnUpdate', !!S.rights.update); setEnabled('btnDelete', !!S.rights.delete);
            fillWagesFilter();
            cb.cust.focus();
        })).catch(fail);
    }
    /* grdHistory Edit button / Ctrl+Enter: Reset(), then ReadById_Update */
    function histRead(r) { return reset().then(function () { return readById(+r.Id); }); }

    /* ------------------------------------------------------------------ Insert */
    function formValid() {
        var no = field('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', function () { $('txtdocno').focus(); });
        if (!cb.cust.row() || vid(cb.cust) === 0) return stop('CustomerName Field is Required', function () { cb.cust.focus(); });
        if (field('txtRefPartyName') === '') return stop('Ref Party Name Field is Required', function () { $('txtRefPartyName').focus(); });
        if (field('txtRefPartyAddress') === '') return stop('Ref Party Address Field is Required', function () { $('txtRefPartyAddress').focus(); });
        if (field('txtRefPartyCellNo') === '') return stop('Ref Party CellNo Field is Required', function () { $('txtRefPartyCellNo').focus(); });
        var fr = num('txtFreightAmountHeader');
        if (fr !== 0 && vid(cb.trans) === 0) return stop('Carriage Ac is Required when Carriage amount is greater than Zero', function () { cb.trans.focus(); });
        if (fr === 0 && vid(cb.trans) !== 0) return stop('Carriage Amount is Required when Carriage Ac is Selected', function () { $('txtFreightAmountHeader').focus(); });
        if (!cb.dterm.row() || vid(cb.dterm) === 0) return stop('Delivery Term Field is Required', function () { cb.dterm.focus(); });
        if (!cb.vtype.row() || vid(cb.vtype) === 0) return stop('Vehicle Type Term Field is Required', function () { cb.vtype.focus(); });
        if (field('CmbVehicleNo') === '') return stop('Vehicle No Field is Required', function () { $('CmbVehicleNo').focus(); });
        return Promise.resolve(true);
    }
    /* Insert: an empty wage Remarks is replaced by the cell texts ("Activity: .. Rate: .. Amount: .. AddLess: .. NetAmount: ..") */
    function wageRemarks(w) {
        var t = nameOr(w.Remarks);
        if (t !== '') return t;
        return 'Activity: ' + nameOr(w.ServiceActivity) + ' Rate: ' + (n(w.Rate) === 0 ? '' : SE.num(n(w.Rate), 3, 0)) + ' Amount: ' + fmtAmt(n(w.Amount)) +
            ' AddLess: ' + fmtAmt(n(w.AddLess)) + ' NetAmount: ' + fmtAmt(n(w.NetAmount));
    }
    function requestBody() {
        return { id: S.id, customerId: vid(cb.cust), deliveryTermId: vid(cb.dterm), transporterId: vid(cb.trans), vehicleTypeId: vid(cb.vtype),
            docDate: $('DocDate').value, docNo: field('txtdocno'), referenceNo: $('txtSupplierReference').value, manualNo: $('txtManualNo').value, remarks: $('txtremarks').value,
            referencePartyName: $('txtRefPartyName').value, referencePartyAddress: $('txtRefPartyAddress').value, referencePartyCellNo: $('txtRefPartyCellNo').value,
            distance: $('TxtDistance').value, carriageAmount: $('txtFreightAmountHeader').value, deliveryTerm: cb.dterm.text(), gpNo: $('txtgatepassno').value, gpDate: $('txtgpdate').value,
            vehicleNo: $('CmbVehicleNo').value, driverName: $('txtDriverName').value, driverCellNo: $('txtDriverCellNo').value, driverCnicNo: $('txtCNICNO').value,
            lines: G.grd.rows().map(function (r) { return lineOf(r, false); }), removed: S.remove,
            wages: G.wg.rows().map(function (w) { return { id: +w.Id || 0, orderId: +w.OrderId || 0, orderWagesId: +w.OrderWagesId || 0, itemId: +w.ItemId || 0, attributeVarientId: +w.AttributeVarientId || 0,
                contractorId: +w.ContractorId || 0, contractorWagesRateScheduleId: +w.ContractorWagesRateScheduleId || 0, parentUomId: +w.ParentUomId || 0, varientEquivalent: n(w.VarientEquivalent),
                qty: n(w.Qty), itemNetWeight: n(w.ItemNetWeight), rate: n(w.Rate), addLess: n(w.AddLess), netAmount: n(w.NetAmount), amount: n(w.Amount), remarks: wageRemarks(w),
                serviceActivity: nameOr(w.ServiceActivity), itemName: nameOr(w.ItemName), attributeVarient: nameOr(w.AttributeVarient) }; }),
            attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt } };
    }
    function insert() {
        if (G.grd.count() === 0) return SE.alert('Grid Record Not Found', 'Database Error');
        return formValid().then(function (ok) {
            if (!ok) return;
            if (num('txtFreightAmountHeader') > 0 && !(vid(cb.trans) > 0)) { cb.trans.focus(); return SE.alert('Transporter Account field Required', 'Database Error'); }
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                return SE.api(API, { method: 'POST', body: requestBody() }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var p = $('ChkPrintslip').checked;
                        return reset().then(function () { if (p) print(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }
    function btnSave() { S.approved = false; S.id = 0; return insert(); }
    function btnUpdate() {
        if (S.approved) return SE.alert('Record Not Update beacause Record has approved', 'Message');
        return insert();
    }
    /* btnDelete_Click */
    function del() {
        if (S.approved) return SE.alert('Record Not Delete beacause Record has approved', 'Message');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message || 'Deleted Successfully').then(reset); })
                .catch(function (e) { return SE.alert(e.message, 'Message'); });
        });
    }

    /* ------------------------------------------------------------------ history (tabPage2) */
    /* HistoryComboBind: PartyName / PartyCode follow RdCustomerHistorySearchByName */
    function histCombos(h) {
        if (h && h.customers) S.hCusts = h.customers;
        keep(cb.hCust, S.hCusts.map(function (r) { return Object.assign({ _t: histByName() ? r.PartyName : r.PartyCode }, r); }));
    }
    function tabHistory() {
        cb.dateType.focus();
        if (vid(cb.dateType) === 0 && cb.dateType.rows().length) cb.dateType.setValue(cb.dateType.rows()[0].Id);
    }
    /* btnshow_Click -> FillHistory */
    function showHistory() {
        var q = { fromDate: $('txtFromdateHistory').value, toDate: $('txtToDateHistory').value, fromNo: SE.toInt($('txtFromNoHistory').value) || 0, toNo: SE.toInt($('txtToDocNoHistory').value) || 0,
            customerId: vid(cb.hCust) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            rows.forEach(function (r) { r.PartyName = g(r, 'CustomerName'); r.PartyCode = g(r, 'PartyCode'); });
            S.hist = rows; G.hist.setRows(rows);
            if (!rows.length) G.hd.setRows([]);
        }).catch(fail);
    }
    /* Resethistory */
    function resetHistory() {
        cb.dateType.setValue(3); dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = ''; cb.hCust.clear();
        cb.dateType.focus();
    }
    /* grdHistory_SelectionChanged -> GetDetailGrdByHeadId */
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id + '/detail').then(function (lines) {
            if (!lines || !lines.length) { G.hd.setRows([]); return; }
            G.hd.setRows(lines.map(function (l) {
                return { Id: l.Id, ItemId: l.ItemId, Warehouse: nameOr(g(l, 'WareHouseName')), ItemName: l.ItemName, ItemCode: l.ItemCode, Uom: nameOr(g(l, 'PackUom')),
                    AttributeVarient: nameOr(g(l, 'ItemAttributeVarient')), VarientUnit: n(g(l, 'VarientEquivalent')), JobLot: nameOr(g(l, 'JobLotDescription')), ItemQty: n(l.ItemQty),
                    ItemWeight: n(l.ItemWeight), NetWeight: n(g(l, 'ItemNetWeight')), CityName: nameOr(l.CityName), IsFOC: toBool(l.IsFOC), RemarksDetail: nameOr(l.RemarksDetail) };
            }));
        }).catch(fail);
    }
    /* grdHistory_KeyDown */
    function histKey(e) {
        var r = G.hist.cur(), k = G.hist.curKey(); if (!r || !k) return;
        var id = +r.Id || 0;
        if (e.ctrlKey && e.code === 'Space') {
            if (k === 'Edit' && S.rights.update) { e.preventDefault(); readById(id).then(function () { cb.cust.focus(); }); }
            else if (k === 'Print') { e.preventDefault(); print(id); }
            else if (k === 'NoOfAttachments') { e.preventDefault(); showAttachments(id); }
        } else if (e.ctrlKey && e.key === 'Enter' && S.rights.update) { e.preventDefault(); readById(id).then(function () { cb.cust.focus(); }); }
        else if (e.ctrlKey && e.key.toLowerCase() === 'p' && S.rights.print) { e.preventDefault(); print(id); }
    }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id) {                                                                  // GenerateCustomerSlip (1866-InvGdnDirect_Slip.rpt)
        if (!(id > 0)) return SE.alert('Not Record Found For Display', 'Message');
        return SPC.openPdf(PRINT + SE.q({ id: id })).catch(function (x) { return msg(x.message); });
    }
    function showAttachments(id) {                                                        // CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 1866)
        return SE.api(API + '/' + id + '/attachments').then(function (rows) { SPC.attachments(S, API, id, true, rows); }).catch(fail);
    }
    function openAttachments() { SPC.attachments(S, API, S.id, false); }                  // btnAttachment_Click

    /* ------------------------------------------------------------------ keys (InvfrmPurchasedirectInvoice_KeyDown_1) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1866'], ['Ctrl+F5', 'For Focus on DocDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target, k = (e.key || '').toLowerCase();
        if (document.querySelector('.se-pop.on') && e.key !== 'Escape') return;                // a pop-up (lookup / attachments / shortcut keys) owns the keyboard
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); cb.dateType.focus(); } return; }
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORT); return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) guard(btnSave)(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) guard(btnUpdate)(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.cust.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { tabs2.select('tabPage3'); $('grd').focus(); }   // the chained ifs on the desktop always end on grd
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.wh.focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachments(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print(S.id); }
            else if (e.altKey && (e.key === '1' || e.code === 'Numpad1' || e.code === 'Digit1')) { e.preventDefault(); print(S.id); }
        } else {
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdDetailHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('grdDetailHistory')) $('grdHistory').focus(); else $('grdDetailHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('grdHistory').focus(); }
        }
    }

    /* ------------------------------------------------------------------ wiring / load */
    function on(id, ev, fn) { $(id).addEventListener(ev, fn); }
    /* txtFreightAmount_KeyPress (digits only) / txtgatepassno_KeyPress (digits only) / txtQty_KeyPress (digits and one decimal point) */
    function onlyKeys(id, re) {
        on(id, 'keypress', function (e) {
            if (e.ctrlKey || e.altKey || e.key.length !== 1) return;
            if (!re.test(e.key) || (re.source.indexOf('\\.') >= 0 && e.key === '.' && $(id).value.indexOf('.') >= 0)) e.preventDefault();
        });
    }
    function wire() {
        $('btnNew').onclick = function () { reset(); }; $('btnRefresh').onclick = function () { refresh(); };
        $('btnSave').onclick = guard(btnSave); $('btnUpdate').onclick = guard(btnUpdate); $('btnDelete').onclick = guard(del);
        $('btnAttachment').onclick = openAttachments; $('btnSlip').onclick = function () { print(S.id); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnLoadGdn').onclick = function () { /* btnLoadGdn is hidden and has no behaviour on the desktop */ };
        $('btnAdd').onclick = guard(btnAdd); $('btnUpdateDetail').onclick = guard(btnUpdateDetail); $('btnCancelUpdateDetial').onclick = guard(btnCancelUpdate);
        $('BtnGenerateWagesRows').onclick = btnGenerate; $('BtnUpdateConctratorInWages').onclick = btnUpdateContractor;
        $('BtnNewHistory').onclick = function () { guard(resetHistory)(); };
        $('BtnRefreshHistory').onclick = function () {                                         // BtnRefreshHistory_Click (no ParameterFill on this screen)
            var id = vid(cb.dateType); cb.dateType.setData(S.dateTypes); if (id > 0) cb.dateType.setValue(id);
            SE.api(API + '/history-combos').then(histCombos).catch(fail);
        };
        $('btnshow').onclick = showHistory;
        on('DocDate', 'change', function () { guard(updateRatesAll)(); });                      // DocDate_ValueChanged -> UpdateWagesRateForAllRows
        on('txtQty', 'input', guard(calcWeight));                                              // txtQty_TextChanged
        on('RdPartyByName', 'change', guard(function () { var id = vid(cb.cust); fillCustomers(); if (id > 0) cb.cust.setValue(id); cb.cust.focus(); }));      // RdPartyByName_CheckedChanged
        on('RdPartyByCode', 'change', guard(function () { var id = vid(cb.cust); fillCustomers(); if (id > 0) cb.cust.setValue(id); cb.cust.focus(); }));
        var rh = guard(function () { var id = vid(cb.hCust); histCombos(); if (id > 0) cb.hCust.setValue(id); cb.hCust.focus(); });                          // RdCustomerHistorySearchByName_CheckedChanged
        on('RdCustomerHistorySearchByName', 'change', rh); on('RdCustomerHistorySearchByCode', 'change', rh);
        onlyKeys('txtFreightAmountHeader', /^\d$/); onlyKeys('txtgatepassno', /^\d$/); onlyKeys('txtQty', /^[\d.]$/);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') tabHistory(); else $('DocDate').focus(); });
        tabs2 = SE.tabs('tabControl2', function () { });
        makeGrids();
        wire();
        SE.api(API + '/initial').then(guard(function (d) {
            S.rights = d.rights || {}; S.fmt = d.fmt || S.fmt; S.def = d.defaults || {}; S.search = d.search || {}; S.fyStart = d.fyStart || ''; S.dateTypes = d.dateTypes || [];
            [G.grd, G.wg, G.hist, G.hd].forEach(function (gr) { gr.spec.dec = S.fmt.amount; });
            $('rdbtnItemCode').checked = !!S.search.itemByCode; $('rdbtnItemName').checked = !S.search.itemByCode;
            $('RdPartyByCode').checked = !!S.search.partyByCode; $('RdPartyByName').checked = !S.search.partyByCode;
            $('RdCustomerHistorySearchByCode').checked = !!S.search.partyByCode; $('RdCustomerHistorySearchByName').checked = !S.search.partyByCode;
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo);
            applyLists(d);
            cb.dateType.setData(S.dateTypes);
            histCombos(d.history || {});
            var r = S.rights;
            $('btnSave').disabled = !r.save; $('btnUpdate').disabled = !r.update; $('btnDelete').disabled = !r.delete; $('btnSlip').disabled = !r.print;
            $('ChkPrintslip').disabled = !r.print; $('ChkPrintslip').checked = !!r.print;
            show('btnUpdate', false); show('btnDelete', false); show('btnSave', true); show('btnLoadGdn', false);
            show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('DocDate').value = SE.today(); $('txtgpdate').value = SE.today(); $('txtFromdateHistory').value = SE.today(); $('txtToDateHistory').value = SE.today();
            fillWagesFilter();
            $('DocDate').focus();
            loadItems().then(function () {
                applyDefaults();
                var rec = SE.param('record'); if (rec) readById(+rec);
            });
        })).catch(fail);
    }
    document.addEventListener('DOMContentLoaded', load);
})();
