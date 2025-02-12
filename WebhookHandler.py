from http.server import HTTPServer, BaseHTTPRequestHandler

class WebhookHandler (BaseHTTPRequestHandler):
    def do_POST(self):
        print("sended webhook request");
        return

if __name__ == "__main__":
    server_address = ("0.0.0.0", 8000)
    httpd = HTTPServer(server_address, WebhookHandler)
    print("Webhook Listening on port 8000...")
    httpd.serve_forever();
