//using BvoipModels.RingGroups;
//using Newtonsoft.Json;

//namespace bvtcx_api {

//    public class RingGroups {

//        private readonly bvtcx bvtcx;

//        public RingGroups(bvtcx bvtcx) {
//            this.bvtcx = bvtcx;
//        }

//        public async Task<List<PbxRingGroup>> List() {
//            using (var client = new HttpClient()) {
//                using (var request = new HttpRequestMessage(HttpMethod.Get, $"https://{bvtcx.Url}/xapi/ring-groups")) {
//                    request.Headers.Add("ApiKey", bvtcx.ApiKey);
//                    var response = await client.SendAsync(request);
//                    response.EnsureSuccessStatusCode();
//                    var strResponse = await response.Content.ReadAsStringAsync();
//                    var obj = JsonConvert.DeserializeObject<List<PbxRingGroup>>(strResponse);
//                    if (obj == null) { throw new Exception("Unable to parse server response."); }
//                    return obj;
//                }
//            }
//        }

//    }

//}
