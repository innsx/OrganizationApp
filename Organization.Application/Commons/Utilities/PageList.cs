namespace Organization.Application.Commons.Utilities
{
    public class PageList<TEntity>
    {
        //PRIVATE CONSTRUCTOR preventing PageList.cs class from being INSTANTIATED/INVOKED from outside of this class
        private PageList(IEnumerable<TEntity> items, int pageNumber, int pageSize, int totalCount)
        {
            //initializing our PROPERTIES with LOCAL FIELDS
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        //PROPERTIES
        public IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage => PageNumber * PageSize < TotalCount;
        public bool HasPreviousPage => PageNumber > 1;

        //So we add a STATIC Create( ) METHOD that will be ACCESSABLE outside of this class
        // and are passing in THESE "PARAMETERS" 
        // then we use these "PARAMETERS" to use PageList class's CONSTRUCTOR to initialize these "PARAMETERS" 
        // and ASSIGN these "PARAMETERS" to it respect PROPERTIES
        public static PageList<TEntity> Create(IEnumerable<TEntity> items, int pageNumber, int pageSize, int totalCount)
        {
            return new PageList<TEntity>(items, pageNumber, pageSize, totalCount);
        }

    }
}
