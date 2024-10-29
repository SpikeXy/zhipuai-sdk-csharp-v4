using System.Collections.Generic;
using System.Text.Json;

namespace ZhipuApi.Models.ResponseModels.VideoGenerateionModels
{
    public class VideoResponseData
    {
        public string model { get; set; }
        public List<VideoResponseDataItem> video_result { get; set; }
        public string task_status { get; set; }
        public string request_id { get; set; }

        public string id { get; set; }
        public static VideoResponseData FromJson(string json)
        {
            return JsonSerializer.Deserialize<VideoResponseData>(json);
        }
    }
}