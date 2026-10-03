'use strict';
(() => {
    const el = id => document.getElementById(id), kind = document.body.dataset.report;
    const payable = kind === 'payables-report';
    const due = kind === 'receivables-by-due-dates', schedule = kind === 'receivables-receipt-schedule';
    let rows = [], lookup = {}, pending = false;
    const titles = {'receivables-by-due-dates':'1007 Receivables By Due Dates New','receivables-receipt-schedule':'1004 Receivables & Receipts Schedule','receivables-report':'1009 Receivables','payables-report':'1010 Payables'};
    document.title = el('heading').textContent = titles[kind];
    /* R4 2026-09-30: toolbar and header captions of each desktop form (Receivables.cs :2667/:2673/:1972,
       ReceivablesByDueDatesNew.cs :1122/:1129/:1136/:742, Payables.cs :1554/:1560/:993). */
    const captions = {'receivables-report':['Reset','122-Print','Receivables Report'],'receivables-by-due-dates':['New','121_01 Print','Receivables By Due Date'],'payables-report':['Refresh','121-Print','Accounts Payables Balance Classification Wise']}[kind] || ['New','Print',titles[kind]];
    el('titleText').textContent = titles[kind]; el('newText').textContent = captions[0]; el('printText').textContent = captions[1]; el('heading').textContent = captions[2];
    document.querySelectorAll('[data-only]').forEach(node => node.hidden = node.dataset.only !== (payable?'receivables-report':kind));
    document.querySelectorAll('[data-except]').forEach(node => node.hidden = node.dataset.except === kind);
    if (due) {
        /* ReceivablesByDueDatesNew: single "3rd Level A/c" combo, "Due From" is a tick-box picker that starts
           UNTICKED (Designer :1070 Checked=false, :1076 ShowCheckBox) - FillGridData sends @FromDate only when ticked. */
        el('controls').multiple = false; el('controls').removeAttribute('data-dtcombo-checked'); el('controls').setAttribute('data-dtcombo', 'single'); el('controls').setAttribute('data-dtcombo-caption', 'Account Title');
        el('fromLabel').textContent = 'Due From'; el('toLabel').textContent = 'Due To'; el('controlsLabel').textContent = '3rd Level A/c';
        el('useFrom').checked = false; el('refresh').hidden = false;
    } else {
        /* Receivables.cs / Payables.cs: the From date has no tick box - @FromDate is always sent. */
        el('useFrom').hidden = true; el('useFrom').checked = true;
    }
    const common = [[payable?'ParentAccountTitle':'ParentAccount','Parent Account',120],['AccountCode','Account Code',95],['AccountTitle','Account Title',250],['AccountType','Account Type',65]];
    const columns = due ? [...common,['Opening','Opening',110],['Debit','Debit',110],['Credit','Credit',110],['Closing','Net Receivables',120],['NotYetDue','Not Yet Due',110],['OverDueReceivables','Over Due',110]] : [...common,['Opening','Opening',102],['CurrDebit','Debit',102],['CurrCredit','Credit',102],[schedule||payable?'Closing':'ClDebit','Closing',102],...(schedule ? [['DueBalance','Due Balance',102],['NotYetDue','Not Yet Due',102],['Short/Excess','Short / Excess',102],['SaleAmount','Sale Amount',102],['SaleQty','Sale Qty',90],['SaleWeight','Sale Weight',90],['OrderQty','Order Qty',90],['DispatchQty','Dispatched Qty',90],['BalQty','Balance Qty',90],['OrderWeight','Order Weight',90],['DispatchWeight','Dispatched Weight',90],['BalWeight','Balance Weight',90],['ReceiptsToday','Receive Today',102]] : [['CityName','City Name',120],['MobilePersonal','Mobile #',110]]),['LastBillDate','Last Bill Date',90],['LastBillsAmount','Last Bill Amount',102],[schedule?'LastReceiptDate':payable?'LastPaidDate':'LastRcvdDate',payable?'Last Paid Date':'Last Received Date',90],[schedule?'LastReceiptAmount':payable?'LastPidAmount':'LastRcvdAmount',payable?'Last Paid Amount':'Last Received Amount',102]];
    /* Receivables.cs DataFill()/GridSettings() (screen 80): column order and captions of the desktop grid. */
    if (kind === 'receivables-report') columns.splice(0, columns.length, ['ParentAccount','Parent Account',150],['AccountClass','Account Class',80],['AccountCode','Account Code',90],['AccountTitle','Account Title',160],['AccountType','Account Type',80],['Opening','Opening Balance',100],['CurrDebit','Debit',100],['CurrCredit','Credit',100],['ClDebit','Closing Balance',100],['LastBillDate','Last Bill Date',80],['LastBillsAmount','Last Bills Amount',90],['BillDays','Bill Days',60],['LastRcvdDate','Received Date',80],['LastRcvdAmount','Received Amount',90],['RcvdDays','Received Days',65],['CityName','City Name',120],['MobilePersonal','Mobile Personal',110],['Title','Title',120],['FollowupDate','Follow Up Date',85]);
    /* R4 2026-09-30 - ReceivablesByDueDatesNew.GridSettings(): the grid is built from the desktop's own table (AccountId,
       ParentAccount, AccountCode, AccountTitle, AccountType, Opening, Debit, Credit, NetReceivables, NotYetDue, OverDue),
       Opening and AccountId hidden, grouped by ParentAccount with the column hidden when grouped, captions = column names. */
    if (due) columns.splice(0, columns.length, ['AccountCode','AccountCode',95],['AccountTitle','AccountTitle',250],['AccountType','AccountType',65],['Debit','Debit',110],['Credit','Credit',110],['Closing','NetReceivables',120],['NotYetDue','NotYetDue',110],['OverDueReceivables','OverDue',110]);
    const numeric = new Set(['Opening','Debit','Credit','Closing','NotYetDue','OverDueReceivables','CurrDebit','CurrCredit','ClDebit','DueBalance','Short/Excess','SaleAmount','SaleQty','SaleWeight','OrderQty','DispatchQty','BalQty','OrderWeight','DispatchWeight','BalWeight','ReceiptsToday','LastBillsAmount','LastReceiptAmount','LastRcvdAmount','LastPidAmount']);
    const amount = value => ReportDecimal.format(value ?? '0', lookup.amountDecimals || 0, true);
    const date = value => { const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(value ?? '')); return m ? m[3]+'-'+['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][+m[2]-1]+'-'+m[1].slice(-2) : value ?? ''; };
    function render() {
        const table = el('results'), term = el('search').value.toLowerCase();
        const visible = rows.filter(row => Object.values(row).some(value => String(value ?? '').toLowerCase().includes(term)));
        table.tHead.replaceChildren(); table.tBodies[0].replaceChildren(); table.tFoot.replaceChildren();
        const head = table.tHead.insertRow();
        columns.forEach(([,title,width]) => { const th=document.createElement('th'); th.textContent=title; th.style.width=width+'px'; head.append(th); });
        if (due) { const order = new Map(); visible.forEach(r => { const k = String(r.ParentAccount ?? ''); if (!order.has(k)) order.set(k, order.size); }); visible.sort((a, b) => order.get(String(a.ParentAccount ?? '')) - order.get(String(b.ParentAccount ?? ''))); }
        let lastGroup = null;
        visible.forEach(row => {
            if (due && String(row.ParentAccount ?? '') !== lastGroup) {
                lastGroup = String(row.ParentAccount ?? '');
                const g = table.tBodies[0].insertRow(); g.className = 'group'; const c = g.insertCell(); c.colSpan = columns.length; c.textContent = 'ParentAccount: ' + lastGroup;
            }
            const tr = table.tBodies[0].insertRow(); tr.tabIndex = 0;
            tr.onclick = () => { table.querySelectorAll('.selected').forEach(node=>node.classList.remove('selected')); tr.classList.add('selected'); };
            columns.forEach(([key]) => {
                const td=tr.insertCell();
                if(key==='AccountCode' && row.AccountId) {
                    const a=document.createElement('a'); a.textContent=row[key]??'';
                    a.href='/accounts/reports/general-ledger?'+new URLSearchParams({accountId:row.AccountId,fromDate:el('fromDate').value,toDate:el('toDate').value,branchId:row.BranchesId||0}); td.append(a);
                    /* grd_FormattingRow-free desktop grids: the account code is the link (ColumnType Link). */
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
        /* InfragisticsHelper.GetBranchesIdsByFeature: branch feature on, consolidated off -> a branch is required. */
        if(!due && lookup.branchFeature && !lookup.branchConsolidated && !Number(el('branches').value||0)) { el('status').textContent='Please Select Branch first!'; const w=$('#branches').closest('.dtcombo-wrap').find('.dtcombo-input'); (w.length?w:$('#branches')).focus(); return; }
        pending=true;el('show').disabled=true;el('status').textContent='';rows=[];render();
        try { rows=await api('/api/accounts/desktop-reports/'+kind+'?'+params());if(kind==='receivables-report'){const at=columns.findIndex(c=>c[0]==='BranchName');if(at>=0)columns.splice(at,1);if(lookup.branchFeature&&[$('#branches').val()].flat().filter(Boolean).length)columns.unshift(['BranchName','Branch Name',120]);}render();if(!rows.length)el('status').textContent='No records for the selected filters.'; }
        catch(error) { el('status').textContent=error.message; }
        finally { pending=false;el('show').disabled=false; }
    }
    const yearStart = () => lookup.year?.Start_Period ? String(lookup.year.Start_Period).slice(0,10) : ReportLoading.localDate();
    /* R4 2026-09-30, per desktop form:
       Receivables.cs btnNew_Click_1 ("Reset"): From = ActiveYr.Start_Period, To = today, Closing From/To, Control Account,
         Inventory Group and Custom Group cleared, grid cleared.
       ReceivablesByDueDatesNew.reset(): focus Due From only.
       Payables.cs reset(): focus From date, Control Account cleared. */
    function reset() {
        if (due) { el('fromDate').focus(); return; }
        if (payable) { $('#controls').val([]); el('fromDate').focus(); return; }
        el('fromDate').value = yearStart(); el('toDate').value = ReportLoading.localDate();
        el('balanceFrom').value = ''; el('balanceTo').value = '';
        $('#controls').val([]); $('#groups').val([]); $('#customGroupId').val('');
        rows=[];render();
    }
    function initDates() { el('fromDate').value=el('toDate').value=ReportLoading.localDate(); el('fromDate').disabled=!el('useFrom').checked; }
    function bind(id,data,label) { const select=el(id);select.replaceChildren();if(!select.multiple)select.add(new Option('',''));data.forEach(row=>select.add(new Option(row[label]??row.Description??row.AccountTitle??'',row.Id))); }
    if (!due && !schedule) {
        document.body.dataset.accountClass=payable?'3':'2';
        document.body.dataset.reportScreenId=payable?'48':'80';
        el('tabs').hidden=false;
        document.querySelectorAll('[data-tab]').forEach(button=>button.onclick=()=>{
            const history=button.dataset.tab==='followup';
            el('followup').hidden=!history;
            ['filters','summary'].forEach(id=>el(id).hidden=history);
            document.querySelector('.grid-wrap').hidden=history;document.querySelector('.sortbar').hidden=history;
            document.querySelectorAll('#tabs > li').forEach(li=>li.classList.toggle('active',li.contains(button)));
        });
        const script=document.createElement('script');script.src='/js/trade-followup.js';document.body.append(script);
    }
    el('filters').onsubmit=show;el('search').oninput=render;el('new').onclick=reset;el('print').onclick=()=>window.print();el('useFrom').onchange=()=>el('fromDate').disabled=!el('useFrom').checked;
    initDates();
    /* ReceivablesByDueDatesNew.AccountFill3rdLevel(): ChartofAccount.ReadAll3rdLevelAccountsForPayablesandReceeivablesAging
       (Sp_AccountsOpeningBalances_GetMethod), rows with TypeNo 2, Id = "id". Served with the signed-in tenancy by the
       Receivables Aging lookups (the desktop's own call builds new ReportsParameters() and so sends org/company 0,
       which returns no rows - that empty combo is not reproduced). */
    async function dueLookups() {
        const l = await api('/api/accounts/receivables-new/lookups');
        lookup = l;
        bind('controls', (l.controls||[]).filter(r=>Number(r.TypeNo)===2).map(r=>({Id:r.id ?? r.Id, AccountTitle:r.AccountTitle})), 'AccountTitle');
        bind('customGroupId', l.customGroups||[], 'AcLookUpsDescription');
    }
    if (due) el('refresh').onclick = async () => { try { await dueLookups(); } catch(error) { el('status').textContent=error.message; } };
    (async()=>{
        try {
            if (due) { await dueLookups(); return; }
            lookup=await api('/api/accounts/trade-report/lookups');
            /* Receivables.cs AccountFill3rdLevel(): ReadAllAccountgroup with AccountClassIds "2,3" and no class filter. */bind('controls',kind==='receivables-report'?lookup.controls:lookup.controls.filter(row=>Number(row.AccountClass)===(payable?3:2)),'AccountTitle');
            bind('customGroupId',lookup.customGroups,'AcLookUpsDescription');bind('cityId',lookup.cities,'CityName');
            bind('groups',lookup.inventoryGroups,'Description');bind('partyGroupId',lookup.partyGroups||[],'Description');
            bind('branches',lookup.branches,'BranchName');
            const features=new Set((lookup.features||[]).map(row=>Number(row.Id)));lookup.branchFeature=features.has(17);lookup.branchConsolidated=features.has(18);
            el('branchesLabel').hidden=el('branchesCell').hidden=due||!features.has(17);
            if(features.has(17)&&!features.has(18)) {
                /* Feature 18 off: a single-pick branch combo (searchable). */
                el('branches').multiple=false; el('branches').setAttribute('data-dtcombo','single');
                if (window.DesktopCombo) DesktopCombo.enhance(el('branches'));
            } else if(features.has(17)) {
                /* Feature 18 on (consolidated): the desktop's CHECKED branch combo (countx_prod_combo.js checked mode, 2026-10-02). */
                el('branches').setAttribute('data-dtcombo','single'); el('branches').setAttribute('data-dtcombo-checked','');
                if (window.DesktopCombo) DesktopCombo.enhance(el('branches'));
            }
            if(features.has(17)&&lookup.branchId)$('#branches').val(String(lookup.branchId));
            if(!due&&lookup.year?.Start_Period)el('fromDate').value=String(lookup.year.Start_Period).slice(0,10);
            render();
            /* Receivables_Load ends with DataFill(): the report is shown on open. */
            if(kind==='receivables-report')await show();
        } catch(error) { el('status').textContent=error.message; }
    })();
})();
