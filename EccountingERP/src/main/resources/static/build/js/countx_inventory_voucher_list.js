$(document).ready(function () {

    $("#voucher_company").prop("selectedIndex", 1);
    $("#voucher_status").prop("selectedIndex", 1);
    $("#search_by_date").prop("selectedIndex", 2);
    $('#from_date').val(new Date().toISOString().substring(0, 10));
    $('#to_date').val(new Date().toISOString().substring(0, 10));

    loadCompanyBranches();
    var regNumbers = [];
    var regNumber = {};
    $("#updateBookNumber").on('click', function () {
        $("#voucher_datatable tbody tr").each(function () {

            regNumber = {
                "bookPage": $(this).find(".bookNumber").val(),
                "voucherId": $(this).find(".voucherId").val(),
            }
            regNumbers.push(regNumber);
            console.log("" + regNumbers);
            console.log("Id:" + $(this).find(".voucherId").val() + " ");
        });
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/receivables/voucher_list_update_book-number",
            data: JSON.stringify(regNumbers),
            dataType: "text",
            success: function (data) {
                console.log("Respose :" + data);
            },
            complete: function () {
                console.log("Respose complete :");
            },
        });

    });

    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 2, maximumFractionDigits: 2});
    $("#voucher_company").on("change", function () {
        loadCompanyBranches();
    });


    function loadCompanyBranches() {

        if (!$("#voucher_company").val() || $("#voucher_company").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#voucher_company").val(), function (data) {
            $("#company_branch").empty();
            $("#company_branch").append("<option value='0'>ALL</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#company_branch").append(option);
            }

            $("#company_branch").prop("selectedIndex", 1);
        });
    }

    // SUBMIT FORM
    $("#voucherFilterForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        getVoucherList();
    });

    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        console.log(day + '/' + month + '/' + year);
        return day + '/' + month + '/' + year;
    }

    function getVoucherList() {

        // disable submit button
        $("#getVoucherList").attr("disabled", true);

        var voucherTable = $("#voucher_datatable").DataTable({
            dom: 'Bfrtip',
            deferRender: true,
            scrollY: 500,
            scroller: true,
            destroy: true,
            fixedHeader: true,
            bPaginate: true,
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                },
//	            $.extend(true, {}, getExcelBuilder(), {
//	            	extend: 'excelHtml5',
//	            	text: 'EXCEL',
//	            	title: null,
//	            	footer: true,
//	            	exportOptions: {
//	                    stripNewlines: false
//	                },
//	            }),
                {
                    extend: 'excelHtml5',
                    title: 'PURCHASE VOUCHER LIST' + ' (' + (new Date().toLocaleString()) + ')',
                    messageTop: '',
                    messageBottom: null,
                    stripNewlines: false
                },
                {
                    extend: 'pdfHtml5',
                    title: '' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()

                        + ' ' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#from_date").val())) + " - " + getFormattedDate(new Date($("#to_date").val())) + ")"
                        + ' ' + 'Search by :' + $("#search_by_date option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        doc.pageMargins = [15, 15, 15, 15];
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
                    }
                },
                {
                    extend: 'print',
                    title: '' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()

                        + ' ' + 'FINANCIAL YEAR: ' + $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#from_date").val())) + " - " + getFormattedDate(new Date($("#to_date").val())) + ")"
                        + ' ' + 'Search by :' + $("#search_by_date option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        stripHtml: false,
                        stripNewlines: false
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
                    }
                }
            ],
            "footerCallback": function (row, data, start, end, display) {
                var api = this.api(), data;

                var colNumber = [5];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    if (typeof i === 'string') {
                        return i.replace(/[\$,]/g, '') * 1;
                    } else if (typeof i === 'number') {
                        return parseFloat(i);
                    } else {
                        return parseFloat(0);
                    }
                };

                for (i = 0; i < colNumber.length; i++) {
                    var colNo = colNumber[i];
                    var total = api
                        .column(colNo)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    //$(api.column(colNo).footer()).html(parseFloat(total).toFixed(2));
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            }
        });

        // PREPARE FORM DATA
        var formData = {
            company: {id: $("#voucherFilterForm #voucher_company").val(), name: ""},
            branch: {id: $("#voucherFilterForm #company_branch").val(), name: ""},
            voucherType: {id: $("#voucherFilterForm #voucher_type").val(), name: ""},

            voucherCode: $("#voucherFilterForm #voucher_code").val(),
            inventoryVoucherType: $("#voucherFilterForm #inventory_voucher_type").val(),
            //voucherNarration : $("#voucherFilterForm #voucher_narration").val(),
            searchByDate: $("#voucherFilterForm #search_by_date").val(),
            fromDate: $("#voucherFilterForm #from_date").val(),
            toDate: $("#voucherFilterForm #to_date").val(),
            postedUnPosted: $("#voucherFilterForm #voucher_posted").val(),
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#voucherFilterForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                // FILL TABLE ROWS

                voucherTable.clear().draw();

                $.each(data.voucherDetailList, function (i, voucher) {

                    var formatter = new Intl.NumberFormat('ur-PK', {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    });
                    console.log(voucher.voucherCode + "  qq");
                    var voucherViewLink;
                    var bookNumber = '<td> <input type= "text" class ="bookNumber" value=' + voucher.register + '>' + '<input type="hidden" class="voucherId" value=' + voucher.id + '></td>';
                    if (voucher.voucherCode.indexOf("STV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else if (voucher.voucherCode.indexOf("PJV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase_journal_vouchers/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else if (voucher.voucherCode.indexOf("PTV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/SaleQty/view_pjv_sjv_qty/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else if (voucher.voucherCode.indexOf("SQV") >= 0) {
                        //console.log(voucher.voucherCode+"  qq");
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale-quantity-vouchers/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else if (voucher.voucherCode.indexOf("PQV") >= 0) {
                        //console.log(voucher.voucherCode+"  qq");
                        bookNumber = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/receivables/new-quantity-voucher/" + voucher.id + ">" + "SALE" + "</a>";

                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/purchase-quantity-vouchers/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else if (voucher.voucherCode.indexOf("SJV") >= 0) {
                        console.log(voucher.voucherCode);
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_journal_vouchers/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else if (voucher.voucherCode.indexOf("SRV") >= 0) {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/sale_return_vouchers/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    } else {
                        voucherViewLink = "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/vouchers/" + voucher.id + ">" + voucher.voucherCode + "</a>";
                    }

                    voucherTable.row.add([
                        (i + 1),
                        "<a style='text-decoration: underline;' target=_blank href=" + location.protocol + "//" + location.host + "/reports/general_ledger/?code=" + voucher.code + ">" + voucher.accountName + "</a>",
                        voucherViewLink,
                        getFormattedDate(new Date(voucher.voucherDate)),

                        getFormattedDate(new Date(voucher.updatedAt)) + ", " + new Date(voucher.updatedAt).toLocaleString().split(",")[1].trim(),
                        formatter.format(voucher.voucherAmount),
                        voucher.userName,
                        voucher.postedUnPosted,
                        bookNumber,

                    ]);
                });

                voucherTable.draw();
                // enable submit button
                $("#getVoucherList").attr("disabled", false);
            },
            complete: function () {


            },
        });


        //highlight
        $('#voucher_datatable tbody').on('mouseenter', 'td', function () {
            if (table instanceof $.fn.dataTable.Api) {
                table.rows().eq(0).each(function (index) {
                    $(table.row(index).nodes()).removeClass('highlight');
                });
                $(table.cells().nodes()).removeClass('highlight');

                var rowIdx = table.cell(this).index().row;
                var colIdx = table.cell(this).index().column;

                $(table.row(rowIdx).nodes()).addClass('highlight');
                $(table.column(colIdx).nodes()).addClass('highlight');
            }
        });

    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'PR_' + new Date().toLocaleString(),
            sheetName: 'sheet1',
            customize: function (xlsx) {
                var sheet = xlsx.xl.worksheets['sheet1.xml'];
                var downrows = 8;
                var clRow = $('row', sheet);
                var msg;
                // update Row
                clRow.each(function () {
                    var attr = $(this).attr('r');
                    var ind = parseInt(attr);
                    ind = ind + downrows;
                    $(this).attr("r", ind);
                });

                // Update row > c
                $('row c ', sheet).each(
                    function () {
                        var attr = $(this).attr('r');
                        var pre = attr.substring(0, 1);
                        var ind = parseInt(attr.substring(
                            1, attr.length));
                        ind = ind + downrows;
                        $(this).attr("r", pre + ind);
                    });

                function Addrow(index, data) {

                    msg = '<row xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" r="'
                        + index + '">';
                    for (var i = 0; i < data.length; i++) {
                        var key = data[i].k;
                        var value = data[i].v;
                        msg += '<c t="inlineStr" r="' + key
                            + index + '">';
                        msg += '<is>';
                        msg += '<t>' + value + '</t>';
                        msg += '</is>';
                        msg += '</c>';
                    }
                    msg += '</row>';
                    return msg;
                }

                var r1 = Addrow(1, [{
                    k: 'A',
                    v: 'PURCHASE REPORT' + ' ('
                        + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
                        + ')'
                }]);
                var r2 = Addrow(2, [{
                    k: 'A',
                    v: 'COMPANY :'
                }, {
                    k: 'B',
                    v: $("#voucher_company option:selected").text()
                }]);
                var r3 = Addrow(3, [{
                    k: 'A',
                    v: 'BRANCH :'
                }, {
                    k: 'B',
                    v: $("#voucher_company option:selected").text()
                }]);
                var r4 = Addrow(4, [{
                    k: 'A',
                    v: 'VOUCHER STATUS :'
                }, {
                    k: 'B',
                    v: $("#voucher_company option:selected").text()
                }]);


                sheet.childNodes[0].childNodes[1].innerHTML = r1
                    + r2
                    + r3
                    + r4

                    + sheet.childNodes[0].childNodes[1].innerHTML;
            },
            /*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
        }
        return xlsBuilder;
    }

})