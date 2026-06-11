namespace Organization.Domain.Commons.Utilities
{
    public class QueryParameters
    {
        //FIELDS with default values
        public int _maxPageSize = 100; //default values for TESTING only
        public int _pageSize = 100;     //default values for TESTING only

        //PROPERTY 
        public int PageNumber { get; set; } = 1; //default value
         
        //PROPERTY 
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                //returns the SMALLER integer number of "_maxPageSize or the PROPERY set value"
                _pageSize = Math.Min(_maxPageSize, value);
            }
        }

    }
}
