#r "System.Web"
#r "Newtonsoft.Json"

#load "MoeDict.csx"
#load "../Slack/Slack.csx"

using System.Net;
using System.Net.Http;
using System.Web;
using System.Text;
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
            text = "*請輸入查詢的字*"
        });
    }

    log.Info($"{form["user_name"]} 查詢 {q}");

    // 產生查詢的 URLs
    var png = $"https://www.moedict.tw/{HttpUtility.UrlEncode(q)}.png";

    var attachments = new List<Attachment>();

    if (q.Length > 1)
    {
        attachments.Add(new Attachment
        {
            AuthorName = "萌典",
            AuthorLink = "https://www.moedict.tw/",
            AuthorIcon = "https://www.moedict.tw/icon.png",
            Title = q,
            TitleLink = $"https://www.moedict.tw/{HttpUtility.UrlEncode(q)}",
            ImageUrl = $"https://www.moedict.tw/{HttpUtility.UrlEncode(q)}.png"
        });
    }

    using (var client = new HttpClient())
    {
        foreach (var letter in q)
        {
            var linkUrl = $"https://www.moedict.tw/{HttpUtility.UrlEncode("" + letter)}";
            var jsonUrl = $"https://www.moedict.tw/uni/{HttpUtility.UrlEncode("" + letter)}.json";
            var imageUrl = $"https://www.moedict.tw/{HttpUtility.UrlEncode("" + letter)}.png";

            using (var response = await client.GetAsync(jsonUrl))
            using (var content = response.Content)
            {
                // 取得查詢的列表內容，TODO: 這裡只查了第一頁，後續應處理其它頁面
                var json = await content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<QueryResult>(json);

                foreach (var heteronym in result.Heteronyms)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"`注音` {heteronym.Bopomofo} `拼音` {heteronym.Pinyin} `部首` {result.Radical} `筆劃` {result.StrokeCount} ");

                    // 各種詞性
					var partsOfSpeech = heteronym
                                            .Definitions
                                            .GroupBy(x => x.Type, x => x.Def)
                                            .Select(x => new AttachmentField
                                            {
                                                Title = x.Key,
                                                Value = string.Join("\n", Enumerable.Range(1, x.Count()).Zip(x, (n, v) => $"{n}. {v}"))
                                            });

                    attachments.Add(new Attachment
                    {
                        Title = result.Title,
                        TitleLink = linkUrl,
                        ThumbUrl = imageUrl,
                        Text = sb.ToString(),
                        Fields = partsOfSpeech,
                        MarkdownFields = new [] { "text", "pretext" }
                    });
                }
            }
        }
    }

    return req.CreateResponse(HttpStatusCode.OK, new
    {
        response_type = "in_channel",
        text = $"*{q}*",
        attachments = attachments
    });
}
