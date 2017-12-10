using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit
{
    internal sealed class DisplayFactDiscoverer : FactDiscoverer
    {
        public DisplayFactDiscoverer(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink) { }

        protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            return new DisplayXunitTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod);
        }
    }
}
