/* ============================================================================================
 * Lab page kit - the acceptance rules every ported Lab form shares (screens 155, 156, 157, 158, 163).
 * Loaded AFTER countx_purchase_request.js and countx_desktop_combo.js, BEFORE the page's own script.
 * No dependency; if this file is missing the page scripts fall back to their previous behaviour.
 *
 *   LabKit.withBusy(btn, asyncFn)  rule 5: disable the button at once, spinner on it (class btn-busy),
 *                                  further clicks ignored while the request is in flight, re-enabled in
 *                                  finally (success AND failure). A button that is disabled for another
 *                                  reason (no Save right) does nothing, as on the desktop.
 *   LabKit.act(btn, asyncFn)       withBusy + the house loading panel + refusal shown as the desktop's
 *                                  MessageBox (alert).
 *   fullscreen                     rule 6: every <button data-full="elementId"> toggles class is-full on
 *                                  that element; Esc leaves fullscreen first (it does not close the form).
 *   combo clear                    rule 3: the desktop's UltraCombo loses its Value when its text is
 *                                  emptied. The shared drop-down restores the old text instead, so a
 *                                  picked City / Supplier / Item criterion could never be removed again.
 *                                  Here an emptied combo text clears the native select (selectedIndex -1)
 *                                  and raises change, so dependent combos and grids follow.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var doc = global.document;

    function msg(e) { return e && e.message ? e.message : String(e); }

    async function withBusy(btn, work) {
        if (btn) {
            if (btn.__labBusy || btn.disabled) return undefined;
            btn.__labBusy = true;
            btn.disabled = true;
            btn.classList.add('btn-busy');
            btn.setAttribute('aria-busy', 'true');
        }
        try {
            return await work();
        } finally {
            if (btn) {
                btn.disabled = false;
                btn.classList.remove('btn-busy');
                btn.removeAttribute('aria-busy');
                btn.__labBusy = false;
            }
        }
    }

    function act(btn, work) {
        var panel = global.PurchaseRequest && global.PurchaseRequest.run
            ? function (w) { return global.PurchaseRequest.run(null, w); }
            : function (w) { return w(); };
        return withBusy(btn || null, function () {
            return panel(async function () {
                try { await work(); } catch (e) { global.alert(msg(e)); }
            });
        });
    }

    // ------------------------------------------------------------------ fullscreen toggle
    function fullTarget() { return doc.querySelector('.lab-fullbox.is-full'); }
    function setFull(el, on) {
        el.classList.toggle('is-full', on);
        doc.documentElement.classList.toggle('lab-has-full', !!fullTarget());
        var b = doc.querySelector('[data-full="' + el.id + '"]');
        if (b) {
            b.title = on ? 'Exit full screen (Esc)' : 'Full screen';
            var i = b.querySelector('i');
            if (i) i.className = on ? 'fa fa-compress' : 'fa fa-expand';
        }
    }
    doc.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('[data-full]') : null;
        if (!b) return;
        var el = doc.getElementById(b.getAttribute('data-full'));
        if (el) setFull(el, !el.classList.contains('is-full'));
    });
    doc.addEventListener('keydown', function (e) {
        if (e.key !== 'Escape') return;
        var el = fullTarget();
        if (!el) return;
        e.preventDefault();
        e.stopPropagation();
        setFull(el, false);
    }, true);

    // ------------------------------------------------------------------ emptied combo text = no value
    doc.addEventListener('input', function (e) {
        var input = e.target;
        if (!input || !input.classList || !input.classList.contains('dtcombo-input')) return;
        if (input.value !== '') return;
        var wrap = input.closest('.dtcombo-wrap');
        var sel = wrap ? wrap.querySelector('select') : null;
        if (!sel || sel.disabled || sel.selectedIndex < 0) return;
        sel.selectedIndex = -1;
        ['input', 'change'].forEach(function (type) { sel.dispatchEvent(new Event(type, { bubbles: true })); });
    }, true);

    global.LabKit = { withBusy: withBusy, act: act, message: msg };
}(window));
