using System;

namespace Shared
{
    public class ServiceConfiguration
    {
        private Uri _serviceDiscoveryAddress;

        private Uri _serviceAddress;

        private string _serviceId;

        private string _serviceName;

        public Uri ServiceDiscoveryAddress
        {
            get => _serviceDiscoveryAddress;
            set
            {
                _serviceDiscoveryAddress = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public Uri ServiceAddress
        {
            get => _serviceAddress;
            set
            {
                _serviceAddress = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string ServiceName
        {
            get => _serviceName;
            set
            {
                _serviceName = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public string ServiceId
        {
            get
            {
                if (_serviceId == null)
                    _serviceId = Guid.NewGuid().ToString();
                return _serviceId;
            }
        }
    }
}
