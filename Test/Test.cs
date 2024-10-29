using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ZhipuApi.Models;
using ZhipuApi.Models.RequestModels;
using ZhipuApi.Models.RequestModels.FunctionModels;
using ZhipuApi.Models.RequestModels.ImageToTextModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ZhipuApi.Test
{
    public class Test
    {
        private static readonly int API_TOKEN_TTL_SECONDS = 60 * 5;
        static readonly HttpClient client = new HttpClient();
        private static string API_KEY = "";
        
        static JsonSerializerOptions DEFAULT_SERIALIZER_OPTION = new JsonSerializerOptions {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };


        private static async Task Main()
        {
            // TestCompletion();
            // TestStream();
            //TestToolsCompletion();
            // TestToolsStream();
            // TestImageGeneration();
            // TestImageToTextCompletion();
            // TestImageToTextStream();
            // TestEmbedding();
            TestVideoGeneration();
        }
        
        

        public static void TestCompletion()
        {
            var clientV4 = new ClientV4(API_KEY);
            var response = clientV4.chat.Completion(
                new TextRequestBase()
                    .SetModel("glm-4")
                    .SetMessages(new[] { new MessageItem("user", "你好，你是谁？") })
                    .SetTemperature(0.7)
                    .SetTopP(0.7)
            );
            
            Console.WriteLine(JsonSerializer.Serialize(response, DEFAULT_SERIALIZER_OPTION));
        }
        
        public static void TestStream()
        {
            var clientV4 = new ClientV4(API_KEY);
            var responseIterator = clientV4.chat.Stream(
                new TextRequestBase()
                    .SetModel("glm-4")
                    .SetMessages(new[]
                    {
                        new MessageItem("user", "1+1等于多少"),
                        new MessageItem("assistant", "1+1等于2。"),
                        new MessageItem("user", "再加2呢？"),
                    })
                    .SetTemperature(0.7)
                    .SetTopP(0.7)
            );
            
            foreach  (var response in responseIterator)
            {
                Console.WriteLine(JsonSerializer.Serialize(response, DEFAULT_SERIALIZER_OPTION));
            }
        }
        
        public static void TestToolsCompletion()
        {
            var clientV4 = new ClientV4(API_KEY);
            var response = clientV4.chat.Completion(
                new TextRequestBase()
                    .SetModel("glm-4")
                    .SetMessages(new[] { new MessageItem("user", "北京今天的天气如何？") })
                    .SetTools(new[]
                    {
                        new FunctionTool().SetName("get_weather")
                            .SetDescription("根据提供的城市名称，提供未来的天气数据")
                            .SetParameters(new FunctionParameters()
                                .AddParameter("city", ParameterType.String, "搜索的城市名称")
                                .AddParameter("days", ParameterType.Integer, "要查询的未来的天数，默认为0")
                                .SetRequiredParameter(new string[] { "city" }))
                    })
                    .SetToolChoice("auto")
                    .SetTemperature(0.7)
                    .SetTopP(0.7)
            );
            
            Console.WriteLine(JsonSerializer.Serialize(response,DEFAULT_SERIALIZER_OPTION));
        }
        
        public static void TestToolsStream()
        {
            var clientV4 = new ClientV4(API_KEY);
            var responseIterator = clientV4.chat.Stream(
                new TextRequestBase()
                    .SetModel("glm-4")
                    .SetMessages(new[] { new MessageItem("user", "北京今天的天气如何？") })
                    .SetTools(new[]
                    {
                        new FunctionTool().SetName("get_weather")
                            .SetDescription("根据提供的城市名称，提供未来的天气数据")
                            .SetParameters(new FunctionParameters()
                                .AddParameter("city", ParameterType.String, "搜索的城市名称")
                                .AddParameter("days", ParameterType.Integer, "要查询的未来的天数，默认为0")
                                .SetRequiredParameter(new string[] { "city" }))
                    })
                    .SetToolChoice("auto")
                    .SetTemperature(0.7)
                    .SetTopP(0.7)
            );
            
            foreach  (var response in responseIterator)
            {
                Console.WriteLine(JsonSerializer.Serialize(response, DEFAULT_SERIALIZER_OPTION));
            }
        }
        
        public static void TestImageGeneration()
        {
            var clientV4 = new ClientV4(API_KEY);
            var response = clientV4.images.Generation(new ImageRequestBase()
                .SetModel("cogview")
                .SetPrompt("一只可爱的科幻风格小猫咪"));
            
            Console.WriteLine(JsonSerializer.Serialize(response,DEFAULT_SERIALIZER_OPTION));

        }
        
        public static void TestImageToTextCompletion()
        {
            var clientV4 = new ClientV4(API_KEY);
            var response = clientV4.chat.Completion(
                new TextRequestBase()
                    .SetModel("cogvlm_28b")
                    .SetMessages(new []
                    {
                        new ImageToTextMessageItem("user")
                            .setText("这是什么")
                            .setImageUrl("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")
                        
                    })
                    .SetTemperature(0.7)
                    .SetTopP(0.7)
            );
            
            Console.WriteLine(JsonSerializer.Serialize(response, DEFAULT_SERIALIZER_OPTION));
        }
        
        public static void TestImageToTextStream()
        {
            var clientV4 = new ClientV4(API_KEY);
            var responseIterator = clientV4.chat.Stream(
                new TextRequestBase()
                    .SetModel("cogvlm_28b")
                    .SetMessages(new []
                    {
                        new ImageToTextMessageItem("user")
                            .setText("这是什么")
                            .setImageUrl("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")
                        
                    })
                    .SetTemperature(0.7)
                    .SetTopP(0.7)
            );
            
            foreach  (var response in responseIterator)
            {
                Console.WriteLine(JsonSerializer.Serialize(response, DEFAULT_SERIALIZER_OPTION));
            }
        }
        
        public static void TestEmbedding()
        {
            var clientV4 = new ClientV4(API_KEY);
            var response = clientV4.embeddings.Process(new EmbeddingRequestBase()
                .SetModel("embedding-2")
                .SetInput("一只可爱的科幻风格小猫咪"));
            
            Console.WriteLine(JsonSerializer.Serialize(response,DEFAULT_SERIALIZER_OPTION));

        }

        public static void TestVideoGeneration()
        {
            var clientV4 = new ClientV4(API_KEY);
            var response = clientV4.videos.Generation(new VideoRequestBase()
                .SetModel("cogvideox")
                .SetPrompt("钢铁侠在国际空间站暴打绿巨人"));
            //等待5分钟
            Thread.Sleep(1000 * 60 * 5);
            if(response != null)
            {
                var id= response.id;
                var fileResponse = clientV4.videos.GenerationFile(id);
                Console.WriteLine(JsonSerializer.Serialize(fileResponse, DEFAULT_SERIALIZER_OPTION));
            }
            else
            {
                Console.WriteLine("返回为空");
            }

            

        }
    }
}