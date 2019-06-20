using UnityEngine;

using System;
using System.Collections.Generic;
using System.Text;
using Behaviour.Player;

public delegate void CommandHandler(string[] args);

public class ConsoleController {
	
	#region Event declarations
	// Used to communicate with ConsoleView
	public delegate void LogChangedHandler(string[] log);
	public event LogChangedHandler logChanged;
	
	public delegate void VisibilityChangedHandler(bool visible);
	public event VisibilityChangedHandler visibilityChanged;
	#endregion

	/// <summary>
	/// Object to hold information about each command
	/// </summary>
	class CommandRegistration {
		public string command { get; private set; }
		public CommandHandler handler { get; private set; }
		public string help { get; private set; }
		
		public CommandRegistration(string command, CommandHandler handler, string help) {
			this.command = command;
			this.handler = handler;
			this.help = help;
		}
	}

	/// <summary>
	/// How many log lines should be retained?
	/// Note that strings submitted to appendLogLine with embedded newlines will be counted as a single line.
	/// </summary>
	const int scrollbackSize = 20;

	Queue<string> scrollback = new Queue<string>(scrollbackSize);
	List<string> commandHistory = new List<string>();
	Dictionary<string, CommandRegistration> commands = new Dictionary<string, CommandRegistration>();

	public string[] log { get; private set; } //Copy of scrollback as an array for easier use by ConsoleView
	
	const string repeatCmdName = "!!"; //Name of the repeat command, constant since it needs to skip these if they are in the command history
	
	public ConsoleController() {
		//When adding commands, you must add a call below to registerCommand() with its name, implementation method, and help text.
		registerCommand("babble", babble, "Example command that demonstrates how to parse arguments. babble [word] [# of times to repeat]");
		registerCommand("echo", echo, "echoes arguments back as array (for testing argument parser)");
		registerCommand("help", help, "Print this help.");
		registerCommand("hide", hide, "Hide the console.");
		registerCommand(repeatCmdName, repeatCommand, "Repeat last command.");
		registerCommand("reload", reload, "Reload game.");
		registerCommand("resetprefs", resetPrefs, "Reset & saves PlayerPrefs.");
		registerCommand("takeitslow", slow, "Take it sloooooooooow");
		registerCommand("supafast", fast, "Run like the wind");
		registerCommand("god", god, "This makes you feel like God");
		registerCommand("ghost", noCollision, "Become a ghostman");
	}
	
	void registerCommand(string command, CommandHandler handler, string help) {
		commands.Add(command, new CommandRegistration(command, handler, help));
	}
	
	public void appendLogLine(string line) {
		Debug.Log(line);
		
		if (scrollback.Count >= ConsoleController.scrollbackSize) {
			scrollback.Dequeue();
		}
		scrollback.Enqueue(line);
		
		log = scrollback.ToArray();
		if (logChanged != null) {
			logChanged(log);
		}
	}
	
	public void runCommandString(string commandString) {
		appendLogLine("$ " + commandString);
		
		string[] commandSplit = parseArguments(commandString);
		string[] args = new string[0];
		if (commandSplit.Length < 1) {
			appendLogLine(string.Format("Unable to process command '{0}'", commandString));
			return;
			
		} else if (commandSplit.Length >= 2) {
			int numArgs = commandSplit.Length - 1;
			args = new string[numArgs];
			Array.Copy(commandSplit, 1, args, 0, numArgs);
		}
		runCommand(commandSplit[0].ToLower(), args);
		commandHistory.Add(commandString);
	}
	
	public void runCommand(string command, string[] args) {
		CommandRegistration reg = null;
		if (!commands.TryGetValue(command, out reg)) {
			appendLogLine(string.Format("Unknown command '{0}'", command));
		} else {
			if (reg.handler == null) {
				appendLogLine(string.Format("Unable to process command '{0}', handler was null.", command));
			} else {
				reg.handler(args);
			}
		}
	}
	
	static string[] parseArguments(string commandString)
	{
		LinkedList<char> parmChars = new LinkedList<char>(commandString.ToCharArray());
		bool inQuote = false;
		var node = parmChars.First;
		while (node != null)
		{
			var next = node.Next;
			if (node.Value == '"') {
				inQuote = !inQuote;
				parmChars.Remove(node);
			}
			if (!inQuote && node.Value == ' ') {
				node.Value = '\n';
			}
			node = next;
		}
		char[] parmCharsArr = new char[parmChars.Count];
		parmChars.CopyTo(parmCharsArr, 0);
		return (new string(parmCharsArr)).Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
	}

	#region Command handlers
	//Implement new commands in this region of the file.

	/// <summary>
	/// A test command to demonstrate argument checking/parsing.
	/// Will repeat the given word a specified number of times.
	/// </summary>
	void babble(string[] args) {
		if (args.Length < 2) {
			appendLogLine("Expected 2 arguments.");
			return;
		}
		string text = args[0];
		if (string.IsNullOrEmpty(text)) {
			appendLogLine("Expected arg1 to be text.");
		} else {
			int repeat = 0;
			if (!Int32.TryParse(args[1], out repeat)) {
				appendLogLine("Expected an integer for arg2.");
			} else {
				for(int i = 0; i < repeat; ++i) {
					appendLogLine(string.Format("{0} {1}", text, i));
				}
			}
		}
	}

	void echo(string[] args) {
		StringBuilder sb = new StringBuilder();
		foreach (string arg in args)
		{
			sb.AppendFormat("{0},", arg);
		}
		sb.Remove(sb.Length - 1, 1);
		appendLogLine(sb.ToString());
	}

	void help(string[] args) {
		foreach(CommandRegistration reg in commands.Values) {
			appendLogLine(string.Format("{0}: {1}", reg.command, reg.help));
		}
	}
	
	void hide(string[] args) {
		if (visibilityChanged != null) {
			visibilityChanged(false);
		}
	}
	
	void repeatCommand(string[] args) {
		for (int cmdIdx = commandHistory.Count - 1; cmdIdx >= 0; --cmdIdx) {
			string cmd = commandHistory[cmdIdx];
			if (String.Equals(repeatCmdName, cmd)) {
				continue;
			}
			runCommandString(cmd);
			break;
		}
	}
	
	void reload(string[] args) {
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void resetPrefs(string[] args) {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}

	void slow(string[] args)
	{
		appendLogLine("Slowmotion mode activated, take it slow buddy");
		Time.timeScale = 0.3f;
	}
	
	void fast(string[] args)
	{
		appendLogLine("Fastmotion activated. *Running in the 90's starts playing*");
		Time.timeScale = 2.5f;
	}
	
	void god(string[] args)
	{
		GameObject player = GameObject.FindWithTag("Player");	
		if (player != null)
		{
			Collider2D bc = player.GetComponent<Collider2D>();
			bc.enabled = !bc.enabled ;
			switch (bc.enabled)
			{
				case true:
					appendLogLine("Godmode deactivated. You are mere mortal now");
					break;
				case false:
					appendLogLine("Godmode activated. You got the power");
					break;
			}
		}
		else
		{
			appendLogLine("Whoa there bucko, at least try to spawn first before cheating");
		}
		
	}
	
	void noCollision(string[] args)
	{
		GameObject player = GameObject.FindWithTag("Player");	
		if (player != null)
		{
			Movement script = player.GetComponent<Movement>();
			Collider2D bc = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
			LayerMask origin = 9 << 13 | 9 << 12 | 9 << 11 | 9 << 10 | 9 << 9;
			bc.enabled = !bc.enabled ;
			if (script.layer == 0)
			{
				script.layer = origin;
				appendLogLine("Ghost mode deactivated. You are corporeal again");
			}
			else
			{
				script.layer = 0;
				appendLogLine("Ghost mode activated. Beware of Ghost Busters");
			}
			
		}
		else
		{
			appendLogLine("Whoa there bucko, at least try to spawn first before cheating");
		}
	}
	

	#endregion
}
