using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Common.Configuration.Deployment
{

	#region DeploymentUtil

	internal static class DeploymentUtil
	{

		internal static bool InWebApplication
		{
			get { return HttpContext.Current != null; }
		}

		internal static string CurrentHostName
		{
			get
			{
				if(!InWebApplication)
					return Environment.MachineName;
				return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
			}
		}

		internal static StringComparer HostNameComparer
		{
			get { return StringComparer.OrdinalIgnoreCase; }
		}

		internal static string PrepareHostName(string hostName)
		{
			return String.IsNullOrEmpty(hostName) ? (hostName ?? String.Empty) : hostName.Trim();
		}

		internal static bool HostNameEquals(string hostName1, string hostName2)
		{
			return HostNameComparer.Equals(PrepareHostName(hostName1), PrepareHostName(hostName2));
		}

		internal static NameValueCollection CreateMergedCopy(NameValueCollection primary, NameValueCollection fallback)
		{
			if(primary == null)
				throw new ArgumentNullException("primary");
			if(fallback == null)
				throw new ArgumentNullException("fallback");
			NameValueCollection merged = new NameValueCollection(fallback);
			foreach(string key in primary.AllKeys)
				merged[key] = primary[key];
			//merged.Add(primary);
			return merged;
		}

		internal static Dictionary<string, string> GetConnectionStringDictionary(ConnectionStringSettingsCollection connectionStrings)
		{
			if(connectionStrings == null)
				throw new ArgumentNullException("connectionStrings");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach(ConnectionStringSettings connectionString in connectionStrings)
				dictionary.Add(connectionString.Name, connectionString.ConnectionString);
			return dictionary;
		}

		internal static NameValueCollection GetValueCollection(ConnectionStringSettingsCollection connectionStrings)
		{
			if(connectionStrings == null)
				throw new ArgumentNullException("connectionStrings");
			NameValueCollection values = new NameValueCollection();
			foreach(ConnectionStringSettings connectionString in connectionStrings)
				values.Add(connectionString.Name, connectionString.ConnectionString);
			return values;
		}

		internal static NameValueCollection GetValueCollection(KeyValueConfigurationCollection keyValueElements)
		{
			if(keyValueElements == null)
				throw new ArgumentNullException("keyValueElements");
			NameValueCollection values = new NameValueCollection();
			foreach(KeyValueConfigurationElement keyValueElement in keyValueElements)
				values.Add(keyValueElement.Key, keyValueElement.Value);
			return values;
		}

		internal static void ApplyOverrides<TKey, TValue>(Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> overrides)
		{
			if(dictionary == null)
				throw new ArgumentNullException("dictionary");
			if(overrides == null)
				throw new ArgumentNullException("overrides");
			foreach(KeyValuePair<TKey, TValue> entry in overrides)
				dictionary[entry.Key] = entry.Value;
		}

		internal static string GetValue(NameValueCollection primary, NameValueCollection fallback, string name)
		{
			if(String.IsNullOrEmpty(name))
				return null;
			if(primary != null)
			{
				string value = primary[name];
				if(value != null)
					return value;
			}
			return fallback == null ? null : fallback[name];
		}

		internal static string GetSetting(KeyValueConfigurationCollection primary, NameValueCollection fallback, string name)
		{
			if(String.IsNullOrEmpty(name))
				return null;
			if(primary != null)
			{
				KeyValueConfigurationElement element = primary[name];
				if(element != null)
					return element.Value;
			}
			return fallback == null ? null : fallback[name];
		}

		internal static string GetConnectionString(ConnectionStringSettingsCollection primary, ConnectionStringSettingsCollection fallback, string name)
		{
			if(String.IsNullOrEmpty(name))
				return null;
			ConnectionStringSettings element;
			if(primary != null)
			{
				element = primary[name];
				if(element != null)
					return element.ConnectionString;
			}
			if(fallback != null)
			{
				element = fallback[name];
				if(element != null)
					return element.ConnectionString;
			}
			return null;
		}

	}

	#endregion

}
