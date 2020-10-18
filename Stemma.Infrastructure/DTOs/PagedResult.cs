using System.Collections.Generic;

namespace Stemma.Infrastructure.DTOs
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Records { get; set; }

        public PagedResult()
        {
            Records = new List<T>();
        }
    }
}
