export interface HttpRequest {
    method?: string;
    url?: string;
    content?: string;
    headers?: Map<string, string>;
}