using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScoreboardController : MonoBehaviour {

	private GameController game;
	
	// game objects that store the x and y positions of the physical scoreboard positions
	private Vector3 column1, column2, column3, column4, row2, row3, row4, outMarker1, outMarker2, strikeMarker1, strikeMarker2, ballMarker1, ballMarker2, ballMarker3;
	// instantiable gameobjects that represent glyphs
	public GameObject inn0, inn1, inn2, inn3, inn4, inn5, inn6, inn7, inn8, inn9;
	public GameObject[] InningMarkerGlyphs;
	
	// instantiable gameobjects that represent out markers
	public GameObject outMarker;
	
	// Results : int { Null=0, Strike=1, FlyOut=2, GroundOut=3, Foul=4, Single=5, Double=6, Triple=7, HomeRun=8 };
	public GameObject stateWaiting, stateStrike, stateFlyOut, stateGroundOut, stateFoul, stateSingle, stateDouble, stateTriple, stateHomeRun;
	
	// var to track the active inning position (which column)
	private int inningPosition;
	private int inning;
	
	public int[] ThisInningScore = new int[] { 0, 0 };
	
	// fields that will contain live gameobjects (find and replace them when you need to update the scoreboard)
	private List<GameObject> InningMarker1, InningMarker2, InningMarker3;
	private List<GameObject> GuestScoreMarker, HomeScoreMarker;
	private List<GameObject> GuestScoreMarker1, GuestScoreMarker2, GuestScoreMarker3;
	private List<GameObject> HomeScoreMarker1, HomeScoreMarker2, HomeScoreMarker3;
	private List<GameObject> OutMarkers = new List<GameObject>();
	private List<GameObject> StrikeMarkers = new List<GameObject>();
	private List<GameObject> BallMarkers = new List<GameObject>();
	private GameObject StatusMessage;
	
	// Use this for initialization
	void Start () {
		
		// grab the game controller so we can access the score variables
		game = GameObject.Find( "GameController" ).GetComponent<GameController>();
		
		InningMarkerGlyphs = new GameObject[] { inn0, inn1, inn2, inn3, inn4, inn5, inn6, inn7, inn8, inn9 };
		
		// Find the physical start location of the scoreboard
		column1 = GameObject.Find( "column1" ).transform.position;
		column2 = GameObject.Find( "column2" ).transform.position;
		column3 = GameObject.Find( "column3" ).transform.position;
		column4 = GameObject.Find( "column4" ).transform.position;
		row2 = GameObject.Find( "row2" ).transform.position;
		row3 = GameObject.Find( "row3" ).transform.position;
		row4 = GameObject.Find( "row4" ).transform.position;
		outMarker1 = GameObject.Find( "outMarker1" ).transform.position;
		outMarker2 = GameObject.Find( "outMarker2" ).transform.position;
		strikeMarker1 = GameObject.Find( "strikeMarker1" ).transform.position;
		strikeMarker2 = GameObject.Find( "strikeMarker2" ).transform.position;
		ballMarker1 = GameObject.Find( "ballMarker1" ).transform.position;
		ballMarker2 = GameObject.Find( "ballMarker2" ).transform.position;
		ballMarker3 = GameObject.Find( "ballMarker3" ).transform.position;
		
		// hack test: set the value of the inning tiles
		GameObject innMarker1, innMarker2, innMarker3;
		innMarker1 = (GameObject) GameObject.Instantiate( inn1 );
		innMarker1.transform.position = column1;
		InningMarker1 = new List<GameObject>();
		InningMarker1.Add( innMarker1 );
		
		innMarker2 = (GameObject) GameObject.Instantiate( inn2 );
		innMarker2.transform.position = column2;
		InningMarker2 = new List<GameObject>();
		InningMarker2.Add( innMarker2 );
		
		innMarker3 = (GameObject) GameObject.Instantiate( inn3 );
		innMarker3.transform.position = column3;
		InningMarker3 = new List<GameObject>();
		InningMarker3.Add( innMarker3 );
		
		
		GameObject currentTile;
		// test set the initial scores
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column1.x, row2.y, row2.z );
		GuestScoreMarker1 = new List<GameObject>();
		GuestScoreMarker1.Add( currentTile );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column2.x, row2.y, row2.z );
		GuestScoreMarker2 = new List<GameObject>();
		GuestScoreMarker2.Add( currentTile );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column3.x, row2.y, row2.z );
		GuestScoreMarker3 = new List<GameObject>();
		GuestScoreMarker3.Add( currentTile );
		
		// total guest team score
		GameObject scoreTileA, scoreTileB;
		scoreTileA = (GameObject) GameObject.Instantiate( inn0 );
		scoreTileA.transform.position = new Vector3( column4.x - 0.7f, row2.y, row2.z );
		scoreTileB = (GameObject) GameObject.Instantiate( inn0 );
		scoreTileB.transform.position = new Vector3( column4.x + 0.7f, row2.y, row2.z );
		
		GuestScoreMarker = new List<GameObject>();
		GuestScoreMarker.Add( scoreTileA );
		GuestScoreMarker.Add( scoreTileB );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column1.x, row3.y, row3.z );
		HomeScoreMarker1 = new List<GameObject>();
		HomeScoreMarker1.Add( currentTile );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column2.x, row3.y, row3.z );
		HomeScoreMarker2 = new List<GameObject>();
		HomeScoreMarker2.Add( currentTile );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column3.x, row3.y, row3.z );
		HomeScoreMarker3 = new List<GameObject>();
		HomeScoreMarker3.Add( currentTile );
		
		// total home team score
		GameObject scoreTileC, scoreTileD;
		scoreTileC = (GameObject) GameObject.Instantiate( inn0 );
		scoreTileC.transform.position = new Vector3( column4.x - 0.7f, row3.y, row3.z );
		scoreTileD = (GameObject) GameObject.Instantiate( inn0 );
		scoreTileD.transform.position = new Vector3( column4.x + 0.7f, row3.y, row3.z );
		
		HomeScoreMarker = new List<GameObject>();
		HomeScoreMarker.Add( scoreTileC );
		HomeScoreMarker.Add( scoreTileD );
		
		SetStatus( 0 );
		
		// set up event listeners for game state stuff
		game.HomeScoreUpdated += new GameController.ScoreUpdatedDelegate( OnHomeScoreUpdate );
		game.VisitorScoreUpdated += new GameController.ScoreUpdatedDelegate( OnVisitorScoreUpdate );
		game.AdvancedAnInning += new GameController.AdvancedAnInningDelegate( OnNewInning );
		game.GotAnOut += new GameController.OutDelegate( OnOut );
		game.GotAStrike += new GameController.StrikeDelegate( OnStrike );
		game.GotABall += new GameController.BallDelegate( OnBall );
		game.GotAHit += new GameController.HitDelegate( OnHit );
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void OnHomeScoreUpdate()
	{
		Debug.Log( "scoreboard noticed that the home team's score updated" );
		UpdateHomeScoreDisplay( game.HomeScore );
		UpdateHomeInningDisplay( game.ScoreByInning[ game.Inning - 1 ][ 0 ] );
	}
	
	private void OnVisitorScoreUpdate()
	{
		Debug.Log( "scoreboard noticed that the visiting team's score updated" );
		UpdateGuestScoreDisplay( game.VisitorScore );
		UpdateGuestInningDisplay( game.ScoreByInning[ game.Inning - 1 ][ 1 ] );
	}
	
	private void OnNewInning()
	{
		Debug.Log( "scoreboard noticed that there is a new inning" );
		SetInning( game.Inning );
	}
	
	private void OnStrike()
	{
		Debug.Log( "scoreboard noticed that batter got a strike" );
		AddStrikeMarker();
	}
	
	private void OnBall()
	{
		Debug.Log( "scoreboard noticed that batter got a ball" );
		AddBallMarker();
	}
	
	private void OnOut()
	{
		Debug.Log( "scoreboard sees out" );
		AddOutMarker();
		ClearStrikes();
		ClearBalls();
	}
	
	private void OnHit()
	{
		Debug.Log( "scoreboard sees hit" );
		ClearStrikes();
		ClearBalls();
	}
	
	public void SetStatus( int status )
	{
		// clear the old status message
		ClearStatus();
		
		// Results : int { Null=0, Strike=1, FlyOut=2, GroundOut=3, Foul=4, Single=5, Double=6, Triple=7, HomeRun=8 };
		
		switch (status)
		{
		case 0:
			StatusMessage = (GameObject) GameObject.Instantiate( stateWaiting );
			break;
		case 1:
			StatusMessage = (GameObject) GameObject.Instantiate( stateStrike );
			break;
		case 2:
			StatusMessage = (GameObject) GameObject.Instantiate( stateFlyOut );
			break;
		case 3:
			StatusMessage = (GameObject) GameObject.Instantiate( stateGroundOut );
			break;
		default: 
			StatusMessage = (GameObject) GameObject.Instantiate( stateWaiting );
			break;
		}
		StatusMessage.transform.position = row4;
	}
	
	private void ClearStatus()
	{
		GameObject.Destroy( StatusMessage );	
	}
	
	// todo maybe do this with events
	public void SetInning( int currentInning )
	{
		inning = currentInning;
		
		ThisInningScore = new int[] { 0, 0 };
		
		UpdateInningDisplay();
	}
	
	public void UpdateInningDisplay()
	{
		// set inningposition (represents which column on the scoreboard represents the current inning) to 1, 2, or 3
		inningPosition = Math.Min( inning, 3 );
		
		if ( inning > 3 )
		{
			// replace inning markers with +1
						
			foreach ( GameObject obj in InningMarker1 )
			{
				GameObject.Destroy( obj );	
			}
			InningMarker1 = new List<GameObject>();
			InningMarker1.AddRange( GetAndPlaceTilesForNumber( inning - 2, column1 ) );
			
			
			foreach ( GameObject obj in InningMarker2 )
			{
				GameObject.Destroy( obj );	
			}
			InningMarker2 = new List<GameObject>();
			InningMarker2.AddRange( GetAndPlaceTilesForNumber( inning - 1, column2 ) );
			
			
			foreach ( GameObject obj in InningMarker3 )
			{
				GameObject.Destroy( obj );	
			}
			InningMarker3 = new List<GameObject>();
			InningMarker3.AddRange( GetAndPlaceTilesForNumber( inning, column3 ) );
			
			
			UpdateGuestInningDisplay( game.ScoreByInning[ game.Inning - 1 ][ 1 ] );
			UpdateGuestInningDisplay( game.ScoreByInning[ game.Inning - 2 ][ 1 ], 2 );
			UpdateGuestInningDisplay( game.ScoreByInning[ game.Inning - 3 ][ 1 ], 1 );
			
			
			UpdateHomeInningDisplay( game.ScoreByInning[ game.Inning - 1 ][ 0 ] );
			UpdateHomeInningDisplay( game.ScoreByInning[ game.Inning - 2 ][ 0 ], 2 );
			UpdateHomeInningDisplay( game.ScoreByInning[ game.Inning - 3 ][ 0 ], 1 );
		}
		
	}
	
	private GameObject[] GetAndPlaceTilesForNumber( int number, Vector3 position )
	{
		return GetAndPlaceTilesForNumber( number, position, false );
	}
	
	private GameObject[] GetAndPlaceTilesForNumber( int number, Vector3 position, bool forceDoubleDigits )
	{
		GameObject tile1, tile2;
		// If we need 2 tiles
		if ( number > 9 || forceDoubleDigits )
		{
			// Place 2 tiles
			int[] glyphArray = FormatScore( number, forceDoubleDigits );
			tile1 = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ glyphArray[0] ] );
			tile1.transform.position = new Vector3( position.x + 0.7f, position.y, position.z );
			tile2 = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ glyphArray[1] ] );
			tile2.transform.position = new Vector3( position.x - 0.7f, position.y, position.z );
			
			return new GameObject[] { tile1, tile2 };
		}
		else
		{
			// Place 1 tile
			tile1 = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ number ] );
			tile1.transform.position = position;
			
			return new GameObject[] { tile1 };
		}
	}
	
	public void UpdateGuestScoreDisplay( int score )
	{
		// clear the score display
		foreach ( GameObject obj in GuestScoreMarker )
		{
			GameObject.Destroy( obj );	
		}
		
		// format score for display
		int[] scoreArray = FormatScore( score, true );
		
		// create and place the tiles
		GameObject scoreMarkerA, scoreMarkerB;
		
		scoreMarkerA = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ scoreArray[0] ] );
		scoreMarkerA.transform.position = new Vector3( column4.x + 0.7f, row2.y, row2.z );
		
		scoreMarkerB = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ scoreArray[1] ] );
		scoreMarkerB.transform.position = new Vector3( column4.x - 0.7f, row2.y, row2.z );
		
		GuestScoreMarker = new List<GameObject>();
		GuestScoreMarker.Add( scoreMarkerA );
		GuestScoreMarker.Add( scoreMarkerB );
	}
	
	public void UpdateGuestInningDisplay( int score )
	{
		UpdateGuestInningDisplay( score, inningPosition );	
	}
	
	public void UpdateGuestInningDisplay( int score, int position )
	{
		// hack just trying to figure this out for now
		
		// which column?
		Vector3 column;
		List<GameObject> MarkerSet;
		switch ( position )
		{
		case 1:
			column = column1;
			MarkerSet = GuestScoreMarker1;
			break;
		case 2:
			column = column2;
			MarkerSet = GuestScoreMarker2;
			break;
		case 3:
			column = column3;
			MarkerSet = GuestScoreMarker3;
			break;
		default:
			column = column3;
			MarkerSet = GuestScoreMarker3;
			break;
		}
		
		foreach ( GameObject obj in MarkerSet )
		{
			GameObject.Destroy( obj );	
		}
		
		Vector3 finalPosition = new Vector3( column.x, row2.y, row2.z );
		
		MarkerSet.AddRange( GetAndPlaceTilesForNumber( score, finalPosition ) );
	}
	
	public void UpdateHomeScoreDisplay( int score )
	{
		// clear the score display
		foreach ( GameObject obj in HomeScoreMarker )
		{
			GameObject.Destroy( obj );
		}
		
		// format score for display
		int[] scoreArray = FormatScore( score, true );
		
		// create and place the tiles
		GameObject scoreMarkerA, scoreMarkerB;
		
		scoreMarkerA = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ scoreArray[0] ] );
		scoreMarkerA.transform.position = new Vector3( column4.x + 0.7f, row3.y, row3.z );
		
		scoreMarkerB = (GameObject) GameObject.Instantiate( InningMarkerGlyphs[ scoreArray[1] ] );
		scoreMarkerB.transform.position = new Vector3( column4.x - 0.7f, row3.y, row3.z );
		
		HomeScoreMarker = new List<GameObject>();
		HomeScoreMarker.Add( scoreMarkerA );
		HomeScoreMarker.Add( scoreMarkerB );
	}
	
	public void UpdateHomeInningDisplay( int score )
	{
		UpdateHomeInningDisplay( score, inningPosition );
	}
	
	public void UpdateHomeInningDisplay( int score, int position )
	{
		// hack just trying to figure this out for now
		
		// which column?
		Vector3 column;
		List<GameObject> MarkerSet;
		switch ( position )
		{
		case 1:
			column = column1;
			MarkerSet = HomeScoreMarker1;
			break;
		case 2:
			column = column2;
			MarkerSet = HomeScoreMarker2;
			break;
		case 3:
			column = column3;
			MarkerSet = HomeScoreMarker3;
			break;
		default:
			column = column3;
			MarkerSet = HomeScoreMarker3;
			break;
		}
		
		foreach ( GameObject obj in MarkerSet )
		{
			GameObject.Destroy( obj );	
		}
		
		Vector3 finalPosition = new Vector3( column.x, row3.y, row3.z );
		
		MarkerSet.AddRange( GetAndPlaceTilesForNumber( score, finalPosition ) );
	}
	
	private int[] FormatScore( int score )
	{
		return FormatScore( score, false );	
	}
	
	private int[] FormatScore( int score, bool forceDoubleDigits )
	{
		// cap score for display
		score = Math.Min( score, 99 );
		
		// convert score to string so we can get value at each position
		string scoreString = score.ToString();
		int score1, score2;
		
		if ( scoreString.Length > 1 )
		{
			// two positions in score
			score1 = int.Parse( scoreString.Substring( 0, 1 ) );
			score2 = int.Parse( scoreString.Substring( 1, 1 ) );
			
		}
		else if ( forceDoubleDigits )
		{
			// one position in score
			score1 = 0;
			score2 = int.Parse( scoreString.Substring( 0, 1 ) );
		}
		else
		{
			score1 = int.Parse( scoreString.Substring( 0, 1 ) );
			return new int[] { score1 };
		}
		
		return new int[] { score1, score2 };
	}
	
	private void AddOutMarker()
	{
		int numOuts = game.Outs;
		GameObject marker;
		switch ( numOuts )
		{
		case 1:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = outMarker1;
			OutMarkers.Add( marker );
			break;
		case 2:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = outMarker2;
			OutMarkers.Add( marker );
			break;
		case 3:
			foreach (GameObject obj in OutMarkers)
			{
				GameObject.Destroy( obj );	
			}
			// empty list
			OutMarkers = new List<GameObject>();
			
			break;
		default:
			break;
		}
	}
	
	private void AddStrikeMarker()
	{
		int numStrikes = game.Strikes;
		GameObject marker;
		switch (numStrikes)
		{
		case 1:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = strikeMarker1;
			StrikeMarkers.Add( marker );
			break;
		case 2:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = strikeMarker2;
			StrikeMarkers.Add( marker );
			break;
		case 3:
			ClearStrikes();
			
			break;
		default:
			break;
		}
	}
	
	private void AddBallMarker()
	{
		int numBalls = game.Balls;
		GameObject marker;
		switch (numBalls)
		{
		case 1:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = ballMarker1;
			BallMarkers.Add( marker );
			break;
		case 2:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = ballMarker2;
			BallMarkers.Add( marker );
			break;
		case 3:
			marker = (GameObject) GameObject.Instantiate( outMarker );
			marker.transform.position = ballMarker3;
			BallMarkers.Add( marker );
			break;
		case 4:
			ClearBalls();
			
			break;
		default:
			break;
		}
	}
	
	private void ClearStrikes()
	{
		foreach (GameObject obj in StrikeMarkers)
		{
			GameObject.Destroy( obj );	
		}
		StrikeMarkers = new List<GameObject>();
	}
	
	private void ClearBalls()
	{
		foreach (GameObject obj in BallMarkers)
		{
			GameObject.Destroy( obj );
		}
		BallMarkers = new List<GameObject>();
	}
}
