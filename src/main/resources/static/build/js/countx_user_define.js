/* ============================================================================================
 * countx_user_define.js - Admin Panel "User Rights"
 *
 *   body[data-screen="frmUserRights"]               Configurations.frmUserRights - the USER MASTER:
 *        Define User (+ ApplicationsAllocateToUser = BookingOffice.frmApplicationsAllocateToUser in the panel),
 *        User History
 *   body[data-screen="frmBranchesAllocationToUser"]  Lookups.frmBranchesAllocationToUser
 *
 * Each handler follows its desktop method: validation wording and order, the Yes/No confirmations, the
 * messages, what New / Save / Update / double-click do to the buttons and the grids. The server repeats every
 * check and builds the model itself; nothing here is trusted.
 * ============================================================================================ */
(function () {
    'use strict';

    var SCREEN = document.body.getAttribute('data-screen');
    var API = '/api/user-define';

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
        var wrap = e.closest ? e.parentNode && e.parentNode.querySelector('.dtcombo-input') : null;
        if (e.tagName === 'SELECT' && wrap) wrap.focus(); else e.focus();
    }
    /** BindDDL / BindDDLNew with ZeroIndex false: the rows, and no row active. */
    function fill(id, rows, valueKey, textKey) {
        var sel = $id(id); if (!sel) return;
        var html = '<option value="0"></option>';
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
    function comboText(id) { var s = $id(id); return (!s || s.selectedIndex < 0 || s.value === '0') ? '' : s.options[s.selectedIndex].textContent; }
    var toastTimer = null;
    function toast(m) {
        var t = $id('udToast'); if (!t) return;
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

    /** A checkbox-selector grid (ActAsSelector + UseHeaderSelector) with one text column. */
    function selectorGrid(tableId, rows, idKey, textKey, extra) {
        var body = document.querySelector('#' + tableId + ' tbody');
        body.innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="ud-center"><input type="checkbox" data-row="' + i + '"/></td><td>' + esc(r[textKey]) + '</td>' + (extra ? extra(r) : '') + '</tr>';
        }).join('');
        var all = document.querySelector('#' + tableId + ' thead input[data-all]');
        if (all) all.checked = false;
        body.setAttribute('data-count', rows.length);
        body._rows = rows;
    }
    function checkedRows(tableId) {
        var body = document.querySelector('#' + tableId + ' tbody');
        var rows = body._rows || [];
        var out = [];
        body.querySelectorAll('input[type=checkbox][data-row]').forEach(function (c) { if (c.checked) out.push(rows[num(c.getAttribute('data-row'))]); });
        return out;
    }
    function wireHeaderSelectors() {
        document.querySelectorAll('thead input[data-all]').forEach(function (h) {
            h.addEventListener('change', function () {
                document.querySelectorAll('#' + h.getAttribute('data-all') + ' tbody input[type=checkbox][data-row]').forEach(function (c) { c.checked = h.checked; });
            });
        });
    }

    // ==============================================================================================
    // frmUserRights
    // ==============================================================================================
    if (SCREEN === 'frmUserRights') {

        var SETUP = null;
        var RecId = 0;
        var USERS = [];
        var CUR_DEF = -1, CUR_HIST = -1;
        var LOC = [];                 /* dtCompanies: Id, CompanyId, Location, Active, UserStatus */
        var imageMode = 'none';       /* ImageNameOrPath: none ("") | keep (GetByID's name) | new (Browse) | clear (null) */
        var newImage = null;

        // ---------------------------------------------------------------- masks (MaskedTextBox)
        function applyMask(input) {
            var mask = input.getAttribute('data-mask');
            var digits = input.value.replace(/\D/g, '');
            var out = '', d = 0;
            for (var i = 0; i < mask.length && d < digits.length; i++) {
                if (mask[i] === '#') out += digits[d++];
                else out += mask[i];
            }
            input.value = out;
        }
        function setMasked(id, v) { var e = $id(id); e.value = str(v); applyMask(e); }
        document.querySelectorAll('.ud-mask').forEach(function (inp) {
            inp.setAttribute('placeholder', inp.getAttribute('data-mask').replace(/#/g, '_'));
            inp.setAttribute('maxlength', inp.getAttribute('data-mask').length);
            inp.addEventListener('input', function () { applyMask(inp); });
        });

        // ---------------------------------------------------------------- grids
        /* grdSettings :328 - ID / AppId / UserGroupName hidden, Title captioned "Role Name", widths 120 (Password 100,
           IsActive 60, AuthenticationEnabled 180); AuthenticationEnabled only with TwoWayAuthentication; DeviceDependency
           hidden only on the History grid when feature 16 is off (grduserdefine never hides it). */
        function columns(history) {
            var c = [
                ['UserName', 'User Name', 120], ['Password', 'Password', 100], ['FirstName', 'First Name', 120],
                ['LastName', 'Last Name', 120], ['FatherName', 'Father Name', 120], ['Title', 'Role Name', 120],
                ['WhatsApp', 'Whats App', 120], ['PhoneNumber', 'Phone Number', 120], ['CNIC', 'CNIC', 120],
                ['Email', 'Email', 120], ['IsActive', 'Is Active', 60]
            ];
            if (SETUP && SETUP.twoWayAuthentication) c.push(['AuthenticationEnabled', 'Authentication Enabled', 180]);
            if (!history || (SETUP && SETUP.deviceFeature)) c.push(['DeviceDependency', 'Device Dependency', 100]);
            return c;
        }
        function drawUsers(tableId, history) {
            var cols = columns(history);
            var t = $id(tableId);
            t.querySelector('thead tr').innerHTML = cols.map(function (c) { return '<th style="width:' + c[2] + 'px;">' + esc(c[1]) + '</th>'; }).join('');
            t.style.width = cols.reduce(function (a, c) { return a + c[2]; }, 0) + 'px';
            var cur = history ? CUR_HIST : CUR_DEF;
            t.querySelector('tbody').innerHTML = USERS.map(function (r, i) {
                return '<tr data-i="' + i + '"' + (i === cur ? ' class="is-current"' : '') + '>' + cols.map(function (c) {
                    var v = r[c[0]];
                    if (c[0] === 'IsActive') return '<td class="ud-center"><input type="checkbox" disabled' + (v ? ' checked' : '') + '/></td>';
                    return '<td title="' + esc(v) + '">' + esc(v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            var bar = $id(history ? 'barUser' : 'barUserDefine');
            bar.innerHTML = 'Record: <b>' + (USERS.length ? (cur >= 0 ? cur + 1 : 1) : 0) + '</b> Of <b>' + USERS.length + '</b>';
        }
        function drawAllUsers() { drawUsers('grduserdefine', false); drawUsers('grdUser', true); }
        function wireUserGrid(tableId, history) {
            var body = document.querySelector('#' + tableId + ' tbody');
            body.addEventListener('click', function (e) {
                var tr = e.target.closest('tr'); if (!tr) return;
                var i = num(tr.getAttribute('data-i'));
                if (history) CUR_HIST = i; else CUR_DEF = i;
                /* mark the row without re-rendering, so a double-click lands on the same element */
                body.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
                tr.classList.add('is-current');
                var bar = $id(history ? 'barUser' : 'barUserDefine');
                bar.innerHTML = 'Record: <b>' + (i + 1) + '</b> Of <b>' + USERS.length + '</b>';
            });
            if (!history) body.addEventListener('dblclick', function (e) {
                var tr = e.target.closest('tr'); if (!tr) return;
                CUR_DEF = num(tr.getAttribute('data-i'));
                grduserdefine_DoubleClick(USERS[CUR_DEF]);
            });
        }

        /* grdBranches - Location (250) | Active (check, header selector); Id / CompanyId / UserStatus hidden */
        function drawLocations() {
            $id('grdBranchesBody').innerHTML = LOC.map(function (r, i) {
                return '<tr data-i="' + i + '"><td>' + esc(r.Location) + '</td><td class="ud-center"><input type="checkbox" data-loc="' + i + '"' + (r.Active ? ' checked' : '') + '/></td></tr>';
            }).join('');
            $id('barBranches').innerHTML = 'Record: <b>' + (LOC.length ? 1 : 0) + '</b> Of <b>' + LOC.length + '</b>';
            $id('chkAllActive').checked = LOC.length > 0 && LOC.every(function (r) { return r.Active; });
        }
        $id('grdBranchesBody').addEventListener('change', function (e) {
            var c = e.target; if (!c.hasAttribute('data-loc')) return;
            LOC[num(c.getAttribute('data-loc'))].Active = c.checked;
        });
        $id('chkAllActive').addEventListener('change', function () {
            var on = this.checked; LOC.forEach(function (r) { r.Active = on; }); drawLocations();
        });
        /** LocationsBind :533 */
        function LocationsBind() {
            LOC = (SETUP.locations || []).map(function (r) { return { Id: 0, CompanyId: r.CompanyId, Location: r.Location, Active: false, UserStatus: false }; });
            drawLocations();
        }

        // ---------------------------------------------------------------- picture
        function setPicture(src) {
            var img = $id('ProfileImage');
            if (src) { img.src = src; img.classList.remove('is-hidden'); }
            else { img.removeAttribute('src'); img.classList.add('is-hidden'); }
        }

        // ---------------------------------------------------------------- load
        function bindUser() {
            return getJson(API + '/users').then(function (rows) {
                USERS = rows || []; CUR_DEF = -1; CUR_HIST = -1; drawAllUsers();
            });
        }
        function load() {
            return getJson(API + '/setup').then(function (s) {
                SETUP = s;
                show('BtnDeviceAllocation', !!s.deviceFeature);
                show('wrapDeviceDependency', !!s.deviceFeature);
                show('wrapEnableAuthentication', !!s.twoWayAuthentication);
                fill('CmbRoleName', s.roles || [], 'Id', 'RoleDescription');
                refreshCombos();
                USERS = s.users || []; drawAllUsers();
                LocationsBind();
                /* AddTabPage - the cost-centre tabs with App 5, CustomerPortalRegistration with App 4/6 */
                document.querySelectorAll('.ud-toptab[data-notported]').forEach(function (b) {
                    var t = b.getAttribute('data-tab');
                    var on = (t === 'tabCustomerPortalRegistration') ? s.customerPortal : s.supplierPortal;
                    b.classList.toggle('is-hidden', !on);
                    if (on) { b.disabled = true; b.title = b.getAttribute('data-notported') + ' is not ported yet.'; }
                });
                focus('txtUsername');
            }).catch(fail);
        }

        // ---------------------------------------------------------------- UserFromRefresh / New
        function UserFromRefresh() {
            ['txtUsername', 'txtPassword', 'txtFirstName', 'txtLastName', 'txtFatherName', 'txtWhatsAppNo', 'txtCellNo', 'txtEmail', 'txtCnicNo']
                .forEach(function (id) { $id(id).value = ''; });
            imageMode = 'none'; newImage = null; setPicture(null);
            $id('chkEnableAuthentication').checked = false;
            show('butnSave', true); show('butnUpdate', false);
            $id('chkboxIsActive').checked = false;
            $id('ChkDeviceDepenedency').checked = false;
            LocationsBind();
            focus('txtFirstName');
        }

        // ---------------------------------------------------------------- grduserdefine_DoubleClick :800
        function grduserdefine_DoubleClick(row) {
            if (!row) return;
            getJson(API + '/by-id?id=' + encodeURIComponent(row.ID)).then(function (ua) {
                show('butnSave', false); show('butnUpdate', true);
                RecId = ua.ID;
                $id('txtFirstName').value = str(ua.FirstName);
                $id('txtLastName').value = str(ua.LastName);
                $id('txtUsername').value = str(ua.UserName);
                $id('txtPassword').value = str(ua.Password);
                setValue('CmbRoleName', ua.UserRoleId); refreshCombos();
                $id('chkboxIsActive').checked = !!ua.IsActive;
                $id('chkEnableAuthentication').checked = !!ua.AuthenticationEnabledForUser;
                $id('ChkDeviceDepenedency').checked = !!ua.DeviceDependency;
                if (ua.hasProfile) {
                    $id('txtFatherName').value = str(ua.FatherName);
                    setMasked('txtWhatsAppNo', ua.WhatsApp);
                    setMasked('txtCellNo', ua.CellNo);
                    setMasked('txtCnicNo', ua.CNICNumber);
                    $id('txtEmail').value = str(ua.Email);
                    imageMode = 'keep'; newImage = null;
                    if (str(ua.ProfileImageFileName) !== '') setPicture(API + '/profile-image?id=' + encodeURIComponent(ua.ID) + '&t=' + Date.now());
                    else setPicture(null);
                }
                /* The desktop moves each allocated company to the end of dtCompanies as (Id, CompanyId, CompName, 1,
                   IsActive) - Active ticked. It works on whatever the grid already holds, so opening a second user
                   without New would carry the first user's allocation ids into the second user's save. The grid is
                   rebuilt first here (and the server refuses an allocation id that is not the user's own). */
                LocationsBind();
                (ua.allocations || []).forEach(function (a) {
                    for (var i = 0; i < LOC.length; i++) {
                        if (LOC[i].CompanyId === a.CompanyId && LOC[i].Id === 0) {
                            LOC.splice(i, 1);
                            LOC.push({ Id: a.Id, CompanyId: a.CompanyId, Location: a.CompName, Active: true, UserStatus: num(a.IsActive) === 1 });
                            break;
                        }
                    }
                });
                drawLocations();
                focus('txtUsername');
            }).catch(fail);
        }

        // ---------------------------------------------------------------- Insert() :589
        function FormValidation() {
            var checks = [
                ['txtUsername', 'UserName Field Required'], ['txtPassword', 'Password Field Required'],
                ['txtFirstName', 'First Name Field Required'], ['txtLastName', 'Last Name Field Required'],
                ['txtFatherName', 'Father Name Field Required']
            ];
            for (var i = 0; i < checks.length; i++) {
                var v = $id(checks[i][0]).value.trim();
                if (v === '' || v === '0') { box(checks[i][1]); focus(checks[i][0]); return false; }
            }
            if (num($id('CmbRoleName').value) === 0) { box('Role Name Field Required'); focus('CmbRoleName'); return false; }
            return true;
        }
        function Insert(btn, update) {
            if (!FormValidation()) return;
            if (!ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var body = {
                update: update, recId: update ? RecId : 0,
                userName: $id('txtUsername').value, password: $id('txtPassword').value,
                firstName: $id('txtFirstName').value, lastName: $id('txtLastName').value, fatherName: $id('txtFatherName').value,
                roleId: num($id('CmbRoleName').value),
                isActive: $id('chkboxIsActive').checked,
                enableAuthentication: $id('chkEnableAuthentication').checked,
                deviceDependency: $id('ChkDeviceDepenedency').checked,
                whatsApp: $id('txtWhatsAppNo').value, cellNo: $id('txtCellNo').value,
                cnic: $id('txtCnicNo').value, email: $id('txtEmail').value,
                locations: LOC.map(function (r) { return { id: r.Id, companyId: r.CompanyId, active: !!r.Active }; }),
                imageMode: imageMode,
                imageName: newImage ? newImage.name : '', imageBase64: newImage ? newImage.base64 : ''
            };
            return busy(btn, function () {
                return postJson(API + '/save', body).then(function (r) {
                    box(r.message);
                    UserFromRefresh();
                    return bindUser();
                }).catch(fail);
            });
        }

        // ---------------------------------------------------------------- tabs
        function selectTopTab(name) {
            document.querySelectorAll('.ud-toptab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === name); });
            var apps = name === 'tabApplicationsAllocateToUser';
            show('PanelUserDefine', !apps);
            show('PanelForViewForm', apps);
            if (apps) APPS.open();
        }
        document.querySelectorAll('.ud-toptab').forEach(function (b) {
            b.addEventListener('click', function () { if (!b.disabled) selectTopTab(b.getAttribute('data-tab')); });
        });
        document.querySelectorAll('.ud-bottomtab').forEach(function (b) {
            b.addEventListener('click', function () {
                var p = b.getAttribute('data-page');
                document.querySelectorAll('.ud-bottomtab').forEach(function (x) { x.classList.toggle('is-active', x === b); });
                show('tabPage3', p === 'tabPage3'); show('tabPage1', p === 'tabPage1');
            });
        });
        function appsActive() { return visible('PanelForViewForm') && visible('tabPage3'); }

        // ---------------------------------------------------------------- frmApplicationsAllocateToUser
        var APPS = (function () {
            var opened = false, companies = [], curCompany = -1;
            function UserNameFill() {
                var keep = num($id('CmbUserNameApps').value);
                return getJson(API + '/apps/users').then(function (rows) {
                    fill('CmbUserNameApps', rows || [], 'Id', 'Name');
                    if (keep > 0) setValue('CmbUserNameApps', keep);
                    refreshCombos();
                }).catch(fail);
            }
            function clearGrids() {
                selectorGrid('GridUnAllocated', [], 'Id', 'AppName');
                selectorGrid('GridAllocated', [], 'Id', 'AppName');
            }
            function drawCompanies() {
                document.querySelector('#grdCompany tbody').innerHTML = companies.map(function (r, i) {
                    return '<tr data-i="' + i + '"' + (i === curCompany ? ' class="is-current"' : '') + '><td>' + esc(r.CompanyName) + '</td><td>' + esc(r.UserName) + '</td></tr>';
                }).join('');
            }
            function selectionChanged() {
                var userId = num($id('CmbUserNameApps').value);
                var r = companies[curCompany];
                if (!r || userId <= 0) { clearGrids(); return Promise.resolve(); }
                var userName = comboText('CmbUserNameApps');
                return getJson(API + '/apps/grids?userId=' + userId + '&companyId=' + r.Id).then(function (g) {
                    selectorGrid('GridUnAllocated', g.unallocated || [], 'Id', 'AppName');
                    selectorGrid('GridAllocated', g.allocated || [], 'Id', 'AppName');
                    $id('labelUnAllocateGridTemplateII').textContent = "UnAllocated Apps Of User : '" + userName + "'\n For Company : " + r.CompanyName;
                    $id('labelAllocateGridTemplateII').textContent = "Allocated Apps Of User : '" + userName + "'\n For Company : " + r.CompanyName;
                }).catch(fail);
            }
            document.querySelector('#grdCompany tbody').addEventListener('click', function (e) {
                var tr = e.target.closest('tr'); if (!tr) return;
                curCompany = num(tr.getAttribute('data-i'));
                tr.parentNode.querySelectorAll('tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
                tr.classList.add('is-current');
                selectionChanged();
            });
            function save(btn, allocate) {
                var word = allocate ? 'Allocate' : 'Un-Allocate';
                var rows = checkedRows(allocate ? 'GridUnAllocated' : 'GridAllocated');
                if (rows.length === 0) { box("Check Row's Which You want To " + word); return; }
                if (!ask('Are you sure to ' + word + ' Users?')) return;
                var r0 = rows[0];
                return busy(btn, function () {
                    return postJson(API + '/apps/save', {
                        allocate: allocate, userId: r0.UserId, companyId: r0.CompanyId,
                        rows: rows.map(function (r) { return { id: r.Id, appId: r.AppId }; })
                    }).then(function (res) { box(res.message); return selectionChanged(); }).catch(fail);
                });
            }
            return {
                open: function () { if (!opened) { opened = true; clearGrids(); UserNameFill(); } },
                btnRefresh: function (btn) { return busy(btn, UserNameFill); },
                BtnShow: function (btn) {
                    var userId = num($id('CmbUserNameApps').value);
                    if (userId > 0) {
                        var userName = comboText('CmbUserNameApps');
                        return busy(btn, function () {
                            return getJson(API + '/apps/companies?userId=' + userId).then(function (rows) {
                                companies = (rows || []).map(function (c) { return { Id: c.Id, CompanyName: c.CompanyName, UserId: userId, UserName: userName }; });
                                curCompany = companies.length ? 0 : -1;
                                drawCompanies();
                                $id('label7').textContent = "Companies Of User : '" + userName + "'";
                                return selectionChanged();
                            }).catch(fail);
                        });
                    }
                    companies = []; curCompany = -1; drawCompanies(); clearGrids();
                },
                allocate: function (btn) { return save(btn, true); },
                unallocate: function (btn) { return save(btn, false); },
                BtnUserDefine: function () { window.open('/user-management/user-define', '_blank'); },
                shortcuts: function () {
                    box('Ctrl+A/Ctrl+S\tFor Press Allocate Button\nCtrl+U\tFor Press Update Button\nCtrl+D/Ctrl+Delete\tFor Press UnAllocate Button\n' +
                        'Ctrl+E\tFor Close\nCtrl+R\tFor Refresh\nCtrl+F5\tFor Focus on User Grid\nCtrl+alt\tTo Show ShortCut Keys Form\n' +
                        'Ctrl+ArrowRight\tto change focus from one grid to another');
                }
            };
        })();

        // ---------------------------------------------------------------- keyboard (frmUserRights_KeyDown :1024)
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) {
                var t = e.target;
                if (t && (t.tagName === 'INPUT' || t.classList.contains('dtcombo-input')) && t.type !== 'file' && t.type !== 'checkbox') {
                    if (t.closest && t.closest('.dtcombo-pop')) return;
                    e.preventDefault(); nextField(t); return;
                }
            }
            if (appsActive() && e.ctrlKey) {
                var k = e.key.toLowerCase();
                if (k === 'a' || k === 's') { e.preventDefault(); APPS.allocate($id('BtnSaveApps')); return; }
                if (k === 'd' || e.key === 'Delete') { e.preventDefault(); APPS.unallocate($id('btnDeleteApps')); return; }
                if (k === 'r') { e.preventDefault(); APPS.btnRefresh($id('btnRefreshApps')); return; }
            }
            if (e.ctrlKey && (e.key === 's' || e.key === 'S')) { e.preventDefault(); UD.btnsave($id('butnSave')); return; }
            if (e.ctrlKey && (e.key === 'n' || e.key === 'N')) { e.preventDefault(); UD.btnnew(); return; }
            /* Ctrl+U tests toolStrip1's btnUpdate, which is never visible - so it never fires on the desktop either. */
            if ((e.ctrlKey && (e.key === 'e' || e.key === 'E')) || e.key === 'Escape') {
                if (document.querySelector('.dtcombo-pop[style*="block"]')) return;
                e.preventDefault(); closeForm();
            }
        });

        // ---------------------------------------------------------------- public handlers
        window.UD = {
            btnnew: function () { UserFromRefresh(); },
            btnsave: function (btn) { RecId = 0; return Insert(btn, false); },
            btnUpdate: function (btn) {
                if (RecId === 0) { box('Id not found'); return; }
                return Insert(btn, true);
            },
            BtnUser: function () { window.open('/user-management/branches-allocation-to-user', '_blank'); },
            browse: function () { $id('fileBrowse').click(); },
            btnClearImage: function () { imageMode = 'clear'; newImage = null; setPicture(null); },
            apps: APPS
        };
        $id('fileBrowse').addEventListener('change', function () {
            var f = this.files && this.files[0];
            this.value = '';
            if (!f) return;
            if (!/\.(jpe?g|png)$/i.test(f.name)) { box('Only *.jpg, *.jpeg and *.png pictures can be chosen.'); return; }
            if (f.size > 5 * 1024 * 1024) { box('File Size Exceeds 5MB Of File: '); return; }
            var reader = new FileReader();
            reader.onload = function () {
                var dataUrl = String(reader.result);
                newImage = { name: f.name, base64: dataUrl.substring(dataUrl.indexOf(',') + 1) };
                imageMode = 'new';
                setPicture(dataUrl);
            };
            reader.onerror = function () { box('The picture could not be read.'); };
            reader.readAsDataURL(f);
        });

        wireUserGrid('grduserdefine', false);
        wireUserGrid('grdUser', true);
        wireHeaderSelectors();
        load();
    }

    // ==============================================================================================
    // frmBranchesAllocationToUser
    // ==============================================================================================
    if (SCREEN === 'frmBranchesAllocationToUser') {

        function UserFill() {
            var keep = num($id('CmbUserName').value);
            return getJson(API + '/branches/users').then(function (rows) {
                /* BindDDLNew(..., ZeroIndex true) */
                var sel = $id('CmbUserName');
                var html = '<option value="0">...Select Any Value...</option>';
                (rows || []).forEach(function (r) { html += '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>'; });
                sel.innerHTML = html; sel.value = '0';
                if (keep > 0) setValue('CmbUserName', keep);
                refreshCombos();
            }).catch(fail);
        }
        function ShowData() {
            var userId = num($id('CmbUserName').value);
            if (userId <= 0) { box('Select Branch Name First...'); return Promise.resolve(); }
            return getJson(API + '/branches/grids?userId=' + userId).then(function (g) {
                selectorGrid('GridUnAllocatedBranchs', g.unallocated || [], 'Id', 'Name');
                selectorGrid('GridAllocatedBranchs', g.allocated || [], 'Id', 'Name');
            }).catch(fail);
        }
        /* CmbUserName.Leave -> ShowData */
        var lastShown = null;
        document.addEventListener('focusout', function (e) {
            var wrap = $id('CmbUserName').parentNode;
            if (!wrap || !wrap.contains(e.target)) return;
            setTimeout(function () {
                if (wrap.contains(document.activeElement)) return;
                if (document.activeElement === $id('btnshow')) return;
                var v = $id('CmbUserName').value;
                if (v === lastShown) return;
                lastShown = v; ShowData();
            }, 0);
        });
        function allocate(btn) {
            var userId = num($id('CmbUserName').value);
            if (userId === 0) { box('Select Branch First'); focus('CmbUserName'); return; }
            var rows = checkedRows('GridUnAllocatedBranchs');
            if (rows.length === 0) { box("Checked Row's first To Allocate Branch"); return; }
            if (!ask('Are you sure to Allocate Branch?')) return;
            return busy(btn, function () {
                return postJson(API + '/branches/allocate', { userId: userId, branchIds: rows.map(function (r) { return r.Id; }) })
                    .then(function (r) { box(r.message); return ShowData(); }).catch(fail);
            });
        }
        function deallocate(btn) {
            var userId = num($id('CmbUserName').value);
            if (userId === 0) { box('Select Branch First'); focus('CmbUserName'); return; }
            var rows = checkedRows('GridAllocatedBranchs');
            if (rows.length === 0) { box("Checked Row's first To Un-Allocate Branch"); return; }
            if (!ask('Are you sure to UnAllocate Branch?')) return;
            return busy(btn, function () {
                return postJson(API + '/branches/deallocate', { userId: userId, branchIds: rows.map(function (r) { return r.Id; }) })
                    .then(function (r) { box(r.message); return ShowData(); }).catch(fail);
            });
        }
        window.UB = {
            btnRefresh: function (btn) { return busy(btn, UserFill); },
            btnshow: function (btn) { lastShown = $id('CmbUserName').value; return busy(btn, ShowData); },
            BtnUser: function () { window.open('/user-management/user-define', '_blank'); },
            allocate: allocate,
            deallocate: deallocate,
            shortcuts: function () {
                box('Ctrl+A\tFor Press Allocate Button to Allocate Branch\nCtrl+D\tFor Press UnAllocate Button to UnAllocate Branch\n' +
                    'Ctrl+E\tFor Close\nCtrl+R\tFor Refresh\nCtrl+F5\tFor Focus on Branch\nCtrl+alt\tTo Show ShortCut Keys Form\n' +
                    'Ctrl+ArrowRight\tto change focus from one grid to another');
            }
        };
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) { if (e.key === 'Escape' && !document.querySelector('.dtcombo-pop[style*="block"]')) closeForm(); return; }
            var k = e.key.toLowerCase();
            if (k === 'r') { e.preventDefault(); UB.btnRefresh($id('btnRefresh')); }
            else if (k === 'a') { e.preventDefault(); allocate($id('BtnAllocateItems')); }
            else if (k === 'd') { e.preventDefault(); deallocate($id('btnDeAllocate')); }
            else if (k === 'e') { e.preventDefault(); closeForm(); }
            else if (e.key === 'F5') { e.preventDefault(); focus('CmbUserName'); }
        });
        wireHeaderSelectors();
        selectorGrid('GridUnAllocatedBranchs', [], 'Id', 'Name');
        selectorGrid('GridAllocatedBranchs', [], 'Id', 'Name');
        UserFill().then(function () { focus('CmbUserName'); });
    }
})();
