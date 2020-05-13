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

	#region DeploymentLocationsSection

	///<summary>
	/// A configuration section that defines a set of DeploymentLocations,
	/// which provide specialized configuration items based on the location of
	/// the application.
	///</summary>
	public class DeploymentLocationsSection : ConfigurationSection
	{

		#region Static / Constant

		internal const string SectionElementName = "deploymentLocations";

		private static readonly ConfigurationProperty _LocationsProperty = new ConfigurationProperty("", typeof(DeploymentLocationSettingsCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
		private static readonly ConfigurationProperty _DefaultLocationProperty = new ConfigurationProperty("defaultLocation", typeof(string));
		private static readonly ConfigurationPropertyCollection _Properties = new ConfigurationPropertyCollection();

		static DeploymentLocationsSection()
		{
			_Properties.Add(_LocationsProperty);
			_Properties.Add(_DefaultLocationProperty);
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
		/// Configured DeploymentLocations
		///</summary>
		[ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public DeploymentLocationSettingsCollection Locations
		{
			get { return (DeploymentLocationSettingsCollection)this[_LocationsProperty]; }
		}

		///<summary>
		/// Name of the default DeploymentLocation, if any
		///</summary>
		[ConfigurationProperty("defaultLocation")]
		public string DefaultLocationName
		{
			get { return (string)this[_DefaultLocationProperty]; }
			set { this[_DefaultLocationProperty] = value; }
		}

		///<summary>
		/// The default DeploymentLocation, if any
		///</summary>
		public DeploymentLocationSettings DefaultLocation
		{
			get
			{
				string name = this.DefaultLocationName;
				return String.IsNullOrEmpty(name) ? null : this.Locations[name];
			}
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal DeploymentLocationSettings GetLocationByHostName(string hostName)
		{
			if(!String.IsNullOrEmpty(hostName))
			{
				foreach(DeploymentLocationSettings location in this.Locations)
				{
					if(location.MatchesHostName(hostName))
						return location;
				}
			}
			return this.DefaultLocation;
		}

		#endregion

	}

	#endregion

}
