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

	#region DeploymentLocationHostCollection

	///<summary>
	/// A collection of DeploymentLocationHost elements
	///</summary>
	public class DeploymentLocationHostCollection : ConfigurationElementCollection
	{

		#region Static / Constant

		internal const string CollectionElementName = "hosts";

		private static readonly ConfigurationPropertyCollection _Properties = new ConfigurationPropertyCollection();

		#endregion

		#region Fields

		#endregion

		#region Properties

		/// <summary>
		/// Gets the name used to identify this collection of elements in the configuration file
		/// </summary>
		/// <returns>
		/// The name of the collection
		/// </returns>
		protected override string ElementName { get { return CollectionElementName; } }

		/// <summary>
		/// Gets the collection of properties.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Configuration.ConfigurationPropertyCollection"/> of properties for the element.
		/// </returns>
		protected override ConfigurationPropertyCollection Properties { get { return _Properties; } }

		///<summary>
		/// Get a DeploymentLocationHost element by index
		///</summary>
		///<param name="index"></param>
		public DeploymentLocationHost this[int index]
		{
			get { return (DeploymentLocationHost)BaseGet(index); }
			set
			{
				if(BaseGet(index) != null)
					BaseRemoveAt(index);
				BaseAdd(index, value);
			}
		}

		///<summary>
		/// Get a DeploymentLocationHost element by name
		///</summary>
		///<param name="name"></param>
		public new DeploymentLocationHost this[string name]
		{
			get { return (DeploymentLocationHost)BaseGet(name); }
		}

		#endregion

		#region Constructors

		#endregion

		#region Methods

		/// <summary>
		/// Create a new DeploymentLocationHost element
		/// </summary>
		/// <returns></returns>
		protected override ConfigurationElement CreateNewElement()
		{
			return new DeploymentLocationHost();
		}

		/// <summary>
		/// Get the key of a DeploymentLocation host element (its name)
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DeploymentLocationHost)element).Name;
		}

		#endregion

	}

	#endregion

}
