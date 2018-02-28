"use strict";

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
var XEvent = function () {
    function XEvent() {
        _classCallCheck(this, XEvent);

        this._timestamp_ = new Date().valueOf();
        this._handlers_ = new Map();
        this._evtNamespace_ = "xEditor";
    }

    _createClass(XEvent, [{
        key: "on",
        value: function on(eventType, handler, context) {
            var handlerMetadata = { handler: handler, context: context };
            this.addHandler(eventType, handlerMetadata);
        }
    }, {
        key: "once",
        value: function once(eventType, handler, context) {
            var once = true;
            var triggered = false;
            var handlerMetadata = { handler: handler, context: context, once: once, triggered: triggered };
            this.addHandler(eventType, handlerMetadata);
        }
    }, {
        key: "trigger",
        value: function trigger(eventType, eventData) {
            var source = this;
            var eventArgs = Object.assign({}, eventData);
            var handlersMetadata = source._handlers_.get(eventType);
            if (handlersMetadata) XEvent._InvokeHandlers(handlersMetadata, source, eventArgs);
        }
    }, {
        key: "un",
        value: function un(eventType, handler) {
            var handlers = this._handlers_;
            var handlersMetadata = handlers.get(eventType);
            for (var i = 0, j = handlersMetadata.length; i < j; i++) {
                if (handlersMetadata[i]["handler"] === handler) {
                    handlersMetadata.splice(i, 1);
                    break;
                }
            }
        }
    }, {
        key: "addHandler",
        value: function addHandler(eventType, handlerMetadata) {
            var handlers = this._handlers_;
            if (handlers.has(eventType)) {
                handlers.get(eventType).push(handlerMetadata);
            } else {
                handlers.set(eventType, [handlerMetadata]);
            }
        }
    }, {
        key: "timestamp",
        get: function get() {
            return this._timestamp_;
        }
    }, {
        key: "evtNamespace",
        get: function get() {
            return this._evtNamespace_;
        }
    }], [{
        key: "_InvokeHandlers",
        value: function _InvokeHandlers(handlersMetadata, source, eventArgs) {
            var _iteratorNormalCompletion = true;
            var _didIteratorError = false;
            var _iteratorError = undefined;

            try {
                for (var _iterator = handlersMetadata[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                    var handlerMetadata = _step.value;

                    if (Reflect.has(handlerMetadata, "once")) {
                        XEvent._InvokeHandlerOnce(handlerMetadata, source, eventArgs);
                    } else {
                        XEvent._InvokeHandler(handlerMetadata, source, eventArgs);
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
        }
    }, {
        key: "_InvokeHandlerOnce",
        value: function _InvokeHandlerOnce(handlerMetadata, source, eventArgs) {
            if (!handlerMetadata.triggered) {
                handlerMetadata.triggered = true;
                XEvent._InvokeHandler(handlerMetadata, source, eventArgs);
            }
        }
    }, {
        key: "_InvokeHandler",
        value: function _InvokeHandler(handlerMetadata, source, eventArgs) {
            var handler = handlerMetadata.handler;
            if (Reflect.has(handlerMetadata, "context")) {
                Reflect.apply(handler, handlerMetadata["context"], [source, eventArgs]);
            } else {
                handler(source, eventArgs);
            }
        }
    }]);

    return XEvent;
}();

var XObject = function (_XEvent) {
    _inherits(XObject, _XEvent);

    function XObject() {
        _classCallCheck(this, XObject);

        var _this = _possibleConstructorReturn(this, (XObject.__proto__ || Object.getPrototypeOf(XObject)).call(this));

        _this._id_ = _this.guid;
        return _this;
    }

    _createClass(XObject, [{
        key: "createNew",
        value: function createNew() {
            return new this.constructor();
        }
    }, {
        key: "id",
        get: function get() {
            return this._id_;
        }
    }, {
        key: "names",
        get: function get() {
            return XObject._GetOwnPropertyNames(this);
        }
    }, {
        key: "values",
        get: function get() {
            return XObject._GetOwnPropertyValues(this);
        }
    }, {
        key: "guid",
        get: function get() {
            return XObject._Guid();
        }
    }], [{
        key: "_GetOwnPropertyNames",
        value: function _GetOwnPropertyNames(obj) {
            return Object.GetOwnPropertyNames(obj);
        }
    }, {
        key: "_GetOwnPropertyValues",
        value: function _GetOwnPropertyValues(obj) {
            var values = [];
            for (var attr in obj) {
                if (obj.hasOwnProperty(attr)) values.push(obj[attr]);
            }
            return values;
        }
    }, {
        key: "_Guid",
        value: function _Guid() {
            var guid = "";
            for (var i = 0; i < 32; i++) {
                guid += Math.floor(Math.random() * 16.0).toString(16);
                if (i == 8 || i == 12 || i == 16 || i == 20) guid += '-';
            }
            return guid;
        }
    }]);

    return XObject;
}(XEvent);

var UIElement = function (_XObject) {
    _inherits(UIElement, _XObject);

    function UIElement() {
        _classCallCheck(this, UIElement);

        var _this2 = _possibleConstructorReturn(this, (UIElement.__proto__ || Object.getPrototypeOf(UIElement)).call(this));

        _this2._document$_ = null;
        _this2._viewId_ = "v__" + _get(UIElement.prototype.__proto__ || Object.getPrototypeOf(UIElement.prototype), "id", _this2);
        _this2._view$_ = null;
        return _this2;
    }

    _createClass(UIElement, [{
        key: "remove",
        value: function remove() {
            this.view$.remove();
        }
    }, {
        key: "toJqueryId",
        value: function toJqueryId(elementId) {
            return UIElement._ToJQueryId(elementId);
        }
    }, {
        key: "triggerHandler",
        value: function triggerHandler(eventType, eventData) {
            this.document.triggerHandler(eventType, eventData);
        }
    }, {
        key: "getBrowserEvent",
        value: function getBrowserEvent(evt$) {
            return evt$.originalEvent;
        }
    }, {
        key: "stopPropagation",
        value: function stopPropagation(event) {
            UIElement._StopPropagation(event);
        }
    }, {
        key: "preventDefault",
        value: function preventDefault(event) {
            UIElement._PreventDefault(event);
        }
    }, {
        key: "dispatchEvent",
        value: function dispatchEvent(event, eventType, eventArgs) {
            this.stopPropagation(event);
            Object.assign(eventArgs, { eType: eventType });
            this.trigger(eventType, eventArgs);
            this.triggerHandler(eventType, { source: this, evtArgs: eventArgs });
        }
    }, {
        key: "document",
        get: function get() {
            var doc = this._document$_;
            if (!doc) doc = this._document$_ = $(document);
            return doc;
        }
    }, {
        key: "viewId",
        get: function get() {
            return this._viewId_;
        }
    }, {
        key: "viewId$",
        get: function get() {
            return this.toJqueryId(this.viewId);
        }
    }, {
        key: "view$",
        get: function get() {
            var v = this._view$_;
            if (!v) v = this._view$_ = $(this.viewId$);
            return v;
        }
    }], [{
        key: "_ToJQueryId",
        value: function _ToJQueryId(elementId) {
            var jId = elementId;
            if (!UIElement._IsJQueryId(elementId)) jId = "#" + jId;
            return jId;
        }
    }, {
        key: "_IsJQueryId",
        value: function _IsJQueryId(elementId) {
            return UIElement._IsJQueryIdCommon(elementId, function (s) {
                return (/^#\w+/g.test(s)
                );
            });
        }
    }, {
        key: "_IsJQueryIdCommon",
        value: function _IsJQueryIdCommon(elementId, predicate) {
            if (!UIElement._IsPredicate(predicate)) throw new TypeError("请传入一个合法的谓词");
            return predicate(elementId);
        }
    }, {
        key: "_IsPredicate",
        value: function _IsPredicate(predicate) {
            return typeof predicate === 'function';
        }
    }, {
        key: "_StopPropagation",
        value: function _StopPropagation(event) {
            event.stopPropagation();
            event.originalEvent && event.originalEvent.stopPropagation();
        }
    }, {
        key: "_PreventDefault",
        value: function _PreventDefault(event) {
            event.preventDefault();
            event.originalEvent && event.originalEvent.preventDefault();
        }
    }]);

    return UIElement;
}(XObject);

var Draggable = function (_UIElement) {
    _inherits(Draggable, _UIElement);

    function Draggable() {
        _classCallCheck(this, Draggable);

        var _this3 = _possibleConstructorReturn(this, (Draggable.__proto__ || Object.getPrototypeOf(Draggable)).call(this));

        _this3._evtNamespace_ = "draggable." + _get(Draggable.prototype.__proto__ || Object.getPrototypeOf(Draggable.prototype), "evtNamespace", _this3);
        _this3._dragStart_ = "dragStart." + _this3._evtNamespace_;
        _this3.__onDragStart();
        return _this3;
    }

    _createClass(Draggable, [{
        key: "getView",
        value: function getView() {
            var v = "<span id=\"" + this.viewId + "\" class=\"xvision-draggable-anchor\" draggable=\"true\" >";
            v += "<i class=\"\"></i>";
            v += '拖动';
            v += '</span>';
            return v;
        }
    }, {
        key: "getDragImage",
        value: function getDragImage() {
            return document.getElementById(this.viewId);;
        }
    }, {
        key: "__onDragStart",
        value: function __onDragStart() {
            var self = this;
            var id$ = this.viewId$;
            $(document).on('dragstart', id$, function (e) {
                self.__dispatchDragStart(e);
            });
        }
    }, {
        key: "__dispatchDragStart",
        value: function __dispatchDragStart(e) {
            var originalEvent = this.getBrowserEvent(e);
            var dataTransfer = originalEvent.dataTransfer;
            var source = this;
            var evtType = this._dragStart_;
            var evtArgs = { dataTransfer: dataTransfer, event: originalEvent };
            Object.assign(evtArgs, e);
            this.dispatchEvent(e, evtType, evtArgs);
        }
    }, {
        key: "dragStartEvent",
        get: function get() {
            return this._dragStart_;
        }
    }]);

    return Draggable;
}(UIElement);

var Configurable = function (_UIElement2) {
    _inherits(Configurable, _UIElement2);

    function Configurable() {
        _classCallCheck(this, Configurable);

        var _this4 = _possibleConstructorReturn(this, (Configurable.__proto__ || Object.getPrototypeOf(Configurable)).call(this));

        _this4._evtNamespace_ = "configurable." + _get(Configurable.prototype.__proto__ || Object.getPrototypeOf(Configurable.prototype), "evtNamespace", _this4);
        _this4._editEvent_ = "edit." + _this4._evtNamespace_;
        _this4._removeEvent_ = "remove." + _this4._evtNamespace_;
        _this4._editButtonId_ = "e__" + _get(Configurable.prototype.__proto__ || Object.getPrototypeOf(Configurable.prototype), "id", _this4);
        _this4._removeButtonId_ = "r__" + _get(Configurable.prototype.__proto__ || Object.getPrototypeOf(Configurable.prototype), "id", _this4);
        _this4.__onEditClick();
        _this4.__onRemoveClick();
        return _this4;
    }

    _createClass(Configurable, [{
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\" class=\"xvision-configurable\">";
            v += this.__getEditButtonView();
            v += this.__getRemoveButtonView();
            v += '</div>';
            return v;
        }
    }, {
        key: "__getEditButtonView",
        value: function __getEditButtonView() {
            var cls = ["xvision-configurable-edit", "glyphicon", "glyphicon-edit"];
            cls = cls.join(" ");
            var v = "<i id=\"" + this._editButtonId_ + "\" class=\"" + cls + "\">";
            v += '</i>';
            return v;
        }
    }, {
        key: "__getRemoveButtonView",
        value: function __getRemoveButtonView() {
            var cls = ["xvision-configurable-edit", "glyphicon", "glyphicon-remove"];
            cls = cls.join(" ");
            var v = "<i id=\"" + this._removeButtonId_ + "\" class=\"" + cls + "\">";
            v += '</i>';
            return v;
        }
    }, {
        key: "__onEditClick",
        value: function __onEditClick() {
            var self = this;
            var editButtonId$ = this.editButtonId$;
            var editEvent = this.editEvent;
            self.document.on("click", editButtonId$, function (e) {
                self.__onClickCommonHandler(e, editEvent);
            });
        }
    }, {
        key: "__onRemoveClick",
        value: function __onRemoveClick() {
            var self = this;
            var removeButtonId$ = this.removeButtonId$;
            var removeEvent = this.removeEvent;
            self.document.on("click", removeButtonId$, function (e) {
                self.__onClickCommonHandler(e, removeEvent);
            });
        }
    }, {
        key: "__onClickCommonHandler",
        value: function __onClickCommonHandler(e, eventType) {
            this.stopPropagation(e);
            var source = this;
            var evtArgs = Object.assign({}, e);
            this.trigger(eventType, evtArgs);
            this.triggerHandler(eventType, { source: source, evtArgs: evtArgs });
        }
    }, {
        key: "EventNamespace",
        get: function get() {
            return this._evtNamespace_;
        }
    }, {
        key: "editEvent",
        get: function get() {
            return this._editEvent_;
        }
    }, {
        key: "removeEvent",
        get: function get() {
            return this._removeEvent_;
        }
    }, {
        key: "editButtonId",
        get: function get() {
            return this._editButtonId_;
        }
    }, {
        key: "editButtonId$",
        get: function get() {
            return this.toJqueryId(this._editButtonId_);
        }
    }, {
        key: "editButton$",
        get: function get() {
            return $(this.editeId$);
        }
    }, {
        key: "removeButtonId",
        get: function get() {
            return this._removeButtonId_;
        }
    }, {
        key: "removeButtonId$",
        get: function get() {
            return this.toJqueryId(this._removeButtonId_);
        }
    }, {
        key: "removeButton$",
        get: function get() {
            return $(this.removeButtonId$);
        }
    }]);

    return Configurable;
}(UIElement);

var Control = function (_UIElement3) {
    _inherits(Control, _UIElement3);

    function Control() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, Control);

        var _this5 = _possibleConstructorReturn(this, (Control.__proto__ || Object.getPrototypeOf(Control)).call(this));

        _this5._name_ = name;
        _this5._type_ = type;

        _this5._evtNamespace_ = "control." + _get(Control.prototype.__proto__ || Object.getPrototypeOf(Control.prototype), "evtNamespace", _this5);

        _this5._dragEnter_ = "dravEnter." + _this5._evtNamespace_;
        _this5._dragLeave_ = "dravLeave." + _this5._evtNamespace_;
        _this5._dragOver_ = "dragOver." + _this5._evtNamespace_;
        _this5._drop_ = "drop." + _this5._evtNamespace_;
        _this5._select_ = "select." + _this5._evtNamespace_;

        _this5._parent_ = null;

        _this5.__onDragDrop();
        _this5.__onSelect();
        return _this5;
    }

    _createClass(Control, [{
        key: "createNew",
        value: function createNew() {
            return new this.constructor("", this.type);
        }
    }, {
        key: "canDrop",
        value: function canDrop(control) {
            return true;
        }
    }, {
        key: "remove",
        value: function remove() {
            _get(Control.prototype.__proto__ || Object.getPrototypeOf(Control.prototype), "remove", this).call(this);
            if (this.parent) {
                this.parent.removeControlFromModel(this);
            }
        }
    }, {
        key: "getView",
        value: function getView() {
            return "<div id=\"" + this.viewId + "\"></div>";
        }
    }, {
        key: "renderSelected",
        value: function renderSelected() {
            this.view$.addClass("selected");
        }
    }, {
        key: "cancelSelected",
        value: function cancelSelected() {
            this.view$.removeClass("selected");
        }
    }, {
        key: "__onDragDrop",
        value: function __onDragDrop() {
            var vid$ = this.viewId$;
            this.__handleDragDrop(vid$);
        }
    }, {
        key: "__handleDragDrop",
        value: function __handleDragDrop(listenElement$) {
            var self = this;
            self.document.on("dragenter", listenElement$, function (e) {
                self.__dispatchDragDrop(e, self.dragEnterEvent);
            }).on("dragleave", listenElement$, function (e) {
                self.__dispatchDragDrop(e, self.dragLeaveEvent);
            }).on("dragover", listenElement$, function (e) {
                self.__dispatchDragDrop(e, self.dragOverEvent);
            }).on("drop", listenElement$, function (e) {
                self.__dispatchDragDrop(e, self.dropEvent);
            });
        }
    }, {
        key: "__dispatchDragDrop",
        value: function __dispatchDragDrop(e, eventType) {
            var originalEvent = this.getBrowserEvent(e);
            var source = this;
            var evtType = eventType;
            var evtArgs = { dataTransfer: originalEvent.dataTransfer, event: originalEvent };
            Object.assign(evtArgs, e);
            this.preventDefault(e);
            this.dispatchEvent(e, evtType, evtArgs);
        }
    }, {
        key: "__onSelect",
        value: function __onSelect() {
            var self = this;
            var vid$ = this.viewId$;
            self.document.on("click", vid$, function (e) {
                self.__handleSelect(e);
            });
        }
    }, {
        key: "__handleSelect",
        value: function __handleSelect(e) {
            this.renderSelected();
            this.__dispatchSelect(e);
        }
    }, {
        key: "__dispatchSelect",
        value: function __dispatchSelect(e) {
            var evtType = this._select_;
            var evtArgs = Object.assign({}, e);
            this.dispatchEvent(e, evtType, evtArgs);
        }
    }, {
        key: "name",
        get: function get() {
            return this._name_;
        },
        set: function set(n) {
            this._name_ = n;
        }
    }, {
        key: "type",
        get: function get() {
            return this._type_;
        },
        set: function set(tn) {
            this._type_ = tn;
        }
    }, {
        key: "evtNamespace",
        get: function get() {
            return this._evtNamespace_;
        }
    }, {
        key: "dragEnterEvent",
        get: function get() {
            return this._dragEnter_;
        }
    }, {
        key: "dragLeaveEvent",
        get: function get() {
            return this._dragLeave_;
        }
    }, {
        key: "dragOverEvent",
        get: function get() {
            return this._dragOver_;
        }
    }, {
        key: "dropEvent",
        get: function get() {
            return this._drop_;
        }
    }, {
        key: "selectEvent",
        get: function get() {
            return this._select_;
        }
    }, {
        key: "parent",
        get: function get() {
            return this._parent_;
        },
        set: function set(control) {
            this._parent_ = control;
        }
    }]);

    return Control;
}(UIElement);

var Container = function (_Control) {
    _inherits(Container, _Control);

    function Container() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, Container);

        var _this6 = _possibleConstructorReturn(this, (Container.__proto__ || Object.getPrototypeOf(Container)).call(this, name, type));

        _this6._containerId_ = "c__" + _get(Container.prototype.__proto__ || Object.getPrototypeOf(Container.prototype), "id", _this6);
        _this6._container$_ = null;
        _this6._controls_ = new Map();

        _this6._selectContainer_ = "selectContainer." + _get(Container.prototype.__proto__ || Object.getPrototypeOf(Container.prototype), "evtNamespace", _this6);

        _this6.__onDragDrop();
        _this6.__onSelectContainer();
        return _this6;
    }

    _createClass(Container, [{
        key: "addControl2Model",
        value: function addControl2Model(control) {
            control.parent = this;
            this.controls.set(control.id, control);
        }
    }, {
        key: "removeControlFromModel",
        value: function removeControlFromModel(control) {
            var controlId = control.id;
            var controls = this.controls;
            if (controls.has(controlId)) {
                control.parent = null;
                this.controls.delete(control.id);
            }
        }
    }, {
        key: "addControl",
        value: function addControl(control) {
            this.addControl2Model(control);
            var v = control.getView();
            this.container$.append(v);
        }
    }, {
        key: "removeControl",
        value: function removeControl(control) {
            control.remove();
        }
    }, {
        key: "getControlById",
        value: function getControlById(id) {
            return this.controls.get(id);
        }
    }, {
        key: "canDrop",
        value: function canDrop(control) {
            return true;
        }
    }, {
        key: "allowDrop",
        value: function allowDrop(browerEvent) {
            browerEvent.preventDefault();
        }
    }, {
        key: "preAddControl",
        value: function preAddControl() {
            var v = "<div class=\"xvision-ctrl-placeholder\"></div>";
            this.container$.append(v);
        }
    }, {
        key: "cancelPreAddControl",
        value: function cancelPreAddControl() {
            this.container$.find(".xvision-ctrl-placeholder").remove();
        }
    }, {
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\">";
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
        }
    }, {
        key: "getHeader",
        value: function getHeader() {
            var v = "<h3>" + this._name_ + "</h3>";
        }
    }, {
        key: "getBody",
        value: function getBody() {
            var v = "<div id=\"" + this.containerId + "\">";
            v += this.getChildView();
            v += '</div>';
            return v;
        }
    }, {
        key: "getChildView",
        value: function getChildView() {
            var v = "";
            this.childControls.forEach(function (ctrl) {
                return v += ctrl.getView();
            });
            return v;
        }
    }, {
        key: "renderContainerSelected",
        value: function renderContainerSelected() {
            this.container$.addClass("selected");
        }
    }, {
        key: "cancelContainerSelected",
        value: function cancelContainerSelected() {
            this.container$.removeClass("selected");
        }
    }, {
        key: "__onDragDrop",
        value: function __onDragDrop() {
            var cid$ = this.containerId$;
            this.__handleDragDrop(cid$);
        }
    }, {
        key: "__onSelectContainer",
        value: function __onSelectContainer() {
            var self = this;
            var cid$ = self.containerId$;
            self.document.on("click", cid$, function (e$) {
                self.__handleSelectContainer(e$);
            });
        }
    }, {
        key: "__handleSelectContainer",
        value: function __handleSelectContainer(e$) {
            this.renderContainerSelected();
            this.__dispatchSelectContainerEvent(e$);
        }
    }, {
        key: "__dispatchSelectContainerEvent",
        value: function __dispatchSelectContainerEvent(e$) {
            var evtType = this._selectContainer_;
            var evtArgs = Object.assign({}, e$);
            this.dispatchEvent(e$, evtType, evtArgs);
        }
    }, {
        key: "containerId",
        get: function get() {
            return this._containerId_;
        }
    }, {
        key: "containerId$",
        get: function get() {
            return this.toJqueryId(this.containerId);
        }
    }, {
        key: "container$",
        get: function get() {
            var ctn = this._container$_;
            if (!ctn) ctn = this._container$_ = $(this.containerId$);
            return ctn;
        }
    }, {
        key: "controls",
        get: function get() {
            return this._controls_;
        }
    }, {
        key: "childControls",
        get: function get() {
            var ctrls = [];
            var _iteratorNormalCompletion2 = true;
            var _didIteratorError2 = false;
            var _iteratorError2 = undefined;

            try {
                for (var _iterator2 = this.controls[Symbol.iterator](), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
                    var itorCtrl = _step2.value;

                    ctrls.push(itorCtrl[1]);
                }
            } catch (err) {
                _didIteratorError2 = true;
                _iteratorError2 = err;
            } finally {
                try {
                    if (!_iteratorNormalCompletion2 && _iterator2.return) {
                        _iterator2.return();
                    }
                } finally {
                    if (_didIteratorError2) {
                        throw _iteratorError2;
                    }
                }
            }

            return ctrls;
        }
    }]);

    return Container;
}(Control);

var Node = function (_Container) {
    _inherits(Node, _Container);

    function Node() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';

        _classCallCheck(this, Node);

        var _this7 = _possibleConstructorReturn(this, (Node.__proto__ || Object.getPrototypeOf(Node)).call(this, name, "tree_node"));

        _this7._expanderViewId_ = "exp__" + _get(Node.prototype.__proto__ || Object.getPrototypeOf(Node.prototype), "id", _this7);
        _this7._text_ = '';
        _this7._data_ = null;

        _this7.__onExpande();
        return _this7;
    }

    _createClass(Node, [{
        key: "getView",
        value: function getView() {
            var v = "<li id=\"" + this._viewId_ + " class=\"xvision-tree-node\">";
            v += this.getHeader();
            v += this.getBody();
            v += '</li>';
            return v;
        }
    }, {
        key: "getHeader",
        value: function getHeader() {
            var v = "<div class=\"xvision-tree-node-header\">";
            v += this.getExpanderView();
            v += this._text_;
            v += '</div>';
            return v;
        }
    }, {
        key: "getBody",
        value: function getBody() {
            var v = "<ul id=\"" + this._containerId_ + "\" class=\"xvision-tree-node-container\">";
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: "getExpanderView",
        value: function getExpanderView() {
            var expId = this._expanderViewId_;
            var expCls = ["glyphicon", "glyphicon-play"];
            if (this.childControls.length > 0) {
                expCls.push("xvision-tree-node-expander");
            } else {
                expCls.push("xvision-tree-node-expander-holder");
            }
            expCls = expCls.join(" ");
            var v = "<i id=\"" + expId + "\" class=\"" + expCls + "\"></i>";
            return v;
        }
    }, {
        key: "__onExpande",
        value: function __onExpande() {
            var self = this;
            var expId$ = this.expanderId$;
            self.document.on("click", expId$, function (e$) {
                self.__handleExpande(e$);
            });
        }
    }, {
        key: "__handleExpande",
        value: function __handleExpande(e$) {
            this.stopPropagation(e$);
            var exp$ = this.expander$;
            var c$ = this.container$;
            if (exp$.hasClass("expanded")) {
                exp$.removeClass("expanded");
                c$.removeClass("expanded");
            } else {
                exp$.addClass("expanded");
                c$.addClass("expanded");
            }
        }
    }, {
        key: "text",
        get: function get() {
            return this._text_;
        },
        set: function set(txt) {
            this._text_ = txt;
        }
    }, {
        key: "data",
        get: function get() {
            return this._data_;
        },
        set: function set(data) {
            this._data_ = data;
        }
    }, {
        key: "expanderId",
        get: function get() {
            return this._expanderViewId_;
        }
    }, {
        key: "expanderId$",
        get: function get() {
            return this.toJqueryId(this._expanderViewId_);
        }
    }, {
        key: "expander$",
        get: function get() {
            return $(this.expanderId$);
        }
    }]);

    return Node;
}(Container);

var Tree = function (_Node) {
    _inherits(Tree, _Node);

    function Tree() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';

        _classCallCheck(this, Tree);

        var _this8 = _possibleConstructorReturn(this, (Tree.__proto__ || Object.getPrototypeOf(Tree)).call(this, name, "tree"));

        _this8._extIdNodeesMap = {};
        return _this8;
    }

    _createClass(Tree, [{
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\" class=\"xvision-tree\">";
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
        }
    }, {
        key: "getHeader",
        value: function getHeader() {
            var v = "<div class=\"xvision-tree-header\">";
            v += "<span>" + this._text_ + "</span>";
            v += '</div>';
            return v;
        }
    }, {
        key: "getBody",
        value: function getBody() {
            var v = "<div class=\"xvision-tree-body\">";
            v += this.getBodyContainer();
            v += '</div>';
            return v;
        }
    }, {
        key: "getBodyContainer",
        value: function getBodyContainer() {
            var v = "<ul id=\"" + this.containerId + "\" class=\"xvision-tree-body-container\">";
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }], [{
        key: "_CreateFromFlatJson",
        value: function _CreateFromFlatJson(jsonArray) {
            var tree = new Tree();
            tree.text = "树测试";
            var treeRoots = [];
            var treeModel = Tree._CreateTreeModelFromFlatJson(jsonArray);
            treeModel.forEach(function (itor) {
                return tree.addControl2Model(Tree._CreateTreeNode(itor));
            });
            return tree;
        }
    }, {
        key: "_CreateTreeModelFromFlatJson",
        value: function _CreateTreeModelFromFlatJson(jsonArray) {
            var roots = Tree._FindRootNodes(jsonArray);
            Tree._CompositeNodes(jsonArray, roots);
            return roots;
        }
    }, {
        key: "_FindRootNodes",
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
        key: "_CompositeNodes",
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
        key: "_CreateTreeNode",
        value: function _CreateTreeNode(node) {
            var tNode = new Node();
            tNode.text = node.text;
            tNode.data = node;
            if (node.children && node.children.length > 0) {
                node.children.forEach(function (itor) {
                    return tNode.addControl2Model(Tree._CreateTreeNode(itor));
                });
            }
            return tNode;
        }
    }, {
        key: "_FindRootNodesByRecursive",
        value: function _FindRootNodesByRecursive(jsonArray) {
            var nodes = [];
            var index = Tree._IndexNodeJsonArray(jsonArray);
            jsonArray.forEach(function (itor) {
                if (!!!index[itor.parentId]) nodes.push(itor);
            });
            return nodes;
        }
    }, {
        key: "_FindRootNodesById",
        value: function _FindRootNodesById(jsonArray) {
            var rootId = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "#";

            var nodes = [];
            jsonArray.forEach(function (itor) {
                if (itor.parentId && itor.parentId == rootId) node.push(itor);
            });
            return nodes;
        }
    }, {
        key: "_IndexNodeJsonArray",
        value: function _IndexNodeJsonArray(jsonArray) {
            return Tree._IndexJsonArray(jsonArray, "id");
        }
    }, {
        key: "_IndexJsonArray",
        value: function _IndexJsonArray(jsonArray, indexKey) {
            var index = {};
            jsonArray.forEach(function (itor) {
                return index[itor[indexKey]] = itor;
            });
            return index;
        }
    }]);

    return Tree;
}(Node);

var DraggableControl = function (_Control2) {
    _inherits(DraggableControl, _Control2);

    function DraggableControl() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "";

        _classCallCheck(this, DraggableControl);

        var _this9 = _possibleConstructorReturn(this, (DraggableControl.__proto__ || Object.getPrototypeOf(DraggableControl)).call(this, name, type));

        _this9._dragStart_ = "dragStart." + _get(DraggableControl.prototype.__proto__ || Object.getPrototypeOf(DraggableControl.prototype), "evtNamespace", _this9);
        _this9._draggable_ = new Draggable();
        _this9._menuitemViewId_ = "miv__" + _get(DraggableControl.prototype.__proto__ || Object.getPrototypeOf(DraggableControl.prototype), "id", _this9);
        _this9._menuitemView$_ = null;
        _this9.__onDragStart();
        return _this9;
    }

    _createClass(DraggableControl, [{
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var miv = "<div id=\"" + this.menuViewRootId + "\">";
            miv += this.dragView;
            miv += '</div>';
            return miv;
        }
    }, {
        key: "getDragImage",
        value: function getDragImage() {
            return document.getElementById(this.menuitemViewId);
        }
    }, {
        key: "__onDragStart",
        value: function __onDragStart() {
            var self = this;
            var dragStartEvent = self.dragStartEvent;
            var originalDragStartEvent = self._draggable_.dragStartEvent;
            self._draggable_.on(originalDragStartEvent, function (source, eventArgs) {
                eventArgs.dataTransfer.setData("srcId", self.id);
                self.trigger(dragStartEvent, eventArgs);
                self.triggerHandler(dragStartEvent, { source: self, evtArgs: eventArgs });
            });
        }
    }, {
        key: "dragStartEvent",
        get: function get() {
            return this._dragStart_;
        }
    }, {
        key: "dragView",
        get: function get() {
            return this._draggable_.getView();
        }
    }, {
        key: "menuitemViewId",
        get: function get() {
            return this._menuitemViewId_;
        }
    }, {
        key: "menuitemViewId$",
        get: function get() {
            return this.toJqueryId(this.menuitemViewId);
        }
    }, {
        key: "menuitemView$",
        get: function get() {
            var miv = this._menuitemView$_;
            if (!miv) miv = this._menuitemView$_ = $(this.menuitemViewId$);
            return miv;
        }
    }]);

    return DraggableControl;
}(Control);

var DraggableContainer = function (_Container2) {
    _inherits(DraggableContainer, _Container2);

    function DraggableContainer() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "";

        _classCallCheck(this, DraggableContainer);

        var _this10 = _possibleConstructorReturn(this, (DraggableContainer.__proto__ || Object.getPrototypeOf(DraggableContainer)).call(this, name, type));

        _this10._dragStart_ = "dragStart." + _get(DraggableContainer.prototype.__proto__ || Object.getPrototypeOf(DraggableContainer.prototype), "evtNamespace", _this10);
        _this10._draggable_ = new Draggable();
        _this10._menuitemViewId_ = "miv__" + _get(DraggableContainer.prototype.__proto__ || Object.getPrototypeOf(DraggableContainer.prototype), "id", _this10);
        _this10._menuitemView$_ = null;
        _this10.__onDragStart();
        return _this10;
    }

    _createClass(DraggableContainer, [{
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var miv = "<div id=\"" + this.menuViewRootId + "\">";
            miv += this.dragView;;
            miv += '</div>';
            return miv;
        }
    }, {
        key: "getDragImage",
        value: function getDragImage() {
            return document.getElementById(this.menuitemViewId);
        }
    }, {
        key: "__onDragStart",
        value: function __onDragStart() {
            var self = this;
            var dragStartEvent = self.dragStartEvent;
            var originalDragStartEvent = self._draggable_.dragStartEvent;
            self._draggable_.on(originalDragStartEvent, function (source, eventArgs) {
                eventArgs.dataTransfer.setData("srcId", self.id);
                self.trigger(dragStartEvent, eventArgs);
                self.triggerHandler(dragStartEvent, { source: self, evtArgs: eventArgs });
            });
        }
    }, {
        key: "dragStartEvent",
        get: function get() {
            return this._dragStart_;
        }
    }, {
        key: "dragView",
        get: function get() {
            return this._draggable_.getView();
        }
    }, {
        key: "menuitemViewId",
        get: function get() {
            return this._menuitemViewId_;
        }
    }, {
        key: "menuitemViewId$",
        get: function get() {
            return this.toJqueryId(this.menuitemViewId);
        }
    }, {
        key: "menuitemView$",
        get: function get() {
            var miv = this._menuitemView$_;
            if (!miv) miv = this._menuitemView$_ = $(this.menuitemViewId$);
            return miv;
        }
    }]);

    return DraggableContainer;
}(Container);

var ConfigurableControl = function (_DraggableControl) {
    _inherits(ConfigurableControl, _DraggableControl);

    function ConfigurableControl() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "configurable.draggable";

        _classCallCheck(this, ConfigurableControl);

        var _this11 = _possibleConstructorReturn(this, (ConfigurableControl.__proto__ || Object.getPrototypeOf(ConfigurableControl)).call(this, name, type));

        _this11._evtNamespace_ = "configurable.Draggable." + _get(ConfigurableControl.prototype.__proto__ || Object.getPrototypeOf(ConfigurableControl.prototype), "evtNamespace", _this11);
        _this11._editEvent_ = "edit." + _this11._evtNamespace_;
        _this11._removeEvent_ = "remove." + _this11._evtNamespace_;

        _this11._configurable_ = new Configurable();

        _this11.__onEdit();
        _this11.__onRemove();
        return _this11;
    }

    _createClass(ConfigurableControl, [{
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\" class=\"xvision-container\">";
            v += this.configurableView;
            v += '</div>';
            return v;
        }
    }, {
        key: "__onEdit",
        value: function __onEdit() {
            var configurable = this._configurable_;
            var srcEditEvt = configurable.editEvent;
            var editEvt = this._editEvent_;
            var self = this;
            configurable.on(srcEditEvt, function (source, eventArgs) {
                var evtArgs = Object.assign({ type: editEvt }, eventArgs);
                self.trigger(editEvt, evtArgs);
                self.triggerHandler(editEvt, { source: self, evtArgs: evtArgs });
            });
        }
    }, {
        key: "__onRemove",
        value: function __onRemove() {
            var configurable = this._configurable_;
            var srcRemoveEvt = configurable.removeEvent;
            var removeEvt = this._removeEvent_;
            var self = this;
            configurable.on(srcRemoveEvt, function (source, eventArgs) {
                var evtArgs = Object.assign({ type: removeEvt }, eventArgs);
                self.trigger(removeEvt, evtArgs);
                self.triggerHandler(removeEvt, { source: self, evtArgs: evtArgs });
                self.remove();
            });
        }
    }, {
        key: "configurableView",
        get: function get() {
            return this._configurable_.getView();
        }
    }]);

    return ConfigurableControl;
}(DraggableControl);

var ConfigurableContainer = function (_DraggableContainer) {
    _inherits(ConfigurableContainer, _DraggableContainer);

    function ConfigurableContainer() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
        var type = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : "configurable.draggable";

        _classCallCheck(this, ConfigurableContainer);

        var _this12 = _possibleConstructorReturn(this, (ConfigurableContainer.__proto__ || Object.getPrototypeOf(ConfigurableContainer)).call(this, name, type));

        _this12._evtNamespace_ = "configurable.Draggable." + _get(ConfigurableContainer.prototype.__proto__ || Object.getPrototypeOf(ConfigurableContainer.prototype), "evtNamespace", _this12);
        _this12._editEvent_ = "edit." + _this12._evtNamespace_;
        _this12._removeEvent_ = "remove." + _this12._evtNamespace_;

        _this12._configurable_ = new Configurable();

        _this12.__onEdit();
        _this12.__onRemove();
        return _this12;
    }

    _createClass(ConfigurableContainer, [{
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\" class=\"xvision-container\">";
            v += this.configurableView;
            v += '</div>';
            return v;
        }
    }, {
        key: "__onEdit",
        value: function __onEdit() {
            var configurable = this._configurable_;
            var srcEditEvt = configurable.editEvent;
            var editEvt = this._editEvent_;
            var self = this;
            configurable.on(srcEditEvt, function (source, eventArgs) {
                var evtArgs = Object.assign({ type: editEvt }, eventArgs);
                self.trigger(editEvt, evtArgs);
                self.triggerHandler(editEvt, { source: self, evtArgs: evtArgs });
            });
        }
    }, {
        key: "__onRemove",
        value: function __onRemove() {
            var configurable = this._configurable_;
            var srcRemoveEvt = configurable.removeEvent;
            var removeEvt = this._removeEvent_;
            var self = this;
            configurable.on(srcRemoveEvt, function (source, eventArgs) {
                var evtArgs = Object.assign({ type: removeEvt }, eventArgs);
                self.trigger(removeEvt, evtArgs);
                self.triggerHandler(removeEvt, { source: self, evtArgs: evtArgs });
                self.remove();
            });
        }
    }, {
        key: "configurableView",
        get: function get() {
            return this._configurable_.getView();
        }
    }]);

    return ConfigurableContainer;
}(DraggableContainer);

var ControlType = function (_Container3) {
    _inherits(ControlType, _Container3);

    function ControlType() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var controlType = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, ControlType);

        return _possibleConstructorReturn(this, (ControlType.__proto__ || Object.getPrototypeOf(ControlType)).call(this, name, controlType));
    }

    _createClass(ControlType, [{
        key: "getView",
        value: function getView() {
            var v = "<li id=\"" + this.viewId + "\" class=\"xvision-accordion-catalog\">";
            v += this.getHeader();
            v += this.getBody();
            v += '</li>';
            return v;
        }
    }, {
        key: "getHeader",
        value: function getHeader() {
            var v = '<div class="xvision-accordion-catalog-header">';
            v += this._name_;
            v += '</div>';
            return v;
        }
    }, {
        key: "getBody",
        value: function getBody() {
            var v = "<ul id=\"" + this._containerId_ + "\" class=\"xvision-accordion-catalog-body\">";
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }, {
        key: "getChildView",
        value: function getChildView() {
            var _this14 = this;

            var v = '';
            this.childControls.forEach(function (ctrl) {
                return v += _this14.getAccordinItemView(ctrl);
            });
            return v;
        }
    }, {
        key: "getAccordinItemView",
        value: function getAccordinItemView(control) {
            var v = "<li id=\"" + control.viewId + "\" class=\"xvision-accordin-item\">";
            v += control.getMenuItemView();
            v += '</li>';
            return v;
        }
    }]);

    return ControlType;
}(Container);

var ControlManager = function (_Container4) {
    _inherits(ControlManager, _Container4);

    function ControlManager() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var controlType = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, ControlManager);

        return _possibleConstructorReturn(this, (ControlManager.__proto__ || Object.getPrototypeOf(ControlManager)).call(this, name, controlType));
    }

    _createClass(ControlManager, [{
        key: "registerControl",
        value: function registerControl(control) {
            var type = control.type;
            var ctls = this.controls;
            if (ctls.has(type)) {
                ctls.get(type).addControl(control);
            } else {
                var ct = this.createCatalog(type);
                ct.addControl(control);
                ctls.set(type, ct);
            }
        }
    }, {
        key: "getControlById",
        value: function getControlById(id) {
            var control = null;
            var _iteratorNormalCompletion3 = true;
            var _didIteratorError3 = false;
            var _iteratorError3 = undefined;

            try {
                for (var _iterator3 = this.childControls[Symbol.iterator](), _step3; !(_iteratorNormalCompletion3 = (_step3 = _iterator3.next()).done); _iteratorNormalCompletion3 = true) {
                    var ctrl = _step3.value;

                    control = ctrl.getControlById(id);
                    if (control) return control;
                }
            } catch (err) {
                _didIteratorError3 = true;
                _iteratorError3 = err;
            } finally {
                try {
                    if (!_iteratorNormalCompletion3 && _iterator3.return) {
                        _iterator3.return();
                    }
                } finally {
                    if (_didIteratorError3) {
                        throw _iteratorError3;
                    }
                }
            }
        }
    }, {
        key: "createCatalog",
        value: function createCatalog(name) {
            if (this._controls_.has(name)) {
                throw new Error("栏目已存在");
            }
            return new ControlType(name, null);
        }
    }, {
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this._viewId_ + "\" class=\"xvision-accordion\">";
            v += this.getHeader();
            v += this.getBody();
            v += '</div>';
            return v;
        }
    }, {
        key: "getHeader",
        value: function getHeader() {
            var v = "<div class=\"xvision-accordion-header\">";
            v += this._name_;
            v += '</div>';
            return v;
        }
    }, {
        key: "getBody",
        value: function getBody() {
            var v = "<div class=\"xvision-accordion-body\">";
            v += this.getBodyContainer();
            v += '</div>';
            return v;
        }
    }, {
        key: "getBodyContainer",
        value: function getBodyContainer() {
            var v = "<ul class=\"xvision-accordion-body-container\">";
            v += this.getChildView();
            v += '</ul>';
            return v;
        }
    }]);

    return ControlManager;
}(Container);

var Designer = function (_Container5) {
    _inherits(Designer, _Container5);

    function Designer() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : '';
        var controlType = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

        _classCallCheck(this, Designer);

        return _possibleConstructorReturn(this, (Designer.__proto__ || Object.getPrototypeOf(Designer)).call(this, name, controlType));
    }

    _createClass(Designer, [{
        key: "canDrop",
        value: function canDrop(control) {
            return true;
        }
    }, {
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\" class=\"xvision-container\">";
            v += "<div id=\"" + this.containerId + "\" class=\"xvision-container\"></div>";
            v += '</div>';
            return v;
        }
    }, {
        key: "getModel",
        value: function getModel() {}
    }]);

    return Designer;
}(Container);

var LayoutContainer = function (_ConfigurableContaine) {
    _inherits(LayoutContainer, _ConfigurableContaine);

    function LayoutContainer() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";

        _classCallCheck(this, LayoutContainer);

        return _possibleConstructorReturn(this, (LayoutContainer.__proto__ || Object.getPrototypeOf(LayoutContainer)).call(this, name, "layout"));
    }

    _createClass(LayoutContainer, [{
        key: "getView",
        value: function getView() {
            var v = "<div id=\"" + this.viewId + "\" class=\"xvision-pre-container\">";
            v += this.configurableView;
            v += "<div id=\"" + this.containerId + "\" class=\"xvision-ctrl-container\"></div>";
            v += "</div>";
            return v;
        }
    }, {
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var v = "<div id=\"" + this.menuitemViewId + "\" class=\"xvision-container\">";
            v += '容器';
            v += this.dragView;
            v += '</div>';
            return v;
        }
    }]);

    return LayoutContainer;
}(ConfigurableContainer);

var LayoutRow = function (_ConfigurableContaine2) {
    _inherits(LayoutRow, _ConfigurableContaine2);

    function LayoutRow() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";

        _classCallCheck(this, LayoutRow);

        return _possibleConstructorReturn(this, (LayoutRow.__proto__ || Object.getPrototypeOf(LayoutRow)).call(this, name, "layout"));
    }

    _createClass(LayoutRow, [{
        key: "createNew",
        value: function createNew() {
            return new LayoutRow("", this.type);
        }
    }, {
        key: "getView",
        value: function getView() {
            var v = "<div class=\"xvision-row\">";
            v += this.configurableView;
            v += '行';
            return v;
        }
    }, {
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var v = "<div id=\"" + this.menuitemViewId + "\" class=\"xvision-row\">";
            v += '行';
            v += this.dragView;
            v += '</div>';
            return v;
        }
    }]);

    return LayoutRow;
}(ConfigurableContainer);

var LayoutRow282 = function (_ConfigurableContaine3) {
    _inherits(LayoutRow282, _ConfigurableContaine3);

    function LayoutRow282() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";

        _classCallCheck(this, LayoutRow282);

        return _possibleConstructorReturn(this, (LayoutRow282.__proto__ || Object.getPrototypeOf(LayoutRow282)).call(this, name, "layout"));
    }

    _createClass(LayoutRow282, [{
        key: "createNew",
        value: function createNew() {
            return new LayoutRow282("", this.type);
        }
    }, {
        key: "getView",
        value: function getView() {
            var v = "<div class=\"xvision-row\">";
            v += this.configurableView;
            v += '282';
            v += '</div>';
            return v;
        }
    }, {
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var v = "<div id=\"" + this.menuitemViewId + "\" class=\"xvision-container\">";
            v += '282';
            v += this.dragView;
            v += "</div>";
            return v;
        }
    }]);

    return LayoutRow282;
}(ConfigurableContainer);

var CaptionOne = function (_ConfigurableControl) {
    _inherits(CaptionOne, _ConfigurableControl);

    function CaptionOne() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";

        _classCallCheck(this, CaptionOne);

        return _possibleConstructorReturn(this, (CaptionOne.__proto__ || Object.getPrototypeOf(CaptionOne)).call(this, name, "catalog"));
    }

    _createClass(CaptionOne, [{
        key: "createNew",
        value: function createNew() {
            return new CaptionOne("", this.type);
        }
    }, {
        key: "getView",
        value: function getView() {
            var v = "<div class=\"text\">";
            v += this.configurableView;
            v += '标题1';
            v += '</div>';
            return v;
        }
    }, {
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var v = "<div id=\"" + this.menuitemViewId + "\" class=\"xvision-container\">\u6807\u98981";
            v += this.dragView;
            v += '</div>';
            return v;
        }
    }]);

    return CaptionOne;
}(ConfigurableControl);

var CaptionTwo = function (_ConfigurableControl2) {
    _inherits(CaptionTwo, _ConfigurableControl2);

    function CaptionTwo() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";

        _classCallCheck(this, CaptionTwo);

        return _possibleConstructorReturn(this, (CaptionTwo.__proto__ || Object.getPrototypeOf(CaptionTwo)).call(this, name, "catalog"));
    }

    _createClass(CaptionTwo, [{
        key: "getView",
        value: function getView() {
            var v = "<div class=\"text\">";
            v += this.configurableView;
            v += '标题2';
            v += '</div>';
            return v;
        }
    }, {
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var v = "<div id=\"" + this.menuitemViewId + "\" class=\"xvision-container\">\u6807\u98982";
            v += this.dragView;
            v += '</div>';
            return v;
        }
    }]);

    return CaptionTwo;
}(ConfigurableControl);

var Button = function (_ConfigurableControl3) {
    _inherits(Button, _ConfigurableControl3);

    function Button() {
        var name = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";

        _classCallCheck(this, Button);

        return _possibleConstructorReturn(this, (Button.__proto__ || Object.getPrototypeOf(Button)).call(this, name, "button"));
    }

    _createClass(Button, [{
        key: "getView",
        value: function getView() {
            var v = "<button class=\"btn\">\u6309\u94AE</button>";
            return v;
        }
    }, {
        key: "getMenuItemView",
        value: function getMenuItemView() {
            var v = "<div id=\"" + this.menuitemViewId + "\" class=\"xvision-container\">\u6309\u94AE";
            v += this.dragView;
            v += '</div>';
            return v;
        }
    }]);

    return Button;
}(ConfigurableControl);