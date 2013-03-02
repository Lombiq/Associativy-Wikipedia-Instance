using Associativy.Extensions;
using Associativy.Instances.Wikipedia.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Associativy.Instances.Wikipedia
{
    public class Migrations : DataMigrationImpl
    {
        private readonly IContentManager _contentManager;

        public Migrations(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }


        public int Create()
        {
            SchemaBuilder.CreateNodeToNodeConnectorRecordTable<WikipediaPageConnectorRecord>();

            SchemaBuilder.CreateTable(typeof(WikipediaPagePartRecord).Name,
                table => table
                    .ContentPartRecord()
                    .Column<string>("Url", column => column.WithLength(1024).Unique())
            ).AlterTable(typeof(WikipediaPagePartRecord).Name,
                table => table
                    .CreateIndex("Url", new string[] { "Url" })
            );

            ContentDefinitionManager.AlterTypeDefinition(WellKnownConsts.WikipediaPageContentType,
                cfg => cfg
                    .WithPart("CommonPart")
                    .WithTitleLabel()
                    .WithPart(typeof(WikipediaPagePart).Name)
                    .Creatable(false)
                    .Draftable(false)
            );

            // This is to prevent exceptions when fast subsequent calls to the API endpoint cause the same entry to be inserted into
            // the ContentTypeRecord table multiple times.
            var probe = _contentManager.New(WellKnownConsts.WikipediaPageContentType);
            _contentManager.Create(probe);
            _contentManager.Remove(probe);

            return 1;
        }
    }
}