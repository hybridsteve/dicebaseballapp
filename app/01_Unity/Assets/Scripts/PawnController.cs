using UnityEngine;
using System.Collections;

public class PawnController : MonoBehaviour {

	public enum runnerPositions : int { Null=0, First=1, Second=2, Third=3, Home=4 };
	private runnerPositions runnerPosition = runnerPositions.Null;
	public runnerPositions RunnerPosition
	{
		get { return runnerPosition; }
	}
	
	public delegate void CrossedHomePlateDelegate( GameObject sender );
	public event CrossedHomePlateDelegate CrossedHomePlate;
	
	public GameObject gameController;
	private GameObject[] bases;
	
	// No longer needed.
	//private bool moving = false;
	
	private Vector3 targetPosition;
	
	void Awake()
	{
		bases = new GameObject[4];
		bases[0] = GameObject.Find( "FirstBase");
		bases[1] = GameObject.Find( "SecondBase");
		bases[2] = GameObject.Find( "ThirdBase");
		bases[3] = GameObject.Find( "HomeBase");
	}
	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// Don't need this anymore using iTween for more efficiency.
//		if (moving)
//		{
//			this.transform.position = Vector3.MoveTowards( this.transform.position, targetPosition, 5f * Time.deltaTime );	
//		}
//		
//		if (this.transform.position == targetPosition && moving == true)
//		{
//			moving = false;
//			if ( runnerPosition == runnerPositions.Home )
//			{
//				CrossedHomePlate( this.gameObject );
//			}
//		}
	}
	
	/// <summary>
	/// Run this instance to the next base.
	/// </summary>
	public void Run()
	{
		// TODO: Might want to put the bases in an array in the Start method to make everything quicker.
		// 		 Also adding them to a layer in the editor will make finding them easier since Unity caches
		//		 the objects per layer.
		
		
		switch ( runnerPosition )
		{
		case runnerPositions.Null:
			// run to first base
			runnerPosition = runnerPositions.First;
			
			break;
			
		case runnerPositions.First:
			// run to second base
			runnerPosition = runnerPositions.Second;
			
			break;
			
		case runnerPositions.Second:
			// run to third base
			runnerPosition = runnerPositions.Third;
			
			break;
			
		case runnerPositions.Third:
			// run home
			runnerPosition = runnerPositions.Home;
			
			break;
			
		default: 
			break;
		}
		
		targetPosition = bases[ (int)runnerPosition - 1].transform.position;
		
		Hashtable animParams = iTween.Hash( "position", targetPosition, "time", 1.25f, "easetype", "easeInOutCubic", "looktarget", targetPosition );
		if( runnerPosition == runnerPositions.Home )
		{
			//animParams.Add( "oncompletetarget", gameController );
			//animParams.Add( "oncomplete", "IncrementScore" );
			//animParams.Add( "oncompleteparams", this.gameObject );
			
			animParams.Add( "oncomplete", "FireCrossedHomeEvent" );
		}
		else
		{
			animParams.Add( "oncomplete", "lookAtNextBase" );
		}
		
		iTween.MoveTo( gameObject, animParams );
	}
	
	void lookAtNextBase()
	{
		iTween.LookTo( this.gameObject, iTween.Hash( "looktarget", bases[ (int)runnerPosition ].transform.position, "axis", "y", "time", 1.0f, "easetype", "easeOutCubic" ) );
	}
	
	void FireCrossedHomeEvent()
	{
		CrossedHomePlate( this.gameObject );
	}
}
