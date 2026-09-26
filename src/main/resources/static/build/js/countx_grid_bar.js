/* ============================================================================================
 * countx_grid_bar.js - the desktop ctrlGrdBar (grid.User_Conrtols.CtrlGrdBar) for any web grid.
 *
 * Desktop source: grid.User_Conrtols.CtrlGrdBar (IL: legacy-app-analysis/il_by_namespace/grid.User_Conrtols.txt)
 *   ToolStrip1 (150x27, no grip) > btnGridControl (ToolStripSplitButton, image only, 36x24, "Grid Control")
 *   drop-down items, in this order (each 187x26):
 *     txtGridTitle                       read-only ToolStripTextBox 100x23, Segoe UI 9 - the print page header
 *     mGridChooseFielder                 "Field Chooser"      GridEX.ShowFieldChooser / HideFieldChooser (toggle)
 *     mGridSaveLayouts                   "Save Layout"        SaveLayoutFile + BLL.GridLayout.Save -> "Layout Saved successfully"
 *     mGridPrint                         "Print"              PageSetup (Landscape) + PrintPreview, PageHeaderCenter = txtGridTitle
 *     mGridExport                        "Export"             SaveFileDialog "Excel Files|*.xls" -> "Exported successfully"
 *     GroupCollaps                       "Group Collapse"     CollapseGroups + CollapseRecords
 *     GroupExpand                        "Group Expand"       ExpandGroups + ExpandRecords
 *     GridResizeColumnToolStripMenuItem  "Auto Adjust Column" toggle: AutoSize every column / restore the widths it saved
 *     removeLayoutToolStripMenuItem      "Remove Layout"      delete file + BLL.GridLayout.DeleteByLayout -> "Layout Removed successfully"
 *                                                             (no local layout file -> throw "File Not Exist" -> MessageBox)
 *   Every handler is wrapped in try/catch -> MessageBox.Show(ex.Message).
 *
 * Layout key (CtrlGrdBar + CommonServices.GetGridLayout/DeleteGrdLayout):  FormName + "_" + GridName + "_" + UserName
 *
 * DELIBERATE DEVIATION - storage. The desktop keeps a Janus *binary* layout (GridEX.SaveLayoutFile) in
 * C:\SCS\Layout\<key> and table GridLayout (BLL Architecture.BLL.GridLayout.Save / DeleteByLayout / GetByID).
 * The desktop feeds those bytes straight to GridEX.LoadLayoutFile, so a web layout written to that table
 * would break the desktop grid. Web layouts are therefore kept per viewer in the browser (localStorage)
 * under "countx.gridlayout." + the SAME key shape. Nothing is written to the database.
 *
 * Rights (CommonServices.SetRightsValueInRightsObject + FormHelper.SetCtrlGrdBarRight): FieldChooser,
 * SaveLayout, GroupCollapse, GroupExpand start TRUE for everybody; "Grid Print" / "Grid Export" come only
 * from tblUserRights rows (the Admin role does not force them). Only forms that call the rights code gate
 * the bar - on other forms every item stays enabled. Pages pass what their desktop form does.
 *
 * Usage
 *   <table data-gridbar="FormName:GridName">                 explicit (GridName defaults to the table id)
 *   <div data-gridbar-form="FormName" [data-gridbar-grid="GridName"]>  host whose inner <table> is (re)rendered
 *   optional attributes on the table / host:
 *     data-gridbar-mount="#selector"      put the gear inside this element (a desktop panel strip) instead of
 *                                         floating it over the grid's top-right corner
 *     data-gridbar-title="text"           txtGridTitle (print page header)
 *     data-gridbar-disable="chooser,save,print,export,collapse,expand,autosize,remove"   Enabled = false
 *     data-gridbar-hide="chooser,save,remove,..."                                           Visible = false
 *     data-gridbar-reorder="false"        never move cells (grids whose script reads row.cells[i])
 *     data-gridbar-nullgrid="true"        desktop bar whose MyGrid is null: items show the NullReferenceException text
 *
 *   GridBar.attach(tableOrHost, "Form:Grid" | {form, grid}, opts)
 *     opts: canChooseFields, canSaveLayout, canPrint, canExport, canGroupCollapse, canGroupExpand,
 *           canAutoAdjust, canRemoveLayout (Enabled; default true)
 *           showChooseFields, showSaveLayout, showRemoveLayout, showPrint, showExport, showGroupCollapse,
 *           showGroupExpand, showAutoAdjust (Visible; default true)
 *           title, mount (element or selector), reorder (bool), fileName, nullGrid (bool)
 *   GridBar.refresh(tableOrHost)      re-apply the layout now (normally automatic via MutationObserver)
 *   GridBar.setRights(tableOrHost, opts)   same keys as attach opts, after the page's rights call returns
 *   GridBar.scan(root)                attach every marked table / host under root (runs on DOMContentLoaded)
 *   GridBar.userName()                Promise of the session user name (GET /api/production/grid-bar/user)
 *
 * Columns are keyed by th[data-col], else by the visible header text. Hidden columns, column order and
 * widths are re-applied after every re-render of the grid. Columns the page itself hides (display:none)
 * and columns without header text (row buttons, check boxes) are not offered in the Field Chooser.
 * Cells are only re-ordered when the grid has no editors (input/select/textarea/button) in its body and
 * no colspan anywhere, so page scripts that read cells by index keep working on entry grids.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var doc = global.document;
    var STORE_PREFIX = 'countx.gridlayout.';
    var USER_URL = '/api/production/grid-bar/user';

    /* Exact desktop texts. */
    var T = {
        gridControl: 'Grid Control',
        chooser: 'Field Chooser',
        save: 'Save Layout',
        print: 'Print',
        exp: 'Export',
        collapse: 'Group Collapse',
        expand: 'Group Expand',
        autosize: 'Auto Adjust Column',
        remove: 'Remove Layout',
        saved: 'Layout Saved successfully',
        removed: 'Layout Removed successfully',
        notExist: 'File Not Exist',
        exported: 'Exported successfully',
        nullRef: 'Object reference not set to an instance of an object.'
    };

    /* item id -> [caption, enabled-opt, visible-opt, attribute token] */
    var ITEMS = [
        ['chooser',  T.chooser,  'canChooseFields',  'showChooseFields'],
        ['save',     T.save,     'canSaveLayout',    'showSaveLayout'],
        ['print',    T.print,    'canPrint',         'showPrint'],
        ['export',   T.exp,      'canExport',        'showExport'],
        ['collapse', T.collapse, 'canGroupCollapse', 'showGroupCollapse'],
        ['expand',   T.expand,   'canGroupExpand',   'showGroupExpand'],
        ['autosize', T.autosize, 'canAutoAdjust',    'showAutoAdjust'],
        ['remove',   T.remove,   'canRemoveLayout',  'showRemoveLayout']
    ];

    var bars = [];          /* every attached bar */
    var openMenu = null;    /* the one drop-down open at a time */

    /* ------------------------------------------------------------------ small helpers */
    function msg(m) { global.alert(m); }                       /* MessageBox.Show(...) */
    function toArr(x) { return Array.prototype.slice.call(x || []); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }
    function resolveEl(x) {
        if (!x) return null;
        if (typeof x === 'string') { try { return doc.querySelector(x); } catch (e) { return null; } }
        return x;
    }
    function tokens(s) {
        var o = {};
        String(s || '').split(/[\s,;]+/).forEach(function (t) { if (t) o[t.toLowerCase()] = true; });
        return o;
    }

    /* localStorage - wrapped: it can throw (private window, blocked site data) or come back empty. */
    function storeGet(k) {
        try { var v = global.localStorage.getItem(STORE_PREFIX + k); return v ? JSON.parse(v) : null; }
        catch (e) { return null; }
    }
    function storeHas(k) {
        try { return global.localStorage.getItem(STORE_PREFIX + k) !== null; } catch (e) { return false; }
    }
    function storeSet(k, v) { global.localStorage.setItem(STORE_PREFIX + k, JSON.stringify(v)); }   /* throws to caller */
    function storeDel(k) { global.localStorage.removeItem(STORE_PREFIX + k); }                        /* throws to caller */

    /* ------------------------------------------------------------------ session user name */
    var userPromise = null;
    function userName() {
        if (userPromise) return userPromise;
        if (typeof global.GridBarUserName === 'string' && global.GridBarUserName) {
            userPromise = Promise.resolve(global.GridBarUserName);
            return userPromise;
        }
        var meta = doc.querySelector('meta[name="gridbar-user"]');
        if (meta && meta.getAttribute('content')) {
            userPromise = Promise.resolve(meta.getAttribute('content'));
            return userPromise;
        }
        userPromise = global.fetch(USER_URL, { credentials: 'same-origin', headers: { 'Accept': 'application/json' } })
            .then(function (r) {
                return r.json().then(function (j) {
                    if (!r.ok) throw new Error((j && (j.message || j.error)) || ('HTTP ' + r.status));
                    return (j && j.userName) || '';
                });
            });
        userPromise.catch(function () { userPromise = null; });  /* allow a retry on the next action */
        return userPromise;
    }

    /* ------------------------------------------------------------------ column model */
    function headText(th) {
        var c = th.cloneNode(true);
        toArr(c.querySelectorAll('.gb-rsz, .gb-bar, input, select, button')).forEach(function (x) { x.remove(); });
        return (c.textContent || '').replace(/\s+/g, ' ').trim();
    }
    function allRows(table) {
        var rows = [];
        if (table.tHead) rows = rows.concat(toArr(table.tHead.rows));
        toArr(table.tBodies).forEach(function (b) { rows = rows.concat(toArr(b.rows)); });
        if (table.tFoot) rows = rows.concat(toArr(table.tFoot.rows));
        if (!table.tHead && !table.tBodies.length) rows = toArr(table.rows);
        return rows;
    }
    function spanOf(cell) {
        var s = cell.getAttribute('data-gb-span');
        if (s === null) { s = String(cell.colSpan || 1); cell.setAttribute('data-gb-span', s); }
        return parseInt(s, 10) || 1;
    }
    function rowWidth(tr) {
        var w = 0;
        toArr(tr.cells).forEach(function (c) { w += spanOf(c); });
        return w;
    }
    /** The key row: the last header row whose cells are all single-span and not the ReportKit filter row. */
    function keyRow(table) {
        var head = table.tHead ? toArr(table.tHead.rows) : [];
        if (!head.length) {
            var first = table.rows[0];
            return first && first.querySelector('th') ? first : null;
        }
        var best = null;
        head.forEach(function (tr) {
            if (tr.classList.contains('rk-filter')) return;
            var ok = toArr(tr.cells).every(function (c) { return spanOf(c) === 1; });
            if (ok) best = tr;
        });
        return best || head[0];
    }
    /** Natural (as-rendered) column list: [{key, text, fixed}] in natural order. */
    function readColumns(table) {
        var tr = keyRow(table);
        if (!tr) return [];
        ensureNatural(tr);
        var cells = naturalCells(tr), seen = {};
        return cells.map(function (th, i) {
            var text = headText(th);
            var key = th.getAttribute('data-col') || text || ('#' + i);
            if (seen[key]) key = key + '#' + (++seen[key]); else seen[key] = 1;
            var pageHidden = !th.classList.contains('gb-hid') && global.getComputedStyle(th).display === 'none';
            return { key: key, text: text || '', fixed: !text || pageHidden, pageHidden: pageHidden };
        });
    }
    /** Tag each cell of a freshly rendered row with its natural column index. */
    function ensureNatural(tr) {
        var cells = toArr(tr.cells);
        var tagged = cells.length && cells.every(function (c) { return c.hasAttribute('data-gb-n'); });
        if (tagged) {
            /* a script replaced/added cells inside an already tagged row: re-tag from scratch only if broken */
            var ns = cells.map(function (c) { return +c.getAttribute('data-gb-n'); });
            var uniq = ns.filter(function (n, i) { return ns.indexOf(n) === i; });
            if (uniq.length === ns.length) return;
        }
        var pos = 0;
        cells.forEach(function (c) { c.setAttribute('data-gb-n', pos); pos += spanOf(c); });
    }
    function naturalCells(tr) {
        return toArr(tr.cells).sort(function (a, b) { return (+a.getAttribute('data-gb-n')) - (+b.getAttribute('data-gb-n')); });
    }

    /* ------------------------------------------------------------------ the bar */
    function Bar(scope, key, opts) {
        this.scope = scope;                   /* the table, or the host whose inner table is re-rendered */
        this.form = key.form;
        this.grid = key.grid;
        this.opts = {};
        this.state = { hidden: {}, order: null, widths: {} };
        this.loaded = false;
        this.autoAdjust = false;              /* CtrlGrdBar._AutoAdjust */
        this.autoWidths = null;               /* CtrlGrdBar.AryColWidth */
        this.chooser = null;
        this.applying = false;
        this.setRights(opts || {});
        this.build();
        this.observe();
        this.loadSaved();
    }

    Bar.prototype.table = function () {
        if (this.scope.tagName === 'TABLE') return this.scope;
        return this.scope.querySelector('table');
    };
    Bar.prototype.layoutKey = function (user) { return this.form + '_' + this.grid + '_' + user; };

    Bar.prototype.setRights = function (o) {
        var s = this.scope, dis = tokens(s.getAttribute('data-gridbar-disable')), hid = tokens(s.getAttribute('data-gridbar-hide'));
        var self = this;
        ITEMS.forEach(function (it) {
            var en = it[2], vi = it[3];
            if (o[en] !== undefined) self.opts[en] = !!o[en];
            else if (self.opts[en] === undefined) self.opts[en] = !dis[it[0]];
            if (o[vi] !== undefined) self.opts[vi] = !!o[vi];
            else if (self.opts[vi] === undefined) self.opts[vi] = !hid[it[0]];
        });
        ['title', 'mount', 'reorder', 'fileName', 'nullGrid'].forEach(function (k) { if (o[k] !== undefined) self.opts[k] = o[k]; });
        if (this.menu) this.paintMenu();
    };

    Bar.prototype.title = function () {
        if (this.opts.title !== undefined) return String(this.opts.title || '');
        return this.scope.getAttribute('data-gridbar-title') || '';   /* txtGridTitle is empty unless a form sets it */
    };

    Bar.prototype.canReorder = function (table) {
        var o = this.opts.reorder;
        if (o === false || this.scope.getAttribute('data-gridbar-reorder') === 'false') return false;
        if (o !== true) {
            var body = table.tBodies[0];
            if (body && body.querySelector('input, select, textarea, button')) return false;
        }
        return !allRows(table).some(function (tr) { return toArr(tr.cells).some(function (c) { return spanOf(c) !== 1; }); });
    };

    /* --- gear + drop-down */
    Bar.prototype.build = function () {
        var self = this;
        var btn = doc.createElement('button');
        btn.type = 'button';
        btn.className = 'gb-gear';
        btn.title = T.gridControl;
        btn.setAttribute('aria-label', T.gridControl);
        btn.setAttribute('aria-haspopup', 'menu');
        btn.innerHTML = '<span class="gb-gear-ico" aria-hidden="true">&#9881;</span><span class="gb-gear-arr" aria-hidden="true">&#9662;</span>';
        btn.addEventListener('click', function (e) { e.preventDefault(); e.stopPropagation(); self.toggleMenu(); });
        this.gear = btn;

        var menu = doc.createElement('div');
        menu.className = 'gb-menu';
        menu.setAttribute('role', 'menu');
        menu.style.display = 'none';
        var html = '<div class="gb-title-row"><input type="text" class="gb-title" readonly tabindex="-1" aria-label="Grid title"></div>';
        ITEMS.forEach(function (it) {
            html += '<button type="button" role="menuitem" class="gb-item gb-i-' + it[0] + '" data-a="' + it[0] + '">'
                + '<span class="gb-ico" aria-hidden="true"></span>' + esc(it[1]) + '</button>';
        });
        menu.innerHTML = html;
        menu.addEventListener('click', function (e) {
            var b = e.target.closest('.gb-item');
            if (!b || b.disabled) return;
            self.closeMenu();
            self.run(b.getAttribute('data-a'));
        });
        menu.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') { self.closeMenu(); btn.focus(); return; }
            if (e.key !== 'ArrowDown' && e.key !== 'ArrowUp') return;
            e.preventDefault();
            var items = toArr(menu.querySelectorAll('.gb-item')).filter(function (x) { return !x.disabled && x.style.display !== 'none'; });
            var i = items.indexOf(doc.activeElement);
            i = e.key === 'ArrowDown' ? (i + 1) % items.length : (i <= 0 ? items.length - 1 : i - 1);
            if (items[i]) items[i].focus();
        });
        doc.body.appendChild(menu);
        this.menu = menu;
        this.paintMenu();
        this.placeGear();
    };

    Bar.prototype.paintMenu = function () {
        var self = this;
        ITEMS.forEach(function (it) {
            var b = self.menu.querySelector('.gb-i-' + it[0]);
            b.disabled = !self.opts[it[2]];
            b.style.display = self.opts[it[3]] ? '' : 'none';
        });
        this.menu.querySelector('.gb-title').value = this.title();
    };

    /** Mount the gear: a desktop panel strip (data-gridbar-mount / opts.mount), else float it over the grid. */
    Bar.prototype.placeGear = function () {
        var mount = resolveEl(this.opts.mount) || resolveEl(this.scope.getAttribute('data-gridbar-mount'));
        if (mount) {
            if (this.gear.parentNode !== mount) mount.appendChild(this.gear);
            this.gear.classList.add('gb-gear-inline');
            /* a page script that rewrites the strip (innerHTML) would drop the gear: put it back */
            if (this.mountEl !== mount && global.MutationObserver) {
                var self = this;
                if (this.mmo) this.mmo.disconnect();
                this.mountEl = mount;
                this.mmo = new MutationObserver(function () {
                    if (!self.gear.isConnected || self.gear.parentNode !== self.mountEl) self.placeGear();
                });
                this.mmo.observe(mount, { childList: true });
            }
            return;
        }
        var anchor = this.scope;
        var p = anchor.parentNode;
        /* a table alone in its scroll box: float above the box so the gear does not scroll away */
        if (anchor.tagName === 'TABLE' && p && p !== doc.body) {
            var cs = global.getComputedStyle(p);
            var only = toArr(p.children).filter(function (c) { return c !== anchor && !c.classList.contains('gb-bar'); }).length === 0;
            if (only && /(auto|scroll)/.test(cs.overflow + cs.overflowX + cs.overflowY)) anchor = p;
        }
        if (!this.barEl) {
            this.barEl = doc.createElement('div');
            this.barEl.className = 'gb-bar';
            this.barEl.appendChild(this.gear);
        }
        if (this.barEl.nextSibling !== anchor || !this.barEl.parentNode) {
            if (anchor.parentNode) anchor.parentNode.insertBefore(this.barEl, anchor);
        }
    };

    Bar.prototype.toggleMenu = function () {
        if (openMenu === this) { this.closeMenu(); return; }
        if (openMenu) openMenu.closeMenu();
        this.paintMenu();
        var r = this.gear.getBoundingClientRect();
        var m = this.menu;
        m.style.display = 'block';
        var w = m.offsetWidth, h = m.offsetHeight;
        var left = Math.min(r.left, global.innerWidth - w - 4);
        var top = r.bottom + 1;
        if (top + h > global.innerHeight - 4) top = Math.max(4, r.top - h - 1);
        m.style.left = Math.max(4, left) + 'px';
        m.style.top = top + 'px';
        openMenu = this;
        var first = toArr(m.querySelectorAll('.gb-item')).filter(function (x) { return !x.disabled && x.style.display !== 'none'; })[0];
        if (first) first.focus();
    };
    Bar.prototype.closeMenu = function () {
        this.menu.style.display = 'none';
        if (openMenu === this) openMenu = null;
    };

    Bar.prototype.run = function (a) {
        /* Desktop quirk (FoodProductionWithValues.ctrlGrdBar17: MyGrid = null in the designer): every item
         * dereferences MyGrid and shows the NullReferenceException text; the group items test _MyGrid first
         * and do nothing. Pages opt in with data-gridbar-nullgrid="true" / opts.nullGrid. */
        if (this.opts.nullGrid || this.scope.getAttribute('data-gridbar-nullgrid') === 'true') {
            if (a !== 'collapse' && a !== 'expand') msg(T.nullRef);
            return;
        }
        try {
            if (a === 'chooser') this.fieldChooser();
            else if (a === 'save') this.saveLayout();
            else if (a === 'print') this.print();
            else if (a === 'export') this.exportGrid();
            else if (a === 'collapse') this.group(false);
            else if (a === 'expand') this.group(true);
            else if (a === 'autosize') this.autoAdjustColumns();
            else if (a === 'remove') this.removeLayout();
        } catch (ex) {
            msg(ex && ex.message ? ex.message : String(ex));       /* catch -> MessageBox.Show(ex.Message) */
        }
    };

    /* --- layout persistence */
    Bar.prototype.loadSaved = function () {
        var self = this;
        userName().then(function (u) {
            var saved = storeGet(self.layoutKey(u));
            if (saved && typeof saved === 'object') {
                self.state.hidden = {};
                (saved.hidden || []).forEach(function (k) { self.state.hidden[k] = true; });
                self.state.order = Array.isArray(saved.order) ? saved.order : null;
                self.state.widths = saved.widths && typeof saved.widths === 'object' ? saved.widths : {};
            }
            self.loaded = true;
            self.apply();
        }).catch(function () { self.loaded = true; self.apply(); });
    };

    /** mGridSaveLayouts_Click */
    Bar.prototype.saveLayout = function () {
        var self = this;
        userName().then(function (u) {
            try {
                self.captureWidths();
                var hidden = Object.keys(self.state.hidden).filter(function (k) { return self.state.hidden[k]; });
                storeSet(self.layoutKey(u), {
                    v: 1, form: self.form, grid: self.grid, user: u,
                    hidden: hidden, order: self.state.order, widths: self.state.widths,
                    savedAt: new Date().toISOString()
                });
                msg(T.saved);
            } catch (ex) { msg(ex && ex.message ? ex.message : String(ex)); }
        }).catch(function (ex) { msg(ex && ex.message ? ex.message : String(ex)); });
    };

    /** removeLayoutToolStripMenuItem_Click - the grid keeps its current look until it is opened again. */
    Bar.prototype.removeLayout = function () {
        var self = this;
        userName().then(function (u) {
            try {
                var k = self.layoutKey(u);
                if (!storeHas(k)) throw new Error(T.notExist);
                storeDel(k);
                msg(T.removed);
            } catch (ex) { msg(ex && ex.message ? ex.message : String(ex)); }
        }).catch(function (ex) { msg(ex && ex.message ? ex.message : String(ex)); });
    };

    /* --- field chooser (mGridChooseFielder_Click: ShowFieldChooser if hidden, else HideFieldChooser) */
    Bar.prototype.fieldChooser = function () {
        if (this.chooser) { this.closeChooser(); return; }
        var self = this;
        var box = doc.createElement('div');
        box.className = 'gb-chooser';
        box.setAttribute('role', 'dialog');
        box.setAttribute('aria-label', T.chooser);
        box.innerHTML = '<div class="gb-ch-head"><span>' + esc(T.chooser) + '</span>'
            + '<button type="button" class="gb-ch-x" title="Close" aria-label="Close">&#215;</button></div>'
            + '<div class="gb-ch-list"></div>'
            + '<div class="gb-ch-foot"><button type="button" data-m="-1" title="Move up">&#9650;</button>'
            + '<button type="button" data-m="1" title="Move down">&#9660;</button></div>';
        doc.body.appendChild(box);
        this.chooser = box;
        this.chooserSel = null;
        this.paintChooser();
        var r = this.gear.getBoundingClientRect();
        box.style.left = Math.max(4, Math.min(r.right - box.offsetWidth, global.innerWidth - box.offsetWidth - 4)) + 'px';
        box.style.top = Math.max(4, Math.min(r.bottom + 2, global.innerHeight - box.offsetHeight - 4)) + 'px';

        box.querySelector('.gb-ch-x').addEventListener('click', function () { self.closeChooser(); });
        box.addEventListener('keydown', function (e) { if (e.key === 'Escape') self.closeChooser(); });
        box.querySelector('.gb-ch-list').addEventListener('change', function (e) {
            var cb = e.target.closest('input[type=checkbox]');
            if (!cb) return;
            var k = cb.getAttribute('data-k');
            if (cb.checked) delete self.state.hidden[k]; else self.state.hidden[k] = true;
            self.apply();
        });
        box.querySelector('.gb-ch-list').addEventListener('click', function (e) {
            var row = e.target.closest('.gb-ch-row');
            if (!row) return;
            self.chooserSel = row.getAttribute('data-k');
            toArr(box.querySelectorAll('.gb-ch-row')).forEach(function (x) { x.classList.toggle('gb-sel', x === row); });
        });
        box.querySelector('.gb-ch-foot').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-m]');
            if (!b || !self.chooserSel) return;
            self.move(self.chooserSel, +b.getAttribute('data-m'));
        });
        /* drag the title bar */
        var head = box.querySelector('.gb-ch-head');
        head.addEventListener('mousedown', function (e) {
            if (e.target.closest('button')) return;
            var sx = e.clientX, sy = e.clientY, ox = box.offsetLeft, oy = box.offsetTop;
            function mv(ev) { box.style.left = (ox + ev.clientX - sx) + 'px'; box.style.top = (oy + ev.clientY - sy) + 'px'; }
            function up() { doc.removeEventListener('mousemove', mv); doc.removeEventListener('mouseup', up); }
            doc.addEventListener('mousemove', mv);
            doc.addEventListener('mouseup', up);
            e.preventDefault();
        });
        var firstCb = box.querySelector('input[type=checkbox]');
        if (firstCb) firstCb.focus();
    };
    Bar.prototype.closeChooser = function () {
        if (this.chooser) { this.chooser.remove(); this.chooser = null; }
    };
    Bar.prototype.paintChooser = function () {
        if (!this.chooser) return;
        var table = this.table();
        var list = this.chooser.querySelector('.gb-ch-list');
        if (!table) { list.innerHTML = ''; return; }
        var cols = readColumns(table), self = this;
        var order = this.orderFor(cols);
        var byKey = {};
        cols.forEach(function (c) { byKey[c.key] = c; });
        var canOrder = this.canReorder(table);
        this.chooser.querySelector('.gb-ch-foot').style.display = canOrder ? '' : 'none';
        list.innerHTML = order.map(function (k) {
            var c = byKey[k];
            if (!c || c.fixed) return '';
            return '<div class="gb-ch-row' + (self.chooserSel === k ? ' gb-sel' : '') + '" data-k="' + esc(k) + '">'
                + '<input type="checkbox" data-k="' + esc(k) + '" aria-label="' + esc(c.text) + '"' + (self.state.hidden[k] ? '' : ' checked') + '> '
                + '<span>' + esc(c.text) + '</span></div>';
        }).join('');
    };
    Bar.prototype.move = function (key, dir) {
        var table = this.table();
        if (!table || !this.canReorder(table)) return;
        var cols = readColumns(table);
        var order = this.orderFor(cols).slice();
        var fixed = {};
        cols.forEach(function (c) { if (c.fixed) fixed[c.key] = true; });
        var i = order.indexOf(key);
        var j = i + dir;
        while (j >= 0 && j < order.length && fixed[order[j]]) j += dir;   /* step over fixed columns */
        if (i < 0 || j < 0 || j >= order.length) return;
        order.splice(i, 1);
        order.splice(j, 0, key);
        this.state.order = order;
        this.apply();
    };

    /** Saved order merged with the columns actually rendered: unknown keys dropped, new ones kept in natural place. */
    Bar.prototype.orderFor = function (cols) {
        var natural = cols.map(function (c) { return c.key; });
        var saved = (this.state.order || []).filter(function (k) { return natural.indexOf(k) >= 0; });
        natural.forEach(function (k, i) {
            if (saved.indexOf(k) >= 0) return;
            var at = 0;
            for (var p = i - 1; p >= 0; p--) {
                var q = saved.indexOf(natural[p]);
                if (q >= 0) { at = q + 1; break; }
            }
            saved.splice(at, 0, k);
        });
        return saved;
    };

    /* --- apply the layout to whatever is rendered now */
    Bar.prototype.apply = function () {
        if (this.applying) return;
        var table = this.table();
        this.placeGear();
        if (!table) return;
        this.applying = true;
        this.pause();
        try {
            table.classList.add('gb-table');
            var cols = readColumns(table);
            if (!cols.length) return;
            var n = cols.length, self = this;
            var hiddenIdx = cols.map(function (c) { return !c.fixed && !!self.state.hidden[c.key]; });
            var order = this.orderFor(cols);
            var perm = order.map(function (k) { for (var i = 0; i < n; i++) if (cols[i].key === k) return i; return -1; })
                .filter(function (i) { return i >= 0; });
            var reorder = this.canReorder(table) && perm.some(function (p, i) { return p !== i; });

            allRows(table).forEach(function (tr) {
                ensureNatural(tr);
                var cells = naturalCells(tr);
                /* hide / show with colspan shrink for spanning cells */
                cells.forEach(function (c) {
                    var start = +c.getAttribute('data-gb-n'), span = spanOf(c), hid = 0;
                    for (var i = start; i < start + span && i < n; i++) if (hiddenIdx[i]) hid++;
                    var covered = Math.min(span, Math.max(0, n - start));
                    if (covered > 0 && hid === covered) c.classList.add('gb-hid');
                    else {
                        c.classList.remove('gb-hid');
                        if (span > 1) c.colSpan = span - hid;
                    }
                });
                /* order: only for 1:1 rows (canReorder guarantees every row is 1:1) */
                if (reorder && cells.length === n) {
                    var cur = toArr(tr.cells);
                    var same = perm.every(function (p, i) { return cur[i] === cells[p]; });
                    if (!same) perm.forEach(function (p) { tr.appendChild(cells[p]); });
                } else if (!reorder && cells.length === n) {
                    var cur2 = toArr(tr.cells);
                    var nat = cells.every(function (c, i) { return cur2[i] === c; });
                    if (!nat) cells.forEach(function (c) { tr.appendChild(c); });   /* back to natural order */
                }
            });

            /* widths on the key row */
            var kr = keyRow(table);
            naturalCells(kr).forEach(function (th, i) {
                var w = self.state.widths[cols[i].key];
                if (w && !self.autoAdjust) { th.style.width = w + 'px'; th.style.minWidth = w + 'px'; }
                self.addResizer(th, cols[i]);
            });
            if (this.chooser) this.paintChooser();
        } finally {
            this.resume();
            this.applying = false;
        }
    };

    Bar.prototype.addResizer = function (th, col) {
        if (col.fixed || th.querySelector(':scope > .gb-rsz')) return;
        var self = this;
        if (global.getComputedStyle(th).position === 'static') th.classList.add('gb-th-rel');
        var h = doc.createElement('span');
        h.className = 'gb-rsz';
        h.setAttribute('aria-hidden', 'true');
        h.addEventListener('click', function (e) { e.stopPropagation(); });
        h.addEventListener('mousedown', function (e) {
            e.preventDefault(); e.stopPropagation();
            var sx = e.clientX, w0 = th.offsetWidth;
            function mv(ev) {
                var w = Math.max(20, w0 + ev.clientX - sx);
                th.style.width = w + 'px'; th.style.minWidth = w + 'px';
            }
            function up() {
                doc.removeEventListener('mousemove', mv); doc.removeEventListener('mouseup', up);
                self.state.widths[col.key] = th.offsetWidth;
            }
            doc.addEventListener('mousemove', mv);
            doc.addEventListener('mouseup', up);
        });
        if (this.applying) th.appendChild(h);
        else { this.pause(); th.appendChild(h); this.resume(); }
    };

    Bar.prototype.captureWidths = function () {
        var table = this.table();
        if (!table) return;
        var cols = readColumns(table), self = this;
        naturalCells(keyRow(table)).forEach(function (th, i) {
            if (cols[i] && !cols[i].fixed && th.style.width) self.state.widths[cols[i].key] = th.offsetWidth;
        });
    };

    /** GridResizeColumnToolStripMenuItem_Click: first click stores the widths and auto-sizes, second restores. */
    Bar.prototype.autoAdjustColumns = function () {
        var table = this.table();
        if (!table) return;                                       /* RootTable == null -> nothing */
        var kr = keyRow(table);
        if (!kr || !kr.cells.length) return;                      /* Columns.Count == 0 -> nothing */
        var ths = naturalCells(kr), cols = readColumns(table), self = this;
        if (this.autoAdjust) {
            ths.forEach(function (th, i) {
                var w = self.autoWidths && self.autoWidths[i];
                th.style.width = w ? w + 'px' : ''; th.style.minWidth = w ? w + 'px' : '';
                if (w && cols[i]) self.state.widths[cols[i].key] = w;
            });
            this.autoAdjust = false;
        } else {
            this.autoWidths = ths.map(function (th) { return th.offsetWidth; });
            ths.forEach(function (th, i) {
                th.style.width = ''; th.style.minWidth = '';
                if (cols[i]) delete self.state.widths[cols[i].key];
            });
            this.autoAdjust = true;
        }
    };

    /** GroupCollaps_Click / GroupExpand_Click - the web grids have no Janus groups; rows marked as group
     *  headers (tr.gb-group, with data-gb-group) collapse their member rows (tr[data-gb-member=same]). */
    Bar.prototype.group = function (expand) {
        var table = this.table();
        if (!table) return;
        toArr(table.querySelectorAll('tr[data-gb-member]')).forEach(function (tr) { tr.classList.toggle('gb-collapsed', !expand); });
        toArr(table.querySelectorAll('tr.gb-group')).forEach(function (tr) { tr.classList.toggle('gb-group-closed', !expand); });
        table.dispatchEvent(new CustomEvent(expand ? 'gridbar:expand' : 'gridbar:collapse', { bubbles: true }));
    };

    /* --- print / export read the grid as shown: visible columns in the current order, visible rows */
    Bar.prototype.snapshot = function () {
        var table = this.table();
        if (!table) return { head: [], rows: [], foot: [] };
        function vis(el) { return !el.classList.contains('gb-hid') && global.getComputedStyle(el).display !== 'none'; }
        function cellText(c) {
            var ctl = c.querySelector('input, select, textarea');
            if (ctl) {
                if (ctl.type === 'checkbox' || ctl.type === 'radio') return ctl.checked ? 'True' : 'False';
                if (ctl.tagName === 'SELECT') return ctl.selectedIndex >= 0 ? ctl.options[ctl.selectedIndex].text : '';
                return ctl.value;
            }
            var cl = c.cloneNode(true);
            toArr(cl.querySelectorAll('.gb-rsz')).forEach(function (x) { x.remove(); });
            return (cl.textContent || '').replace(/\s+/g, ' ').trim();
        }
        function line(tr) {
            return toArr(tr.cells).filter(vis).map(function (c) {
                var t = cellText(c);
                return { t: t, span: c.colSpan || 1, num: /^\(?-?[\d,]+(\.\d+)?\)?$/.test(t) };
            });
        }
        var kr = keyRow(table);
        var head = kr ? [line(kr)] : [];
        var rows = [];
        toArr(table.tBodies).forEach(function (b) {
            toArr(b.rows).forEach(function (tr) { if (vis(tr)) rows.push(line(tr)); });
        });
        var foot = table.tFoot ? toArr(table.tFoot.rows).filter(vis).map(line) : [];
        return { head: head, rows: rows, foot: foot };
    };

    /** printToolStripMenuItem_Click - Landscape page, PageHeaderCenter = txtGridTitle, FitColumns = true. */
    Bar.prototype.print = function () {
        var s = this.snapshot();
        var w = global.open('', '_blank');
        if (!w) throw new Error('The print window was blocked by the browser.');
        function tr(cells, tag) {
            return '<tr>' + cells.map(function (c) {
                return '<' + tag + (c.span > 1 ? ' colspan="' + c.span + '"' : '') + (c.num && tag === 'td' ? ' class="n"' : '') + '>' + esc(c.t) + '</' + tag + '>';
            }).join('') + '</tr>';
        }
        var title = this.title();
        w.document.write('<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + esc(title || this.grid) + '</title><style>'
            + '@page{size:landscape;margin:10mm}body{font:8pt "Segoe UI",Tahoma,sans-serif;margin:0;color:#000}'
            + 'h1{font-size:10pt;text-align:center;margin:0 0 6px;font-weight:600}'
            + 'table{width:100%;border-collapse:collapse;table-layout:auto}th,td{border:1px solid #999;padding:2px 4px;text-align:left;vertical-align:top}'
            + 'th{background:#eee}td.n{text-align:right}thead{display:table-header-group}tfoot td{font-weight:600}'
            + '</style></head><body>' + (title ? '<h1>' + esc(title) + '</h1>' : '')
            + '<table><thead>' + s.head.map(function (r) { return tr(r, 'th'); }).join('') + '</thead><tbody>'
            + s.rows.map(function (r) { return tr(r, 'td'); }).join('') + '</tbody>'
            + (s.foot.length ? '<tfoot>' + s.foot.map(function (r) { return tr(r, 'td'); }).join('') + '</tfoot>' : '')
            + '</table></body></html>');
        w.document.close();
        w.focus();
        setTimeout(function () { try { w.print(); } catch (e) { /* the preview window stays open */ } }, 50);
    };

    /** exportToolStripMenuItem_Click - desktop writes .xls through GridEXExporter; the web writes CSV
     *  (Excel opens it) and shows the same "Exported successfully". */
    Bar.prototype.exportGrid = function () {
        var s = this.snapshot();
        function row(cells) {
            var out = [];
            cells.forEach(function (c) {
                out.push('"' + String(c.t).replace(/"/g, '""') + '"');
                for (var i = 1; i < c.span; i++) out.push('""');
            });
            return out.join(',');
        }
        var lines = s.head.concat(s.rows, s.foot).map(row);
        var blob = new Blob(['\ufeff' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8' });
        var name = (this.opts.fileName || (this.form + '_' + this.grid)).replace(/[\\\/:*?"<>|]+/g, '_') + '.csv';
        var a = doc.createElement('a');
        var url = URL.createObjectURL(blob);
        a.href = url;
        a.download = name;
        doc.body.appendChild(a);
        a.click();
        a.remove();
        setTimeout(function () { URL.revokeObjectURL(url); }, 1000);
        msg(T.exported);
    };

    /* --- re-render tracking */
    Bar.prototype.observe = function () {
        var self = this;
        if (!global.MutationObserver) return;
        this.mo = new MutationObserver(function (records) {
            if (self.applying) return;
            var relevant = records.some(function (r) {
                if (r.type !== 'childList') return false;
                return toArr(r.addedNodes).concat(toArr(r.removedNodes)).some(function (nd) {
                    return nd.nodeType === 1 && !(nd.classList && (nd.classList.contains('gb-rsz') || nd.classList.contains('gb-bar')));
                });
            });
            if (relevant && self.loaded) self.apply();
        });
        this.resume();
        /* the gear strip may be wiped when a parent re-renders */
        if (this.scope.parentNode) {
            this.pmo = new MutationObserver(function () {
                if (self.barEl && !self.barEl.isConnected) self.placeGear();
                else if (self.gear && !self.gear.isConnected) self.placeGear();
            });
            this.pmo.observe(this.scope.parentNode, { childList: true });
        }
    };
    Bar.prototype.pause = function () { if (this.mo) { this.mo.takeRecords(); this.mo.disconnect(); } };
    Bar.prototype.resume = function () {
        if (this.mo) this.mo.observe(this.scope, { childList: true, subtree: true });
    };

    /* ------------------------------------------------------------------ public API */
    function parseKey(scope, key) {
        if (key && typeof key === 'object') return { form: key.form, grid: key.grid || scope.id };
        var s = key || scope.getAttribute('data-gridbar') || '';
        var form, grid;
        if (s) {
            var i = s.indexOf(':');
            form = i >= 0 ? s.slice(0, i) : s;
            grid = i >= 0 ? s.slice(i + 1) : '';
        } else {
            form = scope.getAttribute('data-gridbar-form') || '';
            grid = scope.getAttribute('data-gridbar-grid') || '';
        }
        if (!grid) {
            var t = scope.tagName === 'TABLE' ? scope : scope.querySelector('table');
            grid = (t && t.id) || scope.id || '';
        }
        return { form: form, grid: grid };
    }
    function find(el) {
        el = resolveEl(el);
        for (var i = 0; i < bars.length; i++) {
            var b = bars[i];
            if (b.scope === el || b.table() === el) return b;
        }
        return null;
    }
    function attach(el, key, opts) {
        el = resolveEl(el);
        if (!el) return null;
        var existing = find(el);
        if (existing) { if (opts) existing.setRights(opts); existing.apply(); return existing; }
        var k = parseKey(el, key);
        if (!k.form || !k.grid) return null;
        var b = new Bar(el, k, opts || {});
        bars.push(b);
        return b;
    }
    function refresh(el) {
        var b = find(el);
        if (b) b.apply();
    }
    function setRights(el, opts) {
        var b = find(el);
        if (b) { b.setRights(opts || {}); b.apply(); }
    }
    /** Attach every marked table and host under root. A host whose table is not rendered yet (and that
     *  names no data-gridbar-grid) has no grid name to derive from the table id, so it waits for the table. */
    function scan(root) {
        root = root || doc;
        toArr(root.querySelectorAll('table[data-gridbar], table[data-gridbar-form]')).forEach(function (t) { attach(t); });
        toArr(root.querySelectorAll('[data-gridbar-form]:not(table)')).forEach(function (h) {
            if (find(h) || h.getAttribute('data-gridbar-watch') === '1') return;
            if (h.getAttribute('data-gridbar-grid') || h.querySelector('table')) { attach(h); return; }
            if (!global.MutationObserver) return;
            h.setAttribute('data-gridbar-watch', '1');
            var mo = new MutationObserver(function () {
                if (h.querySelector('table')) { mo.disconnect(); h.removeAttribute('data-gridbar-watch'); attach(h); }
            });
            mo.observe(h, { childList: true, subtree: true });
        });
    }

    doc.addEventListener('click', function (e) {
        if (openMenu && !openMenu.menu.contains(e.target) && !openMenu.gear.contains(e.target)) openMenu.closeMenu();
    });
    global.addEventListener('resize', function () { if (openMenu) openMenu.closeMenu(); });
    global.addEventListener('scroll', function () { if (openMenu) openMenu.closeMenu(); }, true);

    function boot() { scan(doc); }
    if (doc.readyState === 'loading') doc.addEventListener('DOMContentLoaded', boot); else boot();

    global.GridBar = {
        attach: attach,
        refresh: refresh,
        setRights: setRights,
        scan: scan,
        userName: userName,
        messages: T
    };
})(window);
