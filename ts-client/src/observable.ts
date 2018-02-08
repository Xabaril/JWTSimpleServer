
/**
* Creates a new observer
* @param callback defines the callback to call when the observer is notified
* @param scope defines the current scope used to restore the JS context
*/
export class Observer<T> {
    constructor(
        public callback: (eventData: T) => void,
        public scope: any = null
    ) { }
}

/**
* The Observable class is a simple implementation of the Observable pattern.
*/
export class Observable<T> {
    private _observers = new Array<Observer<T>>();

    public subscribe(callback: (eventData: T) => void): Observer<T> {
        if (!callback) throw Error("You should provide a callback to subscribe to an observable");

        let observer = new Observer<T>(callback);
        this._observers.push(observer);

        return observer;
    }

    public notify(eventData: T): void {
        for (let observer of this._observers) {
            if (observer.scope) {
                observer.callback.call(observer.scope, eventData);
            }
            else {
                observer.callback(eventData);
            }
        }
    }

    public remove(observer: Observer<T>): boolean {
        if (!observer) return false;
        let index = this._observers.indexOf(observer);
        if (index !== -1) {
            this._observers.splice(index, 1);
            return true;
        }
        return false;
    }

    public hasObservers(): Boolean {
        return this._observers.length > 0;
    }

}

