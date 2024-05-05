using BvoipModels.CallQueues;
using BvoipModels.Global;
using BvoipModels.InboundRules;
using BvoipModels.IVRs;
using BvoipModels.OutboundRules;
using BvoipModels.RingGroups;
using BvoipModels.Trunks.SipTrunks;
using BvoipModels.Users;
using System.Text.RegularExpressions;
using static bvtcx_api.Test;
using static System.Net.Mime.MediaTypeNames;

namespace bvtcx_api {
    internal static class Test {

        public class DigitLength {
            public int digitLength { get; set; }
        }

        public class Peers {
            public List<SipTrunk> SipTrunks { get; set; }
            public List<Extension> Extensions { get; set; }
            public List<RingGroup> RingGroups { get; set; }
            public List<Queue> Queues { get; set; }
            public List<Ivr> Ivrs { get; set; }
            public List<Fax> Faxs { get; set; }
            public List<RoutePoint> RoutePoints { get; set; }
            public List<Parking> Parking { get; set; }
        }

        public class SipTrunk {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
            public int Type { get; set; }
        }

        public class Extension {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }

        public class RingGroup {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }

        public class Queue {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }

        public class Ivr {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }

        public class Fax {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }

        public class RoutePoint {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }

        public class Parking {
            public int Id { get; set; }
            public string Number { get; set; }
            public string Name { get; set; }
        }


        public static async Task TestLists(string url, string? fileName = null, bool enableRetry = false) {
            string relativePathToRoot = @"..\..\..";  // Adjust accordingly
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePathToRoot);

            // Determine the filename to use
            string filename = fileName ?? Path.Combine(directory, $"test-{DateTime.Now:yyyy-MM-dd}-endpoints-api-bvtcx.txt");

            using (var interceptor = new ConsoleInterceptor(filename)) {
                var bvtcx = new bvtcx(url);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"Testing: {url}");
                Console.ResetColor();


                // Prepare endpoint details
                var endpoints = new List<dynamic> {
                //// OTHERS

                /// DESTINATIONS
                new EndpointDetail<Peers>("xapi/global/peers", x => $"- {x} | ", y => CountItems(y), true, true),
                new EndpointDetail<object>("/xapi/users/100/parameters", x => $"{x} | ", y => CountItems(y)),
                new EndpointDetail<PbxUser>("xapi/users/100", x => $"{x.Number} | ", null, false, true),
                new EndpointDetail<PbxUserGroup>("xapi/groups", x => $"{x.groupName} | ", y => CountItems(y)),
                new EndpointDetail<PbxUserList>("xapi/users", x => $"{x.Number} | ", y => CountItems(y)),
                new EndpointDetail<PbxRingGroup>("xapi/ring-groups", x => $"- {x.Number} {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxQueue>("xapi/queues", x => $"- {x.Number} {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxIvr>("xapi/ivr", x => $"- {x.Number} {x.Name} | ", y => CountItems(y)),

                //// ROUTING
                new EndpointDetail<SipTrunk>("xapi/sip-trunks", x => $" {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxInboundRule>("xapi/inbound-rules", x => $"- {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxOutboundRule>("xapi/outbound-rules", x => $"- {x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxOutboundRule>("xapi/emergency-rules", x => $"- {x.Name} | ", y => CountItems(y)),

                // OTHERS
                new EndpointDetail<DigitLength>("xapi/global/ext-digit-length", x => $" {x.digitLength} | ", y => CountItems(y), true, true),
                new EndpointDetail<PbxParameter>("xapi/parameters", x => $"{x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxParameter>("xapi/parameters/search/X_BVOIP", x => $"{x.Name} | ", y => CountItems(y)),
                new EndpointDetail<PbxParameter>("xapi/parameters/DEFAULT_CONFIGURED_PUBLIC_IP", x => $"{x.Name} | ", y => CountItems(y), false, true),


            };

                // Handle retry logic based on `enableRetry` flag
                bool retryTests;
                do {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Starting tests...");
                    Console.ResetColor();

                    DateTime startTime = DateTime.Now;
                    Console.WriteLine($"Start Testing all endpoints for {url}: {startTime}");

                    foreach (var endpoint in endpoints) {
                        await TestEndpoint(bvtcx, endpoint.Path, endpoint.FormatItem, endpoint.CountItems, endpoint.ShowItems, endpoint.IsSingleObject);
                    }

                    Console.WriteLine("All endpoint tests completed.");

                    if (enableRetry) {
                        Console.Write("Press 'r' to retry all tests or any other key to continue: ");
                        retryTests = Console.ReadLine().Trim().ToLower() == "r";
                    } else {
                        retryTests = false;
                    }
                } while (retryTests);

                Console.WriteLine("\n---------------------\n");
                Console.WriteLine("End of Testing Session");
            }
        }

        private static async Task TestEndpoint<T>(bvtcx bvtcx, string path, Func<T, string> formatItem, Func<IList<T>, Task>? countItems, bool showItems, bool isSingleObject) {
            Console.WriteLine($"Testing Endpoint: {path}");
            IList<T> items;

            if (isSingleObject) {
                T item = await bvtcx.GenericRequests.GetObject<T>(path);
                items = new List<T> { item };

                if (showItems) {
                    Console.Write(formatItem(item));
                }
            } else {
                items = await bvtcx.GenericRequests.List<List<T>>(path);

                if (showItems && items != null) {
                    foreach (var item in items) {
                        Console.Write(formatItem(item));
                    }
                }
            }

            // Execute counting only if CountItems is not null
            if (countItems != null) {
                await countItems(items);
            } else {
                Console.WriteLine("Counting skipped: Single object endpoint. \n");
            }
        }

        private static async Task CountItems<T>(IList<T> items) {
            if (items == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No items to display (items list is null).\n");
                Console.ResetColor();
                return;
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

        private static async Task CountItems<T>(T item) {
            await CountItems(new List<T> { item });
        }

        // Helper class to store endpoint details
        private class EndpointDetail<T> {
            public string Path { get; }
            public Func<T, string> FormatItem { get; }
            public Func<IList<T>, Task>? CountItems { get; } // Make nullable to allow skipping
            public bool ShowItems { get; }
            public bool IsSingleObject { get; }

            public EndpointDetail(string path, Func<T, string> formatItem, Func<IList<T>, Task>? countItems = null, bool showItems = false, bool isSingleObject = false) {
                Path = path;
                FormatItem = formatItem;
                CountItems = countItems;
                ShowItems = showItems;
                IsSingleObject = isSingleObject;
            }
        }

        private static Task CountPeersItems(Peers peers) {
            int totalCount = 0;

            totalCount += peers.SipTrunks?.Count ?? 0;
            totalCount += peers.Extensions?.Count ?? 0;
            totalCount += peers.RingGroups?.Count ?? 0;
            totalCount += peers.Queues?.Count ?? 0;
            totalCount += peers.Ivrs?.Count ?? 0;
            totalCount += peers.Faxs?.Count ?? 0;
            totalCount += peers.RoutePoints?.Count ?? 0;
            totalCount += peers.Parking?.Count ?? 0;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Total items count across all peers: {totalCount}\n");
            Console.ResetColor();

            return Task.CompletedTask;
        }

    }

}
