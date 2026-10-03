/* Admin Panel -> Account Movement = Architecture.WinApp.AccountToAccountTransfer, handler for handler.
 *   AcfrmAcAllocation_Load          -> ThirdLevelAccountsFill + AccountFillFromGlobal      (GET load)
 *   btnRefresh_Click / Ctrl+R       -> GlobalServicesDbCall(AccountsWithCustomGroupId), AccountFillFromGlobal, ThirdLevelAccountsFill
 *   AcfrmAcAllocation_KeyDown       -> Ctrl+R refresh; Ctrl+E or Esc close
 *   CmbParentAccountToFilter_Leave  -> AccountFillFromGlobal(parent > 0 ? [parent] : null)
 *   CmbDetailAccount_Leave          -> (Merge checked) refill CmbToChartOfAccount by the From account's class / type
 *   chkAccountTransactionsMerge_CheckedChanged -> swap 3rd Level Accounts <-> To Account Title
 *   btnUpdate_Click                 -> checks, VoucherHead.ChartOfAccountParentUpdate (POST update)
 * clsGlobalVariables.AllAccountsWithCustomGroupId is the page-level `ALL` list, as on the desktop (filled once, refreshed by Refresh). */
(function () {
    'use strict';
    var API = '/api/admin/account-movement';
    var ALL = [];          /* clsGlobalVariables.AllAccountsWithCustomGroupId (distinct by Id) */
    var busy = {};

    function $(id) { return document.getElementById(id); }
    var cmbParent = $('CmbParentAccountToFilter'), cmbDetail = $('CmbDetailAccount'),
        cmbThird = $('CmbThirdLevelAccounts'), cmbTo = $('CmbToChartOfAccount'), chk = $('chkAccountTransactionsMerge');

    // ------------------------------------------------------------------ MessageBox.Show
    var msgQueue = [], msgOpen = false, msgAfter = null;
    function msg(text, after) {
        msgQueue.push({ text: String(text == null ? '' : text), after: after });
        if (!msgOpen) nextMsg();
    }
    function nextMsg() {
        var m = msgQueue.shift();
        if (!m) { msgOpen = false; return; }
        msgOpen = true; msgAfter = m.after;
        $('amMsgText').textContent = m.text;
        $('amMsg').classList.add('is-on');
        setTimeout(function () { $('amMsgOk').focus(); }, 0);
    }
    function closeMsg() {
        $('amMsg').classList.remove('is-on');
        var f = msgAfter; msgAfter = null;
        if (typeof f === 'function') { try { f(); } catch (e) { /* ignore */ } }
        nextMsg();
    }
    $('amMsgOk').addEventListener('click', closeMsg);
    $('amMsg').addEventListener('keydown', function (e) {
        if (e.key === 'Enter' || e.key === 'Escape' || e.key === ' ') { e.preventDefault(); e.stopPropagation(); closeMsg(); }
    });

    // ------------------------------------------------------------------ http
    function csrf() {
        var t = document.querySelector('meta[name="_csrf"]'), h = document.querySelector('meta[name="_csrf_header"]');
        var o = {};
        if (t && h && t.content && h.content) o[h.content] = t.content;
        return o;
    }
    function call(method, path, body) {
        var headers = csrf();
        var opt = { method: method, credentials: 'same-origin', headers: headers };
        if (body !== undefined) { headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(API + path, opt).then(function (r) {
            return r.text().then(function (t) {
                var j = null; try { j = t ? JSON.parse(t) : null; } catch (e) { j = null; }
                if (!r.ok) throw new Error((j && j.message) || ('HTTP ' + r.status));
                return j;
            });
        });
    }

    /* every server button: disabled at once + spinner, duplicates refused, re-enabled on success and failure */
    function run(btn, key, work) {
        if (busy[key]) return Promise.resolve();
        busy[key] = true;
        var icon = btn ? btn.querySelector('.fa') : null, oldCls = icon ? icon.className : null;
        if (btn) btn.disabled = true;
        if (icon) icon.className = 'am-spin';
        return Promise.resolve().then(work).catch(function (e) { msg(e && e.message ? e.message : e); })
            .then(function () { busy[key] = false; if (btn) btn.disabled = false; if (icon) icon.className = oldCls; });
    }

    // ------------------------------------------------------------------ combos
    function val(sel) { var v = parseInt(sel.value, 10); return isNaN(v) ? 0 : v; }
    function selectedRow(sel) { var o = sel.selectedIndex >= 0 ? sel.options[sel.selectedIndex] : null; return o ? o.__row || null : null; }
    function opt(value, text, extras, row) {
        var o = document.createElement('option');
        o.value = String(value); o.textContent = text == null ? '' : String(text);
        o.setAttribute('data-extra', extras.map(function (x) { return x == null ? '' : String(x); }).join('|'));
        o.__row = row || null;
        return o;
    }
    function fire(sel) { try { sel.dispatchEvent(new Event('change', { bubbles: true })); } catch (e) { /* old browsers */ } }

    /* InfragisticsHelper.BindAndRetainSelection(..., AllColumns:true, previousValue, insertDefaultRow:true):
       "-- Select --" (0) first; the previous value kept when it is still in the list, else row 0 activated. */
    function bindAccounts(sel, rows, previous) {
        sel.innerHTML = '';
        sel.appendChild(opt(0, '-- Select --', ['', '', '', ''], { Id: 0, AccountClassId: 0, AccountTypeId: 0 }));
        rows.forEach(function (a) {
            sel.appendChild(opt(a.Id, a.AccountTitle, [a.AccountCode, a.AccountClass, a.AccountType, a.ParentAccountTitle], a));
        });
        var keep = previous != null && rows.some(function (a) { return String(a.Id) === String(previous); });
        sel.value = keep ? String(previous) : '0';
        fire(sel);
    }
    /* DropDownBind.BindDDL(dt, combo, "Id", "AccountTitle", caption, ZeroIndex:false): no default row, nothing set. */
    function bindThird(sel, rows, previous) {
        sel.innerHTML = '';
        rows.forEach(function (a) {
            sel.appendChild(opt(a.Id, a.AccountTitle, [a.AccountCode, a.AccountType, a.AccountClass, a.ParentAccount], a));
        });
        var keep = previous && rows.some(function (a) { return String(a.Id) === String(previous); });
        if (keep) sel.value = String(previous); else sel.selectedIndex = -1;
        fire(sel);
    }

    function distinct(list) {
        var seen = {}, out = [];
        (list || []).forEach(function (a) { if (!seen[a.Id]) { seen[a.Id] = 1; out.push(a); } });
        return out;
    }

    /* AccountFillFromGlobal(int[] parentAccountIds = null) */
    function accountFillFromGlobal(parentIds) {
        var rows = ALL;
        if (parentIds && parentIds.length) rows = rows.filter(function (a) { return parentIds.indexOf(a.ParentAccountId) >= 0; });
        bindAccounts(cmbDetail, distinct(rows), cmbDetail.selectedIndex >= 0 ? val(cmbDetail) : null);
    }

    /* CmbParentAccountToFilter_Leave */
    function parentLeave() {
        var p = val(cmbParent);
        accountFillFromGlobal(p <= 0 ? null : [p]);
    }

    /* CmbDetailAccount_Leave */
    function detailLeave() {
        if (!chk.checked) return;
        var from = selectedRow(cmbDetail);
        if (!from) { msg('Object reference not set to an instance of an object.'); return; }  /* SelectedRow null */
        var cls = from.AccountClassId || 0, typ = from.AccountTypeId || 0, all = ALL;
        if (cls === 4) all = all.filter(function (a) { return a.AccountClassId === 4; });
        else if (cls === 5) all = all.filter(function (a) { return a.AccountClassId === 5; });
        else if (cls === 2 && (typ === 2 || typ === 15)) all = all.filter(function (a) { return a.AccountTypeId === 2 || a.AccountTypeId === 15; });
        else {
            all = all.filter(function (a) { return a.AccountClassId !== 4 && a.AccountClassId !== 5; });
            all = all.filter(function (a) { return a.AccountTypeId !== 2 && a.AccountTypeId !== 15; });
        }
        bindAccounts(cmbTo, distinct(all), cmbTo.selectedIndex >= 0 ? val(cmbTo) : null);
    }

    /* chkAccountTransactionsMerge_CheckedChanged */
    function mergeChanged() {
        var m = chk.checked;
        $('lblThirdLevelAccount').classList.toggle('am-hidden', m);
        $('boxThirdLevel').classList.toggle('am-hidden', m);
        $('lblToAccountTitle').classList.toggle('am-hidden', !m);
        $('boxToAccount').classList.toggle('am-hidden', !m);
    }

    /* UltraCombo Leave: focus leaves the combo (its wrap / popup) */
    function onLeave(sel, handler) {
        var wrap = sel.closest('.dtcombo-wrap') || sel;
        wrap.addEventListener('focusout', function (e) {
            if (e.relatedTarget && wrap.contains(e.relatedTarget)) return;
            setTimeout(function () {
                if (wrap.contains(document.activeElement)) return;
                handler();
            }, 0);
        });
    }

    function focusCombo(sel) {
        var wrap = sel.closest('.dtcombo-wrap');
        var inp = wrap ? wrap.querySelector('.dtcombo-input') : sel;
        if (inp) inp.focus();
    }

    // ------------------------------------------------------------------ Load / Refresh / Update
    function load() {
        return call('GET', '/load').then(function (d) {
            bindThird(cmbParent, d.thirdLevel || [], null);       /* ThirdLevelAccountsFill */
            bindThird(cmbThird, d.thirdLevel || [], null);
            ALL = d.accounts || [];                                /* AccountFillFromGlobal */
            accountFillFromGlobal(null);
        });
    }

    /* btnRefresh_Click: global accounts re-read, AccountFillFromGlobal(), ThirdLevelAccountsFill() */
    function refresh() {
        return run($('btnRefresh'), 'refresh', function () {
            return call('GET', '/accounts').then(function (rows) {
                ALL = rows || [];
                accountFillFromGlobal(null);
                return call('GET', '/third-level');
            }).then(function (rows) {
                bindThird(cmbParent, rows || [], val(cmbParent));
                bindThird(cmbThird, rows || [], val(cmbThird));
            });
        });
    }

    function fail(text, focusSel) { msg(text, function () { if (focusSel) focusCombo(focusSel); }); }

    /* btnUpdate_Click */
    function update() {
        if (busy.update) return;
        if (cmbDetail.selectedIndex < 0 || val(cmbDetail) === 0) { fail('Detail Account field is required', cmbDetail); return; }
        if (chk.checked) {
            if (cmbTo.selectedIndex < 0 || val(cmbTo) === 0) { fail('To Account Title field is required', cmbTo); return; }
            var f = selectedRow(cmbDetail) || {}, t = selectedRow(cmbTo) || {};
            var fc = f.AccountClassId || 0, tc = t.AccountClassId || 0, ft = f.AccountTypeId || 0, tt = t.AccountTypeId || 0;
            if (fc === 4 && tc !== 4) { focusCombo(cmbTo); msg('Expense Class Account Transactions can be transfer to only Expense Class Accounts'); return; }
            if (fc === 5 && tc !== 5) { focusCombo(cmbTo); msg('Revenue Class Account Transactions can be transfer to only Revenue Class Accounts'); return; }
            if ((ft === 2 || ft === 15) && tt !== 2 && tt !== 15) {
                focusCombo(cmbTo);
                msg((ft === 2 ? 'Cash Equivalent' : 'Bank Equivalent') + ' account transactions can only be transferred to Cash Equivalent or Bank Equivalent accounts.');
                return;
            }
        } else if (cmbThird.selectedIndex < 0 || val(cmbThird) === 0) { fail('Third Level Account field is required', cmbThird); return; }

        var body = { detailAccountId: val(cmbDetail), merge: chk.checked, thirdLevelAccountId: val(cmbThird), toAccountId: val(cmbTo) };
        return run($('btnUpdate'), 'update', function () {
            return call('POST', '/update', body).then(function (r) {
                msg((r && r.message) || 'Parent Account Change Successfully');
                cmbDetail.selectedIndex = -1;          /* CmbDetailAccount.Text = string.Empty */
                fire(cmbDetail);
            });
        });
    }

    function close() {
        if (document.referrer && history.length > 1) history.back();
        else location.href = '/';
    }

    // ------------------------------------------------------------------ wiring
    function wire() {
        if (window.DesktopCombo) window.DesktopCombo.refresh();
        $('btnRefresh').addEventListener('click', refresh);
        $('btnUpdate').addEventListener('click', update);
        chk.addEventListener('change', mergeChanged);
        onLeave(cmbParent, parentLeave);
        onLeave(cmbDetail, detailLeave);
        /* KeyPreview: AcfrmAcAllocation_KeyDown */
        document.addEventListener('keydown', function (e) {
            if ($('amMsg').classList.contains('is-on')) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if ((e.ctrlKey && k === 'e') || (k === 'escape' && !document.querySelector('.dtcombo-pop[style*="block"]'))) { e.preventDefault(); close(); }
        }, true);   /* capture: an open combo popup is seen before its own Esc closes it */
        mergeChanged();
        run(null, 'load', load);
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', wire); else wire();
}());
