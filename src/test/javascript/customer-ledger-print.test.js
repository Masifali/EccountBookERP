// Run with: node src/test/javascript/customer-ledger-print.test.js
const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');

const template = fs.readFileSync('src/main/resources/templates/accounts/reports/customer_ledger.html', 'utf8');
const pageScript = template.match(/<script>\s*([\s\S]*?)<\/script>/)[1];
const buttonTag = template.match(/<button[^>]*onclick="printCL\(\)"[^>]*>/)[0];
const fields = { '#comSupplier': '1920', '#txtfromdate': '2026-08-01', '#txttodate': '2026-10-04' };
const calls = [], messages = [], handlers = {};
const document = {};
const context = vm.createContext({ document, URLSearchParams,
    window: { location: { search: '' }, printRpt: (rpt, args) => calls.push({ rpt, args }),
        print: () => assert.fail('Customer Ledger must use report 130, not a screen print') },
    alert: message => messages.push(message),
    $: selector => {
        if (typeof selector === 'function') return; // Page initialization is covered by the live check.
        if (selector === document) return { on: (event, handler) => { handlers[event] = handler; } };
        return { val: () => fields[selector] };
    }
});
vm.runInContext(pageScript, context);

context.printCL();
assert.equal(messages.pop(), 'Record Not Found For Display');
assert.equal(calls.length, 0);

vm.runInContext('clRows = [{ DocNo: 434 }]', context);
assert.match(buttonTag, /\bdata-rpt-own\b/, 'The shared capture handler must leave page validation intact');
context.printCL();
assert.deepEqual(JSON.parse(JSON.stringify(calls.pop())), {
    rpt: '130-RptAcSupplierCustomerQuantativeGL.rpt',
    args: { supplierCustomerId: 1920, fromDate: '2026-08-01', toDate: '2026-10-04' }
});

let prevented = false;
handlers.keydown({ ctrlKey: true, key: 'p', preventDefault: () => { prevented = true; } });
assert.ok(prevented);
assert.equal(calls.pop().rpt, '130-RptAcSupplierCustomerQuantativeGL.rpt');

fields['#comSupplier'] = '';
context.printCL();
assert.equal(messages.pop(), 'Party Name required. Please Check!');
fields['#comSupplier'] = '1920';
fields['#txtfromdate'] = '2026-10-05';
context.printCL();
assert.equal(messages.pop(), 'Select a valid From Date and To Date.');
fields['#txtfromdate'] = '';
context.printCL();
assert.equal(messages.pop(), 'Select a valid From Date and To Date.');
assert.equal(calls.length, 0, 'Invalid selections must not start a print request');

const controls = JSON.parse(fs.readFileSync('src/main/resources/reports/print-controls.json', 'utf8'));
assert.deepEqual(controls['130-rptacsuppliercustomerquantativegl.rpt'].controls.supplierCustomerId, ['comSupplier']);
console.log('Customer Ledger print: selected party/dates, toolbar, Ctrl+P, empty ledger and invalid filters passed.');
