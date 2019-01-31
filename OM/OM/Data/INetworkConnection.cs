using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    public interface INetworkConnection
    {
        bool IsConnceted { get; }
        void CheckInternetConnection();
    }
}
