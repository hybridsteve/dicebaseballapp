// help


function displayHelpSection(helpSection){
	
	var ht ="";
	
	if(helpSection == "begin")
	{
		ht = "<h3>Begin or Continue Game</h3><p>Touch <em>Play</em> to start or continue a game</p><p>If there is a saved game, the <em>Continue</em> menu will appear.  Touch <em>Play Ball!</em> to resume your previous game or <em>New Game</em> to start a new game.</p>";
		
	}else if(helpSection == "players"){
		ht = "<h3>Choose the Number of Players</h3><p>Touch the die for either <em>One</em> or <em>Two</em> players.</p>";
	}else if(helpSection =="teamName"){
		ht ="<h3>Enter Team Name(s)</h3><p>Touch the text field(s) and enter your Team Name(s).</p><p>Touch the arrow in the bottom, right to continue.</p>";
	}else if(helpSection =="field"){
		ht = "<h3>Select Your Playing Field</h3><p>Preview the fields by touching the <em>Prev</em> and <em>Next</em> Dice at the bottom or by swiping left or right over the fields.</p><p>Once you have found the field you'd like to play on touch the <em>Play Ball</em> die.</p><p>To go back and edit your choices, touch the red arrow in the top, left corner.</p>";
	}else if(helpSection =="gameplay"){
		ht ="<h3>Gameplay</h3><p>The Guest Team plays first in single player games.</p><p>When your team is at bat, touch the dice in the middle of the screen or shake the device to roll.</p> <p>Your roll determines how you did at bat.</p><p>If you hit the ball, a game piece from your team will run to the appropriate base.  Your team gets a point for every game piece that gets around all four bases.  After two outs, the opposite team will be up at bat.</p> <p>After nine innings, the team with the most points wins the game.</p>";
	}else if(helpSection =="quitGame"){
		ht ="<h3>Save and Quit Game</h3><p>Open the <em>Options</em> menu in the lower right corner and Touch <em>Save & Quit</em>. </p>";
	}else if(helpSection == "sounds"){
		ht = "<h3>Game Sounds</h3><p>Muting is available in the in the lower right hand corner of the intro screen as well as in the <em>Options</em> menu during gameplay.</p>";
	}
	
	$("#pb_buttons").fadeOut(300, function(){
		$("#pb_content").html(ht).fadeIn(300);
		$("#btn_return").fadeIn(300);
	});
}
