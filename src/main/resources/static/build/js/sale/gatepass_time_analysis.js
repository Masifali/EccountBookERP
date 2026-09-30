(() => {
  'use strict';
  const api='/sale/gatepass-vehicle-time-analysis/api', $=id=>document.getElementById(id);
  let busy=false,yearStart='';
  const columns=['PartyName','GpSrNo','GpDate','VehicleNo','BiltyDate','GpInDateTime','GpOutDateTime','GpProcessedTime','FirstWbDateTime','SecondWbDateTime','WBProcessedTime','Status','Remarks','GatePassType','BiltyNo','Container','Container1'];
  const summaryColumns=['DocumentType','TypeDescription','TotalVehicles','TotalQty','TotalWeight','AvgTime','MinTime','MaxTime'];
  const dates=new Set(['GpDate','BiltyDate']),times=new Set(['GpInDateTime','GpOutDateTime','FirstWbDateTime','SecondWbDateTime']);
  const text=value=>value==null?'':String(value),number=value=>Number(value||0);
  const localDate=value=>`${value.getFullYear()}-${String(value.getMonth()+1).padStart(2,'0')}-${String(value.getDate()).padStart(2,'0')}`;
  function displayDate(value,withTime){
    if(!value)return '';
    // Parse date-only values as local midnight; ISO instants from SQL timestamps use the local day/time.
    const raw=text(value),date=new Date(/^\d{4}-\d{2}-\d{2}$/.test(raw)?raw+'T00:00:00':raw);
    if(Number.isNaN(date.getTime()))return raw;
    const day=localDate(date).split('-');let result=`${day[2]}-${day[1]}-${day[0].slice(-2)}`;
    if(withTime)result+=` ${String(date.getHours()%12||12).padStart(2,'0')}:${String(date.getMinutes()).padStart(2,'0')} ${date.getHours()<12?'AM':'PM'}`;
    return result;
  }
  function heading(table,keys){const tr=document.createElement('tr');for(const key of keys){const th=document.createElement('th');th.textContent=key==='DocumentType'?'Doc Type':key;tr.append(th);}table.tHead.replaceChildren(tr);}
  function empty(table,message,span){const tr=document.createElement('tr'),td=document.createElement('td');td.colSpan=span;td.textContent=message;tr.append(td);table.tBodies[0].replaceChildren(tr);}
  async function run(action){
    if(busy)return;busy=true;$('loader').hidden=false;$('message').textContent='';
    const controls=Array.from(document.querySelectorAll('button,input,select')).map(control=>[control,control.disabled]);
    controls.forEach(([control])=>{control.disabled=true;control.setAttribute('aria-busy','true');});
    try{await action();}catch(error){$('message').textContent=error.message||String(error);}
    finally{controls.forEach(([control,disabled])=>{control.disabled=disabled;control.removeAttribute('aria-busy');});busy=false;$('loader').hidden=true;}
  }
  async function request(path,options){
    const response=await fetch(api+path,options);
    if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw new Error('Please sign in again.');
    const result=await response.json().catch(()=>{throw new Error('The server did not return report data.');});
    if(!response.ok)throw new Error(result.message||result.detail||`Report request failed (${response.status})`);
    return result;
  }
  function selectedBranches(){return Array.from($('branchIds').selectedOptions,option=>Number(option.value));}
  function fillLookup(id,rows,activity){
    const select=$(id);select.replaceChildren(new Option('...Select Any Value...','0'));
    rows.filter(row=>row.Activity===activity).forEach(row=>select.add(new Option(text(row.ReferenceName),text(row.Id))));
    if(window.jQuery)window.jQuery(select).trigger('change.select2');
  }
  async function lookups(){
    const ids=selectedBranches();fillLookup('customerId',[],'PartyName');fillLookup('documentTypeId',[],'DocumentType');
    if(!ids.length)return;
    const rows=await request('/lookups?branchIds='+encodeURIComponent(ids.join(',')));
    fillLookup('customerId',rows,'PartyName');fillLookup('documentTypeId',rows,'DocumentType');
  }
  function period(){
    const date=new Date(),type=number($('dateType').value);
    if(type===1)$('fromDate').value=localDate(date);
    if(type===2){date.setDate(date.getDate()-7);$('fromDate').value=localDate(date);}
    if(type===3){date.setDate(1);$('fromDate').value=localDate(date);$('toDate').value=localDate(new Date());}
    if(type===4){date.setMonth(0,1);$('fromDate').value=localDate(date);$('toDate').value=localDate(new Date());}
    if(type===5)$('fromDate').value=yearStart;
  }
  function detailRows(rows){
    const table=$('detailTable'),body=table.tBodies[0];body.replaceChildren();$('recordCount').textContent=rows.length;
    if(!rows.length){empty(table,'No records found.',columns.length);return;}
    const groups=new Map();for(const row of rows){const key=text(row.DocumentType);if(!groups.has(key))groups.set(key,[]);groups.get(key).push(row);}
    for(const [name,records] of groups){
      const group=document.createElement('tr'),cell=document.createElement('td');group.className='group';cell.colSpan=columns.length;cell.textContent=`${name} (${records.length})`;group.append(cell);body.append(group);
      for(const row of records){const tr=document.createElement('tr');
        for(const key of columns){const td=document.createElement('td');
          if(key==='GpSrNo'&&number(row.Id)>0){const link=document.createElement('a');link.href='#';link.textContent=text(row[key]);
            link.addEventListener('click',event=>{event.preventDefault();run(async()=>{if(!window.DocLink)throw new Error('Document navigation is unavailable.');await window.DocLink.open(number(row.DocumentTypeId),number(row.Id));});});td.append(link);
          }else td.textContent=dates.has(key)?displayDate(row[key],false):times.has(key)?displayDate(row[key],true):text(row[key]);
          tr.append(td);
        }body.append(tr);
      }
    }
  }
  function summaryRows(id,rows){
    const table=$(id),body=table.tBodies[0];body.replaceChildren();
    if(!rows.length){empty(table,'No records found.',summaryColumns.length);return;}
    for(const row of rows){const tr=document.createElement('tr');for(const key of summaryColumns){const td=document.createElement('td');td.textContent=text(row[key]);tr.append(td);}body.append(tr);}
  }
  async function show(){
    const filter={fromDate:$('fromDate').value,toDate:$('toDate').value,fromNo:number($('fromNo').value),toNo:number($('toNo').value),customerId:number($('customerId').value),documentTypeId:number($('documentTypeId').value),status:document.querySelector('input[name="status"]:checked').value,branchIds:selectedBranches()};
    if(!filter.fromDate||!filter.toDate)throw new Error('From Date and To Date are required');
    if(!filter.branchIds.length)throw new Error('Select Branch First');
    detailRows([]);summaryRows('inwardTable',[]);summaryRows('outwardTable',[]);
    const data=await request('/rows',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(filter)});
    detailRows(data.rows||[]);summaryRows('inwardTable',data.inward||[]);summaryRows('outwardTable',data.outward||[]);
  }
  heading($('detailTable'),columns);heading($('inwardTable'),summaryColumns);heading($('outwardTable'),summaryColumns);
  empty($('detailTable'),'Press Show to load.',columns.length);
  $('showButton').addEventListener('click',()=>run(show));$('refreshButton').addEventListener('click',()=>run(lookups));
  $('newButton').addEventListener('click',()=>run(async()=>{$('fromNo').value='';$('toNo').value='';$('customerId').value='0';if(window.jQuery)window.jQuery('#customerId').trigger('change.select2');await show();}));
  $('dateType').addEventListener('change',period);
  if(window.jQuery&&window.jQuery.fn.select2){window.jQuery('.searchable').select2({width:'100%'});}
  $('branchIds').addEventListener('change',()=>run(lookups));
  /* cmbBranchName: the desktop UltraCombo with check boxes (BranchesFill / CheckedListSettings) - one row per
     allocated branch, the user's own branch checked; the hidden <select multiple> stays the value source. */
  function branchMultiSync(){const sel=$('branchIds');const names=Array.from(sel.selectedOptions,o=>o.text);$('branchMultiText').textContent=names.join(', ');$('branchMultiText').title=names.join(', ');}
  function branchMultiBuild(){const sel=$('branchIds'),list=$('branchMultiList');list.innerHTML='';for(const o of sel.options){const l=document.createElement('label');const c=document.createElement('input');c.type='checkbox';c.checked=o.selected;c.addEventListener('change',()=>{o.selected=c.checked;branchMultiSync();sel.dispatchEvent(new Event('change'));});l.append(c,document.createTextNode(o.text));list.append(l);}branchMultiSync();}
  $('branchMultiBtn').addEventListener('click',e=>{e.stopPropagation();$('branchMulti').classList.toggle('open');});
  $('branchMultiList').addEventListener('click',e=>e.stopPropagation());
  document.addEventListener('click',()=>$('branchMulti').classList.remove('open'));
  run(async()=>{
    const data=await request('/initial');yearStart=data.yearStart;$('toDate').value=localDate(new Date());period();
    const seen=new Set();for(const branch of data.branches||[]){const id=number(branch.BranchId);if(seen.has(id))continue;seen.add(id);$('branchIds').add(new Option(text(branch.BranchName),String(id),false,id===number(data.branchId)));}
    branchMultiBuild();await lookups();
  });
})();
