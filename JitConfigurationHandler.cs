using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Associativy.Frontends.Engines.Jit;
using Associativy.Frontends.Engines.Jit.ViewModels;
using Associativy.Frontends.EventHandlers;
using Associativy.Instances.Wikipedia.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;

namespace Associativy.Instances.Wikipedia
{
    public class JitConfigurationHandler : IJitConfigurationHandler
    {
        public void SetupViewModel(FrontendContext frontendContext, IContent node, NodeViewModel viewModel)
        {
            if (node.ContentItem.ContentType != "WikipediaPage") return;

            viewModel.name = node.As<ITitleAspect>().Title;
            viewModel.data["url"] = node.As<WikipediaPagePart>().Url;
        }
    }
}