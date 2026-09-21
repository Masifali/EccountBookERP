/* Shared non-blocking request feedback for report & data pages (fetch and jQuery/XHR). */
(function () {
    'use strict';
    let pending = 0, panel, showTimer, safetyTimer;

    function mount() {
        if (panel || !document.body) return;
        panel = document.createElement('div');
        panel.className = 'report-loading';
        panel.hidden = true;
        panel.setAttribute('role', 'status');
        panel.setAttribute('aria-live', 'polite');
        panel.innerHTML = '<div class="report-loading-card"><span class="report-loading-spinner" aria-hidden="true"></span><strong style="margin:0;">Loading...</strong></div>';
        const label = document.documentElement.dataset.loadingLabel;
        if (label) {
            panel.querySelector('strong').textContent = 'Loading ' + label + '…';
        }
        document.body.append(panel);
    }

    function hideLoading() {
        pending = 0;
        clearTimeout(showTimer);
        clearTimeout(safetyTimer);
        if (panel) panel.hidden = true;
        document.documentElement.removeAttribute('aria-busy');
        if (window.parent !== window) {
            try { window.parent.postMessage({type:'report-request-state', busy:false}, location.origin); } catch (_) {}
        }
    }

    function begin() {
        mount();
        if (pending++ === 0) {
            clearTimeout(showTimer);
            clearTimeout(safetyTimer);
            // 150ms debounce threshold so fast requests don't flash the indicator
            showTimer = setTimeout(() => {
                if (pending > 0 && panel) {
                    panel.hidden = false;
                    document.documentElement.setAttribute('aria-busy', 'true');
                    if (window.parent !== window) {
                        try { window.parent.postMessage({type:'report-request-state', busy:true}, location.origin); } catch (_) {}
                    }
                }
            }, 150);

            // Safety timeout: Auto-dismiss after 4 seconds max so loading NEVER sticks on screen
            safetyTimer = setTimeout(() => {
                hideLoading();
            }, 4000);
        }

        let finished = false;
        return function () {
            if (finished) return;
            finished = true;
            if (--pending <= 0) {
                hideLoading();
            }
        };
    }

    function isReportRequest(input) {
        try {
            const url = new URL(typeof input === 'string' || input instanceof URL ? input : input.url, location.href);
            return url.origin === location.origin && /\/api\//.test(url.pathname);
        } catch (_) { return false; }
    }

    const originalFetch = window.fetch;
    window.fetch = async function (input, options) {
        if (!isReportRequest(input)) return originalFetch.call(this, input, options);
        const end = begin();
        try {
            return await originalFetch.call(this, input, options);
        } finally { end(); }
    };

    const open = XMLHttpRequest.prototype.open, send = XMLHttpRequest.prototype.send;
    XMLHttpRequest.prototype.open = function (method, url) {
        this.reportRequest = isReportRequest(url);
        return open.apply(this, arguments);
    };
    XMLHttpRequest.prototype.send = function () {
        if (!this.reportRequest) return send.apply(this, arguments);
        const end = begin();
        this.addEventListener('loadend', end, {once: true});
        try { return send.apply(this, arguments); }
        catch (error) { end(); throw error; }
    };

    window.ReportLoading = {
        begin,
        setDisabled() {},
        localDate(date = new Date()) {
            return date.getFullYear() + '-' + String(date.getMonth() + 1).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0');
        }
    };
    document.addEventListener('DOMContentLoaded', mount, {once: true});
}());
