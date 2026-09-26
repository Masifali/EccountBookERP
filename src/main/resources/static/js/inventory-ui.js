/* Inventory request and navigation feedback; no changes to submitted record data. */
(() => {
    'use strict';
    let finishNavigation;
    document.documentElement.dataset.loadingLabel = 'Loading inventory';
    document.addEventListener('click', event => {
        const link = event.target.closest('a[href]');
        if (!link || event.defaultPrevented || event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey || event.altKey || link.target || link.hasAttribute('download')) return;
        const url = new URL(link.href, location.href);
        if (url.origin !== location.origin || !/^\/(inventory|stocks)(\/|$)/.test(url.pathname) || url.href === location.href || url.hash) return;
        if (finishNavigation) { event.preventDefault(); return; }
        finishNavigation = ReportLoading.begin();
    });
    window.addEventListener('pageshow', () => { if(finishNavigation)finishNavigation();finishNavigation=null; });
    window.addEventListener('pagehide', () => { if(finishNavigation)finishNavigation();finishNavigation=null; });
    document.addEventListener('submit', event => {
        // Check after all form handlers have had the opportunity to cancel navigation.
        queueMicrotask(() => {
            if (!event.defaultPrevented && !finishNavigation) finishNavigation = ReportLoading.begin();
        });
    });
    document.addEventListener('DOMContentLoaded', () => {
        // Page-specific initialization gets first choice of selection templates/options.
        setTimeout(async () => {
            if (!document.querySelector('select:not([data-native-select])')) return;
            const script = src => new Promise((resolve,reject) => {
                const node=document.createElement('script');node.src=src;node.onload=resolve;node.onerror=reject;document.head.append(node);
            });
            try {
                if (!window.jQuery) await script('/vendors/jquery/dist/jquery.min.js');
                if (!window.jQuery.fn.select2) {
                    const css=document.createElement('link');css.rel='stylesheet';css.href='/vendors/select2/dist/css/select2.min.css';document.head.append(css);
                    await script('/vendors/select2/dist/js/select2.min.js');
                }
            } catch (_) { return; }
            document.querySelectorAll('select:not([data-native-select])').forEach(select => {
                if(!$(select).hasClass('select2-hidden-accessible')) $(select).select2({width:'resolve',minimumResultsForSearch:0});
            });
        }, 0);
    });
})();
