using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIStartMenu : MonoBehaviour {
	
	public UIToolkit uiTools;
	public UIToolkit uiTextBlack;
	public UIToolkit uiTextWhite;
	
	private UISprite stadiumBG;
	
	private UIText graph15White, graph20, graph20Center, graph26, graph26White, graph36;
	
	UIButton backButton;
	
	// Parent objects
	private UIObject landingParent, startScreenParent, playerNameParent, fieldSelectParent, instructionsParent, statsParent;
	
	private UIObject currentScreen;
	
	// Start Screen Objects
	UIScrollableHorizontalLayout currentGamesLayout;
	//UIButton howToPlayButton;
	
	// TODO: TEMPORARY!!!!! Get the json object from somewhere else to mantain MVC. Proof of concpet for now.
	public TextAsset jsonGames;
	public TextAsset jsonStats;
	
	// Text Input
	private bool playerNameInput = false;
	private string nameInputText = "Player Name";
	GUIStyle textInputStyle;
	float textInputWidth;
	float textInputHeight;
	float textInputPosX;
	float textInputPosY;
	
	// Use this for initialization
	private void Start ()
	{
		// Create the UIText objects needed here.
		graph15White = new UIText( uiTextWhite, "Graphite15", "Graphite15.png" );
		graph26 = new UIText( uiTextBlack, "Graphite26", "Graphite26.png" );
		graph20 = new UIText( uiTextBlack, "Graphite20", "Graphite20.png" );
		graph20Center = new UIText( uiTextBlack, "Graphite20", "Graphite20.png" );
		graph26White = new UIText( uiTextWhite, "Graphite26", "Graphite26.png" );
		graph36 = new UIText( uiTextBlack, "Graphite36", "Graphite36.png" );
		
		graph20Center.alignMode = UITextAlignMode.Center;
		graph26.alignMode = UITextAlignMode.Center;
		
		// Create the panel parents
		landingParent = new UIObject();
		
		// Create the custom style for the text input
		textInputStyle = new GUIStyle();
		textInputStyle.name = "textInputStyle";
		textInputStyle.fontSize = 20;
		textInputStyle.alignment = TextAnchor.LowerCenter;
		
		textInputWidth = 220;
		GUIUtils.hd( ref textInputWidth );
		textInputHeight = 55;
		GUIUtils.hd( ref textInputHeight );
		textInputPosX = Screen.width / 2 - textInputWidth / 2;
		textInputPosY = Screen.height / 2 - textInputHeight / 2 + 15;
		
		// Call the creation methods
		createLandingScreen();
		createStartScreen();
		createPlayerNameScreen();
		createFieldSelectScreen();
		createInstructionsScreen();
		createStatsScreen();
		
		// Create the Back Button
		backButton = UIButton.create( uiTools, "backButton.png", "backButtonDown.png", 0, 0 );
		hideShowBackButton(false);
		backButton.onTouchUpInside += (sender) => backPressed();
	}
	
	private void OnGUI()
	{
		if( playerNameInput )
		{
			GUILayout.BeginArea( new Rect( textInputPosX, textInputPosY, textInputWidth, textInputHeight ) );
			nameInputText = GUILayout.TextArea( nameInputText, textInputStyle );
			GUILayout.EndArea();
		}
	}
	
	#region Layout Creation
	private void createLandingScreen()
	{	
		// Create the panel parent
		landingParent = new UIObject();
		landingParent.client.name = "LandingScreen";
		
		stadiumBG = uiTools.addSprite( "stadiumBG.png", 0, 0, 20 );
		
		UISprite titleSprite = uiTools.addSprite( "title.png", 0, 0, 20 );
		titleSprite.pixelsFromTop( 28 );
		
		int buttonsBottomOffset = 130;
		
		// Start Button
		UIButton startButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		startButton.pixelsFromBottomLeft( buttonsBottomOffset , 13 );
		
		UITextInstance startText = graph26.addTextInstance( " START ", 0, 0 );
		startText.parentUIObject = startButton;
		startText.pixelsFromTopRight( 52, 0 );
		
		// Options Button
		UIButton optionsButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		optionsButton.pixelsFromBottomRight( buttonsBottomOffset, 50 );
		
		UITextInstance optionsText = graph20Center.addTextInstance( "OPTIONS", 0, 0 );
		optionsText.parentUIObject = optionsButton;
		optionsText.pixelsFromTopRight( 56, 5 );
		
		// Parent everything to the landingParent for moving
		UIObject[] objects = { titleSprite, startButton, startText, optionsButton, optionsText };
		foreach (UIObject item in objects)
		{
			item.parentUIObject = landingParent;
		}
		
		// Set button actions
		startButton.onTouchUpInside += (sender) => showStartScreen();

	}
	
	private void createStartScreen()
	{
		// Create the parent
		startScreenParent = new UIObject();
		startScreenParent.client.name = "StartScreen";
		
		int buttonsBottomOffset = 130;
		
		// Single Player Button
		UIButton singlePlayerButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		singlePlayerButton.pixelsFromBottomLeft( buttonsBottomOffset, 13 );
		
		UITextInstance singlePlayerText = graph20Center.addTextInstance( "Single\nPlayer", 0, 0 );
		singlePlayerText.parentUIObject = singlePlayerButton;
		singlePlayerText.pixelsFromTopRight( 45, 24 );
		
		// Multi Player Button
		UIButton multiPlayerButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		multiPlayerButton.pixelsFromBottomRight( buttonsBottomOffset, 50 );
		
		UITextInstance multiPlayerText = graph20Center.addTextInstance( "Multi\nPlayer", 0, 0 );
		multiPlayerText.parentUIObject = multiPlayerButton;
		multiPlayerText.pixelsFromTopRight( 45, 24 );
		
		// How To Play Button
		UIButton howToPlayButton = UIButton.create( uiTools, "howToPlayButton.png", "howToPlayButton.png", 0, 0, 10 );
		howToPlayButton.pixelsFromBottomLeft( 0, 5 );
		howToPlayButton.highlightedTouchOffsets = new UIEdgeOffsets( 25 );
		
		// Stats Button
		UIButton statsButton = UIButton.create( uiTools, "statsButton.png", "statsButton.png", 0, 0, 10 );
		statsButton.pixelsFromBottomRight( -10, 0 );
		
		// Current Games
		UISprite currentGamesBG = uiTools.addSprite( "currentGamesBG.png", 0, 0, 15 );
		currentGamesBG.pixelsFromTop( 45 );
		
		// Current Games Scroller
		currentGamesLayout = new UIScrollableHorizontalLayout( 10 );
		currentGamesLayout.setSize( Screen.width, GUIUtils.hdReturn( 90 ) );
		
		currentGamesLayout.parentUIObject = currentGamesBG;
		currentGamesLayout.pixelsFromBottom( 8 );
		
		// Get the current games
		findCurrentGames();
		
		// Parent everything to the startScreenParent
		UIObject[] objects = { singlePlayerButton, singlePlayerText, multiPlayerButton, multiPlayerText, howToPlayButton, statsButton, currentGamesBG, currentGamesLayout };
		foreach (UIObject item in objects)
		{
			item.parentUIObject = startScreenParent;
		}
		
		// Set button actions
		singlePlayerButton.onTouchUpInside += (sender) => showPlayerNameScreen();
		multiPlayerButton.onTouchUpInside += (sender) => showPlayerNameScreen();
		howToPlayButton.onTouchUpInside += (sender) => showInstructionsScreen();
		statsButton.onTouchUpInside += (sender) => showStatsScreen();
		
		// Move the panel off screen
		startScreenParent.positionFromTopLeft( 0, 1 );
	}
	
	private void createPlayerNameScreen()
	{
		// Parent Object for Player Name Screen
		playerNameParent = new UIObject();
		playerNameParent.client.name = "PlayerNameScreen";
		
		// Play Button
		UIButton playButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		playButton.pixelsFromBottomRight( 43, 25 );
		
		// Play Button Text
		UITextInstance playText = graph26.addTextInstance( "PLAY", 0, 0 );
		playText.parentUIObject = playButton;
		playText.pixelsFromTopRight( 56, 15 );
		
		// Player Name Text Field Background
		UISprite textFieldBackground = uiTools.addSprite( "white.png", 0, 0, 10 );
		
		textFieldBackground.setSize( textInputWidth, textInputHeight );
		textFieldBackground.positionCenter();
		
		// Parent everything to the parent object.
		UIObject[] objects = { playButton, playText, textFieldBackground };
		foreach (UIObject item in objects)
		{
			item.parentUIObject = playerNameParent;
		}
		
		// Set the button actions
		playButton.onTouchUpInside += (sender) => showFieldSelectScreen();
		
		// Move the panel off screen
		playerNameParent.positionFromTopLeft( 0, 2 );
	}
	
	private void createFieldSelectScreen()
	{
		fieldSelectParent = new UIObject();
		fieldSelectParent.client.name = "FieldSelectScreen";
		
		// Field Name Title
		UITextInstance fieldName = graph26White.addTextInstance( "Field Name", 0, 0 );
		fieldName.pixelsFromTop( 10 );
		
		// Field Select Arrow Buttons
		UIButton leftArrow = UIButton.create( uiTools, "arrowFieldSelectLeft.png", "arrowFieldSelectLeft.png", 0, 0, 10 );
		UIButton rightArrow = UIButton.create( uiTools, "arrowFieldSelectRight.png", "arrowFieldSelectRight.png", 0, 0, 10 );
		
		leftArrow.pixelsFromLeft( 5 );
		rightArrow.pixelsFromRight( 5 );
		
		// Play Ball Button
		UIButton playBallButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		// TODO: The numbers are temp until final design is done.
		playBallButton.pixelsFromBottomRight( 77, 115 );
		
		// Play Ball Text
		UITextInstance playBallText = graph26.addTextInstance( "Play\nBall", 0, 0 );
		playBallText.parentUIObject = playBallButton;
		playBallText.pixelsFromTopRight( 45, 28 );
		
		// Parent everything to the fieldSelectParent
		UIObject[] objects = { fieldName, leftArrow, rightArrow, playBallButton, playBallText };
		foreach( UIObject item in objects )
		{
			item.parentUIObject = fieldSelectParent;
		}
		
		// Move everything off screen
		fieldSelectParent.positionFromTopLeft( 0, 3 );
	}
	
	private void createInstructionsScreen()
	{
		// Create the instructionsParent
		instructionsParent = new UIObject();
		instructionsParent.client.name = "InstructionsScreen";
		
		// Title Text
		UITextInstance titleText = graph36.addTextInstance( "Instructions", 0, 0 );
		titleText.pixelsFromTop( 25 );
		
		// Instructions vertical layout
		float layoutWidth = 260f;
		GUIUtils.hd( ref layoutWidth );
		
		UIVerticalLayout instructionsLayout = new UIVerticalLayout( 0 );
		instructionsLayout.beginUpdates();
		
		string tempText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus et lorem et sem vulputate malesuada.";
		
		// Create the instructions list
		for( int i = 0; i < 4; i++ )
		{
			UISprite dieBullet = uiTools.addSprite( "dieBulletPoint.png", 0, 0, 10 );
			UITextInstance instructionText = graph20.addTextInstance( tempText, 0, 0 );
			UISprite textWrapper = GUIUtils.createTextWrapper( uiTools, instructionText, layoutWidth, false );
			instructionsLayout.addChild( textWrapper );
			dieBullet.parentUIObject = textWrapper;
			dieBullet.pixelsFromLeft( -(int)(dieBullet.width + 20) );
		}
		instructionsLayout.endUpdates();
		instructionsLayout.matchSizeToContentSize();
		instructionsLayout.pixelsFromTopLeft( 75, 75 );
		
		// Parent all objects to the instructionsParent
		UIObject[] objects = { titleText, instructionsLayout };
		foreach( UIObject item in objects )
		{
			item.parentUIObject = instructionsParent;
		}
		
		// Move off screen
		instructionsParent.positionFromTopLeft( 0, 4 );
	}
	
	private void createStatsScreen()
	{
		// Create the parent object
		statsParent = new UIObject();
		statsParent.client.name = "StatsScreen";
		
		UISprite statsSheet = uiTools.addSprite( "statsSheet.png", 0, 0, 10 );
		statsSheet.positionFromBottomRight( 0, 0 );
		
		int baseOffset = 18;
		int statOffset = 40;
		UITextInstance[] statNames = new UITextInstance[5];
		for( int i = 1; i <= 5; i++  )	
		{
			string stat = "Statistic " + i;
			UITextInstance statName = graph20.addTextInstance( stat, 0, 0 );
			statName.parentUIObject = statsSheet;
			statName.pixelsFromTopLeft( baseOffset + (statOffset * i), 25 );
			statName.parentUIObject = statsParent;
			statNames[i-1] = statName;
		}
		
		// Get the stats
		string[] stats = getStats();
		
		for( int i = 0; i < statNames.Length; i++)
		{
			UITextInstance stat = graph20.addTextInstance( stats[i], 0, 0 );
			stat.parentUIObject = statNames[i];
			stat.pixelsFromLeft( 200 );
			stat.parentUIObject = statsParent;
		}
		
		// Parent the objects to the parent
		statsSheet.parentUIObject = statsParent;
		
		// Move off screen
		statsParent.positionFromTopLeft( 0, 5 );
	}
	
	#endregion
	
	private void AddGamesToLayout(params List<Game>[] games)
	{
		foreach(List<Game> gameList in games)
		{
			foreach( Game game in  gameList )
			{	
				// Create the button
				UIButton gameButton = UIButton.create( uiTools, "currentGame.png", "currentGame.png", 0, 0, 10 );
				
				string turn = "Your Turn";
				if( !game.playerTurn )
				{
					turn = "Their Turn";
				}
				
				// Turn text
				UITextInstance gameTurn = graph15White.addTextInstance( turn, 0, 0 );
				gameTurn.parentUIObject = gameButton;
				gameTurn.pixelsFromTop( 2 );
				
				// Opponent Name text
				UITextInstance opponentName = graph15White.addTextInstance( game.opponentName, 0, 0 );
				opponentName.parentUIObject = gameButton;
				opponentName.pixelsFromBottom( 2 );
				
				// Score Text
				string score = game.playerScore + "/" + game.opponentScore;
				UITextInstance scoreText = graph26White.addTextInstance( score, 0, 0 );
				scoreText.parentUIObject = gameButton;
				GUIUtils.centerText( scoreText );
				
				currentGamesLayout.addChild( gameButton );
			}
		}
	}
	
	// TODO: Placeholder code until implementation
	private void findCurrentGames()
	{
		// TODO: The jsonRaw variable is temporary right now. Get it from the server and mantain MVC.
		// TODO: Getting the stats here too. Probably not necessary to get them from here.
		GameInfo games = new GameInfo( jsonGames.text, jsonStats.text );
		
		List<Game> playerTurnGames = new List<Game>();
		List<Game> opponentTurnGames = new List<Game>();
		
		foreach( var pair in games.currentGames )
		{
			Game game = pair.Value;
			
			if( game.playerTurn )
			{
				playerTurnGames.Add(game);
			}
			else
			{
				opponentTurnGames.Add( game );
			}
		}
		
		// TODO: You're repeating yourself.
		// Organize the games by date.
		playerTurnGames.Sort( (a,b) => DateTime.Compare( a.date, b.date ) );
		opponentTurnGames.Sort( (a,b) => DateTime.Compare( a.date, b.date ) );
		
		AddGamesToLayout( playerTurnGames, opponentTurnGames );
		
		// TODO: This is temporary just for testing...
		Debug.Log( "TEMP DELETE THIS BLOCK" );
		games.updateLocalGames();
		games.updateLocalStats();
	}
	
	// TODO: Placeholder code until implementation
	private string[] getStats()
	{
		string[] stats = new string[5];
		
		for( int i = 0; i < 5; i++ )
		{
			string randStat = ( Math.Truncate( (double)(UnityEngine.Random.Range( 0f, 1f )) * 1000.0 ) / 1000.0 ).ToString();

			if( randStat.Length < 5 )
			{
				int zeroAdd = 5 - randStat.Length;
				for( int j = 0; j < zeroAdd; j++ )
				{
					randStat += "0";
				}
			}
			stats[i] = randStat;
		}
		
		return stats;
	}
	
	#region showHide Screens
	
	private void hideShowBackButton( bool show )
	{
		if( show )
		{
			backButton.pixelsFromTopLeft( 5, 5 );
		}
		else
		{
			backButton.positionFromTopLeft( 0, -1 );
		}
	}
	
	private void showStadium()
	{
		stadiumBG.positionFromTopLeft( 0, 0 );
	}
	
	private void hideStadium()
	{
		stadiumBG.positionFromTopLeft( 0, -2 );
	}
	
	// TODO: This is temp but I want different methods for each screen so individual animations can be done.
	private void showStartScreen()
	{
		landingParent.positionFromTopLeft( 0, -1 );
		startScreenParent.positionFromTopLeft( 0, 0 );
		hideShowBackButton(true);
		currentScreen = startScreenParent;
	}
	
	// TODO: This is temp but I want different methods for each screen so individual animations can be done.
	private void showPlayerNameScreen()
	{
		// Move the startScreen off the screen.
		// TODO: This should have a method so we can apply it to all the screens maybe?
		startScreenParent.positionFromTopLeft( 0, -2 );
		playerNameParent.positionFromTopLeft( 0, 0 );
		
		currentScreen = playerNameParent;
		
		// TODO: There should be a dynamic way to turn this on and off.
		playerNameInput = true;
	}
	
	private void showFieldSelectScreen()
	{
		// Move the player name screen off view
		playerNameParent.positionFromTopLeft( 0, -2 );
		
		// TODO: This is temp should be more dynamic
		playerNameInput = false;
		
		fieldSelectParent.positionFromTopLeft( 0, 0 );
		
		currentScreen = fieldSelectParent;
	}
	
	private void showInstructionsScreen()
	{
		startScreenParent.positionFromTopLeft( 0, -2 );
		instructionsParent.positionFromTopLeft( 0, 0 );
		hideStadium();
		
		currentScreen = instructionsParent;
	}
	
	private void showStatsScreen()
	{
		Debug.Log( "1" );
		startScreenParent.positionFromTopLeft( 0, 1 );
		Debug.Log( "2" );
		Debug.Log( statsParent.client.name );
		statsParent.positionFromTopLeft( 0, 0 );
		Debug.Log( "3" );
		hideStadium();
		Debug.Log( "4" );
		currentScreen = statsParent;
		Debug.Log( "5" );
	}
	
	// TODO: All of this should use the methods above and create the missing ones that will use the animations.
	private void backPressed()
	{
		switch( currentScreen.client.name )
		{
			case "StartScreen":
				startScreenParent.positionFromTopLeft( 0, 1 );
				landingParent.positionFromTopLeft(0, 0);
				currentScreen = landingParent;
				hideShowBackButton( false );
				break;
			case "PlayerNameScreen":
				playerNameParent.positionFromTopLeft( 0, 2 );
				startScreenParent.positionFromTopLeft( 0, 0 );
				currentScreen = startScreenParent;
				playerNameInput = false;
				break;
			case "FieldSelectScreen":
				fieldSelectParent.positionFromTopLeft( 0, 3 );
				playerNameParent.positionFromTopLeft( 0, 0 );
				currentScreen = playerNameParent;
				playerNameInput = true;
				break;
			case "InstructionsScreen":
				instructionsParent.positionFromTopLeft( 0, 4 );
				startScreenParent.positionFromTopLeft( 0, 0 );
				currentScreen = startScreenParent;
				showStadium();
				break;
			case "StatsScreen":
				statsParent.positionFromTopLeft( 0, 5 );
				startScreenParent.positionFromTopLeft( 0, 0 );
				currentScreen = startScreenParent;
				showStadium();
				break;
			default:
				Debug.Log( "Can't find the current screen for back button." );
				break;
		}
	}
	
	#endregion
}







































