/*
 * Configuration.cs - tabWages / PanelWages02 "Document Wise Wages Configuration".
 *
 *   GetRefDocumentsForWages()   (Configuration.cs:2823, called from the form Load)
 *     InvContractorWagesBillHeader.GetRefDocumentsForWages(0) -> USP_GetRefDocumentsForWages
 *     with @RefDocumentTypeId omitted. The grid is rebuilt from 4 columns
 *     (Id, RefDocumentTypeId, DocumentTypeDescription, IsActive); Id and RefDocumentTypeId are
 *     hidden, DocumentTypeDescription is read-only, IsActive is the editable checkbox column.
 *     Captions are the raw column names (GridEX RetrieveStructure).
 *
 *   btnStatusUpdate_Click()     (Configuration.cs:3739)
 *     rows > 0 and "Are you sure to Update Status?" (Yes/No) -> one
 *     USP_ContractorWagesRefDocumentStatus_Insert per grid row -> "Record's Status Updated
 *     Successfully" -> GetRefDocumentsForWages() again. Errors show ex.Message.
 *
 * Backend: ConfigurationWagesStatusController (/api/configurations/wages-documents).
 */
(function () {
    "use strict";

    var API_URL = "/api/configurations/wages-documents";
    var updating = false;

    function esc(v) { return $("<div>").text(v == null ? "" : String(v)).html(); }
    function pick(row, a, b) { return row[a] !== undefined ? row[a] : row[b]; }
    function toBool(v) { return v === true || v === 1 || v === "1" || v === "true" || v === "True"; }

    function showStatusMessage(msg, isError) {
        $("#wagesStatusMessage").text(msg || "")
            .css({ color: isError ? "#c92a2a" : "#2b8a3e", "font-weight": "600" });
    }

    function renderNavigator() {
        var n = $("#grdWagesRefDocuments tbody tr[data-row]").length;
        var i = $("#grdWagesRefDocuments tbody tr.is-current").index() + 1;
        $("#wagesDocsNavigator").text("Record " + (n ? Math.max(i, 1) : 0) + " of " + n);
        $("#wagesDocsTotal").text(n ? "Total: " + n : "");
    }

    function renderTable(rows) {
        var $tbody = $("#grdWagesRefDocuments tbody").empty();
        // Desktop: when USP_GetRefDocumentsForWages returns no rows the grid is simply left empty.
        $.each(rows || [], function (idx, row) {
            var id = pick(row, "Id", "id");
            var typeId = pick(row, "RefDocumentTypeId", "refDocumentTypeId");
            var desc = pick(row, "DocumentTypeDescription", "documentTypeDescription");
            var active = toBool(pick(row, "IsActive", "isActive"));
            $tbody.append(
                '<tr data-row="1"' + (idx === 0 ? ' class="is-current"' : '') + '>' +
                '<td>' + esc(desc) + '</td>' +
                '<td class="cfg-col-check"><input type="checkbox" class="wages-status-check"' +
                ' data-id="' + esc(id) + '" data-type-id="' + esc(typeId) + '"' +
                (active ? ' checked' : '') + ' aria-label="IsActive - ' + esc(desc) + '"></td></tr>');
        });
        renderNavigator();
    }

    function loadWagesStatus() {
        return $.ajax({ url: API_URL, type: "GET", dataType: "json" })
            .done(function (data) { renderTable(data); })
            .fail(function (xhr) {
                renderTable([]);
                showStatusMessage((xhr.responseJSON && xhr.responseJSON.message) || "Failed to load document statuses", true);
            });
    }

    function setBusy(busy) {
        $("#btnStatusUpdate").prop("disabled", busy).toggleClass("is-loading", busy)
            .attr("aria-busy", busy ? "true" : null);
        $("#grdWagesRefDocuments .wages-status-check").prop("disabled", busy);
    }

    function updateWagesStatus() {
        if (updating) return;                       // no duplicate requests
        var payload = $("#grdWagesRefDocuments tbody .wages-status-check").map(function () {
            var $c = $(this);
            return {
                id: parseInt($c.attr("data-id"), 10),
                refDocumentTypeId: parseInt($c.attr("data-type-id"), 10),
                isActive: $c.is(":checked")
            };
        }).get();
        // Desktop: grdWagesRefDocuments.GetRows().Count() > 0 && Yes/No confirm - else nothing.
        if (!payload.length) return;
        if (!window.confirm("Are you sure to Update Status?")) return;

        updating = true;
        setBusy(true);
        showStatusMessage("", false);
        $.ajax({ url: API_URL, type: "POST", contentType: "application/json", data: JSON.stringify(payload) })
            .done(function (res) {
                var msg = (res && res.message) || "Record's Status Updated Successfully";
                showStatusMessage(msg, false);
                window.alert(msg);
                return loadWagesStatus();
            })
            .fail(function (xhr) {
                var err = (xhr.responseJSON && (xhr.responseJSON.message || xhr.responseJSON.error)) ||
                          "Failed to update status records";
                showStatusMessage(err, true);
                window.alert(err);
            })
            .always(function () { updating = false; setBusy(false); });
    }

    $(document).ready(function () {
        loadWagesStatus();
        $("#btnStatusUpdate").on("click", function (e) { e.preventDefault(); updateWagesStatus(); });
        // Current-row marker for the record navigator (GridEX RecordNavigator).
        $("#grdWagesRefDocuments").on("click focusin", "tbody tr[data-row]", function () {
            $(this).addClass("is-current").siblings().removeClass("is-current");
            renderNavigator();
        });
    });
})();
