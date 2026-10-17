// Run with NODE_PATH pointing to a jsdom installation. All requests are mocked.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { JSDOM, VirtualConsole } = require('jsdom');
const resources = path.resolve(__dirname, '../../main/resources');
const html = fs.readFileSync(path.join(resources, 'templates/purchase/inward_gate_pass.html'), 'utf8');
const requestJs = fs.readFileSync(path.join(resources, 'static/build/js/countx_purchase_request.js'), 'utf8');
const gatePassJs = fs.readFileSync(path.join(resources, 'static/build/js/countx_inward_gate_pass.js'), 'utf8');
const tick = () => new Promise(resolve => setImmediate(resolve));

async function setup({ update = false, canSave = true, canUpdate = true } = {}) {
    const dom = new JSDOM(html, { url: 'http://localhost:8025/purchase/inward-gate-pass', runScripts: 'outside-only', virtualConsole: new VirtualConsole() });
    const w = dom.window, d = w.document;
    // The page's initial lookup handler is not part of a user-triggered save.
    await new Promise(resolve => w.addEventListener('load', resolve, { once: true }));
    const requests = [], alerts = [], confirmations = [], afterSave = [];
    let reply = async () => ({ success: true, igpId: 901, message: 'Saved' });
    w.confirm = text => { confirmations.push(text); return true; };
    w.alert = text => alerts.push(text);
    w.fetch = async (url, options) => {
        let data;
        if (url === '/api/inward-gate-pass/dropdowns') data = { canSave, canUpdate };
        else {
            assert.equal(url, '/api/inward-gate-pass/save');
            assert.equal(options.method, 'POST');
            const payload = JSON.parse(options.body);
            requests.push(payload);
            data = await reply(payload);
        }
        return { ok: true, text: async () => JSON.stringify(data) };
    };
    w.eval(requestJs);
    w.eval(gatePassJs);
    await w.loadDropdowns();
    for (const id of ['btnsave', 'btnupdate']) {
        const button = d.getElementById(id);
        // Exercise the actual inline handler from the template.
        button.onclick = new w.Function(button.getAttribute('onclick'));
    }
    const values = { txtId: update ? '901' : '0', txtvehicleno: 'TEST-123', txtgpdate: '2026-10-13', txtqty: '500', txtPackUnit: '40', txtWeight: '20000', txtSupplierNetWeight: '20040' };
    for (const [id, value] of Object.entries(values)) d.getElementById(id).value = value;
    for (const [id, value, text] of [['cmbgptype', '1', 'Paddy'], ['cmbsupp', '7', 'Test supplier'], ['CmbVariety', '8', 'Test item'], ['cmbvehicletype', '1', 'Truck'], ['CmbOrderType', '41', 'Purchase Order'], ['CmbStatus', 'Open', 'Open']]) {
        const select = d.getElementById(id);
        select.add(new w.Option(text, value, true, true));
    }
    d.getElementById('btnsave').style.display = update ? 'none' : 'inline-flex';
    d.getElementById('btnupdate').style.display = update ? 'inline-flex' : 'none';
    d.getElementById('chkPreview').checked = false;
    d.getElementById('chkDriverInfoForm').checked = false;
    w.onNewRecord = async () => { afterSave.push('reset'); };
    w.loadMainHistoryGrid = async () => { afterSave.push('history'); };
    return { dom, w, d, requests, alerts, confirmations, afterSave, button: d.getElementById(update ? 'btnupdate' : 'btnsave'), setReply(fn) { reply = fn; } };
}

for (const update of [false, true]) {
    const action = update ? 'Update' : 'Save';
    test(`${action} click submits once while loading and restores the button`, async () => {
        const h = await setup({ update });
        try {
            let release;
            h.setReply(() => new Promise(resolve => { release = resolve; }));
            h.button.click();
            assert.equal(h.requests.length, 1, 'Save must not stop at the temporary disabled state');
            assert.equal(h.button.disabled, true);
            h.button.click();
            h.w.document.dispatchEvent(new h.w.KeyboardEvent('keydown', { key: update ? 'u' : 's', ctrlKey: true, bubbles: true }));
            assert.equal(h.requests.length, 1, 'A second action must not duplicate the request');
            assert.equal(h.requests[0].id, update ? 901 : 0);
            assert.equal(h.requests[0].noOfPackages, 500);
            assert.equal(h.requests[0].weightComparedToPoWt, 20000);
            release({ success: true, igpId: 901, message: 'Saved' });
            await tick();
            assert.deepEqual(h.afterSave, ['reset', 'history']);
            assert.deepEqual(h.alerts, ['Saved']);
            assert.equal(h.button.disabled, false);
            assert.equal(h.d.documentElement.hasAttribute('aria-busy'), false);
        } finally { h.dom.window.close(); }
    });

    test(`${action} keyboard shortcut reaches the same save flow`, async () => {
        const h = await setup({ update });
        try {
            h.d.dispatchEvent(new h.w.KeyboardEvent('keydown', { key: update ? 'u' : 's', ctrlKey: true, bubbles: true, cancelable: true }));
            await tick();
            assert.equal(h.requests.length, 1);
            assert.deepEqual(h.afterSave, ['reset', 'history']);
            assert.equal(h.button.disabled, false);
        } finally { h.dom.window.close(); }
    });

    test(`${action} remains blocked when its permission is missing`, async () => {
        const h = await setup({ update, canSave: update, canUpdate: !update });
        try {
            assert.equal(h.button.disabled, true);
            h.button.click();
            await h.w.withButtonLoading(h.button, h.w.onSaveRecord);
            await h.w.onSaveRecord();
            assert.equal(h.requests.length, 0);
            assert.equal(h.confirmations.length, 0);
            assert.equal(h.button.disabled, true);
        } finally { h.dom.window.close(); }
    });
}

test('cancelled confirmation sends nothing and allows another attempt', async () => {
    const h = await setup();
    try {
        h.w.confirm = () => false;
        h.button.click();
        await tick();
        assert.equal(h.requests.length, 0);
        assert.equal(h.button.disabled, false);
        h.w.confirm = () => true;
        h.button.click();
        await tick();
        assert.equal(h.requests.length, 1);
    } finally { h.dom.window.close(); }
});

test('request failure preserves the entry and re-enables retry', async () => {
    const h = await setup();
    try {
        h.setReply(async () => { throw new Error('Connection interrupted'); });
        h.button.click();
        await tick();
        assert.deepEqual(h.alerts, ['Connection interrupted']);
        assert.deepEqual(h.afterSave, []);
        assert.equal(h.d.getElementById('txtvehicleno').value, 'TEST-123');
        assert.equal(h.button.disabled, false);
        assert.equal(h.d.documentElement.hasAttribute('aria-busy'), false);
        h.setReply(async () => ({ success: true, igpId: 901, message: 'Saved' }));
        h.button.click();
        await tick();
        assert.equal(h.requests.length, 2);
        assert.deepEqual(h.afterSave, ['reset', 'history']);
    } finally { h.dom.window.close(); }
});
