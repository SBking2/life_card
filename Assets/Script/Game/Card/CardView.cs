using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LC
{
    public class CardView : MonoBehaviour
    {
        private void OnMouseDown()
        {
            transform.DOMoveY(0.0f, 0.5f);
        }

        private void OnMouseEnter()
        {
            transform.DOScale(1.2f, 0.2f);
        }

        private void OnMouseExit()
        {
            transform.DOScale(1.0f, 0.2f);
        }
    }
}
