$(function () {
    var tree = mini.get("query_tree");
    var grid = mini.get("query_grid");
    var cbxSavedQuery = mini.get("query_cbxSavedQuery");
    var tabAdvQuery = mini.get("tab_AdvQuery");
    var fieldsData = mini.get("select_Field").data;
    var firstProp = fieldsData[0].id;
    var propsData = mini.get("")
    var itemNodes;

    function initGridTree() {
        if (!itemNodes) {
            itemNodes = [];
        }
        if (itemNodes.length == 0) {
            var rootNode = { ExpressionText: 'AND', Expression: "AND", Type: 'Operator', Id: 1 };
            itemNodes.push(rootNode);
        }
        if (tabAdvQuery.activeIndex == 0) {
            initGrid();
        }
        else {
            initTree();
        }
    }

    initGridTree();
    //grid.on("drawcell", drawCell);
    //tree.on("drawcell", drawCell);
    //function drawCell(e) {
    //    if (e.field == "ValueText") {
    //        var record = e.record;
    //        e.cellHtml = !mini.isNull(record.ValueText) ? record.ValueText : record.Value;
    //    }
    //}

    function initGrid() {
        grid.setData(itemNodes);
        grid.filter(function (row) {
            return row.Type != 'Operator';
        });
        if (itemNodes[0].Expression == 'AND') {
            mini.get('btn_simpleAnd').setChecked(true);
            mini.get('btn_simpleOr').setChecked(false);
        }
        else {
            mini.get('btn_simpleOr').setChecked(true);
            mini.get('btn_simpleAnd').setChecked(false);
        }
    }

    function initTree() {
        tree.loadList(itemNodes);
    }

    //http://www.miniui.com/demo/#src=datagrid/celledit2.html

    function getEditor(record) {
        var id = "query_" + record.Expression;
        var editor = mini.get(id);
        return editor;
    }

    function cellBeginEdit(e) {
        if (e.record.Type == "Operator" && e.field != "ExpressionText") {
            e.cancel = true;
        }

        var editor;
        if (e.field == "ValueText") {
            editor = getEditor(e.record);
            e.editor = editor;
            e.column.editor = editor;
        }
        else if (e.field == "ExpressionText") {
            if (e.record.Type == 'Operator') {
                editor = mini.get('select_AndOr');
            }
            else {
                editor = mini.get('select_Field');
            }
            e.editor = editor;
            e.column.editor = editor;
        }
    }

    function cellCommitEdit(e) {
        var record = e.record;
        if (e.field == "ValueText") {
            if (e.editor.getText) {
                record.ValueText = e.editor.getText();
            }
            else {
                record.ValueText = e.value;
            }
            record.Value = e.value;
            e.value = record.ValueText;
        }
        if (e.field == "ExpressionText") {
            if (e.editor.getText) {
                record.ExpressionText = e.editor.getText();
            }
            else {
                record.ExpressionText = e.value;
            }
            record.Expression = e.value;
            e.value = record.ExpressionText;
        }
    }

    grid.on("cellbeginedit", cellBeginEdit);
    tree.on("cellbeginedit", cellBeginEdit);

    grid.on("cellcommitedit", cellCommitEdit);
    tree.on("cellcommitedit", cellCommitEdit);

    function simpleAnd() {
        itemNodes[0].Expression = 'AND';
        itemNodes[0].ExpressionText = 'AND';
        if (tabAdvQuery.selectedIndex == 1) {
            tree.updateNode(itemNodes[0]);
        }
    }
    function simpleOr() {
        itemNodes[0].Expression = 'OR';
        itemNodes[0].ExpressionText = 'OR';
        if (tabAdvQuery.selectedIndex == 1) {
            tree.updateNode(itemNodes[0]);
        }
    }

    function simpleAddNode() {
        row = { Id: getAId(), ParentId: 1, ExpressionText: firstProp, Expression: firstProp, Type: 'Expr', Operator: '=', Value: '', ValueText: '' };
        grid.addRow(row);
    };

    function simpleRemoveNode() {
        var row = grid.getSelected();
        if (!row) {
            showTips({ Type: "warning", Title: $.lang('Plz_Select_A_Record') });
            return;
        }
        grid.removeRow(row, true);
    }

    function showQueryItem() {
        var item = null;
        var id = cbxSavedQuery.getValue();
        for (var i in cbxSavedQuery.data) {
            if (cbxSavedQuery.data[i].Id == id) {
                item = cbxSavedQuery.data[i];
                break;
            }
        }
        if (item == null) {
            return;
        }
        itemNodes = item.Nodes || [];
        initGridTree();
    }

    tabAdvQuery.on("activechanged", function () {
        if (tabAdvQuery.activeIndex == 0) {
            itemNodes = tree.getList();
        }
        else {
            itemNodes = grid.getData();
        }
        initGridTree();
    });

    function updateItemNodes() {
        if (tabAdvQuery.activeIndex == 1) {
            itemNodes = tree.getList();
        }
        else {
            itemNodes = grid.getData();
        }
    }

    cbxSavedQuery.on("valuechanged", showQueryItem);
    tree.on("beforedrop", onBeforeDrop);

    function onBeforeDrop(e) {
        /*
        {
          sender: Object,         //树对象
          dragNode: Object,         //拖拽的节点
          dropNode: Object,         //目标投放节点
          dragAction: String,        //投放方式：add/after/before
          cancel: Boolean
        }*/
        if (e.dragNode.Type == "Operator" && e.dropNode.Type == "Expr" && e.dragAction == "add") {
            showTips({ Type: "error", Title: $.lang("AND/OR can not be the lower node of expression") });
            e.cancel = true;
        }
    }

    $('#winAdvSearch [id^=btn_]').each(function () {
        $(this).click(function () {
            eval(this.id.replace('btn_', '') + '()')
        });
    });

    function getAId() {
        var max = 1;
        for (var i in itemNodes) {
            if (itemNodes[i].Id > max) {
                max = itemNodes[i].Id;
            }
        }
        return max + 1;
    }

    function addAndOr(andOr) {
        var node = tree.getSelectedNode();
        if (node == null) {
            node = itemNodes[0];
        }
        while (node.Type == "Expr") {
            node = tree.getParentNode(node);
            if (node == null) return;
        }
        var newNode = { ExpressionText: andOr, Expression: andOr, Type: 'Operator', Id: getAId() };
        if (node.Id == itemNodes[0]) {
            tree.addNode(newNode, "add", node);
        }
        else {
            tree.addNode(newNode, "after", node);
        }
    }

    function onAddAnd(e) {
        addAndOr("AND");
    }

    function onAddOr(e) {
        addAndOr("OR");
    }

    function onAddExpr() {
        var node = tree.getSelectedNode() || itemNodes[0];
        var newNode = { Id: getAId(), Type: 'Expr', ExpressionText: firstProp, Expression: firstProp, Operator: '=', Value: '', ValueText: '' };
        while (node.Type == 'Expr') {
            node = tree.getParentNode(node);
            if (!node) return;
        }
        tree.addNode(newNode, "add", node);
    }

    function advSearch() {
        updateItemNodes();
        $('.frame-search-grid, .frame-search-tree').each(function () {
            var gridId = this.id;
            var grid = mini.get("#" + gridId);
            grid.load({ AdvQuery: mini.encode(itemNodes), Type: tabAdvQuery.activeIndex });
        });
    }

    function hideAdvSearch() {
        var win = mini.get("winAdvSearch");
        win.hide();
    }

    function onRemoveNode(e) {
        var node = tree.getSelectedNode();

        if (node == itemNodes[0]) {
            showTips({ Type: 'error', Title: $.lang('Can\'t delete root node.') });
            return;
        }

        if (node) {
            if (confirm($.lang("Confirm delete checked nodes?"))) {
                tree.removeNode(node);
            }
        }
        else {
            showTips({ Type: 'warning', Title: $.lang('Plz select a node.') });
        }
    }

    function getCurrentQuery() {
        var row = cbxSavedQuery.getSelected();
        return row || {};
    }

    function saveData() {
        updateItemNodes();
        var item = getCurrentQuery();
        var name = prompt($.lang("plz input query name"), item.Name);
        if (!name) {
            return;
        }
        item. Nodes = mini.encode(itemNodes);
        item.Name =  name;
        item.ModelName = $('#modelName').val();

        //bootPATH是mui的一个全局变量在boot.js中定义的
        $.newPOST(bootPATH + "../AdvQuery/SaveQueryItem", item, function (data) {
            cbxSavedQuery.setUrl(cbxSavedQuery.url);
            cbxSavedQuery.setValue(data.ReturnValue);
        });
    }

    function deleteData() {
        var id = cbxSavedQuery.getValue();
        $.newPOST(bootPATH + "../AdvQuery/DeleteQueryItem", id, function (data) {
            cbxSavedQuery.setUrl(cbxSavedQuery.url);
        });
    }

    $.regButton({
        text: $.lang("Advanced Search"),
        noSubmit: function () {
            var winAdvSearch = mini.get("winAdvSearch");
            var pos = $('.frame-advquery').offset();

            var x = pos.left + $('.frame-advquery').width() - $('#winAdvSearch').width();
            var y = pos.top + $('.frame-advquery').height() + 5;
            winAdvSearch.show();
            $('#winAdvSearch').offset({ left: x, top: y });
            //winAdvSearch.showAtEl(el,{xAlign:'right', vAlign:'above'}); //must be lowercase
            //   winAdvSearch.showAtPos("right", "Top");
        }
    });
});
