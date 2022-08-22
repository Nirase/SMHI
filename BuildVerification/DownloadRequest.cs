using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuildVerification
{
    class DownloadRequest
    {
        List<Data> Stations { get; set; }
        public XmlDocument Document { get; private set; }

        public DownloadRequest() 
        {
            Document = new XmlDocument();
            Stations = new List<Data>();
        }
        public DownloadRequest(XmlDocument document)
        {
            this.Document = document;
        }

      
        public XmlDocument GetData(string url)
        {
            try
            {
                Document.Load(url);
                var nsmgr = new XmlNamespaceManager(Document.NameTable);
                nsmgr.AddNamespace("metObsIntervalData", "https://opendata.smhi.se/xsd/metobs_v1.xsd");

                if (Document == null) // If document is null, throw exception.
                    throw new Exception("Document is null");

                var values = Document.DocumentElement.SelectNodes("//metObsIntervalData:value/metObsIntervalData:value", nsmgr);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Document = null;
                return Document;
            }



            return Document;

        }
    }
}
