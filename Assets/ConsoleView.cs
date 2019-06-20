using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class ConsoleView : MonoBehaviour {
	ConsoleController console = new ConsoleController();
	
	bool didShow = false;
	private bool paused = false;
	private float time;

	public GameObject viewContainer; //Container for console view, should be a child of this GameObject
	public Text logTextArea;
	public InputField inputField;

	void Start() {
		if (console != null) {
			console.visibilityChanged += onVisibilityChanged;
			console.logChanged += onLogChanged;
		}
		updateLogStr(console.log);
		time = Time.timeScale;
		setVisibility(false);
	}
	
	~ConsoleView() {
		console.visibilityChanged -= onVisibilityChanged;
		console.logChanged -= onLogChanged;
	}
	
	void Update() {
		//Toggle visibility when tilde key pressed
		if (Input.GetKeyUp("`"))
		{
			toggleConsole();
		}

		if (Input.GetKeyUp("enter"))
		{
			StartCoroutine(getOut());
		}
	}

	IEnumerator getOut()
	{
		yield return new WaitForSeconds(2);
		console.runCommandString("hide");
		inputField.text = "";
	}

	void toggleConsole() {
		setVisibility(!viewContainer.activeSelf);
		if (!paused)
		{
			Pause();
		}
		else
		{
			Play();
		}
		paused = !paused;
	}

	void Pause()
	{
		time = Time.timeScale;
		Time.timeScale = 0;
	}

	void Play()
	{
		Time.timeScale = time;
	}
	
	
	
	void setVisibility(bool visible) {
		viewContainer.SetActive(visible);
	}
	
	void onVisibilityChanged(bool visible) {
		setVisibility(visible);
	}
	
	void onLogChanged(string[] newLog) {
		updateLogStr(newLog);
	}
	
	void updateLogStr(string[] newLog) {
		if (newLog == null) {
			logTextArea.text = "";
		} else {
			logTextArea.text = string.Join("\n", newLog);
		}
	}

	/// <summary>
	/// Event that should be called by anything wanting to submit the current input to the console.
	/// </summary>
	public void runCommand() {
		console.runCommandString(inputField.text);
		inputField.text = "";
	}

}
