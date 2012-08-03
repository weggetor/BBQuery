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

using System.Data;
using Bitboxx.DNNModules.BBQuery.Components;

namespace Bitboxx.DNNModules.BBQuery
{

	/// ----------------------------------------------------------------------------- 
	/// <summary> 
	/// An abstract class for the data access layer 
	/// </summary> 
	/// <remarks> 
	/// </remarks> 
	/// <history> 
	/// </history> 
	/// ----------------------------------------------------------------------------- 
	public abstract class DataProvider
	{

		#region "Shared/Static Methods"

		/// <summary>
		/// singleton reference to the instantiated object 
		/// </summary>
		private static DataProvider objProvider = null;

		/// <summary>
		/// constructor
		/// </summary>
		static DataProvider()
		{
			CreateProvider();
		}

		/// <summary>
		/// dynamically create provider 
		/// </summary>
		private static void CreateProvider()
		{
			objProvider = (DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "Bitboxx.DNNModules.BBQuery", "");
		}

		/// <summary>
		/// return the provider 
		/// </summary>
		/// <returns></returns>
		public static DataProvider Instance()
		{
			return objProvider;
		}

		#endregion

		#region "Methods"

		public abstract IDataReader GetParameters(int tabModuleID, bool onlySearch);
		public abstract IDataReader GetParameter(int parameterID);
		public abstract void SaveParameter(int tabModuleID, ParameterInfo parameter);
		public abstract void DeleteParameter(int parameterID);

		#endregion

	}
}