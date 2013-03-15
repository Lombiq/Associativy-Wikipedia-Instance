using System;
using Associativy.GraphDiscovery;
using Associativy.Neo4j.Services;
using Associativy.Services;
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
            Func<IGraphDescriptor, Uri, INeo4jPathFinder> pathFinderFactory,
            Func<IGraphDescriptor, IStandardNodeManager> nodeManagerFactory)
        {
            _graphServicesFactory = (graphDescriptor) =>
            {
                var neo4jUri = new Uri("http://localhost:7474/db/data/");

                return new GraphServices(
                    mindFactory(graphDescriptor),
                    connectionManagerFactory(graphDescriptor, neo4jUri),
                    pathFinderFactory(graphDescriptor, neo4jUri),
                    nodeManagerFactory(graphDescriptor));
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