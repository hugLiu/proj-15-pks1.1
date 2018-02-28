
/*
 * 获取Tab页IFrame对象
 * @param {Object} strid
 * @return {TypeName} 
 */
function getTabIFrameById(strid)
{
	var iframe=top.document.frames["frm"+strid];
	if(iframe)
	{
		return iframe;
	}
	else
	{
		return;	
	}
}
function getTabIFrame(strid)
{
	//alert(top.document.all("contentArea").id);
	var iframe=top.document.frames[strid];
	if(iframe)
	{		
		return iframe;
	}
	else
	{
		return;	
	}
}
function getEarthIframe(){
	var iframe=top.document.frames["frm3d"];
	if(iframe )
	{
		return iframe;
	}
	else
	{
		showmap3d();
		return getEarthIframe();
	}
}

function getMapObject()
{
	if(top.mapViewer==null)
	{		
		var iframe=top.document.frames["frm2d"];
		if(iframe)
		{		
			var mapObj=iframe.window.getMapApp();
			top.mapViewer=mapObj;
			return mapObj;
		}
		else
		{
			return null;	
		}
	}
	else
	{
		return top.mapViewer;
	}
}


function getMapIFrame()
{
	var iframe=top.document.frames["frm2d"];
	if(iframe )
	{
		return iframe;
	}
	else
	{
		showmap2d();
		//return getMapIFrame();
	}	
}

var clock;

function setStatusBarText(value)
{
	if(value!="clear")
	{
		window.clearTimeout(clock);
		this.window.status=value;
		clock=window.setTimeout("setStatusBarText('clear')", 15 * 1000);
	}
	else
	{
		this.window.status="";
	}
}

function getGisRestUrl(layerName)
{
	if(top.arcGisLayer!=null)
	{
		layerName=layerName.toUpperCase();
		var strIndex=top.arcGisLayer[layerName];
		if(strIndex!=null&&strIndex!="")
		{
			var strReturn=top.arcgisRestUrl+"/gisdata/MapServer/"+strIndex;
			return strReturn;
		}
		else
		{
			return "";
		}			
	}
	else
	{
		return "";
	}
}

function getFeatureRestUrl(layerName)
{	
	if(top.arcFeatureLayer!=null)
	{
		layerName=layerName.toUpperCase();
		var strIndex=top.arcFeatureLayer[layerName];
		if(strIndex!=null&&strIndex!="")
		{
			var strReturn=top.arcgisRestUrl+"/platformfeature/FeatureServer/"+strIndex;
			return strReturn;
		}
		else
		{
			return "";
		}
	}
	else
	{
		return "";
	}
}

/**
 * 调用ArcGIS Rest服务
 * @param {Object} serviceUrl
 * @param {Object} requestData
 * @param {Object} callback
 */
function postRestService(serviceUrl,requestData,callback)
{
		var strUrl=top.document.location+"";
    	var iIndex=strUrl.indexOf("/",7);
    	strUrl=strUrl.substring(0,iIndex);  
    	strUrl=strUrl+top.crossDomainUrl;
    	requestData["destination"]=serviceUrl;
    	requestData["f"]="json";
    	$.post(strUrl,requestData,function(data){var retjson=eval("("+data+")"); callback(retjson);},"json");
}

function getLayerMap()
{
	try
	{		
		var url=top.arcgisRestUrl+"/platformfeature/FeatureServer";
		var strName;
		var strIndex;
		var arrName;
		var arr;
		postRestService(url,{},function(data){
				if(data!=null)
				{							
					arr=data.layers;
					if(arr && arr.length>0)
					{
						top.arcFeatureLayer={};
						
						for(i=0;i<arr.length;i++)
						{
							strName=arr[i].name;
							arrName=strName.split(".");
							strName=arrName[1];
							strName=strName.toUpperCase();									
							strIndex=arr[i].id.toString();									
							top.arcFeatureLayer[strName]=strIndex;
						}
					}
				}
				
			},"json");
			
		url=top.arcgisRestUrl+"/gisdata/MapServer";
		postRestService(url,{},function(data){
				if(data!=null)
				{							
					arr=data.layers;
					if(arr && arr.length>0)
					{
						top.arcGisLayer={};
						
						for(i=0;i<arr.length;i++)
						{
							strName=arr[i].name;
							arrName=strName.split(".");
							strName=arrName[1];
							strName=strName.toUpperCase();
							//alert(strName);									
							strIndex=arr[i].id.toString();									
							top.arcGisLayer[strName]=strIndex;
						}
					}
				}
				
			},"json");
		
	}
	catch(e)
	{
		top.arcGisLayer=null;
		top.arcFeatureLayer=null;
		alert("获取FeatureService有误");
	}
}

//缓存ArcGIS Rest Url
getLayerMap();


/**
*转换经纬度和80坐标(只返回xy坐标，以逗号隔开)
*x,y根据你输入的xy范围自动判断是经纬度还是80坐标，然后进行相应转换
*transCallbackFunction：回调函数,失败返回空，成功返回xy字符串，逗号隔开
*/
function transGeoXY(x,y,transCallbackFunction){
	var x=Number(x);
	var y=Number(y);
	var requestData={};
	if(x>0 && x<180 && y>0 && y<90){
		requestData={
			'inSR':4326,
			'outSR':2383
		}
	}
	else{
		requestData={
			'inSR':2383,
			'outSR':4326
		}
	}
	
	var inGeometries=[{'geometryType':"esriGeometryPoint",'geometries':[{'x':x,'y':y}]}];
	requestData["inGeometries"]=JSON.stringify(inGeometries);
	
	try
	{
		var serviceUrl=top.arcgisRestUrl+'/apdm/MapServer/exts/apdm/geometryTransform';
		postRestService(serviceUrl,requestData,function(data){
			if(data.outGeometries!= "undefined"){
				var xyarr =data.outGeometries[0].geometries;
				var x= xyarr[0].x;
				var y= xyarr[0].y;
				//如果是经纬度就保留6位，80保留3位
				if(x>0 && x<180)
					x=x.toFixed(6);
				else
					x=x.toFixed(3);
				if(y>0 && y<90)
					y=y.toFixed(6);
				else
					y=y.toFixed(3);
				var xy = x+','+y;
				transCallbackFunction(xy);
			}
			else{
				alert("坐标转换失败");
				return false;
			}
		});
	}
	catch(e)
	{
		alert(e);
		return false;
	}
}

/**
**通过坐标 和搜索范围 计算距离最近的一个桩 和所属于的站列区间
* searchDistance 距离范围
**/
function getStationMarkerByLocation(pointx,pointy,searchDistance,callBackFunction){
	var resturl = top.arcgisRestUrl+"/apdm/MapServer/exts/apdm/pointToLocation";
	var wkid ="";
	if(pointx > 10000 && pointy > 10000){
		wkid = "2383"
	}
	else if(pointx <180 && pointx>0 && pointy>0 && pointy <90){
		wkid = "4326";
	}
	var locationStr = "{x:"+pointx+",y:"+pointy+"}";
	var postdata ={"inputPoint":locationStr,
		"searchDistance":searchDistance,
		"wkid":wkid
	};
	try{
		 top.postRestService(resturl,postdata,function(data){
			 if(data !=null){
				 if(data.hasOwnProperty("error")){
				     callBackFunction(false);
				 }else if(data["stationSeriesEventID"]!=null && data["stationSeriesEventID"]!=""){
					 data["station"] =  data.station.toFixed(2);
					 callBackFunction(data);
				 }
	    	     else{
	    	    	 callBackFunction(1); //没有找到
	    	     }
			 }else{
				  callBackFunction(false);
			 }
			 
         });
	}catch(error){	
		 callBackFunction(false);
	}
    
}

//面查询面板加载定时器
var iClock;
var iAreaQueryCount=0;

/**
 * 点面查询调用中间桥梁
 * @param {string} strGeometryJson
 */
function geoAreaQuery(strGeometryJson)
{
	try
	{		
		var iframe=top.document.frames["frm03010102"];
		if(iframe)
		{		
			iframe.window.setQueryPolygon(strGeometryJson);
			top.L("03010102","空间搜索","../../gis/geosearch/geosearch.htm");
		}
		else
		{			
			top.L("03010102","空间搜索","../../gis/geosearch/geosearch.htm");				
			iClock=window.setTimeout("getAreaQueryIFrame("+strGeometryJson+")", 500);
		}
	}
	catch(error)
	{
		alert(error+"-");		
	}
}

/**
 * 定时器获得面查询过滤面板
 * @param {string} strGeometryJson
 */
function getAreaQueryIFrame(strGeometryJson)
{
	var iframe=top.document.frames["frm03010102"];	
	
	if(iframe&&iframe.window.setQueryPolygon)
	{				
		window.clearTimeout(iClock);
		iframe.window.setQueryPolygon(strGeometryJson);
		top.L("03010102","空间搜索","../../gis/geosearch/geosearch.htm");
	}
	else
	{
		top.L("03010102","空间搜索","../../gis/geosearch/geosearch.htm");
		iClock=window.setTimeout("getAreaQueryIFrame("+strGeometryJson+")", 500);				
	}
	iAreaQueryCount++
	
	if(iAreaQueryCount>20)
	{
		window.clearTimeout(iClock);
		alert("过滤列表加载失败");
	}				
}


function getCenterline(fromStationSeriesEventID,fromStation,toStationSeriesEventID,toStation,callback){
	var requestData={};
	requestData['fromStationSeriesEventID']=fromStationSeriesEventID;
	requestData['fromStation']=fromStation;
	requestData['toStationSeriesEventID']=toStationSeriesEventID;
	requestData['toStation']=toStation;
	requestData['f']='json';
	try
	{
		var serviceUrl=top.arcgisRestUrl+'/apdm/MapServer/exts/apdm/getCenterline';
		postRestService(serviceUrl,requestData,function(data){
			var xyarr =data.centerline;
			callback(xyarr);
		});
	}
	catch(e)
	{
		alert(e);
		return false;
	}
}
function stationToPoint(stationSeriesEventID,station,callback){
	var requestData={};
	requestData['stationSeriesEventID']=stationSeriesEventID;
	requestData['station']=station;
	requestData['f']='json';
	try
	{
		var serviceUrl=top.arcgisRestUrl+'/apdm/MapServer/exts/apdm/stationToPoint';
		postRestService(serviceUrl,requestData,function(data){
			var xyarr =data.point;
			callback(xyarr);
		});
	}
	catch(e)
	{
		alert(e);
		return false;
	}
}

/**
 * 
 * @param {String} url
 */
function getServerUrl(uri)
{
	var strUrl;
	var iIndex;
	var iIndex2;
	
	if(uri.indexOf("http")==0)
	{
		return uri;
	}
	else if(uri.indexOf("/")==0)
	{
		strUrl=top.document.location+"";
    	iIndex=strUrl.indexOf("/",7);
    	if(iIndex>=0)
		{
			iIndex=strUrl.indexOf("/",iIndex+1);
			strUrl=strUrl.substring(0,iIndex);
    		strUrl=strUrl+uri;
		}
    	else
		{
			strUrl=strUrl+url;
		}
    	return strUrl;    	
	}
	else if(uri.indexOf(":")==0)
	{
		strUrl=top.document.location+"";
    	iIndex=strUrl.indexOf("/",7);
    	
    	if(iIndex>=0)
    		strUrl=strUrl.substring(0,iIndex);
    	
    	iIndex=strUrl.indexOf(":",7);
    	
    	if(iIndex>=0)
		{
			strUrl=strUrl.substring(0,iIndex);
		}
    	strUrl=strUrl+uri;
    	return strUrl;
	}
	else
	{
		strUrl=top.document.location+"";
    	iIndex=strUrl.indexOf("/",7);
    	strUrl=strUrl.substring(0,iIndex);
    	strUrl=strUrl+"/"+uri;
    	return strUrl;
	}
}