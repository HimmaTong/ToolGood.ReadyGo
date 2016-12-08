﻿using System.Data.Common;

namespace ToolGood.ReadyGo.Providers
{
    partial class MySqlDatabaseProvider : DatabaseProvider
    {
        public override DbProviderFactory GetFactory()
        {
            return GetFactory("MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data, Culture=neutral, PublicKeyToken=c5687fc88969c44d");
        }

        public override string GetParameterPrefix(string connectionString)
        {
            if (connectionString != null && connectionString.IndexOf("Allow User Variables=true") >= 0)
                return "?";
            else
                return "@";
        }

        public override string EscapeSqlIdentifier(string sqlIdentifier)
        {
            return "`" + sqlIdentifier + "`";
            //return string.Format("`{0}`", sqlIdentifier);
        }

        //public override string GetExistsSql()
        //{
        //    return "SELECT EXISTS (SELECT 1 FROM {0} WHERE {1})";
        //}
    }
}