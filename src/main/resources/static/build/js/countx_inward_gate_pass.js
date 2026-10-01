    /* Inward Gate Pass (screen 134, DocumentTypeId 51) - Architecture.WinApp.Purchase/InwardGatePass.cs.
       Line references below are to that file. */
    const context = { documentTypeId: 51 };
    let loadedHeader={}, loadedBreakups=[], lookupData={}, driverBioId=0, formGeneration=0, orderGeneration=0;
    let breakupLocked=false, netPaidEdited=false, driverGeneration=0, transitGeneration=0, transitRows=[];
    let poId=0;                 // POId
    let actionIdForSpecialApproval=0;
    const field=id=>document.getElementById(id);
    const numeric=id=>Number(field(id).value)||0;
    const caption=id=>field(id).selectedOptions[0]?.textContent.trim()||'';
    const lowerKeys=row=>Object.fromEntries(Object.entries(row||{}).map(([key,value])=>[({UOM:'uom',EBTotal:'ebTotal'}[key])||key[0].toLowerCase()+key.slice(1),value]));
    const localStamp=(date=new Date())=>new Date(date.getTime()-date.getTimezoneOffset()*60000).toISOString();
    const escapeHtml=value=>String(value??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
    function setCaption(id,text) { const select=field(id); const option=Array.from(select.options).find(o=>o.textContent.trim()===String(text||'')); select.value=option?option.value:''; }
    async function igpFetch(url,options) {
        return PurchaseRequest.track(async()=>{
            const response=await fetch(url,options);
            const text=await response.text(); let data;
            try { data=text?JSON.parse(text):null; } catch (_) { throw Error('The session expired or the server returned an invalid response. Please sign in and retry.'); }
            if(!response.ok) throw Error(data?.message||data?.detail||'Request failed ('+response.status+')');
            return {json:()=>Promise.resolve(data)};
        });
    }
    function showRequestError(error) { alert(error?.message||'Request failed. Please retry.'); }
    async function withButtonLoading(btn,work) {
        try { return await PurchaseRequest.run(btn,work); } catch(error) { showRequestError(error); }
    }
    /* tabControl1 "Form" / "History": the History button sits on the RIGHT of a fixed footer (countx_purchase_page_chrome.js,
       as on the other Supplier Purchases forms); the grids get a full-screen toggle and scroll inside themselves. */
    function installChrome() {
        if (!window.PurchaseChrome) return;
        const isHistory=()=>field('viewHistory').style.display!=='none';
        PurchaseChrome.footer({ isHistory, toggle:()=>switchViewMode(isHistory()?'Form':'History'), watch:'#viewHistory' });
        field('bottomModeBar').style.display='none';
        for (const [box,caption] of [['#wrapMainHistory','open gate passes'],['#wrapPoInfo','purchase orders'],['#wrapFullHistory','history']]) PurchaseChrome.fullscreen(box,caption);
    }
    document.addEventListener('DOMContentLoaded',async()=>{
        installChrome();
        try {
            setDefaultDates(); await loadDropdowns();
            const id=Number(new URLSearchParams(location.search).get('id'));
            if(id>0) await loadRecordAndEdit(id); else { await onNewRecord(true); field('cmbWeighBridge').disabled=true; /* designer: Enabled=false until Reset */ }
            await loadMainHistoryGrid();
        } catch(error) { showRequestError(error); }
    });

    function setDefaultDates() {
        const today = localStamp().split('T')[0];
        field('txtgpdate').value = today;
        field('txtBiltyDate').value = today;
        field('txtToPoDate').value = today;
        const threeDaysAgo = new Date(); threeDaysAgo.setDate(threeDaysAgo.getDate() - 3);
        const threeDaysAgoIso = localStamp(threeDaysAgo).split('T')[0];
        field('txtHistFromDate').value = threeDaysAgoIso;   // txtFromDateHistory = Now - 3 days
        field('txtHistToDate').value = today;
        field('txtFromPoDate').value = threeDaysAgoIso;     // FromDateHistoryPoInfo = Now - 3 days (OrderTypeFill)
        const nowIso = localStamp().slice(0, 16);
        field('txtintime').value = nowIso;
        field('txtouttime').value = nowIso;
    }

    function loadDropdowns() {
        const selected=Object.fromEntries(['cmbcity','cmbvehicletype','cmbgptype','cmbWeighBridge','CmbPackingType','CmbOrderType','CmbStatus'].map(id=>[id,field(id).value]));
        return igpFetch('/api/inward-gate-pass/dropdowns')
            .then(res => res.json())
            .then(data => {
                lookupData=data;
                bindSelect('CmbOrderType',data.orderTypes,'id','name');
                bindSelect('cmbcity', data.cities, 'id', 'name');
                bindSelect('cmbvehicletype', data.vehicleTypes, 'id', 'name');
                bindSelect('cmbgptype', data.gatePassTypes, 'id', 'name');
                bindSelect('cmbWeighBridge', data.weighBridges, 'id', 'name');
                bindSelect('CmbPackingType', data.packingTypes, 'id', 'name');
                bindSelect('CmbStatus', data.statuses, 'id', 'name');
                bindSelect('cmbTransitVehicle', data.transitVehicles, 'id', 'name');
                bindSelect('cmbHistSupplier', data.historySuppliers, 'id', 'name');        // HistoryComboFill
                // Load: ItemNameFill() then gatepasstype() -> cmbgptype_Leave while CmbOrderType is still unbound (Value 0),
                // so CmbVariety gets every item (ReadAllItemsIncludedPM) with no row selected.
                if (!field('CmbVariety').options.length || field('CmbVariety').options.length<=1) bindSelect('CmbVariety', data.items, 'id', 'name');
                bindSelect('cmbSupplierNamePoInfo', data.poSuppliers, 'id', 'name');      // OrderInformationComboFill
                bindSelect('CmbDocumentTypePoInfo', data.documentTypes, 'id', 'name');
                field('tabBtnPoInfo').style.display=data.poInfoDocumentTypeId?'':'none';  // tabPage4 only for 41/1500
                field('PanelBreakup').style.display=data.showPurchaseBreakup?'':'none';   // PanelBreakup only for 105
                field('lblIsApproved').style.display=data.canApprove?'':'none';          // ChkIsApproved.Visible
                field('btnsave').disabled=!data.canSave; field('btnupdate').disabled=!data.canUpdate; field('btnPrint').disabled=!data.canPrint;
                field('BtnDriverForm').dataset.allowed=data.canDriverBio?'1':'0';   // View right of "frmDriverBioForInWard"

                if(field('cmbgptype').options.length>2) field('cmbgptype').selectedIndex=2;          // gatepasstype(): Rows[2]
                if(field('cmbcity').options.length>1) field('cmbcity').selectedIndex=1;              // CityFill(): Rows[1]
                if(Number(data.defaultCityId)>0) field('cmbcity').value=String(data.defaultCityId);  // config "City Area"
                if(field('cmbvehicletype').options.length>1) field('cmbvehicletype').selectedIndex=1;
                if(field('CmbOrderType').options.length>1) field('CmbOrderType').selectedIndex=1;    // OrderTypeFill(): Rows[1]
                if(field('CmbStatus').options.length) field('CmbStatus').value='Open';
                if(field('cmbWeighBridge').options.length>1) field('cmbWeighBridge').selectedIndex=1;
                for(const [id,value] of Object.entries(selected)) if(value && Array.from(field(id).options).some(o=>o.value===value)) field(id).value=value;
            });
    }

    function bindSelect(elemId, list, valKey, textKey) {
        const sel = field(elemId);
        if (!sel) return;
        sel.innerHTML = '<option value="">-- Select --</option>';
        (list||[]).forEach(item => {
            const opt = document.createElement('option');
            opt.value = item[valKey];
            opt.textContent = item[textKey];
            sel.appendChild(opt);
        });
    }

    function switchViewMode(mode) {
        const history = mode === 'History';
        field('viewForm').style.display = history ? 'none' : 'block';
        field('viewHistory').style.display = history ? 'block' : 'none';
        field('btnModeForm')?.classList.toggle('active', !history);
        field('btnModeHistory')?.classList.toggle('active', history);
        field('lblFormHeaderTitle').textContent = history ? 'Inward Gate Pass History' : 'Inward Gate Pass';
        if (history) field('txtHistFromDate').focus();   // tabControl1_SelectedIndexChanged: focus only, no search
    }

    function onSwitchToFormNew() { switchViewMode('Form'); return onNewRecord(); }

    /* Reset() :2900 (btnnew). firstLoad = InwardGatePass_Load :643, which binds every item and leaves ChkIsApproved checked. */
    function onNewRecord(firstLoad) {
        if (firstLoad!==true) firstLoad=false;
        const generation=++formGeneration; ++orderGeneration;
        loadedHeader={}; loadedBreakups=[emptyBreakup()]; driverBioId=0; breakupLocked=false; netPaidEdited=false; poId=0; actionIdForSpecialApproval=0;
        renderBreakups(); bindSelect('cmbTransitVehicle',[],'id','name'); transitRows=[];
        // The searchable combos (countx_desktop_combo.js) draw a text input.dtcombo-input over each <select>; clearing those
        // blanked G.P Type / City / Vehicle Type although the selects still held Rows[2] / Rows[1] (the reported blank combos).
        document.querySelectorAll('#viewForm input:not([type="radio"]):not([type="checkbox"]):not(.dtcombo-input)').forEach(input=>input.value=input.defaultValue||'');
        setDefaultDates();
        field('txtId').value='0';
        for (const id of ['txtqty','txtPackUnit','txtfreight','txtAdvanceByParty','txtAdvanceByFactory','txtTotalPayablesFreight','txtNetPaid',
                          'txtSupplierNetWeight','txtSupplierFirstWeight','txtSupplierSecondWeight','txtFactoryWeight','txtsecondWeight','txtFactoryNetWeight',
                          'txtDifferenceWeight','txtWeighBridgeSlipNo','txtremarks','txtAccessWeight','txtvehicleno','txtbiltyno','txtWeightDiffRemarks']) field(id).value='';
        field('txtWeight').value='0';
        field('rowWeightDiffRemarks').style.display='none';
        for (const id of ['txtSupplierNetWeight','txtfreight','txtNetPaid','txtAdvanceByFactory','txtAdvanceByParty','CmbOrderType','txtPackUnit','CmbOrderno','cmbWeighBridge','txtvehicleno','txtSupplierFirstWeight','txtSupplierSecondWeight']) field(id).disabled=false;
        field('cmbTransitVehicle').disabled=true;
        field('RadGrnFormToOpenOnInsert').style.display='none';   // Reset(): Visible = false; its Checked is cleared after Save/Update (below)
        // ChkIsApproved: Load sets Checked = true (:653); Reset() does not touch it, so it keeps the operator's / loaded value.
        if (firstLoad) field('ChkIsApproved').checked=true;
        field('ChkIsApproved').disabled=false;                  // txtAccessWeight.Text = "" -> txtAccessWeight_TextChanged
        resetDriverInfoFields();
        // Reset(): CmbVariety.DataSource = null and cmbsupp.DataSource = null; CmbOrderType_Leave (below) rebinds them per type.
        if (!firstLoad) bindSelect('CmbVariety',[],'id','name');
        if(field('CmbOrderType').options.length>1) field('CmbOrderType').selectedIndex=1;
        if(field('CmbStatus').options.length) field('CmbStatus').value='Open';
        if(field('cmbWeighBridge').options.length>1) field('cmbWeighBridge').selectedIndex=1;
        field('CmbPackingType').value='';
        field('btnsave').style.display='inline-flex';
        field('btnupdate').style.display='none';
        applyOrderType(true);               // the operator tabs through CmbOrderType -> CmbOrderType_Leave
        field('CmbOrderno').value='0';      // Reset: CmbOrderno.Text = "0" (after the Rows[1] activation)
        if ([105,106,98,52,241,204].includes(numeric('CmbOrderType'))) field('CmbOrderno').value=field('txtgpno').value;
        calculateWeight(); calculateNetWeights(); calculateFreight();
        return Promise.all([
            igpFetch('/api/inward-gate-pass/generate-no?gatepassType='+encodeURIComponent(caption('cmbgptype'))).then(res=>res.json()).then(data=>{
                if(generation!==formGeneration) return;
                field('txtgpno').value = data.gpSrNo || '';
                field('txtgptypeno').value = data.gpTypeSrNo || '';
                if ([105,106,98,52,241,204].includes(numeric('CmbOrderType'))) { field('CmbOrderno').value=field('txtgpno').value; poId=numeric('txtgpno'); }
                if ([175,700].includes(numeric('CmbOrderType'))) poId=numeric('txtgpno');
            })
        ]).then(()=>{ field('txtgpdate').focus(); });
    }

    /* WeightComparedPoWtCalculation :4106 */
    function calculateWeight() {
        const qty = parseFloat(field('txtqty').value) || 0;
        const packUnit = parseFloat(field('txtPackUnit').value) || 0;
        field('txtWeight').value = qty>0 && packUnit>0 ? String(qty * packUnit) : '0';
    }

    /* FreightCalculations :4210 */
    function calculateFreight() {
        const totalPayable = (parseFloat(field('txtfreight').value) || 0) - (parseFloat(field('txtAdvanceByParty').value) || 0) - (parseFloat(field('txtAdvanceByFactory').value) || 0);
        field('txtTotalPayablesFreight').value = String(totalPayable);
        if(!netPaidEdited) field('txtNetPaid').value = String(totalPayable);
    }

    /* CalculateSupplierNetWeight :5438 and CalculateDifferenceWeights :5503 */
    function calculateNetWeights() {
        const factLoad = parseFloat(field('txtFactoryWeight').value) || 0;
        const factTare = parseFloat(field('txtsecondWeight').value) || 0;
        const supLoad = parseFloat(field('txtSupplierFirstWeight').value) || 0;
        const supTare = parseFloat(field('txtSupplierSecondWeight').value) || 0;
        if(supLoad>0 || supTare>0) field('txtSupplierNetWeight').value=String(Math.round(Math.abs(supLoad-supTare)*1000)/1000);
        const supNet=numeric('txtSupplierNetWeight'), factNet=numeric('txtFactoryNetWeight');
        field('txtFirstWtDifference').value=String(Math.abs(factLoad-supLoad));
        field('txtSecondWtDifference').value=String(Math.abs(factTare-supTare));
        field('txtDifferenceWeight').value=String(Math.abs(factNet-supNet));
    }

    /* txtSupplierWeight_TextChanged :4071 - typing the supplier net weight zeroes the load/tare weights */
    function onSupplierNetInput() {
        field('txtSupplierFirstWeight').value=0; field('txtSupplierSecondWeight').value=0;
        calculateNetWeights();
    }

    /* txtvehicleno_TextChanged (upper case) and txtvehicleno_Leave (insert the dash after the leading letters) */
    function onVehicleNoInput(input) { const pos=input.selectionStart; input.value=input.value.toUpperCase(); try { input.setSelectionRange(pos,pos); } catch(_) {} }
    function onVehicleNoLeave() {
        let text=field('txtvehicleno').value.trim().toUpperCase(); if(!text) return;
        if(!text.includes('-')) { const letters=(text.match(/^[A-Z]*/)||[''])[0].length; if(letters>=1 && letters<=6) text=text.slice(0,letters)+'-'+text.slice(letters); }
        field('txtvehicleno').value=text;
    }

    const driverFields=['txtCNIC','txtDriverCellNo','txtWhatsAppNo','txtAlternateCellNo','txtDriverName','txtFatherName','txtFatherCNIC'];
    /* MaskedTextBox masks of the designer: txtCNIC / txtFatherCNIC "00000-0000000-0", the three cell numbers "000-000-0000000"
       (designer Text "0923" and ResetDriverFields "092-3-" both show as 092-3). Digits only; the dashes are the mask literals. */
    const DRIVER_MASKS={txtCNIC:'00000-0000000-0',txtFatherCNIC:'00000-0000000-0',txtDriverCellNo:'000-000-0000000',txtWhatsAppNo:'000-000-0000000',txtAlternateCellNo:'000-000-0000000'};
    function maskText(value,mask) {
        const digits=String(value??'').replace(/\D/g,''); let out='', i=0;
        for (const ch of mask) { if (i>=digits.length) break; if (ch==='0') out+=digits[i++]; else out+=ch; }
        return out;
    }
    function onMaskedInput(input) { const mask=DRIVER_MASKS[input.id]; if (mask) input.value=maskText(input.value,mask); }
    const CELL_DEFAULT='092-3';
    function setDriverLocked(locked) { driverFields.forEach(id=>field(id).disabled=locked); }
    /* txtCNIC_Leave :5671 / txtDriverCellNo_Leave :5698 */
    async function lookupDriver(kind,control) {
        const value=field(control).value.trim(), generation=++driverGeneration, form=formGeneration;
        if(!value) { setDriverLocked(false); return; }
        try {
            const bio=await (await igpFetch('/api/inward-gate-pass/driver-bio/'+kind+'?'+new URLSearchParams({[kind]:value}))).json();
            if(generation!==driverGeneration || form!==formGeneration || field(control).value.trim()!==value) return;
            if(!bio || !Number(bio.Id)) { setDriverLocked(false); return; }
            driverBioId=Number(bio.Id); setDriverLocked(true);
            const map={txtCNIC:'CnicNo',txtDriverCellNo:'DriverCellNo',txtWhatsAppNo:'WhatsappNo',txtAlternateCellNo:'AlternateCellNo',txtDriverName:'DriverName',txtFatherName:'FatherName',txtFatherCNIC:'FatherCnicNo'};
            for(const [id,key] of Object.entries(map)) field(id).value=DRIVER_MASKS[id]?maskText(bio[key]||'',DRIVER_MASKS[id]):(bio[key]||'');
        } catch(error) { showRequestError(error); }
    }
    function lookupDriverByCnic() { return lookupDriver('cnic','txtCNIC'); }
    function lookupDriverByCell() { return lookupDriver('cell','txtDriverCellNo'); }

    /* btnResetDriverInfo_Click :5725 -> ResetDriverFields + DriverFieldsDisableOrEnable(true) */
    function resetDriverInfoFields() {
        driverBioId=0; ++driverGeneration; setDriverLocked(false);
        driverFields.forEach(id=>field(id).value='');
        for (const id of ['txtDriverCellNo','txtWhatsAppNo','txtAlternateCellNo']) field(id).value=CELL_DEFAULT;   // "092-3-"
    }

    /* btnResetDriverInfo_Click: driverBiodata.ReadAll (the web looks drivers up on the server at each Leave) + reset */
    function refreshDriversAndReset() { resetDriverInfoFields(); field('txtCNIC').focus(); }

    /* ---------------- Purchase type / gate-pass type / order number ---------------- */

    /* cmbgptype_Leave :1647 - every item (ReadAllItemsIncludedPM) unless the type is 41 or 700 (then the order's items stay). */
    function bindAllItems() {
        if ([700,41].includes(numeric('CmbOrderType'))) return;
        const keep=field('CmbVariety').value;
        bindSelect('CmbVariety',lookupData.items,'id','name');
        if (keep && Array.from(field('CmbVariety').options).some(o=>o.value===keep)) field('CmbVariety').value=keep; else field('CmbVariety').value='';
    }

    /* CmbOrderType_Leave :1583 */
    function applyOrderType(fromReset) {
        const type=numeric('CmbOrderType'), keepSupplier=field('cmbsupp').value;
        field('txtSupplierNetWeight').disabled=false;
        field('cmbTransitVehicle').disabled=true;
        field('CmbOrderno').value=''; field('CmbOrderno').disabled=false;
        poId=0;
        bindSelect('cmbsupp',[],'id','name');
        const bindParties=list=>{ bindSelect('cmbsupp',list,'id','name'); if(keepSupplier && Array.from(field('cmbsupp').options).some(o=>o.value===keepSupplier)) field('cmbsupp').value=keepSupplier; };
        if ([105,106,98,52].includes(type)) {
            poId=numeric('txtgpno'); field('CmbOrderno').value=field('txtgpno').value.trim(); field('CmbOrderno').disabled=true;
            bindParties(type!==98?lookupData.suppliers:lookupData.saleInvoiceParties);   // BindLocalSupplierCustomer / BindCustomerAgainstSaleInvoice
            bindAllItems();
        }
        if ([241,204].includes(type)) {
            poId=numeric('txtgpno'); field('CmbOrderno').value=field('txtgpno').value.trim(); field('CmbOrderno').disabled=true;
            bindParties(lookupData.allSupplierCustomers);                                  // BindAllSupplierCustomer
            bindAllItems();
        }
        if ([175,700].includes(type)) { poId=numeric('txtgpno'); bindParties(lookupData.suppliers); }
        if (type!==105) { field('txtAdvanceByFactory').disabled=false; field('txtAdvanceByParty').disabled=false; }
        else { field('txtAdvanceByFactory').value='0'; field('txtAdvanceByParty').value='0'; field('txtAdvanceByFactory').disabled=true; field('txtAdvanceByParty').disabled=true; calculateFreight(); }
        if (type===41) field('cmbTransitVehicle').disabled=false;
        // CmbOrderType_Leave does not call PreBillNoFill; the transit list follows cmbsupp_Leave / CmbOrderno_Leave only.
    }
    function onOrderTypeChange() { ++orderGeneration; applyOrderType(false); }

    /* cmbgptype.ValueChanged -> cmbgptype_Leave_1 :809 (next number of the type, also while editing) and Leave -> cmbgptype_Leave */
    function onGpTypeChange() {
        const generation=formGeneration, type=caption('cmbgptype');
        bindAllItems();
        if(!type) return Promise.resolve();
        return igpFetch('/api/inward-gate-pass/generate-no?gatepassType='+encodeURIComponent(type)).then(r=>r.json()).then(data=>{
            if(generation!==formGeneration || type!==caption('cmbgptype')) return;
            if(Number(data.gpTypeSrNo)>0) field('txtgptypeno').value=data.gpTypeSrNo; else alert('Please GatePass Type Select');
        }).catch(showRequestError);
    }
    function onItemChange() { }

    /* CmbOrderno_Leave :1099 */
    function onOrderNumberLeave() {
        const generation=++orderGeneration, number=parseInt(field('CmbOrderno').value,10)||0, type=numeric('CmbOrderType'), gpType=caption('cmbgptype');
        if (gpType==='Export Return' || field('CmbOrderno').disabled) return Promise.resolve();
        if (![41,700,1500].includes(type)) {
            if ([175,104,232,236].includes(type)) alert('The '+caption('CmbOrderType')+' order lookup of the desktop form (stock receiving / supply / import contract) has not been ported to this page.');
            return Promise.resolve();
        }
        if (gpType==='Export' || gpType==='Import') return Promise.resolve();
        const clear=()=>{ poId=0; bindSelect('cmbsupp',[],'id','name'); field('CmbOrderno').value=''; bindSelect('CmbVariety',[],'id','name'); };
        return igpFetch('/api/inward-gate-pass/order-party-items?'+new URLSearchParams({documentTypeId:type,number,date:field('txtgpdate').value,gatePassId:numeric('txtId')})).then(r=>r.json()).then(async rows=>{
            if(generation!==orderGeneration) return;
            if(!rows.length) clear();
            else {
                poId=Number(rows[0].PurchaseOrderId)||0;
                if (type!==700 && rows[0].OrderCategoryName) { setCaption('cmbgptype',rows[0].OrderCategoryName); onGpTypeChange(); }
                bindSelect('cmbsupp',rows,'Id','CompanyName'); field('cmbsupp').selectedIndex=1;
                bindSelect('CmbVariety',rows,type===1500?'ItemId':'ItemId','ItemName'); field('CmbVariety').selectedIndex=1;
                if (type===1500) field('cmbcity').value=String(rows[0].CityId??''); else setCaption('cmbcity',rows[0].CityArea);
            }
            if (type===41) { field('cmbTransitVehicle').disabled=false; await loadTransitVehicles(); }   // PreBillNoFill(supplier, POId)
        }).catch(error=>{ clear(); showRequestError(error); });
    }

    /* cmbsupp_Leave :5407 */
    function onSupplierChange() {
        if (numeric('cmbsupp')>0 || poId>0) return loadTransitVehicles().catch(showRequestError);
        transitRows=[]; bindSelect('cmbTransitVehicle',[],'id','name'); return Promise.resolve();
    }

    /* PreBillNoFill :726 - SupplierDispatch_GetNoForGpandGrn(org, company, PartyId, 0, RecId, 0, OrderId) */
    async function loadTransitVehicles(selectedId=numeric('cmbTransitVehicle')) {
        const generation=++transitGeneration, form=formGeneration;
        const supplierId=numeric('cmbsupp'), orderId=poId;
        if(!supplierId && !orderId) { transitRows=[]; bindSelect('cmbTransitVehicle',[],'id','name'); return; }
        const rows=await (await igpFetch('/api/inward-gate-pass/transit-vehicles?'+new URLSearchParams({supplierId,orderId,gatePassId:numeric('txtId')}))).json();
        if(generation!==transitGeneration || form!==formGeneration) return;
        transitRows=rows; bindSelect('cmbTransitVehicle',rows,'id','name'); field('cmbTransitVehicle').value=selectedId||'';
        if (rows.length) onTransitVehicleChange();
    }
    /* CmbSupplierDispatchedPreBillNo_Leave :5366 */
    function onTransitVehicleChange() {
        const row=transitRows.find(r=>Number(r.id)===numeric('cmbTransitVehicle')); if(!row)return;
        if(String(row.VehicleNo||'').trim()) field('txtvehicleno').value=row.VehicleNo;
        if(String(row.BiltyNo||'').trim()) field('txtbiltyno').value=row.BiltyNo;
        if(Number(row.CityId)>0) field('cmbcity').value=row.CityId;
        if(Number(row.Freight)>0) { field('txtfreight').value=Math.round(Number(row.Freight)); if(Number(row.AdvanceFreight)>0) field('txtAdvanceByParty').value=Math.round(Number(row.AdvanceFreight)); }
        calculateFreight();
    }

    /* ---------------- linked screens (status strip) ---------------- */
    var IGP_LINKED_FORMS = {
        LabAnalysis:    { url: '/quality/purchase-analysis', label: 'Lab Purchase Analysis' },   /* InvLabPurchaseAnalysis :3816 */
        WeightBridge:   { url: '/weighbridge/weight-bridge', label: 'Weigh Bridge' },            /* frmWeightbridge :3826 */
        Grn:            { url: '/purchase/goods-receipt-notes', label: 'GRN' },                  /* InvFrmGRN :3843 */
        FreightVoucher: { url: '/accounts/vouchers/freight', label: 'Freight Voucher' }          /* FreightVoucher :3865 */
    };
    function igpOpenLinkedForm(key, gatePassId) {
        var target = IGP_LINKED_FORMS[key];
        if (!target) return false;
        if (key === 'Grn' && caption('CmbOrderType') === 'Purchase_Order_PM') {
            alert('Order Type is Purchase_Order_PM, which opens the Packing Material GRN on the desktop (GrnPackingMaterial). That screen has not been built yet, so the ordinary GRN was NOT opened in its place.');
            return false;
        }
        var url = target.url;
        if (gatePassId) url += (url.indexOf('?') >= 0 ? '&' : '?') + 'gatePassId=' + encodeURIComponent(gatePassId);
        window.open(url, '_blank', 'noopener');
        return false;
    }
    /* OpenLinkformOnInsert(GatePassId, Status) :4027 - the Grn branch also needs Status == "Accepted" */
    function igpOpenLinkedFormAfterSave(gatePassId, status) {
        var sel = document.querySelector('input[name="formMode"]:checked');
        var mode = sel ? sel.value : 'None';
        if (mode === 'None') return;
        if (mode === 'Grn' && String(status || '').trim() !== 'Accepted') return;
        igpOpenLinkedForm(mode, gatePassId);
    }

    /* ---------------- Save / Update ---------------- */
    async function onSaveRecord() {
        const updating=numeric('txtId')>0;
        if (field(updating?'btnupdate':'btnsave').disabled) return;
        if (updating && [204,241].includes(numeric('CmbOrderType')) && !numeric('CmbVariety')) { alert('Please select Variety First'); return; }
        // formvalidation(): the one MessageBox question in it (short vehicle number, :1725); the rest is checked by the server.
        const vehicle=field('txtvehicleno').value.trim().toUpperCase();
        const parts=/^[A-Z]{1,6}-\d{1,6}$/.test(vehicle)?vehicle.split('-'):null;
        if (parts && (parts[0].length<=2 || parts[1].length<=2) && !confirm('The vehicle number is unusually short (letters or digits ≤ 2).\nAre you sure you want to proceed?')) { field('txtvehicleno').focus(); return; }
        if (updating) {   // btnupdate_Click :3627 shows the remarks box whenever factory weight exceeds supplier weight
            if (numeric('txtFactoryNetWeight')>numeric('txtSupplierNetWeight')) field('rowWeightDiffRemarks').style.display='';
        }
        const payload = {
            id: numeric('txtId'),
            documentTypeId: context.documentTypeId,
            gpDate: field('txtgpdate').value,
            gpSrNo: parseInt(field('txtgpno').value,10) || 0,
            gpTypeSrNo: parseInt(field('txtgptypeno').value,10) || 0,
            gatepassType: caption('cmbgptype'),
            supplierCustomerId: numeric('cmbsupp'),
            cityId: numeric('cmbcity'),
            itemId: numeric('CmbVariety'),
            varietyName: caption('CmbVariety'),
            vehicleNo: field('txtvehicleno').value,
            vehicleType: caption('cmbvehicletype'),
            biltyNo: field('txtbiltyno').value,
            biltyDate: field('txtBiltyDate').value,
            freight: parseFloat(field('txtfreight').value) || 0,
            advanceByParty: parseFloat(field('txtAdvanceByParty').value) || 0,
            advanceByFactory: parseFloat(field('txtAdvanceByFactory').value) || 0,
            netPaid: parseFloat(field('txtNetPaid').value) || 0,
            supplierFirstWeight: parseFloat(field('txtSupplierFirstWeight').value) || 0,
            supplierSecondWeight: parseFloat(field('txtSupplierSecondWeight').value) || 0,
            supplierWeight: parseFloat(field('txtSupplierNetWeight').value) || 0,
            status: caption('CmbStatus'),
            packingTypeId: numeric('CmbPackingType'),
            packUnit: parseFloat(field('txtPackUnit').value) || 0,
            noOfPackages: parseInt(field('txtqty').value,10) || 0,
            weightComparedToPoWt: numeric('txtWeight'),
            docAttachment: caption('cmbWeighBridge'),
            supplierDispatchId: numeric('cmbTransitVehicle'),
            otherRemarks: field('txtremarks').value,
            otherSupCust: caption('CmbOrderType'),
            refDocumentTypeId: numeric('CmbOrderType'),
            supplierContractCode: field('CmbOrderno').value,
            purchaseOrderId: poId,
            weightDiffComments: field('txtWeightDiffRemarks').value,
            container: '', container1: '',
            driverBioDataId: driverBioId,
            driverName: field('txtDriverName').value,
            driverCNICNO: field('txtCNIC').value,
            driverMobileNo: field('txtDriverCellNo').value,
            whatsappNo: field('txtWhatsAppNo').value,
            alternateCellNo: field('txtAlternateCellNo').value,
            fatherName: field('txtFatherName').value,
            fatherCnicNo: field('txtFatherCNIC').value,
            isApprovedChecked: field('ChkIsApproved').checked,
            gatePassInwardPurchaseBreakUpList: loadedBreakups.map(row=>({qty:Number(row.qty)||0,uom:Number(row.uom)||0,ebWeight:Number(row.ebWeight)||0})),
            confirmed: []
        };
        if (!confirm(updating?'Are you sure to Update?':'Are you sure to Save?')) return;
        if (payload.status==='Rejected' && !confirm('Are you sure to Reject GatePass?')) return;
        let data;
        try {
            for (;;) {
                data = await (await igpFetch('/api/inward-gate-pass/save', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) })).json();
                if (data && !data.success && data.confirmKey) {
                    if (!confirm(data.message)) return;
                    payload.confirmed.push(data.confirmKey);
                    continue;
                }
                break;
            }
        } catch (error) {
            if (updating) { field('rowWeightDiffRemarks').style.display=''; field('txtWeightDiffRemarks').focus(); }   // btnupdate_Click catch
            if (payload.refDocumentTypeId===105 && /Market Purchase Advance/.test(error.message||'')) { field('txtAdvanceByParty').value='0'; field('txtAdvanceByFactory').value='0'; calculateFreight(); }
            throw error;
        }
        if (!data.success) { alert(data.message); return; }
        alert(data.message);
        const status=payload.status;
        await onNewRecord();                                    // Reset()
        await loadMainHistoryGrid();
        igpOpenLinkedFormAfterSave(data.igpId, updating?status:'');   // btnsave passes "" as status
        if (field('chkPreview').checked) printSlip251(data.igpId);    // ChkBox.Checked -> GatePassInwardSlipAndRegisterReport(success)
        // btnsave_Click :3585 (insert only): chkDriverInfoForm.Checked -> frmDriverBio { RefDocumentTypeId = 51, cmbGatePass.Value = success }.
        if (!updating && field('chkDriverInfoForm').checked) openDriverBio(Number(data.igpId)||0, "You Don't Have View-right Of This Driver-Bio For Inward..");
        if (field('RadGrnFormToOpenOnInsert').checked) document.querySelector('input[name="formMode"][value="None"]').checked=true;   // both buttons
    }

    /* ---------------- grids ---------------- */
    const pad = n => String(n).padStart(2, '0');
    function fmtDateTime(v) { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return escapeHtml(v);
        let h = d.getHours(); const ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return pad(d.getDate()) + '-' + pad(d.getMonth() + 1) + '-' + d.getFullYear() + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + ap; }
    function fmtTime(v) { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return escapeHtml(v);
        let h = d.getHours(); const ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12; return pad(h) + ':' + pad(d.getMinutes()) + ' ' + ap; }
    function fmtDay(v) { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); return isNaN(d) ? escapeHtml(v) : pad(d.getDate()) + '-' + pad(d.getMonth() + 1) + '-' + d.getFullYear(); }
    const accessColor = v => Number(v) === 0 ? 'green' : (Number(v) > 0 ? 'red' : '');                       // grd_FormattingRow
    const approvalColor = v => v === 'Not Approved' ? 'red' : (v === 'Approved' ? 'green' : '');

    /* grd / grdhistory rows: click selects (CurrentRow), double-click = grd_DoubleClick / grdhistory_DoubleClick (ReadById),
       Ctrl+Space / Ctrl+Enter on a focused row = the Edit button (grd_KeyDown :5175, grdhistory_KeyDown :5202,
       InwardGatePass_KeyDown Ctrl+Enter). The Gp No is a link to the same record. */
    function gridRow(id, history) {
        const tr=document.createElement('tr');
        tr.tabIndex=-1; tr.dataset.id=id;
        tr.onclick=()=>selectGridRow(tr);
        tr.ondblclick=()=>withButtonLoading(null,()=>loadRecordAndEdit(id));          // grdhistory_DoubleClick has no Reset()
        tr.onkeydown=event=>{
            if(event.ctrlKey && event.key===' ') { event.preventDefault(); event.stopPropagation(); withButtonLoading(null,()=>history?editFromHistory(id):loadRecordAndEdit(id)); }   // Ctrl+Space = Edit column
            else if(event.ctrlKey && event.key==='Enter') { event.preventDefault(); event.stopPropagation(); withButtonLoading(null,()=>loadRecordAndEdit(id)); if(!history) field('txtgpdate').focus(); }
            else if(event.key==='ArrowDown'||event.key==='ArrowUp') { event.preventDefault(); const next=event.key==='ArrowDown'?tr.nextElementSibling:tr.previousElementSibling; if(next){selectGridRow(next);next.focus();} }
        };
        return tr;
    }
    function selectGridRow(tr) { tr.parentElement.querySelectorAll('tr.selected').forEach(r=>r.classList.remove('selected')); tr.classList.add('selected'); }
    function focusGrid(bodyId) { const body=field(bodyId); const tr=body.querySelector('tr.selected')||body.querySelector('tr'); if(tr){selectGridRow(tr);tr.focus();} }
    function loadSelectedRow(bodyId) { const tr=field(bodyId).querySelector('tr.selected'); if(!tr) return; const id=Number(tr.dataset.id); withButtonLoading(null,()=>loadRecordAndEdit(id)); }
    function codeLink(id, text, history) {
        return `<a href="#" class="code-link" title="Open this gate pass" onclick="event.stopPropagation();withButtonLoading(null,()=>${history?'editFromHistory':'loadRecordAndEdit'}(${Number(id)}));return false;">${escapeHtml(text ?? '')}</a>`;
    }
    /* grdhistory Edit (:4656) calls Reset() before ReadById; grd Edit does not. */
    async function editFromHistory(id) { await onNewRecord(); return loadRecordAndEdit(id); }

    /* NoOfAttachments (ColumnType Link): grd_LinkClicked -> GetNoofAttachmentsByRefDocumentTypeID(Id, 51);
       grdhistory_LinkClicked -> DMSAttachments.GetByID(Id, "InwardGatePass") -> AttachmentView. Read only here. */
    function attachmentLink(id, count, history) {
        const n=Number(count)||0;
        return `<a href="#" class="code-link" title="View attachments" onclick="event.stopPropagation();withButtonLoading(null,()=>showAttachments(${Number(id)},${history}));return false;">${escapeHtml(count ?? 0)}</a>`;
    }
    async function showAttachments(id, history) {
        const rows=await (await igpFetch('/api/inward-gate-pass/'+Number(id)+'/attachments?history='+(history?'true':'false'))).json();
        let box=field('igpAttachmentView');
        if(!box) { box=document.createElement('div'); box.id='igpAttachmentView'; box.className='igp-modal'; document.body.appendChild(box); }
        box.innerHTML='<div class="igp-modal-box" role="dialog" aria-label="Attachments"><div class="igp-modal-title">Attachments<button type="button" class="tool-btn" onclick="this.closest(\'.igp-modal\').style.display=\'none\'">X</button></div>'
            +(rows.length?'<table class="data-grid"><thead><tr><th>#</th><th>Attachment</th></tr></thead><tbody>'
                +rows.map((r,i)=>`<tr><td>${i+1}</td><td><a class="code-link" href="/api/inward-gate-pass/${Number(id)}/attachments/${Number(r.Id)}?history=${history?'true':'false'}">${escapeHtml(r.Attachment)}</a></td></tr>`).join('')+'</tbody></table>'
                :'<div style="padding:8px;">No attachments.</div>')+'</div>';
        box.style.display='flex';
    }

    /* grdfrmfill :2053 - ReadByGPDate, "Vehicles Present In The Factory" */
    function loadMainHistoryGrid() {
        return igpFetch('/api/inward-gate-pass/open-records')
            .then(res => res.json())
            .then(data => {
                const tbody = field('grdMainHistoryBody');
                tbody.innerHTML = '';
                let totalQty = 0, totalSupWt = 0, totalDiff = 0;
                const num = v => '<td style="text-align:right;">' + escapeHtml(v ?? '') + '</td>';
                const txt = v => '<td>' + escapeHtml(v ?? '') + '</td>';
                data.forEach(row => {
                    totalQty += parseFloat(row.ItemQty) || 0;
                    totalSupWt += parseFloat(row.SupplierWeight) || 0;
                    totalDiff += parseFloat(row.DifferenceWeight) || 0;
                    const id = Number(row.Id);
                    const tr = gridRow(id, false);
                    tr.innerHTML =
                        `<td><button class="tool-btn" style="padding:1px 4px;" onclick="event.stopPropagation();withButtonLoading(this,()=>onPrintReport(${id}))">Print</button></td>`
                      + `<td><button class="tool-btn" style="padding:1px 4px;" onclick="event.stopPropagation();withButtonLoading(this,()=>loadRecordAndEdit(${id}))">Edit</button></td>`
                      + `<td style="text-align:right;">${codeLink(id, row.GpSrNo, false)}</td>` + `<td>${fmtDay(row.GpDate)}</td>` + txt(row.GatepassType) + txt(row.OrderType) + txt(row.SupplierName)
                      + txt(row.OrderNo) + txt(row.VehicleType) + txt(row.VehicleNo) + txt(row.BiltyNo)
                      + num(row.ItemQty) + num(row.PackUnit) + num(row.WeightComparedToPoWt) + num(row.SupplierFirstWeight) + num(row.SupplierSecondWeight)
                      + num(row.SupplierWeight) + num(row.FactoryWeight) + num(row.DifferenceWeight)
                      + `<td style="text-align:right; color:${accessColor(row.AccessWeight)};">${escapeHtml(row.AccessWeight ?? '')}</td>`
                      + num(row.NetPaid) + txt(row.VarietyName) + txt(row.CityName) + `<td>${fmtTime(row.InTime)}</td>`
                      + `<td style="text-align:right;">${attachmentLink(id, row.NoOfAttachments, false)}</td>` + txt(row.OtherRemarks)
                      + `<td style="color:${approvalColor(row.ApprovalStatus)};">${escapeHtml(row.ApprovalStatus ?? '')}</td>` + txt(row.Status);
                    tbody.appendChild(tr);
                });
                field('lblRecordCount').textContent = data.length;
                field('lblSumQty').textContent = totalQty.toFixed(2);
                field('lblSumSupWt').textContent = totalSupWt.toFixed(2);
                field('lblSumDiff').textContent = totalDiff.toFixed(2);
            });
    }

    /* gridhistory :4402 - GatepassHistory */
    function executeFullHistorySearch() {
        const payload = {
            documentTypeId: 51,
            dateField: document.querySelector('input[name="histDateFilter"]:checked')?.value||'docDate',
            fromDate: field('txtHistFromDate').value,
            toDate: field('txtHistToDate').value,
            fromDocNo: field('txtHistFromDoc').value,
            toDocNo: field('txtHistToDoc').value,
            supplierId: field('cmbHistSupplier').value
        };
        return igpFetch('/api/inward-gate-pass/history', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) })
            .then(res => res.json())
            .then(data => {
                const tbody = field('grdFullHistoryBody');
                tbody.innerHTML = '';
                const num = v => '<td style="text-align:right;">' + (v === null || v === undefined ? '' : escapeHtml(v)) + '</td>';
                const txt = v => '<td>' + escapeHtml(v ?? '') + '</td>';
                data.forEach(row => {
                    const id = Number(row.Id);
                    const tr = gridRow(id, true);
                    tr.innerHTML =
                        `<td><button class="tool-btn" style="padding:1px 4px;" onclick="event.stopPropagation();withButtonLoading(this,()=>onPrintReport(${id}))">Print</button></td>`
                      + `<td><button class="tool-btn" style="padding:1px 4px;" onclick="event.stopPropagation();withButtonLoading(this,()=>editFromHistory(${id}))">Edit</button></td>`
                      + `<td>${codeLink(id, row.GpSrNo, true)}</td>` + `<td>${fmtDay(row.GpDate)}</td>`
                      + txt(row.GatepassType) + txt(row.OrderType) + num(row.OrderNo) + txt(row.CompanyName) + txt(row.Description)
                      + txt(row.VehicleType) + txt(row.VehicleNo) + `<td>${fmtDay(row.BiltyDate)}</td>` + txt(row.BiltyNo) + txt(row.VarietyName)
                      + num(row.ItemQty) + num(row.PackUnit) + num(row.WeightComparedToPoWt) + num(row.SupplierFirstWeight) + num(row.SupplierSecondWeight)
                      + num(row.SupplierWeight) + num(row.FactoryWeight) + num(row.DifferenceWeight)
                      + num(row.Freight) + num(row.AdvanceByParty) + num(row.AdvanceByFactory) + num(row.TotalPayableFreight) + num(row.NetPaid)
                      + `<td>${fmtDateTime(row.InTime)}</td><td>${fmtDateTime(row.OutTime)}</td>` + txt(row.Status) + txt(row.UserName) + `<td>${fmtDateTime(row.EntryDate)}</td>`
                      + txt(row.ModifyUserName) + `<td>${fmtDateTime(row.ModifyDate)}</td>`
                      + `<td style="text-align:right;">${attachmentLink(id, row.NoOfAttachments, true)}</td>`
                      + txt(row.OtherRemarks) + `<td style="color:${approvalColor(row.ApprovalStatus)};">${escapeHtml(row.ApprovalStatus ?? '')}</td>`
                      + `<td style="text-align:right; color:${accessColor(row.AccessWeight)};">${escapeHtml(row.AccessWeight ?? '')}</td>`
                      + txt(row.PackingType);
                    tbody.appendChild(tr);
                });
            });
    }
    /* btnNewHistory_Click :4679 */
    function onHistoryNew() {
        const threeDaysAgo = new Date(); threeDaysAgo.setDate(threeDaysAgo.getDate() - 3);
        field('txtHistFromDate').value = localStamp(threeDaysAgo).split('T')[0];
        field('txtHistToDate').value = localStamp().split('T')[0];
        field('txtHistFromDoc').value = ''; field('txtHistToDoc').value = ''; field('cmbHistSupplier').value = '';
        field('grdFullHistoryBody').innerHTML = '';
        field('txtHistFromDate').focus();
    }
    /* btnRefreshHistory_Click :4712 -> HistoryComboFill */
    function onHistoryRefresh() {
        const keep = field('cmbHistSupplier').value;
        return igpFetch('/api/inward-gate-pass/history-suppliers').then(r => r.json()).then(rows => {
            bindSelect('cmbHistSupplier', rows, 'id', 'name');
            if (keep && Array.from(field('cmbHistSupplier').options).some(o => o.value === keep)) field('cmbHistSupplier').value = keep;
        });
    }

    /* BtnResetOrderInfo_Click :2881 - dates to today, doc numbers and supplier cleared, grid cleared, combos refilled.
       The desktop sets drdocdate (the History tab's radio) here, not the PO tab's; the PO radio is left as it was. */
    function onResetPoInfo(btn) { return withButtonLoading(btn, resetPoInfoFilters); }
    function resetPoInfoFilters() {
        const today = localStamp().split('T')[0];
        field('txtFromPoDate').value = today;
        field('txtToPoDate').value = today;
        field('txtFromDocNoPoInfo').value = '';
        field('txtToDocNoPoInfo').value = '';
        field('cmbSupplierNamePoInfo').value = '';
        field('grdPoInfoBody').innerHTML = '';
        const hist = document.querySelector('input[name="histDateFilter"][value="docDate"]'); if (hist) hist.checked = true;
        const type = Number(lookupData.poInfoDocumentTypeId) || 0;
        if (!type) return Promise.resolve();
        const keepType = field('CmbDocumentTypePoInfo').value;
        return igpFetch('/api/inward-gate-pass/po-info-combos?documentTypeId=' + type).then(r => r.json()).then(d => {
            bindSelect('cmbSupplierNamePoInfo', d.suppliers, 'id', 'name');
            bindSelect('CmbDocumentTypePoInfo', d.documentTypes, 'id', 'name');
            if (keepType && Array.from(field('CmbDocumentTypePoInfo').options).some(o => o.value === keepType)) field('CmbDocumentTypePoInfo').value = keepType;
        }).catch(showRequestError);
    }

    /* btnshow_Click :2786 -> PurchaseOrderInformationForGatePassInward (41). The Steel variant (1500) is not ported. */
    function loadPoInfoGrid() {
        if (Number(lookupData.poInfoDocumentTypeId) === 1500) { alert('Purchase Order information for Steel orders (1500) has not been ported to this page.'); return Promise.resolve(); }
        if (Number(lookupData.poInfoDocumentTypeId) !== 41) return Promise.resolve();
        const payload = {
            fromDate: field('chkFromPoDate')?.checked ? field('txtFromPoDate').value : '',
            toDate: field('chkToPoDate')?.checked ? field('txtToPoDate').value : '',
            fromDocNo: field('txtFromDocNoPoInfo').value || 0,
            toDocNo: field('txtToDocNoPoInfo').value || 0,
            supplierId: field('cmbSupplierNamePoInfo').value || 0,
            documentTypeId: field('CmbDocumentTypePoInfo').value || 0,
            expiryDays: parseInt(field('txtExpiryDaysPoInfo').value, 10) || 0,
            dateField: document.querySelector('input[name="poDateFilter"]:checked')?.value || 'docDate'
        };
        return igpFetch('/api/inward-gate-pass/po-info', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) })
            .then(res => res.json())
            .then(data => {
                const tbody = field('grdPoInfoBody');
                tbody.innerHTML = '';
                const numCols = new Set(['OrderQty','OrderWeight','ReceivedQty','ReceivedWeight','BalQty','BalWeight','GrnReceivedQty','GrnReceivedWeight','BalQtyByGrn','BalWeightByGrn']);
                data.forEach(row => {
                    const tr = document.createElement('tr');
                    tr.innerHTML = ['BranchName','DocumentTypeDescription','OrderNo','SupplierName','ItemName','PackUom','OrderQty','OrderWeight','ReceivedQty','ReceivedWeight','BalQty','BalWeight','GrnReceivedQty','GrnReceivedWeight','BalQtyByGrn','BalWeightByGrn','RemarksHeader','ApprovedStatus','OrderStatus','OrderExpiryDate']
                        .map(key => {
                            if (key === 'OrderExpiryDate') return '<td>' + fmtDay(row[key]) + '</td>';
                            if (numCols.has(key)) { const v = Number(row[key]); return '<td style="text-align:right;">' + (row[key] == null ? '' : escapeHtml(Math.round(v).toLocaleString('en-US'))) + '</td>'; }   // "#,##0"
                            return '<td>' + escapeHtml(row[key] ?? '') + '</td>';
                        }).join('');
                    tbody.appendChild(tr);
                });
            });
    }

    /* grd_ColumnButtonClick "Edit" / grd_DoubleClick / grdhistory Edit -> LabDataGetByGpId(RecId) + ReadById(RecId) :3047 */
    function loadRecordAndEdit(id) {
        const generation=++formGeneration; ++orderGeneration;
        switchViewMode('Form');
        return igpFetch('/api/inward-gate-pass/'+Number(id)).then(res=>res.json()).then(async res=>{
            if(generation!==formGeneration) return;
            if(!res.success||!res.header) throw Error(res.message||'Record not found');
            const h=res.header; loadedHeader=lowerKeys(h);
            field('btnsave').style.display='none'; field('btnupdate').style.display='inline-flex';
            field('RadGrnFormToOpenOnInsert').style.display='';
            field('txtId').value=h.Id;
            // Lab info (LabDataGetByGpId)
            const lab=res.lab||{};
            field('txtAnaylstName').value=lab.AnalystName??''; field('txtLabReportNo').value=lab.DocNo??'';
            field('txtReportStatus').value=lab.LabStatus??''; field('txtLabRemarks').value=lab.RemarksHeader??'';
            setCaption('cmbgptype',h.GatepassType); bindAllItems();
            setCaption('cmbWeighBridge',h.DocAttachment);
            field('txtgpdate').value=String(h.GpDate||'').slice(0,10);
            field('txtBiltyDate').value=String(h.BiltyDate||'').slice(0,10);
            field('txtgpno').value=h.GpSrNo??''; field('txtgptypeno').value=h.GpTypeSrNo??'';
            field('cmbcity').value=h.CityId??'';
            field('txtbiltyno').value=h.BiltyNo??''; field('txtfreight').value=h.Freight??'';
            field('CmbPackingType').value=h.PackingTypeId??'';
            field('txtqty').value=h.NoOfPackages??'';
            field('txtPackUnit').value=h.PackUnit==null?'':String(Math.round(Number(h.PackUnit)*100)/100); field('txtPackUnit').disabled=true;
            field('txtWeight').value=h.WeightComparedToPoWt??'0';
            field('txtAccessWeight').value=h.AccessWeight??'';
            field('ChkIsApproved').checked=h.IsApproved===true||h.IsApproved===1;
            field('ChkIsApproved').disabled=Number(h.AccessWeight)>0 && !!lookupData.holdAccessWeightForApproval;   // txtAccessWeight_TextChanged
            actionIdForSpecialApproval=Number(h.ActionIdForSpecialApproval)||0;
            field('txtremarks').value=h.OtherRemarks??'';
            for(const [control,key] of [['txtintime','InDateTimeStamp'],['txtouttime','OutDateTimeStamp']]) if(h[key]) field(control).value=localStamp(new Date(h[key])).slice(0,16);
            field('txtSupplierNetWeight').value=h.SupplierWeight??'';
            field('txtSupplierFirstWeight').value=h.SupplierFirstWeight??''; field('txtSupplierSecondWeight').value=h.SupplierSecondWeight??'';
            field('CmbOrderno').value=h.SupplierContractCode??'';
            field('txtvehicleno').value=h.VehicleNo??''; onVehicleNoLeave();
            setCaption('cmbvehicletype',h.VehicleType);
            // GetWeighBridgeWeightAndTicketNos(RecId)
            const wb=res.weighBridgeWeights||[], sum=key=>wb.reduce((total,row)=>total+(Number(row[key])||0),0);
            if (wb.length) {
                field('txtWeighBridgeSlipNo').value=wb.map(row=>','+row.TicketNo).join('');
                field('txtFactoryWeight').value=sum('FirstWeight'); field('txtsecondWeight').value=sum('SecondWeight'); field('txtFactoryNetWeight').value=sum('NetWbWeight');
            } else { field('txtWeighBridgeSlipNo').value='0'; field('txtFactoryWeight').value='0'; field('txtsecondWeight').value='0'; field('txtFactoryNetWeight').value='0'; }
            // order type specific rebinding
            const ref=Number(h.RefDocumentTypeId)||0;
            field('CmbOrderType').value=String(ref); if(!field('CmbOrderType').value) setCaption('CmbOrderType',h.OtherSupCust);
            field('CmbOrderno').disabled=false; field('cmbTransitVehicle').disabled=true;
            poId=0;
            const suppliers=list=>bindSelect('cmbsupp',list,'id','name');
            if ((ref===41||ref===700||ref===1500) && h.GatepassType!=='Import') {
                const rows=await (await igpFetch('/api/inward-gate-pass/order-party-items?'+new URLSearchParams({documentTypeId:ref,number:parseInt(h.SupplierContractCode,10)||0,gatePassId:ref===1500?0:h.Id}))).json();
                if(generation!==formGeneration) return;
                if (rows.length) { poId=Number(rows[0].PurchaseOrderId)||0; bindSelect('cmbsupp',rows,'Id','CompanyName'); bindSelect('CmbVariety',rows,ref===1500?'Id':'ItemId','ItemName'); }
                if (ref===41) field('cmbTransitVehicle').disabled=false;
            } else if ([105,106,98,52].includes(ref)) { suppliers(lookupData.suppliers); field('CmbOrderno').disabled=true; }   // BindLocalSupplierCustomer (98 too, :3180)
            else if ([241,204].includes(ref)) { suppliers(lookupData.allSupplierCustomers); field('CmbOrderno').disabled=true; }
            else if (ref===175) { poId=Number(h.PurchaseOrderId)||0; suppliers(lookupData.suppliers); }
            else suppliers(lookupData.suppliers);
            field('cmbsupp').value=h.SupplierCustomerId??'';
            // Status: saved text; Rejected stays Rejected; an Open pass with factory weight shows Accepted
            field('CmbStatus').value=h.Status??'';
            if (h.Status!=='Rejected' && numeric('txtFactoryNetWeight')>0 && h.Status==='Open') field('CmbStatus').value='Accepted';
            field('txtAdvanceByParty').value=Number(h.AdvanceByParty)>0?h.AdvanceByParty:'0';
            field('txtAdvanceByFactory').value=Number(h.AdvanceByFactory)>0?h.AdvanceByFactory:'0';
            field('txtNetPaid').value=h.NetPaid??''; netPaidEdited=true;
            setCaption('CmbVariety',h.VarietyName); if(!field('CmbVariety').value && h.ItemId) field('CmbVariety').value=h.ItemId;
            field('cmbWeighBridge').disabled=!!(numeric('txtFactoryNetWeight'));
            calculateNetWeights();
            field('txtTotalPayablesFreight').value=String((Number(h.Freight)||0)-(Number(h.AdvanceByParty)||0)-(Number(h.AdvanceByFactory)||0));
            const freightLocked=Number(h.freightVoucherId)>0;
            if (freightLocked && !Number(h.SupplierWeight)) field('txtSupplierNetWeight').value=field('txtFactoryNetWeight').value;
            for (const id of ['txtSupplierFirstWeight','txtSupplierSecondWeight','txtfreight','txtNetPaid']) field(id).disabled=freightLocked;
            if (!freightLocked) field('txtSupplierNetWeight').disabled=false;
            // driver
            driverBioId=Number(h.driverBioDataId)||0;
            resetDriverInfoFields(); driverBioId=Number(h.driverBioDataId)||0;
            if (driverBioId>0) {
                const map={txtCNIC:'DriverCNICNO',txtDriverCellNo:'DriverMobileNo',txtWhatsAppNo:'whatsappNo',txtAlternateCellNo:'AlternateCellNo',txtDriverName:'DriverName',txtFatherName:'FatherName',txtFatherCNIC:'fatherCnicNo'};
                for(const [control,key] of Object.entries(map)) field(control).value=h[key]??'';   // saved text as stored
                setDriverLocked(true);
            }
            // purchase breakup: a saved list is read-only
            loadedBreakups=(res.purchaseBreakUps||[]).map(lowerKeys);
            breakupLocked=loadedBreakups.length>0; if(!loadedBreakups.length) loadedBreakups=[emptyBreakup()]; renderBreakups();
            if (ref!==105) { field('txtAdvanceByFactory').disabled=false; field('txtAdvanceByParty').disabled=false; }
            else { field('txtAdvanceByFactory').value='0'; field('txtAdvanceByParty').value='0'; field('txtAdvanceByFactory').disabled=true; field('txtAdvanceByParty').disabled=true; }
            if (h.RefferedInWbOrLab===true||h.RefferedInWbOrLab===1) { field('CmbOrderType').disabled=true; field('CmbOrderno').disabled=true; }
            else field('CmbOrderType').disabled=false;
            await loadTransitVehicles(Number(h.SupplierDispatchId)||0);
            field('rowWeightDiffRemarks').style.display='none'; field('txtWeightDiffRemarks').value='';
            field('cmbsupp').focus();
        }).catch(error=>{showRequestError(error);throw error;});
    }

    function switchMainSubTab(tabId) {
        document.querySelectorAll('.tab-container .tab-btn').forEach(btn => btn.classList.remove('active'));
        document.querySelectorAll('.tab-container .tab-content').forEach(c => c.classList.remove('active'));
        if (tabId === 'tabGridHistory') {
            document.querySelectorAll('.tab-container .tab-btn')[0].classList.add('active');
            field('tabGridHistory').classList.add('active');
        } else {
            field('tabBtnPoInfo').classList.add('active');
            field('tabPoInfo').classList.add('active');
            field('txtFromPoDate').focus();   // tabControl2_SelectedIndexChanged: focus only
        }
    }

    /* btnRefresh_Click :3017 - reload the global lists and rebind items, packing types, gate-pass types, vehicle types, cities */
    function onRefreshForm() { return loadDropdowns().then(()=>{ bindAllItems(); applySupplierListForType(); }); }
    function applySupplierListForType() {
        const type=numeric('CmbOrderType'), keep=field('cmbsupp').value;
        if ([41,700,1500].includes(type)) return;
        const list=[241,204].includes(type)?lookupData.allSupplierCustomers:(type===98?lookupData.saleInvoiceParties:lookupData.suppliers);
        bindSelect('cmbsupp',list,'id','name'); field('cmbsupp').value=keep;
    }
    /* toolStripButton3_Click :4947 - new DefineCity(UserAccount).Show(): the web DefineCity page (screen 750) in a new tab;
       Refresh (CityFill) then brings the new city into cmbcity, as on the desktop. */
    function onDefineCity() { if(!window.open('/master-data/city','_blank')) alert('The browser blocked the Define City window. Allow pop-ups for this site and retry.'); }
    function onDefineVehicle() { alert('The Vehicle Type definition form (VehicleType) has not been ported to the web yet.'); }
    /* BtnDriverForm_Click :5321 - View right on ScreenName "frmDriverBioForInWard", then frmDriverBio { RefDocumentTypeId = 51 }.Show()
       (non-modal, so a new tab here). The server checks the same right again when the page and its data are requested. */
    function openDriverBio(gatePassId, message) {
        if (field('BtnDriverForm').dataset.allowed!=='1') { alert(message); return false; }
        const url='/purchase/driver-bio'+(gatePassId>0?'?gatePassId='+encodeURIComponent(gatePassId):'');
        const opened=window.open(url,'_blank');
        if (!opened) alert('The browser blocked the Driver Bio window. Allow pop-ups for this site and retry.');
        return false;
    }
    function onOpenDriverForm() { return openDriverBio(0, "You Don't Have View-right Of Of This Driver-Bio For Inward.."); }
    function onOpenAttachments() { alert('Gate pass attachments (DMS attachments of InwardGatePass) have not been ported to this page yet.'); }

    /* ---------------- printing: the same .rpt through the shared print runtime ---------------- */
    function igpPrint(rpt, args) {
        if (window.printRpt) return window.printRpt(rpt, args);
        alert('The print runtime is not loaded on this page.');
    }
    /* grd / grdhistory "Print" column and ChkBox preview: CommonServices.GatePassInwardSlipAndRegisterReport(Id) */
    function printSlip251(id) { return igpPrint('251-InvRptInwardGatePassSlip.rpt', { id: Number(id) }); }
    /* btnPrint_Click -> GenerateReport() with RecId (0 on a new form, as the desktop) */
    function onPrintReport(id) { if (id) return printSlip251(id); if (field('btnPrint').disabled) return; return printSlip251(numeric('txtId')); }
    function onPrintSlip() { return igpPrint('257-InwardGatePassWithWbAndLabSlip.rpt', { id: numeric('txtId') }); }
    function onLabReport() { return igpPrint('653-RptInvLabPurchaseAnalysisSlip.rpt', { history: numeric('txtId') }); }

    /* MakeShortCutKeys :5273 */
    function onShowShortcuts() {
        alert(['Ctrl+E  For Close','Ctrl+N  For New','Ctrl+R  For Refresh','Ctrl+S  For Save','Ctrl+U  For Update','Alt+P  For Print',
               'Ctrl+F5  For Focus on gp Date','Ctrl+F10  For Open Attachments','Ctrl+T  For Tab Transfer','Ctrl+alt  To Show ShortCut Keys Form',
               'Ctrl+ArrowDown  For Focus On Detail Grid','Ctrl+ArrowRight  For Change Focus from one Grid To another Grid',
               'Ctrl+ArrowLeft  For Change Focus from one Grid To another Grid','Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }

    /* InwardGatePass_KeyDown :4971 */
    document.addEventListener('keydown',event=>{
        const key=event.key.toLowerCase(), history=field('viewHistory').style.display!=='none';
        // e.KeyData == Keys.Return -> SendKeys("{TAB}"): Enter moves to the next field (not on buttons, links, grid rows or open combos).
        if(event.key==='Enter' && !event.ctrlKey && !event.altKey && !event.shiftKey) {
            const t=event.target;
            if(t && t.tagName==='INPUT' && !['button','submit','checkbox','radio'].includes(t.type) && !t.closest('.dtcombo-wrap.dtcombo-open') && t.id!=='CmbOrderno') {
                const list=Array.from(document.querySelectorAll(history?'#viewHistory input, #viewHistory select':'#viewForm input, #viewForm select'))
                    .filter(el=>!el.disabled && !el.readOnly && el.offsetParent!==null && el.type!=='hidden' && el.tabIndex>=0 && !el.classList.contains('dtcombo-native'));
                const i=list.indexOf(t); if(i>=0 && list[i+1]) { event.preventDefault(); list[i+1].focus(); }
            }
        }
        if(event.ctrlKey && event.altKey && (key==='control'||key==='alt')) { onShowShortcuts(); return; }
        if(event.ctrlKey && key==='t'){ event.preventDefault(); if(history){switchViewMode('Form');field('txtgpdate').focus();} else switchViewMode('History'); return; }
        if(!history) {
            if(event.ctrlKey && key==='s'){ event.preventDefault(); if(field('btnsave').style.display!=='none' && !field('btnsave').disabled) withButtonLoading(field('btnsave'),onSaveRecord); }
            if(event.ctrlKey && key==='u'){ event.preventDefault(); if(field('btnupdate').style.display!=='none' && !field('btnupdate').disabled) withButtonLoading(field('btnupdate'),onSaveRecord); }
            if(event.ctrlKey && key==='p'){ event.preventDefault(); if(!field('btnPrint').disabled) onPrintReport(); }
            if(event.ctrlKey && key==='n'){ event.preventDefault(); withButtonLoading(field('btnnew'),onNewRecord); }
            if(event.ctrlKey && event.key==='F5'){ event.preventDefault(); field('txtgpdate').focus(); }
            if(event.altKey && key==='r'){ event.preventDefault(); withButtonLoading(field('btnRefresh'),onRefreshForm); }
            if(event.ctrlKey && event.key==='ArrowDown' && !event.target.closest?.('tbody')){ event.preventDefault(); focusGrid('grdMainHistoryBody'); }                  // grd.Focus()
            if(event.ctrlKey && event.key==='Enter' && !event.target.closest?.('tbody')){ event.preventDefault(); loadSelectedRow('grdMainHistoryBody'); field('txtgpdate').focus(); }   // grd_DoubleClick
            if(event.ctrlKey && (event.key==='ArrowLeft'||event.key==='ArrowRight') && field('tabBtnPoInfo').style.display!=='none'){   // tabControl2 pages
                event.preventDefault(); switchMainSubTab(field('tabPoInfo').classList.contains('active')?'tabGridHistory':'tabPoInfo');
            }
        } else {
            if(event.ctrlKey && key==='s'){ event.preventDefault(); withButtonLoading(field('btnHistShow'),executeFullHistorySearch); }
            if(event.ctrlKey && key==='n'){ event.preventDefault(); onHistoryNew(); }
            if(event.ctrlKey && event.key==='F5'){ event.preventDefault(); field('txtHistFromDate').focus(); }
            if(event.altKey && key==='r'){ event.preventDefault(); withButtonLoading(field('btnHistRefresh'),onHistoryRefresh); }
            if(event.ctrlKey && event.key==='ArrowDown' && !event.target.closest?.('tbody')){ event.preventDefault(); focusGrid('grdFullHistoryBody'); }
            if(event.ctrlKey && event.key==='Enter' && !event.target.closest?.('tbody')){ event.preventDefault(); loadSelectedRow('grdFullHistoryBody'); }   // grdhistory_DoubleClick
        }
    });

    /* ---------------- Purchase BreakUp (grdPurchaseBrakup) ---------------- */
    function emptyBreakup() { return {id:0,qty:0,uom:0,grossWeight:0,ebWeight:0,ebTotal:0,netWeight:0}; }
    function breakupTotals() { return loadedBreakups.reduce((sum,row)=>{for(const key of ['qty','grossWeight','ebWeight','ebTotal','netWeight'])sum[key]=(sum[key]||0)+(Number(row[key])||0);return sum;},{}); }
    function renderBreakups() {
        const body=field('grdBreakupBody'); if(!body)return;
        body.innerHTML=loadedBreakups.map((row,index)=>'<tr>'+['add','delete','qty','uom','grossWeight','ebWeight','ebTotal','netWeight'].map(key=>{
            if(key==='add'||key==='delete')return '<td><button type="button" class="tool-btn" '+(breakupLocked?'disabled':'')+' aria-label="'+(key==='add'?'Add row':'Delete row')+'" onclick="'+(key==='add'?'addBreakupRow()':'deleteBreakupRow('+index+')')+'" onkeydown="breakupKeyDown(event,'+index+')">'+(key==='add'?'+':'X')+'</button></td>';
            const editable=!breakupLocked&&['qty','uom','ebWeight'].includes(key);
            return '<td><input type="number" step="any" aria-label="'+key+' row '+(index+1)+'" value="'+(Number(row[key])||0)+'" '+(editable?'':'readonly')+' data-row="'+index+'" data-key="'+key+'" oninput="updateBreakupCell(this)" onkeydown="breakupKeyDown(event,'+index+')"></td>';
        }).join('')+'</tr>').join('');
        updateBreakupTotals();
    }
    function updateBreakupTotals() { const totals=breakupTotals(); for(const [key,value] of Object.entries(totals))if(field('breakupTotal-'+key))field('breakupTotal-'+key).textContent=value.toFixed(2); }
    /* grdPurchaseBrakup_CellUpdated :1958 - Gross = Qty*UOM, EBTotal = Qty*EbWeight, Net = Gross + EBTotal (as the desktop adds it) */
    function updateBreakupCell(input) {
        if(breakupLocked)return;
        const index=Number(input.dataset.row),row=loadedBreakups[index],key=input.dataset.key;
        if(!row||!['qty','uom','ebWeight'].includes(key))return;
        row[key]=Number(input.value)||0; row.grossWeight=row.qty*row.uom; row.ebTotal=row.qty*row.ebWeight;
        row.netWeight=row.grossWeight+row.ebTotal;
        for(const totalKey of ['grossWeight','ebTotal','netWeight']) { const target=document.querySelector('#grdBreakupBody input[data-row="'+index+'"][data-key="'+totalKey+'"]'); if(target)target.value=row[totalKey]; }
        updateBreakupTotals();
    }
    function addBreakupRow() { if(breakupLocked)return; loadedBreakups.push(emptyBreakup()); renderBreakups(); }
    function deleteBreakupRow(index) { if(breakupLocked)return; loadedBreakups.splice(index,1); if(!loadedBreakups.length)loadedBreakups.push(emptyBreakup()); renderBreakups(); }
    /* grdPurchaseBrakup_KeyDown :5111 */
    function breakupKeyDown(event,index) {
        if(!event.ctrlKey||breakupLocked)return;
        if(event.key.toLowerCase()==='d'){event.preventDefault();addBreakupRow();}
        if(event.key==='Delete'){event.preventDefault();if(confirm('Are you sure to Delete?'))deleteBreakupRow(index);}
        if(event.key===' ' && event.target.tagName==='BUTTON'){event.preventDefault(); if(event.target.textContent.trim()==='X'){ if(confirm('Are you sure to Delete?')) deleteBreakupRow(index); } else event.target.click();}
    }
