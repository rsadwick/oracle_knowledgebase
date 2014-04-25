using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsn.RightNow.Proxy;

namespace KFSampleClient
{
    class SampleKFClient {
        private RightNowKnowledgePortClient _client;
        public ClientInfoHeader _header;
        public String _knowledgeInteractionId;

        public SampleKFClient() {
            _client = new RightNowKnowledgePortClient();
            _client.ClientCredentials.UserName.UserName = "";
            _client.ClientCredentials.UserName.Password = "";

            _header = new ClientInfoHeader();
            _header.AppID = "Sample KF Client";

            _knowledgeInteractionId = _client.StartInteraction(_header, "Sample KF Client", "10.0.0.0", null, ".NET Application");
            System.Console.WriteLine("Created new interaction id: " + _knowledgeInteractionId);
        }

        public void displayMostPopularAnswers(int limit)
        {
            //create a place holder for content - no filters for search right now
            var contentSeach = new ContentSearch();
            //without supplying any parameters, default to weight as sort field
            var listResponse = _client.GetPopularContent(_header, _knowledgeInteractionId, contentSeach, limit, null);
            //request status info
            System.Console.WriteLine("**** Content Request Status info:");
            System.Console.WriteLine("* Description: " + listResponse.Status.Description);
            System.Console.WriteLine("* Elapsted Time: " + listResponse.Status.ElapsedTimeInMilliSeconds);
            System.Console.WriteLine("* Status Name: " + listResponse.Status.Status.Name);

            var count = 1;
            System.Console.WriteLine("\n ---------- Popular Answers ------------");

            foreach (var summaryContent in listResponse.SummaryContents)
            {
                System.Console.WriteLine("\n#" + count + " Title: " + summaryContent.Title);
                System.Console.WriteLine("\n#" + count + " Excerpt: " + summaryContent.Excerpt);
                System.Console.WriteLine("\n#" + count + " Url: " + summaryContent.URL);
                count++;
            }
            System.Console.WriteLine("\n-------------- End Popualr ------------");
        }

        public void searchAnswers(String seachTerms, int limit, int start)
        {
            //optional params:
            var origin = new ContentSearchOrigin();
            origin.Name = "Search Normal";

            //No filters, related, spelling suggestions, security - simple example:
            var searchResponse = _client.SearchContent(_header, _knowledgeInteractionId, seachTerms, null, false, false, limit, origin, null, null, start);
            System.Console.WriteLine("**** Content Request Status info:");
            System.Console.WriteLine("* Description: " + searchResponse.Status.Description);
            System.Console.WriteLine("* Elapsted Time: " + searchResponse.Status.ElapsedTimeInMilliSeconds);
            System.Console.WriteLine("* Status Name: " + searchResponse.Status.Status.Name);


            var count = 1;
            System.Console.WriteLine("\n ---------- Search Results ------------");

            foreach (var summaryContent in searchResponse.SummaryContents)
            {
                System.Console.WriteLine("\n#" + count + " Title: " + summaryContent.Title);
                System.Console.WriteLine("\n#" + count + " Excerpt: " + summaryContent.Excerpt);
                System.Console.WriteLine("\n#" + count + " Url: " + summaryContent.URL);
                count++;
            }
            System.Console.WriteLine("\n-------------- End Search ------------");
        }

        static void Main(string[] args)
        {
            var sampleClient = new SampleKFClient();
            //sampleClient.displayMostPopularAnswers(5);
            sampleClient.searchAnswers("returns", 10, 0);
        }
    }
}
