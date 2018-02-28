$(function() {
$("#xy401").hide();
$("#hh201").hide();
$("#hh401").hide();
$("#musicid01").hide();
$("#hh301").hide();
	var zuobiao = $('#aid>div>div:first');
	var music = $('#aid>div>div:eq(2)');
	var picture = $('#aid>div>div:eq(4)');
	var file = $('#aid>div>div:eq(6)');
	var caiji = $('#aid>div>div:last').prev('div');
	$("div[class='panel-header accordion-header']").click(function(item){
			zuobiao.css("background",
				"url('../image/zuobiao1.png')");
			music.css("background",
				"url('../image/music1.png')");
			picture.css("background",
				"url('../image/picture1.png')");
			file.css("background",
				"url('../image/file1.png')");
			caiji.css("background",
				"url('../image/caiji1.png')");
		});
		$("div[class='panel-header accordion-header accordion-header-selected']").click(function(item){
			zuobiao.css("background",
				"url('../image/zuobiao1.png')");
			music.css("background",
				"url('../image/music1.png')");
			picture.css("background",
				"url('../image/picture1.png')");
			file.css("background",
				"url('../image/file1.png')");
			caiji.css("background",
				"url('../image/caiji1.png')");
		});
	if (zuobiao) {
			zuobiao.css("background",
					"url('../image/zuobiao2.png')");

			zuobiao.toggle(function() {
				
					if($("#aid")[0].children[0].clientHeight > 100){
						zuobiao.css("background",
							"url('../image/zuobiao1.png')");
					}
					else{
						zuobiao.css("background",
							"url('../image/zuobiao2.png')");
					} 
				}, 
				
			 	function() {
					if($("#aid")[0].children[0].clientHeight > 100){
						zuobiao.css("background",
							"url('../image/zuobiao1.png')");
					}
					else{
						zuobiao.css("background",
							"url('../image/zuobiao2.png')");
					} 
				});
			
		}
		if (music) {
			music.css("background",
					"url('../image/music1.png')");

			music.toggle(function() {
					if($("#aid")[0].children[1].clientHeight > 100){
						music.css("background",
							"url('../image/music1.png')");
					}
					else{
						music.css("background",
							"url('../image/music2.png')");
					} 
				}, 
				
			 	function() {
					if($("#aid")[0].children[1].clientHeight > 100){
						music.css("background",
							"url('../image/music1.png')");
					}
					else{
						music.css("background",
							"url('../image/music2.png')");
					} 
				});
			
		}
		if (picture) {
			picture.css("background",
					"url('../image/picture1.png')");

			picture.toggle(function() {
					if($("#aid")[0].children[2].clientHeight > 100){
						picture.css("background",
							"url('../image/picture1.png')");
					}
					else{
						picture.css("background",
							"url('../image/picture2.png')");
					} 
				}, 
				
			 	function() {
					if($("#aid")[0].children[2].clientHeight > 100){
						picture.css("background",
							"url('../image/picture1.png')");
					}
					else{
						picture.css("background",
							"url('../image/picture2.png')");
					} 
				});
			
		}
		if (file) {
			file.css("background",
					"url('../image/file1.png')");

			file.toggle(function() {
					if($("#aid")[0].children[3].clientHeight > 100){
						file.css("background",
							"url('../image/file1.png')");
					}
					else{
						file.css("background",
							"url('../image/file2.png')");
					} 
				}, 
				
			 	function() {
					if($("#aid")[0].children[3].clientHeight > 100){
						file.css("background",
							"url('../image/file1.png')");
					}
					else{
						file.css("background",
							"url('../image/file2.png')");
					} 
				});
			
		}
		if (caiji) {
			caiji.css("background",
					"url('../image/caiji1.png')");

			caiji.toggle(function() {
					if($("#aid")[0].children[4].clientHeight > 100){
						caiji.css("background",
							"url('../image/caiji1.png')");
					}
					else{
						caiji.css("background",
							"url('../image/caiji2.png')");
					} 
				}, 
				
			 	function() {
					if($("#aid")[0].children[4].clientHeight > 100){
						caiji.css("background",
							"url('../image/caiji1.png')");
					}
					else{
						caiji.css("background",
							"url('../image/caiji2.png')");
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
	$('#aid').accordion('select',nihao.name); 
	if(zi01!=0){
	var nihao01 = document.getElementById(zi01);
	$("a[name='subMenu']").each(function(){
		$(this).css("color","#999999");
	});
	$('#'+zi01).css("color","#0066FF");}
}