using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.Management;

namespace Common.Configuration.Deployment.Providers
{

	#region DeploymentLocationSqlWebEventProvider

	/// <summary>
	/// This class does something good
	/// </summary>
	public class DeploymentLocationSqlWebEventProvider : SqlWebEventProvider
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
