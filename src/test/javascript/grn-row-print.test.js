const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');

(async () => {
    const elements = new Map(), requests = [];
    const element = id => {
        if (!elements.has(id)) elements.set(id, {
            value: '', listeners: {},
            addEventListener(event, action) { this.listeners[event] = action; }
        });
        return elements.get(id);
    };
    const window = { open: () => ({ location: {}, close() {} }), alert() {} };
    const context = vm.createContext({ window,
        document: { getElementById: element, addEventListener() {} },
        URL: { createObjectURL: () => 'blob:preview', revokeObjectURL() {} }, setTimeout() {},
        fetch: async (url, options) => {
            requests.push({ url, options, body: JSON.parse(options.body) });
            return { ok: true, headers: { get: () => 'application/pdf' }, blob: async () => ({}) };
        }
    });
    vm.runInContext(fs.readFileSync('src/main/resources/static/build/js/purchase/reports/pr_common.js', 'utf8'), context);
    const PR = context.PR = window.PR;
    PR.Picker = function () { this.value = () => '2026-10-05'; };
    PR.CheckList = function () { this.ids = () => [1]; };
    let gridActions;
    PR.Grid = function () { this.show = (_columns, _rows, actions) => { gridActions = actions; }; };
    PR.run = () => {}; // Start with the existing GRN grid loaded, without initialization requests.
    PR.gridTools = PR.enterAsTab = PR.fullscreen = PR.digitsOnly = () => {};
    PR.val = () => 0;
    PR.text = () => '';
    PR.request = async () => ({ rows: [{ Id: 20, DocumentTypeId: 46 }] });
    vm.runInContext(fs.readFileSync('src/main/resources/static/build/js/purchase/reports/grn_report.js', 'utf8'), context);
    let pending;
    PR.run = action => (pending = action());
    element('btnshow').listeners.click();
    await pending;
    for (const row of [{ Id: 20, DocumentTypeId: 46 }, { Id: 21 }, { Id: 22, DocumentTypeId: 217 }]) {
        gridActions.onButton('Print', row);
        await pending;
        const request = requests.at(-1);
        assert.equal(request.url, '/reports/print/211-goods-receipts-notes-rice-slip');
        assert.equal(request.options.credentials, 'same-origin');
        assert.deepEqual(request.body, { id: row.Id, documentTypeId: row.DocumentTypeId || 46 });
    }
    assert.equal(requests.length, 3);
    console.log('GRN row Print uses the Purchase controller with the selected row ID and document type.');
})().catch(error => { console.error(error); process.exitCode = 1; });
