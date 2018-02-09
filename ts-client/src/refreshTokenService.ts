import { ServerClient, Token } from './serverClient';
import { Observer } from './observable';

export class RefreshTokenServiceOptions {
    constructor(
        public intervalSeconds: number | null = null,
        public refreshToken: string = "",
        public onRefreshTokenSuccessCallback?: (eventData: Token) => void) { }
}

export class RefreshTokenService {
    private _intervalSubscription: any;
    private _aborted: boolean = false;
    private _refreshSubscription?: Observer<Token>;

    constructor(private client: ServerClient) { }

    start(refreshTokenOptions: RefreshTokenServiceOptions) {

        this._aborted = false;
        this._ensureOptions(refreshTokenOptions);

        this._refreshSubscription = this.client.onRequestRefreshTokenSuccess.subscribe(token => {

            refreshTokenOptions.onRefreshTokenSuccessCallback &&
                refreshTokenOptions.onRefreshTokenSuccessCallback(token);
        });

        this._intervalSubscription = setInterval(async (): Promise<void> => {

            if (this._aborted) return;

            let token = await this.client.refreshAccessToken({ refreshToken: refreshTokenOptions.refreshToken })
            refreshTokenOptions.refreshToken = token.refresh_token;            

        }, refreshTokenOptions.intervalSeconds! * 1000);
    }

    stop() {
        this._aborted = true;
        if (this._intervalSubscription !== 0) {
            clearInterval(this._intervalSubscription);
            this._intervalSubscription = 0;
        }
        if (this._refreshSubscription) {
            this.client.onRequestAccessTokenSuccess.remove(this._refreshSubscription);
        }
    }

    private _ensureOptions(options: RefreshTokenServiceOptions): void {
        if (!options.onRefreshTokenSuccessCallback) {
            throw Error("You must provide a callback to start the RefreshTokenService");
        }
        if (!options.intervalSeconds) {
            throw Error("You must provide the refresh token interval");
        }
    }
}