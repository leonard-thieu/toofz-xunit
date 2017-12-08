using Xunit.Sdk;

namespace Xunit
{
    [XunitTestCaseDiscoverer("Xunit.DisplayTheoryDiscoverer", "toofz.Xunit")]
    public sealed class DisplayTheoryAttribute : TheoryAttribute
    {
        public DisplayTheoryAttribute(params string[] ignoredWords) { }
    }
}
