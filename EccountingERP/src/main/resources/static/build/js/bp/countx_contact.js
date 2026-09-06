

$( document ).ready(function() {

	for(var i=1;i<=document.querySelectorAll(".mymenu").length;i++)
 	{
     $("#menu"+i).menu(
					{ position: { my: "left top", at: "right-5 top"}});
     $("#menu"+i).hide();
    }
	
	var _imgId=0;
    var index =2;
    var menu="menu";
    
    function showPic(url,row){
   	   	index++;
    	var rowNumer  = row.find("#serial_no").text();
    	//alert(parseFloat(rowNumer)-1);
    	/*var newRow = row.closest("tr").clone(true);*/
   	 	var picDiv=row.find('#_pics')[0];
   	 	_imgId = document.querySelectorAll(".mymenu").length;
        
        _imgId++;
      // var df = picDiv.find("floatleft").clone(true);
    	var container = document.createElement("div");
    	container.className="floatleft";
    	container.style="position:relative;";
    	var img  = document.createElement("img");
    	var inputH = document.createElement("input");
    
    	inputH.setAttribute("id", "prePurchaseEntries"+3+".docPictrueList"+0+".pictureUrl");
    	inputH.setAttribute("name", "prePurchaseEntries["+1+"].docPictrueList["+0+"].pictureUrl");
    	inputH.className="pictureUrl";
    	inputH.type="hidden";
    	inputH.value=url;
    	img.style="cursor: pointer; width: 50px; height: 50px; float: left;";
    	img.className="ImageIcon";
    	img.id="img_"+_imgId;
    	img.src="/img/"+url;
    	img.oncontextmenu=function(){displayCMenu(this,_imgId);return false;}
    	img.onclick = function(){
	           zooMer(this.src);
            }
    	img.setAttribute("oncontextmenu" , "displayCMenu(this,"+_imgId+");return false;");
     	var uLcontainer = document.createElement("div");
    	var ul = document.createElement("ul");
    	    ul.style="position:relative ;width:80px";
    	    ul.id = "menu"+_imgId;
    	var li_1  = document.createElement("li");
    	var a  = document.createElement("a");
    	a.text ="Delete";
    	a.href="javascript:";
    	//a.onclick="deletePic(this.parentNode.parentNode.parentNode.parentNode);";
    	a.setAttribute("onclick", "deletePic(this.parentNode.parentNode.parentNode.parentNode);");
    
	    li_1.appendChild(a);       
    	ul.appendChild(li_1);
    	
    	var li_2  = document.createElement("li");
    	var a2  = document.createElement("a");
    	a2.text ="Cancel";
    	a2.href="javascript:";
       a2.setAttribute("onclick", "$('.mymenu').hide();");
    	li_2.appendChild(a2);
    	ul.appendChild(li_2);
    	
    	ul.className ="mymenu";
    	ul.id =menu+_imgId;
    	
    	uLcontainer.style="position:absolute; width:50px;float:right";
    	uLcontainer.appendChild(ul);
    	container.appendChild(img);
    	container.appendChild(inputH);
    //	container.appendChild(radio);
    	container.appendChild(uLcontainer);
       	picDiv.appendChild(container);
   		AddOrDelRow();
   }
   AddOrDelRow();
   function AddOrDelRow(){
   		var displayCMenu =1;
    	var menu =1;
    	 //span#serial_no
    $("#contact_images").each(function(index, element){	
    		var rowNumer = $(element).closest("td").find("#serial_no").text();
    		//console.log("ss");
   	 		//var picDiv = $(element).closest("td").find('#_pics')[0];
    		var picDiv = $(element).closest("td");
    		//console.log(picDiv);
//           for(var i =0;i<picDiv.querySelectorAll(".floatleft").length;i++)
//            {
//	          picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("id", "prePurchaseEntries"+(parseFloat(rowNumer)-1)+".docPictrueList"+i+".pictureUrl");
//	          picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("name", "prePurchaseEntries["+(parseFloat(rowNumer)-1)+"].docPictrueList["+i+"].pictureUrl");
//	          picDiv.querySelectorAll(".floatleft .ImageIcon")[i].setAttribute('oncontextmenu', "displayCMenu(this,"+displayCMenu+");return false;");
//	          picDiv.querySelectorAll(".floatleft .mymenu")[i].setAttribute('id',"menu"+displayCMenu);
//	          $("#menu"+displayCMenu).menu({ position: { my: "right top", at: "right-5 top+10" } });
//		      $("#menu"+displayCMenu).hide();
//              displayCMenu++;
//	         }
      });
   }
	
	$(".city").on("change", function(){
		loadLinkedCities($(this));
	});

	function loadLinkedCities(thisControl) {

		if (!$(thisControl).val() || $(thisControl).val() == 0){
			return;
		}

		$(thisControl).closest("tr").find(".linkedcontanct").empty();
		$(thisControl).closest("tr").find(".linkedcontanct").append("<option>&emsp;</option>");
		console.log("Citiies " + $(thisControl).val());
		$.get( "/bp/getContactByCity?city=" + $(thisControl).val(), function( data ) {

			for (var i = 0, len = data.length; i < len; i++) {
				var option = "<option value = " + data[i].id + ">" + data[i].firstName + "&emsp;" + data[i].lastName + "&emsp;" + data[i].companyName + "&emsp;" + data[i].city +  "</option>";
				$(thisControl).closest("tr").find(".linkedcontanct").append(option);
			}

			//$(thisControl).closest("tr").find(".itemDef").trigger("change.select2");
			//$(thisControl).closest("tr").find(".itemDef").find("#inputhidden input.select2-input").trigger("input");
		});
	}

	$(".loadConversation").on("click", function(){
		var myID = $(this).closest("tr").find(".id").val();
		var myText = $(this).closest("tr").find(".mySpanText").text();
		$("#txtConversation").val(myText);
		$("#txtConID").val(myID);
		
	});
	
	$("#btnSaveConversation").on("click", function(){
		var myText = $("#txtConversation").val();
		var myID = $("#txtConID").val();
		var myContactID = $(".id").val();
		
		if(myText === null || myText.length === 0 || myText === ""){
			alert("Please! Enter Conversation Text");
			$("#txtConversation").focus();
			return;
		}
		
		var formData = {
			
				id: myID,
				talking: myText,
				contId:myContactID
        }
		
		console.log(formData);
		$.ajax({
			type : "POST",
			contentType : "application/json",
			url:"/bp/updateConversation",
			data : JSON.stringify(formData),
			dataType : "text",
	      
	         success:function(data)
	         {
	        	 $("#txtConversation").val('');
	         }
		 }); 
		 
		//alert("Click on Conversaion Save " + myID + " text " + myText + " contact id " + myContactID);
	});

	$(".add_number").on("click", function () {

		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_single").select2("destroy");

		var newRow = $(this).closest("tr").clone(true);
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			//	$(element).find(".itemCategory:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			$(element).find(".type:first").val("");
		});

		newRow.insertAfter($(this).closest("tr"));
		$("#contactNumber .select2_single").select2(
				{
					dropdownAutoWidth : true, width: '100%'
				});
		// update serial numbers
		$("#contactNumber span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "contactNumbers" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "contactNumbers[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.type']" ).attr("id", "contactNumbers" + index + ".type");
			$(element).closest("tr").find( "select[name*='.type']" ).attr("name", "contactNumbers[" + index + "].type");

			$(element).closest("tr").find( "input[name*='.number']" ).attr("id", "contactNumbers" + index + ".number");
			$(element).closest("tr").find( "input[name*='.number']" ).attr("name", "contactNumbers[" + index + "].number");

		});

	});

	$(".delete_number").on("click", function () {

		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}

		// update serial numbers
		$("#contactNumber span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "contactNumbers" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "contactNumbers[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.type']" ).attr("id", "contactNumbers" + index + ".type");
			$(element).closest("tr").find( "select[name*='.type']" ).attr("name", "contactNumbers[" + index + "].type");

			$(element).closest("tr").find( "input[name*='.number']" ).attr("id", "contactNumbers" + index + ".number");
			$(element).closest("tr").find( "input[name*='.number']" ).attr("name", "contactNumbers[" + index + "].number");


		});	
	});


	$(".add_address").on("click", function () {

		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_single").select2("destroy");

		var newRow = $(this).closest("tr").clone(true);
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			//	$(element).find(".itemCategory:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			$(element).find(".locationType:first").val("");
		});

		newRow.insertAfter($(this).closest("tr"));
		$("#address .select2_single").select2(
				{
					dropdownAutoWidth : true, width: '100%'
				});
		// update serial numbers
		$("#address span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "addresses" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "addresses[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.locationType']" ).attr("id", "addresses" + index + ".locationType");
			$(element).closest("tr").find( "select[name*='.locationType']" ).attr("name", "addresses[" + index + "].locationType");

			$(element).closest("tr").find( "input[name*='.locationName']" ).attr("id", "addresses" + index + ".locationName");
			$(element).closest("tr").find( "input[name*='.locationName']" ).attr("name", "addresses[" + index + "].locationName");

		});

	});

	$(".delete_address").on("click", function () {

		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}

		// update serial numbers
		$("#address span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "addresses" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "addresses[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.locationType']" ).attr("id", "addresses" + index + ".locationType");
			$(element).closest("tr").find( "select[name*='.locationType']" ).attr("name", "addresses[" + index + "].locationType");

			$(element).closest("tr").find( "input[name*='.locationName']" ).attr("id", "addresses" + index + ".locationName");
			$(element).closest("tr").find( "input[name*='.locationName']" ).attr("name", "addresses[" + index + "].locationName");

		});	
	});

	$(".add_media").on("click", function () {

		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_single").select2("destroy");

		var newRow = $(this).closest("tr").clone(true);
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			//	$(element).find(".itemCategory:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			$(element).find(".locationType:first").val("");
		});

		newRow.insertAfter($(this).closest("tr"));
		$("#socailMedia .select2_single").select2(
				{
					dropdownAutoWidth : true, width: '100%'
				});
		// update serial numbers
		$("#socailMedia span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "socialMediaAccounts" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "socialMediaAccounts[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.accountType']" ).attr("id", "socialMediaAccounts" + index + ".accountType");
			$(element).closest("tr").find( "select[name*='.accountType']" ).attr("name", "socialMediaAccounts[" + index + "].accountType");

			$(element).closest("tr").find( "input[name*='.accountAddress']" ).attr("id", "socialMediaAccounts" + index + ".accountAddress");
			$(element).closest("tr").find( "input[name*='.accountAddress']" ).attr("name", "socialMediaAccounts[" + index + "].accountAddress");

		});

	});

	$(".delete_media").on("click", function () {

		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}

		// update serial numbers
		$("#socailMedia span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "socialMediaAccounts" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "socialMediaAccounts[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.accountType']" ).attr("id", "socialMediaAccounts" + index + ".accountType");
			$(element).closest("tr").find( "select[name*='.accountType']" ).attr("name", "socialMediaAccounts[" + index + "].accountType");

			$(element).closest("tr").find( "input[name*='.accountAddress']" ).attr("id", "socialMediaAccounts" + index + ".accountAddress");
			$(element).closest("tr").find( "input[name*='.accountAddress']" ).attr("name", "socialMediaAccounts[" + index + "].accountAddress");

		});	
	});


	$(".add_linked_contanct").on("click", function () {

		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_singleLimt").select2("destroy");

		var newRow = $(this).closest("tr").clone(true);
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			//	$(element).find(".itemCategory:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			$(element).find(".linkedcity:first").val("");
			$(element).find(".linkedcontanct:first").val("");
		});

		newRow.insertAfter($(this).closest("tr"));

		$("#linkedContant .select2_singleLimt").select2(
				{
					dropdownAutoWidth : true,
					width: 'element'
				});
		// update serial numbers
		$("#linkedContant span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "linkedcontancts" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "linkedcontancts[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.city']" ).attr("id", "linkedcontancts" + index + ".city");
			$(element).closest("tr").find( "select[name*='.city']" ).attr("name", "linkedcontancts[" + index + "].city");

			$(element).closest("tr").find( "select[name*='.linkedContact']" ).attr("id", "linkedcontancts" + index + ".linkedContact");
			$(element).closest("tr").find( "select[name*='.linkedContact']" ).attr("name", "linkedcontancts[" + index + "].linkedContact");

		});

	});

	$(".delete_linked_contanct").on("click", function () {

		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}

		// update serial numbers
		$("#linkedContant span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "linkedcontancts" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "linkedcontancts[" + index + "].id");

			$(element).closest("tr").find( "select[name*='.city']" ).attr("id", "linkedcontancts" + index + ".city");
			$(element).closest("tr").find( "select[name*='.city']" ).attr("name", "linkedcontancts[" + index + "].city");

			$(element).closest("tr").find( "input[name*='.linkedcontanct']" ).attr("id", "linkedcontancts" + index + ".linkedcontanct");
			$(element).closest("tr").find( "input[name*='.linkedcontanct']" ).attr("name", "linkedcontancts[" + index + "].linkedcontanct");

		});	
	});


	$(".add_img").on("click", function () {

		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_single").select2("destroy");

		var newRow = $(this).closest("tr").clone(true);
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			//	$(element).find(".itemCategory:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			$(element).find(".pictureUrl").val("");
			$(element).find(".ImageIcon").attr('src','');
			$(element).find(".picDefault").prop("checked", false);
		});

		newRow.insertAfter($(this).closest("tr"));
		$("#imgContact .select2_single").select2(
				{
					dropdownAutoWidth : true, width: '100%'
				});
		// update serial numbers
		$("#imgContact span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "pictures" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "pictures[" + index + "].id");

			$(element).closest("tr").find( "input[name*='.pictureUrl']" ).attr("id", "pictures" + index + ".pictureUrl");
			$(element).closest("tr").find( "input[name*='.pictureUrl']" ).attr("name", "pictures[" + index + "].pictureUrl");

			$(element).closest("tr").find( "input[name*='.picDefault']" ).attr("id", "pictures" + index + ".picDefault");
			$(element).closest("tr").find( "input[name*='.picDefault']" ).attr("name", "pictures[" + index + "].picDefault");

		});

	});

	$(".delete_img").on("click", function () {

		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}

		// update serial numbers
		$("#imgContact span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "pictures" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "pictures[" + index + "].id");

			$(element).closest("tr").find( "input[name*='.pictureUrl']" ).attr("id", "pictures" + index + ".pictureUrl");
			$(element).closest("tr").find( "input[name*='.pictureUrl']" ).attr("name", "pictures[" + index + "].pictureUrl");

			$(element).closest("tr").find( "input[name*='.picDefault']" ).attr("id", "pictures" + index + ".picDefault");
			$(element).closest("tr").find( "input[name*='.picDefault']" ).attr("name", "pictures[" + index + "].picDefault");

		});	
	});


	// ADD BUSINESS TYPE START
	$(".add_business").on("click", function () {

		// un-instrument select2 dropdowns
		$(this).closest("tr").find(".select2_singleLimt").select2("destroy");

		var newRow = $(this).closest("tr").clone(true);
		//when you use clone, even the text is carried over. To remove it, do the following:
		newRow.children("td").children("input, span").each(function(index, element){
			$(element).val("");
			$(element).text("");
		});
		newRow.children("td").each(function(index, element){
			//	console.log("a  "+$(this).children().text());
			if ($(this).children().length < 1 ) {
				$(element).text("");
			}//.prop("selectedIndex", 1);
			//	$(element).find(".itemCategory:first").val("");
			//(element).find(".itemCategory:first").prop("selectedIndex",4);
			$(element).find(".businessTypebsName:first").val("");
			//$(element).find(".linkedcontanct:first").val("");
		});

		newRow.insertAfter($(this).closest("tr"));

		$("#businessTypeList .select2_singleLimt").select2(
				{
					dropdownAutoWidth : true,
					width: 'element'
				});
		// update serial numbers
		$("#businessTypeList span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "businessTypeList" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "businessTypeList[" + index + "].id");


			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("id", "businessTypeList" + index + ".itemCategory");
			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("name", "businessTypeList[" + index + "].itemCategory");

			$(element).closest("tr").find( "input[name*='.ton']" ).attr("id", "businessTypeList" + index + ".ton");
			$(element).closest("tr").find( "input[name*='.ton']" ).attr("name", "businessTypeList[" + index + "].ton");

			$(element).closest("tr").find( "input[name*='.amt']" ).attr("id", "businessTypeList" + index + ".amount");
			$(element).closest("tr").find( "input[name*='.amt']" ).attr("name", "businessTypeList[" + index + "].amount");

		});

	});

	$(".delete_business").on("click", function () {

		if($(this).parents("tbody").find("tr").length > 1)
		{
			$(this).closest("tr").remove();
		}

		// update serial numbers
		$("#businessTypeList span#serial_no").each(function(index, element){
			$(element).text(index + 1);

			$(element).closest("tr").find( "input[name*='.id']" ).attr("id", "businessTypeList" + index + ".id");
			$(element).closest("tr").find( "input[name*='.id']" ).attr("name", "businessTypeList[" + index + "].id");


			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("id", "businessTypeList" + index + ".itemCategory");
			$(element).closest("tr").find( "select[name*='.itemCategory']" ).attr("name", "businessTypeList[" + index + "].itemCategory");

			$(element).closest("tr").find( "input[name*='.ton']" ).attr("id", "businessTypeList" + index + ".ton");
			$(element).closest("tr").find( "input[name*='.ton']" ).attr("name", "businessTypeList[" + index + "].ton");

			$(element).closest("tr").find( "input[name*='.amt']" ).attr("id", "businessTypeList" + index + ".amount");
			$(element).closest("tr").find( "input[name*='.amt']" ).attr("name", "businessTypeList[" + index + "].amount");

		});	
	});
	// ADD BUSINESS TYPE END

	function loadItemCategoryDefs(thisControl) {

		if (!$(thisControl).val() || $(thisControl).val() == 0){
			return;
		}

		$.get( "/payables/category_item_defs?categoryId=" + $(thisControl).val(), function( data ) {
			$(thisControl).closest("tr").find(".itemDef").empty();
			$(thisControl).closest("tr").find(".itemDef").append("<option>&emsp;</option>");

			for (var i = 0, len = data.length; i < len; i++) {
				var option = "<option value = " + data[i].id + ">" + data[i].formattedCode + "&emsp;" + data[i].name +  "</option>";
				$(thisControl).closest("tr").find(".itemDef").append(option);
			}
		});
	}


	function loadItemDefDetail(thisControl) {

		$.get( "/payables/item_def_detail?itemDefId=" + $(thisControl).val()+"&companyId=" + $("#company").val()+"&branchId=" + $("#itemStock\\.branch").val()+"&voucherStatusId=" + $("#voucherStatus").val(), function( data ) {

		});
	}


	$("#submitForm").off().on("click", function (event) {

		$("#submitForm").addClass('disabled');
		setTimeout(function(){
			$("#submitForm").removeClass('disabled');
		}, 10000);

		// Prevent the form from submitting via the browser.
		event.preventDefault();
		event.stopImmediatePropagation();
		// Will immediately show the confirmation popup
		$.confirm({
			title: "CONFIRMATION REQUIRED",
			content: "ARE YOU SURE YOU WANT TO SUBMIT THIS VOUCHER",
			buttons: {
				confirm: function () {
					// SUBMIT FORM
					buttionName =  $("#submitForm").text();
					userLog();
					$("#voucherForm").submit();
				},
				cancel: function () {
				},
			}
		});
	});

	// SUBMIT FORM
	$("#voucherForm").submit(function() {

		var itemEmpty = false;
		var itemAmountZero = false;
		var itemDuplicateExists = false;
		var itemDuplicate = "";
		var itemArray = [];
		$("#contactNumber tbody tr").each(function() {
			if (!$(this).find(".itemCategory").val() || $(this).find(".itemCategory").val() == 0 || !$(this).find(".itemDef").val() || $(this).find(".itemDef").val() == 0){
				itemEmpty = true;
			}

			if (parseFloat($(this).find(".amount").text()) == 0 ){
				itemAmountZero = true;
			}

			if ($(this).find(".itemDef").val() && $.inArray($(this).find(".itemDef").val(), itemArray) >= 0){
				itemDuplicateExists = true;
				itemDuplicate = $(this).find(".itemDef option:selected").text();
			}
			else{
				itemArray.push($(this).find(".itemDef").val());
			}
		});



		// prevent double submit
		if($("#voucherForm").data("submitted")){
			return false;
		}
		else{
			$("#voucherForm").data("submitted", true);
		}
	});

	$('input[type=number]').on('wheel', function(e){
		return false;
	});


})


function asifzoomIn(){
	//alert("ssss");
	//var	zoorDiv =document.querySelector("#zoorDiv");	

	var zoomer =  document.querySelector("#zoomer");	
	zoomer.style.width = (zoomer.width+10)+"px";
	if(screen.height-120>zoomer.height)
		zoomer.style.height = (zoomer.height+10)+"px";

}

function zoomOut(e){

	var curentWidth =  document.querySelector("#zoomer");	
	curentWidth.style.width = (curentWidth.offsetWidth -10)+"px";
	if(screen.height-300<zoomer.height)
		zoomer.style.height = (zoomer.height-10)+"px";

}
/*function displayCMenu(obj, sn){

	$(".mymenu").hide();
	$( "#menu"+sn).show();

}*/

/*function zooMer(src){

	$("#zoomer").attr('src',src);

} */
var degree =90;    
$("#rotate").on('click',function(){

	var zoomer =  document.querySelector("#zoomer");	

	zoomer.style.transform = 'rotate('+degree+'deg)';
	degree +=90; 
}); 

function deletePic(obj){
	if(confirm("Are you sure you want to remove this image? :"+obj)){
		var pNode = obj.parentNode;

		pNode.removeChild(obj); 
		UpdateDocPictureEntry();
	}
}

function UpdateDocPictureEntry()
{

	//var picDiv=document.querySelector('#_pics');
	var picDiv=document.querySelector('#contact_images');

	for (var i = 0, len = picDiv.querySelectorAll(".floatleft").length; i < len; i++)
	{

		picDiv.querySelectorAll(".ImageIcon")[i].setAttribute('oncontextmenu',"displayCMenu(this,"+i+");return false;");
		picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("id","docPictrueList"+i+".pictureUrl");
		picDiv.querySelectorAll(".floatleft .pictureUrl")[i].setAttribute("name","docPictrueList["+i+"].pictureUrl");
		//picDiv.querySelectorAll(".floatleft .pictureId")[i].setAttribute("id","docPictrueList"+i+".id");
		//picDiv.querySelectorAll(".floatleft .pictureId")[i].setAttribute("name","docPictrueList["+i+"].id");

		picDiv.querySelectorAll(".floatleft .mymenu")[i].setAttribute("id","menu"+i);

		$( "#menu"+i).menu( { position: { my: "right buttom", at: "right-5 top+10" } });

		$("#menu"+i).hide(); 
	}
}// end of UpdateDocPictureEntry()
//$('input[type="radio"]').prop('checked', false);
  $('body tr').on('click','.picDefault', function(){
	   $('input[type="radio"').prop('checked',false);
	   $('input[type="radio"').val(0);
	   $(this).prop('checked',true);
	   $(this).val(1);
	  /* var row = $(this).parent('td');
	   row.remove();*/
	});

$('body').on('change','.file',function(){
	//console.log("i m here in file change");
	var formData = new FormData();
	/*jQuery.each(jQuery($(".file"))[0].files, function(i, file) {
			    formData.append('file-'+i, file);
			});*/
	var row =  $("#contact_images TBODY TR").closest("td");
	row.find(".ImageIcon").attr('src','');
	var tds = $("#contact_images").children('td').length;
	//tds++;
	console.log("tds:"+tds);
	//$(this).closest("tr").find(".file").src = $(this).closest("tr").find(".file")[0].files;
	//formData.append('file', $(this).closest("tr").find(".file")[0].files[0]);
	//formData.append('file', $(this).find(".file")[0].files[0]);
	jQuery.each(jQuery($(".file"))[0].files, function(i, file) {
		formData.append('file-'+i, file);
	});
	//$(this).closest("tr").find(".ImageIcon").attr('src','');
	$.ajax({
		type : "POST",
		enctype: "multipart/form-data",
		contentType : "application/json",
		url: "/contact/fileUpload",
		data: formData,
		processData: false, //prevent jQuery from automatically transforming the data into a query string
		contentType: false,
		cache: false,
		success: function(data){
			$.each(data.SucessfulList, function (l, imgName) {
				//console.log("i m in file proces clicking   /img/'"+ imgName);
				//var myDIV = '<td><img th:src="/img/' + imgName +'" onclick="zooMer(this.src);" style="cursor: pointer; width: 50px; height: 50px;" class="ImageIcon" /><br/>';
				//	myDIV += '<input type="radio" class="picDefault" name="picturedefault"  /></td>';
				//console.log(myDIV);
				var myDIV ='<td style="display: block; width: 50px; min-width: 50px; max-width: 50px;" class="text-center floatleft">';
				myDIV +='<input type ="hidden" class = "id" name = "pictures[' + tds + '].id" id= "pictures' + tds + '.id" th:field= ${contact.pictures[' + tds + '].id}>';
				myDIV +='<span id="serial_no" th:text="' + tds + '" style="display: none;"></span>';
				myDIV +='<input type ="hidden" class = "pictureUrl" name = "pictures[' + tds + '].pictureUrl" id= "pictures' + tds + '.pictureUrl" value="'+ imgName + '" />';
				myDIV +='<img src="/img/'+ imgName + '" onclick="zooMer(this.src);" style="display: block; cursor: pointer; width: 50px; height: 50px;" class="ImageIcon" oncontextmenu="displayCMenu(this,' + (tds+1) + ');return false;" />';
				myDIV +='<input type ="radio" class = "picDefault" name = "pictures[' + tds + '].picDefault" id= "pictures' + tds + '.picDefault" value="'+ 1 + '" />';
				myDIV +='<div style="position:relative; display: none;">';
				myDIV +='<ul id="menu'+tds+'" class="mymenu">';
				myDIV +='<li><a href="javascript:"onclick="deletePic(this.parentNode.parentNode.parentNode.parentNode);">Delete</a></li>';
				myDIV +='<li><a href="javascript:"onclick="$(".mymenu").hide();">Cancel</a></li>';
				myDIV +='</ul>';
				myDIV +='</div>';
				myDIV +='</td>';
				
				console.log("Mtd " + myDIV);
				
				$("#contact_images").append(myDIV);
                  
				//console.log("UPloaded image is " + imgName);
				//row.find(".file").val("");
				//alert(imgName);
				//$(this).closest("tr").find(".ImageIcon").attr('src', '/img/'+imgName);
				//row.find(".ImageIcon").attr('src', '/img/'+imgName);

				//row.find(".pictureUrl").attr('value',imgName);
			});
			$('input[type="radio"]').prop('checked', false);
			$('input[type="radio"]').last().prop('checked', true);
		},
		complete:function(){
			//$("#upload_supporting_docs").modal("toggle");
		},
	});
});  
// end of $("#buttionId").on('click',function()

function checkEnterNumber(thisCTRL){
	var setmobileNumber = $(thisCTRL).val();
	console.log(setmobileNumber);
	$.get( "/bp/contactByMobileNumber?mobileNumber=" + setmobileNumber, function( data ) {
		$.confirm({
    	    title: "CONFIRMATION REQUIRED",
    	    content: "ARE YOU SURE YOU WANT TO EDIT THIS CONTACT ?",
    	    buttons: {
    	        confirm: function () {
    	        	//buttionName = $("#convert_to_sjv").text();
    	        	if(parseInt(data) > 0){
    	        		window.location = "" + location.protocol + "//" + location.host + "/bp/contact/" + data + "";
    	        	}
    	         },
    	        cancel: function () {
    	        },
    	    }
    	  });
	});
}