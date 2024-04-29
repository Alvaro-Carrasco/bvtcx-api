using bvtcx_api;

async Task<int> test() {


    await Test.TestLists("b.bvoip.net:4980");
    await Test.TestLists("josevm.bvoip.net:4980");
    await Test.TestLists("alvarodevtest.bvoip.net:4980");

        //await Test.TestLists("truckinsurance.bvoip.net:4980");
        //await Test.TestLists("lutheraneasths.bvoip.net:4980");
        //await Test.TestLists("sprtherapeutics.bvoip.net:4980");
        //await Test.TestLists("vandrie.bvoip.net:4980");
        //await Test.TestLists("ovationpolymers.bvoip.net:4980");

        //await Test.TestLists("bss.bvoip.net:4980");
        //await Test.TestLists("tsdlogistics.bvoip.net:4980");
        //await Test.TestLists("aspcares.bvoip.net:4980");
        //await Test.TestLists("kwikkar-ps.bvoip.net:4980");
        //await Test.TestLists("waterskiamerica.bvoip.net:4980");

        //await Test.TestLists("101ge.bvoip.net:4980");
        //await Test.TestLists("solera.bvoip.net:4980");
        //await Test.TestLists("unicycive.bvoip.net:4980");
        //await Test.TestLists("humanism.bvoip.net:4980");
        //await Test.TestLists("ccs.bvoip.net:4980");

    //await Test.TestLists("educollect.bvoip.net:4980"); //  breaks need to wait for update
        //await Test.TestLists("bepure.bvoip.net:4980");
        //await Test.TestLists("stormwarden.bvoip.net:4980");
        //await Test.TestLists("inttech.bvoip.net:4980");
        //await Test.TestLists("hellmann2.bvoip.net:4980");

        //await Test.TestLists("sps.bvoip.net:4980");
        //await Test.TestLists("adb.bvoip.net:4980");
        //await Test.TestLists("villageofbiblehill.bvoip.net:4980");
        //await Test.TestLists("servicetreena.bvoip.net:4980");
        //await Test.TestLists("centralequipns.bvoip.net:4980");

        //await Test.TestLists("advancia.bvoip.net:4980");
        //await Test.TestLists("thechurchpegaso.bvoip.net:4980");
        //await Test.TestLists("mspft.bvoip.net:4980");
        //await Test.TestLists("wollatondentalcare.bvoip.net:4980");

        //await Test.TestLists("hughwoodinc.bvoip.net:4980");
        //await Test.TestLists("eaglepestserv.bvoip.net:4980");
        //await Test.TestLists("systemssolution.bvoip.net:4980");
        //await Test.TestLists("dpsolutions.bvoip.net:4980");
        //await Test.TestLists("firstchristianchurch.bvoip.net:4980");

    //await Test.TestLists("yellowstonevalleyanimal.bvoip.net:4980");  //  breaks need to wait for update



    return 0;
}


try {
    var i = test().Result;
} catch (Exception ex) {
    Console.WriteLine(ex);
    throw;
}