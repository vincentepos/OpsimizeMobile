using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
