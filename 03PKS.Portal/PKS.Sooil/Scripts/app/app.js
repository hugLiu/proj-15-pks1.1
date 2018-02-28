
function App() {
    this.initFunc = [];
}

App.prototype.init = function (func) {
    this.initFunc.push(func);
}

App.prototype.close=function(func) {
    
}
var app = new App();
$(document).ready(function() {
    if (app.initFunc.length > 0) {
        for (var i = 0; i < app.initFunc.length; i++) {
            app.initFunc[i]();
        }
    }
});








