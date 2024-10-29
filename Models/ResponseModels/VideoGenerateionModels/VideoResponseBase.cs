using System.Text.Json;

namespace ZhipuApi.Models.ResponseModels.VideoGenerateionModels
{
    public class VideoResponseBase
    {
        public string request_id { get; set; }
        public string id { get; set; }
        public string model { get; set; }
        public string task_status { get; set; }
        public static VideoResponseBase FromJson(string json)
        {
            return JsonSerializer.Deserialize<VideoResponseBase>(json);
        }
    }
}