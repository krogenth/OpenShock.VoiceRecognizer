using System.Collections.ObjectModel;
using OpenShock.VoiceRecognizer.Configuration;

namespace OpenShock.VoiceRecognizer.NGramRecognizer;

public static class NGramRecognizer
{
	public static WordRecognition? RecognizedNGram(string recognizedSpeech, ObservableCollection<WordRecognition> words)
	{
		WordRecognition? result = null;

		foreach (var word in words)
		{
			// skip all inactive words
			if (!word.Active)
			{
				continue;
			}

			if (recognizedSpeech.Contains(word.Word, StringComparison.CurrentCultureIgnoreCase) &&
				(result?.Type is null || (int?)result?.Type < (int)word.Type))
			{
				result = word;
			}
		}

		return result;
	}
}
