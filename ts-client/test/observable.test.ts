import { Observable } from "../src/observable";

describe("Observable should not have observers when created", () => {
        let observable = new Observable<string>();        
        expect(observable.hasObservers()).toBe(false);
});

describe("Observable should have observers when subscribed", () => {
        let observable = new Observable<string>();
        observable.subscribe( (data: string) => {});
        expect(observable.hasObservers()).toBe(true);
});