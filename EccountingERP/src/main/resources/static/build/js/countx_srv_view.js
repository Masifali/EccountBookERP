$( document ).ready(function() {
	
	$('#sale_journal_voucher_entry_table').DataTable( {
        dom: 'Bfrtip',
        buttons: [
        	{
                extend: 'print',
                text: 'PRINT',
                title: '',
                messageTop: $(".x_panel:eq(0)").html() + $(".x_panel:eq(1) .x_content:first").html() + "<br />",
                messageBottom: $("#po_footer").html(),
                footer: true,
                autoPrint: false,
                exportOptions: {
                    stripHtml: false,
                    stripNewlines: false,
                    columns: [ 0, 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                },
                customize: function (win) {
                    $(win.document.body).find('table').addClass('display').css('font-size', '12px');
                    $(win.document.body).css('background-color','WHITE');
                    $(win.document.body).css("color", "DIMGRAY");
                    /*$(win.document.body).find('h1').css('text-align', 'center');
                    $(win.document.body).find('div:first').css('text-align', 'center');*/
                    
                }
            }
        ],
        bPaginate: false,
        bFilter: false,
        bInfo: false,
    });
	
});