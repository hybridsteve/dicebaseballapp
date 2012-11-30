using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo : System.Object
{
	//public List<Game> currentGames = new List<Game>();
	public Dictionary< string, Game > currentGames = new Dictionary<string, Game>();
	
	public GameInfo( string rawJSON )
	{
		Hashtable gameInfo = MiniJsonExtensions.hashtableFromJson( rawJSON );
		
		Debug.Log( "gameInfo Hashtable contains key games: " + gameInfo.ContainsKey( "games" ) );
		
		ArrayList games = gameInfo["games"] as ArrayList;
		
		Debug.Log( "Length of game array: " + games.Count );
		
		foreach( Hashtable hash in games )
		{
			Game game = new Game( hash );
			currentGames[game.name] = game;
		}
	}
}