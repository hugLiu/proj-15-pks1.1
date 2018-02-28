var bootstrapTableOptions = {
    showTitleTab: false,
    tableOption: {
        height: "400",
        maintainSelected: false,
        selectedRowIndex: 0,
        showFooter: false,
        pagination: false,
        smartDisplay: true,
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
    },
    isDateType: function (column) {
        if (column.type == "date" || column.type == "Date") return true;
        var field = column.field.toLowerCase();
        if (field.indexOf("date") >= 0) return true;
        if (field.indexOf("time") >= 0) return true;
        return false;
    },
    formatDate: function (index, column, table) {
        for (var i = 0; i < table.rows.length; i++) {
            var row = table.rows[i];
            var value = null;
            var rowIsArray = Array.isArray(row);
            if (rowIsArray) {
                value = row[index];
            } else {
                value = row[column.field];
            }
            if (value == null) continue;
            if (typeof value != "string") continue;
            if (value.length <= 10) continue;
            var value2 = value.substr(0, 10);
            if (rowIsArray) {
                row[index] = value2;
            } else {
                row[column.field] = value2;
            }
        }
    },
    formatNumber: function (index, column, table) {
        for (var i = 0; i < table.rows.length; i++) {
            var row = table.rows[i];
            var value = null;
            var rowIsArray = Array.isArray(row);
            if (rowIsArray) {
                value = row[index];
            } else {
                value = row[column.field];
            }
            if (value == null) continue;
            if (typeof value != "number") continue;
            var value2 = value % 1;
            if (value2 == 0) continue;
            value2 = value * 100;
            if (value2 % 1 == 0) continue;
            value2 = Math.round(value2) / 100;
            if (rowIsArray) {
                row[index] = value2;
            } else {
                row[column.field] = value2;
            }
        }
    },
    //生成合并选项
    buildMergeOptions: function (tables, index, sortFields, sortAndMerge, mergePrimaryField, mergeFields) {
        if (tables == null) return;
        if (!Array.isArray(tables)) return;
        if (index >= tables.length) return;
        this.buildMergeOption(tables[index], sortFields, sortAndMerge, mergePrimaryField, mergeFields);
    },
    //生成合并选项
    buildMergeOption: function (table, sortFields, sortAndMerge, mergeGroupField, mergeFields) {
        for (var i = 0; i < sortFields.length; i++) {
            var field = sortFields[i];
            var columns = table.columns.filter(function (column) {
                return column.field === field;
            });
            columns[0].sortOrder = i;
            if (sortAndMerge) {
                columns[0].merged = true;
                if (columns[0].field === mergeGroupField) columns[0].mergedGroup = true;
            }
        }
        for (var i = 0; i < mergeFields.length; i++) {
            var field = mergeFields[i];
            var columns = table.columns.filter(function (column) {
                return column.field === field;
            });
            columns[0].merged = true;
            if (columns[0].field === mergeGroupField) columns[0].mergedGroup = true;
        }
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
    tables.showTitleTab = options.showTitleTab;
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
            if (options.isDateType(column)) {
                options.formatDate(j, column, tableItem);
            } else {
                options.formatNumber(j, column, tableItem);
            }
            //column.height = 30;
            //column.cellStyle = setTableCellStyle;
        }
    }
    return tables;
};
