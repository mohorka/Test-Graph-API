using System;
using Microsoft.Graph;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfidentialGraphClientAuthenticationProviderConfiguration();
            var authProvider = new ConfidentialGraphClientAuthenticationProvider(config);
            var graphClient = new GraphServiceClient(authProvider);

            var teamId = await graphClient.CreateGroup(
                "Educational Group",
                new[] {
                    ("st000001@hypnospinnergmail.onmicrosoft.com", 0),
                    ("st000002@hypnospinnergmail.onmicrosoft.com", 0),
                    ("st000003@hypnospinnergmail.onmicrosoft.com", 1),
                    ("st000004@hypnospinnergmail.onmicrosoft.com", 1),
                    ("st000005@hypnospinnergmail.onmicrosoft.com", 1),
                },
                "edugroup1"
            );

            var groupId = "9f0d5e88-a9d6-42d6-8eed-ebff637517e4";
        }
    }

    public static class GraphExtensions
    {
        public static async Task<string> CreateGroup(
                    this GraphServiceClient client,
                    string title,
                    (string PrincipalName, int Role)[] members,
                    string emailNickname)
        {
            var group = new Group
            {
                DisplayName = title,
                GroupTypes = new List<string>()
                {
                    "Unified"
                },
                MailEnabled = true,
                MailNickname = emailNickname,
                SecurityEnabled = false,
                Visibility = "Private"
            };

            var groupId = (await client.Groups
                .Request()
                .Select("id")
                .AddAsync(group)).Id;

            foreach (var (PrincipalName, Role) in members.Where(x => x.Role == 0))
            {
                var ownerId = (await client.Users[PrincipalName].Request().Select("id").GetAsync()).Id;

                await client.Groups[groupId]
                    .Owners
                    .References
                    .Request()
                    .AddAsync(new DirectoryObject
                    {
                        Id = ownerId
                    });

                Task.Delay(1000).Wait();
            }

            foreach (var (PrincipalName, Role) in members.Where(x => x.Role == 1))
            {
                var memberId = (await client.Users[PrincipalName].Request().Select("id").GetAsync()).Id;

                await client.Groups[groupId]
                    .Members
                    .References
                    .Request()
                    .AddAsync(new DirectoryObject
                    {
                        Id = memberId
                    });

                Task.Delay(1000).Wait();
            }


            client.BaseUrl = "https://graph.microsoft.com/beta";

            var team = new Team
            {
                AdditionalData = new Dictionary<string, object>()
                {
                    {"template@odata.bind", "https://graph.microsoft.com/beta/teamsTemplates('standard')"},
                    {"group@odata.bind", $"https://graph.microsoft.com/v1.0/groups('{groupId}')"}
                }
            };

            // TODO: understand why this request fails with Not Authorized response
            var teamId = (await client.Teams
                 .Request()
                .AddAsync(team)).Id;

            client.BaseUrl = "https://graph.microsoft.com/v1.0";

            return teamId;
        }
    }

    class UserFullData
    {
        public string UserPrincipalName { get; set; }
        public string Id { get; set; }
        public int Role { get; set; }
    };
}
