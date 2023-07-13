using System.Collections.Generic;

namespace Bookzone.Models.Entity.Product
{
    public class DocumentPurchaseVm : Item
    {
        private List<int> _listDoc;
        public int UserId { get; set; }

        public List<int> ListDoc
        {
            get => _listDoc;
            set => _listDoc = value;
        }

        public string Nonce { get; set; }
        public float Total { get; set; }

    }
}
