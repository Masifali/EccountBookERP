const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');

(async () => {
    const elements = new Map(), requests = [], messages = [];
    const element = id => {
        if (!elements.has(id)) elements.set(id, {
            value: id === 'CmbParentCategory' ? '1' : '', listeners: {},
            addEventListener(event, action) { this.listeners[event] = action; }
        });
        return elements.get(id);
    };
    const window = { open: () => ({ location: {}, close() {} }), alert: text => messages.push(text) };
    const context = vm.createContext({ window,
        document: { getElementById: element, addEventListener() {} },
        URL: { createObjectURL: () => 'blob:preview', revokeObjectURL() {} },
        setTimeout() {},
        fetch: async (url, options) => {
            requests.push({ url, options, body: JSON.parse(options.body) });
            return { ok: true, headers: { get: () => 'application/pdf' }, blob: async () => ({}) };
        }
    });
    vm.runInContext(fs.readFileSync('src/main/resources/static/build/js/purchase/reports/pr_common.js', 'utf8'), context);
    const PR = window.PR;
    context.PR = PR;
    PR.Picker = function () { this.value = () => '2026-10-05'; };
    PR.Grid = function () { this.show = () => {}; this.clear = () => {}; };
    PR.run = () => {}; // Initialization is unrelated to printing the already loaded report.
    PR.gridTools = PR.enterAsTab = () => {};
    PR.text = () => 'Paddy';
    let result = {
        rawDetail: [{ ItemName: 'Detail rice', NetWeight: 6021.25 }],
        rawSummary: [{ ItemName: 'Summary rice', TotalNetWeight: 6021.25 }]
    };
    PR.request = async () => result;
    context.LabRep = { run: (_button, action) => action(), fullscreen() {} };
    vm.runInContext(fs.readFileSync('src/main/resources/static/build/js/lab/countx_lab_purchase_analysis_by_vehicle.js', 'utf8'), context);
    await element('btnshow').listeners.click();
    await element('Print').listeners.click();
    assert.equal(requests.length, 1);
    assert.equal(requests[0].url, '/reports/lab/purchase-analysis-by-vehicle');
    assert.equal(requests[0].options.credentials, 'same-origin');
    assert.deepEqual(requests[0].body, { detail: result.rawDetail, summary: result.rawSummary });
    result = { rawDetail: result.rawDetail, rawSummary: [] };
    await element('btnshow').listeners.click();
    await element('Print').listeners.click();
    assert.equal(requests.length, 1, 'An empty summary must not print a partial report');
    assert.equal(messages.at(-1), 'No Record Found For Display');
    console.log('665 print button sends both original datasets to the Lab controller and blocks empty reports.');
})().catch(error => { console.error(error); process.exitCode = 1; });
