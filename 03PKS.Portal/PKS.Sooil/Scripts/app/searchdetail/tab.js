$(function(){
    $(document).on("click",".jurassic-tab-button",function(ev){
        var target$ = $(ev.currentTarget);
        var id = target$.attr("id");
        target$.addClass("jurassic-tab-button-selected").siblings().removeClass("jurassic-tab-button-selected");
        $(id + "_tab_fragment").addClass(".jurassic-tab-fragment-show").siblings().removeClass(".jurassic-tab-fragment-show");
    });
    $(".jurassic-tab-button:first").trigger("click");
});
