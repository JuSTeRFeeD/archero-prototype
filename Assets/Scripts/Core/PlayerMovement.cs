using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;
        private CharacterController _controller;

        [Space]
        public float movementSpeed = 2f;
        private Vector3 _inputDir;
        
        private const float RotationSpeed = 1000f;

        private Animator _animator;
        private static readonly int IsWalkAnim = Animator.StringToHash("isWalk");
        
        public bool IsMoving() => joystick.Direction != Vector2.zero;
        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(IsWalkAnim, joystick.Direction != Vector2.zero);
            if (joystick.Direction == Vector2.zero) return;
            
            var dt = Time.deltaTime;
            var joystickDirection = joystick.Direction.normalized;
            
            _inputDir = new Vector3(joystickDirection.x, 0, joystickDirection.y); 
            _controller.Move(_inputDir * (movementSpeed * dt));
            
            var toRotation = Quaternion.LookRotation(_inputDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                toRotation, 
                RotationSpeed * dt);
        }
    }
}