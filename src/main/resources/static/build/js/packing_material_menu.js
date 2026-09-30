(() => {
    'use strict';
    const links = [...document.querySelectorAll('.dbm-card, .dbs-card')];
    const indicator = document.getElementById('packingMenuLoading');
    let pending = false;
    const restore = () => {
        pending = false;
        indicator.hidden = true;
        links.forEach(link => { link.removeAttribute('aria-disabled'); link.removeAttribute('aria-busy'); });
    };
    links.forEach(link => {
        // The menu star is decorative; entry forms retain their existing event handlers.
        const icon = link.querySelector('.fa');
        if (link.classList.contains('dbs-card') && icon) icon.className = 'fa fa-star-o';
        if (icon) icon.setAttribute('aria-hidden', 'true');
        link.addEventListener('click', event => {
            if (event.button !== 0 || event.ctrlKey || event.metaKey || event.shiftKey || event.altKey) return;
            event.preventDefault();
            if (pending) return;
            pending = true;
            links.forEach(item => item.setAttribute('aria-disabled', 'true'));
            link.setAttribute('aria-busy', 'true');
            indicator.hidden = false;
            // Give the disabled state and loader a paint before starting navigation.
            requestAnimationFrame(() => requestAnimationFrame(() => {
                try { window.location.assign(link.href); }
                catch (error) { restore(); throw error; }
            }));
        });
    });
    window.addEventListener('pageshow', restore);
})();
