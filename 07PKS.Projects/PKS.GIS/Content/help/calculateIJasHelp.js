$(function() {
$("#xy01").hide();
$("#idxyz01").hide();
$("#idxyz101").hide();
$("#xy101").hide();
$("#xy201").hide();
$("#xy301").hide();
	var calculate = $('#calculate>div>div:first');
	$("div[class='panel-header accordion-header']").click(function(item){
			calculate.css("background",
				"url('../image/calculate1.png')");
		});
		$("div[class='panel-header accordion-header accordion-header-selected']").click(function(item){
			calculate.css("background",
				"url('../image/calculate1.png')");
		});
		if (calculate) {
			calculate.css("background",
					"url('../image/calculate2.png')");

			calculate.toggle(function() {
				
					if($("#calculate")[0].children[0].clientHeight > 100){
						calculate.css("background",
							"url('../image/calculate1.png')");
					}
					else{
						calculate.css("background",
							"url('../image/calculate2.png')");
					} 
				}, 
				
			 	function() {
					if($("#calculate")[0].children[0].clientHeight > 100){
						calculate.css("background",
							"url('../image/calculate1.png')");
					}
					else{
						calculate.css("background",
							"url('../image/calculate2.png')");
					} 
				});
			
		}
			$("a[name='subMenu']").click(function(item){
				$("a[name='subMenu']").each(function(){
					$(this).css("color","#999999");
				});
			$(this).css("color","#0066FF");
		});
});
/**
 * 根据手风琴页id和链接id。控制手风琴页的开合，以及手风琴内链接的高亮
 * 注：父界面调用的方法
 */
function mtpp(zi,zi01){
	var nihao = document.getElementById(zi);
	$('#calculate').accordion('select',nihao.name);  
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");}
}