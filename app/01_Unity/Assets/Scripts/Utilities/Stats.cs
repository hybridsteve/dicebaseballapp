using UnityEngine;
using System.Collections;
using System;

public class Stats : System.Object
{
	public int balls, strikes, runs, outs, wins, loss;
	
	public float winAverage{ get { return wins / (wins + loss); } }
	
	public Stats( Hashtable statsHash )
	{
		foreach( string key in statsHash.Keys )
		{
			switch( key )
			{
				case "balls":
					balls = Convert.ToInt32( statsHash[key] );
					break;
				case "strikes":
					strikes = Convert.ToInt32( statsHash[key] );
					break;
				case "runs":
					runs = Convert.ToInt32( statsHash[key] );
					break;
				case "outs":
					outs = Convert.ToInt32( statsHash[key] );
					break;
				case "wins":
					wins = Convert.ToInt32( statsHash[key] );
					break;
				case "loss":
					loss = Convert.ToInt32( statsHash[key] );
					break;
				default:
					Debug.Log( "Key not found in stats class: " + key + " Check json data or add it to class." );
					break;
			} // end switch
		} // end foreach
	} // end constructor
	
	public Hashtable convertStatsToHashtable()
	{
		Hashtable statsHash = new Hashtable();
		
		statsHash["balls"] = balls;
		statsHash["outs"] = outs;
		statsHash["runs"] = runs;
		statsHash["strikes"] = strikes;
		statsHash["wins"] = wins;
		statsHash["loss"] = loss;
		
		return statsHash;
	}
	
	public string convertStatsToJSON()
	{
		Hashtable statsHash = convertStatsToHashtable();
		return MiniJSON.jsonEncode( statsHash );
	}
}
