import { Observable, Subject, Subscriber, Subscription } from "../src/observable";

describe("Subscription should", () => {
        it("be closed after unsubscribe method call", () => {
                const subscription = new Subscription();
                subscription.unsubscribe();
                expect(subscription.closed).toBe(true);
        });
});

describe("Subscriber should", () => {
        it("be instantiable through constructor that accepts a `next` handler", () => {
                const nextHandlerMock = jest.fn();
                const subscriber = new Subscriber<Date>(nextHandlerMock);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through constructor that accepts a `next` and `error` handlers", () => {
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const subscriber = new Subscriber<Date>(nextHandlerMock, errorHandlerMock);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through constructor that accepts a `next`, `error` and `complete` handlers", () => {
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const completeHandlerMock = jest.fn();
                const subscriber = new Subscriber<Date>(nextHandlerMock, errorHandlerMock, completeHandlerMock);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through constructor that accepts a observer", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through static create method that accepts a `next` handler", () => {
                const nextHandlerMock = jest.fn();
                const subscriber = Subscriber.create(nextHandlerMock);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through static create method that accepts a `next` and `error` handlers", () => {
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const subscriber = Subscriber.create(nextHandlerMock, errorHandlerMock);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through static create method that accepts a `next`, `error` and `complete` handlers", () => {
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const completeHandlerMock = jest.fn();
                const subscriber = Subscriber.create(nextHandlerMock, errorHandlerMock, completeHandlerMock);
                expect(subscriber).toBeDefined();
        });

        it("be instantiable through static create method that accepts a observer", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = Subscriber.create(observer);
                expect(subscriber).toBeDefined();
        });

        it("invoke the `next` handler on next method call", () => {
                const nextHandlerMock = jest.fn();
                const subscriber = new Subscriber<Date>(nextHandlerMock);
                const date = new Date();

                subscriber.next(date);

                expect(nextHandlerMock).toHaveBeenCalledWith(date);
        });

        it("invoke the `error` handler on error method call", () => {
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const subscriber = new Subscriber<Date>(nextHandlerMock, errorHandlerMock);
                const error = new Error();

                subscriber.error(error);

                expect(errorHandlerMock).toHaveBeenCalledWith(error);
        });

        it("invoke the `complete` handler on complete method call", () => {
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const completeHandlerMock = jest.fn();
                const subscriber = new Subscriber<Date>(nextHandlerMock, errorHandlerMock, completeHandlerMock);

                subscriber.complete();

                expect(completeHandlerMock).toHaveBeenCalled();
        });

        it("invoke the obserer `next` handler on next method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);
                const date = new Date();

                subscriber.next(date);

                expect(observer.next).toHaveBeenCalledWith(date);
        });

        it("invoke the obserer `error` handler on error method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);
                const error = new Error();

                subscriber.error(error);

                expect(observer.error).toHaveBeenCalledWith(error);
        });

        it("invoke the obserer `complete` handler on complete method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);

                subscriber.complete();

                expect(observer.complete).toHaveBeenCalled();
        });

        it("not call `next` handler after error method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);
                const error = new Error();

                subscriber.error(error);
                subscriber.next();

                expect(observer.error).toHaveBeenCalledWith(error);
                expect(observer.next).toHaveBeenCalledTimes(0);
        });

        it("not call `error` handler after error method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);
                const error = new Error();

                subscriber.error(error);
                subscriber.error();

                expect(observer.error).toHaveBeenCalledWith(error);
                expect(observer.error).toHaveBeenCalledTimes(1);
        });

        it("not call `complete` handler after error method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);
                const error = new Error();

                subscriber.error(error);
                subscriber.complete();

                expect(observer.error).toHaveBeenCalledWith(error);
                expect(observer.complete).toHaveBeenCalledTimes(0);
        });

        it("not call `next` handler after unsubscribe method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);

                subscriber.unsubscribe();
                subscriber.next();

                expect(observer.next).toHaveBeenCalledTimes(0);
        });

        it("not call `error` handler after unsubscribe method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);

                subscriber.unsubscribe();
                subscriber.error();

                expect(observer.error).toHaveBeenCalledTimes(0);
        });

        it("not call `complete` handler after unsubscribe method call", () => {
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };
                const subscriber = new Subscriber<Date>(observer);

                subscriber.unsubscribe();
                subscriber.complete();

                expect(observer.complete).toHaveBeenCalledTimes(0);
        });
});

describe("Observable should", () => {
        it("be instantiable through constructor that accepts a subscribe function", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                expect(observable).toBeDefined();
        });

        it("be instantiable through static create method that accepts a subscribe function", () => {
                const subscribeMock = jest.fn();
                const observable = Observable.create(subscribeMock);
                expect(observable).toBeDefined();
        });

        it("call the subscribe function on subscription", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const nextHandlerMock = jest.fn();

                observable.subscribe(nextHandlerMock);

                expect(subscribeMock.mock.calls.length).toBe(1);
        });

        it("be subscribable with a `nex` handler", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const nextHandlerMock = jest.fn();

                observable.subscribe(nextHandlerMock);

                const subscriber = <Subscriber<Date>>subscribeMock.mock.calls[0][0];

                expect(subscriber).toBeDefined();
        });

        it("be subscribable with a `error` handler", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();

                observable.subscribe(nextHandlerMock, errorHandlerMock);

                const subscriber = <Subscriber<Date>>subscribeMock.mock.calls[0][0];

                expect(subscriber).toBeDefined();
        });

        it("be subscribable with a `complete` handler", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const completeHandlerMock = jest.fn();

                observable.subscribe(nextHandlerMock, errorHandlerMock, completeHandlerMock);

                const subscriber = <Subscriber<Date>>subscribeMock.mock.calls[0][0];

                expect(subscriber).toBeDefined();
        });

        it("be subscribable with a `complete` handler", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const nextHandlerMock = jest.fn();
                const errorHandlerMock = jest.fn();
                const completeHandlerMock = jest.fn();

                observable.subscribe(nextHandlerMock, errorHandlerMock, completeHandlerMock);

                const subscriber = <Subscriber<Date>>subscribeMock.mock.calls[0][0];

                expect(subscriber).toBeDefined();
        });

        it("be subscribable with a observer", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };

                observable.subscribe(observer);

                const subscriber = <Subscriber<Date>>subscribeMock.mock.calls[0][0];

                expect(subscriber).toBeDefined();
        });

        it("return the subscription instance un subscribe", () => {
                const subscribeMock = jest.fn();
                const observable = new Observable<Date>(subscribeMock);
                const observer = {
                        next: jest.fn(),
                        error: jest.fn(),
                        complete: jest.fn()
                };

                const subscription = observable.subscribe(observer);

                expect(subscription).toBeDefined();
        });
});

describe("Subject should", () => {
        it("not have observers when first created", () => {
                const subject = new Subject<string>();
                expect(subject.observers).toEqual([]);
        });

        it("should have observers when subscribed", () => {
                const subject = new Subject<string>();
                subject.subscribe((data: string) => { });
                expect(subject.observers.length).toBe(1);
        });

        it("should notify observers when published", () => {
                const subject = new Subject<Date>();

                const firstMockObserver = jest.fn();
                const secondMockObserver = jest.fn();

                const date = new Date();
                subject.subscribe(firstMockObserver);
                subject.subscribe(secondMockObserver);
                subject.next(date);

                expect(firstMockObserver).toHaveBeenCalledWith(date);
                expect(secondMockObserver).toHaveBeenCalledWith(date);
        });

        it('should allow the unsubscription of a specific subscriber', () => {
                const subject = new Subject<Date>();

                const firstMockObserver = jest.fn();
                const secondMockObserver = jest.fn();

                const date = new Date();
                const firstSubscription = subject.subscribe(firstMockObserver);
                const secondSubscription = subject.subscribe(secondMockObserver);

                secondSubscription.unsubscribe();

                subject.next(date);

                expect(firstMockObserver).toHaveBeenCalledWith(date);
                expect(secondMockObserver).toHaveBeenCalledTimes(0);
        });

        it('should remove all subscribers on unsubscribe', () => {
                const subject = new Subject<Date>();

                const firstMockObserver = jest.fn();
                const secondMockObserver = jest.fn();

                subject.subscribe(firstMockObserver);
                subject.subscribe(secondMockObserver);

                expect(subject.observers.length).toBe(2);

                subject.unsubscribe();

                expect(subject.observers).toBe(null);
        });
});