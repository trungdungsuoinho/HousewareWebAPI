using Newtonsoft.Json.Linq;

namespace HousewareWebAPI.Helpers.Models
{
    public class GHNResponse
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public JObject Data { get; set; }
	}
}
