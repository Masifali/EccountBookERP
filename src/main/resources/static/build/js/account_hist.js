//var app = angular.module('accountHistApp', ['ngResource']);
var app = angular.module('accountHistApp', ['ngResource', 'datatables', 'datatables.buttons']);

app.controller('AccountsController', ['$scope', '$http', '$filter', 'DTOptionsBuilder', 'DTColumnBuilder', 'DTColumnDefBuilder', function ($scope, $http, $filter, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

    $("#account").on("change", function () {
        $("#upper_limit").text($filter("currency")($('#accounts [value="' + $("#account").val() + '"]').data("limit"), "", 2));
        getAccountHistByAccountCode();
    });

    function getAccountHistByAccountCode() {
        $http({
            method: 'GET',
            url: location.protocol + "//" + location.host + "/accounts/account_hist/accountCode/" + $('#accounts [value="' + $("#account").val() + '"]').data("val")
        }).then(function successCallback(response) {
            // this callback will be called asynchronously
            // when the response is available
            $scope.accountHists = response.data;

        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });
    }

    $scope.addAccountHist = function () {

        $scope.accountHistForm.id = "";
        $scope.accountHistForm.account = $("#account").val();
        $scope.accountHistForm.customerDetail = "";
        $scope.accountHistForm.totalReturnOrBalance = "";
        $scope.accountHistForm.modeOfPayment = "";
        $scope.accountHistForm.pendingFromCustomer = "";
        $scope.accountHistForm.scheduleOfPaymentAndOrder = "";
        $("#add_new_account_hist").modal("toggle");

    };

    $scope.loadAccountHist = function (accountHist) {

        $("#add_new_account_hist").modal("toggle");
        $scope.accountHistForm.id = accountHist.id;
        $scope.accountHistForm.account = $('#accounts [data-val="' + accountHist.accountCode + '"]').val();
        $scope.accountHistForm.customerDetail = accountHist.customerDetail;
        $scope.accountHistForm.totalReturnOrBalance = accountHist.totalReturnOrBalance;
        $scope.accountHistForm.modeOfPayment = accountHist.modeOfPayment;
        $scope.accountHistForm.pendingFromCustomer = accountHist.pendingFromCustomer;
        $scope.accountHistForm.scheduleOfPaymentAndOrder = accountHist.scheduleOfPaymentAndOrder;

    };

    $scope.editAccountHist = function () {

        //if (!$("#party").val() || $('#accounts [value="' + $("#party").val() + '"]').length < 1 || !$("#customerDetail").val() ){
        if (!$("#party").val() || $('#accounts [value="' + $("#party").val() + '"]').length < 1) {
            $.confirm({
                title: "ENCOUNTERED AN ERROR!",
                content: "INPUT DATA IS INCOMPLETE",
                type: 'red',
                typeAnimated: true,
            });
            return false;
        }

        var accountHist = {};
        accountHist.id = $scope.accountHistForm.id;
        accountHist.accountCode = $('#accounts [value="' + $("#party").val() + '"]').data("val");
        accountHist.customerDetail = $scope.accountHistForm.customerDetail;
        accountHist.totalReturnOrBalance = $scope.accountHistForm.totalReturnOrBalance;
        accountHist.modeOfPayment = $scope.accountHistForm.modeOfPayment;
        accountHist.pendingFromCustomer = $scope.accountHistForm.pendingFromCustomer;
        accountHist.scheduleOfPaymentAndOrder = $scope.accountHistForm.scheduleOfPaymentAndOrder;

        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO UPDATE THIS ACCOUNT HISTORY?",
            buttons: {
                confirm: function () {

                    $http({
                        method: 'POST',
                        url: location.protocol + "//" + location.host + "/accounts/account_hist",
                        data: accountHist
                    }).then(function successCallback(response) {
                        // this callback will be called asynchronously
                        // when the response is available
                        $("#add_new_account_hist").modal("toggle");
                        $scope.Message = response.data;
                        getAccountHistByAccountCode();
                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                        $scope.Message = response.data;
                    });

                },
                cancel: function () {
                },
            }
        });

    };

    $scope.deleteAccountHist = function (accountHistId) {

        $.confirm({
            title: "CONFIRMATION REQUIRED",
            content: "ARE YOU SURE YOU WANT TO DELETE THIS ACCOUNT HISTORY?",
            buttons: {
                confirm: function () {

                    $http({
                        method: 'DELETE',
                        url: location.protocol + "//" + location.host + "/accounts/account_hist/delete/" + accountHistId
                    }).then(function successCallback(response) {
                        // this callback will be called asynchronously
                        // when the response is available
                        $scope.Message = response.data;
                        getAccountHistByAccountCode();
                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                        $scope.Message = response.data;
                    });

                },
                cancel: function () {
                },
            }
        });

    };


    $scope.vm = {};
    $scope.vm.dtInstance = {};
    $scope.vm.dtColumnDefs = [DTColumnDefBuilder.newColumnDef(2).notSortable()];
    $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('paging', false)
        .withOption('searching', false)
        .withOption('info', false)
        .withOption('dom', 'Bfrtip')
        .withButtons([]);
    /*.withButtons([
                        {
                            extend:    'copy',
                            text:      '<i class="fa fa-files-o"></i> Copy',
                            titleAttr: 'Copy',
                            exportOptions: {
                                columns: 'th:not(:last-child)'
                            }
                        },
                        {
                            extend:    'excel',
                            text:      '<i class="fa fa-file-text-o"></i> Excel',
                           titleAttr: 'Excel',
                           exportOptions: {
                               columns: 'th:not(:last-child)'
                           }
                        },
                        {
                            extend:    'print',
                            text:      '<i class="fa fa-print" aria-hidden="true"></i> Print',
                            titleAttr: 'Print',
                            exportOptions: {
                                columns: 'th:not(:last-child)'
                            }
                        }
                    ]);*/

}]);