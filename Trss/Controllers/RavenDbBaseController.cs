using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Trss.Indexes;

namespace Trss.Controllers
{
    public class RavenDbBaseController : Controller
    {
        private static readonly Lazy<IDocumentStore> LazyDocStore = new Lazy<IDocumentStore>(() =>
        {
            X509Certificate2 cer = new X509Certificate2(System.IO.File.ReadAllBytes("trss.pfx"));
            var docStore = new DocumentStore
            {
                //3.0 connection string - Url = http://localhost:8080;Database=trss
                Urls = new string[] { "https://a.kappydb.dbs.local.ravendb.net:4443" },
                Database = "trss",
                Certificate = cer
                /*DefaultDatabase = "trss"*/
            };

            docStore.Initialize();
            //docStore.RegisterListener(new UniqueConstraintsStoreListener());

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