using App.Presentation.ViewModel;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace App.Presentation.Elements
{
    public class Hand : MonoBehaviour, IDragHandler
    {
        [field: SerializeField] private Image _handImage;

        private AlarmSetViewModel.Hand _hand;
        private Action<AlarmSetViewModel.Hand, Vector2> _onDrag;

        public void Initialize(AlarmSetViewModel.Hand hand, 
            Action<AlarmSetViewModel.Hand, Vector2> onDrag)
        {
            _hand = hand;
            _onDrag = onDrag;
        }

        public void SetAngle(float angle)
        {
            _handImage.transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }    

        public void OnDrag(PointerEventData eventData)
        {
            _onDrag?.Invoke(_hand, _handImage.transform.position);
        }
    }
}