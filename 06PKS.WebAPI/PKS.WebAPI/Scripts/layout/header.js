var pksGlobalHeader = {
    loadHeader: function () {
        var url = "/CommonInfo/GetPortalMenuUrl";
        PKSUI.http.get(url).then(function (result) {
            pksGlobalHeader.bindHeader(result.body);
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
