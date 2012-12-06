using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public enum GameTypes : int { Single=0, Local=1, Net=2 };
	public GameTypes GameType;
	
	public bool LoadingGame = false;
	
	public Camera GameCamera;
	private GameCamera cameraController;
	
	public GameObject Die;
	public GameObject Batter;
	
	private ScoreboardController Scoreboard;
	
	private bool createdDice = false;
	private List<GameObject> Dice = new List<GameObject>();
	private List<GameObject> Runners = new List<GameObject>();
	
	// hack these are used for some demo text
	public UIToolkit Treb18;
	private UIText ScoreboardText;
	private UITextInstance ScoreboardTextInstance;
	
	private enum Results : int { Null=0, Strike=1, FlyOut=2, GroundOut=3, Foul=4, Single=5, Double=6, Triple=7, HomeRun=8 };
	private Results Result;
	
	private enum States : int { Null=0, Waiting=1, WaitingForRoll=2, Rolling=3, Rolled=4, ShowingOutcomes=5, Ended=6 };
	private States State;
	
	private bool debugMode = false;
	public bool DebugMode
	{
		get { return debugMode; }
	}
	
	private string ResultString;
	private string ScoreboardResultText;
	
	public delegate void WaitBeforeDoingDelegate();
	public delegate void TouchEventDelegate();
	public delegate void ScoreUpdatedDelegate();
	public delegate void AdvancedAnInningDelegate();
	public delegate void OutDelegate();
	
	public event TouchEventDelegate TouchEvent;
	public event ScoreUpdatedDelegate HomeScoreUpdated;
	public event ScoreUpdatedDelegate VisitorScoreUpdated;
	public event AdvancedAnInningDelegate AdvancedAnInning;
	public event OutDelegate GotAnOut;
	
	// todo put in class container
	public enum InningTypes : int { Top=0, Bottom=1 };
	public int Inning = 0;
	public int NumInnings = 9;
	public InningTypes InningType = InningTypes.Top;
	public int Strikes = 0;
	public int Outs = 0;
	//public int Fouls = 0;
	public int HomeScore = 0;
	public string HomeName = "Home Team";
	public int VisitorScore = 0;
	public string VisitorName = "Visiting Team";
	//public int[,] ScoreByInning = new int[,] { { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } , { 0 , 0 } };
	public List<int[]> ScoreByInning = new List<int[]>();
	public int CurrentPlayer = 2; // 1 is home team, 2 is visitor
	
	// todo move this into a mesh controller singleton
	// todo so that we can assign different meshes to pawns easily if we want
	public Mesh PawnMesh;
	
	// Use this for initialization
	void Start () {
		//cameraController = (GameCamera) GameCamera.GetComponent(typeof(GameCamera));
		cameraController = GameCamera.GetComponent<GameCamera>();
		
		// find the scoreboard controller
		Scoreboard = GameObject.Find( "ScoreboardController" ).GetComponent<ScoreboardController>();
		
		// debug text setup
		if ( debugMode )
		{
			ScoreboardText = new UIText( Treb18, "Trebuchet14", "Trebuchet14.png" );
			ScoreboardTextInstance = ScoreboardText.addTextInstance( "Initial Setup", 0f, 0f, 1f, 0 );
			ScoreboardTextInstance.positionFromTop( 0.05f );
		}
		
		// todo set gametype based on menu choice
		//GameType = GameTypes.Single;
		State = States.Null;
		
		SetupGame( LoadingGame, GameType );
	}
	
	// Update is called once per frame
	void Update () {
				
		if ( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended )
		{
			TouchEvent();
		}
		
		if ( Input.GetMouseButtonUp(0) )
		{
			TouchEvent();
		}
		
		Vector3 dir = Vector3.zero;
		
		if ( State == States.WaitingForRoll )
		{
			// if the AI should be throwing
			if ( GameType == GameTypes.Single && CurrentPlayer == 2 )
			{
				// do the ai throw
				AIThrowDice();
			}
			else
			{
				// Get the current acceleration
				
				//dir.x = Mathf.Min( Mathf.Abs( Input.acceleration.y ), 10f ) * Mathf.Sign( Input.acceleration.y );
				//dir.y = Mathf.Min( Mathf.Abs( Input.acceleration.z ), 5f ) * Mathf.Sign( Input.acceleration.z );
				//dir.z = -Mathf.Min( Mathf.Abs( Input.acceleration.x ), 10f ) * Mathf.Sign( Input.acceleration.x );
				
				dir.x = Input.acceleration.y;
				dir.y = -Input.acceleration.z;
				dir.z = -Input.acceleration.x;
				
				// hack debug dice are thrown if acceleration is high enough. code within die controller ignores this unless dice are throwable
				if (Mathf.Abs(dir.sqrMagnitude) > 5 && ( State == States.WaitingForRoll || State == States.Rolling ))
				{
					ThrowDice( dir );	
				}
			}
		}
		
		// hack force throw of dice with keypress for debug
		if ( Input.GetAxis("Horizontal") > 0  && State == States.WaitingForRoll )
		{
			Debug.Log( "forced throw" );
			dir.x = UnityEngine.Random.Range( -10, 10 );
			dir.y = UnityEngine.Random.Range( 5, 10 );
			dir.z = UnityEngine.Random.Range( -10, 10 );
			
			//dir.Normalize();
			
			ThrowDice( dir );
		}
		
		// if we have begun the dice throw process, monitor for dice results
		if ( State == States.Rolling )
		{
			// as long as the AI is not currently throwing, player can keep jostling dice
			if ( !( GameType == GameTypes.Single && CurrentPlayer == 2 ) )
			{
				// Get the current acceleration
				
				//dir.x = Mathf.Min( Mathf.Abs( Input.acceleration.y ), 10f ) * Mathf.Sign( Input.acceleration.y );
				//dir.y = Mathf.Min( Mathf.Abs( Input.acceleration.z ), 5f ) * Mathf.Sign( Input.acceleration.z );
				//dir.z = -Mathf.Min( Mathf.Abs( Input.acceleration.x ), 10f ) * Mathf.Sign( Input.acceleration.x );
				
				dir.x = Input.acceleration.y;
				dir.y = -Input.acceleration.z;
				dir.z = -Input.acceleration.x;
				
				// hack debug dice are thrown if acceleration is high enough. code within die controller ignores this unless dice are throwable
				if (Mathf.Abs(dir.sqrMagnitude) > 5 && ( State == States.WaitingForRoll || State == States.Rolling ))
				{
					ThrowDice( dir );	
				}
			}
			
			int DiceTotal = 0;
			int stoppedDice = 0;
			for ( int i = 0; i < Dice.Count; i ++ )
			{
				// get die controller class
				GameObject die = Dice[i];
				DieController controller = die.GetComponent<DieController>();
				
				// check if die is stopped
				if ( controller.State == DieController.States.Rolled )
				{
					DiceTotal += controller.DieValue;
					stoppedDice ++;
				}
			}
			
			if ( stoppedDice == Dice.Count )
			{
				Debug.Log( "all dice results in" );
			 	Debug.Log( "total rolled is: " + DiceTotal );
				SetResult( DiceTotal );
				ScoreboardResultText = DiceTotal + "! " + ResultString;
				WaitBeforeDoing( 1f, new WaitBeforeDoingDelegate( cameraController.ZoomToField ) );
				cameraController.FinishedMoving += new GameCamera.CameraEventHandler( HandleRollOutcome );
				
				State = States.Rolled;
			}
		}
		
		// hack debug mode
		if ( State == States.WaitingForRoll )
		{
			if ( Input.GetKeyUp( KeyCode.Keypad1 ) )
			{
				SetResult( 4 );
				
				ScoreboardResultText = "4! " + ResultString;
				WaitBeforeDoing( 2f, new WaitBeforeDoingDelegate( cameraController.ZoomToField ) );
				cameraController.FinishedMoving += new GameCamera.CameraEventHandler( HandleRollOutcome );
				
				State = States.Rolled;
			}
			else if ( Input.GetKeyUp( KeyCode.Keypad2 ) )
			{
				SetResult( 8 );
				
				ScoreboardResultText = "8! " + ResultString;
				WaitBeforeDoing( 2f, new WaitBeforeDoingDelegate( cameraController.ZoomToField ) );
				cameraController.FinishedMoving += new GameCamera.CameraEventHandler( HandleRollOutcome );
				
				State = States.Rolled;
			}
			else if ( Input.GetKeyUp( KeyCode.Keypad3 ) )
			{
				SetResult( 10 );
				
				ScoreboardResultText = "10! " + ResultString;
				WaitBeforeDoing( 2f, new WaitBeforeDoingDelegate( cameraController.ZoomToField ) );
				cameraController.FinishedMoving += new GameCamera.CameraEventHandler( HandleRollOutcome );
				
				State = States.Rolled;
			}
			else if ( Input.GetKeyUp( KeyCode.Keypad4 ) )
			{
				SetResult( 12 );
				
				ScoreboardResultText = "12! " + ResultString;
				WaitBeforeDoing( 2f, new WaitBeforeDoingDelegate( cameraController.ZoomToField ) );
				cameraController.FinishedMoving += new GameCamera.CameraEventHandler( HandleRollOutcome );
				
				State = States.Rolled;
			}
			else if ( Input.GetKeyUp( KeyCode.Keypad5 ) )
			{
				SetResult( 5 );
				
				ScoreboardResultText = "5! " + ResultString;
				WaitBeforeDoing( 2f, new WaitBeforeDoingDelegate( cameraController.ZoomToField ) );
				cameraController.FinishedMoving += new GameCamera.CameraEventHandler( HandleRollOutcome );
				
				State = States.Rolled;
			}
		}
		
	}
	
	/// <summary>
	/// Set up the game variables based on whether this is a new game or a continued game. Always called after start.
	/// </summary>
	/// <param name='gameType'>
	/// Game type.
	/// </param>
	private void SetupGame( bool loadingGame, GameTypes gameType )
	{
		if ( loadingGame == false )
		{
			// todo instantiate game class
			Inning = 1;
			InningType = InningTypes.Top;
			ScoreByInning.Add( new int[] { 0, 0 } );
			
			if ( gameType == GameTypes.Local )
			{
				CurrentPlayer = 2; // if local multiplayer, player 2 bats first
			}
			else
			{
				CurrentPlayer = 1; // if single player, player goes before AI
			}
		
			StartBatter();
		}
		else 
		{
			// todo loaded a game
			LoadGame();
		}
		
		Scoreboard.SetInning( Inning );
	}
	
	private void LoadGame()
	{
		// todo get the game date from somewhere
		
		// todo instantiate game class with data
	}
	
	/// <summary>
	/// Beginning of the game logic loop. The batter is at bat, the game details are displayed, waiting for player input.
	/// </summary>
	private void StartBatter()
	{
		State = States.Waiting;
		
		// set up batter
		ShowBatter();
		
		Scoreboard.SetStatus( 0 );
		
		if ( debugMode )
		{
			ShowDebugStats();
		}
		
		// add a touch event handler - player touch sends game into dice roll mode
		// todo do we want the AI to do this automatically?
		TouchEvent += new TouchEventDelegate( DiceView );
	}
	
	private void ShowDebugStats()
	{
		// show / update scoreboard stuff
		string scoreBoardString = "Batter Up!      " + InningType + " of the " + Inning + "th \n";
		if (CurrentPlayer == 1)
		{
			scoreBoardString += HomeName + " at bat! \n";
		}
		else 
		{
			scoreBoardString += VisitorName + " at bat! \n";	
		}
		scoreBoardString += "Strikes: " + Strikes + "  Outs: " + Outs + "\n \n";
		scoreBoardString += HomeName + " Score: " + HomeScore + "  \n" + VisitorName + " Score: " + VisitorScore + " \n \n";
		scoreBoardString += Runners.Count + " runners! Tap to roll!";
		ScoreboardTextInstance.text = scoreBoardString;
	}
	
	/// <summary>
	/// Shows the batter.
	/// </summary>
	private void ShowBatter()
	{
		GameObject batter = GameObject.Find( "C_Piece1_" );
		batter.transform.position = Batter.transform.position;
	}
	
	/// <summary>
	/// Hides the batter.
	/// </summary>
	private void HideBatter()
	{
		GameObject batter = GameObject.Find( "C_Piece1_" );
		batter.transform.position = new Vector3( 0f, -3f, 0f );
	}
	
	private void DiceView()
	{
		// initial camera movement todo follows intro prompts
		//StartCoroutine( MoveCameraAfterSeconds( 3f, 0 ) );
		WaitBeforeDoing( 1f, new WaitBeforeDoingDelegate( cameraController.ZoomToDice ) );
		cameraController.StartedMoving += new GameCamera.CameraEventHandler( ClearScoreboard );
		cameraController.FinishedMoving += new GameCamera.CameraEventHandler( MakeDiceThrowable );
		TouchEvent -= new TouchEventDelegate( DiceView );
	}
	
	private void SetResult( int result )
	{
		switch (result)
		{
		case 2:
			ResultString = "Strike!";
			Result = Results.Strike;
			break;
		case 3:
			ResultString = "Fly out!";
			Result = Results.FlyOut;
			break;
		case 4:
			ResultString = "Single!";
			Result = Results.Single;
			break;
		case 5:
			ResultString = "Ground out!";
			Result = Results.GroundOut;
			break;
		case 6:
			ResultString = "Strike!";
			Result = Results.Strike;
			break;
		case 7:
			ResultString = "Fly out!";
			Result = Results.FlyOut;
			break;
		case 8:
			ResultString = "Double!";
			Result = Results.Double;
			break;
		case 9:
			ResultString = "Ground out!";
			Result = Results.GroundOut;
			break;
		case 10:
			ResultString = "Triple!";
			Result = Results.Triple;
			break;
		case 11:
			ResultString = "Foul ball!";
			Result = Results.Foul;
			break;
		case 12:
			ResultString = "Home run!";
			Result = Results.HomeRun;
			break;
		default:
			ResultString = "Error!";
			Result = Results.Null;
			break;
		}
	}
	
	private void HandleRollOutcome( object sender, EventArgs e)
	{
		State = States.ShowingOutcomes;
		ShowBatter();
		
		UpdateScoreboard( ScoreboardResultText );
		
		// call different functions based on the result
		switch (Result)
		{
		case Results.Strike:
			Strike();
			break;
		case Results.FlyOut:
			Out();
			break;
		case Results.GroundOut:
			Out();
			break;
		case Results.Foul:
			Foul();
			break;
		case Results.Single:
			CreateRunner();
			Hit( 1 );
			break;
		case Results.Double:
			CreateRunner();
			Hit( 2 );
			break;
		case Results.Triple:
			CreateRunner();
			Hit( 3 );
			break;
		case Results.HomeRun:
			CreateRunner();
			Hit( 4 );
			break;
		default:
			ReturnToBatter();
			break;
		}
		
		// set the scoreboard display to the roll result
		Scoreboard.SetStatus( (int) Result );
		
		cameraController.FinishedMoving -= new GameCamera.CameraEventHandler( HandleRollOutcome );
	}
	
	private void Strike()
	{
		Strikes += 1;
		//Fouls = 0;
		
		if ( Strikes >= 3 )
		{
			Out();	
		}
		else
		{
			ReturnToBatter();	
		}
	}
	
	private void Out()
	{
		Strikes = 0;
		//Fouls = 0;
		Outs += 1;
		GotAnOut();
		
		if ( Outs >= 3 )
		{
			EndSubInning();
		}
		else
		{
			ReturnToBatter();
		}
	}
	
	private void Foul()
	{
		// todo this is not a correct implementation of baseball rules
		//Fouls += 1;
		
		//if ( Fouls >= 4 )
		//{
		//	Strike();	
		//}
		
		if ( Strikes < 2 )
		{
			Strike();	
		}
		else
		{
			ReturnToBatter();	
		}
	}
	
	private void CreateRunner()
	{
		// clone the batter into a runner todo variable mesh
		GameObject newRunner = (GameObject) Instantiate( GameObject.Find( "C_Piece1_" ) );
		PawnController newController = newRunner.AddComponent<PawnController>();
		newController.CrossedHomePlate += new PawnController.CrossedHomePlateDelegate( IncrementScore );
		newController.gameController = this.gameObject;
		
		// add new runner to the runner list
		Runners.Add( newRunner );
		
		// hide the batter
		HideBatter();
	}
	
	private void Hit( int bases )
	{
		// remove strikes, fouls
		Strikes = 0;
		//Fouls = 0;
		
		// for each runner, advance a base
		for ( int i = 0; i < Runners.Count; i ++ )
		{
			// advance runner
			GameObject runner = Runners[i];
			PawnController controller = runner.GetComponent<PawnController>();
			controller.Run();
			
		}
		
		// if runners should be advancing more than one base, loop back after time
		if ( bases > 1 )
		{	
			StartCoroutine( HitLooper( 2.5f, bases - 1 ) );
		}
		else
		{
			ReturnToBatter();	
		}
	}
	
	private IEnumerator HitLooper( float time, int bases )
	{
		yield return new WaitForSeconds( time );
		Hit( bases );
	}
	
	private void IncrementScore( GameObject sender )
	{
		if ( CurrentPlayer == 1 )
		{
			//HomeScore += 1;
			ScoreByInning[ Inning - 1 ][ 0 ] += 1;
			int length = ScoreByInning.Count;
			int totalScore = 0;
			for ( int i = 0; i < length; i ++ )
			{
				totalScore += ScoreByInning[ i ][ 0 ];	
			}
			HomeScore = totalScore;
			HomeScoreUpdated();
		}
		else
		{
			//VisitorScore += 1;
			ScoreByInning[ Inning - 1 ][ 1 ] += 1;
			int length = ScoreByInning.Count;
			int totalScore = 0;
			for ( int i = 0; i < length; i ++ )
			{
				totalScore += ScoreByInning[ i ][ 1 ];	
			}
			VisitorScore = totalScore;
			VisitorScoreUpdated();
		}
		
		Runners.Remove( sender );
		Destroy( sender );
	}
	
	private void EndSubInning()
	{
		if ( InningType == InningTypes.Top )
		{
			InningType = InningTypes.Bottom;
			Outs = 0;
			WaitBeforeDoing( 2f, new WaitBeforeDoingDelegate( ChangeTeams ) );
		}
		else 
		{
			// If there are more innings left OR the game is tied, make another inning
			if ( !(Inning + 1 > NumInnings) || HomeScore == VisitorScore )
			{
				InningType = InningTypes.Top;
				Inning += 1;
				Outs = 0;
				
				ScoreByInning.Add( new int[] { 0, 0 } );
				
				AdvancedAnInning();
				
				WaitBeforeDoing( 1f, new WaitBeforeDoingDelegate( ChangeTeams ) );
			}
			else
			{
				// end game	
				EndGame();
			}
		}
	}
	
	private void ChangeTeams()
	{
		HideBatter();
		
		while ( Runners.Count > 0 )
		{
			GameObject runner = Runners[0];
			
			Runners.Remove( runner );
			Destroy( runner );
		}
		
		if ( debugMode )
		{
			ScoreboardTextInstance.text = "( Changing Teams )";
		}
		
		if ( CurrentPlayer == 1 )
		{
			CurrentPlayer = 2;	
		}
		else
		{
			CurrentPlayer = 1;	
		}
		
		WaitBeforeDoing( 1f, new WaitBeforeDoingDelegate( ReturnToBatter ) );
	}
	
	private void EndGame()
	{
		State = States.Ended;
		string victoryString;
		if ( HomeScore > VisitorScore )
		{
			victoryString = HomeName + " Wins!";
		}
		else
		{
			victoryString = VisitorName + " Wins!";	
		}
		
		if ( debugMode )
		{
			ScoreboardTextInstance.text = "( Game Ended ) \n" + victoryString;
		}
	}
	
	// hack a generic loop back to the batter up phase that lasts 3 seconds
	private void ReturnToBatter()
	{
		WaitBeforeDoing( 3f, new WaitBeforeDoingDelegate( StartBatter ) );
	}
	
	public void WaitBeforeDoing( float time, WaitBeforeDoingDelegate function )
	{
		StartCoroutine( WaitBeforeDoingHelper( time, function ) );
	}
	
	internal IEnumerator WaitBeforeDoingHelper( float time, WaitBeforeDoingDelegate function )
	{
		yield return new WaitForSeconds( time );
		function();
	}
	
	// hack sets the scoreboard to whatever ResultString is 
	private void UpdateScoreboard( string newText )
	{
		if ( debugMode )
		{
			ScoreboardTextInstance.text = newText;
		}
	}
	
	// hack clears the scoreboard
	private void ClearScoreboard( object sender, EventArgs args )
	{
		if ( debugMode )
		{
			ScoreboardTextInstance.text = "";
			cameraController.StartedMoving -= new GameCamera.CameraEventHandler( ClearScoreboard );
		}
	}
	
	// TODO this is a debug shorthand method
	public void GetSomeDice()
	{
		if (!createdDice)
		{
			CreateDice( 2 );
			createdDice = true;
		}
	}
	
	/// <summary>
	/// Creates the dice.
	/// </summary>
	/// <param name='numDice'>
	/// Number of dice to create.
	/// </param>
	private void CreateDice( int numDice )
	{
		GameObject diceOrigin = GameObject.Find( "DiceSpawnPoint" );
		Vector3 spawnPosition = diceOrigin.transform.position;
		
		StartCoroutine( TimedCreateDice( 0.4f, numDice, Die, spawnPosition ) );
	}
	
	private IEnumerator TimedCreateDice( float interval, int numDice, GameObject dieTemplate, Vector3 position )
	{
		for (int i = 0; i < numDice; i++)
		{
			// set random forces
			Quaternion rotation = new Quaternion( UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range( 0f, 1f ), UnityEngine.Random.Range( 0f, 1f ), UnityEngine.Random.Range( 0f, 1f ) );
			Vector3 randomForce = new Vector3( UnityEngine.Random.Range( -2f, 2f ), 0f, UnityEngine.Random.Range( -2f, 2f ) );
			Vector3 randomRotation = new Vector3( UnityEngine.Random.Range( 1f, 30f ), UnityEngine.Random.Range( 1f, 30f ), UnityEngine.Random.Range( 1f, 30f ) );
			
			// pause, then instantiate die
			yield return new WaitForSeconds( interval );
			GameObject clonedDie = (GameObject) Instantiate( dieTemplate, position, rotation );
			clonedDie.rigidbody.useGravity = true;
			Dice.Add( clonedDie );
			
			// add forces
			clonedDie.rigidbody.AddForce( randomForce );
			clonedDie.rigidbody.AddTorque( randomRotation );
		}
	}
	
	private void ThrowDice( Vector3 force )
	{
		for ( int i = 0; i < Dice.Count; i ++ )
		{
			GameObject die = Dice[i];
			DieController controller = die.GetComponent<DieController>();
			controller.Throw( force );
		}
		
		State = States.Rolling;
	}
	
	private void AIThrowDice()
	{
		// make a random force
		Vector3 randomForce = new Vector3( UnityEngine.Random.Range( -5f, 5f ), UnityEngine.Random.Range( -5f, 5f ), UnityEngine.Random.Range( -5f, 5f ) );
		
		// throw the dice
		ThrowDice( randomForce );
	}
	
	private void MakeDiceThrowable( object sender, EventArgs args )
	{
		for ( int i = 0; i < Dice.Count; i ++ )
		{
			GameObject die = Dice[i];
			DieController controller = die.GetComponent<DieController>();
			controller.MakeThrowable();
		}
		
		HideBatter();
		State = States.WaitingForRoll;
		
		cameraController.FinishedMoving -= new GameCamera.CameraEventHandler( MakeDiceThrowable );
	}
}
