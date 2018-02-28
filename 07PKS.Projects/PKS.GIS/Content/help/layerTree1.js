var str='<document><Node Name="安信测试"><Node NO="4" Type="Trap" Name="圈闭" Rect="0" Batch="0"><Node ID="6130200" Name="圈闭" ThreeDType="POLYGON" ThreeDRangeH="3e+006" ThreeDRangeL="0" /></Node><Node NO="7" Type="LineNavigation" Name="二维测线" Rect="0" Batch="0"><Node ID="6120100" Name="二维测线" ThreeDType="LINE" ThreeDRangeH="5e+006" ThreeDRangeL="0" /></Node><Node NO="14" Type="Distriction" Name="行政区划线" Rect="0" Batch="0"><Node ID="1640300" Name="地级行政界线" ThreeDType="POLYLINE" ThreeDRangeH="5e+006" ThreeDRangeL="0" /><Node ID="1640200" Name="县级行政界线" ThreeDType="POLYLINE" ThreeDRangeH="1e+007" ThreeDRangeL="0" /><Node ID="1640100" Name="中国国界与省界" ThreeDType="POLYLINE" ThreeDRangeH="2e+007" ThreeDRangeL="0" /></Node><Node NO="21" Type="PlatForm" Name="平台" Rect="0" Batch="0"><Node ID="6332000" Name="平台" ThreeDType="POINT" ThreeDRangeH="600000" ThreeDRangeL="0" /></Node></Node></document>';
var checkNodeId="";
var isCheck=null;
var commandList = new Array();

function createTree(){
	var userName=getParamter("userName");
	
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var result = earthAX.Logon( userName, 123 );
//	str = earthAX.UserObjList( userName );
//	alert(str);
	
	var data=xmlToJson(str).document.Node;
	var treeJson=createTreeJson(data);
	$("#layerTree").tree({
		data:treeJson,
		checkbox:true,
		lines:true,
		onClick:function(node){
			
		},
		onCheck:function(node,onCheck){
			if(node.id==checkNodeId && isCheck==onCheck){
				return;
			}
			var isLeaf=$(this).tree("isLeaf",node.target);
			var childrenNodes=null;
			if(!isLeaf){
				childrenNodes=$(this).tree("getChildren",node.target);
			}
//			//获取孩子节点
//			var childNodes = $("#layerTree").tree("getChildren",node.target);
			isCheck=onCheck;
			if(onCheck){
				if(childrenNodes==null){
					checkNodeId=node.id;
					node.iconCls= 'icon-tree-classify-node-leaf-checked';
					$(this).tree('update', node);
				}else{
					changeNodeIconCls($(this),childrenNodes,onCheck);
				}
			}else{
				if(childrenNodes==null){
					checkNodeId=node.id;
					node.iconCls= 'icon-tree-classify-node-leaf';
					$(this).tree('update', node);
				}else{
					changeNodeIconCls($(this),childrenNodes,onCheck);
				}
			}	
		}
	});
}
function changeNodeIconCls(treeObj,childrenNodes,onCheck){
	if(onCheck){
		for(var i=0;i<childrenNodes.length;i++){
			var childrenNode=childrenNodes[i];
			var isLeaf=treeObj.tree("isLeaf",childrenNode.target);
			if(isLeaf){
				checkNodeId=childrenNode.id;
				childrenNode.iconCls= 'icon-tree-classify-node-leaf-checked';
				treeObj.tree('update', childrenNode);
			}else{
				var childNodes=treeObj.tree("getChildren",childrenNode.target);
				changeNodeIconCls(treeObj,childNodes,onCheck);
			}
		}
	}else{
		for(var i=0;i<childrenNodes.length;i++){
			var childrenNode=childrenNodes[i];
			var isLeaf=treeObj.tree("isLeaf",childrenNode.target);
			if(isLeaf){
				checkNodeId=childrenNode.id;
				childrenNode.iconCls= 'icon-tree-classify-node-leaf';
				treeObj.tree('update', childrenNode);
			}else{
				var childNodes=treeObj.tree("getChildren",childrenNode.target);
				changeNodeIconCls(treeObj,childNodes,onCheck);
			}
		}
	}
}
function xmlToJson(xml) {
//	xml=(xml+"").replace("\\","");
    var index=xml.indexOf("\n<");
//    xml=xml.substring(index+1,xml.length);
    xml=$.parseXML(xml);
		tab = '';
		var X = {
			toObj : function(xml) {
				var o = {};
				if (xml.nodeType == 1) {
					if (xml.attributes.length)
						for (var i = 0; i < xml.attributes.length; i++)
							o["" + xml.attributes[i].nodeName] = (xml.attributes[i].nodeValue || "")
									.toString();
					if (xml.firstChild) {
						var textChild = 0, cdataChild = 0, hasElementChild = false;
						for (var n = xml.firstChild; n; n = n.nextSibling) {
							if (n.nodeType == 1)
								hasElementChild = true;
							else if (n.nodeType == 3
									&& n.nodeValue.match(/[^ \f\n\r\t\v]/))
								textChild++;
							else if (n.nodeType == 4)
								cdataChild++;
						}
						if (hasElementChild) {
							if (textChild < 2 && cdataChild < 2) {
								X.removeWhite(xml);
								for (var n = xml.firstChild; n; n = n.nextSibling) {
									if (n.nodeType == 3)
										o["#text"] = X.escape(n.nodeValue);
									else if (n.nodeType == 4)
										o["#cdata"] = X.escape(n.nodeValue);
									else if (o[n.nodeName]) {
										if (o[n.nodeName] instanceof Array)
											o[n.nodeName][o[n.nodeName].length] = X
													.toObj(n);
										else
											o[n.nodeName] = [
													o[n.nodeName],
													X.toObj(n) ];
									} else
										o[n.nodeName] = X.toObj(n);
								}
							} else {
								if (!xml.attributes.length)
									o = X.escape(X.innerXml(xml));
								else
									o["#text"] = X.escape(X.innerXml(xml));
							}
						} else if (textChild) {
							if (!xml.attributes.length)
								o = X.escape(X.innerXml(xml));
							else
								o["#text"] = X.escape(X.innerXml(xml));
						} else if (cdataChild) {
							if (cdataChild > 1)
								o = X.escape(X.innerXml(xml));
							else
								for (var n = xml.firstChild; n; n = n.nextSibling)
									o["#cdata"] = X.escape(n.nodeValue);
						}
					}
					if (!xml.attributes.length && !xml.firstChild)
						o = null;
				} else if (xml.nodeType == 9) {
					o = X.toObj(xml.documentElement);
				} else
					alert("unhandled node type: " + xml.nodeType);
				return o;
			},
			toJson : function(o, name, ind) {
				var json = name ? ("\"" + name + "\"") : "";
				if (o instanceof Array) {
					for (var i = 0, n = o.length; i < n; i++)
						o[i] = X.toJson(o[i], "", ind + "\t");
					json += (name ? ":[" : "[")
							+ (o.length > 1 ? ("\n" + ind + "\t"
									+ o.join(",\n" + ind + "\t") + "\n" + ind)
									: o.join("")) + "]";
				} else if (o == null)
					json += (name && ":") + "null";
				else if (typeof (o) == "object") {
					var arr = [];
					for ( var m in o)
						arr[arr.length] = X.toJson(o[m], m, ind + "\t");
					json += (name ? ":{" : "{")
							+ (arr.length > 1 ? ("\n" + ind + "\t"
									+ arr.join(",\n" + ind + "\t") + "\n" + ind)
									: arr.join("")) + "}";
				} else if (typeof (o) == "string")
					json += (name && ":") + "\"" + o.toString() + "\"";
				else
					json += (name && ":") + o.toString();
				return json;
			},
			innerXml : function(node) {
				var s = ""
				if ("innerHTML" in node)
					s = node.innerHTML;
				else {
					var asXml = function(n) {
						var s = "";
						if (n.nodeType == 1) {
							s += "<" + n.nodeName;
							for (var i = 0; i < n.attributes.length; i++)
								s += " "
										+ n.attributes[i].nodeName
										+ "=\""
										+ (n.attributes[i].nodeValue || "")
												.toString() + "\"";
							if (n.firstChild) {
								s += ">";
								for (var c = n.firstChild; c; c = c.nextSibling)
									s += asXml(c);
								s += "</" + n.nodeName + ">";
							} else
								s += "/>";
						} else if (n.nodeType == 3)
							s += n.nodeValue;
						else if (n.nodeType == 4)
							s += "<![CDATA[" + n.nodeValue + "]]>";
						return s;
					};
					for (var c = node.firstChild; c; c = c.nextSibling)
						s += asXml(c);
				}
				return s;
			},
			escape : function(txt) {
				return txt.replace(/[\\]/g, "\\\\").replace(/[\"]/g, '\\"')
						.replace(/[\n]/g, '\\n').replace(/[\r]/g, '\\r');
			},
			removeWhite : function(e) {
				e.normalize();
				for (var n = e.firstChild; n;) {
					if (n.nodeType == 3) {
						if (!n.nodeValue.match(/[^ \f\n\r\t\v]/)) {
							var nxt = n.nextSibling;
							e.removeChild(n);
							n = nxt;
						} else
							n = n.nextSibling;
					} else if (n.nodeType == 1) {
						X.removeWhite(n);
						n = n.nextSibling;
					} else
						n = n.nextSibling;
				}
				return e;
			}
		};
		if (xml.nodeType == 9)
			xml = xml.documentElement;
		var json = X.toJson(X.toObj(X.removeWhite(xml)), xml.nodeName, "\t");
		return JSON.parse("{"+ tab+ (tab ? json.replace(/\t/g, tab) : json.replace(/\t|\n/g,"")) + "}");
}
function createTreeJson(dataObj){
	var treeObj=new Array();
	if(dataObj.length>1){
		for(var i=0;i<dataObj.length;i++){
			var data=dataObj[i];
			var name=data.Name;
			var node=data.Node;
			var id1=data.ID;
			var id2=data.NO;
			var id=id1==null?id2:id1;
			var nodeData=null;
			var obj={};
			var attributes={"ThreeDType":data.ThreeDType};
			if(node!=null && node!=""){
				nodeData=getChildrenData(node);
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes};
			}else{//iconCls
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-close"};
			}
			treeObj.push(obj);
		}
	}else{
		var name=dataObj.Name;
		var node=dataObj.Node;
		var id1=dataObj.ID;
		var id2=dataObj.NO;
		var id=id1==null?id2:id1;
		var nodeData=null;
		var obj={};
		var attributes={"ThreeDType":dataObj.ThreeDType};
		if(node!=null && node!=""){
			nodeData=getChildrenData(node);
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node"};
		}else{//iconCls
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-leaf"};
		}
		treeObj.push(obj);
	}
	return treeObj;
}
function getChildrenData(childrenData){
	var treeChildrenObj=new Array();
	if(childrenData.length>1){
		for(var i=0;i<childrenData.length;i++){
			var data=childrenData[i];
			var name=data.Name;
			var node=data.Node;
			var id1=data.ID;
			var id2=data.NO;
			var id=id1==null?id2:id1;
			var nodeData=null;
			var obj={};
			var attributes={"ThreeDType":data.ThreeDType};
			if(node!=null && node!=""){
				nodeData=getChildrenData(node);
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node"};
			}else{
				//alert(id+","+name+","+data.ThreeDType);
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-leaf"};
			}
			treeChildrenObj.push(obj);
		}
	}else{
		var name=childrenData.Name;
		var node=childrenData.Node;
		var id1=childrenData.ID;
		var id2=childrenData.NO;
		var id=id1==null?id2:id1;
		var nodeData=null;
		var obj={};
		var attributes={"ThreeDType":childrenData.ThreeDType};
		if(node!=null && node!=""){
			nodeData=getChildrenData(node);
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node"};
		}else{
			//alert(id+","+name+","+childrenData.ThreeDType);
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-leaf"};
		}
		treeChildrenObj.push(obj);
	}
	return treeChildrenObj;
}