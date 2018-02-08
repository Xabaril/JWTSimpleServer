export class HttpResponse {
    constructor(
        public readonly statusCode: number,
        public readonly statusText: string,
        public readonly content: string) {
    }
}