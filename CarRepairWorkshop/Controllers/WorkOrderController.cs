﻿using CarRepairWorkshop.API.Services;
using CarRepairWorkshop.API.Services.Interfaces;
using CarRepairWorkshop.Shared;
using CarRepairWorkshop.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CarRepairWorkshop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;
        private readonly ICustomerService _customerService;
        private readonly IWorkEstimationService _workEstimationService;
        public WorkOrderController(IWorkOrderService workOrderService, ICustomerService customerService, IWorkEstimationService workEstimationService)
        {
            _workOrderService = workOrderService;
            _customerService = customerService;
            _workEstimationService = workEstimationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetAllWorkOrders()
        {
            try
            {
                var workOrders = await _workOrderService.GetAllWorkOrdersAsync();
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("ByCustomer/{customerId}")]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetWorkOrdersByCustomerId(Guid customerId)
        {
            try
            {
                var workOrders = await _workOrderService.GetWorkOrdersByCustomerIdAsync(customerId);
                return Ok(workOrders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOrder>> GetWorkOrderById(Guid id)
        {
            try
            {
                var workOrder = await _workOrderService.GetWorkOrderByIdAsync(id);

                if (workOrder == null)
                    return NotFound();

                return Ok(workOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddWorkOrder([FromBody] WorkOrder workOrder)
        {
            try
            {
                // Check if the customer exists before adding the work order
                var customer = await _customerService.GetCustomerByIdAsync(workOrder.CustomerId);
                if (customer == null)
                {
                    return NotFound($"Customer with ID {workOrder.CustomerId} not found.");
                }

                await _workOrderService.AddWorkOrderAsync(workOrder);
                return CreatedAtAction(nameof(GetWorkOrderById), new { id = workOrder.Id }, workOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkOrder(Guid id)
        {
            try
            {
                var existingWorkOrder = await _workOrderService.GetWorkOrderByIdAsync(id);

                if (existingWorkOrder == null)
                    return NotFound();

                await _workOrderService.DeleteWorkOrderAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Promote")]
        public async Task<ActionResult> UpdateWorkOrderStatus([FromBody] Guid id)
        {
            try
            {
                var existingWorkOrder = await _workOrderService.GetWorkOrderByIdAsync(id);

                if (existingWorkOrder == null)
                    return NotFound();

                await _workOrderService.UpdateWorkOrderStatusAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("calculate/{workOrderId}")]
        public async Task<ActionResult<double>> CalculateEstimatedTime(Guid workOrderId)
        {
            try
            {
                WorkOrder workOrder = await _workOrderService.GetWorkOrderByIdAsync(workOrderId);
                if (workOrder == null)
                {
                    return NotFound();
                }
                double estimatedTime = _workEstimationService.CalculateWorkHourEstimation(workOrder);
                return Ok(estimatedTime);
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Bad Request: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
