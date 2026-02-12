using eCommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.DTOs
{
    // Cart DTOs (Redis)
    // Add to Cart
    // ============================
    // Add product to cart (single item)
    // ============================
    public class AddToCartDto
    {
        public string UserId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    // ============================
    // Cart DTO (full cart for a user)
    // ============================
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartDto
    {
        public string UserId { get; set; } = null!;
        public List<CartItemDto> Items { get; set; } = new();
    }

    // ============================
    // Checkout DTO
    // ============================
    public class CheckoutDto
    {
        public string UserId { get; set; } = null!;
        public int PaymentMethodId { get; set; }
    }

    // ============================
    // Order DTOs for viewing orders
    // ============================
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public string UserFullName { get; set; } = null!;
        public List<OrderItemDto> Items { get; set; } = new();
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethodName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    }
    public class SaveCheckoutDto
    {
        public string UserId { get; set; } = null!;
        public int PaymentMethodId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
    }

}
