namespace bvtcx_api {
    public class bvtcx {

        public string Url { get; set; }
        private string ApiKey { get; set; } = "ua5J7p3NH3TRROpM8IMNfRKQP4NQ6cFZfbgb_WEhzpTHaPbZ_s74t3NpnLLtDYhxg1KxbJ_zkavx---TzEStx2ZygY1eRzWtbjkqlH4b4aFlJIOfzLUGSbadYrgrzp9OSLv9N5tP413cMCmcQosbCQLY3vVF4DU7qLGRFl-DDzKLxNzgLVSyYZNupAn3OUMahM1k5ufk";
        //public RingGroups RingGroups { get; set; }
        public GenericRequests GenericRequests { get; set; }


        public bvtcx(string url) {
            Url = url;
            //RingGroups = new RingGroups(this);
            GenericRequests = new GenericRequests(url, ApiKey);
        }


    }
}
