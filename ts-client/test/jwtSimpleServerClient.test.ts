import {
    JwtSimpleServerClient,
    SimpleServerClientOptions,
    Token
} from "../src/JwtSimpleServerClient";
import { HttpResponse } from "../src/http";
import { Observable, Observer} from "../src/observable";
import createMockHttpClient from "./mocks/httpClientMock";

let simpleServerOptions: SimpleServerClientOptions = new SimpleServerClientOptions();
let mockToken = {
    refreshToken: "foo",
    accessToken: "bar",
    expiresAt: "5000"
};

describe("JwtSimpleServerClient should", () => {
    it("retrieve a token with configured http client", async () => {
        let mockResponse = createSuccessResponseFrom(mockToken);
        simpleServerOptions.httpClient = createMockHttpClient(() => mockResponse);
        let simpleServerClient: JwtSimpleServerClient = JwtSimpleServerClient.Create(simpleServerOptions);

        let token: Token = await simpleServerClient.requestAccessToken({ userName: "scott", password: "tiger" });

        expect(token).toEqual(mockToken);
    });

    it("notify observers before and after token request success", async () => {
        let mockResponse = createSuccessResponseFrom(mockToken);
        simpleServerOptions.httpClient = createMockHttpClient(() => mockResponse);
        let simpleServerClient: JwtSimpleServerClient = JwtSimpleServerClient.Create(simpleServerOptions);
        let beforeTokenRequestObserver = jest.fn();
        simpleServerClient.onBeforeRequestAccessToken.subscribe(beforeTokenRequestObserver);        
        simpleServerClient.onRequestAccessTokenSuccess.subscribe(token => {
            expect(token).toEqual(token);
        });

        let token = await simpleServerClient.requestAccessToken({userName: "foo", password:"bar"});
        
        expect(beforeTokenRequestObserver).toHaveBeenCalled();
    });
});

let createSuccessResponseFrom = (data: any) => {
    return new HttpResponse(200, "", JSON.stringify(data));
}


