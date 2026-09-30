/* ============================================================================================
 * Goods Dispatching Note (cmagt, DocumentTypeId 1055) - frmGoodsDispatchingNoteCmagt.cs
 *
 *  - Load GRN dialog      = frmPendingGrnLoadingChallanLoader.cs (BLL 0488 PendingDataLoaderForGdn)
 *  - LoadInGridDetail     = form :3465-3563, BindFromGrn :3565-3582, CalculationsCallForDetail :3584
 *  - Detail grid          = GdnBuyerDispatch_Helper.InitializeDetailtable / DetailGridCommonSetting,
 *                           EditColumnsList :1321, grdDetail_CellUpdated :1442-1500
 *  - Empty bags / expense = GetEmptyBagDetailOnBaseOfGrnDetailSO :882, GetExpensesOnBaseOfGrnDetailSO :926,
 *                           grdInvExp_CellUpdated :1624, Add/Delete buttons :1502/:1515/:1667/:1734
 *  - Save                 = Insert() :1812-2070 (the server re-applies every rule)
 *  - Ship To Address      = CommonBindings.BindShipToAddressAgainstBuyer, CmbShipToAddress_Leave :1216
 *
 * Dates are formatted locally (never toISOString - that is UTC).
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/commission/goods-dispatching-note';
    var DD = '/api/commission/dropdowns';

    var detailRows = [];     // dtDetail
    var emptyBagRows = [];   // dtEmptyBags
    var expenseRows = [];    // dtExpGrid
    var loadedDataSet = null;   // LoadedDataSetToGetExpenseAndEbDetail (Tables[1], Tables[2])
    var loadedHeader = null;
    var recId = 0;
    /* private static bool CalculateByEbUnit = true / CalculateByEbTotal = false (:93-95) */
    var calcByEbUnit = true, calcByEbTotal = false;
    var lateCfg = { blockEntry: false, showWarning: false };
    var packingTypes = [], emptyBagItems = [], otherItems = [];
    var loader = { rows: [], emptyBags: [], expenses: [], selectedGrnId: 0 };

    /* ------------------------------------------------------------------ helpers */
    function $id(id) { return document.getElementById(id); }
    function pick(o, k) {
        if (!o) return undefined;
        if (o[k] !== undefined) return o[k];
        var lk = k.toLowerCase();
        for (var key in o) if (key.toLowerCase() === lk) return o[key];
        return undefined;
    }
    function num(v) {
        if (v === null || v === undefined || v === '') return 0;
        var n = parseFloat(String(v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    }
    function int(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function r3(v) { return Math.round((v + Number.EPSILON) * 1000) / 1000; }
    function fmt(v) { return num(v).toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function ymdLocal(d) {
        return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
    }
    function ymd(v) { return v ? String(v).substring(0, 10) : ''; }
    function dmy(v) {
        var s = ymd(v);
        if (!s || s.indexOf('1900-01-01') === 0 || s.indexOf('0001-01-01') === 0) return '';
        var p = s.split('-');
        return p.length === 3 ? p[2] + '-' + p[1] + '-' + p[0] : s;
    }
    function dayNum(s) {   // yyyy-mm-dd -> comparable local day number
        s = ymd(s);
        if (!/^\d{4}-\d{2}-\d{2}$/.test(s)) return null;
        var p = s.split('-');
        return Math.round(new Date(+p[0], +p[1] - 1, +p[2]).getTime() / 86400000);
    }
    function getJSON(url) {
        return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } })
            .then(function (r) { if (!r.ok) throw new Error('HTTP ' + r.status); return r.json(); });
    }
    function sendJSON(url, method, body) {
        return fetch(url, {
            method: method, credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
            body: body === undefined ? undefined : JSON.stringify(body)
        }).then(function (r) { if (!r.ok) throw new Error('HTTP ' + r.status); return r.json(); });
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) { e.value = (v === undefined || v === null) ? '' : v; } }
    function setSel(id, v) {
        var e = $id(id); if (!e) return;
        var s = String(int(v));
        e.value = s;
        if (e.value !== s) e.value = '0';
        e.dispatchEvent(new Event('change', { bubbles: true }));
    }
    /* A stored id whose list does not carry it is posted as stored, not wiped to 0 on Update. */
    function selInt(id) {
        var e = $id(id);
        if (!e) return 0;
        var v = int(e.value);
        if (!v && loadedHeader) {
            var stored = int(pick(loadedHeader, e.getAttribute('data-col') || id));
            if (stored && !e.querySelector('option[value="' + stored + '"]')) return stored;
        }
        return v;
    }
    function selText(id) {
        var e = $id(id);
        if (!e || !int(e.value) || e.selectedIndex < 0) return '';
        return e.options[e.selectedIndex].textContent;
    }
    function fillSelect(el, list, placeholder, optFn) {
        if (typeof el === 'string') el = $id(el);
        if (!el) return;
        var keep = el.value;
        var h = '<option value="0">' + esc(placeholder || '') + '</option>';
        (list || []).forEach(function (d) {
            h += optFn ? optFn(d) : '<option value="' + esc(pick(d, 'id')) + '">' + esc(pick(d, 'name')) + '</option>';
        });
        el.innerHTML = h;
        if (keep) { el.value = keep; if (el.value !== keep) el.value = '0'; }
        /* a list that arrives after ReadById still shows the stored id */
        if (el.value === '0' && loadedHeader && el.id) {
            var stored = int(pick(loadedHeader, el.getAttribute('data-col') || el.id));
            if (stored && el.querySelector('option[value="' + stored + '"]')) el.value = String(stored);
        }
    }
    function partyOption(d) {
        return '<option value="' + esc(d.Id || d.id) + '"'
            + ' data-code="' + esc(d.PartyCode || '') + '"'
            + ' data-city="' + esc(d.CityName || '') + '"'
            + ' data-mobile="' + esc(d.MobileNo || '') + '">' + esc(d.CompanyName || d.name) + '</option>';
    }
    function fire(el) { if (el) el.dispatchEvent(new Event('change', { bubbles: true })); }

    /* ------------------------------------------------------------------ dropdowns */
    function loadDropdowns() {
        getJSON(DD + '/buyers').then(function (d) { fillSelect('buyerId', d, '-- Select Buyer --', partyOption); }).catch(function () {});
        getJSON(DD + '/commission-agents').then(function (d) { fillSelect('commissionAgentId', d, '-- Select Commission Agent --', partyOption); }).catch(function () {});
        getJSON(DD + '/companies').then(function (d) { fillSelect('companyNameId', d, '-- Select Company --'); }).catch(function () {});
        getJSON(DD + '/branches').then(function (d) { fillSelect('branchSelectId', d, '-- Select Branch --'); }).catch(function () {});
        getJSON(DD + '/vehicle-types').then(function (d) { fillSelect('vehicleTypeId', d, '-- Select Vehicle Type --'); }).catch(function () {});
        getJSON(DD + '/delivery-terms').then(function (d) {
            fillSelect('deliveryTermId', d, '-- Select Delivery Term --');
            fillSelect('deliveryTermGrnId', d, '');
        }).catch(function () {});
        getJSON(DD + '/delivery-parties').then(function (d) {
            fillSelect('transporterId', d, '-- Select Transporter --', partyOption);
            fillSelect('deliverToPartyId', d, '-- Select Party --', partyOption);
        }).catch(function () {});
        getJSON(DD + '/cities').then(function (d) {
            fillSelect('loadingCityId', d, '-- Select City --');
            fillSelect('unloadingCityId', d, '-- Select City --');
        }).catch(function () {});
        /* BindViewCombos :776-800 - AllocatedPackingType feeds the empty-bag Packing Type column */
        getJSON(DD + '/view-combos').then(function (d) {
            packingTypes = (d || []).filter(function (r) { return r.Activity === 'AllocatedPackingType'; });
            renderEmptyBags();
        }).catch(function () {});
        /* CommonServices.GetPackingMaterialItemsAllocateToFlow(2) (:532) */
        getJSON(DD + '/empty-bag-items?transactionFlowId=2').then(function (d) { emptyBagItems = d || []; renderEmptyBags(); }).catch(function () {});
        /* InventoryItemsOther.GetAll (:1080-1090) */
        getJSON(DD + '/other-items').then(function (d) { otherItems = d || []; renderExpenses(); }).catch(function () {});
        getJSON(API + '/late-vehicle-config').then(function (c) { lateCfg = c || lateCfg; }).catch(function () {});
    }

    /* CmbDeliveryToParty_Leave :1068 -> BindShipToAddressAgainstBuyer (party 0 = every address;
       a single row is activated). */
    var shipToSync = false;
    function bindShipTo(partyId, selectId) {
        return getJSON(DD + '/ship-to-addresses?supplierCustomerId=' + (int(partyId) || 0)).then(function (d) {
            var el = $id('deliverToAddressId');
            var list = d || [];
            var h = '<option value="0">-- Select Address --</option>';
            list.forEach(function (r) {
                h += '<option value="' + esc(r.Id) + '" data-party-id="' + esc(r.SupplierCustomerId)
                    + '" data-party-name="' + esc(r.CompanyName) + '">' + esc(r.AddressLine1) + '</option>';
            });
            el.innerHTML = h;
            var want = int(selectId);
            if (!want && list.length === 1) want = list[0].Id;
            el.value = String(want || 0);
            if (el.value !== String(want || 0)) el.value = '0';
            fire(el);
        }).catch(function () {});
    }
    /* CmbShipToAddress_Leave :1216-1238 */
    function onShipToChange() {
        if (shipToSync) return;
        var el = $id('deliverToAddressId');
        if (int(el.value) > 0 && el.selectedIndex >= 0) {
            var o = el.options[el.selectedIndex];
            var partyId = int(o.getAttribute('data-party-id'));
            if (partyId > 0 && int(val('deliverToPartyId')) !== partyId) {
                shipToSync = true;
                $id('deliverToPartyId').value = String(partyId);
                fire($id('deliverToPartyId'));
                shipToSync = false;
            }
            if (!addressTag) setVal('deliverToAddress', o.textContent);   // Tag guard :1233
        }
    }
    /* CmbBuyerName_Leave :1036 - Un-Loading City from the buyer's city when none is chosen. */
    function onBuyerChange() {
        var b = $id('buyerId');
        if (int(val('unloadingCityId')) !== 0 || int(b.value) <= 0 || b.selectedIndex < 0) return;
        var city = (b.options[b.selectedIndex].getAttribute('data-city') || '').trim().toLowerCase();
        if (!city) return;
        var u = $id('unloadingCityId');
        for (var i = 0; i < u.options.length; i++) {
            if (u.options[i].textContent.trim().toLowerCase() === city) { setSel('unloadingCityId', u.options[i].value); break; }
        }
    }

    /* ------------------------------------------------ Ship To Address "+" (btnDefineShipToAddress_Click :3422)
       SupfrmShipToAddress - same BLL 0602 endpoints as the PO screen (org/company/user from the session). */
    var ST_API = '/api/commission/purchase-order/ship-to';
    var stLookups = false, stSaved = false;
    function openShipTo() {
        $id('shipToModal').style.display = 'flex';
        stSaved = false;
        stReset();
        var jobs = [stGrid()];
        if (!stLookups) {
            /* cmbsup: the same party list as Deliver To Party */
            var src = $id('deliverToPartyId'), h = '<option value="0"></option>';
            if (src) Array.prototype.forEach.call(src.options, function (o) { if (int(o.value) > 0) h += '<option value="' + esc(o.value) + '">' + esc(o.textContent) + '</option>'; });
            $id('stParty').innerHTML = h; fire($id('stParty'));
            jobs.push(getJSON(ST_API + '/countries').then(function (rows) {
                fillSelect('stCountry', rows, '', function (r) { return '<option value="' + esc(pick(r, 'Id')) + '">' + esc(pick(r, 'Description') || pick(r, 'CountryName')) + '</option>'; });
            }).catch(function () {}));
            jobs.push(getJSON(ST_API + '/cities').then(function (rows) {
                fillSelect('stCity', rows, '', function (r) { return '<option value="' + esc(pick(r, 'Id')) + '">' + esc(pick(r, 'CityName')) + '</option>'; });
            }).catch(function () {}));
        }
        Promise.all(jobs).then(function () {
            stLookups = true;
            var p = int(val('deliverToPartyId'));
            if (p > 0) { $id('stParty').value = String(p); fire($id('stParty')); }
        });
    }
    function closeShipTo() {
        $id('shipToModal').style.display = 'none';
        if (stSaved) bindShipTo(int(val('deliverToPartyId')), int(val('deliverToAddressId')));
    }
    function stReset() {
        setVal('stId', '0');
        ['stParty', 'stCountry', 'stCity'].forEach(function (id) { var e = $id(id); if (e) { e.value = '0'; fire(e); } });
        ['stTitle', 'stAddress', 'stContact', 'stPhone', 'stMobile', 'stWhatsApp'].forEach(function (id) { setVal(id, ''); });
        $id('btnStSave').disabled = false; $id('btnStUpdate').disabled = true;
    }
    function stGrid() {
        return getJSON(ST_API + '/history').then(function (rows) {
            var h = '';
            (rows || []).forEach(function (r) {
                h += '<tr data-st="' + esc(pick(r, 'Id')) + '" style="cursor:pointer"><td>' + esc(pick(r, 'PartyName')) + '</td><td>' + esc(pick(r, 'AddressTitle'))
                    + '</td><td>' + esc(pick(r, 'CountryName')) + '</td><td>' + esc(pick(r, 'CityName')) + '</td><td>' + esc(pick(r, 'ContactPerson'))
                    + '</td><td>' + esc(pick(r, 'PhoneNo')) + '</td><td>' + esc(pick(r, 'MobileNo')) + '</td><td>' + esc(pick(r, 'WhatsAppNo'))
                    + '</td><td>' + esc(dmy(pick(r, 'EntryDate'))) + '</td><td>' + esc(pick(r, 'EntryUser')) + '</td><td>' + esc(dmy(pick(r, 'ModifyDate')))
                    + '</td><td>' + esc(pick(r, 'ModifyUser')) + '</td><td>' + esc(pick(r, 'AddressLine1')) + '</td></tr>';
            });
            $id('grdShipToBody').innerHTML = h;
        }).catch(function () {});
    }
    function stEdit(id) {
        getJSON(ST_API + '/' + id).then(function (b) {
            setVal('stId', String(id));
            [['stParty', 'SupplierCustomerId'], ['stCountry', 'CountryId'], ['stCity', 'CityId']].forEach(function (p) {
                var e = $id(p[0]); e.value = String(int(pick(b, p[1]))); fire(e);
            });
            setVal('stAddress', pick(b, 'AddressLine1') || ''); setVal('stTitle', pick(b, 'AddressTitle') || '');
            setVal('stWhatsApp', pick(b, 'WhatsAppNo') || ''); setVal('stContact', pick(b, 'ContactPerson') || '');
            setVal('stMobile', pick(b, 'MobileNo') || ''); setVal('stPhone', pick(b, 'PhoneNo') || '');
            $id('btnStSave').disabled = true; $id('btnStUpdate').disabled = false;
        }).catch(function (e) { alert(e.message); });
    }
    function stSave() {
        /* FormValidation order as SupfrmShipToAddress */
        if (!int(val('stParty'))) { alert('Please Select Supplier'); return; }
        if (!String(val('stAddress')).trim()) { alert('Please Enter Address'); return; }
        if (!String(val('stTitle')).trim()) { alert('Please Enter Address Title'); return; }
        if (!int(val('stCountry'))) { alert('Please Select Country'); return; }
        if (!int(val('stCity'))) { alert('Please Select City'); return; }
        sendJSON(ST_API + '/save', 'POST', {
            Id: int(val('stId')), SupplierCustomerId: int(val('stParty')), CountryId: int(val('stCountry')), CityId: int(val('stCity')),
            AddressLine1: val('stAddress'), AddressTitle: val('stTitle'), PhoneNo: val('stPhone'), MobileNo: val('stMobile'),
            WhatsAppNo: val('stWhatsApp'), ContactPerson: val('stContact')
        }).then(function (d) {
            if (!d || !d.success) { alert((d && d.message) || 'Save refused.'); return; }
            alert(d.message); stSaved = true; stReset(); stGrid();
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ Load GRN dialog */
    function openLoader() {
        $id('grnLoaderModal').style.display = 'flex';
        /* InitializeComponentMethod :137-156 - buyer pre-set from the form, From = today - 7 */
        var t = new Date();
        if (!val('ldFromDate')) setVal('ldFromDate', ymdLocal(new Date(t.getFullYear(), t.getMonth(), t.getDate() - 7)));
        if (!val('ldToDate')) setVal('ldToDate', ymdLocal(t));
        loadLoaderCombos().then(function () {
            var buyer = int(val('buyerId'));
            if (buyer > 0) { $id('ldBuyerId').value = String(buyer); fire($id('ldBuyerId')); }
            loaderSearch();
        });
    }
    function closeLoader() { $id('grnLoaderModal').style.display = 'none'; }

    /* ComboDbCall / CombosFill :158-275 */
    function loadLoaderCombos() {
        return getJSON(API + '/pending-grn/combos').then(function (c) {
            c = c || {};
            fillSelect('ldCommissionAgentId', c.commissionAgents, '');
            fillSelect('ldSupplierId', c.suppliers, '');
            fillSelect('ldBuyerId', c.buyers, '');
            fillSelect('ldItemId', c.items, '');
            fillSelect('ldDeliverToPartyId', c.deliverToParties, '');
            /* CmbShipToAddress: the loader sends its TEXT as @ShipToAddress (:311) */
            fillSelect('ldShipToAddress', c.shipToAddresses, '', function (d) {
                return '<option value="' + esc(d.name) + '">' + esc(d.name) + '</option>';
            });
        }).catch(function () {});
    }

    /* PendingDataDbCall :290-321 */
    function loaderSearch() {
        var q = new URLSearchParams();
        if (val('ldFromDate')) q.set('fromDate', val('ldFromDate'));
        if (val('ldToDate')) q.set('toDate', val('ldToDate'));
        q.set('fromDocNo', int(val('ldDocNoFrom')));
        q.set('toDocNo', int(val('ldDocNoTo')));
        q.set('commissionAgentId', int(val('ldCommissionAgentId')));
        q.set('supplierId', int(val('ldSupplierId')));
        q.set('buyerId', int(val('ldBuyerId')));
        q.set('itemId', int(val('ldItemId')));
        q.set('deliverToPartyId', int(val('ldDeliverToPartyId')));
        var ship = val('ldShipToAddress');
        if (ship && ship !== '0') q.set('shipToAddress', ship);
        $id('ldMaster').querySelector('tbody').innerHTML = '<tr><td colspan="30" class="text-muted">Loading...</td></tr>';
        $id('ldDetail').querySelector('tbody').innerHTML = '';
        return getJSON(API + '/pending-grn?' + q.toString()).then(function (res) {
            if (!res || res.status !== 'SUCCESS') { alert(res && res.message ? res.message : 'Pending GRN could not be loaded.'); return; }
            loader.rows = res.rows || [];
            loader.emptyBags = res.emptyBags || [];
            loader.expenses = res.expenses || [];
            renderLoaderMaster();
        }).catch(function (e) { alert('Pending GRN could not be loaded: ' + e.message); });
    }

    /* GrdDataBind :322-391 (one row per GRN), grdSettings :393 hides the id columns. */
    var LD_MASTER_COLS = [
        ['docNo', 'DocNo'], ['docDate', 'DocDate', 'd'], ['CommissionAgentName', 'CommissionAgentName'],
        ['SupplierName', 'SupplierName'], ['LoadingCityName', 'LoadingCityName'], ['UnloadingCityName', 'UnloadingCityName'],
        ['supplierRefDocNo', 'SupplierRefDocNo'], ['DeliveryTerm', 'DeliveryTerm'], ['transporterName', 'TransporterName'],
        ['biltyFreight', 'BiltyFreight', 'n'], ['otherAdLesCharges', 'OtherAdLesCharges', 'n'], ['totalFreight', 'TotalFreight', 'n'],
        ['vehicleNo', 'VehicleNo'], ['biltyNo', 'BiltyNo'], ['biltyDate', 'BiltyDate', 'd'], ['biltyQty', 'BiltyQty', 'n'],
        ['loadWeight', 'LoadWeight', 'n'], ['tareWeight', 'TareWeight', 'n'], ['scaleNetWeight', 'ScaleNetWeight', 'n'],
        ['remarksHeader', 'RemarksHeader'], ['entryDate', 'EntryDate', 'd'], ['EntryUserName', 'EntryUserName'],
        ['modifyDate', 'ModifyDate', 'd'], ['ModifyUserName', 'ModifyUserName'], ['approvedDate', 'ApprovedDate', 'd'],
        ['isApproved', 'IsApproved', 'b'], ['ApprovalUserName', 'ApprovalUserName'], ['NoOfAttachments', 'NoOfAttachments', 'n']
    ];
    /* DetailGridBind :506-606 minus GridDetailSetting's hidden columns. */
    var LD_DETAIL_COLS = [
        ['SaleOrderNo', 'SaleOrderNo'], ['SaleOrderDate', 'SaleOrderDate', 'd'], ['SaleOrderExpiryDate', 'SaleOrderExpiryDate', 'd'],
        ['BuyerName', 'BuyerName'], ['DeliverToPartyName', 'DeliverToPartyName'], ['DeliverToAddress', 'DeliverToAddress'],
        ['inventoryParentCategory', 'InventoryParentCategory'], ['PurchaseOrderNo', 'PurchaseOrderNo'],
        ['PurchaseOrderDate', 'PurchaseOrderDate', 'd'], ['PurchaseOrderExpiryDate', 'PurchaseOrderExpiryDate', 'd'],
        ['ItemName', 'ItemName'], ['ItemCode', 'ItemCode'], ['PackUomCode', 'PackUom'], ['cropYear', 'CropYear'],
        ['PackingType', 'PackingType'], ['loadingQty', 'LoadingQty', 'n'], ['wbGrossWeight', 'GrossWeight', 'n'],
        ['ebwPerUnit', 'EbUnit', 'n'], ['ebwTotal', 'EbTotal', 'n'], ['addLessWeight', 'AddLessWeight', 'n'],
        ['netBillWeight', 'NetBillWeight', 'n'], ['UsedQty', 'UsedQty', 'n'], ['BalanceQty', 'BalanceQty', 'n'],
        ['UsedWeight', 'UsedWeight', 'n'], ['BalanceWeight', 'BalanceWeight', 'n'], ['RemarksDetail', 'RemarksDetail'],
        ['warningRemarks', 'WarningRemarks']
    ];
    function cell(r, c) {
        var v = pick(r, c[0]);
        if (c[2] === 'd') return esc(dmy(v));
        if (c[2] === 'n') return esc(fmt(v));
        if (c[2] === 'b') return (v === true || v === 1 || String(v).toLowerCase() === 'true') ? '&#10004;' : '';
        return esc(v);
    }
    function tdClass(c) { return c[2] === 'n' ? ' class="num"' : ''; }
    function renderLoaderMaster() {
        $id('ldMaster').querySelector('thead').innerHTML = '<tr><th></th>' + LD_MASTER_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        var seen = {}, h = '';
        loader.rows.forEach(function (r) {
            var id = int(pick(r, 'grnSupplierLoadingMasterId'));
            if (seen[id]) return;
            seen[id] = true;
            h += '<tr data-grn="' + id + '"><td><button type="button" class="btn btn-sm btn-primary py-0 ld-load" data-grn="' + id + '">Load This</button></td>'
                + LD_MASTER_COLS.map(function (c) { return '<td' + tdClass(c) + '>' + cell(r, c) + '</td>'; }).join('') + '</tr>';
        });
        $id('ldMaster').querySelector('tbody').innerHTML = h || '<tr><td colspan="30" class="text-muted">No pending GRN found.</td></tr>';
        setText('ldMasterCount', 'Records: ' + Object.keys(seen).length);
        setText('ldDetailCount', '');
        var first = $id('ldMaster').querySelector('tbody tr[data-grn]');
        if (first) selectLoaderRow(int(first.getAttribute('data-grn')));
    }
    /* grd_SelectionChanged :485 -> DetailGridBind */
    function selectLoaderRow(grnId) {
        loader.selectedGrnId = grnId;
        Array.prototype.forEach.call($id('ldMaster').querySelectorAll('tbody tr'), function (tr) {
            tr.classList.toggle('ld-selected', int(tr.getAttribute('data-grn')) === grnId);
        });
        $id('ldDetail').querySelector('thead').innerHTML = '<tr>' + LD_DETAIL_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        var h = '';
        loader.rows.forEach(function (r) {
            if (int(pick(r, 'grnSupplierLoadingMasterId')) !== grnId) return;
            h += '<tr>' + LD_DETAIL_COLS.map(function (c) { return '<td' + tdClass(c) + '>' + cell(r, c) + '</td>'; }).join('') + '</tr>';
        });
        $id('ldDetail').querySelector('tbody').innerHTML = h;
        setText('ldDetailCount', 'Detail Records: ' + $id('ldDetail').querySelectorAll('tbody tr').length);
    }
    function setText(id, t) { var e = $id(id); if (e) e.textContent = t; }
    /* grd_ColumnButtonClick :452-483 - every pending row of that GRN, then Hide(). */
    function loaderLoad(grnId) {
        if (grnId <= 0) { alert('No valid row selected.'); return; }
        var rows = loader.rows.filter(function (r) { return int(pick(r, 'grnSupplierLoadingMasterId')) === grnId; });
        if (!rows.length) { alert('No matching records found.'); return; }
        closeLoader();
        loadedDataSet = { emptyBags: loader.emptyBags, expenses: loader.expenses };
        loadInGridDetail(rows);
    }

    /* ------------------------------------------------------------------ LoadInGridDetail :3465 */
    function loadInGridDetail(dt) {
        try {
            if (!dt.length) return;
            var first = dt[0];
            if (detailRows.length) {
                var existing = detailRows.find(function (r) { return int(r.GrnId) > 0; });
                if (existing && int(existing.GrnId) !== int(pick(first, 'grnSupplierLoadingMasterId'))) {
                    throw new Error('Already loaded rows belong to a different Grn. You cannot load rows from another Grn.');
                }
                if (int(val('buyerId')) > 0 && int(pick(first, 'buyerId')) !== int(val('buyerId'))) {
                    throw new Error('Already loaded rows belong to a different buyer. So you cannot load rows from another buyer.');
                }
            }
            var warn = [];
            dt.forEach(function (r) { var w = pick(r, 'warningRemarks'); w = w == null ? '' : String(w); if (warn.indexOf(w) < 0) warn.push(w); });
            setVal('remarksHeader', warn.join(', '));                          // txtRemarks
            setSel('buyerId', pick(first, 'buyerId'));
            $id('buyerId').disabled = true;
            onBuyerChange();
            var partyId = int(pick(first, 'DeliverToPartyId'));
            shipToSync = true;
            setSel('deliverToPartyId', partyId);
            var shipId = int(pick(first, 'DeliverToAddressId')) === 0 ? int(pick(first, 'shipToAddressId')) : int(pick(first, 'DeliverToAddressId'));
            bindShipTo(partyId, shipId).then(function () { shipToSync = false; if (shipId > 0) onShipToChange(); });
            setGrnCombo(int(pick(first, 'grnSupplierLoadingMasterId')), pick(first, 'docNo'));
            setVal('grnDate', ymd(pick(first, 'docDate')));
            var tId = int(pick(first, 'transporterId'));
            if (tId > 0) setSel('transporterId', tId);
            setVal('transporterName', pick(first, 'transporterName') || '');
            setSel('loadingCityId', pick(first, 'loadingCityId'));
            setSel('unloadingCityId', pick(first, 'unloadingCityId'));
            setSel('vehicleTypeId', pick(first, 'vehicleTypeId'));
            setVal('vehicleNo', pick(first, 'vehicleNo') || '');
            setVal('biltyNo', pick(first, 'biltyNo') || '');
            setVal('biltyDate', ymd(pick(first, 'biltyDate')));
            setSel('deliveryTermId', pick(first, 'deliveryTermId'));
            bindFromGrn(dt);
            var ids = {};
            detailRows.forEach(function (r) { ids[int(r.GrnDetailId)] = true; });
            dt.forEach(function (dr) {
                var detailId = int(pick(dr, 'DetailId'));
                if (ids[detailId]) return;
                ids[detailId] = true;
                var itemId = int(pick(dr, 'itemId'));
                detailRows.push({
                    Id: 0,
                    GrnId: int(pick(dr, 'grnSupplierLoadingMasterId')),
                    GrnDetailId: detailId,
                    GrnNo: pick(dr, 'docNo'),
                    GrnDate: ymd(pick(dr, 'docDate')),
                    ParentItemId: int(pick(dr, 'inventoryParentCategoryId')),
                    ParentItem: pick(dr, 'inventoryParentCategory'),
                    ItemId: itemId,
                    ItemName: itemId > 0 ? pick(dr, 'ItemName') : '',
                    ItemCode: itemId > 0 ? pick(dr, 'ItemCode') : '',
                    CropYearId: int(pick(dr, 'cropYearId')),
                    CropYear: pick(dr, 'cropYear'),
                    PackingTypeId: int(pick(dr, 'packingTypeId')),
                    PackingType: pick(dr, 'PackingType'),
                    PackUomId: int(pick(dr, 'packUomId')),
                    PackUom: pick(dr, 'PackUomCode'),
                    PackUomEquivalent: num(pick(dr, 'PackUomEquivalent')),
                    Qty: num(pick(dr, 'BalanceQty')),
                    GrossWeight: num(pick(dr, 'BalanceGrossWeight')),
                    EbUnit: num(pick(dr, 'ebwPerUnit')),
                    EbTotal: num(pick(dr, 'ebwTotal')),
                    AddLessWeight: num(pick(dr, 'addLessWeight')),
                    NetBillWeight: num(pick(dr, 'BalanceWeight')),
                    EmptyBagTermId: int(pick(dr, 'EBWeightDeductionTermId')),
                    EmptyBagTerm: pick(dr, 'EmptyBagDeductionTerm'),
                    PurchaseOrderId: int(pick(dr, 'purchaseOrderMasterId')),
                    PurchaseOrderDetailId: int(pick(dr, 'purchaseOrderDetailId')),
                    PurchaseOrderNo: pick(dr, 'PurchaseOrderNo'),
                    /* The desktop leaves POExpiryDate / SOExpiryDate empty here; the loader row
                       carries them and the late-vehicle rule (:1954) needs the PO one. */
                    POExpiryDate: pick(dr, 'PurchaseOrderExpiryDate'),
                    SaleOrderId: int(pick(dr, 'saleOrderMasterId')),
                    SaleOrderDetailId: int(pick(dr, 'saleOrderDetailId')),
                    SaleOrderNo: pick(dr, 'SaleOrderNo'),
                    SOExpiryDate: pick(dr, 'SaleOrderExpiryDate'),
                    SupplierId: int(pick(dr, 'supplierId')),
                    SupplierName: pick(dr, 'SupplierName'),
                    Remarks: '',
                    WarningRemarks: ''
                });
            });
            calculateEbTotalThenBillWeight();
            getExpensesOnBaseOfGrnDetailSO();
            getEmptyBagDetailOnBaseOfGrnDetailSO();
            renderDetail();
            renderEmptyBags();
            renderExpenses();
        } catch (e) {
            shipToSync = false;
            alert(e.message);
        }
    }

    /* BindFromGrn :3565-3582 - the GRN column of the comparison panel. */
    function bindFromGrn(dt) {
        var f = dt[0];
        var te = $id('deliveryTermGrnId');
        if (te) { te.value = String(int(pick(f, 'deliveryTermId'))); fire(te); }
        setVal('biltyQtyGrn', fmt(pick(f, 'biltyQty')));
        setVal('loadWeightGrn', fmt(pick(f, 'loadWeight')));
        setVal('tareWeightGrn', fmt(pick(f, 'tareWeight')));
        setVal('scaleNetWeightGrn', fmt(pick(f, 'scaleNetWeight')));
        var bal = 0; dt.forEach(function (r) { bal += num(pick(r, 'BalanceWeight')); });
        setVal('billWeightGrn', fmt(bal));
        setVal('biltyFreightGrn', fmt(pick(f, 'biltyFreight')));
        setVal('otherAdLesChargesGrn', fmt(pick(f, 'otherAdLesCharges')));
        setVal('totalFreightGrn', fmt(pick(f, 'totalFreight')));
        refreshDiffs();
    }

    /* CalculateEbTotalThenBillWeight :3597 */
    function calculateEbTotalThenBillWeight() {
        detailRows.forEach(function (r) {
            r.EbTotal = num(r.Qty) * num(r.EbUnit);
            r.NetBillWeight = num(r.GrossWeight) - r.EbTotal + num(r.AddLessWeight);
        });
    }
    function saleOrderIdSet() {
        var s = {};
        detailRows.forEach(function (r) { s[int(r.SaleOrderId)] = true; });
        return s;
    }
    /* GetEmptyBagDetailOnBaseOfGrnDetailSO :882-924 */
    function getEmptyBagDetailOnBaseOfGrnDetailSO() {
        var eb = loadedDataSet ? loadedDataSet.emptyBags : null;
        if (!eb || !eb.length) return;
        var so = saleOrderIdSet(), keys = {};
        emptyBagRows.forEach(function (r) { keys[int(r.SoId) + ':' + int(r.SoEbId)] = true; });
        var match = eb.filter(function (r) { return so[int(pick(r, 'saleOrderMasterId'))]; });
        if (match.length && !emptyBagRows.some(function (r) { return num(r.PurchaseRate) > 0; })) { emptyBagRows = []; keys = {}; }
        match.forEach(function (row) {
            var soId = int(pick(row, 'saleOrderMasterId')), soEbId = int(pick(row, 'saleOrderEmptyBagDetailId'));
            if (keys[soId + ':' + soEbId]) return;
            emptyBagRows.push({ Id: 0, SoId: soId, SoEbId: soEbId, SaleOrderNo: pick(row, 'SaleOrderNo'),
                PackingType: int(pick(row, 'PackingTypeId')), PurchaseRate: num(pick(row, 'Rate')),
                EmptyBagItem: int(pick(row, 'emptyBagPackingMaterialItemId')) });
            keys[soId + ':' + soEbId] = true;
        });
        if (!emptyBagRows.length) addEmptyBagRow();
    }
    /* GetExpensesOnBaseOfGrnDetailSO :926-969 */
    function getExpensesOnBaseOfGrnDetailSO() {
        var ex = loadedDataSet ? loadedDataSet.expenses : null;
        if (!ex) return;
        var so = saleOrderIdSet(), keys = {};
        expenseRows.forEach(function (r) { keys[int(r.SoId) + ':' + int(r.SoExpenseId)] = true; });
        var match = ex.filter(function (r) { return so[int(pick(r, 'saleOrderMasterId'))]; });
        if (match.length && !expenseRows.some(function (r) { return num(r.Amount) > 0; })) { expenseRows = []; keys = {}; }
        match.forEach(function (row) {
            var soId = int(pick(row, 'saleOrderMasterId')), soExId = int(pick(row, 'saleOrderbuyerExpenseDetailId'));
            if (keys[soId + ':' + soExId]) return;
            keys[soId + ':' + soExId] = true;
            expenseRows.push({ Id: 0, SoId: soId, SoExpenseId: soExId, SaleOrderNo: pick(row, 'SaleOrderNo'),
                ItemId: int(pick(row, 'ItemId')), Qty: num(pick(row, 'Qty')), Rate: num(pick(row, 'rate')),
                Amount: num(pick(row, 'amount')), Remarks: pick(row, 'remarks') || '' });
        });
        if (!expenseRows.length) addExpenseRow();
    }

    /* ------------------------------------------------------------------ detail grid */
    /* InitializeDetailtable minus DetailGridCommonSetting's hidden columns; editable = EditColumnsList :1321.
       [key, caption, type, editable] */
    var DETAIL_COLS = [
        ['GrnNo', 'Grn No'], ['ParentItem', 'Parent Item'], ['ItemCode', 'Item Code'], ['ItemName', 'Item Name'],
        ['CropYear', 'Crop Year'], ['PackingType', 'Packing Type'], ['PackUom', 'Pack Uom'],
        ['Qty', 'Qty', 'n', true], ['GrossWeight', 'Gross Weight', 'n', true], ['EbUnit', 'Eb Unit', 'n', true],
        ['EbTotal', 'Eb Total', 'n', true], ['AddLessWeight', 'Add Less Weight', 'n', true],
        ['NetBillWeight', 'Net Bill Weight', 'n'], ['EmptyBagTerm', 'Empty Bag Term'],
        ['PurchaseOrderNo', 'Purchase Order No', 'po'], ['POExpiryDate', 'PO Expiry Date', 'dt'],
        ['SaleOrderNo', 'Sale Order No', 'po'], ['SOExpiryDate', 'SO Expiry Date', 'dt'],
        ['SupplierName', 'Supplier Name'], ['Remarks', 'Remarks', 't', true], ['WarningRemarks', 'Warning Remarks']
    ];
    function detailCols() {
        /* grdDetailSetting :1353-1385 - PO / SO No visible only when a row carries a PO */
        var showPo = detailRows.some(function (r) { return int(r.PurchaseOrderId) > 0; });
        return DETAIL_COLS.filter(function (c) { return c[2] !== 'po' || showPo; });
    }
    function renderDetail() {
        var cols = detailCols();
        var tbl = $id('gridItems');
        tbl.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        if (!detailRows.length) {
            tbl.querySelector('tbody').innerHTML = '<tr><td colspan="' + cols.length + '" class="text-center text-muted">Use Load GRN to bring in the pending GRN rows.</td></tr>';
            tbl.querySelector('tfoot').innerHTML = '<tr><td colspan="' + cols.length + '">&nbsp;</td></tr>';
            gridNav('gridItems', 0);
            return;
        }
        var h = '';
        detailRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                var v = r[c[0]];
                if (c[3]) {
                    if (c[2] === 't') return '<td><input type="text" class="gcell" data-k="' + c[0] + '" value="' + esc(v) + '"></td>';
                    return '<td><input type="number" step="any" class="gcell num" data-k="' + c[0] + '" value="' + esc(r3(num(v))) + '"></td>';
                }
                if (c[2] === 'n') return '<td class="num" data-k="' + c[0] + '">' + esc(fmt(v)) + '</td>';
                if (c[2] === 'dt') return '<td>' + esc(dmy(v)) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        });
        tbl.querySelector('tbody').innerHTML = h;
        renderDetailTotals();
        gridNav('gridItems', detailRows.length);
    }
    /* ctrlGrdBar2 totals */
    function renderDetailTotals() {
        var cols = detailCols();
        var sums = { Qty: 0, GrossWeight: 0, EbTotal: 0, AddLessWeight: 0, NetBillWeight: 0 };
        detailRows.forEach(function (r) { for (var k in sums) sums[k] += num(r[k]); });
        $id('gridItems').querySelector('tfoot').innerHTML = '<tr>' + cols.map(function (c) {
            return '<td class="num">' + (sums[c[0]] !== undefined ? esc(fmt(sums[c[0]])) : '') + '</td>';
        }).join('') + '</tr>';
    }
    /* grdDetail_CellUpdated :1442-1500 */
    function onDetailEdit(e) {
        var inp = e.target;
        if (!inp.classList || !inp.classList.contains('gcell')) return;
        var tr = inp.closest('tr');
        var r = detailRows[int(tr.getAttribute('data-i'))];
        if (!r) return;
        var k = inp.getAttribute('data-k');
        if (k === 'Remarks') { r.Remarks = inp.value; return; }
        r[k] = num(inp.value);
        var qty = num(r.Qty), eq = num(r.PackUomEquivalent), gross = num(r.GrossWeight);
        var ebUnit = num(r.EbUnit), ebTotal = num(r.EbTotal), addLess = num(r.AddLessWeight);
        if (k === 'EbUnit') {
            ebTotal = r3(ebUnit > 0 ? ebUnit * qty : 0); r.EbTotal = ebTotal; calcByEbUnit = true; calcByEbTotal = false;
        } else if (k === 'EbTotal') {
            ebUnit = r3(ebTotal > 0 && qty !== 0 ? ebTotal / qty : 0); r.EbUnit = ebUnit; calcByEbUnit = false; calcByEbTotal = true;
        } else if (calcByEbTotal) {
            ebUnit = r3(ebTotal > 0 && qty !== 0 ? ebTotal / qty : 0); r.EbUnit = ebUnit;
        } else if (calcByEbUnit) {
            ebTotal = r3(ebUnit > 0 ? ebUnit * qty : 0); r.EbTotal = ebTotal;
        }
        if (recId === 0 && k === 'Qty') { gross = qty * eq; r.GrossWeight = gross; }
        r.NetBillWeight = gross - ebTotal + addLess;
        ['EbUnit', 'EbTotal', 'GrossWeight'].forEach(function (c) {
            if (c === k) return;
            var o = tr.querySelector('input[data-k="' + c + '"]');
            if (o) o.value = r3(num(r[c]));
        });
        var n = tr.querySelector('td[data-k="NetBillWeight"]');
        if (n) n.textContent = fmt(r.NetBillWeight);
        renderDetailTotals();
    }

    /* ------------------------------------------------------------------ empty bags grid */
    function addEmptyBagRow() { emptyBagRows.push({ Id: 0, SoId: 0, SoEbId: 0, SaleOrderNo: 0, PackingType: 0, PurchaseRate: 0, EmptyBagItem: 0 }); }
    function optList(list, idKey, nameKey, selected) {
        var h = '<option value="0"></option>';
        (list || []).forEach(function (d) {
            var id = int(pick(d, idKey));
            h += '<option value="' + id + '"' + (id === int(selected) ? ' selected' : '') + '>' + esc(pick(d, nameKey)) + '</option>';
        });
        return h;
    }
    /* InitializeEmptyBagDetailtable / EmptyBagGridCommonSetting / gridEmptyBagsSettings :1693 */
    function renderEmptyBags() {
        var tbl = $id('gridEmptyBags'); if (!tbl) return;
        var showSo = emptyBagRows.some(function (r) { return int(r.SoId) > 0; });
        tbl.querySelector('thead').innerHTML = '<tr><th>X</th><th>+</th>' + (showSo ? '<th>Sale Order No</th>' : '')
            + '<th>Packing Type</th><th>Purchase Rate</th><th>Empty Bag Item</th></tr>';
        var h = '';
        emptyBagRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"><td><button type="button" class="btn btn-sm btn-outline-danger py-0 eb-del">X</button></td>'
                + '<td><button type="button" class="btn btn-sm btn-outline-primary py-0 eb-add">+</button></td>'
                + (showSo ? '<td>' + esc(int(r.SaleOrderNo) || '') + '</td>' : '')
                + '<td><select class="ecell" data-k="PackingType" data-dtcombo="single" data-dtcombo-caption="Packing Type">' + optList(packingTypes, 'Id', 'ReferenceName', r.PackingType) + '</select></td>'
                + '<td><input type="number" step="any" class="ecell num" data-k="PurchaseRate" value="' + esc(num(r.PurchaseRate).toFixed(2)) + '"></td>'
                + '<td><select class="ecell" data-k="EmptyBagItem" data-dtcombo="single" data-dtcombo-caption="Empty Bag Item">' + optList(emptyBagItems, 'ItemId', 'ItemName', r.EmptyBagItem) + '</select></td></tr>';
        });
        tbl.querySelector('tbody').innerHTML = h;
        var tf = tbl.querySelector('tfoot') || tbl.appendChild(document.createElement('tfoot'));
        tf.innerHTML = '<tr><td colspan="' + (showSo ? 6 : 5) + '">&nbsp;</td></tr>';
        gridNav('gridEmptyBags', emptyBagRows.length);
    }
    /* ctrlGrdBar record navigator: "Record: |< < [n] Of n > >|" */
    var navPos = {};
    function gridNav(tblId, count) {
        var tbl = $id(tblId); if (!tbl) return;
        var wrap = tbl.parentNode, bar = wrap.nextElementSibling;
        if (!bar || !bar.classList.contains('g1055-nav')) {
            bar = document.createElement('div'); bar.className = 'g1055-nav'; bar.setAttribute('data-for', tblId);
            wrap.parentNode.insertBefore(bar, wrap.nextSibling);
            bar.addEventListener('click', function (e) {
                var b = e.target.closest ? e.target.closest('button[data-m]') : null; if (!b) return;
                var n = tbl.querySelectorAll('tbody tr[data-i]').length; if (!n) return;
                var p = navPos[tblId] || 0, m = b.getAttribute('data-m');
                p = m === 'f' ? 0 : m === 'p' ? Math.max(0, p - 1) : m === 'n' ? Math.min(n - 1, p + 1) : n - 1;
                navPos[tblId] = p; gridNav(tblId, n);
            });
        }
        var pos = Math.min(navPos[tblId] || 0, Math.max(0, count - 1)); navPos[tblId] = pos;
        tbl.querySelectorAll('tbody tr[data-i]').forEach(function (tr, i) { tr.classList.toggle('g1055-cur', i === pos); });
        var cur = tbl.querySelector('tbody tr.g1055-cur'); if (cur && cur.scrollIntoView && count > 1 && pos > 0) cur.scrollIntoView({ block: 'nearest' });
        bar.innerHTML = 'Record: <button type="button" data-m="f">|&lt;</button><button type="button" data-m="p">&lt;</button>'
            + '<span class="g1055-navbox">' + (count ? pos + 1 : 0) + '</span> Of ' + count
            + ' <button type="button" data-m="n">&gt;</button><button type="button" data-m="l">&gt;|</button>';
    }
    function onEmptyBagEvent(e) {
        var tr = e.target.closest ? e.target.closest('tr[data-i]') : null; if (!tr) return;
        var i = int(tr.getAttribute('data-i'));
        if (e.type === 'click') {
            if (e.target.classList.contains('eb-add')) { addEmptyBagRow(); renderEmptyBags(); }
            else if (e.target.classList.contains('eb-del')) {       // DeleteRowInEmptyBagGrid :1734
                emptyBagRows.splice(i, 1);
                if (!emptyBagRows.length) addEmptyBagRow();
                renderEmptyBags();
            }
            return;
        }
        var el = e.target; if (!el.classList.contains('ecell')) return;
        var r = emptyBagRows[i]; if (!r) return;
        var k = el.getAttribute('data-k');
        r[k] = k === 'PurchaseRate' ? num(el.value) : int(el.value);
    }

    /* ------------------------------------------------------------------ expense grid */
    function addExpenseRow() { expenseRows.push({ Id: 0, SoId: 0, SoExpenseId: 0, SaleOrderNo: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); }
    /* InitializeExpenseDetailtable / ExpenseGridCommonSetting / grdInvExpSettings :1544 */
    function renderExpenses() {
        var tbl = $id('gridExpenses'); if (!tbl) return;
        var showSo = expenseRows.some(function (r) { return int(r.SoId) > 0; });
        tbl.querySelector('thead').innerHTML = '<tr><th>X</th><th>+</th>' + (showSo ? '<th>Sale Order No</th>' : '')
            + '<th>Other Item Name</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        var h = '';
        expenseRows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"><td><button type="button" class="btn btn-sm btn-outline-danger py-0 ex-del">X</button></td>'
                + '<td><button type="button" class="btn btn-sm btn-outline-primary py-0 ex-add">+</button></td>'
                + (showSo ? '<td>' + esc(int(r.SaleOrderNo) || '') + '</td>' : '')
                + '<td><select class="xcell" data-k="ItemId" data-dtcombo="single" data-dtcombo-caption="Other Item Name">' + optList(otherItems, 'id', 'name', r.ItemId) + '</select></td>'
                + '<td><input type="number" step="any" class="xcell num" data-k="Qty" value="' + esc(num(r.Qty)) + '"></td>'
                + '<td><input type="number" step="any" class="xcell num" data-k="Rate" value="' + esc(num(r.Rate).toFixed(2)) + '"></td>'
                + '<td><input type="number" step="any" class="xcell num" data-k="Amount" value="' + esc(num(r.Amount)) + '"></td>'
                + '<td><input type="text" class="xcell" data-k="Remarks" value="' + esc(r.Remarks) + '"></td></tr>';
        });
        tbl.querySelector('tbody').innerHTML = h;
        renderExpenseTotals();
        gridNav('gridExpenses', expenseRows.length);
    }
    function renderExpenseTotals() {
        var tbl = $id('gridExpenses'); if (!tbl) return;
        var showSo = expenseRows.some(function (r) { return int(r.SoId) > 0; });
        var q = 0, a = 0; expenseRows.forEach(function (r) { q += num(r.Qty); a += num(r.Amount); });
        var tf = tbl.querySelector('tfoot') || tbl.appendChild(document.createElement('tfoot'));
        tf.innerHTML = '<tr><td></td><td></td>' + (showSo ? '<td></td>' : '') + '<td></td><td class="num">' + esc(fmt(q)) + '</td><td></td><td class="num">' + esc(fmt(a)) + '</td><td></td></tr>';
    }
    function onExpenseEvent(e) {
        var tr = e.target.closest ? e.target.closest('tr[data-i]') : null; if (!tr) return;
        var i = int(tr.getAttribute('data-i'));
        if (e.type === 'click') {
            if (e.target.classList.contains('ex-add')) { addExpenseRow(); renderExpenses(); }
            else if (e.target.classList.contains('ex-del')) {       // DeleteRowInExpenseGrid :1515
                expenseRows.splice(i, 1);
                if (!expenseRows.length) addExpenseRow();
                renderExpenses();
            }
            return;
        }
        var el = e.target; if (!el.classList.contains('xcell')) return;
        var r = expenseRows[i]; if (!r) return;
        var k = el.getAttribute('data-k');
        if (k === 'Remarks') { r.Remarks = el.value; return; }
        if (k === 'ItemId') { r.ItemId = int(el.value); return; }
        r[k] = num(el.value);
        if (k === 'Qty' || k === 'Rate') {                         // UpdateAmount :1648
            r.Amount = num(r.Qty) * num(r.Rate);
            tr.querySelector('input[data-k="Amount"]').value = r.Amount;
        } else if (k === 'Amount') {                               // UpdateRate :1655
            r.Rate = num(r.Qty) === 0 ? 0 : num(r.Amount) / num(r.Qty);
            tr.querySelector('input[data-k="Rate"]').value = num(r.Rate).toFixed(2);
        }
        renderExpenseTotals();
    }

    /* ------------------------------------------------------------------ header calculations */
    var PAIRS = [
        ['biltyQtyGrn', 'biltyQty', 'biltyQtyDiff'], ['loadWeightGrn', 'loadWeight', 'loadWeightDiff'],
        ['tareWeightGrn', 'tareWeight', 'tareWeightDiff'], ['scaleNetWeightGrn', 'scaleNetWeight', 'scaleNetWeightDiff'],
        ['avgFillGrn', 'avgFillGdn', 'avgFillDiff'], ['billWeightGrn', 'billWeight', 'billWeightDiff'],
        ['biltyFreightGrn', 'biltyFreight', 'biltyFreightDiff'], ['otherAdLesChargesGrn', 'otherAdLesCharges', 'otherAdLesChargesDiff'],
        ['totalFreightGrn', 'totalFreight', 'totalFreightDiff']
    ];
    /* FormHelper.SetDifference = GRN - GDN; average filling per bag = WB net / vehicle qty (:2397-2410) */
    function refreshDiffs() {
        var q = num(val('biltyQtyGrn'));
        setVal('avgFillGrn', fmt(q > 0 ? num(val('scaleNetWeightGrn')) / q : 0));
        var g = num(val('biltyQty'));
        setVal('avgFillGdn', fmt(g > 0 ? num(val('scaleNetWeight')) / g : 0));
        PAIRS.forEach(function (p) { setVal(p[2], fmt(num(val(p[0])) - num(val(p[1])))); });
    }
    /* CalculateGdnGrossWeight :2439 - WB net = |load - tare| */
    function calcGdnNet() {
        setVal('scaleNetWeight', r3(Math.abs(num(val('loadWeight')) - num(val('tareWeight')))));
        netChanged();
    }
    /* txtWbNetWeight_TextChanged :2466-2494 - a vehicle qty of 0 is derived from the average pack equivalent. */
    function netChanged() {
        if (num(val('biltyQty')) === 0 && detailRows.length) {
            var sum = 0; detailRows.forEach(function (r) { sum += num(r.PackUomEquivalent); });
            var avg = sum / detailRows.length;
            setVal('biltyQty', r3(avg > 0 ? num(val('scaleNetWeight')) / avg : 0));
        }
        refreshDiffs();
    }
    var freightBase = 1;   // BaseFreightForCalculation
    function calcFreight() {  // CalculateTotalFreight :2330-2359
        var bilty = num(val('biltyFreight')), other = num(val('otherAdLesCharges'));
        if (freightBase === 2) setVal('biltyFreight', r3(num(val('totalFreight')) - other));
        else setVal('totalFreight', r3(bilty + other));
        refreshDiffs();
    }

    /* ------------------------------------------------------------------ save - Insert() :1812-2070 */
    function askRemarks(msg) {
        var r = window.prompt(msg + '\n\nRemarks:');
        return r && r.trim() ? r.trim() : null;
    }
    function save(isUpdate) {
        if (inFlight.btnSave || inFlight.btnUpdate) return;
        if (isUpdate && recId <= 0) { alert('Record not update because Id not found'); return; }   // :2089
        if (!detailRows.length) { alert('Detail Record Not Found'); return; }
        /* :1828-1850 GRN / GDN date rules (FormHelper.AskForRemarks) */
        var gdn = dayNum(val('docDate')), grn = dayNum(val('grnDate'));
        if (gdn !== null && grn !== null) {
            if (gdn < grn) { alert('Invalid Entry!\n\nGDN Date cannot be less than GRN Date.'); return; }
            if (!val('warningRemarks').trim() && (gdn === grn || gdn > grn + 1)) {
                var rm = askRemarks(gdn === grn ? 'GRN Date and GDN Date are the same.\n\nPlease provide remarks to proceed.'
                                                : 'GDN Date is more than 1 day after GRN Date.\n\nPlease provide remarks to proceed.');
                if (!rm) return;
                setVal('warningRemarks', rm);
                showWarn();
            }
        }
        if (num(val('totalFreight')) > 0 && !int(val('transporterId')) && !val('transporterName').trim()) {
            alert('Please Select Transporter or Fill Transporter Name'); return;
        }
        /* :1954-1990 late vehicle against the PO validity date (frmRemarks "Late Vehicle Remarks") */
        for (var i = 0; i < detailRows.length; i++) {
            var r = detailRows[i];
            if (int(r.PurchaseOrderId) <= 0 || gdn === null) continue;
            var exp = dayNum(r.POExpiryDate);
            if (exp === null) continue;
            if (gdn === exp) { r.WarningRemarks = ''; }
            else if (gdn > exp) {
                if (lateCfg.blockEntry) { alert('Vehicle has reached late compared to PO Expiry Date.\n\nEntry is not allowed.'); return; }
                if (lateCfg.showWarning && !String(r.WarningRemarks || '').trim()) {
                    alert('Vehicle has reached late compared to PO Expiry Date.\n\nRemarks are required to proceed.');
                    var lr = askRemarks('Late Vehicle Remarks');
                    if (!lr) { alert('Remarks are required for late vehicle entry.'); return; }
                    r.WarningRemarks = lr;
                }
            }
        }
        var req = [['commissionAgentId', 'Commission Agent'], ['buyerId', 'Buyer Name'], ['grnNoId', 'Grn No'],
                   ['loadingCityId', 'Loading City'], ['unloadingCityId', 'Un-Loading City'], ['vehicleTypeId', 'Vehicle Type']];
        for (var q = 0; q < req.length; q++) if (!selInt(req[q][0])) { alert(req[q][1] + ' field is required'); return; }
        if (!val('vehicleNo').trim()) { alert('Vehicle No field is required'); return; }
        if (!val('biltyNo').trim()) { alert('Bilty No field is required'); return; }
        if (!selInt('deliveryTermId')) { alert('Delivery Term field is required'); return; }
        if (!num(val('biltyQty'))) { alert('Vehicle Qty must be a non-zero number'); return; }
        if (!num(val('scaleNetWeight'))) { alert('Net Wb Weight must be a non-zero number'); return; }
        if (!confirm(isUpdate ? 'Are you sure to Update' : 'Are you sure to Save')) return;

        var payload = {
            gdnBuyerDispatchMasterId: recId,
            docNo: int(val('docNo')),
            docDate: val('docDate'),
            buyerId: selInt('buyerId'),
            commissionAgentId: selInt('commissionAgentId'),
            vehicleNo: val('vehicleNo'),
            biltyNo: val('biltyNo'),
            remarksHeader: val('remarksHeader'),
            buyerRefDocNo: val('buyerRefDocNo'),
            deliverToPartyId: selInt('deliverToPartyId'),
            deliverToPartyName: selText('deliverToPartyId'),
            deliverToAddressId: selInt('deliverToAddressId'),
            deliverToAddress: val('deliverToAddress'),
            loadingCityId: selInt('loadingCityId'),
            unloadingCityId: selInt('unloadingCityId'),
            transporterId: selInt('transporterId'),
            transporterName: val('transporterName'),
            biltyFreight: num(val('biltyFreight')),
            otherAdLesCharges: num(val('otherAdLesCharges')),
            totalFreight: num(val('totalFreight')),
            freightRemarks: val('freightRemarks'),
            vehicleTypeId: selInt('vehicleTypeId'),
            biltyDate: val('biltyDate'),
            deliveryTermId: selInt('deliveryTermId'),
            biltyQty: num(val('biltyQty')),
            loadWeight: num(val('loadWeight')),
            tareWeight: num(val('tareWeight')),
            scaleNetWeight: num(val('scaleNetWeight')),
            billWeight: num(val('billWeight')),
            warningRemarks: val('warningRemarks'),
            grnDate: val('grnDate'),
            /* FillDetailListCommonForInsertAndDelete :1777-1810 - exactly the fields it sets */
            gdnBuyerDispatchDetailList: detailRows.map(function (r) {
                return {
                    gdnBuyerDispatchDetailId: int(r.Id),
                    grnSupplierLoadingMasterId: int(r.GrnId),
                    grnSupplierLoadingDetailId: int(r.GrnDetailId),
                    grnNo: int(r.GrnNo),
                    grnDate: ymd(r.GrnDate),
                    inventoryParentCategoryId: int(r.ParentItemId),
                    itemId: int(r.ItemId),
                    cropYearId: int(r.CropYearId),
                    cropYear: r.CropYear == null ? null : String(r.CropYear),
                    packingTypeId: int(r.PackingTypeId),
                    packUomId: int(r.PackUomId),
                    loadingQty: num(r.Qty),
                    wbGrossWeight: num(r.GrossWeight),
                    ebwPerUnit: num(r.EbUnit),
                    ebwTotal: num(r.EbTotal),
                    addLessWeight: num(r.AddLessWeight),
                    netBillWeight: num(r.NetBillWeight),
                    ebWeightDeductionTermId: int(r.EmptyBagTermId),
                    purchaseOrderMasterId: int(r.PurchaseOrderId),
                    purchaseOrderDetailId: int(r.PurchaseOrderDetailId),
                    poExpiryDate: ymd(r.POExpiryDate),
                    saleOrderMasterId: int(r.SaleOrderId),
                    saleOrderDetailId: int(r.SaleOrderDetailId),
                    supplierId: int(r.SupplierId),
                    remarks: r.Remarks == null ? '' : String(r.Remarks),
                    warningRemarks: r.WarningRemarks == null ? '' : String(r.WarningRemarks)
                };
            }),
            /* :2002-2016 (the server keeps rows with an item AND a packing type) */
            gdnBuyerDispatchEmptyBagDetailList: emptyBagRows.map(function (r) {
                return {
                    saleOrderMasterId: int(r.SoId),
                    saleOrderEmptyBagDetailId: int(r.SoEbId),
                    packingTypeId: int(r.PackingType),
                    rate: num(r.PurchaseRate),
                    emptyBagPackingMaterialItemId: int(r.EmptyBagItem)
                };
            }),
            /* :2017-2036 (the server keeps rows with an item AND Amount > 0) */
            gdnBuyerDispatchExpenseDetailList: expenseRows.map(function (r) {
                var it = otherItems.find(function (o) { return int(pick(o, 'id')) === int(r.ItemId); });
                return {
                    saleOrderMasterId: int(r.SoId),
                    saleOrderExpenseDetailId: int(r.SoExpenseId),
                    itemId: int(r.ItemId),
                    otherItemName: it ? pick(it, 'name') : '',
                    qty: num(r.Qty),
                    rate: num(r.Rate),
                    amount: num(r.Amount),
                    remarks: r.Remarks == null ? '' : String(r.Remarks)
                };
            })
        };
        return withButton(isUpdate ? 'btnUpdate' : 'btnSave', function () {
            return sendJSON(API + '/' + (isUpdate ? 'update' : 'save'), 'POST', payload).then(function (res) {
                if (res.status === 'SUCCESS') { alert(res.message || 'Save Successfully'); resetForm(); }
                else alert(res.message);
            }).catch(function (e) { alert('Server error while saving: ' + e.message); });
        });
    }

    /* ------------------------------------------------------------------ history / read / delete / reset */
    function mapDetailFromDb(x) {   // GdnBuyerDispatch_Helper.FillDetailtableFromListCommonForReadById
        return {
            Id: int(pick(x, 'gdnBuyerDispatchDetailId')),
            GrnId: int(pick(x, 'grnSupplierLoadingMasterId')),
            GrnDetailId: int(pick(x, 'grnSupplierLoadingDetailId')),
            GrnNo: pick(x, 'GrnNo'), GrnDate: ymd(pick(x, 'GrnDate')),
            ParentItemId: int(pick(x, 'inventoryParentCategoryId')), ParentItem: pick(x, 'InvParentCateDescription'),
            ItemId: int(pick(x, 'itemId')), ItemCode: pick(x, 'ItemCode'), ItemName: pick(x, 'ItemName'),
            CropYearId: int(pick(x, 'cropYearId')), CropYear: pick(x, 'cropYear'),
            PackingTypeId: int(pick(x, 'packingTypeId')), PackingType: pick(x, 'PackingType'),
            PackUomId: int(pick(x, 'packUomId')), PackUom: pick(x, 'PackSize'), PackUomEquivalent: num(pick(x, 'PEquivalent')),
            Qty: num(pick(x, 'loadingQty')), GrossWeight: num(pick(x, 'wbGrossWeight')),
            EbUnit: num(pick(x, 'ebwPerUnit')), EbTotal: num(pick(x, 'ebwTotal')),
            AddLessWeight: num(pick(x, 'addLessWeight')), NetBillWeight: num(pick(x, 'netBillWeight')),
            EmptyBagTermId: int(pick(x, 'EBWeightDeductionTermId')), EmptyBagTerm: pick(x, 'EBWeightDeductionTerm'),
            PurchaseOrderId: int(pick(x, 'purchaseOrderMasterId')), PurchaseOrderDetailId: int(pick(x, 'purchaseOrderDetailId')),
            PurchaseOrderNo: pick(x, 'PurchaseOrderNo'), POExpiryDate: pick(x, 'POExpiryDate'),
            SaleOrderId: int(pick(x, 'saleOrderMasterId')), SaleOrderDetailId: int(pick(x, 'saleOrderDetailId')),
            SaleOrderNo: pick(x, 'SaleOrderNo'), SOExpiryDate: pick(x, 'SOExpiryDate'),
            SupplierId: int(pick(x, 'supplierId')), SupplierName: pick(x, 'SupplierName'),
            Remarks: pick(x, 'remarks') || '', WarningRemarks: pick(x, 'warningRemarks') || ''
        };
    }

    /* ReadById :2101-2190 */
    function loadRecord(id) {
        return getJSON(API + '/' + id).then(function (res) {
            if (!res || res.status !== 'SUCCESS' || !res.data) { alert(res && res.message ? res.message : 'Record Not Found'); return; }
            var d = res.data;
            var details = d.gdnBuyerDispatchDetailList || [];
            if (!details.length) { alert('Record Not Found'); return; }
            resetForm(true);
            loadedHeader = d;
            recId = int(pick(d, 'gdnBuyerDispatchMasterId'));
            setVal('gdnBuyerDispatchMasterId', recId);
            setVal('docNo', pick(d, 'docNo'));
            setVal('docDate', ymd(pick(d, 'docDate')));
            setSel('commissionAgentId', pick(d, 'commissionAgentId'));
            setSel('buyerId', pick(d, 'BuyerId'));
            $id('buyerId').disabled = true;
            setVal('buyerRefDocNo', pick(d, 'BuyerRefDocNo') || '');
            var party = int(pick(d, 'DeliverToPartyId'));
            shipToSync = true;
            if (party > 0) setSel('deliverToPartyId', party);
            bindShipTo(party, int(pick(d, 'DeliverToAddressId'))).then(function () {
                shipToSync = false;
                setVal('deliverToAddress', pick(d, 'DeliverToAddress') || '');
            });
            setSel('loadingCityId', pick(d, 'loadingCityId'));
            setSel('unloadingCityId', pick(d, 'unloadingCityId'));
            if (int(pick(d, 'transporterId')) > 0) setSel('transporterId', pick(d, 'transporterId'));
            setVal('transporterName', pick(d, 'transporterName') || '');
            setVal('biltyFreight', num(pick(d, 'biltyFreight')));
            setVal('otherAdLesCharges', num(pick(d, 'otherAdLesCharges')));
            setVal('totalFreight', num(pick(d, 'totalFreight')));
            setSel('vehicleTypeId', pick(d, 'vehicleTypeId'));
            setVal('vehicleNo', pick(d, 'vehicleNo') || '');
            setVal('biltyNo', pick(d, 'biltyNo') || '');
            setVal('biltyDate', ymd(pick(d, 'biltyDate')));
            setSel('deliveryTermId', pick(d, 'deliveryTermId'));
            setVal('loadWeight', num(pick(d, 'loadWeight')));
            setVal('tareWeight', num(pick(d, 'tareWeight')));
            setVal('scaleNetWeight', num(pick(d, 'scaleNetWeight')));
            setVal('biltyQty', num(pick(d, 'biltyQty')));
            setVal('billWeight', num(pick(d, 'BillWeight')));
            setVal('freightRemarks', pick(d, 'FreightRemarks') || '');
            setVal('remarksHeader', pick(d, 'remarksHeader') || '');
            setVal('warningRemarks', pick(d, 'warningRemarks') || '');
            detailRows = details.map(mapDetailFromDb);
            emptyBagRows = (d.gdnBuyerDispatchEmptyBagDetailList || []).map(function (x) {
                return { Id: int(pick(x, 'gdnBuyerDispatchEmptyBagDetailId')), SoId: int(pick(x, 'saleOrderMasterId')),
                    SaleOrderNo: pick(x, 'SaleOrderNo'), SoEbId: int(pick(x, 'saleOrderEmptyBagDetailId')),
                    PackingType: int(pick(x, 'PackingTypeId')), PurchaseRate: num(pick(x, 'Rate')),
                    EmptyBagItem: int(pick(x, 'emptyBagPackingMaterialItemId')) };
            });
            expenseRows = (d.gdnBuyerDispatchExpenseDetailList || []).map(function (x) {
                return { Id: int(pick(x, 'gdnBuyerDispatchExpenseDetailId')), SoId: int(pick(x, 'saleOrderMasterId')),
                    SaleOrderNo: pick(x, 'SaleOrderNo'), SoExpenseId: int(pick(x, 'saleOrderExpenseDetailId')),
                    ItemId: int(pick(x, 'ItemId')), Qty: num(pick(x, 'Qty')), Rate: num(pick(x, 'rate')),
                    Amount: num(pick(x, 'amount')), Remarks: pick(x, 'remarks') || '' };
            });
            if (!expenseRows.length) addExpenseRow();      // BindGrids :1297
            if (!emptyBagRows.length) addEmptyBagRow();    // BindGrids :1308
            renderDetail(); renderEmptyBags(); renderExpenses();
            var grnId = detailRows[0].GrnId;
            setGrnCombo(grnId, detailRows[0].GrnNo);
            setVal('grnDate', ymd(detailRows[0].GrnDate));
            showWarn();
            refreshDiffs();
            applyButtonState();
            showTab('form');
            /* OutstandingOrdersdtFillDbCall (Id = RecId) then BindFromGrn on this GRN's rows, then GrnNoBind (:2173-2180) */
            return loadPending().then(function () {
                var rows = pendingAll.filter(function (r) { return int(pick(r, 'grnSupplierLoadingMasterId')) === grnId; });
                if (rows.length) bindFromGrn(rows);
                grnNoBind();
                setGrnCombo(grnId, detailRows[0].GrnNo);
            });
        }).catch(function (e) { alert('Record could not be loaded: ' + e.message); });
    }

    function deleteRecord() {   // btnDelete_Click :2191-2212
        if (recId <= 0) { alert('No record found to Delete'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        return withButton('btnDelete', function () {
            return sendJSON(API + '/' + recId, 'DELETE').then(function (res) {
                alert(res.message);
                if (res.status === 'SUCCESS') resetForm();
            }).catch(function () { alert('Server error while deleting.'); });
        });
    }

    function loadDocNo() {      // GenerateCode :677
        getJSON(API + '/generate-code').then(function (r) { if (r && r.docNo && recId <= 0) setVal('docNo', r.docNo); }).catch(function () {});
    }

    /* History From = today - DefaultDaysToLessFromHistoryFromDate (else 3), To = today (:574-575) */
    function setDefaultDates() {
        var t = new Date();
        setVal('docDate', ymdLocal(t));
        setVal('histToDate', ymdLocal(t));
        setVal('histFromDate', ymdLocal(new Date(t.getFullYear(), t.getMonth(), t.getDate() - 3)));
        getJSON(DD + '/config-defaults').then(function (c) {
            var days = c ? int(c.defaultDaysToLessFromHistoryFromDate) : 0;
            if (days > 0) setVal('histFromDate', ymdLocal(new Date(t.getFullYear(), t.getMonth(), t.getDate() - days)));
        }).catch(function () {});
    }

    /* Reset :2227-2280 */
    function resetForm(keepDates) {
        recId = 0;
        loadedHeader = null;
        loadedDataSet = null;
        setVal('gdnBuyerDispatchMasterId', 0);
        setVal('docNo', '');
        if (keepDates !== true) setDefaultDates();
        shipToSync = true;
        ['buyerId', 'commissionAgentId', 'deliverToPartyId', 'loadingCityId', 'unloadingCityId',
         'transporterId', 'vehicleTypeId', 'deliveryTermId', 'deliveryTermGrnId'].forEach(function (id) {
            var e = $id(id); if (e) { e.value = '0'; fire(e); }
        });
        shipToSync = false;
        $id('buyerId').disabled = false;
        $id('deliverToAddressId').innerHTML = '<option value="0">-- Select Address --</option>';
        ['vehicleNo', 'biltyNo', 'remarksHeader', 'buyerRefDocNo', 'deliverToAddress', 'transporterName',
         'freightRemarks', 'warningRemarks', 'biltyDate', 'grnDate', 'grnMasterId'].forEach(function (id) { setVal(id, ''); });
        showWarn();
        ['biltyFreight', 'otherAdLesCharges', 'totalFreight', 'biltyQty', 'loadWeight', 'tareWeight',
         'scaleNetWeight', 'billWeight'].forEach(function (id) { setVal(id, '0'); });
        PAIRS.forEach(function (p) { setVal(p[0], ''); });
        refreshDiffs();
        detailRows = [];
        emptyBagRows = []; addEmptyBagRow();
        expenseRows = []; addExpenseRow();
        renderDetail(); renderEmptyBags(); renderExpenses();
        applyButtonState();
        /* Reset :2280-2284 - OutstandingOrdersdtFillDbCall, GrnNoBind, GenerateCode, portal defaults */
        if (keepDates !== true) loadPending().then(grnNoBind);   // boot / ReadById load it themselves
        applyPortalDefaults();
        if (keepDates !== true) loadDocNo();
    }

    /* ------------------------------------------------------------------ busy buttons / states */
    var inFlight = {};
    /* disables its button, shows the busy spinner, blocks a second click, restores on success or failure */
    function withButton(btnId, work) {
        if (inFlight[btnId]) return Promise.resolve();
        var b = $id(btnId);
        inFlight[btnId] = true;
        if (b) { b.disabled = true; b.classList.add('btn-busy'); }
        var done = function () {
            inFlight[btnId] = false;
            if (b) { b.classList.remove('btn-busy'); b.disabled = false; }
            applyButtonState();
        };
        var res;
        try { res = work(); } catch (e) { done(); alert(e.message); return Promise.resolve(); }
        if (res && typeof res.then === 'function') return res.then(function (v) { done(); return v; }, function (e) { done(); alert(e && e.message ? e.message : e); });
        done();
        return Promise.resolve(res);
    }
    /* Reset :2233-2235 / ReadById :2113-2115 - Save for a new document, Update + Delete once loaded */
    /* formright (:522) - btnsave/btnUpdate/btnDelete/btnPrint.Enabled = the user's rights (:552-555).
       null until /rights answers: buttons stay disabled until then. */
    var rights = null;
    function right(k) { return !!(rights && rights[k]); }
    function loadRights() {
        return getJSON(API + '/rights').then(function (r) { rights = r || {}; applyButtonState(); })
            .catch(function () { rights = {}; applyButtonState(); });
    }
    function applyButtonState() {
        var loaded = recId > 0;
        /* Reset :2233-2236 shows Save and hides Update/Delete; ReadById :2113 hides Save and shows them */
        var vis = { btnSave: !loaded, btnUpdate: loaded, btnDelete: loaded };
        var rk = { btnSave: 'save', btnUpdate: 'update', btnDelete: 'delete', btnPrint: 'print' };
        Object.keys(rk).forEach(function (id) {
            var b = $id(id); if (!b) return;
            if (id in vis) b.style.display = vis[id] ? '' : 'none';
            if (!inFlight[id]) b.disabled = !right(rk[id]);
        });
    }
    /* LabelWarningRemarks / txtWarningRemarks are visible only when remarks exist (:1841, :2155, :2239) */
    function showWarn() {
        var w = $id('warnWrap'); if (w) w.style.display = val('warningRemarks').trim() ? '' : 'none';
    }

    /* ------------------------------------------------------------------ GRN No combo */
    /* OutstandingOrdersdtFillDbCall :829 - PendingDataLoaderForGdn with tenancy + Id = RecId only */
    var pendingAll = [], grnMap = {}, grnSync = false;
    function loadPending() {
        return getJSON(API + '/pending-grn?recId=' + (recId || 0)).then(function (p) {
            pendingAll = p && p.rows ? p.rows : [];
        }).catch(function () { pendingAll = []; });
    }
    /* GrnNoBind :850-880 - one row per GRN of the chosen buyer (DocNo shown, the rest hidden) */
    function grnNoBind() {
        var el = $id('grnNoId'); if (!el) return;
        var buyer = int(val('buyerId'));
        var keep = el.value, seen = {};
        grnMap = {};
        var h = '<option value="0"></option>';
        pendingAll.forEach(function (r) {
            if (int(pick(r, 'buyerId')) !== buyer) return;
            var id = int(pick(r, 'grnSupplierLoadingMasterId'));
            if (seen[id]) return;
            seen[id] = true; grnMap[id] = r;
            h += '<option value="' + id + '">' + esc(pick(r, 'docNo')) + '</option>';
        });
        el.innerHTML = h;
        el.value = keep; if (el.value !== keep) el.value = '0';
        grnSync = true; fire(el); grnSync = false;
    }
    /* set the combo without running its Leave handler (Load GRN :3496 / ReadById :2172) */
    function setGrnCombo(id, docNo) {
        var el = $id('grnNoId'); if (!el) return;
        id = int(id);
        if (id > 0 && !el.querySelector('option[value="' + id + '"]')) {
            var o = document.createElement('option'); o.value = String(id); o.textContent = docNo == null ? String(id) : String(docNo);
            el.appendChild(o);
        }
        el.value = String(id);
        setVal('grnMasterId', id || '');
        grnSync = true; fire(el); grnSync = false;
    }
    /* CmbGrnNoDetail_Leave :1012-1036 */
    function onGrnNoChange() {
        if (grnSync) return;
        var id = int(val('grnNoId'));
        setVal('grnMasterId', id || '');
        var r = grnMap[id]; if (!r || id <= 0) return;
        var tId = int(pick(r, 'transporterId'));
        if (tId > 0) setSel('transporterId', tId);
        setVal('transporterName', pick(r, 'transporterName') || '');
        setSel('loadingCityId', pick(r, 'loadingCityId'));
        setSel('unloadingCityId', pick(r, 'unloadingCityId'));
        setSel('vehicleTypeId', pick(r, 'vehicleTypeId'));
        setVal('vehicleNo', pick(r, 'vehicleNo') || '');
        setVal('biltyNo', pick(r, 'biltyNo') || '');
        setVal('biltyDate', ymd(pick(r, 'biltyDate')));
        setSel('deliveryTermId', pick(r, 'deliveryTermId'));
        var te = $id('deliveryTermGrnId'); if (te) { te.value = String(int(pick(r, 'deliveryTermId'))); fire(te); }
        setSel('deliverToPartyId', pick(r, 'DeliverToPartyId'));
    }

    /* ------------------------------------------------------------------ transporter / address tags */
    var transporterTag = '', addressTag = '';
    /* CmbTransporter_Leave :1255 - the combo's text goes to Transporter Name unless the name was typed */
    function onTransporterChange() {
        if (int(val('transporterId')) > 0 && !transporterTag) setVal('transporterName', selText('transporterId'));
    }

    /* ------------------------------------------------------------------ vehicle no :1098-1214 */
    var vehInternal = false;
    function vehNormalize(t, stripAll) {
        t = String(t || '').trim().toUpperCase();
        if (stripAll) t = t.replace(/[^A-Z0-9]/g, '');
        if (t.indexOf('-') < 0) {
            var m = (t.match(/^[A-Z]*/) || [''])[0].length;
            if (m >= 1 && m <= 6) t = t.slice(0, m) + '-' + t.slice(m);
        }
        return t;
    }
    function wireVehicleNo() {
        var tb = $id('vehicleNo'); if (!tb) return;
        tb.addEventListener('keypress', function (e) {          // txtvehicleno_KeyPress
            if (vehInternal || e.ctrlKey || e.metaKey || !e.key || e.key.length !== 1) return;
            var ch = e.key;
            if (!/[A-Za-z0-9-]/.test(ch)) { e.preventDefault(); return; }
            var text = tb.value, pos = tb.selectionStart || 0;
            if (ch === '-' && text.indexOf('-') >= 0) { e.preventDefault(); return; }
            var fut = text.slice(0, pos) + ch + text.slice(pos);
            var d = fut.indexOf('-');
            var letters = d >= 0 ? fut.slice(0, d) : fut;
            var digits = (d >= 0 && d < fut.length - 1) ? fut.slice(d + 1) : '';
            if (/[A-Za-z]/.test(ch) && letters.length > 6) { e.preventDefault(); return; }
            if (/[0-9]/.test(ch) && d >= 0 && digits.length > 6) { e.preventDefault(); return; }
            if (/[0-9]/.test(ch) && text.indexOf('-') < 0) {
                var lc = (text.match(/^[A-Za-z]*/) || [''])[0].length;
                if (lc >= 1 && lc <= 6) {
                    vehInternal = true;
                    tb.value = text.slice(0, lc) + '-' + text.slice(lc);
                    tb.setSelectionRange(pos + 1, pos + 1);
                    vehInternal = false;
                }
            }
        });
        tb.addEventListener('keydown', function (e) {           // txtvehicleno_KeyDown - Backspace over the dash
            if (vehInternal || e.key !== 'Backspace') return;
            var pos = tb.selectionStart || 0;
            if (pos > 0 && tb.selectionStart === tb.selectionEnd && tb.value.charAt(pos - 1) === '-') {
                e.preventDefault();
                tb.value = tb.value.slice(0, pos - 1) + tb.value.slice(pos);
                tb.setSelectionRange(pos - 1, pos - 1);
            }
        });
        tb.addEventListener('input', function () {              // txtVehicleNo_TextChanged - upper case
            if (vehInternal) return;
            var p = tb.selectionStart;
            var up = tb.value.toUpperCase();
            if (up !== tb.value) { tb.value = up; try { tb.setSelectionRange(p, p); } catch (x) { } }
        });
        tb.addEventListener('paste', function () {              // txtvehicleno_KeyUp Ctrl+V
            setTimeout(function () { tb.value = vehNormalize(tb.value, true); tb.setSelectionRange(tb.value.length, tb.value.length); }, 0);
        });
        tb.addEventListener('blur', function () {               // txtvehicleno_Leave
            if (!tb.value.trim()) return;
            tb.value = vehNormalize(tb.value, false);
        });
    }

    /* ------------------------------------------------------------------ portal defaults / refresh / print */
    /* GetCommissionAgentConfigurationsFromGlobalandBind :705-735 (called at load and on every Reset) */
    function applyPortalDefaults() {
        getJSON(API + '/portal-defaults').then(function (c) {
            if (!c) return;
            [['commissionAgentId', c.commissionAgentId], ['deliveryTermId', c.deliveryTermId],
             ['loadingCityId', c.loadingCityId], ['unloadingCityId', c.unloadingCityId]].forEach(function (d) {
                if (int(d[1]) > 0) trySet(d[0], int(d[1]), 20);
            });
        }).catch(function () {});
    }
    function trySet(id, v, n) {     // the list may still be loading
        if (recId > 0) return;
        var e = $id(id); if (!e) return;
        if (e.querySelector('option[value="' + v + '"]')) setSel(id, v);
        else if (n > 0) setTimeout(function () { trySet(id, v, n - 1); }, 300);
    }
    /* BtnRefresh_Click :2296-2322 */
    function refreshAll() {
        return withButton('btnRefresh', function () {
            loadDropdowns();
            if (int(val('deliverToPartyId')) > 0) {
                shipToSync = true;
                bindShipTo(int(val('deliverToPartyId')), int(val('deliverToAddressId'))).then(function () { shipToSync = false; });
            }
            return loadPending().then(grnNoBind);
        });
    }
    /* btnPrint_Click :2571 - CrystalReportPrint_Helper.GrnSupplierLoadingSlip_1054(RecId) */
    function printCurrent(id) {
        id = int(id) || recId;
        if (id <= 0) { alert('No Data found to display'); return; }
        if (window.printRpt) return window.printRpt('1055_GdnBuyerDispatchSlip.rpt', { id: id });   // Jasper, the GDN slip
        window.open('/api/print/by-template/1055_GdnBuyerDispatchSlip.rpt/pdf?id=' + id, '_blank');
    }

    /* ------------------------------------------------------------------ history */
    var histCombo = [];
    /* HistoryComboDbCall :589 */
    function loadHistoryCombos() {
        return getJSON(API + '/history/combos').then(function (d) { histCombo = d || []; historyComboBind(); }).catch(function () {});
    }
    function fillHist(id, list) {
        var el = $id(id); if (!el) return;
        var keep = el.value;
        el.innerHTML = '<option value="0"></option>' + list.map(function (r) {
            return '<option value="' + esc(r.id) + '">' + esc(r.name) + '</option>';
        }).join('');
        el.value = keep; if (el.value !== keep) el.value = '0';
        fire(el);
    }
    var HIST_MAP = { CommissionAgent: 'histCommissionAgentId', BuyerName: 'histBuyerId', Item: 'histItemId',
                     DeliveryToParty: 'histDeliverToPartyId', DeliverToAddress: 'histDeliverToAddress' };
    function bucketsFor(filter, distinct) {
        var b = {}, seen = {};
        Object.keys(HIST_MAP).forEach(function (k) { b[k] = []; seen[k] = {}; });
        histCombo.forEach(function (r) {
            var a = String(pick(r, 'Activity') || '');
            if (!b[a] || !filter(r)) return;
            var id = int(pick(r, 'Id'));
            if (distinct && a !== 'Item') { if (seen[a][id]) return; seen[a][id] = true; }
            b[a].push({ id: id, name: pick(r, 'ReferenceName') == null ? '' : String(pick(r, 'ReferenceName')) });
        });
        Object.keys(HIST_MAP).forEach(function (k) { fillHist(HIST_MAP[k], b[k]); });
    }
    /* HistoryComboBind :2630-2700 - lists for the most-used parent category; parent list is checkable */
    function historyComboBind() {
        var most = histCombo.filter(function (r) { return pick(r, 'Activity') === 'GetMostUsedParentCategoryId'; });
        var mostId = most.length ? int(pick(most[0], 'Id')) : -1;
        var parents = histCombo.filter(function (r) { return pick(r, 'Activity') === 'ParentCategories'; });
        $id('histPcList').innerHTML = parents.map(function (r) {
            var id = int(pick(r, 'Id'));
            return '<label class="d-block"><input type="checkbox" class="hist-pc-chk" value="' + id + '"'
                + (most.length === 1 && id === mostId ? ' checked' : '') + '> ' + esc(pick(r, 'ReferenceName')) + '</label>';
        }).join('');
        updatePcText();
        bucketsFor(function (r) { return int(pick(r, 'ParentCategoryId')) === mostId; }, false);
    }
    function selectedPcIds() {
        return Array.prototype.map.call(document.querySelectorAll('.hist-pc-chk:checked'), function (c) { return int(c.value); });
    }
    function updatePcText() {
        setVal('histPcText', Array.prototype.map.call(document.querySelectorAll('.hist-pc-chk:checked'), function (c) {
            return c.parentNode.textContent.trim();
        }).join(','));
    }
    /* BindDropdownsAgainstParentCategory :2702-2744 (CmbParentItemHistory_Leave) */
    function bindAgainstParent() {
        var ids = selectedPcIds();
        bucketsFor(function (r) { return !ids.length || ids.indexOf(int(pick(r, 'ParentCategoryId'))) >= 0; }, true);
    }
    function wireHistoryPc() {
        var box = $id('histPcBox'); if (!box) return;
        $id('histPcText').addEventListener('click', function () { box.classList.toggle('open'); });
        $id('histPcList').addEventListener('change', function () { updatePcText(); bindAgainstParent(); });
        document.addEventListener('click', function (e) { if (!box.contains(e.target)) box.classList.remove('open'); });
    }
    /* btnNewHistory_Click :2600 */
    function historyNew() {
        ['histItemId', 'histCommissionAgentId', 'histBuyerId'].forEach(function (id) { var e = $id(id); if (e) { e.value = '0'; fire(e); } });
    }
    /* HistoryFill :2745 - dt2 columns (:2792-2814); Edit + Print buttons (:2860-2861) */
    var HIST_COLS = [
        ['docNo', 'DocNo'], ['docDate', 'DocDate', 'd'], ['CommissionAgentName', 'CommissionAgent'], ['BuyerName', 'BuyerName'],
        ['DeliverToPartyName', 'DeliverToParty'], ['DeliverToAddress', 'DeliverToAddress'], ['LoadingCity', 'LoadingCity'],
        ['UnloadingCity', 'UnloadingCity'], ['biltyFreight', 'BiltyFreight', 'n'], ['otherAdLesCharges', 'OtherCharges', 'n'],
        ['totalFreight', 'TotalFreight', 'n'], ['VehicleType', 'VehicleType'], ['vehicleNo', 'VehicleNo'],
        ['biltyDate', 'BiltyDate', 'd'], ['biltyNo', 'BiltyNo'], ['DeliveryTerm', 'DeliveryTerm'], ['EntryUserName', 'EntryUser'],
        ['entryDate', 'EntryDate', 'dt'], ['ModifyUserName', 'ModifyUser'], ['modifyDate', 'ModifyDate', 'dt'],
        ['NoOfAttachments', 'NoOfAttachments', 'i']
    ];
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dMMMy(v, withTime) {   // dd-MMM-yyyy / dd-MMM-yy hh:mm tt, from the server's local text
        if (!v) return '';
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/);
        if (!m) return String(v);
        var s = m[3] + '-' + MON[int(m[2]) - 1] + '-' + (withTime ? m[1].slice(2) : m[1]);
        if (withTime && m[4] !== undefined) {
            var h = int(m[4]), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
            s += ' ' + (h < 10 ? '0' : '') + h + ':' + m[5] + ' ' + ap;
        }
        return s;
    }
    function fetchHistory() {
        return withButton('btnLoadHistory', function () {
            var q = new URLSearchParams();
            var dt = document.querySelector('input[name="histDateType"]:checked');
            q.set('dateType', dt ? dt.value : 'doc');
            if ($id('histFromChk').checked && val('histFromDate')) q.set('fromDate', val('histFromDate'));
            if ($id('histToChk').checked && val('histToDate')) q.set('toDate', val('histToDate'));
            q.set('commissionAgentId', int(val('histCommissionAgentId')));
            q.set('buyerId', int(val('histBuyerId')));
            q.set('deliverToPartyId', int(val('histDeliverToPartyId')));
            if (int(val('histDeliverToAddress'))) q.set('shipToAddress', selText('histDeliverToAddress'));   // CmbDeliverToAddressHistory.Text
            return getJSON(API + '/history?' + q.toString()).then(renderHistory)
                .catch(function (e) { alert('History could not be loaded: ' + e.message); });
        });
    }
    function renderHistory(data) {
        var tbl = $id('gridHistory');
        tbl.querySelector('thead').innerHTML = '<tr><th></th><th></th>' + HIST_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        ['gridHistoryDetail', 'gridHistoryEb', 'gridHistoryEx'].forEach(function (id) { $id(id).querySelector('thead').innerHTML = ''; $id(id).querySelector('tbody').innerHTML = ''; });
        if (!data || !data.length) {
            tbl.querySelector('tbody').innerHTML = '<tr><td colspan="' + (HIST_COLS.length + 2) + '" class="text-center text-muted">No history records found.</td></tr>';
            tbl.querySelector('tfoot').innerHTML = '';
            return;
        }
        var sums = { biltyFreight: 0, otherAdLesCharges: 0, totalFreight: 0 };
        tbl.querySelector('tbody').innerHTML = data.map(function (r) {
            var id = int(pick(r, 'gdnBuyerDispatchMasterId'));
            for (var k in sums) sums[k] += num(pick(r, k));
            return '<tr data-id="' + id + '"><td><button type="button" class="btn btn-sm btn-outline-primary py-0 h-edit">Edit</button></td>'
                + '<td><button type="button" class="btn btn-sm btn-outline-dark py-0 h-print">Print</button></td>'
                + HIST_COLS.map(function (c) {
                    var v = pick(r, c[0]);
                    if (c[0] === 'docNo') return '<td><span class="clickable-code">' + esc(v) + '</span></td>';
                    if (c[2] === 'd') return '<td>' + esc(dMMMy(v, false)) + '</td>';
                    if (c[2] === 'dt') return '<td>' + esc(dMMMy(v, true)) + '</td>';
                    if (c[2] === 'n') return '<td class="num">' + esc(fmt(v)) + '</td>';
                    if (c[2] === 'i') return '<td class="num">' + int(v) + '</td>';
                    return '<td>' + esc(v == null ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        tbl.querySelector('tfoot').innerHTML = '<tr><td colspan="2">' + data.length + '</td>' + HIST_COLS.map(function (c) {
            return '<td class="num">' + (sums[c[0]] !== undefined ? esc(fmt(sums[c[0]])) : '') + '</td>';
        }).join('') + '</tr>';
    }
    function onHistoryClick(e) {
        var tr = e.target.closest('tr[data-id]'); if (!tr) return;
        var id = int(tr.getAttribute('data-id'));
        /* grdHistory_KeyDown :3386-3412 - Edit needs Update rights, Print needs Print rights */
        if (e.target.closest('.h-edit') || e.target.closest('.clickable-code')) {
            if (!right('update')) { alert("you don't have update rights..."); return; }
            loadRecord(id); return;
        }
        if (e.target.closest('.h-print')) {
            if (!right('print')) { alert("you don't have print rights..."); return; }
            printCurrent(id); return;
        }
        Array.prototype.forEach.call(tr.parentNode.children, function (x) { x.classList.toggle('hist-selected', x === tr); });
        historyDetail(id);
    }
    /* grdHistory_SelectionChanged -> GetDetailGrdByHeadId :3113-3150 */
    function historyDetail(id) {
        getJSON(API + '/' + id).then(function (res) {
            var d = res && res.data; if (!d) return;
            var rows = (d.gdnBuyerDispatchDetailList || []).map(mapDetailFromDb);
            var showPo = rows.some(function (r) { return int(r.PurchaseOrderId) > 0; });
            var cols = DETAIL_COLS.filter(function (c) { return c[2] !== 'po' || showPo; });
            var t = $id('gridHistoryDetail');
            t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.querySelector('tbody').innerHTML = rows.map(function (r) {
                return '<tr>' + cols.map(function (c) {
                    var v = r[c[0]];
                    if (c[2] === 'n') return '<td class="num">' + esc(fmt(v)) + '</td>';
                    if (c[2] === 'dt') return '<td>' + esc(dmy(v)) + '</td>';
                    return '<td>' + esc(v == null ? '' : v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            var nameOf = function (list, idKey, nameKey, id2) {
                var f = (list || []).find(function (o) { return int(pick(o, idKey)) === int(id2); });
                return f ? pick(f, nameKey) : '';
            };
            var eb = d.gdnBuyerDispatchEmptyBagDetailList || [];
            var showSo = eb.some(function (x) { return int(pick(x, 'saleOrderMasterId')) > 0; });
            $id('gridHistoryEb').querySelector('thead').innerHTML = '<tr>' + (showSo ? '<th>Sale Order No</th>' : '') + '<th>Pack Type</th><th>Purchase Rate</th><th>Empty Bag Item Name</th></tr>';
            $id('gridHistoryEb').querySelector('tbody').innerHTML = eb.map(function (x) {
                return '<tr>' + (showSo ? '<td>' + esc(int(pick(x, 'SaleOrderNo')) || '') + '</td>' : '')
                    + '<td>' + esc(pick(x, 'PackingType') || nameOf(packingTypes, 'Id', 'ReferenceName', pick(x, 'PackingTypeId'))) + '</td>'
                    + '<td class="num">' + esc(fmt(pick(x, 'Rate'))) + '</td>'
                    + '<td>' + esc(pick(x, 'EmptyBagItem') || nameOf(emptyBagItems, 'ItemId', 'ItemName', pick(x, 'emptyBagPackingMaterialItemId'))) + '</td></tr>';
            }).join('');
            var ex = d.gdnBuyerDispatchExpenseDetailList || [];
            var showSo2 = ex.some(function (x) { return int(pick(x, 'saleOrderMasterId')) > 0; });
            $id('gridHistoryEx').querySelector('thead').innerHTML = '<tr>' + (showSo2 ? '<th>Sale Order No</th>' : '') + '<th>Other Item Name</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
            $id('gridHistoryEx').querySelector('tbody').innerHTML = ex.map(function (x) {
                return '<tr>' + (showSo2 ? '<td>' + esc(int(pick(x, 'SaleOrderNo')) || '') + '</td>' : '')
                    + '<td>' + esc(pick(x, 'OtherItemName') || nameOf(otherItems, 'id', 'name', pick(x, 'ItemId'))) + '</td>'
                    + '<td class="num">' + esc(fmt(pick(x, 'Qty'))) + '</td><td class="num">' + esc(fmt(pick(x, 'rate'))) + '</td>'
                    + '<td class="num">' + esc(fmt(pick(x, 'amount'))) + '</td><td>' + esc(pick(x, 'remarks') || '') + '</td></tr>';
            }).join('');
        }).catch(function () {});
    }

    /* ------------------------------------------------------------------ shortcuts :3223-3360 */
    function onShortcut(e) {
        if ($id('grnLoaderModal').style.display === 'flex' || $id('shipToModal').style.display === 'flex') return;
        var k = (e.key || '').toLowerCase();
        var onForm = $id('tab-form').classList.contains('active');
        var click = function (id) { var b = $id(id); if (b && !b.disabled && b.style.display !== 'none') b.click(); };
        if (e.ctrlKey && k === 't') { e.preventDefault(); showTab(onForm ? 'history' : 'form'); return; }
        if (onForm) {
            if (e.ctrlKey && e.shiftKey && k === 'delete') { e.preventDefault(); click('btnDelete'); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); click('btnSave'); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); click('btnUpdate'); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); click('btnPrint'); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); click('btnNew'); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); click('btnRefresh'); }
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); $id('docDate').focus(); }
        } else if (e.ctrlKey && k === 's') { e.preventDefault(); click('btnLoadHistory'); }
    }

    function showTab(which) {
        var f = which === 'form';
        $id('tab-form').classList.toggle('show', f); $id('tab-form').classList.toggle('active', f);
        $id('tab-history').classList.toggle('show', !f); $id('tab-history').classList.toggle('active', !f);
        $id('tab-form-link').classList.toggle('active', f);
        $id('tab-history-link').classList.toggle('active', !f);
    }

    /* ------------------------------------------------------------------ wiring */
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function boot() {
        /* report Doc No links open /commission/goods-dispatching-note?id=N: wait for the
           dropdowns, then load the record */
        (function () {
            var qm = /[?&]id=(\d+)/.exec(location.search); if (!qm) return;
            var tries = 0, t = setInterval(function () {
                var b = $id('buyerId');
                if ((b && b.options.length > 1) || ++tries > 40) { clearInterval(t); loadRecord(+qm[1]); }
            }, 250);
        })();
        setDefaultDates();
        loadRights();
        loadDropdowns();
        resetForm(true);
        loadDocNo();

        on('tab-form-link', 'click', function (e) { e.preventDefault(); showTab('form'); });
        on('tab-history-link', 'click', function (e) { e.preventDefault(); showTab('history'); });
        on('btnNew', 'click', function () { withButton('btnNew', function () { resetForm(); }); });
        on('btnRefresh', 'click', refreshAll);
        on('btnPrint', 'click', printCurrent);
        on('btnHistory', 'click', function () { showTab('history'); });
        on('btnNewHistory', 'click', historyNew);
        on('btnRefreshHistory', 'click', function () { withButton('btnRefreshHistory', loadHistoryCombos); });
        on('gridHistory', 'dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]'); if (!tr) return;
            if (!right('update')) { alert("you don't have update rights..."); return; }
            loadRecord(int(tr.getAttribute('data-id')));
        });
        on('grnNoId', 'change', onGrnNoChange);
        on('transporterId', 'change', onTransporterChange);
        on('transporterName', 'keypress', function () {            // txtTransporterName_KeyPress :1270
            transporterTag = val('transporterName');
            var t = $id('transporterId'); if (int(t.value)) { t.value = '0'; fire(t); }
        });
        on('deliverToAddress', 'keypress', function () {           // txtDeliveryToAddress_KeyPress :1242
            addressTag = val('deliverToAddress');
            var a2 = $id('deliverToAddressId'); if (int(a2.value)) { shipToSync = true; a2.value = '0'; fire(a2); shipToSync = false; }
        });
        wireVehicleNo();
        wireHistoryPc();
        Array.prototype.forEach.call(document.querySelectorAll('[data-fs]'), function (b) {
            b.addEventListener('click', function () { var w = $id(b.getAttribute('data-fs')); if (w) w.classList.toggle('gdn-fs'); });
        });
        document.addEventListener('keydown', onShortcut);
        loadHistoryCombos();
        loadPending().then(grnNoBind);
        on('btnSave', 'click', function () { save(false); });
        on('btnUpdate', 'click', function () { save(true); });
        on('btnDelete', 'click', deleteRecord);
        on('btnLoadHistory', 'click', fetchHistory);
        /* tabControl3 (History): Detail / Other Expense / Empty Bags Weight Deduction From Bill Weight */
        Array.prototype.forEach.call(document.querySelectorAll('[data-htab]'), function (t) {
            t.addEventListener('click', function (e) {
                e.preventDefault();
                Array.prototype.forEach.call(document.querySelectorAll('[data-htab]'), function (x) {
                    x.classList.toggle('active', x === t);
                    var p = $id(x.getAttribute('data-htab')); if (p) p.style.display = x === t ? '' : 'none';
                });
            });
        });
        on('btnShortCutKeys', 'click', function () {   // BtnShortCutkeys
            alert('Ctrl+S Save\nCtrl+U Update\nCtrl+Shift+Del Delete\nCtrl+N New\nCtrl+R Refresh\nCtrl+P Print\nCtrl+T Switch Tab');
        });
        on('btnLoadGrn', 'click', openLoader);
        on('gridHistory', 'click', onHistoryClick);

        on('deliverToPartyId', 'change', function () { if (!shipToSync) bindShipTo(int(val('deliverToPartyId'))); });
        on('deliverToAddressId', 'change', onShipToChange);
        on('buyerId', 'change', function () { onBuyerChange(); grnNoBind(); });

        on('gridItems', 'change', onDetailEdit);
        on('gridEmptyBags', 'click', onEmptyBagEvent);
        on('gridEmptyBags', 'change', onEmptyBagEvent);
        on('gridExpenses', 'click', onExpenseEvent);
        on('gridExpenses', 'change', onExpenseEvent);

        on('loadWeight', 'input', calcGdnNet);
        on('tareWeight', 'input', calcGdnNet);
        on('scaleNetWeight', 'input', function () {
            /* a typed WB net that differs from load - tare clears both (:2474-2478) */
            setVal('loadWeight', ''); setVal('tareWeight', '');
            netChanged();
        });
        on('biltyQty', 'input', refreshDiffs);
        on('billWeight', 'input', refreshDiffs);
        on('biltyFreight', 'input', function () { freightBase = 1; calcFreight(); });
        on('otherAdLesCharges', 'input', calcFreight);
        on('totalFreight', 'input', function () { freightBase = 2; calcFreight(); });

        /* loader dialog */
        on('ldBtnLoad', 'click', loaderSearch);
        on('ldBtnReset', 'click', function () {
            /* btnReset_Click :650 - From = active-year start (the procedure already limits to the
               active year, so a blank From is the same set), clear doc nos and item */
            setVal('ldFromDate', ''); setVal('ldDocNoFrom', ''); setVal('ldDocNoTo', '');
            var it = $id('ldItemId'); if (it) { it.value = '0'; fire(it); }
            loaderSearch();
        });
        on('ldBtnRefresh', 'click', loadLoaderCombos);
        on('ldBtnClose', 'click', closeLoader);
        on('ldBtnShortcut', 'click', function () { alert('Ctrl+N  New\nCtrl+R  Refresh\nCtrl+S  Show\nEsc  Close'); });
        on('btnDefineShipToAddress', 'click', openShipTo);
        on('btnStNew', 'click', stReset);
        on('btnStRefresh', 'click', stGrid);
        on('btnStSave', 'click', stSave);
        on('btnStUpdate', 'click', stSave);
        on('btnStClose', 'click', closeShipTo);
        on('grdShipToBody', 'dblclick', function (e) { var tr = e.target.closest('tr[data-st]'); if (tr) stEdit(int(tr.getAttribute('data-st'))); });
        on('ldMaster', 'click', function (e) {
            var b = e.target.closest('.ld-load');
            if (b) { loaderLoad(int(b.getAttribute('data-grn'))); return; }
            var tr = e.target.closest('tr[data-grn]');
            if (tr) selectLoaderRow(int(tr.getAttribute('data-grn')));
        });
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && $id('shipToModal').style.display === 'flex') { closeShipTo(); return; }
            if ($id('grnLoaderModal').style.display === 'flex') {
                if (e.key === 'Escape') closeLoader();
                else if (e.ctrlKey && !e.shiftKey && (e.key === 's' || e.key === 'S')) { e.preventDefault(); loaderSearch(); }
                else if (e.ctrlKey && !e.shiftKey && (e.key === 'r' || e.key === 'R')) { e.preventDefault(); loadLoaderCombos(); }
                else if (e.ctrlKey && !e.shiftKey && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); $id('ldBtnReset').click(); }
            }
        });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot); else boot();
}());
