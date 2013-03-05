using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.EventHandlers;
using Associativy.Instances.Wikipedia.Models;

namespace Associativy.Instances.Wikipedia
{
    public class NodeManagerEventHander : INodeManagerEventHander
    {
        public void QueryBuilt(QueryBuiltContext context)
        {
            if (context.GraphDescriptor.Name != WikipediaGraphProvider.Name) return;

            context.Query.Join<WikipediaPagePartRecord>().WithQueryHintsFor(WellKnownConsts.WikipediaPageContentType); 
        }
    }
}