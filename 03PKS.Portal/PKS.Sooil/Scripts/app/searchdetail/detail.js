var appVue = new Vue({
    el:"#content",
    data:model,
    computed:{
        maxColumn:function(){
            return 2;
        },
        baseProperties:function(){
            var baseProps =[];
            var properties = this.article&&this.article.properties;
            if(!properties)
                return baseProps;
            var prop;
            for(var i= 0,j=properties.length;i<j;i++){
                prop = properties[i];
                if(!prop.wholeRow)
                    baseProps.push(prop);
            }
            var res = [],
                group,
                len = Math.floor(baseProps.length/this.maxColumn);
            for(var i=0;i<len;i++){
                group = [];
                for(var j=0;j<this.maxColumn;j++){
                    group.push(baseProps.shift());
                }
                res.push(group);
            }
            return res;
        },
        wholeRowProperties:function(){
            var res=[];
            var properties = this.article&&this.article.properties;
            if(!properties)
                return res;
            var prop;
            for(var i= 0,j=properties.length;i<j;i++){
                prop = properties[i];
                if(prop.wholeRow)
                    res.push(prop);
            }
            return res;
        }
    }
});


$(function(){
    $("#相关目标").jstree({
        core:{
            data:[
                { "id" : "ajson1", "parent" : "#", "text" : "Simple root node",icon:"glyphicon glyphicon-flash" },
                { "id" : "ajson2", "parent" : "#", "text" : "钻井",icon:"glyphicon glyphicon-ok",state:{opened:true} },
                { "id" : "ajson3", "parent" : "ajson2", "text" : "设计",icon:"glyphicon glyphicon-ok",state:{opened:true} },
                { "id" : "ajson4", "parent" : "ajson3", "text" : "工程设计" ,icon:"glyphicon glyphicon-flash"},
                { "id" : "ajson5", "parent" : "ajson3", "text" : "地质设计" ,icon:"glyphicon glyphicon-flash"},
                { "id" : "ajson6", "parent" : "ajson3", "text" : "程序设计" ,icon:"glyphicon glyphicon-flash"}
            ]
        }
    })
});

