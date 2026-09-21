(() => {
    'use strict';
    const $id = id => document.getElementById(id), endpoint='/inventory/api/reports/stock-transactions';
    const columns=[['DocumentTypeDescription','DocumentType'],['DocCodeNo','DocNo'],['DocDate','Date'],['WareHouseName','WareHouse'],['ItemName','Item'],['ItemUom','Uom'],['JobLotDescription','JobLot'],['CurrInQty','InQty'],['CurrOutQty','OutQty'],['BalQty','BalQty'],['CurrInWeight','InWeight'],['CurrOutWeight','OutWeight'],['CurrInBillWeight','InBillWeight'],['CurrOutBillWeight','OutBillWeight'],['BalWeight','BalWeight']];
    let lookupData, busy=false, records=[], filtersReady=false;
    const months=['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function setDate(key,iso){const [year,month,day]=iso.split('-');$id(key).value=day+'-'+months[Number(month)-1]+'-'+year.slice(-2);$id(key+'-calendar').value=iso;}
    function getDate(key){const match=/^(\d{2})-([A-Za-z]{3})-(\d{2}|\d{4})$/.exec($id(key).value.trim());if(!match)throw Error('Enter dates as dd-MMM-yy');const month=months.findIndex(m=>m.toLowerCase()===match[2].toLowerCase());let year=Number(match[3]);if(match[3].length===2){const previous=Number($id(key+'-calendar').value.slice(0,4));year=previous%100===year?previous:(year<30?2000:1900)+year;}const date=new Date(year,month,Number(match[1]));if(month<0||date.getFullYear()!==year||date.getMonth()!==month||date.getDate()!==Number(match[1]))throw Error('Enter a valid date');return ReportLoading.localDate(date);}
    const value=(row,key)=>row[Object.keys(row).find(k=>k.toLowerCase()===key.toLowerCase())];
    function status(text,error=false){$id('status').textContent=text;$id('status').classList.toggle('error',error);}
    async function request(url,options){
        const headers={...options?.headers},token=document.querySelector('meta[name=_csrf]')?.content,key=document.querySelector('meta[name=_csrf_header]')?.content;if(token&&key)headers[key]=token;
        return InventoryRequest.json(url,{...options,headers});
    }
    async function run(button,label,action){if(busy||button?.disabled)return;busy=true;try{await InventoryRequest.execute(button,label,action);}catch(error){status(error.message,true);}finally{busy=false;}}
    function addOptions(select,rows,idKey,nameKey){
        select.replaceChildren();
        if(!select.multiple)select.add(new Option('', ''));
        (rows||[]).forEach(row=>{
            const text = value(row,nameKey) || value(row,'Name') || value(row,'Description') || value(row,'CategoryDescription') || value(row,'TypeDescription') || value(row,'WareHouseName') || value(row,'ItemName') || value(row,'PartyName') || value(row,'JobLotDescription') || value(row,'InvParentCateDescription') || value(row,'DocumentTypeDescription') || '';
            const val = value(row,idKey) || value(row,'Id') || value(row,'ID') || value(row,'RefDocumentTypeId') || '';
            if (val !== undefined && val !== null && val !== '') {
                select.add(new Option(text, val));
            }
        });
        $(select).trigger('change.select2');
    }
    function dateType(){
        const today=new Date(),from=new Date(today),type=$id('dateType').value;
        if(!type)return;
        if(type==='2')from.setDate(from.getDate()-7);
        if(type==='3'){from.setFullYear(today.getUTCFullYear(),today.getUTCMonth(),1);}
        if(type==='4')from.setMonth(0,1);
        if(type==='5'){
            const years=lookupData?.financialYears||[];
            if(years.length!==1){status('Select one active financial year in your company before using Financial Year.',true);return;}
            const start=value(years[0],'Start_Period');
            if(!start){status('The active financial year has no start date.',true);return;}
            setDate('fromDate',String(start).slice(0,10));return;
        }
        setDate('fromDate',ReportLoading.localDate(from));
        if(type==='3'||type==='4')setDate('toDate',ReportLoading.localDate(today));
    }
    function cell(row,text,tag='td'){const cell=document.createElement(tag);cell.textContent=text;row.append(cell);return cell;}
    const number=v=>ReportDecimal.format(v??'0',3).replace(/(\.\d*?)0+$/,'$1').replace(/\.$/,'');
    function render(data){
        $id('rows').replaceChildren();$id('totals').replaceChildren();
        const fragment=document.createDocumentFragment();
        data.forEach(item=>{
            const row=document.createElement('tr');row.tabIndex=0;row.onclick=()=>{for(const r of $id('rows').rows)r.classList.toggle('selected',r===row);};row.onkeydown=e=>{if(['ArrowUp','ArrowDown'].includes(e.key)){e.preventDefault();(e.key==='ArrowUp'?row.previousElementSibling:row.nextElementSibling)?.focus();}};
            columns.forEach(([key],i)=>{
                let text=value(item,key)??'';
                if(i>=7)text=number(text||'0');
                if(key==='DocDate' && /^\d{4}-\d{2}-\d{2}/.test(String(text))){const [y,m,d]=String(text).slice(0,10).split('-');text=`${d}-${months[Number(m)-1]}-${y.slice(-2)}`;}
                const td=cell(row,text);if(i>=7)td.className='number';
            });fragment.append(row);
        });$id('rows').append(fragment);
        const total=document.createElement('tr');
        columns.forEach(([key],i)=>{const td=cell(total,i===0?'Total':i>=7?number(ReportDecimal.sum(data.map(r=>value(r,key)??'0'))):'');if(i>=7)td.className='number';});
        $id('totals').append(total);status(`${data.length} records`);
    }
    async function load(){
        if(!filtersReady)throw Error('Report filters are unavailable. Refresh after your company access is enabled.');
        if(!$id('filters').reportValidity())return;
        const body={fromDate:getDate('fromDate'),toDate:getDate('toDate')};
        if(body.fromDate>body.toDate)throw Error('From Date must be on or before To Date.');
        status('Loading transactions…');records=[];render([]);
        document.querySelectorAll('select[data-activity],#documentTypeId').forEach(select=>{
            body[select.id]=select.multiple?Array.from(select.selectedOptions,o=>o.value).join(','):select.value?Number(select.value):null;
        });
        records=await request(endpoint,{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(body)});filter();
    }
    async function refresh(){
            filtersReady=false;ReportLoading.setDisabled($id('show'),true);ReportLoading.setDisabled($id('print'),true);
            lookupData=await request(endpoint+'/lookups');
            document.querySelectorAll('select[data-activity]').forEach(select=>addOptions(select,lookupData.choices.filter(row=>value(row,'ActivityType')===select.dataset.activity),'Id','name'));
            addOptions($id('documentTypeId'),lookupData.documents,'RefDocumentTypeId','DocumentTypeDescription');
            $('select').trigger('change.select2');render([]);
            filtersReady=true;ReportLoading.setDisabled($id('show'),false);ReportLoading.setDisabled($id('print'),false);filter();
    }
    function reset(){for(const key of ['warehouseIds','supplierCustomerId','itemId','jobLotId','dateType']){$($id(key)).val(key==='warehouseIds'?[]:'').trigger('change.select2');}$id('dateType').focus();status('');}
    function filter(){const inputs=[...$id('head').querySelectorAll('input')];render(records.filter(row=>inputs.every((input,index)=>String(value(row,columns[index][0])??'').toLowerCase().includes(input.value.toLowerCase()))));}
    const header=document.createElement('tr'),filters=document.createElement('tr');filters.className='column-filters';columns.forEach(([key,label])=>{cell(header,label,'th');const input=document.createElement('input');input.setAttribute('aria-label','Filter '+label);input.oninput=filter;cell(filters,'','th').append(input);});$id('head').append(header,filters);
    $id('filters').addEventListener('submit',event=>{event.preventDefault();run($id('show'),'Loading transactions…',load);});
    // Select2 dispatches jQuery events, so bind the desktop date event through jQuery.
    $('#dateType').off('change').on('change',dateType);
    $id('refresh').addEventListener('click',()=>run($id('refresh'),'Refreshing dropdowns…',refresh));$id('reset').addEventListener('click',()=>{if(!busy)reset();});
    $id('print').addEventListener('click',()=>status('The original 417 Crystal report output is not available yet. Desktop report verification remains pending.',true));
    $id('shortcuts').addEventListener('click',()=>$id('keys').showModal());$id('close-keys').onclick=()=>$id('keys').close();
    for(const key of ['fromDate','toDate']){$id(key+'-calendar').onchange=()=>{if($id(key+'-calendar').value)setDate(key,$id(key+'-calendar').value);};$id(key).onblur=()=>{try{setDate(key,getDate(key));status('');}catch(error){status(error.message,true);}};$id(key).onkeydown=e=>{if(['ArrowUp','ArrowDown'].includes(e.key)){e.preventDefault();try{const date=new Date(getDate(key)+'T00:00:00');date.setDate(date.getDate()+(e.key==='ArrowUp'?1:-1));setDate(key,ReportLoading.localDate(date));}catch(error){status(error.message,true);}}};}
    document.addEventListener('keydown',event=>{if($id('keys').open||document.querySelector('.select2-container--open'))return;const key=event.key.toLowerCase();if(event.ctrlKey&&event.altKey){event.preventDefault();$id('keys').showModal();return;}if(event.ctrlKey){const button={s:'show',r:'refresh',n:'refresh',p:'print'}[key];if(button){event.preventDefault();$id(button).click();}if(['f5','arrowup'].includes(key)){event.preventDefault();$('#dateType').select2('focus');}if(key==='arrowdown'){event.preventDefault();document.querySelector('.grid').focus();}}if(key==='escape'||event.ctrlKey&&key==='e'){event.preventDefault();if(!busy)location.assign('/inventory');}if(key==='enter'&&!event.ctrlKey&&event.target.matches('input,select')){event.preventDefault();const controls=[...$id('filters').querySelectorAll('input:not([type=date]),.select2-selection,button')].filter(e=>e.offsetParent!==null);controls[controls.indexOf(event.target)+1]?.focus();}});
    // Initialize local controls independently of database authorization or availability.
    $('#filters select').each(function(){const width=this.id==='documentTypeId'?'164px':this.closest('.first')?'148px':this.closest('.middle')?'202px':'228px';$(this).select2({width,minimumResultsForSearch:0,placeholder:'',allowClear:true});});
    $id('dateType').value='2';setDate('toDate',ReportLoading.localDate());dateType();$('#dateType').trigger('change.select2');
    ReportLoading.setDisabled($id('show'),true);ReportLoading.setDisabled($id('print'),true);
    run(null,'Loading report filters…',async()=>{await refresh();if(lookupData.fromDate)setDate('fromDate',lookupData.fromDate);});
})();
