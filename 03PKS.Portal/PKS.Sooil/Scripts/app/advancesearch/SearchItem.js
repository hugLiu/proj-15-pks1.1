Vue.component('searchitem',
{
    props: ['itemdata'],
    template: "<li><a :href='\"/detail?id=\"+id'>{{itemdata.title}}</a><br/>" +
        "<span>{{itemdata.PublishTime}}</span><span>{{itemdata.KeyWords}}</span>"
        + "</li>"
});