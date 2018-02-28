/**
 修改的图标文件就是为了给新样式使用的，样式文件可以修改easyui.css也可以修改tree.css，如果修改的是后者的话，
 需要引入tree.css才行，这里拿easyui.css举例，找到"tree-checkbox2"样式的定义，在其下追加三个样式的定义
 （注意，必须是定义在tree-checkbox2样式后，这样才能覆盖前面的background定义）
 .tree-checkbox-disabled0{   
  background: url('images/tree_icons.png') no-repeat -272px -18px;   
}   
.tree-checkbox-disabled1{   
  background: url('images/tree_icons.png') no-repeat -272px 0;   
}   
.tree-checkbox-disabled2{   
  background: url('images/tree_icons.png') no-repeat -288px 0;   
}
*/ 
/**  
 * tree方法扩展  
 */  
$.extend($.fn.tree.methods, {   
    /**
     * 激活复选框  
     * @param {Object} jq  
     * @param {Object} target  
     */  
    enableCheck : function(jq, target) {    
        return jq.each(function(){   
            var realTarget;   
            if(typeof target == "string" || typeof target == "number"){   
                realTarget = $(this).tree("find",target).target;   
            }else{   
                realTarget = target;   
            }   
            var ckSpan = $(realTarget).find(">span.tree-checkbox");   
            if(ckSpan.hasClass('tree-checkbox-disabled0')){   
                ckSpan.removeClass('tree-checkbox-disabled0');   
            }else if(ckSpan.hasClass('tree-checkbox-disabled1')){   
                ckSpan.removeClass('tree-checkbox-disabled1');   
            }else if(ckSpan.hasClass('tree-checkbox-disabled2')){   
                ckSpan.removeClass('tree-checkbox-disabled2');   
            }   
        });   
    },   
    /**
     * 禁用复选框  
     * @param {Object} jq  
     * @param {Object} target  
     */  
    disableCheck : function(jq, target) {   
        return jq.each(function() {   
            var realTarget;   
            var that = this;   
            var state = $.data(this,'tree');   
            var opts = state.options;   
            if(typeof target == "string" || typeof target == "number"){   
                realTarget = $(this).tree("find",target).target;   
            }else{   
                realTarget = target;   
            }   
            var ckSpan = $(realTarget).find(">span.tree-checkbox");   
            ckSpan.removeClass("tree-checkbox-disabled0").removeClass("tree-checkbox-disabled1").removeClass("tree-checkbox-disabled2");   
            if(ckSpan.hasClass('tree-checkbox0')){   
                ckSpan.addClass('tree-checkbox-disabled0');   
            }else if(ckSpan.hasClass('tree-checkbox1')){   
                ckSpan.addClass('tree-checkbox-disabled1');   
            }else{   
                ckSpan.addClass('tree-checkbox-disabled2');
            }   
            if(!state.resetClick){   
                $(this).unbind('click').bind('click', function(e) {   
                    var tt = $(e.target);   
                    var node = tt.closest('div.tree-node');   
                    if (!node.length){return;}   
                    if (tt.hasClass('tree-hit')){   
                        $(this).tree("toggle",node[0]);   
                        return false;   
                    } else if (tt.hasClass('tree-checkbox')){   
                        if(tt.hasClass('tree-checkbox-disabled0') || tt.hasClass('tree-checkbox-disabled1') || tt.hasClass('tree-checkbox-disabled2')){   
                            $(this).tree("select",node[0]);   
                        }else{   
                            if(tt.hasClass('tree-checkbox1')){   
                                $(this).tree('uncheck',node[0]);   
                            }else{   
                                $(this).tree('check',node[0]);   
                            }   
                            return false;   
                        }   
                    } else {   
                        $(this).tree("select",node[0]);   
                        opts.onClick.call(this, $(this).tree("getNode",node[0]));   
                    }   
                    e.stopPropagation();   
                });   
            }   
        });   
    },
    hideNode:function(jq, target){
    	return jq.each(function() {   
            var realTarget;   
            if(typeof target == "string" || typeof target == "number"){   
                realTarget = $(this).tree("find",target).target;   
            }else{   
                realTarget = target;   
            }   
            $(realTarget).addClass("tree-node-hidden");
        }); 
    },
    showNode:function(jq, target){
    	return jq.each(function() {   
            var realTarget;   
            if(typeof target == "string" || typeof target == "number"){   
                realTarget = $(this).tree("find",target).target;   
            }else{   
                realTarget = target;   
            }   
            $(realTarget).removeClass("tree-node-hidden").addClass("tree-node-show");
        }); 
    }
    
}); 