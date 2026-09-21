(() => {
  'use strict';
  const api = '/sale/delivery-order/api';
  const $ = id => document.getElementById(id);
  let initial = null, currentId = 0, rows = [], removedLineIds = [], activeRequests = 0;
  const text = (o, ...keys) => { for (const k of keys) if (o && o[k] != null) return String(o[k]); return ''; };
  const num = (o, ...keys) => Number(text(o, ...keys) || 0);
  const html = value => String(value ?? '').replace(/[&<>'"]/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;',"'":'&#39;','"':'&quot;'}[c]));

  function busy(start) {
    activeRequests += start ? 1 : -1;
    activeRequests = Math.max(0, activeRequests);
    $('requestLoader').hidden = activeRequests === 0;
    document.querySelectorAll('button').forEach(button => button.disabled = activeRequests > 0);
  }
  async function request(url, options = {}) {
    busy(true); clearMessage();
    try {
      const response = await fetch(url, {headers:{'Content-Type':'application/json'}, ...options});
      const body = response.status === 204 ? null : await response.json().catch(() => null);
      if (!response.ok) throw new Error(body?.message || body?.detail || `Request failed (${response.status})`);
      return body;
    } finally { busy(false); }
  }
  function message(value, ok=false) { const box=$('message'); box.textContent=value; box.className=ok?'ok':''; }
  function clearMessage() { message(''); }
  function today() { return new Date().toISOString().slice(0,10); }
  function dateOnly(value) { return value ? String(value).slice(0,10) : ''; }
  function displayDate(value) { const v=dateOnly(value); if(!v)return ''; const [y,m,d]=v.split('-'); return `${d}-${m}-${y}`; }

  function fillOptions(element, list, valueKey, label) {
    element.innerHTML = list.map(row => `<option value="${html(text(row,valueKey,valueKey.toLowerCase()))}">${html(label(row))}</option>`).join('');
  }
  function reset() {
    currentId=0; rows=[]; removedLineIds=[];
    $('docNo').value=initial?.nextDocNo || '';
    $('docDate').value=today(); $('deliveryOrderType').value='Local';
    $('saleType').value='1'; $('vehicleType').value=''; $('vehicleNo').value=''; $('remarks').value=''; $('stockReserved').checked=false;
    $('saveButton').hidden=false; $('updateButton').hidden=true; $('deleteButton').hidden=true;
    renderRows();
  }
  function configure(data) {
    initial=data;
    fillOptions($('branchFrom'),data.branches||[],'BranchId',r=>text(r,'BranchName','branchName'));
    const branch=(data.branches||[])[0]; if(branch)$('branchFrom').value=text(branch,'BranchId','branchId','Id','id');
    fillOptions($('saleType'),data.saleTypes||[],'Id',r=>text(r,'Name'));
    fillOptions($('vehicleTypeList'),data.vehicleTypes||[],'VehicleDescription',r=>text(r,'VehicleDescription'));
    fillOptions($('saleOrderList'),data.pendingOrders||[],'Id',r=>`${text(r,'DocNo')} — ${text(r,'PartyName')} — ${dateOnly(text(r,'DocDate'))}`);
    reset();
  }

  function fromOrderLine(source) {
    return {
      id:num(source,'Id','id'), supplierCustomerId:num(source,'SupplierCustomerId','supplierCustomerId'), customer:text(source,'SupplierCustomer','supplierCustomer'),
      saleOrderId:num(source,'OrderId','SaleOrderId','saleOrderId'), orderNo:text(source,'OrderNo','orderNo'), saleOrderDetailId:num(source,'SaleOrderDetailId','saleOrderDetailId'),
      itemId:num(source,'ItemId','itemId'), itemCode:text(source,'ItemCode','itemCode'), itemName:text(source,'Item','ItemName','itemName'), packUomId:num(source,'ItemUOMId','PackUomId','packUomId'), packUom:text(source,'ItemUOM','PackUOM','packUom'),
      cropYearId:num(source,'CropYearId','cropYearId'), crop:text(source,'CropYear','crop'), packingTypeId:num(source,'PackingTypeId','InvPackingTypeId','packingTypeId'), packingType:text(source,'PackTypeDesc','PackingType','packingType'),
      warehouseId:num(source,'WareHouseId','WarehouseId','warehouseId'), warehouse:text(source,'WareHouseName','Warehouse','warehouse'), jobLotId:num(source,'JobLotId','jobLotId'), jobLot:text(source,'JobLotDescription','JobLot','jobLot'),
      quantity:num(source,'LoadQty','DoQty','LoadingQty','quantity'), weight:num(source,'LoadWeight','DoWeight','LoadingWeight','weight'), packingUnit:num(source,'PackingUnit','PackingWeight','packingUnit'), packingWeight:num(source,'OuterEbTotal','TotalPackingWeight','packingWeight'), grossWeight:num(source,'GrossWeight','grossWeight'),
      rate:num(source,'OrderItemRate','ItemRate','Rate','rate'), rateUom:num(source,'RateUom','rateUom'), rateUomId:num(source,'RateUomId','rateUomId'), remarks:text(source,'Remarks','LoadingRemarks','remarks'), refPartyId:num(source,'RefPartyId'), refDocumentTypeId:num(source,'RefDocumentTypeId'), refDocIdNo:num(source,'RefDocIdNo'), refDocSubIdNo:num(source,'RefDocSubIdNo')
    };
  }
  function numberInput(index,key,value) { return `<input type="number" step="0.001" min="0" data-index="${index}" data-key="${key}" value="${Number(value||0)}">`; }
  function renderRows() {
    $('detailTable').tBodies[0].innerHTML=rows.map((row,index)=>`<tr>
      <td><button type="button" class="row-action" data-edit="${index}">Edit</button></td><td><button type="button" class="row-action" data-delete="${index}">X</button></td>
      <td title="${html(row.customer)}">${html(row.customer)}</td><td>${html(row.orderNo)}</td><td>${html(row.itemCode)}</td><td title="${html(row.itemName)}">${html(row.itemName)}</td><td>${html(row.packUom)}</td><td>${html(row.crop||row.cropYearId)}</td><td>${html(row.packingType||row.packingTypeId)}</td><td>${html(row.warehouse||row.warehouseId)}</td><td>${html(row.jobLot||row.jobLotId)}</td>
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
  }
  function editRow(event) { const button=event.target.closest('[data-edit]'); if(button)$('detailTable').tBodies[0].rows[Number(button.dataset.edit)]?.querySelector('input')?.focus(); }

  async function loadOrder() {
    const typed=$('saleOrderSearch').value.trim();
    const match=(initial.pendingOrders||[]).find(row=>String(text(row,'Id'))===typed || typed.startsWith(`${text(row,'DocNo')} —`));
    if(!match)throw new Error('Select a pending sale order from the searchable list');
    const loaded=await request(`${api}/sale-order/${text(match,'Id')}/lines`);
    const existing=new Set(rows.map(row=>row.saleOrderDetailId)); loaded.map(fromOrderLine).filter(row=>!existing.has(row.saleOrderDetailId)).forEach(row=>rows.push(row));
    renderRows(); $('orderLoader').hidden=true;
  }
  function payload() { return {id:currentId,docDate:$('docDate').value,deliveryOrderType:'Local',saleTypeId:Number($('saleType').value),toBranchId:0,transporterId:0,vehicleType:$('vehicleType').value,vehicleNo:$('vehicleNo').value,loadingInstructions:$('remarks').value,stockReserved:$('stockReserved').checked,removedLineIds,
    lines:rows.map(row=>({id:row.id,supplierCustomerId:row.supplierCustomerId,saleOrderId:row.saleOrderId,saleOrderDetailId:row.saleOrderDetailId,itemId:row.itemId,packUomId:row.packUomId,packingTypeId:row.packingTypeId,warehouseId:row.warehouseId,jobLotId:row.jobLotId,cropYearId:row.cropYearId,refPartyId:row.refPartyId,refDocumentTypeId:row.refDocumentTypeId,refDocIdNo:row.refDocIdNo,refDocSubIdNo:row.refDocSubIdNo,quantity:row.quantity,weight:row.weight,packingUnit:row.packingUnit,packingWeight:row.packingWeight,grossWeight:row.grossWeight,rate:row.rate,rateUom:row.rateUom,rateUomId:row.rateUomId,remarks:row.remarks}))}; }
  async function save() { const updated=currentId>0; const record=await request(api,{method:'POST',body:JSON.stringify(payload())}); loadRecordObject(record); await refreshHistory(); message(`Delivery Order ${$('docNo').value} ${updated?'updated':'saved'} successfully`,true); }
  function loadRecordObject(record) {
    currentId=num(record,'Id','id','InvDeliveryOrderId','HeaderId'); $('docNo').value=text(record,'DocNo','docNo'); $('docDate').value=dateOnly(text(record,'DocDate','docDate')); $('vehicleType').value=text(record,'VehicleType','vehicleType'); $('vehicleNo').value=text(record,'VehicleNo','vehicleNo'); $('remarks').value=text(record,'LoadingInstructions','loadingInstructions'); $('saleType').value=String(num(record,'SaleTypeId','saleTypeId')||1); $('stockReserved').checked=String(text(record,'IsStockReserved','stockReserved')).toLowerCase()==='true';
    rows=(record.lines||[]).map(fromOrderLine); removedLineIds=[]; $('saveButton').hidden=true; $('updateButton').hidden=false; $('deleteButton').hidden=false; renderRows(); window.scrollTo({top:0,behavior:'smooth'});
  }
  async function loadRecord(id) { loadRecordObject(await request(`${api}/${id}`)); }
  async function remove() { if(!currentId)return; if(!confirm('Are you sure to Delete?'))return; await request(`${api}/${currentId}`,{method:'DELETE'}); await reload(); message('Record deleted successfully',true); }
  async function refreshHistory() {
    const data = await request(`${api}/history`), tbody = $('historyTable').tBodies[0];
    tbody.innerHTML = (data || []).map(row => {
      const recordId = num(row, 'Id', 'id', 'ID', 'InvDeliveryOrderId', 'HeaderId', 'DoId', 'InvDeliveryOrderHeaderId');
      const docNo = html(text(row, 'DocNo', 'docNo', 'DocCodeNo', 'VoucherNo'));
      return `<tr data-record="${recordId}" style="cursor: pointer;">
        <td><button type="button" class="voucher-link" data-record="${recordId}">DO-${docNo}</button></td>
        <td>${html(displayDate(text(row, 'DocDate', 'docDate')))}</td>
        <td>${html(text(row, 'SupplierCustomer', 'partyName', 'supplierCustomer'))}</td>
        <td>${html(text(row, 'OrderNo', 'orderNo'))}</td>
        <td>${html(text(row, 'ItemCode', 'itemCode'))}</td>
        <td>${html(text(row, 'ItemName', 'itemName'))}</td>
        <td>${html(text(row, 'PackUOM', 'packUom'))}</td>
        <td>${html(text(row, 'DoQty', 'quantity', 'qty'))}</td>
        <td>${html(text(row, 'DoWeight', 'weight'))}</td>
        <td>${html(text(row, 'WareHouseName', 'warehouse'))}</td>
        <td>${html(text(row, 'VehicleNo', 'vehicleNo'))}</td>
        <td>${html(text(row, 'IsApproved', 'isApproved'))}</td>
        <td>${html(text(row, 'EntryUserName', 'entryUser'))}</td>
      </tr>`;
    }).join('');
  }
  async function reload() { configure(await request(`${api}/initial`)); await refreshHistory(); }
  async function run(action) { try{await action();}catch(error){message(error.message||String(error));} }

  $('detailTable').addEventListener('input',lineInput); $('detailTable').addEventListener('click',event=>{deleteRow(event);editRow(event);});
  $('historyTable').addEventListener('click',event=>{
    const elem = event.target.closest('[data-record]');
    if (elem) {
      const id = Number(elem.dataset.record);
      if (id > 0) run(() => loadRecord(id));
    }
  });
  $('newButton').addEventListener('click',reset); $('saveButton').addEventListener('click',()=>run(save)); $('updateButton').addEventListener('click',()=>run(save)); $('deleteButton').addEventListener('click',()=>run(remove));
  $('loadOrderButton').addEventListener('click',()=>{$('orderLoader').hidden=false;$('saleOrderSearch').focus();}); $('closeOrderLoader').addEventListener('click',()=>{$('orderLoader').hidden=true;}); $('loadSelectedOrder').addEventListener('click',()=>run(loadOrder));
  $('historyButton').addEventListener('click',()=>$('historyPanel').scrollIntoView({behavior:'smooth'})); $('refreshButton').addEventListener('click',()=>run(reload));
  document.addEventListener('keydown',event=>{if(event.ctrlKey&&event.key.toLowerCase()==='s'){event.preventDefault();run(save);}if(event.ctrlKey&&event.key.toLowerCase()==='n'){event.preventDefault();reset();}if(event.ctrlKey&&event.key==='Delete'&&currentId){event.preventDefault();run(remove);}});
  run(reload);
})();
