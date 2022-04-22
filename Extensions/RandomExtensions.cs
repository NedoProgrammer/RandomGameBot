namespace RandomGameBot.Extensions;

public static class RandomExtensions
{
	public static Random New() => new(Guid.NewGuid().GetHashCode());

	public static T NextElement<T>(this Random random, IEnumerable<T> collection)
	{
		var enumerable = collection as T[] ?? collection.ToArray();
		return enumerable[random.Next(0, enumerable.Length)];
	}
}