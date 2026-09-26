(()=>{'use strict';const $=x=>document.getElementById(x),S={id:0,data:{},saleOrderId:0};const v=(r,...k)=>{for(const x of k)if(r?.[x]!=null)return r[x];return''},esc=x=>String(x??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
let pending=0;
const disabledBefore=new Map(), reads=new Map();
function syncBusyButtons(){
 if(!pending)return;
 document.querySelectorAll('button').forEach(b=>{if(!disabledBefore.has(b))disabledBefore.set(b,{disabled:b.disabled,busy:b.getAttribute('aria-busy')});b.disabled=true;b.setAttribute('aria-busy','true')});
}
function busy(start){
 pending=Math.max(0,pending+(start?1:-1));
 $('loader').hidden=!pending;
 if(pending)syncBusyButtons();
 else{for(const[b,state]of disabledBefore){b.disabled=state.disabled;if(state.busy===null)b.removeAttribute('aria-busy');else b.setAttribute('aria-busy',state.busy)}disabledBefore.clear()}
}
function msg(x,ok=false){$('message').textContent=x||'';$('message').style.color=ok?'#067000':'#a00'}
async function action(work){
 if(pending)return;
 busy(true);msg('');
 try{return await work()}finally{busy(false)}
}
function api(url,opt){
 const key=(!opt?.method||opt.method==='GET')?url:null;
 if(key&&reads.has(key))return reads.get(key);
 busy(true);
 const request=(async()=>{try{
  const r=await fetch(url,{headers:{'Content-Type':'application/json','Accept':'application/json'},...opt}),t=await r.text();
  if(r.redirected&&/\/login(?:[?#]|$)/.test(r.url))throw Error('Your session has expired. Please sign in again.');
  let b={};try{b=t?JSON.parse(t):{}}catch{if(r.ok)throw Error('The server did not return valid data. Please refresh or sign in again.');b={message:t}}
  if(!r.ok)throw Error(b.message||b.detail||`Request failed (${r.status})`);
  return b;
 }finally{if(key)reads.delete(key);busy(false)}})();
 if(key)reads.set(key,request);
 return request;
}
// OutwardGatePass.cs:2814-2861 maps these procedure columns into a separate history table.
// Do not reuse the open-grid renderer: it has different names and a different column order.
const historyColumns=[
 ['GatepassType','Gatepass Type',80],['OrderType','Order Type',100],['GpSrNo','Gp No',60,'code'],
 ['GpDate','Gp Date',83,'date'],['DeliveryOrderNo','Delivery Order No',60],['OrderNo','Order No',60],
 ['CompanyName','Customer Name',190],['VarietyName','Item Name',150],['ItemQty','Item Qty',70,'number'],
 ['VehicleType','Vehicle Type',80],['VehicleNo','Vehicle No',60],['BiltyNo','Bilty No',60],['BiltyDate','Bilty Date',73,'dateNumeric'],
 ['InTime','In Time',145,'time'],['OutTime','Out Time',145,'time'],['Description','City Name',90],
 ['Freight','Freight',90,'amount'],['NetPaid','Net Paid',90,'amount'],['Status','Status',75],
 ['SupplierWeight','Supplier Weight',80,'number'],['FactoryWeight','Factory Weight',80,'number'],['DifferenceWeight','Difference Weight',80,'number'],
 ['WeighBridgeStatus','WeighBridge Status',75],['Container','Container 1',100],['Container1','Container 2',100],
 ['UserName','Entry User',100],['EntryDate','Entry Date',145,'time'],['ModifyUserName','Modify User',100],['ModifyDate','Modify Date',145,'time'],
 ['OtherRemarks','Other Remarks',100],['NoOfAttachments','Attachments',90]
];
function dateText(value,kind){
 const m=String(value??'').match(/^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/);
 if(!m)return value??'';
 const month=kind==='date'?['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][+m[2]-1]:m[2];
 const date=`${m[3]}-${month}-${m[1]}`;
 return kind==='time'&&m[4]?`${date} ${String(+m[4]%12||12).padStart(2,'0')}:${m[5]} ${+m[4]>=12?'PM':'AM'}`:date;
}
function numberText(value,kind){
 if(value==null||value==='')return '';
 return Number(value).toLocaleString('en-US',{maximumFractionDigits:kind==='amount'?(S.data.amountDecimals??0):3});
}
function historyRow(r){
 return `<tr><td><button data-id="${esc(r.Id)}">Edit</button></td>`+historyColumns.map(([key,label,width,kind])=>{
  let value=r[key];
  if(kind==='code')return `<td><a href="/sale/outward-gate-pass?record=${esc(r.Id)}" data-id="${esc(r.Id)}">${esc(value)}</a></td>`;
  if(['date','dateNumeric','time'].includes(kind))value=dateText(value,kind);
  if(['number','amount'].includes(kind))value=numberText(value,kind);
  return `<td${['number','amount'].includes(kind)?' class="number"':''}>${esc(value)}</td>`;
 }).join('')+'</tr>';
}
function renderHistory(rows){
 const table=$('historyTable');
 table.querySelector('colgroup').innerHTML='<col style="width:50px">'+historyColumns.map(c=>`<col style="width:${c[2]}px">`).join('');
 table.tHead.innerHTML='<tr><th>Edit</th>'+historyColumns.map(c=>`<th scope="col">${c[1]}</th>`).join('')+'</tr>';
 table.style.minWidth=(50+historyColumns.reduce((n,c)=>n+c[2],0))+'px';
 table.tBodies[0].innerHTML=rows.length?rows.map(historyRow).join(''):`<tr><td colspan="${historyColumns.length+1}">No history found.</td></tr>`;
 table.tFoot.innerHTML='<tr><th>Total</th>'+historyColumns.map(([key,label,width,kind])=>`<td class="number">${['number','amount'].includes(kind)?numberText(rows.reduce((n,r)=>n+(Number(r[key])||0),0),kind):''}</td>`).join('')+'</tr>';
 $('historyCount').textContent=`Total Records: ${rows.length}`;
 wireLoads(table);syncBusyButtons();
}
const pair=(id,name)=>`${id} | ${name}`,pid=x=>+(String(x||'').split('|')[0].trim())||0,pname=x=>String(x||'').includes('|')?String(x).split('|').slice(1).join('|').trim():String(x||'');
function setChoice(id,value,name){
 const select=$(id),key=pair(value,name);
 if(!Array.from(select.options).some(o=>o.value===key))select.insertAdjacentHTML('beforeend',`<option value="${esc(key)}">${esc(name||value)}</option>`);
 select.value=key;
}
function list(id,rows,idk,nk){
 const select=$(id),previous=select.value,label=select.selectedOptions[0]?.textContent||'';
 select.innerHTML='<option value=""></option>'+(rows||[]).map(r=>`<option value="${esc(pair(v(r,idk),v(r,nk)))}">${esc(v(r,nk))}</option>`).join('');
 if(previous){if(!Array.from(select.options).some(o=>o.value===previous))select.insertAdjacentHTML('beforeend',`<option value="${esc(previous)}">${esc(label)}</option>`);select.value=previous}
}
function exportFields(){
 const show=['Export','Export Return','Import'].includes(pname($('gatepassType').value));
 for(const id of ['container','container1','sealNo','sealNo1'])$(id).closest('label').hidden=!show;
}
async function gateTypeLeave(){
 const selected=$('gatepassType').value;
 exportFields();
 if(!selected){$('gpTypeSrNo').value='';return}
 const result=await api(`/sale/outward-gate-pass/api/type-code?type=${encodeURIComponent(pname(selected))}`);
 if($('gatepassType').value===selected)$('gpTypeSrNo').value=result.code;
}
function init(d){S.data=d;if(!S.id)$('gpSrNo').value=d.nextNo;list('gatepassType',d.gatePassTypes,'Id','GpTypeDescription');list('basedOn',d.orderTypes,'Id','OrderType');list('vehicleType',d.vehicleTypes,'Id','VehicleDescription');list('city',d.cities,'Id','CityName');list('warehouse',d.warehouses,'Id','WareHouseName');list('customer',d.customers,'Id','CompanyName');list('item',d.items,'Id','ItemName');openRows(d.openRecords||[])}
function localDay(date=new Date()){return new Date(date.getTime()-date.getTimezoneOffset()*60000).toISOString().slice(0,10)}
function reset(){S.id=0;S.saleOrderId=0;document.querySelectorAll('.grid input:not(.dtcombo-input),.grid select').forEach(i=>i.value='');$('supplierContractCode').disabled=false;exportFields();$('gpSrNo').value=S.data.nextNo||'';$('gpDate').value=localDay();$('biltyDate').value=$('gpDate').value;const n=new Date();n.setMinutes(n.getMinutes()-n.getTimezoneOffset());$('inDateTimeStamp').value=$('outDateTimeStamp').value=n.toISOString().slice(0,16);$('status').value='Open';$('docAttachment').value='Auto';$('saveBtn').hidden=false;$('updateBtn').hidden=true;msg('')}
function rowHtml(r){return `<tr><td><button data-id="${esc(v(r,'Id'))}">Edit</button></td><td><a href="/sale/outward-gate-pass?record=${esc(v(r,'Id'))}">GP-${esc(v(r,'GpSrNo','GpNo'))}</a></td><td>${esc(String(v(r,'GpDate')).slice(0,10))}</td><td>${esc(v(r,'GatepassType'))}</td><td>${esc(v(r,'OrderType','OtherSupCust'))}</td><td>${esc(v(r,'OrderNo','SupplierContractCode'))}</td><td>${esc(v(r,'CustomerName'))}</td><td>${esc(v(r,'ItemName','VarietyName'))}</td><td>${esc(v(r,'WareHouseName'))}</td><td>${esc(v(r,'NoOfBags','NoOfPackages'))}</td><td>${esc(v(r,'VehicleType'))}</td><td>${esc(v(r,'VehicleNo'))}</td><td>${esc(v(r,'BiltyNo'))}</td><td>${esc(v(r,'Status'))}</td><td>${esc(v(r,'CityName'))}</td><td>${esc(v(r,'FactoryWeight'))}</td><td>${esc(v(r,'OtherRemarks','UserName','NoOfAttachments'))}</td></tr>`}
function wireLoads(table){table.querySelectorAll('[data-id]').forEach(b=>b.onclick=e=>{e.preventDefault();action(()=>load(+b.dataset.id)).catch(e=>msg(e.message))});syncBusyButtons()}function openRows(rows){$('openTable').tBodies[0].innerHTML=rows.length?rows.map(rowHtml).join(''):'<tr><td colspan="17">No open gate pass found.</td></tr>';wireLoads($('openTable'))}
async function history(){
 const params=new URLSearchParams({dateType:$('historyDateType').value});
 for(const id of ['fromNo','toNo','fromDate','toDate'])if($(id).value)params.set(id,$(id).value);
 renderHistory(await api('/sale/outward-gate-pass/api/history?'+params));
}
async function reference(){const type=pid($('basedOn').value),code=+$('supplierContractCode').value||0;if(type===92||type===59){S.saleOrderId=0;return}if(!type||!code)return;const d=await api(`/sale/outward-gate-pass/api/reference?type=${type}&code=${code}&date=${$('gpDate').value}`),r=d.rows?.[0]||{};S.saleOrderId=+d.saleOrderId||0;if(v(r,'Id'))setChoice('customer',v(r,'Id'),v(r,'CompanyName','CustomerName'));if(v(r,'ItemId'))setChoice('item',v(r,'ItemId'),v(r,'ItemName'));if(v(r,'WarehouseId')){const w=(S.data.warehouses||[]).find(x=>+v(x,'Id')===+v(r,'WarehouseId'));setChoice('warehouse',v(r,'WarehouseId'),v(w,'WareHouseName'))}$('supplierWeight').value=v(r,'TotalWeight');$('noOfPackages').value=v(r,'DoTotalQty','BalQty');$('vehicleNo').value=v(r,'VehicleNo');if(v(r,'VehicleType')){const vehicle=(S.data.vehicleTypes||[]).find(x=>v(x,'VehicleDescription')===v(r,'VehicleType'));setChoice('vehicleType',v(vehicle,'Id'),v(r,'VehicleType'))}}
function body(){const d={id:S.id,gpSrNo:+$('gpSrNo').value,gpTypeSrNo:+$('gpTypeSrNo').value,gpDate:$('gpDate').value,gatepassType:pname($('gatepassType').value),refDocumentTypeId:pid($('basedOn').value),otherSupCust:pname($('basedOn').value),supplierContractCode:$('supplierContractCode').value,saleOrderId:S.saleOrderId,supplierCustomerId:pid($('customer').value),itemId:pid($('item').value),varietyName:pname($('item').value),warehouseId:pid($('warehouse').value),noOfPackages:+$('noOfPackages').value||0,packUnit:+$('packUnit').value||0,weightCommapredToSoWt:+$('weightCommapredToSoWt').value||0,vehicleType:pname($('vehicleType').value),vehicleNo:$('vehicleNo').value.toUpperCase(),biltyNo:$('biltyNo').value,biltyDate:$('biltyDate').value,cityId:pid($('city').value),status:$('status').value,docAttachment:$('docAttachment').value,weighBridgeId:+$('weighBridgeId').value||0,supplierWeight:+$('supplierWeight').value||0,factoryWeight:+$('factoryWeight').value||0,differenceWeight:+$('differenceWeight').value||0,weightDiffRemarks:$('weightDiffRemarks').value,freight:+$('freight').value||0,netPaid:+$('netPaid').value||0,inDateTimeStamp:$('inDateTimeStamp').value,outDateTimeStamp:$('outDateTimeStamp').value,container:$('container').value,container1:$('container1').value,sealNo:$('sealNo').value,sealNo1:$('sealNo1').value,otherRemarks:$('otherRemarks').value};return d}
async function save(){return action(async()=>{
 const was=S.id>0,r=await api('/sale/outward-gate-pass/api',{method:'POST',body:JSON.stringify(body())});
 S.id=+v(r,'Id');$('saveBtn').hidden=true;$('updateBtn').hidden=false;
 // Refresh lookup/open rows without replacing the saved document number with nextNo.
 init(await api('/sale/outward-gate-pass/api/initial'));await history();
 msg(`Record ${was?'updated':'saved'} successfully. GP-${v(r,'GpSrNo')}`,true);
})}
async function load(id){const r=await api(`/sale/outward-gate-pass/api/${id}`);S.id=id;S.saleOrderId=+v(r,'SaleOrderId')||0;const map={gpSrNo:'GpSrNo',gpTypeSrNo:'GpTypeSrNo',gpDate:'GpDate',supplierContractCode:'SupplierContractCode',noOfPackages:'NoOfPackages',packUnit:'PackUnit',weightCommapredToSoWt:'WeightCommapredToSoWt',vehicleNo:'VehicleNo',biltyNo:'BiltyNo',biltyDate:'BiltyDate',status:'Status',docAttachment:'DocAttachment',weighBridgeId:'WeighBridgeId',supplierWeight:'SupplierWeight',factoryWeight:'FactoryWeight',differenceWeight:'DifferenceWeight',weightDiffRemarks:'WeightDiffRemarks',freight:'Freight',netPaid:'NetPaid',inDateTimeStamp:'InDateTimeStamp',outDateTimeStamp:'OutDateTimeStamp',container:'Container',container1:'Container1',sealNo:'SealNo',sealNo1:'SealNo1',otherRemarks:'OtherRemarks'};for(const[id,k]of Object.entries(map)){let x=v(r,k);if($(id).type==='date')x=String(x).slice(0,10);if($(id).type==='datetime-local')x=String(x).slice(0,16);$(id).value=x}for(const[id,val,name,rows,key]of[['gatepassType',v(r,'GatepassType'),v(r,'GatepassType'),S.data.gatePassTypes,'GpTypeDescription'],['basedOn',v(r,'RefDocumentTypeId'),v(r,'OtherSupCust'),S.data.orderTypes,'OrderType'],['vehicleType','',v(r,'VehicleType'),S.data.vehicleTypes,'VehicleDescription'],['customer',v(r,'SupplierCustomerId'),v(r,'CustomerName'),S.data.customers,'CompanyName'],['item',v(r,'ItemId'),v(r,'VarietyName'),S.data.items,'ItemName'],['warehouse',v(r,'WarehouseId'),v(r,'WareHouseName'),S.data.warehouses,'WareHouseName'],['city',v(r,'CityId'),v(r,'CityName'),S.data.cities,'CityName']]){const found=(rows||[]).find(x=>(val&&+v(x,'Id')===+val)||v(x,key)===name);if(found)setChoice(id,v(found,'Id'),v(found,key));else if(val||name)setChoice(id,val,name);else $(id).value=''}$('saveBtn').hidden=true;$('updateBtn').hidden=false;$('supplierContractCode').disabled=[92,59].includes(pid($('basedOn').value));exportFields();scrollTo(0,0)}
async function reload(){const d=await api('/sale/outward-gate-pass/api/initial');init(d);await history();reset()}
function wire(){$('newBtn').onclick=()=>{if(!pending)reset()};$('refreshBtn').onclick=()=>action(reload).catch(e=>msg(e.message));$('saveBtn').onclick=$('updateBtn').onclick=()=>save().catch(e=>msg(e.message));$('showBtn').onclick=()=>action(history).catch(e=>msg(e.message));$('gatepassType').onchange=()=>gateTypeLeave().catch(e=>msg(e.message));$('gatepassType').onblur=()=>gateTypeLeave().catch(e=>msg(e.message));document.addEventListener('focusout',e=>{if(e.target.matches('.dtcombo-input')&&e.target.closest('.dtcombo-wrap')?.parentElement?.querySelector('#gatepassType'))gateTypeLeave().catch(x=>msg(x.message))});$('basedOn').onchange=()=>{S.saleOrderId=0;const general=[92,59].includes(pid($('basedOn').value));$('supplierContractCode').disabled=general;if(general)$('supplierContractCode').value=$('gpSrNo').value};$('supplierContractCode').onblur=()=>reference().catch(e=>msg(e.message));$('vehicleNo').oninput=e=>e.target.value=e.target.value.toUpperCase();$('supplierWeight').oninput=$('factoryWeight').oninput=()=>{$('differenceWeight').value=(+$('supplierWeight').value||0)-(+$('factoryWeight').value||0)};$('printBtn').onclick=()=>print();$('attachmentsBtn').onclick=()=>msg('Save or load the gate pass before managing attachments.');$('shortcutsBtn').onclick=()=>alert('Ctrl+N New | Ctrl+S Save | Ctrl+U Update | F5 Refresh');addEventListener('keydown',e=>{if(pending&&((e.ctrlKey&&['n','s','u'].includes(e.key.toLowerCase()))||e.key==='F5')){e.preventDefault();return}if(e.ctrlKey&&e.key.toLowerCase()==='n'){e.preventDefault();reset()}if(e.ctrlKey&&['s','u'].includes(e.key.toLowerCase())){e.preventDefault();save().catch(x=>msg(x.message))}if(e.key==='F5'){e.preventDefault();action(reload).catch(x=>msg(x.message))}})}
document.addEventListener('DOMContentLoaded',async()=>{wire();const n=new Date(),p=new Date(n);p.setDate(n.getDate()-3);$('fromDate').value=localDay(p);$('toDate').value=localDay(n);try{await action(async()=>{await reload();const q=new URLSearchParams(location.search),id=Number(q.get('id')||q.get('record')||q.get('Id')||0);if(Number.isSafeInteger(id)&&id>0)await load(id)})}catch(e){msg(e.message)}})})();
