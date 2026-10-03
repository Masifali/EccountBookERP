/* Desktop GridEX column widths for the group-D pages (Day Book 15 / Day Book Off Set 24).
 * The page JS renders each grid's <thead> itself; a table that carries
 *   data-colw='{"Caption":width,...}'
 * gets every header cell whose caption matches sized to the designer width
 * (RootTable.Columns[x].Width in the form's grid-setup method) after every render.
 * Purely visual - never touches ids, values or handlers. */
(function () {
    'use strict';
    function apply(t) {
        var map;
        try { map = JSON.parse(t.getAttribute('data-colw') || '{}'); } catch (e) { return; }
        var ths = t.querySelectorAll('thead th');
        for (var i = 0; i < ths.length; i++) {
            var cap = (ths[i].textContent || '').trim();
            var w = map[cap];
            if (w) { ths[i].style.width = w + 'px'; ths[i].style.minWidth = w + 'px'; }
        }
    }
    function boot() {
        var tables = document.querySelectorAll('table[data-colw]');
        Array.prototype.forEach.call(tables, function (t) {
            apply(t);
            if (window.MutationObserver) new MutationObserver(function () { apply(t); }).observe(t, { childList: true });
        });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot); else boot();
}());
