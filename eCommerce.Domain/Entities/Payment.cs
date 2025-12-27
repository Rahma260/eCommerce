namespace eCommerce.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }

        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
    }

}
