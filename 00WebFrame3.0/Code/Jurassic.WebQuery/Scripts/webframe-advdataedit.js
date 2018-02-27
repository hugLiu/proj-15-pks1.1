(function ($) {
    var obj = function (opt) {
        var gridDetailIds = opt.gridDetailIds;
        var linkedPropertys = opt.linkedPropertys;
        var defaultButtons = opt.defaultButtons;
        var linkedDetailColumns = opt.linkedDetailColumns;
        var buttonEditProps = opt.buttonEditProps;
        var getLinkedListUrl = opt.getLinkedListUrl;
        var getDetailsLinkedUrl = opt.getDetailsLinkedUrl
        var detailsUrl = opt.detailsUrl;
        var copyUrl = opt.copyUrl;
        this.updateForm = bindFormData;
        this.drawSummaryCell = onDrawSummaryCell;
        var that = this;
        function bindFormData(data) {
            var form = new mini.Form("#DefaultForm");
            if (typeof data == "string") {
                data = $.parseJSON(data);
            }
            form.setData(data, false);
            setButtonEditText();
            gridDetailIds.forEach(function (collName) {
                var grid = mini.get("detailsgrid_" + collName);
                grid.loadData(data[collName]);
            })
        }

        function onDrawSummaryCell(e) {
            if (e.column.summaryType) {
                e.cellHtml = "<div style='text-align:right;width:100%'>" + $.lang(e.column.summaryType) + ": " +mini.formatNumber(e.cellHtml, e.column.numberFormat) + "</div>";
            }
        }

        //将每个子表记录总数显示在Tab页的title中
        function updateDetailsCount(collName) {
            var tab = mini.get("tabs");
            var tabPage = tab.getTab("tab_" + collName);
            var count = mini.get("detailsgrid_" + collName).data.length;
            var oldTitle = tabPage.title.split('\t')[0];
            tab.updateTab(tabPage, { title: oldTitle + '\t<small class="badge">' + count + '</small>' });
        }

        //执行copy命令，复制表单数据生成新单据
        function copyFormData(data) {
            var form = new mini.Form("#DefaultForm");
            if (typeof data == "string") {
                data = $.parseJSON(data);
            }
            form.setData(data, false);
            gridDetailIds.forEach(function (collName) {
                var grid = mini.get("detailsgrid_" + collName);
                grid.clearRows();
                grid.accept();
                data[collName].forEach(function (item) { item.Id = 0; });
                grid.addRows(data[collName], 0);
            })
        }

        //验证表单中的子表数据
        function validateGrid(grid) {
            grid.validate();
            if (grid.isValid() == false) {
                //alert("请校验输入单元格内容");
                var error = grid.getCellErrors()[0];
                grid.beginEditCell(error.record, error.column);
                return false;
            }
            return true;
        }

        defaultButtons.forEach(function (btnId) {
            if (btnId.indexOf('DefaultSave') == 0) {
                $.regButton({
                    id: btnId,
                    beforeAjax: function (ajax) {
                        return getDetailsGridData(ajax);
                    },
                    afterSubmit: function (data) {
                        bindFormData(data.ReturnValue);
                    }
                });
            }
            else if (btnId.indexOf('DefaultReturn') == 0) {
                $.regButton({
                    id: btnId,
                    noSubmit: top.goBack
                });
            }
            else if (btnId.indexOf('DefaultCopy') == 0) {
                $.regButton({
                    id: btnId,
                    beforeSubmit: function (ajax) {
                        var id = mini.get('#master_Id').getValue();
                        if (id == 0) {
                            showTips({ Type: 'error', Message: $.lang('New_Object_Cannot_Copy') });
                            return false;
                        }
                        if (!ajax.url) {
                            ajax.url = copyUrl;
                        }
                        ajax.data = { id: id }
                    },
                    afterSubmit: function (data) {
                        copyFormData(data.ReturnValue);
                    }
                });
            }
        });

        linkedPropertys.forEach(function (a) {
            var cbxDest = mini.get("master_" + a.prop);
            var cbxSrc = mini.get("master_" + a.linked);
            if (cbxSrc == null) {
                return;
            }
            cbxSrc.on("valuechanged", function (e) {
                cbxDest.setValue("");
                var linkedValue = cbxSrc.getValue();
                var url = getLinkedListUrl + encodeURIComponent(linkedValue) + "&prop=" + a.prop;
                cbxDest.setUrl(url);
                cbxDest.select(0);
            });
            var linkedValue = cbxSrc.getValue();
            var url = getLinkedListUrl + encodeURIComponent(linkedValue) + "&prop=" + a.prop;
            cbxDest.setUrl(url);
        });

        function setButtonEditText() {
            buttonEditProps.forEach(function (p) {
                var btnEdit = mini.get("master_" + p.prop);
                var display = mini.get("master_" + p.displayField);
                if (!display) return;
                btnEdit.setText(display.getValue());
            });
        }

        function getDetailsGridData() {
            var isValid = true;
            gridDetailIds.forEach(function (collName) {
                var grid = mini.get("detailsgrid_" + collName);
                if (!validateGrid(grid)) {
                    isValid = false;
                    return;
                }
                grid.commitEdit();
                //var data = grid.getChanges("modified");
                var data = grid.getChanges();
                var json = mini.encode(data);
                $('#hdn_' + collName).val(json);
            });
            return isValid;
        }

        function edit(e) {
            grid.beginEditCell(e.row);
        }

        $(gridDetailIds).each(function (idx, collName) {
            var grid = mini.get("detailsgrid_" + collName);
            var url = detailsUrl + collName;
            $.newGET(url, function (details) { grid.loadData(details); });

            grid.on("update", function () {
                updateDetailsCount(collName);
            });
            //设置明细表格的单元格联动
            var mylinkedColumns = linkedDetailColumns[idx];
            if (mylinkedColumns.length > 0) {
                grid.on("cellcommitedit", function (e) {
                    mylinkedColumns.forEach(function (col) {
                        if (col.linked == e.field) {
                            var field = col.prop;
                            e.record[field] = "";
                            grid.updateRow(e.record);
                        }
                    });
                });

                grid.on("cellbeginedit", function (e) {
                    mylinkedColumns.forEach(function (col) {
                        if (col.prop == e.field) {
                            var linkedValue = e.record[col.linked];
                            var collName = grid.id.replace("detailsgrid_", "")
                            var url = getDetailsLinkedUrl + encodeURIComponent(linkedValue) + "&prop=" + col.prop + "&detail=" + collName;
                            e.editor.setUrl(url);
                        }
                    });
                });

            }

            grid.on("rowdblclick", edit);
            grid.on("drawsummarycell", that.drawSummaryCell);
            var addBtnId = $('#detailmenu_' + collName + ' [id^="DefaultAdd"]').attr("id");
            var delBtnId = $('#detailmenu_' + collName + ' [id^="DefaultDelete"]').attr("id");
            if (delBtnId) {
                $.regButton({
                    id: delBtnId,
                    noSubmit: function (ajax) {
                        var rows = grid.getSelecteds();
                        if (rows.length > 0) {
                            grid.removeRows(rows, true);
                        }
                    }
                });
            }

            if (addBtnId) {
                $.regButton({
                    id: addBtnId,
                    noSubmit: function (ajax) {
                        var newRow = {};
                        grid.addRow(newRow, 0);
                        grid.beginEditCell(newRow);
                        updateDetailsCount(collName);
                    }
                });
            }
        });

        //搜索框的事件处理
        $.regButton({
            text: 'Filter',
            noSubmit: function () {
                //获取搜索框文本
                var key = $('.frame-search :text').val();

                $(gridDetailIds).each(function (idx, id) {
                    var grid = mini.get('detailsgrid_' + id);
                    grid.filter(function (row) {
                        for (var p in row) {
                            if ((row[p] || "").toString().indexOf(key) >= 0) {
                                return true;
                            }
                        }
                        return false;
                    });
                });
            }
        });
    };

    $.advDataEdit = function (opt) {
        var o = new obj(opt);
        $.advData = o;
    }
})(jQuery);
