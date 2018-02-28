var bootstrapTableOptions = {
    tableOption: {
        pagination: false,
        height: "400",
        maintainSelected: false,
        selectedRowIndex: 0,
    },
    columnOption: {
        align: "left",
    },
    setTableCellStyle: function (value, row, index, field) {
        //return {
        //    classes: 'text-nowrap another-class',
        //    css: { "color": "blue", "font-size": "50px" }
        //};
        return null;
    }
}

var setBootstrapTableOptionsForIndexData = function (indexData) {
    if (indexData == null) return indexData;
    setBootstrapTableOptions(indexData.Content);
};

var setBootstrapTableOptions = function (tables) {
    if (tables == null) return tables;
    if (!Array.isArray(tables)) return tables;
    var options = bootstrapTableOptions;
    for (var i = 0; i < tables.length; i++) {
        var tableItem = tables[i];
        if (!tableItem.columns || !tableItem.rows) continue;
        tableItem.tableoption = options.tableOption;
        if (options.columnOption) {
            tableItem.columnoption = options.columnOption;
        }
        for (var j = 0; j < tableItem.columns.length; j++) {
            var column = tableItem.columns[j];
            delete column.align;
            delete column.valign;
            //column.height = 30;
            //column.cellStyle = setTableCellStyle;
        }
    }
    return tables;
};
