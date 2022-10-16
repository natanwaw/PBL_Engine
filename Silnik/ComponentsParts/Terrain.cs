using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Silnik
{
    public class Terrain
    {
        public struct VertexPositionColorNormal
        {
            public Vector3 Position;
            public Color Color;
            public Vector3 Normal;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0));
        }

        public int[] _indices;
        private float[,] _heightData;
        public VertexBuffer _myVertexBuffer;
        public IndexBuffer _myIndexBuffer;
        public VertexPositionColorNormal[] _vertices;
        public int _terrainWidth { get; private set; }
        public int _terrainHeight { get; private set; }
        private GraphicsDevice _device;
        public Effect _effect { get; set; }
        Camera _camera;
        Texture2D _heightMap;

        public Terrain(GraphicsDevice graphicsDevice)
        {
            _device = graphicsDevice;

        }

        public void SetUpVertices()
        {
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int x = 0; x < _terrainWidth; x++)
            {
                for (int y = 0; y < _terrainHeight; y++)
                {
                    if (_heightData[x, y] < minHeight)
                        minHeight = _heightData[x, y];
                    if (_heightData[x, y] > maxHeight)
                        maxHeight = _heightData[x, y];
                }
            }

            _vertices = new VertexPositionColorNormal[_terrainWidth * _terrainHeight];
            for (int x = 0; x < _terrainWidth; x++)
            {
                for (int y = 0; y < _terrainHeight; y++)
                {
                    _vertices[x + y * _terrainWidth].Position = new Vector3(x, _heightData[x, y], -y);

                    /*if (_heightData[x, y] < minHeight + (maxHeight - minHeight) / 4)
                    {
                        _vertices[x + y * _terrainWidth].Color = Color.Blue;
                    }
                    else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 2 / 4)
                    {
                        _vertices[x + y * _terrainWidth].Color = Color.Green;
                    }
                    else if (_heightData[x, y] < minHeight + (maxHeight - minHeight) * 3 / 4)
                    {
                        _vertices[x + y * _terrainWidth].Color = Color.Yellow;
                    }
                    else
                    {
                        _vertices[x + y * _terrainWidth].Color = Color.White;
                    }*/
                    //_vertices[x + y * _terrainWidth].Color = new Color(new Vector3(1.0f - (_heightData[x, y] - minHeight) / (maxHeight - minHeight), (_heightData[x, y] - minHeight) / (maxHeight - minHeight), 0.0f));
                    _vertices[x + y * _terrainWidth].Color = new Color(new Vector3((_heightData[x, y] - minHeight) / (maxHeight - minHeight) + 0.41f, (_heightData[x, y] - minHeight) / (maxHeight - minHeight) + 0.32f, (_heightData[x, y] - minHeight) / (maxHeight - minHeight) + 0.18f));
                }
            }
        }
        public void SetUpIndices()
        {
            _indices = new int[(_terrainWidth - 1) * (_terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < _terrainHeight - 1; y++)
            {
                for (short x = 0; x < _terrainWidth - 1; x++)
                {
                    int lowerLeft = (int)(x + y * _terrainWidth);
                    int lowerRight = (int)((x + 1) + y * _terrainWidth);
                    int topLeft = (int)(x + (y + 1) * _terrainWidth);
                    int topRight = (int)((x + 1) + (y + 1) * _terrainWidth);

                    _indices[counter++] = topLeft;
                    _indices[counter++] = lowerRight;
                    _indices[counter++] = lowerLeft;

                    _indices[counter++] = topLeft;
                    _indices[counter++] = topRight;
                    _indices[counter++] = lowerRight;
                }
            }
        }

        public void CalculateNormals()
        {
            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Normal = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < _indices.Length / 3; i++)
            {
                int index1 = _indices[i * 3];
                int index2 = _indices[i * 3 + 1];
                int index3 = _indices[i * 3 + 2];

                Vector3 side1 = _vertices[index1].Position - _vertices[index3].Position;
                Vector3 side2 = _vertices[index1].Position - _vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                _vertices[index1].Normal += normal;
                _vertices[index2].Normal += normal;
                _vertices[index3].Normal += normal;
            }
            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Normal.Normalize();
            }
        }

        public void CopyToBuffers()
        {
            _myVertexBuffer = new VertexBuffer(_device, VertexPositionColorNormal.VertexDeclaration, _vertices.Length, BufferUsage.WriteOnly);
            _myVertexBuffer.SetData(_vertices);

            _myIndexBuffer = new IndexBuffer(_device, typeof(int), _indices.Length, BufferUsage.WriteOnly);
            _myIndexBuffer.SetData(_indices);
        }

        public void LoadHeightData(Texture2D heightMap)
        {
            _terrainWidth = heightMap.Width;
            _terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[_terrainWidth * _terrainHeight];
            heightMap.GetData(heightMapColors);

            _heightData = new float[_terrainWidth, _terrainHeight];
            for (int x = 0; x < _terrainWidth; x++)
            {
                for (int y = 0; y < _terrainHeight; y++)
                {
                    _heightData[x, y] = heightMapColors[x + y * _terrainWidth].R / 5.0f;
                }
            }
        }

        public void setUp(Effect effect, Camera camera, Texture2D heightMap)
        {
            _effect = effect;
            _camera = camera;
            _heightMap = heightMap;
            LoadHeightData(_heightMap);
            SetUpVertices();
            SetUpIndices();
            CalculateNormals();
            CopyToBuffers();
        }

        public float getHeight(Vector2 pos)
        {
            if(pos.X<=0.0f || pos.X>=_terrainWidth || pos.Y <= -_terrainHeight || pos.Y >= 0.0f)
            {
                return 35.0f;
            }
            return _vertices[(int)pos.X + (int)-pos.Y * _terrainWidth].Position.Y;
        }

        public void Draw()
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _device.Indices = _myIndexBuffer;
                _device.SetVertexBuffer(_myVertexBuffer);
                _device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indices.Length / 3);
            }
        }


    }
}
