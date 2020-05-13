using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web.Management;
using System.Web.Profile;
using System.Web.Security;
using System.Configuration.Provider;
using System.Reflection;

namespace Common.Configuration.Deployment.Providers
{

	internal delegate void BaseProviderInitializer(string name, NameValueCollection config);

	#region DeploymentProviderUtil

	internal static class DeploymentProviderUtil
	{

		private static FieldInfo GetSqlProviderConnectionStringField(Type sqlProviderType)
		{
			FieldInfo field = sqlProviderType.GetField("_sqlConnectionString", BindingFlags.Instance | BindingFlags.NonPublic);
			if(field == null)
				throw new ProviderException(String.Format("Error getting connection string field for provider type: {0}", sqlProviderType.FullName));
			return field;
		}

		private static void SetSqlProviderConnectionString<TSqlProvider>(TSqlProvider sqlProvider, string connectionString)
			where TSqlProvider : ProviderBase
		{
			if(sqlProvider == null)
				throw new ArgumentNullException("sqlProvider");
			if(String.IsNullOrEmpty(connectionString))
				throw new ArgumentException("connectionString cannot be null or emtpy", "connectionString");
			FieldInfo field = GetSqlProviderConnectionStringField(typeof(TSqlProvider));
			field.SetValue(sqlProvider, connectionString);
		}

		private static string SafeGetDeploymentLocationConnectionStringName(string name)
		{
			if(String.IsNullOrEmpty(name))
				return null;
			try
			{
				return DeploymentLocationsManager.CurrentConnectionStringValues[name];
			}
			catch
			{
				return null;
			}
		}

		private static void PerformSqlProviderConnectionOverrideInitialize<TSqlProvider>(TSqlProvider sqlProvider, BaseProviderInitializer baseInitializer, string name, NameValueCollection config)
			where TSqlProvider : ProviderBase
		{
			if(config == null)
				throw new ArgumentNullException("config");
			string connectionStringName = config["connectionStringName"];
			string connectionString = SafeGetDeploymentLocationConnectionStringName(connectionStringName);
			baseInitializer(name, config);
			if(connectionString != null)
				SetSqlProviderConnectionString(sqlProvider, connectionString);
		}

		internal static void PerformConnectionOverrideInitialize(SqlMembershipProvider sqlProvider, BaseProviderInitializer baseInitializer, string name, NameValueCollection config)
		{
			PerformSqlProviderConnectionOverrideInitialize(sqlProvider, baseInitializer, name, config);
		}

		internal static void PerformConnectionOverrideInitialize(SqlRoleProvider sqlProvider, BaseProviderInitializer baseInitializer, string name, NameValueCollection config)
		{
			PerformSqlProviderConnectionOverrideInitialize(sqlProvider, baseInitializer, name, config);
		}

		internal static void PerformConnectionOverrideInitialize(SqlProfileProvider sqlProvider, BaseProviderInitializer baseInitializer, string name, NameValueCollection config)
		{
			PerformSqlProviderConnectionOverrideInitialize(sqlProvider, baseInitializer, name, config);
		}

		internal static void PerformConnectionOverrideInitialize(SqlWebEventProvider sqlProvider, BaseProviderInitializer baseInitializer, string name, NameValueCollection config)
		{
			PerformSqlProviderConnectionOverrideInitialize(sqlProvider, baseInitializer, name, config);
		}

	}

	#endregion

}
