//
//  Copyright (c) 2012 bitboxx solutions Torsten Weggen
//  http://www.bitboxx.net
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions 
//  of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.
//


using System;
using System.Data;
using System.Data.SqlClient;
using Bitboxx.DNNModules.BBQuery.Components;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Bitboxx.DNNModules.BBQuery
{

	/// ----------------------------------------------------------------------------- 
	/// <summary> 
	/// SQL Server implementation of the abstract DataProvider class 
	/// </summary> 
	/// <remarks> 
	/// </remarks> 
	/// <history> 
	/// </history> 
	/// ----------------------------------------------------------------------------- 
	public class SqlDataProvider : DataProvider
	{


		#region "Private Members"

		private const string ProviderType = "data";
		private const string ModuleQualifier = "BBQuery_";

		private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string _connectionString;
		private string _providerPath;
		private string _objectQualifier;
		private string _databaseOwner;

		#endregion

		#region "Constructors"

		public SqlDataProvider()
		{

			// Read the configuration specific information for this provider 
			Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

			// Read the attributes for this provider 

			//Get Connection string from web.config 
			_connectionString = Config.GetConnectionString();

			if (_connectionString == "")
			{
				// Use connection string specified in provider 
				_connectionString = objProvider.Attributes["connectionString"];
			}

			_providerPath = objProvider.Attributes["providerPath"];

			_objectQualifier = objProvider.Attributes["objectQualifier"];
			if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false)
			{
				_objectQualifier += "_";
			}

			_databaseOwner = objProvider.Attributes["databaseOwner"];
			if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false)
			{
				_databaseOwner += ".";
			}

		}

		#endregion

		#region "Properties"
		public string ConnectionString
		{
			get { return _connectionString; }
		}
		public string ProviderPath
		{
			get { return _providerPath; }
		}
		public string ObjectQualifier
		{
			get { return _objectQualifier; }
		}
		public string DatabaseOwner
		{
			get { return _databaseOwner; }
		}
		public string Prefix
		{
			get { return _databaseOwner + _objectQualifier + ModuleQualifier; }
		}
		#endregion

		#region "Private Methods"

		private string GetFullyQualifiedName(string name)
		{
			return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
		}
		private object GetNull(object Field)
		{
			return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
		}
		#endregion

		#region "Public Methods"

		public override IDataReader GetParameters(int tabModuleID, bool onlySearch)
		{
			string sqlCmd = "SELECT * FROM " + GetFullyQualifiedName("Parameters") + " WHERE TabModuleID = @TabModuleID";
			if (onlySearch) sqlCmd += " AND ShowInSearch = 1";

			return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sqlCmd,
			                               new SqlParameter("TabModuleId", tabModuleID));
		}
		public override IDataReader GetParameter(int parameterID)
		{
			string sqlCmd = "SELECT * FROM " + GetFullyQualifiedName("Parameters") + " WHERE ParameterID = @ParameterID";
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sqlCmd,
										   new SqlParameter("ParameterID", parameterID));
		}

		public override void SaveParameter(int tabModuleID, ParameterInfo parameter)
		{
			string sqlCmd;
			bool isNew = true;
			SqlParameter[] sqlParams;
			if (parameter.ParameterID > 0)
			{
				sqlCmd = "SELECT ParameterID FROM " + GetFullyQualifiedName("Parameters") +
				         " WHERE TabModuleID = @TabmoduleID AND ParameterID = @ParameterID";

				sqlParams = new SqlParameter[]
				            	{
				            		new SqlParameter("TabModuleId", tabModuleID),
				            		new SqlParameter("ParameterId", parameter.ParameterID),
				            	};
				object paramId = SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, sqlCmd, sqlParams);
				if (paramId != null && (int)paramId == parameter.ParameterID)
					isNew = false;
			}

			if (isNew)
			{
				sqlCmd = "INSERT INTO " + GetFullyQualifiedName("Parameters") +
				         "(TabModuleId,FieldName,DataType,ShowInSearch) VALUES " +
				         "(@TabModuleId,@FieldName,@DataType,@ShowInSearch)";
			}
			else
			{
				sqlCmd = "UPDATE " + GetFullyQualifiedName("Parameters") + " SET " +
				         " TabModuleId = @TabModuleId," +
				         " FieldName = @FieldName," +
				         " DataType = @DataType," +
				         " ShowInSearch = @ShowInSearch" +
				         " WHERE ParameterId = @ParameterId";

			}
			sqlParams = new SqlParameter[]
			            	{
			            		new SqlParameter("TabModuleId", tabModuleID),
			            		new SqlParameter("ParameterId", parameter.ParameterID),
			            		new SqlParameter("FieldName", parameter.FieldName),
			            		new SqlParameter("DataType", parameter.DataType),
			            		new SqlParameter("ShowInSearch", parameter.ShowInSearch)
			            	};
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sqlCmd, sqlParams);
		}

		public override void DeleteParameter(int parameterID)
		{
			string sqlCmd = "DELETE FROM " + GetFullyQualifiedName("Parameters") + " WHERE ParameterID = @ParameterID";
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sqlCmd, new SqlParameter("ParameterID", parameterID));
		}
		#endregion

	}
}