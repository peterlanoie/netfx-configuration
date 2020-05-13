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

	#region DeploymentLocationSqlRoleProvider

	/// <summary>
	/// 
	/// </summary>
	public class DeploymentLocationSqlRoleProvider : SqlRoleProvider
	{

		#region Static / Constant

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

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
