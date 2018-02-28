/// <reference path="../vue.2.3.2.js" />
/*!
 * 搜索结果列表
 * 包含成果类型下拉及高级搜索
 */

var searchResultListVM = new Vue(
{
    el: '#searchresultlist',
    data: {
        searchResultList: [

        ]
    },
    methods: {
        loadData: function (list) {
            this.searchResultList = list;
        },
        loadDataByUrl: function (url) {

        }
    }
});

//searchResultListVM.searchResultList.push({});


//组件
Vue.component('searchitem',
{
    props: ['itemdata'],
    template: "<li><a :href='\"/detail?id=\"+id'>{{itemdata.title}}</a><br/>" +
        "<span>{{itemdata.PublishTime}}</span><span>{{itemdata.KeyWords}}</span>"
        + "</li>"
});

Vue.component('searchlist',
{
    props: ['data'],
    template: '<ul><searchitem v-for="item in data" v-bind:itemdata="item"></searchitem><ul>'
});
//<ul>
//  <searchitem v-for="item in searchResultList" v-bind:itemdata="item"></searchitem>
//</ul>


var data = {
    searchText: '',
    productTypeList: [],
    searchResultList: [

    ],
    created: function () {
        //created 这个钩子在实例被创建之后被调用
    }
}

//页面组件化
var app = new Vue({
    el: '#app',
    data: data,
    methods: {
        searchResult: function (list) {
            this.searchResultList = list;
        },
        searchResultByUrl: function (url) {

        },
        search: function (event) {
            var searchText = this.searchText;
            //todo 调用api加载数据
        }
    },
    watch: {
        searchText: function (newVal, oldVal) {
            //todo:searchText发生改变时触发搜索
            //这个回调将在searchText发生变化后调用
            this.search();
        }
    },
    computed: {
        // 计算属性
        searchResultCount: function () {
            //计算属性只有在它的相关依赖发生改变时才会重新求值
            return this.searchResultList.length;
        },
        filterSearchResults: function () {
            return this.searchResultList.filter(function (item) {
                return item.Title = "";
            });
        }
    }
});
//app.search();

/*表单
  <input v-model="searchText" placeholder="请输入搜索条件">
*/
/*
<div id="app">
    <searchbox></searchbox>
    <button v-on:click="search">搜索</button>
    <producttypelist></producttypelist>
    <searchlist v-bind:data="searchResultList"></searchlist>
</div>
*/

//高级搜索弹框

//<template v-if="loginType === 'username'">
//  <label>Username</label>
//  <input placeholder="Enter your username">
//</template>
//<template v-else>
//  <label>Email</label>
//  <input placeholder="Enter your email address">
//</template>



