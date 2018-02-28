$(function() {
	$("#tuiyanid01").hide();
	$("#qq101").hide();
	$("#qq201").hide();
	$("#dialogScript01").hide();
	$("#setPlayer01").hide();
	
	var special = $('#special>div>div:first');
	$("div[class='panel-header accordion-header']").click(function(item) {
		special.css("background", "url('../image/special1.png')");
	});
	$("div[class='panel-header accordion-header accordion-header-selected']")
			.click(function(item) {
				special.css("background", "url('../image/special1.png')");
			});
	if (special) {
		special.css("background", "url('../image/special2.png')");

		special.toggle(function() {

			if ($("#special")[0].children[0].clientHeight > 100) {
				special.css("background", "url('../image/special1.png')");
			} else {
				special.css("background", "url('../image/special2.png')");
			}
		},

		function() {
			if ($("#special")[0].children[0].clientHeight > 100) {
				special.css("background", "url('../image/special1.png')");
			} else {
				special.css("background", "url('../image/special2.png')");
			}
		});

	}
	$("a[name='subMenu']").click(function(item) {
		$("a[name='subMenu']").each(function() {
			$(this).css("color", "#999999");
		});
		$(this).css("color", "#0066FF");
	});
	
	
});
/**
 * 根据手风琴页id和链接id。控制手风琴页的开合，以及手风琴内链接的高亮
 * 注：父界面调用的方法
 */
function mtpp(zi,zi01){
	var nihao = document.getElementById(zi);
	$('#special').accordion('select',nihao.name);  
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");}
}