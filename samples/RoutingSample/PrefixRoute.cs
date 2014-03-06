﻿﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Routing;

namespace RoutingSample
{
    internal class PrefixRoute : IRouter
    {
        private readonly IRouter _next;
        private readonly string _prefix;

        public PrefixRoute(IRouter next, string prefix)
        {
            _next = next;

            if (prefix == null)
            {
                prefix = "/";
            }
            else if (prefix.Length > 0 && prefix[0] != '/')
            {
                // owin.RequestPath starts with a /
                prefix = "/" + prefix;
            }

            if (prefix.Length > 1 && prefix[prefix.Length - 1] == '/')
            {
                prefix = prefix.Substring(0, prefix.Length - 1);
            }

            _prefix = prefix;
        }

        public async Task RouteAsync(RouteContext context)
        {
            if (context.RequestPath.StartsWith(_prefix, StringComparison.OrdinalIgnoreCase))
            {
                if (context.RequestPath.Length > _prefix.Length)
                {
                    var lastCharacter = context.RequestPath[_prefix.Length];
                    if (lastCharacter != '/' && lastCharacter != '#' && lastCharacter != '?')
                    {
                        return;
                    }
                }

                await _next.RouteAsync(context);
            }
        }

        public void BindPath(BindPathContext context)
        {
            // Do nothing
        }
    }
}
