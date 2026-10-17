// NODE_PATH must point to jsdom. All ERP and PDF requests are mocked.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const { JSDOM, VirtualConsole } = require('jsdom');
const root = path.resolve(__dirname, '../../main/resources');
const html = fs.readFileSync(path.join(root, 'templates/weighbridge/weightbridge_mannual.html'), 'utf8');
const page = fs.readFileSync(path.join(root, 'static/build/js/countx_weightbridge_mannual.js'), 'utf8');
const printRouter = fs.readFileSync(path.join(root, 'static/js/print-rpt.js'), 'utf8');
const flush = async () => { for (let i = 0; i < 6; i++) await new Promise(setImmediate); };
const slip280 = '/reports/print/280-weigh-bridge-slip';
const slip257 = '/reports/print/257-inward-gate-pass-with-wb-and-lab-slip';

async function setup({ canPrint = true, source = page } = {}) {
    const dom = new JSDOM(html, { url: 'http://localhost:8025/weighbridge/weightbridge-mannual', runScripts: 'outside-only', virtualConsole: new VirtualConsole() });
    const w = dom.window, d = w.document;
    await new Promise(resolve => w.addEventListener('load', resolve, { once: true }));
    const calls = [], alerts = [], windows = [];
    const row = { Id: 901, TicketNo: 836, ReferenceDocTypeId: 0, ReferenceDocNoId: 437, VehicleNo: 'TEST-123', FirstWeight: 26750, SecondWeight: 6710, NetWbWeight: 20040, ItemId: 1, ItemDescription: 'Paddy', WeighBridgeType: 'Paddy', VehicleType: 1 };
    const history = [{ ...row, ReferenceDocTypeId: 51 }, { ...row, Id: 903, ReferenceDocNoId: 438, ReferenceDocTypeId: 91 }];
    w.alert = text => alerts.push(text);
    w.confirm = () => true;
    w.URL.createObjectURL = () => 'blob:mock-pdf';
    w.open = () => {
        const win = { location: '', close() {}, document: { open() {}, write() {}, close() {} } };
        windows.push(win); return win;
    };
    w.fetch = async (url, options = {}) => {
        const u = new URL(url, w.location.href);
        calls.push({ path: u.pathname, query: u.searchParams, body: options.body && JSON.parse(options.body) });
        let result;
        if (u.pathname === '/api/weighbridge-manual/lookups') result = {
            canSave: true, canUpdate: true, canPrint, ticketNo: 836,
            vehicleTypes: [{ Id: 1, VehicleDescription: 'Truck' }],
            items: [{ Id: 1, ItemName: 'Paddy' }], wbTypes: [{ Id: 1, GpTypeDescription: 'Paddy' }],
            packingTypes: [], parties: [], referenceTypes: []
        };
        else if (u.pathname.endsWith('/pending-second')) result = [row];
        else if (u.pathname.endsWith('/pending-first') || u.pathname.endsWith('/ref-docs')) result = [];
        else if (u.pathname.endsWith('/ticket-no')) result = { ticketNo: 837 };
        else if (u.pathname.endsWith('/history')) result = history;
        else if (u.pathname.endsWith('/901')) result = { row, approved: false };
        else if (u.pathname.endsWith('/save') || u.pathname.endsWith('/update')) result = { success: true, id: 902, message: 'Saved' };
        else if (u.pathname === '/reports/print/controls') result = { pdf: u.searchParams.get('rpt').startsWith('280-') ? slip280 : slip257, args: ['id'], controls: {} };
        else if ([slip280, slip257].includes(u.pathname)) return { ok: true, headers: { get: () => 'application/pdf' }, blob: async () => new w.Blob(['PDF']) };
        else throw new Error('Unexpected request: ' + url);
        return { ok: true, text: async () => JSON.stringify(result), json: async () => result };
    };
    w.eval(source);
    w.eval(printRouter);
    for (const button of d.querySelectorAll('button[onclick]')) button.onclick = new w.Function(button.getAttribute('onclick'));
    await flush();
    const pdfs = () => calls.filter(c => c.path.startsWith('/reports/print/') && c.path !== '/reports/print/controls').map(c => [c.path, c.query.get('id')]);
    const select = (id, value, label) => { const el = d.getElementById(id); el.innerHTML = ''; el.add(new w.Option(label, String(value), true, true)); };
    const openSaved = async () => { d.querySelector('#grdfill [data-btn="Edit"]').click(); await flush(); };
    const fillValid = () => {
        select('cmbReferenceType', 51, 'GatePass Inward'); select('CmbGatePassNo', 437, '437');
        for (const [id, value] of Object.entries({ CmbWeighBridgeType: 'Paddy', CmbItem: 'Paddy', txtTickectNo: '836', txtVehicleNo: 'TEST-123', txtFirstWeight: '26750', txtLoadWeight: '26750', txtsecondWeight: '6710', txtTearWeight: '6710', txtSupplierWbNetWeight: '20040', txtNetWeight: '20040' })) d.getElementById(id).value = value;
    };
    return { dom, w, d, calls, alerts, windows, pdfs, select, openSaved, fillValid };
}

test('toolbar prints the loaded ticket, ignoring unrelated selected grid rows', async () => {
    const h = await setup();
    try {
        await h.openSaved();
        const unrelated = h.d.querySelector('#grdfill tbody tr'); unrelated.classList.add('selected'); unrelated.dataset.id = '999';
        h.d.getElementById('btnPrint').click(); await flush();
        assert.deepEqual(h.pdfs(), [[slip280, '901']]);
        assert.equal(h.windows[0].location, 'blob:mock-pdf');
        assert.deepEqual(h.alerts, []);
        assert.equal(h.calls.some(c => c.path.includes('/slip/')), false);
    } finally { h.dom.window.close(); }
});

test('Ctrl+P prints the saved ticket through the desktop PDF endpoint', async () => {
    const h = await setup();
    try {
        await h.openSaved();
        h.d.dispatchEvent(new h.w.KeyboardEvent('keydown', { key: 'p', ctrlKey: true })); await flush();
        assert.deepEqual(h.pdfs(), [[slip280, '901']]);
    } finally { h.dom.window.close(); }
});

test('History prints each row ID and uses the reference gate-pass ID for PrintWithLab', async () => {
    const h = await setup();
    try {
        await h.w.WeighBridgeManual.showHistory();
        const rows = h.d.querySelectorAll('#DataGridHistory tbody tr');
        rows[1].querySelector('[data-btn="Print"]').click(); await flush();
        rows[0].querySelector('[data-btn="PrintWithLab"]').click(); await flush();
        h.w.WeighBridgeManual.tab('tabHistory');
        h.d.dispatchEvent(new h.w.KeyboardEvent('keydown', { key: 'p', ctrlKey: true })); await flush();
        assert.deepEqual(h.pdfs(), [[slip280, '903'], [slip257, '437'], [slip280, '901']]);
        rows[1].querySelector('[data-btn="PrintWithLab"]').click(); await flush();
        assert.equal(h.pdfs().length, 3);
        assert.match(h.alerts[0], /Only For 'Inward GatePass'/);
    } finally { h.dom.window.close(); }
});

for (const action of ['save', 'update']) test(`${action} previews retain the returned ticket ID and original gate pass after form reset`, async () => {
    const h = await setup();
    try {
        if (action === 'update') await h.openSaved();
        h.fillValid();
        h.d.getElementById('ChkBox').checked = true; h.d.getElementById('chkPrintWithLab').checked = true;
        h.w.WeighBridgeManual[action](); await flush();
        assert.equal(h.calls.filter(c => c.path === '/api/weighbridge-manual/' + action).length, 1);
        assert.deepEqual(h.pdfs(), [[slip280, '902'], [slip257, '437']]);
        assert.equal(h.d.getElementById('txtTickectNo').value, '837');
    } finally { h.dom.window.close(); }
});

test('empty ticket, missing inward reference and missing print permission do not open a report', async () => {
    const h = await setup();
    try {
        h.w.WeighBridgeManual.print280();
        h.w.WeighBridgeManual.slip257();
        h.select('cmbReferenceType', 51, 'GatePass Inward');
        h.w.WeighBridgeManual.slip257();
        assert.deepEqual(h.alerts, ['No Record Found For Display', 'Please Select the GatePass Type To Inward...', 'Please Select the GatePass No First...']);
        assert.equal(h.windows.length, 0);
    } finally { h.dom.window.close(); }
    const denied = await setup({ canPrint: false });
    try {
        await denied.openSaved(); denied.w.WeighBridgeManual.print280();
        assert.equal(denied.d.getElementById('btnPrint').disabled, true);
        assert.equal(denied.windows.length, 0);
    } finally { denied.dom.window.close(); }
});

test('missing PDF helper reports the problem instead of printing a generic table', async () => {
    const h = await setup();
    try {
        await h.openSaved(); delete h.w.printRpt;
        h.w.WeighBridgeManual.print280();
        assert.deepEqual(h.alerts, ['Print preview could not load. Refresh the page and try again.']);
        assert.equal(h.windows.length, 0);
    } finally { h.dom.window.close(); }
});
