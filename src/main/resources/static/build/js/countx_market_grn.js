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
 *   grn.pending       dtGatePassData (the pending gate pass list, also the loader dialog's data)
 */
function escapeHtml(v) {
    return String(v === undefined || v === null ? '' : v)
        .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
}
let lineItems = [], loadedHeader = {}, editLine = -1, lookupVersion = 0, historyRows = [], printContext = {};
/* the Attachment form (AT) - btnAttachment_Click :6355, saved with Insert() (:3905-3914) */
const grnAttachments = window.PurchaseDocAttachments ? PurchaseDocAttachments.create({
    type: 46, getId: () => numeric('txtId'),
    canEdit: () => { const b = document.getElementById(numeric('txtId') > 0 ? 'btnUpdate' : 'btnSave'); return !!b && b.dataset[numeric('txtId') > 0 ? 'canUpdate' : 'canSave'] !== 'false'; },
    message: m => alert(m) }) : null;
const grn = { ref: 0, freightId: 0, gpId: 0, gpNo: '', gpDate: '', gpQty: 0, purchaseOrderId: 0, purchaseOrderNo: 0, cfg: {}, items: [],
              lab: null, labRows: [], previousData: [], poEmptyBags: [], preBills: [], chargeToParty: 0, subsidiary: false, wages: false,
              pending: [], invoiceId: 0 };

const cfgNum = name => Number(grn.cfg[name]) || 0;
const cfgOn = name => { const v = String(grn.cfg[name] ?? '').trim().toLowerCase(); return v === '1' || v === 'true'; };
const round = (v, d) => { const f = Math.pow(10, d); return Math.round((Number(v) || 0) * f) / f; };
/* C# ToString("#,##") / ("#,##.##") as written into a textbox that is read back as a number. */
const fmt0 = v => String(Math.round(Number(v) || 0));
const fmt2 = v => String(round(v, 2));
/* C# double.ToString() in interpolated messages: 1200 not 1200.0 */
const cs = v => String(Number(v) || 0);

/* PackUomFromGlobalBind :1313 — the item's UOM schedule, keeping the previous UOM by its text. */
function narrowUomToItem() {
    var itemId = parseInt($('#cmbItem').val() || '0', 10);
    var sel = $('#cmbUom'), currentText = sel.find('option:selected').val() !== '0' ? sel.find('option:selected').text() : '', keep = null;
    sel.find('option').each(function () {
        var opt = $(this);
        if (!opt.attr('data-item-id')) return;
        var owns = parseInt(opt.attr('data-item-id'), 10) === itemId;
        opt.prop('hidden', !owns).prop('disabled', !owns);
        if (owns && keep === null && currentText && opt.text() === currentText) keep = opt.val();
    });
    sel.val(keep || '0');
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
    $('#txtEbUnit').on('input', () => { ebCalculations('unit'); total(); });                 /* txtebu_TextChanged_1 */
    $('#txtWtCut').on('input', () => { if (grn.ref === 105) ebCalculations(); total(); });   /* txtwtcut → txtebu_TextChanged */
    $('#txtEbTotal').on('input', () => { ebCalculations('total'); total(); });
    $('#txtEbUnitStock').on('input', () => { stockEbCalculations('unit'); total(); });
    $('#txtEbTotalStock').on('input', () => { stockEbCalculations('total'); total(); });
    $('#txtAddLess').on('input blur', total);
    $('#txtSupplierWeight,#txtFactoryWeight').on('input', supplierFactoryChanged);
    $('#cmbDeliveryTerm').on('change', txtDeliverTermChanged);
    /* designer :2277-2405 — only these four TextBoxes raise FreightCalculations */
    $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct').on('input', freightCalculations);
    /* txtcarramount TextChanged + Leave → txtcarramount_Leave :5668 */
    $('#txtPaidAmount').on('input change', txtcarramountLeave);
    $('#chkScaleDeduct,#chkSupplierDeduct').on('change', () => { totalEbPurchaseAgainstWeightUtilizeInGrid(); renderItemsGrid(); recalculateBreakupHeader(); });
    $('#cmbPreBillNo').on('change', preBillLeave);
    $('#cmbSupplier').on('change', combsupplierLeave);
    $('input[name="itemSearchMode"]').on('change', itemNameBind);
    $('#grdItemsBody').on('dblclick', 'tr[data-line]', function () { grdDoubleClick(Number(this.dataset.line)); })
                      .on('click', 'tr[data-line]', function () { $('#grdItemsBody tr').removeClass('selected'); $(this).addClass('selected'); $('#grdItemsWrap').trigger('focus'); });
    /* grd_KeyDown :6483 - Ctrl+Space: "Are you sure to Delete?" then the current row goes (no Reset check); Ctrl+Enter edits. */
    $('#grdItemsWrap').on('keydown', function (e) {
        const tr = document.querySelector('#grdItemsBody tr.selected'); if (!tr || !e.ctrlKey) return;
        if (e.code === 'Space') { e.preventDefault(); if (!confirm('Are you sure to Delete?')) return; lineItems.splice(Number(tr.dataset.line), 1); scaleShortageProportion(); supplierShortageProportion(); freightProportion(); renderItemsGrid(); }
    });
    /* grdPurchaseBrakup_KeyDown :6699 - Ctrl+Delete (asks) removes the current breakup row, Ctrl+D adds one
       (Ctrl+Space on the X / + buttons is handled by countx_grn_supplements.js). */
    document.addEventListener('keydown', function (e) {
        if (!e.ctrlKey || !e.target.closest) return;
        const tr = e.target.closest('#grdPurchaseBreakupsBody tr'); if (!tr) return;
        if (e.key === 'Delete') { e.preventDefault(); withButtonLoading(null, () => deletePurchaseBreakup(Array.prototype.indexOf.call(tr.parentNode.children, tr))); }
        else if (e.key === 'd' || e.key === 'D') { e.preventDefault(); withButtonLoading(null, addPurchaseBreakup); }
    });
    /* GrdHistory_KeyDown / grdGp_KeyDown :6566-6697 - Ctrl+Space (or Enter) on a focused link cell runs its link. */
    document.addEventListener('keydown', function (e) {
        const a = e.target.closest && e.target.closest('a[tabindex]');
        if (a && ((e.ctrlKey && e.code === 'Space') || (e.key === 'Enter' && !e.ctrlKey))) { e.preventDefault(); a.click(); return; }
        /* grdGp_KeyDown "Load" / GrdHistory_KeyDown "Edit" / "Print" - Ctrl+Space on a focused button cell */
        const b = e.ctrlKey && e.code === 'Space' && e.target.closest && e.target.closest('#grdPendingBody button, #grdHistoryBody button, #grdLoaderBody button');
        if (b) { e.preventDefault(); b.click(); }
    });
    if (window.PurchaseChrome) {
        PurchaseChrome.footer({ isHistory: () => $('#viewHistory').is(':visible'), toggle: () => switchMode($('#viewHistory').is(':visible') ? 'Form' : 'History'), watch: '#viewHistory' });
        ['#grdItemsWrap', '#grdPendingWrap', '#grdHistoryWrap'].forEach(id => PurchaseChrome.fullscreen(id));
    }
    $('#grdHistoryBody').on('click', 'tr[data-id]', function () { grdHistorySelectionChanged(this); })
                        .on('dblclick', 'tr[data-id]', function () { loadRecord(Number(this.dataset.id)); });
    document.addEventListener('keydown', onFormKeyDown);
    /* the print button's own row/record, read by printRptArgs below */
    document.addEventListener('mousedown', e => { const b = e.target.closest && e.target.closest('[data-rpt]'); if (b) printContext = Object.assign({}, b.dataset); }, true);
    document.addEventListener('keydown', e => { const b = e.target.closest && e.target.closest('[data-rpt]'); if (b) printContext = Object.assign({}, b.dataset); }, true);
    narrowUomToItem();
    $('#txtDocDate').val(new Date().toLocaleDateString('en-CA'));
    renderPendingHead(); renderItemsGrid();
    try { grn.cfg = await api('/api/purchase/market-grn/form/config'); } catch (e) { grn.cfg = {}; alert(e.message); }
    grn.subsidiary = !!grn.cfg.SubsidiaryAccountAllownOnVouchers; grn.wages = !!grn.cfg.WagesActiveOrInActive;
    applyConfigToControls();
    historyDefaults();
    historyComboBind();
    refreshPendingGrid();
    const id = new URLSearchParams(window.location.search).get('id');
    if (id) loadRecord(id); else { renderSupplements({}); applyConfigDefaults(); showShortageCheckBoxes(); applyGrnRights(); }
    configZeroWarning();
});

/* InitializeComponentMethod :789-815 */
function applyConfigToControls() {
    $('#cmbSupplier').prop('disabled', true);
    $('#txtSupplierWeight').prop('readonly', true);
    $('#txtEbUnit,#txtEbTotal').prop('disabled', !cfgOn('EmptyBagsWeightCutEditableOnGRN'));
    $('#txtAddLess').prop('disabled', !cfgOn('AddLessWeightCutEditableOnGRN'));
    $('#txtWtCut').prop('disabled', !cfgOn('WeightCutEditable'));
    $('#txtDocDate').prop('disabled', cfgOn('ValidateGrnAndInvoiceDateWithGpDate'));
    $('#btnEbWtByGross').toggle(cfgOn('EBWtAccordingToGrossWeight'));
    $('.grn-excess').toggle(cfgOn('AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt'));
    $('input[name="itemSearchMode"][value="' + (cfgOn('ItemSearchWithNameOrCode') ? 'Code' : 'Name') + '"]').prop('checked', true);
    $('#panelBreakup').toggleClass('disabled-panel', grn.ref !== 105);
}
/* :863-879 */
function configZeroWarning() {
    const list = [];
    if (cfgNum('WeightCutForJuteBagsStock') === 0) list.push('E.B WeightCut For JuteBags');
    if (cfgNum('WeightCutForPPBagsStock') === 0) list.push('E.B WeightCut For PPBags');
    if (cfgNum('WeightCutForOpenBulkStock') === 0) list.push('E.B WeightCut For OpenBulk');
    if (list.length) alert('Warning: The following configurations Of StockWeight value(s) are set to 0:\n\n' + list.join('\n') + '\n\nPlease check and update them in the configuration.');
}
/* GetConfigurationsFromGlobalAndBindValuesInColumns :928 */
function applyConfigDefaults() {
    for (const [cfg, id] of [['Job/Lot', 'cmbJobLot'], ['Default Crop Year', 'cmbCropYear'], ['Paking Type', 'cmbPackingType'], ['Warehouse', 'cmbWarehouse']]) {
        const v = cfgNum(cfg); if (v) $('#' + id).val(String(v));
    }
    if (!grn.subsidiary) { const t = cfgNum('FreightInwardAc'); if (t) $('#cmbTransporter').val(String(t)); }
    transporterAccountDisable();
}
/* TransporterAccountDisable :980 */
function transporterAccountDisable() {
    if (grn.subsidiary) return;
    $('#cmbTransporter').prop('disabled', grn.freightId > 0);
    $('#txtPaidAmount').prop('disabled', grn.freightId > 0);
}
/* lblRecwt/txtReceivedWeight and lblBalwt/txtBalWeight ("GRN Weight") */
function showRecWeight(show) { $('.grn-recwt').toggle(!!show); }
function showBalWeight(show) { $('.grn-balwt').toggle(!!show); }
/* ShowShortageCheckBoxes :2838 */
function showShortageCheckBoxes() {
    $('#lblSupplierDeduct').toggle(grn.ref === 105);
    $('#lblScaleDeduct').toggle(grn.ref !== 106);
    $('#txtGrossWeight').prop('disabled', grn.ref === 105);
    $('#panelBreakup').toggleClass('disabled-panel', grn.ref !== 105);
    $('.bk-105').toggle(grn.ref === 105); $('.bk-not106').toggle(grn.ref !== 106);
}

function withButtonLoading(btn, asyncFn) { return PurchaseRequest.run(btn, asyncFn).catch(error => alert(error.message)).finally(applyGrnRights); }
async function api(url, options) {
    return PurchaseRequest.track(async () => {
        const response = await fetch(url, options); const data = await response.json().catch(() => ({}));
        if (!response.ok || data?.success === false) throw new Error(data.message || data.detail || 'Request failed'); return data;
    });
}
function numeric(id) { return Number($('#' + id).val()) || 0; }
function textValue(id) { return $('#' + id).val() || ''; }
function selectedText(id) { const o = $('#' + id + ' option:selected'); return o.length && o.val() !== '0' && o.val() !== '' ? o.text() : ''; }
/* yyyy-MM-dd; a timestamp (epoch or ISO with a zone) is shown in the browser's local date. */
function displayDate(value) {
    if (value === null || value === undefined || value === '') return '';
    if (typeof value === 'number' || /T.*([+-]\d{2}:?\d{2}|Z)$/.test(String(value))) {
        const d = new Date(value); if (!isNaN(d)) return d.toLocaleDateString('en-CA');
    }
    return String(value).slice(0, 10);
}
function displayDateTime(value) {
    if (!value) return '';
    const d = new Date(value); if (isNaN(d)) return String(value);
    const p = n => String(n).padStart(2, '0'), h = d.getHours() % 12 || 12;
    return p(d.getDate()) + '-' + p(d.getMonth() + 1) + '-' + d.getFullYear() + ' ' + p(h) + ':' + p(d.getMinutes()) + ' ' + (d.getHours() < 12 ? 'AM' : 'PM');
}
function selectValue(id, value, label) {
    const select = document.getElementById(id); if (!select) return;
    if (value && !Array.from(select.options).some(option => String(option.value) === String(value))) select.add(new Option(label || String(value), value));
    $(select).val(value ?? '0');
}
function selectByText(id, text) {
    const select = document.getElementById(id); if (!select) return;
    const t = String(text || '').trim(); if (!t) { $(select).val(select.options.length ? select.options[0].value : ''); return; }
    let opt = Array.from(select.options).find(o => o.text.trim().toLowerCase() === t.toLowerCase());
    if (!opt) { opt = new Option(t, t); select.add(opt); }
    $(select).val(opt.value);
}
const gridSum = (rows, key) => rows.reduce((s, r) => s + (Number(r[key]) || 0), 0);
const breakupSum = key => (typeof purchaseBreakupRows !== 'undefined' ? purchaseBreakupRows : []).reduce((s, r) => s + (Number(r[key.toLowerCase()]) || 0), 0);
const breakupCount = () => (typeof purchaseBreakupRows !== 'undefined' ? purchaseBreakupRows.length : 0);

/* ---------------- items / lab / pre-bill ---------------- */
/* ItemNameBind :1239 (display member ItemName or ItemCode; an unmatched value activates the first row) */
function itemNameBind() {
    const byCode = $('input[name="itemSearchMode"]:checked').val() === 'Code';
    const sel = $('#cmbItem'), current = sel.val();
    sel.empty().append(new Option('-- Select --', '0'));
    for (const it of grn.items) {
        const o = new Option(byCode ? (it.ItemCode || '') : it.ItemName, it.ItemId);
        /* dtItem columns drawn by the drop grid (countx_prod_combo family grnItem) */
        o.setAttribute('data-item-code', it.ItemCode ?? ''); o.setAttribute('data-po-detail', it.PoDetailId ?? it.PODetailId ?? '');
        o.setAttribute('data-wb-weight', it.ItemWbWeight ?? ''); o.setAttribute('data-moisture', it.Moisture ?? '');
        sel.append(o);
    }
    if (current && current !== '0' && grn.items.some(i => String(i.ItemId) === String(current))) sel.val(current);
    else sel.val(grn.items.length ? String(grn.items[0].ItemId) : '0');
    narrowUomToItem();
}
function selectedItem() { const id = numeric('cmbItem'); return grn.items.find(i => Number(i.ItemId) === id) || null; }

/* combitem_Leave :1433 */
async function combitemLeave() {
    $('#txtMoisture').val(0);
    narrowUomToItem();
    await labNoFill(numeric('cmbItem'));
    if (grn.purchaseOrderId > 0) { const it = selectedItem(); if (it && numeric('cmbItem') > 0) $('#txtMoisture').val(Number(it.Moisture) || 0); }
    await weightBusinessCalc();
    total();
}
/* LabNoFillWithGpIdAndItemId :1385 — only dt.Rows[0], activated. */
async function labNoFill(itemId) {
    const sel = $('#cmbLabNo'); sel.empty(); grn.lab = null; grn.labRows = [];
    if (grn.gpId > 0 && itemId > 0) {
        try { grn.labRows = await api('/api/purchase/market-grn/form/lab?gpId=' + grn.gpId + '&itemId=' + itemId); } catch (e) { alert(e.message); grn.labRows = []; }
    }
    if (!grn.labRows.length) { sel.append(new Option('', '0')); sel.val('0'); await cmbLabNoLeave(); return; }
    for (const r of grn.labRows) {
        const o = new Option(String(r.LabNo ?? ''), r.Id);
        o.setAttribute('data-qty-cut', r.QtyForWtCut ?? ''); o.setAttribute('data-wt-cut', r.WtCut ?? '');
        o.setAttribute('data-cut-on', r.WtCutOn ?? ''); o.setAttribute('data-cut-uom', r.WeightCutUom ?? '');
        sel.append(o);
    }
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
            $('#labParamsFoot').html('<tr><td></td><td class="text-end">' + cs(round(rows.reduce((s, r) => s + (Number(r.InAnalysisResult) || 0), 0), 3)) + '</td></tr>');
            $('#labParamsPanel').show();
        } else { $('#labParamsPanel').hide(); $('#labParamsBody,#labParamsFoot').empty(); }
    } else { $('#txtWtCut').val(0); $('#txtWtCutOn').val(''); $('#labParamsPanel').hide(); $('#labParamsBody,#labParamsFoot').empty(); }
    if (grn.ref === 105) ebCalculations();
    total();
}
/* combsupplier_Leave :7074 / PreBillNoFill :884 */
async function combsupplierLeave() {
    const supplier = numeric('cmbSupplier'), current = $('#cmbPreBillNo').val();
    if (supplier > 0 || grn.purchaseOrderId > 0) {
        try { grn.preBills = await api('/api/purchase/market-grn/form/pre-bills?supplierId=' + supplier + '&grnId=' + numeric('txtId') + '&gpId=' + grn.gpId + '&orderId=' + grn.purchaseOrderId); }
        catch (e) { alert(e.message); grn.preBills = []; }
        fillPreBills(current);
    } else { grn.preBills = []; $('#cmbPreBillNo').empty(); }
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
    const active = document.activeElement && document.activeElement.id;
    if (source === 'unit' && active === 'txtEbUnit') $('#txtEbTotal').val(round(unit * qty, 2));
    else if (source === 'total' && active === 'txtEbTotal') $('#txtEbUnit').val(round(qty > 0 ? tot / qty : 0, 3));
    else $('#txtEbTotal').val(round(unit > 0 && qty > 0 ? unit * qty : 0, 2));
}
/* StockEbCalculations :4728 */
function stockEbCalculations(source) {
    let unit = numeric('txtEbUnitStock'), tot = numeric('txtEbTotalStock'); const qty = numeric('txtItemQty');
    if (!(unit > 0 || tot > 0)) return;
    const active = document.activeElement && document.activeElement.id;
    if (source === 'unit' && active === 'txtEbUnitStock') $('#txtEbTotalStock').val(round(unit * qty, 2));
    else if (source === 'total' && active === 'txtEbTotalStock') $('#txtEbUnitStock').val(round(qty > 0 ? tot / qty : 0, 3));
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
    $('#txtAvgWeight').val(qty > 0 ? fmt2(gross / qty) : '0');
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
    if (cfgOn('DeductionPolicyForGrnIsOn') && grn.ref === 41 && term === 'Ponch') {
        try { await deductionPolicyForGrn(gridGross, true); } catch (e) { alert(e.message); }
        return;
    }
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
    if (itemTotal === 0) { itemTotal = totalGross; grd = lineItems.reduce((s, r) => s + (Number(r.grossWeight) || 0), 0); }
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
    $('#txtBalWeight').val(finalWt);
    const grd = lineItems.reduce((s, r, i) => s + (i !== editLine ? Number(r.grossWeight) || 0 : 0), 0);
    $('#txtGrossWeight').val(finalWt - grd);
    if (refuse && gridGross > 0 && gridGross !== finalWt) throw new Error('GrossWeight ' + cs(gridGross) + ' Should Equal To FinalWeight ' + cs(finalWt) + '...' + name);
    showBalWeight(true);
}

/* ---------------- header events ---------------- */
/* TotalSupplierWeight :5311 then the shortage proportions (txtsuppwt/txtfctwt_TextChanged :5342) */
function supplierFactoryChanged() {
    const diff = numeric('txtSupplierWeight') - numeric('txtFactoryWeight');
    $('#txtDiffWeight').val(round(diff, 2));
    $('#lblDiffWeight').text(diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal');
    weightBusinessCalc();
    scaleShortageProportion(); supplierShortageProportion(); renderItemsGrid();
    recalculateBreakupHeader();
}
/* txtDeliverTerm_ValueChanged :1460 */
async function txtDeliverTermChanged() { await weightBusinessCalc(); total(); stockWeightCalculationInGrid(); renderItemsGrid(); }
/* FreightCalculations :5704 */
function freightCalculations() {
    const totalFreight = numeric('txtBiltyFreight') + numeric('txtScaleCharges') - (numeric('txtAdvParty') + numeric('txtAdvFactory')) - numeric('txtOtherDeduct');
    $('#txtTotalFreight').val(round(totalFreight, 2));
    if (grn.freightId === 0) $('#txtPaidAmount').val(round(totalFreight - numeric('txtFreightDeduct'), 2));
    total(); stockWeightCalculationInGrid(); freightProportion(); renderItemsGrid();
}
/* txtcarramount_Leave :5668 */
function txtcarramountLeave() { freightCalculations(); if (grn.ref === 105) freightProportion(); renderItemsGrid(); }

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
/* grdEmptyBags_CellUpdated :2410 — a Type or Purchase Qty edit re-spreads the purchase-against-weight qty. */
function onEmptyBagChanged(key) {
    if (key === 'purchaseqty' || key === 'typeid') { totalEbPurchaseAgainstWeightUtilizeInGrid(); renderItemsGrid(); }
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
/* UpdateEBTotalWithGrossWeight :6791 (toolbar "E.B Wt According To Gross Weight", config EBWtAccordingToGrossWeight). */
function updateEbTotalWithGrossWeight() {
    if (!cfgOn('EBWtAccordingToGrossWeight')) return;
    /* r.Cells["UOM"] is the UOM code text; Conversion.ToDouble of a non-numeric code is 0. */
    const uomNumber = r => { const n = Number(String(r.uom ?? '').trim()); return isNaN(n) ? 0 : n; };
    let grossTotal = 0, qtyIntoWeight = 0;
    for (const r of lineItems) { grossTotal += Number(r.grossWeight) || 0; qtyIntoWeight += (Number(r.itemQty) || 0) * uomNumber(r); }
    if (!(grossTotal > qtyIntoWeight)) return;
    const fac = numeric('txtFactoryWeight'), sup = numeric('txtSupplierWeight'), term = selectedText('cmbDeliveryTerm');
    const diff = sup - (fac + cfgNum('BillWeightAndStockWeightDifferenceTolerance'));
    let status = 0;
    for (const r of lineItems) {
        const ebUnit = Number(r.ebwPerUnit) || 0, gross = Number(r.grossWeight) || 0;
        let ebTotal = Number(r.ebwTotal) || 0, addPurchase = 0;
        if (ebUnit > 0 && (Number(r.purchaseOrderId) || 0) === 0) {
            status = 0;
            const grdQty = gridSum(lineItems, 'itemQty');
            const purQty = emptyBagRows.filter(b => Number(b.typeid) === 2).reduce((s, b) => s + (Number(b.purchaseqty) || 0), 0);
            if (purQty > 0) { status = 2; addPurchase = (Math.abs(purQty) - grdQty) * ebUnit; }
        }
        const wtCutTotal = Number(r.wtCutTotal) || 0, addLess = Number(r.adLsWeight) || 0;
        let weight = 0;
        if (grn.ref === 105) weight = gross - Math.abs(addLess);
        else if (status === 2 && addPurchase > 0) weight = gross - wtCutTotal - ebTotal - Math.round(addLess) + addPurchase;
        else if (status === 2) weight = gross - wtCutTotal - Math.round(addLess);
        else weight = gross - ebTotal - wtCutTotal - Math.round(addLess);
        r.netBillWeight = round(weight, 2);
        if (sup > 0 && fac > 0 && gross > 0) {
            if (cfgOn('DeductionPolicyForGrnIsOn')) r.stockWeight = fac;
            else {
                ebTotal = Number(r.stockEbTotal) || 0;
                const bySup = () => round(fac * gross / sup - ebTotal, 2), byFac = () => round(fac * gross / fac - ebTotal, 2);
                if (grn.freightId > 0) {
                    if (diff >= 0) r.stockWeight = bySup();
                    else {
                        if (term === 'Load' || term === 'Load & PartyWeight' || term === 'Ponch & PartyWeight') r.stockWeight = bySup();
                        if (term === 'Ponch') r.stockWeight = sup > fac ? byFac() : bySup();
                        if (term === 'Ponch & FactoryWeight' || term === 'Load & FactoryWeight') r.stockWeight = byFac();
                    }
                } else {
                    if (term === 'Load' || term === 'Load & PartyWeight' || term === 'Ponch & PartyWeight') r.stockWeight = bySup();
                    if (term === 'Ponch') r.stockWeight = sup > fac ? byFac() : bySup();
                    if (term === 'Ponch & FactoryWeight' || term === 'Load & FactoryWeight') r.stockWeight = byFac();
                }
            }
        }
    }
    renderItemsGrid();
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
function openBulkConfirmed() {
    const eq = grnUomEquivalent();
    return !(numeric('cmbPackingType') === 5 && numeric('cmbUom') > 0 && eq !== null && eq !== 65
        && !confirm("Are you sure to Add Entry, because Packing type is 'OPEN BULK' and Pack Uom not 65?"));
}
function emptyBagsCaseTypeThree(ebTotal) {
    if (grn.ref !== 105 && emptyBagRows.some(r => Number(r.typeid) === 3) && ebTotal === 0) {
        $('#txtEbUnit').trigger('focus');
        const typeName = $('#grnBagTypeOptions').prop('content')?.querySelector('option[value="3"]')?.textContent || '';
        throw new Error('EmptyBags Weight is required when Empty Bags Case is ' + typeName);
    }
}
/* Add_Click :1872 */
function btnAddLine_Click() {
    try {
        if (cfgOn('DeductionPolicyForGrnIsOn') && lineItems.length === 1) throw new Error('Adding multiple rows is not allowed while the deduction policy is active.');
        if (grn.ref === 105 && breakupCount() > 0) {
            const pu = packUom();
            if (!purchaseBreakupRows.some(r => Number(r.netpacksize) === pu)) throw new Error('UOM ' + selectedText('cmbUom') + ' does not exist in BreakUp Grid');
        }
        const invalid = formValidationDetail(); if (invalid) { alert(invalid); return; }
        if (!openBulkConfirmed()) return;
        const net = numeric('txtBillWeight'), gross = numeric('txtGrossWeight');
        if (net > gross) throw new Error('NetBillWeight cannot be greater than Gross Weight. Please check.');
        const it = selectedItem(), lab = grn.lab && numeric('cmbLabNo') > 0 ? grn.lab : null;
        if (grn.lab && $('#chkWtCutComp').prop('checked') && numeric('txtWtCut') === 0 && !confirm('Weight Cut is compulsory but the value is zero. Do you want to continue?')) return;
        let ebUnit = numeric('txtEbUnit'), ebTotal = numeric('txtEbTotal');
        emptyBagsCaseTypeThree(ebTotal);
        const qty = numeric('txtItemQty');
        if (ebUnit > 0 && ebTotal === 0) { ebTotal = qty * ebUnit; $('#txtEbTotal').val(ebTotal); }
        else if (ebUnit === 0 && ebTotal > 0) { ebUnit = ebTotal / qty; $('#txtEbUnit').val(ebUnit); }
        lineItems.push(Object.assign({ id: 0 }, lineFromEntryBox(it, lab, qty, gross, ebUnit, ebTotal, net), {
            ebPurAgainstWeight: 0, scaleShortWeight: 0, supplierShortWeight: 0, freightAmount: 0,
            /* Conversion.ToInt(CmbLabNo.Text): 0 when no lab row is chosen */
            labReportRef: lab ? String(lab.LabNo ?? '') : '0' }));
        $('#txtVehicleNo').prop('disabled', false);
        resetDetail();
        afterGridChange();
    } catch (error) { alert(error.message); }
}
function lineFromEntryBox(it, lab, qty, gross, ebUnit, ebTotal, net) {
    return {
        purchaseOrderDetailId: grn.ref === 41 && it ? Number(it.PoDetailId) || 0 : 0, purchaseOrderId: grn.purchaseOrderId,
        /* the grid's "Order" column; FillGdnDetailListCommonForInsertAndDelete :4188 never copies it to PurchaserOrderNo */
        orderNo: grn.purchaseOrderNo,
        warehouseId: numeric('cmbWarehouse'), whName: selectedText('cmbWarehouse'),
        itemId: numeric('cmbItem'), itemCode: it ? it.ItemCode || '' : '', itemName: it ? it.ItemName : selectedText('cmbItem'),
        cropYearId: numeric('cmbCropYear'), cropYear: selectedText('cmbCropYear'), jobLotId: numeric('cmbJobLot'), itemCategory: selectedText('cmbJobLot'),
        packingTypeId: numeric('cmbPackingType'), packingType: selectedText('cmbPackingType'),
        itemUomId: numeric('cmbUom'), uom: selectedText('cmbUom'), uomEquivalent: grnUomEquivalent(),
        itemQty: qty, qty, grossWeight: gross, ebwPerUnit: ebUnit, ebwTotal: ebTotal,
        labId: numeric('cmbLabNo'), qtyForWtCut: lab ? Number(lab.QtyForWtCut) || 0 : 0,
        weightCutOnId: lab ? Number(lab.WtCutOnId) || 0 : 0, wtCutOn: lab ? lab.WtCutOn || '' : '', weightCutUom: lab ? Number(lab.WeightCutUom) || 0 : 0,
        wtCut: numeric('txtWtCut'), wtCutTotal: numeric('txtWtCutTotal'), adLsWeight: numeric('txtAddLess'), netBillWeight: net,
        stockEbUnit: numeric('txtEbUnitStock'), stockEbTotal: numeric('txtEbTotalStock'), stockWeight: numeric('txtStockWeight'),
        cityId: numeric('cmbCity'), areaCity: selectedText('cmbCity')
    };
}
/* btnUpdateDetail_Click :1962 */
function btnUpdateDetail_Click() {
    try {
        if (editLine < 0 || !lineItems[editLine]) return;
        if (grn.ref === 105 && breakupCount() > 0 && !purchaseBreakupRows.some(r => Number(r.netpacksize) === packUom()))
            throw new Error('Uom ' + selectedText('cmbUom') + ' does not exists in BreakUp Grid');
        const invalid = formValidationDetail(); if (invalid) { alert(invalid); return; }
        if (!openBulkConfirmed()) return;
        const net = numeric('txtBillWeight'), gross = numeric('txtGrossWeight');
        if (net > gross) throw new Error('NetBillWeight cannot greater than Gross Weight Please check');
        const it = selectedItem(), lab = grn.lab && numeric('cmbLabNo') > 0 ? grn.lab : null, old = lineItems[editLine];
        if (lab && $('#chkWtCutComp').prop('checked') && numeric('txtWtCut') === 0 && !confirm('Weight Cut is compulsory but the value is zero. Do you want to continue?')) return;
        const ebUnit = numeric('txtEbUnit'), ebTotal = numeric('txtEbTotal');
        emptyBagsCaseTypeThree(ebTotal);
        if (ebUnit > 0 && ebTotal === 0) $('#txtEbTotal').val(numeric('txtItemQty') * ebUnit);
        else if (ebUnit === 0 && ebTotal > 0) $('#txtEbUnit').val(ebTotal / numeric('txtItemQty'));
        const line = lineFromEntryBox(it, lab, numeric('txtItemQty'), gross, numeric('txtEbUnit'), numeric('txtEbTotal'), net);
        /* the desktop keeps the row's lab cut columns unless a lab row is chosen, and OrderDetailId unless PO */
        if (!lab) { line.qtyForWtCut = old.qtyForWtCut; line.weightCutOnId = old.weightCutOnId; line.wtCutOn = old.wtCutOn; line.weightCutUom = old.weightCutUom; }
        if (grn.ref !== 41) line.purchaseOrderDetailId = old.purchaseOrderDetailId;
        line.labReportRef = $('#cmbLabNo option:selected').text() || '';
        lineItems[editLine] = Object.assign({}, old, line);
        resetDetail();
        afterGridChange();
    } catch (error) { alert(error.message); }
}
/* ResetDetial :4393 + the Add/Update/Cancel button swap */
function resetDetail() {
    editLine = -1; $('#btnAddLine').show(); $('#btnUpdateLine,#btnCancelLine').hide();
    for (const id of ['txtItemQty', 'txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtAddLess', 'txtBillWeight', 'txtStockWeight']) $('#' + id).val('');
    $('#txtWtCut').val(0); $('#txtWtCutOn').val('');
    $('#cmbUom').val('0'); $('#cmbLabNo').val('0');
    $('#labParamsPanel').hide(); $('#labParamsBody,#labParamsFoot').empty();
    $('#cmbItem').trigger('focus');
}
/* grd_DoubleClick :2061 */
async function grdDoubleClick(index) {
    const line = lineItems[index]; if (!line) return;
    editLine = index;
    if (grn.ref === 41) { grn.purchaseOrderId = Number(line.purchaseOrderId) || 0; grn.purchaseOrderNo = Number(line.orderNo) || 0; }
    selectValue('cmbItem', line.itemId, line.itemName);
    await combitemLeave();
    editLine = index;
    for (const [field, key] of Object.entries({ cmbWarehouse: 'warehouseId', cmbCropYear: 'cropYearId', cmbJobLot: 'jobLotId', cmbPackingType: 'packingTypeId', cmbCity: 'cityId' })) selectValue(field, line[key]);
    $('#txtItemQty').val(line.itemQty ?? 0);
    selectValue('cmbUom', line.itemUomId, line.uom);
    for (const [field, key] of Object.entries({ txtGrossWeight: 'grossWeight', txtEbUnit: 'ebwPerUnit', txtEbTotal: 'ebwTotal' })) $('#' + field).val(line[key] ?? 0);
    if (line.labId && !(grn.labRows || []).some(r => Number(r.Id) === Number(line.labId))) {
        grn.labRows = (grn.labRows || []).concat([{ Id: line.labId, LabNo: line.labReportRef, QtyForWtCut: line.qtyForWtCut, WtCut: line.wtCut, WtCutOnId: line.weightCutOnId, WtCutOn: line.wtCutOn, WeightCutUom: line.weightCutUom }]);
        $('#cmbLabNo').append(new Option(String(line.labReportRef ?? ''), line.labId));
    }
    $('#cmbLabNo').val(String(line.labId || 0));
    await cmbLabNoLeave();
    $('#txtWtCutOn').val(line.wtCutOn || '');
    for (const [field, key] of Object.entries({ txtWtCut: 'wtCut', txtWtCutTotal: 'wtCutTotal', txtAddLess: 'adLsWeight', txtBillWeight: 'netBillWeight', txtEbUnitStock: 'stockEbUnit', txtEbTotalStock: 'stockEbTotal', txtStockWeight: 'stockWeight' })) $('#' + field).val(line[key] ?? 0);
    $('#btnAddLine').hide(); $('#btnUpdateLine,#btnCancelLine').show();
    $('#cmbItem').trigger('focus');
}
/* btnCancelUpdateDetial_Click :2053 — only the buttons and the index; the boxes keep their values */
function cancelEditLine() { editLine = -1; $('#btnAddLine').show(); $('#btnUpdateLine,#btnCancelLine').hide(); }
/* DeleteRowInDetailGrid :2234 */
function removeLine(idx) {
    if (editLine !== -1) { alert('Reset Detail first...'); return; }
    const line = lineItems[idx]; if (!line) return;
    if ((Number(line.id) || 0) > 0 && !confirm('Are you sure to Delete?')) return;
    lineItems.splice(idx, 1);
    scaleShortageProportion(); supplierShortageProportion(); freightProportion(); renderItemsGrid();
}

/* grdSettings :2109 + CommonDetailSettings :2129 — FreightAmount/SuppShortWt only for 105, ScaleShortWt hidden for 106 */
const GRID_COLUMNS = [
    ['whName', 'text'], ['itemName', 'text'], ['cropYear', 'text'], ['itemCategory', 'text'], ['packingType', 'text'], ['uom', 'text'],
    ['itemQty', 'sum'], ['grossWeight', 'sum'], ['ebwPerUnit', 'num'], ['ebwTotal', 'sum'], ['ebPurAgainstWeight', 'sum'], ['labReportRef', 'text'],
    ['qtyForWtCut', 'num'], ['wtCutOn', 'text'], ['weightCutUom', 'num'], ['wtCut', 'num'], ['wtCutTotal', 'sum'], ['adLsWeight', 'sum'],
    ['scaleShortWeight', 'sum', 'gd-not106'], ['supplierShortWeight', 'sum', 'gd-105'], ['netBillWeight', 'sum'], ['stockEbUnit', 'num'], ['stockEbTotal', 'sum'],
    ['stockWeight', 'sum'], ['areaCity', 'text'], ['freightAmount', 'sum', 'gd-105']];
function cellNumber(v) { const n = Number(v) || 0; return String(round(n, 3)); }
function colClass(c) { return c[2] ? ' ' + c[2] : ''; }
function renderItemsGrid() {
    const tbody = $('#grdItemsBody'); tbody.empty();
    lineItems.forEach((item, idx) => {
        tbody.append('<tr data-line="' + idx + '"><td class="text-center"><button type="button" class="tool-btn" style="height:17px;padding:0 4px" onclick="event.stopPropagation();removeLine(' + idx + ')">X</button></td>'
            + '<td class="text-end">' + escapeHtml(item.orderNo || '') + '</td>'
            + GRID_COLUMNS.map(c => '<td class="' + (c[1] === 'text' ? '' : 'text-end') + colClass(c) + '">' + (c[1] === 'text' ? escapeHtml(item[c[0]]) : cellNumber(item[c[0]])) + '</td>').join('') + '</tr>');
    });
    $('#grdItemsFoot').html('<tr><td></td><td class="text-end">' + lineItems.length + '</td>' + GRID_COLUMNS.map(c => '<td class="text-end' + colClass(c) + '">' + (c[1] === 'sum' ? cellNumber(gridSum(lineItems, c[0])) : '') + '</td>').join('') + '</tr>');
    $('.gd-105').toggle(grn.ref === 105); $('.gd-not106').toggle(grn.ref !== 106);
}

/* ---------------- pending gate passes (GatepassGridFill :2490, GpGridSetting :2612) ---------------- */
const PENDING_COLUMNS = [['GpSrNo', 'GpSrNo', 'link'], ['FVStatus', 'Freight Status'], ['GpDate', 'GpDate', 'date'], ['OrderType', 'OrderType'], ['SupplierName', 'SupplierName'],
    ['VehicleNo', 'VehicleNo'], ['BiltyNo', 'BiltyNo'], ['VarietyName', 'ItemName'], ['Qty', 'Qty', 'num'], ['SupplierWeight', 'SupplierWeight', 'num'], ['FactoryWeight', 'FactoryWeight', 'num'],
    ['ReceivedWeight', 'ReceivedWeight', 'num'], ['StockWeight', 'StockWeight', 'num'], ['PoAccessWeight', 'Po Excess Weight', 'excess'], ['VehicleType', 'VehicleType'], ['NetPaid', 'NetPaid', 'num'],
    ['EntryDate', 'EntryDate', 'date'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 'date'], ['ModifyUser', 'ModifyUser'], ['ScalCharges', 'ScalCharges', 'num'],
    ['AdvanceByParty', 'AdvanceByParty', 'num'], ['AdvanceByFactory', 'AdvanceByFactory', 'num'], ['OtherDeduction', 'OtherDeduction', 'num'], ['TotalFreight', 'TotalFreight', 'num'],
    ['ShortageAmount', 'ShortageAmount', 'num'], ['DiscountAmount', 'DiscountAmount', 'num'], ['RemainingAmount', 'RemainingAmount', 'num'], ['ChargeToPartyAmount', 'ChargeToPartyAmount', 'num'],
    ['TotalDeductionAmount', 'TotalDeductionAmount', 'num'], ['TotalPaidAmount', 'TotalPaidAmount', 'num'], ['SupplierDispatchNo', 'SupplierDispatchNo'], ['FreightSpecialApprovalStatus', 'FreightSpecialApprovalStatus']];
/* LoadPendingGatePassForGrn :126 — its own column list and "F.V Status" caption */
const LOADER_COLUMNS = PENDING_COLUMNS.filter(c => !['ScalCharges', 'AdvanceByParty', 'AdvanceByFactory', 'OtherDeduction', 'TotalFreight', 'ShortageAmount', 'DiscountAmount', 'RemainingAmount',
    'ChargeToPartyAmount', 'SupplierDispatchNo', 'FreightSpecialApprovalStatus'].includes(c[0])).map(c => c[0] === 'FVStatus' ? ['FVStatus', 'F.V Status'] : c);
function renderPendingHead() {
    $('#grdPendingHead').html('<th>Load</th>' + PENDING_COLUMNS.map(c => '<th>' + escapeHtml(c[1]) + '</th>').join(''));
    $('#grdLoaderHead').html('<th>Load</th>' + LOADER_COLUMNS.map(c => '<th>' + escapeHtml(c[1]) + '</th>').join(''));
}
function pendingCell(g, c) {
    const v = g[c[0]];
    if (c[2] === 'link') return '<td><a tabindex="0" data-gp-print="' + Number(g.Id) + '" data-detail-count="' + (Number(g.DetailIdsCount) || 0) + '">' + escapeHtml(v) + '</a></td>';
    if (c[2] === 'date') return '<td>' + escapeHtml(displayDate(v)) + '</td>';
    if (c[2] === 'excess') return '<td class="text-end ' + ((Number(v) || 0) > 0 ? 'excess-pos' : (Number(v) || 0) === 0 ? 'excess-zero' : '') + '">' + escapeHtml(v ?? '') + '</td>';
    if (c[2] === 'num') return '<td class="text-end">' + escapeHtml(v ?? '') + '</td>';
    return '<td>' + escapeHtml(v ?? '') + '</td>';
}
/* grouped by Status (grdGp.RootTable.Groups.Add("Status"), Status column hidden) */
function pendingRowsHtml(rows, columns, fromLoader) {
    const groups = [];
    for (const g of rows) { let grp = groups.find(x => x.status === String(g.Status ?? '')); if (!grp) groups.push(grp = { status: String(g.Status ?? ''), rows: [] }); grp.rows.push(g); }
    groups.sort((a, b) => a.status.localeCompare(b.status));
    return groups.map(grp => '<tr class="group-row"><td colspan="' + (columns.length + 1) + '">Status: ' + escapeHtml(grp.status) + ' (' + grp.rows.length + ')</td></tr>'
        + grp.rows.map(g => '<tr data-gp-status="' + escapeHtml(g.Status) + '"><td><button type="button" class="tool-btn" style="height:17px;padding:0 6px" data-gp-id="' + Number(g.Id) + '" onclick="withButtonLoading(this,()=>' + (fromLoader ? 'loaderLoad' : 'loadGatePass') + '(this.dataset.gpId))">Load</button></td>'
            + columns.map(c => pendingCell(g, c)).join('') + '</tr>').join('')).join('');
}
/* FillCountFromGpGrid :2596 */
function fillCountFromGpGrid() {
    const statuses = grn.pending.map(g => String(g.Status ?? ''));
    $('#txtAcceptedGpTotal,#loaderAcceptedTotal').text(statuses.filter(s => s === 'Accepted').length);
    $('#txtOpenGpTotal,#loaderOpenTotal').text(statuses.filter(s => s === 'Open').length);
}
async function refreshPendingGrid() {
    try {
        grn.pending = await api('/api/purchase/market-grn/form/pending');
    } catch (e) { alert(e.message); return; }
    $('#grdPendingBody').html(pendingRowsHtml(grn.pending, PENDING_COLUMNS, false));
    fillCountFromGpGrid();
}
/* grdGp_LinkClicked :2675 — 256 with detail rows, otherwise 251 */
$(document).on('click', 'a[data-gp-print]', function () {
    const id = Number(this.dataset.gpPrint);
    openPrint(Number(this.dataset.detailCount) > 0 ? '256-GatePassInward_WithDetailSlip.rpt' : '251-InvRptInwardGatePassSlip.rpt', { id });
});
/* ScreenViewReights: the target screen's View right, else "Please Check Screen Rights" (the window is opened first so
   the pop-up blocker lets it through, then pointed at the page or closed). */
async function openIfViewRight(screen, url) {
    const w = window.open('about:blank', '_blank');
    try {
        const r = await api('/api/purchase/screen-view-right/' + screen);
        if (r && r.view) { if (w) w.location.href = url; else window.open(url, '_blank'); }
        else { if (w) w.close(); alert('Please Check Screen Rights'); }
    } catch (e) { if (w) w.close(); alert(e.message); }
}
/* GrdHistory_LinkClicked OrderNo :6030 → PurchsaeOrder View right, then PurchaseOrderSlipReport203 */
$(document).on('click', 'a[data-po]', function () {
    openIfViewRight('PurchsaeOrder', '/api/print/by-template/' + encodeURIComponent('203-InvRptPurchaseOrderRiceSlip.rpt') + '/pdf?id=' + Number(this.dataset.po));
});
/* btnGrnFormHistory_Click :5739 → frmGRNHistory (screen 477) when its View right is granted */
function btnGrnFormHistory_Click() { return openIfViewRight('frmGRNHistory', '/purchase/reports/grn-register'); }

/* btnLoadGatePass_Click :3029 */
function btnLoadGatePass_Click() {
    const save = document.getElementById('btnSave');
    if (numeric('txtId') > 0 || !save || save.style.display === 'none' || save.disabled) { alert('Please Reset the form First...'); return; }
    $('#grdLoaderBody').html(pendingRowsHtml(grn.pending, LOADER_COLUMNS, true));
    fillCountFromGpGrid();
    bootstrap.Modal.getOrCreateInstance(document.getElementById('loaderModal')).show();
}
/* LoadPendingGatePassForGrn.LoadGPByRow :341 then LoadGatePassByRowFromLoader :3048 */
async function loaderLoad(id) {
    const row = grn.pending.find(g => Number(g.Id) === Number(id)); if (!row) return;
    if (String(row.Status) !== 'Accepted') throw new Error('Status Not Accepted Please check status');
    if (!cfgOn('LabCompulsoryNotCheckingOnGRN') && (Number(row.LastLabId) || 0) <= 0) throw new Error('Lab is pending for this gate pass. Please do lab first then Load');
    bootstrap.Modal.getOrCreateInstance(document.getElementById('loaderModal')).hide();
    await loadGatePass(id, true);
}

/* LoadGPByRow :2716 (fromLoader: LoadGatePassByRowFromLoader :3048) + combgatepass_Leave :2961
   (GatePassRecordFill :2852, FillWeightsAndItems :2929). */
async function loadGatePass(id, fromLoader = false) {
    if (!Number(id)) return;
    if (numeric('txtId') > 0) throw new Error('Please Reset the form First...');
    const version = ++lookupVersion;
    const data = await api('/api/purchase/market-grn/form/load/' + Number(id) + (fromLoader ? '?loader=true' : '')); if (version !== lookupVersion) return;
    const p = data.pending || {}, gp = data.gatePass;
    /* LoadGPByRow never clears dtdetail: rows already in the grid stay (see report, desktop quirks). */
    loadedHeader = {}; editLine = -1; $('#txtId').val(0);
    grn.ref = Number(data.refDocumentTypeId) || 0; grn.gpId = Number(id); grn.freightId = Number(p.FreightId) || 0; grn.gpNo = String(p.GpSrNo ?? '');
    grn.gpQty = Number(p.Qty) || 0; grn.gpDate = displayDate(p.GpDate); grn.chargeToParty = Number(p.ChargeToPartyAmount) || 0;
    grn.previousData = data.previousData || []; grn.preBills = data.preBills || []; grn.items = data.items || []; grn.invoiceId = 0;
    $('#grnTitle').text('Goods Receiving Notes (' + (p.OrderType || '') + ')');
    if (grn.ref === 106 && cfgOn('DeliveryTermEditableOnGrnForGatePurchase')) $('#cmbDeliveryTerm').prop('disabled', false);
    renderSupplements(data); breakupDirty = grn.ref === 105 && !breakupLocked;
    if (!fromLoader) {
        /* the freight block from the pending row (LoadGPByRow :2750-2774) */
        $('#txtBiltyFreight').val(p.NetPaid ?? ''); $('#txtScaleCharges').val(p.ScalCharges ?? '');
        $('#txtAdvParty').val(p.AdvanceByParty ?? ''); $('#txtAdvFactory').val(p.AdvanceByFactory ?? '');
        $('#txtOtherDeduct').val(p.OtherDeduction ?? ''); $('#txtTotalFreight').val(p.TotalFreight ?? '');
        $('#txtShortageAmt').val(p.ShortageAmount ?? ''); $('#txtDiscountAmt').val(p.DiscountAmount ?? '');
        $('#txtRemainingFreight').val(p.RemainingAmount ?? ''); $('#txtChargeToParty').val(p.ChargeToPartyAmount ?? '');
        $('#txtFreightDeduct').val(p.TotalDeductionAmount ?? ''); $('#txtPaidAmount').val(p.TotalPaidAmount ?? '');
        $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct').prop('disabled', grn.freightId !== 0);
        if (Number(p.FreightDebitAccountId) > 0) $('#cmbTransporter').val(String(p.FreightDebitAccountId));
    } else if (grn.freightId > 0) {
        $('#txtPaidAmount').val(p.TotalPaidAmount ?? ''); $('#txtFreightDeduct').val(p.TotalDeductionAmount ?? '');
    }
    $('#txtReceivedWeight').val(p.ReceivedWeight ?? ''); $('#txtAccessWeight').val(p.PoAccessWeight ?? '');
    $('#txtSupplierWeight').prop('readonly', grn.ref !== 105 || Number(p.ReceivedWeight) !== 0);
    showRecWeight(grn.ref === 105); showBalWeight(grn.ref === 105);
    if (cfgOn('ValidateGrnAndInvoiceDateWithGpDate')) $('#txtDocDate').val(grn.gpDate);
    $('#cmbGatePassNo').val(grn.gpId); $('#txtGatePassNo').val(grn.gpNo);
    if (!fromLoader && grn.freightId === 0 && grn.ref === 105) $('#txtAdvFactory,#txtAdvParty').prop('disabled', true).val(0);
    showShortageCheckBoxes();
    /* GatePassRecordFill :2852 (combgatepass.TextChanged) */
    transporterAccountDisable();
    const locked = !!gp;
    $('#cmbVehicleType').prop('disabled', locked); $('#txtVehicleNo,#txtBiltyNo').prop('disabled', locked);
    if (gp) {
        selectByText('cmbVehicleType', gp.VehicleType); $('#txtVehicleNo').val(gp.VehicleNo || ''); $('#txtBiltyNo').val(gp.BiltyNo || '');
        $('#txtRemarks').val(gp.OtherRemarks || ''); $('#txtItemQty').val(gp.NoOfPackages ?? '');
        $('#txtSupplierWeight').val(gp.SupplierWeight ?? ''); $('#txtFactoryWeight').val(gp.FactoryWeight ?? ''); $('#txtDiffWeight').val(gp.DifferenceWeight ?? '');
        $('#txtAnalystName').val(gp.AnalystName || ''); $('#txtWbTickets').val(gp.TicketNos || '');
        selectValue('cmbSupplier', gp.SupplierCustomerId, gp.CompanyName);
        itemNameBind();
        if (grn.ref === 41) {
            grn.purchaseOrderId = Number(gp.PurchaseOrderId) || 0; grn.purchaseOrderNo = Number(gp.SupplierContractCode) || 0;
            /* comborderno_Leave :1346 */
            const po = data.purchaseOrder || {}; grn.poEmptyBags = po.emptyBags || [];
            purchaseOrderEmptyBags();
            const d = (po.detail || [])[0];
            if (d) { selectByText('cmbCity', d.CityArea); selectByText('cmbDeliveryTerm', d.DeliveryTerm); selectValue('cmbUom', d.OrderItemUOMId); }
            fillPreBills(gp.SupplierDispatchId);
            $('#cmbPreBillNo').val(String(gp.SupplierDispatchId || 0));
        } else if (grn.ref === 105 || grn.ref === 106) {
            /* FillWeightsAndItems :2929 */
            const sup = numeric('txtSupplierWeight'), fac = numeric('txtFactoryWeight'), rec = numeric('txtReceivedWeight');
            if (grn.ref === 105) { selectByText('cmbDeliveryTerm', 'Load'); $('#txtGrnWeight').val(sup); $('#txtBalWeight').val(sup - rec); $('#txtGrossWeight').val(sup - rec); }
            else { const b = sup > fac ? fac : sup; selectByText('cmbDeliveryTerm', 'Ponch'); $('#txtGrnWeight').val(b); $('#txtBalWeight').val(b); $('#txtGrossWeight').val(b); }
            $('#cmbSupplier').prop('disabled', false);
            grn.purchaseOrderId = 0; grn.purchaseOrderNo = 0; grn.poEmptyBags = [];
        }
        total();
        await combitemLeave();
    } else { grn.purchaseOrderId = 0; grn.purchaseOrderNo = 0; }
    if (grn.ref !== 41) await combsupplierLeave();   /* combgatepass_Leave :2973 */
    const diff = numeric('txtSupplierWeight') - numeric('txtFactoryWeight'); $('#lblDiffWeight').text(diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal');
    renderItemsGrid();
    if (breakupDirty) await calculatePurchaseBreakups();
    $('#cmbWarehouse').trigger('focus');
    applyGrnRights();
}

/* ---------------- save / load / reset ---------------- */
/* FormValidation :1477 */
function formValidation() {
    if (!textValue('txtDocNo').trim() || textValue('txtDocNo').trim() === '0') return 'DocNo Field is Required';
    if (!numeric('cmbSupplier')) return 'Supplier Field is Required';
    if (!Number(textValue('txtGatePassNo').trim())) return 'Gate pass Field is Required';
    if (numeric('txtSupplierWeight') === 0) return 'Supplier Weight Field is Required';
    const term = selectedText('cmbDeliveryTerm').trim(); if (!term || term === '0') return 'DeliveryTerm Field is Required';
    if (numeric('txtFactoryWeight') === 0) return 'Factory Weight Field is Required';
    return '';
}
/* btnUpdate_Click :3956 */
function onUpdateRecord() {
    if (numeric('txtId') === 0) throw new Error('Record not update because Id not found');
    return insert();
}
/* btnSave_Click :3943 */
function onSaveRecord() { return insert(); }
/* Insert :3553 — the confirmations and client-side refusals in the desktop's order; the server repeats every refusal. */
async function insert() {
    const updating = numeric('txtId') > 0;
    if (updating && grn.invoiceId > 0) { alert("This Document is referred in invoice. So you can't update this record."); return; }
    if (!lineItems.length) { alert('Grid Record Not Found'); return; }
    if (cfgOn('DeductionPolicyForGrnIsOn') && lineItems.length > 1) throw new Error('Multiple rows is not allowed while the deduction policy is active.');
    const invalid = formValidation(); if (invalid) { alert(invalid); return; }
    if (!confirm(updating ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
    if (grn.ref === 105) {
        const scaleShort = gridSum(lineItems, 'scaleShortWeight');
        if (scaleShort > 0 && !$('#chkScaleDeduct').prop('checked') && !confirm('You have not Apply Scale_Short_Weight Deduction In bill Weight...Are you sure to Continue?')) return;
        if (scaleShort > 0 && !$('#chkSupplierDeduct').prop('checked') && !confirm('You have not Apply Supplier_Short_Weight Deduction In bill Weight...Are you sure to Continue?')) return;
    }
    stockWeightCalculationInGrid(); renderItemsGrid();
    const supplements = await supplementPayload();
    /* Insert() walks every empty-bag grid row (condition / quantity checks), not only the filled ones. */
    if (typeof emptyBagsDirty !== 'undefined' && emptyBagsDirty) supplements.emptyBags = emptyBagRows.map(r => Object.assign({}, r));
    const details = lineItems.map(l => { const d = Object.assign({}, l); delete d.orderNo; delete d.purchaserOrderNo; return d; });
    const payload = { ...supplements, id: numeric('txtId'), docNo: numeric('txtDocNo'), docDate: textValue('txtDocDate'), supplierCustomerId: numeric('cmbSupplier'),
        inwardGatePassId: grn.gpId, gpNo: Number(textValue('txtGatePassNo')) || 0,
        vehicleNo: textValue('txtVehicleNo').trim(), vehicleType: selectedText('cmbVehicleType'), biltyNo: textValue('txtBiltyNo').trim(), remarksHeader: textValue('txtRemarks').trim(),
        partyWeight: numeric('txtSupplierWeight'), factoryWeight: numeric('txtFactoryWeight'), deliveryTerm: selectedText('cmbDeliveryTerm').trim(),
        transporterId: numeric('cmbTransporter'), transporterSupCustId: grn.subsidiary ? Number($('#cmbTransporter option:selected').attr('data-party-id')) || 0 : 0,
        carriageAmount: numeric('txtPaidAmount'), biltyFreight: numeric('txtBiltyFreight'), freightDeduction: numeric('txtFreightDeduct'),
        advanceByPartyFreight: numeric('txtAdvParty'), advanceByFactoryFreight: numeric('txtAdvFactory'), supplierDispatchId: numeric('cmbPreBillNo'),
        chargeToPartyAmountFV: numeric('txtChargeToParty'),
        scaleShortWeightApply: $('#chkScaleDeduct').prop('checked'), supplierShortWeightApply: $('#chkSupplierDeduct').prop('checked'), details };
    const attached = grnAttachments ? grnAttachments.payload() : undefined;
    if (attached) payload.attachments = attached;
    const data = await api('/api/purchase/market-grn/save', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
    alert((updating ? 'Record Update Successfully [' : 'Record Save Successfully [') + (data.docNo ?? payload.docNo) + ']');
    /* :3920 — frmwagesBillHeader for this GRN when ContractorWagesCompulsoryBeforeInvoices and wages are active for 46 */
    if (cfgOn('ContractorWagesCompulsoryBeforeInvoices') && grn.wages) {
        alert('Contractor Wages Bill is required for this GRN (Id ' + data.id + ', Gross Weight ' + cs(gridSum(lineItems, 'grossWeight')) + '). The Labour Wages screen opens in a new tab.');
        window.open('/accounts/vouchers/labour-wages?refDocTypeId=46&refDocId=' + Number(data.id), '_blank');
    }
    const preview = $('#chkPreviewAfterSave').prop('checked');
    await onNewRecord(); refreshPendingGrid();
    if (preview) openPrint('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: Number(data.id), documentTypeId: 46 });
}
/* btnDelete_Click :4230 */
async function onDeleteRecord() {
    const id = numeric('txtId');
    if (id > 0 && grn.invoiceId > 0) { alert("This Document is referred in invoice. So you can't delete this record."); return; }
    if (id <= 0) throw new Error('Record Not Found');
    if (!confirm('Are you sure to Delete?')) return;
    const data = await api('/api/purchase/market-grn/' + id, { method: 'DELETE' });
    alert(data.message || 'Delete Record Successfully');
    await onNewRecord(); refreshPendingGrid();
}
/* btnAttachment_Click :6355 - AT.Show(); the changes are posted with Save / Update. */
function onAttachment(button) {
    if (grnAttachments) return grnAttachments.open(button || document.getElementById('btnAttachment'));
}

function resetGrnState() {
    Object.assign(grn, { ref: 0, freightId: 0, gpId: 0, gpNo: '', gpDate: '', gpQty: 0, purchaseOrderId: 0, purchaseOrderNo: 0, items: [], lab: null, labRows: [], previousData: [], poEmptyBags: [], preBills: [], chargeToParty: 0, invoiceId: 0 });
}
/* reset :4280 (+ btnNew_Click :4424 stock E.b boxes to 0) */
async function onNewRecord() {
    lookupVersion++; loadedHeader = {}; lineItems = []; editLine = -1; resetGrnState(); renderSupplements({});
    if (grnAttachments) grnAttachments.reset();
    document.querySelectorAll('#viewForm input, #viewForm textarea').forEach(input => {
        if (input.type === 'radio' || input.id === 'chkPreviewAfterSave') return;
        if (input.type === 'checkbox') input.checked = false; else if (input.id === 'txtId' || input.id === 'cmbGatePassNo') input.value = 0; else input.value = '';
    });
    document.querySelectorAll('#viewForm select').forEach(select => { $(select).val(select.options.length ? select.options[0].value : ''); });
    const vt = document.getElementById('cmbVehicleType'); if (vt && vt.options.length > 1) vt.selectedIndex = 1;   /* combvehtyp.Rows[1].Activate() */
    $('#cmbItem').empty().append(new Option('-- Select --', '0')); $('#cmbLabNo,#cmbPreBillNo').empty();
    $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct,#txtVehicleNo,#txtBiltyNo,#cmbVehicleType').prop('disabled', false);
    $('#cmbDeliveryTerm').prop('disabled', true);
    $('#labParamsPanel').hide(); $('#labParamsBody,#labParamsFoot').empty(); $('#grnTitle').text('Goods Receiving Notes');
    $('#txtWtCut').val(0); $('#txtEbUnitStock,#txtEbTotalStock').val(0);
    showBalWeight(false);
    narrowUomToItem(); applyConfigToControls(); applyConfigDefaults(); showShortageCheckBoxes(); cancelEditLine(); renderItemsGrid();
    $('#txtDocDate').val(new Date().toLocaleDateString('en-CA'));
    $('#btnSave').show(); $('#btnUpdate,#btnDelete').hide();
    applyGrnRights();
    window.history.replaceState(null, '', '/purchase/goods-receipt-notes');
    const next = await api('/api/purchase/market-grn/next-code'); $('#txtDocNo').val(next.docNo);
    refreshPendingGrid();
}
/* toolStripButton1_Click (Refresh) :4431 — reloads the lookups; the entered record stays. */
async function onRefreshForm() {
    try { grn.cfg = await api('/api/purchase/market-grn/form/config'); } catch (e) { alert(e.message); }
    grn.subsidiary = !!grn.cfg.SubsidiaryAccountAllownOnVouchers; grn.wages = !!grn.cfg.WagesActiveOrInActive;
    let d = null;
    try { d = await api('/api/purchase/market-grn/dropdowns'); } catch (e) { alert(e.message); }
    if (d) rebindLookups(d);
    applyConfigDefaults();
    await refreshPendingGrid();
}
/* BindSupplierName / TransporterAcFill / VehicleTypesBind / Warehouses / Crop / JobLot / PackingType / City / UomSchedule /
   EmptyBagsGridComboBind - BindAndRetainSelection: the list is rebuilt and the current value kept when it is still there. */
function rebindLookups(d) {
    const rebind = (id, rows, first, attrs) => {
        const sel = document.getElementById(id); if (!sel || !Array.isArray(rows)) return;
        const cur = sel.value; const $s = $(sel).empty();
        if (first) $s.append(new Option(first[1], first[0]));
        for (const r of rows) {
            const o = new Option(String(r.name ?? ''), r.id);
            if (attrs) for (const [a, k] of Object.entries(attrs)) { const v = typeof k === 'function' ? k(r) : r[k]; if (v !== undefined && v !== null) o.setAttribute(a, v); }
            $s.append(o);
        }
        $s.val(Array.from(sel.options).some(o => o.value === cur) ? cur : (sel.options.length ? sel.options[0].value : ''));
    };
    rebind('cmbSupplier', d.suppliers, ['0', ''], { 'data-code': 'PartyCode', 'data-city': 'CityName', 'data-mobile': 'MobilePersonal' });
    rebind('cmbTransporter', d.transporters, ['0', '-- Select --'], { 'data-party-id': 'Id', 'data-code': r => r.AccountCode ?? r.PartyCode });
    rebind('cmbVehicleType', (d.vehicleTypes || []).map(r => Object.assign({}, r, { id: r.Id })), ['0', '-- Select --']);
    rebind('cmbWarehouse', d.warehouses, ['0', '-- Select --']);
    rebind('cmbCropYear', d.cropYears, ['0', '-- Select --']);
    rebind('cmbJobLot', d.jobLots, ['0', '-- Select --']);
    rebind('cmbPackingType', d.packingTypes, ['0', '-- Select --']);
    rebind('cmbCity', d.cities, ['0', '-- Select --']);
    rebind('cmbUom', d.uoms, ['0', '-- Select --'], { 'data-item-id': 'ItemId', 'data-equivalent': 'Equivalent', 'data-eq': 'Equivalent', 'data-base': 'BaseRateUom', 'data-base-pack': 'BasePackUom' });
    narrowUomToItem();
    const tpl = (id, rows) => { const t = document.getElementById(id); if (t && Array.isArray(rows)) t.innerHTML = rows.map(r => '<option value="' + escapeHtml(r.id) + '">' + escapeHtml(r.name) + '</option>').join(''); };
    tpl('grnBagTypeOptions', d.emptyBagTypes); tpl('grnBagItemOptions', d.emptyBagItems); tpl('grnBagConditionOptions', d.bagConditions);
    if (typeof drawEmptyBags === 'function') drawEmptyBags();
}

/* tabControl1 */
function switchMode(mode) {
    const history = mode === 'History';
    $('#viewForm').toggle(!history); $('#viewHistory').toggle(history);
    $('#btnModeForm').toggleClass('active', !history); $('#btnModeHistory').toggleClass('active', history);
    if (history) $('#txtHistoryFrom').trigger('focus'); else $('#txtDocDate').trigger('focus');
}

/* ---------------- history ---------------- */
function historyDefaults() {
    const days = cfgNum('DefaultDaysToLessFromHistoryFromDate'), from = new Date();
    from.setDate(from.getDate() - (days > 0 ? days : 3));
    $('#txtHistoryFrom').val(from.toLocaleDateString('en-CA')); $('#txtHistoryTo').val(new Date().toLocaleDateString('en-CA'));
}
/* HistoryComboBind :903 */
async function historyComboBind() {
    let rows = [];
    try { rows = await api('/api/purchase/market-grn/form/history-suppliers'); } catch (e) { alert(e.message); }
    const sel = $('#cmbHistorySupplier'), cur = sel.val(); sel.empty().append(new Option('', '0'));
    for (const r of rows) sel.append(new Option(String(r.name ?? ''), r.id));
    sel.val(cur && rows.some(r => String(r.id) === cur) ? cur : '0');
}
/* btnNewHistory_Click :5762 */
function btnNewHistory_Click() {
    const today = new Date().toLocaleDateString('en-CA');
    $('#txtHistoryFrom,#txtHistoryTo').val(today); $('#txtHistoryFromNo,#txtHistoryToNo').val(''); $('#cmbHistorySupplier').val('0');
    historyRows = []; $('#grdHistoryBody,#grdHistoryDetailBody,#grdHistoryEbBody').empty(); $('#historyCount').text('');
}
/* btnRefreshHistory_Click :5780 */
function btnRefreshHistory_Click() { return historyComboBind(); }
/* btnshow_Click → HistoryGridFill :5804 */
async function btnshow_Click() {
    const useFrom = $('#chkHistoryFrom').prop('checked'), useTo = $('#chkHistoryTo').prop('checked');
    const body = { fromDate: useFrom ? textValue('txtHistoryFrom') || null : null, toDate: useTo ? textValue('txtHistoryTo') || null : null,
                   supplierId: numeric('cmbHistorySupplier') || null, fromDocNo: numeric('txtHistoryFromNo') || null, toDocNo: numeric('txtHistoryToNo') || null,
                   dateType: $('input[name="histDateType"]:checked').val(), actionId: Number($('input[name="histRef"]:checked').val()) || null };
    historyRows = await api('/api/purchase/market-grn/history', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) });
    const num = v => '<td class="text-end">' + escapeHtml(v ?? '') + '</td>', txt = v => '<td>' + escapeHtml(v ?? '') + '</td>';
    $('#grdHistoryBody').html(historyRows.map(r => '<tr data-id="' + Number(r.Id) + '" data-ref="' + (Number(r.RefDocumentTypeId) || 0) + '">'
        + '<td><button type="button" class="tool-btn" style="height:17px;padding:0 5px" onclick="event.stopPropagation();loadRecord(' + Number(r.Id) + ')">Edit</button></td>'
        + '<td><button type="button" class="tool-btn" style="height:17px;padding:0 5px" data-rpt="211-InvRptGoodsReceiptsNotesRiceSlip.rpt" data-rpt-need="id" data-print-id="' + Number(r.Id) + '">Print</button></td>'
        + txt(r.OrderType) + '<td>' + (Number(r.OrderId) > 0 ? '<a tabindex="0" data-po="' + Number(r.OrderId) + '">' + escapeHtml(r.OrderNo) + '</a>' : escapeHtml(r.OrderNo ?? '')) + '</td>'
        + num(r.InvoiceNo) + '<td class="text-end"><a tabindex="0" data-grn-load="' + Number(r.Id) + '">' + escapeHtml(r.DocNo ?? '') + '</a></td>' + txt(displayDate(r.docDate ?? r.DocDate)) + txt(r.DeliveryTerm) + txt(r.SupplierName)
        + '<td>' + (Number(r.InwardGatePassId) > 0 ? '<a tabindex="0" data-gp-print="' + Number(r.InwardGatePassId) + '" data-detail-count="' + (Number(r.DetailIdsCount) || 0) + '">' + escapeHtml(r.GpNo) + '</a>' : escapeHtml(r.GpNo ?? '')) + '</td>'
        + txt(r.VehicleNo) + txt(r.BiltyNo)
        + '<td>' + (Number(r.WagesId) > 0 ? '<a tabindex="0" data-wages="' + Number(r.WagesId) + '">' + escapeHtml(r.WagesNo) + '</a>' : escapeHtml(r.WagesNo ?? '')) + '</td>'
        + num(r.FactoryWeight) + num(r.PartyWeight) + txt(r.Transporter) + num(r.CarriageAmount) + txt(r.RemarksHeader)
        + txt(displayDateTime(r.EntryDate)) + txt(r.EntryUser) + txt(displayDateTime(r.ModifyDate)) + txt(r.ModifyUser) + txt(displayDateTime(r.PostDate)) + txt(r.ApprovedUser)
        + '<td class="text-end"><a tabindex="0" data-attach="' + Number(r.Id) + '">' + escapeHtml(r.NoOfAttachments ?? '') + '</a></td></tr>').join(''));
    $('#historyCount').text(historyRows.length);
    $('#grdHistoryDetailBody,#grdHistoryEbBody').empty();
}
/* DocNo link → ReadById (same as the Edit button / Ctrl+Enter) */
$(document).on('click', 'a[data-grn-load]', function (e) { e.stopPropagation(); loadRecord(Number(this.dataset.grnLoad)); });
/* NoOfAttachments link → CommonServices.GetNoofAttachmentsByScreenName (the record's attachment list) */
$(document).on('click', 'a[data-attach]', function () { if (grnAttachments) grnAttachments.view(Number(this.dataset.attach)); });
/* WagesNo link → ContractorWagesBill_SlipandRegister_002 */
$(document).on('click', 'a[data-wages]', function () { openPrint('002-ContractorWagesSlip.rpt', { id: Number(this.dataset.wages) }); });
function historyDetailTab(tab) {
    $('#histTabDetail').toggle(tab === 'Detail'); $('#histTabEb').toggle(tab !== 'Detail');
    $('#btnHistTabDetail').toggleClass('active', tab === 'Detail'); $('#btnHistTabEb').toggleClass('active', tab !== 'Detail');
}
/* GrdHistory_SelectionChanged :6107 + grdHistoryDetailSettings :6147 */
async function grdHistorySelectionChanged(tr) {
    $('#grdHistoryBody tr').removeClass('selected'); $(tr).addClass('selected');
    const id = Number(tr.dataset.id), ref = Number(tr.dataset.ref) || 0;
    let data;
    try { data = await api('/api/purchase/market-grn/' + id); } catch (e) { $('#grdHistoryDetailBody,#grdHistoryEbBody').empty(); return; }
    const cols = [['PurchaserOrderNo', 'Order'], ['WareHouseName', 'WareHouseName'], ['Item', 'Item'], ['CropYear', 'CropYear'], ['JobLot', 'JobLot'], ['PackingType', 'PackingType'], ['UOMCode', 'UOM'],
        ['ItemQty', 'Qty', 1], ['GrossWeight', 'Gross Weight', 1], ['EBWPerUnit', 'EbUnit', 1], ['EBWTotal', 'EbTotal', 1], ['LabNo', 'LabNo'], ['QtyForWtCut', 'QtyForWtCut', 1],
        ['WeightCutOn', 'WtCutOn'], ['WeightCutUom', 'WeightCutUom', 1], ['WtCut', 'WtCut', 1], ['WtCutTotal', 'WtCutTotal', 1], ['AdLsWeight', 'Add Less', 1],
        ['ScaleShortWeight', 'ScaleShortWt', 1, ref !== 106], ['SupplierShortWeight', 'SuppShortWt', 1, ref === 105], ['NetBillWeight', 'Bill Weight', 1], ['StockEbUnit', 'StockEbUnit', 1],
        ['StockEbTotal', 'StockEbTotal', 1], ['StockWeight', 'StockWeight', 1], ['AreaCity', 'CityName'], ['FreightAmount', 'FreightAmount', 1, ref === 105]].filter(c => c[3] !== false);
    const pick = (r, k) => r[k] ?? r[k.charAt(0).toLowerCase() + k.slice(1)] ?? (k === 'Item' ? r.ItemName : k === 'WareHouseName' ? r.WarehouseName : k === 'JobLot' ? r.JobLotDescription : k === 'PackingType' ? r.PackTypeDesc : k === 'AreaCity' ? r.CityName : k === 'WeightCutOn' ? r.WtCutOn : k === 'LabNo' ? r.LabReportRef : '');
    $('#grdHistoryDetailHead').html(cols.map(c => '<th>' + escapeHtml(c[1]) + '</th>').join(''));
    $('#grdHistoryDetailBody').html((data.details || []).map(r => '<tr>' + cols.map(c => '<td' + (c[2] ? ' class="text-end"' : '') + '>' + escapeHtml(pick(r, c[0])) + '</td>').join('') + '</tr>').join(''));
    const optionText = (tpl, v) => document.getElementById(tpl)?.content?.querySelector('option[value="' + Number(v) + '"]')?.textContent || (v ?? '');
    $('#grdHistoryEbBody').html((data.emptyBags || []).map(r => '<tr><td>' + escapeHtml(optionText('grnBagTypeOptions', r.TypeId)) + '</td><td>' + escapeHtml(optionText('grnBagItemOptions', r.ItemId)) + '</td><td>'
        + escapeHtml(optionText('grnBagConditionOptions', r.BagsCondition)) + '</td><td class="text-end">' + escapeHtml(r.ReceivedQty) + '</td><td class="text-end">' + escapeHtml(r.PurchaseQty) + '</td><td>' + escapeHtml(r.Remarks) + '</td></tr>').join(''));
}

/* FilldtDetailFromListCommonForReadById :4161 — the stored row back into the grid's shape. */
function normalizeLine(row) {
    const m = {}; for (const [k, v] of Object.entries(row)) m[k.charAt(0).toLowerCase() + k.slice(1)] = v;
    const n = k => Number(row[k] ?? m[k.charAt(0).toLowerCase() + k.slice(1)]) || 0;
    return { ...m, id: n('Id'), itemId: n('ItemId'), warehouseId: n('WarehouseId'), itemUomId: n('ItemUomId'), cropYearId: n('CropYearId'), jobLotId: n('JobLotId'),
        packingTypeId: n('PackingTypeId'), cityId: n('CityId'), purchaseOrderId: n('PurchaseOrderId'), purchaseOrderDetailId: n('PurchaseOrderDetailId'),
        itemQty: n('ItemQty'), qty: n('ItemQty'), grossWeight: n('GrossWeight'), ebwPerUnit: n('EBWPerUnit'), ebwTotal: n('EBWTotal'), ebPurAgainstWeight: n('EbPurAgainstWeight'),
        labId: n('LabId'), labReportRef: row.LabNo ?? row.LabReportRef ?? m.labReportRef ?? '', qtyForWtCut: n('QtyForWtCut'), weightCutOnId: n('WeightCutOnId'),
        wtCutOn: row.WeightCutOn ?? row.WtCutOn ?? '', weightCutUom: n('WeightCutUom'), wtCut: n('WtCut'), wtCutTotal: n('WtCutTotal'), adLsWeight: n('AdLsWeight'),
        scaleShortWeight: n('ScaleShortWeight'), supplierShortWeight: n('SupplierShortWeight'), netBillWeight: n('NetBillWeight'),
        stockEbUnit: n('StockEbUnit'), stockEbTotal: n('StockEbTotal'), stockWeight: n('StockWeight'), freightAmount: n('FreightAmount'),
        whName: row.WareHouseName ?? row.WarehouseName ?? '', itemCode: row.ItemCode ?? '', itemName: row.Item ?? row.ItemName ?? '', cropYear: row.CropYear ?? '',
        itemCategory: row.JobLot ?? row.JobLotDescription ?? '', packingType: row.PackingType ?? row.PackTypeDesc ?? '', uom: row.UOMCode ?? '',
        uomEquivalent: n('UOM') || n('UOMEquivalent'), areaCity: row.AreaCity ?? row.CityName ?? '', orderNo: row.PurchaserOrderNo ?? '' };
}
/* ReadById :3972 */
async function loadRecord(id, propagate = false) {
    try {
        const version = ++lookupVersion; const data = await api('/api/purchase/market-grn/' + Number(id)); if (version !== lookupVersion) return;
        resetGrnState(); cancelEditLine();
        if (grnAttachments) grnAttachments.reset();
        grn.ref = Number(data.RefDocumentTypeId) || 0; grn.freightId = Number(data.FreightId) || 0; grn.gpId = Number(data.InwardGatePassId) || 0;
        grn.invoiceId = Number(data.InvoiceId) || 0; grn.gpNo = String(data.GpNo ?? '');
        renderSupplements(data); loadedHeader = data; lineItems = (data.details || []).map(normalizeLine); editLine = -1;
        for (const [field, key] of Object.entries({ txtId: 'Id', txtDocNo: 'DocNo', txtVehicleNo: 'VehicleNo', txtBiltyNo: 'BiltyNo', txtRemarks: 'RemarksHeader', txtSupplierWeight: 'PartyWeight', txtFactoryWeight: 'FactoryWeight',
            txtAccessWeight: 'AccessWeight', txtWbTickets: 'TicketNos',
            txtPaidAmount: 'CarriageAmount', txtBiltyFreight: 'BiltyFreight', txtFreightDeduct: 'FreightDeduction', txtAdvParty: 'AdvanceByPartyFreight', txtAdvFactory: 'AdvanceByFactoryFreight',
            txtScaleCharges: 'ScaleCharges', txtOtherDeduct: 'OtherFreightDeduction', txtShortageAmt: 'ShortageAmountFV', txtDiscountAmt: 'DiscountAmountFV', txtChargeToParty: 'ChargeToPartyAmountFV' }))
            $('#' + field).val(data[key] ?? (field === 'txtId' ? 0 : ''));
        $('#txtRemainingFreight').val((Number(data.ShortageAmountFV) || 0) - (Number(data.DiscountAmountFV) || 0));
        $('#cmbGatePassNo').val(grn.gpId); $('#txtGatePassNo').val(grn.gpNo);
        $('#txtDocDate').val(displayDate(data.DocDate)); selectValue('cmbSupplier', data.SupplierCustomerId, data.CompanyName || data.SupplierName);
        $('#cmbSupplier').prop('disabled', true);
        selectByText('cmbVehicleType', data.VehicleType);
        selectValue('cmbTransporter', data.TransporterId, data.Transporter); selectByText('cmbDeliveryTerm', data.DeliveryTerm);
        $('#chkScaleDeduct').prop('checked', !!data.ScaleShortWeightApply); $('#chkSupplierDeduct').prop('checked', !!data.SupplierShortWeightApply);
        await combsupplierLeave(); $('#cmbPreBillNo').val(String(data.SupplierDispatchId || 0));
        transporterAccountDisable();
        $('#grnTitle').text('Goods Receiving Notes (' + (data.TransType || '') + ')');
        if (grn.freightId === 0 && grn.ref === 105) $('#txtAdvFactory,#txtAdvParty').prop('disabled', true).val(0);
        else if (grn.freightId > 0) $('#txtBiltyFreight,#txtAdvParty,#txtAdvFactory,#txtFreightDeduct').prop('disabled', true);
        if (grn.gpId > 0) { try { grn.items = await api('/api/purchase/market-grn/form/items/' + grn.gpId); } catch (e) { grn.items = []; } }
        for (const line of lineItems) if (!grn.items.some(i => Number(i.ItemId) === line.itemId)) grn.items.push({ ItemId: line.itemId, ItemName: line.itemName, ItemCode: line.itemCode });
        itemNameBind();
        const grossTotal = gridSum(lineItems, 'grossWeight');
        if (grn.ref === 105) {
            $('#txtPaidAmount').val(data.OtherCharges ?? 0);
            showRecWeight(true); showBalWeight(true);
            if (grn.gpId > 0) {
                try {
                    const w = await api('/api/purchase/market-grn/form/received-weight/' + grn.gpId);
                    if (w && Object.keys(w).length) { $('#txtReceivedWeight').val((Number(w.ReceivedWeight) || 0) - grossTotal); $('#txtBalWeight').val((Number(w.BalanceWeight) || 0) + grossTotal); $('#cmbSupplier').prop('disabled', false); }
                } catch (e) { alert(e.message); }
            }
        } else if (grn.ref === 106) {
            $('#cmbSupplier').prop('disabled', false); $('#cmbDeliveryTerm').prop('disabled', false);
        } else {
            $('#txtReceivedWeight,#txtBalWeight').val(grossTotal);
            /* ReadById :4080 — the first detail row's DeliveryTerm */
            const firstTerm = (data.details || [])[0] ? ((data.details[0].DeliveryTerm ?? data.details[0].deliveryTerm) || '') : '';
            if (firstTerm) selectByText('cmbDeliveryTerm', firstTerm);
        }
        $('#txtGrnWeight').val(grossTotal);
        if (grn.gpId > 0) { try { grn.previousData = await api('/api/purchase/market-grn/form/previous-data/' + grn.gpId + '?recId=' + Number(id)); } catch (e) { } }
        scaleShortageProportion(); supplierShortageProportion(); showShortageCheckBoxes(); freightProportion();
        const diff = numeric('txtSupplierWeight') - numeric('txtFactoryWeight'); $('#txtDiffWeight').val(round(diff, 2)); $('#lblDiffWeight').text(diff > 0 ? 'Short' : diff < 0 ? 'Excess' : 'Equal');
        $('#txtTotalFreight').val(round(numeric('txtBiltyFreight') + numeric('txtScaleCharges') - numeric('txtAdvParty') - numeric('txtAdvFactory') - numeric('txtOtherDeduct'), 2));
        renderItemsGrid();
        $('#btnSave').hide(); $('#btnUpdate,#btnDelete').show(); applyGrnRights(); switchMode('Form');
        window.history.replaceState(null, '', '/purchase/goods-receipt-notes?id=' + Number(id));
    } catch (error) { if (propagate) throw error; alert(error.message); }
}

/* rights: btnSave/btnUpdate/btnDelete/BtnPrint Enabled from formright (:775-778) */
function applyGrnRights() {
    const set = (id, attr) => { const b = document.getElementById(id); if (b && b.dataset[attr] !== undefined && !b.classList.contains('loading')) b.disabled = b.dataset[attr] === 'false'; };
    set('btnSave', 'canSave'); set('btnUpdate', 'canUpdate'); set('btnDelete', 'canDelete');
}

/* ---------------- printing (print-rpt.js handles [data-rpt]; the arguments come from here) ---------------- */
window.grnPrintRptArgs = window.printRptArgs = function (rpt, a) {
    a = a || {};
    if (/^211/.test(rpt)) { a.id = Number(printContext.printId) || numeric('txtId') || undefined; a.documentTypeId = 46; }
    else if (/^257/.test(rpt)) { a.id = grn.gpId || undefined; }
    printContext = {};
    return a;
};
/* GrnSlipWithSubReports / grid links: the same by-template endpoint, opened directly. */
function openPrint(rpt, args) {
    const q = new URLSearchParams(); for (const [k, v] of Object.entries(args || {})) if (v !== undefined && v !== null && v !== '') q.set(k, v);
    const w = window.open('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf?' + q.toString(), '_blank');
    if (!w) alert('Allow pop-ups for this site to see the report.');
}

/* ---------------- keyboard (InvFrmGRN_KeyDown :6360, MakeShortCutKeys :6532) ---------------- */
const SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
    ['Ctrl+P', 'For Print'], ['Ctrl+H', 'For History Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
    ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On warehouse Combo in Detail Box'],
    ['Ctrl+ArrowRight', 'For Focus From One Grid To Another'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]];
function showShortcutKeys() {
    $('#shortcutBody').html(SHORTCUTS.map(s => '<tr><td>' + escapeHtml(s[0]) + '</td><td>' + escapeHtml(s[1]) + '</td></tr>').join(''));
    bootstrap.Modal.getOrCreateInstance(document.getElementById('shortcutModal')).show();
}
function clickIfEnabled(id) { const b = document.getElementById(id); if (b && !b.disabled && b.offsetParent !== null) b.click(); }
const GRID_CYCLE = ['grdItemsWrap', 'grdEmptyBagsBody', 'grdPendingWrap', 'grdPurchaseBreakupsBody'];
function focusGrid(id) {
    const el = document.getElementById(id); if (!el) return;
    const wrap = el.closest('.grid-wrap') || el; if (!wrap.hasAttribute('tabindex')) wrap.setAttribute('tabindex', '0');
    wrap.focus();
}
function onFormKeyDown(e) {
    const modalOpen = !!document.querySelector('.modal.show'), comboOpen = !!document.querySelector('.dtcombo-pop[style*="block"]');
    /* KeyData == Return -> SendKeys "{TAB}" (not inside a grid, a button, a textarea or an open drop-down) */
    if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !modalOpen && !comboOpen) {
        const t = e.target;
        if (t && t.matches && t.matches('input:not([type=button]):not([type=checkbox]):not([type=radio]), .dtcombo-input') && !t.closest('.grid-wrap')) {
            const all = Array.from(document.querySelectorAll('#viewForm input, #viewForm .dtcombo-input, #viewForm button, #viewHistory input, #viewHistory .dtcombo-input'))
                .filter(x => x.offsetParent !== null && !x.disabled && x.tabIndex >= 0 && x.type !== 'hidden' && !(x.closest('.grid-wrap')));
            const i = all.indexOf(t); if (i >= 0 && all[i + 1]) { e.preventDefault(); all[i + 1].focus(); return; }
        }
    }
    /* Escape -> Close() */
    if (e.key === 'Escape' && !e.ctrlKey && !e.defaultPrevented && !modalOpen && !comboOpen && !document.querySelector('.pc-fs-on')
        && !(e.target && e.target.closest && e.target.closest('.dtcombo-wrap, .dtcombo-pop, .modal'))) { window.location.href = '/purchase'; return; }
    if (!e.ctrlKey) return;
    if (e.altKey && (e.key === 'Alt' || e.key === 'Control')) { e.preventDefault(); showShortcutKeys(); return; }
    const k = e.key.toLowerCase(), history = $('#viewHistory').is(':visible');
    let handled = true;
    if (k === 't') switchMode(history ? 'Form' : 'History');
    else if (k === 'e') window.location.href = '/purchase';
    else if (history) {
        if (k === 's') clickIfEnabled('btnHistoryShow');
        else if (k === 'n') btnNewHistory_Click();
        else if (k === 'r') clickIfEnabled('btnHistoryRefresh');
        else if (k === 'p') { const tr = document.querySelector('#grdHistoryBody tr.selected'); if (tr) openPrint('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: Number(tr.dataset.id), documentTypeId: 46 }); }
        else if (k === 'h') btnGrnFormHistory_Click();
        else if (k === 'arrowup') $('#txtHistoryFrom').trigger('focus');
        else if (k === 'arrowdown') $('#grdHistoryWrap').trigger('focus');
        else if (k === 'enter') { const tr = document.querySelector('#grdHistoryBody tr.selected'); if (tr) loadRecord(Number(tr.dataset.id)); else handled = false; }   /* GrdHistory_KeyDown :6688 */
        else handled = false;
    } else {
        if (k === 'n') clickIfEnabled('btnNew');
        else if (k === 'p') clickIfEnabled('btnPrint211');
        else if (k === 's') clickIfEnabled('btnSave');
        else if (k === 'u') clickIfEnabled('btnUpdate');
        else if (k === 'delete' && e.shiftKey) clickIfEnabled('btnDelete');
        else if (k === 'arrowup') { const w = document.getElementById('cmbWarehouse'); const f = w && w.parentElement && w.parentElement.querySelector('.dtcombo-input'); (f || w).focus(); }
        else if (k === 'arrowdown') focusGrid('grdItemsWrap');
        else if (k === 'arrowright') {
            const at = GRID_CYCLE.findIndex(id => { const el = document.getElementById(id); const w = el && (el.closest('.grid-wrap') || el); return w && w.contains(document.activeElement); });
            focusGrid(GRID_CYCLE[at < 0 ? 0 : (at + 1) % GRID_CYCLE.length]);
        }
        else if (k === 'f5') $('#txtDocDate').trigger('focus');
        else if (k === 'f10') onAttachment();
        else if (k === 'enter') { const tr = document.querySelector('#grdItemsBody tr.selected'); if (tr) grdDoubleClick(Number(tr.dataset.line)); else handled = false; }
        else handled = false;
    }
    if (handled) e.preventDefault();
}
