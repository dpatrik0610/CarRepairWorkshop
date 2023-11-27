using CarRepairWorkshop.API.Services.Interfaces;
using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRepairWorkshop.API.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly ILogger<WorkOrderService> _logger;
        private readonly WorkshopDbContext _dbContext;

        public WorkOrderService(ILogger<WorkOrderService> logger, WorkshopDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WorkOrder>> GetAllWorkOrdersAsync()
        {
            try
            {
                var workOrders = await _dbContext.WorkOrders.ToListAsync();
                return workOrders;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllWorkOrdersAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<WorkOrder>> GetWorkOrdersByCustomerIdAsync(Guid customerId)
        {
            try
            {
                var workOrders = await _dbContext.WorkOrders
                    .Where(x => x.CustomerId == customerId)
                    .ToListAsync();

                return workOrders;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWorkOrdersByCustomerId: {ex.Message}");
                throw;
            }
        }

        public async Task<WorkOrder> GetWorkOrderByIdAsync(Guid workOrderId)
        {
            try
            {
                var workOrder = await _dbContext.WorkOrders.FirstOrDefaultAsync(x => x.Id == workOrderId);
                return workOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetWorkOrderByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task AddWorkOrderAsync(WorkOrder workOrder)
        {
            try
            {
                _dbContext.WorkOrders.Add(workOrder);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddWorkOrderAsync: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteWorkOrderAsync(Guid workOrderId)
        {
            try
            {
                var workOrderToDelete = await _dbContext.WorkOrders.FindAsync(workOrderId);
                if (workOrderToDelete != null)
                {
                    _dbContext.WorkOrders.Remove(workOrderToDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteWorkOrderAsync: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateWorkOrderStatusAsync(Guid workOrderId)
        {
            try
            {
                var workOrder = await _dbContext.WorkOrders.FindAsync(workOrderId);
                if (workOrder.Status != JobStatus.Completed)
                {
                    workOrder.Status++;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateWorkOrderStatusAsync: {ex.Message}");
                throw;
            }
        }
    }
}
