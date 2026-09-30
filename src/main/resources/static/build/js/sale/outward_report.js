(() => {
  'use strict';
  const $=id=>document.getElementById(id),api='/sale/reports/outward-gate-pass-report/api',text=value=>value==null?'':String(value);
  const hidden=new Set(['Id','WeighBridgeId']),dates=new Set(['GpDate','EntryDate','ModifyDate','PostDate']),times=new Set(['InDateTime','OutDateTime']);
  const totals=new Set(['NetPaid','DoQty','GrossWeight','GrossWeightWb','TareWeightWb','FactoryWeight','SupplierWeight','DifferenceWeight']);
  let busy=false,columns=[],yearStart='';
  const onlyPending=new URLSearchParams(location.search).get('onlyPending')==='true';
  const localDate=d=>`${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`;
  function dateText(value,withTime){if(!value)return '';const raw=text(value),d=new Date(/^\d{4}-\d{2}-\d{2}$/.test(raw)?raw+'T00:00:00':raw);if(Number.isNaN(d.getTime()))return raw;
    let result=`${String(d.getDate()).padStart(2,'0')}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getFullYear()).slice(-2)}`;
    if(withTime)result+=` ${String(d.getHours()%12||12).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')} ${d.getHours()<12?'AM':'PM'}`;return result;}
  async function run(action){if(busy)return;busy=true;$('loader').hidden=false;$('message').textContent='';const controls=Array.from(document.querySelectorAll('input,select,button')).map(el=>[el,el.disabled]);controls.forEach(([el])=>{el.disabled=true;el.setAttribute('aria-busy','true');});
    try{await action();}catch(error){$('message').textContent=error.message||text(error);}finally{controls.forEach(([el,disabled])=>{el.disabled=disabled;el.removeAttribute('aria-busy');});busy=false;$('loader').hidden=true;}}
  async function request(path,body){const headers={'Accept':'application/json'},csrf=document.querySelector('meta[name="_csrf"]'),header=document.querySelector('meta[name="_csrf_header"]');if(csrf&&header)headers[header.content]=csrf.content;
    const options={headers};if(body){options.method='POST';headers['Content-Type']='application/json';options.body=JSON.stringify(body);}const response=await fetch(api+path,options);
    if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw new Error('Please sign in again.');const result=await response.json().catch(()=>{throw new Error('The server did not return report data.');});if(!response.ok)throw new Error(result.message||result.detail||`Report request failed (${response.status})`);return result;}
  function branches(){return Array.from($('branchIds').selectedOptions,o=>Number(o.value));}
  function notify(select){if(window.jQuery)window.jQuery(select).trigger('change.select2');}
  function fill(id,rows,activity){const select=$(id);select.replaceChildren(new Option('...Select Any Value...',''));
    for(const row of rows.filter(r=>r.Activity===activity))select.add(new Option(text(row.ReferenceName),text(id==='gatePassType'?row.ReferenceName:row.Id)));notify(select);}
  async function lookups(){fill('customerId',[],'Customer');fill('gatePassType',[],'GatePassType');const ids=branches();if(!ids.length)return;const rows=await request('/lookups?branchIds='+encodeURIComponent(ids.join(',')));fill('customerId',rows,'Customer');fill('gatePassType',rows,'GatePassType');}
  function period(){const type=Number($('dateType').value),d=new Date();if(type===1)$('fromDate').value=localDate(d);if(type===2){d.setDate(d.getDate()-7);$('fromDate').value=localDate(d);}if(type===3){d.setDate(1);$('fromDate').value=localDate(d);$('toDate').value=localDate(new Date());}if(type===4){d.setMonth(0,1);$('fromDate').value=localDate(d);$('toDate').value=localDate(new Date());}if(type===5)$('fromDate').value=yearStart;}
  function render(rows){const table=$('reportTable'),keys=columns.filter(k=>!hidden.has(k)),head=document.createElement('tr');for(const key of keys){const th=document.createElement('th');th.textContent=({GpSrNo:'GP No',DeliveryOrderNo:'DO No',JobLotDescription:'Job Lot'})[key]||key.replace(/([a-z])([A-Z])/g,'$1 $2');head.append(th);}table.tHead.replaceChildren(head);table.tBodies[0].replaceChildren();table.tFoot.replaceChildren();$('recordCount').textContent=`${rows.length} records`;
    if(!rows.length){const tr=document.createElement('tr'),td=document.createElement('td');td.colSpan=Math.max(keys.length,1);td.textContent='No records found.';tr.append(td);table.tBodies[0].append(tr);return;}
    const sums={};for(const key of totals)sums[key]=0;
    for(const row of rows){const tr=document.createElement('tr');for(const key of keys){const td=document.createElement('td');
      if(key==='GpSrNo'&&Number(row.Id)>0&&Number(row.DocumentTypeId)>0){const a=document.createElement('a');a.href='#';a.textContent=text(row[key]);a.addEventListener('click',e=>{e.preventDefault();run(async()=>{if(!window.DocLink)throw new Error('Document navigation is unavailable.');await window.DocLink.open(Number(row.DocumentTypeId),Number(row.Id));});});td.append(a);}
      else td.textContent=dates.has(key)||times.has(key)?dateText(row[key],times.has(key)):text(row[key]);if(typeof row[key]==='number')td.classList.add('numeric');tr.append(td);if(totals.has(key)&&typeof row[key]==='number')sums[key]+=row[key];}table.tBodies[0].append(tr);}
    const footer=document.createElement('tr');keys.forEach((key,index)=>{const td=document.createElement('td');td.textContent=totals.has(key)?sums[key].toLocaleString('en-GB',{maximumFractionDigits:3}):index===0?'Total':'';if(totals.has(key))td.classList.add('numeric');footer.append(td);});table.tFoot.append(footer);
  }
  async function show(){const f={fromDate:$('fromDate').value,toDate:$('toDate').value,fromNo:Number($('fromNo').value)||0,toNo:Number($('toNo').value)||0,customerId:Number($('customerId').value)||0,gatePassType:$('gatePassType').value,status:$('status').value,branchIds:branches(),onlyPending};if(!f.branchIds.length)throw new Error('Select Branch First');if(!f.fromDate||!f.toDate)throw new Error('From Date and To Date are required');render([]);render(await request('/rows',f));}
  $('reportForm').addEventListener('submit',e=>{e.preventDefault();run(show);});$('refreshButton').addEventListener('click',()=>run(lookups));
  $('newButton').addEventListener('click',()=>run(async()=>{$('fromNo').value='';$('toNo').value='';$('status').value='';notify($('status'));await show();}));
  if(window.jQuery&&window.jQuery.fn.select2){window.jQuery('.searchable').select2({width:'100%'});window.jQuery('#branchIds').on('change',()=>run(lookups));window.jQuery('#dateType').on('change',period);}else{$('branchIds').addEventListener('change',()=>run(lookups));$('dateType').addEventListener('change',period);}
  document.addEventListener('keydown',e=>{if(!e.ctrlKey||busy)return;const key=e.key.toLowerCase();if(['s','r','n'].includes(key)){e.preventDefault();$(key==='s'?'showButton':key==='r'?'refreshButton':'newButton').click();}});
  run(async()=>{const data=await request('/initial');columns=data.columns;yearStart=data.yearStart;const seen=new Set();for(const row of data.branches){if(seen.has(row.BranchId))continue;seen.add(row.BranchId);$('branchIds').add(new Option(text(row.BranchName),text(row.BranchId),false,Number(row.BranchId)===Number(data.branchId)));}notify($('branchIds'));$('toDate').value=localDate(new Date());period();render([]);await lookups();});
})();
