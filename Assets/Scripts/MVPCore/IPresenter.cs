using System;
using Zenject;

namespace MVP
{
    public interface IPresenter : IGuid, IDisposable, IInitializable
    {
    }
}