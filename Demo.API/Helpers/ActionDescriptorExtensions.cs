using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Linq;

namespace Demo.API.Helpers
{
    public static class ActionDescriptorExtensions
    {
        public static ApiVersionModel GetApiVersion(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor?.Properties
              .Where(kvp => (Type)kvp.Key == typeof(ApiVersionModel))
              .Select(kvp => kvp.Value as ApiVersionModel).FirstOrDefault();
        }
    }
}