using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankWar
{
	/// <summary>
	/// Projectile Class is build for the tanks class, when tank shoot, the projectiles show
	/// </summary>
	public class Projectile
	{	
		#region fields
		Texture2D sprite;
		Rectangle drawRectangle;
		Vector2 location;

		// Whether the projectile is collided with others
		bool active=true;

		//the fire direction
		Direction direction;


		//the tankwidth from the tank class
		int tankWidth;
		int tankHeight;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TankWar.Projectile"/> is active.
		/// </summary>
		/// <value><c>true</c> if active the projectile is alive; otherwise, it will not update or draw<c>false</c>.</value>

		public bool Active {
			get { return active;}
			set { this.active = value;}
		}

		public Rectangle CollisionRectangle{
			get{ return drawRectangle;}
		}
		#endregion


		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="TankWar.Projectile"/> class.
		/// </summary>
		/// <param name="sprite">Sprite. texture2d</param>
		/// <param name="direction">Direction. fire location</param>
		/// <param name="location">Location. the tank  location (left up corner)</param>
		/// <param name="frameWidth">Frame width. tank width</param>
		/// <param name="frameHeight">Frame height. tank height</param>
		public Projectile(Texture2D sprite,Direction direction, Vector2 location,int frameWidth,
		                  int frameHeight){


			this.direction = direction;
			this.sprite = sprite;
			this.location = location;
			this.tankWidth = frameWidth;
			this.tankHeight = frameHeight;

			//Get the right gunlocation for fire
			GetDrawRectangle ();
			
		}
		#endregion

		#region Methods
		/// <summary>
		/// Draw the specified spriteBatch.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		public void Draw(SpriteBatch spriteBatch){
			// use Get DraRectangle to get the rectangle with the right direction automaticaly
			if (Active) {
				spriteBatch.Draw (sprite, drawRectangle, Color.White);
			}

		}
		/// <summary>
		/// Update the specified gameTime and coolTime.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="coolTime">Cool time.</param>
		public void Update(GameTime gameTime, int coolTime){

			//Let the projectile moving to fire
			if (Active) {
				Fire ();
			}

			// deactive the projectile when it is out of the border
			if (drawRectangle.Top < 0 || drawRectangle.Bottom > GameConstants.WIN_HEIGHT ||
				drawRectangle.Right > GameConstants.RIGHT_BOARDER|| drawRectangle.Left < GameConstants.LEFT_BOARDER)
			{
				this.active = false;
			}
		}


		/// <summary>
		/// Fire to 4 directions. switch.
		/// </summary>
		private void Fire(){
			switch (direction) {
			case Direction.Up:

				drawRectangle.Y -= GameConstants.SHOOTING_SPEED;
				break;
			case Direction.Right:

				drawRectangle.X += GameConstants.SHOOTING_SPEED;
				break;
			case Direction.Down:
				drawRectangle.Y += GameConstants.SHOOTING_SPEED;
				break;
			case Direction.Left:
				drawRectangle.X -= GameConstants.SHOOTING_SPEED;
				break;
			
			}
		}


		/// <summary>
		/// Gets the draw rectangle. For different 4 directions
		/// </summary>
		/// Get Draw Rectangle to hold sprite
		private void GetDrawRectangle(){
			switch (direction) {
			case Direction.Up:
				drawRectangle = new Rectangle ((int)(location.X+tankWidth/2 - sprite.Width / 2),
				                      (int)(location.Y),
				                     sprite.Width, sprite.Height);
				break;
			case Direction.Right:
				drawRectangle = new Rectangle ((int)(location.X +tankWidth),
				                      (int)(location.Y+tankHeight/2-sprite.Width/2),
				                      sprite.Width, sprite.Height);
				break;
			case Direction.Down:
				drawRectangle =new Rectangle ((int)(location.X+tankWidth/2),
				                      (int)(location.Y+tankHeight),
				                      sprite.Width, sprite.Height);
				break;
			case Direction.Left:
				drawRectangle = new Rectangle ((int)(location.X) ,
				                      (int)(location.Y+tankHeight/2-sprite.Width/2),
				                      sprite.Width, sprite.Height);
				break;

			}
		
		}


		#endregion
	}
}
