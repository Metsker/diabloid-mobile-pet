using CodeBase.Infrastructure.Services;
using CodeBase.Logic;

namespace CodeBase.UI.Services
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        void CreateShop();
    }
}