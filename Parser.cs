using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NetPropParser
{
	class Parser
	{
		static readonly Regex ClassDefRegex = new Regex(@"^\w+ \(type (\w+)\)$", RegexOptions.Compiled);
		static readonly Regex InheritanceRegex = new Regex(@"^ Table: baseclass \(offset 0\) \(type (\w+)\)$", RegexOptions.Compiled);
		static readonly Regex MemberRegex = new Regex(@"^ Member: (\w+) \(offset (\d+)\) \(type (\w+)\) \(bits (\d+)\) \(([\w|]*)\)$", RegexOptions.Compiled);
		static readonly Regex TableRegex = new Regex(@"^ Table: (\w+) \(offset (\d+)\) \(type (\w+)\)$", RegexOptions.Compiled);
		static List<ClassDefinition> Classes;
		static ClassDefinition CurrentClass;
		public static List<ClassDefinition> Parse(List<string> input)
		{
			Classes = new List<ClassDefinition>();
			var lines = input.GetRange(0, input.Count);
			while (lines.Count > 0)
			{

				var match = ClassDefRegex.Match(lines[0]);

				if(match.Success)
				{
					CurrentClass = new ClassDefinition()
					{
						ClassName = match.Groups[1].Value
					};
					lines.RemoveAt(0);
					ParseClass(lines);
					Classes.Add(CurrentClass);
				}
				else
				{
					lines.RemoveAt(0);
				}
			}
			return Classes;
		}

		private static void ParseClass(List<string> lines)
		{
			int endRange = lines.Count;
			#region find end of class
			for (int i = 0; i < lines.Count; i++)
			{
				if(lines[i][0] != ' ')
				{
					endRange = i;
					break;
				}
			}
			#endregion
			var matchLines = lines.GetRange(0, endRange);

			foreach (var line in matchLines)
			{
				var match = InheritanceRegex.Match(line);
				if (match.Success)
				{
					CurrentClass.BaseClasses.Add(match.Groups[1].Value);

					match = InheritanceRegex.Match(line);
					continue;
				}

				match = MemberRegex.Match(line);
				if(match.Success)
				{
					var first = CurrentClass.Members.FirstOrDefault(t => t.Name == match.Groups[1].Value);
					if(first == null)
					{
						CurrentClass.Members.Add(new MemberDefinition(match));
					}
					else if(first.Bits == 0)
					{
						CurrentClass.Members.Remove(first);
						CurrentClass.Members.Add(new MemberDefinition(match));
					}
					continue;
				}

				match = TableRegex.Match(line);
				if(match.Success)
				{
					CurrentClass.Members.Add(new MemberDefinition()
					{
						Name = match.Groups[1].Value,
						Offset = Convert.ToInt32(match.Groups[2].Value),
						Type = match.Groups[3].Value,
					});
				}
			}

			lines.RemoveRange(0, endRange);
		}
		
		public static string Serialize(List<ClassDefinition> classes)
		{
			string output = "";
			foreach (var c in classes)
			{
				output += $"\npublic class {c.ClassName}";
				if(c.BaseClasses.Count > 0)
				{
					output += " : ";
				}
				foreach (var baseClass in c.BaseClasses)
				{
					output += baseClass + ",";
				}
				// remove trailing comma
				if (c.BaseClasses.Count > 0)
				{
					output = output.Remove(output.Length - 1);
				}
				output += "\n{\n";
				foreach (var member in c.Members)
				{
					if(member.Type == "array")
					{
						continue;
					}
					string sign = (member.Flags.Contains("Unsigned") && member.Type != "float") ? "u" : "";
					string type = Types.GetType(member.Type);

					output += $"\tpublic {sign}{type} {member.Name} " + "{ get; set; }\n";
				}
				output += "}\n";
			}
			return output;
		}
	}
}
