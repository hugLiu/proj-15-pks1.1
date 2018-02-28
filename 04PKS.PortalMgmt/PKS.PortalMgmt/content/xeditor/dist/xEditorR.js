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

class XMap {

    static _Create() {
        return new XMap();
    }

    constructor() {
        this._keys_ = [];
        this._map_ = new Map();
    }

    has(key) {
        return this._map_.has(key);
    }

    get(key) {
        return this._map_.get(key);
    }

    getFirstKey() {
        return this._keys_[0];
    }

    getLastKey() {
        return this._keys_[this._keys_.length - 1];
    }

    getKeyBefore(key) {
        let i = this.findKey(key);
        return this._keys_[--i];
    }

    getKeyAfter(key) {
        let i = this.findKey(key);
        return this._keys_[++i];
    }

    getFirst() {
        const fKey = this.getFirstKey();
        return this._map_.get(fKey);
    }

    getLast() {
        const lKey = this.getLastKey();
        return this._map_.get(lKey);
    }

    getBefore(key) {
        const bKey = this.getKeyBefore(key);
        return this._map_.get(bKey);
    }

    getAfter(key) {
        const aKey = this.getKeyAfter(key);
        return this._map_.get(aKey);
    }

    set(key, value) {
        this._map_.set(key, value);
    }

    add(key, value) {
        if (this._map_.has(key)) this.__removeKey(key);
        this._keys_.push(key);
        this._map_.set(key, value);
    }

    addBefore(key, value, bKey) {
        const m = this._map_;
        if (m.has(key)) this.__removeKey(key);
        const i = this.findKey(bKey);
        this._keys_.splice(i, 0, key);
        m.set(key, value);
    }

    addAfter(key, value, aKey) {
        const m = this._map_;
        if (m.has(key)) this.__removeKey(key);
        let i = this.findKey(aKey);
        this._keys_.splice(++i, 0, key);
        this._map_.set(key, value);
    }

    remove(key) {
        if (this._map_.has(key)) {
            this.__removeKey(key);
            this._map_.delete(key);
        }
    }

    removeByVal(obj) {
        const key = this.findValue(obj);
        this.remove(key);
    }

    clear() {
        this._keys_ = [];
        this._map_.clear();
    }

    keys() {
        return this._keys_;
    }

    values() {
        const values = [];
        const m = this._map_;
        this.keys().forEach(k => values.push(m.get(k)));
        return values;
    }

    findKey(key) {
        const keys = this._keys_;
        for (let i = keys.length - 1; i >= 0; i--) {
            if (key === keys[i]) return i;
        }
        throw new Error(`${key} not found`);
    }

    findValue(obj) {
        let key = void 0;
        const m = this._map_;
        for (const iterator of m) {
            if (obj === iterator[1]) {
                key = iterator[0];
                break;
            }
        }
        return key;
    }

    __removeKey(key) {
        const i = this.findKey(key);
        this._keys_.splice(i, 1);
    }
}

class XEventArgs {

    static _CreateFromEvent$(e$, eventType) {
        return new XEventArgs(e$.originalEvent, eventType);
    }

    static _CreateFrom(xEventArgs, eventType) {
        return new XEventArgs(xEventArgs.event, eventType);
    }

    static _CreateFromEvent(event, eventType) {
        return new XEventArgs(event, eventType);
    }

    constructor(event = void 0, type = '') {
        this._timestamp_ = new Date().valueOf();
        this._handled_ = false;
        this._path_ = [];
        this._event_ = event;
        this._type_ = type;
        this._data_ = new Map();
    }

    get handled() {
        return this._handled_;
    }

    set handled(b) {
        this._handled_ = b;
    }

    get path() {
        return this._path_;
    }

    get type() {
        return this._type_;
    }

    set type(type) {
        this._type_ = type;
    }

    get event() {
        return this._event_;
    }

    set event(event) {
        this._event_ = event;
    }

    get data() {
        return this._data_;
    }

    set data(data) {
        this._data_ = data;
    }

    get srcTarget() {
        return this.path[0];
    }

    get curTarget() {
        return this.path[this.path.length - 1];
    }

    addPath(source) {
        this.path.push(source);
    }

    addData(key, value) {
        this._data_.add(key, value);
    }
    removeData(key) {
        this._data_.delete(key);
    }

    getData(key) {
        this._data_.get(key);
    }

    dispatch(source) {
        if (source.trigger && typeof source.trigger == 'function') {
            this.addPath(source);
            source.trigger(this.type, this);
        }
    }

    dispose() {
        this._path_ = [];
        this._data_.clear();
    }
}

class HandlerMetaData {

    static _Create(handler = () => void 0, context = void 0, isOnce = false, isTriggered = false) {
        if (typeof handler != 'function') throw new Error("handler不是函数");
        return new HandlerMetaData(handler, context, isOnce, isTriggered);
    }

    constructor(handler = () => void 0, context = null, isOnce = false, isTriggered = false) {
        this._handler_ = handler;
        this._context_ = context;
        this._isOnce_ = isOnce;
        this._isTriggered_ = isTriggered;
    }

    get hander() {
        return this._handler_;
    }

    set hander(handler) {
        if (typeof handler != 'function') throw new Error("handler不是函数");
        this._handler_ = handler;
    }

    get context() {
        return this._context_;
    }

    set context(context) {
        this._context_ = context;
    }

    get isOnce() {
        return this._isOnce_;
    }

    set isOnce(isOnce) {
        this._isOnce_ = isOnce;
    }

    get isTriggered() {
        return this._isTriggered_;
    }

    set isTriggered(isTriggered) {
        this._isTriggered_ = isTriggered;
    }

    invoke(source, eventArgs) {
        if (!(this._isOnce_ && this._isTriggered_)) {
            if (!!this._context_) {
                Reflect.apply(this._handler_, this._context_, [source, eventArgs]);
            } else {
                this._handler_(source, eventArgs);
            }
            this._isTriggered_ = true;
        }
    }

}

class XEvent {

    static _Create() {
        return new XEvent();
    }

    constructor() {
        this._timestamp_ = new Date().valueOf();
        this._eventNamespace_ = "xWander";
        this._handlerMetadataDic_ = new XMap();
    }

    get timestamp() {
        return this._timestamp_;
    }

    get eventNamespace() {
        return this._eventNamespace_;
    }

    on(eventType, handler, context) {
        const handlerMetadata = HandlerMetaData._Create(handler, context);
        this.__addHandler(eventType, handlerMetadata);
    }

    once(eventType, handler, context) {
        const once = true;
        const triggered = false;
        const handlerMetadata = HandlerMetaData._Create(handler, context, once, triggered);
        this.__addHandler(eventType, handlerMetadata);
    }

    trigger(eventType, eventArgs) {
        const source = this;
        const handlersMetadatas = source._handlerMetadataDic_.get(eventType);
        if (handlersMetadatas) handlersMetadatas.forEach(handlerMetaData => handlerMetaData.invoke(source, eventArgs));
    }

    un(eventType, handler) {
        const handlerMetadatas = this._handlerMetadataDic_.get(eventType);
        let handlerMetadata = void 0;
        for (let i = 0, j = handlerMetadatas.length; i < j; i++) {
            handlerMetadata = handlerMetadatas[i];
            if (handlerMetadata.hander === handler) {
                handlerMetadatas.splice(i, 1);
                break;
            }
        }
    }

    remove() {
        this._handlerMetadataDic_.clear();
    }

    __addHandler(eventType, handlerMetadata) {
        const handlerMetadataDic = this._handlerMetadataDic_;
        if (handlerMetadataDic.has(eventType)) {
            handlerMetadataDic.get(eventType).push(handlerMetadata);
        } else {
            handlerMetadataDic.add(eventType, [handlerMetadata]);
        }
    }

}

class XObject extends XEvent {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "XObject";
    }

    static _Is(obj, cls) {
        let itr = obj;
        do {
            if (itr instanceof cls) return true;
        } while (itr = itr.prototype);
        return false;
    }

    static _Guid() {
        let guid = "";
        for (let i = 0; i < 32; i++) {
            guid += Math.floor(Math.random() * 16.0).toString(16);
            if (i == 8 || i == 12 || i == 16 || i == 20) guid += '-';
        }
        return guid;
    }

    static _Create(id = '', name = '') {
        return new XObject(id, name);
    }

    constructor(id = '', name = '') {
        super();

        this._instanceId_ = this.constructor.__InstanceId() || 0;

        this._id_ = id;
        this._name_ = name;
    }

    get className() {
        return this.constructor.__ClassName_ || 'Object';
    }

    get instanceId() {
        return this._instanceId_;
    }

    get id() {
        return this._id_ || (this._id_ = XObject._Guid());
    }

    set id(id) {
        this._id_ = id;
    }

    get name() {
        return this._name_ || (this._name_ = `${this.className}${this.instanceId}`);
    }

    set name(name) {
        this._name_ = name;
    }

    createNew() {
        return new this.constructor();
    }

    equals(xObj) {
        let eq = false;
        if (xObj.id && this.id == xObj.id) eq = true;
        if (xObj.instanceId && this.instanceId == xObj.instanceId) eq = true;
        return eq;
    }

    is(cls) {
        return XObject._Is(this, cls);
    }

    clone(obj) {
        return JSON.parse(JSON.stringify(obj));
    }
}

class Component extends XObject {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Component";
    }

    static get __Version() {
        return "0.5.300.88888";
    }

    static _Create(enviroment = {}, id = '', name = '') {
        return new Component(enviroment, id, name);
    }

    constructor(enviroment = {}, id = '', name = '') {
        super(id, name);

        this._enviroment_ = enviroment;
    }

    get version() {
        return Component.__Version;
    }

    get enviroment() {
        return this._enviroment_;
    }

    set enviroment(enviroment) {
        this._enviroment_ = enviroment;
    }

}

class Node extends Component {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Node";
    }

    static _Create(parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Node(parent, pre, next, enviroment, id, name);
    }

    constructor(parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(enviroment, id, name);

        this._children_ = new XMap();

        this._parent_ = parent;
        this._pre_ = pre;
        this._next_ = next;
    }

    get parent() {
        return this._parent_;
    }

    set parent(node) {
        node && node.addChild(this);
    }

    get pre() {
        return this._pre_;
    }

    set pre(node) {
        this._pre_ = node;
        node && node.setNext(this);
    }

    get next() {
        return this._next_;
    }

    set next(node) {
        this._next_ = node;
        node && node.setPre(this);
    }

    get children() {
        return this._children_.values();
    }

    get firstChild() {
        return this._children_.getFirst();
    }

    get lastChild() {
        return this._children_.getLast();
    }

    hasChild(node) {
        return this.hasChild_(node.id);
    }

    hasChild_(key) {
        return this._children_.has(key);
    }

    getChild(key) {
        return this._children_.get(key);
    }

    addToChildren(key, node) {
        this._children_.add(key, node);
    }

    addToChildren_(node) {
        this._children_.add(node.id, node);
    }

    setParent(node) {
        this._parent_ = node;
    }

    setPre(node) {
        this._pre_ = node;
    }

    setNext(node) {
        this._next_ = node;
    }

    addChild(node) {
        this.addChild_(node.id, node);
    }

    addChild_(key, node) {
        const lNode = this.lastChild;
        node.pre = lNode;
        this.addToChildren(key, node);
        node.setParent(this);
    }

    addChildBefore(node, bNode) {
        this.addChildBefore_(node.id, node, bNode.id);
    }

    addChildBefore_(key, node, bKey) {
        const cs = this._children_;
        if (cs.has(bKey)) {
            const pre = cs.getBefore(bKey);
            if (pre) pre.next = node;
            const next = cs.get(bKey);
            if (next) next.pre = node;
            cs.addBefore(key, node, bKey);
            node.setParent(this);
        }
    }

    addChildAfter(node, aNode) {
        this.addChildAfter_(node.id, node, aNode.id);
    }

    addChildAfter_(key, node, aKey) {
        const cs = this._children_;
        if (cs.has(aKey)) {
            const pre = cs.get(aKey);
            if (pre) pre.next = node;
            const next = cs.getAfter(aKey);
            if (next) next.pre = node;
            cs.addAfter(key, node, aKey);
            node.setParent(this);
        }
    }

    removeChild(node) {
        this.removeChild_(node.id);
    }

    removeChild_(key) {
        const cNode = this._children_.get(key);
        if (cNode) cNode.remove();
    }

    removeFromChildren(node) {
        this.removeFromChildren_(node.id);
    }

    removeFromChildren_(key) {
        this._children_.remove(key);
    }

    remove() {
        super.remove();
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

    findAncestor() {
        let node = this;
        while (node.parent) {
            node = node.parent;
        }
        return node;
    }

    isAncesstor() {
        return !node.parent;
    }

}

class UIElement extends Node {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "UIElement";
    }

    static get __DomParser_() {
        if (!this.domParser) {
            this.domParser = new DOMParser();
        }
        return this.domParser;
    }

    static _IsHtmlElement(obj) {
        return XObject._Is(obj, HTMLElement);
    }

    static _ToCssId(elementId) {
        let jId = elementId;
        if (!UIElement._IsCssId(elementId)) jId = `#${jId}`;
        return jId;
    }

    static _IsCssId(elementId) {
        return UIElement._IsCssIdCommon(elementId, s => /^#\w+/g.test(s));
    }

    static _IsCssIdCommon(elementId, predicate) {
        if (!UIElement._IsPredicate(predicate)) throw new TypeError("请传入一个合法的谓词");
        return predicate(elementId);
    }

    static _IsPredicate(predicate) {
        return typeof predicate === 'function';
    }

    static _JoinClassName(classArray) {
        return classArray.join(" ");
    }

    static _Create(contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new UIElement(parent, pre, next, enviroment, id, name);
    }

    constructor(contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(parent, pre, next, enviroment, id, name);

        this._contextmenu_ = contextmenu;
    }

    get document() {
        return window.document;
    }

    get domParser() {
        return UIElement.__DomParser_;
    }

    get viewId() {
        return `v__${this.id}`;
    }

    get view$() {
        return this.byId(this.viewId);
    }

    get isInDocument() {
        return !!this.view$;
    }

    get clickETN() {
        return `click.${this.eventNamespace}`;
    }

    get contextmenuETN() {
        return `contextmenu.${this.eventNamespace}`;
    }

    get contextmenu() {
        return this._contextmenu_;
    }

    set contextmenu(contextmenu) {
        this._contextmenu_ = contextmenu;
    }

    byId(id) {
        return this.document.getElementById(id);
    }

    mount(elementId) {
        const ele = this.byId(elementId);
        if (ele) {
            if (this.isInDocument) this.destroyView();
            const view = this.getView();
            ele.innerHTML = view;
            this.initialize();
        }
    }

    addChild_(key, ele) {
        super.addChild_(key, ele);
        this.mountChild_(ele);
    }

    mountChild_(ele) {
        if (this.isInDocument) {
            const v = ele.getView();
            this.view$.insertAdjacentHTML('beforeend', v);
            ele.initialize();
        }
    }

    addChildBefore_(key, ele, bKey) {
        super.addChildBefore_(key, ele, bKey);
        this.mountChildBefore_(key, ele, bKey);
    }

    mountChildBefore_(key, ele, bKey) {
        if (this.isInDocument && this.hasChild_(bKey)) {
            const bEle = this.getChild(bKey);
            const v = ele.getView();
            bEle.view$.insertAdjacentHTML('beforebegin', v);
            ele.initialize();
        }
    }

    destroyView() {
        const v = this.view$;
        v.parentNode.removeChild(v);
    }

    remove() {
        super.remove();
        this.destroyView();
    }

    isChildView(cview) {
        return this.isChildView_(this.view$, cview);
    }

    isChildView_(view, cview) {
        let b = false;
        if (!view || !cview) return b;
        if (view === cview) {
            return b;
        }
        let p = cview.parentNode;
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

    on$(eventType, id, handler) {
        var te = this.byId(id);
        if (te) {
            te.addEventListener(eventType, handler, false);
        }
        return this;
    }

    joinClassName(classArray) {
        return UIElement._JoinClassName(classArray);
    }

    toCssId(elementId) {
        return UIElement._ToCssId(elementId);
    }

    routeEvent(xEventArgs) {
        var cur = this;
        do {
            xEventArgs.dispatch(cur);
            cur = cur.parent;
        } while (cur && !xEventArgs.handled);
    }

    routeEventX(eventType) {
        const xEventArgs = new XEventArgs();
        xEventArgs.type = eventType;
        this.routeEvent(xEventArgs);
    }

    showContextmenu(pageX, pageY) {
        this.showContextmenu_(this, pageX, pageY);
    }

    showContextmenu_(target, pageX, pageY) {
        this.contextmenu && this.contextmenu.show && this.contextmenu.show(target, pageX, pageY);
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-margin-ver-md xvision-border" style="position:relative">`;
        v += this.getSelfView();
        v += this.getChildView();
        v += '</div>';
        return v;
    }

    getSelfView() {
        return this.name;
    }

    getChildView() {
        let v = '';
        this.children.forEach(ele => v += ele.getView());
        return v;
    }

    onClick_(ele$) {
        const self = this;
        this.on$("click", ele$, function (e) {
            self.handleClick_(e);
        });
    }

    handleClick_(e) {
        this.dispatchEvent(e, this.clickETN);
    }

    dispatchEvent(e, eventType) {
        e.stopPropagation();
        const xEventArgs = XEventArgs._CreateFromEvent(e, eventType);
        this.routeEvent(xEventArgs);
    }

    onContextmenu_(ele$) {
        const self = this;
        this.on$("contextmenu", ele$, function (e) {
            self.handleContextmenu_(e);
        });
    }

    handleContextmenu_(e) {}

    findChildNearestAfterPixel(ox, oy, diff, view) {
        const v = view || this.view$;
        const sx = v.scrollLeft;
        const sy = v.scrollTop;
        diff = diff || 15;
        const eles = this.children;
        let ele = void 0;
        for (let i = 0, j = eles.length; i < j; i++) {
            ele = eles[i];
            if (this.isBeforePixel(ele.view$, ox, oy, diff, sx, sy)) return ele;
        }
        return void 0;
    }

    isBeforePixel(ele, ox, oy, diff, sx, sy) {
        let oTop = ele.offsetTop;
        let oLeft = ele.offsetLeft;
        let oWidth = ele.offsetWidth;
        let y = oTop - sy - oy;
        return y >= 0 && y <= diff && ox >= diff + oLeft - sx && ox <= diff + oWidth - sx;
    }

    addClass(view, className) {
        if (view && view.classList) {
            view.classList.add(className);
        }
    }

    removeClass(view, className) {
        if (view && view.classList) {
            view.classList.remove(className);
        }
    }

    hasClass(view, className) {
        let bl = false;
        if (view && view.classList) {
            bl = view.classList.contains(className);
        }
        return bl;
    }

    initialize() {
        this.initializeC();
        //this.__onClick();
        //this.__onContextmenu();
    }

    initializeC() {
        this.children.forEach(ele => ele.initialize());
    }

    __onClick() {
        this.onClick_(this.viewId);
    }

    __onContextmenu() {
        this.onContextmenu_(this.viewId);
    }

}

class EsComModel {

    static _FromEsModel(esm) {
        esm = esm || {};
        return new EsComModel(esm.query, esm.sort, esm.size);
    }

    constructor(query, sort, size) {

        this._query_ = this.initQuery(query);

        this._sort_ = this.initSort(sort);

        this._size_ = this.initSize(size);
    }

    get query() {
        return this._query_;
    }

    get sort() {
        return this._sort_;
    }

    get size() {
        return this._size_;
    }

    set size(s) {
        this._size_ = s;
    }

    initQuery(query) {
        const q = {};
        let o;
        for (let n in query) {
            o = query[n];
            q[n] = this.gQuery(n, Object.values(o)[0], Object.keys(o)[0]);
        }
        return q;
    }

    gQuery(name, value, op) {
        return { name, value, op };
    }

    initSort(sort) {
        return JSON.parse(JSON.stringify(sort || {}));
    }

    initSize(size) {
        return size || '';
    }

    setQuery(name, value, op) {
        if (this.hasQuery(name)) {
            this.modifyQuery_(name, value, op);
        } else {
            this.addQuery_(name, value, op);
        }
    }

    hasQuery(name) {
        return !!this._query_[name];
    }

    modifyQuery_(name, value, op) {
        const q = this._query_[name];
        value && (q.value = value);
        op && (q.op = op);
    }

    addQuery_(name, value, op) {
        this._query_[name] = { name, value, op };
    }

    removeQuery(name) {
        delete this._query_[name];
    }

    setSort(name, value) {
        this._sort_[name] = value;
    }

    removeSort(name) {
        delete this._sort_[name];
    }

    setSize(sz) {
        this._size_ = sz;
    }

    toEsModel() {
        const q = this.toEsQuery();
        const s = this._sort_;
        const sz = this._size_;
        return { query: q, sort: s, size: sz };
    }

    toEsQuery() {
        const q = Object.values(this._query_);
        const m = {};
        let o, om;
        for (var i = 0, j = q.length; i < j; i++) {
            om = q[i];
            o = {};
            o[om.op] = om.value;
            m[om.name] = o;
        }
        return m;
    }
}

class ComEditor extends UIElement {

    static _Create(esMeta, contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new ComEditor(esMeta, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(esMeta, contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(contextmenu, parent, pre, next, enviroment, id, name);

        this._esMeta_ = esMeta;

        this._com_ = void 0;

        this._title_ = void 0;

        this._argModel_ = void 0;

        this._queryModel_ = void 0;

        this._htmlText_ = void 0;

        this._htmlEditor_ = void 0;
    }

    get com() {
        return this._com_;
    }

    set com(com) {
        this._com_ = com;
        this._title_ = com.text;
        this._argModel_ = com.args;
        this._queryModel_ = EsComModel._FromEsModel(com.query);
        this._htmlText_ = com.htmlText;
    }

    get esMeta() {
        return this._esMeta_;
    }

    set esMeta(esMeta) {
        this._esMeta_ = esMeta;
    }

    get title() {
        return this._title_;
    }

    get argModel() {
        return this._argModel_;
    }

    get queryModel() {
        return this._queryModel_;
    }

    get saveId() {
        return `sav__${this.id}`;
    }

    get save$() {
        return this.byId(this.saveId);
    }

    get cancelId() {
        return `cel__${this.id}`;
    }

    get cancel$() {
        return this.byId(this.cancelId);
    }

    get queryTableId() {
        return `qt__${this.id}`;
    }

    get queryTable$() {
        return this.byId(this.queryTableId);
    }

    get sortTableId() {
        return `st__${this.id}`;
    }

    get sortTable$() {
        return this.byId(this.sortTableId);
    }

    get sizeId() {
        return `sz__${this.id}`;
    }

    get size$() {
        return this.byId(this.sizeId);
    }

    get titleId() {
        return `ttl__${this.id}`;
    }

    get title$() {
        return this.byId(this.titleId);
    }

    get htmlEditorId() {
        return `her__${this.id}`;
    }

    get hideId() {
        return `hie__${this.id}`;
    }

    get hide$() {
        return this.byId(this.hideId);
    }

    get argTableId() {
        return `arg__${this.id}`;
    }

    get argTable$() {
        return this.byId(this.argTableId);
    }

    show(com) {
        this.com = com;
        this.show_();
        this.init();
    }

    show_() {
        if (!this.isInDocument) {
            $("body").append(this.getView());
            this.initialize();
        }
    }

    init() {
        this.initQuery();
        this.initSort();
        this.initReturn();
        if (this.com.metadata.hastexttemplate) {
            this.initHtmlEditor();
        }
    }

    initQuery() {
        const q = this.queryModel.query;
        let o, name, title, op, value;
        for (let key in q) {
            o = q[key];
            name = key;
            title = this.getEsMetaByName(name).title;
            op = o.op;
            value = o.value;
            this.addQueryRow(name, title, op, value);
        }
    }

    initSort() {
        const s = this.queryModel.sort;
        let name, title, value;
        for (let key in s) {
            name = key;
            title = this.getEsMetaByName(name).title;
            value = s[key];
            this.addSortRow(name, title, value);
            this.removeSortSelect(name);
        }
    }

    removeSortSelect(name) {
        const opt = this.sortTable$.querySelector(`tr:last-child>td:first-child>select>option[value="${name}"]`);
        opt.parentNode.removeChild(opt);
    }

    initReturn() {
        this.size$.value = this.queryModel.size;
    }

    initHtmlEditor() {
        var self = this;
        this._htmlEditor_ = UE.getEditor(this.htmlEditorId);
        this._htmlEditor_.ready(function () {
            self._htmlEditor_.setContent(self._htmlText_ || "");
        });
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-com-editor">`;
        v += `<div class="xvision-window">`;
        v += `<div class="xvision-com-editor-core">`;
        v += this.getHeader();
        v += this.getBody();
        v += this.getFooter();
        v += '</div>';
        v += '</div>';
        v += '</div>';
        return v;
    }
    getHeader() {
        let v = `<div class="xvision-com-editor-core-header">`;
        v += '<div class="xvision-com-editor-core-header-container">';
        v += '<div class="xvision-row">';
        v += '<div class="xvision-col-6">';
        v += '设置文本属性';
        v += '</div>';
        v += '<div class="xvision-col-6 xvision-text-right">';
        v += `<i id="${this.hideId}" class="xvision-tool-btn glyphicon glyphicon-remove"></i>`;
        v += '</div>';
        v += '</div>';
        v += '</div>';
        v += '</div>';
        return v;
    }

    getBody() {
        let v = `<div class="xvision-com-editor-core-body">`;
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

    getCaption() {
        let v = '<div class="xvision-panel">';
        v += '<div class="xvision-panel-header">';
        v += `<h3>标题</h3>`;
        v += '</div>';
        v += `<div class="xvision-panel-body">`;
        v += `<input id="${this.titleId}" type="text" value="${this.title}"/>`;
        v += '</div>';
        v += '</div>';
        return v;
    }

    getText() {
        let v = '<div class="xvision-panel">';
        v += `<div class="xvision-panel-header">`;
        v += `<h3>编辑文本</h3>`;
        v += '</div>';
        v += `<div class="xvision-panel-body">`;
        v += this.getTextEditor();
        v += '</div>';
        v += '</div>';
        return v;
    }

    getTextEditor() {
        return `<script id="${this.htmlEditorId}" type="text/plain" style="width:100%;height:160px;"></script>`;
    }

    getParams() {
        let v = '<div class="xvision-panel">';
        v += `<div class="xvision-panel-header">`;
        v += `<h3>参数设置</h3>`;
        v += `</div>`;
        v += `<div class="xvision-panel-body">`;
        v += this.getTab();
        v += '</div>';
        v += '</div>';
        return v;
    }

    getTab() {
        let v = `<div class="xvision-tab">`;
        v += this.getTabHeader();
        v += this.getTabBody();
        v += '</div>';
        return v;
    }

    getTabHeader() {
        let v = `<div class="xvision-tab-header">`;
        v += `<div class="xvision-tab-btn active">查询参数</div>`;
        v += `<div class="xvision-tab-btn">组件参数</div>`;
        v += '</div>';
        return v;
    }

    getTabBody() {
        let v = '<div class="xvision-tab-body">';
        v += this.getQueryParmasTP();
        v += this.getArgsParamsTP();
        v += '</div>';
        return v;
    }

    getQueryParmasTP() {
        let v = '<div class="xvision-tab-page active" data-anchor="查询参数">';
        v += this.getQueryParmas();
        v += '</div>';
        return v;
    }

    getArgsParamsTP() {
        let v = '<div class="xvision-tab-page" data-anchor="组件参数">';
        v += this.getArgsParams();
        v += '</div>';
        return v;
    }

    getQueryParmas() {
        let v = this.getQuery();
        v += this.getSort();
        return v;
    }

    getQuery() {
        let v = '<div class="xvision-panel">';
        v += '<div class="xvision-panel-header">';
        v += '<h3>查询</h3>';
        v += `</div>`;
        v += `<div class="xvision-panel-body">`;
        v += this.getQueryTable();
        v += '</div>';
        v += '</div>';
        return v;
    }

    getSort() {
        let v = '<div class="xvision-panel">';
        v += '<div class="xvision-panel-header">';
        v += '<h3>排序</h3>';
        v += `</div>`;
        v += `<div class="xvision-panel-body">`;
        v += this.getSortTable();
        v += this.getReturnRecord();
        v += '</div>';
        v += '</div>';
        return v;
    }

    getQueryTable() {
        let v = `<table id="${this.queryTableId}" class="xvision-table query">`;
        v += `<thead><th>参数名</th><th>类型</th><th>操作符</th><th>值</th><th></th></thead>`;
        v += `<tbody>`;
        v += this.getQueryMetaRow();
        v += '</tbody>';
        v += '</table>';
        return v;
    }

    getSortTable() {
        let v = `<table id="${this.sortTableId}" class="xvision-table sort">`;
        v += `<thead><th>参数名</th><th>排序</th><th></th></thead>`;
        v += `<tbody>`;
        v += this.getSortMetaRow();
        v += '</tbody>';
        v += '</table>';
        return v;
    }

    getQueryMetaRow() {
        let v = `<tr>`;
        v += `<td>` + this.getQueryOpt() + `</td>`;
        v += '<td>' + '</td>';
        v += '<td>' + '</td>';
        v += '<td>' + '</td>';
        v += '<td>' + '</td>';
        v += '</tr>';
        return v;
    }

    getSortMetaRow() {
        let v = `<tr>`;
        v += `<td>` + this.getQueryOpt() + `</td>`;
        v += '<td>' + '</td>';
        v += '<td>' + '</td>';
        v += '</tr>';
        return v;
    }

    addQueryRow(name, title, op, value) {
        const self = this;
        const v = this.cosQueryRow(name, title, op, value);
        const tb = this.queryTable$.querySelector("tbody");
        tb.insertAdjacentHTML("afterbegin", v);
        const ov = tb.querySelector("tr:first-child");
        const dio = ov.querySelector(".xvision-radio");
        dio && (dio.onclick = function (e) {
            self.handleQueryRadio_(e);
        });
        const sel = ov.querySelector("select");
        sel && (sel.onchange = function (e) {
            self.handleQueryOp_(e);
        });
        const ipt = ov.querySelector("input[type=text]");
        ipt && (ipt.onblur = function (e) {
            self.handleQueryValue_(e);
        });
        const rm = ov.querySelector(".xvision-com-editor-rm");
        rm && (rm.onclick = function (e) {
            self.handleRemoveQuery(e);
        });
    }

    handleQueryRadio_(e) {
        const tar = e.target;
        const text = tar.nextSibling.data;
        const row = e.currentTarget.parentNode.parentNode;
        const v = row.querySelector("input[type=text]");
        if (text == "变量") {
            v.value = `@${row.getAttribute("name")}`;
            v.disabled = true;
        } else {
            v.disabled = false;
        }
    }

    handleQueryOp_(e) {
        const tar = e.currentTarget;
        const name = tar.parentNode.parentNode.getAttribute("name");
        const v = tar.value;
        this.queryModel.setQuery(name, null, v);
    }

    handleQueryValue_(e) {
        const tar = e.currentTarget;
        const name = tar.parentNode.parentNode.getAttribute("name");
        const v = tar.value;
        this.queryModel.setQuery(name, v);
    }

    handleRemoveQuery(e) {
        const row = e.currentTarget.parentNode.parentNode;
        const name = row.getAttribute("name");
        this.queryModel.removeQuery(name);
        row.parentNode.removeChild(row);
    }

    cosQueryRow(name, title, op, value) {
        let v = `<tr name="${name}">`;
        v += `<td>` + title + `</td>`;
        v += '<td>' + this.getRadio(`${name}.type`, ["常量", "变量"], /^@\w/g.test(value) ? 1 : 0) + '</td>';
        v += '<td class="query-op">' + this.getOperator(op) + '</td>';
        v += '<td>' + `<input type="text" value="${value}"/>` + '</td>';
        v += '<td>' + this.getRmButton() + '</td>';
        v += '</tr>';
        return v;
    }

    addSortRow(name, title, value) {
        const self = this;
        const v = this.cosSortRow(name, title, value);
        const tb = this.sortTable$.querySelector("tbody");
        tb.insertAdjacentHTML('afterbegin', v);
        const ov = tb.querySelector("tr:first-child");
        const dio = ov.querySelector(".xvision-radio");
        dio && (dio.onclick = function (e) {
            self.handleSort(e);
        });
        const rm = ov.querySelector(".xvision-com-editor-rm");
        rm && (rm.onclick = function (e) {
            self.handleRemoveSort(e);
        });
        this.removeSortSelect(name);
    }

    handleSort(e) {
        const txt = e.target.nextSibling.data;
        const name = e.currentTarget.parentNode.parentNode.getAttribute("name");
        this.queryModel.setSort(name, txt == '升序' ? "asc" : "dsc");
    }

    handleRemoveSort(e) {
        const row = e.currentTarget.parentNode.parentNode;
        const name = row.getAttribute("name");
        this.queryModel.removeSort(name);
        const tb = row.parentNode;
        const sel = tb.querySelector("tr:last-child select");
        const esm = this.getEsMetaByName(name);
        const v = `<option value="${esm.name}">${esm.title}</option>`;
        sel.insertAdjacentHTML('beforeend', v);
        row.parentNode.removeChild(row);
    }

    cosSortRow(name, title, value) {
        let v = `<tr name="${name}">`;
        v += `<td>` + title + `</td>`;
        v += '<td>' + this.getRadio(`${name}.type`, ["升序", "降序"], value == 'asc' ? 0 : 1) + '</td>';
        v += '<td>' + this.getRmButton() + '</td>';
        v += '</tr>';
        return v;
    }

    getQueryOpt() {
        const qs = Object.values(this.esMeta);
        const kvArray = [];
        qs.forEach(q => kvArray.push({ key: q.name, value: q.title }));
        return this.getSelect("选择参数", kvArray);
    }

    getReturnRecord() {
        let v = '<div>';
        v += '<span>返回记录数</span>';
        v += `<input id="${this.sizeId}" type="text" />`;
        v += '<span>(为空返回所有数据)</span>';
        v += '</div>';
        return v;
    }

    getArgsParams() {
        let v = `<table id="${this.argTableId}" class="xvision-table arg">`;
        v += `<thead><th>名称</th><th>值</th></thead>`;
        v += `<tbody>`;
        v += this.getArgMetas();
        v += `</tbody>`;
        v += '</table>';
        return v;
    }

    getArgMetas() {
        const self = this;
        let arms = this.argModel;
        let v = '',
            o = void 0;
        for (let key in arms) {
            o = arms[key];
            v += this.addArgRow(o.code, o.name, o.value || o.defaultvalue || '');
        }
        return v;
    }

    addArgRow(code, name, value) {
        let v = `<tr name="${code}">`;
        v += `<td>${name}</td>`;
        v += `<td><input type="text" value="${value}" /></td>`;
        v += `</tr>`;
        return v;
    }

    getFooter() {
        let v = '<div class="xvision-com-editor-core-footer">';
        v += '<div class="xvision-com-editor-core-footer-container">';
        v += `<button id="${this.saveId}">保存</button>`;
        v += `<button id="${this.cancelId}">取消</button>`;
        v += '</div>';
        v += `</div>`;
        return v;
    }

    getRadio(name, values, def = 0) {
        let v = '<span class="xvision-radio">';
        if (def == 0) {
            v += `<span><input type="radio" name="${name}" checked="true"/>${values[0]}</span>`;
            v += `<span><input type="radio" name="${name}" />${values[1]}</span>`;
        } else {
            v += `<span><input type="radio" name="${name}" />${values[0]}</span>`;
            v += `<span><input type="radio" name="${name}" checked="true"/>${values[1]}</span>`;
        }
        v += '</span>';
        return v;
    }

    initialize() {
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

    __onTitleChange() {
        const self = this;
        this.on$('blur', this.titleId, function (e) {
            self.handleTitleChange_(e);
        });
    }

    __onRetSizeChange() {
        const self = this;
        this.on$('blur', this.sizeId, function (e) {
            self.handleRetChange_(e);
        });
    }

    __onAddQueryRow() {
        const self = this;
        const opt = this.queryTable$.querySelector("tr:last-child select");
        if (opt) {
            opt.onchange = function (e) {
                const tar = e.currentTarget;
                const name = tar.value;
                const esm = self.getEsMetaByName(name);
                self.addQuery(esm);
            };
        }
    }

    __onAddSortRow() {
        const self = this;
        const opt = this.sortTable$.querySelector("tr:last-child select");
        if (opt) {
            opt.onchange = function (e) {
                const tar = e.currentTarget;
                const name = tar.value;
                const esm = self.getEsMetaByName(name);
                self.addSort(esm);
            };
        }
    }

    __onTab() {
        const self = this;
        const tBtns = this.view$.querySelectorAll(".xvision-tab-btn");
        let i = 0,
            j = tBtns.length,
            tBtn = void 0;
        for (; i < j; i++) {
            tBtn = tBtns[i];
            tBtn.addEventListener("click", function (e) {
                self.handleTab_(e);
            }, false);
        }
    }

    __onCancel() {
        const self = this;
        this.on$("click", this.cancelId, function (e) {
            self.handleCancel_(e);
        });
    }

    __onSave() {
        const self = this;
        this.on$("click", this.saveId, function (e) {
            self.handleSave_(e);
        });
    }

    __onHide() {
        const self = this;
        this.on$("click", this.hideId, function (e) {
            self.handleCancel_(e);
        });
    }

    __onArgEdit() {
        const ipts = this.argTable$.querySelector("tr td:last-child input[type=text]");
        let i = 0,
            j = ipts.length,
            ipt = void 0;
        for (; i < j; i++) {
            ipt = ipts[i];
            ipt.onblur = function (e) {
                this.handleArgEdit_(e);
            };
        }
    }

    handleTitleChange_(e) {
        this._title_ = e.currentTarget.value;
    }

    addQuery(esm) {
        const name = esm.name;
        const title = esm.title;
        const op = '选择操作符';
        const value = "";
        this.queryModel.setQuery(name, value, op);
        this.addQueryRow(name, title, op, value);
    }

    addSort(ems) {
        const name = ems.name;
        const title = ems.title;
        const value = "升序";
        this.queryModel.setSort(name, value);
        this.addSortRow(name, title, value);
    }

    handleOp_(e$) {
        const r$ = $(e$.currentTarget).closest("tr");
        const name = r$.attr("name");
        const op = $(e$.currentTarget).val();
        this.queryModel.setQuery(name, null, op);
    }

    handleRetChange_(e) {
        this.queryModel.size = e.currentTarget.value;
    }

    handleCancel_(e) {
        if (this.com.metadata.hastexttemplate) {
            this._htmlEditor_.destroy();
        }
        this.destroyView();
    }

    handleSave_(e) {
        this.com.text = this.title;
        this.com.query = this.queryModel.toEsModel();
        if (this.com.metadata.hastexttemplate) {
            this.com.htmlText = this.htmlText = this._htmlEditor_.getContent();
            this.com.vueData.data = [this.htmlText];
            this._htmlEditor_.destroy();
        }
        var xEventArgs = new XEventArgs(null, `save.${this.eventNamespace}`);
        this.routeEvent(xEventArgs);
        this.destroyView();
    }

    handleTab_(e) {
        const tar = e.currentTarget;
        const anchor = tar.innerHTML;
        if (!tar.classList.contains("active")) {
            const tab = tar.parentNode.parentNode;
            const tp = tab.querySelector(`.xvision-tab-page[data-anchor="${anchor}"]`);
            tp.classList.add("active");
            let n = tp;
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

    handleArgEdit_(e) {
        const name = e.currentTarget.parentNode.parentNode.getAttribute("name");
        const v = e.currentTarget.value;
        this.setArgValue(name, v);
    }

    setArgValue(name, value) {
        let a;
        const am = this.argModel;
        for (var i = am.length - 1; i >= 0; i--) {
            a = am[i];
            if (a.code == name) {
                a.value = value;
                break;
            }
        }
    }

    getEsMetaByName(name) {
        const em = this.esMeta;
        for (let i = em.length - 1; i >= 0; i--) {
            if (em[i].name == name) return em[i];
        }
    }

    getOperator(sel = "eq") {
        const kvArray = [{ key: "eq", value: "等于" }, { key: "contains", value: "包含" }, { key: "in", value: "任意值" }, { key: "ge", value: "大于" }, { key: "gte", value: "大于等于" }, { key: "lt", value: "小于" }, { key: "lte", value: "小于等于" }, { key: "ne", value: "不等于" }];
        return this.getSelect("选择操作符", kvArray, sel);
    }

    getSelect(hint, kvArray, selKey) {
        let opt = '';
        let v = `<select>`;
        v += `<option>${hint}</option>`;
        kvArray.forEach(kv => {
            if (kv.key == selKey) {
                opt = `<option value="${kv.key}" select ="true" >${kv.value}</option>`;
            } else {
                opt = `<option value="${kv.key}">${kv.value}</option>`;
            }
            v += opt;
        });
        v += '</select>';
        return v;
    }

    getRmButton() {
        let v = '<button class="xvision-com-editor-rm">删除</button>';
        return v;
    }

}

class ContextmenuItem extends UIElement {

    constructor(text = '', command = c => void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(parent, pre, next, enviroment, id, name);
        this._text_ = text;
        this._command_ = command;
    }

    get text() {
        return this._text_;
    }

    set text(text) {
        this._text_ = text;
    }

    get command() {
        return this._command_;
    }

    set command(command) {
        this._command_ = command;
    }

    initialize() {
        this.__onClick();
    }

    handleClick_(e$) {
        super.handleClick_(e$);
    }

    execute(args) {
        this.command(args);
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-contextmenu-item">`;
        v += this.text;
        v += '</div>';
        return v;
    }
}

class Contextmenu extends UIElement {

    static _CreateD(ref = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        const addCommand = new ContextmenuItem("增加", c => c.addNode());
        const modifyCommand = new ContextmenuItem("修改", c => c.modify());
        const removeCommand = new ContextmenuItem("删除", c => c.remove());
        const cm = new Contextmenu(ref, parent, pre, next, enviroment, id, name);
        cm.addChild(addCommand);
        cm.addChild(modifyCommand);
        cm.addChild(removeCommand);
        return cm;
    }

    constructor(ref = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(parent, pre, next, enviroment, id, name);

        this._ref_ = ref;

        this.__onHide();
        this.__onExecuteCommad();
    }

    get ref() {
        return this._ref_;
    }

    set ref(ref) {
        this._ref_ = ref;
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-contextmenu">`;
        v += this.getChildView();
        v += '</div>';
        return v;
    }

    show(ref, pageX, pageY) {
        this.ref = ref;
        this.show_(pageX, pageY);
    }

    show_(pageX, pageY) {
        if (!this.isInDocument) {
            const body = this.document.body;
            body.insertAdjacentHTML('beforeend', this.getView());
            this.initialize();
        }
        this.view$.style.left = pageX + 'px';
        this.view$.style.top = pageY + 'px';
        this.view$.style.display = 'block';
    }

    hide() {
        this.view$ && (this.view$.style.display = "none");
    }

    __onHide() {
        const self = this;
        this.document.addEventListener("click", function (e) {
            self.__handleHide(e);
        }, false);
    }

    __handleHide(e) {
        const tar = e.target;
        if (!this.isChildView(tar)) {
            this.hide();
        }
    }

    __onExecuteCommad() {
        const self = this;
        this.on("click.xWander", function (source, xEventArgs) {
            self.handleExecuteCommand_(xEventArgs);
        });
    }

    handleExecuteCommand_(xEventArgs) {
        this.hide();
        xEventArgs.srcTarget && xEventArgs.srcTarget.execute(this.ref);
    }

}

class Control extends UIElement {

    static __InstanceId() {
        if (!Control.hasOwnProperty("__instanceId__")) Control.__instanceId__ = 0;
        return ++Control.__instanceId__;
    }

    static get __ClassName_() {
        return "Control";
    }

    static _Create(caption = '', text = '', catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Control(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(caption = '', text = '', catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(contextmenu, parent, pre, next, enviroment, id, name);
        this._caption_ = caption || this.name;
        this._text_ = text || this.name;
        this._catalog_ = catalog;
        this._contextmenu_ = contextmenu;
    }

    get nodeHeaderId() {
        return `ndr__${this.id}`;
    }

    get nodeHeader$() {
        return this.byId(this.nodeHeaderId);
    }

    get caption() {
        return this._caption_;
    }

    set caption(caption) {
        this._caption_ = caption;
        this.isInDocument && (this.caption$.innerHTML = caption);
    }

    get captionId() {
        return `cap__${this.id}`;
    }

    get caption$() {
        return this.byId(this.captionId);
    }

    get text() {
        return this._text_;
    }

    set text(text) {
        this._text_ = text;
        this.isInDocument && this.text$ && (this.text$.textContent = text);
    }

    get textId() {
        return `txt__${this.id}`;
    }

    get text$() {
        return this.byId(this.textId);
    }

    get catalog() {
        return this._catalog_;
    }

    set catalog(catalog) {
        this._catalog_ = catalog;
    }

    get containerId() {
        return `c__${this.id}`;
    }

    get container$() {
        return this.byId(this.containerId);
    }


    get iconId() {
        return `ico__${super.id}`;
    }

    get icon$() {
        return this.byId(this.iconId);
    }

    get rmId() {
        return `rm__${this.id}`;
    }

    get rm$() {
        return this.byId(this.rmId);
    }

    get holderId() {
        return `hor__${this.id}`;
    }

    get holder$() {
        return this.byId(this.holderId);
    }

    get dragStartETN() {
        return `dragStart.${this.eventNamespace}`;
    }

    get dragEnterETN() {
        return `dragEnter.${this.eventNamespace}`;
    }

    get dragLeaveETN() {
        return `dragLeave.${this.eventNamespace}`;
    }

    get dragOverETN() {
        return `dragOver.${this.eventNamespace}`;
    }

    get dropETN() {
        return `drop.${this.eventNamespace}`;
    }

    get containerClickETN() {
        return `containerClick.${this.eventNamespace}`;
    }

    get removeETN() {
        return `remove.${this.eventNamespace}`;
    }

    getModel() {
        return `0`;
    }

    getChildModel() {
        let v = '';
        this.children.forEach(c => v += c.getModel());
        return v;
    }

    getJModel() {
        return null;
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-border xvision-margin-hor xvision-margin-ver-xg">`;
        v += this.getHeader();
        v += this.getBody();
        v += '</div>';
        return v;
    }

    getHeader() {
        let v = `<div id="${this.nodeHeaderId}" class="xvision-row">`;
        v += '<div class="xvision-col-6 xvision-padding-sm">';
        v += this.getCaptionView();
        v += '</div>';
        v += '<div class="xvision-col-6 xvision-padding-sm">';
        v += this.getToolView();
        v += '</div>';
        v += '</div>';
        return v;
    }

    getCaptionView() {
        let v = `<span id="${this.captionId}">${this.caption}</span>`;
        return v;
    }

    getToolView() {
        let v = `<div class="xvision-text-right">`;
        v += this.getRmToolView();
        v += '</div>';
        return v;
    }

    getRmToolView() {
        let v = `<i id="${this.rmId}" class="xvision-tool-btn xvision-margin-md glyphicon glyphicon-remove"></i>`;
        return v;
    }

    getBody() {
        let v = `<div id="${this.containerId}" style="position:relative">`;
        v += this.getChildView();
        v += '</div>';
        return v;
    }

    getIconView() {
        let v = `<div id="${this.iconId}" class="xvision-icon" draggable="true">`;
        v += this.caption;
        v += '</div>';
        return v;
    }

    findChildNearestAfterPixel(ox, oy, diff) {
        let r = void 0;
        if (this.isInDocument) r = super.findChildNearestAfterPixel(ox, oy, diff, this.container$);
        return r;
    }

    getDragImage() {
        return this.icon$;
    }

    canDrop(control) {
        return false;
    }

    select() {
        this.addClass(this.view$, "selected");
    }

    cancelSelect() {
        this.removeClass(this.view$, "selected");
    }

    selectContainer() {
        this.addClass(this.container$, "selected");
    }

    cancelContainerSelected() {
        this.removeClass(this.container$, "selected");
    }

    preAdd() {
        const holder = this.byId(this.holderId);
        if (!holder) {
            const tml = `<div id="${this.holderId}" class="xvision-control-holder"></div>`;
            this.container$.insertAdjacentHTML('beforeend', tml);
        }
    }

    preAddBefore(ele) {
        const holder = this.byId(this.holderId);
        if (!holder) {
            const tml = `<div id="${this.holderId}" class="xvision-control-holder"></div>`;
            ele.insertAdjacentHTML('beforebegin', tml);
        }
    }

    cancelPreAdd() {
        if (this.isInDocument) {
            const holder = this.byId(this.holderId);
            holder.parentNode.removeChild(holder);
        }
    }

    onDragStart_(ele$) {
        const self = this;
        this.on$("dragstart", ele$, function (e) {
            self.handleDragStart_(e);
        }, this.icon$);
    }

    handleDragStart_(e) {
        this.dispatchEvent(e, this.dragStartETN);
    }

    onDragEnter_(ele$) {
        const self = this;
        this.on$("dragenter", ele$, function (e) {
            self.handleDragEnter_(e);
        });
    }

    handleDragEnter_(e) {
        this.dispatchEvent(e, this.dragEnterETN);
    }

    onDragLeave_(ele$) {
        const self = this;
        this.on$("dragleave", ele$, function (e) {
            self.handleDragLeave_(e);
        });
    }

    handleDragLeave_(e) {
        this.dispatchEvent(e, this.dragLeaveETN);
    }

    onDragOver_(ele$) {
        const self = this;
        this.on$("dragover", ele$, function (e) {
            self.handleDragOver_(e);
        });
    }

    handleDragOver_(e) {
        this.dispatchEvent(e, this.dragOverETN);
    }

    onDrop_(ele$) {
        const self = this;
        this.on$("drop", ele$, function (e) {
            self.handleDrop_(e);
        });
    }

    handleDrop_(e) {
        this.dispatchEvent(e, this.dropETN);
    }

    onContainerClick_(ele$) {
        const self = this;
        this.on$("click", ele$, function (e) {
            self.handleContainerClick_(e);
        });
    }

    handleContainerClick_(e) {
        this.dispatchEvent(e, this.containerClickETN);
    }

    onRemove_(ele$) {
        const self = this;
        this.on$("click", ele$, function (e) {
            self.handleRemove_(e);
        });
    }

    handleRemove_(e) {
        this.dispatchEvent(e, this.removeETN);
    }

    initializeIcon() {
        this.__onDragStart();
    }

    __onDragStart() {
        this.onDragStart_(this.iconId);
    }

    __onDragEnter() {
        this.onDragEnter_(this.containerId);
    }

    __onDragOver() {
        this.onDragOver_(this.containerId);
    }

    __onDragLeave() {
        this.onDragLeave_(this.containerId);
    }

    __onDrop() {
        this.onDrop_(this.containerId);
    }

    __onContainerClick() {
        this.onContainerClick_(this.containerId);
    }

    __onRemove() {


        this.onRemove_(this.rmId);
    }
}

class Container extends Control {

    static __InstanceId() {
        if (!Container.hasOwnProperty("__instanceId__")) Container.__instanceId__ = 0;
        return ++Container.__instanceId__;
    }

    static get __ClassName_() {
        return "Container";
    }

    static _Create(caption = '', text = '', catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Container(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(caption = '', text = '', catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    initialize() {
        super.initialize();
        this.__onDragOver();
        this.__onDrop();
    }

    findParentHeadlineId() {
        let p = this;
        while (p = p.parent) {
            if (p.is(knowLayout) && p.headlineId) return p.headlineId;
        }
        return void 0;
    }

}

class UserControl extends Control {

    static __InstanceId() {
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "UserControl";
    }

    static _Create(caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new UserControl(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    initialize() {
        this.__onRemove();
    }
}

class KnowVue extends UserControl {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "KnowVue";
    }

    static _Create(metadata = {}, query = void 0, args = void 0, caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new KnowVue(metadata, query, args, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(metadata, query = void 0, args = void 0, caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {

        super(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);

        this._editETN_ = `edit.${this.eventNamespace}`;

        this._metadata_ = metadata;

        this._query_ = query;

        this._args_ = this.parseArgs(args);

        this._vueData_ = this.parseVueData(this.args);

        this._categoryId_ = void 0;

        this._htmlText_ = void 0;
    }

    get vueId() {
        return `vue__${this.id.replace(/-/g, "")}`;
    }

    get editId() {
        return `edt__${this.id}`;
    }

    get edit$() {
        return this.byId(this.editId);
    }

    get editETN() {
        return this._editETN_;
    }

    get query() {
        return this._query_;
    }

    set query(query) {
        this._query_ = query;
    }

    get args() {
        return     this._args_;
    }

    set args(args) {
        this._args_ = args;
    }

    get categoryId() {
        return this._categoryId_;
    }

    set categoryId(categoryId) {
        this._categoryId_ = categoryId;
    }

    get htmlText() {
        return this._htmlText_;
    }

    set htmlText(htmlText) {
        this._htmlText_ = htmlText;
    }

    get vueData() {
        return this._vueData_;
    }

    get metadata() {
        return this._metadata_;
    }

    isHeadline() {
        return (/^headline/.test(this._metadata_.code)
        );
    }

    getDocLev() {
        let l = -1;
        if (this.isHeadline()) {
            l = this._metadata_.code.replace(/^headline/, "");
            l = parseInt(l);
        }
        return l;
    }

    parseArgs(args) {
        let a = args;
        if (!a) a = this.clone(this.metadata.comparams);
        return a;
    }

    parseVueParams(args) {
        const res = [];
        let arg = void 0;
        for (let i = args.length - 1; i >= 0; i--) {
            arg = args[i];
            arg.defaultvalue&&res.push(`:${arg.code}=${arg.code}`);
        }
        return " " + res.join(" ") + " ";
    }

    parseVueData(args) {
        let res = {};
        let arg = void 0;
        for (let i = args.length - 1; i >= 0; i--) {
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

    createNew() {
        return new this.constructor(this.metadata, null, null, this.caption, this.text, this.catalog);
    }

    remove() {
        const p = this.parent;
        super.remove();
        if (this.isHeadline() && p.is(knowLayout)) p.reHeadline();
    }

    getJModel(templateId) {
        const m = {
            templateid: templateId,
            templatecatalogueid: this.categoryId || 0,
            title: this.text,
            queryparameter: JSON.stringify(this.query),
            componentparameter: JSON.stringify(this.metadata.comparams),
            htmltext: this.htmlText,
            fragmenttypeid: this.metadata.id,
            placeholderid: this.id,
            id: this.vueId || 0
        };
        return m;
    }

    getModel() {
        let v = `<placeholder id="${this.id}" type="${this.metadata.code}" data-class="UserControl"></placeholder>`;
        return v;
    }

    getCaptionView() {
        return " ";
    }

    getBody() {
        let v = `<div id="${this.vueId}" class="xvision-row">`;
        if(this.isHeadline()){
            v += '<div style="width:15px;height:32px;background:rgb(80,156,234);position:absolute"></div>';
        };
        v += this.getVueView();
        v += '</div>';
        return v;
    }

    getVueView() {
        const meta = this.metadata;
        let v = `<pks:${meta.vuetag} ${this.parseVueParams(this.args)}></pks:${meta.vuetag}>`;
        return v;
    }

    getToolView() {
        let v = `<div class="xvision-text-right">`;
        v += this.getEditToolView();
        v += this.getRmToolView();
        v += '</div>';
        return v;
    }

    getEditToolView() {
        let v = `<i id="${this.editId}" class="xvision-tool-btn xvision-margin-md glyphicon glyphicon-edit"></i>`;
        return v;
    }

    initialize() {
        super.initialize();
        this.__onEdit();
        this.initVue();
    }

    initVue() {
        window.PKSUI.bind({
            el: `#${this.vueId}`,
            data: this.vueData,
            model: [`pks:${this.metadata.vuetag}`]
        });
    }

    __onEdit() {
        this.onEdit_(this.editId);
    }

    onEdit_(ele) {
        const self = this;
        this.on$("click", ele, function (e) {
            self.handleEdit_(e);
        });
    }

    handleEdit_(e) {
        this.dispatchEvent(e, this.editETN);
    }

}

class TreeNode extends Control {

    static __InstanceId() {
        if (!TreeNode.hasOwnProperty("__instanceId__")) TreeNode.__instanceId__ = 0;
        return ++TreeNode.__instanceId__;
    }

    static get __ClassName_() {
        return "TreeNode";
    }

    static _Create(data = void 0, caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new TreeNode(data, caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(data = void 0, caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);

        this._data_ = data;
    }

    get isExpanded() {
        return this.isInDocument && this.hasClass(this.expander$, "expanded");
    }

    get data() {
        return this._data_;
    }

    set data(data) {
        this._data_ = data;
    }

    get expanderId() {
        return `exp__${super.id}`;
    }

    get expander$() {
        return this.byId(this.expanderId);
    }

    get selectorId() {
        return `sor__${this.id}`;
    }

    get selector$() {
        return this.byId(this.selectorId);
    }

    get editorId() {
        return `eor__${this.id}`;
    }

    get editor$() {
        return this.byId(this.editorId);
    }

    get editorInputId() {
        return `eii__${this.id}`;
    }

    get editorInput$() {
        return this.byId(this.editorInputId);
    }

    addChild_(key, node) {
        if (this.isInDocument) this.toParent();
        super.addChild_(key, node);
    }

    remove() {
        this.routeEventX(`removeTreeNode.${this.eventNamespace}`);
        const p = this.parent;
        if (p.children.length <= 1) p.toLeaf && p.toLeaf();
        super.remove();
    }

    isLeaf() {
        return this.children.length == 0;
    }

    toParent() {
        const exp = "expanded";
        this.removeClass(this.expander$, "leaf");
        this.addClass(this.expander$, exp);
        this.addClass(this.container$, exp);
    }

    toLeaf() {
        const exp = "expanded";
        this.addClass(this.expander$, "leaf");
        this.removeClass(this.expander$, exp);
        this.removeClass(this.container$, exp);
    }

    handleContextmenu_(e) {
        e.preventDefault();
        this.dispatchEvent(e, this.contextmenuETN);
    }

    select() {
        this.isInDocument && this.addClass(this.selector$, "selected");
    }

    cancelSelect() {
        this.isInDocument && this.removeClass(this.selector$, "selected");
    }

    addNode() {
        const tn = TreeNode._Create();
        tn.parent = this;
        tn.routeEvent(new XEventArgs(null, `addTreeNode.${this.eventNamespace}`));
        tn.modify();
    }

    modify() {
        this.editText();
    }

    editText() {
        this.showEditor();
        const eii$ = this.editorInput$;
        eii$.value = this.text;
        eii$.select();
        eii$.focus();
    }

    cancelEditText() {
        this.hideEditor();
        this.editorInput$.value = "";
    }

    showEditor() {
        if (this.isInDocument) {
            const eor$ = this.editor$;
            eor$.style.opacity = 1;
            eor$.style.zIndex = 1;
        }
    }

    hideEditor() {
        if (this.isInDocument) {
            const eor$ = this.editor$;
            eor$.style.opacity = 0;
            eor$.style.zIndex = -2;
        }
    }

    onExpande_(ele$) {
        const self = this;
        this.on$("click", ele$, function (e) {
            self.handleExpande_(e);
        });
    }

    handleExpande_(e) {
        e.stopPropagation();
        const exp$ = this.expander$;
        const c$ = this.container$;
        const expCls = "expanded";
        if (this.hasClass(exp$, expCls)) {
            this.removeClass(exp$, expCls);
            this.removeClass(c$, expCls);
        } else {
            this.addClass(exp$, expCls);
            this.addClass(c$, expCls);
        }
    }

    onTextEdited_(ele$) {
        const self = this;
        this.on$('blur', ele$, function (e) {
            self.handleTextEdited_(e);
        });
    }

    handleTextEdited_(e) {
        e.stopPropagation();
        const txt = e.currentTarget.value;
        this.text = txt;
        this.hideEditor();
        this.routeEventX(`modifyTreeNode.${this.eventNamespace}`);
    }

    getView() {
        let v = `<li id="${this.viewId}" class="xvision-tree-node">`;
        v += this.getHeader();
        v += this.getBody();
        v += '</li>';
        return v;
    }

    getHeader() {
        let v = `<div id="${this.nodeHeaderId}" class="xvision-tree-node-header">`;
        v += this.getEditor();
        v += `<div id="${this.selectorId}" class="xvision-tree-node-selector"></div>`;
        v += this.getExpanderView();
        v += `<span id="${this.textId}" class="xvision-tree-node-text">${this.text}</span>`;
        v += '</div>';
        return v;
    }

    getExpanderView() {
        const expId = this.expanderId;
        let expCls = ["xvision-tree-node-expander", "glyphicon", "glyphicon-play"];
        if (this.children.length < 1) expCls.push("leaf");
        expCls = this.joinClassName(expCls);
        let v = `<i id="${expId}" class="${expCls}"></i>`;
        return v;
    }

    getEditor() {
        let v = `<div id="${this.editorId}" class="xvision-tree-node-editor">`;
        v += `<div class="xvision-window">`;
        v += `<input id="${this.editorInputId}" class="xvision-tree-node-editor-input" type="text">`;
        v += `</div>`;
        v += '</div>';
        return v;
    }

    getBody() {
        let v = `<ul id="${this.containerId}" class="xvision-tree-node-body">`;
        v += this.getChildView();
        v += '</ul>';
        return v;
    }

    initialize() {
        super.initialize();
        this.__onClick();
        this.__onExpande();
        this.__onTextEdited();
        this.__onContextmenu();
    }

    __onClick() {
        this.onClick_(this.nodeHeaderId);
    }

    __onExpande() {
        this.onExpande_(this.expanderId);
    }

    __onContextmenu() {
        this.onContextmenu_(this.nodeHeaderId);
    }

    handleContextmenu_(e) {
        e.preventDefault();
        e.stopPropagation();
        this.dispatchEvent(e, this.contextmenuETN);
    }

    __onTextEdited() {
        this.onTextEdited_(this.editorInputId);
    }

}

class Tree extends Control {

    static __InstanceId() {
        if (!Tree.hasOwnProperty("__instanceId__")) Tree.__instanceId__ = 0;
        return ++Tree.__instanceId__;
    }

    static get __ClassName_() {
        return "Tree";
    }

    static _CreateFromFlatJson(jsonArray, map) {
        if (Tree._UserMap(map)) jsonArray = Tree._MapFlatJson(jsonArray, map);
        const tree = new Tree();
        tree.contextmenu = Contextmenu._CreateD();
        const treeRoots = [];
        const treeModel = Tree._CreateTreeModelFromFlatJson(jsonArray);
        treeModel.forEach(itor => tree.addChild(Tree._CreateTreeNode(itor)));
        return tree;
    }

    static _UserMap(map) {
        return map && map.id && map.parentId && map.text;
    }

    static _MapFlatJson(jsonArray, map) {
        let t = void 0;
        const jArr = [];
        jsonArray.forEach(o => {
            t = {};
            t.id = o[map.id];
            t.parentId = o[map.parentId] || "#";
            t.text = o[map.text];
            t.raw = o;
            jArr.push(t);
        });
        return jArr;
    }

    static _CreateTreeModelFromFlatJson(jsonArray) {
        const roots = Tree._FindRootNodes(jsonArray);
        Tree._CompositeNodes(jsonArray, roots);
        return roots;
    }

    static _FindRootNodes(jsonArray, rootId) {
        let rootNodes = [];
        if (rootId) {
            rootNodes = Tree._FindRooetNodesById(jsonArray, rootId);
        } else {
            rootNodes = Tree._FindRootNodesByRecursive(jsonArray);
        }
        return rootNodes;
    }

    static _CompositeNodes(jsonArray, rootNodes) {
        const parents = rootNodes.slice();
        let parent, node;
        jsonArray = jsonArray.reverse();
        while ((parent = parents.shift()) && jsonArray.length > 0) {
            for (let i = jsonArray.length - 1; i >= 0; i--) {
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

    static _CreateTreeNode(node) {
        const tNode = TreeNode._Create();
        tNode.text = node.text;
        tNode.data = node.raw;
        if (node.children && node.children.length > 0) {
            node.children.forEach(itor => tNode.addChild(Tree._CreateTreeNode(itor)));
        }
        return tNode;
    }

    static _FindRootNodesByRecursive(jsonArray) {
        const nodes = [];
        const index = Tree._IndexNodeJsonArray(jsonArray);
        jsonArray.forEach(itor => {
            if (!!!index[itor.parentId]) nodes.push(itor);
        });
        return nodes;
    }

    static _FindRootNodesById(jsonArray, rootId = "#") {
        const nodes = [];
        jsonArray.forEach(itor => {
            if (itor.parentId && itor.parentId == rootId) node.push(itor);
        });
        return nodes;
    }

    static _IndexNodeJsonArray(jsonArray) {
        return Tree._IndexJsonArray(jsonArray, "id");
    }

    static _IndexJsonArray(jsonArray, indexKey) {
        const index = {};
        jsonArray.forEach(itor => index[itor[indexKey]] = itor);
        return index;
    }

    static _Create(caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Tree(caption, text, catalog, contextmenu, parent, pre, next, enviroment);
    }

    constructor(caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);

        this._selected_ = void 0;
    }

    get selected() {
        return this._selected_;
    }

    createNode() {
        const tn = TreeNode._Create();
        tn.parent = this;
        tn.modify();
    }

    findNodeByDataId(id) {
        const nodes = this.children;
        return this.findNodeByDataId_(id, nodes);
    }

    findNodeByDataId_(id, nodes) {
        let i = 0,
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

    selectNode(node) {
        this._selected_ && this._selected_.cancelSelect();
        this._selected_ = node;
        this._selected_.select();
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-tree">`;
        v += this.getHeader();
        v += this.getBody();
        v += '</div>';
        return v;
        v += this.getSelfView();
        v += `<ul id="${this.containerId}" class="xvision-tree-body">`;
        v += this.getChildView();
        v += '</div>';
        return v;
    }

    getHeader() {
        let v = `<div class="xvision-tree-header">`;
        v += `<span id="${this.captionId}">${this.caption}</span>`;
        v += '</div>';
        return v;
    }

    getBody() {
        let v = `<ul id="${this.containerId}" class="xvision-tree-body">`;
        v += this.getChildView();
        v += '</ul>';
        return v;
    }

    handleTreeNodeClick_(xEventArgs) {
        const node = xEventArgs.srcTarget;
        if (this !== node) {
            const xe = new XEventArgs(null, `selectTreeNode.${this.eventNamespace}`);
            xe.addPath(xEventArgs.srcTarget);
            this.routeEvent(xe);
        }
    }

    initialize() {
        super.initialize();
        this.__onTreeNodeClick();
        this.__onTreeNodeContextmenu();
    }

    __onTreeNodeClick() {
        const self = this;
        this.on("click.xWander", function (source, xEventArgs) {
            self.handleTreeNodeClick_(xEventArgs);
        });
    }

    __onTreeNodeContextmenu() {
        const self = this;
        this.on("contextmenu.xWander", function (source, xEventArgs) {
            self.handleTreeNodeContextmenu_(xEventArgs);
        });
    }

    __onTreeContextmenu() {
        const self = this;
        this.on$("contextmenu", this.containerId, function (e) {
            self.handleContextmenu_(e);
        });
    }

    handleContextmenu_(e) {
        e.preventDefault();
        const pageX = e.pageX;
        const pageY = e.pageY;
        this.showContextmenu(pageX, pageY);
    }

    handleTreeNodeContextmenu_(xEventArgs) {
        const node = xEventArgs.srcTarget;
        const event = xEventArgs.event;
        const pageX = event.pageX;
        const pageY = event.pageY;
        this.showContextmenu_(node, pageX, pageY);
    }

}

class ControlCatalog extends Control {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "ControlCatalog";
    }

    static _Create(imgUrl='', caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new ControlCatalog(imgUrl,caption, name, catalog, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(imgUrl='', caption = '', text = '', catalog = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, catalog, contextmenu, parent, pre, next, enviroment, id, name);

        this._imgUrl_ = imgUrl;
    }

    get imgUrl(){
        return this._imgUrl_;
    }

    get scrollLeftId() {
        return `scl__${this.id}`;
    }

    get scrollLeft$() {
        return this.byId(this.scrollLeftId);
    }

    get scrollRightId() {
        return `scr__${this.id}`;
    }

    get scrollRight$() {
        return this.byId(this.scrollRightId);
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-catalog">`;
        v += this.getHeader();
        v += this.getBody();
        v += '</div>';
        return v;
    }

    getHeader() {
        let v = `<div class="xvision-catalog-header">`;
        v += `<div class="xvision-catalog-caption">`;
        v += `<img src="${this.imgUrl}" />`;
        v += `<span id="${this.captionId}">${this.caption}</span>`;
        v += '</div>';
        v += '</div>';
        return v;
    }

    getBody() {
        let v = '<div class="xvision-catalog-body">';
        v += this.getScrollLeftAnchor();
        v += this.getContainer();
        v += this.getScrollRightAnchor();
        v += '</div>';
        return v;
    }

    getScrollLeftAnchor() {
        let v = `<div id="${this.scrollLeftId}" class="xvision-scroll-left">`;
        v += '<';
        v += '</div>';
        return v;
    }

    getScrollRightAnchor() {
        let v = `<div id="${this.scrollRightId}" class="xvision-scroll-right">`;
        v += '>';
        v += '</div>';
        return v;
    }

    getContainer() {
        let v = `<ul id="${this.containerId}" class="xvision-catalog-scroller">`;
        v += this.getChildView();
        v += '</ul>';
        return v;
    }

    getChildView() {
        let v = '';
        this.children.forEach(c => {
            v += '<li class="xvision-catalog-scroller-item">';
            v += c.getIconView();
            v += '</li>';
        });
        return v;
    }

    initialize() {
        this.initializeC();
        this.onShowScrollAnchor();
        this.onHideScrollAnchor();
        this.onScrollLeft();
        this.onScrolRight();
    }

    initializeC() {
        this.children.forEach(c => c.initializeIcon());
    }

    onShowScrollAnchor() {
        const self = this;
        this.on$("mouseenter", this.viewId, function (e) {
            self.handleShowScrollAnchor_(e);
        });
    }

    onHideScrollAnchor() {
        const self = this;
        const cid$ = this.viewId$;
        this.on$("mouseleave", this.viewId, function (e) {
            self.handleHideScrollAnchor_(e);
        });
    }

    handleShowScrollAnchor_(e) {
        this.scrollLeft$.style.opacity = 1;
        this.scrollRight$.style.opacity = 1;
    }

    handleHideScrollAnchor_(e) {
        this.scrollLeft$.style.opacity = 0;
        this.scrollRight$.style.opacity = 0;
    }

    onScrollLeft() {
        const self = this;
        const scl = this.scrollLeftId;
        this.on$("click", scl, function (e) {
            self.handleScroll_(e, 0);
        });
    }

    onScrolRight() {
        const self = this;
        const scr = this.scrollRightId;
        this.on$("click", scr, function (e) {
            self.handleScroll_(e, 1);
        });
    }

    handleScroll_(e, directon) {
        const c = this.container$;
        if (directon == 0) {
            c.scrollLeft -= 10;
        } else {
            c.scrollLeft += 10;
        }
    }
}

class ControlManager extends Control {

    static __InstanceId() {
        if (!ControlManager.hasOwnProperty("__instanceId__")) ControlManager.__instanceId__ = 0;
        return ++ControlManager.__instanceId__;
    }

    static get __ClassName_() {
        return "ControlManager";
    }

    static _CreateD(name = "", catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}) {
        const cm = new ControlManager(name, catalog, contextmenu, parent, pre, next, enviroment);
        const layoutCC = ControlCatalog._Create("/content/xeditor/images/布局.png","选择布局");
        const docCC = ControlCatalog._Create("/content/xeditor/images/结构.png","文档结构");
        const ctrlCC = ControlCatalog._Create("/content/xeditor/images/组件.png","选择组件");
        cm.addChild_(layoutCC.caption, layoutCC);
        cm.addChild_(docCC.caption, docCC);
        cm.addChild_(ctrlCC.caption, ctrlCC);
        return cm;
    }

    static _Create(name = "", catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}) {
        return new ControlManager(name, catalog, contextmenu, parent, pre, next, enviroment);
    }

    constructor(name = "", catalog = "", contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}) {
        super(name, catalog, contextmenu, parent, pre, next, enviroment);
    }

    register(control) {
        const catalogName = control.catalog;
        if (!this.hasChild_(catalogName)) throw new Error(`${catalog}目录不存在`);
        const catalog = this.getChild(catalogName);
        catalog.addChild(control);
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-container">`;
        v += this.getChildView();
        v += '</div>';
        return v;
    }

    getChildView() {
        let v = `<div id="${this.containerId}" class="xvision-row">`;
        v += this.getCatalogView();
        v += '</div>';
        return v;
    }

    getCatalogView() {
        const catalogs = this.children;
        let v = ``;
        for (var i = 0, j = catalogs.length; i < j; i++) {
            v += `<div class="xvision-col-4">`;
            v += catalogs[i].getView();
            if (i < j - 1) v += `<div class="xvision-splitor"></div>`;
            v += '</div>';
        }
        return v;
    }
}

class Designer extends Container {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Designer";
    }
    static _Create(caption = '', text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Designer(caption, text, contextmenu, parent, pre, next, enviroment, id, name);
    }
    constructor(caption = '', text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    canDrop(control) {
        return control.is(Layout);
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-window">`;
        v += `<div id="${this.containerId}" class="xvision-window" style="position:relative">`;
        v += this.getChildView();
        v += '</div>';
        v += '</div>';
        return v;
    }

    getModel() {
        let v = '<div class="xvision-container">';
        v += `<div class="xvision-row">`;
        v += this.getChildModel();
        v += `</div>`;
        v += '</div>';
        return v;
    }
}

class Layout extends Container {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Layout";
    }

    static get __Catalog_() {
        return "选择布局";
    }

    constructor(caption = '', text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(caption, text, Layout.__Catalog_, contextmenu, parent, pre, next, enviroment, id, name);
    }

    canDrop(control) {
        return true;
    }

    getCaptionView() {
        return " ";
    }

    createNew() {
        var n = super.createNew();
        n.__init();
        return n;
    }

    initialize() {
        super.initialize();
        this.__onRemove();
    }

    __init() {}
}

class LayoutContainer extends Container {
    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "LayoutContainer";
    }

    static _Create(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new LayoutContainer(text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super('', text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    canDrop(control) {
        if (control.getDocLev && control.getDocLev() != -1) {
            return false;
        }
        return true;
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-row xvision-padding">`;
        v += `<div id="${this.containerId}" class="xvision-col-12 xvision-layout-inner xvision-min-height xvision-border" style="position:relative">`;
        v += this.getChildView();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    getModel() {
        let v = `<div id="${this.viewId}" class="xvision-row xvision-padding" data-class="LayoutContainer">`;
        v += `<div id="${this.containerId}" class="xvision-col-12 xvision-min-height xvision-border">`;
        v += this.getChildModel();
        v += `</div>`;
        v += `</div>`;
        return v;
    }
}

class Layout12 extends Layout {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Layout12";
    }

    static _Create(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Layout12(text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super('12', text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    getIconView() {
        let v = `<div id="${this.iconId}" class="xvision-icon xvision-text-center" draggable="true">`;
        v += `<span id="${this.captionId}">${this.caption}</span>`;
        v += '</div>';
        return v;
    }

    getView() {
        let v = `<div id="${this.viewId}" class="xvision-row xvision-layout xvision-margin-hor xvision-margin-ver-xg xvision-padding xvision-border">`;
        v += this.getHeader();
        v += `<div id="${this.containerId}" class="xvision-col-12 xvision-layout-inner xvision-min-height xvision-border" style="position:relative">`;
        v += this.getChildView();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    getModel() {
        let v = `<div id="${this.viewId}" class="xvision-row xvision-margin-hor xvision-margin-ver-md xvision-padding xvision-border" data-class="Layout12">`;
        v += `<div id="${this.containerId}" class="xvision-col-12 xvision-min-height xvision-border">`;
        v += this.getChildModel();
        v += `</div>`;
        v += `</div>`;
        return v;
    }
}

class knowLayout extends Layout12 {
    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "knowLayout";
    }

    static _Create(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new knowLayout(text, Contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super(text, contextmenu, parent, pre, next, enviroment, id, name);

        this._docLev_ = -1;

        this._headlineId_ = void 0;
    }

    get docLev() {
        return this._docLev_;
    }

    get headlineId() {
        return this._headlineId_;
    }

    reHeadline() {
        this._docLev_ = -1;
        this._headlineIid_ = void 0;
    }

    canDrop(control) {
        const p = this.parent;
        const isHeadline = control.is(KnowVue) && control.isHeadline();
        if (isHeadline) {
            if (this.children.length > 0) return false;
            const lev = control.getDocLev();
            if (p.is(Designer)) {
                return lev == 1;
            } else {
                const pLev = p.docLev;
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

    addChild(control) {
        super.addChild(control);
        this.afterAddChild(control);
    }

    addChildBefore(control, bControl) {
        super.addChildBefore(control, bControl);
        this.afterAddChild(control);
    }

    afterAddChild(control) {
        const isHeadline = control.isHeadline && control.isHeadline();
        if (isHeadline) {
            this._docLev_ = control.getDocLev();
            this._headlineId_ = control.id;
        } else {
            control.categoryId = this._headlineId_;
        }
    }
}

class Layout8_4 extends Layout {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Layout8_4";
    }

    static _Create(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Layout8_4(text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super('8 4', text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    getIconView() {
        let v = `<div id="${this.iconId}" class="xvision-icon xvision-text-center" draggable="true">`;
        v += `<span id="${this.captionId}">${this.caption}</span>`;
        v += '</div>';
        return v;
    }

    getView() {
        const c1 = this.children[0];
        const c2 = this.children[1];
        let v = `<div id="${this.viewId}" class="xvision-row xvision-layout xvision-margin-hor xvision-margin-ver-xg xvision-border">`;
        v += this.getHeader();
        v += `<div class="xvision-col-8">`;
        v += c1.getView();
        v += '</div>';
        v += '<div class="xvision-col-4">';
        v += c2.getView();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    getModel() {
        const c1 = this.children[0];
        const c2 = this.children[1];
        let v = `<div id="${this.viewId}" class="xvision-row xvision-margin-hor xvision-margin-ver-md xvision-border" data-class="Layout8_4">`;
        v += `<div class="xvision-col-8">`;
        v += c1.getModel();
        v += '</div>';
        v += '<div class="xvision-col-4">';
        v += c2.getModel();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    __init() {
        this.addChild(LayoutContainer._Create());
        this.addChild(LayoutContainer._Create());
    }
}

class Layout6_6 extends Layout {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Layout6_6";
    }

    static _Create(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Layout6_6(text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super('6 6', text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    getIconView() {
        let v = `<div id="${this.iconId}" class="xvision-icon xvision-text-center" draggable="true">`;
        v += `<span id="${this.captionId}">${this.caption}</span>`;
        v += '</div>';
        return v;
    }

    getView() {
        const c1 = this.children[0];
        const c2 = this.children[1];
        let v = `<div id="${this.viewId}" class="xvision-row xvision-layout xvision-border xvision-margin-hor xvision-margin-ver-xg">`;
        v += this.getHeader();
        v += `<div class="xvision-col-6">`;
        v += c1.getView();
        v += '</div>';
        v += '<div class="xvision-col-6">';
        v += c2.getView();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    getModel() {
        const c1 = this.children[0];
        const c2 = this.children[1];
        let v = `<div id="${this.viewId}" class="xvision-row xvision-border xvision-margin-hor xvision-margin-ver-lg" data-class="Layout6_6">`;
        v += `<div class="xvision-col-6">`;
        v += c1.getModel();
        v += '</div>';
        v += '<div class="xvision-col-6">';
        v += c2.getModel();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    __init() {
        this.addChild(LayoutContainer._Create());
        this.addChild(LayoutContainer._Create());
    }
}

class Layout4_4_4 extends Layout {

    static __InstanceId() {
        if (!this.hasOwnProperty("__instanceId__")) this.__instanceId__ = 0;
        return ++this.__instanceId__;
    }

    static get __ClassName_() {
        return "Layout4_4_4";
    }

    static _Create(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        return new Layout4_4_4(text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    constructor(text = '', contextmenu = void 0, parent = void 0, pre = void 0, next = void 0, enviroment = {}, id = '', name = '') {
        super('4 4 4', text, contextmenu, parent, pre, next, enviroment, id, name);
    }

    getIconView() {
        let v = `<div id="${this.iconId}" class="xvision-icon xvision-text-center" draggable="true">`;
        v += `<span id="${this.captionId}">${this.caption}</span>`;
        v += '</div>';
        return v;
    }

    getView() {
        const c1 = this.children[0];
        const c2 = this.children[1];
        const c3 = this.children[2];
        let v = `<div id="${this.viewId}" class="xvision-row xvision-layout xvision-margin-hor xvision-margin-ver-xg xvision-border">`;
        v += this.getHeader();
        v += `<div class="xvision-col-4">`;
        v += c1.getView();
        v += '</div>';
        v += '<div class="xvision-col-4">';
        v += c2.getView();
        v += '</div>';
        v += '<div class="xvision-col-4">';
        v += c3.getView();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    getModel() {
        const c1 = this.children[0];
        const c2 = this.children[1];
        const c3 = this.children[2];
        let v = `<div id="${this.viewId}" class="xvision-row xvision-margin-hor xvision-margin-ver-lg xvision-border" data-class="Layout4_4_4">`;
        v += `<div class="xvision-col-4">`;
        v += c1.getModel();
        v += '</div>';
        v += '<div class="xvision-col-4">';
        v += c2.getModel();
        v += '</div>';
        v += '<div class="xvision-col-4">';
        v += c3.getModel();
        v += '</div>';
        v += `</div>`;
        return v;
    }

    __init() {
        this.addChild(LayoutContainer._Create());
        this.addChild(LayoutContainer._Create());
        this.addChild(LayoutContainer._Create());
    }
}