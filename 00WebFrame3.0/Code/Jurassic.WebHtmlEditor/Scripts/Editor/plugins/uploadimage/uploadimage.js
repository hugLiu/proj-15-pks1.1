/*******************************************************************************
* KindEditor - WYSIWYG HTML Editor for Internet
* Copyright (C) 2006-2011 kindsoft.net
*
* @author Roddy <luolonghao@gmail.com>
* @site http://www.kindsoft.net/
* @licence http://www.kindsoft.net/license.php
*******************************************************************************/

KindEditor.plugin('uploadimage', function (K) {
    var self = this, name = 'uploadimage';

    self.plugin.image = {
        upload: function () {
            var dialog = K.dialog({
                width: 500,
                title: '图片上传',
                body: '<iframe id="uploadFrame" src="/UpImg/UploadImg" width="100%" frameborder="0" scrolling="auto"></iframe>',
                closeBtn: {
                    name: '关闭',
                    click: function (e) {
                        dialog.remove();
                    }
                },
                yesBtn: {
                    name: '确定',
                    click: function (e) {
                        var uploadFrame = window.document.getElementById("uploadFrame");
                        var imgIds = $(uploadFrame.contentDocument).find("#imgIds").val();
                        imgIds = imgIds.split(',');
                        $.each(imgIds, function (i, v) {
                            var url = "/Files/GetImg/" + v;
                            self.exec('insertimage', url);
                        });
                        //<img src="/Files/GetSystemAvatar/545">
                        dialog.remove();
                    }
                },
                noBtn: {
                    name: '取消',
                    click: function (e) {
                        dialog.remove();
                    }
                }
            });
        }
    };
    self.clickToolbar(name, self.plugin.image.upload);
});
