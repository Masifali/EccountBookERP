// Run with: node src/test/javascript/trade-report-filters.test.js
const fs = require('node:fs');
const vm = require('node:vm');
const assert = require('node:assert/strict');
const html = fs.readFileSync('src/main/resources/templates/accounts/reports/trade_accounts.html', 'utf8');
const fields = { fromDate: '2026-08-01', toDate: '2026-10-04', branches: '58' };
const lookup = { features: [] };
const context = vm.createContext({ lookup, URLSearchParams, number: v => Number(v || 0),
    value: id => fields[id] || '', selected: id => fields[id] || [], flag: () => true,
    document: { querySelector: selector => ({value: selector.includes('reportClass') ? '2' : selector.includes('classification') ? 'account' : '0'}) }
});
for (const name of ['currentClass', 'reportBranches', 'params']) {
    vm.runInContext(html.split(/\r?\n/).find(line => line.startsWith('function ' + name + '(')), context);
}
function branches(features, selection) {
    lookup.features = features.map(Id => ({Id}));
    fields.branches = selection;
    return context.params().get('branches');
}
assert.equal(branches([], '58'), '', 'A hidden branch must not split opening balances from transactions');
assert.equal(branches([18], ['58', '59']), '', 'Consolidation alone does not enable branch filtering');
assert.equal(branches([17], '58'), '58', 'Enabled single-branch selection must remain effective');
assert.equal(branches([17, 18], ['58', '59']), '58,59', 'Enabled consolidated branches must remain distinct');
assert.equal(branches([17, 18], []), '', 'Empty consolidated selection means all branches');
assert.equal(context.params().get('fromDate'), '2026-08-01');
assert.equal(context.params().get('toDate'), '2026-10-04');
assert.equal(context.params().get('accountClass'), '2');
console.log('Trade report filters: disabled branches, enabled branch selections, consolidation and dates passed.');
