const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');
const html = fs.readFileSync('src/main/resources/templates/accounts/reports/trade_accounts.html', 'utf8');
const opens = [], messages = [];
const fields = { show: { disabled: false, setAttribute() {}, removeAttribute() {} } };
let query = new URLSearchParams({fromDate:'2026-08-01',toDate:'2026-10-04',accountClass:'2',controlAccounts:'41,42',accountId:'123',branches:'58,59',credit:'false',debit:'true',balanceFrom:'123.45'});
let response = [{AccountTitle:'Sample account'}];
const context = vm.createContext({ URLSearchParams, AbortController, rows:[], loadedParams:null, requestId:0, aborter:null,
    $id:id=>fields[id], params:()=>new URLSearchParams(query), render(){}, message:text=>messages.push(text),
    api:async()=>response, window:{open:(...args)=>opens.push(args)} });
for (const prefix of ['async function show(', 'function printTrade(']) {
    vm.runInContext(html.split(/\r?\n/).find(line=>line.startsWith(prefix)),context);
}
(async()=>{
    context.printTrade(true);
    assert.equal(opens.length,0);
    assert.equal(messages.pop(),'No Record Found For Display');
    await context.show();
    const loadedQuery = query.toString();
    query.set('accountId','999'); // Changing controls after Show must not change the loaded report's print filters.
    for(const cityWise of [false,true]) {
        context.printTrade(cityWise);
        const [url,target,features] = opens.pop();
        const printUrl = new URL(url,'http://localhost');
        assert.equal(printUrl.pathname,'/reports/print/trade-accounts');
        assert.equal(printUrl.searchParams.get('cityWise'),String(cityWise));
        printUrl.searchParams.delete('cityWise');
        assert.equal(printUrl.searchParams.toString(),loadedQuery);
        assert.equal(printUrl.searchParams.has('status'),false);
        assert.equal(target,'_blank'); assert.equal(features,'noopener');
    }
    response=[]; await context.show(); context.printTrade(false);
    assert.equal(opens.length,0);
    for(const id of ['printReport','cityPrint']) assert.doesNotMatch(html.match(new RegExp('<button[^>]*id="'+id+'"[^>]*>'))[0],/data-rpt=/);
    assert.match(html, /\$id\('printReport'\)\.onclick=\(\)=>printTrade\(false\)/);
    assert.match(html, /\$id\('cityPrint'\)\.onclick=\(\)=>printTrade\(true\)/);
    console.log('Trade Print / CityWise: loaded filters, account and branch selections, decimal balances, and empty reports passed.');
})().catch(error=>{console.error(error);process.exitCode=1});
