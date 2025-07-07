using QF;
using System;
using UnityEngine.InputSystem;
using static CardInputAction;

namespace LC
{
    public class InputMgr : Singleton<InputMgr>, IBattleActions
    {
        public Action onSpaceAction;
        public Action onQuitAction;
        private CardInputAction m_InputAction;
        public InputMgr()
        {
            if(m_InputAction == null)
            {
                m_InputAction = new CardInputAction();
                m_InputAction.Battle.SetCallbacks(this);
                m_InputAction.Enable();
            }
        }

        public void OnDisCard(InputAction.CallbackContext context)
        {
            if (onQuitAction != null && context.performed)
                onQuitAction.Invoke();
        }

        public void OnTest(InputAction.CallbackContext context)
        {
            if (onSpaceAction != null && context.performed)
                onSpaceAction.Invoke();
        }
    }
}
