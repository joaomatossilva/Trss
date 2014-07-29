using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.UniqueConstraints;
using Trss.Indexes;

namespace Trss.Controllers
{
    public class RavenDbBaseController : Controller
    {
        private static readonly Lazy<IDocumentStore> LazyDocStore = new Lazy<IDocumentStore>(() =>
        {
            var docStore = new DocumentStore
            {
                ConnectionStringName = "RavenDb",
                /*DefaultDatabase = "trss"*/
            };

            docStore.Initialize();
            docStore.RegisterListener(new UniqueConstraintsStoreListener());

            IndexCreation.CreateIndexes(typeof(DownloadReleaseByHash).Assembly, docStore);

            return docStore;
        });


        public static IDocumentStore DocumentStore
        {
            get { return LazyDocStore.Value; }
        }

        public new IDocumentSession Session { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Session = DocumentStore.OpenSession();
            Session.Advanced.UseOptimisticConcurrency = true;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            using (Session)
            {
                if (Session == null)
                    return;
                if (filterContext.Exception != null)
                    return;
                Session.SaveChanges();
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }
    }
}