#r "System.Web"
#r "Newtonsoft.Json"

using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // 讀取 POST 內容
    var body = await req.Content.ReadAsStringAsync();
    // 解析 Slash Command 內容
    var form = HttpUtility.ParseQueryString(body);
    // 查詢公司名稱
    var q = form["text"];

    // TODO: 應檢查 Slash Command Token

    if (string.IsNullOrEmpty(q))
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, new
        {
            text = "*請輸入查詢的公司名稱*"
        });
    }

    log.Info($"{form["user_name"]} 查詢 {q}");

    // 產生查詢的 URLs
    var page = $"http://company.g0v.ronny.tw/index/search?q={HttpUtility.UrlEncode(q)}";
    var api = $"http://company.g0v.ronny.tw/api/search?q={HttpUtility.UrlEncode(q)}";

    using (var client = new HttpClient())
    using (var response = await client.GetAsync(api))
    using (var content = response.Content)
    {
        // 取得查詢的列表內容，TODO: 這裡只查了第一頁，後續應處理其它頁面
        var json = await content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<QueryResult>(json);
        // 產生回覆內容
        var rows = result.Data.Select(x => $"{x.統一編號} *{x.公司名稱 ?? x.商業名稱 ?? x.名稱}({x.公司狀況})* http://company.g0v.ronny.tw/id/{x.統一編號}");
        var list = string.Join("\n", rows);

        var more = string.Empty;
        if (result.Found > rows.Count()) 
        {
            more = $"\n_全部 {result.Found} 筆，僅列出 {rows.Count()} 筆，查更多: {page}_";                
        }

        return req.CreateResponse(HttpStatusCode.OK, new
        {
            text = $"`{form["user_name"]}` 查詢 `{q}`\n{list}\n{more}"
        });
    }
}

public class QueryResult
{
    [JsonProperty("data")]
    public IEnumerable<CompanyProfile> Data { get; set; }

    [JsonProperty("found")]
    public int Found { get; set; }
}

public class CompanyProfile
{
    public string 名稱 { get; set; }
    public string 公司狀況 { get; set; }
    public string 公司名稱 { get; set; }
    public string 商業名稱 { get; set; }
    public string 統一編號 { get; set; }
}
