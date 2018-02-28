var pksGlobalFooter = {
    bindFooter: function (path) {
        PKSUI.bind({
            el: "#footer",
            data: {
                apipath: path + "/api/SecurityService/GetPortalFooterMenu",
            },
            model: ["pks:footer"]
        });
    }
}
