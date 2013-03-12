using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.GraphDiscovery;
using Associativy.Instances.Wikipedia.Models;
using Associativy.Services;
using Orchard.Environment;
using Orchard.Localization;

namespace Associativy.Instances.Wikipedia
{
    public class WikipediaGraphProvider : IGraphProvider
    {
        private readonly IGraphServicesFactory _graphServicesFactory;

        public Localizer T { get; set; }

        public const string Name = "AssociativyWikipedia";


        public WikipediaGraphProvider(IGraphServicesFactory<IStandardMind, ISqlConnectionManager<WikipediaPageConnectorRecord>, IStandardPathFinder, IStandardNodeManager> graphServicesFactory)
        {
            _graphServicesFactory = graphServicesFactory;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext describeContext)
        {
            describeContext.DescribeGraph(
                Name,
                T("Associativy Wikipedia Graph"),
                new[] { "WikipediaPage" },
                _graphServicesFactory.Factory);
        }
    }
}