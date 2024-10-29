namespace ZhipuApi.Models.RequestModels
{
    public class VideoRequestBase
    {
        // "quality": quality,
        // "response_format": response_format,
        // "size": size,
        // "style": style,
        // "user": user,
        // public string request_id { get; private set; }
        public string model { get; private set; }
        public string prompt { get; private set; }
        public string image_url { get; private set; }
        // public int n { get; private set; }

        // public ImageRequestBase SetRequestId(string requestId)
        // {
        //     this.request_id = requestId;
        //     return this;
        // }
        public VideoRequestBase SetImage(string imageUrl)
        {
            this.image_url = imageUrl;
            return this;
        }
        public VideoRequestBase SetModel(string model)
        {
            this.model = model;
            return this;
        }
        public VideoRequestBase SetPrompt(string prompt)
        {
            this.prompt = prompt;
            return this;
        }
        // public ImageRequestBase SetN(int n)
        // {
        //     this.n = n;
        //     return this;
        // }
    }
}