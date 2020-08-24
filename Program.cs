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
            
           /* var group = new Group
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
                 .AddAsync(group);*/

         /*   var group = new Group
            {
                AdditionalData = new Dictionary<string, object>()
	            {
		            {"members@odata.bind", "[\"https://graph.microsoft.com/v1.0/directoryObjects/dd96d6df-b69a-4fd4-a768-b9ad756129eb\",\"https://graph.microsoft.com/v1.0/directoryObjects/343a930b-a144-4e22-ac69-dc4f57416a24\",\"https://graph.microsoft.com/v1.0/directoryObjects/bfca7098-1615-4e2f-85f3-579759eecd5f\"]"}
	            }
            };



        await graphClient.Groups["12a2e9ff-fe65-4b4f-b562-4480f7b7ced6"]
	        .Request()
	        .UpdateAsync(group);
        
        var _group = await graphClient.Groups["12a2e9ff-fe65-4b4f-b562-4480f7b7ced6"].Request().GetAsync();*/

        var _users = await graphClient.Users.Request()
            .Filter("startswith(userPrincipalName, 'st000001') or startswith(userPrincipalName, 'st000002') or startswith(userPrincipalName, 'st000003')")
            .GetAsync();

        foreach(User user in _users)
        {
           await graphClient.Groups["12a2e9ff-fe65-4b4f-b562-4480f7b7ced6"].Members.References.Request().AddAsync(user);
        }
  
            


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
