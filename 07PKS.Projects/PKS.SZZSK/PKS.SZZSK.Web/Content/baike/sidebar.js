
var allEle = $(':header[class*="headline"]');
var headLen = allEle.length;
var ddArr = [];
//根据页面内容生成slide导航；
allEle.each(function(i){
	var sideIndex,
		sideName,
		ddId,
		highlight='',
		ddClass,
		sideAnchor,
		sideidxClass;
	sideName = $(this).find('.headline-content').text();
	if($(this).hasClass('headline-1')){
		sideAnchor = sideIndex = $(this).find('.anchor-1').eq(0).attr('name');
		ddClass = 'sideCatalog-item1';
		sideidxClass = 'sideCatalog-index1';
	}else{
		sideAnchor = $(this).find('.anchor-2').eq(0).attr('name');
		//console.log(sideAnchor);
		sideIndex = sideAnchor.replace('-','.');
		sideIndex = sideAnchor.replace('_','.');
		ddClass = 'sideCatalog-item2';
		sideidxClass = 'sideCatalog-index2';
	}
	ddId = 'sideToolbar-item-0-'+ sideAnchor;
	if(i==0){
		highlight = 'highlight';
	}
	var ddHtml = '<dd id="'+ ddId +'" class="'+ddClass + ' ' + highlight +'">'
		+       '<span class="'+ sideidxClass +'">'+ sideIndex +'</span>'
		+       '<a class="nslog:1026" onclick="return false;" title="part'+sideAnchor+'" href="#'+sideAnchor+'">'+ sideName +'</a>'
		+       '<span class="sideCatalog-dot"></span>'
		+    '</dd>';
	ddArr.push(ddHtml);
});
//for (var i = 0; i < baikeMulu.length; i++) {
//    var mulu = baikeMulu[i];
//    	var sideIndex,
//		sideName,
//		ddId,
//		highlight='',
//		ddClass,
//		sideAnchor,
//		sideidxClass;
//       // sideName = $(this).find('.headline-content').text();

//        sideIndex = mulu.code;
//        sideName = mulu.name;
//        if (mulu.levelnumber = 1) {
//            sideAnchor = sideName;
//            ddClass = 'sideCatalog-item1';
//            sideidxClass = 'sideCatalog-index1';
//        }
//        else {
//            sideAnchor = sideName;
//            ddClass = 'sideCatalog-item2';
//            sideidxClass = 'sideCatalog-index2';
//        }
	
//	ddId = 'sideToolbar-item-0-'+ sideAnchor;
//	if(i==0){
//		highlight = 'highlight';
//	}
//	var ddHtml = '<dd id="'+ ddId +'" class="'+ddClass + ' ' + highlight +'">'
//		+       '<span class="'+ sideidxClass +'">'+ sideIndex +'</span>'
//		+       '<a class="nslog:1026" onclick="return false;" title="part'+sideAnchor+'" href="#'+sideAnchor+'">'+ sideName +'</a>'
//		+       '<span class="sideCatalog-dot"></span>'
//		+    '</dd>';
//	ddArr.push(ddHtml);
//}
$('#sideCatalog-catalog dl').html(ddArr.join(''));

//设置导航的位置
var slideTop = $(window).height() - $('.slide').height();
$('.slide').css('top',slideTop);

var slideInnerHeight = $('#sideCatalog-catalog dl').height();
var slideOutHeight = $('#sideCatalog-catalog').height();
var enableTop = slideInnerHeight - slideOutHeight;
var step = 50;
//点击向上的按钮
$(document).on("click", "#sideCatalog-down", function () {
    if ($(this).hasClass('sideCatalog-down-enable')) {
        if ((enableTop - Math.abs(parseInt($('#sideCatalog-catalog dl').css('top')))) > step) {
            $('#sideCatalog-catalog dl').stop().animate({ 'top': '-=' + step }, 'fast');
            $('#sideCatalog-up').removeClass('sideCatalog-up-disable').addClass('sideCatalog-up-enable');
        } else {
            $('#sideCatalog-catalog dl').stop().animate({ 'top': -enableTop }, 'fast');
            $(this).removeClass('sideCatalog-down-enable').addClass('sideCatalog-down-disable');
        }
    } else {
        return false;
    }
});
//$('#sideCatalog-down').on('click', function () {
//	if ($(this).hasClass('sideCatalog-down-enable')) {
//		if ((enableTop - Math.abs(parseInt($('#sideCatalog-catalog dl').css('top')))) > step) {
//		$('#sideCatalog-catalog dl').stop().animate({'top': '-=' + step}, 'fast');
//		$('#sideCatalog-up').removeClass('sideCatalog-up-disable').addClass('sideCatalog-up-enable');
//		} else {
//		$('#sideCatalog-catalog dl').stop().animate({'top': -enableTop}, 'fast');
//		$(this).removeClass('sideCatalog-down-enable').addClass('sideCatalog-down-disable');
//		}
//	} else {
//		return false;
//	}
//})
//点击向下的按钮
$(document).on("click", "#sideCatalog-up", function () {
    if ($(this).hasClass('sideCatalog-up-enable')) {
        if (Math.abs(parseInt($('#sideCatalog-catalog dl').css('top'))) > step) {
            $('#sideCatalog-catalog dl').stop().animate({ 'top': '+=' + step }, 'fast');
            $('#sideCatalog-down').removeClass('sideCatalog-down-disable').addClass('sideCatalog-down-enable');
        } else {
            $('#sideCatalog-catalog dl').stop().animate({ 'top': '0' }, 'fast');
            $(this).removeClass('sideCatalog-up-enable').addClass('sideCatalog-up-disable');
        }
    } else {
        return false;
    }
})
//$('#sideCatalog-up').on('click', function () {
//	if ($(this).hasClass('sideCatalog-up-enable')) {
//		if (Math.abs(parseInt($('#sideCatalog-catalog dl').css('top'))) > step) {
//		$('#sideCatalog-catalog dl').stop().animate({'top': '+=' + step}, 'fast');
//		$('#sideCatalog-down').removeClass('sideCatalog-down-disable').addClass('sideCatalog-down-enable');
//		} else {
//		$('#sideCatalog-catalog dl').stop().animate({'top': '0'}, 'fast');
//		$(this).removeClass('sideCatalog-up-enable').addClass('sideCatalog-up-disable');
//		}
//	} else {
//		return false;
//	}
//})

//点击导航中的各个目录
//$('#sideCatalog-catalog dl').delegate('dd', 'click', function () {
//	var index = $('#sideCatalog-catalog dl dd').index($(this));
//	scrollSlide($(this), index);
//	var ddIndex = $(this).find('a').stop().attr('href').lastIndexOf('#');
//	var ddId = $(this).find('a').stop().attr('href').substring(ddIndex+1);
//	var windowTop = $('a[name="' + ddId + '"]').offset().top;
//	$('body,html').animate({scrollTop: windowTop}, 'fast');
//});
$(document).on("click", "#sideCatalog-catalog dl dd", function () {
    var index = $('#sideCatalog-catalog dl dd').index($(this));
    scrollSlide($(this), index);
    var ddIndex = $(this).find('a').stop().attr('href').lastIndexOf('#');
    var ddId = $(this).find('a').stop().attr('href').substring(ddIndex + 1);
    var windowTop = $('a[name="' + ddId + '"]').offset().top;
    $('body,html').animate({ scrollTop: windowTop }, 'fast');
});
//滚动页面，即滚动条滚动
var fixscrollTop = 50;
var fixscroll = 0;
$(window).scroll(function () {
	if($(this).scrollTop()>$(this).height()+fixscrollTop){
		$('.slide').show();
	}
	else{
		$('.slide').hide();
	}
	for (var i=headLen-1; i>=0; i--) {
		if ($(this).scrollTop() >=allEle.eq(i).offset().top - allEle.eq(i).height() - fixscroll) {
			var index = i;
			$('#sideCatalog-catalog dl dd').eq(index).addClass('highlight').siblings('dd').removeClass('highlight');
			scrollSlide($('#sideCatalog-catalog dl dd').eq(index), index);
			return false;
		} else {
			$('#sideCatalog-catalog dl dd').eq(0).addClass('highlight').siblings('dd').removeClass('highlight');
		}
	}
});

//导航的滚动，以及向上，向下按钮的显示隐藏
function scrollSlide(that, index){
	if (index < 10) {
		$('#sideCatalog-catalog dl').stop().animate({'top': '0'}, 'fast');
		$('#sideCatalog-down').removeClass('sideCatalog-down-disable').addClass('sideCatalog-down-enable');
		$('#sideCatalog-up').removeClass('sideCatalog-up-enable').addClass('sideCatalog-up-disable');
	}
	else if (index > 16) {
		$('#sideCatalog-catalog dl').stop().animate({'top': -enableTop}, 'fast');
		$('#sideCatalog-down').removeClass('sideCatalog-down-enable').addClass('sideCatalog-down-disable');
		$('#sideCatalog-up').removeClass('sideCatalog-up-disable').addClass('sideCatalog-up-enable');
	}
	else {
		var dlTop = parseInt($('#sideCatalog-catalog dl').css('top')) + slideOutHeight / 2 - (that.offset().top - $('#sideCatalog-catalog').offset().top);
		$('#sideCatalog-catalog dl').stop().animate({'top': dlTop}, 'fast');
		$('#sideCatalog-down').removeClass('sideCatalog-down-disable').addClass('sideCatalog-down-enable');
		$('#sideCatalog-up').removeClass('sideCatalog-up-disable').addClass('sideCatalog-up-enable');
	}
	
	if (index==headLen-1) {
		$('.slide').hide();
	}
}

//置顶
//$('#sideToolbar-up').bind('click', function(){
//$('body,html').animate({scrollTop: 0}, 'fast');
//})
//$('#sideToolbar-up').bind('click', function () {
//    $('body,html').animate({ scrollTop: 0 }, 'fast');
//})
$(document).on("click", "#sideToolbar-up", function () {
    $('body,html').animate({ scrollTop: 0 }, 'fast');
});
//slide内容的显示与隐藏
//$('#sideCatalogBtn').bind('click', function(){
//if($(this).hasClass('sideCatalogBtnDisable')){
//    $(this).removeClass('sideCatalogBtnDisable');
//    $('#sideCatalog').css('visibility','visible');
//}else{
//    $(this).addClass('sideCatalogBtnDisable');
//    $('#sideCatalog').css('visibility','hidden');
//}
//})

$(document).on("click", "#sideCatalogBtn", function () {
    if ($(this).hasClass('sideCatalogBtnDisable')) {
        $(this).removeClass('sideCatalogBtnDisable');
        $('#sideCatalog').css('visibility', 'visible');
    } else {
        $(this).addClass('sideCatalogBtnDisable');
        $('#sideCatalog').css('visibility', 'hidden');
    }
});