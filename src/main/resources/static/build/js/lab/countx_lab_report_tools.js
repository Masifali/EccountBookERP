/* Lab Report (module 1011) - small helpers shared by the six report pages (626, 629/632, 630, 631, 633).
   Loaded AFTER purchase/reports/pr_common.js (window.PR) and BEFORE the page script. Nothing here changes PR.

   LabRep.run(button, fn)        button contract: disabled at once + spinner (class btn-busy), further clicks and
                                 shortcut keys ignored while the request is in flight, re-enabled when fn's promise
                                 settles - success AND failure. fn still runs inside PR.run, so the page's loading
                                 banner and message box behave as before.
   LabRep.fill(id, rows, opts)   rebuild a <select> (the data-dtcombo component follows the option list by itself).
   LabRep.latest(name)           token for a cascading reload: only the newest response may fill the child combos.
   LabRep.fullscreen(btn, sec)   fullscreen toggle of a grid section (Esc leaves fullscreen, not the page).
   LabRep.open(url, id)          open a document page with ?id=<record id> in a new tab (the report keeps its rows). */
(function (global) {
    'use strict';
    var doc = global.document;
    function el(x) { return typeof x === 'string' ? doc.getElementById(x) : x; }

    function start(b) {
        if (!b) return;
        b.dataset.labWasDisabled = b.disabled ? '1' : '';
        b.disabled = true;
        b.classList.add('btn-busy');
        b.setAttribute('aria-busy', 'true');
    }
    function stop(b) {
        if (!b) return;
        b.classList.remove('btn-busy');
        b.removeAttribute('aria-busy');
        b.disabled = b.dataset.labWasDisabled === '1';
        delete b.dataset.labWasDisabled;
    }

    function run(button, fn) {
        var b = el(button);
        if (b && (b.classList.contains('btn-busy') || b.disabled)) return Promise.resolve();   // duplicate click / shortcut key
        start(b);
        return new Promise(function (resolve) {
            var called = false, done = false, rv;
            function finish() { if (done) return; done = true; stop(b); resolve(); }
            function body() {
                called = true;
                var p;
                try { p = fn(); } catch (e) { finish(); throw e; }
                if (p && typeof p.then === 'function') { p.then(finish, finish); return p; }
                finish();
                return p;
            }
            try {
                rv = global.PR && typeof global.PR.run === 'function' ? global.PR.run(body) : body();
            } catch (e) { finish(); throw e; }
            if (rv && typeof rv.then === 'function') rv.then(finish, finish);
            else if (!called) finish();                    // PR.run declined to start it: never leave the button stuck
        });
    }

    /** opts: value / text (row keys), zeroRow (text of a value-0 first row), blank (an empty first row),
        keep (true: the previous value stays selected when the new list still has it), select (value to select). */
    function fill(id, rows, opts) {
        var sel = el(id); if (!sel) return;
        opts = opts || {};
        var vk = opts.value || 'Id', tk = opts.text || 'ReferenceName';
        var previous = sel.value, frag = doc.createDocumentFragment(), seen = false, want;
        function add(v, t) {
            var o = doc.createElement('option');
            o.value = v; o.textContent = t;
            frag.appendChild(o);
        }
        if (opts.zeroRow) add('0', opts.zeroRow);
        else if (opts.blank) add('', '');
        (rows || []).forEach(function (r) {
            var v = r[vk] === null || r[vk] === undefined ? '' : String(r[vk]);
            add(v, r[tk] === null || r[tk] === undefined ? '' : String(r[tk]));
        });
        while (sel.firstChild) sel.removeChild(sel.firstChild);
        sel.appendChild(frag);
        want = opts.select !== undefined && opts.select !== null ? String(opts.select) : (opts.keep ? previous : null);
        if (want !== null && want !== '') {
            for (var i = 0; i < sel.options.length; i++) if (sel.options[i].value === want) { seen = true; break; }
        }
        if (seen) sel.value = want;
        else if (opts.zeroRow || opts.blank) sel.selectedIndex = 0;
        else sel.selectedIndex = -1;
    }

    var tokens = {};
    function latest(name) {
        var t = tokens[name] = (tokens[name] || 0) + 1;
        return function () { return tokens[name] === t; };
    }

    var fsSection = null, fsButton = null;
    function leaveFullscreen() {
        if (!fsSection) return;
        fsSection.classList.remove('lab-fs');
        doc.body.classList.remove('lab-fs-on');
        if (fsButton) { fsButton.textContent = 'Fullscreen'; fsButton.setAttribute('aria-pressed', 'false'); }
        fsSection = null; fsButton = null;
    }
    function fullscreen(button, section) {
        var b = el(button), s = el(section);
        if (!b || !s) return;
        b.setAttribute('aria-pressed', 'false');
        b.addEventListener('click', function () {
            if (fsSection === s) { leaveFullscreen(); return; }
            leaveFullscreen();
            fsSection = s; fsButton = b;
            s.classList.add('lab-fs');
            doc.body.classList.add('lab-fs-on');
            b.textContent = 'Exit Fullscreen';
            b.setAttribute('aria-pressed', 'true');
        });
    }
    /* Capture phase: the pages close on Esc; while a grid is fullscreen Esc only leaves fullscreen. */
    doc.addEventListener('keydown', function (e) {
        if (e.key !== 'Escape' || !fsSection || doc.querySelector('dialog[open]')) return;
        e.preventDefault();
        e.stopImmediatePropagation();
        leaveFullscreen();
    }, true);

    function open(url, id) {
        var n = parseInt(id, 10);
        if (!n || n <= 0) { if (global.PR && global.PR.box) global.PR.box('No Record Found For Display'); return; }
        global.open(url + '?id=' + encodeURIComponent(n), '_blank');
    }

    global.LabRep = { run: run, fill: fill, latest: latest, fullscreen: fullscreen, open: open };
}(window));
