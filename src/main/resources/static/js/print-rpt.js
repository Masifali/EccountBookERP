/*
 * Desktop print buttons -> Jasper PDF, with the SAME contract (procedure + parameters from the BLL /
 * seeder) the desktop uses.
 *
 * 1. Automatic: any button with data-rpt="<.rpt name>" prints that report. Its arguments are read from
 *    the page controls the desktop reads for it (print-controls.json, traced from the desktop launch
 *    site: obj.FromDate = txtFromDate.Value, obj.AccountId = cmbAccountTitle.Value, ...), then from the
 *    usual control names (txtFromDate, cmbItem, lblRecId, ...).
 * 2. By hand:  printRpt('105-AcRptGeneralLedger.rpt', { accountId: 58, fromDate: '2025-07-01' });
 *    or add  window.printRptArgs = function (rpt, args) { args.id = ...; return args; }  on a page.
 *
 * OrganizationId / CompanyId are never sent - the server takes them from the session.
 */
(function (w, d) {
    var controlsCache = {};

    function byIdCI(id) {
        if (!id) return null;
        var el = d.getElementById(id);
        if (el) return el;
        var low = id.toLowerCase(), all = d.querySelectorAll('[id]');
        for (var i = 0; i < all.length; i++) if (all[i].id.toLowerCase() === low) return all[i];
        var named = d.getElementsByName(id);
        return named && named.length ? named[0] : null;
    }

    function isoDate(v) {
        var m = /^(\d{1,2})[\/\-.](\d{1,2})[\/\-.](\d{4})$/.exec(v);          // dd/MM/yyyy -> yyyy-MM-dd
        return m ? m[3] + '-' + ('0' + m[2]).slice(-2) + '-' + ('0' + m[1]).slice(-2) : v;
    }

    function readEl(el) {
        if (!el) return undefined;
        if (el.type === 'checkbox' || el.type === 'radio') return el.checked ? 1 : 0;
        if (el.tagName === 'SELECT' && el.multiple)
            return Array.prototype.filter.call(el.options, function (o) { return o.selected; }).map(function (o) { return o.value; }).join(',');
        if ('value' in el && el.tagName !== 'BUTTON' && el.tagName !== 'LI') return isoDate(String(el.value).trim());
        return isoDate((el.textContent || '').trim());
    }

    function selectedGridId(col) {
        var row = d.querySelector('tr.selected, tr.active, tr.row-selected, tr.k-state-selected, tr.highlight, tr[aria-selected="true"]');
        if (!row) return undefined;
        var v = row.getAttribute('data-id') || row.getAttribute('data-rec-id') || (row.dataset && (row.dataset.id || row.dataset.recid));
        if (!v && col) { var c = row.querySelector('[data-col="' + col + '"], [data-field="' + col + '"]'); if (c) v = c.textContent.trim(); }
        return v || undefined;
    }

    function cap(s) { return s.charAt(0).toUpperCase() + s.slice(1); }

    function generic(arg) {
        var C = cap(arg), base = arg.replace(/Id$/, ''), B = cap(base);
        var names = [arg, C, 'txt' + C, 'cmb' + C, 'Cmb' + C, 'dtp' + C, 'hid' + C, 'hdn' + C,
                     'txt' + B, 'cmb' + B, 'Cmb' + B, 'cmb' + B + 'Name', 'Cmb' + B + 'Name', 'dtp' + B];
        if (arg === 'id') names = ['lblRecId', 'recId', 'RecId', 'Id', 'id', 'hdnId', 'hidId', 'txtId', 'documentId', 'txtRecId',
                                   'recordId', 'stId', 'masterId', 'headerId', 'hdnRecId', 'currentId', 'editId'];
        if (arg === 'fromDate') names = names.concat(['txtFromDate', 'dtpFromDate', 'FromDate', 'fromDate', 'txtDateFrom']);
        if (arg === 'toDate') names = names.concat(['txtToDate', 'dtpToDate', 'ToDate', 'toDate', 'txtDateTo']);
        for (var i = 0; i < names.length; i++) {
            var el = byIdCI(names[i]);
            if (el) { var v = readEl(el); if (v !== '' && v !== undefined && v !== null) return v; }
        }
        if (arg !== 'id') {
            /* a page variable of the same name (window.VoucherHeadId, exposed by the page's script) */
            var wv = w[C] !== undefined ? w[C] : w[arg];
            if ((typeof wv === 'number' || typeof wv === 'string') && wv !== '' && String(wv) !== '0' && ['name', 'origin', 'status'].indexOf(arg) < 0) return wv;
        }
        if (arg === 'id') {
            var q = new URLSearchParams(w.location.search);
            var hid = d.querySelector('input[type=hidden][id$="MasterId"], input[type=hidden][name="id"], input[name="Id"]');
            var v = w.currentId || w.recId || w.RecId || w.editId || w.editingVoucherId || w.selectedId || q.get('id') || (hid && hid.value) || selectedGridId();
            return v && String(v) !== '0' ? v : undefined;
        }
        return undefined;
    }

    function resolveArgs(rpt, info, given) {
        var args = {};
        (info.args || []).forEach(function (a) {
            if (given && given[a] !== undefined) { args[a] = given[a]; return; }
            var v;
            (info.controls && info.controls[a] || []).some(function (c) {
                v = c.indexOf('grid:') === 0 || c === 'grd' || c === 'grid' ? selectedGridId(c.split(':')[1]) : readEl(byIdCI(c));
                return v !== undefined && v !== '';
            });
            if (v === undefined || v === '') v = generic(a);
            if (v !== undefined && v !== '' && v !== null) args[a] = v;
        });
        /* A one-document print (slip / voucher / gate pass ...) with its document id: the desktop passes
           only that Id. The page's history filters (fromDate, toDate, status ...) are not sent, or the
           procedure would filter the document out. */
        if (args.id !== undefined && isOneDoc(rpt)) {
            Object.keys(args).forEach(function (k) {
                if (k !== 'id' && k !== 'documentTypeId' && !(given && given[k] !== undefined)) delete args[k];
            });
        }
        Object.keys(given || {}).forEach(function (k) { if (!(k in args)) args[k] = given[k]; });
        if (typeof w.printRptArgs === 'function') args = w.printRptArgs(rpt, args) || args;
        return args;
    }
    function isOneDoc(rpt) {
        return /slip|voucher|invoice|note|order|pass|bill|challan/i.test(rpt) && !/register|summary|ledger|report|history/i.test(rpt);
    }

    function info(rpt) {
        var k = rpt.toLowerCase();
        if (controlsCache[k]) return Promise.resolve(controlsCache[k]);
        return fetch('/api/print/controls?rpt=' + encodeURIComponent(rpt), { credentials: 'same-origin' })
            .then(function (r) { return r.ok ? r.json() : { args: [], controls: {} }; })
            .then(function (j) { controlsCache[k] = j; return j; });
    }

    function printRpt(rpt, given, btn) {
        var win = nativeOpen.call(w, 'about:blank', '_blank');
        return info(rpt).then(function (inf) {
            var args = resolveArgs(rpt, inf, given), q = new URLSearchParams();
            var miss = btn ? missing(btn, args) : [];
            if (miss.length) { win.close(); alert(btn.getAttribute('data-rpt-need-msg') || 'Select or save a record first.'); return null; }
            Object.keys(args).forEach(function (k) { q.set(k, args[k]); });
            return fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf?' + q.toString(), { credentials: 'same-origin' });
        }).then(function (r) {
            if (!r) return;
            var type = r.headers.get('Content-Type') || '';
            if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { win.location = URL.createObjectURL(b); });
            return r.text().then(function (t) { win.close(); alert(t || ('Print failed (' + r.status + ')')); });
        }).catch(function (e) { try { win.close(); } catch (x) { } alert(e.message); });
    }

    /* One button, several desktop reports: data-rpt-by names the control(s) that decide (the report-type
       combo, or the radio buttons), data-rpt-map maps that control's option text / value (or the checked
       radio's id) to the .rpt - the same switch the desktop's print handler makes. data-rpt is the default. */
    function norm(s) { return String(s == null ? '' : s).toLowerCase().replace(/[^a-z0-9]/g, ''); }
    function rptMap(b) {
        var m = b.getAttribute('data-rpt-map'); if (!m) return null;
        try { m = JSON.parse(m); } catch (x) { return null; }
        var nm = {}; Object.keys(m).forEach(function (k) { nm[norm(k)] = m[k]; }); return nm;
    }
    function pickRpt(b) {
        var rpt = b.getAttribute('data-rpt'), by = b.getAttribute('data-rpt-by'), nm = rptMap(b);
        if (!by || !nm) return rpt;
        var ids = by.split(',');
        for (var i = 0; i < ids.length; i++) {
            var el = byIdCI(ids[i].trim()); if (!el) continue;
            if (el.type === 'radio' || el.type === 'checkbox') { if (el.checked && nm[norm(el.id)]) return nm[norm(el.id)]; continue; }
            if (el.tagName === 'SELECT') {
                var o = el.options[el.selectedIndex];
                if (o && nm[norm(o.text)]) return nm[norm(o.text)];
                if (o && nm[norm(o.value)]) return nm[norm(o.value)];
                continue;
            }
            var v = readEl(el); if (nm[norm(v)]) return nm[norm(v)];
        }
        return rpt;
    }
    /* Fixed arguments (data-rpt-args='{"reportType":1}') and the row's own id for a print button inside
       a grid row (tr data-id), as the desktop's grid-button click passes the row's Id. */
    function givenArgs(b) {
        var g = {}, a = b.getAttribute('data-rpt-args');
        if (a) { try { g = JSON.parse(a) || {}; } catch (x) { g = {}; } }
        var iv = b.getAttribute('data-rpt-id-var');           // the page keeps the document id in window[<name>]
        if (iv && g.id === undefined && w[iv] !== undefined && w[iv] !== null && String(w[iv]) !== '0' && w[iv] !== '') g.id = w[iv];
        var tr = b.closest ? b.closest('tr[data-id]') : null;
        if (tr && g.id === undefined && tr.getAttribute('data-id')) {
            g.id = tr.getAttribute('data-id');
            if (g.voucherHeadId === undefined) g.voucherHeadId = g.id;
        }
        return g;
    }
    function missing(b, args) {
        var need = (b.getAttribute('data-rpt-need') || '').split(',').map(function (x) { return x.trim(); }).filter(Boolean);
        return need.filter(function (n) { return args[n] === undefined || args[n] === '' || String(args[n]) === '0'; });
    }

    // Arguments of every report on the page are fetched up front, so a click can be decided at once.
    function prefetch() {
        Array.prototype.forEach.call(d.querySelectorAll('[data-rpt]'), function (b) {
            info(b.getAttribute('data-rpt'));
            var nm = rptMap(b); if (nm) Object.keys(nm).forEach(function (k) { info(nm[k]); });
        });
    }
    if (d.readyState === 'loading') d.addEventListener('DOMContentLoaded', prefetch); else prefetch();

    /* Grid prints (data-rpt-grid): the desktop hands the SCREEN'S GRID to the .rpt, so the page sends
       the rows it shows - header text -> cell text - to POST /reports/print/grid. */
    function gridRows(tableId) {
        var t = byIdCI(tableId);
        if (!t) { try { t = d.querySelector(tableId); } catch (x) { t = null; } }   // a CSS selector also works
        if (t && t.tagName !== 'TABLE') t = (t.closest && t.closest('table')) || t.querySelector('table');
        if (!t) return [];
        var heads = [];
        var hr = t.tHead && t.tHead.rows.length ? t.tHead.rows[t.tHead.rows.length - 1] : t.rows[0];
        Array.prototype.forEach.call(hr ? hr.cells : [], function (c, i) {
            var h = (c.textContent || '').replace(/\s+/g, ' ').trim();
            heads.push(h || ('Col' + (i + 1)));
        });
        var body = t.tBodies.length ? t.tBodies[0].rows : Array.prototype.slice.call(t.rows, 1);
        var rows = [];
        Array.prototype.forEach.call(body, function (tr) {
            if (tr.offsetParent === null && tr.style.display === 'none') return;
            var cells = tr.cells;
            if (!cells.length || (cells.length === 1 && heads.length > 1)) return;   // "no rows" placeholder
            var r = {}, any = false;
            Array.prototype.forEach.call(cells, function (c, i) {
                var inp = c.querySelector('input:not([type=checkbox]),select,textarea'), chk = c.querySelector('input[type=checkbox]');
                var v = inp ? inp.value : chk ? (chk.checked ? 'Yes' : 'No') : (c.textContent || '').replace(/\s+/g, ' ').trim();
                var h = heads[i] || ('Col' + (i + 1));
                if (!h || /^(#|select|action|actions|edit|delete|)$/i.test(h)) return;
                r[h] = v; if (v !== '') any = true;
            });
            if (any) rows.push(r);
        });
        return rows;
    }

    function printGrid(rpt, tableId, title) {
        var rows = gridRows(tableId);
        if (!rows.length) { alert('No Record Found For Display'); return; }
        var win = nativeOpen.call(w, 'about:blank', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ rpt: rpt, title: title || null, rows: rows }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { win.location = URL.createObjectURL(b); });
                return r.text().then(function (t) { win.close(); alert(t || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { try { win.close(); } catch (x) { } alert(e.message); });
    }

    d.addEventListener('click', function (e) {
        var g = e.target && e.target.closest ? e.target.closest('[data-rpt-grid]') : null;
        if (!g || g.disabled) return;
        e.preventDefault(); e.stopImmediatePropagation();
        printGrid(g.getAttribute('data-rpt-grid'), g.getAttribute('data-grid'), g.getAttribute('data-title'));
    }, true);
    w.printGrid = printGrid;

    // capture phase: the Jasper print replaces the button's old handler (inline onclick or bound)...
    d.addEventListener('click', function (e) {
        var b = e.target && e.target.closest ? e.target.closest('[data-rpt]') : null;
        if (!b || b.disabled) return;
        /* data-rpt-own: the page's own handler prints this .rpt through the same (traced) contract and adds the
           desktop's checks (validation, "Not Record Found For Display" row check) - leave the click to it. */
        if (b.hasAttribute('data-rpt-own')) return;
        var rpt = pickRpt(b), inf = controlsCache[rpt.toLowerCase()], given = givenArgs(b);
        /* a one-document print (slip / voucher / invoice ...) whose document id the page does not expose */
        var oneDoc = isOneDoc(rpt);
        var slip = !b.hasAttribute('data-rpt-need') && inf && ((inf.required || []).indexOf('id') >= 0 || ((inf.args || []).indexOf('id') >= 0 && oneDoc));
        if (slip && resolveArgs(rpt, inf, given).id === undefined) {
            /* ...except a document slip whose current document id this page does not expose: the
               screen's own print (which knows its id) keeps working instead of printing every document. */
            if (w.console) console.info('print-rpt: no document id for ' + rpt + ' - using the page\'s own print');
            return;
        }
        e.preventDefault(); e.stopImmediatePropagation();
        printRpt(rpt, given, b);
    }, true);

    w.printRpt = printRpt;

    /* ---------------------------------------------------------------------------------------------
       Row prints -> Jasper. Several screens print the rows the desktop hands to the .rpt (the slip or
       register procedure's own DataTable) by writing an HTML table into a new window. Those writes are
       caught here and the table goes to POST /reports/print/grid, which lays the same rows out through
       the .rpt's Jasper template (converted template when its fields match, generated otherwise).
       Anything that is not a table (a picture slip, a page that sets location) passes through as before. */
    var nativeOpen = w.open;
    function tableRows(t) {
        var trs = Array.prototype.slice.call(t.rows); if (!trs.length) return { heads: [], rows: [] };
        var hr = t.tHead && t.tHead.rows.length ? t.tHead.rows[t.tHead.rows.length - 1] : trs[0];
        var heads = Array.prototype.map.call(hr.cells, function (c, i) { return (c.textContent || '').replace(/\s+/g, ' ').trim() || ('Col' + (i + 1)); });
        var rows = [];
        trs.forEach(function (tr) {
            if (tr === hr || (t.tHead && t.tHead.contains(tr))) return;
            if (tr.cells.length !== heads.length) return;
            var r = {}, any = false;
            Array.prototype.forEach.call(tr.cells, function (c, i) { var v = (c.textContent || '').replace(/\s+/g, ' ').trim(); r[heads[i]] = v; if (v) any = true; });
            if (any) rows.push(r);
        });
        return { heads: heads, rows: rows };
    }
    function htmlToGrid(html) {
        if (!/<table/i.test(html)) return null;
        var doc = new DOMParser().parseFromString(html, 'text/html');
        var tables = Array.prototype.slice.call(doc.querySelectorAll('table'));
        var best = null, extra = {};
        tables.forEach(function (t) { var g = tableRows(t); if (!best || g.rows.length > best.rows.length) best = g; t._g = g; });
        if (!best || !best.rows.length) return null;
        tables.forEach(function (t) {           // label/value tables (slip header) become header fields
            if (t._g === best) return;
            Array.prototype.forEach.call(t.rows, function (tr) {
                var c = tr.cells;
                for (var i = 0; i + 1 < c.length; i += 2) {
                    var k = (c[i].textContent || '').replace(/[:\s]+$/, '').replace(/\s+/g, ' ').trim(), v = (c[i + 1].textContent || '').replace(/\s+/g, ' ').trim();
                    if (k && k.length < 40 && !(k in best.rows[0])) extra[k] = v;
                }
            });
        });
        var rows = best.rows.map(function (r) { var o = {}; Object.keys(extra).forEach(function (k) { o[k] = extra[k]; }); Object.keys(r).forEach(function (k) { o[k] = r[k]; }); return o; });
        var h = doc.querySelector('h1,h2,h3'), title = (doc.title || (h && h.textContent) || '').replace(/\s+/g, ' ').trim();
        return { title: title, rows: rows };
    }
    function gridPdf(real, title, rows) {
        return fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ rpt: title, title: title, rows: rows }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { real.location = URL.createObjectURL(b); return true; });
                return false;
            }).catch(function () { return false; });
    }
    w.open = function (url, target, features) {
        var real = nativeOpen.apply(w, arguments);
        if (!real || (url && url !== 'about:blank')) return real;
        var buf = [], passed = false, mode = '', timer = null;
        function flush() { mode = 'html'; try { real.document.open(); real.document.write(buf.join('')); real.document.close(); } catch (x) { } }
        function finish() {
            if (passed) return; passed = true; clearTimeout(timer);
            var g = null;
            try { g = htmlToGrid(buf.join('')); } catch (x) { g = null; }
            if (!g) { flush(); return; }
            mode = 'grid';
            gridPdf(real, g.title, g.rows).then(function (ok) { if (!ok) flush(); });
        }
        var fakeDoc = {
            open: function () { buf = []; return fakeDoc; },
            write: function () { buf.push(Array.prototype.join.call(arguments, '')); clearTimeout(timer); timer = setTimeout(finish, 0); },
            writeln: function () { buf.push(Array.prototype.join.call(arguments, '') + '\n'); clearTimeout(timer); timer = setTimeout(finish, 0); },
            close: finish
        };
        if (typeof Proxy === 'undefined') return real;
        return new Proxy(real, {
            get: function (t, k) {
                if (k === 'document') return passed ? t.document : new Proxy(fakeDoc, { get: function (f, dk) { if (dk in f) return f[dk]; var v = t.document[dk]; return typeof v === 'function' ? v.bind(t.document) : v; } });
                if (k === 'print') return function () { if (mode === 'html') t.print(); };   // a PDF prints from its viewer
                var v = t[k]; return typeof v === 'function' ? v.bind(t) : v;
            },
            set: function (t, k, v) { if (k === 'location') { passed = true; mode = 'url'; } t[k] = v; return true; }
        });
    };
    w.printRowsJasper = function (title, rows) {
        if (!rows || !rows.length) { alert('No Record Found For Display'); return; }
        var real = nativeOpen.call(w, 'about:blank', '_blank');
        gridPdf(real, title, rows).then(function (ok) { if (!ok) { try { real.close(); } catch (x) { } alert('Print failed.'); } });
    };
})(window, document);
