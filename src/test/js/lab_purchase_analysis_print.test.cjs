// Run with NODE_PATH pointing to jsdom. No ERP records or messages are created.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { JSDOM, VirtualConsole } = require('jsdom');
const root = path.resolve(__dirname, '../../main/resources');
const html = fs.readFileSync(path.join(root, 'templates/lab/purchase_analysis.html'), 'utf8');
const page = fs.readFileSync(path.join(root, 'static/build/js/lab/countx_lab_purchase_analysis.js'), 'utf8');
const router = fs.readFileSync(path.join(root, 'static/js/print-rpt.js'), 'utf8');
const busy = fs.readFileSync(path.join(root, 'static/build/js/countx_purchase_request.js'), 'utf8');
const API = '/api/lab/purchase-analysis';
const slip653 = '/reports/print/653-inv-lab-purchase-analysis-slip';
const slip257 = '/reports/print/257-inward-gate-pass-with-wb-and-lab-slip';
const flush = async () => { for (let i = 0; i < 8; i++) await new Promise(setImmediate); };
const deferred = () => { let resolve; const promise = new Promise(r => { resolve = r; }); return { promise, resolve }; };

async function setup(options = {}) {
    const dom = new JSDOM(html, { url: 'http://localhost:8025/quality/purchase-analysis?id=901', runScripts: 'outside-only', virtualConsole: new VirtualConsole() });
    const w = dom.window, d = w.document;
    await new Promise(resolve => w.addEventListener('load', resolve, { once: true }));
    const calls = [], alerts = [], windows = [], events = [];
    const row = { Id: 901, DocNo: 446, DocDate: '2026-10-13', GatePassInwardId: 437, GpSrNo: 437, InvLabSampleLogRegisterId: 0, ItemId: 0, IsAccepted: true, rows: [] };
    w.alert = message => alerts.push(message);
    w.confirm = () => options.confirm !== false;
    w.URL.createObjectURL = () => 'blob:mock-pdf';
    w.open = () => {
        events.push('open');
        if (options.blocked) return null;
        const win = { location: '', closed: false, close() { this.closed = true; }, document: { open() {}, write() {}, close() {} } };
        windows.push(win); return win;
    };
    w.fetch = async (url, request = {}) => {
        const u = new URL(url, w.location.href);
        events.push(u.pathname);
        calls.push({ path: u.pathname, query: u.searchParams, body: request.body && JSON.parse(request.body) });
        let value, status = 200;
        if (u.pathname === API + '/lookups') value = { rights: { save: true, update: true, print: true }, docNo: 446, gatePasses: [], history: {} };
        else if (u.pathname === API + '/new') value = { docNo: 447, gatePasses: [] };
        else if (u.pathname === API + '/901' || u.pathname === API + '/903') { if (options.rowWait && u.pathname.endsWith('/903')) await options.rowWait.promise; value = row; }
        else if (u.pathname.startsWith(API + '/gate-pass/')) value = { rows: [], items: [] };
        else if (u.pathname.startsWith(API + '/sample/')) value = { found: false };
        else if (u.pathname.endsWith('/print-check')) {
            if (options.checkWait) await options.checkWait.promise;
            status = options.denied ? 403 : 200;
            value = options.denied ? { message: 'Print permission denied' } : { success: true };
        }
        else if (u.pathname === API + '/history') value = [{ ...row }, { ...row, Id: 903, DocNo: 448 }];
        else if (u.pathname === API + '/save') {
            status = options.saveStatus || 200;
            value = status === 409 ? { confirm: true, message: 'Confirm save?' } : status === 400 ? { message: 'Validation failed' } : { id: 902, gatePassId: 438, message: 'Saved' };
        }
        else if (u.pathname === '/reports/print/controls') {
            const lab = u.searchParams.get('rpt').startsWith('653-');
            value = { pdf: lab ? slip653 : slip257, args: lab ? ['history', 'fromDate', 'toDate', 'accountId'] : ['id'], controls: { fromDate: ['FromDateHistory'], toDate: ['ToDateHistory'], accountId: ['CmbCustomerHistory'] } };
        }
        else if ([slip653, slip257].includes(u.pathname)) {
            if (options.pdfWait) await options.pdfWait.promise;
            return { ok: true, headers: { get: () => 'application/pdf' }, blob: async () => new w.Blob(['PDF']) };
        }
        else throw new Error('Unexpected request: ' + url);
        return { ok: status < 400, status, text: async () => JSON.stringify(value), json: async () => value };
    };
    w.eval(busy); w.eval(router); w.eval(page); await flush();
    assert.deepEqual(alerts, [], 'Page bootstrap should succeed');
    const pdfs = () => calls.filter(c => [slip653, slip257].includes(c.path));
    return { dom, w, d, calls, alerts, windows, events, pdfs };
}

test('653 reserves a single window before validation and stays busy until its PDF finishes', async () => {
    const checkWait = deferred(), pdfWait = deferred();
    const h = await setup({ checkWait, pdfWait });
    try {
        const btn = h.d.getElementById('btnprint');
        h.events.length = 0; btn.click(); btn.click();
        assert.equal(h.windows.length, 1);
        assert.deepEqual(h.events.slice(0, 2), ['open', API + '/901/print-check']);
        assert.equal(btn.classList.contains('btn-busy'), true);
        checkWait.resolve(); await flush();
        assert.equal(btn.classList.contains('btn-busy'), true);
        assert.deepEqual([...h.pdfs()[0].query], [['history', '901']]);
        pdfWait.resolve(); await flush();
        assert.equal(btn.classList.contains('btn-busy'), false);
        assert.equal(h.windows[0].location, 'blob:mock-pdf');
        assert.deepEqual(h.alerts, []);
    } finally { h.dom.window.close(); }
});

test('History row Print reserves its preview before asynchronously selecting a different record', async () => {
    const rowWait = deferred();
    const h = await setup({ rowWait });
    try {
        h.d.getElementById('btnShowHistory').click(); await flush();
        const btn = h.d.querySelectorAll('#grdhistory [data-act="Print"]')[1];
        h.events.length = 0; btn.click();
        assert.equal(h.events[0], 'open');
        assert.equal(h.windows.length, 1);
        assert.equal(h.pdfs().length, 0);
        rowWait.resolve(); await flush();
        assert.deepEqual([...h.pdfs()[0].query], [['history', '903']]);
        assert.equal(h.windows.length, 1);
    } finally { h.dom.window.close(); }
});

test('257 prints the selected gate-pass ID, not the lab document ID', async () => {
    const h = await setup();
    try {
        h.d.getElementById('btnSlip257').click(); await flush();
        assert.equal(h.pdfs()[0].path, slip257);
        assert.deepEqual([...h.pdfs()[0].query], [['id', '437']]);
    } finally { h.dom.window.close(); }
});

test('failed print permission closes the reserved window and unlocks Print', async () => {
    const h = await setup({ denied: true });
    try {
        h.d.getElementById('btnprint').click(); await flush();
        assert.equal(h.windows[0].closed, true);
        assert.equal(h.pdfs().length, 0);
        assert.deepEqual(h.alerts, ['Print permission denied']);
        assert.equal(h.d.getElementById('btnprint').classList.contains('btn-busy'), false);
    } finally { h.dom.window.close(); }
});

test('blocked popups report how to enable previews without starting report requests', async () => {
    const h = await setup({ blocked: true });
    try {
        h.calls.length = 0;
        h.d.getElementById('btnprint').click(); await flush();
        h.d.getElementById('btnSlip257').click(); await flush();
        assert.equal(h.calls.length, 0);
        assert.equal(h.alerts.length, 2);
        assert.match(h.alerts[0], /Allow pop-ups/);
    } finally { h.dom.window.close(); }
});

for (const action of ['Save', 'Update']) test(action + ' opens both previews before POST and retains returned IDs after reset', async () => {
    const h = await setup();
    try {
        h.d.getElementById('IsPrint').checked = true; h.d.getElementById('chkPrintII').checked = true;
        h.events.length = 0;
        h.d.getElementById('btn' + action).click(); await flush();
        assert.deepEqual(h.events.slice(0, 3), ['open', 'open', API + '/save']);
        assert.deepEqual(h.pdfs().map(c => [c.path, [...c.query]]), [[slip653, [['history', '902']]], [slip257, [['id', '438']]]]);
        assert.equal(h.windows.length, 2);
        assert.ok(h.windows.every(win => win.location === 'blob:mock-pdf' && !win.closed));
        assert.equal(h.d.getElementById('txtdocno').value, '447');
    } finally { h.dom.window.close(); }
});

for (const saveStatus of [400, 409]) test('failed or cancelled save closes preview tabs: ' + saveStatus, async () => {
    const h = await setup({ saveStatus, confirm: false });
    try {
        h.d.getElementById('IsPrint').checked = true; h.d.getElementById('chkPrintII').checked = true;
        h.d.getElementById('btnUpdate').click(); await flush();
        assert.equal(h.windows.length, 2);
        assert.ok(h.windows.every(win => win.closed));
        assert.equal(h.pdfs().length, 0);
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 1);
    } finally { h.dom.window.close(); }
});

test('unchecked preview options save without opening tabs or printing', async () => {
    const h = await setup();
    try {
        h.d.getElementById('IsPrint').checked = false; h.d.getElementById('chkPrintII').checked = false;
        h.d.getElementById('btnUpdate').click(); await flush();
        assert.equal(h.windows.length, 0);
        assert.equal(h.pdfs().length, 0);
        assert.equal(h.calls.filter(c => c.path === API + '/save').length, 1);
    } finally { h.dom.window.close(); }
});
