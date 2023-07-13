using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Bookzone.Models.Entity.Document;
using Ebook.Models.Entity.Document;
using MARC4J.Net.MARC;
using EbookC = Bookzone.Models.Entity.Document.Ebook;

namespace Bookzone.Models.BLL
{
    public class BLLMarc
    {
        public static IRecord _Record;
        public static string GetTitle(IRecord record)
        {
            DataField field = (DataField) record.GetVariableField("245");
            Regex reg = new Regex("[^a-zA-Z' ]");
            var Title = reg.Replace(field.GetSubfield('a').Data, string.Empty);

            return Title;
        }
        public static string GetTitle()
        {
            DataField field = (DataField) _Record.GetVariableField("245");
            Regex reg = new Regex("[^a-zA-Z' ]");
            var Title = reg.Replace(field.GetSubfield('a').Data, string.Empty);

            return Title;
        }
        public static string GetSubTitle()
        {
            string SubTitle = null;
            DataField field = (DataField) _Record.GetVariableField("245");
            Regex reg = new Regex("[^a-zA-Z' ]");
            if (field.GetSubfield('b') != null)
            {
                SubTitle = reg.Replace(field.GetSubfield('b').Data, string.Empty);
            }
            
            return SubTitle;
        }
        public static string GetDocumentType(IRecord record)
        {
            string type;
            

            if (!string.IsNullOrEmpty(BLLMarc.GetIssn(record)))
            {
                type = "Ejournal";
            }
            
            else if (!string.IsNullOrEmpty(BLLMarc.GetIsbn(record)))
            {
                type = "Ebook";
            }
            else
            {
                type = null;
            }
            
            return type;
        }
        public static string GetDocumentType()
        {
            string type;
            

            if (!string.IsNullOrEmpty(BLLMarc.GetIssn()))
            {
                type = "Ejournal";
            }
            
            else if (!string.IsNullOrEmpty(BLLMarc.GetIsbn()))
            {
                type = "Ebook";
            }
            else
            {
                type = null;
            }
            
            return type;
        }
        public static string GetAuthor()
        {
            string author = null;
            String[] tags = {"100", "110", "111"};
            var fields = _Record.GetVariableFields(tags);

            foreach (DataField VARIABLE in fields)
            {
                if (string.IsNullOrEmpty(author) && VARIABLE != null)
                {
                    Regex reg = new Regex("[^a-zA-Z' ]");
                    author = reg.Replace(VARIABLE.GetSubfield('a').Data, string.Empty);
                }
            }

            if (string.IsNullOrEmpty(author))
            {
                DataField field = (DataField) _Record.GetVariableField("700");
                if (field != null)
                {
                    Regex reg = new Regex("[^a-zA-Z' ]");
                    author = reg.Replace(field.GetSubfield('a').Data, string.Empty);                    
                }
            }
            
            return author;
        }
        public static string GetLanguage()
        {
            ControlField field = (ControlField) _Record.GetVariableField("008");
            String data = field.Data;

            String lang = data.Substring(35,3);
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var VARIABLE in cultures)
            {
                if (VARIABLE.ThreeLetterISOLanguageName == lang)
                {
                    lang = VARIABLE.DisplayName;
                }
            }
            return lang;
        }
        public static string GetAbstract()
        {
            string Abstract = null;
            DataField field = (DataField) _Record.GetVariableField("520");
            if (field != null)
            {
                Regex reg = new Regex("[^a-zA-Z' ]");
                Abstract = reg.Replace(field.GetSubfield('a').Data, string.Empty);
            }

            return Abstract;
        }
        public static string GetPublicationDate()
        {
            string PublicationDate = null;
            String[] tags = {"260", "264"};
            var fields = _Record.GetVariableFields(tags);

            foreach (DataField VARIABLE in fields)
            {
                if (string.IsNullOrEmpty(PublicationDate) && VARIABLE != null)
                {
                    if (VARIABLE.GetSubfield('c') != null)
                    {
                        var input = VARIABLE.GetSubfield('c')?.Data;
                        if (input != null)
                        {
                            var publicationDate = Regex.Match(input, @"\d+").Value;
                            PublicationDate = publicationDate;
                        }
                    }
                }
            }
            
            return PublicationDate;
        }
        public static List<string> GetKeywords()
        {
            var keywords = new List<string>();

            String[] tags = {"600", "610", "611", "630", "647", "648", "650", "651", "653", "654", "655", "656", "657", "658", "662", "688", "690", "691", "692", "693", "694", "695", "696", "697", "698", "699"};
            var fields = _Record.GetVariableFields(tags);

            foreach (DataField KeywordsX in fields)
            {
                foreach (var keyword in KeywordsX.GetSubfields())
                {
                    Regex reg = new Regex("[^a-zA-Z' ]");
                    var subfield = reg.Replace(keyword.Data, string.Empty);
                    if (!keywords.Contains(subfield))
                        keywords.Add(subfield);
                }

            }
            
            return keywords;
        }
        public static string GetPhysicalDescription()
        {
            string PhysicalDescription = null;
            DataField field = (DataField) _Record.GetVariableField("300");
            if (field != null)
            {
                foreach (var VARIABLE in field.GetSubfields())
                {
                    Regex reg = new Regex("[^a-zA-Z1-9()' ]");
                    PhysicalDescription += reg.Replace(VARIABLE.Data, string.Empty);
                }
            }

            return PhysicalDescription;
        }
        public static string GetTitleVariant()
        {
            string TitleVariant = null;
            DataField field = (DataField) _Record.GetVariableField("246");
            if (field != null)
            {
                Regex reg = new Regex("[^a-zA-Z1-9()' ]");
                TitleVariant = reg.Replace(field.GetSubfield('a').Data, string.Empty);
            }

            return TitleVariant;
        }
        public static string GetTranslator()
        {
            string Translator = null;
            DataField field = (DataField) _Record.GetVariableField("242");
            if (field != null)
            {
                Regex reg = new Regex("[^a-zA-Z1-9()' ]");
                Translator = reg.Replace(field.GetSubfield('a').Data, string.Empty);
            }

            return Translator;
        }
        public static string GetSerie()
        {
            string Serie = null;
            String[] tags = {"490", "830"};
            var fields = _Record.GetVariableFields(tags);

            foreach (DataField VARIABLE in fields)
            {
                if (string.IsNullOrEmpty(Serie) && VARIABLE != null)
                {
                    if (VARIABLE.GetSubfield('a') != null)
                    {
                        Regex reg = new Regex("[^a-zA-Z1-9()' ]");
                        Serie = reg.Replace(VARIABLE.GetSubfield('a').Data, string.Empty);
                    }
                }
            }
            
            return Serie;

        }
        public static string GetCountry()
        {
            string Country = null;
            DataField field = (DataField) _Record.GetVariableField("260");
            if (field != null)
            {
                Regex reg = new Regex("[^a-zA-Z1-9(),' ]");
                Country = reg.Replace(field.GetSubfield('a').Data, string.Empty);
            }

            return Country;
        }
        public static string GetLanguageVariants()
        {
            string lang = null;
            DataField field = (DataField) _Record.GetVariableField("546");
            if (field != null)
            {
                Regex reg = new Regex("[^a-zA-Z' ]");
                lang = reg.Replace(field.GetSubfield('a').Data, string.Empty);
                
            }

            return lang;
        }
        public static int GetVolumeNb()
        {
            int Vnb = 0;
            DataField field = (DataField) _Record.GetVariableField("490");
            if (field != null)
            {
                var input = field.GetSubfield('v')?.Data;
                if (input != null)
                    int.TryParse(Regex.Match(input, @"\d+").Value, out Vnb);
            }

            return Vnb;
        }
        public static string GetMarcRecordNumber()
        {
            string MarcRecordNumber = null;
            DataField field = (DataField) _Record.GetVariableField("010");
            if (field != null)
            {
                var input = field.GetSubfield('a')?.Data;
                if (input != null)
                    MarcRecordNumber = Regex.Match(input, @"\d+").Value;
            }

            return MarcRecordNumber;
        }
        public static List<string> GetSummary()
        {
            var summaryTitle = new List<string>();
            DataField field = (DataField) _Record.GetVariableField("505");
            if (field != null)
            {
                foreach (var VARIABLE in field.GetSubfields('t'))
                {
                    Regex reg = new Regex("[^a-zA-Z' ]");
                    var summary = reg.Replace(VARIABLE.Data, string.Empty);
                    summaryTitle.Add(summary);
                }

            }

            if (!summaryTitle.Any())
            {
                DataField field2 = (DataField) _Record.GetVariableField("505");
                if (field2 != null)
                {
                    var listfield = field2.GetSubfield('a')?.Data.Split("--");
                    if (listfield != null && listfield.Any())
                    {
                        foreach (var VARIABLE in listfield)
                        {
                            Regex reg = new Regex("[^a-zA-Z' ]");
                            var summary = reg.Replace(VARIABLE, string.Empty);
                            summaryTitle.Add(summary);
                        }
                    }
                    

                }
            }

            return summaryTitle;
        }
        public static string GetNotes()
        {
            string Notes = null;
            String[] tags =
            {
                "500", "501", "502", "504", "506", "507","508","510", "511", "512", "513", "514", "515", "516", "518",
                "521", "522", "523", "524", "525","526", "530", "532", "533", "534", "535", "536", "538","540", 
                "541", "542", "544", "545", "547","550", "552", "555", "556", "561", "562", "563", "565","567","580", 
                "581", "583", "584", "585", "586","588", "590", "591", "592", "593", "594", "595", "596","597","598",
                "599"
            };
            var fields = _Record.GetVariableFields(tags);

            foreach (DataField NotesX in fields)
            {
                foreach (var Note in NotesX.GetSubfields())
                {
                    Regex reg = new Regex("[^a-zA-Z()1-9' ]");
                    var subfield = reg.Replace(Note.Data, string.Empty);
                    Notes += subfield + ". ";
                }

            }

            return Notes;
        }
        public static string GetDoi()
        {
            string Doi = null;
            DataField field = (DataField) _Record.GetVariableField("856");
            if (field != null)
            {
                Doi = field.GetSubfield('u') != null ? field.GetSubfield('u').Data : (field.GetSubfield('z').Data != null ? field.GetSubfield('z').Data : null);
            }

            return Doi;
        }

        #region Ebook

        public static string GetIsbn(IRecord record)
        {
            string isbn = null;
            DataField field = (DataField) record.GetVariableField("020");
            if (field != null)
            {
                var input = field.GetSubfield('a')?.Data;
                if (input != null)
                    isbn = Regex.Match(input, @"\d+").Value;
            }

            return isbn;
        }
        
        public static string GetIsbn()
        {
            string isbn = null;
            DataField field = (DataField) _Record.GetVariableField("020");
            if (field != null)
            {
                var input = field.GetSubfield('a')?.Data;
                if (input != null)
                    isbn = Regex.Match(input, @"\d+").Value;
            }

            return isbn;
        }
        
        public static string GetEditionNum()
        {
            string EditionNum = null;
            DataField field = (DataField) _Record.GetVariableField("250");
            if (field != null)
            {
                EditionNum = field.GetSubfield('a').Data;
            }

            return EditionNum;
        }

        public static string GetGenre()
        {
            ControlField field = (ControlField) _Record.GetVariableField("008");
            String data = field.Data;

            String genre = data.Substring(33,1);

            switch (genre)
            {
                case "0": genre = "Not fiction";break;
                case "1": genre = "Fiction";break;
                case "d": genre = "Drama";break;
                case "e": genre = "Essays";break;
                case "f": genre = "Novels";break;
                case "h": genre = "Humor";break;
                case "i": genre = "Letters";break;
                case "p": genre = "Poetry";break;
                case "s": genre = "Speeches";break;
                default: genre = null;break;
            }

            return genre;
        }

        public static string GetCategory()
        {
            string Category = null;
            ControlField field = (ControlField) _Record.GetVariableField("007");
            if (field != null)
            {
                string data = field.Data;
                Category = data.Substring(0,1);
                switch (Category)
                {
                    case "a": Category = "Map";break;
                    case "c": Category = "Electronic resource";break;
                    case "d": Category = "Globe";break;
                    case "f": Category = "Tactile material";break;
                    case "g": Category = "Projected graphic";break;
                    case "h": Category = "Microform";break;
                    case "k": Category = "Nonprojected graphic";break;
                    case "m": Category = "Motion picture";break;
                    case "o": Category = "Kit";break;
                    case "q": Category = "Notated music";break;
                    case "r": Category = "Remote-sensing image";break;
                    case "s": Category = "Sound recording";break;
                    case "t": Category = "Text";break;
                    case "v": Category = "Videorecording";break;
                    case "z": Category = "Unspecified";break;
                    default: Category = null;break;
                }

            }



            return Category;
        }

        public static int GetNbPages()
        {
            int NbPages = 0;
            DataField field = (DataField) _Record.GetVariableField("300");
            if (field != null)
            {
                var input = field.GetSubfield('a')?.Data;
                if (input != null)
                {
                    int.TryParse(Regex.Match(input, @"\d+(?= p)|\d+(?= .p)|\d+(?=.p)").Value, out NbPages);
                    if (NbPages == 0)
                    {
                        int.TryParse(Regex.Match(input, @"\d+").Value, out NbPages);
                    }
                }
                    
            }
            return NbPages;
        }


        #endregion

        #region Ejournal

        public static string GetIssn(IRecord record)
        {
            string isbn = null;
            DataField field = (DataField) record.GetVariableField("022");
            if (field != null)
            {
                isbn = field.GetSubfield('a').Data;
            }

            return isbn;
        }
        
        public static string GetIssn()
        {
            string isbn = null;
            DataField field = (DataField) _Record.GetVariableField("022");
            if (field != null)
            {
                isbn = field.GetSubfield('a').Data;
            }

            return isbn;
        }

        public static string GetFrequency()
        {
            string Frequency = null;
            DataField field = (DataField) _Record.GetVariableField("310");
            if (field != null)
            {
                Frequency = field.GetSubfield('a').Data;
            }

            return Frequency;
        }

        public static string GetDateFirstIssue()
        {
            ControlField field = (ControlField) _Record.GetVariableField("008");
            String data = field.Data;

            String DateFirstIssue = data.Substring(7,3);
            
            if (DateFirstIssue.Contains("u") | DateFirstIssue.Length < 4)
            {
                DateFirstIssue = null;
            }

            return DateFirstIssue;
        }
        #endregion
        
        public static MarcInfo RecordApi(IRecord record)
        {
            _Record = record;

            var MarcInfoList = new MarcInfo()
            {
                Title = GetTitle(),
                DocumentSummaryTitle = GetSummary(),
                Author =  GetAuthor()
            };

            if (GetDocumentType() == "Ebook")
            {
                MarcInfoList.DocumentInfo = new DocumentInfo()
                {
                    DocumentGroup = new Document()
                    {
                        OriginalTitle = GetTitle(),
                        DocumentType = GetDocumentType(),
                        Abstract = GetAbstract(),
                        OriginalLanguage = GetLanguage(),
                        PublicationDate = GetPublicationDate(),
                        Keyword = GetKeywords(),
                        PhysicalDescription = GetPhysicalDescription(),
                        TitlesVariants = GetTitleVariant(),
                        LanguageVariants = GetLanguageVariants(),
                        Country = GetCountry(),
                        VolumeNb = GetVolumeNb(),
                        Subtitle = GetSubTitle(),
                        MarcRecordNumber = GetMarcRecordNumber(),
                        Notes = GetNotes(),
                        Translator = GetTranslator(),
                        State = GetSerie(),
                        Doi = GetDoi()
                    },
                    EbookGroup = new EbookC()
                    {
                        Isbn = GetIsbn(),
                        EditionNum = GetEditionNum(),
                        Genre = GetGenre(),
                        Category = GetCategory(),
                        NbPages = GetNbPages(),
                    },
                };
            }
            else
            {
                MarcInfoList.DocumentInfo = new DocumentInfo()
                {
                    DocumentGroup = new Document()
                    {
                        OriginalTitle = GetTitle(),
                        DocumentType = GetDocumentType(),
                        Abstract = GetAbstract(),
                        OriginalLanguage = GetLanguage(),
                        PublicationDate = GetPublicationDate(),
                        Keyword = GetKeywords(),
                        PhysicalDescription = GetPhysicalDescription(),
                        TitlesVariants = GetTitleVariant(),
                        LanguageVariants = GetLanguageVariants(),
                        VolumeNb = GetVolumeNb(),
                        Subtitle = GetSubTitle(),
                        MarcRecordNumber = GetMarcRecordNumber(),
                        Notes = GetNotes(),
                        Translator = GetTranslator(),
                        Doi = GetDoi()
                    },
                    EjournalGroup = new Ejournal()
                    {
                        Issn = GetIssn(),
                        Frequency = GetFrequency(),
                        DateFirstIssue = GetDateFirstIssue(),
                        
                    }
                };
            }
            
            return MarcInfoList;
        }

    }
}