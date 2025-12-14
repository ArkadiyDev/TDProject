using UnityEngine;

namespace UI
{
    public abstract class UIView : MonoBehaviour, IUiView
    {
        public virtual void Open() =>
            gameObject.SetActive(true);

        public virtual void Close() =>
            gameObject.SetActive(false);
    }
}