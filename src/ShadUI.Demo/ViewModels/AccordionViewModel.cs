using System;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ShadUI.Demo.ViewModels;

[Page("accordion")]
public sealed partial class AccordionViewModel : ViewModelBase, INavigable
{
    private readonly PageManager _pageManager;
    
    public AccordionViewModel(PageManager pageManager)
    {
        _pageManager = pageManager;
        var path = Path.Combine(AppContext.BaseDirectory, "views", "AccordionPage.axaml");
        UsageCode = path.ExtractByLineRange(59, 86).CleanIndentation();
    }
    
    [RelayCommand]
    private void BackPage()
    {
        _pageManager.Navigate<TypographyViewModel>();
    }

    [RelayCommand]
    private void NextPage()
    {
        _pageManager.Navigate<AvatarViewModel>();
    }
    
    [ObservableProperty]
    private string _usageCode = string.Empty;
}