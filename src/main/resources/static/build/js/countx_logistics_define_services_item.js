/* ============================================================================================
 * countx_logistics_define_services_item.js - 234 "Define Services Item's"
 * (Architecture.WinApp.Service/EximServicesDefine.cs). Built on countx_hrm.js (window.HRM).
 *
 * Each handler follows the form: InitializeComponentMethod (load), cmbItemCategory_Leave, FormReset,
 * btnnew_Click, toolStripButton1_Click (Refresh), Insert (Save / Update), ReadById (history double-click),
 * HistoryGridFill (Search) and InvDefrmAddItem_KeyDown. The server repeats every check.
 * ============================================================================================ */
(function () {
    'use strict';

    var API = '/api/logistics/define-services-item';
    var P = {};
    window.LgsSI = P;

    var RecId = 0, rights = {}, FormInitialized = false;

    // ------------------------------------------------------------------------ combos
    function fillAttr(id, rows, valueKey, textKey, zero, attrs) {
        var sel = HRM.$(id); if (!sel) return;
        var keep = sel.value;
        var html = zero === false ? '' : '<option value="0">' + HRM.esc(zero || '') + '</option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            Object.keys(attrs || {}).forEach(function (a) { extra += ' data-' + a + '="' + HRM.esc(HRM.col(r, attrs[a])) + '"'; });
            html += '<option value="' + HRM.esc(HRM.col(r, valueKey)) + '"' + extra + '>' + HRM.esc(HRM.col(r, textKey)) + '</option>';
        });
        sel.innerHTML = html;
        sel._rows = rows || [];
        if (keep && HRM.hasOption(id, keep)) sel.value = keep; else if (zero !== false) sel.value = '0'; else sel.selectedIndex = sel.options.length ? 0 : -1;
        HRM.refreshCombos();
    }
    /** CatagoryBind / AccountlstDtFillFromGlobal / ItemGroupNameBind / MasterItemBind - BindAndRetainSelection(insertDefaultRow: true). */
    function bindLists(d) {
        fillAttr('cmbItemCategory', d.categories, 'Id', 'CategoryDescription', '');
        fillAttr('cmbitemcathistory', d.categories, 'Id', 'CategoryDescription', '');
        fillAttr('cmbPurchaseGL', d.accounts, 'Id', 'AccountTitle', '', { code: 'AccountCode' });
        fillAttr('CmbUomGroup', d.groups, 'Id', 'GroupName', '');
        fillAttr('CmbMasterItem', d.masterItems, 'Id', 'LookupName', '');
    }

    // ------------------------------------------------------------------------ grids
    var grdAllocation = new HRM.Grid('grdAllocation', {
        columns: [                                                     // CompaniesBindInGrid
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'Location', caption: 'Location', width: 250 },
            { key: 'Value', caption: 'Value', type: 'edit-check' }     // CheckBox, UseHeaderSelector
        ],
        checkAll: 'Value'
    });

    /** "dd-MM-yyyy hh:mm tt" */
    function fmtStamp(v) {
        if (!v) return '';
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'), t = HRM.time(v) || '00:00', h = +t.slice(0, 2), m = t.slice(3, 5);
        var ap = h >= 12 ? 'PM' : 'AM', h12 = h % 12 === 0 ? 12 : h % 12;
        return p[2] + '-' + p[1] + '-' + p[0] + ' ' + String(h12).padStart(2, '0') + ':' + m + ' ' + ap;
    }
    function stampCol(key) { return { key: key, caption: key, render: function (v) { return HRM.esc(fmtStamp(v)); } }; }

    var grdhistory = new HRM.Grid('grdhistory', {
        columns: [                                                     // HistoryGridFill dt + gridsetting()
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'Category', caption: 'Category' },
            { key: 'ServiceCode', caption: 'ServiceCode' },
            { key: 'ServiceName', caption: 'ServiceName', width: 220 },
            { key: 'GlAccount', caption: 'Services GL A/C' },
            { key: 'MasterItem', caption: 'MasterItem', hidden: true },   // grouped by (Groups.Add("MasterItem"))
            { key: 'Active', caption: 'Active', type: 'check' },
            { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'EntryDate', caption: 'EntryDate', type: 'date' },     // ToShortDateString
            { key: 'ModifyUser', caption: 'ModifyUser' },
            stampCol('ModifyDate'),
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }
        ],
        filterRow: true, emptyText: '',
        rowClass: function (r) { return r.__group ? 'lgsa-group' : ''; },
        onDouble: function (r) { if (!r.__group) grdhistory_DoubleClick(r); },
        onCode: function (r) { if (!r.__group) HRM.box('Attachments are not ported on the web (' + HRM.int(r.NoOfAttachments) + ' on the desktop).'); }
    });
    /** Groups.Add("MasterItem"): rows ordered by master item, one caption row per group. */
    function groupRows(rows) {
        var groups = {}, order = [];
        (rows || []).forEach(function (r) {
            var g = HRM.str(r.MasterItem);
            if (!groups[g]) { groups[g] = []; order.push(g); }
            groups[g].push(r);
        });
        order.sort(function (a, b) { return a.localeCompare(b); });
        var out = [];
        order.forEach(function (g) {
            out.push({ __group: true, Category: 'MasterItem: ' + g + ' (' + groups[g].length + ')' });
            out.push.apply(out, groups[g]);
        });
        return out;
    }

    // ------------------------------------------------------------------------ form
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }

    /** cmbItemCategory_Leave: GenerateCode -> txtItemCode, GetGLAccountbyItemCategoryId -> cmbPurchaseGL. */
    var leaveSeq = 0;
    function cmbItemCategory_Leave() {
        var seq = ++leaveSeq;
        return HRM.get(API + '/category', { id: HRM.comboVal('cmbItemCategory') }).then(function (d) {
            if (seq !== leaveSeq) return;
            if (d && d.ItemCode !== undefined) HRM.setVal('txtItemCode', d.ItemCode);
            if (d && d.InventoryAccountId !== undefined) HRM.setCombo('cmbPurchaseGL', d.InventoryAccountId);
        }).catch(function () { HRM.box('Record Not Found'); });
    }

    /** FormReset(). */
    function FormReset() {
        RecId = 0;
        buttons(false);
        HRM.focus('cmbItemCategory');
        HRM.setVal('txtItemCode', '');
        HRM.setVal('txtItemName', '');
        HRM.setCombo('cmbPurchaseGL', 0);
        return HRM.get(API + '/allocation').then(function (rows) { grdAllocation.set(rows || []); })
            .catch(HRM.fail)
            .then(cmbItemCategory_Leave);
    }

    function Validation() {
        if (!grdAllocation.rows().length) { HRM.box('Company Grid Record Not Found'); return false; }
        if (!HRM.comboVal('cmbItemCategory')) { HRM.box('Services Category field is required'); HRM.focus('cmbItemCategory'); return false; }
        if (!HRM.val('txtItemName').trim()) { HRM.box('Services Name field is required'); HRM.focus('txtItemName'); return false; }
        if (!HRM.comboVal('cmbPurchaseGL')) { HRM.box('Services GL A/C field is required'); HRM.focus('cmbPurchaseGL'); return false; }
        if (RecId === 0 && !HRM.comboVal('CmbUomGroup')) { HRM.box('UOM Group field is required'); HRM.focus('CmbUomGroup'); return false; }
        return true;
    }

    /** Insert(). */
    function Insert(btn) {
        if (!Validation()) return;
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var upd = RecId > 0;
        if (!upd) {                                                       // "Please select Location first"
            var rows = grdAllocation.rows(), inactive = rows.filter(function (r) { return !HRM.bool(r.Value); }).length;
            if (rows.length > 1 && inactive === rows.length) { HRM.box('Please select Location first'); return; }
        }
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', {
                id: RecId, itemCategoryId: HRM.comboVal('cmbItemCategory'), itemCode: HRM.val('txtItemCode'),
                itemName: HRM.val('txtItemName'), purchaseGl: HRM.comboVal('cmbPurchaseGL'), itemGroupId: HRM.comboVal('CmbUomGroup'),
                itemStatus: HRM.checked('chkstatus'), masterItemId: HRM.comboVal('CmbMasterItem'),
                allocation: grdAllocation.rows().map(function (r) { return { Id: r.Id, Value: HRM.bool(r.Value) }; })
            }).then(function (d) {
                HRM.box(d && d.message ? d.message : (upd ? 'Update Successfully' : 'Save Successfully'));
                return FormReset();
            }).catch(HRM.fail);
        }, 'si-save');
    }

    /** grdhistory_DoubleClick -> ReadById(Id) (the server refuses without the Update right). */
    function grdhistory_DoubleClick(r) {
        var id = HRM.int(r.Id);
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (o) {
            return FormReset().then(function () {
                RecId = HRM.int(o.Id);
                HRM.setCombo('cmbItemCategory', o.ItemCategoryId);
                HRM.setVal('txtItemCode', HRM.str(o.ItemCode));
                HRM.setVal('txtItemName', HRM.str(o.ItemName));
                HRM.setCombo('cmbPurchaseGL', o.PurchaseGLAC);
                HRM.check('chkstatus', HRM.bool(o.ItemStatus));
                HRM.setCombo('CmbMasterItem', o.ServicesMasterItemId);
                buttons(true);
                HRM.focus('cmbItemCategory');
            });
        }).catch(HRM.fail);
    }

    /** HistoryGridFill() (btnsearch_Click). */
    function HistoryGridFill(btn) {
        return HRM.busy(btn || 'btnsearch', function () {
            return HRM.get(API + '/history').then(function (rows) { grdhistory.set(groupRows(rows)); }).catch(HRM.fail);
        });
    }

    // ------------------------------------------------------------------------ handlers
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };                   // BtnSave_Click
    P.btnupdate = function (btn) {                                                               // btnupdate_Click
        if (RecId === 0) { HRM.box('Record not update because RecId not found'); return; }
        return Insert(btn || 'btnupdate');
    };
    P.btnnew = function () { return FormReset().then(function () { HRM.setCombo('CmbUomGroup', 0); }); };   // btnnew_Click
    P.refresh = function (btn) {                                                                 // toolStripButton1_Click
        return HRM.busy(btn || 'toolStripButton1', function () {
            return HRM.get(API + '/refresh').then(bindLists).catch(HRM.fail);
        });
    };
    P.search = function (btn) { return HistoryGridFill(btn); };
    P.attachment = function () { HRM.box('Attachments (DMS) are not ported on the web yet.'); };
    P.serviceCategory = function () { HRM.box('frmServicesItemCategory (Service Category) is not ported on the web yet.'); };
    P.shortcuts = function () {                                                                  // MakeShortCutKeys
        HRM.box(['Ctrl+N  For New', 'Ctrl+S  For Save', 'Ctrl+U  For Update', 'Ctrl+R  For Refresh', 'Ctrl+F5  For Focus on Doc Date',
            'Ctrl+E  For Close', 'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+ArrowUp  For Focus On History Category Combo',
            'Ctrl+ArrowDown  When In History Tab For Focus On Grid history', 'Ctrl+Enter  When Focus On Any Grid For Update Record',
            'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    };

    // cmbItemCategory_Leave: a pick fires `change`; focus leaving the combo is the Leave itself.
    HRM.$('cmbItemCategory').addEventListener('change', cmbItemCategory_Leave);

    HRM.keys({                                                                                   // InvDefrmAddItem_KeyDown
        'ctrl+n': function () { P.btnnew(); },
        'ctrl+r': function () { P.refresh(); },
        'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave(); },
        'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled) P.btnupdate(); },
        'ctrl+arrowup': function () { HRM.focus('cmbitemcathistory'); },
        'ctrl+arrowdown': function () { var tr = document.querySelector('#grdhistory tbody tr[data-i]'); if (tr) tr.focus(); },
        'ctrl+e': HRM.close,
        'ctrl+alt+control': P.shortcuts, 'ctrl+alt+alt': P.shortcuts,
        'ctrl+enter': function () {                                                              // grdhistory_KeyDown Ctrl+Enter
            var r = grdhistory.current();
            if (r && !r.__group && document.activeElement && document.activeElement.closest('#grdhistory')) grdhistory_DoubleClick(r);
        }
    });
    HRM.footer(function (b) {
        return HistoryGridFill(b).then(function () { HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); });
    });

    HRM.loading(HRM.get(API + '/setup')).then(function (d) {                                     // InitializeComponentMethod
        rights = d.rights || {};
        HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate' });
        bindLists(d);
        grdAllocation.set(d.allocation || []);
        FormInitialized = true;
        buttons(false);
        HRM.focus('cmbItemCategory');
    }).catch(HRM.fail);
})();
