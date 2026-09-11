window.onload = function () {
    //loadTwoLevelAccounts();
    // $('#accountDataDiv').hide();
};

var getFocusVehicalNumber = parseFloat(0);
$("#partyName").focus();

$(".itemDef").on("change", function () {

    $("#itemDef").focus();
    getFocusVehicalNumber = 1;

});
$(".branch").on("change", function () {

    $("#branch").focus();
    getFocusVehicalNumber = 3;

});
var weightComplete = false;

function calculateTotal() {

    var katoti = parseFloat(0);
    var totalKatoti = parseFloat(0);
    var bags = parseFloat(0);
    var bagWeightDeduction = parseFloat(0);
    var totalBagWeightDeduction = parseFloat(0);
    var safiKg = parseFloat(0);

    var qtyInMan = parseFloat(0);
    var firstWeight = parseFloat(0);
    var secondWeight = parseFloat(0);
    var grossWeight = parseFloat(0);
    var kgs = parseFloat(0);

    secondWeight = parseFloat($("#secondWeight").val());
    firstWeight = parseFloat($("#firstWeight").val());
    grossWeight = firstWeight - secondWeight;
    $("#grossWeight").val(grossWeight);

    bags = $("#bags").val();
    // bagWeightDeduction = $("#bagWeightDeduction").val();
    // totalBagWeightDeduction = (bags * bagWeightDeduction).toFixed(0);
    // $("#totalBagWeightDeduction").val(totalBagWeightDeduction);

    //katoti = $("#katoti").val();
    // totalKatoti = katoti * bags;
    // $("#totalKatoti").val(totalKatoti);

    //safiKg = grossWeight;// - totalKatoti - totalBagWeightDeduction;
    $("#netWeight").val(grossWeight);
    qtyInMan = grossWeight / 40;

    var int_part = Math.trunc(qtyInMan); // returns 3
    var float_part = Number((qtyInMan - int_part).toFixed(2));
    kgs = float_part * 40;
    $("#mun").val(int_part);
    kgs = (parseFloat(float_part) * 40).toFixed(0);
    $("#kg").val((parseFloat(float_part) * 40).toFixed(0));
}


$("#firstWeight").on("input", function () {
    calculateTotal();
});
$("#secondWeight").on("input", function () {
    if (parseFloat($("#firstWeight").val()) < parseFloat($("#secondWeight").val())) {
        // $("#secondWeight").val(0);
    }
    calculateTotal();
});
$("#bags").on("input", function () {
    calculateTotal();
});
$("#katoti").on("input", function () {
    calculateTotal();
});
$("#bagWeightDeduction").on("input", function () {
    calculateTotal();
});

function downloadWeightPrint() {
    var url = "/kanta/downlaodWeight/?number=" + $("#bridgeId").val() + "&code=" + $("#code").val();
    // window.open("/kanta/downlaodWeight/?number=" + $("#bridgeId").val(), '_blank');
    window.open(url);
    var printWindow = window.open(url, "_blank");
    //  printWindow.print();
    /* printWindow.onload = function() {
         printWindow.print();
     };*/
}

$("#printweight").on('click', function () {
    downloadWeightPrint();
});
$("#bridgeId").on('input', function () {

    // if($("#bridgeId").val()===0 ||$("#bridgeId").val()==="") {
    $("#id").val(0);
    //      }
    secondChangeAble();
    /*if(weightComplete){
    if($("#id").val()<1 ||$("#id").val()==="") {*/
    weightComplete = false;
    // }
    // }
});

function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        //console.log(indexed_array[n['name']]);
        if (n['name'] === 'branch') {
            indexed_array[n['name']] = {id: n['value']};
        } else if (n['name'] === 'supplierAccount') {
            indexed_array[n['name']] = {code: n['value']};
        } else if (n['name'] === 'itemDef') {
            indexed_array[n['name']] = {id: n['value']};
        } else if (n['name'] === 'financialYear') {
            indexed_array[n['name']] = {id: n['value']};
        } else {
            indexed_array[n['name']] = n['value'];
        }
    });

    return indexed_array;
}

secondReadOnly();

function secondReadOnly() {
    $("#secondWeight").prop("readonly", true);
    $("#secondDate").prop("readonly", true);
    $("#secondTime").prop("readonly", true);
}

function secondChangeAble() {
    $("#secondWeight").prop("readonly", false);
    $("#secondDate").prop("readonly", false);
    $("#secondTime").prop("readonly", false);
}

$(document).ready(function () {

    $("#itemDef").focus();
    $("#itemDef").select2("open");
    // $("#itemDef").addClass("select2-offscreen").show();

    $('#sDate').val(new Date().toISOString().substring(0, 10));
    // $('#time').val(new Date().toISOString().substring(10, 18));
    var intervalId = window.setInterval(function () {
        getWeight();
    }, 500);

    function getWeight() {
        $.get("/kanta/get-current-weight", function (data) {
            $("#weightInKg").val(data + " KG");
            if (weightComplete) {
                $("#firstWeight").prop("readonly", true);
                $("#firstDate").prop("readonly", true);
                $("#firstTime").prop("readonly", true);
                $("#partyWeight").prop("readonly", true);

                secondReadOnly();
                if ($("#password").val() === "4269601") {
                    $("#firstWeight").prop("readonly", false);
                    $("#firstDate").prop("readonly", false);
                    $("#firstTime").prop("readonly", false);
                    $("#partyWeight").prop("readonly", false);
                    secondChangeAble();
                }


            } else {
                if ($("#MrA").val() === "A") {
                    if (parseFloat($("#bridgeId").val()) > 0) {
                        $("#secondWeight").val(data);
                        $("#firstWeight").prop("readonly", true);
                        $("#partyWeight").prop("readonly", true);
                        $("#firstDate").prop("readonly", true);
                        secondChangeAble();
                    } else {
                        $("#firstWeight").val(data);
                        $("#firstWeight").prop("readonly", false);
                        $("#partyWeight").prop("readonly", false);
                        $("#firstDate").prop("readonly", false);
                        secondReadOnly();
                    }
                }


                //secondChangeAble();
            }
            calculateTotal();

        });
    }

    $(".select2_single").on("select2:close", (function () {
            $(this).focus();
        })
    );

// SUBMIT FORM
    $("#weightDataForm").submit(function (event) {
        // Prevent the form from submitting via the browser.
        event.preventDefault();
        addOrUpdateWeightData();
    });


    function addOrUpdateWeightData() {
        console.log("ssdd");
        if ($("#bridgeId").val() > 0 && ($("#id").val() == 0 || $("#id").val().trim() === "")) {
            $.confirm({
                title: "آپ کا برج آئی ڈی ٹھیک نہیں ہے",
                content: "بریج آئی ڈی کو ختم کریں یا پھر ٹھیک لکھیں",
                type: 'red',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
        if ($("#itemDef").val() == 0) {
            $.confirm({
                title: "Kindly Select the Item",
                content: "With out Item Entry Will not be saved",
                type: 'blue',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
        if ($("#inOut").val() == 0) {
            $.confirm({
                title: "Kindly Select IN or OUT",
                content: "Kindly tell me this transcetion is IN or OUR",
                type: 'green',
                typeAnimated: true,
            });
            // Prevent the form from submitting via the browser.
            return false;
        }
        if ($("#partyWeight").val() == null || $("#partyWeight").val() == 0) {
            $.confirm({
                title: "Kindly Select Party weight",
                content: "Kindly enter the party weight",
                type: 'green',
                typeAnimated: true,
            });
            $("#partyWeight").val().focus();
            // Prevent the form from submitting via the browser.
            return false;
        }
        var weightDataForm = $('#weightDataForm');

        jsonData = getFormData(weightDataForm);
        jsonData.se = "asif  idres";
        console.log(jsonData);

        // DO POST
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: weightDataForm.attr('action'),
            data: JSON.stringify(jsonData),
            dataType: 'json',
            success: function (data) {
                // $("#modal_success").modal('show');
                $("#bridgeId").val(data.number);
                downloadWeightPrint();

                location.reload();
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


    function showWeightEntry() {


        $.get("/kanta/findyWeightEntryById?number=" + $("#bridgeId").val() + "&code=" + $("#code").val(), '_blank', function (data) {

            // $.get("/kanta/findyWeightEntryById?weightId=" + $("#bridgeId").val(), function (data) {

            if (data.secondWeight > 0) {
                weightComplete = true;
            }
            var now = new Date(data.firstDate);
            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = now.getFullYear() + "-" + (month) + "-" + (day);
            // $("#weightDataForm #id").val(data.id);
            $("#weightDataForm #secondWeight").val(data.secondWeight);
            $("#weightDataForm #firstWeight").val(data.firstWeight);
            $("#weightDataForm #firstDate").val(today);
            $("#weightDataForm #firstDate").val(today);
            $("#weightDataForm #partyWeight").val(data.partyWeight);

            // set second date formate
            now = new Date(data.secondDate);
            day = ("0" + now.getDate()).slice(-2);
            month = ("0" + (now.getMonth() + 1)).slice(-2);
            today = now.getFullYear() + "-" + (month) + "-" + (day);
            $("#weightDataForm #secondDate").val(today);

            if (data.itemDef != null) {
                $("#weightDataForm #itemDef").val(data.itemDef.id).trigger("change.select2");
            } else {

                $("#weightDataForm #itemDef").val(0);
            }
            if (data.branch != null) {
                $("#weightDataForm #branch").val(data.branch.id).trigger("change");
            } else {

                $("#weightDataForm #branch").val(0);
            }

            if (data.supplierAccount != null) {
                $("#weightDataForm #supplierAccount").val(data.supplierAccount.code).trigger("change.select2");
            } else {

                $("#weightDataForm #supplierAccount").val(0);
            }
            $("#weightDataForm #secondTime").val(data.secondTime);
            $("#weightDataForm #firstTime").val(data.firstTime);
            $("#weightDataForm #mobile").val(data.mobile);
            $("#weightDataForm #id").val(data.id);
            $("#weightDataForm #amount").val(data.amount);
            $("#weightDataForm #bags").val(data.bags);
            $("#weightDataForm #remarks").val(data.remarks);
            $("#weightDataForm #vehicalNo").val(data.vehicalNo);
            $("#weightDataForm #number").val(data.number);
            $("#weightDataForm #code").val(data.code);
            $("#weightDataForm #inOut").val(data.transcensionType);
            // console.log(data.supplierAccount);


        });
        calculateTotal();
    }

    $(document).keydown(function (e) {


        console.log("asif idrees");
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
            let key = e.charCode || e.keyCode || 0 //get the key code

            if (key == 13 && !e.shiftKey) { //If enter key
                e.preventDefault();
                var code = e.which,
                    elm = document.activeElement, //capture the current element for later
                    currentTabIndex = elm.tabIndex,

                    nextTabIndex = code == 13 ? currentTabIndex + 1 : null
                $('[tabindex]').filter(function () {
                    nextTabIndex = nextTabIndex - getFocusVehicalNumber;
                    return this.tabIndex == nextTabIndex;
                }).focus();
                getFocusVehicalNumber = 0;	// this use to remove default fuscus of selected class
            } else if (key == 13 && e.shiftKey) { //If enter key
                e.preventDefault();

                var code = e.which,

                    elm = document.activeElement, //capture the current element for later
                    currentTabIndex = elm.tabIndex,

                    nextTabIndex = code == 13 ? currentTabIndex - 1 : null
                $('[tabindex]').filter(function () {
                    console.log("key " + currentTabIndex);
                    return this.tabIndex == nextTabIndex;
                }).focus();
            }

        }

        // We need to capture the [Shift] key and check the [Enter] key either way.
        if (e.shiftKey) {
            enterKey()
        } else {
            enterKey()
        }

        if (e.keyCode == 13) {
            // e.preventDefault();
            if ($("#bridgeId").is(":focus")) {
                showWeightEntry();
            }
            /* if ($("#saved").is(":focus")) {
                 e.preventDefault();
                 addOrUpdateWeightData();
             }*/
        }
        if (e.altKey && e.keyCode == 83) {
            e.preventDefault();
            addOrUpdateWeightData();
        }
        if (e.altKey && e.keyCode == 80) {
            e.preventDefault();
            downloadWeightPrint();
        }
    });
    /*showWeightEntry()()
    {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "/kanta/findyWeightEntryById",
            data: JSON.stringify(jsonData),
            dataType: 'json',
            success: function (data) {

            },
        });
    }*/
});