﻿using OpenShock.VoiceRecognizer.Common.Utility;
using System.Text.Json.Serialization;

namespace OpenShock.VoiceRecognizer.Common.Enums;

[JsonConverter(typeof(TypedStringEnumConverter<RecognizerType>))]
public enum RecognizerType : int
{
	Vosk,
	BrowserProxy,
	//Whisper,
	//Custom,
};
