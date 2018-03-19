using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
                Urls = new string[] { "https://a.kappydb.dbs.local.ravendb.net:4443" },
                Database = "trss",
                Certificate = cer                
            };
        
            docStore.Initialize();
            IndexCreation.CreateIndexes(typeof(DownloadReleaseByHash).Assembly, docStore);

            return docStore;
        });


        public static IDocumentStore DocumentStore
        {
            get { return LazyDocStore.Value; }
        }

        public IDocumentSession Session { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Session = DocumentStore.OpenSession();
            Session.Advanced.UseOptimisticConcurrency = true;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
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
    }
}