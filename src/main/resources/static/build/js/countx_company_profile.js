/* frmCompanyProfile.cs behaviour, method for method.
 *
 * Reset(), btnnew_Click, btnsave_Click, btnupdate_Click, grd_DoubleClick, grd_KeyDown,
 * frmCompanyReport_KeyDown, browse_Click and MakeShortCutKeys are each reproduced below under the
 * desktop's own name, so the two can be read side by side. */
(function () {
    'use strict';

    var rows = [];
    var view = [];             // rows after the grid's dynamic filter
    var current = -1;          // grd.CurrentRow
    var RecId = 0;             // frmCompanyProfile.RecId
    var logoUnchanged = false; // the PictureBox holds the picked image until Save
    var token = null;          // SecurityConfiguration disables CSRF app-wide; see the controller

    var COLS = ['CompanyCode', 'CompanyName', 'OtherName', 'ReportingTitle', 'Email', 'Phone',
                'Country', 'State', 'Address', 'OtherAddress', 'ContactPerson'];

    function $(id) { return document.getElementById(id); }
    function esc(v) { return v === null || v === undefined ? '' : String(v); }

    /* ------------------------------------------------------------- MessageBox.Show */

    function messageBox(text, buttons) {
        return new Promise(function (resolve) {
            $('mbBody').textContent = text;
            var box = $('mbBtns');
            box.innerHTML = '';
            (buttons || ['OK']).forEach(function (b) {
                var btn = document.createElement('button');
                btn.type = 'button';
                btn.textContent = b;
                btn.addEventListener('click', function () {
                    $('mbMask').style.display = 'none';
                    resolve(b);
                });
                box.appendChild(btn);
            });
            $('mbMask').style.display = 'flex';
            var first = box.querySelector('button');
            if (first) { first.focus(); }
        });
    }

    /* ---------------------------------------------------------------------- Reset() */

    function Reset() {
        ['txtCode', 'txtName', 'txtReportingTitle', 'txtEmail', 'txtPhoneNumber', 'txtCountry',
         'txtState', 'txtAdress', 'txtContactPerson', 'txtAddressOtherLingo',
         'txtCompanynameOtherLingo'].forEach(function (f) { $(f).value = ''; });
        RecId = 0;
        logoUnchanged = false;               // LogoPic.Image = null
        setLogo(null);
        $('btnsave').style.display = '';     // btnsave.Visible = true
        $('btnupdate').style.display = 'none';
        var s = $('grdBody').querySelector('tr.sel');
        if (s) { s.classList.remove('sel'); }
        current = -1;
        navLabel();
    }

    function setLogo(dataUri) {
        var box = $('LogoPic');
        box.innerHTML = '';
        if (dataUri) {
            var img = document.createElement('img');
            img.src = dataUri;
            box.appendChild(img);
            box.dataset.uri = dataUri;
        } else {
            delete box.dataset.uri;
        }
    }

    /* ----------------------------------------------------------------- the grid (grd) */

    function buildFilterRow() {
        var tr = $('filterRow');
        tr.innerHTML = '';
        COLS.forEach(function (c) {
            var th = document.createElement('th');
            var i = document.createElement('input');
            i.dataset.col = c;
            i.addEventListener('input', applyFilter);
            th.appendChild(i);
            tr.appendChild(th);
        });
    }

    /* grd.DynamicFiltering = true — the filter row narrows the grid as you type. */
    function applyFilter() {
        var terms = {};
        $('filterRow').querySelectorAll('input').forEach(function (i) {
            var v = (i.value || '').trim().toLowerCase();
            if (v) { terms[i.dataset.col] = v; }
        });
        view = rows.filter(function (r) {
            for (var c in terms) {
                if (esc(r[c]).toLowerCase().indexOf(terms[c]) === -1) { return false; }
            }
            return true;
        });
        paint();
    }

    function paint() {
        var tb = $('grdBody');
        tb.innerHTML = '';
        view.forEach(function (r, i) {
            var tr = document.createElement('tr');
            COLS.forEach(function (c) {
                var td = document.createElement('td');
                td.textContent = esc(r[c]);
                tr.appendChild(td);
            });
            tr.addEventListener('click', function () { select(i); });
            tr.addEventListener('dblclick', function () { grd_DoubleClick(); });
            tb.appendChild(tr);
        });
        current = view.length ? 0 : -1;
        select(current);
    }

    function select(i) {
        var tb = $('grdBody');
        var s = tb.querySelector('tr.sel');
        if (s) { s.classList.remove('sel'); }
        current = i;
        if (i >= 0 && tb.rows[i]) {
            tb.rows[i].classList.add('sel');
            tb.rows[i].scrollIntoView({ block: 'nearest' });
        }
        navLabel();
    }

    function navLabel() {
        $('navPos').textContent = (current >= 0 ? current + 1 : 0) + ' of ' + view.length;
    }

    /* grd_DoubleClick — load the row, hide Save, show Update. */
    function grd_DoubleClick() {
        if (current < 0) { return; }
        var item = view[current];
        RecId = item.Id;
        $('txtCode').value                  = esc(item.CompanyCode);
        $('txtName').value                  = esc(item.CompanyName);
        $('txtCompanynameOtherLingo').value = esc(item.OtherName);
        $('txtReportingTitle').value        = esc(item.ReportingTitle);
        $('txtEmail').value                 = esc(item.Email);
        $('txtPhoneNumber').value           = esc(item.Phone);
        $('txtCountry').value               = esc(item.Country);
        $('txtState').value                 = esc(item.State);
        $('txtAdress').value                = esc(item.Address);
        $('txtAddressOtherLingo').value     = esc(item.OtherAddress);
        $('txtContactPerson').value         = esc(item.ContactPerson);
        setLogo(item.LogoImage || null);
        logoUnchanged = true;
        $('btnsave').style.display = 'none';
        $('btnupdate').style.display = '';
    }

    /* ------------------------------------------------ OrganizationFill + LocationsBind */

    function LocationsBind() {
        return fetch('/configurations/company-profile/api/load', { credentials: 'same-origin' })
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok) { return messageBox(x.b.message || 'Could not load.'); }
                token = x.b.token || null;
                $('CmbOrganizationName').value = esc(x.b.organizationName);
                rows = x.b.rows || [];
                applyFilter();
            })
            .catch(function (e) { return messageBox(e.message); });
    }

    /* ---------------------------------------------------------------------- Insert() */

    function Insert(isUpdate) {
        return messageBox(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?', ['Yes', 'No'])
            .then(function (answer) {
                if (answer !== 'Yes') { return; }        // DialogResult.No — return

                var box = $('LogoPic');
                var body = {
                    token:          token,
                    id:             isUpdate ? RecId : 0,   // btnsave_Click sets RecId = 0 first
                    code:           $('txtCode').value,
                    name:           $('txtName').value,
                    otherName:      $('txtCompanynameOtherLingo').value,
                    reportingTitle: $('txtReportingTitle').value,
                    contactPerson:  $('txtContactPerson').value,
                    email:          $('txtEmail').value,
                    phone:          $('txtPhoneNumber').value,
                    country:        $('txtCountry').value,
                    state:          $('txtState').value,
                    address:        $('txtAdress').value,
                    otherAddress:   $('txtAddressOtherLingo').value,
                    logo:           logoUnchanged ? null : (box.dataset.uri || null),
                    logoUnchanged:  logoUnchanged && isUpdate
                };

                return fetch('/configurations/company-profile/api/save', {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(body)
                })
                    .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
                    .then(function (x) {
                        if (!x.ok) { return messageBox(x.b.message || 'Save failed.'); }
                        return messageBox(x.b.message).then(function () {
                            rows = x.b.rows || [];          // Insert() ends with LocationsBind()
                            applyFilter();
                        });
                    })
                    .catch(function (e) { return messageBox(e.message); });
            });
    }

    /* ------------------------------------------------------------------ toolbar clicks */

    function btnnew_Click()    { Reset(); }
    function btnsave_Click()   { RecId = 0; Insert(false); }
    function btnupdate_Click() { Insert(true); }
    function btnRefresh_Click(){ LocationsBind(); }        // OrganizationFill()

    function visible(el) { return el.style.display !== 'none'; }

    /* --------------------------------------------------- frmCompanyReport_KeyDown */

    function keyDown(e) {
        /* e.KeyData == Keys.Return -> SendKeys.Send("{TAB}") */
        if (e.key === 'Enter' && !e.ctrlKey) {
            var f = Array.prototype.filter.call(
                document.querySelectorAll('#panel4 input.tb, #toolStrip1 button'),
                function (el) { return !el.disabled && el.offsetParent !== null; });
            var i = f.indexOf(document.activeElement);
            if (i > -1) { e.preventDefault(); f[(i + 1) % f.length].focus(); }
            return;
        }
        /* (Ctrl+E) or Escape -> Close() */
        if ((e.ctrlKey && e.key.toLowerCase() === 'e') || e.key === 'Escape') {
            e.preventDefault(); window.location.href = '/admin-panel'; return;
        }
        /* Ctrl+Alt -> MakeShortCutKeys() */
        if (e.ctrlKey && e.altKey) { e.preventDefault(); $('scMask').style.display = 'flex'; return; }

        if (!e.ctrlKey) { return; }
        var k = e.key.toLowerCase();
        if (k === 'n') { e.preventDefault(); btnnew_Click(); }
        if (k === 's' && visible($('btnsave')))   { e.preventDefault(); btnsave_Click(); }
        if (k === 'r') { e.preventDefault(); btnRefresh_Click(); }
        if (k === 'u' && visible($('btnupdate'))) { e.preventDefault(); btnupdate_Click(); }
        if (e.key === 'F5') { e.preventDefault(); $('CmbOrganizationName').focus(); }
        /* Ctrl+ArrowDown -> grd.Focus(); Ctrl+ArrowUp -> CmbOrganizationName.Focus() */
        if (e.key === 'ArrowDown') { e.preventDefault(); $('grdScroll').focus(); if (current < 0 && view.length) { select(0); } }
        if (e.key === 'ArrowUp')   { e.preventDefault(); $('CmbOrganizationName').focus(); }
        /* grd_KeyDown: Ctrl+Enter -> grd_DoubleClick() */
        if (e.key === 'Enter') { e.preventDefault(); grd_DoubleClick(); }
    }

    /* ------------------------------------------------------------------- browse_Click */

    function browse_Click() { $('logoFile').click(); }

    document.addEventListener('DOMContentLoaded', function () {
        if (!$('btnsave')) { return; }            // the Admin condition refused the form

        buildFilterRow();

        $('btnnew').addEventListener('click', btnnew_Click);
        $('btnRefresh').addEventListener('click', btnRefresh_Click);
        $('btnsave').addEventListener('click', btnsave_Click);
        $('btnupdate').addEventListener('click', btnupdate_Click);
        $('BtnShortCutkeys').addEventListener('click', function () { $('scMask').style.display = 'flex'; });
        $('scClose').addEventListener('click', function () { $('scMask').style.display = 'none'; });

        $('browse').addEventListener('click', browse_Click);
        $('logoFile').addEventListener('change', function (e) {
            var f = e.target.files && e.target.files[0];
            if (!f) { return; }
            /* myStream.Length > 5000000 -> "File Size Limit Exceeded" */
            if (f.size > 5000000) { messageBox('File Size Limit Exceeded'); e.target.value = ''; return; }
            var fr = new FileReader();
            fr.onload = function () { logoUnchanged = false; setLogo(fr.result); };
            fr.readAsDataURL(f);
            e.target.value = '';
        });

        $('grdScroll').setAttribute('tabindex', '0');
        $('navigator').addEventListener('click', function (e) {
            var n = e.target.dataset && e.target.dataset.nav;
            if (!n || !view.length) { return; }
            if (n === 'first') { select(0); }
            if (n === 'prev')  { select(Math.max(0, current - 1)); }
            if (n === 'next')  { select(Math.min(view.length - 1, current + 1)); }
            if (n === 'last')  { select(view.length - 1); }
        });

        /* base.KeyPreview = true — the form sees the key before the focused control does. */
        document.addEventListener('keydown', keyDown);

        LocationsBind();
    });
})();
