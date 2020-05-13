using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Configuration
{
	/// <summary>
	/// Used to dynamically add params to sql commands from an xml source file
	/// </summary>
	public class ParamShell
	{
		#region Constructors

		/// <summary>
		/// Main constructor.
		/// </summary>
		/// <param name="name">parameter name</param>
		/// <param name="type">parameter type</param>
		/// <param name="direction">parameter direction</param>
		/// <param name="val">parameter value</param>
		public ParamShell(string name, string type, string direction, object val)
		{
			Name = name;
			Type = type;
			Direction = direction;
			Value = val;
		}


		#endregion Constructors

		#region Properties

		private string _direction = "";
		private string _name = "";
		private string _type = "";
		private object _value = new object();

		/// <summary>
		/// Parameter Direction. Defaults to "".
		/// </summary>
		public string Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}


		/// <summary>
		/// Parameter name. Defaults to "".
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}


		/// <summary>
		/// Parameter type. Defaults to "".
		/// </summary>
		public string Type
		{
			get { return _type; }
			set { _type = value; }
		}


		/// <summary>
		/// Parameter value. Defaults to new object().
		/// </summary>
		public object Value
		{
			get { return _value; }
			set { _value = value; }
		}


		#endregion Properties
	}

}
