/**
 * @author rdelasalas
 */

var currentPlayer = 1; //The player who's turn is currently happening
var numberOfPlayers = 2;
var currentScore = 0; //Holds the score of the current players score
var inningCount; //The current inning variable
var numOuts = 0; //The current players number of outs
var playerOnFirst = false;
var playerOnSecond = false;
var playerOnThird = false;
var muteGameSounds = false;
var current_background = "sandlot";


/* function for player */
/*function to set which player is up to bat*/
function atBat(){
	
	if(currentPlayer == 1)
	{
		//Swap the player piece 
		$(".gp").attr("src", "lib/img/_game/pieces/capPiece_red_45.png");
		baseballData.get('player_one', function(r){
			$("#lbl_batting").text("Now Batting: " + r.plyr1);
		});
		
		
	}else{
		$(".gp").attr("src", "lib/img/_game/pieces/capPiece_blue_45.png");
		baseballData.get('player_two', function(r){
			$("#lbl_batting").text("Now Batting: "+ r.plyr2);
		});
		
	}
}

function updateScore()
{
	if(currentPlayer == 1)
	{
		//Road Team
		var playerScore = parseInt($(".player1").html()) + 1;
		$(".player1").html(playerScore);
	
	}else if (currentPlayer == 2){
		
		//HomeTeam
		var playerScore = parseInt($(".player2").html()) + 1;
		$(".player2").html(playerScore);
	}
};

function diceroll()
{
	//$('#dice_roll').show().sprite({fps: 12, no_of_frames: 23}).active();
	
	
	//alert("It is Player "+ currentPlayer +"'s turn.");
	//generate a random number for the first die
	var dice1 = generateRandom();
	var dice2 = generateRandom();
	var diceTotal = dice1 + dice2;
	var cutscene;
	var isHit;
	
	//The SoundEffecs
	/* uncomment these for the XCode Compile
	var sndSingle = new Media("audio/single.mp3");
	var sndDouble = new Media("audio/double.mp3");
	var sndTriple = new Media("audio/triple.mp3");
	var sndHomeRun = new Media("audio/homer.mp3");
	var sndWhiff = new Media("audio/whiff.mp3");
	var sndOut = new Media("audio/out.mp3"); */
	
	
	
	//Swap the image where the dice roll stopped;
	$("#dice1").css('background-image', 'url(lib/img/dice_0'+dice1+'.png)');
	$("#dice2").css('background-image', 'url(lib/img/dice_0'+dice2+'.png)');
	
	//Using an if or a case statement here lets set an image and baseball action based on
	//the dice roll result.
	
	if(diceTotal == 3 || diceTotal == 4)
	{	
		cutscene = "strikeout";
		isAHit = false;
	}else if(diceTotal == 5 || diceTotal == 6){
		cutscene = "groundout";
		isHit = false;
	}else if (diceTotal == 7 || diceTotal == 8){
		cutscene = "flyout";
		isHit = false;
	}else if(diceTotal == 9){
		cutscene = "single";
		isHit = true;
		//sndSingle.play();
		//runFirstBase();
	}else if(diceTotal ==10){
		cutscene = "double";
		isHit = true;
		//sndDouble.play();
	}else if(diceTotal == 11){
		cutscene = "triple";
		isHit = true;
		//sndTriple.play();
	}else if(diceTotal == 12 || diceTotal == 2){
		cutscene = "homerun";
		isHit = true;
		//sndTriple.play();
	}
	
	
	checkBaseRunners(cutscene); 
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
		
		if(hitType == "homerun")
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
		
		/*Check SecondBase
		 * if its a single and there is a runner on second do nothing
		 * if its a double, triple or homerun and there is a runner on second move the guy on second to ho	*/
		
		/*Check ThirdBase
		 * if its a single and there is a runner on third do nothing
		 * if its a double and there is a runner on third do nothing
		 * if its a triple or a home run move the runner on third to home
		 */
	
	
	
	$("#cutscene").css('background-image', 'url(lib/img/_game/cutscenes/'+hitType+'.png)').show("slow", function(){
		
		$('#dice_roll').hide();
		$("#cutscene").delay(1000).hide("slow", advanceBatter(hitType));
		
	});
	
	
}



//Tell the pawns where to go.
function advanceBatter(cutscene)
{
	
	if(cutscene == "single"){
		single();
	}else if(cutscene =="double"){
		double();
	}else if(cutscene =="triple"){
		triple();
	}else if(cutscene =="homerun"){
		homeRun();
	}else{
		
		if(numOuts == 0){
			$("#out_1").show();
			numOuts += 1;
		}else if(numOuts == 1){
			$("#out_2").show();
			numOuts += 1;
		}else if(numOuts == 2){
			numOuts = 0;
			//Inning Change
			if(currentPlayer == 1){
				
				resetField();
				currentPlayer = 2;
				atBat();
			}else{
				resetField();
				currentPlayer = 1;
				atBat();
				//This updates the inning.
				var updateInning = parseInt($(".inning").html()) + 1;
				
				if(updateInning > 9){
					if(checkResult() == true){
						$("#popup_gameover").fadeIn(300);
					}else{
						
					}
				}else{
						$("#inningcard").css('background-image', 'url(lib/img/_game/innings/inning'+updateInning+'.png)').fadeIn(300 , function(){
							$("#inningcard").delay(2000).fadeOut(500);
							//Update the Inning
							$(".inning").html(updateInning);
						});
				}
				
			}
		}
		
	}

}


function checkResult()
{
	
	var score2 = parseInt($(".player2").html());
	var score1 = parseInt($(".player1").html());

	if((score2 != score1))
	{
		return true;
	}else{
		return false;
		
	}
	
}


/* The hits keep on coming! */
//Animate
function single()
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show();
		playerOnFirst = true;
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











function double()
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
			$('#p2').removeAttr('style').hide();
			$('#p3').show();
			playerOnSecond = true;
		});
	});
}


function triple()
{
	$("#p1").show().animate({"left": '+=95', "top": '-=120'}, 900, function(){
		$('#p1').removeAttr('style').hide();
		$('#p2').show().animate({"left": '-=96', "top": '-=94'}, 900, function(){
			$('#p2').removeAttr('style').hide();
			$('#p3').show().animate({"left": '-=103', "top": '+=95'}, 900, function(){
				$('#p3').removeAttr('style').hide();
				$('#p4').show();
				playerOnThird = true;
			});	
		});
	});
}



function homeRun()
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
		$("#p1").hide();
	}
	
	if( $("#p2").is(":visible") )
	{
		$("#p2").hide();
	}
	
	if( $("#p3").is(":visible") )
	{
		$("#p3").hide();
	}
	
	if( $("#p4").is(":visible") )
	{
		$("#p4").hide();
	}
	
	if( $("#out_1").is(":visible") )
	{
		$("#out_1").hide();
	}
	
	if( $("#out_2").is(":visible") )
	{
		$("#out_2").hide();
	}
	
	playerOnFirst = false;
	playerOnSecond = false;
	playerOnThird = false;
	
}



function changeField(field)
{
	//confirm("You have changed the field view.");
	$("#game").css('background-image', 'url(lib/img/_game/fields/dBaseball-'+field+'.png)');
	
	if(field == "sandlot"){
		
		$("#name_stage").addClass('barSandlot');
	
	}else if(field == "littleLeague")
	{
		$("#name_stage").addClass('barLittle');
		
	}else if(field =="minorLeague"){
		
		$("#name_stage").addClass('barMinor');
		
	}else if(field == "stadium"){
		
		$("#name_stage").addClass('barStadium');
		
	}
	
	
}
	
	



