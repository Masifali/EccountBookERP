/* ============================================================================================
 * Numeric input guards - the desktop's KeyPress handlers, ported.
 *
 * NO jQuery. NO dependency. Pairs with countx_desktop_combo.js.
 *
 * Across the seven main Sale and Purchase forms the desktop wires 65 KeyPress handlers, and 51
 * of them do exactly one thing: reject keystrokes that are not part of a number.
 *
 *     CommonServices.OnlytextNumberFunction          (CommonServices.cs)
 *         e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
 *         -> digits only
 *
 *     CommonServices.OnlytextdecimelFunction
 *         rejects anything that is not a digit, a control char or '.'
 *         AND rejects a second '.' when the text already contains one
 *         -> digits and at most ONE decimal point
 *
 *     CommonServices.OnlytextdecimelFunctionWithMinus
 *         the same, plus '-'
 *
 * --------------------------------------------------------------------------------------------
 * WHY type="number" IS NOT ENOUGH
 * --------------------------------------------------------------------------------------------
 * 112 of the numeric fields on these screens are <input type="number">, which looks like it
 * already covers this. It does not: browsers accept 'e' and 'E' (exponent), a leading '+', and
 * in several engines more than one '.' inside a type=number box. The desktop rejects all of
 * those at the keystroke. So the decimal guard is applied to type="number" too - it only ever
 * removes characters the desktop would also have refused, and it never rejects a digit.
 *
 * --------------------------------------------------------------------------------------------
 * WHICH FIELDS
 * --------------------------------------------------------------------------------------------
 *   data-guard="integer" | "decimal" | "decimalMinus" | "vehicle"
 *       an explicit, traced mapping from a desktop control whose KeyPress handler is known.
 *
 *   <input type="number"> with no data-guard
 *       gets "decimal" - the conservative default, since a type=number field is a numeric field
 *       by construction. A field that must also take a minus carries data-guard="decimalMinus"
 *       rather than being guessed at.
 *
 * A text field with no marker is left completely alone: guessing which <input type="text"> holds
 * a number from its id would be exactly the kind of invention this port avoids.
 *
 * Paste and drop are checked too - the desktop's KeyPress cannot see a paste, but letting one
 * through would put letters in a field the operator cannot type letters into.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var doc = global.document;

    function guardOf(el) {
        var g = el.getAttribute('data-guard');
        if (g) return g;
        if ((el.getAttribute('type') || '').toLowerCase() === 'number') return 'decimal';
        return null;
    }

    function allows(kind, ch, valueBefore) {
        if (ch >= '0' && ch <= '9') return true;
        if (kind === 'integer') return false;
        if (ch === '.') return valueBefore.indexOf('.') < 0;   /* at most one, ditto desktop */
        if (ch === '-' && kind === 'decimalMinus') {
            /* Unconditional, because that is what the desktop does. OnlytextdecimelFunctionWithMinus
               rejects only non-digit/non-dot/non-minus characters and a SECOND dot - it never
               restricts the minus by position or count, so it will happily accept "7-5".

               My first version allowed the minus only as a leading character and only once. That
               is arguably better input hygiene and it is NOT what the form does, so it is not what
               goes here: a stricter guard would block an entry the operator can make on the
               desktop today, and this port does not tighten business rules it was not asked to. */
            return true;
        }
        return false;
    }

    /* Rebuilds a pasted string under the same rule rather than rejecting the paste outright -
       pasting "1,234.50" then getting nothing is worse than getting "1234.50". */
    function clean(kind, text) {
        if (kind === 'vehicle') {
            /* Same alphabet and the same 6+6 shape the KeyPress enforces, applied to the whole
               pasted string. Not a desktop behaviour (KeyPress never sees a paste) - the choice
               is the same one the decimal cleaner makes: keep what the field can legally hold. */
            var v = String(text).replace(/[^0-9A-Za-z-]/g, '');
            var d = v.indexOf('-');
            var L = (d >= 0 ? v.slice(0, d) : v).replace(/[^A-Za-z]/g, '').slice(0, 6);
            var D = (d >= 0 ? v.slice(d + 1) : '').replace(/[^0-9]/g, '').slice(0, 6);
            return d >= 0 ? (L + '-' + D) : L;
        }
        var out = '', seenDot = false, seenMinus = false;
        for (var i = 0; i < text.length; i++) {
            var c = text.charAt(i);
            if (c >= '0' && c <= '9') { out += c; continue; }
            if (kind === 'integer') continue;
            if (c === '.' && !seenDot) { out += c; seenDot = true; continue; }
            /* Paste is the one place a position rule IS applied: the desktop's KeyPress cannot
               see a paste at all, so there is no desktop behaviour to match, and a pasted
               "7-5-2" is far more likely to be a mangled code than a number the operator meant. */
            if (c === '-' && kind === 'decimalMinus' && !seenMinus && out.length === 0) {
                out += c; seenMinus = true;
            }
        }
        return out;
    }


    /* ------------------------------------------------------------------------------------------
     * The one KeyPress handler on these seven forms that is NOT one of the three CommonServices
     * bodies: Inward Gate Pass's txtvehicleno_KeyPress (InwardGatePass.cs:5526).
     *
     * It masks a registration to LETTERS(1-6) '-' DIGITS(1-6) and, when the operator types the
     * first digit after 1..6 leading letters and no hyphen is present yet, it INSERTS the hyphen
     * and lets the digit through - the desktop does not set e.Handled on that branch, so the
     * character still lands, one position further along. Reproduced here by mutating value and
     * caret and NOT calling preventDefault, which leaves the browser to insert the digit at the
     * caret we just moved. Ported branch for branch; nothing tightened, nothing relaxed.
     * ---------------------------------------------------------------------------------------- */
    function isLetter(c) { return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'); }
    function isDigit(c)  { return c >= '0' && c <= '9'; }

    function vehicleKeyPress(el, e, ch) {
        if (el.__vehInternal) return;
        if (!isLetter(ch) && !isDigit(ch) && ch !== '-') { e.preventDefault(); return; }

        var text = el.value || '';
        var start = (typeof el.selectionStart === 'number') ? el.selectionStart : text.length;

        if (ch === '-' && text.indexOf('-') >= 0) { e.preventDefault(); return; }

        var future = text.slice(0, start) + ch + text.slice(start);
        var dash = future.indexOf('-');
        var letters = dash >= 0 ? future.slice(0, dash) : future;
        var digits  = (dash >= 0 && dash < future.length - 1) ? future.slice(dash + 1) : '';

        if (isLetter(ch) && letters.length > 6) { e.preventDefault(); return; }
        if (isDigit(ch) && dash >= 0 && digits.length > 6) { e.preventDefault(); return; }

        if (isDigit(ch) && text.indexOf('-') < 0) {
            var n = 0;
            while (n < text.length && isLetter(text.charAt(n))) n++;   /* TakeWhile(char.IsLetter) */
            if (n >= 1 && n <= 6) {
                el.__vehInternal = true;                                /* == _internalChange */
                el.value = text.slice(0, n) + '-' + text.slice(n);
                try { el.setSelectionRange(start + 1, start + 1); } catch (err) { /* ignore */ }
                el.__vehInternal = false;
            }
        }
    }

    function attach(el) {
        if (el.__numGuard) return false;
        var kind = guardOf(el);
        if (!kind) return false;
        el.__numGuard = kind;

        el.addEventListener('keypress', function (e) {
            /* Control keys carry no printable character - IsControl in the desktop's terms. */
            if (e.ctrlKey || e.metaKey || e.altKey) return;
            var ch = e.key;
            if (!ch || ch.length !== 1) return;          /* Backspace, Tab, Enter, arrows … */
            if (kind === 'vehicle') { vehicleKeyPress(el, e, ch); return; }
            var before = el.value || '';
            if (!allows(kind, ch, before)) {
                e.preventDefault();                       /* == e.Handled = true */
            }
        });

        el.addEventListener('paste', function (e) {
            var cb = e.clipboardData || global.clipboardData;
            if (!cb) return;
            var text = cb.getData('text');
            if (text === null || text === undefined) return;
            var cleaned = clean(kind, String(text));
            if (cleaned === String(text)) return;         /* already acceptable */
            e.preventDefault();
            var start = el.selectionStart, end = el.selectionEnd;
            var v = el.value || '';
            if (typeof start === 'number' && typeof end === 'number') {
                el.value = v.slice(0, start) + cleaned + v.slice(end);
                var pos = start + cleaned.length;
                try { el.setSelectionRange(pos, pos); } catch (err) { /* type=number forbids it */ }
            } else {
                el.value = v + cleaned;
            }
            el.dispatchEvent(new Event('input', { bubbles: true }));
            el.dispatchEvent(new Event('change', { bubbles: true }));
        });

        el.addEventListener('drop', function (e) { e.preventDefault(); });
        return true;
    }

    function init(root) {
        var scope = root || doc;
        var list = scope.querySelectorAll('input[data-guard], input[type="number"]');
        var n = 0;
        for (var i = 0; i < list.length; i++) if (attach(list[i])) n++;
        return n;
    }

    /* Grid rows are built after load, so newly inserted inputs are guarded too. */
    var queued = false;
    function queue() {
        if (queued) return;
        queued = true;
        setTimeout(function () { queued = false; init(); }, 16);
    }

    function boot() {
        init();
        if (!global.MutationObserver) return;
        new MutationObserver(function (recs) {
            for (var i = 0; i < recs.length; i++) {
                var a = recs[i].addedNodes;
                for (var j = 0; j < a.length; j++) {
                    var nd = a[j];
                    if (nd.nodeType !== 1) continue;
                    if (nd.tagName === 'INPUT' || (nd.querySelector && nd.querySelector('input'))) {
                        queue(); return;
                    }
                }
            }
        }).observe(doc.body, { childList: true, subtree: true });
    }

    global.InputGuards = { init: init, attach: attach, clean: clean };

    if (doc.readyState === 'loading') doc.addEventListener('DOMContentLoaded', boot);
    else boot();
}(window));
