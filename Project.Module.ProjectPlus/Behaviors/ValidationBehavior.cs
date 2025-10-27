using FluentValidation;
using MediatR;
using Project.ResponseHandler.ResponseBuilder;
using Project.ResponseHandler.ResponseModel;

namespace Project.Module.ProjectPlus.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ApiResponse
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IResponseBuilder _responseBuilder;

        public ValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            IResponseBuilder responseBuilder)
        {
            _validators = validators;
            _responseBuilder = responseBuilder;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorResponse = await _responseBuilder.BuildErrorResponse(
                    message: "Validation failed",
                    errors: failures.Select(f => f.ErrorMessage).ToList(),
                    statusCode: 400
                );

                return errorResponse as TResponse;
            }

            return await next();
        }
    }
} 