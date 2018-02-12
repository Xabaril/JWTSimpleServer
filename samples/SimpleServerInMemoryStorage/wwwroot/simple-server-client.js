window["JwtSimpleServer"] =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(__webpack_require__(1));
__export(__webpack_require__(6));


/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var httpClient_1 = __webpack_require__(2);
var observable_1 = __webpack_require__(5);
var ClientOptions = /** @class */ (function () {
    function ClientOptions() {
        this.tokenEndpoint = "/token";
        this.host = window.location.origin;
    }
    return ClientOptions;
}());
exports.ClientOptions = ClientOptions;
var ServerClient = /** @class */ (function () {
    function ServerClient(options) {
        this.options = options;
        this.onBeforeRequestAccessToken = new observable_1.Observable();
        this.onRequestAccessTokenSuccess = new observable_1.Observable();
        this.onBeforeRequestRefreshToken = new observable_1.Observable();
        this.onRequestRefreshTokenSuccess = new observable_1.Observable();
        this._httpClient = options.httpClient || new httpClient_1.XMLHttpRequestClient();
    }
    ServerClient.prototype.requestAccessToken = function (credentials) {
        return __awaiter(this, void 0, void 0, function () {
            var requestContent, token;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.onBeforeRequestAccessToken.notify(undefined);
                        requestContent = "grant_type=password&username=" + credentials.userName + "&password=" + credentials.password;
                        return [4 /*yield*/, this._postTokenRequest(requestContent)];
                    case 1:
                        token = _a.sent();
                        this.onRequestAccessTokenSuccess.notify(token);
                        return [2 /*return*/, token];
                }
            });
        });
    };
    ServerClient.prototype.refreshAccessToken = function (credentials) {
        return __awaiter(this, void 0, void 0, function () {
            var content, token;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.onBeforeRequestRefreshToken.notify(undefined);
                        content = "grant_type=refresh_token&refresh_token=" + credentials.refreshToken;
                        return [4 /*yield*/, this._postTokenRequest(content)];
                    case 1:
                        token = _a.sent();
                        this.onRequestRefreshTokenSuccess.notify(token);
                        return [2 /*return*/, token];
                }
            });
        });
    };
    ServerClient.prototype._postTokenRequest = function (content) {
        return __awaiter(this, void 0, void 0, function () {
            var _a, host, tokenEndpoint, response;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _a = this.options, host = _a.host, tokenEndpoint = _a.tokenEndpoint;
                        return [4 /*yield*/, this._httpClient.post("" + host + tokenEndpoint, {
                                content: content
                            })];
                    case 1:
                        response = _b.sent();
                        return [2 /*return*/, this._buildTokenFromResponse(response)];
                }
            });
        });
    };
    ServerClient.prototype._buildTokenFromResponse = function (response) {
        return JSON.parse(response.content);
    };
    return ServerClient;
}());
exports.ServerClient = ServerClient;


/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || Object.assign || function(t) {
    for (var s, i = 1, n = arguments.length; i < n; i++) {
        s = arguments[i];
        for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
            t[p] = s[p];
    }
    return t;
};
Object.defineProperty(exports, "__esModule", { value: true });
var httpResponse_1 = __webpack_require__(3);
var httpError_1 = __webpack_require__(4);
var HttpClient = /** @class */ (function () {
    function HttpClient() {
    }
    HttpClient.prototype.post = function (url, options) {
        return this.send(__assign({}, options, { method: "POST", url: url }));
    };
    return HttpClient;
}());
exports.HttpClient = HttpClient;
var XMLHttpRequestClient = /** @class */ (function (_super) {
    __extends(XMLHttpRequestClient, _super);
    function XMLHttpRequestClient() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    XMLHttpRequestClient.prototype.send = function (request) {
        return new Promise(function (resolve, reject) {
            var xhr = new XMLHttpRequest();
            xhr.open(request.method, request.url, true);
            xhr.setRequestHeader("X-Request-Client", "XMLHttpClient");
            xhr.setRequestHeader("Content-type", request.contentType || "application/x-www-form-urlencoded");
            xhr.onload = function () {
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(new httpResponse_1.HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
                }
                else {
                    reject(new httpError_1.HttpError(xhr.statusText, xhr.status));
                }
            };
            xhr.onerror = function () {
                reject(new httpError_1.HttpError(xhr.statusText, xhr.status));
            };
            xhr.ontimeout = function () {
                reject(new httpError_1.HttpError("Operation timeout", 500));
            };
            xhr.send(request.content || "");
        });
    };
    return XMLHttpRequestClient;
}(HttpClient));
exports.XMLHttpRequestClient = XMLHttpRequestClient;


/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var HttpResponse = /** @class */ (function () {
    function HttpResponse(statusCode, statusText, content) {
        this.statusCode = statusCode;
        this.statusText = statusText;
        this.content = content;
    }
    return HttpResponse;
}());
exports.HttpResponse = HttpResponse;


/***/ }),
/* 4 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var HttpError = /** @class */ (function (_super) {
    __extends(HttpError, _super);
    function HttpError(errorMessage, statusCode) {
        var _this = _super.call(this, errorMessage) || this;
        _this.statusCode = statusCode;
        return _this;
    }
    return HttpError;
}(Error));
exports.HttpError = HttpError;


/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
/**
* Creates a new observer
* @param callback defines the callback to call when the observer is notified
* @param scope defines the current scope used to restore the JS context
*/
var Observer = /** @class */ (function () {
    function Observer(callback, scope) {
        if (scope === void 0) { scope = null; }
        this.callback = callback;
        this.scope = scope;
    }
    return Observer;
}());
exports.Observer = Observer;
/**
* The Observable class is a simple implementation of the Observable pattern.
*/
var Observable = /** @class */ (function () {
    function Observable() {
        this._observers = new Array();
    }
    Observable.prototype.subscribe = function (callback) {
        if (!callback)
            throw Error("You should provide a callback to subscribe to an observable");
        var observer = new Observer(callback);
        this._observers.push(observer);
        return observer;
    };
    Observable.prototype.notify = function (eventData) {
        if (!this.hasObservers())
            return;
        for (var _i = 0, _a = this._observers; _i < _a.length; _i++) {
            var observer = _a[_i];
            if (observer.scope) {
                observer.callback.call(observer.scope, eventData);
            }
            else {
                observer.callback(eventData);
            }
        }
    };
    Observable.prototype.remove = function (observer) {
        if (!observer)
            return false;
        var index = this._observers.indexOf(observer);
        if (index !== -1) {
            this._observers.splice(index, 1);
            return true;
        }
        return false;
    };
    Observable.prototype.hasObservers = function () {
        return this._observers.length > 0;
    };
    return Observable;
}());
exports.Observable = Observable;


/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var RefreshTokenServiceOptions = /** @class */ (function () {
    function RefreshTokenServiceOptions(intervalSeconds, refreshToken, onRefreshTokenSuccessCallback) {
        if (intervalSeconds === void 0) { intervalSeconds = null; }
        if (refreshToken === void 0) { refreshToken = ""; }
        this.intervalSeconds = intervalSeconds;
        this.refreshToken = refreshToken;
        this.onRefreshTokenSuccessCallback = onRefreshTokenSuccessCallback;
    }
    return RefreshTokenServiceOptions;
}());
exports.RefreshTokenServiceOptions = RefreshTokenServiceOptions;
var RefreshTokenService = /** @class */ (function () {
    function RefreshTokenService(client) {
        this.client = client;
        this._aborted = false;
    }
    RefreshTokenService.prototype.start = function (refreshTokenOptions) {
        var _this = this;
        this._aborted = false;
        this._ensureOptions(refreshTokenOptions);
        this._refreshSubscription = this.client.onRequestRefreshTokenSuccess.subscribe(function (token) {
            refreshTokenOptions.onRefreshTokenSuccessCallback &&
                refreshTokenOptions.onRefreshTokenSuccessCallback(token);
        });
        this._intervalSubscription = setInterval(function () { return __awaiter(_this, void 0, void 0, function () {
            var token;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (this._aborted)
                            return [2 /*return*/];
                        return [4 /*yield*/, this.client.refreshAccessToken({ refreshToken: refreshTokenOptions.refreshToken })];
                    case 1:
                        token = _a.sent();
                        refreshTokenOptions.refreshToken = token.refresh_token;
                        return [2 /*return*/];
                }
            });
        }); }, refreshTokenOptions.intervalSeconds * 1000);
    };
    RefreshTokenService.prototype.stop = function () {
        this._aborted = true;
        if (this._intervalSubscription !== 0) {
            clearInterval(this._intervalSubscription);
            this._intervalSubscription = 0;
        }
        if (this._refreshSubscription) {
            this.client.onRequestRefreshTokenSuccess.remove(this._refreshSubscription);
            this._refreshSubscription = undefined;
        }
    };
    RefreshTokenService.prototype._ensureOptions = function (options) {
        if (!options.onRefreshTokenSuccessCallback) {
            throw Error("You must provide a callback to start the RefreshTokenService");
        }
        if (!options.intervalSeconds) {
            throw Error("You must provide the refresh token interval");
        }
    };
    return RefreshTokenService;
}());
exports.RefreshTokenService = RefreshTokenService;


/***/ })
/******/ ]);
//# sourceMappingURL=simple-server-client.js.map