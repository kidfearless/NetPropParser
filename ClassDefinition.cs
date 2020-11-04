using System;
using System.Collections.Generic;
using System.Text;

namespace NetPropParser
{
	[System.Diagnostics.DebuggerDisplay("{ClassName}")]
	class ClassDefinition
	{
		public string ClassName;
		public readonly List<string> BaseClasses = new List<string>();
		public readonly List<MemberDefinition> Members = new List<MemberDefinition>();
	}
}
