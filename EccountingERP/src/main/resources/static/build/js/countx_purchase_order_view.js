function getFormData($form){
var unindexed_array = $form.serializeArray();
var indexed_array = {};

$.map(unindexed_array, function(n, i){
indexed_array[n['name']] = n['value'];
});

return indexed_array;
}

$( document ).ready(function() {
/* $(".ImageIcon").on("click", function(){
console.log("c :");
//    document.getElementById("zoomer").src = this.src;
//$( "#ImageNavigator").dialog('open');
$("#imageZooerModel #zoomer").src =$(".ImageIcon").src;
$("#imageZooerModel").modal("show");
});*/
function displayCMenu(obj, sn){

$(".mymenu").hide();
$( "#menu"+sn).show();

}
function zooMer(src){
console.log("zooMer :"+src);
//document.getElementById("zoomer").src = src;
//$("#imageZooerModel").modal("show");
}

//$("#imageZooerModel").modal("show");
})