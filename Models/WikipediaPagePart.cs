using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Associativy.Instances.Wikipedia.Models
{
    public class WikipediaPagePart : ContentPart<WikipediaPagePartRecord>
    {
        public string Url
        {
            get { return Record.Url; }
            set { Record.Url = value; }
        }
    }

    public class WikipediaPagePartRecord : ContentPartRecord
    {
        public virtual string Url { get; set; }
    }
}