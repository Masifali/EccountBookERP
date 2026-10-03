(() => {
  'use strict';
  const $ = id => document.getElementById(id), api = '/sale/reports/sale-order-report/api';
  const text = value => value == null ? '' : String(value);
  const lookupFields = {customerId:'Customer',itemId:'Item',parentCategoryId:'ParentCategory',categoryId:'ItemCategory',itemTypeId:'ItemType',bookingPersonId:'BookingPerson',cityId:'City',referencePartyId:'RefPartyDetail',jobLotId:'JobLot',packingTypeId:'PackingType',districtId:'District',cropYear:'Crop'};
  const labels = {customerId:'Customer',itemId:'Item',parentCategoryId:'Parent Category',categoryId:'Item Category',itemTypeId:'Item Type',bookingPersonId:'Booking Person',cityId:'City',referencePartyId:'Reference Party',jobLotId:'Job Lot',packingTypeId:'Packing Type',districtId:'District',cropYear:'Crop Year'};
  const hiddenColumns = new Set(['Id','DocumentTypeId','OrderSupCustId','SupplierCustomerId','OrderItemId','ItemId','SaleGLAC']);
  const dateColumns = new Set(['DocDate','DueDate','ExpiryDate','EntryDate','ModifyDate','ApprovedDate']);
  let busy = false, detailData = null, detailFilter = null, summaryFilter = null, grants = new Set();
  const selectedRows = new Set(), rowEditors = new Map();
  const actionRights = {Complete:'CanChangeOrderStatusToComplete',Cancel:'CanChangeOrderStatusToCancel',Open:'CanChangeOrderStatusToOpen',UpdateExpiryDate:'CanChangeOrderExpiryDate'};
  const control = (tab,name) => $(`${tab}_${name}`);
  const isoDate = date => `${date.getFullYear()}-${String(date.getMonth()+1).padStart(2,'0')}-${String(date.getDate()).padStart(2,'0')}`;
  function field(tab,name,label,type='select',wide=false) {
    const wrap=document.createElement('label');wrap.className='field'+(wide?' wide':'');wrap.append(document.createTextNode(label));
    const input=document.createElement(type==='select'?'select':'input');input.id=`${tab}_${name}`;input.name=name;
    if(type!=='select')input.type=type;
    if(type==='number')input.step='1';
    if(type==='checkbox')wrap.classList.add('check');
    if(type==='select')input.classList.add('searchable');
    wrap.append(input);$(`${tab}Filters`).append(wrap);return input;
  }
  function options(select,rows,id,label,blank=true) {
    select.replaceChildren();if(blank)select.add(new Option('...Select Any Value...',''));
    for(const row of rows)select.add(new Option(text(row[label]),text(row[id])));
    if(window.jQuery)window.jQuery(select).trigger('change.select2');
  }
  function refreshSelect(input){if(window.jQuery)window.jQuery(input).trigger('change.select2');}
  for(const tab of ['detail','summary']) {
    field(tab,'fromDate','From Date','date');field(tab,'toDate','To Date','date');
    field(tab,'fromNo','From Doc No','number');field(tab,'toNo','To Doc No','number');
    {const b=field(tab,'branchIds','Branch','select',true);b.multiple=true;b.classList.remove('searchable');b.setAttribute('data-dtcombo','single');b.setAttribute('data-dtcombo-checked','');b.setAttribute('data-dtcombo-caption','Branch Name');}
    field(tab,'costCenterId','Cost Center');
    for(const [name,label] of Object.entries(labels))if(tab==='summary'||!['jobLotId','packingTypeId','districtId','cropYear'].includes(name))field(tab,name,label);
    const status=field(tab,'status','Order Status'),approval=field(tab,'approval','Approval Status');
    options(status,['Open','Cancel','Complete'].map(value=>({value})), 'value','value');
    options(approval,['UnApprove','Approve','All'].map(value=>({value})), 'value','value',false);
    status.value='Open';approval.value='Approve';
    field(tab,'includeApprovedDo','D.O. Approved Include','checkbox');
    if(tab==='summary'){field(tab,'packUom','Pack UOM');field(tab,'activity','Activity','select',true);field(tab,'skipZero','Skip Zero','checkbox');}
  }
  async function run(action) {
    if(busy)return;busy=true;$('loader').hidden=false;$('message').textContent='';
    const controls=Array.from(document.querySelectorAll('input,select,button')).map(el=>[el,el.disabled]);
    controls.forEach(([el])=>{el.disabled=true;el.setAttribute('aria-busy','true');});
    try{await action();}catch(error){$('message').textContent=error.message||text(error);}
    finally{controls.forEach(([el,disabled])=>{el.disabled=disabled||el.dataset.locked==='true';el.removeAttribute('aria-busy');});$('loader').hidden=true;busy=false;}
  }
  async function request(path,body) {
    const headers={'Accept':'application/json'};
    const csrf=document.querySelector('meta[name="_csrf"]'),header=document.querySelector('meta[name="_csrf_header"]');
    if(csrf&&header)headers[header.content]=csrf.content;
    const config={headers};if(body){config.method='POST';headers['Content-Type']='application/json';config.body=JSON.stringify(body);}
    const response=await fetch(api+path,config);
    if(response.redirected&&/\/login(?:[?#]|$)/.test(response.url))throw new Error('Please sign in again.');
    const data=await response.json().catch(()=>{throw new Error('The server did not return report data.');});
    if(!response.ok)throw new Error(data.message||data.detail||`Report request failed (${response.status})`);
    return data;
  }
  function branchIds(tab){return Array.from(control(tab,'branchIds').selectedOptions,option=>Number(option.value));}
  async function lookups(tab) {
    for(const name of Object.keys(lookupFields))if(control(tab,name))options(control(tab,name),[],'Id','ReferenceName');
    const ids=branchIds(tab);if(!ids.length)return;
    const data=await request(`/lookups?branchIds=${encodeURIComponent(ids.join(','))}&costCenterId=${Number(control(tab,'costCenterId').value)||0}`);
    for(const [name,activity] of Object.entries(lookupFields))if(control(tab,name)) {
      // Native Crop parameter is display text; all other lookups pass their actual database Id.
      options(control(tab,name),data.filter(row=>row.Activity===activity),name==='cropYear'?'ReferenceName':'Id','ReferenceName');
    }
  }
  function filter(tab) {
    const f={};for(const el of $(`${tab}Form`).elements){if(!el.name||el.name==='branchIds')continue;
      if(el.type==='checkbox')f[el.name]=el.checked;
      else if(['fromDate','toDate'].includes(el.name))f[el.name]=el.value||null;
      else if(['status','approval','activity','cropYear'].includes(el.name))f[el.name]=el.value;
      else f[el.name]=Number(el.value)||0;
    }f.branchIds=branchIds(tab);return f;
  }
  function dateText(value) {
    if(!value)return '';const raw=text(value),date=new Date(/^\d{4}-\d{2}-\d{2}$/.test(raw)?raw+'T00:00:00':raw);
    if(Number.isNaN(date.getTime()))return raw;
    return `${String(date.getDate()).padStart(2,'0')}-${date.toLocaleString('en-GB',{month:'short'})}-${String(date.getFullYear()).slice(-2)}`;
  }
  function availableActions() {
    const f=detailFilter;if(!detailData||!f||f.approval!=='Approve')return [];
    const actions=f.status==='Open'?['Complete','Cancel','UpdateExpiryDate']:['Cancel','Complete'].includes(f.status)?['Open']:[];
    return actions.filter(action=>grants.has(actionRights[action]));
  }
  function canSelectRows() {
    if(!detailData||detailFilter?.approval!=='Approve')return false;
    const base=grants.has(actionRights.Complete)||grants.has(actionRights.Cancel);
    return $('detailView').value==='header'?Boolean(detailFilter.status)&&base:base||grants.has(actionRights.Open);
  }
  function updateActionButtons() {
    const allowed=availableActions();
    const auto=Boolean(detailData&&detailFilter?.approval==='Approve'&&['Open','Cancel'].includes(detailFilter.status)
      &&(grants.has(actionRights.Complete)||grants.has(actionRights.Cancel)));
    for(const button of document.querySelectorAll('[data-order-action]'))button.hidden=button.dataset.orderAction==='AutoComplete'?!auto:!allowed.includes(button.dataset.orderAction);
    $('orderActions').hidden=!Array.from(document.querySelectorAll('[data-order-action]')).some(b=>!b.hidden);
  }
  async function changeOrders(action,rowIndex) {
    if(!detailData||!detailFilter)throw new Error('Press Show to load the orders first.');
    const auto=action==='AutoComplete',grid=auto?'header':$('detailView').value;
    const rows=grid==='header'?detailData.headers:detailData.rows;
    const indexes=auto?rows.map((_,index)=>index):rowIndex===undefined?Array.from(selectedRows):[rowIndex];
    if(!indexes.length)throw new Error('There is no record to Update Status');
    const editor=rowIndex===undefined?null:rowEditors.get(rowIndex);
    const remarks=auto?'':editor?editor.remarks.value:window.prompt('Action Remarks Required','');
    if(remarks===null)return;
    if(!auto&&!remarks)throw new Error('Action Remarks Required');
    const expiryDate=action==='UpdateExpiryDate'?editor?.expiry?.value:null;
    if(action==='UpdateExpiryDate'&&!expiryDate)throw new Error('Enter the new expiry date.');
    if(!window.confirm(auto?'Are you sure to Auto Complete Order?':`Are you sure to ${action==='UpdateExpiryDate'?'Update ExpiryDate':action+' Status'}?`))return;
    const f=detailFilter;
    let result;
    try {
      result=await request('/action',{filter:f,grid,action,remarks,expiryDate,bulk:rowIndex===undefined,
        selections:indexes.map(index=>({id:Number(rows[index].Id),rowIndex:index}))});
    } catch(error) {
      // A native bulk action can fail after earlier selections have been committed.
      // Require a fresh read before another action, including after a lost response.
      detailData=null;detailFilter=null;selectedRows.clear();updateActionButtons();
      render('detailTable',[],[]);render('linesTable',[],[]);$('detailCount').textContent='0 records';
      throw new Error(error.message+' Press Show to reload current order status.');
    }
    try {await show('detail',f);}catch(error){throw new Error(result.message+' Reload failed: '+error.message);}
    $('message').textContent=result.message;
  }
  function render(id,rows,columns,onSelect) {
    const table=$(id),body=table.tBodies[0],keys=columns.filter(key=>!hiddenColumns.has(key));
    const orderGrid=id==='detailTable',actions=orderGrid?availableActions():[],selectable=orderGrid&&canSelectRows();
    const approvalFilter=orderGrid?detailFilter:id==='summaryTable'&&summaryFilter?.activity==='Order Register'?summaryFilter:null;
    if(orderGrid){selectedRows.clear();rowEditors.clear();}
    const heading=document.createElement('tr');
    if(approvalFilter){const th=document.createElement('th');th.textContent='Approval Detail';heading.append(th);}
    if(selectable){const th=document.createElement('th'),all=document.createElement('input');all.type='checkbox';all.setAttribute('aria-label','Select all report rows');
      all.addEventListener('change',()=>{if(busy)return;body.querySelectorAll('[data-row-selector]').forEach(box=>{box.checked=all.checked;if(all.checked)selectedRows.add(Number(box.dataset.rowSelector));else selectedRows.delete(Number(box.dataset.rowSelector));});});th.append(all);heading.append(th);}
    for(const key of keys){const th=document.createElement('th');th.textContent=key.replace(/([a-z])([A-Z])/g,'$1 $2');heading.append(th);}
    for(const action of actions){const th=document.createElement('th');th.textContent=action==='UpdateExpiryDate'?'Update Expiry Date':action;heading.append(th);}
    table.tHead.replaceChildren(heading);body.replaceChildren();
    if(!rows.length){const tr=document.createElement('tr'),td=document.createElement('td');td.colSpan=Math.max(heading.children.length,1);td.textContent='No records found.';tr.append(td);body.append(tr);return;}
    rows.forEach((row,rowIndex)=>{const tr=document.createElement('tr'),editors={};
      if(approvalFilter){const td=document.createElement('td'),button=document.createElement('button');button.type='button';button.textContent='Approval Detail';
        button.addEventListener('click',e=>{e.stopPropagation();run(()=>window.SaleApprovalHistory.open(()=>request('/approval-history',{
          filter:approvalFilter,summary:id==='summaryTable',id:Number(row.Id),documentTypeId:Number(row.DocumentTypeId)}),run));});td.append(button);tr.append(td);}
      if(selectable){const td=document.createElement('td'),box=document.createElement('input');box.type='checkbox';box.dataset.rowSelector=String(rowIndex);box.setAttribute('aria-label',`Select order ${row.DocNo}`);
        box.addEventListener('click',e=>e.stopPropagation());box.addEventListener('change',()=>{if(busy)return;if(box.checked)selectedRows.add(rowIndex);else selectedRows.delete(rowIndex);});td.append(box);tr.append(td);}
      if(onSelect){tr.tabIndex=0;const select=()=>{Array.from(body.rows).forEach(r=>r.classList.remove('selected'));tr.classList.add('selected');onSelect(row);};tr.addEventListener('click',select);tr.addEventListener('keydown',e=>{if(e.key==='Enter'&&e.target===tr){e.preventDefault();select();}});}
      for(const key of keys){const td=document.createElement('td');
        if(key==='DocNo'&&Number(row.Id)>0&&Number(row.DocumentTypeId)>0){const link=document.createElement('a');link.href='#';link.textContent=text(row[key]);
          link.addEventListener('click',e=>{e.preventDefault();e.stopPropagation();run(async()=>{if(!window.DocLink)throw new Error('Document navigation is unavailable.');await window.DocLink.open(Number(row.DocumentTypeId),Number(row.Id));});});td.append(link);
        }else if(key==='ActionRemarks'&&actions.length){const input=document.createElement('input');input.type='text';input.value=text(row[key]);input.setAttribute('aria-label',`Action remarks for ${row.DocNo}`);editors.remarks=input;td.append(input);}
        else if(key==='ExpiryDate'&&actions.includes('UpdateExpiryDate')){const input=document.createElement('input');input.type='date';input.value=text(row[key]).slice(0,10);input.setAttribute('aria-label',`Expiry date for ${row.DocNo}`);editors.expiry=input;td.append(input);}
        else{td.textContent=dateColumns.has(key)?dateText(row[key]):text(row[key]);if(typeof row[key]==='number')td.classList.add('numeric');}
        tr.append(td);
      }
      if(orderGrid)rowEditors.set(rowIndex,editors);
      for(const action of actions){const td=document.createElement('td'),button=document.createElement('button');button.type='button';button.textContent=action==='UpdateExpiryDate'?'Update Expiry Date':action;
        button.addEventListener('click',e=>{e.stopPropagation();run(()=>changeOrders(action,rowIndex));});td.append(button);tr.append(td);}
      body.append(tr);
    });
  }
  function showDetail() {
    if(!detailData)return;const main=$('detailView').value==='header';$('linesPanel').hidden=!main;
    const rows=main?detailData.headers:detailData.rows,columns=main?detailData.headerColumns:detailData.columns;
    $('detailCount').textContent=`${rows.length} records`;
    render('detailTable',rows,columns,main?row=>render('linesTable',detailData.lines[row.Id]||[],detailData.lineColumns):null);
    render('linesTable',[],detailData.lineColumns);
    updateActionButtons();
  }
  async function show(tab,savedFilter) {
    const f=savedFilter||filter(tab);if(!f.branchIds.length)throw new Error('Select Branch First');if(!f.toDate)throw new Error('To Date is required');
    if(tab==='detail'){detailData=null;detailFilter=null;updateActionButtons();render('detailTable',[],[]);render('linesTable',[],[]);$('detailCount').textContent='0 records';}
    else{summaryFilter=null;render('summaryTable',[],[]);$('summaryCount').textContent='0 records';}
    const data=await request('/'+tab,f);
    if(tab==='detail'){detailData=data;detailFilter=f;showDetail();}
    else{summaryFilter=f;render('summaryTable',data.rows,data.columns);$('summaryCount').textContent=`${data.rows.length} records`;}
  }
  function dates(tab){const today=new Date(),from=new Date();from.setDate(from.getDate()-7);control(tab,'fromDate').value=isoDate(from);control(tab,'toDate').value=isoDate(today);}
  function reset(tab){dates(tab);control(tab,'fromNo').value='';control(tab,'toNo').value='';
    const clear=tab==='detail'?['customerId','itemId','parentCategoryId','categoryId','bookingPersonId','itemTypeId']:['parentCategoryId','categoryId','itemTypeId','cropYear','packUom','packingTypeId','districtId','cityId','jobLotId','itemId','customerId','referencePartyId'];
    for(const name of clear){control(tab,name).value='';refreshSelect(control(tab,name));}
    if(tab==='detail'){control(tab,'includeApprovedDo').checked=false;control(tab,'approval').value='UnApprove';control(tab,'status').value='Open';refreshSelect(control(tab,'approval'));refreshSelect(control(tab,'status'));detailData=null;detailFilter=null;updateActionButtons();render('detailTable',[],[]);render('linesTable',[],[]);$('detailCount').textContent='0 records';}
    else{control(tab,'activity').selectedIndex=0;refreshSelect(control(tab,'activity'));summaryFilter=null;render('summaryTable',[],[]);$('summaryCount').textContent='0 records';}
  }
  document.querySelectorAll('[data-tab]').forEach(button=>button.addEventListener('click',()=>{if(busy)return;for(const tab of ['detail','summary'])$(`${tab}Panel`).hidden=tab!==button.dataset.tab;document.querySelectorAll('[data-tab]').forEach(b=>b.setAttribute('aria-selected',String(b===button)));}));
  document.querySelectorAll('[data-refresh]').forEach(b=>b.addEventListener('click',()=>run(()=>lookups(b.dataset.refresh))));
  document.querySelectorAll('[data-new]').forEach(b=>b.addEventListener('click',()=>run(async()=>reset(b.dataset.new))));
  $('detailView').addEventListener('change',showDetail);
  document.querySelectorAll('[data-order-action]').forEach(button=>button.addEventListener('click',()=>run(()=>changeOrders(button.dataset.orderAction))));
  for(const tab of ['detail','summary'])$(`${tab}Form`).addEventListener('submit',e=>{e.preventDefault();run(()=>show(tab));});
  if(window.jQuery&&window.jQuery.fn.select2)window.jQuery('.searchable').attr('data-dtcombo-skip','').select2({width:'100%'});
  for(const tab of ['detail','summary'])for(const name of ['branchIds','costCenterId']) {
    if(window.jQuery&&window.jQuery.fn.select2)window.jQuery(control(tab,name)).on('change',()=>run(()=>lookups(tab)));
    else control(tab,name).addEventListener('change',()=>run(()=>lookups(tab)));
  }
  run(async()=>{
    const data=await request('/initial');
    grants=new Set(data.actionRights||[]);
    for(const tab of ['detail','summary']){
      dates(tab);options(control(tab,'branchIds'),data.branches,'BranchId','BranchName',false);
      Array.from(control(tab,'branchIds').options).forEach(option=>option.selected=Number(option.value)===Number(data.branchId));refreshSelect(control(tab,'branchIds'));
      options(control(tab,'costCenterId'),data.costCenters,'Id','CostCenterName');
      if(data.costCenterLocked&&data.costCenters.length){control(tab,'costCenterId').value=text(data.costCenters[0].Id);control(tab,'costCenterId').dataset.locked='true';refreshSelect(control(tab,'costCenterId'));}
    }
    options(control('summary','activity'),data.activities.map(value=>({value})),'value','value',false);
    options(control('summary','packUom'),data.packUoms.map(value=>({id:value,label:value+'KG'})),'id','label');
    await lookups('detail');await lookups('summary');
  });
})();
