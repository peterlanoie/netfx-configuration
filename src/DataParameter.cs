using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace Common.Configuration
{
	/// <summary>
	/// An abstraction of a data parameter, as in a parameter passed to a SQL query, function, or procs
	/// </summary>
	public class DataParameter : IDbDataParameter
	{
		#region Constructors

		/// <summary>
		/// Blank constructor.
		/// </summary>
		public DataParameter() { }


		/// <summary>
		/// Partial constructor.
		/// </summary>
		/// <param name="parameterName">Name of parameter.</param>
		/// <param name="dBType">DB data type.</param>
		/// <param name="value">Parameter value.</param>
		public DataParameter(string parameterName, DbType dBType, object value)
		{
			_parameterName = parameterName;
			_dbType = dBType;
			_value = value;
		}


		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="parameterName">Name of parameter.</param>
		/// <param name="dBType">DB data type.</param>
		/// <param name="value">Parameter value.</param>
		/// <param name="direction">Parameter direction.</param>
		public DataParameter(string parameterName, DbType dBType, object value, ParameterDirection direction)
		{
			_parameterName = parameterName;
			_dbType = dBType;
			_direction = direction;
			_value = value;
		}


		/// <summary>
		/// Full constructor.
		/// </summary>
		/// <param name="parameterName">Name of parameter.</param>
		/// <param name="dBType">DB data type.</param>
		/// <param name="value">Parameter value.</param>
		/// <param name="direction">Parameter direction.</param>
		/// <param name="size">Size of variable.</param>
		public DataParameter(string parameterName, DbType dBType, object value, ParameterDirection direction, int size)
		{
			_parameterName = parameterName;
			_dbType = dBType;
			_direction = direction;
			_size = size;
			_value = value;
		}

		#endregion Constructors

		#region IDataParameter Members

		private DbType _dbType = DbType.Int16;
		private ParameterDirection _direction = ParameterDirection.Input;
		private bool _isNullable = false;
		private string _parameterName = string.Empty;
		private byte _precision = 0;
		private byte _scale = 0;
		private int _size = 0;
		private string _sourceColumn = string.Empty;
		private DataRowVersion _sourceVersion = DataRowVersion.Current;
		private object _value = null;

		/// <summary>
		/// DB data type. Defaults to DbType.Int16.
		/// </summary>
		public DbType DbType
		{
			get { return _dbType; }
			set { _dbType = value; }
		}


		/// <summary>
		/// Direction of parameter. Defaults to ParameterDirection.Input.
		/// </summary>
		public ParameterDirection Direction
		{
			get { return _direction; }
			set { _direction = value; }
		}


		/// <summary>
		/// Indicates that parameter can be null.
		/// </summary>
		public bool IsNullable
		{
			get { return _isNullable; }
			set { _isNullable = value; }
		}


		/// <summary>
		/// Name of the parameter. Defaults to string.Empty.
		/// </summary>
		public string ParameterName
		{
			get { return _parameterName; }
			set { _parameterName = value; }
		}


		/// <summary>
		/// Indicates the precision of numeric parameters. Defaults to 0.
		/// </summary>
		public byte Precision
		{
			get { return _precision; }
			set { _precision = value; }
		}


		/// <summary>
		/// Indicates the scale of numeric parameters. Defaults to 0.
		/// </summary>
		public byte Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}


		/// <summary>
		/// The size of the parameter. Defaults to 0.
		/// </summary>
		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}


		/// <summary>
		/// Name of the source column. Defaults to string.Empty.
		/// </summary>
		public string SourceColumn
		{
			get { return _sourceColumn; }
			set { _sourceColumn = value; }
		}


		/// <summary>
		/// The data row version. Defaults to DataRowVersion.Current.
		/// </summary>
		public DataRowVersion SourceVersion
		{
			get { return _sourceVersion; }
			set { _sourceVersion = value; }
		}


		/// <summary>
		/// Parameter value. Defaults to null.
		/// </summary>
		public object Value
		{
			get { return _value; }
			set { _value = value; }
		}


		#endregion
	} //DataParameter

}
