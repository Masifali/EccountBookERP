$( document ).ready(function() {
	
	function getFormData($form){
	    var unindexed_array = $form.serializeArray();
	    var indexed_array = {};

	    $.map(unindexed_array, function(n, i){
	        indexed_array[n['name']] = n['value'];
	    });

	    return indexed_array;
	}
	
	// update serial numbers
	$("table").each(function(i){
		$(this).find("span#serial_no").each(function(index, element){
			$(element).text(index + 1); 
	    });
    });
	
	// CLEAR MILL MODAL FORM
	$("#add_city").on("hidden.bs.modal", function () {
		$("#addCityForm").find("input").val("");
		$("#addCityForm").trigger("reset");
	})
	
	// UPDATE MILL
    $(".city_edit").on("click", function () {
    	// SET ITEM CATEGORY FIELDS
    	$("#addCityForm #id").val($(this).closest("tr").find(".city_id:first").val());
    	$("#addCityForm #accountName").val($(this).closest("tr").find(".city_name:first").val());
    	$("#add_city").modal("show");
    });
	
    function getFormattedDate(date) {
		var year = date.getFullYear();
		var month = (1 + date.getMonth()).toString();
		month = month.length > 1 ? month : '0' + month;
		var day = date.getDate().toString();
		day = day.length > 1 ? day : '0' + day;
		return day + '/' + month + '/' + year;
	}
    
  
   
    
});