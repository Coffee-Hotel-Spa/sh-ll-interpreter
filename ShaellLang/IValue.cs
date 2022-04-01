using System.Collections.Generic;
using System.Collections;

namespace ShaellLang
{
	public interface IValue
	{
		bool ToBool();

		Number ToNumber();

		IFunction ToFunction();

		SString ToSString();

		ITable ToTable();

		JobObject ToJobObject();

		bool IsEqual(IValue other);

		string GetTypeName();
	}
}