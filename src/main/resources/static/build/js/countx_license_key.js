/* Architecture.WinApp.LicenseKey — LicenseKey_Load, FillDate(), btnSubmit_Click.
 *
 * The only behaviour here that the desktop does not have is the button guard the brief asks for:
 * disable on click, show the loader, block a second request, re-enable on either outcome. */
(function () {
    'use strict';

    var token = null;
    var busy = false;

    function $(id) { return document.getElementById(id); }

    /* MessageBox.Show */
    function messageBox(text) {
        return new Promise(function (resolve) {
            $('mbBody').textContent = text;
            $('mbMask').style.display = 'flex';
            $('mbOk').focus();
            $('mbOk').onclick = function () { $('mbMask').style.display = 'none'; resolve(); };
        });
    }

    function setBusy(on) {
        busy = on;
        var b = $('btnSubmit');
        b.disabled = on;
        b.classList.toggle('busy', on);
    }

    /* DateTimePickerFormat.Short — the picker shows the short date of its Value. */
    function shortDate(v) {
        if (!v) { return ''; }
        var d = new Date(v);
        if (isNaN(d.getTime())) { return String(v); }
        var p = function (n) { return (n < 10 ? '0' : '') + n; };
        return p(d.getDate()) + '/' + p(d.getMonth() + 1) + '/' + d.getFullYear();
    }

    /* FillDate() */
    function FillDate() {
        return fetch('/utilities/license-key/api/load', { credentials: 'same-origin' })
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok) { return messageBox(x.b.message || 'Could not load.'); }
                token = x.b.token || null;
                $('txtLicenseExpiryDate').value = shortDate(x.b.licenseDate);
                /* BLL 0087 throws "License Not Found!" when tblCLV has no row; the desktop shows
                   it in a MessageBox from FillDate's own catch. */
                if (x.b.error) { return messageBox(x.b.error); }
            })
            .catch(function (e) { return messageBox(e.message); });
    }

    /* btnSubmit_Click */
    function btnSubmit_Click() {
        if (busy) { return; }                                  // no duplicate request
        var text = $('txtLicense').value;
        if (text === '') {                                     // txtLicense.Text == string.Empty
            messageBox('Enter License Please').then(function () { $('txtLicense').focus(); });
            return;
        }
        setBusy(true);
        fetch('/utilities/license-key/api/submit', {
            method: 'POST',
            credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token: token, license: text })
        })
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok) { return messageBox(x.b.message || 'Request failed.'); }
                return messageBox(x.b.message).then(function () {
                    $('txtLicenseExpiryDate').value = shortDate(x.b.licenseDate);  // FillDate()
                    $('txtLicense').value = '';
                    $('txtLicense').focus();
                });
            })
            .catch(function (e) { return messageBox(e.message); })
            .then(function () { setBusy(false); });             // re-enabled on success OR failure
    }

    document.addEventListener('DOMContentLoaded', function () {
        $('btnSubmit').addEventListener('click', btnSubmit_Click);
        FillDate().then(function () { $('txtLicense').focus(); });   // LicenseKey_Load
    });
})();
