/**
 * 
 * 类描述: 定义加载datagrid类。
 *
 * @author zhanggf
 * @version 1.0
 * 创建时间： 2012-10-30 上午8:46:07
 *********************************更新记录******************************
 * 版本：  1.0       修改日期：         修改人：
 * 修改内容： 
 **********************************************************************
 */
(function($) {
	
	/**
     * 方法描述：定义加载datagrid类并定义类中的方法和属性
     */
	$.BaseJasDatagrid = function() {
		var _this =this;
		//定义公有方法或定义公共属性
//		_this.initDatagrid = function (functionNumber){
//			initDatagrid(functionNumber);
//		};
		_this.loadDatagrid = function (toolbarforgrid,url,functionNnmber,privilegecolumns,idField){
			loadDatagrid(toolbarforgrid,url,functionNnmber,privilegecolumns,idField);
		};
//		_this.EditableDataGrid = function (toobarid,url,datagridid,lastIndex){
//			EditableDataGrid(toobarid,url,datagridid,lastIndex);
//		};
//		_this.DatagridFooterRow = function (toobarid,url,datagridid,columns){
//			DatagridFooterRow(toobarid,url,datagridid,columns);
//		};
		_this.getButtonPrivilege = function (functionNnmber,datagridid,toolbar,idField){
			getButtonPrivilege(functionNnmber,datagridid,toolbar,idField);
		};
		_this.getColumnsPrivilege = function (toolbarforgrid,url,datagridid,idField){
			getColumnsPrivilege(toolbarforgrid,url,datagridid,idField);
		};
	};
	/**
	 * 方法描述：根据编号获得按钮权限并拼写按钮栏加载对象
	 * param functionNnmber 权限编号
	 * param datagridid 数据列权限number(datagrid的id)
	 * param toolbar 按钮信息（JSON）
	 */
	function getButtonPrivilege(functionNnmber,datagridid,toolbar,idField){
		var buttonfunction=[];
		$.getJSON( rootPath + "/jasframework/privilege/privilege/getbuttonPrivilege.do", {
			"priviligeNumber" : functionNnmber,
			"r" : new Date().getTime()
			}, function(buttonPrivilege) {
				//获取按钮的权限
				for(var i=0;i<buttonPrivilege.length;i++){
					var privailege={
						privilegeNumber:buttonPrivilege[i].privilegeNumber
					};
					buttonfunction.push(privailege);
				}
				var addMethod;
				var updateMethod;
				var queryMethod;
				var delMethod;
				var relatedMethod;
				var versionMethod;
				var locationMethod;
				var exportMethod;
				var url;
				var toolbarforgrid =[];
				for(var i=0;i<toolbar.length;i++){
					for(var j=0;j<buttonfunction.length;j++){
						var privilege = buttonfunction[j].privilegeNumber+"";
						if(toolbar[i].id == privilege.substr(privilege.length-2,privilege.length)){
							switch(toolbar[i].id){
								//查看
								case '02':
									queryMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-view',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 选择查看的记录
											var rows = $('#' + datagridid).datagrid('getSelections');
											if (rows.length == 1) {
												var row = $('#' + datagridid).datagrid('getSelected');
												var ids = "'"+eval('('+'row\.'+idField+')')+"'";
												eval(queryMethod+'('+ids+')');
											} else {
												top.showAlert('提示', '请选中一条记录', 'info');
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
								//新增
								case '03':
									addMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-add',
										handler:function(){
											eval(addMethod+'()');
										}
									};
									toolbarforgrid.push(xx);
								break;
								//修改
								case '04':
									updateMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-edit',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 选择查看的记录
											var rows = $('#' + datagridid).datagrid('getSelections');
											if (rows.length == 1) {
												var row = $('#' + datagridid).datagrid('getSelected');
												var ids = "'"+eval('('+'row\.'+idField+')')+"'";
												eval(updateMethod+'('+ids+')');
											} else {
												top.showAlert('提示', '请选中一条记录', 'info');
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
								//删除
								case '05':
									delMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-remove',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 找到所有被选中行
											var rows = $('#' + datagridid).datagrid('getSelections');
											// 是否已经选中记录
											if (rows.length > 0) {
												var ids = "";
												// 遍历取得所有被选中记录的id
												for ( var i = 0; i < rows.length; i++) {
													ids +=",'" + eval('('+'rows[i]\.'+idField+')')+"'";
												}
												if (ids.length > 0)
													ids = ids.substring(1);
												eval(delMethod+'(\"'+ids+'\")');
											} else {
												top.showAlert('提示', '未选择记录', 'info');
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
								//定位
								case '06':
									locationMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-edit',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 选择查看的记录
											var rows = $('#' + datagridid).datagrid('getSelections');
											if (rows.length > 0) {
												var ids = "";
												// 遍历取得所有被选中记录的id
												for ( var i = 0; i < rows.length; i++) {
													ids +=",'" + eval('('+'rows[i]\.'+idField+')')+"'";
												}
												if (ids.length > 0)
													ids = ids.substring(1);
												eval(locationMethod+'(\"'+ids+'\")');
											} else {
												eval(locationMethod+"('')");
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
								//导出
								case '07':
									exportMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-excel',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 选择查看的记录
											var rows = $('#' + datagridid).datagrid('getSelections');
											if (rows.length > 0) {
												var ids = "";
												// 遍历取得所有被选中记录的id
												for ( var i = 0; i < rows.length; i++) {
													ids +=",'" + eval('('+'rows[i]\.'+idField+')')+"'";
												}
												if (ids.length > 0)
													ids = ids.substring(1);
												eval(exportMethod+'(\"'+ids+'\")');
											} else {
												eval(exportMethod+"('')");
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
								//关联文档
								case '09':
									relatedMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-edit',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 选择查看的记录
											var rows = $('#' + datagridid).datagrid('getSelections');
											if (rows.length > 0) {
												var ids = "";
												// 遍历取得所有被选中记录的id
												for ( var i = 0; i < rows.length; i++) {
													ids +=",'" + eval('('+'rows[i]\.'+idField+')')+"'";
												}
												if (ids.length > 0)
													ids = ids.substring(1);
												eval(relatedMethod+'(\"'+ids+'\")');
											} else {
												eval(relatedMethod+"('')");
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
								//数据版本
								case '10':
									versionMethod =toolbar[i].method;
									var xx ={
										id:privilege,
										text:toolbar[i].name,
										iconCls:'icon-edit',
										handler:function(){
											var idField =$("#" + datagridid).datagrid('options').idField;
											// 选择查看的记录
											var rows = $('#' + datagridid).datagrid('getSelections');
											if (rows.length > 0) {
												var ids = "";
												// 遍历取得所有被选中记录的id
												for ( var i = 0; i < rows.length; i++) {
													ids +=",'" + eval('('+'rows[i]\.'+idField+')')+"'";
												}
												if (ids.length > 0)
													ids = ids.substring(1);
												 eval(versionMethod+'(\"'+ids+'\")');
											} else {
												eval(versionMethod+"('')");
											}
										}
									};
									toolbarforgrid.push(xx);
								break;
							}
							//数据版本
						
						}
					}
					if(toolbar[i].id== '00'){
						url =toolbar[i].url;
					}
				}
				getColumnsPrivilege(toolbarforgrid,url,datagridid,idField);
		});
		
	}
	/**
	 * 方法描述：根据数据列权限编号获得列的权限
	 * param toolbarforgrid 工具栏按钮数据 
	 * param url datagrid加载数据url
	 * param datagridid 数据列权限 
	 * param idField 主键 
	 */
	function getColumnsPrivilege(toolbarforgrid,url,datagridid,idField){
		$.getJSON( rootPath + "/jasframework/privilege/privilege/getcolumnPrivilege.do", {
			"datagridNumber" : datagridid,
			"r" : new Date().getTime()
			}, function(data) {
				var columns =[];
				var privilge=[];
				for(var i=0;i<data.length;i++){
					var xx ={
						field:data[i].name,
						//国际化
						title:getLanguageValue(data[i].privilegeNumber),//privilegeNumber
						width:150,
						sortable:true,
						resizable:true
					};
					columns.push(xx);
				}
				privilge.push(columns);
				loadDatagrid(toolbarforgrid,url,datagridid,privilge,idField);
		});
	}
	/**
	 * 方法描述：加载常用datagrid网格
	 *param toobarid 按钮工具栏
	 *param url 网格数据请求url
	 *param datagridid 网格table id
	 *param columns 网格列
	 */
	function loadDatagrid(toobarid,url,datagridid,columns,idField){
		//alert(JSON.stringify(columns));
		$('#'+datagridid).datagrid({
			url:url,
			frozenColumns:[[
                {field:'ck',checkbox:true}
			]],
			idField:idField,
			fitColumns:false,
			pagination:true,
			rownumbers:true,	
			columns:columns,
			toolbar:toobarid,
			onLoadSuccess:function(data){
		    	$('#'+datagridid).datagrid('clearSelections'); //clear selected options
		    },
			onHeaderContextMenu : function(e, field) {
				e.preventDefault();
				if (!$('#tmenu').length) {
					createColumnMenu($('#'+datagridid));
				}
				$('#tmenu').menu('show', { 
					left : e.pageX,
					top : e.pageY
				});
			}
		});
		if($('#queryDiv').height()!= undefined){
			//列表查询页面自适应窗口大小改变
			initDatagrigHeight(datagridid,'queryDiv',$('#queryDiv').height());
		}
	}
	
	/**
	 * 方法描述：加载可编辑datagrid网格
	 *param toobarid 按钮工具栏
	 *param url 网格数据请求url
	 *param datagridid 网格table id
	 *param columns 网格列
	 */
	function EditableDataGrid(toobarid,url,datagridid,lastIndex){
		$('#tt').datagrid({
			url:url,
			toolbar:toobarid,
			onBeforeLoad:function(){
				$(this).datagrid('rejectChanges');
			},
			onClickRow:function(rowIndex){
				if (lastIndex != rowIndex){
					$('#tt').datagrid('endEdit', lastIndex);
					$('#tt').datagrid('beginEdit', rowIndex);
				}
				lastIndex = rowIndex;
			}
		});
		if($('#queryDiv').height()!= undefined){
			//列表查询页面自适应窗口大小改变
			initDatagrigHeight(datagridid,'queryDiv',$('#queryDiv').height());
		}
	}
	/**
	 * 方法描述：加载可对某列进行统计操作的datagrid网格
	 *param toobarid 按钮工具栏
	 *param url 网格数据请求url
	 *param datagridid 网格table id
	 *param columns 列属性
	 */
	function DatagridFooterRow(toobarid,url,datagridid,columns){
		$('#'+datagridid).datagrid({
			url:url,
			showFooter:true,
			toolbar:toobarid,
			frozenColumns:[[
                {field:'ck',checkbox:true}
			]],
			pagination:true,
			rownumbers:true,	
			columns:columns,
			onLoadSuccess:function(data){
		    	$('#'+datagridid).datagrid('clearSelections'); //clear selected options
		    },
			onHeaderContextMenu : function(e, field) {
				e.preventDefault();
				if (!$('#tmenu').length) {
					createColumnMenu($('#'+datagridid));
				}
				$('#tmenu').menu('show', { 
					left : e.pageX,
					top : e.pageY
				});
			}
		});
		if($('#queryDiv').height()!= undefined){
			//列表查询页面自适应窗口大小改变
			initDatagrigHeight(datagridid,'queryDiv',$('#queryDiv').height());
		}
	}
})(jQuery);
