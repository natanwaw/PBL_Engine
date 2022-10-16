using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Silnik
{
    public class TextBox
    {
        public SpriteFont TextFont;
        public string Text { get; set; }

        public bool Active { get; private set; }

        // X and Y coordinates
        public Vector2 Position { get; set; }

        // Width and Height
        public Vector2 Size { get; set; }

        public Color FillColor { get; set; }

        public Color BorderColor { get; set; }

        public Color TextColor { get; set; }

        public int BorderWidth { get; set; }

        private readonly Texture2D _fillTexture;

        private readonly Texture2D _borderTexture;

        private List<string> _pages;

        private const float TextBoxMargin = 24f;

        private Vector2 _characterSize;

        public Vector2 CenterScreen
            => new Vector2(InitContent.Graphics.GraphicsDevice.Viewport.Width / 2f, InitContent.Graphics.GraphicsDevice.Viewport.Height / 2f);

        private int MaxCharsPerLine;

        private int MaxLines;

        public int _currentPage;

        private int _interval;

        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };


        private Vector2 TextPosition => new Vector2(Position.X + TextBoxMargin / 2, Position.Y + TextBoxMargin / 2);


        private Stopwatch _stopwatch;


        public TextBox()
        {
            BorderWidth = 2;
            TextColor = Color.White;

            FillColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

            _fillTexture = new Texture2D(InitContent.Graphics.GraphicsDevice, 1, 1);
            _fillTexture.SetData(new[] { FillColor });

            _borderTexture = new Texture2D(InitContent.Graphics.GraphicsDevice, 1, 1);
            _borderTexture.SetData(new[] { BorderColor });

            _pages = new List<string>();
            _currentPage = 0;

            var sizeX = 500;
            var sizeY = 50000;

            Size = new Vector2(sizeX, sizeY);

            var posX = 30;
            var posY = 100;

            Position = new Vector2(posX, posY);
        }


        public void Initialize(string text = null)
        {

            TextFont = InitContent.Content.Load<SpriteFont>("fonts/text");

            Vector2 _characterSize = TextFont.MeasureString(new StringBuilder("W", 1));

            MaxCharsPerLine = (int)Math.Floor((Size.X - TextBoxMargin) / _characterSize.X);

            MaxLines = (int)Math.Floor((Size.Y - TextBoxMargin) / _characterSize.Y) - 1;





            Text = text ?? Text;

            _currentPage = 0;

            Show();
        }



        public void Show()
        {
            Active = true;

            _stopwatch = new Stopwatch();

            _stopwatch.Start();

            _pages = WordWrap(Text);
        }


        public void Hide()
        {
            Active = false;

            _stopwatch.Stop();

            //_stopwatch = null;
        }

        //if ((Input.GetKey.IsKeyDown(Keys.Enter) && Program.Game.PreviousKeyState.IsKeyUp(Keys.Enter)) || (Program.Game.GamePadState.Buttons.A == ButtonState.Pressed))

        public void Update()
        {
            /*if (Active)
            {
                if (Input.GetKey.IsKeyDown(Keys.Enter))
                {
                    if (_currentPage >= _pages.Count - 1)
                    {
                        Hide();
                    }
                    else
                    {
                        _currentPage++;
                        _stopwatch.Restart();
                    }
                }

                //if ((Input.GetKey.IsKeyDown(Keys.Enter) && Program.Game.PreviousKeyState.IsKeyUp(Keys.Enter)) || (Program.Game.GamePadState.Buttons.A == ButtonState.Pressed))

                if (Input.GetKey.IsKeyDown(Keys.X))
                {
                    Hide();
                }
            }*/
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {

                foreach (var side in BorderRectangles)
                {

                    //spriteBatch.Draw(_borderTexture, side, Color.White);

                }

                //spriteBatch.Draw(_fillTexture, TextRectangle, Color.Black);


                spriteBatch.DrawString(TextFont, _pages[_currentPage], TextPosition, TextColor);

                if (BlinkIndicator() || _currentPage == _pages.Count - 1)
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4,
                        TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));

                    //spriteBatch.DrawString(TextFont, ">", indicatorPosition, Color.White);
                }
            }
        }

        private bool BlinkIndicator()
        {
            _interval = (int)Math.Floor((double)(_stopwatch.ElapsedMilliseconds % 1000));

            return _interval < 500;
        }


        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();

            var capacity = MaxCharsPerLine * MaxLines > text.Length ? text.Length : MaxCharsPerLine * MaxLines;
            //Console.WriteLine(capacity);

            var result = new StringBuilder(capacity);
            var resultLines = 0;

            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;

                currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine)
                    {
                        result.AppendLine(currentLine.ToString());

                        currentLine.Clear();

                        resultLines++;
                    }

                    currentLine.Append(currentWord);

                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
                    }
                }
            }

            return pages;
        }

    }
}
