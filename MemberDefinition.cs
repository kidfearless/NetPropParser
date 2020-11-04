using System;
using System.Text.RegularExpressions;

namespace NetPropParser
{
	[System.Diagnostics.DebuggerDisplay("{Name}: {Type}")]
	public class MemberDefinition
	{
		public string Name;
		public int Offset;
		public string Type;
		public int Bits;
		public string Flags;

		public MemberDefinition(Match match)
		{
			this.Name = match.Groups[1].Value;
			this.Offset = Convert.ToInt32(match.Groups[2].Value);
			this.Type = match.Groups[3].Value;
			this.Bits = Convert.ToInt32(match.Groups[4].Value);
			this.Flags = match.Groups[5].Value;
		}
		public MemberDefinition()
		{
			Name = "";
			Type = "";
			Flags = "";
		}
	}
}