import { HttpClient, XMLHttpRequestClient } from "./http/httpClient";
import { HttpResponse } from "./http/httpResponse";
import { Observable } from './observable';

export interface Token {
    accessToken: string;
    refreshToken: string;
    expiresAt: Date;
}

export interface PasswordGrandTypeCredentials {
    userName: string;
    password: string;
}
export interface RefreshTokenGrandTypeCredentials {
    refreshToken: string;
}


export class SimpleServerClientOptions {
    public tokenEndpoint: string = "/token";
    public host: string = window.location.host;
    public httpClient?: HttpClient;
}

export class JwtSimpleServerClient {

    private static _instance: JwtSimpleServerClient;
    private _httpClient: HttpClient;
    
    public onBeforeRequestAccessToken : Observable<void> = new Observable<void>();
    public onRequestAccessTokenSuccess : Observable<Token> = new Observable<Token>();

    public onBeforeRequestRefreshToken : Observable<void> = new Observable<void>();
    public onRequestRefreshTokenSuccess : Observable<Token> = new Observable<Token>();


    private constructor(private options: SimpleServerClientOptions) {
        this._httpClient = options.httpClient || new XMLHttpRequestClient();
        JwtSimpleServerClient._instance = this;
    }

    public static Create(options: SimpleServerClientOptions) {
        return JwtSimpleServerClient._instance ? JwtSimpleServerClient._instance : new JwtSimpleServerClient(options);
    }

    public static get Instance() {
        if (!JwtSimpleServerClient._instance) {
            throw Error("jwtSimpleServerClient has not being initialized");
        }
        return JwtSimpleServerClient._instance;
    }
    public async requestAccessToken(credentials: PasswordGrandTypeCredentials): Promise<Token> {
        
        this.onBeforeRequestAccessToken.notify();
        let requestContent = `grand_type=password&username=${credentials.userName}&password=${credentials.password}}`;
        
        let token =  this._postTokenRequest(requestContent);
        this.onRequestAccessTokenSuccess.notify(token);

        return token;
    }
    public async refreshAccessToken(credentials: RefreshTokenGrandTypeCredentials): Promise<Token> {
        this.onBeforeRequestRefreshToken.notify();        
        let content = `grand_type=refresh_token&access_token=${credentials.refreshToken}`;        
        
        let token =  this._postTokenRequest(content);
        this.onRequestRefreshTokenSuccess.notify(token);
        return token;
    }

    private async _postTokenRequest(content: string): Promise<Token> {
        let { host, tokenEndpoint } = this.options;
        let response = await this._httpClient.post(`${host}${tokenEndpoint}`, {
            content
        });
        return this._buildTokenFromResponse(response);
    }

    private _buildTokenFromResponse(response: HttpResponse): Token {
        return (<Token>JSON.parse(response.content));
    }
}