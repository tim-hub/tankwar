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
	public class Tank
	{	
		#region Fields

		//Moving control
		//private is default, use is to show more clearly
		private Keys keyUp;
		private Keys keyRight;
		private Keys keyDown;
		private Keys keyLeft;
		#endregion

		#region Properties
		//Active
		public bool Active{ get; set; }

		// 4 parts 
		protected Texture2D Sprite;
		protected int FrameWidth;
		protected int FrameHeight;
		protected Rectangle DrawRectangle ;
		protected Rectangle ShowRectangle;

		//shoot
		public List<Projectile> Projectiles = new List<Projectile> ();
		protected int CoolTime;
		protected Direction GunDirection;
		protected Texture2D ProjectileSprite;
		protected int CurrentShootingTime=0;
		protected bool CanShoot=true;

		// the rows and colum in tank picture
		public const int IMAGES_PER_ROW = 2;
		public const int IMAGES_PER_COLUM = 2;

		//Move facts
		protected int MovingSpeed;
		protected int CurrentMovingTime;

		//map
		protected List<Cube> CubesInMap;

		//explode





		public Rectangle CollisionRectangle{
			get {return DrawRectangle;}
		}


		#endregion


		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="TankWar.Tank"/> class.  4 arugments
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="spriteName">Sprite name.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>

		public Tank( ContentManager content,string spriteName, int x, int y )
		{

			//Load Tank Picture
			Sprite = content.Load<Texture2D> (spriteName);

			//than make active
			Active = true;
			//Load Projectile
			ProjectileSprite = content.Load<Texture2D> ("Projectile");
			GunDirection = Direction.Up;
			CoolTime=GameConstants.COOL_TIME;

			//Moving speed
			MovingSpeed = GameConstants.TANK_MOVING_SPEED_PLAYER;
			CurrentMovingTime = 0;

			//Seperate the different 4 parts of tanks
			FrameWidth = Sprite.Width / 2;
			FrameHeight = Sprite.Height / 2;

			//the original rctangle
			DrawRectangle =new Rectangle(x, y,
			                             FrameWidth, FrameHeight);
			//the rectangle to show
			ShowRectangle = new Rectangle (0, 0,FrameWidth, FrameHeight);

	


		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TankWar.Tank"/> class. with keys setting argument
		/// </summary>
		/// <param name="content">Content. to load content</param>
		/// <param name="spriteName">Sprite name. the content name loaded</param>
		/// <param name="x">The x coordinate.  -- the location of initializes</param></param>
		/// <param name="y">The y coordinate.  -- the location of initializes</param>

		public Tank( ContentManager content,string spriteName, int x, int y,  Keys[] keys, Map map)
			: this (content, spriteName, x, y)
		{

			//Set control keys
			keyUp = keys [0];
			keyRight = keys [1];
			keyDown = keys [2];
			keyLeft = keys [3];

			//set map
			this.CubesInMap = map.Cubes;
		}


		#endregion

		#region Methods


		/// <summary>
		/// Update the specified gameTime and keyboard. Use keyboard to control tank.
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		/// <param name="keyboard">Keyboard.</param>
		public void Update(GameTime gameTime, KeyboardState keyboard){
			//frequency in update is 60 hz defautly, update 60 times in 1 seconds


			//bound 
			Bound ();
			bool halt =CheckHalt();
			if (Active) {


				// di not interact other cubes than move
				if (!halt) {
					//Move smoothly
					MoveSmoothly (gameTime);


					if (keyboard.IsKeyDown (keyLeft) && (!halt)) {
						MoveLeft ();
						GunDirection = Direction.Left;


					} else if (keyboard.IsKeyDown (keyRight) && (!halt)) {
						MoveRight ();
						GunDirection = Direction.Right;


					} else if (keyboard.IsKeyDown (keyUp) && (!halt)) {
						MoveUp ();
						GunDirection = Direction.Up;

					} else if (keyboard.IsKeyDown (keyDown) && (!halt)) {
						MoveDown ();
						GunDirection = Direction.Down;

					}
					//if interacts then move back a little
				} else {
					switch (GunDirection) {
					case Direction.Up:
						DrawRectangle.Y += GameConstants.TANK_MOVING_SPEED_PLAYER;
						break;
					case Direction.Right:
						DrawRectangle.X -= GameConstants.TANK_MOVING_SPEED_PLAYER;
						break;
					case Direction.Left:
						DrawRectangle.X += GameConstants.TANK_MOVING_SPEED_PLAYER;
						break;
					case Direction.Down:
						DrawRectangle.Y -= GameConstants.TANK_MOVING_SPEED_PLAYER;
						break;
					}
				}

				// Press F and shoot
				// Use This support cool time and multi click to shoot
				if (keyboard.IsKeyDown (Keys.F) && CanShoot) {
					Shooting ();
					CanShoot = false;
				}
				if (!CanShoot) {
					CurrentShootingTime += gameTime.ElapsedGameTime.Milliseconds;
					if (CurrentShootingTime >= CoolTime ||
						keyboard.IsKeyUp (Keys.F)) {
						CanShoot = true;
						CurrentShootingTime = 0;
					}

				}


			




			} else {


				Projectiles.Clear ();




			} 
			//update projectiles & Remove the deactive projectiles
			ProjectilesUpdate (gameTime);


		}
		/// <summary>
		/// Projectileses the update and remove the deactive
		/// </summary>
		/// <param name="gameTime">Game time.</param>
		public void ProjectilesUpdate(GameTime gameTime){
			// Update every projectile
			for (int i=Projectiles.Count-1; i>=0; i--) {
				if (!Projectiles [i].Active) {
					Projectiles.RemoveAt (i);
				} else {
					Projectiles[i].Update (gameTime, CoolTime);
				}
			
			}


		}



		/// <summary>
		/// Draw the specified spriteBatch.
		/// </summary>
		/// <param name="spriteBatch">Sprite batch.</param>
		public void Draw(SpriteBatch spriteBatch){
			if (Active) {
				spriteBatch.Draw (Sprite, DrawRectangle, ShowRectangle, Color.White);
				foreach (Projectile projectile in Projectiles) {
					projectile.Draw (spriteBatch);
				}
			}

		}

		/// <summary>
		/// Bound this instance. do not move tank out of the boarder
		/// </summary>
		public void Bound(){
			if (DrawRectangle.Left < GameConstants.LEFT_BOARDER) {
				DrawRectangle.X = GameConstants.LEFT_BOARDER;
			} else if (DrawRectangle.Right > GameConstants.RIGHT_BOARDER) {
				DrawRectangle.X = GameConstants.RIGHT_BOARDER - DrawRectangle.Width;
			}
			if (DrawRectangle.Top < 0) {
				DrawRectangle.Y = 0;
			} else if (DrawRectangle.Bottom > GameConstants.WIN_HEIGHT) {
				DrawRectangle.Y = GameConstants.WIN_HEIGHT-DrawRectangle.Height;
			}


		}





		/// <summary>
		/// Checks the tank is or not halted. retrun true or false
		/// </summary>
		/// <returns><c>true</c>, if halt was checked, <c>false</c> otherwise.</returns>
		public bool CheckHalt(){

			//use map resources
			//and return true or flase
			foreach (Cube cube in CubesInMap ) {
				//tanks will halt regard of grass
				if ((cube.GetType () != typeof(GrassCube)) &&cube.CollisionRectangle.Intersects (CollisionRectangle)) {
					return true;
				}else if (DrawRectangle.Left < GameConstants.LEFT_BOARDER
				          ||DrawRectangle.Right > GameConstants.RIGHT_BOARDER
				          ||DrawRectangle.Top < 0
				          ||DrawRectangle.Bottom > GameConstants.WIN_HEIGHT) {
					return true;
				}
			}

			return false;

		}

		/// <summary>
		/// Shooting
		/// </summary>
		
		public void Shooting()
		{
			//use projectiles list to show projectiles
			Projectiles.Add(new Projectile(ProjectileSprite,GunDirection, 
			                               new Vector2 (DrawRectangle.X,
			             								DrawRectangle.Y),
			                               FrameWidth,FrameHeight));
		}


		/// <summary>
		/// 4 directions to move
		/// </summary>
		public void MoveUp(){

			//sprite = content.Load<Texture2D> (spriteName+"up");
			DrawRectangle.Y -= MovingSpeed;
			ShowRectangle.X = 0;
			ShowRectangle.Y = 0;
		}
		public void MoveDown(){
			DrawRectangle.Y += MovingSpeed;
			ShowRectangle.X = 40;
			ShowRectangle.Y = 40;
		}
		public void MoveRight(){
			DrawRectangle.X += MovingSpeed;
			ShowRectangle.X = 40;
			ShowRectangle.Y = 0;
		}
		public void MoveLeft(){
			DrawRectangle.X -= MovingSpeed;
			ShowRectangle.X = 0;
			ShowRectangle.Y = 40;
		}

		/// <summary>
		/// Moves the smoothly.
		/// 222 is 40(framewidth)/3/60*1000
		/// 10 is one quarter of framewidth
		/// </summary>
		/// <param name="gameTime">Game time.</param>

		public void MoveSmoothly(GameTime gameTime){
			CurrentMovingTime += gameTime.ElapsedGameTime.Milliseconds;


			if (CurrentMovingTime > 222) {
				if (!(DrawRectangle.X % 10 == 0)) {
					//math.round   fout to 0, 6 to 10, 5 to even
					DrawRectangle.X = (int)(Math.Round (DrawRectangle.X / 10.0)) *10;

				}
				if (!(DrawRectangle.Y % 10 == 0)) {
					//math.round   fout to 0, 6 to 10, 5 to even
					DrawRectangle.Y = (int)( Math.Round(DrawRectangle.Y / 10.0)) * 10;

				}
				CurrentMovingTime = 0;

			}
		
		}
		#endregion

	}
}