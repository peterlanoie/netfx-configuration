using System;
using System.Configuration;
using System.Collections.Specialized;

namespace Common.Configuration.Deployment
{

	#region DeploymentLocationsManager

	///<summary>
	/// A utility for accessing configured DeploymentLocationSettings, and determining
	/// the current location based on server name or machine name.
	///</summary>
	public static class DeploymentLocationsManager
	{

		#region Fields / Constants

		//private static DeploymentLocationsManagerInternal _Manager = null;

		private static DeploymentLocationsSection _Section;

		#endregion

		#region Properties

		private static DeploymentLocationsManagerInternal Manager
		{
			get
			{
				//return _Manager ?? (_Manager = new DeploymentLocationsManagerInternal());
				return new DeploymentLocationsManagerInternal();
			}
		}

		///<summary>
		/// The DeploymentLocationsSection from app.config or web.config
		///</summary>
		public static DeploymentLocationsSection DeploymentLocationsSection
		{
			get
			{
				//return Manager.Section;
				return _Section ?? (_Section = LoadSection());
			}
		}

		/// <summary>
		/// The current host name, determined by the SERVER_NAME server variable
		/// if an HttpContext is available, and Environment.MachineName otherwise.
		/// </summary>
		public static string CurrentHostName
		{
			get
			{
				//return Manager.CurrentHostName;
				return GetCurrentHostNameChecked();
			}
		}

		///<summary>
		/// Currently configured location, based on determined CurrentHostName
		///</summary>
		public static DeploymentLocationSettings CurrentLocation
		{
			get
			{
				//return Manager.CurrentLocation;
				DeploymentLocationsSection section = DeploymentLocationsSection;
				if(section == null)
					return null;
				string hostName = GetCurrentHostNameChecked();
				return GetLocationByHostNameChecked(section, hostName);
			}
		}

		///<summary>
		/// The AppSettings configuration collection of the CurrentLocation
		///</summary>
		[Obsolete("Use CurrentAppSettingValues")]
		public static KeyValueConfigurationCollection CurrentAppSettings
		{
			get { return CurrentLocation.AppSettings; }
		}

		/// <summary>
		/// The AppSettings of the CurrentLocation and the standard AppSettings configuratio,
		/// available as key/value pairs
		/// </summary>
		public static NameValueCollection CurrentAppSettingValues
		{
			get
			{
				return Manager.AppSettings;
			}
		}

		///<summary>
		/// The ConnectionStrings configuration collection of the CurrentLocation
		///</summary>
		[Obsolete("Use CurrentConnectionStringValues")]
		public static ConnectionStringSettingsCollection CurrentConnectionStrings
		{
			get { return CurrentLocation.ConnectionStrings; }
		}

		/// <summary>
		/// 
		/// </summary>
		public static NameValueCollection CurrentConnectionStringValues
		{
			get
			{
				return Manager.ConnectionStrings;
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal static string GetCurrentHostNameChecked()
		{
			string hostName;
			try
			{
				hostName = DeploymentUtil.CurrentHostName;
			}
			catch(Exception ex)
			{
				throw new ConfigurationErrorsException("Cannot determine current host name", ex);
			}
			if(String.IsNullOrEmpty(hostName))
				throw new ConfigurationErrorsException("Cannot determine current host name");
			return hostName;
		}

		internal static DeploymentLocationSettings GetLocationByHostNameChecked(DeploymentLocationsSection section, string hostName)
		{
			DeploymentLocationSettings location = section.GetLocationByHostName(hostName);
			if(location == null)
				throw new ConfigurationErrorsException(String.Format("Current deploymentLocation cannot be determined for host name: \"{0}\"", hostName));
			return location;
		}

		private static DeploymentLocationsSection LoadSection()
		{
			DeploymentLocationsSection section = (DeploymentLocationsSection)ConfigurationManager.GetSection(DeploymentLocationsSection.SectionElementName);
			//if(section == null)
			//    throw new ConfigurationErrorsException("deploymentLocations config section not found");
			//if(section == null)
			//    return new DeploymentLocationsSection();
			return section;
		}

		/// <summary>
		/// Gets a single app setting based on the current location.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetAppSetting(string key)
		{
			//return CurrentAppSettingValues[key];
			DeploymentLocationSettings location = CurrentLocation;
			return DeploymentUtil.GetSetting(location == null ? null : location.AppSettings, ConfigurationManager.AppSettings, key);
		}

		/// <summary>
		/// Gets a single connection string based on the current location.
		/// </summary>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		public static string GetConnection(string connectionName)
		{
			//return CurrentConnectionStringValues[connectionName];
			DeploymentLocationSettings location = CurrentLocation;
			return DeploymentUtil.GetConnectionString(location == null ? null : location.ConnectionStrings, ConfigurationManager.ConnectionStrings, connectionName);
		}

		//public static void Refresh()
		//{
		//    if(!_CurrentHostNameDetermined)
		//    {
		//        LoadCurrentLocation();
		//    }
		//    else
		//    {
		//        string oldHostName = _CurrentHostName;
		//        _CurrentHostNameDetermined = false;
		//        LoadCurrentHostName();
		//        if(oldHostName == null || !DeploymentUtil.HostNameEquals(_CurrentHostName, oldHostName))
		//            LoadCurrentLocation();
		//    }
		//}

		///<summary>
		/// Clear cached current location and configurations
		///</summary>
		[Obsolete]
		public static void Reset()
		{
			//_Manager = null;
		}

		#endregion

	}

	#endregion

	#region DeploymentLocationsManagerInternal

	internal class DeploymentLocationsManagerInternal
	{

		#region Static / Constant

		#endregion

		#region Fields

		private readonly string _CurrentHostName;
		private readonly DeploymentLocationSettings _CurrentLocation;
		private readonly NameValueCollection _AppSettings;
		private readonly NameValueCollection _ConnectionStrings;

		#endregion

		#region Properties

		[Obsolete("Use DeploymentLocationsManager.CurrentHostName")]
		public string CurrentHostName { get { return _CurrentHostName; } }

		[Obsolete("Use DeploymentLocationsManager.CurrentLocation")]
		public DeploymentLocationSettings CurrentLocation { get { return _CurrentLocation; } }

		public NameValueCollection AppSettings { get { return _AppSettings; } }

		public NameValueCollection ConnectionStrings { get { return _ConnectionStrings; } }

		#endregion

		#region Constructors

		internal DeploymentLocationsManagerInternal()
		{
			_CurrentHostName = DeploymentLocationsManager.GetCurrentHostNameChecked();
			DeploymentLocationsSection section = DeploymentLocationsManager.DeploymentLocationsSection;
			_AppSettings = new NameValueCollection(ConfigurationManager.AppSettings);
			_ConnectionStrings = DeploymentUtil.GetValueCollection(ConfigurationManager.ConnectionStrings);
			if(section == null)
			{
				//_CurrentLocation = new DeploymentLocationSettings();
			}
			else
			{
				_CurrentLocation = DeploymentLocationsManager.GetLocationByHostNameChecked(section, _CurrentHostName);
				if(_CurrentLocation != null)
				{
					_AppSettings = DeploymentUtil.CreateMergedCopy(DeploymentUtil.GetValueCollection(_CurrentLocation.AppSettings), _AppSettings);
					_ConnectionStrings = DeploymentUtil.CreateMergedCopy(DeploymentUtil.GetValueCollection(_CurrentLocation.ConnectionStrings), _ConnectionStrings);
				}
			}
		}

		#endregion

		#region Methods

		#endregion

	}

	#endregion


}
