'use strict';
// The document-aging grid consumes procedure amounts and captions without recomputing aging.
(()=>{
const el=id=>document.getElementById(id),kind=document.body.dataset.agingKind;
let rows=[],lookup={},sequence=0,request,selectedRow=null;
const numbers=new Set(['1stInterval','2ndInterval','3rdInterval','Above','DueAmount']);
const columns=[['DocumentType','Document Type',70],['InvoiceNo','Doc No',60],['ManualNo','Manual No',100],['InvoiceDate','Invoice Date',85],['PartyBillDate','Supplier Bill Date',85],['InvoiceDueDate','Due Date',85],['AccountCode','Account Code',100],['1stInterval','',100],['2ndInterval','',100],['3rdInterval','',100],['Above','',100],['DueAmount','Due Amount',100],['Remarks','Remarks',350]];
if(kind==='customer')columns.splice(2,3);
const intervalStart=kind==='customer'?4:7;
const status=t=>el('status').textContent=t;
async function api(path,options){const response=await fetch('/api/accounts/document-aging/'+kind+path,options);if(!response.ok)throw Error('Unable to load report ('+response.status+'). The database error is recorded in the server log.');return response.json()}
function bind(id,data,label){const select=el(id),old=select.value;select.replaceChildren(new Option('',''));data.forEach(r=>select.add(new Option(r[label]??'',r.Id)));$('#'+id).val(old).trigger('change.select2')}
async function refresh(){if(el('refresh').disabled)return;el('refresh').disabled=true;try{lookup=await api('/lookups');bind('account',lookup.accounts,'AccountTitle');bind('custom',lookup.customGroups,'AcLookUpsDescription');bind('party',lookup.partyGroups,'Description');status('')}catch(e){status(e.message)}finally{el('refresh').disabled=false}}
function ledger(r){return '/accounts/reports/general-ledger?'+new URLSearchParams({accountId:r.AccountId,fromDate:String(lookup.year.Start_Period).slice(0,10),toDate:el('asOnDate').value})}
function render(){const captions=rows[0]||{},keys=['FirstIntervalCaption','SecondIntervalCaption','ThirdIntervalCaption','AboveIntervalCaption'];columns.slice(intervalStart,intervalStart+4).forEach((column,i)=>column[1]=captions[keys[i]]||'');selectedRow=null;DocumentAgingGrid.render({table:el('results'),rows,columns,numbers,decimals:lookup.amountDecimals||0,search:el('search').value,ledger,select:r=>selectedRow=r,status})}
async function show(){if(el('show').disabled)return;el('show').disabled=true;el('show').setAttribute('aria-busy','true');const token=++sequence;request?.abort();request=new AbortController();rows=[];render();status('Loading…');const query=new URLSearchParams({asOnDate:el('asOnDate').value,agingDays:el('days').value||0,accountId:el('account').value||0,customGroupId:el('custom').value||0,partyGroupId:el('party').value||0});try{const data=await api('?'+query,{signal:request.signal});if(token!==sequence)return;rows=data;render()}catch(e){if(e.name!=='AbortError')status(e.message)}finally{if(token===sequence){el('show').disabled=false;el('show').removeAttribute('aria-busy')}}}
el('show').onclick=show;el('refresh').onclick=refresh;el('reset').onclick=()=>{++sequence;request?.abort();el('show').disabled=false;el('show').removeAttribute('aria-busy');rows=[];el('days').value='';render();el('asOnDate').focus()};el('search').oninput=render;
el('print').onclick=()=>status(rows.length?'Crystal print layout verification is pending.':'Record Not Found For Display');
el('shortcuts').onclick=()=>{el('shortcutDialog').showModal()};el('closeShortcuts').onclick=()=>el('shortcutDialog').close();
document.addEventListener('keydown',e=>{if(e.ctrlKey){const key=e.key.toLowerCase(),actions={s:show,n:()=>el('reset').click(),r:refresh,p:()=>el('print').click(),f5:()=>el('asOnDate').focus(),arrowup:()=>el('asOnDate').focus(),arrowdown:()=>el('grid').focus()};if(actions[key]){e.preventDefault();actions[key]()}}});
const today=new Date();el('asOnDate').value=[today.getFullYear(),String(today.getMonth()+1).padStart(2,'0'),String(today.getDate()).padStart(2,'0')].join('-');$('select').select2({allowClear:true,placeholder:''});refresh();
})();
