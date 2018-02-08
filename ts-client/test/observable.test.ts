import { Observable } from "../src/observable";

describe("Observable should", () => {
        it("Not have observers when first created", () => {
                let observable = new Observable<string>();
                expect(observable.hasObservers()).toBe(false);
        });

        it("should have observers when subscribed", () => {
                let observable = new Observable<string>();
                observable.subscribe((data: string) => { });
                expect(observable.hasObservers()).toBe(true);
        });        

        it("should notify observers when published", () => {
                let observable = new Observable<Date>();
                let mockObserver = jest.fn();
                
                let date = new Date();
                observable.subscribe(mockObserver);
                observable.notify(date);

                expect(mockObserver).toHaveBeenCalledWith(date);
        });

        it('should remove an observer', () => {
                let observable = new Observable<string>();
                let observer = observable.subscribe((str: string) => {});

                expect(observable.hasObservers()).toBe(true);

                let removed = observable.remove(observer);
                expect(removed).toBe(true);
                expect(observable.hasObservers()).toBe(false);
        })

});