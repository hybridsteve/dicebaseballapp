/**
 * @author rdelasalas
 */

//Audio Clips
var sndThemeSong = new Media("lib/audio/ballgame_full.mp3");
var sndPlayBall =  new Media("lib/audio/play_ball.mp3");
var sndYoureOut = new Media("lib/audio/out.mp3");
var sndCheerA = new Media("lib/audio/dbb_snd_hit-cheer-a.mp3");
var sndCheerB = new Media("lib/audio/dbb_snd_hit-cheer-b.mp3");
var sndCheerC = new Media("lib/audio/dbb_snd_hit-cheer-c.mp3");
var sndEnd = new Media("lib/audio/dbb_snd_end.mp3");
var sndSadA = new Media("lib/audio/dbb_snd_crowd-sad-a.mp3");
var sndSadB = new Media("lib/audio/dbb_snd_crowd-sad-b.mp3");
//Game vars
var currentPlayer = 2; //Start this at Player 2 becuase the "guest" team starts the game.
var setNumPlayers = 0;
var currentScore = 0; //Holds the score of the current players score
var inningCount; //The current inning variable
var numOuts = 0; //The current players number of outs
var playerOnFirst = false;
var playerOnSecond = false;
var playerOnThird = false;
var muteSounds = false;
var isEndOfInning = false;
var current_background = "sandlot";
var gameOver = false;
var gameContinue = false;


/* function for player */
/*function to set which player is up to bat*/
function atBat(){
	
	
	//The Home Team
	if(currentPlayer == 1)
	{
		//Swap the player piece 
		$(".gp").attr("src", "lib/img/_game/pieces/capPiece_blue_45.png");
			//baseballData.get('player_one', function(r){
		$("#lbl_batting").text("Now Batting: " + localStorage.getItem("player_one"));
		//});
	}else{
	//The Guest Team
		$(".gp").attr("src", "lib/img/_game/pieces/capPiece_red_45.png");
		//baseballData.get('player_two', function(r){
			$("#lbl_batting").text("Now Batting: "+ localStorage.getItem("player_two"));			
		//});
	}
}

function updateScore()
{
	if(currentPlayer == 2)
	{
		//Road Team
		var playerScore = parseInt($(".playerGuest").html()) + 1;
		$(".playerGuest").html(playerScore);
		
	}else if (currentPlayer == 1){
		//HomeTeam
		var playerScore = parseInt($(".playerHome").html()) + 1;
		$(".playerHome").html(playerScore);
		
	}
	
};

function diceroll()
{

	$("#roll").attr("src", "lib/img/_game/diceroll/btn_roll-dice_clicked.png");
	
	//alert("It is Player "+ currentPlayer +"'s turn.");
	//generate a random number for the first die
	var dice1 = generateRandom();
	var dice2 = generateRandom();
	var diceTotal = dice1 + dice2;
	var cutscene;
	var isHit;
	

	saveStatus();
	
	//Swap the image where the dice roll stopped;
	//$("#dice1").css('background-image', 'url(lib/img/dice_0'+dice1+'.png)');
	//$("#dice2").css('background-image', 'url(lib/img/dice_0'+dice2+'.png)');
	
	//Using an if or a case statement here lets set an image and baseball action based on
	//the dice roll result.
	
	$("#r_dice1").css('background-image', 'url(lib/img/_dice/die_back_r'+dice1+'.png)');
	$("#r_dice2").css('background-image', 'url(lib/img/_dice/die_front_r'+dice2+'.png)');
	
	if(diceTotal == 3 || diceTotal == 4)
	{	
		cutscene = "strikeout";
		//$("#dice_container").show();
		isAHit = false;
	}else if(diceTotal == 5 || diceTotal == 6){
		cutscene = "groundout";
		//$("#dice_container").show();
		isHit = false;
	}else if (diceTotal == 7 || diceTotal == 8){
		cutscene = "flyout";
		//$("#dice_container").show();
		isHit = false;
	}else if(diceTotal == 9){
		cutscene = "single";
		//$("#dice_container").show();
		isHit = true;
		//sndSingle.play();
		//runFirstBase();
	}else if(diceTotal ==10){
		cutscene = "double";
		//$("#dice_container").show();
		isHit = true;
		//sndDouble.play();
	}else if(diceTotal == 11){
		cutscene = "triple";
		//$("#dice_container").show();
		isHit = true;
		//sndTriple.play();
	}else if(diceTotal == 12 || diceTotal == 2){
		cutscene = "homerun";
		//$("#dice_container").show();
		isHit = true;
		//sndTriple.play();
	}
	result(cutscene);
	//checkBaseRunners(cutscene); 
	//checkBaseRunners(true, "double");

};


//This generates the value of the dice roll
function generateRandom(){
	//Generate the "Dice Roll"
	var randomnumber=Math.floor(Math.random()*6);
	//The index starts at 0, so I'm adding a 1 to the random number
	return randomnumber + 1;
}



function checkBaseRunners(hitType){
	
	/*check what the hit is, if the hit is an actual hit continue through this
	 * function.
	 * 
	 * Check to see if there is a base runner visible at each of the bases. If there is, advance the runner
	 * to the next base according to the current hit.
	 * 
	 */

	/*Check FirstBase
	 * if its a single and there is a runner on first move the guy on first to second
	 * if its a double and there is a runner on first move the guy on first to third
	 * if its a triple or a home run and there is a runner on first move the guy on first to home
	*/
	/*Check SecondBase
	 * if its a single and there is a runner on second do nothing
	 * if its a double, triple or homerun and there is a runner on second move the guy on second to ho	*/
	
	/*Check ThirdBase
	 * if its a single and there is a runner on third do nothing
	 * if its a double and there is a runner on third do nothing
	 * if its a triple or a home run move the runner on third to home
	 */

		
		if(hitType == "homerun")
		{
			//Send the grand slam cut scene if there are players on base
			if((playerOnFirst == true) && (playerOnSecond == true) && (playerOnThird == true))
			 {
				 hitType = "grandslam";
			 }
			
			
			if(playerOnFirst == true)
			{
				runFirstToHome();
			}
			
			if(playerOnSecond == true)
			{
				runSecondToHome();
			}
			
			if(playerOnThird == true)
			{	
				runHomePlate();
			}
		}else if (hitType == "triple")
		{
			if(playerOnFirst == true)
			{
				runFirstToHome();
			}
			
			if(playerOnSecond == true)
			{
				runSecondToHome();
			}
			
			if(playerOnThird == true)
			{	
				runHomePlate();
			}
		}else if (hitType == "double"){
			
			//Check Third Base First
			//Is there a guy on second
			if(playerOnThird == true)
			{
				//send him home if there is a guy on second.
				if(playerOnSecond == true)
				{
					runHomePlate();
				}else{
					//nothing
				}
			}
			
			//if there is a guy on second	
			if(playerOnSecond == true)
			{
				runSecondToHome();
			}
			//if there a guy on f
			if(playerOnFirst == true){
				runFirstToThird();
				
			}
			
		}else if (hitType == "single"){
			
			if(playerOnThird == true)
			{	
				if((playerOnSecond == true) && (playerOnFirst == true))
				{
					runHomePlate();
				}else{
					//nothing
				}
			}
			
			if(playerOnSecond == true)
			{
				if(playerOnFirst == true){
					runThirdBase();
				}else{
					//nothing
				}
			}
			
			if(playerOnFirst == true)
			{
				runSecondBase();
			}
		}
	
	advanceBatter(hitType);
	//result(hitType);
	//$('#dice1').hide();
	//$('#dice2').hide();
}

//Computer Turn
function checkComputerTurn()
{	
	//its a one player game and the current player is the computer
	//make the turn go automatically/
	
	if(setNumPlayers == 1 && currentPlayer == 2)
	{
		return true;
	}else{
		
		return false;
	}
}


function  startComputerTurn()
{
	$("#cutscene").css('background-image', 'url(lib/img/_game/atbat/atBat_guest.png)').fadeIn(300, function(){
		$("#cutscene").delay(2000).fadeOut(300,function()
		 {
			$("#roll_c").fadeIn(300);
			//The computer Auto Play. Need to figure out how to delay this.
			if(checkComputerTurn() == true)
			{
				
				setTimeout ( "diceroll()", 2000 );
				
				$("#btn_pause").fadeIn(300);
			}
		});	
	});
}

function  startGame(startPlayer)
{
	var batter;
	if(startPlayer == "p1")
	{
		batter = "home";
		
	}else{
		batter = "guest";
		
	}
	
	$("#cutscene").css('background-image', 'url(lib/img/_game/atbat/atBat_'+ batter +'.png)').fadeIn(300, function(){
		$("#cutscene").delay(2000).fadeOut(300);
		$("#roll").fadeIn(300);
		$("#btn_pause").fadeIn(300);
		watchForShake(1.5);
	});
}



//Tell the pawns where to go.
function advanceBatter(cutscene)
{
	if(cutscene == "single"){
		single(cutscene);
		
	}else if(cutscene =="double"){
		double(cutscene);
		
	}else if(cutscene =="triple"){
		triple(cutscene);
		
	}else if(cutscene =="homerun"){
		homeRun(cutscene);
		
	}else{
		processOuts(cutscene);
		
	}
	
}
function result(cutscene)
{
	if(cutscene == "single"){
		if(muteSounds == false){
			sndCheerA.play();
		}
	}else if(cutscene =="double"){
		if(muteSounds == false){
			sndCheerB.play();
		}
	}else if(cutscene =="triple"){
		if(muteSounds == false){
			sndCheerB.play();
		}
	}else if(cutscene =="homerun"){
		if(muteSounds == false){
			sndCheerC.play();
		}
	}else{
		if(muteSounds == false){
			//Select Random Sound"
			var randomsound=Math.floor(Math.random()*3);
			
			//alert(randomsound);
			if(randomsound == 0)
			{
				sndYoureOut.play();
			}else if(randomsound == 1){
				sndSadA.play();
			}else if(randomsound ==2){
				sndSadB.play();
			}
		}
	}
	
	$("#cutscene").css('background-image', 'url(lib/img/_game/cutscenes/'+cutscene+'.png)').fadeIn(300, function(){
			$("#dice_container").show();
			$("#cutscene").delay(3000).fadeOut(300,function(){
				//advanceBatter(cutscene);
				checkBaseRunners(cutscene);								   
			
											   
			});
	});
	
	saveStatus();

}
																								   
function gameAction(cutscene)
{	
	if(numOuts == 3 && currentPlayer == 2)
	{
		resetField(); //Reset the Field
				currentPlayer = 1; //Put Player One Back up To Bat
				numOuts = 0; //Reset the Outs
				if(inningCount >= 2 && checkResult() == "p1")
				{
					endgame();
					
				}else{
					//Show the Home Team At Bat Graphic
					$("#cutscene").css('background-image', 'url(lib/img/_game/atbat/atBat_home.png)').delay(2000).fadeIn(300 , function(){
						$("#cutscene").delay(1000).fadeOut(300);
						atBat(); //Change The Name at the Bottom of the Screen
						$('#roll_c').hide();
						$('#roll').attr("src", "lib/img/_game/diceroll/btn_roll-dice.png");
						$('#roll').fadeIn(300);
						watchForShake(1.5);
						
					});
				}
				
				
		
	}else if (numOuts == 3 && currentPlayer == 1){
		
											   
				resetField();
				currentPlayer = 2;
				numOuts = 0; //Reset the Outs
				if(inningCount >= 2 && (checkResult() == "p1" || checkResult() == "p2" ))
				{
					endgame();
					
				}else{
				//This updates the inning.
				//Grab the current inning from the box and add 1 to it.
				var updateInning = parseInt($(".inning").html()) + 1;
				inningCount = updateInning;
				$(".inning").html(updateInning);
				//Update the Inning in the display
				//Show the Inning Card
						$("#cutscene").css('background-image', 'url(lib/img/_game/innings/inning'+updateInning+'.png)').delay(2000).fadeIn(300 , function(){
							$("#cutscene").delay(2000).fadeOut(300, function(){
								//Show the Guest At Bat graphic.
								$("#cutscene").css('background-image', 'url(lib/img/_game/atbat/atBat_guest.png)').fadeIn(300 , function(){
									$("#cutscene").delay(1000).fadeOut(300);
									atBat(); //Change The Name at the Bottom of the Screen
									//Update the Inning
									//$(".inning").html(updateInning);
									storeLocal("current_inning", updateInning);
									//baseballData.save({key:'current_inning', gameInning: updateInning  });
									if(checkComputerTurn() == true)
									{
										$('#roll').fadeOut(300);
										$('#roll_c').show();
										setTimeout ( "diceroll()", 4000 );
									}else{
										$('#roll').fadeIn(300);
										$('#roll').attr("src", "lib/img/_game/diceroll/btn_roll-dice.png");
									}
								});
							});
						});
				}
		
	}else{
		//The computer Auto Play. 
					if(checkComputerTurn() == true)
					{
						$('#roll').fadeOut(300);
						$('#roll_c').show();
						setTimeout ( "diceroll()", 2000 );
					}else{
						if(inningCount >= 2 && (checkResult() == "p1"))
						{
							endgame();
						}else{
							$('#roll_c').hide();
							$('#roll').fadeIn(300);
							$("#roll").attr("src", "lib/img/_game/diceroll/btn_roll-dice.png");
							watchForShake(1.5);
						}
					}
					
					
		
	}
}
function endgame()
{
	
	if(muteSounds == false)
		sndEnd.play();
	currentPlayer = 0;
	//Get the score from the scoreboard
	////Guest
	var score2 = parseInt($(".playerGuest").html());
	//Home
	var score1 = parseInt($(".playerHome").html());
	var homeT;
	var guestT;
	
	var gameWinner;
	
	//alert(score2 +": Guest :: "+ score1+": Home");
	
	$("#final_guest").html(score2);
	$("#final_home").html(score1);
	
	//baseballData.all(function(r)
	//{
		homeT = localStorage.getItem("player_one")+ " Wins!";
		guestT = localStorage.getItem("player_two") + " Wins!";
		
		if(score1 > score2)
		{
			$("#final_winner").html(homeT);
		}else{
			$("#final_winner").html(guestT);
		}
	//});
	$("#final_score").html("Guest : " + score2 +" | Home : " + score1);

	$("#endgame").css('background-image', 'url(lib/img/_game/end/dBaseball-endgame.png)').fadeIn(600, function(){
		$("#endgame").delay(4000).animate({"left": '0', "top": '+=240'}, 4000, function(){
			$("#final_winner").fadeIn(400, function(){
				$("#final_score").delay(1000).fadeIn(400, function(){
					$("#btn_end").delay(5000).fadeIn(300);
					
				});
				
			});
		});
	});
}


function processOuts(cutscene)
{
	if(numOuts == 0){
		$("#out_1").show();
		numOuts += 1;
	}else if(numOuts == 1){
		$("#out_2").show();
		numOuts += 1;
	}else if(numOuts == 2){
		numOuts += 1;
		//checkInning();
	}
	//result(cutscene);
	gameAction(cutscene);
}


function checkResult()
{
	//Guest
	var score2 = parseInt($(".playerGuest").html());
	//Home
	var score1 = parseInt($(".playerHome").html());
	var winner;
	if(score2 > score1)
	{
		//player 2 wins - Guest Team //
		winner = "p2";
		
		return winner;
	
	}else if(score1 > score2)
	{	
		//player 1 wins - Home Team //
		winner = "p1";
		return winner;
	
	}else if(score1 == score2){
		
		winner = "tie";
		return winner;
	
	}
	
}


/* The hits keep on coming! */
//Animate
function single(cutscene)
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show();
		playerOnFirst = true;
		//result(cutscene);
		gameAction(cutscene);
	});
}

//Animate player going from home to first
function runFirstBase(){
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show();
		playerOnFirst = true;
		
	});
	
}

//Animate player going from first to second
function runSecondBase()
{
	playerOnFirst = false;
	$("#p2").animate({"left": '-=96', "top": '-=94'}, 900, function(){
		$('#p2').removeAttr('style').hide();
		$('#p3').show();
		playerOnSecond = true;
	});
}

//Animate player going from first to third
function runFirstToThird()
{
	playerOnFirst = false;
	$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
		$('#p2').removeAttr('style').hide();
		$('#p3').show().animate({"left": '-=103', "top": '+=95'}, 900, function(){
			$('#p3').removeAttr('style').hide();
			$('#p4').show();
			playerOnThird = true;
		});	
	});
}

//Animate player going from first to home
function runFirstToHome()
{
	
	playerOnFirst = false;
	$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
		$('#p2').removeAttr('style').hide();
		$('#p3').show().animate({"left": '-=103', "top": '+=95'}, 900, function(){
			$('#p3').removeAttr('style').hide();
			$('#p4').show().animate({"left": '+=103', "top": '+=120'}, 900, function(){
				updateScore();
				$('#p4').removeAttr('style').hide(); 
				
			});
		});	
	});

}

//Animate player going from second to third
function runThirdBase()
{
	playerOnSecond = false;
	$("#p3").animate({"left": '-=103', "top": '+=95'}, 900, function(){
		$('#p3').removeAttr('style').hide();
		$('#p4').show();
		playerOnThird = true;
		
	});
}


//Animate player going from second to home
function runSecondToHome()
{
	playerOnSecond = false;
	$('#p3').show().animate({"left": '-=103', "top": '+=95'}, 900, function(){
		$('#p3').removeAttr('style').hide();
		$('#p4').show().animate({"left": '+=103', "top": '+=120'}, 900, function(){
			updateScore();
			$('#p4').removeAttr('style').hide(); 
		});
	});	

}

//Animate player going from third to home
function runHomePlate(){

	playerOnThird = false;
	$("#p4").animate({"left": '+=103', "top": '+=120'}, 900, function(){
		updateScore();
		$('#p4').removeAttr('style').hide(); 
	});
}

function double(cutscene)
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
			$('#p2').removeAttr('style').hide();
			$('#p3').show();
			playerOnSecond = true;
			//result(cutscene);
			gameAction(cutscene);
		});
	});
}


function triple(cutscene)
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
			$('#p2').removeAttr('style').hide();
			$('#p3').show().animate({"left": '-=103', "top": '+=95'}, 900, function(){
				$('#p3').removeAttr('style').hide();
				$('#p4').show();
				playerOnThird = true;
				//result(cutscene);
				gameAction(cutscene);
			});	
		});
	});
}



function homeRun(cutscene)
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
			$('#p2').removeAttr('style').hide();
			$('#p3').show().animate({"left": '-=103', "top": '+=95'}, 900, function(){
				$('#p3').removeAttr('style').hide();
				$('#p4').show().animate({"left": '+=103', "top": '+=120'}, 900, function(){
					updateScore();
					$('#p4').removeAttr('style').hide(); 
				
					//result(cutscene);
					gameAction(cutscene);
				});
			});	
		});
	});
}



//Reset the field for the next player. Clear and LOB's and Reset the Outs on the Scoreboard
function resetField()
{
	if( $("#p1").is(":visible") )
	{
		$("#p1").fadeOut(200);
	}
	
	if( $("#p2").is(":visible") )
	{
		$("#p2").fadeOut(200);
	}
	
	if( $("#p3").is(":visible") )
	{
		$("#p3").fadeOut(200);
	}
	
	if( $("#p4").is(":visible") )
	{
		$("#p4").fadeOut(200);
	}
	
	if( $("#out_1").is(":visible") )
	{
		$("#out_1").fadeOut(200);
	}
	
	if( $("#out_2").is(":visible") )
	{
		$("#out_2").fadeOut(200);
	}
	
	$('#roll_c').fadeOut(200);
	$('#roll').fadeOut(200);
	$('#dice_container').hide();
	playerOnFirst = false;
	playerOnSecond = false;
	playerOnThird = false;
	numOuts = 0;
	
	
}


//Reset the field for the next player. Clear and LOB's and Reset the Outs on the Scoreboard
function resetGame()
{
	/*Hide all the Game Pieces */
	if( $("#p1").is(":visible") )
	{
		$("#p1").fadeOut(200);
	}
	
	if( $("#p2").is(":visible") )
	{
		$("#p2").fadeOut(200);
	}
	
	if( $("#p3").is(":visible") )
	{
		$("#p3").fadeOut(200);
	}
	
	if( $("#p4").is(":visible") )
	{
		$("#p4").fadeOut(200);
	}
	
	if( $("#out_1").is(":visible") )
	{
		$("#out_1").fadeOut(200);
	}
	
	if( $("#out_2").is(":visible") )
	{
		$("#out_2").fadeOut(200);
	}
	/*hide the dice buttons */
	$('#roll_c').fadeOut(200);
	$('#roll').fadeOut(200);
	//reset the endgame cutscene
	//$('#endgame').css('position', 'absolute');
	//$('#endgame').css('top', '-240px');
	/*Hide the dice container */
	$('#dice_container').hide();
	/*reset the scores */
	$(".playerGuest").html("0");
	$(".playerHome").html("0");
	//reset the inning.
	$(".inning").html("1");
	/*clear the bases*/
	playerOnFirst = false;
	playerOnSecond = false;
	playerOnThird = false;
	/* reset the variables */
	numOuts = 0;
	currentPlayer = 2;
	$("input#txtPlayeOne").attr('readonly', 'false');
	$("input#txtPlayeTwo").attr('readonly', 'false');
	$('#name_stage').removeClass(function(){
		return $(this).prev().attr('class');					 
	});
	if(muteSounds ==false){
		sndEnd.stop();
	}
	$("#btn_end").hide();
	
}



function changeField(field)
{
	//confirm("You have changed the field view.");
	$("#game").css('background-image', 'url(lib/img/_game/fields/dBaseball-'+field+'.png)');
	
	if(field == "sandlot"){
		
		$("#name_stage").addClass('barSandlot');
		//$("#matchup").addClass('barSandlot');
		//$("#roll").addClass('barSandlot');
		//$("#roll_c").addClass('barSandlot');
		
	
	}else if(field == "littleLeague")
	{
		$("#name_stage").addClass('barLittle');
		//$("#matchup").addClass('barLittle');
		//$("#roll").addClass('barLittle');
		//$("#roll_c").addClass('barLittle');
		
	}else if(field =="minorLeague"){
		
		$("#name_stage").addClass('barMinor');
		//$("#matchup").addClass('barMinor');
		//$("#roll").addClass('barMinor');
		//$("#roll_c").addClass('barMinor');
		
	}else if(field == "stadium"){
		
		$("#name_stage").addClass('barStadium');
		//$("#matchup").addClass('barStadium');
		//$("#roll").addClass('barStadium');
		//$("#roll_c").addClass('barStadium');
		
	}
	
	if(gameContinue == true)
	{
		inningCount = parseInt($(".inning").html());
		
	}else{
		inningCount = 1;
	}
	
		
		if(checkComputerTurn() == true)
		{
			jQT.goTo("#game", "slide");
			$("#matchup").fadeIn(300, function(){
				$("#matchup").delay(4000).fadeOut(300, function(){
					if(muteSounds == false){
						sndPlayBall.play();
					}
					$("#cutscene").css('background-image', 'url(lib/img/_game/innings/inning'+ inningCount+'.png)').fadeIn(300 , function(){
						$("#cutscene").delay(2000).fadeOut(300, function(){
								
								
								startComputerTurn();
								//$('#roll_c').fadeIn(200);
								
						});
					});
				});
				atBat();
			});
			$("#roll").hide();
			
		}else{
			
			jQT.goTo("#game", "slide");
			$("#matchup").fadeIn(300, function(){
				$("#matchup").delay(4000).fadeOut(300, function(){
					if(muteSounds == false){
						sndPlayBall.play();
					}
					$("#cutscene").css('background-image', 'url(lib/img/_game/innings/inning'+ inningCount+'.png)').fadeIn(300 , function(){
						$("#cutscene").delay(2000).fadeOut(300, function(){
								if(currentPlayer == 2)
								{
									startGame("p2");
								}else{
									startGame("p1");
								
								}
						});
					});
				});
				atBat();
			});
		//$("#roll").show();
		}
	
	

}

/* This function saves the current game data game data*/
function saveStatus(){
	storeLocal("current_inning", parseInt($(".inning").html()));
	//baseballData.save({key:'current_inning', gameInning: parseInt($(".inning").html())  });
	storeLocal("guest", parseInt($(".playerGuest").html()));
	//baseballData.save({key:'guest', scoreGuest: parseInt($(".playerGuest").html()) });
	storeLocal("home", parseInt($(".playerHome").html()));
	//baseballData.save({key:'home', scoreHome: parseInt($(".playerHome").html()) });
	storeLocal("outs", numOuts);
	//baseballData.save({key:'outs', currentOuts: numOuts });
	if(currentPlayer == 1){
		var inningHalf = "bottom";
	}else{
		var inningHalf = "top";
	}
	storeLocal("inningHalf", inningHalf);
	//baseballData.save({key:'inningHalf', inHalf: inningHalf });
	//alert(playerOnFirst);
	storeLocal("first", playerOnFirst);
	//baseballData.save({key:'first', first: playerOnFirst });
	//alert(playerOnSecond);
	storeLocal("second", playerOnSecond);
	//baseballData.save({key:'second', second: playerOnSecond });
	//alert(playerOnThird);
	storeLocal("third", playerOnThird);
	//baseballData.save({key:'third', third: playerOnThird });
	storeLocal("current_bkg", current_background);
	//baseballData.save({key:'current_bkg', bkg: current_background  });
	storeLocal("num_players", setNumPlayers);
	//baseballData.save({key:'num_players', nump: setNumPlayers  });
}


/*This function takes care of a swipe on the field select */
function swipeFields(direction)
{
	if(direction == 'right'){
		if(current_background == "sandlot")
		{
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-majorLeague.png)');
			current_background ="stadium";
		}else if(current_background == "stadium")
		{
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-minorLeague.png)');
			current_background ="minorLeague";
		} else if(current_background == "minorLeague")
		{
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-littleLeague.png)');
			current_background ="littleLeague";
		}else if(current_background == "littleLeague")
		{
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-sandlot.png)');
			current_background ="sandlot";
		}
	}else if(direction =='left'){
		if(current_background == "sandlot")
		{
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-littleLeague.png)');
			current_background ="littleLeague";
		}else if(current_background == "littleLeague")
		{
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-minorLeague.png)');
			current_background ="minorLeague";
		} else if(current_background == "minorLeague"){
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-majorLeague.png)');
			current_background ="stadium";
		}else if(current_background == "stadium"){
			$("#choose").css('background-image', 'url(lib/img/_choose/dBaseball-sel-sandlot.png)');
			current_background ="sandlot";
		}
	}
	
	
}

	
	



