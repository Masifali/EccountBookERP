/*
 * Client-side handler for the "Document Wise Wages Configuration" status grid on tabWages.
 * Backed by ConfigurationWagesStatusController (/api/configurations/wages-documents).
 */
(function () {
    "use strict";

    var API_URL = "/api/configurations/wages-documents";
    var cachedRows = [];

    function showStatusMessage(msg, isError) {
        var $msg = $("#wagesStatusMessage");
        if ($msg.length) {
            $msg.text(msg)
                .css("color", isError ? "#c92a2a" : "#2b8a3e")
                .css("font-weight", "600");
        }
    }

    function renderTable(rows) {
        cachedRows = rows || [];
        var $tbody = $("#grdWagesRefDocuments tbody");
        $tbody.empty();

        if (!rows || rows.length === 0) {
            $tbody.append('<tr><td colspan="2" style="text-align:center;padding:12px;color:#868e96;">No reference document statuses found</td></tr>');
            return;
        }

        $.each(rows, function (idx, row) {
            var id = row.Id !== undefined ? row.Id : row.id;
            var refTypeId = row.RefDocumentTypeId !== undefined ? row.RefDocumentTypeId : row.refDocumentTypeId;
            var docDesc = row.DocumentTypeDescription || row.documentTypeDescription || ("Document #" + refTypeId);
            var isActive = (row.IsActive !== undefined ? row.IsActive : row.isActive);
            if (isActive === 1 || isActive === true || isActive === "true") {
                isActive = true;
            } else {
                isActive = false;
            }

            var tr = '<tr style="border-bottom:1px solid #dee2e6;">' +
                     '  <td style="padding:8px 10px;">' + $("<div>").text(docDesc).html() + '</td>' +
                     '  <td style="padding:8px 10px;text-align:center;">' +
                     '    <input type="checkbox" class="wages-status-check" ' +
                     '           data-id="' + id + '" ' +
                     '           data-type-id="' + refTypeId + '" ' +
                     (isActive ? 'checked="checked"' : '') + ' />' +
                     '  </td>' +
                     '</tr>';
            $tbody.append(tr);
        });
    }

    function loadWagesStatus() {
        showStatusMessage("Loading status records...", false);
        $.ajax({
            url: API_URL,
            type: "GET",
            dataType: "json",
            success: function (data) {
                renderTable(data);
                showStatusMessage("", false);
            },
            error: function (xhr) {
                var err = "Failed to load document statuses";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    err = xhr.responseJSON.message;
                }
                showStatusMessage(err, true);
            }
        });
    }

    function updateWagesStatus() {
        var payload = [];
        $("#grdWagesRefDocuments tbody tr").each(function () {
            var $chk = $(this).find(".wages-status-check");
            if ($chk.length) {
                var id = parseInt($chk.attr("data-id"), 10);
                var refTypeId = parseInt($chk.attr("data-type-id"), 10);
                var isActive = $chk.is(":checked");
                payload.push({
                    id: id,
                    refDocumentTypeId: refTypeId,
                    isActive: isActive
                });
            }
        });

        if (payload.length === 0) {
            showStatusMessage("No rows to update", true);
            return;
        }

        var $btn = $("#btnStatusUpdate");
        $btn.prop("disabled", true);
        showStatusMessage("Updating status...", false);

        $.ajax({
            url: API_URL,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function (res) {
                $btn.prop("disabled", false);
                var msg = (res && res.message) ? res.message : "Record's Status Updated Successfully";
                showStatusMessage(msg, false);
                loadWagesStatus();
            },
            error: function (xhr) {
                $btn.prop("disabled", false);
                var err = "Failed to update status records";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    err = xhr.responseJSON.message;
                }
                showStatusMessage(err, true);
            }
        });
    }

    $(document).ready(function () {
        loadWagesStatus();
        $("#btnStatusUpdate").on("click", function (e) {
            e.preventDefault();
            updateWagesStatus();
        });
        $("#btnWagesStatusRefresh").on("click", function (e) {
            e.preventDefault();
            loadWagesStatus();
        });
    });
})();
