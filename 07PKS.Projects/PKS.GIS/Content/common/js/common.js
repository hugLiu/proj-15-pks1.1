/**
 * 文件描述: 系统中公有的js方法。
 * 
 * @author zhanggf
 * @version 1.0 
 * 创建时间： 2012-08-30 上午17:46:07 
 * ********************************更新记录****************************** 
 * 版本： 1.0 修改日期： 2012-12-01 修改人： zhanggf
 * 修改内容： 
 * *********************************************************************
 */
// 系统根路径
var rootPath = getRootPath();
/**
 * 页面加载完毕监听，处理某些问题
 */
$(window).bind("load", function() {
	// 解决easyui-combo对象与select对象Backspace导致页面回退问题
	comboBackspaceDisabled();

	setSelectObjWidth();

	// 给html元素添加获取焦点时的样式
	setTimeout('addFocusCssToHtmlElement()', 200);
	setTimeout('addFocusCssToHtmlElement()', 500);
});

$(document).ready(function() {
	
});

/**
 * 功能描述：获取系统根路径
 */
function getRootPath() {
	// 获取当前网址，如： http://localhost:8083/uimcardprj/share/meun.jsp
	var curWwwPath = window.document.location.href;
	// 获取主机地址之后的目录，如： uimcardprj/share/meun.jsp
	var pathName = window.document.location.pathname;
	var pos = curWwwPath.indexOf(pathName);
	// 获取主机地址，如： http://localhost:8083
	var localhostPaht = curWwwPath.substring(0, pos);
	// 获取带"/"的项目名，如：/uimcardprj
	var projectName = pathName.substring(0, pathName.substring(1).indexOf('/') + 1);
	return (localhostPaht + projectName + "/");
}

/**
 * 功能描述：解决easyui-combo对象与select对象Backspace导致页面回退问题
 */
function comboBackspaceDisabled() {
	jQuery.fn.ready(function() {
		// 解决easyui-combo对象Backspace导致页面回退问题
		$.each($(".combo-text"), function(i, item) {
			var combotextreadOnly = $(item).attr("readOnly");
			if (combotextreadOnly == "readonly") {
				$(item).bind('keydown', function() {
					if (8 == event.keyCode) {
						return false;
					}
				});
			}
		});
		// 解决普通select对象Backspace导致页面回退问题
		$.each($("select"), function(i, item) {
			$(item).bind('keydown', function() {
				if (8 == event.keyCode) {
					return false;
				}
			});
		});
	});
}

/**
 * 功能描述：圆角效果ie兼容性问题
 */
function radiusCompatible() {
	var browser = $.browser;
	var documentMode = checkDocumentMode();
	if (browser.msie && documentMode == '9') {
		// 9.0 使用1.CSS
	} else {
		var path = rootPath + "/jasframework/common/css/radius_htc.css";
		// 动态添加一个.js 文件
		loadjscssfile(path, "css");
	}
}

/**
 * 功能描述：加载输入框提示信息
 */
function loadBlankTextJs() {
	var path = rootPath + "/jasframework/common/thirdparty/jquery/jquery.inputDefault.js";
	$.getScript(path, function() {
		$('[blankText]').inputDefault();
	});
}

/**
 * 功能描述：给html元素添加获取焦点时的样式
 */
function addFocusCssToHtmlElement() {
	$("input").not(":checkbox").each(function() {
		$(this).bind('focus', function() {
			// alert('focus');
			if ($(this).hasClass('combo-text')) {
				$(this).parent().addClass('input_bg_focus');
			} else if ($(this).hasClass('Wdate')) {
				$(this).addClass('Wdate_focus');
			} else {
				$(this).addClass('input_bg_focus');
			}
		});
		$(this).bind('blur', function() {
			if ($(this).hasClass('combo-text')) {
				$(this).parent().removeClass('input_bg_focus');
			} else if ($(this).hasClass('Wdate')) {
				$(this).removeClass('Wdate_focus');
			} else {
				$(this).removeClass('input_bg_focus');
			}
		});
	});

	$(" textarea").each(function() {
		$(this).bind('focus', function() {
			// alert('focus');
			$(this).addClass('input_bg_focus');
		});
		$(this).bind('blur', function() {
			// alert('blur');
			$(this).removeClass('input_bg_focus');
		});
	});

	$(" select").each(function() {
		$(this).bind('focus', function() {
			// alert('focus');
			$(this).addClass('input_bg_focus');
		});
		$(this).bind('blur', function() {
			// alert('blur');
			$(this).removeClass('input_bg_focus');
		});
	});
}

/**
 * 功能描述：设置easyui Combo对象的宽度，解决宽度总小于其他input宽度问题
 * 
 * @param comboObjId easyui Combo组件对象id
 * @param width easyui Combo组件对象宽度，相对于页面的百分比宽度，用小数表示，例如0.35表示宽度为页面宽度的35%
 * @param comboObjType easyui Combo组件对象类型，（'combobox'||'combotree'||datetimebox）
 */
function setComboObjWidth(comboObjId, width, comboObjType, containerObjId) {
	setComboObjWidth_private(comboObjId, width, comboObjType, containerObjId);
	var _fn = function() {
		setComboObjWidth_private(comboObjId, width, comboObjType, containerObjId);
	};
	// $(window).bind('resize',_fn);//不能使用该方法
	onWindowResize.add(_fn);// 要使用该方法
}

/**
 * 功能描述：设置easyui Combo对象的宽度，解决宽度总小于其他input宽度问题， 仅供common.js内部调用，页面请使用setComboObjWidth(comboObjId,width,comboObjType)
 * 
 * @param comboObjId easyui Combo组件对象id
 * @param width easyui Combo组件对象宽度，相对于页面的百分比宽度，用小数表示，例如0.35表示宽度为页面宽度的35%
 * @param comboObjType easyui Combo组件对象类型，（'combobox'||'combotree'||datetimebox）
 * @param containerObjId easyui Combo组件所在容器id(div容器)
 */
function setComboObjWidth_private(comboObjId, width, comboObjType, containerObjId) {
	var containerObjWidth; // Combo对象所在容器的宽度(table)
	if (containerObjId && containerObjId != '') {
		containerObjWidth = $('#' + containerObjId).width();
	} else {
		containerObjWidth = document.documentElement.clientWidth;
	}
	var comboObjWidth = containerObjWidth * width - 3;
	if (comboObjId && comboObjId != '') {
		comboObj = $('#' + comboObjId);
		if ('combobox' == comboObjType) {
			comboObj.combobox('resize', comboObjWidth);
		} else if ('combotree' == comboObjType) {
			comboObj.combo('resize', comboObjWidth);
		} else if ('datetimebox' == comboObjType) {
			comboObj.datetimebox('resize', comboObjWidth);
		} else {
			comboObj.combo('resize', comboObjWidth);
		}
		comboObj.parent().css('padding-right', '3px');
	} else {
		$(".combo").each(function() {
			// $(this).parent().css('padding-right','3px');
		});
	}
}

/**
 * 功能描述：设置下拉选的宽度，解决下拉选与页面其他input元素不对其问题
 */
function setSelectObjWidth() {
	$("select").each(function() {
		$(this).parent().css('padding-right', '3px');
	});
}

/**
 * 功能描述：通过脚本动态添加css和js 文件到页面
 * 
 * @param filename 文件名称（路径）
 * @param filetype 文件类型（css\js）
 */
function loadjscssfile(filename, filetype) {
	// 如果文件类型为 .js ,则创建 script 标签，并设置相应属性
	if (filetype == "js") {
		var fileref = document.createElement('script');
		fileref.setAttribute("type", "text/javascript");
		fileref.setAttribute("src", filename);
	}
	// 如果文件类型为 .css ,则创建 script 标签，并设置相应属性
	else if (filetype == "css") {
		var fileref = document.createElement("link");
		fileref.setAttribute("rel", "stylesheet");
		fileref.setAttribute("type", "text/css");
		fileref.setAttribute("href", filename);
	}
	if (typeof fileref != "undefined")
		document.getElementsByTagName("head")[0].appendChild(fileref);
}

/**
 * 功能描述：设置页面main_area区域高度
 */
function setMain_areaHeight() {
	if (document.getElementById('main_area') != null) {
		$("#main_area").height(document.documentElement.clientHeight - 40);
	}
}
/**
 * 功能描述：清空表单
 * 
 * @author:zhanggf
 * @param formId 表单id
 * @param text true为清空文本框。false为不清空文本框
 * @param hidden true为清空隐藏域。false为不清空隐藏域
 * @param select true为清空下拉菜单。false为不清空下拉菜单
 * @param textarea true为清空大文本框。false为不清空大文本框
 */

function clearQueryForm(formId, text, hidden, select, textarea) {
	if (typeof $("#" + formId) == undefined) {
		return;
	}
	if (text) {
		$("#" + formId + " input:text").each(function() {
			var isClear = $(this).attr("isClear");
			if (typeof isClear == 'undefined' || isClear == true) {
				$(this).val("");
			}
		});
	}
	if (hidden) {
		$("#" + formId + " input:hidden").each(function() {
			$(this).val("");
		});
	}
	if (select) {
		$("#" + formId + " select").each(function() {
			var isClear = $(this).attr("isClear");
			if (typeof isClear == 'undefined' || isClear == true) {
				$(this).get(0).selectedIndex = 0;
			}
		});
	}
	if (textarea) {
		$("#" + formId + " textarea").each(function() {
			var isClear = $(this).attr("isClear");
			if (typeof isClear == 'undefined' || isClear == true) {
				$(this).html("");
			}
		});
	}
}

/**
 * 功能描述：为添加了required=true的地方添加span
 * 
 * @author user
 */
function createSpan() {
	$(":input").each(function() {
		var reSpan = "<span style='color:red;vertical-align: bottom;'>&nbsp</span>";
		if ($(this).attr("required")) {
			var requiredSpan = "<span style='color:red;vertical-align: bottom;'>*</span>";
			if ($(this).parent().prev().find("span:last").html() != '*')
				$(this).parent().prev().find("span:first").after(requiredSpan);
		} else {
			$(this).parent().prev().find("span:first").after(reSpan);
		}
	});
}

/** ************************列表查询页面datagrid高度宽度自适应接口--开始********************************* */
var datagridId = '';
var queryPanelId = '';
var queryPanelHeight = 0;
var containerId = '';

/**
 * 功能描述：初始化datagrid的高度，datagrid高度自适应处理
 * 
 * @param datagridObjId datagrid的id
 * @param queryPanelObjId 查群面板的id 如果没有查询面板 则改id赋值为''
 * @param queryPanelH 查询区域的高度，如果没有查询区域，则改值赋值为'0'
 * @param containerDivId table的父html标签id
 */
function initDatagrigHeight(datagridObjId, queryPanelObjId, queryPanelH, containerDivId) {
	datagridId = datagridObjId;
	queryPanelId = queryPanelObjId;
	queryPanelHeight = parseInt(queryPanelH);
	containerId = containerDivId;
	try {
		var containerHeight = $(window).height();
		var containerWidth = $(window).width();
		if (containerId && containerId != '') {
			containerHeight = $("#" + containerId).height();
			containerWidth = $("#" + containerId).width();
		}
		if (queryPanelId && queryPanelId != '') {
			$('#' + queryPanelId).panel({
				onOpen:changeDatagrigHeight,
				onExpand : function() {
					changeDatagrigHeight();
				},
				onCollapse : function() {
					changeDatagrigHeight();
				}
			});
		} else {
			$('#' + datagridId).datagrid('resize', {
				width : containerWidth,
				height : containerHeight
			});
		}
		if (containerId && containerId != '') {
			document.getElementById(containerId).onresize = changeDatagrigHeight;
		} else {
			document.body.onresize = changeDatagrigHeight;// 只能用js原生的方法，不能使用jquery的resize方法：$('body').bind('resize',function(){})
		}
	} catch (e) {
	}
}

/**
 * 功能描述：页面窗口大小改变等情况下的datagrid高度自适应处理函数，common.js内部调用函数。
 */
function changeDatagrigHeight() {
	try {
		var containerHeight = $(window).height();
		var containerWidth = $(window).width();
		if (containerId && containerId != '') {
			containerHeight = $("#" + containerId).height();
			containerWidth = $("#" + containerId).width();
		}
		var gridWidth = containerWidth;
		var gridHeight = containerHeight;
		if (queryPanelId && queryPanelId != '') {
			$('#' + queryPanelId).panel('resize', {
				width : containerWidth
			});
			gridHeight = containerHeight - $('#' + queryPanelId).panel('panel').height();
		}
		$('#' + datagridId).datagrid('resize', {
			width : gridWidth,
			height : gridHeight
		});
	} catch (e) {
		// alert(e);
	}
}

/** ************************列表查询页面datagrid高度宽度自适应接口--结束*********************************** */

/**
 * 方法描述：获取访问路径中某个参数
 * 
 * @param paramName 参数名
 * @param url 指定要截取参数的url（可以为空，如果为空url指向当前页面）
 */
function getParamter(paramName, url) {
	var seachUrl = window.location.search.replace("?", "");
	if (url != null) {
		var index = url.indexOf('?');
		url = url.substr(index + 1);
		seachUrl = url;
	}
	var ss = seachUrl.split("&");
	var paramNameStr = "";
	var paramNameIndex = -1;
	for ( var i = 0; i < ss.length; i++) {
		paramNameIndex = ss[i].indexOf("=");
		paramNameStr = ss[i].substring(0, paramNameIndex);
		if (paramNameStr == paramName) {
			var returnValue = ss[i].substring(paramNameIndex + 1, ss[i].length);
			if (typeof (returnValue) == "undefined") {
				returnValue = "";
			}
			return returnValue;
		}
	}
	return "";
}

/**
 * 功能描述： 解决 lte ie8 & chrome 及其他可能会出现的 原生 window.resize 事件多次执行的 BUG. <methods> add: 添加事件句柄 remove: 删除事件句柄 </methods>
 */
var onWindowResize = function() {
	// 事件队列
	var queue = [], indexOf = Array.prototype.indexOf || function() {
		var i = 0, length = this.length;
		for (; i < length; i++) {
			if (this[i] === arguments[0]) {
				return i;
			}
		}
		return -1;
	};

	var isResizing = {}, // 标记可视区域尺寸状态， 用于消除 lte ie8 / chrome 中
	// window.onresize 事件多次执行的 bug
	lazy = true, // 懒执行标记
	listener = function(e) { // 事件监听器
		var h = window.innerHeight || (document.documentElement && document.documentElement.clientHeight) || document.body.clientHeight, w = window.innerWidth
				|| (document.documentElement && document.documentElement.clientWidth) || document.body.clientWidth;

		if (h === isResizing.h && w === isResizing.w) {
			return;
		} else {
			e = e || window.event;
			var i = 0, len = queue.length;
			for (; i < len; i++) {
				queue[i].call(this, e);
			}
			isResizing.h = h, isResizing.w = w;
		}
	};
	return {
		add : function(fn) {
			if (typeof fn === 'function') {
				if (lazy) { // 懒执行
					if (window.addEventListener) {
						window.addEventListener('resize', listener, false);
					} else {
						window.attachEvent('onresize', listener);
					}
					lazy = false;
				}
				queue.push(fn);
			} else {
			}
			return this;
		},
		remove : function(fn) {
			if (typeof fn === 'undefined') {
				queue = [];
			} else if (typeof fn === 'function') {
				var i = indexOf.call(queue, fn);
				if (i > -1) {
					queue.splice(i, 1);
				}
			}
			return this;
		}
	};
}.call(this);

/**
 * 方法描述：重新加载数据
 * 
 * @param url 网格所在页面url
 * @param elementId 网格id
 */
function reloadData(url, elementId) {
	var fra = top.$("iframe");
	var browser = $.browser;
	for ( var i = 0; i < fra.length; i++) {
		if (fra[i].src.indexOf(url) != -1) {
			if (browser.msie && (document.documentMode == '7')) {// 如果浏览器为ie
				// 且文档模式为ie7，则重新载入页面（因为刷新datagrid会导致datagrid显示不全）
				fra[i].contentWindow.location.reload();
				break;
			} else {
				fra[i].contentWindow.$("#" + elementId).datagrid("reload");
				break;
			}
		}
	}
	try {
		parent.$(elementId).datagrid("reload");
	} catch (e) {

	}
}

/**
 * 方法描述：查看页面赋值方法
 * 
 * @param url 查看页面加载数据请求路径 如：action.do?id=id
 * @param formid 查看页面表格表单id
 */
function businessView(url, formid) {
	$.getJSON(url + '&r=' + new Date().getTime(), function(item) {
		$("#" + formid + " td > span").each(function() {
			var spanid = $(this).attr("id");
			if (spanid != "undefined" && spanid != "") {
				var property = spanid;
				if (item["" + property + ""] != "undefined" && item["" + property + ""] != 'null') {
					$("#" + spanid).html(item[property]);
				}
			}
		});
	});
}

/**
 * 方法描述：将按钮状态置为可用
 * 
 * @param buttonid 按钮id
 */
function enableButtion(buttonid) {
	$('#' + buttonid).linkbutton('enable');
}

/**
 * 方法描述：将按钮状态置为不可用
 * 
 * @param buttonid 按钮id
 */
function disableButtion(buttonid) {
	$('#' + buttonid).linkbutton('disable');
}

/**
 * 方法描述：判断浏览器类型及版本
 * 
 * @return 浏览器类型和版本号，中间空格隔开，各浏览器命名成请参考方法内部返回值
 */
function checkBrowser() {
	var browser = $.browser;
	if (browser.msie) {
		// alert("这是一个IE浏览器" + browser.version);
		return "IE " + browser.version;
		// alert(document.documentMode);
	} else if (browser.opera) {
		// alert("这是一个opera浏览器");
		return "OPERA " + browser.version;
	} else if (browser.mozilla) {
		// alert("这是一个火狐浏览器");
		return "MOZILLA " + browser.version;
	} else if (browser.safari) {
		// alert("这是一safari浏览器");
		return "SAFARI " + browser.version;
	} else if (browser.chrome) {
		// alert("这是一chrome浏览器"+ browser.version);
		return "CHROME " + browser.version;
	} else {
		alert("我不知道");
		return "UNKNOW";
	}
}

/**
 * 方法描述：判断文档模式，只对IE有效，其他浏览器不支持documentMode属性
 * 
 * @return 文档模式，5||6||7||8||9||10
 */
function checkDocumentMode() {
	return document.documentMode;
}

/** ----------------弹出窗口定义开始--------------------------* */

var biframe;// 窗口内容的iframe
var dlgNumber = 1; // 弹出窗口序号1到5
var isMove = false;

/**
 * 方法描述：页面弹出窗口方法 
 * param url 弹出窗口要显示的页面（相对路径） 
 * param dialogid 弹出窗口关闭时需要的id 
 * param title 窗口title 
 * param w 窗口宽度 
 * param h 窗口高度 
 * param modal 是否为模式窗口，true为是，false为不是 
 * param closable 
 * 默认窗口是否处于关闭状态。true 为关闭 
 * 调用demo：parent.getDlg("url","iframeId","title",宽度,高度)
 */
function getDlg(url, dialogid, title, w, h, modal, closable,maximizable,resizable) {
	if (!modal) {
		modal = false;
	}
	if (closable == null) {
		closable = true;
	}
	if (maximizable == null) {
		maximizable = false;
	}
	if (resizable == null) {
		resizable = false;
	}
	
	if (dlgNumber > 5) {
		$.messager.alert(getLanguageValue("tishi"), getLanguageValue("zuiduo"), 'info');
		dlgNumber == 5;
		return;
	}

	h = h + 30;
	var clientHeight = document.documentElement.clientHeight;
	var clientWidth = document.documentElement.clientWidth;
	if (h > clientHeight) {
		h = clientHeight;
	}
	if (w > clientWidth) {
		w = clientWidth;
	}
	// 弹出窗口div的id
	var dlgid = 'dlgDiv_' + dialogid;
	var dlgDiv = $("#" + dlgid);
	var dlgIframe = $("#iframe_" + dialogid);
	var window_mask = $("#" + dlgid + "-mask");
	if (dlgDiv.length == 0) {
		dlgDiv = $("<div></div>").appendTo("body");
		dlgDiv.attr("id", dlgid);
		dlgDiv.attr("name", "dlgDiv");
		dlgIframe = $("<iframe width=\"100%\" height=\"100%\" src=\"\" frameborder=\"0\"></iframe>").appendTo(dlgDiv);
		dlgIframe.attr("id", "iframe_" + dialogid);
		// 在窗口下面添加一个遮罩层，解决窗口被activie控件遮挡问题
		window_mask = $("<div id=\""
				+ dlgid
				+ "-mask\" class=\"window-maskDiv\"><iframe style=\"position: absolute; z-index: -1; width: 100%; height: 100%; top: 0;left:0;scrolling:no;\" frameborder=\"0\"></iframe></div>");
		window_mask.width(w);
		window_mask.height(h);
		window_mask.appendTo("body");
		dlgNumber++;
	}
  
	// 初始化窗口
	$('#' + dlgid).dialog({
		title : title,
		height : h,
		width : w,
		modal : modal,
		shadow : false,
		closable : closable,
		maximizable:maximizable,
		resizable:resizable,
		onMove : function(left, top) {
			if ($(this).panel('options').reSizing) {
				return;
			}
			var parentObj = $(this).panel('panel').parent();
			var width = $(this).panel('options').width;
			var height = $(this).panel('options').height;
			var right = left + width;
			var buttom = top + height;
			var parentWidth = parentObj.width();
			var parentHeight = parentObj.height();

			if (left < 0) {
				left = 0;
				$(this).panel('move', {
					left : 0
				});
			}
			if (top < 0) {
				top = 0;
				$(this).panel('move', {
					top : 0
				});
			}

			if (parentObj.css("overflow") == "hidden") {
				var inline = $.data(this, "window").options.inline;
				if (inline == false) {
					parentObj = $(window);
				}
				if (left > parentObj.width() - width) {
					left = parentObj.width() - width;
					$(this).panel('move', {
						"left" : parentObj.width() - width
					});
				}
				if (top > parentObj.height() - height) {
					top = parentObj.height() - height;
					$(this).panel('move', {
						"top" : parentObj.height() - height
					});
				}
			}
			window_mask.css('left', left);
			window_mask.css('top', top);
		},
		onClose : function() {
			$('#' + dlgid).dialog('destroy');
			$('#' + dlgid).attr("closeFlag", "");
			window_mask.remove();
			dlgNumber = dlgNumber - 1;
		},
		onResize:function(width, height){
			var left = $(this).panel('options').left;
			var top = $(this).panel('options').top;
			window_mask.css('left', left);
			window_mask.css('top', top);
			window_mask.css('width', width);
			window_mask.css('height', height);
		},
		onMinimize : function() {
			window_mask.css('display', 'none');
		},
		onOpen : function() {
			window_mask.css('display', 'block');
			var window_z_index = $(this).panel("panel").css('z-index');
			window_mask.css('z-index', window_z_index - 1);
		}
	});
	// 为属性closeFlag赋值
	$('#' + dlgid).attr("closeFlag", dialogid);
	dlgIframe = window.frames["iframe_" + dialogid];
	dlgIframe.location.href = url;
}

/**
 * 方法说明：关闭弹出窗口 调用demo：parent.getDlg("iframeId");或者top.getDlg("iframeId");
 */
function closeDlg(dialogid) {
	var dlgid = 'dlgDiv_' + dialogid;
	var dlgDiv = $("#" + dlgid);
	if (dlgDiv.length != 0) {
		dlgDiv.dialog('close');
	}
}

/**
 * 方法描述：隐藏所有已弹出的窗口
 */
function hideDlg(dialogid) {
	if (dialogid) {
		var dlgid = 'dlgDiv_' + dialogid;
		var dlgDiv = $("#" + dlgid);
		if (dlgDiv.length != 0) {
			dlgDiv.dialog('minimize');
		}
	} else {
		var dlgDivArray = $("div[name='dlgDiv']");
		$.each(dlgDivArray, function(i) {
			if ($(this).attr("closeFlag") != '') {
				$(this).dialog('minimize');
			}
		});
	}

}

/**
 * 方法描述：显示所有存在但是未显示的窗口
 */
function showDlg(dialogid) {
	if (dialogid) {
		var dlgid = 'dlgDiv_' + dialogid;
		var dlgDiv = $("#" + dlgid);
		if (dlgDiv.length != 0) {
			dlgDiv.dialog('open');
		}
	} else {
		var dlgDivArray = $("div[name='dlgDiv']");
		$.each(dlgDivArray, function(i) {
			if ($(this).attr("closeFlag") != '') {
				$(this).dialog('open');
			}
		});
	}
}
/** ---------------------弹出窗口定义结束--------------------------- */

/**
 * 方法说明：提示信息窗口 
 * @param title 提示信息窗口标题 
 * @param msg 提示信息内容
 * @param type 提示窗口类型（error,info,question,warning），一般操作错误信息使用error，操作结果提示信息使用info，页面校验错误信息可以使用warning
 * @param callbackFn 提示窗口关闭时的回调函数。
 */
function showAlert(title, msg, type, callbackFn) {
	$.messager.alert(title, msg, type, callbackFn);
}

/**
 * 默认datagrid属性定义
 */
(function() {
	if($.fn.datagrid!=null){
		$.extend($.fn.datagrid.defaults, {
			pageSize : 10,
			pageList : [ 5, 10, 15, 20, 50 ],
			striped : true,
			pagination : true,
			rownumbers : true,
			singleSelect : false,
			nowrap : true,
			toolbar : "#toolbar",
			onLoadSuccess : function(data) {
				$(this).datagrid('clearSelections'); // clear selected options
			},
			onHeaderContextMenu : function(e, field) {
				e.preventDefault();
				var menuid=$(this)[0].id+"_menu";
				if (!$('#'+menuid).length) {
					
					createColumnMenu($(this));
				}
				$('#'+menuid).menu('show', {
					left : e.pageX,
					top : e.pageY
				});
			}
		});
	}
})(jQuery);

/**
 * 方法描述：datagrid选择属性下拉菜单
 * 
 * @param datagrid datagrid的jquery对象
 */
function createColumnMenu(datagrid) {
	var menuid=datagrid[0].id+"_menu";
	var tmenu = $("<div id='"+menuid+"' style='width:100px;'></div>").appendTo('body');
	var fields = datagrid.datagrid('getColumnFields');
	for ( var i = 0; i < fields.length; i++) {
		if (fields[i] == "ck") {
			continue;
		}
		var text = $("td[field='" + fields[i] + "']").children(".datagrid-cell").children().first().html();
		if (!datagrid.datagrid('getColumnOption', fields[i]).hidden) {
			$('<div iconCls="icon-ok" id="1' + fields[i] + '"/>').html(text).appendTo(tmenu);
		} else {
			$('<div id="1' + fields[i] + '"/>').html(text).appendTo(tmenu);
		}
	}
	tmenu.menu({
		onClick : function(item) {
			if (item.iconCls == 'icon-ok') {
				datagrid.datagrid('hideColumn', item.id.substring(1));
				tmenu.menu('setIcon', {
					target : item.target,
					iconCls : 'icon-empty'
				});
			} else {
				datagrid.datagrid('showColumn', item.id.substring(1));
				tmenu.menu('setIcon', {
					target : item.target,
					iconCls : 'icon-ok'
				});
			}
		}
	});
}


/**
 * 功能描述：显示正在加载数据提示，页面要进行异步提交数据请求之前调用
 * 
 * @param message 提示信息内容，可以为空，如果为空则取默认值为“正在处理，请稍候。。。”
 */
function showLoadingMessage(message) {
	if (!document.getElementById("load-mask")) {
		$("<div id=\"load-mask\" class=\"datagrid-mask\"></div>").css({
			width : "100%",
			height : $(window).height()
		}).appendTo("body");
		var msg = $("<div id=\"load-mask-msg\" class=\"datagrid-mask-msg\"></div>").appendTo("body");
	}
	if (message) {
		$('#load-mask-msg').html(message);
	}else{
		$('#load-mask-msg').html("正在处理，请稍候。。。");
	}
	msg.css("left",($(document.body).outerWidth()-msg._outerWidth())/2);
	$('#load-mask').css('display', 'block');
	$('#load-mask-msg').css('display', 'block');
}

/**
 * 功能描述：隐藏正在加载数据提示，页面进行异步提交数据得到返回值时调用
 */
function hiddenLoadingMessage() {
	$('#load-mask').css('display', 'none');
	$('#load-mask-msg').css('display', 'none');
}


/**
 * 功能描述：加载输入框提示信息
 */
function loadWordChooseToolJs() {
//	var path = rootPath + "/amhmaintain/common/js/tools.js";
//	$.getScript(path, function() {
//		$("<div id='pop_div' class=\"pop_box\"></div>").appendTo("body").css({
//					display: "none",
//					position: "absolute",
//					background: "#FFF",
//					border: "1px solid #373737"
//		}).css("z-index",9999999);
//		
//		$(window).bind("mouseup",function(){
//			getSelectionText();
//			hide();
//		});
//	});
	var path = rootPath + "/amhmaintain/common/js/tools.js";
	$.getScript(path, function() {
		$("<div id='pop_div' class=\"pop_box\"></div>").appendTo("body").css({
					display: "none",
					position: "absolute",
					background: "#FFF",
					border: "1px solid #373737"
		}).css("z-index",9999999);
		
		document.body.onmouseup=function(){ 
			getSelectionText();
			hide();
		}; 
	});
}


/**
 * 功能描述：加载输入框录入时提示信息 input\textarea
 */
function loadInputChangeToolJs() {
	var path = rootPath + "/amhmaintain/common/js/tools.js";
	$.getScript(path, function() {
		$("<div id='pop_div' class=\"pop_box\"></div>").appendTo("body").css({
					display: "none",
					position: "absolute",
					background: "#FFF",
					border: "1px solid #373737"
		}).css("z-index",9999999);
		
		$(document).keyup(function() {
			inputkeyUp();
		});
		
		$(document).click(function(){
			inputTipClick();
		});
	});
}

