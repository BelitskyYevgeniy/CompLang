using System.Threading;
using System.Threading.Tasks;

namespace CompLang.Interfaces.Managers
{
    public interface IPaginationManager
    {
        int PageSize { get;}
        int MaxPage { get; set; }
        int CurrentPage { get; set; }

        int Back();
        int Forward();
        void Reset();
        int GetPosition();
    }
}