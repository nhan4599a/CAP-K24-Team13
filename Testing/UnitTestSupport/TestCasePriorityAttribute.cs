namespace UnitTestSupport
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestCasePriorityAttribute : Attribute
    {
        public int Priority { get; set; }

        public TestCasePriorityAttribute(int priority) => Priority = priority;
    }
}