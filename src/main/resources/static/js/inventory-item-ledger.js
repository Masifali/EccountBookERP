(() => {
    'use strict';
    const el=id=>document.getElementById(id),base='/api/inventory/item-ledger';
    // Captions/order from ItemEvaluationLedgerNew.resx; widths from GridSettings + InventoryConstants.
    const columns=[['TranDate','Date',100],['DocNo','No',60],['DocType','Type',70],['QtyIn','Bags',70,0,true],['WeightIn','Weight (Kg)',80,0,true],['RateIn','Rate',70,2],['RateUomIn','Rate Uom',65],['AmountIn','Amount',110,0,true],['QtyOut','Bags',70,0,true],['WeightOut','Weight (Kg)',80,0,true],['RateOut','Rate',70,2],['RateUomOut','Rate Uom',65],['AmountOut','Amount',110,0,true],['BalQty','Bags',70,0],['BalWeight','Weight (Kg)',80,0],['BalAmount','Amount',110,0],['AvgRate','Avg Rate',90,2],['PartyName','Party Name',170],['WareHouseName','WareHouse Name',150],['PackUom','Pack Uom',65],['PackingType','Packing Type',80],['CropYear','Crop Year',73],['JobLot','Job Lot',80],['StockHoldStatus','Stock Hold Status',130]];
    let data=null,lookups=null,shownRequest=null,selected=-1;
    const filters=new Map();
    function status(text,error=false){el('status').textContent=text;el('status').classList.toggle('error',error);}
    function format(value,places){if(places===undefined)return value??'';const s=ReportDecimal.format(value??'0',places,places===0);return places?s.replace(/(\.\d*?)0+$/,'$1').replace(/\.$/,''):s;}
    function display(row,column){const value=row[column[0]];if(column[0]==='TranDate'&&value){const parts=String(value).slice(0,10).split('-');if(parts.length===3)return `${parts[2]}-${['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][Number(parts[1])-1]}-${parts[0]}`;}return String(format(value,column[3]));}
    function visibleRows(){return (data?.rows||[]).filter(row=>columns.every(c=>!filters.get(c[0])||display(row,c).toLowerCase().includes(filters.get(c[0]))));}
    function cell(row,text,type='td'){const td=document.createElement(type);td.textContent=text;row.append(td);return td;}
    function render(){
        el('records').replaceChildren();el('totals').replaceChildren();const rows=visibleRows(),fragment=document.createDocumentFragment();
        rows.forEach((r,index)=>{const tr=document.createElement('tr');tr.tabIndex=-1;if(index===selected)tr.className='selected';columns.forEach(c=>{const td=cell(tr,display(r,c));if(c[3]!==undefined)td.className='number';td.title=td.textContent;});tr.addEventListener('click',()=>{selected=index;el('records').querySelectorAll('.selected').forEach(n=>n.classList.remove('selected'));tr.classList.add('selected');tr.focus();});fragment.append(tr);});el('records').append(fragment);
        const total=document.createElement('tr');columns.forEach(c=>{const td=cell(total,c[4]?format(ReportDecimal.sum(rows.map(r=>r[c[0]]??'0')),0):'');if(c[4])td.className='number';});el('totals').append(total);el('rowCount').textContent=`${rows.length} records`;
        el('GroupOpeningInfo').hidden=!data?.rows.length;
        if(data?.rows.length){el('summaryTitle').textContent=`Opening And Closing Info Of Item : (${data.rows[0].ItemName||$('#CmbItem option:selected').text()})`;for(const [side,suffix] of [['opening','Opening'],['closing','Closing']]){for(const [key,field] of [['BalQty','Qty'],['BalWeight','Weight'],['BalAmount','Amount'],['AvgRate','AvgRate']]){const id=key==='BalAmount'&&side==='closing'?'txtAmountclosing':`txt${field}${suffix}`;el(id).value=format(data[side][key],key==='AvgRate'?2:0);}}}
    }
    function dateChanged(){
        const type=el('cmbDateType').value,today=new Date(),from=new Date(today);
        if(!type)return;
        if(type==='2')from.setDate(from.getDate()-7);
        if(type==='3')from.setDate(1);
        if(type==='4')from.setMonth(0,1);
        if(type==='5'){const years=lookups?.financialYears||[];if(years.length!==1||!years[0].Start_Period){status('Select one active financial year before using Financial Year.',true);return;}el('fromdate').value=String(years[0].Start_Period).slice(0,10);return;}
        el('fromdate').value=ReportLoading.localDate(from);if(type==='3'||type==='4')el('todate').value=ReportLoading.localDate(today);
    }
    async function execute(button,label,action){try{return await InventoryRequest.execute(button,label,action);}catch(error){status(error.message,true);}}
    async function refresh(button=el('btnRefresh')){
        return execute(button,'Loading…',async()=>{
            for(const id of ['btnshow','gridPrint','gridExport'])ReportLoading.setDisabled(el(id),true);
            lookups=await InventoryRequest.json(base+'/lookups');
            const item=el('CmbItem');item.replaceChildren(new Option('',''));lookups.items.forEach(row=>item.add(new Option(row.name,row.Id)));$(item).val(null).trigger('change');
            ReportLoading.setDisabled(el('gridPrint'),!lookups.permissions['Grid Print']);
            ReportLoading.setDisabled(el('gridExport'),!lookups.permissions['Grid Export']);
            ReportLoading.setDisabled(el('btnshow'),false);
            status(`${lookups.items.length} items available`);
        });
    }
    async function show(){
        if(el('btnshow').disabled)return;
        if(!el('CmbItem').value){status('Item Field Required...',true);$('#CmbItem').select2('open');return;}
        if(!el('filters').reportValidity())return;
        const request={itemId:Number(el('CmbItem').value),fromDate:el('fromdate').value,toDate:el('todate').value};
        if(request.fromDate>request.toDate){status('From Date must be on or before To Date.',true);return;}
        await execute(el('btnshow'),'Loading…',async()=>{const result=await InventoryRequest.json(base,{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(request)});data=result;shownRequest=request;selected=-1;render();status(result.rows.length?'':'Record Not Found For Display');});
    }
    function reset(){if(el('btnNew').disabled)return;data=null;shownRequest=null;selected=-1;$('#CmbItem').val(null).trigger('change');filters.clear();document.querySelectorAll('.column-filter').forEach(input=>input.value='');render();status('');$('#cmbDateType').select2('focus');}
    async function output(action,button){
        if(!shownRequest||!data?.rows.length){status('Record Not Found For Display',true);return;}
        await execute(button,'Loading…',async()=>{
            // The backend independently checks the requested output right.
            await InventoryRequest.json(base+'/'+action,{method:'POST'});
            if(action==='grid-print'){window.print();return;}
            // Export the displayed filtered rows. Prefix formula-like cells before CSV quoting.
            const quote=v=>'"'+String(v).replace(/^[=+@-]/,"'$&").replace(/"/g,'""')+'"';
            const csv=[columns.map(c=>quote(c[1])).join(','),...visibleRows().map(row=>columns.map(c=>quote(display(row,c))).join(','))].join('\r\n');
            const url=URL.createObjectURL(new Blob(['\uFEFF'+csv],{type:'text/csv;charset=utf-8'})),link=document.createElement('a');link.href=url;link.download='Item-Stock-Ledger.csv';link.click();setTimeout(()=>URL.revokeObjectURL(url),1000);
        });
    }
    const group=document.createElement('tr');group.className='groups';for(const [title,count] of [['Transaction',3],['Receipt',5],['Consumption',5],['Balance',4],['Description',7]])cell(group,title,'th').colSpan=count;el('headers').append(group);
    const captions=document.createElement('tr'),filterRow=document.createElement('tr');filterRow.className='filters';
    for(const c of columns){const col=document.createElement('col');col.style.width=c[2]+'px';el('columnWidths').append(col);cell(captions,c[1],'th');const td=cell(filterRow,'','th'),input=document.createElement('input');input.className='column-filter';input.setAttribute('aria-label','Filter '+c[0]);input.addEventListener('input',()=>{filters.set(c[0],input.value.toLowerCase());selected=-1;render();});td.append(input);}
    el('grd').style.width=columns.reduce((sum,c)=>sum+c[2],0)+'px';el('headers').append(captions,filterRow);
    el('gridScroll').addEventListener('keydown',e=>{
        if(e.target.matches('input')||!['ArrowDown','ArrowUp','Home','End'].includes(e.key))return;
        const rows=Array.from(el('records').children);if(!rows.length)return;e.preventDefault();
        selected=e.key==='Home'?0:e.key==='End'?rows.length-1:Math.max(0,Math.min(rows.length-1,selected+(e.key==='ArrowDown'?1:-1)));
        rows.forEach((row,index)=>row.classList.toggle('selected',index===selected));rows[selected].focus({preventScroll:true});rows[selected].scrollIntoView({block:'nearest',inline:'nearest'});
    });
    $('#cmbDateType,#CmbItem').select2({width:'resolve',minimumResultsForSearch:0});$('#cmbDateType').on('change',dateChanged);
    el('todate').value=ReportLoading.localDate();dateChanged();
    el('filters').addEventListener('submit',e=>{e.preventDefault();show();});el('btnNew').addEventListener('click',reset);el('btnRefresh').addEventListener('click',()=>refresh());el('btnshortcut').addEventListener('click',()=>el('shortcuts').showModal());el('gridPrint').addEventListener('click',()=>output('grid-print',el('gridPrint')));el('gridExport').addEventListener('click',()=>output('grid-export',el('gridExport')));
    document.addEventListener('keydown',e=>{const key=e.key.toLowerCase();if(el('shortcuts').open)return;if(e.ctrlKey&&['s','n','r','e','f5','arrowup','arrowdown'].includes(key)){e.preventDefault();if(key==='s')show();else if(key==='n')reset();else if(key==='r')refresh();else if(key==='e')location.href='/inventory/dashboard';else if(key==='arrowdown')el('gridScroll').focus();else $('#cmbDateType').select2('focus');}else if(e.key==='Escape'){location.href='/inventory/dashboard';}else if(e.ctrlKey&&e.altKey){e.preventDefault();el('shortcuts').showModal();}});
    refresh().then(()=>{const query=new URLSearchParams(location.search),item=query.get('itemId');if(item&&/^\d+$/.test(item)&&[...el('CmbItem').options].some(option=>option.value===item)){$('#CmbItem').val(item).trigger('change.select2');for(const [parameter,control]of [['fromDate','fromdate'],['toDate','todate']]){const date=query.get(parameter);if(date&&/^\d{4}-\d{2}-\d{2}$/.test(date))el(control).value=date;}status('Item and dates loaded from the stock report.');setTimeout(()=>show(),0);}});render();
})();
