/* Goods Receiving Notes (InvFrmGRN.cs, DocumentTypeId 46).
 *
 * Every calculation below is a port of the desktop method named beside it, in the same order the
 * desktop runs them. The server repeats the refusals of Insert() on Save (PurchaseGrnSaveRules),
 * so nothing here is the only guard.
 *
 * Form state the desktop keeps in fields:
 *   grn.ref           RefDocumentTypeId (41 PO, 105 Market Purchase, 106 Gate Purchase)
 *   grn.freightId     FreightId (freight voucher already made against the gate pass)
 *   grn.cfg           GetConfigurationsFromGlobal :1004
 *   grn.items         dtItem  (ItemId, ItemName, ItemCode, PoDetailId, ItemWbWeight, Moisture)
 *   grn.lab           CmbLabNo's single row, or null
 *   grn.previousData  dtPrevoiusDataOfSelectedGP
 */
function escapeHtml(v) {
    return String(v === undefined || v === null ? '' : v)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}
let lineItems = [], loadedHeader = {}, editLine = -1, lookupVersion = 0;
const grn = { ref: 0, freightId: 0, gpId: 0, gpDate: '', gpQty: 0, purchaseOrderId: 0, purchaseOrderNo: 0, cfg: {}, items: [],
              lab: null, previousData: [], poEmptyBags: [], preBills: [], chargeToParty: 0, subsidiary: false, wages: false };

const cfgNum = name => Number(grn.cfg[name]) || 0;
const cfgOn = name => { const v = String(grn.cfg[name] ?? '').trim().toLowerCase(); return v === '1' || v === 'true'; };
const round = (v, d) => { const f = Math.pow(10, d); return Math.round((Number(v) || 0) * f) / f; };
/* C# ToString("#,##") / ("#,##.##") as written into a textbox that is read back as a number. */
const fmt0 = v => String(Math.round(Number(v) || 0));
const fmt2 = v => String(round(v, 2));

function narrowUomToItem() {
    var itemId = parseInt($('#cmbItem').val() || '0', 10);
    var sel = $('#cmbUom'), current = sel.val(), keptCurrent = false;
    sel.find('option').each(function () {
        var opt = $(this);
        if (!opt.attr('data-item-id')) return;
        var owns = parseInt(opt.attr('data-item-id'), 10) === itemId;
        opt.prop('hidden', !owns).prop('disabled', !owns);
        if (owns && opt.val() === current) keptCurrent = true;
    });
    if (!keptCurrent) sel.val('0');
}
/* CmbPackUom.SelectedRow.Cells[2] — Equivalent. Null when missing: callers refuse, never assume 1. */
function grnUomEquivalent() {
    var opt = $('#cmbUom option:selected');
    if (!opt.length || opt.val() === '0') return null;
    var v = parseFloat(opt.attr('data-equivalent'));
    return (isNaN(v) || v <= 0) ? null : v;
}
function packUom() { return grnUomEquivalent() || 0; }

$(document).ready(async function () {
    $('#cmbItem').on('change', combitemLeave);
    $('#cmbLabNo').on('change', cmbLabNoLeave);
    $('#cmbPackingType').on('change', combpcktypLeave);
    $('#cmbUom').on('change', combpckuomLeave);
    $('#txtItemQty').on('input', txtqtyChanged).on('blur', () => { if (grn.ref === 105) markeetPurchaseFormula(); });
    $('#txtGrossWeight').on('input', () => { if (grn.ref === 105) ebCalculations(); total(); }).on('blur', txtgwtLeave);
    $('#txtEbUnit').on('input', () => { ebCalculations('unit'); total(); });
    $('#txtEbTotal').on('input', () => { ebCalculations('total'); total(); });
    $('#txtEbUnitStock').on('input', () => { stockEbCalculations('unit'); total(); });
    $('#txtEbTotalStock').on('input', () => { stockEbCalculations('total'); total(); });
    $('#txtWtCut,#txtAddLess').on('input', total);
    $('#txtSupplierWeight,#txtFactoryWeight').on('input', supplierFactoryChanged);
    $('#cmbDeliveryTerm').on('change', txtDeliverTermChanged);
    $('#txtBiltyFreight,#txtScaleCharges,#txtAdvParty,#txtAdvFactory,#txtOtherDeduct,#txtFreightDeduct').on('input', freightCalculations);
    $('#txtPaidAmount').on('change', () => { freightCalculations(); if (grn.ref === 105) freightProportion(); renderItemsGrid(); });
    $('#chkScaleDeduct,#chkSupplierDeduct').on('change', () => { totalEbPurchaseAgainstWeightUtilizeInGrid(); renderItemsGrid(); recalculateBreakupHeader(); });
    $('#cmbSupplier').on('change', combsupplierLeave);
    $('#cmbPreBillNo').on('change', preBillLeave);
    $('input[name="itemSearchMode"]').on('change', itemNameBind);
    narrowUomToItem();
    $('#txtDocDate').val(new Date().toLocaleDateString('en-CA'));
    try { grn.cfg = await api('/api/purchase/market-grn/form/config'); } catch (e) { grn.cfg = {}; }
    grn.subsidiary = !!grn.cfg.SubsidiaryAccountAllownOnVouchers; grn.wages = !!grn.cfg.WagesActiveOrInActive;
    applyConfigToControls();
    fillCountFromGpGrid();
    const id = new URLSearchParams(window.location.search).get('id');
    if (id) loadRecord(id); else { renderSupplements({}); applyConfigDefaults(); }
    renderItemsGrid();
});

/* InitializeComponentMethod :790-815 */
function applyConfigToControls() {
    $('#txtEbUnit,#txtEbTotal').prop('disabled', !cfgOn('EmptyBagsWeightCutEditableOnGRN'));
    $('#txtAddLess').prop('disabled', !cfgOn('AddLessWeightCutEditableOnGRN'));
    $('#txtWtCut').prop('disabled', !cfgOn('WeightCutEditable'));
    $('#txtDocDate').prop('disabled', cfgOn('ValidateGrnAndInvoiceDateWithGpDate'));
    if (cfgOn('ItemSearchWithNameOrCode')) $('input[name="itemSearchMode"][value="Code"]').prop('checked', true);
    $('#txtSupplierWeight').prop('readonly', true);
    $('#cmbDeliveryTerm').prop('disabled', true);
    showWeightFields(false);
}
/* GetConfigurationsFromGlobalAndBindValuesInColumns :928 */
function applyConfigDefaults() {
    for (const [cfg, id] of [['Job/Lot', 'cmbJobLot'], ['Default Crop Year', 'cmbCropYear'], ['Paking Type', 'cmbPackingType'], ['Warehouse', 'cmbWarehouse']]) {
        const v = cfgNum(cfg); if (v) $('#' + id).val(String(v));
    }
    if (!grn.subsidiary) { const t = cfgNum('FreightInwardAc'); if (t) $('#cmbTransporter').val(String(t)); }
    transporterAccountDisable();
}
function transporterAccountDisable() {
    if (grn.subsidiary) return;
    $('#cmbTransporter').prop('disabled', grn.freightId > 0);
    $('#txtPaidAmount').prop('readonly', grn.freightId > 0);
}
function showWeightFields(show) { $('.grn-105-only').toggle(!!show); }
/* ShowShortageCheckBoxes :2838 */
function showShortageCheckBoxes() {
    $('#lblSupplierDeduct').toggle(grn.ref === 105);
    $('#lblScaleDeduct').toggle(grn.ref !== 106);
    $('#txtGrossWeight').prop('disabled', grn.ref === 105);
}

function withButtonLoading(btn, asyncFn) { return PurchaseRequest.run(btn, asyncFn).catch(error => alert(error.message)).finally(applyGrnRights); }
async function api(url, options) {
    return PurchaseRequest.track(async () => {
        const response = await fetch(url, options); const data = await response.json();
        if (!response.ok || data?.success === false) throw new Error(data.message || data.detail || 'Request failed'); return data;
    });
}
function numeric(id) { return Number($('#' + id).val()) || 0; }
function textValue(id) { return $('#' + id).val() || ''; }
function selectedText(id) { const o = $('#' + id + ' option:selected'); return o.length && o.val() !== '0' && o.val() !== '' ? o.text() : ''; }
function displayDate(value) { return String(value || '').slice(0, 10); }
function selectValue(id, value, label) {
    const select = document.getElementById(id); if (!select) return;
    if (value && !Array.from(select.options).some(option => String(option.value) === String(value))) select.add(new Option(label || String(value), value));
    $(select).val(value ?? '0');
}
function selectByText(id, text) {
    const select = document.getElementById(id); if (!select) return;
    const t = String(text || '').trim(); if (!t) { $(select).val('0'); return; }
    let opt = Array.from(select.options).find(o => o.text.trim().toLowerCase() === t.toLowerCase());
    if (!opt) { opt = new Option(t, t); select.add(opt); }
    $(select).val(opt.value);
}
const gridSum = (rows, key) => rows.reduce((s, r) => s + (Number(r[key]) || 0), 0);
const breakupSum = key => (typeof purchaseBreakupRows !== 'undefined' ? purchaseBreakupRows : []).reduce((s, r) => s + (Number(r[key.toLowerCase()]) || 0), 0);
const breakupCount = () => (typeof purchaseBreakupRows !== 'undefined' ? purchaseBreakupRows.length : 0);

/* ---------------- items / lab / pre-bill ---------------- */
function itemNameBind() {
    const byCode = $('input[name="itemSearchMode"]:checked').val() === 'Code';
    const sel = $('#cmbItem'), current = sel.val();
    sel.empty().append(new Option('-- Select Item --', '0'));
    for (const it of grn.items) {
        const o = new Option(byCode ? (it.ItemCode || it.ItemName) : it.ItemName, it.ItemId);
        sel.append(o);
    }
    sel.val(current && grn.items.some(i => String(i.ItemId) === String(current)) ? current : '0');
    narrowUomToItem();
}
function selectedItem() { const id = numeric('cmbItem'); return grn.items.find(i => Number(i.ItemId) === id) || null; }

/* combitem_Leave :1433 */
async function combitemLeave() {
    narrowUomToItem();
    $('#txtMoisture').val(0);
    await labNoFill(numeric('cmbItem'));
    if (grn.purchaseOrderId > 0) { const it = selectedItem(); if (it) $('#txtMoisture').val(Number(it.Moisture) || 0); }
    weightBusinessCalc();
    total();
}
/* LabNoFillWithGpIdAndItemId :1385 */
async function labNoFill(itemId) {
    const sel = $('#cmbLabNo'); sel.empty(); grn.lab = null; grn.labRows = [];
    if (grn.gpId > 0 && itemId > 0) {
        try { grn.labRows = await api('/api/purchase/market-grn/form/lab?gpId=' + grn.gpId + '&itemId=' + itemId); } catch (e) { alert(e.message); grn.labRows = []; }
    }
    if (!grn.labRows.length) { sel.append(new Option('', '0')); sel.val('0'); await cmbLabNoLeave(); return; }
    for (const r of grn.labRows) sel.append(new Option(String(r.LabNo ?? ''), r.Id));
    sel.val(String(grn.labRows[0].Id));
    await cmbLabNoLeave();
}
/* CmbLabNo_Leave :6995 */
async function cmbLabNoLeave() {
    const labId = numeric('cmbLabNo');
    grn.lab = (grn.labRows || []).find(r => Number(r.Id) === labId) || null;
    if (cfgOn('WeightCutEditable')) $('#txtWtCut').prop('disabled', false);
    $('#txtWtCutComp').val(0); $('#txtRateCutComp').val(0);
    $('#chkRateCutComp').prop('checked', false); $('#chkWtCutComp').prop('checked', false);
    $('#labParamsBody').empty(); $('#labParamsPanel').hide();
    if (grn.lab && labId > 0) {
        const wtCut = Number(grn.lab.WtCut) || 0;
        $('#txtWtCut').val(wtCut); $('#txtWtCutOn').val(grn.lab.WtCutOn || '');
        let rows = [];
        try { rows = await api('/api/purchase/market-grn/form/lab-parameters/' + labId); } catch (e) { alert(e.message); }
        if (rows.length) {
            $('#txtWtCutComp').val(wtCut); $('#txtRateCutComp').val(Number(rows[0].DeductionRate) || 0);
            const rateComp = !!rows[0].IsRateCutCompulsory, wtComp = !!rows[0].IsWeightCutCompulsory;
            $('#chkRateCutComp').prop('checked', rateComp); $('#chkWtCutComp').prop('checked', wtComp);
            if (wtComp) $('#txtWtCut').prop('disabled', true); else if (cfgOn('WeightCutEditable')) $('#txtWtCut').prop('disabled', false);
            $('#labParamsBody').html(rows.map(r => '<tr><td>' + escapeHtml(r.AnalysisParameterDescription) + '</td><td class="text-end">' + escapeHtml(r.InAnalysisResult) + '</td></tr>').join(''));
            $('#labParamsPanel').show();
        }
    } else { $('#txtWtCut').val(0); $('#txtWtCutOn').val(''); }
    if (grn.ref === 105) ebCalculations();
    total();
}
/* combsupplier_Leave :7074 / PreBillNoFill :884 */
async function combsupplierLeave() {
    const supplier = numeric('cmbSupplier'), sel = $('#cmbPreBillNo'), current = sel.val();
    if (supplier > 0 || grn.purchaseOrderId > 0) {
        try { grn.preBills = await api('/api/purchase/market-grn/form/pre-bills?supplierId=' + supplier + '&grnId=' + numeric('txtId') + '&gpId=' + grn.gpId + '&orderId=' + grn.purchaseOrderId); }
        catch (e) { alert(e.message); grn.preBills = []; }
    } else grn.preBills = [];
    fillPreBills(current);
}
function fillPreBills(value) {
    const sel = $('#cmbPreBillNo'); sel.empty().append(new Option('', '0'));
    for (const r of grn.preBills) sel.append(new Option(String(r.SupplierDispatchNo ?? ''), r.SupplierDispatchId));
    if (value && grn.preBills.some(r => String(r.SupplierDispatchId) === String(value))) sel.val(String(value));
    else if (grn.purchaseOrderId > 0 && grn.preBills.length) sel.val(String(grn.preBills[0].SupplierDispatchId));
    else sel.val('0');
    if (grn.preBills.length) preBillLeave();
}
/* CmbSupplierDispatchedPreBillNo_Leave :7093 */
function preBillLeave() {
    const id = numeric('cmbPreBillNo'), row = grn.preBills.find(r => Number(r.SupplierDispatchId) === id);
    if (row && id > 0) {
        if (row.VehicleNo && !textValue('txtVehicleNo').trim()) $('#txtVehicleNo').val(row.VehicleNo);
        if (row.BiltyNo && !textValue('txtBiltyNo').trim()) $('#txtBiltyNo').val(row.BiltyNo);
        if (grn.freightId === 0) {
            if (Number(row.Freight) > 0) $('#txtBiltyFreight').val(Math.round(row.Freight));
            if (Number(row.AdvanceFreight) > 0) $('#txtAdvParty').val(Math.round(row.AdvanceFreight));
            freightCalculations();
        }
    }
    $('#txtVehicleNo').prop('disabled', id !== 0);
}

/* ---------------- entry-box calculations ---------------- */
/* combpcktyp_Leave :5127 (+ PurchaseOrderEmptyBagsDetailByOrderId :2267 for PO gate passes) */
function combpcktypLeave() {
    purchaseOrderEmptyBags();
    const p = numeric('cmbPackingType');
    if (grn.ref !== 41) {
        const cut = { 1: cfgNum('WeightCutForJuteBags'), 2: cfgNum('WeightCutForPPBags'), 5: cfgNum('WeightCutForOpenBulk') }[p];
        if (cut !== undefined && numeric('txtEbUnit') === 0) $('#txtEbUnit').val(cut);
    }
    const stock = { 1: cfgNum('WeightCutForJuteBagsStock'), 2: cfgNum('WeightCutForPPBagsStock'), 5: cfgNum('WeightCutForOpenBulkStock') }[p];
    if (stock !== undefined && numeric('txtEbUnitStock') === 0) $('#txtEbUnitStock').val(stock);
    ebCalculations(); stockEbCalculations(); total();
}
function purchaseOrderEmptyBags() {
    if (grn.ref !== 41) return;
    if (!grn.poEmptyBags.length) { $('#txtEbUnit').val(0); return; }
    if (numeric('txtId') > 0) return;                     /* only while Save is the active button */
    const p = numeric('cmbPackingType'), bags = [];
    for (const r of grn.poEmptyBags) {
        const type = Number(r.Type), pt = Number(r.PackingTypeId);
        if (pt === p) $('#txtEbUnit').val(Number(r.WeightCut) > 0 ? r.WeightCut : (pt === 2 ? cfgNum('WeightCutForPPBags') : cfgNum('WeightCutForJuteBags')));
        if (p === 5) $('#txtEbUnit').val(cfgNum('WeightCutForOpenBulk'));
        bags.push({ purchaseorderid: r.PurchaseOrderId, typeid: type, itemid: r.ItemId, bagscondition: 0, receivedqty: 0,
                    purchaseqty: type === 1 ? 0 : (grn.gpQty > 0 ? grn.gpQty / 2 : 0), remarks: '' });
    }
    emptyBagRows = bags; emptyBagsDirty = true; drawEmptyBags();
}
/* combpckuom_Leave :5171 */
function combpckuomLeave() {
    total();
    if (grn.ref === 105) { totalWeightCalculationForMarketPurchase(); markeetPurchaseFormula(); total(); }
}
/* txtqty_TextChanged :5181 */
function txtqtyChanged() { totalWeightCalculationForMarketPurchase(); ebCalculations(); stockEbCalculations(); total(); }
/* txtgwt_Leave :5206 */
function txtgwtLeave() {
    if (grn.ref === 105) markeetPurchaseFormula();
    const gross = numeric('txtGrossWeight');
    if (numeric('cmbPackingType') === 5) { const pu = packUom(); $('#txtItemQty').val(Math.round(pu > 0 ? gross / pu : 0)); }
    if (grn.ref === 105) ebCalculations();
    total();
}
/* TotalWeightCalculationForMarketPurchase :4698 */
function totalWeightCalculationForMarketPurchase() {
    if (grn.ref !== 105) return;
    const pu = packUom(); $('#txtGrossWeight').val(pu > 0 ? pu * numeric('txtItemQty') : 0);
}
/* MarkeetPurchaseFormula :4769 */
function markeetPurchaseFormula() {
    const grnWt = numeric('txtGrnWeight'), pu = packUom();
    if (numeric('txtItemQty') === 0 && grnWt !== 0 && pu > 0) $('#txtItemQty').val(Math.round(grnWt / pu));
}
/* EbCalculations :4559 — source is the box the operator is typing in ("EbUnit"/"EbTotal" tags). */
function ebCalculations(source) {
    let unit = numeric('txtEbUnit'), tot = numeric('txtEbTotal'); const qty = numeric('txtItemQty');
    if (!(unit > 0 || tot > 0)) return;
    if (source === 'unit') $('#txtEbTotal').val(round(unit * qty, 2));
    else if (source === 'total') $('#txtEbUnit').val(round(qty > 0 ? tot / qty : 0, 3));
    else $('#txtEbTotal').val(round(unit > 0 && qty > 0 ? unit * qty : 0, 2));
}
/* StockEbCalculations :4728 */
function stockEbCalculations(source) {
    let unit = numeric('txtEbUnitStock'), tot = numeric('txtEbTotalStock'); const qty = numeric('txtItemQty');
    if (!(unit > 0 || tot > 0)) return;
    if (source === 'unit') $('#txtEbTotalStock').val(round(unit * qty, 2));
    else if (source === 'total') $('#txtEbUnitStock').val(round(qty > 0 ? tot / qty : 0, 3));
    else $('#txtEbTotalStock').val(round(unit > 0 && qty > 0 ? unit * qty : 0, 2));
}
/* WtCutCalculations :4474 (isTaxBox = the entry box; skipIndex = the row being edited). */
function wtCutCalculations(weightCut, qty, gross, cutOnId, cutOnQty, itemQtyInGrd, cutUom, itemId, skipIndex) {
    if (cutOnId > 0 && cutOnQty === 0) cutOnQty = lineItems.reduce((s, r, i) => s + (i === skipIndex ? 0 : Number(r.itemQty) || 0), 0) + qty;
    lineItems.forEach((r, i) => {
        if (Number(r.itemId) === itemId && i !== skipIndex) {
            const w = Number(r.wtCut) || 0; if (w !== 0) itemQtyInGrd += (Number(r.wtCutTotal) || 0) / w;
        }
    });
    if (cutOnId === 1) {
        const rem = cutOnQty - itemQtyInGrd;
        if (rem > 0 && qty <= rem) return qty * weightCut;
        if (rem > 0 && qty >= rem) return rem * weightCut;
        return 0;
    }
    if (cutOnId === 2) {
        const q = cutUom ? gross / cutUom : 0, rem = cutOnQty - itemQtyInGrd;
        if ((rem === 0 && q <= cutOnQty) || (rem > 0 && q <= rem)) return q * weightCut;
        if (rem > 0 && rem <= q) return rem * weightCut;
        return 0;
    }
    return qty * weightCut;
}
function breakupSupplierWeight() {
    /* 105 with a breakup: Σ breakup GrossWeight − |Σ EBTotal|, otherwise the Supplier Weight box. */
    if (grn.ref === 105 && breakupCount() > 0) return breakupSum('GrossWeight') - Math.abs(breakupSum('EBTotal'));
    return numeric('txtSupplierWeight');
}
function freightDeductionAmount() { return numeric('txtFreightDeduct') + numeric('txtChargeToParty'); }
/* The stock-weight rule shared by Total() and StockWeightCalculationInGrid(). */
function stockWeightFor(gross, stockEbTotal) {
    const fac = numeric('txtFactoryWeight'), sup = breakupSupplierWeight();
    if (!(sup > 0 && fac > 0 && gross > 0)) return null;
    if (cfgOn('DeductionPolicyForGrnIsOn')) return fac;
    const term = selectedText('cmbDeliveryTerm').trim(), diff = sup - (fac + cfgNum('BillWeightAndStockWeightDifferenceTolerance'));
    let useSupplier = true;
    if (term === 'Ponch & FactoryWeight' || term === 'Load & FactoryWeight') useSupplier = false;
    else if (term === 'Ponch') useSupplier = sup <= fac;
    if ((term === 'Load' || term === 'Load & PartyWeight' || term === 'Ponch & PartyWeight') && diff > 0 && grn.ref !== 105 && !cfgOn('ExcludeWeightShortageBusinessOnGrn'))
        useSupplier = freightDeductionAmount() > 0;
    const base = useSupplier ? sup : fac;
    return (base > 0 ? fac * gross / base : 0) - stockEbTotal;
}
/* Total :4600 */
function total() {
    const qty = numeric('txtItemQty'), gross = numeric('txtGrossWeight'), ebTotal = numeric('txtEbTotal');
    $('#txtAvgWeight').val(qty > 0 ? round(gross / qty, 2) : 0);
    let wtCutTotal = 0;
    if (String($('#txtWtCut').val() ?? '').trim() !== '') {
        const lab = grn.lab && numeric('cmbLabNo') > 0 ? grn.lab : null;
        wtCutTotal = wtCutCalculations(numeric('txtWtCut'), qty, gross, lab ? Number(lab.WtCutOnId) || 0 : 0, lab ? Number(lab.QtyForWtCut) || 0 : 0,
            0, lab ? Number(lab.WeightCutUom) || 0 : 0, numeric('cmbItem'), editLine);
    }
    $('#txtWtCutTotal').val(wtCutTotal);
    const addLess = numeric('txtAddLess');
    const net = grn.ref === 105 ? gross - Math.abs(addLess) : gross - ebTotal - wtCutTotal - addLess;
    $('#txtBillWeight').val(round(net, 2));
    const stock = stockWeightFor(gross, numeric('txtEbTotalStock'));
    if (stock !== null) $('#txtStockWeight').val(cfgOn('DeductionPolicyForGrnIsOn') ? stock : round(stock, 2));
}

/* WeightBusinessValidations(…, IsCalculating: true) :3137 — suggests GRN/Balance/Gross weight. */
async function weightBusinessCalc() {
    const sup = numeric('txtSupplierWeight'), fac = numeric('txtFactoryWeight'), received = numeric('txtReceivedWeight');
    const tol = cfgNum('BillWeightAndStockWeightDifferenceTolerance'), term = selectedText('cmbDeliveryTerm');
    const gridGross = gridSum(lineItems, 'grossWeight');
    if (cfgOn('DeductionPolicyForGrnIsOn') && grn.ref === 41 && term === 'Ponch') { await deductionPolicyForGrn(gridGross); return; }
    if (grn.ref === 105) {
        $('#txtGrnWeight').val(fmt2(sup)); $('#txtBalWeight').val(fmt2(sup - received));
        const pu = packUom(); $('#txtGrossWeight').val(pu > 0 ? pu * numeric('txtItemQty') : 0);
        return;
    }
    const diff = sup - (fac + tol), it = selectedItem(), itemWeight = it ? Number(it.ItemWbWeight) || 0 : 0;
    const supSuggest = () => { $('#txtGrnWeight').val(fmt0(sup)); $('#txtBalWeight').val(fmt0(sup)); $('#txtGrossWeight').val(fmt0(itemWeight > 0 ? itemWeight / fac * sup : sup)); };
    const facSuggest = () => { $('#txtGrnWeight').val(fmt0(fac)); $('#txtBalWeight').val(fmt0(fac)); $('#txtGrossWeight').val(fmt0(itemWeight > 0 ? itemWeight : fac)); };
    if (grn.freightId !== 0 && diff < 0 && !cfgOn('ExcludeWeightShortageBusinessOnGrn')) supSuggest();
    else {
        /* ValidateWeights :3361, calculating branch */
        const party = term === 'Load' || term === 'Load & PartyWeight' || term === 'Ponch & PartyWeight';
        const factory = term === 'Load & FactoryWeight' || term === 'Ponch & FactoryWeight';
        if (party) { if (diff > 0 && !(freightDeductionAmount() > 0 || cfgOn('ExcludeWeightShortageBusinessOnGrn'))) facSuggest(); else supSuggest(); }
        else if (term === 'Ponch') { if (sup > fac) facSuggest(); else supSuggest(); }
        else if (factory) facSuggest();
    }
    /* Tail of WeightBusinessValidations: gross = item WB weight (scaled) − what the grid already holds. */
    const itemId = numeric('cmbItem'), totalGross = numeric('txtGrnWeight');
    let itemTotal = grn.items.filter(i => Number(i.ItemId) === itemId).reduce((s, i) => s + (Number(i.ItemWbWeight) || 0), 0);
    if (totalGross === sup && fac) itemTotal = itemTotal / fac * sup;
    let grd = lineItems.reduce((s, r, i) => s + (Number(r.itemId) === itemId && i !== editLine ? Number(r.grossWeight) || 0 : 0), 0);
    if (itemTotal === 0) { itemTotal = totalGross; grd = lineItems.reduce((s, r, i) => s + (i !== editLine ? Number(r.grossWeight) || 0 : 0), 0); }
    $('#txtGrossWeight').val(fmt2(itemTotal - grd));
}
/* DeductionPolicyForGrn :3305 */
async function deductionPolicyForGrn(gridGross, refuse) {
    const fac = numeric('txtFactoryWeight'), sup = numeric('txtSupplierWeight'), diff = fac - sup;
    let policy = 0, name = '';
    if (diff > 0) {
        const p = await api('/api/purchase/market-grn/form/deduction-policy?date=' + encodeURIComponent(textValue('txtDocDate')) + '&difference=' + diff);
        policy = Number(p.PolicyTypeId) || 0; if (policy) name = ' Policy is ' + (p.ConditionDescription || '');
    }
    const finalWt = policy === 2 ? fac - diff / 2 : policy === 3 ? sup : fac;
    $('#txtBalWeight').val(finalWt); showWeightFields(true);
    const grd = lineItems.reduce((s, r, i) => s + (i !== editLine ? Number(r.grossWeight) || 0 : 0), 0);
    $('#txtGrossWeight').val(finalWt - grd);
    if (refuse && gridGross > 0 && gridGross !== finalWt) throw new Error('GrossWeight ' + gridGross + ' Should Equal To FinalWeight ' + finalWt + '...' + name);
}

/* ---------------- header events ---------------- */
/* TotalSupplierWeight :5311 then the shortage proportions (txtsuppwt/txtfctwt_TextChanged) */
function supplierFactoryChanged() {
    const diff = numeric('txtSupplierWeight') - numeric('txtFactoryWeight');
    $('#txtDiffWeight').val(round(diff, 2));
    $('#lblDiffWeight').text(diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal');
    weightBusinessCalc();
    scaleShortageProportion(); supplierShortageProportion(); renderItemsGrid();
    recalculateBreakupHeader();
}
function calcWeights() { supplierFactoryChanged(); }
/* txtDeliverTerm_ValueChanged :1460 */
function txtDeliverTermChanged() { weightBusinessCalc(); total(); stockWeightCalculationInGrid(); renderItemsGrid(); }
/* FreightCalculations :5704 */
function freightCalculations() {
    const totalFreight = numeric('txtBiltyFreight') + numeric('txtScaleCharges') - (numeric('txtAdvParty') + numeric('txtAdvFactory')) - numeric('txtOtherDeduct');
    $('#txtTotalFreight').val(round(totalFreight, 2));
    if (grn.freightId === 0) $('#txtPaidAmount').val(round(totalFreight - numeric('txtFreightDeduct'), 2));
    total(); stockWeightCalculationInGrid(); freightProportion(); renderItemsGrid();
}
function calcFreight() { freightCalculations(); }

/* ---------------- grid-wide calculations ---------------- */
/* ScaleShortageProportion :5570 */
function scaleShortageProportion() {
    const shortage = numeric('txtDiffWeight'), qtyTotal = breakupCount() > 0 ? breakupSum('Qty') : 0;
    for (const r of lineItems) r.scaleShortWeight = shortage > 0 && qtyTotal > 0 ? shortage / qtyTotal * (Number(r.itemQty) || 0) : 0;
}
/* SupplierShortageProportion :5528 */
function supplierShortageProportion() {
    const shortage = (breakupCount() > 0 ? breakupSum('GrossWeight') : 0) - numeric('txtSupplierWeight'), qtyTotal = breakupSum('Qty');
    for (const r of lineItems) r.supplierShortWeight = shortage > 0 && qtyTotal > 0 ? shortage / qtyTotal * (Number(r.itemQty) || 0) : 0;
}
/* FreightProportion :5633 */
function freightProportion() {
    const freight = numeric('txtPaidAmount'), qtyTotal = breakupCount() > 0 ? breakupSum('Qty') : 0;
    for (const r of lineItems) r.freightAmount = freight > 0 && qtyTotal > 0 ? freight / qtyTotal * (Number(r.itemQty) || 0) : 0;
}
/* TotalEbPurchaseAgainstWeightUtilizeInGrid :5080 */
function totalEbPurchaseAgainstWeightUtilizeInGrid() {
    let remaining = (typeof emptyBagRows !== 'undefined' ? emptyBagRows : []).filter(r => Number(r.typeid) === 2).reduce((s, r) => s + (Number(r.purchaseqty) || 0), 0);
    if (remaining <= 0 || gridSum(lineItems, 'itemQty') <= 0) return;
    for (const r of lineItems) {
        const apply = Math.min(remaining, Number(r.itemQty) || 0), weight = apply * (Number(r.ebwPerUnit) || 0);
        r.ebPurAgainstWeight = weight;
        r.netBillWeight = (Number(r.grossWeight) || 0) - (Number(r.ebwTotal) || 0) - (Number(r.wtCutTotal) || 0) - (Number(r.adLsWeight) || 0) + weight;
        remaining -= apply; if (remaining <= 0.0001) break;
    }
}
/* StockWeightCalculationInGrid :5011 */
function stockWeightCalculationInGrid() {
    for (const r of lineItems) {
        const s = stockWeightFor(Number(r.grossWeight) || 0, Number(r.stockEbTotal) || 0);
        r.stockWeight = s === null ? 0 : s;
    }
}
function afterGridChange() {
    scaleShortageProportion(); supplierShortageProportion(); totalEbPurchaseAgainstWeightUtilizeInGrid(); freightProportion(); renderItemsGrid();
}

/* ---------------- detail rows ---------------- */
/* FormValidationDetail :1518 */
function formValidationDetail() {
    if (grn.ref === 41 && grn.purchaseOrderId === 0) return 'PurchaseOrderNo Not Found.Please Check';
    if (!numeric('cmbItem')) return 'Item Name Field is Required';
    if (!numeric('cmbCropYear')) return 'Crop Year Field is Required';
    if (!numeric('cmbJobLot')) return 'Job/Lot Field is Required';
    if (!numeric('cmbPackingType')) return 'Packing Type Field is Required';
    if (numeric('txtItemQty') === 0) return 'Qty Field is Required';
    if (!numeric('cmbUom')) return 'Pack Unit Field is Required';
    if ((grn.labRows || []).length && numeric('cmbLabNo') === 0) return 'Lab No Field is Required';
    if (numeric('txtGrossWeight') === 0) return 'Gross Weight Field is Required';
    if (numeric('txtBillWeight') === 0) return 'Net Bill Weight Field is Required';
    if (numeric('txtStockWeight') === 0) return 'Stock Weight Field is Required';
    if (!numeric('cmbWarehouse')) return 'Warehouse Field is Required';
    if (!numeric('cmbCity')) return 'City Field is Required';
    return '';
}
/* Add_Click :1872 / btnUpdateDetail_Click :1962 */
function btnAddLine_Click() {
    try {
        if (cfgOn('DeductionPolicyForGrnIsOn') && lineItems.length === 1 && editLine < 0) throw new Error('Adding multiple rows is not allowed while the deduction policy is active.');
        if (grn.ref === 105 && breakupCount() > 0) {
            const pu = packUom();
            if (!purchaseBreakupRows.some(r => Number(r.netpacksize) === pu)) throw new Error('UOM ' + selectedText('cmbUom') + ' does not exist in BreakUp Grid');
        }
        const invalid = formValidationDetail(); if (invalid) throw new Error(invalid);
        if (grnUomEquivalent() === null) throw new Error('The selected item UOM has no valid weight factor.');
        if (numeric('cmbPackingType') === 5 && grnUomEquivalent() !== 65 && !confirm("Are you sure to Add Entry, because Packing type is 'OPEN BULK' and Pack Uom not 65?")) return;
        const net = numeric('txtBillWeight'), gross = numeric('txtGrossWeight');
        if (net > gross) throw new Error('NetBillWeight cannot be greater than Gross Weight. Please check.');
        const it = selectedItem();
        const lab = grn.lab && numeric('cmbLabNo') > 0 ? grn.lab : null;
        if (lab && $('#chkWtCutComp').prop('checked') && numeric('txtWtCut') === 0 && !confirm('Weight Cut is compulsory but the value is zero. Do you want to continue?')) return;
        let ebUnit = numeric('txtEbUnit'), ebTotal = numeric('txtEbTotal');
        if (grn.ref !== 105 && emptyBagRows.some(r => Number(r.typeid) === 3) && ebTotal === 0) {
            const typeName = $('#grnBagTypeOptions').prop('content')?.querySelector('option[value="3"]')?.textContent || '3';
            throw new Error('EmptyBags Weight is required when Empty Bags Case is ' + typeName);
        }
        const qty = numeric('txtItemQty');
        if (ebUnit > 0 && ebTotal === 0) { ebTotal = qty * ebUnit; $('#txtEbTotal').val(ebTotal); }
        else if (ebUnit === 0 && ebTotal > 0) { ebUnit = ebTotal / qty; $('#txtEbUnit').val(ebUnit); }
        const line = {
            ...(editLine >= 0 ? lineItems[editLine] : { id: 0 }),
            purchaseOrderDetailId: grn.ref === 41 && it ? Number(it.PoDetailId) || 0 : 0, purchaseOrderId: grn.purchaseOrderId, purchaserOrderNo: grn.purchaseOrderNo,
            warehouseId: numeric('cmbWarehouse'), whName: selectedText('cmbWarehouse'),
            itemId: numeric('cmbItem'), itemCode: it ? it.ItemCode || '' : '', itemName: it ? it.ItemName : selectedText('cmbItem'),
            cropYearId: numeric('cmbCropYear'), cropYear: selectedText('cmbCropYear'), jobLotId: numeric('cmbJobLot'), itemCategory: selectedText('cmbJobLot'),
            packingTypeId: numeric('cmbPackingType'), packingType: selectedText('cmbPackingType'),
            itemUomId: numeric('cmbUom'), uom: selectedText('cmbUom'), uomEquivalent: grnUomEquivalent(),
            itemQty: qty, qty, grossWeight: gross, ebwPerUnit: ebUnit, ebwTotal: ebTotal, ebPurAgainstWeight: 0,
            labId: lab ? Number(lab.Id) : 0, labReportRef: lab ? String(lab.LabNo ?? '') : '', qtyForWtCut: lab ? Number(lab.QtyForWtCut) || 0 : 0,
            weightCutOnId: lab ? Number(lab.WtCutOnId) || 0 : 0, wtCutOn: lab ? lab.WtCutOn || '' : '', weightCutUom: lab ? Number(lab.WeightCutUom) || 0 : 0,
            wtCut: numeric('txtWtCut'), wtCutTotal: numeric('txtWtCutTotal'), adLsWeight: numeric('txtAddLess'),
            scaleShortWeight: 0, supplierShortWeight: 0, netBillWeight: net,
            stockEbUnit: numeric('txtEbUnitStock'), stockEbTotal: numeric('txtEbTotalStock'), stockWeight: numeric('txtStockWeight'),
            cityId: numeric('cmbCity'), areaCity: selectedText('cmbCity'), freightAmount: 0
        };
        if (editLine >= 0) lineItems[editLine] = line; else lineItems.push(line);
        $('#txtVehicleNo').prop('disabled', false);
        resetDetail();
        afterGridChange();
    } catch (error) { alert(error.message); }
}
/* ResetDetial :4393 */
function resetDetail() {
    editLine = -1; $('#btnAddLine').text('+ Add'); $('#btnCancelLine').hide();
    for (const id of ['txtItemQty', 'txtGrossWeight', 'txtEbTotal', 'txtWtCut', 'txtWtCutTotal', 'txtAddLess', 'txtBillWeight', 'txtStockWeight', 'txtAvgWeight', 'txtEbTotalStock']) $('#' + id).val(0);
    $('#cmbItem').val('0'); narrowUomToItem(); $('#cmbLabNo').empty(); grn.lab = null; grn.labRows = [];
}
function editItemLine(index) {
    const line = lineItems[index]; if (!line) return; editLine = index;
    for (const [field, key] of Object.entries({ cmbItem: 'itemId', cmbWarehouse: 'warehouseId', cmbCropYear: 'cropYearId', cmbJobLot: 'jobLotId', cmbPackingType: 'packingTypeId', cmbCity: 'cityId' })) selectValue(field, line[key]);
    narrowUomToItem(); selectValue('cmbUom', line.itemUomId, line.uom);
    for (const [field, key] of Object.entries({ txtItemQty: 'itemQty', txtGrossWeight: 'grossWeight', txtEbUnit: 'ebwPerUnit', txtEbTotal: 'ebwTotal', txtWtCut: 'wtCut', txtWtCutTotal: 'wtCutTotal', txtAddLess: 'adLsWeight', txtBillWeight: 'netBillWeight', txtEbUnitStock: 'stockEbUnit', txtEbTotalStock: 'stockEbTotal', txtStockWeight: 'stockWeight' })) $('#' + field).val(line[key] ?? 0);
    grn.labRows = line.labId ? [{ Id: line.labId, LabNo: line.labReportRef, QtyForWtCut: line.qtyForWtCut, WtCut: line.wtCut, WtCutOnId: line.weightCutOnId, WtCutOn: line.wtCutOn, WeightCutUom: line.weightCutUom }] : [];
    $('#cmbLabNo').empty().append(...(grn.labRows.length ? grn.labRows.map(r => new Option(String(r.LabNo), r.Id)) : [new Option('', '0')]));
    $('#cmbLabNo').val(String(line.labId || 0)); grn.lab = grn.labRows[0] || null; $('#txtWtCutOn').val(line.wtCutOn || '');
    $('#btnAddLine').text('Update row'); $('#btnCancelLine').show();
}
function cancelEditLine() { resetDetail(); }
function removeLine(idx) {
    if (!confirm('Delete this detail row?')) return;
    resetDetail(); lineItems.splice(idx, 1); afterGridChange();
}

const GRID_COLUMNS = [
    ['whName', 'text'], ['itemCode', 'text'], ['itemName', 'text'], ['cropYear', 'text'], ['itemCategory', 'text'], ['packingType', 'text'], ['uom', 'text'],
    ['itemQty', 'sum'], ['grossWeight', 'sum'], ['ebwPerUnit', 'num'], ['ebwTotal', 'sum'], ['ebPurAgainstWeight', 'sum'], ['labReportRef', 'text'],
    ['qtyForWtCut', 'num'], ['wtCutOn', 'text'], ['weightCutUom', 'num'], ['wtCut', 'num'], ['wtCutTotal', 'sum'], ['adLsWeight', 'sum'],
    ['scaleShortWeight', 'sum'], ['supplierShortWeight', 'sum'], ['netBillWeight', 'sum'], ['stockEbUnit', 'num'], ['stockEbTotal', 'sum'],
    ['stockWeight', 'sum'], ['areaCity', 'text'], ['freightAmount', 'sum']];
function cellNumber(v) { const n = Number(v) || 0; return String(round(n, 3)); }
function renderItemsGrid() {
    const tbody = $('#grdItemsBody'); tbody.empty();
    const span = GRID_COLUMNS.length + 2;
    if (!lineItems.length) tbody.html('<tr><td colspan="' + span + '" class="text-center text-muted py-2">No received items added yet.</td></tr>');
    lineItems.forEach((item, idx) => {
        tbody.append('<tr><td class="text-center"><button class="btn btn-sm btn-danger py-0 px-1" onclick="removeLine(' + idx + ')">&times;</button><button class="tool-btn" onclick="editItemLine(' + idx + ')">Edit</button></td>'
            + '<td>' + (item.purchaseOrderId ? '<a href="/purchase/purchase-order?id=' + Number(item.purchaseOrderId) + '">' + escapeHtml(item.purchaserOrderNo || 'Order') + '</a>' : '') + '</td>'
            + GRID_COLUMNS.map(([k, t]) => t === 'text' ? '<td>' + escapeHtml(item[k]) + '</td>' : '<td class="text-end">' + cellNumber(item[k]) + '</td>').join('') + '</tr>');
    });
    $('#grdItemsFoot').html('<tr class="fw-bold bg-light"><td colspan="2" class="text-end">Summary:</td>' + GRID_COLUMNS.map(([k, t]) => t === 'sum' ? '<td class="text-end">' + cellNumber(gridSum(lineItems, k)) + '</td>' : '<td></td>').join('') + '</tr>');
}

/* ---------------- pending gate passes ---------------- */
function fillCountFromGpGrid() {
    const statuses = Array.from(document.querySelectorAll('[data-gp-status]')).map(row => row.dataset.gpStatus);
    $('#pendingGpCounts').text('Open Gp: ' + statuses.filter(s => s === 'Open').length + ' | Accepted Gp: ' + statuses.filter(s => s === 'Accepted').length);
}
async function refreshPendingGrid() {
    try {
        const rows = await api('/api/purchase/market-grn/form/pending');
        $('#grdPendingBody').html(rows.map(g => '<tr data-gp-status="' + escapeHtml(g.Status) + '"><td>' + escapeHtml(g.Status) + '</td><td><button class="tool-btn" data-gp-id="' + Number(g.Id) + '" onclick="withButtonLoading(this,()=>loadGatePass(this.dataset.gpId))">Load</button></td>'
            + '<td><a href="/purchase/inward-gate-pass?id=' + Number(g.Id) + '">' + escapeHtml(g.GpSrNo) + '</a></td>'
            + ['FVStatus', 'GpDate', 'OrderType', 'SupplierName', 'VehicleNo', 'BiltyNo', 'ItemName', 'Qty', 'SupplierWeight', 'FactoryWeight', 'ReceivedWeight', 'PoAccessWeight'].map(k => '<td>' + escapeHtml(k === 'GpDate' ? displayDate(g[k]) : g[k]) + '</td>').join('') + '</tr>').join(''));
        const sel = $('#cmbGatePassNo'), cur = sel.val(); sel.empty().append(new Option('-- Select --', ''));
        for (const g of rows) sel.append(new Option(String(g.GpSrNo), g.Id));
        if (cur && rows.some(g => String(g.Id) === cur)) sel.val(cur);
        fillCountFromGpGrid();
    } catch (e) { /* the grid keeps its server-rendered rows */ }
}

/* LoadGPByRow :2716 + combgatepass_Leave :2961 (GatePassRecordFill :2852, FillWeightsAndItems :2929) */
async function loadGatePass(id) {
    if (!Number(id)) return;
    if (numeric('txtId') > 0) { alert('Please Reset the form First...'); return; }
    if (lineItems.length && !confirm('Replace the selected gate pass and clear detail rows?')) return;
    try {
        const version = ++lookupVersion;
        const data = await api('/api/purchase/market-grn/form/load/' + Number(id)); if (version !== lookupVersion) return;
        const p = data.pending || {}, gp = data.gatePass;
        loadedHeader = {}; lineItems = []; editLine = -1; $('#txtId').val(0); $('#btnSave').text('Save');
        grn.ref = Number(data.refDocumentTypeId) || 0; grn.gpId = Number(id); grn.freightId = Number(p.FreightId) || 0;
        grn.gpQty = Number(p.Qty) || 0; grn.gpDate = displayDate(p.GpDate); grn.chargeToParty = Number(p.ChargeToPartyAmount) || 0;
        grn.previousData = data.previousData || []; grn.preBills = data.preBills || []; grn.items = data.items || [];
        $('#grnTitle').text('Goods Receiving Notes (' + (p.OrderType || '') + ')');
        renderSupplements(data); breakupDirty = grn.ref === 105 && !breakupLocked;
        /* freight block from the pending row */
        $('#txtBiltyFreight').val(p.NetPaid ?? 0); $('#txtScaleCharges').val(p.ScalCharges ?? 0);
        $('#txtAdvParty').val(p.AdvanceByParty ?? 0); $('#txtAdvFactory').val(p.AdvanceByFactory ?? 0);
        $('#txtOtherDeduct').val(p.OtherDeduction ?? 0); $('#txtTotalFreight').val(p.TotalFreight ?? 0);
        $('#txtShortageAmt').val(p.ShortageAmount ?? 0); $('#txtDiscountAmt').val(p.DiscountAmount ?? 0);
        $('#txtRemainingFreight').val(p.RemainingAmount ?? 0); $('#txtChargeToParty').val(p.ChargeToPartyAmount ?? 0);
        $('#txtFreightDeduct').val(p.TotalDeductionAmount ?? 0); $('#txtPaidAmount').val(p.TotalPaidAmount ?? 0);
        $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct').prop('disabled', grn.freightId !== 0);
        if (Number(p.FreightDebitAccountId) > 0) $('#cmbTransporter').val(String(p.FreightDebitAccountId));
        $('#txtReceivedWeight').val(p.ReceivedWeight ?? 0); $('#txtAccessWeight').val(p.PoAccessWeight ?? 0);
        $('#txtSupplierWeight').prop('readonly', grn.ref !== 105 || Number(p.ReceivedWeight) !== 0);
        showWeightFields(grn.ref === 105);
        $('#cmbDeliveryTerm').prop('disabled', !(grn.ref === 106 && cfgOn('DeliveryTermEditableOnGrnForGatePurchase')));
        if (cfgOn('ValidateGrnAndInvoiceDateWithGpDate')) $('#txtDocDate').val(grn.gpDate);
        selectValue('cmbGatePassNo', id, p.GpSrNo);
        if (grn.freightId === 0 && grn.ref === 105) { $('#txtAdvFactory,#txtAdvParty').prop('disabled', true).val(0); }
        /* GatePassRecordFill */
        const locked = !!gp;
        $('#cmbVehicleType').prop('disabled', locked); $('#txtVehicleNo,#txtBiltyNo').prop('disabled', locked);
        if (gp) {
            selectByText('cmbVehicleType', gp.VehicleType); $('#txtVehicleNo').val(gp.VehicleNo || ''); $('#txtBiltyNo').val(gp.BiltyNo || '');
            $('#txtRemarks').val(gp.OtherRemarks || ''); $('#txtItemQty').val(gp.NoOfPackages ?? 0);
            $('#txtSupplierWeight').val(gp.SupplierWeight ?? 0); $('#txtFactoryWeight').val(gp.FactoryWeight ?? 0); $('#txtDiffWeight').val(gp.DifferenceWeight ?? 0);
            $('#txtAnalystName').val(gp.AnalystName || ''); $('#txtWbTickets').val(gp.TicketNos || '');
            selectValue('cmbSupplier', gp.SupplierCustomerId, gp.CompanyName);
            if (grn.ref === 41) {
                grn.purchaseOrderId = Number(gp.PurchaseOrderId) || 0; grn.purchaseOrderNo = Number(gp.SupplierContractCode) || 0;
                /* comborderno_Leave :1346 */
                const po = data.purchaseOrder || {}; grn.poEmptyBags = po.emptyBags || [];
                purchaseOrderEmptyBags();
                const d = (po.detail || [])[0];
                if (d) { selectByText('cmbCity', d.CityArea); selectByText('cmbDeliveryTerm', d.DeliveryTerm); selectValue('cmbUom', d.OrderItemUOMId); }
            } else {
                grn.purchaseOrderId = 0; grn.purchaseOrderNo = 0; grn.poEmptyBags = [];
                /* FillWeightsAndItems :2929 */
                const sup = numeric('txtSupplierWeight'), fac = numeric('txtFactoryWeight'), rec = numeric('txtReceivedWeight');
                if (grn.ref === 105) { selectByText('cmbDeliveryTerm', 'Load'); $('#txtGrnWeight').val(sup); $('#txtBalWeight').val(sup - rec); $('#txtGrossWeight').val(sup - rec); }
                else if (grn.ref === 106) { const b = sup > fac ? fac : sup; selectByText('cmbDeliveryTerm', 'Ponch'); $('#txtGrnWeight').val(b); $('#txtBalWeight').val(b); $('#txtGrossWeight').val(b); }
            }
        } else { grn.purchaseOrderId = 0; grn.purchaseOrderNo = 0; }
        itemNameBind();
        fillPreBills(gp ? gp.SupplierDispatchId : 0);
        if (grn.ref === 41 && gp) $('#cmbPreBillNo').val(String(gp.SupplierDispatchId || 0));
        transporterAccountDisable(); showShortageCheckBoxes();
        const diff = numeric('txtSupplierWeight') - numeric('txtFactoryWeight'); $('#lblDiffWeight').text(diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal');
        total(); renderItemsGrid();
        if (breakupDirty) await calculatePurchaseBreakups();
        const next = await api('/api/purchase/market-grn/next-code'); if (version === lookupVersion) $('#txtDocNo').val(next.docNo);
    } catch (error) { alert(error.message); }
}

/* ---------------- save / load / reset ---------------- */
async function onSaveRecord() {
    const count = lineItems.length;
    if (!count) { alert('Grid Record Not Found'); return; }
    if (grn.ref === 105) {
        const scaleShort = gridSum(lineItems, 'scaleShortWeight');
        if (scaleShort > 0 && !$('#chkScaleDeduct').prop('checked') && !confirm('You have not Apply Scale_Short_Weight Deduction In bill Weight...Are you sure to Continue?')) return;
        if (scaleShort > 0 && !$('#chkSupplierDeduct').prop('checked') && !confirm('You have not Apply Supplier_Short_Weight Deduction In bill Weight...Are you sure to Continue?')) return;
    }
    if (!confirm(numeric('txtId') > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
    stockWeightCalculationInGrid(); renderItemsGrid();
    const payload = { ...await supplementPayload(), id: numeric('txtId'), docNo: numeric('txtDocNo'), docDate: textValue('txtDocDate'), supplierCustomerId: numeric('cmbSupplier'),
        inwardGatePassId: grn.gpId || numeric('cmbGatePassNo'), gpNo: Number(selectedText('cmbGatePassNo')) || loadedHeader.GpNo || 0,
        vehicleNo: textValue('txtVehicleNo'), vehicleType: selectedText('cmbVehicleType'), biltyNo: textValue('txtBiltyNo'), remarksHeader: textValue('txtRemarks'),
        partyWeight: numeric('txtSupplierWeight'), factoryWeight: numeric('txtFactoryWeight'), deliveryTerm: selectedText('cmbDeliveryTerm'),
        transporterId: numeric('cmbTransporter'), transporterSupCustId: grn.subsidiary ? Number($('#cmbTransporter option:selected').attr('data-party-id')) || 0 : 0,
        carriageAmount: numeric('txtPaidAmount'), biltyFreight: numeric('txtBiltyFreight'), freightDeduction: numeric('txtFreightDeduct'),
        advanceByPartyFreight: numeric('txtAdvParty'), advanceByFactoryFreight: numeric('txtAdvFactory'), supplierDispatchId: numeric('cmbPreBillNo'),
        scaleCharges: numeric('txtScaleCharges'), otherFreightDeduction: numeric('txtOtherDeduct'), shortageAmountFV: numeric('txtShortageAmt'),
        discountAmountFV: numeric('txtDiscountAmt'), chargeToPartyAmountFV: numeric('txtChargeToParty'),
        scaleShortWeightApply: $('#chkScaleDeduct').prop('checked'), supplierShortWeightApply: $('#chkSupplierDeduct').prop('checked'), details: lineItems };
    const data = await api('/api/purchase/market-grn/save', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
    alert((numeric('txtId') > 0 ? 'Record Update Successfully [' : 'Record Save Successfully [') + (data.docNo ?? payload.docNo) + ']');
    /* After save: wages bill when ContractorWagesCompulsoryBeforeInvoices and GRN is active for wages (:3920). */
    if (cfgOn('ContractorWagesCompulsoryBeforeInvoices') && grn.wages)
        alert('Contractor Wages Bill is compulsory for this GRN (Id ' + data.id + '). Please enter it on the Labour Wages screen before the invoice.');
    if ($('#chkPreviewAfterSave').prop('checked')) window.open('/purchase/reports/grn?id=' + Number(data.id), '_blank');
    await onNewRecord(); refreshPendingGrid();
}

function resetGrnState() {
    Object.assign(grn, { ref: 0, freightId: 0, gpId: 0, gpDate: '', gpQty: 0, purchaseOrderId: 0, purchaseOrderNo: 0, items: [], lab: null, labRows: [], previousData: [], poEmptyBags: [], preBills: [], chargeToParty: 0 });
}
async function onNewRecord() {
    lookupVersion++; loadedHeader = {}; lineItems = []; editLine = -1; resetGrnState(); renderSupplements({});
    document.querySelectorAll('#viewForm input').forEach(input => { if (input.type === 'radio') return; if (input.type === 'checkbox') input.checked = false; else if (input.type === 'number' || input.id === 'txtId') input.value = 0; else input.value = ''; });
    document.querySelectorAll('#viewForm select').forEach(select => { $(select).val('0'); });
    $('#cmbItem').empty().append(new Option('-- Select Item --', '0')); $('#cmbLabNo,#cmbPreBillNo').empty();
    $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct,#txtVehicleNo,#txtBiltyNo,#cmbVehicleType').prop('disabled', false);
    $('#labParamsPanel').hide(); $('#grnTitle').text('Goods Receiving Notes');
    narrowUomToItem(); renderItemsGrid(); applyConfigToControls(); applyConfigDefaults(); showShortageCheckBoxes();
    $('#txtDocDate').val(new Date().toLocaleDateString('en-CA')); $('#btnSave').text('Save'); $('#btnAddLine').text('+ Add'); $('#btnCancelLine').hide();
    const next = await api('/api/purchase/market-grn/next-code'); $('#txtDocNo').val(next.docNo);
}
function onRefreshForm() { return onNewRecord().then(refreshPendingGrid); }

function switchMode(mode) {
    if (mode === 'History') {
        $('#viewForm').hide(); $('#viewHistory').show(); $('#btnModeForm').removeClass('active'); $('#btnModeHistory').addClass('active');
        return loadHistory();
    }
    $('#viewForm').show(); $('#viewHistory').hide(); $('#btnModeForm').addClass('active'); $('#btnModeHistory').removeClass('active');
}
async function loadHistory() {
    try {
        const body = { fromDate: textValue('txtHistoryFrom') || null, toDate: textValue('txtHistoryTo') || null, supplierId: numeric('cmbHistorySupplier') || null,
                       fromDocNo: numeric('txtHistoryFromNo') || null, toDocNo: numeric('txtHistoryToNo') || null };
        const data = await api('/api/purchase/market-grn/history', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
        $('#grdHistoryBody').html(data.map(row => '<tr><td><a href="/purchase/goods-receipt-notes?id=' + Number(row.id) + '" onclick="event.preventDefault();loadRecord(' + Number(row.id) + ')">' + escapeHtml(row.docNo) + '</a></td>' + ['docDate', 'supplierName', 'vehicleNo', 'biltyNo', 'remarks', 'entryDate'].map(key => '<td>' + escapeHtml(row[key]) + '</td>').join('') + '<td><button class="tool-btn" onclick="loadRecord(' + Number(row.id) + ')">Edit</button></td></tr>').join(''));
    } catch (error) { alert(error.message); }
}
/* FilldtDetailFromListCommonForReadById :4161 — the stored row back into the grid's shape. */
function normalizeLine(row) {
    const m = {}; for (const [k, v] of Object.entries(row)) m[k.charAt(0).toLowerCase() + k.slice(1)] = v;
    const n = k => Number(row[k] ?? m[k.charAt(0).toLowerCase() + k.slice(1)]) || 0;
    return { ...m, id: n('Id'), itemId: n('ItemId'), warehouseId: n('WarehouseId'), itemUomId: n('ItemUomId'), cropYearId: n('CropYearId'), jobLotId: n('JobLotId'),
        packingTypeId: n('PackingTypeId'), cityId: n('CityId'), purchaseOrderId: n('PurchaseOrderId'), purchaseOrderDetailId: n('PurchaseOrderDetailId'),
        itemQty: n('ItemQty'), qty: n('ItemQty'), grossWeight: n('GrossWeight'), ebwPerUnit: n('EBWPerUnit'), ebwTotal: n('EBWTotal'), ebPurAgainstWeight: n('EbPurAgainstWeight'),
        labId: n('LabId'), labReportRef: row.LabReportRef ?? m.labReportRef ?? '', qtyForWtCut: n('QtyForWtCut'), weightCutOnId: n('WeightCutOnId'),
        wtCutOn: row.WtCutOn ?? row.WeightCutOn ?? '', weightCutUom: n('WeightCutUom'), wtCut: n('WtCut'), wtCutTotal: n('WtCutTotal'), adLsWeight: n('AdLsWeight'),
        scaleShortWeight: n('ScaleShortWeight'), supplierShortWeight: n('SupplierShortWeight'), netBillWeight: n('NetBillWeight'),
        stockEbUnit: n('StockEbUnit'), stockEbTotal: n('StockEbTotal'), stockWeight: n('StockWeight'), freightAmount: n('FreightAmount'),
        whName: row.WareHouseName ?? row.WarehouseName ?? '', itemCode: row.ItemCode ?? '', itemName: row.ItemName ?? '', cropYear: row.CropYear ?? '',
        itemCategory: row.JobLotDescription ?? row.JobLot ?? '', packingType: row.PackingType ?? row.PackTypeDesc ?? '', uom: row.UOMCode ?? '',
        uomEquivalent: n('UOMEquivalent') || n('UOM'), areaCity: row.AreaCity ?? row.CityName ?? '', purchaserOrderNo: row.PurchaserOrderNo ?? '' };
}
/* ReadById :3972 */
async function loadRecord(id, propagate = false) {
    try {
        const version = ++lookupVersion; const data = await api('/api/purchase/market-grn/' + Number(id)); if (version !== lookupVersion) return;
        resetGrnState();
        grn.ref = Number(data.RefDocumentTypeId) || 0; grn.freightId = Number(data.FreightId) || 0; grn.gpId = Number(data.InwardGatePassId) || 0;
        renderSupplements(data); loadedHeader = data; lineItems = (data.details || []).map(normalizeLine); editLine = -1;
        for (const [field, key] of Object.entries({ txtId: 'Id', txtDocNo: 'DocNo', txtVehicleNo: 'VehicleNo', txtBiltyNo: 'BiltyNo', txtRemarks: 'RemarksHeader', txtSupplierWeight: 'PartyWeight', txtFactoryWeight: 'FactoryWeight',
            txtPaidAmount: 'CarriageAmount', txtBiltyFreight: 'BiltyFreight', txtFreightDeduct: 'FreightDeduction', txtAdvParty: 'AdvanceByPartyFreight', txtAdvFactory: 'AdvanceByFactoryFreight',
            txtScaleCharges: 'ScaleCharges', txtOtherDeduct: 'OtherFreightDeduction', txtShortageAmt: 'ShortageAmountFV', txtDiscountAmt: 'DiscountAmountFV', txtChargeToParty: 'ChargeToPartyAmountFV' }))
            $('#' + field).val(data[key] ?? (field.startsWith('txtId') ? 0 : ''));
        $('#txtRemainingFreight').val((Number(data.ShortageAmountFV) || 0) - (Number(data.DiscountAmountFV) || 0));
        if (grn.ref === 105) $('#txtPaidAmount').val(data.OtherCharges ?? 0);
        $('#txtDocDate').val(displayDate(data.DocDate)); selectValue('cmbSupplier', data.SupplierCustomerId, data.SupplierCustomer || data.SupplierName);
        selectValue('cmbGatePassNo', data.InwardGatePassId, data.GpNo); selectByText('cmbVehicleType', data.VehicleType);
        selectValue('cmbTransporter', data.TransporterId, data.Transporter); selectByText('cmbDeliveryTerm', data.DeliveryTerm);
        $('#chkScaleDeduct').prop('checked', !!data.ScaleShortWeightApply); $('#chkSupplierDeduct').prop('checked', !!data.SupplierShortWeightApply);
        $('#grnTitle').text('Goods Receiving Notes (' + (data.TransType || '') + ')');
        if (grn.freightId === 0 && grn.ref === 105) $('#txtAdvFactory,#txtAdvParty').prop('disabled', true).val(0);
        else if (grn.freightId > 0) $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct').prop('disabled', true);
        if (grn.gpId > 0) { try { grn.items = await api('/api/purchase/market-grn/form/items/' + grn.gpId); } catch (e) { grn.items = []; } }
        for (const line of lineItems) if (!grn.items.some(i => Number(i.ItemId) === line.itemId)) grn.items.push({ ItemId: line.itemId, ItemName: line.itemName, ItemCode: line.itemCode });
        itemNameBind();
        showWeightFields(grn.ref === 105);
        if (grn.ref === 105 && grn.gpId > 0) {
            try { const w = await api('/api/purchase/market-grn/form/received-weight/' + grn.gpId); $('#txtReceivedWeight').val(w.ReceivedWeight ?? 0); $('#txtBalWeight').val(w.BalanceWeight ?? 0); } catch (e) { }
        }
        $('#cmbDeliveryTerm').prop('disabled', grn.ref !== 106);
        if (grn.gpId > 0) { try { grn.previousData = await api('/api/purchase/market-grn/form/previous-data/' + grn.gpId + '?recId=' + Number(id)); } catch (e) { } }
        await combsupplierLeave(); $('#cmbPreBillNo').val(String(data.SupplierDispatchId || 0));
        transporterAccountDisable(); showShortageCheckBoxes();
        const diff = numeric('txtSupplierWeight') - numeric('txtFactoryWeight'); $('#txtDiffWeight').val(round(diff, 2)); $('#lblDiffWeight').text(diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal');
        $('#txtTotalFreight').val(round(numeric('txtBiltyFreight') + numeric('txtScaleCharges') - numeric('txtAdvParty') - numeric('txtAdvFactory') - numeric('txtOtherDeduct'), 2));
        renderItemsGrid(); $('#btnSave').text('Update'); applyGrnRights(); switchMode('Form');
        window.history.replaceState(null, '', '/purchase/goods-receipt-notes?id=' + Number(id));
    } catch (error) { if (propagate) throw error; alert(error.message); }
}

function applyGrnRights() { const button = document.getElementById("btnSave"); if (!button?.dataset) return; button.disabled = (numeric("txtId") > 0 ? button.dataset.canUpdate : button.dataset.canSave) === "false"; }
