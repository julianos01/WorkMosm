using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Helpers
{
    public static class ResultExtensions
    {
        public static List<string> ExtractMessages(this ApiValidationError? apiError)
        {
            if (apiError?.Errors != null && apiError.Errors.Any())
            {
                return apiError.Errors.SelectMany(e => e.Value).ToList()!;
            }

            return new List<string> { apiError?.Title ?? "An unexpected error occurred" };
        }
    }
}
