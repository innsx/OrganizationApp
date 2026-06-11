namespace Organization.Domain.Commons.Utilities
{
    public class QueryParameters
    {
        //FIELDS with default values
        private int _maxPageSize = 100; //default values for TESTING only
        private int _pageSize = 100;     //default values for TESTING only
        private string _sortOrder = "asc"; //default sortOrder is ascending
        private string _filterBy = string.Empty;
        private string _sortBy = "PagingOrder";

        public string FilterBy
        {
            get
            {
                return _filterBy;
            }

            set
            { 
                _filterBy = value.ToLower();
            }
        } 

        //PROPERTY 
        public int PageNumber { get; set; } = 1; //default value is 1
         
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

        //PROPERTY
        public string SortBy
        { 
            get
            {
                return _sortBy;
            }
            
            set
            {
                string strg = value.ToLower();
                _sortBy = char.ToUpper(strg[0]) + strg.Substring(1); 
            }        
        }  


        //PROPERTY
        public string SortOrder 
        {
            
            get
            {
                return _sortOrder;
            } 
            
            set
            {
                if (value.ToLower() == "asc" || value.ToLower() == "desc")
                { 
                    _sortOrder = value.ToLower();
                }
            }
        }
    }
}
