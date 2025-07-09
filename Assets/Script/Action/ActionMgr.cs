using QF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LC
{
    /// <summary>
    /// Action管理器，控制演出
    /// </summary>
    public class ActionMgr : AutoMonoSingleton<ActionMgr>
    {
        private Dictionary<Type, Func<GameAction, IEnumerator>> m_PerformersDic = new Dictionary<Type, Func<GameAction, IEnumerator>>();
        public bool IsPerforming { get; private set; }

        /// <summary>
        /// 触发一个GameAction
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        public void Perform(GameAction action, Action callback = null)
        {
            if (!IsPerforming) return;
            IsPerforming = true;
            StartCoroutine(Execute(action, () =>
            {
                IsPerforming = false;
                callback?.Invoke();
            }));
        }

        public void RegisterPerformer<T>(Func<T, IEnumerator> func) where T : GameAction
        {
            //使用参数GameAction的包裹参数为T的func
            IEnumerator wrappedFunc(GameAction action) => func((T)action);

            if(m_PerformersDic.ContainsKey(typeof(T)))
                m_PerformersDic[typeof(T)] = wrappedFunc;
            else
                m_PerformersDic.Add(typeof(T), wrappedFunc);
        }

        public void RemovePerformer<T>() where T : GameAction
        {
            if(m_PerformersDic.ContainsKey(typeof(T)))
                m_PerformersDic.Remove(typeof(T));
        }

        /// <summary>
        /// 执行一个GameAction的具体逻辑
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onFinished"></param>
        /// <returns></returns>
        private IEnumerator Execute(GameAction action, Action onFinished = null)
        {
            foreach(var act in action.PreActions)
                yield return Execute(act);

            if(m_PerformersDic.ContainsKey(action.GetType()))
                yield return m_PerformersDic[action.GetType()](action);

            foreach(var act in action.CurActions)
                yield return Execute(act);

            foreach (var act in action.PostActions)
                yield return Execute(act);

            onFinished?.Invoke();
        }
    }
}
