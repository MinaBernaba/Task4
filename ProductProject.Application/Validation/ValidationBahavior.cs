using FluentValidation;
using MediatR;
using System.Net;

namespace ProductProject.Application.Validation
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators)
         : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var errors = failures
                        .Select(x => $"Error in {x.PropertyName}: {x.ErrorMessage}")
                        .ToList();

                    var statusCodes = failures
                        .Select(f => int.TryParse(f.ErrorCode, out int code) ? code : (int)HttpStatusCode.BadRequest)
                        .ToHashSet();

                    var finalStatusCode = statusCodes.Contains(403) ? HttpStatusCode.Forbidden :
                                          statusCodes.Contains(404) ? HttpStatusCode.NotFound :
                                          statusCodes.Contains(409) ? HttpStatusCode.Conflict :
                                          statusCodes.Contains(422) ? HttpStatusCode.UnprocessableEntity :
                                          HttpStatusCode.BadRequest;

                    throw new CustomValidationException(finalStatusCode, errors);
                }
            }
            return await next();
        }
    }
}
