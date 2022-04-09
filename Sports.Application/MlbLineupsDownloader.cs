using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sports.Application
{
    public class MlbLineupsDownloader
    {
        public const string BaseUrl = "https://www.baseballpress.com/lineups/";

        public async Task<string> DownloadHtml(DateTime date)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = BaseUrl + date.ToString("YYYY-MM-dd");
                using HttpResponseMessage response = await client.GetAsync( url );
                using HttpContent content = response.Content;
                string html = await content.ReadAsStringAsync();


                return html;
            }
            catch ( Exception ex )
            {
                throw;
            }
            
            
        }
        
        
        
        
    }
}