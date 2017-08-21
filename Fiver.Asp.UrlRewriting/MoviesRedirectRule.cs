using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;

namespace Fiver.Asp.UrlRewriting
{
    public class MoviesRedirectRule : IRule
    {
        private readonly string[] matchPaths;
        private readonly PathString newPath;

        public MoviesRedirectRule(string[] matchPaths, string newPath)
        {
            if (matchPaths.Count() == 0)
                throw new ArgumentException("matchPaths empty");

            if (matchPaths.Where(s => !s.StartsWith("/")).Count() > 0)
                throw new ArgumentException("matchWith values must start with /");

            if (string.IsNullOrEmpty(newPath))
                throw new ArgumentException("newPath missing");

            if (!newPath.StartsWith("/"))
                throw new ArgumentException("newPath must start with /");

            this.matchPaths = matchPaths;
            this.newPath = new PathString(newPath);
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            // if already redirected, skip
            if (request.Path.StartsWithSegments(new PathString(this.newPath)))
            {
                return;
            }

            if (this.matchPaths.Contains(request.Path.Value))
            {
                var newLocation = $"{this.newPath}{request.QueryString}";

                var response = context.HttpContext.Response;
                response.StatusCode = StatusCodes.Status302Found;
                context.Result = RuleResult.EndResponse;
                response.Headers[HeaderNames.Location] = newLocation;
            }
        }
    }
}
