using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ApiMeta.Common.Filters
{
    public class OutputCache : ActionFilterAttribute
    {
        // cache length in seconds
        private int timespan = 0;

        // client cache length in seconds
        private int clientTimeSpan = 0;

        // cache for anonymous users only?
        private bool anonymousOnly = false;

        // cache key
        private string cachekey = string.Empty;

        // cache repository
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        Func<int, HttpActionContext, bool, bool> isCachingTimeValid = (timespan, ac, anonymous) =>
        {
            if (timespan > 0)
            {
                if (anonymous)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;

                if (ac.Request.Method == HttpMethod.Get) return true;
            }

            return false;
        };

        private CacheControlHeaderValue SetClientCache()
        {
            var cachecontrol = new CacheControlHeaderValue();
            cachecontrol.MaxAge = TimeSpan.FromSeconds(clientTimeSpan);
            cachecontrol.MustRevalidate = true;
            return cachecontrol;
        }

        public OutputCache(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            timespan = timespan;
            clientTimeSpan = clientTimeSpan;
            anonymousOnly = anonymousOnly;
        }

        public override void OnActionExecuting(HttpActionContext ac)
        {
            if (ac != null)
            {
                if (isCachingTimeValid(timespan, ac, anonymousOnly))
                {
                    cachekey = string.Join(":", new string[] { ac.Request.RequestUri.PathAndQuery, ac.Request.Headers.Accept.FirstOrDefault().ToString() });

                    if (WebApiCache.Contains(cachekey))
                    {
                        var val = WebApiCache.Get(cachekey) as string;

                        if (val != null)
                        {
                            var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(cachekey + ":response-ct");
                            if (contenttype == null)
                                contenttype = new MediaTypeHeaderValue(cachekey.Split(':')[1]);

                            ac.Response = ac.Request.CreateResponse();
                            ac.Response.Content = new StringContent(val);

                            ac.Response.Content.Headers.ContentType = contenttype;
                            return;
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!(WebApiCache.Contains(cachekey)) && !string.IsNullOrWhiteSpace(cachekey))
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                WebApiCache.Add(cachekey, body, DateTime.Now.AddSeconds(timespan));
                WebApiCache.Add(cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(timespan));
            }

            if (isCachingTimeValid(clientTimeSpan, actionExecutedContext.ActionContext, anonymousOnly))
                actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        }
    }
}