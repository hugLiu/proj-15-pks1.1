'use strict';

var _get = function get(object, property, receiver) { if (object === null) object = Function.prototype; var desc = Object.getOwnPropertyDescriptor(object, property); if (desc === undefined) { var parent = Object.getPrototypeOf(object); if (parent === null) { return undefined; } else { return get(parent, property, receiver); } } else if ("value" in desc) { return desc.value; } else { var getter = desc.get; if (getter === undefined) { return undefined; } return getter.call(receiver); } };

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

/** 
 * author:xWander
 * email:2317791932@qq.com
 * startDate:2017-11-10
 * version:0.0.1.0
*/

/**
 * Web-Front Programing Principal 
 * 1 any field of object that is computed by other field is not a field,it is property;
 * 2 listen dom object event should always on doucment object
 */
//数据结构
var XMap = function () {
    _createClass(XMap, null, [{
        key: '_Create',
        value: function _Create() {
            return new XMap();
        }
    }]);

    function XMap() {
        _classCallCheck(this, XMap);

        this._keys_ = [];
        this._map_ = new Map();
    }

    _createClass(XMap, [{
        key: 'has',
        value: function has(key) {
            return this._map_.has(key);
        }
    }, {
        key: 'get',
        value: function get(key) {
            return this._map_.get(key);
        }
    }, {
        key: 'getFirstKey',
        value: function getFirstKey() {
            return this._keys_[0];
        }
    }, {
        key: 'getLastKey',
        value: function getLastKey() {
            return this._keys_[this._keys_.length - 1];
        }
    }, {
        key: 'getKeyBefore',
        value: function getKeyBefore(key) {
            var i = this.findKey(key);
            return this._keys_[--i];
        }
    }, {
        key: 'getKeyAfter',
        value: function getKeyAfter(key) {
            var i = this.findKey(key);
            return this._keys_[++i];
        }
    }, {
        key: 'getFirst',
        value: function getFirst() {
            var fKey = this.getFirstKey();
            return this._map_.get(fKey);
        }
    }, {
        key: 'getLast',
        value: function getLast() {
            var lKey = this.getLastKey();
            return this._map_.get(lKey);
        }
    }, {
        key: 'getBefore',
        value: function getBefore(key) {
            var bKey = this.getKeyBefore(key);
            return this._map_.get(bKey);
        }
    }, {
        key: 'getAfter',
        value: function getAfter(key) {
            var aKey = this.getKeyAfter(key);
            return this._map_.get(aKey);
        }
    }, {
        key: 'set',
        value: function set(key, value) {
            this._map_.set(key, value);
        }
    }, {
        key: 'add',
        value: function add(key, value) {
            if (this._map_.has(key)) this.__removeKey(key);
            this._keys_.push(key);
            this._map_.set(key, value);
        }
    }, {
        key: 'addBefore',
        value: function addBefore(key, value, bKey) {
            var m = this._map_;
            if (m.has(key)) this.__removeKey(key);
            var i = this.findKey(bKey);
            this._keys_.splice(i, 0, key);
            m.set(key, value);
        }
    }, {
        key: 'addAfter',
        value: function addAfter(key, value, aKey) {
            var m = this._map_;
            if (m.has(key)) this.__removeKey(key);
            var i = this.findKey(aKey);
            this._keys_.splice(++i, 0, key);
            this._map_.set(key, value);
        }
    }, {
        key: 'remove',
        value: function remove(key) {
            if (this._map_.has(key)) {
                this.__removeKey(key);
                this._map_.delete(key);
            }
        }
    }, {
        key: 'removeByVal',
        value: function removeByVal(obj) {
            var key = this.findValue(obj);
            this.remove(key);
        }
    }, {
        key: 'clear',
        value: function clear() {
            this._keys_ = [];
            this._map_.clear();
        }
    }, {
        key: 'keys',
        value: function keys() {
            return this._keys_;
        }
    }, {
        key: 'values',
        value: function values() {
            var values = [];
            var m = this._map_;
            this.keys().forEach(function (k) {
                return values.push(m.get(k));
            });
            return values;
        }
    }, {
        key: 'findKey',
        value: function findKey(key) {
            var keys = this._keys_;
            for (var i = keys.length - 1; i >= 0; i--) {
                if (key === keys[i]) return i;
            }
            throw new Error(key + ' not found');
        }
    }, {
        key: 'findValue',
        value: function findValue(obj) {
            var key = void 0;
            var m = this._map_;
            var _iteratorNormalCompletion = true;
            var _didIteratorError = false;
            var _iteratorError = undefined;

            try {
                for (var _iterator = m[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                    var iterator = _step.value;

                    if (obj === iterator[1]) {
                        key = iterator[0];
                        break;
                    }
                }
            } catch (err) {
                _didIteratorError = true;
                _iteratorError = err;
            } finally {
                try {
                    if (!_iteratorNormalCompletion && _iterator.return) {
                        _iterator.return();
                    }
                } finally {
                    if (_didIteratorError) {
                        throw _iteratorError;
                    }
                }
            }

            return key;
        }
    }, {
        key: '__removeKey',
        value: function __removeKey(key) {
            var i = this.findKey(key);
            this._keys_.splice(i, 1);
        }
    }]);

    return XMap;
}();


//事件的参数e
var XEventArgs = function () {
    _createClass(XEventArgs, null, [{
        key: '_CreateFromEvent$',
        value: function _CreateFromEvent$(e$, eventType) {
            return new XEventArgs(e$.originalEvent, eventType);
        }
    }, {
        key: '_CreateFrom',
        value: function _CreateFrom(xEventArgs, eventType) {
            return new XEventArgs(xEventArgs.event, eventType);
        }
    }, {
        key: '_CreateFromEvent',
        value: function _CreateFromEvent(event, eventType) {
            return new XEventArgs(event, eventType);
        }
    }]);

    function XEventArgs() {
        var event = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, XEventArgs);

        this._timestamp_ = new Date().valueOf();
        this._handled_ = false;
        this._path_ = [];
        this._event_ = event;
        this._type_ = type;
        this._data_ = new Map();
    }

    _createClass(XEventArgs, [{
        key: 'addPath',
        value: function addPath(source) {
            this.path.push(source);
        }
    }, {
        key: 'addData',
        value: function addData(key, value) {
            this._data_.add(key, value);
        }
    }, {
        key: 'removeData',
        value: function removeData(key) {
            this._data_.delete(key);
        }
    }, {
        key: 'getData',
        value: function getData(key) {
            this._data_.get(key);
        }
    }, {
        key: 'dispatch',
        value: function dispatch(source) {
            if (source.trigger && typeof source.trigger == 'function') {
                this.addPath(source);
                source.trigger(this.type, this);
            }
        }
    }, {
        key: 'dispose',
        value: function dispose() {
            this._path_ = [];
            this._data_.clear();
        }
    }, {
        key: 'handled',
        get: function get() {
            return this._handled_;
        },
        set: function set(b) {
            this._handled_ = b;
        }
    }, {
        key: 'path',
        get: function get() {
            return this._path_;
        }
    }, {
        key: 'type',
        get: function get() {
            return this._type_;
        },
        set: function set(type) {
            this._type_ = type;
        }
    }, {
        key: 'event',
        get: function get() {
            return this._event_;
        },
        set: function set(event) {
            this._event_ = event;
        }
    }, {
        key: 'data',
        get: function get() {
            return this._data_;
        },
        set: function set(data) {
            this._data_ = data;
        }
    }, {
        key: 'srcTarget',
        get: function get() {
            return this.path[0];
        }
    }, {
        key: 'curTarget',
        get: function get() {
            return this.path[this.path.length - 1];
        }
    }]);

    return XEventArgs;
}();


//定义事件时的回掉函数
var HandlerMetaData = function () {
    _createClass(HandlerMetaData, null, [{
        key: '_Create',
        value: function _Create() {
            var handler = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : function () {
                return void 0;
            };
            var context = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var isOnce = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : false;
            var isTriggered = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : false;

            if (typeof handler != 'function') throw new Error("handler不是函数");
            return new HandlerMetaData(handler, context, isOnce, isTriggered);
        }
    }]);

    function HandlerMetaData() {
        var handler = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : function () {
            return void 0;
        };
        var context = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : null;
        var isOnce = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : false;
        var isTriggered = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : false;

        _classCallCheck(this, HandlerMetaData);

        this._handler_ = handler;
        this._context_ = context;
        this._isOnce_ = isOnce;
        this._isTriggered_ = isTriggered;
    }

    _createClass(HandlerMetaData, [{
        key: 'invoke',
        value: function invoke(source, eventArgs) {
            if (!(this._isOnce_ && this._isTriggered_)) {
                if (!!this._context_) {
                    Reflect.apply(this._handler_, this._context_, [source, eventArgs]);
                } else {
                    this._handler_(source, eventArgs);
                }
                this._isTriggered_ = true;
            }
        }
    }, {
        key: 'hander',
        get: function get() {
            return this._handler_;
        },
        set: function set(handler) {
            if (typeof handler != 'function') throw new Error("handler不是函数");
            this._handler_ = handler;
        }
    }, {
        key: 'context',
        get: function get() {
            return this._context_;
        },
        set: function set(context) {
            this._context_ = context;
        }
    }, {
        key: 'isOnce',
        get: function get() {
            return this._isOnce_;
        },
        set: function set(isOnce) {
            this._isOnce_ = isOnce;
        }
    }, {
        key: 'isTriggered',
        get: function get() {
            return this._isTriggered_;
        },
        set: function set(isTriggered) {
            this._isTriggered_ = isTriggered;
        }
    }]);

    return HandlerMetaData;
}();


//事件
var XEvent = function () {
    _createClass(XEvent, null, [{
        key: '_Create',
        value: function _Create() {
            return new XEvent();
        }
    }]);

    function XEvent() {
        _classCallCheck(this, XEvent);

        this._timestamp_ = new Date().valueOf();
        this._eventNamespace_ = "xWander";
        this._handlerMetadataDic_ = new XMap();
    }

    _createClass(XEvent, [{
        key: 'on',
        value: function on(eventType, handler, context) {
            var handlerMetadata = HandlerMetaData._Create(handler, context);
            this.__addHandler(eventType, handlerMetadata);
        }
    }, {
        key: 'once',
        value: function once(eventType, handler, context) {
            var once = true;
            var triggered = false;
            var handlerMetadata = HandlerMetaData._Create(handler, context, once, triggered);
            this.__addHandler(eventType, handlerMetadata);
        }
    }, {
        key: 'trigger',
        value: function trigger(eventType, eventArgs) {
            var source = this;
            var handlersMetadatas = source._handlerMetadataDic_.get(eventType);
            if (handlersMetadatas) handlersMetadatas.forEach(function (handlerMetaData) {
                return handlerMetaData.invoke(source, eventArgs);
            });
        }
    }, {
        key: 'un',
        value: function un(eventType, handler) {
            var handlerMetadatas = this._handlerMetadataDic_.get(eventType);
            var handlerMetadata = void 0;
            for (var i = 0, j = handlerMetadatas.length; i < j; i++) {
                handlerMetadata = handlerMetadatas[i];
                if (handlerMetadata.hander === handler) {
                    handlerMetadatas.splice(i, 1);
                    break;
                }
            }
        }
    }, {
        key: 'remove',
        value: function remove() {
            this._handlerMetadataDic_.clear();
        }
    }, {
        key: '__addHandler',
        value: function __addHandler(eventType, handlerMetadata) {
            var handlerMetadataDic = this._handlerMetadataDic_;
            if (handlerMetadataDic.has(eventType)) {
                handlerMetadataDic.get(eventType).push(handlerMetadata);
            } else {
                handlerMetadataDic.add(eventType, [handlerMetadata]);
            }
        }
    }, {
        key: 'timestamp',
        get: function get() {
            return this._timestamp_;
        }
    }, {
        key: 'eventNamespace',
        get: function get() {
            return this._eventNamespace_;
        }
    }]);

    return XEvent;
}();

var XObject = function (_XEvent) {
    _inherits(XObject, _XEvent);

    _createClass(XObject, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Is',
        value: function _Is(obj, cls) {
            var itr = obj;
            do {
                if (itr instanceof cls) return true;
            } while (itr = itr.prototype);
            return false;
        }
    }, {
        key: '_Guid',
        value: function _Guid() {
            var guid = "";
            for (var i = 0; i < 32; i++) {
                guid += Math.floor(Math.random() * 16.0).toString(16);
                if (i == 8 || i == 12 || i == 16 || i == 20) guid += '-';
            }
            return guid;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var id = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var name = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

            return new XObject(id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "XObject";
        }
    }]);

    function XObject() {
        var id = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var name = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, XObject);

        var _this = _possibleConstructorReturn(this, (XObject.__proto__ || Object.getPrototypeOf(XObject)).call(this));

        _this._instanceId_ = _this.constructor.__InstanceId() || 0;

        _this._id_ = id;
        _this._name_ = name;
        return _this;
    }

    _createClass(XObject, [{
        key: 'createNew',
        value: function createNew() {
            return new this.constructor();
        }
    }, {
        key: 'equals',
        value: function equals(xObj) {
            var eq = false;
            if (xObj.id && this.id == xObj.id) eq = true;
            if (xObj.instanceId && this.instanceId == xObj.instanceId) eq = true;
            return eq;
        }
    }, {
        key: 'is',
        value: function is(cls) {
            return XObject._Is(this, cls);
        }
    }, {
        key: 'clone',
        value: function clone(obj) {
            return JSON.parse(JSON.stringify(obj));
        }
    }, {
        key: 'className',
        get: function get() {
            return this.constructor.__ClassName_ || 'Object';
        }
    }, {
        key: 'instanceId',
        get: function get() {
            return this._instanceId_;
        }
    }, {
        key: 'id',
        get: function get() {
            return this._id_ || (this._id_ = XObject._Guid());
        },
        set: function set(id) {
            this._id_ = id;
        }
    }, {
        key: 'name',
        get: function get() {
            return this._name_ || (this._name_ = '' + this.className + this.instanceId);
        },
        set: function set(name) {
            this._name_ = name;
        }
    }]);

    return XObject;
}(XEvent);
//组件
var Component = function (_XObject) {
    _inherits(Component, _XObject);

    _createClass(Component, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var enviroment = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
            var id = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var name = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';

            return new Component(enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Component";
        }
    }, {
        key: '__Version',
        get: function get() {
            return "0.5.300.88888";
        }
    }]);

    function Component() {
        var enviroment = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
        var id = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var name = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';

        _classCallCheck(this, Component);

        var _this2 = _possibleConstructorReturn(this, (Component.__proto__ || Object.getPrototypeOf(Component)).call(this, id, name));

        _this2._enviroment_ = enviroment;
        return _this2;
    }

    _createClass(Component, [{
        key: 'version',
        get: function get() {
            return Component.__Version;
        }
    }, {
        key: 'enviroment',
        get: function get() {
            return this._enviroment_;
        },
        set: function set(enviroment) {
            this._enviroment_ = enviroment;
        }
    }]);

    return Component;
}(XObject);
//节点
var Node = function (_Component) {
    _inherits(Node, _Component);

    _createClass(Node, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var parent = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
            var pre = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var next = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var enviroment = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : {};
            var id = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : '';
            var name = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';

            return new Node(parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Node";
        }
    }]);

    function Node() {
        var parent = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
        var pre = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var next = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var enviroment = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : {};
        var id = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : '';
        var name = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';

        _classCallCheck(this, Node);

        var _this3 = _possibleConstructorReturn(this, (Node.__proto__ || Object.getPrototypeOf(Node)).call(this, enviroment, id, name));

        _this3._children_ = new XMap();

        _this3._parent_ = parent;
        _this3._pre_ = pre;
        _this3._next_ = next;
        return _this3;
    }

    _createClass(Node, [{
        key: 'hasChild',
        value: function hasChild(node) {
            return this.hasChild_(node.id);
        }
    }, {
        key: 'hasChild_',
        value: function hasChild_(key) {
            return this._children_.has(key);
        }
    }, {
        key: 'getChild',
        value: function getChild(key) {
            return this._children_.get(key);
        }
    }, {
        key: 'addToChildren',
        value: function addToChildren(key, node) {
            this._children_.add(key, node);
        }
    }, {
        key: 'addToChildren_',
        value: function addToChildren_(node) {
            this._children_.add(node.id, node);
        }
    }, {
        key: 'setParent',
        value: function setParent(node) {
            this._parent_ = node;
        }
    }, {
        key: 'setPre',
        value: function setPre(node) {
            this._pre_ = node;
        }
    }, {
        key: 'setNext',
        value: function setNext(node) {
            this._next_ = node;
        }
    }, {
        key: 'addChild',
        value: function addChild(node) {
            this.addChild_(node.id, node);
        }
    }, {
        key: 'addChild_',
        value: function addChild_(key, node) {
            var lNode = this.lastChild;
            node.pre = lNode;
            this.addToChildren(key, node);
            node.setParent(this);
        }
    }, {
        key: 'addChildBefore',
        value: function addChildBefore(node, bNode) {
            this.addChildBefore_(node.id, node, bNode.id);
        }
    }, {
        key: 'addChildBefore_',
        value: function addChildBefore_(key, node, bKey) {
            var cs = this._children_;
            if (cs.has(bKey)) {
                var pre = cs.getBefore(bKey);
                if (pre) pre.next = node;
                var next = cs.get(bKey);
                if (next) next.pre = node;
                cs.addBefore(key, node, bKey);
                node.setParent(this);
            }
        }
    }, {
        key: 'addChildAfter',
        value: function addChildAfter(node, aNode) {
            this.addChildAfter_(node.id, node, aNode.id);
        }
    }, {
        key: 'addChildAfter_',
        value: function addChildAfter_(key, node, aKey) {
            var cs = this._children_;
            if (cs.has(aKey)) {
                var pre = cs.get(aKey);
                if (pre) pre.next = node;
                var next = cs.getAfter(aKey);
                if (next) next.pre = node;
                cs.addAfter(key, node, aKey);
                node.setParent(this);
            }
        }
    }, {
        key: 'removeChild',
        value: function removeChild(node) {
            this.removeChild_(node.id);
        }
    }, {
        key: 'removeChild_',
        value: function removeChild_(key) {
            var cNode = this._children_.get(key);
            if (cNode) cNode.remove();
        }
    }, {
        key: 'removeFromChildren',
        value: function removeFromChildren(node) {
            this.removeFromChildren_(node.id);
        }
    }, {
        key: 'removeFromChildren_',
        value: function removeFromChildren_(key) {
            this._children_.remove(key);
        }
    }, {
        key: 'remove',
        value: function remove() {
            _get(Node.prototype.__proto__ || Object.getPrototypeOf(Node.prototype), 'remove', this).call(this);
            this._children_.clear();
            if (this._parent_) this._parent_.removeFromChildren(this);
            if (this._pre_ && this._next_) {
                this._pre_.next = this._next_;
            } else {
                this._pre_ && (this._pre_.next = void 0);
                this._next_ && (this._next_.pre = void 0);
            }
            this._parent_ = void 0;
            this._pre_ = void 0;
            this._next_ = void 0;
        }
    }, {
        key: 'findAncestor',
        value: function findAncestor() {
            var node = this;
            while (node.parent) {
                node = node.parent;
            }
            return node;
        }
    }, {
        key: 'isAncesstor',
        value: function isAncesstor() {
            return !node.parent;
        }
    }, {
        key: 'parent',
        get: function get() {
            return this._parent_;
        },
        set: function set(node) {
            node && node.addChild(this);
        }
    }, {
        key: 'pre',
        get: function get() {
            return this._pre_;
        },
        set: function set(node) {
            this._pre_ = node;
            node && node.setNext(this);
        }
    }, {
        key: 'next',
        get: function get() {
            return this._next_;
        },
        set: function set(node) {
            this._next_ = node;
            node && node.setPre(this);
        }
    }, {
        key: 'children',
        get: function get() {
            return this._children_.values();
        }
    }, {
        key: 'firstChild',
        get: function get() {
            return this._children_.getFirst();
        }
    }, {
        key: 'lastChild',
        get: function get() {
            return this._children_.getLast();
        }
    }]);

    return Node;
}(Component);
//元素
var UIElement = function (_Node) {
    _inherits(UIElement, _Node);

    _createClass(UIElement, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_IsHtmlElement',
        value: function _IsHtmlElement(obj) {
            return XObject._Is(obj, HTMLElement);
        }
    }, {
        key: '_ToCssId',
        value: function _ToCssId(elementId) {
            var jId = elementId;
            if (!UIElement._IsCssId(elementId)) jId = '#' + jId;
            return jId;
        }
    }, {
        key: '_IsCssId',
        value: function _IsCssId(elementId) {
            return UIElement._IsCssIdCommon(elementId, function (s) {
                return (/^#\w+/g.test(s)
                );
            });
        }
    }, {
        key: '_IsCssIdCommon',
        value: function _IsCssIdCommon(elementId, predicate) {
            if (!UIElement._IsPredicate(predicate)) throw new TypeError("请传入一个合法的谓词");
            return predicate(elementId);
        }
    }, {
        key: '_IsPredicate',
        value: function _IsPredicate(predicate) {
            return typeof predicate === 'function';
        }
    }, {
        key: '_JoinClassName',
        value: function _JoinClassName(classArray) {
            return classArray.join(" ");
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var contextmenu = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
            var parent = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var pre = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var next = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var enviroment = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : {};
            var id = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';
            var name = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';

            return new UIElement(parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "UIElement";
        }
    }, {
        key: '__DomParser_',
        get: function get() {
            if (!this.domParser) {
                this.domParser = new DOMParser();
            }
            return this.domParser;
        }
    }]);

    function UIElement() {
        var contextmenu = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
        var parent = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var pre = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var next = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var enviroment = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : {};
        var id = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';
        var name = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';

        _classCallCheck(this, UIElement);

        var _this4 = _possibleConstructorReturn(this, (UIElement.__proto__ || Object.getPrototypeOf(UIElement)).call(this, parent, pre, next, enviroment, id, name));

        _this4._contextmenu_ = contextmenu;
        return _this4;
    }

    _createClass(UIElement, [{
        key: 'byId',
        value: function byId(id) {
            return this.document.getElementById(id);
        }
    }, {
        key: 'mount',
        value: function mount(elementId) {
            var ele = this.byId(elementId);
            if (ele) {
                if (this.isInDocument) this.destroyView();
                var view = this.getView();
                ele.innerHTML = view;
                this.initialize();
            }
        }
    }, {
        key: 'addChild_',
        value: function addChild_(key, ele) {
            _get(UIElement.prototype.__proto__ || Object.getPrototypeOf(UIElement.prototype), 'addChild_', this).call(this, key, ele);
            this.mountChild_(ele);
        }
    }, {
        key: 'mountChild_',
        value: function mountChild_(ele) {
            if (this.isInDocument) {
                var v = ele.getView();
                this.view$.insertAdjacentHTML('beforeend', v);
                ele.initialize();
            }
        }
    }, {
        key: 'addChildBefore_',
        value: function addChildBefore_(key, ele, bKey) {
            _get(UIElement.prototype.__proto__ || Object.getPrototypeOf(UIElement.prototype), 'addChildBefore_', this).call(this, key, ele, bKey);
            this.mountChildBefore_(key, ele, bKey);
        }
    }, {
        key: 'mountChildBefore_',
        value: function mountChildBefore_(key, ele, bKey) {
            if (this.isInDocument && this.hasChild_(bKey)) {
                var bEle = this.getChild(bKey);
                var v = ele.getView();
                bEle.view$.insertAdjacentHTML('beforebegin', v);
                ele.initialize();
            }
        }
    }, {
        key: 'destroyView',
        value: function destroyView() {
            var v = this.view$;
            v.parentNode.removeChild(v);
        }
    }, {
        key: 'remove',
        value: function remove() {
            _get(UIElement.prototype.__proto__ || Object.getPrototypeOf(UIElement.prototype), 'remove', this).call(this);
            this.destroyView();
        }
    }, {
        key: 'isChildView',
        value: function isChildView(cview) {
            return this.isChildView_(this.view$, cview);
        }
    }, {
        key: 'isChildView_',
        value: function isChildView_(view, cview) {
            var b = false;
            if (!view || !cview) return b;
            if (view === cview) {
                return b;
            }
            var p = cview.parentNode;
            while (p) {
                if (p === view) {
                    b = true;
                    break;
                }
                if (p.tagName === "BODY") break;
                p = p.parentNode;
            }
            return b;
        }
    }, {
        key: 'on$',
        value: function on$(eventType, id, handler) {
            var te = this.byId(id);
            if (te) {
                te.addEventListener(eventType, handler, false);
            }
            return this;
        }
    }, {
        key: 'joinClassName',
        value: function joinClassName(classArray) {
            return UIElement._JoinClassName(classArray);
        }
    }, {
        key: 'toCssId',
        value: function toCssId(elementId) {
            return UIElement._ToCssId(elementId);
        }
    }, {
        key: 'routeEvent',
        value: function routeEvent(xEventArgs) {
            var cur = this;
            do {
                xEventArgs.dispatch(cur);
                cur = cur.parent;
            } while (cur && !xEventArgs.handled);
        }
    }, {
        key: 'routeEventX',
        value: function routeEventX(eventType) {
            var xEventArgs = new XEventArgs();
            xEventArgs.type = eventType;
            this.routeEvent(xEventArgs);
        }
    }, {
        key: 'showContextmenu',
        value: function showContextmenu(pageX, pageY) {
            this.showContextmenu_(this, pageX, pageY);
        }
    }, {
        key: 'showContextmenu_',
        value: function showContextmenu_(target, pageX, pageY) {
            this.contextmenu && this.contextmenu.show && this.contextmenu.show(target, pageX, pageY);
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-margin-ver-md xvision-border" style="position:relative">';
            v += this.getSelfView();
            v += this.getChildView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getSelfView',
        value: function getSelfView() {
            return this.name;
        }
    }, {
        key: 'getChildView',
        value: function getChildView() {
            var v = '';
            this.children.forEach(function (ele) {
                return v += ele.getView();
            });
            return v;
        }
    }, {
        key: 'onClick_',
        value: function onClick_(ele$) {
            var self = this;
            this.on$("click", ele$, function (e) {
                self.handleClick_(e);
            });
        }
    }, {
        key: 'handleClick_',
        value: function handleClick_(e) {
            this.dispatchEvent(e, this.clickETN);
        }
    }, {
        key: 'dispatchEvent',
        value: function dispatchEvent(e, eventType) {
            e.stopPropagation();
            var xEventArgs = XEventArgs._CreateFromEvent(e, eventType);
            this.routeEvent(xEventArgs);
        }
    }, {
        key: 'onContextmenu_',
        value: function onContextmenu_(ele$) {
            var self = this;
            this.on$("contextmenu", ele$, function (e) {
                self.handleContextmenu_(e);
            });
        }
    }, {
        key: 'handleContextmenu_',
        value: function handleContextmenu_(e) { }
    }, {
        key: 'findChildNearestAfterPixel',
        value: function findChildNearestAfterPixel(ox, oy, diff, view) {
            var v = view || this.view$;
            var sx = v.scrollLeft;
            var sy = v.scrollTop;
            diff = diff || 15;
            var eles = this.children;
            var ele = void 0;
            for (var i = 0, j = eles.length; i < j; i++) {
                ele = eles[i];
                if (this.isBeforePixel(ele.view$, ox, oy, diff, sx, sy)) return ele;
            }
            return void 0;
        }
    }, {
        key: 'isBeforePixel',
        value: function isBeforePixel(ele, ox, oy, diff, sx, sy) {
            var oTop = ele.offsetTop;
            var oLeft = ele.offsetLeft;
            var oWidth = ele.offsetWidth;
            var y = oTop - sy - oy;
            return y >= 0 && y <= diff && ox >= diff + oLeft - sx && ox <= diff + oWidth - sx;
        }
    }, {
        key: 'addClass',
        value: function addClass(view, className) {
            if (view && view.classList) {
                view.classList.add(className);
            }
        }
    }, {
        key: 'removeClass',
        value: function removeClass(view, className) {
            if (view && view.classList) {
                view.classList.remove(className);
            }
        }
    }, {
        key: 'hasClass',
        value: function hasClass(view, className) {
            var bl = false;
            if (view && view.classList) {
                bl = view.classList.contains(className);
            }
            return bl;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            this.initializeC();
            //this.__onClick();
            //this.__onContextmenu();
        }
    }, {
        key: 'initializeC',
        value: function initializeC() {
            this.children.forEach(function (ele) {
                return ele.initialize();
            });
        }
    }, {
        key: '__onClick',
        value: function __onClick() {
            this.onClick_(this.viewId);
        }
    }, {
        key: '__onContextmenu',
        value: function __onContextmenu() {
            this.onContextmenu_(this.viewId);
        }
    }, {
        key: 'document',
        get: function get() {
            return window.document;
        }
    }, {
        key: 'domParser',
        get: function get() {
            return UIElement.__DomParser_;
        }
    }, {
        key: 'viewId',
        get: function get() {
            return 'v__' + this.id;
        }
    }, {
        key: 'view$',
        get: function get() {
            return this.byId(this.viewId);
        }
    }, {
        key: 'isInDocument',
        get: function get() {
            return !!this.view$;
        }
    }, {
        key: 'clickETN',
        get: function get() {
            return 'click.' + this.eventNamespace;
        }
    }, {
        key: 'contextmenuETN',
        get: function get() {
            return 'contextmenu.' + this.eventNamespace;
        }
    }, {
        key: 'contextmenu',
        get: function get() {
            return this._contextmenu_;
        },
        set: function set(contextmenu) {
            this._contextmenu_ = contextmenu;
        }
    }]);

    return UIElement;
}(Node);
//组建的查询数据模型
var EsComModel = function () {
    _createClass(EsComModel, null, [{
        key: '_FromEsModel',
        value: function _FromEsModel(esm) {
            esm = esm || {};
            return new EsComModel(esm.query, esm.sort, esm.size);
        }
    }]);

    function EsComModel(query, sort, size) {
        _classCallCheck(this, EsComModel);

        this._query_ = this.initQuery(query);

        this._sort_ = this.initSort(sort);

        this._size_ = this.initSize(size);
    }

    _createClass(EsComModel, [{
        key: 'initQuery',
        value: function initQuery(query) {
            var q = {};
            var o = void 0;
            for (var n in query) {
                o = query[n];
                q[n] = this.gQuery(n, Object.values(o)[0], Object.keys(o)[0]);
            }
            return q;
        }
    }, {
        key: 'gQuery',
        value: function gQuery(name, value, op) {
            return { name: name, value: value, op: op };
        }
    }, {
        key: 'initSort',
        value: function initSort(sort) {
            return JSON.parse(JSON.stringify(sort || {}));
        }
    }, {
        key: 'initSize',
        value: function initSize(size) {
            return size || '';
        }
    }, {
        key: 'setQuery',
        value: function setQuery(name, value, op) {
            if (this.hasQuery(name)) {
                this.modifyQuery_(name, value, op);
            } else {
                this.addQuery_(name, value, op);
            }
        }
    }, {
        key: 'hasQuery',
        value: function hasQuery(name) {
            return !!this._query_[name];
        }
    }, {
        key: 'modifyQuery_',
        value: function modifyQuery_(name, value, op) {
            var q = this._query_[name];
            value && (q.value = value);
            op && (q.op = op);
        }
    }, {
        key: 'addQuery_',
        value: function addQuery_(name, value, op) {
            this._query_[name] = { name: name, value: value, op: op };
        }
    }, {
        key: 'removeQuery',
        value: function removeQuery(name) {
            delete this._query_[name];
        }
    }, {
        key: 'setSort',
        value: function setSort(name, value) {
            this._sort_[name] = value;
        }
    }, {
        key: 'removeSort',
        value: function removeSort(name) {
            delete this._sort_[name];
        }
    }, {
        key: 'setSize',
        value: function setSize(sz) {
            this._size_ = sz;
        }
    }, {
        key: 'toEsModel',
        value: function toEsModel() {
            var q = this.toEsQuery();
            var s = this._sort_;
            var sz = this._size_;
            return { query: q, sort: s, size: sz };
        }
    }, {
        key: 'toEsQuery',
        value: function toEsQuery() {
            var q = Object.values(this._query_);
            var m = {};
            var o = void 0,
                om = void 0;
            for (var i = 0, j = q.length; i < j; i++) {
                om = q[i];
                o = {};
                o[om.op] = om.value;
                m[om.name] = o;
            }
            return m;
        }
    }, {
        key: 'query',
        get: function get() {
            return this._query_;
        }
    }, {
        key: 'sort',
        get: function get() {
            return this._sort_;
        }
    }, {
        key: 'size',
        get: function get() {
            return this._size_;
        },
        set: function set(s) {
            this._size_ = s;
        }
    }]);

    return EsComModel;
}();
//文档编辑器
var ComEditor = function (_UIElement) {
    _inherits(ComEditor, _UIElement);

    _createClass(ComEditor, null, [{
        key: '_Create',
        value: function _Create(esMeta) {
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new ComEditor(esMeta, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }]);

    function ComEditor(esMeta) {
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, ComEditor);

        var _this5 = _possibleConstructorReturn(this, (ComEditor.__proto__ || Object.getPrototypeOf(ComEditor)).call(this, contextmenu, parent, pre, next, enviroment, id, name));

        _this5._esMeta_ = esMeta;

        _this5._com_ = void 0;

        _this5._title_ = void 0;

        _this5._argModel_ = void 0;

        _this5._queryModel_ = void 0;

        _this5._htmlText_ = void 0;

        _this5._htmlEditor_ = void 0;
        return _this5;
    }

    _createClass(ComEditor, [{
        key: 'show',
        value: function show(com) {
            this.com = com;
            this.show_();
            this.init();
        }
    }, {
        key: 'show_',
        value: function show_() {
            if (!this.isInDocument) {
                $("body").append(this.getView());
                this.initialize();
            }
        }
    }, {
        key: 'init',
        value: function init() {
            this.initQuery();
            this.initSort();
            this.initReturn();
            if (this.com.metadata.hastexttemplate) {
                this.initHtmlEditor();
            }
        }
    }, {
        key: 'initQuery',
        value: function initQuery() {
            var q = this.queryModel.query;
            var o = void 0,
                name = void 0,
                title = void 0,
                op = void 0,
                value = void 0;
            for (var key in q) {
                o = q[key];
                name = key;
                title = this.getEsMetaByName(name).title;
                op = o.op;
                value = o.value;
                this.addQueryRow(name, title, op, value);
            }
        }
    }, {
        key: 'initSort',
        value: function initSort() {
            var s = this.queryModel.sort;
            var name = void 0,
                title = void 0,
                value = void 0;
            for (var key in s) {
                name = key;
                title = this.getEsMetaByName(name).title;
                value = s[key];
                this.addSortRow(name, title, value);
                this.removeSortSelect(name);
            }
        }
    }, {
        key: 'removeSortSelect',
        value: function removeSortSelect(name) {
            var opt = this.sortTable$.querySelector('tr:last-child>td:first-child>select>option[value="' + name + '"]');
            opt.parentNode.removeChild(opt);
        }
    }, {
        key: 'initReturn',
        value: function initReturn() {
            this.size$.value = this.queryModel.size;
        }
    }, {
        key: 'initHtmlEditor',
        value: function initHtmlEditor() {
            var self = this;
            this._htmlEditor_ = UE.getEditor(this.htmlEditorId);
            this._htmlEditor_.ready(function () {
                self._htmlEditor_.setContent(self._htmlText_ || "");
            });
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-com-editor">';
            v += '<div class="xvision-window">';
            v += '<div class="xvision-com-editor-core">';
            v += this.getHeader();
            v += this.getBody();
            v += this.getFooter();
            v += '</div>';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div class="xvision-com-editor-core-header">';
            v += '<div class="xvision-com-editor-core-header-container">';
            v += '<div class="xvision-row">';
            v += '<div class="xvision-col-6">';
            v += '设置文本属性';
            v += '</div>';
            v += '<div class="xvision-col-6 xvision-text-right">';
            v += '<i id="' + this.hideId + '" class="xvision-tool-btn glyphicon glyphicon-remove"></i>';
            v += '</div>';
            v += '</div>';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<div class="xvision-com-editor-core-body">';
            v += '<div class="xvision-com-editor-core-body-container">';
            v += this.getCaption();
            if (this.com.metadata.hastexttemplate) {
                v += this.getText();
            }
            v += this.getParams();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getCaption',
        value: function getCaption() {
            var v = '<div class="xvision-panel">';
            v += '<div class="xvision-panel-header">';
            v += '<h3>\u6807\u9898</h3>';
            v += '</div>';
            v += '<div class="xvision-panel-body">';
            v += '<input id="' + this.titleId + '" type="text" value="' + this.title + '"/>';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getText',
        value: function getText() {
            var v = '<div class="xvision-panel">';
            v += '<div class="xvision-panel-header">';
            v += '<h3>\u7F16\u8F91\u6587\u672C</h3>';
            v += '</div>';
            v += '<div class="xvision-panel-body">';
            v += this.getTextEditor();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getTextEditor',
        value: function getTextEditor() {
            return '<script id="' + this.htmlEditorId + '" type="text/plain" style="width:100%;height:160px;"></script>';
        }
    }, {
        key: 'getParams',
        value: function getParams() {
            var v = '<div class="xvision-panel">';
            v += '<div class="xvision-panel-header">';
            v += '<h3>\u53C2\u6570\u8BBE\u7F6E</h3>';
            v += '</div>';
            v += '<div class="xvision-panel-body">';
            v += this.getTab();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getTab',
        value: function getTab() {
            var v = '<div class="xvision-tab">';
            v += this.getTabHeader();
            v += this.getTabBody();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getTabHeader',
        value: function getTabHeader() {
            var v = '<div class="xvision-tab-header">';
            v += '<div class="xvision-tab-btn active">\u67E5\u8BE2\u53C2\u6570</div>';
            v += '<div class="xvision-tab-btn">\u7EC4\u4EF6\u53C2\u6570</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getTabBody',
        value: function getTabBody() {
            var v = '<div class="xvision-tab-body">';
            v += this.getQueryParmasTP();
            v += this.getArgsParamsTP();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getQueryParmasTP',
        value: function getQueryParmasTP() {
            var v = '<div class="xvision-tab-page active" data-anchor="查询参数">';
            v += this.getQueryParmas();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getArgsParamsTP',
        value: function getArgsParamsTP() {
            var v = '<div class="xvision-tab-page" data-anchor="组件参数">';
            v += this.getArgsParams();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getQueryParmas',
        value: function getQueryParmas() {
            var v = this.getQuery();
            v += this.getSort();
            return v;
        }
    }, {
        key: 'getQuery',
        value: function getQuery() {
            var v = '<div class="xvision-panel">';
            v += '<div class="xvision-panel-header">';
            v += '<h3>查询</h3>';
            v += '</div>';
            v += '<div class="xvision-panel-body">';
            v += this.getQueryTable();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getSort',
        value: function getSort() {
            var v = '<div class="xvision-panel">';
            v += '<div class="xvision-panel-header">';
            v += '<h3>排序</h3>';
            v += '</div>';
            v += '<div class="xvision-panel-body">';
            v += this.getSortTable();
            v += this.getReturnRecord();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getQueryTable',
        value: function getQueryTable() {
            var v = '<table id="' + this.queryTableId + '" class="xvision-table query">';
            v += '<thead><th>\u53C2\u6570\u540D</th><th>\u7C7B\u578B</th><th>\u64CD\u4F5C\u7B26</th><th>\u503C</th><th></th></thead>';
            v += '<tbody>';
            v += this.getQueryMetaRow();
            v += '</tbody>';
            v += '</table>';
            return v;
        }
    }, {
        key: 'getSortTable',
        value: function getSortTable() {
            var v = '<table id="' + this.sortTableId + '" class="xvision-table sort">';
            v += '<thead><th>\u53C2\u6570\u540D</th><th>\u6392\u5E8F</th><th></th></thead>';
            v += '<tbody>';
            v += this.getSortMetaRow();
            v += '</tbody>';
            v += '</table>';
            return v;
        }
    }, {
        key: 'getQueryMetaRow',
        value: function getQueryMetaRow() {
            var v = '<tr>';
            v += '<td>' + this.getQueryOpt() + '</td>';
            v += '<td>' + '</td>';
            v += '<td>' + '</td>';
            v += '<td>' + '</td>';
            v += '<td>' + '</td>';
            v += '</tr>';
            return v;
        }
    }, {
        key: 'getSortMetaRow',
        value: function getSortMetaRow() {
            var v = '<tr>';
            v += '<td>' + this.getQueryOpt() + '</td>';
            v += '<td>' + '</td>';
            v += '<td>' + '</td>';
            v += '</tr>';
            return v;
        }
    }, {
        key: 'addQueryRow',
        value: function addQueryRow(name, title, op, value) {
            var self = this;
            var v = this.cosQueryRow(name, title, op, value);
            var tb = this.queryTable$.querySelector("tbody");
            tb.insertAdjacentHTML("afterbegin", v);
            var ov = tb.querySelector("tr:first-child");
            var dio = ov.querySelector(".xvision-radio");
            dio && (dio.onclick = function (e) {
                self.handleQueryRadio_(e);
            });
            var sel = ov.querySelector("select");
            sel && (sel.onchange = function (e) {
                self.handleQueryOp_(e);
            });
            var ipt = ov.querySelector("input[type=text]");
            ipt && (ipt.onblur = function (e) {
                self.handleQueryValue_(e);
            });
            var rm = ov.querySelector(".xvision-com-editor-rm");
            rm && (rm.onclick = function (e) {
                self.handleRemoveQuery(e);
            });
        }
    }, {
        key: 'handleQueryRadio_',
        value: function handleQueryRadio_(e) {
            var tar = e.target;
            var text = tar.nextSibling.data;
            var row = e.currentTarget.parentNode.parentNode;
            var v = row.querySelector("input[type=text]");
            if (text == "变量") {
                v.value = '@' + row.getAttribute("name");
                v.disabled = true;
            } else {
                v.disabled = false;
            }
        }
    }, {
        key: 'handleQueryOp_',
        value: function handleQueryOp_(e) {
            var tar = e.currentTarget;
            var name = tar.parentNode.parentNode.getAttribute("name");
            var v = tar.value;
            this.queryModel.setQuery(name, null, v);
        }
    }, {
        key: 'handleQueryValue_',
        value: function handleQueryValue_(e) {
            var tar = e.currentTarget;
            var name = tar.parentNode.parentNode.getAttribute("name");
            var v = tar.value;
            this.queryModel.setQuery(name, v);
        }
    }, {
        key: 'handleRemoveQuery',
        value: function handleRemoveQuery(e) {
            var row = e.currentTarget.parentNode.parentNode;
            var name = row.getAttribute("name");
            this.queryModel.removeQuery(name);
            row.parentNode.removeChild(row);
        }
    }, {
        key: 'cosQueryRow',
        value: function cosQueryRow(name, title, op, value) {
            var v = '<tr name="' + name + '">';
            v += '<td>' + title + '</td>';
            v += '<td>' + this.getRadio(name + '.type', ["常量", "变量"], /^@\w/g.test(value) ? 1 : 0) + '</td>';
            v += '<td class="query-op">' + this.getOperator(op) + '</td>';
            v += '<td>' + ('<input type="text" value="' + value + '"/>') + '</td>';
            v += '<td>' + this.getRmButton() + '</td>';
            v += '</tr>';
            return v;
        }
    }, {
        key: 'addSortRow',
        value: function addSortRow(name, title, value) {
            var self = this;
            var v = this.cosSortRow(name, title, value);
            var tb = this.sortTable$.querySelector("tbody");
            tb.insertAdjacentHTML('afterbegin', v);
            var ov = tb.querySelector("tr:first-child");
            var dio = ov.querySelector(".xvision-radio");
            dio && (dio.onclick = function (e) {
                self.handleSort(e);
            });
            var rm = ov.querySelector(".xvision-com-editor-rm");
            rm && (rm.onclick = function (e) {
                self.handleRemoveSort(e);
            });
            this.removeSortSelect(name);
        }
    }, {
        key: 'handleSort',
        value: function handleSort(e) {
            var txt = e.target.nextSibling.data;
            var name = e.currentTarget.parentNode.parentNode.getAttribute("name");
            this.queryModel.setSort(name, txt == '升序' ? "asc" : "dsc");
        }
    }, {
        key: 'handleRemoveSort',
        value: function handleRemoveSort(e) {
            var row = e.currentTarget.parentNode.parentNode;
            var name = row.getAttribute("name");
            this.queryModel.removeSort(name);
            var tb = row.parentNode;
            var sel = tb.querySelector("tr:last-child select");
            var esm = this.getEsMetaByName(name);
            var v = '<option value="' + esm.name + '">' + esm.title + '</option>';
            sel.insertAdjacentHTML('beforeend', v);
            row.parentNode.removeChild(row);
        }
    }, {
        key: 'cosSortRow',
        value: function cosSortRow(name, title, value) {
            var v = '<tr name="' + name + '">';
            v += '<td>' + title + '</td>';
            v += '<td>' + this.getRadio(name + '.type', ["升序", "降序"], value == 'asc' ? 0 : 1) + '</td>';
            v += '<td>' + this.getRmButton() + '</td>';
            v += '</tr>';
            return v;
        }
    }, {
        key: 'getQueryOpt',
        value: function getQueryOpt() {
            var qs = Object.values(this.esMeta);
            var kvArray = [];
            qs.forEach(function (q) {
                return kvArray.push({ key: q.name, value: q.title });
            });
            return this.getSelect("选择参数", kvArray);
        }
    }, {
        key: 'getReturnRecord',
        value: function getReturnRecord() {
            var v = '<div>';
            v += '<span>返回记录数</span>';
            v += '<input id="' + this.sizeId + '" type="text" />';
            v += '<span>(为空返回所有数据)</span>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getArgsParams',
        value: function getArgsParams() {
            var v = '<table id="' + this.argTableId + '" class="xvision-table arg">';
            v += '<thead><th>\u540D\u79F0</th><th>\u503C</th></thead>';
            v += '<tbody>';
            v += this.getArgMetas();
            v += '</tbody>';
            v += '</table>';
            return v;
        }
    }, {
        key: 'getArgMetas',
        value: function getArgMetas() {
            var self = this;
            var arms = this.argModel;
            var v = '',
                o = void 0;
            for (var key in arms) {
                o = arms[key];
                v += this.addArgRow(o.code, o.name, o.value || o.defaultvalue || '');
            }
            return v;
        }
    }, {
        key: 'addArgRow',
        value: function addArgRow(code, name, value) {
            var v = '<tr name="' + code + '">';
            v += '<td>' + name + '</td>';
            v += '<td><input type="text" value="' + value + '" /></td>';
            v += '</tr>';
            return v;
        }
    }, {
        key: 'getFooter',
        value: function getFooter() {
            var v = '<div class="xvision-com-editor-core-footer">';
            v += '<div class="xvision-com-editor-core-footer-container">';
            v += '<button id="' + this.saveId + '">\u4FDD\u5B58</button>';
            v += '<button id="' + this.cancelId + '">\u53D6\u6D88</button>';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getRadio',
        value: function getRadio(name, values) {
            var def = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : 0;

            var v = '<span class="xvision-radio">';
            if (def == 0) {
                v += '<span><input type="radio" name="' + name + '" checked="true"/>' + values[0] + '</span>';
                v += '<span><input type="radio" name="' + name + '" />' + values[1] + '</span>';
            } else {
                v += '<span><input type="radio" name="' + name + '" />' + values[0] + '</span>';
                v += '<span><input type="radio" name="' + name + '" checked="true"/>' + values[1] + '</span>';
            }
            v += '</span>';
            return v;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            this.__onTitleChange();
            this.__onRetSizeChange();
            this.__onAddQueryRow();
            this.__onAddSortRow();
            this.__onTab();
            this.__onCancel();
            this.__onSave();
            this.__onHide();
            this.__onArgEdit();
        }
    }, {
        key: '__onTitleChange',
        value: function __onTitleChange() {
            var self = this;
            this.on$('blur', this.titleId, function (e) {
                self.handleTitleChange_(e);
            });
        }
    }, {
        key: '__onRetSizeChange',
        value: function __onRetSizeChange() {
            var self = this;
            this.on$('blur', this.sizeId, function (e) {
                self.handleRetChange_(e);
            });
        }
    }, {
        key: '__onAddQueryRow',
        value: function __onAddQueryRow() {
            var self = this;
            var opt = this.queryTable$.querySelector("tr:last-child select");
            if (opt) {
                opt.onchange = function (e) {
                    var tar = e.currentTarget;
                    var name = tar.value;
                    var esm = self.getEsMetaByName(name);
                    self.addQuery(esm);
                };
            }
        }
    }, {
        key: '__onAddSortRow',
        value: function __onAddSortRow() {
            var self = this;
            var opt = this.sortTable$.querySelector("tr:last-child select");
            if (opt) {
                opt.onchange = function (e) {
                    var tar = e.currentTarget;
                    var name = tar.value;
                    var esm = self.getEsMetaByName(name);
                    self.addSort(esm);
                };
            }
        }
    }, {
        key: '__onTab',
        value: function __onTab() {
            var self = this;
            var tBtns = this.view$.querySelectorAll(".xvision-tab-btn");
            var i = 0,
                j = tBtns.length,
                tBtn = void 0;
            for (; i < j; i++) {
                tBtn = tBtns[i];
                tBtn.addEventListener("click", function (e) {
                    self.handleTab_(e);
                }, false);
            }
        }
    }, {
        key: '__onCancel',
        value: function __onCancel() {
            var self = this;
            this.on$("click", this.cancelId, function (e) {
                self.handleCancel_(e);
            });
        }
    }, {
        key: '__onSave',
        value: function __onSave() {
            var self = this;
            this.on$("click", this.saveId, function (e) {
                self.handleSave_(e);
            });
        }
    }, {
        key: '__onHide',
        value: function __onHide() {
            var self = this;
            this.on$("click", this.hideId, function (e) {
                self.handleCancel_(e);
            });
        }
    }, {
        key: '__onArgEdit',
        value: function __onArgEdit() {
            var self = this;
            var ipts = this.argTable$.querySelectorAll("tr td:last-child input[type=text]");
            var i = 0,
                j = ipts.length,
                ipt = void 0;
            for (; i < j; i++) {
                ipt = ipts[i];
                ipt.onblur = function (e) {
                    self.handleArgEdit_(e);
                };
            }
        }
    }, {
        key: 'handleTitleChange_',
        value: function handleTitleChange_(e) {
            this._title_ = e.currentTarget.value;
            if (this.com.isHeadline())
                this.setArgValue("text", this._title_);
        }
    }, {
        key: 'addQuery',
        value: function addQuery(esm) {
            var name = esm.name;
            var title = esm.title;
            var op = '选择操作符';
            var value = "";
            this.queryModel.setQuery(name, value, op);
            this.addQueryRow(name, title, op, value);
        }
    }, {
        key: 'addSort',
        value: function addSort(ems) {
            var name = ems.name;
            var title = ems.title;
            var value = "升序";
            this.queryModel.setSort(name, value);
            this.addSortRow(name, title, value);
        }
    }, {
        key: 'handleOp_',
        value: function handleOp_(e$) {
            var r$ = $(e$.currentTarget).closest("tr");
            var name = r$.attr("name");
            var op = $(e$.currentTarget).val();
            this.queryModel.setQuery(name, null, op);
        }
    }, {
        key: 'handleRetChange_',
        value: function handleRetChange_(e) {
            this.queryModel.size = e.currentTarget.value;
        }
    }, {
        key: 'handleCancel_',
        value: function handleCancel_(e) {
            if (this.com.metadata.hastexttemplate) {
                this._htmlEditor_.destroy();
            }
            this.destroyView();
        }
    }, {
        key: 'handleSave_',
        value: function handleSave_(e) {
            this.com.text = this.title;
            this.com.query = this.queryModel.toEsModel();
            if (this.com.metadata.hastexttemplate) {
                this.com.htmlText = this.htmlText = this._htmlEditor_.getContent();
                this.com.vueData.data = [this.htmlText];
                this._htmlEditor_.destroy();
            }
            if (this.com.isHeadline()) {
                this.com.vueData.text = this.title;
            } else {
                this.com.caption = this.title;
            }
            var xEventArgs = new XEventArgs(null, 'save.' + this.eventNamespace);
            this.routeEvent(xEventArgs);
            this.destroyView();
        }
    }, {
        key: 'handleTab_',
        value: function handleTab_(e) {
            var tar = e.currentTarget;
            var anchor = tar.innerHTML;
            if (!tar.classList.contains("active")) {
                var tab = tar.parentNode.parentNode;
                var tp = tab.querySelector('.xvision-tab-page[data-anchor="' + anchor + '"]');
                tp.classList.add("active");
                var n = tp;
                while (n = n.previousSibling) {
                    n.classList.remove("active");
                }
                n = tp;
                while (n = n.nextSibling) {
                    n.classList.remove("active");
                }
                tar.classList.add("active");
                n = tar;
                while (n = n.previousSibling) {
                    n.classList.remove("active");
                }
                n = tar;
                while (n = n.nextSibling) {
                    n.classList.remove("active");
                }
            }
        }
    }, {
        key: 'handleArgEdit_',
        value: function handleArgEdit_(e) {
            var name = e.currentTarget.parentNode.parentNode.getAttribute("name");
            var v = e.currentTarget.value;
            this.setArgValue(name, v);
        }
    }, {
        key: 'setArgValue',
        value: function setArgValue(name, value) {
            var a = void 0;
            var am = this.argModel;
            for (var i = am.length - 1; i >= 0; i--) {
                a = am[i];
                if (a.code == name) {
                    a.value = value;
                    break;
                }
            }
        }
    }, {
        key: 'getEsMetaByName',
        value: function getEsMetaByName(name) {
            var em = this.esMeta;
            for (var i = em.length - 1; i >= 0; i--) {
                if (em[i].name == name) return em[i];
            }
        }
    }, {
        key: 'getOperator',
        value: function getOperator() {
            var sel = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "eq";

            var kvArray = [{ key: "eq", value: "等于" }, { key: "contains", value: "包含" }, { key: "in", value: "任意值" }, { key: "ge", value: "大于" }, { key: "gte", value: "大于等于" }, { key: "lt", value: "小于" }, { key: "lte", value: "小于等于" }, { key: "ne", value: "不等于" }];
            return this.getSelect("选择操作符", kvArray, sel);
        }
    }, {
        key: 'getSelect',
        value: function getSelect(hint, kvArray, selKey) {
            var opt = '';
            var v = '<select>';
            v += '<option>' + hint + '</option>';
            kvArray.forEach(function (kv) {
                if (kv.key == selKey) {
                    opt = '<option value="' + kv.key + '" selected="selected" >' + kv.value + '</option>';
                } else {
                    opt = '<option value="' + kv.key + '">' + kv.value + '</option>';
                }
                v += opt;
            });
            v += '</select>';
            return v;
        }
    }, {
        key: 'getRmButton',
        value: function getRmButton() {
            var v = '<button class="xvision-com-editor-rm">删除</button>';
            return v;
        }
    }, {
        key: 'com',
        get: function get() {
            return this._com_;
        },
        set: function set(com) {
            this._com_ = com;
            this._title_ = com.text;
            this._argModel_ = com.args;
            this._queryModel_ = EsComModel._FromEsModel(com.query);
            this._htmlText_ = com.htmlText;
        }
    }, {
        key: 'esMeta',
        get: function get() {
            return this._esMeta_;
        },
        set: function set(esMeta) {
            this._esMeta_ = esMeta;
        }
    }, {
        key: 'title',
        get: function get() {
            return this._title_;
        }
    }, {
        key: 'argModel',
        get: function get() {
            return this._argModel_;
        }
    }, {
        key: 'queryModel',
        get: function get() {
            return this._queryModel_;
        }
    }, {
        key: 'saveId',
        get: function get() {
            return 'sav__' + this.id;
        }
    }, {
        key: 'save$',
        get: function get() {
            return this.byId(this.saveId);
        }
    }, {
        key: 'cancelId',
        get: function get() {
            return 'cel__' + this.id;
        }
    }, {
        key: 'cancel$',
        get: function get() {
            return this.byId(this.cancelId);
        }
    }, {
        key: 'queryTableId',
        get: function get() {
            return 'qt__' + this.id;
        }
    }, {
        key: 'queryTable$',
        get: function get() {
            return this.byId(this.queryTableId);
        }
    }, {
        key: 'sortTableId',
        get: function get() {
            return 'st__' + this.id;
        }
    }, {
        key: 'sortTable$',
        get: function get() {
            return this.byId(this.sortTableId);
        }
    }, {
        key: 'sizeId',
        get: function get() {
            return 'sz__' + this.id;
        }
    }, {
        key: 'size$',
        get: function get() {
            return this.byId(this.sizeId);
        }
    }, {
        key: 'titleId',
        get: function get() {
            return 'ttl__' + this.id;
        }
    }, {
        key: 'title$',
        get: function get() {
            return this.byId(this.titleId);
        }
    }, {
        key: 'htmlEditorId',
        get: function get() {
            return 'her__' + this.id;
        }
    }, {
        key: 'hideId',
        get: function get() {
            return 'hie__' + this.id;
        }
    }, {
        key: 'hide$',
        get: function get() {
            return this.byId(this.hideId);
        }
    }, {
        key: 'argTableId',
        get: function get() {
            return 'arg__' + this.id;
        }
    }, {
        key: 'argTable$',
        get: function get() {
            return this.byId(this.argTableId);
        }
    }]);

    return ComEditor;
}(UIElement);
//上下文菜单项
var ContextmenuItem = function (_UIElement2) {
    _inherits(ContextmenuItem, _UIElement2);

    function ContextmenuItem() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var command = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : function (c) {
            return void 0;
        };
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, ContextmenuItem);

        var _this6 = _possibleConstructorReturn(this, (ContextmenuItem.__proto__ || Object.getPrototypeOf(ContextmenuItem)).call(this, parent, pre, next, enviroment, id, name));

        _this6._text_ = text;
        _this6._command_ = command;
        return _this6;
    }

    _createClass(ContextmenuItem, [{
        key: 'initialize',
        value: function initialize() {
            this.__onClick();
        }
    }, {
        key: 'handleClick_',
        value: function handleClick_(e$) {
            _get(ContextmenuItem.prototype.__proto__ || Object.getPrototypeOf(ContextmenuItem.prototype), 'handleClick_', this).call(this, e$);
        }
    }, {
        key: 'execute',
        value: function execute(args) {
            this.command(args);
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-contextmenu-item">';
            v += this.text;
            v += '</div>';
            return v;
        }
    }, {
        key: 'text',
        get: function get() {
            return this._text_;
        },
        set: function set(text) {
            this._text_ = text;
        }
    }, {
        key: 'command',
        get: function get() {
            return this._command_;
        },
        set: function set(command) {
            this._command_ = command;
        }
    }]);

    return ContextmenuItem;
}(UIElement);
//上下文菜单
var Contextmenu = function (_UIElement3) {
    _inherits(Contextmenu, _UIElement3);

    _createClass(Contextmenu, null, [{
        key: '_CreateD',
        value: function _CreateD() {
            var ref = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
            var parent = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var pre = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var next = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var enviroment = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : {};
            var id = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';
            var name = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';

            var addCommand = new ContextmenuItem("增加", function (c) {
                return c.addNode();
            });
            var modifyCommand = new ContextmenuItem("修改", function (c) {
                return c.modify();
            });
            var removeCommand = new ContextmenuItem("删除", function (c) {
                return c.remove();
            });
            var cm = new Contextmenu(ref, parent, pre, next, enviroment, id, name);
            cm.addChild(addCommand);
            cm.addChild(modifyCommand);
            cm.addChild(removeCommand);
            return cm;
        }
    }]);

    function Contextmenu() {
        var ref = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
        var parent = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var pre = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var next = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var enviroment = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : {};
        var id = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';
        var name = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';

        _classCallCheck(this, Contextmenu);

        var _this7 = _possibleConstructorReturn(this, (Contextmenu.__proto__ || Object.getPrototypeOf(Contextmenu)).call(this, parent, pre, next, enviroment, id, name));

        _this7._ref_ = ref;

        _this7.__onHide();
        _this7.__onExecuteCommad();
        return _this7;
    }

    _createClass(Contextmenu, [{
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-contextmenu">';
            v += this.getChildView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'show',
        value: function show(ref, pageX, pageY) {
            this.ref = ref;
            this.show_(pageX, pageY);
        }
    }, {
        key: 'show_',
        value: function show_(pageX, pageY) {
            if (!this.isInDocument) {
                var body = this.document.body;
                body.insertAdjacentHTML('beforeend', this.getView());
                this.initialize();
            }
            this.view$.style.left = pageX + 'px';
            this.view$.style.top = pageY + 'px';
            this.view$.style.display = 'block';
        }
    }, {
        key: 'hide',
        value: function hide() {
            this.view$ && (this.view$.style.display = "none");
        }
    }, {
        key: '__onHide',
        value: function __onHide() {
            var self = this;
            this.document.addEventListener("click", function (e) {
                self.__handleHide(e);
            }, false);
        }
    }, {
        key: '__handleHide',
        value: function __handleHide(e) {
            var tar = e.target;
            if (!this.isChildView(tar)) {
                this.hide();
            }
        }
    }, {
        key: '__onExecuteCommad',
        value: function __onExecuteCommad() {
            var self = this;
            this.on("click.xWander", function (source, xEventArgs) {
                self.handleExecuteCommand_(xEventArgs);
            });
        }
    }, {
        key: 'handleExecuteCommand_',
        value: function handleExecuteCommand_(xEventArgs) {
            this.hide();
            xEventArgs.srcTarget && xEventArgs.srcTarget.execute(this.ref);
        }
    }, {
        key: 'ref',
        get: function get() {
            return this._ref_;
        },
        set: function set(ref) {
            this._ref_ = ref;
        }
    }]);

    return Contextmenu;
}(UIElement);
//控件
var Control = function (_UIElement4) {
    _inherits(Control, _UIElement4);

    _createClass(Control, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!Control.hasOwnProperty("__instanceId__")) Control.__instanceId__ = 0;
            return ++Control.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "";
            var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
            var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
            var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

            return new Control(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Control";
        }
    }]);

    function Control() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "";
        var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
        var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
        var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

        _classCallCheck(this, Control);

        var _this8 = _possibleConstructorReturn(this, (Control.__proto__ || Object.getPrototypeOf(Control)).call(this, contextmenu, parent, pre, next, enviroment, id, name));

        _this8._caption_ = caption || _this8.name;
        _this8._text_ = text || _this8.name;
        _this8._catalog_ = catalog;
        _this8._contextmenu_ = contextmenu;
        return _this8;
    }

    _createClass(Control, [{
        key: 'mountChild_',
        value: function mountChild_(control) {
            if (this.isInDocument) {
                var v = control.getView();
                this.container$.insertAdjacentHTML('beforeend', v);
                control.initialize();
            }
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            return '';
        }
    }, {
        key: 'getChildModel',
        value: function getChildModel() {
            var v = '';
            this.children.forEach(function (c) {
                return v += c.getModel();
            });
            return v;
        }
    }, {
        key: 'getJModel',
        value: function getJModel() {
            return null;
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-border xvision-margin-hor xvision-margin-ver-xg">';
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div id="' + this.nodeHeaderId + '" class="xvision-row" style="background-color:#f5f5f5">';
            v += '<div class="xvision-col-6 xvision-padding-sm">';
            v += this.getCaptionView();
            v += '</div>';
            v += '<div class="xvision-col-6 xvision-padding-sm">';
            v += this.getToolView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getCaptionView',
        value: function getCaptionView() {
            var v = '<span id="' + this.captionId + '">' + this.caption + '</span>';
            return v;
        }
    }, {
        key: 'getToolView',
        value: function getToolView() {
            var v = '<div class="xvision-text-right">';
            v += this.getRmToolView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getRmToolView',
        value: function getRmToolView() {
            var v = '<i id="' + this.rmId + '" class="xvision-tool-btn xvision-margin-md glyphicon glyphicon-remove"></i>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<div id="' + this.containerId + '" style="position:relative">';
            v += this.getChildView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getIconView',
        value: function getIconView() {
            var v = '<div id="' + this.iconId + '" class="xvision-icon" draggable="true">';
            v += this.caption;
            v += '</div>';
            return v;
        }
    }, {
        key: 'findChildNearestAfterPixel',
        value: function findChildNearestAfterPixel(ox, oy, diff) {
            var r = void 0;
            if (this.isInDocument) r = _get(Control.prototype.__proto__ || Object.getPrototypeOf(Control.prototype), 'findChildNearestAfterPixel', this).call(this, ox, oy, diff, this.container$);
            return r;
        }
    }, {
        key: 'getDragImage',
        value: function getDragImage() {
            return this.icon$;
        }
    }, {
        key: 'canDrop',
        value: function canDrop(control) {
            return false;
        }
    }, {
        key: 'select',
        value: function select() {
            this.addClass(this.view$, "selected");
        }
    }, {
        key: 'cancelSelect',
        value: function cancelSelect() {
            this.removeClass(this.view$, "selected");
        }
    }, {
        key: 'selectContainer',
        value: function selectContainer() {
            this.addClass(this.container$, "selected");
        }
    }, {
        key: 'cancelContainerSelected',
        value: function cancelContainerSelected() {
            this.removeClass(this.container$, "selected");
        }
    }, {
        key: 'preAdd',
        value: function preAdd() {
            var holder = this.byId(this.holderId);
            if (!holder) {
                var tml = '<div id="' + this.holderId + '" class="xvision-control-holder"></div>';
                this.container$.insertAdjacentHTML('beforeend', tml);
            }
        }
    }, {
        key: 'preAddBefore',
        value: function preAddBefore(ele) {
            var holder = this.byId(this.holderId);
            if (!holder) {
                var tml = '<div id="' + this.holderId + '" class="xvision-control-holder"></div>';
                ele.insertAdjacentHTML('beforebegin', tml);
            }
        }
    }, {
        key: 'cancelPreAdd',
        value: function cancelPreAdd() {
            if (this.isInDocument) {
                var holder = this.byId(this.holderId);
                holder.parentNode.removeChild(holder);
            }
        }
    }, {
        key: 'onDragStart_',
        value: function onDragStart_(ele$) {
            var self = this;
            this.on$("dragstart", ele$, function (e) {
                self.handleDragStart_(e);
            }, this.icon$);
        }
    }, {
        key: 'handleDragStart_',
        value: function handleDragStart_(e) {
            this.dispatchEvent(e, this.dragStartETN);
        }
    }, {
        key: 'onDragEnter_',
        value: function onDragEnter_(ele$) {
            var self = this;
            this.on$("dragenter", ele$, function (e) {
                self.handleDragEnter_(e);
            });
        }
    }, {
        key: 'handleDragEnter_',
        value: function handleDragEnter_(e) {
            this.dispatchEvent(e, this.dragEnterETN);
        }
    }, {
        key: 'onDragLeave_',
        value: function onDragLeave_(ele$) {
            var self = this;
            this.on$("dragleave", ele$, function (e) {
                self.handleDragLeave_(e);
            });
        }
    }, {
        key: 'handleDragLeave_',
        value: function handleDragLeave_(e) {
            this.dispatchEvent(e, this.dragLeaveETN);
        }
    }, {
        key: 'onDragOver_',
        value: function onDragOver_(ele$) {
            var self = this;
            this.on$("dragover", ele$, function (e) {
                self.handleDragOver_(e);
            });
        }
    }, {
        key: 'handleDragOver_',
        value: function handleDragOver_(e) {
            this.dispatchEvent(e, this.dragOverETN);
        }
    }, {
        key: 'onDrop_',
        value: function onDrop_(ele$) {
            var self = this;
            this.on$("drop", ele$, function (e) {
                self.handleDrop_(e);
            });
        }
    }, {
        key: 'handleDrop_',
        value: function handleDrop_(e) {
            this.dispatchEvent(e, this.dropETN);
        }
    }, {
        key: 'onContainerClick_',
        value: function onContainerClick_(ele$) {
            var self = this;
            this.on$("click", ele$, function (e) {
                self.handleContainerClick_(e);
            });
        }
    }, {
        key: 'handleContainerClick_',
        value: function handleContainerClick_(e) {
            this.dispatchEvent(e, this.containerClickETN);
        }
    }, {
        key: 'onRemove_',
        value: function onRemove_(ele$) {
            var self = this;
            this.on$("click", ele$, function (e) {
                self.handleRemove_(e);
            });
        }
    }, {
        key: 'handleRemove_',
        value: function handleRemove_(e) {
            this.dispatchEvent(e, this.removeETN);
        }
    }, {
        key: 'initializeIcon',
        value: function initializeIcon() {
            this.__onDragStart();
        }
    }, {
        key: '__onDragStart',
        value: function __onDragStart() {
            this.onDragStart_(this.iconId);
        }
    }, {
        key: '__onDragEnter',
        value: function __onDragEnter() {
            this.onDragEnter_(this.containerId);
        }
    }, {
        key: '__onDragOver',
        value: function __onDragOver() {
            this.onDragOver_(this.containerId);
        }
    }, {
        key: '__onDragLeave',
        value: function __onDragLeave() {
            this.onDragLeave_(this.containerId);
        }
    }, {
        key: '__onDrop',
        value: function __onDrop() {
            this.onDrop_(this.containerId);
        }
    }, {
        key: '__onContainerClick',
        value: function __onContainerClick() {
            this.onContainerClick_(this.containerId);
        }
    }, {
        key: '__onRemove',
        value: function __onRemove() {
            this.onRemove_(this.rmId);
        }
    }, {
        key: 'nodeHeaderId',
        get: function get() {
            return 'ndr__' + this.id;
        }
    }, {
        key: 'nodeHeader$',
        get: function get() {
            return this.byId(this.nodeHeaderId);
        }
    }, {
        key: 'caption',
        get: function get() {
            return this._caption_;
        },
        set: function set(caption) {
            this._caption_ = caption;
            this.isInDocument && (this.caption$.innerHTML = caption);
        }
    }, {
        key: 'captionId',
        get: function get() {
            return 'cap__' + this.id;
        }
    }, {
        key: 'caption$',
        get: function get() {
            return this.byId(this.captionId);
        }
    }, {
        key: 'text',
        get: function get() {
            return this._text_;
        },
        set: function set(text) {
            this._text_ = text;
            this.isInDocument && this.text$ && (this.text$.textContent = text);
        }
    }, {
        key: 'textId',
        get: function get() {
            return 'txt__' + this.id;
        }
    }, {
        key: 'text$',
        get: function get() {
            return this.byId(this.textId);
        }
    }, {
        key: 'catalog',
        get: function get() {
            return this._catalog_;
        },
        set: function set(catalog) {
            this._catalog_ = catalog;
        }
    }, {
        key: 'containerId',
        get: function get() {
            return 'c__' + this.id;
        }
    }, {
        key: 'container$',
        get: function get() {
            return this.byId(this.containerId);
        }
    }, {
        key: 'iconId',
        get: function get() {
            return 'ico__' + _get(Control.prototype.__proto__ || Object.getPrototypeOf(Control.prototype), 'id', this);
        }
    }, {
        key: 'icon$',
        get: function get() {
            return this.byId(this.iconId);
        }
    }, {
        key: 'rmId',
        get: function get() {
            return 'rm__' + this.id;
        }
    }, {
        key: 'rm$',
        get: function get() {
            return this.byId(this.rmId);
        }
    }, {
        key: 'holderId',
        get: function get() {
            return 'hor__' + this.id;
        }
    }, {
        key: 'holder$',
        get: function get() {
            return this.byId(this.holderId);
        }
    }, {
        key: 'dragStartETN',
        get: function get() {
            return 'dragStart.' + this.eventNamespace;
        }
    }, {
        key: 'dragEnterETN',
        get: function get() {
            return 'dragEnter.' + this.eventNamespace;
        }
    }, {
        key: 'dragLeaveETN',
        get: function get() {
            return 'dragLeave.' + this.eventNamespace;
        }
    }, {
        key: 'dragOverETN',
        get: function get() {
            return 'dragOver.' + this.eventNamespace;
        }
    }, {
        key: 'dropETN',
        get: function get() {
            return 'drop.' + this.eventNamespace;
        }
    }, {
        key: 'containerClickETN',
        get: function get() {
            return 'containerClick.' + this.eventNamespace;
        }
    }, {
        key: 'removeETN',
        get: function get() {
            return 'remove.' + this.eventNamespace;
        }
    }]);

    return Control;
}(UIElement);
//容器
var Container = function (_Control) {
    _inherits(Container, _Control);

    _createClass(Container, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!Container.hasOwnProperty("__instanceId__")) Container.__instanceId__ = 0;
            return ++Container.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "";
            var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
            var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
            var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

            return new Container(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Container";
        }
    }]);

    function Container() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : "";
        var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
        var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
        var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

        _classCallCheck(this, Container);

        return _possibleConstructorReturn(this, (Container.__proto__ || Object.getPrototypeOf(Container)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Container, [{
        key: 'initialize',
        value: function initialize() {
            _get(Container.prototype.__proto__ || Object.getPrototypeOf(Container.prototype), 'initialize', this).call(this);
            this.__onDragOver();
            this.__onDrop();
        }
    }, {
        key: 'findParentHeadlineId',
        value: function findParentHeadlineId() {
            var p = this;
            while (p = p.parent) {
                if (p.is(knowLayout) && p.headlineId) return p.headlineId;
            }
            return void 0;
        }
    }]);

    return Container;
}(Control);
//用户控件
var UserControl = function (_Control2) {
    _inherits(UserControl, _Control2);

    _createClass(UserControl, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
            var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
            var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
            var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

            return new UserControl(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "UserControl";
        }
    }]);

    function UserControl() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
        var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
        var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

        _classCallCheck(this, UserControl);

        return _possibleConstructorReturn(this, (UserControl.__proto__ || Object.getPrototypeOf(UserControl)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(UserControl, [{
        key: 'initialize',
        value: function initialize() {
            this.__onRemove();
        }
    }]);
   
    return UserControl;
}(Control);
//vue控件
var KnowVue = function (_UserControl) {
    _inherits(KnowVue, _UserControl);

    _createClass(KnowVue, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var metadata = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
            var query = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var args = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var caption = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
            var text = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : '';
            var catalog = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';
            var contextmenu = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var parent = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
            var pre = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : void 0;
            var next = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : void 0;
            var enviroment = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : {};
            var id = arguments.length > 11 && arguments[11] !== undefined ? arguments[11] : '';
            var name = arguments.length > 12 && arguments[12] !== undefined ? arguments[12] : '';

            return new KnowVue(metadata, query, args, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "KnowVue";
        }
    }]);

    function KnowVue(metadata) {
        var query = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var args = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var caption = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
        var text = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : '';
        var catalog = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : '';
        var contextmenu = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var parent = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
        var pre = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : void 0;
        var next = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : void 0;
        var enviroment = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : {};
        var id = arguments.length > 11 && arguments[11] !== undefined ? arguments[11] : '';
        var name = arguments.length > 12 && arguments[12] !== undefined ? arguments[12] : '';

        _classCallCheck(this, KnowVue);

        var _this11 = _possibleConstructorReturn(this, (KnowVue.__proto__ || Object.getPrototypeOf(KnowVue)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));

        _this11._editETN_ = 'edit.' + _this11.eventNamespace;

        _this11._metadata_ = metadata;

        _this11._query_ = query;

        _this11._args_ = _this11.parseArgs(args);

        _this11._vueData_ = _this11.parseVueData(_this11.args);

        _this11._categoryId_ = void 0;

        _this11._htmlText_ = void 0;
        return _this11;
    }

    _createClass(KnowVue, [{
        key: 'isHeadline',
        value: function isHeadline() {
            return (/^headline/.test(this._metadata_.code)
            );
        }
    }, {
        key: 'getDocLev',
        value: function getDocLev() {
            var l = -1;
            if (this.isHeadline()) {
                l = this._metadata_.code.replace(/^headline/, "");
                l = parseInt(l);
            }
            return l;
        }
    }, {
        key: 'parseArgs',
        value: function parseArgs(args) {
            var a = args;
            if (!a) a = this.clone(this.metadata.comparams);
            return a;
        }
    }, {
        key: 'parseVueParams',
        value: function parseVueParams(args) {
            var res = [];
            var arg = void 0;
            for (var i = args.length - 1; i >= 0; i--) {
                arg = args[i];
                res.push(':' + arg.code + '=' + arg.code);
            }
            return " " + res.join(" ") + " ";
        }
    }, {
        key: 'parseVueData',
        value: function parseVueData(args) {
            var res = {};
            var arg = void 0;
            for (var i = args.length - 1; i >= 0; i--) {
                arg = args[i];
                switch (arg.datatype) {
                    case "Number":
                        res[arg.code] = parseFloat(arg.value || arg.defaultvalue);
                        break;
                    default:
                        res[arg.code] = arg.value || arg.defaultvalue || null;
                        break;
                }
            }
            return res;
        }
    }, {
        key: 'createNew',
        value: function createNew() {
            return new this.constructor(this.metadata, null, null, this.caption, this.text, this.catalog);
        }
    }, {
        key: 'remove',
        value: function remove() {
            var p = this.parent;
            _get(KnowVue.prototype.__proto__ || Object.getPrototypeOf(KnowVue.prototype), 'remove', this).call(this);
            if (this.isHeadline() && p.is(knowLayout)) p.reHeadline();
        }
    }, {
        key: 'getJModel',
        value: function getJModel(templateId) {
            var m = {
                templateid: templateId,
                templatecatalogueid: 0,
                cataloguenodeid: this.categoryId || "",
                title: this.text,
                queryparameter: JSON.stringify(this.query),
                componentparameter: JSON.stringify(this.args),
                htmltext: this.htmlText,
                fragmenttypeid: this.metadata.id,
                placeholderid: this.id,
                id: 0
            };
            return m;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var v = '<placeholder id="' + this.id + '" type="' + this.metadata.code + '" data-class="UserControl"></placeholder>';
            return v;
        }
    }, {
        key: 'getCaptionView',
        value: function getCaptionView() {
            if (this.isHeadline())
                return "";
            else
                return '<span id="' + this.captionId + '">' + this.caption + '</span>';
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<div id="' + this.vueId + '" class="xvision-row">';
            
            v += this.getVueView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getVueView',
        value: function getVueView() {
            var meta = this.metadata;
            var v = '<pks:' + meta.vuetag + ' ' + this.parseVueParams(this.args) + '></pks:' + meta.vuetag + '>';
            return v;
        }
    }, {
        key: 'getToolView',
        value: function getToolView() {
            var v = '<div class="xvision-text-right">';
            v += this.getEditToolView();
            v += this.getRmToolView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getEditToolView',
        value: function getEditToolView() {
            var v = '<i id="' + this.editId + '" class="xvision-tool-btn xvision-margin-md glyphicon glyphicon-edit"></i>';
            return v;
        }
    }, {
        key: 'getIconView',
        value: function getIconView() {
            let v = `<div id="${this.iconId}" class="xvision-icon" draggable="true" title="${this.metadata.name}">`;
            v += `<img src="${this.metadata.imageurl}" />`;
            v += '</div>';
            return v;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            _get(KnowVue.prototype.__proto__ || Object.getPrototypeOf(KnowVue.prototype), 'initialize', this).call(this);
            this.__onEdit();
            this.initVue();
        }
    }, {
        key: 'initVue',
        value: function initVue() {
            window.PKSUI.bind({
                el: '#' + this.vueId,
                data: this.vueData,
                model: ['pks:' + this.metadata.vuetag]
            });
        }
    }, {
        key: '__onEdit',
        value: function __onEdit() {
            this.onEdit_(this.editId);
        }
    }, {
        key: 'onEdit_',
        value: function onEdit_(ele) {
            var self = this;
            this.on$("click", ele, function (e) {
                self.handleEdit_(e);
            });
        }
    }, {
        key: 'handleEdit_',
        value: function handleEdit_(e) {
            this.dispatchEvent(e, this.editETN);
        }
    }, {
        key: 'vueId',
        get: function get() {
            return 'vue__' + this.id.replace(/-/g, "");
        }
    }, {
        key: 'editId',
        get: function get() {
            return 'edt__' + this.id;
        }
    }, {
        key: 'edit$',
        get: function get() {
            return this.byId(this.editId);
        }
    }, {
        key: 'editETN',
        get: function get() {
            return this._editETN_;
        }
    }, {
        key: 'query',
        get: function get() {
            return this._query_;
        },
        set: function set(query) {
            this._query_ = query;
        }
    }, {
        key: 'args',
        get: function get() {
            return this._args_;
        },
        set: function set(args) {
            this._args_ = args;
        }
    }, {
        key: 'categoryId',
        get: function get() {
            return this._categoryId_;
        },
        set: function set(categoryId) {
            this._categoryId_ = categoryId;
        }
    }, {
        key: 'htmlText',
        get: function get() {
            return this._htmlText_;
        },
        set: function set(htmlText) {
            this._htmlText_ = htmlText;
        }
    }, {
        key: 'vueData',
        get: function get() {
            return this._vueData_;
        }
    }, {
        key: 'metadata',
        get: function get() {
            return this._metadata_;
        }
    }]);
    
    return KnowVue;
}(UserControl);
//table>tr
var TrNode = function (_Control3) {
    _inherits(TrNode, _Control3);

    _createClass(TrNode, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!TrNode.hasOwnProperty("__instanceId__")) TrNode.__instanceId__ = 0;
            return ++TrNode.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var data = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
            var caption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var text = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
            var catalog = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
            var contextmenu = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var parent = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var pre = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var next = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
            var enviroment = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : {};
            var id = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';
            var name = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : '';

            return new TrNode(data, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "TrNode";
        }
    }]);

    function TrNode() {
        var data = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
        var caption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var text = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var catalog = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
        var contextmenu = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var parent = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var pre = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var next = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
        var enviroment = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : {};
        var id = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';
        var name = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : '';

        _classCallCheck(this, TrNode);

        var _this12 = _possibleConstructorReturn(this, (TrNode.__proto__ || Object.getPrototypeOf(TrNode)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));

        _this12._data_ = data;
        return _this12;
    }

    _createClass(TrNode, [{    
        key: 'select',
        value: function select() {
            this.isInDocument && this.addClass(this.selector$, "selected");
        }
    }, {
        key: 'cancelSelect',
        value: function cancelSelect() {
            this.isInDocument && this.removeClass(this.selector$, "selected");
        }
    }, {
        key: 'currentTr',
        value: function currentTr(curTr) {
            var trs = curTr.parentNode.children;
            for (var i = 0; i < trs.length; i++) {
                trs[i].classList.remove('activeTr');
            }
            curTr.classList.add('activeTr');
        }
    }, {
        key: 'getView',
        value: function getView() {
            var tdBuider = '';          
            for (var m in this._data_) {
                tdBuider += "<td>" + this._data_[m] + "</td>";
            }
            return '<tr id="' + this.viewId + '" onclick="TrNode._Create().currentTr(this)">' + tdBuider + '</tr>';


            var v = '<li id="' + this.viewId + '" class="xvision-tree-node">';
            v += this.getHeader();
            v += this.getBody();
            v += '</li>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div id="' + this.nodeHeaderId + '" class="xvision-tree-node-header">';
            v += this.getEditor();
            v += '<div id="' + this.selectorId + '" class="xvision-tree-node-selector"></div>';
            v += this.getExpanderView();
            v += '<span id="' + this.textId + '" class="xvision-tree-node-text">' + this.text + '</span>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getExpanderView',
        value: function getExpanderView() {
            var expId = this.expanderId;
            var expCls = ["xvision-tree-node-expander", "glyphicon", "glyphicon-play"];
            if (this.children.length < 1) expCls.push("leaf");
            expCls = this.joinClassName(expCls);
            var v = '<i id="' + expId + '" class="' + expCls + '"></i>';
            return v;
        }
    }, {
        key: 'getEditor',
        value: function getEditor() {
            var v = '<div id="' + this.editorId + '" class="xvision-tree-node-editor">';
            v += '<div class="xvision-window">';
            v += '<input id="' + this.editorInputId + '" class="xvision-tree-node-editor-input" type="text">';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<ul id="' + this.containerId + '" class="xvision-tree-node-body">';
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            _get(TrNode.prototype.__proto__ || Object.getPrototypeOf(TrNode.prototype), 'initialize', this).call(this);
            this.__onClick();
            //this.__onExpande();
            //this.__onTextEdited();
            //this.__onContextmenu();
        }
    }, {
        key: '__onClick',
        value: function __onClick() {
            this.onClick_(this.nodeHeaderId);
        }
    }, {
        key: '__onExpande',
        value: function __onExpande() {
            this.onExpande_(this.expanderId);
        }
    }, {
        key: '__onContextmenu',
        value: function __onContextmenu() {
            this.onContextmenu_(this.nodeHeaderId);
        }
    }, {
        key: 'handleContextmenu_',
        value: function handleContextmenu_(e) {
            e.preventDefault();
            e.stopPropagation();
            this.dispatchEvent(e, this.contextmenuETN);
        }
    }, {
        key: '__onTextEdited',
        value: function __onTextEdited() {
            this.onTextEdited_(this.editorInputId);
        }
    }, {
        key: 'isExpanded',
        get: function get() {
            return this.isInDocument && this.hasClass(this.expander$, "expanded");
        }
    }, {
        key: 'data',
        get: function get() {
            return this._data_;
        },
        set: function set(data) {
            this._data_ = data;
        }
    }, {
        key: 'expanderId',
        get: function get() {
            return 'exp__' + _get(TreeNode.prototype.__proto__ || Object.getPrototypeOf(TreeNode.prototype), 'id', this);
        }
    }, {
        key: 'expander$',
        get: function get() {
            return this.byId(this.expanderId);
        }
    }, {
        key: 'selectorId',
        get: function get() {
            return 'sor__' + this.id;
        }
    }, {
        key: 'selector$',
        get: function get() {
            return this.byId(this.selectorId);
        }
    }, {
        key: 'editorId',
        get: function get() {
            return 'eor__' + this.id;
        }
    }, {
        key: 'editor$',
        get: function get() {
            return this.byId(this.editorId);
        }
    }, {
        key: 'editorInputId',
        get: function get() {
            return 'eii__' + this.id;
        }
    }, {
        key: 'editorInput$',
        get: function get() {
            return this.byId(this.editorInputId);
        }
    }]);

    return TrNode;
}(Control);
//Table
var Table = function (_Control4) {
    _inherits(Table, _Control4);

    _createClass(Table, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!Table.hasOwnProperty("__instanceId__")) Table.__instanceId__ = 0;
            return ++Table.__instanceId__;
        }
    }, {
        key: '_CreateFromFlatJson',
        value: function _CreateFromFlatJson(data, map) {
            if (Table._UserMap(map)) jsonArray = Table._MapFlatJson(jsonArray, map);
            var table = new Table();
            table._data_ = data;
            table.head = data.head;
            for (var i = 0; i < data.rows.length; i++) {
                var row = data.rows[i];            
                var trNode = TrNode._Create(row);
                trNode._data_ = row;
                table.addChild(trNode);
            }
            return table;
        }
    }, {
        key: '_UserMap',
        value: function _UserMap(map) {
            return map && map.id && map.parentId && map.text;
        }
    }, {
        key: '_MapFlatJson',
        value: function _MapFlatJson(jsonArray, map) {
            var t = void 0;
            var jArr = [];
            jsonArray.forEach(function (o) {
                t = {};
                t.id = o[map.id];
                t.parentId = o[map.parentId] || "#";
                t.text = o[map.text];
                t.raw = o;
                jArr.push(t);
            });
            return jArr;
        }
    }, {     
        key: '_FindRootNodes',
        value: function _FindRootNodes(jsonArray, rootId) {
            var rootNodes = [];
            if (rootId) {
                rootNodes = Tree._FindRooetNodesById(jsonArray, rootId);
            } else {
                rootNodes = Tree._FindRootNodesByRecursive(jsonArray);
            }
            return rootNodes;
        }
    }, {  
        key: '_FindRootNodesById',
        value: function _FindRootNodesById(jsonArray) {
            var rootId = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "#";

            var nodes = [];
            jsonArray.forEach(function (itor) {
                if (itor.parentId && itor.parentId == rootId) node.push(itor);
            });
            return nodes;
        }
    }, {
        key: '_IndexNodeJsonArray',
        value: function _IndexNodeJsonArray(jsonArray) {
            return Table._IndexJsonArray(jsonArray, "id");
        }
    }, {
        key: '_IndexJsonArray',
        value: function _IndexJsonArray(jsonArray, indexKey) {
            var index = {};
            jsonArray.forEach(function (itor) {
                return index[itor[indexKey]] = itor;
            });
            return index;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
            var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
            var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
            var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

            return new Table(caption, text, catalog, contextmenu, parent, pre, next, enviroment);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Table";
        }
    }]);

    function Table() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
        var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
        var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

        _classCallCheck(this, Table);

        var _this13 = _possibleConstructorReturn(this, (Table.__proto__ || Object.getPrototypeOf(Table)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));

        _this13._selected_ = void 0;
        return _this13;
    }

    _createClass(Table, [{
        key: 'createNode',
        value: function createNode() {
            var tn = TrNode._Create();
            tn.parent = this;
            tn.modify();
        }
    }, {
        key: 'findNodeByDataId',
        value: function findNodeByDataId(id) {
            var nodes = this.children;
            return this.findNodeByDataId_(id, nodes);
        }
    }, {
        key: 'findNodeByDataId_',
        value: function findNodeByDataId_(id, nodes) {
            var i = 0,
                j = nodes.length,
                node = void 0;
            for (; i < j; i++) {
                node = nodes[i];
                if (node.data && node.data.id == id) return node;
                node = this.findNodeByDataId_(id, node.children);
                if (node) return node;
            }
            return node;
        }
    }, {
        key: 'selectNode',
        value: function selectNode(node) {
            this._selected_ && this._selected_.cancelSelect();
            this._selected_ = node;
            this._selected_.select();
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-table">';
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div class="xvision-table-header">';
            v += '<span id="' + this.captionId + '">' + this.caption + '</span>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<table id="' + this.containerId + '" class="xvision-table-body">';
            var headData = this._data_.head;
            var headTds = "";
            for (var i = 0; i < headData.length; i++) {
                headTds += "<th>" + headData[i] + "</th>";
            }
            v += "<thead><tr>" + headTds+ "</tr></thead>";
            v += "<tbody>" + this.getChildView() + "</tbody>";
            
            v += '</table>';
            return v;
            var v = '<ul id="' + this.containerId + '" class="xvision-tree-body">';
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: 'handleTreeNodeClick_',
        value: function handleTreeNodeClick_(xEventArgs) {
            var node = xEventArgs.srcTarget;
            if (this !== node) {
                var xe = new XEventArgs(null, 'selectTreeNode.' + this.eventNamespace);
                xe.addPath(xEventArgs.srcTarget);
                this.routeEvent(xe);
            }
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            _get(Table.prototype.__proto__ || Object.getPrototypeOf(Table.prototype), 'initialize', this).call(this);
            this.__onTreeNodeClick();
            this.__onTreeNodeContextmenu();
        }
    }, {
        key: '__onTreeNodeClick',
        value: function __onTreeNodeClick() {
            var self = this;
            this.on("click.xWander", function (source, xEventArgs) {
                self.handleTreeNodeClick_(xEventArgs);
                
            });
        }
    }, {
        key: '__onTreeNodeContextmenu',
        value: function __onTreeNodeContextmenu() {
            var self = this;
            this.on("contextmenu.xWander", function (source, xEventArgs) {
                self.handleTreeNodeContextmenu_(xEventArgs);
            });
        }
    }, {
        key: '__onTreeContextmenu',
        value: function __onTreeContextmenu() {
            var self = this;
            this.on$("contextmenu", this.containerId, function (e) {
                self.handleContextmenu_(e);
            });
        }
    }, {
        key: 'handleContextmenu_',
        value: function handleContextmenu_(e) {
            e.preventDefault();
            var pageX = e.pageX;
            var pageY = e.pageY;
            this.showContextmenu(pageX, pageY);
        }
    }, {
        key: 'handleTreeNodeContextmenu_',
        value: function handleTreeNodeContextmenu_(xEventArgs) {
            var node = xEventArgs.srcTarget;
            var event = xEventArgs.event;
            var pageX = event.pageX;
            var pageY = event.pageY;
            this.showContextmenu_(node, pageX, pageY);
        }
    }, {
        key: 'selected',
        get: function get() {
            return this._selected_;
        }
    }]);

    return Table;
}(Control);
//树节点
var TreeNode = function (_Control3) {
    _inherits(TreeNode, _Control3);

    _createClass(TreeNode, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!TreeNode.hasOwnProperty("__instanceId__")) TreeNode.__instanceId__ = 0;
            return ++TreeNode.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var data = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
            var caption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var text = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
            var catalog = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
            var contextmenu = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var parent = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var pre = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var next = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
            var enviroment = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : {};
            var id = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';
            var name = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : '';

            return new TreeNode(data, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "TreeNode";
        }
    }]);

    function TreeNode() {
        var data = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : void 0;
        var caption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var text = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var catalog = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
        var contextmenu = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var parent = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var pre = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var next = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
        var enviroment = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : {};
        var id = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';
        var name = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : '';

        _classCallCheck(this, TreeNode);

        var _this12 = _possibleConstructorReturn(this, (TreeNode.__proto__ || Object.getPrototypeOf(TreeNode)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));

        _this12._data_ = data;
        return _this12;
    }

    _createClass(TreeNode, [{
        key: 'addChild_',
        value: function addChild_(key, node) {
            if (this.isInDocument) this.toParent();
            _get(TreeNode.prototype.__proto__ || Object.getPrototypeOf(TreeNode.prototype), 'addChild_', this).call(this, key, node);
        }
    }, {
        key: 'remove',
        value: function remove() {
            this.routeEventX('removeTreeNode.' + this.eventNamespace);
            var p = this.parent;
            if (p.children.length <= 1) p.toLeaf && p.toLeaf();
            _get(TreeNode.prototype.__proto__ || Object.getPrototypeOf(TreeNode.prototype), 'remove', this).call(this);
        }
    }, {
        key: 'isLeaf',
        value: function isLeaf() {
            return this.children.length == 0;
        }
    }, {
        key: 'toParent',
        value: function toParent() {
            var exp = "expanded";
            this.removeClass(this.expander$, "leaf");
            this.addClass(this.expander$, exp);
            this.addClass(this.container$, exp);
        }
    }, {
        key: 'toLeaf',
        value: function toLeaf() {
            var exp = "expanded";
            this.addClass(this.expander$, "leaf");
            this.removeClass(this.expander$, exp);
            this.removeClass(this.container$, exp);
        }
    }, {
        key: 'handleContextmenu_',
        value: function handleContextmenu_(e) {
            e.preventDefault();
            this.dispatchEvent(e, this.contextmenuETN);
        }
    }, {
        key: 'select',
        value: function select() {
            this.isInDocument && this.addClass(this.selector$, "selected");
        }
    }, {
        key: 'cancelSelect',
        value: function cancelSelect() {
            this.isInDocument && this.removeClass(this.selector$, "selected");
        }
    }, {
        key: 'addNode',
        value: function addNode() {
            var tn = TreeNode._Create();
            tn.parent = this;
            tn.routeEvent(new XEventArgs(null, 'addTreeNode.' + this.eventNamespace));
            tn.modify();
        }
    }, {
        key: 'modify',
        value: function modify() {
            this.editText();
        }
    }, {
        key: 'editText',
        value: function editText() {
            this.showEditor();
            var eii$ = this.editorInput$;
            eii$.value = this.text;
            eii$.select();
            eii$.focus();
        }
    }, {
        key: 'cancelEditText',
        value: function cancelEditText() {
            this.hideEditor();
            this.editorInput$.value = "";
        }
    }, {
        key: 'showEditor',
        value: function showEditor() {
            if (this.isInDocument) {
                var eor$ = this.editor$;
                eor$.style.opacity = 1;
                eor$.style.zIndex = 1;
            }
        }
    }, {
        key: 'hideEditor',
        value: function hideEditor() {
            if (this.isInDocument) {
                var eor$ = this.editor$;
                eor$.style.opacity = 0;
                eor$.style.zIndex = -2;
            }
        }
    }, {
        key: 'onExpande_',
        value: function onExpande_(ele$) {
            var self = this;
            this.on$("click", ele$, function (e) {
                self.handleExpande_(e);
            });
        }
    }, {
        key: 'handleExpande_',
        value: function handleExpande_(e) {
            e.stopPropagation();
            var exp$ = this.expander$;
            var c$ = this.container$;
            var expCls = "expanded";
            if (this.hasClass(exp$, expCls)) {
                this.removeClass(exp$, expCls);
                this.removeClass(c$, expCls);
            } else {
                this.addClass(exp$, expCls);
                this.addClass(c$, expCls);
            }
        }
    }, {
        key: 'onTextEdited_',
        value: function onTextEdited_(ele$) {
            var self = this;
            this.on$('blur', ele$, function (e) {
                self.handleTextEdited_(e);
            });
        }
    }, {
        key: 'handleTextEdited_',
        value: function handleTextEdited_(e) {
            e.stopPropagation();
            var txt = e.currentTarget.value;
            this.text = txt;
            this.hideEditor();
            this.routeEventX('modifyTreeNode.' + this.eventNamespace);
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<li id="' + this.viewId + '" class="xvision-tree-node">';
            v += this.getHeader();
            v += this.getBody();
            v += '</li>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div id="' + this.nodeHeaderId + '" class="xvision-tree-node-header">';
            v += this.getEditor();
            v += '<div id="' + this.selectorId + '" class="xvision-tree-node-selector"></div>';
            v += this.getExpanderView();
            v += '<span id="' + this.textId + '" class="xvision-tree-node-text">' + this.text + '</span>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getExpanderView',
        value: function getExpanderView() {
            var expId = this.expanderId;
            var expCls = ["xvision-tree-node-expander", "glyphicon", "glyphicon-play"];
            if (this.children.length < 1) expCls.push("leaf");
            expCls = this.joinClassName(expCls);
            var v = '<i id="' + expId + '" class="' + expCls + '"></i>';
            return v;
        }
    }, {
        key: 'getEditor',
        value: function getEditor() {
            var v = '<div id="' + this.editorId + '" class="xvision-tree-node-editor">';
            v += '<div class="xvision-window">';
            v += '<input id="' + this.editorInputId + '" class="xvision-tree-node-editor-input" type="text">';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<ul id="' + this.containerId + '" class="xvision-tree-node-body">';
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            _get(TreeNode.prototype.__proto__ || Object.getPrototypeOf(TreeNode.prototype), 'initialize', this).call(this);
            this.__onClick();
            this.__onExpande();
            this.__onTextEdited();
            this.__onContextmenu();
        }
    }, {
        key: '__onClick',
        value: function __onClick() {
            this.onClick_(this.nodeHeaderId);
        }
    }, {
        key: '__onExpande',
        value: function __onExpande() {
            this.onExpande_(this.expanderId);
        }
    }, {
        key: '__onContextmenu',
        value: function __onContextmenu() {
            this.onContextmenu_(this.nodeHeaderId);
        }
    }, {
        key: 'handleContextmenu_',
        value: function handleContextmenu_(e) {
            e.preventDefault();
            e.stopPropagation();
            this.dispatchEvent(e, this.contextmenuETN);
        }
    }, {
        key: '__onTextEdited',
        value: function __onTextEdited() {
            this.onTextEdited_(this.editorInputId);
        }
    }, {
        key: 'isExpanded',
        get: function get() {
            return this.isInDocument && this.hasClass(this.expander$, "expanded");
        }
    }, {
        key: 'data',
        get: function get() {
            return this._data_;
        },
        set: function set(data) {
            this._data_ = data;
        }
    }, {
        key: 'expanderId',
        get: function get() {
            return 'exp__' + _get(TreeNode.prototype.__proto__ || Object.getPrototypeOf(TreeNode.prototype), 'id', this);
        }
    }, {
        key: 'expander$',
        get: function get() {
            return this.byId(this.expanderId);
        }
    }, {
        key: 'selectorId',
        get: function get() {
            return 'sor__' + this.id;
        }
    }, {
        key: 'selector$',
        get: function get() {
            return this.byId(this.selectorId);
        }
    }, {
        key: 'editorId',
        get: function get() {
            return 'eor__' + this.id;
        }
    }, {
        key: 'editor$',
        get: function get() {
            return this.byId(this.editorId);
        }
    }, {
        key: 'editorInputId',
        get: function get() {
            return 'eii__' + this.id;
        }
    }, {
        key: 'editorInput$',
        get: function get() {
            return this.byId(this.editorInputId);
        }
    }]);

    return TreeNode;
}(Control);
//树
var Tree = function (_Control4) {
    _inherits(Tree, _Control4);

    _createClass(Tree, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!Tree.hasOwnProperty("__instanceId__")) Tree.__instanceId__ = 0;
            return ++Tree.__instanceId__;
        }
    }, {
        key: '_CreateFromFlatJson',
        value: function _CreateFromFlatJson(jsonArray, map) {
            if (Tree._UserMap(map)) jsonArray = Tree._MapFlatJson(jsonArray, map);
            var tree = new Tree();
            tree.contextmenu = Contextmenu._CreateD();
            var treeRoots = [];
            var treeModel = Tree._CreateTreeModelFromFlatJson(jsonArray);
            treeModel.forEach(function (itor) {
                return tree.addChild(Tree._CreateTreeNode(itor));
            });
            return tree;
        }
    }, {
        key: '_UserMap',
        value: function _UserMap(map) {
            return map && map.id && map.parentId && map.text;
        }
    }, {
        key: '_MapFlatJson',
        value: function _MapFlatJson(jsonArray, map) {
            var t = void 0;
            var jArr = [];
            jsonArray.forEach(function (o) {
                t = {};
                t.id = o[map.id];
                t.parentId = o[map.parentId] || "#";
                t.text = o[map.text];
                t.raw = o;
                jArr.push(t);
            });
            return jArr;
        }
    }, {
        key: '_CreateTreeModelFromFlatJson',
        value: function _CreateTreeModelFromFlatJson(jsonArray) {
            var roots = Tree._FindRootNodes(jsonArray);
            Tree._CompositeNodes(jsonArray, roots);
            return roots;
        }
    }, {
        key: '_FindRootNodes',
        value: function _FindRootNodes(jsonArray, rootId) {
            var rootNodes = [];
            if (rootId) {
                rootNodes = Tree._FindRooetNodesById(jsonArray, rootId);
            } else {
                rootNodes = Tree._FindRootNodesByRecursive(jsonArray);
            }
            return rootNodes;
        }
    }, {
        key: '_CompositeNodes',
        value: function _CompositeNodes(jsonArray, rootNodes) {
            var parents = rootNodes.slice();
            var parent = void 0,
                node = void 0;
            jsonArray = jsonArray.reverse();
            while ((parent = parents.shift()) && jsonArray.length > 0) {
                for (var i = jsonArray.length - 1; i >= 0; i--) {
                    node = jsonArray[i];
                    if (node.parentId == parent.id) {
                        if (parent.children) {
                            parent.children.push(node);
                        } else {
                            parent.children = [node];
                        }
                        jsonArray.splice(i, 1);
                        parents.push(node);
                    }
                }
            }
        }
    }, {
        key: '_CreateTreeNode',
        value: function _CreateTreeNode(node) {
            var tNode = TreeNode._Create();
            tNode.text = node.text;
            tNode.data = node.raw;
            if (node.children && node.children.length > 0) {
                node.children.forEach(function (itor) {
                    return tNode.addChild(Tree._CreateTreeNode(itor));
                });
            }
            return tNode;
        }
    }, {
        key: '_FindRootNodesByRecursive',
        value: function _FindRootNodesByRecursive(jsonArray) {
            var nodes = [];
            var index = Tree._IndexNodeJsonArray(jsonArray);
            jsonArray.forEach(function (itor) {
                if (!!!index[itor.parentId]) nodes.push(itor);
            });
            return nodes;
        }
    }, {
        key: '_FindRootNodesById',
        value: function _FindRootNodesById(jsonArray) {
            var rootId = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "#";

            var nodes = [];
            jsonArray.forEach(function (itor) {
                if (itor.parentId && itor.parentId == rootId) node.push(itor);
            });
            return nodes;
        }
    }, {
        key: '_IndexNodeJsonArray',
        value: function _IndexNodeJsonArray(jsonArray) {
            return Tree._IndexJsonArray(jsonArray, "id");
        }
    }, {
        key: '_IndexJsonArray',
        value: function _IndexJsonArray(jsonArray, indexKey) {
            var index = {};
            jsonArray.forEach(function (itor) {
                return index[itor[indexKey]] = itor;
            });
            return index;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
            var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
            var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
            var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

            return new Tree(caption, text, catalog, contextmenu, parent, pre, next, enviroment);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Tree";
        }
    }]);

    function Tree() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var catalog = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var contextmenu = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var parent = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var pre = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var next = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var enviroment = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : {};
        var id = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';
        var name = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';

        _classCallCheck(this, Tree);

        var _this13 = _possibleConstructorReturn(this, (Tree.__proto__ || Object.getPrototypeOf(Tree)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));

        _this13._selected_ = void 0;
        return _this13;
    }

    _createClass(Tree, [{
        key: 'createNode',
        value: function createNode() {
            var tn = TreeNode._Create();
            tn.parent = this;
            tn.modify();
        }
    }, {
        key: 'findNodeByDataId',
        value: function findNodeByDataId(id) {
            var nodes = this.children;
            return this.findNodeByDataId_(id, nodes);
        }
    }, {
        key: 'findNodeByDataId_',
        value: function findNodeByDataId_(id, nodes) {
            var i = 0,
                j = nodes.length,
                node = void 0;
            for (; i < j; i++) {
                node = nodes[i];
                if (node.data && node.data.id == id) return node;
                node = this.findNodeByDataId_(id, node.children);
                if (node) return node;
            }
            return node;
        }
    }, {
        key: 'selectNode',
        value: function selectNode(node) {
            this._selected_ && this._selected_.cancelSelect();
            this._selected_ = node;
            this._selected_.select();
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-tree">';
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
            v += this.getSelfView();
            v += '<ul id="' + this.containerId + '" class="xvision-tree-body">';
            v += this.getChildView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div class="xvision-tree-header">';
            v += '<span id="' + this.captionId + '">' + this.caption + '</span>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<ul id="' + this.containerId + '" class="xvision-tree-body">';
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: 'handleTreeNodeClick_',
        value: function handleTreeNodeClick_(xEventArgs) {
            var node = xEventArgs.srcTarget;
            if (this !== node) {
                var xe = new XEventArgs(null, 'selectTreeNode.' + this.eventNamespace);
                xe.addPath(xEventArgs.srcTarget);
                this.routeEvent(xe);
            }
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            _get(Tree.prototype.__proto__ || Object.getPrototypeOf(Tree.prototype), 'initialize', this).call(this);
            this.__onTreeNodeClick();
            this.__onTreeNodeContextmenu();
        }
    }, {
        key: '__onTreeNodeClick',
        value: function __onTreeNodeClick() {
            var self = this;
            this.on("click.xWander", function (source, xEventArgs) {
                self.handleTreeNodeClick_(xEventArgs);
            });
        }
    }, {
        key: '__onTreeNodeContextmenu',
        value: function __onTreeNodeContextmenu() {
            var self = this;
            this.on("contextmenu.xWander", function (source, xEventArgs) {
                self.handleTreeNodeContextmenu_(xEventArgs);
            });
        }
    }, {
        key: '__onTreeContextmenu',
        value: function __onTreeContextmenu() {
            var self = this;
            this.on$("contextmenu", this.containerId, function (e) {
                self.handleContextmenu_(e);
            });
        }
    }, {
        key: 'handleContextmenu_',
        value: function handleContextmenu_(e) {
            e.preventDefault();
            var pageX = e.pageX;
            var pageY = e.pageY;
            this.showContextmenu(pageX, pageY);
        }
    }, {
        key: 'handleTreeNodeContextmenu_',
        value: function handleTreeNodeContextmenu_(xEventArgs) {
            var node = xEventArgs.srcTarget;
            var event = xEventArgs.event;
            var pageX = event.pageX;
            var pageY = event.pageY;
            this.showContextmenu_(node, pageX, pageY);
        }
    }, {
        key: 'selected',
        get: function get() {
            return this._selected_;
        }
    }]);

    return Tree;
}(Control);
//控件目录
var ControlCatalog = function (_Control5) {
    _inherits(ControlCatalog, _Control5);

    _createClass(ControlCatalog, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var imgUrl = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var caption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var text = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
            var catalog = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
            var contextmenu = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var parent = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var pre = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
            var next = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
            var enviroment = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : {};
            var id = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';
            var name = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : '';

            return new ControlCatalog(imgUrl, caption, name, catalog, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "ControlCatalog";
        }
    }]);

    function ControlCatalog() {
        var imgUrl = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var caption = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var text = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : '';
        var catalog = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : '';
        var contextmenu = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var parent = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var pre = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : void 0;
        var next = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : void 0;
        var enviroment = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : {};
        var id = arguments.length > 9 && arguments[9] !== undefined ? arguments[9] : '';
        var name = arguments.length > 10 && arguments[10] !== undefined ? arguments[10] : '';

        _classCallCheck(this, ControlCatalog);

        var _this14 = _possibleConstructorReturn(this, (ControlCatalog.__proto__ || Object.getPrototypeOf(ControlCatalog)).call(this, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name));

        _this14._imgUrl_ = imgUrl;
        return _this14;
    }

    _createClass(ControlCatalog, [{
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-catalog">';
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getHeader',
        value: function getHeader() {
            var v = '<div class="xvision-catalog-header">';
            v += '<div class="xvision-catalog-caption">';
            v += '<img src="' + this.imgUrl + '"/>';
            v += '<span id="' + this.captionId + '">' + this.caption + '</span>';
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getBody',
        value: function getBody() {
            var v = '<div class="xvision-catalog-body">';
            v += this.getScrollLeftAnchor();
            v += this.getContainer();
            v += this.getScrollRightAnchor();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getScrollLeftAnchor',
        value: function getScrollLeftAnchor() {
            var v = '<div id="' + this.scrollLeftId + '" class="xvision-scroll-left">';
            v += '<';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getScrollRightAnchor',
        value: function getScrollRightAnchor() {
            var v = '<div id="' + this.scrollRightId + '" class="xvision-scroll-right">';
            v += '>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getContainer',
        value: function getContainer() {
            var v = '<ul id="' + this.containerId + '" class="xvision-catalog-scroller">';
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: 'getChildView',
        value: function getChildView() {
            var v = '';
            this.children.forEach(function (c) {
                v += '<li class="xvision-catalog-scroller-item">';
                v += c.getIconView();
                v += '</li>';
            });
            return v;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            this.initializeC();
            this.onShowScrollAnchor();
            this.onHideScrollAnchor();
            this.onScrollLeft();
            this.onScrolRight();
        }
    }, {
        key: 'initializeC',
        value: function initializeC() {
            this.children.forEach(function (c) {
                return c.initializeIcon();
            });
        }
    }, {
        key: 'onShowScrollAnchor',
        value: function onShowScrollAnchor() {
            var self = this;
            this.on$("mouseenter", this.viewId, function (e) {
                self.handleShowScrollAnchor_(e);
            });
        }
    }, {
        key: 'onHideScrollAnchor',
        value: function onHideScrollAnchor() {
            var self = this;
            var cid$ = this.viewId$;
            this.on$("mouseleave", this.viewId, function (e) {
                self.handleHideScrollAnchor_(e);
            });
        }
    }, {
        key: 'handleShowScrollAnchor_',
        value: function handleShowScrollAnchor_(e) {
            this.scrollLeft$.style.opacity = 1;
            this.scrollRight$.style.opacity = 1;
        }
    }, {
        key: 'handleHideScrollAnchor_',
        value: function handleHideScrollAnchor_(e) {
            this.scrollLeft$.style.opacity = 0;
            this.scrollRight$.style.opacity = 0;
        }
    }, {
        key: 'onScrollLeft',
        value: function onScrollLeft() {
            var self = this;
            var scl = this.scrollLeftId;
            this.on$("click", scl, function (e) {
                self.handleScroll_(e, 0);
            });
        }
    }, {
        key: 'onScrolRight',
        value: function onScrolRight() {
            var self = this;
            var scr = this.scrollRightId;
            this.on$("click", scr, function (e) {
                self.handleScroll_(e, 1);
            });
        }
    }, {
        key: 'handleScroll_',
        value: function handleScroll_(e, directon) {
            var c = this.container$;
            if (directon == 0) {
                c.scrollLeft -= 10;
            } else {
                c.scrollLeft += 10;
            }
        }
    }, {
        key: 'imgUrl',
        get: function get() {
            return this._imgUrl_;
        }
    }, {
        key: 'scrollLeftId',
        get: function get() {
            return 'scl__' + this.id;
        }
    }, {
        key: 'scrollLeft$',
        get: function get() {
            return this.byId(this.scrollLeftId);
        }
    }, {
        key: 'scrollRightId',
        get: function get() {
            return 'scr__' + this.id;
        }
    }, {
        key: 'scrollRight$',
        get: function get() {
            return this.byId(this.scrollRightId);
        }
    }]);

    return ControlCatalog;
}(Control);
//控件管理器
var ControlManager = function (_Control6) {
    _inherits(ControlManager, _Control6);

    _createClass(ControlManager, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!ControlManager.hasOwnProperty("__instanceId__")) ControlManager.__instanceId__ = 0;
            return ++ControlManager.__instanceId__;
        }
    }, {
        key: '_CreateD',
        value: function _CreateD() {
            var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
            var catalog = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "";
            var contextmenu = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var parent = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var pre = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var next = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var enviroment = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : {};

            var cm = new ControlManager(name, catalog, contextmenu, parent, pre, next, enviroment);
            var layoutCC = ControlCatalog._Create("/content/xeditor/images/布局.png", "选择布局");
            var docCC = ControlCatalog._Create("/content/xeditor/images/结构.png", "文档结构");
            var ctrlCC = ControlCatalog._Create("/content/xeditor/images/组件.png", "选择组件");
            cm.addChild_(layoutCC.caption, layoutCC);
            cm.addChild_(docCC.caption, docCC);
            cm.addChild_(ctrlCC.caption, ctrlCC);
            return cm;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
            var catalog = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "";
            var contextmenu = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var parent = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var pre = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var next = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var enviroment = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : {};

            return new ControlManager(name, catalog, contextmenu, parent, pre, next, enviroment);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "ControlManager";
        }
    }]);

    function ControlManager() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
        var catalog = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "";
        var contextmenu = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var parent = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var pre = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var next = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var enviroment = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : {};

        _classCallCheck(this, ControlManager);

        return _possibleConstructorReturn(this, (ControlManager.__proto__ || Object.getPrototypeOf(ControlManager)).call(this, name, catalog, contextmenu, parent, pre, next, enviroment));
    }

    _createClass(ControlManager, [{
        key: 'register',
        value: function register(control) {
            var catalogName = control.catalog;
            if (!this.hasChild_(catalogName)) throw new Error(catalog + '\u76EE\u5F55\u4E0D\u5B58\u5728');
            var catalog = this.getChild(catalogName);
            catalog.addChild(control);
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-container">';
            v += this.getChildView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getChildView',
        value: function getChildView() {
            var v = '<div id="' + this.containerId + '" class="xvision-row">';
            v += this.getCatalogView();
            v += '</div>';
            return v;
        }
    }, {
        key: 'getCatalogView',
        value: function getCatalogView() {
            var catalogs = this.children;
            var v = '';
            for (var i = 0, j = catalogs.length; i < j; i++) {
                v += '<div class="xvision-col-4">';
                v += catalogs[i].getView();
                if (i < j - 1) v += '<div class="xvision-splitor"></div>';
                v += '</div>';
            }
            return v;
        }
    }]);

    return ControlManager;
}(Control);
//设计器
var Designer = function (_Container) {
    _inherits(Designer, _Container);

    _createClass(Designer, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
            var contextmenu = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var parent = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var pre = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var next = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
            var enviroment = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : {};
            var id = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';
            var name = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';

            return new Designer(caption, text, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Designer";
        }
    }]);

    function Designer() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var contextmenu = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var parent = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var pre = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var next = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var enviroment = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : {};
        var id = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';
        var name = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';

        _classCallCheck(this, Designer);

        return _possibleConstructorReturn(this, (Designer.__proto__ || Object.getPrototypeOf(Designer)).call(this, caption, text, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Designer, [{
        key: 'canDrop',
        value: function canDrop(control) {
            return control.is(Layout);
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-window">';
            v += '<div id="' + this.containerId + '" class="xvision-window" style="position:relative">';
            v += this.getChildView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var v = '<div class="xvision-container">';
            v += '<div class="xvision-row">';
            v += this.getChildModel();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }]);

    return Designer;
}(Container);
//布局
var Layout = function (_Container2) {
    _inherits(Layout, _Container2);

    _createClass(Layout, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Layout";
        }
    }, {
        key: '__Catalog_',
        get: function get() {
            return "选择布局";
        }
    }]);

    function Layout() {
        var caption = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var text = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';
        var contextmenu = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var parent = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var pre = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var next = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : void 0;
        var enviroment = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : {};
        var id = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';
        var name = arguments.length > 8 && arguments[8] !== undefined ? arguments[8] : '';

        _classCallCheck(this, Layout);

        return _possibleConstructorReturn(this, (Layout.__proto__ || Object.getPrototypeOf(Layout)).call(this, caption, text, Layout.__Catalog_, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Layout, [{
        key: 'canDrop',
        value: function canDrop(control) {
            return true;
        }
    }, {
        key: 'getCaptionView',
        value: function getCaptionView() {
            return " ";
        }
    }, {
        key: 'createNew',
        value: function createNew() {
            var n = _get(Layout.prototype.__proto__ || Object.getPrototypeOf(Layout.prototype), 'createNew', this).call(this);
            n.__init();
            return n;
        }
    }, {
        key: 'initialize',
        value: function initialize() {
            _get(Layout.prototype.__proto__ || Object.getPrototypeOf(Layout.prototype), 'initialize', this).call(this);
            this.__onRemove();
        }
    }, {
        key: '__init',
        value: function __init() { }
    }]);

    return Layout;
}(Container);

var LayoutContainer = function (_Container3) {
    _inherits(LayoutContainer, _Container3);

    _createClass(LayoutContainer, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new LayoutContainer(text, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "LayoutContainer";
        }
    }]);

    function LayoutContainer() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, LayoutContainer);

        return _possibleConstructorReturn(this, (LayoutContainer.__proto__ || Object.getPrototypeOf(LayoutContainer)).call(this, '', text, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(LayoutContainer, [{
        key: 'canDrop',
        value: function canDrop(control) {
            if (control.getDocLev && control.getDocLev() != -1) {
                return false;
            }
            return true;
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-padding">';
            v += '<div id="' + this.containerId + '" class="xvision-col-12 xvision-layout-inner xvision-min-height xvision-border" style="position:relative">';
            v += this.getChildView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-padding" data-class="LayoutContainer">';
            v += '<div id="' + this.containerId + '" class="xvision-col-12 xvision-min-height xvision-border">';
            v += this.getChildModel();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }]);

    return LayoutContainer;
}(Container);

var Layout12 = function (_Layout) {
    _inherits(Layout12, _Layout);

    _createClass(Layout12, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new Layout12(text, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Layout12";
        }
    }]);

    function Layout12() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, Layout12);

        return _possibleConstructorReturn(this, (Layout12.__proto__ || Object.getPrototypeOf(Layout12)).call(this, '12', text, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Layout12, [{
        key: 'getIconView',
        value: function getIconView() {
            var v = '<div id="' + this.iconId + '" class="xvision-icon xvision-text-center" draggable="true" title="12布局">';
            v += '<span id="' + this.captionId + '">' +  '</span>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getView',
        value: function getView() {
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-layout xvision-margin-hor xvision-margin-ver-xg xvision-padding xvision-border">';
            v += this.getHeader();
            v += '<div id="' + this.containerId + '" class="xvision-col-12 xvision-layout-inner xvision-min-height xvision-border" style="position:relative">';
            v += this.getChildView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-margin-hor xvision-margin-ver-md xvision-padding xvision-border" data-class="Layout12">';
            v += '<div id="' + this.containerId + '" class="xvision-col-12 xvision-min-height xvision-border">';
            v += this.getChildModel();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }]);

    return Layout12;
}(Layout);

var knowLayout = function (_Layout2) {
    _inherits(knowLayout, _Layout2);

    _createClass(knowLayout, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new knowLayout(text, Contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "knowLayout";
        }
    }]);

    function knowLayout() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, knowLayout);

        var _this20 = _possibleConstructorReturn(this, (knowLayout.__proto__ || Object.getPrototypeOf(knowLayout)).call(this, text, contextmenu, parent, pre, next, enviroment, id, name));

        _this20._docLev_ = -1;

        _this20._headlineId_ = void 0;
        return _this20;
    }

    _createClass(knowLayout, [{
        key: 'reHeadline',
        value: function reHeadline() {
            this._docLev_ = -1;
            this._headlineIid_ = void 0;
        }
    }, {
        key: 'canDrop',
        value: function canDrop(control) {
            var p = this.parent;
            var isHeadline = control.is(KnowVue) && control.isHeadline();
            if (isHeadline) {
                if (this.children.length > 0) return false;
                var lev = control.getDocLev();
                if (p.is(Designer)) {
                    return lev == 1;
                } else {
                    var pLev = p.docLev;
                    return lev - pLev == 1;
                }
            } else {
                if (p.is(Designer)) {
                    return true;
                } else {
                    return this.docLev != -1;
                }
            }
        }
    }, {
        key: 'addChild',
        value: function addChild(control) {
            _get(knowLayout.prototype.__proto__ || Object.getPrototypeOf(knowLayout.prototype), 'addChild', this).call(this, control);
            this.afterAddChild(control);
        }
    }, {
        key: 'addChildBefore',
        value: function addChildBefore(control, bControl) {
            _get(knowLayout.prototype.__proto__ || Object.getPrototypeOf(knowLayout.prototype), 'addChildBefore', this).call(this, control, bControl);
            this.afterAddChild(control);
        }
    }, {
        key: 'afterAddChild',
        value: function afterAddChild(control) {
            var isHeadline = control.isHeadline && control.isHeadline();
            if (isHeadline) {
                this._docLev_ = control.getDocLev();
                this._headlineId_ = control.id;
            } else {
                control.categoryId = this._headlineId_;
            }
        }
    }, {
        key: 'docLev',
        get: function get() {
            return this._docLev_;
        }
    }, {
        key: 'headlineId',
        get: function get() {
            return this._headlineId_;
        }
    }]);

    return knowLayout;
}(Layout12);

var Layout8_4 = function (_Layout3) {
    _inherits(Layout8_4, _Layout3);

    _createClass(Layout8_4, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new Layout8_4(text, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Layout8_4";
        }
    }]);

    function Layout8_4() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, Layout8_4);

        return _possibleConstructorReturn(this, (Layout8_4.__proto__ || Object.getPrototypeOf(Layout8_4)).call(this, '8 4', text, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Layout8_4, [{
        key: 'getIconView',
        value: function getIconView() {
            var v = '<div id="' + this.iconId + '" class="xvision-icon xvision-text-center" draggable="true" title="8 4布局">';
            v += '<span id="' + this.captionId + '">' + '</span>';
            v += '<div style="width:4px;height:100%;background:#fff;margin-left:20px;border-left:1px solid rgb(230,230,230);border-right:1px solid rgb(230,230,230);"></div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getView',
        value: function getView() {
            var c1 = this.children[0];
            var c2 = this.children[1];
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-layout xvision-margin-hor xvision-margin-ver-xg xvision-border">';
            v += this.getHeader();
            v += '<div class="xvision-col-8">';
            v += c1.getView();
            v += '</div>';
            v += '<div class="xvision-col-4">';
            v += c2.getView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var c1 = this.children[0];
            var c2 = this.children[1];
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-margin-hor xvision-margin-ver-md xvision-border" data-class="Layout8_4">';
            v += '<div class="xvision-col-8">';
            v += c1.getModel();
            v += '</div>';
            v += '<div class="xvision-col-4">';
            v += c2.getModel();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: '__init',
        value: function __init() {
            this.addChild(LayoutContainer._Create());
            this.addChild(LayoutContainer._Create());
        }
    }]);

    return Layout8_4;
}(Layout);

var Layout6_6 = function (_Layout4) {
    _inherits(Layout6_6, _Layout4);

    _createClass(Layout6_6, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new Layout6_6(text, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Layout6_6";
        }
    }]);

    function Layout6_6() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, Layout6_6);

        return _possibleConstructorReturn(this, (Layout6_6.__proto__ || Object.getPrototypeOf(Layout6_6)).call(this, '6 6', text, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Layout6_6, [{
        key: 'getIconView',
        value: function getIconView() {
            var v = '<div id="' + this.iconId + '" class="xvision-icon xvision-text-center" draggable="true" title="6 6布局">';
            v += '<span id="' + this.captionId + '">' + '</span>';
            v += '<div style="width:4px;height:100%;background:#fff;margin-left:14px;border-left:1px solid rgb(230,230,230);border-right:1px solid rgb(230,230,230);"></div>'
            v += '</div>';
            return v;
        }
    }, {
        key: 'getView',
        value: function getView() {
            var c1 = this.children[0];
            var c2 = this.children[1];
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-layout xvision-border xvision-margin-hor xvision-margin-ver-xg">';
            v += this.getHeader();
            v += '<div class="xvision-col-6">';
            v += c1.getView();
            v += '</div>';
            v += '<div class="xvision-col-6">';
            v += c2.getView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var c1 = this.children[0];
            var c2 = this.children[1];
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-border xvision-margin-hor xvision-margin-ver-lg" data-class="Layout6_6">';
            v += '<div class="xvision-col-6">';
            v += c1.getModel();
            v += '</div>';
            v += '<div class="xvision-col-6">';
            v += c2.getModel();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: '__init',
        value: function __init() {
            this.addChild(LayoutContainer._Create());
            this.addChild(LayoutContainer._Create());
        }
    }]);

    return Layout6_6;
}(Layout);

var Layout4_4_4 = function (_Layout5) {
    _inherits(Layout4_4_4, _Layout5);

    _createClass(Layout4_4_4, null, [{
        key: '__InstanceId',
        value: function __InstanceId() {
            if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
            return ++this.__instanceId__;
        }
    }, {
        key: '_Create',
        value: function _Create() {
            var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
            var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
            var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
            var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
            var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
            var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
            var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
            var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

            return new Layout4_4_4(text, contextmenu, parent, pre, next, enviroment, id, name);
        }
    }, {
        key: '__ClassName_',
        get: function get() {
            return "Layout4_4_4";
        }
    }]);

    function Layout4_4_4() {
        var text = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var contextmenu = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : void 0;
        var parent = arguments.length > 2 && arguments[2] !== undefined ? arguments[2] : void 0;
        var pre = arguments.length > 3 && arguments[3] !== undefined ? arguments[3] : void 0;
        var next = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : void 0;
        var enviroment = arguments.length > 5 && arguments[5] !== undefined ? arguments[5] : {};
        var id = arguments.length > 6 && arguments[6] !== undefined ? arguments[6] : '';
        var name = arguments.length > 7 && arguments[7] !== undefined ? arguments[7] : '';

        _classCallCheck(this, Layout4_4_4);

        return _possibleConstructorReturn(this, (Layout4_4_4.__proto__ || Object.getPrototypeOf(Layout4_4_4)).call(this, '4 4 4', text, contextmenu, parent, pre, next, enviroment, id, name));
    }

    _createClass(Layout4_4_4, [{
        key: 'getIconView',
        value: function getIconView() {
            var v = '<div id="' + this.iconId + '" class="xvision-icon xvision-text-center" draggable="true" title="4 4 4布局">';
            v += '<span id="' + this.captionId + '"></span>';
            v += '<div style="width:4px;height:100%;background:#fff;margin-left:1px;border-left:1px solid rgb(230,230,230);border-right:1px solid rgb(230,230,230);display:inline-block"></div>';
            v += '<div style="width:4px;height:100%;background:#fff;margin-left:6px;border-left:1px solid rgb(230,230,230);border-right:1px solid rgb(230,230,230);display:inline-block"></div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getView',
        value: function getView() {
            var c1 = this.children[0];
            var c2 = this.children[1];
            var c3 = this.children[2];
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-layout xvision-margin-hor xvision-margin-ver-xg xvision-border">';
            v += this.getHeader();
            v += '<div class="xvision-col-4">';
            v += c1.getView();
            v += '</div>';
            v += '<div class="xvision-col-4">';
            v += c2.getView();
            v += '</div>';
            v += '<div class="xvision-col-4">';
            v += c3.getView();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: 'getModel',
        value: function getModel() {
            var c1 = this.children[0];
            var c2 = this.children[1];
            var c3 = this.children[2];
            var v = '<div id="' + this.viewId + '" class="xvision-row xvision-margin-hor xvision-margin-ver-lg xvision-border" data-class="Layout4_4_4">';
            v += '<div class="xvision-col-4">';
            v += c1.getModel();
            v += '</div>';
            v += '<div class="xvision-col-4">';
            v += c2.getModel();
            v += '</div>';
            v += '<div class="xvision-col-4">';
            v += c3.getModel();
            v += '</div>';
            v += '</div>';
            return v;
        }
    }, {
        key: '__init',
        value: function __init() {
            this.addChild(LayoutContainer._Create());
            this.addChild(LayoutContainer._Create());
            this.addChild(LayoutContainer._Create());
        }
    }]);
    return Layout4_4_4;
}(Layout);