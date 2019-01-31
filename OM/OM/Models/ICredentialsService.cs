using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Models
{
    public interface ICredentialsService
    {
        string UserName { get; }

        string Password { get; }

        string DToken { get; }

        string License { get; }

        void SaveCredentials(string userName, string password, string DToken, string license);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }
}
