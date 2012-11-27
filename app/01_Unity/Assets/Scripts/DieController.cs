using UnityEngine;
using System.Collections;

public class DieController : MonoBehaviour {
	
	public enum States : int { BeforeRoll=1, Rolling=2, Rolled=3 };
	private States state = States.BeforeRoll;
	public States State { 
		get { return state; } 
	}
	private bool debugMode = false;
	
	private int dieValue = 0;
	public int DieValue {
		get { return dieValue; }
	}
	
	public Material staticMaterial, rollingMaterial, rolledMaterial;
	
	private int FreezeTimer = 0;
	private bool Frozen = false;
	
	//private Mesh myMesh;
	private MeshRenderer myRenderer;
	
	// Use this for initialization
	void Start () {
		
		// find the renderer component so that we can change materials (helps debug)
		myRenderer = (MeshRenderer) GetComponentInChildren(typeof(MeshRenderer));
		
		GameObject gameControllerObject = GameObject.Find( "GameController" );
		GameController gameController = gameControllerObject.GetComponent<GameController>();
		debugMode = gameController.DebugMode;
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		// If the die has been thrown and user stops shaking device, we need to watch for die stopping
		if ( state == States.Rolling && Input.acceleration.magnitude < 1 && Frozen == true)
		{
			// measure the die velocity
			if ( rigidbody.velocity.sqrMagnitude == 0 && rigidbody.angularVelocity.sqrMagnitude == 0 )
			{
				Debug.Log( "dice met resting condition" );
				state = States.Rolled;
				StateUpdated();
				
				//get the result
				dieValue = GetRollResult();
				
				Debug.Log( dieValue );
				
				UnFreeze();
			}
		}
		
		// this is to keep the die from instantly being read
		if ( FreezeTimer == 0 && Frozen == false && State == States.Rolling )
		{
			//FreezeTimer -= 1;
			Frozen = true;
			//Spin();
		}
		
		if ( FreezeTimer > 0  && State == States.Rolling )
		{
			FreezeTimer -= 1;
			Frozen = false;
		}
		
	}
	
	private void OnCollisionEnter()
	{
		//Debug.Log( "collided" );	
	}
	
	/// <summary>
	/// Gets the roll result.
	/// </summary>
	/// <returns>
	/// The roll result.
	/// </returns>
	private int GetRollResult()
	{
		int result = CalcSideUp();
		// todo if result is 0, we need to either re-throw die or prompt user to rethrow (bad roll)
		return result;
	}
	
	private void UnFreeze()
	{
		Frozen = false;
	}
	
	private int CalcSideUp() {
    	float dotFwd = Vector3.Dot(transform.forward, Vector3.up);
    	if (dotFwd > 0.99f) return 1;
    	if (dotFwd < -0.99f) return 6;

    	float dotRight = Vector3.Dot(transform.right, Vector3.up);
    	if (dotRight > 0.99f) return 4;
    	if (dotRight < -0.99f) return 3;

    	float dotUp = Vector3.Dot(transform.up, Vector3.up);
    	if (dotUp > 0.99f) return 5;
    	if (dotUp < -0.99f) return 2;

    	return 0;
    }
	
	public void Throw( Vector3 force )
	{
		Throw( force, 1f );	
	}
	
	public void Throw( Vector3 force, float multiplier )
	{
		if ( state != States.Rolled && Frozen == false )
		{
			this.rigidbody.AddForce( force * multiplier );
			
			Spin();
			
			state = States.Rolling;
			FreezeTimer = 20;
			Frozen = false;
			StateUpdated();
		}
		
	}
	
	/// <summary>
	/// Used to add some spin to the final throw.
	/// </summary>
	public void Spin()
	{
		Vector3 randomTorque = new Vector3( Random.Range( -10f, 10f ), Random.Range( -10f, 10f ), Random.Range( -10f, 10f ) );
		randomTorque.Normalize();
		randomTorque *= 100;
		
		this.rigidbody.AddTorque( randomTorque );
	}
	
	public void MakeThrowable()
	{
		state = States.BeforeRoll;
		StateUpdated();
	}
	
	private void StateUpdated()
	{
		if (debugMode == false)
		{
			return;	
		}
		
		if (state == States.BeforeRoll)
		{
			myRenderer.renderer.material = staticMaterial;	
		}
		else if (state == States.Rolling)
		{
			myRenderer.renderer.material = rollingMaterial;
		}
		else
		{
			myRenderer.renderer.material = rolledMaterial;	
		}
	}
}
