using Microsoft.EntityFrameworkCore;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.Features.Orders.Queries.Responses;
using ProductProject.Application.ServiceInterfaces;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;
using ProductProjrect.Data.Helper;
using System.Linq.Expressions;

namespace ProductProject.Application.Services
{
    public class OrderService(IUnitOfWork _unitOfWork) : IOrderService
    {
        public IQueryable<Order> GetAll()
           => _unitOfWork.Orders.GetAllAsNoTracking();

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _unitOfWork.Orders.GetAllAsync();

        public async Task<Order> GetByIdAsync(int orderId)
            => await _unitOfWork.Orders.GetByIdAsync(orderId);

        public async Task<bool> IsExistAsync(int orderId)
            => await _unitOfWork.Orders.IsExistAsync(x => x.OrderId == orderId);

        public async Task<bool> IsExistAsync(params Expression<Func<Order, bool>>[] conditions)
            => await _unitOfWork.Orders.IsExistAsync(conditions);

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            await _unitOfWork.BeginTransactionAsync();

            var order = await GetByIdAsync(orderId);

            // Restore stock for each product in the order
            foreach (var orderDetail in order.OrderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(orderDetail.ProductId);

                product.StockQuantity += orderDetail.Quantity;
                _unitOfWork.Products.Update(product);
            }

            order.Status = enOrderStatus.Cancelled;
            _unitOfWork.Orders.Update(order);

            return await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<GetOrderInfoResponse> GetOrderInfo(int orderId)
        {
            var order = await GetAll()
                .Select(x => new GetOrderInfoResponse
                {
                    OrderId = x.OrderId,
                    OrderDate = x.OrderDate,
                    Status = x.Status.ToString(),
                    TotalAmount = x.TotalAmount,
                    OrderDetailsInfos = x.OrderDetails
                .Select(y => new OrderDetailsInfoResponse
                {
                    ProductName = y.Product.ProductName,
                    Quantity = y.Quantity,
                    UnitPrice = y.UnitPrice
                }).ToList()
                }).FirstAsync(x => x.OrderId == orderId);

            return order;
        }

        public async Task<IEnumerable<GetOrderInfoResponse>> GetAllOrdersInfo()
        {
            var orders = await GetAll().Select(x => new GetOrderInfoResponse
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                Status = x.Status.ToString(),
                TotalAmount = x.TotalAmount,
                OrderDetailsInfos = _unitOfWork.OrderDetails.GetAllAsNoTracking()
                .Where(y => y.OrderId == x.OrderId)
                .Select(y => new OrderDetailsInfoResponse
                {
                    ProductName = y.Product.ProductName,
                    Quantity = y.Quantity,
                    UnitPrice = y.UnitPrice
                }).ToList()
            }).ToListAsync();

            return orders;
        }


        #region crud
        public async Task<bool> AddAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddAsync(HashSet<AddOrderDetail> orderDetails)
        {
            await _unitOfWork.BeginTransactionAsync();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                Status = enOrderStatus.Pending,
                TotalAmount = 0, // Will be calculated
            };

            foreach (var orderDetail in orderDetails)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(orderDetail.ProductId);

                // Calculate total price
                decimal unitPrice = product.Price;
                order.TotalAmount += unitPrice * orderDetail.Quantity;

                // Deduct stock
                product.StockQuantity -= orderDetail.Quantity;

                await _unitOfWork.Orders.AddAsync(order);

                // Add order orderDetail
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = orderDetail.ProductId,
                    Quantity = orderDetail.Quantity,
                    UnitPrice = unitPrice,
                    Order = order
                });
            }

            // Save changes , This will insert Order & OrderDetails
            return await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Order> orders)
        {
            await _unitOfWork.Orders.AddRangeAsync(orders);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Order order)
        {
            _unitOfWork.Orders.Update(order);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<Order> orders)
        {
            _unitOfWork.Orders.UpdateRange(orders);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
            var entity = await GetByIdAsync(orderId);
            _unitOfWork.Orders.Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(HashSet<int> orderIds)
        {
            var orders = await GetAll()
                .Where(c => orderIds.Contains(c.OrderId))
                .ToListAsync();

            _unitOfWork.Orders.DeleteRange(orders);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
