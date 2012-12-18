using UnityEngine;
using System.Collections;

public class Test_json : MonoBehaviour {
	
	// TODO: For now using a text file with a json object.
	// Need to change to getting the string from the server.
	public TextAsset jsonFile;
	
	// Use this for initialization
	void Start ()
	{
		// Commented out because of change to parameters in GameInfo, this test is not needed anymore.
		//string jsonRaw = jsonFile.text;
		
		//GameInfo gameInfo = new GameInfo( jsonRaw );
	}
	
}
