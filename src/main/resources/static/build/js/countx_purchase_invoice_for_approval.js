(function () {
    'use strict';
    var API = '/api/dashboard/purchase-invoice-approval', qs = new URLSearchParams(location.search);
    var unApprove = qs.get('unApprove') === 'true', statusLabel = unApprove ? 'approved' : 'pending';
    var rows = [], lines = {}, loadedFilter = null, busy = false, ready = false, yearStart = '';
    var columns = [['InvoiceNo','Invoice No'],['InvoiceDate','Invoice Date'],['SupplierName','Supplier'],
        ['BillAmount','Bill Amount','number'],['HeaderQty','Quantity','number'],['HeaderNetWeight','Net Weight','number'],
        ['AvgRate','Average Rate','number'],['BranchSrNo','Branch No'],['BranchName','Branch'],['ManualBillNo','Manual Bill No'],
        ['CommissionAgent','Commission Agent'],['CommType','Commission Type'],['CommRate','Commission Rate','number'],
        ['CommAmount','Commission Amount','number'],['RemarksHeader','Remarks'],['UserName','Entry User'],['EntryDate','Entry Date']];
    var details = [['OrderNo','Order No'],['WareHouseName','Warehouse'],['ItemName','Item'],['PackTypeDesc','Packing'],
        ['CropYear','Crop Year'],['JobLotDescription','Job / Lot'],['GpNo','Gate Pass No'],['VehicleNo','Vehicle'],
        ['ItemQty','Quantity','number'],['PackUom','Pack UOM'],['GrossWeight','Gross Weight','number'],
        ['EBTotalWt','Empty Bag Weight','number'],['WeightCutTotal','Weight Cut','number'],['NetWeight','Net Bill Weight','number'],
        ['NetStockWeight','Stock Weight','number'],['ItemRate','Rate','number'],['RateUom','Rate UOM'],
        ['RateCut','Rate Cut','number'],['RateCutAmount','Rate Cut Amount','number'],['ItemAmount','Item Amount','number'],
        ['CommissionAmount','Commission','number'],['ExpenseAmount','Expenses','number'],['FreightAmount','Freight','number'],
        ['LabAnalisysNo','Purchase Analysis No'],['SampleNo','Sample Analysis No']];
    function el(id) { return document.getElementById(id); }
    function esc(v) { return String(v == null ? '' : v).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;'); }
    function message(text, error) { el('piaMessage').textContent=text || ''; el('piaMessage').className=error ? 'error' : ''; }
    function today() { var d=new Date(); return d.getFullYear()+'-'+String(d.getMonth()+1).padStart(2,'0')+'-'+String(d.getDate()).padStart(2,'0'); }
    function filter() { return {documentTypeId:Number(qs.get('documentTypeId') || 56),branchId:Number(qs.get('branchId') || 0),
        fromDate:el('piaFrom').value || null,toDate:el('piaTo').value || null,supplierId:Number(el('piaSupplier').value),itemId:Number(el('piaItem').value),unApprove:unApprove}; }
    function query(f) { var q=new URLSearchParams(); Object.keys(f).forEach(function(k){if(f[k]!=null)q.set(k,f[k]);});return q.toString(); }
    function headers() {
        var h={'Accept':'application/json','Content-Type':'application/json'};
        var csrf=document.querySelector('meta[name="_csrf"]'), name=document.querySelector('meta[name="_csrf_header"]');
        if(csrf && name)h[name.content]=csrf.content;
        return h;
    }
    function controls(on) {
        busy=on;
        document.querySelectorAll('.pia-toolbar button,#piaFilters input,#piaFilters select,#piaFilters button,#piaInvoices button,#piaInvoices input').forEach(function(b){b.disabled=on || !ready;});
        el('piaApprove').disabled=on || !ready || !loadedFilter || !rows.length;
    }
    async function read(url, options) {
        var r=await fetch(url,Object.assign({credentials:'same-origin',headers:{Accept:'application/json'}},options || {}));
        if(r.redirected || r.status===401)throw Error('Your session has expired. Sign in again and reopen the Approval Dashboard.');
        var d; try{d=await r.json();}catch(e){throw Error('The approval screen could not load. Please try again.');}
        if(!r.ok)throw Error(d.message || 'The request failed ('+r.status+').');return d;
    }
    function cell(r,c) {
        var v=r[c[0]];
        if(c[2] && v!=='' && v!=null && Number.isFinite(Number(v)))v=Number(v).toLocaleString(undefined,{maximumFractionDigits:4});
        else if(/Date$/.test(c[0]) && v)v=String(v).replace('T',' ').slice(0,c[0]==='EntryDate'?16:10);
        return '<td'+(c[2]?' class="number"':'')+'>'+esc(v)+'</td>';
    }
    function showDetails(id) {
        document.querySelectorAll('#piaInvoices tbody tr').forEach(function(r){r.classList.toggle('selected',Number(r.dataset.id)===id);});
        var invoice=rows.find(function(r){return Number(r.Id)===id;});
        el('piaDetailsTitle').textContent=invoice ? 'Invoice '+invoice.InvoiceNo+' — details' : 'Invoice details';
        el('piaDetails').tHead.innerHTML='<tr>'+details.map(function(c){return '<th>'+c[1]+'</th>';}).join('')+'</tr>';
        el('piaDetails').tBodies[0].innerHTML=(lines[id] || []).map(function(r){return '<tr>'+details.map(function(c){return cell(r,c);}).join('')+'</tr>';}).join('');
    }
    function button(action,label,id) { return '<button type="button" data-action="'+action+'" data-id="'+id+'">'+label+'</button>'; }
    function render() {
        el('piaInvoices').tHead.innerHTML='<tr><th><input type="checkbox" id="piaAll" aria-label="Select all invoices"></th><th>Actions</th>'
            +columns.map(function(c){return '<th>'+c[1]+'</th>';}).join('')+'</tr>';
        el('piaInvoices').tBodies[0].innerHTML=rows.map(function(r){
            var id=Number(r.Id);
            return '<tr data-id="'+id+'"><td><input type="checkbox" data-select="'+id+'" aria-label="Select invoice '+esc(r.InvoiceNo)+'"></td><td>'
                +button('approve',unApprove?'Undo approval':'Approve',id)+button('slip','Party slip',id)+(Number(r.VoucherHeadId)>0 ? button('voucher','Voucher',id) : '')+'</td>'
                +columns.map(function(c){return cell(r,c);}).join('')+'</tr>';
        }).join('');
        el('piaCount').textContent=rows.length ? rows.length+' '+statusLabel+' invoice(s)' : 'No '+statusLabel+' invoices match these filters.';
        showDetails(rows.length ? Number(rows[0].Id) : 0);
    }
    async function load(keepMessage) {
        var f=filter();
        if(!f.toDate || (f.fromDate && f.fromDate>f.toDate)){message('Select a valid From Date and To Date.',true);return;}
        controls(true); if(!keepMessage)message('Loading '+statusLabel+' invoices…');
        try{var d=await read(API+'/history?'+query(f));rows=d.rows || [];lines=d.lines || {};loadedFilter=f;render();if(!keepMessage)message('');}
        catch(e){rows=[];lines={};loadedFilter=null;render();message(e.message,true);}
        finally{controls(false);}
    }
    async function approve(ids) {
        if(busy || !ready)return;
        if(!ids.length){message('Select at least one invoice first.',true);return;}
        if(JSON.stringify(filter())!==JSON.stringify(loadedFilter)){message('Filters have changed. Press Show before changing invoice approval.',true);return;}
        if(!confirm((unApprove?'Undo approval for ':'Approve ')+ids.length+' selected invoice(s) and their linked vouchers?'))return;
        controls(true);message(unApprove?'Undoing invoice approval…':'Approving invoices…');
        try{
            var d=await read(API+(unApprove?'/unapprove':'/approve'),{method:'POST',headers:headers(),body:JSON.stringify({filter:loadedFilter,ids:ids})});
            message(d.message);await load(true);
        }catch(e){
            // A lost response must not invite another blind approval attempt.
            var failure=e.message;await load(true);message(failure,true);
        }finally{controls(false);}
    }
    async function print(action,id) {
        var row=rows.find(function(r){return Number(r.Id)===id;});if(!row || busy)return;
        var preview=window.open('about:blank','_blank');
        if(!preview){message('Allow pop-ups for this site to open the print preview.',true);return;}
        preview.document.body.textContent='Preparing print preview…';
        var voucher=action==='voucher', rpt=voucher?'103-AcRptPurchaseSalesVoucherSlip.rpt':'220A-InvRptPurchaseBillSupplierRiceSlip.rpt';
        try{
            var h=headers();h.Accept='application/pdf';
            var r=await fetch('/reports/print/by-template/'+encodeURIComponent(rpt)+'/pdf',{method:'POST',credentials:'same-origin',headers:h,
                body:JSON.stringify(voucher ? {id:Number(row.VoucherHeadId),documentTypeId:56} : {id:id})});
            if(r.redirected || r.status===401)throw Error('Sign in again to print this report.');
            if(!r.ok || !(r.headers.get('Content-Type') || '').includes('application/pdf'))throw Error('Could not prepare the print preview. Please try again.');
            var url=URL.createObjectURL(await r.blob());preview.location.replace(url);setTimeout(function(){URL.revokeObjectURL(url);},300000);
        }catch(e){preview.document.body.textContent=e.message;message(e.message,true);}
    }
    el('piaFilters').addEventListener('submit',function(e){e.preventDefault();if(!busy && ready)load();});
    el('piaRefresh').addEventListener('click',function(){if(!busy && ready)load();});
    el('piaReset').addEventListener('click',function(){if(busy || !ready)return;el('piaSupplier').value='0';el('piaItem').value='0';el('piaFrom').value=yearStart;el('piaTo').value=today();load();});
    el('piaApprove').addEventListener('click',function(){approve(Array.from(document.querySelectorAll('#piaInvoices [data-select]:checked')).map(function(c){return Number(c.dataset.select);}));});
    el('piaInvoices').addEventListener('change',function(e){if(e.target.id==='piaAll')document.querySelectorAll('#piaInvoices [data-select]').forEach(function(c){c.checked=e.target.checked;});});
    el('piaInvoices').addEventListener('click',function(e){
        var b=e.target.closest('[data-action]');
        if(b){if(b.dataset.action==='approve')approve([Number(b.dataset.id)]);else print(b.dataset.action,Number(b.dataset.id));return;}
        var r=e.target.closest('tr[data-id]');if(r)showDetails(Number(r.dataset.id));
    });
    async function boot() {
        if(unApprove) {
            document.title=el('piaTitle').textContent='Undo Purchase Invoice Approval';
            el('piaBack').href='/dashboard/un-approved-invoices-and-vouchers';
            el('piaBack').textContent='← Back to Undo Approval Dashboard';
            el('piaApprove').textContent='Undo approval for selected';
            el('piaInvoicesTitle').textContent='Approved purchase invoices';
        }
        controls(true);
        try{
            el('piaFrom').value=qs.get('fromDate') || '';el('piaTo').value=qs.get('toDate') || today();
            var d=await read(API+'/setup?'+query(filter()));yearStart=d.yearStart || '';
            el('piaFrom').value=d.fromDate;el('piaTo').value=d.toDate;
            [['piaSupplier',d.suppliers,'All suppliers'],['piaItem',d.items,'All items']].forEach(function(a){
                el(a[0]).innerHTML='<option value="0">'+a[2]+'</option>'+(a[1] || []).map(function(r){return '<option value="'+esc(r.id)+'">'+esc(r.name)+'</option>';}).join('');
            });
            ready=true;await load();
        }catch(e){message(e.message,true);el('piaCount').textContent='Could not load invoices.';controls(false);}
    }
    boot();
}());
