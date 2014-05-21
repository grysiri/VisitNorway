using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Domain;

namespace Gatherer.TellusConverterHelpers
{
    public enum TellusCategories
    {
        Overnatting=1, Servering=2, ForretningerService=3, Tjenester=4, Aktiviteter=36, Arrangement=39, Attraksjoner=69, Transport=99, Sted=110, 
        Reisepakke=203, Utleie=400, Produksjon=402, Prosjekt=403, Matkultur=404, Generellekategorier=405
    }
    

    public static class Categories
    {
       
        public static Category AddCategory(string value, string parentvalue)
        {
            int parentid;
            int id;
            if (int.TryParse(value, out id) && int.TryParse(parentvalue, out parentid))
            {

// ----- OVERNATTING ----

                if (parentid == (int) TellusCategories.Overnatting)
                {
                    if (id == 3 || id == 28)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Hotell);
                    if (id == 5 || id == 421 || id == 714 || id == 1153 || id == 2804)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Camping);
                    if (id == 26)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Turistforeningshytte);
                    if (id == 261)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Hytter);
                    if (id == 262 || id == 774 || id == 2760)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.VandrerhjemOgHerberge);
                    if (id == 410 || id == 515 || id == 766 || id == 1166 || id == 1429 )
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.HavnMarina);
                    if (id == 809 || id == 451 || id == 845 || id == 847 || id == 1474)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.VertshusOgPensjonat);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Overnatting);
                }

// ----- SERVERING ----

                else if (parentid == (int) TellusCategories.Servering)
                {
                    if (id == 9 || id == 266)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Restauranter);
                    if (id == 10)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.BarPub);
                    if (id==12 || id == 865 || id == 904 || id == 919)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.KafeKaffebarer);
                    if (id == 317 || id == 915)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Lokaler);
                    if (id == 414 || id == 952 || id == 2407)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.GatekjøkkenTakeaway);
                    if (id == 1004 || id == 388)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Nattklubber);
                    if (id == 422)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Veikroer);
                    if (id == 697 || id == 776)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.KonditoriIskrembar);
                    if (id == 976)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Fastfood);
                    if (id == 942)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.VertshusOgPensjonat);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Servering);
                }

// ----- ARRANGEMENT ----

                else if (parentid == (int) TellusCategories.Arrangement)
                {
                    if (id == 323)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Konserter);
                    if (id == 63)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Utstillinger);
                    if (id == 41 || id == 42 || id == 46 || id == 773 || id == 777 || id == 892 || id == 893 || id == 1047 || id == 1062 || id == 1172)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Forestillinger);
                    if (id == 402 || id == 1056 || id == 1055)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.ForedragSeminar);
                    if (id == 696 || id == 771 || id == 820 || id == 1194 )
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Show);
                    if (id == 55 || id == 979)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Sport);
                    if (id == 1049 || id == 1499 || id == 2759)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Litteratur);
                    if (id == 350 || id == 726 || id == 888 || id == 889 || id == 890 || id == 1451 || id == 1096)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.MesseMarked);
                    if ( id == 407 || id == 3002)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Festivaler);
                    if (id == 681 || id == 2411)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Religiøst);
                    if (id == 682 || id == 930)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Konferanse);
                    if (id == 710 || id == 912 || id == 950 || id == 978 || id == 1024 || id == 1518 || id == 1534)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Turer);
                    if (id == 815 || id == 1114 || id == 1150 || id == 1154 || id == 1439 || id == 1440 || id == 1443)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.HøytidMerkedag);
                    if (id == 679 || id == 1046 || id == 1047 || id == 2934)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.ForBarn);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Arrangementer);
                }

// ----- AKTIVITETER ----

                if (parentid == (int) TellusCategories.Aktiviteter)
                {
                    if (id == 37 || id == 1074 || id == 2696 || id == 2740 || id == 2765 || id == 2769 || id == 2814
                         || id == 2815)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Vinteraktiviteter);
                    if (id == 38 || id == 737 || id == 917 || id == 965 || id == 966 || id == 1133 || id == 1170)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Vandring);
                    if (id == 131 || id == 137)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.JaktFiske);
                    if (id == 392 || id == 433 || id == 519 || id == 521 || id == 522 || id == 826 || id == 875
                         || id == 877 || id == 1137 || id == 1140)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Dyr);
                    if (id == 434 || id == 775 || id == 827 || id == 913 || id == 936 || id == 974 || id == 1077
                         || id == 1089 || id == 1182 || id == 1479 || id == 1480 || id == 1502 || id == 1503
                         || id == 2735 || id == 2872)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Natur);
                    if (id == 518 || id == 707 || id == 818 || id == 822 || id == 823 || id == 824 || id == 825
                         || id == 853 || id == 938 || id == 982 || id == 1533 || id == 2428 || id == 2434
                         || id == 2682 || id == 2690 || id == 2694 || id == 2736 || id == 2737 || id == 2738 || id == 2739
                         || id == 2755 || id == 2756 || id == 2757 || id == 2928)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Tur);
                    if (id == 417 || id == 529 || id == 1124 || id == 1127 || id == 1463 || id == 1466 || id == 1475 
                        || id == 2768 || id == 2826)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Båt);
                    if (id == 442 || id == 1519 || id == 2556 || id == 2558)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Parker);
                    if (id == 427 || id == 430 || id == 516 || id == 525 || id == 633 || id == 677 || id == 1013 || id == 1034
                         || id == 1036 || id == 1073 || id == 1171 || id == 1184 || id == 1196 || id == 1431 || id == 1462
                         || id == 1514 || id == 1517)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Kulturopplevelse);
                    if (id == 639 || id == 640 || id == 690 || id == 1012 || id == 1035 || id == 1118 || id == 1125
                         || id == 1507 || id == 2685 || id == 2828)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Sport);
                    if (id == 757)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Sightseeing);
                    if (id == 404 || id == 426)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Guiding);
                    if (id == 878 || id == 920 || id == 1059)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Kurs);
                    if (id == 958)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.RekreasjonBehandling);
                    if (id == 1128)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Shopping);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Aktiviteter);
                }

                if (parentid == (int) TellusCategories.Reisepakke)
                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Reisepakke);


// ----- ATTRAKSJONER ----

                if (parentid == (int) TellusCategories.Attraksjoner)
                {
                    if (id == 70 || id == 1002 || id == 1093 || id == 1512 || id == 2640)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Naturattraksjon);
                    if (id == 883 || id == 1212 || id == 1516 || id == 2409)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Natur);
                    if (id == 171 || id == 172 || id == 397 || id == 801 || id == 887 || id == 972 || id == 987
                         || id == 1044 || id == 1079 || id == 1197 || id == 1510 || id == 1529 || id == 1530 || id == 2641)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Kulturhistorisk);
                    if (id == 743)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Museum);
                    if (id == 386 || id == 1080 || id == 2401 || id == 2429)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Severdighet);
                    if (id == 236 || id == 396 || id == 401 || id == 1183)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Utstilling);
                    if (id == 744)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Kirke);
                    if (id == 730 || id == 810 || id == 882 || id == 885 || id == 944 || id == 1112 || id == 1433)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Byggverk);
                    if (id == 733 || id == 852 || id == 886)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.FestingRuiner);
                    if (id == 856 || id == 881 || id == 1081)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.ParkerHager);
                    if (id == 819 || id == 844 || id == 1129 || id == 2560 || id == 2607)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Temaparker);


                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Attraksjoner);
                }


// ----- FORRETNINGER ---- 
                if (parentid == (int) TellusCategories.ForretningerService)
                {
                    //obs: service skal til Tjenester og Service

                    if (id == 994 || id == 1063)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Planter);
                    if (id == 704)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.KlærSko);
                    if (id == 706)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Jernvare);
                    if (id == 573 || id == 861 || id == 988 || id == 1152)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Håndverk);
                    if (id == 871 || id == 872 || id == 873 || id == 1136)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Shoppingsenter);
                    if (id == 986)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Vinmonopol);
                    if (id == 995)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Kjøretøy);
                    if (id == 996 || id == 1069)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Innredning);
                    if (id == 997)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Foto);
                    if (id == 999)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Musikk);
                    if (id == 1000 || id == 2691)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.SkjønnhetHelse);
                    if (id == 572 || id == 1001)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.SportFriluft);
                    if (id == 1151)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Bensinstasjon);
                    if (id == 1524 || id == 2404 || id == 2700 || id == 2701 || id == 2873)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.MatDrikke);
                    if (id == 756)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.TjenesterOgService);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Forretninger);

                }

// ----- TJENESTER OG SERVICE ---- 
                if (parentid == (int) TellusCategories.Tjenester)
                {
                    //obs: service fra Forretninger og Service

                    if (id == 214 || id == 1531)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Bank);
                    if (id == 802 || id == 1117)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.VaskRens);
                    if (id == 271 || id == 717 || id == 1099)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Turistinformasjon);
                    if (id == 712 || id == 753 || id == 1051 || id == 1157 || id == 1158 || id == 1445)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Arrangør);
                    if (id == 691 || id == 719 || id == 837 || id == 985)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.OffentligeKommTjenester);
                    if (id == 715)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Mat);
                    if (id == 745 || id == 748)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.AmbassaderKonsulater);
                    if (id == 742)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Post);
                    if (id == 450 || id == 740 || id == 914 || id == 1102 || id == 1164)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Reise);
                    if (id == 746 || id == 747 || id == 749 || id == 935 || id == 955 || id == 959 || id == 960 || id == 1086)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Helsetjenester);
                    if (id == 922 || id == 983)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Verksted);
                    if (id == 921 || id == 850 || id == 718 || id == 998 || id == 1066 || id == 1083 || id == 1087
                         || id == 1487 || id == 1488 || id == 1489 || id == 1492 || id == 1493 || id == 1494 || id == 1495
                         || id == 1520 || id == 2436)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Fagmenn);
                    if (id == 961)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Veterinær);
                    if (id == 588 || id == 665 || id == 716 || id == 750 || id == 752 || id == 755 || id == 760)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.AndreTjenester);
                    if (id == 1178)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Taxi);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.TjenesterOgService); 
                }

                if (parentid == (int) TellusCategories.Matkultur)
                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Mat);

// ----- TRANSPORT ---- 
                if (parentid == (int) TellusCategories.Transport)
                {
                    if (id == 1021)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.OffentligTransport);
                    if (id == 438|| id == 973)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Fly);
                    if (id == 436 || id == 1014|| id == 1050|| id == 1088)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Buss);
                    if (id == 437|| id == 508 || id == 509|| id == 1048 || id == 1065)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.BåtFerge);
                    if (id == 439)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Tog);
                    if (id == 723)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Bil);
                    //if (id == )
                    //    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.TrikkBane);
                    if (id == 652)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Taxi);
                    if (id == 1134)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Helikopter);
                    if (id == 651)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.SightseeingTransport);
                    if (id == 722)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Bensinstasjon);


                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Transport);
                }

// ----- UTLEIE ----
                if (parentid == (int) TellusCategories.Utleie)
                {
                    if (id == 699|| id == 905 || id == 1497)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Campingutleie);
                    if (id == 702)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Bussutleie);
                    if (id == 708)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Bilutleie);
                    if (id == 761 || id == 1168)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Båtutleie);
                    if (id == 762 || id == 860 || id == 908)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Overnattingssteder);
                    if (id == 763)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Sykkelutleie);
                    if (id == 813 || id == 1032 || id == 1116)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Vinterutstyr);
                    if (id == 943 || id == 971)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Lokalutleie);
                    //if (id == )
                    //    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Klesutleie);
                    if (id == 1019)
                        return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Helikopterutleie);

                    return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Utleie);
                }

            }
            return DomainCategories.Categories.Single(c => c.CategoryName == DomainCategories.Annet);
        }
    }
}

/*
 * KATEGORIER
 * 
 * Overnatting
 *      Hotell
 *      Camping
 *      Hytter
 *      Turisthytte
 *      Privat
 *      VandrerhjemHerberge
 *      VertshusPensjonat
 *      Havn/Marina
 *      Ved sjøen
 *      Ute
 * 
 * Servering
 *      Restauranter
 *      FastFood
 *      BarPub
 *      Nattklubber
 *      KaféKaffebarer
 *      Kiosk
 *      Gatekjøkken og TakeAway
 *      Veikro
 *      Lokaler
 * 
 * Arrangement
 *      Konsert
 *      Utstilling
 *      Forestilling
 *      Foredrag
 *      Show
 *      Sport
 *      Litteratur
 *      MesseMarked
 *      Festival
 *      Religiøst
 *      Konferanse
 *      Tur
 *      Høytid
 * 
 * Aktiviteter
 *      Reisepakke
 *      Vinteraktiviteter
 *      Vandring
 *      JaktFiske
 *      Dyr
 *      Natur
 *      Tur
 *      Båt
 *      Parker
 *      Kulturopplevelse
 *      Sport
 *      Sightseeing
 *      Guiding
 *      Kurs
 *      RekreasjonBehandlingssenter
 *      Shopping
 *      
 * 
 * Attraksjoner
 *      Naturattraksjon
 *      Natur
 *      Kulturhistorisk
 *      Museum
 *      Severdighet
 *      Utstilling
 *      Kirke
 *      Bygning
 *      FestningRuiner
 *      ParkerHager
 *      Temapark
 *      
 * 
 * Transport
 *      OffentligTransport
 *      Fly
 *      Buss
 *      Båt
 *      Tog
 *      Bil
 *      TrikkBane
 *      Taxi
 *      Helikopter
 *      Sightseeing
 * 
 * Forretninger
 *      Planter
 *      KlærSko
 *      Jernvare
 *      Håndverk
 *      Shoppingsenter
 *      Vinmonopol
 *      Kjøretøy
 *      Innredning
 *      Foto
 *      Musikk
 *      SkjønnhetHelse
 *      SportFriluft
 *      Bensinstasjon
 *      MatDrikke
 * 
 * TjenesterService
 *      Bank
 *      VaskRens
 *      Turistinformasjon
 *      Arrangør
 *      OffentligKommunal
 *      Mat
 *      AmbassadeKonsulat
 *      Post
 *      Reise
 *      Helsetjenester
 *      Verksted
 *      Fagmenn
 *      Veterinær
 *      AndreTjenester
 * 
 * Utleie
 *      Camping
 *      Buss
 *      Bil
 *      Båt
 *      Overnatting
 *      Sykkel
 *      Vinter
 *      Lokale
 *      Klær
 *      Helikopter
 */