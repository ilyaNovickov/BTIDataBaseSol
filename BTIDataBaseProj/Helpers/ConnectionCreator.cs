using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Security;

namespace BTIDataBaseProj.Helpers
{
    /// <summary>
    /// Статический класс формирования строки подключения к БД
    /// </summary>
    internal static class ConnectionCreator
    {
        /// <summary>
        /// Формирует строку подключения к БД. Подключение через пользователя
        /// </summary>
        /// <param name="userId">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="connectionStringName">Базовая строка подключения в файле *.exe.config (при разработке app.config)</param>
        /// <returns></returns>
        public static string GetConnection(string userId, string password, 
            string connectionStringName = "partialConnectString")
        {
            SqlConnectionStringBuilder sqlBuilder = new 
                SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings[connectionStringName].
                ConnectionString);

            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.UserID = userId;
            sqlBuilder.Password = password;

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            entityBuilder.Provider = "System.Data.SqlClient";

            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();

            entityBuilder.Metadata = "res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl";

            return entityBuilder.ToString();
        }

        /// <summary>
        /// Формирует строку подключения к БД. Подключение через аунтификацию Windows
        /// </summary>
        /// <param name="connectionStringName">Базовая строка подключения в файле *.exe.config (при разработке app.config)</param>
        /// <returns></returns>
        public static string GetConnection(string connectionStringName = "partialConnectString")
        {
            SqlConnectionStringBuilder sqlBuilder = new
                SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings[connectionStringName].
                ConnectionString);

            sqlBuilder.IntegratedSecurity = true;

            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            entityBuilder.Provider = "System.Data.SqlClient";

            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();

            entityBuilder.Metadata = "res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl";

            return entityBuilder.ToString();
        }
    }
}
