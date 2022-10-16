using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Silnik
{
	public class Collision : GameObjectComponent
	{
		private BoundingBoxBuffers _buffers;
		private BasicEffect _effect;
		private Transform transform;
		private Matrix baseWorld;
		private Matrix[] modelTransforms;
		private BoundingSphere boundingSphere;
		private Vector3 prevPos;

		public Boolean drawPrimitives;
		public BoundingBox box;
		public Rectangle rect;

		public bool IsStatic = true;
		private bool Pushable;
		public bool IsInteracting;
		private bool Collided;
		private Model model;
		private QuadTreeComponent quadTree;
		public int collisionType;
		//to save size
		private Vector3 Size1, Size2;
		private float R;


		public Collision()
		{
			drawPrimitives = false;
		}
		public Rectangle Rect
		{
			get
			{
				Vector3[] corners = box.GetCorners();
				rect.X = (int)corners[4].X;
				rect.Y = -(int)corners[4].Z;
				rect.Width = (int)Math.Abs(corners[5].X - corners[4].X);
				rect.Height = (int)Math.Abs(corners[0].Z - corners[4].Z);
				return rect;
			}
		}

		public void Load(Mesh mesh, bool isPushable = false)
		{
			collisionType = 0;
			model = mesh.model;
			Pushable = isPushable;
			transform = gameObject.GetComponent<Transform>();
			//box = CreateBoundingBox(model);
			modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);
			buildBoundingSphere(model);
			prevPos = Vector3.Zero;
			box = BoundingBox.CreateFromSphere(BoundingSphere);

			if (drawPrimitives)
            {
				_buffers = CreateBoundingBoxBuffers(box, InitContent.Graphics.GraphicsDevice);

				_effect = new BasicEffect(InitContent.Graphics.GraphicsDevice);
				_effect.LightingEnabled = false;
				_effect.TextureEnabled = false;
				_effect.VertexColorEnabled = true;
				//_effect.AmbientLightColor = new Vector3(0.2f, 1, 0.2f);
			}
			
		}

		public void Load(Vector3 size, bool isPushable = false)
		{
			collisionType = 1;
			Size1 = size;
			Pushable = isPushable;
			transform = gameObject.GetComponent<Transform>();
			prevPos = Vector3.Zero;

			box = new BoundingBox(new Vector3(transform.position.X - size.X / 2, transform.position.Y - size.Y/2, transform.position.Z - size.Z / 2),
				new Vector3(transform.position.X + size.X / 2, transform.position.Y + size.Y/2, transform.position.Z + size.Z/2));

			if (drawPrimitives)
			{
				_buffers = CreateBoundingBoxBuffers(box, InitContent.Graphics.GraphicsDevice);

				_effect = new BasicEffect(InitContent.Graphics.GraphicsDevice);
				_effect.LightingEnabled = false;
				_effect.TextureEnabled = false;
				_effect.VertexColorEnabled = true;
				//_effect.AmbientLightColor = new Vector3(0.2f, 1, 0.2f);
			}

		}
		/// <summary>
		/// Create box collider from points
		/// </summary>
		/// <param name="point1">nearest point</param>
		/// <param name="point2">farthest point</param>
		/// <param name="isPushable">can be pushed by other colliders</param>
		public void Load(Vector3 point1, Vector3 point2, bool isPushable = false)
		{
			collisionType = 2;
			Size1 = point1;
			Size2 = point2;
			Pushable = isPushable;
			transform = gameObject.GetComponent<Transform>();
			Vector3 position = transform.AbsoluteTransform.Translation;
			prevPos = Vector3.Zero;

			box = new BoundingBox(new Vector3(position.X - point1.X, position.Y - point1.Y, position.Z - point1.Z),
				new Vector3(position.X + point2.X, position.Y + point2.Y, position.Z + point2.Z));

			if (drawPrimitives)
			{
				_buffers = CreateBoundingBoxBuffers(box, InitContent.Graphics.GraphicsDevice);

				_effect = new BasicEffect(InitContent.Graphics.GraphicsDevice);
				_effect.LightingEnabled = false;
				_effect.TextureEnabled = false;
				_effect.VertexColorEnabled = true;
				//_effect.AmbientLightColor = new Vector3(0.2f, 1, 0.2f);
			}

		}
		public void LoadSphere(Mesh mesh, bool isPushable = false)
		{
			collisionType = 3;
			model = mesh.model;
			Pushable = isPushable;
			transform = gameObject.GetComponent<Transform>();
			//box = CreateBoundingBox(model);
			modelTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(modelTransforms);
			buildBoundingSphere(model);
			prevPos = Vector3.Zero;
			box = BoundingBox.CreateFromSphere(BoundingSphere);

			if (drawPrimitives)
			{
				_buffers = CreateBoundingBoxBuffers(box, InitContent.Graphics.GraphicsDevice);

				_effect = new BasicEffect(InitContent.Graphics.GraphicsDevice);
				_effect.LightingEnabled = false;
				_effect.TextureEnabled = false;
				_effect.VertexColorEnabled = true;
				//_effect.AmbientLightColor = new Vector3(0.2f, 1, 0.2f);
			}

		}
		public void LoadSphere(float r, bool isPushable = false)
        {
			collisionType = 4;
			R = r;
			Pushable = isPushable;
			transform = gameObject.GetComponent<Transform>();
			Vector3 position = transform.AbsoluteTransform.Translation;
			prevPos = Vector3.Zero;

			boundingSphere = new BoundingSphere(position, r*10);
			box = BoundingBox.CreateFromSphere(BoundingSphere);

			if (drawPrimitives)
			{
				_buffers = CreateBoundingBoxBuffers(box, InitContent.Graphics.GraphicsDevice);

				_effect = new BasicEffect(InitContent.Graphics.GraphicsDevice);
				_effect.LightingEnabled = false;
				_effect.TextureEnabled = false;
				_effect.VertexColorEnabled = true;
				//_effect.AmbientLightColor = new Vector3(0.2f, 1, 0.2f);
			}
		}

		public override void Draw(Matrix world, Matrix view, Matrix projection)
        {
			if(drawPrimitives)
            {
				_buffers = CreateBoundingBoxBuffers(box, InitContent.Graphics.GraphicsDevice);
				InitContent.Graphics.GraphicsDevice.SetVertexBuffer(_buffers.Vertices);
				InitContent.Graphics.GraphicsDevice.Indices = _buffers.Indices;

				baseWorld = Matrix.Identity;

				_effect.World = baseWorld;
				_effect.View = view;
				_effect.Projection = projection;

				foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
				{
					pass.Apply();
					InitContent.Graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0,
						_buffers.VertexCount, 0, _buffers.PrimitiveCount);
				}
			}
			
		}
			


		private BoundingBoxBuffers CreateBoundingBoxBuffers(BoundingBox boundingBox, GraphicsDevice graphicsDevice)
		{
			BoundingBoxBuffers boundingBoxBuffers = new BoundingBoxBuffers();

			boundingBoxBuffers.PrimitiveCount = 12;
			boundingBoxBuffers.VertexCount = 24;

			VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice,
				typeof(VertexPositionColor), boundingBoxBuffers.VertexCount,
				BufferUsage.WriteOnly);
			List<VertexPositionColor> vertices = new List<VertexPositionColor>();

			Vector3[] corners = boundingBox.GetCorners();
			//Console.WriteLine("corn1: {0} corn2: {1} corn3: {2}, corn4: {3} corn5: {4} corn6: {5} corn7: {6}, corn8: {7}", corners[0], corners[1], corners[2], corners[3], corners[4], corners[5], corners[6], corners[7]);

			// line1.
			AddVertex(vertices, corners[0]);
			AddVertex(vertices, corners[1]);



			// line2.
			AddVertex(vertices, corners[1]);
			AddVertex(vertices, corners[2]);



			// line3.
			AddVertex(vertices, corners[2]);
			AddVertex(vertices, corners[3]);



			// line4.
			AddVertex(vertices, corners[3]);
			AddVertex(vertices, corners[0]);



			// line5.
			AddVertex(vertices, corners[4]);
			AddVertex(vertices, corners[5]);



			// line6.
			AddVertex(vertices, corners[5]);
			AddVertex(vertices, corners[6]);



			// line7.
			AddVertex(vertices, corners[6]);
			AddVertex(vertices, corners[7]);



			// line8
			AddVertex(vertices, corners[7]);
			AddVertex(vertices, corners[4]);
			// line9
			AddVertex(vertices, corners[0]);
			AddVertex(vertices, corners[4]);
			// line10
			AddVertex(vertices, corners[1]);
			AddVertex(vertices, corners[5]);
			// line11
			AddVertex(vertices, corners[2]);
			AddVertex(vertices, corners[6]);
			// line12
			AddVertex(vertices, corners[3]);
			AddVertex(vertices, corners[7]);



			vertexBuffer.SetData(vertices.ToArray());
			boundingBoxBuffers.Vertices = vertexBuffer;

			IndexBuffer indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, boundingBoxBuffers.VertexCount,
				BufferUsage.WriteOnly);
			indexBuffer.SetData(Enumerable.Range(0, boundingBoxBuffers.VertexCount).Select(i => (short)i).ToArray());
			boundingBoxBuffers.Indices = indexBuffer;

			return boundingBoxBuffers;
		}

		private void AddVertex(List<VertexPositionColor> vertices, Vector3 position)
		{
			if(collisionType >= 3)
            {
				vertices.Add(new VertexPositionColor(position, Color.Blue));
			}
			else
            {
				vertices.Add(new VertexPositionColor(position, Color.LightGreen));
			}
			
		}

		private BoundingBox CreateBoundingBox(Model model)
		{
			Matrix[] boneTransforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(boneTransforms);

			BoundingBox result = new BoundingBox();
			foreach (ModelMesh mesh in model.Meshes)
				foreach (ModelMeshPart meshPart in mesh.MeshParts)
				{
					BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
					if (meshPartBoundingBox != null)
						result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
				}
			return result;
		}

		private BoundingBox? GetBoundingBox(ModelMeshPart meshPart, Matrix transform)
		{
			if (meshPart.VertexBuffer == null)
				return null;

			Vector3[] positions = VertexElementExtractor.GetVertexElement(meshPart, VertexElementUsage.Position);
			if (positions == null)
				return null;
			//transform *= transform1.MatrixPosition;
			Vector3[] transformedPositions = new Vector3[positions.Length];
			Vector3.Transform(positions, ref transform, transformedPositions);

			return BoundingBox.CreateFromPoints(transformedPositions);
		}

		private void buildBoundingSphere(Model model)
		{
			BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
			// Merge all the model's built in bounding spheres
			foreach (ModelMesh mesh in model.Meshes)
			{
				BoundingSphere transformed = mesh.BoundingSphere.Transform(
					 modelTransforms[mesh.ParentBone.Index]);
				sphere = BoundingSphere.CreateMerged(sphere, transformed);
			}
			this.boundingSphere = sphere;
		}

		public BoundingSphere BoundingSphere
		{
			get
			{
				// No need for rotation, as this is a sphere
				Matrix worldTransform;
				worldTransform = transform.AbsoluteTransform;
				BoundingSphere transformed = boundingSphere;
				transformed = transformed.Transform(worldTransform);
				return transformed;
			}
		}
		public bool Collide(BoundingBox otherBox)
		{
			if (collisionType >= 3)
            {
				if (BoundingSphere.Intersects(otherBox))
				{
					return true;
				}
				return false;
			}
			else
            {
				if (box.Intersects(otherBox))
				{
					return true;
				}
				return false;
			}
			
		}
		public bool Collide(BoundingSphere otherSphere)
		{
			if (collisionType >= 3)
			{
				if (BoundingSphere.Intersects(otherSphere))
				{
					return true;
				}
			}
			else
			{
				if (box.Intersects(otherSphere))
				{
					return true;
				}
			}
			return false;
		}
		public override void Start()
        {
			quadTree = GameObject.Find("Hitpoint").GetComponent<QuadTreeComponent>();
        }
        public override void Update(GameTime gameTime)
		{
			//czy zamienic World.gameObjects na public static?? -BJ
			//to byl wielki błąd jak się okazało -BJ
			//Stopwatch stopwatch = Stopwatch.StartNew();
			if(!IsStatic)
            {
				UpdateBounding();
				if(Pushable)
                {
					foreach (GameObject gameOb in quadTree.CollideObjects(gameObject))
					{
						if (gameOb != gameObject)
                        {
							if (gameOb.GetComponent<Collision>().collisionType >= 3)
							{
								if (Collide(gameOb.GetComponent<Collision>().BoundingSphere))
									ObjectsIntersect(gameOb);
							}
							else
                            {
								if (Collide(gameOb.GetComponent<Collision>().box))
									ObjectsIntersect(gameOb);
							}
						}
							
							
					}
					prevPos = transform.Position;

				}
							
			}

		}
		private void ObjectsIntersect(GameObject ob)
        {
			float distance = Vector3.Distance(prevPos, transform.Position);
			if (distance <= 0)
				distance = 2.0f;
			do
			{
				Vector3 vector = transform.position - ob.GetComponent<Transform>().position;
				vector.Normalize();
				vector.Y = 0;
				float velocity = 0.25f;
				transform.position += vector * velocity * 1/distance;
			} while (checkAgain(ob));

		}
		private bool checkAgain(GameObject ob)
        {
			if (Collide(ob.GetComponent<Collision>().box))
				return false;
			return true;
			//UNFINISHED
			Console.WriteLine(ob.GetComponent<Collision>().collisionType);
			if (ob.GetComponent<Collision>().collisionType >= 3)
			{
				if (Collide(ob.GetComponent<Collision>().BoundingSphere))
					return true;
				return false;
			}
			else
			{
				if (Collide(ob.GetComponent<Collision>().box))
					return true;
				return false;
			}

		}

		private void UpdateBounding()
        {
			if(collisionType == 0)
            {
				box = BoundingBox.CreateFromSphere(BoundingSphere);
				//box = new BoundingBox(new Vector3(transform.position.X - 2, transform.position.Y - 4, transform.position.Z - 2),
				//new Vector3(transform.position.X + 2, transform.position.Y + 10, transform.position.Z + 2));
			}
			else if(collisionType == 1)
            {
				Vector3 position = transform.AbsoluteTransform.Translation;
				//position = transform.AbsoluteTransform.
				box = new BoundingBox(new Vector3(position.X - Size1.X / 2, position.Y - Size1.Y / 2, position.Z - Size1.Z / 2),
				new Vector3(position.X + Size1.X / 2, position.Y + Size1.Y / 2, position.Z + Size1.Z / 2));
				//BoundingBox transformed = box;

			}
			else if(collisionType == 2)
            {

				box = new BoundingBox(new Vector3(transform.position.X - Size1.X, transform.position.Y - Size1.Y, transform.position.Z - Size1.Z),
				new Vector3(transform.position.X + Size2.X, transform.position.Y + Size2.Y, transform.position.Z + Size2.Z));
				Console.WriteLine(box);
			}
			else if(collisionType == 3)
            {
				box = BoundingBox.CreateFromSphere(BoundingSphere);
            }
			else if(collisionType == 4)
            {
				box = BoundingBox.CreateFromSphere(BoundingSphere);

			}

        }
		public void IsPushable(bool push)
        {
			Pushable = push;
        }
	}
}
