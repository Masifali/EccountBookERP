$(document).ready(function () {

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return year + '-' + month + '-' + day;
    }

    function getDateFromString(date) {
        return date.split('/')[2] + '-' + date.split('/')[1] + '-' + date.split('/')[0];
    }

    // update serial numbers
    $("table").each(function (i) {
        $(this).find("span.serial_no").each(function (index, element) {
            $(element).text(index + 1);
        });
    });

    // CLEAR COMPANY MODAL FORM
    $("#add_new_company").on("hidden.bs.modal", function () {
        $(this).find("form").trigger("reset");
    });
    // UPDATE COMPANY
    $(".edit_company").on("click", function () {

        // SET COMPANY FIELDS
        $("#addCompanyForm #id").val($(this).closest("tr").find("#company_id").val());
        $("#addCompanyForm #name").val($(this).closest("tr").find("td:eq(1)").text());
        $("#addCompanyForm #address").val($(this).closest("tr").find("td:eq(2)").text());
        $("#addCompanyForm #contactNo").val($(this).closest("tr").find("td:eq(3)").text());
        $("#addCompanyForm #email").val($(this).closest("tr").find("td:eq(4)").text());
        $("#addCompanyForm #website").val($(this).closest("tr").find("td:eq(5)").text());
        $("#addCompanyForm #calculateAvgRate").val($(this).closest("tr").find("td:eq(6)").text());
        $("#addCompanyForm #brokeryByMan").val($(this).closest("tr").find("td:eq(7)").text());
        $("#add_new_company").modal("show");

    });

    $("#company_table").on("click", ".delete_company", function (e) {
        //$(this).closest('tr').remove();
        var clickedControl = $(this);
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO DELETE THIS COMPANY?",
            buttons: {
                confirm: function () {
                    deleteThisCompany($(clickedControl));
                },
                cancel: function () {
                    // do nothing
                },
            }
        });
    });

    function deleteThisCompany(clickedControl) {
        companyId = $(clickedControl).closest("tr").find("#company_id").val();
        $.get("/utilities/delete_company?companyId=" + companyId, function (data) {
            if (data == true || data == "true") {
                location.reload();
            } else {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: "THIS COMPANY CANNOT BE DELETED...!!",
                    type: 'red',
                    typeAnimated: true,
                });
            }
        });
    }

    // CLEAR BRANCH MODAL FORM
    $("#add_new_branch").on("hidden.bs.modal", function () {
        $(this).find("form").trigger("reset");
    })
    // UPDATE BRANCH
    $(".edit_branch").on("click", function () {

        //$("#add_new_branch").find("input,textarea,select").val("").end();

        // SET BRANCH FIELDS
        $("#addBranchForm #company").val($(this).closest("tr").find("#company_id").val());
        $("#addBranchForm #id").val($(this).closest("tr").find("#branch_id").val());
        $("#addBranchForm #name").val($(this).closest("tr").find("td:eq(2)").text());
        $("#addBranchForm #address").val($(this).closest("tr").find("td:eq(3)").text());
        $("#addBranchForm #contactNo").val($(this).closest("tr").find("td:eq(4)").text());
        $("#addBranchForm #email").val($(this).closest("tr").find("td:eq(5)").text());
        $("#addBranchForm #website").val($(this).closest("tr").find("td:eq(6)").text());
        $("#add_new_branch").modal("show");

    });
    $(".edit_user").on("click", function () {

        //$("#add_new_branch").find("input,textarea,select").val("").end();

        // SET BRANCH FIELDS
        $("#addUserForm #id").val($(this).closest("tr").find(".user_id").val());
        $("#addUserForm #firstName").val($(this).closest("tr").find("td:eq(1)").text());
        $("#addUserForm #lastName").val($(this).closest("tr").find("td:eq(2)").text());
        $("#addUserForm #email").val($(this).closest("tr").find("td:eq(4)").text());
        $("#addUserForm #login").val($(this).closest("tr").find("td:eq(3)").text());
        console.log($(this).closest("tr").find("td:eq(0)").text());

        $("#add_new_user").modal("show");
        $(".checkboxes:checked").each(function () {
            var sThisVal = (this.checked ? $(this).text(false) : "");
            $(this).prop("checked", false);
            // console.log($(this).val()+"  asif");
        });
        $.get("/utilities/user_permission?login=" + $("#addUserForm #login").val(), function (data) {
            console.log(data.labels);
            $.each(data.labels, function (i, labes) {
                $(".checkboxes:input").each(function () {

                    var sd = $(this).val();
                    sd = parseFloat(sd);
                    if (sd == parseFloat(labes.id)) {
                        $(this).prop("checked", true);
                        //console.log($(this).val()+"  asif");
                    }
                });
            });

        });
    });
    $("#branch_table").on("click", ".delete_branch", function (e) {
        //$(this).closest('tr').remove();
        var clickedControl = $(this);
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO DELETE THIS BRANCH?",
            buttons: {
                confirm: function () {
                    deleteThisBranch($(clickedControl));
                },
                cancel: function () {
                    // do nothing
                },
            }
        });
    });

    function deleteThisBranch(clickedControl) {
        branchId = $(clickedControl).closest("tr").find("#branch_id").val();
        $.get("/utilities/delete_branch?branchId=" + branchId, function (data) {
            if (data == true || data == "true") {
                location.reload();
            } else {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: "THIS BRANCH CANNOT BE DELETED...!!",
                    type: 'red',
                    typeAnimated: true,
                });
            }
        });
    }

    // CLEAR FINANCIAL YEAR MODAL FORM
    $("#add_item_def").on("hidden.bs.modal", function () {
        $(this).find("form").trigger("reset");
    });

    // GET NEW FINANCIAL YEAR MODAL FORM
    $("#getNewFinancialYear").on("click", function () {
        $("#month_checkboxes").empty();
        $(this).find("form").trigger("reset");

        $.get("/utilities/get_new_financial_year", function (data) {
            document.getElementById("fromDate").valueAsDate = new Date(data.fromDate);
            document.getElementById("toDate").valueAsDate = new Date(data.toDate);

            $("#month_checkboxes").empty();
            monthsBetweenDates(new Date($("#fromDate").val()), new Date($("#toDate").val()));
        });
    });

    // UPDATE FINANCIAL YEAR
    $(".edit_financialYear").on("click", function () {

        // SET BRANCH FIELDS
        $("#addFinancialYearForm #id").val($(this).closest("tr").find("#financialYear_id").val());
        document.getElementById("fromDate").valueAsDate = new Date(getDateFromString($(this).closest("tr").find("td:eq(2)").text()));
        document.getElementById("toDate").valueAsDate = new Date(getDateFromString($(this).closest("tr").find("td:eq(3)").text()));

        $("#month_checkboxes").empty();
        $("#month_checkboxes").append($(this).closest("tr").find("td:eq(4)").html());

        $("#month_checkboxes input").each(function () {
            $(this).attr("disabled", false);
        });

        $("#add_new_financial_year").modal("show");
    });

    var monthNames = ["JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"];

    function monthsBetweenDates(fromDate, toDate) {
        var arr = [];
        var fromYear = fromDate.getFullYear();
        var toYear = toDate.getFullYear();
        var diffYear = (12 * (toYear - fromYear)) + toDate.getMonth();

        for (var i = fromDate.getMonth(); i <= diffYear; i++) {
            var monthName = monthNames[i % 12] + " " + Math.floor(fromYear + (i / 12));
            arr.push(monthName);

            var month = (i % 12) + 1;
            month = month.toString().length > 1 ? month : '0' + month;
            $("#month_checkboxes").append("<label><input type=checkbox value=" + Math.floor(fromYear + (i / 12)) + "-" + month + "-01" + " checked/> " + monthName + "</label><br/>");
        }

        return arr;
    }

    $("#addFinancialYearForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        addOrUpdateFinancialYear();
    });

    function addOrUpdateFinancialYear() {

        var financialYearMonths = [];
        $("#month_checkboxes input").each(function () {
            var financialYearMonth = {
                id: $(this).attr("id") ? $(this).attr("id") : 0,
                monthYear: $(this).val(),
                active: $(this).is(":checked") ? true : false,
            }
            financialYearMonths.push(financialYearMonth);
        });

        // PREPARE FORM DATA
        var financialYear = {
            id: $("#addFinancialYearForm #id").val(),
            name: "",
            fromDate: $("#addFinancialYearForm #fromDate").val(),
            toDate: $("#addFinancialYearForm #toDate").val(),
            financialYearMonths: financialYearMonths,
        }
        //return;
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $('#addFinancialYearForm').attr("action"),
            data: JSON.stringify(financialYear),
            dataType: "json",
            success: function () {
                $("#add_new_financial_year").modal("hide");
                location.reload();
            },
            complete: function () {
                $("#add_new_financial_year").modal("hide");
                location.reload();
            },
        });
    }

    $("#financial_year_table").on("click", ".delete_financialYear", function (e) {
        var clickedControl = $(this);
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO DELETE THIS FINANCIAL YEAR?",
            buttons: {
                confirm: function () {
                    deleteThisFinancialYear($(clickedControl));
                },
                cancel: function () {
                    // do nothing
                },
            }
        });
    });

    function deleteThisFinancialYear(clickedControl) {
        financialYearId = $(clickedControl).closest("tr").find("#financialYear_id").val();
        $.get("/utilities/delete_financial_year?financialYearId=" + financialYearId, function (data) {
            if (data == true || data == "true") {
                location.reload();
            } else {
                $.confirm({
                    title: "ENCOUNTERED AN ERROR!",
                    content: "THIS FINANCIAL YEAR CANNOT BE DELETED...!!",
                    type: 'red',
                    typeAnimated: true,
                });
            }
        });
    }

})