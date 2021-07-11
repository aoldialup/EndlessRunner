
using System;
using System.Windows.Forms;

namespace WalmartEngine
{
    class Canvas : Form
    {
        public event EventHandler<EventArgs> OnCloseEvent;

        public Canvas()
        {
            DoubleBuffered = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            OnCloseEvent?.Invoke(this, e);
            base.OnClosed(e);
        }
    }
}
