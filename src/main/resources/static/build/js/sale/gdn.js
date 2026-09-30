(function($){'use strict';
const api='/sale/gdn/api',s={initial:{},orders:[],uoms:[],details:[],removed:[],expenses:[],history:[],edit:-1,busy:0,shortWeightDeductionApply:false,isStockReserved:false,billing:{},ebMode:"unit",stockEbMode:"unit"};
const $i=x=>$('#'+x),num=x=>Number(x||0),val=(o,...ks)=>{for(const k of ks)if(o&&o[k]!=null)return o[k];return''},esc=x=>$('<div>').text(x==null?'':x).html(),iso=gdnDate,round=x=>Math.round((num(x)+Number.EPSILON)*1000)/1000,label=id=>$i(id).find('option:selected').text()||'';

function gdnDate(value){
 if(!value)return '';
 const text=String(value);
 // SQL datetime is serialized as an instant. Restore its local calendar date;
 // date-only and timezone-free desktop values already contain the correct day.
 if(/T.*(?:Z|[+-]\d{2}:?\d{2})$/i.test(text)){
  const date=new Date(text);
  if(!Number.isNaN(date.getTime()))return [date.getFullYear(),String(date.getMonth()+1).padStart(2,'0'),String(date.getDate()).padStart(2,'0')].join('-');
 }
 return text.slice(0,10);
}

const disabledBefore=new Map(),pendingReads=new Map();
function busy(on){
 s.busy=Math.max(0,s.busy+(on?1:-1));
 $i('busy').toggleClass('show',s.busy>0);
 if(s.busy)document.querySelectorAll('button').forEach(button=>{
  if(!disabledBefore.has(button))disabledBefore.set(button,{disabled:button.disabled,busy:button.getAttribute('aria-busy')});
  button.disabled=true;button.setAttribute('aria-busy','true');
 });
 else{
  for(const[button,state]of disabledBefore){
   button.disabled=state.disabled;
   if(state.busy===null)button.removeAttribute('aria-busy');else button.setAttribute('aria-busy',state.busy);
  }
  disabledBefore.clear();
 }
}
function req(url,opt){
 const key=(!opt?.method||opt.method==='GET')?url:null;
 if(key&&pendingReads.has(key))return pendingReads.get(key);
 busy(true);
 const work=(async()=>{try{
  const response=await fetch(url,opt),text=await response.text();
  if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw Error('Your session has expired. Please sign in again.');
  let data={};try{data=text?JSON.parse(text):{}}catch{
   throw Error(response.ok?'The server did not return valid data. Please refresh or sign in again.':'Request failed ('+response.status+')');
  }
  if(!response.ok)throw Error(data.message||data.detail||data.error||'Request failed ('+response.status+')');
  return data;
 }finally{if(key)pendingReads.delete(key);busy(false)}})();
 if(key)pendingReads.set(key,work);
 return work;
}

function msg(x,ok){$i('message').text(x||'').css('color',ok?'#075f28':'#b00000')}
/* The drop grids rendered by build/js/countx_desktop_combo.js read their extra columns from
   data- attributes on each <option>; the column SET is declared on the <select> itself
   (data-dtcombo="party4" | …). InvFrmGDN.cs:919-926 binds CmbCustomerName from a dtSupplier of
   Id, CompanyName, PartyCode, GlAccountId, CityId, CityName, MobileNo and hides columns 3 and 4,
   leaving Name | PartyCode | CityName | MobileNo.

   Only real columns of the returned row are published - a row without one gets no attribute and
   the grid renders that cell EMPTY rather than substituting anything. */
function comboColumnAttrs(r){
  if(!r) return '';
  let out='';
  const pick=(...k)=>{for(const n of k){const v=r[n];if(v!==undefined&&v!==null&&v!=='')return v;}return null;};
  const add=(a,v)=>{if(v!==null&&v!==undefined)out+=` data-${a}="${esc(v)}"`;};
  add('code',   pick('PartyCode','partyCode','AccountCode','accountCode'));
  add('city',   pick('CityName','cityName'));
  add('mobile', pick('MobileNo','mobileNo','MobilePersonal'));
  add('item-code', pick('ItemCode','itemCode'));
  add('item-category', pick('ItemCategory','itemCategory'));
  const eq=pick('Equivalent','equivalent'); if(eq!==null) add('eq',eq);
  const br=r.BaseRateUom!==undefined?r.BaseRateUom:r.baseRateUom;
  if(br!==undefined&&br!==null) add('base', br===true?1:br===false?0:br);
  return out;
}
function fill(id,rows,ids,names,blank=''){const q=$i(id),old=q.val();q.html(`<option value="">${esc(blank)}</option>`+(rows||[]).map(r=>`<option value="${esc(val(r,...ids))}"${comboColumnAttrs(r)}>${esc(val(r,...names))}</option>`).join(''));if(old)q.val(old);q.trigger('change.select2')}
function search(){/* select2 removed - build/js/countx_desktop_combo.js renders every combo on this screen, so Sale, Purchase and CMAGT all draw the same control. Leaving select2 on some of them produced two different-looking dropdowns on one form. */}
function lookup(id,rows,ids){const x=String($i(id).val()||'');return(rows||[]).find(r=>String(val(r,...ids))===x)||{}}
function initial(){return req(api+'/initial').then(d=>{s.initial=d;fill('customer',d.customers,['Id','id'],['CompanyName','SupplierName']);fill('stockParty',d.customers,['Id','id'],['CompanyName','SupplierName']);fill('historyCustomer',d.customers,['Id','id'],['CompanyName','SupplierName']);fill('gatePass',d.gatePasses,['Id','OutwardGatePassId'],['GpSrNo','GpNo']);fill('lineItem',d.items,['Id'],['ItemName']);fill('lineBrand',d.brands,['Id'],['BrandName','Description']);fill('linePacking',d.packingTypes,['Id'],['PackTypeDesc','PackingType']);fill('lineCrop',d.crops,['Id'],['CropYear','Description']);fill('lineJob',d.jobLots,['Id'],['JobLotDescription','JobLot']);fill('lineWarehouse',d.warehouses,['Id','WarehouseId'],['WareHouseName','WarehouseName']);fill('lineWarehouseTo',d.warehouses,['Id','WarehouseId'],['WareHouseName','WarehouseName']);fill('lineCity',d.cities,['Id'],['CityName','Description']);fill('transporter',d.transporters,['Id','GlAccountId'],['AccountTitle','CompanyName']);fill('expenseItem',d.otherItems,['Id'],['ItemName','Description']);$i('docNo').val(val(d,'nextNo'));pending(d.gatePasses||[]);reservationControls();search();if(!$i('docDate').val())$i('docDate').val(gdnDate(new Date().toISOString()))})}
async function customerChanged(){
 const customer=num($i('customer').val()),gdn=num($i('recordId').val());
 if(!customer){s.orders=[];fill('advanceDo',[],['Id'],['DocNo']);return;}
 const rows=await req(api+'/advance-orders?customerId='+customer+'&gdnId='+gdn);
 s.orders=rows;fill('advanceDo',rows,['Id','InvDeliveryOrderId'],['DocNo','OrderNo']);
}
function reservationControls(){
 const enabled=s.initial.stockReservationEnabled===true,active=enabled&&s.isStockReserved;
 $i('stockReservationLabel').prop('hidden',!enabled);
 $i('stockReserved').prop('checked',s.isStockReserved);
 $('.reservation-control').prop('hidden',!active);
 $i('lineWarehouseTo').closest('.field').prop('hidden',!active);
 $('#detailPane th.warehouse-to,#detailRows td.warehouse-to').prop('hidden',!active);
}
function billingControls(){
 const shortage=num(s.billing.ShortExcessWeight),pack=num(s.billing.SaleTypeId)===2;
 $i('doNetWeight').val(num(s.billing.TotalDoWeight));$i('shortWeight').val(shortage);
 $i('shortWeightLabel').text(shortage<0&&pack?'Short Weight':shortage>0?'Excess Weight':'Short/Excess Wt');
 $i('shortDeductionLabel').prop('hidden',!(shortage<0&&pack));
 $i('shortDeduction').prop('checked',s.shortWeightDeductionApply);
 if(s.billing.SaleTypeId)set('saleType',s.billing.SaleTypeId);
 emptyBagControls();
}
function emptyBagControls(){
 const enabled=num($i('saleType').val())!==2||s.initial.ebWeightEditableForPackBilling===true;
 $('#lineEbUnit,#lineEbTotal').prop('disabled',!enabled).closest('.field').prop('hidden',!enabled);
 $('.billing-empty-bag').prop('hidden',!enabled);
 if(!enabled){$i('lineEbUnit').val(0);$i('lineEbTotal').val(0);s.details.forEach(row=>{row.EBWPerUnit=0;row.EBWTotal=0})}
}
async function generateStock(){
 if(s.busy)return;
 if(!s.details.length)return msg('Add or load detail rows first.');
 const date=$i('docDate').val();if(!date)return msg('Document Date is required');
 busy(true);
 try{
  const rows=s.details.slice(),lines=rows.map(r=>({itemId:num(r.ItemId),warehouseId:num(r.WarehouseId),cropYear:r.CropYear,
   jobLotId:num(r.JobLotId),packingTypeId:num(r.PackingTypeId),itemUomId:num(r.ItemUomId)}));
  const stocks=await req(api+'/available-stock',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({docDate:date,lines})});
  if(!Array.isArray(stocks)||stocks.length!==rows.length)throw Error('Stock response did not match the detail rows');
  rows.forEach((row,index)=>{row.AvailableStock=num(stocks[index])});renderDetails();msg('Available stock loaded.',true);
 }catch(error){msg(error.message)}finally{busy(false)}
}
async function printDocument(id,variant='gdn-260'){
 if(s.busy)return;
 id=num(id||$i('recordId').val());if(!id)return msg('Load or save a GDN before printing.');
 busy(true);
 try{
  const status=await req('/api/reports/print/status');
  if(!status.available)throw Error('The desktop report engine is unavailable. Please contact your administrator.');
  const response=await fetch('/api/reports/'+variant+'/print.pdf',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({id})});
  if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw Error('Your session has expired. Please sign in again.');
  if(!response.ok)throw Error(await response.text()||'Report could not be printed.');
  if(!(response.headers.get('Content-Type')||'').includes('application/pdf'))throw Error('The report did not return a PDF.');
  const url=URL.createObjectURL(await response.blob()),link=document.createElement('a');
  link.href=url;link.download='GDN-'+($i('docNo').val()||id)+(variant==='gdn-260a'?'-Delivery-Challan':'')+'.pdf';link.click();
  setTimeout(()=>URL.revokeObjectURL(url),60000);msg('Report ready.',true);
 }catch(error){msg(error.message)}finally{busy(false)}
}
async function loadBilling(gp){
 s.billing=gp?await req(api+'/gate-pass/'+gp+'/billing'):{};
 billingControls();recalcHeader();
}
// InvFrmGDN.ProportionateShortWeight and checkbox event, lines 2981 and 4704.
function shortWeightFor(row){
 const total=num(s.billing.TotalDoWeight);
 return total>0?Math.abs(num(s.billing.ShortExcessWeight))/total*num(row.ItemQty)*num(row.UOMEquivalent):0;
}
function deductionChanged(){
 s.shortWeightDeductionApply=$i('shortDeduction').prop('checked');
 s.details.forEach(r=>{
  if(s.shortWeightDeductionApply)r.ShortWeight=shortWeightFor(r);
  r.NetBillWeight=num(r.GrossWeight)-num(r.EBWTotal)+num(r.AdLsWeight)-(s.shortWeightDeductionApply?num(r.ShortWeight):0);
 });
 renderDetails();
}

function uoms(){const id=num($i('lineItem').val());if(!id)return fill('lineUom',[],['Id'],['UOMCode']);return req(api+'/uoms?itemId='+id).then(rows=>{s.uoms=rows;fill('lineUom',rows,['Id'],['UOMCode','UomCode'])}).catch(x=>msg(x.message))}
function calc(event){
 const changed=event?.target?.id,q=num($i('lineQty').val()),pack=num($i('saleType').val())===2;
 if(changed==='lineEbTotal')s.ebMode='total';else if(changed==='lineEbUnit')s.ebMode='unit';
 if(changed==='lineEbStockTotal')s.stockEbMode='total';else if(changed==='lineEbStockUnit')s.stockEbMode='unit';
 let eb=num($i('lineEbTotal').val()),unit=num($i('lineEbUnit').val()),stockEb=num($i('lineEbStockTotal').val()),stockUnit=num($i('lineEbStockUnit').val());
 if(s.ebMode==='total')unit=q?eb/q:0;else eb=q*unit;
 if(s.stockEbMode==='total')stockUnit=q?stockEb/q:0;else stockEb=q*stockUnit;
 const equivalent=num(val(lookup('lineUom',s.uoms,['Id']),'Equivalent'))||num(s.details[s.edit]?.UOMEquivalent);
 let gross=num($i('lineGross').val());
 if(pack&&equivalent>0)gross=q*equivalent;
 const add=num($i('lineAddWeight').val()),short=pack&&s.shortWeightDeductionApply?shortWeightFor({ItemQty:q,UOMEquivalent:equivalent}):0;
 $i('lineGross').val(gross);$i('lineEbUnit').val(unit);$i('lineEbTotal').val(eb);
 $i('lineEbStockUnit').val(stockUnit);$i('lineEbStockTotal').val(stockEb);
 $i('lineNet').val(round(gross-eb+add-short));$i('lineStock').val(round(gross-stockEb+add));
}
function recalcHeader(){
 const gross=s.details.reduce((a,r)=>a+num(r.GrossWeight),0),factory=num($i('factoryWeight').val()),party=num($i('customerWeight').val());
 const dispatched=Math.max(0,num(s.billing.DispatchedGrossWeight)-(num($i('recordId').val())?num(s.loadedGross):0));
 $i('dispatchWeight').val(round(dispatched));$i('balanceGross').val(round(factory-gross));
 $i('balanceWeight').val(round(factory-dispatched));$i('weightDiff').val(round(factory-party));
}

function editorValues(){const it=lookup('lineItem',s.initial.items,['Id']),ord=lookup('lineOrder',s.orders,['Id','InvDeliveryOrderId']);return{Id:s.edit>=0?num(s.details[s.edit].Id):0,ActionTypeId:s.edit>=0&&num(s.details[s.edit].Id)?2:1,OrderId:num($i('lineOrder').val()),OrderNo:label('lineOrder'),SaleOrderId:num(val(ord,'SaleOrderId','OrderId'))||num($i('lineOrder').val()),SaleOrderDetailId:num(val(ord,'SaleOrderDetailId','OrderDetailId')),RefPartyId:num(val(ord,'RefPartyId')),ItemId:num($i('lineItem').val()),ItemCode:val(it,'ItemCodeNew','ItemCode'),Item:val(it,'ItemName'),BrandId:num($i('lineBrand').val()),Brand:label('lineBrand'),PackingTypeId:num($i('linePacking').val()),PackingType:label('linePacking'),ItemQty:num($i('lineQty').val()),LabReportRef:$i('lineLab').val(),ItemUomId:num($i('lineUom').val()),UOMCode:label('lineUom'),GrossWeight:num($i('lineGross').val()),EBWPerUnit:num($i('lineEbUnit').val()),EBWTotal:num($i('lineEbTotal').val()),AdLsWeight:num($i('lineAddWeight').val()),CityId:num($i('lineCity').val()),AreaCity:label('lineCity'),NetBillWeight:num($i('lineNet').val()),StockWeight:num($i('lineStock').val()),CropYearId:num($i('lineCrop').val()),CropYear:label('lineCrop'),JobLotId:num($i('lineJob').val()),JobLot:label('lineJob'),WarehouseId:num($i('lineWarehouse').val()),WareHouseCode:label('lineWarehouse'),ContainerNo:$i('lineContainer').val(),WtCut:num($i('lineWtCut').val()),WtCutTotal:round(num($i('lineQty').val())*num($i('lineWtCut').val())),WarehouseToId:num($i('lineWarehouseTo').val()),WarehouseTo:label('lineWarehouseTo'),RefDocumentTypeId:num(val(ord,'RefDocumentTypeId'))||84,RefDocIdNo:num(val(ord,'RefDocIdNo'))||num($i('lineOrder').val()),RefDocSubIdNo:num(val(ord,'RefDocSubIdNo','Id')),RefDocNo:label('lineOrder')};}
function lineObject(){
 const entered=editorValues(),existing=s.edit>=0?s.details[s.edit]:null;
 const row=Object.assign({},existing||{},entered);
 if(existing){
  for(const key of ['OrderId','OrderNo','SaleOrderId','SaleOrderDetailId','RefPartyId','RefDocumentTypeId','RefDocIdNo','RefDocSubIdNo','RefDocNo','InvDeliveryOrderId','InvDeliveryOrderDetailId','ItemRate'])
   row[key]=existing[key];
 }else{
  // Native manual rows have no stock reference; an SO is not a stock reference.
  row.RefDocumentTypeId=0;row.RefDocIdNo=0;row.RefDocSubIdNo=0;row.RefDocNo=0;
 }
 row.BrandItemId=entered.BrandId;row.WareHouseToId=entered.WarehouseToId;
 row.UOMEquivalent=num(val(lookup('lineUom',s.uoms,['Id']),'Equivalent'))||num(existing?.UOMEquivalent);
 row.EbUnitStock=num($i('lineEbStockUnit').val());row.EbTotalStock=num($i('lineEbStockTotal').val());
 row.ShortWeight=s.shortWeightDeductionApply?shortWeightFor(row):num(existing?.ShortWeight);
 return row;
}
function valid(r){for(const x of [['Order No',r.OrderId],['Item',r.ItemId],['Packing Type',r.PackingTypeId],['UOM',r.ItemUomId],['City/Area',r.CityId],['Crop Year',r.CropYearId],['Job/Lot',r.JobLotId],['Warehouse',r.WarehouseId],['Qty',r.ItemQty],['Gross Weight',r.GrossWeight],['Net Bill Weight',r.NetBillWeight],['Stock Weight',r.StockWeight]])if(!x[1])throw Error(x[0]+' is required')}
function add(){try{const r=lineObject();valid(r);if(s.edit>=0)s.details[s.edit]=r;else s.details.push(r);cancel();renderDetails();msg('')}catch(x){msg(x.message)}}
function set(id,x){$i(id).val(x==null?'':x).trigger('change.select2')}
function edit(i){const r=s.details[i];s.edit=i;fill('lineOrder',[r],['OrderId'],['OrderNo']);set('lineOrder',r.OrderId);$i('lineOrder').prop('disabled',true);set('lineItem',r.ItemId);set('lineBrand',r.BrandId);set('linePacking',r.PackingTypeId);set('lineCity',r.CityId);set('lineCrop',r.CropYearId);set('lineJob',r.JobLotId);set('lineWarehouse',r.WarehouseId);set('lineWarehouseTo',r.WarehouseToId);req(api+'/uoms?itemId='+r.ItemId).then(x=>{s.uoms=x;fill('lineUom',x,['Id'],['UOMCode']);set('lineUom',r.ItemUomId)}).catch(x=>msg(x.message));[['lineQty','ItemQty'],['lineLab','LabReportRef'],['lineGross','GrossWeight'],['lineEbUnit','EBWPerUnit'],['lineEbTotal','EBWTotal'],['lineEbStockUnit','EbUnitStock'],['lineEbStockTotal','EbTotalStock'],['lineAddWeight','AdLsWeight'],['lineNet','NetBillWeight'],['lineStock','StockWeight'],['lineContainer','ContainerNo'],['lineWtCut','WtCut']].forEach(x=>$i(x[0]).val(r[x[1]]));$i('btnAddLine').text('Update');$i('btnCancelLine').prop('hidden',false)}
function cancel(){s.edit=-1;s.ebMode='unit';s.stockEbMode='unit';$i('lineOrder').prop('disabled',false);['lineQty','lineLab','lineGross','lineEbUnit','lineEbTotal','lineEbStockUnit','lineEbStockTotal','lineAddWeight','lineNet','lineStock','lineContainer','lineWtCut'].forEach(x=>$i(x).val(''));$i('btnAddLine').text('+');$i('btnCancelLine').prop('hidden',true)}
function removeLine(i){if(s.busy)return;if(s.edit===i)cancel();else if(s.edit>i)s.edit--;const r=s.details.splice(i,1)[0];if(num(r.Id)){r.ActionTypeId=3;s.removed.push(r)}renderDetails()}
const detailCols=['OrderNo','ItemCode','Item','Brand','PackingType','ItemQty','LabReportRef','UOMCode','GrossWeight','EBWPerUnit','EBWTotal','AdLsWeight','AreaCity','NetBillWeight','EbUnitStock','EbTotalStock','StockWeight','CropYear','JobLot','WareHouseCode','ContainerNo','WtCut','WtCutTotal','AvailableStock','WarehouseTo'];
function referenceLink(r){
 if(num(r.RefDocumentTypeId)>0&&num(r.RefDocIdNo)>0){
  const caption=[r.RefDocType,num(r.RefDocNo)||''].filter(Boolean).join(' ')||'View document';
  return `<button type="button" class="tool row-link document-link" data-type="${num(r.RefDocumentTypeId)}" data-ref="${num(r.RefDocIdNo)}">${esc(caption)}</button>`;
 }
 if(num(r.InvDeliveryOrderId)>0)return `<a class="row-link" href="/sale/delivery-order?id=${num(r.InvDeliveryOrderId)}">Delivery Order</a>`;
 return esc(r.RefDocNo||'');
}
function renderDetails(){$i('detailRows').html(s.details.length?s.details.map((r,i)=>`<tr><td><button class="tool edit" data-i="${i}">Edit</button></td>${detailCols.map(k=>`<td class="${k==='WarehouseTo'?'warehouse-to':['EBWPerUnit','EBWTotal'].includes(k)?'billing-empty-bag':''}">${esc(r[k])}</td>`).join('')}<td>${referenceLink(r)}</td><td><button class="tool del" data-i="${i}">Delete</button></td></tr>`).join(''):'<tr><td colspan="28" class="empty">No detail rows</td></tr>');reservationControls();emptyBagControls();recalcHeader()}
function loadOrder(){const id=num(s.billing.SaleOrderId),customer=num($i('customer').val());if(!id||!customer)return;return req(api+'/delivery-order/'+id+'?customerId='+customer).then(rows=>{if(!rows.length)throw Error('Delivery Order has no available detail');rows.forEach(x=>{const row=mapLine(x);if(num($i('saleType').val())===2){row.EBWPerUnit=0;row.EBWTotal=0;}row.NetBillWeight=row.GrossWeight-row.EBWTotal+row.AdLsWeight;row.StockWeight=row.GrossWeight-row.EbTotalStock+row.AdLsWeight;s.details.push(row)});renderDetails();return loadExpenses([...new Set(s.details.map(r=>r.InvDeliveryOrderId).filter(Boolean))])}).catch(x=>msg(x.message))}
function mapPendingValues(r){return{Id:0,ActionTypeId:1,OrderId:num(val(r,'InvDeliveryOrderId','DeliveryOrderId','Id')),OrderNo:val(r,'DocNo','OrderNo'),SaleOrderId:num(val(r,'SaleOrderId')),SaleOrderDetailId:num(val(r,'SaleOrderDetailId')),RefPartyId:num(val(r,'RefPartyId')),ItemId:num(val(r,'ItemId')),ItemCode:val(r,'ItemCode'),Item:val(r,'ItemName','Item'),BrandId:num(val(r,'BrandId')),Brand:val(r,'BrandName','Brand'),PackingTypeId:num(val(r,'InvPackingTypeId','PackingTypeId')),PackingType:val(r,'PackTypeDesc','PackingType'),ItemQty:num(val(r,'BalanceQty','DoQty','Qty')),LabReportRef:val(r,'LabReportRef','LabNo'),ItemUomId:num(val(r,'PackUomId','ItemUomId','UOMId')),UOMCode:val(r,'UOMCode','UOM'),GrossWeight:num(val(r,'BalanceWeight','DoWeight','GrossWeight')),EBWPerUnit:num(val(r,'EBWPerUnit','EbUnit')),EBWTotal:num(val(r,'EBWTotal','EbTotal')),AdLsWeight:num(val(r,'AdLsWeight','AddLesswt')),CityId:num(val(r,'CityId')),AreaCity:val(r,'CityName','AreaCity'),NetBillWeight:num(val(r,'NetBillWeight','NetWeight','DoWeight')),StockWeight:num(val(r,'StockWeight','DoWeight')),CropYearId:num(val(r,'CropYearId')),CropYear:val(r,'CropYear'),JobLotId:num(val(r,'JobLotId','JobId')),JobLot:val(r,'JobLotDescription','JobLot'),WarehouseId:num(val(r,'WarehouseId','WareHouse')),WareHouseCode:val(r,'WareHouseName','WarehouseName'),ContainerNo:val(r,'ContainerNo'),WtCut:num(val(r,'WtCut')),WtCutTotal:num(val(r,'WtCutTotal')),WarehouseToId:num(val(r,'WarehouseToId')),WarehouseTo:val(r,'WarehouseTo'),RefDocumentTypeId:num(val(r,'RefDocumentTypeId'))||84,RefDocIdNo:num(val(r,'RefDocIdNo'))||num(val(r,'InvDeliveryOrderId','Id')),RefDocSubIdNo:num(val(r,'RefDocSubIdNo'))||num(val(r,'InvDeliveryOrderDetailId','DetailId')),RefDocNo:val(r,'DocNo','OrderNo')}}
// Native DeliveryOrderLoad aliases (InvFrmGDN.cs:1666); SO and DO identities differ.
function mapLine(r){
 const x=Object.assign({},r,mapPendingValues(r));
 x.OrderId=num(val(r,'OrderId','SaleOrderId'));x.SaleOrderId=x.OrderId;x.OrderNo=val(r,'OrderNo','SaleOrderNo');
 x.ItemQty=num(val(r,'Qty','ItemQty'));x.GrossWeight=num(val(r,'GrossWight','GrossWeight'));
 x.UOMEquivalent=num(val(r,'PUomEquivalent','UOMEquivalent'));x.ItemUomId=num(val(r,'UOMId','ItemUomId'));
 x.EbUnitStock=num(val(r,'EbUnitStock','EbUnit'));x.EbTotalStock=num(val(r,'EbTotalStock','EbTotal'));
 x.EBWPerUnit=num(val(r,'EBWPerUnit','EbUnit'));x.EBWTotal=num(val(r,'EBWTotal','EbTotal'));
 x.NetBillWeight=num(val(r,'NetBillWeight','NetWeight'));x.StockWeight=num(r.StockWeight);
 x.JobLot=val(r,'Job','JobLot');x.AreaCity=val(r,'City','AreaCity');x.CropYearId=num(r.CropYearId);
 x.InvDeliveryOrderId=num(r.InvDeliveryOrderId);x.InvDeliveryOrderDetailId=num(r.InvDeliveryOrderDetailId);
 x.WarehouseToId=num(val(r,'WareHouseToId','WareHouse'));x.WareHouseToId=x.WarehouseToId;
 for(const key of ['RefDocumentTypeId','RefDocIdNo','RefDocSubIdNo','RefDocNo'])x[key]=num(r[key]);
 return x;
}
function loadExpenses(ids){if(!ids.length){s.expenses=[];return renderExpenses()}return req(api+'/expenses?deliveryOrderIds='+ids.join(',')).then(rows=>{s.expenses=rows.map(x=>({ItemId:num(val(x,'ItemId')),ItemName:val(x,'ItemName','Description'),Qty:num(val(x,'Qty')),Remarks:val(x,'Remarks')}));renderExpenses()})}
function addExpense(){const id=num($i('expenseItem').val()),qty=num($i('expenseQty').val());if(!id&&!qty)return msg('Select an expense or enter quantity');s.expenses.push({ItemId:id,ItemName:label('expenseItem'),Qty:qty,Remarks:$i('expenseRemarks').val()});$i('expenseQty,#expenseRemarks').val('');renderExpenses()}
function renderExpenses(){$i('expenseRows').html(s.expenses.length?s.expenses.map((r,i)=>`<tr><td>${esc(r.ItemName)}</td><td>${esc(r.Qty)}</td><td>${esc(r.Remarks)}</td><td><button class="tool exp-del" data-i="${i}">Delete</button></td></tr>`).join(''):'<tr><td colspan="4" class="empty">No expense rows</td></tr>')}
function pending(rows){$i('pendingRows').html(rows.length?rows.map((r,i)=>`<tr><td><button class="action load-gp" data-i="${i}">Load</button></td><td class="row-link load-gp" data-i="${i}">${esc(val(r,'GpSrNo','GpNo'))}</td><td>${esc(iso(val(r,'GpDate')))}</td><td>${esc(val(r,'Status'))}</td><td>${esc(val(r,'SupplierName','CustomerName'))}</td><td>${esc(val(r,'OrderNo','DoNo'))}</td><td>${esc(val(r,'ItemName'))}</td><td>${esc(val(r,'Qty'))}</td><td>${esc(val(r,'VehicleNo'))}</td><td>${esc(val(r,'BiltyNo'))}</td><td>${esc(val(r,'FactoryWeight'))}</td><td>${esc(val(r,'WbSlipNos','TicketNos'))}</td></tr>`).join(''):'<tr><td colspan="12" class="empty">No accepted pending gate passes</td></tr>')}
async function loadGp(i){
 if(s.busy)return;
 if(s.details.length)return msg('Reset the form before loading another GP');
 const r=s.initial.gatePasses[i],id=num(val(r,'OutwardGatePassId','Id'));
 if(String(val(r,'Status')).toLowerCase()==='open')return msg('Open gate pass cannot be loaded');
 busy(true);
 try{
  set('gatePass',id);$i('gpDate').val(iso(val(r,'GpDate')));await loadBilling(id);
  const b=s.billing;
  for(const [field,key]of [['vehicleNo','VehicleNo'],['biltyNo','BiltyNo'],['factoryWeight','FactoryWeight'],['freight','NetPaid'],['ticketNos','TicketNos'],['orderTerm','OrderStatus']])$i(field).val(val(b,key));
  $i('customerWeight').val(val(r,'SupplierWeight'));$i('container1').val(val(r,'Container'));$i('container2').val(val(r,'Container1'));
  const customers=await req(api+'/gate-pass/'+id+'/customers');
  if(customers.length){fill('customer',customers,['SupplierCustomerId'],['CompanyName']);set('customer',customers[0].SupplierCustomerId);}
  $i('pendingParty').val(customers.length);s.isStockReserved=b.IsStockReserved===true;
  reservationControls();await customerChanged();await loadOrder();$i('gatePass').prop('disabled',true);recalcHeader();
 }catch(x){msg(x.message)}finally{busy(false)}
}

function reset(){if(s.busy)return;s.shortWeightDeductionApply=false;s.isStockReserved=false;s.billing={};s.loadedGross=0;s.orderStatus='';billingControls();$i('customer').prop('disabled',false);$i('gatePass').prop('disabled',false);s.details=[];s.removed=[];s.expenses=[];s.orders=[];$i('recordId').val('');$i('btnSave').prop('hidden',false);$i('btnUpdate,#btnDelete').prop('hidden',true);$('input.ctl').not('[type=date]').val('');$('select.ctl').val('').trigger('change.select2');$i('saleType').val('1');cancel();renderDetails();renderExpenses();return initial()}
// InvFrmGDN.cs:3835 loads saved detail fields, rather than the pending DO aliases.
function mapStored(r){
 const x=Object.assign({},r,mapLine(r));
 x.Id=num(val(r,'Id'));x.ActionTypeId=2;x.JobLot=val(r,'JobLot','Job');x.AreaCity=val(r,'AreaCity','City');
 x.OrderId=num(val(r,'SaleOrderId','OrderId'));x.OrderNo=val(r,'SaleOrderNo','OrderNo','DoNo');
 x.CropYearId=num(val(r,'CropYearId'));x.UOMEquivalent=num(val(r,'UOMEquivalent','PUomEquivalent'));x.ItemQty=num(val(r,'ItemQty','Qty'));x.WareHouseCode=val(r,'WareHouseCode','WareHouseName','WarehouseName');
 x.BrandId=num(val(r,'BrandItemId','BrandId'));x.Brand=val(r,'BrandItemName','BrandName','Brand');
 x.WarehouseToId=num(val(r,'WareHouseToId','WarehouseToId'));x.WareHouseToId=x.WarehouseToId;x.WarehouseTo=val(r,'WareHouseToCode','WarehouseTo');
 // A zero stock reference is meaningful. Do not replace it with a delivery order ID.
 for(const key of ['RefDocumentTypeId','RefDocIdNo','RefDocSubIdNo','RefDocNo'])x[key]=num(val(r,key));
 return x;
}
function load(id){return req(api+'/'+id).then(async r=>{s.shortWeightDeductionApply=val(r,'ShortWeightDeductionApply')===true;s.isStockReserved=val(r,'IsStockReserved')===true;s.orderStatus=val(r,'TransporterDocRef');$i('gpDate').val(iso(val(r,'GPDate')));$i('customer').prop('disabled',true);$i('gatePass').prop('disabled',true);$i('recordId').val(val(r,'Id'));$i('docNo').val(val(r,'DocNo'));$i('docDate').val(iso(val(r,'DocDate')));set('customer',val(r,'SupplierCustomerId'));set('stockParty',val(r,'StockPartyId'));set('transporter',val(r,'TransporterId'));set('saleType',val(r,'SaleTypeId')||1);[['supplierRef','SupplierReference'],['referenceNo','ReferenceDocNo'],['freight','CarriageAmount'],['customerWeight','PartyWeight'],['factoryWeight','FactoryWeight'],['orderTerm','DeliveryTerm'],['vehicleNo','VehicleNo'],['biltyNo','BiltyNo'],['container1','ContainerNo'],['container2','ContainerNo1'],['ticketNos','TicketNos'],['remarks','RemarksHeader']].forEach(x=>$i(x[0]).val(val(r,x[1])));const gp=val(r,'OutwardGatePassId');if(gp){if(!$i('gatePass option[value="'+gp+'"]').length)$i('gatePass').append(new Option(val(r,'GpNo'),gp));set('gatePass',gp)}s.details=(r.details||[]).map(x=>{const row=mapStored(x);if(!row.CropYearId)row.CropYearId=num((s.initial.crops||[]).find(c=>String(c.CropYear)===String(row.CropYear))?.Id);return row});s.expenses=(r.expenses||[]).map(x=>({ItemId:num(val(x,'ItemId')),ItemName:val(x,'ItemName','Description'),Qty:num(val(x,'Qty')),Remarks:val(x,'Remarks')}));s.loadedGross=s.details.reduce((a,x)=>a+num(x.GrossWeight),0);await loadBilling(num(gp));await customerChanged();set('advanceDo',val(r,'AdvanceDeliveryOrderId'));renderDetails();renderExpenses();reservationControls();$i('btnSave').prop('hidden',true);$i('btnUpdate,#btnDelete').prop('hidden',false);show('formPane');msg('GDN '+val(r,'DocNo')+' loaded',true)})}
function payload(){return{DocNo:num($i('docNo').val()),DocDate:$i('docDate').val(),SupplierCustomerId:num($i('customer').val()),StockPartyId:num($i('stockParty').val()),SupplierReference:$i('supplierRef').val(),ReferenceDocNo:$i('referenceNo').val(),TransporterId:num($i('transporter').val()),TransporterSupCustId:num(val(lookup('transporter',s.initial.transporters,['Id','GlAccountId']),'SupplierCustomerId')),CarriageAmount:num($i('freight').val()),PartyWeight:num($i('customerWeight').val()),FactoryWeight:num($i('factoryWeight').val()),SaleTypeId:num($i('saleType').val()),DeliveryTerm:$i('orderTerm').val(),TransporterDocRef:s.orderStatus||s.billing.OrderStatus||'',OutwardGatePassId:num($i('gatePass').val()),GpNo:num(label('gatePass')),GPDate:$i('gpDate').val()||null,VehicleNo:$i('vehicleNo').val().toUpperCase(),BiltyNo:$i('biltyNo').val(),ContainerNo:$i('container1').val(),ContainerNo1:$i('container2').val(),TicketNos:$i('ticketNos').val(),RemarksHeader:$i('remarks').val(),ShortWeightDeductionApply:s.shortWeightDeductionApply,IsStockReserved:s.isStockReserved,AdvanceDeliveryOrderId:num($i('advanceDo').val()),details:s.details.concat(s.removed),expenses:s.expenses}}
function save(){if(s.busy)return;const id=num($i('recordId').val()),word=id?'Update':'Save';if(!confirm('Are you sure to '+word+'?'))return;req(api+(id?'/'+id:''),{method:id?'PUT':'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload())}).then(r=>{s.removed=[];msg('Record '+word+' Successfully: '+val(r,'DocNo'),true);return load(num(val(r,'Id')))}).catch(x=>msg(x.message))}
function del(){if(s.busy)return;const id=num($i('recordId').val());if(!id)return msg('Record not found');if(!confirm('Are you sure to Delete?'))return;req(api+'/'+id,{method:'DELETE'}).then(()=>reset()).then(()=>msg('Delete Record Successfully',true)).catch(x=>msg(x.message))}
function history(){return req(api+'/history').then(x=>{s.history=x;renderHistory(x)}).catch(x=>msg(x.message))}
const historyColumns=[
 ['DocDate','Doc Date','date'],['DocNo','Doc No'],['CustomerName','Customer Name'],
 ['GpNo','GP No'],['VehicleNo','Vehicle No'],['BiltyNo','Bilty No'],
 ['FactoryWeight','Factory Weight','weight'],['AccountTitle','Account Title'],
 ['Freight','Freight','amount'],['NetPaid','Net Paid','amount'],['RemarksHeader','Remarks'],
 ['StockStatus','Stock Status'],['UserName','Entry User'],['EntryDate','Entry Date','time'],
 ['ModifyUserName','Modify User'],['ModifyDate','Modify Date','time'],
 ['ApprovedUserName','Approved User'],['ApprovedDate','Approved Date','time'],['NoOfAttachments','Attachments']
];
function historyDate(value,time){
 if(!value)return '';
 const date=new Date(value);if(Number.isNaN(date.getTime()))return '';
 const day=String(date.getDate()).padStart(2,'0'),month=['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][date.getMonth()];
 return day+'-'+month+'-'+date.getFullYear()+(time?' '+String(date.getHours()%12||12).padStart(2,'0')+':'+String(date.getMinutes()).padStart(2,'0')+(date.getHours()<12?' am':' pm'):'');
}
function renderHistory(rows){
 const from=$i('historyFrom').val(),to=$i('historyTo').val(),fn=num($i('historyFromNo').val()),tn=num($i('historyToNo').val()),customer=num($i('historyCustomer').val());
 const dateColumn=$i('historyDateType').val()||'DocDate';
 rows=(rows||[]).filter(r=>(!from||iso(r[dateColumn])>=from)&&(!to||iso(r[dateColumn])<=to)&&(!fn||num(r.DocNo)>=fn)&&(!tn||num(r.DocNo)<=tn)&&(!customer||num(r.SupplierCustomerId)===customer));
 const cols=historyColumns.filter(c=>c[0]!=='StockStatus'||s.initial.stockReservationEnabled===true);
 $i('historyHead').html('<tr><th>Edit</th>'+cols.map(c=>'<th>'+esc(c[1])+'</th>').join('')+'</tr>');
 $i('historyRows').html(rows.length?rows.map(r=>'<tr data-id="'+num(r.Id)+'"><td><button class="tool history-edit">Edit</button></td>'+cols.map(([key,title,type])=>{
  let value=r[key]??'';
  if(type==='date'||type==='time')value=historyDate(value,type==='time');
  if(type==='weight'||type==='amount')value=num(value).toLocaleString('en-US',{maximumFractionDigits:type==='weight'?3:2});
  if(key==='DocNo')return '<td><button class="tool row-link history-edit">'+esc(value)+'</button></td>';
  if(key==='GpNo'&&num(r.GpId)>0)return '<td><a href="/sale/outward-gate-pass?id='+num(r.GpId)+'">'+esc(value)+'</a></td>';
  return '<td'+(type==='weight'||type==='amount'?' class="numeric"':'')+'>'+esc(value)+'</td>';
 }).join('')+'</tr>').join(''):'<tr><td colspan="'+(cols.length+1)+'" class="empty">No history records</td></tr>');
 const sum=k=>rows.reduce((total,r)=>total+num(r[k]),0).toLocaleString('en-US',{maximumFractionDigits:3});
 $i('historyTotals').text('Records: '+rows.length+' | Factory Weight: '+sum('FactoryWeight')+' | Freight: '+sum('Freight')+' | Net Paid: '+sum('NetPaid'));
}
function show(id){$('#formPane,#historyPane').prop('hidden',true);$i(id).prop('hidden',false);$('[data-main]').removeClass('active').filter('[data-main="'+id+'"]').addClass('active');if(id==='historyPane')history()}
$(function(){search();initial().then(async()=>{renderDetails();renderExpenses();const q=new URLSearchParams(location.search),id=Number(q.get('id')||q.get('record')||q.get('Id')||0);if(Number.isSafeInteger(id)&&id>0)await load(id)}).catch(x=>msg(x.message));$i('customer').on('change',()=>customerChanged().catch(x=>msg(x.message)));$i('lineItem').on('change',uoms);$i('shortDeduction').on('change',deductionChanged);$i('stockReserved').on('change',function(){s.isStockReserved=this.checked;reservationControls();customerChanged().catch(x=>msg(x.message))});$('#lineQty,#lineGross,#lineEbUnit,#lineEbTotal,#lineEbStockUnit,#lineEbStockTotal,#lineAddWeight,#lineWtCut').on('input',calc);$('#factoryWeight,#customerWeight,#addLess').on('input',recalcHeader);$i('vehicleNo').on('input',function(){this.value=this.value.toUpperCase()});$i('btnAddLine').on('click',add);$i('btnCancelLine').on('click',cancel);$i('btnAddExpense').on('click',addExpense);$i('btnNew,#btnRefresh,#btnHistoryNew').on('click',reset);$i('btnShowHistory,#btnHistoryRefresh').on('click',history);$i('btnSave,#btnUpdate').on('click',save);$i('btnDelete').on('click',del);$i('btnPrint').on('click',()=>printDocument());$i('btnGenerateStock').on('click',generateStock);$i('btnGdnHistory').on('click',()=>{const q=new URLSearchParams();[['fromDate','historyFrom'],['toDate','historyTo'],['fromNo','historyFromNo'],['toNo','historyToNo'],['customerId','historyCustomer']].forEach(([key,id])=>{if($i(id).val())q.set(key,$i(id).val())});location.href='/sale/reports/gdn-report?'+q});$i('btnStockReport').on('click',function(){window.open('/inventory/stock-report','_blank')});$i('btnAttachment').on('click',function(){msg('Attachment (AT.Show) is not ported for this screen yet.')});$i('btnShortcuts').on('click',()=>alert('Ctrl+N New | Ctrl+S Save | Ctrl+U Update | Ctrl+H History | Ctrl+P Print'));$('[data-sub]').on('click',function(){$('[data-sub]').removeClass('active');$(this).addClass('active');$('.pane').removeClass('active');$i($(this).data('sub')).addClass('active')});$('[data-main]').on('click',function(){show($(this).data('main'))});$i('detailRows').on('click','.edit',function(){edit(num($(this).data('i')))}).on('click','.del',function(){removeLine(num($(this).data('i')))}).on('click','.document-link',function(){window.DocLink.open(num($(this).data('type')),num($(this).data('ref')),{message:msg})});$i('expenseRows').on('click','.exp-del',function(){s.expenses.splice(num($(this).data('i')),1);renderExpenses()});$i('pendingRows').on('click','.load-gp',function(){loadGp(num($(this).data('i')))});$i('historyRows').on('dblclick','tr',function(){load(num($(this).data('id'))).catch(x=>msg(x.message))}).on('click','.history-edit',function(){load(num($(this).closest('tr').data('id'))).catch(x=>msg(x.message))});$(document).on('keydown',x=>{if(x.ctrlKey&&x.key.toLowerCase()==='n'){x.preventDefault();reset()}if(x.ctrlKey&&x.key.toLowerCase()==='s'){x.preventDefault();save()}if(x.ctrlKey&&x.key.toLowerCase()==='u'){x.preventDefault();save()}if(x.ctrlKey&&x.key.toLowerCase()==='h'){x.preventDefault();show('historyPane')}if(x.ctrlKey&&x.key.toLowerCase()==='p'){x.preventDefault();printDocument()}})})
})(jQuery);
