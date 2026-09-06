Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}
$(document).ready(function () {
    var formatter = new Intl.NumberFormat('ur-PK', {minimumFractionDigits: 0, maximumFractionDigits: 0});
    // console.log("asif");
    //on load
    restrictFromAndToDate();
    //loadCompanyBranches();
    $("#printGLForm").on('click', function () {
        location.replace("/reports/pre-purchase-report/?branchId=" + $("#branchId").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&itemDefId=" + $("#itemDefId").val() + "&millId=" + $("#millId").val() + "&branchNamed=" + $("#branchId option:selected").text() + "&accountNamed=" + $("#accountCode option:selected").text() + "&itemNamed=" + $("#itemDefId option:selected").text() + "&searchByDate=" + $("#search_by_date option:selected").text() + "&millKhataName=" + $("#millId option:selected").text() + "&sellerAccountCodeName=" + $("#sellerAccountCode option:selected").text() + "&millKhataId="+ $("#millKhataId").val() + "&accountCode=" + $("#accountCode").val()+ "&sellerAccountCode=" + $("#sellerAccountCode").val());
    });

    $("#printWAForm").on('click', function () {
        location.replace("/reports/pre-purchase-wa-report/?branchId=" + $("#branchId").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&itemDefId=" + $("#itemDefId").val() + "&millId=" + $("#millId").val() + "&branchNamed=" + $("#branchId option:selected").text() + "&accountNamed=" + $("#accountCode option:selected").text() + "&itemNamed=" + $("#itemDefId option:selected").text() + "&searchByDate=" + $("#search_by_date option:selected").text() + "&millKhataName=" + $("#millId option:selected").text() + "&sellerAccountCodeName=" + $("#sellerAccountCode option:selected").text() + "&millKhataId="+ $("#millKhataId").val()  + "&accountCode=" + $("#accountCode").val()+ "&sellerAccountCode=" + $("#sellerAccountCode").val() + "");
    });

    $("#printAccountGroup").on('click', function () {
        location.replace("/reports/group-account-pre-purchase-report/?branchId=" + $("#branchId").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&itemDefId=" + $("#itemDefId").val() + "&millId=" + $("#millId").val() + "&branchNamed=" + $("#branchId option:selected").text() + "&accountNamed=" + $("#accountCode option:selected").text() + "&itemNamed=" + $("#itemDefId option:selected").text() + "&searchByDate=" + $("#search_by_date option:selected").text() + "&millKhataName=" + $("#millId option:selected").text() + "&sellerAccountCodeName=" + $("#sellerAccountCode option:selected").text()+ "&millKhataId="+ $("#millKhataId").val()  + "&accountCode=" + $("#accountCode").val()+ "&sellerAccountCode=" + $("#sellerAccountCode").val());
    });

    $("#printMillGroup").on('click', function () {
        location.replace("/reports/group-mill-pre-purchase-report/?branchId=" + $("#branchId").val() + "&fromDate=" + $("#fromDate").val() + "&toDate=" + $("#toDate").val() + "&financialYearId=" + $("#financialYearId").val() + "&itemDefId=" + $("#itemDefId").val() + "&millId=" + $("#millId").val() + "&branchNamed=" + $("#branchId option:selected").text() + "&accountNamed=" + $("#accountCode option:selected").text() + "&itemNamed=" + $("#itemDefId option:selected").text() + "&searchByDate=" + $("#search_by_date option:selected").text() + "&millKhataName=" + $("#millId option:selected").text() + "&sellerAccountCodeName=" + $("#sellerAccountCode option:selected").text()+ "&millKhataId="+ $("#millKhataId").val()  + "&accountCode=" + $("#accountCode").val()+ "&sellerAccountCode=" + $("#sellerAccountCode").val());
    });

    $("#companyId").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if ($("#companyId").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#companyId").val(), function (data) {
            $("#branchId").empty();
            $("#branchId").append("<option value='0'>ALL</option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branchId").append(option);
            }
        });
    }

    $("#financialYearId").on("change", function () {
        restrictFromAndToDate();
    });

    function restrictFromAndToDate() {

        if (!$("#financialYearId").val()) {
            return;
        }

        $("#fromDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
        $("#fromDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
        //$("#fromDate").val($("#financialYearId option:selected").attr("data-value").split("|")[0]);

        $("#toDate").attr("min", $("#financialYearId option:selected").attr("data-value").split("|")[0]);
        $("#toDate").attr("max", $("#financialYearId option:selected").attr("data-value").split("|")[1]);
        //$("#toDate").val($("#financialYearId option:selected").attr("data-value").split("|")[1]);

    }


    function getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        //console.log("asif");
        return day + '/' + month + '/' + year;
    }

    function getMyDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        //console.log("asif");
        return year + '-' + month + '-' + day;
    }

    $("#editPrePurchaseEntryModel #prePurchaseReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        var images = [];
        var picDiv = document.querySelector('#_pics');

        for (var i = 0, len = picDiv.querySelectorAll(".floatleft").length; i < len; i++) {
            var image = {
                pictureUrl: picDiv.querySelectorAll(".floatleft .pictureUrl")[i].value,
                id: picDiv.querySelectorAll(".floatleft .pictureId")[i].value,
            }
            images.push(image);
        }

        var formData = {
            itemDef: {id: $("#prePurchaseReportForm #itemDef").val()},
            millKhata: {id: $("#prePurchaseReportForm #millKhata\\.id").val()},
            supplierAccount: {code: $("#prePurchaseReportForm #supplierAccount\\.code").val()},
            rate: $("#prePurchaseReportForm #rate").val(),
            kg: $("#prePurchaseReportForm #Kg").val(),
            mKg: $("#prePurchaseReportForm #Kg").val(),
            bag: $("#prePurchaseReportForm #bag").val(),
            freight: $("#prePurchaseReportForm #freight").val(),
            vehicalNo: $("#prePurchaseReportForm #vehicalNo").val(),
            id: $("#prePurchaseReportForm #id").val(),
            entryDate: $("#prePurchaseReportForm #entryDate").val(),//.trigger('change');
            company: {id: $("#prePurchaseReportForm #company").val()},
            branch: {id: $("#prePurchaseReportForm #branch").val()},
            prePurchaseCode: $("#prePurchaseReportForm #prePurchaseCode").val(),
            prePurchaseNumber: $("#prePurchaseReportForm #prePurchaseNumber").val(),
            docPictrueList: images,
        }
        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#prePurchaseReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "text",
            success: function (data) {
                $('#editPrePurchaseEntryModel').modal('toggle');
                generateGLReport();
            }
        });
    });


    // SUBMIT FORM
    $("#gPReportForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        generateGLReport();
    });

    function generateGLReport() {

        $("#submitGLForm").attr("disabled", true);  // disable submit button
        $('#loading').show();  // show loading indicator

        $('#gl_report_datatable').DataTable().clear();
        $('#gl_report_datatable').DataTable().destroy();
        var table = $('#gl_report_datatable').DataTable({
            dom: 'Bfrtip',
            deferRender: true,
            scrollY: 500,
            scroller: true,
            destroy: true,
            fixedHeader: true,
            // bPaginate: false,
            buttons: [
                {
                    extend: 'copy',
                    text: 'COPY',
                    title: null,
                    footer: true,
                },
                $.extend(true, {}, getExcelBuilder(), {
                    extend: 'excelHtml5',
                    text: 'EXCEL',
                    title: null,
                    footer: true,
                    exportOptions: {
                        stripNewlines: false
                    },
                }),
                /*{
                    extend: 'excelHtml5',
                    title: 'General Ledger' + ' (' + (new Date().toLocaleString()) + ')',
                    messageTop: 'COMPANY: ' + 'MST' + '\n' + 'ACCOUNT: ' + '111222333',
                    messageBottom: null,
                    stripNewlines: false
                },*/
                {
                    extend: 'pdfHtml5',
                    title: 'PRE PURCHASE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        stripNewlines: false,
                    },
                    customize: function (doc) {
                        doc.defaultStyle.fontSize = 8;
                        doc.pageMargins = [15, 15, 10, 20]; //1st left side ,2th header, 3th right side ,4th buttion
                        doc.styles.tableHeader.fontSize = 8;
                        doc.styles.tableFooter.fontSize = 8;
                        doc.defaultStyle.alignment = 'left';
                        doc.styles.tableHeader.alignment = 'left';
                        doc.styles.tableFooter.alignment = 'left';
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
                    extend: 'print',
                    title: 'PRE PURCHASE REPORT' + ' (' + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim() + ')',
                    messageTop: 'COMPANY: ' + $("#companyId option:selected").text()
                        + '  ' + 'BRANCH: ' + $("#branchId option:selected").text()
                        + '  ' + 'ITEM: ' + $("#itemDefId option:selected").text()
                        + '   ' + 'ACCOUNT: ' + $("#accountCode option:selected").text(),
                    messageBottom: null,
                    footer: true,
                    autoPrint: false,
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7],
                        stripNewlines: false,
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

                var colNumber = [8];

                // Remove the formatting to get integer data for summation
                var intVal = function (i) {
                    return typeof i === 'string' ?
                        i.replace(/[\$,]/g, '') * 1 :
                        typeof i === 'number' ?
                            i.toFixed(2) : 0;
                };

                for (i = 0; i < colNumber.length; i++) {
                    var colNo = colNumber[i];
                    var total = api
                        .column(colNo)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);
                    $(api.column(colNo).footer()).html(formatter.format(parseFloat(total).toFixed(2)));
                }
            }
        });

        //console.log("form submited");
        // PREPARE FORM DATA
        console.log("i haa");
        var formData = {
            companyId: $("#companyId").val(),
            branchId: $("#branchId").val(),
            voucherStatusId: "",
            financialYearId: $("#financialYearId").val(),
            fromDate: $("#fromDate").val(),
            toDate: $("#toDate").val(),
            itemDefId: $("#itemDefId").val(),
            //millId: $("#millId").val(),
            accountCode: $("#accountCode").val(),
            serchByDate: $("#search_by_date").val(),
            //entryStatus: $("#entryStatus").val(),
            //sellerAccountCode: $("#sellerAccountCode").val(),
            //millKhataId: $("#millKhataId").val()
        }

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: $("#gPReportForm").attr("action"),
            data: JSON.stringify(formData),
            dataType: "json",
            success: function (data) {
                console.log(data);
                // FILL TABLE ROWS
                $.each(data, function (i, pPEntry) {

                    table.row.add([
                        '<td>' + (i + 1) + '</td>',
                        '<td>' + pPEntry.voucherCode + '</td>',
                        '<td><a className="date">' + getFormattedDate(new Date(pPEntry.voucherDate)) + '</a></td>',
                        '<td>' + pPEntry.truck + '</td>',
                        '<td>' + pPEntry.itemName + '</td>',
                        '<td>' + pPEntry.amount + '</td>',
                        '<td>' + pPEntry.bookNumber + '</td>',
                        '<td><a className="date">' + getFormattedDate(new Date(pPEntry.pVoucherDate)) + '</a></td>',
                        '<td>' + pPEntry.pTruck + '</td>',
                        '<td>' + pPEntry.pItemName + '</td>',
                        '<td>' + pPEntry.pAmount + '</td>',
                        '<td>' + pPEntry.pVoucherCode + '</td>',
                        '<td>' + pPEntry.pBookNumber + '</td>',
                    ]);
                });

                table.draw();

                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator

            },
            complete: function () {
                $("#submitGLForm").attr("disabled", false);  // enable submit button
                $('#loading').hide();  // hide loading indicator
            },
        });

        //highlight
        /*$('#gl_report_datatable tbody').on( 'mouseenter', 'td', function () {
            
    		table.rows().eq(0).each(function (index) {
    			$(table.row(index).nodes()).removeClass('highlight');
    		});
    		//$(table.cells().nodes()).removeClass('highlight');
    		
    		var rowIdx = table.cell(this).index().row;
            //var colIdx = table.cell(this).index().column;
            
            $(table.row(rowIdx).nodes()).addClass('highlight');
            //$(table.column(colIdx).nodes()).addClass('highlight');
        });*/
    }

    function getExcelBuilder() {
        var xlsBuilder = {
            filename: 'SL_' + new Date().toLocaleString(),
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
                    v: 'SALES REPORT' + ' ('
                        + getFormattedDate(new Date()) + ", " + new Date().toLocaleString().split(",")[1].trim()
                        + ')'
                }]);
                var r2 = Addrow(2, [{
                    k: 'A',
                    v: 'COMPANY :'
                }, {
                    k: 'B',
                    v: $("#companyId option:selected").text()
                }]);
                var r3 = Addrow(3, [{
                    k: 'A',
                    v: 'BRANCH :'
                }, {
                    k: 'B',
                    v: $("#branchId option:selected").text()
                }]);
                var r4 = Addrow(4, [{
                    k: 'A',
                    v: 'VOUCHER STATUS :'
                }, {
                    k: 'B',
                    v: $("#voucherStatusId option:selected").text()
                }]);
                var r5 = Addrow(5, [{
                    k: 'A',
                    v: 'FINANCIAL YEAR :'
                }, {
                    k: 'B',
                    v: $("#financialYearId option:selected").text() + " (" + getFormattedDate(new Date($("#fromDate").val())) + " - " + getFormattedDate(new Date($("#toDate").val())) + ")"
                }]);
                var r6 = Addrow(6, [{
                    k: 'A',
                    v: 'ITEM :'
                }, {
                    k: 'B',
                    v: $("#itemDefId option:selected").text()
                }]);
                var r7 = Addrow(7, [{
                    k: 'A',
                    v: 'ACCOUNT :'
                }, {
                    k: 'B',
                    v: $("#accountCode option:selected").text()
                }]);

                sheet.childNodes[0].childNodes[1].innerHTML = r1
                    + r2
                    + r3
                    + r4
                    + r5
                    + r6
                    + r7
                    + sheet.childNodes[0].childNodes[1].innerHTML;
            },
            /*
             * exportOptions: { columns: [0, 1, 2, 3] }
             */
        }
        return xlsBuilder;
    }

    /* $("#editPrePurchaseEntryModel").on("hidden.bs.modal", function () {
         $("#prePurchaseReportForm").trigger("reset");
     });*/
    $('body').on('click', '.editPrePurchaseEntry', function () {

        $("#zoomer").attr('src', "");
        document.querySelectorAll('#_pics #floatleft');
        var now = new Date($(this).closest("tr").find(".entryDate").val().trim());
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);
        ///console.log("mySalet"+today);
        $("#prePurchaseReportForm #itemDef").val($(this).closest("tr").find('.itemDefId').val()).trigger('change');
        $("#prePurchaseReportForm #millKhata\\.id").val($(this).closest("tr").find(".millId").val()).trigger('change');
        $("#prePurchaseReportForm #supplierAccount\\.code").val($(this).closest("tr").find(".code").val()).trigger('change');
        $("#prePurchaseReportForm #rate").val($(this).closest("tr").find(".rate").text().trim());


        $("#prePurchaseReportForm #kg").val($(this).closest("tr").find(".kg").text().trim());

        $("#prePurchaseReportForm #bag").val($(this).closest("tr").find(".bag").text().trim());
        $("#prePurchaseReportForm #Kg").val($(this).closest("tr").find(".kg").text().trim());
        //$("#prePurchaseReportForm #rate").val($(this).closest("tr").find(".mRate").text().trim());

        //$("#prePurchaseReportForm #kg").val($(this).closest("tr").find(".kg").text().trim());

        $("#prePurchaseReportForm #freight").val($(this).closest("tr").find(".freight").text().trim());
        $("#prePurchaseReportForm #vehicalNo").val($(this).closest("tr").find(".vehicalNo").text().trim());
        $("#prePurchaseReportForm #id").val($(this).closest("tr").find(".id").val().trim());
        $("#prePurchaseReportForm #entryDate").val(today);//.trigger('change');
        $("#prePurchaseReportForm #company").val($(this).closest("tr").find(".company").val().trim());
        $("#prePurchaseReportForm #branch").val($(this).closest("tr").find(".branch").val().trim());
        $("#prePurchaseReportForm #prePurchaseCode").val($(this).closest("tr").find(".ppvn").val().trim());
        //$("#prePurchaseReportForm #remarks").val($(this).closest("tr").find(".remarks").val().trim());
        var pics = document.querySelector('#prePurchaseReportForm #_pics');
        pics.innerHTML = "";

        $("#editPrePurchaseEntryModel").modal("show");

        // pics.appendChild(divClone);
        $.get("/payables/getPictures?prePurchaseEntryId=" + $(this).closest("tr").find(".id").val().trim(), function (data) {
            //console.log("asif idrees " + data.length);
            for (var i = 0, len = data.length; i < len; i++) {

                var divClone = document.querySelector('#floatleft').cloneNode(true);
                divClone.querySelector(".ImageIcon").setAttribute('src', "/img/" + data[i].pictureUrl);

                divClone.querySelector(".ImageIcon").setAttribute('oncontextmenu', "displayCMenu(this," + i + ");return false;");
                divClone.querySelector(".floatleft .pictureUrl").setAttribute("id", "docPictrueList" + i + ".pictureUrl");
                divClone.querySelector(".floatleft .pictureUrl").setAttribute("name", "docPictrueList[" + i + "].pictureUrl");
                divClone.querySelector(".floatleft .pictureId").setAttribute("id", "docPictrueList" + i + ".id");
                divClone.querySelector(".floatleft .pictureId").setAttribute("name", "docPictrueList[" + i + "].id");
                divClone.querySelector(".floatleft .pictureUrl").setAttribute("value", data[i].pictureUrl);
                divClone.querySelector(".floatleft .pictureId").setAttribute("value", data[i].id);
                divClone.querySelector(".floatleft .mymenu").setAttribute("id", "menu" + i);

                pics.appendChild(divClone);
                $("#menu" + i).menu(
                    {position: {my: "right top", at: "right-5 top+10"}}
                );
                $("#menu" + i).hide();
            }
        });

        //pics.remove(".floatleft");
    });

});

function asifzoomIn() {

    var zoomer = document.querySelector("#zoomer");
    zoomer.style.width = (zoomer.width + 10) + "px";
    if (screen.height - 120 > zoomer.height)
        zoomer.style.height = (zoomer.height + 10) + "px";

}

function zoomOut(e) {

    var curentWidth = document.querySelector("#zoomer");
    curentWidth.style.width = (curentWidth.offsetWidth - 10) + "px";
    if (screen.height - 300 < zoomer.height)
        zoomer.style.height = (zoomer.height - 10) + "px";

}

function displayCMenu(obj, sn) {

    $(".mymenu").hide();
    $("#menu" + sn).show();

}

function zooMer(src) {

    $("#zoomer").attr('src', src);

}

var degree = 90;
$("#rotate").on('click', function () {

    var zoomer = document.querySelector("#zoomer");

    zoomer.style.transform = 'rotate(' + degree + 'deg)';
    degree += 90;
});

function deletePic(obj) {
    if (confirm("Are you sure you want to remove this image? :" + obj)) {
        var pNode = obj.parentNode;

        pNode.removeChild(obj);
        UpdateDocPictureEntry();
    }
}

function UpdateDocPictureEntry() {

    var picDiv = document.querySelector('#_pics');

    for (var i = 0, len = picDiv.querySelectorAll(".floatleft").length; i < len; i++) {
        picDiv.querySelectorAll(".ImageIcon")[i].setAttribute('oncontextmenu', "displayCMenu(this," + i + ");return false;");
        picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("id", "docPictrueList" + i + ".pictureUrl");
        picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("name", "docPictrueList[" + i + "].pictureUrl");
        picDiv.querySelectorAll(".floatleft .pictureId")[i].setAttribute("id", "docPictrueList" + i + ".id");
        picDiv.querySelectorAll(".floatleft .pictureId")[i].setAttribute("name", "docPictrueList[" + i + "].id");

        picDiv.querySelectorAll(".floatleft .mymenu")[i].setAttribute("id", "menu" + i);

        $("#menu" + i).menu({position: {my: "right top", at: "right-5 top+10"}});

        $("#menu" + i).hide();
    }
}// end of UpdateDocPictureEntry()

$("#buttionId").on('click', function () {
    var formData = new FormData();
    jQuery.each(jQuery($(".file"))[0].files, function (i, file) {
        formData.append('file-' + i, file);
    });
    var pics = document.querySelector('#prePurchaseReportForm #_pics');
    $.ajax({
        type: "POST",
        enctype: "multipart/form-data",
        contentType: "application/json",
        url: "/payables/fileUpload",
        data: formData,
        processData: false, //prevent jQuery from automatically transforming the data into a query string
        contentType: false,
        cache: false,
        success: function (data) {

            $.each(data.SucessfulList, function (l, imgName) {
                var divClone = document.querySelector('#floatleft').cloneNode(true);
                divClone.querySelector(".ImageIcon").setAttribute('src', "/img/" + imgName);
                divClone.querySelector(".pictureId").setAttribute('value', "0");
                pics.appendChild(divClone);
                UpdateDocPictureEntry();
            });

        },
        complete: function () {
            //$("#upload_supporting_docs").modal("toggle");
        },
    });
});  // end of $("#buttionId").on('click',function()
	