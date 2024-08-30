namespace OpenShock.VoiceRecognizer.UI.ViewModels.Numbers;

public class FloatInputViewModel(
	string title,
	float initialValue,
	float minValue = float.MinValue,
	float maxValue = float.MaxValue,
	double increment = 0.1d
) : BaseNumberInputViewModel<float>(title, initialValue, minValue, maxValue, increment)
{

}
