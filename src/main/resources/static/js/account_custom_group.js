$(document).ready(function () {
    let groupsCache = [];
    let unallocatedCache = [];
    let allocatedCache = [];

    // Initial Data Fetch
    initScreen();

    function initScreen() {
        loadGroups();
        loadAccountTypes();
        loadParentAccounts();
    }

    // --- LEFT PANEL EVENTS ---
    $('#btnAddGroup').click(function () {
        const groupName = $('#txtGroupName').val().trim();
        const groupId = $('#selectedGroupId').val();

        if (!groupName) {
            alert('Please enter a Group Name');
            return;
        }

        const payload = {
            acLookUpsDescription: groupName
        };
        if (groupId) {
            payload.id = parseInt(groupId);
        }

        $.ajax({
            url: '/accounts/api/custom-group',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload),
            success: function (savedGroup) {
                resetLeftPanel();
                loadGroups(savedGroup ? savedGroup.id : null);
            },
            error: function (err) {
                alert('Error saving custom group: ' + (err.responseText || 'Server Error'));
            }
        });
    });

    $('#btnResetGroup, #btnReset').click(function () {
        resetLeftPanel();
        resetCenterPanel();
        resetRightPanel();
    });

    // --- CENTER PANEL EVENTS ---
    $('#ddlGroup').change(function () {
        const groupId = $(this).val();
        if (groupId) {
            $('#selectedGroupId').val(groupId);
            loadUnallocatedAccounts(groupId);
            loadAllocatedAccounts(groupId);
        } else {
            $('#tblUnAllocated tbody').empty();
            $('#tblAllocated tbody').empty();
        }
    });

    $('#ddlAccountType').change(function () {
        filterUnallocatedAccounts();
    });

    $('#chkSelectAll').change(function () {
        const isChecked = $(this).is(':checked');
        $('#tblUnAllocated tbody input[type="checkbox"]').prop('checked', isChecked);
    });

    $('#btnCenterAllocate, #btnAllocate, #btnSave').click(function () {
        const groupId = $('#ddlGroup').val();
        if (!groupId) {
            alert('Please select a Group Name first.');
            return;
        }

        const selectedIds = [];
        $('#tblUnAllocated tbody input[type="checkbox"]:checked').each(function () {
            selectedIds.push(parseInt($(this).val()));
        });

        if (selectedIds.length === 0) {
            alert('Please select at least one account to allocate.');
            return;
        }

        $.ajax({
            url: '/accounts/api/custom-group/allocate?groupId=' + groupId,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selectedIds),
            success: function () {
                loadUnallocatedAccounts(groupId);
                loadAllocatedAccounts(groupId);
            },
            error: function () {
                alert('Error allocating accounts.');
            }
        });
    });

    $('#btnCenterUnAllocate, #btnUnAllocate').click(function () {
        const groupId = $('#ddlGroup').val();
        if (groupId) {
            loadUnallocatedAccounts(groupId);
            loadAllocatedAccounts(groupId);
        }
    });

    $('#btnCenterRefresh, #btnRefresh').click(function () {
        const currentGroup = $('#ddlGroup').val();
        loadGroups(currentGroup);
    });

    // --- RIGHT PANEL EVENTS ---
    $('#btnShow').click(function () {
        filterAllocatedAccounts();
    });

    $('#btnRightReset').click(function () {
        resetRightPanel();
    });

    $('#btnPartyCustomGroup').click(function () {
        alert('Party Custom Group feature clicked.');
    });

    $('#btnShortcut').click(function () {
        alert('Shortcut Keys:\nAlt+S: Save / Allocate\nAlt+R: Reset\nAlt+F: Refresh');
    });

    // --- API & UI LOADERS ---
    function loadGroups(selectGroupId) {
        $.get('/accounts/api/custom-group/groups', function (data) {
            groupsCache = data || [];
            const $tbl = $('#tblGroups tbody').empty();
            const $ddl = $('#ddlGroup').empty().append('<option value="">Select Group...</option>');

            if (groupsCache.length > 0) {
                groupsCache.forEach(g => {
                    const desc = g.acLookUpsDescription || g.description || 'Group #' + g.id;
                    $tbl.append(`<tr data-id="${g.id}"><td>${desc}</td></tr>`);
                    $ddl.append(`<option value="${g.id}">${desc}</option>`);
                });

                // Attach row selection event
                $('#tblGroups tbody tr').click(function () {
                    $('#tblGroups tbody tr').removeClass('selected-row');
                    $(this).addClass('selected-row');
                    const id = $(this).attr('data-id');
                    const selected = groupsCache.find(x => x.id == id);
                    if (selected) {
                        $('#txtGroupName').val(selected.acLookUpsDescription || selected.description);
                        $('#selectedGroupId').val(selected.id);
                        $('#btnAddGroup').text('Update');
                        $('#ddlGroup').val(selected.id).trigger('change');
                    }
                });

                const targetId = selectGroupId || groupsCache[0].id;
                $('#ddlGroup').val(targetId).trigger('change');
            } else {
                $tbl.append('<tr><td style="text-align:center; color:#888;">No Custom Groups</td></tr>');
            }
        });
    }

    function loadAccountTypes() {
        $.get('/accounts/api/custom-group/account-types', function (data) {
            const $ddl = $('#ddlAccountType').empty().append('<option value="">Accounts Types</option>');
            if (data) {
                data.forEach(t => {
                    $ddl.append(`<option value="${t.id}">${t.name || t.typeName || t.accountTypeName}</option>`);
                });
            }
        });
    }

    function loadParentAccounts() {
        $.get('/accounts/api/custom-group/parent-accounts', function (data) {
            const $ddl = $('#ddlParentAccount').empty().append('<option value="">Parent Account</option>');
            if (data) {
                data.forEach(p => {
                    $ddl.append(`<option value="${p.id}">${p.name || p.accountTitle || p.parentAccountName}</option>`);
                });
            }
        });
    }

    function loadUnallocatedAccounts(groupId) {
        $.get('/accounts/api/custom-group/unallocated/' + groupId, function (data) {
            unallocatedCache = data || [];
            filterUnallocatedAccounts();
        });
    }

    function loadAllocatedAccounts(groupId) {
        $.get('/accounts/api/custom-group/allocated/' + groupId, function (data) {
            allocatedCache = data || [];
            renderAllocatedTable(allocatedCache);
        });
    }

    function filterUnallocatedAccounts() {
        const typeId = $('#ddlAccountType').val();
        let filtered = unallocatedCache;
        if (typeId) {
            filtered = unallocatedCache.filter(x => x.accountTypeId == typeId || x.typeId == typeId);
        }

        const $tbl = $('#tblUnAllocated tbody').empty();
        $('#chkSelectAll').prop('checked', false);

        if (filtered.length > 0) {
            filtered.forEach(acc => {
                const accId = acc.accountId || acc.id;
                const title = acc.accountTitle || acc.accountCode || 'Account #' + accId;
                const typeName = acc.accountType || acc.typeName || 'Unallocated';
                $tbl.append(`
                    <tr>
                        <td style="text-align:center;"><input type="checkbox" value="${accId}" /></td>
                        <td>${title}</td>
                        <td>${typeName}</td>
                    </tr>
                `);
            });
        } else {
            $tbl.append('<tr><td colspan="3" style="text-align:center; color:#888;">No unallocated accounts</td></tr>');
        }
    }

    function filterAllocatedAccounts() {
        const parentId = $('#ddlParentAccount').val();
        let filtered = allocatedCache;
        if (parentId) {
            filtered = allocatedCache.filter(x => x.parentAccountId == parentId || x.parentId == parentId);
        }
        renderAllocatedTable(filtered);
    }

    function renderAllocatedTable(list) {
        const $tbl = $('#tblAllocated tbody').empty();
        if (list && list.length > 0) {
            list.forEach(acc => {
                const title = acc.accountTitle || acc.accountCode || 'Account';
                const parentTitle = acc.parentAccount || acc.party || 'Party';
                const allocId = acc.allocationId || acc.id;
                $tbl.append(`
                    <tr>
                        <td>${title}</td>
                        <td>${parentTitle}</td>
                        <td style="text-align:center;">
                            <button type="button" class="btn-del-icon" title="Unallocate" onclick="unallocateAccount(${allocId})">🗑️</button>
                        </td>
                    </tr>
                `);
            });
        } else {
            $tbl.append('<tr><td colspan="3" style="text-align:center; color:#888;">No allocated accounts</td></tr>');
        }
    }

    window.unallocateAccount = function (allocationId) {
        if (!confirm('Remove account from this custom group?')) return;
        const groupId = $('#ddlGroup').val();
        $.ajax({
            url: '/accounts/api/custom-group/unallocate?allocationId=' + allocationId,
            type: 'POST',
            success: function () {
                loadUnallocatedAccounts(groupId);
                loadAllocatedAccounts(groupId);
            },
            error: function () {
                alert('Error unallocating account.');
            }
        });
    };

    function resetLeftPanel() {
        $('#txtGroupName').val('');
        $('#selectedGroupId').val('');
        $('#btnAddGroup').text('Add');
        $('#tblGroups tbody tr').removeClass('selected-row');
    }

    function resetCenterPanel() {
        $('#ddlAccountType').val('');
        filterUnallocatedAccounts();
    }

    function resetRightPanel() {
        $('#ddlParentAccount').val('');
        renderAllocatedTable(allocatedCache);
    }
});
