using Associativy.Extensions;
using Associativy.Instances.Wikipedia.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Associativy.Instances.Wikipedia
{
    public class Migrations : DataMigrationImpl
    {
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
                    .WithPart("TitlePart")
                    .WithLabel()
                    .WithPart(typeof(WikipediaPagePart).Name)
                    .Creatable(false)
                    .Draftable(false)
            );


            return 1;
        }
    }
}