
    const context = { documentTypeId: 51 };
    let loadedHeader={}, loadedDetails=[], loadedBreakups=[], lookupData={}, driverBioId=0, formGeneration=0, orderGeneration=0;
    let breakupLocked=false, netPaidEdited=false, driverGeneration=0, transitGeneration=0, transitRows=[];
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
    document.addEventListener('DOMContentLoaded',async()=>{
        try {
            setDefaultDates(); await loadDropdowns();
            const id=Number(new URLSearchParams(location.search).get('id'));
            if(id>0) await loadRecordAndEdit(id); else await onNewRecord();
            await loadMainHistoryGrid();
        } catch(error) { showRequestError(error); }
    });

    function setDefaultDates() {
        const today = localStamp().split('T')[0];
        document.getElementById('txtgpdate').value = today;
        document.getElementById('txtBiltyDate').value = today;
        document.getElementById('txtFromPoDate').value = today;
        document.getElementById('txtToPoDate').value = today;

        const threeDaysAgo = new Date();
        threeDaysAgo.setDate(threeDaysAgo.getDate() - 3);
        const threeDaysAgoIso = localStamp(threeDaysAgo).split('T')[0];
        document.getElementById('txtHistFromDate').value = threeDaysAgoIso;
        document.getElementById('txtHistToDate').value = today;

        const nowIso = localStamp().slice(0, 16);
        document.getElementById('txtintime').value = nowIso;
        document.getElementById('txtouttime').value = nowIso;
    }

    function loadDropdowns() {
        const selected=Object.fromEntries(['cmbsupp','cmbcity','cmbvehicletype','cmbgptype','CmbVariety','cmbWeighBridge','CmbPackingType','CmbOrderType','CmbStatus'].map(id=>[id,field(id).value]));
        return igpFetch('/api/inward-gate-pass/dropdowns')
            .then(res => res.json())
            .then(data => {
                lookupData=data;
                bindSelect('CmbOrderType',data.orderTypes,'id','name');
                bindSelect('cmbsupp', data.suppliers, 'id', 'name');
                bindSelect('cmbcity', data.cities, 'id', 'name');
                bindSelect('cmbvehicletype', data.vehicleTypes, 'id', 'name');
                bindSelect('cmbgptype', data.gatePassTypes, 'id', 'name');
                bindSelect('CmbVariety', data.items, 'id', 'name');
                bindSelect('cmbWeighBridge', data.weighBridges, 'id', 'name');
                bindSelect('CmbPackingType', data.packingTypes, 'id', 'name');
                bindSelect('CmbStatus', data.statuses, 'id', 'name');
                bindSelect('cmbTransitVehicle', data.transitVehicles, 'id', 'name');
                bindSelect('cmbHistSupplier', data.suppliers, 'id', 'name');
                bindSelect('cmbSupplierNamePoInfo', data.suppliers, 'id', 'name');
                bindSelect('CmbDocumentTypePoInfo', data.documentTypes, 'id', 'name');

                if(field('cmbgptype').options.length>2) field('cmbgptype').selectedIndex=2;
                if(field('cmbcity').options.length>1) field('cmbcity').selectedIndex=1;
                if(field('cmbvehicletype').options.length>1) field('cmbvehicletype').selectedIndex=1;
                // Desktop OrderTypeFill activates row 1 after its blank row, preserving procedure order.
        if(field('CmbOrderType').options.length>1) field('CmbOrderType').selectedIndex=1;
                selectDefaultByText('CmbStatus', ['Open']);
                selectDefaultByText('cmbWeighBridge', ['Auto']);
                for(const [id,value] of Object.entries(selected)) if(value && Array.from(field(id).options).some(o=>o.value===value)) field(id).value=value;
            });
    }

    function bindSelect(elemId, list, valKey, textKey) {
        const sel = document.getElementById(elemId);
        if (!sel) return;
        sel.innerHTML = '<option value="">-- Select --</option>';
        if (list) {
            list.forEach(item => {
                const opt = document.createElement('option');
                opt.value = item[valKey];
                opt.textContent = item[textKey];
                sel.appendChild(opt);
            });
        }
    }

    function switchViewMode(mode) {
        const viewForm = document.getElementById('viewForm');
        const viewHistory = document.getElementById('viewHistory');
        const btnForm = document.getElementById('btnModeForm');
        const btnHist = document.getElementById('btnModeHistory');
        const lblTitle = document.getElementById('lblFormHeaderTitle');

        if (mode === 'History') {
            viewForm.style.display = 'none';
            viewHistory.style.display = 'block';
            btnForm.classList.remove('active');
            btnHist.classList.add('active');
            lblTitle.textContent = 'Inward Gate Pass History';
            executeFullHistorySearch();
        } else {
            viewForm.style.display = 'block';
            viewHistory.style.display = 'none';
            btnForm.classList.add('active');
            btnHist.classList.remove('active');
            lblTitle.textContent = 'Inward Gate Pass';
        }
    }

    function onSwitchToFormNew() {
        switchViewMode('Form');
        onNewRecord();
    }

    function selectDefaultByText(elemId, targetTexts) {
        const sel = document.getElementById(elemId);
        if (!sel || sel.options.length <= 1) return;
        for (let text of targetTexts) {
            for (let i = 0; i < sel.options.length; i++) {
                if (sel.options[i].text.toLowerCase().includes(text.toLowerCase())) {
                    sel.selectedIndex = i;
                    return;
                }
            }
        }
        if (sel.options.length > 1) sel.selectedIndex = 1;
    }

    function onNewRecord() {
        const generation=++formGeneration; ++orderGeneration;
        loadedHeader={}; loadedDetails=[]; loadedBreakups=[emptyBreakup()]; driverBioId=0; breakupLocked=false; netPaidEdited=false;
        renderBreakups(); bindSelect('cmbTransitVehicle',[],'id','name');
        document.querySelectorAll('#viewForm input:not([type="radio"]):not([type="checkbox"])').forEach(input=>input.value=input.defaultValue||'');
        setDefaultDates();
        bindSelect('cmbsupp',lookupData.suppliers,'id','name'); bindSelect('CmbVariety',lookupData.items,'id','name');
        document.getElementById('txtId').value = '0';
        document.getElementById('cmbsupp').value = '';
        document.getElementById('CmbVariety').value = '';
        document.getElementById('txtvehicleno').value = '';
        document.getElementById('txtqty').value = '0';
        document.getElementById('txtPackUnit').value = '60';
        document.getElementById('txtWeight').value = '0';
        document.getElementById('txtfreight').value = '0';
        document.getElementById('txtAdvanceByParty').value = '0';
        document.getElementById('txtAdvanceByFactory').value = '0';
        document.getElementById('txtTotalPayablesFreight').value = '0';
        document.getElementById('txtNetPaid').value = '0';
        document.getElementById('txtFactoryWeight').value = '0';
        document.getElementById('txtSupplierFirstWeight').value = '0';
        document.getElementById('txtsecondWeight').value = '0';
        document.getElementById('txtSupplierSecondWeight').value = '0';

        resetDriverInfoFields();
        field('cmbWeighBridge').disabled=false;

        if(field('cmbgptype').options.length>2) field('cmbgptype').selectedIndex=2;
        if(field('cmbcity').options.length>1) field('cmbcity').selectedIndex=1;
        if(field('cmbvehicletype').options.length>1) field('cmbvehicletype').selectedIndex=1;
        // Desktop OrderTypeFill activates row 1 after its blank row, preserving procedure order.
        if(field('CmbOrderType').options.length>1) field('CmbOrderType').selectedIndex=1;
        selectDefaultByText('CmbStatus', ['Open']);
        selectDefaultByText('cmbWeighBridge', ['Auto']);

        calculateNetWeights();
        calculateFreight();

        document.getElementById('btnsave').style.display = 'inline-flex';
        document.getElementById('btnupdate').style.display = 'none';

        return igpFetch('/api/inward-gate-pass/generate-no?gatepassType='+encodeURIComponent(caption('cmbgptype')))
            .then(res => res.json())
            .then(data => {
                if(generation!==formGeneration) return;
                document.getElementById('txtgpno').value = data.gpSrNo ?? ''; 
                document.getElementById('txtgptypeno').value = data.gpTypeSrNo ?? '';
            });
    }

    function calculateWeight() {
        const qty = parseFloat(document.getElementById('txtqty').value) || 0;
        const packUnit = parseFloat(document.getElementById('txtPackUnit').value) || 0;
        document.getElementById('txtWeight').value = (qty * packUnit).toFixed(2);
    }

    function calculateFreight() {
        const biltyFreight = parseFloat(document.getElementById('txtfreight').value) || 0;
        const advParty = parseFloat(document.getElementById('txtAdvanceByParty').value) || 0;
        const advFactory = parseFloat(document.getElementById('txtAdvanceByFactory').value) || 0;
        const totalPayable = biltyFreight - advParty - advFactory;
        document.getElementById('txtTotalPayablesFreight').value = totalPayable.toFixed(2);
        if(!netPaidEdited) document.getElementById('txtNetPaid').value = totalPayable.toFixed(2);
    }

    function calculateNetWeights() {
        const factLoad = parseFloat(document.getElementById('txtFactoryWeight').value) || 0;
        const factTare = parseFloat(document.getElementById('txtsecondWeight').value) || 0;
        const supLoad = parseFloat(document.getElementById('txtSupplierFirstWeight').value) || 0;
        const supTare = parseFloat(document.getElementById('txtSupplierSecondWeight').value) || 0;
        if(supLoad>0 || supTare>0) field('txtSupplierNetWeight').value=Math.abs(supLoad-supTare).toFixed(3);
        const supNet=numeric('txtSupplierNetWeight'), factNet=numeric('txtFactoryNetWeight');
        field('txtFirstWtDifference').value=Math.abs(supLoad-factLoad).toFixed(3);
        field('txtSecondWtDifference').value=Math.abs(supTare-factTare).toFixed(3);
        field('txtDifferenceWeight').value=Math.abs(supNet-factNet).toFixed(3);
    }

    function onSupplierNetInput() {
        field('txtSupplierFirstWeight').value=0; field('txtSupplierSecondWeight').value=0;
        calculateNetWeights();
    }

    const driverFields=['txtCNIC','txtDriverCellNo','txtWhatsAppNo','txtAlternateCellNo','txtDriverName','txtFatherName','txtFatherCNIC'];
    function setDriverLocked(locked) { driverFields.forEach(id=>field(id).disabled=locked); }
    async function lookupDriver(kind,control) {
        const value=field(control).value.trim(), generation=++driverGeneration, form=formGeneration;
        if(!value) { driverBioId=0; setDriverLocked(false); return; }
        try {
            const bio=await (await igpFetch('/api/inward-gate-pass/driver-bio/'+kind+'?'+new URLSearchParams({[kind]:value}))).json();
            if(generation!==driverGeneration || form!==formGeneration || field(control).value.trim()!==value) return;
            driverBioId=Number(bio?.Id)||0; setDriverLocked(driverBioId>0);
            if(!bio) return;
            const map={txtCNIC:'CnicNo',txtDriverCellNo:'DriverCellNo',txtWhatsAppNo:'WhatsappNo',txtAlternateCellNo:'AlternateCellNo',txtDriverName:'DriverName',txtFatherName:'FatherName',txtFatherCNIC:'FatherCnicNo'};
            for(const [id,key] of Object.entries(map)) field(id).value=bio[key]||'';
        } catch(error) { showRequestError(error); }
    }

    function lookupDriverByCnic() { return lookupDriver('cnic','txtCNIC'); }
    function lookupDriverByCell() { return lookupDriver('cell','txtDriverCellNo'); }

    function resetDriverInfoFields() {
        driverBioId=0; ++driverGeneration; setDriverLocked(false);
        document.getElementById('txtCNIC').value = '';
        document.getElementById('txtDriverCellNo').value = '';
        document.getElementById('txtWhatsAppNo').value = '';
        document.getElementById('txtAlternateCellNo').value = '';
        document.getElementById('txtDriverName').value = '';
        document.getElementById('txtFatherName').value = '';
        document.getElementById('txtFatherCNIC').value = '';
    }


    /* ==================================================================================
       The four linked screens on the status strip - InwardGatePass.cs.

       Each desktop handler wraps the open in FormHelper.CanOpenForm("<ScreenName>", ...), a
       per-user screen-rights check. Navigating here hits the web route, whose own controller and
       security apply, so the check is not duplicated client-side - a client-side one would be
       decoration anyway.

       Grn Form is conditional on the desktop (:3843-3863):
           CmbOrderType.Text == "Purchase_Order_PM"  ->  GrnPackingMaterial
           otherwise                                  ->  InvFrmGRN
       The web has no Packing Material GRN screen yet, so that branch says so rather than opening
       the wrong GRN.
       ================================================================================== */
    var IGP_LINKED_FORMS = {
        /* InvLabPurchaseAnalysis  (RadPurchaseLabFormToOpenOnInsert_Click, :3816) */
        LabAnalysis:    { url: '/quality/purchase-analysis', label: 'Lab Purchase Analysis' },
        /* frmWeightbridge         (RadWBFormToOpenOnInsert_Click, :3827) */
        WeightBridge:   { url: '/weighbridge/weight-bridge', label: 'Weigh Bridge' },   /* screen 411 page; '/weighbridge' is now the application hub */
        /* InvFrmGRN               (RadGrnFormToOpenOnInsert_Click, :3843) */
        Grn:            { url: '/purchase/goods-receipt-notes', label: 'GRN' },
        /* FreightVoucher          (FreightVoucherFormToOpen_Click, :3865) */
        FreightVoucher: { url: '/accounts/vouchers/freight', label: 'Freight Voucher' }
    };

    /* Opens the target screen in a new tab, the way the desktop opens a second form rather than
       replacing the one you are on - the gate pass you are entering is not lost. */
    function igpOpenLinkedForm(key, gatePassId) {
        var target = IGP_LINKED_FORMS[key];
        if (!target) return false;

        if (key === 'Grn') {
            var ot = document.getElementById('CmbOrderType');
            var otText = ot && ot.selectedOptions && ot.selectedOptions.length
                       ? ot.selectedOptions[0].textContent.trim() : '';
            if (otText === 'Purchase_Order_PM') {
                alert('Order Type is Purchase_Order_PM, which opens the Packing Material GRN on the '
                    + 'desktop (GrnPackingMaterial). That screen has not been built yet, so the '
                    + 'ordinary GRN was NOT opened in its place.');
                return false;
            }
        }

        var url = target.url;
        /* OpenLinkformOnInsert passes the new GatePassId to the target so it opens already
           filtered to this gate pass. Only sent when there is one. */
        if (gatePassId) {
            url += (url.indexOf('?') >= 0 ? '&' : '?') + 'gatePassId=' + encodeURIComponent(gatePassId);
        }
        window.open(url, '_blank', 'noopener');
        return false;
    }

    /* OpenLinkformOnInsert(GatePassId, Status) - :3897-3915. Called after a SUCCESSFUL save.
       Note the Grn branch alone also requires Status == "Accepted", and that after opening the
       Grn form the desktop resets the selection to None (:3796-3799). Both reproduced. */
    function igpOpenLinkedFormAfterSave(gatePassId, status) {
        var sel = document.querySelector('input[name="formMode"]:checked');
        var mode = sel ? sel.value : 'None';
        if (mode === 'None') return;
        if (mode === 'Grn' && String(status || '').trim() !== 'Accepted') return;
        igpOpenLinkedForm(mode, gatePassId);
        if (mode === 'Grn') {
            var none = document.querySelector('input[name="formMode"][value="None"]');
            if (none) none.checked = true;
        }
    }

    function onSaveRecord() {
        const payload = {
            ...loadedHeader,
            id: parseInt(document.getElementById('txtId').value) || 0,
            documentTypeId: context.documentTypeId,
            gpDate: document.getElementById('txtgpdate').value,
            gpSrNo: parseInt(document.getElementById('txtgpno').value) || 0,
            gpTypeSrNo: parseInt(document.getElementById('txtgptypeno').value) || 0,
            gatepassType: caption('cmbgptype'),
            supplierCustomerId: parseInt(document.getElementById('cmbsupp').value) || 0,
            cityId: parseInt(document.getElementById('cmbcity').value) || 0,
            itemId: parseInt(document.getElementById('CmbVariety').value) || 0,
            vehicleNo: document.getElementById('txtvehicleno').value,
            vehicleType: caption('cmbvehicletype'),
            biltyNo: document.getElementById('txtbiltyno').value,
            biltyDate: document.getElementById('txtBiltyDate').value,
            freight: parseFloat(document.getElementById('txtfreight').value) || 0,
            advanceByParty: parseFloat(document.getElementById('txtAdvanceByParty').value) || 0,
            advanceByFactory: parseFloat(document.getElementById('txtAdvanceByFactory').value) || 0,
            netPaid: parseFloat(document.getElementById('txtNetPaid').value) || 0,
            supplierFirstWeight: parseFloat(document.getElementById('txtSupplierFirstWeight').value) || 0,
            supplierSecondWeight: parseFloat(document.getElementById('txtSupplierSecondWeight').value) || 0,
            supplierWeight: parseFloat(document.getElementById('txtSupplierNetWeight').value) || 0,
            factoryWeight: parseFloat(document.getElementById('txtFactoryNetWeight').value) || 0,
            differenceWeight: parseFloat(document.getElementById('txtDifferenceWeight').value) || 0,
            status: document.getElementById('CmbStatus').value || 'Open',
            packingTypeId: parseInt(document.getElementById('CmbPackingType').value) || 0,
            packUnit: parseFloat(document.getElementById('txtPackUnit').value) || 60,
            noOfPackages: parseInt(document.getElementById('txtqty').value) || 0,
            driverName: document.getElementById('txtDriverName').value,
            driverCNICNO: document.getElementById('txtCNIC').value,
            driverMobileNo: document.getElementById('txtDriverCellNo').value,
            whatsappNo: document.getElementById('txtWhatsAppNo').value,
            alternateCellNo: document.getElementById('txtAlternateCellNo').value,
            fatherName: document.getElementById('txtFatherName').value,
            fatherCnicNo: document.getElementById('txtFatherCNIC').value,
            docAttachment: caption('cmbWeighBridge'),
            weighBridgeId: Number(loadedHeader.weighBridgeId)||0,
            inDateTimeStamp: field('txtintime').value||null,
            outDateTimeStamp: field('txtouttime').value||null,
            supplierDispatchId: numeric('cmbTransitVehicle'),
            otherRemarks: field('txtremarks').value,
            otherSupCust: caption('CmbOrderType'),
            refDocumentTypeId: numeric('CmbOrderType'),
            supplierContractCode: field('CmbOrderno').value,
            varietyName: caption('CmbVariety'),
            weightComparedToPoWt: numeric('txtWeight'),
            accessWeight: numeric('txtAccessWeight'),
            driverBioDataId: driverBioId,
            gatePassInwardPurchaseBreakUpList: loadedBreakups.filter(row=>Number(row.qty)>0),
            // The desktop main item is a header field, not a fabricated detail-grid row.
            gatePassInwardDetails: loadedDetails
        };

        return igpFetch('/api/inward-gate-pass/save', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    alert(data.message);
                    document.getElementById('txtId').value = data.igpId;
                    document.getElementById('btnsave').style.display = 'none';
                    document.getElementById('btnupdate').style.display = 'inline-flex';
                    field('txtgpno').value=data.gpSrNo; field('txtgptypeno').value=data.gpTypeSrNo;
                    /* OpenLinkformOnInsert(success, status) - :3791 */
                    var st = document.getElementById('CmbStatus');
                    igpOpenLinkedFormAfterSave(data.igpId,
                        st && st.selectedOptions && st.selectedOptions.length
                            ? st.selectedOptions[0].textContent.trim() : '');
                    return loadRecordAndEdit(data.igpId).then(loadMainHistoryGrid);
                } else {
                    alert(data.message);
                }
            });
    }

    function loadMainHistoryGrid() {
        return igpFetch('/api/inward-gate-pass/open-records')
            .then(res => res.json())
            .then(data => {
                const tbody = document.getElementById('grdMainHistoryBody');
                tbody.innerHTML = '';
                let totalQty = 0, totalSupWt = 0, totalDiff = 0;

                data.forEach(row => {
                    totalQty += parseFloat(row.ItemQty) || 0;
                    totalSupWt += parseFloat(row.SupplierWeight) || 0;
                    totalDiff += parseFloat(row.DifferenceWeight) || 0;

                    const tr = document.createElement('tr');
                    tr.ondblclick = function() { loadRecordAndEdit(row.Id); };
                    tr.innerHTML = `
                    <td><button class="tool-btn" style="padding:1px 4px;" onclick="onPrintReport(${row.Id})">Print</button></td>
                    <td><button class="tool-btn" style="padding:1px 4px;" onclick="loadRecordAndEdit(${row.Id})">Edit</button></td>
                    <td><span class="code-link" onclick="loadRecordAndEdit(${row.Id})">${row.GpSrNo || ''}</span></td>
                    <td>${row.GpDate ? row.GpDate.split('T')[0] : ''}</td>
                    <td>${escapeHtml(row.GatepassType || '')}</td>
                    <td>${escapeHtml(row.OrderType || '')}</td>
                    <td>${escapeHtml(row.SupplierName || row.CompanyName || '')}</td>
                    <td>${escapeHtml(row.OrderNo || '')}</td>
                    <td>${escapeHtml(row.VehicleType || '')}</td>
                    <td>${escapeHtml(row.VehicleNo || '')}</td>
                    <td>${escapeHtml(row.BiltyNo || '')}</td>
                    <td style="text-align:right;">${row.ItemQty || 0}</td>
                    <td style="text-align:right;">${row.PackUnit || 0}</td>
                    <td style="text-align:right;">${row.WeightComparedToPoWt || 0}</td>
                    <td style="text-align:right;">${row.SupplierFirstWeight || 0}</td>
                    <td style="text-align:right;">${row.SupplierSecondWeight || 0}</td>
                    <td style="text-align:right;">${row.SupplierWeight || 0}</td>
                    <td style="text-align:right;">${row.FactoryWeight || 0}</td>
                    <td style="text-align:right; color:red;">${row.DifferenceWeight || 0}</td>
                    <td style="text-align:right;">${row.AccessWeight || 0}</td>
                    <td style="text-align:right;">${row.NetPaid || 0}</td>
                    <td>${escapeHtml(row.VarietyName || row.ItemName || '')}</td>
                    <td>${escapeHtml(row.CityName || row.Description || '')}</td>
                `;
                    tbody.appendChild(tr);
                });

                document.getElementById('lblRecordCount').textContent = data.length;
                document.getElementById('lblSumQty').textContent = totalQty.toFixed(2);
                document.getElementById('lblSumSupWt').textContent = totalSupWt.toFixed(2);
                document.getElementById('lblSumDiff').textContent = totalDiff.toFixed(2);
            });
    }

    function executeFullHistorySearch() {
        const payload = {
            documentTypeId: 51,
            dateField: document.querySelector('input[name="histDateFilter"]:checked')?.value||'docDate',
            fromDate: document.getElementById('txtHistFromDate').value,
            toDate: document.getElementById('txtHistToDate').value,
            fromDocNo: document.getElementById('txtHistFromDoc').value,
            toDocNo: document.getElementById('txtHistToDoc').value,
            supplierId: document.getElementById('cmbHistSupplier').value
        };

        return igpFetch('/api/inward-gate-pass/history', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
            .then(res => res.json())
            .then(data => {
                const tbody = document.getElementById('grdFullHistoryBody');
                tbody.innerHTML = '';

                /* InwardGatePass.cs gridhistory() :4455 — every dtcol column, in the desktop's order, from the
                   proc's own column names (CompanyName, Description, UserName, ModifyUserName, OtherRemarks).
                   GpDate is ToShortDateString; In/Out/Entry/Modify use "dd-MM-yyyy hh:mm tt". */
                const pad = n => String(n).padStart(2, '0');
                const dt = v => { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); if (isNaN(d)) return escapeHtml(v);
                    let h = d.getHours(); const ap = h >= 12 ? 'pm' : 'am'; h = h % 12 || 12;
                    return pad(d.getDate()) + '-' + pad(d.getMonth() + 1) + '-' + d.getFullYear() + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + ap; };
                const day = v => { if (!v) return ''; const d = new Date(String(v).replace(' ', 'T')); return isNaN(d) ? escapeHtml(v) : pad(d.getDate()) + '-' + pad(d.getMonth() + 1) + '-' + d.getFullYear(); };
                const num = v => '<td style="text-align:right;">' + (v === null || v === undefined ? '' : escapeHtml(v)) + '</td>';
                const txt = v => '<td>' + escapeHtml(v ?? '') + '</td>';
                data.forEach(row => {
                    const tr = document.createElement('tr');
                    tr.ondblclick = function() { loadRecordAndEdit(row.Id); };
                    const approved = row.ApprovalStatus === 'Approved';
                    tr.innerHTML =
                        `<td><button class="tool-btn" style="padding:1px 4px;" onclick="onPrintReport(${Number(row.Id)})">Print</button></td>`
                      + `<td><button class="tool-btn" style="padding:1px 4px;" onclick="loadRecordAndEdit(${Number(row.Id)})">Edit</button></td>`
                      + `<td>${escapeHtml(row.GpSrNo ?? '')}</td>`
                      + `<td>${day(row.GpDate)}</td>`
                      + txt(row.GatepassType) + txt(row.OrderType) + num(row.OrderNo) + txt(row.CompanyName) + txt(row.Description)
                      + txt(row.VehicleType) + txt(row.VehicleNo) + `<td>${day(row.BiltyDate)}</td>` + txt(row.BiltyNo) + txt(row.VarietyName)
                      + num(row.ItemQty) + num(row.PackUnit) + num(row.WeightComparedToPoWt) + num(row.SupplierFirstWeight) + num(row.SupplierSecondWeight)
                      + num(row.SupplierWeight) + num(row.FactoryWeight) + `<td style="text-align:right; color:red;">${escapeHtml(row.DifferenceWeight ?? '')}</td>`
                      + num(row.Freight) + num(row.AdvanceByParty) + num(row.AdvanceByFactory) + num(row.TotalPayableFreight) + num(row.NetPaid)
                      + `<td>${dt(row.InTime)}</td><td>${dt(row.OutTime)}</td>` + txt(row.Status) + txt(row.UserName) + `<td>${dt(row.EntryDate)}</td>`
                      + txt(row.ModifyUserName) + `<td>${dt(row.ModifyDate)}</td>`
                      + `<td style="text-align:right;"><a href="#" onclick="event.preventDefault();loadRecordAndEdit(${Number(row.Id)})">${escapeHtml(row.NoOfAttachments ?? 0)}</a></td>`
                      + txt(row.OtherRemarks) + `<td style="color:${approved ? 'green' : 'red'};">${escapeHtml(row.ApprovalStatus ?? '')}</td>`
                      + `<td style="text-align:right; color:${Number(row.AccessWeight) > 0 ? 'red' : 'green'};">${escapeHtml(row.AccessWeight ?? 0)}</td>`
                      + txt(row.PackingType);
                    tbody.appendChild(tr);
                });
            });
    }

    function resetPoInfoFilters() {
        const today = localStamp().split('T')[0];
        if (field('txtFromPoDate')) field('txtFromPoDate').value = today;
        if (field('txtToPoDate')) field('txtToPoDate').value = today;
        if (field('chkFromPoDate')) { field('chkFromPoDate').checked = true; field('txtFromPoDate').disabled = false; }
        if (field('chkToPoDate')) { field('chkToPoDate').checked = true; field('txtToPoDate').disabled = false; }
        if (field('txtFromDocNoPoInfo')) field('txtFromDocNoPoInfo').value = '';
        if (field('txtToDocNoPoInfo')) field('txtToDocNoPoInfo').value = '';
        if (field('cmbSupplierNamePoInfo')) field('cmbSupplierNamePoInfo').value = '';
        if (field('CmbDocumentTypePoInfo')) field('CmbDocumentTypePoInfo').value = '';
        if (field('txtExpiryDaysPoInfo')) field('txtExpiryDaysPoInfo').value = '7';
        const docRadio = document.querySelector('input[name="poDateFilter"][value="docDate"]');
        if (docRadio) docRadio.checked = true;
    }

    function loadPoInfoGrid() {
        const payload = {
            fromDate: field('chkFromPoDate')?.checked ? field('txtFromPoDate').value : '',
            toDate: field('chkToPoDate')?.checked ? field('txtToPoDate').value : '',
            fromDocNo: field('txtFromDocNoPoInfo')?.value || 0,
            toDocNo: field('txtToDocNoPoInfo')?.value || 0,
            supplierId: field('cmbSupplierNamePoInfo')?.value || 0,
            documentTypeId: field('CmbDocumentTypePoInfo')?.value || 0,
            expiryDays: field('txtExpiryDaysPoInfo')?.value || 7,
            dateField: document.querySelector('input[name="poDateFilter"]:checked')?.value || 'docDate'
        };
        return igpFetch('/api/inward-gate-pass/po-info', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        })
            .then(res => res.json())
            .then(data => {
                const tbody = document.getElementById('grdPoInfoBody');
                tbody.innerHTML = '';
                data.forEach(row => {
                    const tr = document.createElement('tr');
                    tr.innerHTML = ['BranchName','DocumentTypeDescription','OrderNo','SupplierName','ItemName','PackUom','OrderQty','OrderWeight','ReceivedQty','ReceivedWeight','BalQty','BalWeight','GrnReceivedQty','GrnReceivedWeight','BalQtyByGrn','BalWeightByGrn','RemarksHeader','ApprovedStatus','OrderStatus','OrderExpiryDate']
                        .map(key=>'<td>'+escapeHtml(key==='OrderExpiryDate'?String(row[key]||'').slice(0,10):(row[key]??''))+'</td>').join('');
                    tbody.appendChild(tr);
                });
            });
    }

    function loadRecordAndEdit(id) {
        const generation=++formGeneration; ++orderGeneration;
        switchViewMode('Form');
        return igpFetch('/api/inward-gate-pass/'+Number(id)).then(res=>res.json()).then(res=>{
            if(generation!==formGeneration) return;
            if(!res.success||!res.header) throw Error(res.message||'Record not found');
            const h=res.header; loadedHeader=lowerKeys(h);
            loadedDetails=(res.details||[]).map(lowerKeys); loadedBreakups=(res.purchaseBreakUps||[]).map(lowerKeys);
            breakupLocked=loadedBreakups.length>0; if(!loadedBreakups.length) loadedBreakups=[emptyBreakup()]; renderBreakups();
            driverBioId=Number(h.driverBioDataId)||0;
            setDriverLocked(driverBioId>0);
            const map={txtId:'Id',txtgpno:'GpSrNo',txtgptypeno:'GpTypeSrNo',cmbsupp:'SupplierCustomerId',cmbcity:'CityId',CmbVariety:'ItemId',txtvehicleno:'VehicleNo',txtbiltyno:'BiltyNo',txtfreight:'Freight',txtAdvanceByParty:'AdvanceByParty',txtAdvanceByFactory:'AdvanceByFactory',txtNetPaid:'NetPaid',txtSupplierFirstWeight:'SupplierFirstWeight',txtSupplierSecondWeight:'SupplierSecondWeight',txtFactoryWeight:'FactoryWeight',txtSupplierNetWeight:'SupplierWeight',txtDriverName:'DriverName',txtCNIC:'DriverCNICNO',txtDriverCellNo:'DriverMobileNo',txtWhatsAppNo:'whatsappNo',txtAlternateCellNo:'AlternateCellNo',txtFatherName:'FatherName',txtFatherCNIC:'fatherCnicNo',txtremarks:'OtherRemarks',CmbPackingType:'PackingTypeId',txtAccessWeight:'AccessWeight',txtWeighBridgeSlipNo:'WeighBridgeId',CmbOrderno:'SupplierContractCode',CmbOrderType:'RefDocumentTypeId',CmbStatus:'Status',txtqty:'NoOfPackages',txtPackUnit:'PackUnit'};
            for(const [control,key] of Object.entries(map)) field(control).value=h[key]??'';
            setCaption('cmbgptype',h.GatepassType); setCaption('cmbvehicletype',h.VehicleType); setCaption('cmbWeighBridge',h.DocAttachment);
            for(const [control,key] of [['txtgpdate','GpDate'],['txtBiltyDate','BiltyDate']]) field(control).value=String(h[key]||'').slice(0,10);
            for(const [control,key] of [['txtintime','InDateTimeStamp'],['txtouttime','OutDateTimeStamp']]) field(control).value=h[key]?localStamp(new Date(h[key])).slice(0,16):'';
            const wb=res.weighBridgeWeights||[], sum=key=>wb.reduce((total,row)=>total+(Number(row[key])||0),0);
            field('txtFactoryWeight').value=sum('FirstWeight'); field('txtsecondWeight').value=sum('SecondWeight');
            field('txtFactoryNetWeight').value=wb.length?sum('NetWbWeight'):Number(h.FactoryWeight)||0;
            field('txtWeighBridgeSlipNo').value=wb.map(row=>row.TicketNo).join(', ');
            field('cmbWeighBridge').disabled=wb.length>0 && sum('NetWbWeight')!==0;
            calculateWeight(); calculateNetWeights();
            field('txtTotalPayablesFreight').value=(Number(h.Freight||0)-Number(h.AdvanceByParty||0)-Number(h.AdvanceByFactory||0)).toFixed(2);
            field('txtNetPaid').value=h.NetPaid??0;
            netPaidEdited=Number(h.NetPaid)!==0;
            field('btnsave').style.display='none'; field('btnupdate').style.display='inline-flex';
            return loadTransitVehicles(Number(h.SupplierDispatchId)||0);
        }).catch(error=>{showRequestError(error);throw error;});
    }

    function switchMainSubTab(tabId) {
        document.querySelectorAll('.tab-container .tab-btn').forEach(btn => btn.classList.remove('active'));
        document.querySelectorAll('.tab-container .tab-content').forEach(c => c.classList.remove('active'));

        if (tabId === 'tabGridHistory') {
            document.querySelectorAll('.tab-container .tab-btn')[0].classList.add('active');
            document.getElementById('tabGridHistory').classList.add('active');
        } else {
            document.querySelectorAll('.tab-container .tab-btn')[1].classList.add('active');
            document.getElementById('tabPoInfo').classList.add('active');
            loadPoInfoGrid();
        }
    }

    function onRefreshForm() { return loadDropdowns().then(loadMainHistoryGrid); }
    function onDefineCity() { alert('Define City form dialog placeholder'); }
    function onDefineVehicle() { alert('Define Vehicle form dialog placeholder'); }
    function onOpenDriverForm() { alert('Driver Biodata form dialog placeholder'); }
    function onOpenAttachments() { alert('Attachments dialog placeholder'); }
    function onPrintReport(id) { window.print(); }
    function onPrintSlip() { window.print(); }
    function onLabReport() { alert('Printing Lab Report 653-LabReport...'); }
    function onShowShortcuts() { alert('Shortcuts:\nF2: New\nF5: Refresh\nCtrl+S: Save / Update\nCtrl+U: Update\nPurchase Breakup: Ctrl+D adds a row; Ctrl+Delete removes a row; Ctrl+Space activates the selected row button.'); }

    function onGpTypeChange() {
        const generation=formGeneration, type=caption('cmbgptype');
        if(numeric('txtId')>0) return Promise.resolve();
        return igpFetch('/api/inward-gate-pass/generate-no?gatepassType='+encodeURIComponent(type)).then(r=>r.json()).then(data=>{
            if(generation===formGeneration && type===caption('cmbgptype')) field('txtgptypeno').value=data.gpTypeSrNo;
        }).catch(showRequestError);
    }
    function onItemChange() { calculateWeight(); }
    function onOrderNumberLeave() {
        const generation=++orderGeneration, number=numeric('CmbOrderno');
        if(numeric('CmbOrderType')!==41 || !number) return Promise.resolve();
        return igpFetch('/api/inward-gate-pass/order-party-items?'+new URLSearchParams({number,date:field('txtgpdate').value,gatePassId:numeric('txtId')})).then(r=>r.json()).then(rows=>{
            if(generation!==orderGeneration)return;
            if(!rows.length){loadedHeader.purchaseOrderId=0;bindSelect('cmbsupp',[],'id','name');bindSelect('CmbVariety',[],'id','name');return;}
            loadedHeader.purchaseOrderId=rows[0].PurchaseOrderId;
            bindSelect('cmbsupp',rows,'Id','CompanyName'); bindSelect('CmbVariety',rows,'ItemId','ItemName');
            field('cmbsupp').selectedIndex=1;field('CmbVariety').selectedIndex=1;
            setCaption('cmbgptype',rows[0].OrderCategoryName);setCaption('cmbcity',rows[0].CityArea);
            return loadTransitVehicles();
        }).catch(showRequestError);
    }
    document.addEventListener('keydown',event=>{
        if(event.ctrlKey && event.key.toLowerCase()==='s'){event.preventDefault();withButtonLoading(field(numeric('txtId')>0?'btnupdate':'btnsave'),onSaveRecord);}
        if(event.key==='F5'){event.preventDefault();withButtonLoading(null,onRefreshForm);}
        if(event.key==='F2'){event.preventDefault();withButtonLoading(null,onNewRecord);}
        if(event.ctrlKey && event.key.toLowerCase()==='u' && numeric('txtId')>0){event.preventDefault();withButtonLoading(field('btnupdate'),onSaveRecord);}
    });

    async function loadTransitVehicles(selectedId=numeric('cmbTransitVehicle')) {
        const generation=++transitGeneration, form=formGeneration;
        const supplierId=numeric('cmbsupp'), orderId=Number(loadedHeader.purchaseOrderId)||0;
        if(!supplierId && !orderId) { transitRows=[]; bindSelect('cmbTransitVehicle',[],'id','name'); return; }
        const rows=await (await igpFetch('/api/inward-gate-pass/transit-vehicles?'+new URLSearchParams({supplierId,orderId,gatePassId:numeric('txtId')}))).json();
        if(generation!==transitGeneration || form!==formGeneration) return;
        transitRows=rows; bindSelect('cmbTransitVehicle',rows,'id','name'); field('cmbTransitVehicle').value=selectedId||'';
    }
    function onTransitVehicleChange() {
        const row=transitRows.find(r=>Number(r.id)===numeric('cmbTransitVehicle')); if(!row)return;
        if(row.VehicleNo) field('txtvehicleno').value=row.VehicleNo;
        if(row.BiltyNo) field('txtbiltyno').value=row.BiltyNo;
        if(Number(row.CityId)>0) field('cmbcity').value=row.CityId;
        if(Number(row.Freight)>0) { field('txtfreight').value=row.Freight; if(Number(row.AdvanceFreight)>0)field('txtAdvanceByParty').value=row.AdvanceFreight; }
        calculateFreight();
    }

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
    function updateBreakupCell(input) {
        if(breakupLocked)return;
        const index=Number(input.dataset.row),row=loadedBreakups[index],key=input.dataset.key;
        if(!row||!['qty','uom','ebWeight'].includes(key))return;
        row[key]=Number(input.value)||0; row.grossWeight=row.qty*row.uom; row.ebTotal=row.qty*row.ebWeight;
        row.netWeight=row.grossWeight+row.ebTotal; // Desktop deliberately adds empty-bag weight.
        for(const totalKey of ['grossWeight','ebTotal','netWeight']) { const target=document.querySelector('#grdBreakupBody input[data-row="'+index+'"][data-key="'+totalKey+'"]'); if(target)target.value=row[totalKey]; }
        updateBreakupTotals();
    }
    function addBreakupRow() { if(breakupLocked)return; loadedBreakups.push(emptyBreakup()); renderBreakups(); }
    function deleteBreakupRow(index) { if(breakupLocked)return; loadedBreakups.splice(index,1); if(!loadedBreakups.length)loadedBreakups.push(emptyBreakup()); renderBreakups(); }
    function breakupKeyDown(event,index) {
        if(!event.ctrlKey||breakupLocked)return;
        if(event.key.toLowerCase()==='d'){event.preventDefault();addBreakupRow();}
        if(event.key==='Delete'){event.preventDefault();if(confirm('Are you sure to Delete?'))deleteBreakupRow(index);}
        if(event.key===' ' && event.target.tagName==='BUTTON'){event.preventDefault();event.target.click();}
    }
