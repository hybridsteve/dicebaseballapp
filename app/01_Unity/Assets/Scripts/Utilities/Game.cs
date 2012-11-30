using UnityEngine;
using System;
using System.Collections;

public class Game : System.Object
{
	public string name
	{
		get { return this.gameName; }
	}
	
	string gameName;
	public int inning;
	public bool playerTurn;
	public string opponentName;
	public int playerScore;
	public int opponentScore;
	
	public Game( Hashtable gameData )
	{
		if( gameData.Keys.Count != 1 )
		{
			throw new System.Exception( "There is more than one key in the game hash. Should only be one, the game name." );
		}
		
		foreach( string key in gameData.Keys )
		{
			gameName = key;
		}
		
		Hashtable gameStatus;
		
		if( (gameStatus = gameData[gameName] as Hashtable) == null )
		{
			throw new System.Exception( "Value of " + gameName + "in hashtable is not a hashtable, cannot recurse." );
		}
		
		foreach( string key in gameStatus.Keys )
		{
			switch( key )
			{
				case "inning":
					inning =  Convert.ToInt32( gameStatus[key] );
					break;
				case "opponent":
					opponentName = gameStatus[key].ToString();
					break;
				case "turn":
					playerTurn = Convert.ToBoolean( gameStatus[key] );
					break;
				case "playerScore":
					playerScore = Convert.ToInt32( gameStatus[key] );
					break;
				case "opponentScore":
					opponentScore = Convert.ToInt32( gameStatus[key] );
					break;
				default:
					Debug.Log( key + " in the game hashtable not found in class." );
					break;
			}
		}
		
		Debug.Log( "Game Name: " + gameName );
		Debug.Log( "Current Inning: " + inning );
		Debug.Log( "Player turn: " + playerTurn );
		Debug.Log( "Opponent Name: " + opponentName );
		Debug.Log( "Current Score: " + playerScore + "/" + opponentScore );
		Debug.Log( "-----------------------------" );
	}
}
