/* frmOpeningStockBlancing: event port. No simulated persistence or generated history. */

(() => {

 'use strict';

 const $id=id=>document.getElementById(id),api='/api/inventory/opening-stock';

 let recordId=0,permissions={},history=[],selected=-1,uoms=[],itemLoaded='',busy=false,pendingFiles=[],savedAttachments=[],removedAttachments=[],attachmentOwner=0,pendingUom=false;

 const numberIds=['projectsId','warehouseId','itemId','itemUomSch','jobLotId','packingTypeId','stockCrGLAcId','rateUomSch'];

 const decimalIds=['qty','weightKgs','itemRate','itemAmount'];

 const defaultColumns=[['DocNo','Doc No',60],['DocDate','Doc Date',73],['ItemName','Item',170],['PackUom','Item UOM',60],['CropYear','Crop',73],['JobLotDescription','Job Lot',80],['WareHouseName','Warehouse',150],['PackTypeDesc','Packing Type',115],['AccountTitle','Stock Credit Account',150],['Qty','Qty',70],['WeightKgs','Weight',80],['ItemRate','Rate',70],['RateUom','Rate UOM',60],['ItemAmount','Amount',90],['Remarks','Remarks',100],['IssueWeight','Issue Weight',80],['EntryDate','Entry Date',145],['EntryUser','Entry User',100],['ModifyDate','Modify Date',145],['ModifyUser','Modify User',100],['NoOfAttachments','Attachments',90]];

 let columns=defaultColumns.map(c=>[...c]),hiddenColumns=new Set(),layoutLoaded=false,shownFilters=null;

 const aliases={ItemName:'Item',PackUom:'ItemUOM',CropYear:'Crop',JobLotDescription:'JobLot',WareHouseName:'Warehouse',PackTypeDesc:'PackingType',AccountTitle:'StockCreditAccount',WeightKgs:'Weight',ItemRate:'Rate',RateUom:'RateUOM',ItemAmount:'Amount'},layoutApi='/grid-layout';

 function cell(tr,value,tag='td'){const td=document.createElement(tag);td.textContent=value??'';tr.append(td);return td;}

 function message(text,error=false){const el=$id('message');el.hidden=!text;el.textContent=text;el.classList.toggle('error',error);}

 async function run(button,label,action){if(busy||button?.disabled)return;busy=true;try{return await InventoryRequest.execute(button,label,action);}catch(e){message(e.message,true);}finally{busy=false;if(pendingUom){pendingUom=false;queueUoms();}}}

 function json(path,body){const headers={};const token=document.querySelector('meta[name=_csrf]')?.content,header=document.querySelector('meta[name=_csrf_header]')?.content;if(token&&header)headers[header]=token;if(body!==undefined)headers['Content-Type']='application/json';return InventoryRequest.json(api+path,{headers,...(body===undefined?{}:{method:'POST',body:JSON.stringify(body)})});}

 function set(id,value){if(['docDate','fromDate','toDate'].includes(id)&&value){const raw=String(value).slice(0,10);if(/^\d{4}-\d{2}-\d{2}$/.test(raw)){const parts=raw.split('-');value=parts[2]+'-'+['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][Number(parts[1])-1]+'-'+parts[0].slice(-2);}}$id(id).value=value??'';if($id(id).tagName==='SELECT')window.jQuery($id(id)).trigger('change.select2');}

 function options(id,rows,text,first=false,valueKey='Id'){const el=$id(id),old=el.value;el.replaceChildren(new Option('',''));for(const row of rows)el.add(new Option(row[text]??'',row[valueKey]??''));el.value=Array.from(el.options).some(o=>o.value===old)?old:'';if(first&&!el.value&&rows.length)el.selectedIndex=1;window.jQuery(el).trigger('change.select2');}

 function bindHistory(rows){options('historyItem',rows.filter(r=>r.Activity==='Item'),'ReferenceName');options('historyAccount',rows.filter(r=>r.Activity==='StockAccount'),'ReferenceName');options('historyStockAccount',rows.filter(r=>r.Activity==='ItemStockAccount'),'ReferenceName');}

 function applyMode(){for(const [id,right] of [['grid-print','Grid Print'],['grid-export','Grid Export']])ReportLoading.setDisabled($id(id),!permissions[right]);for(const name of ['save','update'])ReportLoading.setDisabled($id(name),!permissions[name==='save'?'Save':'Update']);$id('save').hidden=recordId>0;$id('update').hidden=!recordId;}

 async function refresh(initial=false){const d=await json('/lookups');permissions=d.permissions;options('projectsId',d.projects,'ProjectName',true);options('warehouseId',d.warehouses,'WareHouseName');options('itemId',d.items,'ItemName');options('packingTypeId',d.packingTypes,'PackTypeDesc');options('cropYear',d.crops,'CropYear',false,'CropYear');options('jobLotId',d.lots,'JobLotDescription');options('stockCrGLAcId',d.accounts,'AccountTitle');bindHistory(d.historyChoices);if(initial){set('docNo',d.defaults.docNo);set('docDate',d.defaults.docDate);}set('transactionType','Stocks');applyMode();}

 async function reset(){const d=await json('/defaults');recordId=0;pendingFiles=[];savedAttachments=[];removedAttachments=[];attachmentOwner=0;itemLoaded='';uoms=[];for(const id of ['itemId','itemUomSch','rateUomSch',...decimalIds])set(id,'');options('itemUomSch',[],'UOMCode');options('rateUomSch',[],'UOMCode');for(const id of ['warehouseId','itemId','itemUomSch','jobLotId','cropYear','packingTypeId']){$id(id).disabled=false;window.jQuery($id(id)).trigger('change.select2');}set('docNo',d.docNo);set('docDate',d.docDate);applyMode();$id('warehouseId').focus();}

 function numeric(id){return Number($id(id).value.replaceAll(',',''))||0;}

 function formatted(n){return Number.isFinite(n)?n.toLocaleString('en-US',{maximumFractionDigits:3}):'0';}

 function equivalent(id){return Number(uoms.find(r=>String(r.Id)===$id(id).value)?.Equivalent)||0;}

 function amount(){const eq=equivalent('rateUomSch');set('itemAmount',formatted(eq?numeric('itemRate')*numeric('weightKgs')/eq:0));}

 function weight(){set('weightKgs',formatted(numeric('qty')*equivalent('itemUomSch')));amount();}

 async function loadUoms(force=false){const item=$id('itemId').value;if(!force&&itemLoaded===item)return;const oldPack=$id('itemUomSch').selectedOptions[0]?.text,oldRate=$id('rateUomSch').selectedOptions[0]?.text;const loaded=item?await json('/uoms/'+encodeURIComponent(item)):[];if(item!==$id('itemId').value){pendingUom=true;return;}uoms=loaded;itemLoaded=item;options('itemUomSch',uoms,'UOMCode');options('rateUomSch',uoms,'UOMCode');for(const [id,text]of[['itemUomSch',oldPack],['rateUomSch',oldRate]])set(id,uoms.find(r=>r.UOMCode===text)?.Id??'');weight();}

 function validate(){for(const [id,label]of [['projectsId','Project'],['docNo','document Number'],['warehouseId','WareHouseName'],['itemId','Item'],['itemUomSch','UOM'],['cropYear','CropYear'],['jobLotId','JobLot'],['stockCrGLAcId','Stock Credit Account'],['qty','Quantity'],['weightKgs','Weight'],['itemRate','Rate'],['rateUomSch','Rate UOM'],['itemAmount','Amount'],['packingTypeId','PackingType']]){if(!$id(id).value||(id!=='cropYear'&&!numeric(id))){$id(id).focus();throw Error(label+' Field Required');}}}

 async function save(){validate();const r={id:recordId};for(const id of numberIds)r[id]=Number($id(id).value)||0;for(const id of decimalIds)r[id]=$id(id).value.replaceAll(',','')||'0';for(const id of ['cropYear','remarks','transactionType'])r[id]=$id(id).value;const updating=recordId>0;if(!confirm(updating?'Are you sure to Update?':'Are you sure to Save?'))return;r.files=await Promise.all(pendingFiles.map(encodeFile));r.removeAttachmentIds=removedAttachments;const saved=await json('',r);await reset();message((updating?'Record Update Successfully':'Record Save Successfully')+' — Document '+saved.DocNo);}

 async function load(id){const r=await json('/'+id);recordId=Number(r.Id);set('docNo',r.DocNo);set('docDate',String(r.DocDate).slice(0,10));for(const field of numberIds){if(['itemUomSch','rateUomSch'].includes(field))continue;set(field,r[field[0].toUpperCase()+field.slice(1)]);}await loadUoms(true);for(const field of ['itemUomSch','rateUomSch',...decimalIds,'cropYear','remarks','transactionType'])set(field,r[field[0].toUpperCase()+field.slice(1)]);for(const field of ['warehouseId','itemId','itemUomSch','jobLotId','cropYear','packingTypeId']){$id(field).disabled=Number(r.IssueWeight)>0;window.jQuery($id(field)).trigger('change.select2');}pendingFiles=[];removedAttachments=[];savedAttachments=await json('/'+recordId+'/attachments');attachmentOwner=recordId;applyMode();message('Document '+r.DocNo+' loaded');}

 function date(value,time=false){if(!value)return '';const d=new Date(value);if(Number.isNaN(d.getTime()))return String(value);return (time?d.toLocaleDateString('en-GB',{day:'2-digit',month:'short',year:'numeric'}).replaceAll(' ','-'):d.toLocaleDateString('en-GB'))+(time?' '+d.toLocaleTimeString('en-US',{hour:'2-digit',minute:'2-digit'}):'');}

 function render(){const shownColumns=columns.filter(c=>!hiddenColumns.has(c[0]));const head=$id('history').tHead,body=$id('history').tBodies[0];head.replaceChildren();body.replaceChildren();const h=head.insertRow();for(const [key,label,width]of [['edit','Edit',40],...shownColumns]){const th=document.createElement('th');th.textContent=label;th.style.width=width+'px';th.style.minWidth=width+'px';h.append(th);}history.forEach((r,index)=>{const tr=body.insertRow();tr.tabIndex=0;tr.addEventListener('click',()=>select(index));tr.addEventListener('dblclick',()=>run(null,'Loading…',()=>load(r.Id)));tr.addEventListener('keydown',e=>{if(e.ctrlKey&&(e.key==='Enter'||e.key===' ')){e.preventDefault();run(null,'Loading…',()=>load(r.Id));}else if(['ArrowDown','ArrowUp'].includes(e.key)){e.preventDefault();select(Math.max(0,Math.min(history.length-1,index+(e.key==='ArrowDown'?1:-1))));body.rows[selected]?.focus();}});const edit=document.createElement('button');edit.textContent='Edit';edit.addEventListener('click',()=>run(edit,'…',()=>load(r.Id)));tr.insertCell().append(edit);for(const [key]of shownColumns){const td=tr.insertCell();if(key==='DocNo'){const link=document.createElement('a');link.href=location.pathname+'?id='+encodeURIComponent(r.Id);link.textContent=r.DocNo;link.addEventListener('click',e=>{e.preventDefault();run(null,'Loading…',()=>load(r.Id));});td.append(link);}else if(key==='NoOfAttachments'){const link=document.createElement('button');link.textContent=r[key];link.addEventListener('click',()=>run(link,'…',()=>attachments(r.Id)));td.append(link);}else if(key.endsWith('Date'))td.textContent=date(r[key],key!=='DocDate');else{td.textContent=r[key]??'';if(['Qty','WeightKgs','ItemRate','ItemAmount','IssueWeight'].includes(key)){td.className='numeric';td.textContent=formatted(Number(r[key]));}}}});const filterRow=head.insertRow();filterRow.className='column-filters';filterRow.insertCell();shownColumns.forEach(([key,label],i)=>{const input=document.createElement('input');input.setAttribute('aria-label','Filter '+label);input.addEventListener('input',()=>filterRows());filterRow.insertCell().append(input);});$id('row-count').textContent=history.length+' record(s)';}

 function encodeFile(file){return new Promise((resolve,reject)=>{const reader=new FileReader();reader.onerror=()=>reject(Error('Could not read '+file.name));reader.onload=()=>resolve({name:file.name,base64:String(reader.result).split(',')[1]});reader.readAsDataURL(file);});}

 async function downloadAttachment(id,name){const headers={},token=document.querySelector('meta[name=_csrf]')?.content,key=document.querySelector('meta[name=_csrf_header]')?.content;if(token&&key)headers[key]=token;const response=await fetch(api+'/'+attachmentOwner+'/attachments/'+id,{headers});if(!response.ok||response.redirected){if((response.headers.get('content-type')||'').includes('json'))throw Error((await response.json()).message||'Attachment could not be downloaded');throw Error('Attachment could not be downloaded. Check that you are signed in.');}const url=URL.createObjectURL(await response.blob()),link=document.createElement('a');link.href=url;link.download=String(name).replaceAll('\\','/').split('/').pop();link.click();setTimeout(()=>URL.revokeObjectURL(url),1000);}

 function attachmentList(rows=savedAttachments){const list=$id('attachment-list');list.replaceChildren();const editable=attachmentOwner===recordId&&Boolean(permissions[recordId?'Update':'Save']);$id('attachment-files-label').hidden=!editable;for(const row of rows.filter(r=>attachmentOwner!==recordId||!removedAttachments.includes(Number(r.Id)))){const p=document.createElement('p'),open=document.createElement('button');open.textContent=row.Attachment||row.UploadedFileCustomName;open.onclick=()=>run(open,'Downloading…',()=>downloadAttachment(row.Id,open.textContent));p.append(open,document.createTextNode(' — '+date(row.EntryDate,true)));if(editable){const remove=document.createElement('button');remove.textContent='Remove';remove.onclick=()=>{removedAttachments.push(Number(row.Id));attachmentList(rows);};p.append(remove);}list.append(p);}if(attachmentOwner===recordId)pendingFiles.forEach((file,index)=>{const p=document.createElement('p'),label=document.createElement('span'),remove=document.createElement('button');label.textContent=file.name+' (ready to save)';remove.textContent='Remove';remove.onclick=()=>{pendingFiles.splice(index,1);attachmentList();};p.append(label,remove);list.append(p);});if(!list.childElementCount)list.textContent='No attachments';}

 async function attachments(id){attachmentOwner=id;attachmentList(id===recordId?savedAttachments:id?await json('/'+id+'/attachments'):[]);$id('attachment-dialog').showModal();}

 $id('attachment-files').onchange=()=>{const incoming=[...$id('attachment-files').files];$id('attachment-files').value='';if(incoming.some(f=>!f.size||f.size>5*1024*1024)){message('Each attachment must be between 1 byte and 5 MB.',true);return;}if(pendingFiles.length+incoming.length>10){message('At most ten attachments may be added at once.',true);return;}pendingFiles.push(...incoming);attachmentList();}; function filterRows(){const inputs=Array.from($id('history').tHead.querySelectorAll('input'));let count=0;for(const row of $id('history').tBodies[0].rows){row.hidden=!inputs.every((input,i)=>!input.value||row.cells[i+1].textContent.toLocaleLowerCase().includes(input.value.toLocaleLowerCase()));if(!row.hidden)count++;}$id('row-count').textContent=count+' / '+history.length+' record(s)';}

 function select(index){selected=index;Array.from($id('history').tBodies[0].rows).forEach((r,i)=>r.classList.toggle('selected',i===index));}

 async function show(){if(!layoutLoaded){applyLayout(await json(layoutApi));layoutLoaded=true;}const filter={};for(const id of ['fromDate','toDate'])if($id(id==='fromDate'?'useFromDate':'useToDate').checked&&$id(id).value)filter[id]=window.jQuery.datepicker.formatDate('yy-mm-dd',window.jQuery.datepicker.parseDate('dd-M-y',$id(id).value));for(const [id,key]of [['fromDocNo','fromDocNo'],['toDocNo','toDocNo'],['historyItem','itemId'],['historyAccount','accountId'],['historyStockAccount','itemStockAccountId']])if($id(id).value)filter[key]=Number($id(id).value);history=await json('/history',filter);shownFilters={...filter};selected=-1;render();message('');}

 for(const [id,fn,label]of [['new',reset,'Resetting…'],['refresh',()=>refresh(),'Refreshing…'],['save',save,'Saving…'],['update',save,'Updating…'],['show',show,'Loading…'],['history-refresh',async()=>bindHistory(await json('/history-choices')),'Refreshing…']])$id(id).addEventListener('click',()=>run($id(id),label,fn));

 function outputRows(){const rows=Array.from($id('history').tBodies[0].rows).map((row,index)=>({row,data:history[index]})).filter(r=>!r.row.hidden);if(!shownFilters||!rows.length)throw Error('Show history records before printing or exporting');if(!columns.some(c=>!hiddenColumns.has(c[0])))throw Error('Choose at least one visible history column');return rows;}

 $id('grid-export').onclick=()=>run($id('grid-export'),'Exporting…',async()=>{const rows=outputRows(),headers={'Content-Type':'application/json'},token=document.querySelector('meta[name=_csrf]')?.content,key=document.querySelector('meta[name=_csrf_header]')?.content;if(token&&key)headers[key]=token;const response=await fetch(api+'/grid-export',{method:'POST',headers,body:JSON.stringify({filters:shownFilters,ids:rows.map(r=>Number(r.data.Id)),columns:columns.filter(c=>!hiddenColumns.has(c[0])).map(c=>c[0])})});if(!response.ok||response.redirected){if((response.headers.get('content-type')||'').includes('json'))throw Error((await response.json()).message||'Export failed');throw Error('Export failed. Check that you are signed in.');}const url=URL.createObjectURL(await response.blob()),link=document.createElement('a');link.href=url;link.download='Opening-Stock-History.xls';link.click();setTimeout(()=>URL.revokeObjectURL(url),1000);});

 $id('grid-print').onclick=()=>run($id('grid-print'),'Preparing print…',async()=>{const rows=outputRows();await json('/grid-print',{});const sheet=$id('grid-print-sheet'),title=document.createElement('h2'),table=document.createElement('table');sheet.replaceChildren();title.textContent='Opening Stock Balancing History';sheet.append(title,table);const head=table.createTHead().insertRow();columns.filter(c=>!hiddenColumns.has(c[0])).forEach(c=>cell(head,c[1],'th'));const body=table.createTBody();for(const {row}of rows){const tr=body.insertRow();Array.from(row.cells).slice(1).forEach(td=>cell(tr,td.textContent));}try{window.print();}finally{sheet.replaceChildren();}});

 $id('history-reset').addEventListener('click',()=>{set('historyItem','');set('historyAccount','');history=[];render();});

 window.jQuery('select:not([hidden])').select2({width:'100%',allowClear:true,placeholder:''});

 function queueUoms(force=false){if(busy){if(itemLoaded!==$id('itemId').value)pendingUom=true;return;}run(null,'Loading units…',()=>loadUoms(force));}

 window.jQuery('#itemId').on('change',()=>queueUoms()).on('select2:close',()=>{if(!busy)queueUoms(true);});$id('itemId').addEventListener('blur',()=>queueUoms(true));

 window.jQuery('#itemUomSch').on('change',weight);window.jQuery('#rateUomSch').on('change',amount);$id('qty').addEventListener('input',weight);for(const id of ['itemRate','weightKgs'])$id(id).addEventListener('input',amount);

 for(const id of ['qty','itemRate','weightKgs'])$id(id).addEventListener('keypress',e=>{if(e.ctrlKey||e.metaKey||e.key.length!==1)return;if(!/^[0-9.]$/.test(e.key)||e.key==='.'&&$id(id).value.includes('.'))e.preventDefault();});

 $id('opening').addEventListener('submit',e=>e.preventDefault());

 function applyLayout(settings){const saved=new Map(settings.map(c=>[c.key,c]));columns=defaultColumns.map(c=>[...c]);hiddenColumns=new Set();for(const c of columns){const value=saved.get(aliases[c[0]]??c[0]);if(value){c[2]=Math.max(24,Math.min(2000,value.width));if(value.visible)hiddenColumns.delete(c[0]);else hiddenColumns.add(c[0]);}}columns.sort((a,b)=>(saved.get(aliases[a[0]]??a[0])?.position??defaultColumns.findIndex(c=>c[0]===a[0]))-(saved.get(aliases[b[0]]??b[0])?.position??defaultColumns.findIndex(c=>c[0]===b[0])));}

 function choose(){const body=$id('grid-fields').tBodies[0];body.replaceChildren();for(const [index,c]of columns.entries()){const tr=body.insertRow(),check=document.createElement('input');check.type='checkbox';check.checked=!hiddenColumns.has(c[0]);check.setAttribute('aria-label','Show '+c[1]);check.onchange=()=>{if(check.checked)hiddenColumns.delete(c[0]);else hiddenColumns.add(c[0]);render();};cell(tr,'').append(check);cell(tr,c[1]);const width=document.createElement('input');width.type='number';width.min=24;width.max=2000;width.value=c[2];width.setAttribute('aria-label','Width '+c[1]);width.oninput=()=>{const value=Number(width.value);if(value>=24&&value<=2000){c[2]=value;render();}};width.onchange=()=>{c[2]=Math.max(24,Math.min(2000,Number(width.value)||100));width.value=c[2];render();};cell(tr,'').append(width);const position=cell(tr,'');for(const [label,delta]of [['↑',-1],['↓',1]]){const button=document.createElement('button');button.textContent=label;button.setAttribute('aria-label',(delta<0?'Move up ':'Move down ')+c[1]);button.disabled=index+delta<0||index+delta>=columns.length;button.onclick=()=>{const other=index+delta;[columns[index],columns[other]]=[columns[other],columns[index]];choose();render();};position.append(button);}}}

 $id('grid-choose').onclick=()=>run($id('grid-choose'),'Loading layout…',async()=>{if(!layoutLoaded){applyLayout(await json(layoutApi));layoutLoaded=true;render();}choose();$id('grid-dialog').showModal();});$id('grid-close').onclick=()=>$id('grid-dialog').close();$id('grid-save').onclick=()=>run($id('grid-save'),'Saving layout…',async()=>{applyLayout(await json(layoutApi,columns.map(([key,,width],position)=>({key:aliases[key]??key,width,position,visible:!hiddenColumns.has(key)}))));render();$id('grid-dialog').close();message('Layout Saved successfully');});$id('grid-remove').onclick=()=>run($id('grid-remove'),'Removing layout…',async()=>{await json(layoutApi+'/remove',{});applyLayout([]);render();choose();message('Layout Removed successfully');});

 $id('shortcuts').onclick=()=>$id('keys-dialog').showModal();$id('close-keys').onclick=()=>$id('keys-dialog').close();

 $id('attachments').onclick=()=>run($id('attachments'),'Loading…',()=>attachments(recordId));$id('close-attachments').onclick=()=>$id('attachment-dialog').close();

 document.addEventListener('keydown',e=>{

  if(e.target.closest('dialog')||document.querySelector('.select2-container--open'))return;

  if(e.ctrlKey&&e.altKey){e.preventDefault();$id('shortcuts').click();return;}

  if(e.ctrlKey&&e.key==='F10'){e.preventDefault();$id('attachments').click();return;}

  if(e.ctrlKey&&(e.key==='F5'||e.key==='ArrowUp')){e.preventDefault();$id('warehouseId').nextElementSibling?.querySelector('[tabindex]')?.focus();return;}

  if(e.ctrlKey&&e.key.toLowerCase()==='p'){e.preventDefault();if(permissions.Print)$id('print').click();return;}

  if((e.ctrlKey&&e.key.toLowerCase()==='e')||e.key==='Escape'){e.preventDefault();if(!busy)location.assign('/inventory');}

 });

 document.addEventListener('keydown',e=>{if(e.target.closest('dialog')||document.querySelector('.select2-container--open'))return;if(e.ctrlKey){const actions={n:'new',r:'refresh',s:recordId?null:'save',u:recordId?'update':null};if(actions[e.key.toLowerCase()]){e.preventDefault();$id(actions[e.key.toLowerCase()]).click();}if(e.key==='ArrowDown'){e.preventDefault();$id('history').tBodies[0].rows[0]?.focus();}}else if(e.key==='Enter'&&!e.target.closest('table,dialog')&&e.target.tagName!=='BUTTON'&&!e.target.classList.contains('select2-search__field')){e.preventDefault();const fields=Array.from(document.querySelectorAll('#opening input:not([readonly]):not(:disabled),#opening select:not([hidden]):not(:disabled),#opening textarea'));fields[(fields.indexOf(e.target)+1)%fields.length]?.focus();}});

 window.jQuery('#fromDate,#toDate').datepicker({dateFormat:'dd-M-y',changeMonth:true,changeYear:true});

 const today=new Date(),past=new Date();past.setDate(past.getDate()-15);const iso=d=>`${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`;set('fromDate',iso(past));set('toDate',iso(today));

 run(null,'Loading…',async()=>{await refresh(true);render();const id=new URLSearchParams(location.search).get('id');if(id&&/^\d+$/.test(id))await load(id);});

})();

