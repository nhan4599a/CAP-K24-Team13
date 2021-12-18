using Xunit.Abstractions;
using Xunit.Sdk;

namespace UnitTestSupport
{
    internal class PriorityTestCaseOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var testCasePriorityAssembly = typeof(TestCasePriorityAttribute).AssemblyQualifiedName!;

            return testCases.OrderBy(testCase => 
                testCase
                    .TestMethod.Method
                    .GetCustomAttributes(testCasePriorityAssembly)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>("Priority") ?? 0
            );
        }
    }
}
