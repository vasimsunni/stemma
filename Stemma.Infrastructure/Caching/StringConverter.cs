using System;
using System.Collections.Generic;
using System.Text;

namespace Stemma.Infrastructure.Caching
{
    internal class StringConverter : IConverter<string>
    {
        public string Deserialize(string value)
        {
            return value;
        }

        public string Serialize(object obj)
        {
            return obj.ToString();
        }
    }
}
