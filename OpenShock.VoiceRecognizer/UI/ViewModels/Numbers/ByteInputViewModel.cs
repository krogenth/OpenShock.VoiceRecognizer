namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class ByteInputViewModel(
	string title,
	byte initialValue,
	byte minValue = byte.MinValue,
	byte maxValue = byte.MaxValue,
	double increment = 1.0d
) : BaseNumberInputViewModel<byte>(title, initialValue, minValue, maxValue, increment)
{

}
