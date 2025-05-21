using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Common
{
    public class AutoResolver : MonoBehaviour
    {
        private static IObjectResolver _objectResolver;

        private LifetimeScope _lifetimeScope;

        [Inject]
        private void SetScope(LifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _objectResolver = _lifetimeScope.Container;
        }

        public static T Resolve<T>()
        {
            return _objectResolver.Resolve<T>();
        }
    }
}