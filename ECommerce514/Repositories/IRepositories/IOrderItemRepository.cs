﻿namespace ECommerce514.Repositories.IRepositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<bool> CreateRangeAsync(List<OrderItem> entities);
    }
}
