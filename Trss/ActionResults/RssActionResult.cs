using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SyndicationFeed;
using System.Web;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;

namespace Trss.ActionResults
{
    public class RssActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            var rssFormatter = new Rss20FeedFormatter(Feed);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Body))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}