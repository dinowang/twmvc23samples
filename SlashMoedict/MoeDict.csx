#r "Newtonsoft.Json"

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class QueryResult
{
    [JsonProperty("heteronyms")]
    public IEnumerable<Heteronym> Heteronyms { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("stroke_count")]
    public int StrokeCount { get; set; }

    [JsonProperty("non_radical_stroke_count")]
    public int NonRadicalStrokeCount { get; set; }

    [JsonProperty("radical")]
    public string Radical { get; set; }
}

public class Heteronym
{
    [JsonProperty("bopomofo")]
    public string Bopomofo { get; set; }

    [JsonProperty("bopomofo2")]
    public string Bopomofo2 { get; set; }

    [JsonProperty("pinyin")]
    public string Pinyin { get; set; }

    [JsonProperty("definitions")]
    public IEnumerable<HeteronymDefinition> Definitions { get; set; }
}

public class HeteronymDefinition
{
    [JsonProperty("def")]
    public string Def { get; set; }

    [JsonProperty("example")]
    public IEnumerable<string> Example { get; set; }

    [JsonProperty("quote")]
    public IEnumerable<string> Quote { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}