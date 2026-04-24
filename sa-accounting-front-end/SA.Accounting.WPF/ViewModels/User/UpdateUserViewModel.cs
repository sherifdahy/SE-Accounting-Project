using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class UpdateUserViewModel : ViewModelBase, IAsyncInitializable<int>
{
    private int _userId;
    private readonly IViewModelAbstractFactory _viewModelFactory;
    private readonly INavigator _navigator;
    private readonly IDialogService _dialogService;

    public override ViewType Section => ViewType.Users;

    // ══════ Current Tab Content ══════
    private ViewModelBase _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set { _currentViewModel = value; OnPropertyChanged(); }
    }

    // ══════ Tab State ══════
    private string _currentSection = "BasicInfo";
    public string CurrentSection
    {
        get => _currentSection;
        set { _currentSection = value; OnPropertyChanged(); }
    }

    // ══════ Commands ══════
    public ICommand ShowBasicInfoCommand { get; }
    public ICommand ShowCompaniesCommand { get; }
    public ICommand ShowCustodiesCommand { get; }
    public ICommand GoBackCommand { get; }

    public UpdateUserViewModel(
        IViewModelAbstractFactory viewModelFactory,
        INavigator navigator,
        IDialogService dialogService)
    {
        _viewModelFactory = viewModelFactory;
        _navigator = navigator;
        _dialogService = dialogService;

        ShowBasicInfoCommand = new AsyncRelayCommand(async _ =>
            await NavigateToTabAsync("BasicInfo", ViewType.UserBasicInfo));
        ShowCompaniesCommand = new AsyncRelayCommand(async _ =>
            await NavigateToTabAsync("Companies", ViewType.UserCompanies));
        ShowCustodiesCommand = new AsyncRelayCommand(async _ =>
            await NavigateToTabAsync("Custodies", ViewType.UserCustodies));
        GoBackCommand = new RelayCommand(_ => GoBack());
    }

    public async Task InitializeAsync(int userId)
    {
        _userId = userId;
        await NavigateToTabAsync("BasicInfo", ViewType.UserBasicInfo);
    }

    private async Task NavigateToTabAsync(string section, ViewType viewType)
    {
        if (CurrentSection == section && CurrentViewModel != null)
            return;

        try
        {
            CurrentSection = section;

            var vm = _viewModelFactory.CreateViewModel(viewType);

            if (vm is IAsyncInitializable<int> init)
                await init.InitializeAsync(_userId);

            CurrentViewModel = vm;  // ✅ ده دلوقتي بيعمل OnPropertyChanged
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل البيانات");
        }
    }

    private void GoBack()
    {
        var vm = _viewModelFactory.CreateViewModel(ViewType.Users);
        if (vm is IAsyncInitializable init) _ = init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}