using UnityEngine;
using System.Collections;

public class ScoreboardController : MonoBehaviour {

	// game objects that store the x and y positions of the physical scoreboard positions
	private Vector3 column1, column2, column3, column4, row2, row3, row4;
	// instantiable gameobjects that represent glyphs
	public GameObject inn0, inn1, inn2, inn3, inn4, inn5, inn6, inn7, inn8, inn9;
	public GameObject stateWaiting, stateStrike, stateFlyOut, stateGroundOut;
	
	// fields that will contain live gameobjects (find and replace them when you need to update the scoreboard)
	private GameObject StatusMessage;
	
	// Use this for initialization
	void Start () {
		// Find the physical start location of the scoreboard
		column1 = GameObject.Find( "column1" ).transform.position;
		column2 = GameObject.Find( "column2" ).transform.position;
		column3 = GameObject.Find( "column3" ).transform.position;
		column4 = GameObject.Find( "column4" ).transform.position;
		row2 = GameObject.Find( "row2" ).transform.position;
		row3 = GameObject.Find( "row3" ).transform.position;
		row4 = GameObject.Find( "row4" ).transform.position;
		
		// hack test: set the value of the inning tiles
		GameObject currentTile = (GameObject) GameObject.Instantiate( inn1 );
		currentTile.transform.position = column1;
		
		currentTile = (GameObject) GameObject.Instantiate( inn2 );
		currentTile.transform.position = column2;
		
		currentTile = (GameObject) GameObject.Instantiate( inn3 );
		currentTile.transform.position = column3;
		
		// test set the initial scores
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column1.x, row2.y, row2.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column2.x, row2.y, row2.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column3.x, row2.y, row2.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column4.x - 0.7f, row2.y, row2.z );
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column4.x + 0.7f, row2.y, row2.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column1.x, row3.y, row3.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column2.x, row3.y, row3.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column3.x, row3.y, row3.z );
		
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column4.x - 0.7f, row3.y, row3.z );
		currentTile = (GameObject) GameObject.Instantiate( inn0 );
		currentTile.transform.position = new Vector3( column4.x + 0.7f, row3.y, row3.z );
		
		SetStatus( 0 );
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
