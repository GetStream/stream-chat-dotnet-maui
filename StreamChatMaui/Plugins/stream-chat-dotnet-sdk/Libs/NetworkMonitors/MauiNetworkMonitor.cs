using Microsoft.Maui.Networking;
using System;

namespace StreamChat.Libs.NetworkMonitors
{
    public delegate void NetworkAvailabilityChangedEventHandler(bool isNetworkAvailable);

    public class MauiNetworkMonitor : INetworkMonitor
    {
        public event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged;

        private bool _isNetworkAvailable;

        public bool IsNetworkAvailable
        {
            get => _isNetworkAvailable;
            private set
            {
                if (_isNetworkAvailable != value)
                {
                    _isNetworkAvailable = value;
                    NetworkAvailabilityChanged?.Invoke(_isNetworkAvailable);
                }
            }
        }

        public void Update()
        {

        }

        public MauiNetworkMonitor()
        {
            UpdateNetworkAvailability();
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            UpdateNetworkAvailability();
        }

        private void UpdateNetworkAvailability()
        {
            var profiles = Connectivity.Current.NetworkAccess;
            IsNetworkAvailable = profiles == NetworkAccess.Internet;
        }

        public void Dispose()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }
    }
}