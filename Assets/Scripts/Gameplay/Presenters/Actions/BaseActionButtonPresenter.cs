using System;
using Gameplay.Models;
using MVP;
using UnityEngine;

namespace Gameplay.Presenters
{
    public abstract class BaseActionButtonPresenter<TActionEnum> : ButtonPresenter
        where TActionEnum : Enum
    {
        [Serializable]
        public class Data : BasePresenterViewSharedData
        {
            [SerializeField] private TActionEnum _action;

            public TActionEnum Action => _action;
        }

        private readonly IActionsHandler<TActionEnum> _actionsHandler;

        protected BaseActionButtonPresenter(IActionsHandler<TActionEnum> actionsHandler)
        {
            _actionsHandler = actionsHandler;
        }

        protected BaseActionButtonPresenter() { }

        public override void Clicked()
        {
            var data = GetSharedData<Data>();

            _actionsHandler.Launch(data.Action);
        }
    }
}