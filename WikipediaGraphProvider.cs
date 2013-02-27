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
        private readonly Func<IPathServices> _pathServicesFactory;

        public Localizer T { get; set; }

        public const string Name = "AssociativyWikipedia";


        public WikipediaGraphProvider(
            Work<ISqlConnectionManager<WikipediaPageConnectorRecord>> connectionManagerWork,
            Work<IStandardPathFinder> pathFinderWork)
        {
            _pathServicesFactory = () => new PathServices(connectionManagerWork.Value, pathFinderWork.Value);

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext describeContext)
        {
            describeContext.DescribeGraph(
                Name,
                T("Associativy Wikipedia Graph"),
                new[] { "WikipediaPage" },
                _pathServicesFactory);
        }
    }
}