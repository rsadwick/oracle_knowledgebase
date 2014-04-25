using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hsn.RightNow.Proxy;

namespace ConsoleApplication1
{
    class Program {
        private RightNowKnowledgePortClient _client;

        public SampleKFClient() {
            _client = new RightNowKnowledgePortClient();
        }

        static void Main(string[] args)
        {
            SampleKFClient sampleClient = new SampleKFClient();
        }
    }
}
