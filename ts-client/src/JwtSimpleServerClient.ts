import { HttpClient, XMLHttpRequestClient } from "./http/httpClient";
import { HttpResponse } from "./http/httpResponse";

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
    private _httpClient: HttpClient;
    public constructor(private options: SimpleServerClientOptions) {
        this._httpClient = options.httpClient || new XMLHttpRequestClient();
    }
    public async requestAccessToken(credentials: PasswordGrandTypeCredentials): Promise<Token> {
        let requestContent = `grand_type=password&username=${credentials.userName}&password=${credentials.password}}`;
        return this._postTokenRequest(requestContent);
    }
    public async refreshAccessToken(credentials: RefreshTokenGrandTypeCredentials): Promise<Token> {             
        let content = `grand_type=refresh_token&access_token=${credentials.refreshToken}`;
        return this._postTokenRequest(content);        
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