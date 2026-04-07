namespace BizyPopAPIs.Utilities.CustomMiddlewares
{
    /// <summary>
    /// Middleware that fixes malformed multipart/form-data Content-Type headers
    /// missing the boundary parameter. Reads the actual boundary from the request
    /// body and patches the header so ASP.NET Core can parse the form correctly.
    /// </summary>
    public class FormContentTypeFixMiddleware(RequestDelegate next, ILogger<FormContentTypeFixMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var contentType = context.Request.ContentType;

            if (!string.IsNullOrEmpty(contentType)
                && contentType.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase)
                && !contentType.Contains("boundary", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning("Request to {Path} has multipart/form-data without boundary. Attempting fix.", context.Request.Path);

                // Enable buffering so we can read the body without consuming it
                context.Request.EnableBuffering();

                // Read just enough bytes to find the boundary (first line)
                var buffer = new byte[256];
                var bytesRead = await context.Request.Body.ReadAsync(buffer);
                context.Request.Body.Position = 0; // Reset immediately

                if (bytesRead > 2)
                {
                    var firstLine = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var lineEnd = firstLine.IndexOf('\r');
                    if (lineEnd < 0) lineEnd = firstLine.IndexOf('\n');
                    if (lineEnd > 0) firstLine = firstLine[..lineEnd];

                    if (firstLine.StartsWith("--"))
                    {
                        var boundary = firstLine[2..].Trim();
                        context.Request.ContentType = $"multipart/form-data; boundary={boundary}";
                        logger.LogInformation("Fixed Content-Type boundary: {Boundary}", boundary);
                    }
                }
            }

            await next(context);
        }
    }
}
