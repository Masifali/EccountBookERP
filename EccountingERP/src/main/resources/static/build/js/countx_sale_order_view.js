function getFormData($form){
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function(n, i){
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

$( document ).ready(function() {
	
	var table = $('#sale_order_entry_table').DataTable( {
        dom: 'Bfrtip',
        'scrollX': true,
        //columnDefs: [
//            {
//                "targets": [8],
//                "visible": false,
//            }
//        ],
        buttons: [
        	{
                extend: 'print',
                text: 'PRINT',
                title: '',
                messageTop: $(".x_panel:first").html(),
                messageBottom: $("#so_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                   // columns: [ 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13],
                },
                customize: function (win) {
                	$(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                    $(win.document.body).css('background-color','WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');*/
                    /*$(win.document.body).find('div:first').css('text-align', 'center');*/
                    
                }
            },
           
        	{
                extend: 'print',
                text: 'PRINT TABLE',
                title: '',
                messageTop: $("#invoice_header").html(),
                messageBottom: $("#so_footer_delivery").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
//                    /columns: [ 0, 1, 2, 3, 5, 14],
                },
                customize: function (win) {
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('padding', '1px 2px 1px 2px');
                    $(win.document.body).find('table:eq(2) tbody tr td,th').addClass('display').css('vertical-align', 'middle');
                    $(win.document.body).find('table:eq(2)').addClass('display').css('font-size', '16px');
                    $(win.document.body).css('background-color','WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');*/
                    /*$(win.document.body).find('div:first').css('text-align', 'center');*/
                    
                }
            }
        ],
        bPaginate: false,
        bFilter: false,
        bInfo: false,
    });
	
	/*if ($("#errorMessagesExists").length > 0){
		$("div.dt-buttons").hide();
	}*/

	//highlight
	$('#sale_order_entry_table tbody').on( 'mouseenter', 'td', function () {
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
	
	
	$("#mark_delivered").on("click", function (event) {
		// Will immediately show the confirmation popup
		$.confirm({
    	    title: "CONFIRMATION REQUIRED",
    	    content: "ARE YOU SURE YOU WANT TO MARK THIS SALE ORDER AS DELIVERED?",
    	    buttons: {
    	        confirm: function () {
    	        	toggleSaleOrderDelivered();
    	        },
    	        cancel: function () {
    	        },
    	    }
    	});
	});
	
	function toggleSaleOrderDelivered() {
		saleOrderId = $("#sale_order_id").val();
	    $.get( "/sale_orders/toggle_sale_order_delivered?saleOrderId="+saleOrderId, function(data) {
	    	location.reload();
	    });
	}
	
})