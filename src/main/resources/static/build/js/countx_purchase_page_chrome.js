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
 *
 * No framework; no dependency. Additive only - a page that does not call it is unaffected.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var doc = global.document;
    var STYLE_ID = 'purchase-chrome-style';

    function injectStyle() {
        if (doc.getElementById(STYLE_ID)) return;
        var s = doc.createElement('style');
        s.id = STYLE_ID;
        s.textContent =
            'html,body{max-width:100%;overflow-x:hidden}' +
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
            '.pc-fs-on{position:fixed!important;inset:6px 6px 40px 6px!important;z-index:9000!important;height:auto!important;max-height:none!important;' +
            'width:auto!important;overflow:auto!important;background:#fff;box-shadow:0 4px 24px rgba(0,0,0,.45)}';
        (doc.head || doc.documentElement).appendChild(s);
    }

    function el(x) { return typeof x === 'string' ? doc.querySelector(x) : x; }

    function footer(opts) {
        injectStyle();
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
        var box = el(container);
        if (!box || box.querySelector(':scope > .pc-fs-btn')) return;
        box.classList.add('pc-fs-host');
        var b = doc.createElement('button');
        b.type = 'button';
        b.className = 'pc-fs-btn';
        b.title = 'Full screen ' + (caption || 'grid') + ' (Esc to restore)';
        b.innerHTML = '&#x26F6;';
        b.addEventListener('click', function (e) {
            e.stopPropagation();
            var on = box.classList.toggle('pc-fs-on');
            b.innerHTML = on ? '&#x2715;' : '&#x26F6;';
        });
        box.insertBefore(b, box.firstChild);
    }
    doc.addEventListener('keydown', function (e) {
        if (e.key !== 'Escape') return;
        var open = doc.querySelector('.pc-fs-on');
        if (!open) return;
        open.classList.remove('pc-fs-on');
        var b = open.querySelector(':scope > .pc-fs-btn'); if (b) b.innerHTML = '&#x26F6;';
        e.stopPropagation();
    }, true);

    global.PurchaseChrome = { footer: footer, fullscreen: fullscreen };
}(window));
