(function () {
    'use strict';
    var navigation = document.getElementById('erp-navigation');
    if (!navigation) return;
    var toggle = document.getElementById('erp-navigation-toggle');
    var body = document.body;
    var smallScreen = window.matchMedia('(max-width: 767px)');
    body.classList.add('erp-with-navigation');

    function expand(open, remember) {
        body.classList.toggle('erp-navigation-expanded', open);
        var sidebar = document.getElementById('erp-module-sidebar');
        sidebar.inert = smallScreen.matches && !open;
        sidebar.setAttribute('aria-hidden', String(smallScreen.matches && !open));
        toggle.setAttribute('aria-expanded', String(open));
        var label = (open ? 'Collapse' : 'Expand') + ' module sidebar';
        toggle.setAttribute('aria-label', label);
        toggle.title = label;
        if (remember && !smallScreen.matches) {
            try { localStorage.setItem('erp.sidebar.expanded', String(open)); } catch (ignored) { }
        }
    }
    var saved = false;
    try { saved = localStorage.getItem('erp.sidebar.expanded') === 'true'; } catch (ignored) { }
    expand(!smallScreen.matches && saved, false);
    toggle.addEventListener('click', function () {
        expand(!body.classList.contains('erp-navigation-expanded'), true);
    });
    navigation.querySelector('.erp-navigation-shade').addEventListener('click', function () {
        expand(false, false);
        toggle.focus();
    });
    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape' && body.classList.contains('erp-navigation-expanded')) {
            expand(false, false);
            toggle.focus();
        }
    });
    smallScreen.addEventListener('change', function () { expand(false, false); });

    function updateExistingBackLinks() {
        var parent = navigation.getAttribute('data-parent-url');
        if (!parent) return;
        var parentTitle = navigation.getAttribute('data-parent-title');
        document.querySelectorAll('a').forEach(function (link) {
            if (navigation.contains(link)) return;
            var text = link.textContent.trim();
            if (!/^back(?:\s+to\b|\s*$)/i.test(text)) return;
            var inlineAction = link.getAttribute('onclick');
            // Keep custom handlers, such as checks for unsaved changes, intact.
            if (inlineAction && !/^\s*(?:window\.)?history\.(?:back\(\)|go\(-1\))\s*;?\s*(?:return false;?)?\s*$/.test(inlineAction)) return;
            link.setAttribute('href', parent);
            link.removeAttribute('onclick');
            link.textContent = '\u2190 Back to ' + parentTitle;
            link.title = 'Back to ' + parentTitle;
        });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', updateExistingBackLinks);
    else updateExistingBackLinks();
}());
