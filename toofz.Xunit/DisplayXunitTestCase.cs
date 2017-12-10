using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Humanizer;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit
{
    [Serializable]
    internal sealed class DisplayXunitTestCase : XunitTestCase
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer", true)]
        public DisplayXunitTestCase() { }

        public DisplayXunitTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            ITestMethod testMethod,
            object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, testMethodArguments) { }

        protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
        {
            var factAttributeDisplayName = factAttribute.GetNamedArgument<string>(nameof(FactAttribute.DisplayName));
            if (factAttributeDisplayName != null)
            {
                return factAttributeDisplayName;
            }

            var ignoredWords = factAttribute.GetConstructorArguments().OfType<string[]>().Single();
            var methodName = displayName.Split('.').Last();

            var formattedDisplayName = string.Join(
                ", ",
                methodName
                    .Split('_')
                    .Select(n => string.Join(" ", Humanize(n, ignoredWords))));

            if (TestMethodArguments?.Length > 0)
            {
                var parameters = Method.GetParameters();

                var args = parameters.Zip(TestMethodArguments, (p, a) =>
                {
                    if (a.GetType() == typeof(string))
                    {
                        return $"{p.Name}: \"{a}\"";
                    }
                    return $"{p.Name}: {a}";
                }).ToList();

                formattedDisplayName += $" ({string.Join(", ", args)})";
            }

            return formattedDisplayName;
        }

        private IEnumerable<string> Humanize(string segment, string[] ignoredWords)
        {
            if (!ignoredWords.Any())
            {
                yield return segment.Humanize();
                yield break;
            }

            var i = 0;
            while (i < segment.Length)
            {
                // Get earlier occurrence of an ignored word at this point in the segment.
                // Multiple ignored words can begin at the same point.
                var iwGroup = ignoredWords
                    .GroupBy(w => segment.IndexOf(w, i))
                    .OrderBy(g => g.Key)
                    .FirstOrDefault(g => g.Key > -1);
                if (iwGroup != null)
                {
                    var ignoreIndex = iwGroup.Key;
                    var segmentLength = ignoreIndex - i;
                    if (segmentLength > 0)
                    {
                        yield return Humanize(segment.Substring(i, segmentLength));
                        i += segmentLength;
                    }

                    // Get the longest ignored word that begins at this point.
                    var ignoredWordLength = iwGroup.Max(w => w.Length);
                    yield return segment.Substring(ignoreIndex, ignoredWordLength);
                    i += ignoredWordLength;
                }
                else
                {
                    yield return Humanize(segment.Substring(i));
                    yield break;
                }

                string Humanize(string s)
                {
                    return i == 0 ?
                        s.Humanize() :
                        s.Humanize(LetterCasing.LowerCase);
                }
            }
        }
    }
}
