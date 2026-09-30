/* Commission Trading: desktop-geometry canvases fill the page width.
   Any element with data-fit-width="<desktop px>" is zoomed so its desktop layout spans the
   width of its container (never below 0.8, never above 1.8). Relative positions stay exactly
   the desktop's, so labels and fields keep the desktop spacing at any screen size.
   Re-fits on resize and whenever a tab/panel is shown (hidden panels measure 0 wide). */
(function () {
    'use strict';
    function fit() {
        var els = document.querySelectorAll('[data-fit-width]');
        for (var i = 0; i < els.length; i++) {
            var el = els[i], w = parseFloat(el.getAttribute('data-fit-width')) || 0;
            var host = el.parentElement; if (!w || !host) continue;
            var avail = host.clientWidth - 2;
            if (avail <= 0) continue;                       /* hidden tab - fit later */
            var z = Math.max(0.8, Math.min(avail / w, 1.8));
            el.style.zoom = z.toFixed(4);
        }
    }
    var t = null;
    function later() { clearTimeout(t); t = setTimeout(fit, 30); }
    window.addEventListener('resize', later);
    document.addEventListener('click', later, true);
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', fit); else fit();
    window.addEventListener('load', fit);
    window.cmagtFit = fit;
})();
