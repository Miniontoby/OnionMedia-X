﻿/*
 * Copyright (C) 2022 Jaden Phil Nebel (Onionware)
 *
 * This file is part of OnionMedia.
 * OnionMedia is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public License as published by the Free Software Foundation, version 3.

 * OnionMedia is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more details.

 * You should have received a copy of the GNU Affero General Public License along with OnionMedia. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using Windows.Networking.Connectivity;
using OnionMedia.Core.Services;

namespace OnionMedia.Services
{
    sealed class NetworkStatusService : INetworkStatusService
    {
        public NetworkStatusService()
        {
            NetworkInformation.NetworkStatusChanged += o => ConnectionStateChanged?.Invoke(this, NetworkInformation.GetInternetConnectionProfile() != null);
        }

        public event EventHandler<bool> ConnectionStateChanged;

        public bool IsNetworkConnectionAvailable()
        {
            return NetworkInformation.GetInternetConnectionProfile() != null;
        }
    }
}
