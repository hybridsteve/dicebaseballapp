using UnityEngine;
using System.Collections;

public class UIStartMenu : MonoBehaviour {
	
	public UIToolkit uiTools;
	public UIToolkit uiTextBlack;
	public UIToolkit uiTextWhite;
	
	private UISprite stadiumBG;
	
	private UIText graph15White, graph20, graph26, graph26White;
	
	// Parent objects
	private UIObject landingParent, startScreenParent;
	
	// Start Screen Objects
	UIScrollableHorizontalLayout currentGamesLayout;
	
	// TODO: TEMPORARY!!!!! Get the json object from somewhere else t mantain MVC. Proof of concpet for now.
	public TextAsset jsonRaw;
	
	// Use this for initialization
	void Start () 
	{
		// Create the UIText objects needed here.
		graph15White = new UIText( uiTextWhite, "Graphite15", "Graphite15.png" );
		graph26 = new UIText( uiTextBlack, "Graphite26", "Graphite26.png" );
		graph20 = new UIText( uiTextBlack, "Graphite20", "Graphite20.png" );
		graph26White = new UIText( uiTextWhite, "Graphite26", "Graphite26.png" );
		
		graph20.alignMode = UITextAlignMode.Center;
		graph26.alignMode = UITextAlignMode.Center;
		
		// Create the panel parents
		landingParent = new UIObject();
		startScreenParent = new UIObject();
		
		// Call the creation methods
		createLandingScreen();
		createStartScreen();
		
		
	}
	
	private void createLandingScreen()
	{	
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
		
		UITextInstance optionsText = graph20.addTextInstance( "OPTIONS", 0, 0 );
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
		int buttonsBottomOffset = 130;
		
		// Single Player Button
		UIButton singlePlayerButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		singlePlayerButton.pixelsFromBottomLeft( buttonsBottomOffset, 13 );
		
		UITextInstance singlePlayerText = graph20.addTextInstance( "Single\nPlayer", 0, 0 );
		singlePlayerText.parentUIObject = singlePlayerButton;
		singlePlayerText.pixelsFromTopRight( 45, 24 );
		
		// Multi Player Button
		UIButton multiPlayerButton = UIButton.create( uiTools, "dieButtonLarge.png", "dieButtonLargeDown.png", 0, 0, 10 );
		multiPlayerButton.pixelsFromBottomRight( buttonsBottomOffset, 50 );
		
		UITextInstance multiPlayerText = graph20.addTextInstance( "Multi\nPlayer", 0, 0 );
		multiPlayerText.parentUIObject = multiPlayerButton;
		multiPlayerText.pixelsFromTopRight( 45, 24 );
		
		// How To Play Button
		UIButton howToPlayButton = UIButton.create( uiTools, "howToPlayButton.png", "howToPlayButton.png", 0, 0, 10 );
		howToPlayButton.pixelsFromBottomLeft( 0, 5 );
		
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
		
		// TODO: Add temp games
		findCurrentGames();
		
		// Parent everything to the startScreenParent
		UIObject[] objects = { singlePlayerButton, singlePlayerText, multiPlayerButton, multiPlayerText, howToPlayButton, statsButton, currentGamesBG, currentGamesLayout };
		foreach (UIObject item in objects)
		{
			item.parentUIObject = startScreenParent;
		}
		
		// Move the panel off screen
		startScreenParent.positionFromTopLeft( 0, 1 );
	}
	
	// TODO: Placeholder code until implementation
	// TODO: Organize the games by your turn then their turn...
	private void findCurrentGames()
	{
		// TODO: The jsonRaw variable is temporary right now. Get it from the server and mantain MVC.
		GameInfo games = new GameInfo( jsonRaw.text );
		
		foreach( var pair in  games.currentGames )
		{
			Game game = pair.Value;
			
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
	
	// TODO: This is temp but I want different methods for each screen so individual animations can be done.
	private void showStartScreen()
	{
		landingParent.positionFromTopLeft( 0, -1 );
		startScreenParent.positionFromTopLeft( 0, 0 );
	}
	
}
