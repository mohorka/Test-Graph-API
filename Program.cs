using System;
using Microsoft.Graph;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfidentialGraphClientAuthenticationProviderConfiguration();
            var authProvider = new ConfidentialGraphClientAuthenticationProvider(config);
            var graphClient = new GraphServiceClient(authProvider);

            var users = await graphClient.Users
            	.Request()
            	.GetAsync();
            //if (users == null){
            //    Console.WriteLine("ops");
            //}
            
            var group = new Group
            {
                Description = "First group",
                DisplayName = "Test group",
                GroupTypes = new List<String>()
                {
                    "Unified"
                },
                MailEnabled = true,
                MailNickname = "testapp",
                SecurityEnabled = false
            };

            await graphClient.Groups
                 .Request()
                 .AddAsync(group);


            // get users : https://docs.microsoft.com/en-us/graph/api/user-list?view=graph-rest-1.0&tabs=csharp

            // var client = new MSGraphClient(graphClient);

            // var input = "";

            // while (string.IsNullOrEmpty(input) || input != "exit") {
            //     switch (input) {
            //         "get-users": 

            //             break;
            //     }
            // }
        }
    }
}
