using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi.Filters
{
    /// <summary>
    /// Filter to validate models passed to a request
    /// </summary>
    public class ModelVaidationFilter : IActionFilter
    {
        public bool AllowMultiple => false;

        /// <summary>
        /// Validate model passed to a request
        /// </summary>
        /// <param name="actionContext">Action context</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="continuation">Continuation</param>
        /// <returns>BadRequest if model is null or invalid, continuation overwise</returns>
        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var nulls = actionContext.ActionArguments.Where(x => x.Value == null).Select(x => x.Key).ToList();
            nulls.ForEach(x => actionContext.ModelState.AddModelError(x, $"{x} cannot be null."));

            return actionContext.ModelState.IsValid
                ? continuation()
                : Task.FromResult(actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState));
        }
    }
}