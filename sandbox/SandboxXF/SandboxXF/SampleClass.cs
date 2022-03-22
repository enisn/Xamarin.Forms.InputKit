using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxXF
{
    public class SampleClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}
