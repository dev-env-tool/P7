using P7CreateRestApi.Filters;
using System.Web.Http;
using System.Web.Http.Filters;

namespace P7CreateRestApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.GetFilterProviders();
        }

        //public void Add(IFilter filter);
    }
}
