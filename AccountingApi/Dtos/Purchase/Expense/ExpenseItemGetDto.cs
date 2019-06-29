namespace AccountingApi.Dtos.Purchase.Expense
{
    public class ExpenseItemGetDto
    {
        public int Id { get; set; }
        public bool IsBank { get; set; }
        public double? PaidMoney { get; set; }
        public double? Residue { get; set; }
        public int ExpenseInvoiceId { get; set; }
    }
}
