using System;
using Hsn.RightNow.Proxy;

namespace KFSampleClient
{
    class SampleKFClient {
        private readonly RightNowKnowledgePortClient client;
        private readonly ClientInfoHeader header;
        private readonly String knowledgeInteractionId;

        private SampleKFClient() {
            client = new RightNowKnowledgePortClient();
            if(client.ClientCredentials != null) {
                client.ClientCredentials.UserName.UserName = "";
                client.ClientCredentials.UserName.Password = "";
            }

            header = new ClientInfoHeader {
                AppID = "Sample KF Client"
            };

            knowledgeInteractionId = client.StartInteraction(header, "Sample KF Client", "10.0.0.0", null, ".NET Application");
            Console.WriteLine("Created new interaction id: " + knowledgeInteractionId);
        }

        private void DisplayMostPopularAnswers(int limit)
        {
            //create a place holder for content - no filters for search right now
            var contentSeach = new ContentSearch();
            //without supplying any parameters, default to weight as sort field
            var listResponse = client.GetPopularContent(header, knowledgeInteractionId, contentSeach, limit, null);
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
                Console.WriteLine("\n#" + count + " Excerpt: " + summaryContent.ID.id);
               
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
            var searchResponse = client.SearchContent(header, knowledgeInteractionId, seachTerms, null, false, false, limit, origin, null, null, start);
            Console.WriteLine("**** Content Request Status info:");
            Console.WriteLine("* Description: " + searchResponse.Status.Description);
            Console.WriteLine("* Elapsted Time: " + searchResponse.Status.ElapsedTimeInMilliSeconds);
            Console.WriteLine("* Status Name: " + searchResponse.Status.Status.Name);

            var count = 1;
            Console.WriteLine("\n ---------- Search Results ------------");

            foreach (var summaryContent in searchResponse.SummaryContents)
            {
                Console.WriteLine("\n#" + count + " Title: " + summaryContent.Title);          
                count++;
            }
            Console.WriteLine("\n-------------- End Search ------------");
        }

        private void GetContentSample(int id)
        {
            var contentTemplate = new AnswerContent {
                ID = new ID() {
                    id = id,
                    idSpecified = true
                }
            };

            var securityOptions = new ContentSecurityOptions();

            var viewOrigin = new ContentViewOrigin {
                ID = new ID() {
                    id = id,
                    idSpecified = true
                }
            };

            var answerContent = (AnswerContent)client.GetContent(header, knowledgeInteractionId, contentTemplate, securityOptions, viewOrigin);
            Console.WriteLine("---------Answer Details------------");
            Console.WriteLine("Answer Type Name: " + answerContent.AnswerType.Name);
            Console.WriteLine("Answer Question: " + answerContent.Question);
            Console.WriteLine("Answer Solution: " + answerContent.Solution);
            Console.WriteLine("Answer Keywords: " + answerContent.Keywords);
            Console.WriteLine("--------End Answer Details---------");      
        }

        private void RateContentSample()
        {
            var contentTemplate = new AnswerContent();
            contentTemplate.ID = new ID() { id = 1, idSpecified = true };

            var contentRate = new ContentRate {
                ID = new ID() {
                    id = 1,
                    idSpecified = true
                }
            };
            var contentRateScale = new ContentRate {
                ID = new ID() {
                    id = 1,
                    idSpecified = true
                }
            };

            var status = client.RateContent(header, knowledgeInteractionId, contentTemplate, contentRate, contentRateScale);

            //Request status information
            //Description will only be set if there is a problem with the request
            Console.WriteLine("Content Request Status Information:");
            Console.WriteLine("Description: " + status.Description);
            Console.WriteLine("Elapsed Time(ms): " + status.ElapsedTimeInMilliSeconds);
            Console.WriteLine("Status Name: " + status.Status.Name);
        }

        static void Main(string[] args)
        {
            var sampleClient = new SampleKFClient();
           //sampleClient.DisplayMostPopularAnswers(5);
            sampleClient.SearchAnswers("accounts orders", 5, 0);
            //sampleClient.GetContentSample(100);
        }
    }
}
