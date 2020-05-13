using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web.Security;
using System.Configuration.Provider;

namespace Common.Configuration.Deployment.Providers
{

	#region DeploymentLocationSqlMembershipProvider

	/// <summary>
	/// 
	/// </summary>
	public class DeploymentLocationSqlMembershipProvider : SqlMembershipProvider
	{

		#region Static / Constant

		#endregion

		#region Fields

		//private string _Name;

		#endregion

		#region Properties

		//public override string Name { get { return _Name; } }

		#endregion

		#region Constructors

		#endregion

		#region Methods

		//public override void Initialize(string name, NameValueCollection config)
		//{
		//    if(config == null)
		//        throw new ArgumentNullException("config");
		//    if(String.IsNullOrEmpty(name))
		//        name = "DeploymentLocationSqlMembershipProvider";
		//    _Name = name;
		//    if(String.IsNullOrEmpty(config["description"]))
		//    {
		//        config.Remove("description");
		//        config.Add("description", "Deployment location SQL membership provider");
		//    }
		//    //base.Initialize(name, config);
		//    throw new NotImplementedException();
		//}

		//public override void Initialize(string name, NameValueCollection config)
		//{
		//    if(config == null)
		//        throw new ArgumentNullException("config");
		//    string connectionStringName = config["connectionStringName"];
		//    string connectionString = DeploymentProviderUtil.SafeGetDeploymentLocationConnectionStringName(connectionStringName);
		//    if(connectionString != null)
		//        config["connectionStringName"] = "DUMMY_CONNECTION_STRING";
		//    base.Initialize(name, config);
		//    if(connectionString != null)
		//        DeploymentProviderUtil.SetConnectionString(this, connectionString);
		//}

		//public override void Initialize(string name, NameValueCollection config)
		//{
		//    if(config == null)
		//        throw new ArgumentNullException("config");
		//    string connectionStringName = config["connectionStringName"];
		//    base.Initialize(name, config);
		//    if(!String.IsNullOrEmpty(connectionStringName))
		//    {
		//        ConnectionStringSettings connectionStringSettings = DeploymentLocationsManager.CurrentConnectionStrings[connectionStringName];
		//        if(connectionStringSettings != null && !String.IsNullOrEmpty(connectionStringSettings.ConnectionString))
		//            DeploymentProviderUtil.SetConnectionString(this, connectionStringSettings.ConnectionString);
		//    }
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="config"></param>
		public override void Initialize(string name, NameValueCollection config)
		{
			DeploymentProviderUtil.PerformConnectionOverrideInitialize(this, base.Initialize, name, config);
		}

		#endregion

	}

	#endregion

}
