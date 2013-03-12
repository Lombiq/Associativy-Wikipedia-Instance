using Associativy.Instances.Wikipedia.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;

namespace Associativy.Instances.Wikipedia.Drivers
{
    public class WikipediaPagePartDriver : ContentPartDriver<WikipediaPagePart>
    {
        protected override void Exporting(WikipediaPagePart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Url", part.Url);
        }

        protected override void Importing(WikipediaPagePart part, ImportContentContext context)
        {
            context.ImportAttribute(part.PartDefinition.Name, "Url", value => part.Url = value);
        }
    }
}