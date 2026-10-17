/* Screen 465 "Goods Receiving Notes For Gate Purchase" - Architecture.WinApp.Paddy_Purchase/frmGrnForGate.cs (InvGrn DocumentTypeId 165).
   Line numbers in the comments are to that file. */
(function () {
    'use strict';
    const API = '/api/paddy-purchase/grn-gate';
    const $ = PP.$;
    let Id = 0;                      // Id (int field): ReadById_Update / history Edit set it, btnSave_Click zeroes it, reset() leaves it
    let GPID = 0;                    // GPID: the gate pass picked from grdGp
    let updateDetailIndex = -1;
    let cfg = { juteBagsCut: 0, ppBagsCut: 0, openBulkCut: 0, emptyBagsWeightCutEditable: false, labCompulsory: false, defaultJobLot: 0, defaultPackingType: 0, defaultTransporter: 0 };
    let lookups = {};
    let rights = { canSave: false, canUpdate: false, canPrint: false };
    let dtDetail = [];               // dtdetail rows (ItemId, Item, CropYear, JobId, Job, PackingTypeId, PackingType, UOM, UOMId, Qty, GrossWeight, ScaleKart, EbUnit, EbTotal, NetWeight, WareHouse, LabNo, City, ContractorId, Contractor)
    let dtEmptyBag = [];             // dtEmptyBag rows (OrderId, Type, ItemId, Condition, RecQty, PurQty, Remarks)
    let dtLabInfo = [];
    let uomGen = 0, historyGen = 0;

    function show(msg) { alert(msg); }
    const blankBag = () => ({ OrderId: 0, Type: 0, ItemId: 0, Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' });
    const g = (row, key) => { if (!row) return null; for (const x in row) if (x.toLowerCase() === key.toLowerCase()) return row[x]; return null; };
    const str = v => (v == null ? '' : String(v));
    const r2 = x => String(Math.round((x + Number.EPSILON) * 100) / 100);    // Math.Round(x, 2).ToString()
    function focusCombo(id) { const w = $(id).parentElement && $(id).parentElement.querySelector('.dtcombo-input'); (w || $(id)).focus(); }
    const btnVisible = id => $(id).style.display !== 'none';

    /* ---------- InvFrmGRN_Load (:1620) ---------- */
    async function load() {
        const d = await PP.fetch(API + '/init');
        rights = d; cfg = Object.assign(cfg, d.config || {}); lookups = d;
        $('txtebu').value = '0'; $('txtScaleKart').value = '0';
        $('txtebu').disabled = !cfg.emptyBagsWeightCutEditable;           // txtebu.Enabled = EmptyBagsWeightCutEditable
        $('btnSave').disabled = !d.canSave; $('printToolStripButton').disabled = !d.canPrint; $('btnUpdate').disabled = !d.canUpdate;
        bindAll(true);
        dtDetail = []; renderDetail();
        dtEmptyBag = [blankBag()]; renderEmptyBags();
        $('txtdocno').value = d.docNo > 0 ? d.docNo : '0';
        $('DocDate').value = PP.today();
        renderGp(d.gatePasses || []);
        $('ChkBox').checked = true;
        defaultConfiquration();
        $('DocDate').focus();
    }
    function bindAll(first) {
        if (first) {
            PP.bind('combbrnch', lookups.branches, 'id', 'name', { activateFirst: true });       // BranchFill
            PP.bind('combproject', lookups.projects, 'id', 'name', { activateFirst: true });     // ProjectFill
            PP.bind('CmbContractor', lookups.contractors, 'id', 'name');                         // ContractorBind
            PP.bind('CmbStatus', lookups.statuses, 'id', 'name', { activateFirst: true });       // Status
        }
        PP.bind('combsupplier', lookups.suppliers, 'id', 'name');                                // SupplierNameFilll
        PP.bind('CmbTransport', lookups.transporters, 'id', 'name');                             // TransportFill
        PP.bind('combvehtyp', lookups.vehicleTypes, 'id', 'name');                               // VehicleTypesFill
        PP.bind('combcropyear', lookups.cropYears, 'id', 'name', { activateFirst: true });       // CropYear
        PP.bind('combjoblot', lookups.jobLots, 'id', 'name');                                    // JobLotFillWithoutOrder
        PP.bind('combpcktyp', lookups.packingTypes, 'id', 'name', { activateFirst: true });      // PackingTypeFill
        PP.bind('combwr', lookups.warehouses, 'id', 'name', { activateFirst: true });            // WarehouseFill
        PP.bind('CmbCity', lookups.cities, 'id', 'name');                                        // CityFill
    }
    function defaultConfiquration() {                                                            // (:1069)
        if (cfg.defaultJobLot > 0) PP.setSel('combjoblot', cfg.defaultJobLot);
        if (cfg.defaultPackingType > 0) PP.setSel('combpcktyp', cfg.defaultPackingType);
        if (cfg.defaultTransporter > 0) PP.setSel('CmbTransport', cfg.defaultTransporter);
    }

    /* ---------- grids ---------- */
    const wareName = id => { const w = (lookups.warehouses || []).find(x => String(x.id) === String(id)); return w ? w.name : str(id); };
    function renderDetail() {                                  // grdSettings (:1480): hidden ids, PackUom / WareHouse Name / City Name captions, sums, "#,##.##"
        const rows = dtDetail.map((r, i) => ({ _i: i, Item: r.Item, CropYear: r.CropYear, Job: r.Job, PackingType: r.PackingType, UOM: r.UOM, Qty: PP.num(r.Qty), GrossWeight: PP.num(r.GrossWeight),
            ScaleKart: PP.num(r.ScaleKart), EbUnit: PP.num(r.EbUnit), EbTotal: PP.num(r.EbTotal), NetWeight: PP.num(r.NetWeight), WareHouse: wareName(r.WareHouse), LabNo: r.LabNo, City: r.City, Contractor: r.Contractor }));
        const two = { Qty: 2, GrossWeight: 2, ScaleKart: 2, EbUnit: 2, EbTotal: 2, NetWeight: 2 };
        PP.renderGrid('grd', rows, { hide: ['_i'], caption: { UOM: 'PackUom', WareHouse: 'WareHouse Name', City: 'City Name' }, numeric: Object.keys(two), totals: two, dec: two,
            post: [{ key: 'Delete', label: 'X', onclick: row => { dtDetail.splice(row._i, 1); renderDetail(); } }],
            onDbl: row => grd_DoubleClick(row._i) });
    }
    function renderGp(rows) {                                  // GRNGridSetting (:1243)
        const z = { Qty: 0, SupplierWeight: 0, FactoryWeight: 0 };
        PP.renderGrid('grdGp', rows, { hide: ['Id'], dateCols: ['GpDate'], link: ['GpSrNo'], numeric: Object.keys(z), totals: z, dec: z,
            post: [{ key: 'Load', label: 'Load', onclick: r => loadGp(r) }],
            onLink: (k, r) => printGatePass(r.Id) });
    }
    function renderEmptyBags() {                               // grdEmptyBagsSettings (:1544)
        const t = $('grdEmptyBags'), head = t.querySelector('thead'), body = t.querySelector('tbody'), foot = t.querySelector('tfoot');
        t.classList.add('pg-eb');
        head.innerHTML = '<tr><th>EmptyBagsType</th><th style="min-width:250px">Item Name</th><th style="min-width:120px">Bags_Condition</th><th>RecQty</th><th>PurQty</th><th>Remarks</th><th>+</th><th>X</th></tr>';
        const opts = (list, sel) => '<option value="0"></option>' + (list || []).map(o => '<option value="' + PP.esc(o.id) + '"' + (String(o.id) === String(sel) ? ' selected' : '') + '>' + PP.esc(o.name) + '</option>').join('');
        body.innerHTML = dtEmptyBag.map((r, i) => '<tr data-i="' + i + '">'
            + '<td style="min-width:80px"><select data-eb="Type">' + opts(lookups.emptyBagTypes, r.Type) + '</select></td>'
            + '<td><select data-eb="ItemId">' + opts(lookups.emptyBagItems, r.ItemId) + '</select></td>'
            + '<td><select data-eb="Condition">' + opts(lookups.bagConditions, r.Condition) + '</select></td>'
            + '<td style="width:70px"><input type="text" data-eb="RecQty" data-guard="decimal" value="' + PP.esc(PP.num(r.RecQty)) + '"></td>'
            + '<td style="width:70px"><input type="text" data-eb="PurQty" data-guard="decimal" value="' + PP.esc(PP.num(r.PurQty)) + '"></td>'
            + '<td><input type="text" class="pg-remarks" data-eb="Remarks" value="' + PP.esc(r.Remarks) + '"></td>'
            + '<td class="btn"><button type="button" data-ebact="Add">+</button></td><td class="btn"><button type="button" data-ebact="Delete">X</button></td></tr>').join('');
        foot.innerHTML = '<tr><td></td><td></td><td></td><td>' + PP.fmtNum(dtEmptyBag.reduce((s, r) => s + PP.num(r.RecQty), 0), 3) + '</td><td>' + PP.fmtNum(dtEmptyBag.reduce((s, r) => s + PP.num(r.PurQty), 0), 3) + '</td><td></td><td></td><td></td></tr>';
    }
    /* grdEmptyBags_CellUpdated (:2353) */
    function emptyBagCellUpdated(i, key) {
        const r = dtEmptyBag[i]; if (!r) return;
        if (Math.trunc(PP.num(r.PurQty)) < 0) { r.PurQty = 0; show('PurQty cannot be less than Zero'); }
        if (Math.trunc(PP.num(r.RecQty)) < 0) { r.RecQty = 0; show('RecQty cannot be less than Zero'); }
        if (key === 'PurQty' || key === 'Type') calculationPurchaseAgainstWeight();
        if ((PP.num(r.Type) === 4 || PP.num(r.Type) === 5) && key === 'PurQty') { r.PurQty = 0; show('Retained or Returned Stock you cannot be purchase'); }
        if (PP.num(r.Type) === 2 && key === 'RecQty') { r.RecQty = 0; show('You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight'); }
        renderEmptyBags(); renderDetail();
    }
    /* CalculationPurchaseAgainstWeight (:2399) */
    function calculationPurchaseAgainstWeight() {
        let AllocateQty = 0, PurQty = 0, EBQtyLessFromBillWeight = 0;
        dtEmptyBag.forEach(b => { if (Math.trunc(PP.num(b.Type)) === 2) PurQty += PP.num(b.PurQty); });
        dtDetail.forEach(item => {
            let TotalPurchaseAgainstWeightAddinBillWeight = 0;
            const Qty = PP.num(item.Qty), GrossWeight = PP.num(item.GrossWeight), EBUnit = PP.num(item.EbUnit), ScaleKart = PP.num(item.ScaleKart), AddLess = 0;
            if (EBUnit > 0) {
                const grdQty = PP.num(item.Qty);
                if (PurQty > 0) {
                    PurQty -= AllocateQty;
                    if (PurQty > grdQty) EBQtyLessFromBillWeight = grdQty;
                    else if (grdQty > PurQty) EBQtyLessFromBillWeight = PurQty;
                    if (PurQty === grdQty) EBQtyLessFromBillWeight = grdQty;
                    if (PurQty > 0) TotalPurchaseAgainstWeightAddinBillWeight = EBQtyLessFromBillWeight * EBUnit;
                }
            }
            let EBTotal;
            if (EBUnit > 0 && Qty > 0) { item.EbTotal = EBUnit * Qty; EBTotal = item.EbTotal; } else { item.EbTotal = 0; EBTotal = 0; }
            const Weight = GrossWeight - ScaleKart - EBTotal + AddLess + TotalPurchaseAgainstWeightAddinBillWeight;
            item.NetWeight = PP.num(r2(Weight));
            AllocateQty = EBQtyLessFromBillWeight;
        });
    }

    /* ---------- Total (:1396) ---------- */
    function total() {
        const FactoryWeight = PP.num($('txtfctwt').value), ScaleKart = PP.num($('txtScaleKart').value), Qty = PP.num($('txtqty').value), GrossWeight = PP.num($('txtgwt').value);
        let ScKartDetail = 0, EBTotal = 0;
        if (GrossWeight > 0 && FactoryWeight > 0 && ScaleKart > 0) { ScKartDetail = GrossWeight / FactoryWeight * ScaleKart; $('txtScaleKartDetail').value = r2(ScKartDetail); }
        else $('txtScaleKartDetail').value = String(ScKartDetail);
        const EBUnit = PP.num($('txtebu').value);
        if (EBUnit > 0 && Qty > 0) EBTotal = EBUnit * Qty;
        $('txtEmptyBagsTotal').value = r2(EBTotal);
        $('txtnetwt').value = r2(GrossWeight - ScKartDetail - EBTotal);
    }

    /* ---------- gate pass pick (grdGp "Load" / Ctrl+Enter) and GatePassRecordFill (:1331) ---------- */
    function loadGp(row) {
        if (!(btnVisible('btnSave') && !$('btnSave').disabled)) return;
        GPID = PP.num(g(row, 'Id'));
        $('combgatepass').value = str(g(row, 'GpSrNo'));
        $('txtsuppwt').value = str(g(row, 'SupplierWeight'));
        $('txtfctwt').value = str(g(row, 'FactoryWeight'));
        if (PP.num($('txtsuppwt').value) === 0) $('txtsuppwt').value = str(g(row, 'FactoryWeight'));
        return gatePassRecordFill();
    }
    async function gatePassRecordFill() {
        $('DocDate').focus();
        const d = await PP.fetch(API + '/gate-pass/' + GPID);
        if (!d.found) return;
        const r = d.gp;
        GPID = Math.trunc(PP.num(g(r, 'Id')));
        PP.setSelText('combvehtyp', str(g(r, 'VehicleType')));
        $('txtvehno').value = str(g(r, 'VehicleNo'));
        $('txtbltyno').value = str(g(r, 'BiltyNo'));
        $('txtremarks').value = str(g(r, 'OtherRemarks'));
        $('txtcarramount').value = str(g(r, 'NetPaid'));
        $('txtqty').value = str(g(r, 'NoOfPackages')); total();
        PP.setSelText('combwr', str(g(r, 'WareHouseName')));
        PP.setSel('combsupplier', g(r, 'SupplierCustomerId'));
        PP.setSel('CmbCity', g(r, 'CityId'));
        $('txtScaleKart').value = str(g(r, 'ScaleKart')); total();
        if (cfg.labCompulsory) bindLab(d.lab);
    }
    function bindLab(lab) {                                    // GetLabDataByGpIdForGrnGate -> combitem (ItemId / ItemName)
        dtLabInfo = lab || [];
        if (dtLabInfo.length > 0) PP.bind('combitem', dtLabInfo.map(r => ({ id: g(r, 'ItemId'), name: g(r, 'ItemName') })), 'id', 'name');
        else PP.bind('combitem', [], 'id', 'name');
    }
    /* combitem_Leave (:1349) with PackUOMFillWithoutOrder (:1170) */
    async function combitemLeave() {
        if (Math.trunc(PP.num($('combgatepass').value)) <= 0) return;
        await packUomFill();
        const v = $('combitem').value;
        if (dtLabInfo.length > 0 && v !== '') {
            const row = dtLabInfo.find(r => String(g(r, 'ItemId')) === String(v));
            if (row) {
                $('txtLabNo').value = str(g(row, 'DocNo'));
                PP.setSelText('combcropyear', str(g(row, 'Crop')));
                $('txtgwt').value = str(g(row, 'NetWeight')); total();
            }
        } else { $('txtgwt').value = '0'; total(); $('txtLabNo').value = ''; }
    }
    async function packUomFill() {
        const gen = ++uomGen, item = $('combitem').value;
        const rows = item === '' ? [] : await PP.fetch(API + '/uoms?itemId=' + encodeURIComponent(item));
        if (gen !== uomGen) return;
        PP.bind('combpckuom', rows, 'Id', 'Equivalent');       // combpckuom.Text cleared and DataSource null when there are no rows
    }
    function combpcktypLeave() {                               // (:2380)
        const t = Math.trunc(PP.num($('combpcktyp').value));
        if (t === 1) { $('txtebu').value = String(cfg.juteBagsCut); total(); }
        if (t === 2) { $('txtebu').value = String(cfg.ppBagsCut); total(); }
        if (t === 5) { $('txtebu').value = String(cfg.openBulkCut); total(); }
    }

    /* ---------- detail row editing ---------- */
    function formValidationDetail() {                          // (:741)
        const fail = (msg, id) => { show(msg); focusCombo(id); return false; };
        if (!$('combitem').value) return fail('Item Name Field is Required', 'combitem');
        if (!$('combcropyear').value) return fail('Crop Year Field is Required', 'combcropyear');
        if (!$('combjoblot').value) return fail('Job/Lot Field is Required', 'combjoblot');
        if (!$('combpcktyp').value) return fail('Packing Type Field is Required', 'combpcktyp');
        if ($('txtqty').value.trim() === '' || PP.num($('txtqty').value) === 0) { show('Qty Field is Required'); $('txtqty').focus(); return false; }
        if (Math.trunc(PP.num($('combpckuom').value)) === 0) return fail('Pack Unit Field is Required', 'combpckuom');
        if ($('txtgwt').value.trim() === '' || PP.num($('txtgwt').value) === 0) { show('Gross Weight Field is Required'); $('txtgwt').focus(); return false; }
        if ($('txtnetwt').value.trim() === '' || PP.num($('txtnetwt').value) === 0) { show('Net Bill Weight Field is Required'); $('txtnetwt').focus(); return false; }
        if (!$('combwr').value) return fail('Warehouse Field is Required', 'combwr');
        if (!$('CmbCity').value) return fail('City Field is Required', 'CmbCity');
        if (PP.num($('txtScaleKart').value) > 0 && PP.num($('txtScaleKartDetail').value) === 0) { show('Scale Kart is Required'); $('txtScaleKartDetail').focus(); return false; }
        if (!$('CmbContractor').value) return fail('Contractor Field is Required', 'CmbContractor');
        return true;
    }
    function rowFromBoxes() {
        return { ItemId: $('combitem').value, Item: PP.selText('combitem'), CropYear: PP.selText('combcropyear'), JobId: $('combjoblot').value, Job: PP.selText('combjoblot'),
            PackingTypeId: $('combpcktyp').value, PackingType: PP.selText('combpcktyp'), UOM: PP.selText('combpckuom'), UOMId: $('combpckuom').value,
            Qty: PP.num($('txtqty').value), GrossWeight: PP.num($('txtgwt').value), ScaleKart: PP.num($('txtScaleKartDetail').value), EbUnit: PP.num($('txtebu').value),
            EbTotal: PP.num($('txtEmptyBagsTotal').value), NetWeight: PP.num($('txtnetwt').value), WareHouse: $('combwr').value, LabNo: $('txtLabNo').value,
            City: PP.selText('CmbCity'), ContractorId: $('CmbContractor').value, Contractor: PP.selText('CmbContractor') };
    }
    function resetDetial() {                                   // ResetDetial (:1714)
        ['txtqty', 'txtgwt', 'txtebu', 'txtEmptyBagsTotal', 'txtnetwt', 'txtLabNo', 'txtScaleKartDetail'].forEach(i => { $(i).value = ''; });
        PP.bind('combpckuom', [], 'Id', 'Equivalent');
        focusCombo('combitem');
    }
    function addClick() {                                      // Add_Click (:259)
        if (!formValidationDetail()) return;
        if (PP.num($('txtnetwt').value) > PP.num($('txtgwt').value)) { show('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        dtDetail.push(rowFromBoxes());
        renderDetail(); resetDetial();
    }
    async function grd_DoubleClick(i) {                        // (:2146)
        const item = dtDetail[i]; if (!item) return;
        updateDetailIndex = i;
        PP.setSel('combitem', item.ItemId);
        await packUomFill();
        PP.setSelText('combcropyear', item.CropYear);
        PP.setSel('combjoblot', item.JobId); PP.setSel('combpcktyp', item.PackingTypeId);
        $('txtqty').value = str(item.Qty); total();
        PP.setSel('combpckuom', item.UOMId);
        $('txtgwt').value = str(item.GrossWeight); total();
        $('txtebu').value = str(item.EbUnit); total();
        $('txtEmptyBagsTotal').value = str(item.EbTotal); $('txtScaleKartDetail').value = str(item.ScaleKart); $('txtnetwt').value = str(item.NetWeight);
        PP.setSel('combwr', item.WareHouse); $('txtLabNo').value = str(item.LabNo); PP.setSelText('CmbCity', item.City); PP.setSel('CmbContractor', item.ContractorId);
        $('Add').style.display = 'none'; $('btnUpdateDetail').style.display = ''; $('btnCancelUpdateDetial').style.display = '';
        focusCombo('combitem');
    }
    function btnUpdateDetailClick() {                          // (:2200)
        if (!formValidationDetail()) return;
        if (PP.num($('txtnetwt').value) > PP.num($('txtgwt').value)) { show('NetBillWeight cannot be greater than GrossWeight Please check'); return; }
        if (!dtDetail[updateDetailIndex]) return;
        dtDetail[updateDetailIndex] = rowFromBoxes();
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
        renderDetail(); resetDetial();
    }
    function btnCancelUpdateDetialClick() { $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none'; }

    /* ---------- ReadById_Update (:565) ---------- */
    async function readById_Update(id) {
        try {
            const d = await PP.fetch(API + '/' + id);
            const h = d.header;
            $('btnSave').style.display = 'none'; $('btnUpdate').style.display = '';
            Id = Number(id);
            PP.setSel('combbrnch', g(h, 'BranchesId')); PP.setSel('combproject', g(h, 'ProjectsId'));
            $('txtdocno').value = str(g(h, 'DocNo'));
            const dd = g(h, 'DocDate'); $('DocDate').value = dd ? PP.localStamp(new Date(String(dd).replace(' ', 'T'))).slice(0, 10) : PP.today();
            switchView('Form');
            PP.setSel('combsupplier', g(h, 'SupplierCustomerId')); PP.setSel('CmbTransport', g(h, 'TransporterId'));
            $('txtcarramount').value = str(g(h, 'CarriageAmount')); $('txtsuppwt').value = str(g(h, 'PartyWeight')); $('txtfctwt').value = str(g(h, 'FactoryWeight'));
            $('txtScaleKart').value = str(g(h, 'ScaleKart')); $('combgatepass').value = str(g(h, 'GpNo'));
            PP.setSelText('combvehtyp', str(g(h, 'VehicleType'))); $('txtvehno').value = str(g(h, 'VehicleNo')); $('txtbltyno').value = str(g(h, 'BiltyNo'));
            $('txtremarks').value = str(g(h, 'RemarksHeader'));
            GPID = Math.trunc(PP.num(g(h, 'InwardGatePassId')));
            if (cfg.labCompulsory) bindLab(d.lab);
            dtDetail = d.details || [];
            dtEmptyBag = (d.emptyBags || []);
            renderDetail();
            if (!dtEmptyBag.length) dtEmptyBag = [blankBag()];
            renderEmptyBags();
        } catch (e) { PP.error(e); }
    }

    /* ---------- reset (:1469) ---------- */
    async function reset() {
        GPID = 0;
        PP.setSel('combsupplier', ''); PP.setSel('CmbTransport', '');
        const hadGross = $('txtgwt').value !== '';
        ['txtcarramount', 'txtsuppwt', 'txtfctwt', 'combgatepass', 'txtvehno', 'txtbltyno', 'txtremarks', 'txtqty', 'txtebu', 'txtEmptyBagsTotal', 'txtScaleKart', 'txtnetwt', 'txtLabNo', 'txtgwt'].forEach(i => { $(i).value = ''; });
        const vt = $('combvehtyp'); if (vt.options.length > 1) vt.selectedIndex = 1;                        // combvehtyp.Rows[0].Activate()
        PP.bind('combitem', [], 'id', 'name'); PP.setSel('CmbCity', '');
        PP.bind('combpckuom', [], 'Id', 'Equivalent');
        if (hadGross) total();                                                                               // txtgwt.Text = "" fires Total()
        $('btnSave').style.display = ''; $('btnUpdate').style.display = 'none';
        dtDetail = []; dtLabInfo = []; renderDetail();
        dtEmptyBag = [blankBag()]; renderEmptyBags();
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
        defaultConfiquration();
        $('txtgwt').disabled = false;
        try {
            const [n, gps] = await Promise.all([PP.fetch(API + '/doc-no'), PP.fetch(API + '/gate-passes')]);
            $('txtdocno').value = n.docNo > 0 ? n.docNo : '0'; renderGp(gps);
        } catch (e) { PP.error(e); }
        $('DocDate').focus();
    }

    /* ---------- Insert (:300): FormValidation, grid, confirm, then the save chain on the server ---------- */
    function formValidation() {                                // (:707)
        const t = $('txtdocno').value.trim();
        if (t === '' || t === '0') { show('DocNo Field is Required'); return false; }
        if (!$('combbrnch').value) { show('Branch Field is Required'); focusCombo('combbrnch'); return false; }
        if (!$('combproject').value) { show('Project Field is Required'); focusCombo('combproject'); return false; }
        if (!$('combsupplier').value) { show('Supplier Field is Required'); focusCombo('combsupplier'); return false; }
        if (Math.trunc(PP.num($('combgatepass').value.trim())) === 0) { show('Gatepass Field is Required'); $('combgatepass').focus(); return false; }
        if (PP.num($('txtsuppwt').value.trim()) === 0) { show('Supplier Weight Field is Required'); $('txtsuppwt').focus(); return false; }
        if (PP.num($('txtfctwt').value.trim()) === 0) { show('Factory Weight Field is Required'); $('txtfctwt').focus(); return false; }
        if (!$('CmbStatus').value) { show('Status Field is Required'); focusCombo('CmbStatus'); return false; }
        return true;
    }
    async function insert() {
        if (!formValidation()) return;
        if (dtDetail.length === 0) { show('Grid Record Not Found'); return; }
        if (!PP.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        const carriage = PP.num($('txtcarramount').value.trim());
        if (carriage > 0 && !$('CmbTransport').value) { show('Transporter Account field required'); focusCombo('CmbTransport'); return; }
        const body = {
            id: Id, docNo: $('txtdocno').value.trim(), docDate: $('DocDate').value, branchId: $('combbrnch').value, projectId: $('combproject').value, supplierId: $('combsupplier').value,
            gpNo: $('combgatepass').value.trim(), gpId: GPID, suppWeight: $('txtsuppwt').value.trim(), factWeight: $('txtfctwt').value.trim(), scaleKart: $('txtScaleKart').value.trim(),
            vehicleType: PP.selText('combvehtyp'), vehicleNo: $('txtvehno').value.trim(), biltyNo: $('txtbltyno').value.trim(), status: PP.selText('CmbStatus'),
            transporterId: $('CmbTransport').value, carriage: $('txtcarramount').value.trim(), remarks: $('txtremarks').value.trim(), details: dtDetail, emptyBags: dtEmptyBag
        };
        const res = await PP.post(API + '/save', body);
        show(res.message);
        const preview = $('ChkBox').checked;
        await reset();
        if (preview) generateReport(res.id);
    }
    async function btnSaveClick() { Id = 0; await insert(); }                 // btnSave_Click
    async function btnUpdateClick() {                                           // btnUpdate_Click
        if (Id === 0) { show('Record not update because Id not found'); return; }
        await insert();
    }

    /* ---------- history ---------- */
    async function historyGridFill(n) {                       // HistoryGridFill (:1786)
        const rows = await PP.fetch(API + '/history?noOfRecords=' + (n || 0));
        PP.renderGrid('GrdHistory', rows, { hide: ['Id', 'InwardGatePassId'], dateCols: ['DocDate', 'EntryDate', 'ModifyDate'], link: ['GpNo', 'NoOfAttachments'], linkAlways: ['NoOfAttachments'], frozen: 2,
            numeric: ['FactoryWeight', 'PartyWeight', 'Freight'], dec: { FactoryWeight: 3, PartyWeight: 3, Freight: 3 },
            pre: [{ key: 'Edit', label: 'Edit', onclick: r => { readById_Update(r.Id); } }, { key: 'Print', label: 'Print', onclick: r => generateReport(r.Id) }],
            onRow: r => history_SelectionChanged(r), onDbl: r => readById_Update(r.Id),
            onLink: (k, r) => { if (k === 'NoOfAttachments') showAttachments(r.Id); else printGatePass(r.InwardGatePassId); } });
        if (!rows.length) PP.renderGrid('GrdHistoryDetail', [], {}); else history_SelectionChanged(rows[0]);
    }
    async function history_SelectionChanged(row) {            // GrdHistory_SelectionChanged (:2445)
        if (!row) return;
        const gen = ++historyGen;
        try {
            const rows = await PP.fetch(API + '/history-detail/' + row.Id);
            if (gen !== historyGen) return;
            const two = { Qty: 0, EbUnit: 0, GrossWeight: 0, EbTotal: 0, NetWeight: 0 };       // gridhistorydetail (:2092): "0,0" sums
            PP.renderGrid('GrdHistoryDetail', rows, { caption: { WareHouse: 'WareHouse Name' }, numeric: Object.keys(two), totals: two, dec: two });
        } catch (e) { PP.error(e); }
    }

    /* ---------- print / attachments / misc ---------- */
    function generateReport(printId) {                         // GenerateReport (:2252) -> CommonServices.GrnSlipWithSubReports(PrintId): DocumentTypeId 0 is sent as 46
        if (!printId) { show('No Record Found For Display'); return; }
        if (window.printRpt) window.printRpt('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: Number(printId), documentTypeId: 46 }); else show('The print runtime is not loaded on this page.');
    }
    function printGatePass(gpId) {                             // CommonServices.GatePassInwardWithDetailAndLabReport(Id)
        if (!gpId) { show('No Record Found For Display'); return; }
        if (window.printRpt) window.printRpt('256-GatePassInward_WithDetailSlip.rpt', { id: Number(gpId) }); else show('The print runtime is not loaded on this page.');
    }
    async function showAttachments(id) {                       // GetNoofAttachmentsByRefDocumentTypeID(Id, 165)
        try {
            const rows = await PP.fetch(API + '/' + id + '/attachments');
            $('attBody').innerHTML = rows.length ? rows.map(r => '<div><a href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank" rel="noopener">' + PP.esc(str(r.Attachment).split(/[\\/]/).pop()) + '</a></div>').join('') : '<div>No attachments</div>';
            $('attModal').style.display = 'flex';
        } catch (e) { PP.error(e); }
    }
    async function btnAttachmentClick() {                      // btnAttachment_Click (:2172): AT.Show()
        if (Id > 0) return showAttachments(Id);
        show('Attachments can be viewed for a saved record here. Adding files from the web is not available yet.');
    }
    async function toolStripButton1Click() {                   // toolStripButton1_Click (:2216)
        const d = await PP.fetch(API + '/lookups'); lookups = Object.assign(lookups, d); cfg = Object.assign(cfg, (await PP.fetch(API + '/config')));
        bindAll(false); renderEmptyBags();
    }
    async function openIfViewRight(screen, url) {              // btnGrnFormHistory_Click (:2313)
        const w = window.open('about:blank', '_blank');
        try {
            const r = await PP.fetch('/api/purchase/screen-view-right/' + screen);
            if (r && r.view) { if (w) w.location.href = url; else window.open(url, '_blank'); } else { if (w) w.close(); show('Please Check Screen Rights'); }
        } catch (e) { if (w) w.close(); show(e.message); }
    }

    /* ---------- tabs and keyboard ---------- */
    function isHistory() { return $('viewHistory').style.display !== 'none'; }
    function switchView(mode) {                                // tabControl1_SelectedIndexChanged (:2123): History loads 50 rows
        const h = mode === 'History';
        $('viewForm').style.display = h ? 'none' : 'block'; $('viewHistory').style.display = h ? 'block' : 'none';
        if (h) historyGridFill(50).catch(PP.error);
    }
    function closeForm() { if (window.opener) window.close(); else history.back(); }
    function onKey(e) {                                        // InvFrmGRN_KeyDown (:1840)
        const k = e.key.toLowerCase(), formTab = !isHistory();
        if (e.ctrlKey && e.key === 'Enter' && formTab && btnVisible('btnSave') && !$('btnSave').disabled) { const r = $('grdGp')._current; if (r) loadGp(r); return; }
        if (e.ctrlKey && k === 'l' && isHistory()) { e.preventDefault(); historyGridFill(0).catch(PP.error); }
        if (e.ctrlKey && k === 'h' && isHistory()) { e.preventDefault(); btnGrnFormHistoryClick(); }
        if (e.ctrlKey && k === 's' && formTab) { e.preventDefault(); if (btnVisible('btnSave') && !$('btnSave').disabled) PP.withButton($('btnSave'), btnSaveClick); }
        if (e.ctrlKey && k === 'n' && formTab) { e.preventDefault(); PP.withButton($('btnNew'), reset); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); switchView(isHistory() ? 'Form' : 'History'); }
        if (e.ctrlKey && k === 'e') { e.preventDefault(); closeForm(); }
        if (e.ctrlKey && k === 'u') { e.preventDefault(); if (btnVisible('btnUpdate') && !$('btnUpdate').disabled) PP.withButton($('btnUpdate'), btnUpdateClick); }
        if (e.ctrlKey && k === 'p') {
            e.preventDefault();
            if (formTab && !$('printToolStripButton').disabled) generateReport(Id);
            if (!formTab) { const r = $('GrdHistory')._current; if (r) generateReport(r.Id); }
        }
        if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focusCombo('combitem'); }
        if (e.ctrlKey && e.key === 'ArrowDown') { const t = $('wrapGrd'); t.tabIndex = -1; t.focus(); }
        if (e.ctrlKey && e.key === 'ArrowRight') { const t = $('wrapGp'); t.tabIndex = -1; t.focus(); }
        if (e.ctrlKey && e.key === 'ArrowLeft') { const t = $('wrapEmptyBags'); t.tabIndex = -1; t.focus(); }
        if (e.ctrlKey && e.key === 'Enter' && isHistory()) { const r = $('GrdHistory')._current; if (r) readById_Update(r.Id); }
        if (e.altKey && k === 'r') { e.preventDefault(); toolStripButton1Click().catch(PP.error); }
        if (e.altKey && k === 'a') { e.preventDefault(); btnAttachmentClick(); }
    }
    function btnGrnFormHistoryClick() { return openIfViewRight('frmGRNHistory', '/purchase/reports/grn-register'); }

    document.addEventListener('DOMContentLoaded', () => {
        PP.installGuards(); PP.enterToTab($('viewForm'));
        if (window.PurchaseChrome) {
            PurchaseChrome.footer({ isHistory, toggle: () => switchView(isHistory() ? 'Form' : 'History'), watch: '#viewHistory' });
            ['#wrapHistory', '#wrapHistoryDetail', '#wrapGrd', '#wrapEmptyBags', '#wrapGp'].forEach(b => PurchaseChrome.fullscreen(b, 'grid'));
        }
        $('btnNew').onclick = () => PP.withButton($('btnNew'), reset);
        $('btnSave').onclick = () => PP.withButton($('btnSave'), btnSaveClick);
        $('btnUpdate').onclick = () => PP.withButton($('btnUpdate'), btnUpdateClick);
        $('printToolStripButton').onclick = () => generateReport(Id);
        $('btnAttachment').onclick = btnAttachmentClick;
        $('toolStripButton1').onclick = () => PP.withButton($('toolStripButton1'), toolStripButton1Click);
        $('CityDefine').onclick = () => { if (!window.open('/master-data/city', '_blank')) show('The browser blocked the Define City window. Allow pop-ups for this site and retry.'); };
        $('btnLoadAll').onclick = () => PP.withButton($('btnLoadAll'), () => historyGridFill(0));
        $('btnGrnFormHistory').onclick = btnGrnFormHistoryClick;
        $('Add').onclick = addClick; $('btnUpdateDetail').onclick = btnUpdateDetailClick; $('btnCancelUpdateDetial').onclick = btnCancelUpdateDetialClick;
        $('attClose').onclick = () => { $('attModal').style.display = 'none'; };
        ['txtqty', 'txtebu', 'txtgwt'].forEach(i => $(i).addEventListener('input', total));                    // TextChanged -> Total()
        PP.onComboLeave('combpckuom', total);                                                                   // combpckuom_Leave
        PP.onComboLeave('combitem', () => combitemLeave().catch(PP.error));                                     // combitem_Leave
        PP.onComboLeave('combpcktyp', combpcktypLeave);                                                         // combpcktyp_Leave
        $('txtvehno').addEventListener('input', e => { const t = e.target; t.value = t.value.toUpperCase(); t.setSelectionRange(t.value.length, t.value.length); });
        const eb = $('grdEmptyBags').querySelector('tbody');
        eb.addEventListener('change', e => {
            const f = e.target.closest('[data-eb]'); if (!f) return;
            const i = Number(f.closest('tr').dataset.i), key = f.dataset.eb;
            dtEmptyBag[i][key] = (key === 'Remarks') ? f.value : (key === 'RecQty' || key === 'PurQty') ? PP.num(f.value) : Number(f.value);
            emptyBagCellUpdated(i, key);
        });
        eb.addEventListener('click', e => {                                                                      // grdEmptyBags_ColumnButtonClick (:2211)
            const b = e.target.closest('[data-ebact]'); if (!b) return;
            const i = Number(b.closest('tr').dataset.i);
            if (b.dataset.ebact === 'Delete') { dtEmptyBag.splice(i, 1); if (dtEmptyBag.length === 0) dtEmptyBag.push(blankBag()); }
            else dtEmptyBag.push(blankBag());
            renderEmptyBags();
        });
        document.addEventListener('keydown', onKey);
        load().catch(PP.error);
    });
}());
