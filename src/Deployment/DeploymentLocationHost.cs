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

	#region DeploymentLocationHost

	///<summary>
	/// A host name used to identify a DeploymentLocation
	///</summary>
	public class DeploymentLocationHost : ConfigurationElement
	{

		#region Static / Constant

		private static readonly ConfigurationProperty _NameProperty = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
		private static readonly ConfigurationPropertyCollection _Properties = new ConfigurationPropertyCollection();

		static DeploymentLocationHost()
		{
			_Properties.Add(_NameProperty);
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
		/// The SERVER_NAME or MachineName
		///</summary>
		[ConfigurationProperty("name", DefaultValue = "", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
		public string Name
		{
			get { return (string)this[_NameProperty]; }
			set { this[_NameProperty] = value; }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		internal bool MatchesHostName(string hostName)
		{
			return DeploymentUtil.HostNameEquals(hostName, this.Name);
		}

		#endregion

	}

	#endregion

}
