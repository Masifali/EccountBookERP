/**
 * Ditto implementation of Desktop WinForms InvDeffrmItemType.cs
 */
$(document).ready(function () {
    loadLookups();
    loadHistory();
    resetForm();

    $('#btnNew').on('click', function () {
        resetForm();
    });

    $('#btnRefresh').on('click', function () {
        loadLookups();
        loadHistory();
    });

    $('#btnSave').on('click', function () {
        saveRecord(false);
    });

    $('#btnUpdate').on('click', function () {
        saveRecord(true);
    });
});

function resetForm() {
    $('#recId').val('0');
    $('#txtItemTypeCode').val('');
    $('#txtTypeDescription').val('');
    $('#cmbItemTypeType').val('');
    $('#cmbParentCategory').val('');
    $('#chkIsMother').prop('checked', false);

    $('#btnSave').show();
    $('#btnUpdate').hide();

    // Auto-generate code
    generateCode();
}

function generateCode() {
    $.ajax({
        url: '/api/inventory/item_types/generate-code',
        type: 'GET',
        success: function (res) {
            if (res && res.code) {
                $('#txtItemTypeCode').val(res.code);
            }
        },
        error: function (err) {
            console.error('Error generating code:', err);
        }
    });
}

function loadLookups() {
    $.ajax({
        url: '/api/inventory/item_types/lookups',
        type: 'GET',
        success: function (res) {
            // Populate Parent Categories
            var $parentCat = $('#cmbParentCategory');
            $parentCat.empty();
            $parentCat.append('<option value="">-- Select Parent Category --</option>');
            if (res.parentCategories) {
                $.each(res.parentCategories, function (i, item) {
                    var id = item.Id || item.id;
                    var name = item.InvParentCateDescription || item.invParentCateDescription || item.Name || item.name;
                    $parentCat.append('<option value="' + id + '">' + name + '</option>');
                });
            }

            // Populate Types
            var $type = $('#cmbItemTypeType');
            $type.empty();
            $type.append('<option value="">-- Select Type --</option>');
            if (res.itemTypes) {
                $.each(res.itemTypes, function (i, item) {
                    var id = item.Id || item.id;
                    var name = item.LookupName || item.lookupName || item.Name || item.name;
                    $type.append('<option value="' + id + '">' + name + '</option>');
                });
            }
        },
        error: function (err) {
            console.error('Error loading lookups:', err);
        }
    });
}

function loadHistory() {
    $.ajax({
        url: '/api/inventory/item_types/history',
        type: 'GET',
        success: function (list) {
            var $tbody = $('#historyTableBody');
            $tbody.empty();
            if (!list || list.length === 0) {
                $tbody.append('<tr><td colspan="6" class="text-center text-muted">No records found</td></tr>');
                return;
            }
            $.each(list, function (i, item) {
                var id = item.Id || item.id;
                var code = item.TypeCode || item.typeCode || '';
                var desc = item.TypeDescription || item.typeDescription || '';
                var typeName = item.TypeName || item.LookupName || item.Type || '';
                var parentName = item.ParentCategoryName || item.InvParentCateDescription || item.ParentCategoryId || '';
                var isMother = (item.IsMother === true || item.IsMother === 1 || item.isMother === true) ? 'Yes' : 'No';

                var $tr = $('<tr style="cursor:pointer;" title="Double-click to edit"></tr>');
                $tr.append('<td>' + id + '</td>');
                $tr.append('<td>' + code + '</td>');
                $tr.append('<td>' + desc + '</td>');
                $tr.append('<td>' + typeName + '</td>');
                $tr.append('<td>' + parentName + '</td>');
                $tr.append('<td>' + isMother + '</td>');

                $tr.on('dblclick', function () {
                    loadRecordForEdit(id);
                });

                $tbody.append($tr);
            });
        },
        error: function (err) {
            console.error('Error loading history:', err);
        }
    });
}

function loadRecordForEdit(id) {
    $.ajax({
        url: '/api/inventory/item_types/' + id,
        type: 'GET',
        success: function (item) {
            if (item) {
                var recId = item.Id || item.id;
                var code = item.TypeCode || item.typeCode || '';
                var desc = item.TypeDescription || item.typeDescription || '';
                var typeVal = item.Type !== undefined ? item.Type : (item.type !== undefined ? item.type : '');
                var parentVal = item.ParentCategoryId !== undefined ? item.ParentCategoryId : (item.parentCategoryId !== undefined ? item.parentCategoryId : '');
                var isMother = (item.IsMother === true || item.IsMother === 1 || item.isMother === true);

                $('#recId').val(recId);
                $('#txtItemTypeCode').val(code);
                $('#txtTypeDescription').val(desc);
                $('#cmbItemTypeType').val(typeVal);
                $('#cmbParentCategory').val(parentVal);
                $('#chkIsMother').prop('checked', isMother);

                $('#btnSave').hide();
                $('#btnUpdate').show();
            }
        },
        error: function (err) {
            console.error('Error loading record:', err);
        }
    });
}

function saveRecord(isUpdate) {
    var recId = parseInt($('#recId').val()) || 0;
    var code = $('#txtItemTypeCode').val().trim();
    var desc = $('#txtTypeDescription').val().trim();
    var typeVal = parseInt($('#cmbItemTypeType').val()) || 0;
    var parentVal = parseInt($('#cmbParentCategory').val()) || 0;
    var isMother = $('#chkIsMother').is(':checked');

    if (!desc) {
        alert('Please enter Item Type Description');
        $('#txtTypeDescription').focus();
        return;
    }

    if (isUpdate && recId === 0) {
        alert('Invalid Record ID for Update');
        return;
    }

    var payload = {
        id: isUpdate ? recId : 0,
        typeCode: code,
        typeDescription: desc,
        type: typeVal,
        parentCategoryId: parentVal,
        isMother: isMother
    };

    $.ajax({
        url: '/api/inventory/item_types/save',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (res) {
            alert(isUpdate ? 'Item Type updated successfully!' : 'Item Type saved successfully!');
            resetForm();
            loadHistory();
        },
        error: function (err) {
            console.error('Error saving item type:', err);
            alert('Failed to save Item Type. See console log for details.');
        }
    });
}
