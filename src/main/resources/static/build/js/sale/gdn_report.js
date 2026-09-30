(function(){'use strict';
const $=id=>document.getElementById(id),api='/sale/reports/gdn-report/api';
const state={busy:false,rows:[],branches:[],yearStart:'',sort:null};
// frmGDNHistory.grdSetting and Constants.InventoryConstants, in desktop display order.
const columns=[['BranchName','Branch Name',130],['DocDate','Doc Date',73,'date'],['DocNo','Doc No',60],
 ['PartyName','Party Name',130],['PartyReference','Party Reference',75],['GpNo','GP No',50],['GPDate','GP Date',73,'date'],
 ['VehicleNo','Vehicle No',80],['BiltyNo','Bilty No',60],['ReferenceDocNo','Ref Doc No',60],['OrderNo','Order No',60],['OrderDate','Order Date',73,'date'],
 ['ItemCode','Item Code',75],['ItemName','Item Name',150],['PackingType','Packing Type',115],['CropYear','Crop Year',73],['JobLot','JobLot',80],
 ['ItemQty','Item Qty',70,'sum'],['PackUom','Pack Uom',60],['GrossWeight','Gross Weight',80,'sum'],['EBWPerUnit','EBW Per Unit',60,'number'],
 ['EBWTotal','EBW Total',60,'number'],['WtCutTotal','Wt Cut Total',70,'number'],['AdLsWeight','AdLs Weight',80,'sum'],
 ['StockWeight','Stock Weight',80,'sum'],['NetBillWeight','Net Bill Weight',80,'sum'],['WareHouseName','WareHouse Name',150],
 ['LabReportReference','Lab Report Reference',75],['FreightAmount','Freight Amount',90,'sum'],['CityName','City Name',75],['EntryUser','Entry User',80],['RemarksDetail','Remarks Detail',100]];
const escape=value=>String(value??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const number=value=>Number(value||0),ymd=d=>[d.getFullYear(),String(d.getMonth()+1).padStart(2,'0'),String(d.getDate()).padStart(2,'0')].join('-');
function day(value){if(!value)return '';const text=String(value);return /T.*(?:Z|[+-]\d{2}:?\d{2})$/i.test(text)?ymd(new Date(text)):text.slice(0,10)}
function display(value,type){if(value===null||value===undefined)return '';if(type==='date'){const d=day(value).split('-');return d.length===3?d[2]+'-'+['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][Number(d[1])-1]+'-'+d[0].slice(-2):''}return type==='number'||type==='sum'?number(value).toLocaleString('en-US',{maximumFractionDigits:2}):String(value)}
function message(text){$('message').textContent=text||''}
async function request(url,options){
 const response=await fetch(url,options);if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw Error('Your session has expired. Please sign in again.');
 const raw=await response.text();let data;try{data=JSON.parse(raw)}catch{throw Error('The server could not load the requested data. Please refresh or sign in again.')}
 if(!response.ok)throw Error(data.message||data.error||'Request failed');return data;
}
async function run(action){
 if(state.busy)return;state.busy=true;message('');$('loading').hidden=false;
 const controls=[...document.querySelectorAll('button,input,select')].map(el=>[el,el.disabled]);controls.forEach(([el])=>{el.disabled=true;el.setAttribute('aria-busy','true')});
 try{return await action()}catch(error){message(error.message)}finally{controls.forEach(([el,disabled])=>{el.disabled=disabled;el.removeAttribute('aria-busy')});$('loading').hidden=true;state.busy=false}
}
function branchIds(){return [...document.querySelectorAll('#branchOptions input:checked')].map(el=>Number(el.value))}
function branchCaption(){const ids=branchIds();$('branchCaption').textContent=state.branches.filter(row=>ids.includes(number(row.BranchId))).map(row=>row.BranchName).join(', ')||'Select Branch';$('allBranches').checked=ids.length>0&&ids.length===state.branches.length}
function bindLookups(rows){document.querySelectorAll('[data-activity]').forEach(select=>{select.innerHTML='<option value="">...Select Any Value...</option>'+rows.filter(row=>row.Activity===select.dataset.activity).map(row=>'<option value="'+escape(row.Id)+'">'+escape(row.ReferenceName)+'</option>').join('')})}
async function refreshLookups(){const ids=branchIds();if(!ids.length){bindLookups([]);throw Error('Select Branch First')}bindLookups(await request(api+'/lookups?branchIds='+ids.join(',')))}
function datePreset(){const today=new Date(),from=new Date(today);switch(number($('dateType').value)){case 1:$('fromDate').value=ymd(today);break;case 2:from.setDate(today.getDate()-7);$('fromDate').value=ymd(from);break;case 3:from.setDate(1);$('fromDate').value=ymd(from);$('toDate').value=ymd(today);break;case 4:from.setMonth(0,1);$('fromDate').value=ymd(from);$('toDate').value=ymd(today);break;case 5:$('fromDate').value=state.yearStart;break}}
function filter(){const out={fromDate:$('fromDate').value,toDate:$('toDate').value,branchIds:branchIds()};for(const id of ['fromNo','toNo','orderNoFrom','orderNoTo','customerId','parentCategoryId','categoryId','itemTypeId','itemClassId','itemId','cropYearId','jobLotId','warehouseId'])out[id]=number($(id).value);return out}
async function show(){const body=filter();if(!body.branchIds.length)throw Error('Select Branch First');if(!body.fromDate||!body.toDate)throw Error('From Date and To Date are required');state.rows=await request(api+'/rows',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(body)});render()}
function render(){
 const filters=[...document.querySelectorAll('[data-column-filter]')].map(input=>[input.dataset.columnFilter,input.value.toLowerCase()]);
 const rows=state.rows.filter(row=>filters.every(([key,term])=>!term||display(row[key],columns.find(c=>c[0]===key)[3]).toLowerCase().includes(term)));
 if(state.sort){const [key,dir]=state.sort;rows.sort((a,b)=>typeof a[key]==='number'?dir*(a[key]-b[key]):dir*String(a[key]??'').localeCompare(String(b[key]??'')))}
 $('rows').innerHTML=rows.length?rows.map(row=>'<tr><td><button class="print-row" data-id="'+number(row.Id)+'">Print</button></td>'+columns.map(([key,,width,type])=>'<td'+(type==='sum'||type==='number'?' class="number"':'')+' title="'+escape(display(row[key],type))+'">'+(key==='DocNo'?'<a href="/sale/gdn?id='+number(row.Id)+'">'+escape(row.DocNo)+'</a>':escape(display(row[key],type)))+'</td>').join('')+'</tr>').join(''):'<tr><td colspan="'+(columns.length+1)+'" class="empty">No records found.</td></tr>';
 $('recordCount').textContent=rows.length+' Records';$('totals').innerHTML='<tr><td>Total</td>'+columns.map(([key,,,type])=>'<td>'+(type==='sum'?display(rows.reduce((sum,row)=>sum+number(row[key]),0),'sum'):'')+'</td>').join('')+'</tr>';
}
function reset(){for(const id of ['fromNo','toNo','orderNoFrom','orderNoTo','parentCategoryId','itemClassId','itemTypeId','itemId','warehouseId','jobLotId','cropYearId'])$(id).value='';$('dateType').focus()}
async function print(key,id){
 const status=await request('/api/reports/print/status');if(!status.available)throw Error('The desktop report engine is unavailable. Please contact your administrator.');
 const f=filter(),args=id?{id}:{fromDate:f.fromDate,toDate:f.toDate,fromNo:f.fromNo,toNo:f.toNo,customerId:f.customerId};
 const response=await fetch('/api/reports/'+key+'/print.pdf',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(args)});
 if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw Error('Your session has expired. Please sign in again.');
 if(!response.ok)throw Error(await response.text()||'Could not print report');
 if(!(response.headers.get('Content-Type')||'').includes('application/pdf'))throw Error('The report did not return a PDF');
 const url=URL.createObjectURL(await response.blob()),link=document.createElement('a');link.href=url;link.download=id?'GDN-'+id+'.pdf':'GDN-Register.pdf';link.click();setTimeout(()=>URL.revokeObjectURL(url),60000);
}
function shortcuts(){alert('Ctrl+N New | Ctrl+R Refresh | Ctrl+S Show | Ctrl+P Register | Ctrl+Up Date Type | Ctrl+Down Grid | Ctrl+E Exit')}
async function init(){
 $('report').style.minWidth=(50+columns.reduce((sum,c)=>sum+c[2],0))+'px';$('columns').innerHTML='<col style="width:50px">'+columns.map(([, ,width])=>'<col style="width:'+width+'px">').join('');
 $('headers').innerHTML='<tr><th>Print</th>'+columns.map(([key,caption])=>'<th tabindex="0" data-sort="'+key+'">'+escape(caption)+'</th>').join('')+'</tr><tr class="filter-row"><th></th>'+columns.map(([key,caption])=>'<th><input data-column-filter="'+key+'" aria-label="Filter '+escape(caption)+'"></th>').join('')+'</tr>';
 const data=await request(api+'/initial');state.branches=data.branches;state.yearStart=data.yearStart;
 $('branchOptions').innerHTML=state.branches.map(row=>'<label><input type="checkbox" value="'+number(row.BranchId)+'"'+(number(row.BranchId)===number(data.branchId)?' checked':'')+'>'+escape(row.BranchName)+'</label>').join('');branchCaption();bindLookups(data.lookups);
 $('toDate').value=ymd(new Date());datePreset();const args=new URLSearchParams(location.search);for(const id of ['fromDate','toDate','fromNo','toNo','customerId'])if(args.has(id))$(id).value=args.get(id);render();
}
$('show').addEventListener('click',()=>run(show));$('refresh').addEventListener('click',()=>run(refreshLookups));$('new').addEventListener('click',reset);$('shortcuts').addEventListener('click',shortcuts);$('register').addEventListener('click',()=>run(()=>print('gdn-261')));
$('dateType').addEventListener('change',datePreset);$('branchOptions').addEventListener('change',()=>{branchCaption();run(refreshLookups)});
$('allBranches').addEventListener('change',event=>{document.querySelectorAll('#branchOptions input').forEach(el=>{el.checked=event.target.checked});branchCaption();run(refreshLookups)});
$('branchSearch').addEventListener('input',event=>document.querySelectorAll('#branchOptions label').forEach(label=>{label.hidden=!label.textContent.toLowerCase().includes(event.target.value.toLowerCase())}));
$('rows').addEventListener('click',event=>{const button=event.target.closest('.print-row');if(button)run(()=>print('gdn-260',number(button.dataset.id)))});
$('headers').addEventListener('input',render);$('headers').addEventListener('click',event=>{const cell=event.target.closest('[data-sort]');if(!cell)return;state.sort=[cell.dataset.sort,state.sort?.[0]===cell.dataset.sort?-state.sort[1]:1];render()});
$('headers').addEventListener('keydown',event=>{if(event.key==='Enter'&&event.target.dataset.sort)event.target.click()});
$('fullscreen').addEventListener('click',async()=>{try{if(document.fullscreenElement)await document.exitFullscreen();else await $('gridSection').requestFullscreen()}catch(error){message(error.message)}});
document.addEventListener('keydown',event=>{
 if(state.busy)return;const key=event.key.toLowerCase();if(event.ctrlKey){let action={n:reset,r:()=>run(refreshLookups),s:()=>run(show),p:()=>run(()=>print('gdn-261')),arrowup:()=>$('dateType').focus(),f5:()=>$('dateType').focus(),arrowdown:()=>document.querySelector('.table-wrap').focus(),e:()=>{location.href='/sale/reports'}}[key];if(action){event.preventDefault();action()}return}
 if(event.key==='Enter'&&event.target.matches('input:not([type=checkbox])')){event.preventDefault();const controls=[...document.querySelectorAll('input,select,button,summary')].filter(el=>!el.disabled&&el.getClientRects().length),index=controls.indexOf(event.target);controls[index+1]?.focus()}
});
run(init);
})();
