export class HttpError extends Error {
    public statusCode: number;
    constructor(errorMessage: string, statusCode: number) {
        super(errorMessage);
        this.statusCode = statusCode;
    }
}