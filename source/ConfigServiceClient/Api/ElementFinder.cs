using System;
using System.Linq;
using System.Linq.Expressions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Api
{
    internal class ElementFinder<TElement> where TElement : class
    {
        private readonly Func<OptionGroup, Func<string, TElement>> _findMethod;

        public ElementFinder(Expression<Func<OptionGroup, Func<string, TElement>>> findMethod)
        {
            if (findMethod == null) throw new ArgumentNullException(nameof(findMethod));
            _findMethod = findMethod.Compile();
        }

        public TElement Find(OptionGroup target, params string[] pathSegments)
        {
            while (true)
            {
                if (!pathSegments.Any())
                {
                    return null;
                }

                var firstSegment = pathSegments.First();
                if (pathSegments.Length == 1)
                {
                    return _findMethod(target)(firstSegment);
                }

                var group = target.FindNested(firstSegment);
                if (group == null)
                {
                    return null;
                }

                target = group;
                pathSegments = pathSegments.Skip(1).ToArray();
            }
        }
    }
}