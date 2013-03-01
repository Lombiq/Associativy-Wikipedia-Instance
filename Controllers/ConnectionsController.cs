using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Associativy.GraphDiscovery;
using Associativy.Instances.Wikipedia.Models;
using Orchard.ContentManagement;
using Orchard.Core.Title.Models;
using Associativy.Services;

namespace Associativy.Instances.Wikipedia.Controllers
{
    public class ConnectionsController : ApiController
    {
        private readonly IGraphManager _graphManager;
        private readonly IContentManager _contentManager;

        public ConnectionsController(IGraphManager graphManager, IContentManager contentManager)
        {
            _graphManager = graphManager;
            _contentManager = contentManager;
        }


        public HttpResponseMessage Post(Connection connection)
        {
            if (connection == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No connection to save.");

            var item1 = CreatePageIfNotExists(connection.Page1);
            var item2 = CreatePageIfNotExists(connection.Page2);

            var graphContext = new GraphContext { GraphName = WikipediaGraphProvider.Name };
            var graph = _graphManager.FindGraph(graphContext);
            graph.PathServices.ConnectionManager.Connect(graphContext, item1, item2);


            var response = Request.CreateResponse<Connection>(HttpStatusCode.Created, connection);

            //var uri = Url.Link("DefaultApi", new { id = item.Id });
            //response.Headers.Location = new Uri(uri);
            return response;
        }


        private ContentItem CreatePageIfNotExists(WikipediaPage page)
        {
            var item = GetPageByUrl(page.Url);

            if (item == null)
            {
                item = _contentManager.New(WellKnownConsts.WikipediaPageContentType);
                item.As<TitlePart>().Title = page.Title;
                item.As<WikipediaPagePart>().Url = page.Url;
                _contentManager.Create(item);

                _contentManager.Flush();
            }

            return item;
        }

        private ContentItem GetPageByUrl(string url)
        {
            return _contentManager
                        .Query(WellKnownConsts.WikipediaPageContentType)
                        .Where<WikipediaPagePartRecord>(record => record.Url == url)
                        .List()
                        .FirstOrDefault();
        }
    }
}