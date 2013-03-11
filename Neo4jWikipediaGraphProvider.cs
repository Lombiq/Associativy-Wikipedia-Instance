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
        private readonly Func<IGraphDescriptor, IGraphServices> _graphServicesFactory;

        public Localizer T { get; set; }

        public const string Name = "AssociativyNeo4jWikipedia";


        public Neo4jWikipediaGraphProvider(
            Func<IGraphDescriptor, IStandardMind> mindFactory,
            Func<IGraphDescriptor, Uri, INeo4jConnectionManager> connectionManagerFactory,
            Func<IGraphDescriptor, IStandardPathFinder> pathFinderFactory,
            Func<IGraphDescriptor, IStandardNodeManager> nodeManagerFactory,
            Func<IGraphDescriptor, INeo4jGraphStatisticsService> graphStatisticsServiceFactory)
        {
            _graphServicesFactory = (graphDescriptor) =>
            {
                return new GraphServices(
                    mindFactory(graphDescriptor),
                    connectionManagerFactory(graphDescriptor, new Uri("http://localhost:7474/db/data/")),
                    pathFinderFactory(graphDescriptor),
                    nodeManagerFactory(graphDescriptor),
                    graphStatisticsServiceFactory(graphDescriptor));
            };

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext describeContext)
        {
            describeContext.DescribeGraph(
                Name,
                T("Associativy Neo4j Wikipedia Graph"),
                new[] { "WikipediaPage" },
                _graphServicesFactory);
        }
    }
}