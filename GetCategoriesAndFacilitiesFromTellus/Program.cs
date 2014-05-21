using System;
using GetCategoriesAndFacilitiesFromTellus.TellusReference24;

namespace GetCategoriesAndFacilitiesFromTellus
{
    class Program
    {
        public static void Main(string[] args)
        {
            private string apiKey = "";

            using (var client = new TellusFeedcopyv24SoapClient())
            {
                Console.WriteLine("Collecting from Østfold..");
                var ostfold = client.GetProductList(apiKey, "no", null, null, null, "no",
                    "1", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Akershus..");
                var akershus = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "2", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Oslo..");
                var oslo = client.GetProductList(apiKey, "no", null, null, null, "no",
                    "3", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Hedmark..");
                var hedmark = client.GetProductList(apiKey, "no", null, null, null, "no",
                    "4", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Oppland..");
                var oppland = client.GetProductList(apiKey, "no", null, null, null, "no",
                    "5", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Buskerud..");
                var buskerud = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "6", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Vestfold..");
                var vestfold = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "7", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Telemark..");
                var telemark = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "8", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Aust-Agder..");
                var austagder = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "9", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Vest-Agder..");
                var vestagder = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "10", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Rogaland..");
                var rogaland = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "11", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Hordaland..");
                var hordaland = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "12", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Sogn og fjordane..");
                var sognogfjordane = client.GetProductList(apiKey, "no", null, null,
                    null, "no", "14", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Møre og romsdal..");
                var møreogromsdal = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "15", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  SørTrøndelag..");
                var sørtrøndelag = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "16", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  NordTrøndelag..");
                var nordtrøndelag = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "17", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Nordland..");
                var nordland = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "18", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Troms..");
                var troms = client.GetProductList(apiKey, "no", null, null, null, "no",
                    "19", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Finnmark..");
                var finnmark = client.GetProductList(apiKey, "no", null, null, null,
                    "no", "20", null, null, null, "220", null, null);
                Console.WriteLine("Collecting from  Spitsbergen..");
                var spitsbergen = client.GetProductList(apiKey, "no", null, null, null,
                    "no", null, "2111", null, null, "220", null, null);
                Console.WriteLine("Collecting from  Bjørnøya..");
                var bjørnøya = client.GetProductList(apiKey, "no", null, null, null,
                    "no", null, "2121", null, null, "220", null, null);
                Console.WriteLine("Collecting from  Hopen..");
                var hopen = client.GetProductList(apiKey, "no", null, null, null, "no",
                    null, "2131", null, null, "220", null, null);
                Console.WriteLine("Collecting from  Jan Mayen..");
                var janmayen = client.GetProductList(apiKey, "no", null, null, null,
                    "no", null, "2211", null, null, "220", null, null);

                var alle = new[]
                {
                    ostfold, akershus, oslo, hedmark, oppland, buskerud, vestfold, telemark, austagder, vestagder,
                    rogaland, hordaland,
                    sognogfjordane, møreogromsdal, sørtrøndelag, nordtrøndelag, nordland, troms, finnmark, spitsbergen,
                    bjørnøya, hopen, janmayen
                };


                var cats = new GetAllCategories();
                cats.Get(alle);

                var facs = new GetAllFacilities();
                facs.Get(alle);
                Console.ReadKey();
            }

        }
    }
}
