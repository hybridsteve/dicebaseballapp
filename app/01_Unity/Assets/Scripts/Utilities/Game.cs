using UnityEngine;
using System;
using System.Collections;

public class Game : System.Object
{
	public string name
	{
		get { return this.gameName; }
	}
	
	private string gameName;
	public int inning;
	public bool playerTurn;
	public string opponentName;
	public int playerScore;
	public int opponentScore;
	public DateTime date;
	
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
				case "date":
					date = DateTime.Parse( gameStatus[key].ToString() );
					break;
				default:
					Debug.Log( key + " in the game hashtable not found in class." );
					break;
			}
		}
	}
	
	public string ConvertGameToJSON()
	{
		Hashtable gamesHash = ConvertGameToHash();
		return MiniJSON.jsonEncode(gamesHash);
	}
	
	public Hashtable ConvertGameToHash()
	{
		Hashtable gamesHash = new Hashtable();
		Hashtable gameHash = new Hashtable();
		
		gameHash["inning"] = inning;
		gameHash["turn"] = playerTurn;
		gameHash["opponentScore"] = opponentScore;
		gameHash["playerScore"] = playerScore;
		gameHash["opponent"] = opponentName;
		gameHash["date"] = date.ToString();
		
		gamesHash[name] = gameHash;
		
		return gamesHash;
	}
}
