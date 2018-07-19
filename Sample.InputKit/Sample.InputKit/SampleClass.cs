using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.InputKit
{
    public class SampleClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}
