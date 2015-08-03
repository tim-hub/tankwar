#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace TankWar
{
	/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {	

		#region Fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		//set screen
		int winWidth=GameConstants.WIN_WIDTH;
		int winHeight=GameConstants.WIN_HEIGHT;
		bool isFullScreen=false;

		//Game Objects
		Tank player1;

		//tank enemies

		AI ai;

		//All tanks with ai
		List<AI> tanks = new List<AI> ();
		List<MediumTank> mediumTanks = new List<MediumTank> ();
		List<ArmoredCar> armoredCars=new List<ArmoredCar>();


		//Game back ground
		Texture2D backSprite;
		public static Map map1;

		//fonts
		SpriteFont font;
		int score;
		string scoresString;
		//explosions
		Texture2D spriteExp;
		List<Explosion> explosions=new List<Explosion>();

		#endregion

//		#region Properties
//		public Map Map1{get{return map1;} }
//
//		#endregion


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = isFullScreen;

			graphics.PreferredBackBufferWidth=winWidth ;
			graphics.PreferredBackBufferHeight=winHeight;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
				
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			//load back
			backSprite = Content.Load<Texture2D> ("back");

			//font load
			font = Content.Load<SpriteFont> ("Arial");
			score = 0;
			scoresString = "Scores: ";
			//load map
			map1 = new Map (Content);
			//Creat new objects;
			player1 = new Tank (Content, "Tanks4", GameConstants.PLAYER1_X, winHeight,GameConstants.PLAYER1,map1);
			//create explosion
			spriteExp = Content.Load<Texture2D> ("explosion");


			//tanks.Add (new AI(Content,"MediumTanks4",GameConstants.LEFT_BOARDER,0,map1));
			mediumTanks.Add(new MediumTank(Content,"MediumTanks4",GameConstants.LEFT_BOARDER,0,map1));
			mediumTanks.Add(new MediumTank(Content,"MediumTanks4",GameConstants.CENTER_V,0,map1));
			armoredCars.Add(new ArmoredCar(Content,"MediumTanks4",GameConstants.RIGHT_BOARDER,0,map1));

			//add all in tanks
			tanks.AddRange (mediumTanks);
			tanks.AddRange (armoredCars);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				Exit();
			}
            // Use Keyboard
			KeyboardState keyboard = Keyboard.GetState ();
			player1.Update (gameTime, keyboard);

			//player1 shoot in map

			CheckMapProjectiles (player1);  



			//tankAIs Update (gameTime, keyboard);
			for (int i=tanks.Count-1; i>=0; i--) {
				// ai tanks shoot in map
				CheckMapProjectiles (tanks [i]);

				//Checnk the collisiond between ai and player's projectiles   Jul 11
				foreach (Projectile projectile in player1.Projectiles) {
					if (tanks[i].Active&&tanks [i].CollisionRectangle.Intersects (projectile.CollisionRectangle)) {
						tanks [i].Active = false;
						//(check the ai tank type)
						score += 1;
						projectile.Active = false;

						Explode (gameTime, tanks [i].CollisionRectangle.X, tanks [i].CollisionRectangle.Y);

					}

				}


				//check the projectiles from ais
				foreach (Projectile projectile in tanks[i].Projectiles) {
					//deactive the projectiles from player
					if (tanks [i].Active) {
						foreach (Projectile playerProjectile in player1.Projectiles) {
							if (projectile.CollisionRectangle.Intersects (playerProjectile.CollisionRectangle)) {
								projectile.Active = false;
								playerProjectile.Active = false;
							}
						}
					}
					//deactive the projectiles from ai
					for (int j=i+1; j<tanks.Count; j++) {
						foreach (Projectile nextProjectile in tanks[j].Projectiles) {
							if (projectile.CollisionRectangle.Intersects(nextProjectile.CollisionRectangle)){
							    
								//projectiles in tanks will removed when it was deactive
								projectile.Active=false;
								nextProjectile.Active=false;

							}
						
						}
					}
					if (player1.Active&&player1.CollisionRectangle.Intersects(projectile.CollisionRectangle)){

						projectile.Active = false;
						Explode (gameTime, player1.CollisionRectangle.X, player1.CollisionRectangle.Y);
						player1.Active=false;
					}
				
				}

				//update tanks
				//to avoid projectiles 
				tanks [i].Update (gameTime);
			
			
			}
			//remove the dead tanks
			for (int i=tanks.Count-1; i>=0; i--) {
				if (!tanks [i].Active) {
					tanks [i].Update (gameTime);
					tanks.RemoveAt (i);

				}
			}


			//update explosions  and remove the finished one

			for (int i= explosions.Count-1; i>=0; i--) {
				if (explosions [i].Finished) {
					explosions.RemoveAt (i);
				} else {
					explosions[i].Update (gameTime);
				}
			}



            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           	graphics.GraphicsDevice.Clear(Color.DarkGray);

            //TODO: Add your drawing code here
			spriteBatch.Begin ();
			//draw background &map
			spriteBatch.Draw (backSprite, new Vector2 (0, 0),Color.White);
			spriteBatch.DrawString (font,scoresString+ score, new Vector2 (560, 40), Color.Red);

			map1.Draw (spriteBatch);

			player1.Draw (spriteBatch);

			
			//draw tank enemies
			foreach (Tank tank in tanks) {
				tank.Draw (spriteBatch);
			}

			//draw explosions
			foreach (Explosion explosion in explosions) {
				explosion.Draw (spriteBatch);
			}

			spriteBatch.End ();
            
            base.Draw(gameTime);
        }


		public void Explode(GameTime gameTime,int x, int y){
			explosions.Add(new Explosion(spriteExp, x,y));


		}

		/// <summary>
		/// Checks the map projectiles. Remove the cubes in map which was shooted
		/// </summary>
		/// <param name="tank">Tank.</param>

		public void CheckMapProjectiles( Tank tank){
			//this nested loop maybe decrease the efficiency

			for (int i=map1.Cubes.Count-1; i>=0; i--) {
				for (int j=tank.Projectiles.Count-1; j>=0; j--) {
					if (map1.Cubes [i].CollisionRectangle.Intersects (tank.Projectiles[j].CollisionRectangle)) {

						//wall cubes gone
						if (map1.Cubes [i].GetType () == typeof(WallCube)) {
							map1.Cubes.RemoveAt (i);
							tank.Projectiles [j].Active = false; //when projectile do not active tank update will remove it automatically

						}
						//iron still there
						if (map1.Cubes [i].GetType () == typeof(IronCube)) {
							tank.Projectiles [j].Active = false;
						}

						//check whether the home is conquered
						if (map1.Cubes [i].GetType () == typeof(EagleCube)) {
							map1.Cubes.RemoveAt (i);

							// add a ruin area

							tank.Projectiles [j].Active = false;
						}


					}	
				}
			}

		}
    }
}

