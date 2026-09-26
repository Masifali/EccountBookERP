(function () {
    'use strict';
    let pending = 0, clicked = null;
    const locks = new WeakMap();
    const panel = document.createElement('div');
    panel.className = 'purchase-loading';
    panel.hidden = true;
    panel.setAttribute('role', 'status');
    panel.innerHTML = '<span class="purchase-spinner" aria-hidden="true"></span> Loading…';
    document.body.appendChild(panel);

    document.addEventListener('click', function (event) {
        clicked = event.target.closest('button,input[type="button"],input[type="submit"]');
        queueMicrotask(function () { clicked = null; });
    }, true);

    function begin(button) {
        pending++;
        panel.hidden = false;
        document.documentElement.setAttribute('aria-busy', 'true');
        if (button) {
            let lock = locks.get(button);
            if (!lock) { lock = {count: 0, disabled: button.disabled}; locks.set(button, lock); }
            lock.count++;
            button.disabled = true;
            button.setAttribute('aria-busy', 'true');
        }
        let finished = false;
        return function () {
            if (finished) return;
            finished = true;
            if (button) {
                const lock = locks.get(button);
                if (--lock.count === 0) {
                    button.disabled = lock.disabled;
                    button.removeAttribute('aria-busy');
                    locks.delete(button);
                }
            }
            if (--pending === 0) { panel.hidden = true; document.documentElement.removeAttribute('aria-busy'); }
        };
    }

    window.PurchaseRequest = {
        isBusy(button) { return !!button && (locks.get(button)?.count || 0) > 0; },
        async run(button, work) {
            if (button && button.disabled) return;
            const finish = begin(button);
            try { return await work(); }
            finally { finish(); }
        },
        async track(work) {
            const finish = begin(clicked);
            try { return await work(); }
            finally { finish(); }
        },
        error(xhr) { return xhr.responseJSON?.message || xhr.responseJSON?.detail || xhr.message || 'The request failed. Please retry.'; }
    };

    // Existing desktop event conversions use jQuery. Track every lookup and chained request too.
    if (typeof $ !== 'undefined' && $.ajaxPrefilter) $.ajaxPrefilter(function (options, original, xhr) {
        if (!options.url.startsWith('/api/')) return;
        const finish = begin(clicked);
        xhr.always(function () { setTimeout(finish, 0); });
    });
}());
