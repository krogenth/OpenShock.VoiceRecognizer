namespace OpenShock.VoiceRecognizer.Common;

public class ReactiveObject<T>(T defaultValue)
{
	private readonly ReaderWriterLock _readerWriterLock = new();
	private bool _isInitialized = false;
	private T _value = defaultValue;

	public event EventHandler<ValueChangedEventArgs<T>>? ValueChanged;

	public T Value
	{
		get
		{
			_readerWriterLock.AcquireReaderLock(Timeout.Infinite);
			T value = _value;
			_readerWriterLock.ReleaseReaderLock();

			return value;
		}
		set
		{
			_readerWriterLock.AcquireWriterLock(Timeout.Infinite);

			T oldValue = _value;

			bool oldIsInitialized = _isInitialized;

			_isInitialized = true;
			_value = value;

			_readerWriterLock.ReleaseWriterLock();

			if (!oldIsInitialized || oldValue == null || !oldValue.Equals(_value))
			{
				ValueChanged?.Invoke(this, new ValueChangedEventArgs<T>(oldValue, value));
			}
		}
	}

	public static implicit operator T(ReactiveObject<T> obj)
	{
		return obj.Value;
	}
}

public class ValueChangedEventArgs<T>(T oldValue, T newValue)
{
	public T OldValue { get; } = oldValue;
	public T NewValue { get; } = newValue;
}
