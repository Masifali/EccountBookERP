/* Screen 469 "Goods Receiving Notes For Ghalla Mandi" - Architecture.WinApp.Paddy_Purchase/frmGrnForGhallaMandi.cs (InvGrn DocumentTypeId 167).
   Line numbers in the comments are to that file. */
(function () {
    'use strict';
    const API = '/api/paddy-purchase/grn-ghalla';
    const $ = PP.$;
    let Id = 0;                      // Id (int field): ReadById_Update / history Edit set it, btnSave_Click zeroes it, reset() leaves it
    let GPID = 0;                    // GPID: the gate pass picked from grdGp
    let updateDetailIndex = -1;
    let cfg = { juteBagsCut: 0, ppBagsCut: 0, openBulkCut: 0, emptyBagsWeightCutEditable: false, defaultJobLot: 0, defaultPackingType: 0 };
    let lookups = {};
    let rights = { canSave: false, canUpdate: false, canPrint: false };
    let dtDetail = [];               // dtdetail rows (ItemId, Item, CropYear, JobId, Job, PackingTypeId, PackingType, UOM, UOMId, Qty, GrossWeight, EbUnit, EbTotal, NetWeight, StockWeight, WareHouse, City, ContractorId, Contractor)
    let dtEmptyBag = [];             // dtEmptyBag rows (OrderId, Type, ItemId, Condition, RecQty, PurQty, Remarks)
    let uomRows = [];                // combpckuom data source (Id, UOMCode, Equivalent): Cells[2] = Equivalent is the pack weight
    let uomGen = 0, historyGen = 0;

    function show(msg) { alert(msg); }
    const blankBag = () => ({ OrderId: 0, Type: 0, ItemId: 0, Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' });
    const g = (row, key) => { if (!row) return null; for (const x in row) if (x.toLowerCase() === key.toLowerCase()) return row[x]; return null; };
    const str = v => (v == null ? '' : String(v));
    const r2 = x => String(Math.round((x + Number.EPSILON) * 100) / 100);    // Math.Round(x, 2).ToString()
    function focusCombo(id) { const w = $(id).parentElement && $(id).parentElement.querySelector('.dtcombo-input'); (w || $(id)).focus(); }
    const btnVisible = id => $(id).style.display !== 'none';
    /* A TextBox.Text assignment: TextChanged (txtqty, txtebu, txtgwt -> Total()) only fires when the text really changes. */
    function put(id, v) {
        const el = $(id), nv = str(v), changed = el.value !== nv;
        el.value = nv;
        if (changed && (id === 'txtqty' || id === 'txtebu' || id === 'txtgwt')) total();
    }

    /* ---------- InvFrmGRN_Load (:1481) ---------- */
    async function load() {
        const d = await PP.fetch(API + '/init');
        rights = d; cfg = Object.assign(cfg, d.config || {}); lookups = d;
        $('txtebu').value = '0';
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
        PP.bind('CmbGhallaMandi', lookups.ghallaMandi, 'id', 'name');                            // GhallaMandiBind
        PP.bind('CmbLoadingContractor', lookups.loadingContractors, 'id', 'name');               // LoadingContractorBind
        PP.bind('CmbTransport', lookups.transporters, 'id', 'name');                             // TransportFill (group 17)
        PP.bind('combitem', lookups.items, 'id', 'name');                                        // ItemsBind
        PP.bind('combvehtyp', lookups.vehicleTypes, 'id', 'name');                               // VehicleTypesFill
        PP.bind('combcropyear', lookups.cropYears, 'id', 'name', { activateFirst: true });       // CropYear
        PP.bind('combjoblot', lookups.jobLots, 'id', 'name');                                    // JobLotFillWithoutOrder
        PP.bind('combpcktyp', lookups.packingTypes, 'id', 'name', { activateFirst: true });      // PackingTypeFill
        PP.bind('combwr', lookups.warehouses, 'id', 'name', { activateFirst: true });            // WarehouseFill
        PP.bind('CmbCity', lookups.cities, 'id', 'name');                                        // CityFill
    }
    function defaultConfiquration() {                                                            // (:750): Job/Lot and Paking Type only
        if (cfg.defaultJobLot > 0) PP.setSel('combjoblot', cfg.defaultJobLot);
        if (cfg.defaultPackingType > 0) PP.setSel('combpcktyp', cfg.defaultPackingType);
    }

    /* ---------- grids ---------- */
    const wareName = id => { const w = (lookups.warehouses || []).find(x => String(x.id) === String(id)); return w ? w.name : str(id); };
    function renderDetail() {                                  // grdSettings (:1351): hidden ids, WareHouse Name / City Name captions, sums, "#,##.##"
        const rows = dtDetail.map((r, i) => ({ _i: i, Item: r.Item, CropYear: r.CropYear, Job: r.Job, PackingType: r.PackingType, UOM: r.UOM, Qty: PP.num(r.Qty), GrossWeight: PP.num(r.GrossWeight),
            EbUnit: PP.num(r.EbUnit), EbTotal: PP.num(r.EbTotal), NetWeight: PP.num(r.NetWeight), StockWeight: PP.num(r.StockWeight), WareHouse: wareName(r.WareHouse), City: r.City, Contractor: r.Contractor }));
        const two = { Qty: 2, GrossWeight: 2, EbUnit: 2, EbTotal: 2, NetWeight: 2, StockWeight: 2 };
        PP.renderGrid('grd', rows, { hide: ['_i'], caption: { WareHouse: 'WareHouse Name', City: 'City Name' }, numeric: Object.keys(two), totals: two, dec: two,
            post: [{ key: 'Delete', label: 'X', onclick: row => { dtDetail.splice(row._i, 1); renderDetail(); } }],
            onDbl: row => grd_DoubleClick(row._i) });
    }
    function renderGp(rows) {                                  // GRNGridSetting (:954)
        const z = { ItemQty: 0, FactoryWeight: 0 };
        PP.renderGrid('grdGp', rows, { hide: ['Id', 'LoadingContractorId', 'GhallaMandiId', 'CarriageContractorId', 'NoOfPackages'], dateCols: ['GpDate'], link: ['GpSrNo'], numeric: Object.keys(z), totals: z, dec: z,
            post: [{ key: 'Load', label: 'Load', onclick: r => loadGp(r) }],
            onLink: (k, r) => printGatePass(r.Id) });
    }
    function renderEmptyBags() {                               // grdEmptyBagsSettings (:1450)
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
    /* grdEmptyBags_CellUpdated (:2315) and CalculationPurchaseAgainstWeight (:2307) are empty methods in this form: no cell rules. */

    /* ---------- weights: EmptyBagsDeductionsApply (:2249), GrossWeightCalculate (:1199), Total (:1220) ---------- */
    function packUomWeight() {                                 // combpckuom.SelectedRow.Cells[2] (Equivalent); 0 when there is no active row
        const v = $('combpckuom').value; if (v === '' || v == null) return 0;
        const r = uomRows.find(x => String(x.Id) === String(v));
        return r ? PP.num(r.Equivalent) : 0;
    }
    function applyEb() {                                       // returns true when txtebu.Text changed
        const t = Math.trunc(PP.num($('combpcktyp').value)), before = $('txtebu').value;
        if (t === 1) $('txtebu').value = String(cfg.juteBagsCut);
        if (t === 2) $('txtebu').value = String(cfg.ppBagsCut);
        if (t === 5) $('txtebu').value = String(cfg.openBulkCut);
        return $('txtebu').value !== before;
    }
    function emptyBagsDeductionsApply() { if (applyEb()) total(); }
    function grossWeightCalculate() {
        const q = $('txtqty').value;
        if (q !== '') put('txtgwt', String(PP.num(q) * packUomWeight()));
    }
    function total() {
        applyEb();
        const PackUOM = packUomWeight();
        const Qty = $('txtqty').value !== '' ? PP.num($('txtqty').value) : 0;
        const GrossWeight = $('txtgwt').value !== '' ? PP.num($('txtgwt').value) : 0;
        let EBUnit = 0, EBTotal = 0;
        $('txtnetwt').value = (Qty > 0 && PackUOM > 0) ? String(Qty * PackUOM) : '0';
        if ($('txtebu').value !== '') EBUnit = PP.num($('txtebu').value);
        if (EBUnit > 0 && Qty > 0) EBTotal = EBUnit * Qty;
        $('txtEmptyBagsTotal').value = r2(EBTotal);
        $('txtStockWeight').value = r2(GrossWeight - EBTotal);
    }

    /* ---------- gate pass pick (grdGp "Load" / Ctrl+Enter) = GatePassRecordFill (:1160), filled from the grdGp current row ---------- */
    function loadGp(row) {
        if (!(btnVisible('btnSave') && !$('btnSave').disabled)) return;
        GPID = Math.trunc(PP.num(g(row, 'Id')));
        $('combgatepass').value = str(g(row, 'GpSrNo'));
        $('txtfctwt').value = str(g(row, 'FactoryWeight'));
        PP.setSel('CmbLoadingContractor', Math.trunc(PP.num(g(row, 'LoadingContractorId'))));
        PP.setSel('CmbGhallaMandi', Math.trunc(PP.num(g(row, 'GhallaMandiId'))));
        PP.setSel('CmbTransport', Math.trunc(PP.num(g(row, 'CarriageContractorId'))));
        PP.setSelText('combvehtyp', str(g(row, 'VehicleType')));
        $('txtvehno').value = str(g(row, 'VehicleNo'));
        $('txtbltyno').value = str(g(row, 'BiltyNo'));
        $('txtremarks').value = str(g(row, 'OtherRemarks'));
        $('txtcarramount').value = str(g(row, 'NetPaid'));
        put('txtqty', str(g(row, 'NoOfPackages')));
    }
    /* combitem_Leave (:1183) with PackUOMFillWithoutOrder (:1064) */
    async function combitemLeave() {
        if (Math.trunc(PP.num($('combgatepass').value)) > 0) await packUomFill();
        emptyBagsDeductionsApply();
    }
    async function packUomFill() {
        const gen = ++uomGen, item = $('combitem').value;
        const rows = item === '' ? [] : await PP.fetch(API + '/uoms?itemId=' + encodeURIComponent(item));
        if (gen !== uomGen) return;
        uomRows = rows;
        PP.bind('combpckuom', rows, 'Id', 'UOMCode');          // combpckuom.Text cleared and DataSource null when there are no rows
    }

    /* ---------- detail row editing ---------- */
    function formValidationDetail() {                          // (:689)
        const fail = (msg, id) => { show(msg); focusCombo(id); return false; };
        if (!$('combitem').value) return fail('Item Name Field is Required', 'combitem');
        if (!$('combcropyear').value) return fail('Crop Year Field is Required', 'combcropyear');
        if (!$('combjoblot').value) return fail('Job/Lot Field is Required', 'combjoblot');
        if (!$('combpcktyp').value) return fail('Packing Type Field is Required', 'combpcktyp');
        if ($('txtqty').value.trim() === '' || PP.num($('txtqty').value) === 0) { show('Qty Field is Required'); $('txtqty').focus(); return false; }
        if (!$('combpckuom').value) return fail('Pack Unit Field is Required', 'combpckuom');
        if ($('txtgwt').value.trim() === '' || PP.num($('txtgwt').value) === 0) { show('Gross Weight Field is Required'); $('txtgwt').focus(); return false; }
        if ($('txtnetwt').value.trim() === '' || PP.num($('txtnetwt').value) === 0) { show('Net Bill Weight Field is Required'); $('txtnetwt').focus(); return false; }
        if (!$('combwr').value) return fail('Warehouse Field is Required', 'combwr');
        if (!$('CmbCity').value) return fail('City Field is Required', 'CmbCity');
        if (!$('CmbContractor').value) return fail('Contractor Field is Required', 'CmbContractor');
        return true;
    }
    function rowFromBoxes() {
        return { ItemId: $('combitem').value, Item: PP.selText('combitem'), CropYear: PP.selText('combcropyear'), JobId: $('combjoblot').value, Job: PP.selText('combjoblot'),
            PackingTypeId: $('combpcktyp').value, PackingType: PP.selText('combpcktyp'), UOM: PP.selText('combpckuom'), UOMId: $('combpckuom').value,
            Qty: PP.num($('txtqty').value), GrossWeight: PP.num($('txtgwt').value), EbUnit: PP.num($('txtebu').value),
            EbTotal: PP.num($('txtEmptyBagsTotal').value), NetWeight: PP.num($('txtnetwt').value), StockWeight: PP.num($('txtStockWeight').value), WareHouse: $('combwr').value,
            City: PP.selText('CmbCity'), ContractorId: $('CmbContractor').value, Contractor: PP.selText('CmbContractor') };
    }
    function resetDetial() {                                   // ResetDetial (:1333): each assignment fires TextChanged -> Total() when the text changes
        put('txtqty', ''); put('txtgwt', ''); put('txtebu', ''); put('txtEmptyBagsTotal', ''); put('txtnetwt', '');
        PP.setSel('combpckuom', '');
        focusCombo('combitem');
    }
    function addClick() {                                      // Add_Click (:289)
        if (!formValidationDetail()) return;
        if (PP.num($('txtnetwt').value) > PP.num($('txtgwt').value)) { show('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        dtDetail.push(rowFromBoxes());
        renderDetail(); total(); resetDetial();
    }
    async function grd_DoubleClick(i) {                        // (:1917)
        const item = dtDetail[i]; if (!item) return;
        updateDetailIndex = i;
        PP.setSel('combitem', item.ItemId);
        await packUomFill();
        PP.setSelText('combcropyear', item.CropYear);
        PP.setSel('combjoblot', item.JobId); PP.setSel('combpcktyp', item.PackingTypeId);
        put('txtqty', str(item.Qty));
        PP.setSel('combpckuom', item.UOMId);
        put('txtgwt', str(item.GrossWeight));
        $('txtStockWeight').value = str(item.StockWeight);
        put('txtebu', str(item.EbUnit));
        $('txtEmptyBagsTotal').value = str(item.EbTotal); $('txtnetwt').value = str(item.NetWeight);
        PP.setSel('combwr', item.WareHouse); PP.setSelText('CmbCity', item.City); PP.setSel('CmbContractor', item.ContractorId);
        $('Add').style.display = 'none'; $('btnUpdateDetail').style.display = ''; $('btnCancelUpdateDetial').style.display = '';
        focusCombo('combitem');
    }
    function btnUpdateDetailClick() {                          // (:1952)
        if (!formValidationDetail()) return;
        if (PP.num($('txtnetwt').value) > PP.num($('txtgwt').value)) { show('NetBillWeight cannot be greater than GrossWeight Please check'); return; }
        if (!dtDetail[updateDetailIndex]) return;
        dtDetail[updateDetailIndex] = rowFromBoxes();
        $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none';
        renderDetail(); resetDetial();
    }
    function btnCancelUpdateDetialClick() { $('Add').style.display = ''; $('btnUpdateDetail').style.display = 'none'; $('btnCancelUpdateDetial').style.display = 'none'; }

    /* ---------- ReadById_Update (:563) ---------- */
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
            PP.setSel('CmbLoadingContractor', g(h, 'SupplierCustomerId')); PP.setSel('CmbGhallaMandi', g(h, 'GhallaMandiId'));
            // the form saves SelectedRow.Cells[2] (the party's GL account) as TransporterId, so the stored value is matched back on that column
            const tid = Math.trunc(PP.num(g(h, 'TransporterId'))), tr = tid > 0 ? (lookups.transporters || []).find(x => Math.trunc(PP.num(x.glAccountId)) === tid) : null;
            PP.setSel('CmbTransport', tr ? tr.id : '');
            $('txtcarramount').value = str(g(h, 'CarriageAmount')); $('txtfctwt').value = str(g(h, 'FactoryWeight'));
            $('combgatepass').value = str(g(h, 'GpNo'));
            PP.setSelText('combvehtyp', str(g(h, 'VehicleType'))); $('txtvehno').value = str(g(h, 'VehicleNo')); $('txtbltyno').value = str(g(h, 'BiltyNo'));
            $('txtremarks').value = str(g(h, 'RemarksHeader'));
            GPID = Math.trunc(PP.num(g(h, 'InwardGatePassId')));
            dtDetail = d.details || [];
            dtEmptyBag = (d.emptyBags || []);
            renderDetail();
            if (!dtEmptyBag.length) dtEmptyBag = [blankBag()];
            renderEmptyBags();
        } catch (e) { PP.error(e); }
    }

    /* ---------- reset (:1283): same assignment order, so the TextChanged -> Total() calls fire as on the desktop; Ghalla Mandi and Status are left alone ---------- */
    async function reset() {
        GPID = 0;
        PP.setSel('CmbLoadingContractor', ''); PP.setSel('CmbTransport', '');
        $('txtcarramount').value = ''; $('txtfctwt').value = ''; $('combgatepass').value = '';
        const vt = $('combvehtyp'); if (vt.options.length > 1) vt.selectedIndex = 1;                        // combvehtyp.Rows[0].Activate()
        $('txtvehno').value = ''; $('txtbltyno').value = ''; $('txtremarks').value = '';
        PP.setSel('combitem', '');
        put('txtqty', ''); put('txtebu', ''); $('txtEmptyBagsTotal').value = ''; $('txtnetwt').value = ''; $('txtStockWeight').value = '';
        PP.setSel('CmbCity', ''); PP.setSel('combpckuom', '');
        put('txtgwt', '');
        uomRows = []; PP.bind('combpckuom', [], 'Id', 'UOMCode');                                           // DataSource = null
        $('btnSave').style.display = ''; $('btnUpdate').style.display = 'none';
        dtDetail = []; renderDetail();
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

    /* ---------- Insert (:315): FormValidation, grid, confirm, then the save chain on the server ---------- */
    function formValidation() {                                // (:626)
        const t = $('txtdocno').value.trim();
        if (t === '' || t === '0') { show('DocNo Field is Required'); return false; }
        if (!$('combbrnch').value) { show('Branch Field is Required'); focusCombo('combbrnch'); return false; }
        if (!$('combproject').value) { show('Project Field is Required'); focusCombo('combproject'); return false; }
        if (!$('CmbLoadingContractor').value) { show('Loading Contractor Field is Required'); focusCombo('CmbLoadingContractor'); return false; }
        if (!$('CmbGhallaMandi').value) { show('Ghalla_Mandi Field is Required'); focusCombo('CmbGhallaMandi'); return false; }
        if (Math.trunc(PP.num($('combgatepass').value.trim())) === 0) { show('Gatepass Field is Required'); $('combgatepass').focus(); return false; }
        if (PP.num($('txtfctwt').value.trim()) === 0) { show('Factory Weight Field is Required'); $('txtfctwt').focus(); return false; }
        if (!$('CmbStatus').value) { show('Status Field is Required'); focusCombo('CmbStatus'); return false; }
        return true;
    }
    async function insert() {
        if (!formValidation()) return;
        if (dtDetail.length === 0) { show('Grid Record Not Found'); return; }
        if (!PP.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        const carriage = PP.num($('txtcarramount').value.trim());
        let transporterId = 0;
        if (carriage > 0) {
            if (!$('CmbTransport').value) { show('Transporter Account field required'); focusCombo('CmbTransport'); return; }
            const tr = (lookups.transporters || []).find(x => String(x.id) === String($('CmbTransport').value));
            transporterId = tr ? Math.trunc(PP.num(tr.glAccountId)) : 0;                                      // SelectedRow.Cells[2]
        }
        const body = {
            id: Id, docNo: $('txtdocno').value.trim(), docDate: $('DocDate').value, branchId: $('combbrnch').value, projectId: $('combproject').value,
            loadingContractorId: $('CmbLoadingContractor').value, ghallaMandiId: $('CmbGhallaMandi').value,
            gpNo: $('combgatepass').value.trim(), gpId: GPID, factWeight: $('txtfctwt').value.trim(),
            vehicleType: PP.selText('combvehtyp'), vehicleNo: $('txtvehno').value.trim(), biltyNo: $('txtbltyno').value.trim(), status: PP.selText('CmbStatus'),
            transporterId: transporterId, carriage: $('txtcarramount').value.trim(), remarks: $('txtremarks').value.trim(), details: dtDetail, emptyBags: dtEmptyBag
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
    async function historyGridFill(n) {                       // HistoryGridFill (:1566)
        const rows = await PP.fetch(API + '/history?noOfRecords=' + (n || 0));
        PP.renderGrid('GrdHistory', rows, { hide: ['Id', 'InwardGatePassId'], dateCols: ['DocDate', 'EntryDate', 'ModifyDate'], link: ['GpNo', 'NoOfAttachments'], linkAlways: ['NoOfAttachments'], frozen: 2,
            numeric: ['FactoryWeight', 'PartyWeight', 'Freight'], dec: { FactoryWeight: 3, PartyWeight: 3, Freight: 3 },
            pre: [{ key: 'Edit', label: 'Edit', onclick: r => { readById_Update(r.Id); } }, { key: 'Print', label: 'Print', onclick: r => generateReport(r.Id) }],
            onRow: r => history_SelectionChanged(r), onDbl: r => readById_Update(r.Id),
            onLink: (k, r) => { if (k === 'NoOfAttachments') showAttachments(r.Id); else printGatePass(r.InwardGatePassId); } });
        if (!rows.length) PP.renderGrid('GrdHistoryDetail', [], {}); else history_SelectionChanged(rows[0]);
    }
    async function history_SelectionChanged(row) {            // GrdHistory_SelectionChanged (:2335)
        if (!row) return;
        const gen = ++historyGen;
        try {
            const rows = await PP.fetch(API + '/history-detail/' + row.Id);
            if (gen !== historyGen) return;
            const z = { Qty: 0, EbUnit: 0, GrossWeight: 0, EbTotal: 0, NetWeight: 0, StockWeight: 0 };       // gridhistorydetail (:1855): "0,0" sums
            PP.renderGrid('GrdHistoryDetail', rows, { caption: { WareHouse: 'WareHouse Name' }, numeric: Object.keys(z), totals: z, dec: z });
        } catch (e) { PP.error(e); }
    }

    /* ---------- print / attachments / misc ---------- */
    function generateReport(printId) {                         // GenerateReport (:2017) -> CommonServices.GrnSlipWithSubReports(PrintId): DocumentTypeId 0 is sent as 46
        if (!printId) { show('No Record Found For Display'); return; }
        if (window.printRpt) window.printRpt('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: Number(printId), documentTypeId: 46 }); else show('The print runtime is not loaded on this page.');
    }
    function printGatePass(gpId) {                             // CommonServices.GatePassInwardWithDetailAndLabReport(Id)
        if (!gpId) { show('No Record Found For Display'); return; }
        if (window.printRpt) window.printRpt('256-GatePassInward_WithDetailSlip.rpt', { id: Number(gpId) }); else show('The print runtime is not loaded on this page.');
    }
    async function showAttachments(id) {                       // GrdHistory_LinkClicked (:2079): the stored attachments of the record
        try {
            const rows = await PP.fetch(API + '/' + id + '/attachments');
            $('attBody').innerHTML = rows.length ? rows.map(r => '<div><a href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank" rel="noopener">' + PP.esc(str(r.Attachment).split(/[\\/]/).pop()) + '</a></div>').join('') : '<div>No attachments</div>';
            $('attModal').style.display = 'flex';
        } catch (e) { PP.error(e); }
    }
    async function btnAttachmentClick() {                      // btnAttachment_Click (:1830): AT.Show()
        if (Id > 0) return showAttachments(Id);
        show('Attachments can be viewed for a saved record here. Adding files from the web is not available yet.');
    }
    async function toolStripButton1Click() {                   // toolStripButton1_Click (:1994)
        const d = await PP.fetch(API + '/lookups'); lookups = Object.assign(lookups, d); cfg = Object.assign(cfg, (await PP.fetch(API + '/config')));
        bindAll(false); renderEmptyBags();
    }
    async function openIfViewRight(screen, url) {              // btnGrnFormHistory_Click (:2275)
        const w = window.open('about:blank', '_blank');
        try {
            const r = await PP.fetch('/api/purchase/screen-view-right/' + screen);
            if (r && r.view) { if (w) w.location.href = url; else window.open(url, '_blank'); } else { if (w) w.close(); show('Please Check Screen Rights'); }
        } catch (e) { if (w) w.close(); show(e.message); }
    }

    /* ---------- tabs and keyboard ---------- */
    function isHistory() { return $('viewHistory').style.display !== 'none'; }
    function switchView(mode) {                                // tabControl1_SelectedIndexChanged (:1888): History loads 50 rows
        const h = mode === 'History';
        $('viewForm').style.display = h ? 'none' : 'block'; $('viewHistory').style.display = h ? 'block' : 'none';
        if (h) historyGridFill(50).catch(PP.error);
    }
    function closeForm() { if (window.opener) window.close(); else history.back(); }
    function onKey(e) {                                        // InvFrmGRN_KeyDown (:1659)
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
        $('txtqty').addEventListener('blur', grossWeightCalculate);                                             // txtqty_Leave
        PP.onComboLeave('combpckuom', () => { grossWeightCalculate(); total(); });                              // combpckuom_Leave
        PP.onComboLeave('combitem', () => combitemLeave().catch(PP.error));                                     // combitem_Leave
        PP.onComboLeave('combpcktyp', emptyBagsDeductionsApply);                                                // combpcktyp_Leave
        $('txtvehno').addEventListener('input', e => { const t = e.target; t.value = t.value.toUpperCase(); t.setSelectionRange(t.value.length, t.value.length); });
        const eb = $('grdEmptyBags').querySelector('tbody');
        eb.addEventListener('change', e => {
            const f = e.target.closest('[data-eb]'); if (!f) return;
            const i = Number(f.closest('tr').dataset.i), key = f.dataset.eb;
            dtEmptyBag[i][key] = (key === 'Remarks') ? f.value : (key === 'RecQty' || key === 'PurQty') ? PP.num(f.value) : Number(f.value);
            renderEmptyBags();                                                                                   // grdEmptyBags_CellUpdated is empty: only the footer sums refresh
        });
        eb.addEventListener('click', e => {                                                                      // grdEmptyBags_ColumnButtonClick (:2116)
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
