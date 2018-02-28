/**
 * function:解析xml字符串
 * xmlValue：xml字符串
 * return: xmlDom
 */
function parseXmlStrDoc(xmlValue){
	//跨浏览器，ie和火狐解析xml使用的解析器是不一样的。
	var xmlStrDoc=null;
	if (window.DOMParser){// Mozilla Explorer
	  parser=new DOMParser();
	  xmlStrDoc=parser.parseFromString(xmlValue,"text/xml");
	  return (xmlStrDoc);
	}else{// Internet Explorer
	  xmlStrDoc=new ActiveXObject("Microsoft.XMLDOM");
	  xmlStrDoc.async="false";
	  xmlStrDoc.loadXML(xmlValue);
	  return (xmlStrDoc);
	}
}

/**
 * function:解析xml
 * xmlUrl：xml地址
 * return: xmlDom
 */
function parseXmlDoc(xmlUrl){
	var xmlDoc = null;
	//跨浏览器，ie和火狐解析xml使用的解析器是不一样的。
	//支持IE浏览器
	if(window.ActiveXObject){
	   xmlDoc=new ActiveXObject("Microsoft.XMLDOM"); 
	}
	//支持Mozilla浏览器
	else if(document.implementation && document.implementation.createDocument){
	   xmlDoc = document.implementation.createDocument('','',null);
	}
	else{
	  //alert("here");
	}
	if(xmlDoc!=null){
	   xmlDoc.async = false;
	   xmlDoc.load(xmlUrl);
	   return (xmlDoc);
	}
}