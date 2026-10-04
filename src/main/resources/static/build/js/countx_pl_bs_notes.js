/* ============================================================================================
 * Admin Panel -> "PL & BS Notes" = Architecture.WinApp.Account_Definition.BsPlSettingForm.
 * Every handler the designer wires, in the desktop's terms:
 *   BsPlSettingForm_Load      tableNote structure bound to grdNoteChange, radPL focused, Assets/Liabilities hidden
 *   DefineCity_KeyDown        (KeyPreview) Enter->Tab, Ctrl+N/S/U/E/Esc/T/F5/Up/Down, Ctrl+Alt
 *   radPL_CheckedChanged      BS -> Assets/Liabilities visible, Assets checked; PL -> both hidden + unchecked
 *   btnShow_Click / GridLoad  Sp_PLBSSetting_GetAllMethod 'GetAllForBsPlSetting' (+ GridSettings value list)
 *   btnnew_Click              radPL.Focus() (a programmatic Focus checks a RadioButton - RadioButton.OnEnter) + GridLoad
 *   btnUpdate_Click           confirm, Sp_PLBSSetting_Insert per changed row (one transaction), GridLoad
 *   btnShowNote_Click         SP_AccountNotes_ReadAllMethodBySPType 'ReadAllNotes'
 *   btnNewNote_Click          radPLNote.Focus() + GridNoteTitleFill
 *   btnUpdateNote_Click       confirm, 'UpdateTitleById' per changed row, GridNoteTitleFill
 *   BtnShortCutkeys / btnSkey MakeShortCutKeys -> ShortCutKeyPopUp
 *   grdBsPl_CellUpdated, tabControl1_SelectedIndexChanged - empty bodies on the desktop
 *   ctrlGrdBar1/2_Load        per-user saved grid layout (GetGridLayout) - layout files are not ported
 * ============================================================================================ */
(function () {
    'use strict';

    var form = document.getElementById('form');
    if (!form) return;                                   /* not Admin: the page shows the gate */
    var TOKEN = form.getAttribute('data-token') || '';
    var BASE = location.pathname.replace(/\/admin\/pl-bs-notes.*$/, '');
    var API = BASE + '/api/admin/pl-bs-notes';

    function $(id) { return document.getElementById(id); }
    function esc(v) {
        return String(v === undefined || v === null ? '' : v)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    /* ------------------------------------------------------------------ MessageBox.Show */
    var mbOpen = false;
    function msg(text, caption) {
        return new Promise(function (resolve) {
            showMb(text, caption || '', [{ t: 'OK', v: true }], resolve);
        });
    }
    function confirmYesNo(text, caption) {
        return new Promise(function (resolve) {
            showMb(text, caption, [{ t: 'Yes', v: true }, { t: 'No', v: false }], resolve);
        });
    }
    function showMb(text, caption, buttons, resolve) {
        var prev = document.activeElement;
        $('mbCap').textContent = caption;
        $('mbBody').textContent = text;
        var box = $('mbBtns');
        box.innerHTML = '';
        buttons.forEach(function (b) {
            var el = document.createElement('button');
            el.type = 'button';
            el.textContent = b.t;
            el.addEventListener('click', function () { close(b.v); });
            box.appendChild(el);
        });
        function close(v) {
            $('mbMask').style.display = 'none';
            mbOpen = false;
            document.removeEventListener('keydown', onKey, true);
            try { if (prev && prev.focus) prev.focus(); } catch (e) { /* ignore */ }
            resolve(v);
        }
        function onKey(e) {
            if (e.key === 'Escape') { e.preventDefault(); e.stopPropagation(); close(buttons.length > 1 ? false : true); }
            else if (buttons.length > 1 && (e.key === 'y' || e.key === 'Y')) { e.preventDefault(); e.stopPropagation(); close(true); }
            else if (buttons.length > 1 && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); e.stopPropagation(); close(false); }
            else if (e.key !== 'Tab' && e.key !== 'Enter' && e.key !== ' ') { e.stopPropagation(); }
        }
        mbOpen = true;
        $('mbMask').style.display = 'flex';
        document.addEventListener('keydown', onKey, true);
        box.firstChild.focus();
    }

    /* ------------------------------------------------------------------ busy buttons */
    var busy = false;
    function withBusy(btn, fn) {
        if (busy) return Promise.resolve();               /* duplicate request refused */
        busy = true;
        var all = document.querySelectorAll('#btnnew,#btnUpdate,#btnShow,#btnNewNote,#btnUpdateNote,#btnShowNote');
        for (var i = 0; i < all.length; i++) all[i].disabled = true;
        if (btn) btn.classList.add('busy');
        var done = function () {
            busy = false;
            for (var j = 0; j < all.length; j++) all[j].disabled = false;
            if (btn) btn.classList.remove('busy');
        };
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(done, function (e) { done(); return msg(e && e.message ? e.message : String(e)); });
    }

    function getJson(url) {
        return fetch(url, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } }).then(readJson);
    }
    function postJson(url, body) {
        body.token = TOKEN;
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
            body: JSON.stringify(body)
        }).then(readJson);
    }
    function readJson(r) {
        return r.text().then(function (t) {
            var j = null;
            try { j = t ? JSON.parse(t) : {}; } catch (e) { j = null; }
            if (!r.ok || !j || j.success === false) {
                throw new Error((j && j.message) || ('Request failed (' + r.status + ').'));
            }
            return j;
        });
    }

    /* ------------------------------------------------------------------ grid (Janus GridEX look) */
    function Grid(tableId, onRender) {
        this.table = $(tableId);
        this.cols = null;            /* null = no structure retrieved yet (grdBsPl before the first GridLoad) */
        this.rows = [];
        this.cur = -1;
        this.filters = {};
        this.onRender = onRender;
        this.nav = document.querySelector('.nav[data-grid="' + tableId + '"]');
        var self = this;
        this.nav.addEventListener('click', function (e) {
            var b = e.target.closest('button');
            if (!b) return;
            var vis = self.visibleIndexes();
            if (!vis.length) return;
            var pos = vis.indexOf(self.cur);
            var to = { first: 0, prev: Math.max(0, pos - 1), next: Math.min(vis.length - 1, pos + 1), last: vis.length - 1 }[b.getAttribute('data-nav')];
            self.select(vis[to], true);
        });
        this.table.addEventListener('mousedown', function (e) {
            var tr = e.target.closest('tbody tr');
            if (tr && tr.hasAttribute('data-i')) self.select(+tr.getAttribute('data-i'), false);
        });
        this.table.addEventListener('focusin', function (e) {
            var tr = e.target.closest('tbody tr');
            if (tr && tr.hasAttribute('data-i')) self.select(+tr.getAttribute('data-i'), false);
        });
    }
    Grid.prototype.text = function (row, c) {
        if (c.display) return c.display(row);
        var v = row[c.key];
        return v === null || v === undefined ? '' : String(v);
    };
    Grid.prototype.matches = function (row) {
        for (var k in this.filters) {
            var f = this.filters[k];
            if (!f) continue;
            var c = this.cols.filter(function (x) { return x.key === k; })[0];
            if (c && this.text(row, c).toLowerCase().indexOf(f.toLowerCase()) < 0) return false;   /* Contains */
        }
        return true;
    };
    Grid.prototype.visibleIndexes = function () {
        var out = [];
        for (var i = 0; i < this.rows.length; i++) if (this.matches(this.rows[i])) out.push(i);
        return out;
    };
    Grid.prototype.render = function () {
        var self = this, t = this.table;
        var thead = t.tHead, tbody = t.tBodies[0], tfoot = t.tFoot;
        if (!this.cols) { thead.innerHTML = ''; tbody.innerHTML = ''; tfoot.innerHTML = ''; this.updateNav(); return; }
        var vc = this.cols.filter(function (c) { return !c.hidden; });
        var h = '<tr>';
        vc.forEach(function (c) { h += '<th' + (c.width ? ' style="min-width:' + c.width + 'px;width:' + c.width + 'px"' : '') + '>' + esc(c.caption) + '</th>'; });
        h += '<th class="fill"></th></tr><tr class="filter">';
        vc.forEach(function (c) { h += '<td><input type="text" aria-label="Filter ' + esc(c.caption) + '" data-f="' + esc(c.key) + '" value="' + esc(self.filters[c.key] || '') + '"></td>'; });
        h += '<td class="fill"></td></tr>';
        thead.innerHTML = h;
        var b = '';
        this.visibleIndexes().forEach(function (i) {
            var row = self.rows[i];
            b += '<tr data-i="' + i + '"' + (i === self.cur ? ' class="sel"' : '') + '>';
            vc.forEach(function (c) { b += c.cell ? c.cell(row, i) : '<td>' + esc(self.text(row, c)) + '</td>'; });
            b += '<td class="fill"></td></tr>';
        });
        tbody.innerHTML = b;
        /* TotalRow = True, TotalRowPosition = BottomFixed - no column has an aggregate, so the row is empty */
        var f = '<tr>';
        vc.forEach(function () { f += '<td></td>'; });
        f += '<td class="fill"></td></tr>';
        tfoot.innerHTML = f;
        var inputs = thead.querySelectorAll('input[data-f]');
        for (var k = 0; k < inputs.length; k++) {
            inputs[k].addEventListener('input', function (e) {
                var key = e.target.getAttribute('data-f');
                self.filters[key] = e.target.value;
                var caret = e.target.selectionStart;
                self.render();
                var again = thead.querySelector('input[data-f="' + key + '"]');
                if (again) { again.focus(); try { again.setSelectionRange(caret, caret); } catch (x) { /* ignore */ } }
            });
        }
        if (this.onRender) this.onRender(tbody);
        this.updateNav();
    };
    Grid.prototype.select = function (i, scroll) {
        this.cur = i;
        var trs = this.table.tBodies[0].rows;
        for (var k = 0; k < trs.length; k++) {
            var on = +trs[k].getAttribute('data-i') === i;
            trs[k].classList.toggle('sel', on);
            if (on && scroll) trs[k].scrollIntoView({ block: 'nearest' });
        }
        this.updateNav();
    };
    Grid.prototype.updateNav = function () {
        var vis = this.cols ? this.visibleIndexes() : [];
        var pos = vis.indexOf(this.cur);
        this.nav.querySelector('.pos').textContent = 'Record ' + (pos < 0 ? 0 : pos + 1) + ' of ' + vis.length;
    };
    Grid.prototype.focus = function () {
        var el = this.table.querySelector('tbody .dtcombo-input, tbody input.cell, tbody select');
        if (el) { el.focus(); return; }
        var sc = this.table.parentNode;
        sc.setAttribute('tabindex', '-1');
        sc.focus();
    };

    /* ================================================================== tab 1 */
    var radPL = $('radPL'), radBS = $('radBS'), radAssets = $('radAssets'), radLiab = $('radLiablities');
    var valueList = [];
    var noteCaption = 'NoteTitle';
    var loaded1 = null;          /* {note, classId, count} - what dtchartofaccount holds */

    var grdBsPl = new Grid('grdBsPl', function (tbody) {
        var sels = tbody.querySelectorAll('select[data-row]');
        for (var i = 0; i < sels.length; i++) {
            sels[i].addEventListener('change', function (e) {
                var r = grdBsPl.rows[+e.target.getAttribute('data-row')];
                r.cur = e.target.value;                   /* the value list Id (LimitToList) */
                /* grdBsPl_CellUpdated: `_ = grdBsPl.CurrentRow;` - nothing else */
            });
        }
        if (window.DesktopCombo) window.DesktopCombo.refresh();
    });

    function noteText(row) {
        /* A picked value shows its value-list text; the loaded value is the note TITLE (the proc returns
           text, the value list is keyed by Id), which Janus draws as-is. */
        if (row.cur !== row.NoteTitle) {
            for (var i = 0; i < valueList.length; i++) if (String(valueList[i].Id) === String(row.cur)) return valueList[i].NoteTitle || '';
        }
        return row.NoteTitle === null || row.NoteTitle === undefined ? '' : String(row.NoteTitle);
    }

    function bsPlColumns() {
        return [
            { key: 'PlBsId', caption: 'PlBsId', hidden: true },
            { key: 'ChartofAccountId', caption: 'ChartofAccountId', hidden: true },
            { key: 'AccountTitle', caption: 'Account title' },
            { key: 'NoteTitle', caption: noteCaption.replace(/([a-zA-Z])(Notes|Title)/g, '$1 $2'), width: 280, display: noteText,
              cell: function (row, i) {
                  var picked = row.cur !== row.NoteTitle;
                  var h = '<td class="combo"><select aria-label="Note for ' + esc(row.AccountTitle) + '" data-dtcombo data-row="' + i + '" style="width:280px;height:34px">';
                  h += '<option value="" hidden' + (picked ? '' : ' selected') + '>' + esc(row.NoteTitle) + '</option>';
                  valueList.forEach(function (n) {
                      h += '<option value="' + esc(n.Id) + '"' + (picked && String(n.Id) === String(row.cur) ? ' selected' : '') + '>' + esc(n.NoteTitle) + '</option>';
                  });
                  return h + '</select></td>';
              } },
            { key: 'NoteRole', caption: 'Note role' },
            { key: 'AccountClassId', caption: 'AccountClassId', hidden: true },
            { key: 'ClassName', caption: 'Class name' }
        ];
    }

    /* radPL_CheckedChanged (fires for both radios: radBS checking clears radPL) */
    function radPLCheckedChanged() {
        if (radBS.checked) {
            $('lblAssets').style.display = ''; $('lblLiab').style.display = '';
            radAssets.checked = true;
        } else {
            $('lblAssets').style.display = 'none'; $('lblLiab').style.display = 'none';
            radAssets.checked = false; radLiab.checked = false;
        }
    }
    radPL.addEventListener('change', radPLCheckedChanged);
    radBS.addEventListener('change', radPLCheckedChanged);
    function assetsVisible() { return $('lblAssets').style.display !== 'none' && $('lblLiab').style.display !== 'none'; }

    /* Control.Focus() on a RadioButton raises Enter, and RadioButton.OnEnter checks it when the focus
       did not come from a mouse click or Tab - so btnnew / Ctrl+F5 / Ctrl+Up re-check PL. */
    function focusRadio(r) {
        if (document.activeElement !== r && !r.checked) {
            r.checked = true;
            r.dispatchEvent(new Event('change', { bubbles: true }));
        }
        r.focus();
    }

    function gridLoad() {
        if (radBS.checked && !radLiab.checked && !radAssets.checked) {
            return msg("Please select either the 'Assets' or 'Liabilities' button...").then(function () { radAssets.focus(); });
        }
        var classId = 0;
        if (assetsVisible()) classId = radAssets.checked ? 2 : 3;
        var note = radBS.checked ? 'BS' : 'PL';
        return getJson(API + '/accounts?note=' + note + '&classId=' + classId).then(function (j) {
            var rows = j.rows || [];
            if (rows.length > 0) {
                rows.forEach(function (r) { r.cur = r.NoteTitle; });
                valueList = j.valueList || [];
                noteCaption = j.noteCaption || 'NoteTitle';
                grdBsPl.rows = rows;
                grdBsPl.cols = bsPlColumns();
                grdBsPl.cur = rows.length ? 0 : -1;
                grdBsPl.render();
                loaded1 = { note: note, classId: classId, count: rows.length };
            } else {
                /* dtchartofaccount is replaced by the empty result but `table` and the grid are left as
                   they were - so the next Update loops over nothing. */
                loaded1 = { note: note, classId: classId, count: 0 };
            }
        });
    }

    function btnShowClick() { return withBusy($('btnShow'), gridLoad); }
    function btnnewClick() { return withBusy($('btnnew'), function () { focusRadio(radPL); return gridLoad(); }); }

    function btnUpdateClick() {
        if (busy) return Promise.resolve();
        return confirmYesNo('Are you sure to Update?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return withBusy($('btnUpdate'), function () {
                var rows = [];
                if (loaded1 && loaded1.count > 0) {
                    grdBsPl.rows.forEach(function (r) {
                        rows.push({ ChartofAccountId: r.ChartofAccountId, NoteTitle: r.cur });
                    });
                }
                var mode = radBS.checked ? 'BS' : (radPL.checked ? 'PL' : '');
                return postJson(API + '/accounts/update', {
                    mode: mode,
                    loadNote: loaded1 ? loaded1.note : (radBS.checked ? 'BS' : 'PL'),
                    loadClassId: loaded1 ? loaded1.classId : 0,
                    rows: rows
                }).then(function (j) {
                    return (j.message ? msg(j.message) : Promise.resolve()).then(gridLoad);
                });
            });
        });
    }

    $('btnShow').addEventListener('click', btnShowClick);
    $('btnnew').addEventListener('click', btnnewClick);
    $('btnUpdate').addEventListener('click', btnUpdateClick);

    /* ================================================================== tab 2 */
    var radAllNote = $('radAllNote'), radPLNote = $('radPLNote'), radBSNote = $('radBSNote');
    var loaded2 = null;          /* {note, count} - what dtNoteTile holds */

    var grdNoteChange = new Grid('grdNoteChange', function (tbody) {
        var ins = tbody.querySelectorAll('input.cell[data-row]');
        for (var i = 0; i < ins.length; i++) {
            ins[i].addEventListener('input', function (e) {
                grdNoteChange.rows[+e.target.getAttribute('data-row')].NoteTitle = e.target.value;
            });
        }
    });
    /* tableNote: Id, NoteTitle, NoteRole, AccountClassId, ClassName - GridNoteSettings hides Id and
       AccountClassId and makes all but NoteTitle read-only. */
    grdNoteChange.cols = [
        { key: 'Id', caption: 'Id', hidden: true },
        { key: 'NoteTitle', caption: 'Note title',
          cell: function (row, i) { return '<td class="ed" style="min-width:' + noteW + 'px"><input type="text" class="cell" data-row="' + i + '" value="' + esc(row.NoteTitle) + '"></td>'; } },
        { key: 'NoteRole', caption: 'Note role' },
        { key: 'AccountClassId', caption: 'AccountClassId', hidden: true },
        { key: 'ClassName', caption: 'Class name' }
    ];

    /* GridAutoAdjustmentNew: Columns[i].AutoSize() - the editable NoteTitle column is sized to its longest text */
    var noteW = 80;
    var measure = document.createElement('canvas').getContext('2d');
    function autoSizeNoteTitle() {
        measure.font = window.getComputedStyle(grdNoteChange.table).font;
        var w = measure.measureText('Note title').width;
        grdNoteChange.rows.forEach(function (r) { w = Math.max(w, measure.measureText(String(r.NoteTitle == null ? '' : r.NoteTitle)).width); });
        noteW = Math.max(240, Math.ceil(w) + 24);
    }

    function gridNoteTitleFill() {
        var note = radBSNote.checked ? 'BS' : (radPLNote.checked ? 'PL' : '');
        return getJson(API + '/notes?note=' + encodeURIComponent(note)).then(function (j) {
            var rows = j.rows || [];
            if (rows.length > 0) {
                grdNoteChange.rows = rows;
                autoSizeNoteTitle();
                grdNoteChange.cur = 0;
                grdNoteChange.render();
                loaded2 = { note: note, count: rows.length };
            } else {
                loaded2 = { note: note, count: 0 };
            }
        });
    }

    function btnShowNoteClick() { return withBusy($('btnShowNote'), gridNoteTitleFill); }
    function btnNewNoteClick() { return withBusy($('btnNewNote'), function () { focusRadio(radPLNote); return gridNoteTitleFill(); }); }
    function btnUpdateNoteClick() {
        if (busy) return Promise.resolve();
        return confirmYesNo('Are you sure to Update?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return withBusy($('btnUpdateNote'), function () {
                var rows = [];
                if (loaded2 && loaded2.count > 0) {
                    grdNoteChange.rows.forEach(function (r) { rows.push({ Id: r.Id, NoteTitle: r.NoteTitle }); });
                }
                return postJson(API + '/notes/update', { loadNote: loaded2 ? loaded2.note : '', rows: rows }).then(function (j) {
                    return (j.message ? msg(j.message) : Promise.resolve()).then(gridNoteTitleFill);
                });
            });
        });
    }
    $('btnShowNote').addEventListener('click', btnShowNoteClick);
    $('btnNewNote').addEventListener('click', btnNewNoteClick);
    $('btnUpdateNote').addEventListener('click', btnUpdateNoteClick);

    /* ================================================================== tabs, grid bars, shortcuts */
    var tabIndex = 0;
    function selectTab(i) {
        tabIndex = i;
        var tabs = document.querySelectorAll('#tabstrip .tab');
        for (var k = 0; k < tabs.length; k++) {
            tabs[k].classList.toggle('on', k === i);
            tabs[k].setAttribute('aria-pressed', String(k === i));
        }
        $('tabPage1').classList.toggle('on', i === 0);
        $('tabPage2').classList.toggle('on', i === 1);
        /* tabControl1_SelectedIndexChanged: empty on the desktop */
    }
    Array.prototype.forEach.call(document.querySelectorAll('#tabstrip .tab'), function (t) {
        t.addEventListener('click', function () { selectTab(+t.getAttribute('data-tab')); });
    });

    /* ctrlGrdBar1 / ctrlGrdBar2: countx_grid_bar.js (data-gridbar-* on the grid hosts) - the desktop
       CtrlGrdBar menu (Field Chooser, Save Layout, Print, Export, ...). BsPlSettingForm calls no rights
       code, so every item stays enabled, as on the desktop. */

    /* MakeShortCutKeys -> ShortCutKeyPopUp */
    function makeShortCutKeys() {
        var rows = [['Ctrl+S', 'For Show Grid Data'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', tabIndex === 0 ? 'For Focus on Radio Button PL' : 'For Focus on Radio Button All'],
            ['Ctrl+ArrowUp', tabIndex === 0 ? 'For Focus on Radio Button PL' : 'For Focus on Radio Button All'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid']];
        $('skBody').innerHTML = rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td class="fill">' + esc(r[1]) + '</td></tr>'; }).join('');
        $('skMask').style.display = 'flex';
    }
    function closeShortcuts() { $('skMask').style.display = 'none'; }
    $('BtnShortCutkeys').addEventListener('click', makeShortCutKeys);
    $('btnSkey').addEventListener('click', makeShortCutKeys);
    $('skClose').addEventListener('click', closeShortcuts);
    $('skMask').addEventListener('mousedown', function (e) { if (e.target === $('skMask')) closeShortcuts(); });

    function closeForm() {
        if (history.length > 1) history.back();
        else location.href = BASE + '/';
    }

    function comboOpen() { return !!document.querySelector('.dtcombo-pop[style*="block"]'); }

    /* DefineCity_KeyDown (KeyPreview) */
    document.addEventListener('keydown', function (e) {
        if (mbOpen) return;
        if ($('skMask').style.display === 'flex') {
            if (e.key === 'Escape') { e.preventDefault(); closeShortcuts(); }
            return;
        }
        var k = e.key;
        var t = e.target;
        if (k === 'Enter' && !e.ctrlKey && !e.altKey && !comboOpen() && t && t.tagName !== 'BUTTON' && !(t.closest && t.closest('.dtcombo-pop'))) {
            if (t.closest && t.closest('tr.filter')) return;
            e.preventDefault();
            moveFocus(t, e.shiftKey ? -1 : 1);                          /* SendKeys.Send("{TAB}") */
            return;
        }
        if (e.ctrlKey && e.altKey && (k === 'Control' || k === 'Alt')) { e.preventDefault(); makeShortCutKeys(); return; }
        if (e.ctrlKey && !e.altKey) {
            var lk = k.length === 1 ? k.toLowerCase() : k;
            if (lk === 'n') { e.preventDefault(); if (tabIndex === 0) btnnewClick(); else btnNewNoteClick(); return; }
            if (lk === 's') { e.preventDefault(); if (tabIndex === 0) btnShowClick(); else btnShowNoteClick(); return; }
            if (lk === 'u') { e.preventDefault(); if (tabIndex === 0) btnUpdateClick(); else btnUpdateNoteClick(); return; }
            if (lk === 'e') { e.preventDefault(); closeForm(); return; }
            if (lk === 't') { e.preventDefault(); selectTab(tabIndex === 0 ? 1 : 0); return; }
            if (k === 'F5' || k === 'ArrowUp') { e.preventDefault(); focusRadio(tabIndex === 0 ? radPL : radAllNote); return; }
            if (k === 'ArrowDown') { e.preventDefault(); (tabIndex === 0 ? grdBsPl : grdNoteChange).focus(); return; }
        }
        if (k === 'Escape' && !comboOpen() && !document.querySelector('.gb-menu:not([style*="none"]), .gb-chooser:not([style*="none"])')) { e.preventDefault(); closeForm(); }
    });

    function moveFocus(from, dir) {
        var page = tabIndex === 0 ? $('tabPage1') : $('tabPage2');
        var list = Array.prototype.filter.call(page.querySelectorAll('input:not([type=hidden]), select, button, .dtcombo-input, [tabindex]'), function (el) {
            if (el.disabled || el.tabIndex < 0) return false;
            if (el.tagName === 'SELECT' && el.__dtcombo) return false;
            if (el.type === 'radio' && !el.checked) return false;
            return el.offsetParent !== null;
        });
        var i = list.indexOf(from);
        var n = list[(i + dir + list.length) % list.length];
        if (n) n.focus();
    }

    /* BsPlSettingForm_Load */
    grdNoteChange.render();                 /* grdNoteChange.DataSource = tableNote; RetrieveStructure */
    grdBsPl.render();                       /* no structure until the first GridLoad */
    $('lblAssets').style.display = 'none'; $('lblLiab').style.display = 'none';
    radPL.focus();
}());
