/* Stock Evaluation Report Vehicle Wise - desktop Architecture.WinApp.Inventory_Stocks_Report.StockEvaluationReportVehicleWise
   (ScreenDefinition 292). Same conventions as inventory-vehicle-transactions.js (TransactionReportVehicleWise). */
(() => {
    'use strict';
    const el=id=>document.getElementById(id),base='/api/inventory/stock-evaluation-vehicle-wise';
    /* StockComboFill: one result set split on ActivityType, as the desktop does. */
    const lookupGroups={CmbItemParentCategory:'ParentCategories',cmbItemType:'ItemTypes',cmbItemCategory:'ItemCategories',CmbClassGroup:'ItemClassGroup',CmbItemName:'Items',CmbJobLot:'JobLot',CmbCropYear:'CropYear',CmbWarehouse:'Warehouse',CmbSupplier:'Supplier_Customer'};
    let rows=[],columns=[],lookups=null,selected=-1,uomVersion=0,uomWork=Promise.resolve(),lastCount=0,lastPrintArgs={},checkedRows=new Set();
    let amountPlaces=0,ratePlaces=2,holdEnabled=false,rateDenied=false;
    const filters=new Map();
    const status=(text,error=false)=>{el('status').textContent=text;el('status').classList.toggle('error',error);};
    const message=text=>window.alert(text);

    /* ConfigureNumericalColumn: "Amount" -> stringFormatsingle + Sum; "Rate"/"Aflatoxins" -> DecimalRateFormate; other doubles -> #,##0.## + Sum. */
    function buildColumns(){
        const list=[];
        if(holdEnabled)list.push({key:'Select',caption:'',width:25,kind:'chk'});
        list.push({key:'TranDetail',caption:'TranDetail',width:70,kind:'btn'});
        const add=(key,width,kind,caption)=>list.push({key,caption:caption||key,width,kind:kind||'text'});
        add('RefDocumentType',100);add('DocDate',73,'date');add('DocNo',60);add('ManualNo',60);add('JobOrderNo',80);
        add('PartyName',130);add('VehicleNo',80);add('BiltyNo',60);add('GpNo',45);add('Aflatoxins',80,'rate');
        add('Warehouse',150);add('ItemName',150);add('CropBatch',73);add('JobLot',80);add('PackingType',115);add('PackUom',60);
        add('QtyIn',70,'qty');add('QtyOut',70,'qty');add('BalQty',70,'qty');add('WeightIn',80,'qty');add('WeightOut',80,'qty');add('BalWeight',80,'qty');
        if(!rateDenied){add('RateUom',60);add('AvgRate',70,'rate');add('AmountIn',90,'amount');add('AmountOut',90,'amount');add('BalAmount',90,'amount');}
        add('StepDescription',130,'text','Last Fumigation Detail');
        return list;
    }
    const trim=v=>v.replace(/(\.\d*?)0+$/,'$1').replace(/\.$/,'');
    function num(value,kind){
        if(value===null||value===undefined||value==='')return '';
        if(kind==='qty')return trim(ReportDecimal.format(value,2,false));
        if(kind==='amount')return ReportDecimal.format(value,amountPlaces,false);
        if(kind==='rate')return ReportDecimal.format(value,ratePlaces,false);
        return String(value);
    }
    function dateText(value){if(!value)return '';const p=String(value).slice(0,10).split('-');return p.length===3?`${p[2]}-${p[1]}-${p[0]}`:String(value);}
    function display(row,c){const value=row[c.key];if(c.kind==='qty'||c.kind==='amount'||c.kind==='rate')return num(value,c.kind);if(c.kind==='date')return dateText(value);if(c.kind==='btn'||c.kind==='chk')return '';return String(value??'');}
    const isNumber=c=>c.kind==='qty'||c.kind==='amount'||c.kind==='rate';
    const hasTotal=c=>c.kind==='qty'||c.kind==='amount';
    const visible=()=>rows.map((row,index)=>({row,index})).filter(({row})=>columns.every(c=>!filters.get(c.key)||display(row,c).toLowerCase().includes(filters.get(c.key))));
    function cell(tr,value,tag='td'){const node=document.createElement(tag);node.textContent=value;tr.append(node);return node;}
    function selectRow(index){const trs=Array.from(el('records').children);if(!trs.length)return;selected=Math.max(0,Math.min(trs.length-1,index));trs.forEach((tr,i)=>tr.classList.toggle('selected',i===selected));trs[selected].focus({preventScroll:true});trs[selected].scrollIntoView({block:'nearest',inline:'nearest'});el('rowCount').textContent=`${selected+1} of ${trs.length}`;}

    /* grd.RetrieveStructure + grdSettings, or ClearStructure when the procedure returned nothing. */
    function structure(){
        el('columnWidths').replaceChildren();el('headers').replaceChildren();filters.clear();
        if(!rows.length){columns=[];el('grd').style.width='';return;}
        columns=buildColumns();
        const header=document.createElement('tr'),filter=document.createElement('tr');filter.className='column-filters';
        columns.forEach((c,i)=>{
            const col=document.createElement('col');col.style.width=c.width+'px';el('columnWidths').append(col);
            const th=cell(header,c.caption,'th');th.dataset.col=c.key;
            const ft=cell(filter,'','th');
            if(c.kind==='chk'){const all=document.createElement('input');all.type='checkbox';all.id='selectAll';all.setAttribute('aria-label','Select all rows');all.addEventListener('change',()=>{for(const v of visible()){if(all.checked)checkedRows.add(v.index);else checkedRows.delete(v.index);}render();});th.textContent='';th.append(all);}
            else if(c.kind!=='btn'){const input=document.createElement('input');input.setAttribute('aria-label','Filter '+c.caption);input.addEventListener('input',()=>{filters.set(c.key,input.value.toLowerCase());render();});ft.append(input);}
            /* grd.FrozenColumns = 1. */
            if(i===0){th.classList.add('frozen');ft.classList.add('frozen');}
        });
        el('headers').append(header,filter);
        el('grd').style.width=columns.reduce((s,c)=>s+c.width,0)+'px';
    }
    function render(){
        const data=visible();el('records').replaceChildren();el('totals').replaceChildren();
        const fragment=document.createDocumentFragment();
        data.forEach(({row,index},position)=>{
            const tr=document.createElement('tr');tr.tabIndex=-1;
            columns.forEach((c,i)=>{
                const td=cell(tr,display(row,c));
                if(c.kind==='btn'){const b=document.createElement('button');b.type='button';b.className='row-button';b.textContent='TranDetail';b.addEventListener('click',e=>{e.stopPropagation();selectRow(position);tranDetail(row);});td.append(b);}
                else if(c.kind==='chk'){td.className='center';const box=document.createElement('input');box.type='checkbox';box.checked=checkedRows.has(index);box.setAttribute('aria-label','Select row');box.addEventListener('change',()=>{if(box.checked)checkedRows.add(index);else checkedRows.delete(index);});td.append(box);}
                else{td.title=td.textContent;if(isNumber(c))td.className='number';}
                if(i===0)td.classList.add('frozen');
            });
            tr.addEventListener('click',()=>selectRow(position));fragment.append(tr);
        });
        el('records').append(fragment);
        if(columns.length){
            const total=document.createElement('tr');
            columns.forEach((c,i)=>{const td=cell(total,hasTotal(c)?num(ReportDecimal.sum(data.map(v=>v.row[c.key]??'0')),c.kind):'');if(hasTotal(c))td.className='number';if(i===0)td.classList.add('frozen');});
            el('totals').append(total);
        }
        el('rowCount').textContent=`${data.length} records`;selected=-1;
    }

    function bind(id,options,key='Id',label='name'){const select=el(id);select.replaceChildren();if(!select.multiple)select.add(new Option('',''));options.forEach(row=>select.add(new Option(row[label]??'',row[key]??'')));$(select).val(select.multiple?[]:null).trigger('change.select2');}
    async function execute(button,label,action){try{return await InventoryRequest.execute(button,label,action);}catch(error){status(error.message,true);message(error.message);}}

    /* Form Load (and btnRefresh: the same three fills, dates untouched). */
    async function refresh(initial=false){await execute(el('btnRefresh'),'Loading…',async()=>{
        el('BtnSearch').disabled=true;
        lookups=await InventoryRequest.json(base+'/lookups');
        for(const [id,activity]of Object.entries(lookupGroups))bind(id,lookups.stock.filter(r=>r.ActivityType===activity));
        if(lookups.packingTypes.length)bind('CmbPackingType',lookups.packingTypes,'Id','PackTypeDesc');
        if(lookups.documentTypes.length)bind('CmbReferenceDocument',lookups.documentTypes,'RefDocumentTypeId','DocumentTypeDescription');
        amountPlaces=Number(lookups.amountDecimals)||0;ratePlaces=Number(lookups.rateDecimals)||2;
        holdEnabled=!!lookups.stockReleaseFromFumigation;el('btnStockHold').hidden=!holdEnabled;
        if(initial){el('FromDate').value=lookups.financialYearStart||'';el('Todate').value=ReportLoading.localDate();}
        el('BtnSearch').disabled=false;status('');
    });}
    function reset(){
        /* Reset_Click: Class Group, Item Category, Item Type and Skip Zero are not cleared; the grid is kept. */
        uomVersion++;el('FromDate').focus();el('FromDate').value=lookups?.financialYearStart||'';el('Todate').value=ReportLoading.localDate();
        for(const id of ['CmbItemParentCategory','CmbItemName','CmbSupplier','CmbWarehouse','CmbJobLot','CmbCropYear','CmbReferenceDocument','CmbPackingType','CmbUom'])$('#'+id).val(el(id).multiple?[]:null).trigger('change.select2');
        status('');
    }
    /* CmbItemName_Leave -> UOMFill: the list is rebound only when the item has UOM rows. */
    function loadUoms(){const version=++uomVersion,item=Number(el('CmbItemName').value)||0;uomWork=execute(null,'Loading…',async()=>{const data=await InventoryRequest.json(base+'/uoms?itemId='+item);if(version!==uomVersion)return;if(data.length)bind('CmbUom',data,'Id','Equivalent');});return uomWork;}

    /* Checked UltraCombo -> "id1,id2," : each checked caption is looked up by name in the bound list (first match), trailing comma kept. */
    function checkedIds(id){
        const all=Array.from(el(id).options);let ids='';
        for(const option of all.filter(o=>o.selected)){const first=all.find(o=>o.text===option.text);if(first)ids+=first.value+',';}
        return ids;
    }
    const comboValue=id=>Number(el(id).value)||0;
    function request(){
        return {fromDate:el('FromDate').value,toDate:el('Todate').value,classGroupId:comboValue('CmbClassGroup'),itemId:comboValue('CmbItemName'),
            supplierCustomerId:comboValue('CmbSupplier'),warehouseId:comboValue('CmbWarehouse'),jobLotId:comboValue('CmbJobLot'),
            cropYear:el('CmbCropYear').value?$('#CmbCropYear option:selected').text():'',packingTypeId:comboValue('CmbPackingType'),itemUomId:comboValue('CmbUom'),
            skipZero:el('ChkSkipZero').checked,refDocumentTypeIds:checkedIds('CmbReferenceDocument'),parentCategoryIds:checkedIds('CmbItemParentCategory'),
            itemCategoryIds:checkedIds('cmbItemCategory'),itemTypeIds:checkedIds('cmbItemType')};
    }
    /* The 622 / 622-A prints re-run the procedure with the arguments of the last Show (the desktop prints dtlst). */
    function printArgs(r){
        const a={fromDate:r.fromDate,toDate:r.toDate,itemClassGroupId:r.classGroupId,itemId:r.itemId,supplierCustomerId:r.supplierCustomerId,warehouseId:r.warehouseId,
            jobLotId:r.jobLotId,cropYear:r.cropYear,packingTypeId:r.packingTypeId,stockUOM:r.itemUomId,skipZero:r.skipZero?1:0,
            refDocumentTypeIds:r.refDocumentTypeIds,parentCategoryIds:r.parentCategoryIds,itemCategoryIds:r.itemCategoryIds,itemTypeIds:r.itemTypeIds};
        Object.keys(a).forEach(k=>{if(a[k]===''||a[k]===0||a[k]===null||a[k]===undefined)delete a[k];});
        return a;
    }
    async function show(){
        if(el('BtnSearch').disabled)return;
        if(!el('FromDate').value||!el('Todate').value){status('Choose a valid From Date and To Date',true);return;}
        await execute(el('BtnSearch'),'Loading…',async()=>{
            await uomWork;const r=request();
            lastCount=0;
            const data=await InventoryRequest.json(base,{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(r)});
            rows=data.rows||[];rateDenied=!!data.rateAndAmountDenied;checkedRows=new Set();lastCount=rows.length;lastPrintArgs=printArgs(r);
            structure();render();status(`${rows.length} records`);
        });
    }
    window.printRptArgs=function(rpt,args){return /^622a?[-_]StockEvalaution_VehicleWiseTransaction/i.test(String(rpt))?Object.assign({},lastPrintArgs):args;};
    function print(rpt){if(!lastCount){message('No Record Found For Display');return;}if(typeof window.printRpt!=='function'){message('Print is not available.');return;}window.printRpt(rpt,Object.assign({},lastPrintArgs));}

    /* grd_ColumnButtonClick "TranDetail" -> StockTransactionDetail.ShowDialog(). */
    async function tranDetail(row){
        await execute(null,'Loading…',async()=>{
            const q=new URLSearchParams({refDocumentTypeId:Number(row.RefDocumentTypeId)||0,refDocIdNo:Number(row.RefDocIdNo)||0,refDocSubIdNo:Number(row.RefDocSubIdNo)||0});
            const data=await InventoryRequest.json(base+'/transaction-detail?'+q.toString());
            const head=el('detailHeaders'),body=el('detailRecords'),foot=el('detailTotals');head.replaceChildren();body.replaceChildren();foot.replaceChildren();
            if(data.length){
                const sums=['QtyOut','StockWeightOut','AmountOut','CgsAmount'];
                const keys=Object.keys(data[0]).filter(k=>k!=='Id');
                const tr=document.createElement('tr');keys.forEach(k=>{cell(tr,k,'th').dataset.col=k;});head.append(tr);
                for(const r of data){const line=document.createElement('tr');keys.forEach(k=>{const v=r[k];const td=cell(line,sums.includes(k)?num(v,'qty'):k==='DocDate'?dateText(v):String(v??''));if(sums.includes(k))td.className='number';});body.append(line);}
                const total=document.createElement('tr');keys.forEach(k=>{const td=cell(total,sums.includes(k)?num(ReportDecimal.sum(data.map(r=>r[k]??'0')),'qty'):'');if(sums.includes(k))td.className='number';});foot.append(total);
            }
            el('tranDetail').showModal();
        });
    }

    /* btnStockHold_Click -> StockHold() with RemarksPopUp. */
    function remarksPopup(){return new Promise(resolve=>{
        const dlg=el('remarksPopup'),text=el('txtRemarks');text.value='';
        const done=()=>{dlg.removeEventListener('close',done);resolve(text.value);};
        dlg.addEventListener('close',done);dlg.showModal();text.focus();
    });}
    el('btnOk').addEventListener('click',()=>{if(el('txtRemarks').value===''){message('Remarks Required!');el('txtRemarks').focus();return;}el('remarksPopup').close();});
    el('remarksPopup').addEventListener('keydown',e=>{
        if(e.ctrlKey&&e.key==='Enter'){e.preventDefault();el('btnOk').click();}
        else if(e.ctrlKey&&(e.key==='e'||e.key==='E')){e.preventDefault();el('txtRemarks').value='';el('remarksPopup').close();}
        else if(e.key==='Escape'){el('txtRemarks').value='';}
    });
    async function stockHold(){
        if(!checkedRows.size){message('Please Check Rows first');return;}
        if(!window.confirm('Are you sure to Stock Hold?'))return;
        const remarks=await remarksPopup();
        if(!remarks){message('Action Remarks Required');return;}
        const payload={remarks,rows:Array.from(checkedRows).map(i=>rows[i]).filter(Boolean).map(r=>({refDocumentTypeId:Number(r.RefDocumentTypeId)||0,refDocIdNo:Number(r.RefDocIdNo)||0,refDocSubIdNo:Number(r.RefDocSubIdNo)||0}))};
        let ok=false;
        await execute(el('btnStockHold'),'Saving…',async()=>{await InventoryRequest.json(base+'/stock-hold',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});ok=true;});
        if(ok){message("Record's Stock hold Successfully");await show();}
    }
    document.querySelectorAll('dialog [data-close]').forEach(b=>b.addEventListener('click',()=>{const d=b.closest('dialog');if(d.id==='remarksPopup')el('txtRemarks').value='';d.close();}));

    /* Enter = Tab (SendKeys "{TAB}") inside the filter panel: the visible field of the next control in tab order. */
    function focusControl(node){
        if(node.tagName!=='SELECT'){node.focus();return;}
        const s2=$(node).next('.select2-container').find('.select2-selection')[0];if(s2){s2.focus();return;}
        const wrap=node.closest('.dtcombo-wrap')||(node.nextElementSibling&&node.nextElementSibling.classList.contains('dtcombo-wrap')?node.nextElementSibling:null)||(node.previousElementSibling&&node.previousElementSibling.classList.contains('dtcombo-wrap')?node.previousElementSibling:null);
        const field=wrap&&wrap.querySelector('.dtcombo-input');(field||node).focus();
    }
    function nextField(current){
        const order=Array.from(document.querySelectorAll('#filters [tabindex]')).sort((a,b)=>a.tabIndex-b.tabIndex);
        const i=order.indexOf(current);return i>=0?order[i+1]:null;
    }

    $('#filters select:not([data-dtcombo-checked])').select2({width:'resolve',minimumResultsForSearch:0,closeOnSelect:true});
    $('#CmbItemName').on('change',loadUoms);$('#CmbItemName').on('select2:close',()=>{if(el('CmbUom').options.length<=1)loadUoms();});
    el('filters').addEventListener('submit',e=>{e.preventDefault();show();});
    el('Reset').addEventListener('click',reset);el('btnRefresh').addEventListener('click',()=>refresh());
    el('btnPrint').addEventListener('click',()=>print('622-StockEvalaution_VehicleWiseTransaction.rpt'));
    el('btnPrint622').addEventListener('click',()=>print('622A_StockEvalaution_VehicleWiseTransaction.rpt'));
    el('btnStockHold').addEventListener('click',stockHold);
    for(const [id,index]of [['firstRow',()=>0],['previousRow',()=>selected-1],['nextRow',()=>selected+1],['lastRow',()=>visible().length-1]])el(id).addEventListener('click',()=>selectRow(index()));
    el('gridScroll').addEventListener('keydown',e=>{if(e.target.matches('input,button'))return;const indexes={ArrowDown:()=>selected+1,ArrowUp:()=>selected-1,Home:()=>0,End:()=>visible().length-1};if(indexes[e.key]){e.preventDefault();selectRow(indexes[e.key]());}});
    /* KeyDown (KeyPreview): Ctrl+Down grid, Ctrl+F5 From Date, Ctrl+L Show, Ctrl+P 622-Register, Ctrl+E / Esc close, Ctrl+N Refresh. */
    document.addEventListener('keydown',e=>{
        if(document.querySelector('dialog[open]'))return;
        /* an open drop-down handles its own keys (Esc closes the list, Enter picks) */
        if(document.querySelector('.select2-container--open')||(e.target.closest&&e.target.closest('.dtcombo-pop'))||document.querySelector('.dtcombo-pop[style*="block"]'))return;
        if(e.key==='Enter'&&!e.ctrlKey&&e.target.matches('#filters input[type=date],#filters input[type=checkbox]')){e.preventDefault();const next=nextField(e.target);if(next)focusControl(next);return;}
        if(e.ctrlKey&&['l','L','n','N','p','P','e','E','F5','ArrowDown'].includes(e.key)){
            e.preventDefault();const k=e.key.length===1?e.key.toLowerCase():e.key;
            if(k==='l')show();if(k==='n')refresh();if(k==='p')print('622-StockEvalaution_VehicleWiseTransaction.rpt');
            if(k==='e')location.href='/inventory/dashboard';if(k==='F5')el('FromDate').focus();if(k==='ArrowDown')el('gridScroll').focus();
        }else if(e.key==='Escape')location.href='/inventory/dashboard';
    });
    refresh(true);render();
})();
