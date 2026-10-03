/* ============================================================================================
 * countx_purchase_doc_attachments.js - the desktop "Attachment" form (AT.Show()) and the History
 * "NoOfAttachments" link (CommonServices.GetNoofAttachmentsByScreenName / ...ByRefDocumentTypeID) for
 * Goods Receipt Notes (46), Grn (Sale Return) (143), GRN Direct (137) and Stock In Transit (251).
 *
 *   var at = PurchaseDocAttachments.create({ type: 46, getId: fn, canEdit: fn, message: fn });
 *   at.open(button)      - the Attachment form of the record on the page: saved files (download /
 *                          Remove), new files staged until the document is saved (the desktop keeps
 *                          them in AT until Save/Update too).
 *   at.payload()         - undefined when nothing changed, else { files:[{name,base64}], removeAttachmentIds:[] }
 *                          to post as "attachments" with Save / Update.
 *   at.reset()           - New / after a save / after another record is opened.
 *   at.view(id, button)  - read-only list of a history row's attachments (the NoOfAttachments link).
 *
 * Server: GET /api/purchase/doc-attachments/{type}/{id}[/{attachmentId}] (PurchaseDocAttachmentController).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var doc = global.document;
    var BASE = '/api/purchase/doc-attachments/';
    function esc(v) { return String(v === undefined || v === null ? '' : v).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }

    function injectStyle() {
        if (doc.getElementById('pda-style')) return;
        var s = doc.createElement('style');
        s.id = 'pda-style';
        s.textContent =
            '.pda-dialog{position:fixed;top:8%;left:50%;transform:translateX(-50%);width:min(720px,96vw);max-height:84vh;display:flex;flex-direction:column;' +
            'background:#ece9d8;border:1px solid #008080;box-shadow:0 6px 24px rgba(0,0,0,.35);z-index:16000;padding:8px;font:12px Verdana,sans-serif;box-sizing:border-box}' +
            '.pda-dialog[hidden]{display:none}' +
            '.pda-dialog h2{background:#008080;color:#fff;font:bold 13px Tahoma,sans-serif;margin:-8px -8px 8px;padding:6px 10px;display:flex;justify-content:space-between;align-items:center}' +
            '.pda-dialog h2 button{background:#fff;border:1px solid #004d40;font:11px Tahoma,sans-serif;cursor:pointer;padding:1px 8px}' +
            '.pda-dialog .pda-note{margin:0 0 6px;color:#333}' +
            '.pda-dialog label{display:block;margin:6px 0}' +
            '.pda-dialog .pda-scroll{overflow:auto;flex:1 1 auto;min-height:60px}' +
            '.pda-dialog table{width:100%;border-collapse:collapse;background:#fff}' +
            '.pda-dialog td,.pda-dialog th{border:1px solid #aaa;padding:3px 6px;text-align:left}' +
            '.pda-dialog th{background:#dcd8c8}' +
            '.pda-dialog td button{font:11px Tahoma,sans-serif;cursor:pointer}';
        (doc.head || doc.documentElement).appendChild(s);
    }

    var pageBusy = null;
    function busy(button, work) {
        if (pageBusy) return pageBusy(button || null, work);
        if (global.PurchaseRequest) return global.PurchaseRequest.run(button || null, work);
        return Promise.resolve().then(work);
    }
    async function getJson(url) {
        var r = await fetch(url, { credentials: 'same-origin', headers: { Accept: 'application/json' } });
        if (r.redirected || r.status === 401) throw new Error('Please sign in to continue');
        var t = await r.text(), d = null;
        try { d = t ? JSON.parse(t) : null; } catch (e) { d = null; }
        if (!r.ok) throw new Error((d && (d.message || d.error)) || ('Attachments could not be loaded (' + r.status + ')'));
        return d || [];
    }

    function create(o) {
        injectStyle();
        var type = o.type, getId = o.getId, canEdit = o.canEdit || function () { return true; };
        var message = o.message || function (m) { global.alert(m); };
        if (o.busy) pageBusy = o.busy;          /* the page's own busy-button helper (e.g. HRM.busy) */
        var dlg = null, rows = [], files = [], removed = new Set(), changed = false, loadedFor = -1, generation = 0, viewOnly = false, viewId = 0;

        function ensure() {
            if (dlg) return;
            dlg = doc.createElement('section');
            dlg.className = 'pda-dialog';
            dlg.hidden = true;
            dlg.setAttribute('role', 'dialog');
            dlg.setAttribute('aria-label', 'Attachments');
            dlg.innerHTML = '<h2><span>Attachments</span><button type="button" data-close>Close</button></h2>' +
                '<p class="pda-note"></p>' +
                '<label class="pda-add">Add files <input type="file" multiple aria-label="Add files"></label>' +
                '<div class="pda-scroll"><table><thead><tr><th>File</th><th>Status</th><th style="width:80px">Remove</th></tr></thead><tbody></tbody></table></div>';
            doc.body.appendChild(dlg);
            dlg.querySelector('[data-close]').addEventListener('click', function () { dlg.hidden = true; });
            dlg.addEventListener('keydown', function (e) { if (e.key === 'Escape') { e.stopPropagation(); dlg.hidden = true; } });
            dlg.querySelector('input[type=file]').addEventListener('change', function (e) {
                var input = e.target, picked = Array.prototype.slice.call(input.files || []), version = generation;
                busy(null, async function () {
                    try {
                        if (!canEdit()) throw new Error('You do not have permission to change these attachments');
                        if (files.length + picked.length > 10) throw new Error('Select at most ten files at once');
                        picked.forEach(function (f) { if (!f.size || f.size > 5 * 1024 * 1024) throw new Error('Each file must be between 1 byte and 5 MB'); });
                        var add = [];
                        for (var i = 0; i < picked.length; i++) {
                            var f = picked[i];
                            var b64 = await new Promise(function (res, rej) {
                                var rd = new FileReader();
                                rd.onload = function () { res(String(rd.result).split(',')[1] || ''); };
                                rd.onerror = function () { rej(new Error('Could not read ' + f.name)); };
                                rd.readAsDataURL(f);
                            });
                            add.push({ name: f.name, base64: b64 });
                        }
                        if (version !== generation) return;
                        Array.prototype.push.apply(files, add);
                        changed = changed || add.length > 0;
                        render();
                    } catch (err) { message(err.message); }
                    finally { input.value = ''; }
                });
            });
            dlg.querySelector('tbody').addEventListener('click', function (e) {
                var b = e.target.closest('button'); if (!b || viewOnly || !canEdit()) return;
                if (b.hasAttribute('data-existing')) {
                    var id = Number(b.getAttribute('data-existing'));
                    if (removed.has(id)) removed.delete(id); else removed.add(id);
                } else if (b.hasAttribute('data-staged')) {
                    files.splice(Number(b.getAttribute('data-staged')), 1);
                }
                changed = true;
                render();
            });
        }

        function render() {
            if (!dlg) return;
            var edit = !viewOnly && canEdit();
            var id = viewOnly ? viewId : getId();
            dlg.querySelector('h2 span').textContent = viewOnly ? 'Attachments of the selected record' : 'Attachments';
            dlg.querySelector('.pda-note').textContent = viewOnly ? 'Open a file to view it.'
                : (edit ? 'Attachment changes are saved with the document (Save / Update).' : 'You can view the saved files.');
            dlg.querySelector('.pda-add').hidden = !edit;
            dlg.querySelector('thead th:last-child').hidden = !edit;
            var html = rows.map(function (r) {
                var rid = Number(r.Id || r.id), gone = removed.has(rid);
                return '<tr><td><a href="' + BASE + type + '/' + id + '/' + rid + '">' + esc(r.Attachment || r.attachment) + '</a></td><td>' +
                    (gone ? 'Remove on Save' : 'Saved') + '</td>' + (edit ? '<td><button type="button" data-existing="' + rid + '">' + (gone ? 'Undo' : 'Remove') + '</button></td>' : '') + '</tr>';
            }).join('');
            if (!viewOnly) html += files.map(function (f, i) {
                return '<tr><td>' + esc(f.name) + '</td><td>New - save the document</td>' + (edit ? '<td><button type="button" data-staged="' + i + '">Remove</button></td>' : '') + '</tr>';
            }).join('');
            if (!html) html = '<tr><td colspan="3">No attachments</td></tr>';
            dlg.querySelector('tbody').innerHTML = html;
        }

        function open(button) {
            ensure();
            var version = generation, id = getId();
            viewOnly = false;
            return busy(button, async function () {
                try {
                    if (id > 0 && loadedFor !== id) {
                        var list = await getJson(BASE + type + '/' + id);
                        if (version !== generation) return;
                        rows = list; loadedFor = id;
                    }
                    if (!(id > 0)) { rows = []; loadedFor = 0; }
                    render();
                    dlg.hidden = false;
                } catch (err) { message(err.message); }
            });
        }

        function view(id, button) {
            ensure();
            if (!(id > 0)) return Promise.resolve();
            return busy(button, async function () {
                try {
                    var list = await getJson(BASE + type + '/' + id);
                    viewOnly = true; viewId = id;
                    var keepRows = rows;
                    rows = list;
                    render();
                    rows = keepRows;          /* the form's own staged state is not disturbed */
                    dlg.hidden = false;
                } catch (err) { message(err.message); }
            });
        }

        function reset() {
            generation++;
            rows = []; files = []; removed = new Set(); changed = false; loadedFor = -1; viewOnly = false;
            if (dlg) { dlg.hidden = true; render(); }
        }

        return {
            open: open, view: view, reset: reset,
            payload: function () { return changed ? { files: files.slice(), removeAttachmentIds: Array.from(removed) } : undefined; }
        };
    }

    global.PurchaseDocAttachments = { create: create };
}(window));
