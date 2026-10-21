(() => {
  'use strict';
  const api = '/sale/delivery-order/api';
  const $ = id => document.getElementById(id);
  let initial = null, cfg = {}, currentId = 0, rows = [], expenses = [], removedLineIds = [], activeRequests = 0;
  let currentToBranchId = 0, updateIndex = -1, itemMap = new Map(), uomMap = new Map(), approved = false;
  try { Object.defineProperty(window, 'RecId', { get: () => currentId, configurable: true }); } catch (e) {}
  const disabledBefore = new Map();
  const text = (o, ...keys) => { for (const k of keys) if (o && o[k] != null) return String(o[k]); return ''; };
  const num = (o, ...keys) => Number(text(o, ...keys) || 0);
  const html = value => String(value ?? '').replace(/[&<>'"]/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','"':'&quot;'}[c]));
  const sel = id => Number($(id).value) || 0;
  const selText = id => $(id).value ? ($(id).selectedOptions[0]?.textContent || '').trim() : '';
  const fmt = (n, d = 3) => (Math.round((Number(n) || 0) * 10 ** d) / 10 ** d).toString();

  function busy(start) {
    activeRequests = Math.max(0, activeRequests + (start ? 1 : -1));
    $('requestLoader').hidden = activeRequests === 0;
    if (activeRequests) {
      document.querySelectorAll('button').forEach(button => {
        if (!disabledBefore.has(button)) disabledBefore.set(button, {disabled:button.disabled,busy:button.getAttribute('aria-busy')});
        button.disabled = true; button.setAttribute('aria-busy','true');
      });
    } else {
      for (const [button,state] of disabledBefore) {
        button.disabled = state.disabled;
        if (state.busy === null) button.removeAttribute('aria-busy'); else button.setAttribute('aria-busy',state.busy);
      }
      disabledBefore.clear();
    }
  }
  async function request(url, options = {}) {
    busy(true); clearMessage();
    try {
      const response = await fetch(url, {headers:{'Content-Type':'application/json'}, ...options});
      if (response.redirected && /\/login(?:[?#]|$)/.test(response.url)) throw new Error('Your session has expired. Please sign in again.');
      const body = response.status === 204 ? null : await response.json().catch(() => {throw new Error('The server did not return valid data. Please refresh or sign in again.');});
      if (!response.ok) throw new Error(body?.message || body?.detail || `Request failed (${response.status})`);
      return body;
    } finally { busy(false); }
  }
  function message(value, ok=false) { const box=$('message'); box.textContent=value; box.className=ok?'ok':''; }
  function clearMessage() { message(''); }
  function today(offset=0) { const now=new Date();now.setDate(now.getDate()+offset);return new Date(now.getTime()-now.getTimezoneOffset()*60000).toISOString().slice(0,10); }
  function dateOnly(value) { return value ? String(value).slice(0,10) : ''; }
  function displayDate(value) { const v=dateOnly(value); if(!v)return ''; const [y,m,d]=v.split('-'); return `${d}-${m}-${y}`; }

  /* ---- combo helpers: option value = id, text = display ---- */
  function list(id, items, idFn, nameFn, blank = true) {
    const el = $(id), previous = el.value;
    el.innerHTML = (blank ? '<option value=""></option>' : '') + (items || []).map(r => `<option value="${html(idFn(r))}">${html(nameFn(r))}</option>`).join('');
    if (previous && Array.from(el.options).some(o => o.value === previous)) el.value = previous;
  }
  function setSel(id, value, label) {
    const el = $(id), v = String(value ?? '');
    if (!v || v === '0') { el.value = ''; return; }
    if (!Array.from(el.options).some(o => o.value === v)) el.insertAdjacentHTML('beforeend', `<option value="${html(v)}">${html(label || v)}</option>`);
    el.value = v;
  }
  const isLocal = () => $('deliveryOrderType').value !== 'StockTransfer';
  function applyType() {
    const transfer = !isLocal();
    $('f_branchTo').hidden = !transfer;
    $('branchFromLabel').textContent = transfer ? 'Branch From' : 'Branch';
    $('dWeight').readOnly = transfer || $('saleType').value === '2';
    bindItems();
  }
  function lockType(lock) { $('deliveryOrderType').disabled = lock; }

  function configure(data) {
    initial = data; cfg = data.config || {};
    list('deliveryOrderType', data.deliveryOrderTypes || [], r => r.Name, r => r.Name, false);
    list('branchFrom', data.branches || [], r => text(r,'BranchId','Id'), r => text(r,'BranchName'), false);
    list('branchTo', data.branches || [], r => text(r,'BranchId','Id'), r => text(r,'BranchName'), false);
    list('vehicleType', data.vehicleTypes || [], r => text(r,'Id'), r => text(r,'VehicleDescription'));
    list('saleType', data.saleTypes || [], r => text(r,'Id'), r => text(r,'Name'), false);
    list('dCustomer', data.customers || [], r => text(r,'Id'), r => text(r,'CompanyName'));
    list('dWarehouse', data.warehouses || [], r => text(r,'Id'), r => text(r,'Name'));
    list('dCrop', data.crops || [], r => text(r,'Id'), r => text(r,'Name'));
    list('dJobLot', data.jobLots || [], r => text(r,'Id'), r => text(r,'Name'));
    list('dPackingType', data.packingTypes || [], r => text(r,'Id'), r => text(r,'Name'));
    $('hCustomerList').innerHTML = (data.customers || []).map(r => `<option value="${html(text(r,'CompanyName'))}"></option>`).join('');
    $('saleOrderList').innerHTML = (data.pendingOrders || []).map(r => `<option value="${html(`${text(r,'DocNo')} — ${text(r,'PartyName')} — ${dateOnly(text(r,'DocDate'))}`)}"></option>`).join('');
    $('stockReservedWrap').hidden = !cfg.stockReservedPerParty;
    reset();
  }
  function defaults() { /* GetConfigurationsFromGlobalAndBindValuesInColumns (:740) */
    const d = (id, v) => { if (v && Number(v) > 0) setSel(id, v, (Array.from($(id).options).find(o => o.value === String(v)) || {}).textContent); };
    d('dJobLot', cfg.jobLot); d('dCrop', cfg.cropYear); d('dPackingType', cfg.packingType); d('dWarehouse', cfg.warehouse);
  }

  /* ---- detail entry panel ---- */
  async function loadOrders() { /* SaleOrderBind (:3333) */
    if (!isLocal()) { list('dOrderNo', [], () => '', () => ''); await loadOrderItems(); return; }
    if (!sel('dCustomer')) { list('dOrderNo', [], () => '', () => ''); itemMap.clear(); bindItems(); return; }
    const orders = await request(`${api}/orders?customerId=${sel('dCustomer')}&recId=${currentId}`);
    list('dOrderNo', orders, r => text(r,'SaleOrderId'), r => text(r,'SaleOrderNo'));
  }
  async function loadOrderItems() { /* ItemBindbyOrderIdDbCall (:3377) */
    itemMap.clear();
    if (!isLocal()) { for (const r of initial.items || []) itemMap.set(String(r.Id), {itemId:num(r,'Id'), name:text(r,'ItemName'), code:text(r,'ItemCode'), detailId:0, rate:0, rateUom:0}); }
    else if (sel('dOrderNo')) {
      for (const r of await request(`${api}/order-items?orderId=${sel('dOrderNo')}`))
        itemMap.set(text(r,'Id'), {itemId:num(r,'OrderItemId'), name:text(r,'ItemName'), code:text(r,'ItemCodeNew','ItemCode'), detailId:num(r,'Id'), rate:num(r,'OrderItemRate'), rateUom:num(r,'RateUom')});
    }
    bindItems();
  }
  function bindItems() { /* ItemNameBind (:947): name or code column */
    const byCode = $('rdCode').checked, el = $('dItem'), previous = el.value;
    el.innerHTML = '<option value=""></option>' + Array.from(itemMap, ([k, v]) => `<option value="${html(k)}">${html(byCode ? v.code : v.name)}</option>`).join('');
    if (previous && itemMap.has(previous)) el.value = previous;
    bindUoms();
  }
  function bindUoms() { /* PackUomFromGlobalBind(:1037): globalUomSchedule filtered by the item */
    const it = itemMap.get($('dItem').value), el = $('dPackUom'), previous = el.value;
    uomMap.clear();
    const items = it ? (initial.uoms || []).filter(u => num(u,'ItemId') === it.itemId) : [];
    for (const u of items) uomMap.set(String(u.Id), u);
    el.innerHTML = '<option value=""></option>' + items.map(u => `<option value="${html(u.Id)}">${html(u.UOMCode)}</option>`).join('');
    if (previous && uomMap.has(previous)) el.value = previous;
  }
  const equivalent = () => { const u = uomMap.get($('dPackUom').value); return u ? Number(u.Equivalent) || 0 : 0; };
  function calcWeight() { /* CalculateWeight (:2790) */
    const w = (Number($('dQty').value) || 0) * equivalent();
    $('dWeight').value = fmt(w); calcGross(false);
  }
  function calcGross(fromPackWeight) { /* CalculateGrossWeight (:2806) */
    const weight = Number($('dWeight').value) || 0;
    if (weight <= 0) { $('dGross').value = '0'; return; }
    const qty = Number($('dQty').value) || 0; let unit = Number($('dPackUnit').value) || 0, pack = Number($('dPackWeight').value) || 0;
    if (fromPackWeight) { unit = qty > 0 ? pack / qty : 0; $('dPackUnit').value = fmt(unit, 5); }
    else { pack = unit * qty; $('dPackWeight').value = fmt(pack); }
    $('dGross').value = fmt(weight + pack);
  }
  async function balance() { /* CalculateOrderBalanceQtyandWeight (:2751) */
    const it = itemMap.get($('dItem').value);
    if (!isLocal() || !sel('dOrderNo') || !it || !sel('dPackUom') || !it.itemId) return;
    const r = await request(`${api}/order-balance?orderId=${sel('dOrderNo')}&detailId=${it.detailId}&itemId=${it.itemId}`);
    const bal = Number(r.balWeight) || 0;
    if (bal > 0) { $('dBalWeight').value = fmt(bal); $('dBalQty').value = equivalent() ? fmt(bal / equivalent()) : ''; }
    else { $('dBalWeight').value = ''; $('dBalQty').value = ''; }
  }
  function resetDetail() { /* ResetDetail (:2283) */
    for (const id of ['dQty','dWeight','dRemarks']) $(id).value = '';
    for (const id of ['dPackUnit','dPackWeight','dGross','dBalQty','dBalWeight']) $(id).value = '0';
    for (const id of ['dOrderNo','dItem','dPackUom','dPackingType','dWarehouse','dJobLot']) $(id).value = '';
    if (isLocal()) { list('dOrderNo', [], () => '', () => ''); itemMap.clear(); bindItems(); }
    updateIndex = -1; $('btnPlus').hidden = false; $('btnUpdateDetail').hidden = true; $('btnCancelDetail').hidden = true;
  }
  function validateDetail() { /* FormValidationDetail (:1136) */
    const need = [['dCustomer','Customer field is required'],['dItem','ItemName field is required'],['dPackUom','PackUOM field is required'],['dPackingType','Packingtype field is required'],['dCrop','CropYear field is required'],['dJobLot','JobLot field is required']];
    for (const [id, msg] of need) if (!sel(id)) { message(msg); $(id).focus?.(); return false; }
    if (!(Number($('dQty').value) > 0)) { message('ItemQty field is required'); return false; }
    if (!(Number($('dWeight').value) > 0)) { message('Weight field is required'); return false; }
    if (!(Number($('dGross').value) > 0)) { message('GrossWeight field is required'); return false; }
    if (isLocal() && !sel('dOrderNo')) { message('OrderNo field is required'); return false; }
    if (!sel('dWarehouse')) { message('WareHouseName field is required'); return false; }
    return true;
  }
  function rowFromPanel(base = {}) {
    const it = itemMap.get($('dItem').value), local = isLocal();
    return { ...base, id: base.id || 0, supplierCustomerId: sel('dCustomer'), customer: selText('dCustomer'),
      saleOrderId: local ? sel('dOrderNo') : 0, orderNo: local ? selText('dOrderNo') : '', saleOrderDetailId: local ? it.detailId : 0,
      deliveryScheduleId: base.deliveryScheduleId || 0, deliveryScheduleDetailId: base.deliveryScheduleDetailId || 0,
      itemId: it.itemId, itemCode: it.code, itemName: it.name, packUomId: sel('dPackUom'), packUom: selText('dPackUom'),
      cropYearId: sel('dCrop'), crop: selText('dCrop'), packingTypeId: sel('dPackingType'), packingType: selText('dPackingType'),
      warehouseId: sel('dWarehouse'), warehouse: selText('dWarehouse'), jobLotId: sel('dJobLot'), jobLot: selText('dJobLot'),
      quantity: Number($('dQty').value) || 0, weight: Number($('dWeight').value) || 0, packingUnit: Number($('dPackUnit').value) || 0,
      packingWeight: Number($('dPackWeight').value) || 0, grossWeight: Number($('dGross').value) || 0,
      rate: local ? it.rate : 0, rateUom: local ? it.rateUom : 0, rateUomId: base.rateUomId || 0, remarks: $('dRemarks').value.trim(),
      refPartyId: base.refPartyId || 0, refParty: base.refParty || '', refDocumentTypeId: base.refDocumentTypeId || 0, refDocIdNo: base.refDocIdNo || 0, refDocSubIdNo: base.refDocSubIdNo || 0 };
  }
  async function addDetail() { /* btnplus_Click (:1207) */
    if (rows.length && rows.some(r => (r.deliveryScheduleId || 0) > 0)) throw new Error('You can not add manual Record because record against Customer Schedule exist in Grid');
    if (!validateDetail()) return;
    const row = rowFromPanel(); rows.push(row);
    if (isLocal() && row.saleOrderDetailId) await mergeExpenses([row.saleOrderDetailId]);
    resetDetail(); lockType(true); renderRows();
  }
  async function updateDetail() { /* btnUpdateDetail_Click (:1410) */
    if (updateIndex < 0 || !validateDetail()) return;
    rows[updateIndex] = rowFromPanel(rows[updateIndex]); resetDetail(); renderRows();
  }
  async function editRow(index) { /* grd_DoubleClick (:1483): row -> entry panel */
    const row = rows[index]; if (!row) return;
    updateIndex = index; setSel('dCustomer', row.supplierCustomerId, row.customer);
    if (isLocal()) { await loadOrders(); setSel('dOrderNo', row.saleOrderId, row.orderNo); await loadOrderItems(); }
    const key = isLocal() ? String(row.saleOrderDetailId) : String(row.itemId);
    if (!itemMap.has(key)) itemMap.set(key, {itemId:row.itemId, name:row.itemName, code:row.itemCode, detailId:row.saleOrderDetailId, rate:row.rate, rateUom:row.rateUom});
    bindItems(); $('dItem').value = key; bindUoms(); setSel('dPackUom', row.packUomId, row.packUom);
    setSel('dCrop', row.cropYearId, row.crop); setSel('dPackingType', row.packingTypeId, row.packingType); setSel('dWarehouse', row.warehouseId, row.warehouse); setSel('dJobLot', row.jobLotId, row.jobLot);
    $('dQty').value = row.quantity; $('dWeight').value = row.weight; $('dPackUnit').value = row.packingUnit; $('dPackWeight').value = row.packingWeight; $('dGross').value = row.grossWeight; $('dRemarks').value = row.remarks || '';
    $('btnPlus').hidden = true; $('btnUpdateDetail').hidden = false; $('btnCancelDetail').hidden = false;
  }

  /* ---- grids ---- */
  function fromOrderLine(source) {
    const refId = num(source,'RefPartyId');
    const refName = text(source,'RefPartyName','ReferencePartyName','RefParty') || (initial?.referenceParties || []).find(r => num(r,'Id') === refId)?.Name || '';
    return {
      id:num(source,'Id','id'), supplierCustomerId:num(source,'SupplierCustomerId','supplierCustomerId'), customer:text(source,'SupplierCustomer','supplierCustomer','CustomerName'),
      saleOrderId:num(source,'OrderId','SaleOrderId','saleOrderId'), orderNo:text(source,'OrderNo','orderNo'), saleOrderDetailId:num(source,'SaleOrderDetailId','saleOrderDetailId'),
      deliveryScheduleId:num(source,'DeliveryScheduleId','DeliveryScheduleCustomerId','deliveryScheduleId'), deliveryScheduleDetailId:num(source,'DeliveryScheduleDetailId','DeliveryScheduleCustomerDetailId','deliveryScheduleDetailId'),
      itemId:num(source,'ItemId','itemId'), itemCode:text(source,'ItemCode','itemCode'), itemName:text(source,'Item','ItemName','itemName'), packUomId:num(source,'ItemUOMId','PackUomId','packUomId'), packUom:text(source,'ItemUOM','PackUOM','packUom'),
      cropYearId:num(source,'CropYearId','cropYearId'), crop:text(source,'CropYear','crop'), packingTypeId:num(source,'PackingTypeId','InvPackingTypeId','packingTypeId'), packingType:text(source,'PackTypeDesc','PackingType','packingType'),
      warehouseId:num(source,'WareHouseId','WarehouseId','warehouseId'), warehouse:text(source,'WareHouseName','Warehouse','warehouse'), jobLotId:num(source,'JobLotId','jobLotId'), jobLot:text(source,'JobLotDescription','JobLot','jobLot'),
      quantity:num(source,'LoadQty','DoQty','LoadingQty','quantity'), weight:num(source,'LoadWeight','DoWeight','LoadingWeight','weight'), packingUnit:num(source,'PackingUnit','PackingWeight','packingUnit'), packingWeight:num(source,'OuterEbTotal','TotalPackingWeight','packingWeight'), grossWeight:num(source,'GrossWeight','grossWeight'),
      rate:num(source,'OrderItemRate','ItemRate','Rate','rate'), rateUom:num(source,'RateUom','rateUom'), rateUomId:num(source,'RateUomId','rateUomId'), remarks:text(source,'Remarks','LoadingRemarks','remarks'),
      refPartyId:refId, refParty:refName, refDocumentTypeId:num(source,'RefDocumentTypeId'), refDocIdNo:num(source,'RefDocIdNo'), refDocSubIdNo:num(source,'RefDocSubIdNo')
    };
  }
  function numberInput(index,key,value) { return `<input type="number" step="0.001" min="0" data-index="${index}" data-key="${key}" value="${Number(value||0)}">`; }
  function renderRows() {
    $('detailTable').tBodies[0].innerHTML=rows.map((row,index)=>`<tr>
      <td><button type="button" class="row-action" data-edit="${index}">Edit</button></td><td><button type="button" class="row-action" data-delete="${index}">X</button></td>
      <td title="${html(row.customer)}">${html(row.customer)}</td><td>${html(row.refParty)}</td><td>${html(row.orderNo)}</td><td>${html(row.itemCode)}</td><td title="${html(row.itemName)}">${html(row.itemName)}</td><td>${html(row.packUom)}</td><td>${html(row.crop||row.cropYearId)}</td><td>${html(row.packingType||row.packingTypeId)}</td><td>${html(row.warehouse||row.warehouseId)}</td><td>${html(row.jobLot||row.jobLotId)}</td>
      <td>${numberInput(index,'quantity',row.quantity)}</td><td>${numberInput(index,'weight',row.weight)}</td><td>${numberInput(index,'packingUnit',row.packingUnit)}</td><td>${numberInput(index,'packingWeight',row.packingWeight)}</td><td>${numberInput(index,'grossWeight',row.grossWeight)}</td><td><input data-index="${index}" data-key="remarks" value="${html(row.remarks)}"></td></tr>`).join('');
    totals();
  }
  function totals() {
    $('totalQty').textContent=rows.reduce((s,r)=>s+Number(r.quantity||0),0).toFixed(3);
    $('totalWeight').textContent=rows.reduce((s,r)=>s+Number(r.weight||0),0).toFixed(3);
    $('totalPacking').textContent=rows.reduce((s,r)=>s+Number(r.packingWeight||0),0).toFixed(3);
    $('totalGross').textContent=rows.reduce((s,r)=>s+Number(r.grossWeight||0),0).toFixed(3);
  }
  function lineInput(event) {
    const input=event.target.closest('[data-key]'); if(!input)return;
    const row=rows[Number(input.dataset.index)], key=input.dataset.key;
    row[key]=key==='remarks'?input.value:Number(input.value||0);
    if(['quantity','packingUnit'].includes(key)){row.packingWeight=row.quantity*row.packingUnit;row.grossWeight=row.weight+row.packingWeight;renderRows();}
    else if(key==='weight'){row.grossWeight=row.weight+row.packingWeight;renderRows();} else totals();
  }
  function deleteRow(event) {
    const button=event.target.closest('[data-delete]'); if(!button)return;
    const index=Number(button.dataset.delete), row=rows[index]; if(row.id>0)removedLineIds.push(row.id); rows.splice(index,1); renderRows();
    if(!rows.length)lockType(false);
  }
  /* Expense grid (grdInvExpSettings :1656): + adds a blank row, X deletes, Item is a value list of other items */
  function blankExpense() { return {id:0, saleOrderId:0, saleOrderCustomerExpId:0, saleOrder:'', itemId:0, quantity:0, remarks:''}; }
  function renderExpenses() {
    if (!expenses.length) expenses.push(blankExpense());
    const itemOpts = '<option value=""></option>' + (initial?.otherItems || []).map(r => `<option value="${html(r.Id)}">${html(r.Name)}</option>`).join('');
    $('expenseTable').tBodies[0].innerHTML = expenses.map((e, i) => `<tr><td><button type="button" class="row-action" data-xadd="${i}">+</button></td><td><button type="button" class="row-action" data-xdel="${i}">X</button></td><td>${html(e.saleOrder || (e.saleOrderId ? e.saleOrderId : ''))}</td><td><select data-xi="${i}" data-xk="itemId">${itemOpts.replace(`value="${e.itemId}"`, `value="${e.itemId}" selected`)}</select></td><td><input type="number" step="any" data-xi="${i}" data-xk="quantity" value="${Number(e.quantity || 0)}"></td><td><input data-xi="${i}" data-xk="remarks" value="${html(e.remarks)}"></td></tr>`).join('');
    $('totalExpQty').textContent = expenses.reduce((s, e) => s + Number(e.quantity || 0), 0).toFixed(3);
  }
  async function mergeExpenses(detailIds) { /* LoadInGridExpense (:3030) */
    if (!detailIds.length) return;
    const found = await request(`${api}/order-expenses?ids=${encodeURIComponent(',' + detailIds.join(','))}`);
    expenses = expenses.filter(e => e.itemId || e.quantity || e.saleOrderCustomerExpId);
    for (const x of found || []) {
      const key = num(x, 'Id');
      if (expenses.some(e => e.saleOrderCustomerExpId === key)) continue;
      expenses.push({id:0, saleOrderId:num(x,'SaleOrderId'), saleOrderCustomerExpId:key, saleOrder:text(x,'SaleOrder'), itemId:num(x,'InvRevExpItemId'), quantity:num(x,'Qty'), remarks:text(x,'Remarks')});
    }
    renderExpenses();
  }

  /* ---- Load_SO_MainDetail (btnLoadSaleOrder_Click :3192) ---- */
  async function loadOrder() {
    if (!isLocal()) throw new Error('Please Select Other DeliveryOrderType');
    if (!$('saleType').value) throw new Error('Please Select Sale Type First');
    const typed=$('saleOrderSearch').value.trim();
    const match=(initial.pendingOrders||[]).find(row=>String(text(row,'Id'))===typed || typed.startsWith(`${text(row,'DocNo')} —`));
    if(!match)throw new Error('Select a pending sale order from the searchable list');
    const loaded=await request(`${api}/sale-order/${text(match,'Id')}/lines?saleTypeId=${sel('saleType')}`);
    const existing=new Set(rows.map(row=>row.saleOrderDetailId));
    const added=loaded.map(fromOrderLine).filter(row=>!existing.has(row.saleOrderDetailId)); added.forEach(row=>rows.push(row));
    if (added.length) await mergeExpenses(added.map(r => r.saleOrderDetailId));
    lockType(true); renderRows(); $('orderLoader').hidden=true;
  }

  /* ---- save / record ---- */
  function payload() { return {id:currentId,docDate:$('docDate').value,deliveryOrderType:$('deliveryOrderType').value||'Local',branchesId:sel('branchFrom'),saleTypeId:sel('saleType'),toBranchId:isLocal()?0:sel('branchTo'),transporterId:0,vehicleType:selText('vehicleType'),vehicleNo:$('vehicleNo').value.toUpperCase(),loadingInstructions:$('remarks').value,stockReserved:$('stockReserved').checked,removedLineIds,
    expenses:expenses.filter(e=>e.itemId||e.quantity||e.saleOrderCustomerExpId).map(e=>({id:e.id,saleOrderId:e.saleOrderId,saleOrderCustomerExpId:e.saleOrderCustomerExpId,itemId:e.itemId,quantity:e.quantity,remarks:e.remarks})),
    lines:rows.map(row=>({id:row.id,supplierCustomerId:row.supplierCustomerId,saleOrderId:row.saleOrderId,saleOrderDetailId:row.saleOrderDetailId,deliveryScheduleId:row.deliveryScheduleId,deliveryScheduleDetailId:row.deliveryScheduleDetailId,itemId:row.itemId,packUomId:row.packUomId,packingTypeId:row.packingTypeId,warehouseId:row.warehouseId,jobLotId:row.jobLotId,cropYearId:row.cropYearId,refPartyId:row.refPartyId,refDocumentTypeId:row.refDocumentTypeId,refDocIdNo:row.refDocIdNo,refDocSubIdNo:row.refDocSubIdNo,quantity:row.quantity,weight:row.weight,packingUnit:row.packingUnit,packingWeight:row.packingWeight,grossWeight:row.grossWeight,rate:row.rate,rateUom:row.rateUom,rateUomId:row.rateUomId,remarks:row.remarks}))}; }
  function validateHeader() { /* Insert (:1768) + FormValidation (:1107) */
    if (!rows.length) { message('Grid Record Not Found'); return false; }
    if (!sel('branchFrom')) { message('branch field is required'); return false; }
    if (!$('docNo').value.trim() || $('docNo').value.trim() === '0') { message('DocNo Field is Required'); return false; }
    if (!$('deliveryOrderType').value) { message('DeliveryOrderType Field is Required'); return false; }
    if (!sel('saleType')) { message('Sale Type Field is Required'); return false; }
    if (!isLocal()) {
      if (!sel('branchTo')) { message("'Branch To' Field is Required..."); return false; }
      if (sel('branchFrom') === sel('branchTo')) { message(" 'Branch From' Cannot Be Equal To 'Branch To' ..."); return false; }
    }
    if (isLocal() && rows.some(r => !r.saleOrderId)) { message('SaleOrder No Not Found'); return false; }
    return true;
  }
  async function save() {
    if (!validateHeader()) return;
    const updated = currentId > 0;
    if (!confirm(updated ? 'Are you sure to Update' : 'Are you sure to Save')) return;
    if (cfg.warnPackingWeight && rows.reduce((s, r) => s + Number(r.packingWeight || 0), 0) === 0 && !confirm('There is no Packing Weight added on any row. Are you Sure To Proceed?')) return;
    const record = await request(api, {method:'POST',body:JSON.stringify(payload())});
    const savedId = num(record,'Id','id'), docNo = text(record,'DocNo','docNo');
    await reload();
    message(`Data ${updated ? 'Update' : 'Save'} Successfully....  ${docNo}`, true);
    if (!updated && $('radGp').checked) window.open(`/sale/outward-gate-pass?basedOn=84&orderNo=${encodeURIComponent(docNo)}`, '_blank');
    if ($('chkPrint').checked) printSlip(savedId);
  }
  function printSlip(id) { if (!id) { message('PrintId not found...'); return; } if (typeof window.printRpt === 'function') window.printRpt('262-DeliveryOrderSlip.rpt', {id, documentTypeId: 84}); else message('Print is not available on this page.'); }
  function loadRecordObject(record) {
    setPane('Form');
    const type = text(record,'DeliveryOrderType','deliveryOrderType') || 'Local';
    if (!Array.from($('deliveryOrderType').options).some(o => o.value === type)) $('deliveryOrderType').add(new Option(type, type));
    $('deliveryOrderType').value = type;
    currentToBranchId = num(record,'ToBranchId','toBranchId');
    currentId = num(record,'Id','id','InvDeliveryOrderId','HeaderId'); $('docNo').value = text(record,'DocNo','docNo'); $('docDate').value = dateOnly(text(record,'DocDate','docDate'));
    { const vt = text(record,'VehicleType','vehicleType'), o = Array.from($('vehicleType').options).find(x => x.textContent.trim() === vt); if (o) $('vehicleType').value = o.value; else setSel('vehicleType', vt, vt); } $('vehicleNo').value = text(record,'VehicleNo','vehicleNo'); $('remarks').value = text(record,'LoadingInstructions','loadingInstructions');
    $('saleType').value = String(num(record,'SaleTypeId','saleTypeId') || 1); $('stockReserved').checked = String(text(record,'IsStockReserved','stockReserved')).toLowerCase() === 'true';
    const recordBranch = num(record,'BranchesId','branchesId'); if (recordBranch) $('branchFrom').value = String(recordBranch); /* DeliveryOrder.cs:2038 */
    if (currentToBranchId) $('branchTo').value = String(currentToBranchId);
    approved = String(text(record,'IsApproved')).toLowerCase() === 'true';
    applyType();
    rows = (record.lines || []).map(fromOrderLine); removedLineIds = [];
    expenses = (record.expenses || []).map(x => ({id:num(x,'Id'), saleOrderId:num(x,'SaleOrderId'), saleOrderCustomerExpId:num(x,'SaleOrderCustomerExpId'), saleOrder:text(x,'SaleOrder','SaleOrderNo'), itemId:num(x,'ItemId'), quantity:num(x,'Qty'), remarks:text(x,'Remarks')}));
    $('saveButton').hidden = true; $('updateButton').hidden = false; $('deleteButton').hidden = false; lockType(rows.length > 0);
    resetDetail(); renderRows(); renderExpenses(); window.scrollTo({top:0,behavior:'smooth'});
  }
  async function loadRecord(id) { loadRecordObject(await request(`${api}/${id}`)); }
  async function remove() {
    if (!currentId) { message('Record Id Not Found'); return; }
    if (!confirm('Are you sure to Delete?')) return;
    if (approved) { message('Record has been approved'); return; }
    await request(`${api}/${currentId}`, {method:'DELETE'}); await reload(); message('Record Deleted Successfully', true);
  }

  /* ---- reset / reload ---- */
  function reset() { /* Reset (:2200) */
    currentId = 0; rows = []; expenses = []; removedLineIds = []; currentToBranchId = 0; approved = false;
    const branch = (initial?.branches || []).find(r => r.UserBranch === true) || (initial?.branches || [])[0];
    if (branch) { $('branchFrom').value = text(branch,'BranchId','Id'); }
    $('docNo').value = initial?.nextDocNo || ''; $('docDate').value = today();
    $('deliveryOrderType').value = 'Local'; lockType(false);
    $('saleType').value = String((initial?.saleTypes || [])[0]?.Id || 1);
    $('vehicleType').value = ''; $('vehicleNo').value = ''; $('remarks').value = ''; $('stockReserved').checked = false;
    $('saveButton').hidden = false; $('updateButton').hidden = true; $('deleteButton').hidden = true;
    $('dCustomer').value = ''; $('dCrop').value = ''; resetDetail(); defaults(); applyType(); renderRows(); renderExpenses();
  }
  async function reload() { configure(await request(`${api}/initial`)); await refreshHistory(); }

  /* ---- history (client-side filters over the BLL FormHistory rows) ---- */
  let historyRows = [];
  function renderHistory() {
    const kind = document.querySelector('input[name=hDate]:checked').value, from = $('hFrom').value, to = $('hTo').value;
    const fromNo = Number($('hFromNo').value) || 0, toNo = Number($('hToNo').value) || 0, cust = $('hCustomer').value.trim().toLowerCase();
    const shown = historyRows.filter(row => {
      const d = dateOnly(text(row, kind, kind === 'DocDate' ? 'docDate' : kind));
      const n = num(row, 'DocNo', 'docNo');
      if (from && d && d < from) return false; if (to && d && d > to) return false;
      if (fromNo && n < fromNo) return false; if (toNo && n > toNo) return false;
      if (cust && !text(row, 'SupplierCustomer', 'CustomerName').toLowerCase().includes(cust)) return false;
      return true;
    });
    $('historyTable').tBodies[0].innerHTML = shown.map(row => {
      const recordId = num(row, 'Id', 'id', 'ID', 'InvDeliveryOrderId', 'HeaderId', 'DoId', 'InvDeliveryOrderHeaderId');
      const docNo = html(text(row, 'DocNo', 'docNo', 'DocCodeNo', 'VoucherNo'));
      return `<tr data-record="${recordId}"><td><button type="button" class="voucher-link" data-record="${recordId}">DO-${docNo}</button></td><td>${html(displayDate(text(row,'DocDate','docDate')))}</td><td>${html(text(row,'SupplierCustomer','partyName','supplierCustomer'))}</td><td>${html(text(row,'OrderNo','orderNo'))}</td><td>${html(text(row,'ItemCode','itemCode'))}</td><td>${html(text(row,'ItemName','itemName'))}</td><td>${html(text(row,'PackUOM','packUom'))}</td><td class="number">${html(text(row,'DoQty','quantity','qty'))}</td><td class="number">${html(text(row,'DoWeight','weight'))}</td><td>${html(text(row,'WareHouseName','warehouse'))}</td><td>${html(text(row,'VehicleNo','vehicleNo'))}</td><td>${html(text(row,'IsApproved','isApproved'))}</td><td>${html(text(row,'EntryUserName','entryUser'))}</td></tr>`;
    }).join('');
    $('historyCount').textContent = `Total Records: ${shown.length}`;
  }
  async function refreshHistory() { historyRows = (await request(`${api}/history`)) || []; renderHistory(); }
  function resetHistory() { for (const id of ['hFromNo','hToNo','hCustomer']) $(id).value = ''; setHistoryDates(); renderHistory(); }
  function setHistoryDates() { const days = cfg.historyDays || 3; $('hFrom').value = today(-days); $('hTo').value = today(); }

  function setPane(name) { $('paneForm').hidden = name !== 'Form'; $('paneHistory').hidden = name !== 'History'; $('tabForm').classList.toggle('on', name === 'Form'); $('tabHistory').classList.toggle('on', name === 'History'); }
  function setGridTab(name) { $('paneDetail').hidden = name !== 'Detail'; $('paneExpense').hidden = name !== 'Expense'; $('gtDetail').classList.toggle('on', name === 'Detail'); $('gtExpense').classList.toggle('on', name === 'Expense'); }

  async function run(action) {
    if (activeRequests) return;
    busy(true);
    try { await action(); } catch (error) { message(error.message || String(error)); } finally { busy(false); }
  }

  /* ---- wiring ---- */
  const on = (id, ev, fn) => $(id).addEventListener(ev, fn);
  on('deliveryOrderType', 'change', () => run(async () => { applyType(); await loadOrders(); }));
  on('saleType', 'change', applyType);
  on('dCustomer', 'change', () => run(loadOrders));
  on('dOrderNo', 'change', () => run(async () => { await loadOrderItems(); await balance(); }));
  on('dItem', 'change', () => run(async () => { bindUoms(); await balance(); }));
  on('dPackUom', 'change', () => run(async () => { calcWeight(); await balance(); }));
  on('rdName', 'change', bindItems); on('rdCode', 'change', bindItems);
  on('dQty', 'input', calcWeight); on('dWeight', 'input', () => calcGross(false)); on('dPackUnit', 'input', () => calcGross(false)); on('dPackWeight', 'input', () => calcGross(true));
  on('vehicleNo', 'input', e => { e.target.value = e.target.value.toUpperCase(); });
  on('btnPlus', 'click', () => run(addDetail)); on('btnUpdateDetail', 'click', () => run(updateDetail)); on('btnCancelDetail', 'click', resetDetail);
  on('detailTable', 'input', lineInput);
  on('detailTable', 'click', event => { deleteRow(event); const b = event.target.closest('[data-edit]'); if (b) run(() => editRow(Number(b.dataset.edit))); });
  on('expenseTable', 'click', event => {
    const a = event.target.closest('[data-xadd]'), d = event.target.closest('[data-xdel]');
    if (a) { expenses.splice(Number(a.dataset.xadd) + 1, 0, blankExpense()); renderExpenses(); }
    if (d) { expenses.splice(Number(d.dataset.xdel), 1); renderExpenses(); }
  });
  on('expenseTable', 'input', event => { const t = event.target.closest('[data-xi]'); if (!t) return; const e = expenses[Number(t.dataset.xi)]; e[t.dataset.xk] = t.dataset.xk === 'remarks' ? t.value : Number(t.value || 0); $('totalExpQty').textContent = expenses.reduce((s, x) => s + Number(x.quantity || 0), 0).toFixed(3); });
  on('expenseTable', 'change', event => { const t = event.target.closest('select[data-xi]'); if (t) expenses[Number(t.dataset.xi)].itemId = Number(t.value || 0); });
  on('historyTable', 'click', event => { const elem = event.target.closest('[data-record]'); if (elem) { const id = Number(elem.dataset.record); if (id > 0) run(() => loadRecord(id)); } });
  on('newButton', 'click', () => run(async () => reset())); on('refreshButton', 'click', () => run(reload));
  on('saveButton', 'click', () => run(save)); on('updateButton', 'click', () => run(save)); on('deleteButton', 'click', () => run(remove));
  on('printButton', 'click', event => { if (!currentId) { event.stopPropagation(); message('PrintId not found...'); } });
  on('attachmentButton', 'click', () => message('Save or load the delivery order before managing attachments.'));
  on('refPartiesButton', 'click', () => message('Define Reference Parties is not available on the web yet.'));
  on('shortcutButton', 'click', () => alert('Ctrl+N New | Ctrl+S Save | Ctrl+Delete Delete | Ctrl+T Form/History'));
  on('loadOrderButton', 'click', () => { if (!isLocal()) { message('Please Select Other DeliveryOrderType'); return; } if (!$('saleType').value) { message('Please Select Sale Type First'); return; } $('orderLoader').hidden = false; $('saleOrderSearch').focus(); });
  on('closeOrderLoader', 'click', () => { $('orderLoader').hidden = true; }); on('loadSelectedOrder', 'click', () => run(loadOrder));
  on('tabForm', 'click', () => setPane('Form')); on('tabHistory', 'click', () => setPane('History'));
  on('gtDetail', 'click', () => setGridTab('Detail')); on('gtExpense', 'click', () => setGridTab('Expense'));
  on('hShow', 'click', renderHistory); on('hReset', 'click', resetHistory);
  document.addEventListener('keydown', event => {
    const k = event.key.toLowerCase();
    if (event.ctrlKey && k === 's') { event.preventDefault(); run(save); }
    if (event.ctrlKey && k === 'n') { event.preventDefault(); run(async () => reset()); }
    if (event.ctrlKey && k === 't') { event.preventDefault(); setPane($('paneForm').hidden ? 'Form' : 'History'); }
    if (event.ctrlKey && event.key === 'Delete' && currentId) { event.preventDefault(); run(remove); }
  });
  run(async () => {
    await reload(); setHistoryDates(); renderHistory();
    const q = new URLSearchParams(location.search), id = Number(q.get('id') || q.get('record') || q.get('Id') || 0);
    if (Number.isSafeInteger(id) && id > 0) await loadRecord(id);
  });
})();
