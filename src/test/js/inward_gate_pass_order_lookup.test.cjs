// All requests are mocked; no gate passes or purchase orders are changed.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { JSDOM, VirtualConsole } = require('jsdom');
const resources = path.resolve(__dirname, '../../main/resources');
const html = fs.readFileSync(path.join(resources, 'templates/purchase/inward_gate_pass.html'), 'utf8');
const script = name => fs.readFileSync(path.join(resources, 'static/build/js', name), 'utf8');

async function setup() {
    const dom = new JSDOM(html, { url: 'http://localhost:8025/purchase/inward-gate-pass', runScripts: 'outside-only', virtualConsole: new VirtualConsole() });
    const w = dom.window, d = w.document, requests = [], alerts = [];
    await new Promise(resolve => w.addEventListener('load', resolve, { once: true }));
    const field = id => d.getElementById(id);
    const select = (id, value, text) => field(id).add(new w.Option(text, value, true, true));
    let reply = async () => ({ data: [{ Id: 7, CompanyName: 'Test supplier', ItemId: 8, ItemName: 'Test item', PurchaseOrderId: 95339 }] });
    w.alert = message => alerts.push(message);
    w.fetch = async url => {
        requests.push(url);
        const result = url.includes('/order-party-items?') ? await reply(url) : { data: [] };
        return { ok: !result.status, status: result.status || 200, text: async () => JSON.stringify(result.data) };
    };
    w.eval(script('countx_purchase_request.js'));
    w.eval(script('countx_inward_gate_pass.js'));
    select('cmbgptype', '1', 'Paddy'); select('CmbOrderType', '41', 'Purchase Order');
    field('CmbOrderno').disabled = false;
    field('CmbOrderno').value = '451';
    field('txtgpdate').value = '2026-10-14'; field('txtId').value = '901';
    return { dom, w, field, select, requests, alerts, setReply(fn) { reply = fn; } };
}

for (const blank of ['', '0']) {
    test(`leaving order ${JSON.stringify(blank)} clears dependent fields without a request or popup`, async () => {
        const h = await setup();
        try {
            await h.w.onOrderNumberLeave();
            assert.equal(h.field('cmbsupp').value, '7');
            h.select('cmbTransitVehicle', '12', 'Old transit');
            h.requests.length = 0;
            h.field('CmbOrderno').value = blank;
            await h.w.onOrderNumberLeave();
            assert.deepEqual(h.requests, []); assert.deepEqual(h.alerts, []);
            for (const id of ['cmbsupp', 'CmbVariety', 'cmbTransitVehicle', 'CmbOrderno']) assert.equal(h.field(id).value, '');
        } finally { h.dom.window.close(); }
    });
}

test('a valid order loads supplier and item and uses the saved order id for transit vehicles', async () => {
    const h = await setup();
    try {
        await h.w.onOrderNumberLeave();
        assert.equal(h.field('cmbsupp').value, '7'); assert.equal(h.field('CmbVariety').value, '8');
        const query = new URL(h.requests[0], h.w.location.origin).searchParams;
        assert.equal(query.get('number'), '451'); assert.equal(query.get('gatePassId'), '901');
        assert.ok(h.requests[1].includes('orderId=95339')); assert.deepEqual(h.alerts, []);
    } finally { h.dom.window.close(); }
});

test('business validation displays the specific message and clears the previous order', async () => {
    const h = await setup();
    try {
        await h.w.onOrderNumberLeave();
        h.setReply(async () => ({ status: 400, data: { message: 'Purchase Order is not approved' } }));
        h.field('CmbOrderno').value = '452';
        await h.w.onOrderNumberLeave();
        assert.deepEqual(h.alerts, ['Purchase Order is not approved']);
        assert.equal(h.field('cmbsupp').value, ''); assert.equal(h.field('CmbVariety').value, '');
    } finally { h.dom.window.close(); }
});

test('an older failed request cannot erase a newer valid order', async () => {
    const h = await setup();
    try {
        let rejectOld;
        h.setReply(() => new Promise((resolve, reject) => { rejectOld = reject; }));
        const old = h.w.onOrderNumberLeave();
        h.field('CmbOrderno').value = '452';
        h.setReply(async () => ({ data: [{ Id: 9, CompanyName: 'New supplier', ItemId: 10, ItemName: 'New item', PurchaseOrderId: 95340 }] }));
        await h.w.onOrderNumberLeave();
        rejectOld(new Error('Old request failed')); await old;
        assert.equal(h.field('CmbOrderno').value, '452');
        assert.equal(h.field('cmbsupp').value, '9'); assert.equal(h.field('CmbVariety').value, '10');
        assert.deepEqual(h.alerts, []);
    } finally { h.dom.window.close(); }
});
