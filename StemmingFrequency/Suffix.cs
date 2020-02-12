using System;
using System.Collections.Generic;
using System.Text;

namespace StemmingFrequency
{
    public class Suffix
    {
        public string Value { get; set; }
        public string Predecessor { get; set; }
        public string IncompatibleWith { get; set; }
    }
}
