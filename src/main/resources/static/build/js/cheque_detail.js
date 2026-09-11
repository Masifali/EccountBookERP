var app = angular.module('chequeDetailApp', ['ngResource']);

app.controller('ReportsController', ['$scope', '$http', '$filter', function($scope, $http, $filter) {
	
	$("#chequeInHandAccounts").on("change", function(){
		fetchAllChequeDetails();
	});
	
    function fetchAllChequeDetails(){
    	$http({
    		  method: 'GET',
    		  url: location.protocol + "//" + location.host + "/reports/cheque_details/account/"+$("#chequeInHandAccounts").val()
    		}).then(function successCallback(response) {
    		    // this callback will be called asynchronously
    		    // when the response is available
    			$scope.chequeDetails = response.data;
    			
    			$scope.accountBalance = $filter("number")($("#chequeInHandAccounts option:selected").attr("data-value"), 2);
    			$scope.accountDifference = $filter("number")($("#chequeInHandAccounts option:selected").attr("data-value") - $scope.getTotal(), 2);
    			
    		  }, function errorCallback(response) {

    		  });
    }
    
    fetchAllChequeDetails();
    
    $scope.addNewCheque = function(){
    	
    	$scope.chequeDetailForm.id = "";
    	$scope.chequeDetailForm.account = "";
    	$scope.chequeDetailForm.chequeInHandAccount = $("#chequeInHandAccounts").val();
    	$scope.chequeDetailForm.chequeDate = "";
    	$scope.chequeDetailForm.chequeNumber = "";
    	$scope.chequeDetailForm.narration = "";
    	$scope.chequeDetailForm.amount = "";
    	$("#add_new_cheque").modal("toggle");
    	
    };
    
    $scope.loadChequeDetail = function(chequeDetail){
    	
    	$("#add_new_cheque").modal("toggle");
    	//$("#party").val($('#accounts [data-val="' + chequeDetail.account.code + '"]').val());
    	$scope.chequeDetailForm.id = chequeDetail.id;
    	$scope.chequeDetailForm.account = $('#accounts [data-val="' + chequeDetail.account.code + '"]').val();
    	$scope.chequeDetailForm.chequeInHandAccount = chequeDetail.chequeInHandAccount.toString();
    	$scope.chequeDetailForm.chequeDate = new Date(chequeDetail.chequeDate);
    	$scope.chequeDetailForm.chequeNumber = chequeDetail.chequeNumber;
    	$scope.chequeDetailForm.narration = chequeDetail.narration;
    	$scope.chequeDetailForm.amount = chequeDetail.amount;
    	//$('#accounts [value="' + $("#party").val() + '"]').data('val')
    	//chequeDetail.account = $scope.chequeDetailForm.account;
    	
    };
    
    $scope.editChequeDetail = function(){
    	
    	if (!$("#party").val() || $('#accounts [value="' + $("#party").val() + '"]').length < 1 || !$("#chequeDate").val() || !$("#chequeNumber").val() || !$("#narration").val() || !$("#amount").val()){
    		$.confirm({
			    title: "ENCOUNTERED AN ERROR!",
			    content: "INPUT DATA IS INCOMPLETE",
			    type: 'red',
			    typeAnimated: true,
			});
    		return false;
    	}
    	
    	var chequeDetail = {};
    	chequeDetail.id = $scope.chequeDetailForm.id;
    	chequeDetail.account = {"code": $('#accounts [value="' + $("#party").val() + '"]').data("val")};
    	chequeDetail.chequeInHandAccount = $scope.chequeDetailForm.chequeInHandAccount;
    	chequeDetail.chequeDate = $scope.chequeDetailForm.chequeDate;
    	chequeDetail.chequeNumber = $scope.chequeDetailForm.chequeNumber;
    	chequeDetail.narration = $scope.chequeDetailForm.narration;
    	chequeDetail.amount = $scope.chequeDetailForm.amount;
    	
    	$.confirm({
    	    title: "CONFIRMATION REQUIRED",
    	    content: "ARE YOU SURE YOU WANT TO UPDATE THIS CHEQUE?",
    	    buttons: {
    	        confirm: function () {
    	        	
    	        	$http({
    	        		  method: 'POST',
    	        		  url: location.protocol + "//" + location.host + "/reports/cheque_details",
    	        		  data: chequeDetail
    	        		}).then(function successCallback(response) {
    	        		    // this callback will be called asynchronously
    	        		    // when the response is available
    	        			$("#add_new_cheque").modal("toggle");
    	        			$scope.Message = response.data;
    	        			fetchAllChequeDetails();
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
    
    $scope.deleteChequeDetail = function(chequeDetailId){
    	
    	$.confirm({
    	    title: "CONFIRMATION REQUIRED",
    	    content: "ARE YOU SURE YOU WANT TO DELETE THIS CHEQUE?",
    	    buttons: {
    	        confirm: function () {
    	        	
    	        	$http({
    	        		  method: 'DELETE',
    	        		  url: location.protocol + "//" + location.host + "/reports/cheque_details/delete/" + chequeDetailId
    	        		}).then(function successCallback(response) {
    	        		    // this callback will be called asynchronously
    	        		    // when the response is available
    	        			$scope.Message = response.data;
    	        			fetchAllChequeDetails();
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
    
    $scope.getTotal = function(){
        var total = 0;
        if($scope.chequeDetails){
        	for(var i = 0; i < $scope.chequeDetails.length; i++){
                total = total + $scope.chequeDetails[i].amount;
            }
        }
        return total;
    }
    
}]);