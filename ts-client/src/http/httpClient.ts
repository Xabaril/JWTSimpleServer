import { HttpResponse } from "./httpResponse";
import { HttpRequest } from "./httpRequest";
import { HttpError } from "./httpError";

export abstract class HttpClient {
    post(url: string, options: HttpRequest): Promise<HttpResponse> {
        return this.send({
            ...options,
            method: "POST",
            url
        });
    }
    public abstract send(request: HttpRequest): Promise<HttpResponse>;
}

export class XMLHttpRequestClient extends HttpClient {
    public send(request: HttpRequest): Promise<HttpResponse> {
        return new Promise((resolve, reject) => {
            const xhr = new XMLHttpRequest();
            xhr.setRequestHeader("X-Request-Client", "XMLHttpClient");
            xhr.onload = () => {
                if (xhr.status >= 200 && xhr.status < 300) {
                    resolve(new HttpResponse(xhr.status, xhr.statusText, xhr.response || xhr.responseText));
                } else {
                    reject(new HttpError(xhr.statusText, xhr.status));
                }
            }
            xhr.onerror = () => {
                reject(new HttpError(xhr.statusText, xhr.status));
            }

            xhr.ontimeout = () => {
                reject( new HttpError("Operation timeout", 500));
            }
        });
    }
    
}