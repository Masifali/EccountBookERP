'use strict';
(() => {
    const el = id => document.getElementById(id), kind = document.body.dataset.report;
    const payable = kind === 'payables-report';
    const due = kind === 'receivables-by-due-dates', schedule = kind === 'receivables-receipt-schedule';
    let rows = [], lookup = {}, pending = false;
    const titles = {'receivables-by-due-dates':'1007 Receivables By Due Dates New','receivables-receipt-schedule':'1004 Receivables & Receipts Schedule','receivables-report':'1009 Receivables','payables-report':'1010 Payables'};
    document.title = el('heading').textContent = titles[kind];
    document.querySelectorAll('[data-only]').forEach(node => node.hidden = node.dataset.only !== (payable?'receivables-report':kind));
    document.querySelectorAll('[data-except]').forEach(node => node.hidden = node.dataset.except === kind);
    if (due) { el('controls').multiple = false; el('fromLabel').textContent = 'Due Date From'; el('toLabel').textContent = 'Due Date To'; }
    const common = [[payable?'ParentAccountTitle':'ParentAccount','Parent Account',120],['AccountCode','Account Code',95],['AccountTitle','Account Title',250],['AccountType','Account Type',65]];
    const columns = due ? [...common,['Opening','Opening',110],['Debit','Debit',110],['Credit','Credit',110],['Closing','Net Receivables',120],['NotYetDue','Not Yet Due',110],['OverDueReceivables','Over Due',110]] : [...common,['Opening','Opening',102],['CurrDebit','Debit',102],['CurrCredit','Credit',102],[schedule||payable?'Closing':'ClDebit','Closing',102],...(schedule ? [['DueBalance','Due Balance',102],['NotYetDue','Not Yet Due',102],['Short/Excess','Short / Excess',102],['SaleAmount','Sale Amount',102],['SaleQty','Sale Qty',90],['SaleWeight','Sale Weight',90],['OrderQty','Order Qty',90],['DispatchQty','Dispatched Qty',90],['BalQty','Balance Qty',90],['OrderWeight','Order Weight',90],['DispatchWeight','Dispatched Weight',90],['BalWeight','Balance Weight',90],['ReceiptsToday','Receive Today',102]] : [['CityName','City Name',120],['MobilePersonal','Mobile #',110]]),['LastBillDate','Last Bill Date',90],['LastBillsAmount','Last Bill Amount',102],[schedule?'LastReceiptDate':payable?'LastPaidDate':'LastRcvdDate',payable?'Last Paid Date':'Last Received Date',90],[schedule?'LastReceiptAmount':payable?'LastPidAmount':'LastRcvdAmount',payable?'Last Paid Amount':'Last Received Amount',102]];
    const numeric = new Set(['Opening','Debit','Credit','Closing','NotYetDue','OverDueReceivables','CurrDebit','CurrCredit','ClDebit','DueBalance','Short/Excess','SaleAmount','SaleQty','SaleWeight','OrderQty','DispatchQty','BalQty','OrderWeight','DispatchWeight','BalWeight','ReceiptsToday','LastBillsAmount','LastReceiptAmount','LastRcvdAmount','LastPidAmount']);
    const amount = value => ReportDecimal.format(value ?? '0', lookup.amountDecimals || 0, true);
    const date = value => { const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(value ?? '')); return m ? m[3]+'-'+['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][+m[2]-1]+'-'+m[1].slice(-2) : value ?? ''; };
    function render() {
        const table = el('results'), term = el('search').value.toLowerCase();
        const visible = rows.filter(row => Object.values(row).some(value => String(value ?? '').toLowerCase().includes(term)));
        table.tHead.replaceChildren(); table.tBodies[0].replaceChildren(); table.tFoot.replaceChildren();
        const head = table.tHead.insertRow();
        columns.forEach(([,title,width]) => { const th=document.createElement('th'); th.textContent=title; th.style.width=width+'px'; head.append(th); });
        visible.forEach(row => {
            const tr = table.tBodies[0].insertRow(); tr.tabIndex = 0;
            tr.onclick = () => { table.querySelectorAll('.selected').forEach(node=>node.classList.remove('selected')); tr.classList.add('selected'); };
            columns.forEach(([key]) => {
                const td=tr.insertCell();
                if(key==='AccountCode' && row.AccountId) {
                    const a=document.createElement('a'); a.textContent=row[key]??'';
                    a.href='/accounts/reports/general-ledger?'+new URLSearchParams({accountId:row.AccountId,fromDate:el('fromDate').value,toDate:el('toDate').value,branchId:row.BranchesId||0}); td.append(a);
                } else if(numeric.has(key)) { td.className='amount'; td.textContent=amount(row[key]); }
                else td.textContent=key.endsWith('Date')?date(row[key]):row[key]??'';
            });
        });
        const total=table.tFoot.insertRow();
        columns.forEach(([key],index)=>{ const td=total.insertCell(); if(numeric.has(key)){td.className='amount';td.textContent=amount(ReportDecimal.sum(visible.map(row=>row[key]??'0')));}else if(index===0)td.textContent='Total'; });
        el('count').textContent=visible.length+' records';
        el('summary').replaceChildren();
        if(due) [['Closing','Net Receivables'],['NotYetDue','Not Yet Due'],['OverDueReceivables','Over Due']].forEach(([key,title])=>{const card=document.createElement('div');card.className='summary-card';card.textContent=title;const value=document.createElement('strong');value.textContent=amount(ReportDecimal.sum(visible.map(row=>row[key]??'0')));card.append(value);el('summary').append(card);});
    }
    function params() {
        const p = new URLSearchParams({toDate:el('toDate').value,customGroupId:el('customGroupId').value||0});
        if(el('useFrom').checked)p.set('fromDate',el('fromDate').value);
        if(due) p.set('parentId',el('controls').value||0);
        else {
            ['controls','branches'].forEach(id=>p.set(id,[$('#'+id).val()||[]].flat().filter(Boolean).join(',')));
            ['balanceFrom','balanceTo'].forEach(id=>p.set(id,el(id).value||0));
            if(schedule) { ['dueFrom','dueTo','saleFrom','saleTo'].forEach(id=>{if(el(id).value)p.set(id,el(id).value);});p.set('partyGroupId',el('partyGroupId').value||0); }
            else { ['cityId','showAssetLiability'].forEach(id=>p.set(id,el(id).value||0));p.set('groups',($('#groups').val()||[]).join(','));['tradeOnly','skipZero'].forEach(id=>p.set(id,el(id).checked)); }
        }
        return p;
    }
    async function api(url) { const response=await fetch(url);if(!response.ok)throw Error('Unable to load report ('+response.status+'). Check the selected filters and report access.');return response.json(); }
    async function show(event) {
        if(event)event.preventDefault(); if(pending || !el('filters').reportValidity())return;
        pending=true;el('show').disabled=true;el('status').textContent='';rows=[];render();
        try { rows=await api('/api/accounts/desktop-reports/'+kind+'?'+params());render();if(!rows.length)el('status').textContent='No records for the selected filters.'; }
        catch(error) { el('status').textContent=error.message; }
        finally { pending=false;el('show').disabled=false; }
    }
    function reset() { el('filters').reset();el('fromDate').value=el('toDate').value=ReportLoading.localDate();el('fromDate').disabled=false;document.querySelectorAll('select').forEach(select=>$(select).val(select.multiple?[]:'').trigger('change.select2'));el('showAssetLiability').value='0';rows=[];render(); }
    function bind(id,data,label) { const select=el(id);select.replaceChildren();if(!select.multiple)select.add(new Option('',''));data.forEach(row=>select.add(new Option(row[label]??row.Description??row.AccountTitle??'',row.Id))); }
    if (!due && !schedule) {
        document.body.dataset.accountClass=payable?'3':'2';
        document.body.dataset.reportScreenId=payable?'48':'80';
        el('tabs').hidden=false;
        document.querySelectorAll('[data-tab]').forEach(button=>button.onclick=()=>{
            const history=button.dataset.tab==='followup';
            el('followup').hidden=!history;
            ['filters','summary'].forEach(id=>el(id).hidden=history);
            document.querySelector('.grid').hidden=history;document.querySelector('.sortbar').hidden=history;
        });
        const script=document.createElement('script');script.src='/js/trade-followup.js';document.body.append(script);
    }
    el('filters').onsubmit=show;el('search').oninput=render;el('new').onclick=reset;el('print').onclick=()=>window.print();el('useFrom').onchange=()=>el('fromDate').disabled=!el('useFrom').checked;
    $('select').select2({placeholder:'',allowClear:true,minimumResultsForSearch:0});reset();
    (async()=>{
        try {
            lookup=await api('/api/accounts/trade-report/lookups');
            bind('controls',lookup.controls.filter(row=>Number(row.AccountClass)===(payable?3:2)),'AccountTitle');
            bind('customGroupId',lookup.customGroups,'AcLookUpsDescription');bind('cityId',lookup.cities,'CityName');
            bind('groups',lookup.inventoryGroups,'Description');bind('partyGroupId',lookup.partyGroups||[],'Description');
            bind('branches',lookup.branches,'BranchName');
            const features=new Set((lookup.features||[]).map(row=>Number(row.Id)));
            el('branches').closest('label').hidden=due||!features.has(17);
            if(features.has(17)&&!features.has(18)) {
                $('#branches').select2('destroy');el('branches').multiple=false;
                $('#branches').select2({placeholder:'',allowClear:true});
            }
            if(features.has(17)&&lookup.branchId)$('#branches').val(String(lookup.branchId));
            if(!due&&lookup.year?.Start_Period)el('fromDate').value=String(lookup.year.Start_Period).slice(0,10);
            $('select').trigger('change.select2');render();
        } catch(error) { el('status').textContent=error.message; }
    })();
})();
