using Gameplay.Models;
using Gameplay.Presenters;
using MVP;
using UnityEngine;

namespace Gameplay.Views
{
    public class BypassCarsObstacleView : View<BypassCarsObstaclePresenter>, IObstacleHeight
    {
        [SerializeField] private RectTransform _rectTransform;

        public float Height => _rectTransform.rect.height;
    }
}