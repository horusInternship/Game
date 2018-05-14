using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.AVFoundation;
using U3DXT.Utils;
using System.Collections.Generic;
using U3DXT.iOS.Speech;
using U3DXT.Utils.SpeechTest;

public class SpeechTest : MonoBehaviour {

	string speakingText = "";
	string inputText = "Hello from U3DXT.";
	GUIStyle labelStyle = new GUIStyle();

	GUIContent[] comboBoxList;
	ComboBox comboBoxControl = new ComboBox();
	GUIStyle listStyle = new GUIStyle();
	int selectedItemIndex = 0;
	
	float pitchValue = 1.0f;
	float speedValue = 0.25f;
	float volumeValue = 1.0f;

	void Start() {
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontSize = 100;
#if UNITY_3_5
#else
		labelStyle.richText = true;
#endif
		
		listStyle.normal.textColor = Color.white; 
		listStyle.hover.background = new Texture2D(2, 2);
		listStyle.padding.bottom = 4;
		listStyle.fontSize = 20;
		
		if (!CoreXT.IsDevice) {
			comboBoxList = new GUIContent[2];
			comboBoxList[0] = new GUIContent("en-US");
			comboBoxList[1] = new GUIContent("en-US");
		} else {
			// get all available voices
			var currentLang = SpeechXT.currentLocaleVoice.language;
			var voices = SpeechXT.availableVoices;
			
			// populate combo box with them
			comboBoxList = new GUIContent[voices.Length];
			for (int i=0; i<voices.Length; i++) {
				comboBoxList[i] = new GUIContent(voices[i].language);
				if (voices[i].language == currentLang)
					selectedItemIndex = i;
			}
			
			// subscribe to events
			SpeechXT.WillSpeak += OnWillSpeak;
			SpeechXT.SpeechFinished += OnSpeechFinished;
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			SpeechXT.WillSpeak -= OnWillSpeak;
			SpeechXT.SpeechFinished -= OnSpeechFinished;
		}
	}
	
	// when a part of speech is about to speak, highlight that part
	void OnWillSpeak(object sender, SpeechWillSpeakEventArgs e) {
		speakingText = e.utterance.speechString;

#if UNITY_3_5
		speakingText = speakingText.Substring(0, (int)e.characterRange.location) + "*"
			+ speakingText.Substring((int)e.characterRange.location, (int)e.characterRange.length) + "*"
				+ speakingText.Substring((int)(e.characterRange.location + e.characterRange.length));
#else
		speakingText = speakingText.Substring(0, (int)e.characterRange.location) + "<color=white><b>"
			+ speakingText.Substring((int)e.characterRange.location, (int)e.characterRange.length) + "</b></color>"
				+ speakingText.Substring((int)(e.characterRange.location + e.characterRange.length));
#endif
	}
	
	// when speech is finished, clear it
	void OnSpeechFinished(object sender, SpeechEventArgs e) {
		speakingText = "";
	}
	

	void OnGUI() {
		
		KitchenSink.OnGUIBack();

		// display the currently speaking text
		GUI.Label(new Rect(50, 50, Screen.width - 100, Screen.height / 4 - 60), speakingText, labelStyle);
		
		// ask for input
		inputText = GUI.TextField(new Rect(50, Screen.height / 4, (float)(Screen.width * 0.8 - 50), 80), inputText);

		// speak the input text
		if (GUI.Button(new Rect((float)(Screen.width * 0.8), Screen.height / 4, (float)(Screen.width * 0.2 - 50), 80), "Speak")) {
			SpeechXT.settings.pitchMultiplier = pitchValue;
			SpeechXT.settings.rate = speedValue;
			SpeechXT.settings.volume = volumeValue;
			SpeechXT.settings.voice = AVSpeechSynthesisVoice.Voice(comboBoxList[selectedItemIndex].text);
			SpeechXT.Speak(inputText);
		}

		// combo box for selecting languages
		GUI.Label(new Rect(50, Screen.height / 4 + 100, 100, 50), "Language");
		selectedItemIndex = comboBoxControl.List(
			new Rect(150, Screen.height / 4 + 100, 100, 50), selectedItemIndex, comboBoxList, listStyle );


		GUILayout.BeginArea(new Rect(50, Screen.height / 4 + 200, Screen.width / 2 - 100, Screen.height / 4 * 3 - 250));
		
		// manually change pitch, speed, and volume
		pitchValue = GUILayout.HorizontalSlider(pitchValue, 0.5f, 2.0f);
		GUILayout.Label("Pitch: " + pitchValue, GUILayout.ExpandHeight(true));

		speedValue = GUILayout.HorizontalSlider(speedValue, 0.0f, 1.0f);
		GUILayout.Label("Speed: " + speedValue, GUILayout.ExpandHeight(true));

		volumeValue = GUILayout.HorizontalSlider(volumeValue, 0.0f, 1.0f);
		GUILayout.Label("Volume: " + volumeValue, GUILayout.ExpandHeight(true));

		GUILayout.EndArea();

		
		// predefined examples in different languages
		GUI.Label(new Rect(Screen.width / 4 * 3 - 50, Screen.height / 4 + 100, 100, 50), "Examples");

		GUILayout.BeginArea(new Rect(Screen.width / 2 + 50, Screen.height / 4 + 150, Screen.width / 4 - 60, Screen.height / 4 * 3 - 200));

		if (GUILayout.Button("American English", GUILayout.ExpandHeight(true))) {
			SpeechXT.settings.pitchMultiplier = 1f;
			SpeechXT.settings.rate = 0.25f;
			SpeechXT.settings.volume = 1f;
			SpeechXT.settings.voice = AVSpeechSynthesisVoice.Voice("en-US");
			SpeechXT.Speak("Hello, I have Siri's voice.");
		}

		if (GUILayout.Button("Mexican Spanish", GUILayout.ExpandHeight(true))) {
			SpeechXT.settings.voice = AVSpeechSynthesisVoice.Voice("es-MX");
			SpeechXT.Speak("Buenos días");
		}

		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(Screen.width / 4 * 3 + 10, Screen.height / 4 + 150, Screen.width / 4 - 60, Screen.height / 4 * 3 - 200));

		if (GUILayout.Button("Aussie", GUILayout.ExpandHeight(true))) {
			SpeechXT.settings.voice = AVSpeechSynthesisVoice.Voice("en-AU");
			SpeechXT.Speak("Good day, mate.");
		}

		if (GUILayout.Button("French", GUILayout.ExpandHeight(true))) {
			SpeechXT.settings.voice = AVSpeechSynthesisVoice.Voice("fr-FR");
			SpeechXT.Speak("bonjour, mon amour.");
		}

		GUILayout.EndArea();
	}
}
