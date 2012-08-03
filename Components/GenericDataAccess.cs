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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;

namespace Bitboxx.DNNModules.BBQuery.Components
{
	public static class GenericDataAccess
	{
		public static DataTable BBQuerySelect(string provider, string connectionString, string sqlCommand, List<ParameterInfo> parameters)
		{
			DataTable dt = new DataTable();
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
				DbConnection conn = factory.CreateConnection();
				if (conn != null)
				{
					conn.ConnectionString = connectionString;
					using (conn)
					{
						try
						{
							// Create the command.
							DbCommand command = conn.CreateCommand();
							command.CommandText = sqlCommand;
							command.CommandType = CommandType.Text;

							foreach (ParameterInfo parameter in parameters)
							{
								DbParameter para = factory.CreateParameter();
								para.ParameterName = parameter.FieldName;
								para.Value = parameter.Value;
								command.Parameters.Add(para);
							}

							// Open the connection.
							conn.Open();

							// Retrieve the data.
							DbDataReader reader = command.ExecuteReader();
							dt.Load(reader);
							conn.Close();
						}
						catch (Exception)
						{
							throw;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return dt;
		}

		public static DataTable BBQuerySelectById(string provider, string connectionString, string sqlCommand, string idField, object idValue)
		{
			DataTable dt = new DataTable();
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
				DbConnection conn = factory.CreateConnection();
				if (conn != null)
				{
					conn.ConnectionString = connectionString;
					using (conn)
					{
						try
						{
							// Create the command.
							DbCommand command = conn.CreateCommand();
							string sqlSingleRecord = sqlCommand +
							                         (sqlCommand.ToUpper().IndexOf(" WHERE") > -1 ? " AND " : " WHERE ") +
							                         idField + "=@id";


							command.CommandText = sqlSingleRecord;
							command.CommandType = CommandType.Text;
							DbParameter para = factory.CreateParameter();
							para.ParameterName = "id";
							para.Value = idValue;
							command.Parameters.Add(para);

							// Open the connection.
							conn.Open();

							// Retrieve the data.
							DbDataReader reader = command.ExecuteReader();
							dt.Load(reader);
							conn.Close();
						}
						catch (Exception)
						{
							throw;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			return dt;
		}

		public static void BBQueryInsert(string provider, string connectionString, string sqlCommand, IOrderedDictionary values)
		{
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
				DbConnection conn = factory.CreateConnection();
				if (conn != null)
				{
					conn.ConnectionString = connectionString;
					using (conn)
					{
						try
						{
							// Create the command.
							DbCommand command = conn.CreateCommand();

							command.CommandText = sqlCommand;
							command.CommandType = CommandType.Text;

							foreach (DictionaryEntry newValue in values)
							{
								DbParameter para = factory.CreateParameter();
								para.ParameterName = (string)newValue.Key;
								para.Value = newValue.Value ?? DBNull.Value;
								command.Parameters.Add(para);
							}

							// Open the connection.
							conn.Open();

							// Retrieve the data.
							command.ExecuteNonQuery();
							conn.Close();
						}
						catch (Exception)
						{
							throw;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}

		}
		public static void BBQueryUpdate(string provider, string connectionString, string sqlCommand, IOrderedDictionary keys, IOrderedDictionary newValues)
		{
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
				DbConnection conn = factory.CreateConnection();
				if (conn != null)
				{
					conn.ConnectionString = connectionString;
					using (conn)
					{
						try
						{
							// Create the command.
							DbCommand command = conn.CreateCommand();

							command.CommandText = sqlCommand;
							command.CommandType = CommandType.Text;

							foreach (DictionaryEntry newValue in newValues)
							{
								DbParameter para = factory.CreateParameter();
								para.ParameterName = (string)newValue.Key;
								para.Value = newValue.Value ?? DBNull.Value;
								command.Parameters.Add(para);	
							}

							foreach (DictionaryEntry key in keys)
							{
								DbParameter para = factory.CreateParameter();
								para.ParameterName = (string)key.Key;
								para.Value = key.Value ?? DBNull.Value;
								command.Parameters.Add(para);
							}
							// Open the connection.
							conn.Open();

							// Retrieve the data.
							command.ExecuteNonQuery();
							conn.Close();
						}
						catch (Exception)
						{
							throw;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
			
		}
		public static void BBQueryDelete(string provider, string connectionString, string sqlCommand, IOrderedDictionary keys)
		{
			try
			{
				DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
				DbConnection conn = factory.CreateConnection();
				if (conn != null)
				{
					conn.ConnectionString = connectionString;
					using (conn)
					{
						try
						{
							// Create the command.
							DbCommand command = conn.CreateCommand();

							command.CommandText = sqlCommand;
							command.CommandType = CommandType.Text;

							foreach (DictionaryEntry key in keys)
							{
								DbParameter para = factory.CreateParameter();
								para.ParameterName = (string)key.Key;
								para.Value = key.Value ?? DBNull.Value;
								command.Parameters.Add(para);
							}
							// Open the connection.
							conn.Open();

							// Retrieve the data.
							command.ExecuteNonQuery();
							conn.Close();
						}
						catch (Exception)
						{
							throw;
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}

		}
	}
}