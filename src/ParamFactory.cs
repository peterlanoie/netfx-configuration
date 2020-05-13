using System;
using System.Diagnostics;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Common.Configuration
{
	/// <summary>
	/// Makes a DataParameter from an XML configuration file
	/// </summary>
	public class ParamFactory
	{
		#region private constructor

		private ParamFactory() { }

		#endregion

		#region private static members

		// Used for thread queuing
		private static object MakeLock = new object();
		private static object MakeShellLock = new object();

		#endregion

		#region public static methods

		/// <summary>
		/// Creates a DataParameter from the ParamShell parameter.
		/// </summary>
		/// <param name="shell">A ParamShell to create from.</param>
		/// <returns>A new populated DataParameter object.</returns>
		public static DataParameter Make(ParamShell shell)
		{
			lock (MakeLock)
			{
				DataParameter sp = new DataParameter();
				try
				{
					sp.ParameterName = "@" + shell.Name;

					switch (shell.Type.ToUpper())
					{
						case "VARCHAR":
							sp.DbType = DbType.String;
							break;
						case "INT":
							sp.DbType = DbType.Int32;
							break;
						case "CHAR":
							sp.DbType = DbType.StringFixedLength;
							break;
						case "DATETIME":
							sp.DbType = DbType.DateTime;
							break;
					}

					switch (shell.Direction.ToUpper())
					{
						case "IN":
							sp.Direction = ParameterDirection.Input;
							break;
						case "OUT":
							sp.Direction = ParameterDirection.Output;
							break;
						case "INOUT":
							sp.Direction = ParameterDirection.InputOutput;
							break;
					}

					sp.Value = shell.Value;

					return sp;
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
					return null;
				}
			}
		}


		/// <summary>
		/// Creates a new ParamShell object from the xmlParamString parameter.
		/// </summary>
		/// <param name="xmlParamString">An XML string to parse properties from.</param>
		/// <returns>A new populated ParamShell object.</returns>
		public static ParamShell MakeShell(string xmlParamString)
		{
			lock (MakeShellLock)
			{
				if (xmlParamString == "")
					return null;

				XmlDocument xdoc = new XmlDocument();
				try
				{
					xdoc.LoadXml(xmlParamString);
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
					return null;
				}

				try
				{
					XmlElement paramName = (XmlElement)xdoc.SelectSingleNode("//name");
					XmlElement paramType = (XmlElement)xdoc.SelectSingleNode("//type");
					XmlElement paramDir = (XmlElement)xdoc.SelectSingleNode("//direction");
					XmlElement paramVal = (XmlElement)xdoc.SelectSingleNode("//value");
					return new ParamShell(paramName.InnerText, paramType.InnerText, paramDir.InnerText, paramVal.InnerText);
				}
				catch (Exception exp)
				{
					Trace.WriteLine(exp);
					return null;
				}
			}
		}


		#endregion Methods
	}
}
