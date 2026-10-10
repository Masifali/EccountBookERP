/*
 * Sale Pcc ("Concrete") shared client helpers, module 85 (Architecture.WinApp.pcc.Sale.*). Builds on window.SE (sale_engr.js).
 *   SPC.openPdf(url)            fetch a report PDF (the server renders the desktop .rpt template) and open it; JSON errors are shown as the desktop message box
 *   SPC.attachments(state, api) the Attachment form (AT.Show()): list, open, remove, add
 *   SPC.setRangeFromDateType()  cmbDateTypeHistory_ValueChanged (CommonServices.DateType: 1 day, 2 week, 3 month, 4 year, 5 financial year)
 *   SPC.fillDatalist(id, rows, key)  free-text combos (CmbReferenceParty, CmbVehicleNo are UltraCombos whose Text is read on the desktop)
 */
(function (w, d) {
    'use strict';
    var SPC = {};
    var SE = w.SE;

    SPC.openPdf = function (url) {
        return fetch(url, { credentials: 'same-origin' }).then(function (r) {
            var ct = r.headers.get('Content-Type') || '';
            if (!r.ok || ct.indexOf('pdf') < 0) {
                return r.text().then(function (t) {
                    var m = t;
                    try { var j = JSON.parse(t); m = j.message || j.error || t; } catch (x) { /* plain text */ }
                    throw new Error(m || 'No Record Found For Display');
                });
            }
            return r.blob().then(function (b) {
                var u = URL.createObjectURL(b);
                var win = w.open(u, '_blank');
                if (!win) w.location.href = u;
            });
        }).catch(function (e) { return SE.alert(e.message, 'Message'); });
    };

    SPC.fillDatalist = function (id, rows, key) {
        var dl = d.getElementById(id + '_dl');
        if (!dl) return;
        dl.innerHTML = (rows || []).map(function (r) { return '<option value="' + SE.esc(r[key]) + '"></option>'; }).join('');
    };

    /* state = { files: [], removedAtt: [], existing: [] } ; api = base url of the screen; id = current record id */
    SPC.attachments = function (state, api, id, readOnly, rows) {
        var list = rows || (state.existing || []).filter(function (r) { return state.removedAtt.indexOf(r.Id) < 0; });
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:110px">Action</th></tr></thead><tbody>';
        list.forEach(function (r) {
            h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (id > 0 ? '<a class="lnk" href="' + api + '/' + id + '/attachments/' + r.Id + '" target="_blank">Open</a>' : '') +
                (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
        });
        if (!readOnly) state.files.forEach(function (f, i) { h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
        h += '</tbody></table></div>';
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the record is saved.</span></div>';
        var pop = SE.pop('Attachments', h, [{ t: 'Close' }]);
        pop.open();
        function again() { pop.close(); SPC.attachments(state, api, id, false); }
        pop.body.addEventListener('click', function (e) {
            var t = e.target;
            var rm = t.getAttribute && t.getAttribute('data-rm');
            if (rm) { state.removedAtt.push(+rm); again(); }
            if (t.hasAttribute && t.hasAttribute('data-nf')) { state.files.splice(+t.getAttribute('data-nf'), 1); again(); }
        });
        var fi = pop.body.querySelector('#atFile');
        if (fi) fi.addEventListener('change', function () {
            var picked = Array.prototype.slice.call(fi.files), left = picked.length;
            if (!left) return;
            picked.forEach(function (f) {
                if (f.size > 5 * 1024 * 1024) { SE.alert(f.name + ' exceeds 5 MB'); if (--left <= 0) again(); return; }
                var fr = new FileReader();
                fr.onload = function () { state.files.push({ name: f.name, base64: String(fr.result).split(',')[1] || '' }); if (--left <= 0) again(); };
                fr.readAsDataURL(f);
            });
        });
        return pop;
    };

    /* cmbDateTypeHistory_ValueChanged */
    SPC.setRangeFromDateType = function (typeId, fromEl, toEl, fyStart) {
        var t = +typeId, now = new Date();
        function f(dt) { return dt.getFullYear() + '-' + ('0' + (dt.getMonth() + 1)).slice(-2) + '-' + ('0' + dt.getDate()).slice(-2); }
        if (t === 1) fromEl.value = SE.today();
        else if (t === 2) fromEl.value = SE.addDays(-7);
        else if (t === 3) { fromEl.value = f(new Date(now.getFullYear(), now.getMonth(), 1)); toEl.value = SE.today(); }
        else if (t === 4) { fromEl.value = f(new Date(now.getFullYear(), 0, 1)); toEl.value = SE.today(); }
        else if (t === 5) { var s = SE.dateInput(fyStart); if (s) fromEl.value = s; }
    };

    /* Janus format condition: IsFOC rows are drawn in red */
    SPC.focRows = function (gridEl, grid) {
        function mark() {
            var rows = grid.rows();
            gridEl.querySelectorAll('tbody tr[data-i]').forEach(function (tr) {
                var r = rows[+tr.getAttribute('data-i')];
                tr.classList.toggle('foc', !!(r && r.IsFOC));
            });
        }
        new MutationObserver(mark).observe(gridEl, { childList: true });
        mark();
        return mark;
    };

    w.SPC = SPC;
})(window, document);
