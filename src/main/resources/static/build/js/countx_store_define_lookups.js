/* ============================================================================================
 * The two desktop lookup forms the Store screens open with their green "+" buttons, as modal
 * dialogs usable from any page (load after countx_store_common.js):
 *
 *   StoreDefine.openDepartment(onClose, opts)  — Architecture.WinApp.Lookups.Define_Department
 *                                                (base.Name "Define_Department", ClientSize 391 x 450)
 *   StoreDefine.openAsset(onClose, opts)       — Architecture.WinApp.Lookups.frmLookUpDefineAsset
 *                                                (base.Name "frmLookUpDefineAsset", ClientSize 906 x 610)
 *
 * opts.host = ScreenName of the opening page (StoreIssuanceDirect, frmDepartmentRequest,
 * frmPurchaseDemand, DepartmentRequestToConsumableStore) — the server checks rights with it.
 * onClose() runs after the dialog is closed (× / Ctrl+E / Esc).
 * API: /api/store/define/department, /api/store/define/asset (StoreDefineLookupsController).
 *
 * Geometry is the designer's, 1:1 in px, inside a window frame whose caption is the form's Text
 * ("EccountBookERP"). Both forms are WindowState = Maximized on the desktop; the dialog uses
 * ClientSize instead (web deviation — a maximized window would cover the page that opened it).
 * The dialog's CSS is injected here (no edits to countx_store.css).
 *
 * Desktop behaviour kept: see StoreDefineLookupsService (numbered list). In short —
 *   Define_Department: New / Save / Update (no delete); double click a grid row = edit;
 *   "Department Name Field Required"; after save/update the grid is refilled and the box cleared.
 *   frmLookUpDefineAsset: New / Save / Update with confirm boxes; "Asset Name Field Required";
 *   Reset clears the five text boxes only (combos keep their values); the history grid refreshes only
 *   on Show (not after a save); the History strip's New / Refresh buttons have no Click handler on the
 *   desktop and do nothing here either.
 *
 *   StoreDefine.openAssetCategory(onClose, opts) — Architecture.WinApp.StoreManagement.frmItemCatagoryStore
 *                                                with FormTypeId = 3, "Item Category (Fix Assets)"
 *                                                (base.Name "frmItemCatagoryStore", ClientSize 964 x 566)
 *   StoreDefine.openFixedAssetItem(onClose, opts) — Architecture.WinApp.FixedAsset.frmDefineAssets,
 *                                                "Add Fixed Assest Item" (base.Name "frmDefineAssets",
 *                                                ClientSize 1044 x 561)
 * The two are opened by the asset form's "+" next to Asset Category (frmLookUpDefineAsset.cs:449) and
 * next to Asset Item (:463); API /api/store/define/asset-category and /api/store/define/fixed-asset-item
 * (StoreDefineAssetsExtraController; behaviour notes in StoreDefineAssetsExtraService). On their close
 * the asset form re-reads Category / Item (selection kept) — web deviation, the desktop never re-reads.
 * Not ported: the grid-bar (ctrlGrdBar1) layout / print / export options; Attachment, Item Type,
 * Item Uom, Barcode Print, ShortCut Keys, Multi Lingo and the picture Browse buttons (disabled, the
 * reason is in each button's tooltip).
 * ============================================================================================ */
(function () {
    'use strict';

    var C = window.StoreCommon;
    var esc = C.esc, intOf = C.intOf;
    var DEP = '/api/store/define/department';
    var AST = '/api/store/define/asset';
    var stack = [];                                   // open dialogs, top-most last

    /* ------------------------------------------------------------------ CSS */
    function injectCss() {
        if (document.getElementById('sdl-style')) return;
        var s = document.createElement('style');
        s.id = 'sdl-style';
        s.textContent = [
            '.sdl-overlay{position:fixed;inset:0;background:rgba(0,0,0,.35);overflow:auto;}',
            '.sdl-win{position:absolute;left:50%;top:3vh;transform:translateX(-50%);background:#fff;border:1px solid #555;box-shadow:0 6px 24px rgba(0,0,0,.4);max-width:98vw;display:flex;flex-direction:column;}',
            '.sdl-cap{height:28px;flex:0 0 28px;background:#008080;color:#fff;display:flex;align-items:center;justify-content:space-between;padding:0 4px 0 8px;font:bold 13px Tahoma,sans-serif;user-select:none;}',
            '.sdl-cap button{background:transparent;border:none;color:#fff;font-size:18px;line-height:18px;cursor:pointer;padding:0 6px;}',
            '.sdl-cap button:hover{background:#c62828;}',
            '.sdl-frame{overflow:auto;max-height:calc(94vh - 28px);}',
            '.sdl-client{position:relative;background:#fff;font-family:"Microsoft Sans Serif",Tahoma,sans-serif;font-size:11px;display:flex;flex-direction:column;}',
            '.sdl-abs{position:absolute;box-sizing:border-box;margin:0;}',
            '.sdl-rel{position:relative;flex:0 0 auto;}',
            '.sdl-lbl{font-family:"Segoe UI Semibold","Segoe UI",Tahoma,sans-serif;font-weight:bold;font-size:12px;line-height:15px;white-space:nowrap;color:#000;}',
            '.sdl-txt{font-family:Verdana,sans-serif;font-size:13px;height:23px;border:1px solid #7a7a7a;padding:1px 3px;background:#fff;}',
            'textarea.sdl-txt{resize:none;overflow:hidden;line-height:18px;padding:2px 3px;}',
            '.sdl-cmb{font-family:Verdana,sans-serif;font-size:13px;height:26px;border:1px solid #7a7a7a;background:#fff;padding:0 2px;}',
            '.sdl-plus{background:#fff;border:1px solid #7a7a7a;padding:0;cursor:pointer;display:flex;align-items:center;justify-content:center;}',
            '.sdl-plus i{color:#2e9e3e;font-size:17px;line-height:1;}',
            '.sdl-plus:disabled{cursor:default;}',
            '.sdl-plus:disabled i{opacity:.55;}',
            '.sdl-strip{height:27px;flex:0 0 27px;display:flex;align-items:center;box-sizing:border-box;padding-left:2px;}',
            '.sdl-strip.white{background:#fff;}',
            '.sdl-strip.grey{background:linear-gradient(to bottom,#fcfcfc 0%,#e0e0e0 100%);border-bottom:1px solid #b5b5b5;}',
            '.sdl-tool{font-family:Verdana,sans-serif;font-size:13px;height:24px;background:transparent;border:1px solid transparent;display:inline-flex;align-items:center;justify-content:center;gap:4px;padding:0 4px;cursor:pointer;box-sizing:border-box;white-space:nowrap;color:#000;}',
            '.sdl-tool:hover:not(:disabled){background:#ffe4a0;border-color:#e59d00;}',
            '.sdl-hidden{display:none !important;}',
            '.sdl-teal{background:#008080;position:relative;flex:0 0 auto;}',
            '.sdl-teal-t{position:absolute;font-family:Tahoma,sans-serif;font-weight:bold;color:#fff;white-space:nowrap;}',
            '.sdl-gbar{width:45px;height:23px;background:transparent;color:#fff;border:1px solid transparent;font-size:14px;cursor:default;}',
            '.sdl-gb{position:absolute;box-sizing:border-box;}',
            '.sdl-gb::before{content:"";position:absolute;left:0;right:0;top:7px;bottom:0;border:1px solid #d5dfe5;border-radius:3px;pointer-events:none;}',
            '.sdl-gb>.sdl-gcap{position:absolute;left:6px;top:0;padding:0 2px;background:#fff;font:bold 13px Tahoma,sans-serif;line-height:14px;color:#000;}',
            '.sdl-dtc{display:flex;align-items:center;gap:2px;height:23px;border:1px solid #7a7a7a;background:#fff;padding:0 2px;}',
            '.sdl-dtc input[type=date]{border:none;font-family:Verdana,sans-serif;font-size:12px;height:18px;width:100%;min-width:0;padding:0;}',
            '.sdl-dtc input[type=checkbox]{margin:0;}',
            '.sdl-dtc input[type=date]:disabled{color:#8a8a8a;background:#fff;}',
            '.sdl-rd{display:inline-flex;align-items:center;gap:3px;font-family:"Segoe UI Semibold","Segoe UI",Tahoma,sans-serif;font-weight:bold;font-size:12px;white-space:nowrap;}',
            '.sdl-rd input{margin:0;}',
            '.sdl-show{background:#008080;color:#fff;border:1px solid #005959;font:bold 13px Verdana,sans-serif;padding:0;cursor:pointer;}',
            '.sdl-show:hover:not(:disabled){background:#005959;}',
            /* Janus GridEX: filter row, total row, record navigator */
            '.sdl-grid{display:flex;flex-direction:column;flex:1 1 auto;min-height:0;border-top:1px solid #9eb6ce;background:#fff;box-sizing:border-box;}',
            '.sdl-grid-scroll{flex:1 1 auto;overflow:auto;min-height:0;}',
            '.sdl-grid table{border-collapse:collapse;font-family:Verdana,sans-serif;font-size:11px;table-layout:fixed;}',
            '.sdl-grid th{background:linear-gradient(to bottom,#e3efff 0%,#c4dcf8 100%);border:1px solid #9eb6ce;font-weight:normal;padding:2px 4px;position:sticky;top:0;z-index:2;white-space:normal;text-align:center;overflow:hidden;}',
            '.sdl-grid tr.sdl-f td{background:#f4f8fd;padding:1px;position:sticky;z-index:2;}',
            '.sdl-grid tr.sdl-f input{width:100%;box-sizing:border-box;height:18px;border:1px solid #b9cde5;font-size:11px;padding:0 2px;}',
            '.sdl-grid td{border:1px solid #d5e1f0;padding:1px 4px;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;height:18px;}',
            '.sdl-grid tbody tr:hover{background:#fff0cf;}',
            '.sdl-grid tbody tr.is-cur{background:#d6ecff;}',
            '.sdl-grid tr.sdl-tot td{background:#eef3fa;border-top:1px solid #9eb6ce;position:sticky;bottom:0;}',
            '.sdl-grid td.sdl-sel,.sdl-grid th.sdl-sel{width:16px;padding:0;background:#e9eef5;}',
            '.sdl-nav{flex:0 0 22px;height:22px;display:flex;align-items:center;gap:6px;padding:0 6px;border-top:1px solid #9eb6ce;background:linear-gradient(to bottom,#f5f9ff 0%,#dde8f6 100%);font:11px Verdana,sans-serif;}',
            '.sdl-nav button{font-size:10px;height:17px;padding:0 4px;border:1px solid #9eb6ce;background:#fff;cursor:pointer;}',
            /* 2026-09-25f: Item Category (Fix Assets) / Add Fixed Assest Item */
            '.sdl-tool:disabled{color:#8a8a8a;cursor:default;}',
            '.sdl-tool:disabled i{opacity:.5;}',
            '.sdl-grid th.sdl-r,.sdl-grid td.sdl-r{text-align:right;}',
            '.sdl-grid td.sdl-c,.sdl-grid th.sdl-c{text-align:center;}',
            '.sdl-grid input[type=checkbox]{margin:0;vertical-align:middle;}',
            '.sdl-star{position:absolute;font:bold 13px "Segoe UI",Tahoma,sans-serif;color:#e00000;line-height:17px;}',
            '.sdl-chk{display:inline-flex;align-items:center;gap:4px;white-space:nowrap;}',
            '.sdl-chk input{margin:0;}',
            '.sdl-pic{border:1px solid #c8c8c8;background:#fff;display:flex;align-items:center;justify-content:center;color:#9a9a9a;font:11px Verdana,sans-serif;overflow:hidden;}',
            '.sdl-browse{background:#008080;color:#fff;border:1px solid #005959;font:bold 16px Verdana,sans-serif;cursor:pointer;}',
            '.sdl-browse:disabled{cursor:not-allowed;opacity:.75;}',
            '.sdl-tabpage{position:absolute;left:4px;top:4px;right:4px;bottom:25px;background:#fff;display:flex;flex-direction:column;}',
            '.sdl-tabbar{position:absolute;left:2px;right:2px;bottom:1px;height:23px;display:flex;align-items:flex-start;border-top:1px solid #a0a0a0;}',
            '.sdl-tabbtn{font:13px Verdana,sans-serif;height:21px;padding:0 8px;margin-right:1px;border:1px solid #a0a0a0;border-top:none;background:#ececec;cursor:pointer;}',
            '.sdl-tabbtn.on{background:#fff;height:23px;margin-top:-1px;border-top:1px solid #fff;}',
            '.sdl-strip.teal{background:#008080;}'
        ].join('\n');
        document.head.appendChild(s);
    }

    /* ------------------------------------------------------------------ window shell */
    function shell(width, height, caption) {
        injectCss();
        var z = 9100 + stack.length * 10;                // above .cx-modal (9000), below .cx-combo-pop (9600)
        var ov = document.createElement('div');
        ov.className = 'sdl-overlay';
        ov.style.zIndex = z;
        ov.innerHTML = '<div class="sdl-win" role="dialog" aria-modal="true" style="width:' + (width + 2) + 'px;">' +
            '<div class="sdl-cap"><span>' + esc(caption || 'EccountBookERP') + '</span><button type="button" data-x title="Close (Ctrl+E / Esc)">&times;</button></div>' +
            '<div class="sdl-frame"><div class="sdl-client" style="width:' + width + 'px;height:' + height + 'px;"></div></div></div>';
        document.body.appendChild(ov);
        var d = { el: ov, client: ov.querySelector('.sdl-client'), keys: null, onClose: null, closed: false };
        d.close = function () {
            if (d.closed) return;
            d.closed = true;
            ov.remove();
            var i = stack.indexOf(d); if (i >= 0) stack.splice(i, 1);
            if (typeof d.onClose === 'function') { try { d.onClose(); } catch (e) { /* page callback */ } }
        };
        ov.querySelector('[data-x]').onclick = d.close;
        stack.push(d);
        return d;
    }
    function q(d, sel) { return d.client.querySelector(sel); }

    /* KeyPreview + Form_KeyDown of both forms: Ctrl+N New, Ctrl+S Save (not in update mode),
       Ctrl+U Update (update mode), Ctrl+E / Esc close. Only the top-most dialog reacts. */
    window.addEventListener('keydown', function (e) {
        var d = stack[stack.length - 1];
        if (!d) return;
        if (e.target && e.target.closest && e.target.closest('.cx-combo-pop')) return;   // the combo search box owns Esc
        if (d.onKey) {                                                  // a form with its own Form_KeyDown
            if (d.onKey(e)) { e.preventDefault(); e.stopPropagation(); }
            return;
        }
        if (!d.keys) return;
        var k = (e.key || '').toLowerCase();
        var handled = false;
        if (e.ctrlKey && k === 'n') { d.keys.newRec(); handled = true; }
        else if (e.ctrlKey && k === 's') { if (!d.keys.updateMood()) d.keys.save(); handled = true; }
        else if (e.ctrlKey && k === 'u') { if (d.keys.updateMood()) d.keys.update(); handled = true; }
        else if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { d.close(); handled = true; }
        if (handled) { e.preventDefault(); e.stopPropagation(); }
    }, true);

    /* ------------------------------------------------------------------ Janus GridEX stand-in
       FilterMode automatic (filter row), TotalRow on (bottom), RecordNavigator on, AllowEdit false,
       DoubleClick on a data row. cols: [{key, caption, w, fmt, right, sum, check}] — hidden columns are
       simply omitted. sum: the total row shows fmt(sum of the column) (AggregateFunction Sum).
       check: a checkbox column bound to row[key] (true/false), with a header selector (UseHeaderSelector).
       opts.noFilter: no filter row (FilterMode None). */
    function grid(host, cols, onDbl, opts) {
        opts = opts || {};
        var rows = [], shown = [], cur = -1, filters = {};
        host.classList.add('sdl-grid');
        host.innerHTML = '<div class="sdl-grid-scroll"><table><colgroup></colgroup><thead></thead><tbody></tbody><tfoot></tfoot></table></div>' +
            '<div class="sdl-nav"><button type="button" data-n="first">|&lt;</button><button type="button" data-n="prev">&lt;</button>' +
            '<span data-pos>Record: 0 of 0</span><button type="button" data-n="next">&gt;</button><button type="button" data-n="last">&gt;|</button></div>';
        var t = host.querySelector('table');
        function cls(c) { return c.check ? ' class="sdl-c"' : (c.right ? ' class="sdl-r"' : ''); }
        function head() {
            t.querySelector('colgroup').innerHTML = '<col style="width:16px">' + cols.map(function (c) { return '<col style="width:' + c.w + 'px">'; }).join('');
            var w = 16; cols.forEach(function (c) { w += c.w; }); t.style.width = w + 'px';
            t.tHead.innerHTML = '<tr><th class="sdl-sel"></th>' + cols.map(function (c) {
                    return c.check ? '<th class="sdl-c"><input type="checkbox" data-hsel="' + esc(c.key) + '" title="Select all">' + (c.caption ? ' ' + esc(c.caption) : '') + '</th>'
                        : '<th>' + esc(c.caption) + '</th>';
                }).join('') + '</tr>' +
                (opts.noFilter ? '' : '<tr class="sdl-f"><td class="sdl-sel"></td>' + cols.map(function (c) {
                    return c.check ? '<td></td>' : '<td><input type="text" data-f="' + esc(c.key) + '"></td>';
                }).join('') + '</tr>');
            var hh = t.tHead.rows[0];
            t.tHead.querySelectorAll('tr.sdl-f td').forEach(function (td) { td.style.top = hh.offsetHeight + 'px'; });
            t.tHead.querySelectorAll('input[data-f]').forEach(function (inp) {
                inp.oninput = function () { filters[inp.getAttribute('data-f')] = inp.value.toLowerCase(); body(); };
            });
            t.tHead.querySelectorAll('input[data-hsel]').forEach(function (inp) {
                inp.onchange = function () {
                    var k = inp.getAttribute('data-hsel');
                    shown.forEach(function (r) { r[k] = inp.checked; });
                    body(true);
                };
            });
            foot();
        }
        function text(r, c) { var v = r[c.key]; return c.fmt ? c.fmt(v) : (v === null || v === undefined ? '' : String(v)); }
        function foot() {
            t.tFoot.innerHTML = '<tr class="sdl-tot"><td class="sdl-sel"></td>' + cols.map(function (c) {
                if (!c.sum || !shown.length) return '<td' + cls(c) + '></td>';
                var s = 0; shown.forEach(function (r) { var n = parseFloat(r[c.key]); if (!isNaN(n)) s += n; });
                return '<td' + cls(c) + '>' + esc(c.fmt ? c.fmt(s) : String(s)) + '</td>';
            }).join('') + '</tr>';
        }
        function body(keepCur) {
            shown = rows.filter(function (r) {
                return cols.every(function (c) { var f = filters[c.key]; return c.check || !f || text(r, c).toLowerCase().indexOf(f) >= 0; });
            });
            if (!keepCur || cur >= shown.length) cur = shown.length ? 0 : -1;
            t.tBodies[0].innerHTML = shown.map(function (r, i) {
                return '<tr data-i="' + i + '"><td class="sdl-sel"></td>' + cols.map(function (c) {
                    if (c.check) return '<td class="sdl-c"><input type="checkbox" data-ck="' + esc(c.key) + '"' + (r[c.key] ? ' checked' : '') + '></td>';
                    return '<td' + cls(c) + ' title="' + esc(text(r, c)) + '">' + esc(text(r, c)) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            foot();
            mark();
        }
        function mark() {
            Array.prototype.forEach.call(t.tBodies[0].rows, function (tr, i) { tr.classList.toggle('is-cur', i === cur); });
            host.querySelector('[data-pos]').textContent = 'Record: ' + (cur + 1) + ' of ' + shown.length;
        }
        t.tBodies[0].addEventListener('change', function (e) {
            var ck = e.target.closest('input[data-ck]'); if (!ck) return;
            var tr = ck.closest('tr[data-i]'); if (!tr) return;
            shown[parseInt(tr.getAttribute('data-i'), 10)][ck.getAttribute('data-ck')] = ck.checked;
        });
        t.tBodies[0].addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            cur = parseInt(tr.getAttribute('data-i'), 10); mark();
        });
        t.tBodies[0].addEventListener('dblclick', function (e) {
            if (e.target.closest('input')) return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            cur = parseInt(tr.getAttribute('data-i'), 10); mark();
            if (onDbl) onDbl(shown[cur]);
        });
        host.querySelector('.sdl-nav').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-n]'); if (!b || !shown.length) return;
            var n = b.getAttribute('data-n');
            cur = n === 'first' ? 0 : n === 'last' ? shown.length - 1 : n === 'prev' ? Math.max(0, cur - 1) : Math.min(shown.length - 1, cur + 1);
            mark();
            var tr = t.tBodies[0].rows[cur]; if (tr) tr.scrollIntoView({ block: 'nearest' });
        });
        head();
        return {
            set: function (list) { rows = list || []; t.style.visibility = ''; body(); },
            /** GridEX.ClearStructure(): no columns, no rows. */
            clear: function () { rows = []; body(); t.style.visibility = 'hidden'; },
            rows: function () { return rows; },
            focus: function () { var b = t.tBodies[0].querySelector('input,td'); host.querySelector('.sdl-grid-scroll').focus(); if (b && b.focus) b.focus(); },
            relayout: head
        };
    }

    /** "dd-MMM-yy hh:mm tt" (Gridbind:568). */
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function ddMMMyyhhmmtt(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || ''));
        if (!m) return v === null || v === undefined ? '' : String(v);
        var h = parseInt(m[4] || '0', 10), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1].slice(2) + ' ' + String(h).padStart(2, '0') + ':' + (m[5] || '00') + ' ' + ap;
    }

    function hostQs(host, extra) {
        var o = { host: host || '' };
        Object.keys(extra || {}).forEach(function (k) { o[k] = extra[k]; });
        return C.qs(o);
    }

    /** InfragisticsHelper.BindAndRetainSelection(insertDefaultRow: false): the list only, previous value kept.
        An empty source leaves the combo empty (Text = "", DataSource = null). The blank option = no value (AllowNull). */
    function bindRetain(sel, rows, valueKey, textKey) {
        var keep = sel.value;
        C.fillSelect(sel, rows, valueKey, textKey);
        if (keep && keep !== '0') { sel.value = keep; if (sel.value !== keep) sel.value = '0'; }
        else sel.value = '0';
    }

    /* ================================================================== Define_Department */

    function openDepartment(onClose, opts) {
        var host = (opts && opts.host) || '';
        var d = shell(391, 450);
        d.onClose = onClose;
        var updateMood = false, recId = 0;
        d.client.innerHTML =
            /* panel2 / toolStrip1 (0,0) 391 x 27, white, Verdana 9.75: btnnew 59, btnUpdate 78 (Visible false), btnsave 64 */
            '<div class="sdl-strip white">' +
            '<button type="button" class="sdl-tool" style="width:59px" data-a="new"><i class="fa fa-plus-circle" style="color:#2e7d32"></i><span><u>N</u>ew</span></button>' +
            '<button type="button" class="sdl-tool sdl-hidden" style="width:78px" data-a="update"><i class="fa fa-pencil" style="color:#1565c0"></i><span><u>U</u>pdate</span></button>' +
            '<button type="button" class="sdl-tool" style="width:64px" data-a="save"><i class="fa fa-floppy-o" style="color:#1565c0"></i><span><u>S</u>ave</span></button>' +
            '</div>' +
            /* panel3 / panel6 (0,27) 391 x 28 teal, label23 (3,4) Tahoma 12 bold */
            '<div class="sdl-teal" style="height:28px;"><span class="sdl-teal-t" style="left:3px;top:4px;font-size:16px;line-height:19px;">Department Define</span></div>' +
            /* panel5 (0,55) 391 x 39 white */
            '<div class="sdl-rel" style="height:39px;">' +
            '<label class="sdl-abs sdl-lbl" style="left:5px;top:13px;">Department Name</label>' +
            '<textarea class="sdl-abs sdl-txt" data-f="name" style="left:116px;top:7px;width:263px;height:26px;"></textarea>' +
            '</div>' +
            /* panel4 (0,94) 391 x 356: grdDefineDepartment Dock Fill */
            '<div data-g style="height:356px;"></div>';
        var txt = q(d, '[data-f="name"]');
        var g = grid(q(d, '[data-g]'), [{ key: 'DepartmentName', caption: 'DepartmentName', w: 200 }], rowDblClick);
        var bSave = q(d, '[data-a="save"]'), bUpdate = q(d, '[data-a="update"]');

        function gridFill() {                                                   // DepartmentDefineGridFill:93
            return C.getJson(DEP + '/list' + hostQs(host)).then(function (rows) { g.set(rows); });
        }
        function reset() {                                                      // Reset:64
            var p = gridFill();
            txt.value = '';
            updateMood = false;
            bSave.classList.remove('sdl-hidden'); bUpdate.classList.add('sdl-hidden');
            return p.catch(function (e) { alert(e.message); });
        }
        function validate() {                                                   // FormValidation:81
            if (txt.value.trim() === '') { alert('Department Name Field Required'); txt.focus(); return false; }
            return true;
        }
        function save() {                                                       // btnsave_Click:147
            if (!validate()) return;
            C.postJson(DEP + '/save', { DepartmentName: txt.value, host: host }).then(function (r) {
                alert(r.message);
                return reset();
            }).catch(function (e) { alert(e.message); });
        }
        function update() {                                                     // btnUpdate_Click:111
            if (!validate()) return;
            C.postJson(DEP + '/update', { Id: recId, DepartmentName: txt.value, host: host }).then(function (r) {
                alert(r.message);
                return reset();
            }).catch(function (e) { alert(e.message); });
        }
        function rowDblClick(row) {                                             // grdDefineCity_DoubleClick:196
            bSave.classList.add('sdl-hidden'); bUpdate.classList.remove('sdl-hidden');
            updateMood = true;
            recId = intOf(row.Id);
            C.getJson(DEP + '/' + recId + hostQs(host)).then(function (dep) {
                txt.value = dep.DepartmentName == null ? '' : dep.DepartmentName;
            }).catch(function (e) { alert(e.message); });
        }
        q(d, '[data-a="new"]').onclick = reset;                                  // btnnew_Click:188
        bSave.onclick = save;
        bUpdate.onclick = update;
        d.keys = { newRec: reset, save: save, update: update, updateMood: function () { return updateMood; } };

        gridFill().catch(function (e) { alert(e.message); });                  // Define_Department_Load:59
        txt.focus();
        return d;
    }

    /* ================================================================== frmLookUpDefineAsset */

    function openAsset(onClose, opts) {
        var host = (opts && opts.host) || '';
        var d = shell(906, 610);
        d.onClose = onClose;
        var updateMood = false, recId = 0;
        var today = C.today();
        function plus(dataA, left, top, title) {
            return '<button type="button" class="sdl-abs sdl-plus" tabindex="-1" data-a="' + dataA + '" style="left:' + left + 'px;top:' + top + 'px;width:26px;height:27px;"' +
                (title ? ' disabled title="' + esc(title) + '"' : '') + '><i class="fa fa-plus-circle"></i></button>';
        }
        d.client.innerHTML =
            /* panel2 / toolStrip1 (0,0) 906 x 27 white */
            '<div class="sdl-strip white">' +
            '<button type="button" class="sdl-tool" style="width:59px" data-a="new"><i class="fa fa-plus-circle" style="color:#2e7d32"></i><span><u>N</u>ew</span></button>' +
            '<button type="button" class="sdl-tool sdl-hidden" style="width:78px" data-a="update"><i class="fa fa-pencil" style="color:#1565c0"></i><span><u>U</u>pdate</span></button>' +
            '<button type="button" class="sdl-tool" style="width:64px" data-a="save"><i class="fa fa-floppy-o" style="color:#1565c0"></i><span><u>S</u>ave</span></button>' +
            '</div>' +
            /* panel3 / panel6 (0,27) 906 x 28 teal, label23 "Define Asset" (3,4) */
            '<div class="sdl-teal" style="height:28px;"><span class="sdl-teal-t" style="left:3px;top:4px;font-size:16px;line-height:19px;">Define Asset</span></div>' +
            /* panel5 (0,55) 906 x 116 white — DOM in TabIndex order (Category 0, Item 1, Brand 2, Name 3, Model 4, Condition 5, Department 6, Location 7) */
            '<div class="sdl-rel" style="height:116px;">' +
            '<label class="sdl-abs sdl-lbl" style="left:9px;top:11px;">Asset Category</label>' +
            '<select class="sdl-abs sdl-cmb win-combo" data-f="cat" style="left:117px;top:5px;width:211px;"></select>' +
            plus('cat', 329, 5, '') +
            '<label class="sdl-abs sdl-lbl" style="left:9px;top:38px;">Asset Item</label>' +
            '<select class="sdl-abs sdl-cmb win-combo" data-f="item" style="left:117px;top:32px;width:211px;"></select>' +
            plus('item', 329, 32, '') +
            '<label class="sdl-abs sdl-lbl" style="left:9px;top:65px;">Brand</label>' +
            '<input type="text" class="sdl-abs sdl-txt" data-f="brand" style="left:117px;top:61px;width:237px;">' +
            '<label class="sdl-abs sdl-lbl" style="left:9px;top:91px;">Asset Name</label>' +
            '<textarea class="sdl-abs sdl-txt" data-f="name" style="left:117px;top:85px;width:237px;height:26px;"></textarea>' +
            '<label class="sdl-abs sdl-lbl" style="left:360px;top:11px;">Model Description</label>' +
            '<input type="text" class="sdl-abs sdl-txt" data-f="model" style="left:469px;top:7px;width:237px;">' +
            '<label class="sdl-abs sdl-lbl" style="left:360px;top:38px;">Assets Condition</label>' +
            '<input type="text" class="sdl-abs sdl-txt" data-f="cond" style="left:469px;top:34px;width:237px;">' +
            '<label class="sdl-abs sdl-lbl" style="left:360px;top:65px;">Asset Deprtment</label>' +
            '<select class="sdl-abs sdl-cmb win-combo" data-f="dep" style="left:469px;top:59px;width:211px;"></select>' +
            plus('dep', 681, 59, '') +
            '<label class="sdl-abs sdl-lbl" style="left:360px;top:91px;">Asset Loacation </label>' +
            '<input type="text" class="sdl-abs sdl-txt" data-f="loc" style="left:469px;top:87px;width:237px;">' +
            '</div>' +
            /* panel4 (0,171) 906 x 439 white */
            '<div style="height:439px;display:flex;flex-direction:column;">' +
            /* toolStrip2 (0,0) 906 x 27 — BtnNewHistory / btnRefreshHistory have no Click handler on the desktop */
            '<div class="sdl-strip grey">' +
            '<button type="button" class="sdl-tool" style="width:59px"><i class="fa fa-plus-circle" style="color:#2e7d32"></i><span><u>N</u>ew</span></button>' +
            '<button type="button" class="sdl-tool" style="width:80px"><i class="fa fa-refresh" style="color:#2e7d32"></i><span>Refresh</span></button>' +
            '</div>' +
            /* panel7 (0,27) 906 x 22 teal, label1 "History" (5,1) */
            '<div class="sdl-teal" style="height:22px;"><span class="sdl-teal-t" style="left:5px;top:1px;font-size:16px;line-height:19px;">History</span></div>' +
            /* panel8 (0,49) 906 x 47 — groupBox2 (3,3) 894 x 43 "Filters" */
            '<div class="sdl-rel" style="height:47px;">' +
            '<div class="sdl-gb" style="left:3px;top:3px;width:894px;height:43px;"><span class="sdl-gcap">Filters</span>' +
            '<label class="sdl-abs sdl-lbl" style="left:3px;top:18px;">From Date</label>' +
            '<div class="sdl-abs sdl-dtc" style="left:67px;top:14px;width:133px;"><input type="checkbox" data-f="fromChk" checked><input type="date" data-f="from"></div>' +
            '<label class="sdl-abs sdl-lbl" style="left:201px;top:18px;">To Date</label>' +
            '<div class="sdl-abs sdl-dtc" style="left:249px;top:14px;width:133px;"><input type="checkbox" data-f="toChk" checked><input type="date" data-f="to"></div>' +
            '<label class="sdl-abs sdl-lbl" style="left:385px;top:18px;">Asset Category</label>' +
            '<select class="sdl-abs sdl-cmb win-combo" data-f="hcat" style="left:470px;top:12px;width:188px;height:26px;font-family:\'Microsoft Sans Serif\',Tahoma,sans-serif;font-size:13.6px;"></select>' +
            '<button type="button" class="sdl-abs sdl-show" data-a="show" style="left:661px;top:13px;width:58px;height:25px;">Show</button>' +
            '<label class="sdl-abs sdl-rd" style="left:722px;top:16px;height:19px;"><input type="radio" name="sdlDateType" data-f="rdEntry" checked> Entry Date</label>' +
            '<label class="sdl-abs sdl-rd" style="left:801px;top:16px;height:19px;"><input type="radio" name="sdlDateType" data-f="rdModify"> Modify Date</label>' +
            '</div></div>' +
            /* panel9 (0,96) 906 x 26 teal — label55 "Filtered Records" (3,2) Tahoma 11 bold, ctrlGrdBar1 (858,1) */
            '<div class="sdl-teal" style="height:26px;"><span class="sdl-teal-t" style="left:3px;top:2px;font-size:14.67px;line-height:18px;">Filtered Records</span>' +
            '<button type="button" class="sdl-abs sdl-gbar" style="left:858px;top:1px;" disabled title="Grid layout / print / export options are not ported"><i class="fa fa-cog"></i></button></div>' +
            /* grd (0,122) 906 x 317 */
            '<div data-g style="height:317px;"></div>' +
            '</div>';

        /* radio group name must be unique per open dialog */
        var rn = 'sdlDateType' + Date.now();
        d.client.querySelectorAll('input[name="sdlDateType"]').forEach(function (r) { r.name = rn; });

        var f = function (k) { return q(d, '[data-f="' + k + '"]'); };
        var cmbCat = f('cat'), cmbItem = f('item'), cmbDep = f('dep'), cmbHCat = f('hcat');
        var bSave = q(d, '[data-a="save"]'), bUpdate = q(d, '[data-a="update"]');
        f('from').value = today; f('to').value = today;                        // DateTimePicker default Value = Now, Checked
        ['from', 'to'].forEach(function (k) {
            var chk = f(k + 'Chk'), dt = f(k);
            chk.onchange = function () { dt.disabled = !chk.checked; };
        });

        /* Captions = the DataTable column names (RetrieveStructure); widths ≈ Constants.InventoryConstants
           (values not in the recovered source). EntryDate / ApprovedDate "dd-MMM-yy hh:mm tt". */
        var g = grid(q(d, '[data-g]'), [
            { key: 'Category', caption: 'Category', w: 150 },
            { key: 'AssetItemName', caption: 'AssetItemName', w: 200 },
            { key: 'AssetName', caption: 'AssetName', w: 200 },
            { key: 'Model', caption: 'Model', w: 90 },
            { key: 'Brand', caption: 'Brand', w: 200 },
            { key: 'Department', caption: 'Department', w: 150 },
            { key: 'AssetCondition', caption: 'AssetCondition', w: 80 },
            { key: 'EntryDate', caption: 'EntryDate', w: 130, fmt: ddMMMyyhhmmtt },
            { key: 'EntryUser', caption: 'EntryUser', w: 120 },
            { key: 'ApprovedDate', caption: 'ApprovedDate', w: 130, fmt: ddMMMyyhhmmtt },
            { key: 'ApprovedUser', caption: 'ApprovedUser', w: 120 }
        ], rowDblClick);

        function load() {                                                       // Define_Department_Load:141
            return C.getJson(AST + '/lookups' + hostQs(host)).then(function (l) {
                bindRetain(cmbCat, l.categories, 'Id', 'Category');             // ItemCategory:155
                bindRetain(cmbHCat, l.categories, 'Id', 'Category');
                bindRetain(cmbItem, l.items, 'Id', 'ItemName');                 // ItemFill:181
                bindRetain(cmbDep, l.departments, 'Id', 'Department');          // Department:194
                cmbCat.focus();
            }).catch(function (e) { alert(e.message); });
        }
        function reset() {                                                      // Reset:220 (combos and RecId kept)
            updateMood = false;
            bSave.classList.remove('sdl-hidden'); bUpdate.classList.add('sdl-hidden');
            f('cond').value = ''; f('loc').value = ''; f('name').value = ''; f('brand').value = ''; f('model').value = '';
            cmbCat.focus();
        }
        function insert() {                                                     // Insert():255
            if (f('name').value.trim() === '') {                                // FormValidation:243
                alert('Asset Name Field Required'); f('name').focus(); return;
            }
            var isUpdate = recId > 0;
            if (!confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            C.postJson(AST + (isUpdate ? '/update' : '/save'), {
                Id: recId,
                FixedAssetsCategoryId: intOf(cmbCat.value),
                ItemId: intOf(cmbItem.value),
                Brand: f('brand').value,
                AssetName: f('name').value,
                ModelDesc: f('model').value,
                AssetCondition: f('cond').value,
                AssetsDepartmentId: intOf(cmbDep.value),
                AssetLocationId: f('loc').value,
                host: host
            }).then(function (r) {
                alert(r.message);
                reset();
            }).catch(function (e) { alert(e.message); });
        }
        function save() { recId = 0; insert(); }                               // btnsave_Click:340
        function update() {                                                     // btnUpdate_Click:324
            if (recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
            insert();
        }
        function setCombo(sel, v) { if (intOf(v) > 0) sel.value = String(intOf(v)); }
        function readById(id) {                                                 // ReadById:386
            recId = id;
            C.getJson(AST + '/' + id + hostQs(host)).then(function (sc) {
                if (!sc) return;
                bSave.classList.add('sdl-hidden'); bUpdate.classList.remove('sdl-hidden');
                updateMood = true;
                setCombo(cmbCat, sc.FixedAssetsCategoryId);
                setCombo(cmbItem, sc.ItemId);
                f('brand').value = sc.Brand == null ? '' : sc.Brand;
                f('name').value = sc.AssetName == null ? '' : sc.AssetName;
                f('model').value = sc.ModelDesc == null ? '' : sc.ModelDesc;
                f('cond').value = sc.AssetCondition == null ? '' : sc.AssetCondition;
                setCombo(cmbDep, sc.AssetsDepartmentId);
                f('loc').value = sc.AssetLocationId == null ? '' : sc.AssetLocationId;
            }).catch(function (e) { alert(e.message); });
        }
        function rowDblClick(row) {                                             // grdDefineCity_DoubleClick:374
            var id = intOf(row.Id);
            readById(id);
        }
        function show() {                                                       // btnShow_Click:497 → Gridbind:505
            var entry = f('rdEntry').checked;
            C.getJson(AST + '/history' + hostQs(host, {
                dateType: entry ? 'entry' : 'modify',
                fromDate: f('fromChk').checked ? f('from').value : '',
                toDate: f('toChk').checked ? f('to').value : '',
                categoryId: intOf(cmbHCat.value)
            })).then(function (rows) {
                g.set(rows && rows.length ? rows : []);                          // no rows → DataSource = null
            }).catch(function (e) { alert(e.message); });
        }
        /* BtnDefineAssetDepartment_Click:474 → new Define_Department(UserAccount).Show().
           Web deviation: on close, cmbDepartment is re-filled (selection kept) — the desktop never
           re-reads it while this form is open. */
        q(d, '[data-a="dep"]').onclick = function () {
            openDepartment(function () {
                if (d.closed) return;
                C.getJson(AST + '/departments' + hostQs(host)).then(function (list) {
                    bindRetain(cmbDep, list, 'Id', 'Department');
                }).catch(function (e) { alert(e.message); });
            }, { host: host });
        };
        /* btnAssetCategory_Click:449 → new frmItemCatagoryStore(UserAccount){ FormTypeId = 3 }.Show();
           BtnAssetItemDefine_Click:463 → new frmDefineAssets(UserAccount).Show().
           Web deviation: on close, Category (both combos) and Item are re-read, selection kept. */
        function reloadCatItem() {
            if (d.closed) return;
            C.getJson(AST + '/lookups' + hostQs(host)).then(function (l) {
                if (d.closed) return;
                bindRetain(cmbCat, l.categories, 'Id', 'Category');
                bindRetain(cmbHCat, l.categories, 'Id', 'Category');
                bindRetain(cmbItem, l.items, 'Id', 'ItemName');
            }).catch(function (e) { alert(e.message); });
        }
        q(d, '[data-a="cat"]').onclick = function () { openAssetCategory(reloadCatItem, { host: host }); };
        q(d, '[data-a="item"]').onclick = function () { openFixedAssetItem(reloadCatItem, { host: host }); };
        q(d, '[data-a="new"]').onclick = reset;                                  // btnnew_Click:350
        bSave.onclick = save;
        bUpdate.onclick = update;
        q(d, '[data-a="show"]').onclick = show;
        d.keys = { newRec: reset, save: save, update: update, updateMood: function () { return updateMood; } };

        C.fillSelect(cmbCat, [], 'Id', 'Category'); C.fillSelect(cmbHCat, [], 'Id', 'Category');
        C.fillSelect(cmbItem, [], 'Id', 'ItemName'); C.fillSelect(cmbDep, [], 'Id', 'Department');
        load();
        return d;
    }

    /* ================================================================== shared bits of the two asset forms */

    var CATX = '/api/store/define/asset-category';
    var FAI = '/api/store/define/fixed-asset-item';

    /** txtItemCategorySerialFrom_KeyPress / CommonServices.OnlytextNumberFunction: digits and control keys only. */
    function digitsOnly(inp) {
        inp.addEventListener('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
            if (!/[0-9]/.test(e.key)) e.preventDefault();
        });
    }
    /** CommonServices.OnlytextdecimelFunction: digits, one '.', control keys. */
    function decimalOnly(inp) {
        inp.addEventListener('keypress', function (e) {
            if (e.ctrlKey || e.metaKey || e.key.length !== 1) return;
            if (/[0-9]/.test(e.key)) return;
            if (e.key === '.' && inp.value.indexOf('.') < 0) return;
            e.preventDefault();
        });
    }
    /** DropDownBind.BindDDL(ZeroIndex: false) + Rows[0].Activate(): the list, first row active (value). */
    function bindFirst(sel, rows, valueKey, textKey) {
        C.fillSelect(sel, rows, valueKey, textKey);
        sel.value = rows && rows.length ? String(rows[0][valueKey]) : '0';
        if (!sel.value) sel.value = '0';
    }
    /** DropDownBind.BindDDL / BindDDLNew(ZeroIndex: false) at load: the list, nothing selected. */
    function bindNone(sel, rows, valueKey, textKey) {
        C.fillSelect(sel, rows, valueKey, textKey);
        sel.value = '0';
    }
    /** UltraCombo.Value = v — a value that is not in the list leaves nothing selected. */
    function setVal(sel, v) {
        var s = String(v === null || v === undefined ? 0 : v);
        sel.value = s;
        if (sel.value !== s) sel.value = '0';
    }
    function tool(dataA, w, icon, color, label, extra) {
        return '<button type="button" class="sdl-tool' + (extra && extra.hidden ? ' sdl-hidden' : '') + '" style="width:' + w + 'px" data-a="' + dataA + '"' +
            (extra && extra.disabled ? ' disabled title="' + esc(extra.disabled) + '"' : '') +
            '><i class="fa ' + icon + '" style="color:' + color + '"></i><span>' + label + '</span></button>';
    }
    function lbl(text, left, top, extraStyle) {
        return '<label class="sdl-abs sdl-lbl" style="left:' + left + 'px;top:' + top + 'px;' + (extraStyle || '') + '">' + text + '</label>';
    }
    function txt(f, left, top, w, extra) {
        return '<input type="text" class="sdl-abs sdl-txt" data-f="' + f + '" style="left:' + left + 'px;top:' + top + 'px;width:' + w + 'px;"' + (extra || '') + '>';
    }
    function cmb(f, left, top, w) {
        return '<select class="sdl-abs sdl-cmb win-combo" data-f="' + f + '" style="left:' + left + 'px;top:' + top + 'px;width:' + w + 'px;"></select>';
    }
    /** "#,##0.###" (GridColumnSettings numeric default with DecimalCount 3). */
    function n3(v) {
        var n = parseFloat(v); if (isNaN(n)) return '';
        var s = (Math.round(n * 1000) / 1000).toFixed(3).replace(/\.?0+$/, '');
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return p.join('.');
    }
    /** DecimalRateFormate "#,#0." + configured rate decimals (2 assumed — the configured count is not read). */
    function rate2(v) {
        var n = parseFloat(v); if (isNaN(n)) return '';
        var p = n.toFixed(2).split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return p.join('.');
    }

    /* ================================================================== frmItemCatagoryStore, FormTypeId 3 */

    function openAssetCategory(onClose, opts) {
        var host = (opts && opts.host) || '';
        var d = shell(964, 566);
        d.onClose = onClose;
        var recId = 0, attributesFeature = false;
        var NO_LINGO = 'The Item Category Multi Lingo form (ItemCategoryMultiLanguage) is not available on the web';
        d.client.innerHTML =
            /* panel1 (0,0) 964 x 29 — toolStrip1: btnnew 59, btnRefresh 80, btnupdate 78 (hidden at Load), btnsave 64, btnMultiLingo 101 (ERP feature 7) */
            '<div class="sdl-strip grey" style="height:29px;flex:0 0 29px;">' +
            tool('new', 59, 'fa-plus-circle', '#2e7d32', '<u>N</u>ew') +
            tool('refresh', 80, 'fa-refresh', '#2e7d32', '<u>R</u>efresh') +
            tool('update', 78, 'fa-pencil', '#1565c0', '<u>U</u>pdate', { hidden: true }) +
            tool('save', 64, 'fa-floppy-o', '#1565c0', '<u>S</u>ave') +
            tool('lingo', 101, 'fa-language', '#1565c0', 'Multi Lingo', { hidden: true, disabled: NO_LINGO }) +
            '</div>' +
            /* panel2 (0,29) 964 x 25 teal — lblCategoryDescription (5,2) Tahoma 12 bold, chkstatus (870,2) "Is Active" Checked */
            '<div class="sdl-teal" style="height:25px;">' +
            '<span class="sdl-teal-t" style="left:5px;top:2px;font-size:16px;line-height:19px;">Item Category (Fix Assets)</span>' +
            '<label class="sdl-abs sdl-chk" style="left:870px;top:2px;height:20px;color:#fff;font:bold 13px Verdana,sans-serif;"><input type="checkbox" data-f="status" checked> Is Active</label>' +
            '</div>' +
            /* panel3 (0,54) 964 x 145 white (FormTypeId 3) */
            '<div class="sdl-rel" style="height:145px;">' +
            /* groupBox1 (0,0) 960 x 97 "Detail" Tahoma 8.25 bold — DOM in TabIndex-like visual order */
            '<div class="sdl-gb" style="left:0;top:0;width:960px;height:97px;"><span class="sdl-gcap" style="font-size:11px;">Detail</span>' +
            lbl('Code', 3, 18) + txt('code', 99, 14, 190) +
            lbl('Description', 3, 45) + txt('desc', 99, 41, 190) +
            lbl('Serial From &amp; To', 3, 72) + txt('sfrom', 99, 68, 89) + txt('sto', 190, 68, 99) +
            lbl('Accumulated A/c', 293, 18) + cmb('rev', 453, 12, 229) +
            lbl('Assets GL A/c', 293, 45) + cmb('inv', 453, 39, 229) +
            lbl('Depreciation A/c', 293, 72) + cmb('cgs', 453, 66, 229) +
            lbl('Parent Category', 685, 18) + cmb('parent', 779, 12, 176) +
            lbl('Class Group', 685, 45) + cmb('cls', 779, 39, 176) +
            lbl('Expense <br>Maintenance A/c', 685, 64, 'height:30px;display:flex;align-items:center;white-space:normal;line-height:14px;') + cmb('exp', 779, 66, 176) +
            '</div>' +
            /* groupBoxDepreciationMethod (0,98) 960 x 46 "Depreciation Method" Segoe UI 8.25 bold (visible for FormTypeId 3) */
            '<div class="sdl-gb" style="left:0;top:98px;width:960px;height:46px;"><span class="sdl-gcap" style="font:bold 11px \'Segoe UI\',Tahoma,sans-serif;">Depreciation Method</span>' +
            lbl('Method Name', 3, 21) + cmb('method', 99, 15, 229) +
            lbl('UseFull Life (Months)', 329, 21) + txt('months', 452, 17, 89) +
            lbl('Depreciation Rate %', 547, 21) + txt('rate', 668, 17, 89) +
            '</div>' +
            '</div>' +
            /* (0,199) 964 x 367 — PanelAttributes Dock Left 350 (ERP feature 13) + panel4 Dock Fill (history) */
            '<div style="height:367px;display:flex;">' +
            '<div data-attrpanel class="sdl-hidden" style="width:350px;flex:0 0 350px;display:flex;flex-direction:column;">' +
            '<div class="sdl-teal" style="height:23px;"><span class="sdl-teal-t" style="left:3px;top:2px;font-size:12px;line-height:14px;">Attributes</span></div>' +
            '<div data-ga style="flex:1 1 auto;min-height:0;"></div>' +
            '</div>' +
            '<div style="flex:1 1 auto;min-width:0;display:flex;flex-direction:column;">' +
            /* panel5 teal 23 — label1 "History" (4,2) Tahoma 12 bold, ctrlGrdBar1 at the right */
            '<div class="sdl-teal" style="height:23px;"><span class="sdl-teal-t" style="left:4px;top:2px;font-size:16px;line-height:19px;">History</span>' +
            '<button type="button" class="sdl-abs sdl-gbar" style="right:5px;top:0;height:22px;" disabled title="Grid layout / print / export options are not ported"><i class="fa fa-cog"></i></button></div>' +
            '<div data-g style="flex:1 1 auto;min-height:0;"></div>' +
            '</div></div>';

        var f = function (k) { return q(d, '[data-f="' + k + '"]'); };
        var cmbRev = f('rev'), cmbInv = f('inv'), cmbCgs = f('cgs'), cmbParent = f('parent'), cmbCls = f('cls'),
            cmbExp = f('exp'), cmbMethod = f('method');
        var bSave = q(d, '[data-a="save"]'), bUpdate = q(d, '[data-a="update"]'), bLingo = q(d, '[data-a="lingo"]');
        var attrPanel = q(d, '[data-attrpanel]');
        digitsOnly(f('sfrom')); digitsOnly(f('sto')); digitsOnly(f('months')); decimalOnly(f('rate'));

        /* gridsetting:743 — FormTypeId 3 hides Id, the three *AccountId, InventoryParentCategoriesId and the
           CGS / Revenue / Inventory titles. Captions = column names (RetrieveStructure), HeaderLines 3.
           Numeric columns: "#,##0.###" and summed in the total row (GridWrappingAndColumnSettings(grd, 3, 3)). */
        var g = grid(q(d, '[data-g]'), [
            { key: 'CategoryCode', caption: 'CategoryCode', w: 100 },
            { key: 'CategoryDescription', caption: 'CategoryDescription', w: 150 },
            { key: 'SerialFrom', caption: 'SerialFrom', w: 100, right: true, sum: true, fmt: n3 },
            { key: 'SerialTo', caption: 'SerialTo', w: 100, right: true, sum: true, fmt: n3 },
            { key: 'CategoryStatus', caption: 'CategoryStatus', w: 100 },
            { key: 'ParentCategory', caption: 'ParentCategory', w: 120 },
            { key: 'AccumulatedDepreciationAccount', caption: 'AccumulatedDepreciationAccount', w: 100 },
            { key: 'AssetGlAccount', caption: 'AssetGlAccount', w: 100 },
            { key: 'DepreciationExpenseAccount', caption: 'DepreciationExpenseAccount', w: 100 },
            { key: 'ExpenseMaintenanceAccount', caption: 'ExpenseMaintenanceAccount', w: 200 },
            { key: 'ClassGroupName', caption: 'ClassGroupName', w: 120 },
            { key: 'DepreciationMethodName', caption: 'DepreciationMethodName', w: 120 },
            { key: 'UsefullLifeInMonths', caption: 'UsefullLifeInMonths', w: 120, right: true, sum: true, fmt: n3 },
            { key: 'DepreciationRate', caption: 'DepreciationRate(%)', w: 100, right: true, fmt: rate2 }
        ], function (row) { readById(intOf(row.Id)); });                        // grdfrm_DoubleClick:779
        /* GrdSetting:268 — Select (selector, header selector) 30, Name 85, Description 135, Status 85. */
        var ga = grid(q(d, '[data-ga]'), [
            { key: 'sel', caption: '', w: 30, check: true },
            { key: 'Name', caption: 'Name', w: 85 },
            { key: 'Description', caption: 'Description', w: 135 },
            { key: 'Status', caption: 'Status', w: 85 }
        ], null);

        /* gridfill:687 — rows are replaced only when the procedure returns some (dt.Rows.Count > 0). */
        function setHistory(rows) { if (rows && rows.length) g.set(rows); }
        function gridfill() {
            return C.getJson(CATX + '/history' + hostQs(host)).then(setHistory).catch(function (e) { alert(e.message); });
        }
        /* AttributeGridFill(CategoryId):234 — no rows → ClearStructure. */
        function setAttributes(rows) {
            if (rows && rows.length) {
                rows.forEach(function (r) { r.sel = intOf(r.StatusValue) === 1; });
                ga.set(rows);
            } else ga.clear();
        }
        function attributeGridFill(catId) {
            return C.getJson(CATX + '/attributes' + hostQs(host, { categoryId: catId })).then(setAttributes)
                .catch(function (e) { alert(e.message); });
        }
        /* The Load / Refresh binding sequence (ItemParentCategoryFill, GetItemClassGroup, AssetAccountBind,
           ExpenseAccountFill, DepreciatonMethodNameBind). */
        function bindLists(l) {
            if (l.parentCategories && l.parentCategories.length) bindFirst(cmbParent, l.parentCategories, 'Id', 'InvParentCateDescription');
            if (l.classGroups && l.classGroups.length) bindFirst(cmbCls, l.classGroups, 'Id', 'ClassGroupName');
            bindRetain(cmbRev, l.assetAccounts, 'Id', 'AccountTitle');
            bindRetain(cmbInv, l.assetAccounts, 'Id', 'AccountTitle');
            bindRetain(cmbExp, l.expenseAccounts, 'Id', 'AccountTitle');
            bindRetain(cmbCgs, l.expenseAccounts, 'Id', 'AccountTitle');
            bindRetain(cmbMethod, l.depreciationMethods, 'depreciationMethodScheduleId', 'DepreciatonMethodName');
        }
        function load() {                                                       // InvDeffrmItemCatagory_Load:166
            return C.getJson(CATX + '/lookups' + hostQs(host)).then(function (l) {
                bLingo.classList.toggle('sdl-hidden', !l.multiLanguage);        // btnMultiLingo.Visible = feature 7
                attributesFeature = !!l.attributesFeature;
                attrPanel.classList.toggle('sdl-hidden', !attributesFeature);   // PanelAttributes.Visible
                if (attributesFeature) { setAttributes(l.attributes); ga.relayout(); }
                bindLists(l);
                setHistory(l.history);
                f('code').focus();
            }).catch(function (e) { alert(e.message); });
        }
        function refresh() {                                                    // btnRefresh_Click:856
            return C.getJson(CATX + '/lookups' + hostQs(host)).then(function (l) {
                if (attributesFeature) { attrPanel.classList.remove('sdl-hidden'); setAttributes(l.attributes); }
                bindLists(l);
            }).catch(function (e) { alert(e.message); });
        }
        function reset() {                                                      // Reset:797 (Is Active kept)
            recId = 0;
            attrPanel.classList.toggle('sdl-hidden', !attributesFeature);
            ['code', 'desc', 'sfrom', 'sto', 'rate', 'months'].forEach(function (k) { f(k).value = ''; });
            [cmbCgs, cmbInv, cmbRev, cmbExp, cmbCls, cmbMethod, cmbParent].forEach(function (c) { c.value = '0'; });
            gridfill();
            cmbParent.disabled = false;
            if (attributesFeature) attributeGridFill(0);
            bSave.classList.remove('sdl-hidden'); bUpdate.classList.add('sdl-hidden');
        }
        function validate() {                                                   // formvalidation:456 (FormTypeId 3 wording)
            function fail(m, el) { alert(m); el.focus(); return false; }
            if (f('code').value.trim() === '' || f('code').value === '0') return fail('Please Insert Code', f('code'));
            if (f('desc').value.trim() === '' || f('desc').value.trim() === '0') return fail('Please Insert Description', f('desc'));
            if (f('sfrom').value.trim() === '' || f('sfrom').value.trim() === '0') return fail('Please Insert Serial From', f('sfrom'));
            if (f('sto').value.trim() === '' || f('sto').value.trim() === '0') return fail('Please Insert Serial To', f('sto'));
            if (intOf(cmbRev.value) === 0) return fail('Please Select Accumulated Account', cmbRev);
            if (intOf(cmbInv.value) === 0) return fail('Please Select Assets Account', cmbInv);
            if (intOf(cmbCgs.value) === 0) return fail('Please Select Depreciation Account', cmbCgs);
            if (intOf(cmbParent.value) === 0) return fail('Please Select Parent Category', cmbParent);
            return true;
        }
        function insert() {                                                     // Insert:512
            if (!validate()) return;
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var checked = attributesFeature ? ga.rows().filter(function (r) { return r.sel; }).map(function (r) { return r.attributeId; }) : [];
            C.postJson(CATX + '/save', {
                Id: recId,
                CategoryCode: f('code').value,
                CategoryDescription: f('desc').value,
                SerialFrom: f('sfrom').value,
                SerialTo: f('sto').value,
                RevenueAccountId: intOf(cmbRev.value),
                InventoryAccountId: intOf(cmbInv.value),
                CGSAccountId: intOf(cmbCgs.value),
                ExpenseMaintenanceAccountId: intOf(cmbExp.value),
                ParentCategoryId: intOf(cmbParent.value),
                ClassGroupId: intOf(cmbCls.value),
                DepreciationMethodScheduleId: intOf(cmbMethod.value),
                UseFullLifeMonths: f('months').value,
                DepreciationRate: f('rate').value,
                CategoryStatus: f('status').checked,
                CheckedAttributeIds: checked,
                host: host
            }).then(function (r) {
                alert(r.message);
                reset();
            }).catch(function (e) { alert(e.message); });
        }
        function save() { recId = 0; insert(); }                               // btnsave_Click:609
        function update() { insert(); }                                         // btnUpdate_Click:622
        function readById(id) {                                                 // ReadById:634
            C.getJson(CATX + '/' + id + hostQs(host)).then(function (c) {
                recId = id;
                bSave.classList.add('sdl-hidden'); bUpdate.classList.remove('sdl-hidden');
                f('code').value = c.CategoryCode;
                f('desc').value = c.CategoryDescription;
                f('sfrom').value = c.SerialFrom;
                f('sto').value = c.SerialTo;
                setVal(cmbRev, c.accumulatedDepreciationAcId);                  // :650 then :655 (FormTypeId 3 overrides)
                setVal(cmbInv, c.CapitalWipAcId);
                setVal(cmbCgs, c.DepreciationExpenseAcId);
                if (intOf(c.ExpenseMaintenanceAccountId) > 0) setVal(cmbExp, c.ExpenseMaintenanceAccountId);
                if (intOf(c.depreciationMethodScheduleId) > 0) setVal(cmbMethod, c.depreciationMethodScheduleId);
                f('months').value = c.usefullLifeInMonths;
                f('rate').value = c.depriciationrate;
                setVal(cmbParent, c.InventoryParentCategoriesId);
                setVal(cmbCls, c.ItemClassGroupId);
                f('status').checked = !!c.CategoryStatus;
                gridfill();
                cmbParent.disabled = false;
                if (attributesFeature) { attrPanel.classList.remove('sdl-hidden'); attributeGridFill(recId); }
            }).catch(function (e) { alert(e.message); });
        }
        q(d, '[data-a="new"]').onclick = reset;                                  // btnnew_Click:832
        q(d, '[data-a="refresh"]').onclick = refresh;
        bSave.onclick = save;
        bUpdate.onclick = update;
        /* InvDeffrmItemCatagory_KeyDown:917 — Ctrl+N, Ctrl+S (Save visible), Ctrl+U (Update visible), Ctrl+E / Esc. */
        d.keys = { newRec: reset, save: save, update: update, updateMood: function () { return bSave.classList.contains('sdl-hidden'); } };

        [cmbRev, cmbInv, cmbCgs, cmbParent, cmbCls, cmbExp, cmbMethod].forEach(function (c) { C.fillSelect(c, [], 'Id', 'x'); });
        load();
        return d;
    }

    /* ================================================================== frmDefineAssets */

    function openFixedAssetItem(onClose, opts) {
        var host = (opts && opts.host) || '';
        var d = shell(1044, 561, 'EccountBook ERP');
        d.onClose = onClose;
        var recId = 0, tabIdx = 0, rights = { save: false, update: false, viewAll: false };
        var NO_ATT = 'Attachments (DMS) are not available on this dialog on the web';
        var NO_TYPE = 'The Item Type form (InvDeffrmItemType, FormTypeId 3) is not available on the web';
        var NO_UOM = 'The Item UOM form (InvDeffrmItemItemUom) is not available on the web';
        var NO_BAR = 'Barcode Print (BarcodeGenerate) is not available on the web';
        var NO_KEYS = 'The ShortCut Keys popup is not available on the web';
        var NO_PIC = 'Asset pictures are stored by the desktop as Windows file paths under the configured "Attachment Folder Path" ' +
            'and copied there from the client PC (File.Copy); the web server cannot reach that folder';
        function star(left, top) { return '<span class="sdl-star" style="left:' + left + 'px;top:' + top + 'px;">*</span>'; }
        d.client.innerHTML =
            '<div style="position:relative;width:1044px;height:561px;background:#f0f0f0;">' +
            /* ---------------- tpItemDefinitionAddItem "Add Asset" (4,4) 1036 x 532, Padding 3 */
            '<div class="sdl-tabpage" data-page="0" style="padding:3px;box-sizing:border-box;">' +
            /* panel6 (3,3) 1030 x 32 — toolStrip1 27 */
            '<div style="height:32px;flex:0 0 32px;"><div class="sdl-strip grey">' +
            tool('new', 59, 'fa-plus-circle', '#2e7d32', '<u>N</u>ew') +
            tool('refresh', 80, 'fa-refresh', '#2e7d32', 'Refresh') +
            tool('save', 64, 'fa-floppy-o', '#1565c0', '<u>S</u>ave') +
            tool('update', 78, 'fa-pencil', '#1565c0', '<u>U</u>pdate', { hidden: true }) +
            tool('att', 109, 'fa-paperclip', '#6d4c41', '<u>A</u>ttachment', { disabled: NO_ATT }) +
            tool('cat', 126, 'fa-sitemap', '#1565c0', 'Item Category') +
            tool('type', 98, 'fa-tags', '#1565c0', 'Item Type', { disabled: NO_TYPE }) +
            tool('uom', 94, 'fa-balance-scale', '#1565c0', 'Item Uom', { disabled: NO_UOM }) +
            tool('barcode', 119, 'fa-barcode', '#000', 'Barcode Print', { disabled: NO_BAR }) +
            tool('keys', 126, 'fa-keyboard-o', '#000', 'ShortCut Keys', { disabled: NO_KEYS }) +
            '</div></div>' +
            /* panel2 (3,35) 1030 x 27 teal — label13 (3,3) Tahoma 12 bold */
            '<div class="sdl-teal" style="height:27px;flex:0 0 27px;"><span class="sdl-teal-t" style="left:3px;top:3px;font-size:16px;line-height:19px;">Add Fixed Assest Item</span></div>' +
            /* panel1 / panel7 (3,62) 1030 x 467 */
            '<div class="sdl-rel" style="height:467px;">' +
            /* groupBox2 "Main Information" Dock Left (0,0) 365 x 467, Tahoma 9.75 bold */
            '<div class="sdl-gb" style="left:0;top:0;width:365px;height:467px;"><span class="sdl-gcap">Main Information</span>' +
            lbl('Item Category', 3, 28) + star(106, 27) + cmb('cat', 126, 22, 232) +
            lbl('Item Type', 3, 55) + star(106, 54) + cmb('type', 126, 49, 232) +
            lbl('Asset Code', 3, 80) + star(106, 79) + txt('codeNew', 126, 76, 232, ' readonly tabindex="-1"') +
            lbl('Asset Name', 3, 104) + star(106, 103) + txt('name', 126, 100, 232) +
            lbl('Base UOM', 3, 131) + star(106, 130) + cmb('uom', 126, 125, 232) +
            lbl('Barcode No', 3, 157) + txt('barcode', 126, 153, 231) +
            lbl('Assets Class', 3, 184) + star(106, 183) + cmb('cls', 126, 178, 232) +
            lbl('Assets GL A/c', 3, 211) + star(106, 210) + cmb('assetGl', 126, 205, 232) +
            lbl('Accumulated Depreciation A/c', 3, 230, 'width:104px;height:31px;white-space:normal;line-height:14px;overflow:hidden;') + star(106, 237) + cmb('accum', 126, 232, 232) +
            lbl('Depreciation Expense A/c', 3, 258, 'width:104px;height:31px;white-space:normal;line-height:14px;overflow:hidden;') + star(106, 265) + cmb('depr', 126, 260, 232) +
            lbl('Expense Maintenance<br>Account', 3, 285, 'width:122px;height:30px;line-height:15px;overflow:hidden;') + cmb('expm', 126, 287, 232) +
            lbl('LeadTime Day', 3, 319) + txt('lead', 126, 315, 160) +
            '<label class="sdl-abs sdl-chk sdl-lbl" style="left:295px;top:317px;height:19px;"><input type="checkbox" data-f="status" checked> Status</label>' +
            '<label class="sdl-abs sdl-chk" style="left:127px;top:342px;height:18px;font:bold 12px Tahoma,sans-serif;"><input type="radio" data-f="local" checked> Local</label>' +
            '<label class="sdl-abs sdl-chk" style="left:189px;top:342px;height:18px;font:bold 12px Tahoma,sans-serif;"><input type="radio" data-f="import"> Import</label>' +
            '</div>' +
            /* BrandImages "Asset Images" Dock Top (365,0) 665 x 296, Tahoma 9.75 bold */
            '<div class="sdl-gb" style="left:365px;top:0;width:665px;height:296px;"><span class="sdl-gcap">Asset Images</span>' +
            '<div class="sdl-abs sdl-pic" data-pic="1" style="left:6px;top:18px;width:279px;height:241px;"></div>' +
            '<button type="button" class="sdl-abs sdl-browse" style="left:6px;top:261px;width:279px;height:31px;" disabled title="' + esc(NO_PIC) + '">Browse</button>' +
            '<div class="sdl-abs sdl-pic" data-pic="2" style="left:293px;top:18px;width:279px;height:241px;"></div>' +
            '<button type="button" class="sdl-abs sdl-browse" style="left:293px;top:261px;width:279px;height:31px;" disabled title="' + esc(NO_PIC) + '">Browse</button>' +
            '</div>' +
            /* groupBox1 "Asset Allocation To Company" Dock Left (365,296) 432 x 171, Verdana 9.75 bold; grdAllocation (3,19) 426 x 149 */
            '<div class="sdl-gb" style="left:365px;top:296px;width:432px;height:171px;"><span class="sdl-gcap">Asset Allocation To Company</span>' +
            '<div class="sdl-abs" data-galloc style="left:3px;top:19px;width:426px;height:149px;display:flex;flex-direction:column;"></div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            /* ---------------- tpItemDefinitionAddItemHistory "History" */
            '<div class="sdl-tabpage sdl-hidden" data-page="1">' +
            /* panel3 teal 53 — toolStrip2 (0,0) 27: BtnLoadAllT "New"; label31 "Asset History" (3,30); ctrlGrdBar1 (903,28) */
            '<div class="sdl-teal" style="height:53px;flex:0 0 53px;">' +
            '<div class="sdl-strip grey" style="position:absolute;left:0;top:0;right:0;">' + tool('loadall', 59, 'fa-plus-circle', '#2e7d32', '<u>N</u>ew') + '</div>' +
            '<span class="sdl-teal-t" style="left:3px;top:30px;font-size:16px;line-height:19px;">Asset History</span>' +
            '<button type="button" class="sdl-abs sdl-gbar" style="right:3px;top:28px;" disabled title="Grid layout / print / export options are not ported"><i class="fa fa-cog"></i></button>' +
            '</div>' +
            '<div data-gh style="flex:1 1 auto;min-height:0;"></div>' +
            '</div>' +
            /* tabCAddItem.Alignment = Bottom */
            '<div class="sdl-tabbar"><button type="button" class="sdl-tabbtn on" data-tab="0">Add Asset</button><button type="button" class="sdl-tabbtn" data-tab="1">History</button></div>' +
            '</div>';

        var f = function (k) { return q(d, '[data-f="' + k + '"]'); };
        var cmbCat = f('cat'), cmbType = f('type'), cmbUom = f('uom'), cmbCls = f('cls'), cmbAssetGl = f('assetGl'),
            cmbAccum = f('accum'), cmbDepr = f('depr'), cmbExpm = f('expm');
        var bSave = q(d, '[data-a="save"]'), bUpdate = q(d, '[data-a="update"]');
        var rdLocal = f('local'), rdImport = f('import');
        var rn = 'sdlLocImp' + Date.now(); rdLocal.name = rn; rdImport.name = rn;
        digitsOnly(f('lead'));                                                   // txtLeadTimeDay_KeyPress → OnlytextNumberFunction

        /* CompaniesBindInGrid:443 — Id hidden, Location 250, Value checkbox (header selector). No filter row. */
        var galloc = grid(q(d, '[data-galloc]'), [
            { key: 'Location', caption: 'Location', w: 250 },
            { key: 'Value', caption: 'Value', w: 60, check: true }
        ], null, { noFilter: true });
        /* grdfrmfill:925 + gridsetting:980 — Id / AssetCode hidden, AssetCodeNew captioned "Asset Code",
           HeaderLines 3; widths Constants.InventoryConstants (values not in the recovered source — estimated). */
        var gh = grid(q(d, '[data-gh]'), [
            { key: 'AssetCodeNew', caption: 'Asset Code', w: 90 },
            { key: 'AssetName', caption: 'AssetName', w: 220 },
            { key: 'ItemType', caption: 'ItemType', w: 130 },
            { key: 'ItemCategory', caption: 'ItemCategory', w: 130 },
            { key: 'ParentCategory', caption: 'ParentCategory', w: 130 },
            { key: 'ClassGroupName', caption: 'ClassGroupName', w: 130 },
            { key: 'ItemStatus', caption: 'ItemStatus', w: 70 },
            { key: 'Assets_GL', caption: 'Assets_GL', w: 200 },
            { key: 'Accumulated_Depreciation', caption: 'Accumulated_Depreciation', w: 200 },
            { key: 'Depreciation_Expense', caption: 'Depreciation_Expense', w: 200 },
            { key: 'EntryDate', caption: 'EntryDate', w: 95, fmt: C.gridDate },
            { key: 'EntryUserName', caption: 'EntryUserName', w: 130 },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', w: 90, right: true },
            { key: 'LeadTimeDay', caption: 'LeadTimeDay', w: 60, right: true }
        ], function (row) { var id = intOf(row.Id); refresh().then(function () { readById(id); }); });   // grdhistory_DoubleClick:901
        gh.clear();

        function saveVisibleEnabled() { return !bSave.classList.contains('sdl-hidden') && !bSave.disabled; }
        function updateVisibleEnabled() { return !bUpdate.classList.contains('sdl-hidden') && !bUpdate.disabled; }
        function pics(p1, p2) {
            [[1, p1], [2, p2]].forEach(function (x) {
                var box = q(d, '[data-pic="' + x[0] + '"]');
                box.textContent = '';
                box.title = x[1] ? 'Stored picture path (not reachable from the web): ' + x[1] : '';
            });
        }
        function tab(i) {                                                       // tabCAddItem_SelectedIndexChanged:1017
            tabIdx = i;
            d.client.querySelectorAll('[data-page]').forEach(function (p) { p.classList.toggle('sdl-hidden', p.getAttribute('data-page') !== String(i)); });
            d.client.querySelectorAll('.sdl-tabbtn').forEach(function (b) { b.classList.toggle('on', b.getAttribute('data-tab') === String(i)); });
            if (i === 1) { gh.relayout(); history(50); }
        }
        function history(n) {                                                   // grdfrmfill(NoOfRecords):925
            return C.getJson(FAI + '/history' + hostQs(host, { noOfRecords: n })).then(function (rows) {
                if (rows && rows.length) gh.set(rows); else gh.clear();
            }).catch(function (e) { alert(e.message); });
        }
        function bindCategories(list, retain) {
            if (retain) bindRetain(cmbCat, list, 'Id', 'CategoryDescription'); else bindNone(cmbCat, list, 'Id', 'CategoryDescription');
        }
        function bindAll(l, first) {
            var bind = first ? bindNone : bindRetain;
            bindCategories(l.categories, !first);                               // ItemCatagoryHistoryFill:308
            bind(cmbType, l.itemTypes, 'Id', 'TypeDescription');                // ItemTypeFill:373
            bindRetain(cmbAccum, l.assetAccounts, 'Id', 'AccountTitle');        // AssetAccountBind:270
            bindRetain(cmbAssetGl, l.assetAccounts, 'Id', 'AccountTitle');
            bindRetain(cmbExpm, l.expenseAccounts, 'Id', 'AccountTitle');       // ExpenseAccountFill:289
            bindRetain(cmbDepr, l.expenseAccounts, 'Id', 'AccountTitle');
            bind(cmbCls, l.classes, 'ClassId', 'ClassDescription');             // ItemClassFill:488
        }
        function load() {                                                       // InvDefrmAddItem_Load:243
            return C.getJson(FAI + '/lookups' + hostQs(host)).then(function (l) {
                rights = l.rights || rights;
                bSave.disabled = !rights.save;                                  // :251
                bUpdate.disabled = !rights.update;                              // :252
                if (!rights.save) bSave.title = 'You do not have the Save right for Fixed Asset Items';
                if (!rights.update) bUpdate.title = 'You do not have the Update right for Fixed Asset Items';
                bindAll(l, true);
                if (l.companies && l.companies.length) galloc.set(l.companies); else galloc.clear();   // CompaniesBindInGrid:443
                bindNone(cmbUom, l.uoms, 'Id', 'UomCode');                      // BaseUnitFill:341
            }).catch(function (e) { alert(e.message); });
        }
        function refreshLists() {                                               // BtnRefresh_Click:871
            return C.getJson(FAI + '/lookups' + hostQs(host)).then(function (l) { bindAll(l, false); })
                .catch(function (e) { alert(e.message); });
        }
        function categoryLeave() {                                              // cmbItemCategory_Leave:402
            if (!saveVisibleEnabled()) return Promise.resolve();
            return C.getJson(FAI + '/category-leave' + hostQs(host, { categoryId: intOf(cmbCat.value) })).then(function (r) {
                if (r.ItemCodeNew !== undefined) f('codeNew').value = r.ItemCodeNew;
                if (r.CapitalWipAcId !== undefined) {
                    setVal(cmbAccum, r.accumulatedDepreciationAcId);
                    setVal(cmbDepr, r.DepreciationExpenseAcId);
                    setVal(cmbAssetGl, r.CapitalWipAcId);
                    setVal(cmbExpm, r.ExpenseMaintenanceAccountId);
                }
            }).catch(function () { alert('Record Not Found'); });
        }
        function refresh() {                                                    // refresh():825
            cmbCat.disabled = false;                                            // cmbItemCategory.ReadOnly = false
            recId = 0;
            bSave.classList.remove('sdl-hidden'); bUpdate.classList.add('sdl-hidden');
            /* txtItemCode (hidden) is cleared; the visible Asset Code (txtItemCodeNew) is refilled by the Leave below */
            f('barcode').value = ''; f('name').value = '';
            cmbCat.focus();
            pics('', '');
            return categoryLeave();
        }
        function newRec() {                                                     // btnnew_Click:857
            var p = refresh();
            cmbUom.value = '0'; cmbType.value = '0';
            return p;
        }
        function validate() {                                                   // FormValidationForAddItem:521
            function fail(m, el) { alert(m); el.focus(); return false; }
            if (f('name').value.trim() === '') return fail('Item Name Field is Required', f('name'));
            if (intOf(cmbUom.value) === 0) return fail('Base Unit Field is Required', cmbUom);
            if (intOf(cmbCat.value) === 0) return fail('Item Category Field is Required', cmbCat);
            if (intOf(cmbType.value) === 0) return fail('Item Type Field is Required', cmbType);
            if (intOf(cmbCls.value) === 0) return fail('Item Class Field is Required', cmbCls);
            if (intOf(cmbAssetGl.value) === 0) return fail('Asset GL A/c Field is Required', cmbAssetGl);
            if (intOf(cmbDepr.value) === 0) return fail('Depreciation_Expense A/c Field is Required', cmbDepr);
            if (intOf(cmbAccum.value) === 0) return fail('Accumulated_Depreciation A/c Filed is Required', cmbAccum);
            if (intOf(cmbExpm.value) === 0) return fail('Expense Maintenance A/c Filed is Required', cmbExpm);
            return true;
        }
        function insert() {                                                     // Insert():580
            if (!validate()) return;
            if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            C.postJson(FAI + '/save', {
                Id: recId,
                ItemCategoryId: intOf(cmbCat.value),
                ItemTypeId: intOf(cmbType.value),
                ItemName: f('name').value,
                BaseUnitId: intOf(cmbUom.value),
                BarcodeNo: f('barcode').value,
                ItemClassId: intOf(cmbCls.value),
                CapitalWipAcId: intOf(cmbAssetGl.value),
                accumulatedDepreciationAcId: intOf(cmbAccum.value),
                DepreciationExpenseAcId: intOf(cmbDepr.value),
                ExpenseMaintenanceAccountId: intOf(cmbExpm.value),
                LeadTimeDay: f('lead').value,
                ItemStatus: f('status').checked,
                Local: rdLocal.checked,
                Allocations: galloc.rows().map(function (r) { return { Id: intOf(r.Id), Value: !!r.Value }; }),
                host: host
            }).then(function (r) {
                alert(r.message);
                refresh();
            }).catch(function (e) { alert(e.message); });
        }
        function save() { recId = 0; insert(); }                               // BtnSave_Click:711
        function update() {                                                     // btnupdate_Click:724
            if (recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
            insert();
        }
        function readById(id) {                                                 // ReadById:740
            return C.getJson(FAI + '/' + id + hostQs(host)).then(function (it) {
                recId = id;
                bSave.classList.add('sdl-hidden'); bUpdate.classList.remove('sdl-hidden');
                tab(0);
                f('codeNew').value = it.ItemCodeNew;
                f('barcode').value = it.BarcodeNo;
                f('name').value = it.ItemName;
                setVal(cmbUom, it.BaseUnitId);
                setVal(cmbCat, it.ItemCategoryId);
                cmbCat.disabled = true;                                         // cmbItemCategory.ReadOnly = true
                setVal(cmbType, it.ItemTypeId);
                f('status').checked = !!it.ItemStatus;
                setVal(cmbAssetGl, it.CapitalWipAcId);
                setVal(cmbDepr, it.DepreciationExpenseAcId);
                setVal(cmbAccum, it.accumulatedDepreciationAcId);
                if (intOf(it.ExpenseMaintenanceAccountId) > 0) setVal(cmbExpm, it.ExpenseMaintenanceAccountId);
                setVal(cmbCls, it.ItemClassId);
                f('lead').value = it.LeadTimeDay;
                pics(it.Pic1, it.Pic2);
            }).catch(function (e) { alert(e.message); });
        }

        cmbCat.addEventListener('change', categoryLeave);                       // Leave (web: on change — see header)
        q(d, '[data-a="new"]').onclick = newRec;
        q(d, '[data-a="refresh"]').onclick = refreshLists;
        bSave.onclick = save;
        bUpdate.onclick = update;
        q(d, '[data-a="loadall"]').onclick = function () { history(0); };     // btnLoadAll_Click:1025
        /* btnitmcat_Click:1139 — the same form and mode as the asset form's "+" (frmItemCatagoryStore, FormTypeId 3).
           Web deviation: Item Category is re-read on close (selection kept). */
        q(d, '[data-a="cat"]').onclick = function () {
            openAssetCategory(function () {
                if (d.closed) return;
                C.getJson(FAI + '/categories' + hostQs(host)).then(function (list) { bindCategories(list, true); })
                    .catch(function (e) { alert(e.message); });
            }, { host: host });
        };
        d.client.querySelectorAll('.sdl-tabbtn').forEach(function (b) {
            b.onclick = function () { var i = parseInt(b.getAttribute('data-tab'), 10); if (i !== tabIdx) tab(i); };
        });
        /* InvDefrmAddItem_KeyDown:1248 (Enter → Tab and Ctrl+Alt shortcut popup not ported). */
        d.onKey = function (e) {
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { d.close(); return true; }
            if (e.ctrlKey && k === 't') {
                if (tabIdx === 1) { tab(0); f('name').focus(); } else { tab(1); gh.focus(); }
                return true;
            }
            if (tabIdx === 0) {
                if (e.ctrlKey && k === 'n') { newRec(); return true; }
                if (e.ctrlKey && k === 's') { if (saveVisibleEnabled()) save(); return true; }
                if (e.ctrlKey && k === 'r') { refreshLists(); return true; }
                if (e.ctrlKey && k === 'u') { if (updateVisibleEnabled()) update(); return true; }
                if (e.ctrlKey && e.key === 'F5') { f('name').focus(); return true; }
                if (e.ctrlKey && e.key === 'ArrowDown') { galloc.focus(); return true; }
                if (e.ctrlKey && e.key === 'ArrowUp') { f('name').focus(); return true; }
            } else {
                if (e.ctrlKey && (k === 'n' || k === 'l')) { history(0); return true; }
                if (e.ctrlKey && e.key === 'ArrowDown') { gh.focus(); return true; }
            }
            return false;
        };

        [cmbCat, cmbType, cmbUom, cmbCls, cmbAssetGl, cmbAccum, cmbDepr, cmbExpm].forEach(function (c) { C.fillSelect(c, [], 'Id', 'x'); });
        load();
        return d;
    }

    window.StoreDefine = {
        openDepartment: openDepartment, openAsset: openAsset,
        openAssetCategory: openAssetCategory, openFixedAssetItem: openFixedAssetItem
    };
})();
