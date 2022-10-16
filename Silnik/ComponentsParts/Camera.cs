using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Camera
    {
        private GraphicsDevice graphicsDevice = null;
        private GameWindow gameWindow = null;

        private MouseState mouseState = default(MouseState);
        private KeyboardState keyboardState = default(KeyboardState);

        public float MovementUnitsPerSecond { get; set; } = 30f;
        public float RotationRadiansPerSecond { get; set; } = 6f;

        public float fieldOfViewDegrees = 45f;
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 500;
        public bool reflectedCamera = false;
        public static bool ultraShadow = false;

        private Vector2 centerOfScreen;
        public BoundingFrustum Frustum { get; private set; }

        float slowing = 5; // jak duzy smoothing

        public Camera(GraphicsDevice _graphicsDevice, GameWindow _gameWindow)
        {
            graphicsDevice = _graphicsDevice;
            gameWindow = _gameWindow;
            ReCreateWorldAndView();
            ReCreateThePerspectiveProjectionMatrix(_graphicsDevice, fieldOfViewDegrees);
            centerOfScreen = new Vector2((float)graphicsDevice.Viewport.Width / 2, (float)graphicsDevice.Viewport.Height / 2);
        }

        private Vector3 up = Vector3.Up;
        private Matrix camerasWorld = Matrix.Identity;
        private Matrix viewMatrix = Matrix.Identity;
        private Matrix projectionMatrix = Matrix.Identity;
        public bool isometric = true;
        private bool isometricStart = true;
        private bool mustShakeBack = false;
        private int shakeTime = 0;
        private int shakeX = 0;
        private int shakeY = 0;
        private int shakeZ = 0;
        private bool lookUp = false;
        private bool lookDown = false;
        private bool lookLeft = false;
        private bool lookRight = false;
        public float lookX = 0;
        public float lookZ = 0;
        public float cameraDistance = 35;
        private float swing = 4; // wychylenie
        private bool czyPrzesowac = true; // mozna wylaczyc zeby kamera tak nie latala
        public GameObject hero = null;
        public float targetX;
        public float targetY;
        public float targetZ;

        private bool canChangeCamera = false;
        private bool canShakeCamera = false;

        public Vector3 Position
        {
            set
            {
                camerasWorld.Translation = value;
                ReCreateWorldAndView();
            }
            get
            {
                return camerasWorld.Translation;
            }
        }

        public Vector3 Forward
        {
            set
            {
                camerasWorld = Matrix.CreateWorld(camerasWorld.Translation, value, up);
                ReCreateWorldAndView();
            }
            get
            {
                return camerasWorld.Forward;
            }
        }
        public Vector3 Up
        {
            set
            {
                up = value;
                camerasWorld = Matrix.CreateWorld(camerasWorld.Translation, camerasWorld.Forward, value);
                ReCreateWorldAndView();
            }
            get
            {
                return up;
            }
        }
        public Vector3 LookAtDirection
        {
            set
            {
                camerasWorld = Matrix.CreateWorld(camerasWorld.Translation, value, up);
                ReCreateWorldAndView();
            }
            get
            {
                return camerasWorld.Forward;
            }
        }
        public Matrix world
        {
            set
            {
                camerasWorld = value;
                viewMatrix = Matrix.CreateLookAt(camerasWorld.Translation, camerasWorld.Forward + camerasWorld.Translation, camerasWorld.Up);
            }
            get
            {
                return camerasWorld;
            }
        }
        public Matrix View
        {
            get
            {
                return viewMatrix;
            }
        }
        public Matrix Projection
        {
            get
            {
                return projectionMatrix;
            }
        }
        private void generateFrustum()
        {
            Matrix viewProjection;
            if (ultraShadow)
            {
                viewProjection = View * Matrix.CreatePerspectiveFieldOfView((fieldOfViewDegrees + 15) * (float)((3.14159265358f) / 180f), (float)InitContent.Graphics.GraphicsDevice.Viewport.Width / (float)InitContent.Graphics.GraphicsDevice.Viewport.Height, 0.1f, farClipPlane);
            }
            else
            {
                viewProjection = View * Projection;
            }
            Frustum = new BoundingFrustum(viewProjection);
        }
        private void ReCreateWorldAndView()
        {
            camerasWorld = Matrix.CreateWorld(camerasWorld.Translation, camerasWorld.Forward, up);
            viewMatrix = Matrix.CreateLookAt(camerasWorld.Translation, camerasWorld.Forward + camerasWorld.Translation, camerasWorld.Up);
            generateFrustum();
        }
        public void ReCreateThePerspectiveProjectionMatrix(GraphicsDevice gd, float fovInDegrees)
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovInDegrees * (float)((3.14159265358f) / 180f), (float)gd.Viewport.Width / (float)gd.Viewport.Height, 0.1f, farClipPlane);
            generateFrustum();
        }
        public void ReCreateThePerspectiveProjectionMatrix(float fovInDegrees, float nearPlane, float farPlane)
        {
            fieldOfViewDegrees = MathHelper.ToRadians(fovInDegrees);
            nearClipPlane = nearPlane;
            farClipPlane = farPlane;
            float aspectRatio = (float)graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fieldOfViewDegrees, aspectRatio, nearClipPlane, farClipPlane);
            generateFrustum();
        }
        public void Update(GameTime gameTime)
        {

            if (hero == null)
            {
                foreach (GameObject GO in World.AllGameObjects)
                {
                    if (GO.tag == "hero")
                    {
                        hero = GO;
                    }
                }
            }

            if (reflectedCamera == false)
            {
                if (isometric)
                    FpsUiControlsLayoutIsometric(gameTime);
                else
                    FpsUiControlsLayout(gameTime);
            }

        }
        private void FpsUiControlsLayoutIsometric(GameTime gameTime)
        {
            smoothingMove(targetX, targetY, targetZ);
            MouseState state = Mouse.GetState(gameWindow);
            KeyboardState kstate = Keyboard.GetState();
            if (hero != null)
            {
                /*
                if (kstate.IsKeyDown(Keys.W))
                {
                    MoveForwardIsometric(gameTime);
                    lookUp = true;
                }
                if (kstate.IsKeyDown(Keys.S) == true)
                {
                    MoveBackwardIsometric(gameTime);
                    lookDown = true;
                }
                if (kstate.IsKeyDown(Keys.A) == true)
                {
                    MoveLeftIsometric(gameTime);
                    lookLeft = true;
                }
                if (kstate.IsKeyDown(Keys.D) == true)
                {
                    MoveRightIsometric(gameTime);
                    lookRight = true;
                }
                */
                targetX = hero.GetComponent<Transform>().position.X - cameraDistance + lookX;
                targetY = hero.GetComponent<Transform>().position.Y + 40;
                targetZ = hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ;
            }

            if (kstate.IsKeyDown(Keys.C) == true && canChangeCamera == true)
            {
                isometric = false;
            }
            else if (kstate.IsKeyDown(Keys.V) == true && canShakeCamera == true)
            {
                shakeTime = 100;
            }

            if (kstate.IsKeyUp(Keys.W))
            {
                lookUp = false;
            }
            if (kstate.IsKeyUp(Keys.S) == true)
            {
                lookDown = false;
            }
            if (kstate.IsKeyUp(Keys.A) == true)
            {
                lookLeft = false;
            }
            if (kstate.IsKeyUp(Keys.D) == true)
            {
                lookRight = false;
            }

            //Console.WriteLine("X = {0}, Z = {1}", heroX, heroZ);


            //mouse


            if (isometricStart)
            {
                //Position = new Vector3(hero.GetComponent<Transform>().position.X - cameraDistance, Position.Y, hero.GetComponent<Transform>().position.Z + cameraDistance);
                LookAtDirection = new Vector3(1, -1, -1);
                isometricStart = false;
            }
            mouseState = state;
            keyboardState = kstate;

            if (shakeTime > 0)
            {
                shakeTime--;
                shake();
                if (shakeTime == 0) mustShakeBack = true;
            }

            if (mustShakeBack) shakeBack();

            // Przesowanie kamery nieco obok gracza podczas poruszania
            if (czyPrzesowac)
            {
                if (lookUp && lookLeft)
                {
                    if (lookZ > swing) lookZ-=Time.DeltaTime*60f;
                    if (lookX > 0) lookX-= Time.DeltaTime * 60f;
                    else if (lookX < 0) lookX+= Time.DeltaTime * 60f;
                }
                else if (lookUp && lookRight)
                {
                    if (lookX < swing) lookX+= Time.DeltaTime * 60f;
                    if (lookZ > 0) lookZ-= Time.DeltaTime * 60f;
                    else if (lookZ < 0) lookZ+= Time.DeltaTime * 60f;
                }
                else if (lookDown && lookLeft)
                {
                    if (lookX > -swing) lookX-= Time.DeltaTime * 60f;
                    if (lookZ > 0) lookZ-= Time.DeltaTime * 60f;
                    else if (lookZ < 0) lookZ+= Time.DeltaTime * 60f;
                }
                else if (lookDown && lookRight)
                {
                    if (lookZ < swing) lookZ+= Time.DeltaTime * 60f;
                    if (lookX > 0) lookX-= Time.DeltaTime * 60f;
                    else if (lookX < 0) lookX+= Time.DeltaTime * 60f;
                }

                else if (lookUp)
                {
                    if (lookX < swing) lookX+= Time.DeltaTime * 60f;
                    if (lookZ > -swing) lookZ-= Time.DeltaTime * 60f;
                }
                else if (lookDown)
                {
                    if (lookX > -swing) lookX-= Time.DeltaTime * 60f;
                    if (lookZ < swing) lookZ+= Time.DeltaTime * 60f;
                }
                else if (lookLeft)
                {
                    if (lookX > -swing) lookX-= Time.DeltaTime * 60f;
                    if (lookZ > -swing) lookZ-= Time.DeltaTime * 60f;
                }
                else if (lookRight)
                {
                    if (lookX < swing) lookX+= Time.DeltaTime * 60f;
                    if (lookZ < swing) lookZ+= Time.DeltaTime * 60f;
                }
            }


        }
        private void shake()
        {
            int tmpX = shakeTime % 5 - 2;
            int tmpY = shakeTime % 10 - 5;
            int tmpZ = shakeTime % 4 - 2;
            Position = new Vector3(Position.X + tmpX, Position.Y + tmpY, Position.Z + tmpZ);

            shakeX += tmpX;
            shakeY += tmpY;
            shakeZ += tmpZ;
        }
        private void shakeBack()
        {
            int tmpX = 0;
            if (shakeX > 0)
            {
                tmpX = -1;
                shakeX--;
            }
            else if (shakeX < 0)
            {
                tmpX = 1;
                shakeX++;
            }
            int tmpY = 0;
            if (shakeY > 0)
            {
                tmpY = -1;
                shakeY--;
            }
            else if (shakeY < 0)
            {
                tmpY = 1;
                shakeY++;
            }
            int tmpZ = 0;
            if (shakeZ > 0)
            {
                tmpZ = -1;
                shakeZ--;
            }
            else if (shakeZ < 0)
            {
                tmpZ = 1;
                shakeZ++;
            }

            Position = new Vector3(Position.X + tmpX, Position.Y + tmpY, Position.Z + tmpZ);

            if (shakeX == 0 && shakeY == 0 && shakeZ == 0) mustShakeBack = false;
        }
        Vector2 lastMousePosition;
        private void FpsUiControlsLayout(GameTime gameTime)
        {
            MouseState state = Mouse.GetState(gameWindow);
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.W))
            {
                MoveForward(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.S) == true)
            {
                MoveBackward(gameTime);
            }
            if (kstate.IsKeyDown(Keys.A) == true)
            {
                MoveLeft(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.D) == true)
            {
                MoveRight(gameTime);
            }

            else if (kstate.IsKeyDown(Keys.C) == true && canChangeCamera == true)
            {
                Position = new Vector3(Position.X, 70, Position.Z);
                isometric = true;
                isometricStart = true;
            }
            //mouse
            Vector2 diff = state.Position.ToVector2() - lastMousePosition;
            lastMousePosition = state.Position.ToVector2();

            if (state.Position.ToVector2().X >= graphicsDevice.Viewport.Width - 5 || state.Position.ToVector2().X <= 5 || state.Position.ToVector2().Y >= graphicsDevice.Viewport.Height - 5 || state.Position.ToVector2().Y <= 5)
            {
                Mouse.SetPosition((int)centerOfScreen.X, (int)centerOfScreen.Y);
                lastMousePosition = centerOfScreen;
            }
            if (diff.X != 0f)
            {
                RotateLeftOrRight(gameTime, diff.X);
            }
            if (diff.Y != 0f)
            {
                RotateUpOrDown(gameTime, diff.Y);
            }
            mouseState = state;
            keyboardState = kstate;
        }

        public void MoveForwardIsometric(GameTime gameTime)
        {
            //Position = new Vector3(Position.X + 1, Position.Y, Position.Z - 1);
            //Position = new Vector3(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            //smoothingMove(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            targetX = hero.GetComponent<Transform>().position.X - cameraDistance + lookX;
            targetY = hero.GetComponent<Transform>().position.Y + 40;
            targetZ = hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ;
            //Position = new Vector3(targetX + tmpX, targetY + tmpY, targetZ + tmpZ);
        }
        public void MoveBackwardIsometric(GameTime gameTime)
        {
            //Position = new Vector3(Position.X - 1, Position.Y, Position.Z + 1);
            //Position = new Vector3(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            //smoothingMove(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            targetX = hero.GetComponent<Transform>().position.X - cameraDistance + lookX;
            targetY = hero.GetComponent<Transform>().position.Y + 40;
            targetZ = hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ;
            //Position = new Vector3(targetX + tmpX, targetY + tmpY, targetZ + tmpZ);
        }
        public void MoveLeftIsometric(GameTime gameTime)
        {
            // Position = new Vector3(Position.X - 1, Position.Y, Position.Z - 1);
            //Position = new Vector3(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            //smoothingMove(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            targetX = hero.GetComponent<Transform>().position.X - cameraDistance + lookX;
            targetY = hero.GetComponent<Transform>().position.Y + 40;
            targetZ = hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ;
            //Position = new Vector3(targetX + tmpX, targetY + tmpY, targetZ + tmpZ);
        }
        public void MoveRightIsometric(GameTime gameTime)
        {
            //Position = new Vector3(Position.X + 1, Position.Y, Position.Z + 1);
            //Position = new Vector3(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            //smoothingMove(hero.GetComponent<Transform>().position.X - cameraDistance + lookX, hero.GetComponent<Transform>().position.Y + 40, hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ);
            targetX = hero.GetComponent<Transform>().position.X - cameraDistance + lookX;
            targetY = hero.GetComponent<Transform>().position.Y + 40;
            targetZ = hero.GetComponent<Transform>().position.Z + cameraDistance + lookZ;
            //Position = new Vector3(targetX + tmpX, targetY + tmpY, targetZ + tmpZ);
        }

        public void smoothingMove(float targetX, float targetY, float targetZ)
        {
            //Position = new Vector3(targetX, targetY, targetZ);
            float tmpX;
            float tmpY;
            float tmpZ;
            if (targetX > Position.X)
                tmpX = Position.X + distance1d(targetX, Position.X) / slowing;
            else
                tmpX = Position.X - distance1d(targetX, Position.X) / slowing;
            if (targetY > Position.Y)
                tmpY = Position.Y + distance1d(targetY, Position.Y) / slowing;
            else
                tmpY = Position.Y - distance1d(targetY, Position.Y) / slowing;
            if (targetZ > Position.Z)
                tmpZ = Position.Z + distance1d(targetZ, Position.Z) / slowing;
            else
                tmpZ = Position.Z - distance1d(targetZ, Position.Z) / slowing;
            Position = new Vector3(tmpX, tmpY, tmpZ);


        }

        public void MoveForward(GameTime gameTime)
        {
            Position += (camerasWorld.Forward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveBackward(GameTime gameTime)
        {
            Position += (camerasWorld.Backward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void MoveLeft(GameTime gameTime)
        {
            Position += (camerasWorld.Left * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void MoveRight(GameTime gameTime)
        {
            Position += (camerasWorld.Right * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void RotateLeftOrRight(GameTime gameTime, float amount)
        {
            var radians = amount * -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix matrix = Matrix.CreateFromAxisAngle(camerasWorld.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public void RotateUpOrDown(GameTime gameTime, float amount)
        {
            var radians = amount * -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix matrix = Matrix.CreateFromAxisAngle(camerasWorld.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }
        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
        public float distance1d(float ax, float bx)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx));
            return (float)dis;
        }

    }
}
