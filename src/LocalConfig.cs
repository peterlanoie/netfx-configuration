using System;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Common.Configuration
{
	/// <summary>
	/// Used to define and read out custom section values from configuration files.
	/// </summary>
	public class LocalConfig
	{
		private const string PreferredSectionToUseKey = "LocalConfig:SectionToUse";
		private const string SectionToUseKey = "ConfigsectionToUse";
		private const string _inheritSectionKey = "LocalConfig:Inherits";

		private static string _keyUsed = null;
		private const string _cacheSectionNameKey = "LocalConfig:CacheSectionName";
		private static string _sectionName; // optional cached value; set above key=true in app web.config to cache section name

		/// <summary>
		/// Gets the application setting key determine at runtime that defines the configuration section to use.
		/// </summary>
		public static string KeyUsed { get { return _keyUsed; } }

		/// <summary>
		/// This is the main method used to get the values from our special config sections.
		/// Returns an empty string for a missing config entry.
		/// </summary>
		/// <param name="name">the name of the add element's key attribute you are looking for</param>
		/// <returns>the correct local configuration setting</returns>
		public static string GetLocalConfigValue(string name)
		{
			string value = GetLocalConfigValueRecursive(LocalSectionName, name);
			if (value != null)
			{
				return value;
			}
			Trace.TraceWarning("GetLocalConfigValue call didn't find value for key '{0}' in section '{1}'.", name, LocalSectionName);
			return string.Empty;
			//throw new ApplicationException("Could not find an element with key of " + name + " in server dependent section");
		}

		private static string GetLocalConfigValueRecursive(string sectionName, string valueKeyName)
		{
			NameValueCollection section = GetConfigSection(sectionName);
			string value = section[valueKeyName];
			if (value == null)
			{
				string inheritSectionName = section[_inheritSectionKey];
				if (inheritSectionName != null)
				{
					if (inheritSectionName.Equals(sectionName, StringComparison.InvariantCultureIgnoreCase))
					{
						throw new ApplicationException(string.Format(
							"The local config section '{0}' contains a recursive inheritance reference to itself.\n" +
							"Please fix the entry that looks like this: <add key=\"{1}\" value=\"{2}\" />", sectionName, _inheritSectionKey, inheritSectionName));
					}
					value = GetLocalConfigValueRecursive(inheritSectionName, valueKeyName);
				}
			}
			return value;
		}

		/// <summary>
		/// Gets the named local config value or the defaultValue if the config entry is missing.
		/// </summary>
		/// <param name="name">Config key name.</param>
		/// <param name="defaultValue">Default value if no config entry found for provided key.</param>
		/// <returns></returns>
		public static string GetLocalConfigValue(string name, string defaultValue)
		{
			var value = GetLocalConfigValue(name);
			return string.IsNullOrEmpty(value) ? defaultValue : value;
		}

		/// <summary>
		/// Gets the named local config boolean value or the defaultValue if the config entry is missing.
		/// </summary>
		/// <param name="name">Config key name.</param>
		/// <param name="defaultValue">Default value if no config entry found for provided key.</param>
		/// <returns></returns>
		public static bool GetLocalConfigValue(string name, bool defaultValue)
		{
			var value = GetLocalConfigValue(name);
			return string.IsNullOrEmpty(value) ? defaultValue : bool.Parse(value);
		}

		/// <summary>
		/// Gets the named local config value, converting it using the converter.
		/// Returns defaultValue if the value is missing.
		/// </summary>
		/// <typeparam name="T">Return type.</typeparam>
		/// <param name="name">Config key name.</param>
		/// <param name="converter">Function to convert string value into return type.</param>
		/// <param name="defaultValue">Default value if no config entry found for provided key.</param>
		/// <returns></returns>
		public static T GetLocalConfigValue<T>(string name, Func<string, T> converter, T defaultValue)
		{
			var value = GetLocalConfigValue(name);
			return string.IsNullOrEmpty(value) ? defaultValue : converter(value);
		}

		/// <summary>
		/// Get a values from any of our special named config sections
		/// </summary>		
		public static string GetSectionConfigValue(string sectionName, string settingName)
		{
			object FoundSection = ConfigurationManager.GetSection(sectionName);
			string Result = string.Empty;
			if (FoundSection == null)
			{
				throw GetConfigError(sectionName);
			}
			else
			{
				NameValueCollection Settings = (NameValueCollection)FoundSection;

				if (Settings[settingName] != null && Settings[settingName] != string.Empty)
				{
					Result = Settings[settingName];
				}
				else
				{
					throw new ApplicationException(string.Format("Setting named {0} is empty or not found in config section {1}", settingName, sectionName));
				}
			}

			return Result;
		}

		/// <summary>
		/// Used to check if a particular class has the TraceClass setting for it (in the .config file) set to true or false.
		/// </summary>
		/// <param name="ClassName">The name of the class.</param>
		/// <returns>T|F</returns>
		public static bool IsTraceClassOn(string ClassName)
		{
			return Convert.ToBoolean(ConfigurationManager.AppSettings["TraceClass__" + ClassName]);
		}

		/// <summary>
		/// This class looks up an appsetting by key and prepends the value of AppRootDir (for the correct server-dependent section - dev, stage or prod) to its value.
		/// </summary>
		/// <param name="name">The named value.</param>
		/// <returns>The path-dependent string</returns>
		/// <remarks>This method should be removed. The same result can be achieved with the Page.ResolveUrl() method. Use the ~ character as the app root placeholder.</remarks>
		[Obsolete("The web application path resolution technique [ResolveUrl('~')] should be used in favor of this.")]
		public static string GetPathDependentString(string name)
		{
			if (ServerDependentSection["AppRootDir"] != null)
			{
				string rv = ServerDependentSection["AppRootDir"] + ConfigurationManager.AppSettings[name];
				return rv;
			}
			else
			{
				Trace.TraceWarning("The AppRootDir value is null in the server dependent section '{0}.", LocalSectionName);
				throw new ApplicationException("The AppRootDir value is null in the current ServerDependentSection.");
			}
		}

		/// <summary>
		/// This gets the current server-dependent section (i.e., dev, stage or prod) as a NameValueCollection
		/// </summary>		
		public static NameValueCollection ServerDependentSection
		{
			get
			{
				return GetConfigSection(LocalSectionName);
			}
		}

		private static ApplicationException GetConfigError(string strSectionName)
		{
			string strErrorMsg = @"Configuration section '{0}' expected but not found.
Please ensure the application's config file contains a 'configSections' child element:
    <configSections>
        <section name=""{0}"" type=""System.Configuration.NameValueFileSectionHandler""/>
    </configSections>
and a 'configuration' child element with the expected value:
    <{0}>
        <!-- app setting '<add ... />' elements -->
    </{0}>";

			return new ApplicationException(string.Format(strErrorMsg, strSectionName));
		}

		/// <summary>
		/// Gets the name of the web server for the current HttpContext, if there is one, otherwise returns NULL.
		/// </summary>
		public static string WebServerName
		{
			get
			{
				if (HttpContext.Current != null && HttpContext.Current.Request != null)
				{
					return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the name of the local configuration section based on the runtime server URL when in an HttpContext
		/// or the machine name if present, or the defined "ConfigsectionToUse".  Default return value is 'dev'.
		/// </summary>
		public static string LocalSectionName
		{
			get
			{
				// use cached value if available
				if (_sectionName != null)
				{
					return _sectionName;
				}

				string strSectionName = null;

				// first, try for a defined section using the more specific key name
				_keyUsed = PreferredSectionToUseKey;
				if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[_keyUsed]))
				{
					strSectionName = ConfigurationManager.AppSettings[_keyUsed];
				}

				// still nothing, try to derive from HTTP context
				if (strSectionName == null)
				{
					strSectionName = GetSectionNameFromHttp();
				}

				// still nothing, could be an EXE, check for a machine name defined
				if (strSectionName == null)
				{

					var strServer = Environment.MachineName;
					strSectionName = ConfigurationManager.AppSettings[_keyUsed = string.Format("MACHINE__{0}", strServer)];

					// REFACTORING CAUTION:
					// while it could make sense to move this next branch up higher in the sequence,
					// doing so could break existing configurations that use this as the fallback
					// for machine name driven scenarios that run on a machine not defined in the config
					if (strSectionName == null)
					{
						// no machine name, try for a defined section
						_keyUsed = SectionToUseKey;
						if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[SectionToUseKey]))
						{
							strSectionName = ConfigurationManager.AppSettings[_keyUsed];
						}
					}
				}

				if (UseSectionNameCache)
				{
					_sectionName = strSectionName;
				}

				//still nothing? throw error
				if (strSectionName == null)
				{
					throw new ApplicationException(string.Format(@"Local config is unable to determine what config section to use. 
Please define at least one regular App setting key with a local config section name value.
The following keys are recognized (ordered by precedence) in the current runtime context:
    - <add key=""LocalConfig:SectionToUse"" value=""{{section name}}"" />
    - <add key=""IP__{0}"" value=""{{section name}}"" /> (web apps)
    - <add key=""MACHINE__{1}"" value=""{{section name}}"" /> (non web apps)", WebServerName ?? "{web hostname}", Environment.MachineName));
				}

				return strSectionName;
			}
		}

		/// <summary>
		/// Gets whether the section name is cached for quicker use
		/// </summary>
		public static bool UseSectionNameCache
		{
			get
			{
				var cacheNameValue = ConfigurationManager.AppSettings[_cacheSectionNameKey];
				if (cacheNameValue != null)
				{
					return bool.Parse(cacheNameValue);
				}
				return false;
			}
		}

		private static string GetSectionNameFromHttp()
		{
			string strServer;
			string strSectionName = null;
			var requestAvailable = false;

			try
			{
				if (HttpContext.Current != null)
				{
					requestAvailable = (HttpContext.Current.Request != null);
				}
			}
			catch
			{
				//noop... request is not yet available (probably running under IIS 7, and being
				// called from Application start...
			}

			// Check for a web context
			if ((HttpContext.Current != null) && (requestAvailable))
			{
				strServer = WebServerName.ToLower();

				var hostNames = ConfigurationManager.AppSettings.AllKeys
					.Where(x => x.StartsWith("IP__"))
					.OrderByDescending(x => x.Length)
					.Select(x => x.Replace("IP__", ""));

				foreach (var hostName in hostNames)
				{
					if (
						(strServer == hostName.ToLower()) // exact match?
						||
						(hostName.StartsWith("*.") && strServer.EndsWith(hostName.Replace("*", ""))) // wildcard pattern match?
					)
					{
						_keyUsed = string.Format("IP__{0}", hostName);
						strSectionName = ConfigurationManager.AppSettings[_keyUsed];
						break; // exit loop
					}
				}
			}

			return strSectionName;
		}

		/// <summary>
		/// Returns a config section by name
		/// </summary>
		/// <param name="sectionName">the one you want</param>		
		public static NameValueCollection GetConfigSection(string sectionName)
		{
			object objSection = ConfigurationManager.GetSection(sectionName);
			if (objSection == null)
			{
				throw GetConfigError(sectionName);
			}
			return (NameValueCollection)objSection;
		}

		/// <summary>
		/// Given a string dictionary, parse the .config file for entries matching the prefix, and add to the dictionary
		/// </summary>
		/// <param name="Coll">Your dictionary</param>
		/// <param name="Prefix">Your prefix</param>
		public static void BuildCollectionFromConfig(StringDictionary Coll, string Prefix)
		{
			BuildCollectionFromConfig(Coll, Prefix, string.Empty);
		}

		/// <summary>
		/// Given a string dictionary, parse the .config file for entries matching a prefix and midfix (located after the prefix), and add to the dictionary
		/// </summary>
		/// <param name="Coll">Your dictionary</param>
		/// <param name="Prefix">Your prefix</param>
		/// <param name="Midfix">Your "midfix"</param>
		public static void BuildCollectionFromConfig(StringDictionary Coll, string Prefix, string Midfix)
		{
			if (Coll == null)
			{
				Coll = new StringDictionary();
			}

			if (Midfix == null)
			{
				Midfix = string.Empty;
			}

			if (string.IsNullOrEmpty(Prefix))
			{
				throw new ApplicationException("SetupCollectionFromConfig requires a prefix and optional midfix to lookup values.");
			}

			string[] Keys = ServerDependentSection.AllKeys;
			foreach (string Key in Keys)
			{
				if (Key.StartsWith(Prefix + Midfix) && !Coll.ContainsKey(Key))
				{
					Coll.Add(Key.Remove(0, Prefix.Length), ServerDependentSection[Key]);
				}
			}
		}

		/// <summary>
		/// Gets the currently loaded value of IP__ or MACHINE__ from appSettings/add
		/// </summary>
		public static string GetLocalConfigAppSettingKey()
		{
			string Server = Environment.MachineName;
			bool RequestAvailable = false;

			try
			{
				if (HttpContext.Current != null)
					RequestAvailable = (HttpContext.Current.Request != null);
			}
			catch { }

			// Check for a web context
			if ((HttpContext.Current != null) && (RequestAvailable))
			{
				Server = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();
			}

			return Server;
		}
	}
}
