using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.Interfaces;

public enum ViewType
{
    Login,
    Home,
    Platforms,
    CreatePlatform,
    UpdatePlatform,
    Companies,
    CreateCompany,
    DisplayCompany,
    UpdateCompany,
    Users,
    CreateUser,
    UpdateUser,
    UserBasicInfo,
    UserCompanies,
    UserCustodies,
    UserPermissions,
    Funds,
    Transactions,
    DisplayTransaction,
    CreateTransaction,
    UpdateTransaction,
    Profile,
    None
}
public interface IViewModelAbstractFactory
{
    ViewModelBase CreateViewModel(ViewType viewType);
}
