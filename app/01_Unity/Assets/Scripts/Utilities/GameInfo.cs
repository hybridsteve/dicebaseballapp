using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo : System.Object
{
	public Dictionary< string, Game > currentGames = new Dictionary<string, Game>();
	public Stats stats;
	
	// TODO: Make this read from the web server if server not available get it from the database.
	// This constructor should not have parameters. Should be an if statement that connects to the server
	// if it doesn't get a response then it goes to the database. Right now just calling the JSON methods.
	public GameInfo( string gamesJSON, string statsJSON )
	{
		getGamesFromJSON( gamesJSON );
		getStatsFromJSON( statsJSON );
	}
	
	public void getGamesFromJSON( string json )
	{
		Hashtable gameInfo = MiniJsonExtensions.hashtableFromJson( json );
		
		ArrayList games = gameInfo["games"] as ArrayList;
		
		foreach( Hashtable hash in games )
		{
			Game game = new Game( hash );
			currentGames[game.name] = game;
		}
	}
	
	public void getStatsFromJSON( string json )
	{
		Hashtable statsFromJSON = MiniJsonExtensions.hashtableFromJson( json );
		
		stats = new Stats( statsFromJSON );
	}
	
	public string ConvertAllGamesToJSON()
	{
		Hashtable mainObject = new Hashtable();
		
		Hashtable[] games = new Hashtable[currentGames.Keys.Count];
		
		int i = 0;
		foreach( string gameName in currentGames.Keys )
		{
			games[i] = currentGames[gameName].ConvertGameToHash();
			i++;
		}
		
		mainObject["games"] = games;
		return MiniJSON.jsonEncode( mainObject );
	}
	
	public void updateLocalGames()
	{
		DBAccess db = new DBAccess();
		db.EmptyTable( "games" );
		db.InsertIntoSingle( "games", "localGames", ConvertAllGamesToJSON() );
		db.CloseDB();
	}
	
	public void updateLocalStats()
	{
		DBAccess db = new DBAccess();
		Hashtable statsHash = stats.convertStatsToHashtable();
		foreach( string key in statsHash.Keys )
		{
			db.UpdateRecordWhere( "stats", "statValue", statsHash[key].ToString(), "statName", key );
		}
		db.CloseDB();
	}
	
	// TODO: Complete this method once the JSON method has been done.
	private void getStatsFromDatabase()
	{
		Debug.Log( "COMPLETE THIS" );
	}
}