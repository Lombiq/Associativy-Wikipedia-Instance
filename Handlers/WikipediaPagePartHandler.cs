using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Instances.Wikipedia.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Associativy.Instances.Wikipedia.Handlers
{
    public class WikipediaPagePartHandler: ContentHandler
    {
        public WikipediaPagePartHandler(IRepository<WikipediaPagePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}