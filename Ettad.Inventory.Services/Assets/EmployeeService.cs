using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ettad.Inventory.Service.Assets
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICrossCuttingRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateEmployeeDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            ICrossCuttingRepository<Employee> employeeRepository,
            IMapper mapper,
            IValidator<CreateUpdateEmployeeDto> validator,
            ICurrentUserService currentUserService,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<EmployeeDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting employee by ID. EmployeeId: {EmployeeId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var employee = await _employeeRepository.FindOneAsync(
                    e => e.Id == id && !e.IsDeleted,
                    false,
                    nameof(Employee.Department)
                );

                if (employee == null)
                    return APIOperationResponse<EmployeeDto>.Fail(ResponseType.NotFound, "Employee not found");

                var dto = _mapper.Map<EmployeeDto>(employee);
                
                _logger.LogInformation("Employee retrieved successfully. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                
                return APIOperationResponse<EmployeeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee by ID. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<EmployeeDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<EmployeeDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all employees. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var employees = await _employeeRepository.FindAsync(
                    e => !e.IsDeleted,
                    false,
                    nameof(Employee.Department)
                );

                var dtos = _mapper.Map<List<EmployeeDto>>(employees);
                
                _logger.LogInformation("All employees retrieved successfully. Count: {Count}, User: {UserId}", 
                    dtos.Count, _currentUserService.UserId);
                
                return APIOperationResponse<List<EmployeeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all employees. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<EmployeeDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateEmployeeDto inputDto)
        {
            _logger.LogInformation("Creating new employee. NameAr: {NameAr}, NameEn: {NameEn}, User: {UserId}", 
                inputDto?.NameAr, inputDto?.NameEn, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Employee validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var employee = _mapper.Map<Employee>(inputDto);
                employee.CreationDate = DateTime.Now;
                employee.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdEmployee = await _employeeRepository.AddAsync(employee);
                _logger.LogInformation("Employee created successfully. EmployeeId: {EmployeeId}, User: {UserId}",
                                createdEmployee.Id, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(createdEmployee.Id, "Employee created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee. User: {UserId}", _currentUserService.UserId);
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateEmployeeDto inputDto)
        {
            _logger.LogInformation("Updating employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if employee exists
                var existingEmployee = await _employeeRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (existingEmployee == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Employee not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingEmployee);
                existingEmployee.ModificationDate = DateTime.Now;
                existingEmployee.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _employeeRepository.UpdateAsync(existingEmployee);
                
                _logger.LogInformation("Employee updated successfully. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Employee updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var employee = await _employeeRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (employee == null)
                {
                    _logger.LogWarning("Employee not found for deletion. EmployeeId: {EmployeeId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Employee not found");
                }

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _employeeRepository.DeleteAsync(employee);

                _logger.LogInformation("Employee deleted successfully. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Employee deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee. EmployeeId: {EmployeeId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

