using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Bookzone.Extensions;
using Bookzone.Models.BLL;
using Bookzone.Models.Entity.Document;
using Ebook.Models;
using Ebook.Models.Entity.Document;
using EbookC = Bookzone.Models.Entity.Document.Ebook;
using static System.String;

namespace Bookzone.Models.DAL
{
    public static class DalDocument
    {
        public static void CreateTable()
        {
            try
            {
                var cnn = DbConnection.GetConnection();
                cnn.Open();
                const string sql = "If not exists (select * from sysobjects where name = 'Document') " +
                                   "CREATE TABLE [dbo].[Document] ( [Id] BIGINT IDENTITY(1, 1) NOT NULL, [OriginalTitle] nvarchar(max) NOT NULL, [IdAuthor] BIGINT NOT NULL, [IdEditor] BIGINT NOT NULL, [IdCollection] BIGINT NOT NULL,  [Doi] nvarchar(max)  NULL, [MarcRecordNumber] nvarchar(max)  NULL, [TitlesVariants] nvarchar(max)  NULL, [Subtitle] nvarchar(max)  NULL, [Keywords] nvarchar(max) NOT NULL, [File] nvarchar(max)  NULL, [FileFormat] nvarchar(max)  NULL, [CoverPage]  nvarchar(max) DEFAULT('default.png') NOT NULL, [Url] nvarchar(max) NOT NULL, [DocumentType] nvarchar(max) NOT NULL, [OriginalLanguage] nvarchar(max) NOT NULL, [LanguageVariants] nvarchar(max)  NULL, [Translator] nvarchar(max)  NULL, [AccessType] nvarchar(max) NOT NULL, [State] nvarchar(max)  NULL, [Price] FLOAT (53) NOT NULL, [PublicationDate] nvarchar(max)  NOT NULL, [Country] nvarchar(max)  NULL, [PhysicalDescription] nvarchar(max)  NULL, [VolumeNb] INT  NULL, [Abstract] varchar(max)  NULL, [Notes] varchar(max)  NULL, [Date] datetime2 NOT NULL default(getdate()),CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                const string sql2 = "If not exists (select * from sysobjects where name = 'Ebook') " +
                                    "CREATE TABLE [dbo].[Ebook] ( [Id] BIGINT NOT NULL, [EditionNum] nvarchar(max)  NULL, [EditionPlace] nvarchar(max)  NULL, [ISBN] nvarchar(max) NOT NULL, [Genre] nvarchar(max)  NULL, [Category] nvarchar(max)  NULL, [NbPages] INT  NULL, CONSTRAINT [PK_Ebook] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                const string sql3 = "If not exists (select * from sysobjects where name = 'Ejournal') " +
                                    "CREATE TABLE [dbo].[Ejournal] ( [Id] BIGINT NOT NULL, [ISSN] nvarchar(max) NOT NULL, [Frequency] nvarchar(max)  NULL, [TotalIssuesNb] INT  NULL, [DateFirstIssue] nvarchar(max)  NULL, [JournalScope] nvarchar(max)  NULL, [ImpactFactor] nvarchar(max)  NULL, CONSTRAINT [PK_Ejournal] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                const string sql4 = "If not exists (select * from sysobjects where name = 'DocumentSummary') " +
                                    "CREATE TABLE [dbo].[DocumentSummary] (  [IdDocument] BIGINT  NOT NULL, [Id] BIGINT IDENTITY(1, 1) NOT NULL, [Title] nvarchar(max) NOT NULL, [Start] INT  NOT NULL, [End] INT NOT NULL, CONSTRAINT [PK_DocumentSummary] PRIMARY KEY CLUSTERED ([Id] ASC) );";
                const string sql5 = "If not exists (select * from sysobjects where name = 'DocumentComments') " +
                                    "CREATE TABLE [dbo].[DocumentComments] (  [IdDocument] BIGINT  NOT NULL, [Id] BIGINT IDENTITY(1, 1) NOT NULL, [IdUser] BIGINT  NOT NULL, [Text] nvarchar(max)  NOT NULL, [Date] datetime2 NOT NULL default(getdate()), CONSTRAINT [PK_DocumentComments] PRIMARY KEY CLUSTERED ([Id] ASC) );";

                using (var command = new SqlCommand(sql, cnn))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(sql2, cnn))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(sql3, cnn))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(sql4, cnn))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(sql5, cnn))
                {
                    command.ExecuteNonQuery();
                }
                cnn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static IEnumerable<DocumentInfo> GetAllBooksGroupedBy(string type, string extra)
        {
            var lstDocuments = new List<Document>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    string sql;
                    if (IsNullOrEmpty(extra))
                        sql = "SELECT * FROM Document where DocumentType=@type";
                    else
                        sql = "SELECT * FROM Document where DocumentType=@type and IdEditor = @IdEditor ";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@type", type);
                        if (!IsNullOrEmpty(extra)) command.Parameters.AddWithValue("@IdEditor", extra);

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Document
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    OriginalTitle = dataReader["OriginalTitle"].ToString() ?? Empty,
                                    IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty),
                                    IdCollection = int.Parse(dataReader["IdCollection"].ToString() ?? Empty),
                                    IdAuthor = int.Parse(dataReader["IdAuthor"].ToString() ?? Empty),
                                    Doi = dataReader["Doi"].ToString() ?? Empty,
                                    MarcRecordNumber = dataReader["MarcRecordNumber"].ToString() ?? Empty,
                                    TitlesVariants = dataReader["TitlesVariants"].ToString() ?? Empty,
                                    Subtitle = dataReader["Subtitle"].ToString() ?? Empty,
                                    Keyword = dataReader["Keywords"].ToString()?.Split(";").ToList(),
                                    File = dataReader["File"].ToString() ?? Empty,
                                    FileFormat = dataReader["FileFormat"].ToString() ?? Empty,
                                    CoverPage = dataReader["CoverPage"].ToString() ?? Empty,
                                    Url = dataReader["Url"].ToString() ?? Empty,
                                    DocumentType = dataReader["DocumentType"].ToString() ?? Empty,
                                    OriginalLanguage = dataReader["OriginalLanguage"].ToString() ?? Empty,
                                    LanguageVariants = dataReader["LanguageVariants"].ToString() ?? Empty,
                                    Translator = dataReader["Translator"].ToString() ?? Empty,
                                    AccessType = dataReader["AccessType"].ToString() ?? Empty,
                                    State = dataReader["State"].ToString() ?? Empty,
                                    Price = float.Parse(dataReader["Price"].ToString() ?? Empty),
                                    PublicationDate = dataReader["PublicationDate"].ToString() ?? Empty,
                                    PhysicalDescription = dataReader["PhysicalDescription"].ToString() ?? Empty,
                                    VolumeNb = int.Parse(dataReader["VolumeNb"].ToString() ?? Empty),
                                    Abstract = dataReader["Abstract"].ToString() ?? Empty,
                                    Notes = dataReader["Notes"].ToString() ?? Empty
                                };
                                lstDocuments.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var lstBookInfo = new List<DocumentInfo>();

            foreach (var book in lstDocuments)
                switch (type)
                {
                    case "Ebook":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EbookGroup = GetEbookBy("Id", book.Id.ToString())
                        });
                        break;
                    case "Ejournal":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EjournalGroup = GetEjournalBy("Id", book.Id.ToString())
                        });
                        break;
                }

            return lstBookInfo;
        }

        public static IEnumerable<DocumentInfo> GetAllDocuments()
        {
            var lstDocuments = new List<Document>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM Document";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Document
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    OriginalTitle = dataReader["OriginalTitle"].ToString() ?? Empty,
                                    IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty),
                                    IdCollection = int.Parse(dataReader["IdCollection"].ToString() ?? Empty),
                                    IdAuthor = int.Parse(dataReader["IdAuthor"].ToString() ?? Empty),
                                    Doi = dataReader["Doi"].ToString() ?? Empty,
                                    MarcRecordNumber = dataReader["MarcRecordNumber"].ToString() ?? Empty,
                                    TitlesVariants = dataReader["TitlesVariants"].ToString() ?? Empty,
                                    Subtitle = dataReader["Subtitle"].ToString() ?? Empty,
                                    Keyword = dataReader["Keywords"].ToString()?.Split(";").ToList(),
                                    File = dataReader["File"].ToString() ?? Empty,
                                    FileFormat = dataReader["FileFormat"].ToString() ?? Empty,
                                    CoverPage = dataReader["CoverPage"].ToString() ?? Empty,
                                    Url = dataReader["Url"].ToString() ?? Empty,
                                    DocumentType = dataReader["DocumentType"].ToString() ?? Empty,
                                    OriginalLanguage = dataReader["OriginalLanguage"].ToString() ?? Empty,
                                    LanguageVariants = dataReader["LanguageVariants"].ToString() ?? Empty,
                                    Translator = dataReader["Translator"].ToString() ?? Empty,
                                    AccessType = dataReader["AccessType"].ToString() ?? Empty,
                                    State = dataReader["State"].ToString() ?? Empty,
                                    Price = float.Parse(dataReader["Price"].ToString() ?? Empty),
                                    PublicationDate = dataReader["PublicationDate"].ToString() ?? Empty,
                                    PhysicalDescription = dataReader["PhysicalDescription"].ToString() ?? Empty,
                                    VolumeNb = int.Parse(dataReader["VolumeNb"].ToString() ?? Empty),
                                    Abstract = dataReader["Abstract"].ToString() ?? Empty,
                                    Notes = dataReader["Notes"].ToString() ?? Empty
                                };
                                lstDocuments.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            var lstBookInfo = new List<DocumentInfo>();

            foreach (var book in lstDocuments)
                switch (book.DocumentType)
                {
                    case "Ebook":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EbookGroup = GetEbookBy("Id", book.Id.ToString())
                        });
                        break;
                    case "Ejournal":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EjournalGroup = GetEjournalBy("Id", book.Id.ToString())
                        });
                        break;
                }

            return lstBookInfo;
        }
        
        public static IEnumerable<DocumentInfo> SearchDocuments(string term, string type)
        {
            var lstDocuments = new List<Document>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    if (type == "ISBN")
                    {
                         var required = $"SELECT Id FROM Ebook WHERE [{type}] LIKE '{term}%' ;";
                         using (var command = new SqlCommand(required, connection))
                         {
                             command.CommandType = CommandType.Text;
                             using (var dataReader = command.ExecuteReader())
                             {
                                 while (dataReader.Read())
                                 {
                                     lstDocuments.Add(GetDocumentBy("Id", int.Parse(dataReader["Id"].ToString() ?? Empty).ToString()));
                                 }
                             }
                         }
                    }else if (type == "ISSN")
                    {
                        var required = $"SELECT Id FROM Ejournal WHERE [{type}] LIKE '{term}%' ;";
                        using (var command = new SqlCommand(required, connection))
                        {
                            command.CommandType = CommandType.Text;
                            using (var dataReader = command.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    lstDocuments.Add(GetDocumentBy("Id", int.Parse(dataReader["Id"].ToString() ?? Empty).ToString()));
                                }
                            }
                        }
                    }else if (type == "Author")
                    {
                        var required = $"SELECT * FROM Author WHERE [Name] LIKE '{term}%' ;";
                        using (var command = new SqlCommand(required, connection))
                        {
                            command.CommandType = CommandType.Text;
                            using (var dataReader = command.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    lstDocuments.Add(GetDocumentBy("IdAuthor", int.Parse(dataReader["Id"].ToString() ?? Empty).ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        var sql = $"SELECT * FROM Document WHERE [{type}] LIKE '{term}%' ORDER BY OriginalTitle;";
                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.CommandType = CommandType.Text;


                            using (var dataReader = command.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var u = new Document
                                    {
                                        Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                        OriginalTitle = dataReader["OriginalTitle"].ToString() ?? Empty,
                                        IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty),
                                        IdCollection = int.Parse(dataReader["IdCollection"].ToString() ?? Empty),
                                        IdAuthor = int.Parse(dataReader["IdAuthor"].ToString() ?? Empty),
                                        Doi = dataReader["Doi"].ToString() ?? Empty,
                                        MarcRecordNumber = dataReader["MarcRecordNumber"].ToString() ?? Empty,
                                        TitlesVariants = dataReader["TitlesVariants"].ToString() ?? Empty,
                                        Subtitle = dataReader["Subtitle"].ToString() ?? Empty,
                                        Keyword = dataReader["Keywords"].ToString()?.Split(";").ToList(),
                                        File = dataReader["File"].ToString() ?? Empty,
                                        FileFormat = dataReader["FileFormat"].ToString() ?? Empty,
                                        CoverPage = dataReader["CoverPage"].ToString() ?? Empty,
                                        Url = dataReader["Url"].ToString() ?? Empty,
                                        DocumentType = dataReader["DocumentType"].ToString() ?? Empty,
                                        OriginalLanguage = dataReader["OriginalLanguage"].ToString() ?? Empty,
                                        LanguageVariants = dataReader["LanguageVariants"].ToString() ?? Empty,
                                        Translator = dataReader["Translator"].ToString() ?? Empty,
                                        AccessType = dataReader["AccessType"].ToString() ?? Empty,
                                        State = dataReader["State"].ToString() ?? Empty,
                                        Price = float.Parse(dataReader["Price"].ToString() ?? Empty),
                                        PublicationDate = dataReader["PublicationDate"].ToString() ?? Empty,
                                        PhysicalDescription = dataReader["PhysicalDescription"].ToString() ?? Empty,
                                        VolumeNb = int.Parse(dataReader["VolumeNb"].ToString() ?? Empty),
                                        Abstract = dataReader["Abstract"].ToString() ?? Empty,
                                        Notes = dataReader["Notes"].ToString() ?? Empty
                                    };
                                    lstDocuments.Add(u);
                                }
                            }
                        }
                    }
                    
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var lstBookInfo = new List<DocumentInfo>();

            foreach (var book in lstDocuments)
                switch (book.DocumentType)
                {
                    case "Ebook":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EbookGroup = GetEbookBy("Id", book.Id.ToString())
                        });
                        break;
                    case "Ejournal":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EjournalGroup = GetEjournalBy("Id", book.Id.ToString())
                        });
                        break;
                }

            return lstBookInfo;
        }

        public static IEnumerable<DocumentInfo> GetAllDocumentGroupedBy(string field, string value)
        {
            var lstDocuments = new List<Document>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Document where [{field}]=@Field and AccessType != @Type";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        command.Parameters.AddWithValue("@Type", "Hidden");

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Document
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    OriginalTitle = dataReader["OriginalTitle"].ToString() ?? Empty,
                                    IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty),
                                    IdCollection = int.Parse(dataReader["IdCollection"].ToString() ?? Empty),
                                    Doi = dataReader["Doi"].ToString() ?? Empty,
                                    MarcRecordNumber = dataReader["MarcRecordNumber"].ToString() ?? Empty,
                                    TitlesVariants = dataReader["TitlesVariants"].ToString() ?? Empty,
                                    Subtitle = dataReader["Subtitle"].ToString() ?? Empty,
                                    Keyword = dataReader["Keywords"].ToString()?.Split(";").ToList(),
                                    File = dataReader["File"].ToString() ?? Empty,
                                    FileFormat = dataReader["FileFormat"].ToString() ?? Empty,
                                    CoverPage = dataReader["CoverPage"].ToString() ?? Empty,
                                    Url = dataReader["Url"].ToString() ?? Empty,
                                    DocumentType = dataReader["DocumentType"].ToString() ?? Empty,
                                    OriginalLanguage = dataReader["OriginalLanguage"].ToString() ?? Empty,
                                    LanguageVariants = dataReader["LanguageVariants"].ToString() ?? Empty,
                                    Translator = dataReader["Translator"].ToString() ?? Empty,
                                    AccessType = dataReader["AccessType"].ToString() ?? Empty,
                                    State = dataReader["State"].ToString() ?? Empty,
                                    Price = float.Parse(dataReader["Price"].ToString() ?? Empty),
                                    PublicationDate = dataReader["PublicationDate"].ToString() ?? Empty,
                                    PhysicalDescription = dataReader["PhysicalDescription"].ToString() ?? Empty,
                                    VolumeNb = int.Parse(dataReader["VolumeNb"].ToString() ?? Empty),
                                    Abstract = dataReader["Abstract"].ToString() ?? Empty,
                                    Notes = dataReader["Notes"].ToString() ?? Empty
                                };
                                lstDocuments.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var lstBookInfo = new List<DocumentInfo>();

            foreach (var book in lstDocuments)
                switch (book.DocumentType)
                {
                    case "Ebook":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EbookGroup = GetEbookBy("Id", book.Id.ToString())
                        });
                        break;
                    case "Ejournal":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EjournalGroup = GetEjournalBy("Id", book.Id.ToString())
                        });
                        break;
                }

            return lstBookInfo;
        }

        public static IEnumerable<DocumentInfo> GetLastDocuments()
        {
            var lstDocuments = new List<Document>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "SELECT TOP 10 * FROM Document  where AccessType != 'Hidden' ORDER BY Document.[Date]  DESC ";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new Document
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    OriginalTitle = dataReader["OriginalTitle"].ToString() ?? Empty,
                                    CoverPage = dataReader["CoverPage"].ToString() ?? Empty,
                                    Url = dataReader["Url"].ToString() ?? Empty,
                                    DocumentType = dataReader["DocumentType"].ToString() ?? Empty,
                                    OriginalLanguage = dataReader["OriginalLanguage"].ToString() ?? Empty,
                                    Price = float.Parse(dataReader["Price"].ToString() ?? Empty),
                                    PublicationDate = dataReader["PublicationDate"].ToString() ?? Empty,
                                    AccessType = dataReader["AccessType"].ToString() ?? Empty
                                };
                                lstDocuments.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            var lstBookInfo = new List<DocumentInfo>();

            foreach (var book in lstDocuments)
                switch (book.DocumentType)
                {
                    case "Ebook":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EbookGroup = GetEbookBy("Id", book.Id.ToString())
                        });
                        break;
                    case "Ejournal":
                        lstBookInfo.Add(new DocumentInfo
                        {
                            DocumentGroup = book,
                            EjournalGroup = GetEjournalBy("Id", book.Id.ToString())
                        });
                        break;
                }

            return lstBookInfo;
        }

        public static IEnumerable<DocumentSummary> GetAllDocumentSummary(string id)
        {
            var lstDocumentSummary = new List<DocumentSummary>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM DocumentSummary where IdDocument=@id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var u = new DocumentSummary
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    Title = dataReader["Title"].ToString() ?? Empty,
                                    Start = int.Parse(dataReader["Start"].ToString() ?? Empty),
                                    End = int.Parse(dataReader["End"].ToString() ?? Empty)
                                };
                                lstDocumentSummary.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }


            return lstDocumentSummary;
        }
        
        public static IEnumerable<DocumentComment> GetDocumentComment(string idDocument)
        {
            var lstComments = new List<DocumentComment>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql = "SELECT * FROM DocumentComments WHERE IdDocument=@IdDocument ORDER BY DocumentComments.[Date]  DESC";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@IdDocument", idDocument);

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var IdUser = int.Parse(dataReader["IdUser"].ToString() ?? Empty);
                                var UserInfo = BllAccount.GetUserBy("Id", IdUser.ToString());
                                string name = null;
                                switch (UserInfo.Role)
                                {
                                    case "Individual" :
                                        var Individual = BllAccount.GetIndvBy("Id", UserInfo.Id.ToString());
                                            name = Individual.Firstname + " "+ Individual.Lastname;
                                            break;
                                    case "Administrator":
                                        name = "Admin";
                                        break;
                                    case "Organization":
                                        var Organization = BllAccount.GetOrganizationBy("Id", UserInfo.Id.ToString());
                                        name = Organization.Name;
                                        break;
                                    case "Editor":
                                        var Editor = BllAccount.GetEditorBy("Id", UserInfo.Id.ToString());
                                        name = Editor.Name;
                                        break;
                                }
                                var u = new DocumentComment
                                {
                                    Id = int.Parse(dataReader["Id"].ToString() ?? Empty),
                                    IdDocument = int.Parse(dataReader["IdDocument"].ToString() ?? Empty),
                                    IdUser = IdUser,
                                    UserName = name,
                                    UserPhoto = UserInfo.Photo,
                                    Text = dataReader["Text"].ToString() ?? Empty,
                                    Date = DateTime.Parse(dataReader["Date"].ToString() ?? Empty),
                                };
                                lstComments.Add(u);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



            return lstComments;
        }

                
        private static EbookC GetEbookBy(string field, string value)
        {
            var ebook = new EbookC();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Ebook where [{field}]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ebook.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                ebook.EditionNum = dataReader["EditionNum"].ToString() ?? Empty;
                                ebook.EditionPlace = dataReader["EditionPlace"].ToString() ?? Empty;
                                ebook.Isbn = dataReader["Isbn"].ToString() ?? Empty;
                                ebook.Genre = dataReader["Genre"].ToString() ?? Empty;
                                ebook.Category = dataReader["Category"].ToString() ?? Empty;
                                ebook.NbPages = int.Parse(dataReader["NbPages"].ToString() ?? Empty);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return ebook;
        }

        private static Ejournal GetEjournalBy(string field, string value)
        {
            var ebook = new Ejournal();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Ejournal where [{field}]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ebook.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                ebook.Issn = dataReader["ISSN"].ToString() ?? Empty;
                                ebook.Frequency = dataReader["Frequency"].ToString() ?? Empty;
                                ebook.TotalIssuesNb = int.Parse(dataReader["TotalIssuesNb"].ToString() ?? Empty);
                                if (!IsNullOrEmpty(dataReader["DateFirstIssue"].ToString()))
                                    ebook.DateFirstIssue =
                                        dataReader["DateFirstIssue"].ToString() ?? Empty;
                                ebook.JournalScope = dataReader["JournalScope"].ToString() ?? Empty;
                                ebook.ImpactFactor = dataReader["ImpactFactor"].ToString() ?? Empty;
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return ebook;
        }

        public static Document GetDocumentBy(string field, string value)
        {
            var document = new Document();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"select * from Document where [{field}]=@Field";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                document.Id = int.Parse(dataReader["Id"].ToString() ?? Empty);
                                document.OriginalTitle = dataReader["OriginalTitle"].ToString() ?? Empty;
                                document.IdEditor = int.Parse(dataReader["IdEditor"].ToString() ?? Empty);
                                document.IdCollection = int.Parse(dataReader["IdCollection"].ToString() ?? Empty);
                                document.IdAuthor = int.Parse(dataReader["IdAuthor"].ToString() ?? Empty);
                                document.Doi = dataReader["Doi"].ToString() ?? Empty;
                                document.MarcRecordNumber = dataReader["MarcRecordNumber"].ToString() ?? Empty;
                                document.TitlesVariants = dataReader["TitlesVariants"].ToString() ?? Empty;
                                document.Subtitle = dataReader["Subtitle"].ToString() ?? Empty;
                                document.Keyword = dataReader["Keywords"].ToString()?.Split(";").ToList();
                                document.File = dataReader["File"].ToString() ?? Empty;
                                document.FileFormat = dataReader["FileFormat"].ToString() ?? Empty;
                                document.CoverPage = dataReader["CoverPage"].ToString() ?? Empty;
                                document.Url = dataReader["Url"].ToString() ?? Empty;
                                document.DocumentType = dataReader["DocumentType"].ToString() ?? Empty;
                                document.OriginalLanguage = dataReader["OriginalLanguage"].ToString() ?? Empty;
                                document.LanguageVariants = dataReader["LanguageVariants"].ToString() ?? Empty;
                                document.Translator = dataReader["Translator"].ToString() ?? Empty;
                                document.AccessType = dataReader["AccessType"].ToString() ?? Empty;
                                document.State = dataReader["State"].ToString() ?? Empty;
                                document.Price = float.Parse(dataReader["Price"].ToString() ?? Empty);
                                document.PublicationDate = dataReader["PublicationDate"].ToString() ?? Empty;
                                document.PhysicalDescription = dataReader["PhysicalDescription"].ToString() ?? Empty;
                                document.VolumeNb = int.Parse(dataReader["VolumeNb"].ToString() ?? Empty);
                                document.Abstract = dataReader["Abstract"].ToString() ?? Empty;
                                document.Notes = dataReader["Notes"].ToString() ?? Empty;
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch
            {
                // ignored
            }

            return document;
        }

        public static JsonResponse DeleteDocumentBy(string field, string value, string documentType)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"delete from Document where [{field}]=@Field";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    var sql2 = $"Delete from {documentType} where [{field}]=@Field";

                    using (var command = new SqlCommand(sql2, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    var sql3 = "Delete from DocumentTheme where [IdDocument]=@Field";

                    using (var command = new SqlCommand(sql3, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }
                    
                    var sql4 = "Delete from PurchaseHistory where [DocumentId]=@Field";

                    using (var command = new SqlCommand(sql4, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    var sql5 = "Delete from WishListUser where [DocumentId]=@Field";

                    using (var command = new SqlCommand(sql5, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }
                    
                    var sql6 = "Delete from DocumentComments where [IdDocument]=@Field";

                    using (var command = new SqlCommand(sql6, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse UpdateDocumentByField(string field, string value, string id)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };

            var tablename = field.Split(".")[0].Replace("Group", "");
            var fieldname = field.Split(".")[1];

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"UPDATE {tablename} SET [{fieldname}] = @value  WHERE Id = @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@id", int.Parse(id));
                        command.Parameters.AddWithValue("@value", Uri.UnescapeDataString(value));
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse UpdateDocument(DocumentInfo documentInfo)
        {
            var message = new JsonResponse
            {
                Success = false
            };
            try
            {
                var oldInfo = GetDocumentBy("id", documentInfo.DocumentGroup.Id.ToString());

                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    const string sql =
                        "UPDATE Document SET OriginalTitle=@OriginalTitle, IdEditor=@IdEditor, IdCollection=@IdCollection, IdAuthor=@IdAuthor, Doi=@Doi, MarcRecordNumber=@MarcRecordNumber, TitlesVariants=@TitlesVariants, Subtitle=@Subtitle, Keywords=@Keywords,  Url=@Url,  DocumentType=@DocumentType, OriginalLanguage=@OriginalLanguage, LanguageVariants=@LanguageVariants, Translator=@Translator, AccessType=@AccessType, State=@State, Price=@Price,  PublicationDate=@PublicationDate, Country=@Country, PhysicalDescription=@PhysicalDescription,  VolumeNb=@VolumeNb, Abstract=@Abstract, Notes=@Notes    WHERE Id = @Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.OriginalTitle))
                            command.Parameters.AddWithValue("@OriginalTitle", documentInfo.DocumentGroup.OriginalTitle);
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.IdEditor.ToString()))
                            command.Parameters.AddWithValue("@IdEditor", documentInfo.DocumentGroup.IdEditor);
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.IdCollection.ToString()))
                            command.Parameters.AddWithValue("@IdCollection", documentInfo.DocumentGroup.IdCollection);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.IdAuthor.ToString()))
                            command.Parameters.AddWithValue("@IdAuthor", documentInfo.DocumentGroup.IdAuthor);

                        if (documentInfo.DocumentGroup.Keyword.Any())
                            command.Parameters.AddWithValue("@Keywords",
                                Join(",", documentInfo.DocumentGroup.Keyword).Replace("\n", ";"));
                        command.Parameters.AddWithValue("@Url", $"/Home/Document?id={documentInfo.DocumentGroup.Id}");

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.DocumentType))
                            command.Parameters.AddWithValue("@DocumentType", documentInfo.DocumentGroup.DocumentType);
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.OriginalLanguage))
                            command.Parameters.AddWithValue("@OriginalLanguage",
                                documentInfo.DocumentGroup.OriginalLanguage);
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Price.ToString(CultureInfo.InvariantCulture)))
                            command.Parameters.AddWithValue("@Price", documentInfo.DocumentGroup.Price);
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.PublicationDate))
                            command.Parameters.AddWithValue("@PublicationDate",
                                documentInfo.DocumentGroup.PublicationDate);
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.AccessType))
                            command.Parameters.AddWithValue("@AccessType",
                                documentInfo.DocumentGroup.AccessType);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Doi))
                            command.Parameters.AddWithValue("@Doi",
                                documentInfo.DocumentGroup.Doi);
                        else
                            command.Parameters.AddWithValue("@Doi", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.MarcRecordNumber))
                            command.Parameters.AddWithValue("@MarcRecordNumber",
                                documentInfo.DocumentGroup.MarcRecordNumber);
                        else
                            command.Parameters.AddWithValue("@MarcRecordNumber", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.TitlesVariants))
                            command.Parameters.AddWithValue("@TitlesVariants",
                                documentInfo.DocumentGroup.TitlesVariants);
                        else
                            command.Parameters.AddWithValue("@TitlesVariants", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Subtitle))
                            command.Parameters.AddWithValue("@Subtitle",
                                documentInfo.DocumentGroup.Subtitle);
                        else
                            command.Parameters.AddWithValue("@Subtitle", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.LanguageVariants))
                            command.Parameters.AddWithValue("@LanguageVariants",
                                documentInfo.DocumentGroup.LanguageVariants);
                        else
                            command.Parameters.AddWithValue("@LanguageVariants", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Translator))
                            command.Parameters.AddWithValue("@Translator",
                                documentInfo.DocumentGroup.Translator);
                        else
                            command.Parameters.AddWithValue("@Translator", DBNull.Value);


                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.State))
                            command.Parameters.AddWithValue("@State",
                                documentInfo.DocumentGroup.State);
                        else
                            command.Parameters.AddWithValue("@State", DBNull.Value);


                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Country))
                            command.Parameters.AddWithValue("@Country",
                                documentInfo.DocumentGroup.Country);
                        else
                            command.Parameters.AddWithValue("@Country", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.PhysicalDescription))
                            command.Parameters.AddWithValue("@PhysicalDescription",
                                documentInfo.DocumentGroup.PhysicalDescription);
                        else
                            command.Parameters.AddWithValue("@PhysicalDescription", DBNull.Value);
                        
                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.VolumeNb.ToString()))
                            command.Parameters.AddWithValue("@VolumeNb",
                                documentInfo.DocumentGroup.VolumeNb);
                        else
                            command.Parameters.AddWithValue("@VolumeNb", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Abstract))
                            command.Parameters.AddWithValue("@Abstract",
                                documentInfo.DocumentGroup.Abstract);
                        else
                            command.Parameters.AddWithValue("@Abstract", DBNull.Value);

                        if (!IsNullOrEmpty(documentInfo.DocumentGroup.Notes))
                            command.Parameters.AddWithValue("@Notes",
                                documentInfo.DocumentGroup.Notes);
                        else
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);


                        command.ExecuteNonQuery();
                    }

                    if (oldInfo.DocumentType != documentInfo.DocumentGroup.DocumentType)
                    {
                        var del = $"delete from {oldInfo.DocumentType} where id =@Field";

                        using (var command = new SqlCommand(del, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@Field", documentInfo.DocumentGroup.Id);
                            command.ExecuteNonQuery();
                        }

                        switch (documentInfo.DocumentGroup.DocumentType)
                        {
                            case "Ebook":
                                const string x1 =
                                    "Insert into Ebook(Id, EditionNum,EditionPlace,ISBN, Genre, Category, NbPages) values(@Id, @EditionNum, @EditionPlace, @ISBN, @Genre, @Category, @NbPages)";

                                using (var command = new SqlCommand(x1, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.Isbn))
                                        command.Parameters.AddWithValue("@ISBN", documentInfo.EbookGroup.Isbn);


                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.EditionNum))
                                        command.Parameters.AddWithValue("@EditionNum",
                                            documentInfo.EbookGroup.EditionNum);
                                    else
                                        command.Parameters.AddWithValue("@EditionNum", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.EditionPlace))
                                        command.Parameters.AddWithValue("@EditionPlace",
                                            documentInfo.EbookGroup.EditionPlace);
                                    else
                                        command.Parameters.AddWithValue("@EditionPlace", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.Genre))
                                        command.Parameters.AddWithValue("@Genre",
                                            documentInfo.EbookGroup.Genre);
                                    else
                                        command.Parameters.AddWithValue("@Genre", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.Category))
                                        command.Parameters.AddWithValue("@Category",
                                            documentInfo.EbookGroup.Category);
                                    else
                                        command.Parameters.AddWithValue("@Category", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.NbPages.ToString()))
                                        command.Parameters.AddWithValue("@NbPages",
                                            documentInfo.EbookGroup.NbPages);
                                    else
                                        command.Parameters.AddWithValue("@NbPages", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Ejournal":
                                const string x2 =
                                    "Insert into Ejournal(Id, ISSN,Frequency,TotalIssuesNb, DateFirstIssue, JournalScope, ImpactFactor) values(@Id, @ISSN, @Frequency, @TotalIssuesNb, @DateFirstIssue, @JournalScope, @ImpactFactor)";
                                using (var command = new SqlCommand(x2, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.Issn))
                                        command.Parameters.AddWithValue("@ISSN", documentInfo.EjournalGroup.Issn);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.Frequency))
                                        command.Parameters.AddWithValue("@Frequency",
                                            documentInfo.EjournalGroup.Frequency);
                                    else
                                        command.Parameters.AddWithValue("@Frequency", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.TotalIssuesNb.ToString()))
                                        command.Parameters.AddWithValue("@TotalIssuesNb",
                                            documentInfo.EjournalGroup.TotalIssuesNb);
                                    else
                                        command.Parameters.AddWithValue("@TotalIssuesNb", DBNull.Value);


                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.DateFirstIssue))
                                        command.Parameters.AddWithValue("@DateFirstIssue", documentInfo.EjournalGroup.DateFirstIssue);
                                    else
                                        command.Parameters.AddWithValue("@DateFirstIssue", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.JournalScope))
                                        command.Parameters.AddWithValue("@JournalScope",
                                            documentInfo.EjournalGroup.JournalScope);
                                    else
                                        command.Parameters.AddWithValue("@JournalScope", DBNull.Value);


                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.ImpactFactor))
                                        command.Parameters.AddWithValue("@ImpactFactor",
                                            documentInfo.EjournalGroup.ImpactFactor);
                                    else
                                        command.Parameters.AddWithValue("@ImpactFactor", DBNull.Value);


                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                        }
                    }
                    else
                    {
                        switch (documentInfo.DocumentGroup.DocumentType)
                        {
                            case "Ebook":
                                const string sql2 =
                                    "UPDATE Ebook SET EditionNum = @EditionNum, EditionPlace = @EditionPlace, ISBN= @ISBN, Genre = @Genre, Category= @Category, NbPages = @NbPages WHERE Id = @Id";
                                using (var command = new SqlCommand(sql2, connection))
                                {
                                    command.CommandType = CommandType.Text;


                                    command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.Isbn))
                                        command.Parameters.AddWithValue("@ISBN", documentInfo.EbookGroup.Isbn);


                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.EditionNum))
                                        command.Parameters.AddWithValue("@EditionNum",
                                            documentInfo.EbookGroup.EditionNum);
                                    else
                                        command.Parameters.AddWithValue("@EditionNum", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.EditionPlace))
                                        command.Parameters.AddWithValue("@EditionPlace",
                                            documentInfo.EbookGroup.EditionPlace);
                                    else
                                        command.Parameters.AddWithValue("@EditionPlace", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.Genre))
                                        command.Parameters.AddWithValue("@Genre",
                                            documentInfo.EbookGroup.Genre);
                                    else
                                        command.Parameters.AddWithValue("@Genre", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.Category))
                                        command.Parameters.AddWithValue("@Category",
                                            documentInfo.EbookGroup.Category);
                                    else
                                        command.Parameters.AddWithValue("@Category", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EbookGroup.NbPages.ToString()))
                                        command.Parameters.AddWithValue("@NbPages",
                                            documentInfo.EbookGroup.NbPages);
                                    else
                                        command.Parameters.AddWithValue("@NbPages", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                            case "Ejournal":
                                const string sql3 =
                                    "UPDATE Ejournal SET ISSN = @ISSN, Frequency = @Frequency, TotalIssuesNb= @TotalIssuesNb, DateFirstIssue = @DateFirstIssue, JournalScope= @JournalScope, ImpactFactor = @ImpactFactor WHERE Id = @Id";
                                using (var command = new SqlCommand(sql3, connection))
                                {
                                    command.CommandType = CommandType.Text;

                                    command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.Issn))
                                        command.Parameters.AddWithValue("@ISSN", documentInfo.EjournalGroup.Issn);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.Frequency))
                                        command.Parameters.AddWithValue("@Frequency",
                                            documentInfo.EjournalGroup.Frequency);
                                    else
                                        command.Parameters.AddWithValue("@Frequency", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.TotalIssuesNb.ToString()))
                                        command.Parameters.AddWithValue("@TotalIssuesNb",
                                            documentInfo.EjournalGroup.TotalIssuesNb);
                                    else
                                        command.Parameters.AddWithValue("@TotalIssuesNb", DBNull.Value);


                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.DateFirstIssue))
                                        command.Parameters.AddWithValue("@DateFirstIssue",
                                            documentInfo.EjournalGroup.DateFirstIssue);
                                    else
                                        command.Parameters.AddWithValue("@DateFirstIssue", DBNull.Value);

                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.JournalScope))
                                        command.Parameters.AddWithValue("@JournalScope",
                                            documentInfo.EjournalGroup.JournalScope);
                                    else
                                        command.Parameters.AddWithValue("@JournalScope", DBNull.Value);


                                    if (!IsNullOrEmpty(documentInfo.EjournalGroup.ImpactFactor))
                                        command.Parameters.AddWithValue("@ImpactFactor",
                                            documentInfo.EjournalGroup.ImpactFactor);
                                    else
                                        command.Parameters.AddWithValue("@ImpactFactor", DBNull.Value);

                                    if (command.ExecuteNonQuery() == 1)
                                    {
                                        message.Success = true;
                                        message.Message = "Resultat Ok";
                                    }
                                }

                                break;
                        }
                    }

                    if (oldInfo.IdCollection != documentInfo.DocumentGroup.IdCollection)
                    {
                        var newTheme =
                            DalCollection.GetThemeByCollection(documentInfo.DocumentGroup.IdCollection.ToString());
                        const string sql4 =
                            "Update DocumentTheme SET IdTheme=@IdTheme WHERE IdDocument=@IdDocument";
                        using (var command = new SqlCommand(sql4, connection))
                        {
                            command.CommandType = CommandType.Text;
                            command.Parameters.AddWithValue("@IdTheme", newTheme.Id);
                            command.Parameters.AddWithValue("@IdDocument", documentInfo.DocumentGroup.Id);
                            if (command.ExecuteNonQuery() == 1)
                            {
                                message.Success = true;
                                message.Message = "Resultat Ok";
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse AddDocument(Document newDoc)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };


            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    const string sql =
                        "Insert into Document(OriginalTitle,IdEditor, IdCollection, IdAuthor, Doi, MarcRecordNumber, TitlesVariants, Subtitle, Keywords, Url, DocumentType, OriginalLanguage, LanguageVariants, Translator, AccessType, State, Price,  PublicationDate, Country, PhysicalDescription,  VolumeNb, Abstract, Notes) values (@OriginalTitle, @IdEditor, @IdCollection, @IdAuthor, @Doi, @MarcRecordNumber, @TitlesVariants, @Subtitle, @Keywords, @Url, @DocumentType, @OriginalLanguage, @LanguageVariants, @Translator, @AccessType, @State, @Price, @PublicationDate, @Country, @PhysicalDescription, @VolumeNb, @Abstract, @Notes);SELECT SCOPE_IDENTITY();";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        if (!IsNullOrEmpty(newDoc.OriginalTitle))
                            command.Parameters.AddWithValue("@OriginalTitle", newDoc.OriginalTitle);
                        if (!IsNullOrEmpty(newDoc.IdEditor.ToString()))
                            command.Parameters.AddWithValue("@IdEditor", newDoc.IdEditor);
                        if (!IsNullOrEmpty(newDoc.IdCollection.ToString()))
                            command.Parameters.AddWithValue("@IdCollection", newDoc.IdCollection);
                        if (!IsNullOrEmpty(newDoc.IdAuthor.ToString()))
                            command.Parameters.AddWithValue("@IdAuthor", newDoc.IdAuthor);
                        if (newDoc.Keyword.Any())
                            command.Parameters.AddWithValue("@Keywords",
                                Join(",", newDoc.Keyword).Replace("\n", ";"));


                        command.Parameters.AddWithValue("@Url", $"/Home/Document?id={newDoc.Id}");
                        if (!IsNullOrEmpty(newDoc.DocumentType))
                            command.Parameters.AddWithValue("@DocumentType", newDoc.DocumentType);
                        if (!IsNullOrEmpty(newDoc.OriginalLanguage))
                            command.Parameters.AddWithValue("@OriginalLanguage", newDoc.OriginalLanguage);
                        if (!IsNullOrEmpty(newDoc.Price.ToString(CultureInfo.InvariantCulture)))
                            command.Parameters.AddWithValue("@Price", newDoc.Price);
                        if (!IsNullOrEmpty(newDoc.PublicationDate))
                            command.Parameters.AddWithValue("@PublicationDate", newDoc.PublicationDate);

                        if (!IsNullOrEmpty(newDoc.Doi))
                            command.Parameters.AddWithValue("@Doi",
                                newDoc.Doi);
                        else
                            command.Parameters.AddWithValue("@Doi", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.MarcRecordNumber))
                            command.Parameters.AddWithValue("@MarcRecordNumber",
                                newDoc.MarcRecordNumber);
                        else
                            command.Parameters.AddWithValue("@MarcRecordNumber", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.TitlesVariants))
                            command.Parameters.AddWithValue("@TitlesVariants",
                                newDoc.TitlesVariants);
                        else
                            command.Parameters.AddWithValue("@TitlesVariants", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.Subtitle))
                            command.Parameters.AddWithValue("@Subtitle",
                                newDoc.Subtitle);
                        else
                            command.Parameters.AddWithValue("@Subtitle", DBNull.Value);



                        if (!IsNullOrEmpty(newDoc.LanguageVariants))
                            command.Parameters.AddWithValue("@LanguageVariants",
                                newDoc.LanguageVariants);
                        else
                            command.Parameters.AddWithValue("@LanguageVariants", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.Translator))
                            command.Parameters.AddWithValue("@Translator",
                                newDoc.Translator);
                        else
                            command.Parameters.AddWithValue("@Translator", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.AccessType))
                            command.Parameters.AddWithValue("@AccessType",
                                newDoc.AccessType);
                        else
                            command.Parameters.AddWithValue("@AccessType", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.State))
                            command.Parameters.AddWithValue("@State",
                                newDoc.State);
                        else
                            command.Parameters.AddWithValue("@State", DBNull.Value);


                        if (!IsNullOrEmpty(newDoc.Country))
                            command.Parameters.AddWithValue("@Country",
                                newDoc.Country);
                        else
                            command.Parameters.AddWithValue("@Country", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.PhysicalDescription))
                            command.Parameters.AddWithValue("@PhysicalDescription",
                                newDoc.PhysicalDescription);
                        else
                            command.Parameters.AddWithValue("@PhysicalDescription", DBNull.Value);
                        
                        if (!IsNullOrEmpty(newDoc.VolumeNb.ToString()))
                            command.Parameters.AddWithValue("@VolumeNb",
                                newDoc.VolumeNb);
                        else
                            command.Parameters.AddWithValue("@VolumeNb", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.Abstract))
                            command.Parameters.AddWithValue("@Abstract",
                                newDoc.Abstract);
                        else
                            command.Parameters.AddWithValue("@Abstract", DBNull.Value);

                        if (!IsNullOrEmpty(newDoc.Notes))
                            command.Parameters.AddWithValue("@Notes",
                                newDoc.Notes);
                        else
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);


                        var id = Convert.ToInt32(command.ExecuteScalar());

                        if (id > 0)
                        {
                            message.Extra = id.ToString();
                            UpdateDocumentByField("DocumentGroup.Url", "/Home/Document?id=" + message.Extra,
                                message.Extra);
                            message.Success = true;
                        }


                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }

        public static JsonResponse NewDocument(DocumentInfo documentInfo)
        {
            var message = new JsonResponse
            {
                Success = false
            };

            var operation = AddDocument(documentInfo.DocumentGroup);
            if (!operation.Success) return operation;
            try
            {
                documentInfo.DocumentGroup.Id = int.Parse(operation.Extra);
                message.Extra = operation.Extra;
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    switch (documentInfo.DocumentGroup.DocumentType)
                    {
                        case "Ebook":
                            const string x1 =
                                "Insert into Ebook(Id, EditionNum,EditionPlace,ISBN, Genre, Category, NbPages) values(@Id, @EditionNum, @EditionPlace, @ISBN, @Genre, @Category, @NbPages)";

                            using (var command = new SqlCommand(x1, connection))
                            {
                                command.CommandType = CommandType.Text;

                                command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                                if (!IsNullOrEmpty(documentInfo.EbookGroup.Isbn))
                                    command.Parameters.AddWithValue("@ISBN", documentInfo.EbookGroup.Isbn);


                                if (!IsNullOrEmpty(documentInfo.EbookGroup.EditionNum))
                                    command.Parameters.AddWithValue("@EditionNum",
                                        documentInfo.EbookGroup.EditionNum);
                                else
                                    command.Parameters.AddWithValue("@EditionNum", DBNull.Value);

                                if (!IsNullOrEmpty(documentInfo.EbookGroup.EditionPlace))
                                    command.Parameters.AddWithValue("@EditionPlace",
                                        documentInfo.EbookGroup.EditionPlace);
                                else
                                    command.Parameters.AddWithValue("@EditionPlace", DBNull.Value);

                                if (!IsNullOrEmpty(documentInfo.EbookGroup.Genre))
                                    command.Parameters.AddWithValue("@Genre",
                                        documentInfo.EbookGroup.Genre);
                                else
                                    command.Parameters.AddWithValue("@Genre", DBNull.Value);

                                if (!IsNullOrEmpty(documentInfo.EbookGroup.Category))
                                    command.Parameters.AddWithValue("@Category",
                                        documentInfo.EbookGroup.Category);
                                else
                                    command.Parameters.AddWithValue("@Category", DBNull.Value);

                                if (!IsNullOrEmpty(documentInfo.EbookGroup.NbPages.ToString()))
                                    command.Parameters.AddWithValue("@NbPages",
                                        documentInfo.EbookGroup.NbPages);
                                else
                                    command.Parameters.AddWithValue("@NbPages", DBNull.Value);

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }

                            break;
                        case "Ejournal":
                            const string x2 =
                                "Insert into Ejournal(Id, ISSN,Frequency,TotalIssuesNb, DateFirstIssue, JournalScope, ImpactFactor) values(@Id, @ISSN, @Frequency, @TotalIssuesNb, @DateFirstIssue, @JournalScope, @ImpactFactor)";
                            using (var command = new SqlCommand(x2, connection))
                            {
                                command.CommandType = CommandType.Text;

                                command.Parameters.AddWithValue("@Id", documentInfo.DocumentGroup.Id);

                                if (!IsNullOrEmpty(documentInfo.EjournalGroup.Issn))
                                    command.Parameters.AddWithValue("@ISSN", documentInfo.EjournalGroup.Issn);

                                if (!IsNullOrEmpty(documentInfo.EjournalGroup.Frequency))
                                    command.Parameters.AddWithValue("@Frequency", documentInfo.EjournalGroup.Frequency);
                                else
                                    command.Parameters.AddWithValue("@Frequency", DBNull.Value);

                                if (!IsNullOrEmpty(documentInfo.EjournalGroup.TotalIssuesNb.ToString()))
                                    command.Parameters.AddWithValue("@TotalIssuesNb",
                                        documentInfo.EjournalGroup.TotalIssuesNb);
                                else
                                    command.Parameters.AddWithValue("@TotalIssuesNb", DBNull.Value);


                                if (!IsNullOrEmpty(documentInfo.EjournalGroup.DateFirstIssue))
                                    command.Parameters.AddWithValue("@DateFirstIssue",
                                        documentInfo.EjournalGroup.DateFirstIssue);
                                else
                                    command.Parameters.AddWithValue("@DateFirstIssue", DBNull.Value);

                                if (!IsNullOrEmpty(documentInfo.EjournalGroup.JournalScope))
                                    command.Parameters.AddWithValue("@JournalScope",
                                        documentInfo.EjournalGroup.JournalScope);
                                else
                                    command.Parameters.AddWithValue("@JournalScope", DBNull.Value);


                                if (!IsNullOrEmpty(documentInfo.EjournalGroup.ImpactFactor))
                                    command.Parameters.AddWithValue("@ImpactFactor",
                                        documentInfo.EjournalGroup.ImpactFactor);
                                else
                                    command.Parameters.AddWithValue("@ImpactFactor", DBNull.Value);


                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }

                            break;
                    }

                    const string documentTheme =
                        "Insert into DocumentTheme(IdDocument,IdTheme) values(@IdDocument, @IdTheme)";

                    using (var command = new SqlCommand(documentTheme, connection))
                    {
                        command.CommandType = CommandType.Text;

                        command.Parameters.AddWithValue("@IdDocument", documentInfo.DocumentGroup.Id);
                        command.Parameters.AddWithValue("@IdTheme",
                            DalCollection.GetThemeByCollection(documentInfo.DocumentGroup.IdCollection.ToString()).Id);


                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Success = false;
                message.Message = "Erreur : " + e.Message;
                Console.WriteLine(e.Message);
            }

            if (!message.Success)
                DeleteDocumentBy("Id", documentInfo.DocumentGroup.Id.ToString(),
                    documentInfo.DocumentGroup.DocumentType);

            return message;
        }

        public static DocumentInfo GetDocumentById(string field, string value)
        {
            var document = new DocumentInfo();
            document.DocumentGroup = GetDocumentBy(field, value);

            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    switch (document.DocumentGroup.DocumentType)
                    {
                        case "Ebook":
                            document.EbookGroup = GetEbookBy(field, value);
                            break;
                        case "Ejournal":
                            document.EjournalGroup = GetEjournalBy(field, value);
                            break;
                    }

                    connection.Close();
                }
            }
            catch
            {
                document = null;
            }

            return document;
        }
        
        public static JsonResponse UpdateDocumentSummary(IEnumerable<DocumentSummary> summaries)
        {
            var message = new JsonResponse
            {
                Success = false
            };


            foreach (var documentSummary in summaries)
                try
                {
                    using (var connection = DbConnection.GetConnection())
                    {
                        connection.Open();

                        if (documentSummary.Id == 0)
                        {
                            const string sql =
                                "Insert into DocumentSummary(IdDocument, Title, [Start], [End] ) values (@IdDocument, @Title, @Start, @End)";
                            using (var command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;
                                command.Parameters.AddWithValue("@IdDocument", documentSummary.IdDocument);
                                command.Parameters.AddWithValue("@Title", documentSummary.Title);
                                command.Parameters.AddWithValue("@Start", documentSummary.Start);
                                command.Parameters.AddWithValue("@End", documentSummary.End);

                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }
                        }
                        else
                        {
                            const string sql =
                                "UPDATE  DocumentSummary SET IdDocument = @IdDocument, Title=@Title, [Start]=@Start, [End]=@End where Id=@id";

                            using (var command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;
                                command.Parameters.AddWithValue("@id", documentSummary.Id);
                                command.Parameters.AddWithValue("@IdDocument", documentSummary.IdDocument);
                                command.Parameters.AddWithValue("@Title", documentSummary.Title);
                                command.Parameters.AddWithValue("@Start", documentSummary.Start);
                                command.Parameters.AddWithValue("@End", documentSummary.End);
                                if (command.ExecuteNonQuery() == 1)
                                {
                                    message.Success = true;
                                    message.Message = "Resultat Ok";
                                }
                            }
                        }


                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    message.Message = e.Message;
                }


            return message;
        }

        public static JsonResponse DeleteDocumentSummaryBy(string field, string value)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"delete from DocumentSummary where [{field}]=@Field";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }


                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
            }

            return message;
        }
        
        public static JsonResponse DeleteDocumentCommentBy(string field, string value)
        {
            var message = new JsonResponse
            {
                Success = false,
                Message = "Erreur"
            };
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = $"delete from DocumentComments where [{field}]=@Field";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Field", value);
                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }


                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = "Erreur : " + e.Message;
                
            }

            return message;
        }

        public static JsonResponse NewDocumentComment(DocumentComment documentComment)
        {
            var message = new JsonResponse
            {
                Success = false
            };


            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    const string sql =
                        "Insert into DocumentComments(IdDocument, IdUser, Text ) values (@IdDocument, @IdUser, @Text);SELECT SCOPE_IDENTITY();";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@IdDocument", documentComment.IdDocument);
                        command.Parameters.AddWithValue("@IdUser", documentComment.IdUser);
                        command.Parameters.AddWithValue("@Text", documentComment.Text);

                        var id = Convert.ToInt32(command.ExecuteScalar());

                        if (id > 0)
                        {
                            message.Extra = id.ToString();
                            message.Success = true;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = e.Message;
            }



            return message;
        }
        
        public static JsonResponse EditDocumentComment(DocumentComment documentComment)
        {
            var message = new JsonResponse
            {
                Success = false
            };


            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();

                    const string sql =
                        "UPDATE  DocumentComments SET Text=@Text where Id=@Id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Id", documentComment.Id);
                        command.Parameters.AddWithValue("@Text", documentComment.Text);

                        if (command.ExecuteNonQuery() == 1)
                        {
                            message.Success = true;
                            message.Message = "Resultat Ok";
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                message.Message = e.Message;
            }



            return message;
        }

        public static IEnumerable<DocumentInfo> GetDocumentByTheme(string id, string type)
        {
            var lstDocuments = new List<DocumentInfo>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    string sql;

                        sql = "SELECT * FROM DocumentTheme where IdTheme=@IdTheme";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@IdTheme", id);

                        using (var dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var Id = int.Parse(dataReader["IdDocument"].ToString() ?? Empty);
                                var result = DalDocument.GetDocumentById("Id", Id.ToString());
                                if (result.DocumentGroup.DocumentType == type)
                                {
                                    lstDocuments.Add(result);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            return lstDocuments;
        }

        public static IEnumerable<int> GetDateRange()
        {
            var range = new List<int>();
            try
            {
                using (var connection = DbConnection.GetConnection())
                {
                    connection.Open();
                    var sql = "SELECT MIN(PublicationDate) AS 'Min' , MAX(PublicationDate) AS 'Max' FROM Document";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        using (var dataReader = command.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                
                                range.Add(int.Parse(dataReader["Min"].ToString() ?? Empty));
                                range.Add(int.Parse(dataReader["Max"].ToString() ?? Empty));
                                
                            }

                        }
                    }
                    connection.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return range;
        }
    }
    
    
}