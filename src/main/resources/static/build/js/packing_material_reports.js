/* Native report contracts: migration/packing-material/desktop-layout.json and recovered BLL. */
(function () {
    'use strict';
    const definitions = {
        'grn-register': 'BranchName:Branch:120,DocDate:GRN Date:70:d,DocNo:GRN No:50:grn,PoDate:Order Date:70:d,PoNo:Order No:50:po,CompanyName:Supplier Name:150,GpDate:G.P Date:70:d,GpNo:G.P No:50:gp,VehicleNo:Vehicle No:75,BiltyNo:Bilty No:70,TransporterName:Transporter:120,FreightAmount:Freight Amount:90:n2,DeliveryTerm:Delivery Term:90,ItemName:Item:150,UOMCode:Pack UOM:60,ItemQty:Qty:80:n4,AdLsWeight:Wt Per Qty:100:n4,GrossWeight:Gross Weight:90:n2,AreaCity:City:80,WareHouseName:Warehouse:120,NoOfAttachments:Attachments:80:attachments,CommentsDetail:Remarks:150',
        'requirement-planning-detail': 'ItemCodeNew:Item Code:100,ItemName:Item Name:180,CustomerName:Customer Name:170,ScheduleCode:Schedule Code:110,CustomerContractNo:Customer Contract No:125,PortName:Destination Port:120,LoadingDate:Loading Date:90:d,ProductionDate:Production Date:95:d,PackingMaterialDate:Packing Material Date:105:d,TotalRequiredQty:Required Qty:100:n2,ReservedStock:Reserved Stock:100:n2,ConsumedReserved:Consumed Reserved:110:n2,RemainingAfterReserved:Remaining After Reserved:120:n2,AvailableStock:Stock In Hand:100:n2,ConsumedFromStock:Consumed From Stock:110:n2,NeedFromOrders:Need From Orders:110:n2,OutstandingOrders:Outstanding Orders:115:n2,MoreRequired:More Required:100:n2',
        'purchase-order-register': 'BranchName:Branch:150,DocNo:Doc No:50:po,DocDate:Doc Date:75:d,SupplierName:Supplier Name:150:supplier,TermsDescription:Payment Term:70,OrderDueDays:Due Days:45,OrderDueDate:Due Date:75:d,DeliveryTerm:Delivery Term:70,DeliveryStartDate:Delivery Start Date:90:d,DeliveryDays:Delivery Days:70,OrderStatus:Order Status:70,OrderExpiryDate:Expiry Date:75:expiry,ItemName:Item Name:150,UomCodeItm:Pack UOM:60,OrderItemQty:Item Qty:80:n2,ReceivedQty:Received Qty:80:n2,BalQty:Balance Qty:80:n2,NetWeight:Net Weight:100:n2,ReceivedWeight:Received Weight:80:n2,BalWeight:Balance Weight:100:n2,OrderItemRate:Item Rate:90:n2,UomCodeRate:Rate UOM:60,Amount:Amount:100:n2,ReceivedAmount:Received Amount:100:n2,BalAmount:Balance Amount:100:n2,TaxName:Tax:100,TaxPercent:Tax Percent:80:n3,TaxAmount:Tax Amount:80:n2,IsApproved:Approved:80:approval,UserNameEusr:Entry User:80,EntryDate:Entry Date:80:d,RemarksHeader:Remarks:120',
        'inventory-transaction-report': 'BranchName:Branch:120,DocumentTypeDescription:Doc Type:150,DocDate:Doc Date:73:d,DocCodeNo:Doc Code:60:transaction,WareHouseName:Warehouse:150,RackName:Rack:150,ItemCode:Item Code:80,ItemName:Item Name:150,UOMCode:UOM:60,ItemCondition:Item Condition:130,InQty:In Qty:70:n3,OutQty:Out Qty:70:n3,BalQty:Balance Qty:70:n3',
        'stock-with-supplier': 'Doc_Date:Doc Date:73:d,Type:Type:70:slip,Doc_No:Doc No:60:slip,Supplier_Name:Supplier Name:190,Item_Name:Item Name:170,Tran_Desc:Description:150,Received:Received:110:n2,Issued:Issued:110:n2,CBalance:Closing:110:n2,EmptyBagsType:Empty Bags Type:130,BagsCondition:Bags Condition:130',
        trial: 'Supplier_Name:Supplier Name:220,Item_Name:Item Name:240,OBalance:Opening:110:n2,Received:Received:130:n2,Issued:Issued:130:n2,CBalance:Closing:110:n2'
    };
    const columnsFor = key => definitions[key].split(',').map(v => {const [key,title,width,type='text']=v.split(':');return {key,title,width:Number(width),type};});
    if (typeof module !== 'undefined' && module.exports) { module.exports={columnsFor}; return; }
    const $ = id => document.getElementById(id), report=document.body.dataset.report, api='/api/packing-material/reports/'+report;
    const C=window.StoreCommon, esc=C.esc, ci=C.ci;
    let initial={},rows=[],lastFilter=null,mode='ledger',pending=false,ready=false,headerKey='',sort=null,columnFilters={};
    const value=id=>$(id)?$(id).value:'', number=id=>value(id)===''?null:Number(value(id));
    function status(text,error=false,loading=false) {$('status').textContent=text;$('status').className=error?'error':loading?'loading':'';}
    async function busy(button,work,message='Loading…') {
        if(pending)return;pending=true;
        const states=Array.from(document.querySelectorAll('.toolbar button,.show-actions button,.grid-tools button,#grid button,#filters input,#filters select')).map(b=>[b,b.disabled]);
        states.forEach(([b])=>b.disabled=true);if(button)button.setAttribute('aria-busy','true');status(message,false,true);
        try{return await work();}catch(e){status(e.message||'The request failed',true);}finally{
            pending=false;states.forEach(([b,disabled])=>b.disabled=disabled);if(button)button.removeAttribute('aria-busy');$('status').classList.remove('loading');
        }
    }
    async function request(path,body,binary=false) {
        const media=binary===true?'application/pdf':binary;
        const headers={Accept:media||'application/json'};
        if(body!==undefined)headers['Content-Type']='application/json';
        const token=document.querySelector('meta[name="_csrf"]'),name=document.querySelector('meta[name="_csrf_header"]');
        if(token&&name&&body!==undefined)headers[name.content]=token.content;
        const response=await fetch(path,{method:body===undefined?'GET':'POST',credentials:'same-origin',headers,body:body===undefined?undefined:JSON.stringify(body)});
        if(response.redirected&&/\/login(?:[/?#]|$)/.test(response.url))throw Error('Your session has expired. Sign in again, then click Refresh.');
        if(media&&response.ok&&response.headers.get('content-type')?.includes(media))return response.blob();
        let data;const text=await response.text();try{data=JSON.parse(text);}catch(e){throw Error(!response.ok&&!text.trim().startsWith('<')?text:'The server did not return the report. Sign in again if needed, then click Refresh.');}
        if(!response.ok)throw Error(data.message||'Report request failed ('+response.status+')');return data;
    }
    function branchIds() {return Array.from(document.querySelectorAll('#branchOptions input:checked')).map(x=>x.value).join(',');}
    function fill(select,list,preserve=true) {
        const keep=preserve?select.value:'';
        select.innerHTML='<option value="">'+(list.length?'…Select Any Value…':'No records for these filters')+'</option>'+list.map(r=>'<option value="'+esc(select.id==='city'?r.text:r.id)+'">'+esc(r.text)+'</option>').join('');
        select.value=Array.from(select.options).some(o=>o.value===keep)?keep:'';
        select.dataset.count=String(list.length);
    }
    function bindChoices(choices) {document.querySelectorAll('select[data-group]').forEach(s=>{if(!['Status','Approval','DateType'].includes(s.dataset.group))fill(s,choices[s.dataset.group]||[]);});}
    function setStatic(id,items) {if($(id))fill($(id),items.map(([id,text])=>({id,text})),false);}
    function caption() {if($('branchCaption'))$('branchCaption').textContent=Array.from(document.querySelectorAll('#branchOptions input:checked')).map(x=>x.parentElement.textContent.trim()).join(', ')||'Select Branch';}
    async function initialize(reset=false) {
        const previous=branchIds();const d=await request(api+'/initial');initial=d;
        if($('branchOptions')) {
            $('branchOptions').innerHTML=d.branches.map(b=>'<label><input type="checkbox" value="'+esc(ci(b,'BranchId'))+'" '+((previous?previous.split(',').includes(String(ci(b,'BranchId'))):String(ci(b,'BranchId'))===String(d.branchId))?'checked':'')+'>'+esc(ci(b,'BranchName'))+'</label>').join('')||'<p>No report records in your allocated branches.</p>';caption();
        }
        if(!ready||reset) {
            if(!ready){$('fromDate').value=d.fromDate;$('toDate').value=d.today;}
            setStatic('orderStatus',[['Open','Open'],['Complete','Complete'],['Cancel','Cancel']]);if($('orderStatus'))$('orderStatus').value='Open';
            setStatic('approval',[['Approved','Approved'],['NotApproved','Not Approved'],['All','All']]);if($('approval'))$('approval').value='Approved';
            setStatic('dateType',[['today','Today'],['week','This Week'],['month','This Month'],['year','This Year'],['financial','This Financial Year']]);if($('dateType'))$('dateType').value='week';
        }
        bindChoices(previous?await request(api+'/lookups?branches='+encodeURIComponent(branchIds())):d.choices);ready=true;
        status(d.branches.length===0&&$('branchPicker')?'No Packing Material history exists in your allocated branches. History-based lists are empty.':'Filters loaded.');
        if(report==='requirement-planning-detail'||report==='stock-with-supplier')await load();
    }
    function filter() {return {fromDate:value('fromDate'),toDate:value('toDate'),branches:branchIds(),supplierId:number('supplierId'),itemId:number('itemId')|| (report==='requirement-planning-detail'?Number(new URLSearchParams(location.search).get('itemId'))||null:null),warehouseId:number('warehouseId'),city:value('city'),fromNo:number('fromNo'),toNo:number('toNo'),gpFrom:number('gpFrom'),gpTo:number('gpTo'),orderFrom:number('orderFrom'),orderTo:number('orderTo'),documentTypeId:number('documentTypeId'),itemTypeId:number('itemTypeId'),conditionId:number('conditionId'),rackId:number('rackId'),status:value('orderStatus'),approval:value('approval'),mode};}
    async function load() {
        if(!$('filters').reportValidity())return;
        const f=filter();rows=await request(api+'/data',f);lastFilter=f;$('search').value='';columnFilters={};headerKey='';sort=null;render();status(rows.length?rows.length+' records loaded.':'No records found for the selected filters.');
    }
    function allowed(action) {
        const rights={Complete:'CanChangeOrderStatusToComplete',Cancel:'CanChangeOrderStatusToCancel',Open:'CanChangeOrderStatusToOpen',UpdateExpiryDate:'CanChangeOrderExpiryDate'};
        return report==='purchase-order-register'&&lastFilter?.approval==='Approved'&&(initial.permissions||[]).includes(rights[action])&&(action==='Open'?lastFilter.status!=='Open':lastFilter.status==='Open');
    }
    function display(v,type) {
        if(v==null)return '';
        if(type==='approval')return v===true||v===1?'True':'False';
        if(type==='d'||type==='expiry')return C.gridDate(v).replace(/-(\d{2})(\d{2})$/, '-$2');if(type==='dt')return C.gridDateTime(v,true);
        if(type.startsWith('n')){const fixed=report==='stock-with-supplier'||report==='requirement-planning-detail';const text=Math.abs(Number(v)).toLocaleString('en-US',{minimumFractionDigits:fixed?Number(type.slice(1)):0,maximumFractionDigits:Number(type.slice(1))});return Number(v)<0?(fixed?'('+text+')':'-'+text):text;}
        return String(v);
    }
    const link=(href,text)=>'<a href="'+esc(href)+'" target="_blank" rel="noopener">'+esc(text)+'</a>';
    function cell(row,col,index) {
        const v=ci(row,col.key),id=Number(ci(row,'Id'));let text=display(v,col.type);
        if(col.type==='supplier'&&Number(ci(row,'OrderSupCustId'))>0)return link('/accounts/reports/customer-ledger?supplierCustomerId='+Number(ci(row,'OrderSupCustId'))+'&fromDate='+lastFilter.fromDate+'&toDate='+lastFilter.toDate,text);
        if(col.type==='grn'&&id>0)return link('/packing-material/grn?id='+id,text);
        if(col.type==='po'&&Number(report==='grn-register'?ci(row,'OrderId'):id)>0)return link('/packing-material/purchase-order?id='+Number(report==='grn-register'?ci(row,'OrderId'):id),text);
        if(col.type==='gp'&&Number(ci(row,'InwardGatePassId'))>0)return link('/purchase/inward-gate-pass?id='+Number(ci(row,'InwardGatePassId')),text);
        if(col.type==='slip'&&id>0&&Number(ci(row,'DocumentTypeId'))>0)return '<button class="link" data-act="document" data-index="'+index+'">'+esc(text)+'</button>';
        if(col.type==='transaction'&&Number(v)>0)return '<button class="link" data-act="transaction" data-index="'+index+'">'+esc(text)+'</button>';
        if(col.type==='attachments'&&Number(v)>0)return '<button class="link" data-act="attachments" data-index="'+index+'">'+esc(text)+'</button>';
        if(col.type==='bool')return '<input type="checkbox" disabled aria-label="Approved" '+(v===true||v===1?'checked':'')+'>';
        if(col.type==='expiry'&&allowed('UpdateExpiryDate'))return '<input type="date" aria-label="Order expiry date" data-expiry="'+index+'" value="'+esc(String(v||'').slice(0,10))+'">';
        return esc(text);
    }
    function render() {
        const key=report==='stock-with-supplier'&&mode==='trial'?'trial':report,columns=columnsFor(key),query=value('search').toLowerCase();
        const visible=rows.map((row,index)=>({row,index})).filter(({row})=>columns.some(c=>display(ci(row,c.key),c.type).toLowerCase().includes(query))&&columns.every(c=>!columnFilters[c.key]||(c.type==='d'?String(ci(row,c.key)||'').slice(0,10)===columnFilters[c.key]:display(ci(row,c.key),c.type).toLowerCase().includes(columnFilters[c.key].toLowerCase()))));
        if(sort)visible.sort((a,b)=>{const av=ci(a.row,sort.key),bv=ci(b.row,sort.key);return sort.direction*(typeof av==='number'&&typeof bv==='number'?av-bv:String(av??'').localeCompare(String(bv??'')));});
        const actionColumns=report==='grn-register'?['Print']:report==='purchase-order-register'?['Slip',...(initial.approvalDetail?['Approval Detail']:[]),...['Complete','Cancel','Open','UpdateExpiryDate'].filter(allowed)]:[];
        const nextHeader=key+actionColumns.join(',');
        if(nextHeader!==headerKey){
            headerKey=nextHeader;
            $('grid').querySelector('thead').innerHTML='<tr>'+actionColumns.map(t=>'<th>'+esc(t)+'</th>').join('')+columns.map(c=>'<th style="min-width:'+c.width+'px"'+(c.type.startsWith('n')?' class="number"':'')+'><button class="column-sort" data-sort="'+esc(c.key)+'">'+esc(c.title)+'</button></th>').join('')+'</tr><tr class="column-filters">'+actionColumns.map(()=>'<th></th>').join('')+columns.map(c=>'<th><input type="'+(c.type==='d'?'date':'search')+'" aria-label="Filter '+esc(c.title)+'" data-column="'+esc(c.key)+'" value="'+esc(columnFilters[c.key]||'')+'"></th>').join('')+'</tr>';
        }
        $('grid').querySelector('tbody').innerHTML=visible.map(({row,index})=>'<tr data-row="'+index+'">'+actionColumns.map(action=>'<td><button data-act="'+esc(action)+'" data-index="'+index+'">'+esc(action==='UpdateExpiryDate'?'Update Expiry Date':action)+'</button></td>').join('')+columns.map(c=>'<td'+(c.type.startsWith('n')?' class="number"':'')+'>'+cell(row,c,index)+'</td>').join('')+'</tr>').join('')||'<tr><td class="empty" colspan="'+(columns.length+actionColumns.length)+'">'+(lastFilter?'No records found.':'Press Show to load.')+'</td></tr>';
        const exclude=new Set(['TaxPercent','OrderItemRate',...(report==='stock-with-supplier'?['CBalance']:[]),...(report==='requirement-planning-detail'?['AvailableStock','OutstandingOrders','ReservedStock']:[])]);
        $('grid').querySelector('tfoot').innerHTML='<tr>'+actionColumns.map(()=>'<td></td>').join('')+columns.map((c,i)=>{
            if(!c.type.startsWith('n')||exclude.has(c.key))return '<td>'+(i===0?'Total':'')+'</td>';
            const total=report==='inventory-transaction-report'&&c.key==='BalQty'?visible.reduce((n,{row})=>n+Number(ci(row,'InQty')||0)-Number(ci(row,'OutQty')||0),0):visible.reduce((n,{row})=>n+Number(ci(row,c.key)||0),0);
            return '<td class="number">'+display(total,c.type)+'</td>';
        }).join('')+'</tr>';
        $('count').textContent=visible.length+' of '+rows.length+' records';
    }
    function dialog(title,html) {$('dialogTitle').textContent=title;$('dialogBody').innerHTML=html;$('detailDialog').showModal();}
    async function rowAction(button) {
        const row=rows[Number(button.dataset.index)],id=Number(ci(row,'Id')),act=button.dataset.act;
        if(!lastFilter)return;
        if(['Complete','Cancel','Open','UpdateExpiryDate'].includes(act)) {
            if(!confirm('Are you sure you want to '+(act==='UpdateExpiryDate'?'update the expiry date':act.toLowerCase()+' this order')+'?'))return;
            return busy(button,async()=>{await request(api+'/action',{filter:lastFilter,id,action:act,expiryDate:document.querySelector('[data-expiry="'+button.dataset.index+'"]')?.value||null});await load();},'Updating order…');
        }
        if(act==='Approval Detail')return busy(button,async()=>{const data=await request(api+'/approval-history',{filter:lastFilter,id});showDetails('Approval Detail',data);status('Approval detail loaded.');});
        if(act==='Print'||act==='Slip')return busy(button,async()=>{await downloadPdf('/api/reports/'+(act==='Print'?'grn-214':'po-201')+'/print.pdf',{id});status('Print preview ready.');},'Preparing print preview…');
        if(act==='attachments')return busy(button,async()=>{const data=await request('/api/packing-material/grn/'+id);const attachments=data.attachments||[];dialog('Attachments',attachments.length?attachments.map(a=>'<p>'+link('/api/packing-material/grn/'+id+'/attachments/'+ci(a,'Id'),ci(a,'FileName')||ci(a,'DocumentName')||'Attachment')+'</p>').join(''):'<p>No attachments available.</p>');status('Attachments loaded.');});
        if(act==='transaction')return busy(button,async()=>{const data=await request(api+'/document',{filter:lastFilter,row:Object.fromEntries(['ItemId','DocDate','DocCodeNo','DocumentTypeDescription','WareHouseName','RackName','BranchName','UOMCode','ItemConditionId'].map(k=>[k,ci(row,k)]))});dialog('Original Documents',data.map(d=>'<p>'+link(d.url,d.code+' — '+d.type)+'</p>').join(''));status('Original documents resolved.');});
        if(act==='document')return busy(button,async()=>{const data=await request(api+'/document',{filter:lastFilter,id,documentTypeId:Number(ci(row,'DocumentTypeId'))});if(data.url)dialog('Original Document','<p>'+link(data.url,'Open document '+ci(row,'Type')+' '+ci(row,'Doc_No'))+'</p>');else throw Error('This document has no matching original form.');status('Original document ready.');});
    }
    function showDetails(title,data) {const keys=data.length?Object.keys(data[0]):[];dialog(title,keys.length?'<table><thead><tr>'+keys.map(k=>'<th>'+esc(k)+'</th>').join('')+'</tr></thead><tbody>'+data.map(r=>'<tr>'+keys.map(k=>'<td>'+esc(r[k])+'</td>').join('')+'</tr>').join('')+'</tbody></table>':'<p>No history records found.</p>');}
    async function downloadPdf(url,body) {
        const blob=await request(url,body,true),href=URL.createObjectURL(blob);
        dialog('Print Preview','<p>'+link(href,'Open PDF')+'</p><iframe title="Report PDF" style="width:100%;height:55vh;border:0" src="'+href+'"></iframe>');
        $('detailDialog').addEventListener('close',()=>URL.revokeObjectURL(href),{once:true});
    }
    function reset() {
        if(pending)return;
        const ids=report==='grn-register'?['supplierId','fromNo','toNo','gpFrom','gpTo','warehouseId','city']:report==='purchase-order-register'?['supplierId','itemId','fromNo','toNo']:report==='inventory-transaction-report'?['warehouseId','documentTypeId','itemId','itemTypeId']:['supplierId','itemId'];
        ids.forEach(id=>{if($(id))$(id).value='';});
        if(report==='grn-register')$('fromDate').value=$('toDate').value=initial.today;
        if(report==='requirement-planning-detail'){const u=new URL(location);u.searchParams.delete('itemId');history.replaceState(null,'',u);}
        if(!['inventory-transaction-report','purchase-order-register'].includes(report)){rows=[];lastFilter=null;columnFilters={};headerKey='';sort=null;render();}status('Filters reset.');(report==='inventory-transaction-report'?$('dateType'):$('fromDate')).focus();
    }
    $('filters').addEventListener('submit',e=>{e.preventDefault();mode='ledger';busy($('show'),load);});
    $('trial')?.addEventListener('click',()=>{mode='trial';busy($('trial'),load);});
    $('new').addEventListener('click',reset);$('refresh').addEventListener('click',()=>busy($('refresh'),()=>initialize(report==='purchase-order-register')));
    $('grid').querySelector('thead').addEventListener('input',e=>{if(e.target.dataset.column){columnFilters[e.target.dataset.column]=e.target.value;render();}});
    $('grid').querySelector('thead').addEventListener('click',e=>{const b=e.target.closest('[data-sort]');if(b){sort={key:b.dataset.sort,direction:sort?.key===b.dataset.sort?-sort.direction:1};render();}});
    $('search').addEventListener('input',render);$('closeDialog').addEventListener('click',()=>$('detailDialog').close());
    $('grid').addEventListener('click',e=>{const b=e.target.closest('button[data-act]');if(b)rowAction(b);else {document.querySelectorAll('#grid tr.selected').forEach(r=>r.classList.remove('selected'));e.target.closest('tbody tr')?.classList.add('selected');}});
    $('branchOptions')?.addEventListener('change',()=>busy(null,async()=>{
        caption();bindChoices({});
        const data=await request(api+'/lookups?branches='+encodeURIComponent(branchIds()));bindChoices(data);
        status(branchIds()?'Branch filters loaded.':'Select a branch to load its filters.');
    },'Loading branch filters…'));
    $('branchSearch')?.addEventListener('input',()=>{const text=value('branchSearch').toLowerCase();document.querySelectorAll('#branchOptions label').forEach(l=>l.hidden=!l.textContent.toLowerCase().includes(text));});
    document.addEventListener('click',e=>{if($('branchPicker')&&!$('branchPicker').contains(e.target))$('branchPicker').open=false;});
    $('dateType')?.addEventListener('change',()=>{
        const today=initial.today;let from=today;
        if(value('dateType')==='week'){const d=new Date(today+'T12:00:00');d.setDate(d.getDate()-7);from=d.getFullYear()+'-'+String(d.getMonth()+1).padStart(2,'0')+'-'+String(d.getDate()).padStart(2,'0');}
        if(value('dateType')==='month')from=today.slice(0,8)+'01';if(value('dateType')==='year')from=today.slice(0,4)+'-01-01';if(value('dateType')==='financial')from=initial.yearStart;
        $('fromDate').value=from;if(['month','year'].includes(value('dateType')))$('toDate').value=today;
    });
    document.querySelectorAll('button.print').forEach(b=>b.addEventListener('click',()=>busy(b,async()=>{
        if(!lastFilter||!rows.length)throw Error('Load the report before printing.');
        let f={...lastFilter},format='main';if(report==='stock-with-supplier')f.mode=b.dataset.format==='1'?'trial':'ledger';
        if(report==='purchase-order-register'&&b.dataset.format==='1')format='alternate';if(report==='inventory-transaction-report'&&b.dataset.format==='1')format='stock';
        await downloadPdf(api+'/print?format='+format,f);status('Print preview ready.');
    },'Preparing print preview…')));
    $('fullscreen').addEventListener('click',()=>busy($('fullscreen'),async()=>{if(document.fullscreenElement)await document.exitFullscreen();else await $('reportArea').requestFullscreen();status('Table view updated.');}));
    $('export').addEventListener('click',()=>busy($('export'),async()=>{
        if(!rows.length||!lastFilter)throw Error('Load the report before exporting.');
        const blob=await request(api+'/export',{filter:lastFilter,search:value('search'),columnFilters,sortKey:sort?.key||null,sortDirection:sort?.direction||1},'application/vnd.ms-excel');
        const url=URL.createObjectURL(blob),a=document.createElement('a');a.href=url;a.download=report+'.xls';a.click();setTimeout(()=>URL.revokeObjectURL(url),1000);status('Report exported.');
    }));
    $('shortcuts').addEventListener('click',()=>dialog('Shortcut Keys','<p>Ctrl+N — New / reset<br>Ctrl+R — Refresh filters<br>Ctrl+S — Show report<br>Ctrl+P — Print<br>Ctrl+L / Ctrl+T — Supplier Ledger / Trial<br>Ctrl+↑ or F5 — First date<br>Ctrl+↓ — Results table<br>Ctrl+E — Packing Material Reports<br>Enter — Next filter<br>Search dropdowns by typing; use ↑/↓ and Enter to select.</p>'));
    document.addEventListener('keydown',e=>{
        if(e.target.closest('.cx-combo-pop'))return;
        if(e.key==='Enter'&&e.target.closest('#filters')&&!['BUTTON','SUMMARY'].includes(e.target.tagName)) {
            e.preventDefault();const fields=Array.from(document.querySelectorAll('#filters input,#filters select,#filters button')).filter(x=>!x.disabled&&x.offsetParent!==null),i=fields.indexOf(e.target);fields[(i+1)%fields.length]?.focus();return;
        }
        if(e.altKey&&['1','2'].includes(e.key)){e.preventDefault();document.querySelectorAll('button.print')[Number(e.key)-1]?.click();return;}
        if(e.key==='Escape'&&!$('detailDialog').open){e.preventDefault();location.assign('/app/packing-material?module=2031');return;}
        if(e.key==='F5'){e.preventDefault();$('fromDate').focus();return;}
        if(e.ctrlKey&&e.code==='Space'&&e.target.closest('#grid')){e.preventDefault();if(e.target.matches('a,button'))e.target.click();return;}
        if(!e.ctrlKey)return;const key=e.key.toLowerCase();
        const actions={n:()=>$('new').click(),r:()=>$('refresh').click(),s:()=>$('show').click(),p:()=>document.querySelector('button.print').click(),l:()=>$('show').click(),t:()=>$('trial')?.click(),arrowup:()=>$('fromDate').focus(),arrowdown:()=>$('tableWrap').focus(),e:()=>location.assign('/app/packing-material?module=2031')};
        if(actions[key]){e.preventDefault();actions[key]();}
    });
    render();busy($('refresh'),()=>initialize());
})();
