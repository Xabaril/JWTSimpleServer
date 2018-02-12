export interface IObserver<T> {
    closed?: boolean;
    next: (value: T) => void;
    error: (err: any) => void;
    complete: () => void;
}

export interface ISubscription {
    unsubscribe(): void;
    readonly closed: boolean;
}

/**
 * Simple Subscription implementation.
 */
export class Subscription implements ISubscription {

    /**
     * Indicates whether this Subscription has already been unsubscribed.
     * @type {boolean}
     */
    public closed: boolean = false;

    /**
     * Disposes the Subscription resources.
     * @return {void}
     */
    unsubscribe(): void {
        this.closed = true;
        return;
    }
}

/**
 * Simple Subscriber implementation (aka Observer).
 */
export class Subscriber<T> extends Subscription implements IObserver<T> {

    private _isStopped: boolean = false;
    private _context: any;
    private _next: (value: T) => void;
    private _error: ((error: any) => void) | undefined;
    private _complete: (() => void) | undefined;

    /**
     * @constructor
     * @param {Observer|Function} [observerOrNext] (optional) A Observer or a `next` handler.
     * @param {Function} [error] (optional) The `error` handler.
     * @param {Function} [complete] (optional) The `complete` handler.
     */
    constructor(
        observerOrNext?: IObserver<T> | ((value?: T) => void),
        error?: (error?: any) => void,
        complete?: () => void) {
        super();

        let next: ((value: T) => void);
        let context: any = this;

        switch (typeof observerOrNext) {
            case 'function':
                next = <(value: T) => void>observerOrNext;
                break;

            case 'object':
                const observer = <IObserver<T>>observerOrNext;
                next = observer.next;
                error = observer.error;
                complete = observer.complete;
                context = Object.create(observer);
                break;

            default:
                throw new Error('The observerOrNext must be a function or an object.');
        }

        this._context = context;
        this._next = next;
        this._error = error;
        this._complete = complete;
    }

    /**
     * Creates a new Subscriber instance.
     * @param {Function} [observerOrNext] (optional) The `next` handler.
     * @param {Function} [error] (optional) The `error` handler.
     * @param {Function} [complete] (optional) The `complete` handler.
     * @return {Subscriber<T>} A new Observable instance.
     */
    static create<T>(
        observerOrNext?: IObserver<T> | ((value?: T) => void),
        error?: (error?: any) => void,
        complete?: () => void): Subscriber<T> {
        return new Subscriber(observerOrNext, error, complete);
    }

    /**
     * The Observer `next` notifications handler.
     * @param {T} [value] The `next` value.
     * @return {void}
     */
    next(value?: T): void {
        if (!this._isStopped && this._next) {
            try {
                this._next.call(this._context, value);
            } catch (err) {
                this._hostReportError(err);
                this.unsubscribe();
            }
        }
    }

    /**
     * The Observer `error` notifications handler.
     * @param {any} [error] The `error` exception.
     * @return {void}
     */
    error(error?: any): void {
        if (!this._isStopped) {
            if (this._error) {
                try {
                    this._error.call(this._context, error);
                } catch (err) {
                    this._hostReportError(err);
                }
            } else {
                this._hostReportError(error);
            }
            this.unsubscribe();
        }
    }

    /**
     * The Observer `complete` notifications handler.
     * @return {void}
     */
    complete(): void {
        if (!this._isStopped) {
            if (this._complete) {
                try {
                    this._complete.call(this._context);
                } catch (err) {
                    this._hostReportError(err);
                }
            }
            this.unsubscribe();
        }
    }

    /**
     * Disposes the Observer resources.
     * @return {void}
     */
    unsubscribe(): void {
        if (this.closed) {
            return;
        }
        this._isStopped = true;
        this._context = null;
        super.unsubscribe();
    }

    private _hostReportError(err: any) {
        setTimeout(() => { throw err; });
    }
}

/**
 * Symbol.Observable polyfill.
 */
function getSymbolObservable(context: any) {
    let $$observable: any;
    let Symbol = context.Symbol;

    if (typeof Symbol === 'function') {
        if (Symbol.observable) {
            $$observable = Symbol.observable;
        } else {
            $$observable = Symbol('observable');
            Symbol.observable = $$observable;
        }
    } else {
        $$observable = '@@observable';
    }

    return $$observable;
}

const Symbol_observable = getSymbolObservable(window);

/**
 * Simple Observable implementation.
 */
export class Observable<T> {

    /**
     * @constructor
     * @param {Function} subscribe The function called on Observable subscription.
     */
    constructor(subscribe?: (subscriber: Subscriber<T>) => void) {
        if (subscribe) {
            this._subscribe = subscribe;
        }
    }

    /**
     * Creates a new Observable instance.
     * @param {Function} [subscribe] (optional) The subscriber function to be passed to the Observable constructor.
     * @return {Observable} A new Observable instance.
     */
    static create: Function = <T>(subscribe?: (subscriber: Subscriber<T>) => void) => {
        return new Observable<T>(subscribe);
    }

    /**
     * Registers Observer handlers for Observable notifications.
     *
     * @param {Observer|Function} [observerOrNext] (optional) Either an observer or next handler.
     * @param {Function} [error] (optional) The `error` handler.
     * @param {Function} [complete] (optional) The `complete` handler.
     * @return {ISubscription} A subscription reference to the registered handlers.
     */
    subscribe(
        observerOrNext?: IObserver<T> | ((value?: T) => void),
        error?: (error: any) => void,
        complete?: () => void): Subscription {
        const subscriber = new Subscriber(observerOrNext, error, complete);
        this._subscribe(subscriber);
        return subscriber;
    }

    protected _subscribe(subscriber: Subscriber<T>) { }

    /**
     * See https://github.com/zenparsing/es-observable
     * @return {Observable} Self reference.
     */
    [Symbol_observable]() {
        return this;
    }
}

/**
 * Simple SubjectSubscription implementation.
 */
export class SubjectSubscription<T> extends Subscription {

    /**
     * Indicates whether this SubjectSubscription has already been unsubscribed.
     * @type {boolean}
     */
    closed: boolean = false;

    /**
     * @constructor
     * @param {Subject<T>} [subject] The subscription Subject instance.
     * @param {IObserver<T>} [subscriber] The subscriber function to be passed to the Observable constructor.
     */
    constructor(public subject: Subject<T>, public subscriber: IObserver<T>) {
        super();
    }

    /**
     * Disposes the SubjectSubscription resources.
     * @return {void}
     */
    unsubscribe() {
        if (this.closed) {
            return;
        }

        this.closed = true;

        const subject = this.subject;
        const observers = subject.observers;

        this.subject = null;

        if (!observers || observers.length === 0 || subject.isStopped || subject.closed) {
            return;
        }

        const subscriberIndex = observers.indexOf(this.subscriber);

        if (subscriberIndex !== -1) {
            observers.splice(subscriberIndex, 1);
        }
    }
}

/**
 * Simple Subject implementation.
 */
export class Subject<T> extends Observable<T> implements ISubscription {

    /**
     * Indicates whether this Subject has already been unsubscribed.
     * @type {boolean}
     */
    closed = false;

    /**
     * Indicates whether this Subject has dispatched a error notification.
     * @type {boolean}
     */
    hasError = false;

    /**
     * Indicates whether this Subject has already been stopped.
     * @type {boolean}
     */
    isStopped = false;

    /**
     * Collection of Subject observers.
     * @type {boolean}
     */
    observers: IObserver<T>[] = [];

    /**
     * Reference to thrown error dispatched by this Subject.
     * @type {any}
     */
    thrownError: any = null;

    /**
     * @constructor
     */
    constructor() {
        super();
    }

    /**
     * Dispatches the `next` notification to the subscribed observers.
     * @param {T} [value] (optional) The `next` value.
     * @return {void}
     */
    next(value?: T) {
        if (this.closed) {
            throw new Error('The subscription is closed.');
        }
        if (!this.isStopped) {
            const { observers } = this;
            const len = observers.length;
            const copy = observers.slice();
            for (let i = 0; i < len; i++) {
                copy[i].next(<T>value);
            }
        }
    }

    /**
     * Dispatches the `error` notification to the subscribed observers.
     * @param {any} [error] The `error` value.
     * @return {void}
     */
    error(error: any) {
        if (this.closed) {
            throw new Error('The subscription is closed.');
        }
        this.hasError = true;
        this.thrownError = error;
        this.isStopped = true;
        const { observers } = this;
        const len = observers.length;
        const copy = observers.slice();
        for (let i = 0; i < len; i++) {
            copy[i].error(error);
        }
        this.observers.length = 0;
    }

    /**
     * Dispatches the `complete` notification to the subscribed observers.
     * @return {void}
     */
    complete() {
        if (this.closed) {
            throw new Error('The subscription is closed.');
        }
        this.isStopped = true;
        const { observers } = this;
        const len = observers.length;
        const copy = observers.slice();
        for (let i = 0; i < len; i++) {
            copy[i].complete();
        }
        this.observers.length = 0;
    }

    unsubscribe() {
        this.isStopped = true;
        this.closed = true;
        this.observers = null;
    }

    protected _subscribe(subscriber: Subscriber<T>): Subscription {
        this.observers.push(subscriber);
        return new SubjectSubscription(this, subscriber);
    }
}