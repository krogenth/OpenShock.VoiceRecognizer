using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Common.Enums;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.NGramRecognizer;

public static class NGramRecognizer
{
	public static WordRecognition? RecognizedNGram(string recognizedSpeech, ObservableCollection<WordRecognition> words)
	{
		WordRecognition? result = null;

		foreach (var word in words)
		{
			if (recognizedSpeech.Contains(word.Word) && (int?)result?.Type < (int)word.Type)
			{
				result = word;
			}
		}

		return result;
	}
}
