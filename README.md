# OpenShock.VoiceRecognizer

## Disclaimers

1. The `Browser Proxy` settings currently only function on `Windows`, looking for specific registry keys to launch the specified browser.
2. By default, the `Browser Proxy` will capture audio from your **DEFAULT** microphone. Please be aware of this.

## Setup

Setup assumes you are wanting to others' voices to trigger shocks. If you are only looking for your own voice to trigger shocks you may simply capture your microphone `capture` device.

1. Setup [VoiceMeeter Banana](https://vb-audio.com/Voicemeeter/banana.htm) or [VoiceMeeter Potato](https://vb-audio.com/Voicemeeter/potato.htm).
2. If you want to use local speech recognition, you can download a `Vosk` model from [here](https://alphacephei.com/vosk/models). Extract the model preferably local to `OpenShock.VoiceRecognizer`. Try different models to see what works best for you and your hardware.
3. If you want to use your browser (`Chrome` or `Edge` currently), then there is no additional software to acquire.
4. Whatever program you are wanting to capture audio from, try to keep it on a separate `output` audio device (you will want to capture the `capture` device this is played to). If there are multiple programs, then they can all be on the same `output` device.
5. In `Banana` or `Potato`, play your `output` device to a single virtual `input` device (do not have multiple `output` devices to this `input` to keep background noise to a minimum).
6. Now you may run `OpenShock.VoiceRecognizer.exe`. Open the `Options > Settings` menu, and customize the settings as you see fit.
7. Ensure the `General > Input` is set to whatever virtual (or phsyical) `capture` device you used in `banana` or `potato`.
8. For `OpenShock` ensure you have an API Key you can provide to the program to be able to communicate to send commands. This can be set in the `OpenShock` sedttings.
9. If you are using a `Browser Proxy`, ensure you set the `Port` to a non-zero value (recommended `5000`, but use whatever is available for you).
10. If you are using `Vosk`, set the `Vosk > Model` setting to the base directory of the extracted model from `step #2`.
11. Once all settings have been modified, press `Save` and then close the window via the `Close` button or `x`.
12. Now you can press the main `Start` button. If you are using a `Browser Proxy`, a new tab in the browser chosen will open. Follow the instructions in the browser tab to begin capturing sound data.

## OSC

There are two available OSC endpoints:

1. `/recognizer/toggle`: toggles the current active state of the recognizer.
2. `/reocgnizer/set`: sets the current state of the recognizer explicitly. This takes a `bool` value.