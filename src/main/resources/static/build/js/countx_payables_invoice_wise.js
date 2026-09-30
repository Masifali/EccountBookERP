(function () {
'use strict';
const byId = id => document.getElementById(id);
const columns = ['DocumentTypeDescription','DocDate','DocNo','ManualBillNo','SupplierName','CommissionAgent','VehicleNos','ItemName','ItemQty','BillWeight','StockWeight','AvgRate','BillAmount','PaidAmount','UnPaidAmount','DueDays','DueDate','CurrentDate','DaysUptoNow','OverDueBy'];
let rows = [], busy = false;
const value = (row,key) => row[Object.keys(row).find(k=>k.toLowerCase()===key.toLowerCase())] ?? '';
async function request(url) {
 const r=await fetch(url,{headers:{Accept:'application/json'}});
 if(!r.ok || !r.headers.get('content-type')?.includes('application/json')) throw new Error(r.status===401 || r.redirected?'Please sign in again.':'Unable to load report data. Please retry.');
 return r.json();
}
async function run(message,work) {
 if(busy)return; busy=true;
 const controls=Array.from(document.querySelectorAll('button,input,select'));
 const states=controls.map(el=>el.disabled);controls.forEach(el=>el.disabled=true);
 byId('status').textContent=message;byId('status').setAttribute('aria-busy','true');
 try {await work();} catch(e) {byId('status').textContent=e.message;}
 finally {controls.forEach((el,i)=>el.disabled=states[i]);busy=false;byId('status').setAttribute('aria-busy','false');}
}
function bind(id,data,key,label) {
 const select=byId(id); select.replaceChildren(new Option('...Select Any Value...','0'));
 data.forEach(row=>select.add(new Option(value(row,label),value(row,key))));
 $(select).trigger('change');
}
async function lookups() {
 const d=await request('/api/accounts/payables-invoice-wise/lookups');
 bind('partyId',d.parties.filter(r=>value(r,'Activity')==='Supplier'),'Id','ReferenceName');
 bind('agentId',d.parties.filter(r=>value(r,'Activity')==='CommissionAgent'),'Id','ReferenceName');
 bind('parentId',d.parents,'Id','AccountTitle');bind('groupId',d.groups,'Id','AcLookUpsDescription');
 byId('status').textContent='Filters loaded. Press Show.';
}
function render() {
 const captions={DocumentTypeDescription:'Document Type',SupplierName:'Party Name',UnPaidAmount:'Balance'};
 const head=document.createElement('tr');columns.forEach(c=>{const th=document.createElement('th');th.textContent=captions[c]||c.replace(/([a-z])([A-Z])/g,'$1 $2');head.append(th);});byId('head').replaceChildren(head);
 byId('rows').replaceChildren();rows.forEach(row=>{const tr=document.createElement('tr');columns.forEach(c=>{
  const td=document.createElement('td');td.textContent=value(row,c);
  if(c==='SupplierName' && Number(value(row,'SupplierGlAccountId'))>0){
   const params=new URLSearchParams({accountId:value(row,'SupplierGlAccountId')});
   if(byId('useFrom').checked)params.set('fromDate',byId('fromDate').value);
   if(byId('useTo').checked)params.set('toDate',byId('toDate').value);
   const link=document.createElement('a');link.href='/accounts/reports/general-ledger?'+params;link.textContent=td.textContent;td.replaceChildren(link);
  }
  tr.append(td);
 });byId('rows').append(tr);});
}
function initialize(){const now=new Date();const today=[now.getFullYear(),String(now.getMonth()+1).padStart(2,'0'),String(now.getDate()).padStart(2,'0')].join('-');['fromDate','toDate'].forEach(id=>byId(id).value=today);['useFrom','useTo'].forEach(id=>byId(id).checked=true);$('select').val('0').trigger('change');$('#actionId').val('2').trigger('change');rows=[];render();byId('status').textContent='Select filters and press Show.';}
byId('show').onclick=()=>run('Loading report…',async()=>{const p=new URLSearchParams();['partyId','agentId','parentId','groupId','actionId'].forEach(id=>p.set(id,byId(id).value||'0'));if(byId('useFrom').checked)p.set('fromDate',byId('fromDate').value);if(byId('useTo').checked)p.set('toDate',byId('toDate').value);const d=await request('/api/accounts/payables-invoice-wise?'+p);rows=d.data;render();byId('status').textContent=rows.length+' records';});
byId('refresh').onclick=()=>run('Loading filters…',lookups);byId('reset').onclick=()=>{if(busy)return;rows=[];render();byId('fromDate').focus();byId('status').textContent='Select filters and press Show.';};
byId('export').onclick=()=>run('Exporting…',async()=>{const cell=v=>'"'+String(v).replace(/^[=+@-]/,"'$&").replace(/"/g,'""')+'"';const csv=[columns,...rows.map(r=>columns.map(c=>value(r,c)))].map(r=>r.map(cell).join(',')).join('\r\n');const url=URL.createObjectURL(new Blob(['\ufeff'+csv],{type:'text/csv;charset=utf-8'}));const a=document.createElement('a');a.href=url;a.download='payables-invoice-wise.csv';a.click();setTimeout(()=>URL.revokeObjectURL(url),1000);byId('status').textContent=rows.length+' records exported';});
$('select').select2({width:'230px'});initialize();run('Loading filters…',lookups);
document.addEventListener('keydown',event=>{
 if(busy)return;
 if(event.ctrlKey){
  const action={s:'show',n:'reset',r:'refresh'}[event.key.toLowerCase()];
  if(action){event.preventDefault();byId(action).click();}
  if(event.key==='ArrowUp'||event.key==='F5'){event.preventDefault();byId('fromDate').focus();}
 }
});
})();

