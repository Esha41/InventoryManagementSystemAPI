using MediatR;
using Project.Module.ProjectPlus.Models;
using Project.Module.ProjectPlus.Queries;
using Project.ResponseHandler.ResponseBuilder;
using Project.ResponseHandler.ResponseModel;

namespace Project.Module.ProjectPlus.Handlers
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ApiResponse>
    {
        private readonly IResponseBuilder _responseBuilder;

        public GetProjectQueryHandler(IResponseBuilder responseBuilder)
        {
            _responseBuilder = responseBuilder;
        }

        public async Task<ApiResponse> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Add your repository and actual data retrieval logic here
                // This is just a mock response
                var projects = new List<Project>
                {
                    new Project
                    {
                        Id = 1,
                        Name = "Sample Project",
                        Description = "This is a sample project",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddMonths(6),
                        Status = "Active",
                        Budget = 100000,
                        IsActive = true
                    }
                };

                // Apply filters (in a real application, this would be done at the database level)
                var filteredProjects = projects
                    .Where(p => !request.Id.HasValue || p.Id == request.Id)
                    .Where(p => string.IsNullOrEmpty(request.Name) || p.Name.Contains(request.Name))
                    .Where(p => string.IsNullOrEmpty(request.Status) || p.Status == request.Status)
                    .Where(p => !request.IsActive.HasValue || p.IsActive == request.IsActive)
                    .Where(p => !request.StartDateFrom.HasValue || p.StartDate >= request.StartDateFrom)
                    .Where(p => !request.StartDateTo.HasValue || p.StartDate <= request.StartDateTo)
                    .ToList();

                return await _responseBuilder.BuildSuccessResponse(
                    data: filteredProjects,
                    message: "Projects retrieved successfully",
                    statusCode: 200
                );
            }
            catch (Exception ex)
            {
                return await _responseBuilder.BuildErrorResponse(
                    message: $"Error retrieving projects: {ex.Message}",
                    statusCode: 500
                );
            }
        }
    }
} 