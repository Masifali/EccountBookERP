/* ============================================================================================
 * countx_adm_panel.js - Admin Panel (module 22)
 *
 *   body[data-screen="DefineCompany"]                                   Templates.DefineCompany                   (312)
 *   body[data-screen="AllocationOrganizationTemplateRights"]            Templates.AllocationOrganizationTemplateRights (314)
 *   body[data-screen="frmModuleAllocateToOrganizationTemplates"]        Templates.frmModuleAllocateToOrganizationTemplates (315)
 *   body[data-screen="frmScreensAllocateToCompany"]                     Templates.frmScreensAllocateToCompany     (316)
 *   body[data-screen="UserRightsEditing"]                               UserRightsManagement.UserRightsEditing    (318 / 397)
 *   body[data-screen="frmUserModuleAdmin"]                              Configurations.frmUserModuleAdmin         (319)
 *   body[data-screen="frmScreenDefinetion"]                             frmScreenDefinetion                       (791)
 *
 * Each handler follows its desktop method: validation wording and order, the Yes/No confirmations, the
 * messages, what New / Save / Update / double-click do to the buttons and the grids. The server repeats every
 * check and builds the model itself from the lists the desktop would have shown; nothing here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';

    var SCREEN = document.body.getAttribute('data-screen');
    var API = '/api/adm-panel';

    // ------------------------------------------------------------------------------ plumbing

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function num(v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; }
    function truthy(v) { if (v === true) return true; var s = str(v).toLowerCase(); return s === 'true' || s === '1'; }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function fail(e) { box(e && e.message ? e.message : String(e)); }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden') && !e.disabled; }
    function focus(id) {
        var e = $id(id); if (!e) return;
        var inp = e.tagName === 'SELECT' && e.parentNode ? e.parentNode.querySelector('.dtcombo-input') : null;
        if (inp) inp.focus(); else e.focus();
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.textContent = v; }
    /** BindDDL / BindDDLNew: zero = the "...Select Any Value..." row (ZeroIndex true) or a blank row (false). */
    function fill(id, rows, valueKey, textKey, zero) {
        var sel = $id(id); if (!sel) return;
        var html = zero ? '<option value="0">...Select Any Value...</option>' : '<option value="0"></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(r[valueKey]) + '">' + esc(r[textKey]) + '</option>'; });
        sel.innerHTML = html;
        sel.value = '0';
    }
    function setValue(id, v) {
        var s = $id(id), x = String(v);
        if (!s) return;
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === x) { s.selectedIndex = i; return; }
        s.value = '0';
    }
    /** UltraCombo.Text: '' when no row is active (the zero row's caption is not text the user typed). */
    function comboText(id) { var s = $id(id); return (!s || s.selectedIndex < 0 || s.value === '0') ? '' : s.options[s.selectedIndex].textContent; }
    var toastTimer = null;
    function toast(m) {
        var t = $id('apToast'); if (!t) return;
        t.textContent = m; t.classList.add('is-on');
        clearTimeout(toastTimer); toastTimer = setTimeout(function () { t.classList.remove('is-on'); }, 2500);
    }
    function closeForm() {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/admin-panel'; } }, 150);
    }
    /** Enter moves on like SendKeys.Send("{TAB}"). */
    function nextField(from) {
        var all = Array.prototype.filter.call(document.querySelectorAll('input, select, textarea, button, .dtcombo-input'), function (el) {
            return !el.disabled && el.offsetParent !== null && el.type !== 'hidden' && el.tabIndex !== -1 && el.tagName !== 'SELECT';
        });
        var i = all.indexOf(from);
        if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
    }
    /** ShortCutKeyPopUp(dt) - KeyCombination / Description. */
    function shortcutBox(title, rows) {
        var m = $id('apModal');
        if (!m) {
            m = document.createElement('div'); m.id = 'apModal'; m.className = 'ap-modal';
            m.innerHTML = '<div class="ap-modal-box"><h4></h4><table><tbody></tbody></table><div class="ap-modal-foot"><button type="button" class="ap-teal-btn" style="width:70px;height:24px;">Close</button></div></div>';
            document.body.appendChild(m);
            m.addEventListener('click', function (e) { if (e.target === m || e.target.tagName === 'BUTTON') m.classList.remove('is-on'); });
        }
        m.querySelector('h4').textContent = title || 'ShortCut Keys';
        m.querySelector('tbody').innerHTML = rows.map(function (r) { return '<tr><td><b>' + esc(r[0]) + '</b></td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        m.classList.add('is-on');
    }
    /** Enter-as-Tab for text boxes (the desktop forms' KeyDown SendKeys.Send("{TAB}")). */
    function enterAsTab() {
        document.addEventListener('keydown', function (e) {
            if (e.key !== 'Enter' || e.ctrlKey || e.shiftKey || e.altKey) return;
            var t = e.target;
            if (t && (t.tagName === 'TEXTAREA' || t.tagName === 'BUTTON' || (t.tagName === 'INPUT' && (t.type === 'button' || t.type === 'checkbox')))) return;
            if (t && t.classList && t.classList.contains('ap-cell')) { e.preventDefault(); nextField(t); return; }
            if (t && t.tagName === 'INPUT') { e.preventDefault(); nextField(t); }
        });
    }

    // ------------------------------------------------------------------------------ GridEX stand-in

    /**
     * cfg: { cols:[{key, caption, width, hidden, type:'text'|'bool'|'link'|'edit'|'button', edit, headerSelect, align, btn}],
     *        selector:bool (ActAsSelector + UseHeaderSelector, 30 wide), filter:bool (FilterRow, contains),
     *        groups:[{key, caption, desc}], collapseGroups:bool, rowClass(row), onSelect(row,i), onDblClick(row,i),
     *        onButton(key,row,i), onCell(key,row,i), auto:bool (columns sized to content) }
     */
    function Grid(tableId, cfg) {
        var self = this;
        this.t = $id(tableId);
        this.cfg = cfg;
        this.rows = [];
        this.cur = -1;
        this.filters = {};
        this.collapsed = {};
        this.head();
        this.body = this.t.querySelector('tbody');
        this.body.addEventListener('click', function (e) { self.onClick(e); });
        this.body.addEventListener('dblclick', function (e) { self.onDbl(e); });
        this.body.addEventListener('change', function (e) { self.onChange(e); });
        this.body.addEventListener('input', function (e) { self.onInput(e); });
        this.t.querySelector('thead').addEventListener('change', function (e) { self.onHeadChange(e); });
        this.t.querySelector('thead').addEventListener('input', function (e) { self.onFilter(e); });
        this.render();
    }
    Grid.prototype.vis = function () { return this.cfg.cols.filter(function (c) { return !c.hidden; }); };
    Grid.prototype.span = function () { return this.vis().length + (this.cfg.selector ? 1 : 0); };
    Grid.prototype.head = function () {
        var c = this.cfg, h = '<tr>', f = '<tr class="ap-filter">';
        if (c.selector) { h += '<th class="ap-center" style="width:30px;"><input type="checkbox" data-all="sel"/></th>'; f += '<th></th>'; }
        this.vis().forEach(function (col) {
            var w = col.width ? ' style="width:' + col.width + 'px;"' : '';
            h += '<th' + w + '>' + (col.headerSelect ? '<input type="checkbox" data-all="' + esc(col.key) + '" style="margin-right:4px;"/>' : '') + esc(col.caption === undefined ? col.key : col.caption) + '</th>';
            f += '<th>' + (c.filter && col.type !== 'button' && col.type !== 'bool' ? '<input data-f="' + esc(col.key) + '"/>' : '') + '</th>';
        });
        h += '</tr>'; f += '</tr>';
        this.t.querySelector('thead').innerHTML = h + (c.filter ? f : '');
        if (c.auto) this.t.classList.add('ap-auto');
    };
    Grid.prototype.matches = function (r) {
        for (var k in this.filters) {
            var f = this.filters[k];
            if (f && str(r[k]).toLowerCase().indexOf(f) < 0) return false;
        }
        return true;
    };
    Grid.prototype.idx = function () {
        var out = [];
        for (var i = 0; i < this.rows.length; i++) if (this.matches(this.rows[i])) out.push(i);
        return out;
    };
    Grid.prototype.render = function () {
        var self = this, html = [], ix = this.idx();
        if (this.cfg.groups && this.cfg.groups.length) this.group(ix, 0, '', html);
        else ix.forEach(function (i) { html.push(self.rowHtml(i)); });
        this.body.innerHTML = html.join('');
    };
    Grid.prototype.group = function (ix, level, path, html) {
        var g = this.cfg.groups[level], self = this;
        if (!g) { ix.forEach(function (i) { html.push(self.rowHtml(i)); }); return; }
        var order = [], map = {};
        ix.forEach(function (i) {
            var v = str(self.rows[i][g.key]);
            if (!Object.prototype.hasOwnProperty.call(map, v)) { map[v] = []; order.push(v); }
            map[v].push(i);
        });
        order.sort(function (a, b) { a = a.toLowerCase(); b = b.toLowerCase(); return a < b ? -1 : a > b ? 1 : 0; });
        if (g.desc) order.reverse();
        order.forEach(function (v) {
            var p = path + '/' + level + ':' + v;
            if (self.collapsed[p] === undefined) self.collapsed[p] = !!self.cfg.collapseGroups;
            var col = self.collapsed[p];
            html.push('<tr class="ap-group" data-gp="' + esc(p) + '"><td colspan="' + self.span() + '" style="padding-left:' + (4 + level * 14) + 'px;"><span class="ap-tw">' + (col ? '+' : '&minus;') + '</span>' + esc(g.caption || g.key) + ' : ' + esc(v) + '</td></tr>');
            if (!col) self.group(map[v], level + 1, p, html);
        });
    };
    Grid.prototype.rowHtml = function (i) {
        var r = this.rows[i], c = this.cfg;
        var cls = (i === this.cur ? 'is-current ' : '') + (c.rowClass ? c.rowClass(r) || '' : '');
        var h = '<tr data-i="' + i + '" class="' + cls + '">';
        if (c.selector) h += '<td class="ap-center"><input type="checkbox" data-sel="' + i + '"' + (r._chk ? ' checked' : '') + '/></td>';
        this.vis().forEach(function (col) {
            var v = r[col.key], t = col.type || 'text', a = col.align === 'center' ? ' class="ap-center"' : (col.align === 'right' ? ' class="ap-num"' : '');
            if (t === 'bool') h += '<td class="ap-center"><input type="checkbox" data-cell="' + esc(col.key) + '"' + (truthy(v) ? ' checked' : '') + (col.edit ? '' : ' disabled') + '/></td>';
            else if (t === 'edit') h += '<td><input class="ap-cell" data-cell="' + esc(col.key) + '" value="' + esc(v) + '"/></td>';
            else if (t === 'button') h += '<td class="ap-center"><button type="button" class="ap-editbtn" data-btn="' + esc(col.key) + '">' + esc(col.btn || col.caption) + '</button></td>';
            else if (t === 'link') h += '<td title="' + esc(v) + '">' + (r.url ? '<a class="ap-link" href="' + esc(r.url) + '" target="_blank" rel="noopener">' + esc(v) + '</a>' : '<span class="ap-link">' + esc(v) + '</span>') + '</td>';
            else h += '<td' + a + ' title="' + esc(v) + '">' + esc(col.fmt ? col.fmt(v, r) : v) + '</td>';
        });
        return h + '</tr>';
    };
    Grid.prototype.load = function (rows, noSelect) {
        this.rows = rows || [];
        this.cur = this.rows.length ? 0 : -1;
        this.collapsed = {};
        this.filters = {};
        this.t.querySelectorAll('thead input[data-f]').forEach(function (x) { x.value = ''; });
        this.t.querySelectorAll('thead input[data-all]').forEach(function (x) { x.checked = false; });
        this.render();
        if (!noSelect && this.cur >= 0 && this.cfg.onSelect) this.cfg.onSelect(this.rows[0], 0);
    };
    Grid.prototype.clear = function () { this.rows = []; this.cur = -1; this.render(); };
    Grid.prototype.current = function () { return this.cur >= 0 ? this.rows[this.cur] : null; };
    Grid.prototype.checked = function () { return this.rows.filter(function (r) { return r._chk; }); };
    Grid.prototype.setCurrent = function (i, silent) {
        this.cur = i;
        this.body.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
        var tr = this.body.querySelector('tr[data-i="' + i + '"]');
        if (tr) tr.classList.add('is-current');
        if (!silent && this.cfg.onSelect && i >= 0) this.cfg.onSelect(this.rows[i], i);
    };
    Grid.prototype.collapseAll = function (on) {
        var self = this;
        Object.keys(this.collapsed).forEach(function (k) { self.collapsed[k] = on; });
        this.cfg.collapseGroups = on;
        this.render();
    };
    Grid.prototype.onClick = function (e) {
        var g = e.target.closest('tr.ap-group');
        if (g) { var p = g.getAttribute('data-gp'); this.collapsed[p] = !this.collapsed[p]; this.render(); return; }
        var tr = e.target.closest('tr[data-i]'); if (!tr) return;
        var i = num(tr.getAttribute('data-i'));
        var b = e.target.closest('button[data-btn]');
        if (i !== this.cur) this.setCurrent(i);
        if (b && this.cfg.onButton) this.cfg.onButton(b.getAttribute('data-btn'), this.rows[i], i);
    };
    Grid.prototype.onDbl = function (e) {
        var tr = e.target.closest('tr[data-i]'); if (!tr || e.target.closest('input,button,a')) return;
        var i = num(tr.getAttribute('data-i'));
        if (this.cfg.onDblClick) this.cfg.onDblClick(this.rows[i], i);
    };
    Grid.prototype.onChange = function (e) {
        var t = e.target, tr = t.closest('tr[data-i]'); if (!tr) return;
        var i = num(tr.getAttribute('data-i'));
        if (t.hasAttribute('data-sel')) this.rows[i]._chk = t.checked;
        else if (t.hasAttribute('data-cell') && t.type === 'checkbox') {
            this.rows[i][t.getAttribute('data-cell')] = t.checked;
            if (this.cfg.onCell) this.cfg.onCell(t.getAttribute('data-cell'), this.rows[i], i);
        }
    };
    Grid.prototype.onInput = function (e) {
        var t = e.target; if (!t.classList.contains('ap-cell')) return;
        var tr = t.closest('tr[data-i]'); if (!tr) return;
        var i = num(tr.getAttribute('data-i')), key = t.getAttribute('data-cell');
        this.rows[i][key] = t.value;
        if (this.cfg.onCell) this.cfg.onCell(key, this.rows[i], i);
    };
    Grid.prototype.onHeadChange = function (e) {
        var t = e.target; if (!t.hasAttribute || !t.hasAttribute('data-all')) return;
        var key = t.getAttribute('data-all'), self = this, on = t.checked;
        this.idx().forEach(function (i) { if (key === 'sel') self.rows[i]._chk = on; else self.rows[i][key] = on; });
        this.body.querySelectorAll(key === 'sel' ? 'input[data-sel]' : 'input[data-cell="' + key + '"]').forEach(function (x) { x.checked = on; });
        if (key !== 'sel' && this.cfg.onCell) this.cfg.onCell(key, null, -1);
    };
    Grid.prototype.onFilter = function (e) {
        var t = e.target; if (!t.hasAttribute || !t.hasAttribute('data-f')) return;
        this.filters[t.getAttribute('data-f')] = t.value.toLowerCase();
        this.render();
    };

    /** Ctrl-key dispatcher shared by the allocator forms: the handlers map holds the desktop KeyDown branches. */
    function bindKeys(h) {
        document.addEventListener('keydown', function (e) {
            if (e.ctrlKey && e.altKey && (e.key === 'Alt' || e.key === 'Control')) { if (h.alt) h.alt(); return; }
            if (!e.ctrlKey) {
                if (e.key === 'Escape' && h.close && !document.querySelector('.dtcombo-pop[style*="block"]') && !document.querySelector('.ap-modal.is-on')) h.close();
                return;
            }
            var k = e.key.toLowerCase();
            var fn = h[k] || (e.key === 'F5' ? h.f5 : null) || (e.key === 'ArrowUp' ? h.up : null) || (e.key === 'ArrowDown' ? h.down : null) ||
                     (e.key === 'ArrowLeft' ? h.left : null) || (e.key === 'ArrowRight' ? h.right : null) || (e.key === 'Delete' ? h.del : null);
            if (fn) { e.preventDefault(); fn(e); }
        });
    }
    function focusGrid(tableId) { var t = $id(tableId); var s = t && t.closest('.ap-scroll'); if (s) s.focus(); }
    function notBuilt(name) { box(name + ' is not available on the web yet.'); }

    // ==============================================================================================
    // 319 frmUserModuleAdmin
    // ==============================================================================================
    if (SCREEN === 'frmUserModuleAdmin') {
        var curUser = null, whichGrid = 'un';
        var gUsers = new Grid('grdItemName', {
            filter: true,
            cols: [{ key: 'UserId', hidden: true }, { key: 'UserName', caption: 'UserName', width: 400 }],
            onSelect: function (r) { selectUser(r); }
        });
        function modCols() {
            return [{ key: 'UserModuleAdminId', hidden: true }, { key: 'UserId', hidden: true }, { key: 'UserName', hidden: true },
                    { key: 'ModuleId', hidden: true }, { key: 'ModuleName', caption: 'ModuleName', width: 320 }, { key: 'IsActive', hidden: true }];
        }
        var gUn = new Grid('GridUnAllocatedTemplateII', { selector: true, filter: true, cols: modCols() });
        var gAl = new Grid('GridAllocatedTemplateII', { selector: true, filter: true, cols: modCols() });

        function UsersGridFill() {
            return getJson(API + '/uma/users').then(function (rows) {
                if (rows && rows.length) gUsers.load(rows); else gUsers.clear();
            }).catch(fail);
        }
        function selectUser(r) {
            curUser = r;
            return getJson(API + '/uma/grids?userId=' + num(r.UserId)).then(function (g) {
                gUn.load(g.unallocated || [], true);
                gAl.load(g.allocated || [], true);
                setText('labelUnAllocateGridTemplateII', "UnAllocated Modules Of User : '" + str(r.UserName) + "'");
                setText('labelAllocateGridTemplateII', "Allocated Modules Of User : '" + str(r.UserName) + "'");
            }).catch(fail);
        }
        function insertTemplateII(grid, message, btn) {
            var rows = grid.checked();
            if (rows.length === 0) { box("Check Row's Which You want To " + message); return; }
            if (!ask('Are you sure to ' + message + ' Modules?')) return;
            return busy(btn, function () {
                return postJson(API + '/uma/save', { userId: num(curUser && curUser.UserId), mode: message, moduleIds: rows.map(function (r) { return r.ModuleId; }) })
                    .then(function (r) { box(r.message); return curUser ? selectUser(curUser) : null; }).catch(fail);
            });
        }
        function allocate() { return insertTemplateII(gUn, 'Allocate', $id('BtnSaveTemplateII')); }
        function update() { return insertTemplateII(gAl, 'Update', $id('BtnUpdateTemplateII')); }
        function refresh() { return busy($id('btnRefresh'), UsersGridFill); }
        var SC = [['Ctrl+A/Ctrl+S', 'For Press Allocate Button'], ['Ctrl+U/Ctrl+D/Ctrl+Delete', 'For Press UnAllocate Button'], ['Ctrl+E', 'For Close'],
                  ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on User Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                  ['Ctrl+ArrowRight', 'to change focus from one grid to another']];
        window.AP = {
            refresh: refresh,
            defineUser: function () { window.open('/user-management/user-define', '_blank'); },
            shortcuts: function () { shortcutBox('ShortCut Keys', SC); },
            allocate: allocate, update: update
        };
        $id('GridUnAllocatedTemplateII').closest('.ap-scroll').addEventListener('focus', function () { whichGrid = 'un'; });
        $id('GridAllocatedTemplateII').closest('.ap-scroll').addEventListener('focus', function () { whichGrid = 'al'; });
        bindKeys({
            close: closeForm, e: closeForm, r: refresh, a: allocate, s: allocate, u: update, d: update, del: update,
            f5: function () { focusGrid('grdItemName'); }, up: function () { focusGrid('grdItemName'); },
            left: function () { focusGrid('GridUnAllocatedTemplateII'); }, right: function () { focusGrid('GridAllocatedTemplateII'); },
            down: function () { focusGrid(whichGrid === 'un' ? 'GridAllocatedTemplateII' : 'GridUnAllocatedTemplateII'); },
            alt: function () { AP.shortcuts(); }
        });
        UsersGridFill();
    }

    // ==============================================================================================
    // 315 frmModuleAllocateToOrganizationTemplates
    // ==============================================================================================
    if (SCREEN === 'frmModuleAllocateToOrganizationTemplates') {
        var screensModuleId = 0, whichGrid2 = 'un';
        var gTpl = new Grid('grdTemplateName', {
            cols: [{ key: 'Id', hidden: true }, { key: 'TemplateName', caption: 'TemplateName', width: 200 }, { key: 'TemplateDescription', hidden: true, width: 100 }],
            onSelect: function (r) { grdTemplateName_SelectionChanged(r); }
        });
        function modCols2() {
            return [{ key: 'Id', hidden: true }, { key: 'TemplateId', hidden: true }, { key: 'TemplateName', hidden: true }, { key: 'ModuleId', hidden: true },
                    { key: 'ModuleName', caption: 'ModuleName', width: 180 }, { key: 'IsActive', hidden: true }];
        }
        var gMUn = new Grid('GridUnAllocatedTemplateII', { selector: true, cols: modCols2() });
        var gMAl = new Grid('GridAllocatedTemplateII', { selector: true, cols: modCols2(), onSelect: function () { ScreensGridFill(false); } });
        function screenCols() {
            return [{ key: 'Id', hidden: true }, { key: 'ScreenId', hidden: true }, { key: 'ModuleId', hidden: true }, { key: 'ModuleName', caption: 'Module', hidden: true },
                    { key: 'TemplateId', hidden: true }, { key: 'TemplateName', hidden: true }, { key: 'ScreenName', hidden: true },
                    { key: 'ScreenAlias', caption: 'Screen Name', width: 170, type: 'link' }, { key: 'IsActive', hidden: true }, { key: 'TargetUrl', hidden: true }];
        }
        var gActive = new Grid('grdActiveScreens', { selector: true, cols: screenCols(), groups: [{ key: 'ModuleName', caption: 'Module' }] });
        var gInactive = new Grid('grdInActiveScreens', { selector: true, cols: screenCols(), groups: [{ key: 'ModuleName', caption: 'Module' }] });

        function TemPlateGridFill() {
            return getJson(API + '/templates').then(function (rows) {
                if (rows && rows.length) gTpl.load(rows); else gMUn.clear();
            }).catch(fail);
        }
        function grdTemplateName_SelectionChanged(r) {
            var tid = num(r.Id), name = str(r.TemplateName);
            return getJson(API + '/mat/modules?templateId=' + tid).then(function (g) {
                gMUn.load(g.unallocated || [], true);
                setText('labelUnAllocateGridTemplateII', "UnAllocated Modules Of Template : '" + name + "'");
                setText('labelAllocateGridTemplateII', "Allocated Modules Of Template : '" + name + "'");
                if ((g.allocated || []).length) gMAl.load(g.allocated); else { gMAl.clear(); ScreensGridFill(false); }
            }).catch(fail);
        }
        function ScreensGridFill(allModules) {
            var templateId = 0, moduleId = 0, moduleName = 'All Allocated Modules';
            var r = gMAl.current();
            if (!allModules && r) {
                templateId = num(r.TemplateId); moduleId = num(r.ModuleId);
                moduleName = moduleId === 0 ? 'All Allocated Modules' : str(r.ModuleName);
            } else { gActive.clear(); gInactive.clear(); }
            if (templateId === 0) { var t = gTpl.current(); if (t) templateId = num(t.Id); }
            if (templateId > 0) {
                screensModuleId = moduleId;
                return getJson(API + '/mat/screens?templateId=' + templateId + '&moduleId=' + moduleId).then(function (g) {
                    var a = g.active || [], i = g.inactive || [];
                    a.forEach(function (x) { x.TemplateId = templateId; });
                    i.forEach(function (x) { x.TemplateId = templateId; });
                    gActive.load(a, true); gInactive.load(i, true);
                    setText('label1', "Active Screens Of Module : '" + moduleName + "'");
                    setText('label2', "InActive Screens Of Module : '" + moduleName + "'");
                }).catch(fail);
            }
        }
        function InsertTemplate(grid, saveRecord, btn) {
            var msg = saveRecord ? 'Allocate' : 'UnAllocate';
            var rows = grid.checked();
            if (rows.length === 0) { box("Check Row's Which You want To " + msg); return; }
            if (!ask('Are you sure to ' + msg + ' Modules?')) return;
            return busy(btn, function () {
                return postJson(API + '/mat/save-modules', { templateId: num(rows[0].TemplateId), allocate: saveRecord, moduleIds: rows.map(function (r) { return r.ModuleId; }) })
                    .then(function (r) { box(r.message); var t = gTpl.current(); return t ? grdTemplateName_SelectionChanged(t) : null; }).catch(fail);
            });
        }
        function InsertScreen(grid, saveRecord, btn) {
            var msg = saveRecord ? 'Allocate' : 'UnAllocate';
            var rows = grid.checked();
            if (rows.length === 0) { box("Check Row's Which You want To " + msg); return; }
            if (!ask('Are you sure to ' + msg + ' Modules?')) return;
            return busy(btn, function () {
                return postJson(API + '/mat/save-screens', {
                    templateId: num(rows[0].TemplateId), moduleId: screensModuleId, allocate: saveRecord,
                    screens: rows.map(function (r) { return { moduleId: r.ModuleId, screenName: r.ScreenName }; })
                }).then(function (r) { box(r.message); return ScreensGridFill(true); }).catch(fail);
            });
        }
        function allocateModules() { return InsertTemplate(gMUn, true, $id('BtnSaveTemplateII')); }
        function unAllocateModules() { return InsertTemplate(gMAl, false, $id('btnDeleteTemplateII')); }
        function refresh() { return busy($id('btnRefresh'), TemPlateGridFill); }
        var SC2 = [['Ctrl+A/Ctrl+S', 'For Press Allocate Button'], ['Ctrl+U', 'For Press Update Button'], ['Ctrl+D/Ctrl+Delete', 'For Press UnAllocate Button'],
                   ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on Template Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                   ['Ctrl+ArrowRight', 'to change focus from one grid to another']];
        window.AP = {
            refresh: refresh,
            defineTemplate: function () { notBuilt('Define Template (OrganizationTemplates)'); },
            shortcuts: function () { shortcutBox('ShortCut Keys', SC2); },
            refreshScreens: function (btn) { return busy(btn, function () { return ScreensGridFill(true); }); },
            allocateModules: allocateModules, unAllocateModules: unAllocateModules,
            allocateScreens: function (btn) { return InsertScreen(gActive, true, btn); },
            unAllocateScreens: function (btn) { return InsertScreen(gInactive, false, btn); }
        };
        $id('GridUnAllocatedTemplateII').closest('.ap-scroll').addEventListener('focus', function () { whichGrid2 = 'un'; });
        $id('GridAllocatedTemplateII').closest('.ap-scroll').addEventListener('focus', function () { whichGrid2 = 'al'; });
        bindKeys({
            close: closeForm, e: closeForm, r: refresh, u: unAllocateModules, a: allocateModules,
            f5: function () { focusGrid('grdTemplateName'); }, up: function () { focusGrid('grdTemplateName'); },
            down: function () { focusGrid(whichGrid2 === 'un' ? 'GridAllocatedTemplateII' : 'GridUnAllocatedTemplateII'); },
            left: function () { focusGrid('GridUnAllocatedTemplateII'); }, right: function () { focusGrid('GridAllocatedTemplateII'); },
            alt: function () { AP.shortcuts(); }
        });
        TemPlateGridFill();
    }

    // ==============================================================================================
    // 316 frmScreensAllocateToCompany
    // ==============================================================================================
    if (SCREEN === 'frmScreensAllocateToCompany') {
        var whichGrid3 = 'un';
        var gCompany = new Grid('grdCompany', {
            cols: [{ key: 'Id', hidden: true }, { key: 'CompanyName', caption: 'CompanyName', width: 200 }],
            onSelect: function (r) { grdCompany_SelectionChanged(r); }
        });
        var gModules = new Grid('grdModules', {
            cols: [{ key: 'Id', hidden: true }, { key: 'TemplateId', hidden: true }, { key: 'TemplateName', hidden: true }, { key: 'ModuleId', hidden: true },
                   { key: 'ModuleName', caption: 'ModuleName', width: 250 }, { key: 'IsActive', hidden: true }, { key: 'CompanyId', hidden: true }, { key: 'CompanyName', hidden: true }],
            onSelect: function (r) { grdModules_SelectionChanged(r); }
        });
        function scrCols() {
            return [{ key: 'Id', hidden: true }, { key: 'CompanyId', hidden: true }, { key: 'CompanyName', hidden: true }, { key: 'ScreenId', hidden: true },
                    { key: 'ScreenName', hidden: true }, { key: 'ScreenAlias', caption: 'Screen Name', width: 180, type: 'link' }, { key: 'IsActive', hidden: true }, { key: 'TargetUrl', hidden: true }];
        }
        var gSUn = new Grid('GridUnAllocatedTemplateII', { selector: true, cols: scrCols() });
        var gSAl = new Grid('GridAllocatedTemplateII', { selector: true, cols: scrCols() });
        var lastTemplate = 0;

        function templateText() { var s = $id('CmbTemplate'); return s && s.selectedIndex >= 0 ? s.options[s.selectedIndex].textContent : ''; }
        function TemplateNameFill() {
            var keep = num(val('CmbTemplate'));
            return getJson(API + '/templates').then(function (rows) {
                if (!rows || !rows.length) return;
                fill('CmbTemplate', rows, 'Id', 'TemplateName', true);
                if (keep > 0) setValue('CmbTemplate', keep);
                refreshCombos();
            }).catch(fail);
        }
        function CompanyGridFill(templateId) {
            lastTemplate = templateId;
            return getJson(API + '/sac/companies?templateId=' + templateId).then(function (rows) {
                if (rows && rows.length) gCompany.load(rows);
                else {
                    gCompany.clear(); gModules.clear(); gSUn.clear(); gSAl.clear();
                    setText('labelUnAllocateGridTemplateII', "UnAllocated Screens Of Company : ''");
                    setText('labelAllocateGridTemplateII', "Allocated Screens Of Company : ''");
                }
            }).catch(fail);
        }
        function BtnShow() {
            return CompanyGridFill(num(val('CmbTemplate'))).then(function () {
                setText('labelModulesOfSelectedTemplate', "Allocated Modules Of Template : '" + templateText() + "'");
            });
        }
        function grdCompany_SelectionChanged(r) {
            var tid = num(val('CmbTemplate'));
            return getJson(API + '/sac/modules?templateId=' + tid).then(function (rows) {
                rows = rows || [];
                rows.forEach(function (x) { x.CompanyId = r.Id; x.CompanyName = r.CompanyName; });
                if (rows.length) gModules.load(rows); else { gModules.clear(); gSUn.clear(); gSAl.clear(); }
            }).catch(fail);
        }
        function grdModules_SelectionChanged(r) {
            var cid = num(r.CompanyId), mid = num(r.ModuleId);
            return getJson(API + '/sac/screens?templateId=' + num(val('CmbTemplate')) + '&companyId=' + cid + '&moduleId=' + mid).then(function (g) {
                var u = g.unallocated || [], a = g.allocated || [];
                u.forEach(function (x) { x.CompanyId = cid; x.CompanyName = r.CompanyName; });
                a.forEach(function (x) { x.CompanyId = cid; x.CompanyName = r.CompanyName; });
                gSUn.load(u, true); gSAl.load(a, true);
                setText('labelUnAllocateGridTemplateII', "UnAllocated Screens Of Company : '" + str(r.CompanyName) + "'");
                setText('labelAllocateGridTemplateII', "Allocated Screens Of Company : '" + str(r.CompanyName) + "'");
            }).catch(fail);
        }
        function Insert(grid, saveRecord, btn) {
            var msg = saveRecord ? 'Allocate' : 'UnAllocate';
            var rows = grid.checked();
            if (rows.length === 0) { box("Check Row's Which You want To " + msg); return; }
            if (!ask('Are you sure to ' + msg + ' Screens?')) return;
            var m = gModules.current();
            return busy(btn, function () {
                return postJson(API + '/sac/save', {
                    templateId: num(val('CmbTemplate')), companyId: num(rows[0].CompanyId), moduleId: m ? num(m.ModuleId) : 0,
                    allocate: saveRecord, screenNames: rows.map(function (r) { return r.ScreenName; })
                }).then(function (r) { box(r.message); return m ? grdModules_SelectionChanged(m) : null; }).catch(fail);
            });
        }
        function allocate() { return Insert(gSUn, true, $id('BtnSaveTemplateII')); }
        function unallocate() { return Insert(gSAl, false, $id('btnDeleteTemplateII')); }
        function refresh() { return busy($id('btnRefresh'), function () { return TemplateNameFill().then(function () { return CompanyGridFill(num(val('CmbTemplate'))); }); }); }
        var SC3 = [['Ctrl+A/Ctrl+S', 'For Press Allocate Button'], ['Ctrl+U', 'For Press Update Button'], ['Ctrl+D/Ctrl+Delete', 'For Press UnAllocate Button'],
                   ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on Template Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                   ['Ctrl+ArrowRight', 'to change focus from one grid to another']];
        window.AP = {
            refresh: refresh,
            show: function (btn) { return busy(btn, BtnShow); },
            defineTemplate: function () { notBuilt('Define Template (OrganizationTemplates)'); },
            shortcuts: function () { shortcutBox('ShortCut Keys', SC3); },
            allocate: allocate, unallocate: unallocate
        };
        $id('GridUnAllocatedTemplateII').closest('.ap-scroll').addEventListener('focus', function () { whichGrid3 = 'un'; });
        $id('GridAllocatedTemplateII').closest('.ap-scroll').addEventListener('focus', function () { whichGrid3 = 'al'; });
        bindKeys({
            close: closeForm, e: closeForm, r: refresh, u: unallocate, a: allocate,
            f5: function () { focusGrid('grdCompany'); }, up: function () { focusGrid('grdCompany'); },
            down: function () { focusGrid(whichGrid3 === 'un' ? 'GridAllocatedTemplateII' : 'GridUnAllocatedTemplateII'); },
            left: function () { focusGrid('GridUnAllocatedTemplateII'); }, right: function () { focusGrid('GridAllocatedTemplateII'); },
            alt: function () { AP.shortcuts(); }
        });
        TemplateNameFill().then(function () { focus('CmbTemplate'); });
    }

    // ==============================================================================================
    // 314 AllocationOrganizationTemplateRights
    // ==============================================================================================
    if (SCREEN === 'AllocationOrganizationTemplateRights') {
        var whichGrid4 = 'un';
        function trCols() { return [{ key: 'ScreenId', hidden: true }, { key: 'ScreenName', caption: 'ScreenName', width: 340 }]; }
        var tUn = new Grid('grdPending', { selector: true, cols: trCols() });
        var tAl = new Grid('grdAllocated', { selector: true, cols: trCols() });

        /** ShowData - UnAllocatedItemsGridFill + AllocatedItemsGridFill. */
        function ShowData() {
            var t = num(val('CmbTemplateName'));
            if (t <= 0) { box('Select Template Name First'); return Promise.resolve(); }
            return getJson(API + '/tr/screens?templateId=' + t + '&moduleId=' + num(val('CmbModuleName'))).then(function (r) {
                tUn.load(r.unallocated || [], true);
                tAl.load(r.allocated || [], true);
            }).catch(fail);
        }
        function ids(grid) { return grid.checked().map(function (r) { return r.ScreenId; }); }
        function allocate4() {
            var t = num(val('CmbTemplateName'));
            if (t === 0) { box('Select Template Name First'); return; }
            if (ids(tUn).length === 0) { box("Checked Row's first To Allocate Items"); return; }
            return busy('BtnAllocateItems', function () {
                return postJson(API + '/tr/allocate', { templateId: t, moduleId: num(val('CmbModuleName')), screenIds: ids(tUn) })
                    .then(function (r) { box(r.message); return ShowData(); }).catch(fail);
            });
        }
        function deallocate4() {
            var t = num(val('CmbTemplateName'));
            if (t === 0) { box('Select Product Type First'); return; }
            if (ids(tAl).length === 0) { box("Checked Row's first To Un-Allocate Items"); return; }
            if (!ask("Are you sure to UnAllocate Item's?")) return;
            return busy('btnDeAllocate', function () {
                return postJson(API + '/tr/deallocate', { templateId: t, moduleId: num(val('CmbModuleName')), screenIds: ids(tAl) })
                    .then(function (r) { box(r.message); return ShowData(); }).catch(fail);
            });
        }
        function refresh4() {
            return getJson(API + '/tr/setup').then(function (r) {
                fill('CmbTemplateName', r.templates, 'Id', 'TemplateName', true);
                fill('CmbModuleName', r.modules, 'Id', 'ModuleDescription', true);
                tUn.clear(); tAl.clear();
                refreshCombos();
            }).catch(fail);
        }
        var SC4 = [['Ctrl+A', 'For Allocate'], ['Ctrl+D', 'For De-Allocate'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
                   ['Ctrl+F5', 'For Focus on ProductType'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                   ['Ctrl+ArrowRight', 'to change focus from one grid to another'], ['Ctrl+Enter', 'For Show'], ['Ctrl+Space', 'For Select/Unselect']];
        window.AP = {
            refresh: refresh4,
            show: function (btn) { return busy(btn, ShowData); },
            defineTemplate: function () { notBuilt('Template Name (OrganizationTemplates)'); },
            allScreens: function () { notBuilt('All Screens In Project (frmAllScreensInProject)'); },
            shortcuts: function () { shortcutBox('ShortCut Keys', SC4); },
            allocate: allocate4, deallocate: deallocate4
        };
        $id('grdPending').closest('.ap-scroll').addEventListener('focus', function () { whichGrid4 = 'un'; });
        $id('grdAllocated').closest('.ap-scroll').addEventListener('focus', function () { whichGrid4 = 'al'; });
        var lastTpl = '0';
        $id('CmbTemplateName').addEventListener('change', function () { if (val('CmbTemplateName') !== lastTpl) { lastTpl = val('CmbTemplateName'); ShowData(); } });
        bindKeys({
            close: closeForm, e: closeForm, r: refresh4, a: allocate4, d: deallocate4,
            f5: function () { focus('CmbTemplateName'); },
            enter: function () { busy('BtnShow', ShowData); },
            down: function () { focusGrid(whichGrid4 === 'un' ? 'grdAllocated' : 'grdPending'); },
            right: function () { focusGrid(whichGrid4 === 'un' ? 'grdAllocated' : 'grdPending'); },
            alt: function () { AP.shortcuts(); }
        });
        refresh4().then(function () { focus('CmbTemplateName'); });
    }

    // ==============================================================================================
    // 318 / 397 UserRightsEditing
    // ==============================================================================================
    if (SCREEN === 'UserRightsEditing') {
        var gRights = new Grid('grduserRights', {
            filter: true,
            groups: [{ key: 'ModuleDescription', caption: 'ModuleDescription', desc: true }, { key: 'ScreenAlias', caption: 'ScreenAlias' }],
            collapseGroups: true,
            cols: [{ key: 'ModuleID', hidden: true }, { key: 'ModuleDescription', hidden: true }, { key: 'ScreenID', hidden: true },
                   { key: 'ScreenName', hidden: true }, { key: 'ScreenAlias', hidden: true }, { key: 'RightsID', hidden: true },
                   { key: 'RightName', caption: 'Right', width: 296 },
                   { key: 'Value', caption: 'Active', width: 413, type: 'bool', edit: true, headerSelect: true }]
        });
        var lastUser = '0';
        function CompanyNameFill() {
            var u = num(val('CmbUserName'));
            return getJson(API + '/ur/companies?userId=' + u).then(function (rows) {
                if (rows && rows.length) { fill('CmbCompanyName', rows, 'CompanyId', 'CompName', false); refreshCombos(); }
            }).catch(fail);
        }
        function validation() {
            if (num(val('CmbUserName')) === 0) { box('User Name Field is Required'); return false; }
            if (num(val('CmbCompanyName')) === 0) { box('Company Field is Required'); return false; }
            return true;
        }
        /** BtnFetchRights_Click: the validation result is ignored, GetbyId runs regardless. */
        function fetchRights() {
            validation();
            return getJson(API + '/ur/rights?userId=' + num(val('CmbUserName')) + '&companyId=' + num(val('CmbCompanyName')))
                .then(function (rows) { gRights.load(rows || [], true); }).catch(fail);
        }
        function save18() {
            return busy('btnSave', function () {
                var rights = gRights.rows.map(function (r) { return { screenId: r.ScreenID, rightsId: r.RightsID, value: truthy(r.Value) }; });
                return postJson(API + '/ur/save', { userId: num(val('CmbUserName')), companyId: num(val('CmbCompanyName')), rights: rights })
                    .then(function (r) { box(r.message); }).catch(fail);
            });
        }
        window.AP = { fetchRights: function (b) { return busy(b, fetchRights); }, save: save18, shortcuts: function () { shortcutBox('ShortCut Keys', [['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+alt', 'To Show ShortCut Keys Form']]); } };
        $id('CmbUserName').addEventListener('change', function () { if (val('CmbUserName') !== lastUser) { lastUser = val('CmbUserName'); CompanyNameFill(); } });
        bindKeys({ close: closeForm, e: closeForm, u: save18, alt: function () { AP.shortcuts(); } });
        getJson(API + '/ur/setup').then(function (r) {
            fill('CmbUserName', r.users, 'Id', 'UserName', false);
            fill('CmbCompanyName', [], 'CompanyId', 'CompName', false);
            refreshCombos();
            focus('CmbUserName');
        }).catch(fail);
    }

    // ==============================================================================================
    // 312 DefineCompany
    // ==============================================================================================
    if (SCREEN === 'DefineCompany') {
        var recId = 0, logoData = '', hasLogo = false;
        var TEXTS = { code: 'txtCode', name: 'txtName', nameOtherLang: 'txtcompanyNameOtherLAng', reportingTitle: 'txtReportingTitle',
            email: 'txtEmail', email02: 'txtEmail02', phone: 'txtPhoneNumber', mobile01: 'txtMobile01', mobile02: 'txtMobile02',
            mobile03: 'txtMobile03', fax: 'txtCompanyFaxNo', address: 'txtAdress', addressOtherLang: 'txtAddressOtherLang',
            website: 'txtcompanyWebsite', country: 'txtCountry', state: 'txtState', city: 'txtCity', baseCurrency: 'txtBaseCurrency',
            contactPerson: 'txtContactPerson', firstName: 'txtFirstName', lastName: 'txtLastName', allowedUsers: 'txtUserAllowedCount',
            companyId: 'txtCompanyId' };
        var HC = ['OrganizationName', 'CompCode', 'CompName', 'CompContactPerson', 'FirstName', 'LastName', 'CompEmailA', 'CompEmailB', 'CompTel',
                  'CompMobileA', 'CompMobileB', 'CompMobileC', 'CompAddress', 'CompReportingTitle', 'CompCountry', 'CompState', 'CompBaseCurr',
                  'CompType', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'AllowedUserCount', 'IsHeadOffice', 'CompanyNameOtherLanguage',
                  'CompanyAddressOtherLanguage', 'CompanyWebsite', 'CompanyFaxNo', 'CityName', 'CompanyId'];
        var histCols = [{ key: 'Id', hidden: true }, { key: 'Edit', caption: 'Edit', type: 'button', btn: 'Edit', width: 40 },
                        { key: 'OrgCompanyTypeId', hidden: true }, { key: 'CompanyTypeId', hidden: true }, { key: 'CompanyTemplateId', hidden: true }];
        HC.forEach(function (k) { histCols.push({ key: k, caption: k }); });
        var gHist = new Grid('grdHistory', {
            auto: true, cols: histCols,
            onButton: function (k, r) { if (k === 'Edit') ReadById(r.Id); },
            onDblClick: function (r) { ReadById(r.Id); }
        });
        function HistoryGridFill() {
            return getJson(API + '/dc/history').then(function (rows) { gHist.load(rows || [], true); }).catch(fail);
        }
        function setLogo(url) {
            var img = $id('LogoPic');
            if (url) { img.src = url; img.style.display = ''; } else { img.removeAttribute('src'); img.style.display = 'none'; }
        }
        function setMode(update) {
            show('btnSave', !update); show('btnUpdate', update);
            $id('txtFirstName').disabled = update; $id('txtLastName').disabled = update;
        }
        /** Reset(): clears everything including the logo, refills the history, back to save mode. */
        function Reset() {
            recId = 0; logoData = ''; hasLogo = false;
            Object.keys(TEXTS).forEach(function (k) { $id(TEXTS[k]).value = ''; });
            setValue('CmbOrganizationName', 0); setValue('CmbTemplate', 0); refreshCombos();
            setLogo('');
            setMode(false);
            HistoryGridFill();
            focus('CmbOrganizationName');
        }
        function body() {
            var b = { recId: recId, organizationId: num(val('CmbOrganizationName')), templateId: num(val('CmbTemplate')),
                      logo: logoData, keepLogo: hasLogo && !logoData };
            Object.keys(TEXTS).forEach(function (k) { b[k] = $id(TEXTS[k]).value; });
            b.companyId = num(val('txtCompanyId'));
            return b;
        }
        function save12() {
            return busy('btnSave', function () {
                return postJson(API + '/dc/save', body()).then(function (r) { box(r.message); Reset(); }).catch(fail);
            });
        }
        function update12() {
            return busy('btnUpdate', function () {
                return postJson(API + '/dc/update', body()).then(function (r) { box(r.message); Reset(); }).catch(fail);
            });
        }
        /** ReadById(Id) - fills the form from Company.GetByID and switches to update mode. */
        function ReadById(id) {
            return getJson(API + '/dc/company?id=' + num(id)).then(function (c) {
                if (!c || !c.Id) return;
                recId = num(c.Id); logoData = '';
                setValue('CmbOrganizationName', c.OrgCompanyTypeId);
                setValue('CmbTemplate', c.CompanyTemplateId);
                var map = { code: 'CompCode', name: 'CompName', nameOtherLang: 'CompanyNameOtherLanguage', reportingTitle: 'CompReportingTitle',
                    email: 'CompEmailA', email02: 'CompEmailB', phone: 'CompTel', mobile01: 'CompMobileA', mobile02: 'CompMobileB',
                    mobile03: 'CompMobileC', fax: 'CompanyFaxNo', address: 'CompAddress', addressOtherLang: 'CompanyAddressOtherLanguage',
                    website: 'CompanyWebsite', country: 'CompCountry', state: 'CompState', city: 'CityName', baseCurrency: 'CompBaseCurr',
                    contactPerson: 'CompContactPerson', firstName: 'FirstName', lastName: 'LastName', allowedUsers: 'AllowedUserCount', companyId: 'CompanyId' };
                Object.keys(map).forEach(function (k) { $id(TEXTS[k]).value = str(c[map[k]]); });
                hasLogo = !!c.hasLogo;
                setLogo(hasLogo ? API + '/dc/logo?id=' + recId + '&t=' + Date.now() : '');
                refreshCombos();
                setMode(true);
            }).catch(fail);
        }
        /** CmbOrganizationName_Leave. */
        function organizationLeave() {
            var id = num(val('CmbOrganizationName'));
            return getJson(API + '/dc/organization?id=' + id).then(function (o) {
                if (o && o.OrgReportingTitle !== undefined) {
                    $id('txtReportingTitle').value = o.OrgReportingTitle; $id('txtState').value = o.OrgState; $id('txtCountry').value = o.OrgCountry;
                    $id('txtContactPerson').value = o.OrgContactPerson; $id('txtEmail').value = o.OrgEmailA; $id('txtPhoneNumber').value = o.OrgTel;
                    setValue('CmbTemplate', o.OrganizaionTemplateId); refreshCombos();
                } else {
                    ['txtReportingTitle', 'txtState', 'txtCountry', 'txtContactPerson', 'txtEmail', 'txtPhoneNumber'].forEach(function (x) { $id(x).value = ''; });
                }
            }).catch(fail);
        }
        function browse() { $id('fileLogo').click(); }
        $id('fileLogo').addEventListener('change', function () {
            var f = this.files && this.files[0]; if (!f) return;
            if (f.size > 5000000) { box('File Size Limit Exceeded'); this.value = ''; return; }
            var rd = new FileReader();
            rd.onload = function () { logoData = String(rd.result); setLogo(logoData); };
            rd.readAsDataURL(f);
        });
        $id('CmbOrganizationName').addEventListener('change', organizationLeave);
        var SC12 = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
                    ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Organization Code'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
                    ['Ctrl+ArrowUp', 'For Move Up'], ['Ctrl+ArrowDown', 'For Move Down'], ['Enter', 'For Move to Next Field'], ['Ctrl+Space', 'For Select/Unselect']];
        window.AP = {
            newForm: Reset, refresh: Reset, save: save12, update: update12, browse: browse,
            shortcuts: function () { shortcutBox('ShortCut Keys', SC12); },
            defineOrganization: function () { window.open('/admin/define-organization', '_blank'); },
            openTab: function (name) {
                var routes = { 'Module Allocate To Templates': '/admin/module-allocate-to-templates', 'Screen Allocate To Company': '/admin/screens-allocate-to-company', 'Define User': '/user-management/user-define' };
                if (routes[name]) window.open(routes[name], '_blank'); else notBuilt(name);
            }
        };
        enterAsTab();
        bindKeys({
            close: closeForm, e: closeForm, r: Reset, n: Reset,
            s: function () { if (visible('btnSave')) save12(); },
            u: function () { if (visible('btnUpdate')) update12(); },
            f5: function () { focus('txtCode'); },
            alt: function () { AP.shortcuts(); }
        });
        getJson(API + '/dc/setup').then(function (r) {
            fill('CmbOrganizationName', r.organizations, 'Id', 'OrgName', false);
            fill('CmbTemplate', r.templates, 'Id', 'TemplateName', false);
            gHist.load(r.history || [], true);
            setMode(false); setLogo('');
            refreshCombos();
            focus('CmbOrganizationName');
        }).catch(fail);
    }

    // ==============================================================================================
    // 791 frmScreenDefinetion (Screen Definition / Document Types / Define Methods)
    // ==============================================================================================
    if (SCREEN === 'frmScreenDefinetion') {
        var sdId = 0, allScreens = [], docId = 0, methId = 0, loginCompany = 0;
        var gSd = new Grid('grdScreenDefination', {
            filter: true,
            cols: [{ key: 'Id', hidden: true }, { key: 'ModuleId', hidden: true }, { key: 'TargetUrl', hidden: true }, { key: 'AppId', hidden: true },
                   { key: 'ScreenName', caption: 'ScreenName', width: 180, type: 'link' },
                   { key: 'ScreenAlias', caption: 'ScreenAlias', width: 200, type: 'edit' },
                   { key: 'SortNo', caption: 'SortNo', width: 50, type: 'edit' },
                   { key: 'Active', caption: 'Active', width: 60, type: 'bool', edit: true }],
            rowClass: function (r) { return truthy(r.Active) ? '' : 'ap-red'; },
            onDblClick: function (r) { RetrivedData(r.Id); }
        });
        var gRt = new Grid('grdScreenRights', {
            cols: [{ key: 'Id', hidden: true }, { key: 'RightsName', caption: 'RightsName', width: 190 }, { key: 'Value', caption: 'Value', type: 'bool', edit: true, headerSelect: true }]
        });
        var gApp = new Grid('grdApplications', {
            cols: [{ key: 'Id', hidden: true }, { key: 'MasterAppId', hidden: true }, { key: 'AppName', caption: 'AppName', width: 120 },
                   { key: 'MasterApp', caption: 'MasterApp', width: 70 }, { key: 'Value', caption: 'Value', type: 'bool', edit: true, headerSelect: true }]
        });
        var gDoc = new Grid('grdDocTypes', {
            auto: true, onDblClick: function (r) { ReadByIdDocType(r); },
            cols: ['Id', 'DocumentTypeCode', 'DocumentTypeDescription', 'DocumentTypeDescriptionOtherLing', 'ScreenName', 'TargetUrl', 'Remarks', 'EntryDate',
                   'EntryUserName', 'ModifyDate', 'ModifyUserName', 'ApprovedDate', 'ApprovedUserName'].map(function (k) {
                return { key: k, caption: k === 'DocumentTypeCode' ? 'Code' : (k === 'DocumentTypeDescription' ? 'Description' : (k === 'DocumentTypeDescriptionOtherLing' ? 'DescriptionOtherLing' : k)) };
            })
        });
        var gMeth = new Grid('grdMethods', {
            auto: true, onDblClick: function (r) { ReadByIdMethod(r); },
            cols: [{ key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'DocumentTypeCode', caption: 'DocumentTypeCode' },
                   { key: 'DocumentTypeDescription', caption: 'DocumentTypeDescription' }, { key: 'ScreenName', caption: 'ScreenName' },
                   { key: 'ReportTypeId', hidden: true }, { key: 'ReportMethod', caption: 'ReportMethod' }, { key: 'Remarks', caption: 'Remarks' }]
        });

        // ---- tabs
        function showTab(id) {
            ['pgScreen', 'pgDoc', 'pgMethods'].forEach(function (p) { $id(p).classList.toggle('is-active', p === id); });
            [['tabScreen', 'pgScreen'], ['tabDoc', 'pgDoc'], ['tabMethods', 'pgMethods']].forEach(function (t) { $id(t[0]).classList.toggle('is-active', t[1] === id); });
        }
        function revealTab(tab, page) { show(tab, true); showTab(page); }

        // ---- Screen Definition
        function ScreenDefinationGrd() {
            return getJson(API + '/sd/screens').then(function (rows) { allScreens = rows || []; gSd.load(allScreens, true); }).catch(fail);
        }
        function Reset() {
            sdId = 0;
            ['txtScreenAlias', 'txtSortNo', 'txtScreenName', 'txtTargetUrl'].forEach(function (x) { $id(x).value = ''; });
            gRt.rows.forEach(function (r) { r.Value = false; }); gRt.render();
            gApp.rows.forEach(function (r) { r.Value = false; }); gApp.render();
            show('btnSave', true); show('btnUpdate', false);
            focus('cmbModuleName');
        }
        function checkedRights() { return gRt.rows.filter(function (r) { return truthy(r.Value); }).map(function (r) { return r.RightsName; }); }
        function checkedApps() { return gApp.rows.filter(function (r) { return truthy(r.Value); }).map(function (r) { return r.Id; }); }
        function selectedCompanies() {
            var s = $id('CmbCompany'), out = [];
            for (var i = 0; i < s.options.length; i++) if (s.options[i].selected) out.push(num(s.options[i].value));
            return out;
        }
        function saveSd() {
            var btn = sdId > 0 ? 'btnUpdate' : 'btnSave';
            return busy(btn, function () {
                var chk = $id('chkNotValidateUrl');
                return postJson(API + '/sd/save', {
                    recId: sdId, moduleId: num(val('cmbModuleName')), screenName: val('txtScreenName'), screenAlias: val('txtScreenAlias'),
                    targetUrl: val('txtTargetUrl'), isActive: $id('ChkIsActive').checked, sortNo: val('txtSortNo'), appId: val('txtAppId'),
                    companyIds: selectedCompanies(), rights: checkedRights(), applications: checkedApps(),
                    notValidateUrl: chk.checked, notValidateVisible: !chk.closest('.is-hidden') && chk.offsetParent !== null
                }).then(function (r) { box(r.message); Reset(); return ScreenDefinationGrd(); }).catch(fail);
            });
        }
        /** RetrivedData(Id). */
        function RetrivedData(id) {
            return getJson(API + '/sd/by-id?id=' + num(id)).then(function (s) {
                if (!s || !s.Id) return;
                sdId = num(s.Id);
                setValue('cmbModuleName', s.ModuleId); refreshCombos();
                $id('txtScreenAlias').value = str(s.ScreenAlias); $id('txtSortNo').value = str(s.SortNo);
                $id('txtScreenName').value = str(s.ScreenName); $id('txtAppId').value = str(s.AppId);
                $id('txtTargetUrl').value = str(s.TargetUrl); $id('ChkIsActive').checked = truthy(s.IsActive);
                var rn = s.rights || [], an = s.applications || [];
                gRt.rows.forEach(function (r) { r.Value = rn.indexOf(r.RightsName) >= 0; }); gRt.render();
                gApp.rows.forEach(function (r) { r.Value = an.indexOf(num(r.Id)) >= 0; }); gApp.render();
                show('btnSave', false); show('btnUpdate', true);
            }).catch(fail);
        }
        /** cmbModuleName_Leave: the grid shows that module's screens; the old grid stays when none match. */
        function moduleLeave() {
            var m = num(val('cmbModuleName'));
            if (m <= 0) return;
            var rows = allScreens.filter(function (r) { return num(r.ModuleId) === m; });
            if (rows.length) gSd.load(rows, true);
        }
        function sorting() {
            return busy('btnSorting', function () {
                return postJson(API + '/sd/sorting', { rows: gSd.rows.map(function (r) { return { Id: r.Id, ScreenAlias: r.ScreenAlias, SortNo: r.SortNo, Active: truthy(r.Active) }; }) })
                    .then(function (r) { box(r.message); return ScreenDefinationGrd(); }).catch(fail);
            });
        }

        // ---- Document Types
        function DocTypeGrd() { return getJson(API + '/sd/document-types').then(function (rows) { gDoc.load(rows || [], true); fillMethodDocTypes(rows || []); }).catch(fail); }
        function ResetDoc() {
            docId = 0;
            ['txtDocTypeId', 'txtDocTypeCode', 'txtDocTypeDescription', 'txtDocTypeScreenName'].forEach(function (x) { $id(x).value = ''; });
            $id('txtDocTypeId').disabled = false;
            show('btnDocSave', true); show('btnDocUpdate', false);
            focus('txtDocTypeId');
        }
        function saveDoc(update) {
            if (!ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return busy(update ? 'btnDocUpdate' : 'btnDocSave', function () {
                return postJson(API + '/sd/document-type', {
                    recId: docId, id: val('txtDocTypeId'), code: val('txtDocTypeCode'), description: val('txtDocTypeDescription'),
                    otherLangDescription: val('txtDocTypeOtherLangDescription'), screenName: val('txtDocTypeScreenName'),
                    targetUrl: val('txtDocTypeTargetUrl'), remarks: val('txtDocTypeRemarks')
                }).then(function (r) { box(r.message); ResetDoc(); return DocTypeGrd(); }).catch(fail);
            });
        }
        /** ReadByIdDocType: fills from the grid row and disables the ID box. */
        function ReadByIdDocType(r) {
            docId = num(r.Id);
            $id('txtDocTypeId').value = str(r.Id); $id('txtDocTypeCode').value = str(r.DocumentTypeCode);
            $id('txtDocTypeDescription').value = str(r.DocumentTypeDescription);
            $id('txtDocTypeOtherLangDescription').value = str(r.DocumentTypeDescriptionOtherLing);
            $id('txtDocTypeScreenName').value = str(r.ScreenName); $id('txtDocTypeTargetUrl').value = str(r.TargetUrl);
            $id('txtDocTypeRemarks').value = str(r.Remarks);
            $id('txtDocTypeId').disabled = true;
            show('btnDocSave', false); show('btnDocUpdate', true);
        }

        // ---- Define Methods
        var docTypeRows = [];
        function fillMethodDocTypes(rows) {
            docTypeRows = rows;
            fill('CmbDocumentTypeMethodDefine', rows, 'Id', 'DocumentTypeDescription', true);
            refreshCombos();
        }
        function MethodsGrd() { return getJson(API + '/sd/methods').then(function (rows) { gMeth.load(rows || [], true); }).catch(fail); }
        function ResetMeth() {
            methId = 0;
            setValue('CmbDocumentTypeMethodDefine', 0); refreshCombos();
            ['txtScreenNameMethodDefine', 'txtReportMethod', 'txtRemarksMethodDefine'].forEach(function (x) { $id(x).value = ''; });
            show('btnMethSave', true); show('btnMethUpdate', false);
            focus('CmbDocumentTypeMethodDefine');
        }
        function saveMeth(update) {
            return busy(update ? 'btnMethUpdate' : 'btnMethSave', function () {
                return postJson(API + '/sd/method', {
                    recId: methId, documentTypeId: num(val('CmbDocumentTypeMethodDefine')), screenName: val('txtScreenNameMethodDefine'),
                    reportMethod: val('txtReportMethod'), remarks: val('txtRemarksMethodDefine')
                }).then(function (r) { box(r.message); ResetMeth(); return MethodsGrd(); }).catch(fail);
            });
        }
        function ReadByIdMethod(r) {
            methId = num(r.Id);
            setValue('CmbDocumentTypeMethodDefine', r.RefDocumentTypeId); refreshCombos();
            $id('txtScreenNameMethodDefine').value = str(r.ScreenName); $id('txtReportMethod').value = str(r.ReportMethod);
            $id('txtRemarksMethodDefine').value = str(r.Remarks);
            show('btnMethSave', false); show('btnMethUpdate', true);
        }

        var SC7 = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
                   ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+F5', 'For Focus on Module Comb'],
                   ['Ctrl+ArrowDown', 'For Move Down'], ['Ctrl+Space', 'For Select/Unselect']];
        function activePage() { return document.querySelector('.ap-page.is-active').id; }
        window.AP = {
            tab: showTab, newForm: Reset, refresh: function () { return ScreenDefinationGrd().then(function () { Reset(); }); },
            save: saveSd, sorting: sorting, shortcuts: function () { shortcutBox('ShortCut Keys', SC7); },
            moduleAdmin: function () { window.open('/admin/user-module-admin', '_blank'); },
            defineModule: function () { notBuilt('Define Module (frmDefineAppModules)'); },
            defineAppMenu: function () { notBuilt('Define AppMenu (frmAppMenu_ScreensDefine)'); },
            docNew: ResetDoc, docSave: function () { saveDoc(false); }, docUpdate: function () { saveDoc(true); },
            methNew: ResetMeth, methRefresh: function () { return MethodsGrd().then(ResetMeth); },
            methSave: function () { saveMeth(false); }, methUpdate: function () { saveMeth(true); }
        };
        enterAsTab();
        $id('cmbModuleName').addEventListener('change', moduleLeave);
        document.addEventListener('keydown', function (e) {
            if (e.ctrlKey && e.shiftKey && e.key === 'F10') { e.preventDefault(); revealTab('tabDoc', 'pgDoc'); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'F11') { e.preventDefault(); revealTab('tabMethods', 'pgMethods'); }
        });
        bindKeys({
            close: closeForm, e: closeForm, alt: function () { AP.shortcuts(); },
            n: function () { var p = activePage(); if (p === 'pgScreen') Reset(); else if (p === 'pgDoc') ResetDoc(); else ResetMeth(); },
            r: function () { var p = activePage(); if (p === 'pgScreen') AP.refresh(); else if (p === 'pgMethods') AP.methRefresh(); },
            s: function () {
                var p = activePage();
                if (p === 'pgScreen') { if (visible('btnSave')) saveSd(); }
                else if (p === 'pgDoc') { if (visible('btnDocSave')) saveDoc(false); }
                else if (visible('btnMethSave')) saveMeth(false);
            },
            u: function () {
                var p = activePage();
                if (p === 'pgScreen') { if (visible('btnUpdate')) saveSd(); }
                else if (p === 'pgDoc') { if (visible('btnDocUpdate')) saveDoc(true); }
                else if (visible('btnMethUpdate')) saveMeth(true);
            },
            f5: function () { focus('cmbModuleName'); },
            down: function () { focusGrid('grdScreenDefination'); },
            up: function () { focus('cmbModuleName'); }
        });
        getJson(API + '/sd/setup').then(function (r) {
            loginCompany = num(r.loginCompanyId);
            allScreens = r.screens || []; gSd.load(allScreens, true);
            fill('cmbModuleName', r.modules, 'Id', 'ModuleDescription', true);
            var cs = $id('CmbCompany'), h = '';
            (r.companies || []).forEach(function (c) { h += '<option value="' + esc(c.Id) + '"' + (num(c.Id) === loginCompany ? ' selected' : '') + '>' + esc(c.CompName) + '</option>'; });
            cs.innerHTML = h;
            gRt.load((r.rights || []).map(function (x) { return { Id: x.Id, RightsName: x.RightsName, Value: false }; }), true);
            gApp.load((r.applications || []).map(function (x) { return { Id: x.Id, AppName: x.AppName, MasterApp: x.MasterApp, MasterAppId: x.MasterAppId, Value: false }; }), true);
            gDoc.load(r.documentTypes || [], true);
            fillMethodDocTypes(r.documentTypes || []);
            gMeth.load(r.methods || [], true);
            show('btnUpdate', false); show('btnDocUpdate', false); show('btnMethUpdate', false);
            show('tabDoc', false); show('tabMethods', false);
            showTab('pgScreen');
            refreshCombos();
            focus('cmbModuleName');
        }).catch(fail);
    }
})();
