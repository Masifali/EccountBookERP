/* Commission Trading: Company and Branch combos open on the logged-in company and branch, as
   every desktop form does (CmbCompany = UserAccount.CompanyId, CmbBranch = UserAccount.BranchesId).
   Only fills a combo that is still on "-- Select --" (value 0/empty), so a loaded record or a
   user choice is never overwritten. Waits for the page's own lookups to fill the options. */
(function () {
    'use strict';
    var COMPANY = ['cmbCompany', 'cmbCompanyName', 'companyNameId', 'companyId', 'cmbCompanyId'];
    var BRANCH  = ['cmbBranch', 'branchSelectId', 'branchId', 'cmbBranchId'];
    function empty(el) { var v = el.value; return !v || v === '0'; }
    function has(el, v) { for (var i = 0; i < el.options.length; i++) if (el.options[i].value === String(v)) return true; return false; }
    function apply(ids, v, done) {
        var ok = true;
        ids.forEach(function (id) {
            var el = document.getElementById(id);
            if (!el || el.tagName !== 'SELECT') return;
            if (done[id]) return;
            if (!empty(el)) { done[id] = true; return; }
            if (has(el, v)) {
                el.value = String(v);
                if (window.jQuery) { try { jQuery(el).trigger('change.select2'); } catch (e) {} }
                done[id] = true;
            } else ok = false;
        });
        return ok;
    }
    function start() {
        fetch('/api/commission/dropdowns/config-defaults', { credentials: 'same-origin' })
            .then(function (r) { return r.ok ? r.json() : null; })
            .then(function (d) {
                if (!d) return;
                var c = d.sessionCompanyId, b = d.sessionBranchId, done = {}, n = 0;
                var t = setInterval(function () {
                    var a = c ? apply(COMPANY, c, done) : true;
                    var bb = b ? apply(BRANCH, b, done) : true;
                    if ((a && bb) || ++n > 60) clearInterval(t);
                }, 250);
            }).catch(function () {});
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', start); else start();
})();
