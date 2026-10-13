/* ============================================================================================
 * countx_purchase_page_chrome.js - page chrome for the Supplier Purchases document forms
 * (Purchase Order 120, Goods Receipt Notes 121, Grn (Sale Return) 866, Stock In Transit 868).
 *
 *   PurchaseChrome.footer({ isHistory: fn -> bool, toggle: fn, watch: element|selector })
 *       A fixed footer strip with the History button on its RIGHT (the same place as the
 *       countx_store_common.js / HRM footers). The button switches the page's own
 *       tabControl1 page ("Form" <-> "History") through the page's toggle(), so every
 *       page keeps its own tab logic; its caption follows the active page.
 *
 *   PurchaseChrome.fullscreen(container, caption)
 *       Adds a small toggle to a grid container: the grid then fills the window and scrolls
 *       inside itself (Esc or the toggle restores it). The page itself never scrolls sideways.
 *       data-fullscreen-mount="#toolbar" places the toggle alongside the grid controls.
 *   PurchaseChrome.gridTools(container, preferredMount)
 *       Returns the shared control slot above a grid; used by GridBar on purchase pages.
 *
 * No framework; no dependency. Additive only - a page that does not call it is unaffected.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var doc = global.document;
    var STYLE_ID = 'purchase-chrome-style';
    var fullscreenControls = new WeakMap();
    var gridToolbars = new WeakMap();
    var toolbarOwners = new WeakMap();

    function injectStyle() {
        if (doc.getElementById(STYLE_ID)) return;
        var s = doc.createElement('style');
        s.id = STYLE_ID;
        s.textContent =
            'html.pc-page,html.pc-page body{max-width:100%;overflow-x:hidden}' +
            'body.pc-has-footer{padding-bottom:40px}' +
            '.pc-footer{position:fixed;left:0;right:0;bottom:0;height:34px;display:flex;align-items:center;justify-content:space-between;gap:8px;' +
            'padding:0 10px;background:linear-gradient(to bottom,#fcfcfc 0%,#e0e0e0 100%);border-top:1px solid #999;z-index:8000;box-sizing:border-box;' +
            'font:11px Tahoma,Verdana,sans-serif}' +
            '.pc-footer-left{color:#333;white-space:nowrap;overflow:hidden;text-overflow:ellipsis}' +
            '.pc-footer-history{margin-left:auto;background:#004d40;color:#fff;font:bold 9pt Verdana,sans-serif;border:1px solid #00251a;height:26px;' +
            'padding:0 14px;cursor:pointer;border-radius:2px;display:inline-flex;align-items:center;gap:5px}' +
            '.pc-footer-history:hover{background:#00251a}' +
            '.pc-fs-host{position:relative}' +
            '.pc-fs-btn{position:absolute;top:1px;right:1px;z-index:30;height:18px;min-width:18px;padding:0 4px;font:9px Verdana,sans-serif;' +
            'background:#ece9d8;border:1px solid #888;cursor:pointer;opacity:.8}' +
            '.pc-fs-btn:hover{opacity:1;background:#ffe4a0}' +
            '.pc-grid-heading{position:relative;box-sizing:border-box;padding-right:82px!important}' +
            '.pc-grid-heading>.cap,.pc-grid-heading>[class$="-title"]{max-width:calc(100% - 86px);overflow:hidden;text-overflow:ellipsis}' +
            '.pc-grid-heading>.cog,.pc-grid-heading>button:disabled[title*="not ported"]{display:none!important}' +
            '.pc-grid-heading-new{height:26px;background:#008080;color:#fff;flex-shrink:0}' +
            '.pc-grid-tools{position:absolute;right:4px;top:0;width:74px;height:100%;display:flex;justify-content:flex-end;align-items:center;gap:6px;box-sizing:border-box}' +
            '.pc-grid-tools>.pc-fs-btn{position:static;width:28px;height:22px;min-width:28px;flex:0 0 28px;font-size:13px;opacity:1}' +
            '.pc-grid-tools>.gb-gear{position:static;width:36px;height:22px;flex:0 0 36px;margin:0!important}' +
            '.pc-grid-tools>button{max-height:calc(100% - 4px);box-sizing:border-box}' +
            '.pc-grid-tools.pc-grid-tools-inline{position:static;display:inline-flex;width:auto;height:22px;margin-left:6px;vertical-align:middle}' +
            '.pc-fs-toolbar-on{position:fixed!important;left:6px!important;right:6px!important;top:6px!important;height:26px!important;' +
            'width:auto!important;justify-content:flex-end;padding:0 4px;background:#008080;z-index:9001!important}' +
            '.pc-fs-toolbar-on>.gb-gear{margin-left:0!important}' +
            '.pc-fs-on.pc-fs-with-toolbar{top:32px!important}' +
            '.pc-fs-on{position:fixed!important;inset:6px 6px 40px 6px!important;z-index:9000!important;height:auto!important;max-height:none!important;' +
            'width:auto!important;overflow:auto!important;background:#fff;box-shadow:0 4px 24px rgba(0,0,0,.45)}';
        (doc.head || doc.documentElement).appendChild(s);
    }

    function el(x) { return typeof x === 'string' ? doc.querySelector(x) : x; }

    // Both controls use the same small strip, outside the scrolling table. Resolve the
    // scroll host even when the other control has already been inserted into it.
    function gridHost(container) {
        var box = el(container);
        if (box && box.tagName === 'TABLE' && box.parentElement) {
            var p = box.parentElement, cs = global.getComputedStyle(p);
            var only = Array.from(p.children).every(function (c) {
                return c === box || c.matches('.pc-fs-btn,.gb-bar');
            });
            if (only && /(auto|scroll)/.test(cs.overflow + cs.overflowX + cs.overflowY)) box = p;
        }
        return box;
    }

    function gridHeading(box) {
        for (var node = box, depth = 0; node && node.parentElement && depth < 8; node = node.parentElement, depth++) {
            for (var s = node.previousElementSibling; s; s = s.previousElementSibling) {
                if (s.matches('table') || s.querySelector('table')) return null;
                var key = s.id + ' ' + s.className;
                if (s.matches('.gb-bar,nav,form,fieldset') || /filter|navigation|sidebar|menu/i.test(key)) continue;
                if (/teal|caption|title|header|toolbar|gridbar|pio-sec|(?:^|[\s_-])(?:bar\d*|head)(?:$|[\s_-])/i.test(key)
                    && s.getBoundingClientRect().height <= 80 && !toolbarOwners.has(s)) return s;
            }
            if (node.matches('section,fieldset,.panel,.card,.groupbox,.win-groupbox,[role="tabpanel"]')) break;
        }
        return null;
    }

    function gridTools(container, preferred) {
        injectStyle();
        var box = gridHost(container);
        if (!box) return null;
        var cached = gridToolbars.get(box);
        if (cached && cached.isConnected) return cached;
        var mount = el(preferred) || el(box.getAttribute('data-fullscreen-mount'));
        // Explicit slots (including the report toolbar's inline span) already own their space.
        if (mount && !mount.classList.contains('pc-grid-tools')) {
            mount.classList.add('pc-grid-tools', 'pc-grid-tools-inline');
            mount.setAttribute('role', 'toolbar');
            mount.setAttribute('aria-label', 'Grid controls');
            gridToolbars.set(box, mount);
            return mount;
        }
        var heading = mount && mount.classList.contains('pc-grid-tools') ? mount.parentElement : mount;
        heading = heading || gridHeading(box);
        if (!heading) {
            heading = doc.createElement('div');
            heading.className = 'pc-grid-heading-new';
            box.parentNode.insertBefore(heading, box);
        }
        heading.classList.add('pc-grid-heading');
        toolbarOwners.set(heading, box);
        if (!mount || !mount.classList.contains('pc-grid-tools')) {
            mount = doc.createElement('div');
            heading.appendChild(mount);
        }
        mount.classList.add('pc-grid-tools');
        mount.setAttribute('role', 'toolbar');
        mount.setAttribute('aria-label', 'Grid controls');
        gridToolbars.set(box, mount);
        return mount;
    }

    function footer(opts) {
        injectStyle();
        doc.documentElement.classList.add('pc-page');
        if (doc.querySelector('.pc-footer')) return;
        var f = doc.createElement('div');
        f.className = 'pc-footer';
        f.innerHTML = '<span class="pc-footer-left"></span>' +
            '<button type="button" class="pc-footer-history" title="Ctrl+T"><i class="fa fa-history"></i> <span>History</span></button>';
        doc.body.appendChild(f);
        doc.body.classList.add('pc-has-footer');
        var btn = f.querySelector('button'), lbl = btn.querySelector('span'), ico = btn.querySelector('i');
        function sync() {
            var h = !!opts.isHistory();
            lbl.textContent = h ? 'Form' : 'History';
            ico.className = h ? 'fa fa-file-text-o' : 'fa fa-history';
        }
        btn.addEventListener('click', function () { opts.toggle(); sync(); });
        var watched = [].concat(opts.watch || []).map(el).filter(Boolean);
        if (global.MutationObserver) watched.forEach(function (w) {
            new MutationObserver(sync).observe(w, { attributes: true, attributeFilter: ['class', 'style', 'hidden'] });
        });
        sync();
        return { sync: sync, left: f.querySelector('.pc-footer-left') };
    }

    function fullscreen(container, caption) {
        injectStyle();
        var box = gridHost(container);
        if (!box || fullscreenControls.has(box)) return;
        caption = box.getAttribute('aria-label') || caption;
        var mount = gridTools(box);
        box.classList.add('pc-fs-host');
        var b = doc.createElement('button');
        b.type = 'button';
        b.className = 'pc-fs-btn';
        function setFullscreen(on) {
            box.classList.toggle('pc-fs-on', on);
            box.classList.toggle('pc-fs-with-toolbar', on && !!mount);
            if (mount) mount.classList.toggle('pc-fs-toolbar-on', on);
            b.innerHTML = on ? '&#x2715;' : '&#x26F6;';
            b.title = on ? 'Exit full screen (Esc)' : 'Full screen ' + (caption || 'grid') + ' (Esc to restore)';
            b.setAttribute('aria-label', b.title);
            b.setAttribute('aria-expanded', String(on));
        }
        b.addEventListener('click', function (e) {
            e.stopPropagation();
            var on = !box.classList.contains('pc-fs-on');
            var current = doc.querySelector('.pc-fs-on');
            if (on && current && current !== box && fullscreenControls.has(current)) fullscreenControls.get(current)(false);
            setFullscreen(on);
        });
        fullscreenControls.set(box, setFullscreen);
        if (box.id) b.setAttribute('aria-controls', box.id);
        setFullscreen(false);
        if (mount) mount.insertBefore(b, mount.firstChild);
        else box.insertBefore(b, box.firstChild);
    }
    doc.addEventListener('keydown', function (e) {
        if (e.key !== 'Escape') return;
        if (e.target.closest && e.target.closest('.gb-menu,.gb-chooser')) return;
        var open = doc.querySelector('.pc-fs-on');
        if (!open) return;
        var restore = fullscreenControls.get(open);
        if (restore) restore(false);
        e.stopPropagation();
    }, true);

    global.PurchaseChrome = { footer: footer, fullscreen: fullscreen, gridTools: gridTools };
}(window));
