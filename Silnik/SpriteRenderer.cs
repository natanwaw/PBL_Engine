using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class SpriteRenderer : GameObjectComponent
    {
        Sprite sprite;
        Transform transform;
        //Layers only 0-4
        public int layer = 0;
        float angle;
        
        public override void Init()
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            //sprite.Update(gameTime);

        }
        public void Set(Sprite Sprite)
        {
            sprite = Sprite;
            transform = gameObject.GetComponent<Transform>();
            angle = sprite.angle * (float)Math.PI / 180;
            sprite.origin = new Vector2(0, 0);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(sprite.texture, new Vector2(transform.position.X, transform.position.Y),sprite.rect, Color.White, 0f, Vector2.Zero, 1, flip, 0);
            //spriteBatch.Draw(sprite.texture, new Vector2(transform.position.X, transform.position.Y), Color.White);
            //spriteBatch.Draw(sprite.texture, sprite.rect, sprite.color);
            //spriteBatch.Draw(sprite.texture, sprite.rect, sprite.sourceRectangle, sprite.color);
            
            //color = zabarwienie bialego, angle to ilosc stopni jakie obraca sie sprite, origin to punkt zaczepienia - domyslnie (0,0) - sluzy do 
            //obrotu i pozycjonowania obrazka wzgledem tego punktu w sprite.rect - np srodek sprite'a to Vector2(sprite.rect.Width/2, sprite.rect.Heigth/2),
            //spriteeffect umozliwia odbice wzgledem osi X albo Y(korzystajcie z metod FlipX(), FlipY, FlipNone(). layer to wiadomo
            spriteBatch.Draw(sprite.texture, sprite.rect, null, sprite.color, angle, sprite.origin, sprite.spriteEffects, sprite.layer);

        }
        public void Rotate(float Angle)
        {
            angle = Angle * (float)Math.PI / 180;
        }
        public void ChangeSize(Rectangle rect)
        {
            sprite.rect = rect;
        }
        public void ChangeColor(Color color)
        {
            sprite.color = color;
        }
        public void ChangeOrigin(Vector2 vector)
        {
            sprite.origin = vector;
        }
        public void FlipX()
        {
            sprite.spriteEffects = SpriteEffects.FlipHorizontally;
        }
        public void FlipY()
        {
            sprite.spriteEffects = SpriteEffects.FlipVertically;
        }
        public void FlipNone()
        {
            sprite.spriteEffects = SpriteEffects.None;
        }
        public void ChangeLayer(int layer)
        {
            sprite.layer = layer;
        }
        public void SetAll(Rectangle rectangle, Color color, float Angle, Vector2 vector, SpriteEffects spriteEffects, int layer)
        {
            sprite.rect = rectangle;
            sprite.color = color;
            angle = Angle * (float)Math.PI / 180;
            sprite.origin = vector;
            sprite.spriteEffects = spriteEffects;
            sprite.layer = layer;
        }
    }
}
