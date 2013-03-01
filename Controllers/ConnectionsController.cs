﻿using System;
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
using Piedone.HelpfulLibraries.Tasks;
using Orchard.Logging;

namespace Associativy.Instances.Wikipedia.Controllers
{
    public class ConnectionsController : ApiController
    {
        private readonly IGraphManager _graphManager;
        private readonly IContentManager _contentManager;
        private readonly ILockFileManager _lockFileManager;

        public ILogger Logger { get; set; }


        public ConnectionsController(
            IGraphManager graphManager, 
            IContentManager contentManager,
            ILockFileManager lockFileManager)
        {
            _graphManager = graphManager;
            _contentManager = contentManager;
            _lockFileManager = lockFileManager;

            Logger = NullLogger.Instance;
        }


        public HttpResponseMessage Post(Connection connection)
        {
            if (connection == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No connection to save.");

            using (var lockFile1 = _lockFileManager.TryAcquireLock(connection.Page1.Title, 30000))
            using (var lockFile2 = _lockFileManager.TryAcquireLock(connection.Page2.Title, 30000))
            {
                if (lockFile1 == null || lockFile2 == null)
                {
                    var message = "Can't save Associativy Wikipedia connection between " + connection.Page1.Title + " and " + connection.Page2.Title + ". The locks for the nodes weren't released in time.";
                    Logger.Error(message);
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
                }

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