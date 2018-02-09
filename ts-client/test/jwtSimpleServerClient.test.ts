import {
    ServerClient,
    ClientOptions,
    Token
} from "../src/serverClient";
import { HttpResponse } from "../src/http";
import { Observable, Observer} from "../src/observable";
import createMockHttpClient from "./mocks/httpClientMock";

let clientOptions : ClientOptions = new ClientOptions();
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

    it("notify observers before and after token request success", async () => {
        let mockResponse = createSuccessResponseFrom(mockToken);
        clientOptions.httpClient = createMockHttpClient(() => mockResponse);
        let simpleServerClient: ServerClient = new ServerClient(clientOptions);
        let beforeTokenRequestObserver = jest.fn();
        simpleServerClient.onBeforeRequestAccessToken.subscribe(beforeTokenRequestObserver);        
        simpleServerClient.onRequestAccessTokenSuccess.subscribe(token => {
            expect(token).toEqual(mockToken);
        });

        let token = await simpleServerClient.requestAccessToken({userName: "foo", password:"bar"});

        expect(beforeTokenRequestObserver).toHaveBeenCalled();
    });
});

let createSuccessResponseFrom = (data: any) => {
    return new HttpResponse(200, "", JSON.stringify(data));
}


