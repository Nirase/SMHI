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

        List<Station> stationSet;
        public XmlDocument Document { get; private set; }

        public DownloadRequest() 
        {
            Document = new XmlDocument();
            stationSet = new List<Station>();
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
