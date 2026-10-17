// NODE_PATH must point to jsdom. All invoice, voucher and PDF requests are mocked.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { JSDOM, VirtualConsole } = require('jsdom');
const root = path.resolve(__dirname, '../../main/resources');
const html = fs.readFileSync(path.join(root, 'templates/purchase/purchase_invoice.html'), 'utf8');
const page = fs.readFileSync(path.join(root, 'static/build/js/countx_purchase_invoice.js'), 'utf8');
const busy = fs.readFileSync(path.join(root, 'static/build/js/countx_purchase_request.js'), 'utf8');
const API = '/api/purchase/purchase-invoice-full';
const flush = async () => { for (let i = 0; i < 8; i++) await new Promise(setImmediate); };
const deferred = () => { let resolve; const promise = new Promise(r => { resolve = r; }); return { promise, resolve }; };

async function setup(options = {}) {
    const dom = new JSDOM(html, { url: 'http://localhost:8025/purchase/purchase-invoice' + (options.newRecord ? '' : '?id=901'), runScripts: 'outside-only', virtualConsole: new VirtualConsole() });
    const w = dom.window, d = w.document;
    await new Promise(resolve => w.addEventListener('load', resolve, { once: true }));
    const calls = [], events = [], previews = [];
    const row = { Id: 901, DocNo: 439, VoucherHeadId: 8801, SupplierCustomerId: 1, DocDate: '2026-10-13', InvoiceTypeId: 1, IsApproved: !!options.approved,
        details: [{ GrnNo: 443, ItemName: 'Paddy', LocationTypeId: 1, ItemQty: 50, ItemAmount: 1000 }], freight: [], journal: [], expenses: [], emptyBags: [], paymentTerms: [] };
    w.PurchaseInvoiceAttachments = { create: () => ({ reset() {}, open() {}, payload: () => [], settled: () => options.attachmentWait ? options.attachmentWait.promise : Promise.resolve() }) };
    w.confirm = () => { events.push('confirm'); return options.confirm !== false; };
    let blob = 0;
    w.URL.createObjectURL = () => 'blob:report-' + (++blob);
    w.open = () => {
        events.push('open');
        if (options.blocked) return null;
        const previewDom = new JSDOM('<!doctype html><title>Print</title><body></body>', { url: w.location.href, virtualConsole: new VirtualConsole() });
        const preview = { document: previewDom.window.document, closed: false, location: { href: '' }, close() { this.closed = true; previewDom.window.close(); } };
        previews.push(preview); return preview;
    };
    w.fetch = async (url, request = {}) => {
        const u = new URL(url, w.location.href);
        events.push(u.pathname);
        calls.push({ path: u.pathname, body: request.body && JSON.parse(request.body) });
        let value, status = 200;
        if (u.pathname === API + '/dropdowns') value = { rights: { Save: true, Update: true, Delete: true, Print: true }, suppliers: [{ Id: 1, CompanyName: 'Sample supplier' }], locations: [{ Id: 1, Location: 'Pakistan' }], historyBranches: [] };
        else if (u.pathname === API + '/901' || u.pathname === API + '/grn-by-no') value = row;
        else if (u.pathname === API + '/902') {
            status = options.voucherFailure ? 500 : 200;
            value = options.voucherFailure ? { message: 'Voucher lookup failed' } : { VoucherHeadId: options.noVoucher ? 0 : 8802 };
        }
        else if (u.pathname === API + '/next-code') {
            status = options.resetFailure && calls.some(c => c.path === API + '/save') ? 500 : 200;
            value = status === 500 ? { message: 'Code lookup failed' } : { docNo: 440 };
        }
        else if (u.pathname === API + '/save') {
            if (options.saveWait) await options.saveWait.promise;
            status = options.saveFailure ? 400 : 200;
            value = status === 400 ? { message: 'Save validation failed' } : { id: 902, message: 'Record Saved Successfully [439]' };
        }
        else if (u.pathname.startsWith('/reports/print/by-template/')) {
            if (options.pdfWait) await options.pdfWait.promise;
            if (options.pdfFailure && u.pathname.includes(options.pdfFailure)) return { ok: false, status: 500, headers: { get: () => 'text/plain' }, text: async () => 'Report unavailable' };
            return { ok: true, status: 200, headers: { get: () => 'application/pdf' }, blob: async () => new w.Blob(['PDF']) };
        }
        else throw Error('Unexpected request: ' + url);
        return { ok: status < 400, status, text: async () => JSON.stringify(value) };
    };
    w.eval(busy); w.eval(page);
    d.dispatchEvent(new w.Event('DOMContentLoaded')); await flush();
    if (options.newRecord) {
        d.getElementById('txtGrnNo').value = '443';
        d.getElementById('txtGrnNo').dispatchEvent(new w.Event('blur')); await flush();
    }
    assert.equal(d.getElementById('fxMsg').textContent, '', 'Bootstrap and record load must succeed');
    const check = ids => { for (const id of ['ChkBokVoucher', 'ChkBoxParty', 'ChkBoxItemPrint', 'chkSummary']) d.getElementById(id).checked = ids.includes(id); };
    const pdfs = () => calls.filter(c => c.path.startsWith('/reports/print/by-template/')).map(c => [decodeURIComponent(c.path.split('/')[4]), c.body]);
    const close = () => { previews.forEach(p => { if (!p.closed) p.close(); }); w.close(); };
    return { w, d, calls, events, previews, check, pdfs, close, options };
}

for (const newRecord of [true, false]) test((newRecord ? 'Save' : 'Update') + ' reserves one preview before requests and prints saved invoice/voucher IDs after reset', async () => {
    const h = await setup({ newRecord });
    try {
        h.events.length = 0;
        h.d.getElementById(newRecord ? 'btnSave' : 'btnUpdate').click(); await flush();
        assert.deepEqual(h.events.slice(0, 2), ['confirm', 'open']);
        assert.equal(h.previews.length, 1);
        assert.deepEqual(h.pdfs(), [
            ['103-AcRptPurchaseSalesVoucherSlip.rpt', { id: 8802, documentTypeId: 56 }],
            ['220A-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: 902 }]
        ]);
        assert.equal(h.calls.find(c => c.path === API + '/save').body.Id, newRecord ? 0 : 901);
        assert.equal(h.d.getElementById('txtdocno').value, '440');
        const doc = h.previews[0].document;
        assert.equal(doc.querySelectorAll('iframe').length, 2);
        assert.equal(doc.querySelectorAll('section:not([hidden])').length, 1);
        doc.querySelectorAll('nav button')[1].click();
        assert.equal(doc.querySelector('section:not([hidden]) iframe').title, '220A-Party Slip');
        assert.match(h.d.getElementById('fxMsg').textContent, /Record Saved Successfully/);
    } finally { h.close(); }
});

test('captures all checked options before attachment/save waits and prevents duplicate saves', async () => {
    const attachmentWait = deferred(), pdfWait = deferred();
    const h = await setup({ attachmentWait, pdfWait });
    try {
        h.check(['ChkBokVoucher', 'ChkBoxParty', 'ChkBoxItemPrint', 'chkSummary']);
        const button = h.d.getElementById('btnUpdate');
        button.click(); button.click();
        assert.equal(h.previews.length, 1);
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 0);
        h.check([]); attachmentWait.resolve(); await flush();
        assert.equal(button.disabled, true);
        pdfWait.resolve(); await flush();
        assert.equal(h.pdfs().length, 4);
        assert.equal(h.previews[0].document.querySelectorAll('iframe').length, 4);
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 1);
    } finally { h.close(); }
});

test('unchecked print options save without opening a window or requesting PDFs', async () => {
    const h = await setup();
    try {
        h.check([]); h.d.getElementById('btnUpdate').click(); await flush();
        assert.equal(h.previews.length, 0); assert.equal(h.pdfs().length, 0);
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 1);
    } finally { h.close(); }
});

for (const options of [{ confirm: false }, { approved: true }, { saveFailure: true }]) test('cancelled or failed save closes reserved preview: ' + JSON.stringify(options), async () => {
    const h = await setup(options);
    try {
        h.d.getElementById('btnUpdate').click(); await flush();
        if (options.saveFailure) assert.equal(h.previews[0].closed, true);
        else assert.equal(h.previews.length, 0);
        assert.equal(h.pdfs().length, 0);
        assert.equal(h.d.getElementById('btnUpdate').disabled, false);
    } finally { h.close(); }
});

test('blocked preview provides a direct retry for the saved reports without saving again', async () => {
    const h = await setup({ blocked: true });
    try {
        h.d.getElementById('btnUpdate').click(); await flush();
        assert.equal(h.pdfs().length, 0);
        assert.match(h.d.getElementById('fxMsg').textContent, /Record Saved Successfully/);
        const retry = h.d.querySelector('#fxMsg button'); assert.ok(retry);
        h.options.blocked = false; retry.click(); await flush();
        assert.equal(h.pdfs().length, 2);
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 1);
    } finally { h.close(); }
});

for (const options of [{ voucherFailure: true }, { noVoucher: true }, { pdfFailure: '103-' }]) test('one failed voucher does not stop the party slip: ' + JSON.stringify(options), async () => {
    const h = await setup(options);
    try {
        h.d.getElementById('btnUpdate').click(); await flush();
        assert.ok(h.pdfs().some(([rpt, args]) => rpt.startsWith('220A-') && args.id === 902));
        assert.equal(h.pdfs().some(([, args]) => args.id === 0), false);
        assert.equal(h.previews[0].document.querySelectorAll('iframe').length, 1);
        assert.match(h.d.getElementById('fxMsg').textContent, /Record Saved Successfully.*Could not preview 103-Voucher/);
    } finally { h.close(); }
});

test('a next-number refresh failure cannot suppress printing a successfully saved invoice', async () => {
    const h = await setup({ newRecord: true, resetFailure: true });
    try {
        h.d.getElementById('btnSave').click(); await flush();
        assert.equal(h.pdfs().length, 2);
        assert.match(h.d.getElementById('fxMsg').textContent, /Record Saved Successfully.*could not reset/);
        assert.equal(h.d.getElementById('btnSave').hidden, true);
        assert.equal(h.d.getElementById('btnUpdate').hidden, false);
        assert.equal(h.w.location.search, '?id=902');
    } finally { h.close(); }
});

test('closing a preview while PDFs load leaves a way to reopen without saving again', async () => {
    const pdfWait = deferred(), h = await setup({ pdfWait });
    try {
        h.d.getElementById('btnUpdate').click(); await flush();
        h.previews[0].close(); pdfWait.resolve(); await flush();
        assert.ok(h.d.querySelector('#fxMsg button'));
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 1);
    } finally { h.close(); }
});

test('manual 103/104 printing still uses the opened accounting voucher', async () => {
    const h = await setup();
    try {
        h.d.getElementById('btnPrint').click(); await flush();
        h.d.getElementById('btn104Voucher').click(); await flush();
        assert.deepEqual(h.pdfs(), [
            ['103-AcRptPurchaseSalesVoucherSlip.rpt', { id: 8801, documentTypeId: 56 }],
            ['104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt', { id: 8801, documentTypeId: 56 }]
        ]);
        assert.ok(h.previews.every(p => p.location.href.startsWith('blob:report-')));
    } finally { h.close(); }
});
