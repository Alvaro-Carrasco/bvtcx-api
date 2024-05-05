using bvtcx_api;
using DB_Global;
using Microsoft.EntityFrameworkCore;

async Task<int> test() {
    try {
        CentralGlobal.SetDbInfo("control-global.bvoip.net", 3307, "alvaro", "ygBYXZR0M61a3!DrT9dJfz8tWL2icseplkO_K4CjFnHA-Qvxo@hWpz6CrG0mV9nOXkAsB_Q4@UcelZj1LT5xRyfi!dP7MHw-3YugeB5lsTaq8iKo9ybJA3_hQmf1SHgECwdrnkLc4!Dj-zZWtvOU0PTaztiM2dI8gCuXpcYOj0RWQwS-36Nnb1r7ZHAx4P!JvesGU_@y", "control_portal");
        using (var gdb = new CentralGlobal()) {
            var list = await gdb.Spinups.Where(row =>
                row.HasFallback == null &&
                !row.IsFallback &&
                row.Status == 99 &&
                row.HypGroup == "UKCLOUD"
            //row.HypCloud == "VACLOUD01"
            )
            .ToListAsync();
            foreach (var item in list) {
                if (item.NetFqdn != "ramservices") {
                    await Test.TestLists($"{item.NetFqdn}.{item.NetDomain}:4980");
                }
            }
        }

    } catch (Exception ex) {
        Console.WriteLine(ex);
    }
    //await Test.TestLists("josevm.bvoip.net:4980");
    //await Test.TestLists("alvarodevtest.bvoip.net:4980");

    return 0;
}


try {
    var i = test().Result;
} catch (Exception ex) {
    Console.WriteLine(ex);
    throw;
}