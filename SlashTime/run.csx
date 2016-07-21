using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    return req.CreateResponse(HttpStatusCode.OK, new 
    {
        text = $"現在是 `{DateTime.Now.ToString("yyyy-MM-dd HH:mm")}`，假的!"
    });
}
