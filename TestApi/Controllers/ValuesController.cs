using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace TestApi
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    public class ValuesController : ApiController
    {
        [Route]
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(
                    @"
<html>
    <body>
        <h1>Docker Demo</h1>
        <img src='/images/cat-in-container.jpg'/>
    </body>
</html>"
                )
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}