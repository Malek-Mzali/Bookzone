using System;
using System.Collections.Generic;
using Ebook.Models.Entity.Document;

namespace Bookzone.Models.Entity.Statistics
{
    public class Statistics
    {
        public Statistics()
        {
            
        }


        public Statistics(int totalUsers, int todayUsers, int totalDocuments, int totalSales, int todaySales, int totalEditors, int totalOrganizations, int totalIndividual, List<Tuple<int, DocumentInfo>> topDocuments, List<int> salesByMonthEbook, List<int> salesByMonthEjournal, Dictionary<string, int>visitorCounter)
        {
            TotalUsers = totalUsers;
            TodayUsers = todayUsers;
            TotalDocuments = totalDocuments;
            TotalSales = totalSales;
            TodaySales = todaySales;
            TotalEditors = totalEditors;
            TotalOrganizations = totalOrganizations;
            TotalIndividuals = totalIndividual;
            TopDocuments = topDocuments;
            SalesByMonthEbook = salesByMonthEbook;
            SalesByMonthEjournal = salesByMonthEjournal;
            VisitorCounter = visitorCounter;
        }

        public int TotalUsers { get; set; }
        public int TodayUsers { get; set; }
        public int TotalDocuments { get; set; }
        public int TotalSales { get; set; }
        public int TodaySales { get; set; }
        public int TotalEditors { get; set; }
        public int TotalOrganizations { get; set; }
        public int TotalIndividuals { get; set; }

        public List<Tuple<int, DocumentInfo>> TopDocuments { get; set; }
        
        public List<int> SalesByMonthEbook { get; set; }

        public List<int>  SalesByMonthEjournal { get; set; }
        public Dictionary<string, int> VisitorCounter { get; set; }
    }
}