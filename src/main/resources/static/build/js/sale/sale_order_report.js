(() => {
  'use strict';
  const $ = id => document.getElementById(id), api = '/sale/reports/sale-order-report/api';
  const text = value => value == null ? '' : String(value);
  const lookupFields = {customerId:'Customer',itemId:'Item',parentCategoryId:'ParentCategory',categoryId:'ItemCategory',itemTypeId:'ItemType',bookingPersonId:'BookingPerson',cityId:'City',referencePartyId:'RefPartyDetail',jobLotId:'JobLot',packingTypeId:'PackingType',districtId:'District',cropYear:'Crop'};
  const labels = {customerId:'Customer Name',itemId:'Item Name',parentCategoryId:'Parent Category',categoryId:'Item Category',itemTypeId:'Item Type',bookingPersonId:'Booking Person',cityId:'City Name',referencePartyId:'Ref Party Detail',jobLotId:'Job Lot',packingTypeId:'Packing Type',districtId:'District',cropYear:'Crop Year'};
  const hiddenColumns = new Set(['Id','DocumentTypeId','OrderSupCustId','SupplierCustomerId','OrderItemId','ItemId','SaleGLAC']);
  const dateColumns = new Set(['DocDate','DueDate','ExpiryDate','EntryDate','ModifyDate','ApprovedDate']);
  let busy = false, detailData = null, detailFilter = null, summaryData = null, summaryFilter = null, summaryPrintLabels = {}, grants = new Set();
  const selectedRows = new Set(), rowEditors = new Map();
  const gridStates = new Map();
  const totalColumns = new Set(['ItemQty','OrderQty','DispatchQty','DispatchedQty','BalQty','Weight','OrderWeight','DispatchWeight','DispatchedWeight','BalWeight','ItemAmount','OrderAmount','DispatchedAmount','BalAmount','CommAmount']);
  const numberText = value => Number(value).toLocaleString('en-US',{maximumFractionDigits:2});
  const caption = key => key.replace(/([a-z])([A-Z])/g,'$1 $2');
  let selectedOrderId = null;
  const actionRights = {Complete:'CanChangeOrderStatusToComplete',Cancel:'CanChangeOrderStatusToCancel',Open:'CanChangeOrderStatusToOpen',UpdateExpiryDate:'CanChangeOrderExpiryDate'};
  const control = (tab,name) => $(`${tab}_${name}`);
  const isoDate = date => `${date.getFullYear()}-${String(date.getMonth()+1).padStart(2,'0')}-${String(date.getDate()).padStart(2,'0')}`;
  function field(tab,name,label,type='select',wide=false) {
    const wrap=document.createElement('label');wrap.className='field'+(wide?' wide':'');const title=document.createElement('span');title.textContent=label;wrap.append(title);
    const input=document.createElement(type==='select'?'select':'input');input.id=`${tab}_${name}`;input.name=name;
    if(type!=='select')input.type=type;
    if(type==='number')input.step='1';
    input.setAttribute('aria-label',label);
    if(type==='checkbox')wrap.classList.add('check');
    if(type==='select')input.classList.add('searchable');
    wrap.append(input);$(`${tab}Filters`).append(wrap);return input;
  }
  function arrangeDetailFilters() {
    const parent=$('detailFilters'),columns=Array.from({length:4},()=>{const col=document.createElement('div');col.className='filter-column';return col;});
    const move=(column,name)=>columns[column].append(control('detail',name).parentElement);
    const pair=(label,a,b)=>{const wrap=document.createElement('div');wrap.className='paired-fields';const title=document.createElement('span');title.textContent=label;wrap.append(title,control('detail',a).parentElement,control('detail',b).parentElement);columns[0].append(wrap);};
    move(0,'branchIds');pair('From,To Date','fromDate','toDate');pair('SO From & To','fromNo','toNo');move(0,'parentCategoryId');
    ['categoryId','itemTypeId','itemId','customerId'].forEach(name=>move(1,name));
    ['referencePartyId','bookingPersonId','cityId','approval'].forEach(name=>move(2,name));
    ['status','includeApprovedDo'].forEach(name=>move(3,name));
    columns[3].append($('detailForm').querySelector('.show'));parent.replaceChildren(...columns);
  }
  function arrangeSummaryFilters(){
    const columns=Array.from({length:4},()=>{const column=document.createElement('div');column.className='filter-column';return column;});
    const move=(column,name)=>columns[column].append(control('summary',name).parentElement);
    const pair=(label,a,b)=>{const row=document.createElement('div');row.className='paired-fields';const title=document.createElement('span');title.textContent=label;row.append(title,control('summary',a).parentElement,control('summary',b).parentElement);columns[0].append(row);};
    move(0,'branchIds');pair('From & To Date','fromDate','toDate');pair('From & To Doc No','fromNo','toNo');['parentCategoryId','categoryId','itemTypeId'].forEach(name=>move(0,name));
    ['cropYear','packUom','packingTypeId','districtId','cityId','jobLotId'].forEach(name=>move(1,name));
    ['itemId','customerId','bookingPersonId','referencePartyId','activity','approval'].forEach(name=>move(2,name));
    ['status','skipZero','includeApprovedDo'].forEach(name=>move(3,name));columns[3].append($('summaryForm').querySelector('.show'));
    $('summaryFilters').replaceChildren(...columns);
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
    if(tab==='summary'){field(tab,'packUom','Pack UOM');field(tab,'activity','Report Type','select',true);field(tab,'skipZero','Skip Zero','checkbox');}
  }
  // Keep native form associations when moving Cost Center into the desktop title band.
  for(const tab of ['detail','summary']) {
    const cost=control(tab,'costCenterId');cost.setAttribute('form',`${tab}Form`);
    cost.parentElement.hidden=tab!=='detail';$('costCenters').append(cost.parentElement);
  }
  arrangeDetailFilters();
  arrangeSummaryFilters();
  async function run(action) {
    if(busy)return;busy=true;$('loader').hidden=false;$('message').textContent='';
    const controls=Array.from($('saleOrderReport').querySelectorAll('input,select,button')).map(el=>[el,el.disabled]);
    controls.forEach(([el])=>{el.disabled=true;el.setAttribute('aria-busy','true');});
    try{await action();}catch(error){$('message').textContent=error.message||text(error);}
    finally{controls.forEach(([el,disabled])=>{el.disabled=disabled||el.dataset.locked==='true';el.removeAttribute('aria-busy');});$('loader').hidden=true;busy=false;updateSummaryPrint();}
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
    if(!value)return '';const raw=text(value),date=new Date(typeof value==='number'?value:/^\d{4}-\d{2}-\d{2}$/.test(raw)?raw+'T00:00:00':raw);
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
    if(keys.includes('ActionRemarks')){keys.splice(keys.indexOf('ActionRemarks'),1);keys.unshift('ActionRemarks');}
    const orderGrid=id==='detailTable',actions=orderGrid?availableActions():[],selectable=orderGrid&&canSelectRows();
    const approvalFilter=orderGrid?detailFilter:id==='summaryTable'&&summaryFilter?.activity==='Order Register'?summaryFilter:null;
    if(orderGrid){selectedRows.clear();rowEditors.clear();}
    const heading=document.createElement('tr');
    if(approvalFilter){const th=document.createElement('th');th.textContent='Approval Detail';heading.append(th);}
    if(selectable){const th=document.createElement('th'),all=document.createElement('input');all.type='checkbox';all.setAttribute('aria-label','Select all report rows');
      all.addEventListener('change',()=>{if(busy)return;body.querySelectorAll('[data-row-selector]').forEach(box=>{if(box.closest('tr').hidden)return;box.checked=all.checked;if(all.checked)selectedRows.add(Number(box.dataset.rowSelector));else selectedRows.delete(Number(box.dataset.rowSelector));});});th.append(all);heading.append(th);}
    for(const key of keys){const th=document.createElement('th');th.textContent=caption(key);th.dataset.col=key;th.draggable=true;
      th.addEventListener('dragstart',e=>e.dataTransfer.setData('text/plain',JSON.stringify({table:id,key})));heading.append(th);}
    for(const action of actions){const th=document.createElement('th');th.textContent=action==='UpdateExpiryDate'?'Update Expiry Date':action;heading.append(th);}
    table.tHead.replaceChildren(heading);body.replaceChildren();table.tFoot.replaceChildren();
    const widths=Array.from(heading.cells,th=>/Name|Party|Remarks|Term/.test(th.dataset.col||'')?190:/Date/.test(th.dataset.col||'')?95:th.dataset.col?90:th.textContent?110:32);
    Array.from(heading.cells).forEach((th,i)=>{th.style.width=widths[i]+'px';});table.style.width=widths.reduce((a,b)=>a+b,0)+'px';
    gridStates.set(id,{rows,keys,filters:new Map(),group:null,heading,onSelect});
    const strip=document.querySelector(`[data-group-table="${id}"]`);strip.firstChild.textContent='Drag a column header here to group by that column.';strip.querySelector('button').hidden=true;
    if(!rows.length){const tr=document.createElement('tr'),td=document.createElement('td');td.colSpan=Math.max(heading.children.length,1);td.textContent='No records found.';tr.append(td);body.append(tr);return;}
    rows.forEach((row,rowIndex)=>{const tr=document.createElement('tr'),editors={};tr.dataset.rowIndex=rowIndex;
      if(approvalFilter){const td=document.createElement('td'),button=document.createElement('button');button.type='button';button.textContent='Approval Detail';
        button.addEventListener('click',e=>{e.stopPropagation();run(()=>window.SaleApprovalHistory.open(()=>request('/approval-history',{
          filter:approvalFilter,summary:id==='summaryTable',id:Number(row.Id),documentTypeId:Number(row.DocumentTypeId)}),run));});td.append(button);tr.append(td);}
      if(selectable){const td=document.createElement('td'),box=document.createElement('input');box.type='checkbox';box.dataset.rowSelector=String(rowIndex);box.setAttribute('aria-label',`Select order ${row.DocNo}`);
        box.addEventListener('click',e=>e.stopPropagation());box.addEventListener('change',()=>{if(busy)return;if(box.checked)selectedRows.add(rowIndex);else selectedRows.delete(rowIndex);});td.append(box);tr.append(td);}
      if(onSelect){tr.tabIndex=0;const select=()=>{Array.from(body.rows).forEach(r=>r.classList.remove('selected'));tr.classList.add('selected');selectedOrderId=row.Id;onSelect(row);updateRecordPosition();};tr.addEventListener('click',select);tr.addEventListener('keydown',e=>{if(e.key==='Enter'&&e.target===tr){e.preventDefault();select();}});}
      for(const key of keys){const td=document.createElement('td');
        if(key==='DocNo'&&Number(row.Id)>0&&Number(row.DocumentTypeId)>0){const link=document.createElement('a');link.href='#';link.textContent=text(row[key]);
          link.addEventListener('click',e=>{e.preventDefault();e.stopPropagation();run(async()=>{if(!window.DocLink)throw new Error('Document navigation is unavailable.');await window.DocLink.open(Number(row.DocumentTypeId),Number(row.Id));});});td.append(link);
        }else if((key==='CustomerName'&&Number(row.SupplierCustomerId||row.OrderSupCustId)>0)||(key==='ItemName'&&Number(row.ItemId||row.OrderItemId)>0)){
          const link=document.createElement('a'),customer=key==='CustomerName',params=new URLSearchParams(),loaded=detailFilter;
          params.set(customer?'supplierCustomerId':'itemId',text(customer?(row.SupplierCustomerId||row.OrderSupCustId):(row.ItemId||row.OrderItemId)));
          if(loaded?.fromDate)params.set('fromDate',loaded.fromDate);if(loaded?.toDate)params.set('toDate',loaded.toDate);
          link.href=(customer?'/accounts/reports/customer-ledger':'/stocks/item-ledger')+'?'+params;link.target='_blank';link.rel='noopener';link.textContent=text(row[key]);link.addEventListener('click',e=>e.stopPropagation());td.append(link);
        }else if(key==='ActionRemarks'&&actions.length){const input=document.createElement('input');input.type='text';input.value=text(row[key]);input.setAttribute('aria-label',`Action remarks for ${row.DocNo}`);editors.remarks=input;td.append(input);}
        else if(key==='ExpiryDate'&&actions.includes('UpdateExpiryDate')){const input=document.createElement('input');input.type='date';input.value=typeof row[key]==='number'?isoDate(new Date(row[key])):text(row[key]).slice(0,10);input.setAttribute('aria-label',`Expiry date for ${row.DocNo}`);editors.expiry=input;td.append(input);}
        else{td.textContent=dateColumns.has(key)?dateText(row[key]):typeof row[key]==='number'?numberText(row[key]):text(row[key]);if(typeof row[key]==='number')td.classList.add('numeric');}
        td.title=td.textContent;
        tr.append(td);
      }
      if(orderGrid)rowEditors.set(rowIndex,editors);
      for(const action of actions){const td=document.createElement('td'),button=document.createElement('button');button.type='button';button.textContent=action==='UpdateExpiryDate'?'Update Expiry Date':action;
        button.addEventListener('click',e=>{e.stopPropagation();run(()=>changeOrders(action,rowIndex));});td.append(button);tr.append(td);}
      body.append(tr);
    });
    const filters=document.createElement('tr');filters.className='column-filters';
    Array.from(heading.cells).forEach(th=>{const cell=document.createElement('th');if(th.dataset.col){const input=document.createElement('input');input.setAttribute('aria-label','Filter '+caption(th.dataset.col));input.addEventListener('input',()=>{gridStates.get(id).filters.set(th.dataset.col,input.value.toLowerCase());applyGridFilter(id);});cell.append(input);}filters.append(cell);});table.tHead.append(filters);
    updateTotals(id);if(window.GridBar)window.GridBar.refresh(table.parentElement);
  }
  function showDetail() {
    const main=$('detailView').value==='header';$('linesPanel').hidden=!main;$('saleOrderReport').classList.toggle('brief-view',main);$('detailGridTitle').textContent=main?'Main Grid':'Detail Register';
    document.querySelectorAll('[data-view]').forEach(button=>button.setAttribute('aria-selected',String(button.dataset.view===$('detailView').value)));
    if(!detailData)return;
    const rows=main?detailData.headers:detailData.rows,columns=main?detailData.headerColumns:detailData.columns;
    $('detailCount').textContent=`${rows.length} records`;
    render('detailTable',rows,columns,main?row=>{const lines=detailData.lines[row.Id]||[];render('linesTable',lines,detailData.lineColumns);$('linesCount').textContent=`Order ${row.DocNo} · ${lines.length} items`;}:null);
    if(main){const index=Math.max(0,rows.findIndex(row=>row.Id===selectedOrderId));const first=$('detailTable').tBodies[0].querySelector(`[data-row-index="${index}"]`);if(first)first.click();else{render('linesTable',[],detailData.lineColumns);$('linesCount').textContent='';}}
    updateRecordPosition();
    updateActionButtons();
  }
  function dataRows(id){return Array.from($(id).tBodies[0].querySelectorAll('tr[data-row-index]'));}
  function visibleRows(id){return dataRows(id).filter(row=>!row.hidden&&!row.classList.contains('gb-collapsed'));}
  function updateRecordPosition(){const rows=visibleRows('detailTable');const index=rows.findIndex(row=>row.classList.contains('selected'));$('currentRecord').textContent=`${index+1} of ${rows.length}`;document.querySelectorAll('[data-record]').forEach(button=>button.hidden=$('detailView').value!=='header');}
  function updateTotals(id){
    const state=gridStates.get(id),foot=document.createElement('tr');if(!state)return;
    const shown=dataRows(id).filter(row=>!row.hidden).map(row=>state.rows[Number(row.dataset.rowIndex)]);
    Array.from(state.heading.cells).forEach((th,index)=>{const td=document.createElement('td'),key=th.dataset.col;
      if(totalColumns.has(key)){td.textContent=numberText(shown.reduce((sum,row)=>sum+(Number(row[key])||0),0));td.className='numeric';}
      else if(index===0)td.textContent='Total:';foot.append(td);
    });$(id).tFoot.replaceChildren(foot);
  }
  function applyGridFilter(id){
    const state=gridStates.get(id);for(const tr of dataRows(id)){
      const row=state.rows[Number(tr.dataset.rowIndex)];tr.hidden=Array.from(state.filters).some(([key,value])=>value&&!text(row[key]).toLowerCase().includes(value)&&!text(tr.cells[Array.from(state.heading.cells).findIndex(th=>th.dataset.col===key)]?.textContent).toLowerCase().includes(value));
      if(tr.hidden){const box=tr.querySelector('[data-row-selector]');if(box){box.checked=false;selectedRows.delete(Number(tr.dataset.rowIndex));}}
    }
    applyGrouping(id);updateTotals(id);
    if(id==='detailTable'){
      const shown=visibleRows(id);$('detailCount').textContent=`${shown.length} of ${state.rows.length} records`;
      if(state.onSelect){if(shown.length)(shown.find(row=>row.classList.contains('selected'))||shown[0]).click();else{render('linesTable',[],detailData.lineColumns);$('linesCount').textContent='';}}
      updateRecordPosition();
    }
    if(id==='summaryTable')$('summaryCount').textContent=`${dataRows(id).filter(row=>!row.hidden).length} of ${state.rows.length} records`;
    if(window.GridBar)window.GridBar.refresh($(id).parentElement);
  }
  function applyGrouping(id){
    const state=gridStates.get(id),body=$(id).tBodies[0],rows=dataRows(id).sort((a,b)=>Number(a.dataset.rowIndex)-Number(b.dataset.rowIndex));
    body.querySelectorAll('tr.gb-group').forEach(row=>row.remove());
    rows.forEach(row=>{delete row.dataset.gbMember;row.classList.remove('gb-collapsed');body.append(row);});
    if(!state.group)return;
    const groups=new Map();rows.filter(row=>!row.hidden).forEach(tr=>{const value=text(state.rows[Number(tr.dataset.rowIndex)][state.group]);if(!groups.has(value))groups.set(value,[]);groups.get(value).push(tr);});
    let index=0;for(const [value,members] of groups){const group=document.createElement('tr'),cell=document.createElement('td'),key=String(index++);group.className='gb-group';group.dataset.gbGroup=key;cell.colSpan=state.heading.cells.length;cell.textContent=`${caption(state.group)}: ${value||'(blank)'} (${members.length})`;group.append(cell);group.tabIndex=0;
      const toggle=()=>{const closed=group.classList.toggle('gb-group-closed');members.forEach(row=>row.classList.toggle('gb-collapsed',closed));updateRecordPosition();};group.addEventListener('click',toggle);group.addEventListener('keydown',e=>{if(e.key==='Enter'){e.preventDefault();toggle();}});
      body.append(group);members.forEach(row=>{row.dataset.gbMember=key;body.append(row);});
    }
  }
  document.querySelectorAll('[data-group-table]').forEach(strip=>{
    strip.addEventListener('dragover',e=>{e.preventDefault();strip.classList.add('drag-over');});strip.addEventListener('dragleave',()=>strip.classList.remove('drag-over'));
    strip.addEventListener('drop',e=>{e.preventDefault();strip.classList.remove('drag-over');let dropped;try{dropped=JSON.parse(e.dataTransfer.getData('text/plain'));}catch{return;}
      const id=strip.dataset.groupTable,state=gridStates.get(id);if(!state||dropped.table!==id||!state.keys.includes(dropped.key))return;
      state.group=dropped.key;strip.firstChild.textContent='Grouped by '+caption(dropped.key);strip.querySelector('button').hidden=false;applyGridFilter(id);
    });
    strip.querySelector('button').addEventListener('click',()=>{const id=strip.dataset.groupTable;gridStates.get(id).group=null;strip.firstChild.textContent='Drag a column header here to group by that column.';strip.querySelector('button').hidden=true;applyGridFilter(id);});
  });
  document.querySelectorAll('[data-record]').forEach(button=>button.addEventListener('click',()=>{if(busy)return;const rows=visibleRows('detailTable');if(!rows.length)return;let index=rows.findIndex(row=>row.classList.contains('selected'));
    index=button.dataset.record==='first'?0:button.dataset.record==='last'?rows.length-1:Math.max(0,Math.min(rows.length-1,index+(button.dataset.record==='next'?1:-1)));rows[index].click();rows[index].scrollIntoView({block:'nearest',inline:'nearest'});
  }));
  async function printReport(variant){
    const loadedFilter=variant==='summary'?summaryFilter:detailFilter,loadedData=variant==='summary'?summaryData:detailData;
    if(!loadedFilter||!loadedData?.rows.length)throw new Error('Press Show to load the report before printing.');
    const headers={'Content-Type':'application/json','Accept':'application/pdf'},csrf=document.querySelector('meta[name="_csrf"]'),header=document.querySelector('meta[name="_csrf_header"]');if(csrf&&header)headers[header.content]=csrf.content;
    const preview=window.open('about:blank','_blank');if(!preview)throw new Error('Allow pop-ups to open the report.');
    preview.document.title='Preparing Sale Order Report';preview.document.body.textContent='Preparing report…';
    try{const response=await fetch(api+'/print/'+variant,{method:'POST',headers,body:JSON.stringify(loadedFilter)});
      if(!response.ok||!response.headers.get('Content-Type')?.includes('application/pdf')){const error=await response.text();let message;try{message=JSON.parse(error).message;}catch{if(response.headers.get('Content-Type')?.includes('text/plain'))message=error;}throw new Error(message||'Unable to print this report. Please check your session and report filters.');}
      const url=URL.createObjectURL(await response.blob());preview.location.href=url;setTimeout(()=>URL.revokeObjectURL(url),300000);
    }catch(error){preview.close();throw error;}
  }
  document.querySelectorAll('[data-print]').forEach(button=>button.addEventListener('click',()=>run(()=>printReport(button.dataset.print))));
  document.querySelectorAll('[data-shortcuts]').forEach(button=>button.addEventListener('click',()=>$('shortcutDialog').showModal()));
  document.addEventListener('keydown',e=>{if(busy||$('shortcutDialog').open)return;const tab=$('detailPanel').hidden?'summary':'detail',key=e.key.toLowerCase();
    if(e.ctrlKey&&['s','r','n'].includes(key)){e.preventDefault();run(()=>key==='s'?show(tab):key==='r'?lookups(tab):reset(tab));}
    if(e.altKey&&['1','2','3'].includes(key)&&tab==='detail'){e.preventDefault();run(()=>printReport({'1':'270','2':'271','3':'271A'}[key]));}
    if(e.ctrlKey&&key==='p'&&tab==='summary'){e.preventDefault();run(()=>printReport('summary'));}
    if(e.ctrlKey&&key==='t'){e.preventDefault();document.querySelector(`[data-tab="${tab==='detail'?'summary':'detail'}"]`).click();}
  });
  function updateSummaryPrint(){
    const activity=summaryFilter?.activity||control('summary','activity').value;
    $('summaryPrint').textContent='▣ '+(summaryData?.printLabel||summaryPrintLabels[activity]||'Print');
    $('summaryPrint').disabled=busy||!summaryData?.rows.length;
  }
  function clearSummary(){summaryFilter=null;summaryData=null;render('summaryTable',[],[]);$('summaryCount').textContent='0 records';$('summaryTitle').textContent=control('summary','activity').value||'Summary';updateSummaryPrint();}
  async function show(tab,savedFilter) {
    const f=savedFilter||filter(tab);if(!f.branchIds.length)throw new Error('Select Branch First');if(!f.toDate)throw new Error('To Date is required');
    if(tab==='detail'){detailData=null;detailFilter=null;updateActionButtons();render('detailTable',[],[]);render('linesTable',[],[]);$('detailCount').textContent='0 records';$('linesCount').textContent='';updateRecordPosition();}
    else clearSummary();
    const data=await request('/'+tab,f);
    if(tab==='detail'){detailData=data;detailFilter=f;showDetail();}
    else{summaryFilter=f;summaryData=data;render('summaryTable',data.rows,data.columns);$('summaryTitle').textContent=f.activity;$('summaryCount').textContent=`${data.rows.length} records`;updateSummaryPrint();}
  }
  function dates(tab){const today=new Date(),from=new Date();from.setDate(from.getDate()-7);control(tab,'fromDate').value=isoDate(from);control(tab,'toDate').value=isoDate(today);}
  function reset(tab){dates(tab);control(tab,'fromNo').value='';control(tab,'toNo').value='';
    const clear=tab==='detail'?['customerId','itemId','parentCategoryId','categoryId','bookingPersonId','itemTypeId']:['parentCategoryId','categoryId','itemTypeId','cropYear','packUom','packingTypeId','districtId','cityId','jobLotId','itemId','customerId','referencePartyId'];
    for(const name of clear){control(tab,name).value='';refreshSelect(control(tab,name));}
    if(tab==='detail'){control(tab,'includeApprovedDo').checked=false;control(tab,'approval').value='UnApprove';control(tab,'status').value='Open';refreshSelect(control(tab,'approval'));refreshSelect(control(tab,'status'));detailData=null;detailFilter=null;selectedOrderId=null;updateActionButtons();render('detailTable',[],[]);render('linesTable',[],[]);$('detailCount').textContent='0 records';$('linesCount').textContent='';updateRecordPosition();}
    else{control(tab,'activity').selectedIndex=0;refreshSelect(control(tab,'activity'));clearSummary();}
  }
  document.querySelectorAll('[data-tab]').forEach(button=>button.addEventListener('click',()=>{if(busy)return;for(const tab of ['detail','summary']){$(`${tab}Panel`).hidden=tab!==button.dataset.tab;control(tab,'costCenterId').parentElement.hidden=tab!==button.dataset.tab;}document.querySelectorAll('[data-tab]').forEach(b=>b.setAttribute('aria-selected',String(b===button)));}));
  document.querySelectorAll('[data-refresh]').forEach(b=>b.addEventListener('click',()=>run(()=>lookups(b.dataset.refresh))));
  document.querySelectorAll('[data-new]').forEach(b=>b.addEventListener('click',()=>run(async()=>reset(b.dataset.new))));
  $('detailView').addEventListener('change',showDetail);
  document.querySelectorAll('[data-view]').forEach(button=>button.addEventListener('click',()=>{if(busy)return;$('detailView').value=button.dataset.view;showDetail();}));
  document.querySelectorAll('[data-order-action]').forEach(button=>button.addEventListener('click',()=>run(()=>changeOrders(button.dataset.orderAction))));
  for(const tab of ['detail','summary'])$(`${tab}Form`).addEventListener('submit',e=>{e.preventDefault();run(()=>show(tab));});
  if(window.jQuery&&window.jQuery.fn.select2)window.jQuery('.searchable').attr('data-dtcombo-skip','').select2({width:'100%'});
  if(window.jQuery)window.jQuery(control('summary','activity')).on('change',()=>{if(!busy)clearSummary();});else control('summary','activity').addEventListener('change',()=>{if(!busy)clearSummary();});
  for(const tab of ['detail','summary'])for(const name of ['branchIds','costCenterId']) {
    if(window.jQuery&&window.jQuery.fn.select2)window.jQuery(control(tab,name)).on('change',()=>run(()=>lookups(tab)));
    else control(tab,name).addEventListener('change',()=>run(()=>lookups(tab)));
  }
  showDetail();
  run(async()=>{
    const data=await request('/initial');
    summaryPrintLabels=data.summaryPrintLabels||{};
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
