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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Bitboxx.DNNModules.BBQuery.Components;
using DotNetNuke.Common.Utilities;

namespace Bitboxx.DNNModules.BBQuery
{

	/// ----------------------------------------------------------------------------- 
	/// <summary> 
	/// The Controller class for BBQuery 
	/// </summary> 
	/// <remarks> 
	/// </remarks> 
	/// <history> 
	/// </history> 
	/// ----------------------------------------------------------------------------- 

	[DNNtc.BusinessControllerClass()]
	public class BBQueryController
	{

		#region "Public Methods"

		public string GetSelect(string sqlCommand, List<ParameterInfo> parameters )
		{
			string where = "";
			int loop = 0;
			if (parameters.Count > 0)
			{
				foreach (ParameterInfo parameter in parameters)
				{
					loop++;
					if (loop == 1)
					{
						if (sqlCommand.ToLower().IndexOf(" WHERE ") > -1)
							where += " AND ";
						else
							where += " WHERE ";
					}
					else
					{
						where += parameter.CompareType + " ";
					}

					switch (parameter.DataType.ToLower())
					{
						case "string":
							switch (parameter.Comparer)
							{
								case 0:
									where += (string)parameter.FieldName + " LIKE '%' + @" + parameter.FieldName + " + '%' ";
									break;
								case 1:
									where += (string)parameter.FieldName + " LIKE @" + parameter.FieldName + " + '%' ";
									break;
								case 2:
									where += (string)parameter.FieldName + " LIKE '%' + @" + parameter.FieldName + " ";
									break;
							}
							break;
						case "integer":
						case "decimal":
						case "datetime":
							switch (parameter.Comparer)
							{
								case 0:
									where += (string)parameter.FieldName + " =  @" + parameter.FieldName + " ";
									break;
								case 1:
									where += (string)parameter.FieldName + " <  @" + parameter.FieldName + " ";
									break;
								case 2:
									where += (string)parameter.FieldName + " >  @" + parameter.FieldName + " ";
									break;
							}
							break;
						case "boolean":
							switch (parameter.Comparer)
							{
								case 0:
									where += (string)parameter.FieldName + " =  @" + parameter.FieldName + " ";
									break;
							}
							break;

					}
				}
			}
			return sqlCommand.Replace("{WHERE}", where);
		}

		public string ReplaceParameters(string sqlCommand, List<ParameterInfo> parameters)
		{
			if (parameters.Count > 0)
			{
				foreach (ParameterInfo parameter in parameters)
				{
					string value = "";
					switch (parameter.DataType.ToLower())
					{
							
						case "string":
						case "datetime":
							value = "'" + parameter.Value.ToString() + "'";
							break;
						case "integer":
						case "decimal":
							value = parameter.Value.ToString();
							break;
						case "boolean":
							value = (bool) parameter.Value ? "1" : "0";
							break;
					}
					sqlCommand = sqlCommand.Replace("@" + parameter.FieldName, value);
				}
			}
			return sqlCommand;
		}

		public DataTable BBQuerySelect(string provider, string connectionString, string sqlCommand, List<ParameterInfo> parameters)
		{
			return GenericDataAccess.BBQuerySelect(provider, connectionString, sqlCommand, parameters);
		}

		public DataTable BBQuerySelectById(string provider, string connectionString, string sqlCommand, string idField, object idValue)
		{
			return GenericDataAccess.BBQuerySelectById(provider, connectionString, sqlCommand, idField, idValue);
		}

		public void BBQueryInsert(string provider, string connectionstring, string sqlCommand, IOrderedDictionary values)
		{
			GenericDataAccess.BBQueryInsert(provider, connectionstring, sqlCommand, values);
		}

		public void BBQueryUpdate(string provider, string connectionstring, string sqlCommand, IOrderedDictionary keys, IOrderedDictionary newValues )
		{
			GenericDataAccess.BBQueryUpdate(provider, connectionstring, sqlCommand, keys, newValues);
		}

		public void BBQueryDelete(string provider, string connectionstring, string sqlCommand, IOrderedDictionary keys)
		{
			GenericDataAccess.BBQueryDelete(provider, connectionstring, sqlCommand, keys);
		}

		public List<ParameterInfo> GetParameters(int tabModuleID, bool onlySearch)
		{
			return CBO.FillCollection<ParameterInfo>(DataProvider.Instance().GetParameters(tabModuleID,onlySearch));
		}

		public ParameterInfo GetParameter(int parameterID)
		{

			return (ParameterInfo)CBO.FillObject(DataProvider.Instance().GetParameter(parameterID), typeof(ParameterInfo));
		}

		public void SaveParameter(int tabModuleID, ParameterInfo parameter)
		{
			DataProvider.Instance().SaveParameter(tabModuleID, parameter);
		}

		public void DeleteParameter(int parameterID)
		{
			DataProvider.Instance().DeleteParameter(parameterID);
		}

		#endregion
	}
}