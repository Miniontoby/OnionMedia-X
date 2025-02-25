﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using OnionMedia.Avalonia.UserControls;
using OnionMedia.Core.Models;
using OnionMedia.Core.ViewModels;

namespace OnionMedia.Avalonia.Views;

public partial class MediaPage : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public MediaViewModel? ViewModel => DataContext as MediaViewModel;

    public MediaPage()
    {
        InitializeComponent();
        DataContext = App.DefaultServiceProvider.GetService<MediaViewModel>();
        ViewModel.PropertyChanged += (o, e) =>
        {
	        if (e.PropertyName == nameof(MediaViewModel.SelectedItem) && ((MediaViewModel)DataContext).SelectedItem is not null)
			{
				var control = this.FindControl<TimeRangeSelector>("TimeSelector");
				control?.UpdateTimeSpanGroup(((MediaViewModel)DataContext).SelectedItem.VideoTimes);
			}

	        if (e.PropertyName == nameof(MediaViewModel.ItemSelectedAndIdle))
	        {
				var control = this.FindControl<TimeRangeSelector>("TimeSelector");
				control?.UpdateIsReadOnly(!((MediaViewModel)DataContext).ItemSelectedAndIdle);
			}
        };
    }

    protected override void OnLoaded()
    {
        base.OnLoaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged += UpdateSizeStyle;
        this.FindControl<NumberBox>("audioBitrateBox").NumberFormatter = NumberboxBitrateFormatter;
        this.FindControl<NumberBox>("videoBitrateBox").NumberFormatter = NumberboxBitrateFormatter;
    }

    protected override void OnUnloaded()
    {
        base.OnUnloaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow.SizeChanged -= UpdateSizeStyle;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public bool SmallWindowStyle { get; set; }

    private void UpdateSizeStyle(object? sender, SizeChangedEventArgs e)
    {
        SmallWindowStyle = e.NewSize.Width < 750;
        PropertyChanged?.Invoke(this, new(nameof(SmallWindowStyle)));
    }
    
    private void HideParentFlyout(Control element)
    {
        if (element is null) return;
        var parent = element.Parent;
        while (parent is not null)
        {
            if (parent is Popup flyout)
            {
                flyout.IsOpen = false;
                return;
            }
            parent = parent.Parent;
        }
    }
    
    private void Close_Flyout(object? sender, RoutedEventArgs e)
    {
        HideParentFlyout(sender as Control);
    }
    
    private void RemoveAllFlyoutButton_Click(object? sender, RoutedEventArgs e)
    {
        HideParentFlyout(sender as Control);
        ((MediaViewModel)DataContext).RemoveAllCommand.Execute(null);
    }
    
    private void ToggleButton_Click(object sender, RoutedEventArgs e) => ((ToggleButton)sender).IsChecked = true;

    private void Framerates_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems?.Count is 0 || e.AddedItems[0] is not double) return;
        ((MediaViewModel)DataContext).SetFramerateCommand.Execute((double)e.AddedItems[0]);
        HideParentFlyout(sender as Control);
    }

    private void Resolutions_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e?.AddedItems?.Count is 0 || e.AddedItems[0] is not Resolution) return;
        ((MediaViewModel)DataContext).SetResolution((Resolution)e.AddedItems[0]);
        Dispatcher.UIThread.Post(() => HideParentFlyout(sender as Control));
    }
        
    public static string NumberboxBitrateFormatter(double value) => value == 0 ? string.Empty : value.ToString(CultureInfo.InvariantCulture);
}