using Newtonsoft.Json;
using System.Collections.Generic;

namespace Exchange.Common
{
    public class ChartResponse
    {
        [JsonProperty("data")]
        public Data data { get; set; }

        [JsonProperty("dataSettings")]
        public DataSetting dataSettings { get; set; }
    }

    public class DataSetting {

        [JsonProperty("useHash")]
        public bool useHash { get; set; }

        [JsonProperty("date_from")]
        public string dateFrom { get; set; }

        [JsonProperty("date_to")]
        public string dateTo { get; set; }

        [JsonProperty("start")]
        public string start { get; set; }

        [JsonProperty("end")]
        public string end { get; set; }

        [JsonProperty("graphicIndicators")]
        public string graphicIndicator { get; set; }

        [JsonProperty("hash")]
        public string hash { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("interval")]
        public string interval { get; set; }

        [JsonProperty("timeframe")]
        public long timeFrame { get; set; }
    }

    public class Data {

        [JsonProperty("hloc")]
        public HLOC HLOC { get; set; }

        [JsonProperty("v1")]
        public V1 V1 { get; set; }

        [JsonProperty("xSeries")]
        public XSeries XSeries { get; set; }
    }

    public class HLOC
    {
        [JsonProperty("LKOH")]
        public List<List<decimal?>> LKOH { get; set; }
    }

    public class V1
    {
        [JsonProperty("LKOH")]
        public List<long?> LKOH { get; set; }
    }

    public class XSeries
    {
        [JsonProperty("LKOH")]
        public List<long?> LKOH { get; set; }
    }
}
