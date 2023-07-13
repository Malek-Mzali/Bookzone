#nullable enable
using System;
using System.Collections.Generic;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.Statistics;
using Ebook.Models.Entity.Document;

namespace Bookzone.Models.BLL
{
    public class BllStatistics
    {
        public static int GetTotalUsers()
        {
            return DALStatistics.GetTotalUsers();
        }
        
        public static int GetTodayUsers()
        {
            return DALStatistics.GetTodayUsers();
        }
        
        public static int GetTotalDocuments(string? identityName)
        {
            return DALStatistics.GetTotalDocuments(identityName);
        }
        
        public static int GetTodaySales(string? identityName)
        {
            return DALStatistics.GetTodaySales(identityName);
        }
        public static int GetTotalSales(string? identityName)
        {
            return DALStatistics.GetTotalSales(identityName);
        }

        public static List<Tuple<int, DocumentInfo>> TopSoldDocuments(string? identityName)
        {
            return DALStatistics.TopSoldDocuments(identityName);
        }

        public static Dictionary<string,int> GetVisitorCounter()
        {
            return DALStatistics.GetVisitorCounter();
        }
        
        public static List<int> GetSalesPerMonth(string documentType, string? identityName)
        {
            return DALStatistics.GetSalesPerMonth(documentType, identityName);
        }
        
        public static int GetTotalUserType(string type)
        {
            return DALStatistics.GetTotalUserType(type);
        }
        
        public static Statistics StatisticsApi()
        {
            
            var statstics = new Statistics()
            {
                TotalUsers = GetTotalUsers(),
                TodayUsers = GetTodayUsers(),
                TotalDocuments = GetTotalDocuments(""),
                TotalSales = GetTotalSales(""),
                TodaySales = GetTodaySales(""),
                TopDocuments = TopSoldDocuments(""),
                SalesByMonthEbook = GetSalesPerMonth("Ebook", ""),
                SalesByMonthEjournal = GetSalesPerMonth("Ejournal", ""),
                TotalEditors = GetTotalUserType("Editor"),
                TotalOrganizations = GetTotalUserType("Organization"),
                TotalIndividuals = GetTotalUserType("Individual"),
                VisitorCounter = GetVisitorCounter(),


            };
            return statstics;
        }

        public static object StatisticsEditorApi(string? identityName)
        {
            
            var statstics = new Statistics()
            {
                TotalDocuments = GetTotalDocuments(identityName),
                TotalSales = GetTotalSales(identityName),
                TodaySales = GetTodaySales(identityName),
                TopDocuments = TopSoldDocuments(identityName),
                SalesByMonthEbook = GetSalesPerMonth("Ebook", identityName),
                SalesByMonthEjournal = GetSalesPerMonth("Ejournal", identityName),
            };
            return statstics;        }
    }
}