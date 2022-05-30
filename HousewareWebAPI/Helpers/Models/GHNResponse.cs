using System.Collections.Generic;

namespace HousewareWebAPI.Helpers.Models
{
    public class GHNDistrictResponse
	{
		public int DistrictID { get; set; }
		public int ProvinceID { get; set; }
		public string DistrictName { get; set; }
		public string Code { get; set; }
		public List<string> NameExtension { get; set; }
	}
}
