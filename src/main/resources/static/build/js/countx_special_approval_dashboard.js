(function () {
    'use strict';
    var API = '/api/dashboard/special-approvals';
    var busy = false, initialized = false;
    var groups = [['accounts', 'saAccounts'], ['inventory', 'saInventory'], ['commission', 'saCommission']];
    function el(id) { return document.getElementById(id); }
    function message(text) { el('saMessage').textContent = text || ''; el('saMessage').hidden = !text; }
    function setBusy(on) {
        busy = on;
        el('saRefresh').disabled = on;
        el('saShow').disabled = on;
        el('saBranch').disabled = on;
        el('saSections').setAttribute('aria-busy', String(on));
    }
    async function get(path) {
        var response = await fetch(API + path, { headers: { Accept: 'application/json' }, cache: 'no-store' });
        if (response.status === 401 || response.redirected) throw new Error('Please sign in again to view special approvals.');
        var data;
        try { data = await response.json(); }
        catch (_) { throw new Error('The server returned an unexpected response. Please retry.'); }
        if (!response.ok) throw new Error(data.message || 'Special approvals could not be loaded. Please retry.');
        return data;
    }
    function add(parent, tag, text, className) {
        var node = document.createElement(tag);
        node.textContent = text;
        if (className) node.className = className;
        parent.appendChild(node);
        return node;
    }
    function clearCards() {
        groups.forEach(function (group) { el(group[1]).replaceChildren(); });
        el('saDesktopNotice').hidden = true;
    }
    function render(data) {
        clearCards();
        var total = 0;
        groups.forEach(function (group) {
            var container = el(group[1]), cards = data[group[0]] || [];
            if (!cards.length) add(container, 'p', 'No pending special approvals.', 'sa-none');
            cards.forEach(function (card) {
                var count = Number(card.count) || 0;
                total += count;
                var tile = add(container, 'article', '', 'sa-card');
                add(tile, 'h3', card.title || 'Special Approval');
                if (card.reason) add(tile, 'p', card.reason, 'sa-reason');
                add(tile, 'span', String(count), 'sa-count').setAttribute('aria-label', count + ' pending');
                add(tile, 'p', 'Desktop: ' + (card.desktopScreen || 'Special Approval'), 'sa-screen');
            });
        });
        el('saDesktopNotice').hidden = total === 0;
        el('saStatus').textContent = total ? total + ' pending special approval' + (total === 1 ? '.' : 's.')
                : 'Nothing is waiting for special approval.';
    }
    async function load() {
        if (busy) return;
        setBusy(true);
        message('');
        clearCards();
        el('saStatus').textContent = 'Loading special approvals…';
        try {
            if (!initialized) {
                var setup = await get('/setup');
                el('saFilters').hidden = !setup.branchFeature;
                var branch = el('saBranch');
                branch.replaceChildren();
                (setup.branches || []).forEach(function (row) {
                    var option = add(branch, 'option', row.BranchName);
                    option.value = String(row.BranchId);
                });
                branch.value = String(setup.currentBranchId || 0);
                if (!branch.value && branch.options.length) branch.selectedIndex = 0;
                initialized = true;
            }
            render(await get('/cards?branchId=' + encodeURIComponent(el('saBranch').value || '0')));
        } catch (error) {
            message(error.message);
            el('saStatus').textContent = 'Special approvals could not be loaded.';
        } finally { setBusy(false); }
    }
    el('saRefresh').addEventListener('click', load);
    el('saFilters').addEventListener('submit', function (event) { event.preventDefault(); load(); });
    // Browser Back may restore the old counts without re-running this script.
    window.addEventListener('pageshow', function (event) { if (event.persisted) load(); });
    load();
})();
