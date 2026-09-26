/* The generic report runner.
 *
 * One page for every ported Crystal report. A report is data - a contract row naming its
 * procedure and parameters - so this script never knows any report by name.
 *
 * Only arg: parameters are offered as inputs. session: values come from the signed-in user on
 * the server and const: values are fixed by the desktop caller; neither is ever sent from here,
 * which is what keeps OrganizationId and CompanyId out of the request. */
(function () {
    'use strict';

    var reports = [], view = [], current = null, contract = null, lastRun = null;
    var statusFilter = '', busy = false;

    function $(id) { return document.getElementById(id); }
    function esc(v) { return v === null || v === undefined ? '' : String(v); }
    function isNum(v) { return typeof v === 'number' || (v !== '' && v !== null && !isNaN(v) && !isNaN(parseFloat(v))); }

    function messageBox(text, buttons) {
        return new Promise(function (resolve) {
            $('mbBody').textContent = text;
            var box = $('mbBtns'); box.innerHTML = '';
            (buttons || ['OK']).forEach(function (b) {
                var btn = document.createElement('button');
                btn.type = 'button'; btn.textContent = b;
                btn.addEventListener('click', function () { $('mbMask').style.display = 'none'; resolve(b); });
                box.appendChild(btn);
            });
            $('mbMask').style.display = 'flex';
            var f = box.querySelector('button'); if (f) { f.focus(); }
        });
    }

    function setBusy(btn, on) {
        busy = on;
        ['btnRun', 'btnCsv', 'btnCrystal', 'btnPrint', 'btnRefresh'].forEach(function (id) { $(id).disabled = on; });
        if (btn) { btn.classList.toggle('busy', on); }
    }

    function api(url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin' }, opts || {}))
            .then(function (r) { return r.json().then(function (b) { return { ok: r.ok, b: b }; }); })
            .then(function (x) {
                if (!x.ok || x.b.success === false) {
                    throw new Error(x.b && x.b.message ? x.b.message : 'Request failed.');
                }
                return x.b;
            });
    }

    function runnable(r) {
        var s = esc(r.Status);
        return s === 'RUNNABLE_TRACED' || s === 'RUNNABLE_SEEDED';
    }

    /* ------------------------------------------------------------------ the catalogue */
    function loadList() {
        return api('/api/reports').then(function (b) {
            reports = b.reports || [];
            paintSummary(b);
            applyFilter();
        }).catch(function (e) { return messageBox(e.message); });
    }

    function paintSummary(b) {
        var s = b.summary || {}, by = s.byStatus || {}, reg = b.registry || {};
        var total = s.total || reports.length;
        var run = (by.RUNNABLE_TRACED || 0) + (by.RUNNABLE_SEEDED || 0);
        var html = '<b>' + total + ' Crystal templates</b> &middot; ' + run + ' runnable';
        if (reg.handTraced !== undefined) {
            html += ' (' + reg.handTraced + ' hand-traced, ' + reg.seeded + ' seeded)';
        }
        if (b.catalogueSeeded === false) {
            html += '<br><span style="color:#7f0000">The catalogue table is not seeded, so only the '
                  + 'registry’s own reports are listed. Run 03_ddl and 04_seed.</span>';
        }
        html += '<br>';
        [['', 'All', total],
         ['RUNNABLE_TRACED', 'Traced', by.RUNNABLE_TRACED || 0],
         ['RUNNABLE_SEEDED', 'Seeded', by.RUNNABLE_SEEDED || 0],
         ['PROCEDURE_ONLY', 'Procedure only', by.PROCEDURE_ONLY || 0],
         ['UNRESOLVED', 'Unresolved', by.UNRESOLVED || 0]].forEach(function (p) {
            html += '<span class="pill' + (statusFilter === p[0] ? ' on' : '') + '" data-s="' + p[0] + '">'
                  + p[1] + ' <span class="n">' + p[2] + '</span></span>';
        });
        $('summary').innerHTML = html;
        $('summary').querySelectorAll('.pill').forEach(function (el) {
            el.addEventListener('click', function () {
                statusFilter = el.getAttribute('data-s');
                paintSummary(b); applyFilter();
            });
        });
    }

    function applyFilter() {
        var q = $('search').value.trim().toLowerCase();
        view = reports.filter(function (r) {
            if (statusFilter && esc(r.Status) !== statusFilter) { return false; }
            if (!q) { return true; }
            return (esc(r.TemplateFileName) + ' ' + esc(r.Procedures) + ' ' + esc(r.DesktopMethod))
                   .toLowerCase().indexOf(q) >= 0;
        });
        paintList();
    }

    function paintList() {
        var l = $('list'); l.innerHTML = '';
        view.slice(0, 500).forEach(function (r) {
            var d = document.createElement('div');
            d.className = 'it' + (runnable(r) ? '' : ' dim') + (current === r ? ' sel' : '');
            var tag = esc(r.Status).replace('RUNNABLE_', '');
            d.innerHTML = '<div>' + esc(r.TemplateFileName) + '<span class="tag">' + tag + '</span></div>'
                        + '<div class="m">' + esc(r.Procedures || 'no procedure resolved') + '</div>';
            d.addEventListener('click', function () { select(r); });
            l.appendChild(d);
        });
        if (view.length > 500) {
            var more = document.createElement('div');
            more.className = 'it dim';
            more.textContent = '... ' + (view.length - 500) + ' more - narrow the filter';
            l.appendChild(more);
        }
    }

    /* ------------------------------------------------------------------ one report */
    function select(r) {
        current = r; contract = null; lastRun = null;
        paintList();
        $('out').innerHTML = '';
        $('head').querySelector('.t').textContent = esc(r.TemplateFileName);
        $('head').querySelector('.p').textContent =
            esc(r.Procedures) + (r.DesktopMethod ? '   ← ' + esc(r.DesktopMethod) : '')
            + (r.SourceFile ? '  (' + esc(r.SourceFile) + ':' + esc(r.SourceLine) + ')' : '');

        if (!runnable(r)) {
            $('params').innerHTML = '';
            $('why').style.display = '';
            $('why').textContent = refusalText(r);
            return;
        }
        $('why').style.display = 'none';
        $('params').innerHTML = '<span class="none">Loading the contract...</span>';
        api('/api/reports/' + encodeURIComponent(r.ReportKey) + '/contract')
            .then(function (c) { contract = c; paintParams(); })
            .catch(function (e) {
                $('params').innerHTML = '';
                $('why').style.display = ''; $('why').textContent = e.message;
            });
    }

    function refusalText(r) {
        if (esc(r.Status) === 'PROCEDURE_ONLY') {
            return 'Catalogued, not runnable.\n\nThe stored procedure is known (' + esc(r.Procedures)
                 + ') but its parameter SOURCES were not traced from the desktop - this report prints '
                 + 'the screen’s own grid, or its launch site builds the parameter object in a '
                 + 'shape the sweep could not read.\n\nRunning it would mean inventing the parameters, '
                 + 'so it is refused rather than served with invented values.\n\nDesktop caller: '
                 + esc(r.DesktopMethod) + ' (' + esc(r.SourceFile) + ':' + esc(r.SourceLine) + ')';
        }
        return 'Catalogued, not runnable.\n\nNo stored procedure could be resolved for this template '
             + 'from the desktop source.\n\nDesktop caller: ' + esc(r.DesktopMethod)
             + ' (' + esc(r.SourceFile) + ':' + esc(r.SourceLine) + ')';
    }

    /* Only the arg: parameters are the user's. session: and const: are shown read-only so the
       full call is visible, but they are never sent. */
    function paintParams() {
        var p = $('params'); p.innerHTML = '';
        var mine = contract.parameters.filter(function (x) { return x.userSupplied; });
        var fixed = contract.parameters.filter(function (x) { return !x.userSupplied; });

        if (!mine.length) {
            p.innerHTML = '<span class="none">No user parameters - this report takes only session '
                        + 'and constant values.</span>';
        }
        mine.forEach(function (x) {
            var lab = document.createElement('label');
            var nm = x.name.replace(/^@/, '');
            lab.innerHTML = '<span>' + x.name + (x.mode === 'GUARDED' ? ' (optional)' : '') + '</span>';
            var i = document.createElement('input');
            i.setAttribute('data-arg', x.source.substring(4));
            i.placeholder = x.mode === 'GUARDED' ? 'omitted when blank' : nm;
            lab.appendChild(i);
            p.appendChild(lab);
        });
        if (fixed.length) {
            var d = document.createElement('div');
            d.className = 'none';
            d.style.marginTop = '4px';
            d.textContent = 'Fixed by the desktop caller: '
                + fixed.map(function (x) { return x.name + '=' + x.source; }).join(', ');
            p.appendChild(d);
        }
    }

    /* ------------------------------------------------------------------ run */
    function run(btn) {
        if (!current) { return messageBox('Select a report first.'); }
        if (!contract) { return messageBox(refusalText(current)); }
        var args = {};
        $('params').querySelectorAll('input[data-arg]').forEach(function (i) {
            var v = i.value.trim();
            if (v !== '') { args[i.getAttribute('data-arg')] = v; }
        });
        setBusy(btn, true);
        return api('/api/reports/' + encodeURIComponent(contract.key) + '/run', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(args)
        }).then(function (b) {
            lastRun = b;
            paintOut(b);
        }).catch(function (e) {
            $('out').innerHTML = '';
            return messageBox(e.message);
        }).then(function () { setBusy(btn, false); });
    }

    function paintOut(b) {
        var o = $('out'); o.innerHTML = '';

        var banner = document.createElement('div');
        banner.id = 'banner';
        var rp = b.reportParameters || {};
        banner.innerHTML = '<b>' + esc(rp['@CompanyName']) + '</b><br>' + esc(rp['@CompanyAddress'])
            + '<br><span style="color:#555">' + esc(b.template) + ' &middot; ' + esc(b.procedure)
            + ' &middot; ' + (b.rows ? b.rows.length : 0) + ' row(s)</span>'
            + '<br><span style="color:#7a5">Data is faithful to the desktop; the Crystal page layout '
            + 'is not reproduced — it is not readable outside Crystal.</span>';
        o.appendChild(banner);

        o.appendChild(grid(esc(b.template), b.rows || []));
        (b.subReports || []).forEach(function (s) {
            o.appendChild(grid('Sub-report: ' + esc(s.template) + '  (' + esc(s.procedure) + ')', s.rows || []));
        });
    }

    function grid(caption, rows) {
        var w = document.createElement('div'); w.className = 'gwrap';
        var c = document.createElement('div'); c.className = 'gcap';
        c.textContent = caption + '  —  ' + rows.length + ' row(s)';
        w.appendChild(c);
        var s = document.createElement('div'); s.className = 'gscroll';
        if (!rows.length) {
            var none = document.createElement('div');
            none.style.padding = '10px';
            none.textContent = 'No Record Found For Display';
            s.appendChild(none);
            w.appendChild(s);
            return w;
        }
        var cols = Object.keys(rows[0]);
        var t = document.createElement('table'); t.className = 'grd';
        var thead = document.createElement('thead'), tr = document.createElement('tr');
        cols.forEach(function (k) { tr.appendChild(document.createElement('th')).textContent = k; });
        thead.appendChild(tr); t.appendChild(thead);
        var tb = document.createElement('tbody');
        rows.forEach(function (r) {
            var row = document.createElement('tr');
            cols.forEach(function (k) {
                var td = document.createElement('td');
                if (isNum(r[k])) { td.className = 'num'; }
                td.textContent = esc(r[k]);
                row.appendChild(td);
            });
            tb.appendChild(row);
        });
        t.appendChild(tb); s.appendChild(t); w.appendChild(s);
        return w;
    }

    function csv() {
        if (!lastRun || !lastRun.rows || !lastRun.rows.length) { return messageBox('Run a report first.'); }
        var cols = Object.keys(lastRun.rows[0]);
        var out = [cols.map(function (c) { return '"' + c.replace(/"/g, '""') + '"'; }).join(',')];
        lastRun.rows.forEach(function (r) {
            out.push(cols.map(function (c) { return '"' + esc(r[c]).replace(/"/g, '""') + '"'; }).join(','));
        });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(new Blob([out.join('\n')], { type: 'text/csv' }));
        a.download = esc(lastRun.template).replace(/\.rpt$/i, '') + '.csv';
        a.click();
        URL.revokeObjectURL(a.href);
    }

    /* ------------------------------------------------------------------ Crystal PDF
       The same rows /run returns, handed to the REAL Crystal engine running the REAL .rpt, so the
       PDF is the desktop's output rather than a redrawn imitation. Needs a Windows host with the
       Crystal runtime; when there is none the button says exactly why instead of failing oddly. */
    var crystalOk = false;

    function crystalStatus() {
        return fetch('/api/reports/print/status', { credentials: 'same-origin' })
            .then(function (r) { return r.json(); })
            .then(function (b) {
                crystalOk = !!b.available;
                $('btnCrystal').disabled = !crystalOk;
                $('btnCrystal').title = crystalOk
                    ? 'Render this report with the real Crystal engine (' + b.templateFiles + ' templates on disk)'
                    : (b.reason || 'Crystal printing is unavailable.');
                if (!crystalOk) {
                    $('crystalNote').style.display = '';
                    $('crystalNote').textContent = 'Crystal PDF unavailable \u2014 ' + (b.reason || '');
                }
            })
            .catch(function () { $('btnCrystal').disabled = true; });
    }

    function crystalPdf(btn) {
        if (!current) { return messageBox('Select a report first.'); }
        if (!contract) { return messageBox(refusalText(current)); }
        var args = {};
        $('params').querySelectorAll('input[data-arg]').forEach(function (i) {
            var v = i.value.trim();
            if (v !== '') { args[i.getAttribute('data-arg')] = v; }
        });
        setBusy(btn, true);
        return fetch('/api/reports/' + encodeURIComponent(contract.key) + '/print.pdf', {
            method: 'POST',
            credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(args)
        }).then(function (r) {
            var ct = r.headers.get('Content-Type') || '';
            if (!r.ok || ct.indexOf('application/pdf') < 0) {
                /* A failure comes back as readable text, never a corrupt PDF. */
                return r.text().then(function (t) { throw new Error(t || 'Printing failed.'); });
            }
            return r.blob().then(function (blob) {
                var url = URL.createObjectURL(blob);
                /* The desktop opens a viewer rather than downloading, so this does too. */
                window.open(url, '_blank');
                setTimeout(function () { URL.revokeObjectURL(url); }, 60000);
            });
        }).catch(function (e) {
            return messageBox(e.message);
        }).then(function () { setBusy(btn, false); });
    }

    /* ------------------------------------------------------------------ wiring */
    $('btnRun').addEventListener('click', function () { run($('btnRun')); });
    $('btnCsv').addEventListener('click', csv);
    $('btnCrystal').addEventListener('click', function () { crystalPdf($('btnCrystal')); });
    $('btnPrint').addEventListener('click', function () { window.print(); });
    $('btnRefresh').addEventListener('click', function () { loadList(); });
    $('search').addEventListener('input', applyFilter);
    document.addEventListener('keydown', function (e) {
        if (e.ctrlKey && e.key.toLowerCase() === 'r' && !busy) { e.preventDefault(); run($('btnRun')); }
    });

    loadList();
    crystalStatus();
})();
