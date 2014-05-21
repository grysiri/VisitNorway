using DomainModels.Domain.Enums;

namespace Gatherer.CbisConverterHelpers
{
    public static class Language
    {
        public static LanguageCode ConvertLanguages(int exLangId)
        {
            switch (exLangId)
            {
                case 1:       //SWEDISH
                    return LanguageCode.Se;
                case 2:      //EN UNITED STATES
                    return LanguageCode.EnUs;
                case 3:      //GERMAN
                    return LanguageCode.De;
                case 4:      //DANISH
                    return LanguageCode.Dk;
                case 5:     //NORWEGIAN
                    return LanguageCode.No;
                case 6:      //FRENCH
                    return LanguageCode.Fr;
                case 7:       //Finnes ikke i CBIS?
                    return LanguageCode.En;
                case 8:      //ITALIAN
                    return LanguageCode.It;
                case 9:       //SPANISH
                    return LanguageCode.Es;
                case 10:       //FINNISH
                    return LanguageCode.Fi;
                case 11:       //JAPANESE
                    return LanguageCode.Ja;
                case 12:       //RUSSIAN
                    return LanguageCode.Ru;
                case 13:        //POLISH
                    return LanguageCode.Pl;
                case 14:      //HUNGARIAN
                    return LanguageCode.Hu;
                case 15:       //LITHUANIAN
                    return LanguageCode.Lt;
                case 16:        //LATVIAN
                    return LanguageCode.Lv;
                case 17:        //ESTONIAN
                    return LanguageCode.Et;
                case 18:       //PORTUGESE
                    return LanguageCode.Pt;
                case 19:       //DUTCH
                    return LanguageCode.Nl;
                case 20:        //CZECH
                    return LanguageCode.Cs;
                case 21:        //ENG UK/GB
                    return LanguageCode.EnGb;
                case 22:       //ENG IRELAND
                    return LanguageCode.EnIe;
                case 23:        //NOR NYNORSK
                    return LanguageCode.NnNo;
                case 24:        //ARABIC
                    return LanguageCode.Ar;
                case 25:       //HEBREW
                    return LanguageCode.He;
                case 26:        //HINDI
                    return LanguageCode.Hi;
                case 27:       //KOREAN
                    return LanguageCode.Ko;
                case 28:      //SLOVENIAN
                    return LanguageCode.Sl;
                case 29:        //TURKISH
                    return LanguageCode.Tr;
                case 30:        //CHINESE (PEOPLES REPUBLIC OF CHINA)
                    return LanguageCode.Cn;     //er China og folkets China forskjellige?
                case 31:        //ICELANDIC
                    return LanguageCode.Is;     
                case 32:        //ROMANIAN
                    return LanguageCode.Ro;     
                case 33:        //GREEK
                    return LanguageCode.El;    
                case 34:        //SLOVAK
                    return LanguageCode.Sk;     
                case 35:       //FRENCH (CANADA)
                    return LanguageCode.FrCa;     //tydeligvis forskjellig fra vanlig fransk..
                case 36:       //INDONESIAN
                    return LanguageCode.IdId;     
                default:
                    return LanguageCode.En;
            }
        }
    }
}
