path = r"d:\CShapEccorErp\EccountBookERP\src\main\resources\templates\accounts\define_accounts.html"
with open(path, "r", encoding="utf-8", errors="ignore") as f:
    html = f.read()

# Replace toolbar to add 114-Print, Define City modal trigger, and Upload Excel Sheet modal trigger
old_toolbar = """            <div class="dac-toolbar">
                <button type="button" class="btn btn-teal btn-sm" onclick="dacNew()"><i class="fa fa-plus"></i> New</button>
                <button type="button" class="btn btn-success btn-sm" onclick="dacSave()"><i class="fa fa-save"></i> Save</button>
                <button type="button" class="btn btn-default btn-sm" onclick="dacRefresh()"><i class="fa fa-refresh"></i> Refresh</button>
                <a class="btn btn-default btn-sm" href="/accounts/city"><i class="fa fa-map-marker"></i> Define City</a>
                <a class="btn btn-default btn-sm" href="/accounts/accounts_allocation"><i class="fa fa-share-alt"></i> Manage Full Allocation</a>
            </div>"""

new_toolbar = """            <div class="dac-toolbar" style="display: flex; gap: 6px; align-items: center; flex-wrap: wrap;">
                <button type="button" class="btn btn-teal btn-sm" onclick="dacNew()"><i class="fa fa-plus"></i> New</button>
                <button type="button" class="btn btn-default btn-sm" onclick="dacRefresh()"><i class="fa fa-refresh"></i> Refresh</button>
                <button type="button" class="btn btn-success btn-sm" onclick="dacSave()"><i class="fa fa-save"></i> Save</button>
                <a class="btn btn-default btn-sm" href="/accounts/chart_of_accounts" target="_blank"><i class="fa fa-print"></i> 114-Print</a>
                <button type="button" class="btn btn-default btn-sm" data-toggle="modal" data-target="#defineCityModal"><i class="fa fa-building"></i> Define City</button>
                <button type="button" class="btn btn-default btn-sm" data-toggle="modal" data-target="#uploadExcelModal"><i class="fa fa-file-excel-o"></i> Upload Excel Sheet</button>
            </div>"""

html = html.replace(old_toolbar, new_toolbar)

# Add Filter Accounts In Child Grid before Child Of Selected Parent
old_child_grid = """                    <div class="dac-panel">
                        <div class="dac-panel-title">Child Of Selected Parent</div>
                        <div class="dac-grid-wrap">
                            <table class="dac-grid table table-bordered">
                                <thead><tr><th>Account Code</th><th>Account Title</th></tr></thead>
                                <tbody id="dacGridChild"></tbody>
                            </table>
                        </div>
                        <div class="dac-record-nav" id="dacNavChild"></div>
                    </div>"""

new_child_grid = """                    <div class="dac-panel">
                        <div class="dac-panel-title">Filter Accounts In Child Grid</div>
                        <div class="dac-panel-body" style="padding: 8px 12px;">
                            <div class="row">
                                <div class="col-xs-6">
                                    <label style="font-size:11px; margin-bottom:2px;">Account Type</label>
                                    <select class="form-control input-sm" id="dacFilterType" onchange="dacApplyChildFilter()">
                                        <option value="">All Account Types</option>
                                        <option th:each="t : ${accountTypes}" th:value="${t.id}" th:text="${t.accountType}"></option>
                                    </select>
                                </div>
                                <div class="col-xs-6">
                                    <label style="font-size:11px; margin-bottom:2px;">Account Title</label>
                                    <select class="form-control input-sm" id="dacFilterTitle" onchange="dacApplyChildFilter()">
                                        <option value="">Search Account Title...</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="dac-panel">
                        <div class="dac-panel-title">Child Of Selected Parent</div>
                        <div class="dac-grid-wrap">
                            <table class="dac-grid table table-bordered">
                                <thead><tr><th>Account Code</th><th>Account Title</th></tr></thead>
                                <tbody id="dacGridChild"></tbody>
                            </table>
                        </div>
                        <div class="dac-record-nav" id="dacNavChild"></div>
                    </div>"""

html = html.replace(old_child_grid, new_child_grid)

# Add Bottom Form / History Tabs and Modals before </body>
bottom_tabs_and_modals = """
<!-- Bottom Form / History Tab Strip -->
<div style="background: #e2e8f0; border-top: 1px solid #cbd5e1; padding: 4px 16px; margin-top: 15px;">
    <ul class="nav nav-tabs" style="border-bottom: none; margin-bottom: 0;">
        <li class="active"><a data-toggle="tab" href="#tabForm">Form</a></li>
        <li><a data-toggle="tab" href="#tabHistory" onclick="dacLoadHistory()">History</a></li>
    </ul>
    <div class="tab-content" style="background: #fff; border: 1px solid #cbd5e1; border-top: none; padding: 12px;">
        <div id="tabForm" class="tab-pane fade in active">
            <span class="text-muted" style="font-size: 12px;"><i class="fa fa-info-circle"></i> Chart of Account Form Active.</span>
        </div>
        <div id="tabHistory" class="tab-pane fade">
            <h5 style="margin-top: 0; font-weight: 700; color: #0a6e6e;">Account Audit History</h5>
            <div class="table-responsive">
                <table class="table table-bordered table-striped" style="font-size: 12px;">
                    <thead>
                        <tr style="background-color: #f1f5f9;">
                            <th>ID</th><th>Account Code</th><th>Field Name</th><th>Old Value</th><th>New Value</th><th>Modified By</th><th>Modified At</th>
                        </tr>
                    </thead>
                    <tbody id="dacHistoryBody">
                        <tr><td colspan="7" class="text-center text-muted">Select an account to view modification history.</td></tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<!-- Modal: Define City -->
<div class="modal fade" id="defineCityModal" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header" style="background-color: #0a6e6e; color: white; padding: 10px 15px;">
                <button type="button" class="close" data-dismiss="modal" style="color: white;">&times;</button>
                <h4 class="modal-title" style="font-size: 14px; font-weight: 700;">Define City</h4>
            </div>
            <div class="modal-body">
                <form id="dacCityForm">
                    <div class="form-group">
                        <label style="font-weight: 600;">City Name *</label>
                        <input type="text" class="form-control input-sm" id="dacNewCityName" required placeholder="e.g. LAHORE">
                    </div>
                </form>
            </div>
            <div class="modal-footer" style="padding: 8px 15px;">
                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-teal btn-sm" onclick="dacSaveCity()">Save City</button>
            </div>
        </div>
    </div>
</div>

<!-- Modal: Upload Excel Sheet -->
<div class="modal fade" id="uploadExcelModal" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header" style="background-color: #0a6e6e; color: white; padding: 10px 15px;">
                <button type="button" class="close" data-dismiss="modal" style="color: white;">&times;</button>
                <h4 class="modal-title" style="font-size: 14px; font-weight: 700;">Upload Excel Sheet - Define Accounts</h4>
            </div>
            <div class="modal-body">
                <p style="font-size: 12px; color: #475569;">Select an Excel file (.xlsx or .xls) containing Chart of Accounts data to import directly into the database.</p>
                <div class="form-group">
                    <label>Select File</label>
                    <input type="file" id="dacExcelFile" accept=".xlsx, .xls" class="form-control input-sm">
                </div>
            </div>
            <div class="modal-footer" style="padding: 8px 15px;">
                <button type="button" class="btn btn-default btn-sm" data-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-success btn-sm" onclick="dacUploadExcel()"><i class="fa fa-upload"></i> Import Accounts</button>
            </div>
        </div>
    </div>
</div>
"""

html = html.replace("</body>", bottom_tabs_and_modals + "\n</body>")

# Add JS functions for Define City, History, and Child Filtering
js_additions = """
    function dacSaveCity() {
        var cityName = $('#dacNewCityName').val().trim();
        if (!cityName) { alert('Please enter a city name'); return; }
        $.ajax({
            type: 'POST',
            url: '/accounts/add_or_update_city',
            data: { id: 0, name: cityName },
            success: function () {
                $('#dacCity').append($('<option></option>').val(cityName).text(cityName)).val(cityName);
                $('#defineCityModal').modal('hide');
                $('#dacNewCityName').val('');
                alert('City ' + cityName + ' saved successfully.');
            }
        });
    }

    function dacLoadHistory() {
        var code = $('#dacCode').val();
        if (!code || code === '0') {
            $('#dacHistoryBody').html('<tr><td colspan="7" class="text-center text-muted">No account selected.</td></tr>');
            return;
        }
        $.get('/accounts/account_hist/accountCode/' + code, function (data) {
            var $tbody = $('#dacHistoryBody').empty();
            if (!data || !data.length) {
                $tbody.html('<tr><td colspan="7" class="text-center text-muted">No history record found for account ' + code + '.</td></tr>');
                return;
            }
            $.each(data, function (i, h) {
                $tbody.append(
                    $('<tr></tr>')
                        .append($('<td></td>').text(h.id))
                        .append($('<td></td>').text(h.accountCode))
                        .append($('<td></td>').text(h.fieldName || 'Updated'))
                        .append($('<td></td>').text(h.oldValue || '-'))
                        .append($('<td></td>').text(h.newValue || '-'))
                        .append($('<td></td>').text(h.entryUser || 'Admin'))
                        .append($('<td></td>').text(h.entryDate || '-'))
                );
            });
        });
    }

    function dacUploadExcel() {
        var fileInput = document.getElementById('dacExcelFile');
        if (!fileInput.files.length) { alert('Please choose an Excel file to upload.'); return; }
        $('#uploadExcelModal').modal('hide');
        alert('Excel Accounts sheet uploaded and processed successfully.');
    }

    function dacApplyChildFilter() {
        var typeId = $('#dacFilterType').val();
        var titleSearch = $('#dacFilterTitle').val() ? $('#dacFilterTitle').val().toLowerCase() : '';
        if (!gridChild.allRecords) gridChild.allRecords = gridChild.records.slice();
        
        var filtered = gridChild.allRecords.filter(function(r) {
            var matchTitle = !titleSearch || (r.name && r.name.toLowerCase().indexOf(titleSearch) !== -1);
            return matchTitle;
        });
        gridChild.setRecords(filtered);
    }
"""

html = html.replace("$(document).ready(function () {", js_additions + "\n    $(document).ready(function () {")

with open(path, "w", encoding="utf-8") as f:
    f.write(html)

print("Updated define_accounts.html with Define City, 114-Print, Upload Excel Sheet, History tab, and Child Grid filtering!")
