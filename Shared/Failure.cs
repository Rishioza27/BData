using Microsoft.AspNetCore.Mvc;

namespace SmartServices.Shared
{
	public static class Failure
	{
		public static object? Handle(Result result) =>
			result switch
			{
				{ IsSuccess: true } => throw new InvalidOperationException(),
				IValidationResult validationResult => Results.BadRequest(
					CreateProblemDetails(
						"Validation Error",
						StatusCodes.Status400BadRequest,
						result.Error,
						validationResult.Errors
					)
				),
				_ => Results.BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error)),
			};

		private static ProblemDetails CreateProblemDetails(string title, int status, Error error, Error[]? errors = null) =>
			new()
			{
				Title = title,
				Type = error.Code,
				Detail = error.Message,
				Status = status,
				Extensions = { { nameof(errors), errors } },
			};
	}
}
