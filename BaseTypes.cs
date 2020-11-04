using System;
using System.Collections.Generic;
using System.Text;

namespace NetPropParser
{
	public class Types
	{
		public static string GetType(string input)
		{
			switch(input)
			{
				case "integer": return  "int";
				case "float": return "float";
				case "string": return "string";
				case "vector":
				case "3":
					return  "Vector3";
				default: return "object";
			}
		}
		public readonly static Dictionary<string, string> Values = new Dictionary<string, string>()
		{
			{"integer", "int" },
			{"float", "float" },
			{"string", "string" },
			{"vector", "Vector3" },
			{"array", "Array" },
			{"3", "Vector3" },

		};
	}
}
