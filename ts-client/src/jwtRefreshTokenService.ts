import { JwtSimpleServerClient } from './JwtSimpleServerClient';
import { Token } from "./JwtSimpleServerClient";
import { Observer } from './observable';

export class RefreshTokenServiceOptions {
    intervalSeconds: number | undefined = undefined;
    refreshToken: string;
    onRefreshTokenSuccessCallback: (eventData: Token) => void;
}

export class JwtRefreshTokenService {
    private _intervalSubscription: number;
    private _aborted: boolean = false;
    private _refreshSubscription: Observer<Token>;

    constructor(private client: JwtSimpleServerClient) { }

    start(refreshTokenOptions: RefreshTokenServiceOptions) {

        this._aborted = false;
        this._ensureOptions(refreshTokenOptions);

        this._refreshSubscription = this.client.onRequestAccessTokenSuccess.subscribe(token => {
            refreshTokenOptions.onRefreshTokenSuccessCallback(token);
        });

        this._intervalSubscription = setInterval(async () => {

            if (this._aborted) return;

            let token = await this.client.refreshAccessToken({ refreshToken: refreshTokenOptions.refreshToken })
            refreshTokenOptions.refreshToken = token.refreshToken;
            this.client.onRequestRefreshTokenSuccess.notify(token);

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