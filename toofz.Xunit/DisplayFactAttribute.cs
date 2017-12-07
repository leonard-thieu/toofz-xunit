using Xunit;
using Xunit.Sdk;

namespace Xunit
{
    [XunitTestCaseDiscoverer("Xunit.DisplayFactDiscoverer", "toofz.Xunit")]
    public sealed class DisplayFactAttribute : FactAttribute
    {
        public DisplayFactAttribute(params string[] ignoredWords) { }
    }
}
