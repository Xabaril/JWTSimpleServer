import { HttpClient, XMLHttpRequestClient } from "./http/httpClient";
import { HttpResponse } from "./http/httpResponse";
import { Subject } from './observable';

export interface Token {
    access_token: string;
    refresh_token: string;
    expires_in: string;
}

export interface PasswordGrandTypeCredentials {
    userName: string;
    password: string;
}
export interface RefreshTokenGrandTypeCredentials {
    refreshToken: string;
}

export class ClientOptions {
    public tokenEndpoint: string = "/token";
    public host: string = window.location.origin;
    public httpClient?: HttpClient;
}

export class ServerClient {
    private _httpClient?: HttpClient;

    public onBeforeRequestAccessToken: Subject<void> = new Subject<void>();
    public onRequestAccessTokenSuccess: Subject<Token> = new Subject<Token>();

    public onBeforeRequestRefreshToken: Subject<void> = new Subject<void>();
    public onRequestRefreshTokenSuccess: Subject<Token> = new Subject<Token>();

    public constructor(private options: ClientOptions) {
        this._httpClient = options.httpClient || new XMLHttpRequestClient();
    }
    public async requestAccessToken(credentials: PasswordGrandTypeCredentials): Promise<Token> {

        this.onBeforeRequestAccessToken.next(undefined);
        let requestContent = `grant_type=password&username=${credentials.userName}&password=${credentials.password}`;

        let token = await this._postTokenRequest(requestContent);
        this.onRequestAccessTokenSuccess.next(token);

        return token;
    }
    public async refreshAccessToken(credentials: RefreshTokenGrandTypeCredentials): Promise<Token> {
        this.onBeforeRequestRefreshToken.next(undefined);
        let content = `grant_type=refresh_token&refresh_token=${credentials.refreshToken}`;

        let token = await this._postTokenRequest(content);
        this.onRequestRefreshTokenSuccess.next(token);
        return token;
    }

    private async _postTokenRequest(content: string): Promise<Token> {
        let { host, tokenEndpoint } = this.options;
        let response = await this._httpClient!.post(`${host}${tokenEndpoint}`, {
            content
        });
        return this._buildTokenFromResponse(response);
    }

    private _buildTokenFromResponse(response: HttpResponse): Token {
        return (<Token>JSON.parse(response.content));
    }
}
