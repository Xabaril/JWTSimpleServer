import { HttpClient, XMLHttpRequestClient } from "./http/httpClient";
import { HttpResponse } from "./http/httpResponse";
import { Observable } from './observable';

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


export class SimpleServerClientOptions {
    public tokenEndpoint: string = "/token";
    public host: string = window.location.origin;
    public httpClient?: HttpClient;
}

export class JwtSimpleServerClient {    
    private _httpClient?: HttpClient;   

    public onBeforeRequestAccessToken : Observable<void> = new Observable<void>();
    public onRequestAccessTokenSuccess : Observable<Token> = new Observable<Token>();

    public onBeforeRequestRefreshToken : Observable<void> = new Observable<void>();
    public onRequestRefreshTokenSuccess : Observable<Token> = new Observable<Token>();


    private constructor(private options: SimpleServerClientOptions) { 
        this._httpClient = options.httpClient || new XMLHttpRequestClient();      
    }    
    public async requestAccessToken(credentials: PasswordGrandTypeCredentials): Promise<Token> {
        
        this.onBeforeRequestAccessToken.notify(undefined);
        let requestContent = `grant_type=password&username=${credentials.userName}&password=${credentials.password}`;
        
        let token = await this._postTokenRequest(requestContent);
        this.onRequestAccessTokenSuccess.notify(token);

        return token;
    }
    public async refreshAccessToken(credentials: RefreshTokenGrandTypeCredentials): Promise<Token> {
        this.onBeforeRequestRefreshToken.notify(undefined);        
        let content = `grant_type=refresh_token&refresh_token=${credentials.refreshToken}`;        
        
        let token = await  this._postTokenRequest(content);
        this.onRequestRefreshTokenSuccess.notify(token);
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
