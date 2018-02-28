/**
 * 
 * 类描述: 定义js类的工厂。
 * 
 * @author zhanggf
 * @version 1.0 创建时间： 2012-11-10
 * ********************************更新记录******************************
 *   版本： 1.0 修改日期： 修改人： 修改内容：
 * *********************************************************************
 */
(function($, undefined) {
	/**
	 * 方法描述：定义工厂类类中的属性和方法。
	 */
	$.jasfcFactory = function(name, base, prototype) {
		var namespace = name.split(".")[0], fullName;
		name = name.split(".")[1];
		fullName = namespace + "-" + name;

		if (!prototype) {
			prototype = base;
			base = $.JasfcFactory;
		}
		// 为插件创建选择器
		$.expr[":"][fullName] = function(elem) {
			return !!$.data(elem, name);
		};
		$[namespace] = $[namespace] || {};
		$[namespace][name] = function(options, element) {
			//允许实例化没有初始化对于简单的继承
			if (arguments.length) {
				this._createWidget(options, element);
			}
		};
		var basePrototype = new base();
		//我们需要使选项散列属性直接在新实例否则我们将修改选项散列的原型
		basePrototype.options = $.extend(true, {}, basePrototype.options);
		$[namespace][name].prototype = $.extend(true, basePrototype, {
			namespace : namespace,
			widgetName : name,
			widgetEventPrefix : $[namespace][name].prototype.widgetEventPrefix
					|| name,
			widgetBaseClass : fullName
		}, prototype);

		$.jasfcFactory.bridge(name, $[namespace][name]);
	};

	$.jasfcFactory.bridge = function(name, object) {
		$.fn[name] = function(options) {
			var isMethodCall = typeof options === "string", args = Array.prototype.slice
					.call(arguments, 1), returnValue = this;

			//允许多个散列通过init
			options = !isMethodCall && args.length ? $.extend.apply(null, [
					true, options ].concat(args)) : options;

			//防止内部方法调用
			if (isMethodCall && options.charAt(0) === "_") {
				return returnValue;
			}
			if (isMethodCall) {
				this.each(function() {
					var instance = $.data(this, name), methodValue = instance
						&& $.isFunction(instance[options]) ? instance[options]
						.apply(instance, args)
						: instance;
					if (methodValue !== instance
							&& methodValue !== undefined) {
						returnValue = methodValue;
						return false;
					}
				});
			} else {
				this.each(function() {
					var instance = $.data(this, name);
					if (instance) {
						instance.option(options || {})._init();
					} else {
						$.data(this, name, new object(options, this));
					}
				});
			}

			return returnValue;
		};
	};

	$.JasfcFactory = function(options, element) {
		// 允许实例化没有初始化对于简单的继承
		if (arguments.length) {
			this._createWidget(options, element);
		}
	};

	$.JasfcFactory.prototype = {
		widgetName : "jasfcFactory",
		widgetEventPrefix : "",
		options : {
			disabled : false
		},
		_createWidget : function(options, element) {
			$.data(element, this.widgetName, this);
			this.element = $(element);
			this.options = $.extend(true, {}, this.options, this
					._getCreateOptions(), options);

			var self = this;
			this.element.bind("remove." + this.widgetName, function() {
				self.destroy();
			});

			this._create();
			this._trigger("create");
			this._init();
		},
		_getCreateOptions : function() {
			return $.metadata
					&& $.metadata.get(this.element[0])[this.widgetName];
		},
		_create : function() {
		},
		_init : function() {
		},

		destroy : function() {
			this.element.unbind("." + this.widgetName).removeData(
					this.widgetName);
			this.jasfcFactory().unbind("." + this.widgetName).removeAttr(
					"aria-disabled").removeClass(
					this.widgetBaseClass + "-disabled " + "ui-state-disabled");
		},

		jasfcFactory : function() {
			return this.element;
		},

		option : function(key, value) {
			var options = key;

			if (arguments.length === 0) {
				// 不要返回一个引用内部散列
				return $.extend({}, this.options);
			}

			if (typeof key === "string") {
				if (value === undefined) {
					return this.options[key];
				}
				options = {};
				options[key] = value;
			}

			this._setOptions(options);

			return this;
		},
		_setOptions : function(options) {
			var self = this;
			$.each(options, function(key, value) {
				self._setOption(key, value);
			});

			return this;
		},
		_setOption : function(key, value) {
			this.options[key] = value;

			if (key === "disabled") {
				this.jasfcFactory()[value ? "addClass" : "removeClass"](
						this.widgetBaseClass + "-disabled" + " "
								+ "ui-state-disabled").attr("aria-disabled",
						value);
			}

			return this;
		},

		enable : function() {
			return this._setOption("disabled", false);
		},
		disable : function() {
			return this._setOption("disabled", true);
		},

		_trigger : function(type, event, data) {
			var prop, orig, callback = this.options[type];

			data = data || {};
			event = $.Event(event);
			event.type = (type === this.widgetEventPrefix ? type
					: this.widgetEventPrefix + type).toLowerCase();
			//最初的事件可能来自任何元素,所以我们需要重置目标的新事件
			event.target = this.element[0];
			// 复制原始事件属性到新事件
			orig = event.originalEvent;
			if (orig) {
				for (prop in orig) {
					if (!(prop in event)) {
						event[prop] = orig[prop];
					}
				}
			}
			this.element.trigger(event, data);
			return !($.isFunction(callback)
					&& callback.call(this.element[0], event, data) === false || event
					.isDefaultPrevented());
		}
	};

})(jQuery);
