(function () {
    'use strict';
    var API = '/api/dashboard/sale-order-approval';
    var qs = new URLSearchParams(location.search);
    var type = Number(qs.get('documentTypeId') || 81), branch = Number(qs.get('branchId') || 0);
    var rows = [], lines = {}, rights = {}, loadedFilter = null, busy = false;
    var headerColumns = [['DocNo','Order No'],['DocDate','Order Date'],['SupplierName','Customer'],
        ['Amount','Amount','number'],['BranchName','Branch'],['CompanyNameSpCAgent','Commission Agent'],
        ['DeliveryTerm','Delivery Term'],['OrderExpiryDate','Expiry Date'],['UserNameEusr','Entry User'],['EntryDate','Entry Date']];
    var detailColumns = [['ItemName','Item'],['Crop','Crop'],['PackTypeDesc','Packing'],['UOMCodeItm','Pack UOM'],
        ['OrderItemQty','Quantity','number'],['NetWeight','Weight','number'],['OrderItemRate','Rate','number'],
        ['UOMCodeRate','Rate UOM'],['Amount','Amount','number'],['BagPrice','Bag Price','number'],
        ['BagWeight','Bag Weight','number'],['JobLotCode','Job Lot'],['CityArea','City / Area'],['IsCanceled','Cancelled']];
    function el(id) { return document.getElementById(id); }
    function esc(v) { return String(v == null ? '' : v).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;'); }
    function message(text, error) { el('soaMessage').textContent = text || ''; el('soaMessage').className = error ? 'error' : ''; }
    function filter() { return {documentTypeId:type,branchId:branch,fromDate:el('soaFrom').value || null,
        toDate:el('soaTo').value || null,customerId:Number(el('soaCustomer').value),itemId:Number(el('soaItem').value),
        approved:el('soaApproved').value,status:el('soaStatus').value}; }
    function query(f) { var q = new URLSearchParams(); Object.keys(f).forEach(function(k) { if(f[k] != null && f[k] !== '') q.set(k,f[k]); }); return q.toString(); }
    function controls(on) {
        busy = on;
        document.querySelectorAll('.soa-toolbar button,#soaFilters button,#soaFilters input,#soaFilters select,#soaOrders button,#soaOrders input').forEach(function(b) { b.disabled = on; });
        var canApprove = loadedFilter && loadedFilter.approved === 'Not Approved' && loadedFilter.status === 'Open';
        el('soaApprove').disabled = on || !canApprove || !rows.length;
        el('soaApproveCancel').disabled = on || !canApprove || !rows.length;
    }
    async function read(url, options) {
        var r = await fetch(url, Object.assign({credentials:'same-origin',headers:{Accept:'application/json'}},options || {}));
        if (r.redirected || r.status === 401) throw new Error('Your session has expired. Sign in again and reopen the Approval Dashboard.');
        var text = await r.text(), d;
        try { d = JSON.parse(text); } catch(e) { throw new Error('The server could not load this approval screen. Please try again.'); }
        if (!r.ok) throw new Error(d.message || 'The request failed (' + r.status + ').');
        return d;
    }
    function cell(row, c) {
        var v = row[c[0]];
        if (c[2] === 'number' && v !== '' && v != null && Number.isFinite(Number(v))) v = Number(v).toLocaleString(undefined,{maximumFractionDigits:4});
        else if (/Date$/.test(c[0]) && v) v = String(v).replace('T',' ').slice(0,c[0] === 'EntryDate' ? 16 : 10);
        else if (c[0] === 'IsCanceled') v = v === true || v === 1 ? 'Yes' : 'No';
        return '<td' + (c[2] ? ' class="number"' : '') + '>' + esc(v) + '</td>';
    }
    function button(action, label, id) { return '<button type="button" data-action="'+action+'" data-id="'+id+'">'+label+'</button>'; }
    function showDetails(id) {
        document.querySelectorAll('#soaOrders tbody tr').forEach(function(r) { r.classList.toggle('selected',Number(r.dataset.id) === id); });
        el('soaDetails').querySelector('thead').innerHTML = '<tr>'+detailColumns.map(function(c){return '<th>'+c[1]+'</th>';}).join('')+'</tr>';
        el('soaDetails').querySelector('tbody').innerHTML = (lines[id] || []).map(function(r){return '<tr>'+detailColumns.map(function(c){return cell(r,c);}).join('')+'</tr>';}).join('');
    }
    function render() {
        el('soaOrders').querySelector('thead').innerHTML = '<tr><th><input type="checkbox" id="soaAll" aria-label="Select all orders"></th><th>Actions</th>'
            +headerColumns.map(function(c){return '<th>'+c[1]+'</th>';}).join('')+'</tr>';
        el('soaOrders').querySelector('tbody').innerHTML = rows.map(function(r) {
            var id = Number(r.Id), actions = '';
            if (loadedFilter.status === 'Open') {
                if (loadedFilter.approved === 'Not Approved') actions = button('Approve','Approve',id)+button('ApprovedAndCancel','Approve &amp; cancel',id);
                if (loadedFilter.approved === 'Approved' && rights.complete) actions += button('Status','Complete',id);
                if (loadedFilter.approved === 'Approved' && rights.cancel) actions += button('Cancel','Cancel',id);
            }
            return '<tr data-id="'+id+'"><td><input type="checkbox" data-select="'+id+'" aria-label="Select order '+esc(r.DocNo)+'"></td><td>'+actions+'</td>'
                +headerColumns.map(function(c){return cell(r,c);}).join('')+'</tr>';
        }).join('');
        el('soaCount').textContent = rows.length ? rows.length+' order(s)' : 'No orders match these filters.';
        showDetails(rows.length ? Number(rows[0].Id) : 0);
    }
    async function load(preserveMessage) {
        var f = filter();
        if (!f.toDate || (f.fromDate && f.fromDate > f.toDate)) { message('Select a valid From Date and To Date.',true); return; }
        controls(true); if (!preserveMessage) message('Loading orders…');
        try { var d = await read(API+'/history?'+query(f)); rows = d.rows || []; lines = d.lines || {}; rights = d.rights || {}; loadedFilter=f; render(); if (!preserveMessage) message(''); }
        catch(e) { rows=[]; lines={}; loadedFilter=null; render(); message(e.message,true); }
        finally { controls(false); }
    }
    async function update(action, ids) {
        if (busy) return;
        if (!ids.length) { message('Select at least one order first.',true); return; }
        if (JSON.stringify(filter()) !== JSON.stringify(loadedFilter)) { message('Filters have changed. Press Show before updating orders.',true); return; }
        var label = {Approve:'approve',ApprovedAndCancel:'approve and cancel',Status:'complete',Cancel:'cancel'}[action];
        if (!confirm('Are you sure you want to '+label+' '+ids.length+' selected order(s)?')) return;
        controls(true); message('Updating orders…');
        try {
            var headers = {'Accept':'application/json','Content-Type':'application/json'};
            var csrf = document.querySelector('meta[name="_csrf"]'), name = document.querySelector('meta[name="_csrf_header"]');
            if (csrf && name) headers[name.content] = csrf.content;
            var d = await read(API+'/update',{method:'POST',headers:headers,body:JSON.stringify({filter:loadedFilter,ids:ids,action:action})});
            message(d.message); await load(true);
        } catch(e) { message(e.message,true); } finally { controls(false); }
    }
    function selected() { return Array.from(document.querySelectorAll('#soaOrders [data-select]:checked')).map(function(c){return Number(c.dataset.select);}); }
    el('soaFilters').addEventListener('submit',function(e){e.preventDefault(); if(!busy) load();});
    el('soaRefresh').addEventListener('click',function(){if(!busy) load();});
    el('soaReset').addEventListener('click',function(){el('soaCustomer').value='0';el('soaItem').value='0';el('soaApproved').value='Not Approved';el('soaStatus').value='Open';load();});
    el('soaApprove').addEventListener('click',function(){update('Approve',selected());});
    el('soaApproveCancel').addEventListener('click',function(){update('ApprovedAndCancel',selected());});
    el('soaOrders').addEventListener('change',function(e){if(e.target.id === 'soaAll') document.querySelectorAll('#soaOrders [data-select]').forEach(function(c){c.checked=e.target.checked;});});
    el('soaOrders').addEventListener('click',function(e){
        var action = e.target.closest('[data-action]');
        if(action){update(action.dataset.action,[Number(action.dataset.id)]);return;}
        var row = e.target.closest('tr[data-id]'); if(row) showDetails(Number(row.dataset.id));
    });
    document.addEventListener('keydown',function(e){
        if(e.key === 'Escape') location.assign('/dashboard/approval-dashboard');
        if(e.ctrlKey && e.key === 'F5'){e.preventDefault();if(!busy)load();}
    });
    async function boot() {
        controls(true);
        try {
            el('soaFrom').value = qs.get('fromDate') || '';
            el('soaTo').value = qs.get('toDate') || new Date().toLocaleDateString('en-CA');
            var d = await read(API+'/setup?'+query(filter()));
            el('soaTitle').textContent = d.title; document.title=d.title;
            el('soaFrom').value=d.fromDate;el('soaTo').value=d.toDate;
            [['soaCustomer',d.customers,'All customers'],['soaItem',d.items,'All items']].forEach(function(a){
                el(a[0]).innerHTML='<option value="0">'+a[2]+'</option>'+(a[1]||[]).map(function(r){return '<option value="'+esc(r.id)+'">'+esc(r.name)+'</option>';}).join('');
            });
            await load();
        } catch(e) {message(e.message,true);el('soaCount').textContent='Could not load orders.';controls(false);}
    }
    boot();
}());
