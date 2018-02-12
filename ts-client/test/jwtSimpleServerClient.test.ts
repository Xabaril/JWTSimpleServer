import {
    ServerClient,
    ClientOptions,
    Token
} from "../src/serverClient";
import { HttpResponse } from "../src/http";
import createMockHttpClient from "./mocks/httpClientMock";

let clientOptions: ClientOptions = new ClientOptions();
let mockToken = {
    refreshToken: "foo",
    accessToken: "bar",
    expiresAt: "5000"
};

describe("JwtSimpleServerClient should", () => {
    it("retrieve a token with configured http client", async () => {
        let mockResponse = createSuccessResponseFrom(mockToken);
        clientOptions.httpClient = createMockHttpClient(() => mockResponse);
        let simpleServerClient: ServerClient = new ServerClient(clientOptions);

        let token: Token = await simpleServerClient.requestAccessToken({ userName: "scott", password: "tiger" });

        expect(token).toEqual(mockToken);
    });

    it("notify subscribed observers before and after token request success", async () => {
        let mockResponse = createSuccessResponseFrom(mockToken);
        clientOptions.httpClient = createMockHttpClient(() => mockResponse);
        let simpleServerClient: ServerClient = new ServerClient(clientOptions);
        let beforeTokenRequestObserverOne = jest.fn();
        let beforeTokenRequestObserverTwo = jest.fn();
        let subscriptionOne = simpleServerClient.onBeforeRequestAccessToken.subscribe(beforeTokenRequestObserverOne);
        let subscriptionTwo = simpleServerClient.onBeforeRequestAccessToken.subscribe(beforeTokenRequestObserverTwo);
        simpleServerClient.onRequestAccessTokenSuccess.subscribe(token => {
            expect(token).toEqual(mockToken);
        });

        subscriptionTwo.unsubscribe();

        let token = await simpleServerClient.requestAccessToken({ userName: "foo", password: "bar" });

        expect(beforeTokenRequestObserverOne).toHaveBeenCalled();
        expect(beforeTokenRequestObserverTwo).toHaveBeenCalledTimes(0);
    });

    it("notify subscribed observers before and after refresh token request success", async () => {
        let mockResponse = createSuccessResponseFrom(mockToken);
        clientOptions.httpClient = createMockHttpClient(() => mockResponse);
        let simpleServerClient: ServerClient = new ServerClient(clientOptions);
        let beforeRefreshTokenRequestObserverOne = jest.fn();
        let beforeRefreshTokenRequestObserverTwo = jest.fn();
        let subscriptionOne = simpleServerClient.onBeforeRequestRefreshToken.subscribe(beforeRefreshTokenRequestObserverOne);
        let subscriptionTwo = simpleServerClient.onBeforeRequestRefreshToken.subscribe(beforeRefreshTokenRequestObserverTwo);
        simpleServerClient.onRequestRefreshTokenSuccess.subscribe(token => {
            expect(token).toEqual(mockToken);
        });

        subscriptionTwo.unsubscribe();

        let token = await simpleServerClient.refreshAccessToken(mockToken);

        expect(beforeRefreshTokenRequestObserverOne).toHaveBeenCalled();
        expect(beforeRefreshTokenRequestObserverTwo).toHaveBeenCalledTimes(0);
    });
});

let createSuccessResponseFrom = (data: any) => {
    return new HttpResponse(200, "", JSON.stringify(data));
}
