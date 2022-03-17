using System;

namespace BurglarAlarm.Web.Server.Component
{
    public class IgnoreMiddlewareAttribute : Attribute
    {
        public IgnoreMiddleware Event { get; private set; }

        public IgnoreMiddlewareAttribute(IgnoreMiddleware ignoreMiddleware)
        {
            Event = ignoreMiddleware;
        }
    }

    public enum IgnoreMiddleware
    {
        Accept,
        Never
    }
}
