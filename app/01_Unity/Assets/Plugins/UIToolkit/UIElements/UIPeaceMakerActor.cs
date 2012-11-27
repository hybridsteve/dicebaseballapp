using UnityEngine;
using System.Collections.Generic;


public class UIPeaceMakerActor : UIAbstractContainer
{
	//variables
	private UISprite _actorBackground;
	private UISprite _actorGradient;
	private UIButton _theButton;
	private UISprite _actorArrow;
	private float _attitude = 1f;
	private string actorName;
	
	public static Color actorRed = new Color(252f/255f, 61f/255f, 12f/255f, 1f);
	public static Color actorYellow = new Color(254f/255f, 246f/255f, 0f, 1f);
	public static Color actorBlue = new Color(11f/255f, 95f/255f, 253f/255f, 1f);
	
	private Rect _touchFrame;
	public Rect touchFrame { get { return _touchFrame; } }
	
	private UISpriteAnimation _arrowUpAnim;
	private UISpriteAnimation _arrowDownAnim;
		
	
	//constructor
	public UIPeaceMakerActor (UIToolkit toolkit, string actorName, string fileName)
	{
		//todo make this scalable
		_height = 50; //background image size
		_width = 50;
		
		_touchFrame = new Rect ( position.x, -position.y, _width, _height );
		
		
		//background image is always the same
		_actorBackground = toolkit.addSprite("actorBG.png", 0, 0);
		_actorBackground.parentUIObject = this;
		_actorBackground.positionFromTopLeft (0f, 0f);
		
		
		//gradient is always the same
		_actorGradient = toolkit.addSprite ("actor_gradient_white.png", 0, 0);
		_actorGradient.parentUIObject = this;
		_actorGradient.positionFromTopLeft (0f, 0f);
		
		
		//image and button
		_theButton = UIButton.create (toolkit, fileName, fileName, 0, 0);
		_theButton.parentUIObject = this;
		_theButton.onTouchUpInside += ( sender ) =>  randomAttitudeTest ();
		
		
		//up/down arrow is always the same
		_actorArrow = toolkit.addSprite ("up_arrow_anim_00.png", 0, 0);
		_actorArrow.parentUIObject = this;
		_actorArrow.positionFromTopRight (0f, 0f);
		
		_arrowUpAnim = _actorArrow.addSpriteAnimation ("arrowUpAnim", 0.1f, "up_arrow_anim_00.png", "up_arrow_anim_01.png", "up_arrow_anim_02.png", "up_arrow_anim_03.png", "up_arrow_anim_04.png", "up_arrow_anim_05.png", "up_arrow_anim_06.png", "up_arrow_anim_07.png", "up_arrow_anim_08.png", "up_arrow_anim_09.png");
		_arrowDownAnim = _actorArrow.addSpriteAnimation ("arrowDownAnim", 0.1f, "down_arrow_anim_00.png", "down_arrow_anim_01.png", "down_arrow_anim_02.png", "down_arrow_anim_03.png", "down_arrow_anim_04.png", "down_arrow_anim_05.png", "down_arrow_anim_06.png", "down_arrow_anim_07.png", "down_arrow_anim_08.png", "down_arrow_anim_09.png");
		_arrowUpAnim.loopReverse = true;
		_arrowDownAnim.loopReverse = true;
		
		hideAttitudeIndicators ();
	}
	
	//todo remove this method for final
	public void randomAttitudeTest ()
	{
		var attitude = Random.Range (0.0f, 1.0f);
		updateAttitude (attitude);
	}
	
	/// <summary>
	/// Updates the attitude. This default method shows the arrow.
	/// </summary>
	/// <param name='newAttitude'>
	/// FLOAT: New attitude.
	/// </param>
	public void updateAttitude (float newAttitude)
	{
		updateAttitude (newAttitude, true);	
	}
	
	/// <summary>
	/// Updates the attitude. Show arrow variable is exposed so that it can be disabled.
	/// </summary>
	/// <param name='newAttitude'>
	/// FLOAT: New attitude.
	/// </param>
	/// <param name='showArrow'>
	/// BOOL: Whether or not to show the arrow.
	/// </param>
	public void updateAttitude (float newAttitude, bool showArrow)
	{
		//variable represents positive or negative change. determines arrow direction.
		var delta = (newAttitude >= _attitude)?"positive":"negative";
		delta = (newAttitude == _attitude)?"neutral":delta; //todo one-line this
		
		_attitude = newAttitude;
		
		
		_actorGradient.beginUpdates ();
		
		//set gradient position and color
		//TODO: make scalable for hd, 50f is height of actor
		var newPosition = (50f - (50f * newAttitude)) - 25f;
		var newPositionInt = (int)newPosition;
		
		var newColor = Color.Lerp (Color.Lerp(actorRed, actorYellow, newAttitude * 2f), actorBlue, (newAttitude - 0.5f)*2);
		
		_actorGradient.pixelsFromTopLeft (newPositionInt, 0);
		_actorGradient.color = newColor;
		_actorGradient.endUpdates ();
		
		//clip gradient
		clipChild (_actorGradient);
		
		if (showArrow)
		{
			//show up or down arrow
			//manages arrow animation states
			if (delta != "neutral") 
			{
				_actorArrow.hidden = false;
				
				//var anim = (delta == "positive" && !_arrowUpAnim.isPlaying)?_arrowUpAnim:_arrowDownAnim;
				if (delta == "positive" && !_arrowUpAnim.isPlaying) 
				{
					if (_arrowDownAnim.isPlaying) 
					{
						_arrowDownAnim.stop ();	
					}
					_actorArrow.playSpriteAnimation (_arrowUpAnim, -1);	
				}
				else if (delta == "negative" && !_arrowDownAnim.isPlaying)
				{
					if (_arrowUpAnim.isPlaying)
					{
						_arrowUpAnim.stop ();	
					}
					_actorArrow.playSpriteAnimation (_arrowDownAnim, -1);	
				}
			}
			else 
			{
				_actorArrow.hidden = true;	
			}
		}
	}
		
	/// <summary>
	/// Hides the attitude indicators.
	/// </summary>
	public void hideAttitudeIndicators ()
	{
		//hide up or down arrow
		_actorArrow.hidden = true;
	}
	
	/// <summary>
	/// Clips the actor gradient image.
	/// </summary>
	/// <param name='child'>
	/// Child. The UISprite to clip (should be the gradient)
	/// </param>
	private void clipChild (UISprite child)
	{
		var topContained = child.localPosition.y <= -touchFrame.yMin && child.localPosition.y >= -touchFrame.yMax;
		var bottomContained = child.localPosition.y - child.height <= -touchFrame.yMin && child.localPosition.y - child.height >= -touchFrame.yMax;
		
		child.clipped = false; //hack this forces the full sprite before clipping it, needed to prevent major change in position from wrecking sprite clipping.

		// first, handle if we are fully visible
		if( topContained && bottomContained )
		{
			// unclip if we are clipped
			if( child.clipped )
				child.clipped = false;
			child.hidden = false;
		}
		else if( topContained || bottomContained )
		{
			// wrap the changes in a call to beginUpdates to avoid changing verts more than once
			child.beginUpdates();

			child.hidden = false;

			// are we clipping the top or bottom?
			if( topContained ) // clipping the bottom
 			{
				var clippedHeight = child.localPosition.y + touchFrame.yMax - 3; //TODO: 3 should be scalable based on hd

				child.uvFrameClipped = child.uvFrame.rectClippedToBounds( child.width / child.scale.x, clippedHeight / child.scale.y, UIClippingPlane.Bottom, child.manager.textureSize );
				child.setClippedSize( child.width / child.scale.x, clippedHeight / child.scale.y, UIClippingPlane.Bottom );
			}
			else // clipping the top, so we need to adjust the position.y as well
 			{
				var clippedHeight = child.height - child.localPosition.y - touchFrame.yMin - 3; //TODO: 3 should be scalable based on hd
				Debug.Log (clippedHeight);

				child.uvFrameClipped = child.uvFrame.rectClippedToBounds( child.width / child.scale.x, clippedHeight / child.scale.y, UIClippingPlane.Top, child.manager.textureSize );
				child.setClippedSize( child.width / child.scale.x, clippedHeight / child.scale.y, UIClippingPlane.Top );
			}
			
			child.updateTransform ();
			child.updateVertPositions ();
			// commit the changes
			child.endUpdates();
		}
		else
		{
			// fully outside bounds
			child.hidden = true;
		}

		// Recurse
		//recurseAndClipChildren( child ); //hack actors do not need this
	}
}