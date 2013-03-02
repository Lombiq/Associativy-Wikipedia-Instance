using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.GraphDiscovery;
using Associativy.Neo4j.Services;
using Associativy.Services;
using Orchard.Environment;
using Orchard.Environment.Extensions;
using Orchard.Localization;

namespace Associativy.Instances.Wikipedia
{
    [OrchardFeature("Associativy.Instances.Wikipedia.Neo4j")]
    public class Neo4jWikipediaGraphProvider : IGraphProvider
    {
        private readonly Func<IPathServices> _pathServicesFactory;

        public Localizer T { get; set; }

        public const string Name = "AssociativyNeo4jWikipedia";


        public Neo4jWikipediaGraphProvider(
            Work<INeo4jConnectionManager> connectionManagerWork,
            Work<INeo4jPathFinder> pathFinderWork,
            Work<IStandardPathFinder> standardPathFinderWork)
        {
            _pathServicesFactory = () =>
            {
                var connectionManager = connectionManagerWork.Value;
                connectionManager.RootUri = new Uri("http://localhost:7474/db/data/");
                return new PathServices(connectionManager, standardPathFinderWork.Value/*pathFinderWork.Value*/);
            };

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext describeContext)
        {
            describeContext.DescribeGraph(
                Name,
                T("Associativy Neo4j Wikipedia Graph"),
                new[] { "WikipediaPage" },
                _pathServicesFactory);
        }
    }
}