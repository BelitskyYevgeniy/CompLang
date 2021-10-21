using CompLang.BLL.Interfaces.Providers;
using CompLang.Interfaces.Managers;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompLang.Managers
{
    public class PaginationManager : IPaginationManager
    {
        private int _maxPage;
        private int _currentPage;

        public int PageSize { get; private set; }
        public int MaxPage 
        {
            get
            {
                return _maxPage;
            }
            set
            {
                if(value <= 0)
                {
                    _maxPage = 1;
                    return;
                }
                _maxPage = value;
            }
        }
        public int CurrentPage 
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if(value <=0 )
                {
                    value = (MaxPage + value);
                }
                _currentPage = value % MaxPage;
            }
        }

        public PaginationManager(IConfiguration configuration)
        {
            PageSize = Int32.Parse(configuration.GetSection("PaginationSize").Value);
            this.MaxPage = 1;
            Reset();
        }
        public int Back()
        {
            return --CurrentPage;
        }

        public int Forward()
        {
            return ++CurrentPage;
        }
        public void Reset()
        {
            _currentPage = 0;
  
        }
        public int GetPosition()
        {
            return PageSize * CurrentPage;
        }
    }
}
