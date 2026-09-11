$(document).ready(function () {

    var buttionName = "";
    //on load
//	$('#update_account_entry_table').DataTable().clear();
//	$('#update_account_entry_table').DataTable().destroy();
//	 var table =$('#update_account_entry_table').DataTable({
//	        scrollY:        "300px",
//	        scrollX:        true,
//	        scrollCollapse: true,
//	        paging:         false,
//	        columnDefs: [
//	            { width: 300, targets: 0 }
//	        ],
//	        fixedColumns: true
//	    } );
    var table = $('#update_account_entry_table').DataTable({
        dom: 'Bfrtip',
        'bPaginate': false,
        destroy: true,
        buttons: [
            {
                extend: 'copy',
                text: 'COPY',
                title: null,
                footer: true,
                exportOptions: {
                    //c///olumns: [0, 1, 2, 5, 7, 8, 9, 10],
                },
                customize: function (doc) {
                    buttionName = "COPY";
                    userLog();
                }
            },

            {
                extend: 'pdfHtml5',
                download: 'open',
                title: 'CONTENT LIST',
                messageBottom: null,
                footer: true,
                exportOptions: {
                    //columns: [0, 1, 2, 5, 7, 8, 9, 10],
                    stripNewlines: false,
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 7;
                    doc.pageMargins = [10, 10, 10, 10];
                    doc.styles.tableHeader.fontSize = 7;
                    doc.styles.tableFooter.fontSize = 7;
                    doc.defaultStyle.alignment = 'left';
                    doc.styles.tableHeader.alignment = 'left';
                    doc.styles.tableFooter.alignment = 'left';
                    buttionName = "PDF";
                    userLog();
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                '',
                                {
                                    // This is the right column
                                    alignment: 'right',
                                    text: ['page ', {text: page.toString()}, ' of ', {text: pages.toString()}]
                                }
                            ],
                            margin: [10, 0]// [left or right , up or down]
                        }
                    });
                }
            },
            {
                extend: 'pdfHtml5',
                download: 'open',
                extension: '.tax',
                footer: true,
                filename: "asif idrees",
                text: '<i class="fa fa-file-pdf-o " style=color:green > PDF</i>',

                title: 'TRIAL BALANCE',
                footer: true,
                messageTop: 'COMPANY : ',

                messageBottom: null,
                messageTop: null,
                messageBottom: null,
                pageSize: 'A4',


                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6],
                    stripNewlines: false,
                },
                customize: function (doc) {
                    doc.defaultStyle.fontSize = 8;
                    //pageMargins [left, top, right, bottom]
                    doc.pageMargins = [15, 15, 15, 25];
                    doc.styles.tableHeader.fontSize = 8;
                    doc.styles.tableFooter.fontSize = 8;
                    doc.defaultStyle.alignment = 'left';
                    doc.styles.tableHeader.alignment = 'left';
                    doc.styles.tableFooter.alignment = 'left';
                    doc.content[1].layout = {
                        hLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 2 : 1;
                        },
                        vLineWidth: function (i, node) {
                            return (i === 0 || i === node.table.widths.length) ? 2 : 1;
                        },
                        hLineColor: function (i, node) {
                            return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
                        },
                        vLineColor: function (i, node) {
                            return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
                        }
                    };
//                    { data: "active", width: '7%', render: function(data, type, row){
//                        if(type === 'display'){
//                            return (data == true)? '<i class="fa fa-check text-green" aria-hidden="true"></i>' : '';
//                        }else if(type == 'export'){
//                            return (data == true)? '<span class="text-green">A</span>' : 'I';
//                        }else{
//                            return (data == true)? 'A' : 'I';
//                        }
//                    }, className: 'text-center'
//                };
                    buttionName = "PDF";
                    userLog();
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                '',
                                {
                                    // This is the right column
                                    alignment: 'right',
                                    text: ['page ', {text: page.toString()}, ' of ', {text: pages.toString()}]
                                }
                            ],
                            margin: [50, 0]// [left or right , up or down]
                        }
                    });
                }
            },
            {
                extend: 'print',
                text: 'VIEW',
                title: 'CONTENT LIST',
                messageBottom: null,
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    // columns: [0, 1, 2, 5, 7, 8, 9, 10],
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                    $(win.document.body).find('tr:nth-child(odd) td').each(function (index) {
                        $(this).css('background-color', 'WHITESMOKE');
                    });
                    $(win.document.body).find('tr:nth-child(even) td').each(function (index) {
                        $(this).css('background-color', 'WHITE');
                    });

                    $(win.document.body).css('background-color', 'WHITE');
                    $(win.document.body).css("color", "DIMGRAY");

                    $(win.document.body).find('h1').css('text-align', 'center');
                    //$(win.document.body).find('h1').css("color", "DIMGRAY");

                    $(win.document.body).find('div:first').css('text-align', 'center');
                    //$(win.document.body).find('div:first').css("color", "DIMGRAY");

                    $(win.document.body).find('th').css("color", "WHITE");
                    $(win.document.body).find('th').css("background-color", "DIMGRAY");
                    buttionName = "VIEW";
                    userLog();
                    doc['footer'] = (function (page, pages) {
                        return {
                            columns: [
                                '',
                                {
                                    // This is the right column
                                    alignment: 'right',
                                    text: ['page ', {text: page.toString()}, ' of ', {text: pages.toString()}]
                                }
                            ],
                            margin: [10, 0]// [left or right , up or down]
                        }
                    });
                }
            }
        ],

    });

    if ($("#reports_download").val() === "false") {
        table.buttons('.dt-button').remove();
    }
    $("#edit_account__information").on("hidden.bs.modal", function () {
        $("#stockOpeningForm").trigger("reset");
    });

    $(".account_infromation_edit").on("click", function () {
        // $('#edit_account_information').show();
        console.log("AA  " + $(this).closest("tr").find(".code").val());

        $.get("/accounts/account_data?code=" + $(this).closest("tr").find(".code").val(), function (data) {

            $("#editFormData #code").val(data.accountData.code);

            console.log("AAaa  " + $("#accountDataForm #code").val());

            $("#editFormData #formattedCode").val(data.accountData.formattedCode);
            $("#editFormData #accountName").val(data.accountData.accountName);
            $("#editFormData #branchId").val(data.accountData.branchId);
            $("#editFormData #balanceLimit").val(data.accountData.balanceLimit);
            $("#editFormData #upperLimit").val(data.accountData.upperLimit);
            $("#editFormData #contactPerson").val(data.accountData.contactPerson);
            $("#editFormData #salesman").val(data.accountData.salesman);
            $("#editFormData #mobile").val(data.accountData.mobile);
            $("#editFormData #phone").val(data.accountData.phone);
            $("#editFormData #fax").val(data.accountData.fax);
            $("#editFormData #email").val(data.accountData.email);
            $("#editFormData #website").val(data.accountData.website);
            $("#editFormData #address").val(data.accountData.address);
            var province = "<option selected=true value=" + "'" + data.accountData.province + "'" + ">" + data.accountData.province + "</option>";
            //$("#accountDataForm #province").empty();
            $("#editFormData #province").append(province);
            var district = "<option selected=true value=" + "'" + data.accountData.district + "'" + ">" + data.accountData.district + "</option>";
            //$("#accountDataForm #district").empty();
            $("#editFormData #district").append(district);

            var option = "<option selected=true value=" + "'" + data.accountData.city + "'" + ">" + data.accountData.city + "</option>";
            //$("#accountDataForm #city").empty();
            $("#editFormData #city").append(option);
            $("#editFormData #accountTitle").val(data.accountData.accountTitle);
            $("#editFormData #accountNumber").val(data.accountData.accountNumber);
            $("#editFormData #bankBranchCode").val(data.accountData.bankBranchCode);
            $("#editFormData #bankBranchName").val(data.accountData.bankBranchName);
            $("#edit_account_information").modal("show");
        });
    });

    function getFormData($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    }

    $("#editFormData").submit(function (event) {
        // Prevent the form from submitting via the browser.

        event.preventDefault();
        addOrUpdateAccountData();
        $('edit_account_information').hide();
    });

    function checkForDuplicateMobile() {
        var isMobileDuplicate;
        $.ajax({
            url: "/accounts/check_for_duplicate_mobile?code=" + $("#editFormData #code").val() + "&mobile=" + $("#mobile").val(),
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

        isMobileDuplicate = checkForDuplicateMobile();
        if (isMobileDuplicate == true || isMobileDuplicate == "true") {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "DUPLICATE MOBILE NUMBER EXISTS IN ANOTHER ACCOUNT",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from ajax.
            return false;
        }

        var accountDataForm = $('#editFormData');

        jsonData = getFormData(accountDataForm);


        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $('#editFormData').attr('action'),
            data: JSON.stringify(jsonData),
            dataType: 'json',
            success: function (data) {
                $("#modal_success").modal('show');
            },
        });
    }

    $("#updateAccountBtn").on("click", function (event) {

        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SAVE ?",
            buttons: {
                confirm: function () {
                    AccountUpdateFun();
                },
                cancel: function () {
                },
            }
        });

    });

    function userLog() {

        var userLogObj = {
            idNumber: "",
            code: "",
            buttonClick: buttionName,

            windowName: $('h2').html(),
        }

        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/viewSaleOrder",
            data: JSON.stringify(userLogObj),
            dataType: "json",
            success: function (data) {
                successmessage = 'Data was succesfully captured';
                //	location.reload();
                //  $("#customerAccount\\.balanceLimit").text(successmessage);
            },
            error: function (data) {
                successmessage = 'Error';
                //  $("#customerAccount\\.balanceLimit").text(successmessage);
            },
        });
    }

    function AccountUpdateFun() {
        var entries = [];
        $("#update_account_entry_table tbody tr").each(function () {
            console.log("asif" + $(this).find(".homePhone").val());
            var entry =
                {
                    code: $(this).find(".code").val(),
                    mobile: $(this).find(".mobile").val(),

                    homePhone: $(this).find(".homePhone").val(),
                    phone: $(this).find(".phone").val(),
                    newCode: $(this).find(".newCode").val(),
                }
            entries.push(entry);
        });
        console.log("asif");
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/accounts/update_bulk_accounts",
            data: JSON.stringify(entries),
            success: function (data) {
                location.reload();
            },
        });

    }
});