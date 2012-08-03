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

namespace Bitboxx.DNNModules.BBQuery.Components
{
	[Serializable]
	public class ParameterInfo
	{
		public int ParameterID { get; set; }
		public int TabModuleID { get; set; }
		public string FieldName { get; set; }
		public string DataType { get; set; }
		public bool ShowInSearch { get; set; }
		public object Value { get; set; }
		public int Comparer { get; set; }
		public string CompareType { get; set; }

		public ParameterInfo()
		{
		}
		public ParameterInfo(int parameterID, int tabModuleId, string fieldName, string dataType, bool showInSearch)
		{
			ParameterID = parameterID;
			TabModuleID = tabModuleId;
			FieldName = fieldName;
			DataType = dataType;
			ShowInSearch = showInSearch;
		}

		public static object GetTypedValue(string strValue, string dataType)
		{
			switch (dataType.ToLower())
			{
				case "string":
					if (strValue != String.Empty)
					{
						return strValue;
					}
					return null;
				case "integer":
					int iSearch = 0;
					if (strValue != String.Empty && Int32.TryParse(strValue, out iSearch))
					{
						return iSearch;
					}
					return null;
				case "decimal":
					decimal dSearch = 0.00m;
					if (strValue != String.Empty && Decimal.TryParse(strValue, out dSearch))
					{
						return dSearch;
					}
					return null;
				case "boolean":
					if (strValue == "1" || strValue.ToLower() == "true")
						return true;
					return false;
				default:
					return null;

			}
		}
	}

}