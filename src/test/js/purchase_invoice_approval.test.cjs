const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const {JSDOM}=require('jsdom');
const root='src/main/resources/';
const html=fs.readFileSync(root+'templates/dashboard/purchase_invoice_for_approval.html','utf8');
const js=fs.readFileSync(root+'static/build/js/countx_purchase_invoice_for_approval.js','utf8');
const flush=async()=>{for(let i=0;i<8;i++)await new Promise(setImmediate);};
async function setup(options={}) {
    const dom=new JSDOM(html,{url:'http://localhost:8025/dashboard/purchase-invoice-for-approval?documentTypeId=56&branchId=58&fromDate=2026-10-06&toDate=2026-10-13'+(options.undo?'&unApprove=true':''),runScripts:'outside-only'});
    const w=dom.window,calls=[],previews=[],confirmations=[];let approved=false;
    w.confirm=text=>{confirmations.push(text);return options.confirm!==false;};
    w.open=()=>{const p={document:{body:{}},location:{replace(url){this.url=url;}}};previews.push(p);return p;};
    w.URL.createObjectURL=()=> 'blob:pdf';w.URL.revokeObjectURL=()=>{};
    w.fetch=async(url,request={})=>{
        const u=new URL(url,w.location.href),body=request.body&&JSON.parse(request.body);calls.push({url:u,body});
        let d,status=200;
        if(u.pathname.endsWith('/setup')) {d={fromDate:'2026-10-06',toDate:'2026-10-13',yearStart:'2026-01-01',suppliers:[],items:[]};if(options.denied){status=403;d={message:'No approval permission'};}}
        else if(u.pathname.endsWith('/history'))d={rows:approved?[]:[{Id:9,InvoiceNo:439,VoucherHeadId:8801,SupplierName:'Rice <script>',BillAmount:100}],lines:{9:[{ItemName:'Paddy',ItemQty:50},{ItemName:'Rice',ItemQty:10}]}};
        else if(u.pathname.endsWith('/approve') || u.pathname.endsWith('/unapprove')){if(options.wait)await options.wait;approved=true;d={message:options.undo?'Approval undone for 1 invoice(s) successfully':'Approved 1 invoice(s) successfully'};if(options.failure){status=503;d={message:'Connection interrupted. Refresh the list.'};}if(options.restricted){approved=false;status=400;d={message:'This invoice has been used in settled production'};}}
        else if(u.pathname.startsWith('/reports/print/'))return {ok:true,status:200,headers:{get:()=> 'application/pdf'},blob:async()=>new w.Blob(['pdf'])};
        else throw Error('Unexpected '+url);
        return {ok:status===200,status,json:async()=>d};
    };
    w.eval(js);await flush();return {dom,w,d:w.document,calls,previews,confirmations};
}
test('dashboard Purchase Invoice card links to type 56 with current filters',async()=>{
    const dom=new JSDOM(fs.readFileSync(root+'templates/dashboard/approval_dashboard.html','utf8'),{url:'http://localhost:8025/dashboard/approval-dashboard',runScripts:'outside-only'});
    const w=dom.window;
    w.fetch=async(url)=>({ok:true,json:async()=>url.includes('/setup')?{fromDate:'2026-10-06',toDate:'2026-10-13',currentBranchId:58,branches:[{BranchId:58,BranchName:'Branch'}]}:{inventory:[{id:23,title:'Purchase Invoice',count:1}]}});
    w.eval(fs.readFileSync(root+'static/build/js/countx_approval_dashboard.js','utf8'));await flush();
    const link=w.document.querySelector('#adInventory a');assert.ok(link);
    const u=new URL(link.href);assert.equal(u.pathname,'/dashboard/purchase-invoice-for-approval');
    assert.equal(u.searchParams.get('documentTypeId'),'56');assert.equal(u.searchParams.get('branchId'),'58');
    assert.equal(u.searchParams.get('fromDate'),'2026-10-06');assert.equal(u.searchParams.get('toDate'),'2026-10-13');dom.window.close();
});
test('loads pending invoices and details without sending an approval',async()=>{
    const s=await setup();assert.equal(s.d.querySelectorAll('#piaInvoices tbody tr').length,1);
    assert.equal(s.d.querySelectorAll('#piaDetails tbody tr').length,2);assert.equal(s.d.querySelector('#piaInvoices tbody script'),null);
    assert.equal(s.calls.filter(c=>c.url.pathname.endsWith('/approve')).length,0);s.dom.window.close();
});
test('approval permission denial leaves all actions disabled',async()=>{
    const s=await setup({denied:true});assert.equal(s.d.getElementById('piaApprove').disabled,true);
    assert.match(s.d.getElementById('piaMessage').textContent,/permission/);assert.equal(s.calls.length,1);s.dom.window.close();
});
test('changed filters and cancelled confirmation never send approval',async()=>{
    const s=await setup({confirm:false});s.d.querySelector('[data-action=approve]').click();await flush();
    s.d.getElementById('piaFrom').value='2026-10-01';s.d.querySelector('[data-action=approve]').click();await flush();
    assert.match(s.d.getElementById('piaMessage').textContent,/Filters have changed/);
    assert.equal(s.calls.filter(c=>c.url.pathname.endsWith('/approve')).length,0);s.dom.window.close();
});
test('selected approval submits exact ids once and refreshes the pending list',async()=>{
    let release;const wait=new Promise(r=>release=r),s=await setup({wait});
    s.d.querySelector('[data-select]').checked=true;s.d.getElementById('piaApprove').click();s.d.getElementById('piaApprove').click();
    assert.equal(s.d.getElementById('piaApprove').disabled,true);release();await flush();
    const posts=s.calls.filter(c=>c.url.pathname.endsWith('/approve'));assert.equal(posts.length,1);assert.deepEqual(posts[0].body.ids,[9]);
    assert.equal(posts[0].body.filter.branchId,58);assert.equal(s.d.querySelectorAll('#piaInvoices tbody tr').length,0);
    assert.match(s.d.getElementById('piaMessage').textContent,/Approved 1/);s.dom.window.close();
});
test('uncertain approval response refreshes before allowing another action',async()=>{
    const s=await setup({failure:true});s.d.querySelector('[data-action=approve]').click();await flush();
    assert.equal(s.d.getElementById('piaApprove').disabled,true);assert.match(s.d.getElementById('piaMessage').textContent,/Connection interrupted/);
    assert.equal(s.calls.filter(c=>c.url.pathname.endsWith('/approve')).length,1);s.dom.window.close();
});
test('voucher preview uses linked voucher id, party slip uses invoice id',async()=>{
    const s=await setup();s.d.querySelector('[data-action=voucher]').click();await flush();s.d.querySelector('[data-action=slip]').click();await flush();
    const prints=s.calls.filter(c=>c.url.pathname.startsWith('/reports/print/'));assert.deepEqual(prints[0].body,{id:8801,documentTypeId:56});assert.deepEqual(prints[1].body,{id:9});
    assert.equal(s.previews[0].location.url,'blob:pdf');s.dom.window.close();
});

test('Undo Approval dashboard links Purchase Invoice to undo mode with its date filters',async()=>{
    const dom=new JSDOM(fs.readFileSync(root+'templates/dashboard/un_approved_invoices_and_vouchers.html','utf8'),{url:'http://localhost:8025/dashboard/un-approved-invoices-and-vouchers',runScripts:'outside-only'});
    try {
        const w=dom.window,calls=[];
        w.fetch=async(url)=>{calls.push(url);return {ok:true,json:async()=>url.includes('/defaults')?{fromDate:'2026-10-07',toDate:'2026-10-14'}:{inventory:[{id:23,title:'Purchase Invoice',count:1}]}};};
        w.eval(fs.readFileSync(root+'static/build/js/countx_un_approved_invoices_and_vouchers.js','utf8'));await flush();
        const link=w.document.querySelector('#uaInventory a');assert.ok(link);
        let u=new URL(link.href);assert.equal(u.pathname,'/dashboard/purchase-invoice-for-approval');
        assert.equal(u.searchParams.get('unApprove'),'true');assert.equal(u.searchParams.get('documentTypeId'),'56');
        assert.equal(u.searchParams.get('fromDate'),'2026-10-07');assert.equal(u.searchParams.get('toDate'),'2026-10-14');
        w.document.getElementById('chkFrom').checked=false;
        assert.equal(w.uaCardClick('inventory',0,{currentTarget:link}),true);
        u=new URL(link.href);assert.equal(u.searchParams.has('fromDate'),false);
        assert.equal(w.document.getElementById('uaMessage').textContent,'');
        assert.ok(calls.every(url=>!url.includes('/unapprove')));
    } finally {dom.window.close();}
});

test('undo mode lists approved invoices and labels its actions and return link correctly',async()=>{
    const s=await setup({undo:true});
    try {
        assert.equal(s.d.getElementById('piaTitle').textContent,'Undo Purchase Invoice Approval');
        assert.equal(s.d.getElementById('piaBack').getAttribute('href'),'/dashboard/un-approved-invoices-and-vouchers');
        assert.equal(s.d.getElementById('piaApprove').textContent,'Undo approval for selected');
        assert.equal(s.d.querySelector('[data-action=approve]').textContent,'Undo approval');
        assert.match(s.d.getElementById('piaCount').textContent,/1 approved invoice/);
        assert.equal(s.calls.find(c=>c.url.pathname.endsWith('/history')).url.searchParams.get('unApprove'),'true');
        assert.equal(s.calls.filter(c=>c.body).length,0);
    } finally {s.dom.window.close();}
});

test('undo requires confirmation and permission',async()=>{
    for(const options of [{undo:true,confirm:false},{undo:true,denied:true}]) {
        const s=await setup(options);
        try {
            if(options.denied)assert.equal(s.d.getElementById('piaApprove').disabled,true);
            else {s.d.querySelector('[data-action=approve]').click();await flush();assert.match(s.confirmations[0],/^Undo approval for/);}
            assert.equal(s.calls.filter(c=>c.body).length,0);
        } finally {s.dom.window.close();}
    }
});

test('undo submits selected invoices once to its own endpoint and reloads the approved list',async()=>{
    let release;const wait=new Promise(r=>release=r),s=await setup({undo:true,wait});
    try {
        s.d.querySelector('[data-select]').checked=true;s.d.getElementById('piaApprove').click();s.d.getElementById('piaApprove').click();
        assert.equal(s.d.getElementById('piaApprove').disabled,true);release();await flush();
        const posts=s.calls.filter(c=>c.body);assert.equal(posts.length,1);
        assert.equal(posts[0].url.pathname,'/api/dashboard/purchase-invoice-approval/unapprove');
        assert.deepEqual(posts[0].body.ids,[9]);assert.equal(posts[0].body.filter.unApprove,true);
        assert.equal(s.d.querySelectorAll('#piaInvoices tbody tr').length,0);
        assert.match(s.d.getElementById('piaMessage').textContent,/Approval undone/);
    } finally {s.dom.window.close();}
});

test('a production restriction keeps the invoice listed and shows the database message',async()=>{
    const s=await setup({undo:true,restricted:true});
    try {
        s.d.querySelector('[data-action=approve]').click();await flush();
        assert.match(s.d.getElementById('piaMessage').textContent,/settled production/);
        assert.equal(s.d.querySelectorAll('#piaInvoices tbody tr').length,1);
        assert.equal(s.calls.filter(c=>c.body).length,1);
    } finally {s.dom.window.close();}
});
