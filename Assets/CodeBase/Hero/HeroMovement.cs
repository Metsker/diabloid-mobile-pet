using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMovement : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private float movementSpeed;

        private CharacterController _characterController;
        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _inputService = AllServices.container.Single<IInputService>();

            _characterController = GetComponent<CharacterController>();
        }

        private void Start() =>
            _camera = Camera.main;

        private void Update()
        {
            var movementVector = Vector3.zero;
            
            if (_inputService.axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.axis);
                movementVector.y = 0;
                movementVector.Normalize();
                transform.forward = movementVector;
            }
            movementVector += Physics.gravity;
            _characterController.Move(movementVector * (movementSpeed * Time.deltaTime));
        }

        public void UpdateProgress(PlayerProgress progress) =>
            progress.worldData.positionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() != progress.worldData.positionOnLevel.level)
                return;
            
            var savedPosition = progress.worldData.positionOnLevel.position;
            if (savedPosition != null)
                Warp(to: savedPosition);
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}