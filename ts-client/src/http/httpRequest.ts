export interface HttpRequest {
    method?: string;
    url?: string;
    content?: string;
    contentType?: string;
    headers?: Map<string, string>;
}