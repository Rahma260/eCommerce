using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Cart;
using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Services.Interfaces.Cart;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;
using eCommerce.Domain.Interfaces.Cart;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Services.Implementations.Cart
{
    public class CartService : ICartService
    {
        private readonly ICart _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMapper _mapper;

        public CartService(
            ICart cartRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService,
            IPaymentMethodService paymentMethodService,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _paymentMethodService = paymentMethodService;
            _mapper = mapper;
        }

        // ============================
        // Add product to cart
        // ============================
        public async Task<ServiceResponse> AddToCartAsync(AddToCartDto dto)
        {
            var cart = await _cartRepository.GetCartAsync(dto.UserId) ?? new CartDto { UserId = dto.UserId };

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItemDto { ProductId = dto.ProductId, Quantity = dto.Quantity });
            }

            await _cartRepository.SaveCartAsync(cart, TimeSpan.FromDays(1));
            return new ServiceResponse { success = true, message = "Added to cart" };
        }

        // ============================
        // View cart
        // ============================
        public async Task<CartDto?> GetCartAsync(string userId)
        {
            return await _cartRepository.GetCartAsync(userId);
        }

        // ============================
        // Update cart
        // ============================
        public async Task<ServiceResponse> UpdateCartAsync(AddToCartDto dto)
        {
            var cart = await _cartRepository.GetCartAsync(dto.UserId);
            if (cart == null) return new ServiceResponse { success = false, message = "Cart not found" };

            var item = cart.Items.FirstOrDefault(x => x.ProductId == dto.ProductId);
            if (item == null) return new ServiceResponse { success = false, message = "Product not in cart" };

            if (dto.Quantity <= 0)
                cart.Items.Remove(item);
            else
                item.Quantity = dto.Quantity;

            await _cartRepository.SaveCartAsync(cart, TimeSpan.FromDays(1));
            return new ServiceResponse { success = true, message = "Cart updated" };
        }

        // ============================
        // Remove product from cart
        // ============================
        public async Task<ServiceResponse> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart == null) return new ServiceResponse { success = false, message = "Cart not found" };

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return new ServiceResponse { success = false, message = "Product not in cart" };

            cart.Items.Remove(item);
            await _cartRepository.SaveCartAsync(cart, TimeSpan.FromDays(1));

            return new ServiceResponse { success = true, message = "Product removed from cart" };
        }

        // ============================
        // Checkout: Payment + Order creation
        // ============================
        public async Task<ServiceResponse> Checkout(string userId, int paymentMethodId)
        {
            // Get cart from Redis
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart == null || !cart.Items.Any())
                return new ServiceResponse { success = false, message = "Cart is empty" };

            // Validate payment method
            var paymentMethods = await _paymentMethodService.GetPaymentMethods();
            var method = paymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
            if (method == null)
                return new ServiceResponse { success = false, message = "Invalid payment method" };

            // Get products from DB
            var products = await _unitOfWork.Products.GetAllAsync();
            var cartProducts = cart.Items
                .Join(products,
                    c => c.ProductId,
                    p => p.Id,
                    (c, p) => new { CartItem = c, Product = p })
                .ToList();

            if (!cartProducts.Any())
                return new ServiceResponse { success = false, message = "No products found" };

            var totalAmount = cartProducts.Sum(x => x.CartItem.Quantity * x.Product.Price);

            // Payment (Stripe or other)
            var paymentResult = await _paymentService.Pay(totalAmount,
                cartProducts.Select(x => x.Product),
                cart.Items);
            if (!paymentResult.success) return paymentResult;

            // Map to Order entity
            var order = new Domain.Entities.Order
            {
                UserId = userId,
                PaymentMethodId = paymentMethodId,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = cartProducts.Select(x => new OrderItem
                {
                    ProductId = x.Product.Id,
                    Quantity = x.CartItem.Quantity,
                    UnitPrice = x.Product.Price
                }).ToList()
            };
            order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();

            // Clear cart
            await _cartRepository.ClearCartAsync(userId);

            return new ServiceResponse
            {
                success = true,
                message = "Order placed successfully",
            };
        }

        // ============================
        // Get all orders for a user
        // ============================
        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(o => o.UserId == userId, includeProperties: "OrderItems,PaymentMethod,User");
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        // ============================
        // Get specific order by id
        // ============================
        public async Task<OrderDto?> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(
                orderId,
                includeProperties: "OrderItems,PaymentMethod,User"
            );
            if (order == null) return null;

            return _mapper.Map<OrderDto>(order);
        }
        public async Task<ServiceResponse> SaveCheckoutAsync(SaveCheckoutDto dto)
        {
            if (!dto.Items.Any())
                return new ServiceResponse { success = false, message = "No items to checkout" };

            var products = await _unitOfWork.Products.GetAllAsync();

            var orderItems = dto.Items
                .Join(products,
                    c => c.ProductId,
                    p => p.Id,
                    (c, p) => new OrderItem
                    {
                        ProductId = p.Id,
                        Quantity = c.Quantity,
                        UnitPrice = p.Price
                    }).ToList();

            var order = new Domain.Entities.Order
            {
                UserId = dto.UserId,
                PaymentMethodId = dto.PaymentMethodId,
                PaymentStatus = PaymentStatus.Pending,
                OrderItems = orderItems,
                TotalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice)
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveAsync();

            return new ServiceResponse
            {
                success = true,
                message = "Checkout saved",
            };
        }
        public async Task CompleteOrderAfterPaymentAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(
                orderId,
                includeProperties: "OrderItems"
            );

            if (order == null || order.UserId != userId)
                return;

            order.PaymentStatus = PaymentStatus.Completed;
            await _unitOfWork.Orders.UpdateAsync(order);

            // clear cart from Redis
            await _cartRepository.ClearCartAsync(userId);

            await _unitOfWork.SaveAsync();
        }

    }


}
