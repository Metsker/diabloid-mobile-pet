using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services
{
    public interface IUIFactory : IService
    {
        void CreateShop();
        Task CreateUIRoot();
    }
}