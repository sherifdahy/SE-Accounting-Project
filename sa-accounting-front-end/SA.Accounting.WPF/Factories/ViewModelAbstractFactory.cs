using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.Factories;

public class ViewModelAbstractFactory : IViewModelAbstractFactory
{
    private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
    private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
    private readonly CreateViewModel<PlatformsViewModel> _creatPlatformsViewModel;
    private readonly CreateViewModel<CreatePlatformViewModel> _createCreatePlatformViewModel;
    private readonly CreateViewModel<UpdatePlatformViewModel> _createUpdatePlatformViewModel;
    private readonly CreateViewModel<UsersViewModel> _createUsersViewModel;
    private readonly CreateViewModel<CreateUserViewModel> _createCreateUserViewModel;
    private readonly CreateViewModel<UpdateUserViewModel> _createUserViewModel;
    private readonly CreateViewModel<UserBasicInfoViewModel> _createUserBasicInfoViewModel;
    private readonly CreateViewModel<UserCompaniesViewModel> _createUserCompaniesViewModel;
    private readonly CreateViewModel<CompaniesViewModel> _createCompaniesViewModel;
    private readonly CreateViewModel<DisplayCompanyViewModel> _createDisplayCompanyViewModel;
    private readonly CreateViewModel<CreateCompanyViewModel> _createCreateCompanyViewModel;
    private readonly CreateViewModel<UpdateCompanyViewModel> _createUpdateCompanyViewModel;
    private readonly CreateViewModel<TransactionsViewModel> _createTransactionsViewModel;
    private readonly CreateViewModel<DisplayTransactionViewModel> _createDisplayTransactionViewModel;
    private readonly CreateViewModel<CreateTransactionViewModel> _createCreateTransactionViewModel;
    private readonly CreateViewModel<UpdateTransactionViewModel> _createUpdateTransactionViewModel;
    private readonly CreateViewModel<ProfileViewModel> _createProfileViewModel;
    private readonly CreateViewModel<UserPermissionsViewModel> _createUserPermissionsViewModel;


    public ViewModelAbstractFactory(CreateViewModel<HomeViewModel> createHomeViewModel,
        CreateViewModel<LoginViewModel> createLoginViewModel,
        CreateViewModel<PlatformsViewModel> creatPlatformsViewModel,
        CreateViewModel<CreatePlatformViewModel> createCreatePlatformViewModel,
        CreateViewModel<UsersViewModel> createUsersViewModel,
        CreateViewModel<CreateUserViewModel> createCreateUserViewModel,
        CreateViewModel<UpdateUserViewModel> createUserViewModel,
        CreateViewModel<UserBasicInfoViewModel> createUserBasicInfoViewModel,
        CreateViewModel<UserCompaniesViewModel> createUserCompaniesViewModel,
        CreateViewModel<CompaniesViewModel> createCompaniesViewModel,
        CreateViewModel<DisplayCompanyViewModel> createDisplayCompanyViewModel,
        CreateViewModel<CreateCompanyViewModel> createCreateCompanyViewModel,
        CreateViewModel<UpdateCompanyViewModel> createUpdateCompanyViewModel,
        CreateViewModel<UpdatePlatformViewModel> createUpdatePlatformViewModel,
        CreateViewModel<TransactionsViewModel> createTransactionsViewModel,
        CreateViewModel<DisplayTransactionViewModel> createDisplayTransactionViewModel,
        CreateViewModel<CreateTransactionViewModel> createCreateTransactionViewModel,
        CreateViewModel<UpdateTransactionViewModel> createUpdateTransactionViewModel,
        CreateViewModel<UserPermissionsViewModel> createUserPermissionsViewModel,
        CreateViewModel<ProfileViewModel> createProfileViewModel)
    {
        _createHomeViewModel = createHomeViewModel;
        _createLoginViewModel = createLoginViewModel;
        _creatPlatformsViewModel = creatPlatformsViewModel;
        _createCreatePlatformViewModel = createCreatePlatformViewModel;
        _createUsersViewModel = createUsersViewModel;
        _createCreateUserViewModel = createCreateUserViewModel;
        _createUserViewModel = createUserViewModel;
        _createUserBasicInfoViewModel = createUserBasicInfoViewModel;
        _createUserCompaniesViewModel = createUserCompaniesViewModel;
        _createCompaniesViewModel = createCompaniesViewModel;
        _createDisplayCompanyViewModel = createDisplayCompanyViewModel;
        _createDisplayCompanyViewModel = createDisplayCompanyViewModel;
        _createCreateCompanyViewModel = createCreateCompanyViewModel;
        _createUpdateCompanyViewModel = createUpdateCompanyViewModel;
        _createUpdatePlatformViewModel = createUpdatePlatformViewModel;
        _createTransactionsViewModel = createTransactionsViewModel;
        _createDisplayTransactionViewModel = createDisplayTransactionViewModel;
        _createCreateTransactionViewModel = createCreateTransactionViewModel;
        _createUpdateTransactionViewModel = createUpdateTransactionViewModel;
        _createProfileViewModel = createProfileViewModel;
        _createUserPermissionsViewModel = createUserPermissionsViewModel;
    }

    public ViewModelBase CreateViewModel(ViewType viewType)
    {
        switch (viewType)
        {
            case ViewType.Login:
                return _createLoginViewModel();
            case ViewType.Home:
                return _createHomeViewModel();
            case ViewType.Platforms:
                return _creatPlatformsViewModel();
            case ViewType.CreatePlatform:
                return _createCreatePlatformViewModel();
            case ViewType.UpdatePlatform:
                return _createUpdatePlatformViewModel();
            case ViewType.Users:
                return _createUsersViewModel();
            case ViewType.CreateUser:
                return _createCreateUserViewModel();
            case ViewType.UpdateUser:
                return _createUserViewModel();
            case ViewType.UserBasicInfo:
                return _createUserBasicInfoViewModel();
            case ViewType.UserCompanies:
                return _createUserCompaniesViewModel();
            case ViewType.UserPermissions:
                return _createUserPermissionsViewModel();
            case ViewType.Companies:
                return _createCompaniesViewModel();
            case ViewType.UpdateCompany:
                return _createUpdateCompanyViewModel();
            case ViewType.DisplayCompany:
                return _createDisplayCompanyViewModel();
            case ViewType.CreateCompany:
                return _createCreateCompanyViewModel();
            case ViewType.Transactions:
                return _createTransactionsViewModel();
            case ViewType.DisplayTransaction:
                return _createDisplayTransactionViewModel();
            case ViewType.CreateTransaction:
                return _createCreateTransactionViewModel();
            case ViewType.UpdateTransaction:
                return _createUpdateTransactionViewModel();
            case ViewType.Profile:
                return _createProfileViewModel();
            default:
                throw new ArgumentException("Invalid view type");
        }
    }
}
