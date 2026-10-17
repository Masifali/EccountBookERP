// NODE_PATH must provide jsdom. This renders local templates without requests or database writes.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { JSDOM, VirtualConsole } = require('jsdom');
const root = path.resolve(__dirname, '../../..');
const resources = path.join(root, 'src/main/resources');
const mapping = JSON.parse(fs.readFileSync(path.join(root, 'migration/sale/grid-controls-desktop-map.json')));
const chrome = fs.readFileSync(path.join(resources, 'static/build/js/countx_purchase_page_chrome.js'), 'utf8');
const gridbar = fs.readFileSync(path.join(resources, 'static/build/js/countx_grid_bar.js'), 'utf8');

function page(html) {
    const dom = new JSDOM(html, { url: 'http://localhost:8025/sale/test', runScripts: 'outside-only', virtualConsole: new VirtualConsole() });
    const w = dom.window;
    w.GridBarUserName = 'sale-grid-test';
    w.eval(chrome);
    w.eval(gridbar);
    w.GridBar.scan();
    return dom;
}

test('Sale templates expose only the desktop-mapped controls, each in its own header', async () => {
    let total = 0;
    for (const entry of mapping) {
        const html = fs.readFileSync(path.join(resources, 'templates/sale', entry.template), 'utf8');
        const dom = page(html), d = dom.window.document;
        try {
            await new Promise(resolve => setTimeout(resolve, 25));
            assert.equal(d.querySelectorAll('.gb-gear').length, entry.controls.length, entry.template + ': unexpected/missing grid control');
            assert.equal(d.querySelectorAll('.pc-fs-btn').length, entry.controls.length, entry.template + ': duplicate/missing fullscreen control');
            const headers = new Set();
            for (const control of entry.controls) {
                const heading = d.getElementById(control.heading);
                assert(heading, entry.template + ': missing caption');
                assert(!headers.has(heading), entry.template + ': two grids share a caption');
                headers.add(heading);
                assert.equal(heading.querySelectorAll('.gb-gear').length, 1, entry.template + ': control outside its caption');
                assert.equal(heading.querySelectorAll('.pc-fs-btn').length, 1, entry.template + ': fullscreen outside its caption');
                assert(heading.classList.contains('sc-grid-heading'));
                assert.equal(heading.querySelector('.gb-gear').parentElement, heading.querySelector('.pc-fs-btn').parentElement);
                assert(!heading.closest('table'), entry.template + ': caption inside scrolling table');
            }
            total += entry.controls.length;
        } finally { dom.window.close(); }
    }
    assert(total > 150, 'The audit must cover all Sale variants and reports');
});

test('a dynamic grid retains one pair of controls after its entire table is replaced', async () => {
    const dom = page('<div id="caption">Detail</div><div id="grid" style="overflow:auto" data-gridbar-form="SaleOrder" data-gridbar-grid="grd" data-gridbar-heading="#caption"></div>');
    const w = dom.window, d = w.document;
    try {
        const host = d.getElementById('grid');
        for (const label of ['Item', 'Changed item']) {
            host.innerHTML = `<table><thead><tr><th>${label}</th></tr></thead><tbody><tr><td>Sample</td></tr></tbody></table>`;
            w.GridBar.refresh(host);
            await new Promise(resolve => setTimeout(resolve, 25));
            assert.equal(d.querySelectorAll('.gb-gear').length, 1);
            assert.equal(d.querySelectorAll('.pc-fs-btn').length, 1);
            assert.equal(d.querySelectorAll('.pc-grid-tools').length, 1);
        }
        d.querySelector('.pc-fs-btn').click();
        assert(host.classList.contains('pc-fs-on'));
        assert(d.querySelector('#caption .pc-grid-tools').classList.contains('pc-fs-toolbar-on'));
        d.querySelector('.gb-gear').click();
        assert.equal(d.querySelector('.gb-menu').style.display, 'block');
        d.dispatchEvent(new w.KeyboardEvent('keydown', { key: 'Escape', bubbles: true }));
        assert(!d.querySelector('.pc-fs-on,.pc-fs-toolbar-on'));
    } finally { dom.window.close(); }
});

test('tables without a desktop grid control stay unchanged, including tables rendered later', async () => {
    const dom = page('<div id="caption">Detail</div><table data-gridbar="GDN:grd" data-gridbar-heading="#caption"><thead><tr><th>Item</th></tr></thead></table><div id="expenses"></div>');
    const w = dom.window, d = w.document;
    try {
        d.getElementById('expenses').innerHTML = '<table id="expenseTable"><thead><tr><th>Expense</th></tr></thead></table>';
        await new Promise(resolve => setTimeout(resolve, 25));
        assert.equal(d.querySelectorAll('.gb-gear').length, 1);
        assert.equal(d.querySelectorAll('.pc-fs-btn').length, 1);
        assert(!d.getElementById('expenseTable').hasAttribute('data-gridbar'));
    } finally { dom.window.close(); }
});
