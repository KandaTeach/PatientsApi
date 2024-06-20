using ErrorOr;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors
{
    /// <summary>
    /// Pipeline behavior for validating requests using FluentValidation.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        /// <summary>
        /// Injecting the validator to be used in this validator class.
        /// </summary>
        /// <param name="validator">The validator for the request type.</param>
        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        /// <summary>
        /// Handles the request and performs validation.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="next">The next delegate in the pipeline.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_validator is null)
            {
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors
                .Select(failure => Error.Validation(
                    failure.PropertyName,
                    failure.ErrorMessage))
                .ToList();

            return (dynamic)errors;
        }
    }
}
