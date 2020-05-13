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

	#region DeploymentLocationSettingsCollection

	///<summary>
	/// A collection of DeploymentLocationSettings elements
	///</summary>
	public class DeploymentLocationSettingsCollection : ConfigurationElementCollection
	{

		#region Static / Constant

		private static readonly ConfigurationPropertyCollection _Properties = new ConfigurationPropertyCollection();

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
		/// Get a DeploymentLocationSettings element by index
		///</summary>
		///<param name="index"></param>
		public DeploymentLocationSettings this[int index]
		{
			get { return (DeploymentLocationSettings) BaseGet(index); }
			set
			{
				if(BaseGet(index) != null)
					BaseRemoveAt(index);
				BaseAdd(index, value);
			}
		}

		///<summary>
		/// Get a DeploymentLocationSettings element by name
		///</summary>
		///<param name="name"></param>
		public new DeploymentLocationSettings this[string name]
		{
			get { return (DeploymentLocationSettings) BaseGet(name); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		/// <summary>
		/// Create a new DeploymentLocationSettings element
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new DeploymentLocationSettings();
		}

		/// <summary>
		/// Get the key of a DeploymentLocationSettings element (its name)
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DeploymentLocationSettings)element).Name;
		}

		#endregion

	}

	#endregion

}
