﻿/*
 * Copyright (C) 2022 Jaden Phil Nebel (Onionware)
 *
 * This file is part of OnionMedia.
 * OnionMedia is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, version 3.

 * OnionMedia is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

 * You should have received a copy of the GNU Affero General Public License along with OnionMedia. If not, see <https://www.gnu.org/licenses/>.
 */

using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using OnionMedia.Core.ViewModels;
using System;
using Windows.Globalization.NumberFormatting;

namespace OnionMedia.Uno.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; }
        private DecimalFormatter RoundingFormatter { get; }

        public SettingsPage()
        {
            ViewModel = Ioc.Default.GetService<SettingsViewModel>();
            DecimalFormatter formatter = new()
            {
                IntegerDigits = 1,
                FractionDigits = 0,
                NumberRounder = new IncrementNumberRounder() { Increment = 1, RoundingAlgorithm = RoundingAlgorithm.RoundHalfUp }
            };
            RoundingFormatter = formatter;

            InitializeComponent();
        }
    }
}
