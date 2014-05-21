using System.Collections.ObjectModel;
using System.Linq;

namespace DomainModels.Domain
{
    public class Category : DomainBase
    {
        public virtual Category ParentCategory { get; set; }
        public string CategoryName { get; set; }
        public virtual Collection<Product> Products { get; set; } 

        public override string ToString()
        {
            var s = CategoryName;
            if (ParentCategory!= null)
                s += " (" + ParentCategory.CategoryName + ") ";
            return s;
        }
    }


    /*
     * Static class with static collection of categories 
     * used in our solution.
     */
    public static class DomainCategories
    {
        private static int Counter = 100;
        public static Collection<Category> Categories { get; set; } //static collection of categories for use in model
        

        public static string Overnatting = "Overnatting",
            Servering = "Servering",
            Forretninger = "Forretninger",
            TjenesterOgService = "Tjenester og service",
            Aktiviteter = "Aktiviteter",
            Arrangementer = "Arrangement",
            Attraksjoner = "Attraksjoner",
            Transport = "Transport",
            Utleie = "Utleie",
            Annet = "Annet",
            Hotell = "Hotell",
            Camping = "Camping",
            Hytter = "Hytter",
            Turistforeningshytte = "Turistforeningshytter",
            VedSjøen = "Ved sjøen",
            HavnMarina = "Havn/Marina",
            Privat = "Privat",
            Ute = "Ute",
            VandrerhjemOgHerberge = "Vandrerhjem og herberge",
            VertshusOgPensjonat = "Vertshus og pensjonat",
            Restauranter = "Restauranter",
            Fastfood = "Fastfood",
            BarPub = "Barer og puber",
            Nattklubber = "Nattklubber",
            KafeKaffebarer = "Kaféer og kaffebarer",
            KonditoriIskrembar = "Konditori og iskrembarer",
            GatekjøkkenTakeaway = "Gatekjøkken og take away",
            Veikroer = "Veikroer",
            Lokaler = "Lokaler",
            Konserter = "Konserter",
            Utstillinger = "Utstillinger",
            Forestillinger = "Forestillinger",
            ForedragSeminar = "Foredrag og seminar",
            Show = "Show",
            Sportarr = "Sportsarrangementer",
            Litteratur = "Litteratur",
            MesseMarked = "Messer/Markeder",
            Festivaler = "Festivaler",
            Religiøst = "Religiøst",
            Konferanse = "Konferanser",
            Turer = "Turer",
            HøytidMerkedag = "Høytider og merkedager",
            ForBarn = "For barn",
            Reisepakke="Reisepakke", Vinteraktiviteter="Vinteraktiviteter", Vandring="Vandring",
            JaktFiske="Jakt og fiske", Dyr="Dyr", Naturaktiviteter="Naturaktiviteter", Tur="Tur", Båt="Båt", Parker="Parker",
            Kulturopplevelse="Kulturopplevelse", Sightseeing="Sightseeing", Sport="Sport",
            Guiding="Guiding", Kurs="Kurs", RekreasjonBehandling="Rekreasjon og behandlingssenter",
            Shopping="Shopping",
            Naturattraksjon="Naturattraksjon", Natur="Natur", Kulturhistorisk="Kulturhistorisk",
            Museum="Museum", Severdighet="Severdighet", Utstilling="Utstilling", Kirke="Kirke",
            Byggverk="Byggverk", FestingRuiner="Festning og ruiner", ParkerHager="Parker og hager",
            Temaparker="Temaparker",
            OffentligTransport="Offentlig transport",Fly="Fly", Buss="Buss", BåtFerge="Båt/Ferge", Tog="Tog", Bil="Bil", TrikkBane="Trikk/Bane",
            Taxi="Taxi", Helikopter="Helikopter",SightseeingTransport="Sightseeing Transport",
            Planter="Planter", KlærSko="Klær og sko", Jernvare="Jernvare", Håndverk="Håndverk", Shoppingsenter="Shoppingsenter", Vinmonopol="Vinmonopol",
            Kjøretøy="Kjøretøy", Innredning="Innredning", Foto="Foto", Musikk="Musikk", SkjønnhetHelse="Skjønnhet og helse", SportFriluft="Sport og friluft",
            Bensinstasjon="Bensinstasjon", MatDrikke="Mat og drikke",
            Bank="Bank", VaskRens="Vask og rens", Turistinformasjon="Turistinformasjon", Arrangør="Arrangør", OffentligeKommTjenester="Offentlige og kommunale tjenester",
            Mat="Mat", AmbassaderKonsulater="Ambassader og konsulater", Post="Post", Reise="Reise", Helsetjenester="Helsetjenester", Verksted="Verksted", Fagmenn="Fagmenn",
            Veterinær="Veterinær", AndreTjenester="Andre tjenester",
            Campingutleie="Campingutleie", Bussutleie="Bussutleie", Bilutleie="Bilutleie", Båtutleie="Båtutleie", Overnattingssteder="Overnattingssteder",
            Sykkelutleie="Sykkelutleie", Vinterutstyr="Vinterutstyr", Lokalutleie="Utleie av lokaler", Klesutleie="Klesutleie", Helikopterutleie="Helikopterutleie";

        static DomainCategories()
        {
            Categories = new Collection<Category>()
            {
                new Category() {CategoryName = Overnatting },
                new Category() {CategoryName = Servering },
                new Category() {CategoryName = Forretninger },
                new Category() {CategoryName = TjenesterOgService },
                new Category() {CategoryName = Aktiviteter },
                new Category() {CategoryName = Arrangementer },
                new Category() {CategoryName = Attraksjoner },
                new Category() {CategoryName = Transport },
                new Category() {CategoryName = Utleie, Id =Counter++},
                new Category() {CategoryName = Annet }
            };
            //overnatting
            Categories.Add(new Category() { CategoryName = Hotell, ParentCategory =Categories.Single(c=>c.CategoryName==Overnatting)  });
            Categories.Add(new Category() { CategoryName = Camping, ParentCategory = Categories.Single(c => c.CategoryName ==Overnatting)  });
            Categories.Add(new Category() { CategoryName = Hytter, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = Turistforeningshytte, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = VedSjøen, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = HavnMarina, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = Privat, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = Ute, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = VandrerhjemOgHerberge, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            Categories.Add(new Category() { CategoryName = VertshusOgPensjonat, ParentCategory =Categories.Single(c => c.CategoryName == Overnatting)  });
            //servering
            Categories.Add(new Category() { CategoryName = Restauranter, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = Fastfood, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = BarPub, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = Nattklubber, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = KafeKaffebarer, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = KonditoriIskrembar, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = GatekjøkkenTakeaway, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = Veikroer, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            Categories.Add(new Category() { CategoryName = Lokaler, ParentCategory =Categories.Single(c => c.CategoryName == Servering)  });
            //arrangement
            Categories.Add(new Category() { CategoryName = Konserter, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Utstillinger, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Forestillinger, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = ForedragSeminar, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Show, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Sportarr, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Litteratur, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = MesseMarked, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Festivaler, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Religiøst, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Konferanse, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = Turer, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = HøytidMerkedag, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            Categories.Add(new Category() { CategoryName = ForBarn, ParentCategory =Categories.Single(c => c.CategoryName == Arrangementer)  });
            //aktiviteter
            Categories.Add(new Category() { CategoryName = Reisepakke, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Vinteraktiviteter, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Vandring, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = JaktFiske, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Dyr, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Naturaktiviteter, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Tur, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Båt, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Parker, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Kulturopplevelse, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Sport, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Sightseeing, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Guiding, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Kurs, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = RekreasjonBehandling, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            Categories.Add(new Category() { CategoryName = Shopping, ParentCategory =Categories.Single(c => c.CategoryName == Aktiviteter)  });
            //attraksjoner
            Categories.Add(new Category() { CategoryName = Naturattraksjon, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Natur, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Kulturhistorisk, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Museum, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Severdighet, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Utstilling, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Kirke, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Byggverk, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = FestingRuiner, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = ParkerHager, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            Categories.Add(new Category() { CategoryName = Temaparker, ParentCategory =Categories.Single(c => c.CategoryName == Attraksjoner)  });
            //transport
            Categories.Add(new Category() { CategoryName = OffentligTransport, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = Fly, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = Buss, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = BåtFerge, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = Tog, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = Bil, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = TrikkBane, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = Taxi, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = Helikopter, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            Categories.Add(new Category() { CategoryName = SightseeingTransport, ParentCategory =Categories.Single(c => c.CategoryName == Transport)  });
            //forretninger
            Categories.Add(new Category() { CategoryName = Planter, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = KlærSko, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Jernvare, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Håndverk, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Shoppingsenter, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Vinmonopol, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Kjøretøy, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Innredning, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Foto, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Musikk, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = SkjønnhetHelse, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = SportFriluft, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = Bensinstasjon, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            Categories.Add(new Category() { CategoryName = MatDrikke, ParentCategory =Categories.Single(c => c.CategoryName == Forretninger)  });
            //tjenesterService
            Categories.Add(new Category() { CategoryName = Bank, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = VaskRens, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Turistinformasjon, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Arrangør, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = OffentligeKommTjenester, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Mat, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = AmbassaderKonsulater, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Post, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Reise, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Helsetjenester, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Verksted, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Fagmenn, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = Veterinær, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            Categories.Add(new Category() { CategoryName = AndreTjenester, ParentCategory =Categories.Single(c => c.CategoryName == TjenesterOgService)  });
            //utleie
            Categories.Add(new Category() { CategoryName = Campingutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Bussutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Bilutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Båtutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Overnattingssteder, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Sykkelutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Vinterutstyr, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Lokalutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Klesutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });
            Categories.Add(new Category() { CategoryName = Helikopterutleie, ParentCategory =Categories.Single(c => c.CategoryName == Utleie)  });           
        }

    }    
}