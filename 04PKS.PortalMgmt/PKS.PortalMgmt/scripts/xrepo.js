function XRepo(defaultKeyName) {
    this.add = {};
    this.delete = {};
    this.modify = {};
    this.defaultKeyName = defaultKeyName || "Id";
}

XRepo.prototype.getKeyName = function (keyName) {
    return keyName || this.defaultKeyName;
}

XRepo.prototype.new = function (obj, keyName) {
    keyName = this.getKeyName(keyName);
    var key = obj[keyName];
    var isDelete = this.delete[key]
    if (isDelete) {
        delete this.delete[key];
    } else {
        this.add[key] = obj;
    }
}
XRepo.prototype.remove = function (obj, keyName) {
    keyName = this.getKeyName(keyName);
    var key = obj[keyName];
    delete this.modify[key];
    var isAdd = this.add[key];
    if (isAdd) {
        delete this.add[key];
    } else {
        this.delete[key] = obj;
    }
}
XRepo.prototype.update = function (obj, keyName) {
    keyName = this.getKeyName(keyName);
    var key = obj[keyName];
    var isAdd = this.add[key];
    if (isAdd) {
        this.new[key] = obj;
    } else {
        this.modify[key] = obj;
    }
}
XRepo.prototype.getModel = function () {
    var model = {}
    model.add = this.toArray(this.add);
    model.delete = this.toArray(this.delete);
    model.modify = this.toArray(this.modify);
    return model;
}
XRepo.prototype.toArray = function (obj) {
    var array = [];
    for (var attr in obj) {
        if (obj.hasOwnProperty(attr))
            array.push(obj[attr]);
    }
    return array;
}
XRepo.prototype.clear = function () {
    this.add = {};
    this.delete = {};
    this.modify = {};
}

