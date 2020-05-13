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

	#region DeploymentLocationSettings

	///<summary>
	/// Configuration for a DeploymentLocation, identified by host names,
	/// containing configuration items specific to the DeploymentLocation.
	///</summary>
	public class DeploymentLocationSettings : ConfigurationElement
	{

		#region Static / Constant

		private static readonly ConfigurationProperty _NameProperty = new ConfigurationProperty("name", typeof(string), String.Empty, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
		private static readonly ConfigurationProperty _HostsProperty = new ConfigurationProperty("hosts", typeof(DeploymentLocationHostCollection));
		private static readonly ConfigurationProperty _AppSettingsProperty = new ConfigurationProperty("appSettings", typeof(KeyValueConfigurationCollection));
		private static readonly ConfigurationProperty _ConnectionStringsProperty = new ConfigurationProperty("connectionStrings", typeof(ConnectionStringSettingsCollection));
		private static readonly ConfigurationPropertyCollection _Properties = new ConfigurationPropertyCollection();

		static DeploymentLocationSettings()
		{
			_Properties.Add(_NameProperty);
			_Properties.Add(_HostsProperty);
			_Properties.Add(_AppSettingsProperty);
			_Properties.Add(_ConnectionStringsProperty);
		}

		#endregion

		#region Fields

		#endregion

		#region Properties

		/// <summary>
		/// Gets the collection of properties.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Configuration.ConfigurationPropertyCollection"/> of properties for the element.
		/// </returns>
		protected override ConfigurationPropertyCollection Properties { get { return _Properties; } }

		///<summary>
		/// The name of the DeploymentLocation (i.e. "dev", "stage", "prod", "qa", etc)
		///</summary>
		[ConfigurationProperty("name", DefaultValue = "", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
		public string Name
		{
			get { return (string)this[_NameProperty]; }
			set { this[_NameProperty] = value; }
		}

		///<summary>
		/// Configured hosts, used to identify the DeploymentLocation
		///</summary>
		[ConfigurationProperty("hosts")]
		public DeploymentLocationHostCollection Hosts
		{
			get { return (DeploymentLocationHostCollection)this[_HostsProperty]; }
		}

		internal IEnumerable<string> HostNames
		{
			get
			{
				DeploymentLocationHostCollection hosts = this.Hosts;
				if(hosts == null)
					yield break;
				foreach(DeploymentLocationHost host in hosts)
					yield return DeploymentUtil.PrepareHostName(host.Name);
			}
		}

		///<summary>
		/// AppSetting configurations specific to the DeploymentLocation
		///</summary>
		[ConfigurationProperty("appSettings")]
		public KeyValueConfigurationCollection AppSettings
		{
			get { return (KeyValueConfigurationCollection)this[_AppSettingsProperty]; }
		}

		///<summary>
		/// ConnectionString configurations specific to the DeploymentLocation
		///</summary>
		[ConfigurationProperty("connectionStrings")]
		public ConnectionStringSettingsCollection ConnectionStrings
		{
			get { return (ConnectionStringSettingsCollection)this[_ConnectionStringsProperty]; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal bool MatchesHostName(string hostName)
		{
			//foreach(DeploymentLocationHost host in this.Hosts)
			//{
			//    if(host.MatchesHostName(hostName))
			//        return true;
			//}
			//return false;
			foreach(string name in this.HostNames)
			{
				if(DeploymentUtil.HostNameEquals(hostName, name))
					return true;
			}
			return false;
		}

		#endregion

	}

	#endregion

}
