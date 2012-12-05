using UnityEngine;
using System.Collections;

public static class GUIUtils
{
	#region Text
	
	public static void centerText(UITextInstance txt, int pixelOffset)
	{
		centerText( txt, pixelOffset, 0 );
	}
	
	public static void centerText(UITextInstance txt)
	{
		centerText( txt, 0, 0 );
	}
	
	public static void centerText( UITextInstance txt, int pixelOffset, int buttonOffset )
	{
		float parentHeight = txt.parentUIObject.height * txt.parentUIObject.scale.y;
		float height = txt.height;
		
		float parentWidth = txt.parentUIObject.width * txt.parentUIObject.scale.x;
		float width = txt.width;
		
		hd( ref pixelOffset );
		hd( ref buttonOffset );
		
		if (pixelOffset == 0 && buttonOffset == 0)
		{
			/* Commenting this out. It is not centering correctly for some reason
			txt.verticalAlignMode = UITextVerticalAlignMode.Middle;
			txt.positionCenter();
			*/
			txt.verticalAlignMode = UITextVerticalAlignMode.Middle;
			int offsetTop = (int)(parentHeight/2) - (int)(height/2);
			if( offsetTop < 0 )
			{
				offsetTop *= -1;
			}
			txt.pixelsFromTop( offsetTop, 0 );
		}
		else
		{
			txt.verticalAlignMode = UITextVerticalAlignMode.Top;
			int offsetTop = (int)(( parentHeight - height )/2) - pixelOffset;
			
			if( offsetTop < 0 )
			{
				offsetTop *= -1;
			}
			
			//txt.pixelsFromTop(offset);
			txt.pixelsFromTopLeft( offsetTop, (int)((parentWidth - width)/2) + buttonOffset );
		}
	}
	
	public static void fitTextHorizontally( UITextInstance text )
	{
		fitTextHorizontally( text, 0, text.parentUIObject );
	}
	
	/// <summary>
	/// Fits the text horizontally if it's bigger than its parent.
	/// </summary>
	public static void fitTextHorizontally( UITextInstance text, int sideOffset )
	{
		fitTextHorizontally( text, sideOffset, text.parentUIObject );
	}
	
	public static void fitTextHorizontally( UITextInstance text, int sideOffset, UIObject fitter )
	{
		float fitterWidth = fitter.width * fitter.scale.x;
		
		// If the text is smaller then we're done.
		if( !textNeedsWrap( text, fitter, sideOffset ) )
		{
			return;
		}
		
		// Border space is so the text doesn't end at the edge of the containing object.
		int borderSpace = 5;
		hd( ref borderSpace );
		
		setTextWrap( text, fitterWidth - borderSpace );
	}
	
	public static bool textNeedsWrap( UITextInstance text, UIObject fitter, int sideOffset )
	{
		float fitterWidth = fitter.width * fitter.scale.x;
		
		// If the text is smaller then we're done.
		if( text.width < fitterWidth - sideOffset )
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	public static void setTextWrap( UITextInstance text, float lineWrapWidth )
	{
		text._parentText.wrapMode = UITextLineWrapMode.MinimumLength;
		text._parentText.lineWrapWidth = lineWrapWidth;
		
		// Re-adding the text to itself, to work around a bug.
		// TODO: See if there is a better way to do this. Also the wrapping is not working 100% well
		// some lines are being wrapped early. This could be because we are getting the text from a string instead
		// of a file. Something that I encountered with my arabic text testing.
		string content = text.text;
		text.clear();
		text.text = content;
	}
	
	
	#endregion
	
	#region Scaling
	
	public static void hd( ref int num )
	{
		if( UI.isHD )
		{
			num *= 2;
		}
	}
	
	public static void hd( ref float num )
	{
		if( UI.isHD )
		{
			num *= 2f;
		}
	}
	
	public static int hdReturn( int num )
	{
		hd( ref num );
		
		return num;
	}
	
	public static float hdReturn( float num )
	{
		hd( ref num );
		
		return num;
	}
	
	public static float getScaleSize( float origSize, float finalSize )
	{
		return finalSize/origSize;
	}
	
	#endregion
	
	#region Containers
	
	public static void parentToContainer( UIAbstractContainer container, params UISprite[] sprites )
	{
		foreach( var sprite in sprites )
		{
			sprite.parentUIObject = container;
		}
		
		container.addUIChild( sprites );
	}
	
	#endregion
	
	#region Scrollable and Scrollbar
	
	public static UIScrollableVerticalLayout createScrollableLayout( UIToolkit toolkit, Vector2 size )
	{
		UIScrollableVerticalLayout layout = new UIScrollableVerticalLayout(0);
		layout.alignMode = UIAbstractContainer.UIContainerAlignMode.Left;
		layout.positionFromBottom( 0 ); // Position it at the bottom of the screen to keep it out of the view.
		layout.edgeInsets = new UIEdgeInsets( 0, 0, 0, 10 );
		layout.setSize( size.x, size.y );
		
		// Create the scrollbar
		UIVerticalPanel scrollbar = UIVerticalPanel.create( toolkit, "scrollbar_top.png", "scrollbar_mid.png", "scrollbar_bottom.png" );
		int spacerSize = 16;
		hd( ref spacerSize );
		UISpacer spacer = new UISpacer( spacerSize, spacerSize );
		scrollbar.addChild( spacer );
		
		layout.addScrollBar( scrollbar, spacer );
		return layout;
	}

	
	public static void textPositionInWrapper (UITextInstance text)
	{
		int textOffset = 4;
		hd( ref textOffset );
		text.pixelsFromTopLeft( textOffset, textOffset );
	}
	
	
	public static void addTextToScrollable( UIScrollableVerticalLayout layout, UITextInstance text, UIToolkit uiTools )
	{
		UISprite textWrapper;
		bool wrapperAlreadyAdded = false;
			
		setTextLayoutWrap( layout, text );
		
		if( layout.TextWrapper == null )
		{
			textWrapper = createTextWrapper( uiTools, text, layout );
			layout.TextWrapper = textWrapper;
		}
		else
		{
			textWrapper = layout.TextWrapper;
			wrapperAlreadyAdded = true;
			
			if( textWrapper.userData != null )
			{
				Debug.Log( "textWrapper has userData. NEED TO DELETE IT?" );
				// TODO: Need to add some checking here for safety.
				UITextInstance oldText = (UITextInstance)textWrapper.userData;
				if( oldText != text )
				{
					Debug.Log( "Removing the old text from the textWrapper.userData by unparenting it" );
					oldText.parentUIObject = null;
					textWrapper.userData = null;
				}
				textWrapperScale( layout, textWrapper, text );
				
				text.parentUIObject = textWrapper;
				textWrapper.userData = text;
				
				textPositionInWrapper( text );
			}
		}
		
		if( !wrapperAlreadyAdded )
		{
			layout.addChild( textWrapper );
		}
	}
	
	public static void setTextLayoutWrap( UIAbstractContainer layout, UITextInstance text )
	{	
		setTextWrap( text, layout.width - layout.edgeInsets.right );
	}
	
	public static void textWrapperScale( UIAbstractContainer layout, UISprite textWrapper, UITextInstance text )
	{
		float width = layout.width;
		textWrapperScale( width, textWrapper, text );
	}
	
	public static void textWrapperScale( float width, UISprite textWrapper, UITextInstance text )
	{
		Vector2 textDimensions = text._parentText.sizeForText( text.text );
		textWrapper.scale = (new Vector3(width / 5, textDimensions.y / 5, 1));
	}
	
	public static UISprite createTextWrapper( UIToolkit uiTools, UITextInstance text, UIAbstractContainer layout )
	{	
		// Get the right wrapping for the text
		setTextLayoutWrap( layout, text );
		
		return createTextWrapper( uiTools, text, layout.width, true );
	}
	
	public static UISprite createTextWrapper( UIToolkit uiTools, UITextInstance text, float width, bool wrapped )
	{
		// Get the right wrapping for the text.
		if( !wrapped )
		{
			setTextWrap( text, width );
		}
		
		// Create the wrapper and scale it
		// TODO: Check if empty.png exists if not then throw a clear error.
		UISprite textWrapper = uiTools.addSprite( "empty.png", 0, 0 );
		textWrapperScale( width, textWrapper, text );
		
		// Make the text a child of the wrapper
		text.parentUIObject = textWrapper;
		textWrapper.userData = text;
		
		textPositionInWrapper( text );
		
		// Return the wrapper
		return textWrapper;
		
	}
	
	#endregion
}

public enum SpriteType
{
	Sprite,
	Button
};

public struct ActorBarStruct
{
	public UISprite title;
	public int index;
}