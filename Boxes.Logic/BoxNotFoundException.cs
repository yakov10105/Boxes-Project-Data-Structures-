using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Boxes.Logic
{
    public class BoxNotFoundException : Exception
    {
        public BoxNotFoundException()
        {
        }

        public BoxNotFoundException(string message) : base(message)
        {
        }

        public BoxNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BoxNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
