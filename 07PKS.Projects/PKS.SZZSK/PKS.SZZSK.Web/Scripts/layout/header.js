var pksGlobalHeader = {
    loadHeader: function () {
        var url = "/CommonInfo/GetPortalMenuUrl";
        $.ajax({
            url: url,
            type: "get",
            success: function (portalMenuUrl) {
                pksGlobalHeader.bindHeader(portalMenuUrl);
            }
        });
    },
    bindHeader: function (portalMenuUrl) {
        PKSUI.bind({
            el: "#xt_header",
            data: {
                apipath: portalMenuUrl,
                navigation: window.navigation
            },
            model: ["pks:header2", "pks:menu"]
        });
    }
};
pksGlobalHeader.loadHeader();
