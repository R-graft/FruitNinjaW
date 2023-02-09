using UnityEngine;

namespace winterStage
{
    public class MoveBlock 
    {
        private Vector3 _direction;

        private const float GravityStep = 0.08f;

        public void SetStartDirection(Vector2 direction)
        {
            if (direction.y <= 2)
            {
                _direction = new Vector2(direction.x, -direction.y/2);
            }

            else
            {
                _direction = direction;
            }
        }
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public Vector3 GetDirection()
        {
            return _direction;
        }

        public void ParabolaMove(Transform transform)
        {
            transform.position += _direction * Time.deltaTime;

            _direction = new Vector2(_direction.x, _direction.y -= GravityStep);
        }

        public void MoveToTarget(Vector3 target, Transform transform)
        {
            transform.localPosition += target * Time.deltaTime;
        }
    }
}
