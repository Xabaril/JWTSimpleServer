import { HttpClient, HttpResponse, HttpRequest } from "../../src/http";

class HttpClientMock extends HttpClient {
    public mockResponseFunction?: () => HttpResponse;
    public send(request: HttpRequest): Promise<HttpResponse> {
        return new Promise<HttpResponse>((res, rej) => {
            res(this.mockResponseFunction!());
        });
    }
}
export default function createMockClient(mockResponseFunction: any): HttpClient {
    let mockHttpClient = new HttpClientMock();
    mockHttpClient.mockResponseFunction = mockResponseFunction;
    return mockHttpClient;
}
