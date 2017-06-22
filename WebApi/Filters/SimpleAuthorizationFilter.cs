using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi.Filters
{
    /// <summary>
    /// Filter to perform simple authorization with predifined users
    /// </summary>
    public class SimpleAuthorizationFilter : IAuthorizationFilter
    {
        private const string Scheme = "Basic";

        private const string Role = "Admin";

        private const char Separator = ':';

        private readonly Dictionary<string, string> users = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Alex"] = "password1",
            ["Bob"] = "password2",
            ["Charlie"] = "password3"
        };

        public bool AllowMultiple => false;

        /// <summary>
        /// Authorize user using basic authorization
        /// </summary>
        /// <param name="actionContext">Action context</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="continuation">Continuation</param>
        /// <returns>Unauthorized if user information is incorrect, continuation overwise</returns>
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var header = actionContext.Request.Headers.Authorization;

            if (header == null || !string.Equals(header.Scheme, Scheme, StringComparison.Ordinal) || string.IsNullOrEmpty(header.Parameter))
            {
                return CreateErrorResponse(actionContext, "Invalid authorization header");
            }

            var credentials = GetCredentials(header.Parameter);

            if (credentials == null)
            {
                return CreateErrorResponse(actionContext, "Malformed authorization header parameter.");
            }

            if (!this.users.TryGetValue(credentials.Item1, out string password))
            {
                return CreateErrorResponse(actionContext, "User does not exists.");
            }

            if (!string.Equals(credentials.Item2, password, StringComparison.Ordinal))
            {
                return CreateErrorResponse(actionContext, "Invalid password.");
            }

            actionContext.RequestContext.Principal = new GenericPrincipal(new GenericIdentity(credentials.Item1), new[] { Role });
            return continuation();
        }

        private static Tuple<string, string> GetCredentials(string parameter)
        {
            byte[] bytes;

            try
            {
                bytes = Convert.FromBase64String(parameter);
            }
            catch (FormatException)
            {
                return null;
            }

            var credentials = Encoding.UTF8.GetString(bytes);
            var position = credentials.IndexOf(Separator);
            return position < 0 ? null : Tuple.Create(credentials.Substring(0, position), credentials.Substring(position + 1));
        }

        private Task<HttpResponseMessage> CreateErrorResponse(HttpActionContext actionContext, string message)
        {
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic"));
            return Task.FromResult(response);
        }
    }
}