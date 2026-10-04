// Run with: node src/test/javascript/erp-navigation.test.js
const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');

const script = fs.readFileSync('src/main/resources/static/build/js/erp_navigation.js', 'utf8');
let removed = false;
const document = {
    getElementById(id) {
        assert.equal(id, 'erp-navigation', 'Embedded forms must not initialise sidebar controls');
        return { remove() { removed = true; } };
    },
    get body() { throw new Error('Embedded forms must not reserve sidebar space'); }
};
// No parent document access or fetch headers are available in this fallback.
const window = { self: {}, top: {} };
vm.runInNewContext(script, { document, window });
assert.equal(removed, true, 'The duplicate navigation must be removed');
console.log('Embedded navigation fallback passed');
