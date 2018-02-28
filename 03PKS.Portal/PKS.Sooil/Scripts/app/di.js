var di= {
    container:{}
}
di.register=function(name, func) {
    di.container[name] = { "type": func };
};
di.resolve = function (name, isCreateNew) {
    if (di.container[name]) {
        if (isCreateNew)
            return new di.container[name].type();
        if (!di.container[name].value)
            di.container[name].value = new di.container[name].type();
        return di.container[name].value;
    }
    return null;
};

//defineLazyProperty兼容性问题
//var di = {
//    container: {}
//};

//di.service = function(name, Constructor) {
//    defineLazyProperty(name, () => new Constructor());
//};

//function defineLazyProperty(name, getter) {
//    Object.defineProperty(di.container,
//        name,
//        {
//            configurable: true,
//            get: function() {
//                var obj = getter(container);
//                Object.defineProperty(di.container,
//                    name,
//                    {
//                        configurable: false,
//                        value: obj
//                    });
//                return obj;
//            }
//        });
//}

//di.factory = function (name, factory) {
//    return defineLazyProperty(name, factory);
//};

//di.provider = function (name, Provider) {
//    return defineLazyProperty(name, function () {
//        var provider = new Provider();
//        return provider.$get();
//    });
//};

//di.value = function (name, val) {
//    return defineLazyProperty(name, () => val);
//};