using System.Diagnostics;

public class CodeLocationDelegatingHandler(HttpMessageHandler innerHandler) : DelegatingHandler(innerHandler)
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // using Ben.Demystifier to get clearer information
        var enhTrace = EnhancedStackTrace.Current();
        // skip 3 frames which include this delegating handler and preparing
        // the request
        var frame = (EnhancedStackFrame)enhTrace.GetFrame(3);
        var method = frame.MethodInfo.Name;
        // or using System.Diagnostics.StackTrace, but it's less clear
        // var frame = new StackFrame(0, true);
        // var method = frame.GetMethod()?.Name ?? string.Empty;
        var fileName = frame.GetFileName();
        var lineNumber = frame.GetFileLineNumber();

        request.Headers.Add("x-src-method", method);
        request.Headers.Add("x-src", $"{fileName}:{lineNumber}");

        return base.SendAsync(request, cancellationToken);
    }
}
