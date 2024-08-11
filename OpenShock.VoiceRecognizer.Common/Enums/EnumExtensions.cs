using System.Numerics;

namespace OpenShock.VoiceRecognizer.Common.Enums;

public static class EnumExtensions
{
	public static T ToEnum<T>(this string enumString)
		where T : struct, Enum =>
		(T)Enum.Parse(typeof(T), enumString);

	public static bool EnumExists<T>(this byte enumValue)
		where T : struct, Enum =>
		Enum.IsDefined(typeof(T), enumValue);

	public static T ToEnum<T>(this byte enumValue)
		where T : struct, Enum
	{
		if (Enum.IsDefined(typeof(T), enumValue))
		{
			return (T)(object)enumValue;
		}

		throw new ArgumentOutOfRangeException(enumValue.ToString());
	}

	public static TType ToNumeric<TType, TEnum>(this TEnum value)
		where TType : IBinaryInteger<TType>, IComparable
		where TEnum : struct, Enum, TType =>
		(TType)(object)value;

	public static TEnum ToEnum<TType, TEnum>(this TType value)
		where TType : IBinaryInteger<TType>, IComparable
		where TEnum : struct, Enum, TType =>
		(TEnum)(object)value;

	public static IList<TEnum> GetEnumList<TEnum>()
		where TEnum : struct, Enum =>
		Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
}
