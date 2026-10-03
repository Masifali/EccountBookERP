// Run with: node src/test/javascript/erp-print-routing.test.js
const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');

const script = fs.readFileSync('src/main/resources/static/js/print-rpt.js', 'utf8');
const calls = [], windows = [], messages = [];
let tables = [];
const document = {
    readyState: 'loading', title: 'Current report',
    addEventListener() {}, getElementById() { return null; }, getElementsByName() { return []; },
    querySelector() { return null; },
    querySelectorAll(selector) { return selector === 'table' ? tables : []; }
};
const window = { open() { const win = { location: '', close() { this.closed = true; } }; windows.push(win); return win; } };
const context = vm.createContext({ window, document, URLSearchParams,
    URL: { createObjectURL: () => 'blob:report' }, Proxy, Promise,
    setTimeout, clearTimeout, alert: message => messages.push(message),
    fetch: async (url, options) => {
        calls.push({ url, options });
        if (url.startsWith('/reports/print/controls?')) return { ok: true, json: async () => ({ args: ['id'], controls: {}, pdf: '/reports/print/273-sale-order-slip' }) };
        return { ok: true, headers: { get: () => 'application/pdf' }, blob: async () => new Uint8Array([1]) };
    }
});
function cell(text, visible = true) {
    return { textContent: text, querySelector: () => null, getClientRects: () => visible ? [1] : [] };
}
function row(cells, visible = true) {
    return { cells, style: {}, offsetParent: {}, getClientRects: () => visible ? [1] : [] };
}
function table(title, bodies, foot = null) {
    const heading = row([cell('Name'), cell('Hidden', false), cell('Amount')]);
    return { tagName: 'TABLE', caption: { textContent: title }, tHead: { rows: [heading] },
        tBodies: bodies.map(rows => ({ rows })), tFoot: foot && { rows: foot }, rows: [heading],
        getClientRects: () => [1], closest: () => null, querySelector: () => null };
}

(async () => {
    vm.runInContext(script, context);
    await window.printRpt('273-InvRptSaleOrderSlip.rpt', { id: 123 });
    assert.equal(calls[0].url, '/reports/print/controls?rpt=273-InvRptSaleOrderSlip.rpt');
    assert.equal(calls[1].url, '/reports/print/273-sale-order-slip?id=123');
    assert.equal(windows[0].location, 'blob:report');

    tables = [
        table('Invoices', [[row([cell('First'), cell('Private', false), cell('1,000.00')]),
            row([cell('Hidden row'), cell('Private', false), cell('99')], false)],
            [row([cell('Second'), cell('Private', false), cell('200.00')])]],
            [row([cell('Total'), cell('', false), cell('1,200.00')])]),
        table('Receipts', [[row([cell('Receipt'), cell('Private', false), cell('50.00')])]])
    ];
    // Column filter controls must not replace real column headings with Col1/Col2.
    tables[0].tHead.rows.push(row([cell(''), cell(''), cell('')]));
    await window.print();
    const screen = calls.at(-1);
    assert.equal(screen.url, '/reports/print/screen');
    const grids = JSON.parse(screen.options.body);
    assert.equal(grids.length, 2);
    assert.deepEqual(grids[0].rows, [
        { Name: 'First', Amount: '1,000.00' }, { Name: 'Second', Amount: '200.00' }, { Name: 'Total', Amount: '1,200.00' }
    ]);
    assert.equal(grids[1].title, 'Receipts');

    tables = [];
    const count = calls.length;
    await window.print();
    assert.equal(calls.length, count);
    assert.equal(messages.at(-1), 'No Record Found For Display');
    const print = window.print;
    vm.runInContext(script, context);
    assert.equal(window.print, print, 'Loading the helper twice must not wrap print/open again');
    console.log('ERP print routing: named routes, multiple grids, hidden rows/columns, totals, empty grids and duplicate loading passed.');
})().catch(error => { console.error(error); process.exitCode = 1; });
