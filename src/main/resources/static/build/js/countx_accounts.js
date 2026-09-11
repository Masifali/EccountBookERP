window.onload = function () {
    //loadTwoLevelAccounts();
    $('#accountDataDiv').hide();
};

function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

function loadTwoLevelAccounts() {

    // clear drop downs
    $("#twoLevelAccounts").empty();
    $("#twoLevelAccounts").append("<option></option>");
    $("#threeLevelAccounts").empty();
    $("#threeLevelAccounts").append("<option></option>");
    $("#fourLevelAccounts").empty();
    $("#fourLevelAccounts").append("<option></option>");
    $('#accountDataDiv').hide();

    var oneLevelAccountCode = $("#oneLevelAccounts").val();

    $.get("/accounts/two_level_accounts?parentCode=" + oneLevelAccountCode, function (data) {

        $.each(data.accountLevelResults, function (i, record) {
            var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
            $("#twoLevelAccounts").append(option);
        });

    });
}

function loadThreeLevelAccounts() {

    // clear drop downs
    $("#threeLevelAccounts").empty();
    $("#threeLevelAccounts").append("<option></option>");
    $("#fourLevelAccounts").empty();
    $("#fourLevelAccounts").append("<option></option>");
    $('#accountDataDiv').hide();

    var twoLevelAccountCode = $("#twoLevelAccounts").val();

    $.get("/accounts/three_level_accounts?parentCode=" + twoLevelAccountCode, function (data) {

        $.each(data.accountLevelResults, function (i, record) {
            var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
            $("#threeLevelAccounts").append(option);
        });

    });
}

function loadFourLevelAccounts() {

    // clear drop downs
    $("#fourLevelAccounts").empty();
    $("#fourLevelAccounts").append("<option></option>");
    $('#accountDataDiv').hide();

    var threeLevelAccountCode = $("#threeLevelAccounts").val();

    $.get("/accounts/four_level_accounts?parentCode=" + threeLevelAccountCode, function (data) {

        $.each(data.accountLevelResults, function (i, record) {
            var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
            $("#fourLevelAccounts").append(option);
        });

    });
}

function showAccountDataForm() {

    var oneLevelAccountCode = $("#oneLevelAccounts").val();

    $("#accountDataForm #code").val($("#fourLevelAccounts").val());
    var opt = $("#fourLevelAccounts option:selected").text().trim();
    $("#accountDataForm #formattedCode").val(opt.substr(0, opt.indexOf((" "))));
    $("#accountDataForm #accountName").val(opt.substr(opt.indexOf((" ")) + 1));

    $('#accountDataDiv').show();
    /*if (oneLevelAccountCode == "11") {
        $('#accountDataDiv').show();
    }
    else {
        $('#accountDataDiv').hide();
    }*/

    $.get("/accounts/account_data?code=" + $("#fourLevelAccounts").val(), function (data) {

        // UPDATE FORM DATA
        $("#accountDataForm #code").val(data.accountData.code);
        $("#accountDataForm #formattedCode").val(data.accountData.formattedCode);
        $("#accountDataForm #accountName").val(data.accountData.accountName);
        $("#accountDataForm #branchId").val(data.accountData.branchId);
        $("#accountDataForm #balanceLimit").val(data.accountData.balanceLimit);
        $("#accountDataForm #upperLimit").val(data.accountData.upperLimit);
        $("#accountDataForm #contactPerson").val(data.accountData.contactPerson);
        $("#accountDataForm #salesman").val(data.accountData.salesman);
        $("#accountDataForm #mobile").val(data.accountData.mobile);
        $("#accountDataForm #phone").val(data.accountData.phone);
        $("#accountDataForm #fax").val(data.accountData.fax);
        $("#accountDataForm #email").val(data.accountData.email);
        $("#accountDataForm #website").val(data.accountData.website);
        $("#accountDataForm #address").val(data.accountData.address);
        $("#accountDataForm #province").val(data.accountData.province).trigger("change.select2");
        var option = "<option selected=true value=" + "'" + data.accountData.city + "'" + ">" + data.accountData.city + "</option>";
        $("#accountDataForm #city").append(option);
        //$("#accountDataForm #city").val(data.accountData.city);
        $("#accountDataForm #accountTitle").val(data.accountData.accountTitle);
        $("#accountDataForm #accountNumber").val(data.accountData.accountNumber);
        $("#accountDataForm #bankBranchCode").val(data.accountData.bankBranchCode);
        $("#accountDataForm #bankBranchName").val(data.accountData.bankBranchName);

        // SELECT BRANCHES
        $("#companyBranches select").each(function () {
            $(this).val("");
            //$(this).val("").trigger("change.select2");A12 4 128 samsum
        });

        $.each(data.accountData.branches, function (i, branch) {
            //$("#select").select2("val", "CA");
            $("#companyBranches select#" + branch.company.id).val(branch.id);
            //$("#companyBranches select#" + branch.company.id).val(branch.id).trigger("change.select2");
        });

    });
}

function resetModalFields() {
    $("#account_level_modal_new #level_one_div").val("");
    $("#account_level_modal_new #level_two_div").val("");
    $("#account_level_modal_new #level_three_div").val("");
    $("#account_level_modal_new #level_four_div").val("");

    $("#account_level_modal_new #level_one_div").show();
    $("#account_level_modal_new #level_two_div").show();
    $("#account_level_modal_new #level_three_div").show();
    $("#account_level_modal_new #level_four_div").show();
}


function getNextAccountCode(accountLevel) {

    /*if (!$("#oneLevelAccounts").parsley().isValid()) {
        return;
    }*/
    //  alert("asif");
    $("#account_level").val(accountLevel);

    resetModalFields();

    if (accountLevel == "2") {
        $("#account_level_modal_new #level_two_div").hide();
        $("#account_level_modal_new #level_three_div").hide();

        var oneLevelAccountCode = $("#oneLevelAccounts").val();

        $.get("/accounts/next_account_code?accountLevel=" + accountLevel + "&parentCode=" + oneLevelAccountCode, function (data) {

            $("#account_level_modal_new #account_name").val("");

            $("#account_level_modal_new #account_code").val(data.nextFormattedCode);
            $("#account_level_modal_new #account_code").attr("data-id", data.nextCode);

            $("#account_level_modal_new #level_one_code").val($("#oneLevelAccounts option:selected").text());
            $("#account_level_modal_new #level_one_code").attr("data-id", $("#oneLevelAccounts").val());

        });
    }

    if (accountLevel == "3") {

        $("#account_level_modal_new #level_three_div").hide();

        var twoLevelAccountCode = $("#twoLevelAccounts").val();

        $.get("/accounts/next_account_code?accountLevel=" + accountLevel + "&parentCode=" + twoLevelAccountCode, function (data) {

            $("#account_level_modal_new #account_name").val("");

            $("#account_level_modal_new #account_code").val(data.nextFormattedCode);
            $("#account_level_modal_new #account_code").attr("data-id", data.nextCode);

            $("#account_level_modal_new #level_one_code").val($("#oneLevelAccounts option:selected").text());
            $("#account_level_modal_new #level_one_code").attr("data-id", $("#oneLevelAccounts").val());

            $("#account_level_modal_new #level_two_code").val($("#twoLevelAccounts option:selected").text());
            $("#account_level_modal_new #level_two_code").attr("data-id", $("#twoLevelAccounts").val());

        });
    }

    if (accountLevel == "4") {

        var threeLevelAccountCode = $("#threeLevelAccounts").val();

        $.get("/accounts/next_account_code?accountLevel=" + accountLevel + "&parentCode=" + threeLevelAccountCode, function (data) {

            $("#account_level_modal_new #account_name").val("");

            $("#account_level_modal_new #account_code").val(data.nextFormattedCode);
            $("#account_level_modal_new #account_code").attr("data-id", data.nextCode);

            $("#account_level_modal_new #level_one_code").val($("#oneLevelAccounts option:selected").text());
            $("#account_level_modal_new #level_one_code").attr("data-id", $("#oneLevelAccounts").val());

            $("#account_level_modal_new #level_two_code").val($("#twoLevelAccounts option:selected").text());
            $("#account_level_modal_new #level_two_code").attr("data-id", $("#twoLevelAccounts").val());

            $("#account_level_modal_new #level_three_code").val($("#threeLevelAccounts option:selected").text());
            $("#account_level_modal_new #level_three_code").attr("data-id", $("#threeLevelAccounts").val());

        });

    }

    /*$('#account_level_modal_new').modal('show');*/
}

function updateAccountName(accountLevel) {

    $("#account_level").val(accountLevel);

    resetModalFields();

    if (accountLevel == "2") {

        $("#account_level_modal_new #level_two_div").hide();
        $("#account_level_modal_new #level_three_div").hide();

        $("#account_level_modal_new #level_one_code").val($("#oneLevelAccounts option:selected").text());
        $("#account_level_modal_new #level_one_code").attr("data-id", $("#oneLevelAccounts").val());

        var opt = $("#twoLevelAccounts option:selected").text().trim();

        $("#account_level_modal_new #account_code").val(opt.substr(0, opt.indexOf((" "))));
        $("#account_level_modal_new #account_code").attr("data-id", $("#twoLevelAccounts").val());

        $("#account_level_modal_new #account_name").val(opt.substr(opt.indexOf((" ")) + 1));
    }

    if (accountLevel == "3") {

        $("#account_level_modal_new #level_three_div").hide();

        $("#account_level_modal_new #level_one_code").val($("#oneLevelAccounts option:selected").text());
        $("#account_level_modal_new #level_one_code").attr("data-id", $("#oneLevelAccounts").val());

        $("#account_level_modal_new #level_two_code").val($("#twoLevelAccounts option:selected").text());
        $("#account_level_modal_new #level_two_code").attr("data-id", $("#twoLevelAccounts").val());

        var opt = $("#threeLevelAccounts option:selected").text().trim();

        $("#account_level_modal_new #account_code").val(opt.substr(0, opt.indexOf((" "))));
        $("#account_level_modal_new #account_code").attr("data-id", $("#threeLevelAccounts").val());

        $("#account_level_modal_new #account_name").val(opt.substr(opt.indexOf((" ")) + 1));
    }

    if (accountLevel == "4") {

        $("#account_level_modal_new #level_one_code").val($("#oneLevelAccounts option:selected").text());
        $("#account_level_modal_new #level_one_code").attr("data-id", $("#oneLevelAccounts").val());

        $("#account_level_modal_new #level_two_code").val($("#twoLevelAccounts option:selected").text());
        $("#account_level_modal_new #level_two_code").attr("data-id", $("#twoLevelAccounts").val());

        $("#account_level_modal_new #level_three_code").val($("#threeLevelAccounts option:selected").text());
        $("#account_level_modal_new #level_three_code").attr("data-id", $("#threeLevelAccounts").val());

        var opt = $("#fourLevelAccounts option:selected").text().trim();

        $("#account_level_modal_new #account_code").val(opt.substr(0, opt.indexOf((" "))));
        $("#account_level_modal_new #account_code").attr("data-id", $("#fourLevelAccounts").val());

        $("#account_level_modal_new #account_name").val(opt.substr(opt.indexOf((" ")) + 1));
    }
}

function deleteAccount(accountLevel) {

    $("#account_level").val(accountLevel);

    if (accountLevel == "2") {
        $("#account_level_modal_delete #account_name_delete").val($("#twoLevelAccounts option:selected").text());
        $("#account_level_modal_delete #account_name_delete").attr("data-id", $("#twoLevelAccounts").val());
    }
    if (accountLevel == "3") {
        $("#account_level_modal_delete #account_name_delete").val($("#threeLevelAccounts option:selected").text());
        $("#account_level_modal_delete #account_name_delete").attr("data-id", $("#threeLevelAccounts").val());
    }
    if (accountLevel == "4") {
        $("#account_level_modal_delete #account_name_delete").val($("#fourLevelAccounts option:selected").text());
        $("#account_level_modal_delete #account_name_delete").attr("data-id", $("#fourLevelAccounts").val());
    }
}

$(document).ready(function () {

    $(".select2_single").on("select2:close", (function () {
            $(this).focus();
        })
    );

    // SUBMIT FORM
    $("#addOrUpdateAccountForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        addOrUpdateAccount();
    });

    /*$('#addOrUpdateAccountForm').validator({
    	account_code: {
    		required: function ($el) { return !!$.trim($el.val()) }
    	},
		errors: {
			required: 'Please fill out this field.'
		}
	});*/

    /*$(document).on("focusin", "#account_code", function() {
	   $(this).prop('readonly', true);  
	});

	$(document).on("focusout", "#account_code", function() {
	   $(this).prop('readonly', false); 
	});*/

    function addOrUpdateAccount() {

        accountLevel = $("#account_level").val()

        var parentCode;

        if (accountLevel == "2") {
            parentCode = $("#account_level_modal_new #level_one_code").attr("data-id")
        }
        if (accountLevel == "3") {
            parentCode = $("#account_level_modal_new #level_two_code").attr("data-id")
        }
        if (accountLevel == "4") {
            parentCode = $("#account_level_modal_new #level_three_code").attr("data-id")
        }

        // PREPARE FORM DATA
        var formData = {
            accountLevel: $("#account_level").val(),
            parentCode: parentCode,
            code: $("#account_level_modal_new #account_code").attr("data-id"),
            formattedCode: $("#account_level_modal_new #account_code").val(),
            name: $("#account_level_modal_new #account_name").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/accounts/add_or_update_account",
            data: JSON.stringify(formData),
            dataType: 'json',
            success: function (data) {
                loadFourLevelAccounts();
                if (accountLevel == "2") {
                    $("#twoLevelAccounts").empty();
                    $("#twoLevelAccounts").append("<option></option>");

                    $.each(data.accountLevelResults, function (i, record) {
                        var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
                        $("#twoLevelAccounts").append(option);
                    });

                    $('#twoLevelAccounts option[value=' + data.selectedCode + ']').attr("selected", "selected");
                }

                if (accountLevel == "3") {
                    $("#threeLevelAccounts").empty();
                    $("#threeLevelAccounts").append("<option></option>");

                    $.each(data.accountLevelResults, function (i, record) {
                        var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
                        $("#threeLevelAccounts").append(option);
                    });

                    $('#threeLevelAccounts option[value=' + data.selectedCode + ']').attr("selected", "selected");
                }

                if (accountLevel == "4") {
                    $("#fourLevelAccounts").empty();
                    $("#fourLevelAccounts").append("<option></option>");

                    $.each(data.accountLevelResults, function (i, record) {
                        var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
                        $("#fourLevelAccounts").append(option);
                    });

                    $('#fourLevelAccounts option[value=' + $("#account_level_modal_new #account_code").attr("data-id") + ']').attr("selected", "selected");
                    showAccountDataForm();
                }
            },
            complete: function (result) {
                //loadTwoLevelAccounts();
            },
        });

        // Reset FormData after Posting
        resetData();

    }

    function resetData() {
        $('#account_level_modal_new').modal('toggle');
        $("#account_level_modal_new #account_name").val("");
    }


    // SUBMIT FORM
    $("#deleteAccountForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        deleteAccount();
    });


    function deleteAccount() {

        accountLevel = $("#account_level").val()

        var parentCode;

        if (accountLevel == "2") {
            parentCode = $("#oneLevelAccounts").val()
        }
        if (accountLevel == "3") {
            parentCode = $("#twoLevelAccounts").val()
        }
        if (accountLevel == "4") {
            parentCode = $("#threeLevelAccounts").val()
        }

        // DO DELETE
        $.ajax({
            type: "DELETE",
            contentType: "application/json",
            url: "/accounts/delete_account?accountLevel=" + accountLevel + "&code=" + $("#account_level_modal_delete #account_name_delete").attr("data-id") + "&parentCode=" + parentCode,
            data: {_method: "delete"},
            success: function (data) {
                if (accountLevel == "2") {
                    $("#twoLevelAccounts").empty();
                    $("#twoLevelAccounts").append("<option></option>");

                    $.each(data.accountLevelResults, function (i, record) {
                        var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
                        $("#twoLevelAccounts").append(option);
                    });
                }

                if (accountLevel == "3") {
                    $("#threeLevelAccounts").empty();
                    $("#threeLevelAccounts").append("<option></option>");

                    $.each(data.accountLevelResults, function (i, record) {
                        var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
                        $("#threeLevelAccounts").append(option);
                    });
                }

                if (accountLevel == "4") {
                    $("#fourLevelAccounts").empty();
                    $("#fourLevelAccounts").append("<option></option>");

                    $.each(data.accountLevelResults, function (i, record) {
                        var option = "<option value = " + record.code + ">" + record.formattedCode + "&emsp;" + record.name + "</option>";
                        $("#fourLevelAccounts").append(option);
                    });
                }
            },
            complete: function (result) {
                $('#account_level_modal_delete').modal('toggle');
            },
        });

    }

    // SUBMIT FORM
    $("#accountDataForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        addOrUpdateAccountData();
    });

    function checkForDuplicateMobile() {
        var isMobileDuplicate;
        $.ajax({
            url: "/accounts/check_for_duplicate_mobile?code=" + $("#fourLevelAccounts").val() + "&mobile=" + $("#mobile").val(),
            type: 'GET',
            async: false,
            success: function (data) {
                isMobileDuplicate = data;
            },
            complete: function () {
            }
        });
        return isMobileDuplicate;
    }

    function addOrUpdateAccountData() {

       /* isMobileDuplicate = checkForDuplicateMobile();
        if (isMobileDuplicate == true || isMobileDuplicate == "true") {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "DUPLICATE MOBILE NUMBER EXISTS IN ANOTHER ACCOUNT",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from ajax.
            return false;
        }*/

        var accountDataForm = $('#accountDataForm');

        $("#accountDataForm #code").val($("#fourLevelAccounts").val());
        var opt = $("#fourLevelAccounts option:selected").text().trim();
        $("#accountDataForm #formattedCode").val(opt.substr(0, opt.indexOf((" "))));
        $("#accountDataForm #accountName").val(opt.substr(opt.indexOf((" ")) + 1));

        // PREPARE FORM DATA
        /*var formData = {
            code : $("#accountDataForm #code").val(),
            formattedCode : $("#accountDataForm #formattedCode").val(),
            branchId : $("#accountDataForm #branchId").val(),
            balanceLimit : $("#accountDataForm #balanceLimit").val(),
            contactPerson : $("#accountDataForm #contactPerson").val(),
            salesman : $("#accountDataForm #salesman").val(),
            mobile : $("#accountDataForm #mobile").val(),
            phone : $("#accountDataForm #phone").val(),
            fax : $("#accountDataForm #fax").val(),
            email : $("#accountDataForm #email").val(),
            website : $("#accountDataForm #website").val(),
            address : $("#accountDataForm #address").val(),
            province : $("#accountDataForm #province").val(),
            city : $("#accountDataForm #city").val(),
            accountTitle : $("#accountDataForm #accountTitle").val(),
            accountNumber : $("#accountDataForm #accountNumber").val(),
            bankBranchCode : $("#accountDataForm #bankBranchCode").val(),
            bankBranchName : $("#accountDataForm #bankBranchName").val()
        }

        // DO POST
        $.ajax({
            type : "POST",
            contentType : "application/json",
            url : accountDataForm.attr('action'),
            data : JSON.stringify(formData),
            dataType : 'json',
            success:function(data){
                alert('aaaaa');
            },
        });*/

        jsonData = getFormData(accountDataForm);

        var branches = [];
        $("#companyBranches tr").each(function () {
            if ($(this).find("td").length) {

                //var company = {};
                //company.id = $(this).find("td:first").text();

                var branch = {};
                branch.id = $(this).find("select").val();
                branch.name = $(this).find("select option:selected").text();
                //branch.company = company;

                branches.push(branch);
            }
        });
        jsonData.branches = branches;

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: accountDataForm.attr('action'),
            data: JSON.stringify(jsonData),
            dataType: 'json',
            success: function (data) {
                $("#modal_success").modal('show');
            },
        });
    }

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return day + '/' + month + '/' + year;
    }

    // update serial numbers
    $("#chart_of_accounts_datatable").each(function (i) {
        $(this).find("span.serial_no").each(function (index, element) {
            $(element).text(index + 1);
        });
    });

    if ($("#chart_of_accounts_datatable").length) {
        var chartOfAccountsTable = $("#chart_of_accounts_datatable").DataTable({
            dom: 'Bfrtip',
            bPaginate: false,
            scrollCollapse: true,

            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: false,
                },
                {
                    extend: 'excelHtml5',
                    text: 'EXCEL',
                    title: 'CHART OF ACCOUNTS' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    footer: false,
                    exportOptions: {
                        stripNewlines: false
                    }
                },
                {
                    extend: 'pdfHtml5',
                    title: 'CHART OF ACCOUNTS' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        //pageMargins [left, top, right, bottom]
                        doc.pageMargins = [5, 5, 5, 5];
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                    }
                },
                {
                    extend: 'print',
                    text: 'VIEW',
                    title: 'CHART OF ACCOUNTS' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false
                    },
                    customize: function (win) {
                        $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                        $(win.document.body).css('background-color', 'WHITE');
                        $(win.document.body).css("color", "DIMGRAY");
                        $(win.document.body).find('h1').css('text-align', 'center');
                        $(win.document.body).find('div:first').css('text-align', 'center');

                    }
                }
            ],
            initComplete: function () {
                this.api().columns([1, 2, 3, 4, 5, 6]).every(function () {
                    var column = this;
                    var select = $('<select><option value="">ALL</option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });

                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            }
        });

        chartOfAccountsTable.on('draw', function () {
            chartOfAccountsTable.columns().indexes().each(function (idx) {
                var select = $(chartOfAccountsTable.column(idx).footer()).find('select');

                if (select.val() === '') {
                    select
                        .empty()
                        .append('<option value="">ALL</option>');

                    chartOfAccountsTable.column(idx, {search: 'applied'}).data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    });
                }
            });
        });

        $("#chart_of_accounts_datatable tfoot tr").appendTo("#chart_of_accounts_datatable thead");
    }


    loadAllProvinces();

    function loadAllProvinces() {

        $("#province").empty();
        $("#province").append("<option></option>");

        /*$.get( "https://member.daraz.pk/locationtree/api/getSubAddressList", function(data) {
            $.each(data.module, function(i, record){
                var option = "<option data-id = " + record.id + " value = " + record.name + ">" + record.name + "</option>";
                $("#province").append(option);
            });
        });*/

        $.ajax({
            url: "https://member.daraz.pk/locationtree/api/getSubAddressList",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            type: 'GET',
            dataType: 'jsonp',
            crossDomain: true,
            success: function (data) {
                $.each(data.module, function (i, record) {
                    var option = "<option data-id = " + record.id + " value = " + "'" + record.name + "'" + ">" + record.name + "</option>";
                    $("#province").append(option);
                });
            },
            complete: function () {
            }
        });

    }

    $("#province").on("change", function () {
        loadProvinceCities($(this));
    });

    function loadProvinceCities(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        /*$.get( "https://member.daraz.pk/locationtree/api/getSubAddressList?addressId=" + $(thisControl).find(':selected').attr("data-id"), function( data ) {

            $("#city").empty();
            $("#city").append("<option></option>");

            $.each(data.module, function(i, record){
                var option = "<option data-id = " + record.id + " value = " + record.name + ">" + record.name + "</option>";
                $("#city").append(option);
            });
        });*/

        $.ajax({
            url: "https://member.daraz.pk/locationtree/api/getSubAddressList?addressId=" + $(thisControl).find(':selected').attr("data-id"),
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept"
            },
            type: 'GET',
            dataType: 'jsonp',
            crossDomain: true,
            success: function (data) {
                $("#city").empty();
                $("#city").append("<option></option>");

                $.each(data.module, function (i, record) {
                    var option = "<option value=" + "'" + record.name + "'" + ">" + record.name + "</option>";
                    $("#city").append(option);
                });
            },
            complete: function () {
            }
        });
    }

});