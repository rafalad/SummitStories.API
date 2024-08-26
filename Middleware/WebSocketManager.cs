using System.Net.WebSockets;
using System.Text;

namespace SummitStories.Api.Middleware
{
    public class WebSocketManager
    {
        private readonly RequestDelegate _next;

        public WebSocketManager(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await HandleWebSocketAsync(webSocket);
            }
            else
            {
                await _next(context);
            }
        }

        private static async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            try
            {
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                while (!result.CloseStatus.HasValue)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var responseMessage = Encoding.UTF8.GetBytes($"Server: {message}");

                    await webSocket.SendAsync(new ArraySegment<byte>(responseMessage, 0, responseMessage.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}");
            }
            finally
            {
                webSocket.Dispose();
            }
        }
    }
}
