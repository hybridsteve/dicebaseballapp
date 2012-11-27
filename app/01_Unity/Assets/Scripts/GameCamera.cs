using UnityEngine;
using System;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	// store the main game view transforms
	private Vector3 GameViewPosition;
	private Quaternion GameViewRotation;
	private bool moving = false;
	
	public delegate void CameraEventHandler( object sender, EventArgs e );
	
	public event CameraEventHandler StartedMoving;
	public event CameraEventHandler FinishedMoving;
	
	public bool Moving
	{
		get
		{
			return moving;
		}
	}
	
	// store the dice view transforms (use reference camera position and rotation)
	private Vector3 DiceViewPosition;
	private Quaternion DiceViewRotation;
	
	private Vector3 TargetPosition;
	private Quaternion TargetRotation;
	
	// Use this for initialization
	void Start () {
		
		GameViewPosition = transform.position;
		GameViewRotation = transform.rotation;
		
		// we do this here because we don't need to keep a reference to the reference camera beyond this assignment
		GameObject ReferenceCamera = GameObject.Find( "ReferenceCamera" );
		DiceViewPosition = ReferenceCamera.transform.position;
		DiceViewRotation = ReferenceCamera.transform.rotation;
		
		this.StartedMoving += new CameraEventHandler( ReportStartedMoving );
		this.FinishedMoving += new CameraEventHandler( ReportFinishedMoving );
		
	}
	
	private void ReportStartedMoving( object obj, EventArgs e )
	{
		Debug.Log( "Camera reports that it has started moving" );	
	}
	
	private void ReportFinishedMoving( object obj, EventArgs e )
	{
		Debug.Log( "Camera reports that it has finished moving" );	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (moving)
		{
			// todo replace this with a nicer interpolation
			this.transform.position = Vector3.MoveTowards( this.transform.position, TargetPosition, 5f * Time.deltaTime );
			this.transform.rotation = Quaternion.RotateTowards( this.transform.rotation, TargetRotation, 50f * Time.deltaTime );
		}
		
		if (this.transform.position == TargetPosition && moving == true)
		{
			if (this.transform.rotation == TargetRotation)
			{
				moving = false;
				//Debug.Log( "moving stopped" );
				
				if (FinishedMoving != null)
				{
					FinishedMoving( this, EventArgs.Empty );	
				}
				
				GameController controller = (GameController) GameObject.Find( "GameController" ).GetComponent( typeof( GameController) );
				controller.GetSomeDice();
			}
		}
		
	}
	
	/// <summary>
	/// Move the camera to dice view.
	/// </summary>
	public void ZoomToDice()
	{
		if (!moving)
		{
			TargetPosition = DiceViewPosition;
			TargetRotation = DiceViewRotation;
			moving = true;
			
			if (StartedMoving != null)
			{
				StartedMoving( this, EventArgs.Empty );	
			}
		}
	}
	
	/// <summary>
	/// Zooms to field.
	/// </summary>
	public void ZoomToField()
	{
		if (!moving)
		{
			TargetPosition = GameViewPosition;
			TargetRotation = GameViewRotation;
			moving = true;
			
			if (StartedMoving != null)
			{
				StartedMoving( this, EventArgs.Empty );	
			}
		}
	}
}
