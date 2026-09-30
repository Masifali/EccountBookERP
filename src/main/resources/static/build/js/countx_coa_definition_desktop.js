/*
 * Screen 9 "Define Accounts" - AcfrmDefCoa, event for event.
 * Server: ChartOfAccountDesktopController (/accounts/chart_of_accounts/api/*), which calls the
 * desktop's own procedures. Handler names below are the desktop's.
 */
(function ($) {
    'use strict';
    var API = '/accounts/chart_of_accounts/api';
    var L = {};                 // lookups
    var dtHistory = [];         // ReadAccountLevel rows
    var ACCOUNTID = 0;
    var saveVisible = true;     // btnsave.Visible (btnUpdate.Visible is the opposite)
    var customerGroupReadOnly = true;
    var flagAfterSave = false, flagFromHistoryButton = false;
    var historyRows = [], historyOriginal = {}, modifiedRowIds = {};

    function v(row, key) {
        if (!row) return null;
        if (row[key] !== undefined) return row[key];
        var lk = key.toLowerCase();
        for (var k in row) { if (k.toLowerCase() === lk) return row[k]; }
        return null;
    }
    function s(x) { return x == null ? '' : String(x); }
    function esc(x) { return s(x).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function toInt(x) { var n = parseInt(x, 10); return isNaN(n) ? 0 : n; }
    function toNum(x) { var n = parseFloat(x); return isNaN(n) ? 0 : n; }

    /* DDL.BindDDL: keeps the current value when it still exists */
    function bind(sel, rows, valueKey, textKey) {
        var cur = $(sel).val();
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            html += '<option value="' + esc(v(r, valueKey)) + '">' + esc(v(r, textKey)) + '</option>';
        });
        $(sel).html(html);
        if (cur != null && $(sel).find('option').filter(function () { return this.value === cur; }).length) $(sel).val(cur);
    }
    function selText(sel) { var o = $(sel).find('option:selected'); return o.length && o.val() !== '' ? o.text() : ''; }
    function setByText(sel, text) {
        var found = false;
        $(sel).find('option').each(function () { if (!found && $(this).text() === s(text)) { $(sel).val(this.value); found = true; } });
        if (!found) $(sel).val('');
        return found;
    }

    // ------------------------------------------------------------------ load

    $(function () {
        $('.erp-tab-button').on('click', function () {
            $('.erp-tab-button').removeClass('active');
            $(this).addClass('active');
            $('.tab-content .tab-pane').removeClass('active');
            $($(this).data('target')).addClass('active');
            /* TabCOA_SelectedIndexChanged */
            if ($(this).data('target') === '#tabHistory' && flagAfterSave && flagFromHistoryButton) {
                formHistoryBind();
                flagAfterSave = false; flagFromHistoryButton = false;
            }
        });
        wire();
        load();
    });

    function load() {
        $.get(API + '/lookups', function (res) {
            L = res;
            var r = res.rights || {};
            $('#btnSaveToolbar').prop('disabled', !r.save);
            $('#btnUpdateToolbar').prop('disabled', !r.update);
            $('#btnUpdateAccountsExcelSheet').toggle(!!r.update);
            $('#btnPrint').prop('disabled', !r.print);
            accountTitleFill();
            bind('#CmbCurrencyId', res.currencies, 'Id', 'CurrencyCode');
            bind('#cmbcustomgroup', res.customGroups, 'Id', 'AcLookUpsDescription');
            cmbparentacfill(res.parentAccounts);
            bind('#cmbbsnote', res.bsNotes, 'Id', 'NoteTitle');
            bind('#cmbplnote', res.plNotes, 'Id', 'NoteTitle');
            cmbAccountType();
            branchesBindInGrid();
            $('#rowCurrency').toggle(!!res.multiCurrency);
            $('#cmbgroupdetail, #cmbplnote, #cmbbsnote').prop('disabled', true);
            bind('#CmbCity', res.cities, 'Id', 'CityName');
            $('#CmbCity, #txtPhoneNo').prop('disabled', true);
            historyGridFill(function () { grdfrmfill(); });
            $('#cmbparentac').focus();
        });
    }

    function accountTitleFill() { bind('#CmbAccountTitle', L.accountTitles, 'Id', 'AccountTitle'); }

    /* cmbparentacfill: ValueMember AccountCode, DisplayMember AccountTitle; ParentCodeId = the group row's Id */
    function cmbparentacfill(rows) {
        var cur = $('#cmbparentac').val();
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) {
            html += '<option value="' + esc(v(r, 'AccountCode')) + '" data-id="' + esc(v(r, 'Id')) + '" data-level="' + esc(v(r, 'Account_Level'))
                 + '" title="' + esc(v(r, 'AccountCode') + ' | ' + s(v(r, 'ClassName')) + ' | Level ' + s(v(r, 'Account_Level'))) + '">' + esc(v(r, 'AccountTitle')) + '</option>';
        });
        $('#cmbparentac').html(html);
        if (cur) $('#cmbparentac').val(cur);
    }

    function cmbAccountType() {
        bind('#cmbactype', L.accountTypes, 'Id', 'AccountType');
        bind('#CmbAccountTypeFilter', L.accountTypes, 'Id', 'AccountType');
        if ((L.accountTypes || []).length) $('#cmbactype').prop('disabled', false);
    }

    function branchesBindInGrid() {
        var html = '';
        (L.locations || []).forEach(function (r) {
            html += '<tr><td>' + esc(v(r, 'CompName')) + '</td><td style="text-align:center;"><input type="checkbox" class="alloc-chk" data-id="' + esc(v(r, 'Id')) + '" checked/></td></tr>';
        });
        $('#grdAllocation tbody').html(html);
    }

    function historyGridFill(done) {
        $.get(API + '/account-levels', function (rows) { dtHistory = rows || []; if (done) done(); });
    }

    /* cmbsupgroupsfill: 22 -> ids 7, 9, 10; 3 -> every other id; any other type -> nothing (no rebind) */
    function cmbsupgroupsfill() {
        var type = toInt($('#cmbactype').val());
        var rows = [];
        (L.customerGroups || []).forEach(function (r) {
            var id = toInt(v(r, 'Id'));
            if (type === 22) { if (id === 7 || id === 9 || id === 10) rows.push(r); }
            else if (type === 3) { if (id !== 7 && id !== 9 && id !== 10) rows.push(r); }
            else rows = [];
        });
        if (rows.length > 0) bind('#CmbCustomerGroup', rows, 'Id', 'Description');
    }

    // ------------------------------------------------------------------ level grids

    function rowsHtml(rows, cls) {
        var html = '';
        rows.forEach(function (r) {
            html += '<tr class="' + cls + '" data-id="' + esc(v(r, 'Id')) + '" data-code="' + esc(v(r, 'AccountCode')) + '" data-title="' + esc(v(r, 'AccountTitle')) + '">'
                 + '<td style="font-weight:bold; color:#00796B;">' + esc(v(r, 'AccountCode')) + '</td><td>' + esc(v(r, 'AccountTitle')) + '</td></tr>';
        });
        return html;
    }
    function levelRows(level, parentCode) {
        return dtHistory.filter(function (r) {
            return toInt(v(r, 'Account_Level')) === level && (parentCode === null || s(v(r, 'ParentAccountCode')) === parentCode);
        });
    }
    function grdfrmfill() {
        $('#grdfrm tbody').html(rowsHtml(levelRows(1, null), 'lvl1'));
        $('#grdlevel2 tbody, #grdlevel3 tbody, #grdlevel4 tbody').html('');
    }

    function current(grid) { return $(grid + ' tbody tr.selected-row'); }

    // ------------------------------------------------------------------ child of selected parent

    function renderParentCodeGrid(rows, withEdit) {
        $('#grdParentCode thead').html('<tr><th style="width:120px;">AccountCode</th><th>AccountTitle</th>' + (withEdit ? '<th style="width:50px;">Edit</th>' : '') + '</tr>');
        var html = '';
        rows.forEach(function (r) {
            html += '<tr data-id="' + esc(v(r, 'Id')) + '"><td>' + esc(v(r, 'AccountCode')) + '</td><td>' + esc(v(r, 'AccountTitle')) + '</td>'
                 + (withEdit ? '<td><button type="button" class="btn-erp coa-edit" style="padding:0 6px;">Edit</button></td>' : '') + '</tr>';
        });
        $('#grdParentCode tbody').html(html);
    }

    // ------------------------------------------------------------------ cmbparentac_Leave

    function classText(c) {
        switch (s(c)) { case '1': return 'Capital'; case '2': return 'Assets'; case '3': return 'Liabilities'; case '4': return 'Expenses'; case '5': return 'Revenue'; default: return null; }
    }

    function cmbparentacLeave(after) {
        $('#button1').text(saveVisible ? 'Save' : 'Update');
        var parentCode = s($('#cmbparentac').val());
        $.get(API + '/new-code', { parentCode: parentCode }, function (res) {
            if (!res.success) { alert(res.message); return; }
            var row = res.row;
            var ct = classText(v(row, 'AccountClass'));
            if (ct !== null) $('#txtacclass').val(ct);
            $('#txtaclevel').val(s(v(row, 'Account_Level')));
            $('#txtaccode').val(s(v(row, 'AccountCode')));
            var level = toNum($('#txtaclevel').val());
            cmbAccountType();
            if (level >= 4) {
                setByText('#cmbactype', s(v(row, 'AccountType')));
                $('#cmbactype').prop('disabled', true);
                $('#CmbCurrencyId').prop('disabled', false);
            }
            cmbsupgroupsfill();
            if (level >= 4) setByText('#CmbCustomerGroup', s(v(row, 'CustomerGroupId')));
            if (level === 4) { customerGroupReadOnly = false; $('#CmbCustomerGroup').val(''); }
            else customerGroupReadOnly = true;
            var lv = toInt($('#txtaclevel').val());
            if (lv < 4) { $('#cmbgroupdetail').val('Group').prop('disabled', true); $('#CmbCurrencyId').prop('disabled', true); }
            else { $('#cmbgroupdetail').val('Detail').prop('disabled', true); }
            if (lv === 3) $('#cmbactype').prop('disabled', false);
            var classcode = toInt(v(row, 'AccountClass'));
            if (lv === 3) {
                if (classcode === 1 || classcode === 2 || classcode === 3) { $('#cmbplnote').prop('disabled', true); $('#cmbbsnote').prop('disabled', false); }
                if (classcode === 4 || classcode === 5) { $('#cmbplnote').prop('disabled', false); $('#cmbbsnote').prop('disabled', true); }
            }
            if (lv !== 3) { $('#cmbplnote, #cmbbsnote').prop('disabled', true); $('#cmbactype').prop('disabled', true); }
            if (lv < 3) { $('#cmbactype').prop('disabled', true).val(''); $('#cmbbsnote, #cmbplnote').val(''); }
            $('#CmbCity, #txtPhoneNo, #txtOpeningBalance').prop('disabled', !(level >= 4));
            /* Child Of Selected Parent: a fresh ReadAccountLevel filtered on ParentAccountCode */
            $.get(API + '/account-levels', function (rows) {
                renderParentCodeGrid((rows || []).filter(function (r) { return s(v(r, 'ParentAccountCode')) === parentCode; }), true);
                cmbgroupdetailLeave();
                cmbactypeLeave();
                if (after) after();
            });
        });
    }

    function cmbgroupdetailLeave() {
        if ($('#cmbgroupdetail').val() === 'Detail') { $('#cmbcustomgroup').prop('disabled', false); return; }
        $('#cmbcustomgroup').prop('disabled', true).val('');
    }

    function cmbactypeLeave() {
        var t = toInt($('#cmbactype').val());
        if (t === 3 || t === 22) { customerGroupReadOnly = false; cmbsupgroupsfill(); }
        else { $('#CmbCustomerGroup').val(''); customerGroupReadOnly = true; }
        $('#CmbCustomerGroup').prop('disabled', customerGroupReadOnly);
    }

    // ------------------------------------------------------------------ New / Refresh

    function refreshForm() {
        $('#button1').text('Save');
        ACCOUNTID = 0;
        $('#cmbbsnote, #cmbplnote, #cmbactype').prop('disabled', true);
        $('#txtactitle, #txtacclass, #txtaclevel, #txtaccode, #txtPhoneNo, #txtOpeningBalance, #txtOtherCode').val('');
        $('#cmbplnote, #cmbbsnote, #cmbgroupdetail, #CmbCity, #CmbCurrencyId, #cmbactype, #CmbAccountTypeFilter, #CmbAccountTitle').val('');
        historyGridFill();
        $.get(API + '/parent-accounts', function (rows) { L.parentAccounts = rows; cmbparentacfill(rows); });
        accountTitleFill();
    }

    function setMode(save) {
        saveVisible = save;
        $('#btnSaveToolbar').toggle(save);
        $('#btnUpdateToolbar').toggle(!save);
    }

    function btnnew() {
        refreshForm();
        setMode(true);
        $('#cmbparentac').val('');
        $('#grdParentCode thead, #grdParentCode tbody').html('');
    }

    // ------------------------------------------------------------------ Save / Update

    function formvalidation() {
        if ($.trim($('#txtactitle').val()) === '') { alert('Please Enter Account Title'); $('#txtactitle').focus(); return false; }
        if ($.trim(selText('#cmbparentac')) === '') { alert('Please Enter Parent Account'); $('#cmbparentac').focus(); return false; }
        var lv = $('#txtaclevel').val();
        if (lv === '3' && $.trim(selText('#cmbactype')) === '') { alert('Please Enter Account Type'); $('#cmbactype').focus(); return false; }
        if (!$('#cmbplnote').prop('disabled') && lv === '3' && $.trim(selText('#cmbplnote')) === '') { alert('Please Enter PL Notes'); $('#cmbplnote').focus(); return false; }
        if (!$('#cmbbsnote').prop('disabled') && lv === '3' && $.trim(selText('#cmbbsnote')) === '') { alert('Please Enter BS Notes'); $('#cmbbsnote').focus(); return false; }
        if (!customerGroupReadOnly && lv === '4' && $.trim(selText('#CmbCustomerGroup')) === '') { alert('Please Select Customer Group'); $('#CmbCustomerGroup').focus(); return false; }
        if ($('#cmbactype').val() && toInt($('#cmbactype').val()) === 3 && L.customGroupCompulsory && !$('#cmbcustomgroup').val()) { alert('Please Select Custom Group'); $('#cmbcustomgroup').focus(); return false; }
        if (L.multiCurrency && $('#cmbgroupdetail').val() === 'Detail' && toInt($('#CmbCurrencyId').val()) === 0) { alert('Please Select Currency'); $('#CmbCurrencyId').focus(); return false; }
        return true;
    }

    function btnsave() {
        if (!formvalidation()) return;
        var allocations = [];
        $('#grdAllocation tbody .alloc-chk').each(function () { allocations.push({ id: toInt($(this).data('id')), value: $(this).is(':checked') }); });
        var body = {
            parentAccountCode: s($('#cmbparentac').val()),
            parentAccountText: selText('#cmbparentac'),
            parentCodeId: toInt($('#cmbparentac option:selected').data('id')),
            accountClass: $('#txtacclass').val(),
            accountLevel: $.trim($('#txtaclevel').val()),
            accountCode: $('#txtaccode').val(),
            accountTitle: $('#txtactitle').val(),
            accountGroup: selText('#cmbgroupdetail'),
            accountTypeId: toInt($('#cmbactype').val()), accountTypeText: selText('#cmbactype'),
            plNoteId: toInt($('#cmbplnote').val()), plNoteText: selText('#cmbplnote'),
            bsNoteId: toInt($('#cmbbsnote').val()), bsNoteText: selText('#cmbbsnote'),
            customerGroupId: toInt($('#CmbCustomerGroup').val()), customerGroupText: selText('#CmbCustomerGroup'),
            customGroupId: toInt($('#cmbcustomgroup').val()),
            cityId: toInt($('#CmbCity').val()),
            currencyId: toInt($('#CmbCurrencyId').val()),
            phoneNo: $('#txtPhoneNo').val(),
            otherCode: $('#txtOtherCode').val(),
            openingBalance: $('#txtOpeningBalance').val(),
            allocations: allocations
        };
        postJson(API + '/save', body, function (res) {
            alert(res.message);
            if (!res.success) return;
            flagAfterSave = true;
            refreshForm();
            historyGridFill(function () { grdfrmfill(); });
            $('#txtactitle').focus();
            cmbparentacLeave();
        });
    }

    function btnUpdate() {
        var title = $('#txtactitle').val();
        if (title === '' || title === '0') { alert('AccountTitle Field Required'); return; }
        if (L.multiCurrency && $('#cmbgroupdetail').val() === 'Detail' && toInt($('#CmbCurrencyId').val()) === 0) { alert('Please Select Currency'); $('#CmbCurrencyId').focus(); return; }
        postJson(API + '/update', {
            id: ACCOUNTID, accountTitle: title, otherCode: $('#txtOtherCode').val(),
            currencyId: toInt($('#CmbCurrencyId').val()), phoneNo: $('#txtPhoneNo').val(), accountGroup: $('#cmbgroupdetail').val()
        }, function (res) {
            alert(res.message);
            if (!res.success) return;
            refreshForm();
            setMode(true);
            historyGridFill(function () { grdfrmfill(); });
        });
    }

    function postJson(url, body, done) {
        $.ajax({ url: url, type: 'POST', contentType: 'application/json', data: JSON.stringify(body), success: done,
                 error: function (x) { alert('Server error: ' + (x.responseText || x.status)); } });
    }

    /* ReadById (grdParentCode "Edit") */
    function readById(id) {
        setMode(false);
        ACCOUNTID = id;
        $.get(API + '/account/' + id, function (res) {
            var chart = res.account;
            if (!chart) return;
            setByText('#cmbparentac', s(v(chart, 'ParentAccountCodeTitle')));
            cmbparentacLeave(function () {
                if (v(chart, 'AccountGroup') === 'Detail') {
                    if (toInt(v(chart, 'CurrencyId')) > 0) $('#CmbCurrencyId').val(s(v(chart, 'CurrencyId')));
                    $('#txtPhoneNo').val(s(v(chart, 'ContactNo')));
                }
                $('#txtactitle').val(s(v(chart, 'AccountTitle')));
                $('#txtOtherCode').val(s(v(chart, 'OtherErpCode')));
                $('#button1').text('Update');
                $('#txtactitle').focus();
            });
        });
    }

    // ------------------------------------------------------------------ print

    function print() {
        $.get(API + '/print', function (res) {
            if (!res.success) { alert(res.message); return; }
            var rows = res.rows || [];
            var first = rows[0] || {};
            var html = '<html><head><title>114-AcRptChartOfAccounts</title><style>body{font-family:Verdana;font-size:11px}table{border-collapse:collapse;width:100%}th,td{border:1px solid #999;padding:2px 4px;text-align:left}th{background:#eee}</style></head><body>'
                + '<h3 style="margin:0">' + esc(v(first, 'CompName')) + '</h3><div>' + esc(v(first, 'CompAddress')) + '</div><h4>Chart Of Accounts</h4>'
                + '<table><thead><tr><th>Account Code</th><th>Account Title</th><th>Group</th><th>Level</th><th>Class</th><th>Type</th><th>Note</th><th>Other Code</th><th>Active</th></tr></thead><tbody>';
            rows.forEach(function (r) {
                var pad = (toInt(v(r, 'Account_Level')) - 1) * 14;
                html += '<tr><td>' + esc(v(r, 'AccountCode')) + '</td><td style="padding-left:' + (4 + pad) + 'px">' + esc(v(r, 'AccountTitle')) + '</td><td>' + esc(v(r, 'AccountGroup'))
                     + '</td><td>' + esc(v(r, 'Account_Level')) + '</td><td>' + esc(v(r, 'AccountClass')) + '</td><td>' + esc(v(r, 'AccountType')) + '</td><td>' + esc(v(r, 'NoteTitle'))
                     + '</td><td>' + esc(v(r, 'OtherErpCode')) + '</td><td>' + esc(v(r, 'IsActive')) + '</td></tr>';
            });
            html += '</tbody></table></body></html>';
            var w = window.open('', '_blank');
            if (w) { w.document.write(html); w.document.close(); }
        });
    }

    // ------------------------------------------------------------------ history tab

    function formHistoryBind() {
        var lv = toInt($('input[name="rdUptoLevel"]:checked').val());
        var map = { 1: '1', 2: '1,2', 3: '1,2,3', 4: '1,2,3,4', 5: '1,2,3,4,5' };
        var levels = map[lv] || '';
        if (!levels) { alert('Please Check the Ac Levels First'); return; }
        $.get(API + '/history', { levels: levels }, function (res) {
            if (!res.success) { alert(res.message); return; }
            var rows = res.rows || [];
            if (rows.length <= 0) return;
            historyRows = rows; historyOriginal = {}; modifiedRowIds = {};
            rows.forEach(function (r) {
                historyOriginal[toInt(v(r, 'Id'))] = { title: s(v(r, 'AccountTitle')), erp: s(v(r, 'OtherErpCode')), active: isTrue(v(r, 'IsActive')), currency: toInt(v(r, 'CurrencyId')) };
            });
            renderHistory();
        });
    }
    function isTrue(x) { return x === true || x === 1 || s(x).toLowerCase() === 'true' || s(x) === '1'; }

    function renderHistory() {
        var mc = !!L.multiCurrency;
        $('#grdFormHistory thead').html('<tr><th style="min-width:260px;">AccountTitle</th><th>AccountCode</th><th>OtherErpCode</th>' + (mc ? '<th>Currency</th>' : '')
            + '<th>AccountGroup</th><th>Account_Level</th><th>IsActive</th><th>AccountClass</th><th>AccountType</th><th>NoteTitle</th><th>Update</th><th>Add Sibling</th><th>Add Child</th></tr>');
        var byParent = {}, ids = {};
        historyRows.forEach(function (r) { ids[toInt(v(r, 'Id'))] = true; });
        historyRows.forEach(function (r) {
            var p = toInt(v(r, 'ParentCodeId'));
            var key = (p !== 0 && ids[p]) ? p : 0;
            (byParent[key] = byParent[key] || []).push(r);
        });
        var collapsed = $('input[name="rdExpandCollapse"]:checked').val() === 'Collapse';
        var html = '';
        function walk(parentId, depth) {
            (byParent[parentId] || []).forEach(function (r) {
                var id = toInt(v(r, 'Id')), lvl = toInt(v(r, 'Account_Level'));
                var hasKids = !!byParent[id];
                var editable = lvl !== 1;
                var act = isTrue(v(r, 'IsActive'));
                html += '<tr data-id="' + id + '" data-parent="' + parentId + '" data-level="' + lvl + '"' + (collapsed && depth > 0 ? ' style="display:none"' : '') + '>'
                    + '<td style="padding-left:' + (4 + depth * 16) + 'px">' + (hasKids ? '<span class="h-tog" style="cursor:pointer">' + (collapsed ? '[+]' : '[-]') + '</span> ' : '')
                    + (editable ? '<input type="text" class="h-title" value="' + esc(v(r, 'AccountTitle')) + '" style="width:85%"/>' : esc(v(r, 'AccountTitle'))) + '</td>'
                    + '<td>' + esc(v(r, 'AccountCode')) + '</td>'
                    + '<td>' + (editable ? '<input type="text" class="h-erp" value="' + esc(v(r, 'OtherErpCode')) + '" style="width:90px"/>' : esc(v(r, 'OtherErpCode'))) + '</td>'
                    + (mc ? '<td>' + currencyCell(r, editable && lvl >= 4) + '</td>' : '')
                    + '<td>' + esc(v(r, 'AccountGroup')) + '</td><td>' + esc(lvl) + '</td>'
                    + '<td>' + (editable && lvl >= 4
                        ? '<select class="h-active" style="color:' + (act ? 'darkgreen' : 'red') + '"><option value="True"' + (act ? ' selected' : '') + '>True</option><option value="False"' + (!act ? ' selected' : '') + '>False</option></select>'
                        : '<span style="color:' + (act ? 'darkgreen' : 'red') + '">' + (act ? 'True' : 'False') + '</span>') + '</td>'
                    + '<td>' + esc(v(r, 'AccountClass')) + '</td><td>' + esc(v(r, 'AccountType')) + '</td><td>' + esc(v(r, 'NoteTitle')) + '</td>'
                    + '<td><button type="button" class="btn-erp h-upd" style="padding:0 6px;">Update</button></td>'
                    + '<td><button type="button" class="btn-erp h-sib" style="padding:0 6px;">Add Sibling</button></td>'
                    + '<td><button type="button" class="btn-erp h-child" style="padding:0 6px;">Add Child</button></td></tr>';
                walk(id, depth + 1);
            });
        }
        walk(0, 0);
        $('#grdFormHistory tbody').html(html);
    }
    function currencyCell(r, editable) {
        var cur = toInt(v(r, 'CurrencyId'));
        if (!editable) {
            var t = ''; (L.currencies || []).forEach(function (c) { if (toInt(v(c, 'Id')) === cur) t = s(v(c, 'CurrencyCode')); });
            return esc(t);
        }
        var h = '<select class="h-cur"><option value="0"></option>';
        (L.currencies || []).forEach(function (c) { h += '<option value="' + esc(v(c, 'Id')) + '"' + (toInt(v(c, 'Id')) === cur ? ' selected' : '') + '>' + esc(v(c, 'CurrencyCode')) + '</option>'; });
        return h + '</select>';
    }
    function historyRowValues($tr) {
        var id = toInt($tr.data('id'));
        var o = historyOriginal[id] || {};
        var title = $tr.find('.h-title').length ? $tr.find('.h-title').val() : o.title;
        var erp = $tr.find('.h-erp').length ? $tr.find('.h-erp').val() : o.erp;
        var active = $tr.find('.h-active').length ? $tr.find('.h-active').val() === 'True' : o.active;
        var currency = $tr.find('.h-cur').length ? toInt($tr.find('.h-cur').val()) : o.currency;
        return { id: id, accountTitle: title, otherErpCode: erp, isActive: active, currencyId: currency };
    }
    function historyRow(id) {
        var r = null; historyRows.forEach(function (x) { if (toInt(v(x, 'Id')) === id) r = x; }); return r;
    }

    /* grdFormHistory_ColumnButtonClick */
    function historyButton($tr, key) {
        var id = toInt($tr.data('id'));
        var row = historyRow(id);
        var level = toInt(v(row, 'Account_Level'));
        if (level === 1) return;
        if (key === 'Edit') {
            if (!confirm('Are you sure to Update?')) return;
            postJson(API + '/history/update', [historyRowValues($tr)], function (res) {
                if (!res.success) { alert(res.message); return; }
                alert('Record Update Successfully...;');
                formHistoryBind();
                $('#button1').text(saveVisible ? 'Save' : 'Update');
            });
        } else if (key === 'AddSibling' || (key === 'AddChild' && level !== 4)) {
            saveVisible = true; $('#btnSaveToolbar').show();
            /* cmbparentac.Value = Conversion.ToInt(code): the int, as the desktop assigns it */
            var code = key === 'AddSibling' ? v(row, 'ParentAccountCode') : v(row, 'AccountCode');
            $('#cmbparentac').val(String(toInt(code)));
            $('.erp-tab-button[data-target="#tabForm"]').click();
            flagFromHistoryButton = true;
            cmbparentacLeave(function () { cmbactypeLeave(); $('#txtactitle').focus(); });
        }
    }

    /* btnUpdateHistory_Click */
    function btnUpdateHistory() {
        if (!confirm('Are you sure you want to update?')) return;
        if ($('#grdFormHistory tbody tr').length === 0) { alert('Record Not found in the grid...'); return; }
        var list = [];
        Object.keys(modifiedRowIds).forEach(function (k) {
            var id = toInt(k);
            var $tr = $('#grdFormHistory tbody tr[data-id="' + id + '"]');
            var o = historyOriginal[id];
            if (!$tr.length || !o) return;
            var n = historyRowValues($tr);
            if (o.title !== n.accountTitle || o.erp !== n.otherErpCode || o.active !== n.isActive || (L.multiCurrency && o.currency !== n.currencyId)) list.push(n);
        });
        if (list.length > 0) {
            postJson(API + '/history/update', list, function (res) {
                if (!res.success) { alert(res.message); return; }
                alert('Record updated successfully.');
                formHistoryBind();
                modifiedRowIds = {};
            });
        } else {
            alert('No changes were found in the grid...');
        }
    }

    // ------------------------------------------------------------------ wiring

    function wire() {
        $('#btnNew').on('click', btnnew);
        $('#btnRefresh').on('click', function () {
            $.get(API + '/lookups', function (res) {
                L = res;
                accountTitleFill();
                bind('#CmbCurrencyId', res.currencies, 'Id', 'CurrencyCode');
                cmbparentacfill(res.parentAccounts);
                bind('#CmbCity', res.cities, 'Id', 'CityName');
                cmbsupgroupsfill();
                bind('#cmbcustomgroup', res.customGroups, 'Id', 'AcLookUpsDescription');
                if ($('#grdFormHistory tbody tr').length > 0) renderHistory();
            });
        });
        $('#btnSaveToolbar').on('click', btnsave);
        $('#btnUpdateToolbar').on('click', btnUpdate);
        $('#button1').on('click', function () { if (ACCOUNTID > 0) btnUpdate(); else btnsave(); });
        $('#btnPrint').on('click', print);
        $('#btnDefineCity').on('click', function () { alert('Define City (DefineCity) is not available in the web application yet.'); });
        $('#btnUploadExcelSheet').on('click', function () { alert('Upload Excel Sheet (frmExcelSheetUpload) is not available in the web application yet.'); });
        $('#btnUpdateAccountsExcelSheet').on('click', function () { alert('Update Chart Of Accounts Through Excel Sheet (frmExcelSheetTitleUpdate) is not available in the web application yet.'); });

        $('#cmbparentac').on('change', function () { cmbparentacLeave(); });
        $('#cmbactype').on('change', cmbactypeLeave);
        $('#cmbgroupdetail').on('change', cmbgroupdetailLeave);
        $('#CmbAccountTitle').on('change', function () {
            var id = toInt($(this).val());
            $.get(API + '/parent-code-by-account/' + id, function (res) {
                var code = toInt(res.code);
                if (code > 0) {
                    $('#cmbparentac').val(String(code));
                    cmbparentacLeave(function () { $('#txtactitle').focus(); });
                }
            });
        });
        $('#CmbAccountTypeFilter').on('change', function () {
            $.get(API + '/third-level-by-type/' + toInt($(this).val()), function (rows) {
                renderParentCodeGrid(rows || [], false);
            });
        });
        $('#chkAllocAll').on('change', function () { $('#grdAllocation .alloc-chk').prop('checked', this.checked); });
        $('#txtOpeningBalance').on('keypress', function (e) {
            var c = String.fromCharCode(e.which);
            if (e.which >= 32 && !/[0-9.\-]/.test(c)) e.preventDefault();
        });
        $('#txtPhoneNo').on('keypress', function (e) {
            var c = String.fromCharCode(e.which);
            if (e.which >= 32 && !/[0-9\-]/.test(c)) e.preventDefault();
        });

        /* level grids */
        $('#grdfrm').on('click', 'tr', function () {
            $('#grdfrm tr').removeClass('selected-row'); $(this).addClass('selected-row');
            $('#grdlevel2 tbody').html(rowsHtml(levelRows(2, s($(this).data('code'))), 'lvl2'));
        });
        $('#grdlevel2').on('click', 'tr', function () {
            $('#grdlevel2 tr').removeClass('selected-row'); $(this).addClass('selected-row');
            $('#grdlevel3 tbody').html(rowsHtml(levelRows(3, s($(this).data('code'))), 'lvl3'));
        }).on('dblclick', 'tr', function () {
            $('#cmbparentac').val(s($(this).data('code'))); cmbparentacLeave();
        });
        $('#grdlevel3').on('click', 'tr', function () {
            $('#grdlevel3 tr').removeClass('selected-row'); $(this).addClass('selected-row');
            $('#grdlevel4 tbody').html(rowsHtml(levelRows(4, s($(this).data('code'))), 'lvl4'));
            $('#grdParentCode thead, #grdParentCode tbody').html('');
        }).on('dblclick', 'tr', function () {
            $('#cmbparentac').val(s($(this).data('code'))); cmbparentacLeave();
        });
        $('#grdlevel4').on('click', 'tr', function () {
            $('#grdlevel4 tr').removeClass('selected-row'); $(this).addClass('selected-row');
            renderParentCodeGrid(levelRows(5, s($(this).data('code'))), false);
        }).on('dblclick', 'tr', function () {
            /* grdlevel4_DoubleClick */
            var $r = $(this);
            setMode(false);
            ACCOUNTID = toInt($r.data('id'));
            $.get(API + '/account/' + ACCOUNTID, function (res) {
                var chart = res.account || {};
                $.get(API + '/parent-accounts', function (rows) {
                    cmbparentacfill(rows);
                    $('#cmbparentac').val(s(current('#grdlevel3').data('code')));
                    $('#txtactitle').val(s($r.data('title')));
                    $('#txtOtherCode').val(s(v(chart, 'OtherErpCode')));
                    cmbparentacLeave();
                });
            });
        });
        $('#grdParentCode').on('click', '.coa-edit', function () { readById(toInt($(this).closest('tr').data('id'))); });

        /* history */
        $('#btnshow').on('click', formHistoryBind);
        $('#btnUpdateHistory').on('click', btnUpdateHistory);
        $('#grdFormHistory').on('input change', 'input, select', function () {
            modifiedRowIds[toInt($(this).closest('tr').data('id'))] = true;
            if ($(this).hasClass('h-active')) $(this).css('color', $(this).val() === 'True' ? 'darkgreen' : 'red');
        });
        $('#grdFormHistory').on('click', '.h-upd', function () { historyButton($(this).closest('tr'), 'Edit'); });
        $('#grdFormHistory').on('click', '.h-sib', function () { historyButton($(this).closest('tr'), 'AddSibling'); });
        $('#grdFormHistory').on('click', '.h-child', function () { historyButton($(this).closest('tr'), 'AddChild'); });
        $('#grdFormHistory').on('click', '.h-tog', function () {
            var $t = $(this), $tr = $t.closest('tr'), id = s($tr.data('id'));
            var open = $t.text() === '[+]';
            $t.text(open ? '[-]' : '[+]');
            (function toggle(pid, show) {
                $('#grdFormHistory tbody tr[data-parent="' + pid + '"]').each(function () {
                    $(this).toggle(show);
                    var tg = $(this).find('.h-tog');
                    if (!show) toggle(s($(this).data('id')), false);
                    else if (tg.length && tg.text() === '[-]') toggle(s($(this).data('id')), true);
                });
            })(id, open);
        });

        /* AcfrmDefCoa_KeyDown */
        $(document).on('keydown', function (e) {
            if (!e.ctrlKey) return;
            if (e.keyCode === 78) { e.preventDefault(); btnnew(); }
            if (e.keyCode === 83 && saveVisible && !$('#btnSaveToolbar').prop('disabled')) { e.preventDefault(); btnsave(); }
            if (e.keyCode === 85 && !saveVisible && !$('#btnUpdateToolbar').prop('disabled')) { e.preventDefault(); btnUpdate(); }
            if (e.keyCode === 80) { e.preventDefault(); print(); }
            if (e.keyCode === 122) { e.preventDefault(); $('#btnUpdateHistory').show(); }
        });
    }
})(jQuery);
