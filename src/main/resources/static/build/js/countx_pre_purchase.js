function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

Number.prototype.toFixed = function (decimalPlaces) {
    return Number(Math.round(this + 'e' + decimalPlaces) + 'e-' + decimalPlaces);
}

$(document).ready(function () {

    for (var i = 1; i <= document.querySelectorAll(".mymenu").length; i++) {
        $("#menu" + i).menu(
            {position: {my: "left top", at: "right-5 top"}}
        );
        $("#menu" + i).hide();
    }

    $('body').on('click', '.buttionId', function () {

        // alert("aaa"+$(this).closest("tr").find(".imagesUrl").val())
        var row = $(this).closest("tr");

        var formData = new FormData();
        jQuery.each(jQuery($(this).closest("tr").find(".file"))[0].files, function (i, file) {
            formData.append('file-' + i, file);
        });

        $(this).closest("tr").find(".file").val("");
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
                var msg = "DOCUMENT HAS BEEN UPLOADED SUCCESSFULLY..!!";
                $.each(data.SucessfulList, function (l, imgName) {
                    showPic(imgName, row);
                });

            },
            complete: function () {
                //$("#upload_supporting_docs").modal("toggle");
            },
        });
    });

    var _imgId = 0;
    var index = 2;
    var menu = "menu";

    function showPic(url, row) {

        index++;
        var rowNumer = row.find("#serial_no").text();
        //alert(parseFloat(rowNumer)-1);
        /*var newRow = row.closest("tr").clone(true);*/
        var picDiv = row.find('#_pics')[0];
        _imgId = document.querySelectorAll(".mymenu").length;

        _imgId++;
        // var df = picDiv.find("floatleft").clone(true);
        var container = document.createElement("div");
        container.className = "floatleft";
        container.style = "position:relative;";
        var img = document.createElement("img");
        var inputH = document.createElement("input");

        inputH.setAttribute("id", "prePurchaseEntries" + 3 + ".docPictrueList" + 0 + ".pictureUrl");
        inputH.setAttribute("name", "prePurchaseEntries[" + 1 + "].docPictrueList[" + 0 + "].pictureUrl");
        inputH.className = "pictureUrl";
        inputH.type = "hidden";
        inputH.value = url;
        img.style = "cursor :pointer; width:50px; height: 50px; float:left ";
        img.className = "ImageIcon";
        img.id = "img_" + _imgId;
        img.src = "/img/" + url;
        img.oncontextmenu = function () {
            displayCMenu(this, _imgId);
            return false;
        }
        img.onclick = function () {
            zooMer(this.src);
        }
        img.setAttribute("oncontextmenu", "displayCMenu(this," + _imgId + ");return false;");
        var uLcontainer = document.createElement("div");
        var ul = document.createElement("ul");
        ul.style = "position:relative ;width:80px";
        ul.id = "menu" + _imgId;
        var li_1 = document.createElement("li");
        var a = document.createElement("a");
        a.text = "Delete";
        a.href = "javascript:";
        //a.onclick="deletePic(this.parentNode.parentNode.parentNode.parentNode);";
        a.setAttribute("onclick", "deletePic(this.parentNode.parentNode.parentNode.parentNode);");

        li_1.appendChild(a);
        ul.appendChild(li_1);

        var li_2 = document.createElement("li");
        var a2 = document.createElement("a");
        a2.text = "Cancel";
        a2.href = "javascript:";
        a2.setAttribute("onclick", "$('.mymenu').hide();");
        li_2.appendChild(a2);
        ul.appendChild(li_2);

        ul.className = "mymenu";
        ul.id = menu + _imgId;

        uLcontainer.style = "position:absolute; width:50px;float:right";
        uLcontainer.appendChild(ul);
        container.appendChild(img);
        container.appendChild(inputH);
        //	container.appendChild(radio);
        container.appendChild(uLcontainer);
        picDiv.appendChild(container);


        AddOrDelRow();
    }

    AddOrDelRow();

    function AddOrDelRow() {
        var displayCMenu = 1;
        var menu = 1;

        $("#purchase_order_entry_table span#serial_no").each(function (index, element) {
            var rowNumer = $(element).closest("tr").find("#serial_no").text();
            //console.log("ss");
            var picDiv = $(element).closest("tr").find('#_pics')[0];

            for (var i = 0; i < picDiv.querySelectorAll(".floatleft").length; i++) {
                picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("id", "prePurchaseEntries" + (parseFloat(rowNumer) - 1) + ".docPictrueList" + i + ".pictureUrl");
                picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("name", "prePurchaseEntries[" + (parseFloat(rowNumer) - 1) + "].docPictrueList[" + i + "].pictureUrl");
                picDiv.querySelectorAll(".floatleft .ImageIcon")[i].setAttribute('oncontextmenu', "displayCMenu(this," + displayCMenu + ");return false;");
                picDiv.querySelectorAll(".floatleft .mymenu")[i].setAttribute('id', "menu" + displayCMenu);
                $("#menu" + displayCMenu).menu(
                    {position: {my: "right top", at: "right-5 top+10"}}
                );
                $("#menu" + displayCMenu).hide();
                displayCMenu++;
            }

        });

    }

    var checkboxClick = "true";
    //instrument select2 dropdowns
    $(".select2_single").on("select2:close", function () {
        $(this).focus();
    });

    $(".entry_add").on("click", function () {

        // un-instrument select2 dropdowns
        $(this).closest("tr").find(".select2_single").select2("destroy");

        var newRow = $(this).closest("tr").clone(true);

        //when you use clone, even the text is carried over. To remove it, do the following:
        newRow.children("td").children("input, span").each(function (index, element) {
            console.log("my object is ", element);
            $(element).val("");
            $(element).text("");
        });
        newRow.children("td").each(function (index, element) {
            if ($(this).children().length < 1) {
                $(element).text("");
            }
            //$(element).find(".itemCategory:first").val("");
            $(element).find(".itemDef:first").val("");
            $(element).find(".vehicalNo").val("");
            $(element).find(".prePurchaseCode").val("");
            $(element).find(".bag").val("");
            $(element).find(".kg").val("");
            $(element).find(".freight").val("");
            $(element).find(".rate").val("");
            $(element).find(".remarks").val("");

            $(element).find("#_pics .floatleft").remove();
        });

        newRow.insertAfter($(this).closest("tr"));

        // again instrument select2 dropdowns
        $("#purchase_order_entry_table .select2_single").select2({dropdownAutoWidth: true, width: '100%'});

        // update serial numbers
        $("#purchase_order_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "prePurchaseEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "prePurchaseEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "prePurchaseEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "prePurchaseEntries[" + index + "].itemDef");

            $(element).closest("tr").find("select[name*='.supplierAccount']").attr("id", "prePurchaseEntries" + index + ".supplierAccount.code");
            $(element).closest("tr").find("select[name*='.supplierAccount']").attr("name", "prePurchaseEntries[" + index + "].supplierAccount.code");

            $(element).closest("tr").find("select[name*='.millKhata']").attr("id", "prePurchaseEntries" + index + ".millKhata.id");
            $(element).closest("tr").find("select[name*='.millKhata']").attr("name", "prePurchaseEntries[" + index + "].millKhata.id");

            $(element).closest("tr").find("input[name*='.rate']").attr("id", "prePurchaseEntries" + index + ".rate");
            $(element).closest("tr").find("input[name*='.rate']").attr("name", "prePurchaseEntries[" + index + "].rate");

            $(element).closest("tr").find("input[name*='.prePurchaseCode']").attr("id", "prePurchaseEntries" + index + ".prePurchaseCode");
            $(element).closest("tr").find("input[name*='.prePurchaseCode']").attr("name", "prePurchaseEntries[" + index + "].prePurchaseCode");

            $(element).closest("tr").find("input[name*='.vehicalNo']").attr("id", "prePurchaseEntries" + index + ".vehicalNo");
            $(element).closest("tr").find("input[name*='.vehicalNo']").attr("name", "prePurchaseEntries[" + index + "].vehicalNo");

            $(element).closest("tr").find("input[name*='.bag']").attr("id", "prePurchaseEntries" + index + ".bag");
            $(element).closest("tr").find("input[name*='.bag']").attr("name", "prePurchaseEntries[" + index + "].bag");

            $(element).closest("tr").find("input[name*='.payementType']").attr("id", "prePurchaseEntries" + index + ".payementType");
            $(element).closest("tr").find("input[name*='.payementType']").attr("name", "prePurchaseEntries[" + index + "].payementType");


            $(element).closest("tr").find("input[name*='.freight']").attr("id", "prePurchaseEntries" + index + ".freight");
            $(element).closest("tr").find("input[name*='.freight']").attr("name", "prePurchaseEntries[" + index + "].freight");

            $(element).closest("tr").find("input[name*='.kg']").attr("id", "prePurchaseEntries" + index + ".kg");
            $(element).closest("tr").find("input[name*='.kg']").attr("name", "prePurchaseEntries[" + index + "].kg");

            $(element).closest("tr").find("input[name*='.remarks']").attr("id", "prePurchaseEntries" + index + ".remarks");
            $(element).closest("tr").find("input[name*='.remarks']").attr("name", "prePurchaseEntries[" + index + "].remarks");

            $(element).closest("tr").find("input[name*='.imagesUrl']").attr("id", "prePurchaseEntries" + index + ".imagesUrl");
            $(element).closest("tr").find("input[name*='.imagesUrl']").attr("name", "prePurchaseEntries[" + index + "].imagesUrl");

            $(element).closest("tr").find("input[name*='.entryDate']").attr("id", "prePurchaseEntries" + index + ".entryDate");
            $(element).closest("tr").find("input[name*='.entryDate']").attr("name", "prePurchaseEntries[" + index + "].entryDate");

            $(element).closest("tr").find("select[name*='.paymentType']").attr("id", "prePurchaseEntries" + index + ".paymentType");
            $(element).closest("tr").find("select[name*='.paymentType']").attr("name", "prePurchaseEntries[" + index + "].paymentType");

        });

        generateTabIndexing();
        calculateTotal();
        AddOrDelRow();
    });

    $(".entry_delete").on("click", function () {

        if ($(this).parents("tbody").find("tr").length > 1) {
            $(this).closest("tr").remove();
        }

        // update serial numbers
        $("#purchase_order_entry_table span#serial_no").each(function (index, element) {
            $(element).text(index + 1);

            $(element).closest("tr").find("input[name*='.id']").attr("id", "prePurchaseEntries" + index + ".id");
            $(element).closest("tr").find("input[name*='.id']").attr("name", "prePurchaseEntries[" + index + "].id");

            $(element).closest("tr").find("select[name*='.itemDef']").attr("id", "prePurchaseEntries" + index + ".itemDef");
            $(element).closest("tr").find("select[name*='.itemDef']").attr("name", "prePurchaseEntries[" + index + "].itemDef");

            $(element).closest("tr").find("select[name*='.supplierAccount']").attr("id", "prePurchaseEntries" + index + ".supplierAccount.code");
            $(element).closest("tr").find("select[name*='.supplierAccount']").attr("name", "prePurchaseEntries[" + index + "].supplierAccount.code");

            $(element).closest("tr").find("select[name*='.millKhata']").attr("id", "prePurchaseEntries" + index + ".millKhata.id");
            $(element).closest("tr").find("select[name*='.millKhata']").attr("name", "prePurchaseEntries[" + index + "].millKhata.id");

            $(element).closest("tr").find("input[name*='.rate']").attr("id", "prePurchaseEntries" + index + ".rate");
            $(element).closest("tr").find("input[name*='.rate']").attr("name", "prePurchaseEntries[" + index + "].rate");

            $(element).closest("tr").find("input[name*='.prePurchaseCode']").attr("id", "prePurchaseEntries" + index + ".prePurchaseCode");
            $(element).closest("tr").find("input[name*='.prePurchaseCode']").attr("name", "prePurchaseEntries[" + index + "].prePurchaseCode");

            $(element).closest("tr").find("input[name*='.vehicalNo']").attr("id", "prePurchaseEntries" + index + ".vehicalNo");
            $(element).closest("tr").find("input[name*='.vehicalNo']").attr("name", "prePurchaseEntries[" + index + "].vehicalNo");

            $(element).closest("tr").find("input[name*='.bag']").attr("id", "prePurchaseEntries" + index + ".bag");
            $(element).closest("tr").find("input[name*='.bag']").attr("name", "prePurchaseEntries[" + index + "].bag");

            $(element).closest("tr").find("input[name*='.payementType']").attr("id", "prePurchaseEntries" + index + ".payementType");
            $(element).closest("tr").find("input[name*='.payementType']").attr("name", "prePurchaseEntries[" + index + "].payementType");


            $(element).closest("tr").find("input[name*='.freight']").attr("id", "prePurchaseEntries" + index + ".freight");
            $(element).closest("tr").find("input[name*='.freight']").attr("name", "prePurchaseEntries[" + index + "].freight");

            $(element).closest("tr").find("input[name*='.kg']").attr("id", "prePurchaseEntries" + index + ".kg");
            $(element).closest("tr").find("input[name*='.kg']").attr("name", "prePurchaseEntries[" + index + "].kg");

            $(element).closest("tr").find("input[name*='.remarks']").attr("id", "prePurchaseEntries" + index + ".remarks");
            $(element).closest("tr").find("input[name*='.remarks']").attr("name", "prePurchaseEntries[" + index + "].remarks");

            $(element).closest("tr").find("input[name*='.imagesUrl']").attr("id", "prePurchaseEntries" + index + ".imagesUrl");
            $(element).closest("tr").find("input[name*='.imagesUrl']").attr("name", "prePurchaseEntries[" + index + "].imagesUrl");

            $(element).closest("tr").find("input[name*='.entryDate']").attr("id", "prePurchaseEntries" + index + ".entryDate");
            $(element).closest("tr").find("input[name*='.entryDate']").attr("name", "prePurchaseEntries[" + index + "].entryDate");

            $(element).closest("tr").find("select[name*='.paymentType']").attr("id", "prePurchaseEntries" + index + ".paymentType");
            $(element).closest("tr").find("select[name*='.paymentType']").attr("name", "prePurchaseEntries[" + index + "].paymentType");

        });
        AddOrDelRow();
        generateTabIndexing();
        calculateTotal();
    });

    $("#company").on("change", function () {
        loadCompanyBranches();
    });

    function loadCompanyBranches() {

        if (!$("#company").val() || $("#company").val() == 0) {
            return;
        }

        $.get("/vouchers/company_branches?companyId=" + $("#company").val(), function (data) {
            $("#branch").empty();
            $("#branch").append("<option></option>");

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].name + "</option>";
                $("#branch").append(option);
            }
        });

        //  getNextPurchaseOrderCode();
    }


    $("#branch").on("change", function () {
        //getNextPurchaseOrderCode();
    });


    function loadItemCategoryDefs(thisControl) {

        if (!$(thisControl).val() || $(thisControl).val() == 0) {
            return;
        }

        $(thisControl).closest("tr").find(".itemDef").empty();
        $(thisControl).closest("tr").find(".itemDef").append("<option>&emsp;</option>");

        $.get("/receivables/category_item_defs?categoryId=" + $(thisControl).val(), function (data) {

            for (var i = 0, len = data.length; i < len; i++) {
                var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name + "</option>";
                $(thisControl).closest("tr").find(".itemDef").append(option);
            }

            //$(thisControl).closest("tr").find(".itemDef").trigger("change.select2");
            //$(thisControl).closest("tr").find(".itemDef").find("#inputhidden input.select2-input").trigger("input");
        });
    }


    $('body').on('input', '.kg', function () {

        $(this).closest("tr").find(".ton").val($(this).closest("tr").find(".kg").val() / 1000);

    });

    $('body').on('input', '.ton', function () {

        $(this).closest("tr").find(".kg").val($(this).closest("tr").find(".ton").val() * 1000);

    });
    $('body').on('input', '.rate', function () {

        //$(this).closest("tr").find(".kg").val($(this).closest("tr").find(".ton").val()*1000);

    });

    function calculateTotal() {


    }

    function calcPercentage(amount, percentage) {
        return ((amount * percentage) / 100).toFixed(4);
    }

    $("#submitForm").off().on("click", function (event) {

        $("#submitForm").addClass('disabled');
        setTimeout(function () {
            $("#submitForm").removeClass('disabled');
        }, 5000);

        // Prevent the form from submitting via the browser.
        event.preventDefault();
        event.stopImmediatePropagation();
        // Will immediately show the confirmation popup
        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO SUBMIT THIS PRE PURCHASE ",
            buttons: {
                confirm: function () {
                    // SUBMIT FORM
                    $("#prepurchaseForm").submit();
                },
                cancel: function () {
                },
            }
        });
    });

    // SUBMIT FORM
    $("#prepurchaseForm").submit(function () {


        if (!$("#company").val() || $("#company").val() == 0 || !$("#branch").val() || $("#branch").val() == 0) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT COMPANY/BRANCH",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        var kgEmpty = false;
        var itemEmpty = false;
        var accountEmpty = false;
        var millKhataEmpty = false;
        var itemAmountZero = false;
        var itemDuplicateExists = false;
        var itemDuplicate = "";
        var itemArray = [];
        $("#purchase_order_entry_table tbody tr").each(function () {

            if (!$(this).find(".itemDef").val() || $(this).find(".itemDef").val() == 0) {
                itemEmpty = true;
            }
            if (!$(this).find(".kg").val() || $(this).find(".kg").val() == 0) {
                kgEmpty = true;
            }
//			if (parseFloat($(this).find(".amount").text()) == 0 ){
//				itemAmountZero = true;
//	    	}

//			if ($(this).find(".itemDef").val() && $.inArray($(this).find(".itemDef").val(), itemArray) >= 0){
//				itemDuplicateExists = true;
//				itemDuplicate = $(this).find(".itemDef option:selected").text();
//			}
            if (!$(this).find(".supplierAccount").val() || $(this).find(".supplierAccount").val() == 0) {
                accountEmpty = true;
                return false;
            }
            if (!$(this).find(".millKhata").val() || $(this).find(".millKhata").val() == 0) {
                millKhataEmpty = true;
                return false;
            } else {
                itemArray.push($(this).find(".itemDef").val());
            }
        });

        if (itemEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ITEM CAT/DEF FOR EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
        if (kgEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT TON  EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
        if (accountEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT ACCOUNT FOR EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
        if (millKhataEmpty) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "PLEASE SELECT MILL FOR EACH ROW",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if (itemAmountZero) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "ITEM AMOUNT MUST BE GREATER THEN ZERO",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        if (itemDuplicateExists) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "DUPLICATE ITEM EXISTS: " + itemDuplicate,
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }

        // prevent double submit
        if ($("#prepurchaseForm").data("submitted")) {
            return false;
        } else {
            $("#prepurchaseForm").data("submitted", true);
        }
    });

    generateTabIndexing();

    function generateTabIndexing() {
        var tabIndex = 0;
        $("#purchase_order_entry_table tbody td").each(function (i) {

            //console.log($(this).find(".supplierAccount").val());

            if ($(this).find("select.select2_single").length) {
                tabIndex = tabIndex + 1;
                $(this).find("select.select2_single:eq(0)").attr('tabindex', tabIndex);

            } else if ($(this).find("input:not(:hidden)").length) {
                tabIndex = tabIndex + 1;
                $(this).find("input:not(:hidden):eq(0)").attr('tabindex', tabIndex);

            } else if ($(this).find("a").length) {
                tabIndex = tabIndex + 1;
                $(this).find("a:eq(0)").attr('tabindex', tabIndex);

            }
        });
    }

    $("input[type=number]").on("focus", function () {
        $(this).on("keydown", function (event) {
            if (event.keyCode === 38 || event.keyCode === 40) {
                event.preventDefault();
            }
        });
    });

    $('input[type=number]').on('wheel', function (e) {
        return false;
    });

    //$("#sale_order_entry_table select, input:not(:hidden)").each(function (i) { $(this).attr('tabindex', i + 1); });


    // Catch the keydown for the entire document
    $(document).keydown(function (e) {

        // Set self as the current item in focus
        var self = $(':focus'),
            // Set the form by the current item in focus
            form = self.parents('form:eq(0)'),
            focusable;

        // Array of Indexable/Tab-able items
        //focusable = form.find('input').filter(':visible');
        focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
            .not(function () {   // do not include inputs with hidden parents
                return $(this).parent().is(':hidden');
            });

        function enterKey() {
            if (e.which === 13 && !self.is('textarea,div[contenteditable=true]')) { // [Enter] key

                // If not a regular hyperlink/button/textarea
                if ($.inArray(self, focusable) && (!self.is('button'))) {
                    // Then prevent the default [Enter] key behaviour from submitting the form
                    e.preventDefault();
                } // Otherwise follow the link/button as by design, or put new line in textarea

                if (self.is('a.entry_add')) {
                    self.click();
                    focusable = form.find(':input:enabled:not([readonly], input:hidden, button:hidden, textarea:hidden), textarea:enabled:not([readonly] textarea:hidden), a.entry_add')
                        .not(function () {   // do not include inputs with hidden parents
                            return $(this).parent().is(':hidden');
                        });
                }

                // Focus on the next item (either previous or next depending on shift)
                if (comingFromSelect2) {
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 0)).focus();
                    comingFromSelect2 = false;
                } else {
                    if (focusable.index(self) < 0) {
                        return false;
                    }
                    focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();
                }
                //focusable.eq(focusable.index(self) + (e.shiftKey ? -1 : 1)).focus();

                return false;
            }
        }

        // We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }
    });
    //aaa() ;

})