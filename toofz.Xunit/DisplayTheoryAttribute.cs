using Xunit.Sdk;

namespace Xunit
{
    [XunitTestCaseDiscoverer("Xunit.DisplayFactDiscoverer", "toofz.Xunit")]
    public sealed class DisplayTheoryAttribute : TheoryAttribute
    {
        public DisplayTheoryAttribute(params string[] ignoredWords) { }
    }
}
