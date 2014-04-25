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

            _header = new ClientInfoHeader {
                AppID = "Sample KF Client"
            };

            _knowledgeInteractionId = _client.StartInteraction(_header, "Sample KF Client", "10.0.0.0", null, ".NET Application");
            Console.WriteLine("Created new interaction id: " + _knowledgeInteractionId);
        }

        private void DisplayMostPopularAnswers(int limit)
        {
            //create a place holder for content - no filters for search right now
            var contentSeach = new ContentSearch();
            //without supplying any parameters, default to weight as sort field
            var listResponse = _client.GetPopularContent(_header, _knowledgeInteractionId, contentSeach, limit, null);
            //request status info
            Console.WriteLine("**** Content Request Status info:");
            Console.WriteLine("* Description: " + listResponse.Status.Description);
            Console.WriteLine("* Elapsted Time: " + listResponse.Status.ElapsedTimeInMilliSeconds);
            Console.WriteLine("* Status Name: " + listResponse.Status.Status.Name);

            var count = 1;
            Console.WriteLine("\n ---------- Popular Answers ------------");

            foreach (var summaryContent in listResponse.SummaryContents)
            {
                Console.WriteLine("\n#" + count + " Title: " + summaryContent.Title);
                Console.WriteLine("\n#" + count + " Excerpt: " + summaryContent.Excerpt);
                Console.WriteLine("\n#" + count + " Url: " + summaryContent.URL);
                count++;
            }
            Console.WriteLine("\n-------------- End Popualr ------------");
        }

        private void SearchAnswers(String seachTerms, int limit, int start)
        {
            //optional params:
            var origin = new ContentSearchOrigin {
                Name = "Search Normal"
            };

            //No filters, related, spelling suggestions, security - simple example:
            var searchResponse = _client.SearchContent(_header, _knowledgeInteractionId, seachTerms, null, false, false, limit, origin, null, null, start);
            Console.WriteLine("**** Content Request Status info:");
            Console.WriteLine("* Description: " + searchResponse.Status.Description);
            Console.WriteLine("* Elapsted Time: " + searchResponse.Status.ElapsedTimeInMilliSeconds);
            Console.WriteLine("* Status Name: " + searchResponse.Status.Status.Name);


            var count = 1;
            Console.WriteLine("\n ---------- Search Results ------------");

            foreach (var summaryContent in searchResponse.SummaryContents)
            {
                Console.WriteLine("\n#" + count + " Title: " + summaryContent.Title);
                Console.WriteLine("\n#" + count + " Excerpt: " + summaryContent.Excerpt);
                Console.WriteLine("\n#" + count + " Url: " + summaryContent.URL);
                count++;
            }
            Console.WriteLine("\n-------------- End Search ------------");
        }

        static void Main(string[] args)
        {
            var sampleClient = new SampleKFClient();
            sampleClient.DisplayMostPopularAnswers(5);
            sampleClient.SearchAnswers("returns", 10, 0);
        }
    }
}
