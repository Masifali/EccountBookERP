// Run the ErpNavigationTemplateTest first to exercise the real service's serialized JDBC rows.
// node src/test/javascript/general-ledger-accounts.test.js
const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');
const rows = JSON.parse(fs.readFileSync('target/ui-verification/ledger-account-lookup.json', 'utf8'));
const window = { DesktopCombo: { define() {}, enhance() {} } };
class Option {
    constructor(text, value) { this.text = String(text); this.value = String(value); this.attributes = {}; this.selected = false; }
    setAttribute(key, value) { this.attributes[key] = String(value); }
}
function select(multiple) {
    return {
        multiple, options: [],
        get selectedOptions() { return this.options.filter(option => option.selected); },
        get value() { return this.selectedOptions[0]?.value ?? ''; },
        set value(value) { this.options.forEach(option => { option.selected = option.value === value; }); },
        replaceChildren() { this.options = []; },
        append(option) { this.options.push(option); },
        setAttribute() {}, removeAttribute() {}
    };
}
vm.runInNewContext(fs.readFileSync('src/main/resources/static/build/js/general_ledger_ui.js', 'utf8'), { window, Option });
for (const multiple of [false, true]) {
    const picker = select(multiple);
    window.GeneralLedgerUi.fillAccounts(picker, rows, true);
    const accounts = picker.options.filter(option => option.value);
    assert.deepEqual(accounts.map(option => [option.value, option.text]), [
        ['1', 'Sample Rice Traders'], ['2', 'Sample Flour Mills'], ['3', 'Sample Bank']
    ]);
    assert.equal(accounts[0].attributes['data-other'], '250404993');
    assert.equal(accounts[0].attributes['data-account-type'], 'Receivables & Payables');
    assert.equal(accounts[0].attributes['data-account-class'], 'Liabilities');
    assert.equal(accounts[0].attributes['data-parent-account'], 'TRADE SUPPLIERS');
    picker.value = '1';
    if (multiple) accounts[1].selected = true;
    const selected = picker.selectedOptions.map(option => option.value);
    window.GeneralLedgerUi.fillAccounts(picker, rows, false);
    assert.deepEqual(picker.selectedOptions.map(option => option.value), selected);
    assert.equal(picker.selectedOptions[0].text, '250404993');
    window.GeneralLedgerUi.fillAccounts(picker, rows, true);
    assert.deepEqual(picker.selectedOptions.map(option => option.value), selected);
    assert.equal(picker.selectedOptions[0].text, 'Sample Rice Traders');
}
const variants = ['Id', 'id', 'ID'].map((key, index) => ({ [key]: index + 1,
    accounttitle: 'Account ' + index, ACCOUNTCODE: 'Code ' + index, ACCOUNTTYPE: 'Bank',
    accountClass: 'Assets', parentAccountTitle: 'Banks' }));
const normalized = window.GeneralLedgerUi.normalizeAccounts(variants);
normalized.forEach((row, index) => {
    assert.equal(row.Id, index + 1);
    assert.equal(row.AccountTitle, 'Account ' + index);
    assert.equal(row.AccountCode, 'Code ' + index);
    assert.equal(row.AccountType, 'Bank');
    assert.equal(row.AccountClass, 'Assets');
    assert.equal(row.ParentAccount, 'Banks');
});
console.log('Single and Multi Ledger: serialized JDBC accounts, metadata, and selected IDs survive Title/Code switching.');
