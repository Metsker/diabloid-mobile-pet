﻿using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire1";
        public abstract Vector2 axis { get; }

        public bool IsAttackButtonUp() => SimpleInput.GetButtonUp(Button);

        protected static Vector2 GetSimpleInputAxis() =>
            new (SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}