using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.EventHandling;

namespace RandomGameBot.Core;

/// <summary>
/// I am not going to comment this.
/// This is <see cref="PaginationRequest"/>, but with a public <see cref="Result"/>
/// field, which is accessible after the request is done.
/// <remarks>Used for Features.ObjectionLol commands to determine on which page did the user stop.</remarks>
/// </summary>
public class CustomPaginationRequest : IPaginationRequest
{
	private readonly PaginationBehaviour _behaviour;
	private readonly CancellationTokenSource _ct;
	private readonly PaginationEmojis _emojis;
	private readonly DiscordMessage _message;
	private readonly List<Page> _pages;
	private readonly DiscordUser _user;
	private int _index;
	private TaskCompletionSource<bool> _tcs;
	private TimeSpan _timeout;

	public event Action<int> Stopped;

    /// <summary>
    ///     Creates a new Pagination request
    /// </summary>
    /// <param name="message">Message to paginate</param>
    /// <param name="user">User to allow control for</param>
    /// <param name="behaviour">Behaviour during pagination</param>
    /// <param name="deletion">Behavior on pagination end</param>
    /// <param name="emojis">Emojis for this pagination object</param>
    /// <param name="timeout">Timeout time</param>
    /// <param name="pages">Pagination pages</param>
    internal CustomPaginationRequest(DiscordMessage message, DiscordUser user, PaginationBehaviour behaviour,
		PaginationDeletion deletion,
		PaginationEmojis emojis, TimeSpan timeout, params Page[] pages)
	{
		_tcs = new TaskCompletionSource<bool>();
		_ct = new CancellationTokenSource(timeout);
		_ct.Token.Register(() => _tcs.TrySetResult(true));
		_timeout = timeout;

		_message = message;
		_user = user;

		PaginationDeletion = deletion;
		_behaviour = behaviour;
		_emojis = emojis;

		_pages = new List<Page>();
		foreach (var p in pages) _pages.Add(p);
	}

	public PaginationDeletion PaginationDeletion { get; }

	public int PageCount => _pages.Count;

	public async Task<Page> GetPageAsync()
	{
		await Task.Yield();

		return _pages[_index];
	}

	public async Task SkipLeftAsync()
	{
		await Task.Yield();

		_index = 0;
	}

	public async Task SkipRightAsync()
	{
		await Task.Yield();

		_index = _pages.Count - 1;
	}

	public async Task NextPageAsync()
	{
		await Task.Yield();

		switch (_behaviour)
		{
			case PaginationBehaviour.Ignore:
				if (_index == _pages.Count - 1)
					break;
				_index++;

				break;

			case PaginationBehaviour.WrapAround:
				if (_index == _pages.Count - 1)
					_index = 0;
				else
					_index++;

				break;
		}
	}

	public async Task PreviousPageAsync()
	{
		await Task.Yield();

		switch (_behaviour)
		{
			case PaginationBehaviour.Ignore:
				if (_index == 0)
					break;
				_index--;

				break;

			case PaginationBehaviour.WrapAround:
				if (_index == 0)
					_index = _pages.Count - 1;
				else
					_index--;

				break;
		}
	}

	public async Task<PaginationEmojis> GetEmojisAsync()
	{
		await Task.Yield();

		return _emojis;
	}

	public Task<IEnumerable<DiscordButtonComponent>> GetButtonsAsync()
	{
		throw new NotSupportedException("This request does not support buttons.");
	}

	public async Task<DiscordMessage> GetMessageAsync()
	{
		await Task.Yield();

		return _message;
	}

	public async Task<DiscordUser> GetUserAsync()
	{
		await Task.Yield();

		return _user;
	}

	public int Result;
	public async Task DoCleanupAsync()
	{
		Result = _index;
		switch (PaginationDeletion)
		{
			case PaginationDeletion.DeleteEmojis:
				await _message.DeleteAllReactionsAsync().ConfigureAwait(false);
				break;

			case PaginationDeletion.DeleteMessage:
				await _message.DeleteAsync().ConfigureAwait(false);
				break;

			case PaginationDeletion.KeepEmojis:
				break;
		}
		
	}

	public async Task<TaskCompletionSource<bool>> GetTaskCompletionSourceAsync()
	{
		await Task.Yield();

		return _tcs;
	}

	~CustomPaginationRequest()
	{
		Dispose();
	}

    /// <summary>
    ///     Disposes this PaginationRequest.
    /// </summary>
    public void Dispose()
	{
		_ct.Dispose();
		_tcs = null;
	}
}