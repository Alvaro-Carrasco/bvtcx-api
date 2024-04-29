using BvoipModels.CallQueues;
using BvoipModels.InboundRules;
using BvoipModels.IVRs;
using BvoipModels.OutboundRules;
using BvoipModels.RingGroups;
using BvoipModels.Trunks.SipTrunks;
using BvoipModels.Users;

namespace bvtcx_api {
    internal static class Test {

        public static async Task TestLists(string url) {
            string relativePathToRoot = @"..\..\..";  // Adjust accordingly
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePathToRoot);
            string filename = Path.Combine(directory, $"test-{DateTime.Now:yyyy-MM-dd}-endpoints-api-bvtcx.txt");

            using (var interceptor = new ConsoleInterceptor(filename)) {
                var bvtcx = new bvtcx(url);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Testing: {url}");
                Console.ResetColor();
                // Prepare endpoint details
                var endpoints = new List<dynamic> {
                /// DESTINATIONS
                new EndpointDetail<PbxUserList>("xapi/users", x => $"{x.Number} | ", y => CountItems(y)),
                new EndpointDetail<PbxRingGroup>("xapi/ring-groups", x => $"- {x.Number} {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxQueue>("xapi/queues", x => $"- {x.Number} {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxIvr>("xapi/ivr", x => $"- {x.Number} {x.Name} | ", y => CountItems(y)),

                //// ROUTING
                new EndpointDetail<SipTrunk>("xapi/sip-trunks", x => $"- {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxInboundRule>("xapi/inbound-rules", x => $"- {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxOutboundRule>("xapi/outbound-rules", x => $"- {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxOutboundRule>("xapi/emergency-rules", x => $"- {x.Name} | ", y => CountItems(y)),
            };

                bool retryTests;
                int tryCount = 0;
                do {
                    tryCount++;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"TRY {tryCount}");
                    Console.ResetColor();

                    DateTime startTime = DateTime.Now;
                    Console.WriteLine($"Start Testing all endpoints for {url}: {startTime}");

                    foreach (var endpoint in endpoints) {
                        await TestEndpoint(bvtcx, endpoint.Path, endpoint.FormatItem, endpoint.CountItems, endpoint.ShowItems);
                    }

                    Console.WriteLine("All endpoint tests completed.");
                    Console.Write("Press 'r' to retry all tests or any other key to continue: ");
                    retryTests = Console.ReadLine().Trim().ToLower() == "r";
                } while (retryTests);

                // You may log additional info or write the separator here
                Console.WriteLine("\n---------------------\n");
                Console.WriteLine("End of Testing Session");
            }
        }

        private static async Task TestEndpoint<T>(bvtcx bvtcx, string path, Func<T, string> formatItem, Func<IList<T>, Task> countItems, bool showItems) {
            Console.WriteLine($"Testing Endpoint: {path}");
            var items = await bvtcx.GenericRequests.List<List<T>>(path);
            if (showItems) {
                foreach (var item in items) {
                    Console.Write(formatItem(item));
                }
            }
            await countItems(items);
        }

        private static async Task CountItems<T>(IList<T> items) {
            // Check if the items are null and handle this case appropriately
            if (items == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No items to display (items list is null).\n");
                Console.ResetColor();
                return; // Exit the method if items is null to prevent further null reference exceptions
            }

            // Existing logic for non-null items list
            if (items.Count == 0) {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            } else {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }

            Console.WriteLine($"Items count: {items.Count}\n");
            Console.ResetColor();
        }


        // Helper class to store endpoint details
        private class EndpointDetail<T> {
            public string Path { get; }
            public Func<T, string> FormatItem { get; }
            public Func<IList<T>, Task> CountItems { get; }
            public bool ShowItems { get; }

            public EndpointDetail(string path, Func<T, string> formatItem, Func<IList<T>, Task> countItems, bool showItems = false) {
                Path = path;
                FormatItem = formatItem;
                CountItems = countItems;
                ShowItems = showItems;
            }
        }
    }
}
