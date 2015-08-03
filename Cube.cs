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
	public class Cube
	{	
		#region fields

		#endregion

		#region Proberties
		protected Texture2D Sprite;
		protected Rectangle DrawRectangle;
		public bool Active { get; set;}

		#endregion


		#region Properties
		public Rectangle  CollisionRectangle{
			get{ return DrawRectangle;}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Create Cube
		/// </summary>
		/// <param name="content">Content.</param>
		/// <param name="spriteName">Sprite name.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Cube (ContentManager content,String spriteName, int x, int y)
		{
			Sprite = content.Load<Texture2D> (spriteName);

			DrawRectangle = new Rectangle (x+GameConstants.LEFT_BOARDER, y,
			                            Sprite.Width,
			                            Sprite.Height);
		}
		#endregion

		#region Methods
		public void Draw(SpriteBatch spriteBatch){
			spriteBatch.Draw (Sprite, DrawRectangle, Color.White);
		}
		#endregion
	}


	//Sub classes of Cube

	public class WallCube :Cube
	{
		
		#region Constructors
		public WallCube(ContentManager content,String spriteName, int x, int y) :base(content,spriteName,x,y)
		{

		}
		public WallCube (ContentManager content,int x, int y):this(content,"Wall",x,y)
		{

		}
		#endregion

	}
	public class IronCube :Cube
	{

		#region Constructors
		public IronCube(ContentManager content,String spriteName, int x, int y) :base(content,spriteName,x,y)
		{

		}
		public IronCube (ContentManager content,int x, int y):this(content,"Iron",x,y)
		{

		}
		#endregion

	}
	public class GrassCube :Cube
	{

		#region Constructors
		public GrassCube(ContentManager content,String spriteName, int x, int y) :base(content,spriteName,x,y)
		{

		}
		public GrassCube (ContentManager content,int x, int y):this(content,"Grass",x,y)
		{

		}
		#endregion

	}
	public class WaterCube :Cube
	{

		#region Constructors
		public WaterCube(ContentManager content,String spriteName, int x, int y) :base(content,spriteName,x,y)
		{

		}
		public WaterCube (ContentManager content,int x, int y):this(content,"Water",x,y)
		{

		}
		#endregion

	}
	public class EagleCube:Cube
	{
		#region Constructors
		public EagleCube(ContentManager content,String spriteName, int x, int y) :base(content,spriteName,x,y)
		{

		}
		public EagleCube (ContentManager content,int x, int y):this(content,"eagle",x,y)
		{

		}
		#endregion

	}
}

