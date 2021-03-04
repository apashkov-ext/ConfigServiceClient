using System;
using System.Linq;
using System.Linq.Expressions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Api
{
    public class OptionGroupChildElementFinder<TChild> where TChild : class
    {
        private readonly Func<IOptionGroup, Func<string, TChild>> _findMethod;

        public OptionGroupChildElementFinder(Expression<Func<IOptionGroup, Func<string, TChild>>> findMethod)
        {
            if (findMethod == null) throw new ArgumentNullException(nameof(findMethod));
            _findMethod = findMethod.Compile();
        }

        public TChild Find(IOptionGroup parent, params string[] pathSegments)
        {
            if (pathSegments.Any(x => x.Contains('.')))
            {
                throw new ApplicationException("Invalid json path segment");
            }

            while (true)
            {
                if (!pathSegments.Any())
                {
                    return null;
                }

                var firstSegment = pathSegments.First();
                if (pathSegments.Length == 1)
                {
                    return _findMethod(parent)(firstSegment);
                }

                var group = parent.FindNested(firstSegment);
                if (group == null)
                {
                    return null;
                }

                parent = group;
                pathSegments = pathSegments.Skip(1).ToArray();
            }
        }
    }
}