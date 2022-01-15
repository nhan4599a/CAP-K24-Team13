using System;

namespace GUI.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VirtualAreaAttribute : Attribute
    {
        public string Name { get; }

        public VirtualAreaAttribute(string name)
        {
            Name = name;
        }
    }
}
