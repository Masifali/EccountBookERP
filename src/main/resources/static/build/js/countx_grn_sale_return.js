/* =============================================================================================
 * Grn (Sale Return) - SaleReturnGrn.cs (screen 866, DocumentTypeId 143).
 * Every function below names the desktop method it ports; ":NNNN" is the line in SaleReturnGrn.cs.
 * Server: /api/purchase/sale-return-grn (SaleReturnGrnRestController). Tenancy comes from the session.
 * ============================================================================================= */
(function (global) {
    'use strict';
    var doc = global.document, API = '/api/purchase/sale-return-grn';
    var BOOT = global.SRG_BOOT || {};

    /* ------------------------------------------------------------------ small helpers */
    function $(id) { return doc.getElementById(id); }
    function esc(v) { return String(v === undefined || v === null ? '' : v).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }
    function col(row, key) {
        if (!row) return undefined;
        if (key in row) return row[key];
        var k = key.toLowerCase();
        for (var p in row) if (p.toLowerCase() === k) return row[p];
        return undefined;
    }
    /* Conversion.ToDouble / ToInt: blank or bad text -> 0; thousands separators accepted. */
    function toD(v) { if (typeof v === 'number') return isFinite(v) ? v : 0; var n = parseFloat(String(v === undefined || v === null ? '' : v).replace(/,/g, '').trim()); return isFinite(n) ? n : 0; }
    function toI(v) { return Math.trunc(toD(v)); }
    /* double.ToString() on .NET Framework: 15 significant digits. */
    function cs(x) { x = toD(x); return String(parseFloat(x.toPrecision(15))); }
    /* Math.Round(x, d) - MidpointRounding.ToEven. */
    function round(x, d) {
        var m = Math.pow(10, d), v = x * m, f = Math.floor(v), diff = v - f;   /* value *= 10^d; Math.Round(value); value /= 10^d */
        var r = diff === 0.5 ? (f % 2 === 0 ? f : f + 1) : Math.round(v);
        return r / m;
    }
    /* ToString("#,##0.###") and ToString("#,##.##") (the latter prints nothing for 0). */
    function fmt3(x) { return toD(x).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }
    function fmt2blank(x) { x = toD(x); return x === 0 ? '' : x.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }); }
    function val(id) { var e = $(id); return e ? e.value : ''; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function addDays(n) { var d = new Date(); d.setDate(d.getDate() + n); return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function dateText(v) {
        if (v === null || v === undefined || v === '') return '';
        if (typeof v === 'number') { var d = new Date(v); return isNaN(d) ? '' : d.toISOString().slice(0, 10); }
        return String(v).replace('T', ' ').slice(0, 10);
    }
    function dateTimeText(v) {
        if (v === null || v === undefined || v === '') return '';
        if (typeof v === 'number') { var d = new Date(v); return isNaN(d) ? '' : d.toISOString().replace('T', ' ').slice(0, 16); }
        return String(v).replace('T', ' ').slice(0, 16);
    }
    function msg(t) { global.alert(t); }
    function fail(e) { msg((e && e.message) || 'The request failed. Please retry.'); }

    async function api(path, options) {
        var run = async function () {
            var res = await fetch(API + path, Object.assign({ credentials: 'same-origin' }, options || {}));
            var text = await res.text(), data = null;
            try { data = text ? JSON.parse(text) : null; } catch (e) { data = null; }
            if (!res.ok || (data && data.success === false)) throw new Error((data && (data.message || data.error || data.detail)) || ('Request failed (' + res.status + ')'));
            return data;
        };
        return global.PurchaseRequest ? global.PurchaseRequest.track(run) : run();
    }
    function post(path, body) { return api(path, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }); }

    /* POST a typed print body and show the PDF (same contract as FA.printPdf). */
    function printPdf(url, body) {
        var win = global.open('about:blank', '_blank');
        return fetch(url, { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', 'Accept': 'application/pdf' }, body: JSON.stringify(body || {}) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { if (win) win.location = URL.createObjectURL(b); });
                return r.text().then(function (x) { if (win) win.close(); var m = x; try { m = JSON.parse(x).message || x; } catch (e) { /* plain text */ } msg(m || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (win) win.close(); fail(e); });
    }

    /* ------------------------------------------------------------------ combos */
    if (global.DesktopCombo) {
        /* CmbItemName: dtitem Id, ItemName, ItemCode bound AllColumns (:927); display member follows the Name/Code radio. */
        global.DesktopCombo.define('srgItem', [
            { caption: 'Item Name', flex: 3 },
            { caption: 'ItemCode', flex: 2, key: 'item-code' },
            { caption: 'ItemName', flex: 3, key: 'item-name' }
        ]);
    }
    function fill(id, rows, valueKey, textKey, opts) {
        var sel = $(id); if (!sel) return;
        opts = opts || {};
        var keep = opts.keep !== undefined ? String(opts.keep) : sel.value;
        /* InfragisticsHelper.BindAndRetainSelection (InfragisticsHelper.cs :15): insertDefaultRow puts a "-- Select --" row (value 0)
           first; when the previous value is not in the list the row at activateRowIndex is activated - 0 by default, so a combo
           bound WITHOUT a default row (Transporter :861, Vehicle Type :877) shows its first record, and Item Name (:927, index 1)
           shows the first item. An empty source clears the combo (:17). */
        var hasRows = !!(rows && rows.length);
        var html = opts.noBlank ? '' : '<option value="0">' + (opts.defaultRow && hasRows ? '-- Select --' : '') + '</option>';
        (rows || []).forEach(function (r) {
            var attrs = opts.attrs ? opts.attrs(r) : '';
            html += '<option value="' + esc(col(r, valueKey)) + '"' + attrs + '>' + esc(col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel._rows = rows || [];
        if (keep && Array.prototype.some.call(sel.options, function (o) { return o.value === keep; })) sel.value = keep;
        else if (opts.activate !== undefined && sel.options.length > opts.activate) sel.selectedIndex = opts.activate;
        else sel.value = opts.noBlank && sel.options.length ? sel.options[0].value : '0';
    }
    /* Janus shows a column key with a space before each capital ("GrossWight" -> "Gross Wight", "GpSrNo" -> "Gp Sr No"). */
    function cap(k) { return String(k).replace(/([a-z])([A-Z])/g, '$1 $2'); }
    function setSel(id, v) {
        var sel = $(id); if (!sel) return;
        var s = String(v === undefined || v === null ? '0' : v);
        if (Array.prototype.some.call(sel.options, function (o) { return o.value === s; })) sel.value = s; else sel.value = sel.options.length && sel.options[0].value === '0' ? '0' : s;
    }
    function selText(id) { var sel = $(id); return sel && sel.selectedOptions[0] && sel.value !== '0' ? sel.selectedOptions[0].textContent : ''; }
    /* Setting .Text on an UltraCombo selects the row whose display text matches. */
    function setSelByText(id, text) {
        var sel = $(id); if (!sel) return;
        var t = String(text === undefined || text === null ? '' : text).trim();
        var hit = Array.prototype.find.call(sel.options, function (o) { return o.textContent.trim() === t && t !== ''; });
        sel.value = hit ? hit.value : (sel.options.length && sel.options[0].value === '0' ? '0' : sel.value);
    }
    function lookupText(list, id, textKey) {
        var r = (list || []).find(function (x) { return String(col(x, 'id')) === String(id); });
        return r ? String(col(r, textKey) === undefined ? col(r, 'name') : col(r, textKey)) : '';
    }

    /* ------------------------------------------------------------------ state (form fields of SaleReturnGrn) */
    var S = {
        lists: {}, configs: {}, rights: {}, subsidiary: false, wagesActive: false,
        RecId: 0, updateDetailIndex: -1, FreightOnCash: 0, FreightId: 0, GdnDetailIds: '',
        dtdetail: [], dtEmptyBag: [], emptyBagsPlus: true, gpRows: [], historyRows: [],
        curDetail: -1, curGp: -1, curHistory: -1, itemRows: [], gdnRows: []
    };
    /* the Attachment form (AT) - btnAttachment_Click, saved with Insert() (:1300 / :1462-1471) */
    var AT = global.PurchaseDocAttachments ? global.PurchaseDocAttachments.create({
        type: 143, getId: function () { return S.RecId; },
        canEdit: function () { return S.RecId > 0 ? !!S.rights.Update : !!S.rights.Save; },
        message: function (m) { msg(m); }
    }) : null;
    function cfgBool(n) { var v = String(S.configs[n] || '').trim().toLowerCase(); return v === '1' || v === 'true'; }
    function cfgInt(n) { return toI(S.configs[n]); }
    function cfgDbl(n) { return toD(S.configs[n]); }
    var saveVisible = function () { return !$('btnSave').hidden; };
    var saveEnabled = function () { return !$('btnSave').disabled; };

    /* ------------------------------------------------------------------ TextChanged cascade
       WinForms raises TextChanged only when the text really changes, so code that assigns a box
       re-enters the same calculations the desktop re-enters. setText reproduces that. */
    var depth = 0;
    var TEXT_CHANGED = {
        txtsuppwt: function () { TotalSupplierWeight(); },
        txtfctwt: function () { TotalSupplierWeight(); },
        txtQty: function () { TotalWeight(); Total(); },
        txtGrossWeight: function () { Total(); },
        txtebu: function () { Total(); },
        txtebt: function () { Total(); },
        txtwtcut: function () { Total(); },
        txtadlswt: function () { Total(); }
    };
    function setText(id, v) {
        var e = $(id); if (!e) return;
        var s = v === undefined || v === null ? '' : String(v);
        if (e.value === s) return;
        e.value = s;
        if (TEXT_CHANGED[id] && depth < 25) { depth++; try { TEXT_CHANGED[id](); } finally { depth--; } }
    }
    function activeTag() { var a = doc.activeElement; return a && a.getAttribute ? (a.getAttribute('data-tag') || '') : ''; }

    /* ------------------------------------------------------------------ binds (InitializeComponentMethod :545) */
    function bindSuppliers() {
        /* BindSupplierName :802 - Id, CompanyName, PartyCode, [GlAccountId], [CityId], CityName, MobileNo. */
        fill('CmbSupplierName', S.lists.suppliers, 'id', 'name', { attrs: function (r) { return ' data-code="' + esc(col(r, 'PartyCode')) + '" data-city="' + esc(col(r, 'CityName')) + '" data-mobile="' + esc(col(r, 'MobilePersonal')) + '"'; } });
    }
    function bindTransporters() {
        /* TransporterDtFillFromGlobal :816 / TransporterAcFill :857 - Id, AccountTitle, AccountCode, [SupplierCustomerId]. */
        fill('CmbTransport', S.lists.transporters, 'id', 'name', { activate: 1, attrs: function (r) {
            var code = col(r, 'AccountCode'); if (code === undefined) code = col(r, 'PartyCode');
            return ' data-code="' + esc(code) + '" data-supcust="' + esc(S.subsidiary ? col(r, 'Id') : 0) + '"';
        } });
    }
    function bindUom(itemId) {
        /* PackUomFromGlobalBind :995 - UomSchedule of the item, selection retained by text (Packuom = CmbUom.Text). */
        var keepText = selText('CmbUom');
        var rows = (S.lists.uoms || []).filter(function (u) { return toI(col(u, 'ItemId')) === toI(itemId); });
        fill('CmbUom', rows, 'Id', 'UOMCode', { keep: '0', defaultRow: true, attrs: function (u) {
            return ' data-eq="' + esc(col(u, 'Equivalent')) + '" data-base="' + esc(col(u, 'BaseRateUom')) + '" data-base-pack="' + esc(col(u, 'BasePackUom')) + '"';
        } });
        if (keepText) setSelByText('CmbUom', keepText);
    }
    function uomEquivalent() { var sel = $('CmbUom'); var o = sel && sel.selectedOptions[0]; return o && sel.value !== '0' ? toD(o.getAttribute('data-eq')) : 0; }
    function bindItems() {
        /* ItemDetailBind :921 - display member ItemName or ItemCode by radio. */
        var byName = $('rdSearchByName').checked;
        fill('CmbItemName', S.itemRows, 'Id', byName ? 'ItemName' : 'ItemCode', { defaultRow: true, activate: 1, attrs: function (r) { return ' data-item-code="' + esc(col(r, 'ItemCode')) + '" data-item-name="' + esc(col(r, 'ItemName')) + '"'; } });
        CmbItemName_Leave();
    }
    async function ItemDtFillDbCall(partyId) {
        /* :900 - USP_GetItemsFromSaleInvoiceAgainstPartyId(party, "95,99"). */
        S.itemRows = await api('/items?supplierId=' + toI(partyId));
    }
    function bindStatic() {
        fill('CmbVehicleType', S.lists.vehicleTypes, 'id', 'name', { noBlank: true });
        fill('CmbWareHouseName', S.lists.warehouses, 'id', 'name', { defaultRow: true });
        fill('CmbCropYear', S.lists.cropYears, 'id', 'name', { defaultRow: true });
        fill('CmbJobLot', S.lists.jobLots, 'id', 'name', { defaultRow: true });
        fill('CmbPackingType', S.lists.packingTypes, 'id', 'name', { defaultRow: true });
        fill('CmbCity', S.lists.cities, 'id', 'name', { defaultRow: true });
        fill('cmbSupplierNameHistory', S.lists.historySuppliers, 'id', 'name');
        fill('gdnCustomer', S.lists.suppliers, 'id', 'name');
    }
    function applyLookups(d) {
        ['suppliers', 'transporters', 'vehicleTypes', 'warehouses', 'cropYears', 'jobLots', 'packingTypes', 'cities', 'uoms',
            'emptyBagTypes', 'emptyBagItems', 'bagConditions', 'gatePasses', 'historySuppliers'].forEach(function (k) { S.lists[k] = d[k] || []; });
        S.configs = d.configs || {};
        S.rights = d.rights || {};
        S.subsidiary = !!d.subsidiaryAccounts;
        S.wagesActive = !!d.wagesActive;
    }

    /* GetConfigurationsFromGlobalAndBindValuesInColumns :692. */
    function GetConfigurationsFromGlobalAndBindValuesInColumns() {
        var job = cfgInt('Job/Lot'); if (job !== 0) setSel('CmbJobLot', job);
        var crop = cfgInt('Default Crop Year'); if (crop !== 0) setSel('CmbCropYear', crop);
        var pack = cfgInt('Paking Type'); if (pack !== 0) setSel('CmbPackingType', pack);
        var wh = cfgInt('Warehouse'); if (wh !== 0) setSel('CmbWareHouseName', wh);
        if (S.subsidiary) return;
        if (S.FreightOnCash > 0) { var t = cfgInt('FreightInwardAc'); if (t !== 0) setSel('CmbTransport', t); }
        else $('CmbTransport').disabled = false;
        if (S.FreightId > 0) { $('CmbTransport').disabled = true; $('txtcarramount').disabled = true; }
        else { $('txtcarramount').disabled = false; $('CmbTransport').disabled = false; }
    }
    /* TransporterAccountDisable :1027 (FreightInwardAc read from the configuration list). */
    function TransporterAccountDisable() {
        if (S.subsidiary) return;
        if (S.FreightOnCash > 0) { if (S.configs['FreightInwardAc'] !== undefined && S.configs['FreightInwardAc'] !== '') setSel('CmbTransport', toI(S.configs['FreightInwardAc'])); }
        else $('CmbTransport').disabled = false;
        if (S.FreightId > 0) { $('CmbTransport').disabled = true; $('txtcarramount').disabled = true; }
        else { $('txtcarramount').disabled = false; $('CmbTransport').disabled = false; }
    }

    /* ------------------------------------------------------------------ calculations */
    function TotalSupplierWeight() {   /* :1849 */
        if (val('txtsuppwt') !== '' && val('txtfctwt') !== '') setText('txtDiffWeight', cs(toD(val('txtsuppwt')) - toD(val('txtfctwt'))));
    }
    function TotalWeight() {           /* :2024 */
        var packUom = uomEquivalent(), qty = val('txtQty') !== '' ? toD(val('txtQty')) : 0;
        setText('txtGrossWeight', cs(packUom * qty));
    }
    function Total() {                 /* :1876 */
        var qty = toD(val('txtQty')), gross = 0, ebUnit = 0, ebTotal, wtCutTotal = 0, addLess, fact = 0, supp = 0;
        if (val('txtfctwt') !== '') fact = toD(val('txtfctwt'));
        if (val('txtsuppwt') !== '') supp = toD(val('txtsuppwt'));
        if (val('txtGrossWeight') !== '') gross = toD(val('txtGrossWeight'));
        if (val('txtebu') !== '') ebUnit = toD(val('txtebu'));
        var purQty = 0, grdQty, extra = 0, status = S.EmptyBagsWeightCutStatus || 0;
        ebTotal = toD(val('txtebt'));
        if (ebUnit > 0 || ebTotal > 0) {
            var tag = activeTag();
            if (tag === 'EbUnit') { ebTotal = ebUnit * qty; setText('txtebt', cs(round(ebTotal, 2))); }
            else if (tag === 'EbTotal') { ebUnit = ebTotal / qty; setText('txtebu', cs(round(ebUnit, 3))); }
            else { ebTotal = (ebUnit > 0 && qty > 0) ? ebUnit * qty : 0; setText('txtebt', cs(round(ebTotal, 2))); }
        }
        if (ebUnit > 0) {
            status = 0;
            grdQty = S.dtdetail.reduce(function (s, r) { return s + toD(r.Qty); }, 0);
            S.dtEmptyBag.forEach(function (b) { if (toI(b.Type) === 2) purQty += toD(b.PurQty); });
            if (purQty > 0) {
                var updQty = !$('btnUpdateDetail').hidden && S.dtdetail[S.updateDetailIndex] ? toD(S.dtdetail[S.updateDetailIndex].Qty) : 0;
                status = 2;
                extra = (Math.abs(purQty) - (grdQty - updQty)) * ebUnit;
            }
        }
        S.EmptyBagsWeightCutStatus = status;
        if (val('txtwtcut') !== '') setText('txtwtcuttotal', cs(qty * toD(val('txtwtcut'))));
        else setText('txtwtcuttotal', '0');
        if (val('txtwtcuttotal') !== '') wtCutTotal = toD(val('txtwtcuttotal'));
        addLess = val('txtadlswt') !== '' ? toD(val('txtadlswt')) : 0;
        var w = 0;
        if (status === 2 && extra > 0) w = gross - wtCutTotal - ebTotal - addLess + extra;
        else if (status === 2) w = gross - wtCutTotal - addLess;
        else w = gross - ebTotal - wtCutTotal - addLess;
        setText('txtNetWeight', cs(round(w, 2)));
        if (!(supp > 0) || !(fact > 0) || !(gross > 0)) return;
        var term = val('txtDeliverTerm');
        if (term === 'Load' || term === 'Load & PartyWeight' || term === 'Ponch & PartyWeight') setText('txtStockWeight', cs(round(fact * gross / supp - ebTotal, 2)));
        if (term === 'Ponch') setText('txtStockWeight', cs(round((supp > fact ? fact * gross / fact : fact * gross / supp) - ebTotal, 2)));
        if (term === 'Ponch & FactoryWeight' || term === 'Load & FactoryWeight') setText('txtStockWeight', cs(round(fact * gross / fact - ebTotal, 2)));
    }
    /* CalculationPurchaseAgainstWeight :2110 - re-derives every detail row from the empty-bag purchase qty. */
    function CalculationPurchaseAgainstWeight() {
        var fact = val('txtfctwt') !== '' ? toD(val('txtfctwt')) : 0, supp = val('txtsuppwt') !== '' ? toD(val('txtsuppwt')) : 0;
        var allocate = 0, pur = 0, lessQty = 0, term = val('txtDeliverTerm');
        S.dtEmptyBag.forEach(function (b) { if (toI(b.Type) === 2) pur += toD(b.PurQty); });
        S.dtdetail.forEach(function (r) {
            var extra = 0, qty = toD(r.Qty), gross = toD(r.GrossWight), ebUnit = toD(r.EbUnit), wtCut = toD(r.WtCut), addLess = toD(r.AddLesswt), ebTotal, wtTotal;
            if (ebUnit > 0) {
                var grdQty = qty;
                if (pur > 0) {
                    pur -= allocate;
                    if (pur > grdQty) lessQty = grdQty; else if (grdQty > pur) lessQty = pur;
                    if (pur === grdQty) lessQty = grdQty;
                    if (pur > 0) extra = lessQty * ebUnit;
                }
            }
            if (ebUnit > 0 && qty > 0) { r.EbTotal = ebUnit * qty; ebTotal = toD(r.EbTotal); } else { r.EbTotal = 0; ebTotal = 0; }
            if (wtCut > 0 && qty > 0) { r.WtCutTotal = toD(cs(qty * wtCut)); wtTotal = r.WtCutTotal; } else { r.WtCutTotal = 0; wtTotal = 0; }
            var w = gross - wtTotal - ebTotal - addLess + extra;
            r.NetWeight = round(w, 2);
            if (supp > 0 && fact > 0 && gross > 0) {
                if (term === 'Load & FactoryWeight' || term === 'Ponch & FactoryWeight') r.StockWeight = round(fact * gross / fact - ebTotal, 2);
                else if (term === 'Ponch') r.StockWeight = round((supp > fact ? fact * gross / fact : fact * gross / supp) - ebTotal, 2);
                else r.StockWeight = round(fact * gross / supp - ebTotal, 2);
            } else r.StockWeight = round(w, 2);
            allocate = lessQty;
        });
        renderDetail();
    }

    /* ------------------------------------------------------------------ Leave / ValueChanged handlers */
    function CmbItemName_Leave() {     /* :1065 */
        bindUom(toI(val('CmbItemName')));
        var supp = toD(val('txtsuppwt'));
        setText('txtMoisture', '0');
        if (val('txtDeliverTerm') === 'Load') setText('txtGrnWeight', cs(supp));
        var grdGross = S.dtdetail.reduce(function (s, r) { return s + toD(r.GrossWight); }, 0);
        if (!$('btnUpdateDetail').hidden && S.dtdetail[S.updateDetailIndex]) grdGross -= toD(S.dtdetail[S.updateDetailIndex].GrossWight);
        setText('txtGrossWeight', cs(supp - grdGross));
    }
    async function CmbSupplierName_Leave() {   /* :1105 */
        var party = toI(val('CmbSupplierName'));
        if (party > 0) { await ItemDtFillDbCall(party); bindItems(); }
    }
    function supplierEnabled() { $('CmbSupplierName').disabled = S.dtdetail.length !== 0; }

    /* ------------------------------------------------------------------ detail grid (grd) */
    function anyGdn() { return S.dtdetail.some(function (r) { return toI(r.GdnId) > 0; }); }
    var GRD_COMBOS = {
        WareHouseId: { list: 'warehouses', caption: 'WareHouse Name' }, CropYearId: { list: 'cropYears', caption: 'Crop Year' },
        JobId: { list: 'jobLots', caption: 'Job Lot' }, PackingTypeId: { list: 'packingTypes', caption: 'Packing Type' }, CityId: { list: 'cities', caption: 'City Name' }
    };
    var GRD_EDIT = ['Qty', 'GrossWight', 'EbUnit', 'WtCut', 'AddLesswt', 'LabNo'];
    function grdColumns() {
        /* dtdetail order :505-530; hidden :2383; GdnNo only when GDN rows exist :2414; Delete/Add buttons at 0/1 :2423. */
        var cols = ['GdnNo', 'WareHouseId', 'ItemCode', 'ItemName', 'CropYearId', 'JobId', 'PackingTypeId', 'UOM', 'Qty', 'GrossWight', 'EbUnit', 'EbTotal', 'WtCut', 'WtCutTotal', 'AddLesswt', 'NetWeight', 'StockWeight', 'LabNo', 'CityId'];
        return anyGdn() ? cols : cols.slice(1);
    }
    function comboCell(key, r, i, locked) {
        var spec = GRD_COMBOS[key], list = S.lists[spec.list] || [];
        var html = '<select class="cell" data-dtcombo-skip data-row="' + i + '" data-key="' + key + '"' + (locked ? ' disabled' : '') + '><option value="0"></option>';
        list.forEach(function (x) { var v = String(col(x, 'id')); html += '<option value="' + esc(v) + '"' + (v === String(r[key]) ? ' selected' : '') + '>' + esc(col(x, 'name')) + '</option>'; });
        return html + '</select>';
    }
    function renderDetail() {
        var gdn = anyGdn(), cols = grdColumns();
        var head = '<thead><tr><th>X</th><th>+</th>' + cols.map(function (c) { return '<th>' + esc(GRD_COMBOS[c] ? GRD_COMBOS[c].caption : cap(c)) + '</th>'; }).join('') + '</tr></thead>';
        var body = S.dtdetail.map(function (r, i) {
            return '<tr data-row="' + i + '"' + (i === S.curDetail ? ' class="cur"' : '') + '><td class="btncell"><button type="button" data-act="del" data-row="' + i + '">X</button></td><td class="btncell"><button type="button" data-act="add" data-row="' + i + '">+</button></td>' +
                cols.map(function (c) {
                    if (GRD_COMBOS[c]) return '<td>' + comboCell(c, r, i, gdn && c !== 'CityId') + '</td>';
                    if (GRD_EDIT.indexOf(c) >= 0) return '<td><input class="cell' + (c === 'LabNo' ? ' txt' : '') + '" data-row="' + i + '" data-key="' + c + '" value="' + esc(c === 'LabNo' ? r[c] : cs(r[c])) + '"></td>';
                    var v = r[c]; var num = typeof v === 'number';
                    return '<td' + (num ? ' class="n"' : '') + '>' + esc(num ? cs(v) : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        /* grdSettings :2379 - the Janus total row is always shown (0 on an empty grid, desktop screenshot). */
        var SUMS = ['Qty', 'GrossWight', 'EbUnit', 'EbTotal', 'WtCut', 'WtCutTotal', 'AddLesswt', 'NetWeight', 'StockWeight'];
        var foot = '<tfoot><tr><td></td><td></td>' + cols.map(function (c) {
            return SUMS.indexOf(c) >= 0 ? '<td class="n">' + esc(S.dtdetail.length ? cs(S.dtdetail.reduce(function (t, r) { return t + toD(r[c]); }, 0)) : '0') + '</td>' : '<td></td>';
        }).join('') + '</tr></tfoot>';
        $('grd').innerHTML = head + '<tbody>' + (body || '') + '</tbody>' + foot;
        if ($('grdNav')) $('grdNav').textContent = (S.dtdetail.length ? Math.max(S.curDetail, 0) + 1 : 0) + ' Of ' + S.dtdetail.length;
    }
    /* grdSettings :2379 */
    function grdSettings() {
        var gdn = anyGdn();
        $('panel5').hidden = gdn;
        renderDetail();
        supplierEnabled();
    }
    /* grd_CellUpdated :2501 - note AddLesswt is ADDED here although Total() subtracts it (desktop). */
    function grdCellUpdated(i, key) {
        var r = S.dtdetail[i]; if (!r) return;
        var qty = toD(r.Qty), gross, ebT, ebU, wtC, wtT, add;
        if (key === 'Qty') {
            gross = qty * toD(r.UomEquivalent); r.GrossWight = gross;
            ebT = toD(r.EbTotal); ebU = toD(r.EbUnit);
            if (ebT === 0 && ebU > 0) { ebT = qty * ebU; r.EbTotal = ebT; } else if (ebU === 0 && ebT > 0) { ebU = ebT / qty; r.EbUnit = ebU; }
            wtC = toD(r.WtCut); wtT = toD(r.WtCutTotal);
            if (wtT === 0 && wtC > 0) { wtT = qty * wtC; r.WtCutTotal = wtT; } else if (wtC === 0 && wtT > 0) { wtC = wtT / qty; r.WtCut = wtC; }
            add = toD(r.AddLesswt);
            r.NetWeight = gross - ebT - wtT + add; r.StockWeight = gross - ebT + add;
        }
        if (key === 'GrossWight' || key === 'AddLesswt' || key === 'EbTotal') {
            gross = toD(r.GrossWight); ebT = toD(r.EbTotal); add = toD(r.AddLesswt); wtT = toD(r.WtCutTotal);
            r.NetWeight = gross - ebT - wtT + add; r.StockWeight = gross - ebT + add;
        }
        if (key === 'EbUnit') {
            gross = toD(r.GrossWight); ebT = qty * toD(r.EbUnit); r.EbTotal = ebT; wtT = toD(r.WtCutTotal); add = toD(r.AddLesswt);
            r.NetWeight = gross - ebT - wtT + add; r.StockWeight = gross - ebT + add;
        }
        if (key === 'WtCut') {
            gross = toD(r.GrossWight); ebT = toD(r.EbTotal); wtT = qty * toD(r.WtCut); r.WtCutTotal = wtT; add = toD(r.AddLesswt);
            r.NetWeight = gross - ebT - wtT + add; r.StockWeight = gross - ebT + add;
        }
    }
    function DeleteRowInDetailGrid(i) {      /* :2451 */
        if (S.updateDetailIndex !== -1) { msg('Reset Detail first...'); return; }
        var r = S.dtdetail[i]; if (!r) return;
        if (toI(r.Id) > 0 && !global.confirm('Are you sure to Delete?')) return;
        S.dtdetail.splice(i, 1); S.curDetail = -1;
        GetDeliveryScheduleDetailIds();
        supplierEnabled();
        $('panel5').hidden = S.dtdetail.length !== 0;   /* :2473 - the entry panel hides while rows remain (desktop). */
        renderDetail();
    }
    function AddRowInDetailGrid(i) {         /* :2484 */
        var r = S.dtdetail[i]; if (!r) return;
        var copy = Object.assign({}, r);
        if (!$('btnUpdate').hidden && !$('btnUpdate').disabled) copy.Id = 0;
        S.dtdetail.push(copy);
        renderDetail();
    }
    function grd_DoubleClick() {             /* :2278 */
        var r = S.dtdetail[S.curDetail];
        if (!r || anyGdn()) return;
        S.updateDetailIndex = S.curDetail;
        setSel('CmbWareHouseName', r.WareHouseId); setSel('CmbItemName', r.ItemId); setSel('CmbCropYear', r.CropYearId);
        setSel('CmbJobLot', r.JobId); setSel('CmbPackingType', r.PackingTypeId);
        CmbItemName_Leave();
        setSel('CmbUom', r.UOMId);
        setText('txtQty', cs(r.Qty)); setText('txtGrossWeight', cs(r.GrossWight)); setText('txtebu', cs(r.EbUnit)); setText('txtebt', cs(r.EbTotal));
        setText('txtwtcut', cs(r.WtCut)); setText('txtwtcuttotal', cs(r.WtCutTotal)); setText('txtadlswt', cs(r.AddLesswt));
        setText('txtNetWeight', cs(r.NetWeight)); setText('txtStockWeight', cs(r.StockWeight)); setText('txtLabNo', r.LabNo || '');
        setSel('CmbCity', r.CityId);
        $('Add').hidden = true; $('btnUpdateDetail').hidden = false; $('btnCancelUpdateDetial').hidden = false;
    }
    function FormValidationDetail() {        /* :1157 */
        var checks = [['CmbWareHouseName', 'Warehouse Field is Required'], ['CmbItemName', 'Item Name Field is Required'], ['CmbCropYear', 'Crop Year Field is Required'],
            ['CmbJobLot', 'Job/Lot Field is Required'], ['CmbPackingType', 'Packing Type Field is Required']];
        for (var i = 0; i < checks.length; i++) if (toI(val(checks[i][0])) === 0) { msg(checks[i][1]); $(checks[i][0]).focus(); return false; }
        if (val('txtQty').trim() === '' || toD(val('txtQty')) === 0) { msg('Qty Field is Required'); $('txtQty').focus(); return false; }
        if (toI(val('CmbUom')) === 0) { msg('Pack Unit Field is Required'); return false; }
        if (val('txtGrossWeight').trim() === '' || toD(val('txtGrossWeight')) === 0) { msg('Gross Weight Field is Required'); $('txtGrossWeight').focus(); return false; }
        if (val('txtNetWeight').trim() === '' || toD(val('txtNetWeight')) === 0) { msg('Net Bill Weight Field is Required'); return false; }
        if (val('txtStockWeight').trim() === '' || toD(val('txtStockWeight')) === 0) { msg('Stock Weight Field is Required'); return false; }
        if (toI(val('CmbCity')) === 0) { msg('City Field is Required'); return false; }
        return true;
    }
    function detailFromPanel(base) {
        var item = $('CmbItemName').selectedOptions[0];
        return Object.assign(base || { Id: 0, GdnId: 0, GdnDetailId: 0, GdnDocumentTypeId: 0, GdnNo: 0 }, {
            WareHouseId: toI(val('CmbWareHouseName')), ItemId: toI(val('CmbItemName')),
            ItemCode: item ? item.getAttribute('data-item-code') : '', ItemName: item ? item.getAttribute('data-item-name') : '',
            CropYearId: toI(val('CmbCropYear')), JobId: toI(val('CmbJobLot')), PackingTypeId: toI(val('CmbPackingType')),
            UOMId: toI(val('CmbUom')), UOM: selText('CmbUom').trim(), UomEquivalent: uomEquivalent(),
            Qty: toD(val('txtQty')), GrossWight: toD(val('txtGrossWeight')), EbUnit: toD(val('txtebu')), EbTotal: toD(val('txtebt')),
            WtCut: toD(val('txtwtcut')), WtCutTotal: toD(val('txtwtcuttotal')), AddLesswt: toD(val('txtadlswt')),
            NetWeight: toD(val('txtNetWeight')), StockWeight: toD(val('txtStockWeight')), LabNo: val('txtLabNo'), CityId: toI(val('CmbCity'))
        });
    }
    function Add_Click() {                   /* :2245 */
        if (toI(val('CmbGpNo')) === 0) { msg('Please Load Any GatePass First...'); return; }
        if (S.dtdetail.length > 0 && anyGdn()) { msg('You can not add manual Record because record against Grn exist in Grid'); return; }
        if (!FormValidationDetail()) return;
        if (toD(val('txtNetWeight')) > toD(val('txtGrossWeight'))) { msg('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        S.dtdetail.push(detailFromPanel());
        grdSettings();
        ResetDetail();
    }
    function btnUpdateDetail_Click() {       /* :2316 - compares Net with txtGrnWeight, not the row's gross (desktop). */
        if (S.dtdetail.length > 0 && anyGdn()) { msg('You can not update manual Record because record against Grn exist in Grid'); return; }
        if (!FormValidationDetail()) return;
        if (toD(val('txtNetWeight')) > toD(val('txtGrnWeight'))) { msg('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        var r = S.dtdetail[S.updateDetailIndex]; if (!r) return;
        detailFromPanel(r);
        $('Add').hidden = false; $('btnUpdateDetail').hidden = true; $('btnCancelUpdateDetial').hidden = true;
        ResetDetail();
        supplierEnabled();
        renderDetail();
    }
    function ResetDetail() {                 /* :1768 */
        ['txtQty', 'txtGrossWeight', 'txtebu', 'txtebt', 'txtadlswt', 'txtNetWeight', 'txtStockWeight', 'txtLabNo'].forEach(function (id) { setText(id, ''); });
        setSel('CmbUom', 0);
        S.updateDetailIndex = -1;
        $('Add').hidden = false; $('btnCancelUpdateDetial').hidden = true; $('btnUpdateDetail').hidden = true;
        supplierEnabled();
        renderDetail();
    }
    function GetDeliveryScheduleDetailIds() { /* :4077 */
        S.GdnDetailIds = S.dtdetail.map(function (r) { return ',' + toI(r.GdnDetailId); }).join('');
    }

    /* ------------------------------------------------------------------ empty bags grid */
    function AddRowInvEmptyBagsGrid() { S.dtEmptyBag.push({ OrderId: 0, Type: 0, ItemId: 0, Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' }); }
    function grdEmptyBagsSettings() { S.emptyBagsPlus = S.dtEmptyBag.length === 1; renderEmptyBags(); }  /* :2694 */
    function bagCombo(key, listName, r, i) {
        var html = '<select class="cell" data-dtcombo-skip data-bag="' + i + '" data-key="' + key + '"><option value="0"></option>';
        (S.lists[listName] || []).forEach(function (x) { var v = String(col(x, 'id')); html += '<option value="' + esc(v) + '"' + (v === String(r[key]) ? ' selected' : '') + '>' + esc(col(x, 'name')) + '</option>'; });
        return html + '</select>';
    }
    function renderEmptyBags() {
        var head = '<thead><tr>' + (S.emptyBagsPlus ? '<th>+</th>' : '') + '<th>EmptyBagsType</th><th>Item Name</th><th>Bags_Condition</th><th>Rec Qty</th><th>Pur Qty</th><th>Remarks</th></tr></thead>';
        var body = S.dtEmptyBag.map(function (r, i) {
            return '<tr>' + (S.emptyBagsPlus ? '<td class="btncell"><button type="button" data-bagadd="' + i + '">+</button></td>' : '') +
                '<td>' + bagCombo('Type', 'emptyBagTypes', r, i) + '</td><td>' + bagCombo('ItemId', 'emptyBagItems', r, i) + '</td><td>' + bagCombo('Condition', 'bagConditions', r, i) + '</td>' +
                '<td><input class="cell" data-bag="' + i + '" data-key="RecQty" value="' + esc(cs(r.RecQty)) + '"></td>' +
                '<td><input class="cell" data-bag="' + i + '" data-key="PurQty" value="' + esc(cs(r.PurQty)) + '"></td>' +
                '<td><input class="cell txt" data-bag="' + i + '" data-key="Remarks" value="' + esc(r.Remarks) + '" style="width:160px"></td></tr>';
        }).join('');
        $('grdEmptyBags').innerHTML = head + '<tbody>' + body + '</tbody>';
    }
    function grdEmptyBagsCellUpdated(i, key) {  /* :2732 */
        var r = S.dtEmptyBag[i]; if (!r) return;
        if (toI(r.PurQty) < 0) { r.PurQty = 0; msg('PurQty cannot be less than Zero'); }
        if (toI(r.RecQty) < 0) { r.RecQty = 0; msg('RecQty cannot be less than Zero'); }
        if (key === 'PurQty' || key === 'Type') CalculationPurchaseAgainstWeight();
        if ((toI(r.Type) === 4 || toI(r.Type) === 5) && key === 'PurQty') { r.PurQty = 0; msg('Retained or Returned Stock you cannot be purchase'); }
        if (toI(r.Type) === 2 && key === 'RecQty') { r.RecQty = 0; msg('You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight'); }
        renderEmptyBags();
    }

    /* ------------------------------------------------------------------ pending gate passes (grdGp) */
    var GP_COLS = ['GpSrNo', 'Status', 'GpDate', 'OrderType', 'SupplierName', 'VehicleNo', 'BiltyNo', 'VarietyName', 'Qty', 'SupplierWeight', 'FactoryWeight', 'VehicleType', 'NetPaid', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser'];
    var GP_SUM = ['Qty', 'SupplierWeight', 'FactoryWeight', 'NetPaid'];
    function GatepassGridFill(rows) {        /* :2801 / GRNGridSetting :2877 */
        S.gpRows = rows || [];
        if (!S.gpRows.length) { $('grdGp').innerHTML = ''; FillCountFromGpGrid(); return; }
        var head = '<thead><tr><th>Load</th>' + GP_COLS.map(function (c) { return '<th>' + cap(c) + '</th>'; }).join('') + '</tr></thead>';
        var body = S.gpRows.map(function (r, i) {
            return '<tr data-gp="' + i + '"' + (i === S.curGp ? ' class="cur"' : '') + '><td class="btncell"><button type="button" data-gpload="' + i + '">Load</button></td>' + GP_COLS.map(function (c) {
                var v = col(r, c);
                if (c === 'GpSrNo') return '<td><a class="lnk" data-gplink="' + i + '">' + esc(v) + '</a></td>';
                if (GP_SUM.indexOf(c) >= 0) return '<td class="n">' + esc(fmt3(v)) + '</td>';
                if (c === 'GpDate') return '<td>' + esc(dateText(v)) + '</td>';
                if (c === 'EntryDate' || c === 'ModifyDate') return '<td>' + esc(dateTimeText(v)) + '</td>';
                return '<td>' + esc(v) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var foot = '<tfoot><tr><td></td>' + GP_COLS.map(function (c) {
            return GP_SUM.indexOf(c) >= 0 ? '<td class="n">' + esc(fmt3(S.gpRows.reduce(function (s, r) { return s + toD(col(r, c)); }, 0))) + '</td>' : '<td></td>';
        }).join('') + '</tr></tfoot>';
        $('grdGp').innerHTML = head + '<tbody>' + body + '</tbody>' + foot;
        FillCountFromGpGrid();
    }
    function FillCountFromGpGrid() {         /* :2849 */
        var open = 0, acc = 0;
        S.gpRows.forEach(function (r) { var s = String(col(r, 'Status') || ''); if (s === 'Accepted') acc++; else if (s === 'Open') open++; });
        $('txtAcceptedGpTotal').textContent = acc; $('txtOpenGpTotal').textContent = open;
    }
    function GpNoBind(id, no) {              /* :3027 */
        $('CmbGpNo').innerHTML = '<option value="' + esc(id) + '">' + esc(no) + '</option>';
        $('CmbGpNo').value = String(id);
    }
    async function LoadClickInGpGrid(i) {    /* :3043 */
        var r = S.gpRows[i]; if (!r) return;
        if (!saveVisible() || !saveEnabled()) { msg("Can't Load Gp in Update Mode...Please Reset The Form First.."); return; }
        if (toI(val('CmbGpNo')) !== 0) { msg('Another Gp Is Already Loaded...Please Reset The Form First..'); return; }
        S.FreightOnCash = toI(col(r, 'FreightOnCash'));
        S.FreightId = toI(col(r, 'FreightId'));
        $('txtsuppwt').readOnly = true;
        if (String(col(r, 'Status') || '').toLowerCase() !== 'accepted') { msg('Status Not Accepted. Please check status'); return; }
        GpNoBind(toI(col(r, 'Id')), String(col(r, 'GpSrNo')));
        await combgatepass_Leave();
    }
    async function combgatepass_Leave() {   /* :1090 */
        if (saveVisible() && saveEnabled()) await GatePassRecordFill();
    }
    async function GatePassRecordFill() {   /* :2955 */
        TransporterAccountDisable();
        var d = await api('/gate-passes/' + toI(val('CmbGpNo')));
        if (d && Object.keys(d).length) {
            $('CmbVehicleType').disabled = true; $('txtvehno').disabled = true; $('txtbltyno').disabled = true;
            setSelByText('CmbVehicleType', col(d, 'VehicleType'));
            setText('txtvehno', col(d, 'VehicleNo') || ''); setText('txtbltyno', col(d, 'BiltyNo') || '');
            setText('txtremarks', col(d, 'OtherRemarks') || ''); setText('txtcarramount', col(d, 'NetPaid') === null || col(d, 'NetPaid') === undefined ? '' : cs(col(d, 'NetPaid')));
            setText('txtQty', col(d, 'NoOfPackages') === null || col(d, 'NoOfPackages') === undefined ? '' : cs(col(d, 'NoOfPackages')));
            setText('txtsuppwt', fmt3(col(d, 'SupplierWeight'))); setText('txtfctwt', fmt3(col(d, 'FactoryWeight')));
            var diff = toD(col(d, 'DifferenceWeight'));
            setText('txtDiffWeight', fmt3(diff));
            $('lblDiffWeightDescription').textContent = diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal';
            setSelByText('CmbWareHouseName', col(d, 'WareHouseName'));
            setText('txtAnaylstName', col(d, 'AnalystName') || '');
            var ded = col(d, 'DeductionWeight'); ded = ded === null || ded === undefined ? '' : cs(ded);
            setText('txtDeductionKg', ded); setText('txtwtcut', ded);
            setText('txtTicketnos', col(d, 'TicketNos') || '');
            setSel('CmbSupplierName', toI(col(d, 'SupplierCustomerId')));
            await CmbSupplierName_Leave();
            setText('txtGrnWeight', fmt3(col(d, 'FactoryWeight')));
            setText('txtGrossWeight', fmt3(col(d, 'FactoryWeight')));
        } else {
            $('CmbVehicleType').disabled = false; $('txtvehno').disabled = false; $('txtbltyno').disabled = false;
        }
    }

    /* ------------------------------------------------------------------ reset / new / load */
    async function reset() {                 /* :1694 */
        $('txtsuppwt').readOnly = true;
        $('CmbTransport').disabled = false; $('txtcarramount').disabled = false;
        S.RecId = 0;
        if (AT) AT.reset();
        $('CmbGpNo').innerHTML = '';
        S.EmptyBagsWeightCutStatus = 0;
        ['txtGrnWeight', 'txtTicketnos', 'txtAnaylstName', 'txtDeductionKg', 'txtcarramount', 'txtsuppwt', 'txtfctwt', 'txtDiffWeight', 'txtvehno', 'txtbltyno', 'txtremarks',
            'txtQty', 'txtebu', 'txtebt', 'txtadlswt', 'txtNetWeight', 'txtStockWeight', 'txtLabNo', 'txtwtcut', 'txtGrossWeight'].forEach(function (id) { setText(id, ''); });
        setSel('CmbSupplierName', 0); setSel('CmbTransport', 0); setSel('CmbCity', 0);
        var vt = $('CmbVehicleType'); if (vt.options.length) vt.selectedIndex = 0;
        $('CmbUom').innerHTML = '<option value="0"></option>';
        S.dtdetail = []; S.curDetail = -1;
        S.dtEmptyBag = []; AddRowInvEmptyBagsGrid(); grdEmptyBagsSettings();
        S.itemRows = []; $('CmbItemName').innerHTML = '<option value="0"></option>';
        $('btnUpdateDetail').hidden = true; $('btnCancelUpdateDetial').hidden = true; $('Add').hidden = false;
        $('btnSave').hidden = false; $('btnUpdate').hidden = true;
        $('txtGrossWeight').disabled = false;
        var next = await api('/next-code');
        setText('txtdocno', next.docNo);
        GatepassGridFill(await api('/pending-gate-passes'));
        GetConfigurationsFromGlobalAndBindValuesInColumns();
        S.GdnDetailIds = '';
        $('panel5').hidden = false;
        renderDetail();
        supplierEnabled();
        $('DocDate').focus();
    }
    async function ReadById(id) {            /* :1592 */
        await reset();
        S.RecId = toI(id);
        var g = await api('/' + S.RecId);
        if (!g) return;
        $('btnSave').hidden = true; $('btnUpdate').hidden = false;
        selectTab(0);
        setText('txtdocno', col(g, 'DocNo'));
        $('DocDate').value = dateText(col(g, 'DocDate'));
        setSel('CmbSupplierName', toI(col(g, 'SupplierCustomerId')));
        await CmbSupplierName_Leave();
        if (toI(col(g, 'TransporterId')) > 0) setSel('CmbTransport', toI(col(g, 'TransporterId')));
        setText('txtcarramount', cs(col(g, 'CarriageAmount')));
        setText('txtsuppwt', cs(col(g, 'PartyWeight'))); setText('txtfctwt', cs(col(g, 'FactoryWeight')));
        setSelByText('CmbVehicleType', col(g, 'VehicleType'));
        setText('txtvehno', col(g, 'VehicleNo') || ''); setText('txtbltyno', col(g, 'BiltyNo') || '');
        setText('txtremarks', col(g, 'RemarksHeader') || ''); setText('txtTicketnos', col(g, 'TicketNos') || '');
        setText('txtDeliverTerm', col(g, 'DeliveryTerm') || '');
        GpNoBind(toI(col(g, 'InwardGatePassId')), String(col(g, 'GpNo') === null || col(g, 'GpNo') === undefined ? '' : col(g, 'GpNo')));
        S.FreightId = toI(col(g, 'FreightId')); S.FreightOnCash = S.FreightId;
        TransporterAccountDisable();
        S.dtdetail = (col(g, 'details') || []).map(detailFromRecord);
        grdSettings();
        setText('txtGrnWeight', fmt2blank(S.dtdetail.reduce(function (s, r) { return s + toD(r.GrossWight); }, 0)));
        S.dtEmptyBag = (col(g, 'emptyBags') || []).map(function (b) {
            return { OrderId: toI(col(b, 'PurchaseOrderId')), Type: toI(col(b, 'TypeId')), ItemId: toI(col(b, 'ItemId')), Condition: toI(col(b, 'BagsCondition')),
                RecQty: toD(col(b, 'ReceivedQty')), PurQty: toD(col(b, 'PurchaseQty')), Remarks: col(b, 'Remarks') || '' };
        });
        if (!S.dtEmptyBag.length) AddRowInvEmptyBagsGrid();
        grdEmptyBagsSettings();
        global.history.replaceState(null, '', '/purchase/grn-sale-return?id=' + S.RecId);
    }
    /* FilldtDetailFromListCommonForReadById :1658 (InvGrnDetail from ReadByInvGrnID). */
    function detailFromRecord(d) {
        return {
            Id: toI(col(d, 'Id')), GdnId: toI(col(d, 'GdnId')), GdnDetailId: toI(col(d, 'GdnDetailId')), GdnDocumentTypeId: toI(col(d, 'GdnDocumentTypeId')),
            GdnNo: toI(col(d, 'GdnNo')), WareHouseId: toI(col(d, 'WarehouseId')), ItemId: toI(col(d, 'ItemId')), ItemCode: col(d, 'ItemCode') || '',
            ItemName: col(d, 'Item') || col(d, 'ItemName') || '', CropYearId: toI(col(d, 'CropYearId')), JobId: toI(col(d, 'JobLotId')),
            PackingTypeId: toI(col(d, 'PackingTypeId')), UOMId: toI(col(d, 'ItemUomId')), UOM: col(d, 'UOMCode') || '', UomEquivalent: toD(col(d, 'UOM')),
            Qty: toD(col(d, 'ItemQty')), GrossWight: toD(col(d, 'GrossWeight')), EbUnit: toD(col(d, 'EBWPerUnit')), EbTotal: toD(col(d, 'EBWTotal')),
            WtCut: toD(col(d, 'WtCut')), WtCutTotal: toD(col(d, 'WtCutTotal')), AddLesswt: toD(col(d, 'AdLsWeight')), NetWeight: toD(col(d, 'NetBillWeight')),
            StockWeight: toD(col(d, 'StockWeight')), LabNo: col(d, 'LabReportRef') || '', CityId: toI(col(d, 'CityId')),
            /* history-detail extras (AddExtraColumns :1677) */
            WareHouseName: col(d, 'WareHouseCode') || '', CropYear: col(d, 'CropYear') || '', JobLot: col(d, 'JobLot') || '', PackingType: col(d, 'PackingType') || '', CityName: col(d, 'AreaCity') || ''
        };
    }

    /* ------------------------------------------------------------------ save / update / delete */
    function FormValidation() {              /* :1122 */
        var dn = val('txtdocno').trim();
        if (dn === '' || dn === '0') { msg('DocNo Field is Required'); return false; }
        if (toI(val('CmbSupplierName')) === 0) { msg('Supplier Field is Required'); return false; }
        if (toI(selText('CmbGpNo').trim()) === 0) { msg('Gatepass Field is Required'); return false; }
        if (toD(val('txtsuppwt')) === 0) { msg('Supplier Weight Field is Required'); return false; }
        if (toD(val('txtfctwt')) === 0) { msg('Factory Weight Field is Required'); return false; }
        return true;
    }
    async function Insert() {                /* :1228 */
        if (S.dtdetail.length === 0) { msg('Grid Record Not Found'); return; }
        if (!FormValidation()) return;
        if (!global.confirm(S.RecId > 0 ? 'Are you sure to Update' : 'Are you sure to Save')) return;
        if (toD(val('txtcarramount')) > 0 && toI(val('CmbTransport')) === 0) { msg('Transporter Account field required'); return; }
        var term = val('txtDeliverTerm').trim();
        var bill = 0, stock = 0;
        S.dtdetail.forEach(function (r) { bill += toD(r.NetWeight); stock += toD(r.StockWeight); });
        var tol = cfgDbl('BillWeightAndStockWeightDifferenceTolerance');
        if ((term === 'Load' || term === 'Load & PartyWeight' || term === 'Ponch & PartyWeight') && (bill - stock) > tol &&
            !global.confirm('Bill weight exceeds the stock weight by this tolerance ' + tol + ' are you sure to Save Or Update')) return;
        var tr = $('CmbTransport').selectedOptions[0];
        var body = {
            Id: S.RecId, DocNo: toI(val('txtdocno')), DocDate: val('DocDate'), SupplierCustomerId: toI(val('CmbSupplierName')),
            InwardGatePassId: toI(val('CmbGpNo')), DeliveryTerm: term, VehicleType: selText('CmbVehicleType'), VehicleNo: val('txtvehno').trim(),
            BiltyNo: val('txtbltyno').trim(), PartyWeight: toD(val('txtsuppwt')), FactoryWeight: toD(val('txtfctwt')), RemarksHeader: val('txtremarks').trim(),
            CarriageAmount: toD(val('txtcarramount')), TransporterId: toI(val('CmbTransport')), TransporterSupCustId: tr ? toI(tr.getAttribute('data-supcust')) : 0,
            GrnWeight: toD(val('txtGrnWeight')),
            details: S.dtdetail.map(function (r) {
                return { Id: S.RecId !== 0 ? toI(r.Id) : 0, GdnId: toI(r.GdnId), GdnDetailId: toI(r.GdnDetailId), GdnDocumentTypeId: toI(r.GdnDocumentTypeId),
                    WarehouseId: toI(r.WareHouseId), ItemId: toI(r.ItemId), CropYearId: toI(r.CropYearId), CropYear: lookupText(S.lists.cropYears, r.CropYearId, 'name'),
                    JobLotId: toI(r.JobId), PackingTypeId: toI(r.PackingTypeId), ItemUomId: toI(r.UOMId), ItemQty: toD(r.Qty), GrossWeight: toD(r.GrossWight),
                    EBWPerUnit: toD(r.EbUnit), EBWTotal: toD(r.EbTotal), WtCut: toD(r.WtCut), WtCutTotal: toD(r.WtCutTotal), AdLsWeight: toD(r.AddLesswt),
                    NetBillWeight: toD(r.NetWeight), StockWeight: toD(r.StockWeight), LabReportRef: r.LabNo || '', CityId: toI(r.CityId),
                    AreaCity: lookupText(S.lists.cities, r.CityId, 'name') };
            }),
            emptyBags: S.dtEmptyBag.map(function (b) {
                return { PurchaseOrderId: toI(b.OrderId), TypeId: toI(b.Type), ItemId: toI(b.ItemId), BagsCondition: toI(b.Condition), ReceivedQty: toD(b.RecQty), PurchaseQty: toD(b.PurQty), Remarks: b.Remarks };
            })
        };
        var attached = AT ? AT.payload() : undefined;
        if (attached) body.attachments = attached;
        var res = await post('/save', body);
        msg(res.message);
        /* FormHelper.ShowWagesBillIfNeeded (:1478) opens the contractor wages bill on the desktop; not available on the web yet. */
        await reset();
        if ($('ChkBox').checked) await GenerateReport(res.id);
    }
    async function btnSave_Click() { S.RecId = 0; await Insert(); }        /* :1527 */
    async function btnUpdate_Click() {                                      /* :1540 */
        if (S.RecId === 0) { msg('Record not update because Id not found'); return; }
        await Insert();
    }
    async function btnDelete_Click() {                                      /* :1556 */
        if (S.RecId > 0) {
            if (!global.confirm('Are you sure to Delete?')) return;
            var r = await api('/' + S.RecId, { method: 'DELETE' });
            msg(r.message);
            await reset();
            return;
        }
        msg('Record Not Found');
    }
    function GenerateReport(id) {            /* :3435 - CommonServices.GrnSlipWithSubReports -> 211 */
        return printPdf('/reports/print/211-goods-receipts-notes-rice-slip', { id: toI(id), documentTypeId: 143 });
    }

    /* ------------------------------------------------------------------ Load GDN (frmLoadGdnForGrnSaleReturn) */
    async function btnLoadGdn_Click() {      /* :3969 */
        if (toI(val('CmbGpNo')) === 0) { msg('Please Load Any GatePass First...'); return; }
        if (S.dtdetail.length > 0) {
            var cur = S.dtdetail[S.curDetail >= 0 ? S.curDetail : 0];
            if (toI(cur && cur.GdnId) === 0) { msg("You cannot load 'GDN' because record without 'GDN' exists in the grid."); return; }
        }
        S.gdnRows = [];
        $('grdGdnLoader').innerHTML = '';
        $('gdnFromDate').value = ''; $('gdnToDate').value = '';
        $('gdnLoader').hidden = false;
    }
    async function searchGdn() {
        var q = '?fromDate=' + encodeURIComponent(val('gdnFromDate')) + '&toDate=' + encodeURIComponent(val('gdnToDate')) + '&customerId=' + toI(val('gdnCustomer'));
        var used = S.GdnDetailIds.split(',').filter(Boolean).map(Number);
        S.gdnRows = (await api('/pending-gdn' + q)).filter(function (r) { return used.indexOf(toI(col(r, 'DetailId'))) < 0; });
        var cols = ['GdnNo', 'DocDate', 'CustomerName', 'GpNo', 'VehicleNo', 'BranchName', 'WareHouseName', 'ItemCode', 'ItemName', 'CropYear', 'JobLot', 'PackingType', 'ItemUom', 'ItemQty', 'NetBillWeight', 'UsedQty', 'BalQty', 'UsedWeight', 'BalWeight'];
        $('grdGdnLoader').innerHTML = '<thead><tr><th><input type="checkbox" id="gdnAll"></th>' + cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr></thead><tbody>' +
            S.gdnRows.map(function (r, i) {
                return '<tr><td><input type="checkbox" data-gdnpick="' + i + '"></td>' + cols.map(function (c) { var v = col(r, c); return c === 'DocDate' ? '<td>' + esc(dateText(v)) + '</td>' : '<td' + (typeof v === 'number' ? ' class="n"' : '') + '>' + esc(typeof v === 'number' ? cs(v) : v) + '</td>'; }).join('') + '</tr>';
            }).join('') + '</tbody>';
        var all = $('gdnAll'); if (all) all.onchange = function () { doc.querySelectorAll('[data-gdnpick]').forEach(function (c) { c.checked = all.checked; }); };
    }
    function closeGdnLoader() { $('gdnLoader').hidden = true; }
    function loadSelectedGdn() {
        var picked = Array.prototype.filter.call(doc.querySelectorAll('[data-gdnpick]'), function (c) { return c.checked; }).map(function (c) { return S.gdnRows[toI(c.getAttribute('data-gdnpick'))]; });
        closeGdnLoader();
        LoadInGridDetailFromGrn(picked);
    }
    function LoadInGridDetailFromGrn(dt) {   /* :4011 */
        if (!dt.length) return;
        if (S.dtdetail.length > 0 && toI(val('CmbSupplierName')) !== toI(col(dt[0], 'SupplierCustomerId'))) { msg("Already Loaded Row's Have Different Supplier. So you Can't Load Rows Of Different Supplier!"); return; }
        setSel('CmbSupplierName', toI(col(dt[0], 'SupplierCustomerId')));
        var existing = S.dtdetail.map(function (r) { return toI(r.GdnDetailId); });
        dt.forEach(function (d) {
            if (existing.indexOf(toI(col(d, 'DetailId'))) >= 0) return;
            S.dtdetail.push({
                Id: 0, GdnId: toI(col(d, 'Id')), GdnDetailId: toI(col(d, 'DetailId')), GdnDocumentTypeId: toI(col(d, 'DocumentTypeId')), GdnNo: toI(col(d, 'GdnNo')),
                WareHouseId: toI(col(d, 'WarehouseId')), ItemId: toI(col(d, 'ItemId')), ItemCode: col(d, 'ItemCode') || '', ItemName: col(d, 'ItemName') || '',
                CropYearId: toI(col(d, 'CropYearId')), JobId: toI(col(d, 'JobLotId')), PackingTypeId: toI(col(d, 'PackingTypeId')), UOMId: toI(col(d, 'ItemUomId')),
                UOM: col(d, 'ItemUom') || '', UomEquivalent: toD(col(d, 'ItemEquivalent')), Qty: toD(col(d, 'BalQty')), GrossWight: toD(col(d, 'BalWeight')),
                EbUnit: toD(col(d, 'EBWPerUnit')), EbTotal: toD(col(d, 'EBWTotal')), WtCut: toD(col(d, 'WtCut')), WtCutTotal: toD(col(d, 'WtCutTotal')),
                AddLesswt: toD(col(d, 'AdLsWeight')), NetWeight: toD(col(d, 'BalWeight')), StockWeight: toD(col(d, 'StockWeight')), LabNo: '', CityId: toI(col(d, 'CityId'))
            });
        });
        grdSettings();
        $('CmbSupplierName').disabled = true;
        $('panel5').hidden = true;
        GetDeliveryScheduleDetailIds();
    }

    /* ------------------------------------------------------------------ History tab */
    var H_COLS = ['OrderType', 'DocDate', 'DocNo', 'DeliveryTerm', 'SupplierName', 'GpNo', 'VehicleNo', 'BiltyNo', 'WagesNo', 'FactoryWeight', 'PartyWeight', 'Transporter', 'CarriageAmount', 'RemarksHeader', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'NoOfAttachments'];
    async function HistoryGridFill() {       /* :3074 */
        var mode = (doc.querySelector('input[name="histDate"]:checked') || {}).value || 'docdate';
        var body = {
            fromDate: $('chkFromDateHistory').checked ? val('FromDateHistory') : null,
            toDate: $('chkToDateHistory').checked ? val('ToDateHistory') : null,
            dateType: mode, fromDocNo: toI(val('txtFromDocNoHistory')), toDocNo: toI(val('txtToDocNoHistory')), supplierId: toI(val('cmbSupplierNameHistory'))
        };
        S.historyRows = await post('/history', body) || [];
        S.curHistory = -1;
        renderHistory();
        $('GrdHistoryDetail').innerHTML = '';
    }
    function renderHistory() {               /* HistoryGridSettings :3188 */
        if (!S.historyRows.length) { $('GrdHistory').innerHTML = ''; return; }
        var head = '<thead><tr><th>Edit</th><th>Print</th>' + H_COLS.map(function (c) { return '<th>' + cap(c) + '</th>'; }).join('') + '</tr></thead>';
        var body = S.historyRows.map(function (r, i) {
            return '<tr data-h="' + i + '"' + (i === S.curHistory ? ' class="cur"' : '') + '><td class="btncell"><button type="button" data-hedit="' + i + '">Edit</button></td><td class="btncell"><button type="button" data-hprint="' + i + '">Print</button></td>' +
                H_COLS.map(function (c) {
                    var v = col(r, c);
                    if (c === 'GpNo' || c === 'WagesNo' || c === 'NoOfAttachments') return '<td><a class="lnk" tabindex="0" data-hlink="' + c + '" data-h2="' + i + '">' + esc(v) + '</a></td>';
                    if (c === 'DocDate') return '<td>' + esc(dateText(v)) + '</td>';
                    if (c === 'EntryDate' || c === 'ModifyDate') return '<td>' + esc(dateTimeText(v)) + '</td>';
                    return '<td' + (typeof v === 'number' ? ' class="n"' : '') + '>' + esc(typeof v === 'number' ? cs(v) : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $('GrdHistory').innerHTML = head + '<tbody>' + body + '</tbody>';
    }
    async function GrdHistory_SelectionChanged(i) {  /* :3363 / DetailGridBind :3385 */
        S.curHistory = i; renderHistory();
        var r = S.historyRows[i];
        if (!r) { $('GrdHistoryDetail').innerHTML = ''; return; }
        var g = await api('/' + toI(col(r, 'Id')));
        var rows = (col(g, 'details') || []).map(detailFromRecord);
        var gdn = rows.some(function (x) { return x.GdnId > 0; });
        /* grdHistoryDetailSettings :3412 - Id columns hidden, the name columns placed after them. */
        var cols = (gdn ? ['GdnNo'] : []).concat(['WareHouseName', 'ItemCode', 'ItemName', 'CropYear', 'JobLot', 'PackingType', 'UOM', 'Qty', 'GrossWight', 'EbUnit', 'EbTotal', 'WtCut', 'WtCutTotal', 'AddLesswt', 'NetWeight', 'StockWeight', 'LabNo', 'CityName']);
        $('GrdHistoryDetail').innerHTML = '<thead><tr>' + cols.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr></thead><tbody>' +
            rows.map(function (x) { return '<tr>' + cols.map(function (c) { var v = x[c]; return '<td' + (typeof v === 'number' ? ' class="n"' : '') + '>' + esc(typeof v === 'number' ? cs(v) : v) + '</td>'; }).join('') + '</tr>'; }).join('') + '</tbody>';
    }
    function historyLink(key, i) {           /* GrdHistory_LinkClicked :3270 */
        var r = S.historyRows[i]; if (!r) return;
        if (key === 'GpNo') return printPdf('/reports/print/251-inward-gate-pass-slip', { id: toI(col(r, 'InwardGatePassId')) });
        if (key === 'WagesNo') return printPdf('/reports/print/002-contractor-wages-slip', { id: toI(col(r, 'WagesId')) });
        if (key === 'NoOfAttachments' && AT) return AT.view(toI(col(r, 'Id')));   /* GetNoofAttachmentsByScreenName */
    }
    function btnNewHistory_Click() {         /* :3333 */
        $('FromDateHistory').value = today(); $('ToDateHistory').value = today();
        $('txtFromDocNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        setSel('cmbSupplierNameHistory', 0);
        S.historyRows = []; $('GrdHistory').innerHTML = ''; $('GrdHistoryDetail').innerHTML = '';
    }
    async function btnRefreshHistory_Click() { /* :3351 */
        S.lists.historySuppliers = await api('/history-suppliers');
        fill('cmbSupplierNameHistory', S.lists.historySuppliers, 'id', 'name');
    }
    /* :3298 frmGRNHistory - opened only with its View right (ScreenViewReights), else "Please Check Screen Rights". */
    function btnGrnFormHistory_Click() {
        var w = global.open('about:blank', '_blank');
        var ask = fetch('/api/purchase/screen-view-right/frmGRNHistory', { credentials: 'same-origin', headers: { Accept: 'application/json' } })
            .then(function (r) { return r.ok ? r.json() : { view: false }; });
        return (global.PurchaseRequest ? global.PurchaseRequest.track(function () { return ask; }) : ask)
            .then(function (d) {
                if (d && d.view) { if (w) w.location.href = '/purchase/reports/grn-register'; else global.open('/purchase/reports/grn-register', '_blank'); }
                else { if (w) w.close(); msg('Please Check Screen Rights'); }
            }).catch(function (e) { if (w) w.close(); fail(e); });
    }
    function btnAttachment_Click(button) { return AT ? AT.open(button || $('btnAttachment')) : null; }

    /* ------------------------------------------------------------------ toolbar */
    async function btnRefresh_Click() {      /* toolStripButton1_Click :1804 - re-read the global lists and rebind */
        var d = await api('/dropdowns');
        applyLookups(d);
        bindSuppliers(); bindTransporters(); bindStatic();
        await ItemDtFillDbCall(toI(val('CmbSupplierName'))); bindItems();
        GetConfigurationsFromGlobalAndBindValuesInColumns();
        renderEmptyBags(); renderDetail();
    }
    function selectTab(n) {                  /* tabControl1 */
        $('tabPage1').hidden = n !== 0; $('tabPage2').hidden = n !== 1;
        $('tabBtn1').classList.toggle('on', n === 0); $('tabBtn2').classList.toggle('on', n === 1);
        if (n === 1) $('FromDateHistory').focus();
    }
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Ctrl+H', 'For History Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On warehouse Combo in Detail Box'],
        ['Ctrl+ArrowRight', 'For Focus From One Grid To Another'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];
    function BtnShortCutkeys_Click() {       /* MakeShortCutKeys :3765 */
        $('shortcutTable').innerHTML = '<thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' + SHORTCUTS.map(function (s) { return '<tr><td>' + esc(s[0]) + '</td><td>' + esc(s[1]) + '</td></tr>'; }).join('') + '</tbody>';
        $('shortcutPopup').hidden = false;
    }
    function run(button, fn) {
        var p = global.PurchaseRequest ? global.PurchaseRequest.run(button, fn) : Promise.resolve().then(fn);
        return p.catch(fail);
    }
    function focusCombo(id) { var s = $(id); var w = s && s.__dtcombo; var inp = w && w.input; (inp || s).focus(); }

    /* InvFrmGRN_KeyDown :3631 */
    function onKey(e) {
        if (!$('gdnLoader').hidden || !$('shortcutPopup').hidden) { if (e.key === 'Escape') { $('gdnLoader').hidden = true; $('shortcutPopup').hidden = true; } return; }
        var c = e.ctrlKey, k = e.key, K = (k || '').toUpperCase(), hist = !$('tabPage2').hidden;
        var handled = true;
        if (c && e.altKey && (k === 'Control' || k === 'Alt')) { BtnShortCutkeys_Click(); }
        else if ((c && K === 'E') || k === 'Escape') { global.location.href = '/purchase'; }
        else if (c && K === 'T') { selectTab(hist ? 0 : 1); if (hist) $('DocDate').focus(); }
        else if (hist) {
            if (c && k === 'ArrowUp') $('FromDateHistory').focus();
            else if (c && K === 'N') btnNewHistory_Click();
            else if (c && K === 'R') run($('btnRefreshHistory'), btnRefreshHistory_Click);
            else if (c && K === 'S') run($('btnshow'), HistoryGridFill);
            else if (c && k === 'ArrowDown') $('GrdHistoryWrap').focus();
            else if (c && K === 'P') { var r = S.historyRows[S.curHistory]; if (r) GenerateReport(toI(col(r, 'Id'))); }
            else if (c && K === 'H') btnGrnFormHistory_Click();
            else if (c && k === 'Enter' && doc.activeElement === $('GrdHistoryWrap') && S.historyRows[S.curHistory]) run(null, function () { return ReadById(toI(col(S.historyRows[S.curHistory], 'Id'))); });
            else handled = false;
        } else {
            if (c && K === 'N') run($('btnNew'), btnNew_Click);
            else if (c && K === 'P' && !$('printToolStripButton').hidden && !$('printToolStripButton').disabled) run($('printToolStripButton'), printToolStripButton_Click);
            else if (c && K === 'S' && saveVisible() && saveEnabled()) run($('btnSave'), btnSave_Click);
            else if (c && k === 'Delete' && doc.activeElement === $('grdWrap') && S.curDetail >= 0) DeleteRowInDetailGrid(S.curDetail);
            else if (c && k === 'Delete' && !$('btnDelete').hidden && !$('btnDelete').disabled) run($('btnDelete'), btnDelete_Click);
            else if (c && K === 'U' && !$('btnUpdate').hidden && !$('btnUpdate').disabled) run($('btnUpdate'), btnUpdate_Click);
            else if (c && K === 'D' && doc.activeElement === $('grdWrap') && S.curDetail >= 0) AddRowInDetailGrid(S.curDetail);
            else if (c && k === 'Enter' && doc.activeElement === $('grdWrap')) grd_DoubleClick();
            else if (c && k === 'Enter' && doc.activeElement === $('grdGpWrap') && S.curGp >= 0) run(null, function () { return LoadClickInGpGrid(S.curGp); });
            else if (c && k === 'ArrowDown') $('grdWrap').focus();
            else if (c && k === 'ArrowUp') focusCombo('CmbWareHouseName');
            else if (c && k === 'ArrowRight') { var a = doc.activeElement; (a === $('grdWrap') ? $('grdEmptyBagsWrap') : a === $('grdEmptyBagsWrap') ? $('grdGpWrap') : $('grdWrap')).focus(); }
            else if (c && k === 'F5') $('DocDate').focus();
            else if (c && k === 'F10') btnAttachment_Click();
            else handled = false;
        }
        if (handled) e.preventDefault();
    }

    /* ------------------------------------------------------------------ wiring */
    function wire() {
        Object.keys(TEXT_CHANGED).forEach(function (id) { $(id).addEventListener('input', function () { depth++; try { TEXT_CHANGED[id](); } finally { depth--; } }); });
        $('txtadlswt').addEventListener('blur', Total);                                  /* txtadlswt_Leave */
        $('CmbSupplierName').addEventListener('change', function () { run(null, CmbSupplierName_Leave); });
        $('CmbItemName').addEventListener('change', CmbItemName_Leave);                 /* ValueChanged + Leave */
        $('CmbUom').addEventListener('change', function () { TotalWeight(); Total(); }); /* CmbUom.Leave -> txtqty_TextChanged */
        $('CmbGpNo').addEventListener('change', function () { run(null, combgatepass_Leave); });
        $('rdSearchByName').addEventListener('change', bindItems);
        $('rdSearchByCode').addEventListener('change', bindItems);
        /* txtcarramount_KeyPress :3507 digits only; txtFrom/ToDocNoHistory numbers only. */
        ['txtcarramount', 'txtFromDocNoHistory', 'txtToDocNoHistory'].forEach(function (id) { $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); }); });
        /* OnlytextdecimelFunction */
        ['txtGrossWeight', 'txtebu', 'txtwtcut', 'txtQty'].forEach(function (id) { $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9.]/.test(e.key)) e.preventDefault(); }); });

        var grd = $('grd');
        grd.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]');
            var tr = e.target.closest('tr[data-row]');
            if (tr) { S.curDetail = toI(tr.getAttribute('data-row')); doc.querySelectorAll('#grd tr.cur').forEach(function (x) { x.classList.remove('cur'); }); tr.classList.add('cur'); }
            if (!b) return;
            var i = toI(b.getAttribute('data-row'));
            if (b.getAttribute('data-act') === 'del') DeleteRowInDetailGrid(i); else AddRowInDetailGrid(i);
        });
        grd.addEventListener('dblclick', function (e) { if (e.target.closest('input,select,button')) return; grd_DoubleClick(); });
        grd.addEventListener('change', function (e) {
            var t = e.target, i = toI(t.getAttribute('data-row')), key = t.getAttribute('data-key'), r = S.dtdetail[i];
            if (!r || !key) return;
            r[key] = (key === 'LabNo') ? t.value : (GRD_COMBOS[key] ? toI(t.value) : toD(t.value));
            grdCellUpdated(i, key);
            renderDetail();
        });
        var bags = $('grdEmptyBags');
        bags.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-bagadd]'); if (!b) return;
            if (S.dtEmptyBag.length === 1) AddRowInvEmptyBagsGrid();                    /* :2676 */
            renderEmptyBags();
        });
        bags.addEventListener('change', function (e) {
            var t = e.target, i = toI(t.getAttribute('data-bag')), key = t.getAttribute('data-key'), r = S.dtEmptyBag[i];
            if (!r || !key) return;
            r[key] = key === 'Remarks' ? t.value : (key === 'RecQty' || key === 'PurQty' ? toD(t.value) : toI(t.value));
            grdEmptyBagsCellUpdated(i, key);
        });
        var gp = $('grdGp');
        gp.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-gp]'); if (tr) { S.curGp = toI(tr.getAttribute('data-gp')); doc.querySelectorAll('#grdGp tr.cur').forEach(function (x) { x.classList.remove('cur'); }); tr.classList.add('cur'); }
            var b = e.target.closest('button[data-gpload]'); if (b) run(b, function () { return LoadClickInGpGrid(toI(b.getAttribute('data-gpload'))); });
            var a = e.target.closest('a[data-gplink]'); if (a) printPdf('/reports/print/251-inward-gate-pass-slip', { id: toI(col(S.gpRows[toI(a.getAttribute('data-gplink'))], 'Id')) });
        });
        var h = $('GrdHistory');
        h.addEventListener('click', function (e) {
            var ed = e.target.closest('button[data-hedit]'), pr = e.target.closest('button[data-hprint]'), ln = e.target.closest('a[data-hlink]'), tr = e.target.closest('tr[data-h]');
            if (ed) { run(ed, function () { return ReadById(toI(col(S.historyRows[toI(ed.getAttribute('data-hedit'))], 'Id'))); }); return; }
            if (pr) { GenerateReport(toI(col(S.historyRows[toI(pr.getAttribute('data-hprint'))], 'Id'))); return; }
            if (ln) { historyLink(ln.getAttribute('data-hlink'), toI(ln.getAttribute('data-h2'))); return; }
            if (tr) run(null, function () { return GrdHistory_SelectionChanged(toI(tr.getAttribute('data-h'))); });
        });
        h.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button,a')) return;
            run(null, function () { return ReadById(toI(col(S.historyRows[toI(tr.getAttribute('data-h'))], 'Id'))); });
        });
        doc.addEventListener('keydown', onKey);
        /* GrdHistory_KeyDown / grdGp_KeyDown - Ctrl+Space (or Enter) on a focused link cell runs the link. */
        doc.addEventListener('keydown', function (e) {
            var a = e.target.closest && e.target.closest('a.lnk[tabindex]');
            if (a && ((e.ctrlKey && e.code === 'Space') || (e.key === 'Enter' && !e.ctrlKey))) { e.preventDefault(); a.click(); return; }
            /* grd_KeyDown :3799 / grdGp_KeyDown :3867 / grdEmptyBags_KeyDown :3899 / GrdHistory_KeyDown :3920 - Ctrl+Space on a button cell */
            var b = e.ctrlKey && e.code === 'Space' && e.target.closest && e.target.closest('#grd button, #grdGp button, #grdEmptyBags button, #GrdHistory button');
            if (b) { e.preventDefault(); b.click(); }
        });
        if (global.PurchaseChrome) {
            global.PurchaseChrome.footer({ isHistory: function () { return !$('tabPage2').hidden; }, toggle: function () { selectTab($('tabPage2').hidden ? 1 : 0); }, watch: $('tabPage2') });
            ['grdWrap', 'grdGpWrap', 'GrdHistoryWrap', 'GrdHistoryDetailWrap'].forEach(function (id) { global.PurchaseChrome.fullscreen($(id)); });
        }
    }

    /* ------------------------------------------------------------------ start-up (InitializeComponentMethod :545) */
    async function init() {
        applyLookups(BOOT);
        $('btnSave').disabled = !S.rights.Save;
        $('printToolStripButton').disabled = !S.rights.Print;
        $('btnUpdate').disabled = !S.rights.Update;
        $('btnDelete').disabled = !S.rights.Delete;
        $('btnDelete').hidden = !S.rights.Delete;
        if (cfgBool('ItemSearchByCode')) $('rdSearchByCode').checked = true; else $('rdSearchByName').checked = true;
        $('DocDate').value = today();
        bindSuppliers(); bindTransporters(); bindStatic();
        $('CmbUom').innerHTML = '<option value="0"></option>';
        S.itemRows = []; bindItems();
        GetConfigurationsFromGlobalAndBindValuesInColumns();
        renderDetail();
        AddRowInvEmptyBagsGrid(); grdEmptyBagsSettings();
        GatepassGridFill(S.lists.gatePasses);
        var days = cfgInt('DefaultDaysToLessFromHistoryFromDate');
        $('FromDateHistory').value = addDays(days > 0 ? -days : -3);
        $('ToDateHistory').value = today();
        $('ChkBox').checked = true;
        $('btnSave').hidden = false; $('btnUpdate').hidden = true;
        $('txtsuppwt').readOnly = true;
        setText('txtebu', '0'); setText('txtadlswt', '0');
        $('txtebu').disabled = true; $('txtadlswt').disabled = true; $('txtebt').disabled = true;
        if (cfgBool('EmptyBagsWeightCutEditableOnGRN')) { $('txtebu').disabled = false; $('txtebt').disabled = false; }
        if (cfgBool('AddLessWeightCutEditableOnGRN')) $('txtadlswt').disabled = false;
        setText('txtDeliverTerm', 'Load & FactoryWeight');
        wire();
        $('DocDate').focus();
        var id = new URLSearchParams(global.location.search).get('id');
        if (id) await run(null, function () { return ReadById(id); });
    }

    async function btnNew_Click() { await reset(); }
    function printToolStripButton_Click() { return GenerateReport(S.RecId); }              /* :3447 */
    function btnSlip257_Click() { return printPdf('/reports/print/257-inward-gate-pass-with-wb-and-lab-slip', { id: toI(val('CmbGpNo')) }); } /* :3459 */
    function CityDefine_Click() { global.open('/master-data/city', '_blank'); }             /* :1689 DefineCity */

    global.SRG = {
        run: run, selectTab: selectTab, btnNew_Click: btnNew_Click, btnRefresh_Click: btnRefresh_Click, btnSave_Click: btnSave_Click,
        btnUpdate_Click: btnUpdate_Click, btnDelete_Click: btnDelete_Click, printToolStripButton_Click: printToolStripButton_Click,
        btnSlip257_Click: btnSlip257_Click, CityDefine_Click: CityDefine_Click, btnAttachment_Click: btnAttachment_Click, btnLoadGdn_Click: btnLoadGdn_Click, BtnShortCutkeys_Click: BtnShortCutkeys_Click,
        Add_Click: Add_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, ResetDetail: ResetDetail,
        btnNewHistory_Click: btnNewHistory_Click, btnRefreshHistory_Click: btnRefreshHistory_Click, btnGrnFormHistory_Click: btnGrnFormHistory_Click,
        btnshow_Click: HistoryGridFill, searchGdn: searchGdn, closeGdnLoader: closeGdnLoader, loadSelectedGdn: loadSelectedGdn,
        _test: { Total: Total, TotalWeight: TotalWeight, CalculationPurchaseAgainstWeight: CalculationPurchaseAgainstWeight, grdCellUpdated: grdCellUpdated, state: S, round: round, cs: cs }
    };

    if (doc.readyState === 'loading') doc.addEventListener('DOMContentLoaded', function () { init().catch(fail); });
    else init().catch(fail);
}(window));
