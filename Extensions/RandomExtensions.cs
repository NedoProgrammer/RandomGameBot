namespace RandomGameBot.Extensions;

/// <summary>
/// Extensions to the <see cref="Random"/> class.
/// </summary>
public static class RandomExtensions
{
	/// <summary>
	/// Create a new seeded random.
	/// </summary>
	/// <returns>A Random object.</returns>
	public static Random New()
	{
		return new(Guid.NewGuid().GetHashCode());
	}

	/// <summary>
	/// Select a random element from a list.
	/// </summary>
	/// <param name="random">Current random.</param>
	/// <param name="collection">Collection of items.</param>
	/// <typeparam name="T">Item type.</typeparam>
	/// <returns>A random element with type T from the collection.</returns>
	public static T NextElement<T>(this Random random, IEnumerable<T> collection)
	{
		var enumerable = collection as T[] ?? collection.ToArray();
		return enumerable[random.Next(0, enumerable.Length)];
	}
}