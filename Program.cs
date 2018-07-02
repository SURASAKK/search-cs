using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;

namespace SearchCs
{
    class Program
    {

        

        static string ToJsonString(Request request){
            var serializer = new DataContractJsonSerializer(typeof(Request));
            using (var stream = new MemoryStream()){
                serializer.WriteObject(stream,request);
                    return Encoding.UTF8.GetString(stream.ToArray());

            }

        }

        static string[] ToArray(string jsonString){
            var serializer = new DataContractJsonSerializer(typeof(string[]));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString))){
                var obj = serializer.ReadObject(stream) as string[];
                return obj;
            }

        }

        
        static void ShowOutput(string[] data){
            foreach (var item in data){
                Console.WriteLine(item);
            }
        }

        static async Task<string[]> SendRequest(Request request){
            var url = "http://localhost:5000/api/search/searchFile";
            using (var client = new HttpClient()){
                var requestJson = ToJsonString(request);
                var response = await client.PostAsync(url,new StringContent(requestJson,Encoding.UTF8,"application/json"));
                var returnJson = await response.Content.ReadAsStringAsync();
                var returnObject = ToArray(returnJson);
                return returnObject;
            }
            

        }  
        static async Task Main(string[] args)
        {
            var request = new Request {
                path = "/Users/surasak/Project/HelloWorld/src/SearchApi",
                pattern = "*.json"
            };

            var Result = await SendRequest(request);
            ShowOutput(Result);
        } 
    }

    [DataContract]
    class Request 
    {
        [DataMember]
        public string pattern{set; get;}
        [DataMember]
        public string path{set; get;} 
    }
}
