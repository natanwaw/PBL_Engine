using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class PlayerSkills : GameObjectComponent
    {
        #region var
        private GameObject HitPoint;
        private Transform transform;
        private Vector3 startingPosition;
        private Vector3 targetPosition;
        private float timerSkill1;

        private bool skill1 = false;
        #endregion

        public override void Start()
        {
            HitPoint = GameObject.Find("HitPoint");
        }
        public override void Update(GameTime gameTime)
        {
            timerSkill1 += Time.DeltaTime;
            if(Input.IsKeyDown(3))
            {
                if(timerSkill1>=2)
                {
                    timerSkill1 = 0;
                    if (HitPoint.GetComponent<TerrainRaycast>().HitPoint.HasValue)
                        Dodge();
                }
            }

            if (skill1)
            {
                if (timerSkill1 >= 0.3f)
                    skill1 = false;

                transform.position += Transform.PositionToMoveTowards(startingPosition, targetPosition, 0.2f) * 500 * Time.DeltaTime;
            }
        }
        private void Dodge()
        {
            startingPosition = transform.AbsoluteTransform.Translation;
            targetPosition = HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value;
            skill1 = true;
            //transform.position += Transform.PositionToMoveTowards(startingPosition, pointTowards, 0.2f) * speed * Time.DeltaTime;
        }
    }
}
