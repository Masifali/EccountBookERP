/* Screen 156 - Item Analysis Parameter. Desktop: Architecture.WinApp.Lab.InvLabAnalysisItems.
 *
 * Every behaviour below is one the desktop form has; nothing else was invented.
 *
 *   refresh()                  :105-126   clear the entry fields, show Save / hide Update, refill grid
 *   Insert()                   :128-183   one save path for both buttons, id decides which
 *   btnsave_Click              :184-196   RecId = 0 first, then Insert()
 *   btnupdate_Click            :197-208   Insert() without clearing RecId
 *   btnnew_Click               :209-213   refresh()
 *   gridfill()                 :214-252   ReadAll, then bind the Parent Parameter combo from it
 *   grdfrm_DoubleClick         :273-304   load a row, hide Save, show Update
 *   MasterParameters()         :305-320   usp_getLabMasterParms
 *   ChkIsSub_CheckedChanged    :398-417   show/hide the Parent Parameter combo and its label
 *   txtMin/MaxValue_KeyPress   :456-478   CommonServices.OnlytextdecimelFunction
 *
 * The confirm prompts at :137 and :146 ("Are you sure to Update?" / "Are you sure to Save?") are
 * reproduced, because a user who cancels there expects nothing to be written.
 */
var LabAnalysisItems = (function () {
    'use strict';

    var API = '/api/lab/item-analysis-parameter';

    /* The desktop's RecId. 0 means insert; anything else means update. It is only ever set from a
       row the grid already returned, never typed. */
    var RecId = 0;
    var masterParameterRows = [];
    var parentParameterRows = [];

    // -----------------------------------------------------------------------------------------
    // small helpers
    // -----------------------------------------------------------------------------------------

    function status(message, isError) {
        var $s = $('#statusLine');
        $s.text(message || '');
        $s.css('color', isError ? '#c00' : '#004d40');
    }

    /* Disable on click, keep disabled while the request is in flight, re-enable on success AND on
       failure. Applied to both toolbar buttons. */
    function busy($btn, on) {
        if (!$btn || !$btn.length) { return; }
        $btn.prop('disabled', !!on).toggleClass('is-busy', !!on);
    }

    function text(v) { return (v === null || v === undefined) ? '' : String(v); }

    function esc(v) {
        return text(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
                      .replace(/"/g, '&quot;');
    }

    /* Conversion.ToDouble("") is 0 in the desktop, never null - an empty box saves as 0. */
    function toDouble(v) {
        var n = parseFloat(String(v === null || v === undefined ? '' : v).trim());
        return isNaN(n) ? 0 : n;
    }

    function toInt(v) {
        var n = parseInt(String(v === null || v === undefined ? '' : v).trim(), 10);
        return isNaN(n) ? 0 : n;
    }

    function fillCombo($select, rows, placeholder, selectedId) {
        var html = '<option value="0">' + esc(placeholder) + '</option>';
        for (var i = 0; i < rows.length; i++) {
            html += '<option value="' + esc(rows[i].id) + '">' + esc(rows[i].description) + '</option>';
        }
        $select.html(html);
        $select.val(String(toInt(selectedId) || 0));
    }

    // -----------------------------------------------------------------------------------------
    // ChkIsSub_CheckedChanged (:398-417)
    // -----------------------------------------------------------------------------------------

    function ChkIsSub_CheckedChanged() {
        var on = $('#ChkIsSub').is(':checked');
        $('#rowParentParameter').toggle(on);
        if (!on) {
            /* The desktop only hides the combo; it does not clear it. But Insert() writes 0 for
               ParentParameterId whenever IsSub is false (:158-165), so what the hidden combo holds
               never reaches the database either way. The value is left alone to match. */
            $('#CmbParentParameter').removeClass('is-invalid');
        }
    }

    // -----------------------------------------------------------------------------------------
    // gridfill (:214-252) + MasterParameters (:305-320)
    // -----------------------------------------------------------------------------------------

    function renderGrid(rows) {
        var $body = $('#grdfrmBody');
        if (!rows || !rows.length) {
            $body.html('<tr><td colspan="5" style="text-align:center;color:#777">No analysis parameters defined.</td></tr>');
            $('#lblRowCount').text('0 rows');
            return;
        }
        var html = '';
        for (var i = 0; i < rows.length; i++) {
            var r = rows[i];
            html += '<tr class="data-row" data-id="' + esc(r.id) + '">'
                 +  '<td>' + esc(r.analysisParameter) + '</td>'
                 +  '<td>' + esc(r.parentParameter) + '</td>'
                 +  '<td>' + esc(r.masterParameter) + '</td>'
                 +  '<td class="num">' + esc(r.minValue) + '</td>'
                 +  '<td class="num">' + esc(r.maxValue) + '</td>'
                 +  '</tr>';
        }
        $body.html(html);
        $('#lblRowCount').text(rows.length + (rows.length === 1 ? ' row' : ' rows'));
    }

    function gridfill() {
        return $.getJSON(API + '/grid').done(function (rows) {
            renderGrid(rows);
            /* The desktop binds the Parent Parameter combo from the grid's own table, and only
               when it has rows (:245-249). Same source, same condition. */
            parentParameterRows = (rows || []).map(function (r) {
                return { id: r.id, description: r.analysisParameter };
            });
            fillCombo($('#CmbParentParameter'), parentParameterRows, 'Parent Parameter',
                      $('#CmbParentParameter').val());
        }).fail(function (xhr) {
            $('#grdfrmBody').html('<tr><td colspan="5" style="text-align:center;color:#c00">'
                + esc(errorOf(xhr, 'The analysis parameter list could not be read.')) + '</td></tr>');
        });
    }

    function loadMasterParameters() {
        return $.getJSON(API + '/master-parameters').done(function (rows) {
            masterParameterRows = rows || [];
            fillCombo($('#CmbMasterParameter'), masterParameterRows, 'Master Parameter', 0);
        }).fail(function (xhr) {
            status(errorOf(xhr, 'The Master Parameter list could not be read.'), true);
        });
    }

    function errorOf(xhr, fallback) {
        if (xhr && xhr.responseJSON && xhr.responseJSON.message) { return xhr.responseJSON.message; }
        if (xhr && xhr.responseText) {
            try {
                var j = JSON.parse(xhr.responseText);
                if (j && j.message) { return j.message; }
            } catch (ignored) { /* not json */ }
        }
        return fallback;
    }

    // -----------------------------------------------------------------------------------------
    // refresh (:105-126)
    // -----------------------------------------------------------------------------------------

    function refresh() {
        RecId = 0;
        $('#btnsave').show();
        $('#btnupdate').hide();
        $('#lblMode').text('New');
        $('#txtdescription').val('');
        $('#CmbParentParameter').val('0');
        $('#CmbMasterParameter').val('0');
        $('#txtMinValue').val('');
        $('#txtMaxValue').val('');
        $('#ChkIsSub').prop('checked', false);
        $('#rowParentParameter').hide();
        $('#grdfrmBody tr').removeClass('selected');
        status('');
        gridfill();
        $('#txtdescription').focus();
    }

    // -----------------------------------------------------------------------------------------
    // grdfrm_DoubleClick (:273-304)
    // -----------------------------------------------------------------------------------------

    function openRow(id) {
        $.getJSON(API + '/' + encodeURIComponent(id)).done(function (row) {
            if (!row) { return; }
            RecId = toInt(row.id);
            $('#btnsave').hide();
            $('#btnupdate').show();
            $('#lblMode').text('Update');
            $('#txtdescription').val(text(row.description));
            $('#ChkIsSub').prop('checked', row.isSub === true || row.isSub === 1);
            ChkIsSub_CheckedChanged();
            $('#CmbParentParameter').val(String(toInt(row.parentParameterId)));
            $('#CmbMasterParameter').val(String(toInt(row.masterParId)));
            $('#txtMinValue').val(text(row.minValue));
            $('#txtMaxValue').val(text(row.maxValue));
            $('#grdfrmBody tr').removeClass('selected');
            $('#grdfrmBody tr[data-id="' + row.id + '"]').addClass('selected');
            status('');
        }).fail(function (xhr) {
            status(errorOf(xhr, 'That analysis parameter could not be opened.'), true);
        });
    }

    // -----------------------------------------------------------------------------------------
    // formvalidation (:88-103) then Insert (:128-183)
    // -----------------------------------------------------------------------------------------

    function formvalidation() {
        if ($('#txtdescription').val().trim() === '') {
            status('Please Insert Description', true);
            $('#txtdescription').focus();
            return false;
        }
        if ($('#ChkIsSub').is(':checked') && toInt($('#CmbParentParameter').val()) <= 0) {
            status('Parent Parameter Field is Required', true);
            $('#CmbParentParameter').focus();
            return false;
        }
        return true;
    }

    function Insert() {
        if (!formvalidation()) { return; }

        var isUpdate = RecId > 0;
        if (!window.confirm(isUpdate ? 'Are you sure to Update?' : 'Are you sure to Save?')) {
            return;                                   // :137-141 / :146-149
        }

        var $btn = isUpdate ? $('#btnupdate') : $('#btnsave');
        busy($btn, true);
        status(isUpdate ? 'Updating...' : 'Saving...');

        var isSub = $('#ChkIsSub').is(':checked');
        var payload = {
            id: isUpdate ? RecId : 0,
            description: $('#txtdescription').val().trim(),
            isSub: isSub,
            /* :158-165 - the parent is only sent when Is Sub is on; otherwise the desktop
               explicitly stores 0 rather than whatever the hidden combo holds. */
            parentParameterId: isSub ? toInt($('#CmbParentParameter').val()) : 0,
            masterParId: toInt($('#CmbMasterParameter').val()),
            minValue: toDouble($('#txtMinValue').val()),
            maxValue: toDouble($('#txtMaxValue').val())
        };

        $.ajax({
            url: API + '/save',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload)
        }).done(function (res) {
            if (res && res.success) {
                status(res.message);                  // "Save Successfully" / "Update Successfully"
                refresh();                            // :175 - the desktop refreshes after both
            } else {
                status((res && res.message) || 'The analysis parameter was not saved.', true);
            }
        }).fail(function (xhr) {
            status(errorOf(xhr, 'The analysis parameter was not saved.'), true);
        }).always(function () {
            busy($btn, false);                        // re-enabled on success AND on failure
        });
    }

    function btnsave_Click() { RecId = 0; Insert(); }     // :184-196
    function btnupdate_Click() { Insert(); }              // :197-208
    function btnnew_Click() { refresh(); }                // :209-213

    // -----------------------------------------------------------------------------------------
    // wiring
    // -----------------------------------------------------------------------------------------

    $(function () {
        /* CommonServices.OnlytextdecimelFunction - digits and a single decimal point. */
        $('#txtMinValue, #txtMaxValue').on('keypress', function (e) {
            var ch = String.fromCharCode(e.which);
            if (e.which === 8 || e.which === 0 || e.which === 13) { return; }
            if (ch === '.' && String($(this).val()).indexOf('.') === -1) { return; }
            if (!/[0-9]/.test(ch)) { e.preventDefault(); }
        });

        $('#grdfrmBody').on('dblclick', 'tr.data-row', function () {
            openRow($(this).data('id'));
        });

        $('#txtdescription').on('keydown', function (e) {
            if (e.which === 13) { e.preventDefault(); $('#CmbMasterParameter').focus(); }
        });

        loadMasterParameters().always(function () { refresh(); });
    });

    return {
        btnsave_Click: btnsave_Click,
        btnupdate_Click: btnupdate_Click,
        btnnew_Click: btnnew_Click,
        ChkIsSub_CheckedChanged: ChkIsSub_CheckedChanged
    };
})();
